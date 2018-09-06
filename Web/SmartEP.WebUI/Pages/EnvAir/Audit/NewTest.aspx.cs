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
using SmartEP.MonitoringBusinessRepository.Air;
using SmartEP.Core.Generic;
using SmartEP.Service.BaseData.MPInfo;
using log4net;
using SmartEP.Service.DataAnalyze.Air.AQIReport;
using SmartEP.Service.DataAnalyze.Air;

namespace SmartEP.WebUI.Pages.EnvAir.Audit
{
    public partial class NewTest : SmartEP.WebUI.Common.BasePage
    {
        #region 属性
        private DayAQIService m_DayAQIService;
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
        MonitoringInstrumentService instrumentService = new MonitoringInstrumentService();
        MonitoringBusinessModel MonitoringBusinessModel = new MonitoringBusinessModel();
        BaseDataModel BaseDataModel = new BaseDataModel();
        ILog log = LogManager.GetLogger("FileLogging");//获取一个日志记录器
        /// <summary>
        /// 审核因子配置接口
        /// </summary>
        SmartEP.Service.AutoMonitoring.common.AuditMonitoringPointService g_AuditMonitoringPointService = Singleton<SmartEP.Service.AutoMonitoring.common.AuditMonitoringPointService>.GetInstance();
        /// <summary>
        /// 应用程序Uid
        /// </summary>
        string ApplicationUid = SmartEP.Core.Enums.EnumMapping.GetApplicationValue(SmartEP.Core.Enums.ApplicationValue.Air);
        /// <summary>
        /// 超级站监站点状态类型
        /// </summary>
        int PointType = 1;
        /// <summary>
        /// 测点类
        /// </summary>
        MonitoringPointAirService g_MonitoringPointAir = null;
        private AQICalculateService m_AQICalculateService;
        /// <summary>
        /// 点位日AQI
        /// </summary>
        DayAQIRepository pointDayAQI = null;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            m_DayAQIService = new DayAQIService();

            m_AQICalculateService = new AQICalculateService();
            string names = ConfigurationManager.AppSettings["NTRegionPointName"].ToString();    //从配置文件获取默认站点
            string name = ConfigurationManager.AppSettings["NTRegionPointNames"].ToString();    //从配置文件获取默认站点
            pointCbxRsms.SetPointValuesFromNames(names);
            pointCbxRsm.SetPointValuesFromNames(name);
            if (!IsPostBack)
            {
                InitControl();
            }
        }
        #region 页面控件初始化
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            RadDatePickerBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddYears(-1).AddMonths(1).ToString("yyyy-MM"));
            RadDatePickerEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"));
            RadCalendar1.FocusedDate = Convert.ToDateTime(DateTime.Now.AddYears(-1).AddMonths(1).ToString("yyyy-MM"));
            //InitRadScheduler(true);//初始化日历控件
            BindGrid("");
        }
        #endregion
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
        ///  除环境空气外，统计信息表格导致重新加载失败,所以讲刷新统计信息表格的Ajax取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RadAjaxManager1_AjaxSettingCreating(object sender, AjaxSettingCreatingEventArgs e)
        {
            if (!EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals("airaaira-aira-aira-aira-airaairaaira"))//空气
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
        /// 日历切换日期
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RadCalendar1_DefaultViewChanged(object sender, Telerik.Web.UI.Calendar.DefaultViewChangedEventArgs e)
        {
            InitRadScheduler(false);//日历绑定
        }

        #region 日历控件绑定
        private void InitRadScheduler(Boolean IsReset)
        {
            DateTime beginDate, endDate;
            beginDate = RadCalendar1.CalendarView.ViewStartDate.AddDays(-7);
            endDate = RadCalendar1.CalendarView.ViewEndDate.AddDays(10);
            int recordTotal = 0;
            if (IsReset)
            {
                this.RadCalendar1.SpecialDays.Clear();
            }
            for (DateTime myDateTime = beginDate; myDateTime <= endDate; myDateTime = myDateTime.AddDays(1))  //循环日历控件日期
            {
                string status = "";
                if (rbtnlType.SelectedValue == "CityProper")
                {
                    string[] portIds = pointCbxRsms.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
                    if (!portIds.Equals(""))
                    {
                        string[] typeArr = { "优", "良", "轻度污染", "中度污染", "重度污染", "严重污染", "无效天" };
                        DataTable dataTable = m_DayAQIService.GetAirQualityDayReportNew(portIds, myDateTime, myDateTime, typeArr, 24, 0, out recordTotal, "PointId,DateTime Desc").ToTable();
                        if (dataTable.IsNotNullOrDBNull() && dataTable.Rows.Count > 0)
                        {
                            status = dataTable.Rows[0]["Grade"].ToString();
                        }

                        #region 填充数据
                        RadCalendarDay myDay = new RadCalendarDay();
                        myDay.Date = myDateTime;
                        myDay.TemplateID = "Adt" + status;
                        this.RadCalendar1.SpecialDays.Add(myDay);
                        #endregion
                    }
                }
                else
                {
                    string[] portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
                    if (!portIds.Equals(""))
                    {
                        string[] typeArr = { "优", "良", "轻度污染", "中度污染", "重度污染", "严重污染", "无效天" };
                        DataTable dataTable = m_DayAQIService.GetAirQualityDayReportNew(portIds, myDateTime, myDateTime, typeArr, 24, 0, out recordTotal, "PointId,DateTime Desc").ToTable();
                        if (dataTable.IsNotNullOrDBNull() && dataTable.Rows.Count>0)
                        {
                            status = dataTable.Rows[0]["Grade"].ToString();
                        }
                        
                        #region 填充数据
                        RadCalendarDay myDay = new RadCalendarDay();
                        myDay.Date = myDateTime;
                        myDay.TemplateID = "Adt" + status;
                        this.RadCalendar1.SpecialDays.Add(myDay);
                        #endregion
                    }
                }
                
                #region 填充数据
                //RadCalendarDay myDay = new RadCalendarDay();
                //myDay.Date = myDateTime;
                //myDay.TemplateID = "Adt" + status;
                //this.RadCalendar1.SpecialDays.Add(myDay);
                #endregion
            }

        }
        #endregion

        /// <summary>
        /// 进入审核页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void EnterAudit_Click(object sender, EventArgs e)
        {
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
            //RadGridAnalyze.Rebind();
        }
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid(string type)
        {
            if(type=="日历表")
            {
                
            }
            else if (type == "柱形图")
            {
            
            }
            else
            {
                
            }
            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            string Pie = "[";
            string Col = "[";
            string Months = "[";
            string[] portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
            DateTime dtStart = Convert.ToDateTime(RadDatePickerBegin.SelectedDate.ToString());
            DateTime dtEnd = Convert.ToDateTime(RadDatePickerEnd.SelectedDate.ToString()).AddMonths(1).AddSeconds(-1);
            if (rbtnlType.SelectedValue == "CityProper")
            {
                portIds = pointCbxRsms.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
                DataTable dt = new DataTable();
                dt.Columns.Add("portIds", typeof(string));
                dt.Columns.Add("StandardDays", typeof(string));
                dt.Columns.Add("OverDays", typeof(string));
                dt.Columns.Add("InvalidDays", typeof(string));
                dt.Columns.Add("StandardDaysRate", typeof(string));

                DataView dataView = m_AQICalculateService.GetRegionAQI(portIds, dtStart, dtEnd, 24, "2").DefaultView;
                DataTable dts = dataView.ToTable();
                decimal all = dataView.Count;
                DataRow drNew = dt.NewRow();
                //dts.Rows[0]["PointId"]
                dataView.RowFilter = "PointId='" + dts.Rows[0]["PointId"] + "' and AQIValue <> '' and AQIValue<=50";
                decimal Good = dataView.Count;
                dataView.RowFilter = "PointId='" + dts.Rows[0]["PointId"] + "' and AQIValue <> '' and AQIValue<=100 and AQIValue>50";
                decimal Moderate = dataView.Count;
                dataView.RowFilter = "PointId='" + dts.Rows[0]["PointId"] + "' and AQIValue <> '' and AQIValue<=150 and AQIValue>100";
                decimal LightlyPolluted = dataView.Count;
                dataView.RowFilter = "PointId='" + dts.Rows[0]["PointId"] + "' and AQIValue <> '' and AQIValue<=200 and AQIValue>150";
                decimal ModeratelyPolluted = dataView.Count;
                dataView.RowFilter = "PointId='" + dts.Rows[0]["PointId"] + "' and AQIValue <> '' and AQIValue<=300 and AQIValue>200";
                decimal HeavilyPolluted = dataView.Count;
                dataView.RowFilter = "PointId='" + dts.Rows[0]["PointId"] + "' and AQIValue <> '' and AQIValue>300";
                decimal SeverelyPolluted = dataView.Count;
                //decimal x = Good / all;
                Pie += "['优(" + Good + "天)'," + Good / all + "],['良(" + Moderate + "天)'," + Moderate / all + "],['轻度污染(" + LightlyPolluted + "天)',"
                    + LightlyPolluted / all + "],['中度污染(" + ModeratelyPolluted + "天)'," + ModeratelyPolluted / all + "],['重度污染(" + HeavilyPolluted + "天)',"
                    + HeavilyPolluted / all + "],['严重污染(" + SeverelyPolluted + "天)'," + SeverelyPolluted / all + "]]";

                int k = (dtEnd - dtStart).Months();
                hdName.Value = dts.Rows[0]["PointId"].ToString();
                for (int i = 0; i <= (dtEnd - dtStart).Months(); i++)
                {
                    Months += "'" + dtStart.AddMonths(i).ToString("yyyy-MM") + "',";
                    DataView dvz = m_AQICalculateService.GetRegionAQI(portIds, dtStart.AddMonths(i), dtStart.AddMonths(i + 1).AddSeconds(-1), 24, "2").DefaultView;

                    decimal allz = dvz.Count;
                    DataRow drNews = dt.NewRow();
                    dvz.RowFilter = "PointId='" + dts.Rows[0]["PointId"] + "' and AQIValue <> '' and AQIValue<=50";
                    decimal Goodz = dvz.Count;
                    dvz.RowFilter = "PointId='" + dts.Rows[0]["PointId"] + "' and AQIValue <> '' and AQIValue<=100 and AQIValue>50";
                    decimal Moderatez = dvz.Count;

                    Col += (Goodz + Moderatez) * 100 / allz + ",";
                }
                Col = Col.TrimEnd(',');
                Col += "]";
            }
            else
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("portIds", typeof(string));
                dt.Columns.Add("StandardDays", typeof(string));
                dt.Columns.Add("OverDays", typeof(string));
                dt.Columns.Add("InvalidDays", typeof(string));
                dt.Columns.Add("StandardDaysRate", typeof(string));
                int recordTotal = 0;
                pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
                DataView dv = pointDayAQI.GetDataPager(portIds, dtStart, dtEnd, int.MaxValue, 0, out recordTotal, "DateTime,PointId");
                foreach (string portItem in portIds)
                {
                    decimal all = dv.Count;
                    DataRow drNew = dt.NewRow();
                    dv.RowFilter = "PointId=" + portItem + " and AQIValue <> '' and AQIValue<=50";
                    decimal Good = dv.Count;
                    dv.RowFilter = "PointId=" + portItem + " and AQIValue <> '' and AQIValue<=100 and AQIValue>50";
                    decimal Moderate = dv.Count;
                    dv.RowFilter = "PointId=" + portItem + " and AQIValue <> '' and AQIValue<=150 and AQIValue>100";
                    decimal LightlyPolluted = dv.Count;
                    dv.RowFilter = "PointId=" + portItem + " and AQIValue <> '' and AQIValue<=200 and AQIValue>150";
                    decimal ModeratelyPolluted = dv.Count;
                    dv.RowFilter = "PointId=" + portItem + " and AQIValue <> '' and AQIValue<=300 and AQIValue>200";
                    decimal HeavilyPolluted = dv.Count;
                    dv.RowFilter = "PointId=" + portItem + " and AQIValue <> '' and AQIValue>300";
                    decimal SeverelyPolluted = dv.Count;
                    //decimal x = Good / all;
                    Pie += "['优(" + Good + "天)'," + Good / all + "],['良(" + Moderate + "天)'," + Moderate / all + "],['轻度污染(" + LightlyPolluted + "天)',"
                        + LightlyPolluted / all + "],['中度污染(" + ModeratelyPolluted + "天)'," + ModeratelyPolluted / all + "],['重度污染(" + HeavilyPolluted + "天)',"
                        + HeavilyPolluted / all + "],['严重污染(" + SeverelyPolluted + "天)'," + SeverelyPolluted / all + "]]";
                }
                int k = (dtEnd - dtStart).Months();
                hdName.Value = g_MonitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(portIds[0])).MonitoringPointName;
                for (int i = 0; i <= (dtEnd - dtStart).Months(); i++)
                {
                    Months += "'" + dtStart.AddMonths(i).ToString("yyyy-MM") + "',";
                    DataView dvz = pointDayAQI.GetDataPager(portIds, dtStart.AddMonths(i), dtStart.AddMonths(i + 1).AddSeconds(-1), int.MaxValue, 0, out recordTotal, "DateTime,PointId");

                    decimal allz = dvz.Count;
                    DataRow drNew = dt.NewRow();
                    dvz.RowFilter = "PointId=" + portIds[0] + " and AQIValue <> '' and AQIValue<=50";
                    decimal Goodz = dvz.Count;
                    dvz.RowFilter = "PointId=" + portIds[0] + " and AQIValue <> '' and AQIValue<=100 and AQIValue>50";
                    decimal Moderate = dvz.Count;
                    if (allz != 0)
                    {
                        Col += (Goodz + Moderate) * 100 / allz + ",";
                    }
                    else
                    {
                        Col += "0,";
                    }

                }
                Col = Col.TrimEnd(',');
                Col += "]";
            }
            hdPie.Value = Pie;
            hdCol.Value = Col;
            hdMonth.Value = Months.TrimEnd(',') + "]";


            InitRadScheduler(true);
        }
        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Search_Click(object sender, EventArgs e)
        {
            BindGrid("");
            if (tabStrip.SelectedIndex == 1)
            {
                RegisterScript("ColChart();");
            }
            else if (tabStrip.SelectedIndex == 2)
            {
                RegisterScript("PieChart();");
            }
        }

        protected void rbtnlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rbtnlType.SelectedValue == "CityProper")
            {
                pointCbxRsm.Visible = false;
                pointCbxRsms.Visible = true;
            }
            else
            {
                pointCbxRsm.Visible = true;
                pointCbxRsms.Visible = false;
            }
        }

        protected void RadCalendar1_DayRender(object sender, Telerik.Web.UI.Calendar.DayRenderEventArgs e)
        {
            //if (e.Day.is)
            //    e.Cell.Controls.Clear(); 
        }
    }
}