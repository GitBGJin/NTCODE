﻿using SmartEP.Core.Enums;
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

namespace SmartEP.Data.SqlServer.MonitoringBusiness.Air
{
    /// <summary>
    /// 名称：SeasonReportDAL.cs
    /// 创建人：李飞
    /// 创建日期：2015-08-18
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 环境空气发布：季数据库处理
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class SeasonReportDAL
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
        /// 数据库连接字符串
        /// </summary>
        private string connection1 = "AMS_AirAutoMonitorConnection";

        /// <summary>
        /// 数据库表名
        /// </summary>
        private string tableName = "AirReport.TB_SeasonReport";
        #endregion

        #region << 构造函数 >>
        /// <summary>
        /// 构造函数
        /// </summary>
        public SeasonReportDAL()
        {
            tableName = SmartEP.Data.Enums.EnumMapping.GetAutoMonitorTableName(ApplicationType.Air, PollutantDataType.Season);
            connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(ApplicationType.Air, PollutantDataType.Season);
        }
        #endregion

        #region << 方法 >>
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="seasonOfYearFrom">开始季数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="seasonOfYearTo">结束季数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetDataPager(string[] portIds, string[] factors
            , int yearFrom, int seasonOfYearFrom, int yearTo, int seasonOfYearTo, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Year,SeasonOfYear")
        {
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "PointId,Year,SeasonOfYear";
            recordTotal = 0;

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

            orderBy = string.IsNullOrEmpty(orderBy) ? "PointId,Year,SeasonOfYear" : orderBy;
            string fieldName = "PointId,Year,SeasonOfYear" + factorSql + factorFlagSql;
            string groupBy = "PointId,Year,SeasonOfYear";
            //string where = string.Format(" Year>='{0}' AND Year<='{1}' AND SeasonOfYear>='{2}' AND SeasonOfYear<='{3}' ", yearFrom, yearTo, seasonOfYearFrom, seasonOfYearTo) + portIdsStr;
            //把季数据抽象成数字
            int seasonFrom = yearFrom * 1000 + seasonOfYearFrom;
            int seasonTo = yearTo * 1000 + seasonOfYearTo;
            string where = string.Format("  (Year*1000 + SeasonOfYear)>= {0} AND (Year*1000 + SeasonOfYear)<={1}  ", seasonFrom, seasonTo) + portIdsStr;

            return g_GridViewPager.GetPivotDataPager(tableName, fieldName, groupBy, orderBy, where, pageSize, pageNo, connection, out recordTotal);
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="seasonOfYearFrom">开始季数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="seasonOfYearTo">结束季数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetSeasonDataPagerRegion(string[] portIds, string[] factors
            , int yearFrom, int seasonOfYearFrom, int yearTo, int seasonOfYearTo, int pageSize, int pageNo, out int recordTotal, string orderBy)
        {
          string tabledt = string.Empty;
          orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "sortNumber desc,Year,SeasonOfYear";
          recordTotal = 0;

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
          string fieldName = "PointId,Year,SeasonOfYear" + factorSql;
          string newFiledName = " Max(t2.Region) as PointId,Year,SeasonOfYear,sortNumber" + factorNewSql;
          string groupBy = "PointId,Year,SeasonOfYear";
          //string where = string.Format(" Year>='{0}' AND Year<='{1}' AND SeasonOfYear>='{2}' AND SeasonOfYear<='{3}' ", yearFrom, yearTo, seasonOfYearFrom, seasonOfYearTo) + portIdsStr;
          //把季数据抽象成数字
          int seasonFrom = yearFrom * 1000 + seasonOfYearFrom;
          int seasonTo = yearTo * 1000 + seasonOfYearTo;
          string where = string.Format("  (Year*1000 + SeasonOfYear)>= {0} AND (Year*1000 + SeasonOfYear)<={1}  AND PollutantCode in ({2})", seasonFrom, seasonTo,factorWhere) + portIdsStr;

          tabledt = "(select PointId,Year,SeasonOfYear,d1.PollutantCode,cast([dbo].[F_Round](d1.PollutantValue,case when d2.DecimalDigit is null then 3 when d2.DecimalDigit='' then 3 else d2.DecimalDigit end) as decimal(18,4)) as PollutantValue from [AMS_MonitorBusiness].[AirReport].[TB_SeasonReport] as d1 left join [AMS_BaseData].[dbo].[V_Factor] as d2 on d1.PollutantCode=d2.PollutantCode) as dt";
          string table1 = string.Format(@"(select {0} from {1} where {2} group by {3}) ", fieldName, tabledt, where, groupBy);
          string sql = string.Format(@"select {0} from {1} as t1 left join {2} as t2 on t1.PointId=t2.PortId  inner join [AMS_BaseData].[dbo].[SY_View_CodeMainItem] svcRG
                                         on t2.RegionUid = svcRG.ItemGuid
          group by t2.RegionUid,Year,SeasonOfYear,svcRG.sortNumber", newFiledName, table1, "[AMS_BaseData].[dbo].[V_Point_UserConfigNew]");
          return g_GridViewPager.GridViewPagerCplexSql(sql, pageSize, pageNo, orderBy, connection1, out recordTotal);
        }
      
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="seasonOfYearFrom">开始季数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="seasonOfYearTo">结束季数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetDataPagerAvg(string[] portIds, string[] factors
            , int yearFrom, int seasonOfYearFrom, int yearTo, int seasonOfYearTo, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Year,SeasonOfYear")
        {
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "PointId,Year,SeasonOfYear";
            recordTotal = 0;

            //取得查询行转列字段拼接
            string factorAsql = "Select Year,SeasonOfYear ";
            string factorSql = "select PointId,Year,SeasonOfYear";
            string factorFlagSql = string.Empty;
            foreach (string factor in factors)
            {
                factorAsql += string.Format(",AVG({0}) {0}", factor);
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

            orderBy = string.IsNullOrEmpty(orderBy) ? "PointId,Year,SeasonOfYear" : orderBy;
            string fieldName = "PointId,Year,SeasonOfYear" + factorSql + factorFlagSql;
            string groupBy = "PointId,Year,SeasonOfYear";
            //把季数据抽象成数字
            int seasonFrom = yearFrom * 1000 + seasonOfYearFrom;
            int seasonTo = yearTo * 1000 + seasonOfYearTo;
            string where = string.Format("  (Year*1000 + SeasonOfYear)>= {0} AND (Year*1000 + SeasonOfYear)<={1}  ", seasonFrom, seasonTo) + portIdsStr;
            string sql = string.Format(@"{0} from ({1} from {2} where {3} group by {4})data group by Year,SeasonOfYear order by Year asc,SeasonOfYear asc", factorAsql, factorSql, tableName, where, groupBy);
            //return g_GridViewPager.GetPivotDataPager(tableName, fieldName, groupBy, orderBy, where, pageSize, pageNo, connection, out recordTotal);
            DataView dv = g_DatabaseHelper.ExecuteDataView(sql,connection);
            return dv;
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="seasonOfYearFrom">开始季数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="seasonOfYearTo">结束季数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetSeasonDataAvg(string[] portIds, string[] factors
            , int yearFrom, int seasonOfYearFrom, int yearTo, int seasonOfYearTo, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Year,SeasonOfYear")
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

                string groupby = "Year,SeasonOfYear";
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND PointId =" + portIdsStr;
                    groupby = "PointId,Year,SeasonOfYear";
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = "AND PointId IN(" + portIdsStr + ")";
                }

                orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "Year,SeasonOfYear";
                string fieldName = "PointId,Year,SeasonOfYear" + factorSql;
                int seasonFrom = yearFrom * 1000 + seasonOfYearFrom;
                int seasonTo = yearTo * 1000 + seasonOfYearTo;
                string where = string.Format("  (Year*1000 + SeasonOfYear)>= {0} AND (Year*1000 + SeasonOfYear)<={1} AND PollutantCode in ({2}) ", seasonFrom, seasonTo, factorWhere) + portIdsStr;
                string sql = string.Format(@"select {5}, Year,SeasonOfYear
											{3}
											from(
											select {1}
											 from {0}
											where {2}
                                            group by PointId,Year,SeasonOfYear
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
        /// <param name="yearFrom">开始年</param>
        /// <param name="seasonOfYearFrom">开始季数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="seasonOfYearTo">结束季数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetDataPager(string[] portIds, string[] factors, int yearFrom, int seasonOfYearFrom, int yearTo, int seasonOfYearTo
            , int yearFromB, int seasonOfYearFromB, int yearToB, int seasonOfYearToB, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Year,SeasonOfYear")
        {
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "PointId,Year,SeasonOfYear";
            recordTotal = 0;

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

            orderBy = string.IsNullOrEmpty(orderBy) ? "PointId,Year,SeasonOfYear" : orderBy;
            string fieldName = "PointId,'审核' as Type,Year,SeasonOfYear" + factorSql + factorFlagSql;
            string groupBy = "PointId,Year,SeasonOfYear";
            //string where = string.Format(" Year>='{0}' AND Year<='{1}' AND SeasonOfYear>='{2}' AND SeasonOfYear<='{3}' ", yearFrom, yearTo, seasonOfYearFrom, seasonOfYearTo) + portIdsStr;
            //把季数据抽象成数字
            int seasonFrom = yearFrom * 1000 + seasonOfYearFrom;
            int seasonTo = yearTo * 1000 + seasonOfYearTo;
            string where = "";
            if (yearFromB > 0 && yearToB > 0)
            {
                int seasonFromB = yearFromB * 1000 + seasonOfYearFromB;
                int seasonToB = yearToB * 1000 + seasonOfYearToB;
                where = string.Format("  (((Year*1000 + SeasonOfYear)>= {0} AND (Year*1000 + SeasonOfYear)<={1}) or ((Year*1000 + SeasonOfYear)>= {2} AND (Year*1000 + SeasonOfYear)<={3}))  ", seasonFrom, seasonTo, seasonFromB, seasonToB) + portIdsStr;
            }
            else
                where = string.Format("  (Year*1000 + SeasonOfYear)>= {0} AND (Year*1000 + SeasonOfYear)<={1}  ", seasonFrom, seasonTo) + portIdsStr;

            return g_GridViewPager.GetPivotDataPager(tableName, fieldName, groupBy, orderBy, where, pageSize, pageNo, connection, out recordTotal);
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="seasonOfYearFrom">开始季数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="seasonOfYearTo">结束季数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetDataPagerDF(string[] portIds, string[] factors, int yearFrom, int seasonOfYearFrom, int yearTo, int seasonOfYearTo
            ,  int pageSize, int pageNo,  string orderBy = "PointId,Year,SeasonOfYear")
        {
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "PointId,Year,SeasonOfYear";
            int recordTotal = 0;

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

            orderBy = string.IsNullOrEmpty(orderBy) ? "PointId,Year,SeasonOfYear" : orderBy;
            string fieldName = "row_number() over(order by PointId,Year,SeasonOfYear) as Ordernum ,PointId,'审核' as Type,Year,SeasonOfYear" + factorSql + factorFlagSql;
            string groupBy = "PointId,Year,SeasonOfYear";
            //string where = string.Format(" Year>='{0}' AND Year<='{1}' AND SeasonOfYear>='{2}' AND SeasonOfYear<='{3}' ", yearFrom, yearTo, seasonOfYearFrom, seasonOfYearTo) + portIdsStr;
            //把季数据抽象成数字
            int seasonFrom = yearFrom * 1000 + seasonOfYearFrom;
            int seasonTo = yearTo * 1000 + seasonOfYearTo;
            string where = "";
            //if (yearFromB > 0 && yearToB > 0)
            //{
            //    int seasonFromB = yearFromB * 1000 + seasonOfYearFromB;
            //    int seasonToB = yearToB * 1000 + seasonOfYearToB;
            //    where = string.Format("  (((Year*1000 + SeasonOfYear)>= {0} AND (Year*1000 + SeasonOfYear)<={1}) or ((Year*1000 + SeasonOfYear)>= {2} AND (Year*1000 + SeasonOfYear)<={3}))  ", seasonFrom, seasonTo, seasonFromB, seasonToB) + portIdsStr;
            //}
            //else
                where = string.Format("  (Year*1000 + SeasonOfYear)>= {0} AND (Year*1000 + SeasonOfYear)<={1}  ", seasonFrom, seasonTo) + portIdsStr;

            return g_GridViewPager.GetPivotDataPager(tableName, fieldName, groupBy, orderBy, where, pageSize, pageNo, connection, out recordTotal);
        }
        /// <summary>
        /// 取得导出数据(行转列数据)
        /// </summary>
        /// <param name="autoMonitorType">查询数据类型</param>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="seasonOfYearFrom">开始季数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="seasonOfYearTo">结束季数</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetExportData(string[] portIds, IList<IPollutant> factors
            , int yearFrom, int seasonOfYearFrom, int yearTo, int seasonOfYearTo, string orderBy = "PointId,Year,SeasonOfYear")
        {
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "PointId,Year,SeasonOfYear";
            int recordTotal = 0;
            int pageSize = 99999999;
            int pageNo = 0;

            //取得查询行转列字段拼接
            string factorSql = string.Empty;
            string factorFlagSql = string.Empty;
            foreach (IPollutant factor in factors)
            {
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
            orderBy = string.IsNullOrEmpty(orderBy) ? "PointId,Year,SeasonOfYear" : orderBy;
            string fieldName = "rowNum as '序号',PointId,Year as '年',SeasonOfYear as '季'" + factorSql + factorFlagSql;
            string groupBy = "PointId,Year,SeasonOfYear";
            //string where = string.Format(" ReportDateTime>=dbo.F_SeasonDate({0},{1}) and ReportDateTime<=dbo.F_SeasonDate({2},{3}) ", yearFrom, seasonOfYearFrom, yearTo, seasonOfYearTo) + portIdsStr;
            //把季数据抽象成数字
            int seasonFrom = yearFrom * 1000 + seasonOfYearFrom;
            int seasonTo = yearTo * 1000 + seasonOfYearTo;
            string where = string.Format("  (Year*1000 + SeasonOfYear)>= {0} AND (Year*1000 + SeasonOfYear)<={1}  ", seasonFrom, seasonTo) + portIdsStr;
            return g_GridViewPager.GetPivotDataPager(tableName, fieldName, groupBy, orderBy, where, pageSize, pageNo, connection, out recordTotal);
        }

        /// <summary>
        /// 取得统计数据（最大值、最小值、平均值）
        /// </summary>
        /// <param name="autoMonitorType">数据类型</param>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="seasonOfYearFrom">开始季数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="seasonOfYearTo">结束季数</param>
        /// <returns></returns>
        public DataView GetStatisticalData(string[] portIds, string[] factors, int yearFrom, int seasonOfYearFrom, int yearTo, int seasonOfYearTo)
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
            //string where = string.Format(" ReportDateTime>=dbo.F_SeasonDate({0},{1}) and ReportDateTime<=dbo.F_SeasonDate({2},{3}) ", yearFrom, seasonOfYearFrom, yearTo, seasonOfYearTo) + portIdsStr;
            //把季数据抽象成数字
            int seasonFrom = yearFrom * 1000 + seasonOfYearFrom;
            int seasonTo = yearTo * 1000 + seasonOfYearTo;
            string where = string.Format("  (Year*1000 + SeasonOfYear)>= {0} AND (Year*1000 + SeasonOfYear)<={1}  ", seasonFrom, seasonTo) + portIdsStr;


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

        /// <summary>
        /// 取得行转列数据总行数
        /// </summary>
        /// <param name="portIds">测点数组</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="seasonOfYearFrom">开始季数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="seasonOfYearTo">结束季数</param>
        /// <returns></returns>
        public int GetAllDataCount(string[] portIds, int yearFrom, int seasonOfYearFrom, int yearTo, int seasonOfYearTo)
        {
            //查询条件拼接
            string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
            if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
            {
                portIdsStr = " AND PointId =" + portIdsStr;
            }
            else if (!string.IsNullOrEmpty(portIdsStr))
            {
                portIdsStr = " AND PointId IN(" + portIdsStr + ")";
            }
            //string whereString = string.Format(" Year>='{0}' AND Year<='{1}' AND SeasonOfYear>='{2}' AND SeasonOfYear<='{3}' {4} ", yearFrom, yearTo, seasonOfYearFrom, seasonOfYearTo, portIdsStr);
            //把季数据抽象成数字
            int seasonFrom = yearFrom * 1000 + seasonOfYearFrom;
            int seasonTo = yearTo * 1000 + seasonOfYearTo;
            string where = string.Format("  (Year*1000 + SeasonOfYear)>= {0} AND (Year*1000 + SeasonOfYear)<={1}  ", seasonFrom, seasonTo) + portIdsStr;

            return g_GridViewPager.GetAllDataCount(tableName, "PointId,Year,SeasonOfYear", where, connection);
        }
        #endregion
    }
}
