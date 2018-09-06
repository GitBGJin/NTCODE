﻿using SmartEP.Utilities.Office;
using SmartEP.Service.AutoMonitoring.Air;
using SmartEP.Service.AutoMonitoring.Interfaces;
using SmartEP.Service.Core.Enums;
using SmartEP.WebControl.CbxRsm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SmartEP.Core.Enums;
using SmartEP.Core.Interfaces;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.DomainModel.BaseData;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Service.BaseData.BusinessRule;
using System.Collections;
using System.ComponentModel;
using SmartEP.Core.Generic;
using SmartEP.Service.Frame;
using SmartEP.DomainModel;
using SmartEP.WebUI.Pages.EnvAir.ChartAjaxRequest;
using SmartEP.Utilities.Web.UI;
using SmartEP.Utilities.Calendar;
using SmartEP.Service.DataAnalyze.Air.DataQuery;
using System.Text.RegularExpressions;


namespace SmartEP.WebUI.Pages.EnvAir.DataAnalyze
{
    public partial class OriginalScatter : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        private IInfectantDALService g_IInfectantDALService = null;
        InfectantByDayService m_DayOriData = Singleton<InfectantByDayService>.GetInstance();
        InfectantBy60Service m_HourOriData = Singleton<InfectantBy60Service>.GetInstance();
        InfectantByMonthService m_MonthOriData = Singleton<InfectantByMonthService>.GetInstance();
        DataQueryByHourService m_HourData = Singleton<DataQueryByHourService>.GetInstance();
        DataQueryByDayService m_DayData = Singleton<DataQueryByDayService>.GetInstance();
        DataQueryByWeekService m_WeekData = Singleton<DataQueryByWeekService>.GetInstance();
        DataQueryByMonthService m_MonthData = Singleton<DataQueryByMonthService>.GetInstance();
        DataQueryBySeasonService m_SeasonData = Singleton<DataQueryBySeasonService>.GetInstance();
        DataQueryByYearService m_YearData = Singleton<DataQueryByYearService>.GetInstance();

        InstrumentChannelService m_InstrumentChannelService = new InstrumentChannelService();
        //获取因子上下限数据处理服务
        ExcessiveSettingService m_ExcessiveSettingService = new ExcessiveSettingService();
        //代码项服务层
        DictionaryService dicService = new DictionaryService();
        /// <summary>
        /// 选择因子
        /// </summary>
        private IList<IPollutant> factors = null;

        /// <summary>
        /// 选择站点
        /// </summary>
        private IList<IPoint> points = null;
        string LZSPfactor = string.Empty;
        string LZSfactorName = string.Empty;
        /// <summary>
        /// 上下限
        /// </summary>
        DataTable dtExcessive = null;
        /// <summary>
        /// 国家标记位
        /// </summary>
        IQueryable<V_CodeMainItemEntity> siteTypeEntites = null;
        /// <summary>
        /// 统计
        /// </summary>
        DataView dvStatistical = null;
        //因子
        SmartEP.Service.BaseData.Channel.AirPollutantService m_AirPollutantService = Singleton<SmartEP.Service.BaseData.Channel.AirPollutantService>.GetInstance();


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
            string PointName = PageHelper.GetQueryString("PointName");
            string DateBegin = PageHelper.GetQueryString("DTBegin");
            string DateEnd = PageHelper.GetQueryString("DTEnd");
            string factors = PageHelper.GetQueryString("Factors");
            if (PointName != "")
            {
                pointCbxRsm.SetPointValuesFromNames(PointName);
                factorCbxRsm.SetFactorValuesFromCodes(factors);
                dtpBegin.SelectedDate = Convert.ToDateTime(DateBegin);
                dtpEnd.SelectedDate = Convert.ToDateTime(DateEnd);
            }
            else
            {
                string pollutantCode = "a34002;a34004;";
                factorCbxRsm.SetFactorValuesFromCodes(pollutantCode);
                dtpBegin.SelectedDate = DateTime.Now.AddDays(-1);
                dtpEnd.SelectedDate = DateTime.Now;
                Session["X"] = "a34002";
                Session["Y"] = "a34004";
            }
            
            //数据类型
            radlDataType.Items.Add(new ListItem("小时", PollutantDataType.Hour.ToString()));
            radlDataType.Items.Add(new ListItem("日", PollutantDataType.Day.ToString()));
            radlDataType.Items.Add(new ListItem("周", PollutantDataType.Week.ToString()));
            radlDataType.Items.Add(new ListItem("月", PollutantDataType.Month.ToString()));
            radlDataType.Items.Add(new ListItem("季", PollutantDataType.Season.ToString()));
            radlDataType.Items.Add(new ListItem("年", PollutantDataType.Year.ToString()));

            radlDataTypeOri.Items.Add(new ListItem("一分钟", PollutantDataType.Min1.ToString()));
            radlDataTypeOri.Items.Add(new ListItem("五分钟", PollutantDataType.Min5.ToString()));
            radlDataTypeOri.Items.Add(new ListItem("小时", PollutantDataType.Min60.ToString()));
            radlDataTypeOri.Items.Add(new ListItem("日", PollutantDataType.OriDay.ToString()));
            radlDataTypeOri.Items.Add(new ListItem("月", PollutantDataType.OriMonth.ToString()));

            radlDataType.SelectedValue = PollutantDataType.Hour.ToString();
            radlDataTypeOri.SelectedValue = PollutantDataType.Min60.ToString();

            //时间框初始化
            hourBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddHours(-47));
            hourEnd.SelectedDate = DateTime.Now;
            dtpBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm"));
            dtpEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            dayBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd"));
            dayEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
            monthBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddYears(-1).ToString("yyyy-MM"));
            monthEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToString("yyyy-MM"));
            seasonBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"));
            seasonEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"));
            weekBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"));
            weekEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"));
            yearBegin.SelectedDate = DateTime.Now.AddYears(-5);
            yearEnd.SelectedDate = DateTime.Now.AddYears(-1);

            BindWeekFComboBox();
            BindWeekTComboBox();

            //dtpHour.Visible = true;
            //dbtHour.Visible = false;
            //dbtDay.Visible = false;
            //dbtMonth.Visible = false;
            //dbtSeason.Visible = false;
            //dbtYear.Visible = false;
            //dbtWeek.Visible = false;
        }
        #endregion
        #region 绑定因子
        public void BindFactors(string CategoryUid, out string Name, out string code, string type = "name")
        {
            IQueryable<PollutantCodeEntity> Pollutant = m_AirPollutantService.RetrieveList().Where(x => x.CategoryUid == CategoryUid);
            string PollutantName = "";
            string PollutantCode = "";

            string[] pollutantarry = Pollutant.Select(p => p.PollutantName).ToArray();
            foreach (string strName in pollutantarry)
            {
                PollutantName += strName + ";";
            }
            string[] pollutantCodearry = Pollutant.Select(p => p.PollutantCode).ToArray();
            foreach (string strName in pollutantCodearry)
            {
                PollutantCode += strName + ";";
            }
            //}
            Name = PollutantName;
            code = PollutantCode;
        }
        #endregion
        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {
            BindFactors("979f05bb-f730-4285-8aee-fafdce1360e2", out LZSfactorName, out LZSPfactor);
            //数据类型对应接口初始化

            if (radlDataTypeOri.SelectedValue == "Min1" || radlDataTypeOri.SelectedValue == "Min5" || radlDataTypeOri.SelectedValue == "Min60")
            {
                g_IInfectantDALService = MonitoringDataAir.GetInfectantDALService(SmartEP.Core.Enums.EnumMapping.ParseEnum<PollutantDataType>(radlDataTypeOri.SelectedValue));
            }

            string[] factorCodes = factorCbxRsm.GetFactorValues(SmartEP.Core.Enums.CbxRsmReturnType.Code);

            points = pointCbxRsm.GetPoints();
            string[] portIds = pointCbxRsm.GetPointValues(CbxRsmReturnType.ID);
            factors = factorCbxRsm.GetFactors();

            //生成RadGrid的绑定列
            dvStatistical = null;
            //是否显示统计行
            if (IsStatistical.Checked && factors.Count != 0)
            {
                DateTime dtBegion = dtpBegin.SelectedDate.Value;
                DateTime dtEnd = dtpEnd.SelectedDate.Value;
                gridOriginal.ShowFooter = true;
                dvStatistical = g_IInfectantDALService.GetStatisticalData(portIds, factors, dtBegion, dtEnd);
            }
            else
            {
                gridOriginal.ShowFooter = false;
            }

            //每页显示数据个数            
            int pageSize = gridOriginal.PageSize;
            //当前页的序号
            int pageNo = gridOriginal.CurrentPageIndex;

            //数据总行数
            int recordTotal = 0;
            DataTable dt = new DataTable();

            if (portIds != null && factors != null)
            {
                if (tabStrip.SelectedIndex == 2 || tabStrip.SelectedIndex == 3)
                {
                    if (ddlDataSource.SelectedValue == "OriData")
                    {
                        DateTime dtBegion = Convert.ToDateTime(dayBegin.SelectedDate.Value.ToString("yyyy-MM-dd 00:00:00"));
                        DateTime dtEnd = Convert.ToDateTime(dayEnd.SelectedDate.Value.ToString("yyyy-MM-dd 23:59:59"));
                        int days = (dtEnd - dtBegion).Days + 1;
                        string orderby = "Tstamp asc";
                        var monitorData = m_HourOriData.GetHourAvgData(portIds, factors.Select(p => p.PollutantCode).ToArray(), dtBegion, dtEnd, "Min60", 99999, 0, out recordTotal, orderby);
                        dt.Columns.Add("PointId", typeof(int));
                        dt.Columns.Add("Hours", typeof(string));
                        foreach (string fac in factorCodes)
                        {
                            dt.Columns.Add(fac, typeof(string));
                        }
                        for (int i = 0; i < 24; i++)
                        {
                            dt.Rows.Add(dt.NewRow());
                        }
                        foreach (string fac in factorCodes)
                        {
                            for (int i = 0; i < 24; i++)
                            {
                                int daycount = (dtEnd - dtBegion).Days + 1;
                                decimal d = 0;  //某因子某整点总和计数器
                                for (int j = 0; j < days; j++)
                                {
                                    monitorData.RowFilter = "Tstamp>='" + dtBegion.AddDays(j).AddHours(i) + "' and Tstamp<'" + dtBegion.AddDays(j).AddHours(i + 1) + "'";
                                    string facValue = "";
                                    if (monitorData.Count <= 0)
                                    {
                                        facValue = "0";
                                    }
                                    else
                                    {
                                        //facValue = (monitorData[j * 24 + i][fac] != null && monitorData[j * 24 + i][fac].ToString().Trim() != "") ? monitorData[j * 24 + i][fac].ToString() : "0";
                                        facValue = (monitorData[0][fac] != DBNull.Value && monitorData[0][fac].ToString().Trim() != "") ? monitorData[0][fac].ToString() : "0";
                                    }
                                    if (facValue == "0") daycount--;
                                    d += Convert.ToDecimal(facValue);
                                    monitorData.RowFilter = "";
                                }
                                decimal? avg = 0;
                                if (daycount > 0)
                                {
                                    avg = d / daycount;
                                    dt.Rows[i][fac] = avg.ToString();
                                }
                                else
                                {
                                    avg = null;
                                }
                            }
                        }
                        for (int i = 0; i < 24; i++)
                        {
                            if (portIds.Length > 1)
                                dt.Rows[i]["PointId"] = "0";
                            else
                                dt.Rows[i]["PointId"] = portIds[0].ToString();
                            dt.Rows[i]["Hours"] = i + "时";
                        }
                        gridOriginal.DataSource = dt;

                        List<string> ipnames = new List<string>();
                        foreach (IPollutant ip in factors)
                        {
                            string ipname = ip.PollutantCode;
                            ipnames.Add(ipname);
                        }
                        string names = string.Join(",", ipnames.ToArray());
                        hddev.Value = string.Join(",", portIds.ToArray()) + "|" + names + "|" + dayBegin.SelectedDate.Value.ToString("yyyy-MM-dd 00:00:00") + "|" + dayEnd.SelectedDate.Value.ToString("yyyy-MM-dd 23:59:59") + "|Ori|" + RadioButtonList1.SelectedValue;
                    }
                    if (ddlDataSource.SelectedValue == "AuditData")
                    {
                        DateTime dtBegion = Convert.ToDateTime(dayBegin.SelectedDate.Value.ToString("yyyy-MM-dd 00:00:00"));
                        DateTime dtEnd = Convert.ToDateTime(dayEnd.SelectedDate.Value.ToString("yyyy-MM-dd 23:59:59"));
                        int days = (dtEnd - dtBegion).Days + 1;
                        string orderby = "Tstamp asc";
                        var monitorData = m_HourData.GetHourDataAvg(portIds, factors.Select(p => p.PollutantCode).ToArray(), dtBegion, dtEnd, 99999, 0, out recordTotal, orderby);
                        //var monitorData = MonitoringDataAir.GetInfectantDALService(SmartEP.Core.Enums.EnumMapping.ParseEnum<PollutantDataType>("Min60")).GetDataPager(portIds, factors, dtBegion, dtEnd, 99999, 0, out recordTotal, orderby);
                        dt.Columns.Add("PointId", typeof(int));
                        dt.Columns.Add("Hours", typeof(string));
                        foreach (string fac in factorCodes)
                        {
                            dt.Columns.Add(fac, typeof(string));
                        }
                        for (int i = 0; i < 24; i++)
                        {
                            dt.Rows.Add(dt.NewRow());
                        }
                        foreach (string fac in factorCodes)
                        {
                            for (int i = 0; i < 24; i++)
                            {
                                int daycount = (dtEnd - dtBegion).Days + 1;
                                decimal d = 0;  //某因子某整点总和计数器
                                for (int j = 0; j < days; j++)
                                {
                                    monitorData.RowFilter = "Tstamp>='" + dtBegion.AddDays(j).AddHours(i) + "' and Tstamp<'" + dtBegion.AddDays(j).AddHours(i + 1) + "'";
                                    string facValue = "";
                                    if (monitorData.Count <= 0)
                                    {
                                        facValue = "0";
                                    }
                                    else
                                    {
                                        //facValue = (monitorData[j * 24 + i][fac] != null && monitorData[j * 24 + i][fac].ToString().Trim() != "") ? monitorData[j * 24 + i][fac].ToString() : "0";
                                        facValue = (monitorData[0][fac] != DBNull.Value && monitorData[0][fac].ToString().Trim() != "") ? monitorData[0][fac].ToString() : "0";
                                    }
                                    if (facValue == "0") daycount--;
                                    d += Convert.ToDecimal(facValue);
                                    monitorData.RowFilter = "";
                                }
                                decimal? avg = 0;
                                if (daycount > 0)
                                {
                                    avg = d / daycount;
                                    dt.Rows[i][fac] = avg.ToString();
                                }
                                else
                                {
                                    avg = null;
                                }
                            }
                        }
                        for (int i = 0; i < 24; i++)
                        {
                            if (portIds.Length > 1)
                                dt.Rows[i]["PointId"] = "0";
                            else
                                dt.Rows[i]["PointId"] = portIds[0].ToString();
                            dt.Rows[i]["Hours"] = i + "时";
                        }
                        gridOriginal.DataSource = dt;

                        List<string> ipnames = new List<string>();
                        foreach (IPollutant ip in factors)
                        {
                            string ipname = ip.PollutantCode;
                            ipnames.Add(ipname);
                        }
                        string names = string.Join(",", ipnames.ToArray());
                        hddev.Value = string.Join(",", portIds.ToArray()) + "|" + names + "|" + dayBegin.SelectedDate.Value.ToString("yyyy-MM-dd 00:00:00") + "|" + dayEnd.SelectedDate.Value.ToString("yyyy-MM-dd 23:59:59") + "|Audit|" + RadioButtonList1.SelectedValue;
                    }
                }
                else
                {
                    if (ddlDataSource.SelectedValue == "OriData")
                    {
                        DateTime dtBegion = dtpBegin.SelectedDate.Value;
                        DateTime dtEnd = dtpEnd.SelectedDate.Value;
                        if (radlDataTypeOri.SelectedValue == "OriDay")
                        {
                            //dtBegion = Convert.ToDateTime(dayBegin.SelectedDate.Value.ToString("yyyy-MM-dd 00:00:00"));
                            //dtEnd = Convert.ToDateTime(dayEnd.SelectedDate.Value.ToString("yyyy-MM-dd 23:59:59"));
                            dtBegion = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd 00:00:00"));
                            dtEnd = Convert.ToDateTime(dtpEnd.SelectedDate.Value.ToString("yyyy-MM-dd 23:59:59"));
                            string orderby = "DateTime desc";
                            if (TimeSort.SelectedValue == "时间升序")
                                orderby = "DateTime asc";
                            var monitorData = m_DayOriData.GetDayAvgData(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderby);
                            gridOriginal.DataSource = monitorData;
                            //【给隐藏域赋值，用于显示Chart】
                            SetHiddenData(portIds, factors, dtBegion, dtEnd);
                        }
                        if (radlDataTypeOri.SelectedValue == "OriMonth")
                        {
                            dtBegion = dtpBegin.SelectedDate.Value;
                            dtEnd = dtpEnd.SelectedDate.Value;
                            string orderby = "Year desc,MonthOfYear desc";
                            if (TimeSort.SelectedValue == "时间升序")
                                orderby = "Year asc,MonthOfYear asc";
                            var monitorData = m_MonthOriData.GetOriDataAvg(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderby);
                            gridOriginal.DataSource = monitorData;
                            //【给隐藏域赋值，用于显示Chart】
                            SetHiddenData(portIds, factors, dtBegion, dtEnd);
                        }
                        if (radlDataTypeOri.SelectedValue == "Min1" || radlDataTypeOri.SelectedValue == "Min5")
                        {
                            string orderby = "Tstamp desc";
                            if (TimeSort.SelectedValue == "时间升序")
                                orderby = "Tstamp asc";
                            var monitorData = m_HourOriData.GetHourAvgData(portIds, factorCodes, dtBegion, dtEnd, radlDataTypeOri.SelectedValue, pageSize, pageNo, out recordTotal, orderby);
                            gridOriginal.DataSource = monitorData;
                            //【给隐藏域赋值，用于显示Chart】
                            SetHiddenData(portIds, factors, dtBegion, dtEnd);
                        }
                        if (radlDataTypeOri.SelectedValue == "Min60")
                        {
                            //dtBegion = Convert.ToDateTime(hourBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:00"));
                            //dtEnd = Convert.ToDateTime(hourEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:00"));

                            dtBegion = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:00"));
                            dtEnd = Convert.ToDateTime(dtpEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:00"));
                            string orderby = "Tstamp desc";
                            if (TimeSort.SelectedValue == "时间升序")
                                orderby = "Tstamp asc";
                            var monitorData = m_HourOriData.GetHourAvgData(portIds, factorCodes, dtBegion, dtEnd, radlDataTypeOri.SelectedValue, pageSize, pageNo, out recordTotal, orderby);
                            gridOriginal.DataSource = monitorData;
                            //【给隐藏域赋值，用于显示Chart】
                            SetHiddenData(portIds, factors, dtBegion, dtEnd);
                        }
                        List<string> ipnames = new List<string>();
                        foreach (IPollutant ip in factors)
                        {
                            string ipname = ip.PollutantCode;
                            ipnames.Add(ipname);
                        }
                        string names = string.Join(",", ipnames.ToArray());
                        hddev.Value = string.Join(",", portIds.ToArray()) + "|" + names + "|" + dayBegin.SelectedDate.Value.ToString("yyyy-MM-dd 00:00:00") + "|" + dayEnd.SelectedDate.Value.ToString("yyyy-MM-dd 23:59:59") + "|Ori|" + RadioButtonList1.SelectedValue;
                    }
                    if (ddlDataSource.SelectedValue == "AuditData")
                    {
                        //小时数据
                        if (radlDataType.SelectedValue == "Hour")
                        {
                            DateTime dtBegion = Convert.ToDateTime(hourBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:00"));
                            DateTime dtEnd = Convert.ToDateTime(hourEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:00"));
                            //【给隐藏域赋值，用于显示Chart】
                            SetHiddenData(portIds, factors, dtBegion, dtEnd);
                            string orderBy = "Tstamp desc";
                            if (TimeSort.SelectedValue == "时间升序")
                                orderBy = "Tstamp asc";
                            var auditData = m_HourData.GetHourDataAvg(portIds, factors.Select(p => p.PollutantCode).ToArray(), dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
                            gridOriginal.DataSource = auditData;

                        }
                        //日数据
                        else if (radlDataType.SelectedValue == "Day")
                        {
                            DateTime dtBegion = dayBegin.SelectedDate.Value;
                            DateTime dtEnd = dayEnd.SelectedDate.Value;
                            //【给隐藏域赋值，用于显示Chart】
                            SetHiddenData(portIds, factors, dtBegion, dtEnd);
                            string orderBy = "DateTime desc";
                            if (TimeSort.SelectedValue == "时间升序")
                                orderBy = "DateTime asc";
                            var auditData = m_DayData.GetDayDataAvg(portIds, factors.Select(p => p.PollutantCode).ToArray(), dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
                            gridOriginal.DataSource = auditData;

                        }
                        //月数据
                        else if (radlDataType.SelectedValue == "Month")
                        {
                            int monthB = monthBegin.SelectedDate.Value.Year;
                            int monthE = monthEnd.SelectedDate.Value.Year;
                            int monthF = monthBegin.SelectedDate.Value.Month;
                            int monthT = monthEnd.SelectedDate.Value.Month;
                            //【给隐藏域赋值，用于显示Chart】
                            SetHiddenData(portIds, factors, monthB + ";" + monthF + ";" + monthE + ";" + monthT);
                            string orderBy = "Year desc,MonthOfYear desc";
                            if (TimeSort.SelectedValue == "时间升序")
                                orderBy = "Year asc,MonthOfYear asc";
                            var auditData = m_MonthData.GetMonthDataAvg(portIds, factors.Select(p => p.PollutantCode).ToArray(), monthB, monthF, monthE, monthT, pageSize, pageNo, out recordTotal, orderBy);
                            gridOriginal.DataSource = auditData;
                        }
                        //季数据
                        else if (radlDataType.SelectedValue == "Season")
                        {
                            int seasonB = seasonBegin.SelectedDate.Value.Year;
                            int seasonE = seasonEnd.SelectedDate.Value.Year;
                            int seasonF = Convert.ToInt32(seasonFrom.SelectedValue);
                            int seasonT = Convert.ToInt32(seasonTo.SelectedValue);
                            //【给隐藏域赋值，用于显示Chart】
                            SetHiddenData(portIds, factors, seasonB + ";" + seasonF + ";" + seasonE + ";" + seasonT);
                            string orderBy = "Year desc,SeasonOfYear desc";
                            if (TimeSort.SelectedValue == "时间升序")
                                orderBy = "Year asc,SeasonOfYear asc";
                            var auditData = m_SeasonData.GetSeasonDataAvg(portIds, factors.Select(p => p.PollutantCode).ToArray(), seasonB, seasonF, seasonE, seasonT, pageSize, pageNo, out recordTotal, orderBy);
                            gridOriginal.DataSource = auditData;


                        }
                        //年数据
                        else if (radlDataType.SelectedValue == "Year")
                        {

                            int yearB = yearBegin.SelectedDate.Value.Year;
                            int yearE = yearEnd.SelectedDate.Value.Year;
                            //【给隐藏域赋值，用于显示Chart】
                            SetHiddenData(portIds, factors, yearB + ";" + yearE);
                            string orderBy = "Year desc";
                            if (TimeSort.SelectedValue == "时间升序")
                                orderBy = "Year asc";
                            var auditData = m_YearData.GetYearDataAvg(portIds, factors.Select(p => p.PollutantCode).ToArray(), yearB, yearE, pageSize, pageNo, out recordTotal, orderBy);
                            gridOriginal.DataSource = auditData;

                        }
                        //周数据
                        else if (radlDataType.SelectedValue == "Week")
                        {
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
                            //【给隐藏域赋值，用于显示Chart】
                            SetHiddenData(portIds, factors, weekB + ";" + weekF + ";" + weekE + ";" + weekT);
                            string orderBy = "Year desc,WeekOfYear desc";
                            if (TimeSort.SelectedValue == "时间升序")
                                orderBy = "Year asc,WeekOfYear asc";
                            var auditData = m_WeekData.GetWeekDataAvg(portIds, factors.Select(p => p.PollutantCode).ToArray(), weekB, weekF, weekE, weekT, pageSize, pageNo, out recordTotal, orderBy);
                            gridOriginal.DataSource = auditData;

                        }
                        List<string> ipnames = new List<string>();
                        foreach (IPollutant ip in factors)
                        {
                            string ipname = ip.PollutantCode;
                            ipnames.Add(ipname);
                        }
                        string names = string.Join(",", ipnames.ToArray());
                        hddev.Value = string.Join(",", portIds.ToArray()) + "|" + names + "|" + dayBegin.SelectedDate.Value.ToString("yyyy-MM-dd 00:00:00") + "|" + dayEnd.SelectedDate.Value.ToString("yyyy-MM-dd 23:59:59") + "|Audit|" + RadioButtonList1.SelectedValue;
                    }
                }
            }
            else
            {
                gridOriginal.DataSource = null;
            }
            //数据总行数
            if (tabStrip.SelectedIndex == 2 || tabStrip.SelectedIndex == 3)
            {
                gridOriginal.VirtualItemCount = dt.Rows.Count;
            }
            else
            {
                gridOriginal.VirtualItemCount = recordTotal;
            }
            //获取上下限
            string DataTypeUid = "1b6367f1-5287-4c14-b120-7a35bd176db1";
            IQueryable<ExcessiveSettingInfo> Excessive = m_ExcessiveSettingService.RetrieveListByPointAndFactor(ApplicationValue.Air, factorCodes, portIds, DataTypeUid);
            dtExcessive = ConvertToDataTable(Excessive);
            //国家数据标记位
            siteTypeEntites = dicService.RetrieveList(DictionaryType.AMS, "国家数据标记");
        }
        #endregion

        #region 服务器端控件事件处理
        /// <summary>
        /// 绑定数据源（用于分页、排序）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridOriginal_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }

        private void BindChart()
        {
            if (ShowType.Text.Equals("分屏"))
            {
                RegisterScript("InitGroupChart();");
            }
            else if (ShowType.Text.Equals("合并"))
            {
                RegisterScript("InitTogetherChart();");
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
                                 + "|" + dtBegin + "|" + dtEnd + "|" + radlDataTypeOri.SelectedValue + "|Air";
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
                                 + "|" + timeStr + "|" + "|" + radlDataTypeOri.SelectedValue + "|Air";
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
        /// 数据行绑定处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridOriginal_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                DataRowView drv = e.Item.DataItem as DataRowView;
                if (item["PointId"] != null)
                {
                    GridTableCell pointCell = (GridTableCell)item["PointId"];
                    if (pointCell.Text == "0")
                    {
                        pointCell.Text = "多站点均值";
                    }
                    else
                    {
                        IPoint point = points.FirstOrDefault(x => x.PointID.Equals(pointCell.Text.Trim()));
                        if (points != null)
                            pointCell.Text = point.PointName;

                    }
                }
                for (int iRow = 0; iRow < factors.Count; iRow++)
                {
                    string siteTypeName = "--";//标记位名称
                    IPollutant factor = factors[iRow];
                    GridTableCell cell = (GridTableCell)item[factor.PollutantCode];
                    //string factorStatus = drv[factor.PollutantCode + "_Status"] != DBNull.Value ? drv[factor.PollutantCode + "_Status"].ToString() : string.Empty;
                    //string factorMark = drv[factor.PollutantCode + "_Mark"] != DBNull.Value ? drv[factor.PollutantCode + "_Mark"].ToString() : string.Empty;
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
                    }
                }
            }
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
        /// 查询按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gridOriginal.CurrentPageIndex = 0;
            gridOriginal.Rebind();
            if (tabStrip.SelectedTab.Text == "图表")
            {
                BindChart();
            }
            else if (tabStrip.SelectedTab.Text == "日变化趋势")
            {
                RegisterScript("createDayDev();");
            }
            else if (tabStrip.SelectedTab.Text == "列表")
            {
                FirstLoadChart.Value = "1";
            }
            else if (tabStrip.SelectedTab.Text == "日变化")
            {
                FirstLoadChart2.Value = "1";
            }
        }

        /// <summary>
        /// ToolBar事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridRTB_ButtonClick(object sender, RadToolBarEventArgs e)
        {
            Telerik.Web.UI.RadToolBarButton button = (Telerik.Web.UI.RadToolBarButton)e.Item;
            if (button.CommandName == "ExportToExcel")
            {

                BindFactors("979f05bb-f730-4285-8aee-fafdce1360e2", out LZSfactorName, out LZSPfactor);
                //数据类型对应接口初始化
                if (radlDataTypeOri.SelectedValue == "Min1" || radlDataTypeOri.SelectedValue == "Min5" || radlDataTypeOri.SelectedValue == "Min60")
                {
                    g_IInfectantDALService = MonitoringDataAir.GetInfectantDALService(SmartEP.Core.Enums.EnumMapping.ParseEnum<PollutantDataType>(radlDataType.SelectedValue));
                }

                string[] factorCodes = factorCbxRsm.GetFactorValues(SmartEP.Core.Enums.CbxRsmReturnType.Code);

                points = pointCbxRsm.GetPoints();
                string[] portIds = pointCbxRsm.GetPointValues(CbxRsmReturnType.ID);
                factors = factorCbxRsm.GetFactors();

                //生成RadGrid的绑定列
                dvStatistical = null;
                //是否显示统计行
                if (IsStatistical.Checked && factors.Count != 0)
                {
                    DateTime dtBegion = dtpBegin.SelectedDate.Value;
                    DateTime dtEnd = dtpEnd.SelectedDate.Value;
                    gridOriginal.ShowFooter = true;
                    dvStatistical = g_IInfectantDALService.GetStatisticalData(portIds, factors, dtBegion, dtEnd);
                }
                else
                {
                    gridOriginal.ShowFooter = false;
                }

                //每页显示数据个数            
                int pageSize = gridOriginal.PageSize;
                //当前页的序号
                int pageNo = gridOriginal.CurrentPageIndex;

                //数据总行数
                int recordTotal = 0;
                DataView dv = new DataView();
                DataTable dt = new DataTable();

                if (portIds != null && factors != null)
                {
                    if (tabStrip.SelectedIndex == 2)
                    {
                        DataTable dtForDay = new DataTable();
                        DateTime dtBegion = Convert.ToDateTime(dayBegin.SelectedDate.Value.ToString("yyyy-MM-dd 00:00:00"));
                        DateTime dtEnd = Convert.ToDateTime(dayEnd.SelectedDate.Value.ToString("yyyy-MM-dd 23:59:59"));
                        int days = (dtEnd - dtBegion).Days + 1;
                        if (ddlDataSource.SelectedValue == "OriData")
                        {
                            string orderby = "Tstamp asc";
                            var monitorData = m_HourOriData.GetHourAvgData(portIds, factors.Select(p => p.PollutantCode).ToArray(), dtBegion, dtEnd, "Min60", 99999, 0, out recordTotal, orderby);
                            dtForDay.Columns.Add("PointId", typeof(int));
                            dtForDay.Columns.Add("Hours", typeof(string));
                            foreach (string fac in factorCodes)
                            {
                                dtForDay.Columns.Add(fac, typeof(string));
                            }
                            for (int i = 0; i < 24; i++)
                            {
                                dtForDay.Rows.Add(dtForDay.NewRow());
                            }
                            foreach (string fac in factorCodes)
                            {
                                for (int i = 0; i < 24; i++)
                                {
                                    int daycount = (dtEnd - dtBegion).Days + 1;
                                    decimal d = 0;  //某因子某整点总和计数器
                                    for (int j = 0; j < days; j++)
                                    {
                                        string facValue = "";
                                        if (monitorData.Count <= (j * 24 + i))
                                        {
                                            facValue = "0";
                                        }
                                        else
                                        {
                                            facValue = (monitorData[j * 24 + i][fac] != DBNull.Value && monitorData[j * 24 + i][fac].ToString().Trim() != "") ? monitorData[j * 24 + i][fac].ToString() : "0";
                                        }
                                        if (facValue == "0") daycount--;
                                        d += Convert.ToDecimal(facValue);
                                    }
                                    decimal? avg = 0;
                                    if (daycount > 0)
                                    {
                                        avg = d / daycount;
                                        dtForDay.Rows[i][fac] = avg.ToString();
                                    }
                                    else
                                    {
                                        avg = null;
                                    }
                                }
                            }
                            for (int i = 0; i < 24; i++)
                            {
                                if (portIds.Length > 1)
                                    dtForDay.Rows[i]["PointId"] = "0";
                                else
                                    dtForDay.Rows[i]["PointId"] = portIds[0].ToString();
                                dtForDay.Rows[i]["Hours"] = i + "时";
                            }
                            dv = dtForDay.AsDataView();
                            dt = UpdateExportColumnName(dv, dvStatistical);
                        }
                        if (ddlDataSource.SelectedValue == "AuditData")
                        {
                            DataTable dtForDayAud = new DataTable();
                            string orderby = "Tstamp asc";
                            var monitorData = m_HourData.GetHourDataAvg(portIds, factors.Select(p => p.PollutantCode).ToArray(), dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderby);
                            dtForDayAud.Columns.Add("PointId", typeof(int));
                            dtForDayAud.Columns.Add("Hours", typeof(string));
                            foreach (string fac in factorCodes)
                            {
                                dtForDayAud.Columns.Add(fac, typeof(string));
                            }
                            for (int i = 0; i < 24; i++)
                            {
                                dtForDayAud.Rows.Add(dtForDayAud.NewRow());
                            }
                            foreach (string fac in factorCodes)
                            {
                                for (int i = 0; i < 24; i++)
                                {
                                    int daycount = (dtEnd - dtBegion).Days + 1;
                                    decimal d = 0;  //某因子某整点总和计数器
                                    for (int j = 0; j < days; j++)
                                    {
                                        string facValue = "";
                                        if (monitorData.Count <= (j * 24 + i))
                                        {
                                            facValue = "0";
                                        }
                                        else
                                        {
                                            facValue = (monitorData[j * 24 + i][fac] != DBNull.Value && monitorData[j * 24 + i][fac].ToString().Trim() != "") ? monitorData[j * 24 + i][fac].ToString() : "0";
                                        }
                                        if (facValue == "0") daycount--;
                                        d += Convert.ToDecimal(facValue);
                                    }
                                    decimal? avg = 0;
                                    if (daycount > 0)
                                    {
                                        avg = d / daycount;
                                        dtForDayAud.Rows[i][fac] = avg.ToString();
                                    }
                                    else
                                    {
                                        avg = null;
                                    }
                                }
                            }
                            for (int i = 0; i < 24; i++)
                            {
                                if (portIds.Length > 1)
                                    dtForDayAud.Rows[i]["PointId"] = "0";
                                else
                                    dtForDayAud.Rows[i]["PointId"] = portIds[0].ToString();
                                dtForDayAud.Rows[i]["Hours"] = i + "时";
                            }
                            dv = dtForDayAud.AsDataView();
                            dt = UpdateExportColumnName(dv, dvStatistical);
                        }
                    }
                    else
                    {
                        if (ddlDataSource.SelectedValue == "OriData")
                        {
                            DateTime dtBegion = dtpBegin.SelectedDate.Value;
                            DateTime dtEnd = dtpEnd.SelectedDate.Value;
                            if (radlDataTypeOri.SelectedValue == "OriDay")
                            {
                                dtBegion = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd 00:00:00"));
                                dtEnd = Convert.ToDateTime(dtpEnd.SelectedDate.Value.ToString("yyyy-MM-dd 23:59:59"));
                                string orderby = "DateTime desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderby = "DateTime asc";
                                var monitorData = m_DayOriData.GetDayAvgData(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderby);
                                dv = monitorData;
                                if (IsStatistical.Checked && factors.Count != 0)
                                {
                                    dvStatistical = g_IInfectantDALService.GetStatisticalData(portIds, factors, dtBegion, dtEnd);
                                }
                                dt = UpdateExportColumnName(dv, dvStatistical);
                            }
                            if (radlDataTypeOri.SelectedValue == "OriMonth")
                            {
                                dtBegion = dtpBegin.SelectedDate.Value;
                                dtEnd = dtpEnd.SelectedDate.Value;
                                string orderby = "Year desc,MonthOfYear desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderby = "Year asc,MonthOfYear asc";
                                var monitorData = m_MonthOriData.GetOriDataAvg(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderby);
                                dv = monitorData; if (IsStatistical.Checked && factors.Count != 0)
                                {
                                    dvStatistical = g_IInfectantDALService.GetStatisticalData(portIds, factors, dtBegion, dtEnd);
                                }
                                dt = UpdateExportColumnName(dv, dvStatistical);
                            }
                            if (radlDataTypeOri.SelectedValue == "Min1" || radlDataTypeOri.SelectedValue == "Min5")
                            {
                                string orderby = "Tstamp desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderby = "Tstamp asc";
                                var monitorData = m_HourOriData.GetHourAvgData(portIds, factorCodes, dtBegion, dtEnd, radlDataTypeOri.SelectedValue, pageSize, pageNo, out recordTotal, orderby);
                                dv = monitorData;
                                if (IsStatistical.Checked && factors.Count != 0)
                                {
                                    dvStatistical = g_IInfectantDALService.GetStatisticalData(portIds, factors, dtBegion, dtEnd);
                                }
                                dt = UpdateExportColumnName(dv, dvStatistical);
                            }
                            if (radlDataTypeOri.SelectedValue == "Min60")
                            {
                                //dtBegion = Convert.ToDateTime(hourBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:00"));
                                //dtEnd = Convert.ToDateTime(hourEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:00"));
                                dtBegion = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:00"));
                                dtEnd = Convert.ToDateTime(dtpEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:00"));
                                string orderby = "Tstamp desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderby = "Tstamp asc";
                                var monitorData = m_HourOriData.GetHourAvgData(portIds, factorCodes, dtBegion, dtEnd, radlDataTypeOri.SelectedValue, pageSize, pageNo, out recordTotal, orderby);
                                dv = monitorData;
                                if (IsStatistical.Checked && factors.Count != 0)
                                {
                                    dvStatistical = g_IInfectantDALService.GetStatisticalData(portIds, factors, dtBegion, dtEnd);
                                }
                                dt = UpdateExportColumnName(dv, dvStatistical);
                            }
                        }
                        if (ddlDataSource.SelectedValue == "AuditData")
                        {
                            //小时数据
                            if (radlDataType.SelectedValue == "Hour")
                            {
                                DateTime dtBegion = Convert.ToDateTime(hourBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:00"));
                                DateTime dtEnd = Convert.ToDateTime(hourEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:00"));
                                string orderBy = "Tstamp desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderBy = "Tstamp asc";
                                var auditData = m_HourData.GetHourDataAvg(portIds, factors.Select(p => p.PollutantCode).ToArray(), dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
                                dv = auditData;
                                if (IsStatistical.Checked && factors.Count != 0)
                                {
                                    dvStatistical = g_IInfectantDALService.GetStatisticalData(portIds, factors, dtBegion, dtEnd);
                                }
                                dt = UpdateExportColumnName(dv, dvStatistical);
                            }
                            //日数据
                            else if (radlDataType.SelectedValue == "Day")
                            {
                                DateTime dtBegion = dayBegin.SelectedDate.Value;
                                DateTime dtEnd = dayEnd.SelectedDate.Value;
                                string orderBy = "DateTime desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderBy = "DateTime asc";
                                var auditData = m_DayData.GetDayDataAvg(portIds, factors.Select(p => p.PollutantCode).ToArray(), dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
                                dv = auditData;
                                if (IsStatistical.Checked && factors.Count != 0)
                                {
                                    dvStatistical = g_IInfectantDALService.GetStatisticalData(portIds, factors, dtBegion, dtEnd);
                                }
                                dt = UpdateExportColumnName(dv, dvStatistical);
                            }
                            //月数据
                            else if (radlDataType.SelectedValue == "Month")
                            {
                                int monthB = monthBegin.SelectedDate.Value.Year;
                                int monthE = monthEnd.SelectedDate.Value.Year;
                                int monthF = monthBegin.SelectedDate.Value.Month;
                                int monthT = monthEnd.SelectedDate.Value.Month;
                                string orderBy = "Year desc,MonthOfYear desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderBy = "Year asc,MonthOfYear asc";
                                var auditData = m_MonthData.GetMonthDataAvg(portIds, factors.Select(p => p.PollutantCode).ToArray(), monthB, monthF, monthE, monthT, pageSize, pageNo, out recordTotal, orderBy);
                                dv = auditData;
                                if (IsStatistical.Checked && factors.Count != 0)
                                {
                                    dvStatistical = m_MonthData.GetMonthStatisticalData(portIds, factors, monthB, monthF, monthE, monthT);
                                }
                                dt = UpdateExportColumnName(dv, dvStatistical);
                            }
                            //季数据
                            else if (radlDataType.SelectedValue == "Season")
                            {
                                int seasonB = seasonBegin.SelectedDate.Value.Year;
                                int seasonE = seasonEnd.SelectedDate.Value.Year;
                                int seasonF = Convert.ToInt32(seasonFrom.SelectedValue);
                                int seasonT = Convert.ToInt32(seasonTo.SelectedValue);
                                string orderBy = "Year desc,SeasonOfYear desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderBy = "Year asc,SeasonOfYear asc";
                                var auditData = m_SeasonData.GetSeasonDataAvg(portIds, factors.Select(p => p.PollutantCode).ToArray(), seasonB, seasonF, seasonE, seasonT, pageSize, pageNo, out recordTotal, orderBy);
                                dv = auditData;
                                if (IsStatistical.Checked && factors.Count != 0)
                                {
                                    dvStatistical = m_SeasonData.GetSeasonStatisticalData(portIds, factors, seasonB, seasonF, seasonE, seasonT);
                                }
                                dt = UpdateExportColumnName(dv, dvStatistical);
                            }
                            //年数据
                            else if (radlDataType.SelectedValue == "Year")
                            {

                                int yearB = yearBegin.SelectedDate.Value.Year;
                                int yearE = yearEnd.SelectedDate.Value.Year;
                                string orderBy = "Year desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderBy = "Year asc";
                                var auditData = m_YearData.GetYearDataAvg(portIds, factors.Select(p => p.PollutantCode).ToArray(), yearB, yearE, pageSize, pageNo, out recordTotal, orderBy);
                                dv = auditData;
                                if (IsStatistical.Checked && factors.Count != 0)
                                {
                                    dvStatistical = m_YearData.GetYearStatisticalData(portIds, factors, yearB, yearE);
                                }
                                dt = UpdateExportColumnName(dv, dvStatistical);
                            }
                            //周数据
                            else if (radlDataType.SelectedValue == "Week")
                            {
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
                                string orderBy = "Year desc,WeekOfYear desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderBy = "Year asc,WeekOfYear asc";
                                var auditData = m_WeekData.GetWeekDataAvg(portIds, factors.Select(p => p.PollutantCode).ToArray(), weekB, weekF, weekE, weekT, pageSize, pageNo, out recordTotal, orderBy);
                                dv = auditData;
                                if (IsStatistical.Checked && factors.Count != 0)
                                {
                                    dvStatistical = m_WeekData.GetWeekStatisticalData(portIds, factors, weekB, weekF, weekE, weekT);
                                }
                                dt = UpdateExportColumnName(dv, dvStatistical);
                            }
                        }
                    }

                }
                else
                {
                    gridOriginal.DataSource = null;
                }
                decimal value = 0M;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //if (dt.Rows[i]["日期"] != DBNull.Value)
                    //{
                    //    dt.Rows[i]["日期"] = string.Format("{0:yyyy-MM-dd}", DateTime.Parse(dt.Rows[i]["日期"].ToString()));
                    //}
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (dt.Columns[j].ColumnName.Contains("μg/m³") && !LZSfactorName.Contains(dt.Columns[j].ColumnName.Replace("(μg/m³)", "")))
                        {
                            if (!string.IsNullOrWhiteSpace(dt.Rows[i][j].ToString()))
                            {
                                if (decimal.TryParse(dt.Rows[i][j].ToString(), out value))
                                {
                                    dt.Rows[i][j] = DecimalExtension.GetPollutantValue(value * 1000, 0);
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
                ExcelHelper.DataTableToExcel(dt, "相关性分析", "相关性分析", this.Page);
            }
        }
        /// <summary>
        /// 修改要导出的数据表中的列名
        /// </summary>
        /// <param name="dv">原始数据表</param>
        /// <returns></returns>
        private DataTable UpdateExportColumnName(DataView dv, DataView dvStatistical)
        {
            DataTable dtNew = dv.ToTable();
            DataTable dtnew = dtNew.Clone();
            if (ddlDataSource.SelectedValue == "AuditData")
            {
                if (radlDataType.SelectedValue == "Day")
                {
                    dtnew.Columns["DateTime"].DataType = typeof(string);
                }
            }
            else
            {
                if (radlDataTypeOri.SelectedValue == "OriDay")
                {
                    dtnew.Columns["DateTime"].DataType = typeof(string);
                }
            }

            for (int i = 0; i < dtNew.Rows.Count; i++)
            {
                DataRow drNew = dtNew.Rows[i];
                DataRow drnew = dtnew.NewRow();
                foreach (DataColumn dcNew in dtNew.Columns)
                {
                    if (!string.IsNullOrWhiteSpace(drNew[dcNew].ToString()))
                    {
                        drnew[dcNew.ColumnName] = drNew[dcNew].ToString().Replace("<br/>", " \r\n");
                    }

                }
                dtnew.Rows.Add(drnew);
            }

            dtnew.Columns.Add("站点", typeof(string)).SetOrdinal(0);
            points = pointCbxRsm.GetPoints();
            for (int i = 0; i < dtnew.Rows.Count; i++)
            {
                DataRow drNew = dtnew.Rows[i];
                drNew["站点"] = (points.Count(t => t.PointID == drNew["PointId"].ToString()) > 0)
                    ? points.Where(t => t.PointID == drNew["PointId"].ToString()).Select(t => t.PointName).FirstOrDefault()
                    : "多站点均值";

                if (ddlDataSource.SelectedValue == "AuditData")
                {
                    if (radlDataType.SelectedValue == "Day")
                    {
                        drNew["DateTime"] = string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(drNew["DateTime"].ToString()).ToString("yyyy-MM-dd"));
                    }
                }
                else
                {
                    if (radlDataTypeOri.SelectedValue == "OriDay")
                    {
                        drNew["DateTime"] = string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(drNew["DateTime"].ToString()).ToString("yyyy-MM-dd"));
                    }
                }
            }
            for (int i = 0; i < dtnew.Columns.Count; i++)
            {
                DataColumn dcNew = dtnew.Columns[i];
                //追加日期
                if (dcNew.ColumnName == "Tstamp")
                {
                    dcNew.ColumnName = "日期";
                }
                else if (dcNew.ColumnName == "Hours")
                {
                    dcNew.ColumnName = "小时";
                }
                else if (dcNew.ColumnName == "DateTime")
                {
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
                else if (dcNew.ColumnName == "序号" || dcNew.ColumnName == "PointId" || dcNew.ColumnName.Contains("_Status") || dcNew.ColumnName.Contains("_DataFlag") || dcNew.ColumnName.Contains("_AuditFlag"))
                {
                    dtnew.Columns.Remove(dcNew);
                    i--;
                }
            }

            if (dvStatistical != null && dvStatistical.Table.Rows.Count > 0)
            {
                DataTable dtStatistical = dvStatistical.Table;
                DataRow drMaxRow = dtnew.NewRow();
                drMaxRow["站点"] = "最大值";
                DataRow drMinRow = dtnew.NewRow();
                drMinRow["站点"] = "最小值";
                DataRow drAvgRow = dtnew.NewRow();
                drAvgRow["站点"] = "平均值";
                for (int i = 0; i < dtStatistical.Rows.Count; i++)
                {
                    DataRow drStatistical = dtStatistical.Rows[i];
                    if (drStatistical["PollutantCode"] != DBNull.Value && drStatistical["PollutantCode"].ToString() != "")
                    {
                        IPollutant factor = factorCbxRsm.GetFactors().FirstOrDefault(x => x.PollutantCode.Equals(drStatistical["PollutantCode"].ToString()));
                        int pdn = 0;
                        if (factor != null)
                        {
                            pdn = Convert.ToInt32(string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum));
                        }
                        if (dtnew.Columns.Contains(factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"))
                        {
                            drMaxRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"] = drStatistical["Value_Max"] != DBNull.Value ? drStatistical["Value_Max"] : "--";
                            drMinRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"] = drStatistical["Value_Min"] != DBNull.Value ? drStatistical["Value_Min"] : "--";
                            drAvgRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"] = drStatistical["Value_Avg"] != DBNull.Value ? drStatistical["Value_Avg"] : "--";

                            if (drMaxRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"].ToString() != "--")
                            {
                                decimal AVG = Convert.ToDecimal(drMaxRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"]);
                                drMaxRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"] = DecimalExtension.GetPollutantValue(AVG, pdn).ToString();
                            }
                            if (drMinRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"].ToString() != "--")
                            {
                                decimal MAX = Convert.ToDecimal(drMinRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"]);
                                drMinRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"] = DecimalExtension.GetPollutantValue(MAX, pdn).ToString();
                            }
                            if (drAvgRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"].ToString() != "--")
                            {
                                decimal MIN = Convert.ToDecimal(drAvgRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"]);
                                drAvgRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"] = DecimalExtension.GetPollutantValue(MIN, pdn).ToString();
                            }
                        }
                    }
                }
                dtnew.Rows.Add(drAvgRow);
                dtnew.Rows.Add(drMaxRow);
                dtnew.Rows.Add(drMinRow);
            }
            return dtnew;
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
        /// grid 创建列事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridOriginal_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            try
            {
                GridBoundColumn col = e.Column as GridBoundColumn;
                if (col == null)
                    return;
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
                else if (col.DataField == "Tstamp")
                {
                    col = (GridDateTimeColumn)e.Column;
                    string tstcolformat = "{0:yyyy-MM-dd HH:mm}";
                    if (SmartEP.Core.Enums.EnumMapping.ParseEnum<PollutantDataType>(radlDataType.SelectedValue) == PollutantDataType.Min60)
                        tstcolformat = "{0:yyyy-MM-dd HH:mm}";
                    if (SmartEP.Core.Enums.EnumMapping.ParseEnum<PollutantDataType>(radlDataType.SelectedValue) == PollutantDataType.Min5)
                        tstcolformat = "{0:yyyy-MM-dd HH:mm}";
                    if (SmartEP.Core.Enums.EnumMapping.ParseEnum<PollutantDataType>(radlDataType.SelectedValue) == PollutantDataType.Min1)
                        tstcolformat = "{0:yyyy-MM-dd HH:mm:ss}";
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
                else if (col.DataField == "Hours")
                {
                    col.HeaderText = "时间";
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
                else if (factors.Select(x => x.PollutantCode).Contains(col.DataField))
                {
                    int radGridColWidthValue = int.Parse(radGridColWidth.Value.ToString());
                    IPollutant factor = factors.FirstOrDefault(x => x.PollutantCode.Equals(col.DataField));
                    string unit = factor.PollutantMeasureUnit;
                    string strName = factor.PollutantName;
                    if (strName == "PM2.5")
                    {
                        strName = "PM<sub>2.5</sub>";
                    }
                    if (strName == "PM10")
                    {
                        strName = "PM<sub>10</sub>";
                    }
                    if (unit.Contains("m3"))
                    {
                        unit = unit.Replace("m3", "m<sup>3</sup>");
                    }
                    col.HeaderText = string.Format("{0}<br>({1})", strName, unit);
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(radGridColWidthValue);
                    col.ItemStyle.Width = Unit.Pixel(radGridColWidthValue);
                    SetGridFooterText(col);
                }
                else
                {
                    e.Column.Visible = false;
                }

            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// 统计行事件
        /// </summary>
        /// <param name="col"></param>
        public void SetGridFooterText(GridBoundColumn col)
        {
            //统计行
            if (dvStatistical != null)
            {
                string avg = string.Empty;
                string max = string.Empty;
                string min = string.Empty;
                dvStatistical.RowFilter = string.Format("PollutantCode='{0}'", col.DataField);
                if (dvStatistical.Count > 0)
                {
                    avg = dvStatistical[0]["Value_Avg"] != DBNull.Value ? dvStatistical[0]["Value_Avg"].ToString() : "--";
                    max = dvStatistical[0]["Value_Max"] != DBNull.Value ? dvStatistical[0]["Value_Max"].ToString() : "--";
                    min = dvStatistical[0]["Value_Min"] != DBNull.Value ? dvStatistical[0]["Value_Min"].ToString() : "--";
                    if (factors != null)
                    {
                        IPollutant factor = factors[0];
                        if (avg != "--")
                        {
                            decimal AVG = Convert.ToDecimal(avg);
                            avg = DecimalExtension.GetPollutantValue(AVG, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum)).ToString();
                        }
                        if (max != "--")
                        {
                            decimal MAX = Convert.ToDecimal(max);
                            max = DecimalExtension.GetPollutantValue(MAX, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum)).ToString();
                        }
                        if (min != "--")
                        {
                            decimal MIN = Convert.ToDecimal(min);
                            min = DecimalExtension.GetPollutantValue(MIN, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum)).ToString();
                        }
                    }
                }
                col.FooterText = string.Format("{0}<br>{1}<br>{2}", avg, max, min);
                col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
            }
        }
        /// <summary>
        /// 站点因子联动
        /// </summary>
        protected void pointCbxRsm_SelectedChanged()
        {
        }

        protected void radlDataTypeOri_SelectedIndexChanged(object sender, EventArgs e)
        {
            RegisterScript("DivSelectedOri();");
            if (radlDataTypeOri.SelectedValue == "Min1"||radlDataTypeOri.SelectedValue == "Min5")
            {
                dtpBegin.DateInput.DateFormat = "yyyy-MM-dd HH:mm";
                dtpEnd.DateInput.DateFormat = "yyyy-MM-dd HH:mm";
            }
            else if (radlDataTypeOri.SelectedValue == "OriDay")
            {
                dtpBegin.DateInput.DateFormat = "yyyy-MM-dd";
                dtpEnd.DateInput.DateFormat = "yyyy-MM-dd";
            }
            else if (radlDataTypeOri.SelectedValue == "OriMonth")
            {
                dtpBegin.DateInput.DateFormat = "yyyy-MM";
                dtpEnd.DateInput.DateFormat = "yyyy-MM";
            }
            else
            {
                dtpBegin.DateInput.DateFormat = "yyyy-MM-dd HH:00";
                dtpEnd.DateInput.DateFormat = "yyyy-MM-dd HH:00";
            }
        }

        //<summary>
        //审核数据数据类型时间框选择
        //</summary>
        //<param name="sender"></param>
        //<param name="e"></param>
        protected void radlDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            RegisterScript("DivSelecteds();");
        }

        /// <summary>
        /// 数据来源选项变化，数据类型选项相应变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlDataSource_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            if (ddlDataSource.SelectedIndex == 0)
            {
                radlDataTypeOri.SelectedIndex = 2;

                dtpBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm"));
                dtpEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                dtpBegin.DateInput.DateFormat = "yyyy-MM-dd HH:mm";
                dtpEnd.DateInput.DateFormat = "yyyy-MM-dd HH:mm";
                if (tabStrip.SelectedIndex == 2 || tabStrip.SelectedIndex == 3)
                {
                    //radlDataTypeOri.SelectedIndex = 3;
                    RegisterScript("DivSelectedNew();");
                }
                else
                    RegisterScript("DivSelected();");
            }
            else
            {
                radlDataType.SelectedIndex = 0;
                hourBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddHours(-47).ToString("yyyy-MM-dd HH:00"));
                hourEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00"));
                hourBegin.DateInput.DateFormat = "yyyy-MM-dd HH:00";
                hourEnd.DateInput.DateFormat = "yyyy-MM-dd HH:00";
                if (tabStrip.SelectedIndex == 2 || tabStrip.SelectedIndex == 3)
                {
                    //radlDataType.SelectedIndex = 1;
                    RegisterScript("DivSelectedNew();");
                }
                else
                    RegisterScript("DivSelected();");
            }
        }

        /// <summary>
        /// 绑定周
        /// </summary>
        private void BindWeekFComboBox()
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
            SetLiteral();
        }

        private void BindWeekTComboBox()
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
            SetLiteralT();
        }
        /// <summary>
        /// 显示所选周的日期
        /// </summary>
        private void SetLiteral()
        {
            txtweekF.Text = string.Format("从{0:yyyy-MM-dd}", weekFrom.SelectedValue);
        }
        private void SetLiteralT()
        {
            DateTime endDate = DateTime.ParseExact(weekTo.SelectedValue, "yyyy-MM-dd", null).AddDays(6);
            if (endDate > System.DateTime.Now)
                endDate = System.DateTime.Now;
            txtweekT.Text = string.Format("到{0:yyyy-MM-dd}", endDate);
        }
        #region 周数据更新日期范围
        protected void weekBegin_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            BindWeekFComboBox();
        }

        protected void weekFrom_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            SetLiteral();
        }

        protected void weekEnd_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            BindWeekTComboBox();
        }

        protected void weekTo_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            SetLiteralT();
        }
        #endregion

        protected void ShowType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindChart();
        }

        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
            RegisterScript("createDayDev();");
        }

        protected void factorCbxRsm_SelectedChanged()
        {
            string[] factorCodes = factorCbxRsm.GetFactorValues(SmartEP.Core.Enums.CbxRsmReturnType.Code);
            Session["X"] = factorCodes[0].ToString();
            Session["Y"] = factorCodes[1].ToString();
        }
    }
}