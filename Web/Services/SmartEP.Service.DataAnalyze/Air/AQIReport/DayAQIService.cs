﻿using SmartEP.Core.Enums;
using SmartEP.DomainModel.BaseData;
using SmartEP.MonitoringBusinessRepository.Air;
using SmartEP.Service.DataAnalyze.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.Frame;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Core.Interfaces;
using SmartEP.Core.Generic;
using SmartEP.Utilities;
using SmartEP.Service.Core.Enums;
using SmartEP.Service.BaseData.Standard;
using SmartEP.Service.DataAnalyze.Air.DataQuery;
using SmartEP.Service.BaseData.Channel;
using System.Linq.Expressions;
using SmartEP.Data.Enums;
using SmartEP.Data.SqlServer.MonitoringBusiness.Air;

namespace SmartEP.Service.DataAnalyze.Air.AQIReport
{
    /// <summary>
    /// 名称：DayAQIService.cs
    /// 创建人：窦曙健
    /// 创建日期：2015-08-18
    /// 维护人员：
    /// 最新维护人员：徐阳
    /// 最新维护日期：2017-06-13
    /// 功能摘要：日AQI
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class DayAQIService : IDayAQI
    {
        /// <summary>
        /// 测点类
        /// </summary>
        MonitoringPointAirService g_MonitoringPointAir = null;

        /// <summary>
        /// 点位日AQI
        /// </summary>
        DayAQIRepository pointDayAQI = null;

        /// <summary>
        /// 区域日AQI
        /// </summary>
        RegionDayAQIRepository regionDayAQI = null;
        /// <summary>
        /// 区域小时AQI
        /// </summary>
        RegionHourAQIRepository regionHourAQI = null;
        #region 根据站点统计
        /// <summary>
        /// 根据key主键判断记录是否存在
        /// </summary>
        /// <param name="strKey">主键值</param>
        /// <returns></returns>
        public bool PIsExist(string strKey)
        {
            pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
            if (pointDayAQI != null)
                return pointDayAQI.IsExist(strKey);
            return false;
        }

        /// <summary>
        /// 获取各级别天数统计
        /// </summary>
        /// <param name="aqiType">AQI分指标字段</param>
        /// <param name="portIds">测点</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetPortGradeStatistics(IAQIType aqiType, string[] portIds, DateTime dtStart, DateTime dtEnd)
        {
            pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
            if (pointDayAQI != null)
                return pointDayAQI.GetGradeStatistics(aqiType, portIds, dtStart, dtEnd);
            return null;
        }

        /// <summary>
        /// 获取多点位平均后的AQI统计
        /// </summary>
        /// <param name="aqiType">AQI分指标字段</param>
        /// <param name="portIds">测点</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetGradeStatisticsMutilPoint(IAQIType aqiType, string[] portIds, DateTime dateStart, DateTime dateEnd)
        {
            pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
            if (pointDayAQI != null)
                return pointDayAQI.GetGradeStatisticsMutilPoint(aqiType, portIds, dateStart, dateEnd);
            return null;
        }

        /// <summary>
        ///  获取时间段内多点的AQI统计数据
        /// </summary>
        /// <param name="aqiType"></param>
        /// <param name="portIds"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataView GetMutilPointAQIData(string[] portIds, DateTime dateStart, DateTime dateEnd)
        {
            pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
            if (pointDayAQI != null)
                return pointDayAQI.GetMutilPointAQIData(portIds, dateStart, dateEnd);
            return null;
        }

        /// <summary>
        /// 获取所有天数
        /// </summary>
        /// <param name="Ponint">站点</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <returns></returns>
        public int GetPortAllDays(MonitoringPointEntity Point, DateTime dtStart, DateTime dtEnd)
        {
            pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
            if (pointDayAQI != null)
                return pointDayAQI.Retrieve(p => p.PointId == Point.PointId && p.DateTime >= dtStart && p.DateTime <= dtEnd && !string.IsNullOrEmpty(p.AQIValue)).Count();
            return 0;
        }

        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页（从0开始）</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetPortDataPager(string[] portIds, DateTime dtStart, DateTime dtEnd, int pageSize
            , int pageNo, out int recordTotal, string orderBy = "DateTime,PointId")
        {
            pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
            recordTotal = 0;
            if (pointDayAQI != null)
                return pointDayAQI.GetDataPager(portIds, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
            return null;
        }

        public DataView GetPortDataPager(string[] portIds, DateTime dtStart, DateTime dtEnd, string[] type, int pageSize
         , int pageNo, out int recordTotal, string orderBy = "DateTime,PointId")
        {
            pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
            recordTotal = 0;
            if (pointDayAQI != null)
                return pointDayAQI.GetDataPager(portIds, dtStart, dtEnd, type, pageSize, pageNo, out recordTotal, orderBy);
            return null;
        }

        public DataTable GetOverDaysList(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd, string TotalType, string strFactor, int StandAQI, int pageSize, int pageNo,
        string[] years, out int recordTotal, string orderBy = "DateTime,PointId")
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("portIds", typeof(string));
            dt.Columns.Add("StandardDays", typeof(string));
            dt.Columns.Add("OverDays", typeof(string));
            dt.Columns.Add("InvalidDays", typeof(string));
            dt.Columns.Add("StandardDaysRate", typeof(string));
            foreach (string year in years)
            {
                dt.Columns.Add(year, typeof(string));
            }
            if (TotalType == "质量类别")
            {
                dt.Columns.Add("Good", typeof(string));
                dt.Columns.Add("Moderate", typeof(string));
                dt.Columns.Add("LightlyPolluted", typeof(string));
                dt.Columns.Add("ModeratelyPolluted", typeof(string));
                dt.Columns.Add("HeavilyPolluted", typeof(string));
                dt.Columns.Add("SeverelyPolluted", typeof(string));
            }
            else
            {
                if (strFactor.Contains("超标"))
                {
                    if (factors.Contains("PM25"))
                    {
                        dt.Columns.Add("OverPM25", typeof(string));
                    }
                    if (factors.Contains("PM10"))
                    {
                        dt.Columns.Add("OverPM10", typeof(string));
                    }
                    if (factors.Contains("NO2"))
                    {
                        dt.Columns.Add("OverNO2", typeof(string));
                    }
                    if (factors.Contains("SO2"))
                    {
                        dt.Columns.Add("OverSO2", typeof(string));
                    }
                    if (factors.Contains("CO"))
                    {
                        dt.Columns.Add("OverCO", typeof(string));
                    }
                    if (factors.Contains("RecentoneHoursO3"))
                    {
                        dt.Columns.Add("OverO3", typeof(string));
                    }
                    if (factors.Contains("Recent8HoursO3"))
                    {
                        dt.Columns.Add("OverRecent8HoursO3", typeof(string));
                    }
                }
                if (strFactor.Contains("达标"))
                {
                    if (factors.Contains("PM25"))
                    {
                        dt.Columns.Add("StandPM25", typeof(string));
                    }
                    if (factors.Contains("PM10"))
                    {
                        dt.Columns.Add("StandPM10", typeof(string));
                    }
                    if (factors.Contains("NO2"))
                    {
                        dt.Columns.Add("StandNO2", typeof(string));
                    }
                    if (factors.Contains("SO2"))
                    {
                        dt.Columns.Add("StandSO2", typeof(string));
                    }
                    if (factors.Contains("CO"))
                    {
                        dt.Columns.Add("StandCO", typeof(string));
                    }
                    if (factors.Contains("RecentoneHoursO3"))
                    {
                        dt.Columns.Add("StandO3", typeof(string));
                    }
                    if (factors.Contains("Recent8HoursO3"))
                    {
                        dt.Columns.Add("StandRecent8HoursO3", typeof(string));
                    }
                }
                if (strFactor.Contains("首要污染物"))
                {
                    
                    if (factors.Contains("PM25"))
                    {
                        dt.Columns.Add("PrimaryPM25", typeof(string));
                    }
                    if (factors.Contains("PM10"))
                    {
                        dt.Columns.Add("PrimaryPM10", typeof(string));
                    }
                    if (factors.Contains("NO2"))
                    {
                        dt.Columns.Add("PrimaryNO2", typeof(string));
                    }
                    if (factors.Contains("SO2"))
                    {
                        dt.Columns.Add("PrimarySO2", typeof(string));
                    }
                    if (factors.Contains("CO"))
                    {
                        dt.Columns.Add("PrimaryCO", typeof(string));
                    }
                    if (factors.Contains("RecentoneHoursO3") || factors.Contains("Recent8HoursO3"))
                    {
                        dt.Columns.Add("PrimaryO3", typeof(string));
                    }
                }
            }
            pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
            DataView dv = pointDayAQI.GetDataPager(portIds, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
            Dictionary<string, DataView> dvlast = new Dictionary<string, DataView>();
            foreach (string year in years)
            {
                int addyear = dtEnd.Year - int.Parse(year);
                DataView dvNew = pointDayAQI.GetDataPager(portIds, dtStart.AddYears(-addyear), dtEnd.AddYears(-addyear), pageSize, pageNo, out recordTotal, orderBy);
                dvlast.Add(year, dvNew);
            }
            foreach (string portItem in portIds)
            {
                DataRow drNew = dt.NewRow();
                drNew["portIds"] = portItem;
                dv.RowFilter = "PointId=" + portItem + " and AQIValue <> '' and AQIValue<= " + StandAQI;
                double StandardDays = dv.Count;
                drNew["StandardDays"] = StandardDays;
                dv.RowFilter = "PointId=" + portItem + " and AQIValue <> '' and AQIValue>" + StandAQI;
                double OverDays = dv.Count;
                drNew["OverDays"] = OverDays;
                dv.RowFilter = "PointId=" + portItem + " and ((AQIValue <> '' and AQIValue<=0) or AQIValue is null or AQIValue = '')";
                double InvalidDays = dv.Count;
                drNew["InvalidDays"] = InvalidDays;
                if ((StandardDays + OverDays) != 0)
                {
                    drNew["StandardDaysRate"] = DecimalExtension.GetPollutantValue((Convert.ToDecimal(StandardDays / (StandardDays + OverDays))) * 100, 1).ToString();
                }
                foreach (string year in years)
                {
                    var dvnewlast = dvlast[year];
                    dvnewlast.RowFilter = "PointId='" + portItem + "' and AQIValue <> '' and AQIValue<= " + StandAQI;
                    double SZStandardDayslast = dvnewlast.Count;
                    dvnewlast.RowFilter = "PointId='" + portItem + "' and AQIValue <> '' and AQIValue>" + StandAQI;
                    double SZOverDayslast = dvnewlast.Count;
                    if (SZStandardDayslast + SZOverDayslast != 0)
                    {
                        drNew[year] = Math.Round(Convert.ToDecimal(SZStandardDayslast / (SZStandardDayslast + SZOverDayslast)) * 100, 1).ToString();
                    }
                }
                if (TotalType == "质量类别")
                {
                    dv.RowFilter = "PointId=" + portItem + " and AQIValue <> '' and AQIValue<=50";
                    drNew["Good"] = dv.Count;
                    dv.RowFilter = "PointId=" + portItem + " and AQIValue <> '' and AQIValue<=100 and AQIValue>50";
                    drNew["Moderate"] = dv.Count;
                    dv.RowFilter = "PointId=" + portItem + " and AQIValue <> '' and AQIValue<=150 and AQIValue>100";
                    drNew["LightlyPolluted"] = dv.Count;
                    dv.RowFilter = "PointId=" + portItem + " and AQIValue <> '' and AQIValue<=200 and AQIValue>150";
                    drNew["ModeratelyPolluted"] = dv.Count;
                    dv.RowFilter = "PointId=" + portItem + " and AQIValue <> '' and AQIValue<=300 and AQIValue>200";
                    drNew["HeavilyPolluted"] = dv.Count;
                    dv.RowFilter = "PointId=" + portItem + " and AQIValue <> '' and AQIValue>300";
                    drNew["SeverelyPolluted"] = dv.Count;
                }
                else
                {
                    if (strFactor.Contains("超标"))
                    {
                        if (factors.Contains("PM25"))
                        {
                            dv.RowFilter = "PointId=" + portItem + " and PM25_IAQI <> '' and PM25_IAQI> " + StandAQI;
                            drNew["OverPM25"] = dv.Count;
                        }
                        if (factors.Contains("PM10"))
                        {
                            dv.RowFilter = "PointId=" + portItem + " and PM10_IAQI <> '' and PM10_IAQI> " + StandAQI;
                            drNew["OverPM10"] = dv.Count;
                        }
                        if (factors.Contains("NO2"))
                        {
                            dv.RowFilter = "PointId=" + portItem + " and NO2_IAQI <> '' and NO2_IAQI> " + StandAQI;
                            drNew["OverNO2"] = dv.Count;
                        }
                        if (factors.Contains("SO2"))
                        {
                            dv.RowFilter = "PointId=" + portItem + " and  SO2_IAQI <> '' and SO2_IAQI> " + StandAQI;
                            drNew["OverSO2"] = dv.Count;
                        }
                        if (factors.Contains("CO"))
                        {
                            dv.RowFilter = "PointId=" + portItem + " and CO_IAQI <> '' and CO_IAQI> " + StandAQI;
                            drNew["OverCO"] = dv.Count;
                        }
                        if (factors.Contains("RecentoneHoursO3"))
                        {
                            dv.RowFilter = "PointId=" + portItem + " and MaxOneHourO3_IAQI <> '' and MaxOneHourO3_IAQI> " + StandAQI;
                            drNew["OverO3"] = dv.Count;
                        }
                        if (factors.Contains("Recent8HoursO3"))
                        {
                            dv.RowFilter = "PointId=" + portItem + " and Max8HourO3_IAQI <> '' and Max8HourO3_IAQI> " + StandAQI;
                            drNew["OverRecent8HoursO3"] = dv.Count;
                        }
                    }
                    if (strFactor.Contains("达标"))
                    {
                        if (factors.Contains("PM25"))
                        {
                            dv.RowFilter = "PointId=" + portItem + " and PM25_IAQI <> '' and PM25_IAQI<= " + StandAQI;
                            drNew["StandPM25"] = dv.Count;
                        }
                        if (factors.Contains("PM10"))
                        {
                            dv.RowFilter = "PointId=" + portItem + " and PM10_IAQI <> '' and PM10_IAQI<= " + StandAQI;
                            drNew["StandPM10"] = dv.Count;
                        }
                        if (factors.Contains("NO2"))
                        {
                            dv.RowFilter = "PointId=" + portItem + " and NO2_IAQI <> '' and NO2_IAQI<= " + StandAQI;
                            drNew["StandNO2"] = dv.Count;
                        }
                        if (factors.Contains("SO2"))
                        {
                            dv.RowFilter = "PointId=" + portItem + " and SO2_IAQI <> '' and SO2_IAQI<= " + StandAQI;
                            drNew["StandSO2"] = dv.Count;
                        }
                        if (factors.Contains("CO"))
                        {
                            dv.RowFilter = "PointId=" + portItem + " and CO_IAQI <> '' and CO_IAQI<= " + StandAQI;
                            drNew["StandCO"] = dv.Count;
                        }
                        if (factors.Contains("RecentoneHoursO3"))
                        {
                            dv.RowFilter = "PointId=" + portItem + " and MaxOneHourO3_IAQI <> '' and MaxOneHourO3_IAQI<= " + StandAQI;
                            drNew["StandO3"] = dv.Count;
                        }
                        if (factors.Contains("Recent8HoursO3"))
                        {
                            dv.RowFilter = "PointId=" + portItem + " and Max8HourO3_IAQI <> '' and Max8HourO3_IAQI<= " + StandAQI;
                            drNew["StandRecent8HoursO3"] = dv.Count;
                        }
                    }
                    if (strFactor.Contains("首要污染物"))
                    {
                        if (factors.Contains("PM25"))
                        {
                            dv.RowFilter = "PointId=" + portItem + " and PrimaryPollutant like '%PM2.5%'";
                            drNew["PrimaryPM25"] = dv.Count;
                        }
                        if (factors.Contains("PM10"))
                        {
                            dv.RowFilter = "PointId=" + portItem + " and PrimaryPollutant like '%PM10%'";
                            drNew["PrimaryPM10"] = dv.Count;
                        }
                        if (factors.Contains("NO2"))
                        {
                            dv.RowFilter = "PointId=" + portItem + " and PrimaryPollutant like '%NO2%'";
                            drNew["PrimaryNO2"] = dv.Count;
                        }
                        if (factors.Contains("SO2"))
                        {
                            dv.RowFilter = "PointId=" + portItem + " and PrimaryPollutant like '%SO2%'";
                            drNew["PrimarySO2"] = dv.Count;
                        }
                        if (factors.Contains("CO"))
                        {
                            dv.RowFilter = "PointId=" + portItem + " and PrimaryPollutant like '%CO%'";
                            drNew["PrimaryCO"] = dv.Count;
                        }
                        if (factors.Contains("RecentoneHoursO3") || factors.Contains("Recent8HoursO3"))
                        {
                            dv.RowFilter = "PointId=" + portItem + " and PrimaryPollutant like '%O3%'";
                            drNew["PrimaryO3"] = dv.Count;
                        }
                    }
                }
                dt.Rows.Add(drNew);
            }
            return dt;

        }
        /// <summary>
        /// 取得导出数据（行转列数据）
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetPortExportData(string[] portIds, DateTime dtStart, DateTime dtEnd, string orderBy = "DateTime,PointId")
        {
            pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
            if (pointDayAQI != null)
                return pointDayAQI.GetExportData(portIds, dtStart, dtEnd, orderBy);
            return null;
        }

        /// <summary>
        /// 获取点位AQI数据，时间点补遗
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetPortAllData(string[] portIds, DateTime dtStart, DateTime dtEnd, string orderBy = "PointId,DateTime")
        {
            pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
            if (pointDayAQI != null)
                return pointDayAQI.GetPortAllData(portIds, dtStart, dtEnd, orderBy);
            return null;
        }


        /// <summary>
        /// 各污染物首要污染物统计
        /// </summary>
        /// <param name="aqiType">AQI分指标</param>
        /// <param name="portIds">测点</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetPortContaminantsStatistics(IAQIType aqiType, string[] portIds, DateTime dtStart, DateTime dtEnd)
        {
            pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
            if (pointDayAQI != null)
                return pointDayAQI.GetContaminantsStatistics(aqiType, portIds, dtStart, dtEnd);
            return null;
        }

        /// <summary>
        /// 获取空气质量日报
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        public DataView GetAirQualityDayReport(string[] portIds, DateTime dtStart, DateTime dtEnd, string[] type, int pageSize
            , int pageNo, out int recordTotal, string orderBy = "DateTime,PointId")
        {
            recordTotal = 0;
            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            DataTable dt = new DataTable();
            dt = GetPortDataPager(portIds, dtStart, dtEnd, type, pageSize, pageNo, out recordTotal, orderBy).Table;
            dt.Columns.Add("PortName", typeof(string));
            dt.Columns.Add("OrderNew", typeof(int));
            dt.Columns.Add("DateTimeNew", typeof(DateTime));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int pointId = Convert.ToInt32(dt.Rows[i]["PointId"]);
                dt.Rows[i]["PortName"] = g_MonitoringPointAir.RetrieveEntityByPointId(pointId).MonitoringPointName;
                if (dt.Rows[i]["AQIValue"] != DBNull.Value && dt.Rows[i]["AQIValue"].ToString().Trim() != "")
                {
                    if (int.Parse(dt.Rows[i]["AQIValue"].ToString()) <= 50)
                    {
                        dt.Rows[i]["OrderNew"] = "1";
                    }
                    else if (int.Parse(dt.Rows[i]["AQIValue"].ToString()) <= 100)
                    {
                        dt.Rows[i]["OrderNew"] = "2";
                    }
                    else if (int.Parse(dt.Rows[i]["AQIValue"].ToString()) <= 150)
                    {
                        dt.Rows[i]["OrderNew"] = "3";
                    }
                    else if (int.Parse(dt.Rows[i]["AQIValue"].ToString()) <= 200)
                    {
                        dt.Rows[i]["OrderNew"] = "4";
                    }
                    else if (int.Parse(dt.Rows[i]["AQIValue"].ToString()) <= 300)
                    {
                        dt.Rows[i]["OrderNew"] = "5";
                    }
                    else
                    {
                        dt.Rows[i]["OrderNew"] = "6";
                    }
                }
                else
                {
                    dt.Rows[i]["OrderNew"] = "7";
                }
                if (dt.Rows[i]["DateTime"] != DBNull.Value)
                {
                    dt.Rows[i]["DateTimeNew"] = DateTime.Parse(dt.Rows[i]["DateTime"].ToString());
                }
            }
            return dt.AsDataView();
        }

        /// <summary>
        /// 获取日AQI相关数据信息
        /// </summary>
        /// <param name="BeginTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        public DataView GetAQIBaseInfo(string[] regionUids, DateTime BeginTime, DateTime EndTime, string[] qulityType)
        {
            regionDayAQI = Singleton<RegionDayAQIRepository>.GetInstance();
            DataView dv = regionDayAQI.GetAQIBaseInfo(BeginTime, EndTime);
            string where1 = string.Empty;
            string where2 = string.Empty;
            if (qulityType.Contains("无效天"))
            {
                if (qulityType.Length > 1)
                {
                    where1 += " or Max_AQI is NULL";
                }
                else
                {
                    where1 += " Max_AQI is NULL";
                }
                var list = qulityType.ToList();
                list.RemoveAt(list.IndexOf("无效天"));
                qulityType = list.ToArray();
            }

            //查询条件拼接
            string where = string.Empty;
            string regionGuidStr = string.Empty;
            if (regionUids != null && regionUids.Length > 0)
                regionGuidStr = " MonitoringRegionUid IN ('" + StringExtensions.GetArrayStrNoEmpty(regionUids.ToList(), "','") + "')";

            where = regionGuidStr;
            string qualityTypeStr = StringExtensions.GetArrayStrNoEmpty(qulityType.ToList<string>(), "','");
            if (qualityTypeStr != "")
            {
                qualityTypeStr = "'" + qualityTypeStr + "'";
            }
            if (!string.IsNullOrEmpty(qualityTypeStr))
            {
                where2 += "Class IN(" + qualityTypeStr + ")";
            }
            if (where1 != "")
                where += " and (" + where2 + where1 + ")";
            else if (where2 != "")
                where += " and " + where2;

            dv.RowFilter = where;
            DataTable dt = dv.ToTable();
            dt.Columns["Max8HourO3"].ColumnName = "O3";
            dt.Columns["Max8HourO3_AQI"].ColumnName = "O3_AQI";
            dt.Columns.Add("Number", typeof(int));
            for (int j = 0; j < dt.Rows.Count; j++)//给DataType、portName字段赋值
            {
                string pointId = dt.Rows[j]["MonitoringRegionUid"].ToString();
                switch (pointId)
                {
                    case "6a4e7093-f2c6-46b4-a11f-0f91b4adf379":
                        dt.Rows[j]["Number"] = 1;
                        break;
                    case "e1c104f3-aaf3-4d0e-9591-36cdc83be15a":
                        dt.Rows[j]["Number"] = 2;
                        break;
                    case "f320aa73-7c55-45d4-a363-e21408e0aac3":
                        dt.Rows[j]["Number"] = 3;
                        break;
                    case "69a993ff-78c6-459b-9322-ee77e0c8cd68":
                        dt.Rows[j]["Number"] = 4;
                        break;
                    case "8756bd44-ff18-46f7-aedf-615006d7474c":
                        dt.Rows[j]["Number"] = 5;
                        break;
                    case "7e05b94c-bbd4-45c3-919c-42da2e63fd43":
                        dt.Rows[j]["Number"] = 6;
                        break;
                    case "66d2abd1-ca39-4e39-909f-da872704fbfd":
                        dt.Rows[j]["Number"] = 7;
                        break;
                    case "d7d7a1fe-493a-4b3f-8504-b1850f6d9eff":
                        dt.Rows[j]["Number"] = 8;
                        break;
                    case "57b196ed-5038-4ad0-a035-76faee2d7a98":
                        dt.Rows[j]["Number"] = 9;
                        break;
                    case "2e2950cd-dbab-43b3-811d-61bd7569565a":
                        dt.Rows[j]["Number"] = 10;
                        break;
                    case "2fea3cb2-8b95-45e6-8a71-471562c4c89c":
                        dt.Rows[j]["Number"] = 11;
                        break;
                    case "5a566145-4884-453c-93ad-16e4344c85c9":
                        dt.Rows[j]["Number"] = 12;
                        break;
                }

            }
            DataView data = dt.DefaultView;
            data.Sort = "Number";
            return data;
        }
        /// <summary>
        /// 获取小时AQI相关数据信息
        /// </summary>
        /// <param name="BeginTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        public DataView GetAQIHourInfo(string[] regionUids, DateTime BeginTime, DateTime EndTime, string[] qulityType)
        {
            regionHourAQI = Singleton<RegionHourAQIRepository>.GetInstance();
            DataView dv = regionHourAQI.GetAQIHourBase(BeginTime, EndTime);
            //string regionGuidStr = string.Empty;
            //if (regionUids != null && regionUids.Length > 0)
            //    regionGuidStr = "MonitoringRegionUid IN ('" + StringExtensions.GetArrayStrNoEmpty(regionUids.ToList(), "','") + "')";
            string where1 = string.Empty;
            string where2 = string.Empty;
            if (qulityType.Contains("无效天"))
            {
                if (qulityType.Length > 1)
                {
                    where1 += " or Max_AQI is NULL";
                }
                else
                {
                    where1 += " Max_AQI is NULL";
                }
                var list = qulityType.ToList();
                list.RemoveAt(list.IndexOf("无效天"));
                qulityType = list.ToArray();
            }

            //查询条件拼接
            string where = string.Empty;
            string regionGuidStr = string.Empty;
            if (regionUids != null && regionUids.Length > 0)
                regionGuidStr = " MonitoringRegionUid IN ('" + StringExtensions.GetArrayStrNoEmpty(regionUids.ToList(), "','") + "')";

            where = regionGuidStr;
            string qualityTypeStr = StringExtensions.GetArrayStrNoEmpty(qulityType.ToList<string>(), "','");
            if (qualityTypeStr != "")
            {
                qualityTypeStr = "'" + qualityTypeStr + "'";
            }
            if (!string.IsNullOrEmpty(qualityTypeStr))
            {
                where2 += "Class IN(" + qualityTypeStr + ")";
            }
            if (where1 != "")
                where += " and (" + where2 + where1 + ")";
            else if (where2 != "")
                where += " and " + where2;

            dv.RowFilter = where;
            DataTable dt = dv.ToTable();
            dt.Columns.Add("Number", typeof(int));
            for (int j = 0; j < dt.Rows.Count; j++)//给DataType、portName字段赋值
            {
                string pointId = dt.Rows[j]["MonitoringRegionUid"].ToString();
                switch (pointId)
                {
                    case "6a4e7093-f2c6-46b4-a11f-0f91b4adf379":
                        dt.Rows[j]["Number"] = 1;
                        break;
                    case "e1c104f3-aaf3-4d0e-9591-36cdc83be15a":
                        dt.Rows[j]["Number"] = 2;
                        break;
                    case "f320aa73-7c55-45d4-a363-e21408e0aac3":
                        dt.Rows[j]["Number"] = 3;
                        break;
                    case "69a993ff-78c6-459b-9322-ee77e0c8cd68":
                        dt.Rows[j]["Number"] = 4;
                        break;
                    case "8756bd44-ff18-46f7-aedf-615006d7474c":
                        dt.Rows[j]["Number"] = 5;
                        break;
                    case "7e05b94c-bbd4-45c3-919c-42da2e63fd43":
                        dt.Rows[j]["Number"] = 6;
                        break;
                    case "66d2abd1-ca39-4e39-909f-da872704fbfd":
                        dt.Rows[j]["Number"] = 7;
                        break;
                    case "d7d7a1fe-493a-4b3f-8504-b1850f6d9eff":
                        dt.Rows[j]["Number"] = 8;
                        break;
                    case "57b196ed-5038-4ad0-a035-76faee2d7a98":
                        dt.Rows[j]["Number"] = 9;
                        break;
                    case "2e2950cd-dbab-43b3-811d-61bd7569565a":
                        dt.Rows[j]["Number"] = 10;
                        break;
                    case "2fea3cb2-8b95-45e6-8a71-471562c4c89c":
                        dt.Rows[j]["Number"] = 11;
                        break;
                    case "5a566145-4884-453c-93ad-16e4344c85c9":
                        dt.Rows[j]["Number"] = 12;
                        break;
                }

            }
            DataView data = dt.DefaultView;
            data.Sort = "Number";

            return data;
        }
        /// <summary>
        /// 获取小时AQI相关数据信息
        /// </summary>
        /// <param name="BeginTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        public DataView GetPointAQIDayInfo(string[] PointIds, DateTime BeginTime, DateTime EndTime, string[] qulityType, string dataType)
        {
            regionDayAQI = Singleton<RegionDayAQIRepository>.GetInstance();
            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            DataTable dt = new DataTable();
            for (int i = 0; i < PointIds.Length; i++)
            {
                int pointId = Convert.ToInt32(PointIds[i]);
                DataView dv = regionDayAQI.GetPointAQIDayBase(BeginTime, EndTime, pointId, dataType);
                if (i == 0)
                {
                    dt = dv.ToTable().Clone();
                }
                if (dv.Count > 0)
                {
                    dv[0]["PortName"] = g_MonitoringPointAir.RetrieveEntityByPointId(pointId).MonitoringPointName;
                    if (dv.Count > 0)
                    {
                        DataRow drNew = dt.NewRow();
                        foreach (DataColumn dc in dt.Columns)
                        {
                            drNew[dc.ColumnName] = dv[0][dc.ColumnName];
                        }

                        dt.Rows.Add(drNew);
                    }
                }
            }
            DataView dvNew = dt.DefaultView;
            string where1 = string.Empty;
            string where2 = string.Empty;
            if (qulityType.Contains("无效天"))
            {
                if (qulityType.Length > 1)
                {
                    where1 += " or Max_AQI is NULL";
                }
                else
                {
                    where1 += " Max_AQI is NULL";
                }
                var list = qulityType.ToList();
                list.RemoveAt(list.IndexOf("无效天"));
                qulityType = list.ToArray();
            }

            //查询条件拼接
            string where = string.Empty;
            string regionGuidStr = string.Empty;
            if (PointIds != null && PointIds.Length > 0)
                regionGuidStr = " PointId IN (" + StringExtensions.GetArrayStrNoEmpty(PointIds.ToList(), ",") + ")";

            where = regionGuidStr;
            string qualityTypeStr = StringExtensions.GetArrayStrNoEmpty(qulityType.ToList<string>(), "','");
            if (qualityTypeStr != "")
            {
                qualityTypeStr = "'" + qualityTypeStr + "'";
            }
            if (!string.IsNullOrEmpty(qualityTypeStr))
            {
                where2 += "Class IN(" + qualityTypeStr + ")";
            }
            if (where1 != "")
                where += " and (" + where2 + where1 + ")";
            else if (where2 != "")
                where += " and " + where2;

            dvNew.RowFilter = where;

            return dvNew;
        }
        /// <summary>
        /// 获取小时AQI相关数据信息
        /// </summary>
        /// <param name="BeginTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        public DataView GetPointAQIHourInfo(string[] PointIds, DateTime BeginTime, DateTime EndTime, string[] qulityType, string dataType)
        {
            regionHourAQI = Singleton<RegionHourAQIRepository>.GetInstance();
            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            DataTable dt = new DataTable();

            for (int i = 0; i < PointIds.Length; i++)
            {
                int pointId = Convert.ToInt32(PointIds[i]);
                DataView dv = regionHourAQI.GetPointAQIHourBase(BeginTime, EndTime, pointId, dataType);
                if (i == 0)
                {
                    dt = dv.ToTable().Clone();
                }
                if (dv.Count > 0)
                {
                    dv[0]["PortName"] = g_MonitoringPointAir.RetrieveEntityByPointId(pointId).MonitoringPointName;
                    if (dv.Count > 0)
                    {
                        DataRow drNew = dt.NewRow();
                        foreach (DataColumn dc in dt.Columns)
                        {
                            drNew[dc.ColumnName] = dv[0][dc.ColumnName];
                        }

                        dt.Rows.Add(drNew);
                    }
                }
            }
            DataView dvNew = dt.DefaultView;
            string where1 = string.Empty;
            string where2 = string.Empty;
            if (qulityType.Contains("无效天"))
            {
                if (qulityType.Length > 1)
                {
                    where1 += " or Max_AQI is NULL";
                }
                else
                {
                    where1 += " Max_AQI is NULL";
                }
                var list = qulityType.ToList();
                list.RemoveAt(list.IndexOf("无效天"));
                qulityType = list.ToArray();
            }

            //查询条件拼接
            string where = string.Empty;
            string regionGuidStr = string.Empty;
            if (PointIds != null && PointIds.Length > 0)
                regionGuidStr = " PointId IN (" + StringExtensions.GetArrayStrNoEmpty(PointIds.ToList(), ",") + ")";

            where = regionGuidStr;
            string qualityTypeStr = StringExtensions.GetArrayStrNoEmpty(qulityType.ToList<string>(), "','");
            if (qualityTypeStr != "")
            {
                qualityTypeStr = "'" + qualityTypeStr + "'";
            }
            if (!string.IsNullOrEmpty(qualityTypeStr))
            {
                where2 += "Class IN(" + qualityTypeStr + ")";
            }
            if (where1 != "")
                where += " and (" + where2 + where1 + ")";
            else if (where2 != "")
                where += " and " + where2;

            dvNew.RowFilter = where;

            return dvNew;
        }
        /// <summary>
        /// 获取小时AQI相关数据信息(时间间隔大于23小时，南通采用不可跨天O3—8)
        /// </summary>
        /// <param name="BeginTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        public DataView GetPointAQIHourInfoOver23(string[] PointIds, DateTime BeginTime, DateTime EndTime, string[] qulityType, string dataType)
        {
            regionHourAQI = Singleton<RegionHourAQIRepository>.GetInstance();
            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            PortHourAQIDAL m_PortAQIDAL = Singleton<PortHourAQIDAL>.GetInstance();
            DataTable dt = new DataTable();

            for (int i = 0; i < PointIds.Length; i++)
            {
                int pointId = Convert.ToInt32(PointIds[i]);
                DataView dvO3 = m_PortAQIDAL.GetOriDataPager(PointIds[i], BeginTime, EndTime);
                dvO3.RowFilter = "Recent8HoursO3 is not null and Recent8HoursO3 <> ''";
                object avgO3 = DBNull.Value;
                if (dvO3.Count > 0)
                {
                    //DataTable dtt = dvO3.Table;
                    avgO3 = dvO3.Table.AsEnumerable().Where(p => p["Recent8HoursO3"] != DBNull.Value && p["Recent8HoursO3"].ToString() != "").Select(t => Convert.ToDecimal(t.Field<string>("Recent8HoursO3"))).Sum() / dvO3.Count;
                }
                DataView dv = regionHourAQI.GetPointAQIHourBaseOver23(BeginTime, EndTime, pointId, dataType, avgO3);
                if (i == 0)
                {
                    dt = dv.ToTable().Clone();
                }
                if (dv.Count > 0)
                {
                    dv[0]["PortName"] = g_MonitoringPointAir.RetrieveEntityByPointId(pointId).MonitoringPointName;
                    if (dv.Count > 0)
                    {
                        DataRow drNew = dt.NewRow();
                        foreach (DataColumn dc in dt.Columns)
                        {
                            drNew[dc.ColumnName] = dv[0][dc.ColumnName];
                        }

                        dt.Rows.Add(drNew);
                    }
                }
            }
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                if (dt.Columns[j].ColumnName.Equals("Recent8HoursO3"))
                {
                    dt.Columns[j].ColumnName = "Recent8HoursO3NT";
                }
                if (dt.Columns[j].ColumnName.Equals("Recent8HoursO3_AQI"))
                {
                    dt.Columns[j].ColumnName = "Recent8HoursO3NT_AQI";
                }
            }
            DataView dvNew = dt.DefaultView;
            string where1 = string.Empty;
            string where2 = string.Empty;
            if (qulityType.Contains("无效天"))
            {
                if (qulityType.Length > 1)
                {
                    where1 += " or Max_AQI is NULL";
                }
                else
                {
                    where1 += " Max_AQI is NULL";
                }
                var list = qulityType.ToList();
                list.RemoveAt(list.IndexOf("无效天"));
                qulityType = list.ToArray();
            }

            //查询条件拼接
            string where = string.Empty;
            string regionGuidStr = string.Empty;
            if (PointIds != null && PointIds.Length > 0)
                regionGuidStr = " PointId IN (" + StringExtensions.GetArrayStrNoEmpty(PointIds.ToList(), ",") + ")";

            where = regionGuidStr;
            string qualityTypeStr = StringExtensions.GetArrayStrNoEmpty(qulityType.ToList<string>(), "','");
            if (qualityTypeStr != "")
            {
                qualityTypeStr = "'" + qualityTypeStr + "'";
            }
            if (!string.IsNullOrEmpty(qualityTypeStr))
            {
                where2 += "Class IN(" + qualityTypeStr + ")";
            }
            if (where1 != "")
                where += " and (" + where2 + where1 + ")";
            else if (where2 != "")
                where += " and " + where2;

            dvNew.RowFilter = where;

            return dvNew;
        }

        /// <summary>
        /// 获取区域时段均值小于一天时，可跨天最大值O3_NT
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="dtmBegion"></param>
        /// <param name="dtmEnd"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public decimal? GetOriDataPagerO3ForNT(string[] ids, DateTime dtmBegion, DateTime dtmEnd, string type)
        {
            PortHourAQIDAL m_PortAQIDAL = Singleton<PortHourAQIDAL>.GetInstance();
            DataView dv = m_PortAQIDAL.GetOriDataPagerO3ForNT(ids, dtmBegion, dtmEnd, type);
            decimal avgO3;
            if ((dtmEnd - dtmBegion).TotalDays > 0)
            {
                int count = Convert.ToInt32((dtmEnd - dtmBegion).TotalDays);
                for (int i = 0; i < count;i++ )
                {
                    //avgO3 += dv.Table.AsEnumerable().Where(p => p["Recent8HoursO3"] != DBNull.Value && p["Recent8HoursO3"].ToString() != "" && p["DateTime"].ToString().Contains("")).Select(t => t.Field<decimal>("Recent8HoursO3")).Max();
                }
                avgO3 = dv.Table.AsEnumerable().Where(p => p["Recent8HoursO3"] != DBNull.Value && p["Recent8HoursO3"].ToString() != "" && p["DateTime"].ToString() == "").Select(t => t.Field<decimal>("Recent8HoursO3")).Max();
            }
            else
            {
                avgO3 = dv.Table.AsEnumerable().Where(p => p["Recent8HoursO3"] != DBNull.Value && p["Recent8HoursO3"].ToString() != "").Select(t => t.Field<decimal>("Recent8HoursO3")).Max();
            }
            return avgO3;
            //dv.RowFilter = "max(Recent8HoursO3) Recent8HoursO3 is not null and Recent8HoursO3 <> ''";
            //return Convert.ToDecimal(dv[0]["Recent8HoursO3"]);
        }
        /// <summary>
        /// 日均值统计
        /// </summary>
        /// <param name="regionGuids">区域Guid</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <param name="orderBy">排序</param>
        /// <returns>
        /// RegionName：区域
        /// FactorName：因子
        /// DayMinValue：最小值
        /// DayMaxValue：最大值
        /// OutDays：超标天数
        /// MonitorDays：监控天数
        /// OutRate：超标率
        /// OutBiggestFactor：最大超标倍数
        /// </returns>
        public DataView GetAvgDayData(string[] PointIds, string[] factor, DateTime dateStart, DateTime dateEnd)
        {
            try
            {
                pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
                g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
                AirPollutantService m_AirPollutantService = new AirPollutantService();
                int PM25Unit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a34004").PollutantDecimalNum);
                int PM10Unit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a34002").PollutantDecimalNum);
                int NO2Unit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a21004").PollutantDecimalNum);
                int SO2Unit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a21026").PollutantDecimalNum);
                int COUnit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a21005").PollutantDecimalNum);
                int Max8HourO3Unit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a05024").PollutantDecimalNum);
                int recordTotal = 0;
                DataView AreaData = GetPortDataPager(PointIds, dateStart, dateEnd, 99999, 0, out recordTotal);
                EQIConcentrationService EQIConcentration = new EQIConcentrationService();
                EQIConcentrationLimitEntity entity = null;
                EQIConcentrationLimitEntity entityyear = null;
                DataView MinValues = GetPointsMinValue(PointIds, dateStart, dateEnd);
                DataView MaxValues = GetPointsMaxValue(PointIds, dateStart, dateEnd);
                DataView ExceedingDatas = GetPointsExceedingDays(PointIds, dateStart, dateEnd);
                DataView AvgValues = GetPointsAvgValue(PointIds, dateStart, dateEnd);
                DictionaryService dictionary = new DictionaryService();
                DataTable dt = new DataTable();
                dt.Columns.Add("RegionName", typeof(string));
                dt.Columns.Add("FactorName", typeof(string));
                dt.Columns.Add("DayMinValue", typeof(string));
                dt.Columns.Add("DayMaxValue", typeof(string));
                dt.Columns.Add("DayAvgValue", typeof(string));
                dt.Columns.Add("OutDays", typeof(string));
                dt.Columns.Add("MonitorDays", typeof(string));
                dt.Columns.Add("OutRate", typeof(string));
                dt.Columns.Add("OutBiggestFactor", typeof(string));
                dt.Columns.Add("OutDate", typeof(string));
                dt.Columns.Add("DataRange", typeof(string));
                dt.Columns.Add("YearPercent", typeof(string));
                dt.Columns.Add("YearPerOutRate", typeof(string));
                dt.Columns.Add("MedianUpper", typeof(string));
                dt.Columns.Add("MedianLower", typeof(string));
                dt.Columns.Add("MedianValueu", typeof(string));
                dt.Columns.Add("lower", typeof(string));
                dt.Columns.Add("upper", typeof(string));
                List<IAQIType> AQITypes = new List<IAQIType>();
                foreach (string item in factor)
                {
                    if (item == "SO2")
                        AQITypes.Add(IAQIType.SO2_IAQI);
                    if (item == "NO2")
                        AQITypes.Add(IAQIType.NO2_IAQI);
                    if (item == "PM10")
                        AQITypes.Add(IAQIType.PM10_IAQI);
                    if (item == "PM25")
                        AQITypes.Add(IAQIType.PM25_IAQI);
                    if (item == "CO")
                        AQITypes.Add(IAQIType.CO_IAQI);
                    if (item == "O3_8h")
                        AQITypes.Add(IAQIType.Max8HourO3_IAQI);
                }
                //AQITypes.Add(IAQIType.NO2_IAQI);
                //AQITypes.Add(IAQIType.PM10_IAQI);
                //AQITypes.Add(IAQIType.PM25_IAQI);
                //AQITypes.Add(IAQIType.CO_IAQI);
                //AQITypes.Add(IAQIType.Max8HourO3_IAQI);

                for (int i = 0; i < PointIds.Length; i++)
                {
                    int pointId = Convert.ToInt32(PointIds[i]);
                    string RegionName = g_MonitoringPointAir.RetrieveEntityByPointId(pointId).MonitoringPointName;//获取站点名称
                    DataRow[] drMin = MinValues.Table.Select("PointId=" + PointIds[i]);
                    DataRow[] drMax = MaxValues.Table.Select("PointId=" + PointIds[i]);
                    DataRow[] drExceeding = ExceedingDatas.Table.Select("PointId=" + PointIds[i]);
                    DataRow[] drAvg = AvgValues.Table.Select("PointId=" + PointIds[i]);

                    if (drMin.Length > 0 && drMax.Length > 0 && drExceeding.Length > 0 && drAvg.Length > 0)
                    {
                        for (int j = 0; j < AQITypes.Count; j++)
                        {
                            DataRow dr = dt.NewRow();
                            dr["RegionName"] = RegionName;
                            switch (AQITypes[j])
                            {
                                case IAQIType.SO2_IAQI:
                                    entity = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a21026", EQITimeType.TwentyFour);
                                    entityyear = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a21026", EQITimeType.Year);
                                    string yearpercentSO2 = "";
                                    //yearpercentSO2 = pointDayAQI.getpercent_Point(PointIds[i], dateStart, dateEnd, "a21026");
                                    AreaData.RowFilter = "SO2 is not null and convert(SO2, 'System.String')<>'' and PointId='" + PointIds[i] + "'";
                                    List<string> listSO2str = AreaData.ToTable().AsEnumerable().Select(r => r.Field<string>("SO2")).ToList<string>();
                                    List<decimal> listSO2 = new List<decimal>();
                                    foreach (string a in listSO2str)
                                    {
                                        listSO2.Add(Convert.ToDecimal(a));
                                    }
                                    listSO2.Sort();
                                    decimal[] arrSO2 = listSO2.ToArray();
                                    yearpercentSO2 = (DecimalExtension.GetPollutantValue(getpercentM("a21026", arrSO2), 3) * 1000).ToString("G0");
                                    dr["FactorName"] = "SO2 (μg/m<sup>3</sup>)";
                                    decimal MinValue = (DecimalExtension.GetPollutantValue(decimal.TryParse(drMin[0]["SO2"].ToString(), out MinValue) ? MinValue : 0, SO2Unit)) * 1000;
                                    dr["DayMinValue"] = MinValue != 0 ? MinValue.ToString("G0") : "";
                                    decimal MaxValue = (DecimalExtension.GetPollutantValue(decimal.TryParse(drMax[0]["SO2"].ToString(), out MaxValue) ? MaxValue : 0, SO2Unit)) * 1000;
                                    dr["DayMaxValue"] = MaxValue != 0 ? MaxValue.ToString("G0") : "";
                                    decimal AvgValue = (DecimalExtension.GetPollutantValue(decimal.TryParse(drAvg[0]["SO2"].ToString(), out AvgValue) ? AvgValue : 0, SO2Unit)) * 1000;
                                    dr["DayAvgValue"] = AvgValue != 0 ? AvgValue.ToString("G0") : "";
                                    dr["DataRange"] = MinValue.ToString("G0") + "~" + MaxValue.ToString("G0");
                                    dr["OutDays"] = drExceeding[0]["SO2_Over"];
                                    dr["MonitorDays"] = AreaData.ToTable().Select("PointId=" + PointIds[i] + " and DateTime>='" + dateStart +
                                        "' and DateTime<='" + dateEnd + "' and SO2_IAQI is not null and SO2_IAQI<>0").Count();
                                    decimal outSO2 = 0;
                                    if (dr["MonitorDays"].ToString().IsNotNullOrDBNull() && Convert.ToDecimal(dr["MonitorDays"]) != 0)
                                    {
                                        outSO2 = (decimal)(Convert.ToDecimal(dr["OutDays"]) / Convert.ToDecimal(dr["MonitorDays"]) * 100);
                                        dr["OutRate"] = DecimalExtension.GetPollutantValue(outSO2, 1).ToString() + "%";
                                    }
                                    decimal dSO2 = -9999;
                                    if (drMax[0]["SO2"].IsNotNullOrDBNull())
                                        dSO2 = (decimal)((Convert.ToDecimal(drMax[0]["SO2"]) - entity.Upper) / entity.Upper);
                                    string strdSO2 = "";
                                    if (dSO2 < 0)
                                    {
                                        strdSO2 = "/";
                                        dr["OutDate"] = "/";
                                    }
                                    else
                                    {
                                        strdSO2 = DecimalExtension.GetPollutantValue(dSO2, 2).ToString();
                                        DataRow[] SO2 = AreaData.ToTable().Select("PointId=" + PointIds[i] + " and DateTime>='" + dateStart +
                                                 "' and DateTime<='" + dateEnd + "' and SO2 ='" + MaxValue / 1000 + "'");
                                        if (SO2[0]["DateTime"].IsNotNullOrDBNull())
                                            dr["OutDate"] = string.Format("{0:yyyy-MM-dd}", SO2[0]["DateTime"]);
                                    }
                                    dr["OutBiggestFactor"] = strdSO2;
                                    if (yearpercentSO2 == null || yearpercentSO2 == "")
                                    {
                                        dr["YearPercent"] = "/";
                                    }
                                    else
                                    {
                                        dr["YearPercent"] = yearpercentSO2;
                                    }
                                    if (entityyear != null && yearpercentSO2 != null && yearpercentSO2 != "" && entityyear.Upper != null && entityyear.Upper != 0)
                                    {
                                        decimal dYearPerSO2 = (decimal)((Convert.ToDecimal(yearpercentSO2) / 1000 - entityyear.Upper) / entityyear.Upper);
                                        string strdYearPerSO2 = "";
                                        if (dYearPerSO2 < 0)
                                        {
                                            strdYearPerSO2 = "/";
                                        }
                                        else
                                        {
                                            strdYearPerSO2 = DecimalExtension.GetPollutantValue(dYearPerSO2, 2).ToString();
                                        }
                                        dr["YearPerOutRate"] = strdYearPerSO2;
                                    }
                                    AreaData.RowFilter = "SO2 is not null and SO2<>'' and PointId=" + PointIds[i];
                                    List<string> strSO2 = AreaData.ToTable().AsEnumerable().Select(r => r.Field<string>("SO2")).ToList();
                                    List<decimal> decSO2 = new List<decimal>(strSO2.Select(x => decimal.Parse(x)));
                                    Dictionary<string, string> ListMedian = Median(decSO2, "");
                                    if (ListMedian.Keys.Contains("median"))
                                    {
                                        dr["MedianValueu"] = ListMedian["median"];
                                    }
                                    if (ListMedian.Keys.Contains("median1"))
                                    {
                                        dr["MedianLower"] = ListMedian["median1"];
                                    }
                                    if (ListMedian.Keys.Contains("median3"))
                                    {
                                        dr["MedianUpper"] = ListMedian["median3"];
                                    } if (ListMedian.Keys.Contains("lower"))
                                    {
                                        dr["lower"] = ListMedian["lower"];
                                    } if (ListMedian.Keys.Contains("upper"))
                                    {
                                        dr["upper"] = ListMedian["upper"];
                                    }
                                    AreaData.RowFilter = string.Empty;
                                    break;
                                case IAQIType.NO2_IAQI:
                                    entity = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a21004", EQITimeType.TwentyFour);
                                    entityyear = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a21004", EQITimeType.Year);
                                    string yearpercentNO2 = "";
                                    //yearpercentNO2 = pointDayAQI.getpercent_Point(PointIds[i], dateStart, dateEnd, "a21004");
                                    AreaData.RowFilter = "NO2 is not null and convert(NO2, 'System.String')<>'' and PointId='" + PointIds[i] + "'";
                                    List<string> listNO2str = AreaData.ToTable().AsEnumerable().Select(r => r.Field<string>("NO2")).ToList<string>();
                                    List<decimal> listNO2 = new List<decimal>();
                                    foreach (string a in listNO2str)
                                    {
                                        listNO2.Add(Convert.ToDecimal(a));
                                    }
                                    listNO2.Sort();
                                    decimal[] arrNO2 = listNO2.ToArray();
                                    yearpercentNO2 = (DecimalExtension.GetPollutantValue(getpercentM("a21004", arrNO2), 3) * 1000).ToString("G0");
                                    dr["FactorName"] = "NO2 (μg/m<sup>3</sup>)";
                                    decimal MinValueNO2 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drMin[0]["NO2"].ToString(), out MinValueNO2) ? MinValueNO2 : 0, NO2Unit)) * 1000;
                                    dr["DayMinValue"] = MinValueNO2 != 0 ? MinValueNO2.ToString("G0") : "";
                                    decimal MaxValueNO2 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drMax[0]["NO2"].ToString(), out MaxValueNO2) ? MaxValueNO2 : 0, NO2Unit)) * 1000;
                                    dr["DayMaxValue"] = MaxValueNO2 != 0 ? MaxValueNO2.ToString("G0") : "";
                                    decimal AvgValueNO2 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drAvg[0]["NO2"].ToString(), out AvgValueNO2) ? AvgValueNO2 : 0, NO2Unit)) * 1000;
                                    dr["DayAvgValue"] = AvgValueNO2 != 0 ? AvgValueNO2.ToString("G0") : "";
                                    dr["DataRange"] = MinValueNO2.ToString("G0") + "~" + MaxValueNO2.ToString("G0");
                                    dr["OutDays"] = drExceeding[0]["NO2_Over"];
                                    dr["MonitorDays"] = AreaData.ToTable().Select("PointId='" + PointIds[i] + "' and DateTime>='" + dateStart +
                                        "' and DateTime<='" + dateEnd + "' and NO2_IAQI is not null and NO2_IAQI<>0").Count();
                                    decimal outNO2 = 0;
                                    if (dr["MonitorDays"].ToString().IsNotNullOrDBNull() && Convert.ToDecimal(dr["MonitorDays"]) != 0)
                                    {
                                        outNO2 = (decimal)(Convert.ToDecimal(dr["OutDays"]) / Convert.ToDecimal(dr["MonitorDays"]) * 100);
                                        dr["OutRate"] = DecimalExtension.GetPollutantValue(outNO2, 1).ToString() + "%";
                                    }
                                    decimal dNO2 = -9999;
                                    if (drMax[0]["NO2"].IsNotNullOrDBNull())
                                        dNO2 = (decimal)((Convert.ToDecimal(drMax[0]["NO2"]) - entity.Upper) / entity.Upper);
                                    string strdNO2 = "";
                                    if (dNO2 < 0)
                                    {
                                        strdNO2 = "/";
                                        dr["OutDate"] = "/";
                                    }
                                    else
                                    {
                                        strdNO2 = DecimalExtension.GetPollutantValue(dNO2, 2).ToString();
                                        DataRow[] NO2 = AreaData.ToTable().Select("PointId=" + PointIds[i] + " and DateTime>='" + dateStart +
                                "' and DateTime<='" + dateEnd + "' and NO2 ='" + MaxValueNO2 / 1000 + "'");
                                        if (NO2[0]["DateTime"].IsNotNullOrDBNull())
                                            dr["OutDate"] = string.Format("{0:yyyy-MM-dd}", NO2[0]["DateTime"]);
                                    }
                                    dr["OutBiggestFactor"] = strdNO2;
                                    if (yearpercentNO2 == null || yearpercentNO2 == "")
                                    {
                                        dr["YearPercent"] = "/";
                                    }
                                    else
                                    {
                                        dr["YearPercent"] = yearpercentNO2;
                                    }
                                    if (entityyear != null && yearpercentNO2 != null && yearpercentNO2 != "" && entityyear.Upper != null && entityyear.Upper != 0)
                                    {
                                        decimal dYearPerNO2 = (decimal)((Convert.ToDecimal(yearpercentNO2) / 1000 - entityyear.Upper) / entityyear.Upper);
                                        string strdYearPerNO2 = "";
                                        if (dYearPerNO2 < 0)
                                        {
                                            strdYearPerNO2 = "/";
                                        }
                                        else
                                        {
                                            strdYearPerNO2 = DecimalExtension.GetPollutantValue(dYearPerNO2, 2).ToString();
                                        }
                                        dr["YearPerOutRate"] = strdYearPerNO2;
                                    }
                                    AreaData.RowFilter = "NO2 is not null and NO2<>'' and PointId=" + PointIds[i];
                                    List<string> strNO2 = AreaData.ToTable().AsEnumerable().Select(r => r.Field<string>("NO2")).ToList();
                                    List<decimal> decNO2 = new List<decimal>(strNO2.Select(x => decimal.Parse(x)));
                                    Dictionary<string, string> ListMedianNO2 = Median(decNO2, "");
                                    if (ListMedianNO2.Keys.Contains("median"))
                                    {
                                        dr["MedianValueu"] = ListMedianNO2["median"];
                                    }
                                    if (ListMedianNO2.Keys.Contains("median1"))
                                    {
                                        dr["MedianLower"] = ListMedianNO2["median1"];
                                    }
                                    if (ListMedianNO2.Keys.Contains("median3"))
                                    {
                                        dr["MedianUpper"] = ListMedianNO2["median3"];
                                    } if (ListMedianNO2.Keys.Contains("lower"))
                                    {
                                        dr["lower"] = ListMedianNO2["lower"];
                                    } if (ListMedianNO2.Keys.Contains("upper"))
                                    {
                                        dr["upper"] = ListMedianNO2["upper"];
                                    }
                                    AreaData.RowFilter = string.Empty;
                                    break;
                                case IAQIType.PM10_IAQI:
                                    entity = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a34002", EQITimeType.TwentyFour);
                                    entityyear = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a34002", EQITimeType.Year);
                                    string yearpercentPM10 = "";
                                    //yearpercentPM10 = pointDayAQI.getpercent_Point(PointIds[i], dateStart, dateEnd, "a34002");
                                    AreaData.RowFilter = "PM10 is not null and convert(PM10, 'System.String')<>''  and PointId='" + PointIds[i] + "'";
                                    List<string> listPM10str = AreaData.ToTable().AsEnumerable().Select(r => r.Field<string>("PM10")).ToList<string>();
                                    List<decimal> listPM10 = new List<decimal>();
                                    foreach (string a in listPM10str)
                                    {
                                        listPM10.Add(Convert.ToDecimal(a));
                                    }
                                    listPM10.Sort();
                                    decimal[] arrPM10 = listPM10.ToArray();
                                    yearpercentPM10 = (DecimalExtension.GetPollutantValue(getpercentM("a34002", arrPM10), 3) * 1000).ToString("G0");
                                    dr["FactorName"] = "PM10 (μg/m<sup>3</sup>)";
                                    decimal MinValuePM10 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drMin[0]["PM10"].ToString(), out MinValuePM10) ? MinValuePM10 : 0, PM10Unit)) * 1000;
                                    dr["DayMinValue"] = MinValuePM10 != 0 ? MinValuePM10.ToString("G0") : "";
                                    decimal MaxValuePM10 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drMax[0]["PM10"].ToString(), out MaxValuePM10) ? MaxValuePM10 : 0, PM10Unit)) * 1000;
                                    dr["DayMaxValue"] = MaxValuePM10 != 0 ? MaxValuePM10.ToString("G0") : "";
                                    decimal AvgValuePM10 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drAvg[0]["PM10"].ToString(), out AvgValuePM10) ? AvgValuePM10 : 0, PM10Unit)) * 1000;
                                    dr["DayAvgValue"] = AvgValuePM10 != 0 ? AvgValuePM10.ToString("G0") : "";
                                    dr["DataRange"] = MinValuePM10.ToString("G0") + "~" + MaxValuePM10.ToString("G0");
                                    dr["OutDays"] = drExceeding[0]["PM10_Over"];
                                    dr["MonitorDays"] = AreaData.ToTable().Select("PointId='" + PointIds[i] + "' and DateTime>='" + dateStart +
                                        "' and DateTime<='" + dateEnd + "' and PM10_IAQI is not null and PM10_IAQI<>0").Count();
                                    decimal outPM10 = 0;
                                    if (dr["MonitorDays"].ToString().IsNotNullOrDBNull() && Convert.ToDecimal(dr["MonitorDays"]) != 0)
                                    {
                                        outPM10 = (decimal)(Convert.ToDecimal(dr["OutDays"]) / Convert.ToDecimal(dr["MonitorDays"]) * 100);
                                        dr["OutRate"] = DecimalExtension.GetPollutantValue(outPM10, 1).ToString() + "%";
                                    }
                                    decimal dPM10 = -9999;
                                    if (drMax[0]["PM10"].IsNotNullOrDBNull())
                                        dPM10 = (decimal)(Convert.ToDecimal(drMax[0]["PM10"]) - entity.Upper);
                                    string strdPM10 = "";
                                    if (dPM10 < 0)
                                    {
                                        strdPM10 = "/";
                                        dr["OutDate"] = "/";
                                    }
                                    else
                                    {
                                        strdPM10 = DecimalExtension.GetPollutantValue(dPM10, 2).ToString();
                                        DataRow[] PM10 = AreaData.ToTable().Select("PointId=" + PointIds[i] + " and DateTime>='" + dateStart +
                                            "' and DateTime<='" + dateEnd + "' and PM10 ='" + MaxValuePM10 / 1000 + "'");
                                        if (PM10[0]["DateTime"].IsNotNullOrDBNull())
                                            dr["OutDate"] = string.Format("{0:yyyy-MM-dd}", PM10[0]["DateTime"]);
                                    }
                                    dr["OutBiggestFactor"] = strdPM10;
                                    if (yearpercentPM10 == null || yearpercentPM10 == "")
                                    {
                                        dr["YearPercent"] = "/";
                                    }
                                    else
                                    {
                                        dr["YearPercent"] = yearpercentPM10;
                                    }
                                    if (entityyear != null && yearpercentPM10 != null && yearpercentPM10 != "" && entityyear.Upper != null && entityyear.Upper != 0)
                                    {
                                        decimal dYearPerPM10 = (decimal)((Convert.ToDecimal(yearpercentPM10) / 1000 - entityyear.Upper) / entityyear.Upper);
                                        string strdYearPerPM10 = "";
                                        if (dYearPerPM10 < 0)
                                        {
                                            strdYearPerPM10 = "/";
                                        }
                                        else
                                        {
                                            strdYearPerPM10 = DecimalExtension.GetPollutantValue(dYearPerPM10, 2).ToString();
                                        }
                                        dr["YearPerOutRate"] = strdYearPerPM10;
                                    }
                                    AreaData.RowFilter = "PM10 is not null and PM10<>'' and PointId=" + PointIds[i];
                                    List<string> strPM10 = AreaData.ToTable().AsEnumerable().Select(r => r.Field<string>("PM10")).ToList();
                                    List<decimal> decPM10 = new List<decimal>(strPM10.Select(x => decimal.Parse(x)));
                                    Dictionary<string, string> ListMedianPM10 = Median(decPM10, "");
                                    if (ListMedianPM10.Keys.Contains("median"))
                                    {
                                        dr["MedianValueu"] = ListMedianPM10["median"];
                                    }
                                    if (ListMedianPM10.Keys.Contains("median1"))
                                    {
                                        dr["MedianLower"] = ListMedianPM10["median1"];
                                    }
                                    if (ListMedianPM10.Keys.Contains("median3"))
                                    {
                                        dr["MedianUpper"] = ListMedianPM10["median3"];
                                    } if (ListMedianPM10.Keys.Contains("lower"))
                                    {
                                        dr["lower"] = ListMedianPM10["lower"];
                                    } if (ListMedianPM10.Keys.Contains("upper"))
                                    {
                                        dr["upper"] = ListMedianPM10["upper"];
                                    }
                                    AreaData.RowFilter = string.Empty;
                                    break;
                                case IAQIType.PM25_IAQI:
                                    entity = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a34004", EQITimeType.TwentyFour);
                                    entityyear = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a34004", EQITimeType.Year);
                                    string yearpercentPM25 = "";
                                    //yearpercentPM25 = pointDayAQI.getpercent_Point(PointIds[i], dateStart, dateEnd, "a34004");
                                    AreaData.RowFilter = "PM25 is not null and convert(PM25, 'System.String')<>''  and PointId='" + PointIds[i] + "'";
                                    List<string> listPM25str = AreaData.ToTable().AsEnumerable().Select(r => r.Field<string>("PM25")).ToList();
                                    List<decimal> listPM25 = new List<decimal>();
                                    foreach (string a in listPM25str)
                                    {
                                        listPM25.Add(Convert.ToDecimal(a));
                                    }
                                    listPM25.Sort();
                                    decimal[] arrPM25 = listPM25.ToArray();
                                    yearpercentPM25 = (DecimalExtension.GetPollutantValue(getpercentM("a34004", arrPM25), 3) * 1000).ToString("G0");
                                    dr["FactorName"] = "PM2.5 (μg/m<sup>3</sup>)";
                                    decimal MinValuePM25 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drMin[0]["PM25"].ToString(), out MinValuePM25) ? MinValuePM25 : 0, PM25Unit)) * 1000;
                                    dr["DayMinValue"] = MinValuePM25 != 0 ? MinValuePM25.ToString("G0") : "";
                                    decimal MaxValuePM25 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drMax[0]["PM25"].ToString(), out MaxValuePM25) ? MaxValuePM25 : 0, PM25Unit)) * 1000;
                                    dr["DayMaxValue"] = MaxValuePM25 != 0 ? MaxValuePM25.ToString("G0") : "";
                                    decimal AvgValuePM25 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drAvg[0]["PM25"].ToString(), out AvgValuePM25) ? AvgValuePM25 : 0, PM25Unit)) * 1000;
                                    dr["DayAvgValue"] = AvgValuePM25 != 0 ? AvgValuePM25.ToString("G0") : "";
                                    dr["DataRange"] = MinValuePM25.ToString("G0") + "~" + MaxValuePM25.ToString("G0");
                                    dr["OutDays"] = drExceeding[0]["PM25_Over"];
                                    dr["MonitorDays"] = AreaData.ToTable().Select("PointId=" + PointIds[i] + " and DateTime>='" + dateStart +
                                        "' and DateTime<='" + dateEnd + "' and PM25_IAQI is not null and PM25_IAQI<>0").Count();
                                    decimal outPM25 = 0;
                                    if (dr["MonitorDays"].ToString().IsNotNullOrDBNull() && Convert.ToDecimal(dr["MonitorDays"]) != 0)
                                    {
                                        outPM25 = (decimal)(Convert.ToDecimal(dr["OutDays"]) / Convert.ToDecimal(dr["MonitorDays"]) * 100);
                                        dr["OutRate"] = DecimalExtension.GetPollutantValue(outPM25, 1).ToString() + "%";
                                    }
                                    //dr["OutBiggestFactor"] = ((Convert.ToDecimal(drMax[0]["PM25"]) - entity.Upper) / entity.Upper).ToString();
                                    decimal dPM25 = -9999;
                                    if (drMax[0]["PM25"].IsNotNullOrDBNull())
                                        dPM25 = (decimal)((Convert.ToDecimal(drMax[0]["PM25"]) - entity.Upper) / entity.Upper);
                                    string strdPM25 = "";
                                    if (dPM25 < 0)
                                    {
                                        strdPM25 = "/";
                                        dr["OutDate"] = "/";
                                    }
                                    else
                                    {
                                        strdPM25 = DecimalExtension.GetPollutantValue(dPM25, 2).ToString();
                                        DataRow[] PM25 = AreaData.ToTable().Select("PointId=" + PointIds[i] + " and DateTime>='" + dateStart +
                                               "' and DateTime<='" + dateEnd + "' and PM25 ='" + MaxValuePM25 / 1000 + "'");
                                        if (PM25[0]["DateTime"].IsNotNullOrDBNull())
                                            dr["OutDate"] = string.Format("{0:yyyy-MM-dd}", PM25[0]["DateTime"]);
                                    }
                                    dr["OutBiggestFactor"] = strdPM25;
                                    if (yearpercentPM25 == null || yearpercentPM25 == "")
                                    {
                                        dr["YearPercent"] = "/";
                                    }
                                    else
                                    {
                                        dr["YearPercent"] = yearpercentPM25;
                                    }
                                    if (entityyear != null && yearpercentPM25 != null && yearpercentPM25 != "" && entityyear.Upper != null && entityyear.Upper != 0)
                                    {
                                        decimal dYearPerPM25 = (decimal)((Convert.ToDecimal(yearpercentPM25) / 1000 - entityyear.Upper) / entityyear.Upper);
                                        string strdYearPerPM25 = "";
                                        if (dYearPerPM25 < 0)
                                        {
                                            strdYearPerPM25 = "/";
                                        }
                                        else
                                        {
                                            strdYearPerPM25 = DecimalExtension.GetPollutantValue(dYearPerPM25, 2).ToString();
                                        }
                                        dr["YearPerOutRate"] = strdYearPerPM25;
                                    }
                                    AreaData.RowFilter = "PM25 is not null and PM25<>'' and PointId=" + PointIds[i];
                                    List<string> strPM25 = AreaData.ToTable().AsEnumerable().Select(r => r.Field<string>("PM25")).ToList();
                                    List<decimal> decPM25 = new List<decimal>(strPM25.Select(x => decimal.Parse(x)));
                                    Dictionary<string, string> ListMedianPM25 = Median(decPM25, "");
                                    if (ListMedianPM25.Keys.Contains("median"))
                                    {
                                        dr["MedianValueu"] = ListMedianPM25["median"];
                                    }
                                    if (ListMedianPM25.Keys.Contains("median1"))
                                    {
                                        dr["MedianLower"] = ListMedianPM25["median1"];
                                    }
                                    if (ListMedianPM25.Keys.Contains("median3"))
                                    {
                                        dr["MedianUpper"] = ListMedianPM25["median3"];
                                    } if (ListMedianPM25.Keys.Contains("lower"))
                                    {
                                        dr["lower"] = ListMedianPM25["lower"];
                                    } if (ListMedianPM25.Keys.Contains("upper"))
                                    {
                                        dr["upper"] = ListMedianPM25["upper"];
                                    }
                                    AreaData.RowFilter = string.Empty;
                                    break;
                                case IAQIType.CO_IAQI:
                                    entity = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a21005", EQITimeType.TwentyFour);
                                    entityyear = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a21005", EQITimeType.Year);
                                    string yearpercentCO = "";
                                    //yearpercentCO = pointDayAQI.getpercent_Point(PointIds[i], dateStart, dateEnd, "a21005");
                                    AreaData.RowFilter = "CO is not null and convert(CO, 'System.String')<>''  and PointId='" + PointIds[i] + "'";
                                    List<string> listCOstr = AreaData.ToTable().AsEnumerable().Select(r => r.Field<string>("CO")).ToList<string>();
                                    List<decimal> listCO = new List<decimal>();
                                    foreach (string a in listCOstr)
                                    {
                                        listCO.Add(Convert.ToDecimal(a));
                                    }
                                    listCO.Sort();
                                    decimal[] arrCO = listCO.ToArray();
                                    yearpercentCO = (DecimalExtension.GetPollutantValue(getpercentM("a21005", arrCO), 1)).ToString();
                                    dr["FactorName"] = "CO (mg/m<sup>3</sup>)";
                                    decimal MinValueCO = (DecimalExtension.GetPollutantValue(decimal.TryParse(drMin[0]["CO"].ToString(), out MinValueCO) ? MinValueCO : 0, COUnit));
                                    dr["DayMinValue"] = MinValueCO != 0 ? MinValueCO.ToString() : "";
                                    decimal MaxValueCO = (DecimalExtension.GetPollutantValue(decimal.TryParse(drMax[0]["CO"].ToString(), out MaxValueCO) ? MaxValueCO : 0, COUnit));
                                    dr["DayMaxValue"] = MaxValueCO != 0 ? MaxValueCO.ToString() : "";
                                    decimal AvgValueCO = (DecimalExtension.GetPollutantValue(decimal.TryParse(drAvg[0]["CO"].ToString(), out AvgValueCO) ? AvgValueCO : 0, COUnit));
                                    dr["DayAvgValue"] = AvgValueCO != 0 ? AvgValueCO.ToString() : "";
                                    dr["DataRange"] = MinValueCO.ToString() + "~" + MaxValueCO.ToString();
                                    dr["OutDays"] = drExceeding[0]["CO_Over"];
                                    dr["MonitorDays"] = AreaData.ToTable().Select("PointId=" + PointIds[i] + " and DateTime>='" + dateStart +
                                        "' and DateTime<='" + dateEnd + "' and CO_IAQI is not null and CO_IAQI<>0").Count();
                                    decimal outCO = 0;
                                    if (dr["MonitorDays"].ToString().IsNotNullOrDBNull() && Convert.ToDecimal(dr["MonitorDays"]) != 0)
                                    {
                                        outCO = (decimal)(Convert.ToDecimal(dr["OutDays"]) / Convert.ToDecimal(dr["MonitorDays"]) * 100);
                                        dr["OutRate"] = DecimalExtension.GetPollutantValue(outCO, 1).ToString() + "%";
                                    }
                                    //dr["OutBiggestFactor"] = ((Convert.ToDecimal(drMax[0]["CO"]) - entity.Upper) / entity.Upper).ToString();
                                    decimal dCO = -9999;
                                    if (drMax[0]["CO"].IsNotNullOrDBNull())
                                        dCO = (decimal)((Convert.ToDecimal(drMax[0]["CO"]) - entity.Upper) / entity.Upper);
                                    string strdCO = "";
                                    if (dCO < 0)
                                    {
                                        strdCO = "/";
                                        dr["OutDate"] = "/";
                                    }
                                    else
                                    {
                                        strdCO = DecimalExtension.GetPollutantValue(dCO, 2).ToString();
                                        DataRow[] CO = AreaData.ToTable().Select("PointId=" + PointIds[i] + " and DateTime>='" + dateStart +
                                                  "' and DateTime<='" + dateEnd + "' and CO ='" + MaxValueCO + "'");
                                        if (CO[0]["DateTime"].IsNotNullOrDBNull())
                                            dr["OutDate"] = string.Format("{0:yyyy-MM-dd}", CO[0]["DateTime"]);
                                    }
                                    dr["OutBiggestFactor"] = strdCO;
                                    if (yearpercentCO == null || yearpercentCO == "")
                                    {
                                        dr["YearPercent"] = "/";
                                    }
                                    else
                                    {
                                        dr["YearPercent"] = yearpercentCO;
                                    }
                                    if (entityyear != null && yearpercentCO != null && yearpercentCO != "" && entityyear.Upper != null && entityyear.Upper != 0)
                                    {
                                        decimal dYearPerCO = (decimal)((Convert.ToDecimal(yearpercentCO) - entityyear.Upper) / entityyear.Upper);
                                        string strdYearPerCO = "";
                                        if (dYearPerCO < 0)
                                        {
                                            strdYearPerCO = "/";
                                        }
                                        else
                                        {
                                            strdYearPerCO = DecimalExtension.GetPollutantValue(dYearPerCO, 2).ToString();
                                        }
                                        dr["YearPerOutRate"] = strdYearPerCO;
                                    }
                                    AreaData.RowFilter = "CO is not null and CO<>'' and PointId=" + PointIds[i];
                                    List<string> strCO = AreaData.ToTable().AsEnumerable().Select(r => r.Field<string>("CO")).ToList();
                                    List<decimal> decCO = new List<decimal>(strCO.Select(x => decimal.Parse(x)));
                                    Dictionary<string, string> ListMedianCO = Median(decCO, "CO");
                                    if (ListMedianCO.Keys.Contains("median"))
                                    {
                                        dr["MedianValueu"] = ListMedianCO["median"];
                                    }
                                    if (ListMedianCO.Keys.Contains("median1"))
                                    {
                                        dr["MedianLower"] = ListMedianCO["median1"];
                                    }
                                    if (ListMedianCO.Keys.Contains("median3"))
                                    {
                                        dr["MedianUpper"] = ListMedianCO["median3"];
                                    } if (ListMedianCO.Keys.Contains("lower"))
                                    {
                                        dr["lower"] = ListMedianCO["lower"];
                                    } if (ListMedianCO.Keys.Contains("upper"))
                                    {
                                        dr["upper"] = ListMedianCO["upper"];
                                    }
                                    AreaData.RowFilter = string.Empty;
                                    break;
                                case IAQIType.Max8HourO3_IAQI:
                                    entity = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a05024", EQITimeType.Eight);
                                    //entityyear = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a05024", EQITimeType.Year);
                                    string yearpercentO3 = "";
                                    //yearpercentO3 = pointDayAQI.getpercent_Point(PointIds[i], dateStart, dateEnd, "a05024");
                                    AreaData.RowFilter = "Max8HourO3 is not null and convert(Max8HourO3, 'System.String')<>''  and PointId='" + PointIds[i] + "'";
                                    List<string> listO3str = AreaData.ToTable().AsEnumerable().Select(r => r.Field<string>("Max8HourO3")).ToList<string>();
                                    List<decimal> listO3 = new List<decimal>();
                                    foreach (string a in listO3str)
                                    {
                                        listO3.Add(Convert.ToDecimal(a));
                                    }
                                    listO3.Sort();
                                    decimal[] arrO3 = listO3.ToArray();
                                    yearpercentO3 = (DecimalExtension.GetPollutantValue(getpercentM("a05024", arrO3), 3) * 1000).ToString("G0");
                                    dr["FactorName"] = "O3-8 (μg/m<sup>3</sup>)";
                                    decimal MinValueMax8HourO3 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drMin[0]["Max8HourO3"].ToString(), out MinValueMax8HourO3) ? MinValueMax8HourO3 : 0, Max8HourO3Unit)) * 1000;
                                    dr["DayMinValue"] = MinValueMax8HourO3 != 0 ? MinValueMax8HourO3.ToString("G0") : "";
                                    decimal MaxValueMax8HourO3 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drMax[0]["Max8HourO3"].ToString(), out MaxValueMax8HourO3) ? MaxValueMax8HourO3 : 0, Max8HourO3Unit)) * 1000;
                                    dr["DayMaxValue"] = MaxValueMax8HourO3 != 0 ? MaxValueMax8HourO3.ToString("G0") : "";
                                    decimal AvgValueMax8HourO3 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drAvg[0]["Max8HourO3"].ToString(), out AvgValueMax8HourO3) ? AvgValueMax8HourO3 : 0, Max8HourO3Unit)) * 1000;
                                    dr["DayAvgValue"] = AvgValueMax8HourO3 != 0 ? AvgValueMax8HourO3.ToString("G0") : "";
                                    dr["DataRange"] = MinValueMax8HourO3.ToString("G0") + "~" + MaxValueMax8HourO3.ToString("G0");
                                    dr["OutDays"] = drExceeding[0]["Max8HourO3_Over"];
                                    dr["MonitorDays"] = AreaData.ToTable().Select("PointId=" + PointIds[i] + " and DateTime>='" + dateStart +
                                        "' and DateTime<='" + dateEnd + "' and Max8HourO3_IAQI is not null and Max8HourO3_IAQI<>0").Count();
                                    decimal outMax8HourO3 = 0;
                                    if (dr["MonitorDays"].ToString().IsNotNullOrDBNull() && Convert.ToDecimal(dr["MonitorDays"]) != 0)
                                    {
                                        outMax8HourO3 = (decimal)(Convert.ToDecimal(dr["OutDays"]) / Convert.ToDecimal(dr["MonitorDays"]) * 100);
                                        dr["OutRate"] = DecimalExtension.GetPollutantValue(outMax8HourO3, 1).ToString() + "%";
                                    }
                                    decimal dSO3 = -9999;
                                    if (drMax[0]["Max8HourO3"].IsNotNullOrDBNull())
                                        dSO3 = (decimal)((Convert.ToDecimal(drMax[0]["Max8HourO3"]) - entity.Upper) / entity.Upper);
                                    string strdO3 = "";
                                    if (dSO3 < 0)
                                    {
                                        strdO3 = "/";
                                        dr["OutDate"] = "/";
                                    }
                                    else
                                    {
                                        strdO3 = DecimalExtension.GetPollutantValue(dSO3, 2).ToString();
                                        DataRow[] O3 = AreaData.ToTable().Select("PointId=" + PointIds[i] + " and DateTime>='" + dateStart +
                                                   "' and DateTime<='" + dateEnd + "' and Max8HourO3 ='" + MaxValueMax8HourO3 / 1000 + "'");
                                        if (O3[0]["DateTime"].IsNotNullOrDBNull())
                                            dr["OutDate"] = string.Format("{0:yyyy-MM-dd}", O3[0]["DateTime"]);
                                    }
                                    dr["OutBiggestFactor"] = strdO3;
                                    //  decimal dMax8HourO3 = (decimal)((Convert.ToDecimal(drMax[0]["Max8HourO3"]) - entity.Upper) / entity.Upper);

                                    decimal YearAverageMax8HourO3 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drAvg[0]["Max8HourO3"].ToString(), out YearAverageMax8HourO3) ? YearAverageMax8HourO3 : 0, Max8HourO3Unit));
                                    if (yearpercentO3 == null || yearpercentO3 == "")
                                    {
                                        dr["YearPercent"] = "/";
                                    }
                                    else
                                    {
                                        dr["YearPercent"] = yearpercentO3;
                                    }
                                    if (entity != null && yearpercentO3 != null && yearpercentO3 != "" && entity.Upper != null && entity.Upper != 0)
                                    {
                                        decimal dYearPerO3 = (decimal)((Convert.ToDecimal(yearpercentO3) / 1000 - entity.Upper) / entity.Upper);
                                        string strdYearPerO3 = "";
                                        if (dYearPerO3 < 0)
                                        {
                                            strdYearPerO3 = "/";
                                        }
                                        else
                                        {
                                            strdYearPerO3 = DecimalExtension.GetPollutantValue(dYearPerO3, 2).ToString();
                                        }
                                        dr["YearPerOutRate"] = strdYearPerO3;
                                    }
                                    //dr["YearPerOutRate"] = "/";
                                    AreaData.RowFilter = "Max8HourO3 is not null and Max8HourO3<>'' and PointId=" + PointIds[i];
                                    List<string> strMax8HourO3 = AreaData.ToTable().AsEnumerable().Select(r => r.Field<string>("Max8HourO3")).ToList();
                                    List<decimal> decMax8HourO3 = new List<decimal>(strMax8HourO3.Select(x => decimal.Parse(x)));
                                    Dictionary<string, string> ListMedianMax8HourO3 = Median(decMax8HourO3, "");
                                    if (ListMedianMax8HourO3.Keys.Contains("median"))
                                    {
                                        dr["MedianValueu"] = ListMedianMax8HourO3["median"];
                                    }
                                    if (ListMedianMax8HourO3.Keys.Contains("median1"))
                                    {
                                        dr["MedianLower"] = ListMedianMax8HourO3["median1"];
                                    }
                                    if (ListMedianMax8HourO3.Keys.Contains("median3"))
                                    {
                                        dr["MedianUpper"] = ListMedianMax8HourO3["median3"];
                                    } if (ListMedianMax8HourO3.Keys.Contains("lower"))
                                    {
                                        dr["lower"] = ListMedianMax8HourO3["lower"];
                                    } if (ListMedianMax8HourO3.Keys.Contains("upper"))
                                    {
                                        dr["upper"] = ListMedianMax8HourO3["upper"];
                                    }
                                    AreaData.RowFilter = string.Empty;
                                    break;
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                }
                return dt.DefaultView;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 日均值统计(重载方法供绘图使用)
        /// </summary>
        /// <param name="regionGuids">区域Guid</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <param name="orderBy">排序</param>
        /// <returns>
        /// RegionName：区域
        /// FactorName：因子
        /// DayMinValue：最小值
        /// DayMaxValue：最大值
        /// OutDays：超标天数
        /// MonitorDays：监控天数
        /// OutRate：超标率
        /// OutBiggestFactor：最大超标倍数
        /// </returns>
        public DataView GetAvgDayData(string[] PointIds, string[] factor, DateTime dateStart, DateTime dateEnd, string flag)
        {
            try
            {
                pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
                g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
                AirPollutantService m_AirPollutantService = new AirPollutantService();
                int PM25Unit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a34004").PollutantDecimalNum);
                int PM10Unit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a34002").PollutantDecimalNum);
                int NO2Unit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a21004").PollutantDecimalNum);
                int SO2Unit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a21026").PollutantDecimalNum);
                int COUnit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a21005").PollutantDecimalNum);
                int Max8HourO3Unit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a05024").PollutantDecimalNum);
                int recordTotal = 0;
                DataView AreaData = GetPortDataPager(PointIds, dateStart, dateEnd, 99999, 0, out recordTotal);
                EQIConcentrationService EQIConcentration = new EQIConcentrationService();
                EQIConcentrationLimitEntity entity = null;
                EQIConcentrationLimitEntity entityyear = null;
                DataView ExceedingDatas = GetPointsExceedingDays(PointIds, dateStart, dateEnd);
                DictionaryService dictionary = new DictionaryService();
                DataTable dt = new DataTable();
                dt.Columns.Add("RegionName", typeof(string));
                dt.Columns.Add("FactorName", typeof(string));
                dt.Columns.Add("OutDays", typeof(string));
                dt.Columns.Add("MonitorDays", typeof(string));
                dt.Columns.Add("OutRate", typeof(string));
                dt.Columns.Add("YearPercent", typeof(string));
                dt.Columns.Add("YearPerOutRate", typeof(string));
                List<IAQIType> AQITypes = new List<IAQIType>();
                foreach (string item in factor)
                {
                    if (item == "SO2")
                        AQITypes.Add(IAQIType.SO2_IAQI);
                    if (item == "NO2")
                        AQITypes.Add(IAQIType.NO2_IAQI);
                    if (item == "PM10")
                        AQITypes.Add(IAQIType.PM10_IAQI);
                    if (item == "PM25")
                        AQITypes.Add(IAQIType.PM25_IAQI);
                    if (item == "CO")
                        AQITypes.Add(IAQIType.CO_IAQI);
                    if (item == "O3_8h")
                        AQITypes.Add(IAQIType.Max8HourO3_IAQI);
                }

                for (int i = 0; i < PointIds.Length; i++)
                {
                    int pointId = Convert.ToInt32(PointIds[i]);
                    string RegionName = g_MonitoringPointAir.RetrieveEntityByPointId(pointId).MonitoringPointName;//获取站点名称
                    DataRow[] drExceeding = ExceedingDatas.Table.Select("PointId=" + PointIds[i]);

                    if (drExceeding.Length > 0)
                    {
                        for (int j = 0; j < AQITypes.Count; j++)
                        {
                            DataRow dr = dt.NewRow();
                            dr["RegionName"] = RegionName;
                            switch (AQITypes[j])
                            {
                                case IAQIType.SO2_IAQI:
                                    entity = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a21026", EQITimeType.TwentyFour);
                                    entityyear = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a21026", EQITimeType.Year);
                                    string yearpercentSO2 = "";
                                    AreaData.RowFilter = "SO2 is not null and convert(SO2, 'System.String')<>'' and PointId='" + PointIds[i] + "'";
                                    List<string> listSO2str = AreaData.ToTable().AsEnumerable().Select(r => r.Field<string>("SO2")).ToList<string>();
                                    List<decimal> listSO2 = new List<decimal>();
                                    foreach (string a in listSO2str)
                                    {
                                        listSO2.Add(Convert.ToDecimal(a));
                                    }
                                    listSO2.Sort();
                                    decimal[] arrSO2 = listSO2.ToArray();
                                    yearpercentSO2 = (DecimalExtension.GetPollutantValue(getpercentM("a21026", arrSO2), 3) * 1000).ToString("G0");
                                    dr["FactorName"] = "SO2 (μg/m<sup>3</sup>)";
                                    dr["OutDays"] = drExceeding[0]["SO2_Over"];
                                    dr["MonitorDays"] = AreaData.ToTable().Select("PointId=" + PointIds[i] + " and DateTime>='" + dateStart +
                                        "' and DateTime<='" + dateEnd + "' and SO2_IAQI is not null and SO2_IAQI<>0").Count();
                                    decimal outSO2 = 0;
                                    if (dr["MonitorDays"].ToString().IsNotNullOrDBNull() && Convert.ToDecimal(dr["MonitorDays"]) != 0)
                                    {
                                        outSO2 = (decimal)(Convert.ToDecimal(dr["OutDays"]) / Convert.ToDecimal(dr["MonitorDays"]) * 100);
                                        dr["OutRate"] = DecimalExtension.GetPollutantValue(outSO2, 1).ToString() + "%";
                                    }
                                    if (yearpercentSO2 == null || yearpercentSO2 == "")
                                    {
                                        dr["YearPercent"] = "/";
                                    }
                                    else
                                    {
                                        dr["YearPercent"] = yearpercentSO2;
                                    }
                                    if (entityyear != null && yearpercentSO2 != null && yearpercentSO2 != "" && entityyear.Upper != null && entityyear.Upper != 0)
                                    {
                                        decimal dYearPerSO2 = (decimal)((Convert.ToDecimal(yearpercentSO2) / 1000 - entityyear.Upper) / entityyear.Upper);
                                        string strdYearPerSO2 = "";
                                        if (dYearPerSO2 < 0)
                                        {
                                            strdYearPerSO2 = "/";
                                        }
                                        else
                                        {
                                            strdYearPerSO2 = DecimalExtension.GetPollutantValue(dYearPerSO2, 2).ToString();
                                        }
                                        dr["YearPerOutRate"] = strdYearPerSO2;
                                    }
                                    break;
                                case IAQIType.NO2_IAQI:
                                    entity = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a21004", EQITimeType.TwentyFour);
                                    entityyear = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a21004", EQITimeType.Year);
                                    string yearpercentNO2 = "";
                                    AreaData.RowFilter = "NO2 is not null and convert(NO2, 'System.String')<>'' and PointId='" + PointIds[i] + "'";
                                    List<string> listNO2str = AreaData.ToTable().AsEnumerable().Select(r => r.Field<string>("NO2")).ToList<string>();
                                    List<decimal> listNO2 = new List<decimal>();
                                    foreach (string a in listNO2str)
                                    {
                                        listNO2.Add(Convert.ToDecimal(a));
                                    }
                                    listNO2.Sort();
                                    decimal[] arrNO2 = listNO2.ToArray();
                                    yearpercentNO2 = (DecimalExtension.GetPollutantValue(getpercentM("a21004", arrNO2), 3) * 1000).ToString("G0");
                                    dr["FactorName"] = "NO2 (μg/m<sup>3</sup>)";
                                    dr["OutDays"] = drExceeding[0]["NO2_Over"];
                                    dr["MonitorDays"] = AreaData.ToTable().Select("PointId='" + PointIds[i] + "' and DateTime>='" + dateStart +
                                        "' and DateTime<='" + dateEnd + "' and NO2_IAQI is not null and NO2_IAQI<>0").Count();
                                    decimal outNO2 = 0;
                                    if (dr["MonitorDays"].ToString().IsNotNullOrDBNull() && Convert.ToDecimal(dr["MonitorDays"]) != 0)
                                    {
                                        outNO2 = (decimal)(Convert.ToDecimal(dr["OutDays"]) / Convert.ToDecimal(dr["MonitorDays"]) * 100);
                                        dr["OutRate"] = DecimalExtension.GetPollutantValue(outNO2, 1).ToString() + "%";
                                    }
                                    if (yearpercentNO2 == null || yearpercentNO2 == "")
                                    {
                                        dr["YearPercent"] = "/";
                                    }
                                    else
                                    {
                                        dr["YearPercent"] = yearpercentNO2;
                                    }
                                    if (entityyear != null && yearpercentNO2 != null && yearpercentNO2 != "" && entityyear.Upper != null && entityyear.Upper != 0)
                                    {
                                        decimal dYearPerNO2 = (decimal)((Convert.ToDecimal(yearpercentNO2) / 1000 - entityyear.Upper) / entityyear.Upper);
                                        string strdYearPerNO2 = "";
                                        if (dYearPerNO2 < 0)
                                        {
                                            strdYearPerNO2 = "/";
                                        }
                                        else
                                        {
                                            strdYearPerNO2 = DecimalExtension.GetPollutantValue(dYearPerNO2, 2).ToString();
                                        }
                                        dr["YearPerOutRate"] = strdYearPerNO2;
                                    }
                                    break;
                                case IAQIType.PM10_IAQI:
                                    entity = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a34002", EQITimeType.TwentyFour);
                                    entityyear = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a34002", EQITimeType.Year);
                                    string yearpercentPM10 = "";
                                    AreaData.RowFilter = "PM10 is not null and convert(PM10, 'System.String')<>''  and PointId='" + PointIds[i] + "'";
                                    List<string> listPM10str = AreaData.ToTable().AsEnumerable().Select(r => r.Field<string>("PM10")).ToList<string>();
                                    List<decimal> listPM10 = new List<decimal>();
                                    foreach (string a in listPM10str)
                                    {
                                        listPM10.Add(Convert.ToDecimal(a));
                                    }
                                    listPM10.Sort();
                                    decimal[] arrPM10 = listPM10.ToArray();
                                    yearpercentPM10 = (DecimalExtension.GetPollutantValue(getpercentM("a34002", arrPM10), 3) * 1000).ToString("G0");
                                    dr["FactorName"] = "PM10 (μg/m<sup>3</sup>)";
                                    dr["OutDays"] = drExceeding[0]["PM10_Over"];
                                    dr["MonitorDays"] = AreaData.ToTable().Select("PointId='" + PointIds[i] + "' and DateTime>='" + dateStart +
                                        "' and DateTime<='" + dateEnd + "' and PM10_IAQI is not null and PM10_IAQI<>0").Count();
                                    decimal outPM10 = 0;
                                    if (dr["MonitorDays"].ToString().IsNotNullOrDBNull() && Convert.ToDecimal(dr["MonitorDays"]) != 0)
                                    {
                                        outPM10 = (decimal)(Convert.ToDecimal(dr["OutDays"]) / Convert.ToDecimal(dr["MonitorDays"]) * 100);
                                        dr["OutRate"] = DecimalExtension.GetPollutantValue(outPM10, 1).ToString() + "%";
                                    }
                                    if (yearpercentPM10 == null || yearpercentPM10 == "")
                                    {
                                        dr["YearPercent"] = "/";
                                    }
                                    else
                                    {
                                        dr["YearPercent"] = yearpercentPM10;
                                    }
                                    if (entityyear != null && yearpercentPM10 != null && yearpercentPM10 != "" && entityyear.Upper != null && entityyear.Upper != 0)
                                    {
                                        decimal dYearPerPM10 = (decimal)((Convert.ToDecimal(yearpercentPM10) / 1000 - entityyear.Upper) / entityyear.Upper);
                                        string strdYearPerPM10 = "";
                                        if (dYearPerPM10 < 0)
                                        {
                                            strdYearPerPM10 = "/";
                                        }
                                        else
                                        {
                                            strdYearPerPM10 = DecimalExtension.GetPollutantValue(dYearPerPM10, 2).ToString();
                                        }
                                        dr["YearPerOutRate"] = strdYearPerPM10;
                                    }
                                    break;
                                case IAQIType.PM25_IAQI:
                                    entity = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a34004", EQITimeType.TwentyFour);
                                    entityyear = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a34004", EQITimeType.Year);
                                    string yearpercentPM25 = "";
                                    AreaData.RowFilter = "PM25 is not null and convert(PM25, 'System.String')<>''  and PointId='" + PointIds[i] + "'";
                                    List<string> listPM25str = AreaData.ToTable().AsEnumerable().Select(r => r.Field<string>("PM25")).ToList();
                                    List<decimal> listPM25 = new List<decimal>();
                                    foreach (string a in listPM25str)
                                    {
                                        listPM25.Add(Convert.ToDecimal(a));
                                    }
                                    listPM25.Sort();
                                    decimal[] arrPM25 = listPM25.ToArray();
                                    yearpercentPM25 = (DecimalExtension.GetPollutantValue(getpercentM("a34004", arrPM25), 3) * 1000).ToString("G0");
                                    dr["FactorName"] = "PM2.5 (μg/m<sup>3</sup>)";
                                    dr["OutDays"] = drExceeding[0]["PM25_Over"];
                                    dr["MonitorDays"] = AreaData.ToTable().Select("PointId=" + PointIds[i] + " and DateTime>='" + dateStart +
                                        "' and DateTime<='" + dateEnd + "' and PM25_IAQI is not null and PM25_IAQI<>0").Count();
                                    decimal outPM25 = 0;
                                    if (dr["MonitorDays"].ToString().IsNotNullOrDBNull() && Convert.ToDecimal(dr["MonitorDays"]) != 0)
                                    {
                                        outPM25 = (decimal)(Convert.ToDecimal(dr["OutDays"]) / Convert.ToDecimal(dr["MonitorDays"]) * 100);
                                        dr["OutRate"] = DecimalExtension.GetPollutantValue(outPM25, 1).ToString() + "%";
                                    }
                                    if (yearpercentPM25 == null || yearpercentPM25 == "")
                                    {
                                        dr["YearPercent"] = "/";
                                    }
                                    else
                                    {
                                        dr["YearPercent"] = yearpercentPM25;
                                    }
                                    if (entityyear != null && yearpercentPM25 != null && yearpercentPM25 != "" && entityyear.Upper != null && entityyear.Upper != 0)
                                    {
                                        decimal dYearPerPM25 = (decimal)((Convert.ToDecimal(yearpercentPM25) / 1000 - entityyear.Upper) / entityyear.Upper);
                                        string strdYearPerPM25 = "";
                                        if (dYearPerPM25 < 0)
                                        {
                                            strdYearPerPM25 = "/";
                                        }
                                        else
                                        {
                                            strdYearPerPM25 = DecimalExtension.GetPollutantValue(dYearPerPM25, 2).ToString();
                                        }
                                        dr["YearPerOutRate"] = strdYearPerPM25;
                                    }
                                    break;
                                case IAQIType.CO_IAQI:
                                    entity = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a21005", EQITimeType.TwentyFour);
                                    entityyear = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a21005", EQITimeType.Year);
                                    string yearpercentCO = "";
                                    AreaData.RowFilter = "CO is not null and convert(CO, 'System.String')<>''  and PointId='" + PointIds[i] + "'";
                                    List<string> listCOstr = AreaData.ToTable().AsEnumerable().Select(r => r.Field<string>("CO")).ToList<string>();
                                    List<decimal> listCO = new List<decimal>();
                                    foreach (string a in listCOstr)
                                    {
                                        listCO.Add(Convert.ToDecimal(a));
                                    }
                                    listCO.Sort();
                                    decimal[] arrCO = listCO.ToArray();
                                    yearpercentCO = (DecimalExtension.GetPollutantValue(getpercentM("a21005", arrCO), 1)).ToString();
                                    dr["FactorName"] = "CO (mg/m<sup>3</sup>)";
                                    dr["OutDays"] = drExceeding[0]["CO_Over"];
                                    dr["MonitorDays"] = AreaData.ToTable().Select("PointId=" + PointIds[i] + " and DateTime>='" + dateStart +
                                        "' and DateTime<='" + dateEnd + "' and CO_IAQI is not null and CO_IAQI<>0").Count();
                                    decimal outCO = 0;
                                    if (dr["MonitorDays"].ToString().IsNotNullOrDBNull() && Convert.ToDecimal(dr["MonitorDays"]) != 0)
                                    {
                                        outCO = (decimal)(Convert.ToDecimal(dr["OutDays"]) / Convert.ToDecimal(dr["MonitorDays"]) * 100);
                                        dr["OutRate"] = DecimalExtension.GetPollutantValue(outCO, 1).ToString() + "%";
                                    }
                                    if (yearpercentCO == null || yearpercentCO == "")
                                    {
                                        dr["YearPercent"] = "/";
                                    }
                                    else
                                    {
                                        dr["YearPercent"] = yearpercentCO;
                                    }
                                    if (entityyear != null && yearpercentCO != null && yearpercentCO != "" && entityyear.Upper != null && entityyear.Upper != 0)
                                    {
                                        decimal dYearPerCO = (decimal)((Convert.ToDecimal(yearpercentCO) - entityyear.Upper) / entityyear.Upper);
                                        string strdYearPerCO = "";
                                        if (dYearPerCO < 0)
                                        {
                                            strdYearPerCO = "/";
                                        }
                                        else
                                        {
                                            strdYearPerCO = DecimalExtension.GetPollutantValue(dYearPerCO, 2).ToString();
                                        }
                                        dr["YearPerOutRate"] = strdYearPerCO;
                                    }
                                    break;
                                case IAQIType.Max8HourO3_IAQI:
                                    entity = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a05024", EQITimeType.Eight);
                                    string yearpercentO3 = "";
                                    AreaData.RowFilter = "Max8HourO3 is not null and convert(Max8HourO3, 'System.String')<>''  and PointId='" + PointIds[i] + "'";
                                    List<string> listO3str = AreaData.ToTable().AsEnumerable().Select(r => r.Field<string>("Max8HourO3")).ToList<string>();
                                    List<decimal> listO3 = new List<decimal>();
                                    foreach (string a in listO3str)
                                    {
                                        listO3.Add(Convert.ToDecimal(a));
                                    }
                                    listO3.Sort();
                                    decimal[] arrO3 = listO3.ToArray();
                                    yearpercentO3 = (DecimalExtension.GetPollutantValue(getpercentM("a05024", arrO3), 3) * 1000).ToString("G0");
                                    dr["FactorName"] = "O3-8 (μg/m<sup>3</sup>)";
                                    dr["OutDays"] = drExceeding[0]["Max8HourO3_Over"];
                                    dr["MonitorDays"] = AreaData.ToTable().Select("PointId=" + PointIds[i] + " and DateTime>='" + dateStart +
                                        "' and DateTime<='" + dateEnd + "' and Max8HourO3_IAQI is not null and Max8HourO3_IAQI<>0").Count();
                                    decimal outMax8HourO3 = 0;
                                    if (dr["MonitorDays"].ToString().IsNotNullOrDBNull() && Convert.ToDecimal(dr["MonitorDays"]) != 0)
                                    {
                                        outMax8HourO3 = (decimal)(Convert.ToDecimal(dr["OutDays"]) / Convert.ToDecimal(dr["MonitorDays"]) * 100);
                                        dr["OutRate"] = DecimalExtension.GetPollutantValue(outMax8HourO3, 1).ToString() + "%";
                                    }
                                    if (yearpercentO3 == null || yearpercentO3 == "")
                                    {
                                        dr["YearPercent"] = "/";
                                    }
                                    else
                                    {
                                        dr["YearPercent"] = yearpercentO3;
                                    }
                                    if (entity != null && yearpercentO3 != null && yearpercentO3 != "" && entity.Upper != null && entity.Upper != 0)
                                    {
                                        decimal dYearPerO3 = (decimal)((Convert.ToDecimal(yearpercentO3) / 1000 - entity.Upper) / entity.Upper);
                                        string strdYearPerO3 = "";
                                        if (dYearPerO3 < 0)
                                        {
                                            strdYearPerO3 = "/";
                                        }
                                        else
                                        {
                                            strdYearPerO3 = DecimalExtension.GetPollutantValue(dYearPerO3, 2).ToString();
                                        }
                                        dr["YearPerOutRate"] = strdYearPerO3;
                                    }
                                    break;
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                }
                return dt.DefaultView;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Dictionary<string, string> Median(IList<decimal> array, string factor)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            return Median(array.ToArray(), factor);
        }

        public Dictionary<string, string> Median(decimal[] array, string factor)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            Dictionary<string, string> listmedian = new Dictionary<string, string>();

            if (array.Length > 0)
            {
                int endIndex = array.Length / 2;

                for (int i = 0; i < array.Length - 1; i++)
                {
                    for (int j = 0; j < array.Length - i - 1; j++)
                    {
                        if (array[j + 1] < array[j])
                        {
                            decimal temp = array[j + 1];
                            array[j + 1] = array[j];
                            array[j] = temp;
                        }
                    }
                }
                //if (array.Length % 2 != 0)
                //{
                //    return array[endIndex];
                //}
                int mid1 = endIndex / 2;
                int mid3 = array.Length % 2 == 0 ? (endIndex + mid1) : (endIndex + mid1 + 1);
                if (array.Length == 1)
                {
                    mid3 = 0;
                }
                decimal median = array.Length % 2 == 0 ? (array[endIndex] + array[endIndex - 1]) / 2 : array[endIndex];
                decimal median1 = (endIndex % 2 == 0 && endIndex != 0) ? (array[mid1] + array[mid1 - 1]) / 2 : array[mid1];
                decimal median3 = (endIndex % 2 == 0 && endIndex != 0) ? (array[mid3] + array[mid3 - 1]) / 2 : array[mid3];

                //decimal lower = median1 - Decimal.Parse("1.5") * (median3 - median1);
                //decimal upper = median3 + Decimal.Parse("1.5") * (median3 - median1);
                decimal lower = array[0];
                decimal upper = array[array.Length - 1];

                string medianValue, median1Value, median3Value, lowerValue, upperValue;
                if (factor == "CO")
                {
                    medianValue = DecimalExtension.GetPollutantValue(median, 1).ToString();
                    median1Value = DecimalExtension.GetPollutantValue(median1, 1).ToString();
                    median3Value = DecimalExtension.GetPollutantValue(median3, 1).ToString();
                    lowerValue = DecimalExtension.GetPollutantValue(lower, 1).ToString();
                    upperValue = DecimalExtension.GetPollutantValue(upper, 1).ToString();
                }
                else
                {
                    medianValue = (DecimalExtension.GetPollutantValue(median, 3) * 1000).ToString("G0");
                    median1Value = (DecimalExtension.GetPollutantValue(median1, 3) * 1000).ToString("G0");
                    median3Value = (DecimalExtension.GetPollutantValue(median3, 3) * 1000).ToString("G0");
                    lowerValue = (DecimalExtension.GetPollutantValue(lower, 3) * 1000).ToString("G0");
                    upperValue = (DecimalExtension.GetPollutantValue(upper, 3) * 1000).ToString("G0");
                }
                listmedian.Add("median", medianValue);
                listmedian.Add("median1", median1Value);
                listmedian.Add("median3", median3Value);
                listmedian.Add("lower", lowerValue);
                listmedian.Add("upper", upperValue);
            }
            return listmedian;
        }
        /// <summary>
        /// 取得指定日期内日数据均值数据
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <returns>
        /// 返回字段：
        /// portIds：站点ID
        /// PM25：PM25浓度
        /// PM10：PM10浓度
        /// NO2：NO2浓度
        /// SO2：SO2浓度
        /// CO：CO浓度
        /// MaxOneHourO3：MaxOneHourO3浓度
        /// Max8HourO3：Max8HourO3浓度
        /// </returns>
        public DataView GetPointsAvgValue(string[] portIds, DateTime dateStart, DateTime dateEnd)
        {
            pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
            if (pointDayAQI != null)
                return pointDayAQI.GetAvgValue(portIds, dateStart, dateEnd);
            return null;
        }

        /// <summary>
        /// 取得指定日期内日数据最大值数据
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <returns>
        /// 返回字段：
        /// portIds：站点ID
        /// PM25：PM25浓度
        /// PM10：PM10浓度
        /// NO2：NO2浓度
        /// SO2：SO2浓度
        /// CO：CO浓度
        /// MaxOneHourO3：MaxOneHourO3浓度
        /// Max8HourO3：Max8HourO3浓度
        /// PM25_IAQI：PM25指数
        /// PM10_IAQI：PM10指数
        /// NO2_IAQI：NO2指数
        /// SO2_IAQI：SO2指数
        /// CO_IAQI：CO指数
        /// MaxOneHourO3_IAQI：MaxOneHourO3指数
        /// Max8HourO3_IAQI：Max8HourO3指数
        /// AQIValue：空气质量指数
        /// </returns>
        public DataView GetPointsMaxValue(string[] portIds, DateTime dateStart, DateTime dateEnd)
        {
            pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
            if (pointDayAQI != null)
                return pointDayAQI.GetMaxValue(portIds, dateStart, dateEnd);
            return null;
        }
        /// <summary>
        /// 取得指定日期内日数据最大值数据
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <returns>
        /// 返回字段：
        /// portIds：站点ID
        /// PM25：PM25浓度
        /// PM10：PM10浓度
        /// NO2：NO2浓度
        /// SO2：SO2浓度
        /// CO：CO浓度
        /// MaxOneHourO3：MaxOneHourO3浓度
        /// Max8HourO3：Max8HourO3浓度
        /// PM25_IAQI：PM25指数
        /// PM10_IAQI：PM10指数
        /// NO2_IAQI：NO2指数
        /// SO2_IAQI：SO2指数
        /// CO_IAQI：CO指数
        /// MaxOneHourO3_IAQI：MaxOneHourO3指数
        /// Max8HourO3_IAQI：Max8HourO3指数
        /// AQIValue：空气质量指数
        /// </returns>
        public DataView GetPointsMaxValueOne(string[] portIds, DateTime dateStart, DateTime dateEnd)
        {
            pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
            if (pointDayAQI != null)
                return pointDayAQI.GetMaxValueOne(portIds, dateStart, dateEnd);
            return null;
        }
        /// <summary>
        /// 取得指定日期内日数据最小值数据
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <returns>
        /// 返回字段：
        /// portIds：站点ID
        /// PM25：PM25浓度
        /// PM10：PM10浓度
        /// NO2：NO2浓度
        /// O2：SO2浓度
        /// CO：CO浓度
        /// MaxOneHourO3：MaxOneHourO3浓度
        /// Max8HourO3：Max8HourO3浓度
        /// PM25_IAQI：PM25指数
        /// PM10_IAQI：PM10指数
        /// NO2_IAQI：NO2指数
        /// SO2_IAQI：SO2指数
        /// CO_IAQI：CO指数
        /// MaxOneHourO3_IAQI：MaxOneHourO3指数
        /// Max8HourO3_IAQI：Max8HourO3指数
        /// AQIValue：空气质量指数
        /// </returns>
        public DataView GetPointsMinValue(string[] portIds, DateTime dateStart, DateTime dateEnd)
        {
            pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
            if (pointDayAQI != null)
                return pointDayAQI.GetMinValue(portIds, dateStart, dateEnd);
            return null;
        }
        /// <summary>
        /// 取得指定日期内日数据最小值数据
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <returns>
        /// 返回字段：
        /// portIds：站点ID
        /// PM25：PM25浓度
        /// PM10：PM10浓度
        /// NO2：NO2浓度
        /// O2：SO2浓度
        /// CO：CO浓度
        /// MaxOneHourO3：MaxOneHourO3浓度
        /// Max8HourO3：Max8HourO3浓度
        /// PM25_IAQI：PM25指数
        /// PM10_IAQI：PM10指数
        /// NO2_IAQI：NO2指数
        /// SO2_IAQI：SO2指数
        /// CO_IAQI：CO指数
        /// MaxOneHourO3_IAQI：MaxOneHourO3指数
        /// Max8HourO3_IAQI：Max8HourO3指数
        /// AQIValue：空气质量指数
        /// </returns>
        public DataView GetPointsMinValueOne(string[] portIds, DateTime dateStart, DateTime dateEnd)
        {
            pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
            if (pointDayAQI != null)
                return pointDayAQI.GetMinValueOne(portIds, dateStart, dateEnd);
            return null;
        }
        /// <summary>
        /// 取得指定日期内日数据样本数
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <returns>
        /// 返回字段：
        /// portIds：站点ID
        /// PM25：PM25浓度
        /// PM10：PM10浓度
        /// NO2：NO2浓度
        /// O2：SO2浓度
        /// CO：CO浓度
        /// MaxOneHourO3：MaxOneHourO3浓度
        /// Max8HourO3：Max8HourO3浓度
        /// PM25_IAQI：PM25指数
        /// PM10_IAQI：PM10指数
        /// NO2_IAQI：NO2指数
        /// SO2_IAQI：SO2指数
        /// CO_IAQI：CO指数
        /// MaxOneHourO3_IAQI：MaxOneHourO3指数
        /// Max8HourO3_IAQI：Max8HourO3指数
        /// AQIValue：空气质量指数
        /// </returns>
        public DataView GetPointsCountValue(string[] portIds, DateTime dateStart, DateTime dateEnd)
        {
            pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
            if (pointDayAQI != null)
                return pointDayAQI.GetCountValue(portIds, dateStart, dateEnd);
            return null;
        }

        /// <summary>
        /// 日数据超标天数统计
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <returns>
        /// 返回字段：
        /// PointId：站点ID
        /// ValidCount：有效天
        /// OverCount：超标天
        /// PM25_Over：PM25超标天
        /// PM10_Over：PM10超标天
        /// NO2_Over：NO2超标天
        /// SO2_Over：SO2超标天
        /// CO_Over：CO超标天
        /// MaxOneHourO3_Over：MaxOneHourO3超标天
        /// Max8HourO3_Over：Max8HourO3超标天
        /// </returns>
        public DataView GetPointsExceedingDays(string[] portIds, DateTime dateStart, DateTime dateEnd)
        {
            pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
            if (pointDayAQI != null)
                return pointDayAQI.GetExceedingData(portIds, dateStart, dateEnd);
            return null;
        }

        /// <summary>
        /// 各项污染物日均值浓度范围
        /// </summary>
        /// <param name="regionGuids">区域Guid</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>
        /// RegionName：区域名称
        /// DateTime：日期
        /// PM25Range：PM2.5浓度
        /// PM10Range：PM10浓度
        /// NO2Range：NO2浓度
        /// SO2Range：SO2浓度
        /// CORange：CO浓度
        /// Max8HourO3Range：臭氧8小时浓度
        /// MaxOneHourO3Range：臭氧1小时浓度
        /// PM25AQI：PM2.5分指数
        /// PM10AQI：PM10分指数
        /// NO2AQI：NO2分指数
        /// SO2AQI：SO2分指数
        /// COAQI：CO分指数
        /// Max8HourO3AQI：臭氧8小时分指数
        /// MaxOneHourO3AQ：臭氧1小时分指数
        /// </returns>
        public DataView GetPointPollutantAvgDayRange(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd)
        {
            MonitoringPointAirService monitoringPoint = Singleton<MonitoringPointAirService>.GetInstance();
            DataTable dt = new DataTable();
            dt.Columns.Add("PortId", typeof(string));
            dt.Columns.Add("RegionName", typeof(string));
            foreach (string factor in factors)
            {
                dt.Columns.Add(factor + "Range", typeof(string));
            }
            dt.Columns.Add("AQI", typeof(string));

            for (int i = 0; i < portIds.Length; i++)
            {
                string PointName = monitoringPoint.RetrieveEntityByPointId(Convert.ToInt32(portIds[i])).MonitoringPointName;
                string portId = portIds[i];
                DataView MinValues = GetPointsMinValue(portIds, dateStart, dateEnd);
                DataView MaxValues = GetPointsMaxValue(portIds, dateStart, dateEnd);

                DataRow dr = dt.NewRow();
                DataRow[] drMin = MinValues.Table.Select("PointId='" + portIds[i] + "'");
                DataRow[] drMax = MaxValues.Table.Select("PointId='" + portIds[i] + "'");
                if (drMin.Length > 0 && drMax.Length > 0)
                {
                    dr["PortId"] = portId;
                    dr["RegionName"] = PointName;
                    //   dr["DateTime"] = (string.Format("{0:yyyy-MM}", DateTime.Parse((dateStart.AddMonths(j).Year + "-" + dateStart.AddMonths(j).Month).ToString())));
                    foreach (string factor in factors)
                    {
                        switch (factor)
                        {
                            case "PM25":
                                if (drMin[0]["PM25"].IsNotNullOrDBNull())
                                {
                                    if (drMax[0]["PM25"].IsNotNullOrDBNull())
                                    {
                                        decimal pm25ntemp = decimal.TryParse(drMin[0]["PM25"].ToString(), out pm25ntemp) ? pm25ntemp : 0;
                                        decimal pm25mtemp = decimal.TryParse(drMax[0]["PM25"].ToString(), out pm25mtemp) ? pm25mtemp : 0;
                                        dr["PM25Range"] = (pm25ntemp * 1000).ToString("0") + "~" + (pm25mtemp * 1000).ToString("0");
                                    }
                                    else
                                    {
                                        decimal pm25ntemp = decimal.TryParse(drMin[0]["PM25"].ToString(), out pm25ntemp) ? pm25ntemp : 0;
                                        dr["PM25Range"] = (pm25ntemp * 1000).ToString("0") + "~" + "/";
                                    }
                                }
                                else if (!drMin[0]["PM25"].IsNotNullOrDBNull())
                                {
                                    if (drMax[0]["PM25"].IsNotNullOrDBNull())
                                    {
                                        decimal pm25mtemp = decimal.TryParse(drMax[0]["PM25"].ToString(), out pm25mtemp) ? pm25mtemp : 0;
                                        dr["PM25Range"] = "/" + "~" + (pm25mtemp * 1000).ToString("0");
                                    }
                                    else
                                    {
                                        dr["PM25Range"] = "/";
                                    }
                                }
                                break;
                            case "PM10":
                                if (drMin[0]["PM10"].IsNotNullOrDBNull())
                                {
                                    if (drMax[0]["PM10"].IsNotNullOrDBNull())
                                    {
                                        decimal pm10ntemp = decimal.TryParse(drMin[0]["PM10"].ToString(), out pm10ntemp) ? pm10ntemp : 0;
                                        decimal pm10mtemp = decimal.TryParse(drMax[0]["PM10"].ToString(), out pm10mtemp) ? pm10mtemp : 0;
                                        dr["PM10Range"] = (pm10ntemp * 1000).ToString("0") + "~" + (pm10mtemp * 1000).ToString("0");
                                    }
                                    else
                                    {
                                        decimal pm10ntemp = decimal.TryParse(drMin[0]["PM10"].ToString(), out pm10ntemp) ? pm10ntemp : 0;
                                        dr["PM10Range"] = (pm10ntemp * 1000).ToString("0") + "~" + "/";
                                    }
                                }
                                else if (!drMin[0]["PM10"].IsNotNullOrDBNull())
                                {
                                    if (drMax[0]["PM10"].IsNotNullOrDBNull())
                                    {
                                        decimal pm10mtemp = decimal.TryParse(drMax[0]["PM10"].ToString(), out pm10mtemp) ? pm10mtemp : 0;
                                        dr["PM10Range"] = "/" + "~" + (pm10mtemp * 1000).ToString("0");
                                    }
                                    else
                                    {
                                        dr["PM10Range"] = "/";
                                    }
                                }
                                break;
                            case "NO2":
                                if (drMin[0]["NO2"].IsNotNullOrDBNull())
                                {
                                    if (drMax[0]["NO2"].IsNotNullOrDBNull())
                                    {
                                        decimal no2ntemp = decimal.TryParse(drMin[0]["NO2"].ToString(), out no2ntemp) ? no2ntemp : 0;
                                        decimal no2mtemp = decimal.TryParse(drMax[0]["NO2"].ToString(), out no2mtemp) ? no2mtemp : 0;
                                        dr["NO2Range"] = (no2ntemp * 1000).ToString("0") + "~" + (no2mtemp * 1000).ToString("0");
                                    }
                                    else
                                    {
                                        decimal no2ntemp = decimal.TryParse(drMin[0]["NO2"].ToString(), out no2ntemp) ? no2ntemp : 0;
                                        dr["NO2Range"] = (no2ntemp * 1000).ToString("0") + "~" + "/";
                                    }
                                }
                                else if (!drMin[0]["NO2"].IsNotNullOrDBNull())
                                {
                                    if (drMax[0]["NO2"].IsNotNullOrDBNull())
                                    {
                                        decimal no2mtemp = decimal.TryParse(drMax[0]["NO2"].ToString(), out no2mtemp) ? no2mtemp : 0;
                                        dr["NO2Range"] = "/" + "~" + (no2mtemp * 1000).ToString("0");
                                    }
                                    else
                                    {
                                        dr["NO2Range"] = "/";
                                    }
                                }
                                break;
                            case "SO2":
                                if (drMin[0]["SO2"].IsNotNullOrDBNull())
                                {
                                    if (drMax[0]["SO2"].IsNotNullOrDBNull())
                                    {
                                        decimal so2ntemp = decimal.TryParse(drMin[0]["SO2"].ToString(), out so2ntemp) ? so2ntemp : 0;
                                        decimal so2mtemp = decimal.TryParse(drMax[0]["SO2"].ToString(), out so2mtemp) ? so2mtemp : 0;
                                        dr["SO2Range"] = (so2ntemp * 1000).ToString("0") + "~" + (so2mtemp * 1000).ToString("0");
                                    }
                                    else
                                    {
                                        decimal so2ntemp = decimal.TryParse(drMin[0]["SO2"].ToString(), out so2ntemp) ? so2ntemp : 0;
                                        dr["SO2Range"] = (so2ntemp * 1000).ToString("0") + "~" + "/";
                                    }
                                }
                                else if (!drMin[0]["SO2"].IsNotNullOrDBNull())
                                {
                                    if (drMax[0]["SO2"].IsNotNullOrDBNull())
                                    {
                                        decimal so2mtemp = decimal.TryParse(drMax[0]["SO2"].ToString(), out so2mtemp) ? so2mtemp : 0;
                                        dr["SO2Range"] = "/" + "~" + (so2mtemp * 1000).ToString("0");
                                    }
                                    else
                                    {
                                        dr["SO2Range"] = "/";
                                    }
                                }
                                break;
                            case "CO":
                                if (drMin[0]["CO"].IsNotNullOrDBNull())
                                {
                                    if (drMax[0]["CO"].IsNotNullOrDBNull())
                                    {
                                        decimal corntemp = decimal.TryParse(drMin[0]["CO"].ToString(), out corntemp) ? corntemp : 0;
                                        decimal cormtemp = decimal.TryParse(drMax[0]["CO"].ToString(), out cormtemp) ? cormtemp : 0;
                                        dr["CORange"] = corntemp.ToString("0.0") + "~" + cormtemp.ToString("0.0");
                                    }
                                    else
                                    {
                                        decimal corntemp = decimal.TryParse(drMin[0]["CO"].ToString(), out corntemp) ? corntemp : 0;
                                        dr["CORange"] = corntemp.ToString("0.0") + "~" + "/";
                                    }
                                }
                                else if (!drMin[0]["CO"].IsNotNullOrDBNull())
                                {
                                    if (drMax[0]["CO"].IsNotNullOrDBNull())
                                    {
                                        decimal cormtemp = decimal.TryParse(drMax[0]["CO"].ToString(), out cormtemp) ? cormtemp : 0;
                                        dr["CORange"] = "/" + "~" + cormtemp.ToString("0.0");
                                    }
                                    else
                                    {
                                        dr["CORange"] = "/";
                                    }
                                }
                                break;
                            case "Max8HourO3":
                                if (drMin[0]["Max8HourO3"].IsNotNullOrDBNull())
                                {
                                    if (drMax[0]["Max8HourO3"].IsNotNullOrDBNull())
                                    {
                                        decimal maxntemp = decimal.TryParse(drMin[0]["Max8HourO3"].ToString(), out maxntemp) ? maxntemp : 0;
                                        decimal maxmtemp = decimal.TryParse(drMax[0]["Max8HourO3"].ToString(), out maxmtemp) ? maxmtemp : 0;
                                        dr["Max8HourO3Range"] = (maxntemp * 1000).ToString("0") + "~" + (maxmtemp * 1000).ToString("0");
                                    }
                                    else
                                    {
                                        decimal maxntemp = decimal.TryParse(drMin[0]["Max8HourO3"].ToString(), out maxntemp) ? maxntemp : 0;
                                        dr["Max8HourO3Range"] = (maxntemp * 1000).ToString("0") + "~" + "/";
                                    }
                                }
                                else if (!drMin[0]["Max8HourO3"].IsNotNullOrDBNull())
                                {
                                    if (drMax[0]["Max8HourO3"].IsNotNullOrDBNull())
                                    {
                                        decimal maxmtemp = decimal.TryParse(drMax[0]["Max8HourO3"].ToString(), out maxmtemp) ? maxmtemp : 0;
                                        dr["Max8HourO3Range"] = "/" + "~" + (maxmtemp * 1000).ToString("0");
                                    }
                                    else
                                    {
                                        dr["Max8HourO3Range"] = "/";
                                    }
                                }
                                break;
                        }
                    }
                    //if (drMin[0]["MaxOneHourO3"].IsNotNullOrDBNull())
                    //{
                    //    if (drMax[0]["MaxOneHourO3"].IsNotNullOrDBNull())
                    //    {
                    //        decimal onentemp = decimal.TryParse(drMin[0]["MaxOneHourO3"].ToString(), out onentemp) ? onentemp : 0;
                    //        decimal onemtemp = decimal.TryParse(drMax[0]["MaxOneHourO3"].ToString(), out onemtemp) ? onemtemp : 0;
                    //        dr["MaxOneHourO3Range"] = (onentemp * 1000).ToString("0") + "~" + (onemtemp * 1000).ToString("0");
                    //    }
                    //    else
                    //    {
                    //        decimal onentemp = decimal.TryParse(drMin[0]["MaxOneHourO3"].ToString(), out onentemp) ? onentemp : 0;
                    //        dr["MaxOneHourO3Range"] = (onentemp * 1000).ToString("0") + "~" + "/";
                    //    }
                    //}
                    //else if (!drMin[0]["MaxOneHourO3"].IsNotNullOrDBNull())
                    //{
                    //    if (drMax[0]["MaxOneHourO3"].IsNotNullOrDBNull())
                    //    {
                    //        decimal onemtemp = decimal.TryParse(drMax[0]["MaxOneHourO3"].ToString(), out onemtemp) ? onemtemp : 0;
                    //        dr["MaxOneHourO3Range"] = "/" + "~" + (onemtemp * 1000).ToString("0");
                    //    }
                    //    else
                    //    {
                    //        dr["MaxOneHourO3Range"] = "/";
                    //    }
                    //}
                    if (drMin[0]["AQIValue"].IsNotNullOrDBNull())
                    {
                        if (drMax[0]["AQIValue"].IsNotNullOrDBNull())
                        {
                            dr["AQI"] = Convert.ToDecimal(drMin[0]["AQIValue"]).ToString("0") + "~" + Convert.ToDecimal(drMax[0]["AQIValue"]).ToString("0");
                        }
                        else
                        {
                            dr["AQI"] = Convert.ToDecimal(drMin[0]["AQIValue"]).ToString("0") + "~/";
                        }
                    }
                    else if (!drMin[0]["AQIValue"].IsNotNullOrDBNull())
                    {
                        if (drMax[0]["AQIValue"].IsNotNullOrDBNull())
                        {
                            dr["AQI"] = "/" + "~" + Convert.ToDecimal(drMax[0]["AQIValue"]).ToString("0");
                        }
                        else
                        {
                            dr["AQI"] = "/";
                        }
                    }
                    //if (drMin[0]["PM10_IAQI"].IsNotNullOrDBNull())
                    //{
                    //    if (drMax[0]["PM10_IAQI"].IsNotNullOrDBNull())
                    //    {
                    //        dr["PM10AQI"] = Convert.ToDecimal(drMin[0]["PM10_IAQI"]).ToString("0") + "~" + Convert.ToDecimal(drMax[0]["PM10_IAQI"]).ToString("0");
                    //    }
                    //    else
                    //    {
                    //        dr["PM10AQI"] = Convert.ToDecimal(drMin[0]["PM10_IAQI"]).ToString("0") + "~/";
                    //    }
                    //}
                    //else if (!drMin[0]["PM10_IAQI"].IsNotNullOrDBNull())
                    //{
                    //    if (drMax[0]["PM10_IAQI"].IsNotNullOrDBNull())
                    //    {
                    //        dr["PM10AQI"] = "/" + "~" + Convert.ToDecimal(drMax[0]["PM10_IAQI"]).ToString("0");
                    //    }
                    //    else
                    //    {
                    //        dr["PM10AQI"] = "/";
                    //    }
                    //}
                    //if (drMin[0]["NO2_IAQI"].IsNotNullOrDBNull())
                    //{
                    //    if (drMax[0]["NO2_IAQI"].IsNotNullOrDBNull())
                    //    {
                    //        dr["NO2AQI"] = Convert.ToDecimal(drMin[0]["NO2_IAQI"]).ToString("0") + "~" + Convert.ToDecimal(drMax[0]["NO2_IAQI"]).ToString("0");
                    //    }
                    //    else
                    //    {
                    //        dr["NO2AQI"] = Convert.ToDecimal(drMin[0]["NO2_IAQI"]).ToString("0") + "~/";
                    //    }
                    //}
                    //else if (!drMin[0]["NO2_IAQI"].IsNotNullOrDBNull())
                    //{
                    //    if (drMax[0]["NO2_IAQI"].IsNotNullOrDBNull())
                    //    {
                    //        dr["NO2AQI"] = "/" + "~" + Convert.ToDecimal(drMax[0]["NO2_IAQI"]).ToString("0");
                    //    }
                    //    else
                    //    {
                    //        dr["NO2AQI"] = "/";
                    //    }
                    //}
                    //if (drMin[0]["SO2_IAQI"].IsNotNullOrDBNull())
                    //{
                    //    if (drMax[0]["SO2_IAQI"].IsNotNullOrDBNull())
                    //    {
                    //        dr["SO2AQI"] = Convert.ToDecimal(drMin[0]["SO2_IAQI"]).ToString("0") + "~" + Convert.ToDecimal(drMax[0]["SO2_IAQI"]).ToString("0");
                    //    }
                    //    else
                    //    {
                    //        dr["SO2AQI"] = Convert.ToDecimal(drMin[0]["SO2_IAQI"]).ToString("0") + "~/";
                    //    }
                    //}
                    //else if (!drMin[0]["SO2_IAQI"].IsNotNullOrDBNull())
                    //{
                    //    if (drMax[0]["SO2_IAQI"].IsNotNullOrDBNull())
                    //    {
                    //        dr["SO2AQI"] = "/" + "~" + Convert.ToDecimal(drMax[0]["SO2_IAQI"]).ToString("0");
                    //    }
                    //    else
                    //    {
                    //        dr["SO2AQI"] = "/";
                    //    }
                    //}
                    //if (drMin[0]["CO_IAQI"].IsNotNullOrDBNull())
                    //{
                    //    if (drMax[0]["CO_IAQI"].IsNotNullOrDBNull())
                    //    {
                    //        dr["COAQI"] = Convert.ToDecimal(drMin[0]["CO_IAQI"]).ToString("0") + "~" + Convert.ToDecimal(drMax[0]["CO_IAQI"]).ToString("0");
                    //    }
                    //    else
                    //    {
                    //        dr["COAQI"] = Convert.ToDecimal(drMin[0]["CO_IAQI"]).ToString("0") + "~/";
                    //    }
                    //}
                    //else if (!drMin[0]["CO_IAQI"].IsNotNullOrDBNull())
                    //{
                    //    if (drMax[0]["CO_IAQI"].IsNotNullOrDBNull())
                    //    {
                    //        dr["COAQI"] = "/" + "~" + Convert.ToDecimal(drMax[0]["CO_IAQI"]).ToString("0");
                    //    }
                    //    else
                    //    {
                    //        dr["COAQI"] = "/";
                    //    }
                    //}
                    //if (drMin[0]["Max8HourO3_IAQI"].IsNotNullOrDBNull())
                    //{
                    //    if (drMax[0]["Max8HourO3_IAQI"].IsNotNullOrDBNull())
                    //    {
                    //        dr["Max8HourO3AQI"] = Convert.ToDecimal(drMin[0]["Max8HourO3_IAQI"]).ToString("0") + "~" + Convert.ToDecimal(drMax[0]["Max8HourO3_IAQI"]).ToString("0");
                    //    }
                    //    else
                    //    {
                    //        dr["Max8HourO3AQI"] = Convert.ToDecimal(drMin[0]["Max8HourO3_IAQI"]).ToString("0") + "~/";
                    //    }
                    //}
                    //else if (!drMin[0]["Max8HourO3_IAQI"].IsNotNullOrDBNull())
                    //{
                    //    if (drMax[0]["Max8HourO3_IAQI"].IsNotNullOrDBNull())
                    //    {
                    //        dr["Max8HourO3AQI"] = "/" + "~" + Convert.ToDecimal(drMax[0]["Max8HourO3_IAQI"]).ToString("0");
                    //    }
                    //    else
                    //    {
                    //        dr["Max8HourO3AQI"] = "/";
                    //    }
                    //}
                    //if (drMin[0]["MaxOneHourO3_IAQI"].IsNotNullOrDBNull())
                    //{
                    //    if (drMax[0]["MaxOneHourO3_IAQI"].IsNotNullOrDBNull())
                    //    {
                    //        dr["MaxOneHourO3AQI"] = Convert.ToDecimal(drMin[0]["MaxOneHourO3_IAQI"]).ToString("0") + "~" + Convert.ToDecimal(drMax[0]["MaxOneHourO3_IAQI"]).ToString("0");
                    //    }
                    //    else
                    //    {
                    //        dr["MaxOneHourO3AQI"] = Convert.ToDecimal(drMin[0]["MaxOneHourO3_IAQI"]).ToString("0") + "~/";
                    //    }
                    //}
                    //else if (!drMin[0]["MaxOneHourO3_IAQI"].IsNotNullOrDBNull())
                    //{
                    //    if (drMax[0]["MaxOneHourO3_IAQI"].IsNotNullOrDBNull())
                    //    {
                    //        dr["MaxOneHourO3AQI"] = "/" + "~" + Convert.ToDecimal(drMax[0]["MaxOneHourO3_IAQI"]).ToString("0");
                    //    }
                    //    else
                    //    {
                    //        dr["MaxOneHourO3AQI"] = "/";
                    //    }
                    //}

                    dt.Rows.Add(dr);
                }
            }

            return dt.AsDataView();
        }

        /// <summary>
        /// 各项污染物日均值浓度范围统计行
        /// </summary>
        /// <param name="regionGuids">区域Guid</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>
        public DataView GetPointStatisticalRange(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd)
        {
            MonitoringPointAirService monitoringPoint = Singleton<MonitoringPointAirService>.GetInstance();
            DataTable dt = new DataTable();
            dt.Columns.Add("RegionName", typeof(string));
            foreach (string factor in factors)
            {
                dt.Columns.Add(factor + "Range", typeof(string));
            }
            string PointName = monitoringPoint.RetrieveEntityByPointId(Convert.ToInt32(portIds[0])).MonitoringPointName;
            string PointNames = "";
            for (int i = 0; i < portIds.Length; i++)
            {
                PointNames = monitoringPoint.RetrieveEntityByPointId(Convert.ToInt32(portIds[i])).MonitoringPointName;
            }
            DataView MinValues = GetPointsMinValueOne(portIds, dateStart, dateEnd);
            DataView MaxValues = GetPointsMaxValueOne(portIds, dateStart, dateEnd);

            DataRow dr = dt.NewRow();
            DataRow[] drMin = MinValues.Table.Select();
            DataRow[] drMax = MaxValues.Table.Select();
            if (drMin.Length > 0 && drMax.Length > 0)
            {
                dr["RegionName"] = PointName + "~" + PointNames;
                foreach (string factor in factors)
                {
                    switch (factor)
                    {
                        case "PM25":
                            if (drMin[0]["PM25"].IsNotNullOrDBNull())
                            {
                                if (drMax[0]["PM25"].IsNotNullOrDBNull())
                                {
                                    decimal pm25ntemp = decimal.TryParse(drMin[0]["PM25"].ToString(), out pm25ntemp) ? pm25ntemp : 0;
                                    decimal pm25mtemp = decimal.TryParse(drMax[0]["PM25"].ToString(), out pm25mtemp) ? pm25mtemp : 0;
                                    dr["PM25Range"] = (pm25ntemp * 1000).ToString("0") + "~" + (pm25mtemp * 1000).ToString("0");
                                }
                                else
                                {
                                    decimal pm25ntemp = decimal.TryParse(drMin[0]["PM25"].ToString(), out pm25ntemp) ? pm25ntemp : 0;
                                    dr["PM25Range"] = (pm25ntemp * 1000).ToString("0") + "~" + "/";
                                }
                            }
                            else if (!drMin[0]["PM25"].IsNotNullOrDBNull())
                            {
                                if (drMax[0]["PM25"].IsNotNullOrDBNull())
                                {
                                    decimal pm25mtemp = decimal.TryParse(drMax[0]["PM25"].ToString(), out pm25mtemp) ? pm25mtemp : 0;
                                    dr["PM25Range"] = "/" + "~" + (pm25mtemp * 1000).ToString("0");
                                }
                                else
                                {
                                    dr["PM25Range"] = "/";
                                }
                            }
                            break;
                        case "PM10":
                            if (drMin[0]["PM10"].IsNotNullOrDBNull())
                            {
                                if (drMax[0]["PM10"].IsNotNullOrDBNull())
                                {
                                    decimal pm10ntemp = decimal.TryParse(drMin[0]["PM10"].ToString(), out pm10ntemp) ? pm10ntemp : 0;
                                    decimal pm10mtemp = decimal.TryParse(drMax[0]["PM10"].ToString(), out pm10mtemp) ? pm10mtemp : 0;
                                    dr["PM10Range"] = (pm10ntemp * 1000).ToString("0") + "~" + (pm10mtemp * 1000).ToString("0");
                                }
                                else
                                {
                                    decimal pm10ntemp = decimal.TryParse(drMin[0]["PM10"].ToString(), out pm10ntemp) ? pm10ntemp : 0;
                                    dr["PM10Range"] = (pm10ntemp * 1000).ToString("0") + "~" + "/";
                                }
                            }
                            else if (!drMin[0]["PM10"].IsNotNullOrDBNull())
                            {
                                if (drMax[0]["PM10"].IsNotNullOrDBNull())
                                {
                                    decimal pm10mtemp = decimal.TryParse(drMax[0]["PM10"].ToString(), out pm10mtemp) ? pm10mtemp : 0;
                                    dr["PM10Range"] = "/" + "~" + (pm10mtemp * 1000).ToString("0");
                                }
                                else
                                {
                                    dr["PM10Range"] = "/";
                                }
                            }
                            break;
                        case "NO2":
                            if (drMin[0]["NO2"].IsNotNullOrDBNull())
                            {
                                if (drMax[0]["NO2"].IsNotNullOrDBNull())
                                {
                                    decimal no2ntemp = decimal.TryParse(drMin[0]["NO2"].ToString(), out no2ntemp) ? no2ntemp : 0;
                                    decimal no2mtemp = decimal.TryParse(drMax[0]["NO2"].ToString(), out no2mtemp) ? no2mtemp : 0;
                                    dr["NO2Range"] = (no2ntemp * 1000).ToString("0") + "~" + (no2mtemp * 1000).ToString("0");
                                }
                                else
                                {
                                    decimal no2ntemp = decimal.TryParse(drMin[0]["NO2"].ToString(), out no2ntemp) ? no2ntemp : 0;
                                    dr["NO2Range"] = (no2ntemp * 1000).ToString("0") + "~" + "/";
                                }
                            }
                            else if (!drMin[0]["NO2"].IsNotNullOrDBNull())
                            {
                                if (drMax[0]["NO2"].IsNotNullOrDBNull())
                                {
                                    decimal no2mtemp = decimal.TryParse(drMax[0]["NO2"].ToString(), out no2mtemp) ? no2mtemp : 0;
                                    dr["NO2Range"] = "/" + "~" + (no2mtemp * 1000).ToString("0");
                                }
                                else
                                {
                                    dr["NO2Range"] = "/";
                                }
                            }
                            break;
                        case "SO2":
                            if (drMin[0]["SO2"].IsNotNullOrDBNull())
                            {
                                if (drMax[0]["SO2"].IsNotNullOrDBNull())
                                {
                                    decimal so2ntemp = decimal.TryParse(drMin[0]["SO2"].ToString(), out so2ntemp) ? so2ntemp : 0;
                                    decimal so2mtemp = decimal.TryParse(drMax[0]["SO2"].ToString(), out so2mtemp) ? so2mtemp : 0;
                                    dr["SO2Range"] = (so2ntemp * 1000).ToString("0") + "~" + (so2mtemp * 1000).ToString("0");
                                }
                                else
                                {
                                    decimal so2ntemp = decimal.TryParse(drMin[0]["SO2"].ToString(), out so2ntemp) ? so2ntemp : 0;
                                    dr["SO2Range"] = (so2ntemp * 1000).ToString("0") + "~" + "/";
                                }
                            }
                            else if (!drMin[0]["SO2"].IsNotNullOrDBNull())
                            {
                                if (drMax[0]["SO2"].IsNotNullOrDBNull())
                                {
                                    decimal so2mtemp = decimal.TryParse(drMax[0]["SO2"].ToString(), out so2mtemp) ? so2mtemp : 0;
                                    dr["SO2Range"] = "/" + "~" + (so2mtemp * 1000).ToString("0");
                                }
                                else
                                {
                                    dr["SO2Range"] = "/";
                                }
                            }
                            break;
                        case "CO":
                            if (drMin[0]["CO"].IsNotNullOrDBNull())
                            {
                                if (drMax[0]["CO"].IsNotNullOrDBNull())
                                {
                                    decimal corntemp = decimal.TryParse(drMin[0]["CO"].ToString(), out corntemp) ? corntemp : 0;
                                    decimal cormtemp = decimal.TryParse(drMax[0]["CO"].ToString(), out cormtemp) ? cormtemp : 0;
                                    dr["CORange"] = corntemp.ToString("0.0") + "~" + cormtemp.ToString("0.0");
                                }
                                else
                                {
                                    decimal corntemp = decimal.TryParse(drMin[0]["CO"].ToString(), out corntemp) ? corntemp : 0;
                                    dr["CORange"] = corntemp.ToString("0.0") + "~" + "/";
                                }
                            }
                            else if (!drMin[0]["CO"].IsNotNullOrDBNull())
                            {
                                if (drMax[0]["CO"].IsNotNullOrDBNull())
                                {
                                    decimal cormtemp = decimal.TryParse(drMax[0]["CO"].ToString(), out cormtemp) ? cormtemp : 0;
                                    dr["CORange"] = "/" + "~" + cormtemp.ToString("0.0");
                                }
                                else
                                {
                                    dr["CORange"] = "/";
                                }
                            }
                            break;
                        case "Max8HourO3":
                            if (drMin[0]["Max8HourO3"].IsNotNullOrDBNull())
                            {
                                if (drMax[0]["Max8HourO3"].IsNotNullOrDBNull())
                                {
                                    decimal maxntemp = decimal.TryParse(drMin[0]["Max8HourO3"].ToString(), out maxntemp) ? maxntemp : 0;
                                    decimal maxmtemp = decimal.TryParse(drMax[0]["Max8HourO3"].ToString(), out maxmtemp) ? maxmtemp : 0;
                                    dr["Max8HourO3Range"] = (maxntemp * 1000).ToString("0") + "~" + (maxmtemp * 1000).ToString("0");
                                }
                                else
                                {
                                    decimal maxntemp = decimal.TryParse(drMin[0]["Max8HourO3"].ToString(), out maxntemp) ? maxntemp : 0;
                                    dr["Max8HourO3Range"] = (maxntemp * 1000).ToString("0") + "~" + "/";
                                }
                            }
                            else if (!drMin[0]["Max8HourO3"].IsNotNullOrDBNull())
                            {
                                if (drMax[0]["Max8HourO3"].IsNotNullOrDBNull())
                                {
                                    decimal maxmtemp = decimal.TryParse(drMax[0]["Max8HourO3"].ToString(), out maxmtemp) ? maxmtemp : 0;
                                    dr["Max8HourO3Range"] = "/" + "~" + (maxmtemp * 1000).ToString("0");
                                }
                                else
                                {
                                    dr["Max8HourO3Range"] = "/";
                                }
                            }
                            break;
                    }
                }
                dt.Rows.Add(dr);
            }
            return dt.AsDataView();
        }
        /// <summary>
        /// 站点污染持续天数及污染程度简表
        /// </summary>
        /// <param name="portsId">站点</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>
        /// 返回字段：DateTime，ContinuousDays，LightPollution，ModeratePollution，HighPollution，SeriousPollution
        /// </returns>
        public DataView GetPortsContinuousDaysTable(string[] portsId, DateTime dateStart, DateTime dateEnd)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("DateTime", typeof(string));
            dt.Columns.Add("ContinuousDays", typeof(int));
            dt.Columns.Add("LightPollution", typeof(int));
            dt.Columns.Add("ModeratePollution", typeof(int));
            dt.Columns.Add("HighPollution", typeof(int));
            dt.Columns.Add("SeriousPollution", typeof(int));
            dt.Columns.Add("LightPollutionDay", typeof(string));
            dt.Columns.Add("ModeratePollutionDay", typeof(string));
            dt.Columns.Add("HighPollutionDay", typeof(string));
            dt.Columns.Add("SeriousPollutionDay", typeof(string));
            int record = 0;
            DataTable AllData = GetPortDataPager(portsId, dateStart, dateEnd, 999999999, 0, out record, "DateTime Asc").Table;
            DataTable NewExceedingDays = AllData.Clone();
            DataRow[] AllExceedingDays = AllData.Select("convert(AQIValue,'System.Int32')>100");

            if (AllExceedingDays.Length > 0)
            {
                NewExceedingDays = AllExceedingDays.CopyToDataTable();
                DataView dv = NewExceedingDays.DefaultView;
                dv.Sort = "DateTime asc";
                List<List<DateTime>> ContinuousDaysList = new List<List<DateTime>>();
                List<DateTime> ContinuousDays = new List<DateTime>();

                for (int i = 1; i < dv.Count; i++)
                {
                    DateTime firstValue = Convert.ToDateTime(dv[i - 1]["DateTime"]);
                    DateTime secondValue = Convert.ToDateTime(dv[i]["DateTime"]);
                    int poor = (secondValue - firstValue).Days;
                    if (poor.Equals(1))
                    {
                        if (!ContinuousDays.Contains(firstValue))
                        {
                            ContinuousDays.Add(firstValue);
                        }
                        if (!ContinuousDays.Contains(secondValue))
                        {
                            ContinuousDays.Add(secondValue);
                        }
                        if (i == dv.Count - 1)
                        {
                            ContinuousDaysList.Add(ContinuousDays);
                        }
                    }
                    else
                    {
                        if (ContinuousDays.Count > 0)
                        {
                            ContinuousDaysList.Add(ContinuousDays);
                        }
                        ContinuousDays = new List<DateTime>();
                    }
                }

                for (int i = 0; i < ContinuousDaysList.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    List<DateTime> dateTimeArray = ContinuousDaysList[i];
                    DataTable ExceedingDays = NewExceedingDays.Clone();
                    ExceedingDays = NewExceedingDays.Select("DateTime>='" + dateTimeArray[0] + "' and DateTime<='" + dateTimeArray[dateTimeArray.Count - 1] + "'").CopyToDataTable();
                    DataRow[] LightPollution = ExceedingDays.Select("convert(AQIValue,'System.Int32')>=101 and convert(AQIValue,'System.Int32')<=150", "DateTime");
                    DataRow[] ModeratePollution = ExceedingDays.Select("convert(AQIValue,'System.Int32')>=151 and convert(AQIValue,'System.Int32')<=200", "DateTime");
                    DataRow[] HighPollution = ExceedingDays.Select("convert(AQIValue,'System.Int32')>=201 and convert(AQIValue,'System.Int32')<=300", "DateTime");
                    DataRow[] SeriousPollution = ExceedingDays.Select("convert(AQIValue,'System.Int32')>300", "DateTime");

                    int outDays = dateTimeArray.Count;
                    int LightPollutionDays = LightPollution.Length;
                    int ModeratePollutionDays = ModeratePollution.Length;
                    int HighPollutionDays = HighPollution.Length;
                    int SeriousPollutionDays = SeriousPollution.Length;
                    string LightPollutionday = "";
                    foreach (DataRow light in LightPollution)
                    {
                        if (light["DateTime"].IsNotNullOrDBNull())
                        {
                            LightPollutionday += light["DateTime"] + "\n";
                        }
                    }
                    string ModeratePollutionday = "";
                    foreach (DataRow Moderate in ModeratePollution)
                    {
                        if (Moderate["DateTime"].IsNotNullOrDBNull())
                        {
                            ModeratePollutionday += Moderate["DateTime"] + "\n";
                        }
                    }
                    string HighPollutionday = "";
                    foreach (DataRow High in HighPollution)
                    {
                        if (High["DateTime"].IsNotNullOrDBNull())
                        {
                            HighPollutionday += High["DateTime"] + "\n";
                        }
                    }
                    string SeriousPollutionday = "";
                    foreach (DataRow Serious in SeriousPollution)
                    {
                        if (Serious["DateTime"].IsNotNullOrDBNull())
                        {
                            SeriousPollutionday += Serious["DateTime"] + "\n";
                        }
                    }
                    dr["DateTime"] = dateTimeArray[0].ToString("yyyy-MM-dd") + "—" + dateTimeArray[dateTimeArray.Count - 1].ToString("yyyy-MM-dd");
                    dr["ContinuousDays"] = outDays;
                    dr["LightPollution"] = LightPollutionDays;
                    dr["ModeratePollution"] = ModeratePollutionDays;
                    dr["HighPollution"] = HighPollutionDays;
                    dr["SeriousPollution"] = SeriousPollutionDays;
                    dr["LightPollutionDay"] = LightPollutionday;
                    dr["ModeratePollutionDay"] = ModeratePollutionday;
                    dr["HighPollutionDay"] = HighPollutionday;
                    dr["SeriousPollutionDay"] = SeriousPollutionday;
                    dt.Rows.Add(dr);
                }
                //return dt.DefaultView;
            }
            return dt.DefaultView;
        }

        /// <summary>
        /// 获取不同站点污染情况分析数据
        /// </summary>
        /// <param name="portsId">站点</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <param name="AQIType">污染因子</param>
        /// <param name="pollutionGrade">污染等级</param>
        /// <returns>returnDataTable</returns>
        public DataView GetPortsPollution(int portsId, DateTime[,] dateTime, int pollutionClass)
        {
            pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
            List<List<DayAQIEntity>> AllData = new List<List<DayAQIEntity>>();
            DataTable dt = new DataTable();
            for (int i = 0; i < dateTime.GetLength(0); i++)
            {
                dt.Columns.Add(dateTime[i, 0].Year.ToString() + "(" + i + ")", typeof(string));
                dt.Columns.Add("AQIValue" + "(" + i + ")", typeof(string));
                dt.Columns.Add("PrimaryPollutant" + "(" + i + ")", typeof(string));
                dt.Columns.Add("Grade" + "(" + i + ")", typeof(string));

                DateTime dateStart = dateTime[i, 0];
                DateTime dateEnd = dateTime[i, 1];
                List<DayAQIEntity> data = pointDayAQI.Retrieve(p => p.PointId == portsId && p.DateTime >= dateStart && p.DateTime <= dateEnd && p.Class == pollutionClass.ToString()).ToList<DayAQIEntity>();
                AllData.Add(data);
            }

            if (AllData.Count > 0)
            {
                int Max = AllData[0].Count;
                for (int i = 1; i < AllData.Count; i++)
                {
                    if (AllData[i].Count > Max)
                    {
                        Max = AllData[i].Count;
                    }
                }

                for (int i = 0; i < Max; i++)
                {
                    DataRow dr = dt.NewRow();
                    for (int k = 0; k < dt.Columns.Count; k++)
                    {
                        dr[k] = "";
                    }
                    dt.Rows.Add(dr);
                }

                if (dateTime.GetLength(0) == 1)
                {
                    List<DayAQIEntity> data1 = AllData[0];
                    for (int i = 0; i < data1.Count; i++)
                    {
                        dt.Rows[i][0] = data1[i].DateTime.ToString();
                        dt.Rows[i][1] = data1[i].AQIValue.ToString();
                        dt.Rows[i][2] = data1[i].PrimaryPollutant.ToString();
                        dt.Rows[i][3] = data1[i].Class.ToString();
                    }
                }

                if (dateTime.GetLength(0) == 2)
                {
                    List<DayAQIEntity> data1 = AllData[0];
                    List<DayAQIEntity> data2 = AllData[1];
                    for (int i = 0; i < data1.Count; i++)
                    {
                        dt.Rows[i][0] = data1[i].DateTime.ToString();
                        dt.Rows[i][1] = data1[i].AQIValue.ToString();
                        dt.Rows[i][2] = data1[i].PrimaryPollutant.ToString();
                        dt.Rows[i][3] = data1[i].Class.ToString();
                    }
                    for (int i = 0; i < data2.Count; i++)
                    {
                        dt.Rows[i][0] = data2[i].DateTime.ToString();
                        dt.Rows[i][1] = data2[i].AQIValue.ToString();
                        dt.Rows[i][2] = data2[i].PrimaryPollutant.ToString();
                        dt.Rows[i][3] = data2[i].Class.ToString();
                    }
                }

                if (dateTime.GetLength(0) == 3)
                {
                    List<DayAQIEntity> data1 = AllData[0];
                    List<DayAQIEntity> data2 = AllData[1];
                    List<DayAQIEntity> data3 = AllData[2];
                    for (int i = 0; i < data1.Count; i++)
                    {
                        dt.Rows[i][0] = data1[i].DateTime.ToString();
                        dt.Rows[i][1] = data1[i].AQIValue.ToString();
                        dt.Rows[i][2] = data1[i].PrimaryPollutant.ToString();
                        dt.Rows[i][3] = data1[i].Class.ToString();
                    }
                    for (int i = 0; i < data2.Count; i++)
                    {
                        dt.Rows[i][4] = data2[i].DateTime.ToString();
                        dt.Rows[i][5] = data2[i].AQIValue.ToString();
                        dt.Rows[i][6] = data2[i].PrimaryPollutant.ToString();
                        dt.Rows[i][7] = data2[i].Class.ToString();
                    }
                    for (int i = 0; i < data3.Count; i++)
                    {
                        dt.Rows[i][8] = data3[i].DateTime.ToString();
                        dt.Rows[i][9] = data3[i].AQIValue.ToString();
                        dt.Rows[i][10] = data3[i].PrimaryPollutant.ToString();
                        dt.Rows[i][11] = data3[i].Class.ToString();
                    }
                }
            }
            return dt.DefaultView;
        }

        /// <summary>
        /// 获取测点统计数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="dtDayAvgData"></param>
        /// <param name="factorList"></param>
        /// <param name="factorField"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataTable GetPortsStatisticalData(string[] portIds, DataTable dtDayAvgData, List<SmartEP.Core.Interfaces.IPollutant> factorList, string[] factorField, DateTime dtStart, DateTime dtEnd)
        {
            DataQueryByDayService dataByDayService = new DataQueryByDayService();//除臭氧外其他因子统计数据
            EQIConcentrationService EQIService = new EQIConcentrationService();

            DataTable dt = new DataTable();
            dt.Columns.Add("PointID");
            dt.Columns.Add("Statistical");
            DataView dvStatistical = dataByDayService.GetDayStatisticalData(portIds, factorList, dtStart, dtEnd);
            DataView dtAQIAvg = GetPointsAvgValue(portIds, dtStart, dtEnd);
            DataView dtAQIMax = GetPointsMaxValue(portIds, dtStart, dtEnd);
            DataView dtAQIMin = GetPointsMinValue(portIds, dtStart, dtEnd);
            DataView dtAQICount = GetPointsCountValue(portIds, dtStart, dtEnd);
            DataView dataDV = dtDayAvgData.DefaultView;
            for (int j = 0; j < portIds.Length; j++)
            {
                dataDV.RowFilter = "";
                dataDV.RowFilter = "Pointid=" + portIds[j];
                DataRow rowAvg = dt.NewRow();//平均值
                DataRow rowMax = dt.NewRow(); //最大值             
                DataRow rowMin = dt.NewRow();//最小值              
                DataRow rowSample = dt.NewRow();//样本数
                DataRow rowIAQI = dt.NewRow();//IAQI
                DataRow rowBaseUpper = dt.NewRow();//标准限值
                DataRow rowDayAvg = dt.NewRow();//日均值超标数
                DataRow rowOver = dt.NewRow();//超标率
                DataRow rowMaxOver = dt.NewRow();//最大日平均超标倍数

                rowAvg["PointID"] = portIds[j];
                rowAvg["Statistical"] = "平均值";
                rowMax["PointID"] = portIds[j];
                rowMax["Statistical"] = "最大值";
                rowMin["PointID"] = portIds[j];
                rowMin["Statistical"] = "最小值";
                rowSample["PointID"] = portIds[j];
                rowSample["Statistical"] = "样本数";
                rowIAQI["PointID"] = portIds[j];
                rowIAQI["Statistical"] = "IAQI";
                rowBaseUpper["PointID"] = portIds[j];
                rowBaseUpper["Statistical"] = "标准限值";
                rowDayAvg["PointID"] = portIds[j];
                rowDayAvg["Statistical"] = "日均值超标数";
                rowOver["PointID"] = portIds[j];
                rowOver["Statistical"] = "超标率";
                rowMaxOver["PointID"] = portIds[j];
                rowMaxOver["Statistical"] = "最大日平均超标倍数";
                for (int i = 0; i < factorField.Length; i++)
                {
                    if (j == 0)
                        dt.Columns.Add(factorField[i]);
                    EQIConcentrationLimitEntity limitTwo = new EQIConcentrationLimitEntity();
                    #region 平均值、最大值、最小值、样本数
                    if (factorField.Equals("MaxOneHourO3") || factorField.Equals("Max8HourO3"))//臭氧1小时，臭氧8小时处理
                    {
                        dtAQIAvg.RowFilter = "PointID=" + portIds[j];//平均值
                        dtAQIMax.RowFilter = "PointID=" + portIds[j];//最大值
                        dtAQIMin.RowFilter = "PointID=" + portIds[j];//最小值
                        dtAQICount.RowFilter = "PointID=" + portIds[j];//样本数
                        SmartEP.Core.Interfaces.IPollutant fac = factorList.Where(x => x.PollutantCode.Equals("a05024")).FirstOrDefault();
                        int pollutantDecimalNum = fac != null ? Convert.ToInt32(fac.PollutantDecimalNum) : 3;
                        if (dtAQIAvg.Count > 0 && dtAQIAvg[0][factorField[i]] != DBNull.Value)
                        {
                            rowAvg[factorField[i]] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dtAQIAvg[0][factorField[i]]), pollutantDecimalNum);
                        }
                        if (dtAQIMax.Count > 0 && dtAQIMax[0][factorField[i]] != DBNull.Value)
                        {
                            rowMax[factorField[i]] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dtAQIMax[0][factorField[i]]), pollutantDecimalNum);
                        }
                        if (dtAQIMin.Count > 0 && dtAQIMin[0][factorField[i]] != DBNull.Value)
                        {
                            rowMin[factorField[i]] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dtAQIMin[0][factorField[i]]), pollutantDecimalNum);
                        }
                        if (dtAQICount.Count > 0)
                        {
                            rowSample[factorField[i]] = dtAQICount[0][factorField[i]];
                        }
                        if (factorField.Equals("MaxOneHourO3"))
                            limitTwo = EQIService.RetrieveAirConcentration(AQIClass.Moderate, "a05024", EQITimeType.One);
                        else
                            limitTwo = EQIService.RetrieveAirConcentration(AQIClass.Moderate, "a05024", EQITimeType.Eight);
                    }
                    else //其他因子处理
                    {
                        dvStatistical.RowFilter = "PointID=" + portIds[j] + " and PollutantCode='" + factorField[i] + "'";
                        SmartEP.Core.Interfaces.IPollutant fac = factorList.Where(x => x.PollutantCode.Equals(factorField[i])).FirstOrDefault();
                        int pollutantDecimalNum = fac != null ? Convert.ToInt32(fac.PollutantDecimalNum) : 3;
                        if (dvStatistical.Count > 0)
                        {
                            if (dvStatistical[0]["Value_Avg"] != DBNull.Value)
                            {
                                rowAvg[factorField[i]] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dvStatistical[0]["Value_Avg"]), pollutantDecimalNum); ;//平均值
                            }
                            if (dvStatistical[0]["Value_Max"] != DBNull.Value)
                            {
                                rowMax[factorField[i]] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dvStatistical[0]["Value_Max"]), pollutantDecimalNum); ;//最大值
                            }
                            if (dvStatistical[0]["Value_Min"] != DBNull.Value)
                            {
                                rowMin[factorField[i]] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dvStatistical[0]["Value_Min"]), pollutantDecimalNum);//最小值
                            }
                            rowSample[factorField[i]] = dvStatistical[0]["Value_Count"];//样本数
                        }
                        if (factorField[i].Equals("a05024"))
                            limitTwo = EQIService.RetrieveAirConcentration(AQIClass.Moderate, factorField[i], EQITimeType.Eight);
                        else
                            limitTwo = EQIService.RetrieveAirConcentration(AQIClass.Moderate, factorField[i], EQITimeType.TwentyFour);

                    }
                    #endregion

                    #region 标准限值、日均值超标数
                    double baseValue = 0;
                    if (limitTwo != null)
                    {
                        if (limitTwo.Low != null && limitTwo.Upper != null)
                        {
                            //标准限值
                            rowBaseUpper[factorField[i]] = limitTwo.Low.Value.ToString("0.00") + "~" + limitTwo.Upper.Value.ToString("0.00");
                            baseValue = Convert.ToDouble(limitTwo.Low.Value);
                            //日均值超标数
                            if (dataDV.ToTable().Compute("Count(" + factorField[i] + ")", factorField[i] + "<" + limitTwo.Low + " or " + factorField[i] + ">" + limitTwo.Upper) != DBNull.Value)
                                rowDayAvg[factorField[i]] = Convert.ToInt32(dataDV.ToTable().Compute("Count(" + factorField[i] + ")", "" + factorField[i] + "<" + limitTwo.Low + " or " + factorField[i] + ">" + limitTwo.Upper));
                            else
                                rowDayAvg[factorField[i]] = 0;
                        }
                        else if (limitTwo.Low != null)
                        {
                            rowBaseUpper[factorField[i]] = limitTwo.Low.Value.ToString("0.00");
                            baseValue = Convert.ToDouble(limitTwo.Low.Value);
                            //日均值超标数
                            if (dataDV.ToTable().Compute("Count(" + factorField[i] + ")", factorField[i] + "<" + limitTwo.Low) != DBNull.Value)
                                rowDayAvg[factorField[i]] = Convert.ToInt32(dataDV.ToTable().Compute("Count(" + factorField[i] + ")", "" + factorField[i] + "<" + limitTwo.Low));
                            else
                                rowDayAvg[factorField[i]] = 0;
                        }
                        else if (limitTwo.Upper != null)
                        {
                            rowBaseUpper[factorField[i]] = limitTwo.Upper.Value.ToString("0.00");
                            baseValue = Convert.ToDouble(limitTwo.Upper.Value);

                            //日均值超标数
                            if (dataDV.ToTable().Compute("Count(" + factorField[i] + ")", factorField[i] + ">" + limitTwo.Upper) != DBNull.Value)
                                rowDayAvg[factorField[i]] = Convert.ToInt32(dataDV.ToTable().Compute("Count(" + factorField[i] + ")", factorField[i] + ">" + limitTwo.Upper));
                            else
                                rowDayAvg[factorField[i]] = 0;
                        }
                    }
                    else
                    {
                        rowDayAvg[factorField[i]] = 0;
                    }
                    #endregion

                    #region 超标率
                    if (!rowDayAvg[factorField[i]].ToString().Equals("--"))
                    {
                        if (rowSample[factorField[i]] != DBNull.Value && !rowSample[factorField[i]].Equals("0"))
                        {
                            if ((Convert.ToDouble(rowDayAvg[factorField[i]]) / Convert.ToDouble(rowSample[factorField[i]])) > 1)
                                rowOver[factorField[i]] = "100.0%";
                            else
                                rowOver[factorField[i]] = ((Convert.ToDouble(rowDayAvg[factorField[i]]) / Convert.ToDouble(rowSample[factorField[i]]) * 100)).ToString("0.0") + "%";
                        }
                        else
                            rowOver[factorField[i]] = "0.0%";
                    }
                    #endregion

                    #region 最大日平均超标倍数
                    if (rowMax[factorField[i]] != DBNull.Value && baseValue != 0)
                    {
                        //object aa = rowMax[factorField[i]];
                        //if ((Convert.ToDouble(rowMax[factorField[i]]) - baseValue) > 0)
                        rowMaxOver[factorField[i]] = System.Math.Abs(((Convert.ToDouble(rowMax[factorField[i]]) - baseValue) / baseValue)).ToString("0.0");
                        //else
                        //    rowMaxOver[factorField[i]] = "0.00";
                    }
                    else
                        rowMaxOver[factorField[i]] = "0.00";
                    #endregion
                }
                dt.Rows.Add(rowAvg);
                dt.Rows.Add(rowMax);
                dt.Rows.Add(rowMin);
                dt.Rows.Add(rowSample);
                dt.Rows.Add(rowBaseUpper);
                dt.Rows.Add(rowDayAvg);
                dt.Rows.Add(rowOver);
                dt.Rows.Add(rowMaxOver);
            }
            return dt;
        }
        #endregion

        #region 根据区域统计
        /// <summary>
        /// 根据key主键判断记录是否存在
        /// </summary>
        /// <param name="strKey">主键值</param>
        /// <returns></returns>
        public bool RIsExist(string strKey)
        {
            regionDayAQI = Singleton<RegionDayAQIRepository>.GetInstance();
            if (regionDayAQI != null)
                return regionDayAQI.IsExist(strKey);
            return false;
        }
        /// <summary>
        /// 获取各级别天数统计
        /// </summary>
        /// <param name="aqiType">AQI分指标字段</param>
        /// <param name="portIds">测点</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetAreaGradeStatistics(IAQIType aqiType, string[] regionGuids, DateTime dtStart, DateTime dtEnd)
        {
            regionDayAQI = Singleton<RegionDayAQIRepository>.GetInstance();
            if (regionDayAQI != null)
                return regionDayAQI.GetGradeStatistics(aqiType, regionGuids, dtStart, dtEnd);
            return null;
        }

        /// <summary>
        /// 获取监测天数
        /// </summary>
        /// <param name="Ponint">站点</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <returns></returns>
        public int GetRegionMonitoringDays(string regionGuids, IAQIType AQIType, DateTime dtStart, DateTime dtEnd)
        {
            int MonitoringDays = 0;
            regionDayAQI = Singleton<RegionDayAQIRepository>.GetInstance();
            if (regionDayAQI != null)
                switch (AQIType)
                {
                    case IAQIType.SO2_IAQI:
                        MonitoringDays = regionDayAQI.Retrieve(p => p.MonitoringRegionUid == regionGuids && p.ReportDateTime >= dtStart && p.ReportDateTime <= dtEnd && !string.IsNullOrEmpty(p.SO2_IAQI)).Count();
                        break;
                    case IAQIType.NO2_IAQI:
                        MonitoringDays = regionDayAQI.Retrieve(p => p.MonitoringRegionUid == regionGuids && p.ReportDateTime >= dtStart && p.ReportDateTime <= dtEnd && !string.IsNullOrEmpty(p.NO2_IAQI)).Count();
                        break;
                    case IAQIType.PM10_IAQI:
                        MonitoringDays = regionDayAQI.Retrieve(p => p.MonitoringRegionUid == regionGuids && p.ReportDateTime >= dtStart && p.ReportDateTime <= dtEnd && !string.IsNullOrEmpty(p.PM10_IAQI)).Count();
                        break;
                    case IAQIType.PM25_IAQI:
                        MonitoringDays = regionDayAQI.Retrieve(p => p.MonitoringRegionUid == regionGuids && p.ReportDateTime >= dtStart && p.ReportDateTime <= dtEnd && !string.IsNullOrEmpty(p.PM25_IAQI)).Count();
                        break;
                    case IAQIType.CO_IAQI:
                        MonitoringDays = regionDayAQI.Retrieve(p => p.MonitoringRegionUid == regionGuids && p.ReportDateTime >= dtStart && p.ReportDateTime <= dtEnd && !string.IsNullOrEmpty(p.CO_IAQI)).Count();
                        break;
                    case IAQIType.Max8HourO3_IAQI:
                        MonitoringDays = regionDayAQI.Retrieve(p => p.MonitoringRegionUid == regionGuids && p.ReportDateTime >= dtStart && p.ReportDateTime <= dtEnd && !string.IsNullOrEmpty(p.Max8HourO3_IAQI)).Count();
                        break;
                }
            return MonitoringDays;
        }
        /// <summary>
        /// 月报短信
        /// </summary>
        /// <param name="regions">站点</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <returns>
        public DataView GetRegionsAQI(DateTime dateStart, DateTime dateEnd, RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            regionDayAQI = Singleton<RegionDayAQIRepository>.GetInstance();
            return regionDayAQI.GetRegionsAQI(dateStart, dateEnd, regionAQIStatisticalType);
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页（从0开始）</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetAreaDataPager(string[] regionGuids, DateTime dtStart, DateTime dtEnd, int pageSize
            , int pageNo, out int recordTotal, string orderBy = "ReportDateTime,MonitoringRegionUid")
        {
            regionDayAQI = Singleton<RegionDayAQIRepository>.GetInstance();
            recordTotal = 0;
            if (regionDayAQI != null)
                return regionDayAQI.GetDataPager(regionGuids, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
            return null;
        }
        public DataView GetAreaDataPager(string[] regionGuids, DateTime dtStart, DateTime dtEnd, string[] type, int pageSize
       , int pageNo, out int recordTotal, string orderBy = "ReportDateTime,MonitoringRegionUid")
        {
            regionDayAQI = Singleton<RegionDayAQIRepository>.GetInstance();
            recordTotal = 0;
            if (regionDayAQI != null)
                return regionDayAQI.GetDataPager(regionGuids, dtStart, dtEnd, type, pageSize, pageNo, out recordTotal, orderBy);
            return null;
        }
        /// <summary>
        /// 根据DataView对象获取其中的字段的值并且将其转换成字符串
        /// </summary>
        /// <param name="dv">DataView对象</param>
        /// <returns></returns>
        public string GetStr(DataView dv, string time)
        {
            string str = "";
            int j = 1;
            for (int i = 0; i < dv.Count; i++)
            {
                string sumStr = dv[i][time].FormatToString("yyyy-MM-dd") + "," + dv[i]["AQIValue"].ToString();
                str = str + sumStr + ";";
                if (i == 8 * j)
                {

                    str += "<br />";
                    j++;
                }


            }
            return str;
        }
        /// <summary>
        /// 创建根据区域和污染种类的DataTable
        /// </summary>
        /// <param name="regionGuids">区域数组</param>
        /// <param name="classDics">污染种类数组</param>
        /// <param name="dtBegion">查询的开始时间</param>
        /// <param name="dtEnd">查询的结束时间</param>
        /// <param name="TotalType">统计类型</param>
        /// <param name="regionName">区域的名称的字典</param>
        /// <param name="className">污染类别的字典</param>
        /// <returns></returns>
        public DataTable GetAreaPolluateClassList(string[] regionGuids, string[] classDics, string[] factorDics, DateTime dtBegion, DateTime dtEnd, string TotalType, Dictionary<string, string> regionName, Dictionary<string, string> className, string lastDays)
        {
            try
            {

                DataTable dt = new DataTable();
                #region 按类别分类
                if (TotalType == "1")
                {
                    dt.Columns.Add("RegionName", typeof(string));

                    dt.Columns.Add("ClassName", typeof(string));
                    dt.Columns.Add("Days", typeof(Int32));
                    dt.Columns.Add("SpecficDay", typeof(string));
                    regionDayAQI = Singleton<RegionDayAQIRepository>.GetInstance();
                    DictionaryService dictionary = Singleton<DictionaryService>.GetInstance();

                    DataView dv = regionDayAQI.GetDataPagerClass(regionGuids, dtBegion, dtEnd);
                    if (dv != null)
                    {
                        foreach (string portItem in regionGuids)
                        {

                            //  DataRow drNew = dt.NewRow();



                            foreach (string classItem in classDics)
                            {
                                DataRow drNew = dt.NewRow();
                                drNew["RegionName"] = regionName[portItem];
                                drNew["ClassName"] = className[classItem];
                                switch (classItem)
                                {
                                    case "0":
                                        dv.RowFilter = "MonitoringRegionUid='" + portItem + "' and AQIValue >= 0 and AQIValue <= 50";
                                        drNew["Days"] = dv.Count;

                                        drNew["SpecficDay"] = "--";
                                        break;
                                    case "1":
                                        dv.RowFilter = "MonitoringRegionUid='" + portItem + "' and AQIValue >= 51 and AQIValue <= 100";

                                        drNew["Days"] = dv.Count;
                                        drNew["SpecficDay"] = "--";
                                        break;
                                    case "2":
                                        dv.RowFilter = "MonitoringRegionUid='" + portItem + "' and AQIValue >= 101 and AQIValue <= 150";
                                        drNew["Days"] = dv.Count;
                                        drNew["SpecficDay"] = GetStr(dv, "ReportDateTime");
                                        break;
                                    case "3":
                                        dv.RowFilter = "MonitoringRegionUid='" + portItem + "' and AQIValue >= 151 and AQIValue <= 200";
                                        drNew["Days"] = dv.Count;
                                        drNew["SpecficDay"] = GetStr(dv, "ReportDateTime");
                                        break;
                                    case "4":
                                        dv.RowFilter = "MonitoringRegionUid='" + portItem + "' and AQIValue >= 201 and AQIValue <= 300";
                                        drNew["Days"] = dv.Count;
                                        drNew["SpecficDay"] = GetStr(dv, "ReportDateTime");
                                        break;
                                    case "5":
                                        dv.RowFilter = "MonitoringRegionUid='" + portItem + "' and AQIValue > 300";
                                        drNew["Days"] = dv.Count;
                                        drNew["SpecficDay"] = GetStr(dv, "ReportDateTime");
                                        break;
                                    case "6":
                                        dv.RowFilter = "MonitoringRegionUid='" + portItem + "' and AQIValue < 0 or AQIValue is null";
                                        drNew["Days"] = dv.Count;
                                        drNew["SpecficDay"] = "--";
                                        break;


                                }
                                dt.Rows.Add(drNew);
                            }

                        }
                    }
                }
                #endregion
                #region 污染持续类别
                if (TotalType == "2")
                {

                    int nLastDays = Convert.ToInt32(lastDays);
                    Dictionary<string, string> classLevel = new Dictionary<string, string>();
                    regionDayAQI = Singleton<RegionDayAQIRepository>.GetInstance();
                    classLevel.Add("2", "轻度污染");
                    classLevel.Add("3", "中度污染");
                    classLevel.Add("4", "重度污染");
                    classLevel.Add("5", "严重污染");
                    string[] classLevels = { "2", "3", "4", "5" };
                    dt.Columns.Add("RegionName", typeof(string));

                    dt.Columns.Add("DateTime", typeof(string));
                    dt.Columns.Add("LastDays", typeof(Int32));
                    dt.Columns.Add("ClassName", typeof(string));
                    dt.Columns.Add("Days", typeof(Int32));
                    dt.Columns.Add("SpecficDay", typeof(string));
                    DataView dv = regionDayAQI.GetDataPagerClass(regionGuids, dtBegion, dtEnd);
                    if (dv != null)
                    {
                        DataTable AreaPolluation = dv.Table;
                        DataTable NewExceedingDays = AreaPolluation.Clone();
                        foreach (string portItem in regionGuids)
                        {
                            DataRow[] AllExceedingDays = AreaPolluation.Select("convert(AQIValue,'System.Int32')>100 and MonitoringRegionUid='" + portItem + "'");
                            if (AllExceedingDays.Length > 0)
                            {
                                NewExceedingDays = AllExceedingDays.CopyToDataTable();
                                DataView dvs = NewExceedingDays.DefaultView;
                                dvs.Sort = "ReportDateTime asc";
                                List<List<DateTime>> ContinuousDaysList = new List<List<DateTime>>();
                                List<DateTime> ContinuousDays = new List<DateTime>();
                                if (dvs.Count >= nLastDays)
                                {
                                    for (int i = 1; i < dvs.Count; i++)
                                    {
                                        DateTime firstValue = Convert.ToDateTime(dvs[i - 1]["ReportDateTime"]);
                                        DateTime secondValue = Convert.ToDateTime(dvs[i]["ReportDateTime"]);
                                        int poor = (secondValue - firstValue).Days;
                                        if (poor.Equals(1))
                                        {
                                            if (!ContinuousDays.Contains(firstValue))
                                            {
                                                ContinuousDays.Add(firstValue);
                                            }
                                            if (!ContinuousDays.Contains(secondValue))
                                            {
                                                ContinuousDays.Add(secondValue);
                                            }
                                            if (i == dvs.Count - 1)
                                            {
                                                if (ContinuousDays.Count >= nLastDays)
                                                {
                                                    ContinuousDaysList.Add(ContinuousDays);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (ContinuousDays.Count >= nLastDays)
                                            {
                                                ContinuousDaysList.Add(ContinuousDays);
                                            }
                                            ContinuousDays = new List<DateTime>();
                                        }
                                    }

                                    for (int i = 0; i < ContinuousDaysList.Count; i++)
                                    {

                                        List<DateTime> dateTimeArray = ContinuousDaysList[i];
                                        DataTable ExceedingDays = NewExceedingDays.Clone();
                                        ExceedingDays = NewExceedingDays.Select("ReportDateTime>='" + dateTimeArray[0] + "' and ReportDateTime<='" + dateTimeArray[dateTimeArray.Count - 1] + "'").CopyToDataTable();
                                        int outDays = dateTimeArray.Count;
                                        foreach (string classItem in classLevels)
                                        {
                                            DataRow dr = dt.NewRow();
                                            switch (classItem)
                                            {
                                                case "2":
                                                    string LightPollutionday = "";
                                                    DataRow[] LightPollution = ExceedingDays.Select("convert(AQIValue,'System.Int32')>=101 and convert(AQIValue,'System.Int32')<=150", "ReportDateTime");
                                                    foreach (DataRow light in LightPollution)
                                                    {
                                                        if (light["ReportDateTime"].IsNotNullOrDBNull())
                                                        {
                                                            LightPollutionday = LightPollutionday + light["ReportDateTime"].FormatToString("yyyy-MM-dd") + "," + light["AQIValue"] + ";";
                                                        }
                                                    }
                                                    int LightPollutionDays = LightPollution.Length;
                                                    dr["RegionName"] = regionName[portItem];
                                                    dr["DateTime"] = dateTimeArray[0].ToString("yyyy-MM-dd") + "—" + dateTimeArray[dateTimeArray.Count - 1].ToString("yyyy-MM-dd");
                                                    dr["LastDays"] = outDays;
                                                    dr["ClassName"] = classLevel[classItem];
                                                    dr["Days"] = LightPollutionDays;
                                                    dr["SpecficDay"] = LightPollutionday;
                                                    break;
                                                case "3":
                                                    string ModeratePollutionday = "";
                                                    DataRow[] ModeratePollution = ExceedingDays.Select("convert(AQIValue,'System.Int32')>=151 and convert(AQIValue,'System.Int32')<=200", "ReportDateTime");
                                                    foreach (DataRow moderate in ModeratePollution)
                                                    {
                                                        if (moderate["ReportDateTime"].IsNotNullOrDBNull())
                                                        {

                                                            ModeratePollutionday = ModeratePollutionday + moderate["ReportDateTime"].FormatToString("yyyy-MM-dd") + "," + moderate["AQIValue"] + ";";
                                                        }
                                                    }
                                                    int ModeratePollutionDays = ModeratePollution.Length;
                                                    dr["RegionName"] = regionName[portItem];
                                                    dr["DateTime"] = dateTimeArray[0].ToString("yyyy-MM-dd") + "—" + dateTimeArray[dateTimeArray.Count - 1].ToString("yyyy-MM-dd");
                                                    dr["LastDays"] = outDays;
                                                    dr["ClassName"] = classLevel[classItem];
                                                    dr["Days"] = ModeratePollutionDays;
                                                    dr["SpecficDay"] = ModeratePollutionday;
                                                    break;
                                                case "4":
                                                    string HighPollutionday = "";
                                                    DataRow[] HighPollution = ExceedingDays.Select("convert(AQIValue,'System.Int32')>=201 and convert(AQIValue,'System.Int32')<=300", "ReportDateTime");
                                                    foreach (DataRow high in HighPollution)
                                                    {
                                                        if (high["ReportDateTime"].IsNotNullOrDBNull())
                                                        {
                                                            HighPollutionday = HighPollutionday + high["ReportDateTime"].FormatToString("yyyy-MM-dd") + "," + high["AQIValue"] + ";";
                                                        }
                                                    }
                                                    int HighPollutionDays = HighPollution.Length;
                                                    dr["RegionName"] = regionName[portItem];
                                                    dr["DateTime"] = dateTimeArray[0].ToString("yyyy-MM-dd") + "—" + dateTimeArray[dateTimeArray.Count - 1].ToString("yyyy-MM-dd");
                                                    dr["LastDays"] = outDays;
                                                    dr["ClassName"] = classLevel[classItem];
                                                    dr["Days"] = HighPollutionDays;
                                                    dr["SpecficDay"] = HighPollutionday;
                                                    break;
                                                case "5":
                                                    string SeriousPollutionday = "";
                                                    DataRow[] SeriousPollution = ExceedingDays.Select("convert(AQIValue,'System.Int32')>=301 ", "ReportDateTime");
                                                    foreach (DataRow serious in SeriousPollution)
                                                    {
                                                        if (serious["ReportDateTime"].IsNotNullOrDBNull())
                                                        {
                                                            SeriousPollutionday = SeriousPollutionday + serious["ReportDateTime"].FormatToString("yyyy-MM-dd") + "," + serious["AQIValue"] + ";";
                                                        }
                                                    }
                                                    int SeriousPollutionDays = SeriousPollution.Length;
                                                    dr["RegionName"] = regionName[portItem];
                                                    dr["DateTime"] = dateTimeArray[0].ToString("yyyy-MM-dd") + "—" + dateTimeArray[dateTimeArray.Count - 1].ToString("yyyy-MM-dd");
                                                    dr["LastDays"] = outDays;
                                                    dr["ClassName"] = classLevel[classItem];
                                                    dr["Days"] = SeriousPollutionDays;
                                                    dr["SpecficDay"] = SeriousPollutionday;
                                                    break;
                                            }
                                            dt.Rows.Add(dr);
                                        }





                                    }
                                }
                            }

                        }
                    }
                }
                #endregion
                #region 首要污染物类别
                if (TotalType == "3")
                {
                    dt.Columns.Add("RegionName", typeof(string));

                    dt.Columns.Add("PolluteName", typeof(string));
                    dt.Columns.Add("Days", typeof(Int32));
                    dt.Columns.Add("Rates", typeof(Int32));
                    dt.Columns.Add("MoreSpecficDay", typeof(string));
                    regionDayAQI = Singleton<RegionDayAQIRepository>.GetInstance();
                    DictionaryService dictionary = Singleton<DictionaryService>.GetInstance();

                    DataView dv = regionDayAQI.GetFirstPollute(regionGuids, dtBegion, dtEnd);
                    if (dv != null)
                    {
                        foreach (string portItem in regionGuids)
                        {
                            dv.RowFilter = "MonitoringRegionUid='" + portItem + "' and (PrimaryPollutant like '%PM2.5%' or PrimaryPollutant like '%PM10%' or PrimaryPollutant like '%NO2%' or PrimaryPollutant like '%SO2%' or PrimaryPollutant like '%CO%' or PrimaryPollutant like '%O3%')";
                            double sum = dv.Count;
                            foreach (string factorItem in factorDics)
                            {
                                int rate;
                                DataRow drNew = dt.NewRow();
                                drNew["RegionName"] = regionName[portItem];
                                drNew["PolluteName"] = factorItem;
                                switch (factorItem)
                                {
                                    case "PM2.5":
                                        dv.RowFilter = "MonitoringRegionUid='" + portItem + "' and PrimaryPollutant like '%PM2.5%'";
                                        drNew["Days"] = dv.Count;
                                        if (sum != 0)
                                        {
                                            rate = (int)(Math.Round(dv.Count / sum, 2) * 100);
                                        }
                                        else
                                        {
                                            rate = 0;
                                        }
                                        drNew["Rates"] = rate;
                                        drNew["MoreSpecficDay"] = GetStr(dv, "ReportDateTime");
                                        break;
                                    case "PM10":
                                        dv.RowFilter = "MonitoringRegionUid='" + portItem + "' and PrimaryPollutant like '%PM10%'";
                                        drNew["Days"] = dv.Count;
                                        if (sum != 0)
                                        {
                                            rate = (int)(Math.Round(dv.Count / sum, 2) * 100);
                                        }
                                        else
                                        {
                                            rate = 0;
                                        }
                                        drNew["Rates"] = rate;
                                        drNew["MoreSpecficDay"] = GetStr(dv, "ReportDateTime");
                                        break;
                                    case "NO2":
                                        dv.RowFilter = "MonitoringRegionUid='" + portItem + "' and PrimaryPollutant like '%NO2%'";
                                        drNew["Days"] = dv.Count;
                                        if (sum != 0)
                                        {
                                            rate = (int)(Math.Round(dv.Count / sum, 2) * 100);
                                        }
                                        else
                                        {
                                            rate = 0;
                                        }
                                        drNew["Rates"] = rate;
                                        drNew["MoreSpecficDay"] = GetStr(dv, "ReportDateTime");
                                        break;
                                    case "SO2":
                                        dv.RowFilter = "MonitoringRegionUid='" + portItem + "' and PrimaryPollutant like '%SO2%'";
                                        drNew["Days"] = dv.Count;
                                        if (sum != 0)
                                        {
                                            rate = (int)(Math.Round(dv.Count / sum, 2) * 100);
                                        }
                                        else
                                        {
                                            rate = 0;
                                        }
                                        drNew["Rates"] = rate;
                                        drNew["MoreSpecficDay"] = GetStr(dv, "ReportDateTime");
                                        break;
                                    case "CO":
                                        dv.RowFilter = "MonitoringRegionUid='" + portItem + "' and PrimaryPollutant like '%CO%'";
                                        drNew["Days"] = dv.Count;
                                        if (sum != 0)
                                        {
                                            rate = (int)(Math.Round(dv.Count / sum, 2) * 100);
                                        }
                                        else
                                        {
                                            rate = 0;
                                        }
                                        drNew["Rates"] = rate;
                                        drNew["MoreSpecficDay"] = GetStr(dv, "ReportDateTime");
                                        break;
                                    case "O3-8小时":
                                        dv.RowFilter = "MonitoringRegionUid='" + portItem + "' and PrimaryPollutant like '%O3%'";
                                        drNew["Days"] = dv.Count;
                                        if (sum != 0)
                                        {
                                            rate = (int)(Math.Round(dv.Count / sum, 2) * 100);
                                        }
                                        else
                                        {
                                            rate = 0;
                                        }
                                        drNew["Rates"] = rate;
                                        drNew["MoreSpecficDay"] = GetStr(dv, "ReportDateTime");
                                        break;



                                }
                                dt.Rows.Add(drNew);
                            }
                        }
                    }
                }
                #endregion
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DataTable GetAreaOverDaysList(string[] ports, string[] regionGuids, string[] factors, DateTime dtStart, DateTime dtEnd, string TotalType, string strFactor, int StandAQI, int pageSize, int pageNo,
         string[] years, Dictionary<string, string> regionName, out int recordTotal, string orderBy = "ReportDateTime,MonitoringRegionUid")
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("MonitoringRegionUid", typeof(string));
                dt.Columns.Add("RegionName", typeof(string));
                dt.Columns.Add("StandardDays", typeof(string));
                dt.Columns.Add("OverDays", typeof(string));
                dt.Columns.Add("InvalidDays", typeof(string));
                dt.Columns.Add("StandardDaysRate", typeof(string));
                foreach (string year in years)
                {
                    dt.Columns.Add(year, typeof(string));
                }
                if (TotalType == "质量类别")
                {
                    dt.Columns.Add("Good", typeof(string));
                    dt.Columns.Add("Moderate", typeof(string));
                    dt.Columns.Add("LightlyPolluted", typeof(string));
                    dt.Columns.Add("ModeratelyPolluted", typeof(string));
                    dt.Columns.Add("HeavilyPolluted", typeof(string));
                    dt.Columns.Add("SeverelyPolluted", typeof(string));
                }
                else
                {
                    if (strFactor.Contains("超标"))
                    {
                        if (factors.Contains("PM25"))
                        {
                            dt.Columns.Add("OverPM25", typeof(string));
                        }
                        if (factors.Contains("PM10"))
                        {
                            dt.Columns.Add("OverPM10", typeof(string));
                        }
                        if (factors.Contains("NO2"))
                        {
                            dt.Columns.Add("OverNO2", typeof(string));
                        }
                        if (factors.Contains("SO2"))
                        {
                            dt.Columns.Add("OverSO2", typeof(string));
                        }
                        if (factors.Contains("CO"))
                        {
                            dt.Columns.Add("OverCO", typeof(string));
                        }
                        if (factors.Contains("RecentoneHoursO3"))
                        {
                            dt.Columns.Add("OverO3", typeof(string));
                        }
                        if (factors.Contains("Recent8HoursO3"))
                        {
                            dt.Columns.Add("OverRecent8HoursO3", typeof(string));
                        }
                    }
                    if (strFactor.Contains("达标"))
                    {
                        if (factors.Contains("PM25"))
                        {
                            dt.Columns.Add("StandPM25", typeof(string));
                        }
                        if (factors.Contains("PM10"))
                        {
                            dt.Columns.Add("StandPM10", typeof(string));
                        }
                        if (factors.Contains("NO2"))
                        {
                            dt.Columns.Add("StandNO2", typeof(string));
                        }
                        if (factors.Contains("SO2"))
                        {
                            dt.Columns.Add("StandSO2", typeof(string));
                        }
                        if (factors.Contains("CO"))
                        {
                            dt.Columns.Add("StandCO", typeof(string));
                        }
                        if (factors.Contains("RecentoneHoursO3"))
                        {
                            dt.Columns.Add("StandO3", typeof(string));
                        }
                        if (factors.Contains("Recent8HoursO3"))
                        {
                            dt.Columns.Add("StandRecent8HoursO3", typeof(string));
                        }
                    }
                }
                regionDayAQI = Singleton<RegionDayAQIRepository>.GetInstance();
                DictionaryService dictionary = Singleton<DictionaryService>.GetInstance();
                //动态计算区域AQI
                AQICalculateService m_AQICalculateService = new AQICalculateService();
                MonitoringPointAirService pointAirService = Singleton<MonitoringPointAirService>.GetInstance();
                DataView dv = new DataView();
                DataTable dtForAQI = new DataTable();
                DataView dvRegion = pointAirService.GetRegionByPointId(ports);
                dtForAQI.Columns.Add("RegionName", typeof(string));
                dtForAQI.Columns.Add("DateTime", typeof(DateTime));
                dtForAQI.Columns.Add("PM25", typeof(string));
                dtForAQI.Columns.Add("PM25_IAQI", typeof(Int32));
                dtForAQI.Columns.Add("PM10", typeof(string));
                dtForAQI.Columns.Add("PM10_IAQI", typeof(Int32));
                dtForAQI.Columns.Add("NO2", typeof(string));
                dtForAQI.Columns.Add("NO2_IAQI", typeof(Int32));
                dtForAQI.Columns.Add("SO2", typeof(string));
                dtForAQI.Columns.Add("SO2_IAQI", typeof(Int32));
                dtForAQI.Columns.Add("CO", typeof(string));
                dtForAQI.Columns.Add("CO_IAQI", typeof(Int32));
                dtForAQI.Columns.Add("O3", typeof(string));
                dtForAQI.Columns.Add("O3_IAQI", typeof(Int32));
                dtForAQI.Columns.Add("Max8HourO3", typeof(string));
                dtForAQI.Columns.Add("Max8HourO3_IAQI", typeof(Int32));
                dtForAQI.Columns.Add("AQIValue", typeof(Int32));
                int dayNum = Convert.ToInt32((dtEnd.Subtract(dtStart)).TotalDays);
                List<string> reName = dvRegion.ToTable().AsEnumerable().Select(t => t.Field<string>("Region")).ToList();
                IEnumerable<string> names = reName.Distinct();
                for (int i = 0; i < dayNum; i++)
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

                        DateTime dayNew = dtStart.AddDays(i);
                        decimal? PM25PollutantValue = m_AQICalculateService.GetRegionValue(ids, "a34004", dayNew, 24, "2");
                        decimal? PM10PollutantValue = m_AQICalculateService.GetRegionValue(ids, "a34002", dayNew, 24, "2");
                        decimal? NO2PollutantValue = m_AQICalculateService.GetRegionValue(ids, "a21004", dayNew, 24, "2");
                        decimal? SO2PollutantValue = m_AQICalculateService.GetRegionValue(ids, "a21026", dayNew, 24, "2");
                        decimal? COPollutantValue = m_AQICalculateService.GetRegionValue(ids, "a21005", dayNew, 24, "2");
                        decimal? O3PollutantValue = m_AQICalculateService.GetRegionValue(ids, "a05024", dayNew, 1, "2");
                        decimal? Max8HourO3PollutantValue = m_AQICalculateService.GetRegionValue(ids, "a05024", dayNew, 8, "2");
                        int? PM25Value = m_AQICalculateService.GetIAQI("a34004", Convert.ToDouble(PM25PollutantValue), 24);
                        int? PM10Value = m_AQICalculateService.GetIAQI("a34002", Convert.ToDouble(PM10PollutantValue), 24);
                        int? NO2Value = m_AQICalculateService.GetIAQI("a21004", Convert.ToDouble(NO2PollutantValue), 24);
                        int? SO2Value = m_AQICalculateService.GetIAQI("a21026", Convert.ToDouble(SO2PollutantValue), 24);
                        int? COValue = m_AQICalculateService.GetIAQI("a21005", Convert.ToDouble(COPollutantValue), 24);
                        int? O3Value = m_AQICalculateService.GetIAQI("a05024", Convert.ToDouble(O3PollutantValue), 1);
                        int? Max8HourO3Value = m_AQICalculateService.GetIAQI("a05024", Convert.ToDouble(Max8HourO3PollutantValue), 8);
                        string AQIValue = m_AQICalculateService.GetAQI_Max_CNV(SO2Value, NO2Value, PM10Value, COValue, Max8HourO3Value, PM25Value, "V");
                        DataRow dr = dtForAQI.NewRow();
                        dr["RegionName"] = name;
                        dr["DateTime"] = dayNew;
                        dr["PM25"] = PM25PollutantValue.ToString();
                        if (PM25Value != null)
                            dr["PM25_IAQI"] = PM25Value;
                        dr["PM10"] = PM10PollutantValue.ToString();
                        if (PM10Value != null)
                            dr["PM10_IAQI"] = PM10Value;
                        dr["NO2"] = NO2PollutantValue.ToString();
                        if (NO2Value != null)
                            dr["NO2_IAQI"] = NO2Value;
                        dr["SO2"] = SO2PollutantValue.ToString();
                        if (SO2Value != null)
                            dr["SO2_IAQI"] = SO2Value;
                        dr["CO"] = COPollutantValue.ToString();
                        if (COValue != null)
                            dr["CO_IAQI"] = COValue;
                        dr["O3"] = O3PollutantValue.ToString();
                        if (O3Value != null)
                            dr["O3_IAQI"] = O3Value;
                        dr["Max8HourO3"] = Max8HourO3PollutantValue.ToString();
                        if (Max8HourO3Value != null)
                            dr["Max8HourO3_IAQI"] = Max8HourO3Value;
                        if (AQIValue != null && AQIValue.Trim() != "")
                            dr["AQIValue"] = Convert.ToInt32(AQIValue);
                        dtForAQI.Rows.Add(dr);
                    }
                }
                dv = dtForAQI.AsDataView();


                Dictionary<string, DataView> dvlast = new Dictionary<string, DataView>();
                foreach (string year in years)
                {
                    int addyear = dtEnd.Year - int.Parse(year);
                    DataView dvNew = new DataView();
                    DataTable dtForAQICompare = new DataTable();
                    dtForAQICompare.Columns.Add("RegionName", typeof(string));
                    dtForAQICompare.Columns.Add("DateTime", typeof(DateTime));
                    dtForAQICompare.Columns.Add("PM25", typeof(string));
                    dtForAQICompare.Columns.Add("PM25_IAQI", typeof(Int32));
                    dtForAQICompare.Columns.Add("PM10", typeof(string));
                    dtForAQICompare.Columns.Add("PM10_IAQI", typeof(Int32));
                    dtForAQICompare.Columns.Add("NO2", typeof(string));
                    dtForAQICompare.Columns.Add("NO2_IAQI", typeof(Int32));
                    dtForAQICompare.Columns.Add("SO2", typeof(string));
                    dtForAQICompare.Columns.Add("SO2_IAQI", typeof(Int32));
                    dtForAQICompare.Columns.Add("CO", typeof(string));
                    dtForAQICompare.Columns.Add("CO_IAQI", typeof(Int32));
                    dtForAQICompare.Columns.Add("O3", typeof(string));
                    dtForAQICompare.Columns.Add("O3_IAQI", typeof(string));
                    dtForAQICompare.Columns.Add("Max8HourO3", typeof(string));
                    dtForAQICompare.Columns.Add("Max8HourO3_IAQI", typeof(Int32));
                    dtForAQICompare.Columns.Add("AQIValue", typeof(Int32));
                    int dayNumCompare = Convert.ToInt32(dtEnd.Subtract(dtStart).TotalDays);
                    for (int i = 0; i <= dayNumCompare; i++)
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

                            DateTime dayNew = dtStart.AddYears(-addyear).AddDays(i);
                            decimal? PM25PollutantValue = m_AQICalculateService.GetRegionValue(ids, "a34004", dayNew, 24, "2");
                            decimal? PM10PollutantValue = m_AQICalculateService.GetRegionValue(ids, "a34002", dayNew, 24, "2");
                            decimal? NO2PollutantValue = m_AQICalculateService.GetRegionValue(ids, "a21004", dayNew, 24, "2");
                            decimal? SO2PollutantValue = m_AQICalculateService.GetRegionValue(ids, "a21026", dayNew, 24, "2");
                            decimal? COPollutantValue = m_AQICalculateService.GetRegionValue(ids, "a21005", dayNew, 24, "2");
                            decimal? O3PollutantValue = m_AQICalculateService.GetRegionValue(ids, "a05024", dayNew, 1, "2");
                            decimal? Max8HourO3PollutantValue = m_AQICalculateService.GetRegionValue(ids, "a05024", dayNew, 8, "2");
                            int? PM25Value = m_AQICalculateService.GetIAQI("a34004", Convert.ToDouble(PM25PollutantValue), 24);
                            int? PM10Value = m_AQICalculateService.GetIAQI("a34002", Convert.ToDouble(PM10PollutantValue), 24);
                            int? NO2Value = m_AQICalculateService.GetIAQI("a21004", Convert.ToDouble(NO2PollutantValue), 24);
                            int? SO2Value = m_AQICalculateService.GetIAQI("a21026", Convert.ToDouble(SO2PollutantValue), 24);
                            int? COValue = m_AQICalculateService.GetIAQI("a21005", Convert.ToDouble(COPollutantValue), 24);
                            int? O3Value = m_AQICalculateService.GetIAQI("a05024", Convert.ToDouble(O3PollutantValue), 1);
                            int? Max8HourO3Value = m_AQICalculateService.GetIAQI("a05024", Convert.ToDouble(Max8HourO3PollutantValue), 8);
                            string AQIValue = m_AQICalculateService.GetAQI_Max_CNV(SO2Value, NO2Value, PM10Value, COValue, Max8HourO3Value, PM25Value, "V");
                            DataRow dr = dtForAQICompare.NewRow();
                            dr["RegionName"] = name;
                            dr["DateTime"] = dayNew;
                            dr["PM25"] = PM25PollutantValue.ToString();
                            if (PM25Value != null)
                                dr["PM25_IAQI"] = PM25Value;
                            dr["PM10"] = PM10PollutantValue.ToString();
                            if (PM10Value != null)
                                dr["PM10_IAQI"] = PM10Value;
                            dr["NO2"] = NO2PollutantValue.ToString();
                            if (NO2Value != null)
                                dr["NO2_IAQI"] = NO2Value;
                            dr["SO2"] = SO2PollutantValue.ToString();
                            if (SO2Value != null)
                                dr["SO2_IAQI"] = SO2Value;
                            dr["CO"] = COPollutantValue.ToString();
                            if (COValue != null)
                                dr["CO_IAQI"] = COValue;
                            dr["O3"] = O3PollutantValue.ToString();
                            if (O3Value != null)
                                dr["O3_IAQI"] = O3Value;
                            dr["Max8HourO3"] = Max8HourO3PollutantValue.ToString();
                            if (Max8HourO3Value != null)
                                dr["Max8HourO3_IAQI"] = Max8HourO3Value;
                            if (AQIValue != null && AQIValue.Trim() != "")
                                dr["AQIValue"] = Convert.ToInt32(AQIValue);
                            dtForAQICompare.Rows.Add(dr);
                        }
                    }
                    dvNew = dtForAQICompare.AsDataView();
                    dvlast.Add(year, dvNew);
                }
                foreach (string portItem in regionGuids)
                {
                    DataRow drNew = dt.NewRow();
                    drNew["MonitoringRegionUid"] = portItem;
                    drNew["RegionName"] = regionName[portItem];
                    dv.RowFilter = "RegionName='" + regionName[portItem] + "' and AQIValue>0 and AQIValue<= " + StandAQI;
                    double StandardDays = dv.Count;
                    drNew["StandardDays"] = StandardDays;
                    dv.RowFilter = "RegionName='" + regionName[portItem] + "' and AQIValue>" + StandAQI;
                    double OverDays = dv.Count;
                    drNew["OverDays"] = OverDays;
                    dv.RowFilter = "RegionName='" + regionName[portItem] + "' and (AQIValue<0 or AQIValue is null)";
                    double InvalidDays = dv.Count;
                    drNew["InvalidDays"] = InvalidDays;
                    if (StandardDays + OverDays != 0)
                    {
                        drNew["StandardDaysRate"] = DecimalExtension.GetPollutantValue((Convert.ToDecimal(StandardDays / (StandardDays + OverDays))) * 100, 1).ToString();
                    }
                    foreach (string year in years)
                    {
                        var dvnewlast = dvlast[year];
                        dvnewlast.RowFilter = "RegionName='" + regionName[portItem] + "' and AQIValue<= " + StandAQI;
                        double SZStandardDayslast = dvnewlast.Count;
                        dvnewlast.RowFilter = "RegionName='" + regionName[portItem] + "' and AQIValue>" + StandAQI;
                        double SZOverDayslast = dvnewlast.Count;
                        if (SZStandardDayslast + SZOverDayslast != 0)
                        {
                            drNew[year] = Math.Round(Convert.ToDecimal(SZStandardDayslast / (SZStandardDayslast + SZOverDayslast)) * 100, 1).ToString();
                        }
                    }
                    if (TotalType == "质量类别")
                    {
                        dv.RowFilter = "RegionName='" + regionName[portItem] + "' and AQIValue<=50";
                        drNew["Good"] = dv.Count;
                        dv.RowFilter = "RegionName='" + regionName[portItem] + "' and AQIValue<=100 and AQIValue>50";
                        drNew["Moderate"] = dv.Count;
                        dv.RowFilter = "RegionName='" + regionName[portItem] + "' and AQIValue<=150 and AQIValue>100";
                        drNew["LightlyPolluted"] = dv.Count;
                        dv.RowFilter = "RegionName='" + regionName[portItem] + "' and AQIValue<=200 and AQIValue>150";
                        drNew["ModeratelyPolluted"] = dv.Count;
                        dv.RowFilter = "RegionName='" + regionName[portItem] + "' and AQIValue<=300 and AQIValue>200";
                        drNew["HeavilyPolluted"] = dv.Count;
                        dv.RowFilter = "RegionName='" + regionName[portItem] + "' and AQIValue>300";
                        drNew["SeverelyPolluted"] = dv.Count;
                    }
                    else
                    {
                        if (strFactor.Contains("超标"))
                        {
                            if (factors.Contains("SO2"))
                            {
                                dv.RowFilter = "RegionName='" + regionName[portItem] + "' and SO2_IAQI> " + StandAQI;
                                drNew["OverSO2"] = dv.Count;
                            }
                            if (factors.Contains("NO2"))
                            {
                                dv.RowFilter = "RegionName='" + regionName[portItem] + "' and NO2_IAQI> " + StandAQI;
                                drNew["OverNO2"] = dv.Count;
                            }
                            if (factors.Contains("PM25"))
                            {
                                dv.RowFilter = "RegionName='" + regionName[portItem] + "' and PM25_IAQI> " + StandAQI;
                                drNew["OverPM25"] = dv.Count;
                            }
                            if (factors.Contains("PM10"))
                            {
                                dv.RowFilter = "RegionName='" + regionName[portItem] + "' and PM10_IAQI> " + StandAQI;
                                drNew["OverPM10"] = dv.Count;
                            }
                            if (factors.Contains("CO"))
                            {
                                dv.RowFilter = "RegionName='" + regionName[portItem] + "' and CO_IAQI> " + StandAQI;
                                drNew["OverCO"] = dv.Count;
                            }
                            if (factors.Contains("RecentoneHoursO3"))
                            {
                                dv.RowFilter = "RegionName='" + regionName[portItem] + "' and O3_IAQI> " + StandAQI;
                                drNew["OverO3"] = dv.Count;
                            }
                            if (factors.Contains("Recent8HoursO3"))
                            {
                                dv.RowFilter = "RegionName='" + regionName[portItem] + "' and Max8HourO3_IAQI> " + StandAQI;
                                drNew["OverRecent8HoursO3"] = dv.Count;
                            }
                        }
                        if (strFactor.Contains("达标"))
                        {
                            if (factors.Contains("SO2"))
                            {
                                dv.RowFilter = "RegionName='" + regionName[portItem] + "' and SO2_IAQI<= " + StandAQI;
                                drNew["StandSO2"] = dv.Count;
                            }
                            if (factors.Contains("NO2"))
                            {
                                dv.RowFilter = "RegionName='" + regionName[portItem] + "' and NO2_IAQI<= " + StandAQI;
                                drNew["StandNO2"] = dv.Count;
                            }
                            if (factors.Contains("PM25"))
                            {
                                dv.RowFilter = "RegionName='" + regionName[portItem] + "' and PM25_IAQI<= " + StandAQI;
                                drNew["StandPM25"] = dv.Count;
                            }
                            if (factors.Contains("PM10"))
                            {
                                dv.RowFilter = "RegionName='" + regionName[portItem] + "' and PM10_IAQI<= " + StandAQI;
                                drNew["StandPM10"] = dv.Count;
                            }
                            if (factors.Contains("CO"))
                            {
                                dv.RowFilter = "RegionName='" + regionName[portItem] + "' and CO_IAQI<= " + StandAQI;
                                drNew["StandCO"] = dv.Count;
                            }
                            if (factors.Contains("RecentoneHoursO3"))
                            {
                                dv.RowFilter = "RegionName='" + regionName[portItem] + "' and O3_IAQI<= " + StandAQI;
                                drNew["StandO3"] = dv.Count;
                            }
                            if (factors.Contains("Recent8HoursO3"))
                            {
                                dv.RowFilter = "RegionName='" + regionName[portItem] + "' and Max8HourO3_IAQI<= " + StandAQI;
                                drNew["StandRecent8HoursO3"] = dv.Count;
                            }
                        }
                    }
                    dt.Rows.Add(drNew);

                }
                recordTotal = dt.Rows.Count;
                return dt;
            }
            catch (Exception ex)
            {
                recordTotal = 0;
                return null;
            }
        }

        public DataTable GetAreaOverDaysList(string[] ports, string[] factors, DateTime dtStart, DateTime dtEnd, string TotalType, string strFactor, int StandAQI, int pageSize, int pageNo,
         string[] years, out int recordTotal, string orderBy = "ReportDateTime,MonitoringRegionUid")
        {
            try
            {
                AQICalculateService m_AQICalculateService = new AQICalculateService();
                DataTable dt = new DataTable();
                dt.Columns.Add("MonitoringRegionUid", typeof(string));
                dt.Columns.Add("RegionName", typeof(string));
                dt.Columns.Add("StandardDays", typeof(string));
                dt.Columns.Add("OverDays", typeof(string));
                dt.Columns.Add("InvalidDays", typeof(string));
                dt.Columns.Add("StandardDaysRate", typeof(string));
                foreach (string year in years)
                {
                    dt.Columns.Add(year, typeof(string));
                }
                if (TotalType == "质量类别")
                {
                    dt.Columns.Add("Good", typeof(string));
                    dt.Columns.Add("Moderate", typeof(string));
                    dt.Columns.Add("LightlyPolluted", typeof(string));
                    dt.Columns.Add("ModeratelyPolluted", typeof(string));
                    dt.Columns.Add("HeavilyPolluted", typeof(string));
                    dt.Columns.Add("SeverelyPolluted", typeof(string));
                }
                else
                {
                    if (strFactor.Contains("超标"))
                    {
                        if (factors.Contains("PM25"))
                        {
                            dt.Columns.Add("OverPM25", typeof(string));
                        }
                        if (factors.Contains("PM10"))
                        {
                            dt.Columns.Add("OverPM10", typeof(string));
                        }
                        if (factors.Contains("NO2"))
                        {
                            dt.Columns.Add("OverNO2", typeof(string));
                        }
                        if (factors.Contains("SO2"))
                        {
                            dt.Columns.Add("OverSO2", typeof(string));
                        }
                        if (factors.Contains("CO"))
                        {
                            dt.Columns.Add("OverCO", typeof(string));
                        }
                        if (factors.Contains("RecentoneHoursO3"))
                        {
                            dt.Columns.Add("OverO3", typeof(string));
                        }
                        if (factors.Contains("Recent8HoursO3"))
                        {
                            dt.Columns.Add("OverRecent8HoursO3", typeof(string));
                        }
                    }
                    if (strFactor.Contains("达标"))
                    {
                        if (factors.Contains("PM25"))
                        {
                            dt.Columns.Add("StandPM25", typeof(string));
                        }
                        if (factors.Contains("PM10"))
                        {
                            dt.Columns.Add("StandPM10", typeof(string));
                        }
                        if (factors.Contains("NO2"))
                        {
                            dt.Columns.Add("StandNO2", typeof(string));
                        }
                        if (factors.Contains("SO2"))
                        {
                            dt.Columns.Add("StandSO2", typeof(string));
                        }
                        if (factors.Contains("CO"))
                        {
                            dt.Columns.Add("StandCO", typeof(string));
                        }
                        if (factors.Contains("RecentoneHoursO3"))
                        {
                            dt.Columns.Add("StandO3", typeof(string));
                        }
                        if (factors.Contains("Recent8HoursO3"))
                        {
                            dt.Columns.Add("StandRecent8HoursO3", typeof(string));
                        }
                    }
                    if (strFactor.Contains("首要污染物"))
                    {
                        if (factors.Contains("PM25"))
                        {
                            dt.Columns.Add("PrimaryPM25", typeof(string));
                        }
                        if (factors.Contains("PM10"))
                        {
                            dt.Columns.Add("PrimaryPM10", typeof(string));
                        }
                        if (factors.Contains("NO2"))
                        {
                            dt.Columns.Add("PrimaryNO2", typeof(string));
                        }
                        if (factors.Contains("SO2"))
                        {
                            dt.Columns.Add("PrimarySO2", typeof(string));
                        }
                        if (factors.Contains("CO"))
                        {
                            dt.Columns.Add("PrimaryCO", typeof(string));
                        }
                        if (factors.Contains("RecentoneHoursO3") || factors.Contains("Recent8HoursO3"))
                        {
                            dt.Columns.Add("PrimaryO3", typeof(string));
                        }
                    }
                }
                //查询数据
                DataView dv = m_AQICalculateService.GetRegionAQI(ports, dtStart, dtEnd, 24, "2").AsDataView();
                //查询所有区域
                string[] regionGuids = dv.Table.AsEnumerable().Select(t => t.Field<string>("PointId")).Distinct().ToArray();
                DictionaryService dictionary = Singleton<DictionaryService>.GetInstance();
                Dictionary<string, DataView> dvlast = new Dictionary<string, DataView>();
                foreach (string year in years)
                {
                    int addyear = dtEnd.Year - int.Parse(year);
                    DataView dvNew = m_AQICalculateService.GetRegionAQI(ports, dtStart.AddYears(-addyear), dtEnd.AddYears(-addyear), 24, "2").AsDataView();
                    dvlast.Add(year, dvNew);
                }
                foreach (string portItem in regionGuids)
                {
                    DataRow drNew = dt.NewRow();
                    drNew["MonitoringRegionUid"] = portItem;
                    drNew["RegionName"] = portItem;
                    dv.RowFilter = "PointId='" + portItem + "' and AQIValue>0 and AQIValue<= " + StandAQI;
                    double StandardDays = dv.Count;
                    drNew["StandardDays"] = StandardDays;
                    dv.RowFilter = "PointId='" + portItem + "' and AQIValue>" + StandAQI;
                    double OverDays = dv.Count;
                    drNew["OverDays"] = OverDays;
                    dv.RowFilter = "PointId='" + portItem + "' and (AQIValue<0 or AQIValue is null)";
                    double InvalidDays = dv.Count;
                    drNew["InvalidDays"] = InvalidDays;
                    if (StandardDays + OverDays != 0)
                    {
                        drNew["StandardDaysRate"] = DecimalExtension.GetPollutantValue((Convert.ToDecimal(StandardDays / (StandardDays + OverDays))) * 100, 1).ToString();
                    }
                    foreach (string year in years)
                    {
                        var dvnewlast = dvlast[year];
                        dvnewlast.RowFilter = "PointId='" + portItem + "' and AQIValue<= " + StandAQI;
                        double SZStandardDayslast = dvnewlast.Count;
                        dvnewlast.RowFilter = "PointId='" + portItem + "' and AQIValue>" + StandAQI;
                        double SZOverDayslast = dvnewlast.Count;
                        if (SZStandardDayslast + SZOverDayslast != 0)
                        {
                            drNew[year] = Math.Round(Convert.ToDecimal(SZStandardDayslast / (SZStandardDayslast + SZOverDayslast)) * 100, 1).ToString();
                        }
                    }
                    if (TotalType == "质量类别")
                    {
                        dv.RowFilter = "PointId='" + portItem + "' and AQIValue<=50";
                        drNew["Good"] = dv.Count;
                        dv.RowFilter = "PointId='" + portItem + "' and AQIValue<=100 and AQIValue>50";
                        drNew["Moderate"] = dv.Count;
                        dv.RowFilter = "PointId='" + portItem + "' and AQIValue<=150 and AQIValue>100";
                        drNew["LightlyPolluted"] = dv.Count;
                        dv.RowFilter = "PointId='" + portItem + "' and AQIValue<=200 and AQIValue>150";
                        drNew["ModeratelyPolluted"] = dv.Count;
                        dv.RowFilter = "PointId='" + portItem + "' and AQIValue<=300 and AQIValue>200";
                        drNew["HeavilyPolluted"] = dv.Count;
                        dv.RowFilter = "PointId='" + portItem + "' and AQIValue>300";
                        drNew["SeverelyPolluted"] = dv.Count;
                    }
                    else
                    {
                        if (strFactor.Contains("超标"))
                        {
                            if (factors.Contains("PM25"))
                            {
                                dv.RowFilter = "PointId='" + portItem + "' and PM25_IAQI> " + StandAQI;
                                drNew["OverPM25"] = dv.Count;
                            }
                            if (factors.Contains("PM10"))
                            {
                                dv.RowFilter = "PointId='" + portItem + "' and PM10_IAQI> " + StandAQI;
                                drNew["OverPM10"] = dv.Count;
                            }
                            if (factors.Contains("NO2"))
                            {
                                dv.RowFilter = "PointId='" + portItem + "' and NO2_IAQI> " + StandAQI;
                                drNew["OverNO2"] = dv.Count;
                            }
                            if (factors.Contains("SO2"))
                            {
                                dv.RowFilter = "PointId='" + portItem + "' and SO2_IAQI> " + StandAQI;
                                drNew["OverSO2"] = dv.Count;
                            }
                            if (factors.Contains("CO"))
                            {
                                dv.RowFilter = "PointId='" + portItem + "' and CO_IAQI> " + StandAQI;
                                drNew["OverCO"] = dv.Count;
                            }
                            if (factors.Contains("RecentoneHoursO3"))
                            {
                                dv.RowFilter = "PointId='" + portItem + "' and MaxOneHourO3_IAQI> " + StandAQI;
                                drNew["OverO3"] = dv.Count;
                            }
                            if (factors.Contains("Recent8HoursO3"))
                            {
                                dv.RowFilter = "PointId='" + portItem + "' and Max8HourO3_IAQI> " + StandAQI;
                                drNew["OverRecent8HoursO3"] = dv.Count;
                            }
                        }
                        if (strFactor.Contains("达标"))
                        {
                            if (factors.Contains("PM25"))
                            {
                                dv.RowFilter = "PointId='" + portItem + "' and PM25_IAQI<= " + StandAQI;
                                drNew["StandPM25"] = dv.Count;
                            }
                            if (factors.Contains("PM10"))
                            {
                                dv.RowFilter = "PointId='" + portItem + "' and PM10_IAQI<= " + StandAQI;
                                drNew["StandPM10"] = dv.Count;
                            }
                            if (factors.Contains("NO2"))
                            {
                                dv.RowFilter = "PointId='" + portItem + "' and NO2_IAQI<= " + StandAQI;
                                drNew["StandNO2"] = dv.Count;
                            }
                            if (factors.Contains("SO2"))
                            {
                                dv.RowFilter = "PointId='" + portItem + "' and SO2_IAQI<= " + StandAQI;
                                drNew["StandSO2"] = dv.Count;
                            }
                            if (factors.Contains("CO"))
                            {
                                dv.RowFilter = "PointId='" + portItem + "' and CO_IAQI<= " + StandAQI;
                                drNew["StandCO"] = dv.Count;
                            }
                            if (factors.Contains("RecentoneHoursO3"))
                            {
                                dv.RowFilter = "PointId='" + portItem + "' and MaxOneHourO3_IAQI<= " + StandAQI;
                                drNew["StandO3"] = dv.Count;
                            }
                            if (factors.Contains("Recent8HoursO3"))
                            {
                                dv.RowFilter = "PointId='" + portItem + "' and Max8HourO3_IAQI<= " + StandAQI;
                                drNew["StandRecent8HoursO3"] = dv.Count;
                            }
                        }
                        if (strFactor.Contains("首要污染物"))
                        {
                            if (factors.Contains("PM25"))
                            {
                                dv.RowFilter = "PointId='" + portItem + "' and PrimaryPollutant like '%PM2.5%'";
                                drNew["PrimaryPM25"] = dv.Count;
                            }
                            if (factors.Contains("PM10"))
                            {
                                dv.RowFilter = "PointId='" + portItem + "' and PrimaryPollutant like '%PM10%'";
                                drNew["PrimaryPM10"] = dv.Count;
                            }
                            if (factors.Contains("NO2"))
                            {
                                dv.RowFilter = "PointId='" + portItem + "' and PrimaryPollutant like '%NO2%'";
                                drNew["PrimaryNO2"] = dv.Count;
                            }
                            if (factors.Contains("SO2"))
                            {
                                dv.RowFilter = "PointId='" + portItem + "' and PrimaryPollutant like '%SO2%'";
                                drNew["PrimarySO2"] = dv.Count;
                            }
                            if (factors.Contains("CO"))
                            {
                                dv.RowFilter = "PointId='" + portItem + "' and PrimaryPollutant like '%CO%'";
                                drNew["PrimaryCO"] = dv.Count;
                            }
                            if (factors.Contains("RecentoneHoursO3") || factors.Contains("Recent8HoursO3"))
                            {
                                dv.RowFilter = "PointId='" + portItem + "' and PrimaryPollutant like '%O3%'";
                                drNew["PrimaryO3"] = dv.Count;
                            }
                        }
                    }
                    dt.Rows.Add(drNew);

                }
                recordTotal = dt.Rows.Count;
                return dt;
            }
            catch (Exception ex)
            {
                recordTotal = 0;
                return null;
            }
        }
        /// <summary>
        /// 取得导出数据（行转列数据）
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetAreaExportData(string[] regionGuids, DateTime dtStart, DateTime dtEnd, string orderBy = "ReportDateTime,MonitoringRegionUid")
        {
            regionDayAQI = Singleton<RegionDayAQIRepository>.GetInstance();
            if (regionDayAQI != null)
                return regionDayAQI.GetExportData(regionGuids, dtStart, dtEnd, orderBy);
            return null;
        }

        /// <summary>
        /// 各污染物首要污染物统计
        /// </summary>
        /// <param name="aqiType">AQI分指标</param>
        /// <param name="portIds">测点</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetAreaContaminantsStatistics(IAQIType aqiType, string[] regionGuids, DateTime dtStart, DateTime dtEnd)
        {
            regionDayAQI = Singleton<RegionDayAQIRepository>.GetInstance();
            if (regionDayAQI != null)
                return regionDayAQI.GetContaminantsStatistics(aqiType, regionGuids, dtStart, dtEnd);

            return null;
        }

        /// <summary>
        /// 全市年数据统计
        /// </summary>
        /// <param name="regionGuids">区域Guid</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <param name="orderBy">排序</param>
        /// <returns>
        /// RegionName：区域
        /// FactorName：因子
        /// DayMinValue：最小值
        /// DayMaxValue：最大值
        /// OutDays：超标天数
        /// MonitorDays：监控天数
        /// OutRate：超标率
        /// OutBiggestFactor：最大超标倍数
        /// YearAverage：年均值
        /// </returns>
        public DataView GetAllYearDataNew(string[] portIds, string[] facor, DateTime dateStart, DateTime dateEnd)
        {
            try
            {
                RegionDayAQIDAL m_RegionAQIDAL = Singleton<RegionDayAQIDAL>.GetInstance();
                AirPollutantService m_AirPollutantService = new AirPollutantService();
                int PM25Unit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a34004").PollutantDecimalNum);
                int PM10Unit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a34002").PollutantDecimalNum);
                int NO2Unit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a21004").PollutantDecimalNum);
                int SO2Unit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a21026").PollutantDecimalNum);
                int COUnit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a21005").PollutantDecimalNum);
                int Max8HourO3Unit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a05024").PollutantDecimalNum);
                AQICalculateService m_AQICalculateService = new AQICalculateService();
                MonitoringPointAirService pointAirService = Singleton<MonitoringPointAirService>.GetInstance();
                DataView AreaData = m_AQICalculateService.GetRegionAQI(portIds, dateStart, dateEnd, 24, "2").AsDataView();
                foreach (DataColumn dc in AreaData.Table.Columns)
                {
                    if (dc.ColumnName.Equals("PointId"))
                    {
                        dc.ColumnName = "RegionName";
                    }
                }
                //DataTable dtForAQI = AreaData.Table;
                //DataView dvRegion = pointAirService.GetRegionByPointId(portIds);                

                EQIConcentrationService EQIConcentration = new EQIConcentrationService();
                EQIConcentrationLimitEntity entity = null;
                EQIConcentrationLimitEntity entityyear = null;

                DataTable MinValues = m_RegionAQIDAL.getRegionData_TongJi(portIds, dateStart, dateEnd, "MIN");
                DataTable MaxValues = m_RegionAQIDAL.getRegionData_TongJi(portIds, dateStart, dateEnd, "MAX");
                DataTable AvgValues = m_RegionAQIDAL.getRegionData_TongJi(portIds, dateStart, dateEnd, "AVG");
                DataTable ExceedingDatas = m_RegionAQIDAL.getExceedingDatas(portIds, dateStart, dateEnd);

                DictionaryService dictionary = new DictionaryService();
                DataTable dt = new DataTable();
                dt.Columns.Add("RegionName", typeof(string));
                dt.Columns.Add("FactorName", typeof(string));
                dt.Columns.Add("DayMinValue", typeof(string));
                dt.Columns.Add("DayMaxValue", typeof(string));
                dt.Columns.Add("DayAvgValue", typeof(string));
                dt.Columns.Add("OutDays", typeof(string));
                dt.Columns.Add("MonitorDays", typeof(string));
                dt.Columns.Add("OutRate", typeof(string));
                dt.Columns.Add("OutBiggestFactor", typeof(string));
                dt.Columns.Add("OutDate", typeof(string));
                dt.Columns.Add("DataRange", typeof(string));
                dt.Columns.Add("YearPercent", typeof(string));
                dt.Columns.Add("YearPerOutRate", typeof(string));
                dt.Columns.Add("MedianUpper", typeof(string));
                dt.Columns.Add("MedianLower", typeof(string));
                dt.Columns.Add("MedianValueu", typeof(string));
                dt.Columns.Add("lower", typeof(string));
                dt.Columns.Add("upper", typeof(string));
                List<IAQIType> AQITypes = new List<IAQIType>();
                foreach (string item in facor)
                {
                    if (item == "SO2")
                        AQITypes.Add(IAQIType.SO2_IAQI);
                    if (item == "NO2")
                        AQITypes.Add(IAQIType.NO2_IAQI);
                    if (item == "PM10")
                        AQITypes.Add(IAQIType.PM10_IAQI);
                    if (item == "PM25")
                        AQITypes.Add(IAQIType.PM25_IAQI);
                    if (item == "CO")
                        AQITypes.Add(IAQIType.CO_IAQI);
                    if (item == "O3_8h")
                        AQITypes.Add(IAQIType.Max8HourO3_IAQI);
                }
                string[] namesValues = MinValues.AsEnumerable().Select(t => t.Field<string>("RegionName")).ToArray();
                foreach (string name in namesValues)
                {
                    DataRow[] drMin = MinValues.Select("RegionName='" + name + "'");
                    DataRow[] drMax = MaxValues.Select("RegionName='" + name + "'");
                    DataRow[] drExceeding = ExceedingDatas.Select("RegionName='" + name + "'");
                    DataRow[] drAvg = AvgValues.Select("RegionName='" + name + "'");

                    if (drMin.Length > 0 && drMax.Length > 0 && drExceeding.Length > 0 && drAvg.Length > 0)
                    {
                        for (int j = 0; j < AQITypes.Count; j++)
                        {
                            DataRow dr = dt.NewRow();
                            dr["RegionName"] = name;
                            switch (AQITypes[j])
                            {
                                case IAQIType.SO2_IAQI:
                                    entity = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a21026", EQITimeType.TwentyFour);
                                    entityyear = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a21026", EQITimeType.Year);
                                    string yearpercentSO2 = "";
                                    AreaData.RowFilter = "SO2 is not null and convert(SO2, 'System.String')<>'' and RegionName='" + name + "'";
                                    List<decimal> listSO2 = AreaData.ToTable().AsEnumerable().Select(r => Convert.ToDecimal(r.Field<string>("SO2"))).ToList<decimal>();
                                    listSO2.Sort();
                                    decimal[] arrSO2 = listSO2.ToArray();
                                    yearpercentSO2 = (DecimalExtension.GetPollutantValue(getpercentM("a21026", arrSO2), 3) * 1000).ToString("G0");
                                    dr["FactorName"] = "SO2 (μg/m<sup>3</sup>)";
                                    decimal MinValue = (DecimalExtension.GetPollutantValue(decimal.TryParse(drMin[0]["SO2"].ToString(), out MinValue) ? MinValue : 0, SO2Unit)) * 1000;
                                    dr["DayMinValue"] = MinValue != 0 ? MinValue.ToString("G0") : "";
                                    decimal MaxValue = (DecimalExtension.GetPollutantValue(decimal.TryParse(drMax[0]["SO2"].ToString(), out MaxValue) ? MaxValue : 0, SO2Unit)) * 1000;
                                    dr["DayMaxValue"] = MaxValue != 0 ? MaxValue.ToString("G0") : "";
                                    decimal AvgValue = (DecimalExtension.GetPollutantValue(decimal.TryParse(drAvg[0]["SO2"].ToString(), out AvgValue) ? AvgValue : 0, SO2Unit)) * 1000;
                                    dr["DayAvgValue"] = AvgValue != 0 ? AvgValue.ToString("G0") : "";
                                    dr["DataRange"] = MinValue.ToString("G0") + "~" + MaxValue.ToString("G0");
                                    dr["OutDays"] = drExceeding[0]["SO2_Over"];
                                    dr["MonitorDays"] = AreaData.ToTable().Select("RegionName='" + name + "' and DateTime>='" + dateStart +
                                        "' and DateTime<='" + dateEnd + "' and SO2_IAQI is not null and SO2_IAQI<>0").Count();
                                    decimal outSO2 = 0;
                                    if (Convert.ToDecimal(dr["MonitorDays"]) != 0)
                                    {
                                        outSO2 = (decimal)(Convert.ToDecimal(dr["OutDays"]) / Convert.ToDecimal(dr["MonitorDays"]) * 100);
                                    }
                                    dr["OutRate"] = DecimalExtension.GetPollutantValue(outSO2, 1).ToString() + "%";
                                    decimal dSO2 = (decimal)((Convert.ToDecimal(drMax[0]["SO2"]) - entity.Upper) / entity.Upper);
                                    string strdSO2 = "";
                                    if (dSO2 < 0)
                                    {
                                        strdSO2 = "/";
                                        dr["OutDate"] = "/";
                                    }
                                    else
                                    {
                                        strdSO2 = DecimalExtension.GetPollutantValue(dSO2, 2).ToString();
                                        DataRow[] SO2 = AreaData.ToTable().Select("RegionName='" + name + "' and DateTime>='" + dateStart +
                                        "' and DateTime<='" + dateEnd + "' and convert(SO2, 'System.String') like'" + drMax[0]["SO2"].ToString() + "%'");
                                        if (SO2.Length > 0)
                                            if (SO2[0]["DateTime"].IsNotNullOrDBNull())
                                                dr["OutDate"] = string.Format("{0:yyyy-MM-dd}", SO2[0]["DateTime"]);
                                    }
                                    dr["OutBiggestFactor"] = strdSO2;
                                    if (yearpercentSO2 == null || yearpercentSO2 == "")
                                    {
                                        dr["YearPercent"] = "/";
                                    }
                                    else
                                    {
                                        dr["YearPercent"] = yearpercentSO2;
                                    }
                                    if (entityyear != null && yearpercentSO2 != null && yearpercentSO2 != "" && entityyear.Upper != null && entityyear.Upper != 0)
                                    {
                                        decimal dYearPerSO2 = (decimal)((Convert.ToDecimal(yearpercentSO2) / 1000 - entityyear.Upper) / entityyear.Upper);
                                        string strdYearPerSO2 = "";
                                        if (dYearPerSO2 < 0)
                                        {
                                            strdYearPerSO2 = "/";
                                        }
                                        else
                                        {
                                            strdYearPerSO2 = DecimalExtension.GetPollutantValue(dYearPerSO2, 2).ToString();
                                        }
                                        dr["YearPerOutRate"] = strdYearPerSO2;
                                    }

                                    List<Decimal> decSO2 = AreaData.ToTable().AsEnumerable().Select(r => Convert.ToDecimal(r.Field<string>("SO2"))).ToList();
                                    Dictionary<string, string> ListMedian = Median(decSO2, "");
                                    if (ListMedian.Keys.Contains("median"))
                                    {
                                        dr["MedianValueu"] = ListMedian["median"];
                                    }
                                    if (ListMedian.Keys.Contains("median1"))
                                    {
                                        dr["MedianLower"] = ListMedian["median1"];
                                    }
                                    if (ListMedian.Keys.Contains("median3"))
                                    {
                                        dr["MedianUpper"] = ListMedian["median3"];
                                    } if (ListMedian.Keys.Contains("lower"))
                                    {
                                        dr["lower"] = ListMedian["lower"];
                                    } if (ListMedian.Keys.Contains("upper"))
                                    {
                                        dr["upper"] = ListMedian["upper"];
                                    }
                                    AreaData.RowFilter = string.Empty;
                                    break;
                                case IAQIType.NO2_IAQI:
                                    entity = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a21004", EQITimeType.TwentyFour);
                                    entityyear = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a21004", EQITimeType.Year);
                                    string yearpercentNO2 = "";
                                    AreaData.RowFilter = "NO2 is not null and convert(NO2, 'System.String')<>'' and RegionName='" + name + "'";
                                    List<decimal> listNO2 = AreaData.ToTable().AsEnumerable().Select(r => Convert.ToDecimal(r.Field<string>("NO2"))).ToList<decimal>();
                                    listNO2.Sort();
                                    decimal[] arrNO2 = listNO2.ToArray();
                                    yearpercentNO2 = (DecimalExtension.GetPollutantValue(getpercentM("a21004", arrNO2), 3) * 1000).ToString("G0");
                                    dr["FactorName"] = "NO2 (μg/m<sup>3</sup>)";
                                    decimal MinValueNO2 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drMin[0]["NO2"].ToString(), out MinValueNO2) ? MinValueNO2 : 0, NO2Unit)) * 1000;
                                    dr["DayMinValue"] = MinValueNO2 != 0 ? MinValueNO2.ToString("G0") : "";
                                    decimal MaxValueNO2 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drMax[0]["NO2"].ToString(), out MaxValueNO2) ? MaxValueNO2 : 0, NO2Unit)) * 1000;
                                    dr["DayMaxValue"] = MaxValueNO2 != 0 ? MaxValueNO2.ToString("G0") : "";
                                    decimal AvgValueNO2 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drAvg[0]["NO2"].ToString(), out AvgValueNO2) ? AvgValueNO2 : 0, NO2Unit)) * 1000;
                                    dr["DayAvgValue"] = AvgValueNO2 != 0 ? AvgValueNO2.ToString("G0") : "";
                                    dr["DataRange"] = MinValueNO2.ToString("G0") + "~" + MaxValueNO2.ToString("G0");
                                    dr["OutDays"] = drExceeding[0]["NO2_Over"];
                                    dr["MonitorDays"] = AreaData.ToTable().Select("RegionName='" + name + "' and DateTime>='" + dateStart +
                                        "' and DateTime<='" + dateEnd + "' and NO2_IAQI is not null and NO2_IAQI<>0").Count();
                                    decimal outNO2 = 0;
                                    if (Convert.ToDecimal(dr["MonitorDays"]) != 0)
                                    {
                                        outNO2 = (decimal)(Convert.ToDecimal(dr["OutDays"]) / Convert.ToDecimal(dr["MonitorDays"]) * 100);
                                    }
                                    dr["OutRate"] = DecimalExtension.GetPollutantValue(outNO2, 1).ToString() + "%";
                                    decimal dNO2 = (decimal)((Convert.ToDecimal(drMax[0]["NO2"]) - entity.Upper) / entity.Upper);
                                    string strdNO2 = "";
                                    if (dNO2 < 0)
                                    {
                                        strdNO2 = "/";
                                        dr["OutDate"] = "/";
                                    }
                                    else
                                    {
                                        strdNO2 = DecimalExtension.GetPollutantValue(dNO2, 2).ToString();
                                        DataRow[] NO2 = AreaData.ToTable().Select("RegionName='" + name + "' and DateTime>='" + dateStart +
                                   "' and DateTime<='" + dateEnd + "' and convert(NO2, 'System.String') like'" + drMax[0]["NO2"].ToString() + "%'");
                                        if (NO2.Length > 0)
                                            if (NO2[0]["DateTime"].IsNotNullOrDBNull())
                                                dr["OutDate"] = string.Format("{0:yyyy-MM-dd}", NO2[0]["DateTime"]);
                                    }
                                    dr["OutBiggestFactor"] = strdNO2;
                                    if (yearpercentNO2 == null || yearpercentNO2 == "")
                                    {
                                        dr["YearPercent"] = "/";
                                    }
                                    else
                                    {
                                        dr["YearPercent"] = yearpercentNO2;
                                    }
                                    if (entityyear != null && yearpercentNO2 != null && yearpercentNO2 != "" && entityyear.Upper != null && entityyear.Upper != 0)
                                    {
                                        decimal dYearPerNO2 = (decimal)((Convert.ToDecimal(yearpercentNO2) / 1000 - entityyear.Upper) / entityyear.Upper);
                                        string strdYearPerNO2 = "";
                                        if (dYearPerNO2 < 0)
                                        {
                                            strdYearPerNO2 = "/";
                                        }
                                        else
                                        {
                                            strdYearPerNO2 = DecimalExtension.GetPollutantValue(dYearPerNO2, 2).ToString();
                                        }
                                        dr["YearPerOutRate"] = strdYearPerNO2;
                                    }
                                    List<Decimal> decNO2 = AreaData.ToTable().AsEnumerable().Select(r => Convert.ToDecimal(r.Field<string>("NO2"))).ToList();
                                    Dictionary<string, string> ListMedianNO2 = Median(decNO2, "");
                                    if (ListMedianNO2.Keys.Contains("median"))
                                    {
                                        dr["MedianValueu"] = ListMedianNO2["median"];
                                    }
                                    if (ListMedianNO2.Keys.Contains("median1"))
                                    {
                                        dr["MedianLower"] = ListMedianNO2["median1"];
                                    }
                                    if (ListMedianNO2.Keys.Contains("median3"))
                                    {
                                        dr["MedianUpper"] = ListMedianNO2["median3"];
                                    } if (ListMedianNO2.Keys.Contains("lower"))
                                    {
                                        dr["lower"] = ListMedianNO2["lower"];
                                    } if (ListMedianNO2.Keys.Contains("upper"))
                                    {
                                        dr["upper"] = ListMedianNO2["upper"];
                                    }
                                    AreaData.RowFilter = string.Empty;
                                    break;
                                case IAQIType.PM10_IAQI:
                                    entity = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a34002", EQITimeType.TwentyFour);
                                    entityyear = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a34002", EQITimeType.Year);
                                    string yearpercentPM10 = "";
                                    AreaData.RowFilter = "PM10 is not null and convert(PM10, 'System.String')<>''  and RegionName='" + name + "'";
                                    List<decimal> listPM10 = AreaData.ToTable().AsEnumerable().Select(r => Convert.ToDecimal(r.Field<string>("PM10"))).ToList<decimal>();
                                    listPM10.Sort();
                                    decimal[] arrPM10 = listPM10.ToArray();
                                    yearpercentPM10 = (DecimalExtension.GetPollutantValue(getpercentM("a34002", arrPM10), 3) * 1000).ToString("G0");
                                    dr["FactorName"] = "PM10 (μg/m<sup>3</sup>)";
                                    decimal MinValuePM10 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drMin[0]["PM10"].ToString(), out MinValuePM10) ? MinValuePM10 : 0, PM10Unit)) * 1000;
                                    dr["DayMinValue"] = MinValuePM10 != 0 ? MinValuePM10.ToString("G0") : "";
                                    decimal MaxValuePM10 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drMax[0]["PM10"].ToString(), out MaxValuePM10) ? MaxValuePM10 : 0, PM10Unit)) * 1000;
                                    dr["DayMaxValue"] = MaxValuePM10 != 0 ? MaxValuePM10.ToString("G0") : "";
                                    decimal AvgValuePM10 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drAvg[0]["PM10"].ToString(), out AvgValuePM10) ? AvgValuePM10 : 0, PM10Unit)) * 1000;
                                    dr["DayAvgValue"] = AvgValuePM10 != 0 ? AvgValuePM10.ToString("G0") : "";
                                    dr["DataRange"] = MinValuePM10.ToString("G0") + "~" + MaxValuePM10.ToString("G0");
                                    dr["OutDays"] = drExceeding[0]["PM10_Over"];
                                    dr["MonitorDays"] = AreaData.ToTable().Select("RegionName='" + name + "' and DateTime>='" + dateStart +
                                        "' and DateTime<='" + dateEnd + "' and PM10_IAQI is not null and PM10_IAQI<>0").Count();
                                    decimal outPM10 = 0;
                                    if (Convert.ToDecimal(dr["MonitorDays"]) != 0)
                                    {
                                        outPM10 = (decimal)(Convert.ToDecimal(dr["OutDays"]) / Convert.ToDecimal(dr["MonitorDays"]) * 100);
                                    }
                                    dr["OutRate"] = DecimalExtension.GetPollutantValue(outPM10, 1).ToString() + "%";
                                    decimal dPM10 = (decimal)(Convert.ToDecimal(drMax[0]["PM10"]) - entity.Upper);
                                    string strdPM10 = "";
                                    if (dPM10 < 0)
                                    {
                                        strdPM10 = "/";
                                        dr["OutDate"] = "/";
                                    }
                                    else
                                    {
                                        strdPM10 = DecimalExtension.GetPollutantValue(dPM10, 2).ToString();
                                        DataRow[] PM10 = AreaData.ToTable().Select("RegionName='" + name + "' and DateTime>='" + dateStart +
                                                         "' and DateTime<='" + dateEnd + "' and convert(PM10, 'System.String') like'" + drMax[0]["PM10"].ToString() + "%'");
                                        if (PM10.Length > 0)
                                            if (PM10[0]["DateTime"].IsNotNullOrDBNull())
                                                dr["OutDate"] = string.Format("{0:yyyy-MM-dd}", PM10[0]["DateTime"]);
                                    }
                                    dr["OutBiggestFactor"] = strdPM10;
                                    if (yearpercentPM10 == null || yearpercentPM10 == "")
                                    {
                                        dr["YearPercent"] = "/";
                                    }
                                    else
                                    {
                                        dr["YearPercent"] = yearpercentPM10;
                                    }
                                    if (entityyear != null && yearpercentPM10 != null && yearpercentPM10 != "" && entityyear.Upper != null && entityyear.Upper != 0)
                                    {
                                        decimal dYearPerPM10 = (decimal)((Convert.ToDecimal(yearpercentPM10) / 1000 - entityyear.Upper) / entityyear.Upper);
                                        string strdYearPerPM10 = "";
                                        if (dYearPerPM10 < 0)
                                        {
                                            strdYearPerPM10 = "/";
                                        }
                                        else
                                        {
                                            strdYearPerPM10 = DecimalExtension.GetPollutantValue(dYearPerPM10, 2).ToString();
                                        }
                                        dr["YearPerOutRate"] = strdYearPerPM10;
                                    }
                                    List<Decimal> decPM10 = AreaData.ToTable().AsEnumerable().Select(r => Convert.ToDecimal(r.Field<string>("PM10"))).ToList();
                                    Dictionary<string, string> ListMedianPM10 = Median(decPM10, "");
                                    if (ListMedianPM10.Keys.Contains("median"))
                                    {
                                        dr["MedianValueu"] = ListMedianPM10["median"];
                                    }
                                    if (ListMedianPM10.Keys.Contains("median1"))
                                    {
                                        dr["MedianLower"] = ListMedianPM10["median1"];
                                    }
                                    if (ListMedianPM10.Keys.Contains("median3"))
                                    {
                                        dr["MedianUpper"] = ListMedianPM10["median3"];
                                    } if (ListMedianPM10.Keys.Contains("lower"))
                                    {
                                        dr["lower"] = ListMedianPM10["lower"];
                                    } if (ListMedianPM10.Keys.Contains("upper"))
                                    {
                                        dr["upper"] = ListMedianPM10["upper"];
                                    }
                                    AreaData.RowFilter = string.Empty;
                                    break;
                                case IAQIType.PM25_IAQI:
                                    entity = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a34004", EQITimeType.TwentyFour);
                                    entityyear = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a34004", EQITimeType.Year);
                                    string yearpercentPM25 = "";
                                    AreaData.RowFilter = "PM25 is not null and convert(PM25, 'System.String')<>''  and RegionName='" + name + "'";
                                    List<Decimal> listPM25 = AreaData.ToTable().AsEnumerable().Select(r => Convert.ToDecimal(r.Field<string>("PM25"))).ToList();
                                    listPM25.Sort();
                                    decimal[] arrPM25 = listPM25.ToArray();
                                    yearpercentPM25 = (DecimalExtension.GetPollutantValue(getpercentM("a34004", arrPM25), 3) * 1000).ToString("G0");
                                    dr["FactorName"] = "PM2.5 (μg/m<sup>3</sup>)";
                                    decimal MinValuePM25 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drMin[0]["PM25"].ToString(), out MinValuePM25) ? MinValuePM25 : 0, PM25Unit)) * 1000;
                                    dr["DayMinValue"] = MinValuePM25 != 0 ? MinValuePM25.ToString("G0") : "";
                                    decimal MaxValuePM25 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drMax[0]["PM25"].ToString(), out MaxValuePM25) ? MaxValuePM25 : 0, PM25Unit)) * 1000;
                                    dr["DayMaxValue"] = MaxValuePM25 != 0 ? MaxValuePM25.ToString("G0") : "";
                                    decimal AvgValuePM25 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drAvg[0]["PM25"].ToString(), out AvgValuePM25) ? AvgValuePM25 : 0, PM25Unit)) * 1000;
                                    dr["DayAvgValue"] = AvgValuePM25 != 0 ? AvgValuePM25.ToString("G0") : "";
                                    dr["DataRange"] = MinValuePM25.ToString("G0") + "~" + MaxValuePM25.ToString("G0");
                                    dr["OutDays"] = drExceeding[0]["PM25_Over"];
                                    dr["MonitorDays"] = AreaData.ToTable().Select("RegionName='" + name + "' and DateTime>='" + dateStart +
                                        "' and DateTime<='" + dateEnd + "' and PM25_IAQI is not null and PM25_IAQI<>0").Count();
                                    decimal outPM25 = 0;
                                    if (Convert.ToDecimal(dr["MonitorDays"]) != 0)
                                    {
                                        outPM25 = (decimal)(Convert.ToDecimal(dr["OutDays"]) / Convert.ToDecimal(dr["MonitorDays"]) * 100);
                                    }
                                    dr["OutRate"] = DecimalExtension.GetPollutantValue(outPM25, 1).ToString() + "%";
                                    //dr["OutBiggestFactor"] = ((Convert.ToDecimal(drMax[0]["PM25"]) - entity.Upper) / entity.Upper).ToString();
                                    decimal dPM25 = (decimal)((Convert.ToDecimal(drMax[0]["PM25"]) - entity.Upper) / entity.Upper);
                                    string strdPM25 = "";
                                    if (dPM25 < 0)
                                    {
                                        strdPM25 = "/";
                                        dr["OutDate"] = "/";
                                    }
                                    else
                                    {
                                        strdPM25 = DecimalExtension.GetPollutantValue(dPM25, 2).ToString();
                                        DataRow[] PM25 = AreaData.ToTable().Select("RegionName='" + name + "' and DateTime>='" + dateStart +
                                                         "' and DateTime<='" + dateEnd + "' and convert(PM25, 'System.String') like'" + drMax[0]["PM25"].ToString() + "%'");
                                        if (PM25.Length > 0)
                                            if (PM25[0]["DateTime"].IsNotNullOrDBNull())
                                                dr["OutDate"] = string.Format("{0:yyyy-MM-dd}", PM25[0]["DateTime"]);
                                    }
                                    dr["OutBiggestFactor"] = strdPM25;
                                    if (yearpercentPM25 == null || yearpercentPM25 == "")
                                    {
                                        dr["YearPercent"] = "/";
                                    }
                                    else
                                    {
                                        dr["YearPercent"] = yearpercentPM25;
                                    }
                                    if (entityyear != null && yearpercentPM25 != null && yearpercentPM25 != "" && entityyear.Upper != null && entityyear.Upper != 0)
                                    {
                                        decimal dYearPerPM25 = (decimal)((Convert.ToDecimal(yearpercentPM25) / 1000 - entityyear.Upper) / entityyear.Upper);
                                        string strdYearPerPM25 = "";
                                        if (dYearPerPM25 < 0)
                                        {
                                            strdYearPerPM25 = "/";
                                        }
                                        else
                                        {
                                            strdYearPerPM25 = DecimalExtension.GetPollutantValue(dYearPerPM25, 2).ToString();
                                        }
                                        dr["YearPerOutRate"] = strdYearPerPM25;
                                    }
                                    List<Decimal> decPM25 = AreaData.ToTable().AsEnumerable().Select(r => Convert.ToDecimal(r.Field<string>("PM25"))).ToList();
                                    Dictionary<string, string> ListMedianPM25 = Median(decPM25, "");
                                    if (ListMedianPM25.Keys.Contains("median"))
                                    {
                                        dr["MedianValueu"] = ListMedianPM25["median"];
                                    }
                                    if (ListMedianPM25.Keys.Contains("median1"))
                                    {
                                        dr["MedianLower"] = ListMedianPM25["median1"];
                                    }
                                    if (ListMedianPM25.Keys.Contains("median3"))
                                    {
                                        dr["MedianUpper"] = ListMedianPM25["median3"];
                                    } if (ListMedianPM25.Keys.Contains("lower"))
                                    {
                                        dr["lower"] = ListMedianPM25["lower"];
                                    } if (ListMedianPM25.Keys.Contains("upper"))
                                    {
                                        dr["upper"] = ListMedianPM25["upper"];
                                    }
                                    AreaData.RowFilter = string.Empty;
                                    break;
                                case IAQIType.CO_IAQI:
                                    entity = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a21005", EQITimeType.TwentyFour);
                                    entityyear = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a21005", EQITimeType.Year);
                                    string yearpercentCO = "";
                                    AreaData.RowFilter = "CO is not null and convert(CO, 'System.String')<>''  and RegionName='" + name + "'";
                                    List<decimal> listCO = AreaData.ToTable().AsEnumerable().Select(r => Convert.ToDecimal(r.Field<string>("CO"))).ToList<decimal>();
                                    listCO.Sort();
                                    decimal[] arrCO = listCO.ToArray();
                                    yearpercentCO = (DecimalExtension.GetPollutantValue(getpercentM("a21005", arrCO), 1)).ToString();
                                    dr["FactorName"] = "CO (mg/m<sup>3</sup>)";
                                    decimal MinValueCO = (DecimalExtension.GetPollutantValue(decimal.TryParse(drMin[0]["CO"].ToString(), out MinValueCO) ? MinValueCO : 0, COUnit));
                                    dr["DayMinValue"] = MinValueCO != 0 ? MinValueCO.ToString() : "";
                                    decimal MaxValueCO = (DecimalExtension.GetPollutantValue(decimal.TryParse(drMax[0]["CO"].ToString(), out MaxValueCO) ? MaxValueCO : 0, COUnit));
                                    dr["DayMaxValue"] = MaxValueCO != 0 ? MaxValueCO.ToString() : "";
                                    decimal AvgValueCO = (DecimalExtension.GetPollutantValue(decimal.TryParse(drAvg[0]["CO"].ToString(), out AvgValueCO) ? AvgValueCO : 0, COUnit));
                                    dr["DayAvgValue"] = AvgValueCO != 0 ? AvgValueCO.ToString() : "";
                                    dr["DataRange"] = MinValueCO.ToString() + "~" + MaxValueCO.ToString();
                                    dr["OutDays"] = drExceeding[0]["CO_Over"];
                                    dr["MonitorDays"] = AreaData.ToTable().Select("RegionName='" + name + "' and DateTime>='" + dateStart +
                                        "' and DateTime<='" + dateEnd + "' and CO_IAQI is not null and CO_IAQI<>0").Count();
                                    decimal outCO = 0;
                                    if (Convert.ToDecimal(dr["MonitorDays"]) != 0)
                                    {
                                        outCO = (decimal)(Convert.ToDecimal(dr["OutDays"]) / Convert.ToDecimal(dr["MonitorDays"]) * 100);
                                    }
                                    dr["OutRate"] = DecimalExtension.GetPollutantValue(outCO, 1).ToString() + "%";

                                    //dr["OutBiggestFactor"] = ((Convert.ToDecimal(drMax[0]["CO"]) - entity.Upper) / entity.Upper).ToString();
                                    decimal dCO = (decimal)((Convert.ToDecimal(drMax[0]["CO"]) - entity.Upper) / entity.Upper);
                                    string strdCO = "";
                                    if (dCO < 0)
                                    {
                                        strdCO = "/";
                                        dr["OutDate"] = "/";
                                    }
                                    else
                                    {
                                        strdCO = DecimalExtension.GetPollutantValue(dCO, 2).ToString();
                                        DataRow[] CO = AreaData.ToTable().Select("RegionName='" + name + "' and DateTime>='" + dateStart +
                                                      "' and DateTime<='" + dateEnd + "' and convert(CO, 'System.String') like'" + drMax[0]["CO"].ToString() + "%'");
                                        if (CO.Length > 0)
                                            if (CO[0]["DateTime"].IsNotNullOrDBNull())
                                                dr["OutDate"] = string.Format("{0:yyyy-MM-dd}", CO[0]["DateTime"]);
                                    }
                                    dr["OutBiggestFactor"] = strdCO;
                                    if (yearpercentCO == null || yearpercentCO == "")
                                    {
                                        dr["YearPercent"] = "/";
                                    }
                                    else
                                    {
                                        dr["YearPercent"] = yearpercentCO;
                                    }
                                    if (entityyear != null && yearpercentCO != null && yearpercentCO != "" && entityyear.Upper != null && entityyear.Upper != 0)
                                    {
                                        decimal dYearPerCO = (decimal)((Convert.ToDecimal(yearpercentCO) - entityyear.Upper) / entityyear.Upper);
                                        string strdYearPerCO = "";
                                        if (dYearPerCO < 0)
                                        {
                                            strdYearPerCO = "/";
                                        }
                                        else
                                        {
                                            strdYearPerCO = DecimalExtension.GetPollutantValue(dYearPerCO, 2).ToString();
                                        }
                                        dr["YearPerOutRate"] = strdYearPerCO;
                                    }
                                    List<Decimal> decCO = AreaData.ToTable().AsEnumerable().Select(r => Convert.ToDecimal(r.Field<string>("CO"))).ToList();
                                    Dictionary<string, string> ListMedianCO = Median(decCO, "CO");
                                    if (ListMedianCO.Keys.Contains("median"))
                                    {
                                        dr["MedianValueu"] = ListMedianCO["median"];
                                    }
                                    if (ListMedianCO.Keys.Contains("median1"))
                                    {
                                        dr["MedianLower"] = ListMedianCO["median1"];
                                    }
                                    if (ListMedianCO.Keys.Contains("median3"))
                                    {
                                        dr["MedianUpper"] = ListMedianCO["median3"];
                                    } if (ListMedianCO.Keys.Contains("lower"))
                                    {
                                        dr["lower"] = ListMedianCO["lower"];
                                    } if (ListMedianCO.Keys.Contains("upper"))
                                    {
                                        dr["upper"] = ListMedianCO["upper"];
                                    }
                                    AreaData.RowFilter = string.Empty;
                                    break;
                                case IAQIType.Max8HourO3_IAQI:
                                    entity = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a05024", EQITimeType.Eight);
                                    //entityyear = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a05024", EQITimeType.Year);
                                    string yearpercentO3 = "";
                                    //yearpercentO3 = pointDayAQI.getpercent(name, dateStart, dateEnd, "a05024");
                                    AreaData.RowFilter = "Max8HourO3 is not null and convert(Max8HourO3, 'System.String')<>''  and RegionName='" + name + "'";
                                    List<decimal> listO3 = AreaData.ToTable().AsEnumerable().Select(r => Convert.ToDecimal(r.Field<string>("Max8HourO3"))).ToList<decimal>();
                                    listO3.Sort();
                                    decimal[] arrO3 = listO3.ToArray();
                                    yearpercentO3 = (DecimalExtension.GetPollutantValue(getpercentM("a05024", arrO3), 3) * 1000).ToString("G0");
                                    dr["FactorName"] = "O3-8 (μg/m<sup>3</sup>)";
                                    decimal MinValueMax8HourO3 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drMin[0]["Max8HourO3"].ToString(), out MinValueMax8HourO3) ? MinValueMax8HourO3 : 0, Max8HourO3Unit)) * 1000;
                                    dr["DayMinValue"] = MinValueMax8HourO3 != 0 ? MinValueMax8HourO3.ToString("G0") : "";
                                    decimal MaxValueMax8HourO3 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drMax[0]["Max8HourO3"].ToString(), out MaxValueMax8HourO3) ? MaxValueMax8HourO3 : 0, Max8HourO3Unit)) * 1000;
                                    dr["DayMaxValue"] = MaxValueMax8HourO3 != 0 ? MaxValueMax8HourO3.ToString("G0") : "";
                                    decimal AvgValueMax8HourO3 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drAvg[0]["Max8HourO3"].ToString(), out AvgValueMax8HourO3) ? AvgValueMax8HourO3 : 0, Max8HourO3Unit)) * 1000;
                                    dr["DayAvgValue"] = AvgValueMax8HourO3 != 0 ? AvgValueMax8HourO3.ToString("G0") : "";
                                    dr["DataRange"] = MinValueMax8HourO3.ToString("G0") + "~" + MaxValueMax8HourO3.ToString("G0");
                                    dr["OutDays"] = drExceeding[0]["Max8HourO3_Over"];
                                    dr["MonitorDays"] = AreaData.ToTable().Select("RegionName='" + name + "' and DateTime>='" + dateStart +
                                        "' and DateTime<='" + dateEnd + "' and Max8HourO3_IAQI is not null and Max8HourO3_IAQI<>0").Count();
                                    decimal outMax8HourO3 = 0;
                                    if (Convert.ToDecimal(dr["MonitorDays"]) != 0)
                                    {
                                        outMax8HourO3 = (decimal)(Convert.ToDecimal(dr["OutDays"]) / Convert.ToDecimal(dr["MonitorDays"]) * 100);
                                    }
                                    dr["OutRate"] = DecimalExtension.GetPollutantValue(outMax8HourO3, 1).ToString() + "%";
                                    decimal dMax8HourO3 = (decimal)((Convert.ToDecimal(drMax[0]["Max8HourO3"]) - entity.Upper) / entity.Upper);
                                    string strdMax8HourO3 = "";
                                    if (dMax8HourO3 < 0)
                                    {
                                        strdMax8HourO3 = "/";
                                        dr["OutDate"] = "/";
                                    }
                                    else
                                    {
                                        strdMax8HourO3 = DecimalExtension.GetPollutantValue(dMax8HourO3, 2).ToString();
                                        DataRow[] O3 = AreaData.ToTable().Select("RegionName='" + name + "' and DateTime>='" + dateStart +
                                                   "' and DateTime<='" + dateEnd + "' and convert(Max8HourO3, 'System.String') like'" + drMax[0]["Max8HourO3"].ToString() + "%'");
                                        if (O3.Length > 0)
                                            if (O3[0]["DateTime"].IsNotNullOrDBNull())
                                                dr["OutDate"] = string.Format("{0:yyyy-MM-dd}", O3[0]["DateTime"]);
                                    }
                                    dr["OutBiggestFactor"] = strdMax8HourO3;
                                    decimal YearAverageMax8HourO3 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drAvg[0]["Max8HourO3"].ToString(), out YearAverageMax8HourO3) ? YearAverageMax8HourO3 : 0, Max8HourO3Unit));
                                    //dr["YearAverage"] = YearAverageMax8HourO3 != 0 ? YearAverageMax8HourO3.ToString() : "";
                                    //dr["YearOutRate"] = "/";
                                    if (yearpercentO3 == null || yearpercentO3 == "")
                                    {
                                        dr["YearPercent"] = "/";
                                    }
                                    else
                                    {
                                        dr["YearPercent"] = yearpercentO3;
                                    }
                                    if (entity != null && yearpercentO3 != null && yearpercentO3 != "" && entity.Upper != null && entity.Upper != 0)
                                    {
                                        decimal dYearPerO3 = (decimal)((Convert.ToDecimal(yearpercentO3) / 1000 - entity.Upper) / entity.Upper);
                                        string strdYearPerO3 = "";
                                        if (dYearPerO3 < 0)
                                        {
                                            strdYearPerO3 = "/";
                                        }
                                        else
                                        {
                                            strdYearPerO3 = DecimalExtension.GetPollutantValue(dYearPerO3, 2).ToString();
                                        }
                                        dr["YearPerOutRate"] = strdYearPerO3;
                                    }
                                    //dr["YearPerOutRate"] = "/";
                                    List<Decimal> decMax8HourO3 = AreaData.ToTable().AsEnumerable().Select(r => Convert.ToDecimal(r.Field<string>("Max8HourO3"))).ToList();
                                    Dictionary<string, string> ListMedianMax8HourO3 = Median(decMax8HourO3, "");
                                    if (ListMedianMax8HourO3.Keys.Contains("median"))
                                    {
                                        dr["MedianValueu"] = ListMedianMax8HourO3["median"];
                                    }
                                    if (ListMedianMax8HourO3.Keys.Contains("median1"))
                                    {
                                        dr["MedianLower"] = ListMedianMax8HourO3["median1"];
                                    }
                                    if (ListMedianMax8HourO3.Keys.Contains("median3"))
                                    {
                                        dr["MedianUpper"] = ListMedianMax8HourO3["median3"];
                                    } if (ListMedianMax8HourO3.Keys.Contains("lower"))
                                    {
                                        dr["lower"] = ListMedianMax8HourO3["lower"];
                                    } if (ListMedianMax8HourO3.Keys.Contains("upper"))
                                    {
                                        dr["upper"] = ListMedianMax8HourO3["upper"];
                                    }
                                    AreaData.RowFilter = string.Empty;
                                    break;
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                }
                return dt.DefaultView;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 全市年数据统计(重载方法)
        /// </summary>
        /// <param name="regionGuids">区域Guid</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <param name="orderBy">排序</param>
        /// <returns>
        /// RegionName：区域
        /// FactorName：因子
        /// DayMinValue：最小值
        /// DayMaxValue：最大值
        /// OutDays：超标天数
        /// MonitorDays：监控天数
        /// OutRate：超标率
        /// OutBiggestFactor：最大超标倍数
        /// YearAverage：年均值
        /// </returns>
        public DataView GetAllYearDataNew(string[] portIds, string[] facor, DateTime dateStart, DateTime dateEnd, string flag)
        {
            try
            {
                RegionDayAQIDAL m_RegionAQIDAL = Singleton<RegionDayAQIDAL>.GetInstance();
                AirPollutantService m_AirPollutantService = new AirPollutantService();
                int PM25Unit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a34004").PollutantDecimalNum);
                int PM10Unit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a34002").PollutantDecimalNum);
                int NO2Unit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a21004").PollutantDecimalNum);
                int SO2Unit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a21026").PollutantDecimalNum);
                int COUnit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a21005").PollutantDecimalNum);
                int Max8HourO3Unit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a05024").PollutantDecimalNum);
                AQICalculateService m_AQICalculateService = new AQICalculateService();
                MonitoringPointAirService pointAirService = Singleton<MonitoringPointAirService>.GetInstance();
                DataView AreaData = m_AQICalculateService.GetRegionAQI(portIds, dateStart, dateEnd, 24, "2").AsDataView();
                foreach (DataColumn dc in AreaData.Table.Columns)
                {
                    if (dc.ColumnName.Equals("PointId"))
                    {
                        dc.ColumnName = "RegionName";
                    }
                }

                EQIConcentrationService EQIConcentration = new EQIConcentrationService();
                EQIConcentrationLimitEntity entity = null;
                EQIConcentrationLimitEntity entityyear = null;

                DataTable ExceedingDatas = m_RegionAQIDAL.getExceedingDatas(portIds, dateStart, dateEnd);

                DictionaryService dictionary = new DictionaryService();
                DataTable dt = new DataTable();
                dt.Columns.Add("RegionName", typeof(string));
                dt.Columns.Add("FactorName", typeof(string));
                dt.Columns.Add("OutDays", typeof(string));
                dt.Columns.Add("MonitorDays", typeof(string));
                dt.Columns.Add("OutRate", typeof(string));
                dt.Columns.Add("YearPercent", typeof(string));
                dt.Columns.Add("YearPerOutRate", typeof(string));
                List<IAQIType> AQITypes = new List<IAQIType>();
                foreach (string item in facor)
                {
                    if (item == "SO2")
                        AQITypes.Add(IAQIType.SO2_IAQI);
                    if (item == "NO2")
                        AQITypes.Add(IAQIType.NO2_IAQI);
                    if (item == "PM10")
                        AQITypes.Add(IAQIType.PM10_IAQI);
                    if (item == "PM25")
                        AQITypes.Add(IAQIType.PM25_IAQI);
                    if (item == "CO")
                        AQITypes.Add(IAQIType.CO_IAQI);
                    if (item == "O3_8h")
                        AQITypes.Add(IAQIType.Max8HourO3_IAQI);
                }
                string[] namesValues = AreaData.Table.AsEnumerable().Select(t => t.Field<string>("RegionName")).ToArray();
                foreach (string name in namesValues)
                {
                    DataRow[] drExceeding = ExceedingDatas.Select("RegionName='" + name + "'");

                    if (drExceeding.Length > 0)
                    {
                        for (int j = 0; j < AQITypes.Count; j++)
                        {
                            DataRow dr = dt.NewRow();
                            dr["RegionName"] = name;
                            switch (AQITypes[j])
                            {
                                case IAQIType.SO2_IAQI:
                                    entity = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a21026", EQITimeType.TwentyFour);
                                    entityyear = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a21026", EQITimeType.Year);
                                    string yearpercentSO2 = "";
                                    AreaData.RowFilter = "SO2 is not null and convert(SO2, 'System.String')<>'' and RegionName='" + name + "'";
                                    List<decimal> listSO2 = AreaData.ToTable().AsEnumerable().Select(r => Convert.ToDecimal(r.Field<string>("SO2"))).ToList<decimal>();
                                    listSO2.Sort();
                                    decimal[] arrSO2 = listSO2.ToArray();
                                    yearpercentSO2 = (DecimalExtension.GetPollutantValue(getpercentM("a21026", arrSO2), 3) * 1000).ToString("G0");
                                    dr["FactorName"] = "SO2 (μg/m<sup>3</sup>)";
                                    dr["OutDays"] = drExceeding[0]["SO2_Over"];
                                    dr["MonitorDays"] = AreaData.ToTable().Select("RegionName='" + name + "' and DateTime>='" + dateStart +
                                        "' and DateTime<='" + dateEnd + "' and SO2_IAQI is not null and SO2_IAQI<>0").Count();
                                    decimal outSO2 = 0;
                                    if (Convert.ToDecimal(dr["MonitorDays"]) != 0)
                                    {
                                        outSO2 = (decimal)(Convert.ToDecimal(dr["OutDays"]) / Convert.ToDecimal(dr["MonitorDays"]) * 100);
                                    }
                                    dr["OutRate"] = DecimalExtension.GetPollutantValue(outSO2, 1).ToString() + "%";
                                    if (yearpercentSO2 == null || yearpercentSO2 == "")
                                    {
                                        dr["YearPercent"] = "/";
                                    }
                                    else
                                    {
                                        dr["YearPercent"] = yearpercentSO2;
                                    }
                                    if (entityyear != null && yearpercentSO2 != null && yearpercentSO2 != "" && entityyear.Upper != null && entityyear.Upper != 0)
                                    {
                                        decimal dYearPerSO2 = (decimal)((Convert.ToDecimal(yearpercentSO2) / 1000 - entityyear.Upper) / entityyear.Upper);
                                        string strdYearPerSO2 = "";
                                        if (dYearPerSO2 < 0)
                                        {
                                            strdYearPerSO2 = "/";
                                        }
                                        else
                                        {
                                            strdYearPerSO2 = DecimalExtension.GetPollutantValue(dYearPerSO2, 2).ToString();
                                        }
                                        dr["YearPerOutRate"] = strdYearPerSO2;
                                    }
                                    break;
                                case IAQIType.NO2_IAQI:
                                    entity = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a21004", EQITimeType.TwentyFour);
                                    entityyear = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a21004", EQITimeType.Year);
                                    string yearpercentNO2 = "";
                                    AreaData.RowFilter = "NO2 is not null and convert(NO2, 'System.String')<>'' and RegionName='" + name + "'";
                                    List<decimal> listNO2 = AreaData.ToTable().AsEnumerable().Select(r => Convert.ToDecimal(r.Field<string>("NO2"))).ToList<decimal>();
                                    listNO2.Sort();
                                    decimal[] arrNO2 = listNO2.ToArray();
                                    yearpercentNO2 = (DecimalExtension.GetPollutantValue(getpercentM("a21004", arrNO2), 3) * 1000).ToString("G0");
                                    dr["FactorName"] = "NO2 (μg/m<sup>3</sup>)";
                                    dr["OutDays"] = drExceeding[0]["NO2_Over"];
                                    dr["MonitorDays"] = AreaData.ToTable().Select("RegionName='" + name + "' and DateTime>='" + dateStart +
                                        "' and DateTime<='" + dateEnd + "' and NO2_IAQI is not null and NO2_IAQI<>0").Count();
                                    decimal outNO2 = 0;
                                    if (Convert.ToDecimal(dr["MonitorDays"]) != 0)
                                    {
                                        outNO2 = (decimal)(Convert.ToDecimal(dr["OutDays"]) / Convert.ToDecimal(dr["MonitorDays"]) * 100);
                                    }
                                    dr["OutRate"] = DecimalExtension.GetPollutantValue(outNO2, 1).ToString() + "%";
                                    if (yearpercentNO2 == null || yearpercentNO2 == "")
                                    {
                                        dr["YearPercent"] = "/";
                                    }
                                    else
                                    {
                                        dr["YearPercent"] = yearpercentNO2;
                                    }
                                    if (entityyear != null && yearpercentNO2 != null && yearpercentNO2 != "" && entityyear.Upper != null && entityyear.Upper != 0)
                                    {
                                        decimal dYearPerNO2 = (decimal)((Convert.ToDecimal(yearpercentNO2) / 1000 - entityyear.Upper) / entityyear.Upper);
                                        string strdYearPerNO2 = "";
                                        if (dYearPerNO2 < 0)
                                        {
                                            strdYearPerNO2 = "/";
                                        }
                                        else
                                        {
                                            strdYearPerNO2 = DecimalExtension.GetPollutantValue(dYearPerNO2, 2).ToString();
                                        }
                                        dr["YearPerOutRate"] = strdYearPerNO2;
                                    }
                                    break;
                                case IAQIType.PM10_IAQI:
                                    entity = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a34002", EQITimeType.TwentyFour);
                                    entityyear = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a34002", EQITimeType.Year);
                                    string yearpercentPM10 = "";
                                    AreaData.RowFilter = "PM10 is not null and convert(PM10, 'System.String')<>''  and RegionName='" + name + "'";
                                    List<decimal> listPM10 = AreaData.ToTable().AsEnumerable().Select(r => Convert.ToDecimal(r.Field<string>("PM10"))).ToList<decimal>();
                                    listPM10.Sort();
                                    decimal[] arrPM10 = listPM10.ToArray();
                                    yearpercentPM10 = (DecimalExtension.GetPollutantValue(getpercentM("a34002", arrPM10), 3) * 1000).ToString("G0");
                                    dr["FactorName"] = "PM10 (μg/m<sup>3</sup>)";
                                    dr["OutDays"] = drExceeding[0]["PM10_Over"];
                                    dr["MonitorDays"] = AreaData.ToTable().Select("RegionName='" + name + "' and DateTime>='" + dateStart +
                                        "' and DateTime<='" + dateEnd + "' and PM10_IAQI is not null and PM10_IAQI<>0").Count();
                                    if (yearpercentPM10 == null || yearpercentPM10 == "")
                                    {
                                        dr["YearPercent"] = "/";
                                    }
                                    else
                                    {
                                        dr["YearPercent"] = yearpercentPM10;
                                    }
                                    if (entityyear != null && yearpercentPM10 != null && yearpercentPM10 != "" && entityyear.Upper != null && entityyear.Upper != 0)
                                    {
                                        decimal dYearPerPM10 = (decimal)((Convert.ToDecimal(yearpercentPM10) / 1000 - entityyear.Upper) / entityyear.Upper);
                                        string strdYearPerPM10 = "";
                                        if (dYearPerPM10 < 0)
                                        {
                                            strdYearPerPM10 = "/";
                                        }
                                        else
                                        {
                                            strdYearPerPM10 = DecimalExtension.GetPollutantValue(dYearPerPM10, 2).ToString();
                                        }
                                        dr["YearPerOutRate"] = strdYearPerPM10;
                                    }
                                    break;
                                case IAQIType.PM25_IAQI:
                                    entity = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a34004", EQITimeType.TwentyFour);
                                    entityyear = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a34004", EQITimeType.Year);
                                    string yearpercentPM25 = "";
                                    AreaData.RowFilter = "PM25 is not null and convert(PM25, 'System.String')<>''  and RegionName='" + name + "'";
                                    List<Decimal> listPM25 = AreaData.ToTable().AsEnumerable().Select(r => Convert.ToDecimal(r.Field<string>("PM25"))).ToList();
                                    listPM25.Sort();
                                    decimal[] arrPM25 = listPM25.ToArray();
                                    yearpercentPM25 = (DecimalExtension.GetPollutantValue(getpercentM("a34004", arrPM25), 3) * 1000).ToString("G0");
                                    dr["FactorName"] = "PM2.5 (μg/m<sup>3</sup>)";
                                    dr["OutDays"] = drExceeding[0]["PM25_Over"];
                                    dr["MonitorDays"] = AreaData.ToTable().Select("RegionName='" + name + "' and DateTime>='" + dateStart +
                                        "' and DateTime<='" + dateEnd + "' and PM25_IAQI is not null and PM25_IAQI<>0").Count();
                                    decimal outPM25 = 0;
                                    if (Convert.ToDecimal(dr["MonitorDays"]) != 0)
                                    {
                                        outPM25 = (decimal)(Convert.ToDecimal(dr["OutDays"]) / Convert.ToDecimal(dr["MonitorDays"]) * 100);
                                    }
                                    dr["OutRate"] = DecimalExtension.GetPollutantValue(outPM25, 1).ToString() + "%";
                                    if (yearpercentPM25 == null || yearpercentPM25 == "")
                                    {
                                        dr["YearPercent"] = "/";
                                    }
                                    else
                                    {
                                        dr["YearPercent"] = yearpercentPM25;
                                    }
                                    if (entityyear != null && yearpercentPM25 != null && yearpercentPM25 != "" && entityyear.Upper != null && entityyear.Upper != 0)
                                    {
                                        decimal dYearPerPM25 = (decimal)((Convert.ToDecimal(yearpercentPM25) / 1000 - entityyear.Upper) / entityyear.Upper);
                                        string strdYearPerPM25 = "";
                                        if (dYearPerPM25 < 0)
                                        {
                                            strdYearPerPM25 = "/";
                                        }
                                        else
                                        {
                                            strdYearPerPM25 = DecimalExtension.GetPollutantValue(dYearPerPM25, 2).ToString();
                                        }
                                        dr["YearPerOutRate"] = strdYearPerPM25;
                                    }
                                    break;
                                case IAQIType.CO_IAQI:
                                    entity = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a21005", EQITimeType.TwentyFour);
                                    entityyear = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a21005", EQITimeType.Year);
                                    string yearpercentCO = "";
                                    AreaData.RowFilter = "CO is not null and convert(CO, 'System.String')<>''  and RegionName='" + name + "'";
                                    List<decimal> listCO = AreaData.ToTable().AsEnumerable().Select(r => Convert.ToDecimal(r.Field<string>("CO"))).ToList<decimal>();
                                    listCO.Sort();
                                    decimal[] arrCO = listCO.ToArray();
                                    yearpercentCO = (DecimalExtension.GetPollutantValue(getpercentM("a21005", arrCO), 1)).ToString();
                                    dr["FactorName"] = "CO (mg/m<sup>3</sup>)";
                                    dr["OutDays"] = drExceeding[0]["CO_Over"];
                                    dr["MonitorDays"] = AreaData.ToTable().Select("RegionName='" + name + "' and DateTime>='" + dateStart +
                                        "' and DateTime<='" + dateEnd + "' and CO_IAQI is not null and CO_IAQI<>0").Count();
                                    decimal outCO = 0;
                                    if (Convert.ToDecimal(dr["MonitorDays"]) != 0)
                                    {
                                        outCO = (decimal)(Convert.ToDecimal(dr["OutDays"]) / Convert.ToDecimal(dr["MonitorDays"]) * 100);
                                    }
                                    dr["OutRate"] = DecimalExtension.GetPollutantValue(outCO, 1).ToString() + "%";
                                    if (yearpercentCO == null || yearpercentCO == "")
                                    {
                                        dr["YearPercent"] = "/";
                                    }
                                    else
                                    {
                                        dr["YearPercent"] = yearpercentCO;
                                    }
                                    if (entityyear != null && yearpercentCO != null && yearpercentCO != "" && entityyear.Upper != null && entityyear.Upper != 0)
                                    {
                                        decimal dYearPerCO = (decimal)((Convert.ToDecimal(yearpercentCO) - entityyear.Upper) / entityyear.Upper);
                                        string strdYearPerCO = "";
                                        if (dYearPerCO < 0)
                                        {
                                            strdYearPerCO = "/";
                                        }
                                        else
                                        {
                                            strdYearPerCO = DecimalExtension.GetPollutantValue(dYearPerCO, 2).ToString();
                                        }
                                        dr["YearPerOutRate"] = strdYearPerCO;
                                    }
                                    break;
                                case IAQIType.Max8HourO3_IAQI:
                                    entity = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a05024", EQITimeType.Eight);
                                    string yearpercentO3 = "";
                                    AreaData.RowFilter = "Max8HourO3 is not null and convert(Max8HourO3, 'System.String')<>''  and RegionName='" + name + "'";
                                    List<decimal> listO3 = AreaData.ToTable().AsEnumerable().Select(r => Convert.ToDecimal(r.Field<string>("Max8HourO3"))).ToList<decimal>();
                                    listO3.Sort();
                                    decimal[] arrO3 = listO3.ToArray();
                                    yearpercentO3 = (DecimalExtension.GetPollutantValue(getpercentM("a05024", arrO3), 3) * 1000).ToString("G0");
                                    dr["FactorName"] = "O3-8 (μg/m<sup>3</sup>)";
                                    dr["OutDays"] = drExceeding[0]["Max8HourO3_Over"];
                                    dr["MonitorDays"] = AreaData.ToTable().Select("RegionName='" + name + "' and DateTime>='" + dateStart +
                                        "' and DateTime<='" + dateEnd + "' and Max8HourO3_IAQI is not null and Max8HourO3_IAQI<>0").Count();
                                    decimal outMax8HourO3 = 0;
                                    if (Convert.ToDecimal(dr["MonitorDays"]) != 0)
                                    {
                                        outMax8HourO3 = (decimal)(Convert.ToDecimal(dr["OutDays"]) / Convert.ToDecimal(dr["MonitorDays"]) * 100);
                                    }
                                    dr["OutRate"] = DecimalExtension.GetPollutantValue(outMax8HourO3, 1).ToString() + "%";
                                    if (yearpercentO3 == null || yearpercentO3 == "")
                                    {
                                        dr["YearPercent"] = "/";
                                    }
                                    else
                                    {
                                        dr["YearPercent"] = yearpercentO3;
                                    }
                                    if (entity != null && yearpercentO3 != null && yearpercentO3 != "" && entity.Upper != null && entity.Upper != 0)
                                    {
                                        decimal dYearPerO3 = (decimal)((Convert.ToDecimal(yearpercentO3) / 1000 - entity.Upper) / entity.Upper);
                                        string strdYearPerO3 = "";
                                        if (dYearPerO3 < 0)
                                        {
                                            strdYearPerO3 = "/";
                                        }
                                        else
                                        {
                                            strdYearPerO3 = DecimalExtension.GetPollutantValue(dYearPerO3, 2).ToString();
                                        }
                                        dr["YearPerOutRate"] = strdYearPerO3;
                                    }
                                    break;
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                }
                return dt.DefaultView;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取百分位数浓度
        /// </summary>
        /// <param name="factor"></param>
        /// <param name="factorValue"></param>
        /// <returns></returns>
        public decimal getpercentM(string factor, decimal[] factorArr)
        {
            int p = 0;  //百分位数
            if (factor == "a21026" || factor == "a21004")
            {
                p = 98;
            }
            if (factor == "a34002" || factor == "a34004" || factor == "a21005")
            {
                p = 95;
            }
            if (factor == "a05024")
            {
                p = 90;
            }
            decimal k = 1 + Convert.ToDecimal((factorArr.Length - 1) * p) / 100;   //系数k 
            int s = (int)Math.Floor(k);                         //系数k的整数部分
            if (factorArr.Length > 1)
            {
                decimal m = factorArr[s - 1] + (factorArr[s] - factorArr[s - 1]) * (k - s); //根据公式计算
                return m;
            }
            else
            {
                decimal m = factorArr[s - 1]; //根据公式计算
                return m;
            }

        }

        ///// <summary>
        ///// 全市年数据统计
        ///// </summary>
        ///// <param name="regionGuids">区域Guid</param>
        ///// <param name="dateStart">开始时间</param>
        ///// <param name="dateEnd">结束时间</param>
        ///// <param name="orderBy">排序</param>
        ///// <returns>
        ///// RegionName：区域
        ///// FactorName：因子
        ///// DayMinValue：最小值
        ///// DayMaxValue：最大值
        ///// OutDays：超标天数
        ///// MonitorDays：监控天数
        ///// OutRate：超标率
        ///// OutBiggestFactor：最大超标倍数
        ///// YearAverage：年均值
        ///// </returns>
        //public DataView GetAllYearDataNew(string[] regionGuids, string[] facor, DateTime dateStart, DateTime dateEnd)
        //{
        //    try
        //    {
        //        pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
        //        AirPollutantService m_AirPollutantService = new AirPollutantService();
        //        int PM25Unit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a34004").PollutantDecimalNum);
        //        int PM10Unit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a34002").PollutantDecimalNum);
        //        int NO2Unit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a21004").PollutantDecimalNum);
        //        int SO2Unit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a21026").PollutantDecimalNum);
        //        int COUnit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a21005").PollutantDecimalNum);
        //        int Max8HourO3Unit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a05024").PollutantDecimalNum);
        //        int recordTotal = 0;
        //        DataView AreaData = GetAreaDataPager(regionGuids, dateStart, dateEnd, 99999, 0, out recordTotal);
        //        EQIConcentrationService EQIConcentration = new EQIConcentrationService();
        //        EQIConcentrationLimitEntity entity = null;
        //        EQIConcentrationLimitEntity entityyear = null;
        //        DataView MinValues = GetRegionsMinValue(regionGuids, dateStart, dateEnd);
        //        DataView MaxValues = GetRegionsMaxValue(regionGuids, dateStart, dateEnd);
        //        DataView ExceedingDatas = GetRegionsExceedingData(regionGuids, dateStart, dateEnd);
        //        DataView AvgValues = GetRegionsAvgValue(regionGuids, dateStart, dateEnd);
        //        DictionaryService dictionary = new DictionaryService();
        //        DataTable dt = new DataTable();
        //        dt.Columns.Add("RegionName", typeof(string));
        //        dt.Columns.Add("FactorName", typeof(string));
        //        dt.Columns.Add("DayMinValue", typeof(string));
        //        dt.Columns.Add("DayMaxValue", typeof(string));
        //        dt.Columns.Add("DayAvgValue", typeof(string));
        //        dt.Columns.Add("OutDays", typeof(string));
        //        dt.Columns.Add("MonitorDays", typeof(string));
        //        dt.Columns.Add("OutRate", typeof(string));
        //        dt.Columns.Add("OutBiggestFactor", typeof(string));
        //        dt.Columns.Add("OutDate", typeof(string));
        //        dt.Columns.Add("DataRange", typeof(string));
        //        //dt.Columns.Add("YearAverage", typeof(string));
        //        //dt.Columns.Add("YearOutRate", typeof(string));
        //        dt.Columns.Add("YearPercent", typeof(string));
        //        dt.Columns.Add("YearPerOutRate", typeof(string));
        //        dt.Columns.Add("MedianUpper", typeof(string));
        //        dt.Columns.Add("MedianLower", typeof(string));
        //        dt.Columns.Add("MedianValueu", typeof(string));
        //        dt.Columns.Add("lower", typeof(string));
        //        dt.Columns.Add("upper", typeof(string));
        //        List<IAQIType> AQITypes = new List<IAQIType>();
        //        foreach (string item in facor)
        //        {
        //            if (item == "SO2")
        //                AQITypes.Add(IAQIType.SO2_IAQI);
        //            if (item == "NO2")
        //                AQITypes.Add(IAQIType.NO2_IAQI);
        //            if (item == "PM10")
        //                AQITypes.Add(IAQIType.PM10_IAQI);
        //            if (item == "PM25")
        //                AQITypes.Add(IAQIType.PM25_IAQI);
        //            if (item == "CO")
        //                AQITypes.Add(IAQIType.CO_IAQI);
        //            if (item == "O3_8h")
        //                AQITypes.Add(IAQIType.Max8HourO3_IAQI);
        //        }
        //        //AQITypes.Add(IAQIType.SO2_IAQI);
        //        //AQITypes.Add(IAQIType.NO2_IAQI);
        //        //AQITypes.Add(IAQIType.PM10_IAQI);
        //        //AQITypes.Add(IAQIType.PM25_IAQI);
        //        //AQITypes.Add(IAQIType.CO_IAQI);
        //        //AQITypes.Add(IAQIType.Max8HourO3_IAQI);

        //        for (int i = 0; i < regionGuids.Length; i++)
        //        {
        //            string RegionName = dictionary.GetCodeDictionaryTextByValue(regionGuids[i]);
        //            DataRow[] drMin = MinValues.Table.Select("MonitoringRegionUid='" + regionGuids[i] + "'");
        //            DataRow[] drMax = MaxValues.Table.Select("MonitoringRegionUid='" + regionGuids[i] + "'");
        //            DataRow[] drExceeding = ExceedingDatas.Table.Select("MonitoringRegionUid='" + regionGuids[i] + "'");
        //            DataRow[] drAvg = AvgValues.Table.Select("MonitoringRegionUid='" + regionGuids[i] + "'");

        //            if (drMin.Length > 0 && drMax.Length > 0 && drExceeding.Length > 0 && drAvg.Length > 0)
        //            {
        //                for (int j = 0; j < AQITypes.Count; j++)
        //                {
        //                    DataRow dr = dt.NewRow();
        //                    dr["RegionName"] = RegionName;
        //                    switch (AQITypes[j])
        //                    {
        //                        case IAQIType.SO2_IAQI:
        //                            entity = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a21026", EQITimeType.TwentyFour);
        //                            entityyear = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a21026", EQITimeType.Year);
        //                            string yearpercentSO2 = "";
        //                            yearpercentSO2 = pointDayAQI.getpercent(regionGuids[i], dateStart, dateEnd, "a21026");
        //                            dr["FactorName"] = "SO2";
        //                            decimal MinValue = (DecimalExtension.GetPollutantValue(decimal.TryParse(drMin[0]["SO2"].ToString(), out MinValue) ? MinValue : 0, SO2Unit));
        //                            dr["DayMinValue"] = MinValue != 0 ? MinValue.ToString() : "";
        //                            decimal MaxValue = (DecimalExtension.GetPollutantValue(decimal.TryParse(drMax[0]["SO2"].ToString(), out MaxValue) ? MaxValue : 0, SO2Unit));
        //                            dr["DayMaxValue"] = MaxValue != 0 ? MaxValue.ToString() : "";
        //                            decimal AvgValue = (DecimalExtension.GetPollutantValue(decimal.TryParse(drAvg[0]["SO2"].ToString(), out AvgValue) ? AvgValue : 0, SO2Unit));
        //                            dr["DayAvgValue"] = AvgValue != 0 ? AvgValue.ToString() : "";
        //                            dr["DataRange"] = MinValue.ToString() + "~" + MaxValue.ToString();
        //                            dr["OutDays"] = drExceeding[0]["SO2_Over"];
        //                            dr["MonitorDays"] = AreaData.ToTable().Select("MonitoringRegionUid='" + regionGuids[i] + "' and ReportDateTime>='" + dateStart +
        //                                "' and ReportDateTime<='" + dateEnd + "' and SO2_IAQI is not null and SO2_IAQI<>0").Count();
        //                            decimal outSO2 = 0;
        //                            if (Convert.ToDecimal(dr["MonitorDays"]) != 0)
        //                            {
        //                                outSO2 = (decimal)(Convert.ToDecimal(dr["OutDays"]) / Convert.ToDecimal(dr["MonitorDays"]) * 100);
        //                            }
        //                            dr["OutRate"] = DecimalExtension.GetPollutantValue(outSO2, 1).ToString() + "%";
        //                            decimal dSO2 = (decimal)((Convert.ToDecimal(drMax[0]["SO2"]) - entity.Upper) / entity.Upper);
        //                            string strdSO2 = "";
        //                            if (dSO2 < 0)
        //                            {
        //                                strdSO2 = "/";
        //                                dr["OutDate"] = "/";
        //                            }
        //                            else
        //                            {
        //                                strdSO2 = DecimalExtension.GetPollutantValue(dSO2, 2).ToString();
        //                                DataRow[] SO2 = AreaData.ToTable().Select("MonitoringRegionUid='" + regionGuids[i] + "' and ReportDateTime>='" + dateStart +
        //                                "' and ReportDateTime<='" + dateEnd + "' and SO2 like'" + MaxValue + "%'");
        //                                if (SO2.Length > 0)
        //                                    if (SO2[0]["ReportDateTime"].IsNotNullOrDBNull())
        //                                        dr["OutDate"] = string.Format("{0:yyyy-MM-dd}", SO2[0]["ReportDateTime"]);
        //                            }
        //                            dr["OutBiggestFactor"] = strdSO2;
        //                            //decimal YearAverage = (DecimalExtension.GetPollutantValue(decimal.TryParse(drAvg[0]["SO2"].ToString(), out YearAverage) ? YearAverage : 0, SO2Unit));
        //                            //dr["YearAverage"] = YearAverage != 0 ? YearAverage.ToString() : "";
        //                            //if (entityyear != null && drAvg[0]["SO2"] != DBNull.Value && entityyear.Upper != null && entityyear.Upper != 0)
        //                            //{
        //                            //    decimal dYearSO2 = (decimal)((Convert.ToDecimal(drAvg[0]["SO2"]) - entityyear.Upper) / entityyear.Upper);
        //                            //    string strdYearSO2 = "";
        //                            //    if (dYearSO2 < 0)
        //                            //    {
        //                            //        strdYearSO2 = "/";
        //                            //    }
        //                            //    else
        //                            //    {
        //                            //        strdYearSO2 = DecimalExtension.GetPollutantValue(dYearSO2, 2).ToString();
        //                            //    }
        //                            //    dr["YearOutRate"] = strdYearSO2;
        //                            //}
        //                            if (yearpercentSO2 == null || yearpercentSO2 == "")
        //                            {
        //                                dr["YearPercent"] = "/";
        //                            }
        //                            else
        //                            {
        //                                dr["YearPercent"] = yearpercentSO2;
        //                            }
        //                            if (entityyear != null && yearpercentSO2 != null && yearpercentSO2 != "" && entityyear.Upper != null && entityyear.Upper != 0)
        //                            {
        //                                decimal dYearPerSO2 = (decimal)((Convert.ToDecimal(yearpercentSO2) - entityyear.Upper) / entityyear.Upper);
        //                                string strdYearPerSO2 = "";
        //                                if (dYearPerSO2 < 0)
        //                                {
        //                                    strdYearPerSO2 = "/";
        //                                }
        //                                else
        //                                {
        //                                    strdYearPerSO2 = DecimalExtension.GetPollutantValue(dYearPerSO2, 2).ToString();
        //                                }
        //                                dr["YearPerOutRate"] = strdYearPerSO2;
        //                            }

        //                            AreaData.RowFilter = "SO2 is not null and SO2<>'' and MonitoringRegionUid='" + regionGuids[i] + "'";
        //                            List<string> strSO2 = AreaData.ToTable().AsEnumerable().Select(r => r.Field<string>("SO2")).ToList();
        //                            List<decimal> decSO2 = new List<decimal>(strSO2.Select(x => decimal.Parse(x)));
        //                            Dictionary<string, decimal> ListMedian = Median(decSO2);
        //                            if (ListMedian.Keys.Contains("median"))
        //                            {
        //                                dr["MedianValueu"] = ListMedian["median"];
        //                            }
        //                            if (ListMedian.Keys.Contains("median1"))
        //                            {
        //                                dr["MedianLower"] = ListMedian["median1"];
        //                            }
        //                            if (ListMedian.Keys.Contains("median3"))
        //                            {
        //                                dr["MedianUpper"] = ListMedian["median3"];
        //                            } if (ListMedian.Keys.Contains("lower"))
        //                            {
        //                                dr["lower"] = ListMedian["lower"];
        //                            } if (ListMedian.Keys.Contains("upper"))
        //                            {
        //                                dr["upper"] = ListMedian["upper"];
        //                            }
        //                            AreaData.RowFilter = string.Empty;
        //                            break;
        //                        case IAQIType.NO2_IAQI:
        //                            entity = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a21004", EQITimeType.TwentyFour);
        //                            entityyear = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a21004", EQITimeType.Year);
        //                            string yearpercentNO2 = "";
        //                            yearpercentNO2 = pointDayAQI.getpercent(regionGuids[i], dateStart, dateEnd, "a21004");
        //                            dr["FactorName"] = "NO2";
        //                            decimal MinValueNO2 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drMin[0]["NO2"].ToString(), out MinValueNO2) ? MinValueNO2 : 0, NO2Unit));
        //                            dr["DayMinValue"] = MinValueNO2 != 0 ? MinValueNO2.ToString() : "";
        //                            decimal MaxValueNO2 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drMax[0]["NO2"].ToString(), out MaxValueNO2) ? MaxValueNO2 : 0, NO2Unit));
        //                            dr["DayMaxValue"] = MaxValueNO2 != 0 ? MaxValueNO2.ToString() : "";
        //                            decimal AvgValueNO2 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drAvg[0]["NO2"].ToString(), out AvgValueNO2) ? AvgValueNO2 : 0, NO2Unit));
        //                            dr["DayAvgValue"] = AvgValueNO2 != 0 ? AvgValueNO2.ToString() : "";
        //                            dr["DataRange"] = MinValueNO2.ToString() + "~" + MaxValueNO2.ToString();
        //                            dr["OutDays"] = drExceeding[0]["NO2_Over"];
        //                            dr["MonitorDays"] = AreaData.ToTable().Select("MonitoringRegionUid='" + regionGuids[i] + "' and ReportDateTime>='" + dateStart +
        //                                "' and ReportDateTime<='" + dateEnd + "' and NO2_IAQI is not null and NO2_IAQI<>0").Count();
        //                            decimal outNO2 = 0;
        //                            if (Convert.ToDecimal(dr["MonitorDays"]) != 0)
        //                            {
        //                                outNO2 = (decimal)(Convert.ToDecimal(dr["OutDays"]) / Convert.ToDecimal(dr["MonitorDays"]) * 100);
        //                            }
        //                            dr["OutRate"] = DecimalExtension.GetPollutantValue(outNO2, 1).ToString() + "%";
        //                            decimal dNO2 = (decimal)((Convert.ToDecimal(drMax[0]["NO2"]) - entity.Upper) / entity.Upper);
        //                            string strdNO2 = "";
        //                            if (dNO2 < 0)
        //                            {
        //                                strdNO2 = "/";
        //                                dr["OutDate"] = "/";
        //                            }
        //                            else
        //                            {
        //                                strdNO2 = DecimalExtension.GetPollutantValue(dNO2, 2).ToString();
        //                                DataRow[] NO2 = AreaData.ToTable().Select("MonitoringRegionUid='" + regionGuids[i] + "' and ReportDateTime>='" + dateStart +
        //                           "' and ReportDateTime<='" + dateEnd + "' and NO2 like'" + MaxValueNO2 + "%'");
        //                                if (NO2.Length > 0)
        //                                    if (NO2[0]["ReportDateTime"].IsNotNullOrDBNull())
        //                                        dr["OutDate"] = string.Format("{0:yyyy-MM-dd}", NO2[0]["ReportDateTime"]);
        //                            }
        //                            dr["OutBiggestFactor"] = strdNO2;
        //                            //decimal YearAverageNO2 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drAvg[0]["NO2"].ToString(), out YearAverageNO2) ? YearAverageNO2 : 0, NO2Unit));
        //                            //dr["YearAverage"] = YearAverageNO2 != 0 ? YearAverageNO2.ToString() : "";
        //                            //if (entityyear != null && drAvg[0]["NO2"] != DBNull.Value && entityyear.Upper != null && entityyear.Upper != 0)
        //                            //{
        //                            //    decimal dYearNO2 = (decimal)((Convert.ToDecimal(drAvg[0]["NO2"]) - entityyear.Upper) / entityyear.Upper);
        //                            //    string strdYearNO2 = "";
        //                            //    if (dYearNO2 < 0)
        //                            //    {
        //                            //        strdYearNO2 = "/";
        //                            //    }
        //                            //    else
        //                            //    {
        //                            //        strdYearNO2 = DecimalExtension.GetPollutantValue(dYearNO2, 2).ToString();
        //                            //    }
        //                            //    dr["YearOutRate"] = strdYearNO2;
        //                            //}
        //                            if (yearpercentNO2 == null || yearpercentNO2 == "")
        //                            {
        //                                dr["YearPercent"] = "/";
        //                            }
        //                            else
        //                            {
        //                                dr["YearPercent"] = yearpercentNO2;
        //                            }
        //                            if (entityyear != null && yearpercentNO2 != null && yearpercentNO2 != "" && entityyear.Upper != null && entityyear.Upper != 0)
        //                            {
        //                                decimal dYearPerNO2 = (decimal)((Convert.ToDecimal(yearpercentNO2) - entityyear.Upper) / entityyear.Upper);
        //                                string strdYearPerNO2 = "";
        //                                if (dYearPerNO2 < 0)
        //                                {
        //                                    strdYearPerNO2 = "/";
        //                                }
        //                                else
        //                                {
        //                                    strdYearPerNO2 = DecimalExtension.GetPollutantValue(dYearPerNO2, 2).ToString();
        //                                }
        //                                dr["YearPerOutRate"] = strdYearPerNO2;
        //                            }
        //                            AreaData.RowFilter = "NO2 is not null and NO2<>'' and MonitoringRegionUid='" + regionGuids[i] + "'";
        //                            List<string> strNO2 = AreaData.ToTable().AsEnumerable().Select(r => r.Field<string>("NO2")).ToList();
        //                            List<decimal> decNO2 = new List<decimal>(strNO2.Select(x => decimal.Parse(x)));
        //                            Dictionary<string, decimal> ListMedianNO2 = Median(decNO2);
        //                            if (ListMedianNO2.Keys.Contains("median"))
        //                            {
        //                                dr["MedianValueu"] = ListMedianNO2["median"];
        //                            }
        //                            if (ListMedianNO2.Keys.Contains("median1"))
        //                            {
        //                                dr["MedianLower"] = ListMedianNO2["median1"];
        //                            }
        //                            if (ListMedianNO2.Keys.Contains("median3"))
        //                            {
        //                                dr["MedianUpper"] = ListMedianNO2["median3"];
        //                            } if (ListMedianNO2.Keys.Contains("lower"))
        //                            {
        //                                dr["lower"] = ListMedianNO2["lower"];
        //                            } if (ListMedianNO2.Keys.Contains("upper"))
        //                            {
        //                                dr["upper"] = ListMedianNO2["upper"];
        //                            }
        //                            AreaData.RowFilter = string.Empty;
        //                            break;
        //                        case IAQIType.PM10_IAQI:
        //                            entity = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a34002", EQITimeType.TwentyFour);
        //                            entityyear = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a34002", EQITimeType.Year);
        //                            string yearpercentPM10 = "";
        //                            yearpercentPM10 = pointDayAQI.getpercent(regionGuids[i], dateStart, dateEnd, "a34002");
        //                            dr["FactorName"] = "PM10";
        //                            decimal MinValuePM10 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drMin[0]["PM10"].ToString(), out MinValuePM10) ? MinValuePM10 : 0, PM10Unit));
        //                            dr["DayMinValue"] = MinValuePM10 != 0 ? MinValuePM10.ToString() : "";
        //                            decimal MaxValuePM10 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drMax[0]["PM10"].ToString(), out MaxValuePM10) ? MaxValuePM10 : 0, PM10Unit));
        //                            dr["DayMaxValue"] = MaxValuePM10 != 0 ? MaxValuePM10.ToString() : "";
        //                            decimal AvgValuePM10 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drAvg[0]["PM10"].ToString(), out AvgValuePM10) ? AvgValuePM10 : 0, PM10Unit));
        //                            dr["DayAvgValue"] = AvgValuePM10 != 0 ? AvgValuePM10.ToString() : "";
        //                            dr["DataRange"] = MinValuePM10.ToString() + "~" + MaxValuePM10.ToString();
        //                            dr["OutDays"] = drExceeding[0]["PM10_Over"];
        //                            dr["MonitorDays"] = AreaData.ToTable().Select("MonitoringRegionUid='" + regionGuids[i] + "' and ReportDateTime>='" + dateStart +
        //                                "' and ReportDateTime<='" + dateEnd + "' and PM10_IAQI is not null and PM10_IAQI<>0").Count();
        //                            decimal outPM10 = 0;
        //                            if (Convert.ToDecimal(dr["MonitorDays"]) != 0)
        //                            {
        //                                outPM10 = (decimal)(Convert.ToDecimal(dr["OutDays"]) / Convert.ToDecimal(dr["MonitorDays"]) * 100);
        //                            }
        //                            dr["OutRate"] = DecimalExtension.GetPollutantValue(outPM10, 1).ToString() + "%";
        //                            decimal dPM10 = (decimal)(Convert.ToDecimal(drMax[0]["PM10"]) - entity.Upper);
        //                            string strdPM10 = "";
        //                            if (dPM10 < 0)
        //                            {
        //                                strdPM10 = "/";
        //                                dr["OutDate"] = "/";
        //                            }
        //                            else
        //                            {
        //                                strdPM10 = DecimalExtension.GetPollutantValue(dPM10, 2).ToString();
        //                                DataRow[] PM10 = AreaData.ToTable().Select("MonitoringRegionUid='" + regionGuids[i] + "' and ReportDateTime>='" + dateStart +
        //                                                 "' and ReportDateTime<='" + dateEnd + "' and PM10 like'" + MaxValuePM10 + "%'");
        //                                if (PM10.Length > 0)
        //                                    if (PM10[0]["ReportDateTime"].IsNotNullOrDBNull())
        //                                        dr["OutDate"] = string.Format("{0:yyyy-MM-dd}", PM10[0]["ReportDateTime"]);
        //                            }
        //                            dr["OutBiggestFactor"] = strdPM10;
        //                            //decimal YearAveragePM10 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drAvg[0]["PM10"].ToString(), out YearAveragePM10) ? YearAveragePM10 : 0, PM10Unit));
        //                            //dr["YearAverage"] = YearAveragePM10 != 0 ? YearAveragePM10.ToString() : "";
        //                            //if (entityyear != null && drAvg[0]["PM10"] != DBNull.Value && entityyear.Upper != null && entityyear.Upper != 0)
        //                            //{
        //                            //    decimal dYearPM10 = (decimal)((Convert.ToDecimal(drAvg[0]["PM10"]) - entityyear.Upper) / entityyear.Upper);
        //                            //    string strdYearPM10 = "";
        //                            //    if (dYearPM10 < 0)
        //                            //    {
        //                            //        strdYearPM10 = "/";
        //                            //    }
        //                            //    else
        //                            //    {
        //                            //        strdYearPM10 = DecimalExtension.GetPollutantValue(dYearPM10, 2).ToString();
        //                            //    }
        //                            //    dr["YearOutRate"] = strdYearPM10;
        //                            //}
        //                            if (yearpercentPM10 == null || yearpercentPM10 == "")
        //                            {
        //                                dr["YearPercent"] = "/";
        //                            }
        //                            else
        //                            {
        //                                dr["YearPercent"] = yearpercentPM10;
        //                            }
        //                            if (entityyear != null && yearpercentPM10 != null && yearpercentPM10 != "" && entityyear.Upper != null && entityyear.Upper != 0)
        //                            {
        //                                decimal dYearPerPM10 = (decimal)((Convert.ToDecimal(yearpercentPM10) - entityyear.Upper) / entityyear.Upper);
        //                                string strdYearPerPM10 = "";
        //                                if (dYearPerPM10 < 0)
        //                                {
        //                                    strdYearPerPM10 = "/";
        //                                }
        //                                else
        //                                {
        //                                    strdYearPerPM10 = DecimalExtension.GetPollutantValue(dYearPerPM10, 2).ToString();
        //                                }
        //                                dr["YearPerOutRate"] = strdYearPerPM10;
        //                            }
        //                            AreaData.RowFilter = "PM10 is not null and PM10<>''  and MonitoringRegionUid='" + regionGuids[i] + "'";
        //                            List<string> strPM10 = AreaData.ToTable().AsEnumerable().Select(r => r.Field<string>("PM10")).ToList();
        //                            List<decimal> decPM10 = new List<decimal>(strPM10.Select(x => decimal.Parse(x)));
        //                            Dictionary<string, decimal> ListMedianPM10 = Median(decPM10);
        //                            if (ListMedianPM10.Keys.Contains("median"))
        //                            {
        //                                dr["MedianValueu"] = ListMedianPM10["median"];
        //                            }
        //                            if (ListMedianPM10.Keys.Contains("median1"))
        //                            {
        //                                dr["MedianLower"] = ListMedianPM10["median1"];
        //                            }
        //                            if (ListMedianPM10.Keys.Contains("median3"))
        //                            {
        //                                dr["MedianUpper"] = ListMedianPM10["median3"];
        //                            } if (ListMedianPM10.Keys.Contains("lower"))
        //                            {
        //                                dr["lower"] = ListMedianPM10["lower"];
        //                            } if (ListMedianPM10.Keys.Contains("upper"))
        //                            {
        //                                dr["upper"] = ListMedianPM10["upper"];
        //                            }
        //                            AreaData.RowFilter = string.Empty;
        //                            break;
        //                        case IAQIType.PM25_IAQI:
        //                            entity = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a34004", EQITimeType.TwentyFour);
        //                            entityyear = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a34004", EQITimeType.Year);
        //                            string yearpercentPM25 = "";
        //                            yearpercentPM25 = pointDayAQI.getpercent(regionGuids[i], dateStart, dateEnd, "a34004");
        //                            dr["FactorName"] = "PM2.5";
        //                            decimal MinValuePM25 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drMin[0]["PM25"].ToString(), out MinValuePM25) ? MinValuePM25 : 0, PM25Unit));
        //                            dr["DayMinValue"] = MinValuePM25 != 0 ? MinValuePM25.ToString() : "";
        //                            decimal MaxValuePM25 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drMax[0]["PM25"].ToString(), out MaxValuePM25) ? MaxValuePM25 : 0, PM25Unit));
        //                            dr["DayMaxValue"] = MaxValuePM25 != 0 ? MaxValuePM25.ToString() : "";
        //                            decimal AvgValuePM25 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drAvg[0]["PM25"].ToString(), out AvgValuePM25) ? AvgValuePM25 : 0, PM25Unit));
        //                            dr["DayAvgValue"] = AvgValuePM25 != 0 ? AvgValuePM25.ToString() : "";
        //                            dr["DataRange"] = MinValuePM25.ToString() + "~" + MaxValuePM25.ToString();
        //                            dr["OutDays"] = drExceeding[0]["PM25_Over"];
        //                            dr["MonitorDays"] = AreaData.ToTable().Select("MonitoringRegionUid='" + regionGuids[i] + "' and ReportDateTime>='" + dateStart +
        //                                "' and ReportDateTime<='" + dateEnd + "' and PM25_IAQI is not null and PM25_IAQI<>0").Count();
        //                            decimal outPM25 = 0;
        //                            if (Convert.ToDecimal(dr["MonitorDays"]) != 0)
        //                            {
        //                                outPM25 = (decimal)(Convert.ToDecimal(dr["OutDays"]) / Convert.ToDecimal(dr["MonitorDays"]) * 100);
        //                            }
        //                            dr["OutRate"] = DecimalExtension.GetPollutantValue(outPM25, 1).ToString() + "%";
        //                            //dr["OutBiggestFactor"] = ((Convert.ToDecimal(drMax[0]["PM25"]) - entity.Upper) / entity.Upper).ToString();
        //                            decimal dPM25 = (decimal)((Convert.ToDecimal(drMax[0]["PM25"]) - entity.Upper) / entity.Upper);
        //                            string strdPM25 = "";
        //                            if (dPM25 < 0)
        //                            {
        //                                strdPM25 = "/";
        //                                dr["OutDate"] = "/";
        //                            }
        //                            else
        //                            {
        //                                strdPM25 = DecimalExtension.GetPollutantValue(dPM25, 2).ToString();
        //                                DataRow[] PM25 = AreaData.ToTable().Select("MonitoringRegionUid='" + regionGuids[i] + "' and ReportDateTime>='" + dateStart +
        //                                                 "' and ReportDateTime<='" + dateEnd + "' and PM25 like'" + MaxValuePM25 + "%'");
        //                                if (PM25.Length > 0)
        //                                    if (PM25[0]["ReportDateTime"].IsNotNullOrDBNull())
        //                                        dr["OutDate"] = string.Format("{0:yyyy-MM-dd}", PM25[0]["ReportDateTime"]);
        //                            }
        //                            dr["OutBiggestFactor"] = strdPM25;
        //                            //decimal YearAveragePM25 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drAvg[0]["PM25"].ToString(), out YearAveragePM25) ? YearAveragePM25 : 0, PM25Unit));
        //                            //dr["YearAverage"] = YearAveragePM25 != 0 ? YearAveragePM25.ToString() : "";
        //                            //if (entityyear != null && drAvg[0]["PM25"] != DBNull.Value && entityyear.Upper != null && entityyear.Upper != 0)
        //                            //{
        //                            //    decimal dYearPM25 = (decimal)((Convert.ToDecimal(drAvg[0]["PM25"]) - entityyear.Upper) / entityyear.Upper);
        //                            //    string strdYearPM25 = "";
        //                            //    if (dYearPM25 < 0)
        //                            //    {
        //                            //        strdYearPM25 = "/";
        //                            //    }
        //                            //    else
        //                            //    {
        //                            //        strdYearPM25 = DecimalExtension.GetPollutantValue(dYearPM25, 2).ToString();
        //                            //    }
        //                            //    dr["YearOutRate"] = strdYearPM25;
        //                            //}
        //                            if (yearpercentPM25 == null || yearpercentPM25 == "")
        //                            {
        //                                dr["YearPercent"] = "/";
        //                            }
        //                            else
        //                            {
        //                                dr["YearPercent"] = yearpercentPM25;
        //                            }
        //                            if (entityyear != null && yearpercentPM25 != null && yearpercentPM25 != "" && entityyear.Upper != null && entityyear.Upper != 0)
        //                            {
        //                                decimal dYearPerPM25 = (decimal)((Convert.ToDecimal(yearpercentPM25) - entityyear.Upper) / entityyear.Upper);
        //                                string strdYearPerPM25 = "";
        //                                if (dYearPerPM25 < 0)
        //                                {
        //                                    strdYearPerPM25 = "/";
        //                                }
        //                                else
        //                                {
        //                                    strdYearPerPM25 = DecimalExtension.GetPollutantValue(dYearPerPM25, 2).ToString();
        //                                }
        //                                dr["YearPerOutRate"] = strdYearPerPM25;
        //                            }
        //                            AreaData.RowFilter = "PM25 is not null and PM25<>''  and MonitoringRegionUid='" + regionGuids[i] + "'";
        //                            List<string> strPM25 = AreaData.ToTable().AsEnumerable().Select(r => r.Field<string>("PM25")).ToList();
        //                            List<decimal> decPM25 = new List<decimal>(strPM25.Select(x => decimal.Parse(x)));
        //                            Dictionary<string, decimal> ListMedianPM25 = Median(decPM25);
        //                            if (ListMedianPM25.Keys.Contains("median"))
        //                            {
        //                                dr["MedianValueu"] = ListMedianPM25["median"];
        //                            }
        //                            if (ListMedianPM25.Keys.Contains("median1"))
        //                            {
        //                                dr["MedianLower"] = ListMedianPM25["median1"];
        //                            }
        //                            if (ListMedianPM25.Keys.Contains("median3"))
        //                            {
        //                                dr["MedianUpper"] = ListMedianPM25["median3"];
        //                            } if (ListMedianPM25.Keys.Contains("lower"))
        //                            {
        //                                dr["lower"] = ListMedianPM25["lower"];
        //                            } if (ListMedianPM25.Keys.Contains("upper"))
        //                            {
        //                                dr["upper"] = ListMedianPM25["upper"];
        //                            }
        //                            AreaData.RowFilter = string.Empty;
        //                            break;
        //                        case IAQIType.CO_IAQI:
        //                            entity = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a21005", EQITimeType.TwentyFour);
        //                            entityyear = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a21005", EQITimeType.Year);
        //                            string yearpercentCO = "";
        //                            yearpercentCO = pointDayAQI.getpercent(regionGuids[i], dateStart, dateEnd, "a21005");
        //                            dr["FactorName"] = "CO";
        //                            decimal MinValueCO = (DecimalExtension.GetPollutantValue(decimal.TryParse(drMin[0]["CO"].ToString(), out MinValueCO) ? MinValueCO : 0, COUnit));
        //                            dr["DayMinValue"] = MinValueCO != 0 ? MinValueCO.ToString() : "";
        //                            decimal MaxValueCO = (DecimalExtension.GetPollutantValue(decimal.TryParse(drMax[0]["CO"].ToString(), out MaxValueCO) ? MaxValueCO : 0, COUnit));
        //                            dr["DayMaxValue"] = MaxValueCO != 0 ? MaxValueCO.ToString() : "";
        //                            decimal AvgValueCO = (DecimalExtension.GetPollutantValue(decimal.TryParse(drAvg[0]["CO"].ToString(), out AvgValueCO) ? AvgValueCO : 0, COUnit));
        //                            dr["DayAvgValue"] = AvgValueCO != 0 ? AvgValueCO.ToString() : "";
        //                            dr["DataRange"] = MinValueCO.ToString() + "~" + MaxValueCO.ToString();
        //                            dr["OutDays"] = drExceeding[0]["CO_Over"];
        //                            dr["MonitorDays"] = AreaData.ToTable().Select("MonitoringRegionUid='" + regionGuids[i] + "' and ReportDateTime>='" + dateStart +
        //                                "' and ReportDateTime<='" + dateEnd + "' and CO_IAQI is not null and CO_IAQI<>0").Count();
        //                            decimal outCO = 0;
        //                            if (Convert.ToDecimal(dr["MonitorDays"]) != 0)
        //                            {
        //                                outCO = (decimal)(Convert.ToDecimal(dr["OutDays"]) / Convert.ToDecimal(dr["MonitorDays"]) * 100);
        //                            }
        //                            dr["OutRate"] = DecimalExtension.GetPollutantValue(outCO, 1).ToString() + "%";

        //                            //dr["OutBiggestFactor"] = ((Convert.ToDecimal(drMax[0]["CO"]) - entity.Upper) / entity.Upper).ToString();
        //                            decimal dCO = (decimal)((Convert.ToDecimal(drMax[0]["CO"]) - entity.Upper) / entity.Upper);
        //                            string strdCO = "";
        //                            if (dCO < 0)
        //                            {
        //                                strdCO = "/";
        //                                dr["OutDate"] = "/";
        //                            }
        //                            else
        //                            {
        //                                strdCO = DecimalExtension.GetPollutantValue(dCO, 2).ToString();
        //                                DataRow[] CO = AreaData.ToTable().Select("MonitoringRegionUid='" + regionGuids[i] + "' and ReportDateTime>='" + dateStart +
        //                                              "' and ReportDateTime<='" + dateEnd + "' and CO like'" + MaxValueCO + "%'");
        //                                if (CO.Length > 0)
        //                                    if (CO[0]["ReportDateTime"].IsNotNullOrDBNull())
        //                                        dr["OutDate"] = string.Format("{0:yyyy-MM-dd}", CO[0]["ReportDateTime"]);
        //                            }
        //                            dr["OutBiggestFactor"] = strdCO;
        //                            //decimal YearAverageCO = (DecimalExtension.GetPollutantValue(decimal.TryParse(drAvg[0]["CO"].ToString(), out YearAverageCO) ? YearAverageCO : 0, COUnit));
        //                            //dr["YearAverage"] = YearAverageCO != 0 ? YearAverageCO.ToString() : "";
        //                            //if (entityyear != null && drAvg[0]["CO"] != DBNull.Value && entityyear.Upper != null && entityyear.Upper != 0)
        //                            //{
        //                            //    decimal dYearCO = (decimal)((Convert.ToDecimal(drAvg[0]["CO"]) - entityyear.Upper) / entityyear.Upper);
        //                            //    string strdYearCO = "";
        //                            //    if (dYearCO < 0)
        //                            //    {
        //                            //        strdYearCO = "/";
        //                            //    }
        //                            //    else
        //                            //    {
        //                            //        strdYearCO = DecimalExtension.GetPollutantValue(dYearCO, 2).ToString();
        //                            //    }
        //                            //    dr["YearOutRate"] = strdYearCO;
        //                            //}
        //                            if (yearpercentCO == null || yearpercentCO == "")
        //                            {
        //                                dr["YearPercent"] = "/";
        //                            }
        //                            else
        //                            {
        //                                dr["YearPercent"] = yearpercentCO;
        //                            }
        //                            if (entityyear != null && yearpercentCO != null && yearpercentCO != "" && entityyear.Upper != null && entityyear.Upper != 0)
        //                            {
        //                                decimal dYearPerCO = (decimal)((Convert.ToDecimal(yearpercentCO) - entityyear.Upper) / entityyear.Upper);
        //                                string strdYearPerCO = "";
        //                                if (dYearPerCO < 0)
        //                                {
        //                                    strdYearPerCO = "/";
        //                                }
        //                                else
        //                                {
        //                                    strdYearPerCO = DecimalExtension.GetPollutantValue(dYearPerCO, 2).ToString();
        //                                }
        //                                dr["YearPerOutRate"] = strdYearPerCO;
        //                            }
        //                            AreaData.RowFilter = "CO is not null and CO<>''  and MonitoringRegionUid='" + regionGuids[i] + "'";
        //                            List<string> strCO = AreaData.ToTable().AsEnumerable().Select(r => r.Field<string>("CO")).ToList();
        //                            List<decimal> decCO = new List<decimal>(strCO.Select(x => decimal.Parse(x)));
        //                            Dictionary<string, decimal> ListMedianCO = Median(decCO);
        //                            if (ListMedianCO.Keys.Contains("median"))
        //                            {
        //                                dr["MedianValueu"] = ListMedianCO["median"];
        //                            }
        //                            if (ListMedianCO.Keys.Contains("median1"))
        //                            {
        //                                dr["MedianLower"] = ListMedianCO["median1"];
        //                            }
        //                            if (ListMedianCO.Keys.Contains("median3"))
        //                            {
        //                                dr["MedianUpper"] = ListMedianCO["median3"];
        //                            } if (ListMedianCO.Keys.Contains("lower"))
        //                            {
        //                                dr["lower"] = ListMedianCO["lower"];
        //                            } if (ListMedianCO.Keys.Contains("upper"))
        //                            {
        //                                dr["upper"] = ListMedianCO["upper"];
        //                            }
        //                            AreaData.RowFilter = string.Empty;
        //                            break;
        //                        case IAQIType.Max8HourO3_IAQI:
        //                            entity = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a05024", EQITimeType.Eight);
        //                            //entityyear = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a05024", EQITimeType.Year);
        //                            string yearpercentO3 = "";
        //                            yearpercentO3 = pointDayAQI.getpercent(regionGuids[i], dateStart, dateEnd, "a05024");
        //                            dr["FactorName"] = "O3-8";
        //                            decimal MinValueMax8HourO3 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drMin[0]["Max8HourO3"].ToString(), out MinValueMax8HourO3) ? MinValueMax8HourO3 : 0, Max8HourO3Unit));
        //                            dr["DayMinValue"] = MinValueMax8HourO3 != 0 ? MinValueMax8HourO3.ToString() : "";
        //                            decimal MaxValueMax8HourO3 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drMax[0]["Max8HourO3"].ToString(), out MaxValueMax8HourO3) ? MaxValueMax8HourO3 : 0, Max8HourO3Unit));
        //                            dr["DayMaxValue"] = MaxValueMax8HourO3 != 0 ? MaxValueMax8HourO3.ToString() : "";
        //                            decimal AvgValueMax8HourO3 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drAvg[0]["Max8HourO3"].ToString(), out AvgValueMax8HourO3) ? AvgValueMax8HourO3 : 0, Max8HourO3Unit));
        //                            dr["DayAvgValue"] = AvgValueMax8HourO3 != 0 ? AvgValueMax8HourO3.ToString() : "";
        //                            dr["DataRange"] = MinValueMax8HourO3.ToString() + "~" + MaxValueMax8HourO3.ToString();
        //                            dr["OutDays"] = drExceeding[0]["Max8HourO3_Over"];
        //                            dr["MonitorDays"] = AreaData.ToTable().Select("MonitoringRegionUid='" + regionGuids[i] + "' and ReportDateTime>='" + dateStart +
        //                                "' and ReportDateTime<='" + dateEnd + "' and Max8HourO3_IAQI is not null and Max8HourO3_IAQI<>0").Count();
        //                            decimal outMax8HourO3 = 0;
        //                            if (Convert.ToDecimal(dr["MonitorDays"]) != 0)
        //                            {
        //                                outMax8HourO3 = (decimal)(Convert.ToDecimal(dr["OutDays"]) / Convert.ToDecimal(dr["MonitorDays"]) * 100);
        //                            }
        //                            dr["OutRate"] = DecimalExtension.GetPollutantValue(outMax8HourO3, 1).ToString() + "%";
        //                            decimal dMax8HourO3 = (decimal)((Convert.ToDecimal(drMax[0]["Max8HourO3"]) - entity.Upper) / entity.Upper);
        //                            string strdMax8HourO3 = "";
        //                            if (dMax8HourO3 < 0)
        //                            {
        //                                strdMax8HourO3 = "/";
        //                                dr["OutDate"] = "/";
        //                            }
        //                            else
        //                            {
        //                                strdMax8HourO3 = DecimalExtension.GetPollutantValue(dMax8HourO3, 2).ToString();
        //                                DataRow[] O3 = AreaData.ToTable().Select("MonitoringRegionUid='" + regionGuids[i] + "' and ReportDateTime>='" + dateStart +
        //                                           "' and ReportDateTime<='" + dateEnd + "' and Max8HourO3 like'" + MaxValueMax8HourO3 + "%'");
        //                                if (O3.Length > 0)
        //                                    if (O3[0]["ReportDateTime"].IsNotNullOrDBNull())
        //                                        dr["OutDate"] = string.Format("{0:yyyy-MM-dd}", O3[0]["ReportDateTime"]);
        //                            }
        //                            dr["OutBiggestFactor"] = strdMax8HourO3;
        //                            decimal YearAverageMax8HourO3 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drAvg[0]["Max8HourO3"].ToString(), out YearAverageMax8HourO3) ? YearAverageMax8HourO3 : 0, Max8HourO3Unit));
        //                            //dr["YearAverage"] = YearAverageMax8HourO3 != 0 ? YearAverageMax8HourO3.ToString() : "";
        //                            //dr["YearOutRate"] = "/";
        //                            if (yearpercentO3 == null || yearpercentO3 == "")
        //                            {
        //                                dr["YearPercent"] = "/";
        //                            }
        //                            else
        //                            {
        //                                dr["YearPercent"] = yearpercentO3;
        //                            }

        //                            dr["YearPerOutRate"] = "/";
        //                            AreaData.RowFilter = "Max8HourO3 is not null and Max8HourO3<>''  and MonitoringRegionUid='" + regionGuids[i] + "'";
        //                            List<string> strMax8HourO3 = AreaData.ToTable().AsEnumerable().Select(r => r.Field<string>("Max8HourO3")).ToList();
        //                            List<decimal> decMax8HourO3 = new List<decimal>(strMax8HourO3.Select(x => decimal.Parse(x)));
        //                            Dictionary<string, decimal> ListMedianMax8HourO3 = Median(decMax8HourO3);
        //                            if (ListMedianMax8HourO3.Keys.Contains("median"))
        //                            {
        //                                dr["MedianValueu"] = ListMedianMax8HourO3["median"];
        //                            }
        //                            if (ListMedianMax8HourO3.Keys.Contains("median1"))
        //                            {
        //                                dr["MedianLower"] = ListMedianMax8HourO3["median1"];
        //                            }
        //                            if (ListMedianMax8HourO3.Keys.Contains("median3"))
        //                            {
        //                                dr["MedianUpper"] = ListMedianMax8HourO3["median3"];
        //                            } if (ListMedianMax8HourO3.Keys.Contains("lower"))
        //                            {
        //                                dr["lower"] = ListMedianMax8HourO3["lower"];
        //                            } if (ListMedianMax8HourO3.Keys.Contains("upper"))
        //                            {
        //                                dr["upper"] = ListMedianMax8HourO3["upper"];
        //                            }
        //                            AreaData.RowFilter = string.Empty;
        //                            break;
        //                    }
        //                    dt.Rows.Add(dr);
        //                }
        //            }
        //        }
        //        return dt.DefaultView;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}
        /// <summary>
        /// 全市年数据统计
        /// </summary>
        /// <param name="regionGuids">区域Guid</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <param name="orderBy">排序</param>
        /// <returns>
        /// RegionName：区域
        /// FactorName：因子
        /// DayMinValue：最小值
        /// DayMaxValue：最大值
        /// OutDays：超标天数
        /// MonitorDays：监控天数
        /// OutRate：超标率
        /// OutBiggestFactor：最大超标倍数
        /// YearAverage：年均值
        /// </returns>
        public DataView GetAllYearData(string[] regionGuids, DateTime dateStart, DateTime dateEnd)
        {
            try
            {
                pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
                AirPollutantService m_AirPollutantService = new AirPollutantService();
                int PM25Unit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a34004").PollutantDecimalNum);
                int PM10Unit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a34002").PollutantDecimalNum);
                int NO2Unit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a21004").PollutantDecimalNum);
                int SO2Unit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a21026").PollutantDecimalNum);
                int COUnit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a21005").PollutantDecimalNum);
                int Max8HourO3Unit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a05024").PollutantDecimalNum);
                int recordTotal = 0;
                DataView AreaData = GetAreaDataPager(regionGuids, dateStart, dateEnd, 99999, 0, out recordTotal);
                EQIConcentrationService EQIConcentration = new EQIConcentrationService();
                EQIConcentrationLimitEntity entity = null;
                EQIConcentrationLimitEntity entityyear = null;
                DataView MinValues = GetRegionsMinValue(regionGuids, dateStart, dateEnd);
                DataView MaxValues = GetRegionsMaxValue(regionGuids, dateStart, dateEnd);
                DataView ExceedingDatas = GetRegionsExceedingData(regionGuids, dateStart, dateEnd);
                DataView AvgValues = GetRegionsAvgValue(regionGuids, dateStart, dateEnd);
                DictionaryService dictionary = new DictionaryService();
                DataTable dt = new DataTable();
                dt.Columns.Add("RegionName", typeof(string));
                dt.Columns.Add("FactorName", typeof(string));
                dt.Columns.Add("DayMinValue", typeof(string));
                dt.Columns.Add("DayMaxValue", typeof(string));
                dt.Columns.Add("DayAvgValue", typeof(string));
                dt.Columns.Add("OutDays", typeof(string));
                dt.Columns.Add("MonitorDays", typeof(string));
                dt.Columns.Add("OutRate", typeof(string));
                dt.Columns.Add("OutBiggestFactor", typeof(string));
                dt.Columns.Add("OutDate", typeof(string));
                dt.Columns.Add("DataRange", typeof(string));
                //dt.Columns.Add("YearAverage", typeof(string));
                //dt.Columns.Add("YearOutRate", typeof(string));
                dt.Columns.Add("YearPercent", typeof(string));
                dt.Columns.Add("YearPerOutRate", typeof(string));
                dt.Columns.Add("MedianUpper", typeof(string));
                dt.Columns.Add("MedianLower", typeof(string));
                dt.Columns.Add("MedianValueu", typeof(string));
                dt.Columns.Add("lower", typeof(string));
                dt.Columns.Add("upper", typeof(string));
                List<IAQIType> AQITypes = new List<IAQIType>();
                AQITypes.Add(IAQIType.SO2_IAQI);
                AQITypes.Add(IAQIType.NO2_IAQI);
                AQITypes.Add(IAQIType.PM10_IAQI);
                AQITypes.Add(IAQIType.PM25_IAQI);
                AQITypes.Add(IAQIType.CO_IAQI);
                AQITypes.Add(IAQIType.Max8HourO3_IAQI);

                for (int i = 0; i < regionGuids.Length; i++)
                {
                    string RegionName = dictionary.GetCodeDictionaryTextByValue(regionGuids[i]);
                    DataRow[] drMin = MinValues.Table.Select("MonitoringRegionUid='" + regionGuids[i] + "'");
                    DataRow[] drMax = MaxValues.Table.Select("MonitoringRegionUid='" + regionGuids[i] + "'");
                    DataRow[] drExceeding = ExceedingDatas.Table.Select("MonitoringRegionUid='" + regionGuids[i] + "'");
                    DataRow[] drAvg = AvgValues.Table.Select("MonitoringRegionUid='" + regionGuids[i] + "'");

                    if (drMin.Length > 0 && drMax.Length > 0 && drExceeding.Length > 0 && drAvg.Length > 0)
                    {
                        for (int j = 0; j < AQITypes.Count; j++)
                        {
                            DataRow dr = dt.NewRow();
                            dr["RegionName"] = RegionName;
                            switch (AQITypes[j])
                            {
                                case IAQIType.SO2_IAQI:
                                    entity = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a21026", EQITimeType.TwentyFour);
                                    entityyear = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a21026", EQITimeType.Year);
                                    string yearpercentSO2 = "";
                                    yearpercentSO2 = pointDayAQI.getpercent(regionGuids[i], dateStart, dateEnd, "a21026");
                                    dr["FactorName"] = "SO2";
                                    decimal MinValue = (DecimalExtension.GetPollutantValue(decimal.TryParse(drMin[0]["SO2"].ToString(), out MinValue) ? MinValue : 0, SO2Unit));
                                    dr["DayMinValue"] = MinValue != 0 ? MinValue.ToString() : "";
                                    decimal MaxValue = (DecimalExtension.GetPollutantValue(decimal.TryParse(drMax[0]["SO2"].ToString(), out MaxValue) ? MaxValue : 0, SO2Unit));
                                    dr["DayMaxValue"] = MaxValue != 0 ? MaxValue.ToString() : "";
                                    decimal AvgValue = (DecimalExtension.GetPollutantValue(decimal.TryParse(drAvg[0]["SO2"].ToString(), out AvgValue) ? AvgValue : 0, SO2Unit));
                                    dr["DayAvgValue"] = AvgValue != 0 ? AvgValue.ToString() : "";
                                    dr["DataRange"] = MinValue.ToString() + "~" + MaxValue.ToString();
                                    dr["OutDays"] = drExceeding[0]["SO2_Over"];
                                    dr["MonitorDays"] = AreaData.ToTable().Select("MonitoringRegionUid='" + regionGuids[i] + "' and ReportDateTime>='" + dateStart +
                                        "' and ReportDateTime<='" + dateEnd + "' and SO2_IAQI is not null and SO2_IAQI<>0").Count();
                                    decimal outSO2 = 0;
                                    if (Convert.ToDecimal(dr["MonitorDays"]) != 0)
                                    {
                                        outSO2 = (decimal)(Convert.ToDecimal(dr["OutDays"]) / Convert.ToDecimal(dr["MonitorDays"]) * 100);
                                    }
                                    dr["OutRate"] = DecimalExtension.GetPollutantValue(outSO2, 1).ToString() + "%";
                                    decimal dSO2 = (decimal)((Convert.ToDecimal(drMax[0]["SO2"]) - entity.Upper) / entity.Upper);
                                    string strdSO2 = "";
                                    if (dSO2 < 0)
                                    {
                                        strdSO2 = "/";
                                        dr["OutDate"] = "/";
                                    }
                                    else
                                    {
                                        strdSO2 = DecimalExtension.GetPollutantValue(dSO2, 2).ToString();
                                        DataRow[] SO2 = AreaData.ToTable().Select("MonitoringRegionUid='" + regionGuids[i] + "' and ReportDateTime>='" + dateStart +
                                        "' and ReportDateTime<='" + dateEnd + "' and SO2 like'" + MaxValue + "%'");
                                        if (SO2.Length > 0)
                                            if (SO2[0]["ReportDateTime"].IsNotNullOrDBNull())
                                                dr["OutDate"] = string.Format("{0:yyyy-MM-dd}", SO2[0]["ReportDateTime"]);
                                    }
                                    dr["OutBiggestFactor"] = strdSO2;
                                    //decimal YearAverage = (DecimalExtension.GetPollutantValue(decimal.TryParse(drAvg[0]["SO2"].ToString(), out YearAverage) ? YearAverage : 0, SO2Unit));
                                    //dr["YearAverage"] = YearAverage != 0 ? YearAverage.ToString() : "";
                                    //if (entityyear != null && drAvg[0]["SO2"] != DBNull.Value && entityyear.Upper != null && entityyear.Upper != 0)
                                    //{
                                    //    decimal dYearSO2 = (decimal)((Convert.ToDecimal(drAvg[0]["SO2"]) - entityyear.Upper) / entityyear.Upper);
                                    //    string strdYearSO2 = "";
                                    //    if (dYearSO2 < 0)
                                    //    {
                                    //        strdYearSO2 = "/";
                                    //    }
                                    //    else
                                    //    {
                                    //        strdYearSO2 = DecimalExtension.GetPollutantValue(dYearSO2, 2).ToString();
                                    //    }
                                    //    dr["YearOutRate"] = strdYearSO2;
                                    //}
                                    if (yearpercentSO2 == null || yearpercentSO2 == "")
                                    {
                                        dr["YearPercent"] = "/";
                                    }
                                    else
                                    {
                                        dr["YearPercent"] = yearpercentSO2;
                                    }
                                    if (entityyear != null && yearpercentSO2 != null && yearpercentSO2 != "" && entityyear.Upper != null && entityyear.Upper != 0)
                                    {
                                        decimal dYearPerSO2 = (decimal)((Convert.ToDecimal(yearpercentSO2) / 1000 - entityyear.Upper) / entityyear.Upper);
                                        string strdYearPerSO2 = "";
                                        if (dYearPerSO2 < 0)
                                        {
                                            strdYearPerSO2 = "/";
                                        }
                                        else
                                        {
                                            strdYearPerSO2 = DecimalExtension.GetPollutantValue(dYearPerSO2, 2).ToString();
                                        }
                                        dr["YearPerOutRate"] = strdYearPerSO2;
                                    }

                                    AreaData.RowFilter = "SO2 is not null and SO2<>'' and MonitoringRegionUid='" + regionGuids[i] + "'";
                                    List<string> strSO2 = AreaData.ToTable().AsEnumerable().Select(r => r.Field<string>("SO2")).ToList();
                                    List<decimal> decSO2 = new List<decimal>(strSO2.Select(x => decimal.Parse(x)));
                                    Dictionary<string, string> ListMedian = Median(decSO2, "");
                                    if (ListMedian.Keys.Contains("median"))
                                    {
                                        dr["MedianValueu"] = ListMedian["median"];
                                    }
                                    if (ListMedian.Keys.Contains("median1"))
                                    {
                                        dr["MedianLower"] = ListMedian["median1"];
                                    }
                                    if (ListMedian.Keys.Contains("median3"))
                                    {
                                        dr["MedianUpper"] = ListMedian["median3"];
                                    } if (ListMedian.Keys.Contains("lower"))
                                    {
                                        dr["lower"] = ListMedian["lower"];
                                    } if (ListMedian.Keys.Contains("upper"))
                                    {
                                        dr["upper"] = ListMedian["upper"];
                                    }
                                    AreaData.RowFilter = string.Empty;
                                    break;
                                case IAQIType.NO2_IAQI:
                                    entity = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a21004", EQITimeType.TwentyFour);
                                    entityyear = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a21004", EQITimeType.Year);
                                    string yearpercentNO2 = "";
                                    yearpercentNO2 = pointDayAQI.getpercent(regionGuids[i], dateStart, dateEnd, "a21004");
                                    dr["FactorName"] = "NO2";
                                    decimal MinValueNO2 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drMin[0]["NO2"].ToString(), out MinValueNO2) ? MinValueNO2 : 0, NO2Unit));
                                    dr["DayMinValue"] = MinValueNO2 != 0 ? MinValueNO2.ToString() : "";
                                    decimal MaxValueNO2 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drMax[0]["NO2"].ToString(), out MaxValueNO2) ? MaxValueNO2 : 0, NO2Unit));
                                    dr["DayMaxValue"] = MaxValueNO2 != 0 ? MaxValueNO2.ToString() : "";
                                    decimal AvgValueNO2 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drAvg[0]["NO2"].ToString(), out AvgValueNO2) ? AvgValueNO2 : 0, NO2Unit));
                                    dr["DayAvgValue"] = AvgValueNO2 != 0 ? AvgValueNO2.ToString() : "";
                                    dr["DataRange"] = MinValueNO2.ToString() + "~" + MaxValueNO2.ToString();
                                    dr["OutDays"] = drExceeding[0]["NO2_Over"];
                                    dr["MonitorDays"] = AreaData.ToTable().Select("MonitoringRegionUid='" + regionGuids[i] + "' and ReportDateTime>='" + dateStart +
                                        "' and ReportDateTime<='" + dateEnd + "' and NO2_IAQI is not null and NO2_IAQI<>0").Count();
                                    decimal outNO2 = 0;
                                    if (Convert.ToDecimal(dr["MonitorDays"]) != 0)
                                    {
                                        outNO2 = (decimal)(Convert.ToDecimal(dr["OutDays"]) / Convert.ToDecimal(dr["MonitorDays"]) * 100);
                                    }
                                    dr["OutRate"] = DecimalExtension.GetPollutantValue(outNO2, 1).ToString() + "%";
                                    decimal dNO2 = (decimal)((Convert.ToDecimal(drMax[0]["NO2"]) - entity.Upper) / entity.Upper);
                                    string strdNO2 = "";
                                    if (dNO2 < 0)
                                    {
                                        strdNO2 = "/";
                                        dr["OutDate"] = "/";
                                    }
                                    else
                                    {
                                        strdNO2 = DecimalExtension.GetPollutantValue(dNO2, 2).ToString();
                                        DataRow[] NO2 = AreaData.ToTable().Select("MonitoringRegionUid='" + regionGuids[i] + "' and ReportDateTime>='" + dateStart +
                                   "' and ReportDateTime<='" + dateEnd + "' and NO2 like'" + MaxValueNO2 + "%'");
                                        if (NO2.Length > 0)
                                            if (NO2[0]["ReportDateTime"].IsNotNullOrDBNull())
                                                dr["OutDate"] = string.Format("{0:yyyy-MM-dd}", NO2[0]["ReportDateTime"]);
                                    }
                                    dr["OutBiggestFactor"] = strdNO2;
                                    //decimal YearAverageNO2 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drAvg[0]["NO2"].ToString(), out YearAverageNO2) ? YearAverageNO2 : 0, NO2Unit));
                                    //dr["YearAverage"] = YearAverageNO2 != 0 ? YearAverageNO2.ToString() : "";
                                    //if (entityyear != null && drAvg[0]["NO2"] != DBNull.Value && entityyear.Upper != null && entityyear.Upper != 0)
                                    //{
                                    //    decimal dYearNO2 = (decimal)((Convert.ToDecimal(drAvg[0]["NO2"]) - entityyear.Upper) / entityyear.Upper);
                                    //    string strdYearNO2 = "";
                                    //    if (dYearNO2 < 0)
                                    //    {
                                    //        strdYearNO2 = "/";
                                    //    }
                                    //    else
                                    //    {
                                    //        strdYearNO2 = DecimalExtension.GetPollutantValue(dYearNO2, 2).ToString();
                                    //    }
                                    //    dr["YearOutRate"] = strdYearNO2;
                                    //}
                                    if (yearpercentNO2 == null || yearpercentNO2 == "")
                                    {
                                        dr["YearPercent"] = "/";
                                    }
                                    else
                                    {
                                        dr["YearPercent"] = yearpercentNO2;
                                    }
                                    if (entityyear != null && yearpercentNO2 != null && yearpercentNO2 != "" && entityyear.Upper != null && entityyear.Upper != 0)
                                    {
                                        decimal dYearPerNO2 = (decimal)((Convert.ToDecimal(yearpercentNO2) / 1000 - entityyear.Upper) / entityyear.Upper);
                                        string strdYearPerNO2 = "";
                                        if (dYearPerNO2 < 0)
                                        {
                                            strdYearPerNO2 = "/";
                                        }
                                        else
                                        {
                                            strdYearPerNO2 = DecimalExtension.GetPollutantValue(dYearPerNO2, 2).ToString();
                                        }
                                        dr["YearPerOutRate"] = strdYearPerNO2;
                                    }
                                    AreaData.RowFilter = "NO2 is not null and NO2<>'' and MonitoringRegionUid='" + regionGuids[i] + "'";
                                    List<string> strNO2 = AreaData.ToTable().AsEnumerable().Select(r => r.Field<string>("NO2")).ToList();
                                    List<decimal> decNO2 = new List<decimal>(strNO2.Select(x => decimal.Parse(x)));
                                    Dictionary<string, string> ListMedianNO2 = Median(decNO2, "");
                                    if (ListMedianNO2.Keys.Contains("median"))
                                    {
                                        dr["MedianValueu"] = ListMedianNO2["median"];
                                    }
                                    if (ListMedianNO2.Keys.Contains("median1"))
                                    {
                                        dr["MedianLower"] = ListMedianNO2["median1"];
                                    }
                                    if (ListMedianNO2.Keys.Contains("median3"))
                                    {
                                        dr["MedianUpper"] = ListMedianNO2["median3"];
                                    } if (ListMedianNO2.Keys.Contains("lower"))
                                    {
                                        dr["lower"] = ListMedianNO2["lower"];
                                    } if (ListMedianNO2.Keys.Contains("upper"))
                                    {
                                        dr["upper"] = ListMedianNO2["upper"];
                                    }
                                    AreaData.RowFilter = string.Empty;
                                    break;
                                case IAQIType.PM10_IAQI:
                                    entity = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a34002", EQITimeType.TwentyFour);
                                    entityyear = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a34002", EQITimeType.Year);
                                    string yearpercentPM10 = "";
                                    yearpercentPM10 = pointDayAQI.getpercent(regionGuids[i], dateStart, dateEnd, "a34002");
                                    dr["FactorName"] = "PM10";
                                    decimal MinValuePM10 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drMin[0]["PM10"].ToString(), out MinValuePM10) ? MinValuePM10 : 0, PM10Unit));
                                    dr["DayMinValue"] = MinValuePM10 != 0 ? MinValuePM10.ToString() : "";
                                    decimal MaxValuePM10 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drMax[0]["PM10"].ToString(), out MaxValuePM10) ? MaxValuePM10 : 0, PM10Unit));
                                    dr["DayMaxValue"] = MaxValuePM10 != 0 ? MaxValuePM10.ToString() : "";
                                    decimal AvgValuePM10 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drAvg[0]["PM10"].ToString(), out AvgValuePM10) ? AvgValuePM10 : 0, PM10Unit));
                                    dr["DayAvgValue"] = AvgValuePM10 != 0 ? AvgValuePM10.ToString() : "";
                                    dr["DataRange"] = MinValuePM10.ToString() + "~" + MaxValuePM10.ToString();
                                    dr["OutDays"] = drExceeding[0]["PM10_Over"];
                                    dr["MonitorDays"] = AreaData.ToTable().Select("MonitoringRegionUid='" + regionGuids[i] + "' and ReportDateTime>='" + dateStart +
                                        "' and ReportDateTime<='" + dateEnd + "' and PM10_IAQI is not null and PM10_IAQI<>0").Count();
                                    decimal outPM10 = 0;
                                    if (Convert.ToDecimal(dr["MonitorDays"]) != 0)
                                    {
                                        outPM10 = (decimal)(Convert.ToDecimal(dr["OutDays"]) / Convert.ToDecimal(dr["MonitorDays"]) * 100);
                                    }
                                    dr["OutRate"] = DecimalExtension.GetPollutantValue(outPM10, 1).ToString() + "%";
                                    decimal dPM10 = (decimal)(Convert.ToDecimal(drMax[0]["PM10"]) - entity.Upper);
                                    string strdPM10 = "";
                                    if (dPM10 < 0)
                                    {
                                        strdPM10 = "/";
                                        dr["OutDate"] = "/";
                                    }
                                    else
                                    {
                                        strdPM10 = DecimalExtension.GetPollutantValue(dPM10, 2).ToString();
                                        DataRow[] PM10 = AreaData.ToTable().Select("MonitoringRegionUid='" + regionGuids[i] + "' and ReportDateTime>='" + dateStart +
                                                         "' and ReportDateTime<='" + dateEnd + "' and PM10 like'" + MaxValuePM10 + "%'");
                                        if (PM10.Length > 0)
                                            if (PM10[0]["ReportDateTime"].IsNotNullOrDBNull())
                                                dr["OutDate"] = string.Format("{0:yyyy-MM-dd}", PM10[0]["ReportDateTime"]);
                                    }
                                    dr["OutBiggestFactor"] = strdPM10;
                                    //decimal YearAveragePM10 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drAvg[0]["PM10"].ToString(), out YearAveragePM10) ? YearAveragePM10 : 0, PM10Unit));
                                    //dr["YearAverage"] = YearAveragePM10 != 0 ? YearAveragePM10.ToString() : "";
                                    //if (entityyear != null && drAvg[0]["PM10"] != DBNull.Value && entityyear.Upper != null && entityyear.Upper != 0)
                                    //{
                                    //    decimal dYearPM10 = (decimal)((Convert.ToDecimal(drAvg[0]["PM10"]) - entityyear.Upper) / entityyear.Upper);
                                    //    string strdYearPM10 = "";
                                    //    if (dYearPM10 < 0)
                                    //    {
                                    //        strdYearPM10 = "/";
                                    //    }
                                    //    else
                                    //    {
                                    //        strdYearPM10 = DecimalExtension.GetPollutantValue(dYearPM10, 2).ToString();
                                    //    }
                                    //    dr["YearOutRate"] = strdYearPM10;
                                    //}
                                    if (yearpercentPM10 == null || yearpercentPM10 == "")
                                    {
                                        dr["YearPercent"] = "/";
                                    }
                                    else
                                    {
                                        dr["YearPercent"] = yearpercentPM10;
                                    }
                                    if (entityyear != null && yearpercentPM10 != null && yearpercentPM10 != "" && entityyear.Upper != null && entityyear.Upper != 0)
                                    {
                                        decimal dYearPerPM10 = (decimal)((Convert.ToDecimal(yearpercentPM10) / 1000 - entityyear.Upper) / entityyear.Upper);
                                        string strdYearPerPM10 = "";
                                        if (dYearPerPM10 < 0)
                                        {
                                            strdYearPerPM10 = "/";
                                        }
                                        else
                                        {
                                            strdYearPerPM10 = DecimalExtension.GetPollutantValue(dYearPerPM10, 2).ToString();
                                        }
                                        dr["YearPerOutRate"] = strdYearPerPM10;
                                    }
                                    AreaData.RowFilter = "PM10 is not null and PM10<>''  and MonitoringRegionUid='" + regionGuids[i] + "'";
                                    List<string> strPM10 = AreaData.ToTable().AsEnumerable().Select(r => r.Field<string>("PM10")).ToList();
                                    List<decimal> decPM10 = new List<decimal>(strPM10.Select(x => decimal.Parse(x)));
                                    Dictionary<string, string> ListMedianPM10 = Median(decPM10, "");
                                    if (ListMedianPM10.Keys.Contains("median"))
                                    {
                                        dr["MedianValueu"] = ListMedianPM10["median"];
                                    }
                                    if (ListMedianPM10.Keys.Contains("median1"))
                                    {
                                        dr["MedianLower"] = ListMedianPM10["median1"];
                                    }
                                    if (ListMedianPM10.Keys.Contains("median3"))
                                    {
                                        dr["MedianUpper"] = ListMedianPM10["median3"];
                                    } if (ListMedianPM10.Keys.Contains("lower"))
                                    {
                                        dr["lower"] = ListMedianPM10["lower"];
                                    } if (ListMedianPM10.Keys.Contains("upper"))
                                    {
                                        dr["upper"] = ListMedianPM10["upper"];
                                    }
                                    AreaData.RowFilter = string.Empty;
                                    break;
                                case IAQIType.PM25_IAQI:
                                    entity = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a34004", EQITimeType.TwentyFour);
                                    entityyear = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a34004", EQITimeType.Year);
                                    string yearpercentPM25 = "";
                                    yearpercentPM25 = pointDayAQI.getpercent(regionGuids[i], dateStart, dateEnd, "a34004");
                                    dr["FactorName"] = "PM2.5";
                                    decimal MinValuePM25 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drMin[0]["PM25"].ToString(), out MinValuePM25) ? MinValuePM25 : 0, PM25Unit));
                                    dr["DayMinValue"] = MinValuePM25 != 0 ? MinValuePM25.ToString() : "";
                                    decimal MaxValuePM25 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drMax[0]["PM25"].ToString(), out MaxValuePM25) ? MaxValuePM25 : 0, PM25Unit));
                                    dr["DayMaxValue"] = MaxValuePM25 != 0 ? MaxValuePM25.ToString() : "";
                                    decimal AvgValuePM25 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drAvg[0]["PM25"].ToString(), out AvgValuePM25) ? AvgValuePM25 : 0, PM25Unit));
                                    dr["DayAvgValue"] = AvgValuePM25 != 0 ? AvgValuePM25.ToString() : "";
                                    dr["DataRange"] = MinValuePM25.ToString() + "~" + MaxValuePM25.ToString();
                                    dr["OutDays"] = drExceeding[0]["PM25_Over"];
                                    dr["MonitorDays"] = AreaData.ToTable().Select("MonitoringRegionUid='" + regionGuids[i] + "' and ReportDateTime>='" + dateStart +
                                        "' and ReportDateTime<='" + dateEnd + "' and PM25_IAQI is not null and PM25_IAQI<>0").Count();
                                    decimal outPM25 = 0;
                                    if (Convert.ToDecimal(dr["MonitorDays"]) != 0)
                                    {
                                        outPM25 = (decimal)(Convert.ToDecimal(dr["OutDays"]) / Convert.ToDecimal(dr["MonitorDays"]) * 100);
                                    }
                                    dr["OutRate"] = DecimalExtension.GetPollutantValue(outPM25, 1).ToString() + "%";
                                    //dr["OutBiggestFactor"] = ((Convert.ToDecimal(drMax[0]["PM25"]) - entity.Upper) / entity.Upper).ToString();
                                    decimal dPM25 = (decimal)((Convert.ToDecimal(drMax[0]["PM25"]) - entity.Upper) / entity.Upper);
                                    string strdPM25 = "";
                                    if (dPM25 < 0)
                                    {
                                        strdPM25 = "/";
                                        dr["OutDate"] = "/";
                                    }
                                    else
                                    {
                                        strdPM25 = DecimalExtension.GetPollutantValue(dPM25, 2).ToString();
                                        DataRow[] PM25 = AreaData.ToTable().Select("MonitoringRegionUid='" + regionGuids[i] + "' and ReportDateTime>='" + dateStart +
                                                         "' and ReportDateTime<='" + dateEnd + "' and PM25 like'" + MaxValuePM25 + "%'");
                                        if (PM25.Length > 0)
                                            if (PM25[0]["ReportDateTime"].IsNotNullOrDBNull())
                                                dr["OutDate"] = string.Format("{0:yyyy-MM-dd}", PM25[0]["ReportDateTime"]);
                                    }
                                    dr["OutBiggestFactor"] = strdPM25;
                                    //decimal YearAveragePM25 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drAvg[0]["PM25"].ToString(), out YearAveragePM25) ? YearAveragePM25 : 0, PM25Unit));
                                    //dr["YearAverage"] = YearAveragePM25 != 0 ? YearAveragePM25.ToString() : "";
                                    //if (entityyear != null && drAvg[0]["PM25"] != DBNull.Value && entityyear.Upper != null && entityyear.Upper != 0)
                                    //{
                                    //    decimal dYearPM25 = (decimal)((Convert.ToDecimal(drAvg[0]["PM25"]) - entityyear.Upper) / entityyear.Upper);
                                    //    string strdYearPM25 = "";
                                    //    if (dYearPM25 < 0)
                                    //    {
                                    //        strdYearPM25 = "/";
                                    //    }
                                    //    else
                                    //    {
                                    //        strdYearPM25 = DecimalExtension.GetPollutantValue(dYearPM25, 2).ToString();
                                    //    }
                                    //    dr["YearOutRate"] = strdYearPM25;
                                    //}
                                    if (yearpercentPM25 == null || yearpercentPM25 == "")
                                    {
                                        dr["YearPercent"] = "/";
                                    }
                                    else
                                    {
                                        dr["YearPercent"] = yearpercentPM25;
                                    }
                                    if (entityyear != null && yearpercentPM25 != null && yearpercentPM25 != "" && entityyear.Upper != null && entityyear.Upper != 0)
                                    {
                                        decimal dYearPerPM25 = (decimal)((Convert.ToDecimal(yearpercentPM25) / 1000 - entityyear.Upper) / entityyear.Upper);
                                        string strdYearPerPM25 = "";
                                        if (dYearPerPM25 < 0)
                                        {
                                            strdYearPerPM25 = "/";
                                        }
                                        else
                                        {
                                            strdYearPerPM25 = DecimalExtension.GetPollutantValue(dYearPerPM25, 2).ToString();
                                        }
                                        dr["YearPerOutRate"] = strdYearPerPM25;
                                    }
                                    AreaData.RowFilter = "PM25 is not null and PM25<>''  and MonitoringRegionUid='" + regionGuids[i] + "'";
                                    List<string> strPM25 = AreaData.ToTable().AsEnumerable().Select(r => r.Field<string>("PM25")).ToList();
                                    List<decimal> decPM25 = new List<decimal>(strPM25.Select(x => decimal.Parse(x)));
                                    Dictionary<string, string> ListMedianPM25 = Median(decPM25, "");
                                    if (ListMedianPM25.Keys.Contains("median"))
                                    {
                                        dr["MedianValueu"] = ListMedianPM25["median"];
                                    }
                                    if (ListMedianPM25.Keys.Contains("median1"))
                                    {
                                        dr["MedianLower"] = ListMedianPM25["median1"];
                                    }
                                    if (ListMedianPM25.Keys.Contains("median3"))
                                    {
                                        dr["MedianUpper"] = ListMedianPM25["median3"];
                                    } if (ListMedianPM25.Keys.Contains("lower"))
                                    {
                                        dr["lower"] = ListMedianPM25["lower"];
                                    } if (ListMedianPM25.Keys.Contains("upper"))
                                    {
                                        dr["upper"] = ListMedianPM25["upper"];
                                    }
                                    AreaData.RowFilter = string.Empty;
                                    break;
                                case IAQIType.CO_IAQI:
                                    entity = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a21005", EQITimeType.TwentyFour);
                                    entityyear = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a21005", EQITimeType.Year);
                                    string yearpercentCO = "";
                                    yearpercentCO = pointDayAQI.getpercent(regionGuids[i], dateStart, dateEnd, "a21005");
                                    dr["FactorName"] = "CO";
                                    decimal MinValueCO = (DecimalExtension.GetPollutantValue(decimal.TryParse(drMin[0]["CO"].ToString(), out MinValueCO) ? MinValueCO : 0, COUnit));
                                    dr["DayMinValue"] = MinValueCO != 0 ? MinValueCO.ToString() : "";
                                    decimal MaxValueCO = (DecimalExtension.GetPollutantValue(decimal.TryParse(drMax[0]["CO"].ToString(), out MaxValueCO) ? MaxValueCO : 0, COUnit));
                                    dr["DayMaxValue"] = MaxValueCO != 0 ? MaxValueCO.ToString() : "";
                                    decimal AvgValueCO = (DecimalExtension.GetPollutantValue(decimal.TryParse(drAvg[0]["CO"].ToString(), out AvgValueCO) ? AvgValueCO : 0, COUnit));
                                    dr["DayAvgValue"] = AvgValueCO != 0 ? AvgValueCO.ToString() : "";
                                    dr["DataRange"] = MinValueCO.ToString() + "~" + MaxValueCO.ToString();
                                    dr["OutDays"] = drExceeding[0]["CO_Over"];
                                    dr["MonitorDays"] = AreaData.ToTable().Select("MonitoringRegionUid='" + regionGuids[i] + "' and ReportDateTime>='" + dateStart +
                                        "' and ReportDateTime<='" + dateEnd + "' and CO_IAQI is not null and CO_IAQI<>0").Count();
                                    decimal outCO = 0;
                                    if (Convert.ToDecimal(dr["MonitorDays"]) != 0)
                                    {
                                        outCO = (decimal)(Convert.ToDecimal(dr["OutDays"]) / Convert.ToDecimal(dr["MonitorDays"]) * 100);
                                    }
                                    dr["OutRate"] = DecimalExtension.GetPollutantValue(outCO, 1).ToString() + "%";

                                    //dr["OutBiggestFactor"] = ((Convert.ToDecimal(drMax[0]["CO"]) - entity.Upper) / entity.Upper).ToString();
                                    decimal dCO = (decimal)((Convert.ToDecimal(drMax[0]["CO"]) - entity.Upper) / entity.Upper);
                                    string strdCO = "";
                                    if (dCO < 0)
                                    {
                                        strdCO = "/";
                                        dr["OutDate"] = "/";
                                    }
                                    else
                                    {
                                        strdCO = DecimalExtension.GetPollutantValue(dCO, 2).ToString();
                                        DataRow[] CO = AreaData.ToTable().Select("MonitoringRegionUid='" + regionGuids[i] + "' and ReportDateTime>='" + dateStart +
                                                      "' and ReportDateTime<='" + dateEnd + "' and CO like'" + MaxValueCO + "%'");
                                        if (CO.Length > 0)
                                            if (CO[0]["ReportDateTime"].IsNotNullOrDBNull())
                                                dr["OutDate"] = string.Format("{0:yyyy-MM-dd}", CO[0]["ReportDateTime"]);
                                    }
                                    dr["OutBiggestFactor"] = strdCO;
                                    //decimal YearAverageCO = (DecimalExtension.GetPollutantValue(decimal.TryParse(drAvg[0]["CO"].ToString(), out YearAverageCO) ? YearAverageCO : 0, COUnit));
                                    //dr["YearAverage"] = YearAverageCO != 0 ? YearAverageCO.ToString() : "";
                                    //if (entityyear != null && drAvg[0]["CO"] != DBNull.Value && entityyear.Upper != null && entityyear.Upper != 0)
                                    //{
                                    //    decimal dYearCO = (decimal)((Convert.ToDecimal(drAvg[0]["CO"]) - entityyear.Upper) / entityyear.Upper);
                                    //    string strdYearCO = "";
                                    //    if (dYearCO < 0)
                                    //    {
                                    //        strdYearCO = "/";
                                    //    }
                                    //    else
                                    //    {
                                    //        strdYearCO = DecimalExtension.GetPollutantValue(dYearCO, 2).ToString();
                                    //    }
                                    //    dr["YearOutRate"] = strdYearCO;
                                    //}
                                    if (yearpercentCO == null || yearpercentCO == "")
                                    {
                                        dr["YearPercent"] = "/";
                                    }
                                    else
                                    {
                                        dr["YearPercent"] = yearpercentCO;
                                    }
                                    if (entityyear != null && yearpercentCO != null && yearpercentCO != "" && entityyear.Upper != null && entityyear.Upper != 0)
                                    {
                                        decimal dYearPerCO = (decimal)((Convert.ToDecimal(yearpercentCO) - entityyear.Upper) / entityyear.Upper);
                                        string strdYearPerCO = "";
                                        if (dYearPerCO < 0)
                                        {
                                            strdYearPerCO = "/";
                                        }
                                        else
                                        {
                                            strdYearPerCO = DecimalExtension.GetPollutantValue(dYearPerCO, 2).ToString();
                                        }
                                        dr["YearPerOutRate"] = strdYearPerCO;
                                    }
                                    AreaData.RowFilter = "CO is not null and CO<>''  and MonitoringRegionUid='" + regionGuids[i] + "'";
                                    List<string> strCO = AreaData.ToTable().AsEnumerable().Select(r => r.Field<string>("CO")).ToList();
                                    List<decimal> decCO = new List<decimal>(strCO.Select(x => decimal.Parse(x)));
                                    Dictionary<string, string> ListMedianCO = Median(decCO, "CO");
                                    if (ListMedianCO.Keys.Contains("median"))
                                    {
                                        dr["MedianValueu"] = ListMedianCO["median"];
                                    }
                                    if (ListMedianCO.Keys.Contains("median1"))
                                    {
                                        dr["MedianLower"] = ListMedianCO["median1"];
                                    }
                                    if (ListMedianCO.Keys.Contains("median3"))
                                    {
                                        dr["MedianUpper"] = ListMedianCO["median3"];
                                    } if (ListMedianCO.Keys.Contains("lower"))
                                    {
                                        dr["lower"] = ListMedianCO["lower"];
                                    } if (ListMedianCO.Keys.Contains("upper"))
                                    {
                                        dr["upper"] = ListMedianCO["upper"];
                                    }
                                    AreaData.RowFilter = string.Empty;
                                    break;
                                case IAQIType.Max8HourO3_IAQI:
                                    entity = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a05024", EQITimeType.Eight);
                                    //entityyear = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a05024", EQITimeType.Year);
                                    string yearpercentO3 = "";
                                    yearpercentO3 = pointDayAQI.getpercent(regionGuids[i], dateStart, dateEnd, "a05024");
                                    dr["FactorName"] = "O3-8";
                                    decimal MinValueMax8HourO3 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drMin[0]["Max8HourO3"].ToString(), out MinValueMax8HourO3) ? MinValueMax8HourO3 : 0, Max8HourO3Unit));
                                    dr["DayMinValue"] = MinValueMax8HourO3 != 0 ? MinValueMax8HourO3.ToString() : "";
                                    decimal MaxValueMax8HourO3 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drMax[0]["Max8HourO3"].ToString(), out MaxValueMax8HourO3) ? MaxValueMax8HourO3 : 0, Max8HourO3Unit));
                                    dr["DayMaxValue"] = MaxValueMax8HourO3 != 0 ? MaxValueMax8HourO3.ToString() : "";
                                    decimal AvgValueMax8HourO3 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drAvg[0]["Max8HourO3"].ToString(), out AvgValueMax8HourO3) ? AvgValueMax8HourO3 : 0, Max8HourO3Unit));
                                    dr["DayAvgValue"] = AvgValueMax8HourO3 != 0 ? AvgValueMax8HourO3.ToString() : "";
                                    dr["DataRange"] = MinValueMax8HourO3.ToString() + "~" + MaxValueMax8HourO3.ToString();
                                    dr["OutDays"] = drExceeding[0]["Max8HourO3_Over"];
                                    dr["MonitorDays"] = AreaData.ToTable().Select("MonitoringRegionUid='" + regionGuids[i] + "' and ReportDateTime>='" + dateStart +
                                        "' and ReportDateTime<='" + dateEnd + "' and Max8HourO3_IAQI is not null and Max8HourO3_IAQI<>0").Count();
                                    decimal outMax8HourO3 = 0;
                                    if (Convert.ToDecimal(dr["MonitorDays"]) != 0)
                                    {
                                        outMax8HourO3 = (decimal)(Convert.ToDecimal(dr["OutDays"]) / Convert.ToDecimal(dr["MonitorDays"]) * 100);
                                    }
                                    dr["OutRate"] = DecimalExtension.GetPollutantValue(outMax8HourO3, 1).ToString() + "%";
                                    decimal dMax8HourO3 = (decimal)((Convert.ToDecimal(drMax[0]["Max8HourO3"]) - entity.Upper) / entity.Upper);
                                    string strdMax8HourO3 = "";
                                    if (dMax8HourO3 < 0)
                                    {
                                        strdMax8HourO3 = "/";
                                        dr["OutDate"] = "/";
                                    }
                                    else
                                    {
                                        strdMax8HourO3 = DecimalExtension.GetPollutantValue(dMax8HourO3, 2).ToString();
                                        DataRow[] O3 = AreaData.ToTable().Select("MonitoringRegionUid='" + regionGuids[i] + "' and ReportDateTime>='" + dateStart +
                                                   "' and ReportDateTime<='" + dateEnd + "' and Max8HourO3 like'" + MaxValueMax8HourO3 + "%'");
                                        if (O3.Length > 0)
                                            if (O3[0]["ReportDateTime"].IsNotNullOrDBNull())
                                                dr["OutDate"] = string.Format("{0:yyyy-MM-dd}", O3[0]["ReportDateTime"]);
                                    }
                                    dr["OutBiggestFactor"] = strdMax8HourO3;
                                    decimal YearAverageMax8HourO3 = (DecimalExtension.GetPollutantValue(decimal.TryParse(drAvg[0]["Max8HourO3"].ToString(), out YearAverageMax8HourO3) ? YearAverageMax8HourO3 : 0, Max8HourO3Unit));
                                    //dr["YearAverage"] = YearAverageMax8HourO3 != 0 ? YearAverageMax8HourO3.ToString() : "";
                                    //dr["YearOutRate"] = "/";
                                    if (yearpercentO3 == null || yearpercentO3 == "")
                                    {
                                        dr["YearPercent"] = "/";
                                    }
                                    else
                                    {
                                        dr["YearPercent"] = yearpercentO3;
                                    }

                                    dr["YearPerOutRate"] = "/";
                                    AreaData.RowFilter = "Max8HourO3 is not null and Max8HourO3<>''  and MonitoringRegionUid='" + regionGuids[i] + "'";
                                    List<string> strMax8HourO3 = AreaData.ToTable().AsEnumerable().Select(r => r.Field<string>("Max8HourO3")).ToList();
                                    List<decimal> decMax8HourO3 = new List<decimal>(strMax8HourO3.Select(x => decimal.Parse(x)));
                                    Dictionary<string, string> ListMedianMax8HourO3 = Median(decMax8HourO3, "");
                                    if (ListMedianMax8HourO3.Keys.Contains("median"))
                                    {
                                        dr["MedianValueu"] = ListMedianMax8HourO3["median"];
                                    }
                                    if (ListMedianMax8HourO3.Keys.Contains("median1"))
                                    {
                                        dr["MedianLower"] = ListMedianMax8HourO3["median1"];
                                    }
                                    if (ListMedianMax8HourO3.Keys.Contains("median3"))
                                    {
                                        dr["MedianUpper"] = ListMedianMax8HourO3["median3"];
                                    } if (ListMedianMax8HourO3.Keys.Contains("lower"))
                                    {
                                        dr["lower"] = ListMedianMax8HourO3["lower"];
                                    } if (ListMedianMax8HourO3.Keys.Contains("upper"))
                                    {
                                        dr["upper"] = ListMedianMax8HourO3["upper"];
                                    }
                                    AreaData.RowFilter = string.Empty;
                                    break;
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                }
                return dt.DefaultView;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 区域综合考评
        /// </summary>
        /// <param name="regionGuids">区域Guid</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <param name="orderBy">排序</param>
        /// <returns>
        /// RegionName：区域
        /// </returns>
        public DataView GetComprehensiveData(string[] regionGuids, DateTime dateStart, DateTime dateEnd, string year)
        {
            try
            {
                pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
                string monthB = dateStart.ToString("MM-dd");
                string monthE = dateEnd.ToString("MM-dd");
                DateTime baseBeginTime = Convert.ToDateTime(year + "-" + monthB + " 00:00:00");
                DateTime baseEndTime = Convert.ToDateTime(year + "-" + dateEnd.Month);

                if (baseEndTime.LastDayOfMonth().Day > dateEnd.Day)
                {
                    baseEndTime = Convert.ToDateTime(year + "-" + monthE + " 23:59:59");
                }
                else
                {
                    baseEndTime = baseEndTime.LastDayOfMonth();
                }
                DateTime lastBegin = dateStart.AddYears(-1);
                DateTime lastEnd = dateEnd.AddYears(-1);
                DataView AreaData = new DataView();
                DataView AreaDataL = new DataView();
                DataView AreaDataB = new DataView();
                regionDayAQI = Singleton<RegionDayAQIRepository>.GetInstance();
                DictionaryService dicService = new DictionaryService();
                if (regionDayAQI != null)
                {
                    AreaData = regionDayAQI.GetRegionsAllData(regionGuids, dateStart, dateEnd);
                    AreaDataL = regionDayAQI.GetRegionsAllData(regionGuids, lastBegin, lastEnd);
                    AreaDataB = regionDayAQI.GetRegionsAllData(regionGuids, baseBeginTime, baseEndTime);

                }
                DictionaryService dictionary = new DictionaryService();
                DataTable dt = new DataTable();
                dt.Columns.Add("RegionName", typeof(string));
                dt.Columns.Add(dateEnd.Year + "年PM2.5浓度(微克/立方米)", typeof(string));
                dt.Columns.Add("PM2.5浓度与" + year + "年同期比较(%)", typeof(string));
                dt.Columns.Add("PM25_Status", typeof(string));
                dt.Columns.Add("达标率与" + lastEnd.Year + "年同期比较(百分点)", typeof(string));
                dt.Columns.Add("Dabiao_Status", typeof(string));
                dt.Columns.Add("重污染天数" + lastEnd.Year + "年同期比较(天)", typeof(string));
                dt.Columns.Add("Days_Status", typeof(string));
                dt.Columns.Add("综合考评", typeof(string));
                dt.Columns.Add("Compre_Status", typeof(string));
                dt.Columns.Add("O3百分位(微克/立方米)", typeof(string));
                decimal Value = Convert.ToDecimal(dicService.GetValueByValue(DictionaryType.Air, "PM2.5年度考核目标", year));
                for (int i = 0; i < regionGuids.Length; i++)
                {
                    string RegionName = dictionary.GetCodeDictionaryTextByValue(regionGuids[i]);
                    DataRow[] drYears = AreaData.Table.Select("MonitoringRegionUid='" + regionGuids[i] + "'");
                    DataRow[] drLast = AreaDataL.Table.Select("MonitoringRegionUid='" + regionGuids[i] + "'");
                    DataRow[] drBase = AreaDataB.Table.Select("MonitoringRegionUid='" + regionGuids[i] + "'");
                    string yearpercentO3 = "";
                    yearpercentO3 = pointDayAQI.getpercent(regionGuids[i], dateStart, dateEnd, "a05024");
                    if (drYears.Length > 0 && drLast.Length > 0 && drBase.Length > 0)
                    {
                        DataRow dr = dt.NewRow();
                        dr["RegionName"] = RegionName;
                        decimal YearsValue = DecimalExtension.GetPollutantValue(decimal.TryParse(drYears[0]["a34004"].ToString(), out YearsValue) ? YearsValue * 1000 : 0, 1);
                        dr[dateEnd.Year + "年PM2.5浓度(微克/立方米)"] = YearsValue;
                        decimal BaseValue = DecimalExtension.GetPollutantValue(decimal.TryParse(drBase[0]["a34004"].ToString(), out BaseValue) ? BaseValue * 1000 : 0, 1);
                        if (BaseValue != 0)
                        {
                            dr["PM2.5浓度与" + year + "年同期比较(%)"] = DecimalExtension.GetPollutantValue((YearsValue - BaseValue) / BaseValue * 100, 1);
                            if (Convert.ToDecimal(dr["PM2.5浓度与" + year + "年同期比较(%)"]) > -Value)
                                dr["PM25_Status"] = "N";
                        }
                        decimal ValidCountY = DecimalExtension.GetPollutantValue(decimal.TryParse(drYears[0]["ValidCount"].ToString(), out ValidCountY) ? ValidCountY : 0, 0);
                        decimal StandardCountY = DecimalExtension.GetPollutantValue(decimal.TryParse(drYears[0]["StandardCount"].ToString(), out StandardCountY) ? StandardCountY : 0, 0);
                        decimal ValidCountL = DecimalExtension.GetPollutantValue(decimal.TryParse(drLast[0]["ValidCount"].ToString(), out ValidCountL) ? ValidCountL : 0, 0);
                        decimal StandardCountL = DecimalExtension.GetPollutantValue(decimal.TryParse(drLast[0]["StandardCount"].ToString(), out StandardCountL) ? StandardCountL : 0, 0);
                        decimal RateY = 0;
                        decimal RateL = 0;
                        if (ValidCountY != 0)
                        {
                            RateY = DecimalExtension.GetPollutantValue(StandardCountY / ValidCountY * 100, 1);
                        }
                        if (ValidCountL != 0)
                        {
                            RateL = DecimalExtension.GetPollutantValue(StandardCountL / ValidCountL * 100, 1);
                        }
                        dr["达标率与" + lastEnd.Year + "年同期比较(百分点)"] = RateY - RateL;
                        if (Convert.ToDecimal(dr["达标率与" + lastEnd.Year + "年同期比较(百分点)"]) < 0)
                            dr["Dabiao_Status"] = "N";
                        decimal PollutionY = Convert.ToInt32(drYears[0]["HighPollution"]) + Convert.ToInt32(drYears[0]["SeriousPollution"]);
                        decimal PollutionL = Convert.ToInt32(drLast[0]["HighPollution"]) + Convert.ToInt32(drLast[0]["SeriousPollution"]);
                        dr["重污染天数" + lastEnd.Year + "年同期比较(天)"] = PollutionY - PollutionL;
                        if (Convert.ToDecimal(dr["重污染天数" + lastEnd.Year + "年同期比较(天)"]) > 0)
                            dr["Days_Status"] = "N";

                        if (dr["PM25_Status"].ToString() == "N" || dr["Dabiao_Status"].ToString() == "N" || dr["Days_Status"].ToString() == "N")
                        {
                            dr["Compre_Status"] = "N";
                            dr["综合考评"] = "不合格";
                        }
                        else
                            dr["综合考评"] = "合格";
                        if (yearpercentO3 == null || yearpercentO3 == "")
                        {
                            dr["O3百分位(微克/立方米)"] = "/";
                        }
                        else
                        {
                            dr["O3百分位(微克/立方米)"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(yearpercentO3) * 1000, 0);
                        }
                        dt.Rows.Add(dr);
                    }
                }
                return dt.DefaultView;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 获取指定日期内日数据最大值数据
        /// </summary>
        /// <param name="regionGuids">区域Guid</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>
        /// 返回字段：
        /// MonitoringRegionUid：区域ID
        /// PM25：PM25浓度
        /// PM10：PM10浓度
        /// NO2：NO2浓度
        /// SO2：SO2浓度
        /// CO：CO浓度
        /// MaxOneHourO3：MaxOneHourO3浓度
        /// Max8HourO3：Max8HourO3浓度
        /// PM25_IAQI：PM25指数
        /// PM10_IAQI：PM10指数
        /// NO2_IAQI：NO2指数
        /// SO2_IAQI：SO2指数
        /// CO_IAQI：CO指数
        /// MaxOneHourO3_IAQI：MaxOneHourO3指数
        /// Max8HourO3_IAQI：Max8HourO3指数
        /// AQIValue：空气质量指数
        /// </returns>
        public DataView GetRegionsMaxValue(string[] regionGuids, DateTime dateStart, DateTime dateEnd)
        {
            regionDayAQI = Singleton<RegionDayAQIRepository>.GetInstance();
            if (regionDayAQI != null)
                return regionDayAQI.GetMaxValue(regionGuids, dateStart, dateEnd);
            return null;
        }
        /// <summary>
        /// 获取指定日期内日数据最大值数据
        /// </summary>
        /// <param name="regionGuids">区域Guid</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>
        /// 返回字段：
        /// MonitoringRegionUid：区域ID
        /// PM25：PM25浓度
        /// PM10：PM10浓度
        /// NO2：NO2浓度
        /// SO2：SO2浓度
        /// CO：CO浓度
        /// MaxOneHourO3：MaxOneHourO3浓度
        /// Max8HourO3：Max8HourO3浓度
        /// PM25_IAQI：PM25指数
        /// PM10_IAQI：PM10指数
        /// NO2_IAQI：NO2指数
        /// SO2_IAQI：SO2指数
        /// CO_IAQI：CO指数
        /// MaxOneHourO3_IAQI：MaxOneHourO3指数
        /// Max8HourO3_IAQI：Max8HourO3指数
        /// AQIValue：空气质量指数
        /// </returns>
        public DataView GetRegionsMaxValueOne(string[] regionGuids, DateTime dateStart, DateTime dateEnd)
        {
            regionDayAQI = Singleton<RegionDayAQIRepository>.GetInstance();
            if (regionDayAQI != null)
                return regionDayAQI.GetMaxValueOne(regionGuids, dateStart, dateEnd);
            return null;
        }

        /// <summary>
        /// 获取指定日期内日数据最小值数据
        /// </summary>
        /// <param name="regionGuids">区域Guid</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>
        /// 返回字段：
        /// MonitoringRegionUid：区域ID
        /// PM25：PM25浓度
        /// PM10：PM10浓度
        /// NO2：NO2浓度
        /// SO2：SO2浓度
        /// CO：CO浓度
        /// MaxOneHourO3：MaxOneHourO3浓度
        /// Max8HourO3：Max8HourO3浓度
        /// PM25_IAQI：PM25指数
        /// PM10_IAQI：PM10指数
        /// NO2_IAQI：NO2指数
        /// SO2_IAQI：SO2指数
        /// CO_IAQI：CO指数
        /// MaxOneHourO3_IAQI：MaxOneHourO3指数
        /// Max8HourO3_IAQI：Max8HourO3指数
        /// AQIValue：空气质量指数
        /// </returns>
        public DataView GetRegionsMinValue(string[] regionGuids, DateTime dateStart, DateTime dateEnd)
        {
            regionDayAQI = Singleton<RegionDayAQIRepository>.GetInstance();
            if (regionDayAQI != null)
                return regionDayAQI.GetMinValue(regionGuids, dateStart, dateEnd);
            return null;
        }
        /// <summary>
        /// 获取指定日期内日数据最小值数据
        /// </summary>
        /// <param name="regionGuids">区域Guid</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>
        /// 返回字段：
        /// MonitoringRegionUid：区域ID
        /// PM25：PM25浓度
        /// PM10：PM10浓度
        /// NO2：NO2浓度
        /// SO2：SO2浓度
        /// CO：CO浓度
        /// MaxOneHourO3：MaxOneHourO3浓度
        /// Max8HourO3：Max8HourO3浓度
        /// PM25_IAQI：PM25指数
        /// PM10_IAQI：PM10指数
        /// NO2_IAQI：NO2指数
        /// SO2_IAQI：SO2指数
        /// CO_IAQI：CO指数
        /// MaxOneHourO3_IAQI：MaxOneHourO3指数
        /// Max8HourO3_IAQI：Max8HourO3指数
        /// AQIValue：空气质量指数
        /// </returns>
        public DataView GetRegionsMinValueOne(string[] regionGuids, DateTime dateStart, DateTime dateEnd)
        {
            regionDayAQI = Singleton<RegionDayAQIRepository>.GetInstance();
            if (regionDayAQI != null)
                return regionDayAQI.GetMinValueOne(regionGuids, dateStart, dateEnd);
            return null;
        }
        /// <summary>
        /// 获取指定日期内日数据均值数据
        /// </summary>
        /// <param name="regionGuids">区域Guid</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>
        /// 返回字段：
        /// MonitoringRegionUid：区域ID
        /// PM25：PM25浓度
        /// PM10：PM10浓度
        /// NO2：NO2浓度
        /// SO2：SO2浓度
        /// CO：CO浓度
        /// MaxOneHourO3：MaxOneHourO3浓度
        /// Max8HourO3：Max8HourO3浓度
        /// </returns>
        public DataView GetRegionsAvgValue(string[] regionGuids, DateTime dateStart, DateTime dateEnd)
        {
            regionDayAQI = Singleton<RegionDayAQIRepository>.GetInstance();
            if (regionDayAQI != null)
                return regionDayAQI.GetAvgValue(regionGuids, dateStart, dateEnd);
            return null;
        }

        /// <summary>
        /// 区域日数据超标天数统计
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <returns>
        /// 返回字段：
        /// MonitoringRegionUid：站点ID
        /// ValidCount：有效天
        /// OverCount：超标天
        /// PM25_Over：PM25超标天
        /// PM10_Over：PM10超标天
        /// NO2_Over：NO2超标天
        /// SO2_Over：SO2超标天
        /// CO_Over：CO超标天
        /// MaxOneHourO3_Over：MaxOneHourO3超标天
        /// Max8HourO3_Over：Max8HourO3超标天
        /// </returns>
        public DataView GetRegionsExceedingData(string[] regionGuids, DateTime dateStart, DateTime dateEnd)
        {
            regionDayAQI = Singleton<RegionDayAQIRepository>.GetInstance();
            if (regionDayAQI != null)
                return regionDayAQI.GetExceedingData(regionGuids, dateStart, dateEnd);
            return null;
        }

        /// <summary>
        /// 获取主要污染物年均值
        /// </summary>
        /// <param name="regionGuids">区域Guid</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>
        /// RegionName：区域名称
        /// Year：年份
        /// PM25Concentration：PM2.5浓度
        /// PM10Concentration：PM10浓度
        /// NO2Concentration：二氧化氮浓度
        /// SO2Concentration：二氧化硫浓度
        /// COConcentration：一氧化碳浓度
        /// Max8HourO3Concentration：最大8小时臭氧浓度
        /// PM25LimitValue：PM2.5年均浓度限值
        /// PM10LimitValue：PM10年均浓度限值
        /// NO2LimitValue：二氧化氮年均浓度限值
        /// SO2LimitValue：二氧化硫年均浓度限值
        /// COLimitValue：一氧化碳年均浓度限值
        /// Max8HourO3LimitValue：最大8小时臭氧年均浓度限值
        /// </returns>
        public DataView GetPrimaryPollutantAvgYearData(string[] regionGuids, string[] factors, DateTime dateStart, DateTime dateEnd)
        {

            pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
            EQIConcentrationService EQIConcentration = new EQIConcentrationService();
            DictionaryService dictionary = new DictionaryService();
            DataTable dt = new DataTable();
            string factorCode = "";
            dt.Columns.Add("RegionName", typeof(string));
            dt.Columns.Add("Year", typeof(string));
            foreach (string factor in factors)
            {
                dt.Columns.Add(factor + "Concentration", typeof(string));
                dt.Columns.Add(factor + "YearPercent", typeof(string));
                dt.Columns.Add(factor + "SI", typeof(string));
                dt.Columns.Add(factor + "LimitValue", typeof(string));
                if (factor == "PM25")
                    factorCode += ("a34004" + ",");
                if (factor == "PM10")
                    factorCode += ("a34002" + ",");
                if (factor == "NO2")
                    factorCode += ("a21004" + ",");
                if (factor == "SO2")
                    factorCode += ("a21026" + ",");
                if (factor == "CO")
                    factorCode += ("a21005" + ",");
                if (factor == "Max8HourO3")
                    factorCode += ("a05024" + ",");

            }
            dt.Columns.Add("CPI", typeof(string));
            string[] factorCodes = factorCode.Trim(',').Split(',');
            try
            {
                for (int j = 0; j < regionGuids.Length; j++)
                {
                    string RegionName = dictionary.GetCodeDictionaryTextByValue(regionGuids[j]);
                    for (int i = 0; i <= dateEnd.Year - dateStart.Year; i++)
                    {
                        DataView AvgValues = GetRegionsAvgValue(regionGuids, dateStart.AddYears(+i), dateStart.AddYears(+i + 1).AddMilliseconds(-1));
                        DataRow[] drAvg = AvgValues.Table.Select("MonitoringRegionUid='" + regionGuids[j] + "'");
                        if (drAvg.Length > 0)
                        {
                            DataRow dr = dt.NewRow();
                            dr["RegionName"] = RegionName;
                            dr["Year"] = dateStart.AddYears(+i).Year;

                            foreach (string factor in factors)
                            {
                                switch (factor)
                                {
                                    case "PM25":
                                        dr["PM25Concentration"] = drAvg[0]["PM25"];
                                        string yearpercentPM25 = "";
                                        yearpercentPM25 = pointDayAQI.getpercent(regionGuids[j], dateStart, dateEnd, "a34004");
                                        if (yearpercentPM25 == null || yearpercentPM25 == "")
                                        {
                                            dr["PM25YearPercent"] = "/";
                                        }
                                        else
                                        {
                                            dr["PM25YearPercent"] = yearpercentPM25;
                                        }
                                        string[] pm25 = { "a34004" };
                                        dr["PM25SI"] = pointDayAQI.getSI(regionGuids[j], dateStart.AddYears(+i), dateStart.AddYears(+i + 1).AddMilliseconds(-1), pm25);
                                        dr["PM25LimitValue"] = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a34004", EQITimeType.TwentyFour).Upper.ToString();
                                        break;
                                    case "PM10":
                                        dr["PM10Concentration"] = drAvg[0]["PM10"];
                                        string yearpercentPM10 = "";
                                        yearpercentPM10 = pointDayAQI.getpercent(regionGuids[j], dateStart, dateEnd, "a34002");
                                        if (yearpercentPM10 == null || yearpercentPM10 == "")
                                        {
                                            dr["PM10YearPercent"] = "/";
                                        }
                                        else
                                        {
                                            dr["PM10YearPercent"] = yearpercentPM10;
                                        }
                                        string[] pm10 = { "a34002" };
                                        dr["PM10SI"] = pointDayAQI.getSI(regionGuids[j], dateStart.AddYears(+i), dateStart.AddYears(+i + 1).AddMilliseconds(-1), pm10);
                                        dr["PM10LimitValue"] = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a34002", EQITimeType.TwentyFour).Upper.ToString();
                                        break;
                                    case "NO2":
                                        dr["NO2Concentration"] = drAvg[0]["NO2"];
                                        string yearpercentNO2 = "";
                                        yearpercentNO2 = pointDayAQI.getpercent(regionGuids[j], dateStart, dateEnd, "a21004");
                                        if (yearpercentNO2 == null || yearpercentNO2 == "")
                                        {
                                            dr["NO2YearPercent"] = "/";
                                        }
                                        else
                                        {
                                            dr["NO2YearPercent"] = yearpercentNO2;
                                        }
                                        string[] no2 = { "a21004" };
                                        dr["NO2SI"] = pointDayAQI.getSI(regionGuids[j], dateStart.AddYears(+i), dateStart.AddYears(+i + 1).AddMilliseconds(-1), no2);
                                        dr["NO2LimitValue"] = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a21004", EQITimeType.TwentyFour).Upper.ToString();
                                        break;
                                    case "SO2":
                                        dr["SO2Concentration"] = drAvg[0]["SO2"];
                                        string yearpercentSO2 = "";
                                        yearpercentSO2 = pointDayAQI.getpercent(regionGuids[j], dateStart, dateEnd, "a21026");
                                        if (yearpercentSO2 == null || yearpercentSO2 == "")
                                        {
                                            dr["SO2YearPercent"] = "/";
                                        }
                                        else
                                        {
                                            dr["SO2YearPercent"] = yearpercentSO2;
                                        }
                                        string[] so2 = { "a21026" };
                                        dr["SO2SI"] = pointDayAQI.getSI(regionGuids[j], dateStart.AddYears(+i), dateStart.AddYears(+i + 1).AddMilliseconds(-1), so2);
                                        dr["SO2LimitValue"] = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a21026", EQITimeType.TwentyFour).Upper.ToString();
                                        break;
                                    case "CO":
                                        dr["COConcentration"] = drAvg[0]["CO"];
                                        string yearpercentCO = "";
                                        yearpercentCO = pointDayAQI.getpercent(regionGuids[j], dateStart, dateEnd, "a21005");
                                        if (yearpercentCO == null || yearpercentCO == "")
                                        {
                                            dr["COYearPercent"] = "/";
                                        }
                                        else
                                        {
                                            dr["COYearPercent"] = yearpercentCO;
                                        }
                                        string[] co = { "a21005" };
                                        dr["COSI"] = pointDayAQI.getSI(regionGuids[j], dateStart.AddYears(+i), dateStart.AddYears(+i + 1).AddMilliseconds(-1), co);
                                        dr["COLimitValue"] = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a21005", EQITimeType.TwentyFour).Upper.ToString();
                                        break;
                                    case "Max8HourO3":
                                        dr["Max8HourO3Concentration"] = drAvg[0]["Max8HourO3"];
                                        string yearpercentO3 = "";
                                        yearpercentO3 = pointDayAQI.getpercent(regionGuids[j], dateStart, dateEnd, "a05024");
                                        if (yearpercentO3 == null || yearpercentO3 == "")
                                        {
                                            dr["Max8HourO3YearPercent"] = "/";
                                        }
                                        else
                                        {
                                            dr["Max8HourO3YearPercent"] = yearpercentO3;
                                        }
                                        string[] Max8HourO3 = { "a05024" };
                                        dr["Max8HourO3SI"] = pointDayAQI.getSI(regionGuids[j], dateStart.AddYears(+i), dateStart.AddYears(+i + 1).AddMilliseconds(-1), Max8HourO3);
                                        dr["Max8HourO3LimitValue"] = EQIConcentration.RetrieveAirConcentration(AQIClass.Moderate, "a05024", EQITimeType.Eight).Upper.ToString();
                                        break;
                                }
                            }
                            dr["CPI"] = pointDayAQI.getSI(regionGuids[j], dateStart.AddYears(+i), dateStart.AddYears(+i + 1).AddMilliseconds(-1), factorCodes);
                            dt.Rows.Add(dr);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return dt.DefaultView;
            }
            return dt.DefaultView;
        }

        /// <summary>
        /// 各项污染物日均值浓度范围
        /// </summary>
        /// <param name="regionGuids">区域Guid</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>
        /// RegionName：区域名称
        /// DateTime：日期
        /// PM25Range：PM2.5浓度
        /// PM10Range：PM10浓度
        /// NO2Range：NO2浓度
        /// SO2Range：SO2浓度
        /// CORange：CO浓度
        /// Max8HourO3Range：臭氧8小时浓度
        /// MaxOneHourO3Range：臭氧1小时浓度
        /// PM25AQI：PM2.5分指数
        /// PM10AQI：PM10分指数
        /// NO2AQI：NO2分指数
        /// SO2AQI：SO2分指数
        /// COAQI：CO分指数
        /// Max8HourO3AQI：臭氧8小时分指数
        /// MaxOneHourO3AQ：臭氧1小时分指数
        /// </returns>
        public DataView GetRegionPollutantDayRange(string[] regionGuids, string[] factors, DateTime dateStart, DateTime dateEnd)
        {
            DictionaryService dictionary = new DictionaryService();
            DataTable dt = new DataTable();
            dt.Columns.Add("RegionId", typeof(string));
            dt.Columns.Add("RegionName", typeof(string));
            // dt.Columns.Add("DateTime", typeof(DateTime));
            foreach (string factor in factors)
            {
                dt.Columns.Add(factor + "Range", typeof(string));
            }
            dt.Columns.Add("AQI", typeof(string));

            for (int i = 0; i < regionGuids.Length; i++)
            {
                string RegionName = dictionary.GetCodeDictionaryTextByValue(regionGuids[i]);
                string RegionId = regionGuids[i];

                DataView MinValues = GetRegionsMinValue(regionGuids, dateStart, dateEnd);
                DataView MaxValues = GetRegionsMaxValue(regionGuids, dateStart, dateEnd);

                DataRow dr = dt.NewRow();
                DataRow[] drMin = MinValues.Table.Select("MonitoringRegionUid='" + regionGuids[i] + "'");
                DataRow[] drMax = MaxValues.Table.Select("MonitoringRegionUid='" + regionGuids[i] + "'");
                if (drMin.Length > 0 && drMax.Length > 0)
                {
                    dr["RegionId"] = RegionId;
                    dr["RegionName"] = RegionName;
                    // dr["DateTime"] = (string.Format("{0:yyyy-MM}", DateTime.Parse((dateStart.AddMonths(j).Year + "-" + dateStart.AddMonths(j).Month).ToString())));

                    foreach (string factor in factors)
                    {
                        switch (factor)
                        {
                            case "PM25":
                                if (drMin[0]["PM25"].IsNotNullOrDBNull())
                                {
                                    if (drMax[0]["PM25"].IsNotNullOrDBNull())
                                    {
                                        decimal pm25ntemp = decimal.TryParse(drMin[0]["PM25"].ToString(), out pm25ntemp) ? pm25ntemp : 0;
                                        decimal pm25mtemp = decimal.TryParse(drMax[0]["PM25"].ToString(), out pm25mtemp) ? pm25mtemp : 0;
                                        dr["PM25Range"] = (pm25ntemp * 1000).ToString("0") + "~" + (pm25mtemp * 1000).ToString("0");
                                    }
                                    else
                                    {
                                        decimal pm25ntemp = decimal.TryParse(drMin[0]["PM25"].ToString(), out pm25ntemp) ? pm25ntemp : 0;
                                        dr["PM25Range"] = (pm25ntemp * 1000).ToString("0") + "~" + "/";
                                    }
                                }
                                else if (!drMin[0]["PM25"].IsNotNullOrDBNull())
                                {
                                    if (drMax[0]["PM25"].IsNotNullOrDBNull())
                                    {
                                        decimal pm25mtemp = decimal.TryParse(drMax[0]["PM25"].ToString(), out pm25mtemp) ? pm25mtemp : 0;
                                        dr["PM25Range"] = "/" + "~" + (pm25mtemp * 1000).ToString("0");
                                    }
                                    else
                                    {
                                        dr["PM25Range"] = "/";
                                    }
                                }
                                break;
                            case "PM10":
                                if (drMin[0]["PM10"].IsNotNullOrDBNull())
                                {
                                    if (drMax[0]["PM10"].IsNotNullOrDBNull())
                                    {
                                        decimal pm10ntemp = decimal.TryParse(drMin[0]["PM10"].ToString(), out pm10ntemp) ? pm10ntemp : 0;
                                        decimal pm10mtemp = decimal.TryParse(drMax[0]["PM10"].ToString(), out pm10mtemp) ? pm10mtemp : 0;
                                        dr["PM10Range"] = (pm10ntemp * 1000).ToString("0") + "~" + (pm10mtemp * 1000).ToString("0");
                                    }
                                    else
                                    {
                                        decimal pm10ntemp = decimal.TryParse(drMin[0]["PM10"].ToString(), out pm10ntemp) ? pm10ntemp : 0;
                                        dr["PM10Range"] = (pm10ntemp * 1000).ToString("0") + "~" + "/";
                                    }
                                }
                                else if (!drMin[0]["PM10"].IsNotNullOrDBNull())
                                {
                                    if (drMax[0]["PM10"].IsNotNullOrDBNull())
                                    {
                                        decimal pm10mtemp = decimal.TryParse(drMax[0]["PM10"].ToString(), out pm10mtemp) ? pm10mtemp : 0;
                                        dr["PM10Range"] = "/" + "~" + (pm10mtemp * 1000).ToString("0");
                                    }
                                    else
                                    {
                                        dr["PM10Range"] = "/";
                                    }
                                }
                                break;
                            case "NO2":
                                if (drMin[0]["NO2"].IsNotNullOrDBNull())
                                {
                                    if (drMax[0]["NO2"].IsNotNullOrDBNull())
                                    {
                                        decimal no2ntemp = decimal.TryParse(drMin[0]["NO2"].ToString(), out no2ntemp) ? no2ntemp : 0;
                                        decimal no2mtemp = decimal.TryParse(drMax[0]["NO2"].ToString(), out no2mtemp) ? no2mtemp : 0;
                                        dr["NO2Range"] = (no2ntemp * 1000).ToString("0") + "~" + (no2mtemp * 1000).ToString("0");
                                    }
                                    else
                                    {
                                        decimal no2ntemp = decimal.TryParse(drMin[0]["NO2"].ToString(), out no2ntemp) ? no2ntemp : 0;
                                        dr["NO2Range"] = (no2ntemp * 1000).ToString("0") + "~" + "/";
                                    }
                                }
                                else if (!drMin[0]["NO2"].IsNotNullOrDBNull())
                                {
                                    if (drMax[0]["NO2"].IsNotNullOrDBNull())
                                    {
                                        decimal no2mtemp = decimal.TryParse(drMax[0]["NO2"].ToString(), out no2mtemp) ? no2mtemp : 0;
                                        dr["NO2Range"] = "/" + "~" + (no2mtemp * 1000).ToString("0");
                                    }
                                    else
                                    {
                                        dr["NO2Range"] = "/";
                                    }
                                }
                                break;
                            case "SO2":
                                if (drMin[0]["SO2"].IsNotNullOrDBNull())
                                {
                                    if (drMax[0]["SO2"].IsNotNullOrDBNull())
                                    {
                                        decimal so2ntemp = decimal.TryParse(drMin[0]["SO2"].ToString(), out so2ntemp) ? so2ntemp : 0;
                                        decimal so2mtemp = decimal.TryParse(drMax[0]["SO2"].ToString(), out so2mtemp) ? so2mtemp : 0;
                                        dr["SO2Range"] = (so2ntemp * 1000).ToString("0") + "~" + (so2mtemp * 1000).ToString("0");
                                    }
                                    else
                                    {
                                        decimal so2ntemp = decimal.TryParse(drMin[0]["SO2"].ToString(), out so2ntemp) ? so2ntemp : 0;
                                        dr["SO2Range"] = (so2ntemp * 1000).ToString("0") + "~" + "/";
                                    }
                                }
                                else if (!drMin[0]["SO2"].IsNotNullOrDBNull())
                                {
                                    if (drMax[0]["SO2"].IsNotNullOrDBNull())
                                    {
                                        decimal so2mtemp = decimal.TryParse(drMax[0]["SO2"].ToString(), out so2mtemp) ? so2mtemp : 0;
                                        dr["SO2Range"] = "/" + "~" + (so2mtemp * 1000).ToString("0");
                                    }
                                    else
                                    {
                                        dr["SO2Range"] = "/";
                                    }
                                }
                                break;
                            case "CO":
                                if (drMin[0]["CO"].IsNotNullOrDBNull())
                                {
                                    if (drMax[0]["CO"].IsNotNullOrDBNull())
                                    {
                                        decimal corntemp = decimal.TryParse(drMin[0]["CO"].ToString(), out corntemp) ? corntemp : 0;
                                        decimal cormtemp = decimal.TryParse(drMax[0]["CO"].ToString(), out cormtemp) ? cormtemp : 0;
                                        dr["CORange"] = corntemp.ToString("0.0") + "~" + cormtemp.ToString("0.0");
                                    }
                                    else
                                    {
                                        decimal corntemp = decimal.TryParse(drMin[0]["CO"].ToString(), out corntemp) ? corntemp : 0;
                                        dr["CORange"] = corntemp.ToString("0.0") + "~" + "/";
                                    }
                                }
                                else if (!drMin[0]["CO"].IsNotNullOrDBNull())
                                {
                                    if (drMax[0]["CO"].IsNotNullOrDBNull())
                                    {
                                        decimal cormtemp = decimal.TryParse(drMax[0]["CO"].ToString(), out cormtemp) ? cormtemp : 0;
                                        dr["CORange"] = "/" + "~" + cormtemp.ToString("0.0");
                                    }
                                    else
                                    {
                                        dr["CORange"] = "/";
                                    }
                                }
                                break;
                            case "Max8HourO3":
                                if (drMin[0]["Max8HourO3"].IsNotNullOrDBNull())
                                {
                                    if (drMax[0]["Max8HourO3"].IsNotNullOrDBNull())
                                    {
                                        decimal maxntemp = decimal.TryParse(drMin[0]["Max8HourO3"].ToString(), out maxntemp) ? maxntemp : 0;
                                        decimal maxmtemp = decimal.TryParse(drMax[0]["Max8HourO3"].ToString(), out maxmtemp) ? maxmtemp : 0;
                                        dr["Max8HourO3Range"] = (maxntemp * 1000).ToString("0") + "~" + (maxmtemp * 1000).ToString("0");
                                    }
                                    else
                                    {
                                        decimal maxntemp = decimal.TryParse(drMin[0]["Max8HourO3"].ToString(), out maxntemp) ? maxntemp : 0;
                                        dr["Max8HourO3Range"] = (maxntemp * 1000).ToString("0") + "~" + "/";
                                    }
                                }
                                else if (!drMin[0]["Max8HourO3"].IsNotNullOrDBNull())
                                {
                                    if (drMax[0]["Max8HourO3"].IsNotNullOrDBNull())
                                    {
                                        decimal maxmtemp = decimal.TryParse(drMax[0]["Max8HourO3"].ToString(), out maxmtemp) ? maxmtemp : 0;
                                        dr["Max8HourO3Range"] = "/" + "~" + (maxmtemp * 1000).ToString("0");
                                    }
                                    else
                                    {
                                        dr["Max8HourO3Range"] = "/";
                                    }
                                }
                                break;
                        }
                    }
                    if (drMin[0]["AQIValue"].IsNotNullOrDBNull())
                    {
                        if (drMax[0]["AQIValue"].IsNotNullOrDBNull())
                        {
                            dr["AQI"] = Convert.ToDecimal(drMin[0]["AQIValue"]).ToString("0") + "~" + Convert.ToDecimal(drMax[0]["AQIValue"]).ToString("0");
                        }
                        else
                        {
                            dr["AQI"] = Convert.ToDecimal(drMin[0]["AQIValue"]).ToString("0") + "~/";
                        }
                    }
                    else if (!drMin[0]["AQIValue"].IsNotNullOrDBNull())
                    {
                        if (drMax[0]["AQIValue"].IsNotNullOrDBNull())
                        {
                            dr["AQI"] = "/" + "~" + Convert.ToDecimal(drMax[0]["AQIValue"]).ToString("0");
                        }
                        else
                        {
                            dr["AQI"] = "/";
                        }
                    }
                    dt.Rows.Add(dr);
                }
            }
            return dt.AsDataView();
        }
        /// <summary>
        /// 各项污染物日均值浓度范围
        /// </summary>
        /// <param name="regionGuids">区域Guid</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>
        /// RegionName：区域名称
        /// DateTime：日期
        /// PM25Range：PM2.5浓度
        /// PM10Range：PM10浓度
        /// NO2Range：NO2浓度
        /// SO2Range：SO2浓度
        /// CORange：CO浓度
        /// Max8HourO3Range：臭氧8小时浓度
        /// MaxOneHourO3Range：臭氧1小时浓度
        /// PM25AQI：PM2.5分指数
        /// PM10AQI：PM10分指数
        /// NO2AQI：NO2分指数
        /// SO2AQI：SO2分指数
        /// COAQI：CO分指数
        /// Max8HourO3AQI：臭氧8小时分指数
        /// MaxOneHourO3AQ：臭氧1小时分指数
        /// </returns>
        public DataView GetRegionStatisticalRange(string[] regionGuids, string[] factors, DateTime dateStart, DateTime dateEnd)
        {
            DictionaryService dictionary = new DictionaryService();
            DataTable dt = new DataTable();
            dt.Columns.Add("RegionName", typeof(string));
            foreach (string factor in factors)
            {
                dt.Columns.Add(factor + "Range", typeof(string));
            }
            string RegionName = dictionary.GetCodeDictionaryTextByValue(regionGuids[0]);
            string RegionNames = "";
            for (int i = 0; i < regionGuids.Length; i++)
            {
                RegionNames = dictionary.GetCodeDictionaryTextByValue(regionGuids[i]);
            }
            DataView MinValues = GetRegionsMinValueOne(regionGuids, dateStart, dateEnd);
            DataView MaxValues = GetRegionsMaxValueOne(regionGuids, dateStart, dateEnd);

            DataRow dr = dt.NewRow();
            DataRow[] drMin = MinValues.Table.Select();
            DataRow[] drMax = MaxValues.Table.Select();
            if (drMin.Length > 0 && drMax.Length > 0)
            {
                dr["RegionName"] = RegionName + "~" + RegionNames;
                // dr["DateTime"] = (string.Format("{0:yyyy-MM}", DateTime.Parse((dateStart.AddMonths(j).Year + "-" + dateStart.AddMonths(j).Month).ToString())));

                foreach (string factor in factors)
                {
                    switch (factor)
                    {
                        case "PM25":
                            if (drMin[0]["PM25"].IsNotNullOrDBNull())
                            {
                                if (drMax[0]["PM25"].IsNotNullOrDBNull())
                                {
                                    decimal pm25ntemp = decimal.TryParse(drMin[0]["PM25"].ToString(), out pm25ntemp) ? pm25ntemp : 0;
                                    decimal pm25mtemp = decimal.TryParse(drMax[0]["PM25"].ToString(), out pm25mtemp) ? pm25mtemp : 0;
                                    dr["PM25Range"] = (pm25ntemp * 1000).ToString("0") + "~" + (pm25mtemp * 1000).ToString("0");
                                }
                                else
                                {
                                    decimal pm25ntemp = decimal.TryParse(drMin[0]["PM25"].ToString(), out pm25ntemp) ? pm25ntemp : 0;
                                    dr["PM25Range"] = (pm25ntemp * 1000).ToString("0") + "~" + "/";
                                }
                            }
                            else if (!drMin[0]["PM25"].IsNotNullOrDBNull())
                            {
                                if (drMax[0]["PM25"].IsNotNullOrDBNull())
                                {
                                    decimal pm25mtemp = decimal.TryParse(drMax[0]["PM25"].ToString(), out pm25mtemp) ? pm25mtemp : 0;
                                    dr["PM25Range"] = "/" + "~" + (pm25mtemp * 1000).ToString("0");
                                }
                                else
                                {
                                    dr["PM25Range"] = "/";
                                }
                            }
                            break;
                        case "PM10":
                            if (drMin[0]["PM10"].IsNotNullOrDBNull())
                            {
                                if (drMax[0]["PM10"].IsNotNullOrDBNull())
                                {
                                    decimal pm10ntemp = decimal.TryParse(drMin[0]["PM10"].ToString(), out pm10ntemp) ? pm10ntemp : 0;
                                    decimal pm10mtemp = decimal.TryParse(drMax[0]["PM10"].ToString(), out pm10mtemp) ? pm10mtemp : 0;
                                    dr["PM10Range"] = (pm10ntemp * 1000).ToString("0") + "~" + (pm10mtemp * 1000).ToString("0");
                                }
                                else
                                {
                                    decimal pm10ntemp = decimal.TryParse(drMin[0]["PM10"].ToString(), out pm10ntemp) ? pm10ntemp : 0;
                                    dr["PM10Range"] = (pm10ntemp * 1000).ToString("0") + "~" + "/";
                                }
                            }
                            else if (!drMin[0]["PM10"].IsNotNullOrDBNull())
                            {
                                if (drMax[0]["PM10"].IsNotNullOrDBNull())
                                {
                                    decimal pm10mtemp = decimal.TryParse(drMax[0]["PM10"].ToString(), out pm10mtemp) ? pm10mtemp : 0;
                                    dr["PM10Range"] = "/" + "~" + (pm10mtemp * 1000).ToString("0");
                                }
                                else
                                {
                                    dr["PM10Range"] = "/";
                                }
                            }
                            break;
                        case "NO2":
                            if (drMin[0]["NO2"].IsNotNullOrDBNull())
                            {
                                if (drMax[0]["NO2"].IsNotNullOrDBNull())
                                {
                                    decimal no2ntemp = decimal.TryParse(drMin[0]["NO2"].ToString(), out no2ntemp) ? no2ntemp : 0;
                                    decimal no2mtemp = decimal.TryParse(drMax[0]["NO2"].ToString(), out no2mtemp) ? no2mtemp : 0;
                                    dr["NO2Range"] = (no2ntemp * 1000).ToString("0") + "~" + (no2mtemp * 1000).ToString("0");
                                }
                                else
                                {
                                    decimal no2ntemp = decimal.TryParse(drMin[0]["NO2"].ToString(), out no2ntemp) ? no2ntemp : 0;
                                    dr["NO2Range"] = (no2ntemp * 1000).ToString("0") + "~" + "/";
                                }
                            }
                            else if (!drMin[0]["NO2"].IsNotNullOrDBNull())
                            {
                                if (drMax[0]["NO2"].IsNotNullOrDBNull())
                                {
                                    decimal no2mtemp = decimal.TryParse(drMax[0]["NO2"].ToString(), out no2mtemp) ? no2mtemp : 0;
                                    dr["NO2Range"] = "/" + "~" + (no2mtemp * 1000).ToString("0");
                                }
                                else
                                {
                                    dr["NO2Range"] = "/";
                                }
                            }
                            break;
                        case "SO2":
                            if (drMin[0]["SO2"].IsNotNullOrDBNull())
                            {
                                if (drMax[0]["SO2"].IsNotNullOrDBNull())
                                {
                                    decimal so2ntemp = decimal.TryParse(drMin[0]["SO2"].ToString(), out so2ntemp) ? so2ntemp : 0;
                                    decimal so2mtemp = decimal.TryParse(drMax[0]["SO2"].ToString(), out so2mtemp) ? so2mtemp : 0;
                                    dr["SO2Range"] = (so2ntemp * 1000).ToString("0") + "~" + (so2mtemp * 1000).ToString("0");
                                }
                                else
                                {
                                    decimal so2ntemp = decimal.TryParse(drMin[0]["SO2"].ToString(), out so2ntemp) ? so2ntemp : 0;
                                    dr["SO2Range"] = (so2ntemp * 1000).ToString("0") + "~" + "/";
                                }
                            }
                            else if (!drMin[0]["SO2"].IsNotNullOrDBNull())
                            {
                                if (drMax[0]["SO2"].IsNotNullOrDBNull())
                                {
                                    decimal so2mtemp = decimal.TryParse(drMax[0]["SO2"].ToString(), out so2mtemp) ? so2mtemp : 0;
                                    dr["SO2Range"] = "/" + "~" + (so2mtemp * 1000).ToString("0");
                                }
                                else
                                {
                                    dr["SO2Range"] = "/";
                                }
                            }
                            break;
                        case "CO":
                            if (drMin[0]["CO"].IsNotNullOrDBNull())
                            {
                                if (drMax[0]["CO"].IsNotNullOrDBNull())
                                {
                                    decimal corntemp = decimal.TryParse(drMin[0]["CO"].ToString(), out corntemp) ? corntemp : 0;
                                    decimal cormtemp = decimal.TryParse(drMax[0]["CO"].ToString(), out cormtemp) ? cormtemp : 0;
                                    dr["CORange"] = corntemp.ToString("0.0") + "~" + cormtemp.ToString("0.0");
                                }
                                else
                                {
                                    decimal corntemp = decimal.TryParse(drMin[0]["CO"].ToString(), out corntemp) ? corntemp : 0;
                                    dr["CORange"] = corntemp.ToString("0.0") + "~" + "/";
                                }
                            }
                            else if (!drMin[0]["CO"].IsNotNullOrDBNull())
                            {
                                if (drMax[0]["CO"].IsNotNullOrDBNull())
                                {
                                    decimal cormtemp = decimal.TryParse(drMax[0]["CO"].ToString(), out cormtemp) ? cormtemp : 0;
                                    dr["CORange"] = "/" + "~" + cormtemp.ToString("0.0");
                                }
                                else
                                {
                                    dr["CORange"] = "/";
                                }
                            }
                            break;
                        case "Max8HourO3":
                            if (drMin[0]["Max8HourO3"].IsNotNullOrDBNull())
                            {
                                if (drMax[0]["Max8HourO3"].IsNotNullOrDBNull())
                                {
                                    decimal maxntemp = decimal.TryParse(drMin[0]["Max8HourO3"].ToString(), out maxntemp) ? maxntemp : 0;
                                    decimal maxmtemp = decimal.TryParse(drMax[0]["Max8HourO3"].ToString(), out maxmtemp) ? maxmtemp : 0;
                                    dr["Max8HourO3Range"] = (maxntemp * 1000).ToString("0") + "~" + (maxmtemp * 1000).ToString("0");
                                }
                                else
                                {
                                    decimal maxntemp = decimal.TryParse(drMin[0]["Max8HourO3"].ToString(), out maxntemp) ? maxntemp : 0;
                                    dr["Max8HourO3Range"] = (maxntemp * 1000).ToString("0") + "~" + "/";
                                }
                            }
                            else if (!drMin[0]["Max8HourO3"].IsNotNullOrDBNull())
                            {
                                if (drMax[0]["Max8HourO3"].IsNotNullOrDBNull())
                                {
                                    decimal maxmtemp = decimal.TryParse(drMax[0]["Max8HourO3"].ToString(), out maxmtemp) ? maxmtemp : 0;
                                    dr["Max8HourO3Range"] = "/" + "~" + (maxmtemp * 1000).ToString("0");
                                }
                                else
                                {
                                    dr["Max8HourO3Range"] = "/";
                                }
                            }
                            break;
                    }
                }
                dt.Rows.Add(dr);
            }
            return dt.AsDataView();
        }
        /// <summary>
        /// 主要污染物月均值
        /// </summary>
        /// <param name="regionGuids">区域Guid</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>
        /// RegionName：区域名称
        /// PM25：PM25浓度
        /// PM10：PM10浓度
        /// NO2：NO2浓度
        /// SO2：SO2浓度
        /// CO：CO浓度
        /// 8HourO3：8HourO3浓度
        /// </returns>
        public DataView GetRegionPollutantMonthAvg(string[] regionGuids, DateTime dateStart, DateTime dateEnd)
        {
            DictionaryService dictionary = new DictionaryService();
            DataView dvAvgValues = GetRegionsAvgValue(regionGuids, dateStart, dateEnd);
            DataTable dt = new DataTable();
            dt.Columns.Add("RegionName", typeof(string));
            dt.Columns.Add("PM25", typeof(decimal));
            dt.Columns.Add("PM10", typeof(decimal));
            dt.Columns.Add("NO2", typeof(decimal));
            dt.Columns.Add("SO2", typeof(decimal));
            dt.Columns.Add("CO", typeof(decimal));
            dt.Columns.Add("8HourO3", typeof(decimal));
            for (int i = 0; i < regionGuids.Length; i++)
            {
                string RegionName = dictionary.GetTextByValue(Core.Enums.DictionaryType.Air, "行政区划", regionGuids[i]);
                DataRow[] drAvgValues = dvAvgValues.Table.Select("MonitoringRegionUid=" + regionGuids[i]);
                if (drAvgValues.Length > 0)
                {
                    DataRow dr = dt.NewRow();
                    dr["RegionName"] = RegionName;
                    if (!string.IsNullOrEmpty(drAvgValues[0]["PM25"].ToString()))
                        dr["PM25"] = DecimalExtension.GetRoundValue(Convert.ToDecimal(drAvgValues[0]["PM25"]), 1);
                    if (!string.IsNullOrEmpty(drAvgValues[0]["PM25"].ToString()))
                        dr["PM10"] = DecimalExtension.GetRoundValue(Convert.ToDecimal(drAvgValues[0]["PM10"]), 1);
                    if (!string.IsNullOrEmpty(drAvgValues[0]["PM25"].ToString()))
                        dr["NO2"] = DecimalExtension.GetRoundValue(Convert.ToDecimal(drAvgValues[0]["NO2"]), 1);
                    if (!string.IsNullOrEmpty(drAvgValues[0]["PM25"].ToString()))
                        dr["SO2"] = DecimalExtension.GetRoundValue(Convert.ToDecimal(drAvgValues[0]["SO2"]), 1);
                    if (!string.IsNullOrEmpty(drAvgValues[0]["PM25"].ToString()))
                        dr["CO"] = DecimalExtension.GetRoundValue(Convert.ToDecimal(drAvgValues[0]["CO"]), 1);
                    if (!string.IsNullOrEmpty(drAvgValues[0]["PM25"].ToString()))
                        dr["8HourO3"] = DecimalExtension.GetRoundValue(Convert.ToDecimal(drAvgValues[0]["Max8HourO3"]), 1);

                    dt.Rows.Add(dr);
                }
            }
            return dt.AsDataView();
        }

        /// <summary>
        /// 空气质量等级分布
        /// </summary>
        /// <param name="regionGuids">区域Guid</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public DataView RealTimeGradeDistribution(string[] regionGuids, DateTime dateStart, DateTime dateEnd)
        {
            DictionaryService dictionary = new DictionaryService();

            DataTable dt = new DataTable();
            dt.Columns.Add("RegionName", typeof(string));
            dt.Columns.Add("Great", typeof(int));
            dt.Columns.Add("Good", typeof(int));
            dt.Columns.Add("LightPollution", typeof(int));
            dt.Columns.Add("MiddlePollution", typeof(int));
            dt.Columns.Add("HighPollution", typeof(int));
            dt.Columns.Add("SeriousPollution", typeof(int));

            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            pointDayAQI = Singleton<DayAQIRepository>.GetInstance();

            for (int i = 0; i < regionGuids.Length; i++)
            {
                DataRow dr = dt.NewRow();
                dr["RegionName"] = dictionary.GetTextByValue(Core.Enums.DictionaryType.Air, "行政区划", regionGuids[i]);
                List<MonitoringPointEntity> monitoringPointEntity = g_MonitoringPointAir.RetrieveAirMPListByRegion(regionGuids[i]).ToList();
                if (monitoringPointEntity.Count > 0)
                {
                    string[] pointIds = monitoringPointEntity.Select(p => p.PointId.ToString()).ToArray();
                    DataTable dv = pointDayAQI.GetLastData(pointIds, dateStart, dateEnd).Table;

                    dr["Great"] = dv.Select("convert(AQIValue,'System.Int32')>=0 and convert(AQIValue,'System.Int32')<=50").Count();
                    dr["Good"] = dv.Select("convert(AQIValue,'System.Int32')>=51 and convert(AQIValue,'System.Int32')<=100").Count();
                    dr["LightPollution"] = dv.Select("convert(AQIValue,'System.Int32')>=101 and convert(AQIValue,'System.Int32')<=150").Count();
                    dr["MiddlePollution"] = dv.Select("convert(AQIValue,'System.Int32')>=151 and convert(AQIValue,'System.Int32')<=200").Count();
                    dr["HighPollution"] = dv.Select("convert(AQIValue,'System.Int32')>=201 and convert(AQIValue,'System.Int32')<=300").Count();
                    dr["SeriousPollution"] = dv.Select("convert(AQIValue,'System.Int32')>=301").Count();

                    dt.Rows.Add(dr);
                }
            }
            return dt.AsDataView();
        }
        /// <summary>
        /// 苏州市区自动监测周报
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>
        /// </returns>
        public DataTable GetRegionsTable(string[] regionGuids, DateTime dateStart, DateTime dateEnd)
        {
            regionDayAQI = Singleton<RegionDayAQIRepository>.GetInstance();
            DataTable dt = new DataTable();
            dt.Columns.Add("project", typeof(string));
            int mCount = 3;
            if (dateEnd.Year - 2013 >= 3)
            {
                mCount = dateEnd.Year - 2013;
            }
            for (int i = 0; i <= mCount; i++)
            {
                dt.Columns.Add(dateEnd.AddYears(-i).Year.ToString(), typeof(string));
            }
            dt.Columns.Add("thisStandRate", typeof(string));
            dt.Columns.Add("lastStandRate", typeof(string));
            dt.Columns.Add("invalidDays", typeof(string));
            dt.Columns.Add("YanzhongDays", typeof(string));
            dt.Columns.Add("lastYanzDays", typeof(string));
            //每页显示数据个数            
            int pageSize = int.MaxValue;
            //当前页的序号
            int currentPageIndex = 0;

            int recordTotal = 0;
            DataRow dtNew = dt.NewRow();
            dtNew["project"] = "PM2.5";
            for (int i = 0; i <= mCount; i++)
            {
                int countTotal = 0;
                int effectiveTotal = 0;
                DataView dv = new DataView();
                int a = dateStart.AddYears(-i).Year;
                if (dateStart.AddYears(-i).Year == 2013)
                {
                    dv = regionDayAQI.GetDataBasePager(regionGuids, dateStart.AddYears(-i), dateEnd.AddYears(-i));
                    countTotal = dv.Count;
                    int m = 0;
                    decimal value = 0;
                    effectiveTotal = dv.Count;
                    for (int j = 0; j < dv.Count; j++)
                    {
                        if (dv[j]["PM25"] != DBNull.Value)
                        {
                            m++;
                            value += decimal.Parse(dv[j]["PM25"].ToString());
                        }
                    }
                    if (i == 0)
                    {
                        if (countTotal != 0)
                        {
                            dtNew["thisStandRate"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(Convert.ToDecimal(dv.Count) / Convert.ToDecimal(countTotal) * 100), 1);
                        }
                        dtNew["invalidDays"] = dv.Count;
                    }
                    if (i == 1)
                    {
                        if (countTotal != 0)
                        {
                            dtNew["lastStandRate"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(Convert.ToDecimal(dv.Count) / Convert.ToDecimal(countTotal) * 100), 1);
                        }
                    }
                    if (m != 0)
                    {
                        dtNew[dateEnd.AddYears(-i).Year.ToString()] = DecimalExtension.GetPollutantValue(value * 1000 / m, 1);
                    }
                }
                else
                {
                    dv = regionDayAQI.GetDataPager(regionGuids, dateStart.AddYears(-i), dateEnd.AddYears(-i), pageSize, currentPageIndex, out recordTotal);
                    dv.RowFilter = "AQIValue is not null";
                    countTotal = dv.Count;
                    int m = 0;
                    decimal value = 0;
                    effectiveTotal = dv.Count;
                    dv.RowFilter = "PM25 is not null";
                    for (int j = 0; j < dv.Count; j++)
                    {
                        if (dv[j]["PM25"] != DBNull.Value)
                        {
                            m++;
                            value += decimal.Parse(dv[j]["PM25"].ToString());
                        }
                    }
                    if (i == 0)
                    {
                        dv.RowFilter = "AQIValue<=100";
                        if (countTotal != 0)
                        {
                            dtNew["thisStandRate"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(Convert.ToDecimal(dv.Count) / Convert.ToDecimal(countTotal) * 100), 1);
                        }
                        dv.RowFilter = "AQIValue is null";
                        dtNew["invalidDays"] = dv.Count;
                        dv.RowFilter = "AQIValue >200";
                        dtNew["YanzhongDays"] = dv.Count;
                    }
                    if (i == 1)
                    {
                        dv.RowFilter = "AQIValue<=100";
                        if (countTotal != 0)
                        {
                            dtNew["lastStandRate"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(Convert.ToDecimal(dv.Count) / Convert.ToDecimal(countTotal) * 100), 1);
                        }
                        dv.RowFilter = "AQIValue >200";
                        dtNew["lastYanzDays"] = dv.Count;
                    }
                    if (m != 0)
                    {
                        dtNew[dateEnd.AddYears(-i).Year.ToString()] = DecimalExtension.GetPollutantValue(value * 1000 / m, 1);
                    }
                }
            }
            dt.Rows.Add(dtNew);
            return dt;
        }
        /// <summary>
        /// 区域污染持续天数及污染程度简表
        /// </summary>
        /// <param name="portsId">站点</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>
        /// 返回字段：DateTime，ContinuousDays，LightPollution，ModeratePollution，HighPollution，SeriousPollution
        /// </returns>
        public DataView GetRegionsContinuousDaysTable(string[] regionGuids, DateTime dateStart, DateTime dateEnd)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("DateTime", typeof(string));
            dt.Columns.Add("ContinuousDays", typeof(int));
            dt.Columns.Add("LightPollution", typeof(int));
            dt.Columns.Add("ModeratePollution", typeof(int));
            dt.Columns.Add("HighPollution", typeof(int));
            dt.Columns.Add("SeriousPollution", typeof(int));
            int record = 0;
            DataTable AllData = GetAreaDataPager(regionGuids, dateStart, dateEnd, 999999999, 0, out record, "ReportDateTime Asc").Table;
            DataTable NewExceedingDays = AllData.Clone();
            DataRow[] AllExceedingDays = AllData.Select("convert(AQIValue,'System.Int32')>100");

            if (AllExceedingDays.Length > 0)
            {
                NewExceedingDays = AllExceedingDays.CopyToDataTable();
                DataView dv = NewExceedingDays.DefaultView;
                dv.Sort = "DateTime asc";
                List<List<DateTime>> ContinuousDaysList = new List<List<DateTime>>();
                List<DateTime> ContinuousDays = new List<DateTime>();

                for (int i = 1; i < NewExceedingDays.Rows.Count; i++)
                {
                    DateTime firstValue = Convert.ToDateTime(dv[i - 1]["DateTime"]);
                    DateTime secondValue = Convert.ToDateTime(dv[i]["DateTime"]);
                    int poor = (secondValue - firstValue).Days;
                    if (poor.Equals(1))
                    {
                        if (!ContinuousDays.Contains(firstValue))
                        {
                            ContinuousDays.Add(firstValue);
                        }
                        if (!ContinuousDays.Contains(secondValue))
                        {
                            ContinuousDays.Add(secondValue);
                        }
                        if (i == dv.Count - 1)
                        {
                            ContinuousDaysList.Add(ContinuousDays);
                        }
                    }
                    else
                    {
                        if (ContinuousDays.Count > 0)
                        {
                            ContinuousDaysList.Add(ContinuousDays);
                        }
                        ContinuousDays = new List<DateTime>();
                    }
                }

                for (int i = 0; i < ContinuousDaysList.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    List<DateTime> dateTimeArray = ContinuousDaysList[i];
                    DataTable ExceedingDays = NewExceedingDays.Clone();
                    ExceedingDays = NewExceedingDays.Select("DateTime>=" + dateTimeArray[0] + " and ReportDateTime<=" + dateTimeArray[dateTimeArray.Count - 1]).CopyToDataTable();
                    DataRow[] LightPollution = ExceedingDays.Select("convert(AQIValue,'System.Int32')>=101 and convert(AQIValue,'System.Int32')<=150");
                    DataRow[] ModeratePollution = ExceedingDays.Select("convert(AQIValue,'System.Int32')>=151 and convert(AQIValue,'System.Int32')<=200");
                    DataRow[] HighPollution = ExceedingDays.Select("convert(AQIValue,'System.Int32')>=201 and convert(AQIValue,'System.Int32')<=300");
                    DataRow[] SeriousPollution = ExceedingDays.Select("convert(AQIValue,'System.Int32')>300");

                    int outDays = dateTimeArray.Count;
                    int LightPollutionDays = LightPollution.Length;
                    int ModeratePollutionDays = ModeratePollution.Length;
                    int HighPollutionDays = HighPollution.Length;
                    int SeriousPollutionDays = SeriousPollution.Length;

                    dr["DateTime"] = dateTimeArray[0].ToString("MM-dd") + "—" + dateTimeArray[dateTimeArray.Count - 1].ToString("MM-dd");
                    dr["ContinuousDays"] = outDays;
                    dr["LightPollution"] = LightPollutionDays;
                    dr["ModeratePollution"] = ModeratePollutionDays;
                    dr["HighPollution"] = HighPollutionDays;
                    dr["SeriousPollution"] = SeriousPollutionDays;

                    dt.Rows.Add(dr);
                }
                return dt.DefaultView;
            }
            return null;
        }

        /// <summary>
        /// 获取不同区域污染情况分析数据
        /// </summary>
        /// <param name="regionGuid">区域</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <param name="AQIType">污染因子</param>
        /// <param name="pollutionGrade">污染等级</param>
        /// <returns>returnDataTable</returns>
        public DataView GetRegionsPollution(string regionGuid, DateTime[,] dateTime, int pollutionClass)
        {
            regionDayAQI = Singleton<RegionDayAQIRepository>.GetInstance();
            List<List<RegionDayAQIReportEntity>> AllData = new List<List<RegionDayAQIReportEntity>>();
            DataTable dt = new DataTable();
            for (int i = 0; i < dateTime.GetLength(0); i++)
            {
                dt.Columns.Add(dateTime[i, 0].Year.ToString() + "(" + i + ")", typeof(string));
                dt.Columns.Add("AQIValue" + "(" + i + ")", typeof(string));
                dt.Columns.Add("PrimaryPollutant" + "(" + i + ")", typeof(string));
                dt.Columns.Add("Grade" + "(" + i + ")", typeof(string));

                DateTime dateStart = dateTime[i, 0];
                DateTime dateEnd = dateTime[i, 1];
                List<RegionDayAQIReportEntity> data = regionDayAQI.Retrieve(p => p.MonitoringRegionUid == regionGuid && p.ReportDateTime >= dateStart && p.ReportDateTime <= dateEnd && p.Class == pollutionClass.ToString()).ToList<RegionDayAQIReportEntity>();
                AllData.Add(data);
            }

            if (AllData.Count > 0)
            {
                int Max = AllData[0].Count;
                for (int i = 1; i < AllData.Count; i++)
                {
                    if (AllData[i].Count > Max)
                    {
                        Max = AllData[i].Count;
                    }
                }

                for (int i = 0; i < Max; i++)
                {
                    DataRow dr = dt.NewRow();
                    for (int k = 0; k < dt.Columns.Count; k++)
                    {
                        dr[k] = "";
                    }
                    dt.Rows.Add(dr);
                }

                if (dateTime.GetLength(0) == 1)
                {
                    List<RegionDayAQIReportEntity> data1 = AllData[0];
                    for (int i = 0; i < data1.Count; i++)
                    {
                        dt.Rows[i][0] = data1[i].ReportDateTime.ToString();
                        dt.Rows[i][1] = data1[i].AQIValue.ToString();
                        dt.Rows[i][2] = data1[i].PrimaryPollutant.ToString();
                        dt.Rows[i][3] = data1[i].Class.ToString();
                    }
                }

                if (dateTime.GetLength(0) == 2)
                {
                    List<RegionDayAQIReportEntity> data1 = AllData[0];
                    List<RegionDayAQIReportEntity> data2 = AllData[1];
                    for (int i = 0; i < data1.Count; i++)
                    {
                        dt.Rows[i][0] = data1[i].ReportDateTime.ToString();
                        dt.Rows[i][1] = data1[i].AQIValue.ToString();
                        dt.Rows[i][2] = data1[i].PrimaryPollutant.ToString();
                        dt.Rows[i][3] = data1[i].Class.ToString();
                    }
                    for (int i = 0; i < data2.Count; i++)
                    {
                        dt.Rows[i][0] = data2[i].ReportDateTime.ToString();
                        dt.Rows[i][1] = data2[i].AQIValue.ToString();
                        dt.Rows[i][2] = data2[i].PrimaryPollutant.ToString();
                        dt.Rows[i][3] = data2[i].Class.ToString();
                    }
                }

                if (dateTime.GetLength(0) == 3)
                {
                    List<RegionDayAQIReportEntity> data1 = AllData[0];
                    List<RegionDayAQIReportEntity> data2 = AllData[1];
                    List<RegionDayAQIReportEntity> data3 = AllData[2];
                    for (int i = 0; i < data1.Count; i++)
                    {
                        dt.Rows[i][0] = data1[i].ReportDateTime.ToString();
                        dt.Rows[i][1] = data1[i].AQIValue.ToString();
                        dt.Rows[i][2] = data1[i].PrimaryPollutant.ToString();
                        dt.Rows[i][3] = data1[i].Class.ToString();
                    }
                    for (int i = 0; i < data2.Count; i++)
                    {
                        dt.Rows[i][4] = data2[i].ReportDateTime.ToString();
                        dt.Rows[i][5] = data2[i].AQIValue.ToString();
                        dt.Rows[i][6] = data2[i].PrimaryPollutant.ToString();
                        dt.Rows[i][7] = data2[i].Class.ToString();
                    }
                    for (int i = 0; i < data3.Count; i++)
                    {
                        dt.Rows[i][8] = data3[i].ReportDateTime.ToString();
                        dt.Rows[i][9] = data3[i].AQIValue.ToString();
                        dt.Rows[i][10] = data3[i].PrimaryPollutant.ToString();
                        dt.Rows[i][11] = data3[i].Class.ToString();
                    }
                }
            }
            return dt.DefaultView;
        }

        /// <summary>
        /// 获取空气质量日报
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        public DataView GetRegionAirQualityDayReport(string[] regionGuids, DateTime dtStart, DateTime dtEnd, string[] type, int pageSize
            , int pageNo, out int recordTotal, string orderBy = "ReportDateTime,MonitoringRegionUid")
        {
            recordTotal = 0;
            DictionaryService dictionary = new DictionaryService();
            DataTable dt = new DataTable();
            dt = GetAreaDataPager(regionGuids, dtStart, dtEnd, type, pageSize, pageNo, out recordTotal, orderBy).Table;
            dt.Columns.Add("RegionName", typeof(string));
            dt.Columns.Add("OrderNew", typeof(int));
            dt.Columns.Add("DateTimeNew", typeof(DateTime));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string regionUid = dt.Rows[i]["MonitoringRegionUid"].ToString();
                dt.Rows[i]["RegionName"] = dictionary.GetTextByValue(Core.Enums.DictionaryType.Air, "行政区划", regionUid);
                if (dt.Rows[i]["AQIValue"] != DBNull.Value)
                {
                    if (int.Parse(dt.Rows[i]["AQIValue"].ToString()) <= 50)
                    {
                        dt.Rows[i]["OrderNew"] = "1";
                    }
                    else if (int.Parse(dt.Rows[i]["AQIValue"].ToString()) <= 100)
                    {
                        dt.Rows[i]["OrderNew"] = "2";
                    }
                    else if (int.Parse(dt.Rows[i]["AQIValue"].ToString()) <= 150)
                    {
                        dt.Rows[i]["OrderNew"] = "3";
                    }
                    else if (int.Parse(dt.Rows[i]["AQIValue"].ToString()) <= 200)
                    {
                        dt.Rows[i]["OrderNew"] = "4";
                    }
                    else if (int.Parse(dt.Rows[i]["AQIValue"].ToString()) <= 300)
                    {
                        dt.Rows[i]["OrderNew"] = "5";
                    }
                    else
                    {
                        dt.Rows[i]["OrderNew"] = "6";
                    }
                }
                else
                {
                    dt.Rows[i]["OrderNew"] = "7";
                }
                if (dt.Rows[i]["ReportDateTime"] != DBNull.Value)
                {
                    dt.Rows[i]["DateTimeNew"] = DateTime.Parse(dt.Rows[i]["ReportDateTime"].ToString());
                }
            }
            return dt.AsDataView();
        }
        public DataView GetRegionAirQualityDayReport(string[] regionGuids, DateTime dtStart, DateTime dtEnd, int pageSize
      , int pageNo, out int recordTotal, string orderBy = "ReportDateTime,MonitoringRegionUid")
        {
            recordTotal = 0;
            DictionaryService dictionary = new DictionaryService();
            DataTable dt = new DataTable();
            dt = GetAreaDataPager(regionGuids, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy).Table;
            dt.Columns.Add("RegionName", typeof(string));
            dt.Columns.Add("OrderNew", typeof(int));
            dt.Columns.Add("DateTimeNew", typeof(DateTime));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string regionUid = dt.Rows[i]["MonitoringRegionUid"].ToString();
                dt.Rows[i]["RegionName"] = dictionary.GetTextByValue(Core.Enums.DictionaryType.Air, "行政区划", regionUid);
                if (dt.Rows[i]["AQIValue"] != DBNull.Value)
                {
                    if (int.Parse(dt.Rows[i]["AQIValue"].ToString()) <= 50)
                    {
                        dt.Rows[i]["OrderNew"] = "1";
                    }
                    else if (int.Parse(dt.Rows[i]["AQIValue"].ToString()) <= 100)
                    {
                        dt.Rows[i]["OrderNew"] = "2";
                    }
                    else if (int.Parse(dt.Rows[i]["AQIValue"].ToString()) <= 150)
                    {
                        dt.Rows[i]["OrderNew"] = "3";
                    }
                    else if (int.Parse(dt.Rows[i]["AQIValue"].ToString()) <= 200)
                    {
                        dt.Rows[i]["OrderNew"] = "4";
                    }
                    else if (int.Parse(dt.Rows[i]["AQIValue"].ToString()) <= 300)
                    {
                        dt.Rows[i]["OrderNew"] = "5";
                    }
                    else
                    {
                        dt.Rows[i]["OrderNew"] = "6";
                    }
                }
                else
                {
                    dt.Rows[i]["OrderNew"] = "7";
                }
                if (dt.Rows[i]["ReportDateTime"] != DBNull.Value)
                {
                    dt.Rows[i]["DateTimeNew"] = DateTime.Parse(dt.Rows[i]["ReportDateTime"].ToString());
                }
            }
            if (regionGuids.Contains("1"))
            {
                DataTable dtReturn = regionDayAQI.GetAQIBaseInfo(dtStart, dtEnd).ToTable();
                if (dtReturn.Rows.Count > 0)
                {
                    for (int j = 0; j < dtReturn.Rows.Count; j++)
                    {
                        DataRow drNew = dt.NewRow();
                        drNew["MonitoringRegionUid"] = "1";
                        drNew["RegionName"] = "苏州大市";
                        drNew["ReportDateTime"] = dtReturn.Rows[j]["DateTime"];
                        foreach (DataColumn dc in dt.Columns)
                        {
                            if (dtReturn.Columns.Contains(dc.ColumnName.Replace("IAQI", "AQI")))
                            {
                                drNew[dc.ColumnName] = dtReturn.Rows[j][dc.ColumnName.Replace("IAQI", "AQI")];
                            }
                        }
                        drNew["AQIValue"] = dtReturn.Rows[j]["Max_AQI"];
                        drNew["RGBValue"] = dtReturn.Rows[j]["Color"];
                        if (dtReturn.Rows[j]["DateTime"] != DBNull.Value)
                        {
                            drNew["DateTimeNew"] = DateTime.Parse(dtReturn.Rows[j]["DateTime"].ToString());
                        }
                        if (dtReturn.Rows[j]["Max_AQI"] != DBNull.Value)
                        {
                            if (int.Parse(dtReturn.Rows[j]["Max_AQI"].ToString()) <= 50)
                            {
                                drNew["OrderNew"] = "1";
                            }
                            else if (int.Parse(dtReturn.Rows[j]["Max_AQI"].ToString()) <= 100)
                            {
                                drNew["OrderNew"] = "2";
                            }
                            else if (int.Parse(dtReturn.Rows[j]["Max_AQI"].ToString()) <= 150)
                            {
                                drNew["OrderNew"] = "3";
                            }
                            else if (int.Parse(dtReturn.Rows[j]["Max_AQI"].ToString()) <= 200)
                            {
                                drNew["OrderNew"] = "4";
                            }
                            else if (int.Parse(dtReturn.Rows[j]["Max_AQI"].ToString()) <= 300)
                            {
                                drNew["OrderNew"] = "5";
                            }
                            else
                            {
                                drNew["OrderNew"] = "6";
                            }
                        }
                        else
                        {
                            drNew["OrderNew"] = "7";
                        }
                        dt.Rows.Add(drNew);
                    }
                }
            }
            return dt.AsDataView();
        }
        #endregion


        #region 接口实现
        /// <summary>
        /// 获取某段时间内某一监测点AQI
        /// </summary>
        /// <returns></returns>
        public int GetTimePortAQI()
        {
            return 0;
        }

        /// <summary>
        /// 获取某段时间内苏州市区所有监测点AQI
        /// </summary>
        /// <returns></returns>
        public int GetTimeSzPortsAQI()
        {
            return 0;
        }

        /// <summary>
        /// 获取某段时间内某一城区所有监测点AQI
        /// </summary>
        /// <returns></returns>
        public int GetTimeAreaPortsAQI()
        {
            return 0;
        }

        /// <summary>
        /// 获取某段时间内某一监测点AQI污染等级统计
        /// </summary>
        /// <returns></returns>
        public int GetTimePortAQIGrade()
        {
            return 0;
        }

        /// <summary>
        /// 获取某段时间内苏州市区所有监测点AQI污染等级统计
        /// </summary>
        /// <returns></returns>
        public int GetTimeSzPortsAQIGrade()
        {
            return 0;
        }

        /// <summary>
        /// 获取某段时间内某一城区所有监测点AQI污染等级统计
        /// </summary>
        /// <returns></returns>
        public int GetTimeAreaPortsAQIGrade()
        {
            return 0;
        }

        /// <summary>
        /// 获取某段时间内某一监测点首要污染物统计
        /// </summary>
        /// <returns></returns>
        public int GetTimePortPrimaryPollutant()
        {
            return 0;
        }

        /// <summary>
        /// 获取某段时间内苏州市区所有监测点首要污染物统计
        /// </summary>
        /// <returns></returns>
        public int GetTimeSzPortsPrimaryPollutant()
        {
            return 0;
        }

        /// <summary>
        /// 获取某段时间内某一城区所有监测点首要污染物统计
        /// </summary>
        /// <returns></returns>
        public int GetTimeAreaPortsPrimaryPollutant()
        {
            return 0;
        }

        /// <summary>
        /// 获取某段时间内某一监测点超标天数统计
        /// </summary>
        /// <returns></returns>
        public int GetTimePortOutDay()
        {
            return 0;
        }

        /// <summary>
        /// 获取某段时间内苏州市区所有监测点超标天数统计
        /// </summary>
        /// <returns></returns>
        public int GetTimeSzPortsOutDay()
        {
            return 0;
        }

        /// <summary>
        /// 获取某段时间内某一城区所有监测点超标天数统计
        /// </summary>
        /// <returns></returns>
        public int GetTimeAreaPortsOutDay()
        {
            return 0;
        }
        #endregion

        #region 获取站点日AQI数据
        /// <summary>
        /// 获取站点日AQI数据
        /// </summary>
        /// <param name="PointId">站点Id</param>
        /// <param name="StartDate">开始日期</param>
        /// <param name="EndDate">截止日期</param>
        /// <returns>DataView</returns>
        public DataView GetDataByPoint(int PointId, DateTime StartDate, DateTime EndDate)
        {
            pointDayAQI = new DayAQIRepository();
            return pointDayAQI.GetDataByPoint(PointId, StartDate, EndDate);
        }
        #endregion

        #region 空气质量日报统计专用方法
        /// <summary>
        /// 获取时间段内指定测点的日数据
        /// </summary>
        /// <param name="PointIds">测点ID数组</param>
        /// <param name="StartDate">开始日期</param>
        /// <param name="EndDate">结束日期</param>
        /// <returns>DataTable</returns>
        public DataTable GetDayAQIData(List<int> PointIds, DateTime StartDate, DateTime EndDate)
        {
            pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
            return pointDayAQI.GetDayAQIData(PointIds, StartDate, EndDate);
        }

        /// <summary>
        /// 计算时间段内指定测点监测因子的浓度及分指数平均值
        /// </summary>
        /// <param name="PointIds">测点ID数组</param>
        /// <param name="StartDate">开始日期</param>
        /// <param name="EndDate">结束日期</param>
        /// <returns>DataTable</returns>
        public DataTable GetAvgDayAQIData(List<int> PointIds, DateTime StartDate, DateTime EndDate)
        {
            pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
            return pointDayAQI.GetAvgDayAQIData(PointIds, StartDate, EndDate);
        }
        #endregion

        /// <summary>
        /// 根据lambda表达式要求，返回所有对象
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IQueryable<RegionDayAQIReportEntity> regionRetrieve(Expression<Func<RegionDayAQIReportEntity, bool>> predicate)
        {
            regionDayAQI = Singleton<RegionDayAQIRepository>.GetInstance();
            return regionDayAQI.Retrieve(predicate);
        }


        /// <summary>
        /// 根据测点和污染种类创建的DataTable
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="classDics"></param>
        /// <param name="dtBegion"></param>
        /// <param name="dtEnd"></param>
        /// <param name="TotalType"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        public DataTable GetPortPolluateClassList(string[] portIds, string[] classDics, string[] factorDics, DateTime dtBegion, DateTime dtEnd, string TotalType, Dictionary<string, string> className, string lastDays)
        {
            try
            {
                DataTable dt = new DataTable();
                #region 统计类型为类别
                if (TotalType == "1")
                {
                    dt.Columns.Add("PointName", typeof(string));

                    dt.Columns.Add("ClassName", typeof(string));
                    dt.Columns.Add("Days", typeof(Int32));
                    dt.Columns.Add("SpecficDay", typeof(string));
                    pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
                    DictionaryService dictionary = Singleton<DictionaryService>.GetInstance();

                    DataView dv = pointDayAQI.GetPointDataPagerClass(portIds, classDics, dtBegion, dtEnd);
                    if (dv != null)
                    {
                        foreach (string portItem in portIds)
                        {

                            //  DataRow drNew = dt.NewRow();



                            foreach (string classItem in classDics)
                            {
                                DataRow drNew = dt.NewRow();
                                drNew["PointName"] = portItem;
                                drNew["ClassName"] = className[classItem];
                                switch (classItem)
                                {
                                    case "0":
                                        dv.RowFilter = "PointId='" + portItem + "' and AQIValue >= 0 and AQIValue <= 50";
                                        drNew["Days"] = dv.Count;

                                        drNew["SpecficDay"] = "--";
                                        break;
                                    case "1":
                                        dv.RowFilter = "PointId='" + portItem + "' and AQIValue >= 51 and AQIValue <= 100";

                                        drNew["Days"] = dv.Count;
                                        drNew["SpecficDay"] = "--";
                                        break;
                                    case "2":
                                        dv.RowFilter = "PointId='" + portItem + "' and AQIValue >= 101 and AQIValue <= 151";
                                        drNew["Days"] = dv.Count;
                                        drNew["SpecficDay"] = GetStr(dv, "DateTime");
                                        break;
                                    case "3":
                                        dv.RowFilter = "PointId='" + portItem + "' and AQIValue >= 151 and AQIValue <= 200";
                                        drNew["Days"] = dv.Count;
                                        drNew["SpecficDay"] = GetStr(dv, "DateTime");
                                        break;
                                    case "4":
                                        dv.RowFilter = "PointId='" + portItem + "' and AQIValue >= 201 and AQIValue <= 300";
                                        drNew["Days"] = dv.Count;
                                        drNew["SpecficDay"] = GetStr(dv, "DateTime");
                                        break;
                                    case "5":
                                        dv.RowFilter = "PointId='" + portItem + "' and AQIValue > 300";
                                        drNew["Days"] = dv.Count;
                                        drNew["SpecficDay"] = GetStr(dv, "DateTime");
                                        break;
                                    case "6":
                                        dv.RowFilter = "PointId='" + portItem + "' and AQIValue < 0 or AQIValue is null";
                                        drNew["Days"] = dv.Count;
                                        drNew["SpecficDay"] = "--";
                                        break;


                                }
                                dt.Rows.Add(drNew);
                            }

                        }
                    }
                }
                #endregion
                #region 统计类型为污染持续
                if (TotalType == "2")
                {

                    int nLastDays = Convert.ToInt32(lastDays);
                    Dictionary<string, string> classLevel = new Dictionary<string, string>();
                    pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
                    classLevel.Add("2", "轻度污染");
                    classLevel.Add("3", "中度污染");
                    classLevel.Add("4", "重度污染");
                    classLevel.Add("5", "严重污染");
                    string[] classLevels = { "2", "3", "4", "5" };
                    dt.Columns.Add("PointName", typeof(string));

                    dt.Columns.Add("DateTime", typeof(string));
                    dt.Columns.Add("LastDays", typeof(Int32));
                    dt.Columns.Add("ClassName", typeof(string));
                    dt.Columns.Add("Days", typeof(Int32));
                    dt.Columns.Add("SpecficDay", typeof(string));
                    DataView dv = pointDayAQI.GetPointDataPagerClass(portIds, classDics, dtBegion, dtEnd);
                    if (dv != null)
                    {
                        DataTable AreaPolluation = dv.Table;
                        DataTable NewExceedingDays = AreaPolluation.Clone();
                        foreach (string portItem in portIds)
                        {
                            DataRow[] AllExceedingDays = AreaPolluation.Select("convert(AQIValue,'System.Int32')>100 and PointId='" + portItem + "'");
                            if (AllExceedingDays.Length > 0)
                            {
                                NewExceedingDays = AllExceedingDays.CopyToDataTable();
                                DataView dvs = NewExceedingDays.DefaultView;
                                dvs.Sort = "DateTime asc";
                                List<List<DateTime>> ContinuousDaysList = new List<List<DateTime>>();
                                List<DateTime> ContinuousDays = new List<DateTime>();
                                if (dvs.Count >= nLastDays)
                                {
                                    for (int i = 1; i < dvs.Count; i++)
                                    {
                                        DateTime firstValue = Convert.ToDateTime(dvs[i - 1]["DateTime"]);
                                        DateTime secondValue = Convert.ToDateTime(dvs[i]["DateTime"]);
                                        int poor = (secondValue - firstValue).Days;
                                        if (poor.Equals(1))
                                        {
                                            if (!ContinuousDays.Contains(firstValue))
                                            {
                                                ContinuousDays.Add(firstValue);
                                            }
                                            if (!ContinuousDays.Contains(secondValue))
                                            {
                                                ContinuousDays.Add(secondValue);
                                            }
                                            if (i == dvs.Count - 1)
                                            {
                                                if (ContinuousDays.Count >= nLastDays)
                                                {
                                                    ContinuousDaysList.Add(ContinuousDays);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (ContinuousDays.Count >= nLastDays)
                                            {
                                                ContinuousDaysList.Add(ContinuousDays);
                                            }
                                            ContinuousDays = new List<DateTime>();
                                        }
                                    }

                                    for (int i = 0; i < ContinuousDaysList.Count; i++)
                                    {

                                        List<DateTime> dateTimeArray = ContinuousDaysList[i];
                                        DataTable ExceedingDays = NewExceedingDays.Clone();
                                        ExceedingDays = NewExceedingDays.Select("DateTime>='" + dateTimeArray[0] + "' and DateTime<='" + dateTimeArray[dateTimeArray.Count - 1] + "'").CopyToDataTable();
                                        int outDays = dateTimeArray.Count;
                                        foreach (string classItem in classLevels)
                                        {
                                            DataRow dr = dt.NewRow();
                                            switch (classItem)
                                            {
                                                case "2":
                                                    string LightPollutionday = "";
                                                    DataRow[] LightPollution = ExceedingDays.Select("convert(AQIValue,'System.Int32')>=101 and convert(AQIValue,'System.Int32')<=150", "DateTime");
                                                    foreach (DataRow light in LightPollution)
                                                    {
                                                        if (light["DateTime"].IsNotNullOrDBNull())
                                                        {
                                                            LightPollutionday = LightPollutionday + light["DateTime"].FormatToString("yyyy-MM-dd") + "," + light["AQIValue"] + ";";
                                                        }
                                                    }
                                                    int LightPollutionDays = LightPollution.Length;
                                                    dr["PointName"] = portItem;
                                                    dr["DateTime"] = dateTimeArray[0].ToString("yyyy-MM-dd") + "—" + dateTimeArray[dateTimeArray.Count - 1].ToString("yyyy-MM-dd");
                                                    dr["LastDays"] = outDays;
                                                    dr["ClassName"] = classLevel[classItem];
                                                    dr["Days"] = LightPollutionDays;
                                                    dr["SpecficDay"] = LightPollutionday;
                                                    break;
                                                case "3":
                                                    string ModeratePollutionday = "";
                                                    DataRow[] ModeratePollution = ExceedingDays.Select("convert(AQIValue,'System.Int32')>=151 and convert(AQIValue,'System.Int32')<=200", "DateTime");
                                                    foreach (DataRow moderate in ModeratePollution)
                                                    {
                                                        if (moderate["DateTime"].IsNotNullOrDBNull())
                                                        {
                                                            ModeratePollutionday = ModeratePollutionday + moderate["DateTime"].FormatToString("yyyy-MM-dd") + "," + moderate["AQIValue"] + ";";
                                                        }
                                                    }
                                                    int ModeratePollutionDays = ModeratePollution.Length;
                                                    dr["PointName"] = portItem;
                                                    dr["DateTime"] = dateTimeArray[0].ToString("yyyy-MM-dd") + "—" + dateTimeArray[dateTimeArray.Count - 1].ToString("yyyy-MM-dd");
                                                    dr["LastDays"] = outDays;
                                                    dr["ClassName"] = classLevel[classItem];
                                                    dr["Days"] = ModeratePollutionDays;
                                                    dr["SpecficDay"] = ModeratePollutionday;
                                                    break;
                                                case "4":
                                                    string HighPollutionday = "";
                                                    DataRow[] HighPollution = ExceedingDays.Select("convert(AQIValue,'System.Int32')>=201 and convert(AQIValue,'System.Int32')<=300", "DateTime");
                                                    foreach (DataRow high in HighPollution)
                                                    {
                                                        if (high["DateTime"].IsNotNullOrDBNull())
                                                        {
                                                            HighPollutionday = HighPollutionday + high["DateTime"].FormatToString("yyyy-MM-dd") + "," + high["AQIValue"] + ";";
                                                        }
                                                    }
                                                    int HighPollutionDays = HighPollution.Length;
                                                    dr["PointName"] = portItem;
                                                    dr["DateTime"] = dateTimeArray[0].ToString("yyyy-MM-dd") + "—" + dateTimeArray[dateTimeArray.Count - 1].ToString("yyyy-MM-dd");
                                                    dr["LastDays"] = outDays;
                                                    dr["ClassName"] = classLevel[classItem];
                                                    dr["Days"] = HighPollutionDays;
                                                    dr["SpecficDay"] = HighPollutionday;
                                                    break;
                                                case "5":
                                                    string SeriousPollutionday = "";
                                                    DataRow[] SeriousPollution = ExceedingDays.Select("convert(AQIValue,'System.Int32')>=301 ", "DateTime");
                                                    foreach (DataRow serious in SeriousPollution)
                                                    {
                                                        if (serious["DateTime"].IsNotNullOrDBNull())
                                                        {
                                                            SeriousPollutionday = SeriousPollutionday + serious["DateTime"].FormatToString("yyyy-MM-dd") + "," + serious["AQIValue"] + ";";
                                                        }
                                                    }
                                                    int SeriousPollutionDays = SeriousPollution.Length;
                                                    dr["PointName"] = portItem;
                                                    dr["DateTime"] = dateTimeArray[0].ToString("yyyy-MM-dd") + "—" + dateTimeArray[dateTimeArray.Count - 1].ToString("yyyy-MM-dd");
                                                    dr["LastDays"] = outDays;
                                                    dr["ClassName"] = classLevel[classItem];
                                                    dr["Days"] = SeriousPollutionDays;
                                                    dr["SpecficDay"] = SeriousPollutionday;
                                                    break;
                                            }
                                            dt.Rows.Add(dr);
                                        }





                                    }
                                }
                            }

                        }
                    }
                }
                #endregion
                #region 统计类型为首要污染物
                if (TotalType == "3")
                {
                    dt.Columns.Add("PointName", typeof(string));

                    dt.Columns.Add("PolluteName", typeof(string));
                    dt.Columns.Add("Days", typeof(Int32));
                    dt.Columns.Add("Rates", typeof(Int32));
                    dt.Columns.Add("MoreSpecficDay", typeof(string));
                    pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
                    DictionaryService dictionary = Singleton<DictionaryService>.GetInstance();

                    DataView dv = pointDayAQI.GetPointFirstPollute(portIds, classDics, dtBegion, dtEnd);
                    if (dv != null)
                    {
                        foreach (string portItem in portIds)
                        {
                            dv.RowFilter = "PointId='" + portItem + "' and (PrimaryPollutant like '%PM2.5%' or PrimaryPollutant like '%PM10%' or PrimaryPollutant like '%NO2%' or PrimaryPollutant like '%SO2%' or PrimaryPollutant like '%CO%' or PrimaryPollutant like '%O3%')";
                            double sum = dv.Count;
                            foreach (string factorItem in factorDics)
                            {
                                int rate;
                                DataRow drNew = dt.NewRow();
                                drNew["PointName"] = portItem;
                                drNew["PolluteName"] = factorItem;
                                switch (factorItem)
                                {
                                    case "PM2.5":
                                        dv.RowFilter = "PointId='" + portItem + "' and PrimaryPollutant like '%PM2.5%'";
                                        drNew["Days"] = dv.Count;
                                        if (sum != 0)
                                        {
                                            rate = (int)(Math.Round(dv.Count / sum, 2) * 100);
                                        }
                                        else
                                        {
                                            rate = 0;
                                        }
                                        drNew["Rates"] = rate;
                                        drNew["MoreSpecficDay"] = GetStr(dv, "DateTime");
                                        break;
                                    case "PM10":
                                        dv.RowFilter = "PointId='" + portItem + "' and PrimaryPollutant like '%PM10%'";
                                        drNew["Days"] = dv.Count;
                                        if (sum != 0)
                                        {
                                            rate = (int)(Math.Round(dv.Count / sum, 2) * 100);
                                        }
                                        else
                                        {
                                            rate = 0;
                                        }
                                        drNew["Rates"] = rate;
                                        drNew["MoreSpecficDay"] = GetStr(dv, "DateTime");
                                        break;
                                    case "NO2":
                                        dv.RowFilter = "PointId='" + portItem + "' and PrimaryPollutant like '%NO2%'";
                                        drNew["Days"] = dv.Count;
                                        if (sum != 0)
                                        {
                                            rate = (int)(Math.Round(dv.Count / sum, 2) * 100);
                                        }
                                        else
                                        {
                                            rate = 0;
                                        }
                                        drNew["Rates"] = rate;
                                        drNew["MoreSpecficDay"] = GetStr(dv, "DateTime");
                                        break;
                                    case "SO2":
                                        dv.RowFilter = "PointId='" + portItem + "' and PrimaryPollutant like '%SO2%'";
                                        drNew["Days"] = dv.Count;
                                        if (sum != 0)
                                        {
                                            rate = (int)(Math.Round(dv.Count / sum, 2) * 100);
                                        }
                                        else
                                        {
                                            rate = 0;
                                        }
                                        drNew["Rates"] = rate;
                                        drNew["MoreSpecficDay"] = GetStr(dv, "DateTime");
                                        break;
                                    case "CO":
                                        dv.RowFilter = "PointId='" + portItem + "' and PrimaryPollutant like '%CO%'";
                                        drNew["Days"] = dv.Count;
                                        if (sum != 0)
                                        {
                                            rate = (int)(Math.Round(dv.Count / sum, 2) * 100);
                                        }
                                        else
                                        {
                                            rate = 0;
                                        }
                                        drNew["Rates"] = rate;
                                        drNew["MoreSpecficDay"] = GetStr(dv, "DateTime");
                                        break;
                                    case "O3-8小时":
                                        dv.RowFilter = "PointId='" + portItem + "' and PrimaryPollutant like '%O3%'";
                                        drNew["Days"] = dv.Count;
                                        if (sum != 0)
                                        {
                                            rate = (int)(Math.Round(dv.Count / sum, 2) * 100);
                                        }
                                        else
                                        {
                                            rate = 0;
                                        }
                                        drNew["Rates"] = rate;
                                        drNew["MoreSpecficDay"] = GetStr(dv, "DateTime");
                                        break;



                                }
                                dt.Rows.Add(drNew);
                            }
                        }
                    }
                }
                #endregion
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        /// <summary>
        /// 获取空气质量日报新方法(补全不存在数据的日期)
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        public DataView GetAirQualityDayReportNew(string[] portIds, DateTime dtStart, DateTime dtEnd, string[] type, int pageSize
            , int pageNo, out int recordTotal, string orderBy = "DateTime,PointId")
        {
            recordTotal = 0;
            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            DataTable dt = new DataTable();
            dt = GetPortDataPagerNew(portIds, dtStart, dtEnd, type, pageSize, pageNo, out recordTotal, orderBy).Table;
            dt.Columns.Add("PortName", typeof(string));
            dt.Columns.Add("OrderNew", typeof(int));
            dt.Columns.Add("DateTimeNew", typeof(DateTime));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["PointId"] != DBNull.Value && dt.Rows[i]["PointId"].ToString().Trim() != "")
                {
                    int pointId = Convert.ToInt32(dt.Rows[i]["PointId"]);
                    dt.Rows[i]["PortName"] = g_MonitoringPointAir.RetrieveEntityByPointId(pointId).MonitoringPointName;
                }

                if (dt.Rows[i]["AQIValue"] != DBNull.Value && dt.Rows[i]["AQIValue"].ToString().Trim() != "")
                {
                    if (int.Parse(dt.Rows[i]["AQIValue"].ToString()) <= 50)
                    {
                        dt.Rows[i]["OrderNew"] = "1";
                    }
                    else if (int.Parse(dt.Rows[i]["AQIValue"].ToString()) <= 100)
                    {
                        dt.Rows[i]["OrderNew"] = "2";
                    }
                    else if (int.Parse(dt.Rows[i]["AQIValue"].ToString()) <= 150)
                    {
                        dt.Rows[i]["OrderNew"] = "3";
                    }
                    else if (int.Parse(dt.Rows[i]["AQIValue"].ToString()) <= 200)
                    {
                        dt.Rows[i]["OrderNew"] = "4";
                    }
                    else if (int.Parse(dt.Rows[i]["AQIValue"].ToString()) <= 300)
                    {
                        dt.Rows[i]["OrderNew"] = "5";
                    }
                    else
                    {
                        dt.Rows[i]["OrderNew"] = "6";
                    }
                }
                else
                {
                    dt.Rows[i]["OrderNew"] = "7";
                }
                if (dt.Rows[i]["DateTime"] != DBNull.Value)
                {
                    dt.Rows[i]["DateTimeNew"] = DateTime.Parse(dt.Rows[i]["DateTime"].ToString());
                }
            }
            return dt.AsDataView();
        }

        /// <summary>
        /// 获取空气质量日报新方法,南通市辖区区域专用方法(补全不存在数据的日期)
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="orderBy">排序</param>
        /// <param name="OorA">原始或审核</param>
        /// <returns></returns>
        public DataView GetAirQualityRegionDayReportNew(DateTime dtStart, DateTime dtEnd, string[] type, int pageSize
            , int pageNo, string OorA, out int recordTotal, string orderBy = "DateTime desc")
        {
            recordTotal = 0;
            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            PortDayAQIDAL m_PortAQIDAL = Singleton<PortDayAQIDAL>.GetInstance();
            DataTable dt = new DataTable();
            dt = m_PortAQIDAL.GetRegionDataPagerNew(dtStart, dtEnd, type, pageSize, pageNo, OorA, out recordTotal, orderBy).Table;
            dt.Columns.Add("PortName", typeof(string));
            dt.Columns.Add("OrderNew", typeof(int));
            dt.Columns.Add("DateTimeNew", typeof(DateTime));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["AQIValue"] != DBNull.Value && dt.Rows[i]["AQIValue"].ToString().Trim() != "")
                {
                    if (int.Parse(dt.Rows[i]["AQIValue"].ToString()) <= 50)
                    {
                        dt.Rows[i]["OrderNew"] = "1";
                    }
                    else if (int.Parse(dt.Rows[i]["AQIValue"].ToString()) <= 100)
                    {
                        dt.Rows[i]["OrderNew"] = "2";
                    }
                    else if (int.Parse(dt.Rows[i]["AQIValue"].ToString()) <= 150)
                    {
                        dt.Rows[i]["OrderNew"] = "3";
                    }
                    else if (int.Parse(dt.Rows[i]["AQIValue"].ToString()) <= 200)
                    {
                        dt.Rows[i]["OrderNew"] = "4";
                    }
                    else if (int.Parse(dt.Rows[i]["AQIValue"].ToString()) <= 300)
                    {
                        dt.Rows[i]["OrderNew"] = "5";
                    }
                    else
                    {
                        dt.Rows[i]["OrderNew"] = "6";
                    }
                }
                else
                {
                    dt.Rows[i]["OrderNew"] = "7";
                }
                if (dt.Rows[i]["DateTime"] != DBNull.Value)
                {
                    dt.Rows[i]["DateTimeNew"] = DateTime.Parse(dt.Rows[i]["DateTime"].ToString());
                }
            }
            return dt.AsDataView();
        }

        /// <summary>
        /// 获取空气质量原始数据日报新方法
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        public DataView GetAirQualityOriDayReportNew(string[] portIds, DateTime dtStart, DateTime dtEnd, string[] type, int pageSize
            , int pageNo, out int recordTotal, string orderBy = "DateTime,PointId")
        {
            recordTotal = 0;
            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            DataTable dt = new DataTable();
            dt = GetPortOriDataPagerNew(portIds, dtStart, dtEnd, type, pageSize, pageNo, out recordTotal, orderBy).Table;
            dt.Columns.Add("PortName", typeof(string));
            dt.Columns.Add("OrderNew", typeof(int));
            dt.Columns.Add("DateTimeNew", typeof(DateTime));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int pointId = Convert.ToInt32(dt.Rows[i]["PointId"]);
                dt.Rows[i]["PortName"] = g_MonitoringPointAir.RetrieveEntityByPointId(pointId).MonitoringPointName;
                if (dt.Rows[i]["AQIValue"] != DBNull.Value && dt.Rows[i]["AQIValue"].ToString().Trim() != "")
                {
                    if (int.Parse(dt.Rows[i]["AQIValue"].ToString()) <= 50)
                    {
                        dt.Rows[i]["OrderNew"] = "1";
                    }
                    else if (int.Parse(dt.Rows[i]["AQIValue"].ToString()) <= 100)
                    {
                        dt.Rows[i]["OrderNew"] = "2";
                    }
                    else if (int.Parse(dt.Rows[i]["AQIValue"].ToString()) <= 150)
                    {
                        dt.Rows[i]["OrderNew"] = "3";
                    }
                    else if (int.Parse(dt.Rows[i]["AQIValue"].ToString()) <= 200)
                    {
                        dt.Rows[i]["OrderNew"] = "4";
                    }
                    else if (int.Parse(dt.Rows[i]["AQIValue"].ToString()) <= 300)
                    {
                        dt.Rows[i]["OrderNew"] = "5";
                    }
                    else
                    {
                        dt.Rows[i]["OrderNew"] = "6";
                    }
                }
                else
                {
                    dt.Rows[i]["OrderNew"] = "7";
                }
                if (dt.Rows[i]["DateTime"] != DBNull.Value)
                {
                    dt.Rows[i]["DateTimeNew"] = DateTime.Parse(dt.Rows[i]["DateTime"].ToString());
                }
            }
            return dt.AsDataView();
        }

        public DataView GetPortDataPagerNew(string[] portIds, DateTime dtStart, DateTime dtEnd, string[] type, int pageSize
         , int pageNo, out int recordTotal, string orderBy = "DateTime,PointId")
        {
            pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
            recordTotal = 0;
            if (pointDayAQI != null)
                return pointDayAQI.GetDataPagerNew(portIds, dtStart, dtEnd, type, pageSize, pageNo, out recordTotal, orderBy);
            return null;
        }

        public DataView GetPortOriDataPagerNew(string[] portIds, DateTime dtStart, DateTime dtEnd, string[] type, int pageSize
         , int pageNo, out int recordTotal, string orderBy = "DateTime,PointId")
        {
            pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
            recordTotal = 0;
            if (pointDayAQI != null)
                return pointDayAQI.GetOriDataPagerNew(portIds, dtStart, dtEnd, type, pageSize, pageNo, out recordTotal, orderBy);
            return null;
        }

        /// <summary>
        /// 获取空气质量日报新方法
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        public DataView GetRegionAirQualityDayReportNew(string[] regionGuids, DateTime dtStart, DateTime dtEnd, string[] type, int pageSize
            , int pageNo, out int recordTotal, string orderBy = "ReportDateTime,MonitoringRegionUid")
        {
            recordTotal = 0;
            DictionaryService dictionary = new DictionaryService();
            DataTable dt = new DataTable();
            dt = GetAreaDataPagerNew(regionGuids, dtStart, dtEnd, type, pageSize, pageNo, out recordTotal, orderBy).Table;
            dt.Columns.Add("RegionName", typeof(string));
            dt.Columns.Add("OrderNew", typeof(int));
            dt.Columns.Add("DateTimeNew", typeof(DateTime));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string regionUid = dt.Rows[i]["MonitoringRegionUid"].ToString();
                dt.Rows[i]["RegionName"] = dictionary.GetTextByValue(Core.Enums.DictionaryType.Air, "行政区划", regionUid);
                if (dt.Rows[i]["AQIValue"] != DBNull.Value)
                {
                    if (int.Parse(dt.Rows[i]["AQIValue"].ToString()) <= 50)
                    {
                        dt.Rows[i]["OrderNew"] = "1";
                    }
                    else if (int.Parse(dt.Rows[i]["AQIValue"].ToString()) <= 100)
                    {
                        dt.Rows[i]["OrderNew"] = "2";
                    }
                    else if (int.Parse(dt.Rows[i]["AQIValue"].ToString()) <= 150)
                    {
                        dt.Rows[i]["OrderNew"] = "3";
                    }
                    else if (int.Parse(dt.Rows[i]["AQIValue"].ToString()) <= 200)
                    {
                        dt.Rows[i]["OrderNew"] = "4";
                    }
                    else if (int.Parse(dt.Rows[i]["AQIValue"].ToString()) <= 300)
                    {
                        dt.Rows[i]["OrderNew"] = "5";
                    }
                    else
                    {
                        dt.Rows[i]["OrderNew"] = "6";
                    }
                }
                else
                {
                    dt.Rows[i]["OrderNew"] = "7";
                }
                if (dt.Rows[i]["ReportDateTime"] != DBNull.Value)
                {
                    dt.Rows[i]["DateTimeNew"] = DateTime.Parse(dt.Rows[i]["ReportDateTime"].ToString());
                }
            }
            return dt.AsDataView();
        }


        public DataView GetAreaDataPagerNew(string[] regionGuids, DateTime dtStart, DateTime dtEnd, string[] type, int pageSize
       , int pageNo, out int recordTotal, string orderBy = "ReportDateTime,MonitoringRegionUid")
        {
            regionDayAQI = Singleton<RegionDayAQIRepository>.GetInstance();
            recordTotal = 0;
            if (regionDayAQI != null)
                return regionDayAQI.GetDataPagerNew(regionGuids, dtStart, dtEnd, type, pageSize, pageNo, out recordTotal, orderBy);
            return null;
        }

    }
}
