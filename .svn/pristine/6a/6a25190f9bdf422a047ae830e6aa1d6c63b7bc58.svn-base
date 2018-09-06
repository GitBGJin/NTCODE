using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.Utilities.AdoData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.MonitoringBusiness
{
    /// <summary>
    /// 名称：AuditPollutantDAL.cs
    /// 创建人：徐龙超
    /// 创建日期：2016-04-11
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 环境发布：审核因子
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class AuditPollutantDAL
    {
        #region << 变量 >>
        /// <summary>
        /// 数据库处理类
        /// </summary>
        DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(Enums.DataConnectionType.MonitoringBusiness);

        #endregion


        /// <summary>
        /// 获取审核因子配置数量
        /// </summary>
        /// <param name="pointID"></param>
        /// <param name="PointType"></param>
        /// <returns></returns>
//        public int GetAuditFactorsCount(int pointID, string applicationUid, string PointType = "0")
//        {
//            int num = 0;
//            string sql = string.Format(@"SELECT Count(distinct A.PollutantCode) num
//                                         FROM [Audit].[TB_AuditMonitoringPointPollutant] A 
//                                         JOIN [Audit].[TB_AuditMonitoringPoint] B 
//                                         ON A.AuditMonitoringPointUid=B.AuditMonitoringPointUid 
//                                         JOIN [dbo].[SY_V_Point_Air] C 
//                                         ON B.monitoringPointUid=C.monitoringPointUid 
//                                         Where [PointType]='{0}' and C.PointId={1} AND B.[ApplicationUid]='{2}'", PointType, pointID, applicationUid);
//            DataView dv = g_DatabaseHelper.ExecuteDataView(sql, connection);
//            if (dv.Count > 0) num = Convert.ToInt32(dv[0][0]);
//            return num;
//        }

        public string[] GetAuditFactorsCount(int pointID, string applicationUid, string PointType = "0")
        {
            string factors="";;
            string sql = string.Format(@"SELECT STUFF((SELECT distinct ','+A.PollutantCode
                                         FROM [Audit].[TB_AuditMonitoringPointPollutant] A 
                                         JOIN [Audit].[TB_AuditMonitoringPoint] B 
                                         ON A.AuditMonitoringPointUid=B.AuditMonitoringPointUid 
                                         JOIN {3} C 
                                         ON B.monitoringPointUid=C.monitoringPointUid 
                                         Where C.PointId={1} AND B.[ApplicationUid]='{2}' {0}
                                         FOR XML PATH('')),1, 1, '') AS PollutantCode"
                , (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUid)) ? "AND [PointType]='" + PointType + "'  " : ""
                , pointID, applicationUid
                , EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUid) ? "[dbo].[SY_V_Point_Air]" : "[dbo].[SY_V_Point_Water]");
            DataView dv = g_DatabaseHelper.ExecuteDataView(sql, connection);
            if (dv.Count > 0) factors =dv[0][0]!=DBNull.Value?dv[0][0].ToString():"";
            return factors.Split(',');
        }
    }
}
