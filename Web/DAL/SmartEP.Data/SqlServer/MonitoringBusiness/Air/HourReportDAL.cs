﻿using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.Data.SqlServer.Common.GridView;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.Utilities.AdoData;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.MonitoringBusiness.Air
{
    /// <summary>
    /// 名称：HourReportDAL.cs
    /// 创建人：李飞
    /// 创建日期：2015-08-18
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 环境空气发布：审核小时数据库处理
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class HourReportDAL
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
        /// 存储过程处理类
        /// </summary>
        BaseDAHelper g_DBBiz = Singleton<BaseDAHelper>.GetInstance();

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connection = "AMS_MonitoringBusinessConnection";
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connection1 = "AMS_AirAutoMonitorConnection";

        private string connection3 = "AMS_BaseDataConnection";
        /// <summary>
        /// 数据库表名
        /// </summary>
        private string tableName = "AirReport.TB_HourReport";
        /// <summary>
        /// 数据库表名
        /// </summary>
        private string tableName1 = "Air.TB_InfectantBy60";

        private string connection2 = "Frame_Connection";
        private string tableName2 = "dbo.TB_OMMP_TaskMain";
        #endregion

        #region << 构造函数 >>
        /// <summary>
        /// 构造函数
        /// </summary>
        public HourReportDAL()
        {
            tableName = SmartEP.Data.Enums.EnumMapping.GetAutoMonitorTableName(ApplicationType.Air, PollutantDataType.Hour);
            tableName1 = SmartEP.Data.Enums.EnumMapping.GetAutoMonitorTableName(ApplicationType.Air, PollutantDataType.Min60);
            connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(ApplicationType.Air, PollutantDataType.Hour);
            connection1 = SmartEP.Data.Enums.EnumMapping.GetConnectionName(ApplicationType.Air, PollutantDataType.Min60);
        }
        #endregion

        #region << 方法 >>

        /// <summary>
        /// 取得测点名称
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <returns></returns>
        public DataView GetPointName(string portIds)
        {
            try
            {
                string sql = @"select PointId,MonitoringPointName from [AMS_BaseData].[MPInfo].[TB_MonitoringPoint] 
                                where PointId= " + portIds;
                DataView DV = g_DatabaseHelper.ExecuteDataView(sql, connection);
                return DV;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetDataPager(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Tstamp")
        {
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "PointId,Tstamp";
            recordTotal = 0;
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                string factorWhere = string.Empty;
                foreach (string factor in factors)
                {
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,',') END ) AS [{0}] ", factor);
                    factorWhere += "'" + factor + "',";
                }
                factorWhere = factorWhere.Remove(factorWhere.Length - 1, 1);

                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND PointId =" + portIdsStr;
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = "AND PointId IN(" + portIdsStr + ")";
                }

                orderBy = string.IsNullOrEmpty(orderBy) ? "PointId,Tstamp" : orderBy;
                string fieldName = "PointId,Tstamp" + factorSql + factorFlagSql;
                string groupBy = "PointId,Tstamp";
                string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}' AND PollutantCode in ({2}) ", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + portIdsStr;
                return g_GridViewPager.GetPivotDataPager(tableName, fieldName, groupBy, orderBy, where, pageSize, pageNo, connection, out recordTotal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetNewDataPager(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Tstamp")
        {
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "PointId,Tstamp";
            recordTotal = 0;
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                string factorWhere = string.Empty;

                foreach (string factor in factors)
                {
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                    string factorFlag = factor + "_Status";

                    factorFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN AuditFlag END) AS [{1}] ", factor, factorFlag);
                    factorWhere += "'" + factor + "',";
                }
                factorWhere = factorWhere.Remove(factorWhere.Length - 1, 1);

                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND PointId =" + portIdsStr;
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = "AND PointId IN(" + portIdsStr + ")";
                }

                orderBy = string.IsNullOrEmpty(orderBy) ? "PointId,Tstamp" : orderBy;
                string fieldName = "PointId,Tstamp" + factorSql + factorFlagSql;
                string groupBy = "PointId,Tstamp";
                string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}' AND PollutantCode in ({2})", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + portIdsStr;
                return g_GridViewPager.GetPivotDataPager(tableName, fieldName, groupBy, orderBy, where, pageSize, pageNo, connection, out recordTotal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetNewDataPagerAvg(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Tstamp")
        {
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "PointId,Tstamp";
            recordTotal = 0;
            try
            {
                //取得查询行转列字段拼接
                string factorASql = string.Empty;
                string factorSql = "select PointId,(Convert(nvarchar(13),Tstamp,120)+':00:00') Tstamp";
                string factorMarkSql = string.Empty;
                string factorWhere = string.Empty;

                foreach (string factor in factors)
                {
                    factorASql += string.Format(",AVG({0}) {0}", factor);
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                }

                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND PointId =" + portIdsStr;
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = "AND PointId IN(" + portIdsStr + ")";
                }

                orderBy = string.IsNullOrEmpty(orderBy) ? "PointId,Tstamp" : orderBy;
                string fieldName = "PointId,Tstamp" + factorSql + factorMarkSql;
                string groupBy = "PointId,Tstamp";
                string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}' ", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr;
                string sql = string.Format(@"Select Tstamp {0} from ({1} from {2} where {3} group by {4})data group by Tstamp order by Tstamp", factorASql, factorSql, tableName, where, groupBy);
                DataView dv = g_DatabaseHelper.ExecuteDataView(sql, connection);
                return dv;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetHourDataAvg(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Tstamp")
        {

            recordTotal = 0;
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorAvg = string.Empty;
                string factorWhere = string.Empty;

                foreach (string factor in factors)
                {
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                    factorAvg += string.Format(",CONVERT(decimal(18,4),[dbo].[F_Round](AVG({0}),(select DecimalDigit from [AMS_BaseData].[Standard].[TB_PollutantCode] where PollutantCode='{0}'))) {0}", factor);
                    factorWhere += "'" + factor + "',";
                }
                factorWhere = factorWhere.Remove(factorWhere.Length - 1, 1);

                string groupby = "Tstamp";
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND PointId =" + portIdsStr;
                    groupby = "PointId,Convert(nvarchar(13),Tstamp,21)";
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = "AND PointId IN(" + portIdsStr + ")";
                }

                orderBy = string.IsNullOrEmpty(orderBy) ? "Tstamp" : orderBy;
                string fieldName = "PointId,Convert(nvarchar(13),Tstamp,21) Tstamp" + factorSql;
                string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}' AND PollutantCode in ({2})", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + portIdsStr;
                string sql = string.Format(@"select {5},convert(DateTime,Tstamp+':00:00.000') Tstamp
											{3}
											from(
											select {1}
											 from {0}
											where {2}
                                            group by PointId,Tstamp
											 ) a 
											 group by {6}
                                             order by {4}"
                    , tableName, fieldName, where, factorAvg, orderBy, portIds.Length > 1 ? "'0' PointId" : "PointId", groupby);

                DataTable dt = g_DatabaseHelper.ExecuteDataTable(sql, connection);
                recordTotal = dt.Rows.Count;
                return dt.DefaultView;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 取得查询数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetNewDataPager(string portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, string orderBy = "PointId,Tstamp")
        {

            try
            {
                //取得查询行转列字段拼接
                orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "PointId,Tstamp";
                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                string factorWhere = string.Empty;

                foreach (string factor in factors)
                {
                    factorSql += string.Format(",MAX(CASE(A.PollutantCode) WHEN '{0}' THEN A.PollutantValue END ) AS [{0}] ", factor);
                    string factorFlag = factor + "_Status";

                    factorFlagSql += string.Format(",MAX(CASE(A.PollutantCode) WHEN '{0}' THEN A.AuditFlag END) AS [{1}] ", factor, factorFlag);
                    factorWhere += "'" + factor + "',";
                }
                factorWhere = factorWhere.Remove(factorWhere.Length - 1, 1);

                string portIdsStr = " AND A.PointId = " + portIds;
                //string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                //if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                //{
                //    portIdsStr = " AND A.PointId =" + portIdsStr;
                //}
                //else if (!string.IsNullOrEmpty(portIdsStr))
                //{
                //    portIdsStr = "AND A.PointId IN(" + portIdsStr + ")";
                //}

                string where = string.Format(" A.Tstamp>='{0}' AND A.Tstamp<='{1}' AND A.PollutantCode in ({2})", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + portIdsStr;
                string sql = String.Empty;
                sql = @"select A.Tstamp,A.PointId,A.PollutantCode,B.MonitoringPointName,C.PollutantName,A.PollutantValue from [AMS_MonitorBusiness].[AirReport].[TB_HourReport] A,[AMS_BaseData].[MPInfo].[TB_MonitoringPoint] B,[AMS_BaseData].[Standard].[TB_PollutantCode] C
                                where A.[PointId] = B.[PointId] AND A.[PollutantCode] = C.[PollutantCode] AND " + where + " order by " + orderBy;
                DataView DV = g_DatabaseHelper.ExecuteDataView(sql, connection);
                return DV;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 取得全月小时数
        /// </summary>
        /// <param name="autoMonitorType">数据类型</param>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetAvgDayData(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd)
        {
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                foreach (string factor in factors)
                {
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                }

                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND PointId =" + portIdsStr;
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = "AND PointId IN(" + portIdsStr + ")";
                }
                string sql = string.Format(@"select a.PointId
							,SUM(CASE WHEN a01001 is not null THEN 1 ELSE 0 END) AS TempCount
							,SUM(CASE WHEN a01002 is not null THEN 1 ELSE 0 END) AS RHCount
							,SUM(CASE WHEN a01006 is not null THEN 1 ELSE 0 END) AS PressCount
							,SUM(CASE WHEN a01008 is not null THEN 1 ELSE 0 END) AS WdCount
							,SUM(CASE WHEN a01007 is not null THEN 1 ELSE 0 END) AS WsCount
							,Max(a01001) as TempMax
							,Max(a01002) as RHMax
							,Max(a01006) as PressMax
							,Max(a01008) as WdMax
							,Max(a01007) as WsMax
							from
							(
							select PointId,[Tstamp] {1} {2}
							 FROM {0}
							where  [Tstamp]>='{3}' AND [Tstamp]<='{4}' {5}
							group by PointId,[Tstamp]
							)a group by PointId
               ", tableName, factorSql, factorFlagSql, dateStart.ToString("yyyy-MM-dd HH:mm:ss"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        ///         /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetDataNewPager(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd)
        {
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                foreach (string factor in factors)
                {
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS {0} ", factor);
                }

                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND HourReport.PointId =" + portIdsStr;
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = "AND HourReport.PointId IN(" + portIdsStr + ")";
                }
                string sql = string.Format(@"SELECT PointId,Tstamp as DateTime
							{1}
							FROM (
							SELECT PointId
							,CONVERT(DATETIME,CONVERT(NVARCHAR(10),Tstamp,120),120) AS Tstamp
							,HourReport.PollutantCode
							,CASE WHEN ((HourReport.PollutantCode IN ('a05024','a21026','a21005','a21004','a90969','a21028','a21001') AND COUNT(dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','))<18) or (HourReport.PollutantCode IN ('a34002','a34004') AND COUNT(dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','))<12))
							THEN NULL ELSE dbo.F_Round(AVG(dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,',')),3)
							END AS PollutantValue
							FROM {0} AS HourReport
							LEFT JOIN 
							(
							SELECT DISTINCT PollutantCode
							,CASE when DecimalDigit IS NULL THEN 3 ELSE DecimalDigit END AS DecimalDigit
							FROM dbo.SY_PollutantCode
							) AS Pollutant
							ON HourReport.PollutantCode = Pollutant.PollutantCode
							WHERE  HourReport.Tstamp>='{2}' AND HourReport.Tstamp<='{3}' {4}
							and HourReport.PollutantCode IN ('a05024','a21026','a21005','a21004','a34002','a34004','a90969','a21028','a21001')
							GROUP BY PointId,CONVERT(DATETIME,CONVERT(NVARCHAR(10),Tstamp,120),120),HourReport.PollutantCode,DecimalDigit
							) AS hourData
							group by PointId,Tstamp
							Order by PointId,Tstamp
               ", tableName, factorSql, dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }
        /// 取得全月均值，有效天
        /// </summary>
        /// <param name="autoMonitorType">数据类型</param>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetAvgDayNewData(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd)
        {
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorCount = string.Empty;
                string factorAvg = string.Empty;
                foreach (string factor in factors)
                {
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS {0} ", factor);
                    factorCount += string.Format(",SUM(CASE WHEN {0} is not null THEN 1 ELSE 0 END) AS {0}" + "Count", factor);
                    factorAvg += string.Format(",CASE WHEN COUNT(dbo.F_ValidValueStr({0}))>=22 THEN [dbo].[F_Round](AVG(CONVERT (decimal(18,6),{0})),3) ELSE NULL END AS {0}" + "Avg", factor);
                }

                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND HourReport.PointId =" + portIdsStr;
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = "AND HourReport.PointId IN(" + portIdsStr + ")";
                }
                string sql = string.Format(@"select a.PointId
								{5}
                                {6}
								from(
								SELECT PointId,Tstamp as DateTime 
                                {1}
								FROM (
								SELECT PointId
								,CONVERT(DATETIME,CONVERT(NVARCHAR(10),Tstamp,120),120) AS Tstamp
								,HourReport.PollutantCode
								,CASE WHEN ((HourReport.PollutantCode IN ('a05024','a21026','a21005','a21004','a90969','a21028','a21001') AND COUNT(dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','))<18) or (HourReport.PollutantCode IN ('a34002','a34004') AND COUNT(dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','))<12))
								THEN NULL ELSE dbo.F_Round(AVG(dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,',')),3)
								END AS PollutantValue
								FROM {0} AS HourReport
								LEFT JOIN 
								(
								SELECT DISTINCT PollutantCode
								,CASE when DecimalDigit IS NULL THEN 3 ELSE DecimalDigit END AS DecimalDigit
								FROM dbo.SY_PollutantCode
								) AS Pollutant
								ON HourReport.PollutantCode = Pollutant.PollutantCode
								WHERE  HourReport.Tstamp>='{2}' AND HourReport.Tstamp<='{3}' {4}
								and HourReport.PollutantCode IN ('a05024','a21026','a21005','a21004','a34002','a34004','a90969','a21028','a21001')
								GROUP BY PointId,CONVERT(DATETIME,CONVERT(NVARCHAR(10),Tstamp,120),120),HourReport.PollutantCode,DecimalDigit
								) AS hourData
								group by PointId,Tstamp
								) a  group by PointId
               ", tableName, factorSql, dateStart.ToString("yyyy-MM-dd HH:mm:ss"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr, factorCount, factorAvg);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 数据捕获率有效时数
        /// </summary>
        /// <param name="autoMonitorType">数据类型</param>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetCaptureRateData(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd)
        {
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                foreach (string factor in factors)
                {
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                    factorFlagSql += string.Format(",SUM(CASE WHEN {0} is not null THEN 1 ELSE 0 END) AS {0}", factor);
                }

                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND PointId =" + portIdsStr;
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = "AND PointId IN(" + portIdsStr + ")";
                }
                string sql = string.Format(@"select a.PointId {2}
							from
							(
							select PointId,[Tstamp] {1}
							 FROM {0}
							where  [Tstamp]>='{3}' AND [Tstamp]<='{4}' {5}
							group by PointId,[Tstamp]
							)a group by PointId
               ", tableName, factorSql, factorFlagSql, dateStart.ToString("yyyy-MM-dd HH:mm:ss"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 数据捕获率有效时数
        /// </summary>
        /// <param name="autoMonitorType">数据类型</param>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetSuperCaptureRateData(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd)
        {
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                string factorInSql = "and data.PollutantCode in (";
                foreach (string factor in factors)
                {
                    factorSql += string.Format(",MAX(CASE(data.PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                    factorFlagSql += string.Format(",SUM(CASE WHEN {0} is not null THEN 1 ELSE 0 END) AS {0}", factor);
                    factorInSql += string.Format("'{0}',", factor);
                }
                factorInSql = factorInSql.TrimEnd(',');
                factorInSql += ")";
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND PointId =" + portIdsStr;
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = "AND PointId IN(" + portIdsStr + ")";
                }
                string sql = string.Format(@"select a.[CategoryUid] {2}
							from
							(
							select PointId,[Tstamp] ,code.[CategoryUid] {1}
							 FROM {0}  data inner join AMS_BaseData.Standard.TB_PollutantCode code
							 on data.PollutantCode=code.PollutantCode
							where  [Tstamp]>='{3}' AND [Tstamp]<='{4}' {5} {6}
							group by PointId,[Tstamp],code.[CategoryUid]
							)a group by PointId,[CategoryUid]
               ", tableName, factorSql, factorFlagSql, dateStart.ToString("yyyy-MM-dd HH:mm:ss"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr, factorInSql);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 数据捕获率有效时数
        /// </summary>
        /// <param name="autoMonitorType">数据类型</param>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetCaptureRateDataNew(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd)
        {
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                foreach (string factor in factors)
                {
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                    factorFlagSql += string.Format(",SUM(CASE WHEN {0} is not null THEN 1 ELSE 0 END) AS {0}", factor);
                }

                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND PointId =" + portIdsStr;
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = "AND PointId IN(" + portIdsStr + ")";
                }
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select * from (");
                for (int i = 0; dateStart.AddMonths(i) <= dateEnd; i++)
                {
                    DateTime dateend = dateStart.AddMonths(i).AddMonths(1).AddSeconds(-1);
                    DateTime datestart = dateStart.AddMonths(i);
                    string sql = string.Format(@"select a.PointId {2},CONVERT(varchar(7),a.Tstamp,121) as Tst 
							from
							(
							select PointId,[Tstamp] {1}
							 FROM {0}
							where  [Tstamp]>='{3}' AND [Tstamp]<='{4}' {5}
							group by PointId,[Tstamp]
							)a group by PointId ,CONVERT(varchar(7),a.Tstamp,121)
               ", tableName, factorSql, factorFlagSql, datestart.ToString("yyyy-MM-dd HH:mm:ss"), dateend.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr);
                    if (i == 0)
                    {
                        strSql.Append(sql);
                    }
                    else
                    {
                        strSql.AppendFormat(@" union {0}", sql);
                    }
                    //DataTable dt = m_HourData.GetCaptureDataPagerNew(portIds, factors, dtBegion.AddMonths(i), dtBegion.AddMonths(i).AddMonths(1).AddSeconds(-1), true).Table;
                    //ExcelHelper.DataTableToExcel(dt, "有效数据捕获率", "有效数据捕获率", this.Page);
                }
                strSql.Append(") AS M");

                return g_DatabaseHelper.ExecuteDataView(strSql.ToString(), connection);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 考核项目
        /// </summary>
        /// <param name="autoMonitorType">数据类型</param>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetCheckNew(string[] portIds, DateTime dateStart, DateTime dateEnd)
        {
            try
            {
                //取得查询行转列字段拼接
                //string factorSql = string.Empty;
                //string factorFlagSql = string.Empty;
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " PointId =" + portIdsStr;
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = "PointId IN(" + portIdsStr + ")";
                }
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select * from (");
                for (int i = 0; dateStart.AddMonths(i) <= dateEnd; i++)
                {
                    DateTime dateend = dateStart.AddMonths(i).AddMonths(1).AddSeconds(-1);
                    DateTime datestart = dateStart.AddMonths(i);
                    string sql = string.Format(@"SELECT SUM(X) AS '巡检(次)',SUM(L) AS '零/跨标准(次)',SUM(J) AS '精密度检查(次)',SUM(D) AS '多点线性检查(次)',MaintenanceObjectGuid,Tst FROM
		(select * from 
		(select COUNT(FormGuid) as X,0 as L, 0 as J ,0 as D,MaintenanceObjectGuid,CONVERT(varchar(7),a.BeginDate,121) as Tst
		  from [EQMS_Framework].[dbo].[TB_OMMP_TaskMain] a
		  left join [EQMS_Framework].[dbo].[TB_OMMP_TaskDetail] b
		  on a.RowGuid=b.MainGuid where ObjectType=2 and a.MaintenanceObjectGuid in 
		(select RowGuid from [EQMS_Framework].[dbo].[TB_OMMP_MaintenanceObject] c
		 left join [AMS_BaseData].[MPInfo].[TB_MonitoringPoint] d
		 on c.ObjectID=d.MonitoringPointUid where {3}) and a.BeginDate<='{2}' and a.BeginDate>='{1}' and FormGuid='5591a8f7-7731-4c84-a3dc-102eeee9067e' group by a.MaintenanceObjectGuid,CONVERT(varchar(7),a.BeginDate,121)
		union
		select 0 AS X,COUNT(FormGuid) as L,0 as J ,0 as D,MaintenanceObjectGuid,CONVERT(varchar(7),a.BeginDate,121) as Tst
		  from [EQMS_Framework].[dbo].[TB_OMMP_TaskMain] a
		  left join [EQMS_Framework].[dbo].[TB_OMMP_TaskDetail] b
		  on a.RowGuid=b.MainGuid where ObjectType=2 and a.MaintenanceObjectGuid in 
		(select RowGuid from [EQMS_Framework].[dbo].[TB_OMMP_MaintenanceObject] c
		 left join [AMS_BaseData].[MPInfo].[TB_MonitoringPoint] d
		 on c.ObjectID=d.MonitoringPointUid where {3}) and a.BeginDate<='{2}' and a.BeginDate>='{1}' and FormGuid='c4b9ac6d-4d77-4a73-a3a6-03922563c44b' group by a.MaintenanceObjectGuid,CONVERT(varchar(7),a.BeginDate,121)
		 union
		select 0 AS X,0 as L,COUNT(FormGuid) as J,0 as D,MaintenanceObjectGuid,CONVERT(varchar(7),a.BeginDate,121) as Tst
		  from [EQMS_Framework].[dbo].[TB_OMMP_TaskMain] a
		  left join [EQMS_Framework].[dbo].[TB_OMMP_TaskDetail] b
		  on a.RowGuid=b.MainGuid where ObjectType=2 and a.MaintenanceObjectGuid in 
		(select RowGuid from [EQMS_Framework].[dbo].[TB_OMMP_MaintenanceObject] c
		 left join [AMS_BaseData].[MPInfo].[TB_MonitoringPoint] d
		 on c.ObjectID=d.MonitoringPointUid where {3}) and a.BeginDate<='{2}' and a.BeginDate>='{1}' and FormGuid='f1d310c4-a2f9-49d9-b2ee-12f9c266ac98' group by a.MaintenanceObjectGuid,CONVERT(varchar(7),a.BeginDate,121)
		  union
		select 0 AS X,0 as L,0 as J,COUNT(FormGuid) as D ,MaintenanceObjectGuid,CONVERT(varchar(7),a.BeginDate,121) as Tst
		  from [EQMS_Framework].[dbo].[TB_OMMP_TaskMain] a
		  left join [EQMS_Framework].[dbo].[TB_OMMP_TaskDetail] b
		  on a.RowGuid=b.MainGuid where ObjectType=2 and a.MaintenanceObjectGuid in 
		(select RowGuid from [EQMS_Framework].[dbo].[TB_OMMP_MaintenanceObject] c
		 left join [AMS_BaseData].[MPInfo].[TB_MonitoringPoint] d
		 on c.ObjectID=d.MonitoringPointUid where {3}) and a.BeginDate<='{2}' and a.BeginDate>='{1}' and FormGuid='5693bc77-e0f3-424a-8046-4f56fafc43be' group by a.MaintenanceObjectGuid,CONVERT(varchar(7),a.BeginDate,121))AS m) AS N
		 group by  MaintenanceObjectGuid,Tst", tableName2, datestart.ToString("yyyy-MM-dd HH:mm:ss"), dateend.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr);
                    //                    string sql = string.Format(@"select a.PointId ,CONVERT(varchar(7),a.Tstamp,121) as Tst 
                    //							from
                    //							(
                    //							select PointId,[Tstamp] 
                    //							 FROM {0}
                    //							where  [Tstamp]>='{1}' AND [Tstamp]<='{2}' {3}
                    //							group by PointId,[Tstamp]
                    //							)a group by PointId ,CONVERT(varchar(7),a.Tstamp,121)
                    //               ", tableName2,  datestart.ToString("yyyy-MM-dd HH:mm:ss"), dateend.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr);
                    if (i == 0)
                    {
                        strSql.Append(sql);
                    }
                    else
                    {
                        strSql.AppendFormat(@" union {0}", sql);
                    }
                    //DataTable dt = m_HourData.GetCaptureDataPagerNew(portIds, factors, dtBegion.AddMonths(i), dtBegion.AddMonths(i).AddMonths(1).AddSeconds(-1), true).Table;
                    //ExcelHelper.DataTableToExcel(dt, "有效数据捕获率", "有效数据捕获率", this.Page);
                }
                strSql.Append(") AS B");

                return g_DatabaseHelper.ExecuteDataView(strSql.ToString(), connection2);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 数据有效率有效时数
        /// </summary>
        /// <param name="autoMonitorType">数据类型</param>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetEffectiveData(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd)
        {
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                foreach (string factor in factors)
                {
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                    factorFlagSql += string.Format(",SUM(CASE WHEN {0} is not null THEN 1 ELSE 0 END) AS {0}", factor);
                }

                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND PointId =" + portIdsStr;
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = "AND PointId IN(" + portIdsStr + ")";
                }
                string sql = string.Format(@"select a.PointId {2}
							from
							(
							select PointId,[Tstamp] {1}
							 FROM {0}
							where  [Tstamp]>='{3}' AND [Tstamp]<='{4}' {5}
							group by PointId,[Tstamp]
							)a group by PointId
               ", tableName1, factorSql, factorFlagSql, dateStart.ToString("yyyy-MM-dd HH:mm:ss"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr);
                return g_DatabaseHelper.ExecuteDataView(sql, connection1);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 数据有效率有效时数
        /// </summary>
        /// <param name="autoMonitorType">数据类型</param>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetSuperEffectiveData(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd)
        {

            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                string factorInSql = "and data.PollutantCode in (";
                foreach (string factor in factors)
                {
                    factorSql += string.Format(",MAX(CASE(data.PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                    factorFlagSql += string.Format(",SUM(CASE WHEN {0} is not null THEN 1 ELSE 0 END) AS {0}", factor);
                    factorInSql += string.Format("'{0}',",factor);
                }
                factorInSql=factorInSql.TrimEnd(',');
                factorInSql += ")";
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND PointId =" + portIdsStr;
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = "AND PointId IN(" + portIdsStr + ")";
                }
                string sql = string.Format(@"select a.[CategoryUid] {2}
							from
							(
							select PointId,[Tstamp] ,code.[CategoryUid] {1}
							 FROM {0}  data inner join AMS_BaseData.Standard.TB_PollutantCode code
							 on data.PollutantCode=code.PollutantCode
							where  [Tstamp]>='{3}' AND [Tstamp]<='{4}' {5} {6}
							group by PointId,[Tstamp],code.[CategoryUid]
							)a group by PointId,[CategoryUid]
               ", tableName1, factorSql, factorFlagSql, dateStart.ToString("yyyy-MM-dd HH:mm:ss"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr,factorInSql);
                return g_DatabaseHelper.ExecuteDataView(sql, connection1);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 根据CategoryUid获取因子
        /// </summary>
        /// <param name="CategoryUid"></param>
        /// <returns></returns>
        public DataView GetPollutantCodeByUid(string CategoryUid)
        {
            string sql = string.Format(@"select PollutantCode from Standard.TB_PollutantCode where CategoryUid='{0}'", CategoryUid);
            return g_DatabaseHelper.ExecuteDataView(sql, connection3);
        }
        /// <summary>
        /// 根据因子获取CategoryUid
        /// </summary>
        /// <param name="PollutantCode"></param>
        /// <returns></returns>
        public DataView GetCategoryUidByPollutantCode(string[] PollutantCode)
        {
            string pollutantCode = string.Empty;
            for (int i = 0; i < PollutantCode.Length;i++ )
            {
                pollutantCode += "'" + PollutantCode[i] + "',";
            }
            string sql = string.Format(@"select distinct CategoryUid from Standard.TB_PollutantCode where PollutantCode in({0})", pollutantCode.TrimEnd(','));
            return g_DatabaseHelper.ExecuteDataView(sql, connection3);
        }
        /// <summary>
        /// 获取CategoryUid的名称
        /// </summary>
        /// <param name="CategoryUid"></param>
        /// <returns></returns>
        public DataView GetCategory(string CategoryUid)
        {
            string sql = string.Format(@"SELECT DISTINCT [category] FROM [dbo].[V_Factor_UserConfig] where CategoryUid='{0}'", CategoryUid);
            return g_DatabaseHelper.ExecuteDataView(sql, connection3);
        }
        /// <summary>
        /// 数据有效率有效时数
        /// </summary>
        /// <param name="autoMonitorType">数据类型</param>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetEffectiveDataNew(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd)
        {
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                foreach (string factor in factors)
                {
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                    factorFlagSql += string.Format(",SUM(CASE WHEN {0} is not null THEN 1 ELSE 0 END) AS {0}", factor);
                }

                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND PointId =" + portIdsStr;
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = "AND PointId IN(" + portIdsStr + ")";
                }
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select * from (");
                for (int i = 0; dateStart.AddMonths(i) <= dateEnd; i++)
                {
                    DateTime dateend = dateStart.AddMonths(i).AddMonths(1).AddSeconds(-1);
                    DateTime datestart = dateStart.AddMonths(i);
                    string sql = string.Format(@"select a.PointId {2},CONVERT(varchar(7),a.Tstamp,121) as Tst
							from
							(
							select PointId,[Tstamp] {1}
							 FROM {0}
							where  [Tstamp]>='{3}' AND [Tstamp]<='{4}' {5}
							group by PointId,[Tstamp]
							)a group by PointId,CONVERT(varchar(7),a.Tstamp,121)
               ", tableName1, factorSql, factorFlagSql, dateStart.ToString("yyyy-MM-dd HH:mm:ss"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr);
                    if (i == 0)
                    {
                        strSql.Append(sql);

                    }
                    else
                    {
                        strSql.AppendFormat(@" union {0}", sql);
                    }
                }
                strSql.Append(") AS M");
                return g_DatabaseHelper.ExecuteDataView(strSql.ToString(), connection1);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)去除质控数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetDataPagerNew(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Tstamp")
        {
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "PointId,Tstamp";
            recordTotal = 0;
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                foreach (string factor in factors)
                {
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,',') END ) AS [{0}] ", factor);
                    string factorFlag = factor + "_Status";

                    factorFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN AuditFlag END) AS [{1}] ", factor, factorFlag);
                }

                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND PointId =" + portIdsStr;
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = "AND PointId IN(" + portIdsStr + ")";
                }

                string factorStr = "";
                if (factors != null && factors.Length > 0 && factors[0].ToString() != "")
                    factorStr = " AND PollutantCode IN ('" + StringExtensions.GetArrayStrNoEmpty(factors.ToList(), "','") + "')";

                string markStr = " AND (AuditFlag  not in ('C') or AuditFlag is null) ";
                orderBy = string.IsNullOrEmpty(orderBy) ? "PointId,Tstamp" : orderBy;
                string fieldName = "PointId,Tstamp" + factorSql + factorFlagSql;
                string groupBy = "PointId,Tstamp";
                string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}' ", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr + factorStr;
                where = where + markStr;
                return g_GridViewPager.GetPivotDataPager(tableName, fieldName, groupBy, orderBy, where, pageSize, pageNo, connection, out recordTotal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)去除质控数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetVOCsKQYDataPagerNew(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Tstamp")
        {
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "PointId,Tstamp";
            recordTotal = 0;
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                foreach (string factor in factors)
                {
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN CASE WHEN Datename(hour,Tstamp)=0 THEN Null ELSE dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,',') END END) AS [{0}] ", factor);
                    string factorFlag = factor + "_Status";

                    factorFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN AuditFlag END) AS [{1}] ", factor, factorFlag);
                }

                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND PointId =" + portIdsStr;
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = "AND PointId IN(" + portIdsStr + ")";
                }

                string factorStr = "";
                if (factors != null && factors.Length > 0 && factors[0].ToString() != "")
                    factorStr = " AND PollutantCode IN ('" + StringExtensions.GetArrayStrNoEmpty(factors.ToList(), "','") + "')";

                string markStr = " AND (AuditFlag  not in ('C') or AuditFlag is null) ";
                orderBy = string.IsNullOrEmpty(orderBy) ? "PointId,Tstamp" : orderBy;
                string fieldName = "PointId,Tstamp" + factorSql + factorFlagSql;
                string groupBy = "PointId,Tstamp";
                string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}' ", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr + factorStr;
                where = where + markStr;
                return g_GridViewPager.GetPivotDataPager(tableName, fieldName, groupBy, orderBy, where, pageSize, pageNo, connection, out recordTotal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetDataHourPager(string[] portIds, string[] factors, DateTime dtBegin, DateTime dtEnd,
            DateTime dtFrom, DateTime dtTo, string type, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,tstamp desc")
        {
            string[] typeArry = type.Replace("'", "").Split(',');
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "PointId,tstamp desc";
            recordTotal = 0;
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                foreach (string factor in factors)
                {
                    foreach (string typeItem in typeArry)
                    {
                        factorSql += string.Format(",MAX(CASE WHEN (PollutantCode)= '{0}' and Type='{1}' THEN dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,',') else -10 END ) AS [{0}{1}] ", factor, typeItem);
                    }
                    // factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue else -10 END ) AS [{0}] ", factor);
                }

                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND PointId =" + portIdsStr;
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = "AND PointId IN(" + portIdsStr + ")";
                }
                if (!string.IsNullOrEmpty(type))
                {
                    type = "AND Type IN(" + type + ")";
                }
                orderBy = string.IsNullOrEmpty(orderBy) ? "PointId" : orderBy;
                string fieldName = "PointId,MonitoringPointName as portName,Tstamp" + factorSql + factorFlagSql;
                string groupBy = "PointId,MonitoringPointName,Tstamp";
                string tableName = "dbo.V_Air_DataCompare";
                DateTime dateB = new DateTime();
                DateTime dateE = new DateTime();
                string where = "";
                if (dtFrom == dateB && dtTo == dateE)
                {
                    where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}' ", dtBegin.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr + type;
                }
                else
                {
                    where = string.Format(" ((Tstamp>='{0}' AND Tstamp<='{1}') or (Tstamp>='{2}' AND Tstamp<='{3}')) ", dtBegin.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), dtFrom.ToString("yyyy-MM-dd HH:mm:ss"), dtTo.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr + type;
                }
                return g_GridViewPager.GetPivotDataPager(tableName, fieldName, groupBy, orderBy, where, pageSize, pageNo, connection, out recordTotal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetDataPager(string[] portIds, string[] factors, DateTime dtBegin, DateTime dtEnd,
            DateTime dtFrom, DateTime dtTo, string type, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,tstamp desc,Type")
        {
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "PointId,tstamp desc,Type";
            recordTotal = 0;
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                foreach (string factor in factors)
                {
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,',') else -10 END ) AS [{0}] ", factor);
                    // factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue else -10 END ) AS [{0}] ", factor);
                }

                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND PointId =" + portIdsStr;
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = "AND PointId IN(" + portIdsStr + ")";
                }
                if (!string.IsNullOrEmpty(type))
                {
                    type = "AND Type IN(" + type + ")";
                }
                orderBy = string.IsNullOrEmpty(orderBy) ? "PointId" : orderBy;
                string fieldName = "PointId,MonitoringPointName,Type,Tstamp" + factorSql + factorFlagSql;
                string groupBy = "PointId,MonitoringPointName,Type,Tstamp";
                string tableName = "dbo.V_Air_DataCompare";
                DateTime dateB = new DateTime();
                DateTime dateE = new DateTime();
                string where = "";
                if (dtFrom == dateB && dtTo == dateE)
                {
                    where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}' ", dtBegin.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr + type;
                }
                else
                {
                    where = string.Format(" ((Tstamp>='{0}' AND Tstamp<='{1}') or (Tstamp>='{2}' AND Tstamp<='{3}')) ", dtBegin.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), dtFrom.ToString("yyyy-MM-dd HH:mm:ss"), dtTo.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr + type;
                }
                return g_GridViewPager.GetPivotDataPager(tableName, fieldName, groupBy, orderBy, where, pageSize, pageNo, connection, out recordTotal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetDataPagerDF(string[] portIds, string[] factors, DateTime dtBegin, DateTime dtEnd,string type, int pageSize, int pageNo, string orderBy = "PointId,tstamp desc,Type")
        {
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "PointId,tstamp desc,Type";
            int recordTotal = 0;
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                foreach (string factor in factors)
                {
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,',') else -10 END ) AS [{0}] ", factor);
                    // factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue else -10 END ) AS [{0}] ", factor);
                }

                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND PointId =" + portIdsStr;
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = "AND PointId IN(" + portIdsStr + ")";
                }
                if (!string.IsNullOrEmpty(type))
                {
                    type = "AND Type IN(" + type + ")";
                }
                orderBy = string.IsNullOrEmpty(orderBy) ? "PointId" : orderBy;
                string fieldName = "row_number() over(order by Tstamp) as Ordernum ,PointId,MonitoringPointName,Type,Tstamp" + factorSql + factorFlagSql;
                string groupBy = "PointId,MonitoringPointName,Type,Tstamp";
                string tableName = "dbo.V_Air_DataCompare";
                //DateTime dateB = new DateTime();
                //DateTime dateE = new DateTime();
                string where = "";
                //if (dtFrom == dateB && dtTo == dateE)
                //{
                where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}' ", dtBegin.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr + type;
                //}
                //else
                //{
                //    where = string.Format(" ((Tstamp>='{0}' AND Tstamp<='{1}') or (Tstamp>='{2}' AND Tstamp<='{3}')) ", dtBegin.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), dtFrom.ToString("yyyy-MM-dd HH:mm:ss"), dtTo.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr + type;
                //}
                return g_GridViewPager.GetPivotDataPager(tableName, fieldName, groupBy, orderBy, where, pageSize, pageNo, connection, out recordTotal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 取得导出数据(行转列数据)
        /// </summary>
        /// <param name="autoMonitorType">查询数据类型</param>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetExportData(string[] portIds, IList<IPollutant> factors
            , DateTime dtStart, DateTime dtEnd, string orderBy = "PointId,Tstamp")
        {
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "PointId,Tstamp";
            int recordTotal = 0;
            int pageSize = 99999999;
            int pageNo = 0;
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                foreach (IPollutant factor in factors)
                {
                    string factorFlag = factor.PollutantCode + "_Status";
                    Int32 decimalNum = string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum);
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN CAST(dbo.F_Round(PollutantValue,{1}) AS DECIMAL(10,{1})) END ) AS [{2}] ", factor.PollutantCode, decimalNum, string.Format("{0}({1})", factor.PollutantName, factor.PollutantMeasureUnit));
                }
                //站点处理
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND PointId =" + portIdsStr;
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = "AND PointId IN(" + portIdsStr + ")";
                }
                string pointSql = string.Empty;
                //foreach (IPoint portId in portIds)
                //{
                //    pointSql += string.Format(" WHEN PointId={0} THEN '{1}' ", portId.PointID, portId.PointName);
                //}
                //if (!string.IsNullOrEmpty(pointSql))
                //{
                //    pointSql = " CASE " + pointSql + " END AS '测点' ";
                //}
                //else
                //{
                //    pointSql = " NULL AS '测点' ";
                //}
                orderBy = string.IsNullOrEmpty(orderBy) ? "PointId,Tstamp" : orderBy;
                string fieldName = "rowNum as '序号',PointId,CONVERT(NVARCHAR(19),Tstamp,120) as '日期'" + factorSql + factorFlagSql;
                string groupBy = "PointId,Tstamp";
                string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}' ", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr;
                return g_GridViewPager.GetPivotDataPager(tableName, fieldName, groupBy, orderBy, where, pageSize, pageNo, connection, out recordTotal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 取得导出数据(行转列数据)
        /// </summary>
        /// <param name="autoMonitorType">查询数据类型</param>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetNewExportData(string[] portIds, IList<IPollutant> factors
            , DateTime dtStart, DateTime dtEnd, string orderBy = "PointId,Tstamp")
        {
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "PointId,Tstamp";
            int recordTotal = 0;
            int pageSize = 99999999;
            int pageNo = 0;
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                foreach (IPollutant factor in factors)
                {
                    string factorFlag = factor.PollutantCode + "_Status";
                    Int32 decimalNum = string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum);
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN CAST(dbo.F_Round(PollutantValue,{1}) AS DECIMAL(10,{1})) END ) AS [{2}] ", factor.PollutantCode, decimalNum, string.Format("{0}({1})", factor.PollutantName, factor.PollutantMeasureUnit));
                    //  factorFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN AuditFlag END) AS [{1}] ", factor, factorFlag);
                }
                //站点处理
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND PointId =" + portIdsStr;
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = "AND PointId IN(" + portIdsStr + ")";
                }
                string[] factorCodes = factors.Select(p => p.PollutantCode).ToArray();
                string factorsStr = string.Empty;
                if (factorCodes != null && factorCodes.Length > 0)
                    factorsStr = " AND PollutantCode IN ('" + StringExtensions.GetArrayStrNoEmpty(factorCodes.ToList(), "','") + "')";

                string pointSql = string.Empty;
                orderBy = string.IsNullOrEmpty(orderBy) ? "PointId,Tstamp" : orderBy;
                string fieldName = "rowNum as '序号',PointId,CONVERT(NVARCHAR(19),Tstamp,120) as '日期'" + factorSql + factorFlagSql;
                string groupBy = "PointId,Tstamp";
                string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}' ", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr + factorsStr;
                return g_GridViewPager.GetPivotDataPager(tableName, fieldName, groupBy, orderBy, where, pageSize, pageNo, connection, out recordTotal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 环境空气质量自动监测数据报表导出
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetAQAutoMonthReportExportData(string[] portIds, IList<IPollutant> factors
         , DateTime dtStart, DateTime dtEnd, string orderBy = "PointId,Tstamp")
        {
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "PointId,Tstamp";
            int recordTotal = 0;
            int pageSize = 99999999;
            int pageNo = 0;
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                foreach (IPollutant factor in factors)
                {
                    string factorFlag = factor.PollutantCode + "_Status";
                    Int32 decimalNum = string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum);
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN CAST(dbo.F_Round(dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),{1}) AS DECIMAL(10,{1})) END ) AS [{0}] ", factor.PollutantCode, decimalNum);
                }
                //站点处理
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND PointId =" + portIdsStr;
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = "AND PointId IN(" + portIdsStr + ")";
                }
                string pointSql = string.Empty;
                orderBy = string.IsNullOrEmpty(orderBy) ? "PointId,Tstamp" : orderBy;
                string fieldName = "rowNum as '序号',PointId,CONVERT(NVARCHAR(16),Tstamp,120) as DateTime,CONVERT(NVARCHAR(10),Tstamp,111) as '日期',CONVERT(varchar(5), Tstamp, 8) as '时间'" + factorSql + factorFlagSql;
                string groupBy = "PointId,Tstamp";
                string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}' ", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr;
                return g_GridViewPager.GetPivotDataPager(tableName, fieldName, groupBy, orderBy, where, pageSize, pageNo, connection, out recordTotal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 取得统计数据（最大值、最小值、平均值）
        /// </summary>
        /// <param name="autoMonitorType">数据类型</param>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetStatisticalData(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd)
        {
            try
            {
                //拼接Where条件
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND PointId =" + portIdsStr;
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = " AND PointId IN(" + portIdsStr + ")";
                }
                string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}' ", dateStart.ToString("yyyy-MM-dd HH:mm:ss"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr;

                //拼接统计数据字串，使用UNION方式减少统计条数，提升统计速度
                string sql = string.Empty;
                for (int iRow = 0; iRow < factors.Length; iRow++)
                {
                    if (iRow > 0)
                        sql += " UNION ";
                    sql += string.Format(@"
                        SELECT PointId,PollutantCode='{1}'
	                        ,AVG(dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,',')) AS Value_Avg
	                        ,MAX(dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,',')) AS Value_Max
	                        ,MIN(dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,',')) AS Value_Min
                            ,Count(dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,',')) AS Value_Count
                        FROM {0}
                        WHERE {2}
                            AND PollutantCode='{1}'
                        GROUP BY PointId,PollutantCode
                    ", tableName, factors[iRow], where);
                }
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 取得统计数据 按日分组（最大值、最小值、平均值）
        /// </summary>
        /// <param name="autoMonitorType">数据类型</param>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetStatisticalDataByDay(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd)
        {
            try
            {
                //拼接Where条件
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND PointId =" + portIdsStr;
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = " AND PointId IN(" + portIdsStr + ")";
                }
                string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}' ", dateStart.ToString("yyyy-MM-dd HH:mm:ss"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr;

                //拼接统计数据字串，使用UNION方式减少统计条数，提升统计速度
                string sql = string.Empty;
                for (int iRow = 0; iRow < factors.Length; iRow++)
                {
                    if (iRow > 0)
                        sql += " UNION ";
                    sql += string.Format(@"
                        SELECT PointId,Convert(datetime,Convert(varchar(10),Tstamp,120)) Tstamp,PollutantCode='{1}'
	                        ,AVG(dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,',')) AS Value_Avg
	                        ,MAX(dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,',')) AS Value_Max
	                        ,MIN(dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,',')) AS Value_Min
                            ,Count(dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,',')) AS Value_Count
                        FROM {0}
                        WHERE {2}
                            AND PollutantCode='{1}'
                        GROUP BY PointId,PollutantCode,Convert(datetime,Convert(varchar(10),Tstamp,120))
                    ", tableName, factors[iRow], where);
                }
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 取得行转列数据总行数
        /// </summary>
        /// <param name="portIds">测点数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <returns></returns>
        public int GetAllDataCount(string[] portIds, DateTime dateStart, DateTime dateEnd)
        {
            try
            {
                //查询条件拼接
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " PointId =" + portIdsStr;
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = " PointId IN(" + portIdsStr + ")";
                }
                string whereString = string.Format("Tstamp>='{0}' AND Tstamp<='{1}' AND {2} ", dateStart.ToString("yyyy-MM-dd HH:mm:ss"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr);

                return g_GridViewPager.GetAllDataCount(tableName, "PointId,Tstamp", whereString, connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 生成点位小时数据
        /// </summary>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="portIds">站点列表</param>
        public void ExportData(DateTime dateStart, DateTime dateEnd, string[] portIds)
        {
            try
            {
                if (portIds == null || portIds.Length == 0)
                    return;
                g_DBBiz.ClearParameters();
                SqlParameter pramViewName = new SqlParameter();
                pramViewName = new SqlParameter();
                pramViewName.SqlDbType = SqlDbType.DateTime;
                pramViewName.ParameterName = "@m_begin";
                pramViewName.Value = dateStart;
                g_DBBiz.SetProcedureParameters(pramViewName);

                SqlParameter pramFieldName = new SqlParameter();
                pramFieldName = new SqlParameter();
                pramFieldName.SqlDbType = SqlDbType.DateTime;
                pramFieldName.ParameterName = "@m_end";
                pramFieldName.Value = dateEnd;
                g_DBBiz.SetProcedureParameters(pramFieldName);

                SqlParameter pramKeyName = new SqlParameter();
                pramKeyName = new SqlParameter();
                pramKeyName.SqlDbType = SqlDbType.NVarChar;
                pramKeyName.ParameterName = "@m_portlist";
                pramKeyName.Value = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ","); ;
                g_DBBiz.SetProcedureParameters(pramKeyName);

                g_DBBiz.ExecuteProcNonQuery("UP_AirReport_Hour_Port_Mul", connection);
            }
            catch (Exception ex)
            {
                //return null;
                throw ex;
            }
        }
        #endregion

        #region 时间段内因子比值数据
        /// <summary>
        /// 时间段内因子比值数据
        /// </summary>
        /// <param name="StartDate">开始日期（时）</param>
        /// <param name="EndDate">截止日期（时）</param>
        /// <param name="PointId">站点ID</param>
        /// <param name="FactorCodes">比对因子Code数组</param>
        /// <returns></returns>
        public DataTable GetHourAvgCompareData(DateTime StartDate, DateTime EndDate, Int32 PointId, string[] FactorCodes)
        {
            if (StartDate == DateTime.Parse("1900-01-01") || EndDate == DateTime.Parse("1900-01-01") || PointId <= 0 || FactorCodes.Length != 2)
            {
                return null;
            }
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select t.*,CAST(t." + FactorCodes[0] + "/t." + FactorCodes[1] + " as decimal(11,3)) as ratio from ( ");
            strSql.Append("select Tstamp  ");
            strSql.Append(",MAX(CASE PollutantCode when '" + FactorCodes[0] + "' then CAST(PollutantValue as decimal(11,3)) end ) as '" + FactorCodes[0] + "' ");
            strSql.Append(",MAX(CASE PollutantCode when '" + FactorCodes[1] + "' then CAST(PollutantValue as decimal(11,3)) end ) as '" + FactorCodes[1] + "' ");
            strSql.Append("from AirReport.TB_HourReport ");
            strSql.Append("where Tstamp>=CONVERT(datetime,'" + StartDate + "') and Tstamp<=CONVERT(datetime,'" + EndDate + "') and PointId='" + PointId + "' ");
            strSql.Append("group by Tstamp ");
            strSql.Append(") as t ");
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), connection);
        }
        #endregion

        #region 时间段内测点因子数据
        /// <summary>
        /// 时间段内测点因子数据
        /// </summary>
        /// <param name="StartDate">开始日期（时）</param>
        /// <param name="EndDate">截止日期（时）</param>
        /// <param name="PointId">站点ID</param>
        /// <param name="factorCodes">因子Code数组</param>
        /// <returns></returns>
        public DataTable GetHourDate(DateTime StartDate, DateTime EndDate, Int32 PointId, string[] factorCodes)
        {
            if (StartDate == DateTime.Parse("1900-01-01") || EndDate == DateTime.Parse("1900-01-01") || PointId <= 0 || factorCodes.Length == 0)
            {
                return null;
            }
            //组装日期数据
            string strDate = "";
            for (DateTime sd = StartDate; sd <= EndDate; sd = sd.AddDays(1))
            {
                strDate += sd.ToString("yyyy-MM-dd HH:00") + ";";
            }
            strDate = strDate.Substring(0, strDate.Length - 1);
            string[] DateList = strDate.Split(';');

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Tstamp  ");
            for (int i = 0; i < factorCodes.Length; i++)
            {
                strSql.Append(",MAX(CASE PollutantCode when '" + factorCodes[i] + "' then CAST(dbo.F_Round(dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,','),3) as decimal(11,3)) end ) as '" + factorCodes[i] + "' ");
            }
            strSql.Append("from AirReport.TB_HourReport ");
            strSql.Append("where PointId='" + PointId + "' ");
            if (DateList.Length == 1)
            {
                strSql.Append("and Tstamp=CONVERT(datetime,'" + DateList[0] + "') ");
            }
            else
            {
                for (int j = 0; j < DateList.Length; j++)
                {
                    if (j == 0)
                    {
                        strSql.Append("and ( Tstamp=CONVERT(datetime,'" + DateList[j] + "') ");
                    }
                    else if (j > 0 && j < DateList.Length - 1)
                    {
                        strSql.Append("or Tstamp=CONVERT(datetime,'" + DateList[j] + "') ");
                    }
                    else
                    {
                        strSql.Append("or Tstamp=CONVERT(datetime,'" + DateList[j] + "') ) ");
                    }
                }
            }

            strSql.Append("group by Tstamp ORDER BY Tstamp ");
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), connection);
        }
        #endregion

        #region 获取时间点下的测点，因子数据
        /// <summary>
        /// 获取时间点下的测点，因子数据
        /// </summary>
        /// <param name="PointIds">监测点Id数组</param>
        /// <param name="PollutantCodes">监测因子Code数组</param>
        /// <param name="DateLists">监测日期数组</param>
        /// <returns></returns>
        public DataTable GetHourData(List<int> PointIds, List<string> PollutantCodes, List<DateTime> DateLists)
        {
            if (PointIds == null || PointIds.Count == 0
                || PollutantCodes == null || PollutantCodes.Count == 0
                || DateLists == null || DateLists.Count == 0)
            {
                return null;
            }
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from " + tableName + " where 1=1 ");

            strSql.Append("and ( ");
            for (int j = 0; j < PointIds.Count; j++)
            {
                if (j == 0)
                {
                    strSql.Append("PointId='" + PointIds[j] + "' ");
                }
                else
                {
                    strSql.Append("or PointId='" + PointIds[j] + "' ");
                }
            }
            strSql.Append(" ) ");

            strSql.Append("and ( ");
            for (int j = 0; j < PollutantCodes.Count; j++)
            {
                if (j == 0)
                {
                    strSql.Append("PollutantCode='" + PollutantCodes[j] + "' ");
                }
                else
                {
                    strSql.Append("or PollutantCode='" + PollutantCodes[j] + "' ");
                }
            }
            strSql.Append(" ) ");

            strSql.Append("and ( ");
            for (int j = 0; j < DateLists.Count; j++)
            {
                if (j == 0)
                {
                    strSql.Append("Tstamp=CONVERT(datetime,'" + DateLists[j] + "') ");
                }
                else
                {
                    strSql.Append("or Tstamp=CONVERT(datetime,'" + DateLists[j] + "') ");
                }
            }
            strSql.Append(" ) ");

            strSql.Append("order by Tstamp,PointId ");
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), connection);
        }
        #endregion

        public DataView GetNewHourDataPagerWidthO8(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy)
        {
          orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "PointId,Tstamp";
          recordTotal = 0;
          try
          {
            //取得查询行转列字段拼接
            string factorSql = string.Empty;
            string factorFlagSql = string.Empty;
            string factorWhere = string.Empty;

            foreach (string factor in factors)
            {
              factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
              string factorFlag = factor + "_Status";

              factorFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN AuditFlag END) AS [{1}] ", factor, factorFlag);
              factorWhere += "'" + factor + "',";
            }
            factorWhere = factorWhere.Remove(factorWhere.Length - 1, 1);

            string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
            if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
            {
              portIdsStr = " AND PointId =" + portIdsStr;
            }
            else if (!string.IsNullOrEmpty(portIdsStr))
            {
              portIdsStr = "AND PointId IN(" + portIdsStr + ")";
            }

            orderBy = string.IsNullOrEmpty(orderBy) ? "PointId,Tstamp" : orderBy;
            string fieldName = "PointId,Tstamp" + factorSql + factorFlagSql;
            string groupBy = "PointId,Tstamp";
            string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}' AND PollutantCode in ({2})", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + portIdsStr;
            string tabledt = string.Format(@"(select  PointId,DateTime as Tstamp,'a05027' as PollutantCode,case when Recent8HoursO3NT is null then null  when Recent8HoursO3NT='' then null else cast(Recent8HoursO3NT as decimal(18,4)) end as PollutantValue,'' as AuditFlag from [AMS_MonitorBusiness].[AirRelease].[TB_HourAQI] union all select PointId,Tstamp,PollutantCode,PollutantValue,AuditFlag from [AMS_MonitorBusiness].[AirReport].[TB_HourReport]) as dt");


          string sql=string.Format(@"select {0} from {1} where {2} group by {3} ",fieldName,tabledt,where,groupBy);
          return g_GridViewPager.GridViewPagerCplexSql(sql, pageSize, pageNo, orderBy, connection1, out recordTotal);

          }
          catch (Exception ex)
          {
            throw ex;
          }
        }
      /// <summary>
      /// 区域审核数据查询
      /// </summary>
      /// <param name="portIds"></param>
      /// <param name="factors"></param>
      /// <param name="dtStart"></param>
      /// <param name="dtEnd"></param>
      /// <param name="pageSize"></param>
      /// <param name="pageNo"></param>
      /// <param name="recordTotal"></param>
      /// <param name="orderBy"></param>
      /// <returns></returns>
        public DataView GetNewHourDataPagerWidthRegionO8(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy)
        {
          string tabledt = string.Empty;
          orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "sortNumber desc,Tstamp";
          recordTotal = 0;
          try
          {
            //取得查询行转列字段拼接
            string factorSql = string.Empty;
            string factorFlagSql = string.Empty;
            string factorWhere = string.Empty;
            string factorNewSql = string.Empty;
            string factorFlagNewSql = string.Empty;
            foreach (string factor in factors)
            {
              factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
              factorNewSql += string.Format(",Avg({0}) as [{0}] ", factor);
              string factorFlag = factor + "_Status";

              factorFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN '' END) AS [{1}] ", factor, factorFlag);
              factorFlagNewSql += string.Format(",Max({0}) as [{0}] ", factorFlag);
              factorWhere += "'" + factor + "',";
            }
            factorWhere = factorWhere.Remove(factorWhere.Length - 1, 1);

            string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
            if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
            {
              portIdsStr = " AND PointId =" + portIdsStr;
            }
            else if (!string.IsNullOrEmpty(portIdsStr))
            {
              portIdsStr = "AND PointId IN(" + portIdsStr + ")";
            }

           // orderBy = string.IsNullOrEmpty(orderBy) ? "PointId,Tstamp" : orderBy;
            string fieldName = "PointId,Tstamp" + factorSql+factorFlagSql;
            string newFiledName = " Max(t2.Region) as PointId,Tstamp, sortNumber" + factorNewSql + factorFlagNewSql;
            string groupBy = "PointId,Tstamp";
            string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}' AND PollutantCode in ({2})", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + portIdsStr;
            if (factors.Contains("a05027"))
            {
              tabledt = string.Format(@"(select PointId,Tstamp,d1.PollutantCode,cast([dbo].[F_Round](d1.PollutantValue,case when d2.DecimalDigit is null then 3 when d2.DecimalDigit='' then 3 else d2.DecimalDigit end) as decimal(18,4)) as PollutantValue,AuditFlag from (select  PointId,DateTime as Tstamp,'a05027' as PollutantCode,case when Recent8HoursO3NT is null then null  when Recent8HoursO3NT='' then null else cast(Recent8HoursO3NT as decimal(18,4)) end as PollutantValue,'' as AuditFlag from [AMS_MonitorBusiness].[AirRelease].[TB_HourAQI] union all select PointId,Tstamp,PollutantCode,PollutantValue,AuditFlag from [AMS_MonitorBusiness].[AirReport].[TB_HourReport]) as d1 left join [AMS_BaseData].[dbo].[V_Factor] as d2 on d1.PollutantCode=d2.PollutantCode) as dt");
            //tabledt = string.Format(@"(select  PointId,DateTime as Tstamp,'a05027' as PollutantCode,case when Recent8HoursO3NT is null then null  when Recent8HoursO3NT='' then null else cast(Recent8HoursO3NT as decimal(18,4)) end as PollutantValue,'' as AuditFlag from [AMS_MonitorBusiness].[AirRelease].[TB_HourAQI] union all select PointId,Tstamp,PollutantCode,PollutantValue,AuditFlag from [AMS_MonitorBusiness].[AirReport].[TB_HourReport]) as dt");
            }
            else
            {
              tabledt = "(select PointId,Tstamp,d1.PollutantCode,cast([dbo].[F_Round](d1.PollutantValue,case when d2.DecimalDigit is null then 3 when d2.DecimalDigit='' then 3 else d2.DecimalDigit end) as decimal(18,4)) as PollutantValue from [AMS_MonitorBusiness].[AirReport].[TB_HourReport] as d1 left join [AMS_BaseData].[dbo].[V_Factor] as d2 on d1.PollutantCode=d2.PollutantCode) as dt";
             
            }

           
            string table1 = string.Format(@"(select {0} from {1} where {2} group by {3}) ", fieldName, tabledt, where, groupBy);
            string sql = string.Format(@"select {0} from {1} as t1 left join {2} as t2 on t1.PointId=t2.PortId  inner join [AMS_BaseData].[dbo].[SY_View_CodeMainItem] svcRG
                                on t2.RegionUid = svcRG.ItemGuid
 group by t2.RegionUid,t1.Tstamp,svcRG.sortNumber", newFiledName, table1, "[AMS_BaseData].[dbo].[V_Point_UserConfigNew]");
            return g_GridViewPager.GridViewPagerCplexSql(sql, pageSize, pageNo, orderBy, connection1, out recordTotal);

          }
          catch (Exception ex)
          {
            throw ex;
          }
        }
      /// <summary>
      /// 根据测点获取区域信息
      /// </summary>
      /// <param name="PointID"></param>
      /// <returns></returns>
        public DataTable GetRegionWithPointId(string[] portIds)
        {
          string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
          if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
          {
            portIdsStr = " AND PortId =" + portIdsStr;
          }
          else if (!string.IsNullOrEmpty(portIdsStr))
          {
            portIdsStr = "AND PortId IN(" + portIdsStr + ")";
          }
          string sql = string.Format(@"select  Region from [dbo].[V_Point_UserConfigNew]  as t2 join [dbo].[SY_View_CodeMainItem] svcRG
                                on t2.RegionUid = svcRG.ItemGuid where 1=1 {0} group by Region,sortNumber order by sortNumber desc", portIdsStr);
          return g_DatabaseHelper.ExecuteDataTable(sql, connection3);
        }
    }
}
