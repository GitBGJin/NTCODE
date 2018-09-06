﻿using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.DataAnalyze.Air.DataQuery;
using SmartEP.Utilities.Calendar;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Utilities.Office;
using SmartEP.Utilities.Web.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.SuperStationManagement
{
    /// <summary>
    /// 名称：SolarRadiationInst.cs
    /// 创建人：徐阳
    /// 创建日期：2017-05-22
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：太阳辐射仪
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class SolarRadiationInst : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        DataQueryByHourService m_HourData = Singleton<DataQueryByHourService>.GetInstance();
        DataQueryByDayService m_DayData = Singleton<DataQueryByDayService>.GetInstance();
        DataQueryByWeekService m_WeekData = Singleton<DataQueryByWeekService>.GetInstance();
        DataQueryByMonthService m_MonthData = Singleton<DataQueryByMonthService>.GetInstance();
        DataQueryBySeasonService m_SeasonData = Singleton<DataQueryBySeasonService>.GetInstance();
        DataQueryByYearService m_YearData = Singleton<DataQueryByYearService>.GetInstance();
        InstrumentChannelService m_InstrumentChannelService = Singleton<InstrumentChannelService>.GetInstance();
        MonitoringPointAirService monitoringPointAir = new MonitoringPointAirService();
        string LZSfactorName = string.Empty;
        string LZSPfactor = string.Empty;

        /// <summary>
        /// 选择因子
        /// </summary>
        private IList<IPollutant> factors = null;
        private string[] strFactors = null;
        /// <summary>
        /// 选择站点
        /// </summary>
        private IList<IPoint> points = null;
        private string[] portIds = null;
        /// <summary>
        /// 统计
        /// </summary>
        DataView dvStatistical = null;

        string isSuper = string.Empty;

        //获取因子小数位
        // channelCode 因子代码 例：CO的code是a21005  （ channelCode=a21005）
        SmartEP.Service.BaseData.Channel.AirPollutantService m_AirPollutantService = new SmartEP.Service.BaseData.Channel.AirPollutantService();

        protected override void OnPreInit(EventArgs e)
        {
            isSuper = PageHelper.GetQueryString("superOrNot");
            pointCbxRsm.isSuper(isSuper);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //hdPointName.Value = cbPoint.SelectedText;
                InitControl();
            }
        }
        #region 页面控件初始化
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            //初始化测点
            //string connSuperStationPointName = System.Configuration.ConfigurationManager.AppSettings["SuperStationPointName"];
            //string[] strPointName = connSuperStationPointName.Split(',');

            //string connSuperStationPointId = System.Configuration.ConfigurationManager.AppSettings["SuperStationPointId"];
            //string[] strPointID = connSuperStationPointId.Split(',');

            //cbPoint.Items.Clear();
            //for (int i = 0; i < strPointName.Length; i++)
            //{
            //    //RadComboBoxItem cmbItemFactor = new RadComboBoxItem();
            //    //cmbItemFactor.Text = strPointName[i].ToString();
            //    //cmbItemFactor.Value = strPointID[i].ToString();

            //    //cmbItemFactor.Checked = true;
            //    //cbPoint.Items.Add(cmbItemFactor);
            //    cbPoint.Items.Add(new DropDownListItem(strPointName[i].ToString(), strPointID[i].ToString()));
            //}
            //cbPoint.DataBind();

            //string[] cFactor = { "a20044", "a20068", "a20038", "a20012", "a20095", "a20033", "a20072", "a20026", "a20029", "a20055", "a20041", "a20004", "a20007", "a20058", "a20064", "a20092", "a20104", "a20101", "a34004" };
            //string[] cFactorName = { "铝", "钾", "钴", "钡", "钛", "铬" ,"硒"，"镉","银没有"，"金没有","钙","锰"，"铜",'锑','砷','汞','铂没有','镍',"镓没有",'锡','锌','钯没有','钒',"PM2.5"};
            //string connStrCode = ConfigurationManager.AppSettings["AerosolIonCode"];
            //string[] cFactor = connStrCode.Split(',');
            //string connStrName = ConfigurationManager.AppSettings["AerosolIonName"];
            //string[] cFactorName = connStrName.Split(',');

            //cbFactor.Items.Clear();
            //for (int i = 0; i < cFactor.Length; i++)
            //{
            //    RadComboBoxItem cmbItemFactofactorCbxRsmr = new RadComboBoxItem();
            //    cmbItemFactor.Text = cFactorName[i].ToString().Trim();
            //    cmbItemFactor.Value = cFactor[i].ToString().Trim();

            //    cmbItemFactor.Checked = true;
            //    cbFactor.Items.Add(cmbItemFactor);
            //}
            //cbFactor.DataBind();

            //因子控件初始化
            BindFactors("aabe91e0-29a4-427c-becc-0b29f1224422", out LZSfactorName, out LZSPfactor);
            string PollutantName = LZSfactorName;
            factorCbxRsm.SetFactorValuesFromNames(PollutantName);

            //时间框初始化
            hourBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddHours(-47));
            hourEnd.SelectedDate = DateTime.Now;
            dtpBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm"));
            dtpEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            dayBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
            dayEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            monthBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToString("yyyy-MM"));
            monthEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"));
            seasonBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"));
            seasonEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"));
            weekBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"));
            weekEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"));
            yearBegin.SelectedDate = DateTime.Now.AddYears(-5);
            yearEnd.SelectedDate = DateTime.Now.AddYears(-1);
            BindWeekComboBox();//绑定周
            SetLiteral();//显示周日期范围
            //数据类型
            radlDataTypeOri.Items.Add(new ListItem("一分钟数据", PollutantDataType.Min1.ToString()));
            radlDataTypeOri.Items.Add(new ListItem("五分钟数据", PollutantDataType.Min5.ToString()));
            radlDataTypeOri.Items.Add(new ListItem("小时数据", PollutantDataType.Min60.ToString()));
            radlDataTypeOri.SelectedValue = PollutantDataType.Min60.ToString();

            radlDataType.Items.Add(new ListItem("小时数据", PollutantDataType.Hour.ToString()));
            radlDataType.Items.Add(new ListItem("日数据", PollutantDataType.Day.ToString()));
            radlDataType.Items.Add(new ListItem("周数据", PollutantDataType.Week.ToString()));
            radlDataType.Items.Add(new ListItem("月数据", PollutantDataType.Month.ToString()));
            radlDataType.Items.Add(new ListItem("季数据", PollutantDataType.Season.ToString()));
            radlDataType.Items.Add(new ListItem("年数据", PollutantDataType.Year.ToString()));

            dtpHour.Visible = true;
            dbtHour.Visible = false;
            dbtDay.Visible = false;
            dbtMonth.Visible = false;
            dbtSeason.Visible = false;
            dbtYear.Visible = false;
            dbtWeek.Visible = false;

        }
        #endregion

        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid(string type)
        {
            DataTable dt = new DataTable();
            try
            {
                //string[] factorCodes = factorCbxRsm.GetFactorValues(SmartEP.Core.Enums.CbxRsmReturnType.Code);                
                //for (int i = 0; i < cbFactor.CheckedItems.Count(); i++)
                //{
                //    listName.Add(cbFactor.CheckedItems[i].Text);
                //    listCode.Add(cbFactor.CheckedItems[i].Value);
                //}

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
                    //审核数据小时数据
                    if (ddlDataSource.SelectedIndex == 1 && radlDataType.SelectedValue == "Hour")
                    {
                        string orderBy1 = "PointId asc,Tstamp desc";
                        dtBegion = Convert.ToDateTime(hourBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:00"));
                        dtEnd = Convert.ToDateTime(hourEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:00"));
                        if (type == "列表")
                        {
                            auditData = m_HourData.GetHourDataPagerNew(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy1);//小时数据查询

                        }
                        else
                        {
                            //【给隐藏域赋值，用于显示Chart】
                            SetHiddenData(portIds, factors, dtBegion, dtEnd);
                            auditData1 = m_HourData.GetHourDataPagerNew(portIds, factorCodes, dtBegion, dtEnd, pageSize1, pageNo1, out recordTotal);//小时数据查询
                        }
                    }
                    //审核数据日数据
                    else if (ddlDataSource.SelectedIndex == 1 && radlDataType.SelectedValue == "Day")
                    {
                        string orderBy1 = "PointId asc,Tstamp desc";
                        dtBegion = dayBegin.SelectedDate.Value;
                        dtEnd = dayEnd.SelectedDate.Value.AddDays(1).AddSeconds(-1);
                        if (type == "列表")
                        {
                            auditData = m_HourData.GetHourDataPagerNew(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy1);//日类型按 小时数据查询

                        }
                        else
                        {
                            //【给隐藏域赋值，用于显示Chart】
                            SetHiddenData(portIds, factors, dtBegion, dtEnd);
                            auditData1 = m_HourData.GetHourDataPagerNew(portIds, factorCodes, dtBegion, dtEnd, pageSize1, pageNo1, out recordTotal);//日类型按 小时数据查询
                        }
                    }
                    //审核数据月数据
                    else if (ddlDataSource.SelectedIndex == 1 && radlDataType.SelectedValue == "Month")
                    {

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
                            auditData = m_DayData.GetDayData(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy); //月类型 按日数据查询
                        }
                        else
                        {
                            //【给隐藏域赋值，用于显示Chart】
                            SetHiddenData(portIds, factors, monthB + ";" + monthF + ";" + monthE + ";" + monthT);
                            auditData1 = m_DayData.GetDayData(portIds, factorCodes, dtBegion, dtEnd, pageSize1, pageNo1, out recordTotal);//月类型 按日数据查询
                        }
                    }
                    //审核数据周数据
                    else if (ddlDataSource.SelectedIndex == 1 && radlDataType.SelectedValue == "Week")
                    {
                        int weekB = weekBegin.SelectedDate.Value.Year;
                        int weekE = weekEnd.SelectedDate.Value.Year;
                        int newyear = DateTime.ParseExact(weekFrom.SelectedValue, "yyyy-MM-dd", null).AddDays(6).Year;
                        int nyear = DateTime.ParseExact(weekTo.SelectedValue, "yyyy-MM-dd", null).Year;
                        int weekF = 0;
                        int weekT = 0;
                        dtBegion = Convert.ToDateTime(weekFrom.SelectedValue);
                        dtEnd = DateTime.ParseExact(weekTo.SelectedValue, "yyyy-MM-dd", null).AddDays(6).AddDays(1).AddSeconds(-1);
                        if (type == "列表")
                        {
                            auditData = m_DayData.GetDayData(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy); //周类型 按日数据查询
                        }
                        else
                        {
                            //【给隐藏域赋值，用于显示Chart】
                            SetHiddenData(portIds, factors, weekB + ";" + weekF + ";" + weekE + ";" + weekT);
                            auditData1 = m_DayData.GetDayData(portIds, factorCodes, dtBegion, dtEnd, pageSize1, pageNo1, out recordTotal);//周类型 按日数据查询
                        }
                    }
                    //审核数据季数据
                    else if (ddlDataSource.SelectedIndex == 1 && radlDataType.SelectedValue == "Season")
                    {
                        int seasonB = seasonBegin.SelectedDate.Value.Year;
                        int seasonE = seasonEnd.SelectedDate.Value.Year;
                        int seasonF = Convert.ToInt32(seasonFrom.SelectedValue);
                        int seasonT = Convert.ToInt32(seasonTo.SelectedValue);
                        if (type == "列表")
                        {
                            auditData = m_DayData.GetDayData(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy); //季类型 按日数据查询
                        }
                        else
                        {
                            //【给隐藏域赋值，用于显示Chart】
                            SetHiddenData(portIds, factors, seasonB + ";" + seasonF + ";" + seasonE + ";" + seasonT);
                            auditData1 = m_DayData.GetDayData(portIds, factorCodes, dtBegion, dtEnd, pageSize1, pageNo1, out recordTotal);//季类型 按日数据查询
                        }
                    }
                    //审核数据年数据
                    else if (ddlDataSource.SelectedIndex == 1 && radlDataType.SelectedValue == "Year")
                    {
                        int yearB = yearBegin.SelectedDate.Value.Year;
                        int yearE = yearEnd.SelectedDate.Value.Year;
                        if (type == "列表")
                        {
                            auditData = m_DayData.GetDayData(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy); //年类型 按日数据查询
                        }
                        else
                        {
                            //【给隐藏域赋值，用于显示Chart】
                            SetHiddenData(portIds, factors, yearB + ";" + yearE);
                            auditData1 = m_DayData.GetDayData(portIds, factorCodes, dtBegion, dtEnd, pageSize1, pageNo1, out recordTotal);//年类型 按日数据查询
                        }
                    }
                    //原始数据一分钟数据
                    else if (ddlDataSource.SelectedIndex == 0 && radlDataTypeOri.SelectedValue == "Min1")
                    {
                        string orderBy1 = "PointId asc,Tstamp desc";
                        dtBegion = Convert.ToDateTime(hourBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:00"));
                        dtEnd = Convert.ToDateTime(hourEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:00"));
                        if (type == "列表")
                        {
                            auditData = m_HourData.GetHourDataPagerNew(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy1);//小时数据查询

                        }
                        else
                        {
                            //【给隐藏域赋值，用于显示Chart】
                            SetHiddenData(portIds, factors, dtBegion, dtEnd);
                            auditData1 = m_HourData.GetHourDataPagerNew(portIds, factorCodes, dtBegion, dtEnd, pageSize1, pageNo1, out recordTotal);//小时数据查询
                        }
                    }
                    //原始数据五分钟数据
                    else if (ddlDataSource.SelectedIndex == 0 && radlDataTypeOri.SelectedValue == "Min5")
                    {
                        string orderBy1 = "PointId asc,Tstamp desc";
                        dtBegion = Convert.ToDateTime(hourBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:00"));
                        dtEnd = Convert.ToDateTime(hourEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:00"));
                        if (type == "列表")
                        {
                            auditData = m_HourData.GetHourDataPagerNew(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy1);//小时数据查询

                        }
                        else
                        {
                            //【给隐藏域赋值，用于显示Chart】
                            SetHiddenData(portIds, factors, dtBegion, dtEnd);
                            auditData1 = m_HourData.GetHourDataPagerNew(portIds, factorCodes, dtBegion, dtEnd, pageSize1, pageNo1, out recordTotal);//小时数据查询
                        }
                    }
                    //原始数据小时数据
                    else if (ddlDataSource.SelectedIndex == 0 && radlDataTypeOri.SelectedValue == "Min60")
                    {
                        string orderBy1 = "PointId asc,Tstamp desc";
                        dtBegion = Convert.ToDateTime(hourBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:00"));
                        dtEnd = Convert.ToDateTime(hourEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:00"));
                        if (type == "列表")
                        {
                            auditData = m_HourData.GetHourDataPagerNew(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy1);//小时数据查询

                        }
                        else
                        {
                            //【给隐藏域赋值，用于显示Chart】
                            SetHiddenData(portIds, factors, dtBegion, dtEnd);
                            auditData1 = m_HourData.GetHourDataPagerNew(portIds, factorCodes, dtBegion, dtEnd, pageSize1, pageNo1, out recordTotal);//小时数据查询
                        }
                    }
                    if (type == "列表")
                    {
                        dt = auditData.ToTable();

                        //for (int i = 0; i < dt.Rows.Count; i++)
                        //{
                        //    foreach (string factorCode in factorCodes)
                        //    {
                        //        dt.Rows[i][factorCode].ToString();

                        //        int DecimalNum = 3;

                        //        if (factorCode != "a20995" && factorCode != "a20996" && factorCode != "a20997" && factorCode != "a20998" && factorCode != "a20999")
                        //        {
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
                        //                dt.Rows[i][factorCode] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i][factorCode]), DecimalNum);
                        //            }
                        //        }
                        //    }
                        //}
                        foreach (string factorCode in factorCodes)
                        {
                            int DecimalNum = 3;
                            if (factorCode != "a20995" && factorCode != "a20996" && factorCode != "a20997" && factorCode != "a20998" && factorCode != "a20999")
                            {
                                IPollutant iFactorCode = m_AirPollutantService.GetPollutantInfo(factorCode);
                                if (iFactorCode != null)
                                {
                                    DecimalNum = int.TryParse(iFactorCode.PollutantDecimalNum, out DecimalNum) ? DecimalNum : 3;
                                }
                            }
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                if (dt.Rows[i][factorCode] != DBNull.Value)
                                {
                                    if (factorCode == "a20029" || factorCode == "a20068")
                                    {
                                        //value 需要进行小数位处理的数据 类型Decimal
                                        dt.Rows[i][factorCode] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i][factorCode]) / 1000, DecimalNum);
                                    }
                                    else if (factorCode == "a34004")// PM2.5 
                                    {
                                        //value 需要进行小数位处理的数据 类型Decimal
                                        dt.Rows[i][factorCode] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i][factorCode]) * 1000, 0);
                                    }
                                    else
                                    {
                                        //value 需要进行小数位处理的数据 类型Decimal
                                        dt.Rows[i][factorCode] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i][factorCode]), DecimalNum);
                                    }
                                }
                            }

                        }
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

                        gridAudit.DataSource = dt.DefaultView;
                        //数据总行数
                        gridAudit.VirtualItemCount = recordTotal;
                    }
                    else
                    {
                        dt = auditData1.ToTable();
                        foreach (string factorCode in factorCodes)
                        {
                            int DecimalNum = 3;
                            if (factorCode != "a20995" && factorCode != "a20996" && factorCode != "a20997" && factorCode != "a20998" && factorCode != "a20999")
                            {
                                IPollutant iFactorCode = m_AirPollutantService.GetPollutantInfo(factorCode);
                                if (iFactorCode != null)
                                {
                                    DecimalNum = int.TryParse(iFactorCode.PollutantDecimalNum, out DecimalNum) ? DecimalNum : 3;
                                }
                            }
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                if (dt.Rows[i][factorCode] != DBNull.Value)
                                {
                                    if (factorCode == "a20029" || factorCode == "a20068")
                                    {
                                        //value 需要进行小数位处理的数据 类型Decimal
                                        dt.Rows[i][factorCode] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i][factorCode]) / 1000, DecimalNum);
                                    }
                                    else if (factorCode == "a34004")// PM2.5 
                                    {
                                        //value 需要进行小数位处理的数据 类型Decimal
                                        dt.Rows[i][factorCode] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i][factorCode]) * 1000, 0);
                                    }
                                    else
                                    {
                                        //value 需要进行小数位处理的数据 类型Decimal
                                        dt.Rows[i][factorCode] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i][factorCode]), DecimalNum);
                                    }
                                }
                            }

                        }
                        hdHeavyMetalMonitor.Value = ToJson(dt);
                    }
                }
                else
                {
                    gridAudit.DataSource = new DataTable();
                }
            }
            catch (Exception ex) { }
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
                //追加测点
                GridBoundColumn col = (GridBoundColumn)e.Column;
                string a = col.DataField;
                if (col.DataField == "PointId")
                {
                    col.HeaderText = "测点";
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
                    //string tstcolformat = "{0:yyyy-MM-dd}";
                    string tstcolformat = "{0:MM-dd HH:mm}";
                    if (SmartEP.Core.Enums.EnumMapping.ParseEnum<PollutantDataType>(radlDataType.SelectedValue) == PollutantDataType.Day && radlDataType.SelectedValue == "Day")
                    {
                        tstcolformat = "{0:MM-dd HH:mm}";
                    }
                    else if (SmartEP.Core.Enums.EnumMapping.ParseEnum<PollutantDataType>(radlDataType.SelectedValue) == PollutantDataType.Month && radlDataType.SelectedValue == "Month")
                    {
                        tstcolformat = "{0:yyyy-MM-dd}";
                    }
                    else if (SmartEP.Core.Enums.EnumMapping.ParseEnum<PollutantDataType>(radlDataType.SelectedValue) == PollutantDataType.Week && radlDataType.SelectedValue == "Week")
                    {
                        tstcolformat = "{0:yyyy-MM-dd}";
                    }
                    col.HeaderText = "日期";
                    col.DataFormatString = tstcolformat;
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
                        if (unit == "ng/m3")
                        {
                            unit = "μg/m3";
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
        /// 页面隐藏域控件赋值（小时、日），将数据需要的参数放入隐藏域，各个参数间用‘|’分割，每个参数内部用‘；’分割
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtBegin"></param>
        /// <param name="dtEnd"></param>
        private void SetHiddenData(string[] portIds, IList<IPollutant> factors, DateTime dtBegin, DateTime dtEnd)
        {
            HiddenData.Value = string.Join(";", portIds) + "|" + string.Join(";", factors.Select(p => p.PollutantCode).ToArray())
                             + "|" + dtBegin + "|" + dtEnd + "|" + radlDataType.SelectedValue + "|Air|" + radlDataTypeOri.SelectedValue;
            HiddenChartType.Value = ChartType.SelectedValue;
            HiddenPointFactor.Value = PointFactor.SelectedValue;
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
                             + "|" + timeStr + "|" + "|" + radlDataType.SelectedValue + "|Air|" + radlDataTypeOri.SelectedValue;
            HiddenChartType.Value = ChartType.SelectedValue;
            HiddenPointFactor.Value = PointFactor.SelectedValue;
        }

        #endregion

        #region 绑定图表
        private void BindChart()
        {
            RegisterScript("InitGroupChart();");
        }
        #endregion

        #region 服务器端控件事件处理
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
        /// 数据行绑定处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridAudit_ItemDataBound(object sender, GridItemEventArgs e)
        {
            GridDataItem item = e.Item as GridDataItem;
            if (e.Item is GridDataItem)
            {
                if (item["PointId"] != null)
                {
                    GridTableCell pointCell = (GridTableCell)item["PointId"];

                    MonitoringPointEntity monitoringPoint = monitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(pointCell.Text.Trim()));

                    if (monitoringPoint != null)
                    {
                        pointCell.Text = monitoringPoint.MonitoringPointName;
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
            //hdPointName.Value = cbPoint.SelectedText;
            if (tabStrip.SelectedTab.Text == "图表")
            {
                BindGrid("图表");
                BindChart();
            }
            else
            {
                gridAudit.CurrentPageIndex = 0;
                gridAudit.Rebind();
                FirstLoadChart.Value = "1";
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
                //string[] portIds = { cbPoint.SelectedValue };
                string[] portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
                //string[] factorCodes = cbFactor.CheckedItems.Select(it => it.Value).ToArray();
                string[] factorCodes = factors.Select(it => it.PollutantCode).ToArray();
                DateTime dateBegion = DateTime.Now;
                DateTime dateEnd = DateTime.Now;
                string orderBy = "PointId asc,DateTime desc";


                //每页显示数据个数            
                int pageSize = int.MaxValue;
                int pageNo = 0;
                int recordTotal = 0;

                if (radlDataType.SelectedValue == "Day")
                {
                    orderBy = "PointId asc,Tstamp desc";
                    dateBegion = dayBegin.SelectedDate.Value;
                    dateEnd = dayEnd.SelectedDate.Value.AddDays(1).AddSeconds(-1);


                    DataView dv = m_HourData.GetHourDataPagerNew(portIds, factorCodes, dateBegion, dateEnd, pageSize, pageNo, out recordTotal, orderBy);//日类型按 小时数据查询

                    DataTable dt = UpdateExportColumnName(dv);

                    decimal value = 0M;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            if (dt.Columns[j].ColumnName == "PM2.5")
                            {
                                if (!string.IsNullOrWhiteSpace(dt.Rows[i][j].ToString()))
                                {
                                    if (decimal.TryParse(dt.Rows[i][j].ToString(), out value))
                                    {
                                        dt.Rows[i][j] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }

                            }
                        }
                    }

                    ExcelHelper.DataTableToExcel(dt, "气溶胶离子数据", "气溶胶离子数据", this.Page);
                }
                else if (radlDataType.SelectedValue == "Month")
                {
                    int monthB = monthBegin.SelectedDate.Value.Year;
                    int monthE = monthEnd.SelectedDate.Value.Year;
                    int monthF = monthBegin.SelectedDate.Value.Month;
                    int monthT = monthEnd.SelectedDate.Value.Month;
                    DateTime mtBegin = monthBegin.SelectedDate.Value;
                    //本月第一天时间 
                    dateBegion = mtBegin.AddDays(-(mtBegin.Day) + 1);
                    DateTime mtEnd = monthEnd.SelectedDate.Value;
                    //将本月月数+1 
                    DateTime dt2 = mtEnd.AddMonths(1);
                    //本月最后一天时间 
                    dateEnd = dt2.AddDays(-(mtEnd.Day)).AddDays(1).AddSeconds(-1);
                    DataView dv = m_DayData.GetDayData(portIds, factorCodes, dateBegion, dateEnd, pageSize, pageNo, out recordTotal, orderBy); //月类型 按日数据查询

                    DataTable dt = UpdateExportColumnName(dv);

                    decimal value = 0M;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            if (dt.Columns[j].ColumnName == "PM2.5")
                            {
                                if (!string.IsNullOrWhiteSpace(dt.Rows[i][j].ToString()))
                                {
                                    if (decimal.TryParse(dt.Rows[i][j].ToString(), out value))
                                    {
                                        dt.Rows[i][j] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }

                            }
                        }
                    }

                    ExcelHelper.DataTableToExcel(dt, "气溶胶离子数据", "气溶胶离子数据", this.Page);
                }

                else if (radlDataType.SelectedValue == "Week")
                {
                    dateBegion = Convert.ToDateTime(weekFrom.SelectedValue);
                    dateEnd = DateTime.ParseExact(weekTo.SelectedValue, "yyyy-MM-dd", null).AddDays(6).AddDays(1).AddSeconds(-1);
                    DataView dv = m_DayData.GetDayData(portIds, factorCodes, dateBegion, dateEnd, pageSize, pageNo, out recordTotal, orderBy); //周类型 按日数据查询
                    DataTable dt = UpdateExportColumnName(dv);

                    decimal value = 0M;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            if (dt.Columns[j].ColumnName == "PM2.5")
                            {
                                if (!string.IsNullOrWhiteSpace(dt.Rows[i][j].ToString()))
                                {
                                    if (decimal.TryParse(dt.Rows[i][j].ToString(), out value))
                                    {
                                        dt.Rows[i][j] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }

                            }
                        }
                    }
                    ExcelHelper.DataTableToExcel(dt, "气溶胶离子数据", "气溶胶离子数据", this.Page);
                }
            }
        }
        /// <summary>
        /// 修改要导出的数据表中的列名
        /// </summary>
        /// <param name="dv">原始数据表</param>
        /// <returns></returns>
        private DataTable UpdateExportColumnName(DataView dv)
        {
            DataTable dtOld = dv.ToTable();
            foreach (DataColumn dc in dtOld.Columns)
            {
                if (dc.ColumnName == "DateTime")
                {
                    dtOld.Columns[dc.ColumnName].ColumnName = "Tstamp";
                }
            }
            DataTable dtNew = dtOld.Clone();
            dtNew.Columns["Tstamp"].DataType = typeof(string);
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


            dtNew.Columns.Add("测点", typeof(string)).SetOrdinal(0);
            for (int i = 0; i < dtNew.Rows.Count; i++)
            {
                DataRow drNew = dtNew.Rows[i];

                MonitoringPointEntity monitoringPoint = monitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(drNew["PointId"].ToString()));

                if (monitoringPoint != null)
                {
                    drNew["测点"] = monitoringPoint.MonitoringPointName;
                }

                if (radlDataType.SelectedValue == "Day")
                {
                    drNew["Tstamp"] = string.Format("{0:MM-dd HH:mm}", drNew["Tstamp"].ToString());
                }
                else if (radlDataType.SelectedValue == "Month" || radlDataType.SelectedValue == "Week")
                {
                    drNew["Tstamp"] = string.Format("{0:yyyy-MM-dd}", drNew["Tstamp"].ToString());

                }

            }

            for (int i = 0; i < dtNew.Columns.Count; i++)
            {
                DataColumn dcNew = dtNew.Columns[i];
                if (dcNew.ColumnName.Trim().StartsWith("a", StringComparison.OrdinalIgnoreCase))
                {
                    //dcNew.ColumnName = cbFactor.CheckedItems.Where(t => t.Value.Equals(dcNew.ColumnName.ToString())).Select(t => t.Text).FirstOrDefault();
                    dcNew.ColumnName = factors.Where(t => t.PollutantCode.Equals(dcNew.ColumnName.ToString())).Select(t => t.PollutantName).FirstOrDefault();
                }
                //追加日期
                else if (dcNew.ColumnName == "Tstamp")
                {
                    dcNew.ColumnName = "日期";
                }
                else if (dcNew.ColumnName == "DateTime")
                {
                    dcNew.ColumnName = "日期";
                }

                else if (dcNew.ColumnName == "序号" || dcNew.ColumnName == "PointId")
                {
                    dtNew.Columns.Remove(dcNew);
                    i--;
                }
            }
            return dtNew;
        }

        #endregion
        /// <summary>
        /// 原始数据数据类型时间框选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void radlDataTypeOri_SelectedIndexChanged(object sender, EventArgs e)
        {
            gridAudit.CurrentPageIndex = 0;
            //一分钟数据
            if (radlDataTypeOri.SelectedValue == "Min1")
            {
                dtpHour.Visible = true;
                dbtHour.Visible = false;
                dbtDay.Visible = false;
                dbtMonth.Visible = false;
                dbtSeason.Visible = false;
                dbtYear.Visible = false;
                dbtWeek.Visible = false;
                dtpBegin.DateInput.DateFormat = "yyyy-MM-dd HH:mm";
                dtpEnd.DateInput.DateFormat = "yyyy-MM-dd HH:mm";
            }
            //五分钟数据
            if (radlDataTypeOri.SelectedValue == "Min5")
            {
                dtpHour.Visible = true;
                dbtHour.Visible = false;
                dbtDay.Visible = false;
                dbtMonth.Visible = false;
                dbtSeason.Visible = false;
                dbtYear.Visible = false;
                dbtWeek.Visible = false;
                dtpBegin.DateInput.DateFormat = "yyyy-MM-dd HH:mm";
                dtpEnd.DateInput.DateFormat = "yyyy-MM-dd HH:mm";
            }
            //小时数据
            if (radlDataTypeOri.SelectedValue == "Min60")
            {
                dtpHour.Visible = true;
                dbtHour.Visible = false;
                dbtDay.Visible = false;
                dbtMonth.Visible = false;
                dbtSeason.Visible = false;
                dbtYear.Visible = false;
                dbtWeek.Visible = false;
                dtpBegin.DateInput.DateFormat = "yyyy-MM-dd HH:00";
                dtpEnd.DateInput.DateFormat = "yyyy-MM-dd HH:00";
            }
        }
        /// <summary>
        /// 审核数据数据类型时间框选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void radlDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            gridAudit.CurrentPageIndex = 0;
            //小时数据
            if (radlDataType.SelectedValue == "Hour")
            {
                dtpHour.Visible = false;
                dbtHour.Visible = true;
                dbtDay.Visible = false;
                dbtMonth.Visible = false;
                dbtYear.Visible = false;
                dbtWeek.Visible = false;
                dbtSeason.Visible = false;
            }
            //日数据
            else if (radlDataType.SelectedValue == "Day")
            {
                dtpHour.Visible = false;
                dbtDay.Visible = true;
                dbtHour.Visible = false;
                dbtMonth.Visible = false;
                dbtYear.Visible = false;
                dbtWeek.Visible = false;
                dbtSeason.Visible = false;
            }
            //周数据
            else if (radlDataType.SelectedValue == "Week")
            {
                dtpHour.Visible = false;
                dbtWeek.Visible = true;
                dbtYear.Visible = false;
                dbtMonth.Visible = false;
                dbtDay.Visible = false;
                dbtHour.Visible = false;
                dbtSeason.Visible = false;
            }
            //月数据
            else if (radlDataType.SelectedValue == "Month")
            {
                dtpHour.Visible = false;
                dbtMonth.Visible = true;
                dbtDay.Visible = false;
                dbtHour.Visible = false;
                dbtYear.Visible = false;
                dbtWeek.Visible = false;
                dbtSeason.Visible = false;
            }
            //季数据
            else if (radlDataType.SelectedValue == "Season")
            {
                dtpHour.Visible = false;
                dbtSeason.Visible = true;
                dbtMonth.Visible = false;
                dbtDay.Visible = false;
                dbtHour.Visible = false;
                dbtYear.Visible = false;
                dbtWeek.Visible = false;

            }
            //年数据
            else if (radlDataType.SelectedValue == "Year")
            {
                dtpHour.Visible = false;
                dbtYear.Visible = true;
                dbtMonth.Visible = false;
                dbtDay.Visible = false;
                dbtHour.Visible = false;
                dbtWeek.Visible = false;
                dbtSeason.Visible = false;
            }
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
                radlDataTypeOri.Visible = true;
                radlDataType.Visible = false;
                radlDataTypeOri.SelectedIndex = 2;
            }
            else
            {
                radlDataTypeOri.Visible = false;
                radlDataType.Visible = true;
                radlDataType.SelectedIndex = 0;
            }
        }

        #region 周数据更新日期范围
        protected void weekBegin_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            BindWeekComboBox();
        }

        protected void weekFrom_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            SetLiteral();
        }

        protected void weekEnd_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            BindWeekComboBox();
        }

        protected void weekTo_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            SetLiteral();
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
        /// 显示所选周的日期
        /// </summary>
        private void SetLiteral()
        {
            DateTime endDate = DateTime.ParseExact(weekTo.SelectedValue, "yyyy-MM-dd", null).AddDays(6);
            txtweekF.Text = string.Format("{0:yyyy-MM-dd}", weekFrom.SelectedValue);
            txtweekT.Text = string.Format("{0:yyyy-MM-dd}", endDate);
        }
        #endregion

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

        /// <summary>
        /// 因子、测点类型图表选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PointFactor_SelectedIndexChanged(object sender, EventArgs e)
        {
            HiddenPointFactor.Value = PointFactor.SelectedValue;
            RegisterScript("PointFactor('" + PointFactor.SelectedValue + "');");
        }

        #region 绑定因子
        public void BindFactors(string CategoryUid, out string Name, out string code, string type = "name")
        {
            IQueryable<PollutantCodeEntity> Pollutant = m_AirPollutantService.RetrieveList().Where(x => x.CategoryUid == CategoryUid);
            string PollutantName = "";
            string PollutantCode = "";
            //if (type == "name")
            //{
            string[] pollutantarry = Pollutant.Select(p => p.PollutantName).ToArray();
            foreach (string strName in pollutantarry)
            {
                PollutantName += strName + ";";
            }
            //}
            //else
            //{
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

    }
}