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
namespace SmartEP.WebUI.Pages.EnvAir.Audit
{
    public partial class AuditState : SmartEP.WebUI.Common.BasePage
    {
        #region 属性
        AirAuditMonitoringPointService pointAirService = new AirAuditMonitoringPointService();//点位接口 空气
        WaterAuditMonitoringPointService pointWaterService = new WaterAuditMonitoringPointService();//点位接口 地表水
        AuditMonitoringPointPollutantService pollutantService = new AuditMonitoringPointPollutantService();//因子接口
        AuditDataService auditDataService = new AuditDataService();//审核数据接口
        AuditLogService auditLogService = new AuditLogService();//审核日志接口
        AuditOperatorService operatorService = new AuditOperatorService();//审核操作接口
        //CbxRsmControl myRSM = new CbxRsmControl();
        #endregion

        #region 方法
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //pointType：空气（0：国控点；1：市控点位；2：超级站点位；3：省控点位；4：区控点位；5：创模点位；6：中意项目）
                //           地表水（0：水源地；1：城区河道；2：浮标站；3：人工监测点；）  
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
                InitControl();//初始化控件
            }
        }

        #region 初始化控件
        private void InitControl()
        {
            RadDatePickerBegin.SelectedDate = DateTime.Now.Date.AddDays(-7);
            RadDatePickerEnd.SelectedDate = DateTime.Now.Date.AddDays(-1);
            //RadCalendar1.FocusedDate = DateTime.Now.AddMonths(-1);
            PointBind();//初始化点位
            RadCalendar1.FocusedDate = Request.QueryString["dtBegin"] != null ? Convert.ToDateTime(Request.QueryString["dtBegin"]) : DateTime.Now;
            InitRadScheduler(true);//初始化日历控件
            if (!radioPoint.SelectedValue.Equals(""))
                factorCbxRsm.FactorBind(Convert.ToInt32(radioPoint.SelectedValue));//绑定SiteMap因子
            #region 底部统计信息隐藏（环境空气中需要）
            if (!EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(Session["applicationUID"].ToString()))//空气
                RadPaneBottom.Visible = false;
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
            if (radioPoint.Items.Count > 0)
            {
                if (Session["PointID"] != null)
                {
                    ListItem item = radioPoint.Items.FindByValue(Session["PointID"].ToString());
                    if (item == null)
                        radioPoint.Items[0].Selected = true;
                    else
                        item.Selected = true;
                }
                else
                    radioPoint.Items[0].Selected = true;
                Session["PointID"] = radioPoint.SelectedValue;
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
                string tem = "";
                AuditStatusForDayEntity status = new AuditStatusForDayEntity();
                if (!radioPoint.SelectedValue.Equals(""))
                {
                    status = auditLogService.RerieveAuditState(Convert.ToInt32(radioPoint.SelectedValue.Equals("") ? "-1" : radioPoint.SelectedValue), myDateTime, myDateTime, Session["applicationUID"].ToString());
                    if (status != null)
                    {
                        tem = status.Status;
                    }
                }
                #region 填充数据
                RadCalendarDay myDay = new RadCalendarDay();
                myDay.Date = myDateTime;
                myDay.TemplateID = "Adt" + tem;
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
        /// 点击日历控件进入审核数据页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RadCalendar1_SelectionChanged(object sender, Telerik.Web.UI.Calendar.SelectedDatesEventArgs e)
        {

            DateTime DTBegin = RadCalendar1.SelectedDate;
            DateTime DTEnd = RadCalendar1.SelectedDate;
            String myUrl = "AuditData.aspx?DTBegin=" + DTBegin + "&pointType=" + ViewState["pointType"] + "&Token=" + Request.QueryString["Token"];//AuditData.aspx
            if (DTBegin.Date <= DateTime.Now.Date && !radioPoint.SelectedValue.Equals(""))
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
            string[] pointid = (radioPoint.SelectedValue.Equals("") ? "-1" : radioPoint.SelectedValue).Split(';');
            RadGridAnalyze.DataSource = auditDataService.RetrieveAuditStatisticalData(Session["applicationUID"].ToString(), pointid, factorCode.Split(';'), RadDatePickerBegin.SelectedDate.Value, RadDatePickerEnd.SelectedDate.Value);
        }

        /// <summary>
        /// 绑定审核日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridAuditLog_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (!radioPoint.SelectedValue.Equals(""))
            {
                factorCbxRsm.FactorBind(Convert.ToInt32(radioPoint.SelectedValue));//绑定SiteMap因子
                factorCbxRsm.GetFactors();//获取sitemap因子，放入Session中保存
                if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(Session["applicationUID"].ToString()))//空气
                    gridAuditLog.DataSource = auditLogService.RerieveAirLog(Convert.ToInt32(radioPoint.SelectedValue.Equals("") ? "-1" : radioPoint.SelectedValue), Session["applicationUID"].ToString(), RadDatePickerBegin.SelectedDate.Value, RadDatePickerEnd.SelectedDate.Value);
                else if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(Session["applicationUID"].ToString()))//地表水
                    gridAuditLog.DataSource = auditLogService.RerieveWaterLog(Convert.ToInt32(radioPoint.SelectedValue.Equals("") ? "-1" : radioPoint.SelectedValue), Session["applicationUID"].ToString(), RadDatePickerBegin.SelectedDate.Value, RadDatePickerEnd.SelectedDate.Value);
            }
        }

        /// <summary>
        /// 删除审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DeleteAudit_Click(object sender, EventArgs e)
        {
            if (!radioPoint.SelectedValue.Equals(""))
            {

                bool result = false;
                if (ViewState["pointType"].Equals("0") && Session["applicationUID"].Equals("airaaira-aira-aira-aira-airaairaaira"))//国控点数据重载
                    result = operatorService.DeleteAudit(radioPoint.SelectedValue, RadDatePickerBegin.SelectedDate.Value.Date, RadDatePickerEnd.SelectedDate.Value.Date.AddDays(1), "", "", "", "重新加载", "重新加载", "");
                else
                    result = operatorService.DeleteAudit(Session["applicationUID"].ToString(), Convert.ToInt32(radioPoint.SelectedValue), RadDatePickerBegin.SelectedDate.Value.Date, RadDatePickerEnd.SelectedDate.Value.Date.AddDays(1).AddHours(-1), "", "", "", "重新加载", "重新加载", "");
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
                Alert("没有数据！");
            }
        }

        /// <summary>
        /// 测点切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void radioPoint_SelectedIndexChanged(object sender, EventArgs e)
        {
            factorCbxRsm.FactorBind(Convert.ToInt32(radioPoint.SelectedValue));//绑定SiteMap因子
            InitRadScheduler(true);
            Session["PointID"] = radioPoint.SelectedValue;
            gridAuditLog.Rebind();
            if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(Session["applicationUID"].ToString()))//空气
                RadGridAnalyze.Rebind();
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
                            if (!cell.Text.Equals("") && !cell.Text.Equals("--"))
                                cell.Text = DecimalExtension.GetRoundValue(Convert.ToDecimal(cell.Text), DecimalNum).ToString();
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
                //if (e.Initiator.ClientID.Equals("DeleteAudit") && e.Updated.ID.Equals("RadGridAnalyze"))
                //{
                //    e.Canceled = true;
                //}
            }
            else
                e.Canceled = false;
        }

        #region 自定义方法
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
        #endregion
        #endregion

        #endregion
    }
}