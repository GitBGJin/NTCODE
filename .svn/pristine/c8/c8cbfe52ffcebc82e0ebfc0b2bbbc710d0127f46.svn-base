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
    /// 名称：SeasonReportDAL.cs
    /// 创建人：李飞
    /// 创建日期：2015-08-18
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 地表水发布：季数据库处理
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
        /// 数据库表名
        /// </summary>
        private string tableName = "WaterReport.TB_SeasonReport";
        #endregion

        #region << 构造函数 >>
        /// <summary>
        /// 构造函数
        /// </summary>
        public SeasonReportDAL()
        {
            tableName = SmartEP.Data.Enums.EnumMapping.GetAutoMonitorTableName(ApplicationType.Water, PollutantDataType.Season);
            connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(ApplicationType.Water, PollutantDataType.Season);
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
            string factorDataFlagSql = string.Empty;
            string factorAuditFlagSql = string.Empty;
            string factorEQISql = string.Empty;
            string factorGradeSql = string.Empty;
            string wqSql = string.Empty;
            foreach (string factor in factors)
            {
                factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                //factorDataFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN dataFlag END ) AS [{1}] ", factor, factor + "_dataFlag");
                //factorAuditFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN AuditFlag END ) AS [{1}] ",factor, factor + "_AuditFlag");
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

            orderBy = string.IsNullOrEmpty(orderBy) ? "PointId,Year,SeasonOfYear" : orderBy;
            string fieldName = "PointId,Year,SeasonOfYear" + factorSql + factorDataFlagSql + factorAuditFlagSql + factorEQISql + factorGradeSql + wqSql;
            string groupBy = "PointId,Year,SeasonOfYear";
            //string where = string.Format(" ReportDateTime>=dbo.F_SeasonDate({0},{1}) and ReportDateTime<=dbo.F_SeasonDate({2},{3}) ", yearFrom, seasonOfYearFrom, yearTo, seasonOfYearTo) + portIdsStr;
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
        public DataView GetExportData(string[] portIds, string[] factors
            , int yearFrom, int seasonOfYearFrom, int yearTo, int seasonOfYearTo, string orderBy = "PointId,Year,SeasonOfYear")
        {
            tableName = "dbo.V_Water_SeasonReport";
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "PointId,Year,SeasonOfYear";
            int recordTotal = 0;
            int pageSize = 99999999;
            int pageNo = 0;

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
            orderBy = string.IsNullOrEmpty(orderBy) ? "PointId,Year,SeasonOfYear" : orderBy;
            string fieldName = "rowNum as '序号',PointId,portName,Year as '年',SeasonOfYear as '季'" + factorSql + factorFlagSql;
            string groupBy = "PointId,portName,Year,SeasonOfYear";
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
            //string where = string.Format(" Year>='{0}' AND Year<='{1}' AND SeasonOfYear>='{2}' AND SeasonOfYear<='{3}' ", yearFrom, yearTo, seasonOfYearFrom, seasonOfYearTo) + portIdsStr;
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
