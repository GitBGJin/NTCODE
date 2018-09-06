using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Telerik.Web.UI;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.DataAuditing.AuditBaseInfo;
using SmartEP.Service.DataAuditing.AuditInterfaces;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.Utilities.Web.UI;
using SmartEP.WebUI.Controls;
using SmartEP.WebControl.CbxRsm;
using SmartEP.Core.Enums;
using Telerik.Web.UI.Calendar;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System.Threading;
using System.Configuration;

namespace SmartEP.WebUI.Pages.EnvAir.Audit
{
    public partial class MutilPointAuditState : SmartEP.WebUI.Common.BasePage
    {
        #region 属性
        AirAuditMonitoringPointService pointAirService = new AirAuditMonitoringPointService();//点位接口 空气
        WaterAuditMonitoringPointService pointWaterService = new WaterAuditMonitoringPointService();//点位接口 地表水
        AuditMonitoringPointPollutantService pollutantService = new AuditMonitoringPointPollutantService();//因子接口
        AuditDataService auditDataService = new AuditDataService();//审核数据接口
        AuditLogService auditLogService = new AuditLogService();//审核日志接口
        AuditOperatorService operatorService = new AuditOperatorService();//审核操作接口
        static DateTime currentBegin = DateTime.Now;
        static DateTime currentEnd = DateTime.Now;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //if (Request.QueryString["pointType"] != null)
                //    ViewState["pointType"] = Request.QueryString["pointType"];
                //else
                //    ViewState["pointType"] = "0";
                if (Request.QueryString["app"] != null && Request.QueryString["app"].Equals("1"))
                    Session["applicationUID"] = "watrwatr-watr-watr-watr-watrwatrwatr";
                else
                    Session["applicationUID"] = "airaaira-aira-aira-aira-airaairaaira";
                if (Session["UserGuid"] == null)
                    Session["UserGuid"] = "4ce5bed9-78bd-489f-8b3f-a830098759c4";
                ViewState["PointID"] = Request.QueryString["PointID"] != null ? Request.QueryString["PointID"].ToString() : "";
                InitControl();//初始化控件
            }
        }

        #region 初始化控件
        private void InitControl()
        {
            if (Request.QueryString["dtBegin"] == null) Session["FactorCode"] = null;//首次加载界面情况因子session
            RadDatePickerBegin.SelectedDate = Request.QueryString["dtBegin"] != null ? Convert.ToDateTime(Request.QueryString["dtBegin"]) : DateTime.Now.Date.AddDays(-7);
            RadDatePickerEnd.SelectedDate = Request.QueryString["dtEnd"] != null ? Convert.ToDateTime(Request.QueryString["dtEnd"]) : DateTime.Now.Date.AddDays(-1);
            //RadCalendar1.FocusedDate = DateTime.Now.AddMonths(-1);
            PointBind();//初始化点位
            RadCalendar1.FocusedDate = Request.QueryString["dtBegin"] != null ? Convert.ToDateTime(Request.QueryString["dtBegin"]) : DateTime.Now;
            InitRadScheduler(true);//初始化日历控件
            string pointids = GetPointSelectedValue();
            if (!pointids.Equals(""))
            {
                //factorCbxRsm.FactorBind(pointids.Split(';'), ViewState["pointType"].Equals("2") ? 1 : 0);//绑定SiteMap因子
                factorCbxRsm.FactorBind(pointids.Split(';'), 0);//绑定SiteMap因子
                FactorRsmAudit.firstLoad = 2;
            }
            #region 底部统计信息隐藏（环境空气中需要）
            if (!EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(Session["applicationUID"].ToString()))//空气
                RadPaneBottom.Visible = false;
            #endregion

            #region 清空Session
            if (ViewState["PointID"].ToString().Equals(""))
            {
                Session["FactorCode"] = "";
                Session["FactorName"] = "";
                Session["isEdit"] = "";
                Session["PollutantDecimalNum"] = "";
                Session["PollutantUnit"] = "";
            }
            #endregion
        }

        #region 点位绑定
        private void PointBind()
        {
            IQueryable<MonitoringPointEntity> pointList = null;
            if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(Session["applicationUID"]))
                pointList = pointAirService.RetrieveAirByAuditType("0", Session["UserGuid"].ToString()).OrderByDescending(p => p.OrderByNum).Concat(pointAirService.RetrieveAirByAuditType("1", Session["UserGuid"].ToString()).OrderByDescending(p => p.OrderByNum)).Concat(pointAirService.RetrieveAirByAuditType("3", Session["UserGuid"].ToString()).OrderByDescending(p => p.OrderByNum)).Distinct();//.Concat(pointAirService.RetrieveAirByAuditType("2", Session["UserGuid"].ToString()).OrderByDescending(p => p.OrderByNum))
            else if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(Session["applicationUID"]))
                pointList = pointWaterService.RetrieveWaterByAuditType(ViewState["pointType"].ToString(), Session["UserGuid"].ToString()).OrderByDescending(p => p.OrderByNum);
            radPoint.DataSource = pointList;
            radPoint.DataValueField = "PointId";
            radPoint.DataTextField = "MonitoringPointName";
            radPoint.DataBind();
            string Pointids = "";
            foreach (ListItem item in radPoint.Items)
            {

                if (ViewState["PointID"].ToString().Split(';').Contains(item.Value))
                    item.Selected = true;
                if (item.Selected)
                    Pointids += Pointids.Equals("") ? item.Value : ";" + item.Value;
            }
            if (radPoint.Items.Count > 0 && radPoint.SelectedItem == null)
            {
                //if (ViewState["pointType"].ToString().Equals("0") && Session["applicationUID"].ToString().Equals("watrwatr-watr-watr-watr-watrwatrwatr"))
                //{
                //    for (int i = 0; i < radPoint.Items.Count; i++)
                //    {
                //        if (radPoint.Items[i].Value.Equals("41") || radPoint.Items[i].Value.Equals("43") || radPoint.Items[i].Value.Equals("52") || radPoint.Items[i].Value.Equals("53"))
                //            radPoint.Items[i].Selected = true;
                //    }
                //}
                //else if (ViewState["pointType"].ToString().Equals("1") && Session["applicationUID"].ToString().Equals("watrwatr-watr-watr-watr-watrwatrwatr"))
                //{
                //    for (int i = 0; i < radPoint.Items.Count; i++)
                //    {
                //        radPoint.Items[i].Selected = true;
                //    }
                //}
                //else if (ViewState["pointType"].ToString().Equals("2") && Session["applicationUID"].ToString().Equals("watrwatr-watr-watr-watr-watrwatrwatr"))
                //{
                //    for (int i = 0; i < radPoint.Items.Count; i++)
                //    {
                //        if (radPoint.Items[i].Value.Equals("51"))
                //            radPoint.Items[i].Selected = true;
                //    }
                //}
                //else
                //radPoint.Items[radPoint.Items.Count - 1].Selected = true;
                radPoint.Items[0].Selected = true;
                Pointids = radPoint.SelectedValue;
            }
            ViewState["PointID"] = Pointids;
        }
        #endregion

        #region 日历控件绑定
        private void InitRadScheduler(Boolean IsReset)
        {
            DateTime beginDate, endDate;
            beginDate = RadCalendar1.CalendarView.ViewStartDate;
            endDate = beginDate.AddDays(41);

            if (IsReset)
            {
                this.RadCalendar1.SpecialDays.Clear();
            }
            List<PointPollutantInfo> PointPollutantInfos = pollutantService.RetrieveSiteMapPollutantList(204, "airaaira-aira-aira-aira-airaairaaira", Session["UserGuid"].ToString()).Where(p => p.PGuid != null && p.PGuid.Equals("339B72C4-7295-4D31-B9EB-23342CB3697E")).ToList<PointPollutantInfo>();
            int x = 0;  //计数器，因a05024出现过两次的情况
            foreach (PointPollutantInfo ppi in PointPollutantInfos)
            {
                if (ppi.PID.Equals("a05024"))
                {
                    x++;
                    if (x == 2)
                    {
                        PointPollutantInfos.Remove(ppi);
                        break;
                    }
                }
            }
            string[] factors = new string[PointPollutantInfos.Count];
            for (int i = 0; i < PointPollutantInfos.Count; i++)
            {
                factors[i] = PointPollutantInfos[i].PID;
            }
            if (factors.Length <= 0) return;
            for (DateTime myDateTime = beginDate; myDateTime <= endDate; myDateTime = myDateTime.AddDays(1))  //循环日历控件日期
            {
                string status = "";
                string Pointids = GetPointSelectedValue();
                if (!Pointids.Equals(""))
                {
                    //status = auditLogService.RerieveAuditState(Pointids.Split(';'), myDateTime, myDateTime, Session["applicationUID"].ToString(), ViewState["pointType"].Equals("2") && Session["applicationUID"].Equals("airaaira-aira-aira-aira-airaairaaira") ? "1" : "0");
                    if (Pointids.Contains("204"))
                    {
                        status = auditLogService.RerieveAuditState(Pointids.Split(';'), myDateTime, myDateTime, Session["applicationUID"].ToString(), "1", factors);
                    }
                    else
                    {
                        status = auditLogService.RerieveAuditState(Pointids.Split(';'), myDateTime, myDateTime, Session["applicationUID"].ToString(), "0");
                    }
                }
                #region 填充数据
                RadCalendarDay myDay = new RadCalendarDay();
                myDay.Date = myDateTime;
                myDay.TemplateID = "Adt" + status;
                //myDay.Repeatable = RecurringEvents.DayInMonth;
                //myDay.ToolTip = "fdfd";
                this.RadCalendar1.SpecialDays.Add(myDay);
                #endregion
            }

        }
        #endregion
        #endregion

        #region 事件
        /// <summary>
        /// 日历切换日期
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RadCalendar1_DefaultViewChanged(object sender, Telerik.Web.UI.Calendar.DefaultViewChangedEventArgs e)
        {
            InitRadScheduler(false);//日历绑定
        }

        /// <summary>
        /// 绑定审核数据统计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RadGridAnalyze_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
        }

        /// <summary>
        /// 绑定审核日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridAuditLog_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            string Pointids = GetPointSelectedValue();
            //factorCbxRsm.FactorBind(Pointids.Split(';'), ViewState["pointType"].Equals("2") ? 1 : 0);//绑定SiteMap因子
            factorCbxRsm.FactorBind(Pointids.Split(';'), 0);//绑定SiteMap因子
            factorCbxRsm.GetFactors();//获取sitemap因子，放入Session中保存
            if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(Session["applicationUID"].ToString()))//空气
            {
                //站点单选
                IQueryable<MonitoringPointEntity> mpe = pointAirService.RetrieveAirPointListByPointId(Convert.ToInt32(Pointids.Split(';')[0]), Session["UserGuid"].ToString());
                MonitoringPointEntity mp = mpe.FirstOrDefault();
                if (mp.MonitoringPointExtensionForEQMSAirEntity.SuperOrNot == true)
                {
                    List<string> list = factorCbxRsm.GetFactors().Select(t => t.PName).ToList();
                    gridAuditLog.DataSource = auditLogService.RerieveAirLog(Pointids.Split(';'), Session["applicationUID"].ToString(), RadDatePickerBegin.SelectedDate.Value.Date, RadDatePickerEnd.SelectedDate.Value.Date.AddDays(1).AddHours(-1)).Where(t => list.Contains(t.PollutantName));
                }
                else
                {
                    gridAuditLog.DataSource = auditLogService.RerieveAirLog(Pointids.Split(';'), Session["applicationUID"].ToString(), RadDatePickerBegin.SelectedDate.Value.Date, RadDatePickerEnd.SelectedDate.Value.Date.AddDays(1).AddHours(-1));
                }
            }
            else if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(Session["applicationUID"].ToString()))//地表水
                gridAuditLog.DataSource = auditLogService.RerieveWaterLog(Pointids.Split(';'), Session["applicationUID"].ToString(), RadDatePickerBegin.SelectedDate.Value.Date, RadDatePickerEnd.SelectedDate.Value.Date.AddDays(1).AddHours(-1));

        }


        /// <summary>
        /// 站点切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void radPoint_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["PointID"] = GetPointSelectedValue();
            BindingFactors();//重新绑定因子
        }

        /// <summary>
        /// 重新绑定因子
        /// </summary>
        private void BindingFactors()
        {
            string Pointids = GetPointSelectedValue();
            if (!Pointids.Equals(""))
            {
                //factorCbxRsm.FactorBind(Pointids.Split(';'), ViewState["pointType"].Equals("2") ? 1 : 0, true);//绑定SiteMap因子
                factorCbxRsm.FactorBind(Pointids.Split(';'), 0, true);//绑定SiteMap因子
            }
            else
            {
                Alert("请选择站点！");
                return;
            }
        }

        /// <summary>
        /// 时间段选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RadDatePickerBegin_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            if (RadDatePickerBegin.SelectedDate == null || RadDatePickerEnd.SelectedDate == null)
            {
                Alert("时间不能为空");
                return;
            }
            else if (RadDatePickerBegin.SelectedDate.Value > RadDatePickerEnd.SelectedDate.Value)
            {
                Alert("开始时间必须小于结束时间！");
                return;
            }
            //gridAuditLog.Rebind();
            RadGridAnalyze.Rebind();
        }

        /// <summary>
        /// 表格item绑定处理小数位数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RadGridAnalyze_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                RadGrid myRadGrid = ((RadGrid)sender);
                int DecimalNum = 3;
                if (e.Item is GridDataItem && myRadGrid.MasterTableView.DataSourceCount > 0)
                {
                    GridDataItem item = (GridDataItem)e.Item;
                    foreach (GridColumn col in myRadGrid.MasterTableView.RenderColumns)
                    {
                        string CurrUName = col.UniqueName;
                        if (!CurrUName.Contains("_total") && !CurrUName.Contains("_enable")
                            && !CurrUName.Equals("Row") && !CurrUName.Equals("DataDateTime") && col.Visible == true)
                        {
                            TableCell cell = item[CurrUName];
                            DataRowView row = (DataRowView)item.DataItem;
                            if (CurrUName.Equals("PointName"))
                            {
                                cell.Text = radPoint.Items.FindByValue(row["PointId"].ToString()).Text;
                            }
                            else
                            {
                                if (!cell.Text.Equals("") && !cell.Text.Equals("--"))
                                    cell.Text = DecimalExtension.GetRoundValue(Convert.ToDecimal(cell.Text), DecimalNum).ToString();
                            }


                        }
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 绑定日志信息点位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridAuditLog_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem && gridAuditLog.MasterTableView.DataSourceCount > 0)
                {
                    GridDataItem item = (GridDataItem)e.Item;
                    if (item.DataItem != null)
                    {
                        if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(Session["applicationUID"].ToString()))//空气
                        {
                            AuditAirLogEntity row = (AuditAirLogEntity)item.DataItem;
                            TableCell cell = item["PointName"];
                            cell.Text = radPoint.Items.FindByValue(auditLogService.GetPointID(row.AuditStatusUid.ToString(), Session["applicationUID"].ToString()).ToString()).Text;

                            cell = item["Tstamp"];
                            cell.Text = Convert.ToDateTime(row.Tstamp).AddHours(1).ToString("MM-dd HH:mm");

                        }
                        else
                        {
                            AuditWaterLogEntity row = (AuditWaterLogEntity)item.DataItem;
                            TableCell cell = item["PointName"];
                            cell.Text = radPoint.Items.FindByValue(auditLogService.GetPointID(row.AuditStatusUid.ToString(), Session["applicationUID"].ToString()).ToString()).Text;
                        }
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        ///  除环境空气外，统计信息表格导致重新加载失败,所以讲刷新统计信息表格的Ajax取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RadAjaxManager1_AjaxSettingCreating(object sender, AjaxSettingCreatingEventArgs e)
        {
            if (!EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(Session["applicationUID"].ToString()))//空气
            {
                if (e.Updated.ID.Equals("RadGridAnalyze"))
                {
                    e.Canceled = true;
                }
            }
            else
                e.Canceled = false;
        }

        /// <summary>
        /// 因子切换
        /// </summary>
        protected void factorCbxRsm_SelectedChanged()
        {
            gridAuditLog.Rebind();
        }

        /// <summary>
        /// 进入审核页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void EnterAudit_Click(object sender, EventArgs e)
        {
            try
            {
                MutilPointAuditData.isfirst = true;
                DateTime DTBegin = DateTime.Parse(selectDateTime.Value);
                DateTime DTEnd = DateTime.Parse(selectDateTime.Value);
                factorCbxRsm.GetFactors();//获取sitemap因子，放入Session中保存
                //String myUrl = "MutilPointAuditData.aspx?dtBegin=" + DTBegin + "&dtEnd=" + DTEnd + "&pointType=" + ViewState["pointType"] + "&PointID=" + ViewState["PointID"] + "&Token=" + Request.QueryString["Token"];//AuditData.aspx
                String myUrl = "MutilPointAuditData.aspx?dtBegin=" + DTBegin + "&dtEnd=" + DTEnd + "&PointID=" + ViewState["PointID"] + "&Token=" + Request.QueryString["Token"];//AuditData.aspx
                if (DTBegin.Date <= DateTime.Now.Date && !GetPointSelectedValue().Equals(""))
                    Server.Transfer(myUrl, false);
                else
                    Alert("没有数据！");
            }
            catch
            {
            }
        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Search_Click(object sender, EventArgs e)
        {
            InitRadScheduler(true);
            gridAuditLog.Rebind();
            //RadGridAnalyze.Rebind();
        }


        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void selectAll_Click(object sender, EventArgs e)
        {
            BindingFactors();//重新绑定因子
        }

        /// <summary>
        /// 反选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void inverse_Click(object sender, EventArgs e)
        {
            BindingFactors();//重新绑定因子
        }

        #region 方法
        /// <summary>
        /// 日历日期处理
        /// </summary>
        /// <param name="dayStr"></param>
        /// <returns></returns>
        protected String getDay(String dayStr)
        {
            String[] arrStr = dayStr.Split('_');
            String day = "xx";
            if (arrStr.Length >= 4) day = arrStr[3].ToString();
            return day;
        }

        /// <summary>
        /// 获取选择的站点值（以;隔开）
        /// </summary>
        /// <returns></returns>
        private string GetPointSelectedValue()
        {
            string Pointids = "";
            foreach (ListItem item in radPoint.Items)
            {
                if (item.Selected == true)
                    Pointids += Pointids.Equals("") ? item.Value : ";" + item.Value;
            }
            return Pointids;
        }
        #endregion

        #endregion
    }
}