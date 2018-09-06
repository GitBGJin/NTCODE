using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.AutoMonitoring.Air;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.DataAnalyze.Air.DataQuery;
using SmartEP.Utilities.Calendar;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Utilities.Web.UI;
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
    /// 名称：DifferentDayCompareNew.aspx.cs
    /// 创建人：
    /// 创建日期：2015-08-14
    /// 维护人员：
    /// 最新维护人员：刘晋
    /// 最新维护日期：2017-05-22
    /// 功能摘要：环境空气常规参数（数据比对分析）
    /// 虚拟分页类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class DifferentDayCompareNew : SmartEP.WebUI.Common.BasePage
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
        /// <summary>
        /// 默认是否常规站字段为空
        /// </summary>
        string isAudit = string.Empty;

        /// <summary>
        /// 设置站点控件选中超级站
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreInit(EventArgs e)
        {
            
            isAudit = PageHelper.GetQueryString("auditOrNot");
            factorCbxRsm.isAudit(isAudit);
            factor.isAudit(isAudit);
        }
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
            string[] EnableOrNotportsarry = EnableOrNotports.Where(p => p.ContrlUid == "6fadff52-2338-4319-9f1d-7317823770ad" || p.ContrlUid == "c1158eb6-4d69-4846-a963-d16b9d2794ca").Select(p => p.MonitoringPointName).ToArray();
            foreach (string point in EnableOrNotportsarry)
            {
                strpointName += point + ";";
            }
            strpointName = System.Configuration.ConfigurationManager.AppSettings["PointId"];
            pointCbxRsm.SetPointValuesFromNames(strpointName);
            //因子
            SmartEP.Service.BaseData.Channel.AirPollutantService m_AirPollutantService = Singleton<SmartEP.Service.BaseData.Channel.AirPollutantService>.GetInstance();
            IQueryable<PollutantCodeEntity> Pollutant = m_AirPollutantService.RetrieveListByCalAQI();
            string PollutantName = "";
            string[] pollutantarry = Pollutant.Select(p => p.PollutantName).ToArray();
            foreach (string strName in pollutantarry)
            {
                PollutantName += strName + ";";
            }
            //设置默认绑定因子
            string pollutantName = System.Configuration.ConfigurationManager.AppSettings["AirPollutant"];
            factorCbxRsm.SetFactorValuesFromCodes(pollutantName);
            factor.SetFactorValuesFromCodes(pollutantName);

            //string pollutantName = System.Configuration.ConfigurationManager.AppSettings["AirPollutant"];
            //factorCbxRsm.SetFactorValuesFromNames(pollutantName);
            //相同时段时间初始化      
            hourBegin.SelectedDate = DateTime.Now.AddHours(-24);
            hourEnd.SelectedDate = DateTime.Now;
            dayBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
            dayEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            monthBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToString("yyyy-MM"));
            monthEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"));
            seasonBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"));
            seasonEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"));
            weekBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToString("yyyy-MM"));
            weekEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"));
            yearBegin.SelectedDate = DateTime.Now.AddYears(-1);
            yearEnd.SelectedDate = DateTime.Now;


            //不同时段比对时间初始化
            rdtpHourFrom.SelectedDate = DateTime.Now.AddDays(-1).AddHours(-24);
            rdtpHourTo.SelectedDate = DateTime.Now.AddDays(-1);
            rdpDayFrom.SelectedDate = Convert.ToDateTime(DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd"));
            rdpDayTo.SelectedDate = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
            rmypMonthFrom.SelectedDate = Convert.ToDateTime(DateTime.Now.AddMonths(-2).ToString("yyyy-MM"));
            rmypMonthTo.SelectedDate = Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToString("yyyy-MM"));
            rmypSeasonFrom.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"));
            rmypSeasonTo.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"));
            rmypYearFrom.SelectedDate = DateTime.Now.AddYears(-1);
            rmypYearTo.SelectedDate = DateTime.Now;
            rmypWeekFrom.SelectedDate = Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToString("yyyy-MM"));
            rmypWeekTo.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"));


            //数据类型
            radlDataType.Items.Add(new ListItem("小时数据", PollutantDataType.Hour.ToString()));
            radlDataType.Items.Add(new ListItem("日数据", PollutantDataType.Day.ToString()));
            radlDataType.Items.Add(new ListItem("周数据", PollutantDataType.Week.ToString()));
            radlDataType.Items.Add(new ListItem("月数据", PollutantDataType.Month.ToString()));
            radlDataType.Items.Add(new ListItem("季数据", PollutantDataType.Season.ToString()));
            radlDataType.Items.Add(new ListItem("年数据", PollutantDataType.Year.ToString()));
            radlDataType.SelectedValue = PollutantDataType.Hour.ToString();

            radlHour.Items.Add(new ListItem("小时数据", PollutantDataType.Hour.ToString()));
            radlHour.SelectedValue = PollutantDataType.Hour.ToString();
            divHour.Visible = true;
            //Tab切换初始绑定
            tabStrip.SelectedIndex = 0;
            tableStrip.SelectedIndex = 0;
            divSame.Visible = true;

            BindWeekBComboBox();
            BindWeekEComboBox();
            BindWeekFComboBox();
            BindWeekTComboBox();
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


                //生成RadGrid的绑定列
                dvStatistical = null;
                //每页显示数据个数            
                int pageSize = gridDataCompare.PageSize;
                //当前页的序号
                int pageNo = gridDataCompare.CurrentPageIndex;

                //数据总行数
                int recordTotal = 0;

                #region   原始审核比对
                if (tabStrip.SelectedIndex == 0)
                {
                    factors = factorCbxRsm.GetFactors();
                    int tabIndex = tabStrip.SelectedIndex;
                    DateTime dtBeginD = new DateTime();
                    DateTime dtEndD = new DateTime();
                    if (portIds != null)
                    {
                        //是否显示原始数据
                        string orderBy = "PointId,tstamp desc";
                        dtBegin = hourBegin.SelectedDate.Value;
                        dtEnd = hourEnd.SelectedDate.Value;
                        string type = "'审核','原始'";
                        //【给隐藏域赋值，用于显示Chart】
                        SetHiddenData(portIds, factors, dtBegin.ToString() + ";" + dtEnd.ToString() + ";" + dtFrom.ToString() + ";" + dtTo.ToString());
                        var dataCompare = m_CompareData.GetHourCompare(portIds, factors.Select(p => p.PollutantCode).ToArray(), dtBegin, dtEnd, dtBeginD, dtEndD, type, pageSize, pageNo, out recordTotal, tabIndex, orderBy);
                        gridDataCompare.DataSource = dataCompare;
                        //默认冻结前三列
                        gridDataCompare.ClientSettings.Scrolling.FrozenColumnsCount = 3;
                    }
                    else
                    {
                        gridDataCompare.DataSource = new DataTable();
                    }
                }
                #endregion
                #region   相同时段多站点多参数
                if (tabStrip.SelectedIndex == 1)
                {
                    factors = factor.GetFactors();
                    int tabIndex = tabStrip.SelectedIndex;
                    DateTime dtBeginD = new DateTime();
                    DateTime dtEndD = new DateTime();
                    if (portIds != null)
                    {
                        //小时数据
                        if (radlDataType.SelectedValue == "Hour")
                        {
                            string orderby = "tstamp desc,PointId,Type desc";
                            dtBegin = hourBegin.SelectedDate.Value;
                            dtEnd = hourEnd.SelectedDate.Value;
                            string type = "'审核'";
                            //【给隐藏域赋值，用于显示Chart】
                            SetHiddenData(portIds, factors, dtBegin.ToString() + ";" + dtEnd.ToString() + ";" + dtFrom.ToString() + ";" + dtTo.ToString());
                            var dataCompare = m_CompareData.GetHourOtherCompare(portIds, factors.Select(p => p.PollutantCode).ToArray(), dtBegin, dtEnd, dtBeginD, dtEndD, type, pageSize, pageNo, out recordTotal, tabIndex, orderby);
                            //dataCompare.Sort = "PointId,tstamp desc,rowMack,Type desc";
                            gridDataCompare.DataSource = dataCompare;
                        }
                        //日数据
                        else if (radlDataType.SelectedValue == "Day")
                        {
                            string orderBy = "DateTime desc,PointId asc";
                            dtBegin = dayBegin.SelectedDate.Value;
                            dtEnd = dayEnd.SelectedDate.Value;
                            //【给隐藏域赋值，用于显示Chart】
                            SetHiddenData(portIds, factors, dtBegin.ToString() + ";" + dtEnd.ToString() + ";" + dtFrom.ToString() + ";" + dtTo.ToString());
                            var dataCompare = m_CompareData.GetDayCompare(portIds, factors.Select(p => p.PollutantCode).ToArray(), dtBegin, dtEnd, dtBeginD, dtEndD, pageSize, pageNo, out recordTotal, tabIndex, orderBy);
                            gridDataCompare.DataSource = dataCompare;
                        }
                        //月数据
                        else if (radlDataType.SelectedValue == "Month")
                        {
                            string orderBy = "PointId asc,Year desc,MonthOfYear desc";
                            int monthB = monthBegin.SelectedDate.Value.Year;
                            int monthE = monthEnd.SelectedDate.Value.Year;
                            int monthF = monthBegin.SelectedDate.Value.Month;
                            int monthT = monthEnd.SelectedDate.Value.Month;
                            int dtmonthB = -1;
                            int dtmonthE = -1;
                            int dtmonthF = -1;
                            int dtmonthT = -1;
                            SetHiddenData(portIds, factors, monthB + ";" + monthF + ";" + monthE + ";" + monthT);
                            var dataCompare = m_CompareData.GetMonthCompare(portIds, factors.Select(p => p.PollutantCode).ToArray(), monthB, monthF, monthE, monthT, dtmonthB, dtmonthF, dtmonthE, dtmonthT, pageSize, pageNo, out recordTotal, tabIndex, orderBy);
                            gridDataCompare.DataSource = dataCompare;
                        }
                        //季数据
                        else if (radlDataType.SelectedValue == "Season")
                        {
                            string orderBy = "PointId asc,Year desc,SeasonOfYear desc";
                            int seasonB = seasonBegin.SelectedDate.Value.Year;
                            int seasonE = seasonEnd.SelectedDate.Value.Year;
                            int seasonF = Convert.ToInt32(seasonFrom.SelectedValue);
                            int seasonT = Convert.ToInt32(seasonTo.SelectedValue);
                            int dtseasonB = -1;
                            int dtseasonE = -1;
                            int dtseasonF = -1;
                            int dtseasonT = -1;
                            //【给隐藏域赋值，用于显示Chart】
                            SetHiddenData(portIds, factors, seasonB + ";" + seasonF + ";" + seasonE + ";" + seasonT);
                            var dataCompare = m_CompareData.GetSeasonCompare(portIds, factors.Select(p => p.PollutantCode).ToArray(), seasonB, seasonF, seasonE, seasonT, dtseasonB, dtseasonF, dtseasonE, dtseasonT, pageSize, pageNo, out recordTotal, tabIndex, orderBy);
                            gridDataCompare.DataSource = dataCompare;
                        }
                        //年数据
                        else if (radlDataType.SelectedValue == "Year")
                        {
                            string orderBy = "PointId asc,Year desc";
                            int yearB = yearBegin.SelectedDate.Value.Year;
                            int yearE = yearEnd.SelectedDate.Value.Year;
                            int dtyearB = -1;
                            int dtyearE = -1;
                            //【给隐藏域赋值，用于显示Chart】
                            SetHiddenData(portIds, factors, yearB + ";" + yearE);
                            var dataCompare = m_CompareData.GetYearCompare(portIds, factors.Select(p => p.PollutantCode).ToArray(), yearB, yearE, dtyearB, dtyearE, pageSize, pageNo, out recordTotal, tabIndex, orderBy);
                            gridDataCompare.DataSource = dataCompare;
                        }
                        //周数据
                        else if (radlDataType.SelectedValue == "Week")
                        {
                            string orderBy = "PointId asc,Year desc,WeekOfYear desc";
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
                                weekT = ChinaDate.WeekOfYear(DateTime.ParseExact(weekTo.SelectedValue, "yyyy-MM-dd", null));

                            int dtweekB = -1;
                            int dtweekF = -1;
                            int dtweekE = -1;
                            int dtweekT = -1;
                            //【给隐藏域赋值，用于显示Chart】
                            SetHiddenData(portIds, factors, weekB + ";" + weekF + ";" + weekE + ";" + weekT);
                            var dataCompare = m_CompareData.GetWeekCompare(portIds, factors.Select(p => p.PollutantCode).ToArray(), weekB, weekF, weekE, weekT, dtweekB, dtweekF, dtweekE, dtweekT, pageSize, pageNo, out recordTotal, tabIndex, orderBy);
                            gridDataCompare.DataSource = dataCompare;
                        }
                    }
                    else
                    {
                        gridDataCompare.DataSource = new DataTable();
                    }
                }
                #endregion
                #region   不同时段多站点多参数
                if (tabStrip.SelectedIndex == 2)
                {
                    factors = factor.GetFactors();
                    int tabIndex = 2;

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
                            //string pointidnew = "";
                            //if (portIds.Length == 1)
                            //{
                            //    pointidnew += portIds[0] + "~" + dtBegin.ToString() + "~" + dtEnd.ToString() + ";";
                            //    pointidnew += portIds[0] + "~" + dtFrom.ToString() + "~" + dtTo.ToString() + ";";
                            //}
                            //else
                            //{
                            //    for (int i = 0; i < portIds.Length; i++)
                            //    {
                            //        if (i == 0)
                            //        {
                            //            pointidnew += portIds[i] + "~" + dtBegin.ToString() + "~" + dtEnd.ToString() + ";";
                            //        }
                            //        else
                            //        {
                            //            pointidnew += portIds[i] + "~" + dtFrom.ToString() + "~" + dtTo.ToString() + ";";
                            //        }
                            //    }
                            //}
                            //string[] portIdsArry = pointidnew.TrimEnd(';').Split(';');
                            string type = "'审核'";
                            //【给隐藏域赋值，用于显示Chart】
                            SetHiddenData(portIds, factors, dtBegin.ToString() + ";" + dtEnd.ToString() + ";" + dtFrom.ToString() + ";" + dtTo.ToString());
                            hdtimeBetween.Value = dtBegin.ToString() + "~" + dtEnd.ToString() + ";" + dtFrom.ToString() + "~" + dtTo.ToString();
                            var dataCompare = m_CompareData.GetHourOtherCompare(portIds, factors.Select(p => p.PollutantCode).ToArray(), dtBegin, dtEnd, dtFrom, dtTo, type, pageSize, pageNo, out recordTotal, tabIndex, orderby);
                            dataCompare.Sort = "Ordernum";
                            gridDataCompare.DataSource = dataCompare;
                        }
                        //日数据
                        else if (radlDataType.SelectedValue == "Day")
                        {
                            string orderBy = "PointId asc,DateTime desc";
                            dtBegin = dayBegin.SelectedDate.Value;
                            dtEnd = dayEnd.SelectedDate.Value;
                            dtFrom = rdpDayFrom.SelectedDate.Value;
                            dtTo = rdpDayTo.SelectedDate.Value;
                            //string pointidnew = "";
                            //if (portIds.Length == 1)
                            //{
                            //    pointidnew += portIds[0] + "~" + dtBegin.ToString() + "~" + dtEnd.ToString() + ";";
                            //    pointidnew += portIds[0] + "~" + dtFrom.ToString() + "~" + dtTo.ToString() + ";";
                            //}
                            //else
                            //{
                            //    for (int i = 0; i < portIds.Length; i++)
                            //    {
                            //        if (i == 0)
                            //        {
                            //            pointidnew += portIds[i] + "~" + dtBegin.ToString() + "~" + dtEnd.ToString() + ";";
                            //        }
                            //        else
                            //        {
                            //            pointidnew += portIds[i] + "~" + dtFrom.ToString() + "~" + dtTo.ToString() + ";";
                            //        }
                            //    }
                            //}
                            //string[] portIdsArry = pointidnew.TrimEnd(';').Split(';');
                            SetHiddenData(portIds, factors, dtBegin.ToString() + ";" + dtEnd.ToString() + ";" + dtFrom.ToString() + ";" + dtTo.ToString());
                            hdtimeBetween.Value = dtBegin.ToString() + "~" + dtEnd.ToString() + ";" + dtFrom.ToString() + "~" + dtTo.ToString();
                            var dataCompare = m_CompareData.GetDayCompare(portIds, factors.Select(p => p.PollutantCode).ToArray(), dtBegin, dtEnd, dtFrom, dtTo, pageSize, pageNo, out recordTotal, tabIndex, orderBy);
                            dataCompare.Sort = "DateTime DESC";
                            gridDataCompare.DataSource = dataCompare;
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

                            //string pointidnew = "";
                            //if (portIds.Length == 1)
                            //{
                            //    pointidnew += portIds[0] + "~" + monthB.ToString() + "~" + monthF.ToString() + "~" + monthE.ToString() + "~" + monthT.ToString() + ";";
                            //    pointidnew += portIds[0] + "~" + dtmonthB.ToString() + "~" + dtmonthF.ToString() + "~" + dtmonthE.ToString() + "~" + dtmonthT.ToString() + ";";
                            //}
                            //else
                            //{
                            //    for (int i = 0; i < portIds.Length; i++)
                            //    {
                            //        if (i == 0)
                            //        {
                            //            pointidnew += portIds[i] + "~" + monthB.ToString() + "~" + monthF.ToString() + "~" + monthE.ToString() + "~" + monthT.ToString() + ";";
                            //        }
                            //        else
                            //        {
                            //            pointidnew += portIds[i] + "~" + dtmonthB.ToString() + "~" + dtmonthF.ToString() + "~" + dtmonthE.ToString() + "~" + dtmonthT.ToString() + ";";
                            //        }
                            //    }
                            //}
                            //string[] portIdsArry = pointidnew.TrimEnd(';').Split(';');
                            //【给隐藏域赋值，用于显示Chart】
                            SetHiddenData(portIds, factors, monthB + ";" + monthF + ";" + monthE + ";" + monthT + ";" + dtmonthB + ";" + dtmonthF + ";" + dtmonthE + ";" + dtmonthT);
                            hdtimeBetween.Value = monthB.ToString() + "~" + monthF.ToString() + "~" + monthE.ToString() + "~" + monthT.ToString() + ";" + dtmonthB.ToString() + "~" + dtmonthF.ToString() + "~" + dtmonthE.ToString() + "~" + dtmonthT.ToString();
                            var dataCompare = m_CompareData.GetMonthCompare(portIds, factors.Select(p => p.PollutantCode).ToArray(), monthB, monthF, monthE, monthT, dtmonthB, dtmonthF, dtmonthE, dtmonthT, pageSize, pageNo, out recordTotal, tabIndex, orderBy);
                            dataCompare.Sort = "Ordernum desc";
                            gridDataCompare.DataSource = dataCompare;
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
                            int dtseasonE = rmypSeasonTo.SelectedDate.Value.Year;
                            int dtseasonF = Convert.ToInt32(ddlSeasonFrom.SelectedValue);
                            int dtseasonT = Convert.ToInt32(ddlSeasonTo.SelectedValue);
                            //string pointidnew = "";
                            //if (portIds.Length == 1)
                            //{
                            //    pointidnew += portIds[0] + "~" + seasonB.ToString() + "~" + seasonF.ToString() + "~" + seasonE.ToString() + "~" + seasonT.ToString() + ";";
                            //    pointidnew += portIds[0] + "~" + dtseasonB.ToString() + "~" + dtseasonF.ToString() + "~" + dtseasonE.ToString() + "~" + dtseasonT.ToString() + ";";
                            //}
                            //else
                            //{
                            //    for (int i = 0; i < portIds.Length; i++)
                            //    {
                            //        if (i == 0)
                            //        {
                            //            pointidnew += portIds[i] + "~" + seasonB.ToString() + "~" + seasonF.ToString() + "~" + seasonE.ToString() + "~" + seasonT.ToString() + ";";
                            //        }
                            //        else
                            //        {
                            //            pointidnew += portIds[i] + "~" + dtseasonB.ToString() + "~" + dtseasonF.ToString() + "~" + dtseasonE.ToString() + "~" + dtseasonT.ToString() + ";";
                            //        }
                            //    }
                            //}
                            //string[] portIdsArry = pointidnew.TrimEnd(';').Split(';');
                            //【给隐藏域赋值，用于显示Chart】
                            SetHiddenData(portIds, factors, seasonB + ";" + seasonF + ";" + seasonE + ";" + seasonT + ";" + dtseasonB + ";" + dtseasonF + ";" + dtseasonE + ";" + dtseasonT);
                            hdtimeBetween.Value = seasonB.ToString() + "~" + seasonF.ToString() + "~" + seasonE.ToString() + "~" + seasonT.ToString() + ";" + dtseasonB.ToString() + "~" + dtseasonF.ToString() + "~" + dtseasonE.ToString() + "~" + dtseasonT.ToString();
                            var dataCompare = m_CompareData.GetSeasonCompare(portIds, factors.Select(p => p.PollutantCode).ToArray(), seasonB, seasonF, seasonE, seasonT, dtseasonB, dtseasonF, dtseasonE, dtseasonT, pageSize, pageNo, out recordTotal, tabIndex, orderBy);
                            dataCompare.Sort = "Ordernum desc";
                            gridDataCompare.DataSource = dataCompare;
                        }
                        //年数据
                        else if (radlDataType.SelectedValue == "Year")
                        {
                            string orderBy = "PointId asc,Year desc";
                            int yearB = yearBegin.SelectedDate.Value.Year;
                            int yearE = yearEnd.SelectedDate.Value.Year;
                            int dtyearB = rmypYearFrom.SelectedDate.Value.Year;
                            int dtyearE = rmypYearTo.SelectedDate.Value.Year;
                            //string pointidnew = "";
                            //if (portIds.Length == 1)
                            //{
                            //    pointidnew += portIds[0] + "~" + yearB.ToString() + "~" + yearE.ToString() + ";";
                            //    pointidnew += portIds[0] + "~" + dtyearB.ToString() + "~" + dtyearE.ToString() + ";";
                            //}
                            //else
                            //{
                            //    for (int i = 0; i < portIds.Length; i++)
                            //    {
                            //        if (i == 0)
                            //        {
                            //            pointidnew += portIds[i] + "~" + yearB.ToString() + "~" + yearE.ToString() + ";";
                            //        }
                            //        else
                            //        {
                            //            pointidnew += portIds[i] + "~" + dtyearB.ToString() + "~" + dtyearE.ToString() + ";";
                            //        }
                            //    }
                            //}
                            //string[] portIdsArry = pointidnew.TrimEnd(';').Split(';');
                            //【给隐藏域赋值，用于显示Chart】
                            SetHiddenData(portIds, factors, yearB + ";" + yearE + ";" + dtyearB + ";" + dtyearE);
                            hdtimeBetween.Value = yearB.ToString() + "~" + yearE.ToString() + ";" + dtyearB.ToString() + "~" + dtyearE.ToString();
                            var dataCompare = m_CompareData.GetYearCompare(portIds, factors.Select(p => p.PollutantCode).ToArray(), yearB, yearE, dtyearB, dtyearE, pageSize, pageNo, out recordTotal, tabIndex, orderBy);
                            dataCompare.Sort = "Ordernum desc";
                            gridDataCompare.DataSource = dataCompare;
                        }
                        //周数据
                        else if (radlDataType.SelectedValue == "Week")
                        {
                            string orderBy = "PointId asc,Year desc,WeekOfYear desc";
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
                                weekT = ChinaDate.WeekOfYear(DateTime.ParseExact(weekTo.SelectedValue, "yyyy-MM-dd", null));

                            int dtweekB = rmypWeekFrom.SelectedDate.Value.Year;
                            int dtweekE = rmypWeekTo.SelectedDate.Value.Year;
                            int Byear = DateTime.ParseExact(ddlWeekFrom.SelectedValue, "yyyy-MM-dd", null).AddDays(6).Year;
                            int Eyear = DateTime.ParseExact(ddlWeekTo.SelectedValue, "yyyy-MM-dd", null).Year;
                            int dtweekF = 0;
                            int dtweekT = 0;
                            if (Byear > dtweekB)
                            {
                                dtweekF = ChinaDate.WeekOfYear(DateTime.ParseExact(ddlWeekFrom.SelectedValue, "yyyy-MM-dd", null));
                            }
                            else
                                dtweekF = ChinaDate.WeekOfYear(DateTime.ParseExact(ddlWeekFrom.SelectedValue, "yyyy-MM-dd", null).AddDays(6));
                            if (dtweekE > Eyear)
                            {
                                dtweekT = ChinaDate.WeekOfYear(DateTime.ParseExact(ddlWeekTo.SelectedValue, "yyyy-MM-dd", null).AddDays(6));
                            }
                            else
                                dtweekT = ChinaDate.WeekOfYear(DateTime.ParseExact(ddlWeekTo.SelectedValue, "yyyy-MM-dd", null));

                            //string pointidnew = "";
                            //if (portIds.Length == 1)
                            //{
                            //    pointidnew += portIds[0] + "~" + weekB.ToString() + "~" + weekF.ToString() + "~" + weekE.ToString() + "~" + weekT.ToString() + ";";
                            //    pointidnew += portIds[0] + "~" + dtweekB.ToString() + "~" + dtweekF.ToString() + "~" + dtweekE.ToString() + "~" + dtweekT.ToString() + ";";
                            //}
                            //else
                            //{
                            //    for (int i = 0; i < portIds.Length; i++)
                            //    {
                            //        if (i == 0)
                            //        {
                            //            pointidnew += portIds[i] + "~" + weekB.ToString() + "~" + weekF.ToString() + "~" + weekE.ToString() + "~" + weekT.ToString() + ";";
                            //        }
                            //        else
                            //        {
                            //            pointidnew += portIds[i] + "~" + dtweekB.ToString() + "~" + dtweekF.ToString() + "~" + dtweekE.ToString() + "~" + dtweekT.ToString() + ";";
                            //        }
                            //    }
                            //}
                            //string[] portIdsArry = pointidnew.TrimEnd(';').Split(';');
                            //【给隐藏域赋值，用于显示Chart】
                            SetHiddenData(portIds, factors, weekB + ";" + weekF + ";" + weekE + ";" + weekT + ";" + dtweekB + ";" + dtweekF + ";" + dtweekE + ";" + dtweekT);
                            hdtimeBetween.Value = weekB.ToString() + "~" + weekF.ToString() + "~" + weekE.ToString() + "~" + weekT.ToString() + ";" + dtweekB.ToString() + "~" + dtweekF.ToString() + "~" + dtweekE.ToString() + "~" + dtweekT.ToString();
                            var dataCompare = m_CompareData.GetWeekCompare(portIds, factors.Select(p => p.PollutantCode).ToArray(), weekB, weekF, weekE, weekT, dtweekB, dtweekF, dtweekE, dtweekT, pageSize, pageNo, out recordTotal, tabIndex, orderBy);
                            dataCompare.Sort = "Ordernum desc";
                            gridDataCompare.DataSource = dataCompare;
                        }
                        //默认冻结前四列
                        gridDataCompare.ClientSettings.Scrolling.FrozenColumnsCount = 4;
                    }
                    else
                    {
                        gridDataCompare.DataSource = new DataTable();
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
                             + "|" + timeStr + "|" + "|" + radlDataType.SelectedValue + "|Air1" + "|" + tabStrip.SelectedIndex;
            HiddenChartType.Value = ChartType.SelectedValue;
            HiddenPointFactor.Value = PointFactor.SelectedValue;
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
                if (col.DataField == "Type")
                {
                    col.HeaderText = "类型";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                    col.AllowSorting = false;
                }
                //追加站点
                else if (col.DataField == "PointId")
                {
                    col.HeaderText = "站点";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                    col.AllowSorting = false;
                }
                //追加日期
                else if (col.DataField == "Tstamp")
                {
                    col = (GridDateTimeColumn)e.Column;
                    string tstcolformat = "{0:MM-dd HH:00}";
                    if (SmartEP.Core.Enums.EnumMapping.ParseEnum<PollutantDataType>(radlDataType.SelectedValue) == PollutantDataType.Hour)
                        tstcolformat = "{0:MM-dd HH:00}";
                    col.HeaderText = "日期";
                    col.DataFormatString = tstcolformat;
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(120);
                    col.ItemStyle.Width = Unit.Pixel(120);
                    col.AllowSorting = false;
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
                    col.AllowSorting = false;
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
                    col.AllowSorting = false;
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
                    col.AllowSorting = false;
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
                    col.AllowSorting = false;
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
                    col.AllowSorting = false;
                }
                //追加因子
                else if (factors.Select(x => x.PollutantCode).Contains(col.DataField.Replace("原始", "")) && !factors.Select(x => x.PollutantCode).Contains(col.DataField))
                {
                    int radGridColWidthValue = int.Parse(radGridColWidth.Value.ToString());
                    IPollutant factor = factors.FirstOrDefault(x => x.PollutantCode.Equals(col.DataField.Replace("原始", "")));
                    string strName = factor.PollutantName;
                    if (strName == "PM2.5")
                    {
                        strName = "PM<sub>2.5</sub>";
                    }
                    if (strName == "PM10")
                    {
                        strName = "PM<sub>10</sub>";
                    }
                    string strUnit = factor.PollutantMeasureUnit;
                    if (strUnit == "mg/m³")
                    {
                        strUnit = "mg/m<sup>3</sup>";
                    }
                    if (strUnit == "μg/m³")
                    {
                        strUnit = "μg/m<sup>3</sup>";
                    }
                    col.HeaderText = string.Format("{0}{1}<br>({2})", strName, "原始", strUnit);
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(radGridColWidthValue);
                    col.ItemStyle.Width = Unit.Pixel(radGridColWidthValue);
                    col.AllowSorting = false;
                }
                //追加因子
                else if (factors.Select(x => x.PollutantCode).Contains(col.DataField.Replace("审核", "")) && !factors.Select(x => x.PollutantCode).Contains(col.DataField))
                {
                    int radGridColWidthValue = int.Parse(radGridColWidth.Value.ToString());
                    IPollutant factor = factors.FirstOrDefault(x => x.PollutantCode.Equals(col.DataField.Replace("审核", "")));
                    string strName = factor.PollutantName;
                    if (strName == "PM2.5")
                    {
                        strName = "PM<sub>2.5</sub>";
                    }
                    if (strName == "PM10")
                    {
                        strName = "PM<sub>10</sub>";
                    }
                    string strUnit = factor.PollutantMeasureUnit;
                    if (strUnit == "mg/m³")
                    {
                        strUnit = "mg/m<sup>3</sup>";
                    }
                    if (strUnit == "μg/m³")
                    {
                        strUnit = "μg/m<sup>3</sup>";
                    }
                    col.HeaderText = string.Format("{0}{1}<br>({2})", strName, "审核", strUnit);
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(radGridColWidthValue);
                    col.ItemStyle.Width = Unit.Pixel(radGridColWidthValue);
                    col.AllowSorting = false;
                }
                //追加因子
                else if (factors.Select(x => x.PollutantCode).Contains(col.DataField))
                {
                    int radGridColWidthValue = int.Parse(radGridColWidth.Value.ToString());
                    IPollutant factor = factors.FirstOrDefault(x => x.PollutantCode.Equals(col.DataField));
                    string strName = factor.PollutantName;
                    if (strName == "PM2.5")
                    {
                        strName = "PM<sub>2.5</sub>";
                    }
                    if (strName == "PM10")
                    {
                        strName = "PM<sub>10</sub>";
                    }
                    string strUnit = factor.PollutantMeasureUnit;
                    if (strUnit == "mg/m³")
                    {
                        strUnit = "mg/m<sup>3</sup>";
                    }
                    if (strUnit == "μg/m³")
                    {
                        strUnit = "μg/m<sup>3</sup>";
                    }
                    col.HeaderText = string.Format("{0}<br>({1})", strName, strUnit);
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(radGridColWidthValue);
                    col.ItemStyle.Width = Unit.Pixel(radGridColWidthValue);
                    col.AllowSorting = false;
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
                    string factorCode = factor.PollutantCode;
                    if (tabStrip.SelectedIndex == 0)
                    {
                        GridTableCell cell = (GridTableCell)item[factorCode + "原始"];
                        decimal value = 0M;
                        if (decimal.TryParse(cell.Text, out value))
                        {
                            if (value == -10)
                            {
                                cell.Text = "--";
                            }
                            else
                            {
                                if (factor.PollutantMeasureUnit == "μg/m³")
                                {
                                    value = DecimalExtension.GetPollutantValue(value * 1000, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 0 : Convert.ToInt32(factor.PollutantDecimalNum) - 3);
                                }
                                else
                                {
                                    value = DecimalExtension.GetPollutantValue(value, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum));
                                }

                                cell.Text = value.ToString("");
                            }
                        }
                        GridTableCell cellAudit = (GridTableCell)item[factorCode + "审核"];
                        decimal valueAudit = 0M;
                        if (decimal.TryParse(cellAudit.Text, out valueAudit))
                        {
                            if (valueAudit == -10)
                            {
                                cellAudit.Text = "--";
                            }
                            else
                            {
                                if (factor.PollutantMeasureUnit == "μg/m³")
                                {
                                    valueAudit = DecimalExtension.GetPollutantValue(valueAudit * 1000, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 0 : Convert.ToInt32(factor.PollutantDecimalNum) - 3);
                                }
                                else
                                {
                                    valueAudit = DecimalExtension.GetPollutantValue(valueAudit, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum));
                                }

                                //valueAudit = DecimalExtension.GetPollutantValue(valueAudit, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum));
                                cellAudit.Text = valueAudit.ToString("");
                            }
                        }
                        if (cell.Text != cellAudit.Text)
                        {
                            cell.ForeColor = System.Drawing.Color.Red;
                            cellAudit.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                    else
                    {
                        GridTableCell cellAudit = (GridTableCell)item[factorCode];
                        decimal valueAudit = 0M;
                        if (decimal.TryParse(cellAudit.Text, out valueAudit))
                        {
                            if (valueAudit == -10)
                            {
                                cellAudit.Text = "--";
                            }
                            else
                            {
                                if (factor.PollutantMeasureUnit == "μg/m³")
                                {
                                    valueAudit = DecimalExtension.GetPollutantValue(valueAudit * 1000, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 0 : Convert.ToInt32(factor.PollutantDecimalNum) - 3);
                                }
                                else
                                {
                                    valueAudit = DecimalExtension.GetPollutantValue(valueAudit, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum));
                                }
                                //valueAudit = DecimalExtension.GetPollutantValue(valueAudit, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum));
                                cellAudit.Text = valueAudit.ToString("");
                            }
                        }
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
            if (tabStrip.SelectedIndex == 2)
            {
                int year = 0;
                int hour = 0;
                int yearT = 0;
                int hourT = 0;
                int Index = 0;
                int IndexT = 0;
                switch (radlDataType.SelectedValue)
                {
                    //小时
                    case "Hour":
                        DateTime dtEnd = new DateTime(hourEnd.SelectedDate.Value.Year, hourEnd.SelectedDate.Value.Month, hourEnd.SelectedDate.Value.Day, hourEnd.SelectedDate.Value.Hour, 0, 0);
                        DateTime dtBegin = new DateTime(hourBegin.SelectedDate.Value.Year, hourBegin.SelectedDate.Value.Month, hourBegin.SelectedDate.Value.Day, hourBegin.SelectedDate.Value.Hour, 0, 0);
                        TimeSpan ts = dtEnd - dtBegin;
                        hour = ts.Hours;

                        DateTime dtTo = new DateTime(rdtpHourTo.SelectedDate.Value.Year, rdtpHourTo.SelectedDate.Value.Month, rdtpHourTo.SelectedDate.Value.Day, rdtpHourTo.SelectedDate.Value.Hour, 0, 0);
                        DateTime dtFrom = new DateTime(rdtpHourFrom.SelectedDate.Value.Year, rdtpHourFrom.SelectedDate.Value.Month, rdtpHourFrom.SelectedDate.Value.Day, rdtpHourFrom.SelectedDate.Value.Hour, 0, 0);
                        TimeSpan tsTo = dtTo - dtFrom;
                        hourT = tsTo.Hours;
                        if (hour != hourT)
                        {
                            Alert("基准时间与比对时间范围不相等，请重新选择比对时间！");
                            return;
                        }
                        break;
                    //日 
                    case "Day":
                        TimeSpan tsDayF = dayEnd.SelectedDate.Value - dayBegin.SelectedDate.Value;
                        TimeSpan tsDayT = rdpDayTo.SelectedDate.Value - rdpDayFrom.SelectedDate.Value;
                        if (tsDayF != tsDayT)
                        {
                            Alert("基准时间与比对时间范围不相等，请重新选择比对时间！");
                            return;
                        }
                        break;
                    //月
                    case "Month":
                        TimeSpan tsFrom = monthEnd.SelectedDate.Value - monthBegin.SelectedDate.Value;
                        TimeSpan tsEnd = rmypMonthTo.SelectedDate.Value - rmypMonthFrom.SelectedDate.Value;
                        if (tsFrom != tsEnd)
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
                        TimeSpan tsF = weekEnd.SelectedDate.Value - weekBegin.SelectedDate.Value;
                        TimeSpan tsE = rmypWeekTo.SelectedDate.Value - rmypWeekFrom.SelectedDate.Value;
                        Index = weekTo.SelectedIndex - weekFrom.SelectedIndex;
                        IndexT = ddlWeekTo.SelectedIndex - ddlWeekFrom.SelectedIndex;
                        if (tsF != tsE || Index != IndexT)
                        {
                            Alert("基准时间与比对时间范围不相等，请重新选择比对时间！");
                            return;
                        }
                        break;
                }
            }
            gridDataCompare.CurrentPageIndex = 0;
            gridDataCompare.Rebind();
            //   rgDataCompare.Rebind();
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
        /// Tab切换监站点和参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tabStrip_TabClick(object sender, RadTabStripEventArgs e)
        {
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
            PointFactor.Visible = false;
            radlHour.Visible = false;
            radlDataType.Visible = false;
            divHour.Visible = false;
            divDay.Visible = false;
            divMonth.Visible = false;
            divSeason.Visible = false;
            divYear.Visible = false;
            divWeek.Visible = false;
            factorCbxRsm.Visible = false;
            factor.Visible = false;
            switch (tabStrip.SelectedIndex)
            {
                case 0:
                    divSame.Visible = true;
                    radlHour.Visible = true;
                    factor.Visible = false;
                    factorCbxRsm.Visible = true;
                    switch (radlHour.SelectedValue)
                    {
                        case "Hour":
                            divHour.Visible = true;
                            break;
                    }
                    break;
                //相同时段多个站点多个参数
                case 1:
                    divSame.Visible = true;
                    PointFactor.Visible = true;
                    radlDataType.Visible = true;
                    factor.Visible = true;
                    factorCbxRsm.Visible = false;
                    switch (radlDataType.SelectedValue)
                    {
                        case "Hour":
                            divHour.Visible = true;
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
                    break;
                //可变时段多个站点同一参数
                case 2:
                    divMul.Visible = true;
                    divMulti.Visible = true;
                    divMultiTime.Visible = true;
                    PointFactor.Visible = false;
                    radlDataType.Visible = true;
                    factor.Visible = true;
                    factorCbxRsm.Visible = false;
                    switch (radlDataType.SelectedValue)
                    {
                        //小时
                        case "Hour":
                            divHour.Visible = true;
                            divHourT.Visible = true;
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
            divHour.Visible = false;
            divDay.Visible = false;
            divMonth.Visible = false;
            divSeason.Visible = false;
            divYear.Visible = false;
            divWeek.Visible = false;
            //不同时段时间框
            if (tabStrip.SelectedIndex == 2)
            {
                divHourT.Visible = false;
                divDayT.Visible = false;
                divMonthT.Visible = false;
                divSeasonT.Visible = false;
                divYearT.Visible = false;
                divWeekT.Visible = false;
                switch (radlDataType.SelectedValue)
                {
                    //小时
                    case "Hour":
                        divHour.Visible = true;
                        divHourT.Visible = true;
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
            else if (tabStrip.SelectedIndex == 1)
            {
                switch (radlDataType.SelectedValue)
                {
                    case "Hour":
                        divHour.Visible = true;
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
            else if (tabStrip.SelectedIndex == 0)
            {
                switch (radlHour.SelectedValue)
                {
                    case "Hour":
                        divHour.Visible = true;
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
            BindWeekFComboBox();
            int year = (weekEnd.SelectedDate.Value.Year) - (weekBegin.SelectedDate.Value.Year);
            int month = (weekEnd.SelectedDate.Value.Month) - (weekBegin.SelectedDate.Value.Month);
            rmypWeekTo.SelectedDate = rmypWeekFrom.SelectedDate.Value.AddYears(+year);
            rmypWeekTo.SelectedDate = rmypWeekTo.SelectedDate.Value.AddMonths(+month);
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
            BindWeekTComboBox();
            int year = (weekEnd.SelectedDate.Value.Year) - (weekBegin.SelectedDate.Value.Year);
            int month = (weekEnd.SelectedDate.Value.Month) - (weekBegin.SelectedDate.Value.Month);
            rmypWeekFrom.SelectedDate = rmypWeekTo.SelectedDate.Value.AddYears(-year);
            rmypWeekFrom.SelectedDate = rmypWeekFrom.SelectedDate.Value.AddMonths(-month);
        }
        protected void ddlWeekTo_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            int index = weekTo.SelectedIndex - weekFrom.SelectedIndex;
            ddlWeekFrom.SelectedIndex = ddlWeekTo.SelectedIndex - index;
        }
        #endregion


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
            BindWeekBComboBox();
        }

        protected void weekEnd_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            BindWeekEComboBox();
        }
        /// <summary>
        /// 绑定周
        /// </summary>
        private void BindWeekBComboBox()
        {
            if (weekBegin.SelectedDate > System.DateTime.Now)
            {
                Alert("选择时间必须小于等于当前时间！");
                return;
            }
            weekFrom.DataValueField = "value";
            weekFrom.DataTextField = "text";
            weekFrom.DataSource = ChinaDate.GetWeekOfMonth(weekBegin.SelectedDate.Value);
            weekFrom.DataBind();
        }

        private void BindWeekEComboBox()
        {
            if (weekEnd.SelectedDate > System.DateTime.Now)
            {
                Alert("选择时间必须小于等于当前时间！");
                return;
            }
            weekTo.DataValueField = "value";
            weekTo.DataTextField = "text";
            weekTo.DataSource = ChinaDate.GetWeekOfMonth(weekEnd.SelectedDate.Value);
            weekTo.DataBind();
        }
        private void BindWeekFComboBox()
        {
            if (rmypWeekFrom.SelectedDate > System.DateTime.Now)
            {
                Alert("选择时间必须小于等于当前时间！");
                return;
            }
            ddlWeekFrom.DataValueField = "value";
            ddlWeekFrom.DataTextField = "text";
            ddlWeekFrom.DataSource = ChinaDate.GetWeekOfMonth(rmypWeekFrom.SelectedDate.Value);
            ddlWeekFrom.DataBind();
        }

        private void BindWeekTComboBox()
        {
            if (rmypWeekTo.SelectedDate > System.DateTime.Now)
            {
                Alert("选择时间必须小于等于当前时间！");
                return;
            }
            ddlWeekTo.DataValueField = "value";
            ddlWeekTo.DataTextField = "text";
            ddlWeekTo.DataSource = ChinaDate.GetWeekOfMonth(rmypWeekTo.SelectedDate.Value);
            ddlWeekTo.DataBind();
        }
        #endregion

        protected void PointFactor_SelectedIndexChanged(object sender, EventArgs e)
        {
            HiddenPointFactor.Value = PointFactor.SelectedValue;
            RegisterScript("PointFactor('" + PointFactor.SelectedValue + "');");
        }
    }
}