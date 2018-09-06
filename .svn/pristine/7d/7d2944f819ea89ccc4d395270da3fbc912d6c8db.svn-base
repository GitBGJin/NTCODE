using SmartEP.Core.Generic;
using SmartEP.Data.Enums;
using SmartEP.Utilities.AdoData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.QualityControlOperation.Air
{
    public class MaintenanceDAL
    {
        /// 数据库处理类
        /// </summary>
        protected DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connectionMonitor = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.MonitoringBusiness);
        private string connectionFrame = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.Frame);
        /// <summary>
        /// 数据库表名
        /// </summary>
        private string tableName = "dbo.TB_WaterInspectionBase";
        #region << 数据查询方法 >>
        /// <summary>
        /// 根据站点guid查询仪器
        /// </summary>
        /// <param name="pointGuids">站点guid</param>
        /// <returns></returns>
        public DataView GetInstanceByGuid(string[] pointGuids, string objectType, string IsSpareParts)
        {
            string tableName1 = "TB_OMMP_InstrumentInstance";
            string tableName2 = "TB_OMMP_MaintenanceObject";
            string tableName3 = "TB_OMMP_InstrumentInfo";
            int intObjectType = int.TryParse(objectType, out intObjectType) ? int.Parse(objectType) : 2;
            try
            {
                string where = " where a.[ObjectType]=" + objectType;
                if (pointGuids != null && pointGuids.Length > 0)
                {
                    string strpointguid = "'" + string.Join("','", pointGuids) + "'";
                    where += string.Format(@" and b.ObjectID in ({0})", strpointguid);
                }
                if (!string.IsNullOrWhiteSpace(IsSpareParts))
                {
                    where += string.Format(@" and a.IsSpareParts ={0}", IsSpareParts);
                }
                string sql = string.Format(@"SELECT a.RowGuid
                                                   ,a.RowGuid InstrumentInstanceGuid
                                                   ,[InstanceName]
                                                   ,[FixedAssetNumber]
                                                   ,[InstanceName]+'/'+[FixedAssetNumber] as instance
                                                   ,b.ObjectID
	                                               ,b.ObjectName 
                                                   ,c.SpecificationModel
                                             FROM {0} as a left join {1} as b on a.SiteGuid=b.RowGuid
                                                    left join {2} as c on a.InfoGuid=c.RowGuid
                                              {3} ",
                                           tableName1, tableName2, tableName3, where);
                return g_DatabaseHelper.ExecuteDataView(sql, connectionFrame);
            }
            catch (Exception ex) { throw ex; }

        }
        /// <summary>
        /// 查询可用配件信息
        /// </summary>
        /// <param name="FittingName">配件名</param>
        /// <returns></returns>
        public DataView GetFittingInstance(string FittingName, string ObjectType)
        {
            string where = string.Empty;
            try
            {
                string sql = string.Format(@"   select b.RowGuid,b.InstanceName,b.FixedAssetNumber,a.Note
                                                from [EQMS_Framework].[dbo].[TB_OMMP_InstrumentInstanceSite] a
                                                left join
                                                [EQMS_Framework].[dbo].[TB_OMMP_InstrumentInstance] b
                                                on a.InstanceGuid=b.RowGuid 
                                                where IsSpareParts=1 and a.Status=0
                                                and b.ObjectType={1}
                                                and a.ID in(
                                                select MAX(ID) from [EQMS_Framework].[dbo].[TB_OMMP_InstrumentInstanceSite] 
                                                group by [InstanceGuid]
                                           )
                                                and b.InstanceName like '%{0}%'", FittingName, ObjectType);

                return g_DatabaseHelper.ExecuteDataView(sql, connectionFrame);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 查询仪器的配件具体信息
        /// </summary>
        /// <param name="InstrumentInstanceGuid">具体仪器实例GUID</param>
        /// <returns></returns>
        public DataView GetInstrumentFittingInstance(string InstrumentInstanceGuid, string FittingName)
        {
            try
            {
                string sql = string.Format(@" select * FROM [EQMS_Framework].[dbo].[TB_OMMP_InstrumentFittingInstance]
                                              where [InstrumentInstanceGuid]='{0}'
                                              and [FittingName] like '%{1}%'
                                                ", InstrumentInstanceGuid, FittingName);
                return g_DatabaseHelper.ExecuteDataView(sql, connectionFrame);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 查询仪器的配件
        /// </summary>
        /// <param name="rowguid">仪器GUID</param>
        /// <returns></returns>
        public DataView GetFitting(string rowguid)
        {
            string tableName1 = "TB_OMMP_Fitting";
            string tableName2 = "TB_OMMP_InstrumentFitting";
            string tableName3 = "TB_OMMP_InstrumentInstance";
            try
            {
                string sql = string.Format(@"SELECT a.[InstanceName]     
                                                   ,a.[FixedAssetNumber]
                                                   ,c.FittingName
                                                   ,c.RowGuid     
                                             FROM {0} as c  left join {1} as b on b.FittingGuid=c.RowGuid  left join {2} as a  on a.InfoGuid=b.InstrumentGuid
                                             where a.RowGuid='{3}'",
                                           tableName1, tableName2, tableName3, rowguid);
                return g_DatabaseHelper.ExecuteDataView(sql, connectionFrame);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 查询仪器清单
        /// </summary>
        /// <param name="FixedAssetNumbers">仪器编号</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetInstanceList(string[] FixedAssetNumbers, DateTime dtStart, DateTime dtEnd)
        {
            string tableName1 = "TB_OMMP_InstrumentInstance";
            string tableName2 = "TB_OMMP_InstrumentInstanceSite";
            string tableName3 = "TB_OMMP_InstrumentInfo";
            string strFixedAssetNumber = "'" + string.Join("','", FixedAssetNumbers) + "'";
            try
            {
                string sql = string.Format(@"SELECT d.ObjectName
                                                    ,b.RowGuid InstrumentInstanceGuid
                                                    ,b.InstanceName
                                                    ,c.SpecificationModel
                                                    ,b.FixedAssetNumber
                                                    ,a.OperateDate
                                                    ,case when a.Status='1' then '出库' when a.Status='0' then '入库' end as Status
                                                    ,b.SiteGuid
                                                    ,a.Note
                                                    FROM {0} as a left join {1} as b on a.InstanceGuid=b.RowGuid left join {2} as c on b.InfoGuid=c.RowGuid
                                                            left join TB_OMMP_MaintenanceObject as d on b.SiteGuid=d.RowGuid
                                                    where a.RowStatus='1' and  OperateDate>='{3}' and OperateDate<='{4}'
                                                    and a.ID in
                                                    (select MAX([ID])
                                                    from [EQMS_Framework].[dbo].[TB_OMMP_InstrumentInstanceSite]
                                                    group by [RowGuid])
                                                    and FixedAssetNumber in ({5})
                                                    order by d.ObjectID,a.OperateDate",
                                           tableName2, tableName1, tableName3, dtStart, dtEnd, strFixedAssetNumber);
                return g_DatabaseHelper.ExecuteDataView(sql, connectionFrame);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 获取仪器所在测点
        /// </summary>
        /// <param name="InstrumentInstanceGuid">仪器实例Guid</param>
        /// <returns></returns>
        public string GetInstanceSite(string InstrumentInstanceGuid)
        {
            try
            {
                string sql = string.Format(@"SELECT TOP 1 point.ObjectName
                                            FROM [EQMS_Framework].[dbo].[TB_OMMP_InstrumentInstanceSite] state
                                            left join [EQMS_Framework].[dbo].[TB_OMMP_InstrumentInstance] ins
                                            on state.[InstanceGuid]=ins.[RowGuid]
                                            left join [EQMS_Framework].[dbo].[TB_OMMP_MaintenanceObject] point
                                            on ins.SiteGuid=point.RowGuid
                                            where [InstanceGuid]='{0}'
                                            order by state.[ID] desc", InstrumentInstanceGuid);
                return g_DatabaseHelper.ExecuteScalar(sql, connectionFrame).ToString();

            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 向出入库中添加仪器/备件的出入库信息
        /// </summary>
        /// <param name="status">出入库状态</param>
        /// <param name="note">提示内容</param>
        /// <param name="InstanceGuid">仪器/配件实例Guid</param>
        /// <returns></returns>
        public int InsertInstrumentInstanceSite(int status, string note, string InstanceGuid)
        {
            try
            {
                string sql = string.Format(@"insert into [EQMS_Framework].[dbo].[TB_OMMP_InstrumentInstanceSite]
                                            ([RowGuid],[Status],[OperateDate],[RowStatus],[Note],[InstanceGuid])
                                            values
                                            (NEWID(),{0},GETDATE(),1,'{1}','{2}')", status, note, InstanceGuid);
                return g_DatabaseHelper.ExecuteInsert(sql, connectionFrame);

            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 查看仪器已选择当前配件类型的配件实例Guid
        /// </summary>
        /// <param name="InstrumentInstanceGuid">仪器实例Guid</param>
        ///<param name="FittingGuid">配件类型Guid</param>
        /// <returns></returns>
        public string GetIsFitting(string InstrumentInstanceGuid, string FittingGuid)
        {
            try
            {
                string sql = string.Format(@"select [FittingInstanceGuid] from [EQMS_Framework].[dbo].[TB_OMMP_InstrumentFittingInstance]
                                            where [InstrumentInstanceGuid]='{0}'
                                            and FittingGuid='{1}'", InstrumentInstanceGuid, FittingGuid);
                return g_DatabaseHelper.ExecuteScalar(sql, connectionFrame).ToString();

            }
            catch (Exception ex) { return null; }
        }
        /// <summary>
        /// 查看仪器已选择当前配件类型的配对RowGuid
        /// </summary>
        /// <param name="InstrumentInstanceGuid">仪器实例Guid</param>
        ///<param name="FittingGuid">配件类型Guid</param>
        /// <returns></returns>
        public string GetIsFittingKey(string InstrumentInstanceGuid, string FittingGuid)
        {
            try
            {
                string sql = string.Format(@"select [RowGuid] from [EQMS_Framework].[dbo].[TB_OMMP_InstrumentFittingInstance]
                                            where [InstrumentInstanceGuid]='{0}'
                                            and FittingGuid='{1}'", InstrumentInstanceGuid, FittingGuid);
                return g_DatabaseHelper.ExecuteScalar(sql, connectionFrame).ToString();

            }
            catch (Exception ex) { return null; }
        }
        /// 查询仪器出入记录
        /// </summary>
        /// <param name="FixedAssetNumbers">仪器编号</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetInstanceSite(string[] FixedAssetNumbers, DateTime dtStart, DateTime dtEnd)
        {
            string tableName1 = "TB_OMMP_InstrumentInstance";
            string tableName2 = "TB_OMMP_InstrumentInstanceSite";
            string tableName3 = "TB_OMMP_InstrumentInfo";
            string strFixedAssetNumber = "'" + string.Join("','", FixedAssetNumbers) + "'";
            try
            {
                string sql = string.Format(@"SELECT d.ObjectName
                                                    ,b.RowGuid InstrumentInstanceGuid
                                                    ,b.InstanceName
                                                    ,c.SpecificationModel
                                                    ,b.FixedAssetNumber
                                                    ,a.OperateDate
                                                    ,case when a.Status='1' then '出库' when a.Status='0' then '入库' end as Status
                                                    ,b.SiteGuid
                                                    ,a.Note
                                                    FROM {0} as a left join {1} as b on a.InstanceGuid=b.RowGuid left join {2} as c on b.InfoGuid=c.RowGuid
                                                            left join TB_OMMP_MaintenanceObject as d on b.SiteGuid=d.RowGuid
                                                    where a.RowStatus='1' and  OperateDate>='{3}' and OperateDate<='{4}' and FixedAssetNumber in ({5})
                                                    order by d.ObjectID,a.OperateDate",
                                           tableName2, tableName1, tableName3, dtStart, dtEnd, strFixedAssetNumber);
                return g_DatabaseHelper.ExecuteDataView(sql, connectionFrame);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 查询仪器出入记录
        /// </summary>
        /// <param name="pointGuids">站点guid</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetInstanceSiteByPoint(string[] pointGuids, DateTime dtStart, DateTime dtEnd)
        {
            string tableName1 = "TB_OMMP_InstrumentInstance";
            string tableName2 = "TB_OMMP_InstrumentInstanceSite";
            string tableName3 = "TB_OMMP_InstrumentInfo";
            string strPoints = "'" + string.Join("','", pointGuids) + "'";
            try
            {
                string sql = string.Format(@"SELECT case when a.Status='1' then '出库' when a.Status='0' then '入库' end as Status
                                                    ,b.RowGuid InstrumentInstanceGuid
                                                    ,a.OperateDate
                                                    ,a.Note,b.FixedAssetNumber
                                                    ,b.InstanceName
                                                    ,b.SiteGuid
                                                    ,c.SpecificationModel
                                                    ,d.ObjectName
                                                    FROM {0} as a left join {1} as b on a.InstanceGuid=b.RowGuid left join {2} as c on b.InfoGuid=c.RowGuid
                                                            left join TB_OMMP_MaintenanceObject as d on b.SiteGuid=d.RowGuid
                                                    where a.RowStatus='1' and  OperateDate>='{3}' and OperateDate<='{4}' and d.ObjectID in ({5})",
                                           tableName2, tableName1, tableName3, dtStart, dtEnd, strPoints);
                return g_DatabaseHelper.ExecuteDataView(sql, connectionFrame);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="pointId">站点编号</param>
        /// <param name="pollingDate">巡检时间</param>
        /// <returns></returns>
        public DataTable GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@" SELECT *  FROM ");
            strSql.Append(tableName);
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), connectionMonitor);
        }
        /// <summary>
        /// 仪器使用信息
        /// </summary>
        /// <param name="FixedAssetNumbers">仪器编号</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <returns></returns>
        public DataTable GetListByInstrument(string[] FixedAssetNumbers, DateTime dtStart, DateTime dtEnd)
        {
            try
            {
                string tableName1 = "TB_OMMP_InstrumentInstance";
                string tableName2 = "TB_OMMP_InstrumentInstanceState";
                string tableName3 = "TB_OMMP_InstrumentInfo";
                string tableName4 = "TB_OMMP_Room";
                string tableName5 = "TB_OMMP_InstrumentInstanceRecord4";
                string FixedAssetNumber = "'" + string.Join("','", FixedAssetNumbers) + "'";
                string where = string.Format(@"OperateDate>='{0}' and OperateDate<='{1}' and FixedAssetNumber in ({2})", dtStart, dtEnd, FixedAssetNumber);
                string sql = string.Format(@"SELECT [InstanceName]
                                                    ,[InstanceName]+'/'+[FixedAssetNumber] as instance
                                                    ,[InfoGuid]
                                                    ,[FixedAssetNumber] 
                                                    ,e.OperateDate
                                                    ,[SiteGuid]
                                                    ,[AreaGuid]
                                                    ,[Status]
                                                    ,[Room]
                                                    ,b.State
                                                    ,c.RoomName
                                                    ,e.OperateUserName
                                                    ,d.InstrumentName
                                                    ,e.[OperateContent]
                                            FROM {0} as e left join {1} as a on e.InstrumentInstanceGuid=a.RowGuid left join {2} as b on a.Status=b.RowGuid
                                            left join {3} as c on a.Room=c.RowGuid left join {4} as d on a.InfoGuid=d.RowGuid where {5}"
                    , tableName5, tableName1, tableName2, tableName4, tableName3, where);
                return g_DatabaseHelper.ExecuteDataTable(sql, connectionFrame);
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion
    }
}
