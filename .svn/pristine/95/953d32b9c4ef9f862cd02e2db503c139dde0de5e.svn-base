namespace SmartEP.Service.Core.Enums
{
    using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

    public class EnumMapping
    {
        #region Methods

        /// <summary>
        /// 缓存种类名称选择
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static string GetCachedClassName(CachedMode mode)
        {
            string strClassName = "";
            switch (mode)
            {
                case CachedMode.DefaultCachedStrategy: strClassName = "DefaultCachedStrategy"; break;
                case CachedMode.MemCachedStrategy: strClassName = "MemCachedStrategy"; break;

                case CachedMode.XMLStrategy: strClassName = "XMLStrategy"; break;
                case CachedMode.DBStrategy: strClassName = "DBStrategy"; break;

                case CachedMode.RedisStrategy: strClassName = "RedisStrategy"; break;
                case CachedMode.SessionStrategy: strClassName = "SessionStrategy"; break;
                case CachedMode.CookieStrategy: strClassName = "CookieStrategy"; break;
            }
            return strClassName;
        }

        /// <summary>
        /// ORM提供者种类选择
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static string GetOrmProviderClassName(OrmProviderMode mode)
        {
            string strClassName = "";
            switch (mode)
            {
                case OrmProviderMode.OpenAccessORM: strClassName = "OpenAccesssRepositoryStrategy"; break;
                case OrmProviderMode.NHibernate: strClassName = "NHibernateRepositoryStrategy"; break;

                case OrmProviderMode.ADONet: strClassName = "AdoRepositoryStrategy"; break;
                case OrmProviderMode.EnterpriseLibrary: strClassName = "EnterpriseLibraryRepositoryStrategy"; break;
            }
            return strClassName;
        }

        /// <summary>
        /// 获取字典类型名称
        /// </summary>
        /// <param name="type">字典类型</param>
        /// <returns></returns>
        public static string GetDictionaryName(DictionaryType type)
        {
            string strClassName = "";
            switch (type)
            {
                case DictionaryType.AMS: strClassName = "自动监控"; break;
                case DictionaryType.Frame: strClassName = "框架"; break;
                case DictionaryType.Air: strClassName = "环境空气"; break;
                case DictionaryType.Water: strClassName = "地表水"; break;
                case DictionaryType.Noise: strClassName = "噪声"; break;
            }
            return strClassName;
        }

        /// <summary>
        /// 获取水质类别
        /// </summary>
        /// <param name="type">AQI类别</param>
        /// <returns></returns>
        public static string GetWaterQualityClass(WaterQualityClass wqClass)
        {
            string strClassName = "";
            switch (wqClass)
            {
                case WaterQualityClass.One: strClassName = "I类"; break;
                case WaterQualityClass.Two: strClassName = "II类"; break;
                case WaterQualityClass.Three: strClassName = "III类"; break;
                case WaterQualityClass.Four: strClassName = "IV类"; break;
                case WaterQualityClass.Five: strClassName = "V类"; break;
                case WaterQualityClass.BadFive: strClassName = "劣V类"; break;
            }
            return strClassName;
        }

        /// <summary>
        /// 获取水质类别罗马字符
        /// </summary>
        /// <param name="type">AQI类别</param>
        /// <returns></returns>
        public static string GetWaterQualityRome(WaterQualityClass wqClass)
        {
            string strClassName = "";
            switch (wqClass)
            {
                case WaterQualityClass.One: strClassName = "Ⅰ"; break;
                case WaterQualityClass.Two: strClassName = "Ⅱ"; break;
                case WaterQualityClass.Three: strClassName = "Ⅲ"; break;
                case WaterQualityClass.Four: strClassName = "Ⅳ"; break;
                case WaterQualityClass.Five: strClassName = "Ⅴ"; break;
                case WaterQualityClass.BadFive: strClassName = "劣Ⅴ"; break;
            }
            return strClassName;
        }

        /// <summary>
        /// 获取AQI类别
        /// </summary>
        /// <param name="type">AQI类别</param>
        /// <returns></returns>
        public static string GetAQIClass(AQIClass aqiClass)
        {
            string strClassName = "";
            switch (aqiClass)
            {
                case AQIClass.Good: strClassName = "优"; break;
                case AQIClass.Moderate: strClassName = "良"; break;
                case AQIClass.LightlyPolluted: strClassName = "轻度污染"; break;
                case AQIClass.ModeratelyPolluted: strClassName = "中度污染"; break;
                case AQIClass.HeavilyPolluted: strClassName = "重度污染"; break;
                case AQIClass.SeverelyPolluted: strClassName = "严重污染"; break;
            }
            return strClassName;
        }

        /// <summary>
        /// 获取AQI级别
        /// </summary>
        /// <param name="type">AQI类别</param>
        /// <returns></returns>
        public static string GetAQIGrade(AQIClass aqiClass)
        {
            string strClassName = "";
            switch (aqiClass)
            {
                case AQIClass.Good: strClassName = "一级"; break;
                case AQIClass.Moderate: strClassName = "二级"; break;
                case AQIClass.LightlyPolluted: strClassName = "三级"; break;
                case AQIClass.ModeratelyPolluted: strClassName = "四级"; break;
                case AQIClass.HeavilyPolluted: strClassName = "五级"; break;
                case AQIClass.SeverelyPolluted: strClassName = "六级"; break;
            }
            return strClassName;
        }

        /// <summary>
        /// 获取AQI等级颜色中文
        /// </summary>
        /// <param name="type">AQI类别</param>
        /// <returns></returns>
        public static string GetAQIColorName(AQIClass aqiClass)
        {
            string strClassName = "";
            switch (aqiClass)
            {
                case AQIClass.Good: strClassName = "绿色"; break;
                case AQIClass.Moderate: strClassName = "黄色"; break;
                case AQIClass.LightlyPolluted: strClassName = "橙色"; break;
                case AQIClass.ModeratelyPolluted: strClassName = "红色"; break;
                case AQIClass.HeavilyPolluted: strClassName = "紫色"; break;
                case AQIClass.SeverelyPolluted: strClassName = "褐红色"; break;
            }
            return strClassName;
        }

        /// <summary>
        /// 获取AQI等级颜色RGB代码
        /// </summary>
        /// <param name="type">AQI类别</param>
        /// <returns></returns>
        public static string GetAQIRGBValue(AQIClass aqiClass)
        {
            string strClassName = "";
            switch (aqiClass)
            {
                case AQIClass.Good: strClassName = "#00e400"; break;
                case AQIClass.Moderate: strClassName = "#ffff00"; break;
                case AQIClass.LightlyPolluted: strClassName = "#ff7e00"; break;
                case AQIClass.ModeratelyPolluted: strClassName = "#ff0000"; break;
                case AQIClass.HeavilyPolluted: strClassName = "#99004c"; break;
                case AQIClass.SeverelyPolluted: strClassName = "#7e0023"; break;
            }
            return strClassName;
        }

        /// <summary>
        /// 获取AQI范围
        /// </summary>
        /// <param name="type">AQI类别</param>
        /// <returns></returns>
        public static string GetAQIRange(AQIClass aqiClass)
        {
            string strClassName = "";
            switch (aqiClass)
            {
                case AQIClass.Good: strClassName = "0~50"; break;
                case AQIClass.Moderate: strClassName = "51~100"; break;
                case AQIClass.LightlyPolluted: strClassName = "101~150"; break;
                case AQIClass.ModeratelyPolluted: strClassName = "151~200"; break;
                case AQIClass.HeavilyPolluted: strClassName = "201~300"; break;
                case AQIClass.SeverelyPolluted: strClassName = ">300"; break;
            }
            return strClassName;
        }

        /// <summary>
        /// 获取AQI对健康影响
        /// </summary>
        /// <param name="type">AQI类别</param>
        /// <returns></returns>
        public static string GetAQIHealthEffect(AQIClass aqiClass)
        {
            string strClassName = "";
            switch (aqiClass)
            {
                case AQIClass.Good: strClassName = "空气质量令人满意,基本无空气污染"; break;
                case AQIClass.Moderate: strClassName = "空气质量可接受,但某些污染物可能对极少数异常敏感人群健康有较弱影响"; break;
                case AQIClass.LightlyPolluted: strClassName = "易感人群症状有轻度加剧,健康人群出现刺激症状"; break;
                case AQIClass.ModeratelyPolluted: strClassName = "进一步加剧易感人群症状,可能对健康人群心脏、呼吸系统有影响"; break;
                case AQIClass.HeavilyPolluted: strClassName = "心脏病和肺病患者症状显著加剧,运动耐受力降低,健康人群普遍出现症状"; break;
                case AQIClass.SeverelyPolluted: strClassName = "健康人运动耐受力降低,有明显强烈症状,提前出现某些疾病"; break;
            }
            return strClassName;
        }

        /// <summary>
        /// 获取AQI建议采取措施
        /// </summary>
        /// <param name="type">AQI类别</param>
        /// <returns></returns>
        public static string GetAQITakeStep(AQIClass aqiClass)
        {
            string strClassName = "";
            switch (aqiClass)
            {
                case AQIClass.Good: strClassName = "各类人群可正常活动"; break;
                case AQIClass.Moderate: strClassName = "极少数异常敏感人群应减少户外活动"; break;
                case AQIClass.LightlyPolluted: strClassName = "儿童、老年人及心脏病、呼吸系统疾病患者应减少长时间、高强度的户外锻炼"; break;
                case AQIClass.ModeratelyPolluted: strClassName = "儿童、老年人及心脏病、呼吸系统疾病患者避免长时间、高强度的户外锻练,一般人群适量减少户外运动"; break;
                case AQIClass.HeavilyPolluted: strClassName = "儿童、老人和心脏病、肺病患者应停留在室内,停止户外运动,一般人群减少户外运动"; break;
                case AQIClass.SeverelyPolluted: strClassName = "儿童、老年人和病人应当留在室内,避免体力消耗,一般人群应避免户外活动"; break;
            }
            return strClassName;
        }

        /// <summary>
        /// To Parse a string to relevent enum value.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="item"></param>
        /// <param name="ignorecase"></param>
        /// <returns>Matched Enum value or other wise default enum value.</returns>
        /// <example>
        ///     <code>
        ///         enum MyEnum
        ///         {
        ///             One=0,
        ///             Two
        ///         }
        ///         string stringToEnum ="One";
        ///         var parsedEnum = ParseEnum<MyEnum>(stringToEnum);
        ///     </code>
        /// </example>
        /// Contributed by Mohammad Rahman
        public static TEnum ParseEnum<TEnum>(string item, bool ignorecase = default(bool))
            where TEnum : struct
        {
            TEnum tenumResult = default(TEnum);
            return Enum.TryParse<TEnum>(item, ignorecase, out tenumResult) ?
                tenumResult : default(TEnum); 
        }

    

        #endregion Methods
    }
}