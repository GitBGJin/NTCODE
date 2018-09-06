﻿using SmartEP.Core.Generic;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.DataAnalyze.Air;
using SmartEP.Service.DataAnalyze.Air.AQIReport;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Utilities.Web.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartEP.WebUI.Pages.EnvAir.Chart
{
    /// <summary>
    /// 名称：AirQualittyAvgStatisticalChart.aspx
    /// 创建人：刘晋
    /// 创建日期：2017-06-05
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 各市区小时数据页面图形数据
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class AirQualittyAvgStatisticalChart : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 空气站点信息服务
        /// </summary>
        private MonitoringPointAirService pointAirService;
        /// <summary>
        /// 区域Uid集合
        /// </summary>
        List<string> listRegionUids = new List<string>();
        /// <summary>
        /// 区域信息
        /// </summary>
        DataView dvRegion = new DataView();

        private DayAQIService m_DayAQIService;
        private AQICalculateService m_AQICalculateService;
        protected void Page_Load(object sender, EventArgs e)
        {
            m_DayAQIService = new DayAQIService();

            m_AQICalculateService = new AQICalculateService();
            pointAirService = new MonitoringPointAirService();
            if (!IsPostBack)
            {
                bind();
            }
        }

        public void bind()
        {
            string pointIdse = PageHelper.GetQueryString("pointIds");
            string quality = PageHelper.GetQueryString("quality");
            string chartType = PageHelper.GetQueryString("chartType");
            string chartContent = PageHelper.GetQueryString("chartContent");
            string dsType = PageHelper.GetQueryString("dsType");
            DateTime dtStart = DateTime.TryParse(PageHelper.GetQueryString("dtBegion"), out dtStart) ? dtStart : DateTime.Now.AddMonths(-1);
            DateTime dtEnd = DateTime.TryParse(PageHelper.GetQueryString("dtEnd"), out dtEnd) ? dtEnd : DateTime.Now;

            string[] pointIds = pointIdse.Split(',');
            string[] qty = quality.Trim(',').Split(',');
            int recordTotal = 0;
            int pageSize = 999999;
            int pageNo = 0;
            string orderBy = "PointId1,DateTime1 Desc";
            var dataView = new DataView();
            //新建一个新的datatable,存放区域数据信息
            DataTable dtForAQI = new DataTable();
            hdxData.Value = chartType;
            #region //六参数浓度值
            if (chartContent == "factorValue")
            {

                hdjsonTitle.Value = "六参数浓度值(mg/m3)";
                #region 站点小时审核六参数浓度值
                if (dsType == "PHourAudit")
                {
                    MonitoringPointAirService monitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
                    hdxData.Value = chartType;
                    if ((int)(dtEnd - dtStart).TotalHours == 23 && dtStart.ToString("yyyy-MM-dd") == dtEnd.ToString("yyyy-MM-dd"))
                    {
                        //dataView = m_DayAQIService.GetPointAQIHourInfo(pointIds, dtStart, dtEnd.AddHours(1).AddSeconds(-1), qty, "Audit");
                        dataView = m_DayAQIService.GetPointAQIHourInfoOver23(pointIds.ToArray(), dtStart, dtEnd.AddHours(1).AddSeconds(-1), qty, "Audit");
                    }
                    else
                    {
                        //dataView = m_DayAQIService.GetPointAQIHourInfoOver23(pointIds.ToArray(), dtStart, dtEnd.AddHours(1).AddSeconds(-1), qty, "Audit");
                        dataView = m_DayAQIService.GetPointAQIHourInfo(pointIds, dtStart, dtEnd.AddHours(1).AddSeconds(-1), qty, "Audit");
                    }
                    string data = "[";
                    for (int j = 0; j < pointIds.Length; j++)
                    {
                        string pointId = pointIds[j];
                        MonitoringPointEntity monitorPoint = monitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(pointId));
                        dataView.RowFilter = "PointId='" + pointIds[j] + "'";
                        data += "{";
                        data += string.Format(" name: '{0}',data:[ ", monitorPoint.MonitoringPointName);
                        int m = 0;
                        foreach (DataRowView drv in dataView)
                        {
                            m++;
                            //DateTime tstamp = Convert.ToDateTime(drv["DateTime1"]);
                            //string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                            if (m != dataView.Count)
                            {
                                if (drv["PM25"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM25"].ToString() + "],";
                                }
                                if (drv["PM10"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM10"].ToString() + "],";
                                }
                                if (drv["NO2"] != DBNull.Value)
                                {
                                    data += "[" + drv["NO2"].ToString() + "],";
                                }
                                if (drv["SO2"] != DBNull.Value)
                                {
                                    data += "[" + drv["SO2"].ToString() + "],";
                                }
                                if (drv["CO"] != DBNull.Value)
                                {
                                    data += "[" + drv["CO"].ToString() + "],";
                                }
                                if (drv["Recent8HoursO3NT"] != DBNull.Value)
                                {
                                    data += "[" + drv["Recent8HoursO3NT"].ToString() + "],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }
                            else
                            {
                                if (drv["PM25"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM25"].ToString() + "],";
                                }
                                if (drv["PM10"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM10"].ToString() + "],";
                                }
                                if (drv["NO2"] != DBNull.Value)
                                {
                                    data += "[" + drv["NO2"].ToString() + "],";
                                }
                                if (drv["SO2"] != DBNull.Value)
                                {
                                    data += "[" + drv["SO2"].ToString() + "],";
                                }
                                if (drv["CO"] != DBNull.Value)
                                {
                                    data += "[" + drv["CO"].ToString() + "],";
                                }
                                if (drv["Recent8HoursO3NT"] != DBNull.Value)
                                {
                                    data += "[" + drv["Recent8HoursO3NT"].ToString() + "],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }

                        }
                        data += "]},";
                    }
                    if (data != "")
                    {
                        data = data.Substring(0, data.Length - 1);
                    }
                    data += "]";
                    hdjsonData.Value = data;
                }
                #endregion
                #region 站点日审核六参数浓度值
                if (dsType == "PDayAudit")
                {
                    MonitoringPointAirService monitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
                    hdxData.Value = chartType;
                    dataView = m_DayAQIService.GetPointAQIDayInfo(pointIds, dtStart, dtEnd, qty, "AuditData");
                    string data = "[";
                    for (int j = 0; j < pointIds.Length; j++)
                    {
                        string pointId = pointIds[j];
                        MonitoringPointEntity monitorPoint = monitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(pointId));
                        dataView.RowFilter = "PointId='" + pointIds[j] + "'";
                        data += "{";
                        data += string.Format(" name: '{0}',data:[ ", monitorPoint.MonitoringPointName);
                        int m = 0;
                        foreach (DataRowView drv in dataView)
                        {
                            m++;
                            //DateTime tstamp = Convert.ToDateTime(drv["DateTime1"]);
                            //string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                            if (m != dataView.Count)
                            {
                                if (drv["PM25"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM25"].ToString() + "],";
                                }
                                if (drv["PM10"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM10"].ToString() + "],";
                                }
                                if (drv["NO2"] != DBNull.Value)
                                {
                                    data += "[" + drv["NO2"].ToString() + "],";
                                }
                                if (drv["SO2"] != DBNull.Value)
                                {
                                    data += "[" + drv["SO2"].ToString() + "],";
                                }
                                if (drv["CO"] != DBNull.Value)
                                {
                                    data += "[" + drv["CO"].ToString() + "],";
                                }
                                if (drv["O3"] != DBNull.Value)
                                {
                                    data += "[" + drv["O3"].ToString() + "],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }
                            else
                            {
                                if (drv["PM25"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM25"].ToString() + "],";
                                }
                                if (drv["PM10"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM10"].ToString() + "],";
                                }
                                if (drv["NO2"] != DBNull.Value)
                                {
                                    data += "[" + drv["NO2"].ToString() + "],";
                                }
                                if (drv["SO2"] != DBNull.Value)
                                {
                                    data += "[" + drv["SO2"].ToString() + "],";
                                }
                                if (drv["CO"] != DBNull.Value)
                                {
                                    data += "[" + drv["CO"].ToString() + "],";
                                }
                                if (drv["O3"] != DBNull.Value)
                                {
                                    data += "[" + drv["O3"].ToString() + "],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }

                        }
                        data += "]},";
                    }
                    if (data != "")
                    {
                        data = data.Substring(0, data.Length - 1);
                    }
                    data += "]";
                    hdjsonData.Value = data;
                }
                #endregion
                #region 区域小时审核六参数浓度值
                if (dsType == "RHourAudit")
                {
                    MonitoringPointAirService monitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();

                    dvRegion = GetRegionByPointId(pointIds);
                    foreach (DataRowView dr in dvRegion)
                    {
                        string regionUid = dr["RegionUid"].ToString();
                        listRegionUids.Add(regionUid);
                    }
                    string[] regionUids = listRegionUids.ToArray();
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
                    dtForAQI.Columns.Add("AQIValue", typeof(string));
                    dtForAQI.Columns.Add("PrimaryPollutant", typeof(string));
                    dtForAQI.Columns.Add("Grade", typeof(string));
                    dtForAQI.Columns.Add("Class", typeof(string));
                    dtForAQI.Columns.Add("RGBValue", typeof(string));

                    //int hourNum = Convert.ToInt32((dtmhourEnd.Subtract(dtmhourBegin)).TotalHours);
                    //for (int i = 0; i < hourNum; i++)
                    //{

                    List<string> regionName = dvRegion.ToTable().AsEnumerable().Select(t => t.Field<string>("Region")).ToList();
                    IEnumerable<string> names = regionName.Distinct();
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

                        DateTime hourBegion = dtStart;
                        DateTime hourEndion = dtEnd;
                        decimal? PM25PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34004", hourBegion, hourEndion, 1, "2");
                        decimal? PM10PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34002", hourBegion, hourEndion, 1, "2");
                        decimal? NO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21004", hourBegion, hourEndion, 1, "2");
                        decimal? SO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21026", hourBegion, hourEndion, 1, "2");
                        decimal? COPollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21005", hourBegion, hourEndion, 1, "2");
                        //decimal? O3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05024", hourBegion, hourEndion, 1, "2");
                        decimal? O3PollutantValue = 0;
                        if ((int)(hourEndion - hourBegion).TotalHours == 23 && hourBegion.ToString("yyyy-MM-dd") == hourEndion.ToString("yyyy-MM-dd"))
                        {
                            //求最大的可跨天的O3_NT
                            O3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05024", hourBegion, hourEndion, 1, "2");
                        }
                        else
                        {
                            O3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05027", hourBegion, hourEndion, 1, "2");
                        }
                        int? PM25Value = m_AQICalculateService.GetIAQI("a34004", Convert.ToDouble(PM25PollutantValue), 24);
                        int? PM10Value = m_AQICalculateService.GetIAQI("a34002", Convert.ToDouble(PM10PollutantValue), 24);
                        int? NO2Value = m_AQICalculateService.GetIAQI("a21004", Convert.ToDouble(NO2PollutantValue), 1);
                        int? SO2Value = m_AQICalculateService.GetIAQI("a21026", Convert.ToDouble(SO2PollutantValue), 1);
                        int? COValue = m_AQICalculateService.GetIAQI("a21005", Convert.ToDouble(COPollutantValue), 1);
                        int? O3Value = m_AQICalculateService.GetIAQI("a05024", Convert.ToDouble(O3PollutantValue), 1);
                        string AQIValue = m_AQICalculateService.GetAQI_Max_CNV(PM25Value, PM10Value, NO2Value, SO2Value, COValue, O3Value, "V");
                        string primaryPollutant = m_AQICalculateService.GetAQI_Max_CNV(PM25Value, PM10Value, NO2Value, SO2Value, COValue, O3Value, "N");
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
                        dr["O3"] = O3PollutantValue.ToString();
                        dr["O3_AQI"] = O3Value.ToString();
                        dr["AQIValue"] = AQIValue;
                        dr["PrimaryPollutant"] = primaryPollutant;
                        dr["Grade"] = grade;
                        dr["Class"] = class_AQI;
                        dr["RGBValue"] = color;
                        dtForAQI.Rows.Add(dr);

                    }
                    //}
                    dataView = dtForAQI.AsDataView();
                    string data = "[";
                    for (int j = 0; j < dtForAQI.Rows.Count; j++)
                    {
                        string pointId = dtForAQI.Rows[j][0].ToString();
                        dataView.RowFilter = "PointId='" + pointId + "'";
                        data += "{";
                        data += string.Format(" name: '{0}',data:[ ", pointId);
                        int m = 0;
                        foreach (DataRowView drv in dataView)
                        {
                            m++;
                            //DateTime tstamp = Convert.ToDateTime(drv["DateTime1"]);
                            //string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                            if (m != dataView.Count)
                            {
                                if (drv["PM25"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM25"].ToString() + "],";
                                }
                                if (drv["PM10"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM10"].ToString() + "],";
                                }
                                if (drv["NO2"] != DBNull.Value)
                                {
                                    data += "[" + drv["NO2"].ToString() + "],";
                                }
                                if (drv["SO2"] != DBNull.Value)
                                {
                                    data += "[" + drv["SO2"].ToString() + "],";
                                }
                                if (drv["CO"] != DBNull.Value)
                                {
                                    data += "[" + drv["CO"].ToString() + "],";
                                }
                                if (drv["Recent8HoursO3NT"] != DBNull.Value)
                                {
                                    data += "[" + drv["Recent8HoursO3NT"].ToString() + "],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }
                            else
                            {
                                if (drv["PM25"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM25"].ToString() + "],";
                                }
                                if (drv["PM10"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM10"].ToString() + "],";
                                }
                                if (drv["NO2"] != DBNull.Value)
                                {
                                    data += "[" + drv["NO2"].ToString() + "],";
                                }
                                if (drv["SO2"] != DBNull.Value)
                                {
                                    data += "[" + drv["SO2"].ToString() + "],";
                                }
                                if (drv["CO"] != DBNull.Value)
                                {
                                    data += "[" + drv["CO"].ToString() + "],";
                                }
                                if (drv["Recent8HoursO3NT"] != DBNull.Value)
                                {
                                    data += "[" + drv["Recent8HoursO3NT"].ToString() + "],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }

                        }
                        data += "]},";
                    }
                    for (int j = 0; j < dtForAQI.Rows.Count; j++)
                    {
                        string pointId = dtForAQI.Rows[j][0].ToString();
                        dataView.RowFilter = "PointId='" + pointId + "'";
                        data += "{";
                        data += string.Format(" name: '{0}',data:[ ", pointId);
                        int m = 0;
                        foreach (DataRowView drv in dataView)
                        {
                            m++;
                            //DateTime tstamp = Convert.ToDateTime(drv["DateTime1"]);
                            //string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                            if (m != dataView.Count)
                            {
                                if (drv["PM25"] != DBNull.Value)
                                {
                                    data += "[" + Convert.ToDecimal(drv["PM25"].ToString()) * 1000 + "],";
                                }
                                if (drv["PM10"] != DBNull.Value)
                                {
                                    data += "[" + Convert.ToDecimal(drv["PM10"].ToString()) * 1000 + "],";
                                }
                                if (drv["NO2"] != DBNull.Value)
                                {
                                    data += "[" + Convert.ToDecimal(drv["NO2"].ToString()) * 1000 + "],";
                                }
                                if (drv["SO2"] != DBNull.Value)
                                {
                                    data += "[" + Convert.ToDecimal(drv["SO2"].ToString()) * 1000 + "],";
                                }
                                if (drv["CO"] != DBNull.Value)
                                {
                                    data += "[],";
                                }
                                if (drv["Recent8HoursO3NT"] != DBNull.Value)
                                {
                                    data += "[" + Convert.ToDecimal(drv["Recent8HoursO3NT"].ToString()) * 1000 + "],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }
                            else
                            {
                                if (drv["PM25"] != DBNull.Value)
                                {
                                    data += "[],";
                                }
                                if (drv["PM10"] != DBNull.Value)
                                {
                                    data += "[],";
                                }
                                if (drv["NO2"] != DBNull.Value)
                                {
                                    data += "[],";
                                }
                                if (drv["SO2"] != DBNull.Value)
                                {
                                    data += "[],";
                                }
                                if (drv["CO"] != DBNull.Value)
                                {
                                    data += "[" + drv["CO"].ToString() + "],";
                                }
                                if (drv["Recent8HoursO3NT"] != DBNull.Value)
                                {
                                    data += "[],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }

                        }
                        data += "]},";
                    }
                    if (data != "")
                    {
                        data = data.Substring(0, data.Length - 1);
                    }
                    data += "]";
                    hdjsonData.Value = data;

                    RegisterScript("generate();");
                }
                #endregion
                #region 区域日审核六参数浓度值
                if (dsType == "RDayAudit")
                {
                    MonitoringPointAirService monitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();

                    dvRegion = GetRegionByPointId(pointIds);
                    foreach (DataRowView dr in dvRegion)
                    {
                        string regionUid = dr["RegionUid"].ToString();
                        listRegionUids.Add(regionUid);
                    }
                    string[] regionUids = listRegionUids.ToArray();
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
                    dtForAQI.Columns.Add("AQIValue", typeof(string));
                    dtForAQI.Columns.Add("PrimaryPollutant", typeof(string));
                    dtForAQI.Columns.Add("Grade", typeof(string));
                    dtForAQI.Columns.Add("Class", typeof(string));
                    dtForAQI.Columns.Add("RGBValue", typeof(string));

                    List<string> regionName = dvRegion.ToTable().AsEnumerable().Select(t => t.Field<string>("Region")).ToList();
                    IEnumerable<string> names = regionName.Distinct();
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
                        DateTime hourBegion = dtStart;
                        DateTime hourEndion = dtEnd;
                        decimal? PM25PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34004", hourBegion, hourEndion, 1, "2");
                        decimal? PM10PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34002", hourBegion, hourEndion, 1, "2");
                        decimal? NO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21004", hourBegion, hourEndion, 1, "2");
                        decimal? SO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21026", hourBegion, hourEndion, 1, "2");
                        decimal? COPollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21005", hourBegion, hourEndion, 1, "2");
                        decimal? O3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05024", hourBegion, hourEndion, 1, "2");
                        int? PM25Value = m_AQICalculateService.GetIAQI("a34004", Convert.ToDouble(PM25PollutantValue), 24);
                        int? PM10Value = m_AQICalculateService.GetIAQI("a34002", Convert.ToDouble(PM10PollutantValue), 24);
                        int? NO2Value = m_AQICalculateService.GetIAQI("a21004", Convert.ToDouble(NO2PollutantValue), 1);
                        int? SO2Value = m_AQICalculateService.GetIAQI("a21026", Convert.ToDouble(SO2PollutantValue), 1);
                        int? COValue = m_AQICalculateService.GetIAQI("a21005", Convert.ToDouble(COPollutantValue), 1);
                        int? O3Value = m_AQICalculateService.GetIAQI("a05024", Convert.ToDouble(O3PollutantValue), 1);
                        string AQIValue = m_AQICalculateService.GetAQI_Max_CNV(PM25Value, PM10Value, NO2Value, SO2Value, COValue, O3Value, "V");
                        string primaryPollutant = m_AQICalculateService.GetAQI_Max_CNV(PM25Value, PM10Value, NO2Value, SO2Value, COValue, O3Value, "N");
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
                        dr["O3"] = O3PollutantValue.ToString();
                        dr["O3_AQI"] = O3Value.ToString();
                        dr["AQIValue"] = AQIValue;
                        dr["PrimaryPollutant"] = primaryPollutant;
                        dr["Grade"] = grade;
                        dr["Class"] = class_AQI;
                        dr["RGBValue"] = color;
                        dtForAQI.Rows.Add(dr);

                    }
                    dataView = dtForAQI.AsDataView();
                    string data = "[";
                    for (int j = 0; j < dtForAQI.Rows.Count; j++)
                    {
                        string pointId = dtForAQI.Rows[j][0].ToString();
                        dataView.RowFilter = "PointId='" + pointId + "'";
                        data += "{";
                        data += string.Format(" name: '{0}',data:[ ", pointId);
                        int m = 0;
                        foreach (DataRowView drv in dataView)
                        {
                            m++;
                            //DateTime tstamp = Convert.ToDateTime(drv["DateTime1"]);
                            //string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                            if (m != dataView.Count)
                            {
                                if (drv["PM25"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM25"].ToString() + "],";
                                }
                                if (drv["PM10"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM10"].ToString() + "],";
                                }
                                if (drv["NO2"] != DBNull.Value)
                                {
                                    data += "[" + drv["NO2"].ToString() + "],";
                                }
                                if (drv["SO2"] != DBNull.Value)
                                {
                                    data += "[" + drv["SO2"].ToString() + "],";
                                }
                                if (drv["CO"] != DBNull.Value)
                                {
                                    data += "[" + drv["CO"].ToString() + "],";
                                }
                                if (drv["O3"] != DBNull.Value)
                                {
                                    data += "[" + drv["O3"].ToString() + "],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }
                            else
                            {
                                if (drv["PM25"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM25"].ToString() + "],";
                                }
                                if (drv["PM10"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM10"].ToString() + "],";
                                }
                                if (drv["NO2"] != DBNull.Value)
                                {
                                    data += "[" + drv["NO2"].ToString() + "],";
                                }
                                if (drv["SO2"] != DBNull.Value)
                                {
                                    data += "[" + drv["SO2"].ToString() + "],";
                                }
                                if (drv["CO"] != DBNull.Value)
                                {
                                    data += "[" + drv["CO"].ToString() + "],";
                                }
                                if (drv["O3"] != DBNull.Value)
                                {
                                    data += "[" + drv["O3"].ToString() + "],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }

                        }
                        data += "]},";
                    }
                    if (data != "")
                    {
                        data = data.Substring(0, data.Length - 1);
                    }
                    data += "]";
                    hdjsonData.Value = data;

                    RegisterScript("generate();");
                }
                #endregion
                #region 站点小时原始六参数浓度值
                if (dsType == "PHourOri")
                {
                    MonitoringPointAirService monitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
                    hdxData.Value = chartType;
                    if ((int)(dtEnd - dtStart).TotalHours == 23 && dtStart.ToString("yyyy-MM-dd") == dtEnd.ToString("yyyy-MM-dd"))
                    {
                        //dataView = m_DayAQIService.GetPointAQIHourInfo(pointIds, dtStart, dtEnd.AddHours(1).AddSeconds(-1), qty, "OriData");
                        dataView = m_DayAQIService.GetPointAQIHourInfoOver23(pointIds.ToArray(), dtStart, dtEnd.AddHours(1).AddSeconds(-1), qty, "OriData");
                    }
                    else
                    {
                        //dataView = m_DayAQIService.GetPointAQIHourInfoOver23(pointIds.ToArray(), dtStart, dtEnd.AddHours(1).AddSeconds(-1), qty, "OriData");
                        dataView = m_DayAQIService.GetPointAQIHourInfo(pointIds, dtStart, dtEnd.AddHours(1).AddSeconds(-1), qty, "OriData");
                    }
                    string data = "[";
                    for (int j = 0; j < pointIds.Length; j++)
                    {
                        string pointId = pointIds[j];
                        MonitoringPointEntity monitorPoint = monitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(pointId));
                        dataView.RowFilter = "PointId='" + pointIds[j] + "'";
                        data += "{";
                        data += string.Format(" name: '{0}',yAxis:0,data:[ ", monitorPoint.MonitoringPointName);
                        int m = 0;
                        foreach (DataRowView drv in dataView)
                        {
                            m++;
                            //DateTime tstamp = Convert.ToDateTime(drv["DateTime1"]);
                            //string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                            if (m != dataView.Count)
                            {
                                if (drv["PM25"] != DBNull.Value)
                                {
                                    data += "[" + Convert.ToDecimal(drv["PM25"].ToString()) * 1000 + "],";
                                }
                                if (drv["PM10"] != DBNull.Value)
                                {
                                    data += "[" + Convert.ToDecimal(drv["PM10"].ToString()) * 1000 + "],";
                                }
                                if (drv["NO2"] != DBNull.Value)
                                {
                                    data += "[" + Convert.ToDecimal(drv["NO2"].ToString()) * 1000 + "],";
                                }
                                if (drv["SO2"] != DBNull.Value)
                                {
                                    data += "[" + Convert.ToDecimal(drv["SO2"].ToString()) * 1000 + "],";
                                }
                                if (drv["CO"] != DBNull.Value)
                                {
                                    data += "[],";
                                }
                                if (drv["Recent8HoursO3NT"] != DBNull.Value)
                                {
                                    data += "[" + Convert.ToDecimal(drv["Recent8HoursO3NT"].ToString())*1000 + "],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }
                            else
                            {
                                if (drv["PM25"] != DBNull.Value)
                                {
                                    data += "[" + Convert.ToDecimal(drv["PM25"].ToString())*1000 + "],";
                                }
                                if (drv["PM10"] != DBNull.Value)
                                {
                                    data += "[" + Convert.ToDecimal(drv["PM10"].ToString())*1000 + "],";
                                }
                                if (drv["NO2"] != DBNull.Value)
                                {
                                    data += "[" + Convert.ToDecimal(drv["NO2"].ToString())*1000 + "],";
                                }
                                if (drv["SO2"] != DBNull.Value)
                                {
                                    data += "[" + Convert.ToDecimal(drv["SO2"].ToString())*1000 + "],";
                                }
                                if (drv["CO"] != DBNull.Value)
                                {
                                    data += "[],";
                                }
                                if (drv["Recent8HoursO3NT"] != DBNull.Value)
                                {
                                    data += "[" + Convert.ToDecimal(drv["Recent8HoursO3NT"].ToString())*1000 + "],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }

                        }
                        data += "]},";
                    }
                    for (int j = 0; j < pointIds.Length; j++)
                    {
                        string pointId = pointIds[j];
                        MonitoringPointEntity monitorPoint = monitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(pointId));
                        dataView.RowFilter = "PointId='" + pointIds[j] + "'";
                        data += "{";
                        data += string.Format(" name: '{0}',yAxis:1,data:[ ", monitorPoint.MonitoringPointName);
                        int m = 0;
                        foreach (DataRowView drv in dataView)
                        {
                            m++;
                            //DateTime tstamp = Convert.ToDateTime(drv["DateTime1"]);
                            //string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                            if (m != dataView.Count)
                            {
                                if (drv["PM25"] != DBNull.Value)
                                {
                                    data += "[],";
                                }
                                if (drv["PM10"] != DBNull.Value)
                                {
                                    data += "[],";
                                }
                                if (drv["NO2"] != DBNull.Value)
                                {
                                    data += "[],";
                                }
                                if (drv["SO2"] != DBNull.Value)
                                {
                                    data += "[],";
                                }
                                if (drv["CO"] != DBNull.Value)
                                {
                                    data += "[" + drv["CO"].ToString() + "],";
                                }
                                if (drv["Recent8HoursO3NT"] != DBNull.Value)
                                {
                                    data += "[],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }
                            else
                            {
                                if (drv["PM25"] != DBNull.Value)
                                {
                                    data += "[],";
                                }
                                if (drv["PM10"] != DBNull.Value)
                                {
                                    data += "[],";
                                }
                                if (drv["NO2"] != DBNull.Value)
                                {
                                    data += "[],";
                                }
                                if (drv["SO2"] != DBNull.Value)
                                {
                                    data += "[],";
                                }
                                if (drv["CO"] != DBNull.Value)
                                {
                                    data += "[" + drv["CO"].ToString() + "],";
                                }
                                if (drv["Recent8HoursO3NT"] != DBNull.Value)
                                {
                                    data += "[],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }

                        }
                        data += "]},";
                    }
                    if (data != "")
                    {
                        data = data.Substring(0, data.Length - 1);
                    }
                    data += "]";
                    hdjsonData.Value = data;
                }
                #endregion
                #region 站点日原始六参数浓度值
                if (dsType == "PDayOri")
                {
                    MonitoringPointAirService monitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
                    hdxData.Value = chartType;
                    dataView = m_DayAQIService.GetPointAQIDayInfo(pointIds, dtStart, dtEnd, qty, "OriData");
                    string data = "[";
                    for (int j = 0; j < pointIds.Length; j++)
                    {
                        string pointId = pointIds[j];
                        MonitoringPointEntity monitorPoint = monitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(pointId));
                        dataView.RowFilter = "PointId='" + pointIds[j] + "'";
                        data += "{";
                        data += string.Format(" name: '{0}',data:[ ", monitorPoint.MonitoringPointName);
                        int m = 0;
                        foreach (DataRowView drv in dataView)
                        {
                            m++;
                            //DateTime tstamp = Convert.ToDateTime(drv["DateTime1"]);
                            //string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                            if (m != dataView.Count)
                            {
                                if (drv["PM25"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM25"].ToString() + "],";
                                }
                                if (drv["PM10"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM10"].ToString() + "],";
                                }
                                if (drv["NO2"] != DBNull.Value)
                                {
                                    data += "[" + drv["NO2"].ToString() + "],";
                                }
                                if (drv["SO2"] != DBNull.Value)
                                {
                                    data += "[" + drv["SO2"].ToString() + "],";
                                }
                                if (drv["CO"] != DBNull.Value)
                                {
                                    data += "[" + drv["CO"].ToString() + "],";
                                }
                                if (drv["O3"] != DBNull.Value)
                                {
                                    data += "[" + drv["O3"].ToString() + "],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }
                            else
                            {
                                if (drv["PM25"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM25"].ToString() + "],";
                                }
                                if (drv["PM10"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM10"].ToString() + "],";
                                }
                                if (drv["NO2"] != DBNull.Value)
                                {
                                    data += "[" + drv["NO2"].ToString() + "],";
                                }
                                if (drv["SO2"] != DBNull.Value)
                                {
                                    data += "[" + drv["SO2"].ToString() + "],";
                                }
                                if (drv["CO"] != DBNull.Value)
                                {
                                    data += "[" + drv["CO"].ToString() + "],";
                                }
                                if (drv["O3"] != DBNull.Value)
                                {
                                    data += "[" + drv["O3"].ToString() + "],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }

                        }
                        data += "]},";
                    }
                    if (data != "")
                    {
                        data = data.Substring(0, data.Length - 1);
                    }
                    data += "]";
                    hdjsonData.Value = data;
                }
                #endregion
                #region 区域小时原始六参数浓度值
                if (dsType == "RHourOri")
                {
                    MonitoringPointAirService monitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();

                    dvRegion = GetRegionByPointId(pointIds);
                    foreach (DataRowView dr in dvRegion)
                    {
                        string regionUid = dr["RegionUid"].ToString();
                        listRegionUids.Add(regionUid);
                    }
                    string[] regionUids = listRegionUids.ToArray();
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
                    dtForAQI.Columns.Add("AQIValue", typeof(string));
                    dtForAQI.Columns.Add("PrimaryPollutant", typeof(string));
                    dtForAQI.Columns.Add("Grade", typeof(string));
                    dtForAQI.Columns.Add("Class", typeof(string));
                    dtForAQI.Columns.Add("RGBValue", typeof(string));

                    //int hourNum = Convert.ToInt32((dtmhourEnd.Subtract(dtmhourBegin)).TotalHours);
                    //for (int i = 0; i < hourNum; i++)
                    //{

                    List<string> regionName = dvRegion.ToTable().AsEnumerable().Select(t => t.Field<string>("Region")).ToList();
                    IEnumerable<string> names = regionName.Distinct();
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

                        DateTime hourBegion = dtStart;
                        DateTime hourEndion = dtEnd;
                        decimal? PM25PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34004", hourBegion, hourEndion, 1, "1");
                        decimal? PM10PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34002", hourBegion, hourEndion, 1, "1");
                        decimal? NO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21004", hourBegion, hourEndion, 1, "1");
                        decimal? SO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21026", hourBegion, hourEndion, 1, "1");
                        decimal? COPollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21005", hourBegion, hourEndion, 1, "1");
                        //decimal? O3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05024", hourBegion, hourEndion, 1, "1");
                        decimal? O3PollutantValue = 0;
                        if ((int)(hourEndion - hourBegion).TotalHours == 23 && hourBegion.ToString("yyyy-MM-dd") == hourEndion.ToString("yyyy-MM-dd"))
                        {
                            //求最大的可跨天的O3_NT
                            O3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05024", hourBegion, hourEndion, 1, "1");
                        }
                        else
                        {
                            O3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05027", hourBegion, hourEndion, 1, "1");
                        }
                        int? PM25Value = m_AQICalculateService.GetIAQI("a34004", Convert.ToDouble(PM25PollutantValue), 24);
                        int? PM10Value = m_AQICalculateService.GetIAQI("a34002", Convert.ToDouble(PM10PollutantValue), 24);
                        int? NO2Value = m_AQICalculateService.GetIAQI("a21004", Convert.ToDouble(NO2PollutantValue), 1);
                        int? SO2Value = m_AQICalculateService.GetIAQI("a21026", Convert.ToDouble(SO2PollutantValue), 1);
                        int? COValue = m_AQICalculateService.GetIAQI("a21005", Convert.ToDouble(COPollutantValue), 1);
                        int? O3Value = m_AQICalculateService.GetIAQI("a05024", Convert.ToDouble(O3PollutantValue), 1);
                        string AQIValue = m_AQICalculateService.GetAQI_Max_CNV(PM25Value, PM10Value, NO2Value, SO2Value, COValue, O3Value, "V");
                        string primaryPollutant = m_AQICalculateService.GetAQI_Max_CNV(PM25Value, PM10Value, NO2Value, SO2Value, COValue, O3Value, "N");
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
                        dr["O3"] = O3PollutantValue.ToString();
                        dr["O3_AQI"] = O3Value.ToString();
                        dr["AQIValue"] = AQIValue;
                        dr["PrimaryPollutant"] = primaryPollutant;
                        dr["Grade"] = grade;
                        dr["Class"] = class_AQI;
                        dr["RGBValue"] = color;
                        dtForAQI.Rows.Add(dr);

                    }
                    //}
                    dataView = dtForAQI.AsDataView();
                    string data = "[";
                    for (int j = 0; j < dtForAQI.Rows.Count; j++)
                    {
                        string pointId = dtForAQI.Rows[j][0].ToString();
                        dataView.RowFilter = "PointId='" + pointId + "'";
                        data += "{";
                        data += string.Format(" name: '{0}',yAxis:0,data:[ ", pointId);
                        int m = 0;
                        foreach (DataRowView drv in dataView)
                        {
                            m++;
                            //DateTime tstamp = Convert.ToDateTime(drv["DateTime1"]);
                            //string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                            if (m != dataView.Count)
                            {
                                if (drv["PM25"] != DBNull.Value)
                                {
                                    data += "[" + DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv["PM25"].ToString()) * 1000,0) + "],";
                                }
                                if (drv["PM10"] != DBNull.Value)
                                {
                                    data += "[" + DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv["PM10"].ToString()) * 1000,0) + "],";
                                }
                                if (drv["NO2"] != DBNull.Value)
                                {
                                    data += "[" + DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv["NO2"].ToString()) * 1000,0) + "],";
                                }
                                if (drv["SO2"] != DBNull.Value)
                                {
                                    data += "[" + DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv["SO2"].ToString()) * 1000,0) + "],";
                                }
                                if (drv["CO"] != DBNull.Value)
                                {
                                    data += "[],";
                                }
                                if (drv["O3"] != DBNull.Value)
                                {
                                    data += "[" + DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv["O3"].ToString()) * 1000,0) + "],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }
                            else
                            {
                                if (drv["PM25"] != DBNull.Value)
                                {
                                    data += "[" + DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv["PM25"].ToString()) * 1000,0) + "],";
                                }
                                if (drv["PM10"] != DBNull.Value)
                                {
                                    data += "[" + DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv["PM10"].ToString()) * 1000,0) + "],";
                                }
                                if (drv["NO2"] != DBNull.Value)
                                {
                                    data += "[" + DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv["NO2"].ToString()) * 1000,0) + "],";
                                }
                                if (drv["SO2"] != DBNull.Value)
                                {
                                    data += "[" + DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv["SO2"].ToString()) * 1000,0) + "],";
                                }
                                if (drv["CO"] != DBNull.Value)
                                {
                                    data += "[],";
                                }
                                if (drv["O3"] != DBNull.Value)
                                {
                                    data += "[" + DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv["O3"].ToString()) * 1000,0) + "],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }

                        }
                        data += "]},";
                    }
                    for (int j = 0; j < dtForAQI.Rows.Count; j++)
                    {
                        string pointId = dtForAQI.Rows[j][0].ToString();
                        dataView.RowFilter = "PointId='" + pointId + "'";
                        data += "{";
                        data += string.Format(" name: '{0}',yAxis:1,data:[ ", pointId);
                        int m = 0;
                        foreach (DataRowView drv in dataView)
                        {
                            m++;
                            //DateTime tstamp = Convert.ToDateTime(drv["DateTime1"]);
                            //string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                            if (m != dataView.Count)
                            {
                                if (drv["PM25"] != DBNull.Value)
                                {
                                    data += "[],";
                                }
                                if (drv["PM10"] != DBNull.Value)
                                {
                                    data += "[],";
                                }
                                if (drv["NO2"] != DBNull.Value)
                                {
                                    data += "[],";
                                }
                                if (drv["SO2"] != DBNull.Value)
                                {
                                    data += "[],";
                                }
                                if (drv["CO"] != DBNull.Value)
                                {
                                    data += "[" + DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv["CO"].ToString()), 1) + "],";
                                }
                                if (drv["O3"] != DBNull.Value)
                                {
                                    data += "[],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }
                            else
                            {
                                if (drv["PM25"] != DBNull.Value)
                                {
                                    data += "[],";
                                }
                                if (drv["PM10"] != DBNull.Value)
                                {
                                    data += "[],";
                                }
                                if (drv["NO2"] != DBNull.Value)
                                {
                                    data += "[],";
                                }
                                if (drv["SO2"] != DBNull.Value)
                                {
                                    data += "[],";
                                }
                                if (drv["CO"] != DBNull.Value)
                                {
                                    data += "[" + DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv["CO"].ToString()),1) + "],";
                                }
                                if (drv["O3"] != DBNull.Value)
                                {
                                    data += "[],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }

                        }
                        data += "]},";
                    }




                    //for (int j = 0; j < dtForAQI.Rows.Count; j++)
                    //{
                    //    string pointId = dtForAQI.Rows[j][0].ToString();
                    //    dataView.RowFilter = "PointId='" + pointId + "'";
                    //    data += "{";
                    //    data += string.Format(" name: '{0}',data:[ ", pointId);
                    //    int m = 0;
                    //    foreach (DataRowView drv in dataView)
                    //    {
                    //        m++;
                    //        //DateTime tstamp = Convert.ToDateTime(drv["DateTime1"]);
                    //        //string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                    //        if (m != dataView.Count)
                    //        {
                    //            if (drv["PM25"] != DBNull.Value)
                    //            {
                    //                data += "[" + DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv["PM25"].ToString()) * 1000,0) + "],";
                    //            }
                    //            if (drv["PM10"] != DBNull.Value)
                    //            {
                    //                data += "[" + DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv["PM10"].ToString()) * 1000,0) + "],";
                    //            }
                    //            if (drv["NO2"] != DBNull.Value)
                    //            {
                    //                data += "[" + DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv["NO2"].ToString()) * 1000,0) + "],";
                    //            }
                    //            if (drv["SO2"] != DBNull.Value)
                    //            {
                    //                data += "[" + DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv["SO2"].ToString()) * 1000,0) + "],";
                    //            }
                    //            if (drv["CO"] != DBNull.Value)
                    //            {
                    //                data += "[" + DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv["CO"].ToString()),1) + "],";
                    //            }
                    //            if (drv["Recent8HoursO3NT"] != DBNull.Value)
                    //            {
                    //                data += "[" + DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv["Recent8HoursO3NT"].ToString()) * 1000,0) + "],";
                    //            }
                    //            else
                    //            {
                    //                data += "[" + "null]";
                    //            }
                    //        }
                    //        else
                    //        {
                    //            if (drv["PM25"] != DBNull.Value)
                    //            {
                    //                data += "[" + DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv["PM25"].ToString()) * 1000,0) + "],";
                    //            }
                    //            if (drv["PM10"] != DBNull.Value)
                    //            {
                    //                data += "[" + DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv["PM10"].ToString()) * 1000,0) + "],";
                    //            }
                    //            if (drv["NO2"] != DBNull.Value)
                    //            {
                    //                data += "[" + DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv["NO2"].ToString()) * 1000,0) + "],";
                    //            }
                    //            if (drv["SO2"] != DBNull.Value)
                    //            {
                    //                data += "[" + DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv["SO2"].ToString()) * 1000,0) + "],";
                    //            }
                    //            if (drv["CO"] != DBNull.Value)
                    //            {
                    //                data += "[" + DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv["CO"].ToString()), 1) + "],";
                    //            }
                    //            if (drv["O3"] != DBNull.Value)
                    //            {
                    //                data += "[" + DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv["O3"].ToString()) * 1000,0) + "],";
                    //            }
                    //            else
                    //            {
                    //                data += "[" + "null]";
                    //            }
                    //        }

                    //    }
                    //    data += "]},";
                    //}
                    if (data != "")
                    {
                        data = data.Substring(0, data.Length - 1);
                    }
                    data += "]";
                    hdjsonData.Value = data;

                    RegisterScript("generate();");
                }
                #endregion
                #region 区域日原始六参数浓度值
                if (dsType == "RDayOri")
                {
                    MonitoringPointAirService monitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();

                    dvRegion = GetRegionByPointId(pointIds);
                    foreach (DataRowView dr in dvRegion)
                    {
                        string regionUid = dr["RegionUid"].ToString();
                        listRegionUids.Add(regionUid);
                    }
                    string[] regionUids = listRegionUids.ToArray();
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
                    dtForAQI.Columns.Add("AQIValue", typeof(string));
                    dtForAQI.Columns.Add("PrimaryPollutant", typeof(string));
                    dtForAQI.Columns.Add("Grade", typeof(string));
                    dtForAQI.Columns.Add("Class", typeof(string));
                    dtForAQI.Columns.Add("RGBValue", typeof(string));

                    List<string> regionName = dvRegion.ToTable().AsEnumerable().Select(t => t.Field<string>("Region")).ToList();
                    IEnumerable<string> names = regionName.Distinct();
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
                        DateTime hourBegion = dtStart;
                        DateTime hourEndion = dtEnd;
                        decimal? PM25PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34004", hourBegion, hourEndion, 1, "1");
                        decimal? PM10PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34002", hourBegion, hourEndion, 1, "1");
                        decimal? NO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21004", hourBegion, hourEndion, 1, "1");
                        decimal? SO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21026", hourBegion, hourEndion, 1, "1");
                        decimal? COPollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21005", hourBegion, hourEndion, 1, "1");
                        decimal? O3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05024", hourBegion, hourEndion, 1, "1");
                        int? PM25Value = m_AQICalculateService.GetIAQI("a34004", Convert.ToDouble(PM25PollutantValue), 24);
                        int? PM10Value = m_AQICalculateService.GetIAQI("a34002", Convert.ToDouble(PM10PollutantValue), 24);
                        int? NO2Value = m_AQICalculateService.GetIAQI("a21004", Convert.ToDouble(NO2PollutantValue), 1);
                        int? SO2Value = m_AQICalculateService.GetIAQI("a21026", Convert.ToDouble(SO2PollutantValue), 1);
                        int? COValue = m_AQICalculateService.GetIAQI("a21005", Convert.ToDouble(COPollutantValue), 1);
                        int? O3Value = m_AQICalculateService.GetIAQI("a05024", Convert.ToDouble(O3PollutantValue), 1);
                        string AQIValue = m_AQICalculateService.GetAQI_Max_CNV(PM25Value, PM10Value, NO2Value, SO2Value, COValue, O3Value, "V");
                        string primaryPollutant = m_AQICalculateService.GetAQI_Max_CNV(PM25Value, PM10Value, NO2Value, SO2Value, COValue, O3Value, "N");
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
                        dr["O3"] = O3PollutantValue.ToString();
                        dr["O3_AQI"] = O3Value.ToString();
                        dr["AQIValue"] = AQIValue;
                        dr["PrimaryPollutant"] = primaryPollutant;
                        dr["Grade"] = grade;
                        dr["Class"] = class_AQI;
                        dr["RGBValue"] = color;
                        dtForAQI.Rows.Add(dr);

                    }
                    dataView = dtForAQI.AsDataView();
                    string data = "[";
                    for (int j = 0; j < dtForAQI.Rows.Count; j++)
                    {
                        string pointId = dtForAQI.Rows[j][0].ToString();
                        dataView.RowFilter = "PointId='" + pointId + "'";
                        data += "{";
                        data += string.Format(" name: '{0}',data:[ ", pointId);
                        int m = 0;
                        foreach (DataRowView drv in dataView)
                        {
                            m++;
                            //DateTime tstamp = Convert.ToDateTime(drv["DateTime1"]);
                            //string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                            if (m != dataView.Count)
                            {
                                if (drv["PM25"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM25"].ToString() + "],";
                                }
                                if (drv["PM10"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM10"].ToString() + "],";
                                }
                                if (drv["NO2"] != DBNull.Value)
                                {
                                    data += "[" + drv["NO2"].ToString() + "],";
                                }
                                if (drv["SO2"] != DBNull.Value)
                                {
                                    data += "[" + drv["SO2"].ToString() + "],";
                                }
                                if (drv["CO"] != DBNull.Value)
                                {
                                    data += "[" + drv["CO"].ToString() + "],";
                                }
                                if (drv["O3"] != DBNull.Value)
                                {
                                    data += "[" + drv["O3"].ToString() + "],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }
                            else
                            {
                                if (drv["PM25"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM25"].ToString() + "],";
                                }
                                if (drv["PM10"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM10"].ToString() + "],";
                                }
                                if (drv["NO2"] != DBNull.Value)
                                {
                                    data += "[" + drv["NO2"].ToString() + "],";
                                }
                                if (drv["SO2"] != DBNull.Value)
                                {
                                    data += "[" + drv["SO2"].ToString() + "],";
                                }
                                if (drv["CO"] != DBNull.Value)
                                {
                                    data += "[" + drv["CO"].ToString() + "],";
                                }
                                if (drv["O3"] != DBNull.Value)
                                {
                                    data += "[" + drv["O3"].ToString() + "],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }

                        }
                        data += "]},";
                    }
                    if (data != "")
                    {
                        data = data.Substring(0, data.Length - 1);
                    }
                    data += "]";
                    hdjsonData.Value = data;

                    RegisterScript("generate();");
                }
                #endregion
            }
            #endregion

            #region //六参数分指数（IAQI）

            else if (chartContent == "factorIAQI")
            {
                MonitoringPointAirService monitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
                hdjsonTitle.Value = "六参数分指数(IAQI)";
                #region 站点小时审核六参数分指数(IAQI)
                if (dsType == "PHourAudit")
                {
                    hdxData.Value = chartType;
                    //if ((dtEnd - dtStart).Days <= 1)
                    //{
                    //    dataView = m_DayAQIService.GetPointAQIHourInfo(pointIds, dtStart, dtEnd, qty, "AuditData");
                    //}
                    //else
                    //{
                    //    dataView = m_DayAQIService.GetPointAQIHourInfoOver23(pointIds, dtStart, dtEnd, qty, "Audit");
                    //}                    
                    //if ((int)(dtEnd - dtStart).TotalDays < 1)
                    //{
                    //    dataView = m_DayAQIService.GetPointAQIHourInfo(pointIds, dtStart, dtEnd.AddHours(1).AddSeconds(-1), qty, "Audit");
                    //}
                    //if ((int)(dtEnd - dtStart).TotalDays >= 1)
                    //{
                    //    dataView = m_DayAQIService.GetPointAQIHourInfoOver23(pointIds.ToArray(), dtStart, dtEnd.AddHours(1).AddSeconds(-1), qty, "Audit");
                    //}
                    if ((int)(dtEnd - dtStart).TotalHours == 23 && dtStart.ToString("yyyy-MM-dd") == dtEnd.ToString("yyyy-MM-dd"))
                    {
                        //dataView = m_DayAQIService.GetPointAQIHourInfo(pointIds, dtStart, dtEnd.AddHours(1).AddSeconds(-1), qty, "Audit");
                        dataView = m_DayAQIService.GetPointAQIHourInfoOver23(pointIds.ToArray(), dtStart, dtEnd.AddHours(1).AddSeconds(-1), qty, "Audit");
                    }
                    else
                    {
                        //dataView = m_DayAQIService.GetPointAQIHourInfoOver23(pointIds.ToArray(), dtStart, dtEnd.AddHours(1).AddSeconds(-1), qty, "Audit");
                        dataView = m_DayAQIService.GetPointAQIHourInfo(pointIds, dtStart, dtEnd.AddHours(1).AddSeconds(-1), qty, "Audit");
                    }
                    string data = "[";
                    for (int j = 0; j < pointIds.Length; j++)
                    {
                        string pointId = pointIds[j];
                        MonitoringPointEntity monitorPoint = monitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(pointId));
                        dataView.RowFilter = "PointId='" + pointIds[j] + "'";
                        data += "{";
                        data += string.Format(" name: '{0}',data:[ ", monitorPoint.MonitoringPointName);
                        int m = 0;
                        foreach (DataRowView drv in dataView)
                        {
                            m++;
                            //DateTime tstamp = Convert.ToDateTime(drv["DateTime1"]);
                            //string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                            if (m != dataView.Count)
                            {
                                if (drv["PM25_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM25_AQI"].ToString() + "],";
                                }
                                if (drv["PM10_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM10_AQI"].ToString() + "],";
                                }
                                if (drv["NO2_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["NO2_AQI"].ToString() + "],";
                                }
                                if (drv["SO2_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["SO2_AQI"].ToString() + "],";
                                }
                                if (drv["CO_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["CO_AQI"].ToString() + "],";
                                }
                                if (drv["Recent8HoursO3NT_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["Recent8HoursO3NT_AQI"].ToString() + "],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }
                            else
                            {
                                if (drv["PM25_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM25_AQI"].ToString() + "],";
                                }
                                if (drv["PM10_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM10_AQI"].ToString() + "],";
                                }
                                if (drv["NO2_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["NO2_AQI"].ToString() + "],";
                                }
                                if (drv["SO2_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["SO2_AQI"].ToString() + "],";
                                }
                                if (drv["CO_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["CO_AQI"].ToString() + "],";
                                }
                                if (drv["Recent8HoursO3NT_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["Recent8HoursO3NT_AQI"].ToString() + "],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }

                        }
                        data += "]},";
                    }
                    if (data != "")
                    {
                        data = data.Substring(0, data.Length - 1);
                    }
                    data += "]";
                    hdjsonData.Value = data;
                }
                #endregion
                #region 站点日审核六参数分指数(IAQI)
                if (dsType == "PDayAudit")
                {
                    hdxData.Value = chartType;
                    dataView = m_DayAQIService.GetPointAQIDayInfo(pointIds, dtStart, dtEnd, qty, "AuditData");

                    string data = "[";
                    for (int j = 0; j < pointIds.Length; j++)
                    {
                        string pointId = pointIds[j];
                        MonitoringPointEntity monitorPoint = monitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(pointId));
                        dataView.RowFilter = "PointId='" + pointIds[j] + "'";
                        data += "{";
                        data += string.Format(" name: '{0}',data:[ ", monitorPoint.MonitoringPointName);
                        int m = 0;
                        foreach (DataRowView drv in dataView)
                        {
                            m++;
                            //DateTime tstamp = Convert.ToDateTime(drv["DateTime1"]);
                            //string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                            if (m != dataView.Count)
                            {
                                if (drv["AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["AQI"].ToString() + "],";
                                }
                                else
                                {
                                    data += "[" + "null],";
                                }
                            }
                            else
                            {
                                if (drv["AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["AQI"].ToString() + "],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }

                        }
                        data += "]},";
                    }
                    if (data != "")
                    {
                        data = data.Substring(0, data.Length - 1);
                    }
                    data += "]";
                    hdjsonData.Value = data;

                    RegisterScript("generate();");
                }
                #endregion
                #region 区域小时审核六参数分指数(IAQI)
                if (dsType == "RHourAudit")
                {
                    dvRegion = GetRegionByPointId(pointIds);
                    foreach (DataRowView dr in dvRegion)
                    {
                        string regionUid = dr["RegionUid"].ToString();
                        listRegionUids.Add(regionUid);
                    }
                    string[] regionUids = listRegionUids.ToArray();
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
                    dtForAQI.Columns.Add("AQIValue", typeof(string));
                    dtForAQI.Columns.Add("PrimaryPollutant", typeof(string));
                    dtForAQI.Columns.Add("Grade", typeof(string));
                    dtForAQI.Columns.Add("Class", typeof(string));
                    dtForAQI.Columns.Add("RGBValue", typeof(string));

                    //int hourNum = Convert.ToInt32((dtmhourEnd.Subtract(dtmhourBegin)).TotalHours);
                    //for (int i = 0; i < hourNum; i++)
                    //{

                    List<string> regionName = dvRegion.ToTable().AsEnumerable().Select(t => t.Field<string>("Region")).ToList();
                    IEnumerable<string> names = regionName.Distinct();
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

                        DateTime hourBegion = dtStart;
                        DateTime hourEndion = dtEnd;
                        decimal? PM25PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34004", hourBegion, hourEndion, 1, "2");
                        decimal? PM10PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34002", hourBegion, hourEndion, 1, "2");
                        decimal? NO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21004", hourBegion, hourEndion, 1, "2");
                        decimal? SO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21026", hourBegion, hourEndion, 1, "2");
                        decimal? COPollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21005", hourBegion, hourEndion, 1, "2");
                        //decimal? O3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05024", hourBegion, hourEndion, 1, "2");
                        decimal? O3PollutantValue = 0;
                        if ((int)(hourEndion - hourBegion).TotalDays < 1)
                        {
                            //求最大的可跨天的O3_NT
                            O3PollutantValue = m_DayAQIService.GetOriDataPagerO3ForNT(ids, hourBegion, hourEndion, "AuditData");
                        }
                        else
                        {
                            O3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05024", Convert.ToDateTime(hourBegion.ToString("yyyy-MM-dd 00:00:00")), Convert.ToDateTime(hourEndion.ToString("yyyy-MM-dd 00:00:00")), 8, "2");
                        }

                        int? PM25Value = m_AQICalculateService.GetIAQI("a34004", Convert.ToDouble(PM25PollutantValue), 24);
                        int? PM10Value = m_AQICalculateService.GetIAQI("a34002", Convert.ToDouble(PM10PollutantValue), 24);
                        int? NO2Value = m_AQICalculateService.GetIAQI("a21004", Convert.ToDouble(NO2PollutantValue), 24);
                        int? SO2Value = m_AQICalculateService.GetIAQI("a21026", Convert.ToDouble(SO2PollutantValue), 24);
                        int? COValue = m_AQICalculateService.GetIAQI("a21005", Convert.ToDouble(COPollutantValue), 24);
                        int? O3Value = m_AQICalculateService.GetIAQI("a05024", Convert.ToDouble(O3PollutantValue), 8);
                        string AQIValue = m_AQICalculateService.GetAQI_Max_CNV(SO2Value, NO2Value, PM10Value, COValue, O3Value, PM25Value, "V");
                        string primaryPollutant = m_AQICalculateService.GetAQI_Max_CNV(SO2Value, NO2Value, PM10Value, COValue, O3Value, PM25Value, "N");
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
                        dr["O3"] = O3PollutantValue.ToString();
                        dr["O3_AQI"] = O3Value.ToString();
                        dr["AQIValue"] = AQIValue;
                        dr["PrimaryPollutant"] = primaryPollutant;
                        dr["Grade"] = grade;
                        dr["Class"] = class_AQI;
                        dr["RGBValue"] = color;
                        dtForAQI.Rows.Add(dr);

                    }
                    //}
                    dataView = dtForAQI.AsDataView();
                    string data = "[";
                    for (int j = 0; j < dtForAQI.Rows.Count; j++)
                    {
                        string pointId = dtForAQI.Rows[j][0].ToString();
                        dataView.RowFilter = "PointId='" + pointId + "'";
                        data += "{";
                        data += string.Format(" name: '{0}',data:[ ", pointId);
                        int m = 0;
                        foreach (DataRowView drv in dataView)
                        {
                            m++;
                            //DateTime tstamp = Convert.ToDateTime(drv["DateTime1"]);
                            //string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                            if (m != dataView.Count)
                            {
                                if (drv["PM25_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM25_AQI"].ToString() + "],";
                                }
                                if (drv["PM10_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM10_AQI"].ToString() + "],";
                                }
                                if (drv["NO2_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["NO2_AQI"].ToString() + "],";
                                }
                                if (drv["SO2_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["SO2_AQI"].ToString() + "],";
                                }
                                if (drv["CO_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["CO_AQI"].ToString() + "],";
                                }
                                if (drv["O3_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["O3_AQI"].ToString() + "],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }
                            else
                            {
                                if (drv["PM25_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM25_AQI"].ToString() + "],";
                                }
                                if (drv["PM10_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM10_AQI"].ToString() + "],";
                                }
                                if (drv["NO2_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["NO2_AQI"].ToString() + "],";
                                }
                                if (drv["SO2_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["SO2_AQI"].ToString() + "],";
                                }
                                if (drv["CO_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["CO_AQI"].ToString() + "],";
                                }
                                if (drv["O3_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["O3_AQI"].ToString() + "],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }

                        }
                        data += "]},";
                    }
                    if (data != "")
                    {
                        data = data.Substring(0, data.Length - 1);
                    }
                    data += "]";
                    hdjsonData.Value = data;

                    RegisterScript("generate();");
                }
                #endregion
                #region 区域日审核六参数分指数(IAQI)
                if (dsType == "RDayAudit")
                {
                    dvRegion = GetRegionByPointId(pointIds);
                    foreach (DataRowView dr in dvRegion)
                    {
                        string regionUid = dr["RegionUid"].ToString();
                        listRegionUids.Add(regionUid);
                    }
                    string[] regionUids = listRegionUids.ToArray();
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
                    dtForAQI.Columns.Add("AQIValue", typeof(string));
                    dtForAQI.Columns.Add("PrimaryPollutant", typeof(string));
                    dtForAQI.Columns.Add("Grade", typeof(string));
                    dtForAQI.Columns.Add("Class", typeof(string));
                    dtForAQI.Columns.Add("RGBValue", typeof(string));

                    List<string> regionName = dvRegion.ToTable().AsEnumerable().Select(t => t.Field<string>("Region")).ToList();
                    IEnumerable<string> names = regionName.Distinct();
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
                        DateTime hourBegion = dtStart;
                        DateTime hourEndion = dtEnd;
                        decimal? PM25PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34004", hourBegion, hourEndion, 1, "2");
                        decimal? PM10PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34002", hourBegion, hourEndion, 1, "2");
                        decimal? NO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21004", hourBegion, hourEndion, 1, "2");
                        decimal? SO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21026", hourBegion, hourEndion, 1, "2");
                        decimal? COPollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21005", hourBegion, hourEndion, 1, "2");
                        decimal? O3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05024", hourBegion, hourEndion, 1, "2");
                        int? PM25Value = m_AQICalculateService.GetIAQI("a34004", Convert.ToDouble(PM25PollutantValue), 24);
                        int? PM10Value = m_AQICalculateService.GetIAQI("a34002", Convert.ToDouble(PM10PollutantValue), 24);
                        int? NO2Value = m_AQICalculateService.GetIAQI("a21004", Convert.ToDouble(NO2PollutantValue), 1);
                        int? SO2Value = m_AQICalculateService.GetIAQI("a21026", Convert.ToDouble(SO2PollutantValue), 1);
                        int? COValue = m_AQICalculateService.GetIAQI("a21005", Convert.ToDouble(COPollutantValue), 1);
                        int? O3Value = m_AQICalculateService.GetIAQI("a05024", Convert.ToDouble(O3PollutantValue), 1);
                        string AQIValue = m_AQICalculateService.GetAQI_Max_CNV(PM25Value, PM10Value, NO2Value, SO2Value, COValue, O3Value, "V");
                        string primaryPollutant = m_AQICalculateService.GetAQI_Max_CNV(PM25Value, PM10Value, NO2Value, SO2Value, COValue, O3Value, "N");
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
                        dr["O3"] = O3PollutantValue.ToString();
                        dr["O3_AQI"] = O3Value.ToString();
                        dr["AQIValue"] = AQIValue;
                        dr["PrimaryPollutant"] = primaryPollutant;
                        dr["Grade"] = grade;
                        dr["Class"] = class_AQI;
                        dr["RGBValue"] = color;
                        dtForAQI.Rows.Add(dr);

                    }
                    dataView = dtForAQI.AsDataView();
                    string data = "[";
                    for (int j = 0; j < dtForAQI.Rows.Count; j++)
                    {
                        string pointId = dtForAQI.Rows[j][0].ToString();
                        dataView.RowFilter = "PointId='" + pointId + "'";
                        data += "{";
                        data += string.Format(" name: '{0}',data:[ ", pointId);
                        int m = 0;
                        foreach (DataRowView drv in dataView)
                        {
                            m++;
                            //DateTime tstamp = Convert.ToDateTime(drv["DateTime1"]);
                            //string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                            if (m != dataView.Count)
                            {
                                if (drv["PM25_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM25_AQI"].ToString() + "],";
                                }
                                if (drv["PM10_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM10_AQI"].ToString() + "],";
                                }
                                if (drv["NO2_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["NO2_AQI"].ToString() + "],";
                                }
                                if (drv["SO2_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["SO2_AQI"].ToString() + "],";
                                }
                                if (drv["CO_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["CO_AQI"].ToString() + "],";
                                }
                                if (drv["O3_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["O3_AQI"].ToString() + "],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }
                            else
                            {
                                if (drv["PM25_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM25_AQI"].ToString() + "],";
                                }
                                if (drv["PM10_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM10_AQI"].ToString() + "],";
                                }
                                if (drv["NO2_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["NO2_AQI"].ToString() + "],";
                                }
                                if (drv["SO2_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["SO2_AQI"].ToString() + "],";
                                }
                                if (drv["CO_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["CO_AQI"].ToString() + "],";
                                }
                                if (drv["O3_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["O3_AQI"].ToString() + "],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }

                        }
                        data += "]},";
                    }
                    if (data != "")
                    {
                        data = data.Substring(0, data.Length - 1);
                    }
                    data += "]";
                    hdjsonData.Value = data;

                    RegisterScript("generate();");
                }
                #endregion
                #region 站点小时原始六参数分指数(IAQI)
                if (dsType == "PHourOri")
                {
                    hdxData.Value = chartType;
                    //if ((dtEnd - dtStart).Days <= 1)
                    //{
                    //    //dataView = m_DayAQIService.GetPointAQIHourInfo(pointIds, dtStart, dtEnd, qty, "AuditData");
                    //    dataView = m_DayAQIService.GetPointAQIHourInfo(pointIds, dtStart, dtEnd, qty, "OriData");
                    //}
                    //else
                    //{
                    //    dataView = m_DayAQIService.GetPointAQIHourInfoOver23(pointIds, dtStart, dtEnd, qty, "OriData");
                    //}
                    if ((int)(dtEnd - dtStart).TotalHours == 23 && dtStart.ToString("yyyy-MM-dd") == dtEnd.ToString("yyyy-MM-dd"))
                    {
                        //dataView = m_DayAQIService.GetPointAQIHourInfo(pointIds, dtStart, dtEnd.AddHours(1).AddSeconds(-1), qty, "OriData");
                        dataView = m_DayAQIService.GetPointAQIHourInfoOver23(pointIds.ToArray(), dtStart, dtEnd.AddHours(1).AddSeconds(-1), qty, "OriData");
                    }
                    else
                    {
                        //dataView = m_DayAQIService.GetPointAQIHourInfoOver23(pointIds.ToArray(), dtStart, dtEnd.AddHours(1).AddSeconds(-1), qty, "OriData");
                        dataView = m_DayAQIService.GetPointAQIHourInfo(pointIds, dtStart, dtEnd.AddHours(1).AddSeconds(-1), qty, "OriData");
                    }
                    string data = "[";
                    for (int j = 0; j < pointIds.Length; j++)
                    {
                        string pointId = pointIds[j];
                        MonitoringPointEntity monitorPoint = monitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(pointId));
                        dataView.RowFilter = "PointId='" + pointIds[j] + "'";
                        data += "{";
                        data += string.Format(" name: '{0}',data:[ ", monitorPoint.MonitoringPointName);
                        int m = 0;
                        foreach (DataRowView drv in dataView)
                        {
                            m++;
                            //DateTime tstamp = Convert.ToDateTime(drv["DateTime1"]);
                            //string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                            if (m != dataView.Count)
                            {
                                if (drv["PM25_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM25_AQI"].ToString() + "],";
                                }
                                if (drv["PM10_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM10_AQI"].ToString() + "],";
                                }
                                if (drv["NO2_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["NO2_AQI"].ToString() + "],";
                                }
                                if (drv["SO2_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["SO2_AQI"].ToString() + "],";
                                }
                                if (drv["CO_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["CO_AQI"].ToString() + "],";
                                }
                                if (drv["Recent8HoursO3NT_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["Recent8HoursO3NT_AQI"].ToString() + "],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }
                            else
                            {
                                if (drv["PM25_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM25_AQI"].ToString() + "],";
                                }
                                if (drv["PM10_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM10_AQI"].ToString() + "],";
                                }
                                if (drv["NO2_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["NO2_AQI"].ToString() + "],";
                                }
                                if (drv["SO2_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["SO2_AQI"].ToString() + "],";
                                }
                                if (drv["CO_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["CO_AQI"].ToString() + "],";
                                }
                                if (drv["Recent8HoursO3NT_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["Recent8HoursO3NT_AQI"].ToString() + "],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }

                        }
                        data += "]},";
                    }
                    if (data != "")
                    {
                        data = data.Substring(0, data.Length - 1);
                    }
                    data += "]";
                    hdjsonData.Value = data;
                }
                #endregion
                #region 站点日原始六参数分指数(IAQI)
                if (dsType == "PDayOri")
                {
                    hdxData.Value = chartType;
                    dataView = m_DayAQIService.GetPointAQIDayInfo(pointIds, dtStart, dtEnd, qty, "OriData");

                    string data = "[";
                    for (int j = 0; j < pointIds.Length; j++)
                    {
                        string pointId = pointIds[j];
                        MonitoringPointEntity monitorPoint = monitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(pointId));
                        dataView.RowFilter = "PointId='" + pointIds[j] + "'";
                        data += "{";
                        data += string.Format(" name: '{0}',data:[ ", monitorPoint.MonitoringPointName);
                        int m = 0;
                        foreach (DataRowView drv in dataView)
                        {
                            m++;
                            //DateTime tstamp = Convert.ToDateTime(drv["DateTime1"]);
                            //string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                            if (m != dataView.Count)
                            {
                                if (drv["PM25_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM25_AQI"].ToString() + "],";
                                }
                                if (drv["PM10_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM10_AQI"].ToString() + "],";
                                }
                                if (drv["NO2_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["NO2_AQI"].ToString() + "],";
                                }
                                if (drv["SO2_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["SO2_AQI"].ToString() + "],";
                                }
                                if (drv["CO_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["CO_AQI"].ToString() + "],";
                                }
                                if (drv["O3_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["O3_AQI"].ToString() + "],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }
                            else
                            {
                                if (drv["PM25_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM25_AQI"].ToString() + "],";
                                }
                                if (drv["PM10_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM10_AQI"].ToString() + "],";
                                }
                                if (drv["NO2_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["NO2_AQI"].ToString() + "],";
                                }
                                if (drv["SO2_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["SO2_AQI"].ToString() + "],";
                                }
                                if (drv["CO_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["CO_AQI"].ToString() + "],";
                                }
                                if (drv["O3_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["O3_AQI"].ToString() + "],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }

                        }
                        data += "]},";
                    }
                    if (data != "")
                    {
                        data = data.Substring(0, data.Length - 1);
                    }
                    data += "]";
                    hdjsonData.Value = data;

                    RegisterScript("generate();");
                }
                #endregion
                #region 区域小时原始六参数分指数(IAQI)
                if (dsType == "RHourOri")
                {
                    dvRegion = GetRegionByPointId(pointIds);
                    foreach (DataRowView dr in dvRegion)
                    {
                        string regionUid = dr["RegionUid"].ToString();
                        listRegionUids.Add(regionUid);
                    }
                    string[] regionUids = listRegionUids.ToArray();
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
                    dtForAQI.Columns.Add("AQIValue", typeof(string));
                    dtForAQI.Columns.Add("PrimaryPollutant", typeof(string));
                    dtForAQI.Columns.Add("Grade", typeof(string));
                    dtForAQI.Columns.Add("Class", typeof(string));
                    dtForAQI.Columns.Add("RGBValue", typeof(string));

                    //int hourNum = Convert.ToInt32((dtmhourEnd.Subtract(dtmhourBegin)).TotalHours);
                    //for (int i = 0; i < hourNum; i++)
                    //{

                    List<string> regionName = dvRegion.ToTable().AsEnumerable().Select(t => t.Field<string>("Region")).ToList();
                    IEnumerable<string> names = regionName.Distinct();
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

                        DateTime hourBegion = dtStart;
                        DateTime hourEndion = dtEnd;
                        decimal? PM25PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34004", hourBegion, hourEndion, 1, "1");
                        decimal? PM10PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34002", hourBegion, hourEndion, 1, "1");
                        decimal? NO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21004", hourBegion, hourEndion, 1, "1");
                        decimal? SO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21026", hourBegion, hourEndion, 1, "1");
                        decimal? COPollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21005", hourBegion, hourEndion, 1, "1");
                        //decimal? O3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05024", hourBegion, hourEndion, 1, "1");
                        decimal? O3PollutantValue = 0;
                        if ((int)(hourEndion - hourBegion).TotalHours == 23 && hourBegion.ToString("yyyy-MM-dd") == hourEndion.ToString("yyyy-MM-dd"))
                        {
                            //求最大的可跨天的O3_NT
                            O3PollutantValue = m_DayAQIService.GetOriDataPagerO3ForNT(ids, hourBegion, hourEndion, "OriData");
                        }
                        else
                        {
                            O3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05024", Convert.ToDateTime(hourBegion.ToString("yyyy-MM-dd 00:00:00")), Convert.ToDateTime(hourEndion.ToString("yyyy-MM-dd 00:00:00")), 8, "1");
                        }

                        int? PM25Value = m_AQICalculateService.GetIAQI("a34004", Convert.ToDouble(PM25PollutantValue), 24);
                        int? PM10Value = m_AQICalculateService.GetIAQI("a34002", Convert.ToDouble(PM10PollutantValue), 24);
                        int? NO2Value = m_AQICalculateService.GetIAQI("a21004", Convert.ToDouble(NO2PollutantValue), 24);
                        int? SO2Value = m_AQICalculateService.GetIAQI("a21026", Convert.ToDouble(SO2PollutantValue), 24);
                        int? COValue = m_AQICalculateService.GetIAQI("a21005", Convert.ToDouble(COPollutantValue), 24);
                        int? O3Value = m_AQICalculateService.GetIAQI("a05024", Convert.ToDouble(O3PollutantValue), 8);
                        string AQIValue = m_AQICalculateService.GetAQI_Max_CNV(SO2Value, NO2Value, PM10Value, COValue, O3Value, PM25Value, "V");
                        string primaryPollutant = m_AQICalculateService.GetAQI_Max_CNV(SO2Value, NO2Value, PM10Value, COValue, O3Value, PM25Value, "N");
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
                        dr["O3"] = O3PollutantValue.ToString();
                        dr["O3_AQI"] = O3Value.ToString();
                        dr["AQIValue"] = AQIValue;
                        dr["PrimaryPollutant"] = primaryPollutant;
                        dr["Grade"] = grade;
                        dr["Class"] = class_AQI;
                        dr["RGBValue"] = color;
                        dtForAQI.Rows.Add(dr);

                    }
                    //}
                    dataView = dtForAQI.AsDataView();
                    string data = "[";
                    for (int j = 0; j < dtForAQI.Rows.Count; j++)
                    {
                        string pointId = dtForAQI.Rows[j][0].ToString();
                        dataView.RowFilter = "PointId='" + pointId + "'";
                        data += "{";
                        data += string.Format(" name: '{0}',data:[ ", pointId);
                        int m = 0;
                        foreach (DataRowView drv in dataView)
                        {
                            m++;
                            //DateTime tstamp = Convert.ToDateTime(drv["DateTime1"]);
                            //string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                            if (m != dataView.Count)
                            {
                                if (drv["PM25_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM25_AQI"].ToString() + "],";
                                }
                                if (drv["PM10_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM10_AQI"].ToString() + "],";
                                }
                                if (drv["NO2_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["NO2_AQI"].ToString() + "],";
                                }
                                if (drv["SO2_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["SO2_AQI"].ToString() + "],";
                                }
                                if (drv["CO_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["CO_AQI"].ToString() + "],";
                                }
                                if (drv["O3_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["O3_AQI"].ToString() + "],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }
                            else
                            {
                                if (drv["PM25_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM25_AQI"].ToString() + "],";
                                }
                                if (drv["PM10_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM10_AQI"].ToString() + "],";
                                }
                                if (drv["NO2_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["NO2_AQI"].ToString() + "],";
                                }
                                if (drv["SO2_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["SO2_AQI"].ToString() + "],";
                                }
                                if (drv["CO_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["CO_AQI"].ToString() + "],";
                                }
                                if (drv["O3_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["O3_AQI"].ToString() + "],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }

                        }
                        data += "]},";
                    }
                    if (data != "")
                    {
                        data = data.Substring(0, data.Length - 1);
                    }
                    data += "]";
                    hdjsonData.Value = data;

                    RegisterScript("generate();");
                }
                #endregion
                #region 区域日原始六参数分指数(IAQI)
                if (dsType == "RDayOri")
                {
                    dvRegion = GetRegionByPointId(pointIds);
                    foreach (DataRowView dr in dvRegion)
                    {
                        string regionUid = dr["RegionUid"].ToString();
                        listRegionUids.Add(regionUid);
                    }
                    string[] regionUids = listRegionUids.ToArray();
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
                    dtForAQI.Columns.Add("AQIValue", typeof(string));
                    dtForAQI.Columns.Add("PrimaryPollutant", typeof(string));
                    dtForAQI.Columns.Add("Grade", typeof(string));
                    dtForAQI.Columns.Add("Class", typeof(string));
                    dtForAQI.Columns.Add("RGBValue", typeof(string));

                    List<string> regionName = dvRegion.ToTable().AsEnumerable().Select(t => t.Field<string>("Region")).ToList();
                    IEnumerable<string> names = regionName.Distinct();
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
                        DateTime hourBegion = dtStart;
                        DateTime hourEndion = dtEnd;
                        decimal? PM25PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34004", hourBegion, hourEndion, 1, "1");
                        decimal? PM10PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34002", hourBegion, hourEndion, 1, "1");
                        decimal? NO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21004", hourBegion, hourEndion, 1, "1");
                        decimal? SO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21026", hourBegion, hourEndion, 1, "1");
                        decimal? COPollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21005", hourBegion, hourEndion, 1, "1");
                        decimal? O3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05024", hourBegion, hourEndion, 1, "1");
                        int? PM25Value = m_AQICalculateService.GetIAQI("a34004", Convert.ToDouble(PM25PollutantValue), 24);
                        int? PM10Value = m_AQICalculateService.GetIAQI("a34002", Convert.ToDouble(PM10PollutantValue), 24);
                        int? NO2Value = m_AQICalculateService.GetIAQI("a21004", Convert.ToDouble(NO2PollutantValue), 1);
                        int? SO2Value = m_AQICalculateService.GetIAQI("a21026", Convert.ToDouble(SO2PollutantValue), 1);
                        int? COValue = m_AQICalculateService.GetIAQI("a21005", Convert.ToDouble(COPollutantValue), 1);
                        int? O3Value = m_AQICalculateService.GetIAQI("a05024", Convert.ToDouble(O3PollutantValue), 1);
                        string AQIValue = m_AQICalculateService.GetAQI_Max_CNV(PM25Value, PM10Value, NO2Value, SO2Value, COValue, O3Value, "V");
                        string primaryPollutant = m_AQICalculateService.GetAQI_Max_CNV(PM25Value, PM10Value, NO2Value, SO2Value, COValue, O3Value, "N");
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
                        dr["O3"] = O3PollutantValue.ToString();
                        dr["O3_AQI"] = O3Value.ToString();
                        dr["AQIValue"] = AQIValue;
                        dr["PrimaryPollutant"] = primaryPollutant;
                        dr["Grade"] = grade;
                        dr["Class"] = class_AQI;
                        dr["RGBValue"] = color;
                        dtForAQI.Rows.Add(dr);

                    }
                    dataView = dtForAQI.AsDataView();
                    string data = "[";
                    for (int j = 0; j < dtForAQI.Rows.Count; j++)
                    {
                        string pointId = dtForAQI.Rows[j][0].ToString();
                        dataView.RowFilter = "PointId='" + pointId + "'";
                        data += "{";
                        data += string.Format(" name: '{0}',data:[ ", pointId);
                        int m = 0;
                        foreach (DataRowView drv in dataView)
                        {
                            m++;
                            //DateTime tstamp = Convert.ToDateTime(drv["DateTime1"]);
                            //string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                            if (m != dataView.Count)
                            {
                                if (drv["PM25_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM25_AQI"].ToString() + "],";
                                }
                                if (drv["PM10_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM10_AQI"].ToString() + "],";
                                }
                                if (drv["NO2_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["NO2_AQI"].ToString() + "],";
                                }
                                if (drv["SO2_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["SO2_AQI"].ToString() + "],";
                                }
                                if (drv["CO_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["CO_AQI"].ToString() + "],";
                                }
                                if (drv["O3_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["O3_AQI"].ToString() + "],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }
                            else
                            {
                                if (drv["PM25_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM25_AQI"].ToString() + "],";
                                }
                                if (drv["PM10_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["PM10_AQI"].ToString() + "],";
                                }
                                if (drv["NO2_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["NO2_AQI"].ToString() + "],";
                                }
                                if (drv["SO2_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["SO2_AQI"].ToString() + "],";
                                }
                                if (drv["CO_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["CO_AQI"].ToString() + "],";
                                }
                                if (drv["O3_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["O3_AQI"].ToString() + "],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }

                        }
                        data += "]},";
                    }
                    if (data != "")
                    {
                        data = data.Substring(0, data.Length - 1);
                    }
                    data += "]";
                    hdjsonData.Value = data;

                    RegisterScript("generate();");
                }
                #endregion
            }
            #endregion

            #region //空气质量指数(AQI)
            else if (chartContent == "primaryAQI")
            {
                MonitoringPointAirService monitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
                hdjsonTitle.Value = "空气质量指数(AQI)";
                #region 站点小时审核空气质量指数(AQI)
                if (dsType == "PHourAudit")
                {
                    hdxData.Value = chartType;
                    //if ((dtEnd - dtStart).Days <= 1)
                    //{
                    //    //dataView = m_DayAQIService.GetPointAQIHourInfo(pointIds, dtStart, dtEnd, qty, "AuditData");
                    //    dataView = m_DayAQIService.GetPointAQIHourInfo(pointIds, dtStart, dtEnd, qty, "AuditData");
                    //}
                    //else
                    //{
                    //    dataView = m_DayAQIService.GetPointAQIHourInfoOver23(pointIds, dtStart, dtEnd, qty, "AuditData");
                    //}     
                    //if ((int)(dtEnd - dtStart).TotalDays < 1)
                    //{
                    //    dataView = m_DayAQIService.GetPointAQIHourInfo(pointIds, dtStart, dtEnd.AddHours(1).AddSeconds(-1), qty, "Audit");
                    //}
                    //if ((int)(dtEnd - dtStart).TotalDays >= 1)
                    //{
                    //    dataView = m_DayAQIService.GetPointAQIHourInfoOver23(pointIds.ToArray(), dtStart, dtEnd.AddHours(1).AddSeconds(-1), qty, "Audit");
                    //}
                    if ((int)(dtEnd - dtStart).TotalHours == 23 && dtStart.ToString("yyyy-MM-dd") == dtEnd.ToString("yyyy-MM-dd"))
                    {
                        //dataView = m_DayAQIService.GetPointAQIHourInfo(pointIds, dtStart, dtEnd.AddHours(1).AddSeconds(-1), qty, "Audit");
                        dataView = m_DayAQIService.GetPointAQIHourInfoOver23(pointIds.ToArray(), dtStart, dtEnd.AddHours(1).AddSeconds(-1), qty, "Audit");
                    }
                    else
                    {
                        //dataView = m_DayAQIService.GetPointAQIHourInfoOver23(pointIds.ToArray(), dtStart, dtEnd.AddHours(1).AddSeconds(-1), qty, "Audit");
                        dataView = m_DayAQIService.GetPointAQIHourInfo(pointIds, dtStart, dtEnd.AddHours(1).AddSeconds(-1), qty, "Audit");
                    }
                    string data = "[";
                    for (int j = 0; j < pointIds.Length; j++)
                    {
                        string pointId = pointIds[j];
                        MonitoringPointEntity monitorPoint = monitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(pointId));
                        dataView.RowFilter = "PointId='" + pointIds[j] + "'";
                        data += "{";
                        data += string.Format(" name: '{0}',data:[ ", monitorPoint.MonitoringPointName);
                        int m = 0;
                        foreach (DataRowView drv in dataView)
                        {
                            m++;
                            //DateTime tstamp = Convert.ToDateTime(drv["DateTime1"]);
                            //string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                            if (m != dataView.Count)
                            {
                                if (drv["Max_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["Max_AQI"].ToString() + "],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }
                            else
                            {
                                if (drv["Max_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["Max_AQI"].ToString() + "],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }

                        }
                        data += "]},";
                    }
                    if (data != "")
                    {
                        data = data.Substring(0, data.Length - 1);
                    }
                    data += "]";
                    hdjsonData.Value = data;
                }
                #endregion
                #region 站点日审核空气质量指数(AQI)
                else if (dsType == "PDayAudit")
                {
                    hdxData.Value = chartType;
                    dataView = m_DayAQIService.GetPointAQIDayInfo(pointIds, dtStart, dtEnd, qty, "AuditData");
                    string data = "[";
                    for (int j = 0; j < pointIds.Length; j++)
                    {
                        string pointId = pointIds[j];
                        MonitoringPointEntity monitorPoint = monitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(pointId));
                        dataView.RowFilter = "PointId='" + pointIds[j] + "'";
                        data += "{";
                        data += string.Format(" name: '{0}',data:[ ", monitorPoint.MonitoringPointName);
                        int m = 0;
                        foreach (DataRowView drv in dataView)
                        {
                            m++;
                            //DateTime tstamp = Convert.ToDateTime(drv["DateTime1"]);
                            //string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                            if (m != dataView.Count)
                            {
                                if (drv["Max_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["Max_AQI"].ToString() + "],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }
                            else
                            {
                                if (drv["Max_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["Max_AQI"].ToString() + "],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }
                        }
                        data += "]},";
                    }
                    if (data != "")
                    {
                        data = data.Substring(0, data.Length - 1);
                    }
                    data += "]";
                    hdjsonData.Value = data;
                }
                #endregion
                #region 区域小时审核空气质量指数(AQI)
                else if (dsType == "RHourAudit")
                {
                    dvRegion = GetRegionByPointId(pointIds);
                    foreach (DataRowView dr in dvRegion)
                    {
                        string regionUid = dr["RegionUid"].ToString();
                        listRegionUids.Add(regionUid);
                    }
                    string[] regionUids = listRegionUids.ToArray();
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
                    dtForAQI.Columns.Add("AQIValue", typeof(string));
                    dtForAQI.Columns.Add("PrimaryPollutant", typeof(string));
                    dtForAQI.Columns.Add("Grade", typeof(string));
                    dtForAQI.Columns.Add("Class", typeof(string));
                    dtForAQI.Columns.Add("RGBValue", typeof(string));

                    //int hourNum = Convert.ToInt32((dtmhourEnd.Subtract(dtmhourBegin)).TotalHours);
                    //for (int i = 0; i < hourNum; i++)
                    //{

                    List<string> regionName = dvRegion.ToTable().AsEnumerable().Select(t => t.Field<string>("Region")).ToList();
                    IEnumerable<string> names = regionName.Distinct();
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

                        DateTime hourBegion = dtStart;
                        DateTime hourEndion = dtEnd;
                        decimal? PM25PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34004", hourBegion, hourEndion, 1, "2");
                        decimal? PM10PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34002", hourBegion, hourEndion, 1, "2");
                        decimal? NO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21004", hourBegion, hourEndion, 1, "2");
                        decimal? SO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21026", hourBegion, hourEndion, 1, "2");
                        decimal? COPollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21005", hourBegion, hourEndion, 1, "2");
                        //decimal? O3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05024", hourBegion, hourEndion, 1, "2");
                        decimal? O3PollutantValue = 0;
                        if ((int)(hourEndion - hourBegion).TotalHours == 23 && hourBegion.ToString("yyyy-MM-dd") == hourEndion.ToString("yyyy-MM-dd"))
                        {
                            //求最大的可跨天的O3_NT
                            O3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05024", hourBegion, hourEndion, 1, "2");
                        }
                        else
                        {
                            O3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05027", hourBegion, hourEndion, 1, "2");
                        }

                        int? PM25Value = m_AQICalculateService.GetIAQI("a34004", Convert.ToDouble(PM25PollutantValue), 24);
                        int? PM10Value = m_AQICalculateService.GetIAQI("a34002", Convert.ToDouble(PM10PollutantValue), 24);
                        int? NO2Value = m_AQICalculateService.GetIAQI("a21004", Convert.ToDouble(NO2PollutantValue), 24);
                        int? SO2Value = m_AQICalculateService.GetIAQI("a21026", Convert.ToDouble(SO2PollutantValue), 24);
                        int? COValue = m_AQICalculateService.GetIAQI("a21005", Convert.ToDouble(COPollutantValue), 24);
                        int? O3Value = m_AQICalculateService.GetIAQI("a05024", Convert.ToDouble(O3PollutantValue), 8);
                        string AQIValue = m_AQICalculateService.GetAQI_Max_CNV(PM25Value, PM10Value, NO2Value, SO2Value, COValue, O3Value, "V");
                        string primaryPollutant = m_AQICalculateService.GetAQI_Max_CNV(PM25Value, PM10Value, NO2Value, SO2Value, COValue, O3Value, "N");
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
                        dr["O3"] = O3PollutantValue.ToString();
                        dr["O3_AQI"] = O3Value.ToString();
                        dr["AQIValue"] = AQIValue;
                        dr["PrimaryPollutant"] = primaryPollutant;
                        dr["Grade"] = grade;
                        dr["Class"] = class_AQI;
                        dr["RGBValue"] = color;
                        dtForAQI.Rows.Add(dr);

                    }
                    //}
                    dataView = dtForAQI.AsDataView();
                    string data = "[";
                    for (int j = 0; j < dtForAQI.Rows.Count; j++)
                    {
                        string pointId = dtForAQI.Rows[j][0].ToString();
                        dataView.RowFilter = "PointId='" + pointId + "'";
                        data += "{";
                        data += string.Format(" name: '{0}',data:[ ", pointId);
                        int m = 0;
                        foreach (DataRowView drv in dataView)
                        {
                            m++;
                            //DateTime tstamp = Convert.ToDateTime(drv["DateTime1"]);
                            //string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                            if (m != dataView.Count)
                            {
                                if (drv["AQIValue"] != DBNull.Value)
                                {
                                    data += "[" + drv["AQIValue"].ToString() + "],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }
                            else
                            {
                                if (drv["AQIValue"] != DBNull.Value)
                                {
                                    data += "[" + drv["AQIValue"].ToString() + "],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }

                        }
                        data += "]},";
                    }
                    if (data != "")
                    {
                        data = data.Substring(0, data.Length - 1);
                    }
                    data += "]";
                    hdjsonData.Value = data;

                    RegisterScript("generate();");
                }
                #endregion
                #region 区域日审核空气质量指数(AQI)
                else if (dsType == "RDayAudit")
                {
                    dvRegion = GetRegionByPointId(pointIds);
                    foreach (DataRowView dr in dvRegion)
                    {
                        string regionUid = dr["RegionUid"].ToString();
                        listRegionUids.Add(regionUid);
                    }
                    string[] regionUids = listRegionUids.ToArray();
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
                    dtForAQI.Columns.Add("AQIValue", typeof(string));
                    dtForAQI.Columns.Add("PrimaryPollutant", typeof(string));
                    dtForAQI.Columns.Add("Grade", typeof(string));
                    dtForAQI.Columns.Add("Class", typeof(string));
                    dtForAQI.Columns.Add("RGBValue", typeof(string));

                    List<string> regionName = dvRegion.ToTable().AsEnumerable().Select(t => t.Field<string>("Region")).ToList();
                    IEnumerable<string> names = regionName.Distinct();
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
                        DateTime hourBegion = dtStart;
                        DateTime hourEndion = dtEnd;
                        decimal? PM25PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34004", hourBegion, hourEndion, 1, "2");
                        decimal? PM10PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34002", hourBegion, hourEndion, 1, "2");
                        decimal? NO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21004", hourBegion, hourEndion, 1, "2");
                        decimal? SO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21026", hourBegion, hourEndion, 1, "2");
                        decimal? COPollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21005", hourBegion, hourEndion, 1, "2");
                        decimal? O3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05024", hourBegion, hourEndion, 1, "2");
                        int? PM25Value = m_AQICalculateService.GetIAQI("a34004", Convert.ToDouble(PM25PollutantValue), 24);
                        int? PM10Value = m_AQICalculateService.GetIAQI("a34002", Convert.ToDouble(PM10PollutantValue), 24);
                        int? NO2Value = m_AQICalculateService.GetIAQI("a21004", Convert.ToDouble(NO2PollutantValue), 1);
                        int? SO2Value = m_AQICalculateService.GetIAQI("a21026", Convert.ToDouble(SO2PollutantValue), 1);
                        int? COValue = m_AQICalculateService.GetIAQI("a21005", Convert.ToDouble(COPollutantValue), 1);
                        int? O3Value = m_AQICalculateService.GetIAQI("a05024", Convert.ToDouble(O3PollutantValue), 1);
                        string AQIValue = m_AQICalculateService.GetAQI_Max_CNV(PM25Value, PM10Value, NO2Value, SO2Value, COValue, O3Value, "V");
                        string primaryPollutant = m_AQICalculateService.GetAQI_Max_CNV(PM25Value, PM10Value, NO2Value, SO2Value, COValue, O3Value, "N");
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
                        dr["O3"] = O3PollutantValue.ToString();
                        dr["O3_AQI"] = O3Value.ToString();
                        dr["AQIValue"] = AQIValue;
                        dr["PrimaryPollutant"] = primaryPollutant;
                        dr["Grade"] = grade;
                        dr["Class"] = class_AQI;
                        dr["RGBValue"] = color;
                        dtForAQI.Rows.Add(dr);

                    }
                    dataView = dtForAQI.AsDataView();
                    string data = "[";
                    for (int j = 0; j < dtForAQI.Rows.Count; j++)
                    {
                        string pointId = dtForAQI.Rows[j][0].ToString();
                        dataView.RowFilter = "PointId='" + pointId + "'";
                        data += "{";
                        data += string.Format(" name: '{0}',data:[ ", pointId);
                        int m = 0;
                        foreach (DataRowView drv in dataView)
                        {
                            m++;
                            //DateTime tstamp = Convert.ToDateTime(drv["DateTime1"]);
                            //string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                            if (m != dataView.Count)
                            {
                                if (drv["AQIValue"] != DBNull.Value)
                                {
                                    data += "[" + drv["AQIValue"].ToString() + "],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }
                            else
                            {
                                if (drv["AQIValue"] != DBNull.Value)
                                {
                                    data += "[" + drv["AQIValue"].ToString() + "],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }

                        }
                        data += "]},";
                    }
                    if (data != "")
                    {
                        data = data.Substring(0, data.Length - 1);
                    }
                    data += "]";
                    hdjsonData.Value = data;

                    RegisterScript("generate();");
                }
                #endregion
                #region 站点小时原始空气质量指数(AQI)
                else if (dsType == "PHourOri")
                {
                    hdxData.Value = chartType;
                    //if ((dtEnd - dtStart).Days <= 1)
                    //{
                    //    dataView = m_DayAQIService.GetPointAQIHourInfo(pointIds, dtStart, dtEnd, qty, "OriData");
                    //}
                    //else
                    //{
                    //    dataView = m_DayAQIService.GetPointAQIHourInfoOver23(pointIds, dtStart, dtEnd, qty, "OriData");
                    //}     
                    if ((int)(dtEnd - dtStart).TotalHours == 23 && dtStart.ToString("yyyy-MM-dd") == dtEnd.ToString("yyyy-MM-dd"))
                    {
                        //dataView = m_DayAQIService.GetPointAQIHourInfo(pointIds, dtStart, dtEnd.AddHours(1).AddSeconds(-1), qty, "OriData");
                        dataView = m_DayAQIService.GetPointAQIHourInfoOver23(pointIds.ToArray(), dtStart, dtEnd.AddHours(1).AddSeconds(-1), qty, "OriData");
                    }
                    else
                    {
                        //dataView = m_DayAQIService.GetPointAQIHourInfoOver23(pointIds.ToArray(), dtStart, dtEnd.AddHours(1).AddSeconds(-1), qty, "OriData");
                        dataView = m_DayAQIService.GetPointAQIHourInfo(pointIds, dtStart, dtEnd.AddHours(1).AddSeconds(-1), qty, "OriData");
                    }
                    string data = "[";
                    for (int j = 0; j < pointIds.Length; j++)
                    {
                        string pointId = pointIds[j];
                        MonitoringPointEntity monitorPoint = monitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(pointId));
                        dataView.RowFilter = "PointId='" + pointIds[j] + "'";
                        data += "{";
                        data += string.Format(" name: '{0}',data:[ ", monitorPoint.MonitoringPointName);
                        int m = 0;
                        foreach (DataRowView drv in dataView)
                        {
                            m++;
                            //DateTime tstamp = Convert.ToDateTime(drv["DateTime1"]);
                            //string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                            if (m != dataView.Count)
                            {
                                if (drv["Max_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["Max_AQI"].ToString() + "],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }
                            else
                            {
                                if (drv["Max_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["Max_AQI"].ToString() + "],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }

                        }
                        data += "]},";
                    }
                    if (data != "")
                    {
                        data = data.Substring(0, data.Length - 1);
                    }
                    data += "]";
                    hdjsonData.Value = data;
                }
                #endregion
                #region 站点日原始空气质量指数(AQI)
                else if (dsType == "PDayOri")
                {
                    hdxData.Value = chartType;
                    dataView = m_DayAQIService.GetPointAQIDayInfo(pointIds, dtStart, dtEnd, qty, "OriData");
                    string data = "[";
                    for (int j = 0; j < pointIds.Length; j++)
                    {
                        string pointId = pointIds[j];
                        MonitoringPointEntity monitorPoint = monitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(pointId));
                        dataView.RowFilter = "PointId='" + pointIds[j] + "'";
                        data += "{";
                        data += string.Format(" name: '{0}',data:[ ", monitorPoint.MonitoringPointName);
                        int m = 0;
                        foreach (DataRowView drv in dataView)
                        {
                            m++;
                            //DateTime tstamp = Convert.ToDateTime(drv["DateTime1"]);
                            //string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                            if (m != dataView.Count)
                            {
                                if (drv["Max_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["Max_AQI"].ToString() + "],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }
                            else
                            {
                                if (drv["Max_AQI"] != DBNull.Value)
                                {
                                    data += "[" + drv["Max_AQI"].ToString() + "],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }

                        }
                        data += "]},";
                    }
                    if (data != "")
                    {
                        data = data.Substring(0, data.Length - 1);
                    }
                    data += "]";
                    hdjsonData.Value = data;
                }
                #endregion
                #region 区域小时原始空气质量指数(AQI)
                else if (dsType == "RHourOri")
                {
                    dvRegion = GetRegionByPointId(pointIds);
                    foreach (DataRowView dr in dvRegion)
                    {
                        string regionUid = dr["RegionUid"].ToString();
                        listRegionUids.Add(regionUid);
                    }
                    string[] regionUids = listRegionUids.ToArray();
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
                    dtForAQI.Columns.Add("AQIValue", typeof(string));
                    dtForAQI.Columns.Add("PrimaryPollutant", typeof(string));
                    dtForAQI.Columns.Add("Grade", typeof(string));
                    dtForAQI.Columns.Add("Class", typeof(string));
                    dtForAQI.Columns.Add("RGBValue", typeof(string));

                    //int hourNum = Convert.ToInt32((dtmhourEnd.Subtract(dtmhourBegin)).TotalHours);
                    //for (int i = 0; i < hourNum; i++)
                    //{

                    List<string> regionName = dvRegion.ToTable().AsEnumerable().Select(t => t.Field<string>("Region")).ToList();
                    IEnumerable<string> names = regionName.Distinct();
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

                        DateTime hourBegion = dtStart;
                        DateTime hourEndion = dtEnd;
                        decimal? PM25PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34004", hourBegion, hourEndion, 1, "1");
                        decimal? PM10PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34002", hourBegion, hourEndion, 1, "1");
                        decimal? NO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21004", hourBegion, hourEndion, 1, "1");
                        decimal? SO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21026", hourBegion, hourEndion, 1, "1");
                        decimal? COPollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21005", hourBegion, hourEndion, 1, "1");
                        //decimal? O3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05024", hourBegion, hourEndion, 1, "1");
                        decimal? O3PollutantValue = 0;
                        if ((int)(hourEndion - hourBegion).TotalHours == 23 && hourBegion.ToString("yyyy-MM-dd") == hourEndion.ToString("yyyy-MM-dd"))
                        {
                            //求最大的可跨天的O3_NT
                            O3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05024", hourBegion, hourEndion, 1, "1");
                        }
                        else
                        {
                            O3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05027", hourBegion, hourEndion, 1, "1");
                        }

                        int? PM25Value = m_AQICalculateService.GetIAQI("a34004", Convert.ToDouble(PM25PollutantValue), 24);
                        int? PM10Value = m_AQICalculateService.GetIAQI("a34002", Convert.ToDouble(PM10PollutantValue), 24);
                        int? NO2Value = m_AQICalculateService.GetIAQI("a21004", Convert.ToDouble(NO2PollutantValue), 24);
                        int? SO2Value = m_AQICalculateService.GetIAQI("a21026", Convert.ToDouble(SO2PollutantValue), 24);
                        int? COValue = m_AQICalculateService.GetIAQI("a21005", Convert.ToDouble(COPollutantValue), 24);
                        int? O3Value = m_AQICalculateService.GetIAQI("a05024", Convert.ToDouble(O3PollutantValue), 8);
                        string AQIValue = m_AQICalculateService.GetAQI_Max_CNV(PM25Value, PM10Value, NO2Value, SO2Value, COValue, O3Value, "V");
                        string primaryPollutant = m_AQICalculateService.GetAQI_Max_CNV(PM25Value, PM10Value, NO2Value, SO2Value, COValue, O3Value, "N");
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
                        dr["O3"] = O3PollutantValue.ToString();
                        dr["O3_AQI"] = O3Value.ToString();
                        dr["AQIValue"] = AQIValue;
                        dr["PrimaryPollutant"] = primaryPollutant;
                        dr["Grade"] = grade;
                        dr["Class"] = class_AQI;
                        dr["RGBValue"] = color;
                        dtForAQI.Rows.Add(dr);

                    }
                    //}
                    dataView = dtForAQI.AsDataView();
                    string data = "[";
                    for (int j = 0; j < dtForAQI.Rows.Count; j++)
                    {
                        string pointId = dtForAQI.Rows[j][0].ToString();
                        dataView.RowFilter = "PointId='" + pointId + "'";
                        data += "{";
                        data += string.Format(" name: '{0}',data:[ ", pointId);
                        int m = 0;
                        foreach (DataRowView drv in dataView)
                        {
                            m++;
                            //DateTime tstamp = Convert.ToDateTime(drv["DateTime1"]);
                            //string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                            if (m != dataView.Count)
                            {
                                if (drv["AQIValue"] != DBNull.Value)
                                {
                                    data += "[" + drv["AQIValue"].ToString() + "],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }
                            else
                            {
                                if (drv["AQIValue"] != DBNull.Value)
                                {
                                    data += "[" + drv["AQIValue"].ToString() + "],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }

                        }
                        data += "]},";
                    }
                    if (data != "")
                    {
                        data = data.Substring(0, data.Length - 1);
                    }
                    data += "]";
                    hdjsonData.Value = data;

                    RegisterScript("generate();");
                }
                #endregion
                #region 区域日原始空气质量指数(AQI)
                else if (dsType == "RDayOri")
                {
                    dvRegion = GetRegionByPointId(pointIds);
                    foreach (DataRowView dr in dvRegion)
                    {
                        string regionUid = dr["RegionUid"].ToString();
                        listRegionUids.Add(regionUid);
                    }
                    string[] regionUids = listRegionUids.ToArray();
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
                    dtForAQI.Columns.Add("AQIValue", typeof(string));
                    dtForAQI.Columns.Add("PrimaryPollutant", typeof(string));
                    dtForAQI.Columns.Add("Grade", typeof(string));
                    dtForAQI.Columns.Add("Class", typeof(string));
                    dtForAQI.Columns.Add("RGBValue", typeof(string));

                    List<string> regionName = dvRegion.ToTable().AsEnumerable().Select(t => t.Field<string>("Region")).ToList();
                    IEnumerable<string> names = regionName.Distinct();
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
                        DateTime hourBegion = dtStart;
                        DateTime hourEndion = dtEnd;
                        decimal? PM25PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34004", hourBegion, hourEndion, 1, "1");
                        decimal? PM10PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34002", hourBegion, hourEndion, 1, "1");
                        decimal? NO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21004", hourBegion, hourEndion, 1, "1");
                        decimal? SO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21026", hourBegion, hourEndion, 1, "1");
                        decimal? COPollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21005", hourBegion, hourEndion, 1, "1");
                        decimal? O3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05024", hourBegion, hourEndion, 1, "1");
                        int? PM25Value = m_AQICalculateService.GetIAQI("a34004", Convert.ToDouble(PM25PollutantValue), 24);
                        int? PM10Value = m_AQICalculateService.GetIAQI("a34002", Convert.ToDouble(PM10PollutantValue), 24);
                        int? NO2Value = m_AQICalculateService.GetIAQI("a21004", Convert.ToDouble(NO2PollutantValue), 1);
                        int? SO2Value = m_AQICalculateService.GetIAQI("a21026", Convert.ToDouble(SO2PollutantValue), 1);
                        int? COValue = m_AQICalculateService.GetIAQI("a21005", Convert.ToDouble(COPollutantValue), 1);
                        int? O3Value = m_AQICalculateService.GetIAQI("a05024", Convert.ToDouble(O3PollutantValue), 1);
                        string AQIValue = m_AQICalculateService.GetAQI_Max_CNV(PM25Value, PM10Value, NO2Value, SO2Value, COValue, O3Value, "V");
                        string primaryPollutant = m_AQICalculateService.GetAQI_Max_CNV(PM25Value, PM10Value, NO2Value, SO2Value, COValue, O3Value, "N");
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
                        dr["O3"] = O3PollutantValue.ToString();
                        dr["O3_AQI"] = O3Value.ToString();
                        dr["AQIValue"] = AQIValue;
                        dr["PrimaryPollutant"] = primaryPollutant;
                        dr["Grade"] = grade;
                        dr["Class"] = class_AQI;
                        dr["RGBValue"] = color;
                        dtForAQI.Rows.Add(dr);

                    }
                    dataView = dtForAQI.AsDataView();
                    string data = "[";
                    for (int j = 0; j < dtForAQI.Rows.Count; j++)
                    {
                        string pointId = dtForAQI.Rows[j][0].ToString();
                        dataView.RowFilter = "PointId='" + pointId + "'";
                        data += "{";
                        data += string.Format(" name: '{0}',data:[ ", pointId);
                        int m = 0;
                        foreach (DataRowView drv in dataView)
                        {
                            m++;
                            //DateTime tstamp = Convert.ToDateTime(drv["DateTime1"]);
                            //string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                            if (m != dataView.Count)
                            {
                                if (drv["AQIValue"] != DBNull.Value)
                                {
                                    data += "[" + drv["AQIValue"].ToString() + "],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }
                            else
                            {
                                if (drv["AQIValue"] != DBNull.Value)
                                {
                                    data += "[" + drv["AQIValue"].ToString() + "],";
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }
                        }
                        data += "]},";
                    }
                    if (data != "")
                    {
                        data = data.Substring(0, data.Length - 1);
                    }
                    data += "]";
                    hdjsonData.Value = data;

                    RegisterScript("generate();");
                }
                #endregion
            }
            #endregion

            #region //首要污染物浓度值
            else if (chartContent == "primaryValue")
            {
                hdjsonTitle.Value = "首要污染物浓度值(mg/m3)";
                #region 站点小时审核首要污染物浓度值
                if (dsType == "PHourAudit")
                {
                    MonitoringPointAirService monitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
                    hdxData.Value = chartType;
                    //if ((dtEnd - dtStart).Days <= 1)
                    //{
                    //    dataView = m_DayAQIService.GetPointAQIHourInfo(pointIds, dtStart, dtEnd, qty, "AuditData");
                    //}
                    //else
                    //{
                    //    dataView = m_DayAQIService.GetPointAQIHourInfoOver23(pointIds, dtStart, dtEnd, qty, "AuditData");
                    //}     
                    if ((int)(dtEnd - dtStart).TotalHours == 23 && dtStart.ToString("yyyy-MM-dd") == dtEnd.ToString("yyyy-MM-dd"))
                    {
                        //dataView = m_DayAQIService.GetPointAQIHourInfo(points.Select(t => t.PointID).ToArray(), dtmBegion, dtmEnd.AddHours(1).AddSeconds(-1), qulityType, dataType);
                        dataView = m_DayAQIService.GetPointAQIHourInfoOver23(pointIds, dtStart, dtEnd.AddHours(1).AddSeconds(-1), qty, "Audit");
                    }
                    //if ((int)(dtmEnd - dtmBegion).TotalDays >= 1)
                    else
                    {
                        //dataView = m_DayAQIService.GetPointAQIHourInfoOver23(points.Select(t => t.PointID).ToArray(), dtmBegion, dtmEnd.AddHours(1).AddSeconds(-1), qulityType, dataType);
                        dataView = m_DayAQIService.GetPointAQIHourInfo(pointIds, dtStart, dtEnd.AddHours(1).AddSeconds(-1), qty, "Audit");
                    }
                    
                    string data = "[";
                    for (int j = 0; j < pointIds.Length; j++)
                    {
                        string pointId = pointIds[j];
                        MonitoringPointEntity monitorPoint = monitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(pointId));
                        dataView.RowFilter = "PointId='" + pointIds[j] + "'";
                        data += "{";
                        data += string.Format(" name: '{0}{1}',data:[ ", monitorPoint.MonitoringPointName, dataView.ToTable().Rows[0]["PrimaryPollutant"].ToString());
                        int m = 0;
                        foreach (DataRowView drv in dataView)
                        {
                            m++;
                            //DateTime tstamp = Convert.ToDateTime(drv["DateTime1"]);
                            //string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                            if (m != dataView.Count)
                            {
                                if (drv["PrimaryPollutant"] != DBNull.Value)
                                {
                                    if (drv["PrimaryPollutant"].ToString() == "PM2.5")
                                    {
                                        data += "[" + Convert.ToDecimal(drv["PM25"].ToString())*1000 + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "PM10")
                                    {
                                        data += "[" + Convert.ToDecimal(drv["PM10"].ToString())*1000 + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "SO2")
                                    {
                                        data += "[" + Convert.ToDecimal(drv["SO2"].ToString())*1000 + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "NO2")
                                    {
                                        data += "[" + Convert.ToDecimal(drv["NO2"].ToString())*1000 + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "CO")
                                    {
                                        data += "[" + drv["CO"].ToString() + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "O3")
                                    {
                                        data += "[" + Convert.ToDecimal(drv["O3"].ToString())*1000 + "],";
                                    }

                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }
                            else
                            {
                                if (drv["PrimaryPollutant"] != DBNull.Value)
                                {
                                    if (drv["PrimaryPollutant"].ToString() == "PM2.5")
                                    {
                                        data += "[" + Convert.ToDecimal(drv["PM25"].ToString()) * 1000 + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "PM10")
                                    {
                                        data += "[" + Convert.ToDecimal(drv["PM10"].ToString()) * 1000 + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "SO2")
                                    {
                                        data += "[" + Convert.ToDecimal(drv["SO2"].ToString()) * 1000 + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "NO2")
                                    {
                                        data += "[" + Convert.ToDecimal(drv["NO2"].ToString()) * 1000 + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "CO")
                                    {
                                        data += "[" + drv["CO"].ToString() + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "O3")
                                    {
                                        data += "[" + Convert.ToDecimal(drv["O3"].ToString()) * 1000 + "],";
                                    }
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }

                        }
                        data += "]},";
                    }
                    if (data != "")
                    {
                        data = data.Substring(0, data.Length - 1);
                    }
                    data += "]";
                    hdjsonData.Value = data;
                }
                #endregion
                #region 站点日审核首要污染物浓度值
                else if (dsType == "PDayAudit")
                {
                    MonitoringPointAirService monitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
                    hdxData.Value = chartType;
                    dataView = m_DayAQIService.GetPointAQIDayInfo(pointIds, dtStart, dtEnd, qty, "AuditData");
                    string data = "[";
                    for (int j = 0; j < pointIds.Length; j++)
                    {
                        string pointId = pointIds[j];
                        MonitoringPointEntity monitorPoint = monitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(pointId));
                        dataView.RowFilter = "PointId='" + pointIds[j] + "'";
                        data += "{";
                        data += string.Format(" name: '{0}',data:[ ", monitorPoint.MonitoringPointName);
                        int m = 0;
                        foreach (DataRowView drv in dataView)
                        {
                            m++;
                            //DateTime tstamp = Convert.ToDateTime(drv["DateTime1"]);
                            //string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                            if (m != dataView.Count)
                            {
                                if (drv["PrimaryPollutant"] != DBNull.Value)
                                {
                                    if (drv["PrimaryPollutant"].ToString() == "PM2.5")
                                    {
                                        data += "[" + drv["PM25_AQI"].ToString() + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "PM10")
                                    {
                                        data += "[" + drv["PM10_AQI"].ToString() + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "SO2")
                                    {
                                        data += "[" + drv["SO2_AQI"].ToString() + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "NO2")
                                    {
                                        data += "[" + drv["NO2_AQI"].ToString() + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "CO")
                                    {
                                        data += "[" + drv["CO_AQI"].ToString() + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "O3")
                                    {
                                        data += "[" + drv["O3_AQI"].ToString() + "],";
                                    }

                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }
                            else
                            {
                                if (drv["PrimaryPollutant"] != DBNull.Value)
                                {
                                    if (drv["PrimaryPollutant"].ToString() == "PM2.5")
                                    {
                                        data += "[" + drv["PM25_AQI"].ToString() + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "PM10")
                                    {
                                        data += "[" + drv["PM10_AQI"].ToString() + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "SO2")
                                    {
                                        data += "[" + drv["SO2_AQI"].ToString() + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "NO2")
                                    {
                                        data += "[" + drv["NO2_AQI"].ToString() + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "CO")
                                    {
                                        data += "[" + drv["CO_AQI"].ToString() + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "O3")
                                    {
                                        data += "[" + drv["O3_AQI"].ToString() + "],";
                                    }
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }

                        }
                        data += "]},";
                    }
                    if (data != "")
                    {
                        data = data.Substring(0, data.Length - 1);
                    }
                    data += "]";
                    hdjsonData.Value = data;
                }
                #endregion
                #region 区域小时审核首要污染物浓度值
                else if (dsType == "RHourAudit")
                {
                    MonitoringPointAirService monitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();

                    dvRegion = GetRegionByPointId(pointIds);
                    foreach (DataRowView dr in dvRegion)
                    {
                        string regionUid = dr["RegionUid"].ToString();
                        listRegionUids.Add(regionUid);
                    }
                    string[] regionUids = listRegionUids.ToArray();
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
                    dtForAQI.Columns.Add("AQIValue", typeof(string));
                    dtForAQI.Columns.Add("PrimaryPollutant", typeof(string));
                    dtForAQI.Columns.Add("Grade", typeof(string));
                    dtForAQI.Columns.Add("Class", typeof(string));
                    dtForAQI.Columns.Add("RGBValue", typeof(string));

                    //int hourNum = Convert.ToInt32((dtmhourEnd.Subtract(dtmhourBegin)).TotalHours);
                    //for (int i = 0; i < hourNum; i++)
                    //{

                    List<string> regionName = dvRegion.ToTable().AsEnumerable().Select(t => t.Field<string>("Region")).ToList();
                    IEnumerable<string> names = regionName.Distinct();
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

                        DateTime hourBegion = dtStart;
                        DateTime hourEndion = dtEnd;
                        decimal? PM25PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34004", hourBegion, hourEndion, 1, "2");
                        decimal? PM10PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34002", hourBegion, hourEndion, 1, "2");
                        decimal? NO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21004", hourBegion, hourEndion, 1, "2");
                        decimal? SO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21026", hourBegion, hourEndion, 1, "2");
                        decimal? COPollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21005", hourBegion, hourEndion, 1, "2");
                        //decimal? O3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05024", hourBegion, hourEndion, 1, "2");
                        decimal? O3PollutantValue = 0;
                        if ((int)(hourEndion - hourBegion).TotalHours == 23 && hourBegion.ToString("yyyy-MM-dd") == hourEndion.ToString("yyyy-MM-dd"))
                        {
                            //求最大的可跨天的O3_NT
                            O3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05024", hourBegion, hourEndion, 1, "2");
                        }
                        else
                        {
                            O3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05027", hourBegion, hourEndion, 1, "2");
                        }
                        int? PM25Value = m_AQICalculateService.GetIAQI("a34004", Convert.ToDouble(PM25PollutantValue), 24);
                        int? PM10Value = m_AQICalculateService.GetIAQI("a34002", Convert.ToDouble(PM10PollutantValue), 24);
                        int? NO2Value = m_AQICalculateService.GetIAQI("a21004", Convert.ToDouble(NO2PollutantValue), 24);
                        int? SO2Value = m_AQICalculateService.GetIAQI("a21026", Convert.ToDouble(SO2PollutantValue), 24);
                        int? COValue = m_AQICalculateService.GetIAQI("a21005", Convert.ToDouble(COPollutantValue), 24);
                        int? O3Value = m_AQICalculateService.GetIAQI("a05024", Convert.ToDouble(O3PollutantValue), 8);
                        string AQIValue = m_AQICalculateService.GetAQI_Max_CNV(SO2Value, NO2Value, PM10Value, COValue, O3Value, PM25Value, "V");
                        string primaryPollutant = m_AQICalculateService.GetAQI_Max_CNV(SO2Value, NO2Value, PM10Value, COValue, O3Value, PM25Value, "N");
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
                        dr["O3"] = O3PollutantValue.ToString();
                        dr["O3_AQI"] = O3Value.ToString();
                        dr["AQIValue"] = AQIValue;
                        dr["PrimaryPollutant"] = primaryPollutant;
                        dr["Grade"] = grade;
                        dr["Class"] = class_AQI;
                        dr["RGBValue"] = color;
                        dtForAQI.Rows.Add(dr);

                    }
                    //}
                    dataView = dtForAQI.AsDataView();
                    string data = "[";
                    for (int j = 0; j < dtForAQI.Rows.Count; j++)
                    {
                        string pointId = dtForAQI.Rows[j][0].ToString();
                        dataView.RowFilter = "PointId='" + pointId + "'";
                        data += "{";
                        data += string.Format(" name: '{0}{1}',data:[ ", pointId, dataView.ToTable().Rows[0]["PrimaryPollutant"].ToString());
                        int m = 0;
                        foreach (DataRowView drv in dataView)
                        {
                            m++;
                            //DateTime tstamp = Convert.ToDateTime(drv["DateTime1"]);
                            //string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                            if (m != dataView.Count)
                            {
                                if (drv["PrimaryPollutant"] != DBNull.Value)
                                {
                                    if (drv["PrimaryPollutant"].ToString() == "PM2.5")
                                    {
                                        data += "[" + Convert.ToDecimal(drv["PM25"].ToString()) * 1000 + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "PM10")
                                    {
                                        data += "[" + Convert.ToDecimal(drv["PM10"].ToString()) * 1000 + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "SO2")
                                    {
                                        data += "[" + Convert.ToDecimal(drv["SO2"].ToString()) * 1000 + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "NO2")
                                    {
                                        data += "[" + Convert.ToDecimal(drv["NO2"].ToString()) * 1000 + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "CO")
                                    {
                                        data += "[" + drv["CO"].ToString() + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "O3")
                                    {
                                        data += "[" + Convert.ToDecimal(drv["O3"].ToString()) * 1000 + "],";
                                    }

                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }
                            else
                            {
                                if (drv["PrimaryPollutant"] != DBNull.Value)
                                {
                                    if (drv["PrimaryPollutant"].ToString() == "PM2.5")
                                    {
                                        data += "[" + Convert.ToDecimal(drv["PM25"].ToString()) * 1000 + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "PM10")
                                    {
                                        data += "[" + Convert.ToDecimal(drv["PM10"].ToString()) * 1000 + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "SO2")
                                    {
                                        data += "[" + Convert.ToDecimal(drv["SO2"].ToString()) * 1000 + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "NO2")
                                    {
                                        data += "[" + Convert.ToDecimal(drv["NO2"].ToString()) * 1000 + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "CO")
                                    {
                                        data += "[" + drv["CO"].ToString() + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "O3")
                                    {
                                        data += "[" + Convert.ToDecimal(drv["O3"].ToString()) * 1000 + "],";
                                    }
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }

                        }
                        data += "]},";
                    }
                    if (data != "")
                    {
                        data = data.Substring(0, data.Length - 1);
                    }
                    data += "]";
                    hdjsonData.Value = data;

                    RegisterScript("generate();");
                }
                #endregion
                #region 区域日审核首要污染物浓度值
                else if (dsType == "RDayAudit")
                {
                    MonitoringPointAirService monitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();

                    dvRegion = GetRegionByPointId(pointIds);
                    foreach (DataRowView dr in dvRegion)
                    {
                        string regionUid = dr["RegionUid"].ToString();
                        listRegionUids.Add(regionUid);
                    }
                    string[] regionUids = listRegionUids.ToArray();
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
                    dtForAQI.Columns.Add("AQIValue", typeof(string));
                    dtForAQI.Columns.Add("PrimaryPollutant", typeof(string));
                    dtForAQI.Columns.Add("Grade", typeof(string));
                    dtForAQI.Columns.Add("Class", typeof(string));
                    dtForAQI.Columns.Add("RGBValue", typeof(string));

                    List<string> regionName = dvRegion.ToTable().AsEnumerable().Select(t => t.Field<string>("Region")).ToList();
                    IEnumerable<string> names = regionName.Distinct();
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
                        DateTime hourBegion = dtStart;
                        DateTime hourEndion = dtEnd;
                        decimal? PM25PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34004", hourBegion, hourEndion, 1, "2");
                        decimal? PM10PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34002", hourBegion, hourEndion, 1, "2");
                        decimal? NO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21004", hourBegion, hourEndion, 1, "2");
                        decimal? SO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21026", hourBegion, hourEndion, 1, "2");
                        decimal? COPollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21005", hourBegion, hourEndion, 1, "2");
                        decimal? O3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05024", hourBegion, hourEndion, 1, "2");
                        int? PM25Value = m_AQICalculateService.GetIAQI("a34004", Convert.ToDouble(PM25PollutantValue), 24);
                        int? PM10Value = m_AQICalculateService.GetIAQI("a34002", Convert.ToDouble(PM10PollutantValue), 24);
                        int? NO2Value = m_AQICalculateService.GetIAQI("a21004", Convert.ToDouble(NO2PollutantValue), 1);
                        int? SO2Value = m_AQICalculateService.GetIAQI("a21026", Convert.ToDouble(SO2PollutantValue), 1);
                        int? COValue = m_AQICalculateService.GetIAQI("a21005", Convert.ToDouble(COPollutantValue), 1);
                        int? O3Value = m_AQICalculateService.GetIAQI("a05024", Convert.ToDouble(O3PollutantValue), 1);
                        string AQIValue = m_AQICalculateService.GetAQI_Max_CNV(PM25Value, PM10Value, NO2Value, SO2Value, COValue, O3Value, "V");
                        string primaryPollutant = m_AQICalculateService.GetAQI_Max_CNV(PM25Value, PM10Value, NO2Value, SO2Value, COValue, O3Value, "N");
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
                        dr["O3"] = O3PollutantValue.ToString();
                        dr["O3_AQI"] = O3Value.ToString();
                        dr["AQIValue"] = AQIValue;
                        dr["PrimaryPollutant"] = primaryPollutant;
                        dr["Grade"] = grade;
                        dr["Class"] = class_AQI;
                        dr["RGBValue"] = color;
                        dtForAQI.Rows.Add(dr);

                    }
                    dataView = dtForAQI.AsDataView();
                    string data = "[";
                    for (int j = 0; j < dtForAQI.Rows.Count; j++)
                    {
                        string pointId = dtForAQI.Rows[j][0].ToString();
                        dataView.RowFilter = "PointId='" + pointId + "'";
                        data += "{";
                        data += string.Format(" name: '{0}',data:[ ", pointId);
                        int m = 0;
                        foreach (DataRowView drv in dataView)
                        {
                            m++;
                            //DateTime tstamp = Convert.ToDateTime(drv["DateTime1"]);
                            //string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                            if (m != dataView.Count)
                            {
                                if (drv["PrimaryPollutant"] != DBNull.Value)
                                {
                                    if (drv["PrimaryPollutant"].ToString() == "PM2.5")
                                    {
                                        data += "[" + drv["PM25_AQI"].ToString() + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "PM10")
                                    {
                                        data += "[" + drv["PM10_AQI"].ToString() + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "SO2")
                                    {
                                        data += "[" + drv["SO2_AQI"].ToString() + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "NO2")
                                    {
                                        data += "[" + drv["NO2_AQI"].ToString() + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "CO")
                                    {
                                        data += "[" + drv["CO_AQI"].ToString() + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "O3")
                                    {
                                        data += "[" + drv["O3_AQI"].ToString() + "],";
                                    }

                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }
                            else
                            {
                                if (drv["PrimaryPollutant"] != DBNull.Value)
                                {
                                    if (drv["PrimaryPollutant"].ToString() == "PM2.5")
                                    {
                                        data += "[" + drv["PM25_AQI"].ToString() + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "PM10")
                                    {
                                        data += "[" + drv["PM10_AQI"].ToString() + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "SO2")
                                    {
                                        data += "[" + drv["SO2_AQI"].ToString() + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "NO2")
                                    {
                                        data += "[" + drv["NO2_AQI"].ToString() + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "CO")
                                    {
                                        data += "[" + drv["CO_AQI"].ToString() + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "O3")
                                    {
                                        data += "[" + drv["O3_AQI"].ToString() + "],";
                                    }
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }

                        }
                        data += "]},";
                    }
                    if (data != "")
                    {
                        data = data.Substring(0, data.Length - 1);
                    }
                    data += "]";
                    hdjsonData.Value = data;

                    RegisterScript("generate();");
                }
                #endregion
                #region 站点小时原始首要污染物浓度值
                else if (dsType == "PHourOri")
                {
                    MonitoringPointAirService monitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
                    hdxData.Value = chartType;
                    //if ((dtEnd - dtStart).Days <= 1)
                    //{
                    //    dataView = m_DayAQIService.GetPointAQIHourInfo(pointIds, dtStart, dtEnd, qty, "OriData");
                    //}
                    //else
                    //{
                    //    dataView = m_DayAQIService.GetPointAQIHourInfoOver23(pointIds, dtStart, dtEnd, qty, "OriData");
                    //}     
                    if ((int)(dtEnd - dtStart).TotalHours == 23 && dtStart.ToString("yyyy-MM-dd") == dtEnd.ToString("yyyy-MM-dd"))
                    {
                        //dataView = m_DayAQIService.GetPointAQIHourInfo(points.Select(t => t.PointID).ToArray(), dtmBegion, dtmEnd.AddHours(1).AddSeconds(-1), qulityType, dataType);
                        dataView = m_DayAQIService.GetPointAQIHourInfoOver23(pointIds, dtStart, dtEnd.AddHours(1).AddSeconds(-1), qty, "OriData");
                    }
                    //if ((int)(dtmEnd - dtmBegion).TotalDays >= 1)
                    else
                    {
                        //dataView = m_DayAQIService.GetPointAQIHourInfoOver23(points.Select(t => t.PointID).ToArray(), dtmBegion, dtmEnd.AddHours(1).AddSeconds(-1), qulityType, dataType);
                        dataView = m_DayAQIService.GetPointAQIHourInfo(pointIds, dtStart, dtEnd.AddHours(1).AddSeconds(-1), qty, "OriData");
                    }
                    //if ((int)(dtEnd - dtStart).TotalDays < 1)
                    //{
                    //    dataView = m_DayAQIService.GetPointAQIHourInfo(pointIds, dtStart, dtEnd.AddHours(1).AddSeconds(-1), qty, "OriData");
                    //}
                    //if ((int)(dtEnd - dtStart).TotalDays >= 1)
                    //{
                    //    dataView = m_DayAQIService.GetPointAQIHourInfoOver23(pointIds.ToArray(), dtStart, dtEnd.AddHours(1).AddSeconds(-1), qty, "OriData");
                    //}
                    string data = "[";
                    for (int j = 0; j < pointIds.Length; j++)
                    {
                        string pointId = pointIds[j];
                        MonitoringPointEntity monitorPoint = monitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(pointId));
                        dataView.RowFilter = "PointId='" + pointIds[j] + "'";
                        data += "{";
                        data += string.Format(" name: '{0}{1}',data:[ ", monitorPoint.MonitoringPointName, dataView.ToTable().Rows[0]["PrimaryPollutant"].ToString());
                        int m = 0;
                        foreach (DataRowView drv in dataView)
                        {
                            m++;
                            //DateTime tstamp = Convert.ToDateTime(drv["DateTime1"]);
                            //string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                            if (m != dataView.Count)
                            {
                                if (drv["PrimaryPollutant"] != DBNull.Value)
                                {
                                    if (drv["PrimaryPollutant"].ToString() == "PM2.5")
                                    {
                                        data += "[PM2.5" + Convert.ToDecimal(drv["PM25"].ToString()) * 1000 + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "PM10")
                                    {
                                        data += "[" + Convert.ToDecimal(drv["PM10"].ToString()) * 1000 + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "SO2")
                                    {
                                        data += "[" + Convert.ToDecimal(drv["SO2"].ToString()) * 1000 + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "NO2")
                                    {
                                        data += "[" + Convert.ToDecimal(drv["NO2"].ToString()) * 1000 + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "CO")
                                    {
                                        data += "[" + drv["CO"].ToString() + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "O3")
                                    {
                                        data += "[" + Convert.ToDecimal(drv["Recent8HoursO3NT"].ToString()) * 1000 + "],";
                                    }
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }
                            else
                            {
                                if (drv["PrimaryPollutant"] != DBNull.Value)
                                {
                                    if (drv["PrimaryPollutant"].ToString() == "PM2.5")
                                    {
                                        data += "[" + Convert.ToDecimal(drv["PM25"].ToString()) * 1000 + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "PM10")
                                    {
                                        data += "[" + Convert.ToDecimal(drv["PM10"].ToString()) * 1000 + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "SO2")
                                    {
                                        data += "[" + Convert.ToDecimal(drv["SO2"].ToString()) * 1000 + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "NO2")
                                    {
                                        data += "[" + Convert.ToDecimal(drv["NO2"].ToString()) * 1000 + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "CO")
                                    {
                                        data += "[" + drv["CO"].ToString() + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "O3")
                                    {
                                        data += "[" + Convert.ToDecimal(drv["Recent8HoursO3NT"].ToString()) * 1000 + "],";
                                    }
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }

                        }
                        data += "]},";
                    }
                    if (data != "")
                    {
                        data = data.Substring(0, data.Length - 1);
                    }
                    data += "]";
                    hdjsonData.Value = data;
                }
                #endregion
                #region 站点日原始首要污染物浓度值
                else if (dsType == "PDayOri")
                {
                    MonitoringPointAirService monitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
                    hdxData.Value = chartType;
                    dataView = m_DayAQIService.GetPointAQIDayInfo(pointIds, dtStart, dtEnd, qty, "OriData");
                    string data = "[";
                    for (int j = 0; j < pointIds.Length; j++)
                    {
                        string pointId = pointIds[j];
                        MonitoringPointEntity monitorPoint = monitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(pointId));
                        dataView.RowFilter = "PointId='" + pointIds[j] + "'";
                        data += "{";
                        data += string.Format(" name: '{0}',data:[ ", monitorPoint.MonitoringPointName);
                        int m = 0;
                        foreach (DataRowView drv in dataView)
                        {
                            m++;
                            //DateTime tstamp = Convert.ToDateTime(drv["DateTime1"]);
                            //string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                            if (m != dataView.Count)
                            {
                                if (drv["PrimaryPollutant"] != DBNull.Value)
                                {
                                    if (drv["PrimaryPollutant"].ToString() == "PM2.5")
                                    {
                                        data += "[" + drv["PM25_AQI"].ToString() + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "PM10")
                                    {
                                        data += "[" + drv["PM10_AQI"].ToString() + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "SO2")
                                    {
                                        data += "[" + drv["SO2_AQI"].ToString() + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "NO2")
                                    {
                                        data += "[" + drv["NO2_AQI"].ToString() + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "CO")
                                    {
                                        data += "[" + drv["CO_AQI"].ToString() + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "O3")
                                    {
                                        data += "[" + drv["O3_AQI"].ToString() + "],";
                                    }

                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }
                            else
                            {
                                if (drv["PrimaryPollutant"] != DBNull.Value)
                                {
                                    if (drv["PrimaryPollutant"].ToString() == "PM2.5")
                                    {
                                        data += "[" + drv["PM25_AQI"].ToString() + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "PM10")
                                    {
                                        data += "[" + drv["PM10_AQI"].ToString() + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "SO2")
                                    {
                                        data += "[" + drv["SO2_AQI"].ToString() + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "NO2")
                                    {
                                        data += "[" + drv["NO2_AQI"].ToString() + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "CO")
                                    {
                                        data += "[" + drv["CO_AQI"].ToString() + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "O3")
                                    {
                                        data += "[" + drv["O3_AQI"].ToString() + "],";
                                    }
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }
                        }
                        data += "]},";
                    }
                    if (data != "")
                    {
                        data = data.Substring(0, data.Length - 1);
                    }
                    data += "]";
                    hdjsonData.Value = data;
                }
                #endregion
                #region 区域小时原始首要污染物浓度值
                else if (dsType == "RHourOri")
                {
                    MonitoringPointAirService monitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();

                    dvRegion = GetRegionByPointId(pointIds);
                    foreach (DataRowView dr in dvRegion)
                    {
                        string regionUid = dr["RegionUid"].ToString();
                        listRegionUids.Add(regionUid);
                    }
                    string[] regionUids = listRegionUids.ToArray();
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
                    dtForAQI.Columns.Add("AQIValue", typeof(string));
                    dtForAQI.Columns.Add("PrimaryPollutant", typeof(string));
                    dtForAQI.Columns.Add("Grade", typeof(string));
                    dtForAQI.Columns.Add("Class", typeof(string));
                    dtForAQI.Columns.Add("RGBValue", typeof(string));

                    //int hourNum = Convert.ToInt32((dtmhourEnd.Subtract(dtmhourBegin)).TotalHours);
                    //for (int i = 0; i < hourNum; i++)
                    //{

                    List<string> regionName = dvRegion.ToTable().AsEnumerable().Select(t => t.Field<string>("Region")).ToList();
                    IEnumerable<string> names = regionName.Distinct();
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

                        DateTime hourBegion = dtStart;
                        DateTime hourEndion = dtEnd;
                        decimal? PM25PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34004", hourBegion, hourEndion, 1, "1");
                        decimal? PM10PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34002", hourBegion, hourEndion, 1, "1");
                        decimal? NO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21004", hourBegion, hourEndion, 1, "1");
                        decimal? SO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21026", hourBegion, hourEndion, 1, "1");
                        decimal? COPollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21005", hourBegion, hourEndion, 1, "1");
                        //decimal? O3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05024", hourBegion, hourEndion, 1, "1");
                        decimal? O3PollutantValue = 0;
                        if ((int)(hourEndion - hourBegion).TotalHours == 23 && hourBegion.ToString("yyyy-MM-dd") == hourEndion.ToString("yyyy-MM-dd"))
                        {
                            //求最大的可跨天的O3_NT
                            O3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05024", hourBegion, hourEndion, 1, "1");
                        }
                        else
                        {
                            O3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05027", hourBegion, hourEndion, 1, "1");
                        }

                        int? PM25Value = m_AQICalculateService.GetIAQI("a34004", Convert.ToDouble(PM25PollutantValue), 24);
                        int? PM10Value = m_AQICalculateService.GetIAQI("a34002", Convert.ToDouble(PM10PollutantValue), 24);
                        int? NO2Value = m_AQICalculateService.GetIAQI("a21004", Convert.ToDouble(NO2PollutantValue), 24);
                        int? SO2Value = m_AQICalculateService.GetIAQI("a21026", Convert.ToDouble(SO2PollutantValue), 24);
                        int? COValue = m_AQICalculateService.GetIAQI("a21005", Convert.ToDouble(COPollutantValue), 24);
                        int? O3Value = m_AQICalculateService.GetIAQI("a05024", Convert.ToDouble(O3PollutantValue), 8);
                        string AQIValue = m_AQICalculateService.GetAQI_Max_CNV(SO2Value,NO2Value,PM10Value,COValue,O3Value,PM25Value, "V");
                        string primaryPollutant = m_AQICalculateService.GetAQI_Max_CNV(SO2Value, NO2Value, PM10Value, COValue, O3Value, PM25Value, "N");
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
                        dr["O3"] = O3PollutantValue.ToString();
                        dr["O3_AQI"] = O3Value.ToString();
                        dr["AQIValue"] = AQIValue;
                        dr["PrimaryPollutant"] = primaryPollutant;
                        dr["Grade"] = grade;
                        dr["Class"] = class_AQI;
                        dr["RGBValue"] = color;
                        dtForAQI.Rows.Add(dr);

                    }
                    //}
                    dataView = dtForAQI.AsDataView();
                    string data = "[";
                    for (int j = 0; j < dtForAQI.Rows.Count; j++)
                    {
                        string pointId = dtForAQI.Rows[j][0].ToString();
                        dataView.RowFilter = "PointId='" + pointId + "'";
                        data += "{";
                        data += string.Format(" name: '{0}{1}',data:[ ", pointId, dataView.ToTable().Rows[0]["PrimaryPollutant"].ToString());
                        int m = 0;
                        foreach (DataRowView drv in dataView)
                        {
                            m++;
                            //DateTime tstamp = Convert.ToDateTime(drv["DateTime1"]);
                            //string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                            if (m != dataView.Count)
                            {
                                if (drv["PrimaryPollutant"] != DBNull.Value)
                                {
                                    if (drv["PrimaryPollutant"].ToString() == "PM2.5")
                                    {
                                        data += "[" + Convert.ToDecimal(drv["PM25"].ToString())*1000 + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "PM10")
                                    {
                                        data += "[" + Convert.ToDecimal(drv["PM10"].ToString())*1000 + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "SO2")
                                    {
                                        data += "[" + Convert.ToDecimal(drv["SO2"].ToString())*1000 + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "NO2")
                                    {
                                        data += "[" + Convert.ToDecimal(drv["NO2"].ToString())*1000 + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "CO")
                                    {
                                        data += "[" + drv["CO"].ToString() + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "O3")
                                    {
                                        data += "[" + Convert.ToDecimal(drv["O3"].ToString())*1000 + "],";
                                    }

                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }
                            else
                            {
                                if (drv["PrimaryPollutant"] != DBNull.Value)
                                {
                                    if (drv["PrimaryPollutant"].ToString() == "PM2.5")
                                    {
                                        data += "[" + Convert.ToDecimal(drv["PM25"].ToString()) * 1000 + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "PM10")
                                    {
                                        data += "[" + Convert.ToDecimal(drv["PM10"].ToString()) * 1000 + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "SO2")
                                    {
                                        data += "[" + Convert.ToDecimal(drv["SO2"].ToString()) * 1000 + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "NO2")
                                    {
                                        data += "[" + Convert.ToDecimal(drv["NO2"].ToString()) * 1000 + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "CO")
                                    {
                                        data += "[" + drv["CO"].ToString() + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "O3")
                                    {
                                        data += "[" + Convert.ToDecimal(drv["O3"].ToString()) * 1000 + "],";
                                    }
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }
                        }
                        data += "]},";
                    }
                    if (data != "")
                    {
                        data = data.Substring(0, data.Length - 1);
                    }
                    data += "]";
                    hdjsonData.Value = data;

                    RegisterScript("generate();");
                }
                #endregion
                #region 区域日原始首要污染物浓度值
                else if (dsType == "RDayOri")
                {
                    MonitoringPointAirService monitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();

                    dvRegion = GetRegionByPointId(pointIds);
                    foreach (DataRowView dr in dvRegion)
                    {
                        string regionUid = dr["RegionUid"].ToString();
                        listRegionUids.Add(regionUid);
                    }
                    string[] regionUids = listRegionUids.ToArray();
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
                    dtForAQI.Columns.Add("AQIValue", typeof(string));
                    dtForAQI.Columns.Add("PrimaryPollutant", typeof(string));
                    dtForAQI.Columns.Add("Grade", typeof(string));
                    dtForAQI.Columns.Add("Class", typeof(string));
                    dtForAQI.Columns.Add("RGBValue", typeof(string));

                    List<string> regionName = dvRegion.ToTable().AsEnumerable().Select(t => t.Field<string>("Region")).ToList();
                    IEnumerable<string> names = regionName.Distinct();
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
                        DateTime hourBegion = dtStart;
                        DateTime hourEndion = dtEnd;
                        decimal? PM25PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34004", hourBegion, hourEndion, 1, "1");
                        decimal? PM10PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34002", hourBegion, hourEndion, 1, "1");
                        decimal? NO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21004", hourBegion, hourEndion, 1, "1");
                        decimal? SO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21026", hourBegion, hourEndion, 1, "1");
                        decimal? COPollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21005", hourBegion, hourEndion, 1, "1");
                        decimal? O3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05024", hourBegion, hourEndion, 1, "1");
                        int? PM25Value = m_AQICalculateService.GetIAQI("a34004", Convert.ToDouble(PM25PollutantValue), 24);
                        int? PM10Value = m_AQICalculateService.GetIAQI("a34002", Convert.ToDouble(PM10PollutantValue), 24);
                        int? NO2Value = m_AQICalculateService.GetIAQI("a21004", Convert.ToDouble(NO2PollutantValue), 1);
                        int? SO2Value = m_AQICalculateService.GetIAQI("a21026", Convert.ToDouble(SO2PollutantValue), 1);
                        int? COValue = m_AQICalculateService.GetIAQI("a21005", Convert.ToDouble(COPollutantValue), 1);
                        int? O3Value = m_AQICalculateService.GetIAQI("a05024", Convert.ToDouble(O3PollutantValue), 1);
                        string AQIValue = m_AQICalculateService.GetAQI_Max_CNV(PM25Value, PM10Value, NO2Value, SO2Value, COValue, O3Value, "V");
                        string primaryPollutant = m_AQICalculateService.GetAQI_Max_CNV(PM25Value, PM10Value, NO2Value, SO2Value, COValue, O3Value, "N");
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
                        dr["O3"] = O3PollutantValue.ToString();
                        dr["O3_AQI"] = O3Value.ToString();
                        dr["AQIValue"] = AQIValue;
                        dr["PrimaryPollutant"] = primaryPollutant;
                        dr["Grade"] = grade;
                        dr["Class"] = class_AQI;
                        dr["RGBValue"] = color;
                        dtForAQI.Rows.Add(dr);

                    }
                    dataView = dtForAQI.AsDataView();
                    string data = "[";
                    for (int j = 0; j < dtForAQI.Rows.Count; j++)
                    {
                        string pointId = dtForAQI.Rows[j][0].ToString();
                        dataView.RowFilter = "PointId='" + pointId + "'";
                        data += "{";
                        data += string.Format(" name: '{0}',data:[ ", pointId);
                        int m = 0;
                        foreach (DataRowView drv in dataView)
                        {
                            m++;
                            //DateTime tstamp = Convert.ToDateTime(drv["DateTime1"]);
                            //string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                            if (m != dataView.Count)
                            {
                                if (drv["PrimaryPollutant"] != DBNull.Value)
                                {
                                    if (drv["PrimaryPollutant"].ToString() == "PM2.5")
                                    {
                                        data += "[" + drv["PM25_AQI"].ToString() + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "PM10")
                                    {
                                        data += "[" + drv["PM10_AQI"].ToString() + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "SO2")
                                    {
                                        data += "[" + drv["SO2_AQI"].ToString() + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "NO2")
                                    {
                                        data += "[" + drv["NO2_AQI"].ToString() + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "CO")
                                    {
                                        data += "[" + drv["CO_AQI"].ToString() + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "O3")
                                    {
                                        data += "[" + drv["O3_AQI"].ToString() + "],";
                                    }

                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }
                            else
                            {
                                if (drv["PrimaryPollutant"] != DBNull.Value)
                                {
                                    if (drv["PrimaryPollutant"].ToString() == "PM2.5")
                                    {
                                        data += "[" + drv["PM25_AQI"].ToString() + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "PM10")
                                    {
                                        data += "[" + drv["PM10_AQI"].ToString() + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "SO2")
                                    {
                                        data += "[" + drv["SO2_AQI"].ToString() + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "NO2")
                                    {
                                        data += "[" + drv["NO2_AQI"].ToString() + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "CO")
                                    {
                                        data += "[" + drv["CO_AQI"].ToString() + "],";
                                    }
                                    else if (drv["PrimaryPollutant"].ToString() == "O3")
                                    {
                                        data += "[" + drv["O3_AQI"].ToString() + "],";
                                    }
                                }
                                else
                                {
                                    data += "[" + "null]";
                                }
                            }

                        }
                        data += "]},";
                    }
                    if (data != "")
                    {
                        data = data.Substring(0, data.Length - 1);
                    }
                    data += "]";
                    hdjsonData.Value = data;

                    RegisterScript("generate();");
                }
                #endregion
            }
            #endregion
        }


        public bool IsOrNotNumber(string a)
        {
            decimal d = 0;
            if (decimal.TryParse(a, out d) == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool IsNumeric(string text)
        {
            return Regex.IsMatch(text, @"^[+-]?\d*[.]?\d*$");
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
        /// 根据所选站点ID获取相应区域信息
        /// </summary>
        /// <param name="pointIds"></param>
        /// <returns></returns>
        public DataView GetRegionByPointId(string[] pointIds)
        {
            return pointAirService.GetRegionByPointId(pointIds);
        }

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
    }
}