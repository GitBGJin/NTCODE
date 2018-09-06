using SmartEP.AMSRepository.Air;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.AutoMonitoring.Air
{
    /// <summary>
    /// 名称：InstrumentDataBy60Service.cs
    /// 创建人：李飞
    /// 创建日期：2015-08-28
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 小时仪器状态服务层
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class InstrumentDataBy60Service
    {
        /// <summary>
        /// 仪器状态仓储层
        /// </summary>
        InstrumentDataBy60Repository g_InstrumentDataBy60Repository = Singleton<InstrumentDataBy60Repository>.GetInstance();

        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portId">测点数据</param>
        /// <param name="instrumentCode">仪器code</param>
        /// <param name="pollutantCodes">因子数据</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetDataPager(string portId, string instrumentCode, string[] pollutantCodes
            , DateTime dateStart, DateTime dateEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "Tstamp")
        {
            return g_InstrumentDataBy60Repository.GetDataPager(portId, instrumentCode, pollutantCodes, dateStart, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
        }

        /// <summary>
        /// 取得导出数据(行转列数据)
        /// </summary>
        /// <param name="portId">测点数据</param>
        /// <param name="instrumentCode">仪器code</param>
        /// <param name="pollutantCodes">因子数据</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetExportData(string portId, string instrumentCode, string[] pollutantCodes
            , DateTime dateStart, DateTime dateEnd, string orderBy = "Tstamp")
        {
            return g_InstrumentDataBy60Repository.GetExportData(portId, instrumentCode, pollutantCodes, dateStart, dateEnd, orderBy);
        }

        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="instrumentCode">仪器code</param>
        /// <param name="pollutantCodes">因子数据</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetDataPager(string[] portIds, string instrumentCode, string[] pollutantCodes
            , DateTime dateStart, DateTime dateEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Tstamp")
        {
            return g_InstrumentDataBy60Repository.GetDataPager(portIds, instrumentCode, pollutantCodes, dateStart, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
        }

        /// <summary>
        /// 取得导出数据(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="instrumentCode">仪器code</param>
        /// <param name="pollutantCodes">因子数据</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetExportData(string[] portIds, string instrumentCode, string[] pollutantCodes
            , DateTime dateStart, DateTime dateEnd, string orderBy = "PointId,Tstamp")
        {
            return g_InstrumentDataBy60Repository.GetExportData(portIds, instrumentCode, pollutantCodes, dateStart, dateEnd, orderBy);
        }
    }
}
