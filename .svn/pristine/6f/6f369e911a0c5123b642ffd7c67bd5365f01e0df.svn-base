using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SmartEP.Core.Enums
{
    /// <summary>
    /// Copyright (c) 2013 , 江苏远大信息股份有限公司产品事业部
    /// MonitoringEnum.cs
    /// 创建人：JiKe
    /// 创建日期：2015-08-11
    /// 维护人员：
    /// 最新维护人员：刘晋
    /// 最新维护日期：2017-06-01
    /// 功能摘要：
    /// 枚举映射类
    /// </summary>
    /// <summary>
    /// 监测业务枚举类型
    /// </summary>
    [Flags]
    public enum PollutantDataType
    {
        /// <summary>
        /// 实时数据
        /// </summary>
        [Description("b2a59147-8ca5-40e9-8cec-f0d494b5ffc7")]
        RealTime = 0x0,
        /// <summary>
        /// 1分钟
        /// </summary>
        [Description("c36398ef-2bec-49be-8fca-b491fecaa359")]
        Min1 = 0x1,
        /// <summary>
        /// 5分钟
        /// </summary>
         [Description("7a894b1f-e990-4cc3-87bb-be1e431c46bf")]
        Min5 = 0x2,
        /// <summary>
        /// 10分钟
        /// </summary>
        [Description("3de5e8f7-9f8c-4ea1-a6a9-7edd84bafc2e")]
        Min10 = 0x3,
        /// <summary>
        /// 30分钟
        /// </summary>
         [Description("3e380038-b202-498c-bf7d-6d1dff19beea")]
        Min30 = 0x4,
        /// <summary>
        /// 原始60分钟
        /// </summary>
        [Description("1b6367f1-5287-4c14-b120-7a35bd176db1")]
        Min60 = 0x5,
        /// <summary>
        /// 原始日数据
        /// </summary>
        [Description("4a9d5015-f63d-402e-b1c1-e32f12e7b85e")]
        OriDay=0x13,

        /// <summary>
        /// 原始月数据
        /// </summary>
        [Description("34b5d74b-f33a-4fec-a369-3ee54caae706")]
        OriMonth = 0x14,
        /// <summary>
        /// 审核小时数据
        /// </summary>
        [Description("d6f304bf-98c4-422e-972a-521a61cb77df")]
        Hour = 0x6,
        /// <summary>
        /// 日数据
        /// </summary>
        [Description("9c270eec-feb5-43ab-9962-3f80b884abdc")]
        Day = 0x7,
        /// <summary>
        /// 周数据
        /// </summary>
        [Description("e12a6dfa-7319-412e-bc41-a0af136a2c08")]
        Week = 0x8,
        /// <summary>
        /// 月数据
        /// </summary>
        [Description("f15dbefa-9eee-4774-90f1-12db6150f532")]
        Month = 0x9,
        /// <summary>
        /// 季数据
        /// </summary>
        [Description("")]
        Season = 0x10,
        /// <summary>
        /// 年数据
        /// </summary>
        [Description("5b42fb0f-d339-4373-a041-b221c6c9d939")]
        Year = 0x11,
        /// <summary>
        /// 仪器状态数据
        /// </summary>
        [Description("")]
        InstrumentData60 = 0x12
    }

    /// <summary>
    /// 应用类型（空气、地表水、噪声）
    /// </summary>
    [Flags]
    public enum ApplicationType
    {
        /// <summary>
        /// 空气
        /// </summary>
        [Description("airaaira-aira-aira-aira-airaairaaira")]
        Air = 0x0,
        /// <summary>
        /// 地表水
        /// </summary>
        [Description("watrwatr-watr-watr-watr-watrwatrwatr")]
        Water = 0x1,
        /// <summary>
        /// 噪声
        /// </summary>
        [Description("noisnois-nois-nois-nois-noisnoisnois")]
        Noise = 0x2,
        /// <summary>
        /// 蓝藻
        /// </summary>
        [Description("watrwatr-watr-watr-watr-watrwatrwatr")]
        BlueAlga = 0x3
    }

    /// <summary>
    /// 应用程序数值
    /// </summary>
    [Flags]
    public enum ApplicationValue
    {
        /// <summary>
        /// 空气
        /// </summary>
        Air = 0x0,
        /// <summary>
        /// 地表水
        /// </summary>
        Water = 0x1,
        /// <summary>
        /// 噪声
        /// </summary>
        Noise = 0x2,
        /// <summary>
        /// VOCs
        /// </summary>
        VOCs = 0x3,
        /// <summary>
        /// 烟气
        /// </summary>
        CEMS = 0x4,
        /// <summary>
        /// 废水
        /// </summary>
        WasteWater = 0x5
    }

    /// <summary>
    /// 污染物类型数值
    /// </summary>
    [Flags]
    public enum PollutantTypeValue
    {
        /// <summary>
        /// 大气污染物
        /// </summary>
        Air = 0x0,
        /// <summary>
        /// 水污染物
        /// </summary>
        Water = 0x1,
        /// <summary>
        /// 噪声污染物
        /// </summary>
        Noise = 0x2

    }

    /// <summary>
    /// AQI分指数类型
    /// </summary>
    [Flags]
    public enum IAQIType
    {
        /// <summary>
        /// SO2
        /// </summary>
        SO2_IAQI = 0x0,
        /// <summary>
        /// NO2
        /// </summary>
        NO2_IAQI = 0x1,
        /// <summary>
        /// PM10
        /// </summary>
        PM10_IAQI = 0x2,
        /// <summary>
        /// CO
        /// </summary>
        CO_IAQI = 0x3,
        /// <summary>
        /// PM25
        /// </summary>
        PM25_IAQI = 0x4,
        /// <summary>
        /// O3 1小时
        /// </summary>
        MaxOneHourO3_IAQI = 0x5,
        /// <summary>
        /// O3 8小时
        /// </summary>
        Max8HourO3_IAQI = 0x6,
        /// <summary>
        /// PM10 24小时滑动均值
        /// </summary>
        Recent24HoursPM10_IAQI = 0x7,
        /// <summary>
        /// PM2.5 24小时滑动均值
        /// </summary>
        Recent24HoursPM25_IAQI = 0x8,
        /// <summary>
        /// AQIValue
        /// </summary>
        AQIValue = 0x9
    }
}
