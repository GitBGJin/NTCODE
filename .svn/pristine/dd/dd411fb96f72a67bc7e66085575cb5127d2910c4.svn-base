using SmartEP.Utilities.AdoData;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.AutoMonitoring
{
    public class Infectant5Or60AirDAL
    {        /// <summary>
        /// 数据库出库类
        /// </summary>
        DatabaseHelper g_DatabaseHelper = new DatabaseHelper();

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connection = "AMS_AirAutoMonitorConnection";

        /// <summary>
        /// 获取该测点接近当前时间的数据(5分钟或60分钟)
        /// </summary>
        /// <param name="portIds">测点组</param>
        /// <param name="factors">因子组</param>
        /// <returns></returns>
        public DataTable GetAirRecentTimeDataBy5Or60(string[] portIds, string[] factors, string dataType)
        {
            string sql = string.Empty;
            string portIdsStr = string.Empty;//站点处理
            string factorsStr = string.Empty;//因子处理
            if (portIds.Length == 1 && !string.IsNullOrWhiteSpace(portIds[0]))
            {
                portIdsStr = " AND A.PointId =" + portIds[0];
            }
            else
            {
                portIdsStr = "AND A.PointId IN(" + StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",") + ")";
            }
            if (factors.Length == 1 && !string.IsNullOrWhiteSpace(factors[0]))
            {
                factorsStr = " AND A.PollutantCode ='" + factors[0] + "'";
            }
            else
            {
                //给因子加单引号
                for (int i = 0; i < factors.Length; i++)
                {
                    factors[i] = "'" + factors[i] + "'";
                }

                factorsStr = "AND A.PollutantCode IN(" + StringExtensions.GetArrayStrNoEmpty(factors.ToList<string>(), ",") + ")";
            }

            if (dataType.Equals("Min60"))
            {
                sql = string.Format(@"
                SELECT A.*
                FROM Air.TB_InfectantBy60 A
                WHERE Tstamp=(SELECT max(Tstamp) FROM Air.TB_InfectantBy60 WHERE PointId = A.PointId) {0} {1}
                ORDER BY A.PointId", portIdsStr, factorsStr);
            }
            else
            {
                sql = string.Format(@"
                SELECT A.*
                FROM Air.TB_InfectantBy5 A
                WHERE Tstamp=(SELECT max(Tstamp) FROM Air.TB_InfectantBy5 WHERE PointId = A.PointId) {0} {1}
                ORDER BY A.PointId", portIdsStr, factorsStr);
            }
            return g_DatabaseHelper.ExecuteDataTable(sql, connection);
        }

        /// <summary>
        /// 获取该测点接近当前时间的数据(5分钟和60分钟)
        /// </summary>
        /// <param name="portIds">测点组</param>
        /// <param name="factors">因子组</param>
        /// <returns></returns>
        public DataTable GetAirRecentTimeDataBy5And60(string[] portIds, string dataType)
        {
            string sql = string.Empty;
            string portIdsStr = string.Empty;//站点处理
            string factorsStr = string.Empty;//因子处理
            if (portIds.Length == 1 && !string.IsNullOrWhiteSpace(portIds[0]))
            {
                portIdsStr = " AND A.PointId =" + portIds[0];
            }
            else
            {
                portIdsStr = "AND A.PointId IN(" + StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",") + ")";
            }

            if (dataType.Equals("Min60"))
            {
                sql = string.Format(@"
                SELECT A.*
                FROM Air.TB_InfectantBy60 A
                WHERE Tstamp=(SELECT max(Tstamp) FROM Air.TB_InfectantBy60 WHERE PointId = A.PointId) {0} {1}
                ORDER BY A.PointId", portIdsStr, factorsStr);
            }
            else
            {
                sql = string.Format(@"
                SELECT A.*
                FROM Air.TB_InfectantBy5 A
                WHERE Tstamp=(SELECT max(Tstamp) FROM Air.TB_InfectantBy5 WHERE PointId = A.PointId) {0} {1}
                ORDER BY A.PointId", portIdsStr, factorsStr);
            }
            return g_DatabaseHelper.ExecuteDataTable(sql, connection);
        }

    }
}
