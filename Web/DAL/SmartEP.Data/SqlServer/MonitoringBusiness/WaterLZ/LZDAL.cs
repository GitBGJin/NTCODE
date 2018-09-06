using SmartEP.Core.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SmartEP.Utilities.DataTypes.ExtensionMethods;

namespace SmartEP.Data.SqlServer.MonitoringBusiness.WaterLZ
{
    public class LZDAL
    {
        /// <summary>
        /// 数据库处理类
        /// </summary>
        SmartEP.Utilities.AdoData.DatabaseHelper g_DatabaseHelper = SmartEP.Core.Generic.Singleton<SmartEP.Utilities.AdoData.DatabaseHelper>.GetInstance();

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(Enums.DataConnectionType.MonitoringBusiness);
        
        /// <summary>
        /// 取得藻密度排名前五
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="STime">开始时间</param>
        /// <param name="ETime">截止时间</param>
        /// <returns></returns>
        public DataTable GetTopFiveAlgalDensity(DateTime STime, DateTime ETime, string[] pointIds)
        {
            //string ApplicationUid = SmartEP.Core.Enums.EnumMapping.GetDesc(applicationType);
            string strPoints = "";
            foreach (string pointId in pointIds)
            {
                strPoints += pointId.ToString() + ",";
            }
            strPoints = strPoints.Remove(strPoints.Length - 1, 1);

            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT top 5 b.PointId, b.MonitoringPointName, AVG(PollutantValue) as AlgalDensity
                            FROM [AMS_WaterAutoMonitorSZ].[Water].[TB_InfectantBy60] a,[AMS_BaseData].MPInfo.TB_MonitoringPoint  b                       
                            where a.PointId=b.PointId
                            and PollutantCode='w19011' 
                            and b.PointId in (" + strPoints + ") and [Tstamp]>='" + STime + "' and [Tstamp]<='" + ETime + "'");
            strSql.Append(@" GROUP  BY b.PointId,b.MonitoringPointName
                             order by AlgalDensity desc ");
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), connection);
        }

        /// <summary>
        /// 取得藻密度排名（全36个点位）
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="STime">开始时间</param>
        /// <param name="ETime">截止时间</param>
        /// <returns></returns>
        public DataTable GetBlueAlgalSort(DateTime STime, DateTime ETime, string[] pointIds, string[] factors)
        {
            try
            {
                string factorSql = string.Empty;
                string avgSql = string.Empty;
                string factorWhere = string.Empty;
                string orderby = string.Empty;
                foreach (string factor in factors)
                {
                    avgSql += string.Format(",AVG([{0}])  '{0}'", factor);
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                    factorWhere += "'" + factor + "',";
                    orderby = string.Format(@" order by '{0}' desc ", factor);
                }
                factorWhere = factorWhere.Remove(factorWhere.Length - 1, 1);
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(pointIds.ToList<string>(), ",");
                if (pointIds.Length == 1 && !string.IsNullOrEmpty(pointIds[0]))
                {
                    portIdsStr = " AND a.PointId =" + portIdsStr;
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = " AND a.PointId IN(" + portIdsStr + ")";
                }
                string fieldName = " c.PointId,c.MonitoringPointName" + avgSql;
                string where = string.Format(@" where Tstamp>='{0}' AND Tstamp<='{1}'  AND PollutantCode in ({2}) ", STime, ETime, factorWhere) + portIdsStr;
                string from = string.Format(@"from (select a.PointId,b.MonitoringPointName,Tstamp" + factorSql+ 
                                                   @"from [AMS_WaterAutoMonitorSZ].[dbo].[V_InfectantBy60] a
                                                     left join [AMS_BaseData].MPInfo.TB_MonitoringPoint  b
                                                     on a.PointId =b.PointId" + where+
                                                   @" group by a.PointId,b.MonitoringPointName,Tstamp) c ");
                string groupby = " group by c.PointId,c.MonitoringPointName ";
                
                string sql = "select ";
                sql += fieldName;
                sql += from;
                sql += groupby;
                sql += orderby;
                return g_DatabaseHelper.ExecuteDataTable(sql, connection);

//                string strPoints = "";
//                foreach (string pointId in pointIds)
//                {
//                    strPoints += pointId.ToString() + ",";
//                }
//                strPoints = strPoints.Remove(strPoints.Length - 1, 1);

//                StringBuilder strSql = new StringBuilder();
//                strSql.Append(@"SELECT b.PointId, b.MonitoringPointName, AVG(PollutantValue) as AlgalDensity
//                            FROM [AMS_WaterAutoMonitorSZ].[Water].[TB_InfectantBy60] a,[AMS_BaseData].MPInfo.TB_MonitoringPoint  b                       
//                            where a.PointId=b.PointId
//                            and PollutantCode='w19011' 
//                            and b.PointId in (" + strPoints + ") and [Tstamp]>='" + STime + "' and [Tstamp]<='" + ETime + "'");
//                strSql.Append(@" GROUP  BY b.PointId,b.MonitoringPointName
//                             order by AlgalDensity desc ");
//                return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), connection);
             }
             catch (Exception ex)
            {
                     throw ex;
             }

        }
    }
}
