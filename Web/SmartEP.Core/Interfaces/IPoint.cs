using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.Core.Interfaces
{
    /// <summary>
    /// 名称：IPoint.cs
    /// 创建人：李飞
    /// 创建日期：2015-08-22
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 站点接口
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public interface IPoint
    {
        /// <summary>
        /// 站点名称
        /// </summary>
        String PointName { get; }

        /// <summary>
        /// 站点ID
        /// </summary>
        String PointID { get; }

        /// <summary>
        /// 站点Guide
        /// </summary>
        String PointGuid { get; }
    }
}
