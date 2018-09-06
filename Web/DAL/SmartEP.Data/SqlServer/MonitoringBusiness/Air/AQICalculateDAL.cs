﻿using log4net;
using SmartEP.Core.Generic;
using SmartEP.Utilities.AdoData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.MonitoringBusiness.Air
{
    /// <summary>
    /// 名称：AQIDAL.cs
    /// 创建人：吕云
    /// 创建日期：2017-5-18
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：AQI计算
    /// 虚拟分页类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class AQICalculateDAL
    {
        #region <<变量>>
        /// <summary>
        /// 数据库处理类
        /// </summary>
        DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string MonitoringBusinessConnection = "AMS_MonitoringBusinessConnection";
        private string AirAutoMonitorConnection = "AMS_AirAutoMonitorConnection";
        private string BaseDataConnection = "AMS_BaseDataConnection";

        ILog log = LogManager.GetLogger("FileLogging");//获取一个日志记录器

        #endregion
        #region <<方法>>
        /// <summary>
        /// 获取浓度限值和空气质量分指数
        /// </summary>
        /// <param name="PollutantCode">因子编码</param>
        /// <param name="TimeTypeUid">时间类型Uid</param>
        /// <returns></returns>
        public DataTable GetFactorLimitAndIAQI(string PollutantCode, string TimeTypeUid)
        {
            try
            {
                string sql = string.Format(@"select * from [Audit].[TB_AQI]
                                            where [PollutantCode]='{0}'
                                            and [TimeTypeUid]='{1}'
                                            order by [IAQI]", PollutantCode, TimeTypeUid);
                return g_DatabaseHelper.ExecuteDataTable(sql, MonitoringBusinessConnection);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 获取多站点小时因子浓度数据
        /// </summary>
        /// <param name="PointIds">站点</param>
        /// <param name="PollutantCode">因子编码</param>
        /// <param name="Tstamp">时间</param>
        /// <returns></returns>
        public DataTable GetHourRegionValue(string[] PointIds, string PollutantCode, DateTime Tstamp,string Type)
        {
            try
            {
                string pointStr = string.Join(",", PointIds);
                string sql = string.Empty;
                if (Type == "2")//审核
                {
                    sql = string.Format(@"select
 AVG(case when [PollutantCode]='a21005' then  convert(numeric(18, 4),[dbo].[F_Round](PollutantValue,1)) else convert(numeric(18, 4),[dbo].[F_Round](PollutantValue,3)) end) as PollutantValue 
 from [AirReport].[TB_HourReport]
                                                where 1=1  and PointId in({0}) and [PollutantCode]='{1}'
                                                and Tstamp ='{2}'
                                                and [dbo].[F_ValidValueByFlagNT](PollutantValue,[AuditFlag],',') is not null"
                    , pointStr, PollutantCode, Tstamp);
                }
                else//原始
                {
                    sql = string.Format(@"select 
 AVG(case when [PollutantCode]='a21005' then  convert(numeric(18, 4),[dbo].[F_Round](PollutantValue,1)) else convert(numeric(18, 4),[dbo].[F_Round](PollutantValue,3)) end) as PollutantValue 
from [dbo].[SY_Air_InfectantBy60]
                                                where 1=1  and PointId in({0}) and [PollutantCode]='{1}'
                                                and Tstamp ='{2}'
                                                and [dbo].[F_ValidValueByFlagNT](PollutantValue,[Status],',') is not null"
                    , pointStr, PollutantCode, Tstamp);
                }
                //log.Info("计算区域浓度-------"+sql.ToString());
                return g_DatabaseHelper.ExecuteDataTable(sql, MonitoringBusinessConnection);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 获取区域小时数据
        /// </summary>
        /// <param name="PointIds">站点</param>
        /// <param name="sTime">开始时间</param>
        /// <param name="eTime">结束时间</param>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public DataTable GetRegionHourData(string[] PointIds, DateTime StartTime, DateTime EndTime, string TableName)
        {
            try
            {
                string pointStr = string.Join(",", PointIds);
                string sql = string.Format(@"with a as(
SELECT  region.RegionUid,region.Region,DateTime
			,[dbo].[F_Round]([dbo].[F_GetValidRegionValue](COUNT(*),SUM(CASE WHEN [SO2] IS not NULL and [SO2] !='' THEN 1 ELSE 0 END),AVG(dbo.F_ValidValueStr([SO2])),MAX(dbo.F_ValidValueStr([SO2])),'hour'),3) AS [SO2]
			,[dbo].[F_Round]([dbo].[F_GetValidRegionValue](COUNT(*),SUM(CASE WHEN [NO2] IS not NULL and [NO2] !='' THEN 1 ELSE 0 END),AVG(dbo.F_ValidValueStr([NO2])),MAX(dbo.F_ValidValueStr([NO2])),'hour'),3) AS [NO2]
			,[dbo].[F_Round]([dbo].[F_GetValidRegionValue](COUNT(*),SUM(CASE WHEN [PM10] IS not NULL and [PM10] !='' THEN 1 ELSE 0 END),AVG(dbo.F_ValidValueStr([PM10])),MAX(dbo.F_ValidValueStr([PM10])),'hour'),3) AS [PM10]
			,[dbo].[F_Round]([dbo].[F_GetValidRegionValue](COUNT(*),SUM(CASE WHEN [Recent24HoursPM10] IS not NULL and [Recent24HoursPM10] !='' THEN 1 ELSE 0 END),AVG(dbo.F_ValidValueStr([Recent24HoursPM10])),MAX(dbo.F_ValidValueStr([Recent24HoursPM10])),'hour'),3) AS [Recent24HoursPM10]
			,[dbo].[F_Round]([dbo].[F_GetValidRegionValue](COUNT(*),SUM(CASE WHEN [CO] IS not NULL and [CO] !='' THEN 1 ELSE 0 END),AVG(dbo.F_ValidValueStr([CO])),MAX(dbo.F_ValidValueStr([CO])),'hour'),1) AS [CO]
			,[dbo].[F_Round]([dbo].[F_GetValidRegionValue](COUNT(*),SUM(CASE WHEN [O3] IS not NULL and [O3] !='' THEN 1 ELSE 0 END),AVG(dbo.F_ValidValueStr([O3])),MAX(dbo.F_ValidValueStr([O3])),'hour'),3) AS [O3]
			,[dbo].[F_Round]([dbo].[F_GetValidRegionValue](COUNT(*),SUM(CASE WHEN [Recent8HoursO3] IS not NULL and [Recent8HoursO3] !='' THEN 1 ELSE 0 END),AVG(dbo.F_ValidValueStr([Recent8HoursO3])),MAX(dbo.F_ValidValueStr([Recent8HoursO3])),'hour'),3) AS [Recent8HoursO3]
            ,[dbo].[F_Round]([dbo].[F_GetValidRegionValue](COUNT(*),SUM(CASE WHEN [Recent8HoursO3NT] IS not NULL and [Recent8HoursO3NT] !='' THEN 1 ELSE 0 END),AVG(dbo.F_ValidValueStr([Recent8HoursO3NT])),MAX(dbo.F_ValidValueStr([Recent8HoursO3NT])),'hour'),3) AS [Recent8HoursO3NT]
			,[dbo].[F_Round]([dbo].[F_GetValidRegionValue](COUNT(*),SUM(CASE WHEN [PM25] IS not NULL and [PM25] !='' THEN 1 ELSE 0 END),AVG(dbo.F_ValidValueStr([PM25])),MAX(dbo.F_ValidValueStr([PM25])),'hour'),3) AS [PM25]
			,[dbo].[F_Round]([dbo].[F_GetValidRegionValue](COUNT(*),SUM(CASE WHEN [Recent24HoursPM25] IS not NULL and [Recent24HoursPM25] !='' THEN 1 ELSE 0 END),AVG(dbo.F_ValidValueStr([Recent24HoursPM25])),MAX(dbo.F_ValidValueStr([Recent24HoursPM25])),'hour'),3) AS [Recent24HoursPM25]
		FROM {0} data  right join AMS_BaseData.dbo.V_Point_UserConfigNew region
		on  data.PointId=region.PortId
		WHERE 1=1
		AND PointId in({1})
		AND DateTime>= '{2}'
			AND DateTime<='{3}'
		GROUP BY region.RegionUid,region.Region,DateTime)
, b as(
select *
,[dbo].[f_getAQI]([SO2],'a21026',1) [SO2_IAQI]
,[dbo].[f_getAQI]([NO2],'a21004',1) [NO2_IAQI]
,[dbo].[f_getAQI]([PM10],'a34002',24) [PM10_IAQI] 
,[dbo].[f_getAQI]([Recent24HoursPM10],'a34002',24) [Recent24HoursPM10_IAQI]
,[dbo].[f_getAQI]([CO],'a21005',1) [CO_IAQI]
,[dbo].[f_getAQI]([O3],'a05024',1) [O3_IAQI]
,[dbo].[f_getAQI]([Recent8HoursO3],'a05024',8) [Recent8HoursO3_IAQI]
,[dbo].[f_getAQI]([Recent8HoursO3NT],'a05024',8) [Recent8HoursO3NT_IAQI]
,[dbo].[f_getAQI]([PM25],'a34004',24) [PM25_IAQI] 
,[dbo].[f_getAQI]([Recent24HoursPM25],'a34004',24) [Recent24HoursPM25_IAQI]
 from a )
, c as(
select *
      ,[dbo].[F_GetAQI_Max_CNV_Hour] ([SO2_IAQI],[NO2_IAQI],[PM10_IAQI],[CO_IAQI],[O3_IAQI],[Recent8HoursO3_IAQI],[PM25_IAQI],'V') [AQIValue]
      ,[dbo].[F_GetAQI_Max_CNV_Hour] ([SO2_IAQI],[NO2_IAQI],[PM10_IAQI],[CO_IAQI],[O3_IAQI],[Recent8HoursO3_IAQI],[PM25_IAQI],'N') [PrimaryPollutant]
      from b)
      select RegionUid,Region as PointId,DateTime
      ,[SO2]
      ,[SO2_IAQI]
      ,[NO2]
      ,[NO2_IAQI]
      ,[PM10]
      ,[PM10_IAQI]
      ,[Recent24HoursPM10]
      ,[Recent24HoursPM10_IAQI]
      ,[CO]
      ,[CO_IAQI]
      ,[O3]
      ,[O3_IAQI]
      ,[Recent8HoursO3]
      ,[Recent8HoursO3_IAQI]
      ,[Recent8HoursO3NT]
      ,[Recent8HoursO3NT_IAQI]
      ,[PM25]
      ,[PM25_IAQI]
      ,[Recent24HoursPM25]
      ,[Recent24HoursPM25_IAQI]
      ,[AQIValue]
      ,[PrimaryPollutant]
      ,dbo.F_GetAQI_Grade([AQIValue],'RANGE') [Range]
      ,dbo.F_GetAQI_Grade([AQIValue],'Color') [RGBValue]
      ,dbo.F_GetAQI_Grade([AQIValue],'CLASS') [Class]
      ,dbo.F_GetAQI_Grade([AQIValue],'GRADE') [Grade]
      ,dbo.F_GetAQI_Grade([AQIValue],'HEALTHEFFECT') [HealthEffect]
      ,dbo.F_GetAQI_Grade([AQIValue],'TAKESTEP') [TakeStep]
      from c", TableName, pointStr, StartTime.ToString("yyyy-MM-dd HH:mm:ss"), EndTime.ToString("yyyy-MM-dd HH:59:59"));
                return g_DatabaseHelper.ExecuteDataTable(sql, MonitoringBusinessConnection);

            }
            catch (Exception ex)
            {
                log.Error("----------------GetRegionHourData数据库处理方法异常：" + ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 获取区域日数据
        /// </summary>
        /// <param name="PointIds">站点</param>
        /// <param name="sTime">开始时间</param>
        /// <param name="eTime">结束时间</param>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public DataTable GetRegionDayData(string[] PointIds, DateTime StartTime, DateTime EndTime, string TableName)
        {
            try
            {
                string pointStr = string.Join(",", PointIds);
                string sql = string.Format(@"with a as(
SELECT  region.RegionUid,region.Region,DateTime
			,[dbo].[F_Round]([dbo].[F_GetValidRegionValue](COUNT(*),SUM(CASE WHEN [SO2] IS not NULL and [SO2] !='' THEN 1 ELSE 0 END),AVG(dbo.F_ValidValueStr([SO2])),MAX(dbo.F_ValidValueStr([SO2])),'day'),3) AS [SO2]
			,[dbo].[F_Round]([dbo].[F_GetValidRegionValue](COUNT(*),SUM(CASE WHEN [NO2] IS not NULL and [NO2] !='' THEN 1 ELSE 0 END),AVG(dbo.F_ValidValueStr([NO2])),MAX(dbo.F_ValidValueStr([NO2])),'day'),3) AS [NO2]
			,[dbo].[F_Round]([dbo].[F_GetValidRegionValue](COUNT(*),SUM(CASE WHEN [PM10] IS not NULL and [PM10] !='' THEN 1 ELSE 0 END),AVG(dbo.F_ValidValueStr([PM10])),MAX(dbo.F_ValidValueStr([PM10])),'day'),3) AS [PM10]
			,[dbo].[F_Round]([dbo].[F_GetValidRegionValue](COUNT(*),SUM(CASE WHEN [CO] IS not NULL and [CO] !='' THEN 1 ELSE 0 END),AVG(dbo.F_ValidValueStr([CO])),MAX(dbo.F_ValidValueStr([CO])),'day'),1) AS [CO]
			,[dbo].[F_Round]([dbo].[F_GetValidRegionValue](COUNT(*),SUM(CASE WHEN [MaxOneHourO3] IS not NULL and [MaxOneHourO3] !='' THEN 1 ELSE 0 END),AVG(dbo.F_ValidValueStr([MaxOneHourO3])),MAX(dbo.F_ValidValueStr([MaxOneHourO3])),'day'),3) AS [MaxOneHourO3]
			,[dbo].[F_Round]([dbo].[F_GetValidRegionValue](COUNT(*),SUM(CASE WHEN [Max8HourO3] IS not NULL and [Max8HourO3] !='' THEN 1 ELSE 0 END),AVG(dbo.F_ValidValueStr([Max8HourO3])),MAX(dbo.F_ValidValueStr([Max8HourO3])),'day'),3) AS [Max8HourO3]
			,[dbo].[F_Round]([dbo].[F_GetValidRegionValue](COUNT(*),SUM(CASE WHEN [PM25] IS not NULL and [PM25] !='' THEN 1 ELSE 0 END),AVG(dbo.F_ValidValueStr([PM25])),MAX(dbo.F_ValidValueStr([PM25])),'day'),3) AS [PM25]
		FROM {0} data  right join AMS_BaseData.dbo.V_Point_UserConfigNew region
		on  data.PointId=region.PortId
		WHERE 1=1
		AND PointId in({1})
		AND DateTime>= '{2}'
			AND DateTime<='{3}'
		GROUP BY region.RegionUid,region.Region,DateTime)
, b as(
select *
,[dbo].[f_getAQI]([SO2],'a21026',24) [SO2_IAQI]
,[dbo].[f_getAQI]([NO2],'a21004',24) [NO2_IAQI]
,[dbo].[f_getAQI]([PM10],'a34002',24) [PM10_IAQI] 
,[dbo].[f_getAQI]([CO],'a21005',24) [CO_IAQI]
,[dbo].[f_getAQI]([MaxOneHourO3],'a05024',1) [MaxOneHourO3_IAQI]
,[dbo].[f_getAQI]([Max8HourO3],'a05024',8) [Max8HourO3_IAQI]
,[dbo].[f_getAQI]([PM25],'a34004',24) [PM25_IAQI] 
 from a )
, c as(
select *
      ,[dbo].[F_GetAQI_Max_CNV_Day] ([SO2_IAQI],[NO2_IAQI],[PM10_IAQI],[CO_IAQI],[Max8HourO3_IAQI],[PM25_IAQI],'V') [AQIValue]
      ,[dbo].[F_GetAQI_Max_CNV_Day] ([SO2_IAQI],[NO2_IAQI],[PM10_IAQI],[CO_IAQI],[Max8HourO3_IAQI],[PM25_IAQI],'N') [PrimaryPollutant]
      from b)
      select RegionUid,Region as PointId,DateTime
	  ,[SO2]
      ,[SO2_IAQI]
      ,[NO2]
      ,[NO2_IAQI]
      ,[PM10]
      ,[PM10_IAQI]
      ,[CO]
      ,[CO_IAQI]
      ,[MaxOneHourO3]
      ,[MaxOneHourO3_IAQI]
      ,[Max8HourO3]
      ,[Max8HourO3_IAQI]
      ,[PM25]
      ,[PM25_IAQI]
      ,[AQIValue]
      ,[PrimaryPollutant]
      ,dbo.F_GetAQI_Grade([AQIValue],'RANGE') [Range]
      ,dbo.F_GetAQI_Grade([AQIValue],'Color') [RGBValue]
      ,dbo.F_GetAQI_Grade([AQIValue],'CLASS') [Class]
      ,dbo.F_GetAQI_Grade([AQIValue],'GRADE') [Grade]
      ,dbo.F_GetAQI_Grade([AQIValue],'HEALTHEFFECT') [HealthEffect]
      ,dbo.F_GetAQI_Grade([AQIValue],'TAKESTEP') [TakeStep]
      from c", TableName, pointStr, StartTime.ToString("yyyy-MM-dd HH:mm:ss"), EndTime.ToString("yyyy-MM-dd HH:mm:ss"));
                return g_DatabaseHelper.ExecuteDataTable(sql, MonitoringBusinessConnection);
            }
            catch (Exception ex)
            {
                log.Error("----------------GetRegionDayData数据库处理方法异常：" + ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 获取区域时间段数据
        /// </summary>
        /// <param name="PointIds">站点</param>
        /// <param name="PollutantCode">因子</param>
        /// <param name="StartTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        public DataTable GetHourRegionValueByTime(string[] PointIds, string PollutantCode, DateTime StartTime, DateTime EndTime, string Type)
        {
            try
            {
                string pointStr = string.Join(",", PointIds);
                string sql = string.Empty;
                if (Type == "2")
                {
                    sql = string.Format(@"  select PointId,[PollutantCode]
, case when [PollutantCode]='a21005' then convert(numeric(18, 4),[dbo].[F_Round](
AVG(case when [PollutantCode]='a21005' then  convert(numeric(18, 4),[dbo].[F_Round](PollutantValue,1)) else convert(numeric(18, 4),[dbo].[F_Round](PollutantValue,3)) end)
,1) )
else convert(numeric(18, 4),[dbo].[F_Round](
AVG(case when [PollutantCode]='a21005' then  convert(numeric(18, 4),[dbo].[F_Round](PollutantValue,1)) else convert(numeric(18, 4),[dbo].[F_Round](PollutantValue,3)) end)
,3)) end
 PollutantValue
from [AirReport].[TB_HourReport]
                                                    where 1=1  and PointId in({0}) and [PollutantCode]='{1}'
                                                    and Tstamp >='{2}' and tstamp<='{3}'
                                                    and [dbo].[F_ValidValueByFlagNT](PollutantValue,[AuditFlag],',') is not null
                                                    group by PointId,[PollutantCode]"
                    , pointStr, PollutantCode, StartTime.ToString("yyyy-MM-dd HH:mm:ss"), EndTime.ToString("yyyy-MM-dd HH:59:59"));
                }
                else
                {
                    sql = string.Format(@"  select PointId,[PollutantCode]
, case when [PollutantCode]='a21005' then convert(numeric(18, 4),[dbo].[F_Round](
AVG(case when [PollutantCode]='a21005' then  convert(numeric(18, 4),[dbo].[F_Round](PollutantValue,1)) else convert(numeric(18, 4),[dbo].[F_Round](PollutantValue,3)) end)
,1) )
else convert(numeric(18, 4),[dbo].[F_Round](
AVG(case when [PollutantCode]='a21005' then  convert(numeric(18, 4),[dbo].[F_Round](PollutantValue,1)) else convert(numeric(18, 4),[dbo].[F_Round](PollutantValue,3)) end)
,3)) end
 PollutantValue
from [dbo].[SY_Air_InfectantBy60]
                                                    where 1=1  and PointId in({0}) and [PollutantCode]='{1}'
                                                    and Tstamp >='{2}' and tstamp<='{3}'
                                                    and [dbo].[F_ValidValueByFlagNT](PollutantValue,[Status],',') is not null
                                                    group by PointId,[PollutantCode]"
                    , pointStr, PollutantCode, StartTime.ToString("yyyy-MM-dd HH:mm:ss"), EndTime.ToString("yyyy-MM-dd HH:59:59"));
                }
                return g_DatabaseHelper.ExecuteDataTable(sql, MonitoringBusinessConnection);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 获取多站点日因子浓度数据
        /// </summary>
        /// <param name="PointIds"></param>
        /// <param name="PollutantCode"></param>
        /// <param name="Tstamp"></param>
        /// <returns></returns>
        public DataTable GetDayRegionValue(string[] PointIds, string PollutantCode, DateTime Tstamp, string Type)
        {
            try
            {
                string time = Tstamp.ToString("yyyy-MM-dd");
                string pointStr = string.Join(",", PointIds);
                string sql = string.Empty;
                if (Type == "2")
                {
                    sql = string.Format(@"select * from [AirReport].[TB_DayReport]
                                                where 1=1  and PointId in({0}) and [PollutantCode]='{1}'
                                                and Datetime ='{2}'
                                                and PollutantValue is not null"
                    , pointStr, PollutantCode, time);
                }
                else
                {
                    sql = string.Format(@"select * from [dbo].[SY_Air_InfectantByDay]
                                                where 1=1  and PointId in({0}) and [PollutantCode]='{1}'
                                                and Datetime ='{2}'
                                                and PollutantValue is not null"
                    , pointStr, PollutantCode, time);
                }
                return g_DatabaseHelper.ExecuteDataTable(sql, MonitoringBusinessConnection);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 获取多站点时间段数据
        /// </summary>
        /// <param name="PointIds">站点</param>
        /// <param name="PollutantCode">因子</param>
        /// <param name="StartTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        public DataTable GetDayRegionValueByTime(string[] PointIds, string PollutantCode, DateTime StartTime, DateTime EndTime, string Type)
        {
            try
            {
                string pointStr = string.Join(",", PointIds);
                //获取有效日数据
                string sql = string.Empty;
                if (Type == "2")
                {
                    sql = string.Format(@"select * from [AirReport].[TB_DayReport]
                                                where 1=1  and PointId in({0}) and [PollutantCode]='{1}'
                                                and Datetime >='{2}' and Datetime<='{3}'
                                                and PollutantValue is not null"
                    , pointStr, PollutantCode, StartTime.ToString("yyyy-MM-dd HH:mm:ss"), EndTime.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else
                {
                    sql = string.Format(@"select * from [dbo].[SY_Air_InfectantByDay]
                                                where 1=1  and PointId in({0}) and [PollutantCode]='{1}'
                                                and Datetime >='{2}' and Datetime<='{3}'
                                                and PollutantValue is not null"
                    , pointStr, PollutantCode, StartTime.ToString("yyyy-MM-dd HH:mm:ss"), EndTime.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                return g_DatabaseHelper.ExecuteDataTable(sql, MonitoringBusinessConnection);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 获取多站点可跨天臭氧8小时数据
        /// </summary>
        /// <param name="PointIds">站点</param>
        /// <param name="Tstamp">时间</param>
        /// <returns></returns>
        public DataTable GetO3_8NTRegionValue(string[] PointIds, DateTime Tstamp, string Type)
        {
            try
            {
                string pointStr = string.Join(",", PointIds);
                string sql = string.Empty;
                if (Type == "2")
                {
                    sql = string.Format(@"select Recent8HoursO3NT from AirRelease.TB_HourAQI
  where 1=1 and PointId in({0})
  and DateTime ='{1}'
  and Recent8HoursO3NT is not null and Recent8HoursO3NT !=''"
                        , pointStr, Tstamp);
                }
                else
                {
                    sql = string.Format(@"select Recent8HoursO3NT from dbo.SY_OriHourAQI
  where 1=1 and PointId in({0})
  and DateTime ='{1}'
  and Recent8HoursO3NT is not null and Recent8HoursO3NT !=''"
                        , pointStr, Tstamp);
                }
                return g_DatabaseHelper.ExecuteDataTable(sql, MonitoringBusinessConnection);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 获取多站点最大臭氧8小时数据
        /// </summary>
        /// <param name="PointIds">站点</param>
        /// <param name="Tstamp">时间</param>
        /// <returns></returns>
        public DataTable GetO3_8RegionValue(string[] PointIds, DateTime Tstamp, string Type)
        {
            try
            {
                string time = Tstamp.ToString("yyyy-MM-dd");
                string pointStr = string.Join(",", PointIds);
                string sql = string.Empty;
                if (Type == "2")
                {
                    sql = string.Format(@"select Max8HourO3 from AirRelease.TB_DayAQI
  where 1=1 and PointId in({0})
  and DateTime ='{1}'
  and Max8HourO3 is not null and Max8HourO3 !=''"
                        , pointStr, time);
                }
                else
                {
                    sql = string.Format(@"select Max8HourO3 from dbo.SY_OriDayAQI
  where 1=1 and PointId in({0})
  and DateTime ='{1}'
  and Max8HourO3 is not null and Max8HourO3 !=''"
                        , pointStr, time);
                }
                return g_DatabaseHelper.ExecuteDataTable(sql, MonitoringBusinessConnection);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 获取多站点最近臭氧时间段数据
        /// </summary>
        /// <param name="PointIds">站点</param>
        /// <param name="StartTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        public DataTable GetO3_8NTRegionValueByTime(string[] PointIds, DateTime StartTime, DateTime EndTime, string Type)
        {
            try
            {
                string pointStr = string.Join(",", PointIds);
                string sql = string.Empty;
                if (Type == "2")
                {
                    sql = string.Format(@"select [Recent8HoursO3NT] from AirRelease.TB_HourAQI
  where 1=1 and PointId in({0})
  and DateTime>='{1}' and DateTime<='{2}'
  and Recent8HoursO3NT is not null and Recent8HoursO3NT !=''"
                        , pointStr, StartTime.ToString("yyyy-MM-dd HH:mm:ss"), EndTime.ToString("yyyy-MM-dd HH:59:59"));
                }
                else
                {
                    sql = string.Format(@"select [Recent8HoursO3NT] from dbo.SY_OriHourAQI
  where 1=1 and PointId in({0})
  and DateTime>='{1}' and DateTime<='{2}'
  and Recent8HoursO3NT is not null and Recent8HoursO3NT !=''"
                        , pointStr, StartTime.ToString("yyyy-MM-dd HH:mm:ss"), EndTime.ToString("yyyy-MM-dd HH:59:59"));
                }
                return g_DatabaseHelper.ExecuteDataTable(sql, MonitoringBusinessConnection);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }
        public DataTable GetO3_8RegionValueByHours(string[] PointIds, DateTime StartTime, DateTime EndTime, string Type)
        {
            try
            {
                string pointStr = string.Join(",", PointIds);
                string sql = string.Empty;
                if (Type == "2")
                {
                    sql = string.Format(@"select PointId ,avg(convert(decimal(18,4),Recent8HoursO3)) Recent8HoursO3 from (
select PointId,CONVERT(varchar(100), DateTime, 23) DateTime
,max(Recent8HoursO3) Recent8HoursO3 from [AirRelease].[TB_HourAQI]
  where 1=1 and PointId in({0})
  and DateTime>='{1}' and DateTime<='{2}'
  group by  PointId,CONVERT(varchar(100), DateTime, 23))t group by PointId"
                        , pointStr, StartTime.ToString("yyyy-MM-dd HH:mm:ss"), EndTime.ToString("yyyy-MM-dd HH:59:59"));
                }
                else
                {
                    sql = string.Format(@"select PointId ,avg(convert(decimal(18,4),Recent8HoursO3)) Recent8HoursO3 from (
select PointId,CONVERT(varchar(100), DateTime, 23) DateTime
,max(Recent8HoursO3) Recent8HoursO3 from dbo.[SY_OriHourAQI]
  where 1=1 and PointId in({0})
  and DateTime>='{1}' and DateTime<='{2}'
  group by  PointId,CONVERT(varchar(100), DateTime, 23))t group by PointId"
                        , pointStr, StartTime.ToString("yyyy-MM-dd HH:mm:ss"), EndTime.ToString("yyyy-MM-dd HH:59:59"));
                }
                return g_DatabaseHelper.ExecuteDataTable(sql, MonitoringBusinessConnection);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }
        public DataTable GetNTO3_8RegionValueByHours(string[] PointIds, DateTime StartTime, DateTime EndTime, string Type)
        {
            try
            {
                string pointStr = string.Join(",", PointIds);
                string sql = string.Empty;
                if (Type == "2")
                {
                    sql = string.Format(@"select PointId
,max(convert(decimal(18,4),Recent8HoursO3NT)) Recent8HoursO3 from [AirRelease].[TB_HourAQI]
  where 1=1 and PointId in({0})
  and DateTime>='{1}' and DateTime<='{2}'
  group by  PointId", pointStr, StartTime.ToString("yyyy-MM-dd HH:mm:ss"), EndTime.ToString("yyyy-MM-dd HH:59:59"));
                }
                else
                {
                    sql = string.Format(@"select PointId
,max(convert(decimal(18,4),Recent8HoursO3NT)) Recent8HoursO3 from   dbo.[SY_OriHourAQI]
  where 1=1 and PointId in({0})
  and DateTime>='{1}' and DateTime<='{2}'
  group by  PointId", pointStr, StartTime.ToString("yyyy-MM-dd HH:mm:ss"), EndTime.ToString("yyyy-MM-dd HH:59:59"));
                }
                return g_DatabaseHelper.ExecuteDataTable(sql, MonitoringBusinessConnection);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 获取多站点臭氧时间段均值
        /// </summary>
        /// <param name="PointIds">站点</param>
        /// <param name="StartTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        public DataTable GetO3_8RegionValueByTime(string[] PointIds, DateTime StartTime, DateTime EndTime,string Type)
        {
            try
            {
                string pointStr = string.Join(",", PointIds);
                string sql = string.Empty;
                if (Type == "2")
                {
                    sql = string.Format(@"select Max8HourO3 from AirRelease.TB_DayAQI
  where 1=1 and PointId in({0})
  and DateTime>='{1}' and DateTime<='{2}'
  and Max8HourO3 is not null and Max8HourO3 !=''"
                        , pointStr, StartTime.ToString("yyyy-MM-dd HH:mm:ss"), EndTime.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else
                {
                    sql = string.Format(@"select Max8HourO3 from dbo.SY_OriDayAQI
  where 1=1 and PointId in({0})
  and DateTime>='{1}' and DateTime<='{2}'
  and Max8HourO3 is not null and Max8HourO3 !=''"
                        , pointStr, StartTime.ToString("yyyy-MM-dd HH:mm:ss"), EndTime.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                return g_DatabaseHelper.ExecuteDataTable(sql, MonitoringBusinessConnection);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        /// 获取因子相关配置信息
        /// </summary>
        /// <param name="PollutantCode"></param>
        /// <returns></returns>
        public DataTable GetPollutantUnit(string PollutantCode)
        {
            try
            {
                string sql = string.Format(@"select * from [dbo].[V_Factor_Air_SiteMap] where [PID]='{0}'", PollutantCode);
                return g_DatabaseHelper.ExecuteDataTable(sql, BaseDataConnection);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }

        }
        /// <summary>
        /// 获取数据无效标记位配置信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetFlag(string[] Flag)
        {
            try
            {
                string FlagStr = "'";
                FlagStr += string.Join("','", Flag);
                FlagStr += "'";

                string sql = string.Format(@"select * from [dbo].[DT_Flag] where [EnableOrNot]=1 and isValid=0 and Flag in({0})", FlagStr);
                return g_DatabaseHelper.ExecuteDataTable(sql, BaseDataConnection);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }
        #endregion
    }
}
