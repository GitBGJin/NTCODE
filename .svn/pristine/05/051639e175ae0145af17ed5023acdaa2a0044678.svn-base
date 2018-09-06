using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.AutoMonitoring.Air;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.DataAnalyze.Air.DataQuery;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.WebControl.CbxRsm;
using SmartEP.WebUI.Common;
using SmartEP.WebUI.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.DataAnalyze
{
    /// <summary>
    /// 名称：DifferentDayCompare.aspx.cs
    /// 创建人：刘长敏
    /// 创建日期：2015-08-23
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：数据比对分析(相同时段多个站点多个参数、可变时段多个站点同一参数、相同时段同一站点同一参数、相同时段同一站点多个参数)
    /// 虚拟分页
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class DifferentDayCompare : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        DataCompare m_CompareData = Singleton<DataCompare>.GetInstance();
        /// <summary>
        /// 选择因子
        /// </summary>
        private IList<IPollutant> factors = null;
        /// <summary>
        /// 选择站点
        /// </summary>
        private IList<IPoint> points = null;
        /// <summary>
        /// 统计
        /// </summary>
        DataView dvStatistical = null;

        protected void Page_Load(object sender, EventArgs e)
        {

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
            //国控点，对照点，路边站
            MonitoringPointAirService m_MonitoringPointAirService = Singleton<MonitoringPointAirService>.GetInstance();
            string strpointName = "";
            IQueryable<MonitoringPointEntity> EnableOrNotports = m_MonitoringPointAirService.RetrieveAirMPListByEnable();
            string[] EnableOrNotportsarry = EnableOrNotports.Where(p => p.ContrlUid == "6fadff52-2338-4319-9f1d-7317823770ad" || p.ContrlUid == "bdf0837a-eb59-4c4a-a05f-c774a17f3077" || p.ContrlUid == "c1158eb6-4d69-4846-a963-d16b9d2794ca").Select(p => p.MonitoringPointName).ToArray();
            foreach (string point in EnableOrNotportsarry)
            {
                strpointName += point + ";";
            }
            pointCbxRsm.SetPointValuesFromNames(strpointName);
            ////因子
            //SmartEP.Service.BaseData.Channel.AirPollutantService m_AirPollutantService = Singleton<SmartEP.Service.BaseData.Channel.AirPollutantService>.GetInstance();
            //IQueryable<PollutantCodeEntity> Pollutant = m_AirPollutantService.RetrieveListByCalAQI();
            //string PollutantName = "";
            //string[] pollutantarry = Pollutant.Select(p => p.PollutantName).ToArray();
            //foreach (string strName in pollutantarry)
            //{
            //    PollutantName += strName + ";";
            //}
            //factorCbxRsm.SetFactorValuesFromNames(PollutantName);
            string pollutantName = System.Configuration.ConfigurationManager.AppSettings["AirPollutant"];
            factorCbxRsm.SetFactorValuesFromNames(pollutantName);

            //相同时段时间初始化      
            hourBegin.SelectedDate = DateTime.Now.AddHours(-1);
            hourEnd.SelectedDate = DateTime.Now;
            dayBegin.SelectedDate = DateTime.Now.AddDays(-1);
            dayEnd.SelectedDate = DateTime.Now;
            monthBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToString("yyyy-MM"));
            monthEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"));
            seasonBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"));
            seasonEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"));
            weekBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToString("yyyy-MM"));
            weekEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"));
            int monthDay = DateTime.DaysInMonth(weekBegin.SelectedDate.Value.Year, weekBegin.SelectedDate.Value.Month);
            if (monthDay % 7 > 0)
            {
                int week = monthDay / 7 + 1;
                for (int i = 0; i < week; i++)
                {
                    string j = Convert.ToString(i + 1);
                    weekFrom.Items.Add(new DropDownListItem(j, j));
                }
            }
            else
            {
                int week = monthDay / 7;
                for (int i = 0; i < week; i++)
                {
                    string j = Convert.ToString(i + 1);
                    weekFrom.Items.Add(new DropDownListItem(j, j));
                }
            }
            weekFrom.SelectedText = "1";
            int monthDt = DateTime.DaysInMonth(weekEnd.SelectedDate.Value.Year, weekEnd.SelectedDate.Value.Month);
            if (monthDt % 7 > 0)
            {
                int week = monthDt / 7 + 1;
                for (int i = 0; i < week; i++)
                {
                    string j = Convert.ToString(i + 1);
                    weekTo.Items.Add(new DropDownListItem(j, j));
                }
            }
            else
            {
                int week = monthDt / 7;
                for (int i = 0; i < week; i++)
                {
                    string j = Convert.ToString(i + 1);
                    weekTo.Items.Add(new DropDownListItem(j, j));
                }
            }
            weekTo.SelectedText = "4";

            yearBegin.SelectedDate = DateTime.Now.AddYears(-1);
            yearEnd.SelectedDate = DateTime.Now;

            int weekfrom = Convert.ToInt32(weekFrom.SelectedText);
            int weekto = Convert.ToInt32(weekTo.SelectedValue);
            var week1 = weekBegin.SelectedDate.Value.DayOfWeek;
            var week2 = weekEnd.SelectedDate.Value.DayOfWeek;
            DateTime DateB1 = weekBegin.SelectedDate.Value;
            DateTime DateB2 = DateB1.AddDays(7 * (weekfrom - 1));
            DateTime dateB3 = DateB2.AddDays(0 - (int)week1);
            //    txtweekF.Text = dateB3.ToString("yyyy年MM月dd日");
            DateTime DateE1 = weekBegin.SelectedDate.Value;
            DateTime DateE2 = DateE1.AddDays(7 * (weekto - 1));
            DateTime dateE3 = DateE2.AddDays(0 - (int)week2 + 6);
            //     txtweekT.Text = dateE3.ToString("yyyy年MM月dd日");
            //不同时段比对时间初始化
            rdtpHourFrom.SelectedDate = DateTime.Now.AddHours(-1);
            rdtpHourTo.SelectedDate = DateTime.Now;
            rdpDayFrom.SelectedDate = DateTime.Now.AddDays(-1);
            rdpDayTo.SelectedDate = DateTime.Now;
            rmypMonthFrom.SelectedDate = Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToString("yyyy-MM"));
            rmypMonthTo.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"));
            rmypSeasonFrom.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"));
            rmypSeasonTo.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"));
            rmypYearFrom.SelectedDate = DateTime.Now.AddYears(-1);
            rmypYearTo.SelectedDate = DateTime.Now;
            rmypWeekFrom.SelectedDate = Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToString("yyyy-MM"));
            rmypWeekTo.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"));
            int ddlMonthDay = DateTime.DaysInMonth(rmypWeekFrom.SelectedDate.Value.Year, rmypWeekFrom.SelectedDate.Value.Month);
            if (ddlMonthDay % 7 > 0)
            {
                int week = ddlMonthDay / 7 + 1;
                for (int i = 0; i < week; i++)
                {
                    string j = Convert.ToString(i + 1);
                    ddlWeekFrom.Items.Add(new DropDownListItem(j, j));
                }
            }
            else
            {
                int week = ddlMonthDay / 7;
                for (int i = 0; i < week; i++)
                {
                    string j = Convert.ToString(i + 1);
                    ddlWeekFrom.Items.Add(new DropDownListItem(j, j));
                }
            }
            ddlWeekFrom.SelectedText = "1";
            int ddlMonthDt = DateTime.DaysInMonth(rmypWeekTo.SelectedDate.Value.Year, rmypWeekTo.SelectedDate.Value.Month);
            if (ddlMonthDt % 7 > 0)
            {
                int week = ddlMonthDt / 7 + 1;
                for (int i = 0; i < week; i++)
                {
                    string j = Convert.ToString(i + 1);
                    ddlWeekTo.Items.Add(new DropDownListItem(j, j));
                }
            }
            else
            {
                int week = ddlMonthDt / 7;
                for (int i = 0; i < week; i++)
                {
                    string j = Convert.ToString(i + 1);
                    ddlWeekTo.Items.Add(new DropDownListItem(j, j));
                }
            }
            ddlWeekTo.SelectedText = "4";

            //数据类型
            radlDataType.Items.Add(new ListItem("小时数据", PollutantDataType.Hour.ToString()));
            radlDataType.Items.Add(new ListItem("日数据", PollutantDataType.Day.ToString()));
            radlDataType.Items.Add(new ListItem("周数据", PollutantDataType.Week.ToString()));
            radlDataType.Items.Add(new ListItem("月数据", PollutantDataType.Month.ToString()));
            radlDataType.Items.Add(new ListItem("季数据", PollutantDataType.Season.ToString()));
            radlDataType.Items.Add(new ListItem("年数据", PollutantDataType.Year.ToString()));
            radlDataType.SelectedValue = PollutantDataType.Hour.ToString();
            divHour.Visible = true;
            //Tab切换初始绑定
            tabStrip.SelectedIndex = 0;
            divSame.Visible = true;
            //paneWhere.Height = 60;

        }
        #endregion

        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {
            if (!IsPostBack)
            {
                pointCbxRsm_SelectedChanged();
            }
            try
            {
                string[] factorCodes = factorCbxRsm.GetFactorValues(SmartEP.Core.Enums.CbxRsmReturnType.Code);

                DateTime dtBegin = DateTime.Now;
                DateTime dtEnd = DateTime.Now;
                DateTime dtFrom = DateTime.Now;
                DateTime dtTo = DateTime.Now;
                points = pointCbxRsm.GetPoints();
                string[] portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
                factors = factorCbxRsm.GetFactors();

                //生成RadGrid的绑定列
                dvStatistical = null;
                //每页显示数据个数            
                int pageSize = gridDataCompare.PageSize;
                //当前页的序号
                int pageNo = gridDataCompare.CurrentPageIndex;

                //数据总行数
                int recordTotal = 0;
                #region   相同时段多站点多参数
                if (tabStrip.SelectedIndex == 0)
                {
                    int tabIndex = tabStrip.SelectedIndex;

                    //是否显示原始数据
                    if (OriginalData.Checked)
                    {
                        if (portIds != null)
                        {
                            string orderby = "PointId asc,Tstamp desc";
                            dtBegin = hourBegin.SelectedDate.Value;
                            dtEnd = hourEnd.SelectedDate.Value;
                            //【给隐藏域赋值，用于显示Chart】
                            SetHiddenData(portIds, factors, dtBegin.ToString() + ";" + dtEnd.ToString() + ";" + dtFrom.ToString() + ";" + dtTo.ToString());
                            var dataCompare = m_CompareData.GetHourCompareView(portIds, factors.Select(p => p.PollutantCode).ToArray(), new DateTime[,] { { dtBegin, dtEnd } }, pageSize, pageNo, out recordTotal, tabIndex, orderby, OriginalData.Checked);
                            gridDataCompare.DataSource = dataCompare;
                            recordTotal = dataCompare.Count;
                        }
                        else
                        {
                            gridDataCompare.DataSource = new DataTable();
                        }
                    }
                    else
                    {
                        if (portIds != null)
                        {
                            //小时数据
                            if (radlDataType.SelectedValue == "Hour")
                            {
                                string orderby = "PointId asc,Tstamp desc";
                                dtBegin = hourBegin.SelectedDate.Value;
                                dtEnd = hourEnd.SelectedDate.Value;
                                //【给隐藏域赋值，用于显示Chart】
                                SetHiddenData(portIds, factors, dtBegin.ToString() + ";" + dtEnd.ToString() + ";" + dtFrom.ToString() + ";" + dtTo.ToString());
                                var dataCompare = m_CompareData.GetHourCompareView(portIds, factors.Select(p => p.PollutantCode).ToArray(), new DateTime[,] { { dtBegin, dtEnd } }, pageSize, pageNo, out recordTotal, tabIndex, orderby);
                                gridDataCompare.DataSource = dataCompare;
                                recordTotal = dataCompare.Count;
                            }
                            //日数据
                            else if (radlDataType.SelectedValue == "Day")
                            {
                                string orderBy = "PointId asc,DateTime desc";
                                dtBegin = dayBegin.SelectedDate.Value;
                                dtEnd = dayEnd.SelectedDate.Value;
                                //【给隐藏域赋值，用于显示Chart】
                                SetHiddenData(portIds, factors, dtBegin.ToString() + ";" + dtEnd.ToString() + ";" + dtFrom.ToString() + ";" + dtTo.ToString());
                                var dataCompare = m_CompareData.GetDayCompareView(portIds, factors.Select(p => p.PollutantCode).ToArray(), new DateTime[,] { { dtBegin, dtEnd } }, pageSize, pageNo, out recordTotal, tabIndex, orderBy);
                                gridDataCompare.DataSource = dataCompare;
                                recordTotal = dataCompare.Count;
                            }
                            //月数据
                            else if (radlDataType.SelectedValue == "Month")
                            {
                                string orderBy = "PointId asc,Year desc,MonthOfYear desc";
                                int monthB = monthBegin.SelectedDate.Value.Year;
                                int monthE = monthEnd.SelectedDate.Value.Year;
                                int monthF = monthBegin.SelectedDate.Value.Month;
                                int monthT = monthEnd.SelectedDate.Value.Month;
                                //【给隐藏域赋值，用于显示Chart】
                                SetHiddenData(portIds, factors, monthB + ";" + monthF + ";" + monthE + ";" + monthT);
                                var dataCompare = m_CompareData.GetMonthCompareView(portIds, factors.Select(p => p.PollutantCode).ToArray(), new int[,] { { monthB, monthF, monthE, monthT } }, pageSize, pageNo, out recordTotal, tabIndex, orderBy);
                                gridDataCompare.DataSource = dataCompare;
                                recordTotal = dataCompare.Count;

                            }
                            //季数据
                            else if (radlDataType.SelectedValue == "Season")
                            {
                                string orderBy = "PointId asc,Year desc,SeasonOfYear desc";
                                int seasonB = seasonBegin.SelectedDate.Value.Year;
                                int seasonE = seasonEnd.SelectedDate.Value.Year;
                                int seasonF = Convert.ToInt32(seasonFrom.SelectedValue);
                                int seasonT = Convert.ToInt32(seasonTo.SelectedValue);
                                //【给隐藏域赋值，用于显示Chart】
                                SetHiddenData(portIds, factors, seasonB + ";" + seasonF + ";" + seasonE + ";" + seasonT);
                                var dataCompare = m_CompareData.GetSeasonCompareView(portIds, factors.Select(p => p.PollutantCode).ToArray(), new int[,] { { seasonB, seasonF, seasonE, seasonT } }, pageSize, pageNo, out recordTotal, tabIndex, orderBy);
                                gridDataCompare.DataSource = dataCompare;
                                recordTotal = dataCompare.Count;
                            }
                            //年数据
                            else if (radlDataType.SelectedValue == "Year")
                            {
                                string orderBy = "PointId asc,Year desc";
                                int yearB = yearBegin.SelectedDate.Value.Year;
                                int yearE = yearEnd.SelectedDate.Value.Year;
                                //【给隐藏域赋值，用于显示Chart】
                                SetHiddenData(portIds, factors, yearB + ";" + yearE);
                                var dataCompare = m_CompareData.GetYearCompareView(portIds, factors.Select(p => p.PollutantCode).ToArray(), new int[,] { { yearB, yearE } }, pageSize, pageNo, out recordTotal, tabIndex, orderBy);
                                gridDataCompare.DataSource = dataCompare;
                                recordTotal = dataCompare.Count;
                            }
                            //周数据
                            else if (radlDataType.SelectedValue == "Week")
                            {
                                string orderBy = "PointId asc,Year desc,WeekOfYear desc";
                                DateTime dateB3 = DateTime.Now;
                                DateTime dateE3 = DateTime.Now;
                                int weekB = weekBegin.SelectedDate.Value.Year;
                                int weekE = weekEnd.SelectedDate.Value.Year;
                                int weekfrom = Convert.ToInt32(weekFrom.SelectedText);
                                int weekto = Convert.ToInt32(weekTo.SelectedValue);
                                var week1 = weekBegin.SelectedDate.Value.DayOfWeek;
                                var week2 = weekEnd.SelectedDate.Value.DayOfWeek;
                                DateTime DateB1 = weekBegin.SelectedDate.Value;
                                DateTime DateB2 = DateB1.AddDays(7 * (weekfrom - 1));
                                if (DateB2.Month == 1 && DateB2.Day == 1)
                                {
                                    dateB3 = DateB2;
                                }
                                else
                                {
                                    dateB3 = DateB2.AddDays(0 - (int)week1);
                                }
                                DateTime DateE1 = weekEnd.SelectedDate.Value;
                                DateTime DateE2 = DateE1.AddDays(7 * (weekto - 1));
                                if (DateE2.Month == 12 && weekto == 5)
                                {
                                    dateE3 = DateE2;
                                }
                                else
                                {
                                    dateE3 = DateE2.AddDays(0 - (int)week2 + 6);
                                }
                                int weekF = WeekOfYear(dateB3);
                                int weekT = WeekOfYear(dateE3);
                                //【给隐藏域赋值，用于显示Chart】
                                SetHiddenData(portIds, factors, weekB + ";" + weekF + ";" + weekE + ";" + weekT);
                                var dataCompare = m_CompareData.GetWeekCompareView(portIds, factors.Select(p => p.PollutantCode).ToArray(), new int[,] { { weekB, weekF, weekE, weekT } }, pageSize, pageNo, out recordTotal, tabIndex, orderBy);
                                gridDataCompare.DataSource = dataCompare;
                                recordTotal = dataCompare.Count;
                            }
                        }
                        else
                        {
                            gridDataCompare.DataSource = new DataTable();
                        }
                    }
                }
                #endregion
                #region   不同时段多站点多参数
                if (tabStrip.SelectedIndex == 1)
                {
                    int tabIndex = 1;
                    //是否显示原始数据
                    if (OriginalData.Checked)
                    {
                        if (portIds != null)
                        {
                            string orderby = "PointId asc,Tstamp desc";
                            dtBegin = hourBegin.SelectedDate.Value;
                            dtEnd = hourEnd.SelectedDate.Value;
                            dtFrom = rdtpHourFrom.SelectedDate.Value;
                            dtTo = rdtpHourTo.SelectedDate.Value;
                            //【给隐藏域赋值，用于显示Chart】
                            SetHiddenData(portIds, factors, dtBegin.ToString() + ";" + dtEnd.ToString() + ";" + dtFrom.ToString() + ";" + dtTo.ToString());
                            var dataCompare = m_CompareData.GetHourCompareView(portIds, factors.Select(p => p.PollutantCode).ToArray(), new DateTime[,] { { dtBegin, dtEnd }, { dtFrom, dtTo } }, pageSize, pageNo, out recordTotal, tabIndex, orderby, OriginalData.Checked);
                            gridDataCompare.DataSource = dataCompare;
                            recordTotal = dataCompare.Count;
                        }
                        else
                        {
                            gridDataCompare.DataSource = new DataTable();
                        }
                    }
                    else
                    {
                        if (portIds != null)
                        {
                            //小时数据
                            if (radlDataType.SelectedValue == "Hour")
                            {
                                string orderby = "PointId asc,Tstamp desc";
                                dtBegin = hourBegin.SelectedDate.Value;
                                dtEnd = hourEnd.SelectedDate.Value;
                                dtFrom = rdtpHourFrom.SelectedDate.Value;
                                dtTo = rdtpHourTo.SelectedDate.Value;
                                //【给隐藏域赋值，用于显示Chart】
                                SetHiddenData(portIds, factors, dtBegin.ToString() + ";" + dtEnd.ToString() + ";" + dtFrom.ToString() + ";" + dtTo.ToString());
                                var dataCompare = m_CompareData.GetHourCompareView(portIds, factors.Select(p => p.PollutantCode).ToArray(), new DateTime[,] { { dtBegin, dtEnd }, { dtFrom, dtTo } }, pageSize, pageNo, out recordTotal, tabIndex, orderby);
                                gridDataCompare.DataSource = dataCompare;
                                recordTotal = dataCompare.Count;
                            }
                            //日数据
                            else if (radlDataType.SelectedValue == "Day")
                            {
                                string orderBy = "PointId asc,DateTime desc";
                                dtBegin = dayBegin.SelectedDate.Value;
                                dtEnd = dayEnd.SelectedDate.Value;
                                dtFrom = rdpDayFrom.SelectedDate.Value;
                                dtTo = rdpDayTo.SelectedDate.Value;
                                //【给隐藏域赋值，用于显示Chart】
                                SetHiddenData(portIds, factors, dtBegin.ToString() + ";" + dtEnd.ToString() + ";" + dtFrom.ToString() + ";" + dtTo.ToString());
                                var dataCompare = m_CompareData.GetDayCompareView(portIds, factors.Select(p => p.PollutantCode).ToArray(), new DateTime[,] { { dtBegin, dtEnd }, { dtFrom, dtTo } }, pageSize, pageNo, out recordTotal, tabIndex, orderBy);
                                gridDataCompare.DataSource = dataCompare;
                                recordTotal = dataCompare.Count;
                            }
                            //月数据
                            else if (radlDataType.SelectedValue == "Month")
                            {
                                string orderBy = "PointId asc,Year desc,MonthOfYear desc";
                                int monthB = monthBegin.SelectedDate.Value.Year;
                                int monthE = monthEnd.SelectedDate.Value.Year;
                                int monthF = monthBegin.SelectedDate.Value.Month;
                                int monthT = monthEnd.SelectedDate.Value.Month;
                                int dtmonthB = rmypMonthFrom.SelectedDate.Value.Year;
                                int dtmonthE = rmypMonthTo.SelectedDate.Value.Year;
                                int dtmonthF = rmypMonthFrom.SelectedDate.Value.Month;
                                int dtmonthT = rmypMonthTo.SelectedDate.Value.Month;
                                //【给隐藏域赋值，用于显示Chart】
                                SetHiddenData(portIds, factors, monthB + ";" + monthF + ";" + monthE + ";" + monthT + ";" + dtmonthB + ";" + dtmonthF + ";" + dtmonthE + ";" + dtmonthT);
                                var dataCompare = m_CompareData.GetMonthCompareView(portIds, factors.Select(p => p.PollutantCode).ToArray(), new int[,] { { monthB, monthF, monthE, monthT }, { dtmonthB, dtmonthF, dtmonthE, dtmonthT } }, pageSize, pageNo, out recordTotal, tabIndex, orderBy);
                                gridDataCompare.DataSource = dataCompare;
                                recordTotal = dataCompare.Count;
                            }
                            //季数据
                            else if (radlDataType.SelectedValue == "Season")
                            {
                                string orderBy = "PointId asc,Year desc,SeasonOfYear desc";
                                int seasonB = seasonBegin.SelectedDate.Value.Year;
                                int seasonE = seasonEnd.SelectedDate.Value.Year;
                                int seasonF = Convert.ToInt32(seasonFrom.SelectedValue);
                                int seasonT = Convert.ToInt32(seasonTo.SelectedValue);
                                int dtseasonB = rmypSeasonFrom.SelectedDate.Value.Year;
                                int dtseasonE = rmypSeasonFrom.SelectedDate.Value.Year;
                                int dtseasonF = Convert.ToInt32(ddlSeasonFrom.SelectedValue);
                                int dtseasonT = Convert.ToInt32(ddlSeasonTo.SelectedValue);
                                //【给隐藏域赋值，用于显示Chart】
                                SetHiddenData(portIds, factors, seasonB + ";" + seasonF + ";" + seasonE + ";" + seasonT + ";" + dtseasonB + ";" + dtseasonF + ";" + dtseasonE + ";" + dtseasonT);
                                var dataCompare = m_CompareData.GetSeasonCompareView(portIds, factors.Select(p => p.PollutantCode).ToArray(), new int[,] { { seasonB, seasonF, seasonE, seasonT }, { dtseasonB, dtseasonF, dtseasonE, dtseasonT } }, pageSize, pageNo, out recordTotal, tabIndex, orderBy);
                                gridDataCompare.DataSource = dataCompare;
                                recordTotal = dataCompare.Count;
                            }
                            //年数据
                            else if (radlDataType.SelectedValue == "Year")
                            {
                                string orderBy = "PointId asc,Year desc";
                                int yearB = yearBegin.SelectedDate.Value.Year;
                                int yearE = yearEnd.SelectedDate.Value.Year;
                                int dtyearB = rmypYearFrom.SelectedDate.Value.Year;
                                int dtyearE = rmypYearTo.SelectedDate.Value.Year;
                                //【给隐藏域赋值，用于显示Chart】
                                SetHiddenData(portIds, factors, yearB + ";" + yearE + ";" + dtyearB + ";" + dtyearE);
                                var dataCompare = m_CompareData.GetYearCompareView(portIds, factors.Select(p => p.PollutantCode).ToArray(), new int[,] { { yearB, yearE }, { dtyearB, dtyearE } }, pageSize, pageNo, out recordTotal, tabIndex, orderBy);
                                gridDataCompare.DataSource = dataCompare;
                                recordTotal = dataCompare.Count;
                            }
                            //周数据
                            else if (radlDataType.SelectedValue == "Week")
                            {
                                string orderBy = "PointId asc,Year desc,WeekOfYear desc";
                                DateTime dateB3 = DateTime.Now;
                                DateTime dateE3 = DateTime.Now;
                                int weekB = weekBegin.SelectedDate.Value.Year;
                                int weekE = weekEnd.SelectedDate.Value.Year;
                                int weekfrom = Convert.ToInt32(weekFrom.SelectedText);
                                int weekto = Convert.ToInt32(weekTo.SelectedValue);
                                var week1 = weekBegin.SelectedDate.Value.DayOfWeek;
                                var week2 = weekEnd.SelectedDate.Value.DayOfWeek;
                                DateTime DateB1 = weekBegin.SelectedDate.Value;
                                DateTime DateB2 = DateB1.AddDays(7 * (weekfrom - 1));
                                if (DateB2.Month == 1 && DateB2.Day == 1)
                                {
                                    dateB3 = DateB2;
                                }
                                else
                                {
                                    dateB3 = DateB2.AddDays(0 - (int)week1);
                                }
                                DateTime DateE1 = weekEnd.SelectedDate.Value;
                                DateTime DateE2 = DateE1.AddDays(7 * (weekto - 1));
                                if (DateE2.Month == 12 && weekto == 5)
                                {
                                    dateE3 = DateE2;
                                }
                                else
                                {
                                    dateE3 = DateE2.AddDays(0 - (int)week2 + 6);
                                }
                                int weekF = WeekOfYear(dateB3);
                                int weekT = WeekOfYear(dateE3);
                                DateTime dtDateB3 = DateTime.Now;
                                DateTime dtDateE3 = DateTime.Now;
                                int dtweekB = rmypWeekFrom.SelectedDate.Value.Year;
                                int dtweekE = rmypWeekTo.SelectedDate.Value.Year;
                                int dtWeekfrom = Convert.ToInt32(ddlWeekFrom.SelectedValue);
                                int dtWeekto = Convert.ToInt32(ddlWeekTo.SelectedValue);
                                var week3 = rmypWeekFrom.SelectedDate.Value.DayOfWeek;
                                var week4 = rmypWeekTo.SelectedDate.Value.DayOfWeek;
                                DateTime dtDateB1 = rmypWeekFrom.SelectedDate.Value;
                                DateTime dtDateB2 = dtDateB1.AddDays(7 * (dtWeekfrom - 1));
                                if (dtDateB2.Month == 1 && dtDateB2.Day == 1)
                                {
                                    dtDateB3 = dtDateB2;
                                }
                                else
                                {
                                    dtDateB3 = dtDateB2.AddDays(0 - (int)week3);
                                }
                                DateTime dtDateE1 = rmypWeekTo.SelectedDate.Value;
                                DateTime dtDateE2 = dtDateE1.AddDays(7 * (dtWeekto - 1));
                                if (dtDateE2.Month == 12 && dtWeekto == 5)
                                {
                                    dtDateE3 = dtDateE2;
                                }
                                else
                                {
                                    dtDateE3 = dtDateE2.AddDays(0 - (int)week4 + 6);
                                }
                                int dtweekF = WeekOfYear(dtDateB3);
                                int dtweekT = WeekOfYear(dtDateE3);
                                //【给隐藏域赋值，用于显示Chart】
                                SetHiddenData(portIds, factors, weekB + ";" + weekF + ";" + weekE + ";" + weekT + ";" + dtweekB + ";" + dtweekF + ";" + dtweekE + ";" + dtweekT);
                                var dataCompare = m_CompareData.GetWeekCompareView(portIds, factors.Select(p => p.PollutantCode).ToArray(), new int[,] { { weekB, weekF, weekE, weekT }, { dtweekB, dtweekF, dtweekE, dtweekT } }, pageSize, pageNo, out recordTotal, tabIndex, orderBy);
                                gridDataCompare.DataSource = dataCompare;
                                recordTotal = dataCompare.Count;
                            }
                        }
                        else
                        {
                            gridDataCompare.DataSource = new DataTable();
                        }
                    }

                }
                #endregion
                //数据总行数
                gridDataCompare.VirtualItemCount = recordTotal;
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// 页面隐藏域控件赋值,(周、月、季、年)
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="timeStr"></param>
        private void SetHiddenData(string[] portIds, IList<IPollutant> factors, string timeStr)
        {
            HiddenData.Value = string.Join(";", portIds) + "|" + string.Join(";", factors.Select(p => p.PollutantCode).ToArray())
                             + "|" + timeStr + "|" + "|" + radlDataType.SelectedValue + "|Air" + "|" + tabStrip.SelectedIndex;
            HiddenChartType.Value = ChartType.SelectedValue;
        }

        /// <summary>
        /// 生成RadGrid的绑定列
        /// </summary>
        /// <param sender></param>
        /// <param e></param>
        protected void gridDataCompare_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            try
            {
                if (e.Column.ColumnType.Equals("GridExpandColumn"))
                    return;

                GridBoundColumn col = (GridBoundColumn)e.Column;
                //追加类型
                if (col.DataField == "DataType")
                {
                    col.HeaderText = "类型";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                    col.AllowSorting = true;
                }
                //追加测点
                else if (col.DataField == "PointId")
                {
                    col.HeaderText = "测点";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
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
                else if (factors.Select(x => x.PollutantCode).Contains(col.DataField))
                {
                    int radGridColWidthValue = int.Parse(radGridColWidth.Value.ToString());
                    IPollutant factor = factors.FirstOrDefault(x => x.PollutantCode.Equals(col.DataField));
                    if (factor.PollutantName == "流向")
                    {
                        col.HeaderText = string.Format("{0}", factor.PollutantName);
                    }
                    else
                    {
                        col.HeaderText = string.Format("{0}<br>({1})", factor.PollutantName, factor.PollutantMeasureUnit);
                    }
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(radGridColWidthValue);
                    col.ItemStyle.Width = Unit.Pixel(radGridColWidthValue);
                }
                else
                {
                    e.Column.Visible = false;
                }

            }
            catch (Exception ex) { }
        }
        #endregion

        #region 服务器端控件事件处理
        /// <summary>
        /// 绑定数据源（用于分页、排序）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridDataCompare_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }

        /// <summary>
        /// 数据行绑定处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridDataCompare_ItemDataBound(object sender, GridItemEventArgs e)
        {
            GridDataItem item = e.Item as GridDataItem;
            if (e.Item is GridDataItem)
            {
                if (item["PointId"] != null)
                {
                    GridTableCell pointCell = (GridTableCell)item["PointId"];
                    IPoint point = points.FirstOrDefault(x => x.PointID.Equals(pointCell.Text.Trim()));
                    if (point != null)
                        pointCell.Text = point.PointName;
                }
                for (int iRow = 0; iRow < factors.Count; iRow++)
                {
                    IPollutant factor = factors[iRow];
                    GridTableCell cell = (GridTableCell)item[factor.PollutantCode];
                    decimal value = 0M;
                    if (decimal.TryParse(cell.Text, out value))
                    {
                        value = DecimalExtension.GetPollutantValue(value, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum));
                        cell.Text = value.ToString("");
                    }
                }
            }
        }

        /// <summary>
        /// 查询按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (tabStrip.SelectedIndex == 1)
            {
                int year = 0;
                int month = 0;
                int day = 0;
                int hour = 0;
                int yearT = 0;
                int monthT = 0;
                int dayT = 0;
                int hourT = 0;
                int Index = 0;
                int IndexT = 0;
                switch (radlDataType.SelectedValue)
                {
                    //小时
                    case "Hour":
                        year = (hourEnd.SelectedDate.Value.Year) - (hourBegin.SelectedDate.Value.Year);
                        month = (hourEnd.SelectedDate.Value.Month) - (hourBegin.SelectedDate.Value.Month);
                        day = (hourEnd.SelectedDate.Value.Day) - (hourBegin.SelectedDate.Value.Day);
                        hour = (hourEnd.SelectedDate.Value.Hour) - (hourBegin.SelectedDate.Value.Hour);
                        yearT = (rdtpHourTo.SelectedDate.Value.Year) - (rdtpHourFrom.SelectedDate.Value.Year);
                        monthT = (rdtpHourTo.SelectedDate.Value.Month) - (rdtpHourFrom.SelectedDate.Value.Month);
                        dayT = (rdtpHourTo.SelectedDate.Value.Day) - (rdtpHourFrom.SelectedDate.Value.Day);
                        hourT = (rdtpHourTo.SelectedDate.Value.Hour) - (rdtpHourFrom.SelectedDate.Value.Hour);
                        if (year != yearT || month != monthT || day != dayT || hour != hourT)
                        {
                            Alert("基准时间与比对时间范围不相等，请重新选择比对时间！");
                            return;
                        }
                        break;
                    //日 
                    case "Day":
                        year = (dayEnd.SelectedDate.Value.Year) - (dayBegin.SelectedDate.Value.Year);
                        month = (dayEnd.SelectedDate.Value.Month) - (dayBegin.SelectedDate.Value.Month);
                        day = (dayEnd.SelectedDate.Value.Day) - (dayBegin.SelectedDate.Value.Day);
                        yearT = (rdpDayTo.SelectedDate.Value.Year) - (rdpDayFrom.SelectedDate.Value.Year);
                        monthT = (rdpDayTo.SelectedDate.Value.Month) - (rdpDayFrom.SelectedDate.Value.Month);
                        dayT = (rdpDayTo.SelectedDate.Value.Day) - (rdpDayFrom.SelectedDate.Value.Day);
                        if (year != yearT || month != monthT || day != dayT)
                        {
                            Alert("基准时间与比对时间范围不相等，请重新选择比对时间！");
                            return;
                        }
                        break;
                    //月
                    case "Month":
                        year = (monthEnd.SelectedDate.Value.Year) - (monthBegin.SelectedDate.Value.Year);
                        month = (monthEnd.SelectedDate.Value.Month) - (monthBegin.SelectedDate.Value.Month);
                        yearT = (rmypMonthTo.SelectedDate.Value.Year) - (rmypMonthFrom.SelectedDate.Value.Year);
                        monthT = (rmypMonthTo.SelectedDate.Value.Month) - (rmypMonthFrom.SelectedDate.Value.Month);
                        if (year != yearT || month != monthT)
                        {
                            Alert("基准时间与比对时间范围不相等，请重新选择比对时间！");
                            return;
                        }
                        break;
                    //季
                    case "Season":
                        year = (seasonEnd.SelectedDate.Value.Year) - (seasonBegin.SelectedDate.Value.Year);
                        Index = seasonTo.SelectedIndex - seasonFrom.SelectedIndex;
                        yearT = (rmypSeasonTo.SelectedDate.Value.Year) - (rmypSeasonFrom.SelectedDate.Value.Year);
                        IndexT = ddlSeasonTo.SelectedIndex - ddlSeasonFrom.SelectedIndex;
                        if (year != yearT || Index != IndexT)
                        {
                            Alert("基准时间与比对时间范围不相等，请重新选择比对时间！");
                            return;
                        }
                        break;
                    //年
                    case "Year":
                        year = (yearEnd.SelectedDate.Value.Year) - (yearBegin.SelectedDate.Value.Year);
                        yearT = (rmypYearTo.SelectedDate.Value.Year) - (rmypYearFrom.SelectedDate.Value.Year);
                        if (year != yearT)
                        {
                            Alert("基准时间与比对时间范围不相等，请重新选择比对时间！");
                            return;
                        }
                        break;
                    //周
                    case "Week":
                        year = (weekEnd.SelectedDate.Value.Year) - (weekBegin.SelectedDate.Value.Year);
                        month = (weekEnd.SelectedDate.Value.Month) - (weekBegin.SelectedDate.Value.Month);
                        yearT = (rmypWeekTo.SelectedDate.Value.Year) - (rmypWeekFrom.SelectedDate.Value.Year);
                        monthT = (rmypWeekTo.SelectedDate.Value.Month) - (rmypWeekFrom.SelectedDate.Value.Month);
                        Index = weekTo.SelectedIndex - weekFrom.SelectedIndex;
                        IndexT = ddlWeekTo.SelectedIndex - ddlWeekFrom.SelectedIndex;
                        if (year != yearT || month != monthT || Index != IndexT)
                        {
                            Alert("基准时间与比对时间范围不相等，请重新选择比对时间！");
                            return;
                        }
                        break;
                }
            }
            gridDataCompare.CurrentPageIndex = 0;
            //  rgDataCompare.CurrentPageIndex = 0;
            gridDataCompare.Rebind();
            //  rgDataCompare.Rebind();
            if (tableStrip.SelectedTab.IsNotNullOrDBNull())
            {
                if (tableStrip.SelectedTab.Text == "图表")
                {
                    RegisterScript("InitGroupChart();");
                }
                else
                {
                    FirstLoadChart.Value = "1";
                }
            }
            else
            {
                RegisterScript("InitGroupChart();");
            }
        }

        /// <summary>
        /// 图表类型选择（折线图、柱形图、点状图）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ChartType_SelectedIndexChanged(object sender, EventArgs e)
        {
            HiddenChartType.Value = ChartType.SelectedValue;
            RegisterScript("ChartTypeChanged('" + ChartType.SelectedValue + "');");
        }
        #endregion

        /// <summary>
        /// Tab切换监测点和参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tabStrip_TabClick(object sender, RadTabStripEventArgs e)
        {
            BindGrid();
            //Bind();
            divSame.Visible = false;
            divMul.Visible = false;
            divMulti.Visible = false;
            divMultiTime.Visible = false;
            divHourT.Visible = false;
            divDayT.Visible = false;
            divMonthT.Visible = false;
            divSeasonT.Visible = false;
            divYearT.Visible = false;
            divWeekT.Visible = false;
            OriginalData.Checked = false;
            switch (tabStrip.SelectedIndex)
            {
                //相同时段多个站点多个参数
                case 0:
                    divSame.Visible = true;
                    //    paneWhere.Height = 60;
                    break;
                //可变时段多个站点同一参数
                case 1:
                    divMul.Visible = true;
                    divMulti.Visible = true;
                    divMultiTime.Visible = true;
                    paneWhere.Height = 80;
                    switch (radlDataType.SelectedValue)
                    {
                        //小时
                        case "Hour":
                            divHourT.Visible = true;
                            break;
                        //日 
                        case "Day":
                            divDayT.Visible = true;
                            break;
                        //月
                        case "Month":
                            divMonthT.Visible = true;
                            break;
                        //季
                        case "Season":
                            divSeasonT.Visible = true;
                            break;
                        //年
                        case "Year":
                            divYearT.Visible = true;
                            break;
                        //周
                        case "Week":
                            divWeekT.Visible = true;
                            break;
                    }
                    break;
            }
        }
        /// <summary>
        /// 数据类型切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void radlDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            OriginalData.Checked = false;
            OriginalData.Enabled = false;
            //不同时段时间框
            if (tabStrip.SelectedIndex == 1)
            {
                divHourT.Visible = false;
                divDayT.Visible = false;
                divMonthT.Visible = false;
                divSeasonT.Visible = false;
                divYearT.Visible = false;
                divWeekT.Visible = false;
                divHour.Visible = false;
                divDay.Visible = false;
                divMonth.Visible = false;
                divSeason.Visible = false;
                divYear.Visible = false;
                divWeek.Visible = false;
                switch (radlDataType.SelectedValue)
                {
                    //小时
                    case "Hour":
                        divHour.Visible = true;
                        divHourT.Visible = true;
                        OriginalData.Enabled = true;
                        break;
                    //日 
                    case "Day":
                        divDay.Visible = true;
                        divDayT.Visible = true;
                        break;
                    //月
                    case "Month":
                        divMonth.Visible = true;
                        divMonthT.Visible = true;
                        break;
                    //季
                    case "Season":
                        divSeason.Visible = true;
                        divSeasonT.Visible = true;
                        break;
                    //年
                    case "Year":
                        divYear.Visible = true;
                        divYearT.Visible = true;
                        break;
                    //周
                    case "Week":
                        divWeek.Visible = true;
                        divWeekT.Visible = true;
                        break;
                }
            }
            //相同时段时间框
            else
            {
                divHour.Visible = false;
                divDay.Visible = false;
                divMonth.Visible = false;
                divSeason.Visible = false;
                divYear.Visible = false;
                divWeek.Visible = false;
                switch (radlDataType.SelectedValue)
                {
                    case "Hour":
                        divHour.Visible = true;
                        OriginalData.Enabled = true;
                        break;
                    case "Day":
                        divDay.Visible = true;
                        break;
                    case "Month":
                        divMonth.Visible = true;
                        break;
                    case "Season":
                        divSeason.Visible = true;
                        break;
                    case "Year":
                        divYear.Visible = true;
                        break;
                    case "Week":
                        divWeek.Visible = true;
                        break;
                }
            }
        }

        #region  比对时间自动计算
        /// <summary>
        /// 比对时间计算
        /// </summary>
        /// 小时结束时间
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rdtpHourFrom_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            int year = (hourEnd.SelectedDate.Value.Year) - (hourBegin.SelectedDate.Value.Year);
            int month = (hourEnd.SelectedDate.Value.Month) - (hourBegin.SelectedDate.Value.Month);
            int day = (hourEnd.SelectedDate.Value.Day) - (hourBegin.SelectedDate.Value.Day);
            int hour = (hourEnd.SelectedDate.Value.Hour) - (hourBegin.SelectedDate.Value.Hour);
            rdtpHourTo.SelectedDate = rdtpHourFrom.SelectedDate.Value.AddYears(+year);
            rdtpHourTo.SelectedDate = rdtpHourTo.SelectedDate.Value.AddMonths(+month);
            rdtpHourTo.SelectedDate = rdtpHourTo.SelectedDate.Value.AddDays(+day);
            rdtpHourTo.SelectedDate = rdtpHourTo.SelectedDate.Value.AddHours(+hour);
        }
        /// <summary>
        /// 比对时间计算
        /// </summary>
        /// 小时开始时间
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rdtpHourTo_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            int year = (hourEnd.SelectedDate.Value.Year) - (hourBegin.SelectedDate.Value.Year);
            int month = (hourEnd.SelectedDate.Value.Month) - (hourBegin.SelectedDate.Value.Month);
            int day = (hourEnd.SelectedDate.Value.Day) - (hourBegin.SelectedDate.Value.Day);
            int hour = (hourEnd.SelectedDate.Value.Hour) - (hourBegin.SelectedDate.Value.Hour);
            rdtpHourFrom.SelectedDate = rdtpHourTo.SelectedDate.Value.AddYears(-year);
            rdtpHourFrom.SelectedDate = rdtpHourFrom.SelectedDate.Value.AddMonths(-month);
            rdtpHourFrom.SelectedDate = rdtpHourFrom.SelectedDate.Value.AddDays(-day);
            rdtpHourFrom.SelectedDate = rdtpHourFrom.SelectedDate.Value.AddHours(-hour);
        }
        /// <summary>
        /// 比对时间计算
        /// </summary>
        /// 日结束时间
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rdpDayFrom_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            int year = (dayEnd.SelectedDate.Value.Year) - (dayBegin.SelectedDate.Value.Year);
            int month = (dayEnd.SelectedDate.Value.Month) - (dayBegin.SelectedDate.Value.Month);
            int day = (dayEnd.SelectedDate.Value.Day) - (dayBegin.SelectedDate.Value.Day);
            rdpDayTo.SelectedDate = rdpDayFrom.SelectedDate.Value.AddYears(+year);
            rdpDayTo.SelectedDate = rdpDayTo.SelectedDate.Value.AddMonths(+month);
            rdpDayTo.SelectedDate = rdpDayTo.SelectedDate.Value.AddDays(+day);
        }
        /// <summary>
        /// 比对时间计算
        /// </summary>
        /// 日开始时间
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rdpDayTo_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            int year = (dayEnd.SelectedDate.Value.Year) - (dayBegin.SelectedDate.Value.Year);
            int month = (dayEnd.SelectedDate.Value.Month) - (dayBegin.SelectedDate.Value.Month);
            int day = (dayEnd.SelectedDate.Value.Day) - (dayBegin.SelectedDate.Value.Day);
            rdpDayFrom.SelectedDate = rdpDayTo.SelectedDate.Value.AddYears(-year);
            rdpDayFrom.SelectedDate = rdpDayFrom.SelectedDate.Value.AddMonths(-month);
            rdpDayFrom.SelectedDate = rdpDayFrom.SelectedDate.Value.AddDays(-day);
        }
        /// <summary>
        /// 比对时间计算
        /// </summary>
        /// 月结束时间
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rmypMonthFrom_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            int year = (monthEnd.SelectedDate.Value.Year) - (monthBegin.SelectedDate.Value.Year);
            int month = (monthEnd.SelectedDate.Value.Month) - (monthBegin.SelectedDate.Value.Month);
            rmypMonthTo.SelectedDate = rmypMonthFrom.SelectedDate.Value.AddYears(+year);
            rmypMonthTo.SelectedDate = rmypMonthTo.SelectedDate.Value.AddMonths(+month);
        }
        /// <summary>
        /// 比对时间计算
        /// </summary>
        /// 月开始时间
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rmypMonthTo_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            int year = (monthEnd.SelectedDate.Value.Year) - (monthBegin.SelectedDate.Value.Year);
            int month = (monthEnd.SelectedDate.Value.Month) - (monthBegin.SelectedDate.Value.Month);
            rmypMonthFrom.SelectedDate = rmypMonthTo.SelectedDate.Value.AddYears(-year);
            rmypMonthFrom.SelectedDate = rmypMonthFrom.SelectedDate.Value.AddMonths(-month);
        }
        /// <summary>
        /// 比对时间计算
        /// </summary>
        /// 季结束时间
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rmypSeasonFrom_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            int year = (seasonEnd.SelectedDate.Value.Year) - (seasonBegin.SelectedDate.Value.Year);
            rmypSeasonTo.SelectedDate = rmypSeasonFrom.SelectedDate.Value.AddYears(+year);
        }

        protected void ddlSeasonFrom_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            int index = seasonTo.SelectedIndex - seasonFrom.SelectedIndex;
            ddlSeasonTo.SelectedIndex = ddlSeasonFrom.SelectedIndex + index;
        }
        /// <summary>
        /// 比对时间计算
        /// </summary>
        /// 季开始时间
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rmypSeasonTo_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            int year = (seasonEnd.SelectedDate.Value.Year) - (seasonBegin.SelectedDate.Value.Year);
            rmypSeasonFrom.SelectedDate = rmypSeasonTo.SelectedDate.Value.AddYears(-year);
        }

        protected void ddlSeasonTo_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            int index = seasonTo.SelectedIndex - seasonFrom.SelectedIndex;
            ddlSeasonFrom.SelectedIndex = ddlSeasonTo.SelectedIndex - index;
        }
        /// <summary>
        /// 比对时间计算
        /// </summary>
        /// 年结束时间
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rmypYearFrom_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            int year = (yearEnd.SelectedDate.Value.Year) - (yearBegin.SelectedDate.Value.Year);
            rmypYearTo.SelectedDate = rmypYearFrom.SelectedDate.Value.AddYears(+year);
        }
        /// <summary>
        /// 比对时间计算
        /// </summary>
        /// 年开始时间
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rmypYearTo_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            int year = (yearEnd.SelectedDate.Value.Year) - (yearBegin.SelectedDate.Value.Year);
            rmypYearFrom.SelectedDate = rmypYearTo.SelectedDate.Value.AddYears(-year);
        }
        /// <summary>
        /// 比对时间计算
        /// </summary>
        /// 周结束时间
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rmypWeekFrom_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            int year = (weekEnd.SelectedDate.Value.Year) - (weekBegin.SelectedDate.Value.Year);
            int month = (weekEnd.SelectedDate.Value.Month) - (weekBegin.SelectedDate.Value.Month);
            rmypWeekTo.SelectedDate = rmypWeekFrom.SelectedDate.Value.AddYears(+year);
            rmypWeekTo.SelectedDate = rmypWeekTo.SelectedDate.Value.AddMonths(+month);

            ddlWeekFrom.Items.Clear();
            int monthDay = DateTime.DaysInMonth(rmypWeekFrom.SelectedDate.Value.Year, rmypWeekFrom.SelectedDate.Value.Month);
            if (monthDay % 7 > 0)
            {
                int week = monthDay / 7 + 1;
                for (int i = 0; i < week; i++)
                {
                    string j = Convert.ToString(i + 1);
                    ddlWeekFrom.Items.Add(new DropDownListItem(j, j));
                }
            }
            else
            {
                int week = monthDay / 7;
                for (int i = 0; i < week; i++)
                {
                    string j = Convert.ToString(i + 1);
                    ddlWeekFrom.Items.Add(new DropDownListItem(j, j));
                }
            }
            ddlWeekFrom.SelectedText = "1";
        }
        protected void ddlWeekFrom_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            int index = weekTo.SelectedIndex - weekFrom.SelectedIndex;
            ddlWeekTo.SelectedIndex = ddlWeekFrom.SelectedIndex + index;
        }
        /// <summary>
        /// 比对时间计算
        /// </summary>
        /// 周开始时间
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rmypWeekTo_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            int year = (weekEnd.SelectedDate.Value.Year) - (weekBegin.SelectedDate.Value.Year);
            int month = (weekEnd.SelectedDate.Value.Month) - (weekBegin.SelectedDate.Value.Month);
            rmypWeekFrom.SelectedDate = rmypWeekTo.SelectedDate.Value.AddYears(-year);
            rmypWeekFrom.SelectedDate = rmypWeekFrom.SelectedDate.Value.AddMonths(-month);

            ddlWeekTo.Items.Clear();
            int monthDay = DateTime.DaysInMonth(rmypWeekTo.SelectedDate.Value.Year, rmypWeekTo.SelectedDate.Value.Month);
            if (monthDay % 7 > 0)
            {
                int week = monthDay / 7 + 1;
                for (int i = 0; i < week; i++)
                {
                    string j = Convert.ToString(i + 1);
                    ddlWeekTo.Items.Add(new DropDownListItem(j, j));
                }
            }
            else
            {
                int week = monthDay / 7;
                for (int i = 0; i < week; i++)
                {
                    string j = Convert.ToString(i + 1);
                    ddlWeekTo.Items.Add(new DropDownListItem(j, j));
                }
            }
            ddlWeekTo.SelectedText = "1";
        }
        protected void ddlWeekTo_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            int index = weekTo.SelectedIndex - weekFrom.SelectedIndex;
            ddlWeekFrom.SelectedIndex = ddlWeekTo.SelectedIndex - index;
        }
        #endregion

        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        //public void Bind()
        //{
        //    if (!IsPostBack)
        //    {
        //        pointCbxRsm_SelectedChanged();
        //    }
        //    try
        //    {
        //        string[] factorCodes = factorCbxRsm.GetFactorValues(SmartEP.Core.Enums.CbxRsmReturnType.Code);
        //        DateTime dtBegin = DateTime.Now;
        //        DateTime dtEnd = DateTime.Now;
        //        DateTime dtFrom = DateTime.Now;
        //        DateTime dtTo = DateTime.Now;
        //        points = pointCbxRsm.GetPoints();
        //        string[] portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
        //        factors = factorCbxRsm.GetFactors();
        //        var dataCompare = new DataView();
        //        //生成RadGrid的绑定列
        //        dvStatistical = null;
        //        //每页显示数据个数            
        //        int pageSize = rgDataCompare.PageSize;
        //        //当前页的序号
        //        int pageNo = rgDataCompare.CurrentPageIndex;

        //        #region   相同时段多站点多参数
        //        if (tabStrip.SelectedIndex == 0)
        //        {

        //            if (portIds != null && factors.Count>0)
        //            {
        //                //是否显示原始数据
        //                if (OriginalData.Checked)
        //                {

        //                    dtBegin = hourBegin.SelectedDate.Value;
        //                    dtEnd = hourEnd.SelectedDate.Value;
        //                    dataCompare = m_CompareData.GetHourStatisticalData(portIds, factors, new DateTime[,] { { dtBegin, dtEnd } }, OriginalData.Checked);
        //                    rgDataCompare.DataSource = dataCompare;
        //                }
        //                else
        //                {
        //                    //小时数据
        //                    if (radlDataType.SelectedValue == "Hour")
        //                    {
        //                        dtBegin = hourBegin.SelectedDate.Value;
        //                        dtEnd = hourEnd.SelectedDate.Value;
        //                        dataCompare = m_CompareData.GetHourStatisticalData(portIds, factors, new DateTime[,] { { dtBegin, dtEnd } });
        //                        rgDataCompare.DataSource = dataCompare;
        //                    }
        //                    //日数据
        //                    else if (radlDataType.SelectedValue == "Day")
        //                    {

        //                        dtBegin = dayBegin.SelectedDate.Value;
        //                        dtEnd = dayEnd.SelectedDate.Value;
        //                        dataCompare = m_CompareData.GetDayStatisticalData(portIds, factors, new DateTime[,] { { dtBegin, dtEnd } });
        //                        rgDataCompare.DataSource = dataCompare;
        //                    }
        //                    //月数据
        //                    else if (radlDataType.SelectedValue == "Month")
        //                    {

        //                        int monthB = monthBegin.SelectedDate.Value.Year;
        //                        int monthE = monthEnd.SelectedDate.Value.Year;
        //                        int monthF = monthBegin.SelectedDate.Value.Month;
        //                        int monthT = monthEnd.SelectedDate.Value.Month;
        //                        dataCompare = m_CompareData.GetMonthStatisticalData(portIds, factors, new int[,] { { monthB, monthF, monthE, monthT } });
        //                        rgDataCompare.DataSource = dataCompare;
        //                    }
        //                    //季数据
        //                    else if (radlDataType.SelectedValue == "Season")
        //                    {

        //                        int seasonB = seasonBegin.SelectedDate.Value.Year;
        //                        int seasonE = seasonEnd.SelectedDate.Value.Year;
        //                        int seasonF = Convert.ToInt32(seasonFrom.SelectedValue);
        //                        int seasonT = Convert.ToInt32(seasonTo.SelectedValue);
        //                        dataCompare = m_CompareData.GetSeasonStatisticalData(portIds, factors, new int[,] { { seasonB, seasonF, seasonE, seasonT } });
        //                        rgDataCompare.DataSource = dataCompare;
        //                    }
        //                    //年数据
        //                    else if (radlDataType.SelectedValue == "Year")
        //                    {

        //                        int yearB = yearBegin.SelectedDate.Value.Year;
        //                        int yearE = yearEnd.SelectedDate.Value.Year;
        //                        dataCompare = m_CompareData.GetYearStatisticalData(portIds, factors, new int[,] { { yearB, yearE } });
        //                        rgDataCompare.DataSource = dataCompare;
        //                    }
        //                    //周数据
        //                    else if (radlDataType.SelectedValue == "Week")
        //                    {

        //                        DateTime dateB3 = DateTime.Now;
        //                        DateTime dateE3 = DateTime.Now;
        //                        int weekB = weekBegin.SelectedDate.Value.Year;
        //                        int weekE = weekEnd.SelectedDate.Value.Year;
        //                        int weekfrom = Convert.ToInt32(weekFrom.SelectedText);
        //                        int weekto = Convert.ToInt32(weekTo.SelectedValue);
        //                        var week1 = weekBegin.SelectedDate.Value.DayOfWeek;
        //                        var week2 = weekEnd.SelectedDate.Value.DayOfWeek;
        //                        DateTime DateB1 = weekBegin.SelectedDate.Value;
        //                        DateTime DateB2 = DateB1.AddDays(7 * (weekfrom - 1));
        //                        if (DateB2.Month == 1 && DateB2.Day == 1)
        //                        {
        //                            dateB3 = DateB2;
        //                        }
        //                        else
        //                        {
        //                            dateB3 = DateB2.AddDays(0 - (int)week1);
        //                        }
        //                        DateTime DateE1 = weekEnd.SelectedDate.Value;
        //                        DateTime DateE2 = DateE1.AddDays(7 * (weekto - 1));
        //                        if (DateE2.Month == 12 && weekto == 5)
        //                        {
        //                            dateE3 = DateE2;
        //                        }
        //                        else
        //                        {
        //                            dateE3 = DateE2.AddDays(0 - (int)week2 + 6);
        //                        }
        //                        int weekF = WeekOfYear(dateB3);
        //                        int weekT = WeekOfYear(dateE3);
        //                        dataCompare = m_CompareData.GetWeekStatisticalData(portIds, factors, new int[,] { { weekB, weekF, weekE, weekT } });
        //                        rgDataCompare.DataSource = dataCompare;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                rgDataCompare.DataSource = new DataTable();
        //            }
        //        }
        //        #endregion
        //        #region   不同时段多站点多参数
        //        if (tabStrip.SelectedIndex == 1)
        //        {
        //            if (portIds != null)
        //            {
        //                //是否显示原始数据
        //                if (OriginalData.Checked)
        //                {
        //                    dtBegin = hourBegin.SelectedDate.Value;
        //                    dtEnd = hourEnd.SelectedDate.Value;
        //                    dtFrom = rdtpHourFrom.SelectedDate.Value;
        //                    dtTo = rdtpHourTo.SelectedDate.Value;
        //                    dataCompare = m_CompareData.GetHourStatisticalData(portIds, factors, new DateTime[,] { { dtBegin, dtEnd }, { dtFrom, dtTo } }, OriginalData.Checked);
        //                    rgDataCompare.DataSource = dataCompare;
        //                }
        //                else
        //                {
        //                    //小时数据
        //                    if (radlDataType.SelectedValue == "Hour")
        //                    {
        //                        dtBegin = hourBegin.SelectedDate.Value;
        //                        dtEnd = hourEnd.SelectedDate.Value;
        //                        dtFrom = rdtpHourFrom.SelectedDate.Value;
        //                        dtTo = rdtpHourTo.SelectedDate.Value;
        //                        dataCompare = m_CompareData.GetHourStatisticalData(portIds, factors, new DateTime[,] { { dtBegin, dtEnd }, { dtFrom, dtTo } });
        //                        rgDataCompare.DataSource = dataCompare;
        //                    }
        //                    //日数据
        //                    else if (radlDataType.SelectedValue == "Day")
        //                    {
        //                        dtBegin = dayBegin.SelectedDate.Value;
        //                        dtEnd = dayEnd.SelectedDate.Value;
        //                        dtFrom = rdpDayFrom.SelectedDate.Value;
        //                        dtTo = rdpDayTo.SelectedDate.Value;
        //                        dataCompare = m_CompareData.GetDayStatisticalData(portIds, factors, new DateTime[,] { { dtBegin, dtEnd }, { dtFrom, dtTo } });
        //                        rgDataCompare.DataSource = dataCompare;
        //                    }
        //                    //月数据
        //                    else if (radlDataType.SelectedValue == "Month")
        //                    {
        //                        int monthB = monthBegin.SelectedDate.Value.Year;
        //                        int monthE = monthEnd.SelectedDate.Value.Year;
        //                        int monthF = monthBegin.SelectedDate.Value.Month;
        //                        int monthT = monthEnd.SelectedDate.Value.Month;
        //                        int dtmonthB = rmypMonthFrom.SelectedDate.Value.Year;
        //                        int dtmonthE = rmypMonthTo.SelectedDate.Value.Year;
        //                        int dtmonthF = rmypMonthFrom.SelectedDate.Value.Month;
        //                        int dtmonthT = rmypMonthTo.SelectedDate.Value.Month;
        //                        dataCompare = m_CompareData.GetMonthStatisticalData(portIds, factors, new int[,] { { monthB, monthF, monthE, monthT }, { dtmonthB, dtmonthF, dtmonthE, dtmonthT } });
        //                        rgDataCompare.DataSource = dataCompare;
        //                    }
        //                    //季数据
        //                    else if (radlDataType.SelectedValue == "Season")
        //                    {
        //                        int seasonB = seasonBegin.SelectedDate.Value.Year;
        //                        int seasonE = seasonEnd.SelectedDate.Value.Year;
        //                        int seasonF = Convert.ToInt32(seasonFrom.SelectedValue);
        //                        int seasonT = Convert.ToInt32(seasonTo.SelectedValue);
        //                        int dtseasonB = rmypSeasonFrom.SelectedDate.Value.Year;
        //                        int dtseasonE = rmypSeasonFrom.SelectedDate.Value.Year;
        //                        int dtseasonF = Convert.ToInt32(ddlSeasonFrom.SelectedValue);
        //                        int dtseasonT = Convert.ToInt32(ddlSeasonTo.SelectedValue);
        //                        dataCompare = m_CompareData.GetSeasonStatisticalData(portIds, factors, new int[,] { { seasonB, seasonF, seasonE, seasonT }, { dtseasonB, dtseasonF, dtseasonE, dtseasonT } });
        //                        rgDataCompare.DataSource = dataCompare;
        //                    }
        //                    //年数据
        //                    else if (radlDataType.SelectedValue == "Year")
        //                    {
        //                        int yearB = yearBegin.SelectedDate.Value.Year;
        //                        int yearE = yearEnd.SelectedDate.Value.Year;
        //                        int dtyearB = rmypYearFrom.SelectedDate.Value.Year;
        //                        int dtyearE = rmypYearTo.SelectedDate.Value.Year;
        //                        dataCompare = m_CompareData.GetYearStatisticalData(portIds, factors, new int[,] { { yearB, yearE }, { dtyearB, dtyearE } });
        //                        rgDataCompare.DataSource = dataCompare;
        //                    }
        //                    //周数据
        //                    else if (radlDataType.SelectedValue == "Week")
        //                    {
        //                        DateTime dateB3 = DateTime.Now;
        //                        DateTime dateE3 = DateTime.Now;
        //                        int weekB = weekBegin.SelectedDate.Value.Year;
        //                        int weekE = weekEnd.SelectedDate.Value.Year;
        //                        int weekfrom = Convert.ToInt32(weekFrom.SelectedText);
        //                        int weekto = Convert.ToInt32(weekTo.SelectedValue);
        //                        var week1 = weekBegin.SelectedDate.Value.DayOfWeek;
        //                        var week2 = weekEnd.SelectedDate.Value.DayOfWeek;
        //                        DateTime DateB1 = weekBegin.SelectedDate.Value;
        //                        DateTime DateB2 = DateB1.AddDays(7 * (weekfrom - 1));
        //                        if (DateB2.Month == 1 && DateB2.Day == 1)
        //                        {
        //                            dateB3 = DateB2;
        //                        }
        //                        else
        //                        {
        //                            dateB3 = DateB2.AddDays(0 - (int)week1);
        //                        }
        //                        DateTime DateE1 = weekEnd.SelectedDate.Value;
        //                        DateTime DateE2 = DateE1.AddDays(7 * (weekto - 1));
        //                        if (DateE2.Month == 12 && weekto == 5)
        //                        {
        //                            dateE3 = DateE2;
        //                        }
        //                        else
        //                        {
        //                            dateE3 = DateE2.AddDays(0 - (int)week2 + 6);
        //                        }
        //                        int weekF = WeekOfYear(dateB3);
        //                        int weekT = WeekOfYear(dateE3);
        //                        DateTime dtDateB3 = DateTime.Now;
        //                        DateTime dtDateE3 = DateTime.Now;
        //                        int dtweekB = rmypWeekFrom.SelectedDate.Value.Year;
        //                        int dtweekE = rmypWeekTo.SelectedDate.Value.Year;
        //                        int dtWeekfrom = Convert.ToInt32(ddlWeekFrom.SelectedValue);
        //                        int dtWeekto = Convert.ToInt32(ddlWeekTo.SelectedValue);
        //                        var week3 = rmypWeekFrom.SelectedDate.Value.DayOfWeek;
        //                        var week4 = rmypWeekTo.SelectedDate.Value.DayOfWeek;
        //                        DateTime dtDateB1 = rmypWeekFrom.SelectedDate.Value;
        //                        DateTime dtDateB2 = dtDateB1.AddDays(7 * (dtWeekfrom - 1));
        //                        if (dtDateB2.Month == 1 && dtDateB2.Day == 1)
        //                        {
        //                            dtDateB3 = dtDateB2;
        //                        }
        //                        else
        //                        {
        //                            dtDateB3 = dtDateB2.AddDays(0 - (int)week3);
        //                        }
        //                        DateTime dtDateE1 = rmypWeekTo.SelectedDate.Value;
        //                        DateTime dtDateE2 = dtDateE1.AddDays(7 * (dtWeekto - 1));
        //                        if (dtDateE2.Month == 12 && dtWeekto == 5)
        //                        {
        //                            dtDateE3 = dtDateE2;
        //                        }
        //                        else
        //                        {
        //                            dtDateE3 = dtDateE2.AddDays(0 - (int)week4 + 6);
        //                        }
        //                        int dtweekF = WeekOfYear(dtDateB3);
        //                        int dtweekT = WeekOfYear(dtDateE3);
        //                        dataCompare = m_CompareData.GetWeekStatisticalData(portIds, factors, new int[,] { { weekB, weekF, weekE, weekT }, { dtweekB, dtweekF, dtweekE, dtweekT } });
        //                        rgDataCompare.DataSource = dataCompare;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                rgDataCompare.DataSource = new DataTable();
        //            }
        //        }
        //        #endregion
        //        //数据总行数
        //        rgDataCompare.VirtualItemCount = dataCompare.Count;
        //    }
        //    catch (Exception ex) { }
        //}
        //protected void rgDataCompare_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        //{
        //    Bind();
        //}
        ///// <summary>
        ///// 数据行绑定处理
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void rgDataCompare_ItemDataBound(object sender, GridItemEventArgs e)
        //{
        //    GridDataItem item = e.Item as GridDataItem;
        //    if (e.Item is GridDataItem)
        //    {
        //        if (item["PointId"] != null)
        //        {
        //            GridTableCell pointCell = (GridTableCell)item["PointId"];
        //            IPoint point = points.FirstOrDefault(x => x.PointID.Equals(pointCell.Text.Trim()));
        //            if (point != null)
        //                pointCell.Text = point.PointName;
        //        }
        //        if (item["PollutantCode"] != null)
        //        {
        //            GridTableCell factorCell = (GridTableCell)item["PollutantCode"];
        //            IPollutant factor = factors.FirstOrDefault(x => x.PollutantCode.Equals(factorCell.Text.Trim()));
        //            if (factor != null)
        //                factorCell.Text = factor.PollutantName;
        //        }
        //    }
        //}

        //protected void rgDataCompare_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Column.ColumnType.Equals("GridExpandColumn"))
        //            return;


        //        GridBoundColumn col = (GridBoundColumn)e.Column;
        //        //追加类型
        //        if (col.DataField == "DataType")
        //        {
        //            col.HeaderText = "类型";
        //            col.EmptyDataText = "--";
        //            col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        //            col.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
        //            col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
        //            col.HeaderStyle.Width = Unit.Pixel(110);
        //            col.ItemStyle.Width = Unit.Pixel(110);
        //            col.AllowSorting = true;
        //        }
        //        //追加测点
        //        else if (col.DataField == "PointId")
        //        {
        //            col.HeaderText = "测点";
        //            col.EmptyDataText = "--";
        //            col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        //            col.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
        //            col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
        //            col.HeaderStyle.Width = Unit.Pixel(110);
        //            col.ItemStyle.Width = Unit.Pixel(110);
        //        }

        //        //追加因子
        //        else if (col.DataField == "PollutantCode")
        //        {
        //            col.HeaderText = "因子";
        //            col.EmptyDataText = "--";
        //            col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        //            col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
        //            col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
        //            col.HeaderStyle.Width = Unit.Pixel(110);
        //            col.ItemStyle.Width = Unit.Pixel(110);
        //        }
        //        //追加平均值
        //        else if (col.DataField == "Value_Avg")
        //        {
        //            col.HeaderText = "平均值";
        //            col.EmptyDataText = "--";
        //            col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        //            col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
        //            col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
        //            col.HeaderStyle.Width = Unit.Pixel(110);
        //            col.ItemStyle.Width = Unit.Pixel(110);
        //        }
        //        //追加最大值
        //        else if (col.DataField == "Value_Max")
        //        {
        //            col.HeaderText = "最大值";
        //            col.EmptyDataText = "--";
        //            col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        //            col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
        //            col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
        //            col.HeaderStyle.Width = Unit.Pixel(110);
        //            col.ItemStyle.Width = Unit.Pixel(110);
        //        }
        //        //追加最小值
        //        else if (col.DataField == "Value_Min")
        //        {
        //            col.HeaderText = "最小值";
        //            col.EmptyDataText = "--";
        //            col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        //            col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
        //            col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
        //            col.HeaderStyle.Width = Unit.Pixel(110);
        //            col.ItemStyle.Width = Unit.Pixel(110);
        //        }
        //        else if (col.DataField == "blankspaceColumn") { }
        //        else
        //        {
        //            e.Column.Visible = false;
        //        }

        //    }
        //    catch (Exception ex) { }
        //}
        /// <summary>
        /// 获取该年中是第几周
        /// </summary>
        /// <param name="day">日期</param>
        /// <returns></returns>
        private int WeekOfYear(DateTime day)
        {
            int weeknum;
            DateTime fDt = DateTime.Parse(day.Year.ToString() + "-01-01");
            int k = Convert.ToInt32(fDt.DayOfWeek);//得到该年的第一天是周几 
            if (k == 0)
            {
                k = 7;
            }
            int l = Convert.ToInt32(day.DayOfYear);//得到当天是该年的第几天 
            l = l - (7 - k + 1);
            if (l <= 0)
            {
                weeknum = 1;
            }
            else
            {
                if (l % 7 == 0)
                {
                    weeknum = l / 7 + 1;
                }
                else
                {
                    weeknum = l / 7 + 2;//不能整除的时候要加上前面的一周和后面的一周 
                }
            }
            return weeknum;
        }
        /// <summary>
        /// 站点因子关联
        /// </summary>
        protected void pointCbxRsm_SelectedChanged()
        {
            //points = pointCbxRsm.GetPoints();
            //InstrumentChannelService m_InstrumentChannelService = new InstrumentChannelService();
            //IList<string> list = new List<string>();
            //string[] factor;
            //string factors = string.Empty;
            //foreach (IPoint point in points)
            //{
            //    IQueryable<PollutantCodeEntity> p = m_InstrumentChannelService.RetrieveChannelListByPointUid(point.PointGuid);
            //    list = list.Union(p.Select(t => t.PollutantName)).ToList();
            //}
            //factor = list.ToArray();
            //foreach (string f in factor)
            //{
            //    factors += f + ";";
            //}
            //factorCbxRsm.SetFactorValuesFromNames(factors);
        }
        #region 获取开始和结束时间每月有几周
        protected void weekBegin_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            weekFrom.Items.Clear();
            int monthDay = DateTime.DaysInMonth(weekBegin.SelectedDate.Value.Year, weekBegin.SelectedDate.Value.Month);
            if (monthDay % 7 > 0)
            {
                int week = monthDay / 7 + 1;
                for (int i = 0; i < week; i++)
                {
                    string j = Convert.ToString(i + 1);
                    weekFrom.Items.Add(new DropDownListItem(j, j));
                }
            }
            else
            {
                int week = monthDay / 7;
                for (int i = 0; i < week; i++)
                {
                    string j = Convert.ToString(i + 1);
                    weekFrom.Items.Add(new DropDownListItem(j, j));
                }
            }
            weekFrom.SelectedText = "1";

        }

        protected void weekEnd_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            weekTo.Items.Clear();
            int monthDt = DateTime.DaysInMonth(weekEnd.SelectedDate.Value.Year, weekEnd.SelectedDate.Value.Month);
            if (monthDt % 7 > 0)
            {
                int week = monthDt / 7 + 1;
                for (int i = 0; i < week; i++)
                {
                    string j = Convert.ToString(i + 1);
                    weekTo.Items.Add(new DropDownListItem(j, j));
                }
            }
            else
            {
                int week = monthDt / 7;
                for (int i = 0; i < week; i++)
                {
                    string j = Convert.ToString(i + 1);
                    weekTo.Items.Add(new DropDownListItem(j, j));
                }
            }
            weekTo.SelectedText = "4";
        }
        #endregion

        protected void tableStrip_TabClick(object sender, RadTabStripEventArgs e)
        {

        }
    }
}