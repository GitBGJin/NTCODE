using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SmartEP.Utilities.DataTypes.ExtensionMethods;

namespace SmartEP.Data.SqlServer.MonitoringBusiness.Air
{
    /// <summary>
    /// 名称：AuditReasonDAL.cs
    /// 创建人：徐龙超
    /// 创建日期：2016-01-04
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 环境发布：灰霾天数统计
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class DustHazeDayDAL
    {
        #region << 变量 >>
        /// <summary>
        /// 数据库处理类
        /// </summary>
        SmartEP.Utilities.AdoData.DatabaseHelper g_DatabaseHelper = SmartEP.Core.Generic.Singleton<SmartEP.Utilities.AdoData.DatabaseHelper>.GetInstance();

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(Enums.DataConnectionType.MonitoringBusiness);

        /// <summary>
        /// 数据库表名
        /// </summary>
        private string tableName = "AirReport.TB_DustHazeDay";
        #endregion

        /// <summary>
        /// 获取灰霾天数统计
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataView GetDustHazeDayStatistical(string[] portIds, DateTime dateStart, DateTime dateEnd)
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
            string where = string.Format("Where [DateTime]>='{0}' AND [DateTime]<='{1}' ", dateStart.ToString("yyyy-MM-dd HH:mm:ss"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr;

            //拼接统计数据字串，使用UNION方式减少统计条数，提升统计速度
            string sql = string.Format(@"SELECT PointId,COUNT(CASE when IsDustHaze=0 THEN 1 ELSE NULL END) AS NODustHaze
                                                ,COUNT(CASE when IsDustHaze=1 THEN 1 ELSE NULL END) AS IsDustHaze
                                                ,COUNT(CASE when DustHazeGrade='轻微' THEN 1 ELSE NULL END) AS Grade1
                                                ,COUNT(CASE when DustHazeGrade='轻度' THEN 1 ELSE NULL END) AS Grade2
                                                ,COUNT(CASE when DustHazeGrade='中度' THEN 1 ELSE NULL END) AS Grade3
                                                ,COUNT(CASE when DustHazeGrade='重度' THEN 1 ELSE NULL END) AS Grade4
                                           FROM {0} {1} {2}
                                           GROUP BY PointId"
                                        , tableName, where, portIdsStr);
            return g_DatabaseHelper.ExecuteDataView(sql, connection);
        }
    }
}
