﻿using Aspose.Cells;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.BusinessRule;
using SmartEP.Service.BaseData.Channel;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.Core.Enums;
using SmartEP.Service.DataAnalyze.Air;
using SmartEP.Service.DataAnalyze.Air.AQIReport;
using SmartEP.Service.DataAnalyze.Enums;
using SmartEP.Service.Frame;
using SmartEP.Utilities.AdoData;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Utilities.Office;
using SmartEP.Utilities.Web.UI;
using SmartEP.WebControl.CbxRsm;
using SmartEP.WebUI.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.Report
{
    /// <summary>
    /// 名称：AirQualityAvgStatistical.cs
    /// 创建人：
    /// 创建日期：
    /// 维护人员：
    /// 最新维护人员：刘晋
    /// 最新维护日期：2017-06-01
    /// 功能摘要：各市区小时数据查询
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class AirQualittyAvgStatistical : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        private DayAQIService m_DayAQIService;
        private AQICalculateService m_AQICalculateService;
        /// <summary>
        /// 数据库处理类
        /// </summary>
        DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();
        /// <summary>
        /// 空气站点信息服务
        /// </summary>
        private MonitoringPointAirService pointAirService;

        /// <summary>
        /// 选择站点
        /// </summary>
        private IList<IPoint> points = null;

        /// <summary>
        /// 选择因子
        /// </summary>
        private IList<PollutantCodeEntity> factors = null;

        string type = "1";
        /// <summary>
        /// 区域Uid集合
        /// </summary>
        List<string> listRegionUids = new List<string>();
        /// <summary>
        /// 区域信息
        /// </summary>
        DataView dvRegion = new DataView();
        static DateTime dtms = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00"));
        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            m_DayAQIService = new DayAQIService();

            m_AQICalculateService = new AQICalculateService();
            pointAirService = new MonitoringPointAirService();
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
            DictionaryService dicService = new DictionaryService();
            MonitoringPointAirService pointAirService = new MonitoringPointAirService();

            hourBegin.SelectedDate = DateTime.Parse(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:00"));
            hourEnd.SelectedDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:00"));
            dtpBegin.SelectedDate = DateTime.Parse(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
            dtpEnd.SelectedDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));

            MonitoringPointAirService m_MonitoringPointAirService = Singleton<MonitoringPointAirService>.GetInstance();
            IQueryable<MonitoringPointEntity> monitoringPointQueryable = m_MonitoringPointAirService.RetrieveAirMPListByEnable();//获取所有启用的空气点位列表
            monitoringPointQueryable = monitoringPointQueryable.Where(p => p.ContrlUid == "6fadff52-2338-4319-9f1d-7317823770ad"     //国控点
                                                                           || p.ContrlUid == "c1158eb6-4d69-4846-a963-d16b9d2794ca"  //对照点
                                                                           || p.ContrlUid == "bdf0837a-eb59-4c4a-a05f-c774a17f3077");//路边站
            //string pointNames = monitoringPointQueryable.Select(t => t.MonitoringPointName)
            //                        .Aggregate((a, b) => a + ";" + b);
            //pointCbxRsm.SetPointValuesFromNames(pointNames);
            string names = ConfigurationManager.AppSettings["NTRegionPointName"].ToString();    //从配置文件获取默认站点
            pointCbxRsm.SetPointValuesFromNames(names);
            if (radlDataType.SelectedValue == "Hour")
            {
                gridAvgStatistical.Visible = false;
                gridAvgStatisticalNew.Visible = true;
            }
            else if (radlDataType.SelectedValue == "Day")
            {
                gridAvgStatistical.Visible = true;
                gridAvgStatisticalNew.Visible = false;
            }
        }
        #endregion

        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {
            try
            {
                string orderByAQI = PageHelper.GetQueryString("orderBy");
                if (dtpBegin.SelectedDate == null || dtpEnd.SelectedDate == null)
                {
                    return;
                }
                else if (dtpBegin.SelectedDate > dtpEnd.SelectedDate)
                {
                    return;
                }
                string rcbType = "";
                foreach (RadComboBoxItem item in rcbCityProper.CheckedItems)
                {
                    rcbType += (item.Text.ToString() + ",");
                }
                string[] qulityType = rcbType.Trim(',').Split(',');
                DateTime dtmBegion = dtpBegin.SelectedDate.Value;
                DateTime dtmEnd = dtpEnd.SelectedDate.Value.AddDays(1).AddSeconds(-1);
                DateTime dtmhourBegin = hourBegin.SelectedDate.Value;
                DateTime dtmhourEnd = hourEnd.SelectedDate.Value;
                points = pointCbxRsm.GetPoints();
                factors = GetPollutantListByCalAQI();//获取参与评价AQI的常规6因子
                int pageSize = gridAvgStatistical.PageSize;  //每页显示数据个数  
                int pageNo = gridAvgStatistical.CurrentPageIndex;   //当前页的序号
                var dataView = new DataView();

                if (rbtnlType.SelectedValue == "Port")
                {
                    if (points != null && points.Count > 0)
                    {
                        if (radlDataType.SelectedValue == "Hour")
                        {

                            dtmBegion = hourBegin.SelectedDate.Value;
                            dtmEnd = hourEnd.SelectedDate.Value;
                            
                            int cks = (int)(dtmEnd - dtmBegion).TotalHours;
                            if (ddlDataFrom.SelectedValue == "OriData")
                            {
                                string dataType = "OriData";
                                //int a = (int)(dtmEnd - dtmBegion).TotalDays;
                                if ((int)(dtmEnd - dtmBegion).TotalHours == 23 && dtmBegion.ToString("yyyy-MM-dd") == dtmEnd.ToString("yyyy-MM-dd"))
                                {
                                    //dataView = m_DayAQIService.GetPointAQIHourInfo(points.Select(t => t.PointID).ToArray(), dtmBegion, dtmEnd.AddHours(1).AddSeconds(-1), qulityType, dataType);
                                    dataView = m_DayAQIService.GetPointAQIHourInfoOver23(points.Select(t => t.PointID).ToArray(), dtmBegion, dtmEnd.AddHours(1).AddSeconds(-1), qulityType, dataType);
                                }
                                    //if ((int)(dtmEnd - dtmBegion).TotalDays >= 1)
                                else 
                                {
                                    //dataView = m_DayAQIService.GetPointAQIHourInfoOver23(points.Select(t => t.PointID).ToArray(), dtmBegion, dtmEnd.AddHours(1).AddSeconds(-1), qulityType, dataType);
                                    dataView = m_DayAQIService.GetPointAQIHourInfo(points.Select(t => t.PointID).ToArray(), dtmBegion, dtmEnd.AddHours(1).AddSeconds(-1), qulityType, dataType);
                                }
                                dataView.Sort = "PointId";
                                hdDSType.Value = "PHourOri";
                            }
                            else
                            {
                                string dataType = "AuditData";
                                if ((int)(dtmEnd - dtmBegion).TotalHours == 23 && dtmBegion.ToString("yyyy-MM-dd") == dtmEnd.ToString("yyyy-MM-dd"))
                                {
                                    //dataView = m_DayAQIService.GetPointAQIHourInfo(points.Select(t => t.PointID).ToArray(), dtmBegion, dtmEnd.AddHours(1).AddSeconds(-1), qulityType, dataType);
                                    dataView = m_DayAQIService.GetPointAQIHourInfoOver23(points.Select(t => t.PointID).ToArray(), dtmBegion, dtmEnd.AddHours(1).AddSeconds(-1), qulityType, dataType);
                                }
                                else
                                {
                                    //dataView = m_DayAQIService.GetPointAQIHourInfoOver23(points.Select(t => t.PointID).ToArray(), dtmBegion, dtmEnd.AddHours(1).AddSeconds(-1), qulityType, dataType);
                                    dataView = m_DayAQIService.GetPointAQIHourInfo(points.Select(t => t.PointID).ToArray(), dtmBegion, dtmEnd.AddHours(1).AddSeconds(-1), qulityType, dataType);
                                }
                                dataView.Sort = "PointId";
                                hdDSType.Value = "PHourAudit";
                            }


                            //将数据存入隐藏域，供绘图使用
                            string[] pointIds = points.Select(t => t.PointID).ToArray();
                            string dtBegion = dtmBegion.ToString("yyyy-MM-dd HH:mm:ss");
                            string dtEnd = dtmEnd.ToString("yyyy-MM-dd HH:mm:ss");
                            hdPointId.Value = string.Join(",", pointIds);
                            hddtBegion.Value = dtBegion;
                            hddtEnd.Value = dtEnd;
                            hdQuality.Value = rcbType;
                            hdChartType.Value = ChartType.SelectedValue;
                            hdChartContent.Value = ChartContent.SelectedValue;
                            //hdDSType.Value="PHourAudit";
                        }
                        else
                        {
                            dtmBegion = dtpBegin.SelectedDate.Value;
                            dtmEnd = dtpEnd.SelectedDate.Value;
                            if (ddlDataFrom.SelectedValue == "OriData")
                            {
                                dataView = m_DayAQIService.GetPointAQIDayInfo(points.Select(t => t.PointID).ToArray(), dtmBegion, dtmEnd, qulityType, "OriData");
                                dataView.Sort = "PointId";
                                hdDSType.Value = "PDayOri";
                            }
                            else
                            {
                                dataView = m_DayAQIService.GetPointAQIDayInfo(points.Select(t => t.PointID).ToArray(), dtmBegion, dtmEnd, qulityType, "AuditData");
                                dataView.Sort = "PointId";
                                hdDSType.Value = "PDayAudit";
                            }


                            //将数据存入隐藏域，供绘图使用
                            string[] pointIds = points.Select(t => t.PointID).ToArray();
                            string dtBegion = dtmBegion.ToString("yyyy-MM-dd HH:mm:ss");
                            string dtEnd = dtmEnd.ToString("yyyy-MM-dd HH:mm:ss");
                            hdPointId.Value = string.Join(",", pointIds);
                            hddtBegion.Value = dtBegion;
                            hddtEnd.Value = dtEnd;
                            hdQuality.Value = rcbType;
                            hdChartType.Value = ChartType.SelectedValue;
                            hdChartContent.Value = ChartContent.SelectedValue;
                            //hdDSType.Value = "PDayAudit";
                        }
                    }
                    else
                    {
                        dataView = null;
                    }
                }
                else if (rbtnlType.SelectedValue == "CityProper")
                {

                    //新建一个新的datatable,存放区域数据信息
                    DataTable dtForAQI = new DataTable();

                    if (points != null && points.Count > 0)
                    {
                        string[] pointIds = points.Select(t => t.PointID).ToArray();
                        dvRegion = GetRegionByPointId(pointIds);

                        foreach (DataRowView dr in dvRegion)
                        {
                            string regionUid = dr["RegionUid"].ToString();
                            listRegionUids.Add(regionUid);
                        }
                        string[] regionUids = listRegionUids.ToArray();

                        if (radlDataType.SelectedValue == "Day")
                        {
                            //给datatable增加列
                            dtForAQI.Columns.Add("PointId", typeof(string));
                            dtForAQI.Columns.Add("PM25", typeof(string));
                            dtForAQI.Columns.Add("PM25_AQI", typeof(string));
                            dtForAQI.Columns.Add("PM10", typeof(string));
                            dtForAQI.Columns.Add("PM10_AQI", typeof(string));
                            dtForAQI.Columns.Add("NO2", typeof(string));
                            dtForAQI.Columns.Add("NO2_AQI", typeof(string));
                            dtForAQI.Columns.Add("SO2", typeof(string));
                            dtForAQI.Columns.Add("SO2_AQI", typeof(string));
                            dtForAQI.Columns.Add("CO", typeof(string));
                            dtForAQI.Columns.Add("CO_AQI", typeof(string));
                            dtForAQI.Columns.Add("O3", typeof(string));
                            dtForAQI.Columns.Add("O3_AQI", typeof(string));
                            dtForAQI.Columns.Add("Max_AQI", typeof(string));
                            dtForAQI.Columns.Add("PrimaryPollutant", typeof(string));
                            dtForAQI.Columns.Add("Grade", typeof(string));
                            dtForAQI.Columns.Add("Class", typeof(string));
                            dtForAQI.Columns.Add("Color", typeof(string));

                            //int dayNum = Convert.ToInt32((dtmEnd.Subtract(dtmBegion)).TotalDays);
                            //for (int i = 0; i < dayNum; i++)
                            //{
                            List<string> regionName = dvRegion.ToTable().AsEnumerable().Select(t => t.Field<string>("Region")).ToList();
                            IEnumerable<string> names = regionName.Distinct();
                            if (ddlDataFrom.SelectedValue == "OriData")
                            {
                                foreach (string name in names)
                                {
                                    List<string> list = new List<string>();
                                    string[] ids = { };
                                    DataRow[] drs = dvRegion.ToTable().Select("Region='" + name + "'").ToArray<DataRow>();
                                    for (int j = 0; j < drs.Length; j++)
                                    {
                                        list.Add(drs[j]["PortId"].ToString());
                                    }
                                    ids = list.ToArray();

                                    //DateTime dayBegin = dtmBegion.AddDays(i);
                                    //DateTime dayEnd = dtmBegion.AddDays(i);
                                    DateTime dayBegin = dtmBegion;
                                    DateTime dayEnd = dtmEnd;
                                    decimal? PM25PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34004", dayBegin, dayEnd, 24, "1");
                                    decimal? PM10PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34002", dayBegin, dayEnd, 24, "1");
                                    decimal? NO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21004", dayBegin, dayEnd, 24, "1");
                                    decimal? SO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21026", dayBegin, dayEnd, 24, "1");
                                    decimal? COPollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21005", dayBegin, dayEnd, 24, "1");
                                    decimal? Max8HourO3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05024", dayBegin, dayEnd, 8, "1");
                                    int? PM25Value = m_AQICalculateService.GetIAQI("a34004", Convert.ToDouble(PM25PollutantValue), 24);
                                    int? PM10Value = m_AQICalculateService.GetIAQI("a34002", Convert.ToDouble(PM10PollutantValue), 24);
                                    int? NO2Value = m_AQICalculateService.GetIAQI("a21004", Convert.ToDouble(NO2PollutantValue), 24);
                                    int? SO2Value = m_AQICalculateService.GetIAQI("a21026", Convert.ToDouble(SO2PollutantValue), 24);
                                    int? COValue = m_AQICalculateService.GetIAQI("a21005", Convert.ToDouble(COPollutantValue), 24);
                                    int? Max8HourO3Value = m_AQICalculateService.GetIAQI("a05024", Convert.ToDouble(Max8HourO3PollutantValue), 8);
                                    string AQIValue = m_AQICalculateService.GetAQI_Max_CNV(SO2Value, NO2Value, PM10Value, COValue, Max8HourO3Value, PM25Value, "V");
                                    string primaryPollutant = m_AQICalculateService.GetAQI_Max_CNV(SO2Value, NO2Value, PM10Value, COValue, Max8HourO3Value, PM25Value, "N");
                                    string grade = string.Empty;
                                    string class_AQI = string.Empty;
                                    string color = string.Empty;
                                    if (AQIValue != null && AQIValue.Trim() != "")
                                    {
                                        grade = m_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Grade");
                                        class_AQI = m_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Class");
                                        color = m_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
                                    }

                                    DataRow dr = dtForAQI.NewRow();
                                    dr["PointId"] = name;
                                    dr["PM25"] = PM25PollutantValue.ToString();
                                    dr["PM25_AQI"] = PM25Value.ToString();
                                    dr["PM10"] = PM10PollutantValue.ToString();
                                    dr["PM10_AQI"] = PM10Value.ToString();
                                    dr["NO2"] = NO2PollutantValue.ToString();
                                    dr["NO2_AQI"] = NO2Value.ToString();
                                    dr["SO2"] = SO2PollutantValue.ToString();
                                    dr["SO2_AQI"] = SO2Value.ToString();
                                    dr["CO"] = COPollutantValue.ToString();
                                    dr["CO_AQI"] = COValue.ToString();
                                    dr["O3"] = Max8HourO3PollutantValue.ToString();
                                    dr["O3_AQI"] = Max8HourO3Value.ToString();
                                    dr["Max_AQI"] = AQIValue;
                                    dr["PrimaryPollutant"] = primaryPollutant;
                                    dr["Grade"] = grade;
                                    dr["Class"] = class_AQI;
                                    dr["Color"] = color;
                                    dtForAQI.Rows.Add(dr);
                                    //将数据存入隐藏域，供绘图使用
                                    string dtBegion = dtmBegion.ToString("yyyy-MM-dd 00:00:00");
                                    string dtEnd = dtmEnd.ToString("yyyy-MM-dd 23:59:59");
                                    hdPointId.Value = string.Join(",", pointIds);
                                    hddtBegion.Value = dtBegion;
                                    hddtEnd.Value = dtEnd;
                                    hdQuality.Value = rcbType;
                                    hdChartType.Value = ChartType.SelectedValue;
                                    hdChartContent.Value = ChartContent.SelectedValue;
                                    hdDSType.Value = "RDayOri";
                                }
                                //}
                                dataView = dtForAQI.AsDataView();
                            }
                            else
                            {

                                foreach (string name in names)
                                {
                                    List<string> list = new List<string>();
                                    string[] ids = { };
                                    DataRow[] drs = dvRegion.ToTable().Select("Region='" + name + "'").ToArray<DataRow>();
                                    for (int j = 0; j < drs.Length; j++)
                                    {
                                        list.Add(drs[j]["PortId"].ToString());
                                    }
                                    ids = list.ToArray();

                                    //DateTime dayBegin = dtmBegion.AddDays(i);
                                    //DateTime dayEnd = dtmBegion.AddDays(i);
                                    DateTime dayBegin = dtmBegion;
                                    DateTime dayEnd = dtmEnd;
                                    decimal? PM25PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34004", dayBegin, dayEnd, 24, "2");
                                    decimal? PM10PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34002", dayBegin, dayEnd, 24, "2");
                                    decimal? NO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21004", dayBegin, dayEnd, 24, "2");
                                    decimal? SO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21026", dayBegin, dayEnd, 24, "2");
                                    decimal? COPollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21005", dayBegin, dayEnd, 24, "2");
                                    decimal? Max8HourO3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05024", dayBegin, dayEnd, 8, "2");
                                    int? PM25Value = m_AQICalculateService.GetIAQI("a34004", Convert.ToDouble(PM25PollutantValue), 24);
                                    int? PM10Value = m_AQICalculateService.GetIAQI("a34002", Convert.ToDouble(PM10PollutantValue), 24);
                                    int? NO2Value = m_AQICalculateService.GetIAQI("a21004", Convert.ToDouble(NO2PollutantValue), 24);
                                    int? SO2Value = m_AQICalculateService.GetIAQI("a21026", Convert.ToDouble(SO2PollutantValue), 24);
                                    int? COValue = m_AQICalculateService.GetIAQI("a21005", Convert.ToDouble(COPollutantValue), 24);
                                    int? Max8HourO3Value = m_AQICalculateService.GetIAQI("a05024", Convert.ToDouble(Max8HourO3PollutantValue), 8);
                                    string AQIValue = m_AQICalculateService.GetAQI_Max_CNV(SO2Value, NO2Value, PM10Value, COValue, Max8HourO3Value, PM25Value, "V");
                                    string primaryPollutant = m_AQICalculateService.GetAQI_Max_CNV(SO2Value, NO2Value, PM10Value, COValue, Max8HourO3Value, PM25Value, "N");
                                    string grade = string.Empty;
                                    string class_AQI = string.Empty;
                                    string color = string.Empty;
                                    if (AQIValue != null && AQIValue.Trim() != "")
                                    {
                                        grade = m_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Grade");
                                        class_AQI = m_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Class");
                                        color = m_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
                                    }

                                    DataRow dr = dtForAQI.NewRow();
                                    dr["PointId"] = name;
                                    dr["PM25"] = PM25PollutantValue.ToString();
                                    dr["PM25_AQI"] = PM25Value.ToString();
                                    dr["PM10"] = PM10PollutantValue.ToString();
                                    dr["PM10_AQI"] = PM10Value.ToString();
                                    dr["NO2"] = NO2PollutantValue.ToString();
                                    dr["NO2_AQI"] = NO2Value.ToString();
                                    dr["SO2"] = SO2PollutantValue.ToString();
                                    dr["SO2_AQI"] = SO2Value.ToString();
                                    dr["CO"] = COPollutantValue.ToString();
                                    dr["CO_AQI"] = COValue.ToString();
                                    dr["O3"] = Max8HourO3PollutantValue.ToString();
                                    dr["O3_AQI"] = Max8HourO3Value.ToString();
                                    dr["Max_AQI"] = AQIValue;
                                    dr["PrimaryPollutant"] = primaryPollutant;
                                    dr["Grade"] = grade;
                                    dr["Class"] = class_AQI;
                                    dr["Color"] = color;
                                    dtForAQI.Rows.Add(dr);
                                    //将数据存入隐藏域，供绘图使用
                                    string dtBegion = dtmBegion.ToString("yyyy-MM-dd 00:00:00");
                                    string dtEnd = dtmEnd.ToString("yyyy-MM-dd 23:59:59");
                                    hdPointId.Value = string.Join(",", pointIds);
                                    hddtBegion.Value = dtBegion;
                                    hddtEnd.Value = dtEnd;
                                    hdQuality.Value = rcbType;
                                    hdChartType.Value = ChartType.SelectedValue;
                                    hdChartContent.Value = ChartContent.SelectedValue;
                                    hdDSType.Value = "RDayAudit";
                                }
                                //}
                                dataView = dtForAQI.AsDataView();
                            }

                        }
                        if (radlDataType.SelectedValue == "Hour")
                        {
                            //给datatable增加列
                            dtForAQI.Columns.Add("PointId", typeof(string));
                            dtForAQI.Columns.Add("PM25", typeof(string));
                            dtForAQI.Columns.Add("PM25_AQI", typeof(string));
                            dtForAQI.Columns.Add("PM10", typeof(string));
                            dtForAQI.Columns.Add("PM10_AQI", typeof(string));
                            dtForAQI.Columns.Add("NO2", typeof(string));
                            dtForAQI.Columns.Add("NO2_AQI", typeof(string));
                            dtForAQI.Columns.Add("SO2", typeof(string));
                            dtForAQI.Columns.Add("SO2_AQI", typeof(string));
                            dtForAQI.Columns.Add("CO", typeof(string));
                            dtForAQI.Columns.Add("CO_AQI", typeof(string));
                            //dtForAQI.Columns.Add("O3", typeof(string));
                            //dtForAQI.Columns.Add("O3_AQI", typeof(string));
                            dtForAQI.Columns.Add("Recent8HoursO3NT", typeof(string));
                            dtForAQI.Columns.Add("Recent8HoursO3NT_AQI", typeof(string));
                            dtForAQI.Columns.Add("Max_AQI", typeof(string));
                            dtForAQI.Columns.Add("PrimaryPollutant", typeof(string));
                            dtForAQI.Columns.Add("Grade", typeof(string));
                            dtForAQI.Columns.Add("Class", typeof(string));
                            dtForAQI.Columns.Add("Color", typeof(string));

                            List<string> regionName = dvRegion.ToTable().AsEnumerable().Select(t => t.Field<string>("Region")).ToList();
                            IEnumerable<string> names = regionName.Distinct();
                            if (ddlDataFrom.SelectedValue == "OriData")
                            {
                                foreach (string name in names)
                                {
                                    List<string> list = new List<string>();
                                    string[] ids = { };
                                    DataRow[] drs = dvRegion.ToTable().Select("Region='" + name + "'").Distinct().ToArray<DataRow>();
                                    for (int j = 0; j < drs.Length; j++)
                                    {
                                        list.Add(drs[j]["PortId"].ToString());
                                    }
                                    ids = list.Distinct().ToArray();

                                    //DateTime hourBegion = dtmhourBegin.AddHours(i);
                                    //DateTime hourEndion = dtmhourEnd.AddHours(i);
                                    DateTime hourBegion = dtmhourBegin;
                                    DateTime hourEndion = Convert.ToDateTime(dtmhourEnd.ToString("yyyy-MM-dd HH:59:59"));
                                    decimal? PM25PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34004", hourBegion, hourEndion, 1, "1");
                                    decimal? PM10PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34002", hourBegion, hourEndion, 1, "1");
                                    decimal? NO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21004", hourBegion, hourEndion, 1, "1");
                                    decimal? SO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21026", hourBegion, hourEndion, 1, "1");
                                    decimal? COPollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21005", hourBegion, hourEndion, 1, "1");
                                    //decimal? O3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05024", hourBegion, hourEndion, 1, "1");
                                    decimal? Max8HourO3PollutantValue = 0;
                                    if ((int)(hourEndion - hourBegion).TotalHours == 23 && hourBegion.ToString("yyyy-MM-dd") == hourEndion.ToString("yyyy-MM-dd"))
                                    {
                                        Max8HourO3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05024", hourBegion, hourEndion, 1, "1");
                                        //Max8HourO3PollutantValue = m_DayAQIService.GetOriDataPagerO3ForNT(ids, hourBegion, hourEndion, "OriData");
                                    }
                                    else
                                    {
                                        //Max8HourO3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05024", hourBegion, hourEndion, 1, "1");
                                        //求最大的可跨天的O3_NT
                                        Max8HourO3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05027", hourBegion, hourEndion, 1, "1");
                                    }
                                    int? PM25Value = m_AQICalculateService.GetIAQI("a34004", Convert.ToDouble(PM25PollutantValue), 24);
                                    int? PM10Value = m_AQICalculateService.GetIAQI("a34002", Convert.ToDouble(PM10PollutantValue), 24);
                                    int? NO2Value = m_AQICalculateService.GetIAQI("a21004", Convert.ToDouble(NO2PollutantValue), 24);
                                    int? SO2Value = m_AQICalculateService.GetIAQI("a21026", Convert.ToDouble(SO2PollutantValue), 24);
                                    int? COValue = m_AQICalculateService.GetIAQI("a21005", Convert.ToDouble(COPollutantValue), 24);
                                    //int? O3Value = m_AQICalculateService.GetIAQI("a05024", Convert.ToDouble(O3PollutantValue), 1);
                                    int? Max8HourO3Value = m_AQICalculateService.GetIAQI("a05024", Convert.ToDouble(Max8HourO3PollutantValue), 8);
                                    string AQIValue = m_AQICalculateService.GetAQI_Max_CNV(SO2Value, NO2Value, PM10Value, COValue, Max8HourO3Value, PM25Value, "V");
                                    string primaryPollutant = m_AQICalculateService.GetAQI_Max_CNV(SO2Value, NO2Value, PM10Value, COValue, Max8HourO3Value, PM25Value, "N");
                                    string grade = string.Empty;
                                    string class_AQI = string.Empty;
                                    string color = string.Empty;
                                    if (AQIValue != null && AQIValue.Trim() != "")
                                    {
                                        grade = m_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Grade");
                                        class_AQI = m_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Class");
                                        color = m_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
                                    }

                                    DataRow dr = dtForAQI.NewRow();
                                    dr["PointId"] = name;
                                    dr["PM25"] = PM25PollutantValue.ToString();
                                    dr["PM25_AQI"] = PM25Value.ToString();
                                    dr["PM10"] = PM10PollutantValue.ToString();
                                    dr["PM10_AQI"] = PM10Value.ToString();
                                    dr["NO2"] = NO2PollutantValue.ToString();
                                    dr["NO2_AQI"] = NO2Value.ToString();
                                    dr["SO2"] = SO2PollutantValue.ToString();
                                    dr["SO2_AQI"] = SO2Value.ToString();
                                    dr["CO"] = COPollutantValue.ToString();
                                    dr["CO_AQI"] = COValue.ToString();
                                    //dr["O3"] = O3PollutantValue.ToString();
                                    //dr["O3_AQI"] = O3Value.ToString();
                                    dr["Recent8HoursO3NT"] = Max8HourO3PollutantValue.ToString();
                                    dr["Recent8HoursO3NT_AQI"] = Max8HourO3Value.ToString();
                                    dr["Max_AQI"] = AQIValue;
                                    dr["PrimaryPollutant"] = primaryPollutant;
                                    dr["Grade"] = grade;
                                    dr["Class"] = class_AQI;
                                    dr["Color"] = color;
                                    dtForAQI.Rows.Add(dr);
                                    //将数据存入隐藏域，供绘图使用
                                    string dtBegion = hourBegion.ToString("yyyy-MM-dd HH:mm:ss");
                                    string dtEnd = hourEndion.ToString("yyyy-MM-dd HH:mm:ss");
                                    hdPointId.Value = string.Join(",", pointIds);
                                    hddtBegion.Value = dtBegion;
                                    hddtEnd.Value = dtEnd;
                                    hdQuality.Value = rcbType;
                                    hdChartType.Value = ChartType.SelectedValue;
                                    hdChartContent.Value = ChartContent.SelectedValue;
                                    hdDSType.Value = "RHourOri";
                                }
                                //}
                                dataView = dtForAQI.AsDataView();
                            }
                            else
                            {
                                foreach (string name in names)
                                {
                                    List<string> list = new List<string>();
                                    string[] ids = { };
                                    DataRow[] drs = dvRegion.ToTable().Select("Region='" + name + "'").ToArray<DataRow>();
                                    for (int j = 0; j < drs.Length; j++)
                                    {
                                        list.Add(drs[j]["PortId"].ToString());
                                    }
                                    ids = list.ToArray();

                                    //DateTime hourBegion = dtmhourBegin.AddHours(i);
                                    //DateTime hourEndion = dtmhourEnd.AddHours(i);
                                    DateTime hourBegion = dtmhourBegin;
                                    DateTime hourEndion = Convert.ToDateTime(dtmhourEnd.ToString("yyyy-MM-dd HH:59:59"));
                                    decimal? PM25PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34004", hourBegion, hourEndion, 1, "2");
                                    decimal? PM10PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34002", hourBegion, hourEndion, 1, "2");
                                    decimal? NO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21004", hourBegion, hourEndion, 1, "2");
                                    decimal? SO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21026", hourBegion, hourEndion, 1, "2");
                                    decimal? COPollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21005", hourBegion, hourEndion, 1, "2");
                                    //decimal? O3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05024", hourBegion, hourEndion, 1, "2");
                                    //decimal? Max8HourO3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05027", hourBegion, hourEndion, 8, "2");
                                    decimal? Max8HourO3PollutantValue = 0;
                                    if ((int)(dtmEnd - dtmBegion).TotalHours == 23 && dtmBegion.ToString("yyyy-MM-dd") == dtmEnd.ToString("yyyy-MM-dd"))
                                    {
                                        Max8HourO3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05024", hourBegion, hourEndion, 1, "2");
                                        ////求最大的可跨天的O3_NT
                                        //Max8HourO3PollutantValue = m_DayAQIService.GetOriDataPagerO3ForNT(ids, hourBegion, hourEndion, "AuditData");
                                    }
                                    else
                                    {
                                        //Max8HourO3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05024", hourBegion, hourEndion, 1, "2");
                                        //求最大的可跨天的O3_NT
                                        Max8HourO3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05027", hourBegion, hourEndion, 1, "2");
                                    }
                                    int? PM25Value = m_AQICalculateService.GetIAQI("a34004", Convert.ToDouble(PM25PollutantValue), 24);
                                    int? PM10Value = m_AQICalculateService.GetIAQI("a34002", Convert.ToDouble(PM10PollutantValue), 24);
                                    int? NO2Value = m_AQICalculateService.GetIAQI("a21004", Convert.ToDouble(NO2PollutantValue), 24);
                                    int? SO2Value = m_AQICalculateService.GetIAQI("a21026", Convert.ToDouble(SO2PollutantValue), 24);
                                    int? COValue = m_AQICalculateService.GetIAQI("a21005", Convert.ToDouble(COPollutantValue), 24);
                                    //int? O3Value = m_AQICalculateService.GetIAQI("a05024", Convert.ToDouble(O3PollutantValue), 1);
                                    int? Max8HourO3Value = m_AQICalculateService.GetIAQI("a05024", Convert.ToDouble(Max8HourO3PollutantValue), 8);
                                    string AQIValue = m_AQICalculateService.GetAQI_Max_CNV(SO2Value, NO2Value, PM10Value, COValue, Max8HourO3Value, PM25Value, "V");
                                    string primaryPollutant = m_AQICalculateService.GetAQI_Max_CNV(SO2Value, NO2Value, PM10Value, COValue, Max8HourO3Value, PM25Value, "N");
                                    string grade = string.Empty;
                                    string class_AQI = string.Empty;
                                    string color = string.Empty;
                                    if (AQIValue != null && AQIValue.Trim() != "")
                                    {
                                        grade = m_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Grade");
                                        class_AQI = m_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Class");
                                        color = m_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
                                    }

                                    DataRow dr = dtForAQI.NewRow();
                                    dr["PointId"] = name;
                                    dr["PM25"] = PM25PollutantValue.ToString();
                                    dr["PM25_AQI"] = PM25Value.ToString();
                                    dr["PM10"] = PM10PollutantValue.ToString();
                                    dr["PM10_AQI"] = PM10Value.ToString();
                                    dr["NO2"] = NO2PollutantValue.ToString();
                                    dr["NO2_AQI"] = NO2Value.ToString();
                                    dr["SO2"] = SO2PollutantValue.ToString();
                                    dr["SO2_AQI"] = SO2Value.ToString();
                                    dr["CO"] = COPollutantValue.ToString();
                                    dr["CO_AQI"] = COValue.ToString();
                                    //dr["O3"] = O3PollutantValue.ToString();
                                    //dr["O3_AQI"] = O3Value.ToString();
                                    dr["Recent8HoursO3NT"] = Max8HourO3PollutantValue.ToString();
                                    dr["Recent8HoursO3NT_AQI"] = Max8HourO3Value.ToString();
                                    dr["Max_AQI"] = AQIValue;
                                    dr["PrimaryPollutant"] = primaryPollutant;
                                    dr["Grade"] = grade;
                                    dr["Class"] = class_AQI;
                                    dr["Color"] = color;
                                    dtForAQI.Rows.Add(dr);
                                    //将数据存入隐藏域，供绘图使用
                                    string dtBegion = hourBegion.ToString("yyyy-MM-dd HH:mm:ss");
                                    string dtEnd = hourEndion.ToString("yyyy-MM-dd HH:mm:ss");
                                    hdPointId.Value = string.Join(",", pointIds);
                                    hddtBegion.Value = dtBegion;
                                    hddtEnd.Value = dtEnd;
                                    hdQuality.Value = rcbType;
                                    hdChartType.Value = ChartType.SelectedValue;
                                    hdChartContent.Value = ChartContent.SelectedValue;
                                    hdDSType.Value = "RHourAudit";
                                }
                                //}
                                dataView = dtForAQI.AsDataView();
                            }

                        }
                    }
                    else
                    {
                        dataView = null;
                    }
                }

                if (dataView == null)
                {
                    gridAvgStatistical.DataSource = new DataTable();
                    gridAvgStatisticalNew.DataSource = new DataTable();
                }
                else
                {
                    if (radlDataType.SelectedValue == "Hour")
                    {
                        gridAvgStatisticalNew.DataSource = dataView;
                        gridAvgStatisticalNew.VirtualItemCount = dataView.Count;
                    }
                    else if (radlDataType.SelectedValue == "Day")
                    {
                        gridAvgStatistical.DataSource = dataView;
                        gridAvgStatistical.VirtualItemCount = dataView.Count;
                    }

                }
                if (radlDataType.SelectedValue == "Hour")
                {
                    gridAvgStatisticalNew.MasterTableView.ColumnGroups[1].HeaderText = "PM<sub>2.5</sub>1小时平均";
                    gridAvgStatisticalNew.MasterTableView.ColumnGroups[2].HeaderText = "PM<sub>10</sub>1小时平均";
                    gridAvgStatisticalNew.MasterTableView.ColumnGroups[3].HeaderText = "二氧化氮(NO<sub>2</sub>)1小时平均";
                    gridAvgStatisticalNew.MasterTableView.ColumnGroups[4].HeaderText = "二氧化硫(SO<sub>2</sub>)1小时平均";
                    gridAvgStatisticalNew.MasterTableView.ColumnGroups[5].HeaderText = "一氧化碳(CO)1小时平均";
                    //gridAvgStatisticalNew.MasterTableView.ColumnGroups[6].HeaderText = "臭氧(O<sub>3</sub>)1小时平均";
                    gridAvgStatisticalNew.MasterTableView.ColumnGroups[6].HeaderText = "臭氧(O<sub>3</sub>)8小时";
                }
                else
                {
                    gridAvgStatistical.MasterTableView.ColumnGroups[1].HeaderText = "PM<sub>2.5</sub>24小时平均值";
                    gridAvgStatistical.MasterTableView.ColumnGroups[2].HeaderText = "PM<sub>10</sub>24小时平均值";
                    gridAvgStatistical.MasterTableView.ColumnGroups[3].HeaderText = "二氧化氮(NO<sub>2</sub>)24小时平均值";
                    gridAvgStatistical.MasterTableView.ColumnGroups[4].HeaderText = "二氧化硫(SO<sub>2</sub>)24小时平均值";
                    gridAvgStatistical.MasterTableView.ColumnGroups[5].HeaderText = "一氧化碳(CO)24小时平均值";
                    gridAvgStatistical.MasterTableView.ColumnGroups[6].HeaderText = "臭氧(O<sub>3</sub>)最大8小时滑动平均值";
                }
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #region 服务器端控件事件处理
        /// <summary>
        /// 绑定数据源（用于分页、排序）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }

        /// <summary>
        /// 数据行绑定处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grid_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem item = e.Item as GridDataItem;
                    DataRowView drv = e.Item.DataItem as DataRowView;
                    if (item["PointId"] != null)
                    {
                        GridTableCell pointCell = (GridTableCell)item["PointId"];
                        if (rbtnlType.SelectedValue == "Port")
                        {
                            IPoint point = points.FirstOrDefault(x => x.PointID.Equals(drv["PointId"].ToString().Trim()));
                            if (point != null)
                                pointCell.Text = point.PointName;
                        }
                        else if (rbtnlType.SelectedValue == "CityProper")
                        {
                            RadComboBox comboBox = null;
                            switch (rbtnlType.SelectedValue)
                            {
                                case "CityProper":
                                    //comboBox = comboCity;
                                    comboBox = null;
                                    break;
                                default: break;
                            }
                            if (comboBox != null)
                            {
                                string regionName = comboBox.Items.Where(t => t.Value == drv["MonitoringRegionUid"].ToString())
                                                    .Select(t => t.Text).FirstOrDefault();
                                pointCell.Text = regionName;
                            }
                        }

                    }
                    if (item["Color"] != null)
                    {
                        GridTableCell cell = item["Color"] as GridTableCell;
                        cell.Style.Add("background-color", cell.Text);
                        cell.Text = string.Empty;
                    }
                    for (int i = 0; i < factors.Count; i++)
                    {
                        string[] uniqueNames;
                        if (radlDataType.SelectedValue == "Hour")
                        {
                            uniqueNames = GetUniqueNameByPollutantNameHour(factors[i].PollutantCode);
                        }
                        else
                        {
                            uniqueNames = GetUniqueNameByPollutantNameDay(factors[i].PollutantCode);
                        }
                        //string[] uniqueNames = GetUniqueNameByPollutantName(factors[i].PollutantCode);
                        foreach (string uniqueName in uniqueNames)
                        {
                            if (drv.DataView.Table.Columns.Contains(uniqueName) && item[uniqueName] != null)
                            {
                                GridTableCell factorCell = (GridTableCell)item[uniqueName];
                                decimal pollutantValue;

                                if (decimal.TryParse(factorCell.Text, out pollutantValue))
                                {
                                    //获取因子小数位,channelCode 因子代码 例：CO的code是a21005  （ channelCode=a21005）
                                    AirPollutantService m_AirPollutantService = new AirPollutantService();
                                    int DecimalNum = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo(factors[i].PollutantCode).PollutantDecimalNum);

                                    //保留小数位数,value 需要进行小数位处理的数据 类型Decimal
                                    if (uniqueName == "CO")
                                    {
                                        factorCell.Text = DecimalExtension.GetPollutantValue(pollutantValue, DecimalNum).ToString();
                                    }
                                    else
                                    {
                                        factorCell.Text = (DecimalExtension.GetPollutantValue(pollutantValue, DecimalNum) * 1000).ToString("G0");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private string[] GetUniqueNameByPollutantNameHour(string pollutantName)
        {
            string[] returnValues = new string[0];
            switch (pollutantName)
            {
                case "a21026":
                    returnValues = new string[] { "SO2" };
                    break;
                case "a21004":
                    returnValues = new string[] { "NO2" };
                    break;
                case "a34002":
                    returnValues = new string[] { "PM10" };
                    break;
                case "a21005":
                    returnValues = new string[] { "CO" };
                    break;
                //case "a05024":
                //    returnValues = new string[] { "O3" };
                //    break;
                case "a34004":
                    returnValues = new string[] { "PM25" };
                    break;
                case "a05027":
                    returnValues = new string[] { "Recent8HoursO3NT" };
                    break;
                default: break;
            }
            return returnValues;
        }
        private string[] GetUniqueNameByPollutantNameDay(string pollutantName)
        {
            string[] returnValues = new string[0];
            switch (pollutantName)
            {
                case "a21026":
                    returnValues = new string[] { "SO2" };
                    break;
                case "a21004":
                    returnValues = new string[] { "NO2" };
                    break;
                case "a34002":
                    returnValues = new string[] { "PM10" };
                    break;
                case "a21005":
                    returnValues = new string[] { "CO" };
                    break;
                case "a05024":
                    returnValues = new string[] { "O3" };
                    break;
                case "a34004":
                    returnValues = new string[] { "PM25" };
                    break;
                case "a05027":
                    returnValues = new string[] { "Recent8HoursO3NT" };
                    break;
                default: break;
            }
            return returnValues;
        }
        /// <summary>
        /// 查询按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            if (radlDataType.SelectedValue == "Day")
            {
                gridAvgStatistical.CurrentPageIndex = 0;
                gridAvgStatistical.Rebind();
                gridAvgStatisticalNew.Visible = false;
                gridAvgStatistical.Visible = true;
            }
            if (radlDataType.SelectedValue == "Hour")
            {
                if (dtms == Convert.ToDateTime(hourEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:00")))
                {
                    hourEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00"));
                }
                gridAvgStatisticalNew.CurrentPageIndex = 0;
                gridAvgStatisticalNew.Rebind();
                gridAvgStatistical.Visible = false;
                gridAvgStatisticalNew.Visible = true;
            }
            if (tabStrip.SelectedTab.Text == "图表")
            {
                BindChart();
            }
            else
            {
                FirstLoadChart.Value = "1";
            }
        }

        /// <summary>
        /// 绑定图表
        /// </summary>
        private void BindChart()
        {
            RegisterScript("CreatChart();");
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
                if (radlDataType.SelectedValue == "Hour")
                {
                    if (hourBegin.SelectedDate == null || hourEnd.SelectedDate == null)
                    {
                        Alert("开始时间或者终止时间，不能为空！");
                        return;
                    }
                    else if (hourBegin.SelectedDate > hourEnd.SelectedDate)
                    {
                        Alert("开始时间不能大于终止时间！");
                        return;
                    }
                }
                else if (radlDataType.SelectedValue == "Day")
                {
                    if (dtpBegin.SelectedDate == null || dtpEnd.SelectedDate == null)
                    {
                        Alert("开始时间或者终止时间，不能为空！");
                        return;
                    }
                    else if (dtpBegin.SelectedDate > dtpEnd.SelectedDate)
                    {
                        Alert("开始时间不能大于终止时间！");
                        return;
                    }
                }
                string rcbType = "";
                foreach (RadComboBoxItem item in rcbCityProper.CheckedItems)
                {
                    rcbType += (item.Text.ToString() + ",");
                }
                string[] qulityType = rcbType.Trim(',').Split(',');
                DateTime dtmBegion = dtpBegin.SelectedDate.Value;
                DateTime dtmEnd = dtpEnd.SelectedDate.Value;
                DateTime dtmhourBegin = hourBegin.SelectedDate.Value;
                DateTime dtmhourEnd = hourEnd.SelectedDate.Value;
                points = pointCbxRsm.GetPoints();
                factors = GetPollutantListByCalAQI();//获取参与评价AQI的常规6因子
                int pageSize = gridAvgStatistical.PageSize;  //每页显示数据个数  
                int pageNo = gridAvgStatistical.CurrentPageIndex;   //当前页的序号
                DataView dataView = null;
                

                if (rbtnlType.SelectedValue == "Port")
                {
                    if (points != null && points.Count > 0)
                    {
                        if (radlDataType.SelectedValue == "Hour")
                        {

                            dtmBegion = hourBegin.SelectedDate.Value;
                            dtmEnd = hourEnd.SelectedDate.Value;

                            if (ddlDataFrom.SelectedValue == "OriData")
                            {
                                if ((int)(dtmEnd - dtmBegion).TotalHours == 23 && dtmBegion.ToString("yyyy-MM-dd") == dtmEnd.ToString("yyyy-MM-dd"))
                                {
                                    //dataView = m_DayAQIService.GetPointAQIHourInfo(points.Select(t => t.PointID).ToArray(), dtmBegion, dtmEnd.AddHours(1).AddSeconds(-1), qulityType, "OriData");
                                    dataView = m_DayAQIService.GetPointAQIHourInfoOver23(points.Select(t => t.PointID).ToArray(), dtmBegion, dtmEnd.AddHours(1).AddSeconds(-1), qulityType, "OriData");
                                }
                                else
                                {
                                    //dataView = m_DayAQIService.GetPointAQIHourInfoOver23(points.Select(t => t.PointID).ToArray(), dtmBegion, dtmEnd.AddHours(1).AddSeconds(-1), qulityType, "OriData");
                                    dataView = m_DayAQIService.GetPointAQIHourInfo(points.Select(t => t.PointID).ToArray(), dtmBegion, dtmEnd.AddHours(1).AddSeconds(-1), qulityType, "OriData");
                                }
                                //dataView = m_DayAQIService.GetPointAQIHourInfo(points.Select(t => t.PointID).ToArray(), dtmBegion, dtmEnd, qulityType, "OriData");
                                dataView.Sort = "PointId";
                                DataTableToExcel(dataView, "点位空气质量小时报", "点位空气质量小时报");
                            }
                            else
                            {
                                if ((int)(dtmEnd - dtmBegion).TotalHours == 23 && dtmBegion.ToString("yyyy-MM-dd") == dtmEnd.ToString("yyyy-MM-dd"))
                                {
                                    //dataView = m_DayAQIService.GetPointAQIHourInfo(points.Select(t => t.PointID).ToArray(), dtmBegion, dtmEnd.AddHours(1).AddSeconds(-1), qulityType, "AuditData");
                                    dataView = m_DayAQIService.GetPointAQIHourInfoOver23(points.Select(t => t.PointID).ToArray(), dtmBegion, dtmEnd.AddHours(1).AddSeconds(-1), qulityType, "AuditData");
                                }
                                else
                                {
                                    //dataView = m_DayAQIService.GetPointAQIHourInfoOver23(points.Select(t => t.PointID).ToArray(), dtmBegion, dtmEnd.AddHours(1).AddSeconds(-1), qulityType, "AuditData");
                                    dataView = m_DayAQIService.GetPointAQIHourInfo(points.Select(t => t.PointID).ToArray(), dtmBegion, dtmEnd.AddHours(1).AddSeconds(-1), qulityType, "AuditData");
                                }
                                //dataView = m_DayAQIService.GetPointAQIHourInfo(points.Select(t => t.PointID).ToArray(), dtmBegion, dtmEnd, qulityType, "AuditData");
                                dataView.Sort = "PointId";
                                DataTableToExcel(dataView, "点位空气质量小时报", "点位空气质量小时报");
                            }
                        }
                        else
                        {
                            dtmBegion = dtpBegin.SelectedDate.Value;
                            dtmEnd = dtpEnd.SelectedDate.Value;
                            if (ddlDataFrom.SelectedValue == "OriData")
                            {
                                string dataType = "OriData";
                                dataView = m_DayAQIService.GetPointAQIDayInfo(points.Select(t => t.PointID).ToArray(), dtmBegion, dtmEnd, qulityType, dataType);
                                dataView.Sort = "PointId";
                                DataTableToExcel(dataView, "点位空气质量日报", "点位空气质量日报");
                            }
                            else
                            {
                                string dataType = "AuditData";
                                dataView = m_DayAQIService.GetPointAQIDayInfo(points.Select(t => t.PointID).ToArray(), dtmBegion, dtmEnd, qulityType, dataType);
                                dataView.Sort = "PointId";
                                DataTableToExcel(dataView, "点位空气质量日报", "点位空气质量日报");
                            }
                        }
                    }
                    else
                    {
                        dataView = null;
                    }
                }
                else if (rbtnlType.SelectedValue == "CityProper")
                {

                    //新建一个新的datatable,存放区域数据信息
                    DataTable dtForAQI = new DataTable();

                    if (points != null && points.Count > 0)
                    {
                        string[] pointIds = points.Select(t => t.PointID).ToArray();
                        dvRegion = GetRegionByPointId(pointIds);

                        foreach (DataRowView dr in dvRegion)
                        {
                            string regionUid = dr["RegionUid"].ToString();
                            listRegionUids.Add(regionUid);
                        }
                        string[] regionUids = listRegionUids.ToArray();

                        if (radlDataType.SelectedValue == "Day")
                        {
                            //给datatable增加列
                            dtForAQI.Columns.Add("PointId", typeof(string));
                            dtForAQI.Columns.Add("PM25", typeof(string));
                            dtForAQI.Columns.Add("PM25_AQI", typeof(string));
                            dtForAQI.Columns.Add("PM10", typeof(string));
                            dtForAQI.Columns.Add("PM10_AQI", typeof(string));
                            dtForAQI.Columns.Add("NO2", typeof(string));
                            dtForAQI.Columns.Add("NO2_AQI", typeof(string));
                            dtForAQI.Columns.Add("SO2", typeof(string));
                            dtForAQI.Columns.Add("SO2_AQI", typeof(string));
                            dtForAQI.Columns.Add("CO", typeof(string));
                            dtForAQI.Columns.Add("CO_AQI", typeof(string));
                            dtForAQI.Columns.Add("O3", typeof(string));
                            dtForAQI.Columns.Add("O3_AQI", typeof(string));
                            dtForAQI.Columns.Add("Max_AQI", typeof(string));
                            dtForAQI.Columns.Add("PrimaryPollutant", typeof(string));
                            dtForAQI.Columns.Add("Grade", typeof(string));
                            dtForAQI.Columns.Add("Class", typeof(string));
                            dtForAQI.Columns.Add("Color", typeof(string));


                            List<string> regionName = dvRegion.ToTable().AsEnumerable().Select(t => t.Field<string>("Region")).ToList();
                            IEnumerable<string> names = regionName.Distinct();
                            if (ddlDataFrom.SelectedValue == "OriData")
                            {
                                foreach (string name in names)
                                {
                                    List<string> list = new List<string>();
                                    string[] ids = { };
                                    DataRow[] drs = dvRegion.ToTable().Select("Region='" + name + "'").ToArray<DataRow>();
                                    for (int j = 0; j < drs.Length; j++)
                                    {
                                        list.Add(drs[j]["PortId"].ToString());
                                    }
                                    ids = list.ToArray();

                                    DateTime dayBegin = dtmBegion;
                                    DateTime dayEnd = dtmEnd;
                                    decimal? PM25PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34004", dayBegin, dayEnd, 24, "1");
                                    decimal? PM10PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34002", dayBegin, dayEnd, 24, "1");
                                    decimal? NO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21004", dayBegin, dayEnd, 24, "1");
                                    decimal? SO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21026", dayBegin, dayEnd, 24, "1");
                                    decimal? COPollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21005", dayBegin, dayEnd, 24, "1");
                                    decimal? Max8HourO3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05024", dayBegin, dayEnd, 8, "1");
                                    int? PM25Value = m_AQICalculateService.GetIAQI("a34004", Convert.ToDouble(PM25PollutantValue), 24);
                                    int? PM10Value = m_AQICalculateService.GetIAQI("a34002", Convert.ToDouble(PM10PollutantValue), 24);
                                    int? NO2Value = m_AQICalculateService.GetIAQI("a21004", Convert.ToDouble(NO2PollutantValue), 24);
                                    int? SO2Value = m_AQICalculateService.GetIAQI("a21026", Convert.ToDouble(SO2PollutantValue), 24);
                                    int? COValue = m_AQICalculateService.GetIAQI("a21005", Convert.ToDouble(COPollutantValue), 24);
                                    int? Max8HourO3Value = m_AQICalculateService.GetIAQI("a05024", Convert.ToDouble(Max8HourO3PollutantValue), 8);
                                    string AQIValue = m_AQICalculateService.GetAQI_Max_CNV(SO2Value, NO2Value, PM10Value, COValue, Max8HourO3Value, PM25Value, "V");
                                    string primaryPollutant = m_AQICalculateService.GetAQI_Max_CNV(SO2Value, NO2Value, PM10Value, COValue, Max8HourO3Value, PM25Value, "N");
                                    string grade = string.Empty;
                                    string class_AQI = string.Empty;
                                    string color = string.Empty;
                                    if (AQIValue != null && AQIValue.Trim() != "")
                                    {
                                        grade = m_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Grade");
                                        class_AQI = m_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Class");
                                        color = m_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
                                    }

                                    DataRow dr = dtForAQI.NewRow();
                                    dr["PointId"] = name;
                                    dr["PM25"] = PM25PollutantValue.ToString();
                                    dr["PM25_AQI"] = PM25Value.ToString();
                                    dr["PM10"] = PM10PollutantValue.ToString();
                                    dr["PM10_AQI"] = PM10Value.ToString();
                                    dr["NO2"] = NO2PollutantValue.ToString();
                                    dr["NO2_AQI"] = NO2Value.ToString();
                                    dr["SO2"] = SO2PollutantValue.ToString();
                                    dr["SO2_AQI"] = SO2Value.ToString();
                                    dr["CO"] = COPollutantValue.ToString();
                                    dr["CO_AQI"] = COValue.ToString();
                                    dr["O3"] = Max8HourO3PollutantValue.ToString();
                                    dr["O3_AQI"] = Max8HourO3Value.ToString();
                                    dr["Max_AQI"] = AQIValue;
                                    dr["PrimaryPollutant"] = primaryPollutant;
                                    dr["Grade"] = grade;
                                    dr["Class"] = class_AQI;
                                    dr["Color"] = color;
                                    dtForAQI.Rows.Add(dr);
                                }
                                dataView = dtForAQI.AsDataView();
                                DataTableToExcel(dataView, "区域空气质量日报", "区域空气质量日报");
                            }
                            else
                            {

                                foreach (string name in names)
                                {
                                    List<string> list = new List<string>();
                                    string[] ids = { };
                                    DataRow[] drs = dvRegion.ToTable().Select("Region='" + name + "'").ToArray<DataRow>();
                                    for (int j = 0; j < drs.Length; j++)
                                    {
                                        list.Add(drs[j]["PortId"].ToString());
                                    }
                                    ids = list.ToArray();

                                    //DateTime dayBegin = dtmBegion.AddDays(i);
                                    //DateTime dayEnd = dtmBegion.AddDays(i);
                                    DateTime dayBegin = dtmBegion;
                                    DateTime dayEnd = dtmEnd;
                                    decimal? PM25PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34004", dayBegin, dayEnd, 24, "2");
                                    decimal? PM10PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34002", dayBegin, dayEnd, 24, "2");
                                    decimal? NO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21004", dayBegin, dayEnd, 24, "2");
                                    decimal? SO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21026", dayBegin, dayEnd, 24, "2");
                                    decimal? COPollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21005", dayBegin, dayEnd, 24, "2");
                                    decimal? Max8HourO3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05024", dayBegin, dayEnd, 8, "2");
                                    int? PM25Value = m_AQICalculateService.GetIAQI("a34004", Convert.ToDouble(PM25PollutantValue), 24);
                                    int? PM10Value = m_AQICalculateService.GetIAQI("a34002", Convert.ToDouble(PM10PollutantValue), 24);
                                    int? NO2Value = m_AQICalculateService.GetIAQI("a21004", Convert.ToDouble(NO2PollutantValue), 24);
                                    int? SO2Value = m_AQICalculateService.GetIAQI("a21026", Convert.ToDouble(SO2PollutantValue), 24);
                                    int? COValue = m_AQICalculateService.GetIAQI("a21005", Convert.ToDouble(COPollutantValue), 24);
                                    int? Max8HourO3Value = m_AQICalculateService.GetIAQI("a05024", Convert.ToDouble(Max8HourO3PollutantValue), 8);
                                    string AQIValue = m_AQICalculateService.GetAQI_Max_CNV(SO2Value, NO2Value, PM10Value, COValue, Max8HourO3Value, PM25Value, "V");
                                    string primaryPollutant = m_AQICalculateService.GetAQI_Max_CNV(SO2Value, NO2Value, PM10Value, COValue, Max8HourO3Value, PM25Value, "N");
                                    string grade = string.Empty;
                                    string class_AQI = string.Empty;
                                    string color = string.Empty;
                                    if (AQIValue != null && AQIValue.Trim() != "")
                                    {
                                        grade = m_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Grade");
                                        class_AQI = m_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Class");
                                        color = m_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
                                    }

                                    DataRow dr = dtForAQI.NewRow();
                                    dr["PointId"] = name;
                                    dr["PM25"] = PM25PollutantValue.ToString();
                                    dr["PM25_AQI"] = PM25Value.ToString();
                                    dr["PM10"] = PM10PollutantValue.ToString();
                                    dr["PM10_AQI"] = PM10Value.ToString();
                                    dr["NO2"] = NO2PollutantValue.ToString();
                                    dr["NO2_AQI"] = NO2Value.ToString();
                                    dr["SO2"] = SO2PollutantValue.ToString();
                                    dr["SO2_AQI"] = SO2Value.ToString();
                                    dr["CO"] = COPollutantValue.ToString();
                                    dr["CO_AQI"] = COValue.ToString();
                                    dr["O3"] = Max8HourO3PollutantValue.ToString();
                                    dr["O3_AQI"] = Max8HourO3Value.ToString();
                                    dr["Max_AQI"] = AQIValue;
                                    dr["PrimaryPollutant"] = primaryPollutant;
                                    dr["Grade"] = grade;
                                    dr["Class"] = class_AQI;
                                    dr["Color"] = color;
                                    dtForAQI.Rows.Add(dr);
                                }
                                dataView = dtForAQI.AsDataView();
                                DataTableToExcel(dataView, "区域空气质量日报", "区域空气质量日报");
                            }

                        }
                        if (radlDataType.SelectedValue == "Hour")
                        {
                            //给datatable增加列
                            dtForAQI.Columns.Add("PointId", typeof(string));
                            dtForAQI.Columns.Add("PM25", typeof(string));
                            dtForAQI.Columns.Add("PM25_AQI", typeof(string));
                            dtForAQI.Columns.Add("PM10", typeof(string));
                            dtForAQI.Columns.Add("PM10_AQI", typeof(string));
                            dtForAQI.Columns.Add("NO2", typeof(string));
                            dtForAQI.Columns.Add("NO2_AQI", typeof(string));
                            dtForAQI.Columns.Add("SO2", typeof(string));
                            dtForAQI.Columns.Add("SO2_AQI", typeof(string));
                            dtForAQI.Columns.Add("CO", typeof(string));
                            dtForAQI.Columns.Add("CO_AQI", typeof(string));
                            //dtForAQI.Columns.Add("O3", typeof(string));
                            //dtForAQI.Columns.Add("O3_AQI", typeof(string));
                            dtForAQI.Columns.Add("Recent8HoursO3NT", typeof(string));
                            dtForAQI.Columns.Add("Recent8HoursO3NT_AQI", typeof(string));
                            dtForAQI.Columns.Add("Max_AQI", typeof(string));
                            dtForAQI.Columns.Add("PrimaryPollutant", typeof(string));
                            dtForAQI.Columns.Add("Grade", typeof(string));
                            dtForAQI.Columns.Add("Class", typeof(string));
                            dtForAQI.Columns.Add("Color", typeof(string));

                            List<string> regionName = dvRegion.ToTable().AsEnumerable().Select(t => t.Field<string>("Region")).ToList();
                            IEnumerable<string> names = regionName.Distinct();
                            if (ddlDataFrom.SelectedValue == "OriData")
                            {
                                foreach (string name in names)
                                {
                                    List<string> list = new List<string>();
                                    string[] ids = { };
                                    DataRow[] drs = dvRegion.ToTable().Select("Region='" + name + "'").ToArray<DataRow>();
                                    for (int j = 0; j < drs.Length; j++)
                                    {
                                        list.Add(drs[j]["PortId"].ToString());
                                    }
                                    ids = list.ToArray();

                                    //DateTime hourBegion = dtmhourBegin.AddHours(i);
                                    //DateTime hourEndion = dtmhourEnd.AddHours(i);
                                    
                                    DateTime hourBegion = dtmhourBegin;
                                    DateTime hourEndion = Convert.ToDateTime(dtmhourEnd.ToString("yyyy-MM-dd HH:59:59"));
                                    decimal? PM25PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34004", hourBegion, hourEndion, 1, "1");
                                    decimal? PM10PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34002", hourBegion, hourEndion, 1, "1");
                                    decimal? NO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21004", hourBegion, hourEndion, 1, "1");
                                    decimal? SO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21026", hourBegion, hourEndion, 1, "1");
                                    decimal? COPollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21005", hourBegion, hourEndion, 1, "1");
                                    //decimal? O3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05024", hourBegion, hourEndion, 1, "1");
                                    decimal? Max8HourO3PollutantValue = 0;
                                    if ((int)(hourEndion - hourBegion).TotalHours == 23 && hourBegion.ToString("yyyy-MM-dd") == hourEndion.ToString("yyyy-MM-dd"))
                                    {
                                        Max8HourO3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05024", hourBegion, hourEndion, 1, "1");
                                        //Max8HourO3PollutantValue = m_DayAQIService.GetOriDataPagerO3ForNT(ids, hourBegion, hourEndion, "OriData");
                                    }
                                    else
                                    {
                                        //Max8HourO3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05024", hourBegion, hourEndion, 1, "1");
                                        //求最大的可跨天的O3_NT
                                        Max8HourO3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05027", hourBegion, hourEndion, 1, "1");
                                    }
                                    int? PM25Value = m_AQICalculateService.GetIAQI("a34004", Convert.ToDouble(PM25PollutantValue), 24);
                                    int? PM10Value = m_AQICalculateService.GetIAQI("a34002", Convert.ToDouble(PM10PollutantValue), 24);
                                    int? NO2Value = m_AQICalculateService.GetIAQI("a21004", Convert.ToDouble(NO2PollutantValue), 24);
                                    int? SO2Value = m_AQICalculateService.GetIAQI("a21026", Convert.ToDouble(SO2PollutantValue), 24);
                                    int? COValue = m_AQICalculateService.GetIAQI("a21005", Convert.ToDouble(COPollutantValue), 24);
                                    //int? O3Value = m_AQICalculateService.GetIAQI("a05024", Convert.ToDouble(O3PollutantValue), 1);
                                    int? Max8HourO3Value = m_AQICalculateService.GetIAQI("a05024", Convert.ToDouble(Max8HourO3PollutantValue), 8);
                                    string AQIValue = m_AQICalculateService.GetAQI_Max_CNV(SO2Value, NO2Value, PM10Value, COValue, Max8HourO3Value, PM25Value, "V");
                                    string primaryPollutant = m_AQICalculateService.GetAQI_Max_CNV(SO2Value, NO2Value, PM10Value, COValue, Max8HourO3Value, PM25Value, "N");
                                    string grade = string.Empty;
                                    string class_AQI = string.Empty;
                                    string color = string.Empty;
                                    if (AQIValue != null && AQIValue.Trim() != "")
                                    {
                                        grade = m_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Grade");
                                        class_AQI = m_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Class");
                                        color = m_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
                                    }

                                    DataRow dr = dtForAQI.NewRow();
                                    dr["PointId"] = name;
                                    dr["PM25"] = PM25PollutantValue.ToString();
                                    dr["PM25_AQI"] = PM25Value.ToString();
                                    dr["PM10"] = PM10PollutantValue.ToString();
                                    dr["PM10_AQI"] = PM10Value.ToString();
                                    dr["NO2"] = NO2PollutantValue.ToString();
                                    dr["NO2_AQI"] = NO2Value.ToString();
                                    dr["SO2"] = SO2PollutantValue.ToString();
                                    dr["SO2_AQI"] = SO2Value.ToString();
                                    dr["CO"] = COPollutantValue.ToString();
                                    dr["CO_AQI"] = COValue.ToString();
                                    //dr["O3"] = O3PollutantValue.ToString();
                                    //dr["O3_AQI"] = O3Value.ToString();
                                    dr["Recent8HoursO3NT"] = Max8HourO3PollutantValue.ToString();
                                    dr["Recent8HoursO3NT_AQI"] = Max8HourO3Value.ToString();
                                    dr["Max_AQI"] = AQIValue;
                                    dr["PrimaryPollutant"] = primaryPollutant;
                                    dr["Grade"] = grade;
                                    dr["Class"] = class_AQI;
                                    dr["Color"] = color;
                                    dtForAQI.Rows.Add(dr);

                                }
                                dataView = dtForAQI.AsDataView();
                                DataTableToExcel(dataView, "区域空气质量小时报", "区域空气质量小时报");
                            }
                            else
                            {
                                foreach (string name in names)
                                {
                                    List<string> list = new List<string>();
                                    string[] ids = { };
                                    DataRow[] drs = dvRegion.ToTable().Select("Region='" + name + "'").ToArray<DataRow>();
                                    for (int j = 0; j < drs.Length; j++)
                                    {
                                        list.Add(drs[j]["PortId"].ToString());
                                    }
                                    ids = list.ToArray();

                                    DateTime hourBegion = dtmhourBegin;
                                    DateTime hourEndion = dtmhourEnd;
                                    decimal? PM25PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34004", hourBegion, hourEndion, 1, "2");
                                    decimal? PM10PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34002", hourBegion, hourEndion, 1, "2");
                                    decimal? NO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21004", hourBegion, hourEndion, 1, "2");
                                    decimal? SO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21026", hourBegion, hourEndion, 1, "2");
                                    decimal? COPollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21005", hourBegion, hourEndion, 1, "2");
                                    //decimal? O3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05024", hourBegion, hourEndion, 1, "2");
                                    decimal? Max8HourO3PollutantValue = 0;
                                    if ((int)(hourEndion - hourBegion).TotalHours == 23 && hourBegion.ToString("yyyy-MM-dd") == hourEndion.ToString("yyyy-MM-dd"))
                                    {
                                        Max8HourO3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05024", hourBegion, hourEndion, 1, "2");
                                        //Max8HourO3PollutantValue = m_DayAQIService.GetOriDataPagerO3ForNT(ids, hourBegion, hourEndion, "OriData");
                                    }
                                    else
                                    {
                                        //Max8HourO3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05024", hourBegion, hourEndion, 1, "1");
                                        //求最大的可跨天的O3_NT
                                        Max8HourO3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05027", hourBegion, hourEndion, 1, "2");
                                    }
                                    int? PM25Value = m_AQICalculateService.GetIAQI("a34004", Convert.ToDouble(PM25PollutantValue), 24);
                                    int? PM10Value = m_AQICalculateService.GetIAQI("a34002", Convert.ToDouble(PM10PollutantValue), 24);
                                    int? NO2Value = m_AQICalculateService.GetIAQI("a21004", Convert.ToDouble(NO2PollutantValue), 1);
                                    int? SO2Value = m_AQICalculateService.GetIAQI("a21026", Convert.ToDouble(SO2PollutantValue), 1);
                                    int? COValue = m_AQICalculateService.GetIAQI("a21005", Convert.ToDouble(COPollutantValue), 1);
                                    int? Max8HourO3Value = m_AQICalculateService.GetIAQI("a05024", Convert.ToDouble(Max8HourO3PollutantValue), 8);
                                    //int? O3Value = m_AQICalculateService.GetIAQI("a05024", Convert.ToDouble(O3PollutantValue), 1);
                                    string AQIValue = m_AQICalculateService.GetAQI_Max_CNV(SO2Value, NO2Value, PM10Value, COValue, Max8HourO3Value, PM25Value, "V");
                                    string primaryPollutant = m_AQICalculateService.GetAQI_Max_CNV(SO2Value, NO2Value, PM10Value, COValue, Max8HourO3Value, PM25Value, "N");
                                    string grade = string.Empty;
                                    string class_AQI = string.Empty;
                                    string color = string.Empty;
                                    if (AQIValue != null && AQIValue.Trim() != "")
                                    {
                                        grade = m_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Grade");
                                        class_AQI = m_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Class");
                                        color = m_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
                                    }

                                    DataRow dr = dtForAQI.NewRow();
                                    dr["PointId"] = name;
                                    dr["PM25"] = PM25PollutantValue.ToString();
                                    dr["PM25_AQI"] = PM25Value.ToString();
                                    dr["PM10"] = PM10PollutantValue.ToString();
                                    dr["PM10_AQI"] = PM10Value.ToString();
                                    dr["NO2"] = NO2PollutantValue.ToString();
                                    dr["NO2_AQI"] = NO2Value.ToString();
                                    dr["SO2"] = SO2PollutantValue.ToString();
                                    dr["SO2_AQI"] = SO2Value.ToString();
                                    dr["CO"] = COPollutantValue.ToString();
                                    dr["CO_AQI"] = COValue.ToString();
                                    //dr["O3"] = O3PollutantValue.ToString();
                                    //dr["O3_AQI"] = O3Value.ToString();
                                    dr["Recent8HoursO3NT"] = Max8HourO3PollutantValue.ToString();
                                    dr["Recent8HoursO3NT_AQI"] = Max8HourO3Value.ToString();
                                    dr["Max_AQI"] = AQIValue;
                                    dr["PrimaryPollutant"] = primaryPollutant;
                                    dr["Grade"] = grade;
                                    dr["Class"] = class_AQI;
                                    dr["Color"] = color;
                                    dtForAQI.Rows.Add(dr);

                                }
                                //}
                                dataView = dtForAQI.AsDataView();
                                DataTableToExcel(dataView, "区域空气质量小时报", "区域空气质量小时报");
                            }

                        }
                    }
                    else
                    {
                        dataView = null;
                    }
                }
            }
        }

        /// <summary>
        /// 导出空气质量实时报
        /// </summary>
        /// <param name="dv">原始数据表</param>
        /// <returns></returns>
        private void DataTableToExcel(DataView dv, string fileName, string sheetName)
        {
            DataTable dtNew = dv.ToTable();
            points = pointCbxRsm.GetPoints();
            Workbook workbook = new Workbook();
            Worksheet sheet = workbook.Worksheets[0];
            Cells cells = sheet.Cells;
            Aspose.Cells.Style cellStyle = workbook.Styles[workbook.Styles.Add()];
            workbook.FileName = fileName;
            sheet.Name = sheetName;
            cellStyle.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center; // 设置居中 
            cellStyle.Font.Name = "宋体"; //文字字体
            cellStyle.Font.Size = 12;//文字大小
            cellStyle.IsLocked = false; //单元格解锁
            cellStyle.IsTextWrapped = true; //单元格内容自动换行

            #region 数据修改
            AirPollutantService m_AirPollutantService = new AirPollutantService();
            factors = GetPollutantListByCalAQI();//获取参与评价AQI的常规6因子
            if (!dtNew.Columns.Contains("PointName"))
            {
                dtNew.Columns.Add("PointName");
            }
            for (int i = 0; i < dtNew.Rows.Count; i++)
            {
                DataRow drNew = dtNew.Rows[i];
                if (rbtnlType.SelectedValue == "Port")
                {
                    drNew["PointName"] = (points.Count(t => t.PointID == drNew["PointId"].ToString()) > 0)
                     ? points.Where(t => t.PointID == drNew["PointId"].ToString()).Select(t => t.PointName).FirstOrDefault()
                     : drNew["PointId"].ToString();
                }
                else if (rbtnlType.SelectedValue == "CityProper")
                {
                    RadComboBox comboBox = null;
                    switch (rbtnlType.SelectedValue)
                    {
                        case "CityProper":
                            //comboBox = comboCity;
                            comboBox = null;
                            break;
                        default: break;
                    }
                    if (comboBox != null)
                    {
                        drNew["PointName"] = (comboBox.Items.Count(t => t.Value == drNew["MonitoringRegionUid"].ToString()) > 0)
                         ? comboBox.Items.Where(t => t.Value == drNew["MonitoringRegionUid"].ToString()).Select(t => t.Text).FirstOrDefault()
                         : drNew["MonitoringRegionUid"].ToString();
                    }
                }
                for (int j = 0; j < factors.Count; j++)
                {
                    string[] uniqueNames = GetUniqueNameByPollutantNameDay(factors[j].PollutantCode);
                    foreach (string uniqueName in uniqueNames)
                    {
                        if (dtNew.Columns.Contains(uniqueName) && !string.IsNullOrWhiteSpace(drNew[uniqueName].ToString()))
                        {
                            //获取因子小数位,channelCode 因子代码 例：CO的code是a21005  （ channelCode=a21005）
                            int DecimalNum = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo(factors[j].PollutantCode).PollutantDecimalNum);
                            decimal pollutantValue = decimal.TryParse(drNew[uniqueName].ToString(), out pollutantValue) ? pollutantValue : 0;

                            //保留小数位数,value 需要进行小数位处理的数据 类型Decimal
                            if (uniqueName == "CO")
                            {
                                drNew[uniqueName] = DecimalExtension.GetPollutantValue(pollutantValue, DecimalNum).ToString();
                            }
                            else
                            {
                                drNew[uniqueName] = (DecimalExtension.GetPollutantValue(pollutantValue, DecimalNum) * 1000).ToString("G0");
                            }
                        }
                    }
                }
            }
            #endregion

            #region 表头
            //第一行
            cells[0, 0].PutValue("监测点位名称");
            cells.Merge(0, 0, 3, 1);
            cells[0, 1].PutValue("污染物浓度及空气质量分指数（IAQI）");
            cells.Merge(0, 1, 1, 12);
            cells[0, 13].PutValue("空气质量指数(AQI)");
            cells.Merge(0, 13, 3, 1);
            cells[0, 14].PutValue("首要污染物");
            cells.Merge(0, 14, 3, 1);
            cells[0, 15].PutValue("空气质量指数级别");
            cells.Merge(0, 15, 3, 1);
            cells[0, 16].PutValue("空气质量指数类别");
            cells.Merge(0, 16, 2, 2);

            if (radlDataType.SelectedValue == "Hour")
            {
                cells[1, 1].PutValue("PM2.5 1小时平均");
                cells.Merge(1, 1, 1, 2);
                cells[1, 3].PutValue("PM10 1小时平均");
                cells.Merge(1, 3, 1, 2);
                cells[1, 5].PutValue("二氧化氮(NO2)1小时平均");
                cells.Merge(1, 5, 1, 2);
                cells[1, 7].PutValue("二氧化硫(SO2)1小时平均");
                cells.Merge(1, 7, 1, 2);
                cells[1, 9].PutValue("一氧化碳(CO)1小时平均");
                cells.Merge(1, 9, 1, 2);
                cells[1, 11].PutValue("臭氧(O3)8小时平均");
                cells.Merge(1, 11, 1, 2);

            }
            else if (radlDataType.SelectedValue == "Day")
            {
                //第二行
                cells[1, 1].PutValue("PM2.5 24小时平均值");
                cells.Merge(1, 1, 1, 2);
                cells[1, 3].PutValue("PM10 24小时平均值");
                cells.Merge(1, 3, 1, 2);
                cells[1, 5].PutValue("二氧化氮(NO2)24小时平均值");
                cells.Merge(1, 5, 1, 2);
                cells[1, 7].PutValue("二氧化硫(SO2)24小时平均值");
                cells.Merge(1, 7, 1, 2);
                cells[1, 9].PutValue("一氧化碳(CO)24小时平均值");
                cells.Merge(1, 9, 1, 2);
                cells[1, 11].PutValue("臭氧(O3)最大8小时滑动平均值");
                cells.Merge(1, 11, 1, 2);

            }


            //第三行
            cells[2, 1].PutValue("浓度/(μg/m3)");
            cells[2, 2].PutValue("分指数");
            cells[2, 3].PutValue("浓度/(μg/m3)");
            cells[2, 4].PutValue("分指数");
            cells[2, 5].PutValue("浓度/(μg/m3)");
            cells[2, 6].PutValue("分指数");
            cells[2, 7].PutValue("浓度/(μg/m3)");
            cells[2, 8].PutValue("分指数");
            cells[2, 9].PutValue("浓度/(mg/m3)");
            cells[2, 10].PutValue("分指数");
            cells[2, 11].PutValue("浓度/(μg/m3)");
            cells[2, 12].PutValue("分指数");
            cells[2, 16].PutValue("类别");
            cells[2, 17].PutValue("颜色");
            cells.SetRowHeight(0, 20);//设置行高
            cells.SetRowHeight(1, 30);//设置行高
            cells.SetColumnWidth(0, 20);//设置列宽
            cells.SetColumnWidth(16, 10);//设置列宽
            cells.SetColumnWidth(17, 10);//设置列宽
            for (int i = 1; i <= 14; i++)
            {
                cells.SetColumnWidth(i, 10);//设置列宽
            }
            #endregion

            for (int i = 0; i < dtNew.Rows.Count; i++)
            {
                DataRow drNew = dtNew.Rows[i];
                int rowIndex = i + 3;
                if (rbtnlType.SelectedValue == "Port")
                {
                    cells[rowIndex, 0].PutValue(drNew["PointName"].ToString());
                }
                else if (rbtnlType.SelectedValue == "CityProper")
                {
                    cells[rowIndex, 0].PutValue(drNew["PointId"].ToString());
                }
                //cells[rowIndex, 0].PutValue(drNew["PointName"].ToString());
                //if (drNew["PM25"].IsNullOrDBNull())
                //{
                //    drNew["PM25"] = "";
                //}
                //if (drNew["PM25_AQI"].IsNullOrDBNull())
                //{
                //    drNew["PM25_AQI"] = "";
                //}
                //if (drNew["PM10"].IsNullOrDBNull())
                //{
                //    drNew["PM10"] = "";
                //}
                //if (drNew["PM10_AQI"].IsNullOrDBNull())
                //{
                //    drNew["PM10_AQI"] = "";
                //}
                //if (drNew["NO2"].IsNullOrDBNull())
                //{
                //    drNew["NO2"] = "";
                //}
                //if (drNew["NO2_AQI"].IsNullOrDBNull())
                //{
                //    drNew["NO2_AQI"] = "";
                //}
                //if (drNew["SO2"].IsNullOrDBNull())
                //{
                //    drNew["SO2"] = "";
                //}
                //if (drNew["SO2_AQI"].IsNullOrDBNull())
                //{
                //    drNew["SO2_AQI"] = "";
                //}
                //if (drNew["CO"].IsNullOrDBNull())
                //{
                //    drNew["CO"] = "";
                //}
                //if (drNew["CO_AQI"].IsNullOrDBNull())
                //{
                //    drNew["CO_AQI"] = "";
                //}
                if (drNew["PM25"].IsNullOrDBNull() && drNew["PM25"].ToString() == "")
                {
                    cells[rowIndex, 1].PutValue("");
                }
                else
                {
                    cells[rowIndex, 1].PutValue(Convert.ToDecimal(drNew["PM25"]));
                }
                if (drNew["PM25_AQI"].IsNullOrDBNull() && drNew["PM25_AQI"].ToString() == "")
                {
                    cells[rowIndex, 2].PutValue("");
                }
                else
                {
                    cells[rowIndex, 2].PutValue(Convert.ToDecimal(drNew["PM25_AQI"]));
                }
                if (drNew["PM10"].IsNullOrDBNull() && drNew["PM10"].ToString() == "")
                {
                    cells[rowIndex, 3].PutValue("");
                }
                else
                {
                    cells[rowIndex, 3].PutValue(Convert.ToDecimal(drNew["PM10"]));
                }
                if (drNew["PM10_AQI"].IsNullOrDBNull() && drNew["PM10_AQI"].ToString() == "")
                {
                    cells[rowIndex, 4].PutValue("");
                }
                else
                {
                    cells[rowIndex, 4].PutValue(Convert.ToDecimal(drNew["PM10_AQI"]));
                }
                if (drNew["NO2"].IsNullOrDBNull() && drNew["NO2"].ToString() == "")
                {
                    cells[rowIndex, 5].PutValue("");
                }
                else
                {
                    cells[rowIndex, 5].PutValue(Convert.ToDecimal(drNew["NO2"]));
                }
                if (drNew["NO2_AQI"].IsNullOrDBNull() && drNew["NO2_AQI"].ToString() == "")
                {
                    cells[rowIndex, 6].PutValue("");
                }
                else
                {
                    cells[rowIndex, 6].PutValue(Convert.ToDecimal(drNew["NO2_AQI"]));
                }
                if (drNew["SO2"].IsNullOrDBNull() && drNew["SO2"].ToString() == "")
                {
                    cells[rowIndex, 7].PutValue("");
                }
                else
                {
                    cells[rowIndex, 7].PutValue(Convert.ToDecimal(drNew["SO2"]));
                }
                if (drNew["SO2_AQI"].IsNullOrDBNull() && drNew["SO2_AQI"].ToString() == "")
                {
                    cells[rowIndex, 8].PutValue("");
                }
                else
                {
                    cells[rowIndex, 8].PutValue(Convert.ToDecimal(drNew["SO2_AQI"]));
                }
                if (drNew["CO"].IsNullOrDBNull() && drNew["CO"].ToString() == "")
                {
                    cells[rowIndex, 9].PutValue("");
                }
                else
                {
                    cells[rowIndex, 9].PutValue(Convert.ToDecimal(drNew["CO"]));
                }
                if (drNew["CO_AQI"].IsNullOrDBNull() && drNew["CO_AQI"].ToString() == "")
                {
                    cells[rowIndex, 10].PutValue("");
                }
                else
                {
                    cells[rowIndex, 10].PutValue(Convert.ToDecimal(drNew["CO_AQI"]));
                }
                //cells[rowIndex, 1].PutValue(drNew["PM25"]);
                //cells[rowIndex, 2].PutValue(drNew["PM25_AQI"]);
                //cells[rowIndex, 3].PutValue(drNew["PM10"]);
                //cells[rowIndex, 4].PutValue(drNew["PM10_AQI"]);
                //cells[rowIndex, 5].PutValue(drNew["NO2"]);
                //cells[rowIndex, 6].PutValue(drNew["NO2_AQI"]);
                //cells[rowIndex, 7].PutValue(drNew["SO2"]);
                //cells[rowIndex, 8].PutValue(drNew["SO2_AQI"]);
                //cells[rowIndex, 9].PutValue(drNew["CO"]);
                //cells[rowIndex, 10].PutValue(drNew["CO_AQI"]);
                if (rbtnlType.SelectedValue == "CityProper" && radlDataType.SelectedValue == "Day")
                {
                    if (drNew["O3"].IsNullOrDBNull() && drNew["O3"].ToString() == "")
                    {
                        cells[rowIndex, 11].PutValue("");
                    }
                    else
                    {
                        cells[rowIndex, 11].PutValue(Convert.ToDecimal(drNew["O3"]));
                    }
                    if (drNew["O3_AQI"].IsNullOrDBNull() && drNew["O3_AQI"].ToString() == "")
                    {
                        cells[rowIndex, 12].PutValue("");
                    }
                    else
                    {
                        cells[rowIndex, 12].PutValue(Convert.ToDecimal(drNew["O3_AQI"]));
                    }
                    //cells[rowIndex, 11].PutValue(drNew["O3"]);
                    //cells[rowIndex, 12].PutValue(drNew["O3_AQI"]);
                }
                else if (rbtnlType.SelectedValue == "Port" && radlDataType.SelectedValue == "Hour")
                {
                    if (drNew["Recent8HoursO3NT"].IsNullOrDBNull() && drNew["Recent8HoursO3NT"].ToString() == "")
                    {
                        cells[rowIndex, 11].PutValue("");
                    }
                    else
                    {
                        cells[rowIndex, 11].PutValue(Convert.ToDecimal(drNew["Recent8HoursO3NT"]));
                    }
                    if (drNew["Recent8HoursO3NT_AQI"].IsNullOrDBNull() && drNew["Recent8HoursO3NT_AQI"].ToString() == "")
                    {
                        cells[rowIndex, 12].PutValue("");
                    }
                    else
                    {
                        cells[rowIndex, 12].PutValue(Convert.ToDecimal(drNew["Recent8HoursO3NT_AQI"]));
                    }
                    //cells[rowIndex, 11].PutValue(drNew["Recent8HoursO3NT"]);
                    //cells[rowIndex, 12].PutValue(drNew["Recent8HoursO3NT_AQI"]);
                }
                else if (rbtnlType.SelectedValue == "Port" && radlDataType.SelectedValue == "Day")
                {
                    if (drNew["O3"].IsNullOrDBNull() && drNew["O3"].ToString() == "")
                    {
                        cells[rowIndex, 11].PutValue("");
                    }
                    else
                    {
                        cells[rowIndex, 11].PutValue(Convert.ToDecimal(drNew["O3"]));
                    }
                    if (drNew["O3_AQI"].IsNullOrDBNull() && drNew["O3_AQI"].ToString() == "")
                    {
                        cells[rowIndex, 12].PutValue("");
                    }
                    else
                    {
                        cells[rowIndex, 12].PutValue(Convert.ToDecimal(drNew["O3_AQI"]));
                    }
                    //cells[rowIndex, 11].PutValue(drNew["O3"]);
                    //cells[rowIndex, 12].PutValue(drNew["O3_AQI"]);
                }
                else if (rbtnlType.SelectedValue == "CityProper" && radlDataType.SelectedValue == "Hour")
                {
                    if (drNew["Recent8HoursO3NT"].IsNullOrDBNull() && drNew["Recent8HoursO3NT"].ToString() == "")
                    {
                        cells[rowIndex, 11].PutValue("");
                    }
                    else
                    {
                        cells[rowIndex, 11].PutValue(Convert.ToDecimal(drNew["Recent8HoursO3NT"]));
                    }
                    if (drNew["Recent8HoursO3NT_AQI"].IsNullOrDBNull() && drNew["Recent8HoursO3NT_AQI"].ToString() == "")
                    {
                        cells[rowIndex, 12].PutValue("");
                    }
                    else
                    {
                        cells[rowIndex, 12].PutValue(Convert.ToDecimal(drNew["Recent8HoursO3NT_AQI"]));
                    }
                    //cells[rowIndex, 11].PutValue(drNew["Recent8HoursO3NT"]);
                    //cells[rowIndex, 12].PutValue(drNew["Recent8HoursO3NT_AQI"]);
                }
                //cells[rowIndex, 11].PutValue(drNew["O3"].ToString());
                //cells[rowIndex, 12].PutValue(drNew["O3_AQI"].ToString());
                cells[rowIndex, 13].PutValue(drNew["Max_AQI"].ToString());
                cells[rowIndex, 14].PutValue(drNew["PrimaryPollutant"].ToString());
                cells[rowIndex, 15].PutValue(drNew["Grade"].ToString());
                cells[rowIndex, 16].PutValue(drNew["Class"].ToString());
                cells[rowIndex, 17].PutValue("");
                if (rbtnlType.SelectedValue == "Port")
                {
                    if (drNew["Color"].ToString() != "")
                    {
                        Aspose.Cells.Style styleTemp = cells[rowIndex, 17].GetStyle();
                        styleTemp.ForegroundColor = System.Drawing.ColorTranslator.FromHtml(drNew["Color"].ToString());//设置背景色
                        styleTemp.Pattern = BackgroundType.Solid;//设置背景样式
                        cells[rowIndex, 17].SetStyle(styleTemp);
                    }
                }
                else if (rbtnlType.SelectedValue == "CityProper")
                {
                    if (drNew["Color"].ToString() != "")
                    {
                        Aspose.Cells.Style styleTemp = cells[rowIndex, 17].GetStyle();
                        styleTemp.ForegroundColor = System.Drawing.ColorTranslator.FromHtml(drNew["Color"].ToString());//设置背景色
                        styleTemp.Pattern = BackgroundType.Solid;//设置背景样式
                        cells[rowIndex, 17].SetStyle(styleTemp);
                    }
                }
                //if (drNew["Color"].ToString() != "")
                //{
                //    Aspose.Cells.Style styleTemp = cells[rowIndex, 17].GetStyle();
                //    styleTemp.ForegroundColor = System.Drawing.ColorTranslator.FromHtml(drNew["Color"].ToString());//设置背景色
                //    styleTemp.Pattern = BackgroundType.Solid;//设置背景样式
                //    cells[rowIndex, 17].SetStyle(styleTemp);
                //}
            }
            foreach (Cell cell in cells)
            {
                if (!cell.IsStyleSet)
                {
                    cell.SetStyle(cellStyle);
                }
            }

            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "utf-8";
            //Response.AppendHeader("Content-Disposition", string.Format("attachment;filename={0}.xls", HttpUtility.UrlEncode(fileName) + DateTime.Now.ToString("yyyyMMddHHmmss")));
            Response.AppendHeader("Content-Disposition", string.Format("attachment;filename={0}.xls", HttpUtility.UrlEncode(fileName)));
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.ContentType = "application/ms-excel";
            Response.BinaryWrite(workbook.SaveToStream().ToArray());
            Response.End();
        }

        /// <summary>
        /// 查询类型切换事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rbtnlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //pointCbxRsm.Visible = false;
            dvPoint.Style["display"] = "none";
            //comboCity.Visible = false;
            switch (rbtnlType.SelectedValue)
            {
                case "CityProper":
                    //comboCity.Visible = true;
                    dvPoint.Style["display"] = "normal";
                    break;
                case "Port":
                    //pointCbxRsm.Visible = true;
                    dvPoint.Style["display"] = "normal";
                    break;
            }
        }
        #endregion

        /// <summary>
        /// 根据测点Id数组获取因子列
        /// </summary>
        /// <param name="pointIds">测点Id数组</param>
        /// <returns></returns>
        private IList<PollutantCodeEntity> GetPollutantCodesByPointIds(string[] pointIds)
        {
            MonitoringPointAirService monitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            IQueryable<MonitoringPointEntity> monitorPointQueryable = monitoringPointAir.RetrieveListByPointIds(pointIds);//根据站点ID数组获取站点
            IList<PollutantCodeEntity> pollutantList = new List<PollutantCodeEntity>();
            InstrumentChannelService instrumentChannelService = Singleton<InstrumentChannelService>.GetInstance();//提供仪器通道信息服务
            foreach (MonitoringPointEntity monitoringPointEntity in monitorPointQueryable)
            {
                IQueryable<PollutantCodeEntity> pollutantCodeQueryable =
                    instrumentChannelService.RetrieveChannelListByPointUid(monitoringPointEntity.MonitoringPointUid);//根据站点Uid获取所有监测通道
                pollutantList = pollutantList.Union(pollutantCodeQueryable).ToList();
            }
            return pollutantList;
        }

        /// <summary>
        /// 获取参与评价AQI的常规6因子
        /// </summary>
        /// <returns></returns>
        private IList<PollutantCodeEntity> GetPollutantListByCalAQI()
        {
            AirPollutantService airPollutantService = new AirPollutantService();
            return airPollutantService.RetrieveListByCalAQI().ToList();
        }

        protected void rcbCityProper_ItemCreated(object sender, RadComboBoxItemEventArgs e)
        {
            e.Item.Checked = true;
        }

        protected void radlDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            gridAvgStatistical.CurrentPageIndex = 0;
            //小时均值
            if (radlDataType.SelectedValue == "Hour")
            {
                tbHour.Visible = true;
                tbDay.Visible = false;
                ddlDataFrom.Visible = true;
            }
            //日均值
            else if (radlDataType.SelectedValue == "Day")
            {
                tbDay.Visible = true;
                tbHour.Visible = false;
                ddlDataFrom.Visible = true;
            }
        }
        /// <summary>
        /// 根据所选站点ID获取相应区域信息
        /// </summary>
        /// <param name="pointIds"></param>
        /// <returns></returns>
        public DataView GetRegionByPointId(string[] pointIds)
        {
            return pointAirService.GetRegionByPointId(pointIds);
        }

        protected void ChartContent_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            string Pic = ChartContent.SelectedValue;
            hdChartContent.Value = Pic;
            RegisterScript("PointFactor('" + Pic + "');");
        }
    }
}