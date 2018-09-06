using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Data.Enums;
using SmartEP.Utilities.AdoData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace SmartEP.Data.SqlServer.BaseData
{
    public class PersonalizedSetDAL
    {
        /// <summary>
        /// 数据库处理类
        /// </summary>
        protected DatabaseHelper dbHelper = Singleton<DatabaseHelper>.GetInstance();

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.BaseData);

        /// <summary>
        /// 根据用户GUID获取授权设置
        /// </summary>
        /// <param name="userGuid">用户Guid</param>
        /// <param name="appUid">系统应用类型Uid</param>
        /// <returns></returns>
        public DataTable GetPersonalizedSetByUserGuid(string userGuid, string appUid)
        {
            string querySql = string.Format(@"
                                            select PersonalizedSettingUid,
                                                   ParameterUid,
                                                   ParameterType,
                                                   EnableCustomOrNot 
                                              from [BasicInfo].[TB_PersonalizedSettings] 
                                             where UserUid = '{0}' 
                                               and ApplicationUid = '{1}'
                                            ", userGuid
                                             , appUid);

            return dbHelper.ExecuteDataTable(querySql, connection);
        }

        /// <summary>
        /// 根据用户GUID获取测点分组设置
        /// </summary>
        /// <param name="userGuid">用户Guid</param>
        /// <param name="appUid">系统应用类型Uid</param>
        /// <returns></returns>
        public DataView GetPersonalizedSetPointGroupByUserGuid(string userGuid, string appUid)
        {
            string querySql = string.Format(@"
                                             with tempInfo as (
                                           select NULL AS GID, 
                                                  NULL AS PGuid, 
                                                  'NULL-NULL-NULL-NULL-NULL' AS CGuid,
                                                  '' AS PID, 
                                                  '未分类' AS PName, 
                                                  NULL AS EnableCustomOrNot,
                                                  99999 AS POrder, 
                                                  NULL AS COrder 
                                             union 
                                            select frmCode.ItemGuid as GID,
                                                   NULL as PGuid, 
                                                   frmCode.ItemGuid as CGuid,
                                                   frmCode.ItemCode as PID,
                                                   frmCode.ItemText as PName, 
                                                   NULL as EnableCustomOrNot,
                                                   frmCode.SortNumber as POrder, 
                                                   NULL as COrder 
                                              from dbo.SY_View_CodeMainItem frmCode
                                             inner join (  
			                                            select distinct SiteTypeUid 
			                                              from [MPInfo].[TB_MonitoringPoint] mp
			                                             inner join BasicInfo.TB_PersonalizedSettings tbp
				                                            on mp.MonitoringPointUid = tbp.ParameterUid
			                                               and tbp.ApplicationUid = '{0}'
			                                               and tbp.UserUid = '{1}'
			                                               and tbp.ParameterType = 'Port'
                                                           and mp.EnableOrNot = 1) as temp
                                                on frmCode.ItemGuid = temp.SiteTypeUid
                                             union 
                                            select PersonalizedSettingUid as GID,
                                                   SiteTypeUid as PGuid, 
                                                   ParameterUid as CGuid,
                                                   PointId as PID,
                                                   MonitoringPointName as PName,
                                                   EnableCustomOrNot as EnableCustomOrNot, 
                                                   NULL as POrder,
                                                   mp_p.OrderByNum as COrder 
                                              from BasicInfo.TB_PersonalizedSettings tbp_p
                                             inner join [MPInfo].[TB_MonitoringPoint] mp_p
                                                on tbp_p.ParameterUid = mp_p.MonitoringPointUid
                                             where tbp_p.ApplicationUid='{0}' 
                                               and tbp_p.UserUid = '{1}'
                                               and tbp_p.ParameterType = 'Port'
                                               and mp_p.EnableOrNot = 1)
                                              select * 
                                                from tempInfo  
                                               where PGuid is null
                                                 and CGuid in (select distinct PGuid from tempInfo)
                                               union 
                                              select *
                                                from tempInfo
                                               where PGuid is not null
                                                 and PGuid in (select distinct CGuid from tempInfo where PGuid is null)
                                             order by POrder desc, COrder desc"
                                             , appUid
                                             , userGuid);

            return dbHelper.ExecuteDataView(querySql, connection);
        }

        /// <summary>
        /// 根据用户GUID获取授权测点分组
        /// </summary>
        /// <param name="userGuid">用户Guid</param>
        /// <param name="appUid">系统应用类型Uid</param>
        /// <returns></returns>
        public DataView GetAuthPointGroupByUserGuid(string userGuid, string appUid)
        {
            string querySql = string.Format(@"
                                             with tempInfo as (
                                           select NULL AS GID, 
                                                  NULL AS PGuid, 
                                                  'NULL-NULL-NULL-NULL-NULL' AS CGuid,
                                                  '' AS PID, 
                                                  '未分类' AS PName, 
                                                  NULL AS EnableCustomOrNot,
                                                  99999 AS POrder, 
                                                  NULL AS COrder 
                                             union 
                                            select frmCode.ItemGuid as GID,
                                                   NULL as PGuid, 
                                                   frmCode.ItemGuid as CGuid,
                                                   frmCode.ItemCode as PID,
                                                   frmCode.ItemText as PName, 
                                                   NULL as EnableCustomOrNot,
                                                   frmCode.SortNumber as POrder, 
                                                   NULL as COrder 
                                              from dbo.SY_View_CodeMainItem frmCode
                                             inner join (  
			                                            select distinct SiteTypeUid 
			                                              from [MPInfo].[TB_MonitoringPoint]
			                                             where ApplicationUid = '{0}'
                                                           and EnableOrNot = 1 
			                                             ) as temp
                                                on frmCode.ItemGuid = temp.SiteTypeUid
                                             union 
                                            select PersonalizedSettingUid as GID,
                                                   SiteTypeUid as PGuid, 
                                                   MonitoringPointUid as CGuid,
                                                   PointId as PID,
                                                   MonitoringPointName as PName,
                                                   ISNULL((case EnableCustomOrNot when 0 then 1 when 1 then 1 end), 0) as EnableCustomOrNot, 
                                                   NULL as POrder,
                                                   mp_p.OrderByNum as COrder 
                                              from BasicInfo.TB_PersonalizedSettings tbp_p
                                             right join [MPInfo].[TB_MonitoringPoint] mp_p
                                                on tbp_p.ParameterUid = mp_p.MonitoringPointUid
                                               and tbp_p.UserUid = '{1}'
                                               and tbp_p.ParameterType = 'Port'
                                             where mp_p.ApplicationUid='{0}'
                                               and mp_p.EnableOrNot = 1)
                                              select * 
                                                from tempInfo  
                                               where PGuid is null
                                                 and CGuid in (select distinct PGuid from tempInfo)
                                               union 
                                              select *
                                                from tempInfo
                                               where PGuid is not null
                                                 and PGuid in (select distinct CGuid from tempInfo where PGuid is null)
                                             order by POrder desc, COrder desc"
                                             , appUid
                                             , userGuid);

            return dbHelper.ExecuteDataView(querySql, connection);
        }

        /// <summary>
        /// 根据用户GUID获取因子分组设置
        /// </summary>
        /// <param name="userGuid">用户Guid</param>
        /// <param name="pollutantTypeUid">系统应用类型Uid</param>
        /// <returns></returns>
        public DataView GetPersonalizedSetPollutantGroupByUserGuid(string userGuid, string pollutantTypeUid)
        {
            string querySql = string.Format(@"
                                               with tempInfo as (
                                             select NULL AS GID, 
                                                  NULL AS PGuid, 
                                                  'NULL-NULL-NULL-NULL-NULL' AS CGuid,
                                                  '' AS PID, 
                                                  '未分类' AS PName, 
                                                  NULL AS EnableCustomOrNot,
                                                  99999 AS POrder, 
                                                  NULL AS COrder 
                                             union 
                                            select frmCode.ItemGuid as GID,
                                                   NULL as PGuid, 
                                                   frmCode.ItemGuid as CGuid,
                                                   frmCode.ItemCode as PID,
                                                   frmCode.ItemText as PName, 
                                                   NULL as EnableCustomOrNot,
                                                   frmCode.SortNumber as POrder, 
                                                   NULL as COrder 
                                              from dbo.SY_View_CodeMainItem frmCode
                                             inner join [Standard].[TB_PollutantCode] temp
                                                on frmCode.ItemGuid = temp.CategoryUid
                                               and temp.TypeUid = 'ae39f55e-5c43-4b4a-b224-0b925b5f3c9f'
                                               and temp.IsUseOrNot = 1 
                                             union 
                                            select PersonalizedSettingUid as GID,
                                                   CategoryUid as PGuid, 
                                                   ParameterUid as CGuid,
                                                   PollutantCode as PID,
                                                   PollutantName as PName,
                                                   EnableCustomOrNot as EnableCustomOrNot, 
                                                   NULL as POrder,
                                                   stp_p.OrderByNum as COrder 
                                              from BasicInfo.TB_PersonalizedSettings tbp_p
                                             inner join [Standard].[TB_PollutantCode] stp_p
                                                on tbp_p.ParameterUid = stp_p.PollutantUid
                                             where stp_p.PollutantTypeUid='{0}' 
                                               and stp_p.TypeUid = 'ae39f55e-5c43-4b4a-b224-0b925b5f3c9f'
                                               and stp_p.IsUseOrNot = 1 
                                               and tbp_p.UserUid = '{1}'
                                               and tbp_p.ParameterType = 'pollutant')
                                              select * 
                                                from tempInfo  
                                               where PGuid is null
                                                 and CGuid in (select distinct PGuid from tempInfo)
                                               union 
                                              select *
                                                from tempInfo
                                               where PGuid is not null
                                                 and PGuid in (select distinct CGuid from tempInfo where PGuid is null)
                                             order by POrder desc, COrder desc"
                                             , pollutantTypeUid
                                             , userGuid);

            return dbHelper.ExecuteDataView(querySql, connection);
        }

        /// <summary>
        /// 根据用户GUID获取授权因子分组
        /// </summary>
        /// <param name="userGuid">用户Guid</param>
        /// <param name="appUid">系统因子类型Uid</param>
        /// <returns></returns>
        public DataView GetAuthPollutantGroupByUserGuid(string userGuid, string appUid)
        {
            string querySql = string.Format(@"

                                               with tempInfo as (
                                             select NULL AS GID, 
                                                  NULL AS PGuid, 
                                                  'NULL-NULL-NULL-NULL-NULL' AS CGuid,
                                                  '' AS PID, 
                                                  '未分类' AS PName, 
                                                  NULL AS EnableCustomOrNot,
                                                  99999 AS POrder, 
                                                  NULL AS COrder 
                                             union 
                                            select frmCode.ItemGuid as GID,
                                                   NULL as PGuid, 
                                                   frmCode.ItemGuid as CGuid,
                                                   frmCode.ItemCode as PID,
                                                   frmCode.ItemText as PName, 
                                                   NULL as EnableCustomOrNot,
                                                   frmCode.SortNumber as POrder, 
                                                   NULL as COrder 
                                              from dbo.SY_View_CodeMainItem frmCode
                                             inner join (
                                                         select distinct stp.PollutantCode,
                                                                stp.CategoryUid,
                                                                stp.PollutantTypeUid,
                                                                stp.TypeUid
                                                           from [Standard].[TB_PollutantCode] stp
                                                          inner join [dbo].[V_Point_InstrumentChannels] vpi
                                                             on stp.PollutantCode = vpi.PollutantCode
                                                            and stp.TypeUid = 'ae39f55e-5c43-4b4a-b224-0b925b5f3c9f'
                                                            and stp.IsUseOrNot = 1 
                                                            and vpi.ApplicationUid = '{0}' 
                                                         ) as temp
                                                on frmCode.ItemGuid = temp.CategoryUid
                                             union 
                                            select PersonalizedSettingUid as GID,
                                                   CategoryUid as PGuid, 
                                                   PollutantUid as CGuid,
                                                   PollutantCode as PID,
                                                   PollutantName as PName,
                                                   ISNULL((case EnableCustomOrNot when 0 then 1 when 1 then 1 end), 0) as EnableCustomOrNot, 
                                                   NULL as POrder,
                                                   stp_p.OrderByNum as COrder 
                                              from BasicInfo.TB_PersonalizedSettings tbp_p
                                             right join (
                                                         select distinct stp.PollutantUid,
                                                                stp.PollutantCode,
                                                                stp.PollutantName,
                                                                stp.CategoryUid,
                                                                stp.PollutantTypeUid,
                                                                stp.TypeUid,
                                                                stp.OrderByNum 
                                                           from [Standard].[TB_PollutantCode] stp
                                                          inner join [dbo].[V_Point_InstrumentChannels] vpi
                                                             on stp.PollutantCode = vpi.PollutantCode
                                                            and stp.TypeUid = 'ae39f55e-5c43-4b4a-b224-0b925b5f3c9f'
                                                            and stp.IsUseOrNot = 1 
                                                            and vpi.ApplicationUid = '{0}'           
                                                         ) as stp_p
                                                on tbp_p.ParameterUid = stp_p.PollutantUid
                                               and tbp_p.UserUid = '{1}'
                                               and tbp_p.ParameterType = 'pollutant')
                                              select * 
                                                from tempInfo  
                                               where PGuid is null
                                                 and CGuid in (select distinct PGuid from tempInfo)
                                               union 
                                              select *
                                                from tempInfo
                                               where PGuid is not null
                                                 and PGuid in (select distinct CGuid from tempInfo where PGuid is null)
                                             order by POrder desc, COrder desc"
                                             , appUid
                                             , userGuid);

            return dbHelper.ExecuteDataView(querySql, connection);
        }

        /// <summary>
        ///根据用户GUID删除授权设置
        /// </summary>
        /// <param name="userGuid">用户Guid</param>
        /// <param name="appUid">系统因子类型Uid</param>
        /// <param name="paramType">个性化类型（port：站点、pollutant：因子）</param>
        /// <param name="notDelParamUid">id1','id2','id3</param>
        public void DelPersonalizedSetByUserGuid(string userGuid, string appUid, 
                                                 string paramType, string notDelParamUid = null)
        {
            string deleteSql = null;

            if (notDelParamUid == null)
            {
                deleteSql = string.Format(@"
                                            delete [BasicInfo].[TB_PersonalizedSettings] 
                                             where UserUid = '{0}' 
                                               and ApplicationUid = '{1}' 
                                               and ParameterType= '{2}'
                                            ", userGuid
                                             , appUid
                                             , paramType);
            }
            else
            {
                deleteSql = string.Format(@"
                                            delete [BasicInfo].[TB_PersonalizedSettings] 
                                             where UserUid = '{0}' 
                                               and ApplicationUid = '{1}' 
                                               and ParameterType = '{2}'
                                               and ParameterUid not in ('{3}')
                                            ", userGuid
                                             , appUid
                                             , paramType
                                             , notDelParamUid);
            }

            dbHelper.ExecuteNonQuery(deleteSql, connection);
        }

        /// <summary>
        /// 更新用户授权设置
        /// </summary>
        /// <param name="userGuid">用户Guid</param>
        /// <param name="paramUid">参数Uid</param>
        /// <param name="paramType">个性化类型（port：站点、pollutant：因子）</param>
        /// <param name="enable">是否启用（1：启用、0：禁用）</param>
        public void UpdatePersonalizedSetByUserGuid(string userGuid, string paramUid,
                                                    string paramType, string enable)
        {
            string updateSql = string.Format(@"
                                            update [BasicInfo].[TB_PersonalizedSettings] 
                                               set EnableCustomOrNot = {0} 
                                             where UserUid = '{1}' 
                                               and ParameterUid = '{2}' 
                                               and ParameterType= '{3}'
                                            ", enable
                                             , userGuid
                                             , paramUid
                                             , paramType);

            dbHelper.ExecuteNonQuery(updateSql, connection);
        }

        /// <summary>
        /// 添加用户授权设置
        /// </summary>
        /// <param name="userGuid">用户Guid</param>
        /// <param name="appUid">系统因子类型Uid</param>
        /// <param name="paramType">个性化类型（port：站点、pollutant：因子）</param>
        /// <param name="paramUids">参数Uid列表</param>
        public void AddPersonalizedSet(string userGuid, string appUid, 
                                       string paramType, string [] paramUids)
        {
            if (paramUids == null || paramUids.Length == 0)
            {
                return;
            }

            string tblName = "[BasicInfo].[TB_PersonalizedSettings]";
            DataTable dataDt = new DataTable(tblName);
            dataDt.Columns.Add(new DataColumn("ApplicationUid", Type.GetType("System.String")));
            dataDt.Columns.Add(new DataColumn("UserUid", Type.GetType("System.String")));
            dataDt.Columns.Add(new DataColumn("ParameterUid", Type.GetType("System.String")));
            dataDt.Columns.Add(new DataColumn("ParameterType", Type.GetType("System.String")));
            dataDt.Columns.Add(new DataColumn("EnableCustomOrNot", Type.GetType("System.Int32")));
            DataRow dr = null;

            foreach (string paramUid in paramUids)
            {
                dr = dataDt.NewRow();
                dr["ApplicationUid"] = appUid;
                dr["UserUid"] = userGuid;
                dr["ParameterUid"] = paramUid;
                dr["ParameterType"] = paramType;
                dr["EnableCustomOrNot"] = 1;

                dataDt.Rows.Add(dr);
            }

            string connStr = ConfigurationManager.ConnectionStrings[connection].ConnectionString;
            SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(connStr, SqlBulkCopyOptions.UseInternalTransaction);

            sqlbulkcopy.DestinationTableName = tblName;//目标表表名
            sqlbulkcopy.ColumnMappings.Add("[ApplicationUid]", "[ApplicationUid]");
            sqlbulkcopy.ColumnMappings.Add("[UserUid]", "[UserUid]");
            sqlbulkcopy.ColumnMappings.Add("[ParameterUid]", "[ParameterUid]");
            sqlbulkcopy.ColumnMappings.Add("[ParameterType]", "[ParameterType]");
            sqlbulkcopy.ColumnMappings.Add("[EnableCustomOrNot]", "[EnableCustomOrNot]");

            sqlbulkcopy.WriteToServer(dataDt);
            sqlbulkcopy.Close();
        }
    }
}
