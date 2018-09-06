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
    /// 名称：HourReportDAL.cs
    /// 创建人：李飞
    /// 创建日期：2015-08-18
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 地表水发布：审核小时数据库处理
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
        /// 数据库表名
        /// </summary>
        private string tableName = "WaterReport.TB_HourReport";
        #endregion

        #region << 构造函数 >>
        /// <summary>
        /// 构造函数
        /// </summary>
        public HourReportDAL()
        {
            tableName = SmartEP.Data.Enums.EnumMapping.GetAutoMonitorTableName(ApplicationType.Water, PollutantDataType.Hour);
            connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(ApplicationType.Water, PollutantDataType.Hour);
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
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Tstamp")
        {
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "PointId,Tstamp";
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
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN dbo.F_ValidValueByFlag(PollutantValue,AuditFlag,',') END ) AS [{0}] ", factor);
                    factorDataFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN dataFlag END ) AS [{0}] ", factor + "_dataFlag");
                    factorAuditFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN AuditFlag END ) AS [{0}] ", factor + "_AuditFlag");
                    factorEQISql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN EQI END ) AS [{0}] ", factor + "_EQI");
                    factorGradeSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN Grade END ) AS [{0}] ", factor + "_Grade");
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

                orderBy = string.IsNullOrEmpty(orderBy) ? "PointId,Tstamp" : orderBy;
                string fieldName = "PointId,Tstamp" + factorSql + factorDataFlagSql + factorAuditFlagSql + factorEQISql + factorGradeSql + wqSql;
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

                foreach (string factor in factors)
                {
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
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

                orderBy = string.IsNullOrEmpty(orderBy) ? "PointId,Tstamp" : orderBy;
                string fieldName = "PointId,Tstamp" + factorSql + factorFlagSql;
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
                        factorSql += string.Format(",MAX(CASE WHEN (PollutantCode)= '{0}' and Type='{1}' THEN dbo.F_ValidValueByFlagWaterNEW(PollutantValue,AuditFlag,',','1') else -10 END ) AS [{0}{1}] ", factor, typeItem);
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
                string tableName = "dbo.V_Water_DataCompare";
                DateTime dateB = new DateTime();
                DateTime dateE = new DateTime();
                string where = "";
                if (dtFrom == dateB && dtTo == dateE)
                {
                    where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}'", dtBegin.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr + type;
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
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN dbo.F_ValidValueByFlagWaterNEW(PollutantValue,AuditFlag,',','1') else -10 END ) AS [{0}] ", factor);
                    //  factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue else -10 END ) AS [{0}] ", factor);
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
                string fieldName = "PointId,Type,Tstamp" + factorSql + factorFlagSql;
                string groupBy = "PointId,Type,Tstamp";
                string tableName = "dbo.V_Water_DataCompare";
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
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN CAST(dbo.F_Round(dbo.F_ValidValueByFlagWater(PollutantValue,AuditFlag,',','1'),{1}) AS DECIMAL(10,{1})) END ) AS [{2}] ", factor.PollutantCode, decimalNum, string.Format("{0}({1})", factor.PollutantName, factor.PollutantMeasureUnit));
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

        public DataView GetExportData(string[] portIds, IList<IPollutant> factors
                , DateTime dtStart, DateTime dtEnd, string lv, string orderBy = "PointId,Tstamp")
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
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN CAST(dbo.F_Round(dbo.F_ValidValueByFlagWater(PollutantValue,AuditFlag,',','1'),{1}) AS DECIMAL(10,{1})) END ) AS [{2}] ", factor.PollutantCode, decimalNum, string.Format("{0}({1})", factor.PollutantName, factor.PollutantMeasureUnit));
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
                string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}' ", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr + lv;
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
            , DateTime dtStart, DateTime dtEnd, string orderBy = "PointId,Tstamp")
        {
            tableName = "dbo.V_Water_HourReport";
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "PointId,Tstamp";
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
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN dbo.F_ValidValueByFlagWater(PollutantValue,AuditFlag,',','1') END ) AS [{0}],MAX(CASE(PollutantCode) WHEN '{0}' THEN AuditFlag END ) AS [{0}_AuditFlag] ", factor);
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
                string fieldName = "rowNum as '序号',PointId,portName,CONVERT(NVARCHAR(19),Tstamp,120) as '日期' " + factorSql + factorFlagSql;
                string groupBy = "PointId,portName,Tstamp";
                string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}' ", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr;
                return g_GridViewPager.GetPivotDataPager(tableName, fieldName, groupBy, orderBy, where, pageSize, pageNo, connection, out recordTotal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataView GetExportData(string[] portIds, string[] factors
                , DateTime dtStart, DateTime dtEnd, string lv, string orderBy = "PointId,Tstamp")
        {
            tableName = "dbo.V_Water_HourReport";
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "PointId,Tstamp";
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
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN dbo.F_ValidValueByFlagWater(PollutantValue,AuditFlag,',','1') END ) AS [{0}] ", factor);
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
                string fieldName = "rowNum as '序号',PointId,portName,CONVERT(NVARCHAR(19),Tstamp,120) as '日期'" + factorSql + factorFlagSql;
                string groupBy = "PointId,portName,Tstamp";
                string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}' ", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr + lv;
                return g_GridViewPager.GetPivotDataPager(tableName, fieldName, groupBy, orderBy, where, pageSize, pageNo, connection, out recordTotal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获得小时审核前数据
        /// </summary>
        /// <param name="pointId"></param>
        /// <param name="tstamp"></param>
        /// <returns></returns>
        public DataView GetHourValue(string pointId, string tstamp)
        {
            try
            {
                string sql = "select SourcePollutantDataValue " + "from [AMS_MonitorBusiness].[Audit].[TB_AuditWaterLog] a " +
                "join  [AMS_MonitorBusiness].[Audit].[TB_AuditStatusForDay] b " + "on a.AuditStatusUid " + "= " + "b.AuditStatusUid " +
                "where a.AuditPollutantDataValue like '%RM%' and b.PointId= " + pointId + "and a.tstamp=' " + tstamp + "'";
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }



        }

        /// <summary>
        /// 获得小时审核原因
        /// </summary>
        /// <param name="pointId"></param>
        /// <param name="tstamp"></param>
        /// <returns></returns>
        public DataView GetHourReason(string pointId, string tstamp)
        {
            try
            {
                string sql = "select OperationReason " + "from [AMS_MonitorBusiness].[Audit].[TB_AuditWaterLog] a " +
                           "join  [AMS_MonitorBusiness].[Audit].[TB_AuditStatusForDay] b " + "on a.AuditStatusUid " + "= " + "b.AuditStatusUid " +
                           "where a.AuditPollutantDataValue like '%RM%' and b.PointId= " + pointId + "and a.tstamp=' " + tstamp + "'";
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 获得数据比对分析审核原因
        /// </summary>
        /// <param name="pointId"></param>
        /// <param name="tstamp"></param>
        /// <param name="factorCode"></param>
        /// <returns></returns>
        public DataView GetCompareReason(string tstamp, string factorCode)
        {
            try
            {
                string sql = "select OperationReason " + "from [AMS_MonitorBusiness].[Audit].[TB_AuditWaterLog]  " +
                           "where  tstamp=' " + tstamp + "' " + "and PollutantCode='" + factorCode + "' ";
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// 取得报表导出数据(行转列数据)
        /// </summary>
        /// <param name="autoMonitorType">查询数据类型</param>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetExportDataReport(string[] portIds, IList<IPollutant> factors
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
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN CAST(dbo.F_Round(dbo.F_ValidValueByFlagWaterNEW(PollutantValue,AuditFlag,','),{1}) AS DECIMAL(10,{1})) END ) AS [{0}] ", factor.PollutantCode, decimalNum);
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
                string fieldName = "rowNum as '序号',PointId,datepart(mm,Tstamp) Month,datepart(dd,Tstamp) Day,CONVERT(NVARCHAR(5),Tstamp,108) Hour,CONVERT(NVARCHAR(19),Tstamp,120) as '日期'" + factorSql + factorFlagSql;
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
            string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
            if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
            {
                portIdsStr = " AND PointId =" + portIds[0];
            }
            else if (!string.IsNullOrEmpty(portIdsStr))
            {
                portIdsStr = "AND PointId IN(" + portIdsStr + ")";
            }
            string valueSql = string.Empty;
            string WQISql = string.Empty;
            string WQLSql = string.Empty;
            if (WQPollutants != null)
            {
                foreach (string pollutant in WQPollutants)
                {
                    valueSql += string.Format(",MAX(CASE WHEN  PollutantCode='{0}' THEN dbo.F_ValidValueByFlagWater(PollutantValue,AuditFlag,',','1') END) AS '{0}'", pollutant);
                    WQISql += string.Format(",MAX(CASE WHEN  PollutantCode='{0}' THEN dbo.F_GetWQI('{0}','7c67a857-d602-4f90-a26d-edd3e9f4d36c',dbo.F_ValidValueByFlagWater(PollutantValue,AuditFlag,',','1'),3,'d8197909-568e-4319-874c-3ad7cbc92a7e') END) AS 'WQI_{0}'", pollutant);
                    WQLSql += string.Format(",MAX(CASE WHEN  PollutantCode='{0}' THEN dbo.F_GetWQL('{0}','7c67a857-d602-4f90-a26d-edd3e9f4d36c',dbo.F_ValidValueByFlagWater(PollutantValue,AuditFlag,',','1'),'d8197909-568e-4319-874c-3ad7cbc92a7e','LEVEL') END) AS 'WQL_{0}'", pollutant);
                }
            }
            string sql = string.Format(@"
                SELECT PointId
	                ,Tstamp
                    --浓度
                    {0}
	                --指数
	                {1}
	                --等级
	                {2}
                FROM {3}
                WHERE Tstamp>='{4}' and Tstamp<='{5}' {6}
                GROUP BY PointId,Tstamp", valueSql, WQISql, WQLSql, tableName, dateStart.ToString("yyyy-MM-dd HH:mm:ss"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr);
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
                string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}' ", dateStart.ToString("yyyy-MM-dd HH:mm:ss"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr;

                //拼接统计数据字串，使用UNION方式减少统计条数，提升统计速度
                string sql = string.Empty;
                for (int iRow = 0; iRow < factors.Length; iRow++)
                {
                    if (iRow > 0)
                        sql += " UNION ";
                    sql += string.Format(@"
                        SELECT PointId,PollutantCode='{1}'
	                        ,AVG(dbo.F_ValidValueByFlagWater(PollutantValue,AuditFlag,',','1')) AS Value_Avg
	                        ,MAX(dbo.F_ValidValueByFlagWater(PollutantValue,AuditFlag,',','1')) AS Value_Max
	                        ,MIN(dbo.F_ValidValueByFlagWater(PollutantValue,AuditFlag,',','1')) AS Value_Min
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
                string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}' ", dateStart.ToString("yyyy-MM-dd HH:mm:ss"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr;

                //拼接统计数据字串，使用UNION方式减少统计条数，提升统计速度
                string sql = string.Empty;
                for (int iRow = 0; iRow < factors.Length; iRow++)
                {
                    if (iRow > 0)
                        sql += " UNION ";
                    sql += string.Format(@"
                        SELECT PointId,PollutantCode='{1}'
	                        ,AVG(dbo.F_ValidValueNew(PollutantValue,AuditFlag,',','1')) AS Value_Avg
	                        ,MAX(dbo.F_ValidValueNew(PollutantValue,AuditFlag,',','1')) AS Value_Max
	                        ,MIN(dbo.F_ValidValueNew(PollutantValue,AuditFlag,',','1')) AS Value_Min
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
        /// 取得统计数据（最大值对应的时间）
        /// </summary>
        /// <param name="autoMonitorType">数据类型</param>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetStatisticalMaxTstampData(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd)
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
SELECT PointId ,PollutantCode='{1}',PollutantValue,Tstamp FROM {0}  WHERE {2} AND PollutantValue IN(
                        SELECT MAX(PollutantValue) AS MAX_Avg
	                        
                        FROM {0}
                        WHERE {2}
                            AND PollutantCode='{1}'
                        GROUP BY PointId,PollutantCode)
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
        /// 蓝藻日报预警（小时数据前一天8点到第二天12点数据平均值）
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetBlueAlgaeDayData(string[] portIds, string[] factors
    , DateTime dtStart, DateTime dtEnd, string orderBy = "PointId,Tstamp")
        {

            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "PointId,Tstamp";
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                //string factorDataFlagSql = string.Empty;
                //string factorAuditFlagSql = string.Empty;
                //string factorEQISql = string.Empty;
                //string factorGradeSql = string.Empty;
                string factorAvgSql = string.Empty;
                string wqSql = string.Empty;

                foreach (string factor in factors)
                {
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                    //factorDataFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN dataFlag END ) AS [{0}] ", factor + "_dataFlag");
                    //factorAuditFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN AuditFlag END ) AS [{0}] ", factor + "_AuditFlag");
                    //factorEQISql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN EQI END ) AS [{0}] ", factor + "_EQI");
                    //factorGradeSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN Grade END ) AS [{0}] ", factor + "_Grade");
                    factorAvgSql += string.Format(",AVG([{0}]) AS [{0}] ", factor);
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

                orderBy = string.IsNullOrEmpty(orderBy) ? "PointId,Tstamp" : orderBy;
                string fieldName = "PointId,Tstamp" + factorSql;
                //string fieldName = "PointId,Tstamp" + factorSql + factorDataFlagSql + factorAuditFlagSql + factorEQISql + factorGradeSql + wqSql;
                string groupBy = "PointId,Tstamp";
                string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}' ", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr;
                string sql = string.Format(@" WITH hourData AS (SELECT {0} FROM {1} WHERE {2} GROUP BY {3})", fieldName, tableName, where, groupBy);
                //groupBy = "PointId,CONVERT(DATETIME,CONVERT(VARCHAR(10),Tstamp,120),120)";
                groupBy = "PointId";
                sql += string.Format(@" SELECT PointId,CONVERT(VARCHAR(10),MAX(Tstamp),120) as Tstamp {1} FROM hourData group by {0}", groupBy, factorAvgSql);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取所选点位数据数量
        /// </summary>
        /// <param name="pointId"></param>
        /// <param name="dtBegin"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public int GetDataCountByPointId(string pointId, DateTime dtBegin, DateTime dtEnd)
        {
            string sql = string.Format("select COUNT(*) as count from WaterReport.TB_HourReport where pointid = {0} and tstamp >= '{1}' and tstamp <= '{2}'", pointId, dtBegin, dtEnd);
            return Convert.ToInt32(g_DatabaseHelper.ExecuteDataView(sql, connection)[0]["count"]);
        }

        /// <summary>
        /// 获取蓝藻预警预报数据源
        /// </summary>
        /// <param name="pointId"></param>
        /// <param name="dtBegin"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataView GetAlgaeWQEarlyWarnReportDataSource(string pointId, DateTime dtBegin, DateTime dtEnd)
        {
            string sql = string.Format(@"SELECT 
                                        w.watersname as watersname
                                        ,p.monitoringpointname as portname
                                        ,p.pointId as portId
                                        ,datepart(MONTH,Tstamp) as m1
                                        ,datepart(DAY,Tstamp) as d1
                                        ,Tstamp
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
                                FROM WaterReport.TB_HourReport AS d INNER JOIN
                                      dbo.SY_MonitoringPoint as p on p.pointId = d.PointId 
                                      INNER JOIN dbo.SY_MonitoringPointExtensionForEQMSWater as w on w.monitoringpointuid = p.monitoringpointuid
                                      WHERE  p.pointId in ({0})
                                AND d.Tstamp >= '{1}' AND 
                                    d.Tstamp < '{2}'
                                group by p.monitoringpointname,Tstamp,p.PointId,w.watersname  order by p.monitoringpointname,Tstamp", pointId, dtBegin, dtEnd);
            return g_DatabaseHelper.ExecuteDataView(sql, connection);
        }
        #endregion
    }
}
