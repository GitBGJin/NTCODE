using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Data.Enums;
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
    /// 名称：PortDayAQIDAL.cs
    /// 创建人：李飞
    /// 创建日期：2015-08-18
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 环境空气发布：区域日AQI数据库处理
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class RegionDayAQIDAL
    {
        #region << 变量 >>
        /// <summary>
        /// 数据库处理类
        /// </summary>
        protected DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();

        /// <summary>
        /// 虚拟分页类
        /// </summary>
        protected GridViewPagerDAL g_GridViewPager = Singleton<GridViewPagerDAL>.GetInstance();

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        protected string connection = null;

        /// <summary>
        /// 数据库表名
        /// </summary>
        protected string tableName = null;
        protected string tableName1 = null;
        /// <summary>
        /// 存储过程处理类
        /// </summary>
        BaseDAHelper g_DBBiz = Singleton<BaseDAHelper>.GetInstance();
        #endregion

        #region << 构造函数 >>
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="aQIDataType">AQI数据类型</param>
        public RegionDayAQIDAL()
        {
            tableName = SmartEP.Data.Enums.EnumMapping.GetTableName(AQIDataType.RegionDayAQI);
            tableName1 = SmartEP.Data.Enums.EnumMapping.GetTableName(AQIDataType.DayAQI);
            connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.MonitoringBusiness);
        }
        #endregion

        #region << 数据查询方法 >>
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <param name="orderBy">排序字段(MonitoringRegionUid,ReportDateTime)</param>
        /// <returns></returns>
        public DataView GetDataPager(string[] regionGuids, DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo
            , out int recordTotal, RegionAQIStatisticalType regionAQIStatisticalType, string orderBy = "MonitoringRegionUid,ReportDateTime")
        {
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "MonitoringRegionUid,ReportDateTime";
            recordTotal = 0;

            string fieldName = GetFieldName();
            string keyName = "MonitoringRegionUid";

            //查询条件拼接
            string where = string.Empty;
            string regionGuidStr = string.Empty;
            if (regionGuids != null && regionGuids.Length > 0)
                regionGuidStr = " AND MonitoringRegionUid IN ('" + StringExtensions.GetArrayStrNoEmpty(regionGuids.ToList(), "','") + "')";

            where = string.Format(" ReportDateTime>='{0}' AND ReportDateTime<='{1}' AND StatisticalType='{2}' ", dtStart.ToString("yyyy-MM-dd HH:mm:ss")
                , dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), SmartEP.Core.Enums.EnumMapping.GetDesc(regionAQIStatisticalType)) + regionGuidStr;

            return g_GridViewPager.GetGridViewPager(tableName, fieldName, keyName, pageSize, pageNo, orderBy, where, connection, out recordTotal);
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <param name="orderBy">排序字段(MonitoringRegionUid,ReportDateTime)</param>
        /// <returns></returns>
        public DataView GetDataBasePager(string[] regionGuids, DateTime dtStart, DateTime dtEnd)
        {
            string tableNameBase = "AirRelease.TB_CheckRegionDayAQI";

            try
            {
                string sql = string.Format(@"SELECT [RegionUid] as [MonitoringRegionUid]
                                            ,[DateTime]
                                            ,[SO2]
                                            ,[SO2_IAQI]
                                            ,[NO2]
                                            ,[NO2_IAQI]
                                            ,[PM10]
                                            ,[PM10_IAQI]
                                            ,[CO]
                                            ,[CO_IAQI]
                                            ,[MaxOneHourO3]
                                            ,[MaxOneHourO3_IAQI]
                                            ,[Max8HourO3]
                                            ,[Max8HourO3_IAQI]
                                            ,[PM25]
                                            ,[PM25_IAQI]
                                            ,[AQIValue]
                                            FROM {0}
                                            where RegionUid='7e05b94c-bbd4-45c3-919c-42da2e63fd43'    
                                                and DateTime>='{1}' and DateTime<='{2}'",
                                           tableNameBase, dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"));
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }

        }
        /// <summary>
        ///市区空气质量
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段(PointId,DateTime)</param>
        /// <returns></returns>
        public DataView GetDataMonthPager(DateTime dtStart, DateTime dtEnd)
        {
            try
            {
                string sql = string.Format(@"SELECT 
                                              AVG(CONVERT (decimal(18,6),[PM25])) as a34004
                                              ,AVG(CONVERT (decimal(18,6),[PM10])) as a34002
                                              ,AVG(CONVERT (decimal(18,6),SO2)) as a21026
                                              ,AVG(CONVERT (decimal(18,6),NO2)) as a21004
                                              ,AVG(CONVERT (decimal(18,6),CO)) as a21005
                                              ,AVG(CONVERT (decimal(18,6),Max8HourO3)) as a05024
                                              ,SUM(CASE WHEN AQIValue >0 and AQIValue <=100 THEN 1 ELSE 0 END) as Good
                                              ,SUM(CASE WHEN AQIValue >0  THEN 1 ELSE 0 END) as ValueCount
                                              ,SUM(CASE WHEN AQIValue is null THEN 1 ELSE 0 END) as Days
                                            FROM {0}
                                            where MonitoringRegionUid='7e05b94c-bbd4-45c3-919c-42da2e63fd43'    
                                                and ReportDateTime>='{1}' and ReportDateTime<='{2}'",
                                           tableName, dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"));
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <param name="orderBy">排序字段(MonitoringRegionUid,ReportDateTime)</param>
        /// <returns></returns>
        public DataView GetDataPager(string[] regionGuids, DateTime dtStart, DateTime dtEnd, string[] qualityType, int pageSize, int pageNo
        , out int recordTotal, RegionAQIStatisticalType regionAQIStatisticalType, string orderBy = "MonitoringRegionUid,ReportDateTime")
        {
            string where1 = string.Empty;
            string where2 = string.Empty;
            if (qualityType.Contains("无效天"))
            {
                if (qualityType.Length > 1)
                {
                    where1 += " or AQIValue is NULL";
                }
                else
                {
                    where1 += " AQIValue is NULL";
                }
                var list = qualityType.ToList();
                list.RemoveAt(list.IndexOf("无效天"));
                qualityType = list.ToArray();
            }
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "MonitoringRegionUid,ReportDateTime";
            recordTotal = 0;

            string fieldName = GetFieldName();
            string keyName = "MonitoringRegionUid";

            //查询条件拼接
            string where = string.Empty;
            string regionGuidStr = string.Empty;
            if (regionGuids != null && regionGuids.Length > 0)
                regionGuidStr = " AND MonitoringRegionUid IN ('" + StringExtensions.GetArrayStrNoEmpty(regionGuids.ToList(), "','") + "')";

            where = string.Format(" ReportDateTime>='{0}' AND ReportDateTime<='{1}' AND StatisticalType='{2}' ", dtStart.ToString("yyyy-MM-dd HH:mm:ss")
                , dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), SmartEP.Core.Enums.EnumMapping.GetDesc(regionAQIStatisticalType)) + regionGuidStr;
            string qualityTypeStr = StringExtensions.GetArrayStrNoEmpty(qualityType.ToList<string>(), "','");
            if (qualityTypeStr != "")
            {
                qualityTypeStr = "'" + qualityTypeStr + "'";
            }
            if (!string.IsNullOrEmpty(qualityTypeStr))
            {
                where2 += "Class IN(" + qualityTypeStr + ")";
            }
            if (where1 != "")
                where += " and (" + where2 + where1 + ")";
            else if (where2 != "")
                where += " and " + where2;
            return g_GridViewPager.GetGridViewPager(tableName, fieldName, keyName, pageSize, pageNo, orderBy, where, connection, out recordTotal);
        }

        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)新方法
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <param name="orderBy">排序字段(MonitoringRegionUid,ReportDateTime)</param>
        /// <returns></returns>
        public DataView GetDataPagerNew(string[] regionGuids, DateTime dtStart, DateTime dtEnd, string[] qualityType, int pageSize, int pageNo
        , out int recordTotal, RegionAQIStatisticalType regionAQIStatisticalType, string orderBy = "MonitoringRegionUid,ReportDateTime")
        {
            //string where1 = string.Empty;
            //string where2 = string.Empty;
            //if (qualityType.Contains("无效天"))
            //{
            //    if (qualityType.Length > 1)
            //    {
            //        where1 += " or AQIValue is NULL";
            //    }
            //    else
            //    {
            //        where1 += " AQIValue is NULL";
            //    }
            //    var list = qualityType.ToList();
            //    list.RemoveAt(list.IndexOf("无效天"));
            //    qualityType = list.ToArray();
            //}
            //orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "MonitoringRegionUid,ReportDateTime";
            //recordTotal = 0;

            //string fieldName = GetFieldName();
            //string keyName = "MonitoringRegionUid";

            ////查询条件拼接
            //string where = string.Empty;
            //string regionGuidStr = string.Empty;
            //if (regionGuids != null && regionGuids.Length > 0)
            //    regionGuidStr = " AND MonitoringRegionUid IN ('" + StringExtensions.GetArrayStrNoEmpty(regionGuids.ToList(), "','") + "')";

            //where = string.Format(" ReportDateTime>='{0}' AND ReportDateTime<='{1}' AND StatisticalType='{2}' ", dtStart.ToString("yyyy-MM-dd HH:mm:ss")
            //    , dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), SmartEP.Core.Enums.EnumMapping.GetDesc(regionAQIStatisticalType)) + regionGuidStr;
            //string qualityTypeStr = StringExtensions.GetArrayStrNoEmpty(qualityType.ToList<string>(), "','");
            //if (qualityTypeStr != "")
            //{
            //    qualityTypeStr = "'" + qualityTypeStr + "'";
            //}
            //if (!string.IsNullOrEmpty(qualityTypeStr))
            //{
            //    where2 += "Class IN(" + qualityTypeStr + ")";
            //}
            //if (where1 != "")
            //    where += " and (" + where2 + where1 + ")";
            //else if (where2 != "")
            //    where += " and " + where2;
            //return g_GridViewPager.GetGridViewPager(tableName, fieldName, keyName, pageSize, pageNo, orderBy, where, connection, out recordTotal);

            string where1 = string.Empty;
            string where2 = string.Empty;
            string where = string.Empty;
            if (qualityType.Contains("无效天"))
            {
                if (qualityType.Length > 1)
                {
                    where1 += " or AQIValue is NULL";
                }
                else
                {
                    where1 += " AQIValue is NULL";
                }
                var list = qualityType.ToList();
                list.RemoveAt(list.IndexOf("无效天"));
                qualityType = list.ToArray();
            }
            string qualityTypeStr = StringExtensions.GetArrayStrNoEmpty(qualityType.ToList<string>(), "','");
            if (qualityTypeStr != "")
            {
                qualityTypeStr = "'" + qualityTypeStr + "'";
            }
            if (!string.IsNullOrEmpty(qualityTypeStr))
            {
                where2 += "Class IN(" + qualityTypeStr + ")";
            }
            if (where1 != "")
                where += " (" + where2 + where1 + ")";
            else if (where2 != "")
                where += " " + where2;
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "OrderByNum1,ReportDateTime1";
            string sqlCount = "select count(convert(varchar(10),dateadd(dd,number,'" + dtStart + "'),120)) from master..spt_values B where B.type='p'  and B.number <= datediff(dd,'" + dtStart + "','" + dtEnd.AddDays(-1) + "')";
            int countOnPort = (int)g_DatabaseHelper.ExecuteScalar(sqlCount, connection);
            recordTotal = countOnPort * regionGuids.Length;
            string portIdsStr = string.Empty;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from (");
            if (regionGuids != null && regionGuids.Length > 0)
            {
                for (int i = 0; i < regionGuids.Length; i++)
                {
                    portIdsStr = regionGuids[i];
                    string sql = "select (select distinct OrderByNum from [AMS_MonitorBusiness].[AirReport].[TB_RegionDayAQIReport] where MonitoringRegionUid='" + portIdsStr + "' and OrderByNum is not null) OrderByNum1, '" + portIdsStr + "' MonitoringRegionUid1,convert(varchar(10),dateadd(dd,number,'" + dtStart + "'),120) ReportDateTime1,A.* from master..spt_values B left join (select * from [AMS_MonitorBusiness].[AirReport].[TB_RegionDayAQIReport] where " + where + ") A on A.ReportDateTime = convert(varchar(10),dateadd(dd,number,'" + dtStart + "'),120) and A.MonitoringRegionUid='" + portIdsStr + "' where B.type='p'  and B.number <= datediff(dd,'" + dtStart + "','" + dtEnd.AddDays(-1) + "') ";
                    if (i == 0)
                    {
                        strSql.Append(sql);
                    }
                    else
                    {
                        strSql.AppendFormat(@" union {0}", sql);
                    }
                }
                strSql.Append(") AS M order by " + orderBy);
                DataView dt = g_DatabaseHelper.ExecuteDataView(strSql.ToString(), connection);
                return dt;
            }
            return null;
        }

        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段(PointId,DateTime)</param>
        /// <returns></returns>
        public DataView GetAreaDataPager(string[] regionGuids, DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal, RegionAQIStatisticalType regionAQIStatisticalType, string orderBy = "PointId")
        {
            recordTotal = 0;
            try
            {
                string regionGuidStr = string.Empty;
                if (regionGuids != null && regionGuids.Length > 0)
                    regionGuidStr = " AND MonitoringRegionUid IN ('" + StringExtensions.GetArrayStrNoEmpty(regionGuids.ToList(), "','") + "')";

                string sql = string.Format(@"SELECT [MonitoringRegionUid]
								,AVG(CONVERT (decimal(18,3),PM25)) as a34004
								,AVG(CONVERT (decimal(18,3),PM10)) as a34002
								,AVG(CONVERT (decimal(18,3),NO2)) as a21004
								,AVG(CONVERT (decimal(18,3),SO2)) as a21026
								,AVG(CONVERT (decimal(18,3),CO)) as a21005
								,AVG(CONVERT (decimal(18,3),Max8HourO3)) as a05024
                                FROM {0}
                                WHERE [ReportDateTime] >= '{1}' and [ReportDateTime] <= '{2}' AND StatisticalType='{3}' {4}
                                GROUP BY [MonitoringRegionUid] 
                                order by [MonitoringRegionUid]", tableName, dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"),
                                                              SmartEP.Core.Enums.EnumMapping.GetDesc(regionAQIStatisticalType), regionGuidStr);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 获取苏州大市AQI相关数据信息
        /// </summary>
        /// <param name="BeginTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        public DataView GetDayDataPager(DateTime BeginTime, DateTime EndTime)
        {
            try
            {
                string sql = string.Format(@"SELECT ReportDateTime,AVG(CONVERT (decimal(18,5),SO2)) as SO2
                                    ,AVG(CONVERT (decimal(18,5),NO2)) as NO2
                                    ,AVG(CONVERT (decimal(18,5),PM10)) as PM10
                                    ,AVG(CONVERT (decimal(18,5),CO)) as CO
                                    ,AVG(CONVERT (decimal(18,5),Max8HourO3)) as Max8HourO3
                                    ,AVG(CONVERT (decimal(18,5),PM25)) as PM25
                                    FROM AirReport.TB_RegionDayAQIReport
                                    WHERE [ReportDateTime] >= '{0}' and [ReportDateTime] <= '{1}' AND StatisticalType='CG'  
                                    AND MonitoringRegionUid IN 
                                    (
                                    select ItemGuid from EQMS_Framework.dbo.V_CodeDictionary 
                                    where CodeDictionaryName='空气点位区域类型' and CodeName in('空气点位区域类型','苏州市区')
                                    and ItemGuid!='7e05b94c-bbd4-45c3-919c-42da2e63fd43'
                                    )
                                    GROUP BY [ReportDateTime] 
                                    ORDER BY [ReportDateTime] ", BeginTime, EndTime);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 获取苏州大市AQI相关数据信息
        /// </summary>
        /// <param name="BeginTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        public DataView GetAQIBaseInfo(DateTime BeginTime, DateTime EndTime)
        {
            try
            {
                g_DBBiz.ClearParameters();
                SqlParameter pramBeginTime = new SqlParameter();
                pramBeginTime = new SqlParameter();
                pramBeginTime.SqlDbType = SqlDbType.DateTime;
                pramBeginTime.ParameterName = "@BeginTime";
                pramBeginTime.Value = BeginTime;
                g_DBBiz.SetProcedureParameters(pramBeginTime);

                SqlParameter pramEndTime = new SqlParameter();
                pramEndTime = new SqlParameter();
                pramEndTime.SqlDbType = SqlDbType.DateTime;
                pramEndTime.ParameterName = "@EndTime";
                pramEndTime.Value = EndTime;
                g_DBBiz.SetProcedureParameters(pramEndTime);

                var dv = g_DBBiz.ExecuteProc("GetAQI_DayBase_Mul", connection);
                return dv;
            }
            catch (Exception ex)
            {
                //return null;
                throw ex;
            }
        }
        /// <summary>
        /// 获取测点小时AQI相关数据信息
        /// </summary>
        /// <param name="BeginTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        public DataView GetPointAQIDayBase(DateTime BeginTime, DateTime EndTime, int pointStr, string dataType)
        {
            try
            {
                g_DBBiz.ClearParameters();
                SqlParameter pramBeginTime = new SqlParameter();
                pramBeginTime = new SqlParameter();
                pramBeginTime.SqlDbType = SqlDbType.DateTime;
                pramBeginTime.ParameterName = "@BeginTime";
                pramBeginTime.Value = BeginTime;
                g_DBBiz.SetProcedureParameters(pramBeginTime);

                SqlParameter pramEndTime = new SqlParameter();
                pramEndTime = new SqlParameter();
                pramEndTime.SqlDbType = SqlDbType.DateTime;
                pramEndTime.ParameterName = "@EndTime";
                pramEndTime.Value = EndTime;
                g_DBBiz.SetProcedureParameters(pramEndTime);

                SqlParameter pointIdStr = new SqlParameter();
                pointIdStr = new SqlParameter();
                pointIdStr.SqlDbType = SqlDbType.Int;
                pointIdStr.ParameterName = "@m_portlist";
                pointIdStr.Value = pointStr;
                g_DBBiz.SetProcedureParameters(pointIdStr);
                var dv = new System.Data.DataView();
                if (dataType == "OriData")
                {
                    dv = g_DBBiz.ExecuteProc("GetAQI_PointDayBase_Mul", "AMS_AirAutoMonitorConnection");
                }
                else
                {
                    dv = g_DBBiz.ExecuteProc("GetAQI_PointDayBase_Mul", connection);
                }

                return dv;
            }
            catch (Exception ex)
            {
                //return null;
                throw ex;
            }
        }
        /// <summary>
        /// 获取苏州大市浓度AQI日范围数据信息
        /// </summary>
        /// <param name="BeginTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        public DataView GetAQIDayRange(DateTime BeginTime, DateTime EndTime)
        {
            try
            {
                g_DBBiz.ClearParameters();
                SqlParameter pramBeginTime = new SqlParameter();
                pramBeginTime = new SqlParameter();
                pramBeginTime.SqlDbType = SqlDbType.DateTime;
                pramBeginTime.ParameterName = "@BeginTime";
                pramBeginTime.Value = BeginTime;
                g_DBBiz.SetProcedureParameters(pramBeginTime);

                SqlParameter pramEndTime = new SqlParameter();
                pramEndTime = new SqlParameter();
                pramEndTime.SqlDbType = SqlDbType.DateTime;
                pramEndTime.ParameterName = "@EndTime";
                pramEndTime.Value = EndTime;
                g_DBBiz.SetProcedureParameters(pramEndTime);

                var dv = g_DBBiz.ExecuteProc("GetAQI_DayRange_Mul", connection);
                return dv;
            }
            catch (Exception ex)
            {
                //return null;
                throw ex;
            }
        }
        /// <summary>
        /// 取得导出数据(行转列数据)
        /// </summary>
        /// <param name="autoMonitorType">查询数据类型</param>
        /// <param name="regionGuids">测点数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <param name="orderBy">排序字段(MonitoringRegionUid,ReportDateTime)</param>
        /// <returns></returns>
        public DataView GetExportData(string[] regionGuids, DateTime dtStart, DateTime dtEnd, RegionAQIStatisticalType regionAQIStatisticalType, string orderBy = "MonitoringRegionUid,ReportDateTime")
        {
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "MonitoringRegionUid,ReportDateTime";
            int recordTotal = 0;
            int pageSize = 99999999;
            int pageNo = 0;

            string fieldName = GetFieldName();
            string keyName = "MonitoringRegionUid";
            //查询条件拼接
            string where = string.Empty;
            string regionGuidStr = string.Empty;
            if (regionGuids != null && regionGuids.Length > 0)
                regionGuidStr = " AND MonitoringRegionUid IN ('" + StringExtensions.GetArrayStrNoEmpty(regionGuids.ToList(), "','") + "')";

            where = string.Format(" ReportDateTime>='{0}' AND ReportDateTime<='{1}' AND StatisticalType='{2}' ", dtStart.ToString("yyyy-MM-dd HH:mm:ss")
                , dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), SmartEP.Core.Enums.EnumMapping.GetDesc(regionAQIStatisticalType)) + regionGuidStr;

            return g_GridViewPager.GetGridViewPager(tableName, fieldName, keyName, pageSize, pageNo, orderBy, where, connection, out recordTotal);
        }

        /// <summary>
        /// 根据区域取得最新日AQI数据
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <returns></returns>
        public DataView GetLastData(string[] regionGuids, DateTime dtStart, DateTime dtEnd, RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            try
            {
                string regionGuidStr = string.Empty;
                if (regionGuids != null && regionGuids.Length > 0)
                    regionGuidStr = " AND MonitoringRegionUid IN ('" + StringExtensions.GetArrayStrNoEmpty(regionGuids.ToList(), "','") + "')";

                //拼接where条件
                string where = string.Format(" ReportDateTime>='{0}' AND ReportDateTime<='{1}' AND StatisticalType='{2}' ", dtStart.ToString("yyyy-MM-dd HH:mm:ss")
                    , dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), SmartEP.Core.Enums.EnumMapping.GetDesc(regionAQIStatisticalType)) + regionGuidStr;

                string sql = string.Format(@"
                WITH lastData AS
                (
                    SELECT MonitoringRegionUid AS RegionUid
	                    ,MAX(ReportDateTime) AS Tstamp
                    FROM {0}
                    where {1}
                    GROUP BY MonitoringRegionUid
                )
                SELECT {2}
                FROM lastData
                INNER JOIN {0} AS data
	                ON lastData.RegionUid = data.MonitoringRegionUid AND lastData.Tstamp = data.ReportDateTime
                ", tableName, where, GetFieldName());
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 取得查询字段
        /// </summary>
        /// <returns></returns>
        private string GetFieldName()
        {
            string fieldName = @"[MonitoringRegionUid]
                        ,[ReportDateTime]
                        ,[ReportType]
                        ,[SO2]
                        ,[SO2_IAQI]
                        ,[NO2]
                        ,[NO2_IAQI]
                        ,[PM10]
                        ,[PM10_IAQI]
                        ,[CO]
                        ,[CO_IAQI]
                        ,[MaxOneHourO3]
                        ,[MaxOneHourO3_IAQI]
                        ,[Max8HourO3]
                        ,[Max8HourO3_IAQI]
                        ,[PM25]
                        ,[PM25_IAQI]
                        ,[AQIValue]
                        ,[PrimaryPollutant]
                        ,[Range]
                        ,[RGBValue]
                        ,[PicturePath]
                        ,[Class]
                        ,[Grade]
                        ,[HealthEffect]
                        ,[TakeStep]";
            return fieldName;
        }

        #endregion

        #region << 环境质量统计 >>
        /// <summary>
        /// 各等级天数统计
        /// </summary>
        /// <param name="aqiType">AQI分指标字段</param>
        /// <param name="regionGuids"></param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <returns></returns>
        public DataView GetGradeStatistics(IAQIType aqiType, string[] regionGuids, DateTime dateStart, DateTime dateEnd, RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            string regionGuidStr = string.Empty;
            if (regionGuids != null && regionGuids.Length > 0)
                regionGuidStr = " AND MonitoringRegionUid IN ('" + StringExtensions.GetArrayStrNoEmpty(regionGuids.ToList(), "','") + "')";

            string sql = string.Format(@"
                SELECT MonitoringRegionUid
                    ,SUM(CASE WHEN {0} >= 0 AND {0}<=50 THEN 1 ELSE 0 END) AS Level1Count
                    ,SUM(CASE WHEN {0} > 50 AND {0}<=100 THEN 1 ELSE 0 END) AS Level2Count
                    ,SUM(CASE WHEN {0} > 100 AND {0}<=150 THEN 1 ELSE 0 END) AS Level3Count
                    ,SUM(CASE WHEN {0} > 150 AND {0}<=200 THEN 1 ELSE 0 END) AS Level4Count
                    ,SUM(CASE WHEN {0} > 200 AND {0}<=300 THEN 1 ELSE 0 END) AS Level5Count
                    ,SUM(CASE WHEN {0} > 300 THEN 1 ELSE 0 END) AS Level6Count
                    ,SUM(CASE WHEN {0} >= 0 AND {0} <= 100 THEN 1 ELSE 0 END) AS FineCount
                    ,SUM(CASE WHEN {0} > 100 THEN 1 ELSE 0 END) AS OverCount
                    ,SUM(CASE WHEN {0} >= 0 THEN 1 ELSE 0 END) AS ValidCount
                FROM {1}
                WHERE ReportDateTime>='{2}' AND ReportDateTime<='{3}' AND StatisticalType='{4}' {5}
                GROUP BY MonitoringRegionUid ", SmartEP.Core.Enums.EnumMapping.GetIAQITypeColumn(aqiType), tableName
                                              , dateStart.ToString("yyyy-MM-dd HH:mm:ss"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss"), SmartEP.Core.Enums.EnumMapping.GetDesc(regionAQIStatisticalType), regionGuidStr);
            return g_DatabaseHelper.ExecuteDataView(sql, connection);
        }
        /// <summary>
        /// 市区无效日期
        /// </summary>
        /// <param name="aqiType">AQI分指标字段</param>
        /// <param name="regionGuids"></param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <returns></returns>
        public DataView GetMonthReportTimeDataTable(DateTime dateStart, DateTime dateEnd, RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            string sql = string.Format(@"SELECT ReportDateTime as DateTime
                    FROM {0}
                    WHERE ReportDateTime >= '{1}' and ReportDateTime <= '{2}' AND StatisticalType='{3}' AND [MonitoringRegionUid] in
                           (
                             '7e05b94c-bbd4-45c3-919c-42da2e63fd43'                 --市区
		                ) and AQIValue is null", tableName
                                              , dateStart.ToString("yyyy-MM-dd HH:mm:ss"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss"), SmartEP.Core.Enums.EnumMapping.GetDesc(regionAQIStatisticalType));
            return g_DatabaseHelper.ExecuteDataView(sql, connection);
        }
        /// <summary>
        /// 各污染物首要污染物统计
        /// </summary>
        /// <param name="aqiType">AQI分指标字段</param>
        /// <param name="regionGuids"></param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <returns></returns>
        public DataView GetContaminantsStatistics(IAQIType aqiType, string[] regionGuids, DateTime dateStart, DateTime dateEnd, RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            string regionGuidStr = string.Empty;
            if (regionGuids != null && regionGuids.Length > 0)
                regionGuidStr = " AND MonitoringRegionUid IN ('" + StringExtensions.GetArrayStrNoEmpty(regionGuids.ToList(), "','") + "')";

            string sql = string.Format(@"
                SELECT MonitoringRegionUid
                    ,SUM(CASE WHEN {0}=AQIValue AND AQIValue>50 THEN 1 ELSE 0 END) AS Count
                FROM {1}
                WHERE ReportDateTime>='{2}' AND ReportDateTime<='{3}' AND StatisticalType='{4}' {5}
                GROUP BY MonitoringRegionUid ", SmartEP.Core.Enums.EnumMapping.GetIAQITypeColumn(aqiType), tableName, dateStart.ToString("yyyy-MM-dd HH:mm:ss")
                                              , dateEnd.ToString("yyyy-MM-dd HH:mm:ss"), SmartEP.Core.Enums.EnumMapping.GetDesc(regionAQIStatisticalType), regionGuidStr);
            return g_DatabaseHelper.ExecuteDataView(sql, connection); ;
        }

        /// <summary>
        /// 取得指定日期内日数据均值数据
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <returns>
        /// 返回字段：
        /// PointId：站点ID
        /// PM25：PM25浓度
        /// PM10：PM10浓度
        /// NO2：NO2浓度
        /// SO2：SO2浓度
        /// CO：CO浓度
        /// MaxOneHourO3：MaxOneHourO3浓度
        /// Max8HourO3：Max8HourO3浓度
        /// </returns>
        public DataView GetAvgValue(string[] regions, DateTime dateStart, DateTime dateEnd, RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            try
            {
                string regionsStr = "'" + StringExtensions.GetArrayStrNoEmpty(regions.ToList<string>(), "','") + "'";
                if (regions.Length > 0) regionsStr = string.Format(" and MonitoringRegionUid IN ({0}) ", regionsStr);
                string sql = string.Format(@"
                    SELECT MonitoringRegionUid
                        ,Convert(numeric(18,5),[dbo].[F_Round](AVG(dbo.F_ValidValueStr([PM25])),5)) AS [PM25]
                        ,Convert(numeric(18,5),[dbo].[F_Round](AVG(dbo.F_ValidValueStr([PM10])),5)) AS [PM10]
                        ,Convert(numeric(18,5),[dbo].[F_Round](AVG(dbo.F_ValidValueStr([NO2])),5)) AS [NO2]
                        ,Convert(numeric(18,5),[dbo].[F_Round](AVG(dbo.F_ValidValueStr([SO2])),5)) AS [SO2]
                        ,Convert(numeric(18,5),[dbo].[F_Round](AVG(dbo.F_ValidValueStr([CO])),5)) AS [CO]
                        ,Convert(numeric(18,5),[dbo].[F_Round](AVG(dbo.F_ValidValueStr([MaxOneHourO3])),5)) AS [MaxOneHourO3]
                        ,Convert(numeric(18,5),[dbo].[F_Round](AVG(dbo.F_ValidValueStr([Max8HourO3])),5)) AS [Max8HourO3]
                    FROM {0}
                    WHERE ReportDateTime >= '{1}' and ReportDateTime <= '{2}' AND StatisticalType='{3}' {4}
                    GROUP BY MonitoringRegionUid ", SmartEP.Data.Enums.EnumMapping.GetTableName(AQIDataType.RegionDayAQI), dateStart.ToString("yyyy-MM-dd HH:mm:ss")
                                                  , dateEnd.ToString("yyyy-MM-dd HH:mm:ss"), SmartEP.Core.Enums.EnumMapping.GetDesc(regionAQIStatisticalType), regionsStr);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 取得指定日期内日数据最大值数据
        /// </summary>
        /// <param name="regions">站点</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <returns>
        /// 返回字段：
        /// PointId：站点ID
        /// PM25：PM25浓度
        /// PM10：PM10浓度
        /// NO2：NO2浓度
        /// SO2：SO2浓度
        /// CO：CO浓度
        /// MaxOneHourO3：MaxOneHourO3浓度
        /// Max8HourO3：Max8HourO3浓度
        /// PM25_IAQI：PM25指数
        /// PM10_IAQI：PM10指数
        /// NO2_IAQI：NO2指数
        /// SO2_IAQI：SO2指数
        /// CO_IAQI：CO指数
        /// MaxOneHourO3_IAQI：MaxOneHourO3指数
        /// Max8HourO3_IAQI：Max8HourO3指数
        /// AQIValue：空气质量指数
        /// </returns>
        public DataView GetMaxValue(string[] regions, DateTime dateStart, DateTime dateEnd, RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            try
            {
                string regionsStr = "'" + StringExtensions.GetArrayStrNoEmpty(regions.ToList<string>(), "','") + "'";
                if (regions.Length > 0) regionsStr = string.Format(" and MonitoringRegionUid IN ({0}) ", regionsStr);
                string sql = string.Format(@"
                    SELECT MonitoringRegionUid
                        ,MAX(dbo.F_ValidValueStr([PM25])) AS [PM25]
                        ,MAX(dbo.F_ValidValueStr([PM10])) AS [PM10]
                        ,MAX(dbo.F_ValidValueStr([NO2])) AS [NO2]
                        ,MAX(dbo.F_ValidValueStr([SO2])) AS [SO2]
                        ,MAX(dbo.F_ValidValueStr([CO])) AS [CO]
                        ,MAX(dbo.F_ValidValueStr([MaxOneHourO3])) AS [MaxOneHourO3]
                        ,MAX(dbo.F_ValidValueStr([Max8HourO3])) AS [Max8HourO3]
                        ,MAX(dbo.F_ValidValueStr([PM25_IAQI])) AS [PM25_IAQI]
                        ,MAX(dbo.F_ValidValueStr([PM10_IAQI])) AS [PM10_IAQI]
                        ,MAX(dbo.F_ValidValueStr([NO2_IAQI])) AS [NO2_IAQI]
                        ,MAX(dbo.F_ValidValueStr([SO2_IAQI])) AS [SO2_IAQI]
                        ,MAX(dbo.F_ValidValueStr([CO_IAQI])) AS [CO_IAQI]
                        ,MAX(dbo.F_ValidValueStr([MaxOneHourO3_IAQI])) AS [MaxOneHourO3_IAQI]
                        ,MAX(dbo.F_ValidValueStr([Max8HourO3_IAQI])) AS [Max8HourO3_IAQI]
                        ,MAX(dbo.F_ValidValueStr([AQIValue])) AS [AQIValue]
                    FROM {0}
                    WHERE ReportDateTime >= '{1}' and ReportDateTime <= '{2}' AND StatisticalType='{3}' {4}
                    GROUP BY MonitoringRegionUid", SmartEP.Data.Enums.EnumMapping.GetTableName(AQIDataType.RegionDayAQI), dateStart.ToString("yyyy-MM-dd HH:mm:ss")
                                                  , dateEnd.ToString("yyyy-MM-dd HH:mm:ss"), SmartEP.Core.Enums.EnumMapping.GetDesc(regionAQIStatisticalType), regionsStr);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 取得指定日期内日数据最大值数据
        /// </summary>
        /// <param name="regions">站点</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <returns>
        /// 返回字段：
        /// PointId：站点ID
        /// PM25：PM25浓度
        /// PM10：PM10浓度
        /// NO2：NO2浓度
        /// SO2：SO2浓度
        /// CO：CO浓度
        /// MaxOneHourO3：MaxOneHourO3浓度
        /// Max8HourO3：Max8HourO3浓度
        /// PM25_IAQI：PM25指数
        /// PM10_IAQI：PM10指数
        /// NO2_IAQI：NO2指数
        /// SO2_IAQI：SO2指数
        /// CO_IAQI：CO指数
        /// MaxOneHourO3_IAQI：MaxOneHourO3指数
        /// Max8HourO3_IAQI：Max8HourO3指数
        /// AQIValue：空气质量指数
        /// </returns>
        public DataView GetMaxValueOne(string[] regions, DateTime dateStart, DateTime dateEnd, RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            try
            {
                string regionsStr = "'" + StringExtensions.GetArrayStrNoEmpty(regions.ToList<string>(), "','") + "'";
                if (regions.Length > 0) regionsStr = string.Format(" and MonitoringRegionUid IN ({0}) ", regionsStr);
                string sql = string.Format(@"
                    SELECT MAX(dbo.F_ValidValueStr([PM25])) AS [PM25]
                        ,MAX(dbo.F_ValidValueStr([PM10])) AS [PM10]
                        ,MAX(dbo.F_ValidValueStr([NO2])) AS [NO2]
                        ,MAX(dbo.F_ValidValueStr([SO2])) AS [SO2]
                        ,MAX(dbo.F_ValidValueStr([CO])) AS [CO]
                        ,MAX(dbo.F_ValidValueStr([MaxOneHourO3])) AS [MaxOneHourO3]
                        ,MAX(dbo.F_ValidValueStr([Max8HourO3])) AS [Max8HourO3]
                        ,MAX(dbo.F_ValidValueStr([PM25_IAQI])) AS [PM25_IAQI]
                        ,MAX(dbo.F_ValidValueStr([PM10_IAQI])) AS [PM10_IAQI]
                        ,MAX(dbo.F_ValidValueStr([NO2_IAQI])) AS [NO2_IAQI]
                        ,MAX(dbo.F_ValidValueStr([SO2_IAQI])) AS [SO2_IAQI]
                        ,MAX(dbo.F_ValidValueStr([CO_IAQI])) AS [CO_IAQI]
                        ,MAX(dbo.F_ValidValueStr([MaxOneHourO3_IAQI])) AS [MaxOneHourO3_IAQI]
                        ,MAX(dbo.F_ValidValueStr([Max8HourO3_IAQI])) AS [Max8HourO3_IAQI]
                        ,MAX(dbo.F_ValidValueStr([AQIValue])) AS [AQIValue]
                    FROM {0}
                    WHERE ReportDateTime >= '{1}' and ReportDateTime <= '{2}' AND StatisticalType='{3}' {4}
                    ", SmartEP.Data.Enums.EnumMapping.GetTableName(AQIDataType.RegionDayAQI), dateStart.ToString("yyyy-MM-dd HH:mm:ss")
                                                  , dateEnd.ToString("yyyy-MM-dd HH:mm:ss"), SmartEP.Core.Enums.EnumMapping.GetDesc(regionAQIStatisticalType), regionsStr);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 月报短信
        /// </summary>
        /// <param name="regions">站点</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <returns>
        public DataView GetRegionsAQI(DateTime dateStart, DateTime dateEnd, RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            try
            {
                string sql = string.Format(@"SELECT 
                                AVG(CONVERT (decimal(18,6),PM25)) as a34004
                                ,AVG(CONVERT (decimal(18,6),PM10)) as a34002
                                ,AVG(CONVERT (decimal(18,6),SO2)) as a21026
                                ,AVG(CONVERT (decimal(18,6),NO2)) as a21004
                                ,AVG(CONVERT (decimal(18,6),CO)) as a21005
                                ,AVG(CONVERT (decimal(18,6),Max8HourO3)) as a05024
								FROM {0}
								where [ReportDateTime]>='{1}' AND [ReportDateTime]<='{2}' and StatisticalType='{3}'  
								and [MonitoringRegionUid] in (
								'7e05b94c-bbd4-45c3-919c-42da2e63fd43'	                --市区
								)
								group by  [ReportDateTime]", SmartEP.Data.Enums.EnumMapping.GetTableName(AQIDataType.RegionDayAQI), dateStart.ToString("yyyy-MM-dd HH:mm:ss")
                                                  , dateEnd.ToString("yyyy-MM-dd HH:mm:ss"), SmartEP.Core.Enums.EnumMapping.GetDesc(regionAQIStatisticalType));
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 取得指定日期内日数据最小值数据
        /// </summary>
        /// <param name="regions">站点</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <returns>
        /// 返回字段：
        /// PointId：站点ID
        /// PM25：PM25浓度
        /// PM10：PM10浓度
        /// NO2：NO2浓度
        /// SO2：SO2浓度
        /// CO：CO浓度
        /// MaxOneHourO3：MaxOneHourO3浓度
        /// Max8HourO3：Max8HourO3浓度
        /// PM25_IAQI：PM25指数
        /// PM10_IAQI：PM10指数
        /// NO2_IAQI：NO2指数
        /// SO2_IAQI：SO2指数
        /// CO_IAQI：CO指数
        /// MaxOneHourO3_IAQI：MaxOneHourO3指数
        /// Max8HourO3_IAQI：Max8HourO3指数
        /// AQIValue：空气质量指数
        /// </returns>
        public DataView GetMinValue(string[] regions, DateTime dateStart, DateTime dateEnd, RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            try
            {
                string regionsStr = "'" + StringExtensions.GetArrayStrNoEmpty(regions.ToList<string>(), "','") + "'";
                if (regions.Length > 0) regionsStr = string.Format(" and MonitoringRegionUid IN ({0}) ", regionsStr);
                string sql = string.Format(@"
                    SELECT MonitoringRegionUid
                        ,MIN(dbo.F_ValidValueStr([PM25])) AS [PM25]
                        ,MIN(dbo.F_ValidValueStr([PM10])) AS [PM10]
                        ,MIN(dbo.F_ValidValueStr([NO2])) AS [NO2]
                        ,MIN(dbo.F_ValidValueStr([SO2])) AS [SO2]
                        ,MIN(dbo.F_ValidValueStr([CO])) AS [CO]
                        ,MIN(dbo.F_ValidValueStr([MaxOneHourO3])) AS [MaxOneHourO3]
                        ,MIN(dbo.F_ValidValueStr([Max8HourO3])) AS [Max8HourO3]
                        ,MIN(dbo.F_ValidValueStr([PM25_IAQI])) AS [PM25_IAQI]
                        ,MIN(dbo.F_ValidValueStr([PM10_IAQI])) AS [PM10_IAQI]
                        ,MIN(dbo.F_ValidValueStr([NO2_IAQI])) AS [NO2_IAQI]
                        ,MIN(dbo.F_ValidValueStr([SO2_IAQI])) AS [SO2_IAQI]
                        ,MIN(dbo.F_ValidValueStr([CO_IAQI])) AS [CO_IAQI]
                        ,MIN(dbo.F_ValidValueStr([MaxOneHourO3_IAQI])) AS [MaxOneHourO3_IAQI]
                        ,MIN(dbo.F_ValidValueStr([Max8HourO3_IAQI])) AS [Max8HourO3_IAQI]
                        ,MIN(dbo.F_ValidValueStr([AQIValue])) AS [AQIValue]
                    FROM {0}
                    WHERE ReportDateTime >= '{1}' and ReportDateTime <= '{2}' AND StatisticalType='{3}' {4}
                    GROUP BY MonitoringRegionUid ", SmartEP.Data.Enums.EnumMapping.GetTableName(AQIDataType.RegionDayAQI), dateStart.ToString("yyyy-MM-dd HH:mm:ss")
                                                  , dateEnd.ToString("yyyy-MM-dd HH:mm:ss"), SmartEP.Core.Enums.EnumMapping.GetDesc(regionAQIStatisticalType), regionsStr);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 取得指定日期内日数据最小值数据
        /// </summary>
        /// <param name="regions">站点</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <returns>
        /// 返回字段：
        /// PointId：站点ID
        /// PM25：PM25浓度
        /// PM10：PM10浓度
        /// NO2：NO2浓度
        /// SO2：SO2浓度
        /// CO：CO浓度
        /// MaxOneHourO3：MaxOneHourO3浓度
        /// Max8HourO3：Max8HourO3浓度
        /// PM25_IAQI：PM25指数
        /// PM10_IAQI：PM10指数
        /// NO2_IAQI：NO2指数
        /// SO2_IAQI：SO2指数
        /// CO_IAQI：CO指数
        /// MaxOneHourO3_IAQI：MaxOneHourO3指数
        /// Max8HourO3_IAQI：Max8HourO3指数
        /// AQIValue：空气质量指数
        /// </returns>
        public DataView GetMinValueOne(string[] regions, DateTime dateStart, DateTime dateEnd, RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            try
            {
                string regionsStr = "'" + StringExtensions.GetArrayStrNoEmpty(regions.ToList<string>(), "','") + "'";
                if (regions.Length > 0) regionsStr = string.Format(" and MonitoringRegionUid IN ({0}) ", regionsStr);
                string sql = string.Format(@"
                    SELECT MIN(dbo.F_ValidValueStr([PM25])) AS [PM25]
                        ,MIN(dbo.F_ValidValueStr([PM10])) AS [PM10]
                        ,MIN(dbo.F_ValidValueStr([NO2])) AS [NO2]
                        ,MIN(dbo.F_ValidValueStr([SO2])) AS [SO2]
                        ,MIN(dbo.F_ValidValueStr([CO])) AS [CO]
                        ,MIN(dbo.F_ValidValueStr([MaxOneHourO3])) AS [MaxOneHourO3]
                        ,MIN(dbo.F_ValidValueStr([Max8HourO3])) AS [Max8HourO3]
                        ,MIN(dbo.F_ValidValueStr([PM25_IAQI])) AS [PM25_IAQI]
                        ,MIN(dbo.F_ValidValueStr([PM10_IAQI])) AS [PM10_IAQI]
                        ,MIN(dbo.F_ValidValueStr([NO2_IAQI])) AS [NO2_IAQI]
                        ,MIN(dbo.F_ValidValueStr([SO2_IAQI])) AS [SO2_IAQI]
                        ,MIN(dbo.F_ValidValueStr([CO_IAQI])) AS [CO_IAQI]
                        ,MIN(dbo.F_ValidValueStr([MaxOneHourO3_IAQI])) AS [MaxOneHourO3_IAQI]
                        ,MIN(dbo.F_ValidValueStr([Max8HourO3_IAQI])) AS [Max8HourO3_IAQI]
                        ,MIN(dbo.F_ValidValueStr([AQIValue])) AS [AQIValue]
                    FROM {0}
                    WHERE ReportDateTime >= '{1}' and ReportDateTime <= '{2}' AND StatisticalType='{3}' {4}
                    ", SmartEP.Data.Enums.EnumMapping.GetTableName(AQIDataType.RegionDayAQI), dateStart.ToString("yyyy-MM-dd HH:mm:ss")
                                                  , dateEnd.ToString("yyyy-MM-dd HH:mm:ss"), SmartEP.Core.Enums.EnumMapping.GetDesc(regionAQIStatisticalType), regionsStr);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 日数据超标天数统计
        /// </summary>
        /// <param name="regions">站点</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <returns>
        /// 返回字段：
        /// PointId：站点ID
        /// ValidCount：有效天
        /// OverCount：超标天
        /// PM25_Over：PM25超标天
        /// PM10_Over：PM10超标天
        /// NO2_Over：NO2超标天
        /// SO2_Over：SO2超标天
        /// CO_Over：CO超标天
        /// MaxOneHourO3_Over：MaxOneHourO3超标天
        /// Max8HourO3_Over：Max8HourO3超标天
        /// </returns>
        public DataView GetExceedingData(string[] regions, DateTime dateStart, DateTime dateEnd, RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            try
            {
                string regionsStr = "'" + StringExtensions.GetArrayStrNoEmpty(regions.ToList<string>(), "','") + "'";
                if (regions.Length > 0) regionsStr = string.Format(" and MonitoringRegionUid IN ({0}) ", regionsStr);
                string sql = string.Format(@"
                    SELECT MonitoringRegionUid
	                    ,SUM(CASE WHEN [AQIValue] >=0 THEN 1 ELSE 0 END) AS ValidCount
	                    ,SUM(CASE WHEN [AQIValue] > 100 THEN 1 ELSE 0 END) AS OverCount
                        ,SUM(CASE WHEN [PM25_IAQI] > 100 THEN 1 ELSE 0 END) AS PM25_Over
                        ,SUM(CASE WHEN [PM10_IAQI] > 100 THEN 1 ELSE 0 END) AS PM10_Over
                        ,SUM(CASE WHEN [NO2_IAQI] > 100 THEN 1 ELSE 0 END) AS NO2_Over
                        ,SUM(CASE WHEN [SO2_IAQI] > 100 THEN 1 ELSE 0 END) AS SO2_Over
                        ,SUM(CASE WHEN [CO_IAQI] > 100 THEN 1 ELSE 0 END) AS CO_Over
                        ,SUM(CASE WHEN [MaxOneHourO3_IAQI] > 100 THEN 1 ELSE 0 END) AS MaxOneHourO3_Over
                        ,SUM(CASE WHEN [Max8HourO3_IAQI] > 100 THEN 1 ELSE 0 END) AS Max8HourO3_Over
                    FROM {0}
                    WHERE ReportDateTime >= '{1}' and ReportDateTime <= '{2}' AND StatisticalType='{3}' {4} 
                    GROUP BY MonitoringRegionUid ", SmartEP.Data.Enums.EnumMapping.GetTableName(AQIDataType.RegionDayAQI), dateStart.ToString("yyyy-MM-dd HH:mm:ss")
                                                  , dateEnd.ToString("yyyy-MM-dd HH:mm:ss"), SmartEP.Core.Enums.EnumMapping.GetDesc(regionAQIStatisticalType), regionsStr);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 日数据超标天数统计
        /// </summary>
        /// <param name="regions">站点</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <returns>
        /// 返回字段：
        /// PointId：站点ID
        /// ValidCount：有效天
        /// OverCount：超标天
        /// PM25_Over：PM25超标天
        /// PM10_Over：PM10超标天
        /// NO2_Over：NO2超标天
        /// SO2_Over：SO2超标天
        /// CO_Over：CO超标天
        /// MaxOneHourO3_Over：MaxOneHourO3超标天
        /// Max8HourO3_Over：Max8HourO3超标天
        /// </returns>
        public DataView GetExceedingMonthData(string[] regions, DateTime dateStart, DateTime dateEnd, RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            try
            {
                string regionsStr = "'" + StringExtensions.GetArrayStrNoEmpty(regions.ToList<string>(), "','") + "'";
                if (regions.Length > 0) regionsStr = string.Format(" and MonitoringRegionUid IN ({0}) ", regionsStr);
                string sql = string.Format(@"
                    SELECT MonitoringRegionUid
	                    ,SUM(CASE WHEN [AQIValue] >=0 THEN 1 ELSE 0 END) AS ValidCount
	                    ,SUM(CASE WHEN [AQIValue] >= 0 and [AQIValue] <= 100 THEN 1 ELSE 0 END) AS StandardCount
                        ,SUM(CASE WHEN [PM25_IAQI] > 100 THEN 1 ELSE 0 END) AS PM25_Over
                        ,SUM(CASE WHEN [PM10_IAQI] > 100 THEN 1 ELSE 0 END) AS PM10_Over
                        ,SUM(CASE WHEN [NO2_IAQI] > 100 THEN 1 ELSE 0 END) AS NO2_Over
                        ,SUM(CASE WHEN [SO2_IAQI] > 100 THEN 1 ELSE 0 END) AS SO2_Over
                        ,SUM(CASE WHEN [CO_IAQI] > 100 THEN 1 ELSE 0 END) AS CO_Over
                        ,SUM(CASE WHEN [MaxOneHourO3_IAQI] > 100 THEN 1 ELSE 0 END) AS MaxOneHourO3_Over
                        ,SUM(CASE WHEN [Max8HourO3_IAQI] > 100 THEN 1 ELSE 0 END) AS Max8HourO3_Over
                    FROM {0}
                    WHERE ReportDateTime >= '{1}' and ReportDateTime <= '{2}' AND StatisticalType='{3}' {4} 
                    GROUP BY MonitoringRegionUid ", SmartEP.Data.Enums.EnumMapping.GetTableName(AQIDataType.RegionDayAQI), dateStart.ToString("yyyy-MM-dd HH:mm:ss")
                                                  , dateEnd.ToString("yyyy-MM-dd HH:mm:ss"), SmartEP.Core.Enums.EnumMapping.GetDesc(regionAQIStatisticalType), regionsStr);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 年报
        /// </summary>
        /// <param name="regions">站点</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <returns>
        public DataView GetRegionsAllData(string[] regions, DateTime dateStart, DateTime dateEnd, RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            try
            {
                string regionsStr = "'" + StringExtensions.GetArrayStrNoEmpty(regions.ToList<string>(), "','") + "'";
                if (regions.Length > 0) regionsStr = string.Format(" and MonitoringRegionUid IN ({0}) ", regionsStr);
                string sql = string.Format(@"
                    SELECT MonitoringRegionUid
						,AVG(CONVERT (decimal(18,6),[PM25])) as a34004
                        ,AVG(CONVERT (decimal(18,6),[PM10])) as a34002
                        ,AVG(CONVERT (decimal(18,6),SO2)) as a21026
                        ,AVG(CONVERT (decimal(18,6),NO2)) as a21004
                        ,AVG(CONVERT (decimal(18,6),CO)) as a21005
                        ,AVG(CONVERT (decimal(18,6),Max8HourO3)) as a05024
						,SUM(CASE WHEN [AQIValue] >0 THEN 1 ELSE 0 END) AS ValidCount
                        ,SUM(CASE WHEN [AQIValue] >0 and [AQIValue] <= 100 THEN 1 ELSE 0 END) AS StandardCount
						,SUM(CASE WHEN [AQIValue] >0 and [AQIValue] <= 50 THEN 1 ELSE 0 END) AS Optimal
						,SUM(CASE WHEN [AQIValue] >= 51 and [AQIValue] <= 100 THEN 1 ELSE 0 END) AS benign
						,SUM(CASE WHEN [AQIValue] >=101 THEN 1 ELSE 0 END) AS ExcessiveCount
						,SUM(CASE WHEN [AQIValue] >= 101 and [AQIValue] <= 150 THEN 1 ELSE 0 END) AS LightPollution
						,SUM(CASE WHEN [AQIValue] >= 151 and [AQIValue] <= 200 THEN 1 ELSE 0 END) AS ModeratePollution
						,SUM(CASE WHEN [AQIValue] >= 201 and [AQIValue] <= 300 THEN 1 ELSE 0 END) AS HighPollution
						,SUM(CASE WHEN [AQIValue] >= 301  THEN 1 ELSE 0 END) AS SeriousPollution
                        ,SUM(CASE WHEN [Max8HourO3_IAQI]>0  THEN 1 ELSE 0 END) AS O3AQICount
                        ,SUM(CASE WHEN [Max8HourO3_IAQI] >= 101  THEN 1 ELSE 0 END) AS O3AQI
						,SUM(CASE WHEN [PrimaryPollutant] like '%PM2.5%'  THEN 1 ELSE 0 END) AS PM25
						,SUM(CASE WHEN [PrimaryPollutant] like '%PM10%'  THEN 1 ELSE 0 END) AS PM10
						,SUM(CASE WHEN [PrimaryPollutant] like '%O3%'  THEN 1 ELSE 0 END) AS O3
						,SUM(CASE WHEN [PrimaryPollutant] like '%SO2%'  THEN 1 ELSE 0 END) AS SO2
						,SUM(CASE WHEN [PrimaryPollutant] like '%NO2%'  THEN 1 ELSE 0 END) AS NO2
						,SUM(CASE WHEN [PrimaryPollutant] like '%CO%'  THEN 1 ELSE 0 END) AS CO
                    FROM {0}
                    WHERE ReportDateTime >= '{1}' and ReportDateTime <= '{2}' AND StatisticalType='{3}' {4}
                    GROUP BY MonitoringRegionUid ", SmartEP.Data.Enums.EnumMapping.GetTableName(AQIDataType.RegionDayAQI), dateStart.ToString("yyyy-MM-dd HH:mm:ss")
                                                  , dateEnd.ToString("yyyy-MM-dd HH:mm:ss"), SmartEP.Core.Enums.EnumMapping.GetDesc(regionAQIStatisticalType), regionsStr);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 年报
        /// </summary>
        /// <param name="regions">站点</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <returns>
        public DataView GetMonthAvgAllData(DateTime dateStart, DateTime dateEnd, RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            try
            {
                string str = "";
                for (int i = dateStart.Month; i <= dateEnd.Month; i++)
                {
                    str += "CASE WHEN SUM(CASE WHEN [AQIValue] >= 0 THEN 1 ELSE 0 END)=0 THEN '--' ELSE " +
                                "dbo.F_Round(SUM(CASE WHEN [AQIValue] >0 AND [AQIValue] <= 100 and Convert(varchar(2),datepart(MONTH,[ReportDateTime]))=" + i +
                                " THEN 1 ELSE 0 END)*100.0/SUM(CASE WHEN [AQIValue] > 0 " + " and Convert(varchar(2),datepart(MONTH,[ReportDateTime]))=" + i + " THEN 1 ELSE 0 END),1) END AS '" + i + "月',";
                }
                str = str.Trim(',');
                string sql = string.Format(@"SELECT {4}
                    FROM {0}
                    WHERE ReportDateTime >= '{1}' and ReportDateTime <= '{2}' AND StatisticalType='{3}' AND [MonitoringRegionUid] in
                           (
                             '7e05b94c-bbd4-45c3-919c-42da2e63fd43'                 --市区
		                )", SmartEP.Data.Enums.EnumMapping.GetTableName(AQIDataType.RegionDayAQI), dateStart.ToString("yyyy-MM-dd HH:mm:ss")
                                                  , dateEnd.ToString("yyyy-MM-dd HH:mm:ss"), SmartEP.Core.Enums.EnumMapping.GetDesc(regionAQIStatisticalType), str);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 获取时间段内的所有区域的数据
        /// </summary>
        /// <param name="regions">站点</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <returns>
        public DataView GetRegionsAllDataByDate(DateTime dateStart, DateTime dateEnd, RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            try
            {
                string sql = string.Format(@"
                    SELECT MonitoringRegionUid
						,AVG(CONVERT (decimal(18,6),[PM25])) as a34004
                        ,AVG(CONVERT (decimal(18,6),[PM10])) as a34002
                        ,AVG(CONVERT (decimal(18,6),SO2)) as a21026
                        ,AVG(CONVERT (decimal(18,6),NO2)) as a21004
                        ,AVG(CONVERT (decimal(18,6),CO)) as a21005
                        ,AVG(CONVERT (decimal(18,6),Max8HourO3)) as a05024
						,SUM(CASE WHEN [AQIValue] > 0 THEN 1 ELSE 0 END) AS ValidCount
                        ,SUM(CASE WHEN [AQIValue] > 0 and [AQIValue] <= 100 THEN 1 ELSE 0 END) AS StandardCount
						,SUM(CASE WHEN [AQIValue] > 0 and [AQIValue] <= 50 THEN 1 ELSE 0 END) AS Optimal
						,SUM(CASE WHEN [AQIValue] >= 51 and [AQIValue] <= 100 THEN 1 ELSE 0 END) AS benign
						,SUM(CASE WHEN [AQIValue] >= 101 THEN 1 ELSE 0 END) AS ExcessiveCount
						,SUM(CASE WHEN [AQIValue] >= 101 and [AQIValue] <= 150 THEN 1 ELSE 0 END) AS LightPollution
						,SUM(CASE WHEN [AQIValue] >= 151 and [AQIValue] <= 200 THEN 1 ELSE 0 END) AS ModeratePollution
						,SUM(CASE WHEN [AQIValue] >= 201 and [AQIValue] <= 300 THEN 1 ELSE 0 END) AS HighPollution
						,SUM(CASE WHEN [AQIValue] >= 301  THEN 1 ELSE 0 END) AS SeriousPollution
						,SUM(CASE WHEN [PrimaryPollutant] like '%PM2.5%' and AQIValue>100  THEN 1 ELSE 0 END) AS PM25
						,SUM(CASE WHEN [PrimaryPollutant] like '%PM10%' and AQIValue>100 THEN 1 ELSE 0 END) AS PM10
						,SUM(CASE WHEN [PrimaryPollutant] like '%O3%' and AQIValue>100 THEN 1 ELSE 0 END) AS O3
						,SUM(CASE WHEN [PrimaryPollutant] like '%SO2%' and AQIValue>100 THEN 1 ELSE 0 END) AS SO2
						,SUM(CASE WHEN [PrimaryPollutant] like '%NO2%' and AQIValue>100 THEN 1 ELSE 0 END) AS NO2
						,SUM(CASE WHEN [PrimaryPollutant] like '%CO%' and AQIValue>100  THEN 1 ELSE 0 END) AS CO

						,SUM(CASE WHEN [PM25_IAQI] > 0 THEN 1 ELSE 0 END) AS PM25_ValidCount
                        ,SUM(CASE WHEN [PM25_IAQI] > 0 and [AQIValue] <= 100 THEN 1 ELSE 0 END) AS PM25_StandardCount
                        ,SUM(CASE WHEN [PM25_IAQI] >= 101 THEN 1 ELSE 0 END) AS PM25_OverCount

						,SUM(CASE WHEN [PM10_IAQI] > 0 THEN 1 ELSE 0 END) AS PM10_ValidCount
                        ,SUM(CASE WHEN [PM10_IAQI] > 0 and [AQIValue] <= 100 THEN 1 ELSE 0 END) AS PM10_StandardCount
                        ,SUM(CASE WHEN [PM10_IAQI] >= 101 THEN 1 ELSE 0 END) AS PM10_OverCount

						,SUM(CASE WHEN [Max8HourO3_IAQI] >=0 THEN 1 ELSE 0 END) AS O3_ValidCount
                        ,SUM(CASE WHEN [Max8HourO3_IAQI] >=0 and [AQIValue] <= 100 THEN 1 ELSE 0 END) AS O3_StandardCount
                        ,SUM(CASE WHEN [Max8HourO3_IAQI] >= 101 THEN 1 ELSE 0 END) AS O3_OverCount

						,SUM(CASE WHEN [SO2_IAQI] > 0 THEN 1 ELSE 0 END) AS SO2_ValidCount
                        ,SUM(CASE WHEN [SO2_IAQI] > 0 and [AQIValue] <= 100 THEN 1 ELSE 0 END) AS SO2_StandardCount
                        ,SUM(CASE WHEN [SO2_IAQI] >= 101 THEN 1 ELSE 0 END) AS SO2_OverCount

						,SUM(CASE WHEN [NO2_IAQI] > 0 THEN 1 ELSE 0 END) AS NO2_ValidCount
                        ,SUM(CASE WHEN [NO2_IAQI] > 0 and [AQIValue] <= 100 THEN 1 ELSE 0 END) AS NO2_StandardCount
                        ,SUM(CASE WHEN [NO2_IAQI] >= 101 THEN 1 ELSE 0 END) AS NO2_OverCount

						,SUM(CASE WHEN [CO_IAQI] > 0 THEN 1 ELSE 0 END) AS CO_ValidCount
                        ,SUM(CASE WHEN [CO_IAQI] > 0 and [AQIValue] <= 100 THEN 1 ELSE 0 END) AS CO_StandardCount
                        ,SUM(CASE WHEN [CO_IAQI] >= 101 THEN 1 ELSE 0 END) AS CO_OverCount
                    FROM {0}
                    WHERE ReportDateTime >= '{1}' and ReportDateTime <= '{2}' AND StatisticalType='{3}' AND [MonitoringRegionUid] in
                           (
                             '7e05b94c-bbd4-45c3-919c-42da2e63fd43'                 --市区
			                ,'66d2abd1-ca39-4e39-909f-da872704fbfd'					--张家港市
                            ,'d7d7a1fe-493a-4b3f-8504-b1850f6d9eff'					--常熟市
			                ,'57b196ed-5038-4ad0-a035-76faee2d7a98'					--太仓市
			                ,'2e2950cd-dbab-43b3-811d-61bd7569565a'					--昆山市
			                ,'2fea3cb2-8b95-45e6-8a71-471562c4c89c'					--吴江区
                            ,'e1c104f3-aaf3-4d0e-9591-36cdc83be15a'					--吴中区
			                ,'8756bd44-ff18-46f7-aedf-615006d7474c'					--相城区
			                ,'6a4e7093-f2c6-46b4-a11f-0f91b4adf379'					--姑苏区
			                ,'69a993ff-78c6-459b-9322-ee77e0c8cd68'					--工业园区
			                ,'f320aa73-7c55-45d4-a363-e21408e0aac3'					--高新区
                            ,'5a566145-4884-453c-93ad-16e4344c85c9'                 --全市
		                )
                    GROUP BY MonitoringRegionUid ", SmartEP.Data.Enums.EnumMapping.GetTableName(AQIDataType.RegionDayAQI), dateStart.ToString("yyyy-MM-dd HH:mm:ss")
                                                  , dateEnd.ToString("yyyy-MM-dd HH:mm:ss"), SmartEP.Core.Enums.EnumMapping.GetDesc(regionAQIStatisticalType));
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 获取时间段内的所有区域的数据
        /// </summary>
        /// <param name="regions">站点</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <returns>
        public DataView GetPointsAllDataByDate(DateTime dateStart, DateTime dateEnd)
        {
            try
            {
                string sql = string.Format(@"
                    SELECT PointId
						,AVG(CONVERT (decimal(18,6),[PM25])) as a34004
                        ,AVG(CONVERT (decimal(18,6),[PM10])) as a34002
                        ,AVG(CONVERT (decimal(18,6),SO2)) as a21026
                        ,AVG(CONVERT (decimal(18,6),NO2)) as a21004
                        ,AVG(CONVERT (decimal(18,6),CO)) as a21005
                        ,AVG(CONVERT (decimal(18,6),Max8HourO3)) as a05024
						,SUM(CASE WHEN [AQIValue] > 0 THEN 1 ELSE 0 END) AS ValidCount
                        ,SUM(CASE WHEN [AQIValue] > 0 and [AQIValue] <= 100 THEN 1 ELSE 0 END) AS StandardCount
						,SUM(CASE WHEN [AQIValue] > 0 and [AQIValue] <= 50 THEN 1 ELSE 0 END) AS Optimal
						,SUM(CASE WHEN [AQIValue] >= 51 and [AQIValue] <= 100 THEN 1 ELSE 0 END) AS benign
						,SUM(CASE WHEN [AQIValue] >= 101 THEN 1 ELSE 0 END) AS ExcessiveCount
						,SUM(CASE WHEN [AQIValue] >= 101 and [AQIValue] <= 150 THEN 1 ELSE 0 END) AS LightPollution
						,SUM(CASE WHEN [AQIValue] >= 151 and [AQIValue] <= 200 THEN 1 ELSE 0 END) AS ModeratePollution
						,SUM(CASE WHEN [AQIValue] >= 201 and [AQIValue] <= 300 THEN 1 ELSE 0 END) AS HighPollution
						,SUM(CASE WHEN [AQIValue] >= 301  THEN 1 ELSE 0 END) AS SeriousPollution
						,SUM(CASE WHEN [PrimaryPollutant] like '%PM2.5%' and AQIValue>100  THEN 1 ELSE 0 END) AS PM25
						,SUM(CASE WHEN [PrimaryPollutant] like '%PM10%' and AQIValue>100 THEN 1 ELSE 0 END) AS PM10
						,SUM(CASE WHEN [PrimaryPollutant] like '%O3%' and AQIValue>100 THEN 1 ELSE 0 END) AS O3
						,SUM(CASE WHEN [PrimaryPollutant] like '%SO2%' and AQIValue>100 THEN 1 ELSE 0 END) AS SO2
						,SUM(CASE WHEN [PrimaryPollutant] like '%NO2%' and AQIValue>100 THEN 1 ELSE 0 END) AS NO2
						,SUM(CASE WHEN [PrimaryPollutant] like '%CO%' and AQIValue>100  THEN 1 ELSE 0 END) AS CO

						,SUM(CASE WHEN [PM25_IAQI] > 0 THEN 1 ELSE 0 END) AS PM25_ValidCount
                        ,SUM(CASE WHEN [PM25_IAQI] > 0 and [AQIValue] <= 100 THEN 1 ELSE 0 END) AS PM25_StandardCount
                        ,SUM(CASE WHEN [PM25_IAQI] >= 101 THEN 1 ELSE 0 END) AS PM25_OverCount

						,SUM(CASE WHEN [PM10_IAQI] > 0 THEN 1 ELSE 0 END) AS PM10_ValidCount
                        ,SUM(CASE WHEN [PM10_IAQI] > 0 and [AQIValue] <= 100 THEN 1 ELSE 0 END) AS PM10_StandardCount
                        ,SUM(CASE WHEN [PM10_IAQI] >= 101 THEN 1 ELSE 0 END) AS PM10_OverCount

						,SUM(CASE WHEN [Max8HourO3_IAQI] >=0 THEN 1 ELSE 0 END) AS O3_ValidCount
                        ,SUM(CASE WHEN [Max8HourO3_IAQI] >=0 and [AQIValue] <= 100 THEN 1 ELSE 0 END) AS O3_StandardCount
                        ,SUM(CASE WHEN [Max8HourO3_IAQI] >= 101 THEN 1 ELSE 0 END) AS O3_OverCount

						,SUM(CASE WHEN [SO2_IAQI] > 0 THEN 1 ELSE 0 END) AS SO2_ValidCount
                        ,SUM(CASE WHEN [SO2_IAQI] > 0 and [AQIValue] <= 100 THEN 1 ELSE 0 END) AS SO2_StandardCount
                        ,SUM(CASE WHEN [SO2_IAQI] >= 101 THEN 1 ELSE 0 END) AS SO2_OverCount

						,SUM(CASE WHEN [NO2_IAQI] > 0 THEN 1 ELSE 0 END) AS NO2_ValidCount
                        ,SUM(CASE WHEN [NO2_IAQI] > 0 and [AQIValue] <= 100 THEN 1 ELSE 0 END) AS NO2_StandardCount
                        ,SUM(CASE WHEN [NO2_IAQI] >= 101 THEN 1 ELSE 0 END) AS NO2_OverCount

						,SUM(CASE WHEN [CO_IAQI] > 0 THEN 1 ELSE 0 END) AS CO_ValidCount
                        ,SUM(CASE WHEN [CO_IAQI] > 0 and [AQIValue] <= 100 THEN 1 ELSE 0 END) AS CO_StandardCount
                        ,SUM(CASE WHEN [CO_IAQI] >= 101 THEN 1 ELSE 0 END) AS CO_OverCount
                    FROM {0}
                    WHERE DateTime >= '{1}' and DateTime <= '{2}' AND PointId in
                           (1,2,3,4,5,6,7)
                    GROUP BY PointId ", SmartEP.Data.Enums.EnumMapping.GetTableName(AQIDataType.DayAQI), dateStart.ToString("yyyy-MM-dd HH:mm:ss")
                                                  , dateEnd.ToString("yyyy-MM-dd HH:mm:ss"));
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 年报
        /// </summary>
        /// <param name="regions">站点</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <returns>
        public DataView GetRegionsSeasonData(DateTime dateStart, DateTime dateEnd, RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            try
            {
                string str = "";
                for (int i = 1; i <= dateEnd.Month; i++)
                {
                    str += " ,SUM(CASE WHEN [AQIValue] >100 and Convert(varchar(2),datepart(MONTH,[ReportDateTime]))=" + i + " THEN 1 ELSE 0 END) AS '" + i + "月'";
                }
                string sql = string.Format(@" select  a.MonitoringRegionUid {4}
								,Min(AQIValue) as AQIMin
								,Max(AQIValue) as AQIMax
								,SUM(CASE WHEN [AQIValue] >=0 THEN 1 ELSE 0 END) AS ValidCount
								,SUM(CASE WHEN [AQIValue] >=0 and [AQIValue] <= 100 THEN 1 ELSE 0 END) AS StandardCount
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
								,AVG(PM25)*1000 AS PM25Avg
								,AVG([PM10])*1000 AS PM10Avg
								,AVG([SO2])*1000 AS SO2Avg
								,AVG([NO2])*1000 AS NO2Avg
								,AVG([MaxOneO3])*1000 AS MaxOneO3Avg
								,AVG([MaxO3])*1000 AS O3Avg
								,AVG([CO]) AS COAvg
								from
								(
								select MonitoringRegionUid
								,[ReportDateTime]
								,CONVERT (decimal(18,6),[SO2]) as [SO2]
								,CONVERT (decimal(18,6),[NO2]) as [NO2]
								,CONVERT (decimal(18,6),[CO]) as [CO]
								,CONVERT (decimal(18,6),MaxOneHourO3) as [MaxOneO3]
								,CONVERT (decimal(18,6),Max8HourO3) as [MaxO3]
								,CONVERT (decimal(18,6),[PM10]) as [PM10]
								,CONVERT (decimal(18,6),PM25) as PM25
								,CONVERT (decimal(18,6),AQIValue) as AQIValue
								,[PrimaryPollutant]
								  FROM {0}
								where  MonitoringRegionUid='7e05b94c-bbd4-45c3-919c-42da2e63fd43'
                                      and [ReportDateTime]>='{1}' and [ReportDateTime]<='{2}' AND StatisticalType='{3}'
								) a
								group by MonitoringRegionUid ", SmartEP.Data.Enums.EnumMapping.GetTableName(AQIDataType.RegionDayAQI), dateStart.ToString("yyyy-MM-dd HH:mm:ss")
                                                  , dateEnd.ToString("yyyy-MM-dd HH:mm:ss"), SmartEP.Core.Enums.EnumMapping.GetDesc(regionAQIStatisticalType), str);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 日数据超标天数统计(全市均值)
        /// </summary>
        /// <param name="regions">站点</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <returns>
        /// 返回字段：
        /// ValidCount：有效天
        /// OverCount：超标天
        /// PM25_Over：PM25超标天
        /// PM10_Over：PM10超标天
        /// NO2_Over：NO2超标天
        /// SO2_Over：SO2超标天
        /// CO_Over：CO超标天
        /// MaxOneHourO3_Over：MaxOneHourO3超标天
        /// Max8HourO3_Over：Max8HourO3超标天
        /// </returns>
        public DataView GetMonthData(string[] regions, DateTime dateStart, DateTime dateEnd, RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            try
            {
                string regionsStr = "'" + StringExtensions.GetArrayStrNoEmpty(regions.ToList<string>(), "','") + "'";
                if (regions.Length > 0) regionsStr = string.Format(" and MonitoringRegionUid IN ({0}) ", regionsStr);
                string sql = string.Format(@"
                         select  SUM(CASE WHEN [AQIValue] >=0 THEN 1 ELSE 0 END) AS ValidCount
	                    ,SUM(CASE WHEN [AQIValue] >= 0 and [AQIValue] <= 100 THEN 1 ELSE 0 END) AS StandardCount
                        ,SUM(CASE WHEN [PM25AQI] > 100 THEN 1 ELSE 0 END) AS PM25_Over
                        ,SUM(CASE WHEN [PM10AQI] > 100 THEN 1 ELSE 0 END) AS PM10_Over
                        ,SUM(CASE WHEN [NO2AQI] > 100 THEN 1 ELSE 0 END) AS NO2_Over
                        ,SUM(CASE WHEN [SO2AQI] > 100 THEN 1 ELSE 0 END) AS SO2_Over
                        ,SUM(CASE WHEN [COAQI] > 100 THEN 1 ELSE 0 END) AS CO_Over
                        ,SUM(CASE WHEN [MaxOneHourO3AQI] > 100 THEN 1 ELSE 0 END) AS MaxOneHourO3_Over
                        ,SUM(CASE WHEN [Max8HourO3AQI] > 100 THEN 1 ELSE 0 END) AS Max8HourO3_Over
                      from
                       (
                        SELECT [ReportDateTime],
                               AVG(CONVERT (decimal(18,0),[AQIValue])) as AQIValue,
                               AVG(CONVERT (decimal(18,0),[SO2_IAQI])) as [SO2AQI],
                               AVG(CONVERT (decimal(18,0),[PM10_IAQI])) as [PM10AQI],
                               AVG(CONVERT (decimal(18,0),[CO_IAQI])) as [COAQI],
                               AVG(CONVERT (decimal(18,0),[Max8HourO3_IAQI])) as [Max8HourO3AQI],
                               AVG(CONVERT (decimal(18,0),[MaxOneHourO3_IAQI])) as [MaxOneHourO3AQI],
                               AVG(CONVERT (decimal(18,0),[PM25_IAQI])) as [PM25AQI],
                               AVG(CONVERT (decimal(18,0),[NO2_IAQI])) as [NO2AQI]
                               FROM {0}
                               where [ReportDateTime]>='{1}' AND [ReportDateTime]<='{2}' and StatisticalType='{3}' {4} and AQIValue is not null and [SO2_IAQI] is not null 
                               and [PM10_IAQI] is not null  and [CO_IAQI] is not null  and [Max8HourO3_IAQI] is not null  and [PM25_IAQI] is not null  and [NO2_IAQI] is not null and [MaxOneHourO3_IAQI] is not null
                               group by  [ReportDateTime]
                 ) a", SmartEP.Data.Enums.EnumMapping.GetTableName(AQIDataType.RegionDayAQI), dateStart.ToString("yyyy-MM-dd HH:mm:ss")
                                                  , dateEnd.ToString("yyyy-MM-dd HH:mm:ss"), SmartEP.Core.Enums.EnumMapping.GetDesc(regionAQIStatisticalType), regionsStr);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 年报
        /// </summary>
        /// <param name="regions">站点</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <returns>
        public DataView GetRegionsHalfYearData(DateTime dateStart, DateTime dateEnd, RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            try
            {
                string sql = string.Format(@"SELECT [ReportDateTime]
                                ,AVG(CONVERT (decimal(18,6),PM25)) as a34004
                                ,AVG(CONVERT (decimal(18,6),PM10)) as a34002
                                ,AVG(CONVERT (decimal(18,6),SO2)) as a21026
                                ,AVG(CONVERT (decimal(18,6),NO2)) as a21004
                                ,AVG(CONVERT (decimal(18,6),CO)) as a21005
                                ,AVG(CONVERT (decimal(18,6),Max8HourO3)) as a05024
								FROM {0}
								where [ReportDateTime]>='{1}' AND [ReportDateTime]<='{2}' and StatisticalType='{3}'  
								and [MonitoringRegionUid] in (
								'7e05b94c-bbd4-45c3-919c-42da2e63fd43'	                --市区
								,'66d2abd1-ca39-4e39-909f-da872704fbfd'					--张家港市
                                ,'d7d7a1fe-493a-4b3f-8504-b1850f6d9eff'					--常熟市
			                    ,'57b196ed-5038-4ad0-a035-76faee2d7a98'					--太仓市
			                    ,'2e2950cd-dbab-43b3-811d-61bd7569565a'					--昆山市
			                    ,'2fea3cb2-8b95-45e6-8a71-471562c4c89c'					--吴江区
								)
								group by  [ReportDateTime]", SmartEP.Data.Enums.EnumMapping.GetTableName(AQIDataType.RegionDayAQI), dateStart.ToString("yyyy-MM-dd HH:mm:ss")
                                                  , dateEnd.ToString("yyyy-MM-dd HH:mm:ss"), SmartEP.Core.Enums.EnumMapping.GetDesc(regionAQIStatisticalType));
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 年报
        /// </summary>
        /// <param name="regions">站点</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <returns>
        public DataView GetRegionsYearAQIData(DateTime dateStart, DateTime dateEnd, RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            try
            {
                string sql = string.Format(@"select   
                                AVG(CONVERT (decimal(18,6),PM25)) as a34004
                                ,AVG(CONVERT (decimal(18,6),PM10)) as a34002
                                ,AVG(CONVERT (decimal(18,6),SO2)) as a21026
                                ,AVG(CONVERT (decimal(18,6),NO2)) as a21004
                                ,AVG(CONVERT (decimal(18,6),CO)) as a21005
                                ,AVG(CONVERT (decimal(18,6),O3)) as a05024
                                from
                                (
                                SELECT [ReportDateTime]
                                ,AVG(CONVERT (decimal(18,6),[PM25])) as PM25
                                ,AVG(CONVERT (decimal(18,6),[PM10])) as PM10
                                ,AVG(CONVERT (decimal(18,6),[SO2])) as SO2
                                ,AVG(CONVERT (decimal(18,6),[NO2])) as NO2
                                ,AVG(CONVERT (decimal(18,6),[CO])) as CO
                                ,AVG(CONVERT (decimal(18,6),[Max8HourO3])) as O3
								FROM {0}
								where [ReportDateTime]>='{1}' AND [ReportDateTime]<='{2}' and StatisticalType='{3}'  
								and [MonitoringRegionUid] in (
								'7e05b94c-bbd4-45c3-919c-42da2e63fd43'	                --市区
								,'66d2abd1-ca39-4e39-909f-da872704fbfd'					--张家港市
                                ,'d7d7a1fe-493a-4b3f-8504-b1850f6d9eff'					--常熟市
			                    ,'57b196ed-5038-4ad0-a035-76faee2d7a98'					--太仓市
			                    ,'2e2950cd-dbab-43b3-811d-61bd7569565a'					--昆山市
			                    ,'2fea3cb2-8b95-45e6-8a71-471562c4c89c'					--吴江区
								)
								group by  [ReportDateTime]
								) a", SmartEP.Data.Enums.EnumMapping.GetTableName(AQIDataType.RegionDayAQI), dateStart.ToString("yyyy-MM-dd HH:mm:ss")
                                                  , dateEnd.ToString("yyyy-MM-dd HH:mm:ss"), SmartEP.Core.Enums.EnumMapping.GetDesc(regionAQIStatisticalType));
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 全市均值达标天数统计
        /// </summary>
        /// <param name="regions">站点</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <returns>
        /// 返回字段：
        /// ValidCount：有效天
        /// OverCount：达标天
        /// </returns>
        public DataView GetMonthReportData(string[] regions, DateTime dateStart, DateTime dateEnd, RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            try
            {
                string regionsStr = "'" + StringExtensions.GetArrayStrNoEmpty(regions.ToList<string>(), "','") + "'";
                if (regions.Length > 0) regionsStr = string.Format(" and MonitoringRegionUid IN ({0}) ", regionsStr);
                string sql = string.Format(@"
                         select  SUM(CASE WHEN [AQIValue] >=0 THEN 1 ELSE 0 END) AS ValidCount
	                    ,SUM(CASE WHEN [AQIValue] >= 0 and [AQIValue] <= 100 THEN 1 ELSE 0 END) AS StandardCount
                      from
                       (
                        SELECT [ReportDateTime],
                               AVG(CONVERT (decimal(18,0),[AQIValue])) as AQIValue
                             
                               FROM {0}
                               where [ReportDateTime]>='{1}' AND [ReportDateTime]<='{2}' and StatisticalType='{3}' {4} and AQIValue is not null 
                               group by  [ReportDateTime]
                 ) a", SmartEP.Data.Enums.EnumMapping.GetTableName(AQIDataType.RegionDayAQI), dateStart.ToString("yyyy-MM-dd HH:mm:ss")
                                                  , dateEnd.ToString("yyyy-MM-dd HH:mm:ss"), SmartEP.Core.Enums.EnumMapping.GetDesc(regionAQIStatisticalType), regionsStr);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 取得制定日期内指定因子平均浓度及达标天数
        /// </summary>
        /// <param name="factorCode">因子</param>
        /// <param name="num">因子放大倍数</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <returns>
        /// 返回字段：
        /// RegionName：区域名称
        /// DBRate：达标天数
        /// factorCode：因子平均浓度
        /// </returns>
        public DataView GetVillageWeekRepSource(string factorCode, int num, DateTime dateStart, DateTime dateEnd)
        {
            try
            {
                string sql = string.Empty;
                sql = string.Format(@"select v.itemtext as RegionName,
                            CASE WHEN SUM(CASE WHEN [AQIValue] >= 0 THEN 1 ELSE 0 END)=0 THEN '--' ELSE 
			                dbo.F_Round(SUM(CASE WHEN [AQIValue] >= 0 AND [AQIValue] <= 100 THEN 1 ELSE 0 END)*100.0/
			                SUM(CASE WHEN [AQIValue] >= 0 THEN 1 ELSE 0 END),1) END AS DBRate,
                                dbo.F_Round(AVG(dbo.decFactorValueStr2Dec({0}))*{3},1) as {0} from AirReport.TB_RegionDayAQIReport  
                                left join dbo.SY_V_CodeDictionary as v on v.itemguid = AirReport.TB_RegionDayAQIReport.[MonitoringRegionUid]
                                 where [MonitoringRegionUid] in (
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
		                ) and ReportDateTime >= '{1}' and ReportDateTime <= '{2}'
                            GROUP BY [MonitoringRegionUid],v.itemtext", factorCode, dateStart, dateEnd, num);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 取得制定日期内指定因子平均浓度及达标天数
        /// </summary>
        /// <param name="factorCode">因子</param>
        /// <param name="num">因子放大倍数</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <returns>
        /// 返回字段：
        /// RegionName：区域名称
        /// DBRate：达标天数
        /// factorCode：因子平均浓度
        /// </returns>
        public DataView GetVillageMonthBaseSource(string factorCode, int num, DateTime dateStart, DateTime dateEnd)
        {
            try
            {
                string sql = string.Empty;
                sql = string.Format(@"select v.itemtext as RegionName,
                            CASE WHEN SUM(CASE WHEN [AQIValue] >= 0 THEN 1 ELSE 0 END)=0 THEN '--' ELSE 
			                dbo.F_Round(SUM(CASE WHEN [AQIValue] >= 0 AND [AQIValue] <= 100 THEN 1 ELSE 0 END)*100.0/
			                SUM(CASE WHEN [AQIValue] >= 0 THEN 1 ELSE 0 END),1) END AS DBRate,
                                dbo.F_Round(AVG(dbo.decFactorValueStr2Dec({0}))*{3},1) as {0} from AirRelease.TB_CheckRegionDayAQI  
                                left join dbo.SY_V_CodeDictionary as v on v.itemguid = AirRelease.TB_CheckRegionDayAQI.[RegionUid]
                                 where [RegionUid] in (
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
		                ) and DateTime >= '{1}' and DateTime <= '{2}'
                            GROUP BY [RegionUid],v.itemtext", factorCode, dateStart, dateEnd, num);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }


        /// <summary>
        /// 按污染种类查询
        /// </summary>
        /// <param name="regionGuids"></param>
        /// <param name="dtBegion"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataView GetDataPagerClass(string[] regionGuids, DateTime dtBegion, DateTime dtEnd)
        {
            try
            {
                string tableName = "AirReport.TB_RegionDayAQIReport";
                string sql = string.Empty;
                sql = string.Format(@"select MonitoringRegionUid,AQIValue,ReportDateTime from {0} where ReportDateTime >= '{1}' AND          ReportDateTime <= '{2}' order by ReportDateTime", tableName, dtBegion, dtEnd);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 按首要污染物查询
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dtBegion">开始日期</param>
        /// <param name="dtEnd">结束日期</param>
        /// <returns></returns>
        public DataView GetFirstPollute(string[] regionGuids, DateTime dtBegion, DateTime dtEnd)
        {

            try
            {
                string tableName = "AirReport.TB_RegionDayAQIReport";
                string sql = string.Empty;
                sql = string.Format(@"select MonitoringRegionUid,AQIValue,ReportDateTime,PrimaryPollutant from {0} where ReportDateTime>='{1}' AND  ReportDateTime <= '{2}' order by ReportDateTime", tableName, dtBegion, dtEnd);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// 获取区域日统计数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataTable getRegionData_TongJi(string[] PointIds, DateTime StartTime, DateTime EndTime, string type)
        {
            try
            {
                string TableName = "AirRelease.TB_DayAQI";
                string MonitoringBusinessConnection = "AMS_MonitoringBusinessConnection";
                string pointStr = string.Join(",", PointIds);
                string sql = string.Format(@"with a as(
SELECT  region.RegionUid,region.Region,DateTime
			,[dbo].[F_Round]([dbo].[F_GetValidRegionValue](COUNT(*),SUM(CASE WHEN [SO2] IS not NULL and [SO2] !='' THEN 1 ELSE 0 END),AVG(dbo.F_ValidValueStr([SO2])),MAX(dbo.F_ValidValueStr([SO2])),'day'),3) AS [SO2]
			,[dbo].[F_Round]([dbo].[F_GetValidRegionValue](COUNT(*),SUM(CASE WHEN [NO2] IS not NULL and [NO2] !='' THEN 1 ELSE 0 END),AVG(dbo.F_ValidValueStr([NO2])),MAX(dbo.F_ValidValueStr([NO2])),'day'),3) AS [NO2]
			,[dbo].[F_Round]([dbo].[F_GetValidRegionValue](COUNT(*),SUM(CASE WHEN [PM10] IS not NULL and [PM10] !='' THEN 1 ELSE 0 END),AVG(dbo.F_ValidValueStr([PM10])),MAX(dbo.F_ValidValueStr([PM10])),'day'),3) AS [PM10]
			,[dbo].[F_Round]([dbo].[F_GetValidRegionValue](COUNT(*),SUM(CASE WHEN [CO] IS not NULL and [CO] !='' THEN 1 ELSE 0 END),AVG(dbo.F_ValidValueStr([CO])),MAX(dbo.F_ValidValueStr([CO])),'day'),1) AS [CO]
			,[dbo].[F_Round]([dbo].[F_GetValidRegionValue](COUNT(*),SUM(CASE WHEN [Max8HourO3] IS not NULL and [Max8HourO3] !='' THEN 1 ELSE 0 END),AVG(dbo.F_ValidValueStr([Max8HourO3])),MAX(dbo.F_ValidValueStr([Max8HourO3])),'day'),3) AS [Max8HourO3]
			,[dbo].[F_Round]([dbo].[F_GetValidRegionValue](COUNT(*),SUM(CASE WHEN [PM25] IS not NULL and [PM25] !='' THEN 1 ELSE 0 END),AVG(dbo.F_ValidValueStr([PM25])),MAX(dbo.F_ValidValueStr([PM25])),'day'),3) AS [PM25]
		FROM {0} data  right join AMS_BaseData.dbo.V_Point_UserConfigNew region
		on  data.PointId=region.PortId
		WHERE 1=1
		AND PointId in({1})
		AND DateTime>= '{2}'
			AND DateTime<='{3}'
		GROUP BY region.RegionUid,region.Region,DateTime)
, b as(
select *
,[dbo].[f_getAQI]([SO2],'a21026',24) [SO2_IAQI]
,[dbo].[f_getAQI]([NO2],'a21004',24) [NO2_IAQI]
,[dbo].[f_getAQI]([PM10],'a34002',24) [PM10_IAQI] 
,[dbo].[f_getAQI]([CO],'a21005',24) [CO_IAQI]
,[dbo].[f_getAQI]([Max8HourO3],'a05024',8) [Max8HourO3_IAQI]
,[dbo].[f_getAQI]([PM25],'a34004',24) [PM25_IAQI] 
 from a )
, c as(
select *
      ,[dbo].[F_GetAQI_Max_CNV_Day] ([SO2_IAQI],[NO2_IAQI],[PM10_IAQI],[CO_IAQI],[Max8HourO3_IAQI],[PM25_IAQI],'V') [AQIValue]
      from b)
      select RegionUid,Region as RegionName
	  ,{4}(convert(decimal(6,3),case SO2 when '' then null else SO2 end)) [SO2]
      ,{4}(convert(decimal(6,3),case [NO2] when '' then null else [NO2] end)) [NO2]
      ,{4}(convert(decimal(6,3),case [PM10] when '' then null else [PM10] end)) [PM10]
      ,{4}(convert(decimal(6,3),case [CO] when '' then null else [CO] end)) [CO]
      ,{4}(convert(decimal(6,3),case [Max8HourO3] when '' then null else [Max8HourO3] end)) [Max8HourO3]
      ,{4}(convert(decimal(6,3),case [PM25] when '' then null else [PM25] end)) [PM25]
      from c group by RegionUid,Region", TableName, pointStr, StartTime, EndTime, type);
                return g_DatabaseHelper.ExecuteDataTable(sql, MonitoringBusinessConnection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取区域日统计数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataTable getExceedingDatas(string[] PointIds, DateTime StartTime, DateTime EndTime)
        {
            try
            {
                string TableName = "AirRelease.TB_DayAQI";
                string MonitoringBusinessConnection = "AMS_MonitoringBusinessConnection";
                string pointStr = string.Join(",", PointIds);
                string sql = string.Format(@"with a as(
SELECT  region.RegionUid,region.Region,DateTime
			,[dbo].[F_Round]([dbo].[F_GetValidRegionValue](COUNT(*),SUM(CASE WHEN [SO2] IS not NULL and [SO2] !='' THEN 1 ELSE 0 END),AVG(dbo.F_ValidValueStr([SO2])),MAX(dbo.F_ValidValueStr([SO2])),'day'),3) AS [SO2]
			,[dbo].[F_Round]([dbo].[F_GetValidRegionValue](COUNT(*),SUM(CASE WHEN [NO2] IS not NULL and [NO2] !='' THEN 1 ELSE 0 END),AVG(dbo.F_ValidValueStr([NO2])),MAX(dbo.F_ValidValueStr([NO2])),'day'),3) AS [NO2]
			,[dbo].[F_Round]([dbo].[F_GetValidRegionValue](COUNT(*),SUM(CASE WHEN [PM10] IS not NULL and [PM10] !='' THEN 1 ELSE 0 END),AVG(dbo.F_ValidValueStr([PM10])),MAX(dbo.F_ValidValueStr([PM10])),'day'),3) AS [PM10]
			,[dbo].[F_Round]([dbo].[F_GetValidRegionValue](COUNT(*),SUM(CASE WHEN [CO] IS not NULL and [CO] !='' THEN 1 ELSE 0 END),AVG(dbo.F_ValidValueStr([CO])),MAX(dbo.F_ValidValueStr([CO])),'day'),1) AS [CO]
			,[dbo].[F_Round]([dbo].[F_GetValidRegionValue](COUNT(*),SUM(CASE WHEN [Max8HourO3] IS not NULL and [Max8HourO3] !='' THEN 1 ELSE 0 END),AVG(dbo.F_ValidValueStr([Max8HourO3])),MAX(dbo.F_ValidValueStr([Max8HourO3])),'day'),3) AS [Max8HourO3]
			,[dbo].[F_Round]([dbo].[F_GetValidRegionValue](COUNT(*),SUM(CASE WHEN [PM25] IS not NULL and [PM25] !='' THEN 1 ELSE 0 END),AVG(dbo.F_ValidValueStr([PM25])),MAX(dbo.F_ValidValueStr([PM25])),'day'),3) AS [PM25]
		FROM {0} data  right join AMS_BaseData.dbo.V_Point_UserConfigNew region
		on  data.PointId=region.PortId
		WHERE 1=1
		AND PointId in({1})
		AND DateTime>= '{2}'
			AND DateTime<='{3}'
		GROUP BY region.RegionUid,region.Region,DateTime)
, b as(
select *
,[dbo].[f_getAQI]([SO2],'a21026',24) [SO2_IAQI]
,[dbo].[f_getAQI]([NO2],'a21004',24) [NO2_IAQI]
,[dbo].[f_getAQI]([PM10],'a34002',24) [PM10_IAQI] 
,[dbo].[f_getAQI]([CO],'a21005',24) [CO_IAQI]
,[dbo].[f_getAQI]([Max8HourO3],'a05024',8) [Max8HourO3_IAQI]
,[dbo].[f_getAQI]([PM25],'a34004',24) [PM25_IAQI] 
 from a )
, c as(
select *
      ,[dbo].[F_GetAQI_Max_CNV_Day] ([SO2_IAQI],[NO2_IAQI],[PM10_IAQI],[CO_IAQI],[Max8HourO3_IAQI],[PM25_IAQI],'V') [AQIValue]
      from b)
      select RegionUid,Region as RegionName
	  ,sum(case when (convert(int,case [AQIValue] when '' then null else [AQIValue] end) >= 0) then 1 else 0 end ) ValidCount
	  ,sum(case when (convert(int,case [AQIValue] when '' then null else [AQIValue] end) >= 0 
		and convert(int,case [AQIValue] when '' then null else [AQIValue] end) <= 100) then 1 else 0 end ) StandardCount
	  ,sum(case when [PM25_IAQI] >100 then 1 else 0 end) [PM25_Over]
	  ,sum(case when [PM10_IAQI] >100 then 1 else 0 end) [PM10_Over] 
	  ,sum(case when [NO2_IAQI] >100 then 1 else 0 end) [NO2_Over]
	  ,sum(case when [SO2_IAQI] >100 then 1 else 0 end) [SO2_Over]
	  ,sum(case when [Max8HourO3_IAQI] >100 then 1 else 0 end) [Max8HourO3_Over]
	  ,sum(case when [CO_IAQI] >100 then 1 else 0 end) [CO_Over]
      from c group by RegionUid,Region", TableName, pointStr, StartTime, EndTime);
                return g_DatabaseHelper.ExecuteDataTable(sql, MonitoringBusinessConnection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
