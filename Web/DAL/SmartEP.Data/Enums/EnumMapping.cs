using SmartEP.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.Data.Enums
{
    /// <summary>
    /// 名称：EnumMapping.cs
    /// 创建人：李飞
    /// 创建日期：2015-08-18
    /// 维护人员：
    /// 最新维护人员：刘晋
    /// 最新维护日期：2017-06-01
    /// 功能摘要：
    /// 环境空气发布：枚举映射处理
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class EnumMapping
    {
        /// <summary>
        /// 取得自动监测数据类型对应的数据表名
        /// </summary>
        /// <param name="applicationType"></param>
        /// <param name="autoMonitorType"></param>
        /// <returns></returns>
        public static string GetAutoMonitorTableName(ApplicationType applicationType, PollutantDataType autoMonitorType)
        {
            string tableName = "";
            switch (applicationType)
            {
                case ApplicationType.Air:
                    switch (autoMonitorType)
                    {
                        case PollutantDataType.Min1: tableName = "Air.TB_InfectantBy1"; break;
                        case PollutantDataType.Min5: tableName = "Air.TB_InfectantBy5"; break;
                        case PollutantDataType.Min10: tableName = "Air.TB_InfectantBy10"; break;
                        case PollutantDataType.Min30: tableName = "Air.TB_InfectantBy30"; break;
                        case PollutantDataType.Min60: tableName = "Air.TB_InfectantBy60"; break;
                        case PollutantDataType.OriDay: tableName = "Air.TB_InfectantByDay"; break;
                        case PollutantDataType.OriMonth: tableName = "Air.TB_InfectantByMonth"; break;
                        case PollutantDataType.RealTime: tableName = "Air.TB_InfectantByRT"; break;
                        case PollutantDataType.Hour: tableName = "AirReport.TB_HourReport"; break;
                        case PollutantDataType.Day: tableName = "AirReport.TB_DayReport"; break;
                        case PollutantDataType.Week: tableName = "AirReport.TB_WeekReport"; break;
                        case PollutantDataType.Month: tableName = "AirReport.TB_MonthReport"; break;
                        case PollutantDataType.Season: tableName = "AirReport.TB_SeasonReport"; break;
                        case PollutantDataType.Year: tableName = "AirReport.TB_YearReport"; break;
                        case PollutantDataType.InstrumentData60: tableName = "Air.TB_InstrumentDataBy60"; break;
                    }
                    break;
                case ApplicationType.Water:
                    switch (autoMonitorType)
                    {
                        case PollutantDataType.Min1: tableName = "Water.TB_InfectantBy1"; break;
                        case PollutantDataType.Min5: tableName = "Water.TB_InfectantBy5"; break;
                        case PollutantDataType.Min10: tableName = "Water.TB_InfectantBy10"; break;
                        case PollutantDataType.Min30: tableName = "Water.TB_InfectantBy30"; break;
                        case PollutantDataType.Min60: tableName = "Water.TB_InfectantBy60"; break;
                        case PollutantDataType.RealTime: tableName = "Water.TB_InfectantByRT"; break;
                        case PollutantDataType.Hour: tableName = "WaterReport.TB_HourReport"; break;
                        case PollutantDataType.Day: tableName = "WaterReport.TB_DayReport"; break;
                        case PollutantDataType.Week: tableName = "WaterReport.TB_WeekReport"; break;
                        case PollutantDataType.Month: tableName = "WaterReport.TB_MonthReport"; break;
                        case PollutantDataType.Season: tableName = "WaterReport.TB_SeasonReport"; break;
                        case PollutantDataType.Year: tableName = "WaterReport.TB_YearReport"; break;
                        case PollutantDataType.InstrumentData60: tableName = "Water.TB_InstrumentDataBy60"; break;
                    }
                    break;
                case ApplicationType.BlueAlga:
                    switch (autoMonitorType)
                    {
                        case PollutantDataType.Min5: tableName = "dbo.V_InfectantBy5"; break;
                        case PollutantDataType.Min60: tableName = "dbo.V_InfectantBy60"; break;
                    }
                    break;
            }
            return tableName;
        }

        /// <summary>
        /// 取得AQI数据类型对应数据库表名
        /// </summary>
        /// <param name="aQIDataType"></param>
        /// <returns></returns>
        public static string GetTableName(AQIDataType aQIDataType)
        {
            string tableName = "";
            switch (aQIDataType)
            {
                case AQIDataType.HourAQI: tableName = "AirRelease.TB_HourAQI"; break;
                case AQIDataType.HourAPI: tableName = "AirRelease.TB_HourAPI"; break;
                case AQIDataType.DayAQI: tableName = "AirRelease.TB_DayAQI"; break;
                case AQIDataType.DayAPI: tableName = "AirRelease.TB_DayAPI"; break;
		case AQIDataType.OriHourAQI: tableName = "Air.TB_OriHourAQI"; break;
                case AQIDataType.RegionHourAQI: tableName = "AirRelease.TB_RegionHourAQI"; break;
                case AQIDataType.RegionDayAQI: tableName = "AirReport.TB_RegionDayAQIReport"; break;
                case AQIDataType.RegionDayAPI: tableName = "AirReport.TB_RegionDayAPIReport"; break;
            }

            return tableName;
        }

        /// <summary>
        /// 取得自动监测数据类型对应的数据库连接字符串
        /// </summary>
        /// <param name="applicationType"></param>
        /// <param name="autoMonitorType"></param>
        /// <returns></returns>
        public static string GetConnectionName(ApplicationType applicationType, PollutantDataType autoMonitorType)
        {
            string connection = string.Empty;
            switch (applicationType)
            {
                case ApplicationType.Air:
                    switch (autoMonitorType)
                    {
                        case PollutantDataType.Min1:
                        case PollutantDataType.Min5:
                        case PollutantDataType.Min10:
                        case PollutantDataType.Min30:
                        case PollutantDataType.Min60:
                        case PollutantDataType.OriDay:
                        case PollutantDataType.OriMonth:
                        case PollutantDataType.RealTime:
                        case PollutantDataType.InstrumentData60:
                            connection = GetConnectionName(DataConnectionType.AirAutoMonitoring);
                            break;
                        case PollutantDataType.Hour:
                        case PollutantDataType.Day:
                        case PollutantDataType.Week:
                        case PollutantDataType.Month:
                        case PollutantDataType.Season:
                        case PollutantDataType.Year:
                            connection = GetConnectionName(DataConnectionType.MonitoringBusiness);
                            break;
                    }
                    break;
                case ApplicationType.Water:
                    switch (autoMonitorType)
                    {
                        case PollutantDataType.Min1:
                        case PollutantDataType.Min5:
                        case PollutantDataType.Min10:
                        case PollutantDataType.Min30:
                        case PollutantDataType.Min60:
                        case PollutantDataType.RealTime:
                        case PollutantDataType.InstrumentData60:
                            connection = GetConnectionName(DataConnectionType.WaterAutoMonitoring);
                            break;
                        case PollutantDataType.Hour:
                        case PollutantDataType.Day:
                        case PollutantDataType.Week:
                        case PollutantDataType.Month:
                        case PollutantDataType.Season:
                        case PollutantDataType.Year:
                            connection = GetConnectionName(DataConnectionType.MonitoringBusiness);
                            break;
                    }
                    break;
                case ApplicationType.BlueAlga:
                    switch (autoMonitorType)
                    {
                        case PollutantDataType.Min5:
                        case PollutantDataType.Min60:
                            connection = GetConnectionName(DataConnectionType.WaterAutoMonitoring);
                            break;
                    }
                    break;
            }
            return connection;
        }

        /// <summary>
        /// 数据库连接字符字符串取得
        /// </summary>
        /// <param name="dataConnectionType"></param>
        /// <returns></returns>
        public static string GetConnectionName(DataConnectionType dataConnectionType)
        {
            string connection = string.Empty;
            switch (dataConnectionType)
            {
                case DataConnectionType.BaseData:
                    connection = "AMS_BaseDataConnection";
                    break;
                case DataConnectionType.Frame:
                    connection = "Frame_Connection";
                    break;
                case DataConnectionType.AirAutoMonitoring:
                    connection = "AMS_AirAutoMonitorConnection";
                    break;
                case DataConnectionType.WaterAutoMonitoring:
                    connection = "AMS_WaterAutoMonitorConnection";
                    break;
                case DataConnectionType.MonitoringBusiness:
                    connection = "AMS_MonitoringBusinessConnection";
                    break;
            }

            return connection;
        }
    }
}
