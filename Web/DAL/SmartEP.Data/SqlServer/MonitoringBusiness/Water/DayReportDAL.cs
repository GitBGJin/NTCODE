using SmartEP.Core.Enums;
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

namespace SmartEP.Data.SqlServer.MonitoringBusiness.Water
{
    /// <summary>
    /// 名称：DayReportDAL.cs
    /// 创建人：李飞
    /// 创建日期：2015-08-18
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 地表水发布：日数据处理
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
        /// 数据库表名
        /// </summary>
        private string tableName = "WaterReport.TB_DayReport";
        #endregion

        #region << 构造函数 >>
        /// <summary>
        /// 构造函数
        /// </summary>
        public DayReportDAL()
        {
            tableName = SmartEP.Data.Enums.EnumMapping.GetAutoMonitorTableName(ApplicationType.Water, PollutantDataType.Day);
            connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(ApplicationType.Water, PollutantDataType.Day);
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

                orderBy = string.IsNullOrEmpty(orderBy) ? "PointId,DateTime" : orderBy;
                string fieldName = "PointId,DateTime" + factorSql + factorDataFlagSql + factorAuditFlagSql + factorEQISql + factorGradeSql + wqSql;
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
            , DateTime dtStart, DateTime dtEnd, string LVTable, string orderBy = "PointId,DateTime")
        {
            tableName = LVTable;
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
                string where = "";
                if (tableName == "WaterReport.TB_DayReport_SZ")
                {
                    where = string.Format(" DateTime>='{0}' AND DateTime<='{1}' ", dtStart.AddHours(-12).ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.AddHours(-12).ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr;
                }
                else
                {
                    where = string.Format(" DateTime>='{0}' AND DateTime<='{1}' ", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr;
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
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetExportData(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, string orderBy = "PointId,DateTime")
        {
            tableName = "dbo.V_Water_DayReport";
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "PointId,DateTime";
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
                orderBy = string.IsNullOrEmpty(orderBy) ? "PointId,DateTime" : orderBy;
                string fieldName = "rowNum as '序号',PointId,portName,CONVERT(NVARCHAR(19),DateTime,120) as '日期'" + factorSql + factorFlagSql;
                string groupBy = "PointId,portName,DateTime";
                string where = "";
                if (tableName == "WaterReport.TB_DayReport_SZ")
                {
                    where = string.Format(" DateTime>='{0}' AND DateTime<='{1}' ", dtStart.AddHours(-12).ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.AddHours(-12).ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr;
                }
                else
                {
                    where = string.Format(" DateTime>='{0}' AND DateTime<='{1}' ", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr;
                }
                return g_GridViewPager.GetPivotDataPager(tableName, fieldName, groupBy, orderBy, where, pageSize, pageNo, connection, out recordTotal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataView GetExportData(string[] portIds, string[] factors
            , DateTime dtStart, DateTime dtEnd, string LVTable, string orderBy = "PointId,DateTime")
        {
            tableName = LVTable;
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "PointId,DateTime";
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
                orderBy = string.IsNullOrEmpty(orderBy) ? "PointId,DateTime" : orderBy;
                string fieldName = "rowNum as '序号',PointId,portName,CONVERT(NVARCHAR(19),DateTime,120) as '日期'" + factorSql + factorFlagSql;
                string groupBy = "PointId,portName,DateTime";
                string where = "";
                if (tableName == "WaterReport.TB_DayReport_SZ")
                {
                    where = string.Format(" DateTime>='{0}' AND DateTime<='{1}' ", dtStart.AddHours(-12).ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.AddHours(-12).ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr;
                }
                else
                {
                    where = string.Format(" DateTime>='{0}' AND DateTime<='{1}' ", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr;
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
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetExportDataReport_SZ(string[] portIds, IList<IPollutant> factors
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
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN CAST(dbo.F_Round(PollutantValue,{1}) AS DECIMAL(10,{1})) END ) AS [{0}] ", factor.PollutantCode);
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
                orderBy = string.IsNullOrEmpty(orderBy) ? "PointId,DateTime" : orderBy;
                string fieldName = "rowNum as '序号',PointId,CONVERT(NVARCHAR(10),DateTime,120) as 'Tstamp'" + factorSql + factorFlagSql;
                string groupBy = "PointId,DateTime";
                string where = string.Format(" DateTime>='{0}' AND DateTime<='{1}' ", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr;
                return g_GridViewPager.GetPivotDataPager("WaterReport.TB_DayReport_SZ", fieldName, groupBy, orderBy, where, pageSize, pageNo, connection, out recordTotal);
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
        public DataView GetExportNewDataReport_SZ(string[] portIds, IList<IPollutant> factors
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
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor.PollutantCode, decimalNum);
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
                orderBy = string.IsNullOrEmpty(orderBy) ? "PointId,DateTime" : orderBy;
                string fieldName = "rowNum as '序号',PointId,CONVERT(NVARCHAR(10),DateTime,120) as 'Tstamp'" + factorSql + factorFlagSql;
                string groupBy = "PointId,DateTime";
                string where = string.Format(" DateTime>='{0}' AND DateTime<='{1}' ", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr;
                return g_GridViewPager.GetPivotDataPager("WaterReport.TB_DayReport_SZ", fieldName, groupBy, orderBy, where, pageSize, pageNo, connection, out recordTotal);
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
        public DataView GetExportDataReport(string[] portIds, IList<IPollutant> factors
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
                    if (factor.PollutantCode == "w21003")
                    {
                        decimalNum = 2;
                    }
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
                orderBy = string.IsNullOrEmpty(orderBy) ? "PointId,DateTime" : orderBy;
                string fieldName = "rowNum as '序号',PointId,CONVERT(NVARCHAR(10),DateTime,120) as 'Tstamp'" + factorSql + factorFlagSql;
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
        /// 取得指定范围内的各指标的污染指数和水质等级
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="WQPollutants">所有参与统计的因子</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <returns></returns>
        public DataView GetWaterQualityData(string[] portIds, string[] WQPollutants, DateTime dateStart, DateTime dateEnd)
        {
            //站点处理
            string portIdsStr = string.Empty;
            if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
            {
                portIdsStr = " AND PointId =" + portIds[0];
            }
            else if (!string.IsNullOrEmpty(portIdsStr))
            {
                portIdsStr = "AND PointId IN(" + StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",") + ")";
            }
            string valueSql = string.Empty;
            string WQISql = string.Empty;
            string WQLSql = string.Empty;
            if (WQPollutants != null)
            {
                foreach (string pollutant in WQPollutants)
                {
                    valueSql += string.Format(",MAX(CASE WHEN  PollutantCode='{0}' THEN PollutantValue END) AS '{0}'", pollutant);
                    WQISql += string.Format(",MAX(CASE WHEN  PollutantCode='{0}' THEN dbo.F_GetWQI('{0}','7c67a857-d602-4f90-a26d-edd3e9f4d36c',PollutantValue,3,'d8197909-568e-4319-874c-3ad7cbc92a7e') END) AS 'WQI_{0}'", pollutant);
                    WQLSql += string.Format(",MAX(CASE WHEN  PollutantCode='{0}' THEN dbo.dbo.F_GetWQL('{0}','7c67a857-d602-4f90-a26d-edd3e9f4d36c',PollutantValue,'d8197909-568e-4319-874c-3ad7cbc92a7e','LEVEL') END) AS 'WQL_{0}'", pollutant);
                }
            }
            string sql = string.Format(@"
                SELECT PointId
	                ,DateTime
                    --浓度
                    {0}
	                --指数
	                {1}
	                --等级
	                {2}
                FROM {3}
                WHERE DateTime>='{4}' and DateTime<='{5}' {6}
                GROUP BY PointId,DateTime", valueSql, WQISql, WQLSql, tableName, dateStart.ToString("yyyy-MM-dd HH:mm:ss"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr);
            return g_DatabaseHelper.ExecuteDataView(sql, connection);
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
                string where = string.Format(" CONVERT(nvarchar(10),DateTime,120)>='{0}' AND CONVERT(nvarchar(10),DateTime,120)<='{1}' ", dateStart.ToString("yyyy-MM-dd"), dateEnd.ToString("yyyy-MM-dd")) + portIdsStr;

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
        /// 取得统计数据（最大值、最小值、平均值）
        /// </summary>
        /// <param name="autoMonitorType">数据类型</param>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetStatisticalDataNew(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd)
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
                string where = string.Format(" CONVERT(nvarchar(10),DateTime,120)>='{0}' AND CONVERT(nvarchar(10),DateTime,120)<='{1}' ", dateStart.ToString("yyyy-MM-dd"), dateEnd.ToString("yyyy-MM-dd")) + portIdsStr;

                //拼接统计数据字串，使用UNION方式减少统计条数，提升统计速度
                string sql = string.Empty;
                for (int iRow = 0; iRow < factors.Length; iRow++)
                {
                    if (iRow > 0)
                        sql += " UNION ";
                    if (factors[iRow] == "w21003")
                    {
                        sql += string.Format(@"
                        SELECT PointId,PollutantCode
	                        ,AVG(PollutantValue) AS Value_Avg
	                        ,MAX(PollutantValue) AS Value_Max
	                        ,MIN(PollutantValue) AS Value_Min
                            From(
                        SELECT PointId,PollutantCode='{1}'
	                        ,CONVERT(decimal(18, 2),dbo.F_Round(PollutantValue,2) )AS PollutantValue
                        FROM {0}
                        WHERE {2}
                            AND PollutantCode='{1}'
                        GROUP BY PointId,PollutantCode,PollutantValue) as a
                         group by PointId,PollutantCode
                    ", tableName, factors[iRow], where);
                    }
                    else
                    {
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

                g_DBBiz.ExecuteProcNonQuery("UP_WaterReport_Hour_Port_Mul", connection);
            }
            catch (Exception ex)
            {
                //return null;
                throw ex;
            }
        }
        /// <summary>
        /// 生成点位小时数据
        /// </summary>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="portIds">站点列表</param>
        public void ExportDataNew(DateTime dateStart, DateTime dateEnd, string[] portIds)
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

                g_DBBiz.ExecuteProcNonQuery("UP_WaterReportNew_Hour_Port_Mul", connection);
            }
            catch (Exception ex)
            {
                //return null;
                throw ex;
            }
        }
        /// <summary>
        /// 获取日均值数据报表
        /// </summary>
        /// <param name="pointId">点位id</param>
        /// <param name="beginTime">监测起始时间</param>
        /// <param name="endTime">监测截止时间</param>
        /// <returns></returns>
        public DataView GetAVGDayReport(string pointId, DateTime beginTime, DateTime endTime)
        {
            string sql = @"select datetime,p.monitoringpointname
                        ,CAST (MAX(CASE(PollutantCode) WHEN 'w01001' THEN PollutantValue END ) as decimal(18,2)) AS [pH]
                        ,CAST (MAX(CASE(PollutantCode) WHEN 'w01010' THEN PollutantValue END ) as decimal(18,1)) AS [T]
                        ,CAST (MAX(CASE(PollutantCode) WHEN 'w01003' THEN PollutantValue END ) as decimal(18,1)) AS [TB]
                        ,CAST (MAX(CASE(PollutantCode) WHEN 'w01009' THEN PollutantValue END ) as decimal(18,2)) AS [DO]
                        ,CAST (MAX(CASE(PollutantCode) WHEN 'w01014' THEN PollutantValue END ) as decimal(18,0)) AS [EC]
                        ,CAST (MAX(CASE(PollutantCode) WHEN 'w01019' THEN PollutantValue END ) as decimal(18,1)) AS [CODMn]
                        ,CAST (MAX(CASE(PollutantCode) WHEN 'w01020' THEN PollutantValue END ) as decimal(18,2)) AS [TOC]
                        ,CAST (MAX(CASE(PollutantCode) WHEN 'w21003' THEN PollutantValue END ) as decimal(18,2)) AS [NH3_N]
                        ,CAST (MAX(CASE(PollutantCode) WHEN 'w21011' THEN PollutantValue END ) as decimal(18,3)) AS [TP]
                        ,CAST (MAX(CASE(PollutantCode) WHEN 'w21001' THEN PollutantValue END ) as decimal(18,2)) AS [TN]
                        from WaterReport.TB_DayReport as d
                        left join dbo.SY_MonitoringPoint as p on p.pointId = d.PointId
                         where 1=1";
            if (pointId != null && pointId != "")
            {
                sql += " and d.PointId in (" + pointId + ")";
            }

            if (beginTime != null)
            {
                sql += " and datetime >= '" + beginTime + "'";
            }
            if (endTime != null)
            {
                sql += " and datetime <= '" + endTime + "'";
            }
            sql += " group by p.monitoringpointname,datetime  order by p.monitoringpointname,datetime";

            DataView dv = new DataView();
            return g_DatabaseHelper.ExecuteDataView(sql, connection);

        }

        /// <summary>
        /// 获取水质周报数据源
        /// </summary>
        /// <param name="pointId">点位id</param>
        /// <param name="beginTime">监测起始时间</param>
        /// <param name="endTime">监测截止时间</param>
        /// <returns></returns>
        public DataView GetWeekReport(string pointId, DateTime beginTime, DateTime endTime)
        {
            string sql = @"select p.pointId,datetime,p.monitoringpointname,Valley.ItemText as watersname,s.NAME AS Station
                        ,MAX(CASE(PollutantCode) WHEN 'w01001' THEN PollutantValue END ) AS w01001
                        ,MAX(CASE(PollutantCode) WHEN 'w01009' THEN PollutantValue END ) AS w01009
                        ,MAX(CASE(PollutantCode) WHEN 'w01019' THEN PollutantValue END ) AS w01019
                        ,MAX(CASE(PollutantCode) WHEN 'w21003' THEN PollutantValue END ) AS w21003
                        ,MAX(CASE(PollutantCode) WHEN 'w21011' THEN PollutantValue END ) AS w21011
                        ,MAX(CASE(PollutantCode) WHEN 'w21001' THEN PollutantValue END ) AS w21001
                        from WaterReport.TB_DayReport as d
                        left join dbo.SY_MonitoringPoint as p on p.pointId = d.PointId
                        left JOIN dbo.SY_MonitoringStation AS s
							ON p.StationUid = s.StationUid
                        left join dbo.SY_MonitoringPointExtensionForEQMSWater as w 
                            on w.monitoringpointuid = p.monitoringpointuid
                        left join dbo.SY_View_CodeMainItem as Valley
                            on w.Valley = Valley.ItemGuid
                         where 1=1";
            if (pointId != null && pointId != "")
            {
                sql += " and d.PointId in (" + pointId + ")";
            }

            if (beginTime != null)
            {
                sql += " and datetime >= '" + beginTime + "'";
            }
            if (endTime != null)
            {
                sql += " and datetime <= '" + endTime + "'";
            }
            sql += " group by p.pointId,w.rscode,p.monitoringpointname,Valley.ItemText,s.NAME,datetime  order by w.rscode,datetime--order by p.monitoringpointname,datetime";

            DataView dv = new DataView();
            return g_DatabaseHelper.ExecuteDataView(sql, connection);
        }

        /// <summary>
        /// 获取首要污染物
        /// </summary>
        /// <returns></returns>
        public string GetPrimaryPollutant(string pointid, string PH, string NH3, string CODmn, string DO, string TP, string TN)
        {
            string pointCalWQType = "d8197909-568e-4319-874c-3ad7cbc92a7e";//地表水水质评价站点属性类型（河流）
            string hourTypeUid = "7c67a857-d602-4f90-a26d-edd3e9f4d36c";//时间类型

            string sql = string.Format(@"select dbo.F_GetWQL_Max('NAME','w01001,w01009,w01019,w21003'
                                         ,dbo.F_GetWQL('w01001','{0}',{2},'{1}','LEVEL')
                                         ,dbo.F_GetWQL('w21003','{0}',{3},'{1}','LEVEL')
                                         ,dbo.F_GetWQL('w01019','{0}',{4},'{1}','LEVEL')
                                         ,dbo.F_GetWQL('w01009','{0}',{5},'{1}','LEVEL')
                                         ,dbo.F_GetWQL('w21011','{0}',{6},'{1}','LEVEL')
                                         ,dbo.F_GetWQL('w21001','{0}',{7},'{1}','LEVEL')
                                         ,dbo.F_GetWQL('w01018','{0}',null,'{1}','LEVEL')
                                         ,dbo.F_GetWQL('w01017','{0}',null,'{1}','LEVEL')
                                         ,dbo.F_GetWQL('w20122','{0}',null,'{1}','LEVEL')
                                         ,dbo.F_GetWQL('w20123','{0}',null,'{1}','LEVEL')
                                         ,dbo.F_GetWQL('w21017','{0}',null,'{1}','LEVEL')
                                         ,dbo.F_GetWQL('w20128','{0}',null,'{1}','LEVEL')
                                         ,dbo.F_GetWQL('w20119','{0}',null,'{1}','LEVEL')
                                         ,dbo.F_GetWQL('w20111','{0}',null,'{1}','LEVEL')
                                         ,dbo.F_GetWQL('w20115','{0}',null,'{1}','LEVEL')
                                         ,dbo.F_GetWQL('w20117','{0}',null,'{1}','LEVEL')
                                         ,dbo.F_GetWQL('w20120','{0}',null,'{1}','LEVEL')
                                         ,dbo.F_GetWQL('w21016','{0}',null,'{1}','LEVEL')
                                         ,dbo.F_GetWQL('w23002','{0}',null,'{1}','LEVEL')
                                         ,dbo.F_GetWQL('w21019','{0}',null,'{1}','LEVEL')
                                         ,dbo.F_GetWQL('w22001','{0}',null,'{1}','LEVEL')
                                         ,dbo.F_GetWQL('w19002','{0}',null,'{1}','LEVEL')
                                         ,dbo.F_GetWQL('w02003','{0}',null,'{1}','LEVEL')) as PrimaryPollutant"
, hourTypeUid, pointCalWQType, PH, NH3, CODmn, DO, TP, TN);
            DataView dv = g_DatabaseHelper.ExecuteDataView(sql, connection);
            return dv[0]["PrimaryPollutant"].ToString();
        }

        /// <summary>
        /// 获取水质类别
        /// </summary>
        /// <param name="pointId">点位id</param>
        /// <param name="beginTime">监测起始时间</param>
        /// <param name="endTime">监测截止时间</param>
        /// <returns></returns>
        public string GetLevel(string pointid, string PH, string NH3, string CODmn, string DO, string TP, string TN)
        {
            string pointCalWQType = "d8197909-568e-4319-874c-3ad7cbc92a7e";//地表水水质评价站点属性类型（河流）
            string hourTypeUid = "7c67a857-d602-4f90-a26d-edd3e9f4d36c";//时间类型

            string sql = string.Format(@"select dbo.F_GetWQL_Max('ROMAN','w01001,w01009,w01019,w21003'
                                         ,dbo.F_GetWQL('w01001','{0}',{2},'{1}','LEVEL')
                                         ,dbo.F_GetWQL('w21003','{0}',{3},'{1}','LEVEL')
                                         ,dbo.F_GetWQL('w01019','{0}',{4},'{1}','LEVEL')
                                         ,dbo.F_GetWQL('w01009','{0}',{5},'{1}','LEVEL')
                                         ,dbo.F_GetWQL('w21011','{0}',{6},'{1}','LEVEL')
                                         ,dbo.F_GetWQL('w21001','{0}',{7},'{1}','LEVEL')
                                         ,dbo.F_GetWQL('w01018','{0}',null,'{1}','LEVEL')
                                         ,dbo.F_GetWQL('w01017','{0}',null,'{1}','LEVEL')
                                         ,dbo.F_GetWQL('w20122','{0}',null,'{1}','LEVEL')
                                         ,dbo.F_GetWQL('w20123','{0}',null,'{1}','LEVEL')
                                         ,dbo.F_GetWQL('w21017','{0}',null,'{1}','LEVEL')
                                         ,dbo.F_GetWQL('w20128','{0}',null,'{1}','LEVEL')
                                         ,dbo.F_GetWQL('w20119','{0}',null,'{1}','LEVEL')
                                         ,dbo.F_GetWQL('w20111','{0}',null,'{1}','LEVEL')
                                         ,dbo.F_GetWQL('w20115','{0}',null,'{1}','LEVEL')
                                         ,dbo.F_GetWQL('w20117','{0}',null,'{1}','LEVEL')
                                         ,dbo.F_GetWQL('w20120','{0}',null,'{1}','LEVEL')
                                         ,dbo.F_GetWQL('w21016','{0}',null,'{1}','LEVEL')
                                         ,dbo.F_GetWQL('w23002','{0}',null,'{1}','LEVEL')
                                         ,dbo.F_GetWQL('w21019','{0}',null,'{1}','LEVEL')
                                         ,dbo.F_GetWQL('w22001','{0}',null,'{1}','LEVEL')
                                         ,dbo.F_GetWQL('w19002','{0}',null,'{1}','LEVEL')
                                         ,dbo.F_GetWQL('w02003','{0}',null,'{1}','LEVEL')) as Level"
, hourTypeUid, pointCalWQType, PH, NH3, CODmn, DO, TP, TN);
            DataView dv = g_DatabaseHelper.ExecuteDataView(sql, connection);
            return dv[0]["Level"].ToString();
        }


        /// <summary>
        /// 获取水质监测例行成果表数据源
        /// </summary>
        /// <returns></returns>
        public DataView GetWaterMonthMoutineList(string pointId, DateTime beginTime, DateTime endTime)
        {
            string sql = @"select p.pointId,datetime,p.monitoringpointname
                        ,Cast (MAX(CASE(PollutantCode) WHEN 'w01001' THEN PollutantValue END ) as decimal(18,2))AS PH
                        ,Cast (MAX(CASE(PollutantCode) WHEN 'w01010' THEN PollutantValue END ) as decimal(18,1))AS SW
                        ,Cast (MAX(CASE(PollutantCode) WHEN 'w01003' THEN PollutantValue END ) as decimal(18,1))AS ZD
                        ,Cast (MAX(CASE(PollutantCode) WHEN 'w01009' THEN PollutantValue END ) as decimal(18,2))AS RJY
                        ,Cast (MAX(CASE(PollutantCode) WHEN 'w01014' THEN PollutantValue END ) as decimal(18,0))AS DDL
                        ,Cast (MAX(CASE(PollutantCode) WHEN 'w01019' THEN PollutantValue END ) as decimal(18,2))AS CODmn
                        ,Cast (MAX(CASE(PollutantCode) WHEN 'w01020' THEN PollutantValue END ) as decimal(18,2))AS TC
                        ,Cast (MAX(CASE(PollutantCode) WHEN 'w21003' THEN PollutantValue END ) as decimal(18,2))AS NH3
                        ,Cast (MAX(CASE(PollutantCode) WHEN 'w21011' THEN PollutantValue END ) as decimal(18,3))AS TP
                        ,Cast (MAX(CASE(PollutantCode) WHEN 'w21001' THEN PollutantValue END ) as decimal(18,2))AS TN
                        ,Cast (MAX(CASE(PollutantCode) WHEN 'w01016' THEN PollutantValue END ) as decimal(18,4))AS YLS
                        ,Cast (MAX(CASE(PollutantCode) WHEN 'w19011' THEN PollutantValue END ) as decimal(18,1))AS LLZ
                        from WaterReport.TB_DayReport as d
                        left join dbo.SY_MonitoringPoint as p on p.pointId = d.PointId
                         where 1=1";
            if (pointId != null && pointId != "")
            {
                sql += " and d.PointId in (" + pointId + ")";
            }

            if (beginTime != null)
            {
                sql += " and datetime >= '" + beginTime + "'";
            }
            if (endTime != null)
            {
                sql += " and datetime <= '" + endTime + "'";
            }
            sql += " group by p.pointId,p.monitoringpointname,datetime  order by p.monitoringpointname,datetime";

            DataView dv = new DataView();
            return g_DatabaseHelper.ExecuteDataView(sql, connection);

        }

        /// <summary>
        /// 获取沙渚锡东周均值表数据源
        /// </summary>
        /// <returns></returns>
        public DataView GetSZXDWeekAvgData(string pointIds, DateTime beginTime, DateTime endTime)
        {
            string TimeRange = string.Empty;
            TimeRange = beginTime.ToString("yyyy年MM月dd日") + "-" + endTime.AddDays(-1).ToString("yyyy年MM月dd日");
            string str = string.Format(@"'{0}' as TimeRange,datetime,
                        Cast (AVG(CASE(PollutantCode) WHEN 'w01001' THEN PollutantValue END ) as decimal(18,2))AS PH
                        ,Cast (AVG(CASE(PollutantCode) WHEN 'w01010' THEN PollutantValue END ) as decimal(18,1))AS SW
                        ,Cast (AVG(CASE(PollutantCode) WHEN 'w01004' THEN PollutantValue END ) as decimal(18,2))AS TMD
                        ,Cast (AVG(CASE(PollutantCode) WHEN 'w01003' THEN PollutantValue END ) as decimal(18,1))AS ZD
                        ,Cast (AVG(CASE(PollutantCode) WHEN 'w01009' THEN PollutantValue END ) as decimal(18,2))AS RJY
                        ,Cast (AVG(CASE(PollutantCode) WHEN 'w01014' THEN PollutantValue END ) as decimal(18,0))AS DDL
                        ,Cast (AVG(CASE(PollutantCode) WHEN 'w01019' THEN PollutantValue END ) as decimal(18,1))AS CODmn
                        ,Cast (AVG(CASE(PollutantCode) WHEN 'w01020' THEN PollutantValue END ) as decimal(18,2))AS TC
                        ,Cast (AVG(CASE(PollutantCode) WHEN 'w21003' THEN PollutantValue END ) as decimal(18,2))AS NH3
                        ,Cast (AVG(CASE(PollutantCode) WHEN 'w21011' THEN PollutantValue END ) as decimal(18,3))AS TP
                        ,Cast (AVG(CASE(PollutantCode) WHEN 'w21001' THEN PollutantValue END ) as decimal(18,2))AS TN
                        ,Cast (AVG(CASE(PollutantCode) WHEN 'w01016' THEN PollutantValue END ) as decimal(18,4))AS YLS
                        ,Cast (AVG(CASE(PollutantCode) WHEN 'w19011' THEN PollutantValue END ) as decimal(18,1))AS LLZ", TimeRange);
            string sql = "";
            foreach (string pointId in pointIds.Split(';'))
            {
                if (sql != "") sql += " union ";
                sql += string.Format(@"select p.monitoringpointname as portName, {0} from WaterReport.TB_DayReport as d left join dbo.SY_MonitoringPoint as p on p.pointId = d.PointId where d.PointId in ({1}) and datetime >= '{2}' and datetime <= '{3}' group by p.pointId,p.monitoringpointname,datetime ", str, pointId, beginTime, endTime);
            }
            return g_DatabaseHelper.ExecuteDataView(sql, connection);
        }

        /// <summary>
        /// 获取水质月报数据源
        /// </summary>
        /// <returns></returns>
        public DataView GetRep_MonthSelfDataSouce(string pointid, DateTime beginTime, DateTime endTime)
        {
            string sql = string.Format(@"SELECT Convert(varchar(10),d.datetime,120) AS '日期', SUBSTRING(DATENAME(weekday, d.datetime), 3, 1) AS '星期', 
                                      p.monitoringPointName AS '站点'
                                      , MAX(CASE(PollutantCode) WHEN 'w01010' THEN PollutantValue END ) AS 水温, 
                                      MAX(CASE(PollutantCode) WHEN 'w01001' THEN PollutantValue END ) AS PH, 
                                      MAX(CASE(PollutantCode) WHEN 'w01009' THEN PollutantValue END ) AS 溶解氧, 
                                      MAX(CASE(PollutantCode) WHEN 'w01014' THEN PollutantValue END ) AS 电导率, 
                                      MAX(CASE(PollutantCode) WHEN 'w01003' THEN PollutantValue END ) AS 浊度, 
                                      MAX(CASE(PollutantCode) WHEN 'w01019' THEN PollutantValue END ) AS 高锰酸盐指数, 
                                      MAX(CASE(PollutantCode) WHEN 'w21003' THEN PollutantValue END ) AS 氨氮, 
                                      MAX(CASE(PollutantCode) WHEN 'w21011' THEN PollutantValue END ) AS 总磷, 
                                      MAX(CASE(PollutantCode) WHEN 'w21001' THEN PollutantValue END ) AS 总氮, 
                                      MAX(CASE(PollutantCode) WHEN 'w01020' THEN PollutantValue END ) AS 总有机碳
                                FROM WaterReport.TB_DayReport AS d INNER JOIN
                                      dbo.SY_MonitoringPoint as p on p.pointId = d.PointId 
                                WHERE (d.datetime >= CONVERT(NVARCHAR(10), '{1}', 120)) AND 
                                      (d.datetime < CONVERT(NVARCHAR(10), '{2}', 120))
                                        AND p.pointId = {0}
                                group by p.monitoringpointname,datetime  order by p.monitoringpointname,datetime", pointid, beginTime, endTime);
            return g_DatabaseHelper.ExecuteDataView(sql, connection);
        }

        /// <summary>
        /// 获取浮标水域月报数据源
        /// </summary>
        /// <returns></returns>
        public DataView GetBuoyWaterAreaMonthReportSoucrce(string pointid, DateTime beginTime, DateTime endTime)
        {
            string sql = string.Format(@"SELECT 
                                        Valley.ItemText as watersname
                                        ,p.monitoringpointname as portname
                                        ,p.pointId as portId
                                        ,datepart(MONTH,datetime) as m1
                                        ,datepart(DAY,datetime) as d1
                                        ,datetime
                                      , MAX(CASE(PollutantCode) WHEN 'w01010' THEN PollutantValue END ) AS SW, 
                                      MAX(CASE(PollutantCode) WHEN 'w01001' THEN PollutantValue END ) AS PH, 
                                      MAX(CASE(PollutantCode) WHEN 'w01009' THEN PollutantValue END ) AS RJY, 
                                      MAX(CASE(PollutantCode) WHEN 'w01014' THEN PollutantValue END ) AS DDL, 
                                      MAX(CASE(PollutantCode) WHEN 'w01003' THEN PollutantValue END ) AS ZD, 
                                      MAX(CASE(PollutantCode) WHEN 'w01019' THEN PollutantValue END ) AS CODmn, 
                                      MAX(CASE(PollutantCode) WHEN 'w21003' THEN PollutantValue END ) AS NH3, 
                                      MAX(CASE(PollutantCode) WHEN 'w21011' THEN PollutantValue END ) AS TP, 
                                      MAX(CASE(PollutantCode) WHEN 'w21001' THEN PollutantValue END ) AS TN, 
                                      MAX(CASE(PollutantCode) WHEN 'w01016' THEN PollutantValue END ) AS YLS,
                                      MAX(CASE(PollutantCode) WHEN 'w19011' THEN PollutantValue END ) AS ZMD
                                FROM WaterReport.TB_DayReport AS d 
                                INNER JOIN dbo.SY_MonitoringPoint as p on p.pointId = d.PointId 
                                INNER JOIN dbo.SY_MonitoringPointExtensionForEQMSWater as w on w.monitoringpointuid = p.monitoringpointuid
                                left join dbo.SY_View_CodeMainItem as Valley
                                    on w.Valley = Valley.ItemGuid
                                      WHERE  p.pointId in ({0})
                                AND d.datetime >= '{1}' AND 
                                    d.datetime < '{2}'
                                group by p.monitoringpointname,datetime,Valley.ItemText,p.PointId  order by p.monitoringpointname,datetime", pointid, beginTime, endTime);
            return g_DatabaseHelper.ExecuteDataView(sql, connection);
        }

        /// <summary>
        /// 获取太湖引用水源地水质监测结果表数据源
        /// </summary>
        /// <returns></returns>
        public DataView GetTHDrinkWaterMonitoringReportDataSouce(string pointid, DateTime beginTime, DateTime endTime)
        {
            string sql = string.Format(@"SELECT 
                                        p.monitoringpointname as portname
                                        ,p.pointId as portId
                                      , Cast (MAX(CASE(PollutantCode) WHEN 'w01010' THEN PollutantValue END ) as decimal(18,1)) AS SW, 
                                      Cast (MAX(CASE(PollutantCode) WHEN 'w01001' THEN PollutantValue END ) as decimal(18,2)) AS PH, 
                                      Cast (MAX(CASE(PollutantCode) WHEN 'w01009' THEN PollutantValue END ) as decimal(18,2)) AS RJY, 
                                      Cast (MAX(CASE(PollutantCode) WHEN 'w01004' THEN PollutantValue END ) as decimal(18,2)) AS TMD, 
                                      Cast (MAX(CASE(PollutantCode) WHEN 'w01019' THEN PollutantValue END ) as decimal(18,1)) AS CODmn, 
                                      Cast (MAX(CASE(PollutantCode) WHEN 'w21003' THEN PollutantValue END ) as decimal(18,2)) AS NH3, 
                                      Cast (MAX(CASE(PollutantCode) WHEN 'w21011' THEN PollutantValue END ) as decimal(18,3)) AS TP, 
                                      Cast (MAX(CASE(PollutantCode) WHEN 'w21001' THEN PollutantValue END ) as decimal(18,2)) AS TN, 
                                      Cast (MAX(CASE(PollutantCode) WHEN 'w01016' THEN PollutantValue END ) as decimal(18,4)) AS YLS,
                                      Cast (MAX(CASE(PollutantCode) WHEN 'w19011' THEN PollutantValue END ) as decimal(18,1)) AS ZMD
                                FROM WaterReport.TB_DayReport AS d INNER JOIN
                                      dbo.SY_MonitoringPoint as p on p.pointId = d.PointId 
                                      WHERE  p.pointId in ({0})
                                AND d.datetime >= '{1}' AND 
                                    d.datetime < '{2}'
                                group by p.monitoringpointname,p.PointId  order by p.monitoringpointname", pointid, beginTime, endTime);
            return g_DatabaseHelper.ExecuteDataView(sql, connection);
        }

        /// <summary>
        /// 获取重点断面水质周报数据源
        /// </summary>
        /// <returns></returns>
        public DataView GetWatershedsWQWeekReportDataSouce(string pointid, DateTime beginTime, DateTime endTime)
        {
            string sql = string.Format(@"SELECT Convert(varchar(10),d.datetime,120) AS '日期', SUBSTRING(DATENAME(weekday, d.datetime), 3, 1) AS '星期', 
                                      p.monitoringPointName AS '站点'
                                      , MAX(CASE(PollutantCode) WHEN 'w01010' THEN PollutantValue END ) AS 水温, 
                                      MAX(CASE(PollutantCode) WHEN 'w01001' THEN PollutantValue END ) AS PH, 
                                      MAX(CASE(PollutantCode) WHEN 'w01009' THEN PollutantValue END ) AS 溶解氧, 
                                      MAX(CASE(PollutantCode) WHEN 'w01014' THEN PollutantValue END ) AS 电导率, 
                                      MAX(CASE(PollutantCode) WHEN 'w01003' THEN PollutantValue END ) AS 浊度, 
                                      MAX(CASE(PollutantCode) WHEN 'w01019' THEN PollutantValue END ) AS 高锰酸盐指数, 
                                      MAX(CASE(PollutantCode) WHEN 'w21003' THEN PollutantValue END ) AS 氨氮, 
                                      MAX(CASE(PollutantCode) WHEN 'w21011' THEN PollutantValue END ) AS 总磷, 
                                      MAX(CASE(PollutantCode) WHEN 'w21001' THEN PollutantValue END ) AS 总氮, 
                                      MAX(CASE(PollutantCode) WHEN 'w01020' THEN PollutantValue END ) AS 总有机碳
                                FROM WaterReport.TB_DayReport AS d INNER JOIN
                                      dbo.SY_MonitoringPoint as p on p.pointId = d.PointId 
                                WHERE (d.datetime >= CONVERT(NVARCHAR(10), '{1}', 120)) AND 
                                      (d.datetime < CONVERT(NVARCHAR(10), '{2}', 120))
                                         AND p.pointId = {0}
                                group by p.monitoringpointname,datetime  order by p.monitoringpointname,datetime", pointid, beginTime.ToShortDateString(), endTime.ToShortDateString());
            return g_DatabaseHelper.ExecuteDataView(sql, connection);
        }

        /// <summary>
        /// 获取重点断面水质周报--水质评价数据源
        /// </summary>
        /// <returns></returns>
        public DataView GetWatershedsWQDataSouce(string pointid, DateTime beginTime, DateTime endTime)
        {
            string sql = string.Format(@"SELECT p.monitoringPointName AS '站点',
                                      AVG(CASE(PollutantCode) WHEN 'w01001' THEN PollutantValue END ) AS PH, 
                                      AVG(CASE(PollutantCode) WHEN 'w01009' THEN PollutantValue END ) AS 溶解氧, 
                                      AVG(CASE(PollutantCode) WHEN 'w01019' THEN PollutantValue END ) AS 高锰酸盐指数, 
                                      AVG(CASE(PollutantCode) WHEN 'w21003' THEN PollutantValue END ) AS 氨氮 
                                    FROM WaterReport.TB_DayReport AS d INNER JOIN
                                      dbo.SY_MonitoringPoint as p on p.pointId = d.PointId 
                                WHERE (d.datetime >= CONVERT(NVARCHAR(10), '{1}', 120)) AND 
                                      (d.datetime < CONVERT(NVARCHAR(10), '{2}', 120))
                                        AND p.pointId = {0}
                                group by p.monitoringpointname ", pointid, beginTime.ToShortDateString(), endTime.ToShortDateString());
            return g_DatabaseHelper.ExecuteDataView(sql, connection);
        }

        /// <summary>
        /// 获取区域水质状况
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetRegionWQData(string[] portIds, IList<IPollutant> factors
          , DateTime dtStart, DateTime dtEnd, string orderBy = "DateTime")
        {
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "DateTime";
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string avgSql = string.Empty;
                string WQISql = string.Empty;
                string SumSql = string.Empty;
                string CountSql = string.Empty;
                foreach (IPollutant factor in factors)
                {
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor.PollutantCode);
                    avgSql += string.Format(",CONVERT(numeric(18,{1}),dbo.F_Round(AVG({0}),{1})) AS [{0}] ", factor.PollutantCode, factor.PollutantDecimalNum);
                    WQISql += string.Format(",dbo.F_GetWQI('{0}','7c67a857-d602-4f90-a26d-edd3e9f4d36c',{0},3,'d8197909-568e-4319-874c-3ad7cbc92a7e') AS '{0}_EQI'", factor.PollutantCode, factor.PollutantDecimalNum);
                    SumSql += string.Format("ISNULL({0},0)+", factor.PollutantCode);
                    CountSql += string.Format("(CASE WHEN ISNULL({0},0) >0 THEN 1.0000 ELSE 0 END) +", factor.PollutantCode);
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
                string sql = string.Format(@"WITH dayData AS
                                    (
                                        SELECT PointId,[DateTime] {0} FROM {1}
                                        WHERE [DateTime]>='{2}' AND [DateTime]<='{3}' {4}
                                        Group by PointId,DateTime
                                    ) ", factorSql, tableName, dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr);
                sql += string.Format(@"SELECT * {1} {2} FROM 
                                       (
                                       SELECT [DateTime] {0}  
                                       FROM dayData 
                                       Group by [DateTime]) AS A
                                       ORDER BY A.[DateTime]", avgSql, WQISql, ",dbo.[F_Round](" + SumSql.Trim('+') + "/" + CountSql.Trim('+') + ",4) AS EQI");
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 水质自动监测月度小结，获取最大藻密度值与点位信息
        /// </summary>
        /// <param name="datetime">日期</param>
        /// <param name="PointIds">监测点ID数组</param>
        /// <param name="PointIds">监测因子Code数组</param>
        /// <param name="SiteTypeUids">监测点类型Uid数组</param>
        /// <param name="isSiteType">是否监测点类型</param>
        /// <returns></returns>
        public DataTable GetMaxPollutantValue(DateTime datetime, List<int> PointIds, List<string> PollutantCodes, List<string> SiteTypeUids, bool isSiteType)
        {
            if (PointIds.Count == 0 || PollutantCodes.Count == 0)
            {
                return null;
            }
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select p.monitoringPointName,d.PointId,d.DateTime ");
            foreach (string code in PollutantCodes)
            {
                strSql.Append(",MAX(case(PollutantCode) when '" + code + "' then CAST(PollutantValue as decimal(18,4)) end ) as '" + code + "' ");
            }
            strSql.Append("from WaterReport.TB_DayReport as d ");
            strSql.Append("inner join dbo.SY_MonitoringPoint as p on p.pointId = d.PointId ");
            strSql.Append("where d.DateTime >= Convert(datetime,'" + datetime.ToString("yyyy-MM-dd") + "') ");
            strSql.Append("and d.DateTime < Convert(datetime,'" + datetime.AddMonths(1).ToString("yyyy-MM-dd") + "') ");
            if (PointIds.Count == 1)
            {
                strSql.Append("and d.PointId='" + PointIds[0] + "' ");
            }
            else
            {
                strSql.Append("and (");
                for (int i = 0; i < PointIds.Count; i++)
                {
                    if (i == 0)
                    {
                        strSql.Append(" d.PointId='" + PointIds[i] + "' ");
                    }
                    else
                    {
                        strSql.Append(" or d.PointId='" + PointIds[i] + "' ");
                    }
                }
                strSql.Append(")");
            }
            if (isSiteType)
            {
                if (SiteTypeUids.Count > 0)
                {
                    if (SiteTypeUids.Count == 1)
                    {
                        strSql.Append("and p.sitetypeUid='" + SiteTypeUids[0] + "' ");
                    }
                    else
                    {
                        strSql.Append("and (");
                        for (int i = 0; i < SiteTypeUids.Count; i++)
                        {
                            if (i == 0)
                            {
                                strSql.Append(" p.sitetypeUid='" + SiteTypeUids[i] + "' ");
                            }
                            else
                            {
                                strSql.Append(" or p.sitetypeUid='" + SiteTypeUids[i] + "' ");
                            }
                        }
                        strSql.Append(")");
                    }
                }
            }
            else
            {
                foreach (string SiteTypeUid in SiteTypeUids)
                {
                    strSql.Append("and p.sitetypeUid!='" + SiteTypeUid + "' ");
                }
            }
            strSql.Append("group by p.monitoringPointName,d.PointId,d.DateTime ");
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), connection);
        }

        /// <summary>
        /// 取得无锡水质周报所需日数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataTable GetDataForWeekReport(string[] portIds, IList<IPollutant> factors, DateTime dtStart, DateTime dtEnd)
        {
            string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
            string factorSql = string.Empty;
            foreach (IPollutant factor in factors)
            {
                Int32 decimalNum = string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum);
                factorSql += string.Format(",ISNULL(MAX(CASE(PollutantCode) WHEN '{0}' THEN CAST(dbo.F_Round(PollutantValue,{1}) AS DECIMAL(10,{1})) END ),-1) AS [{0}] ", factor.PollutantCode.Trim(), decimalNum);
            }
            string sql = string.Format(@"
                select datetime
	                ,w.stcode
	                ,w.stname
	                ,w.rscode
	                ,w.rsname
	                ,p.monitoringpointname
	                ,DATEPART(YEAR,DateTime) AS ye
	                ,DATEPART(month,DateTime) AS mon
	                ,DATEPART(day,DateTime) AS DAY
	                ,dbo.F_GetCurWeekNum(DateTime,'WEEK') AS week_id
                    ,dbo.F_GetDayOfWeekName(DateTime,1) as week
                    {0}
                from WaterReport.TB_DayReport as d
                left join dbo.SY_MonitoringPoint as p on p.pointId = d.PointId
                INNER join dbo.SY_MonitoringPointExtensionForEQMSWater as w on w.monitoringpointuid = p.monitoringpointuid
                left join dbo.SY_View_CodeMainItem as Valley on w.Valley = Valley.ItemGuid
                WHERE DateTime>='{1}' AND DateTime<='{2}' and d.PointId IN ({3})
                group by p.pointId,w.stcode,w.stname,w.rscode,w.rsname,p.monitoringpointname,Valley.ItemText,datetime
                order by w.rscode,datetime ", factorSql, dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr);
            return g_DatabaseHelper.ExecuteDataTable(sql, connection);
        }

        /// <summary>
        /// 取得无锡水质周报所需VOC日数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataTable GetVOCDataForWeekReport(string[] portIds, IList<IPollutant> factors, DateTime dtStart, DateTime dtEnd)
        {
            string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
            string factorSql = string.Empty;
            string factorZhiXianStr = System.Configuration.ConfigurationManager.AppSettings["WaterWeekReprt_DayVOCZhiXian"].ToString();
            string[] factorZhiXians = factorZhiXianStr.Split(';');
            foreach (IPollutant factor in factors)
            {
                string factorZhiXian = factorZhiXians.FirstOrDefault(x => x.IndexOf(factor.PollutantCode) >= 0);
                Int32 decimalNum = string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum);
                if (string.IsNullOrEmpty(factorZhiXian))
                {
                    factorSql += string.Format(",ISNULL(MAX(CASE(PollutantCode) WHEN '{0}' THEN dbo.F_Round(PollutantValue,{1}) END ),'未检出') AS [{0}] ", factor.PollutantCode.Trim(), decimalNum);
                }
                else
                {
                    string zhixian = factorZhiXian.Split(',').Length > 1 ? factorZhiXian.Split(',')[1] : "0.00";
                    factorSql += string.Format(",ISNULL(MAX(CASE WHEN PollutantCode='{0}' AND PollutantValue>={2} THEN dbo.F_Round(PollutantValue,{1}) END ),'未检出') AS [{0}] ", factor.PollutantCode.Trim(), decimalNum, zhixian);
                }
            }
            string sql = string.Format(@"
                select datetime
	                ,w.stcode
	                ,w.stname
	                ,w.rscode
	                ,w.rsname
	                ,p.monitoringpointname
	                ,DATEPART(YEAR,DateTime) AS ye
	                ,DATEPART(month,DateTime) AS mon
	                ,DATEPART(day,DateTime) AS DAY
	                ,dbo.F_GetCurWeekNum(DateTime,'WEEK') AS week_id
                    ,dbo.F_GetDayOfWeekName(DateTime,1) as week
                    {0}
                from WaterReport.TB_DayReport as d
                left join dbo.SY_MonitoringPoint as p on p.pointId = d.PointId
                INNER join dbo.SY_MonitoringPointExtensionForEQMSWater as w on w.monitoringpointuid = p.monitoringpointuid
                left join dbo.SY_View_CodeMainItem as Valley on w.Valley = Valley.ItemGuid
                WHERE DateTime>='{1}' AND DateTime<='{2}' and d.PointId IN ({3})
                group by p.pointId,w.stcode,w.stname,w.rscode,w.rsname,p.monitoringpointname,Valley.ItemText,datetime
                order by w.rscode,datetime ", factorSql, dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr);
            return g_DatabaseHelper.ExecuteDataTable(sql, connection);
        }

        /// <summary>
        /// 获取测点相关基础信息及相关因子日数据及等级
        /// </summary>
        /// <param name="PointIDs">测点数组</param>
        /// <param name="PollutantCodes">因子Code数组</param>
        /// <param name="Date">日期</param>
        /// <returns></returns>
        public DataTable GetWaterIEQI(List<int> PointIDs, List<string> PollutantCodes, DateTime Date)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select water.Grade,MONTH(water.DateTime) as months,DAY(water.DateTime) as days,water.PointId");
            strSql.Append(",mp.monitoringPointUid,mp.MonitoringPointName");
            strSql.Append(",ISNULL(item.ItemText,mp.MonitoringPointName) as WatersName ");
            if (PollutantCodes != null)
            {
                foreach (string code in PollutantCodes)
                {
                    strSql.Append(",(select top 1 PollutantValue from WaterReport.TB_DayReport ");
                    strSql.Append("where PollutantCode='" + code + "' ");
                    strSql.Append("and PointId=water.PointId and DateTime=water.DateTime) as '" + code + "' ");
                }
            }
            strSql.Append("from WaterReport.TB_DayReport water ");
            strSql.Append("inner join dbo.SY_MonitoringPoint mp on water.PointId=mp.PointId ");
            strSql.Append("inner join dbo.SY_MonitoringPointExtensionForEQMSWater mpw on mp.monitoringPointUid=mpw.monitoringPointUid ");
            strSql.Append("left join dbo.SY_View_CodeMainItem item on mpw.WatersName=item.ItemGuid ");
            strSql.Append("where water.PollutantCode='WaterQuality' ");
            if (Date != null && Date > DateTime.Parse("1900-01-01"))
            {
                strSql.Append("and water.DateTime=CONVERT(DATETIME,'" + Date + "') ");
            }
            if (PointIDs.Count > 0)
            {
                if (PointIDs.Count == 1)
                {
                    strSql.Append("and water.PointId='" + PointIDs[0] + "' ");
                }
                else
                {
                    strSql.Append("and (");
                    for (int i = 0; i < PointIDs.Count; i++)
                    {
                        if (i == 0)
                        {
                            strSql.Append(" water.PointId='" + PointIDs[i] + "' ");
                        }
                        else
                        {
                            strSql.Append(" or water.PointId='" + PointIDs[i] + "' ");
                        }
                    }
                    strSql.Append(")");
                }
            }
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), connection);
        }

        /// <summary>
        /// 获取自动点下的点位因子数据
        /// </summary>
        /// <param name="PointIds">站点ID数组</param>
        /// <param name="PollutantCodes">因子Code数组</param>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public DataTable GetWaterDayReport(List<int> PointIds, List<string> PollutantCodes, DateTime date)
        {
            if (PointIds.Count == 0 || PollutantCodes.Count == 0)
            {
                return null;
            }
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from WaterReport.TB_DayReport where DateTime=Convert(datetime,'" + date.ToString("yyyy-MM-dd") + "') ");
            if (PointIds.Count == 1)
            {
                strSql.Append("and PointId='" + PointIds[0] + "' ");
            }
            else
            {
                strSql.Append("and (");
                for (int i = 0; i < PointIds.Count; i++)
                {
                    if (i == 0)
                    {
                        strSql.Append(" PointId='" + PointIds[i] + "' ");
                    }
                    else
                    {
                        strSql.Append(" or PointId='" + PointIds[i] + "' ");
                    }
                }
                strSql.Append(")");
            }
            if (PollutantCodes.Count == 1)
            {
                strSql.Append("and PollutantCode='" + PollutantCodes[0] + "' ");
            }
            else
            {
                strSql.Append("and (");
                for (int i = 0; i < PollutantCodes.Count; i++)
                {
                    if (i == 0)
                    {
                        strSql.Append(" PollutantCode='" + PollutantCodes[i] + "' ");
                    }
                    else
                    {
                        strSql.Append(" or PollutantCode='" + PollutantCodes[i] + "' ");
                    }
                }
                strSql.Append(")");
            }
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), connection);
        }

        /// <summary>
        /// 获取水源地测点日均监测值
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="PointIds"></param>
        /// <param name="PollutantCodes"></param>
        /// <returns></returns>
        public DataView GetJSGDayReport(DateTime beginTime, DateTime endTime, List<int> PointIds, List<string> PollutantCodes)
        {
            try
            {
                string strPointIds = "";
                for (int i = 0; i < PointIds.Count; i++)
                {
                    strPointIds += PointIds[i] + ",";
                }
                strPointIds = strPointIds.TrimEnd(',');
                string strPollutantCodes = "";
                StringBuilder strFields = new StringBuilder();
                strFields.Append("PointId,CONVERT(nvarchar(10),DateTime,120) as Date ");
                for (int i = 0; i < PollutantCodes.Count; i++)
                {
                    strPollutantCodes += "'" + PollutantCodes[i] + "',";
                    strFields.AppendFormat(",MAX(CASE(PollutantCode) when '{0}' then PollutantValue end) as {0}", PollutantCodes[i]);
                }
                strPollutantCodes = strPollutantCodes.TrimEnd(',');
                string subSql = string.Format(@"select * FROM [AMS_MonitorBusiness].[WaterReport].[TB_DayReport]
                                    where PointId in ({0}) and PollutantCode in({1}) and DateTime>='{2}' and DateTime<='{3}'
                                    ", strPointIds, strPollutantCodes, beginTime.ToString("yyyy-MM-dd 00:00:00"), endTime.ToString("yyyy-MM-dd 00:00:00"));
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat(@"select {0} from ({1}) as DR
                                    group by PointId,DateTime
                                    order by PointId,DateTime", strFields, subSql);
                return g_DatabaseHelper.ExecuteDataView(strSql.ToString(), connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 获取水质现场调查相关监测数据
        /// </summary>
        /// <param name="PointIds">点位Id</param>
        /// <param name="Date">采集日期</param>
        /// <returns></returns>
        public DataView GetJSGSamplingData(DateTime BeginTime, DateTime EndTime, List<int> PointIds, List<string> PollutantCodes)
        {
            try
            {
                string strPointIds = "";
                for (int i = 0; i < PointIds.Count; i++)
                {
                    strPointIds += PointIds[i] + ",";
                }
                strPointIds = strPointIds.TrimEnd(',');
                string strPollutantCodes = "";
                StringBuilder strFields = new StringBuilder();
                strFields.Append("PointId,PointName,SamplingPosition,'' as Region,Remark,CONVERT(nvarchar(10),SamplingTime,120) as Date,CONVERT(nvarchar(5),SamplingTime,108) as Time");
                for (int i = 0; i < PollutantCodes.Count; i++)
                {
                    strPollutantCodes += "'" + PollutantCodes[i] + "',";
                    strFields.AppendFormat(",MAX(CASE(PollutantCode) when '{0}' then PollutantValue end) as {0}", PollutantCodes[i]);
                }
                strPollutantCodes = strPollutantCodes.TrimEnd(',');

                string subSql = string.Format(@"select * from dbo.TB_SamplingRecordDetail where PointId in ({0}) and PollutantCode in({1})
                                and CONVERT(nvarchar(10),SamplingTime,120)>='{2}' and CONVERT(nvarchar(10),SamplingTime,120)<='{3}'"
                    , strPointIds, strPollutantCodes, BeginTime.ToString("yyyy-MM-dd"), EndTime.ToString("yyyy-MM-dd"));
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat(@"select {0} from ({1}) as SRD
                                group by PointId,PointName,SamplingPosition,SamplingTime,Remark
                                order by SamplingTime", strFields, subSql);
                return g_DatabaseHelper.ExecuteDataView(strSql.ToString(), connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #region 水质运行月报数据
        /// <summary>
        /// 获取水质运行月报数据（最大值、最小值、平均值）
        /// </summary>
        /// <param name="PointIds"></param>
        /// <param name="BeginDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public DataView GetRMStatisticalData(List<int> PointIds, DateTime BeginDate, DateTime EndDate)
        {
            try
            {
                string dayWhere = string.Format(@"and CONVERT(nvarchar(10),DateTime,120)>='{0}' 
                                                   and CONVERT(nvarchar(10),DateTime,120)<='{1}'"
                                                 , BeginDate.ToString("yyyy-MM-dd"), EndDate.ToString("yyyy-MM-dd"));
                string hourWhere = string.Format(@"
                                                   and PointId =53 and PollutantCode in('w01028','w01029')
                                                   and CONVERT(nvarchar(10),Tstamp,120)>='{0}' 
                                                   and CONVERT(nvarchar(10),Tstamp,120)<='{1}'"
                                                 , BeginDate.ToString("yyyy-MM-dd"), EndDate.ToString("yyyy-MM-dd"));
                string subSqlu = string.Format(@"select PointId,PollutantCode,CONVERT(nvarchar(10),Tstamp,120) as Date,CONVERT(decimal(18,3),dbo.F_Round(AVG(PollutantValue),3)) as DayAvg,60*15*SUM(PollutantValue) as DaySum
                                                from WaterReport.TB_HourReport
                                                where 1=1 {0} {1}
                                                group by PointId,PollutantCode,CONVERT(nvarchar(10),Tstamp,120)", hourWhere, "and PollutantValue>=0");
                string subSqll = string.Format(@"select PointId,PollutantCode,CONVERT(nvarchar(10),Tstamp,120) as Date,CONVERT(decimal(18,3),dbo.F_Round(AVG(PollutantValue),3)) as DayAvg,60*15*SUM(PollutantValue) as DaySum
                                                from WaterReport.TB_HourReport
                                                where 1=1 {0} {1}
                                                group by PointId,PollutantCode,CONVERT(nvarchar(10),Tstamp,120)", hourWhere, "and PollutantValue<0");
                string pointWhere = "";
                if (PointIds.Count != 0)
                {
                    string strPointIds = "";
                    for (int i = 0; i < PointIds.Count; i++)
                    {
                        strPointIds += PointIds[i] + ",";
                    }
                    strPointIds = strPointIds.TrimEnd(',');
                    pointWhere = " and t1.PointId in(" + strPointIds + ")";
                }
                StringBuilder strSql = new StringBuilder();
                string strFields = " t1.PointId,PollutantCode";
                strSql.AppendFormat(@"select {0},MAX(PollutantValue) as Value_Max,MIN(PollutantValue) as Value_Min,AVG(PollutantValue) as Value_Avg,0 as Total_Max,0 as Total_Min,0 as Total_Avg,CategoryUid,0 as F 
                                    from WaterReport.TB_DayReport as t1
                                    left join AMS_BaseData.dbo.TB_FactorsRelationConfig as t2 on t1.PointId=t2.PointId and t1.PollutantCode=t2.PollutantCodes
                                    where 1=1 {1} {2}
                                    and PollutantCode in(
                                    select PollutantCodes from AMS_BaseData.dbo.TB_FactorsRelationConfig as t1
                                    where 1=1 {1}
                                    )
                                    and PollutantCode not in('w01028','w01029')
                                    group by t1.PointId,PollutantCode,CategoryUid", strFields, pointWhere, dayWhere);
                strSql.AppendFormat(@"  union
                                    select {0},MAX(DayAvg) as Value_Max,MIN(DayAvg) as Value_Min,AVG(DayAvg) as Value_Avg,MAX(DaySum) as Total_Max,MIN(DaySum) as Total_Min,AVG(DaySum) as Total_Avg,CategoryUid,1 as F
                                    from 
                                    (
                                    select * from ({1}) as t
                                    where Date in(select Date  from ({1}) as t where PollutantCode='w01028' and DayAvg>=0.02)
                                    union 
                                    select PointId,PollutantCode,Date,0 as DayAvg,0 as DaySum from ({1}) as t
                                    where Date in(select Date  from ({1}) as t where PollutantCode='w01028' and DayAvg<0.02)
                                    ) as t1
                                    left join AMS_BaseData.dbo.TB_FactorsRelationConfig as t2 on t1.PointId=t2.PointId and t1.PollutantCode=t2.PollutantCodes
                                    group by t1.PointId,PollutantCode,CategoryUid", strFields, subSqlu);
                strSql.AppendFormat(@"  union
                                    select {0},MIN(DayAvg) as Value_Max,MAX(DayAvg) as Value_Min,AVG(DayAvg) as Value_Avg,MIN(DaySum) as Total_Max,MAX(DaySum) as Total_Min,AVG(DaySum) as Total_Avg,CategoryUid,-1 as F
                                    from 
                                    (
                                    select * from ({1}) as t
                                    where Date in(select Date  from ({1}) as t where PollutantCode='w01028' and DayAvg<=-0.02)
                                    union 
                                    select PointId,PollutantCode,Date,0 as DayAvg,0 as DaySum from ({1}) as t
                                    where Date in(select Date  from ({1}) as t where PollutantCode='w01028' and DayAvg>-0.02)
                                    ) as t1
                                    left join AMS_BaseData.dbo.TB_FactorsRelationConfig as t2 on t1.PointId=t2.PointId and t1.PollutantCode=t2.PollutantCodes
                                    group by t1.PointId,PollutantCode,CategoryUid", strFields, subSqll);
                return g_DatabaseHelper.ExecuteDataView(strSql.ToString(), connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取水质运行月报数据（平均值）
        /// </summary>
        /// <param name="PointIds"></param>
        /// <param name="BeginDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public DataView GetRMStatisticalDataAvg(List<int> PointIds, DateTime BeginDate, DateTime EndDate)
        {
            try
            {
                string dayWhere = string.Format(@"and CONVERT(nvarchar(10),DateTime,120)>='{0}' 
                                                   and CONVERT(nvarchar(10),DateTime,120)<='{1}'"
                                                 , BeginDate.ToString("yyyy-MM-dd"), EndDate.ToString("yyyy-MM-dd"));
                string hourWhere = string.Format(@"and PointId =53 and PollutantCode in('w01028','w01029')
                                                   and CONVERT(nvarchar(10),Tstamp,120)>='{0}' 
                                                   and CONVERT(nvarchar(10),Tstamp,120)<='{1}'"
                                                 , BeginDate.ToString("yyyy-MM-dd"), EndDate.ToString("yyyy-MM-dd"));
                string subSqlu = string.Format(@"select PointId,PollutantCode,CONVERT(nvarchar(10),Tstamp,120) as Date,CONVERT(decimal(18,3),dbo.F_Round(AVG(PollutantValue),3)) as DayAvg,60*15*SUM(PollutantValue) as DaySum
                                                from WaterReport.TB_HourReport
                                                where 1=1 {0} {1}
                                                group by PointId,PollutantCode,CONVERT(nvarchar(10),Tstamp,120)", hourWhere, "and PollutantValue>=0");
                string subSqll = string.Format(@"select PointId,PollutantCode,CONVERT(nvarchar(10),Tstamp,120) as Date,CONVERT(decimal(18,3),dbo.F_Round(AVG(PollutantValue),3)) as DayAvg,60*15*SUM(PollutantValue) as DaySum
                                                from WaterReport.TB_HourReport
                                                where 1=1 {0} {1}
                                                group by PointId,PollutantCode,CONVERT(nvarchar(10),Tstamp,120)", hourWhere, "and PollutantValue<0");
                string pointWhere = "";
                if (PointIds.Count != 0)
                {
                    string strPointIds = "";
                    for (int i = 0; i < PointIds.Count; i++)
                    {
                        strPointIds += PointIds[i] + ",";
                    }
                    strPointIds = strPointIds.TrimEnd(',');
                    pointWhere = " and t1.PointId in(" + strPointIds + ")";
                }
                StringBuilder strSql = new StringBuilder();
                string strFields = " t1.PointId,PollutantCode";
                strSql.AppendFormat(@"select {0},AVG(PollutantValue) as Value_Avg,0 as Total_Avg,0 as F 
                                    from WaterReport.TB_DayReport as t1
                                    where 1=1 {1} {2}
                                    and PollutantCode in(
                                    select PollutantCodes from AMS_BaseData.dbo.TB_FactorsRelationConfig as t1
                                    where 1=1 {1}
                                    )
                                    and PollutantCode not in('w01028','w01029')
                                    group by t1.PointId,PollutantCode", strFields, pointWhere, dayWhere);
                strSql.AppendFormat(@"  union
                                    select {0},AVG(DayAvg) as Value_Avg,AVG(DaySum) as Total_Avg,1 as F
                                   from 
                                    (
                                    select * from ({1}) as t
                                    where Date in(select Date  from ({1}) as t where PollutantCode='w01028' and DayAvg>=0.02)
                                    union 
                                    select PointId,PollutantCode,Date,0 as DayAvg,0 as DaySum from ({1}) as t
                                    where Date in(select Date  from ({1}) as t where PollutantCode='w01028' and DayAvg<0.02)
                                    ) as t1
                                    group by t1.PointId,PollutantCode", strFields, subSqlu);
                strSql.AppendFormat(@"  union
                                    select {0},AVG(DayAvg) as Value_Avg,AVG(DaySum) as Total_Avg,-1 as F
                                    from 
                                    (
                                    select * from ({1}) as t
                                    where Date in(select Date  from ({1}) as t where PollutantCode='w01028' and DayAvg<=-0.02)
                                    union 
                                    select PointId,PollutantCode,Date,0 as DayAvg,0 as DaySum from ({1}) as t
                                    where Date in(select Date  from ({1}) as t where PollutantCode='w01028' and DayAvg>-0.02)
                                    ) as t1
                                    group by t1.PointId,PollutantCode", strFields, subSqll);
                return g_DatabaseHelper.ExecuteDataView(strSql.ToString(), connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取水质运行月报数据（日数据）
        /// </summary>
        /// <param name="PointIds"></param>
        /// <param name="BeginDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public DataView GetRMDayData(List<int> PointIds, DateTime BeginDate, DateTime EndDate)
        {
            try
            {
                string dayWhere = string.Format(@"and CONVERT(nvarchar(10),DateTime,120)>='{0}' 
                                                   and CONVERT(nvarchar(10),DateTime,120)<='{1}'"
                                                 , BeginDate.ToString("yyyy-MM-dd"), EndDate.ToString("yyyy-MM-dd"));
                string hourWhere = string.Format(@"and CONVERT(nvarchar(10),Tstamp,120)>='{0}' 
                                                   and CONVERT(nvarchar(10),Tstamp,120)<='{1}'"
                                                 , BeginDate.ToString("yyyy-MM-dd"), EndDate.ToString("yyyy-MM-dd"));
                string pointWhere = "";
                if (PointIds.Count != 0)
                {
                    string strPointIds = "";
                    for (int i = 0; i < PointIds.Count; i++)
                    {
                        strPointIds += PointIds[i] + ",";
                    }
                    strPointIds = strPointIds.TrimEnd(',');
                    pointWhere = " and t1.PointId in(" + strPointIds + ")";
                }
                StringBuilder strSql = new StringBuilder();
                string strFields = " t1.PointId,PollutantCode";
                strSql.AppendFormat(@"select {0},CONVERT(nvarchar(10),DateTime,120) as DateTime,PollutantValue,0 as TotalValue,CategoryUid,0 as F 
                                    from WaterReport.TB_DayReport as t1
                                    left join AMS_BaseData.dbo.TB_FactorsRelationConfig as t2 on t1.PointId=t2.PointId and t1.PollutantCode=t2.PollutantCodes
                                    where 1=1 {1} {2}
                                    and PollutantCode in(
                                    select PollutantCodes from AMS_BaseData.dbo.TB_FactorsRelationConfig as t1
                                    where 1=1 {1}
                                    )
                                    and PollutantCode not in('w01028','w01029')", strFields, pointWhere, dayWhere);
                strSql.AppendFormat(@"  union
                                    select {0},CONVERT(nvarchar(10),Tstamp,120) as Date,AVG(PollutantValue) as PollutantValue,60*15*SUM(PollutantValue) as TotalValue,CategoryUid,1 as F 
                                    from WaterReport.TB_HourReport as t1
                                    left join AMS_BaseData.dbo.TB_FactorsRelationConfig as t2 on t1.PointId=t2.PointId and t1.PollutantCode=t2.PollutantCodes
                                    where t1.PointId =53 {1}
                                    and PollutantCode in('w01028','w01029')
                                    and PollutantValue>=0
                                    group by t1.PointId,PollutantCode,CategoryUid,CONVERT(nvarchar(10),Tstamp,120)", strFields, hourWhere);
                strSql.AppendFormat(@"  union
                                    select {0},CONVERT(nvarchar(10),Tstamp,120) as Date,AVG(PollutantValue) as PollutantValue,60*15*SUM(PollutantValue) as TotalValue,CategoryUid,-1 as F 
                                    from WaterReport.TB_HourReport as t1
                                    left join AMS_BaseData.dbo.TB_FactorsRelationConfig as t2 on t1.PointId=t2.PointId and t1.PollutantCode=t2.PollutantCodes
                                    where t1.PointId =53 {1}
                                    and PollutantCode in('w01028','w01029')
                                    and PollutantValue<0
                                    group by t1.PointId,PollutantCode,CategoryUid,CONVERT(nvarchar(10),Tstamp,120)", strFields, hourWhere);
                return g_DatabaseHelper.ExecuteDataView(strSql.ToString(), connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取水质类别或因子限值
        /// </summary>
        /// <param name="PointIds"></param>
        /// <param name="Year"></param>
        /// <param name="Month"></param>
        /// <returns></returns>
        public DataView GetGradeOrLimit(List<int> PointIds, int Year, int Month)
        {
            string VOCGuid = "41fb0a54-660b-4f8b-b298-47d48955cfe0"; //VOC类型Uid
            try
            {
                string pointWhere = "";
                if (PointIds.Count != 0)
                {
                    string strPointIds = "";
                    for (int i = 0; i < PointIds.Count; i++)
                    {
                        strPointIds += PointIds[i] + ",";
                    }
                    strPointIds = strPointIds.TrimEnd(',');
                    pointWhere = " and PointId in(" + strPointIds + ")";
                }
                string strSql = string.Format(@"select PointId,PollutantCode,Grade as Grade,99999 as UPPER,-99999 as Low,'Grade' as F from WaterReport.TB_MonthReport
                                            where 1=1 {0}
                                            and PollutantCode in(
                                            select PollutantCodes from AMS_BaseData.dbo.TB_FactorsRelationConfig as t1
                                            where 1=1 {0}
                                            and t1.CategoryUid !='{1}'
                                            )
                                            and YEAR={2} and MonthOfYear={3}
                                            Union
                                            select 0 as PointId,PollutantCode,99999 as Grade,UPPER,Low,'Limit' as F from AMS_BaseData.Standard.TB_EQIConcentrationLimit
                                            where PollutantCode in(
                                            select PollutantCodes from AMS_BaseData.dbo.TB_FactorsRelationConfig as t1
                                            where 1=1 {0}
                                            and t1.CategoryUid ='{1}'
                                            )", pointWhere, VOCGuid, Year, Month);
                return g_DatabaseHelper.ExecuteDataView(strSql, connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #endregion
    }
}
