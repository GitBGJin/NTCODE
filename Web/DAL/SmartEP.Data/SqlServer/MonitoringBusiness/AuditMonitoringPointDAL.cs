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
    /// 名称：AuditMonitoringPointDAL.cs
    /// 创建人：朱佳伟
    /// 创建日期：2015-11-10
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 环境发布：测点审核配置
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class AuditMonitoringPointDAL
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

        #region 批量生成审核配置数据
        /// <summary>
        /// 批量生成审核配置数据
        /// </summary>
        /// <param name="PointUids">监测点Uid数组</param>
        /// <param name="MianLists">主表实体数组</param>
        /// <param name="DetailLists">从表实体数组</param>
        /// <param name="ApplicationUid">系统类型Uid</param>
        /// <param name="PointType">监测点类型 1超级，0普通</param>
        public void GetData(List<string> PointUids, List<AuditMonitoringPointEntity> MianLists, List<AuditMonitoringPointPollutantEntity> DetailLists, string ApplicationUid, int PointType)
        {
            if (PointUids != null)
            {
                List<CommandInfo> sqllist = new List<CommandInfo>();
                //删除从表数据
                StringBuilder strSql = new StringBuilder();
                strSql.Append(" delete from [Audit].[TB_AuditMonitoringPointPollutant] ");
                strSql.Append(" where AuditMonitoringPointUid in ( ");
                strSql.Append(" select [AuditMonitoringPointUid] from [Audit].[TB_AuditMonitoringPoint] ");
                strSql.Append(" where ApplicationUid=@ApplicationUid ");
                if (PointType == 0)
                {
                    strSql.Append(" and (PointType=@PointType or PointType is null) ");
                }
                if (PointType == 1)
                {
                    strSql.Append(" and PointType=@PointType ");
                }
                strSql.Append(" and ( ");
                for (int i = 0; i < PointUids.Count; i++)
                {
                    if (i == 0)
                    {
                        strSql.Append("[monitoringPointUid]='" + PointUids[i] + "' ");
                    }
                    else
                    {
                        strSql.Append("or [monitoringPointUid]='" + PointUids[i] + "' ");
                    }
                }
                strSql.Append(" )) ");
                SqlParameter[] parameters1 = { 
                                new SqlParameter("@ApplicationUid", SqlDbType.NVarChar, 50),
                                new SqlParameter("@PointType", SqlDbType.Int)};
                parameters1[0].Value = ApplicationUid;
                parameters1[1].Value = PointType;
                CommandInfo cmd = new CommandInfo(strSql.ToString(), parameters1);
                sqllist.Add(cmd);
                //删除主表数据
                strSql = new StringBuilder();
                strSql.Append(" delete from [Audit].[TB_AuditMonitoringPoint] ");
                strSql.Append(" where ApplicationUid=@ApplicationUid ");
                if (PointType == 0)
                {
                    strSql.Append(" and (PointType=@PointType or PointType is null) ");
                }
                if (PointType == 1)
                {
                    strSql.Append(" and PointType=@PointType ");
                }
                strSql.Append(" and ( ");
                for (int i = 0; i < PointUids.Count; i++)
                {
                    if (i == 0)
                    {
                        strSql.Append("[monitoringPointUid]='" + PointUids[i] + "' ");
                    }
                    else
                    {
                        strSql.Append("or [monitoringPointUid]='" + PointUids[i] + "' ");
                    }
                }
                strSql.Append(" ) ");
                SqlParameter[] parameters2 = { 
                                new SqlParameter("@ApplicationUid", SqlDbType.NVarChar, 50),
                                new SqlParameter("@PointType", SqlDbType.Int)};
                parameters2[0].Value = ApplicationUid;
                parameters2[1].Value = PointType;
                cmd = new CommandInfo(strSql.ToString(), parameters2);
                sqllist.Add(cmd);
                //添加主表数据
                if (MianLists != null)
                {
                    foreach (AuditMonitoringPointEntity main in MianLists)
                    {
                        strSql = new StringBuilder();
                        strSql.Append(" insert into [Audit].[TB_AuditMonitoringPoint]([AuditMonitoringPointUid],[ApplicationUid] ");
                        strSql.Append(" ,[monitoringPointUid],[AuditTypeUid],[PointType],[CreatUser],[CreatDateTime]) ");
                        strSql.Append(" values(@AuditMonitoringPointUid,@ApplicationUid,@monitoringPointUid,@AuditTypeUid,@PointType,@CreatUser,@CreatDateTime)");
                        SqlParameter[] parameters = { 
                                new SqlParameter("@AuditMonitoringPointUid", SqlDbType.NVarChar, 50),
                                new SqlParameter("@ApplicationUid", SqlDbType.NVarChar, 50),
                                new SqlParameter("@monitoringPointUid", SqlDbType.NVarChar, 50),
                                new SqlParameter("@AuditTypeUid", SqlDbType.NVarChar, 50),
                                new SqlParameter("@PointType", SqlDbType.Int),
                                new SqlParameter("@CreatUser", SqlDbType.NVarChar, 10),
                                new SqlParameter("@CreatDateTime", SqlDbType.DateTime)};
                        parameters[0].Value = main.AuditMonitoringPointUid;
                        parameters[1].Value = main.ApplicationUid;
                        parameters[2].Value = main.MonitoringPointUid;
                        parameters[3].Value = main.AuditTypeUid;
                        parameters[4].Value = main.PointType;
                        parameters[5].Value = main.CreatUser;
                        parameters[6].Value = main.CreatDateTime;
                        cmd = new CommandInfo(strSql.ToString(), parameters);
                        sqllist.Add(cmd);
                    }
                    //添加从表数据
                    foreach (AuditMonitoringPointPollutantEntity detail in DetailLists)
                    {
                        strSql = new StringBuilder();
                        strSql.Append(" insert into [Audit].[TB_AuditMonitoringPointPollutant]([AuditPollutantUID],[ApplicationUid],[AuditMonitoringPointUid] ");
                        strSql.Append(" ,[PollutantUid],[PollutantCode],[PollutantName],[ReadOnly],[CreatUser],[CreatDateTime]) ");
                        strSql.Append(" values(@AuditPollutantUID,@ApplicationUid,@AuditMonitoringPointUid,@PollutantUid,@PollutantCode ");
                        strSql.Append(" ,@PollutantName,@ReadOnly,@CreatUser,@CreatDateTime)");
                        SqlParameter[] parameters = { 
                                new SqlParameter("@AuditPollutantUID", SqlDbType.NVarChar, 50),
                                new SqlParameter("@ApplicationUid", SqlDbType.NVarChar, 50),
                                new SqlParameter("@AuditMonitoringPointUid", SqlDbType.NVarChar, 50),
                                new SqlParameter("@PollutantUid", SqlDbType.NVarChar, 50),
                                new SqlParameter("@PollutantCode", SqlDbType.NVarChar, 20),
                                new SqlParameter("@PollutantName", SqlDbType.NVarChar, 20),
                                new SqlParameter("@ReadOnly", SqlDbType.Bit),
                                new SqlParameter("@CreatUser", SqlDbType.NVarChar, 10),
                                new SqlParameter("@CreatDateTime", SqlDbType.DateTime)};
                        parameters[0].Value = detail.AuditPollutantUID;
                        parameters[1].Value = detail.ApplicationUid;
                        parameters[2].Value = detail.AuditMonitoringPointUid;
                        parameters[3].Value = detail.PollutantUid;
                        parameters[4].Value = detail.PollutantCode;
                        parameters[5].Value = detail.PollutantName;
                        parameters[6].Value = detail.ReadOnly.Value ? 1 : 0;
                        parameters[7].Value = detail.CreatUser;
                        parameters[8].Value = detail.CreatDateTime;
                        cmd = new CommandInfo(strSql.ToString(), parameters);
                        sqllist.Add(cmd);
                    }
                }
                g_DatabaseHelper.ExecuteSqlTranWithIndentity(sqllist, connection);
            }
        }
        #endregion
    }
}
