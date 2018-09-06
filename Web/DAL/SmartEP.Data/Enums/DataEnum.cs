using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SmartEP.Data.Enums
{
    /// <summary>
    /// 自动监测数据库连接
    /// </summary>
    [Flags]
    public enum DataConnectionType
    {
        /// <summary>
        /// 基础数据库
        /// </summary>
        BaseData = 0x0,
        /// <summary>
        /// 空气实时监控
        /// </summary>
        AirAutoMonitoring = 0x1,
        /// <summary>
        /// 地表水实时监控
        /// </summary>
        WaterAutoMonitoring = 0x2,
        /// <summary>
        /// 业务数据库
        /// </summary>
        MonitoringBusiness = 0x3,
        /// <summary>
        /// 框架数据库
        /// </summary>
        Frame = 0x4
    }

    /// <summary>
    /// AQI数据类型
    /// </summary>
    [Flags]
    public enum AQIDataType
    {
        /// <summary>
        /// 小时AQI
        /// </summary>
        HourAQI = 0x0,
        /// <summary>
        /// 小时API
        /// </summary>
        HourAPI = 0x1,
        /// <summary>
        /// 日AQI
        /// </summary>
        DayAQI = 0x2,
        /// <summary>
        /// 日API
        /// </summary>
        DayAPI = 0x3,
        /// <summary>
        /// 区域小时AQI
        /// </summary>
        RegionHourAQI = 0x4,
        /// <summary>
        /// 区域日AQI
        /// </summary>
        RegionDayAQI = 0x5,
        /// <summary>
        /// 区域日API
        /// </summary>
        RegionDayAPI = 0x6,
        /// <summary>
        /// 原始小时AQI
        /// </summary>
        OriHourAQI = 0x7
    }

    /// <summary>
    /// 区域AQI统计类型
    /// </summary>
    [Flags]
    public enum RegionAQIStatisticalType
    {
        /// <summary>
        /// 常规统计
        /// </summary>
        [Description("CG")]
        Conventional = 0x0,
        /// <summary>
        /// 创模点统计
        /// </summary>
        [Description("CM")]
        Model = 0x1
    }

}
