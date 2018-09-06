using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Data.SqlServer.Common.GridView;
using SmartEP.Utilities.AdoData;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.MonitoringBusiness.Air
{
    /// <summary>
    /// 名称：AuditInfectantByHourDAL.cs
    /// 创建人：李飞
    /// 创建日期：2015-08-30
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 审核小时数据库处理
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class AuditAirInfectantByHourDAL
    {
        #region << 变量 >>
        /// <summary>
        /// 数据库处理类
        /// </summary>
        DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();

        /// <summary>
        /// 虚拟分页类
        /// </summary>
        GridViewPagerDAL g_GridViewPager = Singleton<GridViewPagerDAL>.GetInstance();

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connection = "AMS_MonitoringBusinessConnection";

        /// <summary>
        /// 数据库表名
        /// </summary>
        private string tableName = "Audit.TB_AuditAirInfectantByHour";
        #endregion

        #region << 构造函数 >>
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="applicationType">应用类型（空气、地表水）</param>
        public AuditAirInfectantByHourDAL()
        {
            connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(ApplicationType.Air, PollutantDataType.Hour);
        }
        #endregion

        #region << 方法 >>
        /// <summary>
        /// 取得行转列数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="isAllDate">是否补充缺失数据</param>
        /// <returns></returns>
        public DataView GetDataView(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd, bool isAllDate = false, string PointType = "0")
        {
            try
            {
                string portIdsStr = string.Empty;
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND dayInfo.PointId =" + portIds[0];
                }
                else if (portIds.Length > 1)
                {
                    portIdsStr = "AND dayInfo.PointId IN(" + StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",") + ")";
                }

                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                string factorMarkSql = string.Empty;
                string factorIsAuditSql = string.Empty;
                string factorSel = string.Empty;
                string factorFlagSel = string.Empty;
                string factorMarkSel = string.Empty;
                string factorIsAuditSel = string.Empty;


                foreach (string factor in factors)
                {
                    if (!factor.ToString().Equals(""))
                    {
                        string factorDataFlag = factor + "_dataFlag";
                        string factorAuditFlag = factor + "_AuditFlag";
                        string factorIsAudit = factor + "_IsAudit";
                        factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                        factorFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN dataFlag END) AS [{1}] ", factor, factorDataFlag);
                        factorMarkSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN AuditFlag END) AS [{1}] ", factor, factorAuditFlag);
                        factorIsAuditSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN IsAudit END) AS [{1}] ", factor, factorIsAudit);
                        factorSel += string.Format(",[{0}]", factor);
                        factorFlagSel += string.Format(",[{0}]", factorDataFlag);
                        factorMarkSel += string.Format(",[{0}]", factorAuditFlag);
                        factorIsAuditSel += string.Format(",[{0}]", factorIsAudit);
                    }
                }
                //,(CASE WHEN MAX(hourStatus.[Status]) is not null THEN MAX(hourStatus.[Status]) ELSE CASE WHEN Count(hourInfo.DataDateTime)>0 THEN '0' ELSE '' END END) AS AuditStatus                  
                        
                string sql = string.Format(@"
                    SELECT dayInfo.PointId
                        ,hourInfo.DataDateTime
                        ,MAX(CASE WHEN hourInfo.[IsAudit] IS NOT NULL THEN hourInfo.[IsAudit] ELSE 0 END) AS AuditStatus    
                        {0}
                        {1}
                        {2}
                        {3}
                    FROM {4} AS hourInfo
                    INNER JOIN Audit.TB_AuditStatusForDay AS dayInfo
	                    ON hourInfo.AuditStatusUid = dayInfo.AuditStatusUid  
                    INNER JOIN Audit.TB_AuditAirStatusForHour AS hourStatus
                        ON hourInfo.PointId=hourStatus.PointId and  hourInfo.DataDateTime=hourStatus.[Date]                        
                    WHERE hourInfo.DataDateTime>='{5}' 
                        AND hourInfo.DataDateTime<='{6}'
                        AND dayInfo.PointId is not NULL
                        AND hourStatus.[PointIdType]='{8}'
	                    {7}
                    GROUP BY dayInfo.PointId,hourInfo.DataDateTime
                    ", factorSql, factorFlagSql, factorMarkSql, factorIsAuditSql, tableName, dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr, PointType);

                //是否自动补充缺失数据
                if (isAllDate && portIds != null && portIds.Length > 0)
                {
                    sql = string.Format(@"
                    WITH hourData AS
                    (
                        {0}
                    )
                    SELECT allInfo.PointID
                        ,allInfo.tstamp as DataDateTime
                        ,AuditStatus
                        {1}
                        {2}
                        {3}
                        {4}
                    FROM dbo.F_GetAllDataByHour('{5}',',','{6}','{7}') AS allInfo
                    LEFT JOIN hourData
	                    ON allInfo.PointID = hourData.PointId AND SUBSTRING(CONVERT(varchar(100), allInfo.tstamp, 20),0,14) = SUBSTRING(CONVERT(varchar(100), hourData.DataDateTime, 20),0,14)
                    ORDER BY allInfo.PointID,allInfo.tstamp
                    ", sql, factorSel, factorFlagSel, factorMarkSel, factorIsAuditSel
                     , StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",")
                     , dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"));
                    //ON allInfo.PointID = hourData.PointId AND allInfo.tstamp = hourData.DataDateTime
                }
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 取得行转列数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="isAllDate">是否补充缺失数据</param>
        /// <returns></returns>
        public DataView GetDataViewS(string[] portIds, string InsId, string[] factors, DateTime dtStart, DateTime dtEnd, bool isAllDate = false, string PointType = "0")
        {
            try
            {
                //if (InsId.Equals("6e4aa38a-f68b-490b-9cd7-3b92c7805c2d"))
                //{
                //    InsId = "d2747011-ff8d-4b04-a006-c32ecaad4507";
                //}
                string portIdsStr = string.Empty;
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND dayInfo.PointId =" + portIds[0];
                }
                else if (portIds.Length > 1)
                {
                    portIdsStr = "AND dayInfo.PointId IN(" + StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",") + ")";
                }

                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                string factorMarkSql = string.Empty;
                string factorIsAuditSql = string.Empty;
                string factorSel = string.Empty;
                string factorFlagSel = string.Empty;
                string factorMarkSel = string.Empty;
                string factorIsAuditSel = string.Empty;
                string factorWhere = "'" + string.Join("','", factors) + "'";

                foreach (string factor in factors)
                {
                    if (!factor.ToString().Equals(""))
                    {
                        string factorDataFlag = factor + "_dataFlag";
                        string factorAuditFlag = factor + "_AuditFlag";
                        string factorIsAudit = factor + "_IsAudit";
                        if (InsId.Equals("3745f768-a789-4d58-9578-9e41fde5e5f0"))
                        {
                            factorSql += string.Format(",MAX(CASE (PollutantCode) WHEN '{0}' THEN CASE WHEN Datename(hour,DataDateTime)=0 THEN Null ELSE PollutantValue END END) AS [{0}] ", factor);
                            factorFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN CASE WHEN Datename(hour,DataDateTime)=0 THEN Null ELSE dataFlag END END) AS [{1}] ", factor, factorDataFlag);
                            factorMarkSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN CASE WHEN Datename(hour,DataDateTime)=0 THEN Null ELSE AuditFlag END END) AS [{1}] ", factor, factorAuditFlag);
                            factorIsAuditSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN  CASE WHEN Datename(hour,DataDateTime)=0 THEN Null ELSE IsAudit END END) AS [{1}] ", factor, factorIsAudit);
                        }
                        else
                        {
                            factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                            factorFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN dataFlag END) AS [{1}] ", factor, factorDataFlag);
                            factorMarkSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN AuditFlag END) AS [{1}] ", factor, factorAuditFlag);
                            factorIsAuditSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN IsAudit END) AS [{1}] ", factor, factorIsAudit);
                        }
                        //factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                        //factorFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN dataFlag END) AS [{1}] ", factor, factorDataFlag);
                        //factorMarkSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN AuditFlag END) AS [{1}] ", factor, factorAuditFlag);
                        //factorIsAuditSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN IsAudit END) AS [{1}] ", factor, factorIsAudit);
                        factorSel += string.Format(",[{0}]", factor);
                        factorFlagSel += string.Format(",[{0}]", factorDataFlag);
                        factorMarkSel += string.Format(",[{0}]", factorAuditFlag);
                        factorIsAuditSel += string.Format(",[{0}]", factorIsAudit);
                    }
                }
                #region 原方法
                //                string sql = string.Format(@"
//                    SELECT dayInfo.PointId
//                        ,hourInfo.DataDateTime
//                        ,(CASE WHEN MAX(hourStatus.[Status]) is not null THEN MAX(hourStatus.[Status]) ELSE CASE WHEN Count(hourInfo.DataDateTime)>0 THEN '0' ELSE '' END END) AS AuditStatus                  
//                        {0}
//                        {1}
//                        {2}
//                        {3}
//                    FROM {4} AS hourInfo
//                    INNER JOIN Audit.TB_AuditStatusForDay AS dayInfo
//	                    ON hourInfo.AuditStatusUid = dayInfo.AuditStatusUid  
//                    INNER JOIN Audit.TB_AuditAirStatusForHour AS hourStatus
//                        ON hourInfo.PointId=hourStatus.PointId and  hourInfo.DataDateTime=hourStatus.[Date]                        
//                    WHERE hourInfo.DataDateTime>='{5}' 
//                        AND hourInfo.DataDateTime<='{6}'
//                        AND dayInfo.PointId is not NULL
//                        AND hourStatus.[PointIdType]='{8}'
//AND hourStatus.[Description]='{9}' AND hourInfo.PollutantCode in ({10})
//	                    {7}
//                    GROUP BY dayInfo.PointId,hourInfo.DataDateTime
//                    ", factorSql, factorFlagSql, factorMarkSql, factorIsAuditSql, tableName, dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr, PointType, InsId, factorWhere);

                //,MAX(CASE WHEN hourInfo.[IsAudit] IS NOT NULL THEN hourInfo.[IsAudit] ELSE 0 END) AS AuditStatus
                #endregion
                string sql = "";
                if (InsId.Equals("3745f768-a789-4d58-9578-9e41fde5e5f0"))
                {
                    sql = string.Format(@"
                    SELECT dayInfo.PointId
                        ,hourInfo.DataDateTime
                        ,MAX(CASE WHEN hourInfo.[IsAudit] IS NOT NULL THEN(CASE WHEN Datename(hour,DataDateTime)=0 THEN Null ELSE  hourInfo.[IsAudit] END) ELSE 0 END) AS AuditStatus
                        {0}
                        {1}
                        {2}
                        {3}
                    FROM {4} AS hourInfo
                    INNER JOIN Audit.TB_AuditStatusForDay AS dayInfo
	                    ON hourInfo.AuditStatusUid = dayInfo.AuditStatusUid  
                    INNER JOIN Audit.TB_AuditAirStatusForHour AS hourStatus
                        ON hourInfo.PointId=hourStatus.PointId and  hourInfo.DataDateTime=hourStatus.[Date]                        
                    WHERE hourInfo.DataDateTime>='{5}' 
                        AND hourInfo.DataDateTime<='{6}'
                        AND dayInfo.PointId is not NULL
                        AND hourStatus.[PointIdType]='{8}'
AND hourStatus.[Description]='{9}' AND hourInfo.PollutantCode in ({10})
	                    {7}
                    GROUP BY dayInfo.PointId,hourInfo.DataDateTime
                    ", factorSql, factorFlagSql, factorMarkSql, factorIsAuditSql, tableName, dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr, PointType, InsId, factorWhere);

                }
                else
                {
                    sql = string.Format(@"
                    SELECT dayInfo.PointId
                        ,hourInfo.DataDateTime
                       ,MAX(CASE WHEN hourInfo.[IsAudit] IS NOT NULL THEN hourInfo.[IsAudit] ELSE 0 END) AS AuditStatus
                        {0}
                        {1}
                        {2}
                        {3}
                    FROM {4} AS hourInfo
                    INNER JOIN Audit.TB_AuditStatusForDay AS dayInfo
	                    ON hourInfo.AuditStatusUid = dayInfo.AuditStatusUid  
                    INNER JOIN Audit.TB_AuditAirStatusForHour AS hourStatus
                        ON hourInfo.PointId=hourStatus.PointId and  hourInfo.DataDateTime=hourStatus.[Date]                        
                    WHERE hourInfo.DataDateTime>='{5}' 
                        AND hourInfo.DataDateTime<='{6}'
                        AND dayInfo.PointId is not NULL
                        AND hourStatus.[PointIdType]='{8}'
AND hourStatus.[Description]='{9}' AND hourInfo.PollutantCode in ({10})
	                    {7}
                    GROUP BY dayInfo.PointId,hourInfo.DataDateTime
                    ", factorSql, factorFlagSql, factorMarkSql, factorIsAuditSql, tableName, dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr, PointType, InsId, factorWhere);

                }
                
                //是否自动补充缺失数据
                if (isAllDate && portIds != null && portIds.Length > 0)
                {
                    sql = string.Format(@"
                    WITH hourData AS
                    (
                        {0}
                    )
                    SELECT allInfo.PointID
                        ,allInfo.tstamp as DataDateTime
                        ,AuditStatus
                        {1}
                        {2}
                        {3}
                        {4}
                    FROM dbo.F_GetAllDataByHour('{5}',',','{6}','{7}') AS allInfo
                    LEFT JOIN hourData
	                    ON allInfo.PointID = hourData.PointId AND SUBSTRING(CONVERT(varchar(100), allInfo.tstamp, 20),0,14) = SUBSTRING(CONVERT(varchar(100), hourData.DataDateTime, 20),0,14)
                    ORDER BY allInfo.PointID,allInfo.tstamp
                    ", sql, factorSel, factorFlagSel, factorMarkSel, factorIsAuditSel
                     , StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",")
                     , dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"));
                    //ON allInfo.PointID = hourData.PointId AND allInfo.tstamp = hourData.DataDateTime
                }
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 取得行转列数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="isAllDate">是否补充缺失数据</param>
        /// <returns></returns>
        public DataView GetDataViewSuper(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd, bool isAllDate = false, string PointType = "0")
        {
            try
            {
                string sql = "";

                if (factors.Contains("a99074") || factors.Contains("a99072") || factors.Contains("a99070"))
                {
                    string portIdsStr = string.Empty;
                    if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                    {
                        portIdsStr = " AND dayInfo.PointId =" + portIds[0];
                    }
                    else if (portIds.Length > 1)
                    {
                        portIdsStr = "AND dayInfo.PointId IN(" + StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",") + ")";
                    }

                    //取得查询行转列字段拼接
                    string factorSql = string.Empty;
                    string factorFlagSql = string.Empty;
                    string factorMarkSql = string.Empty;
                    string factorIsAuditSql = string.Empty;
                    string factorSel = string.Empty;
                    string factorFlagSel = string.Empty;
                    string factorMarkSel = string.Empty;
                    string factorIsAuditSel = string.Empty;
                    string wFactorSql = string.Empty;


                    foreach (string factor in factors)
                    {
                        if (!factor.ToString().Equals(""))
                        {
                            string factorDataFlag = factor + "_dataFlag";
                            string factorAuditFlag = factor + "_AuditFlag";
                            string factorIsAudit = factor + "_IsAudit";
                            factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                            factorFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN dataFlag END) AS [{1}] ", factor, factorDataFlag);
                            factorMarkSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN AuditFlag END) AS [{1}] ", factor, factorAuditFlag);
                            factorIsAuditSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN IsAudit END) AS [{1}] ", factor, factorIsAudit);
                            factorSel += string.Format(",[{0}]", factor);
                            factorFlagSel += string.Format(",[{0}]", factorDataFlag);
                            factorMarkSel += string.Format(",[{0}]", factorAuditFlag);
                            factorIsAuditSel += string.Format(",[{0}]", factorIsAudit);
                            wFactorSql += "'" + factor + "',";
                        }
                    }

                    wFactorSql = wFactorSql.Remove(wFactorSql.Length - 1, 1);

                    sql = string.Format(@"
                    SELECT dayInfo.PointId
                        ,hourInfo.DataDateTime
                        ,(CASE WHEN MAX(hourStatus.[Status]) is not null THEN MAX(hourStatus.[Status]) ELSE CASE WHEN Count(hourInfo.DataDateTime)>0 THEN '0' ELSE '' END END) AS AuditStatus                  
                        {0}
                        {1}
                        {2}
                        {3}
                    FROM {4} AS hourInfo
                    INNER JOIN Audit.TB_AuditStatusForDay AS dayInfo
	                    ON hourInfo.AuditStatusUid = dayInfo.AuditStatusUid  
                    INNER JOIN Audit.TB_AuditAirStatusForHour AS hourStatus
                        ON hourInfo.PointId=hourStatus.PointId                      
                    WHERE hourInfo.DataDateTime>='{5}'
                        AND  hourInfo.PollutantCode in ({9})
                        AND hourInfo.DataDateTime<='{6}'
                        AND dayInfo.PointId is not NULL
                        AND hourStatus.[PointIdType]='{8}'
	                    {7}
                    GROUP BY dayInfo.PointId,hourInfo.DataDateTime
                    ", factorSql, factorFlagSql, factorMarkSql, factorIsAuditSql, tableName, dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr, PointType, wFactorSql);
                }
                else if (factors.Contains("401") || factors.Contains("402") || factors.Contains("404"))
                {
                    string portSql = "";
                    foreach (string portId in portIds)
                    {
                        portSql += portId + ",";
                    }
                    portSql = portSql.Remove(portSql.Length - 1, 1);

                    string factorSql = "";
                    foreach (string factor in factors)
                    {
                        factorSql += "'" + factor + "',";
                    }
                    factorSql = factorSql.Remove(factorSql.Length - 1, 1);

                    sql = string.Format(@"SELECT wb.*,mp.monitoringpointname as pointname,pc.PollutantName as PollutantName
  FROM [AMS_MonitorBusiness].[dbo].[TB_AuditSuperStation_Weibo] wb
  left join
  dbo.SY_MonitoringPoint mp
  on wb.PointId=mp.pointid
  left join
  dbo.SY_PollutantCode pc
  on wb.PollutantCode=pc.PollutantCode where wb.PointId in ({0}) and wb.PollutantCode in ({1}) and wb.DateTime>='{2}' and wb.DateTime<='{3}' order by wb.DateTime,wb.PollutantCode", portSql, factorSql, dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else if (factors.Contains("ASP") || factors.Contains("L72"))
                {
                    string portSql = "";
                    foreach (string portId in portIds)
                    {
                        portSql += portId + ",";
                    }
                    portSql = portSql.Remove(portSql.Length - 1, 1);

                    string factorSql = "";
                    foreach (string factor in factors)
                    {
                        factorSql += factor + ",";
                    }
                    factorSql = factorSql.Remove(factorSql.Length - 1, 1);

                    if (factorSql.Equals("ASP"))
                    {
                        sql = string.Format(@"SELECT wb.*,mp.monitoringpointname as pointname
  FROM AMS_MonitorBusiness.dbo.TB_AuditSuperStation_lijingpu_L wb
  left join
  dbo.SY_MonitoringPoint mp
  on wb.PointId=mp.pointid where wb.PointId in ({0}) and wb.DateTime>='{1}' and wb.DateTime<='{2}' order by wb.DateTime", portSql, dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    else if (factorSql.Equals("L72"))
                    {
                        sql = string.Format(@"SELECT wb.*,mp.monitoringpointname as pointname
  FROM AMS_MonitorBusiness.dbo.TB_AuditSuperStation_lijingpu_M wb
  left join
  dbo.SY_MonitoringPoint mp
  on wb.PointId=mp.pointid where wb.PointId in ({0}) and wb.DateTime>='{1}' and wb.DateTime<='{2}' order by wb.DateTime", portSql, dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                }
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 取得统计数据常规因子（最大值、最小值、平均值）
        /// </summary>
        /// <param name="autoMonitorType">数据类型</param>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetStatisticalData(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {
            try
            {
                string portIdsStr = string.Empty;
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND dayInfo.PointId =" + portIds[0];
                }
                else if (portIds.Length > 1)
                {
                    portIdsStr = "AND dayInfo.PointId IN(" + StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",") + ")";
                }

                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorMarkSql = string.Empty;
                string factorSel = string.Empty;
                string factorMarkSel = string.Empty;
                string avgSel = string.Empty;
                //string maxSel = string.Empty;
                //string minSel = string.Empty;
                //string totalSel = string.Empty;
                //string enableSel = string.Empty;
                //string disableSel = string.Empty;

                foreach (string factor in factors)
                {
                    if (!factor.ToString().Equals(""))
                    {
                        string factorDataFlag = factor + "_dataFlag";
                        string factorAuditFlag = factor + "_AuditFlag";
                        string factorIsAudit = factor + "_IsAudit";
                        factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                        factorMarkSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN AuditFlag END) AS [{1}] ", factor, factorAuditFlag);
                        factorSel += string.Format(",[{0}]", factor);
                        factorMarkSel += string.Format(",[{0}]", factorAuditFlag);
                        avgSel += string.Format(@",count({0}) as {0}_total,count(CASE WHEN CHARINDEX ('RM',{0})<=0 THEN {0} ELSE NULL END) as {0}_enable,avg({0}) as {0}_avg,max({0}) as {0}_max,min({0}) as {0}_min", factor);
                    }
                }
                string sql = string.Format(@"
                    SELECT A.* FROM(
                    SELECT PointId,Convert(varchar(10),DataDateTime,120) DataDateTime {6} FROM(
                    SELECT dayInfo.PointId
                        ,hourInfo.DataDateTime
                        {0}
                        {1}
                    FROM {2} AS hourInfo
                    LEFT JOIN Audit.TB_AuditStatusForDay AS dayInfo
	                    ON hourInfo.AuditStatusUid = dayInfo.AuditStatusUid
                    WHERE hourInfo.DataDateTime>='{3}' 
                        AND hourInfo.DataDateTime<='{4}'
                        AND dayInfo.PointId is not NULL
	                    {5}
                    GROUP BY dayInfo.PointId,hourInfo.DataDateTime
                     ) A GROUP BY PointId,Convert(varchar(10),DataDateTime,120)
                     )  A ORDER BY DataDateTime DESC
                    ", factorSql, factorMarkSql, tableName, dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr, avgSel);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 取得统计数据所有因子（最大值、最小值、平均值）
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataView GetStatisticalAllData(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd, bool isAllDate = false)
        {
            try
            {
                string portIdsStr = string.Empty;
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND dayInfo.PointId =" + portIds[0];
                }
                else if (portIds.Length > 1)
                {
                    portIdsStr = "AND dayInfo.PointId IN(" + StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",") + ")";
                }

                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                string factorMarkSql = string.Empty;
                string factorIsAuditSql = string.Empty;
                string factorSel = string.Empty;
                string factorFlagSel = string.Empty;
                string factorMarkSel = string.Empty;
                string factorIsAuditSel = string.Empty;
                string avgSel = string.Empty;
                string maxSel = string.Empty;
                string minSel = string.Empty;
                string totalSel = string.Empty;
                string enableSel = string.Empty;
                string disableSel = string.Empty;
                string dataflag = string.Empty;
                string auditflag = string.Empty;
                string isaudit = string.Empty;

                foreach (string factor in factors)
                {
                    if (!factor.ToString().Equals(""))
                    {
                        string factorDataFlag = factor + "_dataFlag";
                        string factorAuditFlag = factor + "_AuditFlag";
                        string factorIsAudit = factor + "_IsAudit";
                        factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                        factorFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN dataFlag END) AS [{1}] ", factor, factorDataFlag);
                        factorMarkSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN AuditFlag END) AS [{1}] ", factor, factorAuditFlag);
                        factorIsAuditSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN IsAudit END) AS [{1}] ", factor, factorIsAudit);
                        factorSel += string.Format(",[{0}]", factor);
                        factorFlagSel += string.Format(",[{0}]", factorDataFlag);
                        factorMarkSel += string.Format(",[{0}]", factorAuditFlag);
                        factorIsAuditSel += string.Format(",[{0}]", factorIsAudit);
                        totalSel += string.Format(@",count({0}) as {0}", factor);
                        disableSel += string.Format(@",count(CASE WHEN (CHARINDEX ('RM',{0})>0 or {0} is null) THEN {0} ELSE NULL END) as {0}", factor);
                        enableSel += string.Format(@",count(CASE WHEN CHARINDEX ('RM',{0})<=0 THEN {0} ELSE NULL END) as {0}", factor);
                        maxSel += string.Format(@",max({0}) as {0}", factor);
                        avgSel += string.Format(@",avg({0}) as {0}", factor);
                        minSel += string.Format(@",min({0}) as {0}", factor);
                        dataflag += ",null [" + factor + "_dataFlag]";
                        auditflag += ",null [" + factor + "_AuditFlag]";
                        isaudit += ",null [" + factor + "_IsAudit]";

                    }
                }
                string sql = string.Format(@"
                    SELECT dayInfo.PointId
                        ,hourInfo.DataDateTime
                        {0}
                        {1}
                        {2}
                        {3}
                    FROM {4} AS hourInfo
                    LEFT JOIN Audit.TB_AuditStatusForDay AS dayInfo
	                    ON hourInfo.AuditStatusUid = dayInfo.AuditStatusUid
                    WHERE hourInfo.DataDateTime>='{5}' 
                        AND hourInfo.DataDateTime<='{6}'
                        AND dayInfo.PointId is not NULL
	                    {7}
                    GROUP BY dayInfo.PointId,hourInfo.DataDateTime
                    ", factorSql, factorFlagSql, factorMarkSql, factorIsAuditSql, tableName, dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr);

                sql = string.Format(@"
                    WITH hourData AS
                    (
                        {0}
                    ),total AS (
                    SELECT convert(datetime,'1900-01-01') DataDateTime
                    UNION
                     SELECT convert(datetime,'1900-01-02') DataDateTime
                     UNION
                     SELECT convert(datetime,'1900-01-03') DataDateTime
                     UNION
                     SELECT convert(datetime,'1900-01-04') DataDateTime
                     UNION
                     SELECT convert(datetime,'1900-01-05') DataDateTime
                     UNION
                     SELECT convert(datetime,'1900-01-06') DataDateTime
                    )", sql);
                //是否自动补充缺失数据
                if (isAllDate && portIds != null && portIds.Length > 0)
                {
                    sql += string.Format(@"
                    SELECT allInfo.PointID
                        ,allInfo.tstamp as DataDateTime
                        {1}
                        {2}
                        {3}
                        {4}
                    FROM dbo.F_GetAllDataByHour('{5}',',','{6}','{7}') AS allInfo
                    LEFT JOIN hourData
	                    ON allInfo.PointID = hourData.PointId AND allInfo.tstamp = hourData.DataDateTime
                    ", sql, factorSel, factorFlagSel, factorMarkSel, factorIsAuditSel
                     , StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",")
                     , dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else
                { sql += string.Format(@" SELECT * FROM hourData "); }


                sql += string.Format(@"  UNION ALL      
                     select 0 PointId,total.DataDateTime {10} {11} {12} {13} from total left join(             
                       SELECT PointId
                        ,convert(datetime,'1900-01-01') DataDateTime
                        {1}
                        {7} {8} {9} FROM hourData
                        Group BY PointId,Convert(varchar(10),DataDateTime,120)
                       UNION
                     SELECT PointId
                        ,convert(datetime,'1900-01-02') DataDateTime
                        {2}
                        {7} {8} {9} FROM hourData
                         Group BY PointId,Convert(varchar(10),DataDateTime,120)
                     UNION
                      SELECT PointId
                        ,convert(datetime,'1900-01-03') DataDateTime
                        {3}
                        {7} {8} {9} FROM hourData
                       Group BY PointId,Convert(varchar(10),DataDateTime,120)
                     UNION
                      SELECT PointId
                        ,convert(datetime,'1900-01-04') DataDateTime
                        {4}
                        {7} {8} {9} FROM hourData
                          Group BY PointId,Convert(varchar(10),DataDateTime,120)
                     UNION
                      SELECT PointId
                        ,convert(datetime,'1900-01-05') DataDateTime
                        {5}
                        {7} {8} {9} FROM hourData
                       Group BY PointId,Convert(varchar(10),DataDateTime,120)
                     UNION
                      SELECT PointId
                        ,convert(datetime,'1900-01-06') DataDateTime
                        {6}
                        {7} {8} {9} FROM hourData
                         Group BY PointId,Convert(varchar(10),DataDateTime,120)
                      )  AS A ON total.DataDateTime=A.DataDateTime
                       ", sql, totalSel, disableSel, enableSel, maxSel, minSel, avgSel, dataflag, auditflag, isaudit, factorSel, factorFlagSel, factorMarkSel, factorIsAuditSel);

                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 取得统计数据所选点位的所有因子（最大值、最小值、平均值）
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataView GetStatisticalMutilPoint(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd, bool isAllDate = false)
        {
            try
            {
                string portIdsStr = string.Empty;
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND dayInfo.PointId =" + portIds[0];
                }
                else if (portIds.Length > 1)
                {
                    portIdsStr = "AND dayInfo.PointId IN(" + StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",") + ")";
                }

                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                string factorMarkSql = string.Empty;
                string factorIsAuditSql = string.Empty;
                string factorSel = string.Empty;
                string factorFlagSel = string.Empty;
                string factorMarkSel = string.Empty;
                string factorIsAuditSel = string.Empty;
                string avgSel = string.Empty;
                string maxSel = string.Empty;
                string minSel = string.Empty;
                //string totalSel = string.Empty;
                //string enableSel = string.Empty;
                //string disableSel = string.Empty;
                string dataflag = string.Empty;
                string auditflag = string.Empty;
                string isaudit = string.Empty;

                foreach (string factor in factors)
                {
                    if (!factor.ToString().Equals(""))
                    {
                        string factorDataFlag = factor + "_dataFlag";
                        string factorAuditFlag = factor + "_AuditFlag";
                        string factorIsAudit = factor + "_IsAudit";
                        factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                        factorFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN dataFlag END) AS [{1}] ", factor, factorDataFlag);
                        factorMarkSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN AuditFlag END) AS [{1}] ", factor, factorAuditFlag);
                        factorIsAuditSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN IsAudit END) AS [{1}] ", factor, factorIsAudit);
                        factorSel += string.Format(",[{0}]", factor);
                        factorFlagSel += string.Format(",[{0}]", factorDataFlag);
                        factorMarkSel += string.Format(",[{0}]", factorAuditFlag);
                        factorIsAuditSel += string.Format(",[{0}]", factorIsAudit);
                        //totalSel += string.Format(@",count({0}) as {0}", factor);
                        //disableSel += string.Format(@",count(CASE WHEN (CHARINDEX ('RM',{0})>0 or {0} is null) THEN {0} ELSE NULL END) as {0}", factor);
                        //enableSel += string.Format(@",count(CASE WHEN CHARINDEX ('RM',{0})<=0 THEN {0} ELSE NULL END) as {0}", factor);
                        maxSel += string.Format(@",max({0}) as {0}", factor);
                        avgSel += string.Format(@",avg({0}) as {0}", factor);
                        minSel += string.Format(@",min({0}) as {0}", factor);
                        dataflag += ",null [" + factor + "_dataFlag]";
                        auditflag += ",null [" + factor + "_AuditFlag]";
                        isaudit += ",null [" + factor + "_IsAudit]";

                    }
                }
                string sql = string.Format(@"
                    SELECT dayInfo.PointId
                        ,hourInfo.DataDateTime
                        {0}
                        {1}
                        {2}
                        {3}
                    FROM {4} AS hourInfo
                    LEFT JOIN Audit.TB_AuditStatusForDay AS dayInfo
	                    ON hourInfo.AuditStatusUid = dayInfo.AuditStatusUid
                    WHERE hourInfo.DataDateTime>='{5}' 
                        AND hourInfo.DataDateTime<='{6}'
                        AND dayInfo.PointId is not NULL
	                    {7}
                    GROUP BY dayInfo.PointId,hourInfo.DataDateTime
                    ", factorSql, factorFlagSql, factorMarkSql, factorIsAuditSql, tableName, dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr);

                sql = string.Format(@"
                    WITH hourData AS
                    (
                        {0}
                    ),total AS (
                     SELECT convert(datetime,'1900-01-06') DataDateTime
                     UNION
                     SELECT convert(datetime,'1900-01-05') DataDateTime
                     UNION
                     SELECT convert(datetime,'1900-01-04') DataDateTime
                    )", sql);
                //是否自动补充缺失数据
                if (isAllDate && portIds != null && portIds.Length > 0)
                {
                    sql += string.Format(@"
                    SELECT 0 orderid,allInfo.PointID
                        ,allInfo.tstamp as DataDateTime
                        {1}
                        {2}
                        {3}
                        {4}
                    FROM dbo.F_GetAllDataByHour('{5}',',','{6}','{7}') AS allInfo
                    LEFT JOIN hourData
	                    ON allInfo.PointID = hourData.PointId AND allInfo.tstamp = hourData.DataDateTime
                    ", sql, factorSel, factorFlagSel, factorMarkSel, factorIsAuditSel
                     , StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",")
                     , dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else
                { sql += string.Format(@" SELECT 0 orderid, * FROM hourData "); }


                sql += string.Format(@"  UNION ALL      
                     select 1 orderid,0 PointId,total.DataDateTime {10} {11} {12} {13} from total left join(             
                      SELECT convert(datetime,'1900-01-06') DataDateTime
                        {4}
                        {7} {8} {9} FROM hourData
                     UNION
                      SELECT  convert(datetime,'1900-01-05') DataDateTime
                        {5}
                        {7} {8} {9} FROM hourData
                     UNION
                      SELECT convert(datetime,'1900-01-04') DataDateTime
                        {6}
                        {7} {8} {9} FROM hourData
                      )  AS A ON total.DataDateTime=A.DataDateTime
                     ORDER BY  orderid,PointId,DataDateTime DESC
                       ", sql, "", "", "", avgSel, minSel, maxSel, dataflag, auditflag, isaudit, factorSel, factorFlagSel, factorMarkSel, factorIsAuditSel);

                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据点位类型、时间段获取审核状态统计信息
        /// </summary>
        /// <param name="application">系统标识Uid</param>
        /// EnumMapping.GetApplicationValue(ApplicationValue.Air)
        /// <param name="SiteType">站点类型Uid</param>
        /// <param name="beginTime">开始日期</param>
        /// <param name="endTime">截止日期</param>
        /// <returns></returns>
        public DataTable AuditFlagStatisticsByPoint(string application, string SiteType, DateTime beginTime, DateTime endTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select * from (");
            strSql.Append("select smp0.PointId,smp0.MonitoringPointName,(case when dayInfo0.[Date] is null then '" + beginTime + "' else dayInfo0.[Date] end) as [Date]  ");
            strSql.Append(",(case when dayInfo0.PointId is NULL then '无数据' ");
            strSql.Append(" else case when [Status] is NULL or [Status]=0 then '未审核' ");
            strSql.Append(" else case when [Status]=1 then '已审核'  else  case when [Status]=2 then '部分审核' ");
            strSql.Append(" else '无数据' end end end end) as AuditStatus ");
            strSql.Append(" ,dayInfo0.DataException as Abnormal,smp0.OrderByNum ");
            strSql.Append(" from dbo.SY_MonitoringPoint as smp0 ");
            strSql.Append(" left join ( ");
            strSql.Append(" SELECT PointId,[Date],Status = MAX(Status),DataException=MAX(DataException) ");
            strSql.Append(" from Audit.TB_AuditStatusForDay ");
            strSql.Append(" where [Date] = '" + beginTime + "' and ApplicationUid='" + application + "' ");
            strSql.Append(" group by [Date],PointId ) as dayInfo0 on smp0.PointId = dayInfo0.PointId ");
            strSql.Append(" where smp0.SiteTypeUid='" + SiteType + "' ");
            strSql.Append(" and smp0.ApplicationUid='" + application + "' ");
            strSql.Append(" and smp0.EnableOrNot=1 and smp0.ShowOrNot=1 ");
            if (endTime > beginTime)
            {
                TimeSpan ts = endTime - beginTime;
                int days = ts.Days;
                for (int i = 1; i <= days; i++)
                {
                    DateTime newTimes = beginTime.AddDays(i);
                    strSql.Append(" union all ");
                    strSql.Append("select smp" + i.ToString() + ".PointId,smp" + i.ToString() + ".MonitoringPointName ");
                    strSql.Append(",(case when dayInfo" + i.ToString() + ".[Date] is null then '" + newTimes + "' else dayInfo" + i.ToString() + ".[Date] end) as [Date] ");
                    strSql.Append(",(case when dayInfo" + i.ToString() + ".PointId is NULL then '无数据' ");
                    strSql.Append(" else case when [Status] is NULL or [Status]=0 then '未审核' ");
                    strSql.Append(" else case when [Status]=1 then '已审核'  else  case when [Status]=2 then '部分审核' ");
                    strSql.Append(" else '无数据' end end end end) as AuditStatus ");
                    strSql.Append(" ,dayInfo" + i.ToString() + ".DataException as Abnormal,smp" + i.ToString() + ".OrderByNum ");
                    strSql.Append(" from dbo.SY_MonitoringPoint as smp" + i.ToString() + " ");
                    strSql.Append(" left join ( ");
                    strSql.Append(" SELECT PointId,[Date],Status = MAX(Status),DataException=MAX(DataException) ");
                    strSql.Append(" from Audit.TB_AuditStatusForDay ");
                    strSql.Append(" where [Date] = '" + newTimes + "' and ApplicationUid='" + application + "' ");
                    strSql.Append(" group by [Date],PointId ) as dayInfo" + i.ToString() + " on smp" + i.ToString() + ".PointId = dayInfo" + i.ToString() + ".PointId ");
                    strSql.Append(" where smp" + i.ToString() + ".SiteTypeUid='" + SiteType + "' ");
                    strSql.Append(" and smp" + i.ToString() + ".ApplicationUid='" + application + "' ");
                    strSql.Append(" and smp" + i.ToString() + ".EnableOrNot=1 and smp" + i.ToString() + ".ShowOrNot=1 ");
                }
            }
            strSql.Append(" ) as t order by t.OrderByNum desc,t.PointId asc,t.Date desc ");
            //string sql = string.Format(@"select * from [dbo].[F_GetAuditStatus] ('{0}','{1}','{2}','{3}') order by OrderByNum desc,PointId asc,Date desc "
            //                            , beginTime.Date, endTime.Date, SiteType, application);
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), connection);
        }

        /// <summary>
        /// 更新因子审核状态
        /// </summary>
        /// <param name="PointID"></param>
        /// <param name="AuditStatusUid"></param>
        public void AuditFactorStatusUpdate(int PointID, string AuditStatusUid, string UserName)
        {
            string sql = string.Format(@"update {0} set [IsAudit]=1,[UpdateUser]='{3}',[UpdateDateTime]=GETDATE() where [PointId]={1} and [AuditStatusUid]='{2}'"
                                       , tableName, PointID, AuditStatusUid, UserName);
            g_DatabaseHelper.ExecuteNonQuery(sql, connection);
        }

        /// <summary>
        /// 更新因子审核状态
        /// </summary>
        /// <param name="PointID"></param>
        /// <param name="AuditStatusUid"></param>
        public void AuditFactorStatusUpdate(int PointID, string[] factors, string AuditStatusUid, string UserName)
        {
            string sql = string.Format(@"update {0} set [IsAudit]=1,[UpdateUser]='{3}',[UpdateDateTime]=GETDATE() where [PointId]={1} and [AuditStatusUid]='{2}' and PollutantCode in ({4})"
                                       , tableName, PointID, AuditStatusUid, UserName, "'" + string.Join("','", factors) + "'");
            g_DatabaseHelper.ExecuteNonQuery(sql, connection);
        }

        /// <summary>
        /// 更新因子审核小时状态
        /// </summary>
        /// <param name="PointID"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="UserName"></param>
        public void AuditFactorHourStatusUpdate(int PointID, DateTime beginTime, DateTime endTime, string[] factors, string UserName, string PointType)
        {
            string sql = string.Format(@"UPDATE hourStatus 
	                                     SET hourStatus.[Status]=(Case when FactorCount<{0} then '2' else '1' end)
		                                    ,hourStatus.UpdateDateTime = GETDATE()
                                            ,hourStatus.[UpdateUser]='{1}'
	                                     FROM [Audit].[TB_AuditAirStatusForHour] AS hourStatus
	                                     LEFT JOIN  (SELECT PointId,DataDateTime,COUNT(distinct PollutantCode) FactorCount
                                                     FROM Audit.TB_AuditAirInfectantByHour
                                                     WHERE PointId={2} and DataDateTime>='{3}' and DataDateTime<='{4}' 	
                                                           AND 	PollutantCode IN({6})
                                                     GROUP by PointId,DataDateTime) AS adtData 
                                        ON hourStatus.PointId = adtData.PointId AND hourStatus.[Date] = adtData.DataDateTime
	                                    WHERE  adtData.PointId IS NOT NULL AND hourStatus.PointIdType='{5}'"
                                      , factors.Length, UserName, PointID, beginTime, endTime, PointType, "'" + string.Join("','", factors) + "'");
            g_DatabaseHelper.ExecuteNonQuery(sql, connection);
        }

        /// <summary>
        /// 更新因子审核小时状态
        /// </summary>
        /// <param name="PointID"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="UserName"></param>
        public void AuditFactorHourStatusUpdateS(int PointID, string InsId, DateTime beginTime, DateTime endTime, string[] factors, string UserName, string PointType)
        {
            //if (InsId.Equals("6e4aa38a-f68b-490b-9cd7-3b92c7805c2d"))
            //{
            //    InsId = "d2747011-ff8d-4b04-a006-c32ecaad4507";
            //}
            string sql = string.Format(@"UPDATE hourStatus 
	                                     SET hourStatus.[Status]=(Case when FactorCount<{0} then '2' else '1' end)
		                                    ,hourStatus.UpdateDateTime = GETDATE()
                                            ,hourStatus.[UpdateUser]='{1}'
	                                     FROM [Audit].[TB_AuditAirStatusForHour] AS hourStatus
	                                     LEFT JOIN  (SELECT PointId,DataDateTime,COUNT(distinct PollutantCode) FactorCount
                                                     FROM Audit.TB_AuditAirInfectantByHour
                                                     WHERE PointId={2} and DataDateTime>='{3}' and DataDateTime<='{4}' 	
                                                           AND 	PollutantCode IN({6})
                                                     GROUP by PointId,DataDateTime) AS adtData 
                                        ON hourStatus.PointId = adtData.PointId AND hourStatus.[Date] = adtData.DataDateTime
	                                    WHERE  adtData.PointId IS NOT NULL AND hourStatus.PointIdType='{5}' AND hourStatus.Description = '{7}'"
                                      , factors.Length, UserName, PointID, beginTime, endTime, PointType, "'" + string.Join("','", factors) + "'", InsId);
            g_DatabaseHelper.ExecuteNonQuery(sql, connection);
        }

        /// <summary>
        /// 更新因子审核小时状态(气象)
        /// </summary>
        /// <param name="PointID"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="UserName"></param>
        public void AuditFactorHourStatusUpdateWea(int PointID, DateTime beginTime, DateTime endTime, string[] factors, string UserName, string PointType)
        {
            string sql = string.Format(@"UPDATE hourStatus 
	                                     SET hourStatus.[Status]=(Case when FactorCount<{0} or FactorCounts<=0 then '2' else '1' end)
		                                    ,hourStatus.UpdateDateTime = GETDATE()
                                            ,hourStatus.[UpdateUser]='{1}'
	                                     FROM [Audit].[TB_AuditAirStatusForHour] AS hourStatus
	                                     LEFT JOIN  (SELECT PointId,DataDateTime,COUNT(distinct PollutantCode) FactorCount
                                                     FROM Audit.TB_AuditAirInfectantByHour
                                                     WHERE PointId={2} and DataDateTime>='{3}' and DataDateTime<='{4}' 	
                                                           AND 	PollutantCode IN({6})
                                                     GROUP by PointId,DataDateTime) AS adtData 
                                        ON hourStatus.PointId = adtData.PointId AND hourStatus.[Date] = adtData.DataDateTime
                                        LEFT JOIN  (SELECT PointId,Tstamp,COUNT(distinct PollutantCode) FactorCounts
													FROM AirReport.TB_HourReport
													WHERE PointId={2} and Tstamp>='{3}' and Tstamp<='{4}' 	
													AND PollutantCode IN('a21026','a21004','a21005','a05024','a34002','a34004','a21003','a21002')
													GROUP by PointId,Tstamp) AS hourData 
													ON hourStatus.PointId = hourData.PointId AND hourStatus.[Date] = hourData.Tstamp
	                                    WHERE  adtData.PointId IS NOT NULL AND hourStatus.PointIdType='{5}'"
                                      , factors.Length, UserName, PointID, beginTime, endTime, PointType, "'" + string.Join("','", factors) + "'");
            g_DatabaseHelper.ExecuteNonQuery(sql, connection);
        }
        /// <summary>
        /// 获取审核预处理小时记录数
        /// </summary>
        /// <param name="AuditStatusUid"></param>
        /// <returns></returns>
        public int GetAuditRecordNumByHour(string AuditStatusUid, string[] factors)
        {
            int count = 0;
            string sql = string.Format(@"SELECT COUNT(*) AS num FROM {0} 
                                         WHERE AuditStatusUid='{1}' AND [PollutantCode] IN({2})
                                         GROUP BY PointId,Convert(varchar(10),DataDateTime,120)", tableName, AuditStatusUid, "'" + string.Join("','", factors) + "'");
            DataView dv = g_DatabaseHelper.ExecuteDataView(sql, connection);
            if (dv.Count > 0) count = Convert.ToInt32(dv[0][0]);
            return count;
        }
        /// <summary>
        /// 获取审核预处理小时记录数
        /// </summary>
        /// <param name="AuditStatusUid"></param>
        /// <returns></returns>
        public int GetAuditRecordNumByHourSuper(string AuditStatusUid, string[] factors)
        {
            int count = 0;
            string sql = string.Format(@"SELECT COUNT(*) AS num FROM {0} 
                                         WHERE AuditStatusUid='{1}' AND [PollutantCode] IN({2}) AND IsAudit=1
                                         GROUP BY PointId,Convert(varchar(10),DataDateTime,120)", tableName, AuditStatusUid, "'" + string.Join("','", factors) + "'");
            DataView dv = g_DatabaseHelper.ExecuteDataView(sql, connection);
            if (dv.Count > 0) count = Convert.ToInt32(dv[0][0]);
            return count;
        }
        /// <summary>
        /// 获取审核预处理小时记录数(气象)
        /// </summary>
        /// <param name="AuditStatusUid"></param>
        /// <returns></returns>
        public int GetAuditRecordNumByHourWea(int PointID, DateTime beginTime, DateTime endTime)
        {
            int count = 0;
            string sql = string.Format(@"select COUNT(*) FROM [Audit].[TB_AuditAirStatusForHour]
									WHERE PointId={0} and date>='{1}' and date<='{2}' and PointIdType=0 and  Convert(varchar(2),datepart(MINUTE,date))=0
									GROUP BY PointId,   Convert(varchar(10),date,120)", PointID, beginTime, endTime);
            DataView dv = g_DatabaseHelper.ExecuteDataView(sql, connection);
            if (dv.Count > 0) count = Convert.ToInt32(dv[0][0]);
            return count;
        }
        #endregion
    }
}
