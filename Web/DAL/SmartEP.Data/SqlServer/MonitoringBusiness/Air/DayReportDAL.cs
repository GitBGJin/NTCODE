﻿using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.Data.SqlServer.Common.GridView;
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
    /// 名称：DayReportDAL.cs
    /// 创建人：李飞
    /// 创建日期：2015-08-18
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 环境空气发布：日数据处理
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class DayReportDAL
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
        /// <summary>
        /// 数据库表名
        /// </summary>
        private string tableName = "AirReport.TB_DayReport";
        private string tableName2 = "AirRelease.TB_CheckDataDayAQI";
        private string tableName3 = "AirRelease.TB_CheckRegionDayAQI";
        private string tableName4 = "AirReport.TB_RegionDayAQIReport";
        #endregion

        #region << 构造函数 >>
        /// <summary>
        /// 构造函数
        /// </summary>
        public DayReportDAL()
        {
            tableName = SmartEP.Data.Enums.EnumMapping.GetAutoMonitorTableName(ApplicationType.Air, PollutantDataType.Day);
            connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(ApplicationType.Air, PollutantDataType.Day);
        }
        #endregion

        #region << 方法 >>
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
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime")
        {
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "PointId,DateTime";
            recordTotal = 0;
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

                orderBy = string.IsNullOrEmpty(orderBy) ? "PointId,DateTime" : orderBy;
                string fieldName = "PointId,DateTime" + factorSql + factorFlagSql;
                string groupBy = "PointId,DateTime";
                string where = string.Format(" DateTime>='{0}' AND DateTime<='{1}' and datepart(HOUR,DateTime)=0 ", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr;
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
        public DataView GetAvgDataPager(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime")
        {
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "PointId,DateTime";
            recordTotal = 0;
            try
            {
                //取得查询行转列字段拼接
                string factorASql = string.Empty;
                string factorSql = "select PointId,(Convert(nvarchar(13),datetime,120)+':00:00') datetime";
                string factorMarkSql = string.Empty;
                string factorWhere = string.Empty;

                foreach (string factor in factors)
                {
                    factorASql += string.Format(",AVG({0}) {0}", factor);
                    //string factorMark = factor + "_Mark";
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                    //factorMarkSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN Mark END) AS [{1}] ", factor, factorMark);
                    //factorWhere += "'" + factor + "',";
                }
                //factorWhere = factorWhere.Remove(factorWhere.Length - 1, 1);

                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND PointId =" + portIdsStr;
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = "AND PointId IN(" + portIdsStr + ")";
                }

                orderBy = string.IsNullOrEmpty(orderBy) ? "PointId,DateTime" : orderBy;
                string fieldName = "PointId,DateTime" + factorSql + factorMarkSql;
                string groupBy = "PointId,DateTime";
                string where = string.Format(" DateTime>='{0}' AND DateTime<='{1}' ", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr;
                string sql = string.Format(@"Select datetime {0} from ({1} from {2} where {3} group by {4})data group by datetime order by datetime", factorASql, factorSql, tableName, where, groupBy);
                //return g_GridViewPager.GetPivotDataPager(tableName, fieldName, groupBy, orderBy, where, pageSize, pageNo, connection, out recordTotal);
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
        public DataView GetDayDataAvg(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime")
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

                string groupby = "DateTime";
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND PointId =" + portIdsStr;
                    groupby = "PointId,DateTime";
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = "AND PointId IN(" + portIdsStr + ")";
                }

                orderBy = string.IsNullOrEmpty(orderBy) ? "DateTime" : orderBy;
                string fieldName = "PointId,DateTime" + factorSql;
                string where = string.Format(" DateTime>='{0}' AND DateTime<='{1}' and datepart(HOUR,DateTime)=0  AND PollutantCode in ({2})", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + portIdsStr;
                string sql = string.Format(@"select {5}, DateTime
											{3}
											from(
											select {1}
											 from {0}
											where {2}
                                            group by PointId,DateTime
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
        public DataView GetDataPager(string portIds, string[] factors
           , DateTime dtStart, DateTime dtEnd, string orderBy = "PointId,DateTime")
        {
            try
            {
                //取得查询行转列字段拼接
                orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "PointId,DateTime";
                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                string factorWhere = string.Empty;
                //foreach (string factor in factors)
                //{
                //    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                //}
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
                //    portIdsStr = " AND PointId =" + portIdsStr;
                //}
                //else if (!string.IsNullOrEmpty(portIdsStr))
                //{
                //    portIdsStr = "AND PointId IN(" + portIdsStr + ")";
                //}

                string where = string.Format(" DateTime>='{0}' AND DateTime<='{1}' and datepart(HOUR,DateTime)=0 AND A.PollutantCode in ({2})", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + portIdsStr;
                string sql = String.Empty;
                sql = @"select A.[DateTime],A.PointId,A.PollutantCode,B.MonitoringPointName,C.PollutantName,A.PollutantValue from [AMS_MonitorBusiness].[AirReport].[TB_DayReport] A,[AMS_BaseData].[MPInfo].[TB_MonitoringPoint] B,[AMS_BaseData].[Standard].[TB_PollutantCode] C
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
        /// 取得全月均值，最大最小值
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
								,AVG(CONVERT (decimal(18,6),a01001)) as TempValue
								,AVG(CONVERT (decimal(18,6),a01002)) as RHValue
								,AVG(CONVERT (decimal(18,6),a01006)) as PressValue
								,AVG(CONVERT (decimal(18,6),a01008)) as WdValue
								,AVG(CONVERT (decimal(18,6),a01007)) as WsValue
								,Max(a01001) as TempMax
								,Max(a01002) as RHMax
								,Max(a01006) as PressMax
								,Max(a01008) as WdMax
								,Max(a01007) as WsMax
								,Min(a01001) as TempMin
								,Min(a01002) as RHMin
								,Min(a01006) as PressMin
								,Min(a01008) as WdMin
								,Min(a01007) as WsMin
								from
								(
								select PointId,DateTime {1} {2} 
								from {0}
								where  DateTime>='{3}' AND DateTime<='{4}'  {5}
								group by PointId,DateTime
								)a  group by PointId
               ", tableName, factorSql, factorFlagSql, dateStart.ToString("yyyy-MM-dd HH:mm:ss"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
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
        public DataView GetDataPager(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd
            , DateTime dtFrom, DateTime dtTo, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime desc")
        {
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "PointId,DateTime desc,Type";
            recordTotal = 0;
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                foreach (string factor in factors)
                {
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue else -10 END ) AS [{0}] ", factor);
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
                orderBy = string.IsNullOrEmpty(orderBy) ? "PointId,DateTime desc" : orderBy;
                string fieldName = "PointId,'审核' as Type,DateTime" + factorSql + factorFlagSql;
                string groupBy = "PointId,DateTime";
                DateTime dateB = new DateTime();
                DateTime dateE = new DateTime();
                string where = "";
                if (dtFrom == dateB && dtTo == dateE)
                {
                    where = string.Format(" DateTime>='{0}' AND DateTime<='{1}' ", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr;
                }
                else
                {
                    where = string.Format(" ((DateTime>='{0}' AND DateTime<='{1}') or (DateTime>='{2}' AND DateTime<='{3}')) ", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), dtFrom.ToString("yyyy-MM-dd HH:mm:ss"), dtTo.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr;
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
        public DataView GetDataPagerDF(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd
            ,  int pageSize, int pageNo,  string orderBy = "PointId,DateTime desc")
        {
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "PointId,DateTime desc,Type";
            int recordTotal = 0;
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                foreach (string factor in factors)
                {
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue else -10 END ) AS [{0}] ", factor);
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
                orderBy = string.IsNullOrEmpty(orderBy) ? "PointId,DateTime desc" : orderBy;
                string fieldName = "row_number() over(order by DateTime) as Ordernum ,PointId,'审核' as Type,DateTime" + factorSql + factorFlagSql;
                string groupBy = "PointId,DateTime";
                //DateTime dateB = new DateTime();
                //DateTime dateE = new DateTime();
                string where = "";
                //if (dtFrom == dateB && dtTo == dateE)
                //{
                    where = string.Format(" DateTime>='{0}' AND DateTime<='{1}' ", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr;
                //}
                //else
                //{
                //    where = string.Format(" ((DateTime>='{0}' AND DateTime<='{1}') or (DateTime>='{2}' AND DateTime<='{3}')) ", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), dtFrom.ToString("yyyy-MM-dd HH:mm:ss"), dtTo.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr;
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
            , DateTime dtStart, DateTime dtEnd, string orderBy = "PointId,DateTime")
        {
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "PointId,DateTime";
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
                orderBy = string.IsNullOrEmpty(orderBy) ? "PointId,DateTime" : orderBy;
                string fieldName = "rowNum as '序号',PointId,CONVERT(NVARCHAR(19),DateTime,120) as '日期'" + factorSql + factorFlagSql;
                string groupBy = "PointId,DateTime";
                string where = string.Format(" DateTime>='{0}' AND DateTime<='{1}' ", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr;
                return g_GridViewPager.GetPivotDataPager(tableName, fieldName, groupBy, orderBy, where, pageSize, pageNo, connection, out recordTotal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 环境空气质量例行监测成果表导出
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetAQRoutineMonthReportExportData(string[] portIds, IList<IPollutant> factors
            , DateTime dtStart, DateTime dtEnd, string orderBy = "A.PointId,DateTime")
        {
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "A.PointId,DateTime";
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                foreach (IPollutant factor in factors)
                {
                    string factorFlag = factor.PollutantCode + "_Status";
                    Int32 decimalNum = string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum);
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN CAST(dbo.F_Round(PollutantValue,{1}) AS DECIMAL(10,{1})) END ) AS [{0}] ", factor.PollutantCode, decimalNum);
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
                orderBy = string.IsNullOrEmpty(orderBy) ? " order by A.PointId,DateTime" : " order by " + orderBy;
                string fieldName = string.Format(@"PointId,Convert(varchar(10),[DateTime],120) [DateTime],
                                                   DateName(month,[DateTime]) as monthBegin,DateName(day,[DateTime]) as dayBegin,0 as hourBegin,0 as minuteBegin,"
                                              + " DateName(month,[DateTime]) as monthEnd,DateName(day,[DateTime]) as dayEnd,24 as hourEnd,0 as minuteEnd"
                                              + factorSql + factorFlagSql);
                string groupBy = "PointId,DateTime";
                string where = string.Format(" [DateTime]>='{0}' AND [DateTime]<='{1}' ", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr;
                string sql = string.Format(@"SELECT A.*,B.[MaxOneHourO3],B.[Max8HourO3] FROM (SELECT {0} FROM {1} where {2} group by {3} ) AS A JOIN {4} AS B ON A.PointId=B.PointId AND A.[DateTime]=B.[DateTime] {5}"
                                      , fieldName, tableName, where, groupBy, "AirRelease.TB_DayAQI", orderBy);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
                //return g_GridViewPager.GetPivotDataPager(tableName, fieldName, groupBy, orderBy, where, pageSize, pageNo, connection, out recordTotal);
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
                string where = string.Format(" DateTime>='{0}' AND DateTime<='{1}' ", dateStart.ToString("yyyy-MM-dd HH:mm:ss"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr;

                //拼接统计数据字串，使用UNION方式减少统计条数，提升统计速度
                string sql = string.Empty;
                for (int iRow = 0; iRow < factors.Length; iRow++)
                {
                    if (iRow > 0)
                        sql += " UNION ";
                    sql += string.Format(@"
                        SELECT PointId,PollutantCode='{1}'
	                        ,AVG(PollutantValue) AS Value_Avg
	                        ,MAX(PollutantValue) AS Value_Max
	                        ,MIN(PollutantValue) AS Value_Min
                            ,Count(PollutantValue) AS Value_Count
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
        /// 环境空气质量例行监测成果表统计
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataView GetAQRoutineMonthReportStatisticalData(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd)
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
                string where = string.Format(" DateTime>='{0}' AND DateTime<='{1}' ", dateStart.ToString("yyyy-MM-dd HH:mm:ss"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr;

                //拼接统计数据字串，使用UNION方式减少统计条数，提升统计速度
                string sql = string.Empty;
                for (int iRow = 0; iRow < factors.Length; iRow++)
                {
                    if (iRow > 0)
                        sql += " UNION ";
                    sql += string.Format(@"
                        SELECT PointId,PollutantCode='{1}'
	                        ,AVG(PollutantValue) AS Value_Avg
	                        ,MAX(PollutantValue) AS Value_Max
	                        ,MIN(PollutantValue) AS Value_Min
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
                string whereString = string.Format("DateTime>='{0}' AND DateTime<='{1}' AND {2} ", dateStart.ToString("yyyy-MM-dd HH:mm:ss"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr);

                return g_GridViewPager.GetAllDataCount(tableName, "PointId,DateTime", whereString, connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 取得AQI表中日数据替换日数据表中的数据的日数据
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="factors">因子</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="isAllDate">是否显示缺失数据</param>
        /// <returns></returns>
        public DataView GetDataViewFromAQI(string[] portIds, IList<IPollutant> factors, DateTime dateStart, DateTime dateEnd, bool isAllDate)
        {
            try
            {
                string dayTable = tableName;
                string aqiTable = SmartEP.Data.Enums.EnumMapping.GetTableName(SmartEP.Data.Enums.AQIDataType.DayAQI);

                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");

                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorSelSql = string.Empty;
                foreach (IPollutant factor in factors)
                {
                    Int32 decimalNum = string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum);
                    //臭氧
                    if (factor.PollutantCode.Equals("a05024"))
                    {
                        factorSelSql += string.Format(",dbo.F_Round(aqiInfo.[MaxOneHourO3],{0}) as 'MaxOneHourO3'", decimalNum);
                        factorSelSql += string.Format(",dbo.F_Round(aqiInfo.[Max8HourO3],{0}) as 'Max8HourO3'", decimalNum);
                    }
                    //二氧化硫
                    else if (factor.PollutantCode.Equals("a21026")) factorSelSql += string.Format(",dbo.F_Round(aqiInfo.[SO2],{0}) as '{1}'", decimalNum, factor.PollutantCode);
                    //二氧化氮
                    else if (factor.PollutantCode.Equals("a21004")) factorSelSql += string.Format(",dbo.F_Round(aqiInfo.[NO2],{0}) as '{1}'", decimalNum, factor.PollutantCode);
                    //PM10
                    else if (factor.PollutantCode.Equals("a34002")) factorSelSql += string.Format(",dbo.F_Round(aqiInfo.[PM10],{0}) as '{1}'", decimalNum, factor.PollutantCode);
                    //一氧化碳
                    else if (factor.PollutantCode.Equals("a21005")) factorSelSql += string.Format(",dbo.F_Round(aqiInfo.[CO],{0}) as '{1}'", decimalNum, factor.PollutantCode);
                    //PM25
                    else if (factor.PollutantCode.Equals("a34004")) factorSelSql += string.Format(",dbo.F_Round(aqiInfo.[PM25],{0}) as '{1}'", decimalNum, factor.PollutantCode);
                    else factorSelSql += string.Format(",dayInfo.[{0}]", factor.PollutantCode);

                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN dbo.F_Round(PollutantValue,{1}) END ) AS [{0}] ", factor.PollutantCode, decimalNum);
                }
                string sql = string.Empty;
                //显示所有缺失日数据
                if (isAllDate)
                {
                    sql = string.Format(@"
                    with dayInfo as 
                    (
                        select PointId
                            ,DateTime
                            {0}
                        from {1}
                        where PointId IN ({2}) and DateTime>='{5}' and DateTime<='{6}'
                        group by PointId,DateTime
                    )
                    select allInfo.PointId
                        ,allInfo.DateTime
                        {4}
                    from dbo.F_GetAllDataByDay({2},',','{5}','{6}') as allInfo
                    left join {3} as aqiInfo
                        on allInfo.PointId = dayInfo.PointId and allInfo.DateTime = dayInfo.DateTime
                    left join dayInfo
                        on aqiInfo.PointId = dayInfo.PointId and aqiInfo.DateTime = dayInfo.DateTime 
                    where aqiInfo.PointId IN ({2}) and aqiInfo.DateTime>='{5}' and aqiInfo.DateTime<='{6}'
                ", factorSql, dayTable, portIdsStr, aqiTable, factorSelSql, dateStart.ToString("yyyy-MM-dd HH:mm:ss"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else
                {
                    sql = string.Format(@"
                    with dayInfo as 
                    (
                        select PointId
                            ,DateTime
                            {0}
                        from {1}
                        where PointId IN ({2}) and DateTime>='{5}' and DateTime<='{6}'
                        group by PointId,DateTime
                    )
                    select aqiInfo.PointId
                        ,aqiInfo.DateTime
                        {4}
                    from {3} as aqiInfo
                    left join dayInfo
                        on aqiInfo.PointId = dayInfo.PointId and aqiInfo.DateTime = dayInfo.DateTime 
                    where aqiInfo.PointId IN ({2}) and aqiInfo.DateTime>='{5}' and aqiInfo.DateTime<='{6}'
                ", factorSql, dayTable, portIdsStr, aqiTable, factorSelSql, dateStart.ToString("yyyy-MM-dd HH:mm:ss"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 生成点位日数据
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

                g_DBBiz.ExecuteProcNonQuery("UP_AirReport_Day_Port_Mul", connection);
            }
            catch (Exception ex)
            {
                //return null;
                throw ex;
            }
        }
        #region 获取区域数据
        public DataView GetCheckRegionDayAQI(string strWhere)
        {
            try
            {
                string dayTable = tableName3;
                string sql = string.Format(@"select * from {0} where {1}", dayTable, strWhere);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }
        public DataTable GetCheckRegionDataType()
        {
            try
            {
                string dayTable = tableName3;
                string sql = string.Format(@"select distinct(DataType) from {0}", dayTable);
                return g_DatabaseHelper.ExecuteDataTable(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }
        public DataView GetRegionConcentrationDay(string[] RegionUids, DateTime dtStart, DateTime dtEnd, string DataType)
        {
            try
            {
                string monthB = dtStart.ToString("MM-dd");
                string monthE = dtEnd.ToString("MM-dd");
                DateTime baseBeginTime = Convert.ToDateTime(DataType + "-" + monthB + " 00:00:00");

                DateTime baseEndTime = Convert.ToDateTime(DataType + "-" + dtEnd.Month);
                if (baseEndTime.LastDayOfMonth().Day > dtEnd.Day)
                {
                    baseEndTime = Convert.ToDateTime(DataType + "-" + monthE + " 23:59:59");
                }
                else
                {
                    baseEndTime = baseEndTime.LastDayOfMonth();
                }

                string dayTable = tableName3;
                string dayTable1 = tableName4;

                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(RegionUids.ToList<string>(), "','");
                string RegionUidStr = StringExtensions.GetArrayStrNoEmpty(RegionUids.ToList<string>(), "','");
                if (portIdsStr != "")
                {
                    portIdsStr = "'" + portIdsStr + "'";
                }
                if (RegionUidStr != "")
                {
                    RegionUidStr = "'" + RegionUidStr + "'";
                    RegionUidStr = RegionUidStr.Replace("'7e05b94c-bbd4-45c3-919c-42da2e63fd43',", "");
                }
                if (RegionUids.Length > 0) portIdsStr = string.Format(" and RegionUid IN ({0}) ", portIdsStr);
                if (RegionUids.Length > 0) RegionUidStr = string.Format(" and MonitoringRegionUid IN ({0}) ", RegionUidStr);
                string sql = string.Format(@"SELECT RegionUid as MonitoringRegionUid
								,AVG(CONVERT (decimal(18,3),a.PM25)) as a34004
								,AVG(CONVERT (decimal(18,3),a.PM10)) as a34002
								,AVG(CONVERT (decimal(18,3),b.NO2)) as a21004
								,AVG(CONVERT (decimal(18,3),b.SO2)) as a21026
								,AVG(CONVERT (decimal(18,3),b.CO)) as a21005
								,AVG(CONVERT (decimal(18,3),b.Max8HourO3)) as a05024
                                FROM {0} as a left join {6} as b on  a.DateTime=b.ReportDateTime and b.MonitoringRegionUid='7e05b94c-bbd4-45c3-919c-42da2e63fd43'
                                where DataType='{5}' and datepart(MONTH,DateTime)>= {1} and datepart(MONTH,DateTime)<={2}
                                and datepart(DAY,DateTime)>={3} and datepart(DAY,DateTime)<={4} group by RegionUid
                                 union
                                 SELECT MonitoringRegionUid
                                ,AVG(CONVERT (decimal(18,3),PM25)) as a34004
								,AVG(CONVERT (decimal(18,3),PM10)) as a34002
								,AVG(CONVERT (decimal(18,3),NO2)) as a21004
								,AVG(CONVERT (decimal(18,3),SO2)) as a21026
								,AVG(CONVERT (decimal(18,3),CO)) as a21005
								,AVG(CONVERT (decimal(18,3),Max8HourO3)) as a05024
                                FROM {6}
                                WHERE ReportDateTime >= '{7}' and ReportDateTime <= '{8}' {9}
                       GROUP BY MonitoringRegionUid",
                               dayTable, dtStart.Month, dtEnd.Month, dtStart.Day, dtEnd.Day, DataType, dayTable1, baseBeginTime, baseEndTime, RegionUidStr);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }

        public DataView GetRegionYearBaseData(DateTime dtStart, DateTime dtEnd, string DataType)
        {
            try
            {
                string dayTable = tableName3;
                string sql = string.Format(@"
                                SELECT RegionUid as MonitoringRegionUid
                                    ,AVG(CONVERT (decimal(18,6),[AQIValue])) as AQIValue
						,AVG(CONVERT (decimal(18,6),PM25)*1000) as PM25_C
						,AVG(CONVERT (decimal(18,6),PM10)*1000) as PM10_C
						,AVG(CONVERT (decimal(18,6),[SO2])*1000) as SO2_C
						,AVG(CONVERT (decimal(18,6),NO2)*1000) as NO2_C
						,AVG(CONVERT (decimal(18,6),CO)) as CO_C
						,AVG(CONVERT (decimal(18,6),[Max8HourO3])*1000) as O3_C
						,SUM(CASE WHEN [AQIValue] >=0 THEN 1 ELSE 0 END) AS ValidCount
                        ,SUM(CASE WHEN [AQIValue] >=0 and [AQIValue] <= 100 THEN 1 ELSE 0 END) AS StandardCount
						,SUM(CASE WHEN [AQIValue] >= 0 and [AQIValue] <= 50 THEN 1 ELSE 0 END) AS Optimal
						,SUM(CASE WHEN [AQIValue] >= 51 and [AQIValue] <= 100 THEN 1 ELSE 0 END) AS benign
						,SUM(CASE WHEN [AQIValue] >=101 THEN 1 ELSE 0 END) AS ExcessiveCount
						,SUM(CASE WHEN [AQIValue] >= 101 and [AQIValue] <= 150 THEN 1 ELSE 0 END) AS LightPollution
						,SUM(CASE WHEN [AQIValue] >= 151 and [AQIValue] <= 200 THEN 1 ELSE 0 END) AS ModeratePollution
						,SUM(CASE WHEN [AQIValue] >= 201 and [AQIValue] <= 300 THEN 1 ELSE 0 END) AS HighPollution
						,SUM(CASE WHEN [AQIValue] >= 301  THEN 1 ELSE 0 END) AS SeriousPollution
						,SUM(CASE WHEN [PrimaryPollutant] like '%PM2.5%'  THEN 1 ELSE 0 END) AS PM25
						,SUM(CASE WHEN [PrimaryPollutant] like '%PM10%'  THEN 1 ELSE 0 END) AS PM10
						,SUM(CASE WHEN [PrimaryPollutant] like '%O3%'  THEN 1 ELSE 0 END) AS O3
						,SUM(CASE WHEN [PrimaryPollutant] like '%SO2%'  THEN 1 ELSE 0 END) AS SO2
						,SUM(CASE WHEN [PrimaryPollutant] like '%NO2%'  THEN 1 ELSE 0 END) AS NO2
						,SUM(CASE WHEN [PrimaryPollutant] like '%CO%'  THEN 1 ELSE 0 END) AS CO

                                FROM {0} where DataType='{1}' and datepart(MONTH,DateTime)>= {2} and datepart(MONTH,DateTime)<={3}
                                and datepart(DAY,DateTime)>={4} and datepart(DAY,DateTime)<={5} AND [RegionUid] in
                           (
                             '7e05b94c-bbd4-45c3-919c-42da2e63fd43'                 --市区
                            ,'66d2abd1-ca39-4e39-909f-da872704fbfd'					--张家港市
                            ,'d7d7a1fe-493a-4b3f-8504-b1850f6d9eff'					--常熟市
			                ,'57b196ed-5038-4ad0-a035-76faee2d7a98'					--太仓市
			                ,'2e2950cd-dbab-43b3-811d-61bd7569565a'					--昆山市
			                ,'2fea3cb2-8b95-45e6-8a71-471562c4c89c'					--吴江区
		                  )
                        group by RegionUid",
                               dayTable, DataType, dtStart.Month, dtEnd.Month, dtStart.Day, dtEnd.Day, DataType);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }

        public DataView GetRegionSeasonBaseData(DateTime dtStart, DateTime dtEnd, string DataType)
        {
            try
            {
                string dayTable = tableName3;
                string sql = string.Format(@"
                                SELECT RegionUid as MonitoringRegionUid
                                    ,AVG(CONVERT (decimal(18,6),[AQIValue])) as AQIValue
						,AVG(CONVERT (decimal(18,6),PM25)*1000) as PM25Avg
						,AVG(CONVERT (decimal(18,6),PM10)*1000) as PM10Avg
						,AVG(CONVERT (decimal(18,6),[SO2])*1000) as SO2Avg
						,AVG(CONVERT (decimal(18,6),NO2)*1000) as NO2Avg
						,AVG(CONVERT (decimal(18,6),CO)) as COAvg
						,AVG(CONVERT (decimal(18,6),[Max8HourO3])*1000) as O3Avg
                                FROM {0} where DataType='{1}' and datepart(MONTH,DateTime)>= {2} and datepart(MONTH,DateTime)<={3}
                                and datepart(DAY,DateTime)>={4} and datepart(DAY,DateTime)<={5} AND [RegionUid] in
                           (
                             '7e05b94c-bbd4-45c3-919c-42da2e63fd43'                 --市区
		                  )
                        group by RegionUid",
                               dayTable, DataType, dtStart.Month, dtEnd.Month, dtStart.Day, dtEnd.Day, DataType);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 获取时间段内的所有区域的数据
        /// </summary>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <param name="DataType"></param>
        /// <returns></returns>
        public DataView GetRegionBaseDataByDate(DateTime dtStart, DateTime dtEnd, string DataType)
        {
            try
            {
                string monthB = dtStart.ToString("MM-dd");
                string monthE = dtEnd.ToString("MM-dd");
                DateTime baseBeginTime = Convert.ToDateTime(DataType + "-" + monthB + " 00:00:00");

                DateTime baseEndTime = Convert.ToDateTime(DataType + "-" + dtEnd.Month);
                if (baseEndTime.LastDayOfMonth().Day > dtEnd.Day)
                {
                    baseEndTime = Convert.ToDateTime(DataType + "-" + monthE + " 23:59:59");
                }
                else
                {
                    baseEndTime = baseEndTime.LastDayOfMonth();
                }

                string dayTable = tableName3;
                string dayTable1 = tableName4;
                string sql = string.Format(@"
                                SELECT RegionUid as MonitoringRegionUid
                                    ,AVG(CONVERT (decimal(18,6),[AQIValue])) as AQIValue
						,AVG(CONVERT (decimal(18,6),PM25)*1000) as a34004
						,AVG(CONVERT (decimal(18,6),PM10)*1000) as a34002
						,AVG(CONVERT (decimal(18,6),[SO2])*1000) as a21026
						,AVG(CONVERT (decimal(18,6),NO2)*1000) as a21004
						,AVG(CONVERT (decimal(18,6),CO)) as a21005
						,AVG(CONVERT (decimal(18,6),[Max8HourO3])*1000) as a05024
						,SUM(CASE WHEN [AQIValue] > 0 THEN 1 ELSE 0 END) AS ValidCount
                        ,SUM(CASE WHEN [AQIValue] > 0 and [AQIValue] <= 100 THEN 1 ELSE 0 END) AS StandardCount
						,SUM(CASE WHEN [AQIValue] > 0 and [AQIValue] <= 50 THEN 1 ELSE 0 END) AS Optimal
						,SUM(CASE WHEN [AQIValue] >= 51 and [AQIValue] <= 100 THEN 1 ELSE 0 END) AS benign
						,SUM(CASE WHEN [AQIValue] >= 101 THEN 1 ELSE 0 END) AS ExcessiveCount
						,SUM(CASE WHEN [AQIValue] >= 101 and [AQIValue] <= 150 THEN 1 ELSE 0 END) AS LightPollution
						,SUM(CASE WHEN [AQIValue] >= 151 and [AQIValue] <= 200 THEN 1 ELSE 0 END) AS ModeratePollution
						,SUM(CASE WHEN [AQIValue] >= 201 and [AQIValue] <= 300 THEN 1 ELSE 0 END) AS HighPollution
						,SUM(CASE WHEN [AQIValue] >= 301  THEN 1 ELSE 0 END) AS SeriousPollution
						,SUM(CASE WHEN [PrimaryPollutant] like '%PM2.5%'  THEN 1 ELSE 0 END) AS PM25
						,SUM(CASE WHEN [PrimaryPollutant] like '%PM10%'  THEN 1 ELSE 0 END) AS PM10
						,SUM(CASE WHEN [PrimaryPollutant] like '%O3%'  THEN 1 ELSE 0 END) AS O3
						,SUM(CASE WHEN [PrimaryPollutant] like '%SO2%'  THEN 1 ELSE 0 END) AS SO2
						,SUM(CASE WHEN [PrimaryPollutant] like '%NO2%'  THEN 1 ELSE 0 END) AS NO2
						,SUM(CASE WHEN [PrimaryPollutant] like '%CO%'  THEN 1 ELSE 0 END) AS CO

                                FROM {0} where DataType='{1}' and DateTime>= '{2}' and DateTime<='{3}' AND [RegionUid] in
                           (
                             '7e05b94c-bbd4-45c3-919c-42da2e63fd43'                 --市区
		                  )
                        group by RegionUid
                        union
                        SELECT MonitoringRegionUid
                        ,AVG(CONVERT (decimal(18,6),[AQIValue])) as AQIValue
						,AVG(CONVERT (decimal(18,6),PM25)*1000) as a34004
						,AVG(CONVERT (decimal(18,6),PM10)*1000) as a34002
						,AVG(CONVERT (decimal(18,6),[SO2])*1000) as a21026
						,AVG(CONVERT (decimal(18,6),NO2)*1000) as a21004
						,AVG(CONVERT (decimal(18,6),CO)) as a21005
						,AVG(CONVERT (decimal(18,6),[Max8HourO3])*1000) as a05024
						,SUM(CASE WHEN [AQIValue] > 0 THEN 1 ELSE 0 END) AS ValidCount
                        ,SUM(CASE WHEN [AQIValue] > 0 and [AQIValue] <= 100 THEN 1 ELSE 0 END) AS StandardCount
						,SUM(CASE WHEN [AQIValue] > 0 and [AQIValue] <= 50 THEN 1 ELSE 0 END) AS Optimal
						,SUM(CASE WHEN [AQIValue] >= 51 and [AQIValue] <= 100 THEN 1 ELSE 0 END) AS benign
						,SUM(CASE WHEN [AQIValue] >= 101 THEN 1 ELSE 0 END) AS ExcessiveCount
						,SUM(CASE WHEN [AQIValue] >= 101 and [AQIValue] <= 150 THEN 1 ELSE 0 END) AS LightPollution
						,SUM(CASE WHEN [AQIValue] >= 151 and [AQIValue] <= 200 THEN 1 ELSE 0 END) AS ModeratePollution
						,SUM(CASE WHEN [AQIValue] >= 201 and [AQIValue] <= 300 THEN 1 ELSE 0 END) AS HighPollution
						,SUM(CASE WHEN [AQIValue] >= 301  THEN 1 ELSE 0 END) AS SeriousPollution
						,SUM(CASE WHEN [PrimaryPollutant] like '%PM2.5%'  THEN 1 ELSE 0 END) AS PM25
						,SUM(CASE WHEN [PrimaryPollutant] like '%PM10%'  THEN 1 ELSE 0 END) AS PM10
						,SUM(CASE WHEN [PrimaryPollutant] like '%O3%'  THEN 1 ELSE 0 END) AS O3
						,SUM(CASE WHEN [PrimaryPollutant] like '%SO2%'  THEN 1 ELSE 0 END) AS SO2
						,SUM(CASE WHEN [PrimaryPollutant] like '%NO2%'  THEN 1 ELSE 0 END) AS NO2
						,SUM(CASE WHEN [PrimaryPollutant] like '%CO%'  THEN 1 ELSE 0 END) AS CO

                                FROM {4} 
                                WHERE ReportDateTime >= '{2}' and ReportDateTime <= '{3}' AND [MonitoringRegionUid] in
                           (
			                '66d2abd1-ca39-4e39-909f-da872704fbfd'					--张家港市
                            ,'d7d7a1fe-493a-4b3f-8504-b1850f6d9eff'					--常熟市
			                ,'57b196ed-5038-4ad0-a035-76faee2d7a98'					--太仓市
			                ,'2e2950cd-dbab-43b3-811d-61bd7569565a'					--昆山市
			                ,'2fea3cb2-8b95-45e6-8a71-471562c4c89c'					--吴江区
                            ,'e1c104f3-aaf3-4d0e-9591-36cdc83be15a'					--吴中区
			                ,'8756bd44-ff18-46f7-aedf-615006d7474c'					--相城区
			                ,'6a4e7093-f2c6-46b4-a11f-0f91b4adf379'					--姑苏区
			                ,'69a993ff-78c6-459b-9322-ee77e0c8cd68'					--工业园区
			                ,'f320aa73-7c55-45d4-a363-e21408e0aac3'					--高新区
		                ) 
                       GROUP BY MonitoringRegionUid",
                               dayTable, DataType, baseBeginTime, baseEndTime, dayTable1);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }

        #endregion
        #region 获取数据
        public DataView GetCheckDataDayAQI(string strWhere)
        {
            try
            {
                string dayTable = tableName2;
                string sql = string.Format(@"select * from {0} where {1}", dayTable, strWhere);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }
        public DataTable GetCheckDataType()
        {
            try
            {
                string dayTable = tableName2;
                string sql = string.Format(@"select distinct(DataType) from {0}", dayTable);
                return g_DatabaseHelper.ExecuteDataTable(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }
        public DataView GetConcentrationDay(string[] portIds, DateTime dtStart, DateTime dtEnd, string DataType)
        {
            try
            {
                string dayTable = tableName2;
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                if (portIds.Length > 0) portIdsStr = string.Format(" and PointId IN ({0}) ", portIdsStr);

                string sql = string.Format(@"SELECT PointId
                                ,AVG(CONVERT (decimal(18,3),PM25)) as a34004
								,AVG(CONVERT (decimal(18,3),PM10)) as a34002
								,AVG(CONVERT (decimal(18,3),NO2)) as a21004
								,AVG(CONVERT (decimal(18,3),SO2)) as a21026
								,AVG(CONVERT (decimal(18,3),CO)) as a21005
								,AVG(CONVERT (decimal(18,3),Max8HourO3)) as a05024
                                FROM {0} where DataType='{5}' and datepart(MONTH,DateTime)>= {1} and datepart(MONTH,DateTime)<={2}
                                and datepart(DAY,DateTime)>={3} and datepart(DAY,DateTime)<={4} {6} group by PointId",
                               dayTable, dtStart.Month, dtEnd.Month, dtStart.Day, dtEnd.Day, DataType, portIdsStr);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion
        #region 插入数据
        public void insertTable(DataTable dt, string DataType, string[] portIds, bool isPointTable = true)
        {
            List<CommandInfo> sqllist = new List<CommandInfo>();
            if (portIds == null || portIds.Length == 0)
                return;
            string SqlColumns = string.Empty;
            string dayTable = tableName2;
            string RegionTable = tableName3;
            //StringBuilder strdaySql = new StringBuilder();
            //strdaySql.Append("delete from ");
            //strdaySql.Append(dayTable);
            //strdaySql.Append(" where DataType='@DataType';");
            //SqlParameter[] parametersDay = 
            //{ 
            //                                   new SqlParameter("@DataType", SqlDbType.NVarChar,50)
            //                            };
            //parametersDay[0].Value = DataType;
            //CommandInfo cmdDay = new CommandInfo(strdaySql.ToString(), parametersDay);
            //sqllist.Add(cmdDay);
            //StringBuilder strRegionSql = new StringBuilder();
            //strRegionSql.Append("delete from ");
            //strRegionSql.Append(RegionTable);
            //strRegionSql.Append(" where DataType='@DataType';");
            //SqlParameter[] parametersRegion = 
            //{ 
            //                                   new SqlParameter("@DataType", SqlDbType.NVarChar,50)
            //                            };
            //parametersRegion[0].Value = DataType;
            StringBuilder strdaySql = new StringBuilder();
            strdaySql.Append("delete from ");
            strdaySql.Append(dayTable);
            strdaySql.Append(" where DataType='");
            strdaySql.Append(DataType);
            strdaySql.Append("';");
            strdaySql.Append("delete from ");
            strdaySql.Append(RegionTable);
            strdaySql.Append(" where DataType='");
            strdaySql.Append(DataType);
            strdaySql.Append("';");
            g_DatabaseHelper.ExecuteNonQuery(strdaySql.ToString(), connection);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string strPointId = dt.Rows[i]["portid"].ToString();
                int pointId = int.TryParse(strPointId, out pointId) ? pointId : 0;
                if (pointId != 0)
                {
                    #region Point
                    DateTime? datetime = null;
                    DateTime dttemp = DateTime.Now.AddYears(+10);
                    DateTime dtime = DateTime.TryParse(dt.Rows[i]["datetime"].ToString(), out dtime) ? dtime : dttemp;
                    if (DateTime.Compare(dttemp, dtime) != 0)
                    {
                        datetime = dtime;
                    }
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("insert into ");
                    strSql.Append(dayTable);
                    strSql.Append("(PointId,DateTime,DataType,SO2,NO2,PM10,CO,MaxOneHourO3,Max8HourO3,PM25,PointName)");
                    strSql.Append("values(@PointId,@datetime,@DataType,@SO2,@NO2,@PM10,@CO,@MaxOneHourO3,@Max8HourO3,@PM25,@PointName);");
                    SqlParameter[] parameters = { 
                                new SqlParameter("@PointId", SqlDbType.Int),
                                new SqlParameter("@datetime", SqlDbType.DateTime),
                                new SqlParameter("@DataType", SqlDbType.NVarChar,50),
                                new SqlParameter("@SO2", SqlDbType.NVarChar, 20),
                                new SqlParameter("@NO2", SqlDbType.NVarChar,20),
                                new SqlParameter("@PM10", SqlDbType.NVarChar,20),
                                new SqlParameter("@CO", SqlDbType.NVarChar, 20),
                                new SqlParameter("@MaxOneHourO3", SqlDbType.NVarChar, 20),
                                new SqlParameter("@Max8HourO3", SqlDbType.NVarChar,20),
                                new SqlParameter("@PM25", SqlDbType.NVarChar,20),
                                new SqlParameter("@PointName", SqlDbType.NVarChar,50)};
                    parameters[0].Value = pointId;
                    parameters[1].Value = datetime;
                    parameters[2].Value = DataType;
                    parameters[3].Value = dt.Rows[i]["so2"];
                    parameters[4].Value = dt.Rows[i]["no2"];
                    parameters[5].Value = dt.Rows[i]["pm10"];
                    parameters[6].Value = dt.Rows[i]["co"];
                    parameters[7].Value = dt.Rows[i]["o3_1h"];
                    parameters[8].Value = dt.Rows[i]["o3_8h"];
                    parameters[9].Value = dt.Rows[i]["pm25"];
                    parameters[10].Value = dt.Rows[i]["portname"];
                    CommandInfo cmd = new CommandInfo(strSql.ToString(), parameters);
                    sqllist.Add(cmd);
                    #endregion
                }
                else if (!string.IsNullOrWhiteSpace(strPointId))
                {
                    #region Region
                    string regionUid = dt.Rows[i]["portid"].ToString();
                    if (!string.IsNullOrWhiteSpace(regionUid))
                    {
                        DateTime? datetime = null;
                        DateTime dttemp = DateTime.Now.AddYears(+10);
                        DateTime dtime = DateTime.TryParse(dt.Rows[i]["datetime"].ToString(), out dtime) ? dtime : dttemp;
                        if (DateTime.Compare(dttemp, dtime) != 0)
                        {
                            datetime = dtime;
                        }
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("insert into ");
                        strSql.Append(RegionTable);
                        strSql.Append("(RegionUid,DateTime,DataType,SO2,NO2,PM10,CO,MaxOneHourO3,Max8HourO3,PM25,PointName)");
                        strSql.Append("values(@RegionUid,@datetime,@DataType,@SO2,@NO2,@PM10,@CO,@MaxOneHourO3,@Max8HourO3,@PM25,@PointName);");
                        SqlParameter[] parameters = { 
                                new SqlParameter("@RegionUid", SqlDbType.NVarChar,50),
                                new SqlParameter("@datetime", SqlDbType.DateTime),
                                new SqlParameter("@DataType", SqlDbType.NVarChar,50),
                                new SqlParameter("@SO2", SqlDbType.NVarChar, 20),
                                new SqlParameter("@NO2", SqlDbType.NVarChar,20),
                                new SqlParameter("@PM10", SqlDbType.NVarChar,20),
                                new SqlParameter("@CO", SqlDbType.NVarChar, 20),
                                new SqlParameter("@MaxOneHourO3", SqlDbType.NVarChar, 20),
                                new SqlParameter("@Max8HourO3", SqlDbType.NVarChar,20),
                                new SqlParameter("@PM25", SqlDbType.NVarChar,20),
                                new SqlParameter("@PointName", SqlDbType.NVarChar,50)};
                        parameters[0].Value = regionUid;
                        parameters[1].Value = datetime;
                        parameters[2].Value = DataType;
                        parameters[3].Value = dt.Rows[i]["so2"];
                        parameters[4].Value = dt.Rows[i]["no2"];
                        parameters[5].Value = dt.Rows[i]["pm10"];
                        parameters[6].Value = dt.Rows[i]["co"];
                        parameters[7].Value = dt.Rows[i]["o3_1h"];
                        parameters[8].Value = dt.Rows[i]["o3_8h"];
                        parameters[9].Value = dt.Rows[i]["pm25"];
                        parameters[10].Value = dt.Rows[i]["portname"];
                        CommandInfo cmd = new CommandInfo(strSql.ToString(), parameters);
                        sqllist.Add(cmd);
                    }
                    #endregion
                }
            }

            g_DatabaseHelper.ExecuteSqlTranWithIndentity(sqllist, connection);

            g_DBBiz.ClearParameters();
            SqlParameter pramViewName = new SqlParameter();
            pramViewName = new SqlParameter();
            pramViewName.SqlDbType = SqlDbType.NVarChar;
            pramViewName.ParameterName = "@m_DataType";
            pramViewName.Value = DataType;
            g_DBBiz.SetProcedureParameters(pramViewName);

            SqlParameter pramKeyName = new SqlParameter();
            pramKeyName = new SqlParameter();
            pramKeyName.SqlDbType = SqlDbType.NVarChar;
            pramKeyName.ParameterName = "@m_portlist";
            pramKeyName.Value = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ","); ;
            g_DBBiz.SetProcedureParameters(pramKeyName);

            //g_DBBiz.ExecuteProcNonQuery("UP_AirRelease_TB_CheckDataDayAQI", connection);
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
        public DataTable GetDayData(List<int> PointIds, List<string> PollutantCodes, List<DateTime> DateLists)
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
                    strSql.Append("DateTime=CONVERT(datetime,'" + DateLists[j] + "') ");
                }
                else
                {
                    strSql.Append("or DateTime=CONVERT(datetime,'" + DateLists[j] + "') ");
                }
            }
            strSql.Append(" ) ");

            strSql.Append("order by DateTime,PointId ");
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), connection);
        }
        #endregion
        #endregion
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
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetDayDataRegionPager(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy)
        {
          string tabledt = string.Empty;
          orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "sortNumber desc,DateTime";
          recordTotal = 0;
          try
          {
            //取得查询行转列字段拼接
            string factorSql = string.Empty;
            string factorNewSql = string.Empty;
            string factorWhere = string.Empty;
            foreach (string factor in factors)
            {
              factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
              factorNewSql += string.Format(",Avg({0}) as [{0}] ", factor);
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

            //orderBy = string.IsNullOrEmpty(orderBy) ? "PointId,DateTime" : orderBy;
            string fieldName = "PointId,DateTime" + factorSql;
            string newFiledName = " Max(t2.Region) as PointId,DateTime, sortNumber" + factorNewSql ;
            string groupBy = "PointId,DateTime";
            string where = string.Format(" DateTime>='{0}' AND DateTime<='{1}' and datepart(HOUR,DateTime)=0 AND PollutantCode in ({2}) ", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"),factorWhere) + portIdsStr;
            tabledt = "(select PointId,DateTime,d1.PollutantCode,cast([dbo].[F_Round](d1.PollutantValue,case when d2.DecimalDigit is null then 3 when d2.DecimalDigit='' then 3 else d2.DecimalDigit end) as decimal(18,4)) as PollutantValue from [AMS_MonitorBusiness].[AirReport].[TB_DayReport] as d1 left join [AMS_BaseData].[dbo].[V_Factor] as d2 on d1.PollutantCode=d2.PollutantCode) as dt";
            string table1 = string.Format(@"(select {0} from {1} where {2} group by {3}) ", fieldName, tabledt, where, groupBy);
            string sql = string.Format(@"select {0} from {1} as t1 left join {2} as t2 on t1.PointId=t2.PortId  inner join [AMS_BaseData].[dbo].[SY_View_CodeMainItem] svcRG
                               on t2.RegionUid = svcRG.ItemGuid
group by t2.RegionUid,t1.DateTime,svcRG.sortNumber", newFiledName, table1, "[AMS_BaseData].[dbo].[V_Point_UserConfigNew]");
           return g_GridViewPager.GridViewPagerCplexSql(sql, pageSize, pageNo, orderBy, connection1, out recordTotal);
          }
          catch (Exception ex)
          {
            throw ex;
          }
        }

    }
}
