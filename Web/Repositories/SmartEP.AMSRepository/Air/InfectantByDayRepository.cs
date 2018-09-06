
using SmartEP.AMSRepository.Interfaces;
using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.Data.SqlServer.AutoMonitoring;
using SmartEP.DomainModel.AirAutoMonitoring;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.AMSRepository.Air
{
    /// <summary>
    /// 名称：InfectantByDayRepository.cs
    /// 创建人：刘晋
    /// 创建日期：2017-06-01
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 原始日数据实时数据仓储类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class InfectantByDayRepository : BaseGenericRepository<AirAutoMonitoringModel, InfectantBy5Entity>, IInfectantRepository
    {
        public override bool IsExist(string strKey)
        {
            return true;
        }
        #region << ADO.NET >>
        /// <summary>
        /// 5分钟数据处理DAL类
        /// </summary>
        InfectantDAL m_InfectantDAL = new InfectantDAL(ApplicationType.Air, PollutantDataType.OriDay);

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
            return m_InfectantDAL.GetDataPager(portIds, factors, dateStart, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
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
        public DataView GetDayAvgData(string[] portIds, string[] factors
            , DateTime dateStart, DateTime dateEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime")
        {
            return m_InfectantDAL.GetDayAvgData(portIds, factors, dateStart, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
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
        public DataView GetDataPagers(string[] portIds, string[] factors
            , DateTime dateStart, DateTime dateEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime")
        {
            return m_InfectantDAL.GetDataPagers(portIds, factors, dateStart, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
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
        public DataView GetVOCsDataPagers(string[] portIds, string[] factors
            , DateTime dateStart, DateTime dateEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime")
        {
            return m_InfectantDAL.GetVOCsDataPagers(portIds, factors, dateStart, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
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
        public DataView GetAvgDataPagers(string[] portIds, string[] factors
            , DateTime dateStart, DateTime dateEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime")
        {
            return m_InfectantDAL.GetAvgDataPagers(portIds, factors, dateStart, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
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
        public DataView GetDataPagersForAllTime(string[] portIds, string[] factors
            , DateTime dateStart, DateTime dateEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime")
        {
            return m_InfectantDAL.GetDataPagersForAllTime(portIds, factors, dateStart, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
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
        public DataView GetDataPagersForNTO3AllTime(string[] portIds, string[] factors
            , DateTime dateStart, DateTime dateEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime")
        {
            return m_InfectantDAL.GetDataPagersForNTO3AllTime(portIds, factors, dateStart, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
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
        public DataView GetDataPagerForAllTimeRegion(string[] portIds, string[] factors
            , DateTime dateStart, DateTime dateEnd, int pageSize, int pageNo, out int recordTotal, string orderBy )
        {
          return m_InfectantDAL.GetDataPagerForAllTimeRegion(portIds, factors, dateStart, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
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
        public DataView GetDataPagersWithMax(string[] portIds, string[] factors
            , DateTime dateStart, DateTime dateEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime")
        {
            return m_InfectantDAL.GetDataPagersWithMax(portIds, factors, dateStart, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
        }

        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="autoMonitorType">查询数据类型</param>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns></returns>
        public DataView GetExportData(string[] portIds, IList<IPollutant> factors
            , DateTime dateStart, DateTime dateEnd, string orderBy = "PointId,Tstamp")
        {
            return m_InfectantDAL.GetExportData(portIds, factors, dateStart, dateEnd, orderBy);
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
            return m_InfectantDAL.GetAllDataCount(portIds, dateStart, dateEnd);
        }

        /// <summary>
        /// 取得统计数据（最大值、最小值、平均值）
        /// </summary>
        /// <param name="autoMonitorType">数据类型</param>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetStatisticalData(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd)
        {
            return m_InfectantDAL.GetStatisticalData(portIds, factors, dateStart, dateEnd);
        }

        /// <summary>
        /// 取得指定站点指定时间内最新一条数据
        /// </summary>
        /// <param name="portIds">测点</param>
        /// <param name="factors">因子</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <returns></returns>
        public DataView GetLastDataByPort(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {
            return m_InfectantDAL.GetLastDataByPort(portIds, factors, dtStart, dtEnd);
        }

        /// <summary>
        /// 取得指定因子指定时间范围内最新一条数据
        /// </summary>
        /// <param name="portIds">测点</param>
        /// <param name="factors">因子</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <returns></returns>
        public DataView GetLastDataByPollutant(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {
            return m_InfectantDAL.GetLastDataByPollutant(portIds, factors, dtStart, dtEnd);
        }

        #endregion
    }
}
