﻿using log4net;
using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.Data.SqlServer.Common.GridView;
using SmartEP.Utilities.AdoData;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.AutoMonitoring
{
    /// <summary>
    /// 名称：InfectantDAL.cs
    /// 创建人：李飞
    /// 创建日期：2015-08-18
    /// 维护人员：
    /// 最新维护人员：刘晋
    /// 最新维护日期：2017-06-01
    /// 功能摘要：
    /// 环境空气发布：原始数据处理
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class InfectantDAL
    {
        #region << 变量 >>
        /// <summary>
        /// 数据库处理类
        /// </summary>
        protected DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();

        /// <summary>
        /// 虚拟分页类
        /// </summary>
        GridViewPagerDAL g_GridViewPager = Singleton<GridViewPagerDAL>.GetInstance();

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        protected string connection = string.Empty;

        /// <summary>
        /// 数据库表名
        /// </summary>
        protected string tableName = string.Empty;
        #endregion
        ILog log = LogManager.GetLogger("FileLogging");//获取一个日志记录器
        #region << 构造函数 >>
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="applicationType">应用类型（空气、地表水、噪声）</param>
        /// <param name="autoMonitorType">自动监测数据类型</param>
        public InfectantDAL(ApplicationType applicationType, PollutantDataType autoMonitorType)
        {
            tableName = SmartEP.Data.Enums.EnumMapping.GetAutoMonitorTableName(applicationType, autoMonitorType);
            connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(applicationType, autoMonitorType);
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
        public DataView GetDataPagerWithO3(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Tstamp")
        {
            recordTotal = 0;
            try
            {
                StringBuilder sb = new StringBuilder();
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                string factorDataFlagSql = string.Empty;
                string factorAuditFlagSql = string.Empty;
                string factorWhere = string.Empty;
                if (factors.Contains("a05024"))
                {
                    sb.Append(string.Format(",AVG(CASE(PollutantCode) WHEN 'a05024' THEN PollutantValue END ) AS 'a05024'  "));
                    factorFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN 'a05024' THEN Status END) AS 'a05024_Status' ");
                    factorDataFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN 'a05024' THEN DataFlag END) AS 'a05024_DataFlag' ");
                    factorAuditFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN 'a05024' THEN AuditFlag END) AS 'a05024_AuditFlag' ");
                }
                

                foreach (string factor in factors)
                {
                    if (factor != "a05024")
                    {
                        string factorFlag = factor + "_Status";
                        string factorDataFlag = factor + "_DataFlag";
                        string factorAuditFlag = factor + "_AuditFlag";
                        factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN convert(varchar(50),PollutantValue) END ) AS [{0}] ", factor);
                        factorFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN Status END) AS [{1}] ", factor, factorFlag);
                        factorDataFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN DataFlag END) AS [{1}] ", factor, factorDataFlag);
                        factorAuditFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN AuditFlag END) AS [{1}] ", factor, factorAuditFlag);
                        factorWhere += "'" + factor + "',";
                    }
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
                string fieldName = "PointId,Tstamp" + factorSql + factorFlagSql + factorDataFlagSql + factorAuditFlagSql;
                string groupBy = "PointId,Tstamp";
                string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}'  AND PollutantCode in ({2})", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + portIdsStr;
                string where2 = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}' ", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + portIdsStr;
                string sql = string.Format(@"select * from (SELECT {0} FROM {1} WHERE {2} GROUP BY {3} ) A
                                            left join
                                            (select CONVERT(nvarchar(15),Tstamp,21) BTstamp {4} from Air.TB_InfectantBy1
                                            where {5} group by CONVERT(nvarchar(15),Tstamp,21)) B on CONVERT(nvarchar(15),B.BTstamp,21) = CONVERT(nvarchar(15),A.Tstamp,21)
                                            ORDER BY PointId,A.Tstamp", fieldName, tableName, where, groupBy, sb.ToString(), where2);
                DataView dv = g_DatabaseHelper.ExecuteDataView(sql, connection);
                for (int i = dv.Table.Columns.Count - 1; i > 0; i--)
                {
                    if (dv.Table.Columns[i].ColumnName.Equals("BTstamp"))
                    {
                        dv.Table.Columns.Remove("BTstamp");
                    }
                }
                recordTotal = dv.Count;
                return dv;
                //return g_GridViewPager.GetPivotDataPager(tableName, fieldName, groupBy, orderBy, where, pageSize, pageNo, connection, out recordTotal);
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
            recordTotal = 0;
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                string factorDataFlagSql = string.Empty;
                string factorAuditFlagSql = string.Empty;
                string factorWhere = string.Empty;

                foreach (string factor in factors)
                {
                    string factorFlag = factor + "_Status";
                    string factorDataFlag = factor + "_DataFlag";
                    string factorAuditFlag = factor + "_AuditFlag";
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                    factorFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN Status END) AS [{1}] ", factor, factorFlag);
                    factorDataFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN DataFlag END) AS [{1}] ", factor, factorDataFlag);
                    factorAuditFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN AuditFlag END) AS [{1}] ", factor, factorAuditFlag);
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
                string fieldName = "PointId,Tstamp" + factorSql + factorFlagSql + factorDataFlagSql + factorAuditFlagSql;
                string groupBy = "PointId,Tstamp";
                string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}'  AND PollutantCode in ({2})", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + portIdsStr;
                string tableStr = string.Format("(SELECT  row_number() over(partition by PointId,Tstamp,PollutantCode order by id desc ) rn,PointId,Tstamp,PollutantCode,PollutantValue,Status,DataFlag,AuditFlag FROM {1} where {0}) ASD",where,tableName);
                string sql = string.Format("SELECT {0} FROM {1} WHERE {2} GROUP BY {3} ORDER BY {4}", fieldName, tableStr, "ASD.rn=1", groupBy, orderBy);
                
                if (tableName != "Air.TB_InfectantBy60")
                {
                    return g_GridViewPager.GetPivotDataPager(tableName, fieldName, groupBy, orderBy, where, pageSize, pageNo, connection, out recordTotal);
                }
                else
                {
                    DataView dv = g_DatabaseHelper.ExecuteDataView(sql, connection);
                    recordTotal = dv.Count;
                    return dv;
                }
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
        public DataView GetAvgDataPagerMin1(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Tstamp")
        {
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
        public DataView GetAvgDataPagerMin5(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Tstamp")
        {
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
        public DataView GetAvgDataPagerMin60(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Tstamp")
        {
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
        /// 常规站补充缺失数据时间
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
        public DataView GetDataPagerAllTime(string[] portIds, string[] factors
    , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId asc,tstamp desc")
        {
            recordTotal = 0;
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                string factorDataFlagSql = string.Empty;
                string factorAuditFlagSql = string.Empty;
                string factorWhere = string.Empty;
                string factorFieldSql = string.Empty;

                foreach (string factor in factors)
                {
                    string factorFlag = factor + "_Status";
                    string factorDataFlag = factor + "_DataFlag";
                    string factorAuditFlag = factor + "_AuditFlag";
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                    factorFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN Status END) AS [{1}] ", factor, factorFlag);
                    factorDataFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN DataFlag END) AS [{1}] ", factor, factorDataFlag);
                    factorAuditFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN AuditFlag END) AS [{1}] ", factor, factorAuditFlag);
                    factorWhere += "'" + factor + "',";
                    factorFieldSql += "," + factor;
                    factorFieldSql += "," + factorFlag;
                    factorFieldSql += "," + factorDataFlag;
                    factorFieldSql += "," + factorAuditFlag;
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

                orderBy = string.IsNullOrEmpty(orderBy) ? "order by PointId,Tstamp" : "order by " + orderBy;
                string fieldName = "PointId,Tstamp" + factorSql + factorFlagSql + factorDataFlagSql + factorAuditFlagSql;
                string groupBy = "PointId,Tstamp";
                string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}'  AND PollutantCode in ({2})", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + portIdsStr;

                string dataSql = string.Format(@"select {0} from {1} where {2} group by {3} ", fieldName, tableName, where, groupBy);
                //补充时间段
                string sql = string.Empty;
                string allHoursSql = string.Empty;
                //由于实时数据通常晚1小时到数据库（离子色谱仪晚两个小时），补数据时间需要剔除当前小时及之后的小时
                DateTime AllHourdtEndNew = Convert.ToDateTime(dtEnd.ToString("yyyy-MM-dd HH:00")) >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00")) ?
                    DateTime.Now.AddHours(-1) : dtEnd;

                allHoursSql += string.Format(@"select time.PointId,time.Tstamp{0} from dbo.SY_F_GetAllDataByHour('{1}',',','{2}','{3}') time", factorFieldSql, StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ","), dtStart.ToString("yyyy-MM-dd HH:mm:ss"), AllHourdtEndNew.ToString("yyyy-MM-dd HH:mm:ss"));

                sql += string.Format(@"{0} left join ({1}) data
                        on convert(varchar(13),time.tstamp,120)=convert(varchar(13),data.Tstamp,120)
                        and time.pointId=data.PointId"
                    , allHoursSql, dataSql);
                //DataView dv = g_DatabaseHelper.ExecuteDataView(sql, connection);
                DataView dv = g_GridViewPager.GridViewPagerCplexSql(sql, pageSize, pageNo, orderBy, connection, out recordTotal);
                return dv;
                //return g_GridViewPager.GetPivotDataPager(tableName, fieldName, groupBy, orderBy, where, pageSize, pageNo, connection, out recordTotal);
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
        public DataView GetDataPagerNew(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Tstamp")
        {
            recordTotal = 0;
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                string factorDataFlagSql = string.Empty;
                string factorAuditFlagSql = string.Empty;
                string factorWhere = string.Empty;

                foreach (string factor in factors)
                {
                    string factorFlag = factor + "_Status";
                    string factorDataFlag = factor + "_DataFlag";
                    string factorAuditFlag = factor + "_AuditFlag";
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                    factorFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN Status END) AS [{1}] ", factor, factorFlag);
                    factorDataFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN DataFlag END) AS [{1}] ", factor, factorDataFlag);
                    factorAuditFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN AuditFlag END) AS [{1}] ", factor, factorAuditFlag);
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
                string fieldName = "PointId,Tstamp" + factorSql + factorFlagSql + factorDataFlagSql + factorAuditFlagSql;
                string groupBy = "PointId,Tstamp";
                string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}'  AND PollutantCode in ({2})", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + portIdsStr;
                string sql = string.Format("SELECT {0} FROM {1} WHERE {2} GROUP BY {3} ORDER BY {4}", fieldName, tableName, where, groupBy, orderBy);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
                //return g_GridViewPager.GetPivotDataPager(tableName, fieldName, groupBy, orderBy, where, pageSize, pageNo, connection, out recordTotal);
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
        public DataView GetVOCsKQYDataPager(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Tstamp")
        {
            recordTotal = 0;
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                string factorDataFlagSql = string.Empty;
                string factorAuditFlagSql = string.Empty;
                string factorWhere = string.Empty;

                foreach (string factor in factors)
                {
                    string factorFlag = factor + "_Status";
                    string factorDataFlag = factor + "_DataFlag";
                    string factorAuditFlag = factor + "_AuditFlag";
                    factorSql += string.Format(",MAX(CASE (PollutantCode) WHEN '{0}' THEN CASE WHEN Datename(hour,Tstamp)=0 THEN Null ELSE PollutantValue END END) AS [{0}] ", factor);
                    factorFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN Status END) AS [{1}] ", factor, factorFlag);
                    factorDataFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN DataFlag END) AS [{1}] ", factor, factorDataFlag);
                    factorAuditFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN AuditFlag END) AS [{1}] ", factor, factorAuditFlag);
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
                string fieldName = "PointId,Tstamp" + factorSql + factorFlagSql + factorDataFlagSql + factorAuditFlagSql;
                string groupBy = "PointId,Tstamp";
                string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}'  AND PollutantCode in ({2})", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + portIdsStr;
                string sql = string.Format(@"select {0} from {1} where {2} group by {3} order by {4} ", fieldName, tableName, where, groupBy, orderBy);
                DataView dv=g_DatabaseHelper.ExecuteDataView(sql, connection);
                recordTotal = dv.ToTable().Rows.Count;
                return dv;
                
                //return g_GridViewPager.GetPivotDataPager(tableName, fieldName, groupBy, orderBy, where, pageSize, pageNo, connection, out recordTotal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 取得VOC外标原始小时虚拟分页查询数据和总行数(行转列数据)
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
        public DataView GetDataVOCWPager(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Tstamp")
        {
            recordTotal = 0;
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                string factorDataFlagSql = string.Empty;
                string factorAuditFlagSql = string.Empty;
                string factorWhere = string.Empty;

                foreach (string factor in factors)
                {
                    string factorFlag = factor + "_Status";
                    string factorDataFlag = factor + "_DataFlag";
                    string factorAuditFlag = factor + "_AuditFlag";
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                    factorFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN Status END) AS [{1}] ", factor, factorFlag);
                    factorDataFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN DataFlag END) AS [{1}] ", factor, factorDataFlag);
                    factorAuditFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN AuditFlag END) AS [{1}] ", factor, factorAuditFlag);
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
                string fieldName = "PointId,Tstamp" + factorSql + factorFlagSql + factorDataFlagSql + factorAuditFlagSql;
                string groupBy = "PointId,Tstamp";
                string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}'  AND PollutantCode in ({2}) AND  Status='C'", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + portIdsStr;
                string tableStr = string.Format("(SELECT  row_number() over(partition by PointId,Tstamp,PollutantCode order by id desc ) rn,PointId,Tstamp,PollutantCode,PollutantValue,Status,DataFlag,AuditFlag FROM Air.TB_InfectantBy60 where {0}) ASD", where);
                string sql = string.Format("SELECT {0} FROM {1} WHERE {2} GROUP BY {3} ORDER BY {4}", fieldName, tableStr, "ASD.rn=1", groupBy, orderBy);
                DataView dv = g_DatabaseHelper.ExecuteDataView(sql, connection);
                recordTotal = dv.Count;
                return dv;
                //return g_GridViewPager.GetPivotDataPager(tableName, fieldName, groupBy, orderBy, where, pageSize, pageNo, connection, out recordTotal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 取得VOC外标原始小时虚拟分页查询数据和总行数(行转列数据)
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
        public DataView GetDataVOCKQYPager(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Tstamp")
        {
            recordTotal = 0;
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                string factorDataFlagSql = string.Empty;
                string factorAuditFlagSql = string.Empty;
                string factorWhere = string.Empty;

                foreach (string factor in factors)
                {
                    string factorFlag = factor + "_Status";
                    string factorDataFlag = factor + "_DataFlag";
                    string factorAuditFlag = factor + "_AuditFlag";
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                    factorFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN Status END) AS [{1}] ", factor, factorFlag);
                    factorDataFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN DataFlag END) AS [{1}] ", factor, factorDataFlag);
                    factorAuditFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN AuditFlag END) AS [{1}] ", factor, factorAuditFlag);
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
                string fieldName = "PointId,Tstamp" + factorSql + factorFlagSql + factorDataFlagSql + factorAuditFlagSql;
                string groupBy = "PointId,Tstamp";
                string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}'  AND PollutantCode in ({2}) AND  DATENAME(HOUR,Tstamp)=0", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + portIdsStr;
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
        public DataView GetHourAvgData(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, string type, int pageSize, int pageNo, out int recordTotal, string orderBy = "Tstamp")
        {
            recordTotal = 0;
            try
            {
                tableName = "Air.TB_InfectantBy60";
                if (type == "Min1")
                    tableName = "Air.TB_InfectantBy1";
                if (type == "Min5")
                    tableName = "Air.TB_InfectantBy5";
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
                string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}'  AND PollutantCode in ({2})", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + portIdsStr;
                string sql = string.Format(@"select {5}, convert(DateTime,Tstamp+':00:00.000') Tstamp
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
        /// 激光雷达图表数据查询
        /// </summary>
        /// <param name="Quality"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <param name="recordTotal"></param>
        /// <param name="DtStart"></param>
        /// <param name="DtEnd"></param>
        /// <returns></returns>
        public DataView GetLadarData(string Quality, int pageSize, int pageNo, out int recordTotal, string DtStart, string DtEnd,string stch,string stchMin)
        {
            recordTotal = 0;
            try
            {
                string sql = string.Format(@"select DateTime, Height,Number from [dbo].[TB_SuperStation_jiguangleida]
                                                where DateTime<='{2}' and DateTime>='{1}' and Height<='{3}' and Height>='{4}'  and DataType='{0}'  order by Height,DateTime"
                        , Quality, DtStart, DtEnd,stch,stchMin);

                DataTable dt = g_DatabaseHelper.ExecuteDataTable(sql, "AMS_AirAutoMonitorConnection");
                recordTotal = dt.Rows.Count;
                return dt.DefaultView;
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 激光雷达列表数据查询
        /// </summary>
        /// <param name="Quality"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <param name="recordTotal"></param>
        /// <param name="DtStart"></param>
        /// <param name="DtEnd"></param>
        /// <returns></returns>
        public DataView GetLadarDataNew(string Quality, int pageSize, int pageNo, out int recordTotal, string DtStart, string DtEnd)
        {
            recordTotal = 0;
            try
            {
                string sql = string.Format(@"select DateTime, Height,Number from [dbo].[TB_SuperStation_jiguangleida]
                                                where DateTime<='{2}' and DateTime>='{1}' and Height<='3'  and DataType='{0}' and Number !='0'  order by Height,DateTime desc"
                        , Quality, DtStart, DtEnd);

                DataTable dt = g_DatabaseHelper.ExecuteDataTable(sql, "AMS_AirAutoMonitorConnection");
                recordTotal = dt.Rows.Count;
                return dt.DefaultView;
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 激光雷达列表数据查询
        /// </summary>
        /// <param name="Quality"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <param name="recordTotal"></param>
        /// <param name="DtStart"></param>
        /// <param name="DtEnd"></param>
        /// <returns></returns>
        public DataView GetLadarBorder(string Quality, int pageSize, int pageNo, out int recordTotal, string DtStart, string DtEnd)
        {
            recordTotal = 0;
            try
            {
                string sql = string.Format(@"select DateTime,Number from [dbo].[TB_SuperStation_jiguangleida]
                                                where DateTime<='{2}' and DateTime>='{1}'   and DataType='{0}'  order by Height,DateTime"
                        , Quality, DtStart, DtEnd);

                DataTable dt = g_DatabaseHelper.ExecuteDataTable(sql, "AMS_AirAutoMonitorConnection");
                recordTotal = dt.Rows.Count;
                return dt.DefaultView;
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 取得所有小时数据（缺失数据全显示）
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
        public DataView GetDataPagerForO3AllTimeOld(string[] portIds, string[] factors
    , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId asc,tstamp desc")
        {
            recordTotal = 0;
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                string factorDataFlagSql = string.Empty;
                string factorAuditFlagSql = string.Empty;
                string factorWhere = string.Empty;
                DataView dv = new DataView();
                bool exist = ((IList<string>)factors).Contains("a05024");
                if (exist)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("select A.*,B.a05024,B.a05024_AuditFlag,B.a05024_DataFlag,B.a05024_Status from (select ");
                    foreach (string factor in factors)
                    {
                        if (factor != "a05024")
                        {
                            string factorFlag = factor + "_Status";
                            string factorDataFlag = factor + "_DataFlag";
                            string factorAuditFlag = factor + "_AuditFlag";
                            factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                            factorFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN Status END) AS [{1}] ", factor, factorFlag);
                            factorDataFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN DataFlag END) AS [{1}] ", factor, factorDataFlag);
                            factorAuditFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN AuditFlag END) AS [{1}] ", factor, factorAuditFlag);
                            //factorWhere += "'" + factor + "','a05024',";
                            factorWhere += "'" + factor + "',";
                        }
                    }
                    factorWhere = factorWhere + "'a05024'";

                    string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                    if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                    {
                        portIdsStr = " AND PointId =" + portIdsStr;
                    }
                    else if (!string.IsNullOrEmpty(portIdsStr))
                    {
                        portIdsStr = "AND PointId IN(" + portIdsStr + ")";
                    }
                    //if (orderBy == "PointId asc,Tstamp desc")
                    //{
                    //    orderBy = "desc";
                    //}
                    //else
                    //{
                    //    orderBy = "asc";
                    //}
                    orderBy = string.IsNullOrEmpty(orderBy) ? "PointId,Tstamp" : orderBy;
                    string fieldName = "PointId ,Tstamp " + factorSql + factorFlagSql + factorDataFlagSql + factorAuditFlagSql;
                    string groupBy = "PointId,Tstamp";
                    string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}'  AND PollutantCode in ({2})", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + portIdsStr;
                    //return g_GridViewPager.GetPivotDataPager(tableName, fieldName, groupBy, orderBy, where, pageSize, pageNo, connection, out recordTotal);
                    string sqlForO3 = string.Format(@"select PointId,Tstamp
                                                    ,MAX(CASE(PollutantCode) WHEN 'a05024' THEN PollutantValue END ) AS [a05024] 
                                                    ,MAX(CASE(PollutantCode) WHEN 'a05024' THEN Status END) AS [a05024_Status] 
                                                    ,MAX(CASE(PollutantCode) WHEN 'a05024' THEN DataFlag END) AS [a05024_DataFlag] 
                                                    ,MAX(CASE(PollutantCode) WHEN 'a05024' THEN AuditFlag END) AS [a05024_AuditFlag]  
                                                    from {0}  WHERE Tstamp>='{1}' AND Tstamp<='{2}' {3} group by PointId,Tstamp) B ", tableName, dtStart.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr);
                    sb.Append(fieldName + " from " + tableName + " where " + where + " group by " + groupBy + ") A left join (" + sqlForO3 + "on A.Tstamp = DATEADD(hour,1, B.Tstamp) and A.PointId=B.PointId  ");

                    //补充时间段
                    string sql = string.Empty;
                    string allHoursSql = string.Empty;
                    //由于实时数据通常晚1小时到数据库（离子色谱仪晚两个小时），补数据时间需要剔除当前小时及之后的小时
                    DateTime AllHourdtEndNew = dtEnd.Hour >= DateTime.Now.Hour ? dtEnd.AddHours(-1) : dtEnd;

                    foreach (string pointId in portIds)
                    {
                        allHoursSql += string.Format(@"select * from [dbo].[F_AllHoursByPoint]('{0}','{1}',{2}) union ", dtStart, AllHourdtEndNew, pointId);
                    }
                    allHoursSql = allHoursSql.Remove(allHoursSql.LastIndexOf("union"));//删除最后一个union

                    sql += string.Format(@"select * from({0}) time left join ({1}) data
                        on convert(varchar(13),time.tstamp,120)=convert(varchar(13),data.tstamp,120)
                        and time.pointId=data.pointId
                        order by time.PointId asc,{2}", allHoursSql, sb.ToString(), orderBy);
                    //dv = g_GridViewPager.GridViewPagerCplexSql(sql, pageSize, pageNo, orderBy, connection, out recordTotal);

                    dv = g_DatabaseHelper.ExecuteDataView(sql, connection);
                    recordTotal = dv.Count;
                    return dv;
                }
                else
                {
                    foreach (string factor in factors)
                    {
                        string factorFlag = factor + "_Status";
                        string factorDataFlag = factor + "_DataFlag";
                        string factorAuditFlag = factor + "_AuditFlag";
                        factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                        factorFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN Status END) AS [{1}] ", factor, factorFlag);
                        factorDataFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN DataFlag END) AS [{1}] ", factor, factorDataFlag);
                        factorAuditFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN AuditFlag END) AS [{1}] ", factor, factorAuditFlag);
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
                    string fieldName = "PointId p,Tstamp t" + factorSql + factorFlagSql + factorDataFlagSql + factorAuditFlagSql;
                    string groupBy = "PointId,Tstamp";
                    string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}'  AND PollutantCode in ({2})", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + portIdsStr;


                    string dataSql = string.Format(@"select {0} from {1} where {2} group by {3} ", fieldName, tableName, where, groupBy);
                    //补充时间段
                    string sql = string.Empty;
                    string allHoursSql = string.Empty;
                    //由于实时数据通常晚1小时到数据库（离子色谱仪晚两个小时），补数据时间需要剔除当前小时及之后的小时
                    DateTime AllHourdtEndNew = dtEnd.Hour >= DateTime.Now.Hour ? dtEnd.AddHours(-1) : dtEnd;

                    foreach (string pointId in portIds)
                    {
                        allHoursSql += string.Format(@"select * from [dbo].[F_AllHoursByPoint]('{0}','{1}',{2}) union ", dtStart, AllHourdtEndNew, pointId);
                    }
                    allHoursSql = allHoursSql.Remove(allHoursSql.LastIndexOf("union"));//删除最后一个union

                    sql += string.Format(@"select * from({0}) time left join ({1}) data on convert(varchar(13),time.tstamp,120)=convert(varchar(13),data.t,120) and time.pointId=data.p order by  time.pointId,{2}", allHoursSql, dataSql, orderBy);
                    //dv = g_GridViewPager.GridViewPagerCplexSql(sql, pageSize, pageNo, orderBy, connection, out recordTotal);

                    dv = g_DatabaseHelper.ExecuteDataView(sql, connection);

                    //dv = g_GridViewPager.GetPivotDataPager(tableName, fieldName, groupBy, orderBy, where, pageSize, pageNo, connection, out recordTotal);
                    return dv;

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataView GetDataPagerForO3AllTime(string[] portIds, string[] factors
, DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId asc,tstamp desc")
        {
            recordTotal = 0;
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                string factorDataFlagSql = string.Empty;
                string factorAuditFlagSql = string.Empty;
                string factorWhere = string.Empty;
                string factorFieldSql = string.Empty;
                DataView dv = new DataView();
                bool exist = ((IList<string>)factors).Contains("a05024");
                if (exist)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("select A.*,B.a05024,B.a05024_AuditFlag,B.a05024_DataFlag,B.a05024_Status INTO #tempTable  from (select ");
                    foreach (string factor in factors)
                    {
                        string factorFlag = string.Empty;
                        string factorDataFlag = string.Empty;
                        string factorAuditFlag = string.Empty;
                        if (factor != "a05024")
                        {
                            factorFlag = factor + "_Status";
                            factorDataFlag = factor + "_DataFlag";
                            factorAuditFlag = factor + "_AuditFlag";
                            factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                            factorFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN Status END) AS [{1}] ", factor, factorFlag);
                            factorDataFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN DataFlag END) AS [{1}] ", factor, factorDataFlag);
                            factorAuditFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN AuditFlag END) AS [{1}] ", factor, factorAuditFlag);
                            factorWhere += "'" + factor + "',";

                        }
                        factorFieldSql += "," + factor;
                        factorFieldSql += "," + factor + "_Status";
                        factorFieldSql += "," + factor + "_DataFlag";
                        factorFieldSql += "," + factor + "_AuditFlag";
                    }
                    factorWhere = factorWhere + "'a05024'";

                    string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                    if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                    {
                        portIdsStr = " AND PointId =" + portIdsStr;
                    }
                    else if (!string.IsNullOrEmpty(portIdsStr))
                    {
                        portIdsStr = "AND PointId IN(" + portIdsStr + ")";
                    }
                    orderBy = string.IsNullOrEmpty(orderBy) ? "order by PointId,Tstamp" : "order by " + orderBy;
                    string fieldName = "PointId ,Tstamp " + factorSql + factorFlagSql + factorDataFlagSql + factorAuditFlagSql;
                    string groupBy = "PointId,Tstamp";
                    string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}'  AND PollutantCode in ({2})", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + portIdsStr;
                    //return g_GridViewPager.GetPivotDataPager(tableName, fieldName, groupBy, orderBy, where, pageSize, pageNo, connection, out recordTotal);
                    string sqlForO3 = string.Format(@"select PointId,Tstamp
                                                    ,MAX(CASE(PollutantCode) WHEN 'a05024' THEN PollutantValue END ) AS [a05024] 
                                                    ,MAX(CASE(PollutantCode) WHEN 'a05024' THEN Status END) AS [a05024_Status] 
                                                    ,MAX(CASE(PollutantCode) WHEN 'a05024' THEN DataFlag END) AS [a05024_DataFlag] 
                                                    ,MAX(CASE(PollutantCode) WHEN 'a05024' THEN AuditFlag END) AS [a05024_AuditFlag]  
                                                    from {0}  WHERE Tstamp>='{1}' AND Tstamp<='{2}' {3} group by PointId,Tstamp) B ", tableName, dtStart.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr);
                    sb.Append(fieldName + " from " + tableName + " where " + where + " group by " + groupBy + ") A left join (" + sqlForO3 + "on A.Tstamp = DATEADD(hour,1, B.Tstamp) and A.PointId=B.PointId  ");

                    //补充时间段
                    string sql = string.Empty;
                    string allHoursSql = string.Empty;
                    //由于实时数据通常晚1小时到数据库（离子色谱仪晚两个小时），补数据时间需要剔除当前小时及之后的小时
                    DateTime AllHourdtEndNew = Convert.ToDateTime(dtEnd.ToString("yyyy-MM-dd HH:00")) >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00")) ?
                        DateTime.Now.AddHours(-1) : dtEnd;

                    allHoursSql += string.Format(@"select time.PointId,time.Tstamp{0} from dbo.SY_F_GetAllDataByHour('{1}',',','{2}','{3}') time", factorFieldSql, StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ","), dtStart.ToString("yyyy-MM-dd HH:mm:ss"), AllHourdtEndNew.ToString("yyyy-MM-dd HH:mm:ss"));

                    sql += string.Format(@"{0} left join #tempTable data on convert(varchar(13),time.tstamp,120)=convert(varchar(13),data.tstamp,120) and time.pointId=data.pointId"
                        , allHoursSql);
                    dv = g_GridViewPager.GridViewPagerAllTimeSql(sql, sb.ToString(), pageSize, pageNo, orderBy, connection, out recordTotal);

                    //dv = g_GridViewPager.GridViewPagerCplexSql(sql, pageSize, pageNo, orderBy, connection, out recordTotal);
                    return dv;
                }
                else
                {
                    foreach (string factor in factors)
                    {
                        string factorFlag = factor + "_Status";
                        string factorDataFlag = factor + "_DataFlag";
                        string factorAuditFlag = factor + "_AuditFlag";
                        factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                        factorFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN Status END) AS [{1}] ", factor, factorFlag);
                        factorDataFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN DataFlag END) AS [{1}] ", factor, factorDataFlag);
                        factorAuditFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN AuditFlag END) AS [{1}] ", factor, factorAuditFlag);
                        factorWhere += "'" + factor + "',";
                        factorFieldSql += "," + factor;
                        factorFieldSql += "," + factorFlag;
                        factorFieldSql += "," + factorDataFlag;
                        factorFieldSql += "," + factorAuditFlag;
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

                    orderBy = string.IsNullOrEmpty(orderBy) ? "order by PointId,Tstamp" : "order by " + orderBy;
                    string fieldName = "PointId ,Tstamp " + factorSql + factorFlagSql + factorDataFlagSql + factorAuditFlagSql;
                    string groupBy = "PointId,Tstamp";
                    string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}'  AND PollutantCode in ({2})", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + portIdsStr;


                    string dataSql = string.Format(@"select {0} INTO #tempTable from {1} where {2} group by {3} ", fieldName, tableName, where, groupBy);
                    //补充时间段
                    string sql = string.Empty;
                    string allHoursSql = string.Empty;
                    //由于实时数据通常晚1小时到数据库（离子色谱仪晚两个小时），补数据时间需要剔除当前小时及之后的小时
                    DateTime AllHourdtEndNew = Convert.ToDateTime(dtEnd.ToString("yyyy-MM-dd HH:00")) >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00")) ?
                        DateTime.Now.AddHours(-1) : dtEnd;

                    allHoursSql += string.Format(@"select time.PointId,time.Tstamp{0} from dbo.SY_F_GetAllDataByHour('{1}',',','{2}','{3}') time", factorFieldSql, StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ","), dtStart.ToString("yyyy-MM-dd HH:mm:ss"), AllHourdtEndNew.ToString("yyyy-MM-dd HH:mm:ss"));

                    sql += string.Format(@"{0} left join #tempTable data on convert(varchar(13),time.tstamp,120)=convert(varchar(13),data.tstamp,120) and time.pointId=data.pointId "
                        , allHoursSql);
                    dv = g_GridViewPager.GridViewPagerAllTimeSql(sql, dataSql, pageSize, pageNo, orderBy, connection, out recordTotal);

                    //dv = g_GridViewPager.GridViewPagerCplexSql(sql, pageSize, pageNo, orderBy, connection, out recordTotal);

                    return dv;

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataView GetDataPagerForO3AllTimeNew(string[] portIds, string[] factors
, DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId asc,tstamp desc")
        {
            recordTotal = 0;
            try
            {
                //取得查询行转列字段拼接
                string factorA = "select p.Tstamp";
                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                string factorDataFlagSql = string.Empty;
                string factorAuditFlagSql = string.Empty;
                string factorWhere = string.Empty;
                string factorFieldSql = string.Empty;
                DataView dv = new DataView();
                bool exist = ((IList<string>)factors).Contains("a05024");
                if (exist)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("select A.*,B.a05024,B.a05024_AuditFlag,B.a05024_DataFlag,B.a05024_Status INTO #tempTable from (select ");
                    foreach (string factor in factors)
                    {
                        factorA += string.Format(",AVG({0}) {0}", factor);
                        string factorFlag = string.Empty;
                        string factorDataFlag = string.Empty;
                        string factorAuditFlag = string.Empty;
                        if (factor != "a05024")
                        {
                            
                            factorFlag = factor + "_Status";
                            factorDataFlag = factor + "_DataFlag";
                            factorAuditFlag = factor + "_AuditFlag";
                            factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                            factorFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN Status END) AS [{1}] ", factor, factorFlag);
                            factorDataFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN DataFlag END) AS [{1}] ", factor, factorDataFlag);
                            factorAuditFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN AuditFlag END) AS [{1}] ", factor, factorAuditFlag);
                            factorWhere += "'" + factor + "',";

                        }
                        factorFieldSql += "," + factor;
                        factorFieldSql += "," + factor + "_Status";
                        factorFieldSql += "," + factor + "_DataFlag";
                        factorFieldSql += "," + factor + "_AuditFlag";
                    }
                    factorWhere = factorWhere + "'a05024'";

                    string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                    if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                    {
                        portIdsStr = " AND PointId =" + portIdsStr;
                    }
                    else if (!string.IsNullOrEmpty(portIdsStr))
                    {
                        portIdsStr = "AND PointId IN(" + portIdsStr + ")";
                    }
                    orderBy = string.IsNullOrEmpty(orderBy) ? "order by PointId,Tstamp" : "order by " + orderBy;
                    string fieldName = "PointId ,Tstamp " + factorSql + factorFlagSql + factorDataFlagSql + factorAuditFlagSql;
                    string groupBy = "PointId,Tstamp";
                    string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}'  AND PollutantCode in ({2})", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + portIdsStr;
                    //return g_GridViewPager.GetPivotDataPager(tableName, fieldName, groupBy, orderBy, where, pageSize, pageNo, connection, out recordTotal);
                    string sqlForO3 = string.Format(@"select PointId,Tstamp
                                                    ,MAX(CASE(PollutantCode) WHEN 'a05024' THEN PollutantValue END ) AS [a05024] 
                                                    ,MAX(CASE(PollutantCode) WHEN 'a05024' THEN Status END) AS [a05024_Status] 
                                                    ,MAX(CASE(PollutantCode) WHEN 'a05024' THEN DataFlag END) AS [a05024_DataFlag] 
                                                    ,MAX(CASE(PollutantCode) WHEN 'a05024' THEN AuditFlag END) AS [a05024_AuditFlag]  
                                                    from {0}  WHERE Tstamp>='{1}' AND Tstamp<='{2}' {3} group by PointId,Tstamp) B ", tableName, dtStart.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr);
                    sb.Append(fieldName + " from " + tableName + " where " + where + " group by " + groupBy + ") A left join (" + sqlForO3 + "on A.Tstamp = DATEADD(hour,1, B.Tstamp) and A.PointId=B.PointId  ");
                    string dataSql = sb.ToString();
                    //补充时间段
                    string sql = string.Empty;
                    string allHoursSql = string.Empty;
                    //由于实时数据通常晚1小时到数据库（离子色谱仪晚两个小时），补数据时间需要剔除当前小时及之后的小时
                    DateTime AllHourdtEndNew = Convert.ToDateTime(dtEnd.ToString("yyyy-MM-dd HH:00")) >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00")) ?
                        DateTime.Now.AddHours(-1) : dtEnd;

                    allHoursSql += string.Format(@"select time.PointId,time.Tstamp{0} from dbo.SY_F_GetAllDataByHour('{1}',',','{2}','{3}') time", factorFieldSql, StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ","), dtStart.ToString("yyyy-MM-dd HH:mm:ss"), AllHourdtEndNew.ToString("yyyy-MM-dd HH:mm:ss"));
                    allHoursSql = factorA +" from(" + allHoursSql;
                    sql += string.Format(@"{0} left join #tempTable data on convert(varchar(13),time.tstamp,120)=convert(varchar(13),data.tstamp,120) and time.pointId=data.pointId)p group by Tstamp order by Tstamp"
                        , allHoursSql);
                    //dv = g_DatabaseHelper.ExecuteDataView(sql,connection);
                    dv = g_DatabaseHelper.ExecuteDataView(dataSql + " " + sql, connection);
                    return dv;
                }
                else
                {
                    foreach (string factor in factors)
                    {
                        factorA += string.Format(",AVG({0}) {0}", factor);
                        string factorFlag = factor + "_Status";
                        string factorDataFlag = factor + "_DataFlag";
                        string factorAuditFlag = factor + "_AuditFlag";
                        factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                        factorFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN Status END) AS [{1}] ", factor, factorFlag);
                        factorDataFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN DataFlag END) AS [{1}] ", factor, factorDataFlag);
                        factorAuditFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN AuditFlag END) AS [{1}] ", factor, factorAuditFlag);
                        factorWhere += "'" + factor + "',";
                        factorFieldSql += "," + factor;
                        factorFieldSql += "," + factorFlag;
                        factorFieldSql += "," + factorDataFlag;
                        factorFieldSql += "," + factorAuditFlag;
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

                    orderBy = string.IsNullOrEmpty(orderBy) ? "order by PointId,Tstamp" : "order by " + orderBy;
                    string fieldName = "PointId ,Tstamp " + factorSql + factorFlagSql + factorDataFlagSql + factorAuditFlagSql;
                    string groupBy = "PointId,Tstamp";
                    string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}'  AND PollutantCode in ({2})", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + portIdsStr;


                    //string dataSql = string.Format(@"select {0} from {1} where {2} group by {3} ", fieldName, tableName, where, groupBy);
                    string dataSql = string.Format(@"select {0} INTO #tempTable from {1} where {2} group by {3} ", fieldName, tableName, where, groupBy);
                    //补充时间段
                    string sql = string.Empty;
                    string allHoursSql = string.Empty;
                    //由于实时数据通常晚1小时到数据库（离子色谱仪晚两个小时），补数据时间需要剔除当前小时及之后的小时
                    DateTime AllHourdtEndNew = Convert.ToDateTime(dtEnd.ToString("yyyy-MM-dd HH:00")) >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00")) ?
                        DateTime.Now.AddHours(-1) : dtEnd;

                    allHoursSql += string.Format(@"select time.PointId,time.Tstamp{0} from dbo.SY_F_GetAllDataByHour('{1}',',','{2}','{3}') time", factorFieldSql, StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ","), dtStart.ToString("yyyy-MM-dd HH:mm:ss"), AllHourdtEndNew.ToString("yyyy-MM-dd HH:mm:ss"));
                    allHoursSql = factorA + " from(" + allHoursSql;
                    sql += string.Format(@"{0} left join  #tempTable data on convert(varchar(13),time.tstamp,120)=convert(varchar(13),data.tstamp,120) and time.pointId=data.pointId)p group by Tstamp order by Tstamp "
                        , allHoursSql);
                    dv = g_DatabaseHelper.ExecuteDataView(dataSql+" "+sql, connection);
                    //dv = g_GridViewPager.GridViewPagerAllTimeSql(sql, dataSql, pageSize, pageNo, orderBy, connection, out recordTotal);
                    
                    return dv;

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataView GetDataPagerForO3AllTimeVOCs(string[] portIds, string[] factors
, DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId asc,tstamp desc")
        {
            recordTotal = 0;
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                string factorDataFlagSql = string.Empty;
                string factorAuditFlagSql = string.Empty;
                string factorWhere = string.Empty;
                string factorFieldSql = string.Empty;
                DataView dv = new DataView();
                
                    foreach (string factor in factors)
                    {
                        //string factorFlag = factor + "_Status";
                        //string factorDataFlag = factor + "_DataFlag";
                        //string factorAuditFlag = factor + "_AuditFlag";
                        factorSql += string.Format(",MAX(CASE (PollutantCode) WHEN '{0}' THEN CASE WHEN Datename(hour,Tstamp)=0 THEN Null ELSE PollutantValue END END) AS [{0}] ", factor);
                        //factorFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN Status END) AS [{1}] ", factor, factorFlag);
                        //factorDataFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN DataFlag END) AS [{1}] ", factor, factorDataFlag);
                        //factorAuditFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN AuditFlag END) AS [{1}] ", factor, factorAuditFlag);
                        factorWhere += "'" + factor + "',";
                        factorFieldSql += "," + factor;
                        //factorFieldSql += "," + factorFlag;
                        //factorFieldSql += "," + factorDataFlag;
                        //factorFieldSql += "," + factorAuditFlag;
                    }
                    foreach (string factor in factors)
                    {
                        string factorFlag = factor + "_Status";
                        factorFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN Status END) AS [{1}] ", factor, factorFlag);
                        factorFieldSql += "," + factorFlag;
                        
                    }
                    foreach (string factor in factors)
                    {
                        string factorDataFlag = factor + "_DataFlag";
                        factorDataFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN DataFlag END) AS [{1}] ", factor, factorDataFlag);
                        factorFieldSql += "," + factorDataFlag;
                        
                    }
                    foreach (string factor in factors)
                    {
                        string factorAuditFlag = factor + "_AuditFlag";
                        factorAuditFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN AuditFlag END) AS [{1}] ", factor, factorAuditFlag);
                        factorFieldSql += "," + factorAuditFlag;
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

                    orderBy = string.IsNullOrEmpty(orderBy) ? "order by PointId,Tstamp" : "order by " + orderBy;
                    string fieldName = "PointId ,Tstamp " + factorSql + factorFlagSql + factorDataFlagSql + factorAuditFlagSql;
                    string groupBy = "PointId,Tstamp";
                    string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}'  AND PollutantCode in ({2}) AND Status!='C'", dtStart.ToString("yyyy-MM-dd HH:00:00"), dtEnd.ToString("yyyy-MM-dd HH:00:00"), factorWhere) + portIdsStr;


                    //string dataSql = string.Format(@"select {0} INTO #tempTable from {1} where {2} group by {3} ", fieldName, tableName, where, groupBy);
                    string tableStr = string.Format("(SELECT  row_number() over(partition by PointId,Tstamp,PollutantCode order by id desc ) rn,PointId,Tstamp,PollutantCode,PollutantValue,Status,DataFlag,AuditFlag FROM Air.TB_InfectantBy60 where {0}) ASD", where);

                    string dataSql = string.Format(@"select {0} INTO #tempTable from {1} where {2} group by {3} ", fieldName, tableStr, " ASD.rn=1 ", groupBy);
                    //补充时间段
                    string sql = string.Empty;
                    string allHoursSql = string.Empty;
                    //由于实时数据通常晚1小时到数据库（离子色谱仪晚两个小时），补数据时间需要剔除当前小时及之后的小时
                    DateTime AllHourdtEndNew = Convert.ToDateTime(dtEnd.ToString("yyyy-MM-dd HH:00")) >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00")) ?
                        DateTime.Now.AddHours(-1) : dtEnd;

                    allHoursSql += string.Format(@"select time.PointId,time.Tstamp{0} from dbo.SY_F_GetAllDataByHour('{1}',',','{2}','{3}') time", factorFieldSql, StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ","), dtStart.ToString("yyyy-MM-dd HH:00:00"), AllHourdtEndNew.ToString("yyyy-MM-dd HH:00:00"));

                    sql += string.Format(@"{0} left join  #tempTable data on convert(varchar(13),time.tstamp,120)=convert(varchar(13),data.tstamp,120) and time.pointId=data.pointId {1}"
                        , allHoursSql,orderBy);
                    //dv = g_GridViewPager.GridViewPagerAllTimeSql(sql, dataSql, pageSize, pageNo, orderBy, connection, out recordTotal);
                    dv = g_DatabaseHelper.ExecuteDataView(dataSql + " " + sql, connection);
                    recordTotal = dv.Count;
                    return dv;
                }

            
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 离子色谱仪取得所有小时数据（缺失数据全显示）
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
        public DataView GetDataPagerForO3AllTimeLZSPY(string[] portIds, string[] factors
    , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId asc,tstamp desc")
        {
            recordTotal = 0;
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                string factorDataFlagSql = string.Empty;
                string factorAuditFlagSql = string.Empty;
                string factorWhere = string.Empty;
                string factorFieldSql = string.Empty;

                DataView dv = new DataView();
                foreach (string factor in factors)
                {
                    string factorFlag = factor + "_Status";
                    string factorDataFlag = factor + "_DataFlag";
                    string factorAuditFlag = factor + "_AuditFlag";
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                    factorFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN Status END) AS [{1}] ", factor, factorFlag);
                    factorDataFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN DataFlag END) AS [{1}] ", factor, factorDataFlag);
                    factorAuditFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN AuditFlag END) AS [{1}] ", factor, factorAuditFlag);
                    factorWhere += "'" + factor + "',";
                    factorFieldSql += "," + factor;
                    factorFieldSql += "," + factorFlag;
                    factorFieldSql += "," + factorDataFlag;
                    factorFieldSql += "," + factorAuditFlag;

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

                orderBy = string.IsNullOrEmpty(orderBy) ? "order by PointId,Tstamp" : "order by " + orderBy;
                string fieldName = "PointId,Tstamp" + factorSql + factorFlagSql + factorDataFlagSql + factorAuditFlagSql;
                string groupBy = "PointId,Tstamp";
                string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}'  AND PollutantCode in ({2})", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + portIdsStr;


                string dataSql = string.Format(@"select {0} INTO #tempTable from {1} where {2} group by {3} ", fieldName, tableName, where, groupBy);
                //补充时间段
                string sql = string.Empty;
                string allHoursSql = string.Empty;
                //由于实时数据通常晚1小时到数据库（离子色谱仪晚两个小时），补数据时间需要剔除当前小时及之后的小时
                DateTime AllHourdtEndNew = Convert.ToDateTime(dtEnd.ToString("yyyy-MM-dd HH:00")) >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00")) ?
                    DateTime.Now.AddHours(-2) : dtEnd;

                allHoursSql += string.Format(@"select time.PointId,time.Tstamp{0} from dbo.SY_F_GetAllDataByHour('{1}',',','{2}','{3}') time", factorFieldSql, StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ","), dtStart.ToString("yyyy-MM-dd HH:mm:ss"), AllHourdtEndNew.ToString("yyyy-MM-dd HH:mm:ss"));

                sql += string.Format(@"{0} left join #tempTable data
                        on convert(varchar(13),time.tstamp,120)=convert(varchar(13),data.Tstamp,120)
                        and time.pointId=data.PointId ",  allHoursSql);
                string tableStr = string.Format("(SELECT  row_number() over(partition by PointId,Tstamp,PollutantCode order by id desc ) rn,PointId,Tstamp,PollutantCode,PollutantValue,Status,DataFlag,AuditFlag FROM {1} where {0}) ASD WHERE ASD.rn=1  group by PointId,Tstamp {2}", where, tableName, orderBy);
                string tableSql = string.Format("select {0} INTO #tempTable from {1} ", fieldName, tableStr);
                if (tableName == "Air.TB_InfectantBy60")
                {
                    dv = g_DatabaseHelper.ExecuteDataView(tableSql + " " + sql+" "+orderBy, connection);
                    recordTotal = dv.Count;
                }
                else
                {
                    dv = g_GridViewPager.GridViewPagerAllTimeSql(sql, dataSql, pageSize, pageNo, orderBy, connection, out recordTotal);
                }
                

                //dv = g_DatabaseHelper.ExecuteDataView(sql, connection);
                //dv = g_GridViewPager.GridViewPagerCplexSql(sql, pageSize, pageNo, orderBy, connection, out recordTotal);

                //dv = g_GridViewPager.GetPivotDataPager(tableName, fieldName, groupBy, orderBy, where, pageSize, pageNo, connection, out recordTotal);
                return dv;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据) NT O3做处理
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
        public DataView GetDataPagerForO3(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Tstamp")
        {
            recordTotal = 0;
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                string factorDataFlagSql = string.Empty;
                string factorAuditFlagSql = string.Empty;
                string factorWhere = string.Empty;
                DataView dv = new DataView();
                bool exist = ((IList<string>)factors).Contains("a05024");
                if (exist)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("select A.*,B.a05024,B.a05024_AuditFlag,B.a05024_DataFlag,B.a05024_Status from (select ");
                    foreach (string factor in factors)
                    {
                        if (factor != "a05024")
                        {
                            string factorFlag = factor + "_Status";
                            string factorDataFlag = factor + "_DataFlag";
                            string factorAuditFlag = factor + "_AuditFlag";
                            factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                            factorFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN Status END) AS [{1}] ", factor, factorFlag);
                            factorDataFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN DataFlag END) AS [{1}] ", factor, factorDataFlag);
                            factorAuditFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN AuditFlag END) AS [{1}] ", factor, factorAuditFlag);
                            //factorWhere += "'" + factor + "','a05024',";
                            factorWhere += "'" + factor + "',";
                        }
                    }
                    factorWhere = factorWhere + "'a05024'";

                    string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                    if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                    {
                        portIdsStr = " AND PointId =" + portIdsStr;
                    }
                    else if (!string.IsNullOrEmpty(portIdsStr))
                    {
                        portIdsStr = "AND PointId IN(" + portIdsStr + ")";
                    }
                    if (orderBy == "PointId asc,Tstamp desc")
                    {
                        orderBy = "desc";
                    }
                    else
                    {
                        orderBy = "asc";
                    }
                    orderBy = string.IsNullOrEmpty(orderBy) ? "PointId,Tstamp" : orderBy;
                    string fieldName = "PointId,Tstamp" + factorSql + factorFlagSql + factorDataFlagSql + factorAuditFlagSql;
                    string groupBy = "PointId,Tstamp";
                    string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}'  AND PollutantCode in ({2})", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + portIdsStr;
                    //return g_GridViewPager.GetPivotDataPager(tableName, fieldName, groupBy, orderBy, where, pageSize, pageNo, connection, out recordTotal);
                    string sqlForO3 = string.Format(@"select PointId,Tstamp
                                                    ,MAX(CASE(PollutantCode) WHEN 'a05024' THEN PollutantValue END ) AS [a05024] 
                                                    ,MAX(CASE(PollutantCode) WHEN 'a05024' THEN Status END) AS [a05024_Status] 
                                                    ,MAX(CASE(PollutantCode) WHEN 'a05024' THEN DataFlag END) AS [a05024_DataFlag] 
                                                    ,MAX(CASE(PollutantCode) WHEN 'a05024' THEN AuditFlag END) AS [a05024_AuditFlag]  
                                                    from {0}  WHERE Tstamp>='{1}' AND Tstamp<='{2}' {3} group by PointId,Tstamp) B ", tableName, dtStart.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr);
                    sb.Append(fieldName + " from " + tableName + " where " + where + " group by " + groupBy + ") A left join (" + sqlForO3 + "on A.Tstamp = DATEADD(hour,1, B.Tstamp) and A.PointId=B.PointId order by A.PointId asc,A.Tstamp " + orderBy);
                    dv = g_DatabaseHelper.ExecuteDataView(sb.ToString(), connection);
                    recordTotal = dv.Count;
                    return dv;
                }
                else
                {
                    foreach (string factor in factors)
                    {
                        string factorFlag = factor + "_Status";
                        string factorDataFlag = factor + "_DataFlag";
                        string factorAuditFlag = factor + "_AuditFlag";
                        factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                        factorFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN Status END) AS [{1}] ", factor, factorFlag);
                        factorDataFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN DataFlag END) AS [{1}] ", factor, factorDataFlag);
                        factorAuditFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN AuditFlag END) AS [{1}] ", factor, factorAuditFlag);
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
                    string fieldName = "PointId,Tstamp" + factorSql + factorFlagSql + factorDataFlagSql + factorAuditFlagSql;
                    string groupBy = "PointId,Tstamp";
                    string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}'  AND PollutantCode in ({2})", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + portIdsStr;
                    dv = g_GridViewPager.GetPivotDataPager(tableName, fieldName, groupBy, orderBy, where, pageSize, pageNo, connection, out recordTotal);
                    return dv;

                }

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
        public DataView GetDataPagers(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime")
        {
            recordTotal = 0;
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorMarkSql = string.Empty;
                string factorWhere = string.Empty;

                foreach (string factor in factors)
                {
                    //string factorMark = factor + "_Mark";
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                    //factorMarkSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN Mark END) AS [{1}] ", factor, factorMark);
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

                orderBy = string.IsNullOrEmpty(orderBy) ? "PointId,DateTime" : orderBy;
                string fieldName = "PointId,DateTime" + factorSql + factorMarkSql;
                string groupBy = "PointId,DateTime";
                string where = string.Format(" DateTime>='{0}' AND DateTime<='{1}'  AND PollutantCode in ({2})", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + portIdsStr;
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
        public DataView GetVOCsDataPagers(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime")
        {
            recordTotal = 0;
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorMarkSql = string.Empty;
                string factorWhere = string.Empty;

                foreach (string factor in factors)
                {
                    //string factorMark = factor + "_Mark";
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                    //factorMarkSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN Mark END) AS [{1}] ", factor, factorMark);
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

                orderBy = string.IsNullOrEmpty(orderBy) ? "PointId,DateTime" : orderBy;
                string fieldName = "PointId,DateTime" + factorSql + factorMarkSql;
                string groupBy = "PointId,DateTime";
                string where = string.Format(" DateTime>='{0}' AND DateTime<='{1}'  AND PollutantCode in ({2})", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + portIdsStr;
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
        public DataView GetAvgDataPagers(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime")
        {
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
        public DataView GetDataPagersForAllTime(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime")
        {
            recordTotal = 0;
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorMarkSql = string.Empty;
                string factorWhere = string.Empty;
                string factorFieldSql = string.Empty;

                foreach (string factor in factors)
                {
                    string factorFlag = factor + "_Status";
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                    factorWhere += "'" + factor + "',";
                    factorFieldSql += "," + factor;
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

                orderBy = string.IsNullOrEmpty(orderBy) ? "PointId,DateTime" : orderBy;
                string fieldName = "PointId,DateTime" + factorSql + factorMarkSql;
                string groupBy = "PointId,DateTime";
                string where = string.Format(" DateTime>='{0}' AND DateTime<='{1}'  AND PollutantCode in ({2})", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + portIdsStr;
                string allDaysSql = string.Empty;
                allDaysSql += string.Format(@"select time.PointId,time.DateTime{0} from dbo.SY_F_GetAllDataByDay('{1}',',','{2}','{3}') time", factorFieldSql, StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ","), dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd > DateTime.Now ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : dtEnd.ToString("yyyy-MM-dd HH:mm:ss"));
                string sql = string.Empty;
                sql += string.Format(@"{0} left join ( select {1} from {2} where {3} group by {4} ) data on convert(varchar(10),time.DateTime,120)=convert(varchar(10),data.DateTime,120) and time.pointId=data.pointId"
                    , allDaysSql, fieldName, tableName, where, groupBy);
                DataView dv = g_GridViewPager.GridViewPagerCplexSql(sql, pageSize, pageNo, orderBy, connection, out recordTotal);
                return dv;
                //return g_GridViewPager.GetPivotDataPager(tableName, fieldName, groupBy, orderBy, where, pageSize, pageNo, connection, out recordTotal);
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
        public DataView GetDataPagersForNTO3AllTime(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime")
        {
            recordTotal = 0;
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorMarkSql = string.Empty;
                string factorWhere = string.Empty;
                string factorFieldSql = string.Empty;
                DataView dv;
                if (factors.Contains("a05027"))
                {
                    string factorkey = "D.PointId,D.DateTime,";
                    foreach (string factor in factors)
                    {
                        string factorFlag = factor + "_Status";
                        factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                        factorWhere += "'" + factor + "',";
                        factorFieldSql += "," + factor;
                    }
                    factorWhere = factorWhere.Remove(factorWhere.Length - 1, 1);
                    foreach (string factor in factors)
                    {
                        if (factor == "a05027")
                        {
                            factorkey += "E.Max8HourO3 AS " + factor + ",";
                        }
                        else
                        {
                            factorkey += "D." + factor+",";
                        }
                        
                    }
                    string portIdstr = string.Empty;
                    string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                    if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                    {
                        portIdsStr = " AND PointId =" + portIdsStr;
                        portIdstr = " AND A.PointId =" + portIdsStr;
                    }
                    else if (!string.IsNullOrEmpty(portIdsStr))
                    {
                        portIdsStr = "AND PointId IN(" + portIdsStr + ")";
                        portIdstr = "AND A.PointId IN(" + portIdsStr + ")";
                    }
                    string key = string.Format(@"(select * from 
(select PointId,convert(varchar(10),dateadd(dd,number,'{1}'),120) DateTime
,A.Max8HourO3
from 
master..spt_values B 
left join 
(select * from [Air].[TB_OriDayAQI] ) A 
on 
A.DateTime = convert(varchar(10),dateadd(dd,number,'{1}'),120)  {0} 
where B.type='p'  and B.number <= datediff(dd,'{1}','{2}') ) 
AS M ) E
ON D.DateTime=E.DateTime AND D.PointId = E.PointId", portIdsStr, dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"));
                    orderBy = string.IsNullOrEmpty(orderBy) ? "PointId,DateTime" : orderBy;
                    string fieldName = "PointId,DateTime" + factorSql + factorMarkSql;
                    string groupBy = "PointId,DateTime";
                    string where = string.Format(" DateTime>='{0}' AND DateTime<='{1}'  AND PollutantCode in ({2})", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + portIdsStr;
                    string allDaysSql = string.Empty;
                    allDaysSql += string.Format(@"select time.PointId,time.DateTime{0} from dbo.SY_F_GetAllDataByDay('{1}',',','{2}','{3}') time", factorFieldSql, StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ","), dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd > DateTime.Now ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : dtEnd.ToString("yyyy-MM-dd HH:mm:ss"));
                    string sql = string.Empty;
                    sql += string.Format(@"{0} left join (select {6}  from  ( select {1} from {2} where {3} group by {4} )as D left join {5} )data on convert(varchar(10),time.DateTime,120)=convert(varchar(10),data.DateTime,120) and time.pointId=data.pointId"
                        , allDaysSql, fieldName, tableName, where, groupBy, key, factorkey.TrimEnd(','));
                    dv = g_GridViewPager.GridViewPagerCplexSql(sql, pageSize, pageNo, orderBy, connection, out recordTotal);
                }
                else
                {
                    foreach (string factor in factors)
                    {
                        string factorFlag = factor + "_Status";
                        factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                        factorWhere += "'" + factor + "',";
                        factorFieldSql += "," + factor;
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

                    orderBy = string.IsNullOrEmpty(orderBy) ? "PointId,DateTime" : orderBy;
                    string fieldName = "PointId,DateTime" + factorSql + factorMarkSql;
                    string groupBy = "PointId,DateTime";
                    string where = string.Format(" DateTime>='{0}' AND DateTime<='{1}'  AND PollutantCode in ({2})", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + portIdsStr;
                    string allDaysSql = string.Empty;
                    allDaysSql += string.Format(@"select time.PointId,time.DateTime{0} from dbo.SY_F_GetAllDataByDay('{1}',',','{2}','{3}') time", factorFieldSql, StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ","), dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd > DateTime.Now ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : dtEnd.ToString("yyyy-MM-dd HH:mm:ss"));
                    string sql = string.Empty;
                    sql += string.Format(@"{0} left join ( select {1} from {2} where {3} group by {4} ) data on convert(varchar(10),time.DateTime,120)=convert(varchar(10),data.DateTime,120) and time.pointId=data.pointId"
                        , allDaysSql, fieldName, tableName, where, groupBy);
                    dv = g_GridViewPager.GridViewPagerCplexSql(sql, pageSize, pageNo, orderBy, connection, out recordTotal);
                    
                }
                return dv;
                //return g_GridViewPager.GetPivotDataPager(tableName, fieldName, groupBy, orderBy, where, pageSize, pageNo, connection, out recordTotal);
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
        public DataView GetDataPagerForAllTimeRegion(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy)
        {
          recordTotal = 0;
          try
          {
            string tableUnion = string.Format(@"(select PointId,DateTime,d1.PollutantCode,cast([dbo].[F_Round](d1.PollutantValue,case when d2.DecimalDigit is null then 3 when d2.DecimalDigit='' then 3 else d2.DecimalDigit end) as decimal(18,4)) as PollutantValue from(select PointId,DateTime,PollutantCode,PollutantValue from {0}) as d1 left join  [AMS_BaseData].[dbo].[V_Factor] as d2 on d1.PollutantCode=d2.PollutantCode) as dt ", tableName);
            //取得查询行转列字段拼接
            string factorSql = string.Empty;
            string factorNewSql = string.Empty;
            //string factorMarkSql = string.Empty;
            string factorWhere = string.Empty;
            string factorFieldSql = string.Empty;

            foreach (string factor in factors)
            {
              string factorFlag = factor + "_Status";
              factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
              factorNewSql += string.Format(",Avg({0}) as [{0}] ", factor);
              factorWhere += "'" + factor + "',";
              factorFieldSql += "," + factor;
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
            string fieldName = "PointId,DateTime" + factorSql;
            string newFiledName = " Max(t2.Region) as PointId,DateTime, sortNumber" + factorNewSql;
            string groupBy = "PointId,DateTime";
            string where = string.Format(" DateTime>='{0}' AND DateTime<='{1}'  AND PollutantCode in ({2})", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + portIdsStr;
            string allDaysSql = string.Empty;
            allDaysSql += string.Format(@"select time.PointId,time.DateTime{0} from dbo.SY_F_GetAllDataByDay('{1}',',','{2}','{3}') time", factorFieldSql, StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ","), dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd > DateTime.Now ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : dtEnd.ToString("yyyy-MM-dd HH:mm:ss"));
            string sql = string.Empty;
            string table1 = string.Format(@"({0} left join ( select {1} from {2} where {3} group by {4} ) data on convert(varchar(10),time.DateTime,120)=convert(varchar(10),data.DateTime,120) and time.pointId=data.pointId)"
                , allDaysSql, fieldName, tableUnion, where, groupBy);
            sql = string.Format(@"select {0} from {1} as t1 left join {2} as t2 on t1.PointId=t2.PortId  inner join [AMS_BaseData].[dbo].[SY_View_CodeMainItem] svcRG
                                        on t2.RegionUid = svcRG.ItemGuid
            group by t2.RegionUid,t1.DateTime,svcRG.sortNumber", newFiledName, table1, "[AMS_BaseData].[dbo].[V_Point_UserConfigNew]");
            DataView dv = g_GridViewPager.GridViewPagerCplexSql(sql, pageSize, pageNo, orderBy, connection, out recordTotal);
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
        public DataView GetDataPagersWithMax(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime")
        {
            recordTotal = 0;
            try
            {
                StringBuilder sb = new StringBuilder();
                if (factors.Contains("a05040"))
                {
                    sb.Append(string.Format(",MAX(CASE(PollutantCode) WHEN 'a51039' THEN PollutantValue END ) AS 'a05040' "));
                }
                if (factors.Contains("a05041"))
                {
                    sb.Append(string.Format(",MAX(CASE(PollutantCode) WHEN 'a04003' THEN PollutantValue END ) AS 'a05041' "));
                }
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorWhere = string.Empty;
                string factorFieldSql = string.Empty;
                foreach (string factor in factors)
                {
                    factorFieldSql += "," + factor;
                    if (factor != "a05040" && factor != "a05041")
                    {
                        factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                        factorWhere += "'" + factor + "',";
                    }
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
                orderBy = string.IsNullOrEmpty(orderBy) ? "PointId,DateTime" : orderBy;
                string fieldName = "PointId,DateTime" + factorSql;
                string groupBy = "PointId,DateTime";
                string where = string.Format(" DateTime>='{0}' AND DateTime<='{1}'  AND PollutantCode in ({2})", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + portIdsStr;
                string where2 = string.Format(" Tstamp>='{0}' AND Tstamp<'{1}'  AND PollutantCode in ({2})", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + portIdsStr;
                string sql = string.Format(@"select * from ( select {0} from {1} where {2} group by {3} ) A 
                                                            left join 
                                                            (select CONVERT(nvarchar(10),Tstamp,21) Tstamp {4} from [Air].[TB_InfectantBy60] 
                                                            where {5} group by CONVERT(nvarchar(10),Tstamp,21)) B on CONVERT(nvarchar(10),A.DateTime,21) = B.Tstamp
                                                            ", fieldName, tableName, where, groupBy, sb.ToString(), where2);
                string allDaysSql = string.Empty;
                allDaysSql += string.Format(@"select time.PointId,time.DateTime{0} from dbo.SY_F_GetAllDataByDay('{1}',',','{2}','{3}') time", factorFieldSql, StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ","), dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd > DateTime.Now ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : dtEnd.ToString("yyyy-MM-dd HH:mm:ss"));
                string sqllast = string.Format(@"{0} left join ( {1} ) data on convert(varchar(10),time.DateTime,120)=convert(varchar(10),data.DateTime,120) and time.pointId=data.pointId"
                    ,allDaysSql,sql);
                DataView dv = g_GridViewPager.GridViewPagerCplexSql(sqllast, pageSize, pageNo, orderBy, connection, out recordTotal);
                for (int i = dv.Table.Columns.Count - 1; i > 0; i--)
                {
                    if (dv.Table.Columns[i].ColumnName.Equals("Tstamp"))
                    {
                        dv.Table.Columns.Remove("Tstamp");
                    }
                }
                recordTotal = dv.Count;
                return dv;
                //return g_GridViewPager.GetPivotDataPager(tableName, fieldName, groupBy, orderBy, where, pageSize, pageNo, connection, out recordTotal);
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
        public DataView GetDayAvgData(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "Tstamp")
        {
            recordTotal = 0;
            try
            {
                tableName = "Air.TB_InfectantByDay";
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
                string where = string.Format(" DateTime>='{0}' AND DateTime<='{1}'  AND PollutantCode in ({2})", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + portIdsStr;
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
        public DataView GetOriDataPager(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId asc,Year desc,MonthOfYear desc")
        {
            recordTotal = 0;
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorMarkSql = string.Empty;
                string factorWhere = string.Empty;

                foreach (string factor in factors)
                {
                    //string factorMark = factor + "_Mark";
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                    //factorMarkSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN Mark END) AS [{1}] ", factor, factorMark);
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

                orderBy = string.IsNullOrEmpty(orderBy) ? "PointId asc,Year desc,MonthOfYear desc" : orderBy;
                string fieldName = "PointId,Year,MonthOfYear" + factorSql + factorMarkSql;
                string groupBy = "PointId,Year,MonthOfYear";
                string where = string.Format(" DateTime>='{0}' AND DateTime<='{1}'  AND PollutantCode in ({2})", dtStart.ToString("yyyy-MM-01"), Convert.ToDateTime(dtEnd.AddMonths(1).ToString("yyyy-MM-01 00:00:00")).AddSeconds(-1).ToString("yyyy-MM-dd"), factorWhere) + portIdsStr;
                return g_GridViewPager.GetPivotDataPager(tableName, fieldName, groupBy, orderBy, where, pageSize, pageNo, connection, out recordTotal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
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
        public DataView GetOriDataPagerRegion(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy )
        {
          recordTotal = 0;
          try
          {
            string tableUnion = string.Format(@"(select PointId,Year,MonthOfYear,DateTime,d1.PollutantCode,cast([dbo].[F_Round](d1.PollutantValue,case when d2.DecimalDigit is null then 3 when d2.DecimalDigit='' then 3 else d2.DecimalDigit end) as decimal(18,4)) as PollutantValue from(select PointId,Year,MonthOfYear,DateTime,PollutantCode,PollutantValue from {0}) as d1 left join  [AMS_BaseData].[dbo].[V_Factor] as d2 on d1.PollutantCode=d2.PollutantCode) as dt ", tableName);
            //取得查询行转列字段拼接
            string factorSql = string.Empty;
          
            string factorWhere = string.Empty;
            string factorNewSql = string.Empty;
            foreach (string factor in factors)
            {
              //string factorMark = factor + "_Mark";
              factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
              factorNewSql += string.Format(",Avg({0}) as [{0}] ", factor);
              //factorMarkSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN Mark END) AS [{1}] ", factor, factorMark);
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

            orderBy = string.IsNullOrEmpty(orderBy) ? "sortNumber desc,Year desc,MonthOfYear desc" : orderBy;
            string fieldName = "PointId,Year,MonthOfYear" + factorSql;
            string newFiledName = " Max(t2.Region) as PointId,Year,MonthOfYear,sortNumber" + factorNewSql;
            string groupBy = "PointId,Year,MonthOfYear";
            string where = string.Format(" DateTime>='{0}' AND DateTime<='{1}'  AND PollutantCode in ({2})", dtStart.ToString("yyyy-MM-01"), Convert.ToDateTime(dtEnd.AddMonths(1).ToString("yyyy-MM-01 00:00:00")).AddSeconds(-1).ToString("yyyy-MM-dd"), factorWhere) + portIdsStr;
            string sql = string.Empty;
            string table1 = string.Format(@"( select {0} from {1} where {2} group by {3} )"
                , fieldName, tableUnion, where, groupBy);
            sql = string.Format(@"select {0} from {1} as t1 left join {2} as t2 on t1.PointId=t2.PortId  inner join [AMS_BaseData].[dbo].[SY_View_CodeMainItem] svcRG
                                                    on t2.RegionUid = svcRG.ItemGuid
                        group by t2.RegionUid,Year,MonthOfYear,svcRG.sortNumber", newFiledName, table1, "[AMS_BaseData].[dbo].[V_Point_UserConfigNew]");
            DataView dv = g_GridViewPager.GridViewPagerCplexSql(sql, pageSize, pageNo, orderBy, connection, out recordTotal);
            return dv;
           
          }
          catch (Exception ex)
          {
            throw ex;
          }
        }
//      recordTotal = 0;
//          try
//          {
//            string tableUnion = string.Format(@"(select PointId,Tstamp,d1.PollutantCode,cast([dbo].[F_Round](d1.PollutantValue,case when d2.DecimalDigit is null then 3 when d2.DecimalDigit='' then 3 else d2.DecimalDigit end) as decimal(18,4)) as PollutantValue,Status,DataFlag,AuditFlag from(select PointId,Tstamp,PollutantCode,PollutantValue,Status,DataFlag,AuditFlag from {0}) as d1 left join  [AMS_BaseData].[dbo].[V_Factor] as d2 on d1.PollutantCode=d2.PollutantCode) as dt ", tableName);
//            //取得查询行转列字段拼接
//            string factorSql = string.Empty;
//            string factorNewSql = string.Empty;
//            //string factorMarkSql = string.Empty;
//            string factorWhere = string.Empty;
//            string factorFieldSql = string.Empty;

//            foreach (string factor in factors)
//            {
//              string factorFlag = factor + "_Status";
//              factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
//              factorNewSql += string.Format(",Avg({0}) as [{0}] ", factor);
//              factorWhere += "'" + factor + "',";
//              factorFieldSql += "," + factor;
//            }
//            factorWhere = factorWhere.Remove(factorWhere.Length - 1, 1);

//            string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
//            if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
//            {
//              portIdsStr = " AND PointId =" + portIdsStr;
//            }
//            else if (!string.IsNullOrEmpty(portIdsStr))
//            {
//              portIdsStr = "AND PointId IN(" + portIdsStr + ")";
//            }
//            string fieldName = "PointId,DateTime" + factorSql;
//            string newFiledName = " Max(t2.Region) as PointId,DateTime, sortNumber" + factorNewSql;
//            string groupBy = "PointId,DateTime";
//            string where = string.Format(" DateTime>='{0}' AND DateTime<='{1}'  AND PollutantCode in ({2})", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + portIdsStr;
//            string allDaysSql = string.Empty;
//            allDaysSql += string.Format(@"select time.PointId,time.DateTime{0} from dbo.SY_F_GetAllDataByDay('{1}',',','{2}','{3}') time", factorFieldSql, StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ","), dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd > DateTime.Now ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : dtEnd.ToString("yyyy-MM-dd HH:mm:ss"));
//            string sql = string.Empty;
//            string table1 = string.Format(@"({0} left join ( select {1} from {2} where {3} group by {4} ) data on convert(varchar(10),time.DateTime,120)=convert(varchar(10),data.DateTime,120) and time.pointId=data.pointId)"
//                , allDaysSql, fieldName, tableName, where, groupBy);
//            sql = string.Format(@"select {0} from {1} as t1 left join {2} as t2 on t1.PointId=t2.PortId  inner join [AMS_BaseData].[dbo].[SY_View_CodeMainItem] svcRG
//                                        on t2.RegionUid = svcRG.ItemGuid
//            group by t2.RegionUid,t1.DateTime,svcRG.sortNumber", newFiledName, table1, "[AMS_BaseData].[dbo].[V_Point_UserConfigNew]");
//            DataView dv = g_GridViewPager.GridViewPagerCplexSql(sql, pageSize, pageNo, orderBy, connection, out recordTotal);
//            return dv;
           
//          }
//          catch (Exception ex)
//          {
//            throw ex;
//          }
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
        public DataView GetOriAvgDataPager(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId asc,Year desc,MonthOfYear desc")
        {
            recordTotal = 0;
            try
            {
                //取得查询行转列字段拼接
                string factorASql = string.Empty;
                string factorSql = "select PointId,Year,MonthOfYear,Convert(datetime,Convert(nvarchar(13),datetime,120)+':00:00') datetime";
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

                orderBy = string.IsNullOrEmpty(orderBy) ? "Year asc,MonthOfYear asc" : "Year asc,MonthOfYear asc";
                string fieldName = "PointId,DateTime" + factorSql + factorMarkSql;
                string groupBy = "PointId,Year,MonthOfYear,datetime";
                string where = string.Format(" DateTime>='{0}' AND DateTime<='{1}' ", dtStart.ToString("yyyy-MM-01"), Convert.ToDateTime(dtEnd.AddMonths(1).ToString("yyyy-MM-01 00:00:00")).AddSeconds(-1).ToString("yyyy-MM-dd")) + portIdsStr;
                string sql = string.Format(@"Select Year,MonthOfYear {0} from ({1} from {2} where {3} group by {4})data group by Year,MonthOfYear order by Year asc,MonthOfYear asc", factorASql, factorSql, tableName, where, groupBy);
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
        public DataView GetOriDataAvg(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId asc,Year desc,MonthOfYear desc")
        {
            recordTotal = 0;
            try
            {
                tableName = "Air.TB_InfectantByMonth";
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

                string groupby = "Year,MonthOfYear";
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND PointId =" + portIdsStr;
                    groupby = "PointId,Year,MonthOfYear";
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = "AND PointId IN(" + portIdsStr + ")";
                }

                orderBy = string.IsNullOrEmpty(orderBy) ? "Year desc,MonthOfYear desc" : orderBy;
                string fieldName = "PointId,Year,MonthOfYear" + factorSql;
                string where = string.Format(" DateTime>='{0}' AND DateTime<='{1}'  AND PollutantCode in ({2})", dtStart.ToString("yyyy-MM-01"), Convert.ToDateTime(dtEnd.AddMonths(1).ToString("yyyy-MM-01 00:00:00")).AddSeconds(-1).ToString("yyyy-MM-dd"), factorWhere) + portIdsStr;
                string sql = string.Format(@"select {5}, Year,MonthOfYear
											{3}
											from(
											select {1}
											 from {0}
											where {2}
                                            group by PointId,Year,MonthOfYear
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
        public DataView GetDayAvgData(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, string orderBy = "PointId")
        {
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                string factorMarkSql = string.Empty;
                string factorWhere = string.Empty;

                foreach (string factor in factors)
                {
                    string factorFlag = factor + "_Status";
                    string factorMark = factor + "_Mark";
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                    factorFlagSql += string.Format(",null AS {0} ", factorFlag);
                    factorMarkSql += string.Format(",null AS {0} ", factorMark);
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

                orderBy = string.IsNullOrEmpty(orderBy) ? "PointId" : orderBy;
                string fieldName = "PointId,'" + dtStart.ToString("yyyy-MM-dd") + "' Tstamp" + factorSql + factorFlagSql + factorMarkSql + ",null as blankspaceColumn";
                string groupBy = "PointId";
                string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}'  AND PollutantCode in ({2})", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + portIdsStr;
                string sql = string.Format(@"SELECT {1}
                        FROM {0}
                        WHERE {2}
                        GROUP BY {3}
                    ", tableName, fieldName, where, groupBy);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 取得最新蓝藻虚拟分页查询数据和总行数(行转列数据)
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
        public DataView GetLZDataPager(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal)
        {
            recordTotal = 0;
            try
            {
                //取得查询行转列字段拼接


                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                string factorMarkSql = string.Empty;
                string factorWhere = string.Empty;

                foreach (string factor in factors)
                {
                    string factorFlag = factor + "_Status";
                    string factorMark = factor + "_Mark";
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                    factorFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN Status END) AS [{1}] ", factor, factorFlag);
                    factorMarkSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN Mark END) AS [{1}] ", factor, factorMark);
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

                string fieldName = "a.Name,a.PointId,a.Tstamp,a.BluedOrderNUm" + factorSql + factorFlagSql + factorMarkSql + ",null as blankspaceColumn";
                string where1 = string.Format(@" Tstamp = (select max(Tstamp) from dbo.V_InfectantBy60 where [PointId] = a.[PointId]
                                                           AND Tstamp>='{0}' AND Tstamp<='{1}')", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"));
                string where2 = string.Format(" AND Tstamp>='{0}' AND Tstamp<='{1}'  AND PollutantCode in ({2})", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + portIdsStr;
                string sql = "select ";
                sql += fieldName;
                sql += " from ";
                sql += tableName;
                sql += " a ";
                sql += " where ";
                sql += where1;
                sql += where2;
                sql += " group  by  PointId,Tstamp,BluedOrderNUm,Name ";
                sql += " order  by a.BluedOrderNUm ";
                return g_DatabaseHelper.ExecuteDataView(sql, connection);

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
        public DataView GetExportData(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, string orderBy = "PointId,Tstamp")
        {
            tableName = "dbo.V_Water_InfectantBy60";
            int recordTotal = 0;
            int pageSize = 99999999;
            int pageNo = 0;
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                string factorWhere = string.Empty;
                foreach (string factor in factors)
                {
                    string factorFlag = factor + "_Status";
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                    factorFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN Status END) AS [{1}] ", factor, factorFlag);
                    factorWhere += "'" + factor + "',";
                }
                factorWhere = factorWhere.Remove(factorWhere.Length - 1, 1);
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
                string fieldName = "rowNum as '序号',PointId,portName,CONVERT(NVARCHAR(19),Tstamp,120) as '日期'" + factorSql + factorFlagSql;
                string groupBy = "PointId,portName,Tstamp";
                string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}' AND PollutantCode in ({2})", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + portIdsStr;
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
        public DataView GetDayExportData(string[] portIds, IList<IPollutant> factors
            , DateTime dtStart, DateTime dtEnd, string orderBy = "PointId")
        {
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                string factorWhere = string.Empty;
                foreach (IPollutant factor in factors)
                {
                    string factorFlag = factor.PollutantCode + "_Status";
                    Int32 decimalNum = string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum);
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN CAST(dbo.F_Round(PollutantValue,{1}) AS DECIMAL(10,{1})) END ) AS [{2}] ", factor.PollutantCode, decimalNum, string.Format("{0}({1})", factor.PollutantName, factor.PollutantMeasureUnit));
                    factorWhere += "'" + factor.PollutantCode + "',";
                }
                factorWhere = factorWhere.Remove(factorWhere.Length - 1, 1);
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
                string fieldName = "PointId,'" + dtStart.ToString("yyyy-MM-dd") + "' as '日期'" + factorSql + factorFlagSql;
                string groupBy = "PointId";
                string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}' AND PollutantCode in ({2})", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + portIdsStr;
                string sql = string.Format(@"SELECT {1}
                        FROM {0}
                        WHERE {2}
                        GROUP BY {3}
                    ", tableName, fieldName, where, groupBy);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
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
            int recordTotal = 0;
            int pageSize = 99999999;
            int pageNo = 0;
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                string factorWhere = string.Empty;
                foreach (IPollutant factor in factors)
                {
                    string factorFlag = factor.PollutantCode + "_Status";
                    Int32 decimalNum = string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum);
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN CAST(dbo.F_Round(PollutantValue,{1}) AS DECIMAL(10,{1})) END ) AS [{2}] ", factor.PollutantCode, decimalNum, string.Format("{0}({1})", factor.PollutantName, factor.PollutantMeasureUnit));
                    factorWhere += "'" + factor.PollutantCode + "',";
                }
                factorWhere = factorWhere.Remove(factorWhere.Length - 1, 1);
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
                string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}' AND PollutantCode in ({2})", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + portIdsStr;
                return g_GridViewPager.GetPivotDataPager(tableName, fieldName, groupBy, orderBy, where, pageSize, pageNo, connection, out recordTotal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// VOC总和数据（非零点）
        /// </summary>
        /// <param name="TypeNames"></param>
        /// <param name="ds"></param>
        /// <param name="de"></param>
        /// <returns></returns>
        public DataTable GetVOCDataAll(string[] TypeNames, DateTime ds, DateTime de)
        {
            try
            {
                string selectsql = "select time.PointId,time.Tstamp";
                string factorSql = string.Empty;
                foreach (string TypeName in TypeNames)
                {
                    selectsql += string.Format(@",[{0}] ", TypeName);//sql防止列名中含有中文括号需要[]表示列名
                    factorSql += string.Format(",MAX(CASE TypeName WHEN '{0}' THEN CASE WHEN Datename(hour,Tstamp)=0 THEN Null ELSE  PollutantValue END END) AS [{0}] ", TypeName);
                }
                DateTime AllHourdtEndNew = Convert.ToDateTime(de.ToString("yyyy-MM-dd HH:00")) >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00")) ?
                        DateTime.Now.AddHours(-1) : de;
                selectsql += string.Format(@"from dbo.SY_F_GetAllDataByHour('204',',','{0}','{1}') time left join (", ds.ToString("yyyy-MM-dd HH:00:00"), AllHourdtEndNew.ToString("yyyy-MM-dd HH:00:00"));
                string sql = string.Format(@"{0} SELECT '204' AS PointId ,Tstamp {1} FROM [Air].[TB_VOCStatistics] WHERE Tstamp>='{2}' AND Tstamp<='{3}'    GROUP BY Tstamp) data 
ON convert(varchar(13),time.tstamp,120)=convert(varchar(13),data.tstamp,120) and time.pointId=data.pointId order by Tstamp", selectsql, factorSql, ds.ToString("yyyy-MM-dd HH:00:00"), de.ToString("yyyy-MM-dd HH:00:00"));
                return g_DatabaseHelper.ExecuteDataTable(sql, "AMS_AirAutoMonitorConnection");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 区分单位的VOC总和数据（非零点）
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="TypeNames"></param>
        /// <param name="ds"></param>
        /// <param name="de"></param>
        /// <returns></returns>
        public DataTable GetVOCDataAll(string unit,string[] TypeNames, DateTime ds, DateTime de)
        {
            try
            {
                string selectsql = "select time.PointId,time.Tstamp";
                string factorSql = string.Empty;
                foreach (string TypeName in TypeNames)
                {
                    selectsql += string.Format(@",[{0}] ", TypeName);//sql防止列名中含有中文括号需要[]表示列名
                    factorSql += string.Format(",MAX(CASE TypeName WHEN '{0}' THEN CASE WHEN Datename(hour,Tstamp)=0 THEN Null ELSE  PollutantValue END END) AS [{0}] ", TypeName);
                }
                DateTime AllHourdtEndNew = Convert.ToDateTime(de.ToString("yyyy-MM-dd HH:00")) >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00")) ?
                        DateTime.Now.AddHours(-1) : de;
                selectsql += string.Format(@"from dbo.SY_F_GetAllDataByHour('204',',','{0}','{1}') time left join (", ds.ToString("yyyy-MM-dd HH:00:00"), AllHourdtEndNew.ToString("yyyy-MM-dd HH:00:00"));
                string sql = string.Format(@"{0} SELECT '204' AS PointId ,Tstamp {1} FROM [Air].[TB_VOCStatistics] WHERE Tstamp>='{2}' AND Tstamp<='{3}'  and Description='{4}'  GROUP BY Tstamp) data 
ON convert(varchar(13),time.tstamp,120)=convert(varchar(13),data.tstamp,120) and time.pointId=data.pointId order by Tstamp", selectsql, factorSql, ds.ToString("yyyy-MM-dd HH:00:00"), de.ToString("yyyy-MM-dd HH:00:00"),unit);
                return g_DatabaseHelper.ExecuteDataTable(sql, "AMS_AirAutoMonitorConnection");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 区分单位的VOC总和数据（零点）
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="TypeNames"></param>
        /// <param name="ds"></param>
        /// <param name="de"></param>
        /// <returns></returns>
        public DataTable GetVOCWDataAll(string unit, string[] TypeNames, DateTime ds, DateTime de)
        {
            try
            {
                string selectsql = "select time.PointId,time.Tstamp";
                string factorSql = string.Empty;
                foreach (string TypeName in TypeNames)
                {
                    selectsql += string.Format(@",[{0}] ", TypeName);//sql防止列名中含有中文括号需要[]表示列名
                    factorSql += string.Format(",MAX(CASE TypeName WHEN '{0}' THEN PollutantValue  ELSE  Null END) AS [{0}] ", TypeName);
                }
                string sql = string.Format(@"SELECT '204' AS PointId ,Tstamp {0} FROM [Air].[TB_VOCStatistics] WHERE Tstamp>='{1}' AND Tstamp<='{2}' AND Datename(hour,Tstamp)=0  AND Description='{3}'  GROUP BY Tstamp order by Tstamp", factorSql, ds.ToString("yyyy-MM-dd HH:00:00"), de.ToString("yyyy-MM-dd HH:00:00"), unit);
                return g_DatabaseHelper.ExecuteDataTable(sql, "AMS_AirAutoMonitorConnection");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 区分单位的VOC总和数据（非零点的数据--可排序）
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="TypeNames"></param>
        /// <param name="ds"></param>
        /// <param name="de"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public DataTable GetVOCDataAll(string unit, string[] TypeNames, DateTime ds, DateTime de, string orderby)
        {
            try
            {
                string selectsql = "select time.PointId,time.Tstamp";
                string factorSql = string.Empty;
                foreach (string TypeName in TypeNames)
                {
                    selectsql += string.Format(@",[{0}] ", TypeName);//sql防止列名中含有中文括号需要[]表示列名
                    factorSql += string.Format(",MAX(CASE TypeName WHEN '{0}' THEN CASE WHEN Datename(hour,Tstamp)=0 THEN Null ELSE  PollutantValue END END) AS [{0}] ", TypeName);
                }
                DateTime AllHourdtEndNew = Convert.ToDateTime(de.ToString("yyyy-MM-dd HH:00")) >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00")) ?
                        DateTime.Now.AddHours(-1) : de;
                selectsql += string.Format(@"from dbo.SY_F_GetAllDataByHour('204',',','{0}','{1}') time left join (", ds.ToString("yyyy-MM-dd HH:00:00"), AllHourdtEndNew.ToString("yyyy-MM-dd HH:00:00"));
                string sql = string.Format(@"{0} SELECT '204' AS PointId ,Tstamp {1} FROM [Air].[TB_VOCStatistics] WHERE Tstamp>='{2}' AND Tstamp<='{3}' and Description='{5}'  GROUP BY Tstamp) data 
ON convert(varchar(13),time.tstamp,120)=convert(varchar(13),data.tstamp,120) and time.pointId=data.pointId order by {4}", selectsql, factorSql, ds.ToString("yyyy-MM-dd HH:00:00"), de.ToString("yyyy-MM-dd HH:00:00"), orderby, unit);
                return g_DatabaseHelper.ExecuteDataTable(sql, "AMS_AirAutoMonitorConnection");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// VOC总和数据（非零点的数据--可排序）
        /// </summary>
        /// <param name="TypeNames"></param>
        /// <param name="ds"></param>
        /// <param name="de"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public DataTable GetVOCDataAll(string[] TypeNames, DateTime ds, DateTime de,string orderby)
        {
            try
            {
                string selectsql = "select time.PointId,time.Tstamp";
                string factorSql = string.Empty;
                foreach (string TypeName in TypeNames)
                {
                    selectsql += string.Format(@",[{0}] ", TypeName);//sql防止列名中含有中文括号需要[]表示列名
                    factorSql += string.Format(",MAX(CASE TypeName WHEN '{0}' THEN CASE WHEN Datename(hour,Tstamp)=0 THEN Null ELSE  PollutantValue END END) AS [{0}] ", TypeName);
                }
                DateTime AllHourdtEndNew = Convert.ToDateTime(de.ToString("yyyy-MM-dd HH:00")) >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00")) ?
                        DateTime.Now.AddHours(-1) : de;
                selectsql += string.Format(@"from dbo.SY_F_GetAllDataByHour('204',',','{0}','{1}') time left join (", ds.ToString("yyyy-MM-dd HH:00:00"), AllHourdtEndNew.ToString("yyyy-MM-dd HH:00:00"));
                string sql = string.Format(@"{0} SELECT '204' AS PointId ,Tstamp {1} FROM [Air].[TB_VOCStatistics] WHERE Tstamp>='{2}' AND Tstamp<='{3}'  GROUP BY Tstamp) data 
ON convert(varchar(13),time.tstamp,120)=convert(varchar(13),data.tstamp,120) and time.pointId=data.pointId order by {4}", selectsql, factorSql, ds.ToString("yyyy-MM-dd HH:00:00"), de.ToString("yyyy-MM-dd HH:00:00"), orderby);
                return g_DatabaseHelper.ExecuteDataTable(sql, "AMS_AirAutoMonitorConnection");
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
                        SELECT PointId
                            ,PollutantCode='{1}'
	                        ,AVG(PollutantValue) AS Value_Avg
	                        ,MAX(PollutantValue) AS Value_Max
	                        ,MIN(PollutantValue) AS Value_Min
                        FROM {0}
                        WHERE {2}
                            AND PollutantCode='{1}'
                        GROUP BY PointId
                    ", tableName, factors[iRow], where);
            }
            return g_DatabaseHelper.ExecuteDataView(sql, connection);
        }
        /// <summary>
        /// 取得蓝藻统计数据（最大值、最小值、平均值）
        /// </summary>
        /// <param name="autoMonitorType">数据类型</param>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>

        public DataView GetStatisticalLZData(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd)
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
                        SELECT 
                            PollutantCode='{1}'
	                        ,AVG(PollutantValue) AS Value_Avg
	                        ,MAX(PollutantValue) AS Value_Max
	                        ,MIN(PollutantValue) AS Value_Min
                        FROM {0} a
                        WHERE Tstamp = (
                    select max(Tstamp) from dbo.V_InfectantBy60 where [PointId] = a.[PointId]
                    AND Tstamp>='{2}' AND Tstamp<='{3}'  
                                        )  
                            AND {4}
                            AND PollutantCode='{1}'
                        
                        
                    ", tableName, factors[iRow], dateStart.ToString("yyyy-MM-dd HH:mm:ss"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss"), where);
            }
            return g_DatabaseHelper.ExecuteDataView(sql, connection);
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
            //查询条件拼接
            string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
            if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
            {
                portIdsStr = " AND PointId =" + portIdsStr;
            }
            else if (!string.IsNullOrEmpty(portIdsStr))
            {
                portIdsStr = " PointId IN(" + portIdsStr + ")";
            }
            string whereString = string.Format("Tstamp>='{0}' AND Tstamp<='{1}' AND {2} ", dateStart.ToString("yyyy-MM-dd HH:mm:ss"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr);

            return g_GridViewPager.GetAllDataCount(tableName, "PointId,Tstamp", whereString, connection);
        }

        /// <summary>
        /// 取得指定站点指定时间内最新一条数据
        /// </summary>
        /// <param name="portIds">测点</param>
        /// <param name="factors">因子</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <returns></returns>
        public DataView GetLastDataByPort(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                string factorMarkSql = string.Empty;
                foreach (string factor in factors)
                {
                    string factorFlag = factor + "_Status";
                    string factorMark = factor + "_Mark";
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                    factorFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN Status END) AS [{1}] ", factor, factorFlag);
                    factorMarkSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN Mark END) AS [{1}] ", factor, factorMark);
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
                //拼接where条件
                string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}' ", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr;

                string sql = string.Format(@"
                    WITH lastData AS
                    (
	                    SELECT PointId
		                    ,MAX(Tstamp) AS Tstamp
	                    FROM {0}
	                    WHERE {1}
	                    GROUP BY PointId
                    )
                    SELECT lastData.PointId
	                    ,lastData.Tstamp
	                    {2}
                    FROM lastData
                    INNER JOIN {0} AS data
	                    ON lastData.PointId = data.PointId AND lastData.Tstamp = data.Tstamp 
                    GROUP BY lastData.PointId,lastData.Tstamp
                ", tableName, where, factorSql + factorFlagSql + factorMarkSql);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 取得指定因子指定时间范围内最新一条数据
        /// </summary>
        /// <param name="portIds">测点</param>
        /// <param name="factors">因子</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <returns></returns>
        public DataView GetLastDataByPollutant(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                if (factors != null && factors.Length > 0)
                    factorSql = " AND PollutantCode IN ('" + StringExtensions.GetArrayStrNoEmpty(factors.ToList(), "','") + "')";
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND PointId =" + portIdsStr;
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = "AND PointId IN(" + portIdsStr + ")";
                }
                //拼接where条件
                string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}' ", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr + factorSql;

                string sql = string.Format(@"
                    WITH lastData AS
                    (
	                    SELECT PointId
		                    ,PollutantCode
		                    ,MAX(Tstamp) AS Tstamp
	                    FROM {0}
	                    WHERE {1}
	                    GROUP BY PointId,PollutantCode
                    )
                    SELECT lastData.PointId
	                    ,lastData.Tstamp
	                    ,lastData.PollutantCode
	                    ,data.PollutantValue
	                    ,data.Status
	                    ,data.Mark
                    FROM lastData
                    INNER JOIN {0} AS data
	                    ON lastData.PointId = data.PointId AND lastData.Tstamp = data.Tstamp AND lastData.PollutantCode = data.PollutantCode
                ", tableName, where);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 返回指定站点、因子的捕获条数
        /// </summary>
        /// <param name="portIds">测点</param>
        /// <param name="factors">因子</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <returns></returns>
        public DataView GetPollutantValueCount(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                if (factors != null && factors.Length > 0)
                    factorSql = " AND PollutantCode IN ('" + StringExtensions.GetArrayStrNoEmpty(factors.ToList(), "','") + "')";
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND PointId =" + portIdsStr;
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = "AND PointId IN(" + portIdsStr + ")";
                }
                //拼接where条件
                string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}' ", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr + factorSql;
                string sql = string.Format(@"
                    SELECT DISTINCT PointId
	                    ,PollutantCode
	                    ,COUNT(1) AS Count
                    FROM
                    (
	                    SELECT PointId,PollutantCode,Tstamp
	                    FROM {0}
	                    WHERE {1}
	                    GROUP BY PointId,PollutantCode,Tstamp
                    ) AS data
                    GROUP BY PointId,PollutantCode
                ", tableName, where);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion

        /// <summary>
        /// 从小时数据表中, 分组统计取得各测点的日均值
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtBegion"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataView GetDayAvgData(string[] portIds, IList<IPollutant> factors, DateTime dtStart, DateTime dtEnd)
        {
            try
            {
                //取得查询行转列字段拼接
                string factorMaxSql = string.Empty;// 结果类似于=,MAX(CASE(PollutantCode) WHEN 'a34004' THEN PollutantValue END ) AS [a34004] ,MAX(CASE(PollutantCode) WHEN 'a34002' THEN PollutantValue END ) AS [a34002]
                string factorAvgSql = string.Empty;// 结果类似于=,AVG(a34004) as a3404,AVG(a34002) as a3402

                string factorMaxFlagSql = string.Empty;// 结果类似于=
                string factorMaxMarkSql = string.Empty;// 结果类似于=

                string factorAvgFlagSql = string.Empty;// 结果类似于= MAX(a34004_Status) as a34004_Status,MAX(a34002_Status) as a34002_Status,
                string factorAvgMarkSql = string.Empty;// 结果类似于= MAX(a34004_Mark) as a34004_Mark,MAX(a34002_Mark) as a34002_Mark
                string factorWhere = string.Empty; //结果类似于='a34004','a34002'

                foreach (IPollutant factor in factors)
                {
                    string factorFlag = factor.PollutantCode + "_Status";
                    string factorMark = factor.PollutantCode + "_Mark";
                    factorMaxSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor.PollutantCode);
                    factorMaxFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN Status END) AS [{1}] ", factor.PollutantCode, factorFlag);
                    factorMaxMarkSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN Mark END) AS [{1}] ", factor.PollutantCode, factorMark);

                    factorAvgSql += string.Format(",AVG({0}) as {0}", factor.PollutantCode);
                    factorAvgFlagSql += string.Format(",MAX({0}) as {0}", factorFlag);
                    factorAvgMarkSql += string.Format(",MAX({0}) as {0}", factorMark);

                    factorWhere += "'" + factor.PollutantCode + "',";
                }
                factorWhere = factorWhere.Remove(factorWhere.Length - 1, 1);
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
                string fieldMaxName = "PointId, Tstamp" + factorMaxSql + factorMaxFlagSql + factorMaxMarkSql;
                string fieldAvgName = "PointId, CONVERT(Datetime,CONVERT(varchar(10),Tstamp,120)) as Tstamp" + factorAvgSql + factorAvgFlagSql + factorAvgMarkSql;
                string groupMaxBy = "PointId,Tstamp";
                string groupAvgBy = "PointId, CONVERT(Datetime,CONVERT(varchar(10),Tstamp,120))";
                string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}' AND PollutantCode in ({2})", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + portIdsStr;
                string strSql = string.Format(@" select {0} 
                                              from ( 
                                                    select {1} 
                                                    from {5} 
	                                                WHERE {2} 
                                                    group by {3} 
                                                    ) t 
                                               group by {4}   order by PointId,Tstamp", fieldAvgName, fieldMaxName, where, groupMaxBy, groupAvgBy, tableName);
                return g_DatabaseHelper.ExecuteDataView(strSql, connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataView GetDataList(string portIds, DateTime dtTime)
        {

            try
            {
                string portIdsStr = " AND PointId =" + portIds;

                string where = string.Format(" Tstamp='{0}'", dtTime.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr;

                string sql = string.Format(@"SELECT [ID]
						  ,[PointId]
						  ,[PointAQM]
						  ,[FileName]
						  ,[DateTime]
						  ,[Tstamp]
						  ,[IsSuccess]
						  ,[Note]
						  ,[UpdateTme]
						  ,[CreaterUser]
						  ,[Number]
						  ,[RecordNumber]
						  ,[XMLDBF]
					  FROM [AMS_MonitorBusiness].[dbo].[GenerateXMLDBF]
                        WHERE {0} order by Tstamp desc
                    ", where);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }

        public DataView GetDataLists(string[] portIds, DateTime dtBegin, DateTime dtEnd)
        {

            try
            {
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND D.PointId =" + portIdsStr;
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = "AND D.PointId IN(" + portIdsStr + ")";
                }

                string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}'", dtBegin.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr;

                string sql = string.Format(@"SELECT [ID]
						  ,D.[PointId]
						  ,MonitoringPointName
						  ,[PointAQM]
						  ,[FileName]
						  ,[DateTime]
						  ,[Tstamp]
						  ,[IsSuccess]
						  ,[Note]
						  ,[UpdateTme]
						  ,[CreaterUser]
						  ,[Number]
						  ,[RecordNumber]
						  ,[XMLDBF]
					  FROM [AMS_MonitorBusiness].[dbo].[GenerateXMLDBF] D
					  left join AMS_BaseData.MPInfo.TB_MonitoringPoint P
					  on d.PointId=p.PointId
                        WHERE {0} order by PointId asc, Tstamp desc
                    ", where);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }

        public DataView GetHourDatas(string portIds, string[] factors, DateTime dtStart, DateTime dtEnd, string orderBy = "PointId")
        {

            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                string factorMarkSql = string.Empty;
                string factorWhere = string.Empty;

                foreach (string factor in factors)
                {
                    string factorFlag = factor + "_Status";
                    string factorMark = factor + "_Mark";
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                    factorFlagSql += string.Format(",null AS {0} ", factorFlag);
                    factorMarkSql += string.Format(",null AS {0} ", factorMark);
                    factorWhere += "'" + factor + "',";
                }
                factorWhere = factorWhere.Remove(factorWhere.Length - 1, 1);

                string portIdsStr = " AND PointId =" + portIds;


                orderBy = string.IsNullOrEmpty(orderBy) ? "PointId" : orderBy;
                string fieldName = "PointId,Tstamp,data.PollutantCode,Bdata.ChemicalSymbol PollutantName,Status,PollutantValue,Bdata.DecimalDigit";
                string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}'  AND data.PollutantCode in ({2})", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + portIdsStr;
                string sql = string.Format(@"SELECT {0}
                        FROM  Air.TB_InfectantBy60 data
                        left join [AMS_BaseData].[Standard].[TB_PollutantCode] Bdata
                        on data.PollutantCode=Bdata.PollutantCode
                        WHERE {1}
                    ", fieldName, where);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }

        public void GetAddData(string pointId, string PointAQM, string FileName, DateTime tstamp, DateTime dtTime, string Number, int RecordNumber, int XMLDBF)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("insert into [AMS_MonitorBusiness].[dbo].[GenerateXMLDBF](PointId,PointAQM,FileName,Tstamp,DateTime,IsSuccess,Note,Number,RecordNumber,CreaterUser,XMLDBF)");
                builder.Append(" values(" + pointId + ",'" + PointAQM + "','" + FileName + "','" + tstamp + "','" + dtTime + "'," + 1 + ",'','" + Number + "'," + RecordNumber + ",''," + XMLDBF);
                builder.Append(")");
                string strSql = builder.ToString();
                g_DatabaseHelper.ExecuteNonQuery(strSql.ToString(), connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void GetUpdateData(string pointId, DateTime tstamp, int RecordNumber)
        {
            try
            {
                StringBuilder builderUpData = new StringBuilder();
                builderUpData.Append("update [AMS_MonitorBusiness].[dbo].[GenerateXMLDBF]");
                builderUpData.Append("set RecordNumber=" + RecordNumber + ",UpdateTme='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'");
                builderUpData.Append(" where PointId='" + pointId + "' AND Tstamp='" + tstamp + "'");
                string strSqlUp = builderUpData.ToString();
                g_DatabaseHelper.ExecuteNonQuery(strSqlUp.ToString(), connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 常规站补充缺失数据时间（包括O3-8因子）
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
        public DataView GetDataPagerAllTimeWithO8(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId asc,tstamp desc")
        {
          recordTotal = 0;
          try
          {
            string tableUnion = string.Format(@"(select  PointId,DateTime as Tstamp,'a05027' as PollutantCode,case when Recent8HoursO3NT is null then null  when Recent8HoursO3NT='' then null else cast(Recent8HoursO3NT as decimal(18,4)) end as PollutantValue,'' as Status,'' as DataFlag,'' as AuditFlag from [Air].[TB_OriHourAQI] union all select PointId,Tstamp,PollutantCode,PollutantValue,Status,DataFlag,AuditFlag from {0}) as dt ", tableName);
            //取得查询行转列字段拼接
            string factorSql = string.Empty;
            string factorFlagSql = string.Empty;
            string factorDataFlagSql = string.Empty;
            string factorAuditFlagSql = string.Empty;
            string factorWhere = string.Empty;
            string factorFieldSql = string.Empty;

            foreach (string factor in factors)
            {
              string factorFlag = factor + "_Status";
              string factorDataFlag = factor + "_DataFlag";
              string factorAuditFlag = factor + "_AuditFlag";
              factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
              factorFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN Status END) AS [{1}] ", factor, factorFlag);
              factorDataFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN DataFlag END) AS [{1}] ", factor, factorDataFlag);
              factorAuditFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN AuditFlag END) AS [{1}] ", factor, factorAuditFlag);
              factorWhere += "'" + factor + "',";
              factorFieldSql += "," + factor;
              factorFieldSql += "," + factorFlag;
              factorFieldSql += "," + factorDataFlag;
              factorFieldSql += "," + factorAuditFlag;
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

            orderBy = string.IsNullOrEmpty(orderBy) ? "order by PointId,Tstamp" : "order by " + orderBy;
            string fieldName = "PointId,Tstamp" + factorSql + factorFlagSql + factorDataFlagSql + factorAuditFlagSql;
            string groupBy = "PointId,Tstamp";
            string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}'  AND PollutantCode in ({2})", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + portIdsStr;

            string dataSql = string.Format(@"select {0} INTO #tempTable from {1}   where {2} group by {3} ", fieldName, tableUnion, where, groupBy);
            //补充时间段
            string sql = string.Empty;
            string allHoursSql = string.Empty;
            //由于实时数据通常晚1小时到数据库（离子色谱仪晚两个小时），补数据时间需要剔除当前小时及之后的小时
            DateTime AllHourdtEndNew = Convert.ToDateTime(dtEnd.ToString("yyyy-MM-dd HH:00")) >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00")) ?
                DateTime.Now.AddHours(-1) : dtEnd;

            allHoursSql += string.Format(@"select time.PointId,time.Tstamp{0} from dbo.SY_F_GetAllDataByHour('{1}',',','{2}','{3}') time", factorFieldSql, StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ","), dtStart.ToString("yyyy-MM-dd HH:mm:ss"), AllHourdtEndNew.ToString("yyyy-MM-dd HH:mm:ss"));

            sql += string.Format(@"{0} left join #tempTable data
                        on convert(varchar(13),time.tstamp,120)=convert(varchar(13),data.Tstamp,120)
                        and time.pointId=data.PointId"
                , allHoursSql);
            DataView dv = g_GridViewPager.GridViewPagerAllTimeSql(sql, dataSql, pageSize, pageNo, orderBy, connection, out recordTotal);

            //DataView dv = g_DatabaseHelper.ExecuteDataView(sql, connection);
            //DataView dv = g_GridViewPager.GridViewPagerCplexSql(sql, pageSize, pageNo, orderBy, connection, out recordTotal);
            return dv;
            //return g_GridViewPager.GetPivotDataPager(tableName, fieldName, groupBy, orderBy, where, pageSize, pageNo, connection, out recordTotal);
          }
          catch (Exception ex)
          {
            throw ex;
          }
        }
        /// <summary>
        /// 常规站补充缺失数据时间（包括O3-8因子）
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
        public DataView GetDataPagerAllTimeWithO8Region(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "sortNumber desc,tstamp desc")
        {
          recordTotal = 0;
          try
          {
            string tableUnion = string.Format(@"(select PointId,Tstamp,d1.PollutantCode,cast([dbo].[F_Round](d1.PollutantValue,case when d2.DecimalDigit is null then 3 when d2.DecimalDigit='' then 3 else d2.DecimalDigit end) as decimal(18,4)) as PollutantValue,Status,DataFlag,AuditFlag from(select  PointId,DateTime as Tstamp,'a05027' as PollutantCode,case when Recent8HoursO3NT is null then null  when Recent8HoursO3NT='' then null else cast(Recent8HoursO3NT as decimal(18,4)) end as PollutantValue,'' as Status,'' as DataFlag,'' as AuditFlag from [Air].[TB_OriHourAQI] union all select PointId,Tstamp,PollutantCode,PollutantValue,'' as Status,'' as DataFlag,'' as AuditFlag from {0}) as d1 left join  [AMS_BaseData].[dbo].[V_Factor] as d2 on d1.PollutantCode=d2.PollutantCode) as dt ", tableName);
            
            //取得查询行转列字段拼接
            string factorSql = string.Empty;
            string factorFlagSql = string.Empty;
            string factorDataFlagSql = string.Empty;
            string factorAuditFlagSql = string.Empty;
            string factorWhere = string.Empty;
            string factorFieldSql = string.Empty;
            string factorNewSql = string.Empty;
            string factorFlagNewSql = string.Empty;
            string factorDataFlagNewSql = string.Empty;
            string factorAuditFlagNewSql = string.Empty;
            foreach (string factor in factors)
            {
              string factorFlag = factor + "_Status";
              string factorDataFlag = factor + "_DataFlag";
              string factorAuditFlag = factor + "_AuditFlag";
              factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
              factorNewSql += string.Format(",Avg({0}) as [{0}] ", factor);
              factorFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN Status END) AS [{1}] ", factor, factorFlag);
             
              factorFlagNewSql += string.Format(",MAX({0}) AS [{0}] ", factorFlag);
              factorDataFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN DataFlag END) AS [{1}] ", factor, factorDataFlag);
              factorDataFlagNewSql += string.Format(",MAX({0}) AS [{0}] ", factorDataFlag);
              factorAuditFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN AuditFlag END) AS [{1}] ", factor, factorAuditFlag);
              factorAuditFlagNewSql += string.Format(",MAX({0}) AS [{0}] ", factorAuditFlag);
              factorWhere += "'" + factor + "',";
              factorFieldSql += "," + factor;
              factorFieldSql += "," + factorFlag;
              factorFieldSql += "," + factorDataFlag;
              factorFieldSql += "," + factorAuditFlag;
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

           // orderBy = string.IsNullOrEmpty(orderBy) ? "order by sortNumber desc,Tstamp" : "order by " + orderBy;
            string fieldName = "PointId,Tstamp" + factorSql + factorFlagSql + factorDataFlagSql + factorAuditFlagSql;
            string newFiledName = " Max(t2.Region) as PointId,Tstamp, sortNumber" + factorNewSql +factorFlagNewSql+factorDataFlagNewSql + factorAuditFlagNewSql;
            string groupBy = "PointId,Tstamp";
            string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}'  AND PollutantCode in ({2})", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + portIdsStr;

            string dataSql = string.Format(@"select {0} from {1} where {2} group by {3} ", fieldName, tableUnion, where, groupBy);
            //补充时间段
            string sql = string.Empty;
            string allHoursSql = string.Empty;
            //由于实时数据通常晚1小时到数据库（离子色谱仪晚两个小时），补数据时间需要剔除当前小时及之后的小时
            DateTime AllHourdtEndNew = Convert.ToDateTime(dtEnd.ToString("yyyy-MM-dd HH:00")) >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00")) ?
                DateTime.Now.AddHours(-1) : dtEnd;

            allHoursSql += string.Format(@"select time.PointId,time.Tstamp{0} from dbo.SY_F_GetAllDataByHour('{1}',',','{2}','{3}') time", factorFieldSql, StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ","), dtStart.ToString("yyyy-MM-dd HH:mm:ss"), AllHourdtEndNew.ToString("yyyy-MM-dd HH:mm:ss"));

            string table1= string.Format(@"({0} left join ({1}) data
                        on convert(varchar(13),time.tstamp,120)=convert(varchar(13),data.Tstamp,120)
                        and time.pointId=data.PointId)"
                , allHoursSql, dataSql);
           sql = string.Format(@"select {0} from {1} as t1 left join {2} as t2 on t1.PointId=t2.PortId  inner join [AMS_BaseData].[dbo].[SY_View_CodeMainItem] svcRG
                                         on t2.RegionUid = svcRG.ItemGuid
            group by t2.RegionUid,t1.Tstamp,svcRG.sortNumber", newFiledName, table1, "[AMS_BaseData].[dbo].[V_Point_UserConfigNew]");
            //DataView dv = g_DatabaseHelper.ExecuteDataView(sql, connection);
            DataView dv = g_GridViewPager.GridViewPagerCplexSql(sql, pageSize, pageNo, orderBy, connection, out recordTotal);
            return dv;
            //return g_GridViewPager.GetPivotDataPager(tableName, fieldName, groupBy, orderBy, where, pageSize, pageNo, connection, out recordTotal);
          }
          catch (Exception ex)
          {
            throw ex;
          }
        }
     
    }
}
