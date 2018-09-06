using SmartEP.Core.Enums;
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
    /// 名称：InstrumentDataBy60Repository.cs
    /// 创建人：李飞
    /// 创建日期：2015-08-17
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 小时仪器状态仓储层
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class InstrumentDataBy60Repository : BaseGenericRepository<WaterAutoMonitoringModel, InstrumentDataBy60Entity>
    {
        /// <summary>
        /// 仪器状态DAL
        /// </summary>
        InstrumentDataDAL g_InstrumentDataDAL = new InstrumentDataDAL(ApplicationType.Water);
        /// <summary>
        /// 根据主键Key判断记录是否存在
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public override bool IsExist(string strKey)
        {
            return RetrieveCount(x => x.Id.Equals(strKey)) == 0 ? false : true;
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portId">测点数据</param>
        /// <param name="instrumentCode">仪器</param>
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
            return g_InstrumentDataDAL.GetDataPager(portId, instrumentCode, pollutantCodes, dateStart, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
        }

        /// <summary>
        /// 取得导出数据(行转列数据)
        /// </summary>
        /// <param name="portId">测点数据</param>
        /// <param name="instrumentCode">仪器</param>
        /// <param name="pollutantCodes">因子数据</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetExportData(string portId, string instrumentCode, string[] pollutantCodes
            , DateTime dateStart, DateTime dateEnd, string orderBy = "Tstamp")
        {
            return g_InstrumentDataDAL.GetExportData(portId, instrumentCode, pollutantCodes, dateStart, dateEnd, orderBy);
        }

        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="instrumentCode">仪器</param>
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
            return g_InstrumentDataDAL.GetDataPager(portIds, instrumentCode, pollutantCodes, dateStart, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
        }

        /// <summary>
        /// 取得导出数据(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="instrumentCode">仪器</param>
        /// <param name="pollutantCodes">因子数据</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetExportData(string[] portIds, string instrumentCode, string[] pollutantCodes
            , DateTime dateStart, DateTime dateEnd, string orderBy = "PointId,Tstamp")
        {
            return g_InstrumentDataDAL.GetExportData(portIds, instrumentCode, pollutantCodes, dateStart, dateEnd, orderBy);
        }
    }
}
