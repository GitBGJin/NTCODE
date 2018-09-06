using SmartEP.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.DataAnalyze.Interfaces
{
    /// <summary>
    /// 名称：IRealTimeOnline.cs
    /// 创建人：王秉晟
    /// 创建日期：2015-08-26
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：实时在线接口
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public interface IRealTimeOnline
    {
        /// <summary>
        /// 获取实时在线状态信息
        /// </summary>
        /// <param name="netWorkType">联网状态（1：全部，2：在线，3：离线）</param>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="dvOnlineRate">联网率信息</param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns></returns>
        DataView GetRealTimeOnlineStateDataPager(string netWorkType, string[] portIds, string[] factors,
              int pageSize, int pageNo, out int recordTotal, out DataView dvOnlineRate, string orderBy);
    }
}
