﻿using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.Data.SqlServer.MonitoringBusiness.Air;
using SmartEP.DomainModel.MonitoringBusiness;
using System;
using System.Collections.Generic;
using System.Data;

namespace SmartEP.MonitoringBusinessRepository.Air
{
    /// <summary>
    /// 空气审核小时数据仓储类
    /// </summary>
    public class HourReportRepository : BaseGenericRepository<MonitoringBusinessModel, AirHourReportEntity>
    {
        /// <summary>
        /// 数据处理DAL类
        /// </summary>
        HourReportDAL m_HourReportDAC = Singleton<HourReportDAL>.GetInstance();

        public override bool IsExist(string strKey)
        {
            throw new NotImplementedException();
        }

        #region << ADO.NET >>

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
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns></returns>
        public DataView GetDataPager(string[] portIds, string[] factors
            , DateTime dateStart, DateTime dateEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Tstamp")
        {
            return m_HourReportDAC.GetDataPager(portIds, factors, dateStart, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
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
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns></returns>
        public DataView GetNewDataPager(string[] portIds, string[] factors
            , DateTime dateStart, DateTime dateEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Tstamp")
        {
            return m_HourReportDAC.GetNewDataPager(portIds, factors, dateStart, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
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
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns></returns>
        public DataView GetNewDataPagerAvg(string[] portIds, string[] factors
            , DateTime dateStart, DateTime dateEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Tstamp")
        {
            return m_HourReportDAC.GetNewDataPagerAvg(portIds, factors, dateStart, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
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
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns></returns>
        public DataView GetHourDataAvg(string[] portIds, string[] factors
            , DateTime dateStart, DateTime dateEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Tstamp")
        {
            return m_HourReportDAC.GetHourDataAvg(portIds, factors, dateStart, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
        }
        /// <summary>
        /// 取得查询数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <returns></returns>
        public DataView GetNewDataPager(string portIds, string[] factors
            , DateTime dateStart, DateTime dateEnd)
        {
            return m_HourReportDAC.GetNewDataPager(portIds, factors, dateStart, dateEnd);
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
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns></returns>
        public DataView GetDataNewPager(string[] portIds, string[] factors
            , DateTime dateStart, DateTime dateEnd)
        {
            return m_HourReportDAC.GetDataNewPager(portIds, factors, dateStart, dateEnd);
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
        public DataView GetAvgDayData(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd)
        {
            return m_HourReportDAC.GetAvgDayData(portIds, factors, dateStart, dateEnd);
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)去除质控数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns></returns>
        public DataView GetDataPagerNew(string[] portIds, string[] factors
            , DateTime dateStart, DateTime dateEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Tstamp")
        {
            return m_HourReportDAC.GetDataPagerNew(portIds, factors, dateStart, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)去除质控数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns></returns>
        public DataView GetVOCsKQYDataPagerNew(string[] portIds, string[] factors
            , DateTime dateStart, DateTime dateEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Tstamp")
        {
            return m_HourReportDAC.GetVOCsKQYDataPagerNew(portIds, factors, dateStart, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtBegin">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns></returns>
        public DataView GetDataHourPager(string[] portIds, string[] factors, DateTime dtBegin, DateTime dtEnd,
            DateTime dtFrom, DateTime dtTo, string type, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,tstamp desc")
        {
            return m_HourReportDAC.GetDataHourPager(portIds, factors, dtBegin, dtEnd, dtFrom, dtTo, type, pageSize, pageNo, out recordTotal, orderBy);
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtBegin">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns></returns>
        public DataView GetDataPager(string[] portIds, string[] factors, DateTime dtBegin, DateTime dtEnd,
            DateTime dtFrom, DateTime dtTo, string type, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,tstamp desc")
        {
            return m_HourReportDAC.GetDataPager(portIds, factors, dtBegin, dtEnd, dtFrom, dtTo, type, pageSize, pageNo, out recordTotal, orderBy);
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtBegin">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns></returns>
        public DataView GetDataPagerDF(string[] portIds, string[] factors, DateTime dtBegin, DateTime dtEnd,string type, int pageSize, int pageNo, string orderBy = "PointId,tstamp desc")
        {
            return m_HourReportDAC.GetDataPagerDF(portIds, factors, dtBegin, dtEnd,  type, pageSize, pageNo, orderBy);
        }
        /// <summary>
        /// 取得所有数据供导出(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns></returns>
        public DataView GetExportData(string[] portIds, IList<IPollutant> factors
            , DateTime dateStart, DateTime dateEnd, string orderBy = "PointId,Tstamp")
        {
            return m_HourReportDAC.GetExportData(portIds, factors, dateStart, dateEnd, orderBy);
        }
        /// <summary>
        /// 取得所有数据供导出(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns></returns>
        public DataView GetNewExportData(string[] portIds, IList<IPollutant> factors
            , DateTime dateStart, DateTime dateEnd, string orderBy = "PointId,Tstamp")
        {
            return m_HourReportDAC.GetNewExportData(portIds, factors, dateStart, dateEnd, orderBy);
        }
        /// <summary>
        /// 环境空气质量自动监测数据报表 
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetAQAutoMonthReportExportData(string[] portIds, IList<IPollutant> factors
            , DateTime dateStart, DateTime dateEnd, string orderBy = "PointId,Tstamp")
        {
            return m_HourReportDAC.GetAQAutoMonthReportExportData(portIds, factors, dateStart, dateEnd, orderBy);
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
        public DataView GetDataPager(string[] portIds, string[] factors
            , DateTime dateStart, DateTime dateEnd)
        {
            return m_HourReportDAC.GetDataNewPager(portIds, factors, dateStart, dateEnd);
        }
        /// <summary>
        /// 取得全月均值,有效天
        /// </summary>
        /// <param name="autoMonitorType">数据类型</param>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetAvgDayNewData(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd)
        {
            return m_HourReportDAC.GetAvgDayNewData(portIds, factors, dateStart, dateEnd);
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
            return m_HourReportDAC.GetAllDataCount(portIds, dateStart, dateEnd);
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
            return m_HourReportDAC.GetStatisticalData(portIds, factors, dateStart, dateEnd);
        }

        /// <summary>
        /// 取得统计数据 按日分组（最大值、最小值、平均值）
        /// </summary>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetStatisticalDataByDay(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd)
        {
            return m_HourReportDAC.GetStatisticalDataByDay(portIds, factors, dateStart, dateEnd);
        }
        #endregion

        #region 时间段内因子比值数据
        /// <summary>
        /// 时间段内因子比值数据
        /// </summary>
        /// <param name="StartDate">开始日期（时）</param>
        /// <param name="EndDate">开始日期（时）</param>
        /// <param name="PointId">站点ID</param>
        /// <param name="FactorCodes">比对因子Code数组</param>
        /// <returns></returns>
        public DataTable GetHourAvgCompareData(DateTime StartDate, DateTime EndDate, Int32 PointId, string[] FactorCodes)
        {
            return m_HourReportDAC.GetHourAvgCompareData(StartDate, EndDate, PointId, FactorCodes);
        }
        #endregion

        #region 时间段内测点因子数据
        /// <summary>
        /// 时间段内测点因子数据
        /// </summary>
        /// <param name="StartDate">开始日期（时）</param>
        /// <param name="EndDate">截止日期（时）</param>
        /// <param name="PointId">站点ID</param>
        /// <param name="factorCodes">因子Code数组</param>
        /// <returns></returns>
        public DataTable GetHourDate(DateTime StartDate, DateTime EndDate, Int32 PointId, string[] factorCodes)
        {
            return m_HourReportDAC.GetHourDate(StartDate, EndDate, PointId, factorCodes);
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
        public DataTable GetHourData(List<int> PointIds, List<string> PollutantCodes, List<DateTime> DateLists)
        {
            return m_HourReportDAC.GetHourData(PointIds, PollutantCodes, DateLists);
        }
        #endregion
        #region 捕获有效时数
        /// <summary>
        /// 获取时间点下的测点，因子数据
        /// </summary>
        /// <param name="PointIds">监测点Id数组</param>
        /// <param name="PollutantCodes">监测因子Code数组</param>
        /// <param name="DateLists">监测日期数组</param>
        /// <returns></returns>
        public DataView GetCaptureDataPager(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {
            return m_HourReportDAC.GetCaptureRateData(portIds, factors, dtStart, dtEnd);
        }
        /// <summary>
        /// 获取时间点下的测点，因子数据
        /// </summary>
        /// <param name="PointIds">监测点Id数组</param>
        /// <param name="PollutantCodes">监测因子Code数组</param>
        /// <param name="DateLists">监测日期数组</param>
        /// <returns></returns>
        public DataView GetSuperCaptureDataPager(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {
            return m_HourReportDAC.GetSuperCaptureRateData(portIds, factors, dtStart, dtEnd);
        }
        #endregion

        #region 捕获有效时数
        /// <summary>
        /// 获取时间点下的测点，因子数据
        /// </summary>
        /// <param name="PointIds">监测点Id数组</param>
        /// <param name="PollutantCodes">监测因子Code数组</param>
        /// <param name="DateLists">监测日期数组</param>
        /// <returns></returns>
        public DataView GetCaptureDataPagerNew(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {
            return m_HourReportDAC.GetCaptureRateDataNew(portIds, factors, dtStart, dtEnd);
        }
        #endregion

        
        /// <summary>
        /// 取得测点名称
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <returns></returns>
        public DataView GetPointName(string portIds)
        {
            return m_HourReportDAC.GetPointName(portIds);
        }


        /// <summary>
        /// 考核项目
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataView GetCheckNew(string[] portIds, DateTime dateStart, DateTime dateEnd)
        {
            return m_HourReportDAC.GetCheckNew(portIds, dateStart, dateEnd);
        }
        #region 有效率有效时数
        /// <summary>
        /// 获取时间点下的测点，因子数据
        /// </summary>
        /// <param name="PointIds">监测点Id数组</param>
        /// <param name="PollutantCodes">监测因子Code数组</param>
        /// <param name="DateLists">监测日期数组</param>
        /// <returns></returns>
        public DataView GetEffectiveData(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {
            return m_HourReportDAC.GetEffectiveData(portIds, factors, dtStart, dtEnd);
        }
        /// <summary>
        /// 获取时间点下的测点，因子数据
        /// </summary>
        /// <param name="PointIds">监测点Id数组</param>
        /// <param name="PollutantCodes">监测因子Code数组</param>
        /// <param name="DateLists">监测日期数组</param>
        /// <returns></returns>
        public DataView GetSuperEffectiveData(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {
            return m_HourReportDAC.GetSuperEffectiveData(portIds, factors, dtStart, dtEnd);
        }
        /// <summary>
        /// 根据CategoryUid获取因子
        /// </summary>
        /// <param name="CategoryUid"></param>
        /// <returns></returns>
        public DataView GetPollutantCodeByUid(string CategoryUid)
        {
            return m_HourReportDAC.GetPollutantCodeByUid(CategoryUid);
        }
        /// <summary>
        /// 根据因子获取CategoryUid
        /// </summary>
        /// <param name="PollutantCode"></param>
        /// <returns></returns>
        public DataView GetCategoryUidByPollutantCode(string[] PollutantCode)
        {
            return m_HourReportDAC.GetCategoryUidByPollutantCode(PollutantCode);
        }
        /// <summary>
        /// 获取CategoryUid的名称
        /// </summary>
        /// <param name="CategoryUid"></param>
        /// <returns></returns>
        public DataView GetCategory(string CategoryUid)
        {
            return m_HourReportDAC.GetCategory(CategoryUid);
        }
        #endregion

        /// <summary>
        /// 获取时间点下的测点，因子数据
        /// </summary>
        /// <param name="PointIds">监测点Id数组</param>
        /// <param name="PollutantCodes">监测因子Code数组</param>
        /// <param name="DateLists">监测日期数组</param>
        /// <returns></returns>
        public DataView GetEffectiveDataNew(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {
            return m_HourReportDAC.GetEffectiveDataNew(portIds, factors, dtStart, dtEnd);
        }

        public DataView GetNewHourDataPagerWidthO8(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd, int pageSize, int pageNo, out int recordTotal, string orderBy)
        {
          return m_HourReportDAC.GetNewHourDataPagerWidthO8(portIds, factors, dateStart, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
        }
        public DataView GetNewHourDataPagerWidthRegionO8(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd, int pageSize, int pageNo, out int recordTotal, string orderBy)
        {
          return m_HourReportDAC.GetNewHourDataPagerWidthRegionO8(portIds, factors, dateStart, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
        }
      /// <summary>
      /// 根据测点获取区域信息
      /// </summary>
      /// <param name="PointID"></param>
      /// <returns></returns>
        public DataTable GetRegionWithPointId(string[] PointID)
        {
          return m_HourReportDAC.GetRegionWithPointId(PointID);
        }
    }
}
