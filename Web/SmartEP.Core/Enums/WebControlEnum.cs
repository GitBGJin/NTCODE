using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.Core.Enums
{
    #region << SiteMap（测点、因子） >>
    /// <summary>
    /// SiteMap种类（0、测点；1、通道因子；2、用户；3、状态因子）
    /// </summary>
    [Flags]
    public enum CbxRsmType
    {
        /// <summary>
        /// 测点
        /// </summary>
        Point = 0x0,
        /// <summary>
        /// 通道因子
        /// </summary>
        ChannelFactor = 0x1,
        /// <summary>
        /// 用户
        /// </summary>
        User = 0x2,
        /// <summary>
        /// 状态因子
        /// </summary>
        StateFactor = 0x3
    }

    /// <summary>
    /// 测点筛选类型
    /// </summary>
    [Flags]
    public enum RsmPointMode
    {
        /// <summary>
        /// 站点级别（国控、省控、市控、市控移动站）
        /// </summary>
        Class = 0x0,
        /// <summary>
        /// 站点区域
        /// </summary>
        Region = 0x1,
        /// <summary>
        /// 站点类型
        /// </summary>
        Type = 0x2,
        /// <summary>
        /// 运维商
        /// </summary>
        Business = 0x3,
        /// <summary>
        /// 蓝藻
        /// </summary>
        BlueAlga = 0x4,
        /// <summary>
        /// 属性
        /// </summary>
        Property = 0x5
    }

    /// <summary>
    /// SiteMap返回值类型
    /// </summary>
    [Flags]
    public enum CbxRsmReturnType
    {
        /// <summary>
        /// Guid
        /// </summary>
        Guid = 0x0,
        /// <summary>
        /// ID
        /// </summary>
        ID = 0x1,
        /// <summary>
        /// Code
        /// </summary>
        Code = 0x2,
        /// <summary>
        /// Name
        /// </summary>
        Name = 0x3,
        /// <summary>
        /// ID:Guid:Name
        /// </summary>
        ID_Guid_Name = 0x4,
        /// <summary>
        /// Code:Guid:Name
        /// </summary>
        Code_Guid_Name = 0x5
    }
    #endregion
}
