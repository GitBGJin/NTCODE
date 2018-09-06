﻿using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.AutoMonitoring.Air;
using SmartEP.Service.BaseData.BusinessRule;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.Core.Enums;
using SmartEP.Service.DataAnalyze.Air.DataQuery;
using SmartEP.Service.Frame;
using SmartEP.Utilities.Calendar;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Utilities.Office;
using SmartEP.Utilities.Web.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
namespace SmartEP.WebUI.Pages.EnvAir.SuperStationManagement
{
    /// <summary>
    /// 名称：OzonePrecursor.cs
    /// 创建人：刘晋
    /// 创建日期：2017-05-26
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：常规因子
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class GeneralFactor : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        InfectantBy1Service m_Min1Data = Singleton<InfectantBy1Service>.GetInstance();
        InfectantBy5Service m_Min5Data = Singleton<InfectantBy5Service>.GetInstance();
        InfectantBy60Service m_Min60Data = Singleton<InfectantBy60Service>.GetInstance();
        InfectantByDayService m_DayOriData = Singleton<InfectantByDayService>.GetInstance();
        InfectantByMonthService m_MonthOriData = Singleton<InfectantByMonthService>.GetInstance();
        DataQueryByHourService m_HourData = Singleton<DataQueryByHourService>.GetInstance();
        DataQueryByDayService m_DayData = Singleton<DataQueryByDayService>.GetInstance();
        DataQueryByWeekService m_WeekData = Singleton<DataQueryByWeekService>.GetInstance();
        DataQueryByMonthService m_MonthData = Singleton<DataQueryByMonthService>.GetInstance();
        DataQueryBySeasonService m_SeasonData = Singleton<DataQueryBySeasonService>.GetInstance();
        DataQueryByYearService m_YearData = Singleton<DataQueryByYearService>.GetInstance();
        InstrumentChannelService m_InstrumentChannelService = Singleton<InstrumentChannelService>.GetInstance();
        MonitoringPointAirService monitoringPointAir = new MonitoringPointAirService();
        //获取因子上下限数据处理服务
        ExcessiveSettingService m_ExcessiveSettingService = new ExcessiveSettingService();
        //代码项服务层
        DictionaryService dicService = new DictionaryService();
        //获取因子小数位
        // channelCode 因子代码 例：CO的code是a21005  （ channelCode=a21005）
        SmartEP.Service.BaseData.Channel.AirPollutantService m_AirPollutantService = new SmartEP.Service.BaseData.Channel.AirPollutantService();

        static DateTime dtms = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00"));
        string LZSPfactor = string.Empty;
        /// <summary>
        /// 选择因子
        /// </summary>
        private IList<IPollutant> factors = null;
        private string[] strFactors = null;
        string LZSfactorName = string.Empty;
        /// <summary>
        /// 选择站点
        /// </summary>
        private IList<IPoint> points = null;
        private string[] portIds = null;
        /// <summary>
        /// 统计
        /// </summary>
        DataView dvStatistical = null;
        /// <summary>
        /// 全局日数据表 List
        /// </summary>
        DataTable dt = null;
        /// <summary>
        /// 国家标记位
        /// </summary>
        IQueryable<V_CodeMainItemEntity> siteTypeEntites = null;
        /// <summary>
        /// 全局日数据表 图标
        /// </summary>
        DataTable dtIcon = null;
        /// <summary>
        /// 全局周数据表 List
        /// </summary>
        DataTable dtWeek = null;

        /// <summary>
        /// 全局周数据表 图标
        /// </summary>
        DataTable dtWeekIcon = null;
        /// <summary>
        /// 全局日数据表 List
        /// </summary>
        DataTable dtMonth = null;

        /// <summary>
        /// 全局日数据表 图标
        /// </summary>
        DataTable dtMonthIcon = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //hdPointName.Value = cbPoint.SelectedText;
                InitControl();
                //gridAudit.DataSource = new DataTable();
                //timer.Enabled = true;
            }
        }
        #region 页面控件初始化
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {

            ////初始化站点
            string FactorNames = System.Configuration.ConfigurationManager.AppSettings["AirPollutant1"];
            factorCbxRsm.SetFactorValuesFromCodes(FactorNames);
            //时间框初始化
            DateTime dt = DateTime.Now;
            DateTime dtt = DateTime.Now.AddHours(-2);
            dt.ToLongTimeString().ToString();
            dtt.ToLongTimeString().ToString();
            dtpBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddHours(-24).ToString("yyyy-MM-dd HH:mm"));
            dtpEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));

            dtpDayBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd"));
            dtpDayEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));

            dtpMonthBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddYears(-1).ToString("yyyy-MM"));
            dtpMonthEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToString("yyyy-MM"));

            hourBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddHours(-47).ToString("yyyy-MM-dd HH:00"));
            hourEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00"));
            dayBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd"));
            dayEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
            monthBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToString("yyyy-MM"));
            monthEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"));
            seasonBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"));
            seasonEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"));
            weekBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToString("yyyy-MM"));
            weekEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"));

            yearBegin.SelectedDate = DateTime.Now.AddYears(-1);
            yearEnd.SelectedDate = DateTime.Now;

            BindWeekComboBox();//绑定周
            SetLiteral();//显示周日期范围
            //数据类型
            radlDataType.Items.Add(new ListItem("小时数据", PollutantDataType.Hour.ToString()));
            radlDataType.Items.Add(new ListItem("日数据", PollutantDataType.Day.ToString()));
            radlDataType.Items.Add(new ListItem("周数据", PollutantDataType.Week.ToString()));
            radlDataType.Items.Add(new ListItem("月数据", PollutantDataType.Month.ToString()));
            radlDataType.Items.Add(new ListItem("季数据", PollutantDataType.Season.ToString()));
            radlDataType.Items.Add(new ListItem("年数据", PollutantDataType.Year.ToString()));
            radlDataType.SelectedValue = PollutantDataType.Hour.ToString();

            RadioButtonList1.Items.Add(new ListItem("一分钟数据", PollutantDataType.Min1.ToString()));
            //RadioButtonList1.Items.Add(new ListItem("五分钟数据", PollutantDataType.Min5.ToString()));
            RadioButtonList1.Items.Add(new ListItem("小时数据", PollutantDataType.Min60.ToString()));
            RadioButtonList1.Items.Add(new ListItem("日数据", PollutantDataType.OriDay.ToString()));
            RadioButtonList1.Items.Add(new ListItem("月数据", PollutantDataType.OriMonth.ToString()));
            RadioButtonList1.SelectedValue = PollutantDataType.Min60.ToString();
            dbtMonth.Visible = false;

            dbtWeek.Visible = false;
        }
        #endregion
        protected void timer_Tick(object sender, EventArgs e)
        {
            gridAudit.CurrentPageIndex = 0;
            gridAudit.Rebind();
            timer.Enabled = false;
        }
        /// <summary>
        /// 判断是否是超级站
        /// </summary>
        string isSuper = string.Empty;
        string isAudit = string.Empty;
        protected override void OnPreInit(EventArgs e)
        {
            isSuper = PageHelper.GetQueryString("superOrNot");
            pointCbxRsm.isSuper(isSuper);
            isAudit = PageHelper.GetQueryString("auditOrNot");
            if (isAudit != null && isAudit != "")
            {
                factorCbxRsm.isAudit(isAudit);
            }
        }
        /// <summary>
        /// 显示所选周的日期
        /// </summary>
        private void SetLiteral()
        {
            DateTime endDate = DateTime.ParseExact(weekTo.SelectedValue, "yyyy-MM-dd", null).AddDays(6);
            txtweekF.Text = string.Format("{0:yyyy-MM-dd}", weekFrom.SelectedValue);
            txtweekT.Text = string.Format("{0:yyyy-MM-dd}", endDate);
        }
        /// <summary>
        /// 绑定周
        /// </summary>
        private void BindWeekComboBox()
        {
            weekFrom.DataValueField = "value";
            weekFrom.DataTextField = "text";
            weekFrom.DataSource = ChinaDate.GetWeekOfMonth(weekBegin.SelectedDate.Value);
            weekFrom.DataBind();
            weekTo.DataValueField = "value";
            weekTo.DataTextField = "text";
            weekTo.DataSource = ChinaDate.GetWeekOfMonth(weekEnd.SelectedDate.Value);
            weekTo.DataBind();
            SetLiteral();
        }
        /// <summary>
        /// 数据来源切换事件
        /// </summary>
        protected void ddlDataSource_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            if (ddlDataSource.SelectedValue == "OriData")
            {
                radlDataType.Visible = false;
                RadioButtonList1.Visible = true;
                dtpHour.Visible = true;
                dtpDay.Visible = false;
                dtpMonth.Visible = false;
                dbtHour.Visible = false;
                dbtDay.Visible = false;
                dbtWeek.Visible = false;
                dbtMonth.Visible = false;
                dbtSeason.Visible = false;
                dbtYear.Visible = false;
            }
            if (ddlDataSource.SelectedValue == "AuditData")
            {
                radlDataType.Visible = true;
                RadioButtonList1.Visible = false;
                dtpDay.Visible = false;
                dtpMonth.Visible = false;
                dtpHour.Visible = false;

                dbtHour.Visible = true;
                dbtDay.Visible = false;
                dbtWeek.Visible = false;
                dbtMonth.Visible = false;
                dbtSeason.Visible = false;
                dbtYear.Visible = false;
            }
        }
         #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid(string type)
        {
            DataTable dt = new DataTable();
            if (!IsPostBack)
            {

            }
            try
            {
                List<string> listName = new List<string>();
                List<string> listCode = new List<string>();
                factors = factorCbxRsm.GetFactors();
                foreach (IPollutant factor in factors)
                {
                    listCode.Add(factor.PollutantCode);
                    listName.Add(factor.PollutantName);
                }
                string[] factorCodes = listCode.ToArray();
                string[] factorNames = listName.ToArray();

                DateTime dtBegion = DateTime.Now;
                DateTime dtEnd = DateTime.Now;
                points = pointCbxRsm.GetPoints();
                portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
                //string[] portIds = { cbPoint.SelectedValue };

                //生成RadGrid的绑定列
                dvStatistical = null;

                //每页显示数据个数            
                int pageSize1 = int.MaxValue;
                //当前页的序号
                int pageNo1 = 0;
                var auditData1 = new DataView();

                //每页显示数据个数            
                int pageSize = gridAudit.PageSize;
                //当前页的序号
                int pageNo = gridAudit.CurrentPageIndex;

                var auditData = new DataView();
                //数据总行数
                int recordTotal = 0;
                string orderBy = "PointId asc,DateTime desc";
                if (portIds != null)
                {
                    if (ddlDataSource.SelectedValue == "AuditData")
                    {
                        if (radlDataType.SelectedValue == "Hour")
                        {
                            string orderBy1 = "PointId asc,Tstamp desc";
                            if (TimeSort.SelectedValue == "时间升序")
                                orderBy1 = "PointId asc,Tstamp asc";
                            dtBegion = hourBegin.SelectedDate.Value;
                            dtEnd = hourEnd.SelectedDate.Value.AddDays(1).AddSeconds(-1);
                            if (type == "列表")
                            {
                                auditData = m_HourData.GetNewHourDataPager(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy1);//小时类型按 小时数据查询
                                SetHiddenData(portIds, factors, dtBegion, dtEnd);
                                hdGroupFac.Value = string.Join(";", factors.Select(p => p.PollutantCode).ToArray());
                                hdGroupName.Value = "常规参数";
                            }
                            else
                            {
                                //【给隐藏域赋值，用于显示Chart】
                                
                                auditData1 = m_HourData.GetHourDataPagerNew(portIds, factorCodes, dtBegion, dtEnd, pageSize1, pageNo1, out recordTotal);//小时类型按 小时数据查询
                            }
                        }
                        else if (radlDataType.SelectedValue == "Day")
                        {
                            string orderBy1 = "PointId asc,DateTime desc";
                            if (TimeSort.SelectedValue == "时间升序")
                                orderBy1 = "PointId asc,DateTime asc";
                            dtBegion = dayBegin.SelectedDate.Value;
                            dtEnd = dayEnd.SelectedDate.Value;
                            if (type == "列表")
                            {
                                auditData = m_DayData.GetDayData(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy1);//日类型按 日数据查询
                                SetHiddenData(portIds, factors, dtBegion, dtEnd);
                                hdGroupFac.Value = string.Join(";", factors.Select(p => p.PollutantCode).ToArray());
                                hdGroupName.Value = "常规参数";
                            }
                            else
                            {
                                //【给隐藏域赋值，用于显示Chart】
                                
                                auditData1 = m_DayData.GetDayData(portIds, factorCodes, dtBegion, dtEnd, pageSize1, pageNo1, out recordTotal, orderBy1);//日类型按 日数据查询
                            }

                        }
                        //月数据
                        else if (radlDataType.SelectedValue == "Month")
                        {

                            string orderBy1 = "PointId asc,Year desc,MonthOfYear desc";
                            if (TimeSort.SelectedValue == "时间升序")
                                orderBy1 = "PointId asc,Year asc,MonthOfYear asc";
                            int monthB = monthBegin.SelectedDate.Value.Year;
                            int monthE = monthEnd.SelectedDate.Value.Year;
                            int monthF = monthBegin.SelectedDate.Value.Month;
                            int monthT = monthEnd.SelectedDate.Value.Month;

                            DateTime mtBegin = monthBegin.SelectedDate.Value;
                            //本月第一天时间 
                            dtBegion = mtBegin.AddDays(-(mtBegin.Day) + 1);

                            DateTime mtEnd = monthEnd.SelectedDate.Value;
                            //将本月月数+1 
                            DateTime dt2 = mtEnd.AddMonths(1);
                            //本月最后一天时间 
                            dtEnd = dt2.AddDays(-(mtEnd.Day)).AddDays(1).AddSeconds(-1);
                            if (type == "列表")
                            {
                                auditData = m_MonthData.GetMonthDataPager(portIds, factorCodes, monthB, monthF, monthE, monthT, pageSize, pageNo, out recordTotal, orderBy1); //月类型 按月数据查询
                                SetHiddenData(portIds, factors, monthB + ";" + monthF + ";" + monthE + ";" + monthT);
                                hdGroupFac.Value = string.Join(";", factors.Select(p => p.PollutantCode).ToArray());
                                hdGroupName.Value = "常规参数";
                            }
                            else
                            {
                                //【给隐藏域赋值，用于显示Chart】
                                
                                auditData1 = m_MonthData.GetMonthDataPager(portIds, factorCodes, monthB, monthF, monthE, monthT, pageSize1, pageNo1, out recordTotal, orderBy1); //月类型 按月数据查询
                            }
                        }

                        //周数据
                        else if (radlDataType.SelectedValue == "Week")
                        {

                            string orderBy1 = "PointId asc,Year desc,WeekOfYear desc";
                            if (TimeSort.SelectedValue == "时间升序")
                                orderBy1 = "PointId asc,Year asc,WeekOfYear asc";
                            int weekB = weekBegin.SelectedDate.Value.Year;
                            int weekE = weekEnd.SelectedDate.Value.Year;
                            int newyear = DateTime.ParseExact(weekFrom.SelectedValue, "yyyy-MM-dd", null).AddDays(6).Year;
                            int nyear = DateTime.ParseExact(weekTo.SelectedValue, "yyyy-MM-dd", null).Year;
                            int weekF = 0;
                            int weekT = 0;
                            if (newyear > weekB)
                            {
                                weekF = ChinaDate.WeekOfYear(DateTime.ParseExact(weekFrom.SelectedValue, "yyyy-MM-dd", null));
                            }
                            else
                                weekF = ChinaDate.WeekOfYear(DateTime.ParseExact(weekFrom.SelectedValue, "yyyy-MM-dd", null).AddDays(6));

                            if (weekE > nyear)
                            {
                                weekT = ChinaDate.WeekOfYear(DateTime.ParseExact(weekTo.SelectedValue, "yyyy-MM-dd", null).AddDays(6));
                            }
                            else
                            {
                                weekT = ChinaDate.WeekOfYear(DateTime.ParseExact(weekTo.SelectedValue, "yyyy-MM-dd", null));
                            }

                            if (type == "列表")
                            {
                                auditData = m_WeekData.GetWeekDataPager(portIds, factorCodes, weekB, weekF, weekE, weekT, pageSize, pageNo, out recordTotal, orderBy1); //周类型 按周数据查询
                                //【给隐藏域赋值，用于显示Chart】
                                SetHiddenData(portIds, factors, weekB + ";" + weekF + ";" + weekE + ";" + weekT);
                                hdGroupFac.Value = string.Join(";", factors.Select(p => p.PollutantCode).ToArray());
                                hdGroupName.Value = "常规参数";
                            }
                            else
                            {
                                //【给隐藏域赋值，用于显示Chart】
                                //SetHiddenData(portIds, factors, weekB + ";" + weekF + ";" + weekE + ";" + weekT);
                                //auditData1 = m_DayData.GetDayData(portIds, factorCodes, dtBegion, dtEnd, pageSize1, pageNo1, out recordTotal);//周类型 按日数据查询
                                auditData1 = m_WeekData.GetWeekDataPager(portIds, factorCodes, weekB, weekF, weekE, weekT, pageSize1, pageNo1, out recordTotal, orderBy1); //周类型 按周数据查询
                            }
                        }
                        else if (radlDataType.SelectedValue == "Season")
                        {
                            string orderBy1 = "PointId asc,Year desc,SeasonOfYear desc";
                            if (TimeSort.SelectedValue == "时间升序")
                                orderBy1 = "PointId asc,Year asc,SeasonOfYear asc";
                            int seasonB = seasonBegin.SelectedDate.Value.Year;
                            int seasonE = seasonEnd.SelectedDate.Value.Year;
                            int seasonF = Convert.ToInt32(seasonFrom.SelectedValue);
                            int seasonT = Convert.ToInt32(seasonTo.SelectedValue);
                            //dtBegion = Convert.ToDateTime(weekFrom.SelectedValue);
                            //dtEnd = DateTime.ParseExact(weekTo.SelectedValue, "yyyy-MM-dd", null).AddDays(6).AddDays(1).AddSeconds(-1);
                            if (type == "列表")
                            {
                                auditData = m_SeasonData.GetSeasonDataPager(portIds, factorCodes, seasonB, seasonF, seasonE, seasonT, pageSize, pageNo, out recordTotal, orderBy1); //季类型 按季数据查询
                                SetHiddenData(portIds, factors, seasonB + ";" + seasonF + ";" + seasonE + ";" + seasonT);
                                hdGroupFac.Value = string.Join(";", factors.Select(p => p.PollutantCode).ToArray());
                                hdGroupName.Value = "常规参数";
                            }
                            else
                            {
                                //【给隐藏域赋值，用于显示Chart】
                                //SetHiddenData(portIds, factors, weekB + ";" + weekF + ";" + weekE + ";" + weekT);
                                //auditData = m_SeasonData.GetSeasonDataPager(portIds, factorCodes, seasonB, seasonF, seasonE, seasonT, pageSize, pageNo, out recordTotal, orderBy); //季类型 按日数据查询
                                auditData1 = m_SeasonData.GetSeasonDataPager(portIds, factorCodes, seasonB, seasonF, seasonE, seasonT, pageSize1, pageNo1, out recordTotal, orderBy1); //季类型 按季数据查询
                            }
                        }
                        else if (radlDataType.SelectedValue == "Year")
                        {
                            string orderBy1 = "PointId asc,Year desc";
                            if (TimeSort.SelectedValue == "时间升序")
                                orderBy1 = "PointId asc,Year asc";
                            int yearB = yearBegin.SelectedDate.Value.Year;
                            int yearE = yearEnd.SelectedDate.Value.Year;
                            //dtBegion = Convert.ToDateTime(weekFrom.SelectedValue);
                            //dtEnd = DateTime.ParseExact(weekTo.SelectedValue, "yyyy-MM-dd", null).AddDays(6).AddDays(1).AddSeconds(-1);
                            if (type == "列表")
                            {
                                auditData = m_YearData.GetYearDataPager(portIds, factorCodes, yearB, yearE, pageSize, pageNo, out recordTotal, orderBy1); //年类型 按年数据查询
                                //【给隐藏域赋值，用于显示Chart】
                                SetHiddenData(portIds, factors, yearB + ";" + yearE);
                                hdGroupFac.Value = string.Join(";", factors.Select(p => p.PollutantCode).ToArray());
                                hdGroupName.Value = "常规参数";
                            }
                            else
                            {
                                //【给隐藏域赋值，用于显示Chart】
                                //SetHiddenData(portIds, factors, weekB + ";" + weekF + ";" + weekE + ";" + weekT);
                                //auditData1 = m_DayData.GetDayData(portIds, factorCodes, dtBegion, dtEnd, pageSize1, pageNo1, out recordTotal);//季类型 按日数据查询
                                auditData1 = m_YearData.GetYearDataPager(portIds, factorCodes, yearB, yearE, pageSize1, pageNo1, out recordTotal, orderBy1); //年类型 按年数据查询
                            }
                        }
                    }
                    else
                    {
                        if (RadioButtonList1.SelectedValue == "Min1")
                        {
                            string orderBy1 = "PointId asc,Tstamp desc";
                            if (TimeSort.SelectedValue == "时间升序")
                                orderBy1 = "PointId asc,Tstamp asc";
                            dtBegion = dtpBegin.SelectedDate.Value;
                            dtEnd = dtpEnd.SelectedDate.Value;
                            if (type == "列表")
                            {
                                auditData = m_Min1Data.GetDataPager(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy1);//一分钟类型按 一分钟数据查询
                                SetHiddenData(portIds, factors, dtBegion, dtEnd);
                                hdGroupFac.Value = string.Join(";", factors.Select(p => p.PollutantCode).ToArray());
                                hdGroupName.Value = "常规参数";
                            }
                            else
                            {
                                //【给隐藏域赋值，用于显示Chart】
                                
                                //auditData1 = m_HourData.GetHourDataPagerNew(portIds, factorCodes, dtBegion, dtEnd, pageSize1, pageNo1, out recordTotal);//一分钟类型按 小时数据查询
                                auditData = m_Min1Data.GetDataPager(portIds, factorCodes, dtBegion, dtEnd, pageSize1, pageNo1, out recordTotal, orderBy1);//一分钟类型按 一分钟数据查询
                            }
                        }
                        else if (RadioButtonList1.SelectedValue == "Min5")
                        {
                            string orderBy1 = "PointId asc,Tstamp desc";
                            if (TimeSort.SelectedValue == "时间升序")
                                orderBy1 = "PointId asc,Tstamp asc";
                            dtBegion = dtpBegin.SelectedDate.Value;
                            dtEnd = dtpEnd.SelectedDate.Value;
                            if (type == "列表")
                            {
                                auditData = m_Min5Data.GetDataPager(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy1);//五分钟类型按 小时数据查询
                                SetHiddenData(portIds, factors, dtBegion, dtEnd);
                                hdGroupFac.Value = string.Join(";", factors.Select(p => p.PollutantCode).ToArray());
                                hdGroupName.Value = "常规参数";
                            }
                            else
                            {
                                //【给隐藏域赋值，用于显示Chart】
                                
                                //auditData1 = m_HourData.GetHourDataPagerNew(portIds, factorCodes, dtBegion, dtEnd, pageSize1, pageNo1, out recordTotal);//五分钟类型按 小时数据查询
                                auditData = m_Min5Data.GetDataPager(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy1);//五分钟类型按 五分钟数据查询
                            }
                        }
                        else if (RadioButtonList1.SelectedValue == "Min60")
                        {
                            string orderBy1 = "PointId asc,Tstamp desc";
                            if (TimeSort.SelectedValue == "时间升序")
                                orderBy1 = "PointId asc,Tstamp asc";
                            dtBegion = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:00:00"));
                            dtEnd = Convert.ToDateTime(dtpEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:00:00"));
                            if (type == "列表")
                            {
                                //auditData = m_Min60Data.GetDataPagerForO3AllTime(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy1);//原始小时数据查询
                                auditData = m_Min60Data.GetDataPagerAllTimeWithO8(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy1);
                                //auditData = m_Min60Data.GetDataPager(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy1);//60分钟类型按 60分钟数据查询
                                SetHiddenData(portIds, factors, dtBegion, dtEnd);
                                hdGroupFac.Value = string.Join(";", factors.Select(p => p.PollutantCode).ToArray());
                                hdGroupName.Value = "常规参数";
                            }
                            else
                            {
                                //【给隐藏域赋值，用于显示Chart】
                                
                                //auditData1 = m_HourData.GetHourDataPagerNew(portIds, factorCodes, dtBegion, dtEnd, pageSize1, pageNo1, out recordTotal);//60分钟类型按 小时数据查询
                                auditData = m_Min60Data.GetDataPager(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy1);//60分钟类型按 60分钟数据查询
                            }
                        }
                        else if (RadioButtonList1.SelectedValue == "OriDay")
                        {
                            string orderBy1 = "PointId asc,DateTime desc";
                            if (TimeSort.SelectedValue == "时间升序")
                                orderBy1 = "PointId asc,DateTime asc";
                            dtBegion = dtpDayBegin.SelectedDate.Value;
                            dtEnd = dtpDayEnd.SelectedDate.Value;
                            if (type == "列表")
                            {
                                auditData = m_DayOriData.GetDataPagerForNTO3AllTime(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy1);
                                //auditData = m_DayOriData.GetDataPagers(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy1);//原始日数据类型按 原始日数据查询
                                SetHiddenData(portIds, factors, dtBegion, dtEnd);
                                hdGroupFac.Value = string.Join(";", factors.Select(p => p.PollutantCode).ToArray());
                                hdGroupName.Value = "常规参数";
                            }
                            else
                            {
                                //【给隐藏域赋值，用于显示Chart】
                                
                                //auditData1 = m_HourData.GetHourDataPagerNew(portIds, factorCodes, dtBegion, dtEnd, pageSize1, pageNo1, out recordTotal);//60分钟类型按 小时数据查询
                                auditData = m_DayOriData.GetDataPagers(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy1);//原始日数据类型按 原始日数据查询
                            }
                        }
                        else if (RadioButtonList1.SelectedValue == "OriMonth")
                        {
                            string orderBy1 = "PointId asc,Year desc,MonthOfYear desc";
                            if (TimeSort.SelectedValue == "时间升序")
                                orderBy1 = "PointId asc,Year asc,MonthOfYear asc";
                            dtBegion = dtpMonthBegin.SelectedDate.Value;
                            dtEnd = dtpMonthEnd.SelectedDate.Value;
                            if (type == "列表")
                            {
                                auditData = m_MonthOriData.GetOriDataPager(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy1);//原始月数据类型按 原始月数据查询
                                SetHiddenData(portIds, factors, dtBegion, dtEnd);
                                hdGroupFac.Value = string.Join(";", factors.Select(p => p.PollutantCode).ToArray());
                                hdGroupName.Value = "常规参数";
                            }
                            else
                            {
                                //【给隐藏域赋值，用于显示Chart】
                                
                                //auditData1 = m_HourData.GetHourDataPagerNew(portIds, factorCodes, dtBegion, dtEnd, pageSize1, pageNo1, out recordTotal);//60分钟类型按 小时数据查询
                                auditData = m_MonthOriData.GetOriDataPager(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy1);//原始月数据类型按 原始月数据查询
                            }
                        }
                    }

                        dt = auditData.ToTable();
                        
                        if (dt != null)
                        {
                            bool a = dt.Columns.Contains("Tstamp");
                            if (a && RadioButtonList1.SelectedValue != "Min60")
                            {
                                if (dt.Columns.Count > 2)
                                {
                                    dt.Columns["Tstamp"].SetOrdinal(1);
                                }
                            }
                            bool b = dt.Columns.Contains("DateTime");
                            if (b)
                            {
                                if (dt.Columns.Count > 2)
                                {
                                    dt.Columns["DateTime"].SetOrdinal(1);
                                }
                            }
                        }

                        gridAudit.DataSource = dt.DefaultView;
                        //数据总行数
                        gridAudit.VirtualItemCount = recordTotal;
                        //国家数据标记位
                        siteTypeEntites = dicService.RetrieveList(DictionaryType.AMS, "国家数据标记");
                    //}
                    
                        hdHeavyMetalMonitor.Value = ToJson(dt);
                }
                else
                {
                    gridAudit.DataSource = new DataTable();
                }
                
            }
            catch (Exception ex) { }
        }

        /// <summary>   
        /// Datatable转换为Json   
        /// </summary>   
        /// <param name="table">Datatable对象</param>   
        /// <returns>Json字符串</returns>   
        public static string ToJson(DataTable dt)
        {
            StringBuilder jsonString = new StringBuilder();
            jsonString.Append("[");
            DataRowCollection drc = dt.Rows;
            for (int i = 0; i < drc.Count; i++)
            {
                jsonString.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string strKey = dt.Columns[j].ColumnName;
                    string strValue = drc[i][j].ToString();
                    Type type = dt.Columns[j].DataType;
                    jsonString.Append("\"" + strKey + "\":");
                    strValue = StringFormat(strKey, strValue, type);
                    if (j < dt.Columns.Count - 1)
                    {
                        jsonString.Append(strValue + ",");
                    }
                    else
                    {
                        jsonString.Append(strValue);
                    }
                }
                jsonString.Append("},");
            }
            if (jsonString.ToString() != "[") jsonString.Remove(jsonString.Length - 1, 1);
            jsonString.Append("]");
            return jsonString.ToString();
        }

        /// <summary>  
        /// 格式化字符型、日期型、布尔型  
        /// </summary>  
        /// <param name="str"></param>  
        /// <param name="type"></param>  
        /// <returns></returns>  
        private static string StringFormat(string key, string str, Type type)
        {
            if (key.Equals("Tstamp", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(str))
            {
                str = ConvertDateTimeToInt(Convert.ToDateTime(str)).ToString();
            }
            if (key.Equals("DateTime", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(str))
            {
                str = ConvertDateTimeToInt(Convert.ToDateTime(str)).ToString();
            }
            if (type == typeof(string))
            {
                str = String2Json(str);
                str = "\"" + str + "\"";
            }
            else if (type == typeof(DateTime))
            {
                str = "\"" + str + "\"";
            }
            else if (type == typeof(bool))
            {
                str = str.ToLower();
            }
            else if (string.IsNullOrEmpty(str))
            {
                str = "null";
            }
            return str;
        }
        /// <summary>  
        /// 过滤特殊字符  
        /// </summary>  
        /// <param name="s"></param>  
        /// <returns></returns>  
        private static string String2Json(String s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s.ToCharArray()[i];
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\""); break;
                    case '\\':
                        sb.Append("\\\\"); break;
                    case '/':
                        sb.Append("\\/"); break;
                    case '\b':
                        sb.Append("\\b"); break;
                    case '\f':
                        sb.Append("\\f"); break;
                    case '\n':
                        sb.Append("\\n"); break;
                    case '\r':
                        sb.Append("\\r"); break;
                    case '\t':
                        sb.Append("\\t"); break;
                    default:
                        sb.Append(c); break;
                }
            }
            return sb.ToString();
        }

        /// <summary>  
        ///DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time">时间</param>  
        /// <returns>long</returns>  
        public static long ConvertDateTimeToInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.AddHours(-8).Ticks) / 10000;   //除10000调整为13位      
            return t;
        }

        #endregion
        #region 绑定图表
        private void BindChart()
        {
            RegisterScript("InitGroupChart();");
        }
        #endregion
        /// <summary>
        /// 查找按钮事件
        /// </summary>
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            if (tabStrip.SelectedTab.Text == "图表")
            {
                BindGrid("列表");
                BindChart();
            }
            else
            {
                gridAudit.CurrentPageIndex = 0;
                if (ddlDataSource.SelectedValue == "OriData" && RadioButtonList1.SelectedValue != "Min1")
                {
                    if (dtms == Convert.ToDateTime(dtpEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:00")))
                    {
                        dtpEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00"));
                    }
                }
                gridAudit.Rebind();
                FirstLoadChart.Value = "1";
            }
        }
        /// <summary>
        /// 数据类型切换事件
        /// </summary>
        protected void radlDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            gridAudit.CurrentPageIndex = 0;
            //小时数据
            if (radlDataType.SelectedValue == "Hour")
            {
                dbtHour.Visible = true;
                dbtDay.Visible = false;
                dbtWeek.Visible = false;
                dbtMonth.Visible = false;
                dbtSeason.Visible = false;
                dbtYear.Visible = false;
            }
            //日数据
            else if (radlDataType.SelectedValue == "Day")
            {
                dbtDay.Visible = true;

                dbtHour.Visible = false;
                dbtWeek.Visible = false;
                dbtMonth.Visible = false;
                dbtSeason.Visible = false;
                dbtYear.Visible = false;

            }
            //周数据
            else if (radlDataType.SelectedValue == "Week")
            {
                dbtWeek.Visible = true;

                dbtHour.Visible = false;
                dbtDay.Visible = false;
                dbtMonth.Visible = false;
                dbtSeason.Visible = false;
                dbtYear.Visible = false;

            }
            //月数据
            else if (radlDataType.SelectedValue == "Month")
            {
                dbtMonth.Visible = true;

                dbtHour.Visible = false;
                dbtDay.Visible = false;
                dbtWeek.Visible = false;
                dbtSeason.Visible = false;
                dbtYear.Visible = false;

            }
            //季数据
            else if (radlDataType.SelectedValue == "Season")
            {
                dbtSeason.Visible = true;

                dbtHour.Visible = false;
                dbtDay.Visible = false;
                dbtWeek.Visible = false;
                dbtMonth.Visible = false;
                dbtYear.Visible = false;

            }
            //年数据
            else if (radlDataType.SelectedValue == "Year")
            {
                dbtYear.Visible = true;

                dbtHour.Visible = false;
                dbtDay.Visible = false;
                dbtWeek.Visible = false;
                dbtMonth.Visible = false;
                dbtSeason.Visible = false;

            }
        }
        /// <summary>
        /// 原始数据对应数据类型切换事件
        /// </summary>
        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RadioButtonList1.SelectedValue == "Min1")
            {
                dtpHour.Visible = true;
                dtpDay.Visible = false;
                dtpMonth.Visible = false;
                dtpBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm"));
                dtpEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                dtpBegin.DateInput.DateFormat = "yyyy-MM-dd HH:mm";
                dtpEnd.DateInput.DateFormat = "yyyy-MM-dd HH:mm";
            }
            //else if (RadioButtonList1.SelectedValue == "Min5")
            //{
            //    dtpBegin.DateInput.DateFormat = "yyyy-MM-dd HH:mm";
            //    dtpEnd.DateInput.DateFormat = "yyyy-MM-dd HH:mm";
            //}
            else if (RadioButtonList1.SelectedValue == "Min60")
            {
                dtpHour.Visible = true;
                dtpDay.Visible = false;
                dtpMonth.Visible = false;
                dtpBegin.DateInput.DateFormat = "yyyy-MM-dd HH:00";
                dtpEnd.DateInput.DateFormat = "yyyy-MM-dd HH:00";
            }
            else if (RadioButtonList1.SelectedValue == "OriDay")
            {
                //dtpBegin.DateInput.DateFormat = "yyyy-MM-dd";
                //dtpEnd.DateInput.DateFormat = "yyyy-MM-dd";
                dtpHour.Visible = false;
                dtpDay.Visible = true;
                dtpMonth.Visible = false;
            }
            else
            {
                //dtpBegin.DateInput.DateFormat = "yyyy-MM";
                //dtpEnd.DateInput.DateFormat = "yyyy-MM";
                dtpHour.Visible = false;
                dtpDay.Visible = false;
                dtpMonth.Visible = true;
            }
        }
        #region 周数据更新日期范围
        /// <summary>
        /// 周数据更新日期范围
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void weekBegin_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            BindWeekComboBox();
        }
        /// <summary>
        /// 周数据更新日期范围
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void weekFrom_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            SetLiteral();
        }
        /// <summary>
        /// 周数据更新日期范围
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void weekEnd_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            BindWeekComboBox();
        }
        /// <summary>
        /// 周数据更新日期范围
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void weekTo_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            SetLiteral();
        }
        #endregion
        /// <summary>
        /// ToolBar事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridRTB_ButtonClick(object sender, RadToolBarEventArgs e)
        {
            List<string> listName = new List<string>();
                List<string> listCode = new List<string>();
                factors = factorCbxRsm.GetFactors();
                foreach (IPollutant factor in factors)
                {
                    listCode.Add(factor.PollutantCode);
                    listName.Add(factor.PollutantName);
                }
                string[] factorCodes = listCode.ToArray();
                string[] factorNames = listName.ToArray();

                DateTime dtBegion = DateTime.Now;
                DateTime dtEnd = DateTime.Now;
                points = pointCbxRsm.GetPoints();
                portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
                //string[] portIds = { cbPoint.SelectedValue };

                //生成RadGrid的绑定列
                dvStatistical = null;

                //每页显示数据个数            
                int pageSize1 = int.MaxValue;
                //当前页的序号
                int pageNo1 = 0;
                var auditData1 = new DataView();

                //每页显示数据个数            
                int pageSize = gridAudit.PageSize;
                //当前页的序号
                int pageNo = gridAudit.CurrentPageIndex;

                var auditData = new DataView();
                //数据总行数
                int recordTotal = 0;
                string orderBy = "PointId asc,DateTime desc";
                if (portIds != null)
                {
                    if (ddlDataSource.SelectedValue == "AuditData")
                    {
                        if (radlDataType.SelectedValue == "Hour")
                        {
                            string orderBy1 = "PointId asc,Tstamp desc";
                            if (TimeSort.SelectedValue == "时间升序")
                                orderBy1 = "PointId asc,Tstamp asc";
                            dtBegion = hourBegin.SelectedDate.Value;
                            dtEnd = hourEnd.SelectedDate.Value.AddDays(1).AddSeconds(-1);
                            auditData = m_HourData.GetNewHourDataPager(portIds, factorCodes, dtBegion, dtEnd, pageSize1, pageNo1, out recordTotal, orderBy1);//小时类型按 小时数据查询

                        }
                        else if (radlDataType.SelectedValue == "Day")
                        {
                            string orderBy1 = "PointId asc,DateTime desc";
                            if (TimeSort.SelectedValue == "时间升序")
                                orderBy1 = "PointId asc,DateTime asc";
                            dtBegion = dayBegin.SelectedDate.Value;
                            dtEnd = dayEnd.SelectedDate.Value;
                            auditData = m_DayData.GetDayData(portIds, factorCodes, dtBegion, dtEnd, pageSize1, pageNo1, out recordTotal, orderBy1);//日类型按 日数据查询

                            

                        }
                        //月数据
                        else if (radlDataType.SelectedValue == "Month")
                        {

                            string orderBy1 = "PointId asc,Year desc,MonthOfYear desc";
                            if (TimeSort.SelectedValue == "时间升序")
                                orderBy = "PointId asc,Year asc,MonthOfYear asc";
                            int monthB = monthBegin.SelectedDate.Value.Year;
                            int monthE = monthEnd.SelectedDate.Value.Year;
                            int monthF = monthBegin.SelectedDate.Value.Month;
                            int monthT = monthEnd.SelectedDate.Value.Month;

                            DateTime mtBegin = monthBegin.SelectedDate.Value;
                            //本月第一天时间 
                            dtBegion = mtBegin.AddDays(-(mtBegin.Day) + 1);

                            DateTime mtEnd = monthEnd.SelectedDate.Value;
                            //将本月月数+1 
                            DateTime dt2 = mtEnd.AddMonths(1);
                            //本月最后一天时间 
                            dtEnd = dt2.AddDays(-(mtEnd.Day)).AddDays(1).AddSeconds(-1);
                            auditData = m_MonthData.GetMonthDataPager(portIds, factorCodes, monthB, monthF, monthE, monthT, pageSize1, pageNo1, out recordTotal, orderBy1); //月类型 按月数据查询
                            
                        }

                        //周数据
                        else if (radlDataType.SelectedValue == "Week")
                        {

                            string orderBy1 = "PointId asc,Year desc,WeekOfYear desc";
                            if (TimeSort.SelectedValue == "时间升序")
                                orderBy1 = "PointId asc,Year asc,WeekOfYear asc";
                            int weekB = weekBegin.SelectedDate.Value.Year;
                            int weekE = weekEnd.SelectedDate.Value.Year;
                            int newyear = DateTime.ParseExact(weekFrom.SelectedValue, "yyyy-MM-dd", null).AddDays(6).Year;
                            int nyear = DateTime.ParseExact(weekTo.SelectedValue, "yyyy-MM-dd", null).Year;
                            int weekF = 0;
                            int weekT = 0;
                            if (newyear > weekB)
                            {
                                weekF = ChinaDate.WeekOfYear(DateTime.ParseExact(weekFrom.SelectedValue, "yyyy-MM-dd", null));
                            }
                            else
                                weekF = ChinaDate.WeekOfYear(DateTime.ParseExact(weekFrom.SelectedValue, "yyyy-MM-dd", null).AddDays(6));

                            if (weekE > nyear)
                            {
                                weekT = ChinaDate.WeekOfYear(DateTime.ParseExact(weekTo.SelectedValue, "yyyy-MM-dd", null).AddDays(6));
                            }
                            else
                            {
                                weekT = ChinaDate.WeekOfYear(DateTime.ParseExact(weekTo.SelectedValue, "yyyy-MM-dd", null));
                            }

                            //dtBegion = Convert.ToDateTime(weekFrom.SelectedValue);
                            //dtEnd = DateTime.ParseExact(weekTo.SelectedValue, "yyyy-MM-dd", null).AddDays(6).AddDays(1).AddSeconds(-1);
                            auditData = m_WeekData.GetWeekDataPager(portIds, factorCodes, weekB, weekF, weekE, weekT, pageSize1, pageNo1, out recordTotal, orderBy1); //周类型 按周数据查询
                            
                        }
                        else if (radlDataType.SelectedValue == "Season")
                        {
                            string orderBy1 = "PointId asc,Year desc,SeasonOfYear desc";
                            if (TimeSort.SelectedValue == "时间升序")
                                orderBy1 = "PointId asc,Year asc,SeasonOfYear asc";
                            int seasonB = seasonBegin.SelectedDate.Value.Year;
                            int seasonE = seasonEnd.SelectedDate.Value.Year;
                            int seasonF = Convert.ToInt32(seasonFrom.SelectedValue);
                            int seasonT = Convert.ToInt32(seasonTo.SelectedValue);
                            //dtBegion = Convert.ToDateTime(weekFrom.SelectedValue);
                            //dtEnd = DateTime.ParseExact(weekTo.SelectedValue, "yyyy-MM-dd", null).AddDays(6).AddDays(1).AddSeconds(-1);
                            auditData = m_SeasonData.GetSeasonDataPager(portIds, factorCodes, seasonB, seasonF, seasonE, seasonT, pageSize1, pageNo1, out recordTotal, orderBy1); //季类型 按季数据查询
                           
                        }
                        else if (radlDataType.SelectedValue == "Year")
                        {
                            string orderBy1 = "PointId asc,Year desc";
                            if (TimeSort.SelectedValue == "时间升序")
                                orderBy1 = "PointId asc,Year asc";
                            int yearB = yearBegin.SelectedDate.Value.Year;
                            int yearE = yearEnd.SelectedDate.Value.Year;
                            //dtBegion = Convert.ToDateTime(weekFrom.SelectedValue);
                            //dtEnd = DateTime.ParseExact(weekTo.SelectedValue, "yyyy-MM-dd", null).AddDays(6).AddDays(1).AddSeconds(-1);
                            auditData = m_YearData.GetYearDataPager(portIds, factorCodes, yearB, yearE, pageSize1, pageNo1, out recordTotal, orderBy1); //年类型 按年数据查询
                            
                        }
                    }
                    else
                    {
                        if (RadioButtonList1.SelectedValue == "Min1")
                        {
                            string orderBy1 = "PointId asc,Tstamp desc";
                            if (TimeSort.SelectedValue == "时间升序")
                                orderBy1 = "PointId asc,Tstamp asc";
                            dtBegion = dtpBegin.SelectedDate.Value;
                            dtEnd = dtpEnd.SelectedDate.Value;

                            auditData = m_Min1Data.GetDataPager(portIds, factorCodes, dtBegion, dtEnd, pageSize1, pageNo1, out recordTotal, orderBy1);//一分钟类型按 一分钟数据查询

                        }
                        else if (RadioButtonList1.SelectedValue == "Min5")
                        {
                            string orderBy1 = "PointId asc,Tstamp desc";
                            if (TimeSort.SelectedValue == "时间升序")
                                orderBy1 = "PointId asc,Tstamp asc";
                            dtBegion = dtpBegin.SelectedDate.Value;
                            dtEnd = dtpEnd.SelectedDate.Value;

                            auditData = m_Min5Data.GetDataPager(portIds, factorCodes, dtBegion, dtEnd, pageSize1, pageNo1, out recordTotal, orderBy1);//五分钟类型按 小时数据查询

                        }
                        else if (RadioButtonList1.SelectedValue == "Min60")
                        {
                            string orderBy1 = "PointId asc,Tstamp desc";
                            if (TimeSort.SelectedValue == "时间升序")
                                orderBy1 = "PointId asc,Tstamp asc";
                            dtBegion = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:00:00"));
                            dtEnd = Convert.ToDateTime(dtpEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:00:00"));

                            //auditData = m_Min60Data.GetDataPagerForO3AllTime(portIds, factorCodes, dtBegion, dtEnd, pageSize1, pageNo1, out recordTotal, orderBy1);//原始小时数据查询
                            auditData = m_Min60Data.GetDataPagerAllTimeWithO8(portIds, factorCodes, dtBegion, dtEnd, pageSize1, pageNo1, out recordTotal, orderBy1);
                            //for (int i = auditData.Table.Columns.Count - 1; i >= 0; i--)
                            //{
                            //    DataColumn dc = auditData.Table.Columns[i];
                            //    if ( dc.ColumnName.ToString().Contains("rows"))
                            //    {
                            //        auditData.Table.Columns.Remove(dc.ColumnName.ToString());
                            //    }
                            //}
                            //auditData = m_Min60Data.GetDataPager(portIds, factorCodes, dtBegion, dtEnd, pageSize1, pageNo1, out recordTotal, orderBy1);//60分钟类型按 60分钟数据查询

                        }
                        else if (RadioButtonList1.SelectedValue == "OriDay")
                        {
                            string orderBy1 = "PointId asc,DateTime desc";
                            if (TimeSort.SelectedValue == "时间升序")
                                orderBy1 = "PointId asc,DateTime asc";
                            dtBegion = dtpDayBegin.SelectedDate.Value;
                            dtEnd = dtpDayEnd.SelectedDate.Value;
                            //auditData = m_DayOriData.GetDataPagers(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy1);//原始日类型按 60分钟数据查询
                            auditData = m_DayOriData.GetDataPagerForNTO3AllTime(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy1);
                            
                        }
                        else if (RadioButtonList1.SelectedValue == "OriMonth")
                        {
                            string orderBy1 = "PointId asc,Year desc,MonthOfYear desc";
                            if (TimeSort.SelectedValue == "时间升序")
                                orderBy1 = "PointId asc,Year asc,MonthOfYear asc";
                            dtBegion = dtpMonthBegin.SelectedDate.Value;
                            dtEnd = dtpMonthEnd.SelectedDate.Value;
                            auditData = m_MonthOriData.GetOriDataPager(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy1);//原始月类型按 60分钟数据查询

                        }
                    }
                    dt = auditData.ToTable();
                    
                    decimal value = 0M;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            if (dt.Columns[j].ColumnName.Contains("a34004") || dt.Columns[j].ColumnName.Contains("a34005") || dt.Columns[j].ColumnName.Contains("a34002") || dt.Columns[j].ColumnName.Contains("a21026") || dt.Columns[j].ColumnName.Contains("a21004") || dt.Columns[j].ColumnName.Contains("a05024") || dt.Columns[j].ColumnName.Contains("a05027") || dt.Columns[j].ColumnName.Contains("a21003") || dt.Columns[j].ColumnName.Contains("a21002") || dt.Columns[j].ColumnName.Contains("a51001") || dt.Columns[j].ColumnName.Contains("a51002") || dt.Columns[j].ColumnName.Contains("a51003") || dt.Columns[j].ColumnName.Contains("a51004"))
                            {
                                if (!string.IsNullOrWhiteSpace(dt.Rows[i][j].ToString()))
                                {
                                    if (decimal.TryParse(dt.Rows[i][j].ToString(), out value))
                                    {
                                        dt.Rows[i][j] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }

                            }
                            else if (dt.Columns[j].ColumnName.Contains("a21005") || dt.Columns[j].ColumnName.Contains("a01001") || dt.Columns[j].ColumnName.Contains("a01007") || dt.Columns[j].ColumnName.Contains("a01008") || dt.Columns[j].ColumnName.Contains("a01006") || dt.Columns[j].ColumnName.Contains("a01020"))
                            {
                                if (!string.IsNullOrWhiteSpace(dt.Rows[i][j].ToString()))
                                {
                                    if (decimal.TryParse(dt.Rows[i][j].ToString(), out value))
                                    {
                                        dt.Rows[i][j] = DecimalExtension.GetPollutantValue(value, 1);
                                    }
                                }

                            }
                            else if (dt.Columns[j].ColumnName.Contains("a01002"))
                            {
                                if (!string.IsNullOrWhiteSpace(dt.Rows[i][j].ToString()))
                                {
                                    if (decimal.TryParse(dt.Rows[i][j].ToString(), out value))
                                    {
                                        dt.Rows[i][j] = DecimalExtension.GetPollutantValue(value, 0);
                                    }
                                }

                            }
                            else
                            {
                                if (!string.IsNullOrWhiteSpace(dt.Rows[i][j].ToString()))
                                {
                                    if (decimal.TryParse(dt.Rows[i][j].ToString(), out value))
                                    {
                                        dt.Rows[i][j] = DecimalExtension.GetPollutantValue(value, 3);
                                    }
                                }
                            }
                        }
                    }
                    //for (int i = 0; i < dt.Rows.Count; i++)
                    //{
                    //    foreach (string factorCode in factorCodes)
                    //    {

                    //        int DecimalNum = 3;

                    //            if (m_AirPollutantService.GetPollutantInfo(factorCode) != null)
                    //            {
                    //                if (!string.IsNullOrWhiteSpace(m_AirPollutantService.GetPollutantInfo(factorCode).PollutantDecimalNum))
                    //                {
                    //                    DecimalNum = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo(factorCode).PollutantDecimalNum);
                    //                }
                    //            }
                    //            if (dt.Rows[i][factorCode] != DBNull.Value)
                    //            {
                    //                //value 需要进行小数位处理的数据 类型Decimal
                    //                if (m_AirPollutantService.GetPollutantInfo(factorCode).PollutantMeasureUnit == "μg/m³")
                    //                {
                    //                    dt.Rows[i][factorCode] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i][factorCode]), DecimalNum)*1000;
                    //                }
                    //                else
                    //                {
                    //                    dt.Rows[i][factorCode] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i][factorCode]), DecimalNum);
                    //                }
                                    
                    //            }
                    //    }
                    //}
                        
                        if (dt != null)
                        {
                            bool a = dt.Columns.Contains("Tstamp");
                            if (a)
                            {
                                if (dt.Columns.Count > 2)
                                {
                                    dt.Columns["Tstamp"].SetOrdinal(1);
                                }
                            }
                            bool b = dt.Columns.Contains("DateTime");
                            if (b)
                            {
                                if (dt.Columns.Count > 2)
                                {
                                    dt.Columns["DateTime"].SetOrdinal(1);
                                }
                            }
                        }
                        if (RadioButtonList1.SelectedValue == "OriDay")
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                dt.Rows[i]["DateTime"] = Convert.ToDateTime(dt.Rows[i]["DateTime"].ToString()).ToString("yyyy-MM-dd");
                            }
                        }
                        DataTable dts = UpdateExportColumnName(dt.DefaultView);
                        ExcelHelper.DataTableToExcel(dts, "常规参数", "常规参数数据", this.Page);
                    }
               
        }
        /// <summary>
        /// 绑定数据源（用于分页、排序）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridAudit_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindGrid("列表");
        }

        /// <summary>
        /// 修改要导出的数据表中的列名
        /// </summary>
        /// <param name="dv">原始数据表</param>
        /// <returns></returns>
        private DataTable UpdateExportColumnName(DataView dv)
        {
            DataTable dtOld = dv.ToTable();
            DataTable dtNew = dtOld.Clone();
            //dtNew.Columns["Tstamp"].DataType = typeof(string);
            
            if (ddlDataSource.SelectedValue == "AuditData")
            {
                if (radlDataType.SelectedValue == "Day")
                {
                    dtNew.Columns["DateTime"].DataType = typeof(string);
                }
            }
            else
            {
                if (RadioButtonList1.SelectedValue == "OriDay")
                {
                    dtNew.Columns["DateTime"].DataType = typeof(string);
                }
            }
            
            for (int i = 0; i < dtOld.Rows.Count; i++)
            {
                DataRow drOld = dtOld.Rows[i];
                DataRow drNew = dtNew.NewRow();
                foreach (DataColumn dcOld in dtOld.Columns)
                {
                    if (!string.IsNullOrWhiteSpace(drOld[dcOld].ToString()))
                    {
                        drNew[dcOld.ColumnName] = drOld[dcOld].ToString().Replace("<br/>", " \r\n");
                    }

                }
                dtNew.Rows.Add(drNew);
            }


            dtNew.Columns.Add("站点", typeof(string)).SetOrdinal(0);
            for (int i = 0; i < dtNew.Rows.Count; i++)
            {
                DataRow drNew = dtNew.Rows[i];

                MonitoringPointEntity monitoringPoint = monitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(drNew["PointId"].ToString()));

                if (monitoringPoint != null)
                {
                    drNew["站点"] = monitoringPoint.MonitoringPointName;
                }
                if (ddlDataSource.SelectedValue == "AuditData")
                {
                    if (radlDataType.SelectedValue == "Day")
                    {
                        drNew["DateTime"] = string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(drNew["DateTime"].ToString()).ToString("yyyy-MM-dd"));
                    }
                    else if (radlDataType.SelectedValue == "Month" || radlDataType.SelectedValue == "Week")
                    {
                        drNew["DateTime"] = string.Format("{0:yyyy-MM-dd}", drNew["DateTime"].ToString());

                    }
                }
                else
                {
                    if (RadioButtonList1.SelectedValue == "OriDay")
                    {
                        //dtNew.Columns["DateTime"].DataType = typeof(string);
                        drNew["DateTime"] = string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(drNew["DateTime"].ToString()).ToString("yyyy-MM-dd"));
                    }
                    //else if (RadioButtonList1.SelectedValue == "OriMonth")
                    //{
                    //    drNew["DateTime"] = string.Format("{0:yyyy-MM-dd}", drNew["DateTime"].ToString());

                    //}
                }
                

            }

            for (int i = 0; i < dtNew.Columns.Count; i++)
            {
                DataColumn dcNew = dtNew.Columns[i];
                //追加日期
                if (dcNew.ColumnName == "Tstamp")
                {
                    string tstcolformat = "{0:MM-dd HH:mm}";
                    dcNew.ColumnName = "日期";
                }
                else if (dcNew.ColumnName == "DateTime")
                {
                    string tstcolformat = "{0:yyyy-MM-dd}";
                    dcNew.ColumnName = "日期";
                }
                else if (dcNew.ColumnName == "Year")
                {

                    dcNew.ColumnName = "年份";
                }
                else if (dcNew.ColumnName == "WeekOfYear")
                {

                    dcNew.ColumnName = "周";
                }
                else if (dcNew.ColumnName == "MonthOfYear")
                {

                    dcNew.ColumnName = "月份";
                }
                else if (dcNew.ColumnName == "SeasonOfYear")
                {

                    dcNew.ColumnName = "季";
                }
                //对因子列进行处理
                else if (isFactor(dcNew.ColumnName))
                {
                    IPollutant factor = factorCbxRsm.GetFactors().FirstOrDefault(x => x.PollutantCode.Equals(dcNew.ColumnName));
                    if (factor != null)
                    {
                        string name = factor.PollutantName;
                        string unit = factor.PollutantMeasureUnit;
                        dcNew.ColumnName = name + "(" + unit + ")";
                    }
                }

                else if (dcNew.ColumnName == "序号" || dcNew.ColumnName == "rows" || dcNew.ColumnName == "PointId" || dcNew.ColumnName.Contains("_Status") || dcNew.ColumnName.Contains("_DataFlag") || dcNew.ColumnName.Contains("_AuditFlag"))
                {
                    dtNew.Columns.Remove(dcNew);
                    i--;
                }
            }
            return dtNew;
        }
        /// <summary>
        /// 判断一个字符串是否为因子编码
        /// </summary>
        /// <param name="factorCode"></param>
        /// <returns></returns>
        public bool isFactor(string factorCode)
        {
            string reg = "^[A-Za-z]+[0-9]{4,7}$";
            if (Regex.IsMatch(factorCode, reg))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 把IQueryable转化为DataTable
        /// </summary>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public DataTable ConvertToDataTable(IEnumerable enumerable)
        {
            var dataTable = new DataTable();
            foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(typeof(ExcessiveSettingInfo)))
            {
                dataTable.Columns.Add(pd.Name);
            }
            foreach (ExcessiveSettingInfo item in enumerable)
            {
                var Row = dataTable.NewRow();

                foreach (PropertyDescriptor dp in TypeDescriptor.GetProperties(typeof(ExcessiveSettingInfo)))
                {
                    Row[dp.Name] = dp.GetValue(item);
                }
                dataTable.Rows.Add(Row);
            }
            return dataTable;
        }
        /// <summary>
        /// 数据行绑定处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridAudit_ItemDataBound(object sender, GridItemEventArgs e)
        {
            string DataTypeUid = "1b6367f1-5287-4c14-b120-7a35bd176db1";
            string[] portIds = pointCbxRsm.GetPointValues(CbxRsmReturnType.ID);
            string[] factorCodes = factorCbxRsm.GetFactorValues(SmartEP.Core.Enums.CbxRsmReturnType.Code);
            IQueryable<ExcessiveSettingInfo> Excessive = m_ExcessiveSettingService.RetrieveListByPointAndFactor(ApplicationValue.Air, factorCodes, portIds, DataTypeUid);
            DataTable dtExcessive = ConvertToDataTable(Excessive);
            GridDataItem item = e.Item as GridDataItem;
            DataRowView drv = e.Item.DataItem as DataRowView;
            if (e.Item is GridDataItem)
            {
                if (item["PointId"] != null)
                {
                    GridTableCell pointCell = (GridTableCell)item["PointId"];
                    IPoint point = points.FirstOrDefault(x => x.PointID.Equals(pointCell.Text.Trim()));
                    if (points != null)
                        pointCell.Text = point.PointName;
                }
                if ((RadioButtonList1.SelectedValue == "Min60" || RadioButtonList1.SelectedValue == "Min5" || RadioButtonList1.SelectedValue == "Min1") && ddlDataSource.SelectedValue == "OriData")
                {
                    for (int iRow = 0; iRow < factors.Count; iRow++)
                    {
                        string siteTypeName = "--";//标记位名称
                        IPollutant factor = factors[iRow];
                        GridTableCell cell = (GridTableCell)item[factor.PollutantCode];
                        string factorStatus = drv[factor.PollutantCode + "_Status"] != DBNull.Value ? drv[factor.PollutantCode + "_Status"].ToString() : string.Empty;
                        string factorDataFlag = drv[factor.PollutantCode + "_DataFlag"] != DBNull.Value ? drv[factor.PollutantCode + "_DataFlag"].ToString() : string.Empty;
                        string factorAuditFlag = drv[factor.PollutantCode + "_AuditFlag"] != DBNull.Value ? drv[factor.PollutantCode + "_AuditFlag"].ToString() : string.Empty;
                        decimal value = 0M;
                        if (decimal.TryParse(cell.Text, out value))
                        {
                            if (factor.PollutantMeasureUnit == "μg/m³" && !LZSPfactor.Contains(factor.PollutantCode))
                            {
                                value = DecimalExtension.GetPollutantValue(value * 1000, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 0 : Convert.ToInt32(factor.PollutantDecimalNum) - 3);
                            }
                            else
                            {
                                value = DecimalExtension.GetPollutantValue(value, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum));
                            }
                            cell.Text = value.ToString("");
                            if (((factorStatus != "N" && factorStatus != "MF" && factorStatus != "NULL" && !string.IsNullOrEmpty(factorStatus)) || (factorDataFlag != "N" && factorDataFlag != "MF" && factorDataFlag != "NULL" && !string.IsNullOrEmpty(factorDataFlag)) || (factorAuditFlag != "N" && factorAuditFlag != "MF" && factorAuditFlag != "NULL" && !string.IsNullOrEmpty(factorAuditFlag))))
                            {
                                string markContent = string.Empty;
                                if ((factorStatus != "N" && factorStatus != "MF" && factorStatus != "NULL" && !string.IsNullOrEmpty(factorStatus)) || (factorDataFlag != "N" && factorDataFlag != "MF" && factorDataFlag != "NULL" && !string.IsNullOrEmpty(factorDataFlag)) || (factorAuditFlag != "N" && factorAuditFlag != "MF" && factorAuditFlag != "NULL" && !string.IsNullOrEmpty(factorAuditFlag)))
                                {
                                    markContent += factorStatus.Replace("、", ",") + factorDataFlag + factorAuditFlag + "|";
                                    //暂时代码里转换成<>显示
                                    //若存在 “>”，则标记为 “D”
                                    //若存在 “<”，则标记为 “d”
                                    markContent = markContent.Replace("d", "<").Replace("D", ">");
                                    siteTypeName = factorStatus.Replace("d", "<").Replace("D", ">").Replace("、", ",") + factorDataFlag + factorAuditFlag + "(" + siteTypeEntites.Where(t => t.ItemValue.Equals(factorStatus))
                    .Select(t => t.ItemText).FirstOrDefault() + siteTypeEntites.Where(t => t.ItemValue.Equals(factorDataFlag))
                    .Select(t => t.ItemText).FirstOrDefault() + siteTypeEntites.Where(t => t.ItemValue.Equals(factorAuditFlag))
                    .Select(t => t.ItemText).FirstOrDefault() + ")" + "|";//标记位名称
                                }
                                markContent = markContent.TrimEnd('|');
                                cell.Text = cell.Text + "(" + markContent + ")";
                                cell.ForeColor = System.Drawing.Color.Red;
                                cell.Font.Bold = true;
                            }
                            if (dtExcessive.DefaultView.Count > 0)
                            {
                                if (item["PointId"] != null)
                                {
                                    GridTableCell pointCell = (GridTableCell)item["PointId"];
                                    IPoint point = points.FirstOrDefault(x => x.PointName.Equals(pointCell.Text.Trim()));
                                    DataRow[] drExcessive = dtExcessive.Select("PointID='" + point.PointID + "' and PollutantCode='" + factor.PollutantCode + "'");
                                    if (drExcessive.Count() > 0)
                                    {
                                        cell.ToolTip = "上限：" + drExcessive[0]["excessiveUpper"] + "\n下限：" + drExcessive[0]["excessiveLow"] + "\n标记位：" + siteTypeName.TrimEnd('|');
                                    }
                                    else
                                    {
                                        cell.ToolTip = "上限：-- \n下限：--" + "\n标记位：" + siteTypeName.TrimEnd('|');
                                    }
                                }
                            }
                            else
                            {
                                cell.ToolTip = "上限：--\n下限：--";
                            }
                        }
                    }
                }
                else if (radlDataType.SelectedValue == "Hour" && ddlDataSource.SelectedValue == "AuditData")
                {
                    for (int iRow = 0; iRow < factors.Count; iRow++)
                    {
                        string siteTypeName = "--";//标记位名称
                        IPollutant factor = factors[iRow];
                        GridTableCell cell = (GridTableCell)item[factor.PollutantCode];
                        string factorStatus = drv[factor.PollutantCode + "_Status"] != DBNull.Value ? drv[factor.PollutantCode + "_Status"].ToString() : string.Empty;
                        //string factorDataFlag = drv[factor.PollutantCode + "_DataFlag"] != DBNull.Value ? drv[factor.PollutantCode + "_DataFlag"].ToString() : string.Empty;
                        //string factorAuditFlag = drv[factor.PollutantCode + "_AuditFlag"] != DBNull.Value ? drv[factor.PollutantCode + "_AuditFlag"].ToString() : string.Empty;
                        decimal value = 0M;
                        if (decimal.TryParse(cell.Text, out value))
                        {
                            if (factor.PollutantMeasureUnit == "μg/m³" && !LZSPfactor.Contains(factor.PollutantCode))
                            {
                                value = DecimalExtension.GetPollutantValue(value * 1000, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 0 : Convert.ToInt32(factor.PollutantDecimalNum) - 3);
                            }
                            else
                            {
                                value = DecimalExtension.GetPollutantValue(value, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum));
                            }
                            cell.Text = value.ToString("");
                            if (((factorStatus != "N" && factorStatus != "MF" && !string.IsNullOrEmpty(factorStatus))))
                            {
                                string markContent = string.Empty;
                                if ((factorStatus != "N" && factorStatus != "MF" && !string.IsNullOrEmpty(factorStatus)))
                                {
                                    markContent += factorStatus.Replace("、", ",") + "|";
                                    siteTypeName = factorStatus.Replace("、", ",") + "(" + siteTypeEntites.Where(t => t.ItemValue.Equals(factorStatus))
                    .Select(t => t.ItemText).FirstOrDefault() + ")" + "|";//标记位名称
                                }
                                markContent = markContent.TrimEnd('|');
                                cell.Text = cell.Text + "(" + markContent + ")";
                                cell.ForeColor = System.Drawing.Color.Red;
                                cell.Font.Bold = true;
                            }
                            if (dtExcessive.DefaultView.Count > 0)
                            {
                                if (item["PointId"] != null)
                                {
                                    GridTableCell pointCell = (GridTableCell)item["PointId"];
                                    IPoint point = points.FirstOrDefault(x => x.PointName.Equals(pointCell.Text.Trim()));
                                    DataRow[] drExcessive = dtExcessive.Select("PointID='" + point.PointID + "' and PollutantCode='" + factor.PollutantCode + "'");
                                    if (drExcessive.Count() > 0)
                                    {
                                        cell.ToolTip = "上限：" + drExcessive[0]["excessiveUpper"] + "\n下限：" + drExcessive[0]["excessiveLow"] + "\n标记位：" + siteTypeName.TrimEnd('|');
                                    }
                                    else
                                    {
                                        cell.ToolTip = "上限：-- \n下限：--" + "\n标记位：" + siteTypeName.TrimEnd('|');
                                    }
                                }
                            }
                            else
                            {
                                cell.ToolTip = "上限：--\n下限：--";
                            }
                        }
                    }
                }
                else
                {
                    for (int iRow = 0; iRow < factors.Count; iRow++)
                    {
                        IPollutant factor = factors[iRow];
                        GridTableCell cell = (GridTableCell)item[factor.PollutantCode];
                        decimal value = 0M;
                        if (decimal.TryParse(cell.Text, out value))
                        {
                            if (factor.PollutantMeasureUnit == "μg/m³" && !LZSPfactor.Contains(factor.PollutantCode))
                            {
                                value = DecimalExtension.GetPollutantValue(value * 1000, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 0 : Convert.ToInt32(factor.PollutantDecimalNum) - 3);
                            }
                            else
                            {
                                value = DecimalExtension.GetPollutantValue(value, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum));
                            }

                            //value = DecimalExtension.GetPollutantValue(value, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum));
                            cell.Text = value.ToString("");

                            if (dtExcessive.DefaultView.Count > 0)
                            {
                                if (item["PointId"] != null)
                                {
                                    GridTableCell pointCell = (GridTableCell)item["PointId"];
                                    IPoint point = points.FirstOrDefault(x => x.PointName.Equals(pointCell.Text.Trim()));
                                    DataRow[] drExcessive = dtExcessive.Select("PointID='" + point.PointID + "' and PollutantCode='" + factor.PollutantCode + "'");
                                    if (drExcessive.Count() > 0)
                                    {
                                        cell.ToolTip = "上限：" + drExcessive[0]["excessiveUpper"] + "\n下限：" + drExcessive[0]["excessiveLow"];
                                    }
                                    else
                                    {
                                        cell.ToolTip = "上限：--\n下限：--";
                                    }
                                }
                            }
                        }
                    }
                }

            }
        }
        /// <summary>
        /// 生成RadGrid的绑定列
        /// </summary>
        /// <param sender></param>
        /// <param e></param>
        protected void gridAudit_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            try
            {
                if (e.Column.ColumnType.Equals("GridExpandColumn"))
                    return;
                //追加站点
                GridBoundColumn col = (GridBoundColumn)e.Column;
                string a = col.DataField;
                if (col.DataField == "PointId")
                {
                    col.HeaderText = "站点";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterText = "平均值<br>最大值<br>最小值";
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                //追加日期
                else if (col.DataField == "Tstamp")
                {
                    col = (GridDateTimeColumn)e.Column;
                    string tstcolformat = "{0:MM-dd HH:mm}";
                    if (SmartEP.Core.Enums.EnumMapping.ParseEnum<PollutantDataType>(radlDataType.SelectedValue) == PollutantDataType.Hour)
                        tstcolformat = "{0:MM-dd HH:mm}";
                    col.HeaderText = "日期";
                    col.DataFormatString = tstcolformat;
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(120);
                    col.ItemStyle.Width = Unit.Pixel(120);
                }
                else if (col.DataField == "DateTime")
                {
                    col = (GridDateTimeColumn)e.Column;
                    string tstcolformat = "{0:yyyy-MM-dd}";
                    if (SmartEP.Core.Enums.EnumMapping.ParseEnum<PollutantDataType>(radlDataType.SelectedValue) == PollutantDataType.Day)
                        tstcolformat = "{0:yyyy-MM-dd}";
                    col.HeaderText = "日期";
                    col.DataFormatString = tstcolformat;
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(120);
                    col.ItemStyle.Width = Unit.Pixel(120);
                }
                else if (col.DataField == "Year")
                {
                    col = (GridNumericColumn)e.Column;
                    col.HeaderText = "年份";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(120);
                    col.ItemStyle.Width = Unit.Pixel(120);
                }
                else if (col.DataField == "WeekOfYear")
                {
                    col = (GridNumericColumn)e.Column;
                    col.HeaderText = "周";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(120);
                    col.ItemStyle.Width = Unit.Pixel(120);
                }
                else if (col.DataField == "MonthOfYear")
                {
                    col = (GridNumericColumn)e.Column;
                    col.HeaderText = "月份";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(120);
                    col.ItemStyle.Width = Unit.Pixel(120);
                }
                else if (col.DataField == "SeasonOfYear")
                {
                    col = (GridNumericColumn)e.Column;
                    col.HeaderText = "季";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(120);
                    col.ItemStyle.Width = Unit.Pixel(120);
                }

                //追加因子
                //else if (cbFactor.CheckedItems.Select(t => t.Value).Contains(col.DataField))//(factors.Select(x => x.PollutantCode).Contains(col.DataField))
                else if (factors.Select(x => x.PollutantCode).Contains(col.DataField))
                {
                    string unit = "";
                    int radGridColWidthValue = int.Parse(radGridColWidth.Value.ToString());
                    IPollutant iFactorCode = m_AirPollutantService.GetPollutantInfo(col.DataField);
                    if (iFactorCode != null)
                    {
                        unit = iFactorCode.PollutantMeasureUnit;
                        if (unit == "ng/m³")
                        {
                            unit = "μg/m³";
                        }
                    }
                    IPollutant factor = factors.FirstOrDefault(x => x.PollutantCode.Equals(col.DataField));
                    col.HeaderText = string.Format("{0}<br>({1})", factor.PollutantName, factor.PollutantMeasureUnit);
                    //col.HeaderText = string.Format("{0}<br>({1})", cbFactor.Items.Where(t => t.Value == col.DataField).Select(t => t.Text).FirstOrDefault(), unit);
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(radGridColWidthValue);
                    col.ItemStyle.Width = Unit.Pixel(radGridColWidthValue);
                    SetGridFooterText(col);
                }
                else if (col.DataField == "blankspaceColumn") { }
                else
                {
                    e.Column.Visible = false;
                }

            }
            catch (Exception ex) { }
        }
        /// <summary>
        /// 平均大小值
        /// </summary>
        /// <param name="col"></param>
        public void SetGridFooterText(GridBoundColumn col)
        {

        }

        /// <summary>
        /// 获取数据源 日数据
        /// </summary>
        public void GetDataTable(string columnName, DataTable auDT, int type, int DecimalNum)
        {
            if (type == 0)
            {
                DataTable InstrumenTotalDt = (DataTable)this.ViewState["dt"];


                if (InstrumenTotalDt.DefaultView.Count > 0)//该表已经有数据了 修改数据 否则就是填充该表
                {
                    foreach (DataRow dr in auDT.Rows)
                    {
                        //IPollutant iFactorCode = m_AirPollutantService.GetPollutantInfo(factorCode);

                        decimal wanValue = 0;
                        foreach (DataColumn dc in auDT.Columns)
                        {
                            if (dc.ColumnName != "PointId" && dc.ColumnName != "Tstamp")
                            {
                                wanValue += dr[dc.ColumnName] != DBNull.Value ? DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[dc.ColumnName]), DecimalNum) : 0;
                            }
                        }

                        DataRow[] drs = InstrumenTotalDt.Select(string.Format("Tstamp='{0}' and PointId={1}", dr["Tstamp"], dr["PointId"]));

                        if (drs.Length > 0)
                        {
                            if (wanValue == 0)
                            {
                                drs[0][columnName] = "";
                            }
                            else
                            {
                                drs[0][columnName] = wanValue.ToString();
                            }
                        }

                    }
                }
                else
                {
                    foreach (DataRow dr in auDT.Rows)
                    {
                        decimal wanValue = 0;
                        DataRow odr = InstrumenTotalDt.NewRow();

                        odr["PointId"] = dr["PointId"];
                        odr["Tstamp"] = dr["Tstamp"];
                        foreach (DataColumn dc in auDT.Columns)
                        {
                            if (dc.ColumnName != "PointId" && dc.ColumnName != "Tstamp")
                            {
                                wanValue += dr[dc.ColumnName] != DBNull.Value ? DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[dc.ColumnName]), DecimalNum) : 0;
                            }
                        }
                        if (wanValue == 0)
                        {
                            odr[columnName] = "";
                        }
                        else
                        {
                            odr[columnName] = wanValue.ToString();
                        }
                        InstrumenTotalDt.Rows.Add(odr);
                        InstrumenTotalDt.AcceptChanges();
                    }
                }
            }
            else
            {
                DataTable InstrumenTotalDt = (DataTable)this.ViewState["dtIcon"];

                if (InstrumenTotalDt.DefaultView.Count > 0)//该表已经有数据了 修改数据 否则就是填充该表
                {
                    foreach (DataRow dr in auDT.Rows)
                    {
                        decimal wanValue = 0;
                        int m = 0;
                        foreach (DataColumn dc in auDT.Columns)
                        {
                            if (dc.ColumnName != "PointId" && dc.ColumnName != "Tstamp")
                            {
                                if (dr[dc.ColumnName] != DBNull.Value)
                                {
                                    wanValue += dr[dc.ColumnName] != DBNull.Value ? DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[dc.ColumnName]), DecimalNum) : 0;
                                    m++;
                                }
                            }
                        }

                        DataRow[] drs = InstrumenTotalDt.Select(string.Format("Tstamp='{0}' and PointId={1}", dr["Tstamp"], dr["PointId"]));

                        if (drs.Length > 0)
                        {
                            if (wanValue != 0 || m != 0)
                            {
                                drs[0][columnName] = wanValue;
                            }

                        }

                    }
                }
                else
                {
                    foreach (DataRow dr in auDT.Rows)
                    {
                        decimal wanValue = 0;
                        int m = 0;
                        DataRow odr = InstrumenTotalDt.NewRow();

                        odr["PointId"] = dr["PointId"];
                        odr["Tstamp"] = dr["Tstamp"];
                        foreach (DataColumn dc in auDT.Columns)
                        {
                            if (dc.ColumnName != "PointId" && dc.ColumnName != "Tstamp")
                            {
                                if (dr[dc.ColumnName] != DBNull.Value)
                                {
                                    wanValue += dr[dc.ColumnName] != DBNull.Value ? DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[dc.ColumnName]), DecimalNum) : 0;
                                    m++;
                                }
                            }
                        }
                        if (wanValue != 0 || m != 0)
                        {
                            odr[columnName] = wanValue;
                        }
                        InstrumenTotalDt.Rows.Add(odr);
                        InstrumenTotalDt.AcceptChanges();
                    }
                }
            }
        }
        /// <summary>
        /// 获取数据源 周数据
        /// </summary>
        public DataTable GetTable(DataView dv, string[] typeName, Dictionary<string, string[]> factorCode, int Ode, string timeType)
        {
            DataTable dtNew = new DataTable();
            dtNew.Columns.Add("PointId", typeof(System.String));//污染物名称 参数
            foreach (string colName in typeName)
            {
                dtNew.Columns.Add(colName, typeof(System.String));//污染物值                           
            }
            dtNew.Columns.Add("Tstamp", typeof(System.DateTime));//时间戳
            for (int i = 0; i < dv.Count; i++)
            {
                DataRow drNew = dtNew.NewRow();
                drNew["PointId"] = dv[i]["PointId"].ToString();
                DateTime dtTemp = DateTime.Now;
                if (timeType == "Day")
                {
                    dtTemp = DateTime.TryParse(dv[i]["Tstamp"].ToString(), out dtTemp) ? dtTemp : DateTime.Now;
                }
                else
                {
                    dtTemp = DateTime.TryParse(dv[i]["DateTime"].ToString(), out dtTemp) ? dtTemp : DateTime.Now;
                }
                drNew["Tstamp"] = dtTemp;
                foreach (string strItem in typeName)
                {
                    string[] strTemp = factorCode[strItem];
                    decimal AlkaneValue = 0;
                    int m = 0;
                    foreach (string strCode in strTemp)
                    {
                        if (dv.ToTable().Columns.Contains(strCode))
                        {
                            if (dv[i][strCode] != DBNull.Value)
                            {
                                m++;
                                AlkaneValue += DecimalExtension.GetPollutantValue(Convert.ToDecimal(dv[i][strCode]), Ode);
                            }
                        }
                    }
                    if (AlkaneValue != 0 || m != 0)
                    {
                        drNew[strItem] = AlkaneValue;
                    }
                }
                dtNew.Rows.Add(drNew);
            }
            return dtNew;
        }
        /// <summary>
        /// 获取数据源 季数据
        /// </summary>
        public DataTable GetTable1(DataView dv, string[] typeName, Dictionary<string, string[]> factorCode, int Ode, string timeType)
        {
            DataTable dtNew = new DataTable();
            dtNew.Columns.Add("PointId", typeof(System.String));//污染物名称 参数
            foreach (string colName in typeName)
            {
                dtNew.Columns.Add(colName, typeof(System.Decimal));//污染物值                           
            }
            dtNew.Columns.Add("Tstamp", typeof(System.DateTime));//时间戳
            for (int i = 0; i < dv.Count; i++)
            {
                DataRow drNew = dtNew.NewRow();
                drNew["PointId"] = dv[i]["PointId"].ToString();
                DateTime dtTemp = DateTime.Now;
                if (timeType == "Day")
                {
                    dtTemp = DateTime.TryParse(dv[i]["Tstamp"].ToString(), out dtTemp) ? dtTemp : DateTime.Now;
                }
                else
                {
                    dtTemp = DateTime.TryParse(dv[i]["DateTime"].ToString(), out dtTemp) ? dtTemp : DateTime.Now;
                }
                drNew["Tstamp"] = dtTemp;
                foreach (string strItem in typeName)
                {
                    string[] strTemp = factorCode[strItem];
                    decimal AlkaneValue = 0;
                    int m = 0;
                    foreach (string strCode in strTemp)
                    {
                        if (dv.ToTable().Columns.Contains(strCode))
                        {
                            if (dv[i][strCode] != DBNull.Value)
                            {
                                m++;
                                AlkaneValue += DecimalExtension.GetPollutantValue(Convert.ToDecimal(dv[i][strCode]), Ode);
                            }
                        }
                    }
                    if (AlkaneValue != 0 || m != 0)
                    {
                        drNew[strItem] = AlkaneValue;
                    }
                }
                dtNew.Rows.Add(drNew);
            }
            return dtNew;
        }

        /// <summary>
        /// 获取数据源 月数据
        /// </summary>
        public void GetMonthDataTable(string ColumnName, DataTable auDT, int type, int DecimalNum)
        {
            if (type == 0)
            {
                DataTable InstrumenTotalDt = (DataTable)this.ViewState["dtMonth"];



                if (InstrumenTotalDt.DefaultView.Count > 0)//该表已经有数据了 修改数据 否则就是填充该表
                {
                    foreach (DataRow dr in auDT.Rows)
                    {
                        decimal wanValue = 0;
                        foreach (DataColumn dc in auDT.Columns)
                        {
                            if (dc.ColumnName != "PointId" && dc.ColumnName != "DateTime")
                            {
                                wanValue += dr[dc.ColumnName] != DBNull.Value ? DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[dc.ColumnName]), DecimalNum) : 0;
                            }
                        }

                        DataRow[] drs = InstrumenTotalDt.Select(string.Format("Tstamp='{0}'", dr["DateTime"]));

                        if (drs.Length > 0)
                        {
                            if (wanValue == 0)
                            {
                                drs[0][ColumnName] = "";

                            }
                            else
                            {
                                drs[0][ColumnName] = wanValue.ToString();
                            }
                        }

                    }
                }
                else
                {
                    foreach (DataRow dr in auDT.Rows)
                    {
                        decimal wanValue = 0;
                        DataRow odr = InstrumenTotalDt.NewRow();

                        odr["PointId"] = dr["PointId"];
                        odr["Tstamp"] = dr["DateTime"];
                        foreach (DataColumn dc in auDT.Columns)
                        {
                            if (dc.ColumnName != "PointId" && dc.ColumnName != "DateTime")
                            {
                                wanValue += dr[dc.ColumnName] != DBNull.Value ? DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[dc.ColumnName]), DecimalNum) : 0;
                            }
                        }
                        if (wanValue == 0)
                        {
                            odr[ColumnName] = "";

                        }
                        else
                        {
                            odr[ColumnName] = wanValue.ToString();
                        }
                        InstrumenTotalDt.Rows.Add(odr);
                        InstrumenTotalDt.AcceptChanges();
                    }
                }
            }
            else
            {
                DataTable InstrumenTotalDt = (DataTable)this.ViewState["dtMonthIcon"];


                if (InstrumenTotalDt.DefaultView.Count > 0)//该表已经有数据了 修改数据 否则就是填充该表
                {
                    foreach (DataRow dr in auDT.Rows)
                    {
                        decimal wanValue = 0;
                        int m = 0;
                        foreach (DataColumn dc in auDT.Columns)
                        {
                            if (dc.ColumnName != "PointId" && dc.ColumnName != "DateTime")
                            {
                                if (dr[dc.ColumnName] != DBNull.Value)
                                {
                                    wanValue += dr[dc.ColumnName] != DBNull.Value ? DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[dc.ColumnName]), DecimalNum) : 0;
                                    m++;
                                }
                            }
                        }

                        DataRow[] drs = InstrumenTotalDt.Select(string.Format("Tstamp='{0}'", dr["DateTime"]));

                        if (drs.Length > 0)
                        {
                            if (wanValue != 0 || m != 0)
                            {
                                drs[0][ColumnName] = wanValue;
                            }
                        }

                    }
                }
                else
                {
                    foreach (DataRow dr in auDT.Rows)
                    {
                        decimal wanValue = 0;
                        int m = 0;
                        DataRow odr = InstrumenTotalDt.NewRow();

                        odr["PointId"] = dr["PointId"];
                        odr["Tstamp"] = dr["DateTime"];
                        foreach (DataColumn dc in auDT.Columns)
                        {
                            if (dc.ColumnName != "PointId" && dc.ColumnName != "DateTime")
                            {
                                if (dr[dc.ColumnName] != DBNull.Value)
                                {
                                    wanValue += dr[dc.ColumnName] != DBNull.Value ? DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[dc.ColumnName]), DecimalNum) : 0;
                                }
                            }
                        }
                        if (wanValue != 0 || m != 0)
                        {
                            odr[ColumnName] = wanValue;
                        }
                        InstrumenTotalDt.Rows.Add(odr);
                        InstrumenTotalDt.AcceptChanges();
                    }
                }
            }
        }
        /// <summary>
        /// 页面隐藏域控件赋值（小时、日），将数据需要的参数放入隐藏域，各个参数间用‘|’分割，每个参数内部用‘；’分割
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtBegin"></param>
        /// <param name="dtEnd"></param>
        private void SetHiddenData(string[] portIds, IList<IPollutant> factors, DateTime dtBegin, DateTime dtEnd)
        {
            if (ddlDataSource.SelectedValue == "OriData")
            {
                HiddenData.Value = string.Join(";", portIds) + "|" + string.Join(";", factors.Select(p => p.PollutantCode).ToArray())
                                 + "|" + dtBegin + "|" + dtEnd + "|" + RadioButtonList1.SelectedValue + "|Air";
                HiddenChartType.Value = ChartType.SelectedValue;
            }
            if (ddlDataSource.SelectedValue == "AuditData")
            {
                HiddenData.Value = string.Join(";", portIds) + "|" + string.Join(";", factors.Select(p => p.PollutantCode).ToArray())
                                 + "|" + dtBegin + "|" + dtEnd + "|" + radlDataType.SelectedValue + "|Air";
                HiddenChartType.Value = ChartType.SelectedValue;
            }
        }

        /// <summary>
        /// 页面隐藏域控件赋值,(周、月、季、年)
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="timeStr"></param>
        private void SetHiddenData(string[] portIds, IList<IPollutant> factors, string timeStr)
        {
            if (ddlDataSource.SelectedValue == "OriData")
            {
                HiddenData.Value = string.Join(";", portIds) + "|" + string.Join(";", factors.Select(p => p.PollutantCode).ToArray())
                                 + "|" + timeStr + "|" + "|" + RadioButtonList1.SelectedValue + "|Air";
                HiddenChartType.Value = ChartType.SelectedValue;
            }
            if (ddlDataSource.SelectedValue == "AuditData")
            {
                HiddenData.Value = string.Join(";", portIds) + "|" + string.Join(";", factors.Select(p => p.PollutantCode).ToArray())
                                 + "|" + timeStr + "|" + "|" + radlDataType.SelectedValue + "|Air";
                HiddenChartType.Value = ChartType.SelectedValue;
            }
        }
        /// <summary>
        /// 图表按站点（因子）显示切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PointFactor_SelectedIndexChanged(object sender, EventArgs e)
        {
            HiddenPointFactor.Value = PointFactor.SelectedValue;
            RegisterScript("PointFactor('" + PointFactor.SelectedValue + "');");
        }
        /// <summary>
        /// 折线图曲线图切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ChartType_SelectedIndexChanged(object sender, EventArgs e)
        {
            HiddenChartType.Value = ChartType.SelectedValue;
            RegisterScript("ChartTypeChanged('" + ChartType.SelectedValue + "');");
        }
    }
}