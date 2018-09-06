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
using SmartEP.Service.BaseData.Channel;
using SmartEP.Service.Frame;

namespace SmartEP.WebUI.Pages.EnvAir.Audit
{
    public partial class MutilPointAuditStateSuperCopy : SmartEP.WebUI.Common.BasePage
    {
        #region 属性
        AirAuditMonitoringPointService pointAirService = new AirAuditMonitoringPointService();//点位接口 空气
        WaterAuditMonitoringPointService pointWaterService = new WaterAuditMonitoringPointService();//点位接口 地表水
        AuditMonitoringPointPollutantService pollutantService = new AuditMonitoringPointPollutantService();//因子接口
        AuditDataService auditDataService = new AuditDataService();//审核数据接口
        AuditLogService auditLogService = new AuditLogService();//审核日志接口
        AuditOperatorService operatorService = new AuditOperatorService();//审核操作接口
        public static IQueryable<PointPollutantInfo> pollutantList = null;
        //CbxRsmControl myRSM = new CbxRsmControl();
        static DateTime currentBegin = DateTime.Now;
        static DateTime currentEnd = DateTime.Now;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["pointType"] != null)
                    ViewState["pointType"] = Request.QueryString["pointType"];
                else
                    ViewState["pointType"] = "0";
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
            PointBind();//初始化点位
            PollutantBind();//初始化因子
            RadCalendar1.FocusedDate = Request.QueryString["dtBegin"] != null ? Convert.ToDateTime(Request.QueryString["dtBegin"]) : DateTime.Now;
            InitRadScheduler(true);//初始化日历控件
            string pointids = GetPointSelectedValue();
            if (!pointids.Equals(""))
            {
                

            }
            #region 底部统计信息隐藏（环境空气中需要）
            if (!EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(Session["applicationUID"].ToString()))//空气
                RadPaneBottom.Visible = false;
            #endregion

            #region 清空Session
            if (ViewState["PointID"].ToString().Equals(""))
            {
                Session["FactorCodeSuper"] = "";
                Session["FactorNameSuper"] = "";
                Session["isEditSuper"] = "";
                Session["PollutantDecimalNumSuper"] = "";
                Session["PollutantUnitSuper"] = "";
            }
            #endregion
        }

        #region 点位绑定
        private void PointBind()
        {
            IQueryable<MonitoringPointEntity> pointList = null;
            if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(Session["applicationUID"]))
                pointList = pointAirService.RetrieveAirByAuditType(ViewState["pointType"].ToString(), Session["UserGuid"].ToString());
            else if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(Session["applicationUID"]))
                pointList = pointWaterService.RetrieveWaterByAuditType(ViewState["pointType"].ToString(), Session["UserGuid"].ToString());
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

        #region 因子绑定
        private void PollutantBind()
        {
            if (ViewState["PointID"].ToString() == "1")
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("text", typeof(string));
                dt.Columns.Add("value", typeof(string));

                DataRow drHg = dt.NewRow();
                drHg["text"] = "汞分析仪";
                drHg["value"] = "c50a2fc0-0832-42b0-be17-640503c9de70";
                dt.Rows.Add(drHg);
                DataRow drlijingpu = dt.NewRow();
                drlijingpu["text"] = "粒径谱仪";
                drlijingpu["value"] = "da92c7c1-4932-4007-a6d5-2866aa8c63f1";
                dt.Rows.Add(drlijingpu);

                radioPollutant.DataSource = dt;
                radioPollutant.DataTextField = "text";
                radioPollutant.DataValueField = "value";
                radioPollutant.DataBind();
            }
            else if (ViewState["PointID"].ToString() == "9")
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("text", typeof(string));
                dt.Columns.Add("value", typeof(string));

                DataRow drweibo = dt.NewRow();
                drweibo["text"] = "微波辐射";
                drweibo["value"] = "93c6df1e-cc2c-4997-b2c6-a8b16c1a742c";
                dt.Rows.Add(drweibo);

                radioPollutant.DataSource = dt;
                radioPollutant.DataTextField = "text";
                radioPollutant.DataValueField = "value";
                radioPollutant.DataBind();
            }
            else
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("text", typeof(string));
                dt.Columns.Add("value", typeof(string));

                DataRow drHg = dt.NewRow();
                drHg["text"] = "汞分析仪";
                drHg["value"] = "c50a2fc0-0832-42b0-be17-640503c9de70";
                dt.Rows.Add(drHg);
                DataRow drlijingpu = dt.NewRow();
                drlijingpu["text"] = "粒径谱仪";
                drlijingpu["value"] = "da92c7c1-4932-4007-a6d5-2866aa8c63f1";
                dt.Rows.Add(drlijingpu);
                DataRow drweibo = dt.NewRow();
                drweibo["text"] = "微波辐射";
                drweibo["value"] = "93c6df1e-cc2c-4997-b2c6-a8b16c1a742c";
                dt.Rows.Add(drweibo);

                radioPollutant.DataSource = dt;
                radioPollutant.DataTextField = "text";
                radioPollutant.DataValueField = "value";
                radioPollutant.DataBind();
            }
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
            string factorCode = "a21026;a21005;a05024;a21004;a34002;a34004";//常规6因子设置
            //string factorName = "二氧化硫;一氧化碳;臭氧;二氧化氮;PM10;PM2.5";
            string[] pointid = GetPointSelectedValue().Split(';');
            #region 从审核历史表导入审核预处理数据
            try
            {
                string AuditTransfer = ConfigurationManager.AppSettings["AuditTransfer"] != null ? ConfigurationManager.AppSettings["AuditTransfer"].ToString() : "";
                if (!AuditTransfer.Equals("") && AuditTransfer.Split(':')[0].Equals("1") && Convert.ToInt32(AuditTransfer.Split(':')[1]) > 0)
                    operatorService.GetDataFromHis(Session["applicationUID"].ToString(), pointid, RadDatePickerBegin.SelectedDate.Value.Date, RadDatePickerEnd.SelectedDate.Value.Date.AddDays(1).AddHours(-1), Convert.ToInt32(AuditTransfer.Split(':')[1]));
            }
            catch
            {
            }
            #endregion

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
            
            if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(Session["applicationUID"].ToString()))//空气
                gridAuditLog.DataSource = auditLogService.RerieveAirLog(Pointids.Split(';'), Session["applicationUID"].ToString(), RadDatePickerBegin.SelectedDate.Value.Date, RadDatePickerEnd.SelectedDate.Value.Date.AddDays(1).AddHours(-1));
            else if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(Session["applicationUID"].ToString()))//地表水
                gridAuditLog.DataSource = auditLogService.RerieveWaterLog(Pointids.Split(';'), Session["applicationUID"].ToString(), RadDatePickerBegin.SelectedDate.Value.Date, RadDatePickerEnd.SelectedDate.Value.Date.AddDays(1).AddHours(-1));

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
                if (ViewState["pointType"].Equals("0") && Session["applicationUID"].Equals("airaaira-aira-aira-aira-airaairaaira"))//国控点数据重载
                    result = operatorService.DeleteAudit(Pointids.Replace(";", ","), RadDatePickerBegin.SelectedDate.Value.Date, RadDatePickerEnd.SelectedDate.Value.Date.AddDays(1), "", "", "", "重新加载", "重新加载", "");
                else if (ViewState["pointType"].Equals("2") && Session["applicationUID"].Equals("airaaira-aira-aira-aira-airaairaaira"))//超级站数据重载
                    result = operatorService.DeleteAuditSuper(Session["applicationUID"].ToString(), Pointids.Split(';'), RadDatePickerBegin.SelectedDate.Value.Date, RadDatePickerEnd.SelectedDate.Value.Date.AddDays(1).AddHours(-1), "", "", "", "重新加载", "重新加载", "");
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
                ViewState["PointID"] = GetPointSelectedValue();
                PollutantBind();
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

                            cell = item["Tstamp"];
                            cell.Text = Convert.ToDateTime(row.Tstamp).AddHours(1).ToString("MM-dd HH:mm");
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
        /// 进入审核页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void EnterAudit_Click(object sender, EventArgs e)
        {
            try
            {
                string codes = Session["FactorCodeSuper"].ToString();
                if (codes.Contains("a99074"))
                {
                    MutilPointAuditDataHg.isfirst = true;
                    DateTime DTBegin = DateTime.Parse(selectDateTime.Value);
                    DateTime DTEnd = DateTime.Parse(selectDateTime.Value);

                    String myUrl = "MutilPointAuditDataHg.aspx?dtBegin=" + DTBegin + "&dtEnd=" + DTEnd + "&pointType=2&PointID=" + ViewState["PointID"] + "&Token=" + Request.QueryString["Token"];//AuditData.aspx
                    if (DTBegin.Date <= DateTime.Now.Date && !GetPointSelectedValue().Equals(""))
                        Server.Transfer(myUrl, false);
                    else
                        Alert("没有数据！");
                }
                else if (codes.Contains("401"))
                {
                    MutilPointAuditDataWeibo.isfirst = true;
                    DateTime DTBegin = DateTime.Parse(selectDateTime.Value);
                    DateTime DTEnd = DateTime.Parse(selectDateTime.Value);

                    String myUrl = "MutilPointAuditDataWeibo.aspx?dtBegin=" + DTBegin + "&dtEnd=" + DTEnd + "&pointType=2&PointID=" + ViewState["PointID"] + "&Token=" + Request.QueryString["Token"];//AuditData.aspx
                    if (DTBegin.Date <= DateTime.Now.Date && !GetPointSelectedValue().Equals(""))
                        Server.Transfer(myUrl, false);
                    else
                        Alert("没有数据！");
                }
                else
                {
                    MutilPointAuditDataLijingpu.isfirst = true;
                    DateTime DTBegin = DateTime.Parse(selectDateTime.Value);
                    DateTime DTEnd = DateTime.Parse(selectDateTime.Value);

                    String myUrl = "MutilPointAuditDataLijingpu.aspx?dtBegin=" + DTBegin + "&dtEnd=" + DTEnd + "&pointType=2&PointID=" + ViewState["PointID"] + "&Token=" + Request.QueryString["Token"];//AuditData.aspx
                    if (DTBegin.Date <= DateTime.Now.Date && !GetPointSelectedValue().Equals(""))
                        Server.Transfer(myUrl, false);
                    else
                        Alert("没有数据！");
                }
            }
            catch(Exception ex)
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
            RadGridAnalyze.Rebind();
        }

        #region 方法
        /// <summary>
        /// 日历日期处理
        /// </summary>
        /// <param name="dayStr"></param>
        /// <returns></returns>
        public String getDay(String dayStr)
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

        protected void radioPollutant_SelectedIndexChanged(object sender, EventArgs e)
        {
            string CategoryUid = "";
            string selCode = "";
            string selName = "";
            string isEdit = "";
            string PollutantDecimalNum = "";
            string PollutantUnit = "";
            for (int i = 0; i < radioPollutant.Items.Count; i++)
            {
                if (radioPollutant.Items[i].Selected)
                    CategoryUid = radioPollutant.Items[i].Value;
            }

            AirPollutantService airservice = new AirPollutantService();
            PollutantCodeEntity[] pEntity = airservice.RetrieveListByUseOrNot(true).Where(p => p.CategoryUid == CategoryUid).ToArray();
            for (int i = 0; i < pEntity.Length; i++)
            {
                selCode += selCode == "" ? pEntity[i].PollutantCode : ";" + pEntity[i].PollutantCode;
                selName += selName == "" ? pEntity[i].PollutantName : ";" + pEntity[i].PollutantName;
                PollutantDecimalNum += PollutantDecimalNum == "" ? pEntity[i].DecimalDigit.ToString() : ";" + pEntity[i].DecimalDigit.ToString();

                DictionaryService dservice = new DictionaryService();
                string Unit = dservice.GetTextByGuid(Service.Core.Enums.DictionaryType.AMS, "计量单位", pEntity[i].MeasureUnitUid);
                PollutantUnit += Unit + ";";
                isEdit += isEdit == "" ? "0" : ";" + "0";
            }

            Session["FactorCategorySuper"] = CategoryUid;
            Session["FactorCodeSuper"] = selCode;
            Session["FactorNameSuper"] = selName;
            Session["isEditSuper"] = isEdit;
            Session["PollutantDecimalNumSuper"] = PollutantDecimalNum;
            Session["PollutantUnitSuper"] = !PollutantUnit.Equals("") ? PollutantUnit.Substring(0, PollutantUnit.Length - 1) : "";
        }

        #endregion
    }
}