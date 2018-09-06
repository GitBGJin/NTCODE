﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.Core.Interfaces
{
    /// <summary>
    /// 名称：IPollutant.cs
    /// 创建人：李飞
    /// 创建日期：2015-08-22
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 因子接口
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public interface IPollutant
    {
        /// <summary>
        /// 名称
        /// </summary>
        String PollutantName { get; }

        /// <summary>
        /// 编码
        /// </summary>
        String PollutantCode { get; }

        /// <summary>
        /// 小数位
        /// </summary>
        String PollutantDecimalNum { get; }

        /// <summary>
        /// 单位
        /// </summary>
        String PollutantMeasureUnit { get; }

        /// <summary>
        /// Guid
        /// </summary>
        String PollutantGuid { get; }

        /// <summary>
        /// 排序
        /// </summary>
        Int32 OrderNumber { get; }

        ///// <summary>
        ///// 对应范围
        ///// </summary>
        //String PollutantNewName { get; }
    }
}
