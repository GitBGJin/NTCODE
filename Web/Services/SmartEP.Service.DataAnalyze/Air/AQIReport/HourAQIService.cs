﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartEP.MonitoringBusinessRepository.Air;
using System.Data;
using SmartEP.Core.Enums;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.DataAnalyze.Interfaces;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Core.Interfaces;
using SmartEP.Core.Generic;
using SmartEP.Service.Frame;
using log4net;
using SmartEP.Data.SqlServer.MonitoringBusiness.Air;

namespace SmartEP.Service.DataAnalyze.Air.AQIReport
{
    /// <summary>
    /// 名称：HourAQIService.cs
    /// 创建人：窦曙健
    /// 创建日期：2015-08-19
    /// 维护人员：
    /// 最新维护人员：徐阳   
    /// 最新维护日期：2017-06-02
    /// 功能摘要：小时AQI
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class HourAQIService : IHourAQI
    {
        /// <summary>
        /// 测点类
        /// </summary>
        MonitoringPointAirService g_MonitoringPointAir = null;

        /// <summary>
        /// 测点小时AQI
        /// </summary>
        HourAQIRepository pointHourAQI = null;

        /// <summary>
        /// 区域小时AQI
        /// </summary>
        RegionHourAQIRepository regionHourAQI = null;

        ILog log = LogManager.GetLogger("FileLogging");//获取一个日志记录器

        #region 根据站点统计
        /// <summary>
        /// 根据key主键判断记录是否存在
        /// </summary>
        /// <param name="strKey">主键值</param>
        /// <returns></returns>
        public bool PIsExist(string strKey)
        {
            pointHourAQI = new HourAQIRepository();
            if (pointHourAQI != null)
                return pointHourAQI.IsExist(strKey);
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
            pointHourAQI = Singleton<HourAQIRepository>.GetInstance();
            if (pointHourAQI != null)
                return pointHourAQI.GetGradeStatistics(aqiType, portIds, dtStart, dtEnd);
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
            pointHourAQI = Singleton<HourAQIRepository>.GetInstance();
            if (pointHourAQI != null)
                return pointHourAQI.Retrieve(p => p.PointId == Point.PointId && p.DateTime >= dtStart && p.DateTime <= dtEnd && !string.IsNullOrEmpty(p.AQIValue)).Count();
            return 0;
        }

        /// <summary>
        /// 获取超标天数
        /// </summary>
        /// <param name="Ponint">站点</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <returns></returns>
        public int GetPortOutDays(MonitoringPointEntity Point, DateTime dtStart, DateTime dtEnd)
        {
            pointHourAQI = Singleton<HourAQIRepository>.GetInstance();
            if (pointHourAQI != null)
                return pointHourAQI.Retrieve(p => p.PointId == Point.PointId && p.DateTime >= dtStart && p.DateTime <= dtEnd && Convert.ToInt32(p.AQIValue) > 100).Count();
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
        /// <returns>
        /// 
        /// </returns>
        public DataView GetPortDataPager(string[] portIds, DateTime dtStart, DateTime dtEnd, int pageSize
            , int pageNo, out int recordTotal, string orderBy = "DateTime,PointId")
        {
            pointHourAQI = Singleton<HourAQIRepository>.GetInstance();
            recordTotal = 0;
            if (pointHourAQI != null)
                return pointHourAQI.GetDataPager(portIds, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
            return null;
        }

        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据),计算原始小时数据AQI
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页（从0开始）</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns>
        /// 
        /// </returns>
        public DataView GetPortOriDataPager(string[] portIds, DateTime dtStart, DateTime dtEnd, int pageSize
            , int pageNo, out int recordTotal, string orderBy = "DateTime,PointId")
        {
            pointHourAQI = Singleton<HourAQIRepository>.GetInstance();
            recordTotal = 0;
            if (pointHourAQI != null)
                return pointHourAQI.GetOriDataPager(portIds, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
            return null;
        }

        /// <summary>
        /// 取得全月小时数
        /// </summary>
        /// <param name="autoMonitorType">数据类型</param>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetAvgDayData(string[] portIds, DateTime dateStart, DateTime dateEnd)
        {
            pointHourAQI = Singleton<HourAQIRepository>.GetInstance();
            if (pointHourAQI != null)
                return pointHourAQI.GetAvgDayData(portIds, dateStart, dateEnd);
            return null;
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
            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            pointHourAQI = Singleton<HourAQIRepository>.GetInstance();
            if (pointHourAQI != null)
            {
                DataTable dt = pointHourAQI.GetExportData(portIds, dtStart, dtEnd, orderBy).Table;
                dt.Columns.Add("portName", typeof(string));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int pointId = Convert.ToInt32(dt.Rows[i]["PointId"]);
                    dt.Rows[i]["PortName"] = g_MonitoringPointAir.RetrieveEntityByPointId(pointId).MonitoringPointName;
                }
                return dt.AsDataView();
            }
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
            pointHourAQI = Singleton<HourAQIRepository>.GetInstance();
            if (pointHourAQI != null)
                return pointHourAQI.GetContaminantsStatistics(aqiType, portIds, dtStart, dtEnd);
            return null;
        }

        /// <summary>
        /// 获取空气质量实时报
        /// </summary>
        /// <param name="portIds">测点</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="orderBy">排序</param>
        /// <returns>
        /// 返回字段：
        /// id：编号
        /// PointId：站点编号
        /// DateTime：数据时间
        /// SO2：SO2浓度
        /// SO2_IAQI：SO2IAQI分指数
        /// NO2：NO2浓度
        /// NO2_IAQI：NO2IAQI分指数
        /// PM10：PM10浓度
        /// PM20_IAQI：PM10IAQI分指数
        /// Recent24HoursPM10：最近24小时PM10浓度
        /// Recent24HoursPM10_IAQI：最近24小时PM10IAQI分指数
        /// CO：CO浓度
        /// CO_IAQI：COIAQI分指数
        /// O3：O3浓度
        /// O3_IAQI：O3IAQI分指数
        /// Recent8HoursO3：最近8小时O3浓度
        /// Recent8HoursO3_IAQI：最近8小时O3IAQI分指数
        /// PM25：PM2.5浓度
        /// PM25_IAQI：PM2.5IAQI分指数
        /// Recent24HoursPM25：最近24小时PM2.5浓度
        /// Recent24HoursPM25_IAQI：最近24小时PM2.5IAQI分指数
        /// AQIValue：AQI值
        /// PrimaryPollutant：首要污染物
        /// Range：指数等级范围
        /// RGBValue：RGB颜色值
        /// PicturePath：颜色对应的图片路径
        /// Class：空气质量指数类别
        /// Grade：空气质量指数级别
        /// HealthEffect：对健康影响情况
        /// TakeStep：建议采取的措施
        /// OrderByNum：排序
        /// Description：描述
        /// CreatUser：创建人
        /// CreatDateTime：创建时间
        /// UpdateUser：修改人
        /// UpdateDateTime：修改时间
        /// </returns>
        public DataView GetAirQualityRTReport(string[] portIds, DateTime dtStart, DateTime dtEnd, int pageSize
            , int pageNo, out int recordTotal, string orderBy = "DateTime,PointId")
        {
            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            recordTotal = 0;
            try
            {
                DataTable dt = GetPortDataPager(portIds, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy).Table;
                dt.Columns.Add("PortName", typeof(string));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (string.IsNullOrEmpty(dt.Rows[i]["PointId"].ToString()))
                    {
                        int pointId = Convert.ToInt32(dt.Rows[i]["PointId"]);
                        dt.Rows[i]["PortName"] = g_MonitoringPointAir.RetrieveEntityByPointId(pointId).MonitoringPointName;
                    }
                }
                return dt.AsDataView();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取空气质量原始小时数据实时报
        /// </summary>
        /// <param name="portIds">测点</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="orderBy">排序</param>
        /// <returns>
        /// 返回字段：
        /// id：编号
        /// PointId：站点编号
        /// DateTime：数据时间
        /// SO2：SO2浓度
        /// SO2_IAQI：SO2IAQI分指数
        /// NO2：NO2浓度
        /// NO2_IAQI：NO2IAQI分指数
        /// PM10：PM10浓度
        /// PM20_IAQI：PM10IAQI分指数
        /// Recent24HoursPM10：最近24小时PM10浓度
        /// Recent24HoursPM10_IAQI：最近24小时PM10IAQI分指数
        /// CO：CO浓度
        /// CO_IAQI：COIAQI分指数
        /// O3：O3浓度
        /// O3_IAQI：O3IAQI分指数
        /// Recent8HoursO3：最近8小时O3浓度
        /// Recent8HoursO3_IAQI：最近8小时O3IAQI分指数
        /// PM25：PM2.5浓度
        /// PM25_IAQI：PM2.5IAQI分指数
        /// Recent24HoursPM25：最近24小时PM2.5浓度
        /// Recent24HoursPM25_IAQI：最近24小时PM2.5IAQI分指数
        /// AQIValue：AQI值
        /// PrimaryPollutant：首要污染物
        /// Range：指数等级范围
        /// RGBValue：RGB颜色值
        /// PicturePath：颜色对应的图片路径
        /// Class：空气质量指数类别
        /// Grade：空气质量指数级别
        /// HealthEffect：对健康影响情况
        /// TakeStep：建议采取的措施
        /// OrderByNum：排序
        /// Description：描述
        /// CreatUser：创建人
        /// CreatDateTime：创建时间
        /// UpdateUser：修改人
        /// UpdateDateTime：修改时间
        /// </returns>
        public DataView GetAirQualityOriRTReport(string[] portIds, DateTime dtStart, DateTime dtEnd, int pageSize
            , int pageNo, out int recordTotal, string orderBy = "DateTime,PointId")
        {
            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            recordTotal = 0;
            try
            {
                DataTable dt = GetPortOriDataPager(portIds, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy).Table;
                dt.Columns.Add("PortName", typeof(string));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (string.IsNullOrEmpty(dt.Rows[i]["PointId"].ToString()))
                    {
                        int pointId = Convert.ToInt32(dt.Rows[i]["PointId"]);
                        dt.Rows[i]["PortName"] = g_MonitoringPointAir.RetrieveEntityByPointId(pointId).MonitoringPointName;
                    }
                }
                return dt.AsDataView();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 针对南通数据需获取区域数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <param name="recordTotal"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetOriRTData(string[] portIds, DateTime dtStart, DateTime dtEnd, string orderBy, string isCheck)
        {
            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            AQICalculateService m_AQICalculateService = new AQICalculateService();
            try
            {
                PortHourAQIDAL m_PortAQIDAL = Singleton<PortHourAQIDAL>.GetInstance();
                DataTable dt = m_PortAQIDAL.GetOriRTData(portIds, dtStart, dtEnd, orderBy, isCheck).Table;
                dt.Columns.Add("PortName", typeof(string));

                //Double? SO2 = null;
                //Double? NO2 = null;
                //Double? PM10 = null;
                //Double? PM25 = null;
                //Double? CO = null;
                //Double? O3 = null;

                //int? PM25Value = null;
                //int? PM10Value = null;
                //int? NO2Value = null;
                //int? SO2Value = null;
                //int? COValue = null;
                //int? O3Value = null;

                double a;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["DateTime"] = Convert.ToDateTime(Convert.ToDateTime(dt.Rows[i]["DateTime"]).ToString("yyyy-MM-dd HH:00:00"));
                    if (string.IsNullOrEmpty(dt.Rows[i]["PointId"].ToString()))
                    {
                        int pointId = Convert.ToInt32(dt.Rows[i]["PointId"]);
                        dt.Rows[i]["PortName"] = g_MonitoringPointAir.RetrieveEntityByPointId(pointId).MonitoringPointName;
                    }
                    //if (isCheck == "1")
                    //{
                    //    if (double.TryParse(dt.Rows[i]["SO2"].ToString(), out a))
                    //    {
                    //        SO2 = Convert.ToDouble(dt.Rows[i]["SO2"].ToString());
                    //    }
                    //    if (double.TryParse(dt.Rows[i]["NO2"].ToString(), out a))
                    //    {
                    //        NO2 = Convert.ToDouble(dt.Rows[i]["NO2"].ToString());
                    //    }
                    //    if (double.TryParse(dt.Rows[i]["PM10"].ToString(), out a))
                    //    {
                    //        PM10 = Convert.ToDouble(dt.Rows[i]["PM10"].ToString());
                    //    }
                    //    if (double.TryParse(dt.Rows[i]["PM25"].ToString(), out a))
                    //    {
                    //        PM25 = Convert.ToDouble(dt.Rows[i]["PM25"].ToString());
                    //    }
                    //    if (double.TryParse(dt.Rows[i]["CO"].ToString(), out a))
                    //    {
                    //        CO = Convert.ToDouble(dt.Rows[i]["CO"].ToString());
                    //    }
                    //    if (double.TryParse(dt.Rows[i]["O3"].ToString(), out a))
                    //    {
                    //        O3 = Convert.ToDouble(dt.Rows[i]["O3"].ToString());
                    //    }

                    //    PM25Value = m_AQICalculateService.GetIAQI("a34004", PM25, 24);
                    //    PM10Value = m_AQICalculateService.GetIAQI("a34002", PM10, 24);
                    //    NO2Value = m_AQICalculateService.GetIAQI("a21004", NO2, 1);
                    //    SO2Value = m_AQICalculateService.GetIAQI("a21026", SO2, 1);
                    //    COValue = m_AQICalculateService.GetIAQI("a21005", CO, 1);
                    //    O3Value = m_AQICalculateService.GetIAQI("a05024", O3, 1);

                    //    string AQIValue = m_AQICalculateService.GetAQI_Max_CNV(SO2Value, NO2Value, PM10Value, COValue, O3Value, PM25Value, "V");
                    //    string PrimaryPollutant = m_AQICalculateService.GetAQI_Max_CNV(SO2Value, NO2Value, PM10Value, COValue, O3Value, PM25Value, "N");
                    //    dt.Rows[i]["SO2_IAQI"] = SO2Value;
                    //    dt.Rows[i]["NO2_IAQI"] = NO2Value;
                    //    dt.Rows[i]["PM10_IAQI"] = PM10Value;
                    //    dt.Rows[i]["CO_IAQI"] = COValue;
                    //    dt.Rows[i]["O3_IAQI"] = O3Value;
                    //    dt.Rows[i]["PM25_IAQI"] = PM25Value;
                    //    dt.Rows[i]["AQIValue"] = AQIValue;
                    //    dt.Rows[i]["PrimaryPollutant"] = PrimaryPollutant != "" ? PrimaryPollutant : "--";
                    //}
                }
                return dt.AsDataView();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取空气质量原始小时数据实时报,南通市辖区区域专用方法
        /// </summary>
        /// <param name="portIds">测点</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="orderBy">排序</param>
        /// <returns>
        /// </returns>
        public DataTable GetAirQualityRegionRTReport(DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, string OorA, out int recordTotal, string orderBy = "DateTime desc")
        {
            PortHourAQIDAL m_PortAQIDAL = Singleton<PortHourAQIDAL>.GetInstance();
            recordTotal = 0;
            try
            {
                DataTable dt = m_PortAQIDAL.GetRegionOriDataPager(dtStart, dtEnd, pageSize, pageNo, OorA, out recordTotal, orderBy);
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 查询测点最新一条原始小时数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <param name="recordTotal"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetAirQualityNewestOriRTReport(string[] portIds, int pageSize, int pageNo, out int recordTotal, DateTime dt, string orderBy = "DateTime,PointId")
        {
            try
            {
                pointHourAQI = Singleton<HourAQIRepository>.GetInstance();
                recordTotal = 0;
                if (pointHourAQI != null)
                    return pointHourAQI.GetAirQualityNewestOriRTReport(portIds, pageSize, pageNo, out recordTotal, dt, orderBy);
                return null;
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        /// 查询所选站点当天的小时AQI
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="dt2"></param>
        /// <param name="dt"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetOriRTAQI(string[] portIds, DateTime dt2, DateTime dt, string orderBy = "DateTime,PointId")
        {
            try
            {
                PortHourAQIDAL m_PortAQIDAL = Singleton<PortHourAQIDAL>.GetInstance();
                AQICalculateService m_AQICalculateService = new AQICalculateService();
                if (m_PortAQIDAL != null)
                {
                    DataView dv = m_PortAQIDAL.GetOriRTAQI(portIds, dt2, dt, orderBy);

                    Double? SO2 = null;
                    Double? NO2 = null;
                    Double? PM10 = null;
                    Double? PM25 = null;
                    Double? CO = null;
                    Double? O3 = null;

                    int? PM25Value = null;
                    int? PM10Value = null;
                    int? NO2Value = null;
                    int? SO2Value = null;
                    int? COValue = null;
                    int? O3Value = null;

                    double a;
                    foreach (DataRowView dr in dv)
                    {
                        if (double.TryParse(dr["SO2"].ToString(), out a))
                        {
                            SO2 = Convert.ToDouble(dr["SO2"].ToString());
                        }
                        if (double.TryParse(dr["NO2"].ToString(), out a))
                        {
                            NO2 = Convert.ToDouble(dr["NO2"].ToString());
                        }
                        if (double.TryParse(dr["PM10"].ToString(), out a))
                        {
                            PM10 = Convert.ToDouble(dr["PM10"].ToString());
                        }
                        if (double.TryParse(dr["PM25"].ToString(), out a))
                        {
                            PM25 = Convert.ToDouble(dr["PM25"].ToString());
                        }
                        if (double.TryParse(dr["CO"].ToString(), out a))
                        {
                            CO = Convert.ToDouble(dr["CO"].ToString());
                        }
                        if (double.TryParse(dr["O3"].ToString(), out a))
                        {
                            O3 = Convert.ToDouble(dr["O3"].ToString());
                        }

                        PM25Value = m_AQICalculateService.GetIAQI("a34004", PM25, 24);
                        PM10Value = m_AQICalculateService.GetIAQI("a34002", PM10, 24);
                        NO2Value = m_AQICalculateService.GetIAQI("a21004", NO2, 1);
                        SO2Value = m_AQICalculateService.GetIAQI("a21026", SO2, 1);
                        COValue = m_AQICalculateService.GetIAQI("a21005", CO, 1);
                        O3Value = m_AQICalculateService.GetIAQI("a05024", O3, 1);

                        string AQIValue = m_AQICalculateService.GetAQI_Max_CNV(SO2Value, NO2Value, PM10Value, COValue, O3Value, PM25Value, "V");
                        dr["AQIValue"] = AQIValue;
                    }
                    return dv;
                }
                return null;
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        /// 获取站点最新数据的时间
        /// </summary>
        /// <param name="portIds"></param>
        /// <returns></returns>
        public DateTime GetOriAQINewestTime(string[] portIds)
        {
            PortHourAQIDAL m_PortAQIDAL = Singleton<PortHourAQIDAL>.GetInstance();
            DateTime time = m_PortAQIDAL.GetOriAQINewestTime(portIds);
            return time;
        }

        /// <summary>
        /// 实时空气质量
        /// </summary>
        /// <param name="portIds">站点Id</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>
        /// 返回字段：PointId,DateTime,SO2,SO2_IAQI,NO2,NO2_IAQI,PM10,PM10_IAQI,Recent24HoursPM10,Recent24HoursPM10_IAQI,
        /// CO,CO_IAQI,O3,O3_IAQI,Recent8HoursO3,Recent8HoursO3_IAQI,PM25,PM25_IAQI,Recent24HoursPM25,Recent24HoursPM25_IAQI,
        /// AQIValue,PrimaryPollutant,Range,RGBValue,PicturePath,Class,Grade,HealthEffect,TakeStep
        /// </returns>
        public DataView RealTimeAirPointsQuality(string[] portIds, DateTime dateStart, DateTime dateEnd)
        {
            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            pointHourAQI = Singleton<HourAQIRepository>.GetInstance();
            DataTable dt = pointHourAQI.GetLastData(portIds, dateStart, dateEnd).Table;
            dt.Columns.Add("PortName", typeof(string));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(dt.Rows[i]["PointId"].ToString()))
                {
                    int pointId = Convert.ToInt32(dt.Rows[i]["PointId"]);
                    dt.Rows[i]["PortName"] = g_MonitoringPointAir.RetrieveEntityByPointId(pointId).MonitoringPointName;
                }
            }
            return dt.AsDataView();
        }
        #endregion

        #region 根据区域统计
        /// <summary>
        /// 根据key主键判断记录是否存在
        /// </summary>
        /// <param name="strKey">主键值</param>
        /// <returns></returns>
        public bool RegionIsExist(string strKey)
        {
            regionHourAQI = Singleton<RegionHourAQIRepository>.GetInstance();
            if (regionHourAQI != null)
                return regionHourAQI.IsExist(strKey);
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
        public DataView GetRegionGradeStatistics(IAQIType aqiType, string[] regionGuids, DateTime dtStart, DateTime dtEnd)
        {
            regionHourAQI = Singleton<RegionHourAQIRepository>.GetInstance();
            if (regionHourAQI != null)
                return regionHourAQI.GetGradeStatistics(aqiType, regionGuids, dtStart, dtEnd);
            return null;
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
        /// <returns>
        /// 返回字段：
        /// id：编号
        /// MonitoringRegionUid：监测区域Uid
        /// DateTime：数据时间
        /// SO2：SO2浓度
        /// SO2_IAQI：SO2IAQI分指数
        /// NO2：NO2浓度
        /// NO2_IAQI：NO2IAQI分指数
        /// PM10：PM10浓度
        /// PM20_IAQI：PM10IAQI分指数
        /// Recent24HoursPM10：最近24小时PM10浓度
        /// Recent24HoursPM10_IAQI：最近24小时PM10IAQI分指数
        /// CO：CO浓度
        /// CO_IAQI：COIAQI分指数
        /// O3：O3浓度
        /// O3_IAQI：O3IAQI分指数
        /// Recent8HoursO3：最近8小时O3浓度
        /// Recent8HoursO3_IAQI：最近8小时O3IAQI分指数
        /// PM25：PM2.5浓度
        /// PM25_IAQI：PM2.5IAQI分指数
        /// Recent24HoursPM25：最近24小时PM2.5浓度
        /// Recent24HoursPM25_IAQI：最近24小时PM2.5IAQI分指数
        /// AQIValue：AQI值
        /// PrimaryPollutant：首要污染物
        /// Range：指数等级范围
        /// RGBValue：RGB颜色值
        /// PicturePath：颜色对应的图片路径
        /// Class：空气质量指数类别
        /// Grade：空气质量指数级别
        /// HealthEffect：对健康影响情况
        /// TakeStep：建议采取的措施
        /// OrderByNum：排序
        /// Description：描述
        /// CreatUser：创建人
        /// CreatDateTime：创建时间
        /// UpdateUser：修改人
        /// UpdateDateTime：修改时间
        /// </returns>
        public DataView GetRegionDataPager(string[] regionGuids, DateTime dtStart, DateTime dtEnd, int pageSize
            , int pageNo, out int recordTotal, string orderBy = "DateTime,MonitoringRegionUid")
        {
            DictionaryService dictionary = new DictionaryService();
            regionHourAQI = Singleton<RegionHourAQIRepository>.GetInstance();
            recordTotal = 0;
            if (regionHourAQI != null)
            {
                DataTable dt = regionHourAQI.GetDataPager(regionGuids, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy).Table;
                dt.Columns.Add("RegionName", typeof(string));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string RegionName = dictionary.GetCodeDictionaryTextByValue(dt.Rows[i]["MonitoringRegionUid"].ToString());
                    dt.Rows[i]["RegionName"] = RegionName;
                }


                return dt.DefaultView;
            }
            return null;
        }

        /// <summary>
        /// 取得导出数据（行转列数据）
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetRegionExportData(string[] regionGuids, DateTime dtStart, DateTime dtEnd, string orderBy = "DateTime,MonitoringRegionUid")
        {
            regionHourAQI = Singleton<RegionHourAQIRepository>.GetInstance();
            if (regionHourAQI != null)
                return regionHourAQI.GetExportData(regionGuids, dtStart, dtEnd, orderBy);
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
        public DataView GetRegionContaminantsStatistics(IAQIType aqiType, string[] regionGuids, DateTime dtStart, DateTime dtEnd)
        {
            regionHourAQI = Singleton<RegionHourAQIRepository>.GetInstance();
            if (regionHourAQI != null)
                return regionHourAQI.GetContaminantsStatistics(aqiType, regionGuids, dtStart, dtEnd);

            return null;
        }

        /// <summary>
        /// 实时空气质量
        /// </summary>
        /// <param name="regionGuids">区域Id</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>
        /// 返回字段：MonitoringRegionUid,DateTime,SO2,SO2_IAQI,NO2,NO2_IAQI,PM10,PM10_IAQI,Recent24HoursPM10,Recent24HoursPM10_IAQI,
        /// CO,CO_IAQI,O3,O3_IAQI,Recent8HoursO3,Recent8HoursO3_IAQI,PM25,PM25_IAQI,Recent24HoursPM25,Recent24HoursPM25_IAQI,
        /// AQIValue,PrimaryPollutant,Range,RGBValue,PicturePath,Class,Grade,HealthEffect,TakeStep
        /// </returns>
        public DataView RealTimeAirRegionsQuality(string[] regionGuids, DateTime dateStart, DateTime dateEnd)
        {
            DictionaryService dictionary = new DictionaryService();
            regionHourAQI = Singleton<RegionHourAQIRepository>.GetInstance();
            DataTable dt = regionHourAQI.GetLastData(regionGuids, dateStart, dateEnd).Table;
            dt.Columns.Add("RegionName", typeof(string));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(dt.Rows[i]["MonitoringRegionUid"].ToString()))
                {
                    string regionUid = dt.Rows[i]["MonitoringRegionUid"].ToString();
                    dt.Rows[i]["RegionName"] = dictionary.GetCodeDictionaryTextByValue(regionUid);
                }
            }
            return dt.AsDataView();
        }

        /// <summary>
        /// 获取空气质量实时报
        /// </summary>
        /// <param name="portIds">测点</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="orderBy">排序</param>
        /// <returns>
        /// 返回字段：
        /// id：编号
        /// regionGuids：区域编号
        /// DateTime：数据时间
        /// SO2：SO2浓度
        /// SO2_IAQI：SO2IAQI分指数
        /// NO2：NO2浓度
        /// NO2_IAQI：NO2IAQI分指数
        /// PM10：PM10浓度
        /// PM20_IAQI：PM10IAQI分指数
        /// Recent24HoursPM10：最近24小时PM10浓度
        /// Recent24HoursPM10_IAQI：最近24小时PM10IAQI分指数
        /// CO：CO浓度
        /// CO_IAQI：COIAQI分指数
        /// O3：O3浓度
        /// O3_IAQI：O3IAQI分指数
        /// Recent8HoursO3：最近8小时O3浓度
        /// Recent8HoursO3_IAQI：最近8小时O3IAQI分指数
        /// PM25：PM2.5浓度
        /// PM25_IAQI：PM2.5IAQI分指数
        /// Recent24HoursPM25：最近24小时PM2.5浓度
        /// Recent24HoursPM25_IAQI：最近24小时PM2.5IAQI分指数
        /// AQIValue：AQI值
        /// PrimaryPollutant：首要污染物
        /// Range：指数等级范围
        /// RGBValue：RGB颜色值
        /// PicturePath：颜色对应的图片路径
        /// Class：空气质量指数类别
        /// Grade：空气质量指数级别
        /// HealthEffect：对健康影响情况
        /// TakeStep：建议采取的措施
        /// OrderByNum：排序
        /// Description：描述
        /// CreatUser：创建人
        /// CreatDateTime：创建时间
        /// UpdateUser：修改人
        /// UpdateDateTime：修改时间
        /// </returns>
        public DataView GetRegionAirQualityRTReport(string[] regionGuids, DateTime dtStart, DateTime dtEnd, int pageSize
            , int pageNo, out int recordTotal, string orderBy = "DateTime,MonitoringRegionUid")
        {
            DictionaryService dictionary = new DictionaryService();
            recordTotal = 0;
            try
            {
                DataTable dt = GetRegionDataPager(regionGuids, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy).Table;
                if (!dt.Columns.Contains("RegionName"))
                {
                    dt.Columns.Add("RegionName", typeof(string));
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (string.IsNullOrEmpty(dt.Rows[i]["MonitoringRegionUid"].ToString()))
                    {
                        string regionUid = dt.Rows[i]["MonitoringRegionUid"].ToString();
                        dt.Rows[i]["RegionName"] = dictionary.GetTextByValue(Core.Enums.DictionaryType.Air, "行政区划", regionUid);
                    }
                }
                return dt.AsDataView();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region 接口实现
        /// <summary>
        /// 获取实时小时某一监测点AQI
        /// </summary>
        /// <returns></returns>
        public int GetRTHourPortAQI()
        {
            return 0;
        }

        /// <summary>
        /// 获取实时小时苏州市区所有监测点AQI
        /// </summary>
        /// <returns></returns>
        public int GetRTHourSzPortsAQI()
        {
            return 0;
        }

        /// <summary>
        /// 获取实时小时某一城区所有监测点AQI
        /// </summary>
        /// <returns></returns>
        public int GetRTHourAreaPortsAQI()
        {
            return 0;
        }

        /// <summary>
        /// 获取实时小时某一监测点AQI污染等级统计
        /// </summary>
        /// <returns></returns>
        public int GetRTHourPortAQIGrade()
        {
            return 0;
        }

        /// <summary>
        /// 获取实时小时苏州市区所有监测点AQI污染等级统计
        /// </summary>
        /// <returns></returns>
        public int GetRTHourSzPortsAQIGrade()
        {
            return 0;
        }

        /// <summary>
        /// 获取实时小时某一城区所有监测点AQI污染等级统计
        /// </summary>
        /// <returns></returns>
        public int GetRTHourAreaPortsAQIGrade()
        {
            return 0;
        }

        /// <summary>
        /// 获取实时小时某一监测点首要污染物统计
        /// </summary>
        /// <returns></returns>
        public int GetRTHourPortPrimaryPollutant()
        {
            return 0;
        }

        /// <summary>
        /// 获取实时小时苏州市区所有监测点首要污染物统计
        /// </summary>
        /// <returns></returns>
        public int GetRTHourSzPortsPrimaryPollutant()
        {
            return 0;
        }

        /// <summary>
        /// 获取实时小时某一城区所有监测点首要污染物统计
        /// </summary>
        /// <returns></returns>
        public int GetRTHourAreaPortsPrimaryPollutant()
        {
            return 0;
        }
        #endregion

        #region 空气质量日报统计专用方法
        /// <summary>
        /// 获取时间段内指定测点的小时数据
        /// </summary>
        /// <param name="PointIds">测点ID数组</param>
        /// <param name="StartDate">开始日期</param>
        /// <param name="EndDate">结束日期</param>
        /// <returns>DataTable</returns>
        public DataTable GetHourAQIData(List<int> PointIds, DateTime StartDate, DateTime EndDate)
        {
            pointHourAQI = Singleton<HourAQIRepository>.GetInstance();
            return pointHourAQI.GetHourAQIData(PointIds, StartDate, EndDate);
        }

        /// <summary>
        /// 计算时间段内指定测点监测因子的浓度及分指数平均值
        /// </summary>
        /// <param name="PointIds">测点ID数组</param>
        /// <param name="StartDate">开始日期</param>
        /// <param name="EndDate">结束日期</param>
        /// <returns>DataTable</returns>
        public DataTable GetAvgHourAQIData(List<int> PointIds, DateTime StartDate, DateTime EndDate)
        {
            pointHourAQI = Singleton<HourAQIRepository>.GetInstance();
            return pointHourAQI.GetAvgHourAQIData(PointIds, StartDate, EndDate);
        }
        #endregion
    }
}
