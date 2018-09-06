namespace SmartEP.Core.Enums
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    /// <summary>
    /// Copyright (c) 2013 , 江苏远大信息股份有限公司产品事业部
    /// EnumMapping.cs
    /// 创建人：JiKe
    /// 创建日期：2015-08-11
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 枚举映射类
    /// </summary>
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
        /// 获取应用程序种类选择的值
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static string GetApplicationValue(ApplicationValue mode)
        {
            string strApplicationValue = "";
            switch (mode)
            {
                case ApplicationValue.Air: strApplicationValue = "airaaira-aira-aira-aira-airaairaaira"; break;
                case ApplicationValue.Water: strApplicationValue = "watrwatr-watr-watr-watr-watrwatrwatr"; break;
                case ApplicationValue.Noise: strApplicationValue = "noisnois-nois-nois-nois-noisnoisnois"; break;
                case ApplicationValue.VOCs: strApplicationValue = "vocsvocs-vocs-vocs-vocs-vocsvocsvocs"; break;
                case ApplicationValue.CEMS: strApplicationValue = "cemscems-cems-cems-cems-cemscemscems"; break;
                case ApplicationValue.WasteWater: strApplicationValue = "wastwast-wast-wast-wast-wastwastwast"; break;
            }
            return strApplicationValue;
        }

        /// <summary>
        /// 根据应用程序种类获取应用程序种类选择的值
        /// </summary>
        /// <param name="type">应用程序类型字典</param>
        /// <returns></returns>
        public static string GetApplicationValue(ApplicationType type)
        {
            string strApplicationValue = "";
            switch (type)
            {
                case ApplicationType.Air: strApplicationValue = "airaaira-aira-aira-aira-airaairaaira"; break;
                case ApplicationType.Water: strApplicationValue = "watrwatr-watr-watr-watr-watrwatrwatr"; break;
                case ApplicationType.Noise: strApplicationValue = "noisnois-nois-nois-nois-noisnoisnois"; break;
            }
            return strApplicationValue;
        }

        /// 获取污染物类型选择的值
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static string GetPollutantTypeValue(PollutantTypeValue mode)
        {
            string strPollutantTypeValue = "";
            switch (mode)
            {
                case PollutantTypeValue.Air: strPollutantTypeValue = "8d89b62d-36e1-4f05-b00d-3a585f6a90d7"; break;
                case PollutantTypeValue.Water: strPollutantTypeValue = "80ca99de-3b78-422f-9baa-d47f23324231"; break;
                case PollutantTypeValue.Noise: strPollutantTypeValue = "a24e5381-c4c8-4f1d-b3e9-56bc4983dbda"; break;

            }
            return strPollutantTypeValue;
        }

        /// <summary>
        /// 取得AQI分指数数据库字段
        /// </summary>
        /// <param name="iAQIType"></param>
        /// <returns></returns>
        public static string GetIAQITypeColumn(IAQIType iAQIType)
        {
            string iAQITypeColumn = "AQIValue";
            switch (iAQIType)
            {
                case IAQIType.SO2_IAQI: iAQITypeColumn = "SO2_IAQI"; break;
                case IAQIType.NO2_IAQI: iAQITypeColumn = "NO2_IAQI"; break;
                case IAQIType.CO_IAQI: iAQITypeColumn = "CO_IAQI"; break;
                case IAQIType.PM10_IAQI: iAQITypeColumn = "PM10_IAQI"; break;
                case IAQIType.PM25_IAQI: iAQITypeColumn = "PM25_IAQI"; break;
                case IAQIType.Max8HourO3_IAQI: iAQITypeColumn = "Max8HourO3_IAQI"; break;
                case IAQIType.MaxOneHourO3_IAQI: iAQITypeColumn = "MaxOneHourO3_IAQI"; break;
                case IAQIType.Recent24HoursPM10_IAQI: iAQITypeColumn = "Recent24HoursPM10_IAQI"; break;
                case IAQIType.Recent24HoursPM25_IAQI: iAQITypeColumn = "Recent24HoursPM25_IAQI"; break;
                case IAQIType.AQIValue: iAQITypeColumn = "AQIValue"; break;
            }
            return iAQITypeColumn;
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

        /// <summary>
        /// 获取枚举类子项描述信息
        /// </summary>
        /// <param name="enumSubitem">枚举类子项</param>        
        public static string GetDesc(object enumSubitem)
        {
            enumSubitem = (Enum)enumSubitem;
            string strValue = enumSubitem.ToString();
            FieldInfo fieldinfo = enumSubitem.GetType().GetField(strValue);
            if (fieldinfo != null)
            {
                Object[] objs = fieldinfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (objs == null || objs.Length == 0)
                {
                    return strValue;
                }
                else
                {
                    DescriptionAttribute da = (DescriptionAttribute)objs[0];
                    return da.Description;
                }
            }
            else
            {
                return "";
            }
        }
        #endregion Methods
    }
}