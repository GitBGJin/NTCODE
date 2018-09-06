using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Data.SqlServer.Common.GridView;
using SmartEP.Utilities.AdoData;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.MonitoringBusiness.Water
{
    /// <summary>
    /// 名称：AuditWaterInfectantByMinuteDAL.cs
    /// 创建人：李飞
    /// 创建日期：2015-08-30
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 审核小时数据库处理
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class AuditWaterInfectantByMinuteDAL
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
        private string connection = "AMS_MonitoringBusinessConnection";

        /// <summary>
        /// 数据库表名
        /// </summary>
        private string tableName = "Audit.TB_AuditWaterInfectantByMinute";
        #endregion

        #region << 构造函数 >>
        /// <summary>
        /// 构造函数
        /// </summary>
        public AuditWaterInfectantByMinuteDAL()
        {
            connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(ApplicationType.Air, PollutantDataType.Hour);
        }
        #endregion

        #region << 方法 >>
        /// <summary>
        /// 取得行转列数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <returns></returns>
        public DataView GetDataView(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {
            try
            {
                string portIdsStr = string.Empty;
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND PointId =" + portIds[0];
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = "AND PointId IN(" + StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",") + ")";
                }

                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                string factorMarkSql = string.Empty;
                string factorIsAuditSql = string.Empty;
                string factorSel = string.Empty;
                string factorFlagSel = string.Empty;
                string factorMarkSel = string.Empty;
                string factorIsAuditSel = string.Empty;

                foreach (string factor in factors)
                {
                    string factorDataFlag = factor + "_dataFlag";
                    string factorAuditFlag = factor + "_AuditFlag";
                    string factorIsAudit = factor + "_IsAudit";
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                    factorFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN dataFlag END) AS [{1}] ", factor, factorDataFlag);
                    factorMarkSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN AuditFlag END) AS [{1}] ", factor, factorAuditFlag);
                    factorIsAuditSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN IsAudit END) AS [{1}] ", factor, factorIsAudit);
                    factorSel += string.Format("[{0}]", factor);
                    factorFlagSel += string.Format("[{0}]", factorDataFlag);
                    factorMarkSel += string.Format("[{0}]", factorAuditFlag);
                    factorIsAuditSel += string.Format("[{0}]", factorIsAudit);
                }
                string sql = string.Format(@"
                    SELECT dayInfo.PointId
                        ,hourInfo.DataDateTime
                        {0}
                        {1}
                        {2}
                        {3}
                    FROM {4} AS hourInfo
                    LEFT JOIN Audit.TB_AuditStatusForDay AS dayInfo
	                    ON hourInfo.AuditStatusUid = dayInfo.AuditStatusUid
                    WHERE hourInfo.DataDateTime>='{5}' 
                        AND hourInfo.DataDateTime<='{6}'
                        AND dayInfo.PointId is not NULL
	                    {7}
                    GROUP BY dayInfo.PointId,hourInfo.DataDateTime
                    ", factorSql, factorFlagSql, factorMarkSql, factorIsAuditSql, tableName, dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), portIdsStr);
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
