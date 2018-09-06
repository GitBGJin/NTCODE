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

namespace SmartEP.Data.SqlServer.MonitoringBusiness.Air
{
    /// <summary>
    /// 名称：MonthReportDAL.cs
    /// 创建人：李飞
    /// 创建日期：2015-08-18
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 环境空气发布：月数据库处理
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class MonthReportDAL
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
        private string tableName = "AirReport.TB_MonthReport";
        #endregion

        #region << 构造函数 >>
        /// <summary>
        /// 构造函数
        /// </summary>
        public MonthReportDAL()
        {
            tableName = SmartEP.Data.Enums.EnumMapping.GetAutoMonitorTableName(ApplicationType.Air, PollutantDataType.Month);
            connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(ApplicationType.Air, PollutantDataType.Month);
        }
        #endregion

        #region << 方法 >>
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="monthOfYearFrom">开始月数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="monthOfYearTo">结束月数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetDataPager(string[] portIds, string[] factors
            , int yearFrom, int monthOfYearFrom, int yearTo, int monthOfYearTo, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Year,MonthOfYear")
        {
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "PointId,Year,MonthOfYear";
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

                orderBy = string.IsNullOrEmpty(orderBy) ? "PointId,Year,MonthOfYear" : orderBy;
                string fieldName = "PointId,Year,MonthOfYear" + factorSql + factorFlagSql;
                string groupBy = "PointId,Year,MonthOfYear";
                string where = string.Format(" ReportDateTime>='{0}-{1}-01' and ReportDateTime<='{2}-{3}-01' ", yearFrom, monthOfYearFrom, yearTo, monthOfYearTo) + portIdsStr;
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
        /// <param name="yearFrom">开始年</param>
        /// <param name="monthOfYearFrom">开始月数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="monthOfYearTo">结束月数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetDataPager(string[] portIds, string[] factors, int yearFrom, int monthOfYearFrom, int yearTo, int monthOfYearTo,
            int yearFromB, int monthOfYearFromB, int yearToB, int monthOfYearToB, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Year,MonthOfYear")
        {
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "PointId,Year,MonthOfYear";
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

                orderBy = string.IsNullOrEmpty(orderBy) ? "PointId,Year,MonthOfYear" : orderBy;
                string fieldName = "PointId,'审核' as Type,ReportDateTime,Year,MonthOfYear" + factorSql + factorFlagSql;
                string groupBy = "PointId,ReportDateTime,Year,MonthOfYear";
                string where = "";
                if (yearFromB > 0 && yearToB > 0)
                {
                    where = string.Format(" ((ReportDateTime>='{0}-{1}-01' and ReportDateTime<='{2}-{3}-01') or (ReportDateTime>='{4}-{5}-01' and ReportDateTime<='{6}-{7}-01')) ", yearFrom, monthOfYearFrom, yearTo, monthOfYearTo, yearFromB, monthOfYearFromB, yearToB, monthOfYearToB) + portIdsStr;
                }
                else
                {
                    where = string.Format(" ReportDateTime>='{0}-{1}-01' and ReportDateTime<='{2}-{3}-01' ", yearFrom, monthOfYearFrom, yearTo, monthOfYearTo) + portIdsStr;
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
        /// <param name="yearFrom">开始年</param>
        /// <param name="monthOfYearFrom">开始月数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="monthOfYearTo">结束月数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetDataPagerDF(string[] portIds, string[] factors, int yearFrom, int monthOfYearFrom, int yearTo, int monthOfYearTo,
             int pageSize, int pageNo,  string orderBy = "PointId,Year,MonthOfYear")
        {
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "PointId,Year,MonthOfYear";
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

                orderBy = string.IsNullOrEmpty(orderBy) ? "PointId,Year,MonthOfYear" : orderBy;
                string fieldName = "row_number() over(order by PointId,Year,MonthOfYear) as Ordernum ,PointId,'审核' as Type,ReportDateTime,Year,MonthOfYear" + factorSql + factorFlagSql;
                string groupBy = "PointId,ReportDateTime,Year,MonthOfYear";
                string where = "";
                //if (yearFromB > 0 && yearToB > 0)
                //{
                    //where = string.Format(" ((ReportDateTime>='{0}-{1}-01' and ReportDateTime<='{2}-{3}-01') or (ReportDateTime>='{4}-{5}-01' and ReportDateTime<='{6}-{7}-01')) ", yearFrom, monthOfYearFrom, yearTo, monthOfYearTo, yearFromB, monthOfYearFromB, yearToB, monthOfYearToB) + portIdsStr;
                //}
                //else
                //{
                where = string.Format(" ReportDateTime>='{0}-{1}-01' and ReportDateTime<='{2}-{3}-01' ", yearFrom, monthOfYearFrom, yearTo, monthOfYearTo) + portIdsStr;
                //}
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
        /// <param name="yearFrom">开始年</param>
        /// <param name="monthOfYearFrom">开始月数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="monthOfYearTo">结束月数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetDataPager(string[] portIds, IList<IPollutant> factors
            , int yearFrom, int monthOfYearFrom, int yearTo, int monthOfYearTo, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Year,MonthOfYear")
        {
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "PointId,Year,MonthOfYear";
            recordTotal = 0;
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                foreach (IPollutant factor in factors)
                {
                    Int32 decimalNum = string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum);
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN CAST(dbo.F_Round(PollutantValue,{1}) AS DECIMAL(10,{1})) END ) AS [{0}] ", factor.PollutantCode, decimalNum);
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

                orderBy = string.IsNullOrEmpty(orderBy) ? "PointId,Year,MonthOfYear" : orderBy;
                string fieldName = "PointId,Year,MonthOfYear" + factorSql + factorFlagSql;
                string groupBy = "PointId,Year,MonthOfYear";

                //string where = string.Format(" Year>={0} and Year<={1} and MonthOfYear>={2} and MonthOfYear<={3} ", yearFrom, yearTo, monthOfYearFrom, monthOfYearTo) + portIdsStr;
                string where = string.Format(" ReportDateTime>='{0}-{1}-01' and ReportDateTime<='{2}-{3}-01' ", yearFrom, monthOfYearFrom, yearTo, monthOfYearTo) + portIdsStr;
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
        /// <param name="yearFrom">开始年</param>
        /// <param name="monthOfYearFrom">开始月数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="monthOfYearTo">结束月数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetDataPagerRegion(string[] portIds, IList<IPollutant> factors
            , int yearFrom, int monthOfYearFrom, int yearTo, int monthOfYearTo, int pageSize, int pageNo, out int recordTotal, string orderBy )
        {
          orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "sortNumber desc,Year,MonthOfYear";
          recordTotal = 0;
          try
          {
            //取得查询行转列字段拼接
            string tabledt = string.Empty;
            string factorSql = string.Empty;
            string factorNewSql = string.Empty;
            string factorWhere = string.Empty;
            foreach (IPollutant factor in factors)
            {
              Int32 decimalNum = string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum);
              factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN CAST(dbo.F_Round(PollutantValue,{1}) AS DECIMAL(10,{1})) END ) AS [{0}] ", factor.PollutantCode, decimalNum);
              factorNewSql += string.Format(",Avg({0}) as [{0}] ", factor.PollutantCode);
              factorWhere += "'" + factor.PollutantCode + "',";
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
            string fieldName = "PointId,Year,MonthOfYear" + factorSql;
            string newFiledName = " Max(t2.Region) as PointId,Year,MonthOfYear,sortNumber" + factorNewSql;
            string groupBy = "PointId,Year,MonthOfYear,ReportDateTime";

            //string where = string.Format(" Year>={0} and Year<={1} and MonthOfYear>={2} and MonthOfYear<={3} ", yearFrom, yearTo, monthOfYearFrom, monthOfYearTo) + portIdsStr;
            string where = string.Format(" ReportDateTime>='{0}-{1}-01' and ReportDateTime<='{2}-{3}-01' AND PollutantCode in ({4}) ", yearFrom, monthOfYearFrom, yearTo, monthOfYearTo,factorWhere) + portIdsStr;
            tabledt = "(select PointId,Year,MonthOfYear,ReportDateTime,d1.PollutantCode,cast([dbo].[F_Round](d1.PollutantValue,case when d2.DecimalDigit is null then 3 when d2.DecimalDigit='' then 3 else d2.DecimalDigit end) as decimal(18,4)) as PollutantValue from [AMS_MonitorBusiness].[AirReport].[TB_MonthReport] as d1 left join [AMS_BaseData].[dbo].[V_Factor] as d2 on d1.PollutantCode=d2.PollutantCode) as dt";
            string table1 = string.Format(@"(select {0} from {1} where {2} group by {3}) ", fieldName, tabledt, where, groupBy);
            string sql = string.Format(@"select {0} from {1} as t1 left join {2} as t2 on t1.PointId=t2.PortId  inner join [AMS_BaseData].[dbo].[SY_View_CodeMainItem] svcRG
                                           on t2.RegionUid = svcRG.ItemGuid
            group by t2.RegionUid,Year,MonthOfYear,svcRG.sortNumber", newFiledName, table1, "[AMS_BaseData].[dbo].[V_Point_UserConfigNew]");
            return g_GridViewPager.GridViewPagerCplexSql(sql, pageSize, pageNo, orderBy, connection1, out recordTotal);
           
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
        /// <param name="monthOfYearFrom">开始月数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="monthOfYearTo">结束月数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetDataPagersRegion(string[] portIds, string[] factors
            , int yearFrom, int monthOfYearFrom, int yearTo, int monthOfYearTo, int pageSize, int pageNo, out int recordTotal, string orderBy)
        {
          orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "sortNumber desc,Year,MonthOfYear";
          recordTotal = 0;
          try
          {
            //取得查询行转列字段拼接
            string tabledt = string.Empty;
            string factorSql = string.Empty;
            string factorNewSql = string.Empty;
            string factorWhere = string.Empty;
            foreach (string factor in factors)
            {
             
              factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue end ) AS [{0}] ", factor);
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
            string fieldName = "PointId,Year,MonthOfYear" + factorSql;
            string newFiledName = " Max(t2.Region) as PointId,Year,MonthOfYear,sortNumber" + factorNewSql;
            string groupBy = "PointId,Year,MonthOfYear,ReportDateTime";

            //string where = string.Format(" Year>={0} and Year<={1} and MonthOfYear>={2} and MonthOfYear<={3} ", yearFrom, yearTo, monthOfYearFrom, monthOfYearTo) + portIdsStr;
            string where = string.Format(" ReportDateTime>='{0}-{1}-01' and ReportDateTime<='{2}-{3}-01' AND PollutantCode in ({4}) ", yearFrom, monthOfYearFrom, yearTo, monthOfYearTo, factorWhere) + portIdsStr;
            tabledt = "(select PointId,Year,MonthOfYear,ReportDateTime,d1.PollutantCode,cast([dbo].[F_Round](d1.PollutantValue,case when d2.DecimalDigit is null then 3 when d2.DecimalDigit='' then 3 else d2.DecimalDigit end) as decimal(18,4)) as PollutantValue from [AMS_MonitorBusiness].[AirReport].[TB_MonthReport] as d1 left join [AMS_BaseData].[dbo].[V_Factor] as d2 on d1.PollutantCode=d2.PollutantCode) as dt";
            string table1 = string.Format(@"(select {0} from {1} where {2} group by {3}) ", fieldName, tabledt, where, groupBy);
            string sql = string.Format(@"select {0} from {1} as t1 left join {2} as t2 on t1.PointId=t2.PortId  inner join [AMS_BaseData].[dbo].[SY_View_CodeMainItem] svcRG
                                           on t2.RegionUid = svcRG.ItemGuid
            group by t2.RegionUid,Year,MonthOfYear,svcRG.sortNumber", newFiledName, table1, "[AMS_BaseData].[dbo].[V_Point_UserConfigNew]");
            return g_GridViewPager.GridViewPagerCplexSql(sql, pageSize, pageNo, orderBy, connection1, out recordTotal);

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
        public DataView GetDataPagerAvg(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId asc,Year desc,MonthOfYear desc")
        {
            recordTotal = 0;
            try
            {
                //取得查询行转列字段拼接
                string factorASql = string.Empty;
                string factorSql = "select PointId,Year,MonthOfYear,Convert(datetime,Convert(nvarchar(13),ReportDateTime,120)+':00:00') datetime";
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
                string groupBy = "PointId,Year,MonthOfYear,ReportDateTime";
                string where = string.Format(" ReportDateTime>='{0}' AND ReportDateTime<='{1}' ", dtStart.ToString("yyyy-MM-01"), Convert.ToDateTime(dtEnd.AddMonths(1).ToString("yyyy-MM-01 00:00:00")).AddSeconds(-1).ToString("yyyy-MM-dd")) + portIdsStr;
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
        /// <param name="yearFrom">开始年</param>
        /// <param name="monthOfYearFrom">开始月数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="monthOfYearTo">结束月数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetMonthDataAvg(string[] portIds, string[] factors
            , int yearFrom, int monthOfYearFrom, int yearTo, int monthOfYearTo, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Year,MonthOfYear")
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
                string where = string.Format(" ReportDateTime>='{0}-{1}-01' and ReportDateTime<='{2}-{3}-01' AND PollutantCode in ({4})", yearFrom, monthOfYearFrom, yearTo, monthOfYearTo, factorWhere) + portIdsStr;
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
        /// 取得导出数据(行转列数据)
        /// </summary>
        /// <param name="autoMonitorType">查询数据类型</param>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="monthOfYearFrom">开始月数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="monthOfYearTo">结束月数</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetExportData(string[] portIds, IList<IPollutant> factors
            , int yearFrom, int monthOfYearFrom, int yearTo, int monthOfYearTo, string orderBy = "PointId,Year,MonthOfYear")
        {
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "PointId,Year,MonthOfYear";
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
                orderBy = string.IsNullOrEmpty(orderBy) ? "PointId,Year,MonthOfYear" : orderBy;
                string fieldName = "rowNum as '序号',PointId,Year as '年',MonthOfYear as '月'" + factorSql + factorFlagSql;
                string groupBy = "PointId,Year,MonthOfYear";
                string where = string.Format(" ReportDateTime>='{0}-{1}-01' and ReportDateTime<='{2}-{3}-01' ", yearFrom, monthOfYearFrom, yearTo, monthOfYearTo) + portIdsStr;
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
        /// <param name="yearFrom">开始年</param>
        /// <param name="monthOfYearFrom">开始月数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="monthOfYearTo">结束月数</param>
        /// <returns></returns>
        public DataView GetStatisticalData(string[] portIds, string[] factors, int yearFrom, int monthOfYearFrom, int yearTo, int monthOfYearTo)
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
                string where = string.Format(" Year>='{0}' AND Year<='{1}' AND MonthOfYear>='{2}' AND MonthOfYear<='{3}' ", yearFrom, yearTo, monthOfYearFrom, monthOfYearTo) + portIdsStr;

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
        /// 取得行转列数据总行数
        /// </summary>
        /// <param name="portIds">测点数组</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="monthOfYearFrom">开始月数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="monthOfYearTo">结束月数</param>
        /// <returns></returns>
        public int GetAllDataCount(string[] portIds, int yearFrom, int monthOfYearFrom, int yearTo, int monthOfYearTo)
        {
            try
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
                string whereString = string.Format(" Year>='{0}' AND Year<='{1}' AND MonthOfYear>='{2}' AND MonthOfYear<='{3}' {4} ", yearFrom, yearTo, monthOfYearFrom, monthOfYearTo, portIdsStr);

                return g_GridViewPager.GetAllDataCount(tableName, "PointId,Year,MonthOfYear", whereString, connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 月报数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="monthDate"></param>
        /// <param name="isO3AQI"></param>
        /// <returns></returns>
        public DataView GetMonthReport(string[] portIds, string[] factors, int year, int month, bool isO3AQI)
        {
            //monthDate = monthDate.ToString("")
            return null;
        }

        /// <summary>
        /// 按月统计站点月报数据有效率
        /// </summary>
        /// <param name="portIds">站点列表</param>
        /// <param name="factorCodes">因子编码列表</param>
        /// <param name="startTime">开始时间(格式：yyyy-MM)</param>
        /// <param name="endTime">结束时间(格式：yyyy-MM)</param>
        /// <returns>PointName、Tstamp(yyyy-MM)、Factor_QualifiedNum、Factor_QualifiedRate</returns>
        public DataTable GetQualifiedRate(string[] portIds, string[] factorCodes, string startTime, string endTime)
        {
            try
            {
                //拼接Where条件
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = "  sym.PointId =" + portIdsStr;
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = "  sym.PointId IN (" + portIdsStr + ")";
                }
                string where = string.Format(" WHERE convert(char(7), [ReportDateTime], 20) between '{0}' and '{1}' ", startTime, endTime) + " AND " + portIdsStr.Replace("sym.", "");


                //拼接统计数据字串，使用UNION方式减少统计条数，提升统计速度
                StringBuilder colSb = new StringBuilder();
                StringBuilder rateSb = new StringBuilder();
                StringBuilder sqlSb = new StringBuilder();
                foreach (string factorCode in factorCodes)
                {
                    colSb.AppendFormat(@"
                                      ,Convert(varchar(10),SUM(case when PollutantCode = '{0}' then QualifiedNumber end)) as [{0}] "
                        , factorCode);
                    rateSb.AppendFormat(@"                                
			                          , dbo.F_Round((SUM(case when PollutantCode = '{0}' then QualifiedNumber end)/(SUM(case when PollutantCode = '{0}' then CollectionNumber end) + 0.0)*100), 1) as [{0}]                                      "
                     , factorCode);
                    sqlSb.AppendFormat(@",isnull([{0}],'--') [{0}] "
                     , factorCode);
                }
                string sql = string.Format(@"
                               SELECT sym.MonitoringPointName as PointName,'有效个数' DataType,tbr.Tstamp {4} from dbo.SY_MonitoringPoint sym 
                               Left Join (
                                   SELECT Pointid ,'有效个数' DataType,convert(char(7), [ReportDateTime], 20) as Tstamp {0}
                                   FROM dbo.TB_ReportQualifiedRateByDay 
                                   {2}
                                   GROUP BY Pointid, convert(char(7), [ReportDateTime], 20)                                                                                                                                                                       
                                    ) tbr
                              ON tbr.PointId = sym.PointId
                              where {3}	
                              UNION
                              SELECT sym.MonitoringPointName as PointName,'有效率' DataType,tbr.Tstamp {4} from dbo.SY_MonitoringPoint sym 
                               Left Join (
                                   SELECT Pointid ,convert(char(7), [ReportDateTime], 20) as Tstamp {1}
                                   FROM dbo.TB_ReportQualifiedRateByDay 
                                   {2}
                                   GROUP BY Pointid, convert(char(7), [ReportDateTime], 20)                                                                                                                                                                       
                                    ) tbr
                              ON tbr.PointId = sym.PointId
                              where {3}	                    
		                            "
                                   , colSb.ToString()
                                   , rateSb.ToString()
                                   , where, portIdsStr, sqlSb);
                return g_DatabaseHelper.ExecuteDataTable(sql, connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取监测点概况
        /// </summary>
        /// <param name="portIds">站点列表</param>
        /// <param name="startTime">开始时间(格式：yyyy-MM)</param>
        /// <param name="endTime">结束时间(格式：yyyy-MM)</param>
        public DataTable GetPointGeneral(string[] portIds, string startTime, string endTime)
        {
            try
            {
                string sql = string.Format(@"EXEC [dbo].[UP_AirMonthPointInfo] '{0}','{1}','{2}'", string.Join(";", portIds), startTime, endTime);
                //                string sql = @"IF EXISTS (SELECT OBJECTPROPERTY(id,N'IsUserTable'),* FROM [tempdb].[dbo].sysobjects where id = object_id(N'[tempdb].[dbo].[#TmpDataRep]'))
                //                              DROP TABLE #TmpDataRep;";//临时表
                //                sql += string.Format(@" SELECT * INTO #TmpDataRep 
                //                                       FROM(
                //                                              SELECT c.Pointid,B.PollutantCode,B.PollutantName,B.tstamp,B.OperationReason,B.[Description]
                //                                              FROM [Audit].[TB_AuditStatusForDay] A JOIN [Audit].[TB_AuditAirLog] B
                //                                              ON A.AuditStatusUid=B.[AuditStatusUid] JOIN [Audit].[TB_AuditAirInfectantByHour] C
                //                                              ON C.AuditStatusUid=A.AuditStatusUid AND B.tstamp=C.DataDateTime
                //                                              WHERE  IsAudit=1 AND [AuditFlag]<>''  AND  charindex('C',[AuditFlag])<=0 AND
                //                                                    A.[ApplicationUid]='airaaira-aira-aira-aira-airaairaaira' 
                //                                                    AND C.PointId  IN (SELECT * FROM dbo.F_Split('{0}',';')) AND C.DataDateTime>='{1}' AND C.DataDateTime<='{2}'
                //                                        ) A", string.Join(";", portIds), startTime, endTime);//日志记录写入临时表
                //                sql += string.Format(@" SELECT A.[MonitoringPointName] AS PointName,A.MonitoringPointUid,A.[RegionUid],A.[ContrlUid],a.PointId,
                //                                          STUFF((SELECT distinct ',' +CONVERT(VARCHAR(10),DAY(min(tstamp)))+'日'+Convert(varchar(5),min(tstamp),24)+'~'+Convert(varchar(5),max(tstamp),24) 
                //                                                 FROM #TmpDataRep WHERE PointId=A.PointId 
                //                                                 GROUP BY PollutantCode,PollutantName,CONVERT(DATETIME,CONVERT(VARCHAR(10),TSTAMP,120))
                //                                                 FOR XML PATH('')),1, 1, '')  as tstamp,
                //                                         STUFF((SELECT distinct ',' +PollutantName FROM #TmpDataRep WHERE PointId=A.PointId FOR XML PATH('')),1, 1, '')  as losefacors,
                //                                         STUFF((SELECT ',' +OperationReason FROM #TmpDataRep WHERE PointId=A.PointId and OperationReason<>'' FOR XML PATH('')),1, 1, '') as reason, 
                //                                         STUFF((SELECT ',' +[Description] FROM #TmpDataRep  WHERE PointId=A.PointId and [Description]<>'' FOR XML PATH('')),1, 1, '') as memo   
                //                                         FROM  [dbo].[SY_MonitoringPoint] A 
                //                                        where a.PointId  IN (SELECT * FROM dbo.F_Split('{0}',';')) ", string.Join(";", portIds));
                //                sql += "   DROP TABLE #TmpDataRep;";//删除临时表
                return g_DatabaseHelper.ExecuteDataTable(sql, connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
