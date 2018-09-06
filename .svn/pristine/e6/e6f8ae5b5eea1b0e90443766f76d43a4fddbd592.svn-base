using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SmartEP.Service.DataAnalyze.Enums
{
    /// <summary>
    /// 数据采集状态（在线、离线、报警、故障、停运、始终在线）
    /// </summary>
    [Flags]
    public enum DataSamplingStatus
    {
        /// <summary>
        /// 在线 1
        /// </summary>
        [Description("1")]
        Online = 0x1,
        /// <summary>
        /// 离线 2
        /// </summary>
        [Description("2")]
        Offline = 0x2,
        /// <summary>
        /// 报警 4
        /// </summary>
        [Description("4")]
        Alarm = 0x3,
        /// <summary>
        /// 故障 8
        /// </summary>
        [Description("8")]
        Failure = 0x4,
        /// <summary>
        /// 停运 16
        /// </summary>
        [Description("16")]
        Stop = 0x5,
        /// <summary>
        /// 始终在线 32
        /// </summary>
        [Description("32")]
        AlwaysOnline = 0x6
    }
}
