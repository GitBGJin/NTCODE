using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.Data.SqlServer.MonitoringBusiness;
using SmartEP.Data.SqlServer.MonitoringBusiness.Water;
using SmartEP.DomainModel.MonitoringBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.MonitoringBusinessRepository.Water
{
    /// <summary>
    /// 地表水季数据仓储类
    /// </summary>
    public class SeasonReportRepository : BaseGenericRepository<MonitoringBusinessModel, WaterSeasonReportEntity>
    {

        public override bool IsExist(string strKey)
        {
            throw new NotImplementedException();
        }

        #region << ADO.NET >>
        /// <summary>
        /// 数据处理DAL类
        /// </summary>
        SeasonReportDAL m_SeasonReportDAC = Singleton<SeasonReportDAL>.GetInstance();

        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="seasonOfYearFrom">开始季数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="seasonOfYearTo">结束季数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Year,WeekOfYear）</param>
        /// <returns></returns>
        public DataView GetDataPager(string[] portIds, string[] factors
            , int yearFrom, int seasonOfYearFrom, int yearTo, int seasonOfYearTo, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Year,SeasonOfYear")
        {
            return m_SeasonReportDAC.GetDataPager(portIds, factors, yearFrom, seasonOfYearFrom, yearTo, seasonOfYearTo, pageSize, pageNo, out recordTotal, orderBy);
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="seasonOfYearFrom">开始季数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="seasonOfYearTo">结束季数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Year,SeasonOfYear）</param>
        /// <returns></returns>
        public DataView GetDataPager(string[] portIds, string[] factors, int yearFrom, int seasonOfYearFrom, int yearTo, int seasonOfYearTo
            , int yearFromB, int seasonOfYearFromB, int yearToB, int seasonOfYearToB, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Year,SeasonOfYear")
        {
            return m_SeasonReportDAC.GetDataPager(portIds, factors, yearFrom, seasonOfYearFrom, yearTo, seasonOfYearTo, yearFromB, seasonOfYearFromB, yearToB, seasonOfYearToB, pageSize, pageNo, out recordTotal, orderBy);
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="seasonOfYearFrom">开始季数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="seasonOfYearTo">结束季数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Year,WeekOfYear）</param>
        /// <returns></returns>
        public DataView GetExportData(string[] portIds, IList<IPollutant> factors
            , int yearFrom, int seasonOfYearFrom, int yearTo, int seasonOfYearTo, string orderBy = "PointId,Year,SeasonOfYear")
        {
            return m_SeasonReportDAC.GetExportData(portIds, factors, yearFrom, seasonOfYearFrom, yearTo, seasonOfYearTo, orderBy);
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="seasonOfYearFrom">开始季数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="seasonOfYearTo">结束季数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Year,WeekOfYear）</param>
        /// <returns></returns>
        public DataView GetExportData(string[] portIds, string[] factors
            , int yearFrom, int seasonOfYearFrom, int yearTo, int seasonOfYearTo, string orderBy = "PointId,Year,SeasonOfYear")
        {
            return m_SeasonReportDAC.GetExportData(portIds, factors, yearFrom, seasonOfYearFrom, yearTo, seasonOfYearTo, orderBy);
        }
        /// <summary>
        /// 取得行转列数据总行数
        /// </summary>
        /// <param name="portIds">测点数组</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="seasonOfYearFrom">开始季数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="seasonOfYearTo">结束季数</param>
        /// <returns></returns>
        public int GetAllDataCount(string[] portIds, int yearFrom, int seasonOfYearFrom, int yearTo, int seasonOfYearTo)
        {
            return m_SeasonReportDAC.GetAllDataCount(portIds, yearFrom, seasonOfYearFrom, yearTo, seasonOfYearTo);
        }

        /// <summary>
        /// 取得统计数据（最大值、最小值、平均值）
        /// </summary>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="seasonOfYearFrom">开始季数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="seasonOfYearTo">结束季数</param>
        /// <returns></returns>
        public DataView GetStatisticalData(string[] portIds, string[] factors, int yearFrom, int seasonOfYearFrom, int yearTo, int seasonOfYearTo)
        {
            return m_SeasonReportDAC.GetStatisticalData(portIds, factors, yearFrom, seasonOfYearFrom, yearTo, seasonOfYearTo);
        }
        #endregion
    }
}
