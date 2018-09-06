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

namespace SmartEP.AMSRepository.WaterLZ
{
    /// <summary>
    /// 60分钟实时数据仓储类
    /// </summary>
    public class InfectantBy60Repository
    {
        //public override bool IsExist(string strKey)
        //{
        //    return true;
        //}
        #region << ADO.NET >>
        /// <summary>
        /// 60分钟数据处理DAL类
        /// </summary>
        InfectantDAL m_InfectantDAL = new InfectantDAL(ApplicationType.BlueAlga, PollutantDataType.Min60);

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
        public DataView GetLZDataPager(string[] portIds, string[] factors
            , DateTime dateStart, DateTime dateEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Tstamp")
        {
            recordTotal = 0;
            return m_InfectantDAL.GetLZDataPager(portIds, factors, dateStart, dateEnd, pageSize, pageNo, out recordTotal);
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
        public DataView GetStatisticalLZData(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd)
        {
            return m_InfectantDAL.GetStatisticalLZData(portIds, factors, dateStart, dateEnd);
        }

        #endregion
    }
}
