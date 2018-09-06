using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SmartEP.Service.Core.Enums
{
    /// <summary>
    /// 空气质量指数类别
    /// </summary>
    [Flags]
    public enum AQIClass
    {
        /// <summary>
        /// 优
        /// </summary>
        [Description("16AFCA7A-8EE0-44F7-917E-24286953736B")]
        Good = 0x0,
        /// <summary>
        /// 良
        /// </summary>
        [Description("D7B3866A-724B-41A5-8E33-BB574A028AC1")]
        Moderate = 0x1,
        /// <summary>
        ///轻度污染
        /// </summary>
        [Description("57734A2B-5FA8-47BA-99B7-89F781665D34")]
        LightlyPolluted = 0x2,
        /// <summary>
        ///中度污染
        /// </summary>
        [Description("DFE7307C-7134-460F-B60F-2D2461E99522")]
        ModeratelyPolluted = 0x3,
        /// <summary>
        ///重度污染
        /// </summary>
        [Description("D776B5A1-9A91-4AC7-87FE-B58A30B31614")]
        HeavilyPolluted = 0x4,
        /// <summary>
        /// 严重污染
        /// </summary>
        [Description("78FAA721-4002-4B16-B2F4-7F9718311355")]
        SeverelyPolluted = 0x5
    }

    /// <summary>
    /// 空气质量指数级别
    /// </summary>
    [Flags]
    public enum AQIGrade
    {
        /// <summary>
        /// 一级
        /// </summary>
        One = 0x0,
        /// <summary>
        /// 二级
        /// </summary>
        Two = 0x1,
        /// <summary>
        ///三级
        /// </summary>
        Three = 0x2,
        /// <summary>
        ///四级
        /// </summary>
        Four = 0x3,
        /// <summary>
        ///五级
        /// </summary>
        Five = 0x4,
        /// <summary>
        /// 六级
        /// </summary>
        Six = 0x5
    }

    /// <summary>
    /// 水质类别
    /// </summary>
    [Flags]
    public enum WaterQualityClass
    {
        /// <summary>
        /// I类
        /// </summary>
        [Description("87A0AB76-1FA6-40EF-A310-382D1B662476")]
        One = 0x1,
        /// <summary>
        /// II类
        /// </summary>
        [Description("0454152A-5AD1-456F-B95F-40C9D57569A3")]
        Two = 0x2,
        /// <summary>
        ///III类
        /// </summary>
        [Description("30492B50-47E3-4661-B2D9-F791D10EA117")]
        Three = 0x3,
        /// <summary>
        ///IV类
        /// </summary>
        [Description("F4E62E3E-4CBB-4D1C-A537-8FD5588B055B")]
        Four = 0x4,
        /// <summary>
        ///V类
        /// </summary>
        [Description("5D7D4183-E82E-4BF3-9CD9-98377C73C209")]
        Five = 0x5,
        /// <summary>
        /// 劣V类
        /// </summary>
        [Description("F22A2F8F-1E5E-4AE4-8BFF-C4F540B9ECAF")]
        BadFive = 0x6
    }

    /// <summary>
    /// 污染源浓度值限时间类型
    /// </summary>
    [Flags]
    public enum EQITimeType
    {
        /// <summary>
        /// 1小时
        /// </summary>
        [Description("7c67a857-d602-4f90-a26d-edd3e9f4d36c")]
        One = 0x0,
        /// <summary>
        /// 8小时
        /// </summary>
        [Description("1cc20274-210c-4c22-8fe8-553b46ddf112")]
        Eight = 0x1,
        /// <summary>
        ///24小时
        /// </summary>
        [Description("a7056afa-9c7f-4876-8853-9c95c5d7e2b3")]
        TwentyFour = 0x2,
        /// <summary>
        /// 年均值
        /// </summary>
        [Description("815258fb-846b-4320-93f6-ab036b0e8531")]
        Year = 0x3
    }

    /// <summary>
    /// 地表水水质评价站点属性类型
    /// </summary>
    [Flags]
    public enum WaterPointCalWQType
    {
        /// <summary>
        /// 河流
        /// </summary>
        [Description("d8197909-568e-4319-874c-3ad7cbc92a7e")]
        River = 0x0,
        /// <summary>
        /// 湖、库
        /// </summary>
        [Description("e82cd86f-71ba-4f87-8e5c-6ac7ca055a6b")]
        Lake = 0x1

    }

    /// <summary>
    /// 计算AQI或水质时返回值类型
    /// </summary>
    [Flags]
    public enum EQIReurnType
    {
        /// <summary>
        /// 指数等级范围
        /// </summary>
        [Description("Range")]
        Range = 0x0,
        /// <summary>
        /// 质量指数级别
        /// </summary>
        [Description("Grade")]
        Grade = 0x1,
        /// <summary>
        /// 等级的罗马字符
        /// </summary>
        [Description("Roman")]
        Roman = 0x2,
        /// <summary>
        /// 质量指数类别
        /// </summary>
        [Description("Class")]
        Class = 0x3,
        /// <summary>
        /// 对应显示颜色
        /// </summary>
        [Description("Color")]
        Color = 0x4,
        /// <summary>
        /// RGB颜色值
        /// </summary>
        [Description("RGBValue")]
        RGBValue = 0x5,
        /// <summary>
        /// 对健康影响情况
        /// </summary>
        [Description("HealthEffect")]
        HealthEffect = 0x6,
        /// <summary>
        /// 建议采取的措施
        /// </summary>
        [Description("TakeStep")]
        TakeStep = 0x7,
        /// <summary>
        /// 等级值
        /// </summary>
        [Description("Level")]
        Level = 0x8,
        /// <summary>
        /// 编码
        /// </summary>
        [Description("CODE")]
        Code = 0x9,
        /// <summary>
        /// 名称
        /// </summary>
        [Description("NAME")]
        Name = 0x10,
        /// <summary>
        /// 英文名
        /// </summary>
        [Description("ENAME")]
        EnglishName = 0x11,
        /// <summary>
        /// 值
        /// </summary>
        [Description("VALUE")]
        Value = 0x12,
        /// <summary>
        /// 简写
        /// </summary>
        [Description("SIMPLE")]
        ShortName = 0x13
    }

}
