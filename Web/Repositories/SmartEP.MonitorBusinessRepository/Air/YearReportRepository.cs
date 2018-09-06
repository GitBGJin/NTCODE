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
    /// 空气年数据仓储类
    /// </summary>
    public class YearReportRepository : BaseGenericRepository<MonitoringBusinessModel, AirYearReportEntity>
    {

        public override bool IsExist(string strKey)
        {
            throw new NotImplementedException();
        }

        #region << ADO.NET >>
        /// <summary>
        /// 数据处理DAL类
        /// </summary>
        YearReportDAL m_YearReportDAC = Singleton<YearReportDAL>.GetInstance();

        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Year,WeekOfYear）</param>
        /// <returns></returns>
        public DataView GetDataPager(string[] portIds, string[] factors
            , int yearFrom, int yearTo, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Year")
        {
            return m_YearReportDAC.GetDataPager(portIds, factors, yearFrom, yearTo, pageSize, pageNo, out recordTotal, orderBy);
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Year,WeekOfYear）</param>
        /// <returns></returns>
        public DataView GetYearDataPagerRegion(string[] portIds, string[] factors
            , int yearFrom, int yearTo, int pageSize, int pageNo, out int recordTotal, string orderBy)
        {
          return m_YearReportDAC.GetYearDataPagerRegion(portIds, factors, yearFrom, yearTo, pageSize, pageNo, out recordTotal, orderBy);
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Year,WeekOfYear）</param>
        /// <returns></returns>
        public DataView GetDataPagerAvg(string[] portIds, string[] factors
            , int yearFrom, int yearTo, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Year")
        {
            return m_YearReportDAC.GetDataPagerAvg(portIds, factors, yearFrom, yearTo, pageSize, pageNo, out recordTotal, orderBy);
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Year,WeekOfYear）</param>
        /// <returns></returns>
        public DataView GetYearDataAvg(string[] portIds, string[] factors
            , int yearFrom, int yearTo, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Year")
        {
            return m_YearReportDAC.GetYearDataAvg(portIds, factors, yearFrom, yearTo, pageSize, pageNo, out recordTotal, orderBy);
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Year,WeekOfYear）</param>
        /// <returns></returns>
        public DataView GetDataPager(string[] portIds, string[] factors, int yearFrom, int yearTo
            , int yearFromB, int yearToB, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Year")
        {
            return m_YearReportDAC.GetDataPager(portIds, factors, yearFrom, yearTo,yearFromB,yearToB, pageSize, pageNo, out recordTotal, orderBy);
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Year,WeekOfYear）</param>
        /// <returns></returns>
        public DataView GetDataPagerDF(string[] portIds, string[] factors, int yearFrom, int yearTo
            ,  int pageSize, int pageNo,  string orderBy = "PointId,Year")
        {
            return m_YearReportDAC.GetDataPagerDF(portIds, factors, yearFrom, yearTo,  pageSize, pageNo,  orderBy);
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="orderBy">排序方式（字段：PointId,Year,WeekOfYear）</param>
        /// <returns></returns>
        public DataView GetExportData(string[] portIds, IList<IPollutant> factors
            , int yearFrom, int yearTo, string orderBy = "PointId,Year")
        {
            return m_YearReportDAC.GetExportData(portIds, factors, yearFrom, yearTo, orderBy);
        }

        /// <summary>
        /// 取得行转列数据总行数
        /// </summary>
        /// <param name="portIds">测点数组</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="yearTo">结束年</param>
        /// <returns></returns>
        public int GetAllDataCount(string[] portIds, int yearFrom, int yearTo)
        {
            return m_YearReportDAC.GetAllDataCount(portIds, yearFrom, yearTo);
        }

        /// <summary>
        /// 取得统计数据（最大值、最小值、平均值）
        /// </summary>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="yearTo">结束年</param>
        /// <returns></returns>
        public DataView GetStatisticalData(string[] portIds, string[] factors, int yearFrom, int yearTo)
        {
            return m_YearReportDAC.GetStatisticalData(portIds, factors, yearFrom, yearTo);
        }
        #endregion
    }
}
