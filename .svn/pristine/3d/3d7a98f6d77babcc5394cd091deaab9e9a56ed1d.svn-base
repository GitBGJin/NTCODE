using SmartEP.AMSRepository.Interfaces;
using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.Data.SqlServer.AutoMonitoring;
using SmartEP.DomainModel.WaterAutoMonitoring;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.AMSRepository.Water
{
    /// <summary>
    /// 60分钟实时数据仓储类
    /// </summary>
    public class InfectantBy60Repository : BaseGenericRepository<WaterAutoMonitoringModel, InfectantBy60Entity>, IInfectantRepository
    {
        public override bool IsExist(string strKey)
        {
            return true;
        }

        #region << ADO.NET >>
        /// <summary>
        /// 60分钟数据处理DAL类
        /// </summary>
        Infectant60WaterDAL m_InfectantDAL = Singleton<Infectant60WaterDAL>.GetInstance();

        /// <summary>
        /// 获取该测点接近当前时间的数据
        /// </summary>
        /// <param name="portIds">测点组</param>
        /// <param name="factors">因子组</param>
        /// <returns></returns>
        public DataView GetWaterRecentTimeData(string[] portIds, string[] factors)
        {
            return m_InfectantDAL.GetWaterRecentTimeData(portIds, factors);
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
        public DataView GetDataPager(string[] portIds, string[] factors
            , DateTime dateStart, DateTime dateEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Tstamp")
        {
            recordTotal = 0;
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
            , DateTime dateStart, DateTime dateEnd, string orderBy = "PointId,Tstamp")
        {
            return m_InfectantDAL.GetDayAvgData(portIds, factors, dateStart, dateEnd,orderBy);
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
        public DataView GetExportData(string[] portIds, string[] factors
            , DateTime dateStart, DateTime dateEnd, string orderBy = "PointId,Tstamp")
        {
            return m_InfectantDAL.GetExportData(portIds, factors, dateStart, dateEnd, orderBy);
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
        public DataView GetDayExportData(string[] portIds, IList<IPollutant> factors
            , DateTime dateStart, DateTime dateEnd, string orderBy = "PointId")
        {
            return m_InfectantDAL.GetDayExportData(portIds, factors, dateStart, dateEnd, orderBy);
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

        /// <summary>
        /// 返回指定站点、因子的捕获条数
        /// </summary>
        /// <param name="portIds">测点</param>
        /// <param name="factors">因子</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <returns></returns>
        public DataView GetPollutantValueCount(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {
            return m_InfectantDAL.GetPollutantValueCount(portIds, factors, dtStart, dtEnd);
        }

        /// <summary>
        /// 取得指定范围内的各指标的污染指数和水质等级
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="WQPollutants">所有参与统计的因子</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <returns></returns>
        public DataView GetWaterQualityData(string[] portIds, string[] WQPollutants, DateTime dateStart, DateTime dateEnd)
        {
            return m_InfectantDAL.GetWaterQualityData(portIds, WQPollutants, dateStart, dateEnd);
        }
        #endregion

        /// <summary>
        /// 从小时数据表中, 分组统计取得各测点的日均值
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtBegion"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataView GetDayAvgData(string[] portIds, IList<IPollutant> factors, DateTime dtBegion, DateTime dtEnd)
        {
            return m_InfectantDAL.GetDayAvgData(portIds, factors, dtBegion, dtEnd);
        }
    }
}
