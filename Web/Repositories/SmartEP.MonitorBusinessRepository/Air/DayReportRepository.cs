﻿using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.Data.SqlServer.MonitoringBusiness;
using SmartEP.Data.SqlServer.MonitoringBusiness.Air;
using SmartEP.DomainModel.MonitoringBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.MonitoringBusinessRepository.Air
{
    /// <summary>
    /// 空气日数据仓储类
    /// </summary>
    public class DayReportRepository : BaseGenericRepository<MonitoringBusinessModel, AirDayReportEntity>
    {

        public override bool IsExist(string strKey)
        {
            throw new NotImplementedException();
        }

        #region << ADO.NET >>
        /// <summary>
        /// 数据处理DAL类
        /// </summary>
        DayReportDAL m_DayReportDAC = Singleton<DayReportDAL>.GetInstance();

        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetDataPager(string[] portIds, string[] factors
            , DateTime dateStart, DateTime dateEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime")
        {
            return m_DayReportDAC.GetDataPager(portIds, factors, dateStart, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetAvgDataPager(string[] portIds, string[] factors
            , DateTime dateStart, DateTime dateEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime")
        {
            return m_DayReportDAC.GetAvgDataPager(portIds, factors, dateStart, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetDayDataAvg(string[] portIds, string[] factors
            , DateTime dateStart, DateTime dateEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime")
        {
            return m_DayReportDAC.GetDayDataAvg(portIds, factors, dateStart, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
        }
        /// <summary>
        /// 取得查询数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <returns></returns>
        public DataView GetDataPager(string portIds, string[] factors
            , DateTime dateStart, DateTime dateEnd)
        {
            return m_DayReportDAC.GetDataPager(portIds, factors, dateStart, dateEnd);
        }
        /// <summary>
        /// 取得全月均值，最大最小值
        /// </summary>
        /// <param name="autoMonitorType">数据类型</param>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetAvgDayData(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd)
        {
            return m_DayReportDAC.GetAvgDayData(portIds, factors, dateStart, dateEnd);
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetDataPager(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd
            , DateTime dtFrom, DateTime dtTo, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime desc")
        {
            return m_DayReportDAC.GetDataPager(portIds, factors, dateStart, dateEnd, dtFrom, dtTo, pageSize, pageNo, out recordTotal, orderBy);
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetDataPagerDF(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd
            ,  int pageSize, int pageNo,  string orderBy = "PointId,DateTime desc")
        {
            return m_DayReportDAC.GetDataPagerDF(portIds, factors, dateStart, dateEnd,  pageSize, pageNo,  orderBy);
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetExportData(string[] portIds, IList<IPollutant> factors
            , DateTime dateStart, DateTime dateEnd, string orderBy = "PointId,DateTime")
        {
            return m_DayReportDAC.GetExportData(portIds, factors, dateStart, dateEnd, orderBy);
        }

        /// <summary>
        /// 环境空气质量例行监测成果表导出
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetAQRoutineMonthReportExportData(string[] portIds, IList<IPollutant> factors
            , DateTime dateStart, DateTime dateEnd, string orderBy = "PointId,DateTime")
        {
            return m_DayReportDAC.GetAQRoutineMonthReportExportData(portIds, factors, dateStart, dateEnd, orderBy);
        }


        /// <summary>
        /// 取得行转列数据总行数
        /// </summary>
        /// <param name="portIds">测点数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <returns></returns>
        public int GetAllDataCount(string[] portIds, DateTime dateStart, DateTime dateEnd)
        {
            return m_DayReportDAC.GetAllDataCount(portIds, dateStart, dateEnd);
        }

        /// <summary>
        /// 取得统计数据（最大值、最小值、平均值）
        /// </summary>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetStatisticalData(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd)
        {
            return m_DayReportDAC.GetStatisticalData(portIds, factors, dateStart, dateEnd);
        }

        /// <summary>
        /// 取得AQI表中日数据替换日数据表中的数据的日数据
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="factors">因子</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="isAllDate">是否显示缺失数据</param>
        /// <returns></returns>
        public DataView GetDataViewFromAQI(string[] portIds, IList<IPollutant> factors, DateTime dateStart, DateTime dateEnd, bool isAllDate)
        {
            return m_DayReportDAC.GetDataViewFromAQI(portIds, factors, dateStart, dateEnd, isAllDate);
        }
        #region 获取数据
        /// <summary>
        /// 获取基准数据
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public DataView GetCheckDataDayAQI(string strWhere)
        {
            return m_DayReportDAC.GetCheckDataDayAQI(strWhere);
        }
        /// <summary>
        /// 获取平均浓度
        /// </summary>
        /// <param name="portIds">测点</param>
        /// <param name="factor">因子</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="DataType">类型</param>
        /// <returns></returns>
        public DataView GetConcentrationDay(string[] portIds,  DateTime dtStart, DateTime dtEnd, string DataType)
        {
            return m_DayReportDAC.GetConcentrationDay(portIds,  dtStart, dtEnd, DataType);
        }
        /// <summary>
        /// 获取基准数据类型
        /// </summary>   
        /// <returns></returns>
        public DataTable GetCheckDataType()
        {
            return m_DayReportDAC.GetCheckDataType();
        }
        #endregion

        #region 获取区域数据
        /// <summary>
        /// 获取区域基准数据
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public DataView GetCheckRegionDayAQI(string strWhere)
        {
            return m_DayReportDAC.GetCheckRegionDayAQI(strWhere);
        }
        /// <summary>
        /// 获取区域平均浓度
        /// </summary>
        /// <param name="RegionUids">区域</param>
        /// <param name="factor">因子</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="DataType">类型</param>
        /// <returns></returns>
        public DataView GetRegionConcentrationDay(string[] RegionUids,DateTime dtStart, DateTime dtEnd, string DataType)
        {
            return m_DayReportDAC.GetRegionConcentrationDay(RegionUids, dtStart, dtEnd, DataType);
        }
        /// <summary>
        /// 年报
        /// </summary>
        /// <param name="RegionUids">区域</param>
        /// <param name="factor">因子</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="DataType">类型</param>
        /// <returns></returns>
        public DataView GetRegionYearBaseData(DateTime dtStart, DateTime dtEnd, string DataType)
        {
            return m_DayReportDAC.GetRegionYearBaseData(dtStart, dtEnd, DataType);
        }
        /// <summary>
        /// 季报
        /// </summary>
        /// <param name="RegionUids">区域</param>
        /// <param name="factor">因子</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="DataType">类型</param>
        /// <returns></returns>
        public DataView GetRegionSeasonBaseData(DateTime dtStart, DateTime dtEnd, string DataType)
        {
            return m_DayReportDAC.GetRegionSeasonBaseData(dtStart, dtEnd, DataType);
        }
        /// <summary>
        /// 获取时间段内的所有区域的数据
        /// </summary>
        /// <param name="RegionUids">区域</param>
        /// <param name="factor">因子</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="DataType">类型</param>
        /// <returns></returns>
        public DataView GetRegionBaseDataByDate(DateTime dtStart, DateTime dtEnd, string DataType)
        {
            return m_DayReportDAC.GetRegionBaseDataByDate(dtStart, dtEnd, DataType);
        }
        /// <summary>
        /// 获取区域基准数据类型
        /// </summary>   
        /// <returns></returns>
        public DataTable GetCheckRegionDataType()
        {
            return m_DayReportDAC.GetCheckRegionDataType();
        }
        #endregion
        #region 插入数据
        /// <summary>
        /// 取得AQI表中日数据替换日数据表中的数据的日数据
        /// </summary>
        /// <param name="dt">待插入数据表</param>
        /// <param name="DataType">数据类型</param>
        /// <param name="portIds">站点</param>
        /// <returns></returns>
        public void insertTable(DataTable dt, string DataType, string[] portIds)
        {
            m_DayReportDAC.insertTable(dt, DataType, portIds);
        }
        #endregion
        #region 获取时间点下的测点，因子数据
        /// <summary>
        /// 获取时间点下的测点，因子数据
        /// </summary>
        /// <param name="PointIds">监测点Id数组</param>
        /// <param name="PollutantCodes">监测因子Code数组</param>
        /// <param name="DateLists">监测日期数组</param>
        /// <returns></returns>
        public DataTable GetDayData(List<int> PointIds, List<string> PollutantCodes, List<DateTime> DateLists)
        {
            return m_DayReportDAC.GetDayData(PointIds, PollutantCodes, DateLists);
        }
        #endregion
        #endregion
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetDayDataRegionPager(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd, int pageSize, int pageNo, out int recordTotal, string orderBy)
        {
          return m_DayReportDAC.GetDayDataRegionPager(portIds, factors, dateStart, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
        }
    }
}
