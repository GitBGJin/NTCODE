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

namespace SmartEP.Data.SqlServer.MonitoringBusiness.WaterLZ
{
    /// <summary>
    /// 名称：DayReportDAL.cs
    /// 创建人：吕云
    /// 创建日期：2016-07-17
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 蓝藻预警发布：日数据处理
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
        //private string tableName = "[dbo].[V_ClassifyWaterDayReport]";
        private string tableName = "[dbo].[V_WaterDayReport]";

        #endregion

        #region 方法
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)（蓝藻含水源地分类，和空数据）
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
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal)
        {
            recordTotal = 0;
            try
            {
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorDataFlagSql = string.Empty;
                string factorAuditFlagSql = string.Empty;
                string factorEQISql = string.Empty;
                string factorGradeSql = string.Empty;
                string factorField = string.Empty;
                string wqSql = string.Empty;

                foreach (string factor in factors)
                {
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                    factorField += string.Format(",d.{0}",factor);
                    factorEQISql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN EQI END ) AS [{1}] ", factor, factor + "_EQI");
                    factorGradeSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN Grade END ) AS [{1}] ", factor, factor + "_Grade");
                }
                //wqSql = ",MAX(CASE(PollutantCode) WHEN 'WaterQuality' THEN EQI END ) AS [EQI] ";
                //wqSql += ",MAX(CASE(PollutantCode) WHEN 'WaterQuality' THEN Grade END ) AS [Grade] ";

                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");

                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " where PointId =" + portIdsStr;
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = " where PointId IN(" + portIdsStr + ")";
                }
                string portIds2Str = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");

                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIds2Str = " where c.PointId =" + portIds2Str;
                }
                else if (!string.IsNullOrEmpty(portIds2Str))
                {
                    portIds2Str = " where c.PointId IN(" + portIds2Str + ")";
                }
                string where = portIdsStr + string.Format(" AND DateTime>='{0}' AND DateTime<='{1}' ", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"));

                string sql =string.Format( @"select c.Name,c.PointId,c.BluedOrderNUm,c.MonitoringPointName,d.DateTime{1}
                                from
                               (select Name,a.PointId,a.MonitoringPointName,BluedOrderNUm from AMS_BaseData.MPInfo.TB_MonitoringPoint a
                                left join 
                                AMS_BaseData.MPInfo.TB_MonitoringBlueAlgaeRegion b
                                on a.BlueAlgaeRegionUid=b.BlueAlgaeRegionUid
                                where a.BluedOrderNUm is not null) c 
                                left join
                                (select PointId,MonitoringPointName,DateTime,BluedOrderNUm
                                {0}
                                from AMS_MonitorBusiness.[dbo].[V_WaterDayReport]", factorSql, factorField);
                //sql += tableName;
                //sql+=" where ";
                sql += where;
                sql += @"group by PointId,MonitoringPointName,DateTime,BluedOrderNUm) d
                        on c.PointId=d.PointId";
                sql += portIds2Str;
                sql += " order by [BluedOrderNUm] ";

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
        public DataView GetStatisticalData(string[] portIds, DateTime dateStart, DateTime dateEnd)
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
                string where = string.Format(" DateTime>='{0}' AND DateTime<='{1}'",
                    dateStart.ToString("yyyy-MM-dd HH:mm:ss"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr;

                //拼接统计数据字串，使用UNION方式减少统计条数，提升统计速度
                string sql = string.Empty;
                sql += string.Format(@"
                        SELECT PointId,MonitoringPointName,PollutantCode='w19011'
	                        ,AVG(PollutantValue) AS Value_Avg
	                        ,MAX(PollutantValue) AS Value_Max
	                        ,MIN(PollutantValue) AS Value_Min
                        FROM {0}
                        WHERE {1}
                            AND PollutantCode='w19011'
                        GROUP BY PointId,MonitoringPointName
                    ", tableName, where);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion
    }
}
