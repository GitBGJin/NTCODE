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
    /// 名称：RegionHourAQIDAL.cs
    /// 创建人：李飞
    /// 创建日期：2015-08-18
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 环境空气发布：区域AQI数据库处理
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class RegionHourAQIDAL
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
        /// 存储过程处理类
        /// </summary>
        BaseDAHelper g_DBBiz = Singleton<BaseDAHelper>.GetInstance();
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        protected string connection = null;

        /// <summary>
        /// 数据库表名
        /// </summary>
        protected string tableName = null;

        #endregion

        #region << 构造函数 >>
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="aQIDataType">AQI数据类型</param>
        public RegionHourAQIDAL()
        {
            tableName = SmartEP.Data.Enums.EnumMapping.GetTableName(AQIDataType.RegionHourAQI);
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
        /// <param name="orderBy">排序字段(MonitoringRegionUid,DateTime)</param>
        /// <returns></returns>
        public DataView GetDataPager(string[] regionGuids, DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo
            , out int recordTotal, RegionAQIStatisticalType regionAQIStatisticalType, string orderBy = "MonitoringRegionUid,DateTime")
        {
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "MonitoringRegionUid,DateTime";
            recordTotal = 0;

            string fieldName = GetFieldName();
            string keyName = "MonitoringRegionUid";

            //查询条件拼接
            string where = string.Empty;
            string regionGuidStr = string.Empty;
            if (regionGuids != null && regionGuids.Length > 0)
                regionGuidStr = " AND MonitoringRegionUid IN ('" + StringExtensions.GetArrayStrNoEmpty(regionGuids.ToList(), "','") + "')";

            where = string.Format(" DateTime>='{0}' AND DateTime<='{1}' AND StatisticalType='{2}' "
                , dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), SmartEP.Core.Enums.EnumMapping.GetDesc(regionAQIStatisticalType)) + regionGuidStr;

            return g_GridViewPager.GetGridViewPager(tableName, fieldName, keyName, pageSize, pageNo, orderBy, where, connection, out recordTotal);
        }
        /// <summary>
        /// 获取小时AQI相关数据信息
        /// </summary>
        /// <param name="BeginTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        public DataView GetAQIHourBase(DateTime BeginTime, DateTime EndTime)
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

                var dv = g_DBBiz.ExecuteProc("GetAQI_HourBase_Mul", connection);
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
        public DataView GetPointAQIHourBase(DateTime BeginTime, DateTime EndTime, int pointStr,string dataType)
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
                var dv=new System.Data.DataView();
                if (dataType == "OriData")
                {
                    dv = g_DBBiz.ExecuteProc("GetAQI_PointHourBase_Mul", "AMS_AirAutoMonitorConnection");
                }
                else if(dataType=="OverOriData")
                {
                    dv = g_DBBiz.ExecuteProc("GetAQI_PointHourBase_MulOverDay", "AMS_AirAutoMonitorConnection");
                }
                else
                {
                    dv = g_DBBiz.ExecuteProc("GetAQI_PointHourBase_Mul", connection);
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
        /// 获取测点小时AQI相关数据信息
        /// </summary>
        /// <param name="BeginTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        public DataView GetPointAQIHourBaseOver23(DateTime BeginTime, DateTime EndTime, int pointStr, string dataType,object avgO3)
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

                SqlParameter AvgO3 = new SqlParameter();
                AvgO3 = new SqlParameter();
                AvgO3.SqlDbType = SqlDbType.Decimal;
                AvgO3.ParameterName = "@m_avgO3";
                AvgO3.Value = avgO3;
                g_DBBiz.SetProcedureParameters(AvgO3);
                var dv = new System.Data.DataView();
                if (dataType == "OriData")
                {
                    dv = g_DBBiz.ExecuteProc("GetAQI_PointHourBase_MulOver23", "AMS_AirAutoMonitorConnection");
                }
                else
                {
                    dv = g_DBBiz.ExecuteProc("GetAQI_PointHourBase_MulOver23", connection);
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
        /// 取得导出数据(行转列数据)
        /// </summary>
        /// <param name="autoMonitorType">查询数据类型</param>
        /// <param name="regionGuids">测点数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <param name="orderBy">排序字段(MonitoringRegionUid,DateTime)</param>
        /// <returns></returns>
        public DataView GetExportData(string[] regionGuids, DateTime dtStart, DateTime dtEnd, RegionAQIStatisticalType regionAQIStatisticalType, string orderBy = "MonitoringRegionUid,DateTime")
        {
            orderBy = !string.IsNullOrEmpty(orderBy) ? orderBy : "MonitoringRegionUid,DateTime";
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

            where = string.Format(" DateTime>='{0}' AND DateTime<='{1}' AND StatisticalType='{2}' ", dtStart.ToString("yyyy-MM-dd HH:mm:ss")
                , dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), SmartEP.Core.Enums.EnumMapping.GetDesc(regionAQIStatisticalType)) + regionGuidStr;

            return g_GridViewPager.GetGridViewPager(tableName, fieldName, keyName, pageSize, pageNo, orderBy, where, connection, out recordTotal);
        }

        /// <summary>
        /// 根据站点取得最新小时AQI数据
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <returns></returns>
        public DataView GetLastData(string[] regionGuids, DateTime dtStart, DateTime dtEnd, RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            try
            {
                string regionGuidStr = string.Empty;
                if (regionGuids != null && regionGuids.Length > 0)
                    regionGuidStr = " AND MonitoringRegionUid IN ('" + StringExtensions.GetArrayStrNoEmpty(regionGuids.ToList(), "','") + "')";

                //拼接where条件
                string where = string.Format(" DateTime>='{0}' AND DateTime<='{1}' AND StatisticalType='{2}' AND AQIValue is not null ", dtStart.ToString("yyyy-MM-dd HH:mm:ss")
                    , dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), SmartEP.Core.Enums.EnumMapping.GetDesc(regionAQIStatisticalType)) + regionGuidStr;

                string sql = string.Format(@"
                WITH lastData AS
                (
                    SELECT MonitoringRegionUid AS RegionUid
	                    ,MAX(DateTime) AS Tstamp
                    FROM {0}
                    where {1}
                    GROUP BY MonitoringRegionUid
                )
                SELECT {2}
                FROM lastData
                INNER JOIN {0} AS data
	                ON lastData.RegionUid = data.MonitoringRegionUid AND lastData.Tstamp = data.DateTime
                ", tableName, where, GetFieldName());
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 取得查询字段
        /// </summary>
        /// <param name="aQIDataType"></param>
        /// <returns></returns>
        private string GetFieldName()
        {
            string fieldName = @"MonitoringRegionUid
                        ,DateTime
                        ,SO2
                        ,SO2_IAQI
                        ,NO2
                        ,NO2_IAQI
                        ,PM10
                        ,PM10_IAQI
                        ,Recent24HoursPM10
                        ,Recent24HoursPM10_IAQI
                        ,CO
                        ,CO_IAQI
                        ,O3
                        ,O3_IAQI
                        ,Recent8HoursO3
                        ,Recent8HoursO3_IAQI
                        ,PM25
                        ,PM25_IAQI
                        ,Recent24HoursPM25
                        ,Recent24HoursPM25_IAQI
                        ,AQIValue
                        ,PrimaryPollutant
                        ,Range
                        ,RGBValue
                        ,PicturePath
                        ,Class
                        ,Grade
                        ,HealthEffect
                        ,TakeStep";

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
        /// <returns></returns>
        public DataView GetGradeStatistics(IAQIType aqiType, string[] regionGuids, DateTime dateStart, DateTime dateEnd, RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            string regionGuidStr = string.Empty;
            foreach (string regionGuid in regionGuids)
            {
                regionGuidStr += string.Format("'{0}',", regionGuid);
            }
            regionGuidStr = regionGuidStr.TrimEnd(',');
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
                WHERE MonitoringRegionUid IN ({2}) AND DateTime>='{3}' AND DateTime<='{4}' AND StatisticalType='{5}'
                GROUP BY MonitoringRegionUid ", SmartEP.Core.Enums.EnumMapping.GetIAQITypeColumn(aqiType), tableName, regionGuidStr
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
        /// <returns></returns>
        public DataView GetContaminantsStatistics(IAQIType aqiType, string[] regionGuids, DateTime dateStart, DateTime dateEnd, RegionAQIStatisticalType regionAQIStatisticalType = RegionAQIStatisticalType.Conventional)
        {
            string regionGuidStr = string.Empty;
            foreach (string regionGuid in regionGuids)
            {
                regionGuidStr += string.Format("'{0}',", regionGuid);
            }
            regionGuidStr = regionGuidStr.TrimEnd(',');
            string sql = string.Format(@"
                SELECT MonitoringRegionUid
                    ,SUM(CASE WHEN {0}=AQIValue AND AQIValue>50 THEN 1 ELSE 0 END) AS Count
                FROM {1}
                WHERE MonitoringRegionUid IN ({2}) AND DateTime>='{3}' AND DateTime<='{4}' AND StatisticalType='{5}'
                GROUP BY MonitoringRegionUid ", SmartEP.Core.Enums.EnumMapping.GetIAQITypeColumn(aqiType), tableName, regionGuidStr
                                              , dateStart.ToString("yyyy-MM-dd HH:mm:ss"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss"), SmartEP.Core.Enums.EnumMapping.GetDesc(regionAQIStatisticalType));
            return g_DatabaseHelper.ExecuteDataView(sql, connection); ;
        }
        #endregion
    }
}
