namespace SmartEP.Service.DataAnalyze.PollutionIndex
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class PollutionIndexLibrary
    {
        #region Properties

        /// <summary>
        /// 等级标准
        /// </summary>
        private static List<PollutionStandard> PollutionLevelStandards
        {
            get
            {
                return new System.Collections.Generic.List<PollutionStandard>() {
                #region AQI
                                new PollutionStandard(){
                                    Min=0,
                                    Max=50,
                                    Range="0~50",
                                    Level="一级",
                                    Roman="I",
                                    LevelType="优",
                                    ColorCode="#00e400",
                                    Color="绿色",
                                    HealthEffect="空气质量令人满意，基本无空气污染",
                                    Suggestion="各类人群可正常活动",
                                    PollutionIndex_Type=Enums.PollutionIndexType.AQI
                                },
                                new PollutionStandard(){
                                    Min=51,
                                    Max=100,
                                    Range="51~100",
                                    Level="二级",
                                    Roman="II",
                                    LevelType="良",
                                    ColorCode="#ffff00",
                                    Color="黄色",
                                    HealthEffect="空气质量可接受，但某些污染物可能对极少数异常敏感人群健康有较弱影响",
                                    Suggestion="极少数异常敏感人群应减少户外活动",
                                    PollutionIndex_Type=Enums.PollutionIndexType.AQI
                                },
                                new PollutionStandard(){
                                    Min=101,
                                    Max=150,
                                    Range="101~150",
                                    Level="三级",
                                    Roman="III",
                                    LevelType="轻度污染",
                                    ColorCode="#ff7e00",
                                    Color="橙色",
                                    HealthEffect="易感人群症状有轻度加剧，健康人群出现刺激症状",
                                    Suggestion="儿童、老年人及心脏病、呼吸系统疾病患者应减少长时间、高强度的户外锻炼",
                                    PollutionIndex_Type=Enums.PollutionIndexType.AQI
                                },
                                new PollutionStandard(){
                                    Min=151,
                                    Max=200,
                                    Range="151~200",
                                    Level="四级",
                                    Roman="IV",
                                    LevelType="中度污染",
                                    ColorCode="#ff0000",
                                    Color="红色",
                                    HealthEffect="进一步加剧易感人群症状，可能对 健康人群心脏、呼吸系统有影响",
                                    Suggestion="儿童、老年人及心脏病、呼吸系统 疾病患者避免长时间、高强度的户 外锻练，一般人群适量减少户外运动",
                                    PollutionIndex_Type=Enums.PollutionIndexType.AQI
                                },
                                new PollutionStandard(){
                                    Min=201,
                                    Max=300,
                                    Range="201~300",
                                    Level="五级",
                                    Roman="V",
                                    LevelType="重度污染",
                                    ColorCode="#99004c",
                                    Color="紫色",
                                    HealthEffect="心脏病和肺病患者症状显著加剧，运动耐受力降低，健康人群普遍出 现症状",
                                    Suggestion="儿童、老年人和心脏病、肺病患者应停留在室内，停止户外运动，一 般人群减少户外运动",
                                    PollutionIndex_Type=Enums.PollutionIndexType.AQI
                                },
                                new PollutionStandard(){
                                    Min=301,
                                    Range=">300",
                                    Level="六级",
                                    Roman="VI",
                                    LevelType="严重污染",
                                    ColorCode="#7e0023",
                                    Color="褐色",
                                    HealthEffect="健康人群运动耐受力降低，有明显 强烈症状，提前出现某些疾病",
                                    Suggestion="儿童、老年人和病人应当留在室内，避免体力消耗，一般人群应避免户外活动",
                                    PollutionIndex_Type=Enums.PollutionIndexType.AQI
                                }
                #endregion
                                ,
                #region API
                             new PollutionStandard(){
                                Min=1,
                                Max=50,
                                Range="0~50",
                                Level="一级",
                                Roman="I",
                                LevelType="优",
                                ColorCode="#00e400",
                                //Color="绿色",
                                HealthEffect="空气质量令人满意，基本无空气污染",
                                Suggestion="各类人群可正常活动",
                                PollutionIndex_Type=Enums.PollutionIndexType.API
                            },
                            new PollutionStandard(){
                                Min=51,
                                Max=100,
                                Range="51~100",
                                Level="二级",
                                Roman="II",
                                LevelType="良",
                                ColorCode="#ffff00",
                                //Color="绿色",
                                HealthEffect="空气质量令人满意，基本无空气污染",
                                Suggestion="各类人群可正常活动",
                                PollutionIndex_Type=Enums.PollutionIndexType.API
                            },
                            new PollutionStandard(){
                                Min=101,
                                Max=150,
                                Range="101~150",
                                Level="三级",
                                Roman="III",
                                LevelType="轻微污染",
                                ColorCode="#ff7e00",
                                //Color="绿色",
                                HealthEffect="易感人群症状有轻度加剧，健康人群出现刺激症状",
                                Suggestion="心脏病和呼吸系统疾病患者应减少体力消耗和户外活动",
                                PollutionIndex_Type=Enums.PollutionIndexType.API
                            },
                            new PollutionStandard(){
                                Min=151,
                                Max=200,
                                Range="151~200",
                                Level="四级",
                                Roman="IV",
                                LevelType="轻度污染",
                                ColorCode="#ff0000",
                                //Color="绿色",
                                HealthEffect="易感人群症状有轻度加剧，健康人群出现刺激症状",
                                Suggestion="心脏病和呼吸系统疾病患者应减少体力消耗和户外活动",
                                PollutionIndex_Type=Enums.PollutionIndexType.API
                            },
                            new PollutionStandard(){
                                Min=201,
                                Max=250,
                                Range="201~250",
                                Level="五级",
                                Roman="V",
                                LevelType="中度污染",
                                ColorCode="#99004c",
                                //Color="绿色",
                                HealthEffect="心脏病和肺病患者症状显著加剧，运动耐受力降低，健康人群普遍出现症状",
                                Suggestion="儿童、老年人和心脏病、肺病患者应停留在室内，停止户外运动，一般人群减少户外运动",
                                PollutionIndex_Type=Enums.PollutionIndexType.API
                            },
                            new PollutionStandard(){
                                Min=251,
                                Max=300,
                                Range="251~300",
                                Level="六级",
                                Roman="VI",
                                LevelType="中度重污染",
                                ColorCode="#99004c",
                                //Color="绿色",
                                HealthEffect="心脏病和肺病患者症状显著加剧，运动耐受力降低，健康人群普遍出现症状",
                                Suggestion="儿童、老年人和心脏病、肺病患者应停留在室内，停止户外运动，一般人群减少户外运动",
                                PollutionIndex_Type=Enums.PollutionIndexType.API
                            },
                            new PollutionStandard(){
                                Min=300,
                                Range=">300",
                                Level="七级",
                                Roman="VII",
                                LevelType="重污染",
                                ColorCode="#7e0023",
                                //Color="绿色",
                                HealthEffect="健康人群运动耐受力降低，有明显强烈症状，提前出现某些疾病'",
                                Suggestion="儿童、老年人和病人应当留在室内，避免体力消耗，一般人群应避免户外活动",
                            }
                #endregion
               };
            }
        }

        /// <summary>
        /// 因子标准
        /// </summary>
        private static List<PollutionIndexStandard> Standards
        {
            get
            {
                return new List<PollutionIndexStandard>()
                {
                #region AQI
                                    #region  SO2 24小时
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="SO2",
                                        FactorCNName="二氧化硫",
                                        CaculatorType=Enums.CaculatorType.TwentyHours,
                                        Unit=@"mg/m3",
                                        MinConcentration=0M,
                                        MaxConcentration=0.05M,
                                        MinPollutionIndex=0,
                                        MaxPollutionIndex=50,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="SO2",
                                        FactorCNName="二氧化硫",
                                        CaculatorType=Enums.CaculatorType.TwentyHours,
                                        Unit=@"mg/m3",
                                        MinConcentration=0.05M,
                                        MaxConcentration=0.15M,
                                        MinPollutionIndex=51,
                                        MaxPollutionIndex=100,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="SO2",
                                        FactorCNName="二氧化硫",
                                        CaculatorType=Enums.CaculatorType.TwentyHours,
                                        Unit=@"mg/m3",
                                        MinConcentration=0.15M,
                                        MaxConcentration=0.475M,
                                        MinPollutionIndex=101,
                                        MaxPollutionIndex=150,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="SO2",
                                        FactorCNName="二氧化硫",
                                        CaculatorType=Enums.CaculatorType.TwentyHours,
                                        Unit=@"mg/m3",
                                        MinConcentration=0.475M,
                                        MaxConcentration=0.8M,
                                        MinPollutionIndex=151,
                                        MaxPollutionIndex=200,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="SO2",
                                        FactorCNName="二氧化硫",
                                        CaculatorType=Enums.CaculatorType.TwentyHours,
                                        Unit=@"mg/m3",
                                        MinConcentration=0.8M,
                                        MaxConcentration=1.6M,
                                        MinPollutionIndex=201,
                                        MaxPollutionIndex=300,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="SO2",
                                        FactorCNName="二氧化硫",
                                        CaculatorType=Enums.CaculatorType.TwentyHours,
                                        Unit=@"mg/m3",
                                        MinConcentration=1.6M,
                                        MaxConcentration=2.1M,
                                        MinPollutionIndex=301,
                                        MaxPollutionIndex=400,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="SO2",
                                        FactorCNName="二氧化硫",
                                        CaculatorType=Enums.CaculatorType.TwentyHours,
                                        Unit=@"mg/m3",
                                        MinConcentration=2.1M,
                                        MaxConcentration=2.62M,
                                        MinPollutionIndex=401,
                                        MaxPollutionIndex=500,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    #endregion
                                    #region  SO2 1小时
                                        new PollutionIndexStandard()
                                    {
                                        FactorName="SO2",
                                        FactorCNName="二氧化硫",
                                        CaculatorType=Enums.CaculatorType.OneHour,
                                        Unit=@"mg/m3",
                                        MinConcentration=0M,
                                        MaxConcentration=0.15M,
                                        MinPollutionIndex=0,
                                        MaxPollutionIndex=50,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="SO2",
                                        FactorCNName="二氧化硫",
                                        CaculatorType=Enums.CaculatorType.OneHour,
                                        Unit=@"mg/m3",
                                        MinConcentration=0.15M,
                                        MaxConcentration=0.5M,
                                        MinPollutionIndex=51,
                                        MaxPollutionIndex=100,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="SO2",
                                        FactorCNName="二氧化硫",
                                        CaculatorType=Enums.CaculatorType.OneHour,
                                        Unit=@"mg/m3",
                                        MinConcentration=0.5M,
                                        MaxConcentration=0.65M,
                                        MinPollutionIndex=101,
                                        MaxPollutionIndex=150,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="SO2",
                                        FactorCNName="二氧化硫",
                                        CaculatorType=Enums.CaculatorType.OneHour,
                                        Unit=@"mg/m3",
                                        MinConcentration=0.65M,
                                        MaxConcentration=0.8M,
                                        MinPollutionIndex=151,
                                        MaxPollutionIndex=200,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="SO2",
                                        FactorCNName="二氧化硫",
                                        CaculatorType=Enums.CaculatorType.OneHour,
                                        Unit=@"mg/m3",
                                        MinPollutionIndex=200,
                                        MaxPollutionIndex=300,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="SO2",
                                        FactorCNName="二氧化硫",
                                        CaculatorType=Enums.CaculatorType.OneHour,
                                        Unit=@"mg/m3",
                                        MinPollutionIndex=301,
                                        MaxPollutionIndex=400,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="SO2",
                                        FactorCNName="二氧化硫",
                                        CaculatorType=Enums.CaculatorType.OneHour,
                                        Unit=@"mg/m3",
                                        MinPollutionIndex=401,
                                        MaxPollutionIndex=500,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    #endregion
                                    #region  NO2 24小时
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="NO2",
                                        FactorCNName="二氧化氮",
                                        CaculatorType=Enums.CaculatorType.TwentyHours,
                                        Unit=@"mg/m3",
                                        MinConcentration=0M,
                                        MaxConcentration=0.04M,
                                        MinPollutionIndex=0,
                                        MaxPollutionIndex=50,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="NO2",
                                        FactorCNName="二氧化氮",
                                        CaculatorType=Enums.CaculatorType.TwentyHours,
                                        Unit=@"mg/m3",
                                        MinConcentration=0.04M,
                                        MaxConcentration=0.08M,
                                        MinPollutionIndex=51,
                                        MaxPollutionIndex=100,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="NO2",
                                        FactorCNName="二氧化氮",
                                        CaculatorType=Enums.CaculatorType.TwentyHours,
                                        Unit=@"mg/m3",
                                        MinConcentration=0.08M,
                                        MaxConcentration=0.18M,
                                        MinPollutionIndex=101,
                                        MaxPollutionIndex=150,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="NO2",
                                        FactorCNName="二氧化氮",
                                        CaculatorType=Enums.CaculatorType.TwentyHours,
                                        Unit=@"mg/m3",
                                        MinConcentration=0.18M,
                                        MaxConcentration=0.28M,
                                        MinPollutionIndex=151,
                                        MaxPollutionIndex=200,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="NO2",
                                        FactorCNName="二氧化氮",
                                        CaculatorType=Enums.CaculatorType.TwentyHours,
                                        Unit=@"mg/m3",
                                        MinConcentration=0.28M,
                                        MaxConcentration=0.565M,
                                        MinPollutionIndex=201,
                                        MaxPollutionIndex=300,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="NO2",
                                        FactorCNName="二氧化氮",
                                        CaculatorType=Enums.CaculatorType.TwentyHours,
                                        Unit=@"mg/m3",
                                        MinConcentration=0.565M,
                                        MaxConcentration=0.75M,
                                        MinPollutionIndex=301,
                                        MaxPollutionIndex=400,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="NO2",
                                        FactorCNName="二氧化氮",
                                        CaculatorType=Enums.CaculatorType.TwentyHours,
                                        Unit=@"mg/m3",
                                        MinConcentration=0.75M,
                                        MaxConcentration=0.94M,
                                        MinPollutionIndex=401,
                                        MaxPollutionIndex=500,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    #endregion
                                    #region  NO2 1小时
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="NO2",
                                        FactorCNName="二氧化氮",
                                        CaculatorType=Enums.CaculatorType.OneHour,
                                        Unit=@"mg/m3",
                                        MinConcentration=0M,
                                        MaxConcentration=0.1M,
                                        MinPollutionIndex=0,
                                        MaxPollutionIndex=50,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="NO2",
                                        FactorCNName="二氧化氮",
                                        CaculatorType=Enums.CaculatorType.OneHour,
                                        Unit=@"mg/m3",
                                        MinConcentration=0.1M,
                                        MaxConcentration=0.2M,
                                        MinPollutionIndex=51,
                                        MaxPollutionIndex=100,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="NO2",
                                        FactorCNName="二氧化氮",
                                        CaculatorType=Enums.CaculatorType.OneHour,
                                        Unit=@"mg/m3",
                                        MinConcentration=0.2M,
                                        MaxConcentration=0.7M,
                                        MinPollutionIndex=101,
                                        MaxPollutionIndex=150,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="NO2",
                                        FactorCNName="二氧化氮",
                                        CaculatorType=Enums.CaculatorType.OneHour,
                                        Unit=@"mg/m3",
                                        MinConcentration=0.7M,
                                        MaxConcentration=1.2M,
                                        MinPollutionIndex=151,
                                        MaxPollutionIndex=200,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="NO2",
                                        FactorCNName="二氧化氮",
                                        CaculatorType=Enums.CaculatorType.OneHour,
                                        Unit=@"mg/m3",
                                        MinConcentration=1.2M,
                                        MaxConcentration=2.34M,
                                        MinPollutionIndex=201,
                                        MaxPollutionIndex=300,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="NO2",
                                        FactorCNName="二氧化氮",
                                        CaculatorType=Enums.CaculatorType.OneHour,
                                        Unit=@"mg/m3",
                                        MinConcentration=2.34M,
                                        MaxConcentration=3.09M,
                                        MinPollutionIndex=301,
                                        MaxPollutionIndex=400,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="NO2",
                                        FactorCNName="二氧化氮",
                                        CaculatorType=Enums.CaculatorType.OneHour,
                                        Unit=@"mg/m3",
                                        MinConcentration=3.09M,
                                        MaxConcentration=3.84M,
                                        MinPollutionIndex=401,
                                        MaxPollutionIndex=500,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    #endregion
                                    #region  PM10 24小时
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="PM10",
                                        FactorCNName="PM10",
                                        CaculatorType=Enums.CaculatorType.TwentyHours,
                                        Unit=@"mg/m3",
                                        MinConcentration=0M,
                                        MaxConcentration=0.05M,
                                        MinPollutionIndex=0,
                                        MaxPollutionIndex=50,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="PM10",
                                        FactorCNName="PM10",
                                        CaculatorType=Enums.CaculatorType.TwentyHours,
                                        Unit=@"mg/m3",
                                        MinConcentration=0.05M,
                                        MaxConcentration=0.15M,
                                        MinPollutionIndex=51,
                                        MaxPollutionIndex=100,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="PM10",
                                        FactorCNName="PM10",
                                        CaculatorType=Enums.CaculatorType.TwentyHours,
                                        Unit=@"mg/m3",
                                        MinConcentration=0.15M,
                                        MaxConcentration=0.25M,
                                        MinPollutionIndex=101,
                                        MaxPollutionIndex=150,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="PM10",
                                        FactorCNName="PM10",
                                        CaculatorType=Enums.CaculatorType.TwentyHours,
                                        Unit=@"mg/m3",
                                        MinConcentration=0.25M,
                                        MaxConcentration=0.35M,
                                        MinPollutionIndex=151,
                                        MaxPollutionIndex=200,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="PM10",
                                        FactorCNName="PM10",
                                        CaculatorType=Enums.CaculatorType.TwentyHours,
                                        Unit=@"mg/m3",
                                        MinConcentration=0.35M,
                                        MaxConcentration=0.42M,
                                        MinPollutionIndex=201,
                                        MaxPollutionIndex=300,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                       FactorName="PM10",
                                        FactorCNName="PM10",
                                        CaculatorType=Enums.CaculatorType.TwentyHours,
                                        Unit=@"mg/m3",
                                        MinConcentration=0.42M,
                                        MaxConcentration=0.5M,
                                        MinPollutionIndex=301,
                                        MaxPollutionIndex=400,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="PM10",
                                        FactorCNName="PM10",
                                        CaculatorType=Enums.CaculatorType.TwentyHours,
                                        Unit=@"mg/m3",
                                        MinConcentration=0.5M,
                                        MaxConcentration=0.6M,
                                        MinPollutionIndex=401,
                                        MaxPollutionIndex=500,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    #endregion
                                    #region  一氧化碳 24小时
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="CO",
                                        FactorCNName="一氧化碳",
                                        CaculatorType=Enums.CaculatorType.TwentyHours,
                                        Unit=@"mg/m3",
                                        MinConcentration=0M,
                                        MaxConcentration=2M,
                                        MinPollutionIndex=0,
                                        MaxPollutionIndex=50,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="CO",
                                        FactorCNName="一氧化碳",
                                        CaculatorType=Enums.CaculatorType.TwentyHours,
                                        Unit=@"mg/m3",
                                        MinConcentration=2M,
                                        MaxConcentration=4M,
                                        MinPollutionIndex=51,
                                        MaxPollutionIndex=100,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="CO",
                                        FactorCNName="一氧化碳",
                                        CaculatorType=Enums.CaculatorType.TwentyHours,
                                        Unit=@"mg/m3",
                                        MinConcentration=4M,
                                        MaxConcentration=14M,
                                        MinPollutionIndex=101,
                                        MaxPollutionIndex=150,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="CO",
                                        FactorCNName="一氧化碳",
                                        CaculatorType=Enums.CaculatorType.TwentyHours,
                                        Unit=@"mg/m3",
                                        MinConcentration=14M,
                                        MaxConcentration=24M,
                                        MinPollutionIndex=151,
                                        MaxPollutionIndex=200,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="CO",
                                        FactorCNName="一氧化碳",
                                        CaculatorType=Enums.CaculatorType.TwentyHours,
                                        Unit=@"mg/m3",
                                        MinConcentration=24M,
                                        MaxConcentration=36M,
                                        MinPollutionIndex=201,
                                        MaxPollutionIndex=300,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="CO",
                                        FactorCNName="一氧化碳",
                                        CaculatorType=Enums.CaculatorType.TwentyHours,
                                        Unit=@"mg/m3",
                                        MinConcentration=36M,
                                        MaxConcentration=48M,
                                        MinPollutionIndex=301,
                                        MaxPollutionIndex=400,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="CO",
                                        FactorCNName="一氧化碳",
                                        CaculatorType=Enums.CaculatorType.TwentyHours,
                                        Unit=@"mg/m3",
                                        MinConcentration=48M,
                                        MaxConcentration=60M,
                                        MinPollutionIndex=401,
                                        MaxPollutionIndex=500,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    #endregion
                                    #region  一氧化碳 1小时
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="CO",
                                        FactorCNName="一氧化碳",
                                        CaculatorType=Enums.CaculatorType.OneHour,
                                        Unit=@"mg/m3",
                                        MinConcentration=0M,
                                        MaxConcentration=5M,
                                        MinPollutionIndex=0,
                                        MaxPollutionIndex=50,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="CO",
                                        FactorCNName="一氧化碳",
                                        CaculatorType=Enums.CaculatorType.OneHour,
                                        Unit=@"mg/m3",
                                        MinConcentration=5M,
                                        MaxConcentration=10M,
                                        MinPollutionIndex=51,
                                        MaxPollutionIndex=100,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="CO",
                                        FactorCNName="一氧化碳",
                                        CaculatorType=Enums.CaculatorType.OneHour,
                                        Unit=@"mg/m3",
                                        MinConcentration=10M,
                                        MaxConcentration=35M,
                                        MinPollutionIndex=101,
                                        MaxPollutionIndex=150,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="CO",
                                        FactorCNName="一氧化碳",
                                        CaculatorType=Enums.CaculatorType.OneHour,
                                        Unit=@"mg/m3",
                                        MinConcentration=35M,
                                        MaxConcentration=60M,
                                        MinPollutionIndex=151,
                                        MaxPollutionIndex=200,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="CO",
                                        FactorCNName="一氧化碳",
                                        CaculatorType=Enums.CaculatorType.OneHour,
                                        Unit=@"mg/m3",
                                        MinConcentration=60M,
                                        MaxConcentration=90M,
                                        MinPollutionIndex=201,
                                        MaxPollutionIndex=300,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="CO",
                                        FactorCNName="一氧化碳",
                                        CaculatorType=Enums.CaculatorType.OneHour,
                                        Unit=@"mg/m3",
                                        MinConcentration=90M,
                                        MaxConcentration=120M,
                                        MinPollutionIndex=301,
                                        MaxPollutionIndex=400,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="CO",
                                        FactorCNName="一氧化碳",
                                        CaculatorType=Enums.CaculatorType.OneHour,
                                        Unit=@"mg/m3",
                                        MinConcentration=120M,
                                        MaxConcentration=150M,
                                        MinPollutionIndex=401,
                                        MaxPollutionIndex=500,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    #endregion
                                    #region  臭氧 1小时
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="O3",
                                        FactorCNName="臭氧",
                                        CaculatorType=Enums.CaculatorType.OneHour,
                                        Unit=@"mg/m3",
                                        MinConcentration=0M,
                                        MaxConcentration=0.16M,
                                        MinPollutionIndex=0,
                                        MaxPollutionIndex=50,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="O3",
                                        FactorCNName="臭氧",
                                        CaculatorType=Enums.CaculatorType.OneHour,
                                        Unit=@"mg/m3",
                                        MinConcentration=0.16M,
                                        MaxConcentration=0.2M,
                                        MinPollutionIndex=51,
                                        MaxPollutionIndex=100,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="O3",
                                        FactorCNName="臭氧",
                                        CaculatorType=Enums.CaculatorType.OneHour,
                                        Unit=@"mg/m3",
                                        MinConcentration=0.2M,
                                        MaxConcentration=0.3M,
                                        MinPollutionIndex=101,
                                        MaxPollutionIndex=150,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="O3",
                                        FactorCNName="臭氧",
                                        CaculatorType=Enums.CaculatorType.OneHour,
                                        Unit=@"mg/m3",
                                        MinConcentration=0.3M,
                                        MaxConcentration=0.4M,
                                        MinPollutionIndex=151,
                                        MaxPollutionIndex=200,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="O3",
                                        FactorCNName="臭氧",
                                        CaculatorType=Enums.CaculatorType.OneHour,
                                        Unit=@"mg/m3",
                                        MinConcentration=0.4M,
                                        MaxConcentration=0.8M,
                                        MinPollutionIndex=201,
                                        MaxPollutionIndex=300,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="O3",
                                        FactorCNName="臭氧",
                                        CaculatorType=Enums.CaculatorType.OneHour,
                                        Unit=@"mg/m3",
                                        MinConcentration=0.8M,
                                        MaxConcentration=1M,
                                        MinPollutionIndex=301,
                                        MaxPollutionIndex=400,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="O3",
                                        FactorCNName="臭氧",
                                        CaculatorType=Enums.CaculatorType.OneHour,
                                        Unit=@"mg/m3",
                                        MinConcentration=1M,
                                        MaxConcentration=1.2M,
                                        MinPollutionIndex=401,
                                        MaxPollutionIndex=500,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    #endregion
                                    #region  臭氧 8小时
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="O3",
                                        FactorCNName="臭氧",
                                        CaculatorType=Enums.CaculatorType.EightHours,
                                        Unit=@"mg/m3",
                                        MinConcentration=0M,
                                        MaxConcentration=0.1M,
                                        MinPollutionIndex=0,
                                        MaxPollutionIndex=50,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="O3",
                                        FactorCNName="臭氧",
                                        CaculatorType=Enums.CaculatorType.EightHours,
                                        Unit=@"mg/m3",
                                        MinConcentration=0.1M,
                                        MaxConcentration=0.16M,
                                        MinPollutionIndex=51,
                                        MaxPollutionIndex=100,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="O3",
                                        FactorCNName="臭氧",
                                        CaculatorType=Enums.CaculatorType.EightHours,
                                        Unit=@"mg/m3",
                                        MinConcentration=0.16M,
                                        MaxConcentration=0.215M,
                                        MinPollutionIndex=101,
                                        MaxPollutionIndex=150,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="O3",
                                        FactorCNName="臭氧",
                                        CaculatorType=Enums.CaculatorType.EightHours,
                                        Unit=@"mg/m3",
                                        MinConcentration=0.215M,
                                        MaxConcentration=0.265M,
                                        MinPollutionIndex=151,
                                        MaxPollutionIndex=200,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="O3",
                                        FactorCNName="臭氧",
                                        CaculatorType=Enums.CaculatorType.EightHours,
                                        Unit=@"mg/m3",
                                        MinConcentration=0.265M,
                                        MaxConcentration=0.8M,
                                        MinPollutionIndex=201,
                                        MaxPollutionIndex=300,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="O3",
                                        FactorCNName="臭氧",
                                        CaculatorType=Enums.CaculatorType.EightHours,
                                        Unit=@"mg/m3",
                                        MinPollutionIndex=301,
                                        MaxPollutionIndex=400,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="O3",
                                        FactorCNName="臭氧",
                                        CaculatorType=Enums.CaculatorType.EightHours,
                                        Unit=@"mg/m3",
                                        MinPollutionIndex=401,
                                        MaxPollutionIndex=500,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    #endregion
                                    #region  PM2.5 24小时
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="PM2.5",
                                        FactorCNName="PM2.5",
                                        CaculatorType=Enums.CaculatorType.TwentyHours,
                                        Unit=@"mg/m3",
                                        MinConcentration=0M,
                                        MaxConcentration=0.035M,
                                        MinPollutionIndex=0,
                                        MaxPollutionIndex=50,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="PM2.5",
                                        FactorCNName="PM2.5",
                                        CaculatorType=Enums.CaculatorType.TwentyHours,
                                        Unit=@"mg/m3",
                                        MinConcentration=0.035M,
                                        MaxConcentration=0.075M,
                                        MinPollutionIndex=51,
                                        MaxPollutionIndex=100,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="PM2.5",
                                        FactorCNName="PM2.5",
                                        CaculatorType=Enums.CaculatorType.TwentyHours,
                                        Unit=@"mg/m3",
                                        MinConcentration=0.075M,
                                        MaxConcentration=0.115M,
                                        MinPollutionIndex=101,
                                        MaxPollutionIndex=150,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="PM2.5",
                                        FactorCNName="PM2.5",
                                        CaculatorType=Enums.CaculatorType.TwentyHours,
                                        Unit=@"mg/m3",
                                        MinConcentration=0.115M,
                                        MaxConcentration=0.15M,
                                        MinPollutionIndex=151,
                                        MaxPollutionIndex=200,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="PM2.5",
                                        FactorCNName="PM2.5",
                                        CaculatorType=Enums.CaculatorType.TwentyHours,
                                        Unit=@"mg/m3",
                                        MinConcentration=0.15M,
                                        MaxConcentration=0.25M,
                                        MinPollutionIndex=201,
                                        MaxPollutionIndex=300,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="PM2.5",
                                        FactorCNName="PM2.5",
                                        CaculatorType=Enums.CaculatorType.TwentyHours,
                                        Unit=@"mg/m3",
                                        MinConcentration=0.25M,
                                        MaxConcentration=0.35M,
                                        MinPollutionIndex=301,
                                        MaxPollutionIndex=400,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="PM2.5",
                                        FactorCNName="PM2.5",
                                        CaculatorType=Enums.CaculatorType.TwentyHours,
                                        Unit=@"mg/m3",
                                        MinConcentration=0.35M,
                                        MaxConcentration=0.5M,
                                        MinPollutionIndex=401,
                                        MaxPollutionIndex=500,
                                        PollutionIndexType=Enums.PollutionIndexType.AQI
                                    }
                                    #endregion
                #endregion
                                 ,
                #region API
                                    #region SO2
                                         new PollutionIndexStandard()
                                        {
                                            FactorName="SO2",
                                            FactorCNName="二氧化硫",
                                            CaculatorType=Enums.CaculatorType.OneHour,
                                            Unit=@"mg/m3",
                                            MinConcentration=0M,
                                            MaxConcentration=0.05M,
                                            MinPollutionIndex=0,
                                            MaxPollutionIndex=50,
                                            PollutionIndexType=Enums.PollutionIndexType.API
                                        },
                                         new PollutionIndexStandard()
                                        {
                                            FactorName="SO2",
                                            FactorCNName="二氧化硫",
                                            CaculatorType=Enums.CaculatorType.OneHour,
                                            Unit=@"mg/m3",
                                            MinConcentration=0.05M,
                                            MaxConcentration=0.15M,
                                            MinPollutionIndex=51,
                                            MaxPollutionIndex=100,
                                            PollutionIndexType=Enums.PollutionIndexType.API
                                        },
                                        new PollutionIndexStandard()
                                        {
                                            FactorName="SO2",
                                            FactorCNName="二氧化硫",
                                            CaculatorType=Enums.CaculatorType.OneHour,
                                            Unit=@"mg/m3",
                                            MinConcentration=0.15M,
                                            MaxConcentration=0.8M,
                                            MinPollutionIndex=101,
                                            MaxPollutionIndex=200,
                                            PollutionIndexType=Enums.PollutionIndexType.API
                                        },
                                        new PollutionIndexStandard()
                                        {
                                            FactorName="SO2",
                                            FactorCNName="二氧化硫",
                                            CaculatorType=Enums.CaculatorType.OneHour,
                                            Unit=@"mg/m3",
                                            MinConcentration=0.8M,
                                            MaxConcentration=1.6M,
                                            MinPollutionIndex=201,
                                            MaxPollutionIndex=300,
                                            PollutionIndexType=Enums.PollutionIndexType.API
                                        },
                                        new PollutionIndexStandard()
                                        {
                                            FactorName="SO2",
                                            FactorCNName="二氧化硫",
                                            CaculatorType=Enums.CaculatorType.OneHour,
                                            Unit=@"mg/m3",
                                            MinConcentration=1.6M,
                                            MaxConcentration=2.1M,
                                            MinPollutionIndex=301,
                                            MaxPollutionIndex=400,
                                            PollutionIndexType=Enums.PollutionIndexType.API
                                        },
                                        new PollutionIndexStandard()
                                        {
                                            FactorName="SO2",
                                            FactorCNName="二氧化硫",
                                            CaculatorType=Enums.CaculatorType.OneHour,
                                            Unit=@"mg/m3",
                                            MinConcentration=2.1M,
                                            MaxConcentration=2.62M,
                                            MinPollutionIndex=401,
                                            MaxPollutionIndex=500,
                                            PollutionIndexType=Enums.PollutionIndexType.API
                                        },
                                    #endregion
                                    #region NO2
                                        new PollutionIndexStandard()
                                        {
                                            FactorName="NO2",
                                            FactorCNName="二氧化氮",
                                            CaculatorType=Enums.CaculatorType.OneHour,
                                            Unit=@"mg/m3",
                                            MinConcentration=0M,
                                            MaxConcentration=0.08M,
                                            MinPollutionIndex=0,
                                            MaxPollutionIndex=50,
                                            PollutionIndexType=Enums.PollutionIndexType.API
                                        },
                                        new PollutionIndexStandard()
                                        {
                                            FactorName="NO2",
                                            FactorCNName="二氧化氮",
                                            CaculatorType=Enums.CaculatorType.OneHour,
                                            Unit=@"mg/m3",
                                            MinConcentration=0.08M,
                                            MaxConcentration=0.12M,
                                            MinPollutionIndex=51,
                                            MaxPollutionIndex=100,
                                            PollutionIndexType=Enums.PollutionIndexType.API
                                        },
                                        new PollutionIndexStandard()
                                        {
                                            FactorName="NO2",
                                            FactorCNName="二氧化氮",
                                            CaculatorType=Enums.CaculatorType.OneHour,
                                            Unit=@"mg/m3",
                                            MinConcentration=0.12M,
                                            MaxConcentration=0.28M,
                                            MinPollutionIndex=101,
                                            MaxPollutionIndex=200,
                                            PollutionIndexType=Enums.PollutionIndexType.API
                                        },
                                        new PollutionIndexStandard()
                                        {
                                            FactorName="NO2",
                                            FactorCNName="二氧化氮",
                                            CaculatorType=Enums.CaculatorType.OneHour,
                                            Unit=@"mg/m3",
                                            MinConcentration=0.28M,
                                            MaxConcentration=0.565M,
                                            MinPollutionIndex=201,
                                            MaxPollutionIndex=300,
                                            PollutionIndexType=Enums.PollutionIndexType.API
                                        },
                                        new PollutionIndexStandard()
                                        {
                                            FactorName="NO2",
                                            FactorCNName="二氧化氮",
                                            CaculatorType=Enums.CaculatorType.OneHour,
                                            Unit=@"mg/m3",
                                            MinConcentration=0.565M,
                                            MaxConcentration=0.75M,
                                            MinPollutionIndex=301,
                                            MaxPollutionIndex=400,
                                            PollutionIndexType=Enums.PollutionIndexType.API
                                        },
                                         new PollutionIndexStandard()
                                        {
                                            FactorName="NO2",
                                            FactorCNName="二氧化氮",
                                            CaculatorType=Enums.CaculatorType.OneHour,
                                            Unit=@"mg/m3",
                                            MinConcentration=0.75M,
                                            MaxConcentration=0.94M,
                                            MinPollutionIndex=401,
                                            MaxPollutionIndex=500,
                                            PollutionIndexType=Enums.PollutionIndexType.API
                                        },
                                        #endregion
                                    #region PM10
                                     new PollutionIndexStandard()
                                    {
                                        FactorName="PM10",
                                        FactorCNName="PM10",
                                        CaculatorType=Enums.CaculatorType.OneHour,
                                        Unit=@"mg/m3",
                                        MinConcentration=0M,
                                        MaxConcentration=0.05M,
                                        MinPollutionIndex=0,
                                        MaxPollutionIndex=50,
                                        PollutionIndexType=Enums.PollutionIndexType.API
                                    },
                                     new PollutionIndexStandard()
                                    {
                                        FactorName="PM10",
                                        FactorCNName="PM10",
                                        CaculatorType=Enums.CaculatorType.OneHour,
                                        Unit=@"mg/m3",
                                        MinConcentration=0.05M,
                                        MaxConcentration=0.15M,
                                        MinPollutionIndex=51,
                                        MaxPollutionIndex=100,
                                        PollutionIndexType=Enums.PollutionIndexType.API
                                    },
                                     new PollutionIndexStandard()
                                    {
                                        FactorName="PM10",
                                        FactorCNName="PM10",
                                        CaculatorType=Enums.CaculatorType.OneHour,
                                        Unit=@"mg/m3",
                                        MinConcentration=0.15M,
                                        MaxConcentration=0.35M,
                                        MinPollutionIndex=101,
                                        MaxPollutionIndex=200,
                                        PollutionIndexType=Enums.PollutionIndexType.API
                                    },
                                     new PollutionIndexStandard()
                                    {
                                        FactorName="PM10",
                                        FactorCNName="PM10",
                                        CaculatorType=Enums.CaculatorType.OneHour,
                                        Unit=@"mg/m3",
                                        MinConcentration=0.35M,
                                        MaxConcentration=0.42M,
                                        MinPollutionIndex=201,
                                        MaxPollutionIndex=300,
                                        PollutionIndexType=Enums.PollutionIndexType.API
                                    },
                                     new PollutionIndexStandard()
                                    {
                                        FactorName="PM10",
                                        FactorCNName="PM10",
                                        CaculatorType=Enums.CaculatorType.OneHour,
                                        Unit=@"mg/m3",
                                        MinConcentration=0.42M,
                                        MaxConcentration=0.5M,
                                        MinPollutionIndex=301,
                                        MaxPollutionIndex=400,
                                        PollutionIndexType=Enums.PollutionIndexType.API
                                    },
                                     new PollutionIndexStandard()
                                    {
                                        FactorName="PM10",
                                        FactorCNName="PM10",
                                        CaculatorType=Enums.CaculatorType.OneHour,
                                        Unit=@"mg/m3",
                                        MinConcentration=0.5M,
                                        MaxConcentration=0.6M,
                                        MinPollutionIndex=401,
                                        MaxPollutionIndex=500,
                                        PollutionIndexType=Enums.PollutionIndexType.API
                                    },
                                    #endregion
                                    #region CO
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="CO",
                                        FactorCNName="一氧化碳",
                                        CaculatorType=Enums.CaculatorType.OneHour,
                                        Unit=@"mg/m3",
                                        MinConcentration=0M,
                                        MaxConcentration=5M,
                                        MinPollutionIndex=0,
                                        MaxPollutionIndex=50,
                                        PollutionIndexType=Enums.PollutionIndexType.API
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="CO",
                                        FactorCNName="一氧化碳",
                                        CaculatorType=Enums.CaculatorType.OneHour,
                                        Unit=@"mg/m3",
                                        MinConcentration=5M,
                                        MaxConcentration=10M,
                                        MinPollutionIndex=51,
                                        MaxPollutionIndex=100,
                                        PollutionIndexType=Enums.PollutionIndexType.API
                                    } ,
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="CO",
                                        FactorCNName="一氧化碳",
                                        CaculatorType=Enums.CaculatorType.OneHour,
                                        Unit=@"mg/m3",
                                        MinConcentration=10M,
                                        MaxConcentration=60M,
                                        MinPollutionIndex=101,
                                        MaxPollutionIndex=200,
                                        PollutionIndexType=Enums.PollutionIndexType.API
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="CO",
                                        FactorCNName="一氧化碳",
                                        CaculatorType=Enums.CaculatorType.OneHour,
                                        Unit=@"mg/m3",
                                        MinConcentration=60M,
                                        MaxConcentration=90M,
                                        MinPollutionIndex=201,
                                        MaxPollutionIndex=300,
                                        PollutionIndexType=Enums.PollutionIndexType.API
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="CO",
                                        FactorCNName="一氧化碳",
                                        CaculatorType=Enums.CaculatorType.OneHour,
                                        Unit=@"mg/m3",
                                        MinConcentration=90M,
                                        MaxConcentration=120M,
                                        MinPollutionIndex=301,
                                        MaxPollutionIndex=400,
                                        PollutionIndexType=Enums.PollutionIndexType.API
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="CO",
                                        FactorCNName="一氧化碳",
                                        CaculatorType=Enums.CaculatorType.OneHour,
                                        Unit=@"mg/m3",
                                        MinConcentration=120M,
                                        MaxConcentration=150M,
                                        MinPollutionIndex=401,
                                        MaxPollutionIndex=500,
                                        PollutionIndexType=Enums.PollutionIndexType.API
                                    },
                                    #endregion
                                    #region O3
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="O3",
                                        FactorCNName="臭氧",
                                        CaculatorType=Enums.CaculatorType.OneHour,
                                        Unit=@"mg/m3",
                                        MinConcentration=0M,
                                        MaxConcentration=0.12M,
                                        MinPollutionIndex=0,
                                        MaxPollutionIndex=50,
                                        PollutionIndexType=Enums.PollutionIndexType.API
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="O3",
                                        FactorCNName="臭氧",
                                        CaculatorType=Enums.CaculatorType.OneHour,
                                        Unit=@"mg/m3",
                                        MinConcentration=0.12M,
                                        MaxConcentration=0.2M,
                                        MinPollutionIndex=51,
                                        MaxPollutionIndex=100,
                                        PollutionIndexType=Enums.PollutionIndexType.API
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="O3",
                                        FactorCNName="臭氧",
                                        CaculatorType=Enums.CaculatorType.OneHour,
                                        Unit=@"mg/m3",
                                        MinConcentration=0.2M,
                                        MaxConcentration=0.4M,
                                        MinPollutionIndex=101,
                                        MaxPollutionIndex=200,
                                        PollutionIndexType=Enums.PollutionIndexType.API
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="O3",
                                        FactorCNName="臭氧",
                                        CaculatorType=Enums.CaculatorType.OneHour,
                                        Unit=@"mg/m3",
                                        MinConcentration=0.4M,
                                        MaxConcentration=0.8M,
                                        MinPollutionIndex=201,
                                        MaxPollutionIndex=300,
                                        PollutionIndexType=Enums.PollutionIndexType.API
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="O3",
                                        FactorCNName="臭氧",
                                        CaculatorType=Enums.CaculatorType.OneHour,
                                        Unit=@"mg/m3",
                                        MinConcentration=0.8M,
                                        MaxConcentration=1M,
                                        MinPollutionIndex=301,
                                        MaxPollutionIndex=400,
                                        PollutionIndexType=Enums.PollutionIndexType.API
                                    },
                                    new PollutionIndexStandard()
                                    {
                                        FactorName="O3",
                                        FactorCNName="臭氧",
                                        CaculatorType=Enums.CaculatorType.OneHour,
                                        Unit=@"mg/m3",
                                        MinConcentration=1M,
                                        MaxConcentration=1.2M,
                                        MinPollutionIndex=401,
                                        MaxPollutionIndex=500,
                                        PollutionIndexType=Enums.PollutionIndexType.API
                                    }
                                    #endregion
                #endregion
                };
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 获得因子的污染信息
        /// </summary>
        /// <param name="factorName">因子名称</param>
        /// <param name="caculatorType">因子的时间类型</param>
        /// <param name="factorConcentration">因子的浓度</param>
        /// <returns>没有值返回NULL</returns>
        public static PollutionLevelInfo GetPollutionLevelInfo(string factorName, Enums.CaculatorType caculatorType, decimal? factorConcentration, Enums.PollutionIndexType pollutionIndexType)
        {
            PollutionLevelInfo newPollutionLevelInfo = null;
            List<PollutionIndexStandard> standards = Standards.Where(x => x.PollutionIndexType == pollutionIndexType && x.FactorName == factorName).OrderBy(x => x.MinPollutionIndex).ToList();
            if (!standards.Exists(x => x.CaculatorType == caculatorType))//不存在此标准
            {
                newPollutionLevelInfo = GetEmptyPollutionLevelInfo(factorName, factorConcentration, caculatorType, pollutionIndexType, false);
                newPollutionLevelInfo.Id = newPollutionLevelInfo.GetHashCode().ToString();
                return newPollutionLevelInfo;
            }
            standards = standards.Where(x => x.CaculatorType == caculatorType).ToList();

            //使用规则
            decimal? tempFactorConcentration = GetConcentrationByRules(factorName, caculatorType, factorConcentration);

            if (tempFactorConcentration == null)//污染物浓度为空
            {
                newPollutionLevelInfo = GetEmptyPollutionLevelInfo(factorName, factorConcentration, caculatorType, pollutionIndexType);
                newPollutionLevelInfo.Id = newPollutionLevelInfo.GetHashCode().ToString();
                return newPollutionLevelInfo;
            }

            int? pollutionIndex;//得到IAQI
            if (factorConcentration <= 0M)
                pollutionIndex = 0;
            else
            {
                decimal maxValue = standards.Max(x => x.MaxConcentration);

                if (factorConcentration > maxValue)
                {
                    pollutionIndex = standards.Where(x => x.MaxConcentration == maxValue).Select(x => x.MaxPollutionIndex).FirstOrDefault();
                }
                else
                {
                    PollutionIndexStandard pistd = standards.Where(x => factorConcentration > x.MinConcentration && factorConcentration <= x.MaxConcentration).FirstOrDefault();
                    pollutionIndex = Caculator(factorConcentration, pistd);
                }
            }
            if (pollutionIndex == null) //IAQI为NULL
            {
                newPollutionLevelInfo = GetEmptyPollutionLevelInfo(factorName, factorConcentration, caculatorType, pollutionIndexType);
                newPollutionLevelInfo.Id = newPollutionLevelInfo.GetHashCode().ToString();
                return newPollutionLevelInfo;
            }
            //得到IAQI
            PollutionStandard ps = GetPollutionStandard((int)pollutionIndex, pollutionIndexType);
            newPollutionLevelInfo = new PollutionLevelInfo() { Id = ps.GetHashCode().ToString(), FactorName = factorName, Concentration = factorConcentration, PollutionIndex = pollutionIndex, Min = ps.Min, Max = ps.Max, CaculatorType = caculatorType, Color = ps.Color, Level = ps.Level, HealthEffect = ps.HealthEffect, Range = ps.Range, Suggestion = ps.Suggestion, LevelType = ps.LevelType, Roman = ps.Roman, PollutionIndexType = pollutionIndexType, ColorCode = ps.ColorCode,IsPollutionIndexFactor=true };
            return newPollutionLevelInfo;
        }

        /// <summary>
        /// 污染指数计算公式
        /// </summary>
        /// <param name="factorConcentration">因子浓度</param>
        /// <param name="pistd">因子信息</param>
        /// <returns></returns>
        private static int? Caculator(decimal? factorConcentration, PollutionIndexStandard pistd)
        {
            if (factorConcentration == null) return null;
            decimal tempConcentration = (decimal)factorConcentration < 0M ? 0M : (decimal)factorConcentration;
            int minPollutionIndex = pistd.MinPollutionIndex == 0 ? 0 : pistd.MinPollutionIndex - 1;
            decimal value = ((pistd.MaxPollutionIndex - minPollutionIndex) / (pistd.MaxConcentration - pistd.MinConcentration)) * ((decimal)factorConcentration - pistd.MinConcentration) + minPollutionIndex;
            return System.Convert.ToInt32(Math.Ceiling(Math.Round(value, 3)));
        }

        /// <summary>
        /// 使用规则
        /// </summary>
        /// <returns></returns>
        private static decimal? GetConcentrationByRules(string factorName, Enums.CaculatorType caculatorType, decimal? factorConcentration)
        {
            if (factorConcentration == null) return null;
            if (factorConcentration <= 0M)
            {
                return 0M;
            }
            return factorConcentration;
        }

        /// <summary>
        /// 获取没有标准的污染物等级
        /// </summary>
        /// <param name="factorName"></param>
        /// <param name="factorConcentration"></param>
        /// <param name="caculatorType"></param>
        /// <param name="pollutionIndexType"></param>
        /// <param name="isPollutionIndexFactor">是不是参与计算的因子</param>
        /// <returns></returns>
        private static PollutionLevelInfo GetEmptyPollutionLevelInfo(string factorName, decimal? factorConcentration, Enums.CaculatorType caculatorType, Enums.PollutionIndexType pollutionIndexType, bool isPollutionIndexFactor = true)
        {
            return new PollutionLevelInfo() { FactorName = factorName, Concentration = factorConcentration, CaculatorType = caculatorType, Color = "--", Level = "--", HealthEffect = "--", Range = "--", Suggestion = "--", LevelType = "--", Roman = "--", PollutionIndexType = pollutionIndexType, ColorCode = "--", IsPollutionIndexFactor = isPollutionIndexFactor };
        }

        /// <summary>
        /// 根据AQI获得污染情况
        /// </summary>
        /// <param name="pollutionIndex">AQI的污染指数</param>
        /// <returns></returns>
        private static PollutionStandard GetPollutionStandard(int pollutionIndex, Enums.PollutionIndexType pollutionIndexType)
        {
            System.Collections.Generic.List<PollutionStandard> standards = PollutionLevelStandards.Where(x => x.PollutionIndex_Type == pollutionIndexType).OrderBy(x => x.PollutionIndex_Type).ToList();
            int maxPollutionIndex = standards.Max(x => x.Min).GetValueOrDefault();
            if (pollutionIndex >= maxPollutionIndex)
                return standards.Where(x => maxPollutionIndex == x.Min).FirstOrDefault();
            return standards.Where(x => pollutionIndex >= x.Min && pollutionIndex <= x.Max).FirstOrDefault();
        }

        #endregion Methods
    }
}