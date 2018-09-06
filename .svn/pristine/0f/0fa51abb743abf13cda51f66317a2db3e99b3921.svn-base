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

namespace SmartEP.Data.SqlServer.MonitoringBusiness.Water
{
    /// <summary>
    /// 名称：MonthReportDAL.cs
    /// 创建人：李飞
    /// 创建日期：2015-08-18
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 地表水发布：月数据库处理
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
        /// 数据库表名
        /// </summary>
        private string tableName = "WaterReport.TB_MonthReport";
        #endregion

        #region << 构造函数 >>
        /// <summary>
        /// 构造函数
        /// </summary>
        public MonthReportDAL()
        {
            tableName = SmartEP.Data.Enums.EnumMapping.GetAutoMonitorTableName(ApplicationType.Water, PollutantDataType.Month);
            connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(ApplicationType.Water, PollutantDataType.Month);
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
                string factorDataFlagSql = string.Empty;
                string factorAuditFlagSql = string.Empty;
                string factorEQISql = string.Empty;
                string factorGradeSql = string.Empty;
                string wqSql = string.Empty;
                foreach (string factor in factors)
                {
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                    //factorDataFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN dataFlag END ) AS [{1}] ", factor, factor + "_dataFlag");
                    //factorAuditFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN AuditFlag END ) AS [{1}] ", factor, factor + "_AuditFlag");
                    factorEQISql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN EQI END ) AS [{1}] ", factor, factor + "_EQI");
                    factorGradeSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN Grade END ) AS [{1}] ", factor, factor + "_Grade");
                }
                wqSql = ",MAX(CASE(PollutantCode) WHEN 'WaterQuality' THEN EQI END ) AS [EQI] ";
                wqSql += ",MAX(CASE(PollutantCode) WHEN 'WaterQuality' THEN Grade END ) AS [Grade] ";

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
                string fieldName = "PointId,Year,MonthOfYear" + factorSql + factorDataFlagSql + factorAuditFlagSql + factorEQISql + factorGradeSql + wqSql;
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
                string fieldName = "PointId,'审核' as Type,ReportDateTime,Year,MonthOfYear,CONVERT(NVARCHAR(5),Year)+'-'+CONVERT(NVARCHAR(2),MonthOfYear) AS DateTime" + factorSql + factorFlagSql;
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
        public DataView GetExportData(string[] portIds, string[] factors
            , int yearFrom, int monthOfYearFrom, int yearTo, int monthOfYearTo, string orderBy = "PointId,Year,MonthOfYear")
        {
            tableName = "dbo.V_Water_MonthReport";
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "PointId,Year,MonthOfYear";
            int recordTotal = 0;
            int pageSize = 99999999;
            int pageNo = 0;

            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                foreach (string factor in factors)
                {
                    string factorFlag = factor + "_Status";
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
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
                orderBy = string.IsNullOrEmpty(orderBy) ? "PointId,Year,MonthOfYear" : orderBy;
                string fieldName = "rowNum as '序号',PointId,portName,Year as '年',MonthOfYear as '月'" + factorSql + factorFlagSql;
                string groupBy = "PointId,portName,Year,MonthOfYear";
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
        /// 水质自动监测月度小结,水质监测统计表数据源
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="nowtime"></param>
        /// <returns></returns>
        public DataView GetMonthSummaryDataSource(string[] portIds, DateTime nowTime)
        {
            DateTime lastMonthTime = nowTime.AddMonths(-1);
            DateTime lastYearTime = nowTime.AddYears(-1);
            string sql = string.Empty;
            foreach (string pId in portIds)
            {
                if (sql != string.Empty) sql += " union ";
                sql = string.Format(@"SELECT  
                                      p.monitoringPointName AS '水源地名称',
                                      '{1}' AS '时间段',
                                      Cast (MAX(CASE(PollutantCode) WHEN 'w01009' THEN PollutantValue END ) as decimal(18,2)) AS 溶解氧, 
                                      Cast (MAX(CASE(PollutantCode) WHEN 'w01019' THEN PollutantValue END ) as decimal(18,1)) AS 高锰酸盐指数, 
                                      Cast (MAX(CASE(PollutantCode) WHEN 'w21003' THEN PollutantValue END ) as decimal(18,2)) AS 氨氮, 
                                      Cast (MAX(CASE(PollutantCode) WHEN 'w21011' THEN PollutantValue END ) as decimal(18,3)) AS 总磷, 
                                      Cast (MAX(CASE(PollutantCode) WHEN 'w21001' THEN PollutantValue END ) as decimal(18,2)) AS 总氮, 
                                      Cast (MAX(CASE(PollutantCode) WHEN 'w19011' THEN PollutantValue END ) as decimal(18,1)) AS 藻密度
                                FROM WaterReport.TB_MonthReport AS d INNER JOIN
                                      dbo.SY_MonitoringPoint as p on p.pointId = d.PointId 
                                WHERE (d.Year = CONVERT(NVARCHAR(10), '{2}', 120)) AND 
                                      (d.MonthOfYear = CONVERT(NVARCHAR(10), '{3}', 120))
                                         AND p.pointId = {0}
                                group by p.monitoringpointname  ", pId, nowTime.ToString("yyyy年MM月") + "均值", nowTime.Year, nowTime.Month);
                sql += " union " + string.Format(@"SELECT  
                                      p.monitoringPointName AS '水源地名称',
                                      '{1}' AS '时间段',
                                      Cast (MAX(CASE(PollutantCode) WHEN 'w01009' THEN PollutantValue END ) as decimal(18,2)) AS 溶解氧, 
                                      Cast (MAX(CASE(PollutantCode) WHEN 'w01019' THEN PollutantValue END ) as decimal(18,1)) AS 高锰酸盐指数, 
                                      Cast (MAX(CASE(PollutantCode) WHEN 'w21003' THEN PollutantValue END ) as decimal(18,2)) AS 氨氮, 
                                      Cast (MAX(CASE(PollutantCode) WHEN 'w21011' THEN PollutantValue END ) as decimal(18,3)) AS 总磷, 
                                      Cast (MAX(CASE(PollutantCode) WHEN 'w21001' THEN PollutantValue END ) as decimal(18,2)) AS 总氮, 
                                      Cast (MAX(CASE(PollutantCode) WHEN 'w19011' THEN PollutantValue END ) as decimal(18,1)) AS 藻密度
                                FROM WaterReport.TB_MonthReport AS d INNER JOIN
                                      dbo.SY_MonitoringPoint as p on p.pointId = d.PointId 
                                WHERE (d.Year = CONVERT(NVARCHAR(10), '{2}', 120)) AND 
                                      (d.MonthOfYear = CONVERT(NVARCHAR(10), '{3}', 120))
                                         AND p.pointId = {0}
                                group by p.monitoringpointname  ", pId, lastMonthTime.ToString("yyyy年MM月") + "均值", lastMonthTime.Year, lastMonthTime.Month);
                sql += " union " + string.Format(@"SELECT  
                                      p.monitoringPointName AS '水源地名称',
                                      '{1}' AS '时间段',
                                      Cast (MAX(CASE(PollutantCode) WHEN 'w01009' THEN PollutantValue END ) as decimal(18,2)) AS 溶解氧, 
                                      Cast (MAX(CASE(PollutantCode) WHEN 'w01019' THEN PollutantValue END ) as decimal(18,1)) AS 高锰酸盐指数, 
                                      Cast (MAX(CASE(PollutantCode) WHEN 'w21003' THEN PollutantValue END ) as decimal(18,2)) AS 氨氮, 
                                      Cast (MAX(CASE(PollutantCode) WHEN 'w21011' THEN PollutantValue END ) as decimal(18,3)) AS 总磷, 
                                      Cast (MAX(CASE(PollutantCode) WHEN 'w21001' THEN PollutantValue END ) as decimal(18,2)) AS 总氮, 
                                      Cast (MAX(CASE(PollutantCode) WHEN 'w19011' THEN PollutantValue END ) as decimal(18,1)) AS 藻密度
                                FROM WaterReport.TB_MonthReport AS d INNER JOIN
                                      dbo.SY_MonitoringPoint as p on p.pointId = d.PointId 
                                WHERE (d.Year = CONVERT(NVARCHAR(10), '{2}', 120)) AND 
                                      (d.MonthOfYear = CONVERT(NVARCHAR(10), '{3}', 120))
                                         AND p.pointId = {0}
                                group by p.monitoringpointname  ", pId, lastYearTime.ToString("yyyy年MM月") + "均值", lastYearTime.Year, lastYearTime.Month);
            }
            return g_DatabaseHelper.ExecuteDataView(sql, connection);
        }
        #endregion
    }
}
