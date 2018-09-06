using SmartEP.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.AMSRepository.Interfaces
{
    /// <summary>
    /// 实时数据仓储接口
    /// </summary>
    public interface IInfectantRepository
    {
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
        /// <returns></returns>
        DataView GetDataPager(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "");

        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="autoMonitorType">查询数据类型</param>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <returns></returns>
        DataView GetExportData(string[] portIds, IList<IPollutant> factors, DateTime dateStart, DateTime dateEnd, string orderBy = "");

        /// <summary>
        /// 取得行转列数据总行数
        /// </summary>
        /// <param name="portIds">测点数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <returns></returns>
        int GetAllDataCount(string[] portIds, DateTime dateStart, DateTime dateEnd);

        /// <summary>
        /// 取得统计数据（最大值、最小值、平均值）
        /// </summary>
        /// <param name="autoMonitorType">数据类型</param>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        DataView GetStatisticalData(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd);

        /// <summary>
        /// 取得指定站点指定时间内最新一条数据
        /// </summary>
        /// <param name="portIds">测点</param>
        /// <param name="factors">因子</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <returns></returns>
        DataView GetLastDataByPort(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd);

        /// <summary>
        /// 取得指定因子指定时间范围内最新一条数据
        /// </summary>
        /// <param name="portIds">测点</param>
        /// <param name="factors">因子</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <returns></returns>
        DataView GetLastDataByPollutant(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd);
        #endregion
    }
}
