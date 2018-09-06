﻿using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NTDataProcessApplication
{
    /// <summary>
    /// 底层方法类
    /// </summary>
    public class DAL
    {
        #region <<变量>>
        /// <summary>
        /// 获取一个日志记录器
        /// </summary>
        ILog log = LogManager.GetLogger("App.Logging");

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string BaseDataConnection = "AMS_BaseDataConnection";
        private string AirAutoMonitorConnection = "AMS_AirAutoMonitorConnection";
        private string Frame_Connection = "Frame_Connection";
        private string MonitoringBusinessConnection = "AMS_MonitoringBusinessConnection";


        DatabaseHelper g_DatabaseHelper = new DatabaseHelper();
        #endregion

        #region <<方法>>
        /// <summary>
        /// 删除重复数据
        /// </summary>
        /// <param name="id">记录id</param>
        /// <param name="BufferTableName">缓存表</param>
        /// <returns></returns>
        public void DeleteRepeatData(long id, string BufferTableName)
        {
            try
            {
                string sql = string.Format(@"  with m as ( 
  select  *,  row_number() over(partition by PointId,Tstamp,PollutantCode order by id desc ) rn
  from {0}  where id >={1})
 delete m where rn>1 ", BufferTableName, id);
                g_DatabaseHelper.ExecuteScalar(sql, AirAutoMonitorConnection);
            }
            catch (Exception ex)
            {
                log.Error("----------------DeleteRepeatData数据库处理方法异常：" + ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 删除原始缓存表数据，原始表数据,update by xy
        /// </summary>
        /// <param name="sTime"></param>
        /// <param name="eTime"></param>
        public void DeleteOriDataByTime(DateTime sTime, DateTime eTime, string pid, string fac)
        {
            try
            {
                string sql = string.Format(@"delete FROM [Air].[TB_InfectantBy60Buffer]
                                            where Tstamp>='{0}'
                                            and Tstamp<='{1}' 
                                            and pointId<>204 and pointId in ({2}) and PollutantCode in ({3})
                                            delete FROM Air.TB_InfectantBy60
                                            where Tstamp>='{0}'
                                            and Tstamp<='{1}' 
                                            and pointId<>204 and pointId in ({2}) and PollutantCode in ({3})
                                            ", sTime.ToString("yyyy-MM-dd HH:mm:ss"), eTime.ToString("yyyy-MM-dd HH:mm:ss"), pid, fac);
                g_DatabaseHelper.ExecuteScalar(sql, AirAutoMonitorConnection);
            }
            catch (Exception ex)
            {
                log.Error("----------------DeleteRepeatData数据库处理方法异常：" + ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        /// 删除小时计算数据，审核小时数据
        /// </summary>
        /// <param name="sTime">开始时间</param>
        /// <param name="eTime">结束时间</param>
        public void DeleteAuditDataByTime(DateTime sTime, DateTime eTime, string pid, string fac)
        {
            try
            {
                string sql = string.Format(@"delete FROM [AirReport].[TB_HourReport_Calculate]
                                            where Tstamp>='{0}'
                                            and Tstamp<='{1}' and pointId<>204 and pointId in ({2}) and PollutantCode in ({3})
                                            delete FROM [AirReport].[TB_HourReport]
                                            where Tstamp>='{0}'
                                            and Tstamp<='{1}' and pointId<>204 and pointId in ({2}) and PollutantCode in ({3})
                                            ", sTime.ToString("yyyy-MM-dd HH:mm:ss"), eTime.ToString("yyyy-MM-dd HH:mm:ss"), pid, fac);
                g_DatabaseHelper.ExecuteScalar(sql, MonitoringBusinessConnection);
            }
            catch (Exception ex)
            {
                log.Error("----------------DeleteRepeatData数据库处理方法异常：" + ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 获取时间段数据
        /// </summary>
        /// <param name="PointId">站点</param>
        /// <param name="sTime">开始时间</param>
        /// <param name="eTime">结束时间</param>
        /// <param name="SYTableName">表名</param>
        public int GetRecentHourRecords(int PointId, DateTime sTime, DateTime eTime, string SYTableName)
        {
            try
            {
                string sql = string.Format(@"  select COUNT(t.Tstamp) from (select Tstamp from {3} 
where PointId={0} and Tstamp>='{1}' and Tstamp<'{2}' group by Tstamp) as t
                                            ", PointId, sTime.ToString("yyyy-MM-dd HH:mm:ss"), eTime.ToString("yyyy-MM-dd HH:mm:ss"), SYTableName);
                return Convert.ToInt32(g_DatabaseHelper.ExecuteScalar(sql, MonitoringBusinessConnection));
            }
            catch (Exception ex)
            {
                log.Error("----------------DeleteRepeatData数据库处理方法异常：" + ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 获取超标配置
        /// </summary>
        /// <param name="DataTypeUid">数据类型</param>
        /// <param name="UseForUid">规则用途</param>
        public DataTable GetExcessiveConfig(string DataTypeUid, string UseForUid, string ApplicationUid)
        {
            try
            {
                string sql = string.Format(@" 
SELECT excessive.MonitoringPointUid
			,smp.PointId
			,smp.MonitoringPointName
			,PollutantCode
			,PollutantName
			,NotifyGradeUid
			,ExcessiveUpper
			,ExcessiveLow
		FROM [BusinessRule].[TB_ExcessiveSetting] AS excessive
		right JOIN InstrInfo.TB_InstrumentChannels AS channel
			ON excessive.InstrumentChannelsUid = channel.InstrumentChannelsUid
		LEFT JOIN MPInfo.TB_MonitoringPoint AS smp
			ON excessive.MonitoringPointUid = smp.MonitoringPointUid
		LEFT JOIN dbo.SY_View_CodeMainItem AS grade
			ON NotifyGradeUid = grade.ItemGuid
		WHERE excessive.RowStatus=1
		AND excessive.ApplicationUid = '{2}'
		AND UseForUid = '{0}'
			AND DataTypeUid = '{1}'
			AND excessive.EnableOrNot = 1
			AND smp.EnableOrNot = 1
        ORDER BY grade.SortNumber DESC ", UseForUid, DataTypeUid, ApplicationUid);

                return g_DatabaseHelper.ExecuteDataTable(sql, BaseDataConnection);
            }
            catch (Exception ex)
            {
                log.Error("----------------GetExcessiveConfig数据库处理方法异常：" + ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 获取超上限数据
        /// </summary>
        /// <param name="DataTypeUid"></param>
        /// <param name="id"></param>
        /// <param name="Flag">标记</param>
        /// <param name="FlagType"></param>
        /// <param name="UseForUid">业务用途</param>
        /// <param name="SYBufferTableName">缓存表同义词</param>
        /// <returns></returns>
        public DataTable GetUpperData(string DataTypeUid, long id, string Flag, string FlagType, string UseForUid, string SYBufferTableName, string ApplicationUid)
        {
            try
            {
                string sql = string.Format(@" 	select * from
	(
SELECT excessive.MonitoringPointUid
			,smp.PointId
			,smp.MonitoringPointName
			,PollutantCode
			,PollutantName
			,NotifyGradeUid
			,ExcessiveUpper
			,ExcessiveLow
		FROM [BusinessRule].[TB_ExcessiveSetting] AS excessive
		right JOIN InstrInfo.TB_InstrumentChannels AS channel
			ON excessive.InstrumentChannelsUid = channel.InstrumentChannelsUid
		LEFT JOIN MPInfo.TB_MonitoringPoint AS smp
			ON excessive.MonitoringPointUid = smp.MonitoringPointUid
		LEFT JOIN dbo.SY_View_CodeMainItem AS grade
			ON NotifyGradeUid = grade.ItemGuid
		WHERE excessive.RowStatus=1
		AND excessive.ApplicationUid = '{6}'
		AND UseForUid = '{5}'
			AND DataTypeUid = '{0}'
			AND excessive.EnableOrNot = 1
			AND smp.EnableOrNot = 1) as data1
			left join {1} data2 WITH(NOLOCK)
		on data1.pointid=data2.pointid
		and data1.pollutantcode=data2.pollutantcode
		and data2.pollutantvalue>data1.excessiveupper
		where 1 = 1 
	and data2.id>={2}
	AND CHARINDEX(',{3},',','+ISNULL({4},'')+',')>0
    order by tstamp", DataTypeUid, SYBufferTableName, id, Flag, FlagType, UseForUid, ApplicationUid);

                return g_DatabaseHelper.ExecuteDataTable(sql, BaseDataConnection);
            }
            catch (Exception ex)
            {
                log.Error("----------------GetUpperData数据库处理方法异常：" + ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        /// 超上限标记更新
        /// </summary>
        /// <param name="DataTypeUid">数据类型Guid：分，时...</param>
        /// <param name="id">id记录</param>
        /// <param name="Flag">标记类型：超上限：HSp，超下限：LSp</param>
        /// <param name="FlagType">标记类型表中字段：审核：AuditFlag，报警：DataFlag</param>
        /// <param name="UseForUid">规则用途Guid：审核，报警</param>
        /// <param name="BufferTableName">缓存表同义词</param>
        /// <param name="UpdateUser">更新人</param>
        /// <param name="UpdateDateTime">更新时间</param>
        public void FlagUpper(string DataTypeUid, long id, string Flag, string FlagType, string UseForUid, string TableName, string ApplicationUid, string UpdateUser, DateTime UpdateDateTime)
        {
            try
            {
                string sql = string.Format(@" 	update data2
	set data2.{4} = case when (data2.{4} IS NULL or LEN(data2.{4})=0) then '{3}' else data2.[{4}]+',{3}' end,
    data2.UpdateUser='{7}',data2.UpdateDateTime='{8}'
	from 
	(
SELECT excessive.MonitoringPointUid
			,smp.PointId
			,smp.MonitoringPointName
			,PollutantCode
			,PollutantName
			,NotifyGradeUid
			,ExcessiveUpper
			,ExcessiveLow
		FROM [BusinessRule].[TB_ExcessiveSetting] AS excessive
		right JOIN InstrInfo.TB_InstrumentChannels AS channel
			ON excessive.InstrumentChannelsUid = channel.InstrumentChannelsUid
		LEFT JOIN MPInfo.TB_MonitoringPoint AS smp
			ON excessive.MonitoringPointUid = smp.MonitoringPointUid
		LEFT JOIN dbo.SY_View_CodeMainItem AS grade
			ON NotifyGradeUid = grade.ItemGuid
		WHERE excessive.RowStatus=1
		AND excessive.ApplicationUid = '{6}'
		AND UseForUid = '{5}'
			AND DataTypeUid = '{0}'
			AND excessive.EnableOrNot = 1
			AND smp.EnableOrNot = 1) as data1
			left join {1} data2 WITH(NOLOCK)
		on data1.pointid=data2.pointid
		and data1.pollutantcode=data2.pollutantcode
		and data2.pollutantvalue>data1.excessiveupper
		where 1 = 1 
	and data2.id>={2}
	AND CHARINDEX(',{3},',','+ISNULL({4},'')+',')=0", DataTypeUid, TableName, id, Flag, FlagType, UseForUid, ApplicationUid, UpdateUser, UpdateDateTime.ToString("yyyy-MM-dd HH:mm:ss"));

                g_DatabaseHelper.ExecuteScalar(sql, BaseDataConnection);
            }
            catch (Exception ex)
            {
                log.Error("----------------FlagUpper数据库处理方法异常：" + ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 记录超上限
        /// </summary>
        /// <param name="DataTypeUid">数据类型</param>
        /// <param name="id">id记录</param>
        /// <param name="Flag">标记</param>
        /// <param name="UseForUid">规则用途：审核，报警</param>
        /// <param name="SYBufferTableName">缓存表同义词</param>
        /// <param name="ip">ip地址</param>
        public void LogUpper(string DataTypeUid, long id, string Flag, string UseForUid, string SYBufferTableName, string ip, string ApplicationUid)
        {
            try
            {
                string sql = string.Format(@"	insert into dbo.SY_TB_AuditAirLog
([AuditLogUid],[tstamp],[AuditTime],[AuditType],[PollutantCode],[PollutantName],[SourcePollutantDataValue],[AuditPollutantDataValue],[OperationTypeEnum],[OperationReason],[UserIP],[CreatUser],[CreatDateTime])
	select NEWID(),data2.Tstamp,GETDATE(),'自动审核', data2.PollutantCode , PollutantName=(select [PollutantName] from Standard.TB_PollutantCode where PollutantCode=data2.PollutantCode)
	,data2.PollutantValue ,convert(nvarchar,data2.[PollutantValue])+'({0})' ,'自动审核','自动审核' ,'{1}','SystemSync',GETDATE()
	from 
	( SELECT excessive.MonitoringPointUid
			,smp.PointId
			,smp.MonitoringPointName
			,PollutantCode
			,PollutantName
			,NotifyGradeUid
			,ExcessiveUpper
			,ExcessiveLow
		FROM [BusinessRule].[TB_ExcessiveSetting] AS excessive
		right JOIN InstrInfo.TB_InstrumentChannels AS channel
			ON excessive.InstrumentChannelsUid = channel.InstrumentChannelsUid
		LEFT JOIN MPInfo.TB_MonitoringPoint AS smp
			ON excessive.MonitoringPointUid = smp.MonitoringPointUid
		LEFT JOIN dbo.SY_View_CodeMainItem AS grade
			ON NotifyGradeUid = grade.ItemGuid
		WHERE excessive.RowStatus=1 AND excessive.ApplicationUid = '{6}'
		AND UseForUid = '{2}' AND DataTypeUid = '{3}' AND excessive.EnableOrNot = 1 AND smp.EnableOrNot = 1) as data1
			left join {4} data2 WITH(NOLOCK)
		on data1.pointid=data2.pointid
		and data1.pollutantcode=data2.pollutantcode
		and data2.pollutantvalue>data1.excessiveupper
		where 1 = 1  and data2.id>={5} AND CHARINDEX(',{0},',','+ISNULL(AuditFlag,'')+',')>0
        AND not exists(
  select * from dbo.SY_TB_AuditAirLog [log]
  where data2.tstamp=[log].tstamp and [log].[AuditType]='自动审核' and data2.PollutantCode=[log].PollutantCode 
  and convert(nvarchar,data2.pollutantvalue)+'({0})'=[log].[AuditPollutantDataValue] 
  and [log].UserIP='{1}' and CreatUser='SystemSync' )"
                    , Flag, ip, UseForUid, DataTypeUid, SYBufferTableName, id, ApplicationUid);
                g_DatabaseHelper.ExecuteInsert(sql, BaseDataConnection);
            }
            catch (Exception ex)
            {
                log.Error("----------------LogUpper数据库处理方法异常：" + ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 新增超上限报警信息
        /// </summary>
        /// <param name="ApplicationUid"></param>
        /// <param name="PUid">站点Uid</param>
        /// <param name="AlarmTypeUid_Hsp">报警类型</param>
        /// <param name="NotifyGradeUid">通知级别</param>
        /// <param name="DataTypeUid">数据类型：分，时...</param>
        /// <param name="PName">站点名称</param>
        /// <param name="ExcessiveUpper">上限</param>
        /// <param name="FactorName">因子名称</param>
        /// <param name="FactorCode">因子编码</param>
        /// <param name="PId">站点id</param>
        /// <param name="SYBufferTableName">缓存表名称</param>
        /// <param name="DateStart">开始时间</param>
        /// <param name="DateEnd">截止时间</param>
        public void CreateAlarmUpper(string ApplicationUid, string PUid, string AlarmTypeUid_Hsp, string NotifyGradeUid, string DataTypeUid, string PName, decimal ExcessiveUpper, string FactorName, string FactorCode
            , string PId, string SYBufferTableName, DateTime DateStart, DateTime DateEnd, string Flag)
        {
            try
            {
                string ApplicationName = string.Empty;
                if (ApplicationUid == "airaaira-aira-aira-aira-airaairaaira")
                {
                    ApplicationName = "气";
                }
                else
                {
                    ApplicationName = "水";
                }
                string sql = string.Format(@"INSERT INTO [AlarmNotify].[TB_CreatAlarm]([AlarmUid],[ApplicationUid],[MonitoringPointUid],[RecordDateTime],[AlarmEventUid],[AlarmGradeUid]
					,[DataTypeUid],[Content],[SendContent],ItemCode,[ItemName],[MonitoringPoint],[ItemValue],[CreatUser],[CreatDateTime])
					SELECT [AlarmUid] = NEWID()
						,'{0}' ,'{1}' ,Tstamp ,'{2}' ,'{3}' ,'{4}'
						,content='{5}['+convert(nvarchar(19),Tstamp,120)+'][{8}]['+convert(nvarchar(20),PollutantValue)+']超出上限值[{6}]'
						,SendContent='@1@={7},@2@={5},@3@='+cast(datepart(month,Tstamp) as nvarchar(10))+',@4@='+cast(datepart(day,Tstamp) as nvarchar(10))+',@5@='+cast(datepart(hour,Tstamp) as nvarchar(10))+',@6@={8},@7@='+CAST(PollutantValue AS NVARCHAR(20))+''
						,'{9}' ,'{8}' ,'{5}' ,PollutantValue ,'SystemSync' ,GETDATE()
					FROM {11}
					WHERE [PointId]={10} AND [PollutantCode]='{9}' AND [PollutantValue]>{6} 
					and Tstamp>='{12}' and Tstamp<='{13}'
					AND CHARINDEX(',{14},',','+ISNULL([DataFlag],'')+',')>0
					and not exists(
					SELECT * FROM [AlarmNotify].[TB_CreatAlarm]
				where ApplicationUid='{0}' and MonitoringPointUid='{1}'
				and AlarmEventUid='{2}' and AlarmGradeUid='{3}' and DataTypeUid='{4}'
				and ItemCode='{9}' and RecordDateTime>='{12}' and RecordDateTime<='{13}'
					)"
                    , ApplicationUid, PUid, AlarmTypeUid_Hsp, NotifyGradeUid, DataTypeUid, PName, ExcessiveUpper, ApplicationName, FactorName, FactorCode, PId, SYBufferTableName, DateStart.ToString("yyyy-MM-dd HH:mm:ss"), DateEnd.ToString("yyyy-MM-dd HH:mm:ss"), Flag);
                g_DatabaseHelper.ExecuteInsert(sql, BaseDataConnection);
            }
            catch (Exception ex)
            {
                log.Error("----------------CreateAlarmUpper数据库处理方法异常：" + ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 获取超下限数据
        /// </summary>
        /// <param name="DataTypeUid"></param>
        /// <param name="id"></param>
        /// <param name="Flag">标记</param>
        /// <param name="FlagType">缓存表标记字段</param>
        /// <param name="UseForUid">业务用途</param>
        /// <param name="SYBufferTableName">缓存表</param>
        /// <param name="ApplicationUid">应用程序类型</param>
        /// <returns></returns>
        public DataTable GetLowerData(string DataTypeUid, long id, string Flag, string FlagType, string UseForUid, string SYBufferTableName, string ApplicationUid)
        {
            try
            {
                string sql = string.Format(@" 	select * from
	(
SELECT excessive.MonitoringPointUid
			,smp.PointId
			,smp.MonitoringPointName
			,PollutantCode
			,PollutantName
			,NotifyGradeUid
			,ExcessiveUpper
			,ExcessiveLow
		FROM [BusinessRule].[TB_ExcessiveSetting] AS excessive
		right JOIN InstrInfo.TB_InstrumentChannels AS channel
			ON excessive.InstrumentChannelsUid = channel.InstrumentChannelsUid
		LEFT JOIN MPInfo.TB_MonitoringPoint AS smp
			ON excessive.MonitoringPointUid = smp.MonitoringPointUid
		LEFT JOIN dbo.SY_View_CodeMainItem AS grade
			ON NotifyGradeUid = grade.ItemGuid
		WHERE excessive.RowStatus=1
		AND excessive.ApplicationUid = '{6}'
		AND UseForUid = '{5}'
			AND DataTypeUid = '{0}'
			AND excessive.EnableOrNot = 1
			AND smp.EnableOrNot = 1) as data1
			left join {1} data2 WITH(NOLOCK)
		on data1.pointid=data2.pointid
		and data1.pollutantcode=data2.pollutantcode
		and data2.pollutantvalue<data1.ExcessiveLow
		where 1 = 1 
	and data2.id>={2}
	AND CHARINDEX(',{3},',','+ISNULL({4},'')+',')>0
    order by tstamp", DataTypeUid, SYBufferTableName, id, Flag, FlagType, UseForUid, ApplicationUid);

                return g_DatabaseHelper.ExecuteDataTable(sql, BaseDataConnection);
            }
            catch (Exception ex)
            {
                log.Error("----------------GetLowerData数据库处理方法异常：" + ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 超下限标记更新
        /// </summary>
        /// <param name="DataTypeUid">数据类型Guid：分，时...</param>
        /// <param name="id">id记录</param>
        /// <param name="Flag">标记类型：超上限：HSp，超下限：LSp</param>
        /// <param name="FlagType">标记类型表中字段：审核：AuditFlag，报警：DataFlag</param>
        /// <param name="BufferTableName">缓存表同义词</param>
        public void FlagLower(string DataTypeUid, long id, string Flag, string FlagType, string UseForUid, string SYBufferTableName, string ApplicationUid, string UpdateUser, DateTime UpdateDateTime)
        {
            try
            {
                string sql = string.Format(@" 	update data2
	set data2.{4} = case when (data2.{4} IS NULL or LEN(data2.{4})=0) then '{3}' else data2.[{4}]+',{3}' end,
data2.UpdateUser='{7}',data2.UpdateDateTime='{8}'
	from 
	(
SELECT excessive.MonitoringPointUid
			,smp.PointId
			,smp.MonitoringPointName
			,PollutantCode
			,PollutantName
			,NotifyGradeUid
			,ExcessiveUpper
			,ExcessiveLow
		FROM [BusinessRule].[TB_ExcessiveSetting] AS excessive
		right JOIN InstrInfo.TB_InstrumentChannels AS channel
			ON excessive.InstrumentChannelsUid = channel.InstrumentChannelsUid
		LEFT JOIN MPInfo.TB_MonitoringPoint AS smp
			ON excessive.MonitoringPointUid = smp.MonitoringPointUid
		LEFT JOIN dbo.SY_View_CodeMainItem AS grade
			ON NotifyGradeUid = grade.ItemGuid
		WHERE excessive.RowStatus=1
		AND excessive.ApplicationUid = '{6}'
		AND UseForUid = '{5}' AND DataTypeUid = '{0}' AND excessive.EnableOrNot = 1 AND smp.EnableOrNot = 1) as data1
			left join {1} data2 WITH(NOLOCK)
		on data1.pointid=data2.pointid
		and data1.pollutantcode=data2.pollutantcode
		and data2.pollutantvalue<data1.ExcessiveLow
		where 1 = 1  and data2.id>={2} AND CHARINDEX(',{3},',','+ISNULL({4},'')+',')=0"
                    , DataTypeUid, SYBufferTableName, id, Flag, FlagType, UseForUid, ApplicationUid, UpdateUser, UpdateDateTime.ToString("yyyy-MM-dd HH:mm:ss"));

                g_DatabaseHelper.ExecuteScalar(sql, BaseDataConnection);
            }
            catch (Exception ex)
            {
                log.Error("----------------FlagLower数据库处理方法异常：" + ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 记录超下限
        /// </summary>
        /// <param name="DataTypeUid">数据类型</param>
        /// <param name="id">id记录</param>
        /// <param name="Flag">标记</param>
        /// <param name="UseForUid">规则用途：审核，报警</param>
        /// <param name="SYBufferTableName">缓存表同义词</param>
        /// <param name="ip">ip地址</param>
        /// <returns></returns>
        public void LogLower(string DataTypeUid, long id, string Flag, string UseForUid, string SYBufferTableName, string ip, string ApplicationUid)
        {
            try
            {
                string sql = string.Format(@"	insert into dbo.SY_TB_AuditAirLog
([AuditLogUid],[tstamp],[AuditTime],[AuditType],[PollutantCode],[PollutantName],[SourcePollutantDataValue],[AuditPollutantDataValue],[OperationTypeEnum],[OperationReason],[UserIP],[CreatUser],[CreatDateTime])
	select NEWID(),data2.Tstamp,GETDATE(),'自动审核', data2.PollutantCode , PollutantName=(select [PollutantName] from Standard.TB_PollutantCode where PollutantCode=data2.PollutantCode)
	,data2.PollutantValue ,convert(nvarchar,data2.[PollutantValue])+'({0})' ,'自动审核','自动审核' ,'{1}','SystemSync',GETDATE()
	from 
	( SELECT excessive.MonitoringPointUid
			,smp.PointId
			,smp.MonitoringPointName
			,PollutantCode
			,PollutantName
			,NotifyGradeUid
			,ExcessiveUpper
			,ExcessiveLow
		FROM [BusinessRule].[TB_ExcessiveSetting] AS excessive
		right JOIN InstrInfo.TB_InstrumentChannels AS channel
			ON excessive.InstrumentChannelsUid = channel.InstrumentChannelsUid
		LEFT JOIN MPInfo.TB_MonitoringPoint AS smp
			ON excessive.MonitoringPointUid = smp.MonitoringPointUid
		LEFT JOIN dbo.SY_View_CodeMainItem AS grade
			ON NotifyGradeUid = grade.ItemGuid
		WHERE excessive.RowStatus=1 AND excessive.ApplicationUid = '{6}'
		AND UseForUid = '{2}' AND DataTypeUid = '{3}' AND excessive.EnableOrNot = 1 AND smp.EnableOrNot = 1) as data1
			left join {4} data2 WITH(NOLOCK)
		on data1.pointid=data2.pointid
		and data1.pollutantcode=data2.pollutantcode
		and data2.pollutantvalue<data1.ExcessiveLow
		where 1 = 1  and data2.id>={5} AND CHARINDEX(',{0},',','+ISNULL(AuditFlag,'')+',')>0
AND not exists(
  select * from dbo.SY_TB_AuditAirLog [log]
  where data2.tstamp=[log].tstamp and [log].[AuditType]='自动审核' and data2.PollutantCode=[log].PollutantCode 
  and convert(nvarchar,data2.pollutantvalue)+'({0})'=[log].[AuditPollutantDataValue] 
  and [log].UserIP='{1}' and CreatUser='SystemSync' )"
                    , Flag, ip, UseForUid, DataTypeUid, SYBufferTableName, id, ApplicationUid);
                g_DatabaseHelper.ExecuteInsert(sql, BaseDataConnection);
            }
            catch (Exception ex)
            {
                log.Error("----------------LogLower数据库处理方法异常：" + ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 新增超上限报警信息
        /// </summary>
        /// <param name="ApplicationUid"></param>
        /// <param name="PUid">站点Uid</param>
        /// <param name="AlarmTypeUid_Lsp">报警类型</param>
        /// <param name="NotifyGradeUid">通知级别</param>
        /// <param name="DataTypeUid">数据类型：分，时...</param>
        /// <param name="PName">站点名称</param>
        /// <param name="ExcessiveUpper">上限</param>
        /// <param name="FactorName">因子名称</param>
        /// <param name="FactorCode">因子编码</param>
        /// <param name="PId">站点id</param>
        /// <param name="SYBufferTableName">缓存表名称</param>
        /// <param name="DateStart">开始时间</param>
        /// <param name="DateEnd">截止时间</param>
        public void CreateAlarmLower(string ApplicationUid, string PUid, string AlarmTypeUid_Lsp, string NotifyGradeUid, string DataTypeUid, string PName, decimal ExcessiveLow, string FactorName, string FactorCode
            , string PId, string SYBufferTableName, DateTime DateStart, DateTime DateEnd, string Flag)
        {
            try
            {
                string ApplicationName = string.Empty;
                if (ApplicationUid == "airaaira-aira-aira-aira-airaairaaira")
                {
                    ApplicationName = "气";
                }
                else
                {
                    ApplicationName = "水";
                }
                string sql = string.Format(@"INSERT INTO [AlarmNotify].[TB_CreatAlarm]([AlarmUid],[ApplicationUid],[MonitoringPointUid],[RecordDateTime],[AlarmEventUid],[AlarmGradeUid]
					,[DataTypeUid],[Content],[SendContent],ItemCode,[ItemName],[MonitoringPoint],[ItemValue],[CreatUser],[CreatDateTime])
					SELECT [AlarmUid] = NEWID()
						,'{0}' ,'{1}' ,Tstamp ,'{2}' ,'{3}' ,'{4}'
						,content='{5}['+convert(nvarchar(19),Tstamp,120)+'][{8}]['+convert(nvarchar(20),PollutantValue)+']超出下限值[{6}]'
						,SendContent='@1@={7},@2@={5},@3@='+cast(datepart(month,Tstamp) as nvarchar(10))+',@4@='+cast(datepart(day,Tstamp) as nvarchar(10))+',@5@='+cast(datepart(hour,Tstamp) as nvarchar(10))+',@6@={8},@7@='+CAST(PollutantValue AS NVARCHAR(20))+''
						,'{9}' ,'{8}' ,'{5}' ,PollutantValue ,'SystemSync' ,GETDATE()
					FROM {11}
					WHERE [PointId]={10} AND [PollutantCode]='{9}' AND [PollutantValue]<{6} 
					and Tstamp>='{12}' and Tstamp<='{13}'
					AND CHARINDEX(',{14},',','+ISNULL([DataFlag],'')+',')>0
					and not exists(
					SELECT * FROM [AlarmNotify].[TB_CreatAlarm]
				where ApplicationUid='{0}' and MonitoringPointUid='{1}'
				and AlarmEventUid='{2}' and AlarmGradeUid='{3}' and DataTypeUid='{4}'
				and ItemCode='{9}' and RecordDateTime>='{12}' and RecordDateTime<='{13}'
					)"
                    , ApplicationUid, PUid, AlarmTypeUid_Lsp, NotifyGradeUid, DataTypeUid, PName, ExcessiveLow, ApplicationName, FactorName, FactorCode, PId, SYBufferTableName, DateStart, DateEnd, Flag);
                g_DatabaseHelper.ExecuteInsert(sql, BaseDataConnection);
            }
            catch (Exception ex)
            {
                log.Error("----------------CreateAlarmLower数据库处理方法异常：" + ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 获取重复值配置信息
        /// </summary>
        /// <param name="UseForUid">规则用途：审核，报警</param>
        /// <param name="DataTypeUid">数据类型Guid：分，时...</param>
        /// <returns></returns>
        public DataTable GetRepeatLimitConfig(string UseForUid, string DataTypeUid, string ApplicationUid)
        {
            try
            {
                string sql = string.Format(@"SELECT repeatLimit.MonitoringPointUid
			,smp.PointId
			,smp.MonitoringPointName
			,PollutantCode
			,PollutantName
			,NotifyGradeUid
			,RepeatableNumber
		FROM [BusinessRule].[TB_RepeatLimitSetting] AS repeatLimit
		LEFT JOIN InstrInfo.TB_InstrumentChannels AS channel
			ON repeatLimit.InstrumentChannelsUid = channel.InstrumentChannelsUid
		LEFT JOIN MPInfo.TB_MonitoringPoint AS smp
			ON repeatLimit.MonitoringPointUid = smp.MonitoringPointUid
		LEFT JOIN dbo.SY_View_CodeMainItem AS grade
			ON NotifyGradeUid = grade.ItemGuid
		WHERE repeatLimit.RowStatus=1
		AND repeatLimit.ApplicationUid = '{2}'
			AND UseForUid = '{0}'
			AND DataTypeUid = '{1}'
			AND repeatLimit.EnableOrNot = 1
			AND smp.EnableOrNot = 1
		ORDER BY grade.SortNumber DESC ", UseForUid, DataTypeUid, ApplicationUid);
                return g_DatabaseHelper.ExecuteDataTable(sql, BaseDataConnection);
            }
            catch (Exception ex)
            {
                log.Error("----------------GetRepeatLimitConfig数据库处理方法异常：" + ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 获取重复数据
        /// </summary>
        /// <param name="id">id记录</param>
        /// <param name="BufferTableName">缓存表</param>
        /// <param name="PointId">站点</param>
        /// <param name="PollutantCode">因子</param>
        public DataTable GetRepeatLimitData(long id, string BufferTableName, string PointId, string PollutantCode)
        {
            try
            {
                string sql = string.Format(@"with ##TmpTbRep_A as
( SELECT pointid,PollutantCode,Tstamp,PollutantValue
				,ROW_NUMBER() OVER(ORDER BY [tstamp]) AS IDS
			FROM {0}
			WHERE id>={1} 
			AND PointId='{2}'
				AND PollutantCode='{3}' )			
			SELECT * FROM ##TmpTbRep_A AS temp_A
			LEFT JOIN ##TmpTbRep_A AS temp_B
				ON temp_A.IDS = (temp_B.IDS-1)
					AND temp_A.PollutantValue = temp_B.PollutantValue
			WHERE temp_A.PollutantValue IS NOT NULL
				AND temp_B.Tstamp IS NOT NULL", BufferTableName, id, PointId, PollutantCode);
                return g_DatabaseHelper.ExecuteDataTable(sql, AirAutoMonitorConnection);

            }
            catch (Exception ex)
            {
                log.Error("----------------GetRepeatLimitData数据库处理方法异常：" + ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 更新重复标记
        /// </summary>
        /// <param name="PointId">站点</param>
        /// <param name="PollutantCode">因子</param>
        /// <param name="DateStart">重复开始时间</param>
        /// <param name="DateEnd">重复截止时间</param>
        /// <param name="BufferTableName">缓存表</param>
        /// <param name="FlagType">标记类型表中字段：审核：AuditFlag，报警：DataFlag</param>
        public void RepeatLimitFlag(string PointId, string PollutantCode, DateTime DateStart, DateTime DateEnd, string BufferTableName, string FlagType, string UpdateUser, DateTime UpdateDateTime)
        {
            try
            {
                string sql = string.Format(@"UPDATE {0}  
SET {5}=case when ({5} IS NULL or LEN({5})=0) then 'Rep' else [{5}]+',Rep' end,
UpdateUser='{6}',UpdateDateTime='{7}'
									WHERE PointId='{1}' 
										AND [PollutantCode]='{2}'
										AND Tstamp>='{3}' 
										AND Tstamp<='{4}'
										AND CHARINDEX(',Rep,',','+ISNULL({5},'')+',')=0"
                    , BufferTableName, PointId, PollutantCode, DateStart, DateEnd, FlagType, UpdateUser, UpdateDateTime);
                g_DatabaseHelper.ExecuteScalar(sql, AirAutoMonitorConnection);
            }
            catch (Exception ex)
            {
                log.Error("----------------RepeatLimitFlag数据库处理方法异常：" + ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 记录重复审核
        /// </summary>
        /// <param name="PointId"></param>
        /// <param name="PollutantCode"></param>
        /// <param name="DateStart"></param>
        /// <param name="DateEnd"></param>
        /// <param name="BufferTableName"></param>
        /// <param name="FlagType"></param>
        /// <param name="ip"></param>
        public void LogRepeatLimit(string PointId, string PollutantCode, DateTime DateStart, DateTime DateEnd, string BufferTableName, string FlagType, string ip)
        {
            try
            {
                string sql = string.Format(@"insert into dbo.SY_TB_AuditAirLog([AuditLogUid],[tstamp],[AuditTime],[AuditType],[PollutantCode],[PollutantName],[SourcePollutantDataValue],[AuditPollutantDataValue],[OperationTypeEnum],[OperationReason],[UserIP],[CreatUser],[CreatDateTime])
	select  NEWID(),Tstamp,GETDATE(),'自动审核', PollutantCode
	, PollutantName=(select [PollutantName] from dbo.SY_PollutantCode where [PollutantCode]=a.PollutantCode)
		,PollutantValue ,convert(nvarchar,[PollutantValue])+'(Rep)'
	,'自动审核','自动审核' ,'{6}','SystemSync',GETDATE()
	from {0} a
									WHERE PointId='{1}' 
										AND [PollutantCode]='{2}'
										AND Tstamp>='{3}' 
										AND Tstamp<='{4}'
										AND CHARINDEX(',Rep,',','+ISNULL({5},'')+',')>0
                AND not exists(
  select * from dbo.SY_TB_AuditAirLog [log]
  where a.tstamp=[log].tstamp and [log].[AuditType]='自动审核' and a.PollutantCode=[log].PollutantCode 
  and convert(nvarchar,a.pollutantvalue)+'(Rep)'=[log].[AuditPollutantDataValue] 
  and [log].UserIP='{6}' and CreatUser='SystemSync' )"
                    , BufferTableName, PointId, PollutantCode, DateStart, DateEnd, FlagType, ip);
                g_DatabaseHelper.ExecuteInsert(sql, AirAutoMonitorConnection);
            }
            catch (Exception ex)
            {
                log.Error("----------------LogRepeatLimit数据库处理方法异常：" + ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 获取数据缺失配置信息
        /// </summary>
        /// <param name="DataTypeUid">数据类型</param>
        /// <param name="ApplicationUid">应用程序类型</param>
        /// <returns></returns>
        public DataTable GetLostConfig(string ApplicationUid, string DataTypeUid)
        {
            try
            {
                string sql = string.Format(@"SELECT lost.MonitoringPointUid
			,smp.PointId
			,smp.MonitoringPointName
			,NotifyGradeUid
			,PollutantCode
			,PollutantName
			,LostNum
		FROM BusinessRule.TB_LostSetting AS lost
		LEFT JOIN InstrInfo.TB_InstrumentChannels AS channel
			ON lost.InstrumentChannelsUid = channel.InstrumentChannelsUid
		LEFT JOIN MPInfo.TB_MonitoringPoint AS smp
			ON lost.MonitoringPointUid = smp.MonitoringPointUid
		LEFT JOIN dbo.SY_View_CodeMainItem AS grade
			ON NotifyGradeUid = grade.ItemGuid
		WHERE lost.RowStatus=1
			AND lost.ApplicationUid = '{0}'
			AND DataTypeUid = '{1}'
			AND lost.EnableOrNot = 1
			AND smp.EnableOrNot = 1
		ORDER BY grade.SortNumber DESC  ", ApplicationUid, DataTypeUid);
                return g_DatabaseHelper.ExecuteDataTable(sql, BaseDataConnection);
            }
            catch (Exception ex)
            {
                log.Error("----------------GetLostConfig数据库处理方法异常：" + ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 获取最新数据时间
        /// </summary>
        /// <param name="id"></param>
        /// <param name="BufferTableName"></param>
        /// <returns></returns>
        public DataTable GetLatestData(long id, string BufferTableName)
        {
            try
            {
                string sql = string.Format(@"SELECT TOP 1 *
					FROM {0}
					WHERE id>={1}
					ORDER BY Tstamp desc ", BufferTableName, id);
                return g_DatabaseHelper.ExecuteDataTable(sql, AirAutoMonitorConnection);

            }
            catch (Exception ex)
            {
                log.Error("----------------GetLatestData数据库处理方法异常：" + ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 获取最新数据时间
        /// </summary>
        /// <param name="id"></param>
        /// <param name="BufferTableName"></param>
        /// <returns></returns>
        public DataTable GetNewDataInstrument(string BufferTableName, string InstrumentUid)
        {
            try
            {
                string sql = string.Format(@"SELECT 
									MAX([Tstamp]) [Tstamp]
									FROM {0} by1
									right join (
									select  Ins.[RowGuid]
									,[InstrumentName] ,[PollutantCode] from [AMS_BaseData].[InstrInfo].[TB_Instruments] Ins
									left join [AMS_BaseData].[InstrInfo].[TB_InstrumentChannels] cha
									on Ins.RowGuid=cha.InstrumentUid
									where ApplyTypeUid='3b5ac81c-cefb-4db8-b19f-6c4c2f41eb03' and EnableOrNot=1 and Ins.RowGuid='{1}') a
									on by1.PollutantCode=a.[PollutantCode]
									where PointId=204", BufferTableName, InstrumentUid);
                return g_DatabaseHelper.ExecuteDataTable(sql, AirAutoMonitorConnection);

            }
            catch (Exception ex)
            {
                log.Error("----------------GetNewDataInstrument数据库处理方法异常：" + ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 获取当前站点最新数据时间
        /// </summary>
        /// <param name="id"></param>
        /// <param name="BufferTableName"></param>
        /// <param name="PointId"></param>
        /// <returns></returns>
        public DataTable GetLatestData(long id, string BufferTableName, int PointId)
        {
            try
            {
                string sql = string.Format(@"SELECT TOP 1 *
					FROM {0}
					WHERE id>={1}
						AND PointId =  '{2}'
					ORDER BY Tstamp desc ", BufferTableName, id, PointId);
                return g_DatabaseHelper.ExecuteDataTable(sql, AirAutoMonitorConnection);

            }
            catch (Exception ex)
            {
                log.Error("----------------GetLatestData数据库处理方法异常：" + ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 获取缓存表中该站点该因子最新一笔数据
        /// </summary>
        /// <param name="id">id记录</param>
        /// <param name="BufferTableName">缓存表</param>
        /// <param name="PointId">站点id</param>
        /// <param name="PollutantCode">因子编码</param>
        /// <returns></returns>
        public DataTable GetLatestData(long id, string BufferTableName, int PointId, string PollutantCode)
        {
            try
            {
                string sql = string.Format(@"SELECT TOP 1 *
					FROM {0}
					WHERE id>={1}
						AND PointId =  '{2}'
						AND PollutantCode='{3}'
                        AND PollutantValue IS NOT NULL
					ORDER BY Tstamp desc ", BufferTableName, id, PointId, PollutantCode);
                return g_DatabaseHelper.ExecuteDataTable(sql, AirAutoMonitorConnection);

            }
            catch (Exception ex)
            {
                log.Error("----------------GetLatestData数据库处理方法异常：" + ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 获取站点离线配置
        /// </summary>
        /// <param name="ApplicationUid">应用数据类型</param>
        /// <param name="DataTypeUid">数据类型</param>
        /// <returns></returns>
        public DataTable GetOfflineConfig(string ApplicationUid, string DataTypeUid)
        {
            try
            {
                string sql = string.Format(@"select [offline].MonitoringPointUid
			,smp.PointId
			,smp.MonitoringPointName
			,NotifyGradeUid
			,OffLineTimeSpan  
        from [BusinessRule].[TB_OfflineSetting] [offline]
		LEFT JOIN MPInfo.TB_MonitoringPoint AS smp
		ON [offline].MonitoringPointUid = smp.MonitoringPointUid
		LEFT JOIN dbo.SY_View_CodeMainItem AS grade
			ON NotifyGradeUid = grade.ItemGuid
			WHERE [offline].RowStatus=1
			AND [offline].ApplicationUid = '{0}'
			AND DataTypeUid = '{1}'
			AND [offline].EnableOrNot = 1
			AND smp.EnableOrNot = 1
		ORDER BY grade.SortNumber DESC   ", ApplicationUid, DataTypeUid);
                return g_DatabaseHelper.ExecuteDataTable(sql, BaseDataConnection);
            }
            catch (Exception ex)
            {
                log.Error("----------------GetOfflineConfig数据库处理方法异常：" + ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 获取站点离线配置
        /// </summary>
        /// <param name="ApplicationUid">应用数据类型</param>
        /// <param name="DataTypeUid">数据类型</param>
        /// <returns></returns>
        public DataTable GetInstrumentSet(string DataTypeUid)
        {
            try
            {
                string sql = string.Format(@"SELECT InstrumentUid
										,InstrumentName
										,[DataTypeUid]
										,[OffLineTimeSpan]
										FROM [AMS_BaseData].[dbo].[Instrument_OnlineSetting] line
										left join InstrInfo.TB_Instruments Ins
										on InstrumentUid=Ins.RowGuid
										where line.EnableOrNot = 1 and [DataTypeUid]='{0}'
										and ApplyTypeUid='3b5ac81c-cefb-4db8-b19f-6c4c2f41eb03' and Ins.EnableOrNot=1", DataTypeUid);
                return g_DatabaseHelper.ExecuteDataTable(sql, BaseDataConnection);
            }
            catch (Exception ex)
            {
                log.Error("----------------GetInstrumentSet数据库处理方法异常：" + ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        /// 获取当前测点最新在线时间信息
        /// </summary>
        /// <param name="PointId">站点id</param>
        /// <returns></returns>
        public DataTable GetLatestOnline(int PointId)
        {
            try
            {
                string sql = string.Format(@"select top 1*  FROM [dbo].[TB_OriginalPacketBackup] packet
		 left join [dbo].[SY_Point_Acquisition] acq
		 on packet.MN=acq.MN
  where packet.receiveTime>DATEADD(WEEK,-1,GETDATE()) 
  and PointId={0}
  and cmdContent like '%CN=4011%'
  order by receiveTime desc ", PointId);
                return g_DatabaseHelper.ExecuteDataTable(sql, AirAutoMonitorConnection);

            }
            catch (Exception ex)
            {
                log.Error("----------------GetLatestOnline数据库处理方法异常：" + ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 获取ip地址
        /// </summary>
        /// <returns></returns>
        public string GetIpAddress()
        {
            string hostName = Dns.GetHostName();   //获取本机名
            IPHostEntry localhost = Dns.GetHostByName(hostName);    //方法已过期，可以获取IPv4的地址
            //IPHostEntry localhost = Dns.GetHostEntry(hostName);   //获取IPv6地址
            IPAddress localaddr = localhost.AddressList[0];

            return localaddr.ToString();
        }
        /// <summary>
        /// 从缓存表批量新增数据到原始表
        /// </summary>
        /// <param name="BufferTable">缓存表</param>
        /// <param name="OriginalTable">原始表</param>
        /// <param name="IDLog">缓存表ID记录</param>
        public void AddOriginalData(string BufferTable, string OriginalTable, long IDLog, long IDLogFlag, int[] PointIds)
        {
            try
            {
                string pointStr = string.Join(",", PointIds);
                string sql = string.Format(@"  insert into {0}(PointId,Tstamp,ReceiveTime,Status,DataFlag,AuditFlag,PollutantCode,PollutantValue,MonitoringDataTypeCode,CreatUser,CreatDateTime)
  select PointId,Tstamp,GETDATE(),Status,DataFlag,AuditFlag,PollutantCode,PollutantValue,MonitoringDataTypeCode,'SystemSync',GETDATE() 
  from {1} a
  where id>{2} and id<={3}  and PointId in({4})
  order by Tstamp", OriginalTable, BufferTable, IDLog, IDLogFlag, pointStr);
                g_DatabaseHelper.ExecuteInsert(sql, AirAutoMonitorConnection);

            }
            catch (Exception ex)
            {
                log.Error("----------------AddOriginalData数据库处理方法异常：" + ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 生成原始日数据
        /// </summary>
        /// <param name="sTime">开始时间</param>
        /// <param name="eTime">结束时间</param>
        /// <param name="PointId">站点id</param>
        public void AddOriginalDayData(DateTime sTime, DateTime eTime, int PointId)
        {
            try
            {
                string StartTime = sTime.ToString("yyyy-MM-dd HH:mm:ss");
                string EndTime = eTime.ToString("yyyy-MM-dd HH:mm:ss");

                string sql = string.Format(@"
  --更新日数据
	UPDATE dayData 
	SET dayData.PollutantValue=hourData.PollutantValue
		,dayData.UpdateDateTime=GETDATE()
	FROM [dbo].[SY_Air_InfectantByDay] AS dayData
	LEFT JOIN
	(
		SELECT PointId
			,CONVERT(DATETIME,CONVERT(NVARCHAR(10),Tstamp,120),120) AS Tstamp
			,HourReport.PollutantCode
			,CASE WHEN (HourReport.PollutantCode IN ('a05024','a34004','a34002','a21026','a21005','a21004') AND COUNT(dbo.F_ValidValueByFlagNT(PollutantValue,[Status],','))<20)
				THEN NULL 
ELSE dbo.F_Round(AVG(dbo.F_ValidValueByFlagVOC(PollutantValue,[Status],',',CategoryUid,Tstamp,VOCType)),DecimalDigit) 
			 END AS PollutantValue
		FROM [dbo].[SY_Air_InfectantBy60] AS HourReport
		LEFT JOIN 
		(
			SELECT DISTINCT PollutantCode
				,CASE when DecimalDigit IS NULL THEN 3 ELSE DecimalDigit END AS DecimalDigit
                ,CategoryUid,VOCType
			FROM dbo.SY_PollutantCode
		) AS Pollutant
			ON HourReport.PollutantCode = Pollutant.PollutantCode
		WHERE HourReport.PointId={0} 
			AND HourReport.Tstamp>='{1}'
			AND HourReport.Tstamp<='{2}'
		GROUP BY PointId,CONVERT(DATETIME,CONVERT(NVARCHAR(10),Tstamp,120),120),HourReport.PollutantCode,DecimalDigit) AS hourData
		ON dayData.PointId = hourData.PointId AND dayData.[DateTime] = hourData.Tstamp AND dayData.PollutantCode = hourData.PollutantCode
	WHERE dayData.DateTime >= CONVERT(DATETIME,CONVERT(NVARCHAR(10),'{1}',120),120)
		AND dayData.DateTime <= CONVERT(DATETIME,CONVERT(NVARCHAR(10),'{2}',120),120)
		AND dayData.PointId = {0}
		AND hourData.PointId IS NOT NULL
		
	--插入日数据
	INSERT INTO [dbo].[SY_Air_InfectantByDay]
		([InfectantByDayUid]
		   ,PointId
		   ,DateTime
		   ,DayOfYear
		   ,PollutantCode
		   ,PollutantValue
		   ,CreatUser
		   ,CreatDateTime
		   ,UpdateUser
		   ,UpdateDateTime
		   )
	SELECT NEWID()
		,hourData.PointId
		,hourData.Tstamp
		,DATEPART(day,hourData.Tstamp)
		,hourData.PollutantCode
		,hourData.PollutantValue
		,'SystemSync'
		,GETDATE()
		,'SystemSync'
		,GETDATE()
	FROM (
		SELECT PointId
			,CONVERT(DATETIME,CONVERT(NVARCHAR(10),Tstamp,120),120) AS Tstamp
			,HourReport.PollutantCode
			,CASE WHEN (HourReport.PollutantCode IN ('a05024','a34004','a34002','a21026','a21005','a21004') AND COUNT(dbo.F_ValidValueByFlagNT(PollutantValue,[Status],','))<20)
				THEN NULL 
ELSE dbo.F_Round(AVG(dbo.F_ValidValueByFlagVOC(PollutantValue,[Status],',',CategoryUid,Tstamp,VOCType)),DecimalDigit) 
			 END AS PollutantValue
		FROM [dbo].[SY_Air_InfectantBy60] AS HourReport
		LEFT JOIN 
		(
			SELECT DISTINCT PollutantCode
				,CASE when DecimalDigit IS NULL THEN 3 ELSE DecimalDigit END AS DecimalDigit
                ,CategoryUid,VOCType
			FROM dbo.SY_PollutantCode
		) AS Pollutant
			ON HourReport.PollutantCode = Pollutant.PollutantCode
		WHERE HourReport.PointId={0}
			AND HourReport.Tstamp>='{1}'
			AND HourReport.Tstamp<='{2}'
		GROUP BY PointId,CONVERT(DATETIME,CONVERT(NVARCHAR(10),Tstamp,120),120),HourReport.PollutantCode,DecimalDigit
	) AS hourData
	LEFT JOIN [dbo].[SY_Air_InfectantByDay] AS dayData
		ON hourData.PointId = dayData.PointId AND hourData.Tstamp = dayData.[DateTime] AND hourData.PollutantCode = dayData.PollutantCode 
	WHERE hourData.Tstamp >= CONVERT(DATETIME,CONVERT(NVARCHAR(10),'{1}',120),120)
		AND hourData.Tstamp <= CONVERT(DATETIME,CONVERT(NVARCHAR(10),'{2}',120),120)
		AND hourData.PointId = {0}
		AND dayData.[InfectantByDayUid] IS NULL", PointId, StartTime, EndTime);
                g_DatabaseHelper.ExecuteScalar(sql, MonitoringBusinessConnection);
            }
            catch (Exception ex)
            {
                log.Error("----------------AddOriginalDayData数据库处理方法异常：" + ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 生成原始月数据
        /// </summary>
        /// <param name="sTime">开始时间</param>
        /// <param name="eTime">结束时间</param>
        /// <param name="PointId">站点id</param>
        public void AddOriginalMonthData(DateTime sTime, DateTime eTime, int PointId)
        {
            try
            {
                string StartTime = sTime.ToString("yyyy-MM-dd HH:mm:ss");
                string EndTime = eTime.ToString("yyyy-MM-dd HH:mm:ss");

                string sql = string.Format(@" 
    --更新月数据
	UPDATE monData 
	SET monData.PollutantValue=dayData.PollutantValue
		,monData.UpdateDateTime = GETDATE()
	FROM [dbo].[SY_Air_InfectantByMonth] AS monData
	LEFT JOIN
	(
		SELECT PointId
			,CONVERT(DATETIME,CONVERT(NVARCHAR(7),[DateTime],120)+'-01',120) AS [DateTime]
			,dayReport.PollutantCode
			,dbo.F_Round(AVG(PollutantValue),DecimalDigit) AS PollutantValue
		FROM [dbo].[SY_Air_InfectantByDay]AS dayReport
		LEFT JOIN 
		(
			SELECT DISTINCT PollutantCode
				,CASE when DecimalDigit IS NULL THEN 3 ELSE DecimalDigit END AS DecimalDigit
			FROM dbo.SY_PollutantCode
		) AS Pollutant
			ON dayReport.PollutantCode = Pollutant.PollutantCode
		WHERE dayReport.PointId={0} 
			AND dayReport.[DateTime]>=CONVERT(DATETIME,CONVERT(NVARCHAR(10),'{1}',120),120)
			AND dayReport.[DateTime]<=CONVERT(DATETIME,CONVERT(NVARCHAR(10),'{2}',120),120)
		GROUP BY PointId,CONVERT(DATETIME,CONVERT(NVARCHAR(7),[DateTime],120)+'-01',120),dayReport.PollutantCode,DecimalDigit
	) AS dayData
		ON monData.PointId = dayData.PointId AND monData.[DateTime] = dayData.[DateTime] AND monData.PollutantCode = dayData.PollutantCode
	WHERE monData.[DateTime] >= CONVERT(DATETIME,CONVERT(NVARCHAR(7),'{1}',120)+'-01',120)
		AND monData.[DateTime] <= CONVERT(DATETIME,CONVERT(NVARCHAR(7),'{2}',120)+'-01',120)
		AND monData.PointId = {0}
		AND dayData.PointId IS NOT NULL

	--插入月数据
	INSERT INTO [dbo].[SY_Air_InfectantByMonth]
		([InfectantByMonUid]
		   ,PointId
		   ,[DateTime]
		   ,[Year]
		   ,[MonthOfYear]
		   ,PollutantCode
		   ,PollutantValue
		   ,CreatUser
		   ,CreatDateTime
		   ,UpdateUser
		   ,UpdateDateTime)
	SELECT NEWID()
		,dayData.PointId
		,dayData.[DateTime]
		,DATEPART(year,dayData.[DateTime])
		,DATEPART(month,dayData.[DateTime])
		,dayData.PollutantCode
		,dayData.PollutantValue
		,'SystemSync'
		,GETDATE()
		,'SystemSync'
		,GETDATE()
	FROM (
		SELECT PointId
			,CONVERT(DATETIME,CONVERT(NVARCHAR(7),[DateTime],120)+'-01',120) AS [DateTime]
			,dayReport.PollutantCode
			,dbo.F_Round(AVG(PollutantValue),DecimalDigit) AS PollutantValue
		FROM [dbo].[SY_Air_InfectantByDay]AS dayReport
		LEFT JOIN 
		(
			SELECT DISTINCT PollutantCode
				,CASE when DecimalDigit IS NULL THEN 3 ELSE DecimalDigit END AS DecimalDigit
			FROM dbo.SY_PollutantCode
		) AS Pollutant
			ON dayReport.PollutantCode = Pollutant.PollutantCode
		WHERE dayReport.PointId={0} 
			AND dayReport.[DateTime]>=CONVERT(DATETIME,CONVERT(NVARCHAR(10),'{1}',120),120)
			AND dayReport.[DateTime]<=CONVERT(DATETIME,CONVERT(NVARCHAR(10),'{2}',120),120)
		GROUP BY PointId,CONVERT(DATETIME,CONVERT(NVARCHAR(7),[DateTime],120)+'-01',120),dayReport.PollutantCode,DecimalDigit
	) AS dayData
	LEFT JOIN [dbo].[SY_Air_InfectantByMonth] AS monData
		ON dayData.PointId = monData.PointId AND dayData.[DateTime] = monData.[DateTime] AND dayData.PollutantCode = monData.PollutantCode 
	WHERE dayData.[DateTime] >= CONVERT(DATETIME,CONVERT(NVARCHAR(7),'{1}',120)+'-01',120)
		AND dayData.[DateTime] <= CONVERT(DATETIME,CONVERT(NVARCHAR(7),'{2}',120)+'-01',120)
		AND dayData.PointId = {0}
		AND monData.[InfectantByMonUid] IS NULL", PointId, StartTime, EndTime);
                g_DatabaseHelper.ExecuteInsert(sql, MonitoringBusinessConnection);
            }
            catch (Exception ex)
            {
                log.Error("----------------AddOriginalMonthData数据库处理方法异常：" + ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 获取空气质量六参数行转列【原始小时】数据（测点）
        /// </summary>
        /// <param name="sTime"></param>
        /// <param name="eTime"></param>
        /// <returns></returns>
        public DataTable GetPivotOriHourData(int PointId, DateTime sTime, DateTime eTime)
        {
            try
            {
                string sql = string.Format(@"SELECT PointId
			,Tstamp
			,MAX(CASE WHEN PollutantCode='a21026' THEN [dbo].[F_Round](dbo.F_ValidValueByFlagNT(PollutantValue,[Status],','),3) END) AS 'SO2'
			,MAX(CASE WHEN PollutantCode='a21004' THEN [dbo].[F_Round](dbo.F_ValidValueByFlagNT(PollutantValue,[Status],','),3) END) AS 'NO2'
			,MAX(CASE WHEN PollutantCode='a34002' THEN [dbo].[F_Round](dbo.F_ValidValueByFlagNT(PollutantValue,[Status],','),3) END) AS 'PM10'
			,MAX(CASE WHEN PollutantCode='a34002' THEN [dbo].[F_Round](dbo.F_GetAirMovingAvg('O',PointId,'a34002',Tstamp,24,1,0),3) END) AS 'Recent24HoursPM10'
			,MAX(CASE WHEN PollutantCode='a21005' THEN [dbo].[F_Round](dbo.F_ValidValueByFlagNT(PollutantValue,[Status],','),1) END) AS 'CO'
			,MAX(CASE WHEN PollutantCode='a05024' THEN [dbo].[F_Round](dbo.F_ValidValueByFlagNT(PollutantValue,[Status],','),3) END) AS 'O3'
			,MAX(CASE WHEN PollutantCode='a05024' THEN [dbo].[F_Round](dbo.F_GetAirMovingAvg('O',PointId,'a05024',Tstamp,8,0,1),3) END) AS 'Recent8HoursO3'
            ,MAX(CASE WHEN PollutantCode='a05024' THEN [dbo].[F_Round](dbo.F_GetAirMovingAvg('O',PointId,'a05024',Tstamp,8,1,1),3) END) AS 'Recent8HoursO3NT'
			,MAX(CASE WHEN PollutantCode='a34004' THEN [dbo].[F_Round](dbo.F_ValidValueByFlagNT(PollutantValue,[Status],','),3) END) AS 'PM25'
			,MAX(CASE WHEN PollutantCode='a34004' THEN [dbo].[F_Round](dbo.F_GetAirMovingAvg('O',PointId,'a34004',Tstamp,24,1,0),3) END) AS 'Recent24HoursPM25'
		FROM dbo.SY_Air_InfectantBy60
		WHERE 1=1
		AND PointId = {0} 
		AND [Tstamp]>= '{1}'
			AND [Tstamp]<='{2}'
		and PollutantCode in ('a21026','a21004','a34002','a21005','a05024','a34004')
		GROUP BY PointId,Tstamp", PointId, sTime.ToString("yyyy-MM-dd HH:mm:ss"), eTime.ToString("yyyy-MM-dd HH:mm:ss"));
                return g_DatabaseHelper.ExecuteDataTable(sql, MonitoringBusinessConnection);

            }
            catch (Exception ex)
            {
                log.Error("----------------GetPivotOriHourData数据库处理方法异常：" + ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        /// 获取空气质量六参数行转列【审核小时】数据（测点）
        /// </summary>
        /// <param name="PointId"></param>
        /// <param name="sTime"></param>
        /// <param name="eTime"></param>
        /// <returns></returns>
        public DataTable GetPivotAudHourData(int PointId, DateTime sTime, DateTime eTime)
        {
            try
            {
                string sql = string.Format(@"SELECT PointId
			,Tstamp
			,MAX(CASE WHEN PollutantCode='a21026' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,[AuditFlag],','),3) END) AS 'SO2'
			,MAX(CASE WHEN PollutantCode='a21004' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,[AuditFlag],','),3) END) AS 'NO2'
			,MAX(CASE WHEN PollutantCode='a34002' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,[AuditFlag],','),3) END) AS 'PM10'
			,MAX(CASE WHEN PollutantCode='a34002' THEN [dbo].[F_Round](dbo.F_GetAirMovingAvg('A',PointId,'a34002',Tstamp,24,1,0),3) END) AS 'Recent24HoursPM10'
			,MAX(CASE WHEN PollutantCode='a21005' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,[AuditFlag],','),1) END) AS 'CO'
			,MAX(CASE WHEN PollutantCode='a05024' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,[AuditFlag],','),3) END) AS 'O3'
			,MAX(CASE WHEN PollutantCode='a05024' THEN [dbo].[F_Round](dbo.F_GetAirMovingAvg('A',PointId,'a05024',Tstamp,8,0,1),3) END) AS 'Recent8HoursO3'
            ,MAX(CASE WHEN PollutantCode='a05024' THEN [dbo].[F_Round](dbo.F_GetAirMovingAvg('A',PointId,'a05024',Tstamp,8,1,1),3) END) AS 'Recent8HoursO3NT'
			,MAX(CASE WHEN PollutantCode='a34004' THEN [dbo].[F_Round](dbo.F_ValidValueByFlag(PollutantValue,[AuditFlag],','),3) END) AS 'PM25'
			,MAX(CASE WHEN PollutantCode='a34004' THEN [dbo].[F_Round](dbo.F_GetAirMovingAvg('A',PointId,'a34004',Tstamp,24,1,0),3) END) AS 'Recent24HoursPM25'
		FROM [AirReport].[TB_HourReport_Calculate]
		WHERE 1=1
		AND PointId = {0} 
		AND [Tstamp]>= '{1}'
			AND [Tstamp]<='{2}'
		and PollutantCode in ('a21026','a21004','a34002','a21005','a05024','a34004')
		GROUP BY PointId,Tstamp", PointId, sTime.ToString("yyyy-MM-dd HH:mm:ss"), eTime.ToString("yyyy-MM-dd HH:mm:ss"));
                return g_DatabaseHelper.ExecuteDataTable(sql, MonitoringBusinessConnection);

            }
            catch (Exception ex)
            {
                log.Error("----------------GetPivotAudHourData数据库处理方法异常：" + ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 获取空气质量六参数行转列【原始小时】数据(区域)
        /// </summary>
        /// <param name="RegionUid"></param>
        /// <param name="sTime"></param>
        /// <param name="eTime"></param>
        /// <returns></returns>
        public DataTable GetPivotOriRegionHourData(string RegionUid, DateTime sTime, DateTime eTime)
        {
            try
            {
                string sql = string.Format(@"SELECT CityTypeUid
			,[DateTime]
			,Convert(numeric(18,3),[dbo].[F_Round]([dbo].SY_F_GetStatisticalValue(smp.CityTypeUid,SUM(CASE WHEN [SO2] IS not NULL and [SO2] !='' THEN 1 ELSE 0 END),AVG(dbo.F_ValidValueStr([SO2]))),3)) AS [SO2]
			, Convert(numeric(18,3),[dbo].[F_Round]([dbo].SY_F_GetStatisticalValue(smp.CityTypeUid,SUM(CASE WHEN [NO2] IS not NULL and [NO2] !='' THEN 1 ELSE 0 END),AVG(dbo.F_ValidValueStr([NO2]))),3)) AS [NO2]
			, Convert(numeric(18,3),[dbo].[F_Round]([dbo].SY_F_GetStatisticalValue(smp.CityTypeUid,SUM(CASE WHEN [PM10] IS not NULL and [PM10] !='' THEN 1 ELSE 0 END),AVG(dbo.F_ValidValueStr([PM10]))),3)) AS [PM10]
			, Convert(numeric(18,3),[dbo].[F_Round]([dbo].SY_F_GetStatisticalValue(smp.CityTypeUid,SUM(CASE WHEN [Recent24HoursPM10] IS not NULL and [Recent24HoursPM10] !='' THEN 1 ELSE 0 END),AVG(dbo.F_ValidValueStr([Recent24HoursPM10]))),3)) AS [Recent24HoursPM10]
			, Convert(numeric(18,1),[dbo].[F_Round]([dbo].SY_F_GetStatisticalValue(smp.CityTypeUid,SUM(CASE WHEN [CO] IS not NULL and [CO] !='' THEN 1 ELSE 0 END),AVG(dbo.F_ValidValueStr([CO]))),1)) AS [CO]
			, Convert(numeric(18,3), [dbo].[F_Round]([dbo].SY_F_GetStatisticalValue(smp.CityTypeUid,SUM(CASE WHEN [O3] IS not NULL and [O3] !='' THEN 1 ELSE 0 END),AVG(dbo.F_ValidValueStr([O3]))),3)) AS [O3]
			, Convert(numeric(18,3), [dbo].[F_Round]([dbo].SY_F_GetStatisticalValue(smp.CityTypeUid,SUM(CASE WHEN [Recent8HoursO3] IS not NULL and [Recent8HoursO3] !='' THEN 1 ELSE 0 END),AVG(dbo.F_ValidValueStr([Recent8HoursO3]))),3)) AS [Recent8HoursO3]
            , Convert(numeric(18,3), [dbo].[F_Round]([dbo].SY_F_GetStatisticalValue(smp.CityTypeUid,SUM(CASE WHEN [Recent8HoursO3NT] IS not NULL and [Recent8HoursO3NT] !='' THEN 1 ELSE 0 END),AVG(dbo.F_ValidValueStr([Recent8HoursO3NT]))),3)) AS [Recent8HoursO3NT]
			, Convert(numeric(18,3), [dbo].[F_Round]([dbo].SY_F_GetStatisticalValue(smp.CityTypeUid,SUM(CASE WHEN [PM25] IS not NULL and [PM25] !='' THEN 1 ELSE 0 END),AVG(dbo.F_ValidValueStr([PM25]))),3)) AS [PM25]
			, Convert(numeric(18,3), [dbo].[F_Round]([dbo].SY_F_GetStatisticalValue(smp.CityTypeUid,SUM(CASE WHEN [Recent24HoursPM25] IS not NULL and [Recent24HoursPM25] !='' THEN 1 ELSE 0 END),AVG(dbo.F_ValidValueStr([Recent24HoursPM25]))),3)) AS [Recent24HoursPM25]
		FROM Air.TB_OriHourAQI AS hourData
		LEFT JOIN dbo.SY_V_Point_Air AS smp
			ON hourData.PointId = smp.PointId
		WHERE hourData.[DateTime] >= '{1}' 
			AND hourData.[DateTime] <= '{2}' 
			AND smp.CityTypeUid = '{0}'
			AND smp.CalAQIOrNot = 1
		GROUP BY smp.CityTypeUid,[DateTime]", RegionUid, sTime.ToString("yyyy-MM-dd HH:mm:ss"), eTime.ToString("yyyy-MM-dd HH:mm:ss"));
                return g_DatabaseHelper.ExecuteDataTable(sql, AirAutoMonitorConnection);

            }
            catch (Exception ex)
            {
                log.Error("----------------GetPivotOriRegionHourData数据库处理方法异常：" + ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 获取空气质量六参数行转列【审核小时】数据(区域)
        /// </summary>
        /// <param name="RegionUid"></param>
        /// <param name="sTime"></param>
        /// <param name="eTime"></param>
        /// <returns></returns>
        public DataTable GetPivotAudRegionHourData(string RegionUid, DateTime sTime, DateTime eTime)
        {
            try
            {
                string sql = string.Format(@"SELECT CityTypeUid
			,[DateTime]
			,Convert(numeric(18,3),[dbo].[F_Round]([dbo].F_GetStatisticalValue(smp.CityTypeUid,SUM(CASE WHEN [SO2] IS not NULL and [SO2] !='' THEN 1 ELSE 0 END),AVG(dbo.F_ValidValueStr([SO2]))),3)) AS [SO2]
			, Convert(numeric(18,3),[dbo].[F_Round]([dbo].F_GetStatisticalValue(smp.CityTypeUid,SUM(CASE WHEN [NO2] IS not NULL and [NO2] !='' THEN 1 ELSE 0 END),AVG(dbo.F_ValidValueStr([NO2]))),3)) AS [NO2]
			, Convert(numeric(18,3),[dbo].[F_Round]([dbo].F_GetStatisticalValue(smp.CityTypeUid,SUM(CASE WHEN [PM10] IS not NULL and [PM10] !='' THEN 1 ELSE 0 END),AVG(dbo.F_ValidValueStr([PM10]))),3)) AS [PM10]
			, Convert(numeric(18,3),[dbo].[F_Round]([dbo].F_GetStatisticalValue(smp.CityTypeUid,SUM(CASE WHEN [Recent24HoursPM10] IS not NULL and [Recent24HoursPM10] !='' THEN 1 ELSE 0 END),AVG(dbo.F_ValidValueStr([Recent24HoursPM10]))),3)) AS [Recent24HoursPM10]
			, Convert(numeric(18,1),[dbo].[F_Round]([dbo].F_GetStatisticalValue(smp.CityTypeUid,SUM(CASE WHEN [CO] IS not NULL and [CO] !='' THEN 1 ELSE 0 END),AVG(dbo.F_ValidValueStr([CO]))),1)) AS [CO]
			, Convert(numeric(18,3), [dbo].[F_Round]([dbo].F_GetStatisticalValue(smp.CityTypeUid,SUM(CASE WHEN [O3] IS not NULL and [O3] !='' THEN 1 ELSE 0 END),AVG(dbo.F_ValidValueStr([O3]))),3)) AS [O3]
			, Convert(numeric(18,3), [dbo].[F_Round]([dbo].F_GetStatisticalValue(smp.CityTypeUid,SUM(CASE WHEN [Recent8HoursO3] IS not NULL and [Recent8HoursO3] !='' THEN 1 ELSE 0 END),AVG(dbo.F_ValidValueStr([Recent8HoursO3]))),3)) AS [Recent8HoursO3]
            , Convert(numeric(18,3), [dbo].[F_Round]([dbo].F_GetStatisticalValue(smp.CityTypeUid,SUM(CASE WHEN [Recent8HoursO3NT] IS not NULL and [Recent8HoursO3NT] !='' THEN 1 ELSE 0 END),AVG(dbo.F_ValidValueStr([Recent8HoursO3NT]))),3)) AS [Recent8HoursO3NT]
			, Convert(numeric(18,3), [dbo].[F_Round]([dbo].F_GetStatisticalValue(smp.CityTypeUid,SUM(CASE WHEN [PM25] IS not NULL and [PM25] !='' THEN 1 ELSE 0 END),AVG(dbo.F_ValidValueStr([PM25]))),3)) AS [PM25]
			, Convert(numeric(18,3), [dbo].[F_Round]([dbo].F_GetStatisticalValue(smp.CityTypeUid,SUM(CASE WHEN [Recent24HoursPM25] IS not NULL and [Recent24HoursPM25] !='' THEN 1 ELSE 0 END),AVG(dbo.F_ValidValueStr([Recent24HoursPM25]))),3)) AS [Recent24HoursPM25]
		FROM AirRelease.TB_HourAQI AS hourData
		LEFT JOIN dbo.SY_V_Point_Air AS smp
			ON hourData.PointId = smp.PointId
		WHERE hourData.[DateTime] >= '{1}' 
			AND hourData.[DateTime] <= '{2}' 
			AND smp.CityTypeUid = '{0}'
			AND smp.CalAQIOrNot = 1
		GROUP BY smp.CityTypeUid,[DateTime]", RegionUid, sTime.ToString("yyyy-MM-dd HH:mm:ss"), eTime.ToString("yyyy-MM-dd HH:mm:ss"));
                return g_DatabaseHelper.ExecuteDataTable(sql, MonitoringBusinessConnection);

            }
            catch (Exception ex)
            {
                log.Error("----------------GetPivotAudRegionHourData数据库处理方法异常：" + ex.ToString());
                throw ex;
            }
        }


        /// <summary>
        /// 获取空气质量六参数行转列日数据【原始】（测点）
        /// </summary>
        /// <param name="PointId"></param>
        /// <param name="sTime"></param>
        /// <param name="eTime"></param>
        /// <returns></returns>
        public DataTable GetPivotOriDayData(int PointId, DateTime sTime, DateTime eTime)
        {
            try
            {
                string sql = string.Format(@"SELECT PointId
			,CONVERT(DATETIME,CONVERT(NVARCHAR(10),[DateTime],120),120) AS [DateTime]
			,CASE WHEN COUNT(dbo.F_ValidValueStr([SO2]))>=20 THEN [dbo].[F_Round](AVG(dbo.F_ValidValueStr([SO2])),3) ELSE NULL END AS [SO2]
			,CASE WHEN COUNT(dbo.F_ValidValueStr([NO2]))>=20 THEN [dbo].[F_Round](AVG(dbo.F_ValidValueStr([NO2])),3) ELSE NULL END AS [NO2]
			,CASE WHEN COUNT(dbo.F_ValidValueStr([PM10]))>=20 THEN [dbo].[F_Round](AVG(dbo.F_ValidValueStr([PM10])),3) ELSE NULL END AS [PM10]
			,CASE WHEN COUNT(dbo.F_ValidValueStr([CO]))>=20 THEN [dbo].[F_Round](AVG(dbo.F_ValidValueStr([CO])),1) ELSE NULL END AS [CO]
			,CASE WHEN COUNT(dbo.F_ValidValueStr([O3]))>=20 THEN [dbo].[F_Round](MAX(dbo.F_ValidValueStr([O3])),3) ELSE NULL END AS [MaxOneHourO3]
			,CASE WHEN (COUNT(dbo.F_ValidValueStr(Recent8HoursO3))>=14 OR [dbo].[F_Round](MAX(dbo.F_ValidValueStr(Recent8HoursO3)),3)>0.160) THEN [dbo].[F_Round](MAX(dbo.F_ValidValueStr(Recent8HoursO3)),3) ELSE NULL END AS [Max8HourO3]
			,CASE WHEN COUNT(dbo.F_ValidValueStr([PM25]))>=20 THEN[dbo].[F_Round](AVG(dbo.F_ValidValueStr([PM25])),3) ELSE NULL END AS [PM25]
		FROM [dbo].[SY_OriHourAQI] 
		WHERE PointId = {0}
			AND [DateTime]>= '{1}'
			AND [DateTime]<='{2}'
		GROUP BY PointId,CONVERT(DATETIME,CONVERT(NVARCHAR(10),[DateTime],120),120)", PointId, sTime.ToString("yyyy-MM-dd HH:mm:ss"), eTime.ToString("yyyy-MM-dd HH:mm:ss"));
                return g_DatabaseHelper.ExecuteDataTable(sql, MonitoringBusinessConnection);

            }
            catch (Exception ex)
            {
                log.Error("----------------GetPivotDayData数据库处理方法异常：" + ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 获取空气质量六参数行转列日数据【审核】（测点）
        /// </summary>
        /// <param name="PointId"></param>
        /// <param name="sTime"></param>
        /// <param name="eTime"></param>
        /// <returns></returns>
        public DataTable GetPivotAudDayData(int PointId, DateTime sTime, DateTime eTime)
        {
            try
            {
                string sql = string.Format(@"SELECT PointId
			,CONVERT(DATETIME,CONVERT(NVARCHAR(10),[DateTime],120),120) AS [DateTime]
			,CASE WHEN COUNT(dbo.F_ValidValueStr([SO2]))>=20 THEN [dbo].[F_Round](AVG(dbo.F_ValidValueStr([SO2])),3) ELSE NULL END AS [SO2]
			,CASE WHEN COUNT(dbo.F_ValidValueStr([NO2]))>=20 THEN [dbo].[F_Round](AVG(dbo.F_ValidValueStr([NO2])),3) ELSE NULL END AS [NO2]
			,CASE WHEN COUNT(dbo.F_ValidValueStr([PM10]))>=20 THEN [dbo].[F_Round](AVG(dbo.F_ValidValueStr([PM10])),3) ELSE NULL END AS [PM10]
			,CASE WHEN COUNT(dbo.F_ValidValueStr([CO]))>=20 THEN [dbo].[F_Round](AVG(dbo.F_ValidValueStr([CO])),1) ELSE NULL END AS [CO]
			,CASE WHEN COUNT(dbo.F_ValidValueStr([O3]))>=20 THEN [dbo].[F_Round](MAX(dbo.F_ValidValueStr([O3])),3) ELSE NULL END AS [MaxOneHourO3]
			,CASE WHEN (COUNT(dbo.F_ValidValueStr(Recent8HoursO3))>=14 OR [dbo].[F_Round](MAX(dbo.F_ValidValueStr(Recent8HoursO3)),3)>0.160) THEN [dbo].[F_Round](MAX(dbo.F_ValidValueStr(Recent8HoursO3)),3) ELSE NULL END AS [Max8HourO3]
			,CASE WHEN COUNT(dbo.F_ValidValueStr([PM25]))>=20 THEN[dbo].[F_Round](AVG(dbo.F_ValidValueStr([PM25])),3) ELSE NULL END AS [PM25]
		FROM AirRelease.TB_HourAQI  
		WHERE PointId = {0}
			AND [DateTime]>= '{1}'
			AND [DateTime]<='{2}'
		GROUP BY PointId,CONVERT(DATETIME,CONVERT(NVARCHAR(10),[DateTime],120),120)", PointId, sTime.ToString("yyyy-MM-dd HH:mm:ss"), eTime.ToString("yyyy-MM-dd HH:mm:ss"));
                return g_DatabaseHelper.ExecuteDataTable(sql, MonitoringBusinessConnection);

            }
            catch (Exception ex)
            {
                log.Error("----------------GetPivotDayData数据库处理方法异常：" + ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 获取空气质量六参数行转列【原始日】数据(区域)
        /// </summary>
        /// <param name="RegionUid"></param>
        /// <param name="sTime"></param>
        /// <param name="eTime"></param>
        /// <returns></returns>
        public DataTable GetPivotOriRegionDayData(string RegionUid, DateTime sTime, DateTime eTime)
        {
            try
            {
                string sql = string.Format(@"SELECT CityTypeUid
			,[DateTime]
			,Convert(numeric(18,3),[dbo].[F_Round]([dbo].[SY_F_GetStatisticalValue_Day](smp.CityTypeUid,COUNT([SO2]),AVG(dbo.F_ValidValueStr([SO2])),MAX(dbo.F_ValidValueStr([SO2]))),3)) AS [SO2]
			, Convert(numeric(18,3),[dbo].[F_Round]([dbo].[SY_F_GetStatisticalValue_Day](smp.CityTypeUid,COUNT([NO2]),AVG(dbo.F_ValidValueStr([NO2])),MAX(dbo.F_ValidValueStr([NO2]))),3)) AS [NO2]
			, Convert(numeric(18,3),[dbo].[F_Round]([dbo].[SY_F_GetStatisticalValue_Day](smp.CityTypeUid,COUNT([PM10]),AVG(dbo.F_ValidValueStr([PM10])),MAX(dbo.F_ValidValueStr([PM10]))),3)) AS [PM10]
			, Convert(numeric(18,1),[dbo].[F_Round]([dbo].[SY_F_GetStatisticalValue_Day](smp.CityTypeUid,COUNT([CO]),AVG(dbo.F_ValidValueStr([CO])),MAX(dbo.F_ValidValueStr([CO]))),1)) AS [CO]
			, Convert(numeric(18,3), [dbo].[F_Round]([dbo].[SY_F_GetStatisticalValue_Day](smp.CityTypeUid,COUNT([MaxOneHourO3]),AVG(dbo.F_ValidValueStr([MaxOneHourO3])),MAX(dbo.F_ValidValueStr([MaxOneHourO3]))),3)) AS [MaxOneHourO3]
			, Convert(numeric(18,3), [dbo].[F_Round]([dbo].[SY_F_GetStatisticalValue_Day](smp.CityTypeUid,COUNT([Max8HourO3]),AVG(dbo.F_ValidValueStr([Max8HourO3])),MAX(dbo.F_ValidValueStr([Max8HourO3]))),3)) AS [Max8HourO3]
			, Convert(numeric(18,3), [dbo].[F_Round]([dbo].[SY_F_GetStatisticalValue_Day](smp.CityTypeUid,COUNT([PM25]),AVG(dbo.F_ValidValueStr([PM25])),MAX(dbo.F_ValidValueStr([PM25]))),3)) AS [PM25]
		FROM Air.TB_OriDayAQI AS hourData
		LEFT JOIN dbo.SY_V_Point_Air AS smp
			ON hourData.PointId = smp.PointId
		WHERE 1=1
		and hourData.[DateTime] >= '{1}' 
			AND hourData.[DateTime] <= '{2}' 
			AND smp.CityTypeUid = '{0}'
			AND smp.CalRegionAQIOrNot=1
		GROUP BY smp.CityTypeUid,smp.CityTypeUid,[DateTime]", RegionUid, sTime.ToString("yyyy-MM-dd HH:mm:ss"), eTime.ToString("yyyy-MM-dd HH:mm:ss"));
                return g_DatabaseHelper.ExecuteDataTable(sql, AirAutoMonitorConnection);

            }
            catch (Exception ex)
            {
                log.Error("----------------GetPivotOriRegionDayData数据库处理方法异常：" + ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 获取空气质量六参数行转列【审核日】数据(区域)
        /// </summary>
        /// <param name="RegionUid"></param>
        /// <param name="sTime"></param>
        /// <param name="eTime"></param>
        /// <returns></returns>
        public DataTable GetPivotAudRegionDayData(string RegionUid, DateTime sTime, DateTime eTime)
        {
            try
            {
                string sql = string.Format(@"SELECT CityTypeUid
			,[DateTime]
			,Convert(numeric(18,3),[dbo].[F_Round]([dbo].[F_GetStatisticalValue_Day](smp.CityTypeUid,COUNT([SO2]),AVG(dbo.F_ValidValueStr([SO2])),MAX(dbo.F_ValidValueStr([SO2]))),3)) AS [SO2]
			, Convert(numeric(18,3),[dbo].[F_Round]([dbo].[F_GetStatisticalValue_Day](smp.CityTypeUid,COUNT([NO2]),AVG(dbo.F_ValidValueStr([NO2])),MAX(dbo.F_ValidValueStr([NO2]))),3)) AS [NO2]
			, Convert(numeric(18,3),[dbo].[F_Round]([dbo].[F_GetStatisticalValue_Day](smp.CityTypeUid,COUNT([PM10]),AVG(dbo.F_ValidValueStr([PM10])),MAX(dbo.F_ValidValueStr([PM10]))),3)) AS [PM10]
			, Convert(numeric(18,1),[dbo].[F_Round]([dbo].[F_GetStatisticalValue_Day](smp.CityTypeUid,COUNT([CO]),AVG(dbo.F_ValidValueStr([CO])),MAX(dbo.F_ValidValueStr([CO]))),1)) AS [CO]
			, Convert(numeric(18,3), [dbo].[F_Round]([dbo].[F_GetStatisticalValue_Day](smp.CityTypeUid,COUNT([MaxOneHourO3]),AVG(dbo.F_ValidValueStr([MaxOneHourO3])),MAX(dbo.F_ValidValueStr([MaxOneHourO3]))),3)) AS [MaxOneHourO3]
			, Convert(numeric(18,3), [dbo].[F_Round]([dbo].[F_GetStatisticalValue_Day](smp.CityTypeUid,COUNT([Max8HourO3]),AVG(dbo.F_ValidValueStr([Max8HourO3])),MAX(dbo.F_ValidValueStr([Max8HourO3]))),3)) AS [Max8HourO3]
			, Convert(numeric(18,3), [dbo].[F_Round]([dbo].[F_GetStatisticalValue_Day](smp.CityTypeUid,COUNT([PM25]),AVG(dbo.F_ValidValueStr([PM25])),MAX(dbo.F_ValidValueStr([PM25]))),3)) AS [PM25]
		FROM AirRelease.TB_DayAQI AS hourData
		LEFT JOIN dbo.SY_V_Point_Air AS smp
			ON hourData.PointId = smp.PointId
		WHERE 1=1
		and hourData.[DateTime] >= '{1}'
			AND hourData.[DateTime] <= '{2}'
			AND smp.CityTypeUid = '{0}'
			AND smp.CalRegionAQIOrNot=1
		GROUP BY smp.CityTypeUid,smp.CityTypeUid,[DateTime]", RegionUid, sTime.ToString("yyyy-MM-dd HH:mm:ss"), eTime.ToString("yyyy-MM-dd HH:mm:ss"));
                return g_DatabaseHelper.ExecuteDataTable(sql, MonitoringBusinessConnection);

            }
            catch (Exception ex)
            {
                log.Error("----------------GetPivotAudRegionDayData数据库处理方法异常：" + ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        /// 加载预处理数据
        /// </summary>
        /// <param name="PointList">站点list,多个测点以逗号隔开</param>
        /// <param name="IDLog">批量处理id记录</param>
        /// <param name="IDLogFlag">自动标记id记录</param>
        /// <param name="ApplicationUid">应用程序类型</param>
        public void Load_AuditPreData_NotAudit(int[] PointIds, string ApplicationUid, long IDLog, long IDLogFlag)
        {
            try
            {
                string AuditHourTableName = string.Empty;
                string InfectantBy60TableName = string.Empty;
                if (ApplicationUid == "airaaira-aira-aira-aira-airaairaaira")
                {
                    AuditHourTableName = "[Audit].[TB_AuditAirInfectantByHour]";
                    InfectantBy60TableName = "[dbo].[SY_Air_InfectantBy60Buffer]";
                }
                string PointID_SQL = string.Empty;
                if (PointIds.Length > 0)
                {
                    PointID_SQL += string.Format("AND c.Pointid IN ({0})", string.Join(",", PointIds));
                }
                //判断临时表是否存在
                string sql = string.Format(@"if object_id('tempdb..#tempTable') is not null Begin drop table #tempTable End;");
                sql += string.Format(@"with PollutantInfo as
(
SELECT  C.pointid Pointid,a.PollutantCode,b.monitoringPointUid  FROM [Audit].[TB_AuditMonitoringPointPollutant] A join [Audit].[TB_AuditMonitoringPoint] B 
        ON a.[AuditMonitoringPointUid]=b.[AuditMonitoringPointUid] AND A.ApplicationUid=B.ApplicationUid JOIN [dbo].[SY_MonitoringPoint] C
        ON B.monitoringPointUid=C.monitoringPointUid AND B.ApplicationUid=C.ApplicationUid
        WHERE C.ApplicationUid='{0}'  {1} 
),DataInfo as
(
 SELECT   '{0}' ApplicationUid,A.[Pointid],A.[Tstamp],max(A.[AuditFlag]) [DataFlag]
         ,max(A.[Status]) [AuditFlag],A.[PollutantCode],max(A.[PollutantValue]) [PollutantValue],GETDATE() [CreatDateTime] 
 FROM {2}  as A  JOIN PollutantInfo  B   ON   A.Pointid=B.pointid AND A.PollutantCode=b.PollutantCode 
WHERE  id>{3} and id<={4}
group by A.[Pointid],A.[Tstamp],A.[PollutantCode]
)", ApplicationUid, PointID_SQL, InfectantBy60TableName, IDLog, IDLogFlag, AuditHourTableName);
                //将原始数据写入临时表
                sql += string.Format(@" SELECT NEWID() [AuditHourUid],*  INTO #tempTable FROM DataInfo");
                #region  导入审核日状态数据
                sql += string.Format(@" INSERT INTO [Audit].[TB_AuditStatusForDay](AuditStatusUid,ApplicationUid,PointId,[Date],[Status],CreatUser,CreatDateTime)
select  NEWID() AuditStatusUid,'{0}' ApplicationUid,PointID,CONVERT(VARCHAR(10),[Tstamp],120) [Tstamp],'0' [Status],'SystemSync' CreatUser,GETDATE() CreatDateTime from #tempTable T
where not exists(
	select * from [Audit].[TB_AuditStatusForDay] D
	where T.[PointId]=D.PointId and CONVERT(VARCHAR(10),[Tstamp],120)=D.Date
)
   Group by PointID,CONVERT(VARCHAR(10),[Tstamp],120)", ApplicationUid);
                #endregion

                #region 导入审核小时状态数据
                sql += string.Format(@"  INSERT INTO [Audit].[TB_AuditAirStatusForHour] ([AuditStatusUid] ,[ApplicationUid] ,[PointId] ,[PointIdType] ,[Date] ,[Status] ,[CreatUser] ,[CreatDateTime] ,[UpdateUser],[UpdateDateTime])  
  Select NEWID(),'{0}',T.PointId,'0',T.Tstamp,'0','SystemSync',GETDATE(),'SystemSync',GETDATE()
    from #tempTable T  
where not exists(
	select * from [Audit].[TB_AuditAirStatusForHour] H
	where T.[PointId]=H.PointId and T.[Tstamp]=H.Date
) 
GROUP by PointId,Tstamp", ApplicationUid);
                #endregion

                #region 导入审核小时数据
                sql += string.Format(@" INSERT INTO {0}  ([AuditHourUid],[AuditStatusUid], [ApplicationUid], [PointId], [DataDateTime],[dataFlag],[AuditFlag],[PollutantCode],[PollutantValue],[CreatUser],[CreatDateTime]) 
SELECT NEWID() [AuditHourUid],MAX(B.AuditStatusUid), MAX(A.ApplicationUid),B.[Pointid],A.[Tstamp],MAX(A.[DataFlag]),MAX([AuditFlag]),A.[PollutantCode],MAX(A.[PollutantValue]),'SystemSync',MAX(A.[CreatDateTime]) FROM 
  #tempTable A JOIN [Audit].[TB_AuditStatusForDay] B ON A.[Pointid]=B.PointId AND A.ApplicationUid=b.ApplicationUid AND CONVERT(VARCHAR(10),a.[Tstamp],120)=CONVERT(VARCHAR(10),b.[Date],120)
    GROUP BY   CONVERT(VARCHAR(10),b.[Date],120),B.[Pointid],A.[Tstamp],A.[PollutantCode]"
                    , AuditHourTableName);
                #endregion

                sql += string.Format(" if object_id('tempdb..#tempTable') is not null Begin drop table #tempTable End;");
                //log.Info("sql:" + sql );
                g_DatabaseHelper.ExecuteScalar(sql, MonitoringBusinessConnection);
            }
            catch (Exception ex)
            {
                log.Error("----------------Load_AuditPreData_NotAudit数据库处理方法异常：" + ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 加载超级站预处理数据
        /// </summary>
        /// <param name="PointIds"></param>
        /// <param name="ApplicationUid"></param>
        /// <param name="IDLog"></param>
        public void Load_AuditPreData_NotAudit(int[] PointIds, string ApplicationUid, long IDLog)
        {
            try
            {
                string AuditHourTableName = string.Empty;
                string InfectantBy60TableName = string.Empty;
                if (ApplicationUid == "airaaira-aira-aira-aira-airaairaaira")
                {
                    AuditHourTableName = "[Audit].[TB_AuditAirInfectantByHour]";
                    InfectantBy60TableName = "[dbo].[SY_Air_InfectantBy60]";
                }
                string PointID_SQL = string.Empty;
                if (PointIds.Length > 0)
                {
                    PointID_SQL += string.Format("AND c.Pointid IN ({0})", string.Join(",", PointIds));
                }
                //判断临时表是否存在
                string sql = string.Format(@"if object_id('tempdb..#tempTable') is not null Begin drop table #tempTable End;");
                sql += string.Format(@"with PollutantInfo as
(
SELECT  C.pointid Pointid,a.PollutantCode,b.monitoringPointUid  FROM [Audit].[TB_AuditMonitoringPointPollutant] A join [Audit].[TB_AuditMonitoringPoint] B 
        ON a.[AuditMonitoringPointUid]=b.[AuditMonitoringPointUid] AND A.ApplicationUid=B.ApplicationUid JOIN [dbo].[SY_MonitoringPoint] C
        ON B.monitoringPointUid=C.monitoringPointUid AND B.ApplicationUid=C.ApplicationUid
        WHERE C.ApplicationUid='{0}'  {1} 
),DataInfo as
(
 SELECT   '{0}' ApplicationUid,A.[Pointid],A.[Tstamp],max(A.[AuditFlag]) [DataFlag]
         ,max(A.[Status]) [AuditFlag],A.[PollutantCode],max(A.[PollutantValue]) [PollutantValue],GETDATE() [CreatDateTime] 
 FROM {2}  as A  JOIN PollutantInfo  B   ON   A.Pointid=B.pointid AND A.PollutantCode=b.PollutantCode 
WHERE  id>{3}
group by A.[Pointid],A.[Tstamp],A.[PollutantCode]
)", ApplicationUid, PointID_SQL, InfectantBy60TableName, IDLog);
                //将原始数据写入临时表
                sql += string.Format(@" SELECT NEWID() [AuditHourUid],*  INTO #tempTable FROM DataInfo");
                #region 导入审核日状态数据
                sql += string.Format(@" INSERT INTO [Audit].[TB_AuditStatusForDay](AuditStatusUid,ApplicationUid,PointId,[Date],[Status],CreatUser,CreatDateTime)
select  NEWID() AuditStatusUid,'{0}' ApplicationUid,PointID,CONVERT(VARCHAR(10),[Tstamp],120) [Tstamp],'0' [Status],'SystemSync' CreatUser,GETDATE() CreatDateTime from #tempTable T
where not exists(
	select * from [Audit].[TB_AuditStatusForDay] D
	where T.[PointId]=D.PointId and CONVERT(VARCHAR(10),[Tstamp],120)=D.Date
)
   Group by PointID,CONVERT(VARCHAR(10),[Tstamp],120)", ApplicationUid);
                #endregion

                #region 导入审核小时状态数据
                sql += string.Format(@"  INSERT INTO [Audit].[TB_AuditAirStatusForHour] ([AuditStatusUid] ,[ApplicationUid] ,[PointId] ,[PointIdType] ,[Date] ,[Status] ,[CreatUser] ,[CreatDateTime] ,[UpdateUser],[UpdateDateTime],[Description])  
  Select NEWID(),'{0}',T.PointId,'1',T.Tstamp,'0','SystemSync',GETDATE(),'SystemSync',GETDATE(),[AMS_BaseData].[InstrInfo].[TB_Instruments].RowGuid
    from #tempTable T ,[AMS_BaseData].[InstrInfo].[TB_Instruments] 
where [AMS_BaseData].[InstrInfo].[TB_Instruments].RowGuid in ('56dd6e9b-4c8f-4e67-a70f-b6a277cb44d7','6e4aa38a-f68b-490b-9cd7-3b92c7805c2d',
			'3745f768-a789-4d58-9578-9e41fde5e5f0','1589850e-0df1-4d9d-b508-4a77def158ba','a6b3d80c-8281-4bc6-af47-f0febf568a5c','da4f968f-cc6e-4fec-8219-6167d100499d','9ef57f3c-8cce-4fe3-980f-303bbcfde260','6cd5c158-8a79-4540-a8b1-2a062759c9a0') and	not exists(
	select * from [Audit].[TB_AuditAirStatusForHour] H
	where T.[PointId]=H.PointId and T.[Tstamp]=H.Date
) 
GROUP by PointId,Tstamp,[AMS_BaseData].[InstrInfo].[TB_Instruments].RowGuid ", ApplicationUid);
                #endregion

                #region 导入审核小时数据
                sql += string.Format(@" INSERT INTO {0}  ([AuditHourUid],[AuditStatusUid], [ApplicationUid], [PointId], [DataDateTime],[dataFlag],[AuditFlag],[PollutantCode],[PollutantValue],[CreatUser],[CreatDateTime]) 
SELECT NEWID() [AuditHourUid],MAX(B.AuditStatusUid), MAX(A.ApplicationUid),B.[Pointid],A.[Tstamp],MAX(A.[DataFlag]),MAX([AuditFlag]),A.[PollutantCode],MAX(A.[PollutantValue]),'SystemSync',MAX(A.[CreatDateTime]) FROM 
  #tempTable A JOIN [Audit].[TB_AuditStatusForDay] B ON A.[Pointid]=B.PointId AND A.ApplicationUid=b.ApplicationUid AND CONVERT(VARCHAR(10),a.[Tstamp],120)=CONVERT(VARCHAR(10),b.[Date],120)
    GROUP BY   CONVERT(VARCHAR(10),b.[Date],120),B.[Pointid],A.[Tstamp],A.[PollutantCode]"
                    , AuditHourTableName);
                #endregion



                sql += string.Format(" if object_id('tempdb..#tempTable') is not null Begin drop table #tempTable End;");
                //log.Info("----------"+sql);
                g_DatabaseHelper.ExecuteScalar(sql, MonitoringBusinessConnection);
            }
            catch (Exception ex)
            {
                log.Error("----------------Load_AuditPreData_NotAudit数据库处理方法异常：" + ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 批量生成小时计算表数据（用于常规站，需把status字段值放到AuditFlag中）
        /// </summary>
        /// <param name="IDLog">批量处理id记录</param>
        /// <param name="IDLogFlag">自动标记id记录</param>
        public void AddCommonAirReport_Hour_Mul(long IDLog, long IDLogFlag, int[] PointIds, string TableName)
        {
            try
            {
                string pointStr = string.Join(",", PointIds);
                string sql = string.Format(@" insert into {3}([HourReportUid],PointId,[ReportDateTime],Tstamp,[HourOfDay],PollutantCode,PollutantValue,dataFlag,AuditFlag,CreatUser,CreatDateTime)
  select NEWID(), PointId,GETDATE(),Tstamp,DATEPART(HOUR,Tstamp),PollutantCode,PollutantValue,AuditFlag,Status,'SystemSync',GETDATE() 
  from [dbo].[SY_Air_InfectantBy60Buffer] a
  where id>{0} and id<={1} and PointId in({2})
  order by Tstamp", IDLog, IDLogFlag, pointStr, TableName);
                //log.Info("--------sql:" + sql);
                g_DatabaseHelper.ExecuteInsert(sql, MonitoringBusinessConnection);

            }
            catch (Exception ex)
            {
                log.Error("----------------AddAirReport_Hour_Calculate_Mul数据库处理方法异常：" + ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        /// 批量生成小时计算表数据
        /// </summary>
        /// <param name="IDLog">批量处理id记录</param>
        /// <param name="IDLogFlag">自动标记id记录</param>
        public void AddAirReport_Hour_Mul(long IDLog, long IDLogFlag, int[] PointIds, string TableName)
        {
            try
            {
                string pointStr = string.Join(",", PointIds);
                string sql = string.Format(@" insert into {3}([HourReportUid],PointId,[ReportDateTime],Tstamp,[HourOfDay],PollutantCode,PollutantValue,dataFlag,CreatUser,CreatDateTime)
  select NEWID(), PointId,GETDATE(),Tstamp,DATEPART(HOUR,Tstamp),PollutantCode,PollutantValue,AuditFlag,'SystemSync',GETDATE() 
  from [dbo].[SY_Air_InfectantBy60Buffer] a
  where id>{0} and id<={1} and PointId in({2})
  order by Tstamp", IDLog, IDLogFlag, pointStr, TableName);
                //log.Info("--------sql:" + sql);
                g_DatabaseHelper.ExecuteInsert(sql, MonitoringBusinessConnection);

            }
            catch (Exception ex)
            {
                log.Error("----------------AddAirReport_Hour_Calculate_Mul数据库处理方法异常：" + ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 生成日计算表数据
        /// </summary>
        /// <param name="sTime">开始时间</param>
        /// <param name="eTime">结束时间</param>
        /// <param name="PointId">站点id</param>
        public void AddAirReport_Day_Mul(DateTime sTime, DateTime eTime, int PointId, string TableName)
        {
            try
            {
                string StartTime = sTime.ToString("yyyy-MM-dd HH:mm:ss");
                string EndTime = eTime.ToString("yyyy-MM-dd HH:mm:ss");

                string sql = string.Format(@"
  	--更新日数据
	UPDATE dayData 
	SET dayData.PollutantValue=hourData.PollutantValue
		,dayData.UpdateDateTime=GETDATE()
	FROM {3} AS dayData
	LEFT JOIN
	(
		SELECT PointId
			,CONVERT(DATETIME,CONVERT(NVARCHAR(10),Tstamp,120),120) AS Tstamp
			,HourReport.PollutantCode
			,CASE WHEN (HourReport.PollutantCode IN ('a05024','a34004','a34002','a21026','a21005','a21004') AND COUNT(dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','))<20)
				THEN NULL 
ELSE dbo.F_Round(AVG(dbo.F_ValidValueByFlagVOC(PollutantValue,[AuditFlag],',',CategoryUid,Tstamp,VOCType)),DecimalDigit) 
			 END AS PollutantValue
		FROM AirReport.TB_HourReport_Calculate AS HourReport
		LEFT JOIN 
		(
			SELECT DISTINCT PollutantCode
				,CASE when DecimalDigit IS NULL THEN 3 ELSE DecimalDigit END AS DecimalDigit
                ,CategoryUid,VOCType
			FROM dbo.SY_PollutantCode
		) AS Pollutant
			ON HourReport.PollutantCode = Pollutant.PollutantCode
		WHERE HourReport.PointId={0} 
			AND HourReport.Tstamp>='{1}'
			AND HourReport.Tstamp<='{2}'
		GROUP BY PointId,CONVERT(DATETIME,CONVERT(NVARCHAR(10),Tstamp,120),120),HourReport.PollutantCode,DecimalDigit) AS hourData
		ON dayData.PointId = hourData.PointId AND dayData.[DateTime] = hourData.Tstamp AND dayData.PollutantCode = hourData.PollutantCode
	WHERE dayData.DateTime >= CONVERT(DATETIME,CONVERT(NVARCHAR(10),'{1}',120),120)
		AND dayData.DateTime <= CONVERT(DATETIME,CONVERT(NVARCHAR(10),'{2}',120),120)
		AND dayData.PointId ={0}
		AND hourData.PointId IS NOT NULL

	--插入日数据
	INSERT INTO {3}
		(DayReportUid
		   ,PointId
		   ,ReportDateTime
		   ,DateTime
		   ,DayOfYear
		   ,PollutantCode
		   ,PollutantValue
		   ,CreatUser
		   ,CreatDateTime
		   ,UpdateUser
		   ,UpdateDateTime
		   )
	SELECT NEWID()
		,hourData.PointId
		,GETDATE()
		,hourData.Tstamp
		,DATEPART(day,hourData.Tstamp)
		,hourData.PollutantCode
		,hourData.PollutantValue
		,'SystemSync'
		,GETDATE()
		,'SystemSync'
		,GETDATE()
	FROM (
		SELECT PointId
			,CONVERT(DATETIME,CONVERT(NVARCHAR(10),Tstamp,120),120) AS Tstamp
			,HourReport.PollutantCode
			,CASE WHEN (HourReport.PollutantCode IN ('a05024','a34004','a34002','a21026','a21005','a21004') AND COUNT(dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','))<20)
				THEN NULL 
ELSE dbo.F_Round(AVG(dbo.F_ValidValueByFlagVOC(PollutantValue,[AuditFlag],',',CategoryUid,Tstamp,VOCType)),DecimalDigit) 
			 END AS PollutantValue
		FROM AirReport.TB_HourReport_Calculate AS HourReport
		LEFT JOIN 
		(
			SELECT DISTINCT PollutantCode
				,CASE when DecimalDigit IS NULL THEN 3 ELSE DecimalDigit END AS DecimalDigit
                ,CategoryUid,VOCType
			FROM dbo.SY_PollutantCode
		) AS Pollutant
			ON HourReport.PollutantCode = Pollutant.PollutantCode
		WHERE HourReport.PointId={0} 
			AND HourReport.Tstamp>='{1}'
			AND HourReport.Tstamp<='{2}'
		GROUP BY PointId,CONVERT(DATETIME,CONVERT(NVARCHAR(10),Tstamp,120),120),HourReport.PollutantCode,DecimalDigit
	) AS hourData
	LEFT JOIN {3} AS dayData
		ON hourData.PointId = dayData.PointId AND hourData.Tstamp = dayData.[DateTime] AND hourData.PollutantCode = dayData.PollutantCode 
	WHERE hourData.Tstamp >= CONVERT(DATETIME,CONVERT(NVARCHAR(10),'{1}',120),120)
		AND hourData.Tstamp <= CONVERT(DATETIME,CONVERT(NVARCHAR(10),'{2}',120),120)
		AND hourData.PointId = {0}
		AND dayData.DayReportUid IS NULL", PointId, StartTime, EndTime, TableName);
                g_DatabaseHelper.ExecuteScalar(sql, MonitoringBusinessConnection);
            }
            catch (Exception ex)
            {
                log.Error("----------------AddAirReport_Day_Calculate_Mul数据库处理方法异常：" + ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 生成周数据
        /// </summary>
        /// <param name="sTime">开始时间</param>
        /// <param name="eTime">结束时间</param>
        /// <param name="PointId">站点id</param>
        public void AddAirReport_Week_Mul(DateTime sTime, DateTime eTime, int PointId)
        {
            try
            {
                string StartTime = sTime.ToString("yyyy-MM-dd HH:mm:ss");
                string EndTime = eTime.ToString("yyyy-MM-dd HH:mm:ss");
                string sql = string.Format(@"
--更新周数据
	UPDATE weekData 
	SET weekData.PollutantValue=dayData.PollutantValue
		,weekData.UpdateDateTime = GETDATE()
	FROM AirReport.TB_WeekReport AS weekData
	LEFT JOIN
	(
		SELECT PointId
			,dbo.F_GetCurWeekNum([DateTime],'date') AS [DateTime]
			,dayReport.PollutantCode
			,dbo.F_Round(AVG(PollutantValue),DecimalDigit) AS PollutantValue
		FROM AirReport.TB_DayReport_Calculate AS dayReport
		LEFT JOIN 
		(
			SELECT DISTINCT PollutantCode
				,CASE when DecimalDigit IS NULL THEN 3 ELSE DecimalDigit END AS DecimalDigit
			FROM dbo.SY_PollutantCode
		) AS Pollutant
			ON dayReport.PollutantCode = Pollutant.PollutantCode
		WHERE dayReport.PointId={0} 
			AND dayReport.[DateTime]>='{1}'
			AND dayReport.[DateTime]<='{2}'
		GROUP BY PointId,dbo.F_GetCurWeekNum([DateTime],'date'),dayReport.PollutantCode,DecimalDigit
	) AS dayData
		ON weekData.PointId = dayData.PointId AND weekData.[ReportDateTime] = dayData.[DateTime] AND weekData.PollutantCode = dayData.PollutantCode
	WHERE weekData.[ReportDateTime] >= dbo.F_GetCurWeekNum('{1}','date')
		AND weekData.[ReportDateTime] <= dbo.F_GetCurWeekNum('{2}','date')
		AND weekData.PointId = {0}
		AND dayData.PointId IS NOT NULL

	--插入周数据
	INSERT INTO AirReport.TB_WeekReport
		(WeekReportUid
		   ,PointId
		   ,ReportDateTime
		   ,[Year]
		   ,WeekOfYear
		   ,PollutantCode
		   ,PollutantValue
		   ,CreatUser
		   ,CreatDateTime
		   ,UpdateUser
		   ,UpdateDateTime)
	SELECT NEWID()
		,dayData.PointId
		,dayData.[DateTime]
		,dbo.F_GetCurWeekNum([DateTime],'year')
		,dbo.F_GetCurWeekNum([DateTime],'week')
		,dayData.PollutantCode
		,dayData.PollutantValue
		,'SystemSync'
		,GETDATE()
		,'SystemSync'
		,GETDATE()
	FROM (
		SELECT PointId
			,dbo.F_GetCurWeekNum([DateTime],'date') AS [DateTime]
			,dayReport.PollutantCode
			,dbo.F_Round(AVG(PollutantValue),DecimalDigit) AS PollutantValue
		FROM AirReport.TB_DayReport_Calculate AS dayReport
		LEFT JOIN 
		(
			SELECT DISTINCT PollutantCode
				,CASE when DecimalDigit IS NULL THEN 3 ELSE DecimalDigit END AS DecimalDigit
			FROM dbo.SY_PollutantCode
		) AS Pollutant
			ON dayReport.PollutantCode = Pollutant.PollutantCode
		WHERE dayReport.PointId={0} 
			AND dayReport.[DateTime]>='{1}'
			AND dayReport.[DateTime]<='{2}'
		GROUP BY PointId,dbo.F_GetCurWeekNum([DateTime],'date'),dayReport.PollutantCode,DecimalDigit
	) AS dayData
	LEFT JOIN AirReport.TB_WeekReport AS weekData
		ON dayData.PointId = weekData.PointId AND dayData.[DateTime] = weekData.[ReportDateTime] AND dayData.PollutantCode = weekData.PollutantCode 
	WHERE dayData.[DateTime] >= dbo.F_GetCurWeekNum('{1}','date')
		AND dayData.[DateTime] <= dbo.F_GetCurWeekNum('{2}','date')
		AND dayData.PointId = {0}
		AND weekData.WeekReportUid IS NULL", PointId, StartTime, EndTime);
                g_DatabaseHelper.ExecuteScalar(sql, MonitoringBusinessConnection);
            }
            catch (Exception ex)
            {
                log.Error("----------------AddAirReport_Week_Calculate_Mul数据库处理方法异常：" + ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 生成月计算数据
        /// </summary>
        /// <param name="sTime"></param>
        /// <param name="eTime"></param>
        /// <param name="PointId"></param>
        public void AddAirReport_Month_Mul(DateTime sTime, DateTime eTime, int PointId, string TableName)
        {
            try
            {
                string StartTime = sTime.ToString("yyyy-MM-dd HH:mm:ss");
                string EndTime = eTime.ToString("yyyy-MM-dd HH:mm:ss");
                string sql = string.Format(@"
    --更新月数据
	UPDATE monData 
	SET monData.PollutantValue=dayData.PollutantValue
		,monData.UpdateDateTime = GETDATE()
	FROM {3} AS monData
	LEFT JOIN
	(
		SELECT PointId
			,CONVERT(DATETIME,CONVERT(NVARCHAR(7),[DateTime],120)+'-01',120) AS [DateTime]
			,dayReport.PollutantCode
			,dbo.F_Round(AVG(PollutantValue),DecimalDigit) AS PollutantValue
		FROM [AirReport].[TB_DayReport_Calculate] AS dayReport
		LEFT JOIN 
		(
			SELECT DISTINCT PollutantCode
				,CASE when DecimalDigit IS NULL THEN 3 ELSE DecimalDigit END AS DecimalDigit
			FROM dbo.SY_PollutantCode
		) AS Pollutant
			ON dayReport.PollutantCode = Pollutant.PollutantCode
		WHERE dayReport.PointId={0}
			AND dayReport.[DateTime]>=CONVERT(DATETIME,CONVERT(NVARCHAR(10),'{1}',120),120)
			AND dayReport.[DateTime]<=CONVERT(DATETIME,CONVERT(NVARCHAR(10),'{2}',120),120)
		GROUP BY PointId,CONVERT(DATETIME,CONVERT(NVARCHAR(7),[DateTime],120)+'-01',120),dayReport.PollutantCode,DecimalDigit
	) AS dayData
		ON monData.PointId = dayData.PointId AND monData.[ReportDateTime] = dayData.[DateTime] AND monData.PollutantCode = dayData.PollutantCode
	WHERE monData.[ReportDateTime] >= CONVERT(DATETIME,CONVERT(NVARCHAR(7),'{1}',120)+'-01',120)
		AND monData.[ReportDateTime] <= CONVERT(DATETIME,CONVERT(NVARCHAR(7),'{2}',120)+'-01',120)
		AND monData.PointId = {0}
		AND dayData.PointId IS NOT NULL
		
	--插入月数据
	INSERT INTO {3}
		(MonthReportUid
		   ,PointId
		   ,ReportDateTime
		   ,[Year]
		   ,[MonthOfYear]
		   ,PollutantCode
		   ,PollutantValue
		   ,CreatUser
		   ,CreatDateTime
		   ,UpdateUser
		   ,UpdateDateTime)
	SELECT NEWID()
		,dayData.PointId
		,dayData.[DateTime]
		,DATEPART(year,dayData.[DateTime])
		,DATEPART(month,dayData.[DateTime])
		,dayData.PollutantCode
		,dayData.PollutantValue
		,'SystemSync'
		,GETDATE()
		,'SystemSync'
		,GETDATE()
	FROM (
		SELECT PointId
			,CONVERT(DATETIME,CONVERT(NVARCHAR(7),[DateTime],120)+'-01',120) AS [DateTime]
			,dayReport.PollutantCode
			,dbo.F_Round(AVG(PollutantValue),DecimalDigit) AS PollutantValue
		FROM [AirReport].[TB_DayReport_Calculate] AS dayReport
		LEFT JOIN 
		(
			SELECT DISTINCT PollutantCode
				,CASE when DecimalDigit IS NULL THEN 3 ELSE DecimalDigit END AS DecimalDigit
			FROM dbo.SY_PollutantCode
		) AS Pollutant
			ON dayReport.PollutantCode = Pollutant.PollutantCode
		WHERE dayReport.PointId={0}
			AND dayReport.[DateTime]>=CONVERT(DATETIME,CONVERT(NVARCHAR(10),'{1}',120),120)
			AND dayReport.[DateTime]<=CONVERT(DATETIME,CONVERT(NVARCHAR(10),'{2}',120),120)
		GROUP BY PointId,CONVERT(DATETIME,CONVERT(NVARCHAR(7),[DateTime],120)+'-01',120),dayReport.PollutantCode,DecimalDigit
	) AS dayData
	LEFT JOIN {3} AS monData
		ON dayData.PointId = monData.PointId AND dayData.[DateTime] = monData.[ReportDateTime] AND dayData.PollutantCode = monData.PollutantCode 
	WHERE dayData.[DateTime] >= CONVERT(DATETIME,CONVERT(NVARCHAR(7),'{1}',120)+'-01',120)
		AND dayData.[DateTime] <= CONVERT(DATETIME,CONVERT(NVARCHAR(7),'{2}',120)+'-01',120)
		AND dayData.PointId = {0}
		AND monData.MonthReportUid IS NULL
", PointId, StartTime, EndTime, TableName);
                g_DatabaseHelper.ExecuteScalar(sql, MonitoringBusinessConnection);
            }
            catch (Exception ex)
            {
                log.Error("----------------AddAirReport_Month_Calculate_Mul数据库处理方法异常：" + ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 生成季数据
        /// </summary>
        /// <param name="sTime">开始时间</param>
        /// <param name="eTime">结束时间</param>
        /// <param name="PointId">站点id</param>
        public void AddAirReport_Season_Mul(DateTime sTime, DateTime eTime, int PointId)
        {
            try
            {
                string StartTime = sTime.ToString("yyyy-MM-dd HH:mm:ss");
                string EndTime = eTime.ToString("yyyy-MM-dd HH:mm:ss");
                string sql = string.Format(@"
	--更新季数据
	UPDATE seasonData 
	SET seasonData.PollutantValue=MonthData.PollutantValue
		,seasonData.UpdateDateTime = GETDATE()
	FROM [AirReport].[TB_SeasonReport] AS seasonData
	LEFT JOIN
	(
		SELECT PointId
			,dbo.F_SeasonDate(DATEPART(YEAR,[ReportDateTime]),DATEPART(quarter,[ReportDateTime])) AS [DateTime]
			,MonthReport.PollutantCode
			,dbo.F_Round(AVG(PollutantValue),DecimalDigit) AS PollutantValue
		FROM [AirReport].[TB_MonthReport_Calculate] AS MonthReport
		LEFT JOIN 
		(
			SELECT DISTINCT PollutantCode
				,CASE when DecimalDigit IS NULL THEN 3 ELSE DecimalDigit END AS DecimalDigit
			FROM dbo.SY_PollutantCode
		) AS Pollutant
			ON MonthReport.PollutantCode = Pollutant.PollutantCode
		WHERE MonthReport.PointId={0} 
			AND MonthReport.[ReportDateTime]>=CONVERT(DATETIME,CONVERT(NVARCHAR(10),'{1}',120),120)
			AND MonthReport.[ReportDateTime]<=CONVERT(DATETIME,CONVERT(NVARCHAR(10),'{2}',120),120)
		GROUP BY PointId,dbo.F_SeasonDate(DATEPART(YEAR,[ReportDateTime]),DATEPART(quarter,[ReportDateTime])),MonthReport.PollutantCode,DecimalDigit
	) AS MonthData
		ON seasonData.PointId = MonthData.PointId AND seasonData.[ReportDateTime] = MonthData.[DateTime] AND seasonData.PollutantCode = MonthData.PollutantCode
	WHERE seasonData.[ReportDateTime] >= dbo.F_SeasonDate(DATEPART(YEAR,'{1}'),DATEPART(quarter,'{1}'))
		AND seasonData.[ReportDateTime] <= dbo.F_SeasonDate(DATEPART(YEAR,'{2}'),DATEPART(quarter,'{2}'))
		AND seasonData.PointId = {0}
		AND MonthData.PointId IS NOT NULL

	--插入季数据
	INSERT INTO [AirReport].[TB_SeasonReport]
		(SeasonReportUid
		   ,PointId
		   ,ReportDateTime
		   ,[Year]
		   ,[SeasonOfYear]
		   ,PollutantCode
		   ,PollutantValue
		   ,CreatUser
		   ,CreatDateTime
		   ,UpdateUser
		   ,UpdateDateTime)
	SELECT NEWID()
		,MonthData.PointId
		,MonthData.[DateTime]
		,DATEPART(year,MonthData.[DateTime])
		,DATEPART(quarter,MonthData.[DateTime])
		,MonthData.PollutantCode
		,MonthData.PollutantValue
		,'SystemSync'
		,GETDATE()
		,'SystemSync'
		,GETDATE()
	FROM (
		SELECT PointId
			,dbo.F_SeasonDate(DATEPART(YEAR,[ReportDateTime]),DATEPART(quarter,[ReportDateTime])) AS [DateTime]
			,MonthReport.PollutantCode
			,dbo.F_Round(AVG(PollutantValue),DecimalDigit) AS PollutantValue
		FROM [AirReport].[TB_MonthReport_Calculate] AS MonthReport
		LEFT JOIN 
		(
			SELECT DISTINCT PollutantCode
				,CASE when DecimalDigit IS NULL THEN 3 ELSE DecimalDigit END AS DecimalDigit
			FROM dbo.SY_PollutantCode
		) AS Pollutant
			ON MonthReport.PollutantCode = Pollutant.PollutantCode
		WHERE MonthReport.PointId={0} 
			AND MonthReport.[ReportDateTime]>=CONVERT(DATETIME,CONVERT(NVARCHAR(10),'{1}',120),120)
			AND MonthReport.[ReportDateTime]<=CONVERT(DATETIME,CONVERT(NVARCHAR(10),'{2}',120),120)
		GROUP BY PointId,dbo.F_SeasonDate(DATEPART(YEAR,[ReportDateTime]),DATEPART(quarter,[ReportDateTime])),MonthReport.PollutantCode,DecimalDigit
	) AS MonthData
	LEFT JOIN [AirReport].[TB_SeasonReport] AS seasonData
		ON MonthData.PointId = seasonData.PointId AND MonthData.[DateTime] = seasonData.[ReportDateTime] AND MonthData.PollutantCode = seasonData.PollutantCode 
	WHERE MonthData.[DateTime] >= dbo.F_SeasonDate(DATEPART(YEAR,'{1}'),DATEPART(quarter,'{1}'))
		AND MonthData.[DateTime] <= dbo.F_SeasonDate(DATEPART(YEAR,'{2}'),DATEPART(quarter,'{2}'))
		AND MonthData.PointId = {0}
		AND seasonData.SeasonReportUid IS NULL"
                    , PointId, StartTime, EndTime);
                g_DatabaseHelper.ExecuteScalar(sql, MonitoringBusinessConnection);
            }
            catch (Exception ex)
            {
                log.Error("----------------AddAirReport_Month_Calculate_Mul数据库处理方法异常：" + ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 生成年数据
        /// </summary>
        /// <param name="sTime"></param>
        /// <param name="eTime"></param>
        /// <param name="PointId"></param>
        public void AddAirReport_Year_Mul(DateTime sTime, DateTime eTime, int PointId)
        {
            try
            {
                string StartTime = sTime.ToString("yyyy-MM-dd HH:mm:ss");
                string EndTime = eTime.ToString("yyyy-MM-dd HH:mm:ss");
                string sql = string.Format(@"
--更新年数据
	UPDATE yearData 
	SET yearData.PollutantValue=MonthData.PollutantValue
		,yearData.UpdateDateTime = GETDATE()
	FROM AirReport.TB_YearReport AS yearData
	LEFT JOIN
	(
		SELECT PointId
			,CONVERT(DATETIME,CONVERT(NVARCHAR(4),[ReportDateTime],120)+'-01-01',120) AS [DateTime]
			,MonthReport.PollutantCode
			,dbo.F_Round(AVG(PollutantValue),DecimalDigit) AS PollutantValue
		FROM AirReport.TB_MonthReport_Calculate AS MonthReport
		LEFT JOIN 
		(
			SELECT DISTINCT PollutantCode
				,CASE when DecimalDigit IS NULL THEN 3 ELSE DecimalDigit END AS DecimalDigit
			FROM dbo.SY_PollutantCode
		) AS Pollutant
			ON MonthReport.PollutantCode = Pollutant.PollutantCode
		WHERE MonthReport.PointId={0} 
			AND MonthReport.[ReportDateTime]>=CONVERT(DATETIME,CONVERT(NVARCHAR(10),'{1}',120),120)
			AND MonthReport.[ReportDateTime]<=CONVERT(DATETIME,CONVERT(NVARCHAR(10),'{2}',120),120)
		GROUP BY PointId,CONVERT(DATETIME,CONVERT(NVARCHAR(4),[ReportDateTime],120)+'-01-01',120),MonthReport.PollutantCode,DecimalDigit
	) AS MonthData
		ON yearData.PointId = MonthData.PointId AND yearData.[ReportDateTime] = MonthData.[DateTime] AND yearData.PollutantCode = MonthData.PollutantCode
	WHERE yearData.[ReportDateTime] >= CONVERT(DATETIME,CONVERT(NVARCHAR(4),'{1}',120)+'-01-01',120)
		AND yearData.[ReportDateTime] <= CONVERT(DATETIME,CONVERT(NVARCHAR(4),'{2}',120)+'-01-01',120)
		AND yearData.PointId = {0}
		AND MonthData.PointId IS NOT NULL

	--插入年数据
	INSERT INTO AirReport.TB_YearReport
		(YearReportUid
		   ,PointId
		   ,ReportDateTime
		   ,[Year]
		   ,PollutantCode
		   ,PollutantValue
		   ,CreatUser
		   ,CreatDateTime
		   ,UpdateUser
		   ,UpdateDateTime)
	SELECT NEWID()
		,MonthData.PointId
		,MonthData.[DateTime]
		,DATEPART(year,MonthData.[DateTime])
		,MonthData.PollutantCode
		,MonthData.PollutantValue
		,'System'
		,GETDATE()
		,'System'
		,GETDATE()
	FROM (
		SELECT PointId
			,CONVERT(DATETIME,CONVERT(NVARCHAR(4),[ReportDateTime],120)+'-01-01',120) AS [DateTime]
			,MonthReport.PollutantCode
			,dbo.F_Round(AVG(PollutantValue),DecimalDigit) AS PollutantValue
		FROM AirReport.TB_MonthReport_Calculate AS MonthReport
		LEFT JOIN 
		(
			SELECT DISTINCT PollutantCode
				,CASE when DecimalDigit IS NULL THEN 3 ELSE DecimalDigit END AS DecimalDigit
			FROM dbo.SY_PollutantCode
		) AS Pollutant
			ON MonthReport.PollutantCode = Pollutant.PollutantCode
		WHERE MonthReport.PointId={0} 
			AND MonthReport.[ReportDateTime]>=CONVERT(DATETIME,CONVERT(NVARCHAR(10),'{1}',120),120)
			AND MonthReport.[ReportDateTime]<=CONVERT(DATETIME,CONVERT(NVARCHAR(10),'{2}',120),120)
		GROUP BY PointId,CONVERT(DATETIME,CONVERT(NVARCHAR(4),[ReportDateTime],120)+'-01-01',120),MonthReport.PollutantCode,DecimalDigit
	) AS MonthData
	LEFT JOIN AirReport.TB_YearReport AS yearData
		ON MonthData.PointId = yearData.PointId AND MonthData.[DateTime] = yearData.[ReportDateTime] AND MonthData.PollutantCode = yearData.PollutantCode 
	WHERE MonthData.[DateTime] >= CONVERT(DATETIME,CONVERT(NVARCHAR(4),'{1}',120)+'-01-01',120)
		AND MonthData.[DateTime] <= CONVERT(DATETIME,CONVERT(NVARCHAR(4),'{2}',120)+'-01-01',120)
		AND MonthData.PointId = {0}
		AND yearData.YearReportUid IS NULL"
                    , PointId, StartTime, EndTime);
                g_DatabaseHelper.ExecuteScalar(sql, MonitoringBusinessConnection);
            }
            catch (Exception ex)
            {
                log.Error("----------------AddAirReport_Month_Calculate_Mul数据库处理方法异常：" + ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 获取当前站点最新时间数据
        /// </summary>
        /// <param name="PointId">站点</param>
        /// <param name="TableName">表名</param>
        /// <returns></returns>
        public DataTable GetLatestData(int PointId, string TableName)
        {
            try
            {
                string sql = string.Format(@"  select top 1*   FROM {0}
  where PointId={1}
  order by Tstamp desc", TableName, PointId);
                return g_DatabaseHelper.ExecuteDataTable(sql, AirAutoMonitorConnection);
            }
            catch (Exception ex)
            {
                log.Error("----------------GetLatestData数据库处理方法异常：" + ex.ToString());
                throw ex;
            }
        }
        #endregion

    }
}
