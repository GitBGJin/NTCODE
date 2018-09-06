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
    /// 名称：InstrumentDataService.cs
    /// 创建人：李飞
    /// 创建日期：2015-08-27
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 环境空气发布：仪器状态数据
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class InstrumentDataDAL
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
        private string connection = string.Empty;

        /// <summary>
        /// 数据库表名
        /// </summary>
        private string tableName = string.Empty;
        #endregion

        #region << 构造函数 >>
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="applicationType">应用类型（空气、地表水、噪声）</param>
        public InstrumentDataDAL(ApplicationType applicationType)
        {
            tableName = SmartEP.Data.Enums.EnumMapping.GetAutoMonitorTableName(applicationType, PollutantDataType.InstrumentData60);
            connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(applicationType, PollutantDataType.InstrumentData60);
        }
        #endregion

        #region << 方法 >>
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="instrumentCode">仪器</param>
        /// <param name="pollutantCodes">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetDataPager(string portId, string instrumentCode, string[] pollutantCodes
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "Tstamp")
        {
            recordTotal = 0;
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                foreach (string factor in pollutantCodes)
                {
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                }

                string portIdsStr = string.Empty;
                if (!string.IsNullOrEmpty(portId))
                {
                    portIdsStr = " AND PointId =" + portId;
                }
                string instrumentCodeStr = string.Empty;
                if (!string.IsNullOrEmpty(instrumentCode))
                {
                    instrumentCodeStr = string.Format(" AND instrumentCode ='{0}'", instrumentCode);
                }

                orderBy = string.IsNullOrEmpty(orderBy) ? "Tstamp" : orderBy;
                string fieldName = "PointId,instrumentCode,Tstamp" + factorSql + ",null as blankspaceColumn";
                string groupBy = "PointId,instrumentCode,Tstamp";
                string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}' ", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr + instrumentCodeStr;
                return g_GridViewPager.GetPivotDataPager(tableName, fieldName, groupBy, orderBy, where, pageSize, pageNo, connection, out recordTotal);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 取得导出数据(行转列数据)
        /// </summary>
        /// <param name="portId">测点数据</param>
        /// <param name="instrumentCode">仪器</param>
        /// <param name="pollutantCodes">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetExportData(string portId, string instrumentCode, string[] pollutantCodes
            , DateTime dtStart, DateTime dtEnd, string orderBy = "Tstamp")
        {
            int recordTotal = 0;
            int pageSize = 99999999;
            int pageNo = 0;
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                //string factorFlagSql = string.Empty;
                //foreach (IPollutant factor in pollutants)
                //{
                //    string factorFlag = factor.PollutantCode + "_Status";
                //    Int32 decimalNum = string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum);
                //    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN CAST(dbo.F_Round(PollutantValue,{1}) AS DECIMAL(10,{1})) END ) AS [{2}] ", factor.PollutantCode, decimalNum, string.Format("{0}({1})", factor.PollutantName, factor.PollutantMeasureUnit));
                //}
                foreach (string factor in pollutantCodes)
                {
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                }
                //站点处理
                string portIdsStr = string.Empty;
                if (!string.IsNullOrEmpty(portId))
                {
                    portIdsStr = " AND PointId =" + portId;
                }
                string instrumentCodeStr = string.Empty;
                if (!string.IsNullOrEmpty(instrumentCode))
                {
                    instrumentCodeStr = string.Format(" AND instrumentCode ='{0}'", instrumentCode);
                }

                orderBy = string.IsNullOrEmpty(orderBy) ? "Tstamp" : orderBy;
                string fieldName = "rowNum as '序号',PointId,instrumentCode,CONVERT(NVARCHAR(19),Tstamp,120) as '日期'" + factorSql;//+factorFlagSql;
                string groupBy = "PointId,instrumentCode,Tstamp";
                string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}' ", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr + instrumentCodeStr;
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
        /// <param name="instrumentCode">仪器</param>
        /// <param name="pollutantCodes">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetDataPager(string[] portIds, string instrumentCode, string[] pollutantCodes
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,Tstamp")
        {
            recordTotal = 0;
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                foreach (string factor in pollutantCodes)
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
                    portIdsStr = " AND PointId IN(" + portIdsStr + ")";
                }
                string instrumentCodeStr = string.Empty;
                if (!string.IsNullOrEmpty(instrumentCode))
                {
                    instrumentCodeStr = string.Format(" AND instrumentCode ='{0}'", instrumentCode);
                }

                orderBy = string.IsNullOrEmpty(orderBy) ? "PointId,Tstamp" : orderBy;
                string fieldName = "PointId,instrumentCode,Tstamp" + factorSql + ",null as blankspaceColumn";
                string groupBy = "PointId,instrumentCode,Tstamp";
                string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}' ", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr + instrumentCodeStr;
                return g_GridViewPager.GetPivotDataPager(tableName, fieldName, groupBy, orderBy, where, pageSize, pageNo, connection, out recordTotal);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 取得导出数据(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="instrumentCode">仪器</param>
        /// <param name="pollutantCodes">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetExportData(string[] portIds, string instrumentCode, string[] pollutantCodes
            , DateTime dtStart, DateTime dtEnd, string orderBy = "PointId,Tstamp")
        {
            int recordTotal = 0;
            int pageSize = 99999999;
            int pageNo = 0;
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                //string factorFlagSql = string.Empty;
                //foreach (IPollutant factor in pollutants)
                //{
                //    string factorFlag = factor.PollutantCode + "_Status";
                //    Int32 decimalNum = string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum);
                //    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN CAST(dbo.F_Round(PollutantValue,{1}) AS DECIMAL(10,{1})) END ) AS [{2}] ", factor.PollutantCode, decimalNum, string.Format("{0}({1})", factor.PollutantName, factor.PollutantMeasureUnit));
                //}
                foreach (string factor in pollutantCodes)
                {
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
                    portIdsStr = " AND PointId IN(" + portIdsStr + ")";
                }
                string instrumentCodeStr = string.Empty;
                if (!string.IsNullOrEmpty(instrumentCode))
                {
                    instrumentCodeStr = string.Format(" AND instrumentCode ='{0}'", instrumentCode);
                }

                orderBy = string.IsNullOrEmpty(orderBy) ? "PointId,Tstamp" : orderBy;
                string fieldName = "rowNum as '序号',PointId,instrumentCode,CONVERT(NVARCHAR(19),Tstamp,120) as '日期'" + factorSql;//+factorFlagSql;
                string groupBy = "PointId,instrumentCode,Tstamp";
                string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}' ", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr + instrumentCodeStr;
                return g_GridViewPager.GetPivotDataPager(tableName, fieldName, groupBy, orderBy, where, pageSize, pageNo, connection, out recordTotal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
