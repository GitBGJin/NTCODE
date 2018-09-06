using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.Data.Enums;
using SmartEP.Data.SqlServer.MonitoringBusiness.Air;
using SmartEP.DomainModel.MonitoringBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SmartEP.MonitoringBusinessRepository.Air
{
    /// <summary>
    /// 名称：RegionDayAQIRepository.cs
    /// 创建人：李飞
    /// 创建日期：2015-08-17
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 区域、全市、区县日AQI仓储层
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class RegionDayAQIRepository : BaseGenericRepository<MonitoringBusinessModel, RegionDayAQIReportEntity>
    {
        /// <summary>
        /// 根据key主键判断记录是否存在
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public override bool IsExist(string strKey)
        {
            return true;
        }

        #region << ADO.NET >>
        /// <summary>
        /// 数据处理DAL类
        /// </summary>
        RegionDayAQIDAL m_RegionAQIDAL = Singleton<RegionDayAQIDAL>.GetInstance();

        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <param name="orderBy">排序字段(MonitoringRegionUid,DateTime)</param>
        /// <returns></returns>
        public DataView GetDataPager(string[] regionGuids, DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "MonitoringRegionUid,ReportDateTime", RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            return m_RegionAQIDAL.GetDataPager(regionGuids, dtStart, dtEnd, pageSize, pageNo, out recordTotal, regionAQIStatisticalType, orderBy);
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <param name="orderBy">排序字段(MonitoringRegionUid,DateTime)</param>
        /// <returns></returns>
        public DataView GetDataBasePager(string[] regionGuids, DateTime dtStart, DateTime dtEnd)
        {
            return m_RegionAQIDAL.GetDataBasePager(regionGuids, dtStart, dtEnd);
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <param name="orderBy">排序字段(MonitoringRegionUid,DateTime)</param>
        /// <returns></returns>
        public DataView GetDataPager(string[] regionGuids, DateTime dtStart, DateTime dtEnd, string[] type, int pageSize, int pageNo, out int recordTotal, string orderBy = "MonitoringRegionUid,ReportDateTime", RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            return m_RegionAQIDAL.GetDataPager(regionGuids, dtStart, dtEnd, type, pageSize, pageNo, out recordTotal, regionAQIStatisticalType, orderBy);
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)新方法
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <param name="orderBy">排序字段(MonitoringRegionUid,DateTime)</param>
        /// <returns></returns>
        public DataView GetDataPagerNew(string[] regionGuids, DateTime dtStart, DateTime dtEnd, string[] type, int pageSize, int pageNo, out int recordTotal, string orderBy = "MonitoringRegionUid,ReportDateTime", RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            return m_RegionAQIDAL.GetDataPagerNew(regionGuids, dtStart, dtEnd, type, pageSize, pageNo, out recordTotal, regionAQIStatisticalType, orderBy);
        }
        /// <summary>
        /// 市区空气质量
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <param name="orderBy">排序字段(MonitoringRegionUid,DateTime)</param>
        /// <returns></returns>
        public DataView GetDataMonthPager(DateTime dtStart, DateTime dtEnd)
        {
            return m_RegionAQIDAL.GetDataMonthPager(dtStart, dtEnd);
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <param name="orderBy">排序字段(MonitoringRegionUid,DateTime)</param>
        /// <returns></returns>
        public DataView GetAreaDataPager(string[] regionGuids,  DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "MonitoringRegionUid", RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            return m_RegionAQIDAL.GetAreaDataPager(regionGuids, dtStart, dtEnd, pageSize, pageNo, out recordTotal, regionAQIStatisticalType, orderBy);
        }

        /// <summary>
        /// 获取苏州大市AQI相关数据信息
        /// </summary>
        /// <param name="BeginTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        public DataView GetDayDataPager(DateTime BeginTime, DateTime EndTime)
        {
            return m_RegionAQIDAL.GetDayDataPager(BeginTime, EndTime);
        }

        /// <summary>
        /// 获取区域AQI相关数据信息
        /// </summary>
        /// <param name="BeginTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        public DataView GetAQIBaseInfo(DateTime BeginTime, DateTime EndTime)
        {
           DataView dv= m_RegionAQIDAL.GetAQIBaseInfo(BeginTime, EndTime);
           return dv;
        }
        /// <summary>
        /// 获取测点AQI相关数据信息
        /// </summary>
        /// <param name="BeginTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        public DataView GetPointAQIDayBase(DateTime BeginTime, DateTime EndTime,int pointId,string dataType)
        {
            DataView dv = m_RegionAQIDAL.GetPointAQIDayBase(BeginTime, EndTime, pointId,dataType);
            return dv;
        }
        /// <summary>
        /// 获取苏州大市浓度AQI日数据范围信息
        /// </summary>
        /// <param name="BeginTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        public DataView GetAQIDayRange(DateTime BeginTime, DateTime EndTime)
        {
            return m_RegionAQIDAL.GetAQIDayRange(BeginTime, EndTime);
        }
        /// <summary>
        /// 全市均值达标天数统计
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <returns>
        /// 返回字段：
        /// PointId：站点ID
        /// ValidCount：有效天
        /// OverCount：达标天
        /// </returns>
        public DataView GetMonthReportData(string[] regionGuids, DateTime dateStart, DateTime dateEnd, RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            return m_RegionAQIDAL.GetMonthReportData(regionGuids, dateStart, dateEnd, regionAQIStatisticalType);
        }

        /// <summary>
        /// 取得导出数据(行转列数据)
        /// </summary>
        /// <param name="regionGuids">测点数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <param name="orderBy">排序字段(MonitoringRegionUid,DateTime)</param>
        /// <returns></returns>
        public DataView GetExportData(string[] regionGuids, DateTime dtStart, DateTime dtEnd, string orderBy = "MonitoringRegionUid,ReportDateTime", RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            return m_RegionAQIDAL.GetExportData(regionGuids, dtStart, dtEnd, regionAQIStatisticalType, orderBy);
        }

        /// <summary>
        /// 根据站点取得最新小时AQI数据
        /// </summary>
        /// <param name="regions">区域</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <returns></returns>
        public DataView GetLastData(string[] regions, DateTime dateStart, DateTime dateEnd, RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            return m_RegionAQIDAL.GetLastData(regions, dateStart, dateEnd, regionAQIStatisticalType);
        }

        /// <summary>
        /// 各等级天数统计
        /// </summary>
        /// <param name="aqiType">AQI分指标字段</param>
        /// <param name="regionGuids">测点</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <returns></returns>
        public DataView GetGradeStatistics(IAQIType aqiType, string[] regionGuids, DateTime dateStart, DateTime dateEnd, RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            return m_RegionAQIDAL.GetGradeStatistics(aqiType, regionGuids, dateStart, dateEnd, regionAQIStatisticalType);
        }
        /// <summary>
        ///市区无效天数日期
        /// </summary>
        /// <param name="aqiType">AQI分指标字段</param>
        /// <param name="regionGuids">测点</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <returns></returns>
        public DataView GetMonthReportTimeDataTable(DateTime dateStart, DateTime dateEnd, RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            return m_RegionAQIDAL.GetMonthReportTimeDataTable(dateStart, dateEnd, regionAQIStatisticalType);
        }
        /// <summary>
        /// 各污染物首要污染物统计
        /// </summary>
        /// <param name="aqiType">AQI分指标字段</param>
        /// <param name="regionGuids">测点</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <returns></returns>
        public DataView GetContaminantsStatistics(IAQIType aqiType, string[] regionGuids, DateTime dateStart, DateTime dateEnd, RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            return m_RegionAQIDAL.GetContaminantsStatistics(aqiType, regionGuids, dateStart, dateEnd, regionAQIStatisticalType);
        }

        /// <summary>
        /// 取得指定日期内日数据均值数据
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <returns>
        /// 返回字段：
        /// PointId：站点ID
        /// PM25：PM25浓度
        /// PM10：PM10浓度
        /// NO2：NO2浓度
        /// SO2：SO2浓度
        /// CO：CO浓度
        /// MaxOneHourO3：MaxOneHourO3浓度
        /// Max8HourO3：Max8HourO3浓度
        /// </returns>
        public DataView GetAvgValue(string[] regionGuids, DateTime dateStart, DateTime dateEnd, RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            return m_RegionAQIDAL.GetAvgValue(regionGuids, dateStart, dateEnd, regionAQIStatisticalType);
        }

        /// <summary>
        /// 取得指定日期内日数据最大值数据
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <returns>
        /// 返回字段：
        /// PointId：站点ID
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
        public DataView GetMaxValue(string[] regionGuids, DateTime dateStart, DateTime dateEnd, RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            return m_RegionAQIDAL.GetMaxValue(regionGuids, dateStart, dateEnd, regionAQIStatisticalType);
        }
        /// <summary>
        /// 取得指定日期内日数据最大值数据
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <returns>
        /// 返回字段：
        /// PointId：站点ID
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
        public DataView GetMaxValueOne(string[] regionGuids, DateTime dateStart, DateTime dateEnd, RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            return m_RegionAQIDAL.GetMaxValueOne(regionGuids, dateStart, dateEnd, regionAQIStatisticalType);
        }
        /// <summary>
        /// 取得指定日期内日数据最小值数据
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <returns>
        /// 返回字段：
        /// PointId：站点ID
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
        public DataView GetMinValue(string[] regionGuids, DateTime dateStart, DateTime dateEnd, RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            return m_RegionAQIDAL.GetMinValue(regionGuids, dateStart, dateEnd, regionAQIStatisticalType);
        }
        /// <summary>
        /// 取得指定日期内日数据最小值数据
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <returns>
        /// 返回字段：
        /// PointId：站点ID
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
        public DataView GetMinValueOne(string[] regionGuids, DateTime dateStart, DateTime dateEnd, RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            return m_RegionAQIDAL.GetMinValueOne(regionGuids, dateStart, dateEnd, regionAQIStatisticalType);
        }
        /// <summary>
        /// 日数据超标天数统计
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
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
        public DataView GetExceedingData(string[] regionGuids, DateTime dateStart, DateTime dateEnd, RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            return m_RegionAQIDAL.GetExceedingMonthData(regionGuids, dateStart, dateEnd, regionAQIStatisticalType);
        }

        /// <summary>
        /// 年报
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        public DataView GetRegionsAllData(string[] regions, DateTime dateStart, DateTime dateEnd, RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            return m_RegionAQIDAL.GetRegionsAllData(regions, dateStart, dateEnd, regionAQIStatisticalType);
        }
        /// <summary>
        /// 年报市区每月超标率
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        public DataView GetMonthAvgAllData(DateTime dateStart, DateTime dateEnd, RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            return m_RegionAQIDAL.GetMonthAvgAllData(dateStart, dateEnd, regionAQIStatisticalType);
        }
        /// <summary>
        /// 季报
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        public DataView GetRegionsSeasonData(DateTime dateStart, DateTime dateEnd, RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            return m_RegionAQIDAL.GetRegionsSeasonData(dateStart, dateEnd, regionAQIStatisticalType);
        }
        /// <summary>
        /// 日数据超标天数统计(全市均值)
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
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
        public DataView GetMonthData(string[] regionGuids, DateTime dateStart, DateTime dateEnd, RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            return m_RegionAQIDAL.GetMonthData(regionGuids, dateStart, dateEnd, regionAQIStatisticalType);
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
            return m_RegionAQIDAL.GetRegionsAQI(dateStart, dateEnd, regionAQIStatisticalType);
        }
        /// <summary>
        /// 年报
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        public DataView GetRegionsHalfYearData(DateTime dateStart, DateTime dateEnd, RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            return m_RegionAQIDAL.GetRegionsHalfYearData(dateStart, dateEnd, regionAQIStatisticalType);
        }
        /// <summary>
        /// 年报
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        public DataView GetRegionsYearAQIData(DateTime dateStart, DateTime dateEnd, RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            return m_RegionAQIDAL.GetRegionsYearAQIData(dateStart, dateEnd, regionAQIStatisticalType);
        }

        /// <summary>
        /// 获取时间段内的所有区域的数据
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        public DataView GetRegionsAllDataByDate(DateTime dateStart, DateTime dateEnd, RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            return m_RegionAQIDAL.GetRegionsAllDataByDate(dateStart, dateEnd, regionAQIStatisticalType);
        }
        /// <summary>
        /// 获取时间段内的所有测点的数据
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        public DataView GetPointsAllDataByDate(DateTime dateStart, DateTime dateEnd)
        {
            return m_RegionAQIDAL.GetPointsAllDataByDate(dateStart, dateEnd);
        }

        /// <summary>
        /// 获取时间段内区域的数据
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="classDics">污染种类</param>
        /// <param name="dtBegion">开始日期</param>
        /// <param name="dtEnd">结束日期</param>
        /// <returns></returns>
        public DataView GetDataPagerClass(string[] regionGuids, DateTime dtBegion, DateTime dtEnd)
        {
            return m_RegionAQIDAL.GetDataPagerClass(regionGuids, dtBegion, dtEnd);
        }

        /// <summary>
        /// 获取时间段内首要污染物的数据
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dtBegion">开始日期</param>
        /// <param name="dtEnd">结束日期</param>
        /// <returns></returns>
        public DataView GetFirstPollute(string[] regionGuids, DateTime dtBegion, DateTime dtEnd)
        {
            return m_RegionAQIDAL.GetFirstPollute(regionGuids, dtBegion, dtEnd);
        }
        #endregion
    }
}
