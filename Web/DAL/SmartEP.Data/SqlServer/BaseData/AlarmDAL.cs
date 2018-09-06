using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Data.Enums;
using SmartEP.Data.SqlServer.Common.GridView;
using SmartEP.Utilities.AdoData;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.BaseData
{
    public class AlarmDAL
    {
        #region << 变量 >>
        /// <summary>
        /// 数据库处理类
        /// </summary>
        DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connection = string.Empty;
        /// <summary>
        /// 数据库表名
        /// </summary>
        private string tableName = string.Empty;

        /// <summary>
        /// 虚拟分页类
        /// </summary>
        GridViewPagerDAL g_GridViewPagerDAL = Singleton<GridViewPagerDAL>.GetInstance();
        #endregion

        /// <summary>
        /// 获取单点单因子最新一条数据
        /// </summary>
        /// <param name="appType"></param>
        /// <param name="pollutantDataType"></param>
        /// <param name="portIds"></param>
        /// <param name="factorCodes"></param>
        /// <returns></returns>
        public DataView GetLatestData(ApplicationType applicationType, PollutantDataType pollutantDataType, string portId, string factorCode)
        {
            tableName = SmartEP.Data.Enums.EnumMapping.GetAutoMonitorTableName(applicationType, pollutantDataType);
            connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(applicationType, pollutantDataType);
            ////获取测点id字符串，以逗号隔开
            //string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds, ",");
            ////获取因子编码字符串，以逗号隔开
            //string factorCodesStr = StringExtensions.GetArrayStrNoEmpty(factorCodes, ",");
            //小时数据推前两个小时，五分钟推前10分钟查找最新数据
            DateTime dtBegin = pollutantDataType == PollutantDataType.Min5 ? DateTime.Now.AddMinutes(-10) : DateTime.Now.AddHours(-2);
            StringBuilder sqlStringBuilder = new StringBuilder();
            sqlStringBuilder.AppendFormat(@"SELECT a.PointId,a.Tstamp,a.PollutantCode,a.PollutantValue FROM {0} a INNER JOIN (SELECT MAX(Tstamp) 'Tstamp',PointId,PollutantCode FROM {0}", tableName);
            sqlStringBuilder.AppendFormat(@" WHERE PointId ={0} AND PollutantCode ='{1}' AND Tstamp>='{2}' AND Tstamp<GETDATE()", portId, factorCode, dtBegin.ToString("yyyy-MM-dd HH:mm"));
            sqlStringBuilder.AppendFormat(@" GROUP BY PointId,PollutantCode) AS  b ON a.PointId=b.PointId AND a.PollutantCode = b.PollutantCode AND a.Tstamp = b.Tstamp ");
            return g_DatabaseHelper.ExecuteDataView(sqlStringBuilder.ToString(), connection);
        }

        /// <summary>
        /// 获取多点多因子每个点位最新一条数据
        /// </summary>
        /// <param name="appType"></param>
        /// <param name="pollutantDataType"></param>
        /// <param name="portIds"></param>
        /// <param name="factorCodes"></param>
        /// <returns></returns>
        public DataView GetLatestData(ApplicationType applicationType, PollutantDataType pollutantDataType, List<string> listPortIds, List<string> listFactorCodes)
        {
            tableName = SmartEP.Data.Enums.EnumMapping.GetAutoMonitorTableName(applicationType, pollutantDataType);
            connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(applicationType, pollutantDataType);
            //获取测点id字符串，以逗号隔开
            string portIds = StringExtensions.GetArrayStrNoEmpty(listPortIds, ",");
            //获取因子编码字符串，以逗号隔开
            string factorCodes = "'" + StringExtensions.GetArrayStrNoEmpty(listFactorCodes, "','") + "'";
            //小时数据推前两个小时，五分钟推前10分钟查找最新数据
            DateTime dtBegin = pollutantDataType == PollutantDataType.Min5 ? DateTime.Now.AddMinutes(-10) : DateTime.Now.AddHours(-2);
            StringBuilder sqlStringBuilder = new StringBuilder();
            sqlStringBuilder.AppendFormat(@"SELECT a.PointId,a.Tstamp,a.PollutantCode,a.PollutantValue FROM {0} a INNER JOIN (SELECT MAX(Tstamp) 'Tstamp',PointId,PollutantCode FROM {0}", tableName);
            sqlStringBuilder.AppendFormat(@" WHERE PointId in ({0}) AND PollutantCode in ({1}) AND Tstamp>='{2}' AND Tstamp<GETDATE()", portIds, factorCodes, dtBegin.ToString("yyyy-MM-dd HH:mm"));
            sqlStringBuilder.AppendFormat(@" GROUP BY PointId,PollutantCode) AS  b ON a.PointId=b.PointId AND a.PollutantCode = b.PollutantCode AND a.Tstamp = b.Tstamp ");
            return g_DatabaseHelper.ExecuteDataView(sqlStringBuilder.ToString(), connection);
        }

        /// <summary>
        /// 获取多点位最新数据时间
        /// </summary>
        /// <param name="appType"></param>
        /// <param name="pollutantDataType"></param>
        /// <param name="portIds"></param>
        /// <param name="factorCodes"></param>
        /// <returns></returns>
        public DataView GetLatestDataTime(ApplicationType applicationType, PollutantDataType pollutantDataType, List<string> portIds)
        {
            tableName = SmartEP.Data.Enums.EnumMapping.GetAutoMonitorTableName(applicationType, pollutantDataType);
            connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(applicationType, pollutantDataType);
            //获取测点id字符串，以逗号隔开
            string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds, ",");
            StringBuilder sqlStringBuilder = new StringBuilder();
            sqlStringBuilder.AppendFormat(@"SELECT distinct a.PointId,a.Tstamp FROM {0} a INNER JOIN (SELECT MAX(Tstamp) 'Tstamp',PointId FROM {0}", tableName);
            sqlStringBuilder.AppendFormat(@" WHERE PointId IN ({0})  AND Tstamp<GETDATE() GROUP BY PointId) AS  b ON a.PointId=b.PointId AND a.Tstamp = b.Tstamp", portIdsStr);
            return g_DatabaseHelper.ExecuteDataView(sqlStringBuilder.ToString(), connection);
        }

        /// <summary>
        /// 查询单点单因子某时间点之前的N组数据
        /// </summary>
        /// <param name="appType">应用程序类型</param>
        /// <param name="pollutantDataType">数据类型</param>
        /// <param name="portId">测点id</param>
        /// <param name="tstamp">时间点</param>
        /// <param name="factorCode">因子编码</param>
        /// <param name="compareBeforeGroups">往前推N组</param>
        /// <param name="isContainThisRecord">是否包含此时间点数据</param>
        /// <returns></returns>
        public DataView GetCompareBeforeData(ApplicationType applicationType, PollutantDataType pollutantDataType, string portId, DateTime tstamp, string factorCode, int compareBeforeGroups, bool isContainThisRecord)
        {
            tableName = SmartEP.Data.Enums.EnumMapping.GetAutoMonitorTableName(applicationType, pollutantDataType);
            connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(applicationType, pollutantDataType);
            StringBuilder sqlStringBuilder = new StringBuilder();
            sqlStringBuilder.AppendFormat(@"SELECT TOP {0} PointId,Tstamp,PollutantCode,PollutantValue FROM {1} ", compareBeforeGroups, tableName);
            sqlStringBuilder.AppendFormat(@"WHERE PointId = {0} AND PollutantCode='{1}'  AND Tstamp<GETDATE(){2} order by Tstamp desc", portId, factorCode, isContainThisRecord ? "" : string.Format(" AND Tstamp<>'{0}'", tstamp.ToString("yyyy-MM-dd HH:mm")));
            return g_DatabaseHelper.ExecuteDataView(sqlStringBuilder.ToString(), connection);
        }

        /// <summary>
        /// 判定是否重复数据
        /// </summary>
        /// <param name="applicationType"></param>
        /// <param name="pollutantDataType"></param>
        /// <param name="portId"></param>
        /// <param name="tstamp"></param>
        /// <param name="factorCode"></param>
        /// <param name="repeatNumber"></param>
        /// <returns></returns>
        public bool IsRepeatData(ApplicationType applicationType, PollutantDataType pollutantDataType, string portId, DateTime tstamp, string factorCode, int repeatNumber)
        {
            tableName = SmartEP.Data.Enums.EnumMapping.GetAutoMonitorTableName(applicationType, pollutantDataType);
            connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(applicationType, pollutantDataType);
            StringBuilder sqlStringBuilder = new StringBuilder();
            sqlStringBuilder.AppendFormat(@"select  PollutantCode,PollutantValue FROM 
                (SELECT TOP {0} PointId,Tstamp,PollutantCode,PollutantValue FROM {1}
                WHERE PointId = {2} AND PollutantCode='{3}' AND Tstamp<'{4}' AND Tstamp<GETDATE()   
                order by Tstamp desc ) a GROUP BY PollutantCode,PollutantValue having COUNT(1)={0}", repeatNumber.ToString(), tableName, portId, factorCode, tstamp.ToString("yyyy-MM-dd HH:mm"));

            return g_DatabaseHelper.ExecuteDataTable(sqlStringBuilder.ToString(), connection).Rows.Count > 0 ? true : false;

        }

        /// <summary>
        /// 取得虚拟分页数据和总行数
        /// </summary>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="where">WHERE条件</param>
        /// <param name="recordTotal">数据总行数</param>
        /// <returns></returns>
        public DataView GetGridViewPager(int pageSize, int pageNo, string where, out int recordTotal)
        {
            connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.BaseData);
            return g_GridViewPagerDAL.GetGridViewPager("V_AlarmHandle", "*", "AlarmUid", pageSize, pageNo, "Order by OrderByNum asc, RecordDateTime desc,Content desc", where, connection, out recordTotal);
        }

        /// <summary>
        /// 取得导出报警信息
        /// </summary>
        /// <param name="where">查询条件，不带where</param>
        /// <param name="orderBy">排序，不带order by</param>
        /// <returns></returns>
        public DataView GetExportData(string where, string orderBy)
        {
            connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.BaseData);
            orderBy = string.IsNullOrEmpty(orderBy) ? "RecordDateTime,Content desc " : orderBy;
            where = string.IsNullOrEmpty(where) ? " 1=1 " : where;
            string sql = string.Format(@"
                SELECT ROW_NUMBER() OVER(ORDER BY {0}) AS [序号]
	                ,[MonitoringPointName] AS [站点]
	                ,[RecordDateTime] AS [时间]
	                ,[AlarmEventName] AS [报警类型]
	                ,[AlarmGradeName] AS [报警等级]
	                ,[DataTypeName] AS [数据类型]
	                ,[Content] AS [报警内容]
	                
	                ,CASE WHEN [dealFlag] = 1 THEN '已处理' ELSE '未处理' end AS [是否处理]
	                ,[dealMan] AS [处理人]
	                ,[dealTime] AS [处理时间]
	                ,[dealContent] AS [处理内容]
	                ,CASE WHEN [auditFlag] = 1 THEN '已处理' ELSE '未处理' end AS [是否审核]
	                ,[auditMan] AS [审核人]
	                ,[auditTime] AS [审核时间]
	                ,[auditContent] AS [审核内容]
                FROM [dbo].[V_AlarmHandle]
                where {1}
            ", orderBy, where);
            return g_DatabaseHelper.ExecuteDataView(sql, connection);
        }
        /// <summary>
        /// 根据用户名获取联系方式
        /// </summary>
        /// <param name="Name">用户名</param>
        /// <returns></returns>
        public DataView GetNumberByName(string Name)
        {
            connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.BaseData);
            Name = string.IsNullOrEmpty(Name) ? "超级管理员" : Name;
            string sql = string.Format(@"SELECT     b.Name, a.Number, a.RowGuid, b.Description, b.OrderByNum, b.CreatUser, b.CreatDateTime, b.UpdateUser, b.UpdateDateTime
                        FROM         AlarmNotify.TB_NotifyNumber AS a INNER JOIN
                        AlarmNotify.TB_NotifyAddress AS b ON a.NotifyAddressUid = b.RowGuid AND b.Name='{0}'  and a.NotifyTypeUid='501c4de6-4b4b-4462-9932-4379934def46'", Name);
            return g_DatabaseHelper.ExecuteDataView(sql, connection);
        }

        /// <summary>
        /// 批量查询报警信息  by  zhuwei  2016-7-14
        /// </summary>
        /// <param name="applicationUid"></param>
        /// <param name="monitoringPointUid"></param>
        /// <param name="alarmEventUid"></param>
        /// <param name="itemName"></param>
        /// <returns></returns>
        public DataView GetPLAlarmInfo(string applicationUid, string monitoringPointUid, string alarmEventUid, string itemName, string pLHandleid, DateTime? recordDateTime)
        {
            connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.BaseData);
            string sql = string.Empty;
            if (pLHandleid == "1")//处理当天数据
            {
                sql = string.Format(@"  select AlarmUid,RecordDateTime,CreatDateTime from [AMS_BaseData].[dbo].[V_AlarmHandle] where ApplicationUid='{0}'
  and MonitoringPointUid='{1}' and AlarmEventUid='{2}' and ItemName='{3}'
   and  CreatDateTime>=convert(varchar(10),dateadd(d,0,'{4}'),120) 
   and CreatDateTime<convert(varchar(10),dateadd(d,1,'{4}'),120) AND (dealflag = 0 OR dealflag IS NULL) order by CreatDateTime desc", applicationUid, monitoringPointUid, alarmEventUid, itemName, recordDateTime);
            }
            else
            {
                sql = string.Format(@"  select AlarmUid,RecordDateTime,CreatDateTime from [AMS_BaseData].[dbo].[V_AlarmHandle] where ApplicationUid='{0}'
  and MonitoringPointUid='{1}' and AlarmEventUid='{2}' and ItemName='{3}' AND (dealflag = 0 OR dealflag IS NULL) order by CreatDateTime desc", applicationUid, monitoringPointUid, alarmEventUid, itemName);
            }
            return g_DatabaseHelper.ExecuteDataView(sql, connection);
        }


        /// <summary>
        /// 批量查询审核的报警信息  by  zhuwei  2016-7-14
        /// </summary>
        /// <param name="applicationUid"></param>
        /// <param name="monitoringPointUid"></param>
        /// <param name="alarmEventUid"></param>
        /// <param name="itemName"></param>
        /// <returns></returns>
        public DataView GetAuditPLAlarmInfo(string applicationUid, string monitoringPointUid, string alarmEventUid, string itemName, string pLHandleid, DateTime? recordDateTime)
        {
            connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.BaseData);
            string sql = string.Empty;
            if (pLHandleid == "1")
            {
                sql = string.Format(@"  select AlarmUid,RecordDateTime,CreatDateTime from [AMS_BaseData].[dbo].[V_AlarmHandle] where ApplicationUid='{0}'
  and MonitoringPointUid='{1}' and AlarmEventUid='{2}' and ItemName='{3}'
   and  CreatDateTime>=convert(varchar(10),dateadd(d,0,'{4}'),120) 
   and CreatDateTime<convert(varchar(10),dateadd(d,1,'{4}'),120) and dealflag='1' AND (auditFlag = 0 OR auditFlag IS NULL) order by CreatDateTime desc", applicationUid, monitoringPointUid, alarmEventUid, itemName, recordDateTime);
            }
            else
            {
                sql = string.Format(@"  select AlarmUid,RecordDateTime,CreatDateTime from [AMS_BaseData].[dbo].[V_AlarmHandle] where ApplicationUid='{0}'
  and MonitoringPointUid='{1}' and AlarmEventUid='{2}' and ItemName='{3}' and dealflag='1' AND (auditFlag = 0 OR auditFlag IS NULL) order by CreatDateTime desc", applicationUid, monitoringPointUid, alarmEventUid, itemName);
            }
            return g_DatabaseHelper.ExecuteDataView(sql, connection);
        }


        /// <summary>
        /// 查询报警最新信息  by  zhuwei  2016-7-14
        /// </summary>
        /// <param name="applicationUid"></param>
        /// <param name="monitoringPointUid"></param>
        /// <param name="alarmEventUid"></param>
        /// <param name="itemName"></param>
        /// <returns></returns>
        public DataView GetLastNewAlarmInfo(string applicationUid, string monitoringPointUid, string alarmEventUid, string itemName, string startTime, string endTime)
        {
            connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.BaseData);
            string sql = string.Empty;
            sql = string.Format(@"  select top 1 * from [AMS_BaseData].[dbo].[V_AlarmHandle] where ApplicationUid='{0}'
  and MonitoringPointUid='{1}' and AlarmEventUid='{2}' and ItemName='{3}'
   and  RecordDateTime>='{4}' 
   and RecordDateTime<='{5}' order by RecordDateTime desc", applicationUid, monitoringPointUid, alarmEventUid, itemName, startTime, endTime);
            return g_DatabaseHelper.ExecuteDataView(sql, connection);
        }


        /// <summary>
        /// 查询审核的报警最新信息  by  zhuwei  2016-7-14
        /// </summary>
        /// <param name="applicationUid"></param>
        /// <param name="monitoringPointUid"></param>
        /// <param name="alarmEventUid"></param>
        /// <param name="itemName"></param>
        /// <returns></returns>
        public DataView GetAuditLastNewAlarmInfo(string applicationUid, string monitoringPointUid, string alarmEventUid, string itemName, string startTime, string endTime)
        {
            connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.BaseData);
            string sql = string.Empty;
            sql = string.Format(@"  select top 1 * from [AMS_BaseData].[dbo].[V_AlarmHandle] where ApplicationUid='{0}'
  and MonitoringPointUid='{1}' and AlarmEventUid='{2}' and ItemName='{3}'
   and  CreatDateTime>='{4}' 
   and CreatDateTime<='{5}' and dealflag=1 order by CreatDateTime desc", applicationUid, monitoringPointUid, alarmEventUid, itemName, startTime, endTime);
            return g_DatabaseHelper.ExecuteDataView(sql, connection);
        }


        /// <summary>
        /// 查询报警信息  by  zhuwei  2016-7-14
        /// </summary>
        /// <returns></returns>
        public DataView GetAlarmInfo(string where)
        {
            connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.BaseData);
            string sql = string.Empty;
            sql = string.Format(@"  select ApplicationUid,monitoringPointUid,alarmEventUid,itemName from [AMS_BaseData].[dbo].[V_AlarmHandle] where 1=1 and {0}  Order by OrderByNum asc, RecordDateTime,Content desc", where);
            return g_DatabaseHelper.ExecuteDataView(sql, connection);
        }
        public DataView GetAlarmInfoNew(string where)
        {
            connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.BaseData);
            string sql = string.Empty;
            sql = string.Format(@"  select b.* from
                                (select ApplicationUid,MonitoringPointUid,MAX(RecordDateTime) as RecordDateTime,MAX(CreatDateTime) as CreatDateTime,AlarmEventUid ,ItemName FROM [dbo].[V_AlarmHandle]
                                where 1=1 and {0}
                                 group by ApplicationUid,MonitoringPointUid,AlarmEventUid,ItemName) as a inner join  [dbo].[V_AlarmHandle]  as b 
                                on a.AlarmEventUid=b.AlarmEventUid and a.ApplicationUid=b.ApplicationUid and a.MonitoringPointUid=b.MonitoringPointUid  and a.RecordDateTime=b.RecordDateTime
                                and a.CreatDateTime=b.CreatDateTime and a.ItemName=b.ItemName    order by b.CreatDateTime desc", where);
            return g_DatabaseHelper.ExecuteDataView(sql, connection);
        }

        /// <summary>
        /// 根据报警ID查询报警信息  by  zhuwei  2016-7-14
        /// </summary>
        /// <returns></returns>
        public DataView GetAlarmInfoByAlarmId(string alarmId)
        {
            connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.BaseData);
            string sql = string.Empty;
            sql = string.Format(@"  select auditFlag from [AMS_BaseData].[dbo].[V_AlarmHandle] where alarmUid='{0}'", alarmId);
            return g_DatabaseHelper.ExecuteDataView(sql, connection);
        }


    }
}
