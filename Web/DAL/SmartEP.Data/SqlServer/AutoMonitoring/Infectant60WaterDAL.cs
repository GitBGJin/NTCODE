using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.AutoMonitoring
{
    /// <summary>
    /// 名称：Infectant60WaterDAL.cs
    /// 创建人：李飞
    /// 创建日期：2015-08-18
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 地表水原始小时数据处理
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class Infectant60WaterDAL : InfectantDAL
    {
        #region << 构造函数 >>
        /// <summary>
        /// 构造函数
        /// </summary>
        public Infectant60WaterDAL()
            : base(Core.Enums.ApplicationType.Water, Core.Enums.PollutantDataType.Min60)
        { }
        #endregion

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
                    WQLSql += string.Format(",MAX(CASE WHEN  PollutantCode='{0}' THEN dbo.F_GetWQL('{0}','7c67a857-d602-4f90-a26d-edd3e9f4d36c',PollutantValue,'d8197909-568e-4319-874c-3ad7cbc92a7e','LEVEL') END) AS 'WQL_{0}'", pollutant);
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
        /// 获取该测点接近当前时间的数据
        /// </summary>
        /// <param name="portIds">测点组</param>
        /// <param name="factors">因子组</param>
        /// <returns></returns>
        public DataView GetWaterRecentTimeData(string[] portIds, string[] factors)
        {
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

            string sql = string.Format(@"
                SELECT A.*
                FROM Water.TB_InfectantBy60 A
                WHERE Tstamp=(SELECT max(Tstamp) FROM Water.TB_InfectantBy60 WHERE PointId = A.PointId) {0} {1}
                ORDER BY A.PointId", portIdsStr, factorsStr);
            return g_DatabaseHelper.ExecuteDataView(sql, connection);
        }
    }
}
