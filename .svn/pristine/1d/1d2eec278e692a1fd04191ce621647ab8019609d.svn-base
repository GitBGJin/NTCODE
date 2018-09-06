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
using SmartEP.Service.Frame;
using SmartEP.DomainModel;

namespace SmartEP.WebUI.Pages.EnvAir.Audit
{
    public partial class MutilPointAuditStateWX : SmartEP.WebUI.Common.BasePage
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
        //CbxRsmControl myRSM = new CbxRsmControl();
        #endregion

        #region 方法
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //AuditType：空气（0：国控点；1：市控点位；2：超级站点位；3：省控点位；4：区控点位；5：创模点位；6：中意项目;7:路边站;8:对照点;9移动车）
                //           地表水（0：水源地；1：城区河道；2：浮标站；3：人工监测点；）  
                if (Request.QueryString["AuditType"] != null)
                    ViewState["AuditType"] = Request.QueryString["AuditType"];
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
            RadDatePickerBegin.SelectedDate = Request.QueryString["dtBegin"] != null ? Convert.ToDateTime(Request.QueryString["dtBegin"]) : DateTime.Now.Date.AddDays(-7);
            RadDatePickerEnd.SelectedDate = Request.QueryString["dtEnd"] != null ? Convert.ToDateTime(Request.QueryString["dtEnd"]) : DateTime.Now.Date.AddDays(-1);
            //RadCalendar1.FocusedDate = DateTime.Now.AddMonths(-1);
            InitAuditType();//初始化审核类型
            if (AuditType.SelectedItem != null)
            {
                PointBind();//初始化点位
                RadCalendar1.FocusedDate = Request.QueryString["dtBegin"] != null ? Convert.ToDateTime(Request.QueryString["dtBegin"]) : DateTime.Now;
                InitRadScheduler(true, true);//初始化日历控件
                //string pointids = GetPointSelectedValue();
                //if (!pointids.Equals(""))
                //{
                factorCbxRsm.FactorBind(AuditType.SelectedItem.Value);//绑定SiteMap因子
                //FactorRsmAudit.firstLoad = 2;
                //}
                #region 底部统计信息隐藏（环境空气中需要）
                if (!EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(Session["applicationUID"].ToString()))//空气
                    RadPaneBottom.Visible = false;
                #endregion
            }

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
                pointList = pointAirService.RetrieveAirPointListBySiteType(AuditType.SelectedItem.Value, Session["UserGuid"].ToString());
            else if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(Session["applicationUID"]))
                pointList = pointWaterService.RetrieveWaterPointList(AuditType.SelectedItem.Value, Session["UserGuid"].ToString());
            radioPoint.DataSource = pointList;
            radioPoint.DataValueField = "PointId";
            radioPoint.DataTextField = "MonitoringPointName";
            radioPoint.DataBind();
            string Pointids = "";
            foreach (ListItem item in radioPoint.Items)
            {

                if (ViewState["PointID"].ToString().Split(';').Contains(item.Value))
                    item.Selected = true;
                if (item.Selected)
                    Pointids += Pointids.Equals("") ? item.Value : ";" + item.Value;
            }
            if (radioPoint.Items.Count > 0 && radioPoint.SelectedItem == null)
            {
                radioPoint.Items[0].Selected = true;
                Pointids = radioPoint.SelectedValue;
            }
            ViewState["PointID"] = Pointids;
        }
        #endregion

        #region 日历控件绑定
        private void InitRadScheduler(Boolean IsReset, bool IsSetMonth = false)
        {
            DateTime beginDate, endDate;
            if (IsSetMonth && RadDatePickerBegin.SelectedDate != null)
                RadCalendar1.FocusedDate = RadDatePickerBegin.SelectedDate.Value.Date;
            beginDate = RadCalendar1.CalendarView.ViewStartDate;
            endDate = beginDate.AddDays(41);

            if (IsReset)
            {
                this.RadCalendar1.SpecialDays.Clear();
            }

            for (DateTime myDateTime = beginDate; myDateTime <= endDate; myDateTime = myDateTime.AddDays(1))  //循环日历控件日期
            {
                string status = "";
                string Pointids = GetPointSelectedValue();
                if (!Pointids.Equals(""))
                {
                    status = auditLogService.RerieveAuditState(Pointids.Split(';'), myDateTime, myDateTime, Session["applicationUID"].ToString(), ViewState["pointType"].Equals("2") && Session["applicationUID"].Equals("airaaira-aira-aira-aira-airaairaaira") ? "1" : "0");
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

        /// <summary>
        ///  初始化审核类型
        /// </summary>
        private void InitAuditType()
        {
            DictionaryService dicService = new DictionaryService();
            IQueryable<V_CodeMainItemEntity> siteTypeEntites = null;
            List<V_CodeMainItemEntity> list = new List<V_CodeMainItemEntity>();
            if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(Session["applicationUID"]))
            {
                siteTypeEntites = dicService.RetrieveList(SmartEP.Service.Core.Enums.DictionaryType.Water, "地表水站点类型");
                foreach (V_CodeMainItemEntity item in siteTypeEntites)
                {
                    if (pointWaterService.RetrieveWaterPointList(item.ItemGuid, Session["UserGuid"].ToString()).Count() > 0 && !item.ItemGuid.Equals("160e08ec-1d1b-4095-b898-3a3d925ed4e6"))//去除人工监测点
                        list.Add(item);
                }
            }
            else
            {
                siteTypeEntites = dicService.RetrieveList(SmartEP.Service.Core.Enums.DictionaryType.Air, "空气站点类型");
                foreach (V_CodeMainItemEntity item in siteTypeEntites)
                {
                    if (pointAirService.RetrieveAirPointListBySiteType(item.ItemGuid, Session["UserGuid"].ToString()).Count() > 0)
                        list.Add(item);
                }
            }
            AuditType.DataSource = list;
            AuditType.DataTextField = "ItemText";
            AuditType.DataValueField = "ItemGuid";
            AuditType.DataBind();
            if (AuditType.Items.Count > 0)
            {
                if (ViewState["AuditType"] != null)
                    AuditType.Items.FindByValue(ViewState["AuditType"].ToString()).Selected = true;
                else
                    AuditType.Items[0].Selected = true;
            }
            ViewState["AuditType"] = AuditType.SelectedItem.Value;
        }
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
        /// 点击日历控件进入审核数据页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RadCalendar1_SelectionChanged(object sender, Telerik.Web.UI.Calendar.SelectedDatesEventArgs e)
        {

            DateTime DTBegin = RadCalendar1.SelectedDate;
            DateTime DTEnd = RadCalendar1.SelectedDate;
            if (DTBegin == Convert.ToDateTime("0001/1/1 0:00:00") || DTEnd == Convert.ToDateTime("0001/1/1 0:00:00"))
            {
                DTBegin = Convert.ToDateTime(currentBegin);
                DTEnd = Convert.ToDateTime(currentEnd);
            }
            else
            {
                currentBegin = DTBegin;
                currentEnd = DTEnd;
            }
            factorCbxRsm.GetFactors();//获取sitemap因子，放入Session中保存
            String myUrl = "MutilPointAuditDataWX.aspx?dtBegin=" + DTBegin + "&dtEnd=" + DTEnd + "&AuditType=" + ViewState["AuditType"] + "&PointID=" + ViewState["PointID"] + "&Token=" + Request.QueryString["Token"];//AuditData.aspx
            if (DTBegin.Date <= DateTime.Now.Date && !GetPointSelectedValue().Equals(""))
                Response.Redirect(myUrl, true);
            else
                Alert("没有数据！");
        }

        /// <summary>
        /// 绑定审核数据统计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RadGridAnalyze_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            string factorCode = "a21026;a21005;a05024;a21004;a34002;a34004";//常规6因子设置
            //string factorName = "二氧化硫;一氧化碳;臭氧;二氧化氮;PM10;PM2.5";
            string[] pointid = GetPointSelectedValue().Split(';');
            RadGridAnalyze.DataSource = auditDataService.RetrieveAuditStatisticalData(Session["applicationUID"].ToString(), pointid, factorCode.Split(';'), RadDatePickerBegin.SelectedDate.Value.Date, RadDatePickerEnd.SelectedDate.Value.Date.AddDays(1).AddHours(-1));
        }

        /// <summary>
        /// 绑定审核日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridAuditLog_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            string Pointids = GetPointSelectedValue();
            //factorCbxRsm.FactorBind(AuditType.SelectedItem.Value);//绑定SiteMap因子
            List<PointPollutantInfo> facList = factorCbxRsm.GetFactors();//获取sitemap因子，放入Session中保存
            string[] facs = facList.Select(y => y.PID).ToArray();
            if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(Session["applicationUID"].ToString()))//空气
                gridAuditLog.DataSource = auditLogService.RerieveAirLog(Pointids.Split(';'), Session["applicationUID"].ToString(), RadDatePickerBegin.SelectedDate.Value.Date, RadDatePickerEnd.SelectedDate.Value.Date.AddDays(1).AddHours(-1)).Where(x => facs.Contains(x.PollutantCode));
            else if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(Session["applicationUID"].ToString()))//地表水
                gridAuditLog.DataSource = auditLogService.RerieveWaterLog(Pointids.Split(';'), Session["applicationUID"].ToString(), RadDatePickerBegin.SelectedDate.Value.Date, RadDatePickerEnd.SelectedDate.Value.Date.AddDays(1).AddHours(-1)).Where(x => facs.Contains(x.PollutantCode));

        }

        /// <summary>
        /// 删除审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DeleteAudit_Click(object sender, EventArgs e)
        {
            string Pointids = GetPointSelectedValue();
            if (!Pointids.Equals(""))
            {

                bool result = false;
                if (ViewState["AuditType"].Equals("0") && Session["applicationUID"].Equals("airaaira-aira-aira-aira-airaairaaira"))//国控点数据重载
                    result = operatorService.DeleteAuditWX(Pointids.Replace(";", ","), RadDatePickerBegin.SelectedDate.Value.Date, RadDatePickerEnd.SelectedDate.Value.Date.AddDays(1), "", "", "", "重新加载", "重新加载", "");
                else
                    result = operatorService.DeleteAudit(Session["applicationUID"].ToString(), Pointids.Split(';'), RadDatePickerBegin.SelectedDate.Value.Date, RadDatePickerEnd.SelectedDate.Value.Date.AddDays(1).AddHours(-1), "", "", "", "重新加载", "重新加载", "");
                if (result)
                {
                    RadNotification1.Show();
                    InitRadScheduler(true);
                    RadNotification1.Value = "1";//更新通知窗口值
                    //Alert("重新加载审核成功！");
                }
                else Alert("重新加载审核失败！");
            }
            else
            {
                Alert("请选择测点！");
            }
        }

        /// <summary>
        /// 测点切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void radioPoint_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Pointids = GetPointSelectedValue();
            if (!Pointids.Equals(""))
            {
                //InitRadScheduler(true);
                ViewState["PointID"] = GetPointSelectedValue();
                //factorCbxRsm.FactorBind(AuditType.SelectedItem.Value);//绑定SiteMap因子
                //gridAuditLog.Rebind();
                //RadGridAnalyze.Rebind();
            }
            else
            {
                Alert("请选择测点！");
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
            gridAuditLog.Rebind();
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
                                cell.Text = radioPoint.Items.FindByValue(row["PointId"].ToString()).Text;
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
                            cell.Text = radioPoint.Items.FindByValue(auditLogService.GetPointID(row.AuditStatusUid.ToString(), Session["applicationUID"].ToString()).ToString()).Text;
                        }
                        else
                        {
                            AuditWaterLogEntity row = (AuditWaterLogEntity)item.DataItem;
                            TableCell cell = item["PointName"];
                            cell.Text = radioPoint.Items.FindByValue(auditLogService.GetPointID(row.AuditStatusUid.ToString(), Session["applicationUID"].ToString()).ToString()).Text;
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
        /// 审核类型切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AuditType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["AuditType"] = AuditType.SelectedItem.Value;
            PointBind();
            factorCbxRsm.FactorBind(AuditType.SelectedItem.Value);//绑定SiteMap因子
            //string Pointids = GetPointSelectedValue();
            //if (!Pointids.Equals(""))
            //{
            //    ViewState["PointID"] = GetPointSelectedValue();

            //}
            //else
            //{
            //    Alert("此类型未配置站点！");
            //    return;
            //}
        }


        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void selectAll_Click(object sender, EventArgs e)
        {
            foreach (ListItem item in radioPoint.Items)
            {
                item.Selected = true;
            }
            string Pointids = GetPointSelectedValue();
            if (!Pointids.Equals(""))
            {
                InitRadScheduler(true);
                ViewState["PointID"] = GetPointSelectedValue();
                factorCbxRsm.FactorBind(AuditType.SelectedItem.Value);//绑定SiteMap因子
                gridAuditLog.Rebind();
                RadGridAnalyze.Rebind();
            }
        }

        /// <summary>
        /// 反选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void inverse_Click(object sender, EventArgs e)
        {
            foreach (ListItem item in radioPoint.Items)
            {
                if (item.Selected)
                    item.Selected = false;
                else
                    item.Selected = true;
            }
            string Pointids = GetPointSelectedValue();
            if (!Pointids.Equals(""))
            {
                InitRadScheduler(true);
                ViewState["PointID"] = GetPointSelectedValue();
                factorCbxRsm.FactorBind(AuditType.SelectedItem.Value);//绑定SiteMap因子
                gridAuditLog.Rebind();
                RadGridAnalyze.Rebind();
            }
            else
            {
                Alert("请选择测点！");
                return;
            }
        }

        /// <summary>
        /// 不选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void unselect_Click(object sender, EventArgs e)
        {
            foreach (ListItem item in radioPoint.Items)
            {
                item.Selected = false;
            }
            string Pointids = GetPointSelectedValue();
            if (!Pointids.Equals(""))
            {
                InitRadScheduler(true);
                ViewState["PointID"] = GetPointSelectedValue();
                factorCbxRsm.FactorBind(AuditType.SelectedItem.Value);//绑定SiteMap因子
                gridAuditLog.Rebind();
                RadGridAnalyze.Rebind();
            }
            else
            {
                Alert("请选择测点！");
                return;
            }
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
        /// 获取选择的测点值（以;隔开）
        /// </summary>
        /// <returns></returns>
        private string GetPointSelectedValue()
        {
            string Pointids = "";
            foreach (ListItem item in radioPoint.Items)
            {
                if (item.Selected == true)
                    Pointids += Pointids.Equals("") ? item.Value : ";" + item.Value;
            }
            return Pointids;
        }
        #endregion

        /// <summary>
        /// 数据查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Search_Click(object sender, EventArgs e)
        {
            InitRadScheduler(true, true);
            gridAuditLog.Rebind();
            RadGridAnalyze.Rebind();
        }
        #endregion
        #endregion
    }
}