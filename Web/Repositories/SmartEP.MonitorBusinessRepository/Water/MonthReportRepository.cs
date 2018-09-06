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
    /// 地表水月数据仓储类
    /// </summary>
    public class MonthReportRepository : BaseGenericRepository<MonitoringBusinessModel, WaterMonthReportEntity>
    {

        public override bool IsExist(string strKey)
        {
            throw new NotImplementedException();
        }

        #region << ADO.NET >>
        /// <summary>
        /// 数据处理DAL类
        /// </summary>
        MonthReportDAL m_MonthReportDAC = Singleton<MonthReportDAL>.GetInstance();

        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="monthOfYearFrom">开始月数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="monthOfYearTo">结束月数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Year,MonthOfYear）</param>
        /// <returns></returns>
        public DataView GetDataPager(string[] portIds, string[] factors
            , int yearFrom, int monthOfYearFrom, int yearTo, int monthOfYearTo, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Year,MonthOfYear")
        {
            return m_MonthReportDAC.GetDataPager(portIds, factors, yearFrom, monthOfYearFrom, yearTo, monthOfYearTo, pageSize, pageNo, out recordTotal, orderBy);
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="monthOfYearFrom">开始月数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="monthOfYearTo">结束月数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Year,WeekOfYear）</param>
        /// <returns></returns>
        public DataView GetDataPager(string[] portIds, string[] factors, int yearFrom, int monthOfYearFrom, int yearTo, int monthOfYearTo,
           int yearFromB, int monthOfYearFromB, int yearToB, int monthOfYearToB, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Year,MonthOfYear")
        {
            return m_MonthReportDAC.GetDataPager(portIds, factors, yearFrom, monthOfYearFrom, yearTo, monthOfYearTo, yearFromB, monthOfYearFromB, yearToB, monthOfYearToB, pageSize, pageNo, out recordTotal, orderBy);
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="monthOfYearFrom">开始月数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="monthOfYearTo">结束月数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Year,MonthOfYear）</param>
        /// <returns></returns>
        public DataView GetExportData(string[] portIds, IList<IPollutant> factors
            , int yearFrom, int monthOfYearFrom, int yearTo, int monthOfYearTo, string orderBy = "PointId,Year,MonthOfYear")
        {
            return m_MonthReportDAC.GetExportData(portIds, factors, yearFrom, monthOfYearFrom, yearTo, monthOfYearTo, orderBy);
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="monthOfYearFrom">开始月数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="monthOfYearTo">结束月数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Year,MonthOfYear）</param>
        /// <returns></returns>
        public DataView GetExportData(string[] portIds, string[] factors
            , int yearFrom, int monthOfYearFrom, int yearTo, int monthOfYearTo, string orderBy = "PointId,Year,MonthOfYear")
        {
            return m_MonthReportDAC.GetExportData(portIds, factors, yearFrom, monthOfYearFrom, yearTo, monthOfYearTo, orderBy);
        }
        /// <summary>
        /// 取得行转列数据总行数
        /// </summary>
        /// <param name="portIds">测点数组</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="monthOfYearFrom">开始月数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="monthOfYearTo">结束月数</param>
        /// <returns></returns>
        public int GetAllDataCount(string[] portIds, int yearFrom, int monthOfYearFrom, int yearTo, int monthOfYearTo)
        {
            return m_MonthReportDAC.GetAllDataCount(portIds, yearFrom, monthOfYearFrom, yearTo, monthOfYearTo);
        }

        /// <summary>
        /// 取得统计数据（最大值、最小值、平均值）
        /// </summary>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="monthOfYearFrom">开始月数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="monthOfYearTo">结束月数</param>
        /// <returns></returns>
        public DataView GetStatisticalData(string[] portIds, string[] factors, int yearFrom, int monthOfYearFrom, int yearTo, int monthOfYearTo)
        {
            return m_MonthReportDAC.GetStatisticalData(portIds, factors, yearFrom, monthOfYearFrom, yearTo, monthOfYearTo);
        }
        #endregion
    }
}
