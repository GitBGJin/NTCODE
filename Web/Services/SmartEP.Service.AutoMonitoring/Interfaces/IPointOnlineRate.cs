using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.AutoMonitoring.Interfaces
{
    /// <summary>
    /// 名称：IPointOnlineRate.cs
    /// 创建人：王秉晟
    /// 创建日期：2015-08-24
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：站点在线接口
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public interface IPointOnlineRate
    {
        /// <summary>
        /// 获取在线率明细数据
        /// </summary>
        /// <param name="operations">运维商数据</param>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">结束时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns></returns>
        DataView GetOnlineRateDataPager(string[] operations, string[] portIds
            , DateTime dtmStart, DateTime dtmEnd, int pageSize, int pageNo, out int recordTotal, string orderBy);

        /// <summary>
        /// 获取在线率明细导出数据
        /// </summary>
        /// <param name="operations">运维商数据</param>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">结束时间</param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns></returns>
        DataView GetOnlineRateExportData(string[] operations, string[] portIds,
            DateTime dtmStart, DateTime dtmEnd, string orderBy);
    }
}
