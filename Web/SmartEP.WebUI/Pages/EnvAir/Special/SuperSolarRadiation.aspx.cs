﻿using log4net;
using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.MonitoringBusinessRepository.Air;
using SmartEP.Service.AutoMonitoring.Air;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.DataAnalyze.Air.DataQuery;
using SmartEP.Utilities.AdoData;
using SmartEP.Utilities.Calendar;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Utilities.IO;
using SmartEP.Utilities.Web.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.Special
{

    public partial class SuperSolarRadiation : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据库处理类
        /// </summary>
        DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();
        /// <summary>
        /// 日数据接口
        /// </summary>
        GranuleSpecialService g_GranuleSpecial = new GranuleSpecialService();
        /// <summary>
        /// 空气日数据
        /// </summary>
        DayReportRepository DayData = Singleton<DayReportRepository>.GetInstance();
        /// <summary>
        /// 空气月数据
        /// </summary>
        MonthReportRepository MonthData = Singleton<MonthReportRepository>.GetInstance();
        /// <summary>
        /// 选择因子
        /// </summary>
        private IList<IPollutant> pfactors = null;
        
        ILog log = LogManager.GetLogger("FileLogging");
        SmartEP.Service.BaseData.Channel.AirPollutantService m_AirPollutantService = new SmartEP.Service.BaseData.Channel.AirPollutantService();
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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.ViewState["Type"] = PageHelper.GetQueryString("Type");
                InitControl();
            }
        }
        DateTime dtms = new DateTime();
        /// <summary>
        /// 默认是否常规站字段为空
        /// </summary>
        string isAudit = string.Empty;
        string isSuper = string.Empty;
        protected override void OnPreInit(EventArgs e)
        {
            isSuper = PageHelper.GetQueryString("superOrNot");
            pointCbxRsm.isSuper("1");
            pointCbxRsmCG.isSuper("0");
            isAudit = "superSR";
            if (isAudit != null && isAudit != "")
            {
                factorCbxRsm.isAudit(isAudit);
            }
        }
        
        ///// <summary>
        ///// 初始化控件
        ///// </summary>
        private void InitControl()
        {
            string factors = System.Configuration.ConfigurationManager.AppSettings["SolarRadiationCode"];

            string VOC1Type = string.Empty;

            string Type = this.ViewState["Type"].ToString();
            string points = System.Configuration.ConfigurationManager.AppSettings["SuperStationPointName"];
            pointCbxRsmCG.SetPointValuesFromNames(points);
            string factorCode1 = System.Configuration.ConfigurationManager.AppSettings["PMCode"];
            string factorCode2 = "TVOC";
            if (Type == "PM")
            {
                factorCode1 = System.Configuration.ConfigurationManager.AppSettings["PMCode"];
            }
            else
            {
                factorCode1 = System.Configuration.ConfigurationManager.AppSettings["O3Code"];
            }
            //因子控件
            foreach (RadComboBoxItem item in factorCom.Items)
            {
                if (factorCode1.Contains(item.Value))
                {
                    item.Checked = true;
                }
            }
            foreach (RadComboBoxItem item in ddlVOC.Items)
            {
                if (factorCode2.Contains(item.Text))
                {
                    item.Checked = true;
                }
            }
            factorCbxRsm.SetFactorValuesFromCodes(factors.Trim(';'));
            //数据类型
            radlDataType.Items.Add(new ListItem("小时", PollutantDataType.Hour.ToString()));
            radlDataType.Items.Add(new ListItem("日", PollutantDataType.Day.ToString()));
            radlDataType.Items.Add(new ListItem("月", PollutantDataType.Month.ToString()));

            //radlDataTypeOri.Items.Add(new ListItem("一分钟", PollutantDataType.Min1.ToString()));
            //radlDataTypeOri.Items.Add(new ListItem("五分钟", PollutantDataType.Min5.ToString()));
            radlDataTypeOri.Items.Add(new ListItem("小时", PollutantDataType.Min60.ToString()));
            radlDataTypeOri.Items.Add(new ListItem("日", PollutantDataType.OriDay.ToString()));
            radlDataTypeOri.Items.Add(new ListItem("月", PollutantDataType.OriMonth.ToString()));

            radlDataType.SelectedValue = PollutantDataType.Hour.ToString();
            radlDataTypeOri.SelectedValue = PollutantDataType.Min60.ToString();

            //时间框初始化
            hourBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddHours(-47));
            hourEnd.SelectedDate = DateTime.Now;
            dtpBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:00:00"));
            dtpEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00:00"));
            dayBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd"));
            dayEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
            monthBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddYears(-1).ToString("yyyy-MM"));
            monthEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToString("yyyy-MM"));


            dtpHour.Visible = true;
            dbtHour.Visible = false;
            dbtDay.Visible = false;
            dbtMonth.Visible = false;
            BindData();
            RegisterScript("ReChart();");

        }
        ///// <summary>
        ///// 绑定数据
        ///// </summary>
        private void BindData()
        {
            try
            {
                #region 默认一级类和二级类的因子
                DataView auditData = new DataView();
                DataTable dtc =new DataTable();
                DataTable auDT = new DataTable();
                DataTable dtInstrumen = new DataTable();
                string dataUnit = string.Empty;
                string WaterFactor = System.Configuration.ConfigurationManager.AppSettings["WeatherFactor"];
                string[] factors = WaterFactor.Split(',');
                string[] portIds = pointCbxRsm.GetPointValues(CbxRsmReturnType.ID);
                string[] portIdsCG = null;
                List<string> pointIDD = new List<string>();
                if (pointCbxRsmCG.GetPointValues(CbxRsmReturnType.ID).Contains("204"))
                {
                    portIdsCG = pointCbxRsmCG.GetPointValues(CbxRsmReturnType.ID);
                }
                else
                {
                    for (int i = 0; i < pointCbxRsmCG.GetPointValues(CbxRsmReturnType.ID).Length; i++)
                    {
                        pointIDD.Add(pointCbxRsmCG.GetPointValues(CbxRsmReturnType.ID)[i]);
                    }
                    pointIDD.Add("204");
                    portIdsCG = pointIDD.ToArray();
                }

                string[] fac = factorCom.CheckedItems.Select(x => x.Value).ToArray();


                List<string> listTypeName = new List<string>();
                List<string> listCTypeName = new List<string>();
                string[] typeName = new string[] { "非甲烷碳氢化合物", "卤代烃类", "含氧（氮）类" };

                pfactors = factorCbxRsm.GetFactors();

                IPollutant iFactorCode = null;
                for (int i = 0; i < fac.Length; i++)
                {
                    iFactorCode = m_AirPollutantService.GetPollutantInfo(fac[i]);

                    pfactors.Add(iFactorCode);
                }
                int pageSize = int.MaxValue;
                int pageNo = 0;
                int recordTotal = 0;
                IPollutant iiFactorCode = m_AirPollutantService.GetPollutantInfo("a05024");
                int Ode = int.TryParse(iiFactorCode.PollutantDecimalNum, out Ode) ? Ode : 3;
                int Ade = Ode;
                DataTable dt = new DataTable();
                DataTable dtMonth = new DataTable();
                dtMonth.Columns.Add("PointId", typeof(System.String));//污染物名称 参数
                dtMonth.Columns.Add("Year", typeof(System.Int32));//时间戳
                dtMonth.Columns.Add("MonthOfYear", typeof(System.Int32));//时间戳

                dt.Columns.Add("PointId", typeof(System.String));//污染物名称 参数
                dt.Columns.Add("Tstamp", typeof(System.DateTime));//时间戳
                #region 查询因子sql
                string sql1 = string.Format(@"select PollutantCode from [dbo].[DT_VOC3Type]
where VOC1TypeGuid IN (SELECT RowGuid from [dbo].[DT_VOC1Type] where VOC1Type ='{0}') "
                    , "非甲烷碳氢化合物");
                string sql2 = string.Format(@"select PollutantCode from [dbo].[DT_VOC3Type]
where VOC1TypeGuid IN (SELECT RowGuid from [dbo].[DT_VOC1Type] where VOC1Type ='{0}') "
                    , "卤代烃类VOCs");
                string sql3 = string.Format(@"select PollutantCode from [dbo].[DT_VOC3Type]
where VOC1TypeGuid IN (SELECT RowGuid from [dbo].[DT_VOC1Type] where VOC1Type ='{0}') "
                    , "含氧（氮）类VOCs");
                string sql4 = string.Format(@"select PollutantCode from [dbo].[DT_VOC3Type]
where PollutantCode in (SELECT  PollutantCode FROM [Standard].[TB_PollutantCode] where VOCType!='2')");
                string sqle1 = string.Format(@"select PollutantCode from [dbo].[DT_VOC3Type] 
                                                    where VOC2TypeGuid='{0}'"
                        , "06a02408-6eab-4188-b442-86dd8e97654c");
                string sqle2 = string.Format(@"select PollutantCode from [dbo].[DT_VOC3Type] 
                                                    where VOC2TypeGuid='{0}'"
                        , "5b1918b9-7c92-477a-8e23-64cbae6477f6");
                string sqle3 = string.Format(@"select PollutantCode from [dbo].[DT_VOC3Type] 
                                                    where VOC2TypeGuid='{0}'"
                        , "7bc5f3d8-7cb3-4273-a646-1ed7df60cdd8");
                string sqle4 = string.Format(@"select PollutantCode from [dbo].[DT_VOC3Type] 
                                                    where VOC2TypeGuid='{0}'"
                        , "a41db3c8-0fb4-4b0e-b1b0-e5950f04eff6");
                string sqle5 = string.Format(@"select PollutantCode from [dbo].[DT_VOC3Type] 
                                                    where VOC2TypeGuid='{0}'"
                        , "e9607fce-75dc-4134-9a8d-af2a1eb4a7bf");
                string sqle6 = string.Format(@"select PollutantCode from [dbo].[DT_VOC3Type] 
                                                    where VOC2TypeGuid='{0}'"
                        , "fb1fc34b-770f-4141-b75a-015919725e0b");
                string sqle7 = string.Format(@"select PollutantCode from [dbo].[DT_VOC3Type] 
                                                    where VOC2TypeGuid='{0}'"
                        , "053c74fd-d1ae-4341-b258-1788079970bd");
                string sqle8 = string.Format(@"select PollutantCode from [dbo].[DT_VOC3Type] 
                                                    where VOC2TypeGuid='{0}'"
                        , "1eaac416-0d69-48b9-aca1-9ff7904907bb");
                string sqle9 = string.Format(@"select PollutantCode from [dbo].[DT_VOC3Type] 
                                                    where VOC2TypeGuid='{0}'"
                        , "21de4143-2c28-4256-b71e-6cb5ce63e417");
                string sqle10 = string.Format(@"select PollutantCode from [dbo].[DT_VOC3Type] 
                                                    where VOC2TypeGuid='{0}' and PollutantCode in (SELECT  PollutantCode FROM [Standard].[TB_PollutantCode] where VOCType!='2')"
                        , "a0bad5d7-9eec-4fa4-9c36-828aad78041d");
                string sqle11 = string.Format(@"select PollutantCode from [dbo].[DT_VOC3Type] 
                                                    where VOC2TypeGuid='{0}'"
                        , "3bbe4b30-53e4-48a8-a884-c3b38a03b705");
                string sqle12 = string.Format(@"select PollutantCode from [dbo].[DT_VOC3Type] 
                                                    where VOC2TypeGuid='{0}' and PollutantCode in (SELECT  PollutantCode FROM [Standard].[TB_PollutantCode] where VOCType!='2')"
                        , "8198b6fc-7a77-427d-8e3e-9c9228ac168c");
                string sqle13 = string.Format(@"select PollutantCode from [dbo].[DT_VOC3Type] 
                                                    where VOC2TypeGuid='{0}'"
                        , "8c9ce5f3-4716-485e-95e1-72608b2843ce");
                string sqle14 = string.Format(@"select PollutantCode from [dbo].[DT_VOC3Type] 
                                                    where VOC2TypeGuid='{0}'"
                        , "e5f83fd9-0b77-4d1b-935f-1826fddcc343");

                #endregion

                //一级类因子Table
                DataTable dt1 = null;
                DataTable dt2 = null;
                DataTable dt3 = null;
                DataTable dt4 = null;
                //二级类因子Table
                DataTable dte1 = null;
                DataTable dte2 = null;
                DataTable dte3 = null;
                DataTable dte4 = null;
                DataTable dte5 = null;
                DataTable dte6 = null;
                DataTable dte7 = null;
                DataTable dte8 = null;
                DataTable dte9 = null;
                DataTable dte10 = null;
                DataTable dte11 = null;
                DataTable dte12 = null;
                DataTable dte13 = null;
                DataTable dte14 = null;
                #region 一级类
                if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("0"))
                {
                    dt.Columns.Add("非甲烷碳氢化合物", typeof(System.String));
                    dtMonth.Columns.Add("非甲烷碳氢化合物", typeof(System.String));
                    listTypeName.Add("非甲烷碳氢化合物");
                    dt1 = g_DatabaseHelper.ExecuteDataTable(sql1, "AMS_BaseDataConnection");
                }
                if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("1"))
                {
                    dt.Columns.Add("卤代烃类", typeof(System.String));
                    dtMonth.Columns.Add("卤代烃类", typeof(System.String));
                    listTypeName.Add("卤代烃类");
                    dt2 = g_DatabaseHelper.ExecuteDataTable(sql2, "AMS_BaseDataConnection");
                }
                if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("2"))
                {
                    dt.Columns.Add("含氧（氮）类", typeof(System.String));
                    dtMonth.Columns.Add("含氧（氮）类", typeof(System.String));
                    listTypeName.Add("含氧（氮）类");
                    dt3 = g_DatabaseHelper.ExecuteDataTable(sql3, "AMS_BaseDataConnection");
                }
                if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("17"))
                {
                    dt.Columns.Add("TVOC", typeof(System.String));
                    dtMonth.Columns.Add("TVOC", typeof(System.String));
                    listTypeName.Add("TVOC");
                    dt4 = g_DatabaseHelper.ExecuteDataTable(sql4, "AMS_BaseDataConnection");
                }
                #endregion

                #region 二级类
                if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("3"))
                {
                    dt.Columns.Add("高碳烷烃C6-C12", typeof(System.String));
                    dtMonth.Columns.Add("高碳烷烃C6-C12", typeof(System.String));
                    listTypeName.Add("高碳烷烃C6-C12");
                    dte1 = g_DatabaseHelper.ExecuteDataTable(sqle1, "AMS_BaseDataConnection");
                }
                if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("4"))
                {
                    dt.Columns.Add("炔烃", typeof(System.String));
                    dtMonth.Columns.Add("炔烃", typeof(System.String));
                    listTypeName.Add("炔烃");
                    dte2 = g_DatabaseHelper.ExecuteDataTable(sqle2, "AMS_BaseDataConnection");
                }
                if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("5"))
                {
                    dt.Columns.Add("高碳烯烃C6-C12", typeof(System.String));
                    dtMonth.Columns.Add("高碳烯烃C6-C12", typeof(System.String));
                    listTypeName.Add("高碳烯烃C6-C12");
                    dte3 = g_DatabaseHelper.ExecuteDataTable(sqle3, "AMS_BaseDataConnection");
                }
                if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("6"))
                {
                    dt.Columns.Add("低碳烯烃C2-C5", typeof(System.String));
                    dtMonth.Columns.Add("低碳烯烃C2-C5", typeof(System.String));
                    listTypeName.Add("低碳烯烃C2-C5");
                    dte4 = g_DatabaseHelper.ExecuteDataTable(sqle4, "AMS_BaseDataConnection");
                }
                if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("7"))
                {
                    dt.Columns.Add("苯系物", typeof(System.String));
                    dtMonth.Columns.Add("苯系物", typeof(System.String));
                    listTypeName.Add("苯系物");
                    dte5 = g_DatabaseHelper.ExecuteDataTable(sqle5, "AMS_BaseDataConnection");
                }
                if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("8"))
                {
                    dt.Columns.Add("低碳烷烃C2-C5", typeof(System.String));
                    dtMonth.Columns.Add("低碳烷烃C2-C5", typeof(System.String));
                    listTypeName.Add("低碳烷烃C2-C5");
                    dte6 = g_DatabaseHelper.ExecuteDataTable(sqle6, "AMS_BaseDataConnection");
                }
                if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("9"))
                {
                    dt.Columns.Add("氟利昂", typeof(System.String));
                    dtMonth.Columns.Add("氟利昂", typeof(System.String));
                    listTypeName.Add("氟利昂");
                    dte7 = g_DatabaseHelper.ExecuteDataTable(sqle7, "AMS_BaseDataConnection");
                }
                if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("10"))
                {
                    dt.Columns.Add("卤代芳香烃", typeof(System.String));
                    dtMonth.Columns.Add("卤代芳香烃", typeof(System.String));
                    listTypeName.Add("卤代芳香烃");
                    dte8 = g_DatabaseHelper.ExecuteDataTable(sqle8, "AMS_BaseDataConnection");
                }
                if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("11"))
                {
                    dt.Columns.Add("卤代烯烃", typeof(System.String));
                    dtMonth.Columns.Add("卤代烯烃", typeof(System.String));
                    listTypeName.Add("卤代烯烃");
                    dte9 = g_DatabaseHelper.ExecuteDataTable(sqle9, "AMS_BaseDataConnection");
                }
                if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("12"))
                {
                    dt.Columns.Add("卤代烷烃", typeof(System.String));
                    dtMonth.Columns.Add("卤代烷烃", typeof(System.String));
                    listTypeName.Add("卤代烷烃");
                    dte10 = g_DatabaseHelper.ExecuteDataTable(sqle10, "AMS_BaseDataConnection");
                }
                if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("13"))
                {
                    dt.Columns.Add("含氮有机物", typeof(System.String));
                    dtMonth.Columns.Add("含氮有机物", typeof(System.String));
                    listTypeName.Add("含氮有机物");
                    dte11 = g_DatabaseHelper.ExecuteDataTable(sqle11, "AMS_BaseDataConnection");
                }
                if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("14"))
                {
                    dt.Columns.Add("醛类有机物", typeof(System.String));
                    dtMonth.Columns.Add("醛类有机物", typeof(System.String));
                    listTypeName.Add("醛类有机物");
                    dte12 = g_DatabaseHelper.ExecuteDataTable(sqle12, "AMS_BaseDataConnection");
                }
                if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("15"))
                {
                    dt.Columns.Add("酮类有机物", typeof(System.String));
                    dtMonth.Columns.Add("酮类有机物", typeof(System.String));
                    listTypeName.Add("酮类有机物");
                    dte13 = g_DatabaseHelper.ExecuteDataTable(sqle13, "AMS_BaseDataConnection");
                }
                if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("16"))
                {
                    dt.Columns.Add("醚类有机物", typeof(System.String));
                    dtMonth.Columns.Add("醚类有机物", typeof(System.String));
                    listTypeName.Add("醚类有机物");
                    dte14 = g_DatabaseHelper.ExecuteDataTable(sqle14, "AMS_BaseDataConnection");
                }
                #endregion
                //一级类因子数组
                string[] factors1 = dtToArr(dt1);
                string[] factors2 = dtToArr(dt2);
                string[] factors3 = dtToArr(dt3);
                string[] factors4 = dtToArr(dt4);
                //二级类因子数组
                string[] factorse1 = dtToArr(dte1);
                string[] factorse2 = dtToArr(dte2);
                string[] factorse3 = dtToArr(dte3);
                string[] factorse4 = dtToArr(dte4);
                string[] factorse5 = dtToArr(dte5);
                string[] factorse6 = dtToArr(dte6);
                string[] factorse7 = dtToArr(dte7);
                string[] factorse8 = dtToArr(dte8);
                string[] factorse9 = dtToArr(dte9);
                string[] factorse10 = dtToArr(dte10);
                string[] factorse11 = dtToArr(dte11);
                string[] factorse12 = dtToArr(dte12);
                string[] factorse13 = dtToArr(dte13);
                string[] factorse14 = dtToArr(dte14);

                DataTable dtOneMin = new DataTable();

                #endregion
                if (portIds != null && factors != null)
                {
                    string pointId = portIds[0];
                    if (ddlDataSource.SelectedValue == "OriData")
                    {
                        DateTime dtBegion = dtpBegin.SelectedDate.Value;
                        DateTime dtEnd = dtpEnd.SelectedDate.Value;
                        if (radlDataTypeOri.SelectedValue == "OriDay")
                        {

                            this.ViewState.Add("dt", dt);
                            #region VOC一级类总值
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("0"))
                            {
                                auditData = m_DayOriData.GetDataPagerForAllTime(portIds, factors1, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("非甲烷碳氢化合物", auDT, 0, Ade);
                                }
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("1"))
                            {
                                auditData = m_DayOriData.GetDataPagerForAllTime(portIds, factors2, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("卤代烃类", auDT, 0, Ade);
                                }
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("2"))
                            {
                                auditData = m_DayOriData.GetDataPagerForAllTime(portIds, factors3, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("含氧（氮）类", auDT, 0, Ade);
                                }
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("17"))
                            {
                                auditData = m_DayOriData.GetDataPagerForAllTime(portIds, factors4, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("TVOC", auDT, 0, Ade);
                                }
                            }
                            #endregion
                            #region VOC二级类总值
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("3"))
                            {
                                auditData = m_DayOriData.GetDataPagerForAllTime(portIds, factorse1, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("高碳烷烃C6-C12", auDT, 0, Ade);
                                }
                                
                            }
                            
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("4"))
                            {
                                auditData = m_DayOriData.GetDataPagerForAllTime(portIds, factorse2, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("炔烃", auDT, 0, Ade);
                                }
                                
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("5"))
                            {
                                auditData = m_DayOriData.GetDataPagerForAllTime(portIds, factorse3, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("高碳烯烃C6-C12", auDT, 0, Ade);
                                }
                                
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("6"))
                            {
                                auditData = m_DayOriData.GetDataPagerForAllTime(portIds, factorse4, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("低碳烯烃C2-C5", auDT, 0, Ade);
                                }
                                
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("7"))
                            {
                                auditData = m_DayOriData.GetDataPagerForAllTime(portIds, factorse5, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("苯系物", auDT, 0, Ade);
                                }
                                
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("8"))
                            {
                                auditData = m_DayOriData.GetDataPagerForAllTime(portIds, factorse6, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("低碳烷烃C2-C5", auDT, 0, Ade);
                                }
                                
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("9"))
                            {
                                auditData = m_DayOriData.GetDataPagerForAllTime(portIds, factorse7, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("氟利昂", auDT, 0, Ade);
                                }
                                
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("10"))
                            {
                                auditData = m_DayOriData.GetDataPagerForAllTime(portIds, factorse8, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("卤代芳香烃", auDT, 0, Ade);
                                }
                                
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("11"))
                            {
                                auditData = m_DayOriData.GetDataPagerForAllTime(portIds, factorse9, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("卤代烯烃", auDT, 0, Ade);
                                }
                                
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("12"))
                            {
                                auditData = m_DayOriData.GetDataPagerForAllTime(portIds, factorse10, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("卤代烷烃", auDT, 0, Ade);
                                }
                                
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("13"))
                            {
                                auditData = m_DayOriData.GetDataPagerForAllTime(portIds, factorse11, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("含氮有机物", auDT, 0, Ade);
                                }
                                
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("14"))
                            {
                                auditData = m_DayOriData.GetDataPagerForAllTime(portIds, factorse12, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("醛类有机物", auDT, 0, Ade);
                                }
                                
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("15"))
                            {
                                auditData = m_DayOriData.GetDataPagerForAllTime(portIds, factorse13, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("酮类有机物", auDT, 0, Ade);
                                }
                                
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("16"))
                            {
                                auditData = m_DayOriData.GetDataPagerForAllTime(portIds, factorse14, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("醚类有机物", auDT, 0, Ade);
                                }
                            }
                            #endregion
                            #region 其他因子
                            if (pfactors.Select(p => p.PollutantCode).ToArray().Length > 0)
                            {
                                for (int i = 0; i < pfactors.Select(p => p.PollutantCode).ToArray().Length; i++)
                                {
                                    dt.Columns.Add(pfactors.Select(p => p.PollutantCode).ToArray()[i], typeof(System.String));
                                }
                                auditData = m_DayOriData.GetAvgDataPagers(portIdsCG, pfactors.Select(p => p.PollutantCode).ToArray(), dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                //auditData = m_DayOriData.GetDataPagers(portIds, pfactors.Select(p => p.PollutantCode).ToArray(), dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Length>0)
                                {
                                    dt.Columns["Tstamp"].ColumnName = "DateTime";
                                    for (int i = 0; i < auditData.ToTable().Rows.Count; i++)
                                    {
                                        //DataRow drNew = dt.NewRow();
                                        //dt.Rows.Add(drNew);
                                        for (int j = 0; j < pfactors.Select(p => p.PollutantCode).ToArray().Length; j++)
                                        {
                                            
                                            dt.Rows[i][pfactors.Select(p => p.PollutantCode).ToArray()[j]] = auditData.ToTable().Rows[i][pfactors.Select(p => p.PollutantCode).ToArray()[j]];
                                        }
                                    }
                                }
                                else
                                {
                                    dt.Columns["Tstamp"].ColumnName = "DateTime";
                                    for (int i = 0; i < auditData.ToTable().Rows.Count; i++)
                                    {
                                        DataRow drNew = dt.NewRow();
                                        dt.Rows.Add(drNew);
                                        for (int j = 0; j < pfactors.Select(p => p.PollutantCode).ToArray().Length+1; j++)
                                        {
                                            
                                            dt.Rows[i][auditData.ToTable().Columns[j].ColumnName] = auditData.ToTable().Rows[i][auditData.ToTable().Columns[j].ColumnName];
                                        }
                                    }
                                }
                            }
                            
                            //dt.Columns["旧列名"].ColumnName = "新的列名";
                            for (int i = 0; i < pfactors.Select(p => p.PollutantCode).ToArray().Length; i++)
                            {
                                dt.Columns[pfactors.Select(p => p.PollutantCode).ToArray()[i]].ColumnName = m_AirPollutantService.GetPollutantInfo(pfactors.Select(p => p.PollutantCode).ToArray()[i]).PollutantName;
                                listTypeName.Add(m_AirPollutantService.GetPollutantInfo(pfactors.Select(p => p.PollutantCode).ToArray()[i]).PollutantName);
                            }
                            
                            dtInstrumen = (DataTable)this.ViewState["dt"];
                            if (dtInstrumen != null)
                            {
                                bool a = dtInstrumen.Columns.Contains("Tstamp");
                                if (a)
                                {
                                    if (dtInstrumen.Columns.Count > 2)
                                    {
                                        dtInstrumen.Columns["Tstamp"].SetOrdinal(0);
                                    }
                                }
                                bool b = dtInstrumen.Columns.Contains("DateTime");
                                if (b)
                                {
                                    if (dtInstrumen.Columns.Count > 2)
                                    {
                                        dtInstrumen.Columns["DateTime"].SetOrdinal(0);
                                    }
                                }
                            }

                            #region 数据*1000
                            if (dtInstrumen.Columns.Contains("O₃"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;

                                    if (decimal.TryParse(dtInstrumen.Rows[i]["O₃"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["O₃"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("PM₁₀"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;

                                    if (decimal.TryParse(dtInstrumen.Rows[i]["PM₁₀"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["PM₁₀"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("PM₂.₅"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["PM₂.₅"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["PM₂.₅"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("TSP"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["TSP"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["TSP"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("PM₁.₀"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["PM₁.₀"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["PM₁.₀"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("NOᵧ"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["NOᵧ"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("NO"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["NO"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["NO"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("NOₓ"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["NOₓ"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["NOₓ"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("NOᵧ₁"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ₁"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["NOᵧ₁"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("NOᵧ₂"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ₂"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["NOᵧ₂"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            #endregion
                            string Time = "[";
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++)
                            {
                                //Time += "'" + string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Year, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Month - 1, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Day, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Hour, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Minute, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Second, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Millisecond) + "',";
                                Time += "'" + Convert.ToDateTime(dtInstrumen.Rows[i]["DateTime"].ToString()).ToString("yyyy/MM/dd") + "',";
                            }
                            Time = Time.TrimEnd(',');
                            Time += "]";

                            string data = "[{connectNulls:true,";
                            for (int i = 0; i < listTypeName.ToArray().Length; i++)
                            {
                                if ("TVOC;非甲烷碳氢化合物;卤代烃类;含氧（氮）类;高碳烷烃C6-C12;炔烃;高碳烯烃C6-C12;低碳烯烃C2-C5;苯系物;低碳烷烃C2-C5;氟利昂;卤代芳香烃;卤代烯烃;卤代烷烃;含氮有机物;醛类有机物;酮类有机物;醚类有机物".Contains(dtInstrumen.Columns[i + 2].ColumnName))
                                {
                                    data += "yAxis: 0,tooltip: { valueSuffix: 'ppb' },name:'" + dtInstrumen.Columns[i + 2].ColumnName + "',data:[";
                                }
                                else if (dtInstrumen.Columns[i + 2].ColumnName == "紫外平均辐射" || dtInstrumen.Columns[i + 2].ColumnName == "太阳总辐射")
                                {
                                    data += "yAxis: 3,tooltip: { valueSuffix: 'W/m²' },name:'" + dtInstrumen.Columns[i + 2].ColumnName + "',data:[";
                                }
                                else if (dtInstrumen.Columns[i + 2].ColumnName == "CO")
                                {
                                    data += "yAxis: 2,tooltip: { valueSuffix: 'mg/m3' },name:'" + dtInstrumen.Columns[i + 2].ColumnName + "',data:[";
                                }
                                else
                                {
                                    data += "yAxis: 1,tooltip: { valueSuffix: 'μg/m3' },name:'" + dtInstrumen.Columns[i + 2].ColumnName + "',data:[";
                                }
                                for (int j = 0; j < dtInstrumen.Rows.Count; j++)
                                {
                                    if (dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() == "" || dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() == "0" || dtInstrumen.Rows[j][listTypeName.ToArray()[i]].IsNullOrDBNull())
                                    {
                                        data += "null,";
                                    }
                                    else
                                    {
                                        data += dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() + ",";
                                    }
                                }
                                data += "]}, {connectNulls:true,";
                            }
                            data = data.Substring(0, data.Length - 21);
                            data += "]";
                            hdDate.Value = data;
                            hdDTime.Value = Time;
                            SetHiddenData(portIds, pfactors, dtBegion, dtEnd);
                            #endregion
                        }
                        if (radlDataTypeOri.SelectedValue == "OriMonth")
                        {

                            this.ViewState.Add("dtMonth", dtMonth);
                            #region VOC一级类总值
                            
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("0"))
                            {
                                auditData = m_MonthOriData.GetOriDataPager(portIds, factors1, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Year asc,MonthOfYear asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetMonthDataTable("非甲烷碳氢化合物", auDT, 0, Ade);
                                }
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("1"))
                            {
                                auditData = m_MonthOriData.GetOriDataPager(portIds, factors2, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Year asc,MonthOfYear asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetMonthDataTable("卤代烃类", auDT, 0, Ade);
                                }
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("2"))
                            {
                                auditData = m_MonthOriData.GetOriDataPager(portIds, factors3, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Year asc,MonthOfYear asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetMonthDataTable("含氧（氮）类", auDT, 0, Ade);
                                }
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("17"))
                            {
                                auditData = m_MonthOriData.GetOriDataPager(portIds, factors4, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Year asc,MonthOfYear asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetMonthDataTable("TVOC", auDT, 0, Ade);
                                }
                            }
                            #endregion
                            #region VOC二级类总值
                            
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("3"))
                            {
                                auditData = m_MonthOriData.GetOriDataPager(portIds, factorse1, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Year asc,MonthOfYear asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetMonthDataTable("高碳烷烃C6-C12", auDT, 0, Ade);
                                }

                            }

                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("4"))
                            {
                                auditData = m_MonthOriData.GetOriDataPager(portIds, factorse2, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Year asc,MonthOfYear asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetMonthDataTable("炔烃", auDT, 0, Ade);
                                }

                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("5"))
                            {
                                auditData = m_MonthOriData.GetOriDataPager(portIds, factorse3, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Year asc,MonthOfYear asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetMonthDataTable("高碳烯烃C6-C12", auDT, 0, Ade);
                                }

                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("6"))
                            {
                                auditData = m_MonthOriData.GetOriDataPager(portIds, factorse4, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Year asc,MonthOfYear asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetMonthDataTable("低碳烯烃C2-C5", auDT, 0, Ade);
                                }

                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("7"))
                            {
                                auditData = m_MonthOriData.GetOriDataPager(portIds, factorse5, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Year asc,MonthOfYear asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetMonthDataTable("苯系物", auDT, 0, Ade);
                                }

                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("8"))
                            {
                                auditData = m_MonthOriData.GetOriDataPager(portIds, factorse6, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Year asc,MonthOfYear asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetMonthDataTable("低碳烷烃C2-C5", auDT, 0, Ade);
                                }

                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("9"))
                            {
                                auditData = m_MonthOriData.GetOriDataPager(portIds, factorse7, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Year asc,MonthOfYear asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetMonthDataTable("氟利昂", auDT, 0, Ade);
                                }

                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("10"))
                            {
                                auditData = m_MonthOriData.GetOriDataPager(portIds, factorse8, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Year asc,MonthOfYear asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetMonthDataTable("卤代芳香烃", auDT, 0, Ade);
                                }

                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("11"))
                            {
                                auditData = m_MonthOriData.GetOriDataPager(portIds, factorse9, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Year asc,MonthOfYear asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetMonthDataTable("卤代烯烃", auDT, 0, Ade);
                                }

                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("12"))
                            {
                                auditData = m_MonthOriData.GetOriDataPager(portIds, factorse10, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Year asc,MonthOfYear asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetMonthDataTable("卤代烷烃", auDT, 0, Ade);
                                }

                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("13"))
                            {
                                auditData = m_MonthOriData.GetOriDataPager(portIds, factorse11, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Year asc,MonthOfYear asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetMonthDataTable("含氮有机物", auDT, 0, Ade);
                                }

                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("14"))
                            {
                                auditData = m_MonthOriData.GetOriDataPager(portIds, factorse12, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Year asc,MonthOfYear asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetMonthDataTable("醛类有机物", auDT, 0, Ade);
                                }
                                

                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("15"))
                            {
                                auditData = m_MonthOriData.GetOriDataPager(portIds, factorse13, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Year asc,MonthOfYear asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetMonthDataTable("酮类有机物", auDT, 0, Ade);
                                }
                                

                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("16"))
                            {
                                auditData = m_MonthOriData.GetOriDataPager(portIds, factorse14, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Year asc,MonthOfYear asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetMonthDataTable("醚类有机物", auDT, 0, Ade);
                                }
                            }
                            #endregion
                            #region 其他因子
                            if (pfactors.Select(p => p.PollutantCode).ToArray().Length > 0)
                            {
                                for (int i = 0; i < pfactors.Select(p => p.PollutantCode).ToArray().Length; i++)
                                {
                                    dtMonth.Columns.Add(pfactors.Select(p => p.PollutantCode).ToArray()[i], typeof(System.String));
                                }
                                auditData = m_MonthOriData.GetOriAvgDataPager(portIdsCG, pfactors.Select(p => p.PollutantCode).ToArray(), dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Length>0)
                                {
                                    for (int i = 0; i < auditData.ToTable().Rows.Count; i++)
                                    {
                                        for (int j = 0; j < pfactors.Select(p => p.PollutantCode).ToArray().Length; j++)
                                        {
                                            
                                            dtMonth.Rows[i][pfactors.Select(p => p.PollutantCode).ToArray()[j]] = auditData.ToTable().Rows[i][pfactors.Select(p => p.PollutantCode).ToArray()[j]];
                                        }
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < auditData.ToTable().Rows.Count; i++)
                                    {
                                        DataRow drNew = dtMonth.NewRow();
                                        dtMonth.Rows.Add(drNew);
                                        for (int j = 0; j < pfactors.Select(p => p.PollutantCode).ToArray().Length + 2; j++)
                                        {
                                            
                                            dtMonth.Rows[i][auditData.ToTable().Columns[j].ColumnName] = auditData.ToTable().Rows[i][auditData.ToTable().Columns[j].ColumnName];
                                        }
                                    }
                                }
                            }
                            
                            //dt.Columns["旧列名"].ColumnName = "新的列名";
                            for (int i = 0; i < pfactors.Select(p => p.PollutantCode).ToArray().Length; i++)
                            {
                                dtMonth.Columns[pfactors.Select(p => p.PollutantCode).ToArray()[i]].ColumnName = m_AirPollutantService.GetPollutantInfo(pfactors.Select(p => p.PollutantCode).ToArray()[i]).PollutantName;
                                listTypeName.Add(m_AirPollutantService.GetPollutantInfo(pfactors.Select(p => p.PollutantCode).ToArray()[i]).PollutantName);
                            }
                            dtInstrumen = (DataTable)this.ViewState["dtMonth"];
                            if (dtInstrumen != null)
                            {
                                bool a = dtInstrumen.Columns.Contains("Tstamp");
                                if (a)
                                {
                                    if (dtInstrumen.Columns.Count > 2)
                                    {
                                        dtInstrumen.Columns["Tstamp"].SetOrdinal(0);
                                    }
                                }
                                bool b = dtInstrumen.Columns.Contains("DateTime");
                                if (b)
                                {
                                    if (dtInstrumen.Columns.Count > 2)
                                    {
                                        dtInstrumen.Columns["DateTime"].SetOrdinal(0);
                                    }
                                }
                            }
                            #region 数据*1000
                            if (dtInstrumen.Columns.Contains("O₃"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;

                                    if (decimal.TryParse(dtInstrumen.Rows[i]["O₃"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["O₃"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("PM₁₀"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;

                                    if (decimal.TryParse(dtInstrumen.Rows[i]["PM₁₀"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["PM₁₀"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("PM₂.₅"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["PM₂.₅"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["PM₂.₅"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("TSP"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["TSP"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["TSP"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("PM₁.₀"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["PM₁.₀"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["PM₁.₀"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("NOᵧ"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["NOᵧ"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("NO"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["NO"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["NO"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("NOₓ"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["NOₓ"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["NOₓ"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("NOᵧ₁"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ₁"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["NOᵧ₁"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("NOᵧ₂"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ₂"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["NOᵧ₂"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            #endregion
                            string Time = "[";
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++)
                            {
                                //Time += "'" + string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Year, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Month - 1, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Day, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Hour, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Minute, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Second, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Millisecond) + "',";
                                Time += "'" + dtInstrumen.Rows[i]["Year"].ToString() + "/" + dtInstrumen.Rows[i]["MonthOfYear"].ToString() + "',";
                            }
                            Time = Time.TrimEnd(',');
                            Time += "]";

                            string data = "[{connectNulls:true,";
                            for (int i = 0; i < listTypeName.ToArray().Length; i++)
                            {
                                if ("TVOC;非甲烷碳氢化合物;卤代烃类;含氧（氮）类;高碳烷烃C6-C12;炔烃;高碳烯烃C6-C12;低碳烯烃C2-C5;苯系物;低碳烷烃C2-C5;氟利昂;卤代芳香烃;卤代烯烃;卤代烷烃;含氮有机物;醛类有机物;酮类有机物;醚类有机物".Contains(dtInstrumen.Columns[i + 2].ColumnName))
                                {
                                    data += "yAxis: 0,tooltip: { valueSuffix: 'ppb' },name:'" + dtInstrumen.Columns[i + 3].ColumnName + "',data:[";
                                }
                                else if (dtInstrumen.Columns[i + 3].ColumnName == "紫外平均辐射" || dtInstrumen.Columns[i + 3].ColumnName == "太阳总辐射")
                                {
                                    data += "yAxis: 3,tooltip: { valueSuffix: 'W/m²' },name:'" + dtInstrumen.Columns[i + 3].ColumnName + "',data:[";
                                }
                                else if (dtInstrumen.Columns[i + 3].ColumnName == "CO")
                                {
                                    data += "yAxis: 2,tooltip: { valueSuffix: 'mg/m3' },name:'" + dtInstrumen.Columns[i + 3].ColumnName + "',data:[";
                                }
                                else
                                {
                                    data += "yAxis: 1,tooltip: { valueSuffix: 'μg/m3' },name:'" + dtInstrumen.Columns[i + 3].ColumnName + "',data:[";
                                }
                                for (int j = 0; j < dtInstrumen.Rows.Count; j++)
                                {
                                    if (dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() == "" || dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() == "0" || dtInstrumen.Rows[j][listTypeName.ToArray()[i]].IsNullOrDBNull())
                                    {
                                        data += "null,";
                                    }
                                    else
                                    {
                                        data += dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() + ",";
                                    }
                                }
                                data += "]}, {connectNulls:true,";
                            }
                            data = data.Substring(0, data.Length - 21);
                            data += "]";
                            hdDate.Value = data;
                            hdDTime.Value = Time;
                            SetHiddenData(portIds, pfactors, dtBegion, dtEnd);
                            #endregion

                        }
                        if ( radlDataTypeOri.SelectedValue == "Min60")
                        {
                            //dt.Columns.Add("Tstamp", typeof(System.DateTime));//时间戳
                            this.ViewState.Add("dt", dt);
                            string TypeGuid = string.Empty;
                            #region VOC一级类总值
                            //if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("0"))
                            //{
                            //    auditData = m_Min60Data.GetDataPagerForO3AllTimeVOCs(portIds, factors1, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                            //    auDT = auditData.ToTable();
                            //    if (auDT.DefaultView.Count > 0)
                            //    {
                            //        GetDataTable("非甲烷碳氢化合物", auDT, 0, Ade);
                            //    }
                            //}
                            //if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("1"))
                            //{
                            //    auditData = m_Min60Data.GetDataPagerForO3AllTimeVOCs(portIds, factors2, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                            //    auDT = auditData.ToTable();
                            //    if (auDT.DefaultView.Count > 0)
                            //    {
                            //        GetDataTable("卤代烃类", auDT, 0, Ade);
                            //    }
                            //}
                            //if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("2"))
                            //{
                            //    auditData = m_Min60Data.GetDataPagerForO3AllTimeVOCs(portIds, factors3, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                            //    auDT = auditData.ToTable();
                            //    if (auDT.DefaultView.Count > 0)
                            //    {
                            //        GetDataTable("含氧（氮）类", auDT, 0, Ade);
                            //    }
                            //}
                            //if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("17"))
                            //{
                            //    auditData = m_Min60Data.GetDataPagerForO3AllTimeVOCs(portIds, factors4, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                            //    auDT = auditData.ToTable();
                            //    if (auDT.DefaultView.Count > 0)
                            //    {
                            //        GetDataTable("TVOC", auDT, 0, Ade);
                            //    }
                            //}
                            #endregion
                            #region VOC二级类总值
                            //if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("3"))
                            //{
                            //    auditData = m_Min60Data.GetDataPagerForO3AllTimeVOCs(portIds, factorse1, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                            //    auDT = auditData.ToTable();
                            //    if (auDT.DefaultView.Count > 0)
                            //    {
                            //        GetDataTable("高碳烷烃C6-C12", auDT, 0, Ade);
                            //    }
                            //}

                            //if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("4"))
                            //{
                            //    auditData = m_Min60Data.GetDataPagerForO3AllTimeVOCs(portIds, factorse2, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                            //    auDT = auditData.ToTable();
                            //    if (auDT.DefaultView.Count > 0)
                            //    {
                            //        GetDataTable("炔烃", auDT, 0, Ade);
                            //    }
                                

                            //}
                            //if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("5"))
                            //{
                            //    auditData = m_Min60Data.GetDataPagerForO3AllTimeVOCs(portIds, factorse3, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                            //    auDT = auditData.ToTable();
                            //    if (auDT.DefaultView.Count > 0)
                            //    {
                            //        GetDataTable("高碳烯烃C6-C12", auDT, 0, Ade);
                            //    }
                                

                            //}
                            //if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("6"))
                            //{
                            //    auditData = m_Min60Data.GetDataPagerForO3AllTimeVOCs(portIds, factorse4, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                            //    auDT = auditData.ToTable();
                            //    if (auDT.DefaultView.Count > 0)
                            //    {
                            //        GetDataTable("低碳烯烃C2-C5", auDT, 0, Ade);
                            //    }
                                

                            //}
                            //if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("7"))
                            //{
                            //    auditData = m_Min60Data.GetDataPagerForO3AllTimeVOCs(portIds, factorse5, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                            //    auDT = auditData.ToTable();
                            //    if (auDT.DefaultView.Count > 0)
                            //    {
                            //        GetDataTable("苯系物", auDT, 0, Ade);
                            //    }
                                

                            //}
                            //if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("8"))
                            //{
                            //    auditData = m_Min60Data.GetDataPagerForO3AllTimeVOCs(portIds, factorse6, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                            //    auDT = auditData.ToTable();
                            //    if (auDT.DefaultView.Count > 0)
                            //    {
                            //        GetDataTable("低碳烷烃C2-C5", auDT, 0, Ade);
                            //    }
                                

                            //}
                            //if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("9"))
                            //{
                            //    auditData = m_Min60Data.GetDataPagerForO3AllTimeVOCs(portIds, factorse7, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                            //    auDT = auditData.ToTable();
                            //    if (auDT.DefaultView.Count > 0)
                            //    {
                            //        GetDataTable("氟利昂", auDT, 0, Ade);
                            //    }
                                

                            //}
                            //if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("10"))
                            //{
                            //    auditData = m_Min60Data.GetDataPagerForO3AllTimeVOCs(portIds, factorse8, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                            //    auDT = auditData.ToTable();
                            //    if (auDT.DefaultView.Count > 0)
                            //    {
                            //        GetDataTable("卤代芳香烃", auDT, 0, Ade);
                            //    }
                                

                            //}
                            //if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("11"))
                            //{
                            //    auditData = m_Min60Data.GetDataPagerForO3AllTimeVOCs(portIds, factorse9, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                            //    auDT = auditData.ToTable();
                            //    if (auDT.DefaultView.Count > 0)
                            //    {
                            //        GetDataTable("卤代烯烃", auDT, 0, Ade);
                            //    }
                                

                            //}
                            //if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("12"))
                            //{
                            //    auditData = m_Min60Data.GetDataPagerForO3AllTimeVOCs(portIds, factorse10, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                            //    auDT = auditData.ToTable();
                            //    if (auDT.DefaultView.Count > 0)
                            //    {
                            //        GetDataTable("卤代烷烃", auDT, 0, Ade);
                            //    }
                                
                            //}
                            //if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("13"))
                            //{
                            //    auditData = m_Min60Data.GetDataPagerForO3AllTimeVOCs(portIds, factorse11, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                            //    auDT = auditData.ToTable();
                            //    if (auDT.DefaultView.Count > 0)
                            //    {
                            //        GetDataTable("含氮有机物", auDT, 0, Ade);
                            //    }
                                
                            //}
                            //if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("14"))
                            //{
                            //    auditData = m_Min60Data.GetDataPagerForO3AllTimeVOCs(portIds, factorse12, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                            //    auDT = auditData.ToTable();
                            //    if (auDT.DefaultView.Count > 0)
                            //    {
                            //        GetDataTable("醛类有机物", auDT, 0, Ade);
                            //    }
                            //}
                            //if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("15"))
                            //{

                            //    auditData = m_Min60Data.GetDataPagerForO3AllTimeVOCs(portIds, factorse13, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                            //    auDT = auditData.ToTable();
                            //    if (auDT.DefaultView.Count > 0)
                            //    {
                            //        GetDataTable("酮类有机物", auDT, 0, Ade);
                            //    }
                            //}
                            //if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("16"))
                            //{
                            //    auditData = m_Min60Data.GetDataPagerForO3AllTimeVOCs(portIds, factorse14, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                            //    auDT = auditData.ToTable();
                            //    if (auDT.DefaultView.Count > 0)
                            //    {
                            //        GetDataTable("醚类有机物", auDT, 0, Ade);
                            //    }
                            //}
                            #endregion
                            #region 一级类、二级类总值从表中取
                            List<string> TypeNames=new List<string>();
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("0"))
                            {
                                //TypeGuid += "'0f9c7b24-059e-47d6-bbc2-d1e62bd1eba1',";
                                TypeNames.Add(GetType1Name("0f9c7b24-059e-47d6-bbc2-d1e62bd1eba1"));
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("1"))
                            {
                                //TypeGuid += "'0a153ee0-c7c3-4782-953f-74db4b4c5396',";
                                TypeNames.Add(GetType1Name("0a153ee0-c7c3-4782-953f-74db4b4c5396"));
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("2"))
                            {
                                //TypeGuid += "'219651d8-3463-4de8-941e-a38aae42bf48',";
                                TypeNames.Add(GetType1Name("219651d8-3463-4de8-941e-a38aae42bf48"));
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("17"))
                            {
                                //TypeGuid += "'af6c560e-07b2-422d-8ea6-ec9dc1ca3e91',";
                                TypeNames.Add(GetType1Name("af6c560e-07b2-422d-8ea6-ec9dc1ca3e91"));
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("3"))
                            {
                                //TypeGuid += "'0f9c7b24-059e-47d6-bbc2-d1e62bd1eba1',";
                                TypeNames.Add(GetType2Name("06a02408-6eab-4188-b442-86dd8e97654c"));
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("4"))
                            {
                                //TypeGuid += "'0f9c7b24-059e-47d6-bbc2-d1e62bd1eba1',";
                                TypeNames.Add(GetType2Name("5b1918b9-7c92-477a-8e23-64cbae6477f6"));
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("5"))
                            {
                                //TypeGuid += "'0f9c7b24-059e-47d6-bbc2-d1e62bd1eba1',";
                                TypeNames.Add(GetType2Name("7bc5f3d8-7cb3-4273-a646-1ed7df60cdd8"));
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("6"))
                            {
                                //TypeGuid += "'0f9c7b24-059e-47d6-bbc2-d1e62bd1eba1',";
                                TypeNames.Add(GetType2Name("a41db3c8-0fb4-4b0e-b1b0-e5950f04eff6"));
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("7"))
                            {
                                //TypeGuid += "'0f9c7b24-059e-47d6-bbc2-d1e62bd1eba1',";
                                TypeNames.Add(GetType2Name("e9607fce-75dc-4134-9a8d-af2a1eb4a7bf"));
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("8"))
                            {
                                //TypeGuid += "'0f9c7b24-059e-47d6-bbc2-d1e62bd1eba1',";
                                TypeNames.Add(GetType2Name("fb1fc34b-770f-4141-b75a-015919725e0b"));
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("9"))
                            {
                                //TypeGuid += "'0f9c7b24-059e-47d6-bbc2-d1e62bd1eba1',";
                                TypeNames.Add(GetType2Name("053c74fd-d1ae-4341-b258-1788079970bd"));
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("10"))
                            {
                                //TypeGuid += "'0f9c7b24-059e-47d6-bbc2-d1e62bd1eba1',";
                                TypeNames.Add(GetType2Name("1eaac416-0d69-48b9-aca1-9ff7904907bb"));
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("11"))
                            {
                                //TypeGuid += "'0f9c7b24-059e-47d6-bbc2-d1e62bd1eba1',";
                                TypeNames.Add(GetType2Name("21de4143-2c28-4256-b71e-6cb5ce63e417"));
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("12"))
                            {
                                //TypeGuid += "'0f9c7b24-059e-47d6-bbc2-d1e62bd1eba1',";
                                TypeNames.Add(GetType2Name("a0bad5d7-9eec-4fa4-9c36-828aad78041d"));
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("13"))
                            {
                                //TypeGuid += "'0f9c7b24-059e-47d6-bbc2-d1e62bd1eba1',";
                                TypeNames.Add(GetType2Name("3bbe4b30-53e4-48a8-a884-c3b38a03b705"));
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("14"))
                            {
                                //TypeGuid += "'0f9c7b24-059e-47d6-bbc2-d1e62bd1eba1',";
                                TypeNames.Add(GetType2Name("8198b6fc-7a77-427d-8e3e-9c9228ac168c"));
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("15"))
                            {
                                //TypeGuid += "'0f9c7b24-059e-47d6-bbc2-d1e62bd1eba1',";
                                TypeNames.Add(GetType2Name("8c9ce5f3-4716-485e-95e1-72608b2843ce"));
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("16"))
                            {
                                //TypeGuid += "'0f9c7b24-059e-47d6-bbc2-d1e62bd1eba1',";
                                TypeNames.Add(GetType2Name("e5f83fd9-0b77-4d1b-935f-1826fddcc343"));
                            }
                            string[] TypeN = TypeNames.ToArray();
                            dt = m_Min60Data.GetVOCDataAll(uunit.SelectedValue,TypeN, dtBegion, dtEnd);
                            #endregion
                            #region 其他因子
                            if (pfactors.Select(p => p.PollutantCode).ToArray().Length > 0)
                            {
                                //for (int i = 0; i < pfactors.Select(p => p.PollutantCode).ToArray().Length; i++)
                                //{
                                //    dt.Columns.Add(pfactors.Select(p => p.PollutantCode).ToArray()[i], typeof(System.String));
                                //}
                                auditData = m_Min60Data.GetDataPagerForO3AllTimeNew(portIdsCG, pfactors.Select(p => p.PollutantCode).ToArray(), dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                                DataTable dtNew = auditData.ToTable();
                                for (int i = 0; i < dtNew.Columns.Count; i++)
                                {
                                    DataColumn dcNew = dtNew.Columns[i];
                                    if (dcNew.ColumnName.Contains("_Status") || dcNew.ColumnName.Contains("_DataFlag") || dcNew.ColumnName.Contains("_AuditFlag") || dcNew.ColumnName=="rows")
                                    {
                                        dtNew.Columns.Remove(dcNew);
                                        i--;
                                    }
                                }
                                auditData = dtNew.DefaultView;
                                //if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Length>0)
                                //{
                                //    for (int i = 0; i < auditData.ToTable().Rows.Count; i++)
                                //    {
                                //        for (int j = 0; j < pfactors.Select(p => p.PollutantCode).ToArray().Length; j++)
                                //        {
                                //            //DataRow drNew = dt.NewRow();
                                //            //dt.Rows.Add(drNew);
                                //            dt.Rows[i][pfactors.Select(p => p.PollutantCode).ToArray()[j]] = auditData.ToTable().Rows[i][pfactors.Select(p => p.PollutantCode).ToArray()[j]];
                                //        }
                                //    }
                                //}
                                //else
                                //{
                                //    for (int i = 0; i < auditData.ToTable().Rows.Count; i++)
                                //    {
                                //        DataRow drNew = dt.NewRow();
                                //        dt.Rows.Add(drNew);
                                //        for (int j = 0; j < pfactors.Select(p => p.PollutantCode).ToArray().Length + 1; j++)
                                //        {

                                //            dt.Rows[i][auditData.ToTable().Columns[j].ColumnName] = auditData.ToTable().Rows[i][auditData.ToTable().Columns[j].ColumnName];
                                //        }
                                //    }
                                //}
                            }
                            dtc = auditData.ToTable();
                            //dt.Columns["旧列名"].ColumnName = "新的列名";
                            for (int i = 0; i < pfactors.Select(p => p.PollutantCode).ToArray().Length; i++)
                            {
                                dtc.Columns[pfactors.Select(p => p.PollutantCode).ToArray()[i]].ColumnName = m_AirPollutantService.GetPollutantInfo(pfactors.Select(p => p.PollutantCode).ToArray()[i]).PollutantName;
                                listCTypeName.Add(m_AirPollutantService.GetPollutantInfo(pfactors.Select(p => p.PollutantCode).ToArray()[i]).PollutantName);
                            }
                            
                            dtInstrumen = dt;
                            if (dtInstrumen != null)
                            {
                                bool a = dtInstrumen.Columns.Contains("Tstamp");
                                if (a)
                                {
                                    if (dtInstrumen.Columns.Count > 2)
                                    {
                                        dtInstrumen.Columns["Tstamp"].SetOrdinal(0);
                                    }
                                }
                                bool b = dtInstrumen.Columns.Contains("DateTime");
                                if (b)
                                {
                                    if (dtInstrumen.Columns.Count > 2)
                                    {
                                        dtInstrumen.Columns["DateTime"].SetOrdinal(0);
                                    }
                                }
                            }
                            #region 数据*1000
                            if (dtc.Columns.Contains("O₃"))
                            {
                                for (int i = 0; i < dtc.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;

                                    if (decimal.TryParse(dtc.Rows[i]["O₃"].ToString(), out value))
                                    {
                                        dtc.Rows[i]["O₃"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtc.Columns.Contains("PM₁₀"))
                            {
                                for (int i = 0; i < dtc.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;

                                    if (decimal.TryParse(dtc.Rows[i]["PM₁₀"].ToString(), out value))
                                    {
                                        dtc.Rows[i]["PM₁₀"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtc.Columns.Contains("PM₂.₅"))
                            {
                                for (int i = 0; i < dtc.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtc.Rows[i]["PM₂.₅"].ToString(), out value))
                                    {
                                        dtc.Rows[i]["PM₂.₅"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtc.Columns.Contains("TSP"))
                            {
                                for (int i = 0; i < dtc.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtc.Rows[i]["TSP"].ToString(), out value))
                                    {
                                        dtc.Rows[i]["TSP"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtc.Columns.Contains("PM₁.₀"))
                            {
                                for (int i = 0; i < dtc.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtc.Rows[i]["PM₁.₀"].ToString(), out value))
                                    {
                                        dtc.Rows[i]["PM₁.₀"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtc.Columns.Contains("NOᵧ"))
                            {
                                for (int i = 0; i < dtc.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtc.Rows[i]["NOᵧ"].ToString(), out value))
                                    {
                                        dtc.Rows[i]["NOᵧ"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtc.Columns.Contains("NO"))
                            {
                                for (int i = 0; i < dtc.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtc.Rows[i]["NO"].ToString(), out value))
                                    {
                                        dtc.Rows[i]["NO"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtc.Columns.Contains("NOₓ"))
                            {
                                for (int i = 0; i < dtc.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtc.Rows[i]["NOₓ"].ToString(), out value))
                                    {
                                        dtc.Rows[i]["NOₓ"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtc.Columns.Contains("NOᵧ₁"))
                            {
                                for (int i = 0; i < dtc.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtc.Rows[i]["NOᵧ₁"].ToString(), out value))
                                    {
                                        dtc.Rows[i]["NOᵧ₁"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtc.Columns.Contains("NOᵧ₂"))
                            {
                                for (int i = 0; i < dtc.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtc.Rows[i]["NOᵧ₂"].ToString(), out value))
                                    {
                                        dtc.Rows[i]["NOᵧ₂"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            #endregion
                            string Time = "[";
                            string data = "[{connectNulls:true,";
                            if (dtInstrumen != null)
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++)
                                {
                                    //Time += "'" + string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Year, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Month - 1, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Day, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Hour, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Minute, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Second, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Millisecond) + "',";
                                    string dtime = dtInstrumen.Rows[i]["Tstamp"].ToString();
                                    Time += "'" + Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).ToString("MM/dd HH时") + "',";
                                }
                                Time = Time.TrimEnd(',');
                                for (int i = 0; i < listTypeName.ToArray().Length; i++)
                                {
                                    if ("TVOC;非甲烷碳氢化合物;卤代烃类;含氧（氮）类;高碳烷烃C6-C12;炔烃;高碳烯烃C6-C12;低碳烯烃C2-C5;苯系物;低碳烷烃C2-C5;氟利昂;卤代芳香烃;卤代烯烃;卤代烷烃;含氮有机物;醛类有机物;酮类有机物;醚类有机物".Contains(dtInstrumen.Columns[i + 2].ColumnName))
                                    {
                                        data += "yAxis: 0,tooltip: { valueSuffix: 'ppb' },name:'" + dtInstrumen.Columns[i + 2].ColumnName + "',data:[";
                                    }
                                    //else if (dtInstrumen.Columns[i + 2].ColumnName == "紫外平均辐射" || dtInstrumen.Columns[i + 2].ColumnName == "太阳总辐射")
                                    //{
                                    //    data += "yAxis: 3,tooltip: { valueSuffix: 'W/m²' },name:'" + dtInstrumen.Columns[i + 2].ColumnName + "',data:[";
                                    //}
                                    //else if (dtInstrumen.Columns[i + 2].ColumnName == "CO")
                                    //{
                                    //    data += "yAxis: 2,tooltip: { valueSuffix: 'mg/m3' },name:'" + dtInstrumen.Columns[i + 2].ColumnName + "',data:[";
                                    //}
                                    //else
                                    //{
                                    //    data += "yAxis: 1,tooltip: { valueSuffix: 'μg/m3' },name:'" + dtInstrumen.Columns[i + 2].ColumnName + "',data:[";
                                    //}

                                    for (int j = 0; j < dtInstrumen.Rows.Count; j++)
                                    {
                                        if (dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() == "" || dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() == "0" || dtInstrumen.Rows[j][listTypeName.ToArray()[i]].IsNullOrDBNull())
                                        {
                                            data += "null,";
                                        }
                                        else
                                        {
                                            data += dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() + dataUnit + ",";
                                        }
                                    }
                                    data += "]}, {connectNulls:true,";
                                }
                            }
                            else
                            {
                                for (int i = 0; i < dtc.Rows.Count; i++)
                                {
                                    //Time += "'" + string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Year, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Month - 1, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Day, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Hour, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Minute, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Second, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Millisecond) + "',";
                                    string dtime = dtc.Rows[i]["Tstamp"].ToString();
                                    Time += "'" + Convert.ToDateTime(dtc.Rows[i]["Tstamp"].ToString()).ToString("MM/dd HH时") + "',";
                                }
                                Time = Time.TrimEnd(',');
                            }
                            
                            Time += "]";

                            
                            
                            if(dtc.Rows.Count>0)
                            {
                                for (int i = 0; i < listCTypeName.ToArray().Length; i++)
                                {
                                    if (dtc.Columns[i + 1].ColumnName == "紫外平均辐射" || dtc.Columns[i + 1].ColumnName == "太阳总辐射")
                                    {
                                        data += "yAxis: 3,tooltip: { valueSuffix: 'W/m²' },name:'" + dtc.Columns[i + 1].ColumnName + "',data:[";
                                    }
                                    else if (dtc.Columns[i + 1].ColumnName == "CO")
                                    {
                                        data += "yAxis: 2,tooltip: { valueSuffix: 'mg/m3' },name:'" + dtc.Columns[i + 1].ColumnName + "',data:[";
                                    }
                                    else
                                    {
                                        data += "yAxis: 1,tooltip: { valueSuffix: 'μg/m3' },name:'" + dtc.Columns[i + 1].ColumnName + "',data:[";
                                    }

                                    for (int j = 0; j < dtc.Rows.Count; j++)
                                    {
                                        if (dtc.Rows[j][listCTypeName.ToArray()[i]].ToString() == "" || dtc.Rows[j][listCTypeName.ToArray()[i]].ToString() == "0" || dtc.Rows[j][listCTypeName.ToArray()[i]].IsNullOrDBNull())
                                        {
                                            data += "null,";
                                        }
                                        else
                                        {
                                            data += dtc.Rows[j][listCTypeName.ToArray()[i]].ToString() + dataUnit + ",";
                                        }
                                    }
                                    data += "]}, {connectNulls:true,";
                                }
                            }
                            
                            data = data.Substring(0, data.Length - 21);
                            data += "]";
                            hdDate.Value = data;
                            hdDTime.Value = Time;
                            SetHiddenData(portIds, pfactors, dtBegion, dtEnd);
                            #endregion
                            
                        }
                        if(radlDataTypeOri.SelectedValue == "Min1")
                        {
                            this.ViewState.Add("dt", dt);
                            #region VOC一级类总值
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("0"))
                            {
                                auditData = m_Min1Data.GetDataPager(portIds, factors1, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("非甲烷碳氢化合物", auDT, 0, Ade);
                                }
                                
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("1"))
                            {
                                auditData = m_Min1Data.GetDataPager(portIds, factors2, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("卤代烃类", auDT, 0, Ade);
                                }
                                
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("2"))
                            {
                                auditData = m_Min1Data.GetDataPager(portIds, factors3, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("含氧（氮）类", auDT, 0, Ade);
                                }
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("17"))
                            {
                                auditData = m_Min1Data.GetDataPager(portIds, factors4, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("TVOC", auDT, 0, Ade);
                                }
                            }
                            #endregion
                            #region VOC二级类总值
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("1"))
                            {
                                
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("3"))
                            {
                                auditData = m_Min1Data.GetDataPager(portIds, factorse1, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("高碳烷烃C6-C12", auDT, 0, Ade);
                                }
                                
                            }

                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("4"))
                            {
                                auditData = m_Min1Data.GetDataPager(portIds, factorse2, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("炔烃", auDT, 0, Ade);
                                }
                                
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("5"))
                            {
                                auditData = m_Min1Data.GetDataPager(portIds, factorse3, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("高碳烯烃C6-C12", auDT, 0, Ade);
                                }
                                
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("6"))
                            {
                                auditData = m_Min1Data.GetDataPager(portIds, factorse4, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("低碳烯烃C2-C5", auDT, 0, Ade);
                                }
                                
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("7"))
                            {
                                auditData = m_Min1Data.GetDataPager(portIds, factorse5, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("苯系物", auDT, 0, Ade);
                                }
                                
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("8"))
                            {
                                auditData = m_Min1Data.GetDataPager(portIds, factorse6, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("低碳烷烃C2-C5", auDT, 0, Ade);
                                }
                                
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("9"))
                            {
                                auditData = m_Min1Data.GetDataPager(portIds, factorse7, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("氟利昂", auDT, 0, Ade);
                                }
                                
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("10"))
                            {
                                auditData = m_Min1Data.GetDataPager(portIds, factorse8, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("卤代芳香烃", auDT, 0, Ade);
                                }
                                
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("11"))
                            {
                                auditData = m_Min1Data.GetDataPager(portIds, factorse9, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("卤代烯烃", auDT, 0, Ade);
                                }
                                
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("12"))
                            {
                                auditData = m_Min1Data.GetDataPager(portIds, factorse10, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("卤代烷烃", auDT, 0, Ade);
                                }
                                
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("13"))
                            {
                                auditData = m_Min1Data.GetDataPager(portIds, factorse11, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("含氮有机物", auDT, 0, Ade);
                                }
                                
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("14"))
                            {
                                auditData = m_Min1Data.GetDataPager(portIds, factorse12, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("醛类有机物", auDT, 0, Ade);
                                }
                                
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("15"))
                            {
                                auditData = m_Min1Data.GetDataPager(portIds, factorse13, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("酮类有机物", auDT, 0, Ade);
                                }
                                
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("16"))
                            {
                                auditData = m_Min1Data.GetDataPager(portIds, factorse14, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("醚类有机物", auDT, 0, Ade);
                                }
                            }
                            #endregion
                            #region 其他因子
                            if (pfactors.Select(p => p.PollutantCode).ToArray().Length > 0)
                            {

                                for (int i = 0; i < pfactors.Select(p => p.PollutantCode).ToArray().Length; i++)
                                {
                                    dt.Columns.Add(pfactors.Select(p => p.PollutantCode).ToArray()[i], typeof(System.String));
                                }
                                auditData = m_Min1Data.GetAvgDataPager(portIdsCG, pfactors.Select(p => p.PollutantCode).ToArray(), dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc"); 
                                if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Length > 0)
                                {
                                    for (int i = 0; i < auditData.ToTable().Rows.Count; i++)
                                    {
                                        for (int j = 0; j < pfactors.Select(p => p.PollutantCode).ToArray().Length; j++)
                                        {
                                            //DataRow drNew = dt.NewRow();
                                            //dt.Rows.Add(drNew);
                                            dt.Rows[i][pfactors.Select(p => p.PollutantCode).ToArray()[j]] = auditData.ToTable().Rows[i][pfactors.Select(p => p.PollutantCode).ToArray()[j]];
                                        }
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < auditData.ToTable().Rows.Count; i++)
                                    {
                                        DataRow drNew = dt.NewRow();
                                        dt.Rows.Add(drNew);
                                        for (int j = 0; j < pfactors.Select(p => p.PollutantCode).ToArray().Length + 1; j++)
                                        {

                                            dt.Rows[i][auditData.ToTable().Columns[j].ColumnName] = auditData.ToTable().Rows[i][auditData.ToTable().Columns[j].ColumnName];
                                        }
                                    }
                                }
                            }
                            
                            for (int i = 0; i < pfactors.Select(p => p.PollutantCode).ToArray().Length; i++)
                            {
                                dt.Columns[pfactors.Select(p => p.PollutantCode).ToArray()[i]].ColumnName = m_AirPollutantService.GetPollutantInfo(pfactors.Select(p => p.PollutantCode).ToArray()[i]).PollutantName;
                                listTypeName.Add(m_AirPollutantService.GetPollutantInfo(pfactors.Select(p => p.PollutantCode).ToArray()[i]).PollutantName);
                            }
                            

                            dtInstrumen = (DataTable)this.ViewState["dt"];
                            if (dtInstrumen != null)
                            {
                                bool a = dtInstrumen.Columns.Contains("Tstamp");
                                if (a)
                                {
                                    if (dtInstrumen.Columns.Count > 2)
                                    {
                                        dtInstrumen.Columns["Tstamp"].SetOrdinal(0);
                                    }
                                }
                                bool b = dtInstrumen.Columns.Contains("DateTime");
                                if (b)
                                {
                                    if (dtInstrumen.Columns.Count > 2)
                                    {
                                        dtInstrumen.Columns["DateTime"].SetOrdinal(0);
                                    }
                                }
                            }
                            #region 数据*1000
                            if (dtInstrumen.Columns.Contains("O₃"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;

                                    if (decimal.TryParse(dtInstrumen.Rows[i]["O₃"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["O₃"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("PM₁₀"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;

                                    if (decimal.TryParse(dtInstrumen.Rows[i]["PM₁₀"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["PM₁₀"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("PM₂.₅"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["PM₂.₅"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["PM₂.₅"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("TSP"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["TSP"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["TSP"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("PM₁.₀"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["PM₁.₀"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["PM₁.₀"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("NOᵧ"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["NOᵧ"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("NO"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["NO"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["NO"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("NOₓ"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["NOₓ"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["NOₓ"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("NOᵧ₁"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ₁"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["NOᵧ₁"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("NOᵧ₂"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ₂"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["NOᵧ₂"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            #endregion
                            string Time = "[";
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++)
                            {
                                //Time += "'" + string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Year, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Month - 1, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Day, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Hour, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Minute, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Second, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Millisecond) + "',";
                                string dtime = dtInstrumen.Rows[i]["Tstamp"].ToString();
                                Time += "'" + Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).ToString("MM/dd HH:mm:00") + "',";
                            }
                            Time = Time.TrimEnd(',');
                            Time += "]";

                            string data = "[{connectNulls:true,";
                            for (int i = 0; i < listTypeName.ToArray().Length; i++)
                            {
                                if ("TVOC;非甲烷碳氢化合物;卤代烃类;含氧（氮）类;高碳烷烃C6-C12;炔烃;高碳烯烃C6-C12;低碳烯烃C2-C5;苯系物;低碳烷烃C2-C5;氟利昂;卤代芳香烃;卤代烯烃;卤代烷烃;含氮有机物;醛类有机物;酮类有机物;醚类有机物".Contains(dtInstrumen.Columns[i + 2].ColumnName))
                                {
                                    data += "yAxis: 0,tooltip: { valueSuffix: 'ppb' },name:'" + dtInstrumen.Columns[i + 2].ColumnName + "',data:[";
                                }
                                else if (dtInstrumen.Columns[i + 2].ColumnName == "紫外平均辐射" || dtInstrumen.Columns[i + 2].ColumnName == "太阳总辐射")
                                {
                                    data += "yAxis: 3,tooltip: { valueSuffix: 'W/m²' },name:'" + dtInstrumen.Columns[i + 2].ColumnName + "',data:[";
                                }
                                else if (dtInstrumen.Columns[i + 2].ColumnName == "CO")
                                {
                                    data += "yAxis: 2,tooltip: { valueSuffix: 'mg/m3' },name:'" + dtInstrumen.Columns[i + 2].ColumnName + "',data:[";
                                }
                                else
                                {
                                    data += "yAxis: 1,tooltip: { valueSuffix: 'μg/m3' },name:'" + dtInstrumen.Columns[i + 2].ColumnName + "',data:[";
                                }
                                for (int j = 0; j < dtInstrumen.Rows.Count; j++)
                                {
                                    if (dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() == "" || dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() == "0" || dtInstrumen.Rows[j][listTypeName.ToArray()[i]].IsNullOrDBNull())
                                    {
                                        data += "null,";
                                    }
                                    else
                                    {
                                        data += dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() + ",";
                                    }
                                }
                                data += "]}, {connectNulls:true,";
                            }
                            data = data.Substring(0, data.Length - 21);
                            data += "]";
                            hdDate.Value = data;
                            hdDTime.Value = Time;
                            SetHiddenData(portIds, pfactors, dtBegion, dtEnd);
                            #endregion
                        }
                        if(radlDataTypeOri.SelectedValue == "Min5")
                        {
                            this.ViewState.Add("dt", dt);
                            #region VOC一级类总值
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("0"))
                            {
                                auditData = m_Min5Data.GetDataPager(portIds, factors1, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("非甲烷碳氢化合物", auDT, 0, Ade);
                                }
                                auditData = m_Min5Data.GetDataPager(portIds, factors2, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("卤代烃类", auDT, 0, Ade);
                                }
                                auditData = m_Min5Data.GetDataPager(portIds, factors3, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("含氧（氮）类", auDT, 0, Ade);
                                }
                                auditData = m_Min5Data.GetDataPager(portIds, factors4, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("TVOC", auDT, 0, Ade);
                                }

                            }
                            #endregion
                            #region VOC二级类总值
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("1"))
                            {
                                auditData = m_Min5Data.GetDataPager(portIds, factorse1, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("高碳烷烃C6-C12", auDT, 0, Ade);
                                }
                                auditData = m_Min5Data.GetDataPager(portIds, factorse2, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("炔烃", auDT, 0, Ade);
                                }
                                auditData = m_Min5Data.GetDataPager(portIds, factorse3, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("高碳烯烃C6-C12", auDT, 0, Ade);
                                }
                                auditData = m_Min5Data.GetDataPager(portIds, factorse4, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("低碳烯烃C2-C5", auDT, 0, Ade);
                                }
                                auditData = m_Min5Data.GetDataPager(portIds, factorse5, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("苯系物", auDT, 0, Ade);
                                }
                                auditData = m_Min5Data.GetDataPager(portIds, factorse6, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("低碳烷烃C2-C5", auDT, 0, Ade);
                                }
                                auditData = m_Min5Data.GetDataPager(portIds, factorse7, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("氟利昂", auDT, 0, Ade);
                                }
                                auditData = m_Min5Data.GetDataPager(portIds, factorse8, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("卤代芳香烃", auDT, 0, Ade);
                                }
                                auditData = m_Min5Data.GetDataPager(portIds, factorse9, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("卤代烯烃", auDT, 0, Ade);
                                }
                                auditData = m_Min5Data.GetDataPager(portIds, factorse10, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("卤代烷烃", auDT, 0, Ade);
                                }
                                auditData = m_Min5Data.GetDataPager(portIds, factorse11, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("含氮有机物", auDT, 0, Ade);
                                }
                                auditData = m_Min5Data.GetDataPager(portIds, factorse12, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("醛类有机物", auDT, 0, Ade);
                                }
                                auditData = m_Min5Data.GetDataPager(portIds, factorse13, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("酮类有机物", auDT, 0, Ade);
                                }
                                auditData = m_Min5Data.GetDataPager(portIds, factorse14, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("醚类有机物", auDT, 0, Ade);
                                }
                            }
                            #endregion
                            #region 其他因子
                            if (pfactors.Select(p => p.PollutantCode).ToArray().Length > 0)
                            {

                                for (int i = 0; i < pfactors.Select(p => p.PollutantCode).ToArray().Length; i++)
                                {
                                    dt.Columns.Add(pfactors.Select(p => p.PollutantCode).ToArray()[i], typeof(System.String));
                                }
                                auditData = m_Min5Data.GetAvgDataPager(portIdsCG, pfactors.Select(p => p.PollutantCode).ToArray(), dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                                if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("0") || ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("1"))
                                {
                                    for (int i = 0; i < auditData.ToTable().Rows.Count; i++)
                                    {
                                        for (int j = 0; j < pfactors.Select(p => p.PollutantCode).ToArray().Length; j++)
                                        {
                                            //DataRow drNew = dt.NewRow();
                                            //dt.Rows.Add(drNew);
                                            dt.Rows[i][pfactors.Select(p => p.PollutantCode).ToArray()[j]] = auditData.ToTable().Rows[i][pfactors.Select(p => p.PollutantCode).ToArray()[j]];
                                        }
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < auditData.ToTable().Rows.Count; i++)
                                    {
                                        DataRow drNew = dt.NewRow();
                                        dt.Rows.Add(drNew);
                                        for (int j = 0; j < pfactors.Select(p => p.PollutantCode).ToArray().Length + 1; j++)
                                        {

                                            dt.Rows[i][auditData.ToTable().Columns[j].ColumnName] = auditData.ToTable().Rows[i][auditData.ToTable().Columns[j].ColumnName];
                                        }
                                    }
                                }
                            }
                            
                            //dt.Columns["旧列名"].ColumnName = "新的列名";
                            for (int i = 0; i < pfactors.Select(p => p.PollutantCode).ToArray().Length; i++)
                            {
                                dt.Columns[pfactors.Select(p => p.PollutantCode).ToArray()[i]].ColumnName = m_AirPollutantService.GetPollutantInfo(pfactors.Select(p => p.PollutantCode).ToArray()[i]).PollutantName;
                                listTypeName.Add(m_AirPollutantService.GetPollutantInfo(pfactors.Select(p => p.PollutantCode).ToArray()[i]).PollutantName);
                            }
                            
                            dtInstrumen = (DataTable)this.ViewState["dt"];
                            if (dtInstrumen != null)
                            {
                                bool a = dtInstrumen.Columns.Contains("Tstamp");
                                if (a)
                                {
                                    if (dtInstrumen.Columns.Count > 2)
                                    {
                                        dtInstrumen.Columns["Tstamp"].SetOrdinal(1);
                                    }
                                }
                                bool b = dtInstrumen.Columns.Contains("DateTime");
                                if (b)
                                {
                                    if (dtInstrumen.Columns.Count > 2)
                                    {
                                        dtInstrumen.Columns["DateTime"].SetOrdinal(1);
                                    }
                                }
                            }
                            #region 数据*1000
                            if (dtInstrumen.Columns.Contains("O₃"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;

                                    if (decimal.TryParse(dtInstrumen.Rows[i]["O₃"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["O₃"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("PM₁₀"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;

                                    if (decimal.TryParse(dtInstrumen.Rows[i]["PM₁₀"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["PM₁₀"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("PM₂.₅"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["PM₂.₅"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["PM₂.₅"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("TSP"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["TSP"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["TSP"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("PM₁.₀"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["PM₁.₀"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["PM₁.₀"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("NOᵧ"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["NOᵧ"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("NO"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["NO"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["NO"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("NOₓ"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["NOₓ"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["NOₓ"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("NOᵧ₁"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ₁"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["NOᵧ₁"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("NOᵧ₂"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ₂"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["NOᵧ₂"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            #endregion
                            string Time = "[";
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++)
                            {
                                //Time += "'" + string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Year, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Month - 1, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Day, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Hour, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Minute, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Second, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Millisecond) + "',";
                                string dtime = dtInstrumen.Rows[i]["Tstamp"].ToString();
                                Time += "'" + Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).ToString("MM/dd HH:mm:00") + "',";
                            }
                            Time = Time.TrimEnd(',');
                            Time += "]";

                            string data = "[{connectNulls:true,";
                            for (int i = 0; i < listTypeName.ToArray().Length; i++)
                            {
                                if ("TVOC;非甲烷碳氢化合物;卤代烃类;含氧（氮）类;高碳烷烃C6-C12;炔烃;高碳烯烃C6-C12;低碳烯烃C2-C5;苯系物;低碳烷烃C2-C5;氟利昂;卤代芳香烃;卤代烯烃;卤代烷烃;含氮有机物;醛类有机物;酮类有机物;醚类有机物".Contains(dtInstrumen.Columns[i + 2].ColumnName))
                                {
                                    data += "yAxis: 0,tooltip: { valueSuffix: 'ppb' },name:'" + dtInstrumen.Columns[i + 2].ColumnName + "',data:[";
                                }
                                else if (dtInstrumen.Columns[i + 2].ColumnName == "紫外平均辐射" || dtInstrumen.Columns[i + 2].ColumnName == "太阳总辐射")
                                {
                                    data += "yAxis: 3,tooltip: { valueSuffix: 'W/m²' },name:'" + dtInstrumen.Columns[i + 2].ColumnName + "',data:[";
                                }
                                else if (dtInstrumen.Columns[i + 2].ColumnName == "CO")
                                {
                                    data += "yAxis: 2,tooltip: { valueSuffix: 'mg/m3' },name:'" + dtInstrumen.Columns[i + 2].ColumnName + "',data:[";
                                }
                                else
                                {
                                    data += "yAxis: 1,tooltip: { valueSuffix: 'μg/m3' },name:'" + dtInstrumen.Columns[i + 2].ColumnName + "',data:[";
                                }
                                for (int j = 0; j < dtInstrumen.Rows.Count; j++)
                                {
                                    if (dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() == "" || dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() == "0" || dtInstrumen.Rows[j][listTypeName.ToArray()[i]].IsNullOrDBNull())
                                    {
                                        data += "null,";
                                    }
                                    else
                                    {
                                        data += dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() + ",";
                                    }
                                }
                                data += "]}, {connectNulls:true,";
                            }
                            data = data.Substring(0, data.Length - 21);
                            data += "]";
                            hdDate.Value = data;
                            hdDTime.Value = Time;
                            SetHiddenData(portIds, pfactors, dtBegion, dtEnd);
                            #endregion
                        }
                    }
                    if (ddlDataSource.SelectedValue == "AuditData")
                    {
                        //小时数据
                        if (radlDataType.SelectedValue == "Hour")
                        {
                            DateTime dtBegion = Convert.ToDateTime(hourBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:00"));
                            DateTime dtEnd = Convert.ToDateTime(hourEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:00"));
                            this.ViewState.Add("dt", dt);
                            #region VOC一级类总值
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("0"))
                            {
                                auditData = m_HourData.GetHourDataPagerNew(portIds, factors1, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("非甲烷碳氢化合物", auDT, 0, Ade);
                                }
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("1"))
                            {
                                auditData = m_HourData.GetHourDataPagerNew(portIds, factors2, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("卤代烃类", auDT, 0, Ade);
                                }
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("2"))
                            {
                                auditData = m_HourData.GetHourDataPagerNew(portIds, factors3, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("含氧（氮）类", auDT, 0, Ade);
                                }
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("17"))
                            {
                                auditData = m_HourData.GetHourDataPagerNew(portIds, factors4, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("TVOC", auDT, 0, Ade);
                                }
                            }
                            #endregion
                            #region VOC二级类总值
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("3"))
                            {
                                auditData = m_HourData.GetHourDataPagerNew(portIds, factorse1, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("高碳烷烃C6-C12", auDT, 0, Ade);
                                }
                            }

                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("4"))
                            {
                                auditData = m_HourData.GetHourDataPagerNew(portIds, factorse2, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("炔烃", auDT, 0, Ade);
                                }


                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("5"))
                            {
                                auditData = m_HourData.GetHourDataPagerNew(portIds, factorse3, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("高碳烯烃C6-C12", auDT, 0, Ade);
                                }


                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("6"))
                            {
                                auditData = m_HourData.GetHourDataPagerNew(portIds, factorse4, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("低碳烯烃C2-C5", auDT, 0, Ade);
                                }


                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("7"))
                            {
                                auditData = m_HourData.GetHourDataPagerNew(portIds, factorse5, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("苯系物", auDT, 0, Ade);
                                }


                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("8"))
                            {
                                auditData = m_HourData.GetHourDataPagerNew(portIds, factorse6, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("低碳烷烃C2-C5", auDT, 0, Ade);
                                }


                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("9"))
                            {
                                auditData = m_HourData.GetHourDataPagerNew(portIds, factorse7, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("氟利昂", auDT, 0, Ade);
                                }


                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("10"))
                            {
                                auditData = m_HourData.GetHourDataPagerNew(portIds, factorse8, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("卤代芳香烃", auDT, 0, Ade);
                                }


                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("11"))
                            {
                                auditData = m_HourData.GetHourDataPagerNew(portIds, factorse9, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("卤代烯烃", auDT, 0, Ade);
                                }


                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("12"))
                            {
                                auditData = m_HourData.GetHourDataPagerNew(portIds, factorse10, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("卤代烷烃", auDT, 0, Ade);
                                }

                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("13"))
                            {
                                auditData = m_HourData.GetHourDataPagerNew(portIds, factorse11, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("含氮有机物", auDT, 0, Ade);
                                }

                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("14"))
                            {
                                auditData = m_HourData.GetHourDataPagerNew(portIds, factorse12, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("醛类有机物", auDT, 0, Ade);
                                }
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("15"))
                            {

                                auditData = m_HourData.GetHourDataPagerNew(portIds, factorse13, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("酮类有机物", auDT, 0, Ade);
                                }
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("16"))
                            {
                                auditData = m_HourData.GetHourDataPagerNew(portIds, factorse14, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("醚类有机物", auDT, 0, Ade);
                                }
                            }
                            #endregion
                            #region 其他因子
                            if (pfactors.Select(p => p.PollutantCode).ToArray().Length > 0)
                            {
                                for (int i = 0; i < pfactors.Select(p => p.PollutantCode).ToArray().Length; i++)
                                {
                                    dt.Columns.Add(pfactors.Select(p => p.PollutantCode).ToArray()[i], typeof(System.String));
                                }
                                auditData = m_HourData.GetNewHourDataPagerAvg(portIdsCG, pfactors.Select(p => p.PollutantCode).ToArray(), dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                DataTable dtNew = auditData.ToTable();
                                for (int i = 0; i < dtNew.Columns.Count; i++)
                                {
                                    DataColumn dcNew = dtNew.Columns[i];
                                    if (dcNew.ColumnName.Contains("_Status") || dcNew.ColumnName.Contains("_DataFlag") || dcNew.ColumnName.Contains("_AuditFlag") || dcNew.ColumnName == "rows")
                                    {
                                        dtNew.Columns.Remove(dcNew);
                                        i--;
                                    }
                                }
                                auditData = dtNew.DefaultView;
                                if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Length > 0)
                                {
                                    for (int i = 0; i < auditData.ToTable().Rows.Count; i++)
                                    {
                                        for (int j = 0; j < pfactors.Select(p => p.PollutantCode).ToArray().Length; j++)
                                        {
                                            //DataRow drNew = dt.NewRow();
                                            //dt.Rows.Add(drNew);
                                            dt.Rows[i][pfactors.Select(p => p.PollutantCode).ToArray()[j]] = auditData.ToTable().Rows[i][pfactors.Select(p => p.PollutantCode).ToArray()[j]];
                                        }
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < auditData.ToTable().Rows.Count; i++)
                                    {
                                        DataRow drNew = dt.NewRow();
                                        dt.Rows.Add(drNew);
                                        for (int j = 0; j < pfactors.Select(p => p.PollutantCode).ToArray().Length + 1; j++)
                                        {

                                            dt.Rows[i][auditData.ToTable().Columns[j].ColumnName] = auditData.ToTable().Rows[i][auditData.ToTable().Columns[j].ColumnName];
                                        }
                                    }
                                }
                            }

                            //dt.Columns["旧列名"].ColumnName = "新的列名";
                            for (int i = 0; i < pfactors.Select(p => p.PollutantCode).ToArray().Length; i++)
                            {
                                dt.Columns[pfactors.Select(p => p.PollutantCode).ToArray()[i]].ColumnName = m_AirPollutantService.GetPollutantInfo(pfactors.Select(p => p.PollutantCode).ToArray()[i]).PollutantName;
                                listTypeName.Add(m_AirPollutantService.GetPollutantInfo(pfactors.Select(p => p.PollutantCode).ToArray()[i]).PollutantName);
                            }

                            dtInstrumen = (DataTable)this.ViewState["dt"];
                            if (dtInstrumen != null)
                            {
                                bool a = dtInstrumen.Columns.Contains("Tstamp");
                                if (a)
                                {
                                    if (dtInstrumen.Columns.Count > 2)
                                    {
                                        dtInstrumen.Columns["Tstamp"].SetOrdinal(0);
                                    }
                                }
                                bool b = dtInstrumen.Columns.Contains("DateTime");
                                if (b)
                                {
                                    if (dtInstrumen.Columns.Count > 2)
                                    {
                                        dtInstrumen.Columns["DateTime"].SetOrdinal(0);
                                    }
                                }
                            }
                            #region 数据*1000
                            if (dtInstrumen.Columns.Contains("O₃"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;

                                    if (decimal.TryParse(dtInstrumen.Rows[i]["O₃"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["O₃"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("PM₁₀"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;

                                    if (decimal.TryParse(dtInstrumen.Rows[i]["PM₁₀"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["PM₁₀"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("PM₂.₅"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["PM₂.₅"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["PM₂.₅"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("TSP"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["TSP"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["TSP"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("PM₁.₀"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["PM₁.₀"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["PM₁.₀"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("NOᵧ"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["NOᵧ"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("NO"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["NO"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["NO"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("NOₓ"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["NOₓ"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["NOₓ"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("NOᵧ₁"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ₁"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["NOᵧ₁"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("NOᵧ₂"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ₂"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["NOᵧ₂"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            #endregion
                            string Time = "[";
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++)
                            {
                                //Time += "'" + string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Year, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Month - 1, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Day, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Hour, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Minute, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Second, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Millisecond) + "',";
                                string dtime = dtInstrumen.Rows[i]["DateTime"].ToString();
                                Time += "'" + Convert.ToDateTime(dtInstrumen.Rows[i]["DateTime"].ToString()).ToString("MM/dd HH时") + "',";
                            }
                            Time = Time.TrimEnd(',');
                            Time += "]";

                            string data = "[{connectNulls:true,";
                            for (int i = 0; i < listTypeName.ToArray().Length; i++)
                            {
                                if ("TVOC;非甲烷碳氢化合物;卤代烃类;含氧（氮）类;高碳烷烃C6-C12;炔烃;高碳烯烃C6-C12;低碳烯烃C2-C5;苯系物;低碳烷烃C2-C5;氟利昂;卤代芳香烃;卤代烯烃;卤代烷烃;含氮有机物;醛类有机物;酮类有机物;醚类有机物".Contains(dtInstrumen.Columns[i + 2].ColumnName))
                                {
                                    data += "yAxis: 0,tooltip: { valueSuffix: 'ppb' },name:'" + dtInstrumen.Columns[i + 2].ColumnName + "',data:[";
                                }
                                else if (dtInstrumen.Columns[i + 2].ColumnName == "紫外平均辐射" || dtInstrumen.Columns[i + 2].ColumnName == "太阳总辐射")
                                {
                                    data += "yAxis: 3,tooltip: { valueSuffix: 'W/m²' },name:'" + dtInstrumen.Columns[i + 2].ColumnName + "',data:[";
                                }
                                else if (dtInstrumen.Columns[i + 2].ColumnName == "CO")
                                {
                                    data += "yAxis: 2,tooltip: { valueSuffix: 'mg/m3' },name:'" + dtInstrumen.Columns[i + 2].ColumnName + "',data:[";
                                }
                                else
                                {
                                    data += "yAxis: 1,tooltip: { valueSuffix: 'μg/m3' },name:'" + dtInstrumen.Columns[i + 2].ColumnName + "',data:[";
                                }

                                for (int j = 0; j < dtInstrumen.Rows.Count; j++)
                                {
                                    if (dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() == "" || dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() == "0" || dtInstrumen.Rows[j][listTypeName.ToArray()[i]].IsNullOrDBNull())
                                    {
                                        data += "null,";
                                    }
                                    else
                                    {
                                        data += dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() + dataUnit + ",";
                                    }
                                }
                                data += "]}, {connectNulls:true,";
                            }
                            data = data.Substring(0, data.Length - 21);
                            data += "]";
                            hdDate.Value = data;
                            hdDTime.Value = Time;
                            SetHiddenData(portIds, pfactors, dtBegion, dtEnd);
                            #endregion

                        }
                        //日数据
                        else if (radlDataType.SelectedValue == "Day")
                        {
                            DateTime dtBegion = dayBegin.SelectedDate.Value;
                            DateTime dtEnd = dayEnd.SelectedDate.Value;
                            SetHiddenData(portIds, pfactors, dtBegion, dtEnd);
                            this.ViewState.Add("dt", dt);
                            #region VOC一级类总值
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("0"))
                            {
                                auditData = DayData.GetDataPager(portIds, factors1, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("非甲烷碳氢化合物", auDT, 0, Ade);
                                }
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("1"))
                            {
                                auditData = DayData.GetDataPager(portIds, factors2, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("卤代烃类", auDT, 0, Ade);
                                }
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("2"))
                            {
                                auditData = DayData.GetDataPager(portIds, factors3, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("含氧（氮）类", auDT, 0, Ade);
                                }
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("17"))
                            {
                                auditData = DayData.GetDataPager(portIds, factors4, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("TVOC", auDT, 0, Ade);
                                }
                            }
                            #endregion
                            #region VOC二级类总值
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("3"))
                            {
                                auditData = DayData.GetDataPager(portIds, factorse1, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("高碳烷烃C6-C12", auDT, 0, Ade);
                                }

                            }

                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("4"))
                            {
                                auditData = DayData.GetDataPager(portIds, factorse2, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("炔烃", auDT, 0, Ade);
                                }

                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("5"))
                            {
                                auditData = DayData.GetDataPager(portIds, factorse3, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("高碳烯烃C6-C12", auDT, 0, Ade);
                                }

                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("6"))
                            {
                                auditData = DayData.GetDataPager(portIds, factorse4, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("低碳烯烃C2-C5", auDT, 0, Ade);
                                }

                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("7"))
                            {
                                auditData = DayData.GetDataPager(portIds, factorse5, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("苯系物", auDT, 0, Ade);
                                }

                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("8"))
                            {
                                auditData = DayData.GetDataPager(portIds, factorse6, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("低碳烷烃C2-C5", auDT, 0, Ade);
                                }

                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("9"))
                            {
                                auditData = DayData.GetDataPager(portIds, factorse7, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("氟利昂", auDT, 0, Ade);
                                }

                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("10"))
                            {
                                auditData = DayData.GetDataPager(portIds, factorse8, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("卤代芳香烃", auDT, 0, Ade);
                                }

                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("11"))
                            {
                                auditData = DayData.GetDataPager(portIds, factorse9, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("卤代烯烃", auDT, 0, Ade);
                                }

                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("12"))
                            {
                                auditData = DayData.GetDataPager(portIds, factorse10, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("卤代烷烃", auDT, 0, Ade);
                                }

                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("13"))
                            {
                                auditData = DayData.GetDataPager(portIds, factorse11, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("含氮有机物", auDT, 0, Ade);
                                }

                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("14"))
                            {
                                auditData = DayData.GetDataPager(portIds, factorse12, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("醛类有机物", auDT, 0, Ade);
                                }

                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("15"))
                            {
                                auditData = DayData.GetDataPager(portIds, factorse13, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("酮类有机物", auDT, 0, Ade);
                                }

                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("16"))
                            {
                                auditData = DayData.GetDataPager(portIds, factorse14, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetDataTable("醚类有机物", auDT, 0, Ade);
                                }
                            }
                            #endregion
                            #region 其他因子
                            if (pfactors.Select(p => p.PollutantCode).ToArray().Length > 0)
                            {
                                for (int i = 0; i < pfactors.Select(p => p.PollutantCode).ToArray().Length; i++)
                                {
                                    dt.Columns.Add(pfactors.Select(p => p.PollutantCode).ToArray()[i], typeof(System.String));
                                }
                                auditData = m_DayData.GetAvgDayDataPager(portIdsCG, pfactors, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                //auditData = m_DayOriData.GetDataPagers(portIds, pfactors.Select(p => p.PollutantCode).ToArray(), dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Length > 0)
                                {
                                    dt.Columns["Tstamp"].ColumnName = "DateTime";
                                    for (int i = 0; i < auditData.ToTable().Rows.Count; i++)
                                    {
                                        //DataRow drNew = dt.NewRow();
                                        //dt.Rows.Add(drNew);
                                        for (int j = 0; j < pfactors.Select(p => p.PollutantCode).ToArray().Length; j++)
                                        {

                                            dt.Rows[i][pfactors.Select(p => p.PollutantCode).ToArray()[j]] = auditData.ToTable().Rows[i][pfactors.Select(p => p.PollutantCode).ToArray()[j]];
                                        }
                                    }
                                }
                                else
                                {
                                    dt.Columns["Tstamp"].ColumnName = "DateTime";
                                    for (int i = 0; i < auditData.ToTable().Rows.Count; i++)
                                    {
                                        DataRow drNew = dt.NewRow();
                                        dt.Rows.Add(drNew);
                                        for (int j = 0; j < pfactors.Select(p => p.PollutantCode).ToArray().Length + 1; j++)
                                        {

                                            dt.Rows[i][auditData.ToTable().Columns[j].ColumnName] = auditData.ToTable().Rows[i][auditData.ToTable().Columns[j].ColumnName];
                                        }
                                    }
                                }
                            }

                            //dt.Columns["旧列名"].ColumnName = "新的列名";
                            for (int i = 0; i < pfactors.Select(p => p.PollutantCode).ToArray().Length; i++)
                            {
                                dt.Columns[pfactors.Select(p => p.PollutantCode).ToArray()[i]].ColumnName = m_AirPollutantService.GetPollutantInfo(pfactors.Select(p => p.PollutantCode).ToArray()[i]).PollutantName;
                                listTypeName.Add(m_AirPollutantService.GetPollutantInfo(pfactors.Select(p => p.PollutantCode).ToArray()[i]).PollutantName);
                            }

                            dtInstrumen = (DataTable)this.ViewState["dt"];
                            if (dtInstrumen != null)
                            {
                                bool a = dtInstrumen.Columns.Contains("Tstamp");
                                if (a)
                                {
                                    if (dtInstrumen.Columns.Count > 2)
                                    {
                                        dtInstrumen.Columns["Tstamp"].SetOrdinal(0);
                                    }
                                }
                                bool b = dtInstrumen.Columns.Contains("DateTime");
                                if (b)
                                {
                                    if (dtInstrumen.Columns.Count > 2)
                                    {
                                        dtInstrumen.Columns["DateTime"].SetOrdinal(0);
                                    }
                                }
                            }

                            #region 数据*1000
                            if (dtInstrumen.Columns.Contains("O₃"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;

                                    if (decimal.TryParse(dtInstrumen.Rows[i]["O₃"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["O₃"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("PM₁₀"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;

                                    if (decimal.TryParse(dtInstrumen.Rows[i]["PM₁₀"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["PM₁₀"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("PM₂.₅"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["PM₂.₅"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["PM₂.₅"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("TSP"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["TSP"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["TSP"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("PM₁.₀"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["PM₁.₀"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["PM₁.₀"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("NOᵧ"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["NOᵧ"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("NO"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["NO"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["NO"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("NOₓ"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["NOₓ"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["NOₓ"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("NOᵧ₁"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ₁"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["NOᵧ₁"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("NOᵧ₂"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ₂"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["NOᵧ₂"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            #endregion
                            string Time = "[";
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++)
                            {
                                //Time += "'" + string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Year, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Month - 1, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Day, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Hour, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Minute, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Second, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Millisecond) + "',";
                                Time += "'" + Convert.ToDateTime(dtInstrumen.Rows[i]["DateTime"].ToString()).ToString("yyyy/MM/dd") + "',";
                            }
                            Time = Time.TrimEnd(',');
                            Time += "]";

                            string data = "[{connectNulls:true,";
                            for (int i = 0; i < listTypeName.ToArray().Length; i++)
                            {
                                if ("TVOC;非甲烷碳氢化合物;卤代烃类;含氧（氮）类;高碳烷烃C6-C12;炔烃;高碳烯烃C6-C12;低碳烯烃C2-C5;苯系物;低碳烷烃C2-C5;氟利昂;卤代芳香烃;卤代烯烃;卤代烷烃;含氮有机物;醛类有机物;酮类有机物;醚类有机物".Contains(dtInstrumen.Columns[i + 2].ColumnName))
                                {
                                    data += "yAxis: 0,tooltip: { valueSuffix: 'ppb' },name:'" + dtInstrumen.Columns[i + 2].ColumnName + "',data:[";
                                }
                                else if (dtInstrumen.Columns[i + 2].ColumnName == "紫外平均辐射" || dtInstrumen.Columns[i + 2].ColumnName == "太阳总辐射")
                                {
                                    data += "yAxis: 3,tooltip: { valueSuffix: 'W/m²' },name:'" + dtInstrumen.Columns[i + 2].ColumnName + "',data:[";
                                }
                                else if (dtInstrumen.Columns[i + 2].ColumnName == "CO")
                                {
                                    data += "yAxis: 2,tooltip: { valueSuffix: 'mg/m3' },name:'" + dtInstrumen.Columns[i + 2].ColumnName + "',data:[";
                                }
                                else
                                {
                                    data += "yAxis: 1,tooltip: { valueSuffix: 'μg/m3' },name:'" + dtInstrumen.Columns[i + 2].ColumnName + "',data:[";
                                }
                                for (int j = 0; j < dtInstrumen.Rows.Count; j++)
                                {
                                    if (dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() == "" || dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() == "0" || dtInstrumen.Rows[j][listTypeName.ToArray()[i]].IsNullOrDBNull())
                                    {
                                        data += "null,";
                                    }
                                    else
                                    {
                                        data += dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() + ",";
                                    }
                                }
                                data += "]}, {connectNulls:true,";
                            }
                            data = data.Substring(0, data.Length - 21);
                            data += "]";
                            hdDate.Value = data;
                            hdDTime.Value = Time;
                            SetHiddenData(portIds, pfactors, dtBegion, dtEnd);
                            #endregion
                        }
                        //月数据
                        else if (radlDataType.SelectedValue == "Month")
                        {
                            int monthB = monthBegin.SelectedDate.Value.Year;
                            int monthE = monthEnd.SelectedDate.Value.Year;
                            int monthF = monthBegin.SelectedDate.Value.Month;
                            int monthT = monthEnd.SelectedDate.Value.Month;
                            DateTime dtBegion = monthBegin.SelectedDate.Value;
                            DateTime dtEnd = monthEnd.SelectedDate.Value;
                            this.ViewState.Add("dtMonth", dtMonth);
                            #region VOC一级类总值

                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("0"))
                            {
                                auditData = MonthData.GetDataPager(portIds, factors1, monthB, monthF, monthE, monthT, pageSize, pageNo, out recordTotal, "PointId asc,Year asc,MonthOfYear asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetMonthDataTable("非甲烷碳氢化合物", auDT, 0, Ade);
                                }
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("1"))
                            {
                                auditData = MonthData.GetDataPager(portIds, factors2, monthB, monthF, monthE, monthT, pageSize, pageNo, out recordTotal, "PointId asc,Year asc,MonthOfYear asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetMonthDataTable("卤代烃类", auDT, 0, Ade);
                                }
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("2"))
                            {
                                auditData = MonthData.GetDataPager(portIds, factors3, monthB, monthF, monthE, monthT, pageSize, pageNo, out recordTotal, "PointId asc,Year asc,MonthOfYear asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetMonthDataTable("含氧（氮）类", auDT, 0, Ade);
                                }
                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("17"))
                            {
                                auditData = MonthData.GetDataPager(portIds, factors4, monthB, monthF, monthE, monthT, pageSize, pageNo, out recordTotal, "PointId asc,Year asc,MonthOfYear asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetMonthDataTable("TVOC", auDT, 0, Ade);
                                }
                            }
                            #endregion
                            #region VOC二级类总值

                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("3"))
                            {
                                auditData = MonthData.GetDataPager(portIds, factorse1, monthB, monthF, monthE, monthT, pageSize, pageNo, out recordTotal, "PointId asc,Year asc,MonthOfYear asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetMonthDataTable("高碳烷烃C6-C12", auDT, 0, Ade);
                                }

                            }

                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("4"))
                            {
                                auditData = MonthData.GetDataPager(portIds, factorse2, monthB, monthF, monthE, monthT, pageSize, pageNo, out recordTotal, "PointId asc,Year asc,MonthOfYear asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetMonthDataTable("炔烃", auDT, 0, Ade);
                                }

                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("5"))
                            {
                                auditData = MonthData.GetDataPager(portIds, factorse3, monthB, monthF, monthE, monthT, pageSize, pageNo, out recordTotal, "PointId asc,Year asc,MonthOfYear asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetMonthDataTable("高碳烯烃C6-C12", auDT, 0, Ade);
                                }

                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("6"))
                            {
                                auditData = MonthData.GetDataPager(portIds, factorse4, monthB, monthF, monthE, monthT, pageSize, pageNo, out recordTotal, "PointId asc,Year asc,MonthOfYear asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetMonthDataTable("低碳烯烃C2-C5", auDT, 0, Ade);
                                }

                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("7"))
                            {
                                auditData = MonthData.GetDataPager(portIds, factorse5, monthB, monthF, monthE, monthT, pageSize, pageNo, out recordTotal, "PointId asc,Year asc,MonthOfYear asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetMonthDataTable("苯系物", auDT, 0, Ade);
                                }

                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("8"))
                            {
                                auditData = MonthData.GetDataPager(portIds, factorse6, monthB, monthF, monthE, monthT, pageSize, pageNo, out recordTotal, "PointId asc,Year asc,MonthOfYear asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetMonthDataTable("低碳烷烃C2-C5", auDT, 0, Ade);
                                }

                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("9"))
                            {
                                auditData = MonthData.GetDataPager(portIds, factorse7, monthB, monthF, monthE, monthT, pageSize, pageNo, out recordTotal, "PointId asc,Year asc,MonthOfYear asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetMonthDataTable("氟利昂", auDT, 0, Ade);
                                }

                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("10"))
                            {
                                auditData = MonthData.GetDataPager(portIds, factorse8, monthB, monthF, monthE, monthT, pageSize, pageNo, out recordTotal, "PointId asc,Year asc,MonthOfYear asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetMonthDataTable("卤代芳香烃", auDT, 0, Ade);
                                }

                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("11"))
                            {
                                auditData = MonthData.GetDataPager(portIds, factorse9, monthB, monthF, monthE, monthT, pageSize, pageNo, out recordTotal, "PointId asc,Year asc,MonthOfYear asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetMonthDataTable("卤代烯烃", auDT, 0, Ade);
                                }

                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("12"))
                            {
                                auditData = MonthData.GetDataPager(portIds, factorse10, monthB, monthF, monthE, monthT, pageSize, pageNo, out recordTotal, "PointId asc,Year asc,MonthOfYear asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetMonthDataTable("卤代烷烃", auDT, 0, Ade);
                                }

                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("13"))
                            {
                                auditData = MonthData.GetDataPager(portIds, factorse11, monthB, monthF, monthE, monthT, pageSize, pageNo, out recordTotal, "PointId asc,Year asc,MonthOfYear asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetMonthDataTable("含氮有机物", auDT, 0, Ade);
                                }

                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("14"))
                            {
                                auditData = MonthData.GetDataPager(portIds, factorse12, monthB, monthF, monthE, monthT, pageSize, pageNo, out recordTotal, "PointId asc,Year asc,MonthOfYear asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetMonthDataTable("醛类有机物", auDT, 0, Ade);
                                }


                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("15"))
                            {
                                auditData = MonthData.GetDataPager(portIds, factorse13, monthB, monthF, monthE, monthT, pageSize, pageNo, out recordTotal, "PointId asc,Year asc,MonthOfYear asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetMonthDataTable("酮类有机物", auDT, 0, Ade);
                                }


                            }
                            if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Contains("16"))
                            {
                                auditData = MonthData.GetDataPager(portIds, factorse14, monthB, monthF, monthE, monthT, pageSize, pageNo, out recordTotal, "PointId asc,Year asc,MonthOfYear asc");
                                auDT = auditData.ToTable();
                                if (auDT.DefaultView.Count > 0)
                                {
                                    GetMonthDataTable("醚类有机物", auDT, 0, Ade);
                                }
                            }
                            #endregion
                            #region 其他因子
                            if (pfactors.Select(p => p.PollutantCode).ToArray().Length > 0)
                            {
                                for (int i = 0; i < pfactors.Select(p => p.PollutantCode).ToArray().Length; i++)
                                {
                                    dtMonth.Columns.Add(pfactors.Select(p => p.PollutantCode).ToArray()[i], typeof(System.String));
                                }
                                auditData = m_MonthData.GetMonthDataPagerAvg(portIdsCG, pfactors.Select(p => p.PollutantCode).ToArray(), dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                                if (ddlVOC.CheckedItems.Select(x => x.Value).ToArray().Length > 0)
                                {
                                    for (int i = 0; i < auditData.ToTable().Rows.Count; i++)
                                    {
                                        for (int j = 0; j < pfactors.Select(p => p.PollutantCode).ToArray().Length; j++)
                                        {

                                            dtMonth.Rows[i][pfactors.Select(p => p.PollutantCode).ToArray()[j]] = auditData.ToTable().Rows[i][pfactors.Select(p => p.PollutantCode).ToArray()[j]];
                                        }
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < auditData.ToTable().Rows.Count; i++)
                                    {
                                        DataRow drNew = dtMonth.NewRow();
                                        dtMonth.Rows.Add(drNew);
                                        for (int j = 0; j < pfactors.Select(p => p.PollutantCode).ToArray().Length + 2; j++)
                                        {

                                            dtMonth.Rows[i][auditData.ToTable().Columns[j].ColumnName] = auditData.ToTable().Rows[i][auditData.ToTable().Columns[j].ColumnName];
                                        }
                                    }
                                }
                            }

                            //dt.Columns["旧列名"].ColumnName = "新的列名";
                            for (int i = 0; i < pfactors.Select(p => p.PollutantCode).ToArray().Length; i++)
                            {
                                dtMonth.Columns[pfactors.Select(p => p.PollutantCode).ToArray()[i]].ColumnName = m_AirPollutantService.GetPollutantInfo(pfactors.Select(p => p.PollutantCode).ToArray()[i]).PollutantName;
                                listTypeName.Add(m_AirPollutantService.GetPollutantInfo(pfactors.Select(p => p.PollutantCode).ToArray()[i]).PollutantName);
                            }
                            dtInstrumen = (DataTable)this.ViewState["dtMonth"];
                            if (dtInstrumen != null)
                            {
                                bool a = dtInstrumen.Columns.Contains("Tstamp");
                                if (a)
                                {
                                    if (dtInstrumen.Columns.Count > 2)
                                    {
                                        dtInstrumen.Columns["Tstamp"].SetOrdinal(0);
                                    }
                                }
                                bool b = dtInstrumen.Columns.Contains("DateTime");
                                if (b)
                                {
                                    if (dtInstrumen.Columns.Count > 2)
                                    {
                                        dtInstrumen.Columns["DateTime"].SetOrdinal(0);
                                    }
                                }
                            }
                            #region 数据*1000
                            if (dtInstrumen.Columns.Contains("O₃"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;

                                    if (decimal.TryParse(dtInstrumen.Rows[i]["O₃"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["O₃"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("PM₁₀"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;

                                    if (decimal.TryParse(dtInstrumen.Rows[i]["PM₁₀"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["PM₁₀"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("PM₂.₅"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["PM₂.₅"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["PM₂.₅"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("TSP"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["TSP"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["TSP"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("PM₁.₀"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["PM₁.₀"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["PM₁.₀"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("NOᵧ"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["NOᵧ"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("NO"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["NO"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["NO"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("NOₓ"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["NOₓ"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["NOₓ"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("NOᵧ₁"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ₁"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["NOᵧ₁"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            if (dtInstrumen.Columns.Contains("NOᵧ₂"))
                            {
                                for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                                {
                                    decimal value = 0M;
                                    if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ₂"].ToString(), out value))
                                    {
                                        dtInstrumen.Rows[i]["NOᵧ₂"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                    }
                                }
                            }
                            #endregion
                            string Time = "[";
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++)
                            {
                                //Time += "'" + string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Year, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Month - 1, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Day, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Hour, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Minute, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Second, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Millisecond) + "',";
                                Time += "'" + dtInstrumen.Rows[i]["Year"].ToString() + "/" + dtInstrumen.Rows[i]["MonthOfYear"].ToString() + "',";
                            }
                            Time = Time.TrimEnd(',');
                            Time += "]";

                            string data = "[{connectNulls:true,";
                            for (int i = 0; i < listTypeName.ToArray().Length; i++)
                            {
                                if ("TVOC;非甲烷碳氢化合物;卤代烃类;含氧（氮）类;高碳烷烃C6-C12;炔烃;高碳烯烃C6-C12;低碳烯烃C2-C5;苯系物;低碳烷烃C2-C5;氟利昂;卤代芳香烃;卤代烯烃;卤代烷烃;含氮有机物;醛类有机物;酮类有机物;醚类有机物".Contains(dtInstrumen.Columns[i + 2].ColumnName))
                                {
                                    data += "yAxis: 0,tooltip: { valueSuffix: 'ppb' },name:'" + dtInstrumen.Columns[i + 3].ColumnName + "',data:[";
                                }
                                else if (dtInstrumen.Columns[i + 3].ColumnName == "紫外平均辐射" || dtInstrumen.Columns[i + 3].ColumnName == "太阳总辐射")
                                {
                                    data += "yAxis: 3,tooltip: { valueSuffix: 'W/m²' },name:'" + dtInstrumen.Columns[i + 3].ColumnName + "',data:[";
                                }
                                else if (dtInstrumen.Columns[i + 3].ColumnName == "CO")
                                {
                                    data += "yAxis: 2,tooltip: { valueSuffix: 'mg/m3' },name:'" + dtInstrumen.Columns[i + 3].ColumnName + "',data:[";
                                }
                                else
                                {
                                    data += "yAxis: 1,tooltip: { valueSuffix: 'μg/m3' },name:'" + dtInstrumen.Columns[i + 3].ColumnName + "',data:[";
                                }
                                for (int j = 0; j < dtInstrumen.Rows.Count; j++)
                                {
                                    if (dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() == "" || dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() == "0" || dtInstrumen.Rows[j][listTypeName.ToArray()[i]].IsNullOrDBNull())
                                    {
                                        data += "null,";
                                    }
                                    else
                                    {
                                        data += dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() + ",";
                                    }
                                }
                                data += "]}, {connectNulls:true,";
                            }
                            data = data.Substring(0, data.Length - 21);
                            data += "]";
                            hdDate.Value = data;
                            hdDTime.Value = Time;
                            SetHiddenData(portIds, pfactors, dtBegion, dtEnd);
                            #endregion
                        }
                    }

                }
            }
            catch(Exception ex)
            {
                log.Error(ex.ToString());
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
                if(ddlVOC.SelectedValue!=null)
                {
                    if (ddlVOC.CheckedItems.Select(x => x.Text).ToArray().Length>0)
                    {
                        HiddenData.Value = string.Join(";", portIds) + "|" + string.Join(";", factors.Select(p => p.PollutantCode).ToArray()) +";"+ string.Join(";", ddlVOC.CheckedItems.Select(x => x.Text).ToArray())
                                 + "|" + dtBegin + "|" + dtEnd + "|" + radlDataTypeOri.SelectedValue + "|Air";
                    }
                    else
                    {
                        HiddenData.Value = string.Join(";", portIds) + "|" + string.Join(";", factors.Select(p => p.PollutantCode).ToArray())
                                 + "|" + dtBegin + "|" + dtEnd + "|" + radlDataTypeOri.SelectedValue + "|Air";
                    }
                    
                }
                else
                {
                    HiddenData.Value = string.Join(";", portIds) + "|" + string.Join(";", factors.Select(p => p.PollutantCode).ToArray())
                                 + "|" + dtBegin + "|" + dtEnd + "|" + radlDataTypeOri.SelectedValue + "|Air";
                }
                

            }
            if (ddlDataSource.SelectedValue == "AuditData")
            {
                if (ddlVOC.SelectedValue != null)
                {
                    if (ddlVOC.CheckedItems.Select(x => x.Text).ToArray().Length > 0)
                    {
                        HiddenData.Value = string.Join(";", portIds) + "|" + string.Join(";", factors.Select(p => p.PollutantCode).ToArray()) +";"+ string.Join(";", ddlVOC.CheckedItems.Select(x => x.Text).ToArray())
                                 + "|" + dtBegin + "|" + dtEnd + "|" + radlDataType.SelectedValue + "|Air";
                    }
                    else
                    {
                        HiddenData.Value = string.Join(";", portIds) + "|" + string.Join(";", factors.Select(p => p.PollutantCode).ToArray())
                                     + "|" + dtBegin + "|" + dtEnd + "|" + radlDataType.SelectedValue + "|Air";
                    }
                    
                }
                else
                {
                    HiddenData.Value = string.Join(";", portIds) + "|" + string.Join(";", factors.Select(p => p.PollutantCode).ToArray())
                                 + "|" + dtBegin + "|" + dtEnd + "|" + radlDataType.SelectedValue + "|Air";
                }
                

            }
        }
        /// <summary>
        /// 审核数据数据类型时间框选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void radlDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //小时数据
            if (radlDataType.SelectedValue == "Hour")
            {
                dtpHour.Visible = false;
                dbtHour.Visible = true;
                dbtDay.Visible = false;
                dbtMonth.Visible = false;
            }
            //日数据
            else if (radlDataType.SelectedValue == "Day")
            {
                dtpHour.Visible = false;
                dbtDay.Visible = true;
                dbtHour.Visible = false;
                dbtMonth.Visible = false;
            }
            //周数据
            else if (radlDataType.SelectedValue == "Week")
            {
                dtpHour.Visible = false;
                dbtMonth.Visible = false;
                dbtDay.Visible = false;
                dbtHour.Visible = false;
            }
            //月数据
            else if (radlDataType.SelectedValue == "Month")
            {
                dtpHour.Visible = false;
                dbtMonth.Visible = true;
                dbtDay.Visible = false;
                dbtHour.Visible = false;
            }
            //季数据
            else if (radlDataType.SelectedValue == "Season")
            {
                dtpHour.Visible = false;
                dbtMonth.Visible = false;
                dbtDay.Visible = false;
                dbtHour.Visible = false;

            }
            //年数据
            else if (radlDataType.SelectedValue == "Year")
            {
                dtpHour.Visible = false;
                dbtMonth.Visible = false;
                dbtDay.Visible = false;
                dbtHour.Visible = false;
            }
        }
        /// <summary>
        /// 原始数据数据类型时间框选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void radlDataTypeOri_SelectedIndexChanged(object sender, EventArgs e)
        {
            //一分钟数据
            if (radlDataTypeOri.SelectedValue == "Min1")
            {
                dtpHour.Visible = true;
                dbtHour.Visible = false;
                dbtDay.Visible = false;
                dbtMonth.Visible = false;
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
                dtpBegin.DateInput.DateFormat = "yyyy-MM-dd HH:00";
                dtpEnd.DateInput.DateFormat = "yyyy-MM-dd HH:00";
            }
            //日数据
            if (radlDataTypeOri.SelectedValue == "OriDay")
            {
                dtpHour.Visible = true;
                dbtHour.Visible = false;
                dbtDay.Visible = false;
                dbtMonth.Visible = false;
                dtpBegin.DateInput.DateFormat = "yyyy-MM-dd";
                dtpEnd.DateInput.DateFormat = "yyyy-MM-dd";
            }
            //月数据
            if (radlDataTypeOri.SelectedValue == "OriMonth")
            {
                dtpHour.Visible = true;
                dbtHour.Visible = false;
                dbtDay.Visible = false;
                dbtMonth.Visible = false;
                dtpBegin.DateInput.DateFormat = "yyyy-MM";
                dtpEnd.DateInput.DateFormat = "yyyy-MM";
            }
        }
        /// <summary>
        /// 数据来源选项变化，数据类型选项相应变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlDataSource_SelectedIndexChanged(object sender, Telerik.Web.UI.DropDownListEventArgs e)
        {
            if (ddlDataSource.SelectedIndex == 0)
            {
                radlDataTypeOri.Visible = true;
                radlDataType.Visible = false;
                radlDataTypeOri.SelectedIndex = 0;
            }
            else
            {
                radlDataTypeOri.Visible = false;
                radlDataType.Visible = true;
                radlDataType.SelectedIndex = 0;
            }
        }


        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            BindData();
            RegisterScript("ReChart();");

        }

//        protected void ddlVOC_SelectedIndexChanged(object sender, Telerik.Web.UI.DropDownListEventArgs e)
//        {
//            string factors = System.Configuration.ConfigurationManager.AppSettings["SolarRadiationCode"];

//            string VOC1Type = string.Empty;

//            if (ddlVOC.SelectedValue == "0")
//            {
//                VOC1Type = "'非甲烷碳氢化合物'";
//            }
//            if (ddlVOC.SelectedValue == "1")
//            {
//                VOC1Type = "'卤代烃类'";
//            }
//            if (ddlVOC.SelectedValue == "2")
//            {
//                VOC1Type = "'含氧（氮）类'";
//            }
//            string sql = string.Format(@"select PollutantCode from [dbo].[DT_VOC3Type]
//                                        where VOC1TypeGuid IN (SELECT RowGuid from [dbo].[DT_VOC1Type] where VOC1Type in ({0})) order by VOC1TypeGuid"
//            , VOC1Type);
//            DataTable dt = g_DatabaseHelper.ExecuteDataTable(sql, "AMS_BaseDataConnection");
//            string[] factorCodes = dtToArr(dt);
//            string factorCodel = string.Empty;
//            //分小类
//            factorCodel = string.Join(";", factorCodes);
//            factors += factorCodel;
//            factorCbxRsm.SetFactorValuesFromCodes(factors.Trim(';'));
//        }
        /// <summary>
        /// DataTable转换为一维字符串数组
        /// </summary>
        /// <returns></returns>
        public static string[] dtToArr(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return new string[0];
            }
            else
            {
                string[] sr = new string[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.IsDBNull(dt.Rows[i][0]))
                    {
                        sr[i] = "";
                    }
                    else
                    {
                        sr[i] = dt.Rows[i][0] + "";
                    }
                }
                return sr;
            }
        }
        public string GetType1Name(string Guid)
        {
            string sql = string.Format("select VOC1Type from  [dbo].[DT_VOC1Type] where RowGuid='{0}'", Guid);
            return dtToString(g_DatabaseHelper.ExecuteDataTable(sql, "AMS_BaseDataConnection"));
        }
        public string GetType2Name(string Guid)
        {
            string sql = string.Format("select VOC2Type from  [dbo].[DT_VOC2Type] where RowGuid='{0}'", Guid);
            return dtToString(g_DatabaseHelper.ExecuteDataTable(sql, "AMS_BaseDataConnection"));
        }
        /// <summary>
        /// 获取数据源 日数据
        /// </summary>
        public void GetDataTable(string columnName, DataTable auDT, int type, int DecimalNum)
        {
            if (type == 0)
            {
                DataTable InstrumenTotalDt = (DataTable)this.ViewState["dt"];

                if (auDT.Columns.Contains("DateTime"))
                {
                    auDT.Columns["DateTime"].ColumnName = "Tstamp";
                }
                if (InstrumenTotalDt.DefaultView.Count > 0)//该表已经有数据了 修改数据 否则就是填充该表
                {
                    foreach (DataRow dr in auDT.Rows)
                    {
                        //IPollutant iFactorCode = m_AirPollutantService.GetPollutantInfo(factorCode);

                        decimal wanValue = 0;
                        foreach (DataColumn dc in auDT.Columns)
                        {
                            if (dc.ColumnName != "PointId" && dc.ColumnName != "Tstamp" && dc.ColumnName !="rows" && !dc.ColumnName.Contains("_Status") && !dc.ColumnName.Contains("_AuditFlag") && !dc.ColumnName.Contains("_DataFlag"))
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
                            if (dc.ColumnName != "PointId" && dc.ColumnName != "Tstamp" && dc.ColumnName != "rows" && !dc.ColumnName.Contains("_Status") && !dc.ColumnName.Contains("_AuditFlag") && !dc.ColumnName.Contains("_DataFlag"))
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
                            if (dc.ColumnName != "PointId" && dc.ColumnName != "Year" && dc.ColumnName != "MonthOfYear")
                            {
                                wanValue += dr[dc.ColumnName] != DBNull.Value ? DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[dc.ColumnName]), DecimalNum) : 0;
                            }
                        }

                        DataRow[] drs = InstrumenTotalDt.Select(string.Format("MonthOfYear='{0}'", dr["MonthOfYear"]));

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
                        odr["Year"] = dr["Year"];
                        odr["MonthOfYear"] = dr["MonthOfYear"];
                        foreach (DataColumn dc in auDT.Columns)
                        {
                            if (dc.ColumnName != "PointId" && dc.ColumnName != "Year" && dc.ColumnName != "MonthOfYear")
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
        }
        /// <summary>
        /// 站点因子关联
        /// </summary>
        protected void pointCbxRsmCG_SelectedChanged()
        {

        }
        /// <summary>
        /// DataTable转换为字符串
        /// </summary>
        /// <returns></returns>
        public static string dtToString(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return string.Empty;
            }
            else
            {
                string sr = string.Empty;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sr += dt.Rows[i][0];
                }
                return sr;
            }
        }
    }
}