using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using SmartEP.Utilities.AdoData;
using SmartEP.Core.Generic;
using SmartEP.Data.Enums;

namespace SmartEP.Data.SqlServer.MonitoringBusiness.Air
{
    public class SyncGuoJiaDataDAL
    {
        private string AMS_MonitorBusiness_Conn = EnumMapping.GetConnectionName(DataConnectionType.MonitoringBusiness);

        private const string GuoJia_Air_Conn = "GuoJia_Air_Conn";

        private const string SysType = "GuoJia";

        /// <summary>
        /// 审核状态(未审核)
        /// </summary>
        private const string STATUS_NOT_AUDITED = "0";

        /// <summary>
        /// 同步国家审核日志因子映射标识
        /// </summary>
        private const string SysType_AuditLog = "GuoJiaAuditLog";

        /// <summary>
        /// 通过审核小时数据标识
        /// </summary>
        private const string BusinessType_Data = "Data";

        /// <summary>
        /// 通过审核日志数据标识
        /// </summary>
        private const string BusinessType_AuditLog = "AuditLog";

        /// <summary>
        /// 数据库处理类
        /// </summary>
        protected DatabaseHelper dbHelper = Singleton<DatabaseHelper>.GetInstance();

        /// <summary>
        /// 原始状态分割字符
        /// </summary>
        private char sourceStatusSplitChar = ',';

        /// <summary>
        /// 本地状态存储分割字符
        /// </summary>
        private char localStatusSplitChar = ',';

        private DataTable channelInfo = null;

        /// <summary>
        /// 同步国家审核日志因子映射
        /// </summary>
        private DataTable auditLogChannelInfo = null;

        /// <summary>
        /// 国家点位映射
        /// </summary>
        private DataTable portInfo = null;

        private SyncTransferMappingDAL mappingBiz = null;

        /// <summary>
        /// 状态映射字典
        /// </summary>
        private Dictionary<string, string> statusDic = null;

        public SyncGuoJiaDataDAL()
        {
            mappingBiz = new SyncTransferMappingDAL();
            //关联因子映射
            channelInfo = mappingBiz.GetChannelMapping(SysType);

            // 同步国家审核日志因子映射
            auditLogChannelInfo = mappingBiz.GetChannelMapping(SysType_AuditLog);

            //国家点位映射
            portInfo = mappingBiz.GetPortMapping(SysType, null);

            //状态字典映射
            statusDic = mappingBiz.GetStatusMappingDic(SysType, BusinessType_Data);

            sourceStatusSplitChar = Convert.ToChar(System.Configuration.ConfigurationManager.AppSettings["sourceStatusSplitChar"].ToString());
            localStatusSplitChar = Convert.ToChar(System.Configuration.ConfigurationManager.AppSettings["localStatusSplitChar"].ToString());
        }

        #region << 同步审核小时数据 >>
        /// <summary>
        /// 同步审核小时数据
        /// </summary>
        /// <param name="auditStatusUid">状态UID</param>
        /// <param name="appUid">应用类型UID</param>
        /// <param name="pointId">点位ID</param>
        /// <param name="date">数据时间</param>
        /// <param name="guoJiaTblName">国家库数据表完整表名</param>
        /// <param name="conn_db_str">数据库完整名</param>
        /// <param name="guoJiaFactors">国家因子列表(为空表示所有因子)</param>
        /// <param name="localFactors">本地因子列表(为空表示所有因子)</param>
        /// <param name="isPerYearLastHour">是否为同步每年最后一小时数据</param>
        public void SyncAuditAirInfectantByHourData(string auditStatusUid, string appUid,
                                                    string pointId, DateTime date,
                                                    string guoJiaTblName, string conn_db_str,
                                                    string guoJiaFactors, string localFactors,
                                                    bool isPerYearLastHour)
        {
            string guoJiacodeWhere = " 1 = 1 ";
            string localCodeWhere = " 1 = 1 ";

            if (!string.IsNullOrEmpty(guoJiaFactors) && !string.IsNullOrEmpty(localFactors))
            {// 因子条件都不为空
                guoJiacodeWhere = string.Format(@" PollutantCode in ('{0}')", guoJiaFactors);
                localCodeWhere = string.Format(@" PollutantCode in ('{0}')", localFactors);
            }

            // 国家查询数据的时间条件
            string guoJiaStartTime = date.ToString("yyyy-MM-dd 01:00:00.000");
            string guoJiaEndTime = date.AddDays(1).ToString("yyyy-MM-dd 01:00:00.000");

            // 本地删除数据的时间条件
            string localStartTime = date.ToString("yyyy-MM-dd 00:00:00.000");
            string localEndTime = date.AddDays(1).ToString("yyyy-MM-dd 00:00:00.000");

            if (isPerYearLastHour)
            {// 为同步每年最后一小时数据
                guoJiaStartTime = date.AddDays(1).ToString("yyyy-MM-dd 00:00:00.000");
                guoJiaEndTime = date.AddDays(1).ToString("yyyy-MM-dd 01:00:00.000");

                localStartTime = date.ToString("yyyy-MM-dd 23:00:00.000");
                localEndTime = date.AddDays(1).ToString("yyyy-MM-dd 00:00:00.000");

                // 国家表名需要替换为第二年表名(表名样例：[dbo].[Air_h_2016_1011A_App])
                guoJiaTblName = guoJiaTblName.Replace("_" + date.Year + "_", "_" + date.AddYears(1).Year + "_");
            }

            String queryDataSql = string.Format(@"                                            
                                            select '{0}' as AuditStatusUid, 
                                                   '{1}' as ApplicationUid, 
                                                    {2} as PointId, 
                                                    dateadd(hh, -1, TimePoint) as Tstamp, 
	                                                {3} as PollutantCode, 
                                                    (case when MonValue = -1 then null else MonValue end) as PollutantValue, 
                                                    Mark as AuditFlag, 
                                                    'SystemSync' as CreatUser 
                                              from {4} 
                                             where {5}
                                               and TimePoint >= '{6}' 
                                               and TimePoint < '{7}' 
                                             order by TimePoint asc"
                                            , auditStatusUid
                                            , appUid
                                            , pointId
                                            , mappingBiz.ConvertPollutantCodeToCaseSql(channelInfo)
                                            , guoJiaTblName
                                            , guoJiacodeWhere
                                            , guoJiaStartTime
                                            , guoJiaEndTime);

            DataTable dtGuoJia = null;

            try
            {
                dtGuoJia = dbHelper.ExecuteDataTable(queryDataSql, conn_db_str);
            }
            catch (Exception e)
            { }

            if (dtGuoJia == null || dtGuoJia.Rows.Count == 0)
            {
                /*
                CommonFunction.WriteInfoLog("Exist Sync ([Audit].[TB_AuditAirInfectantByHour]) Not Query Data, PortId=[" 
                                          + LocalPort + "] TotalCount=["
                                          + dtGuoJia.Rows.Count + "] Tstamp From (" + dtStart.ToString("yyyy-MM-dd HH:mm:ss")
                                          + ") To (" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss") + ")");
                */
                return;
            }

            // 根据配置将因子单位转换为对应值
            mappingBiz.ConversionPollutantUnit(dtGuoJia, "PollutantCode", "PollutantValue", channelInfo);

            foreach (DataRow drMap in dtGuoJia.Rows)
            {
                drMap["AuditFlag"] = mappingBiz.GetConvertStatus(statusDic, drMap["AuditFlag"].ToString(), sourceStatusSplitChar, localStatusSplitChar, BusinessType_Data);
            }

            string preDelSql = string.Format(@"
                                               delete from [AirReport].[TB_HourReport] 
                                                where PointId = {0} 
                                                  and {1}
                                                  and Tstamp >= '{2}' 
                                                  and Tstamp <  '{3}'"
                                     , pointId
                                     , localCodeWhere
                                     , localStartTime
                                     , localEndTime
                                     );
            //先删除表中数据避免重复插入
            dbHelper.ExecuteNonQuery(preDelSql, AMS_MonitorBusiness_Conn);

            /*
           CommonFunction.WriteInfoLog("Delete ([Audit].[TB_AuditAirInfectantByHour]) Data Success PortId=[" + LocalPort
                                     + "] Tstamp From (" + dtStart.ToString("yyyy-MM-dd HH:mm:ss")
                                     + ") To (" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss") + ")");

           CommonFunction.WriteInfoLog("Begin Sync ([Audit].[TB_AuditAirInfectantByHour]) Data PortId=[" + LocalPort + "]");
            */

            // 添加数据
            string connStr = ConfigurationManager.ConnectionStrings[AMS_MonitorBusiness_Conn].ConnectionString;
            SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(connStr, SqlBulkCopyOptions.UseInternalTransaction);

            sqlbulkcopy.DestinationTableName = "[AirReport].[TB_HourReport]";//目标表表名
            sqlbulkcopy.ColumnMappings.Add("[PointId]", "[PointId]");
            sqlbulkcopy.ColumnMappings.Add("[Tstamp]", "[Tstamp]");
            sqlbulkcopy.ColumnMappings.Add("[PollutantCode]", "[PollutantCode]");
            sqlbulkcopy.ColumnMappings.Add("[PollutantValue]", "[PollutantValue]");
            sqlbulkcopy.ColumnMappings.Add("[AuditFlag]", "[AuditFlag]");
            sqlbulkcopy.ColumnMappings.Add("[CreatUser]", "[CreatUser]");

            sqlbulkcopy.WriteToServer(dtGuoJia);
            sqlbulkcopy.Close();

            // 同步国家审核小时数据过来后，更新日概况数据状态
            updateAuditStatusForDayDataStatus(auditStatusUid);

            /*
            CommonFunction.WriteInfoLog("End Sync ([Audit].[TB_AuditAirInfectantByHour]) Data Success PortId=[" + LocalPort
                                      + "] TotalCount=[" + dtGuoJia.Rows.Count + "] Tstamp From ("
                                      + dtStart.ToString("yyyy-MM-dd HH:mm:ss") + ") To ("
                                      + dtEnd.ToString("yyyy-MM-dd HH:mm:ss") + ")");
            */
        }
        /// <summary>
        /// 判断是否存在日概况数据
        /// </summary>
        /// <param name="appUid">应用类型UID</param>
        /// <param name="pointId">点位ID</param>
        /// <param name="date">数据时间</param>
        /// <param name="status">审核状态</param>
        /// <returns></returns>
        public string GetAuditStatusUid(string appUid, string pointId, DateTime date, out string status)
        {
            String queryDataSql = string.Format(@"                                            
                                            select AuditStatusUid, 
                                                   Status 
                                              from [Audit].[TB_AuditStatusForDay] 
                                             where ApplicationUid = '{0}'
                                               and PointId = {1} 
                                               and Date = '{2}'"
                                           , appUid
                                           , pointId
                                           , date.ToString("yyyy-MM-dd 00:00:00.000")
                                           );

            DataTable dataDt = dbHelper.ExecuteDataTable(queryDataSql, AMS_MonitorBusiness_Conn);

            foreach (DataRow drMap in dataDt.Rows)
            {
                status = drMap["Status"].ToString();
                return drMap["AuditStatusUid"].ToString();
            }

            // 默认为未审核
            status = STATUS_NOT_AUDITED;

            return string.Empty;
        }

        /// <summary>
        /// 添加日概况数据
        /// </summary>
        /// <param name="auditStatusUid">状态UID</param>
        /// <param name="appUid">应用类型UID</param>
        /// <param name="pointId">点位ID</param>
        /// <param name="date">数据时间</param>
        public void AddAuditStatusForDayData(string auditStatusUid, string appUid,
                                             string pointId, DateTime date)
        {
            String insertSql = string.Format(@"                                            
                                            insert into [Audit].[TB_AuditStatusForDay] 
                                                       ([AuditStatusUid],
                                                        [ApplicationUid],
                                                        [PointId],
                                                        [Date],
                                                        [Status],
                                                        [CreatUser],
                                                        [CreatDateTime])
                                                values ('{0}',
                                                        '{1}',
                                                         {2},
                                                        '{3}',
                                                        '0',
                                                        'SystemSync',
                                                        getdate())"
                                           , auditStatusUid
                                           , appUid
                                           , pointId
                                           , date.ToString("yyyy-MM-dd 00:00:00.000")
                                           );

            dbHelper.ExecuteNonQuery(insertSql, AMS_MonitorBusiness_Conn);
        }
        /// <summary>
        ///更新审核数据
        /// </summary>
        /// <param name="dt">数据源</param>
        public void UpdateAuditData(DataTable dt)
        {
            string tableName = "[Audit].[TB_AuditAirInfectantByHour]";
            string appUid = SmartEP.Core.Enums.EnumMapping.GetDesc(SmartEP.Core.Enums.ApplicationType.Air);
            List<CommandInfo> sqllist = new List<CommandInfo>();
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["VerifyDataType"].ToString() != "7")
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("delete from  ");
                        strSql.Append(tableName);
                        strSql.Append(" where AuditStatusUid='");
                        strSql.Append(dr["AuditStatusUid"].ToString());
                        strSql.Append("' and DataDateTime='");
                        strSql.Append(dr["TimePoint"].ToString());
                        strSql.Append("' and PollutantCode='");
                        strSql.Append(dr["PollutantCode"].ToString());
                        strSql.Append("';");
                        strSql.Append("insert into ");
                        strSql.Append(tableName);
                        strSql.Append("(AuditStatusUid, PointId,ApplicationUid,DataDateTime,PollutantCode,PollutantValue,AuditFlag,CreatUser)");
                        strSql.Append("values(@AuditStatusUid,@PointId,@ApplicationUid,@DataDateTime,@PollutantCode,@PollutantValue,@AuditFlag,@CreatUser);");
                        SqlParameter[] parameters = { 
                                new SqlParameter("@AuditStatusUid", SqlDbType.NVarChar,50),
                                new SqlParameter("@PointId", SqlDbType.Int),
                                new SqlParameter("@ApplicationUid",  SqlDbType.NVarChar,50),
                                new SqlParameter("@DataDateTime", SqlDbType.DateTime),
                                new SqlParameter("@PollutantCode", SqlDbType.NVarChar,20),
                                new SqlParameter("@PollutantValue", SqlDbType.Decimal,18),
                                new SqlParameter("@AuditFlag", SqlDbType.NVarChar,20),
                                new SqlParameter("@CreatUser", SqlDbType.NVarChar,200)};
                        parameters[0].Value = dr["AuditStatusUid"].ToString();
                        parameters[1].Value = dr["PointId"].ToString();
                        parameters[2].Value = appUid;
                        parameters[3].Value = dr["TimePoint"].ToString();
                        parameters[4].Value = dr["PollutantCode"].ToString();
                        parameters[5].Value = dr["PollutantCode"].ToString() == "a21005" ? decimal.Parse(dr["VerifyValue"].ToString()) : decimal.Parse(dr["VerifyValue"].ToString()) / 1000;
                        parameters[6].Value = dr["VerifyMark"].ToString();
                        parameters[7].Value = "SystemSync";
                        CommandInfo cmd = new CommandInfo(strSql.ToString(), parameters);
                        sqllist.Add(cmd);
                    }
                    else
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("delete from  ");
                        strSql.Append(tableName);
                        strSql.Append(" where AuditStatusUid='");
                        strSql.Append(dr["AuditStatusUid"].ToString());
                        strSql.Append("' and DataDateTime='");
                        strSql.Append(dr["TimePoint"].ToString());
                        strSql.Append("' and PollutantCode='");
                        strSql.Append(dr["PollutantCode"].ToString());
                        strSql.Append("';");
                        strSql.Append("insert into ");
                        strSql.Append(tableName);
                        strSql.Append("(AuditStatusUid, PointId,ApplicationUid,DataDateTime,PollutantCode,PollutantValue,AuditFlag,CreatUser)");
                        strSql.Append("values(@AuditStatusUid,@PointId,@ApplicationUid,@DataDateTime,@PollutantCode,@PollutantValue,@AuditFlag,@CreatUser);");
                        SqlParameter[] parameters = { 
                                new SqlParameter("@AuditStatusUid", SqlDbType.NVarChar,50),
                                new SqlParameter("@PointId", SqlDbType.Int),
                                new SqlParameter("@ApplicationUid",  SqlDbType.NVarChar,50),
                                new SqlParameter("@DataDateTime", SqlDbType.DateTime),
                                new SqlParameter("@PollutantCode", SqlDbType.NVarChar,20),
                                new SqlParameter("@PollutantValue", SqlDbType.Decimal,18),
                                new SqlParameter("@AuditFlag", SqlDbType.NVarChar,20),
                                new SqlParameter("@CreatUser", SqlDbType.NVarChar,200)};
                        parameters[0].Value = dr["AuditStatusUid"].ToString();
                        parameters[1].Value = dr["PointId"].ToString();
                        parameters[2].Value = appUid;
                        parameters[3].Value = dr["TimePoint"].ToString();
                        parameters[4].Value = dr["PollutantCode"].ToString();
                        parameters[5].Value = dr["PollutantCode"].ToString() == "a21005" ? decimal.Parse(dr["VerifyValue"].ToString()) : decimal.Parse(dr["VerifyValue"].ToString()) / 1000;
                        parameters[6].Value = dr["VerifyMark"].ToString();
                        parameters[7].Value = "SystemSync";
                        CommandInfo cmd = new CommandInfo(strSql.ToString(), parameters);
                        sqllist.Add(cmd);
                    }
                }
                dbHelper.ExecuteSqlTranWithIndentity(sqllist, AMS_MonitorBusiness_Conn);
            }
            catch (Exception ex)
            {
            }
        }
        /// <summary>
        /// 更新日概况数据状态
        /// </summary>
        /// <param name="auditStatusUid">状态UID</param>
        /// <param name="appUid">应用类型UID</param>
        /// <param name="pointId">点位ID</param>
        /// <param name="date">数据时间</param>
        public void updateAuditStatusForDayDataStatus(string auditStatusUid)
        {
            String updateSql = string.Format(@"                                            
                                            update [Audit].[TB_AuditStatusForDay] 
                                               set [Status] = '1' 
                                             where [AuditStatusUid] = '{0}'"
                                           , auditStatusUid);

            dbHelper.ExecuteNonQuery(updateSql, AMS_MonitorBusiness_Conn);
        }

        #endregion
        /// <summary>
        /// 获取本地国控点审核日志
        /// </summary>
        /// <param name="date"></param>
        /// <param name="dateEnd"></param>
        /// <param name="PollutantCode"></param>
        /// <returns></returns>
        public DataTable LocalAuditAirLog(DateTime date, DateTime dateEnd, string PollutantCode)
        {
            try
            {
                string preDelSql = string.Format(@"
                                               select [AuditLogUid]
      ,[AuditStatusUid]
      ,[tstamp]
      ,CONVERT(datetime, CONVERT(varchar(100), AuditTime, 20)) as AuditTime
      ,[AuditType]
      ,[PollutantCode]
      ,[PollutantName]
      ,[SourcePollutantDataValue]
      ,[AuditPollutantDataValue]
      ,[OperationTypeEnum]
      ,[OperationReason]
      ,[UserIP]
      ,[UserUid]
      ,[Description]
      ,[OrderByNum]
      ,[CreatUser]
      ,[CreatDateTime]
      ,[UpdateUser]
      ,[UpdateDateTime]
    from [Audit].[TB_AuditAirLog] 
                                                where tstamp >= '{0}' 
                                                  and tstamp <  '{1}' and  PollutantCode in ('{2}')"
                         , date.ToString("yyyy-MM-dd 00:00:00.000")
                         , dateEnd.AddDays(1).ToString("yyyy-MM-dd 00:00:00.000")
                         , PollutantCode
                         );
                //先删除表中数据避免重复插入
                DataTable dt = dbHelper.ExecuteDataTable(preDelSql, AMS_MonitorBusiness_Conn);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region << 同步审核日志数据 >>
        /// <summary>
        /// 获取审核超过时间超过一天的日志
        /// </summary>
        /// <param name="SourcePort">点位ID(多个点位ID以英文,分割)</param>
        /// <param name="date"></param>
        /// <param name="dateEnd"></param>
        /// <param name="guoJiaTblName"></param>
        /// <param name="conn_db_str"></param>
        /// <returns></returns>
        public DataTable GuojiaAuditAirLog(string SourcePort, DateTime date, DateTime dateEnd,
                                        string guoJiaTblName, string conn_db_str)
        {
            try
            {
                String queryDataSql = string.Format(@"                                            
                                            select '' as  AuditStatusUid,
                                                    {0} as PollutantCode, 
                                                    PollutantName,
                                                    dateadd(hh, -1, TimePoint) as TimePoint, 
                                                    '数据审核' as AuditType,
                                                    {1} as PointId,
                                                    SrcValue,
	                                                SrcMark,
	                                                VerifyValue,
	                                                VerifyMark,
                                                    VerifyValue as VerifyValueMark,
	                                                VerifyTime,
	                                                Description,
	                                                VerifyPerson, 
                                                    VerifyDataType,
                                                    StationCode,
                                                    'SystemSync' as CreatUser 
                                              from {2} 
                                             where  TimePoint >= '{3}' 
                                               and TimePoint < '{4}' and VerifyDataType<>4 
                                               and DATEDIFF(day,TimePoint,VerifyTime)>1 and StationCode in ({5})
                                             order by  TimePoint desc"
                                               , mappingBiz.ConvertPollutantCodeToCaseSql(auditLogChannelInfo, "PollutantName")
                                               , mappingBiz.ConvertPointIdToCaseSql(portInfo, "StationCode")
                                               , guoJiaTblName
                                               , date.ToString("yyyy-MM-dd 01:00:00.000")
                                               , dateEnd.AddDays(1).ToString("yyyy-MM-dd 01:00:00.000")
                                               , SourcePort
                                               );
                DataTable dtGuoJia = dbHelper.ExecuteDataTable(queryDataSql, conn_db_str);
                //if (dtGuoJia.Rows.Count == 0)
                //{
                //    return null;
                //}
                string verifyMark = null;
                // 将国家的审核值和审核标识拼接成0.0390(RM)格式
                foreach (DataRow drMap in dtGuoJia.Rows)
                {
                    verifyMark = mappingBiz.GetConvertStatus(statusDic, drMap["VerifyMark"].ToString(), sourceStatusSplitChar, localStatusSplitChar, BusinessType_AuditLog);

                    if (!string.IsNullOrEmpty(verifyMark))
                    {
                        drMap["VerifyValueMark"] = new StringBuilder().Append(drMap["VerifyValue"].ToString()).Append("(").Append(verifyMark).Append(")");
                    }
                }
                return dtGuoJia;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 同步审核日志数据
        /// </summary>
        /// <param name="auditStatusUid">状态UID</param>
        /// <param name="SourcePort">国家点位ID</param>
        /// <param name="date">数据时间</param>
        /// <param name="guoJiaTblName">国家库审核日志完整表名</param>
        /// <param name="conn_db_str">数据库完整名</param>
        public void SyncAuditAirLogData(string auditStatusUid, string SourcePort, DateTime date,
                                        string guoJiaTblName, string conn_db_str)
        {
            String queryDataSql = string.Format(@"                                            
                                            select '{0}' as AuditStatusUid,
                                                    {1} as PollutantCode, 
                                                    PollutantName,
                                                    dateadd(hh, -1, TimePoint) as TimePoint, 
                                                    '数据审核' as AuditType,
                                                    SrcValue,
	                                                SrcMark,
	                                                VerifyValue,
	                                                VerifyMark,
	                                                VerifyTime,
	                                                Description,
	                                                VerifyPerson, 
                                                    'SystemSync' as CreatUser 
                                              from {2} 
                                             where TimePoint >= '{3}' 
                                               and TimePoint < '{4}'  and [StationCode]='{5}'
                                             order by TimePoint asc"
                                            , auditStatusUid
                                            , mappingBiz.ConvertPollutantCodeToCaseSql(auditLogChannelInfo, "PollutantName")
                                            , guoJiaTblName
                                            , date.ToString("yyyy-MM-dd 01:00:00.000")
                                            , date.AddDays(1).ToString("yyyy-MM-dd 01:00:00.000")
                                            , SourcePort
                                            );

            DataTable dtGuoJia = dbHelper.ExecuteDataTable(queryDataSql, conn_db_str);

            string preDelSql = string.Format(@"
                                               delete from [Audit].[TB_AuditAirLog] 
                                                where AuditStatusUid = '{0}' 
                                                  and tstamp >= '{1}' 
                                                  and tstamp <  '{2}'"
                                     , auditStatusUid
                                     , date.ToString("yyyy-MM-dd 00:00:00.000")
                                     , date.AddDays(1).ToString("yyyy-MM-dd 00:00:00.000")
                                     );
            //先删除表中数据避免重复插入
            dbHelper.ExecuteNonQuery(preDelSql, AMS_MonitorBusiness_Conn);

            if (dtGuoJia.Rows.Count == 0)
            {
                /*
                CommonFunction.WriteInfoLog("Exist Sync ([Audit].[TB_AuditAirLog]) Not Query Data, PortId=[" 
                                          + LocalPort + "] TotalCount=["
                                          + dtGuoJia.Rows.Count + "] Tstamp From (" + dtStart.ToString("yyyy-MM-dd HH:mm:ss")
                                          + ") To (" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss") + ")");
                */
                return;
            }

            string verifyMark = null;

            // 将国家的审核值和审核标识拼接成0.0390(RM)格式
            foreach (DataRow drMap in dtGuoJia.Rows)
            {
                verifyMark = mappingBiz.GetConvertStatus(statusDic, drMap["VerifyMark"].ToString(), sourceStatusSplitChar, localStatusSplitChar, BusinessType_AuditLog);

                if (!string.IsNullOrEmpty(verifyMark))
                {
                    drMap["VerifyValue"] = new StringBuilder().Append(drMap["VerifyValue"].ToString()).Append("(").Append(verifyMark).Append(")");
                }
            }

            /*
           CommonFunction.WriteInfoLog("Delete ([Audit].[TB_AuditAirLog]) Data Success PortId=[" + LocalPort
                                     + "] Tstamp From (" + dtStart.ToString("yyyy-MM-dd HH:mm:ss")
                                     + ") To (" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss") + ")");

           CommonFunction.WriteInfoLog("Begin Sync ([Audit].[TB_AuditAirInfectantByHour]) Data PortId=[" + LocalPort + "]");
            */

            // 添加数据
            string connStr = ConfigurationManager.ConnectionStrings[AMS_MonitorBusiness_Conn].ConnectionString;
            SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(connStr, SqlBulkCopyOptions.UseInternalTransaction);
            sqlbulkcopy.DestinationTableName = "[Audit].[TB_AuditAirLog]";//目标表表名
            sqlbulkcopy.ColumnMappings.Add("[AuditStatusUid]", "[AuditStatusUid]");
            sqlbulkcopy.ColumnMappings.Add("[TimePoint]", "[tstamp]");
            sqlbulkcopy.ColumnMappings.Add("[VerifyTime]", "[AuditTime]");
            sqlbulkcopy.ColumnMappings.Add("[AuditType]", "[AuditType]");
            sqlbulkcopy.ColumnMappings.Add("[PollutantCode]", "[PollutantCode]");
            sqlbulkcopy.ColumnMappings.Add("[PollutantName]", "[PollutantName]");
            sqlbulkcopy.ColumnMappings.Add("[SrcValue]", "[SourcePollutantDataValue]");
            sqlbulkcopy.ColumnMappings.Add("[VerifyValue]", "[AuditPollutantDataValue]");
            sqlbulkcopy.ColumnMappings.Add("[Description]", "[Description]");
            sqlbulkcopy.ColumnMappings.Add("[VerifyPerson]", "[CreatUser]");
            sqlbulkcopy.ColumnMappings.Add("[VerifyPerson]", "[UpdateUser]");

            sqlbulkcopy.WriteToServer(dtGuoJia);
            sqlbulkcopy.Close();

            /*
            CommonFunction.WriteInfoLog("End Sync ([Audit].[TB_AuditAirLog]) Data Success PortId=[" + LocalPort
                                      + "] TotalCount=[" + dtGuoJia.Rows.Count + "] Tstamp From ("
                                      + dtStart.ToString("yyyy-MM-dd HH:mm:ss") + ") To ("
                                      + dtEnd.ToString("yyyy-MM-dd HH:mm:ss") + ")");
            */
        }
        #endregion
        /// <summary>
        /// 更新本地日志信息
        /// </summary>
        /// <param name="dtGuoJia"></param>
        public void copyAuditLog(DataTable dtGuoJia)
        {

            // 添加数据
            string connStr = ConfigurationManager.ConnectionStrings[AMS_MonitorBusiness_Conn].ConnectionString;
            SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(connStr, SqlBulkCopyOptions.UseInternalTransaction);
            sqlbulkcopy.DestinationTableName = "[Audit].[TB_AuditAirLog]";//目标表表名
            sqlbulkcopy.ColumnMappings.Add("[AuditStatusUid]", "[AuditStatusUid]");
            sqlbulkcopy.ColumnMappings.Add("[TimePoint]", "[tstamp]");
            sqlbulkcopy.ColumnMappings.Add("[VerifyTime]", "[AuditTime]");
            sqlbulkcopy.ColumnMappings.Add("[AuditType]", "[AuditType]");
            sqlbulkcopy.ColumnMappings.Add("[PollutantCode]", "[PollutantCode]");
            sqlbulkcopy.ColumnMappings.Add("[PollutantName]", "[PollutantName]");
            sqlbulkcopy.ColumnMappings.Add("[SrcValue]", "[SourcePollutantDataValue]");
            sqlbulkcopy.ColumnMappings.Add("[VerifyValueMark]", "[AuditPollutantDataValue]");
            sqlbulkcopy.ColumnMappings.Add("[Description]", "[Description]");
            sqlbulkcopy.ColumnMappings.Add("[VerifyPerson]", "[CreatUser]");
            sqlbulkcopy.ColumnMappings.Add("[VerifyPerson]", "[UpdateUser]");

            sqlbulkcopy.WriteToServer(dtGuoJia);
            sqlbulkcopy.Close();
        }
        #region << 同步AQI数据 >>
        /// <summary>
        /// 同步一个点位RTD_DayAQI数据
        /// </summary>
        /// <param name="LocalPortId">远大库点位ID</param>
        /// <param name="SourcePortId">国家库点位ID</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        public void SyncOnePointRTD_DayAQI(String LocalPortId, String SourcePortId,
                                           DateTime dtStart, DateTime dtEnd)
        {
            try
            {
                string linkServerName = System.Configuration.ConfigurationManager.AppSettings["airLinkServiceName"].ToString();
                string linkDBName = System.Configuration.ConfigurationManager.AppSettings["airLinkDBName"].ToString();
                string IsUseLinkServer = System.Configuration.ConfigurationManager.AppSettings["IsUseLinkService"].ToString();

                string fullTableNameByLink = string.Format("[{0}].[{1}].dbo.Air_day_AQI_App ",
                                                              linkServerName,
                                                              linkDBName);

                string fullTableName = "dbo.Air_day_AQI_App";
                string conn_db_str = GuoJia_Air_Conn;

                //是否启用Link链接（0: 不启用，1：启用）
                if ("1".Equals(IsUseLinkServer))
                {// 启用Link链接
                    fullTableName = fullTableNameByLink;
                    conn_db_str = AMS_MonitorBusiness_Conn;
                }

                String queryDataSql = string.Format(@"select {0} as portId, [Date] ,[SO2]
		                                                    ,[PM2_5] ,[PM10] ,[O3_8h]
		                                                    ,[O3] ,[NO2] ,[CO]
		                                                    ,[AQI] ,[PrimaryPollutant] ,[Type] ,[Level] 
                                                        from {1} 
                                                       where StationCode='{2}' 
                                                         and Date >= '{3}' 
                                                         and Date <  '{4}'"
                                                            , LocalPortId
                                                            , fullTableName
                                                            , SourcePortId
                                                            , dtStart.ToString("yyyy-MM-dd HH:mm:ss")
                                                            , dtEnd.ToString("yyyy-MM-dd HH:mm:ss")
                                                            );

                DataTable guoJiaAqiDT = dbHelper.ExecuteDataTable(queryDataSql, conn_db_str);

                if (guoJiaAqiDT == null || guoJiaAqiDT.Rows.Count == 0)
                {// 无数据可同步，跳过本次同步
                    /*
                    CommonFunction.WriteInfoLog("Exist Sync ([AirRelease].[TB_DayAQI]) Not Query Data, PortId=[" + LocalPortId + "] TotalCount=["
                                              + guoJiaAqiDT.Rows.Count + "] Tstamp From (" + beginTime.ToString("yyyy-MM-dd HH:mm:ss")
                                              + ") To (" + endTime.ToString("yyyy-MM-dd HH:mm:ss") + ")");
                     */
                    return;
                }

                string preDelSql = string.Format(@"delete from [AirRelease].[TB_DayAQI] 
                                                    where PointId = {0} 
                                                      and DateTime >= '{1}' 
                                                      and DateTime < '{2}'"
                                                  , LocalPortId
                                                  , dtStart.ToString("yyyy-MM-dd HH:mm:ss")
                                                  , dtEnd.ToString("yyyy-MM-dd HH:mm:ss")
                                                  );
                //先删除表中数据避免重复插入
                dbHelper.ExecuteNonQuery(preDelSql, AMS_MonitorBusiness_Conn);

                /*
                CommonFunction.WriteInfoLog("Delete ([AirRelease].[TB_DayAQI]) Data Success PortId=[" + LocalPortId + "] Tstamp From ("
                                             + beginTime.ToString("yyyy-MM-dd HH:mm:ss")
                                             + ") To (" + endTime.ToString("yyyy-MM-dd HH:mm:ss") + ")");

                CommonFunction.WriteInfoLog("Begin Sync ([AirRelease].[TB_DayAQI]) Data PortId=[" + LocalPortId + "]");
                */

                // 添加数据
                string connStr = ConfigurationManager.ConnectionStrings[AMS_MonitorBusiness_Conn].ConnectionString;
                SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(connStr, SqlBulkCopyOptions.UseInternalTransaction);
                sqlbulkcopy.DestinationTableName = "[AirRelease].[TB_DayAQI]";//目标表表名
                sqlbulkcopy.ColumnMappings.Add("portId", "[PointId]");
                sqlbulkcopy.ColumnMappings.Add("[Date]", "[DateTime]");
                sqlbulkcopy.ColumnMappings.Add("[SO2]", "[SO2]");
                sqlbulkcopy.ColumnMappings.Add("[PM2_5]", "[PM25]");
                sqlbulkcopy.ColumnMappings.Add("[PM10]", "[PM10]");
                sqlbulkcopy.ColumnMappings.Add("[O3_8h]", "[Max8HourO3]");
                sqlbulkcopy.ColumnMappings.Add("[O3]", "[MaxOneHourO3]");
                sqlbulkcopy.ColumnMappings.Add("[NO2]", "[NO2]");
                sqlbulkcopy.ColumnMappings.Add("[CO]", "[CO]");
                sqlbulkcopy.ColumnMappings.Add("[AQI]", "[AQIValue]");
                sqlbulkcopy.ColumnMappings.Add("[PrimaryPollutant]", "[PrimaryPollutant]");
                sqlbulkcopy.WriteToServer(guoJiaAqiDT);
                sqlbulkcopy.Close();

                /*
                CommonFunction.WriteInfoLog("End Sync ([AirRelease].[TB_DayAQI]) Data Success PortId=[" + LocalPortId + "] TotalCount=["
                                           + guoJiaAqiDT.Rows.Count + "] Tstamp From (" + beginTime.ToString("yyyy-MM-dd HH:mm:ss")
                                           + ") To (" + endTime.ToString("yyyy-MM-dd HH:mm:ss") + ")");
                */

                //更新AQI操作
                String updateAqiValSql = string.Format(@"update [AirRelease].[TB_DayAQI] 
                                                            set [SO2_IAQI] = [dbo].f_getAQI([SO2], 'a21026', 24), 
                                                                [NO2_IAQI] = [dbo].f_getAQI([NO2], 'a21004', 24), 
                                                                [PM10_IAQI] = [dbo].f_getAQI([PM10], 'a34002', 24), 
                                                                [CO_IAQI] = [dbo].f_getAQI([CO], 'a21005', 24), 
                                                                [MaxOneHourO3_IAQI] = [dbo].f_getAQI([MaxOneHourO3], 'a05024', 1), 
                                                                [Max8HourO3_IAQI] = [dbo].f_getAQI([Max8HourO3], 'a05024', 8), 
                                                                [PM25_IAQI] = [dbo].f_getAQI([PM25], 'a34004', 24)
                                                          where PointId = {0} 
                                                            and DateTime >= '{1}' 
                                                            and DateTime < '{2}'"
                                                          , LocalPortId
                                                          , dtStart.ToString("yyyy-MM-dd HH:mm:ss")
                                                          , dtEnd.ToString("yyyy-MM-dd HH:mm:ss")
                                                          );

                //更新数据库中的因子AQI指标值
                dbHelper.ExecuteNonQuery(updateAqiValSql, AMS_MonitorBusiness_Conn);

                updateAqiValSql = string.Format(@"update [AirRelease].[TB_DayAQI] 
                                                     set [PrimaryPollutant] = dbo.F_GetAQI_Max_CNV_Day([SO2_IAQI], [NO2_IAQI], [PM10_IAQI], [CO_IAQI], [Max8HourO3_IAQI], [PM25_IAQI], 'N'),
                                                         [AQIValue] = dbo.F_GetAQI_Max_CNV_Day([SO2_IAQI], [NO2_IAQI], [PM10_IAQI], [CO_IAQI], [Max8HourO3_IAQI], [PM25_IAQI], 'V') 
                                                   where PointId = {0} 
                                                     and DateTime >= '{1}' 
                                                     and DateTime <  '{2}'"
                                                        , LocalPortId
                                                        , dtStart.ToString("yyyy-MM-dd HH:mm:ss")
                                                        , dtEnd.ToString("yyyy-MM-dd HH:mm:ss")
                                                        );

                //更新数据库中的因子AQI指标值
                dbHelper.ExecuteNonQuery(updateAqiValSql, AMS_MonitorBusiness_Conn);

                updateAqiValSql = string.Format(@"update [AirRelease].[TB_DayAQI] 
                                                    set [RANGE] = [dbo].F_GetAQI_Grade([AQIValue], 'RANGE'), 
                                                        [GRADE] = [dbo].F_GetAQI_Grade([AQIValue], 'GRADE'), 
                                                        [CLASS] = [dbo].F_GetAQI_Grade([AQIValue], 'CLASS'),  
                                                        [RGBValue] = [dbo].F_GetAQI_Grade([AQIValue], 'RGBValue'),  
                                                        [HEAlTHEFFECT] = [dbo].F_GetAQI_Grade([AQIValue], 'HEAlTHEFFECT'),  
                                                        [TAKESTEP] = [dbo].F_GetAQI_Grade([AQIValue], 'TAKESTEP')
                                                  where PointId = {0} 
                                                    and DateTime >= '{1}' 
                                                    and DateTime <  '{2}'"
                                                        , LocalPortId
                                                        , dtStart.ToString("yyyy-MM-dd HH:mm:ss")
                                                        , dtEnd.ToString("yyyy-MM-dd HH:mm:ss")
                                                        );

                //更新数据库中的因子AQI指标值
                dbHelper.ExecuteNonQuery(updateAqiValSql, AMS_MonitorBusiness_Conn);

                /*
                CommonFunction.WriteInfoLog("End Update ([AirRelease].[TB_DayAQI]) Data Success PortId=[" + LocalPortId + "] TotalCount=["
                                           + guoJiaAqiDT.Rows.Count + "] Tstamp From (" + beginTime.ToString("yyyy-MM-dd HH:mm:ss")
                                           + ") To (" + endTime.ToString("yyyy-MM-dd HH:mm:ss") + ")");
                 */
            }
            catch (Exception ex)
            {
                //CommonFunction.WriteErrorLog("End Sync ([AirRelease].[TB_DayAQI]) Data Error: " + ex.Message);
            }
        }

        /// <summary>
        /// 按时间区段同步国家RTD_CityDayAQI数据
        /// </summary>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        public void SyncRTD_CityDayAQI(DateTime dtStart, DateTime dtEnd)
        {
            try
            {
                string linkServerName = System.Configuration.ConfigurationManager.AppSettings["airLinkServiceName"].ToString();
                string linkDBName = System.Configuration.ConfigurationManager.AppSettings["airLinkDBName"].ToString();
                string IsUseLinkServer = System.Configuration.ConfigurationManager.AppSettings["IsUseLinkService"].ToString();

                string fullTableNameByLink = string.Format("[{0}].[{1}].dbo.Air_Cityday_AQI_App",
                                                              linkServerName,
                                                              linkDBName);

                string fullTableName = "dbo.Air_Cityday_AQI_App";
                string conn_db_str = GuoJia_Air_Conn;

                //是否启用Link链接（0: 不启用，1：启用）
                if ("1".Equals(IsUseLinkServer))
                {// 启用Link链接
                    fullTableName = fullTableNameByLink;
                    conn_db_str = AMS_MonitorBusiness_Conn;
                }

                String regionUid = System.Configuration.ConfigurationManager.AppSettings["CityAQI_MonitoringRegionUid"].ToString();

                String queryDataSql = string.Format(@"select '{0}' as RegionUid 
                                                            ,'CG' as StatisticalType
                                                            ,[Date], [SO2], [NO2]
		                                                    ,[PM10], [CO], [O3_1h]
                                                            ,[O3_8h], [PM2_5], [AQI]
		                                                    ,[PrimaryPollutant] ,[Type] ,[Level] 
                                                        from {1}  
                                                       where Date >= '{2}' 
                                                         and Date <  '{3}'"
                                                            , regionUid
                                                            , fullTableName
                                                            , dtStart.ToString("yyyy-MM-dd HH:mm:ss")
                                                            , dtEnd.ToString("yyyy-MM-dd HH:mm:ss")
                                                            );

                DataTable guoJiaAqiDT = dbHelper.ExecuteDataTable(queryDataSql, conn_db_str);

                if (guoJiaAqiDT == null || guoJiaAqiDT.Rows.Count == 0)
                {// 无数据可同步，跳过本次同步
                    /*
                    CommonFunction.WriteInfoLog("Exist Sync ([AirReport].[TB_RegionDayAQIReport]) Not Query Data, TotalCount=["
                                              + guoJiaAqiDT.Rows.Count + "] Tstamp From (" + beginTime.ToString("yyyy-MM-dd HH:mm:ss")
                                              + ") To (" + endTime.ToString("yyyy-MM-dd HH:mm:ss") + ")");
                     */
                    return;
                }

                string preDelSql = string.Format(@"delete from [AirReport].[TB_RegionDayAQIReport]  
                                                    where MonitoringRegionUid = '{0}' 
                                                      and ReportDateTime >= '{1}' 
                                                      and ReportDateTime < '{2}'"
                                                     , regionUid
                                                     , dtStart.ToString("yyyy-MM-dd HH:mm:ss")
                                                     , dtEnd.ToString("yyyy-MM-dd HH:mm:ss")
                                                     );
                //先删除表中数据避免重复插入
                dbHelper.ExecuteNonQuery(preDelSql, AMS_MonitorBusiness_Conn);

                /*
                CommonFunction.WriteInfoLog("Delete ([AirReport].[TB_RegionDayAQIReport]) Data Success Tstamp From ("
                                         + beginTime.ToString("yyyy-MM-dd HH:mm:ss")
                                         + ") To (" + endTime.ToString("yyyy-MM-dd HH:mm:ss") + ")");

                CommonFunction.WriteInfoLog("Begin Sync ([AirReport].[TB_RegionDayAQIReport]) Data");
                */

                // 添加数据
                string connStr = ConfigurationManager.ConnectionStrings[AMS_MonitorBusiness_Conn].ConnectionString;
                SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(connStr, SqlBulkCopyOptions.UseInternalTransaction);

                sqlbulkcopy.DestinationTableName = "[AirReport].[TB_RegionDayAQIReport]";//目标表表名
                sqlbulkcopy.ColumnMappings.Add("[RegionUid]", "[MonitoringRegionUid]");
                sqlbulkcopy.ColumnMappings.Add("[Date]", "[ReportDateTime]");
                sqlbulkcopy.ColumnMappings.Add("[StatisticalType]", "[StatisticalType]");
                sqlbulkcopy.ColumnMappings.Add("[SO2]", "[SO2]");
                sqlbulkcopy.ColumnMappings.Add("[NO2]", "[NO2]");
                sqlbulkcopy.ColumnMappings.Add("[PM10]", "[PM10]");
                sqlbulkcopy.ColumnMappings.Add("[CO]", "[CO]");
                sqlbulkcopy.ColumnMappings.Add("[O3_1h]", "[MaxOneHourO3]");
                sqlbulkcopy.ColumnMappings.Add("[O3_8h]", "[Max8HourO3]");
                sqlbulkcopy.ColumnMappings.Add("[PM2_5]", "[PM25]");
                sqlbulkcopy.ColumnMappings.Add("[AQI]", "[AQIValue]");
                sqlbulkcopy.ColumnMappings.Add("[PrimaryPollutant]", "[PrimaryPollutant]");

                sqlbulkcopy.WriteToServer(guoJiaAqiDT);
                sqlbulkcopy.Close();

                /*
                CommonFunction.WriteInfoLog("End Sync ([AirReport].[TB_RegionDayAQIReport]) Data Success TotalCount=["
                                          + guoJiaAqiDT.Rows.Count + "] Tstamp From (" + beginTime.ToString("yyyy-MM-dd HH:mm:ss")
                                          + ") To (" + endTime.ToString("yyyy-MM-dd HH:mm:ss") + ")");
                */

                String updateAqiValSql = string.Format(@"update [AirReport].[TB_RegionDayAQIReport] 
                                                        set [SO2_IAQI] = [dbo].f_getAQI([SO2], 'a21026', 24), 
                                                            [NO2_IAQI] = [dbo].f_getAQI([NO2], 'a21004', 24), 
                                                            [PM10_IAQI] = [dbo].f_getAQI([PM10], 'a34002', 24), 
                                                            [CO_IAQI] = [dbo].f_getAQI([CO], 'a21005', 24), 
                                                            [MaxOneHourO3_IAQI] = [dbo].f_getAQI([MaxOneHourO3], 'a05024', 1), 
                                                            [Max8HourO3_IAQI] = [dbo].f_getAQI([Max8HourO3], 'a05024', 8), 
                                                            [PM25_IAQI] = [dbo].f_getAQI([PM25], 'a34004', 24)
                                                      where MonitoringRegionUid = '{0}'
                                                        and ReportDateTime >= '{1}' 
                                                        and ReportDateTime < '{2}'"
                                                          , regionUid
                                                          , dtStart.ToString("yyyy-MM-dd HH:mm:ss")
                                                          , dtEnd.ToString("yyyy-MM-dd HH:mm:ss")
                                                          );

                //更新数据库中的因子AQI指标值
                dbHelper.ExecuteNonQuery(updateAqiValSql, AMS_MonitorBusiness_Conn);

                updateAqiValSql = string.Format(@"update [AirReport].[TB_RegionDayAQIReport]  
                                                     set [PrimaryPollutant] = dbo.F_GetAQI_Max_CNV_Day([SO2_IAQI], [NO2_IAQI], [PM10_IAQI], [CO_IAQI], [Max8HourO3_IAQI], [PM25_IAQI], 'N'),
                                                         [AQIValue] = dbo.F_GetAQI_Max_CNV_Day([SO2_IAQI], [NO2_IAQI], [PM10_IAQI], [CO_IAQI], [Max8HourO3_IAQI], [PM25_IAQI], 'V')
                                                   where MonitoringRegionUid = '{0}' 
                                                     and ReportDateTime >= '{1}' 
                                                     and ReportDateTime < '{2}'"
                                                        , regionUid
                                                        , dtStart.ToString("yyyy-MM-dd HH:mm:ss")
                                                        , dtEnd.ToString("yyyy-MM-dd HH:mm:ss")
                                                        );

                //更新数据库中的因子AQI指标值
                dbHelper.ExecuteNonQuery(updateAqiValSql, AMS_MonitorBusiness_Conn);

                updateAqiValSql = string.Format(@"update [AirReport].[TB_RegionDayAQIReport]  
                                                    set [RANGE] = [dbo].F_GetAQI_Grade([AQIValue], 'RANGE'), 
                                                        [GRADE] = [dbo].F_GetAQI_Grade([AQIValue], 'GRADE'),
                                                        [CLASS] = [dbo].F_GetAQI_Grade([AQIValue], 'CLASS'),  
                                                        [RGBValue] = [dbo].F_GetAQI_Grade([AQIValue], 'RGBValue'),  
                                                        [HEAlTHEFFECT] = [dbo].F_GetAQI_Grade([AQIValue], 'HEAlTHEFFECT'),  
                                                        [TAKESTEP] = [dbo].F_GetAQI_Grade([AQIValue], 'TAKESTEP')
                                                  where MonitoringRegionUid = '{0}' 
                                                     and ReportDateTime >= '{1}' 
                                                     and ReportDateTime < '{2}'"
                                                        , regionUid
                                                        , dtStart.ToString("yyyy-MM-dd HH:mm:ss")
                                                        , dtEnd.ToString("yyyy-MM-dd HH:mm:ss")
                                                        );

                //更新数据库中的因子AQI指标值
                dbHelper.ExecuteNonQuery(updateAqiValSql, AMS_MonitorBusiness_Conn);

                /*
                CommonFunction.WriteInfoLog("End Update ([AirReport].[TB_RegionDayAQIReport]) Data Success TotalCount=["
                                           + guoJiaAqiDT.Rows.Count + "] Tstamp From (" + beginTime.ToString("yyyy-MM-dd HH:mm:ss")
                                           + ") To (" + endTime.ToString("yyyy-MM-dd HH:mm:ss") + ")");
                 */
            }
            catch (Exception ex)
            {
                //CommonFunction.WriteErrorLog("End Sync ([AirReport].[TB_RegionDayAQIReport]) Data Error: " + ex.Message);
            }
        }
        #endregion
    }
}
