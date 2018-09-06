using SmartEP.Core.Generic;
using SmartEP.DomainModel.WaterQualityControlOperation;
using SmartEP.Utilities.AdoData;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.QualityControlOperation.Water
{
    /// <summary>
    /// 名称：AbnormalConfigDAL.cs
    /// 创建人：窦曙健
    /// 创建日期：2015-10-9
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 异常配置表数据访问类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class AbnormalConfigDAL
    {
        protected DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();

        #region  Method
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(AbnormalConfigEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            StringBuilder strSql1 = new StringBuilder();
            StringBuilder strSql2 = new StringBuilder();
            if (model.AbnormalName != null)
            {
                strSql1.Append("AbnormalName,");
                strSql2.Append("'" + model.AbnormalName + "',");
            }
            if (model.AbnormalItemType != null)
            {
                strSql1.Append("AbnormalItemType,");
                strSql2.Append("'" + model.AbnormalItemType + "',");
            }
            strSql.Append("insert into TB_AbnormalConfig(");
            strSql.Append(strSql1.ToString().Remove(strSql1.Length - 1));
            strSql.Append(")");
            strSql.Append(" values (");
            strSql.Append(strSql2.ToString().Remove(strSql2.Length - 1));
            strSql.Append(")");
            strSql.Append(";select @@IDENTITY");
            object obj = g_DatabaseHelper.ExecuteScalar(strSql.ToString(), ConnectionString);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(AbnormalConfigEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update TB_AbnormalConfig set ");
            if (model.AbnormalName != null)
            {
                strSql.Append("AbnormalName='" + model.AbnormalName + "',");
            }
            else
            {
                strSql.Append("AbnormalName= null ,");
            }
            if (model.AbnormalItemType != null)
            {
                strSql.Append("AbnormalItemType='" + model.AbnormalItemType + "',");
            }
            else
            {
                strSql.Append("AbnormalItemType= null ,");
            }
            int n = strSql.ToString().LastIndexOf(",");
            strSql.Remove(n, 1);
            strSql.Append(" where id=" + model.id + "");
            int rowsAffected = Convert.ToInt32(g_DatabaseHelper.ExecuteScalar(strSql.ToString(), ConnectionString));
            if (rowsAffected > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from TB_AbnormalConfig ");
            strSql.Append(" where id=" + id + "");
            int rowsAffected = Convert.ToInt32(g_DatabaseHelper.ExecuteScalar(strSql.ToString(), ConnectionString));
            if (rowsAffected > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,AbnormalName,AbnormalItemType ");
            strSql.Append(" FROM TB_AbnormalConfig ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), ConnectionString);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetInstrumentNew(string[] pointIds)
        {
            //查询条件拼接
            string objectId = "'" + StringExtensions.GetArrayStrNoEmpty(pointIds.ToList<string>(), "','") + "'";
            if (pointIds.Length > 0) objectId = string.Format(" ObjectID IN ({0}) ", objectId);

            string sql = string.Format(@"select tb.RowGuid,data.MonitoringPointName as PointName,data.PointId  from TB_OMMP_MaintenanceObject as tb
                                         left join [AMS_BaseData].[MPInfo].[TB_MonitoringPoint] as data
                                         on tb.ObjectID=data.MonitoringPointUid
                                         where {0} and RowStatus=1", objectId);
            DataTable dt = g_DatabaseHelper.ExecuteDataTable(sql, ConnectionString1);
            string SiteGuid = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == 0)
                    SiteGuid += " iis.SiteGuid='" + dt.Rows[i]["RowGuid"].ToString() + "'";
                else
                    SiteGuid += " Or iis.SiteGuid='" + dt.Rows[i]["RowGuid"].ToString() + "'";
            }
            string strsql = @"SELECT iis.RowGuid, iis.InfoGuid, iis.InstanceName, iis.BuyPrice, iis.BuyDate, iis.FactoryDate, iis.QualityDate, iis.MaintainDate, iis.FactoryNumber, iis.FixedAssetNumber, iis.TeamGuid, iis.TeamName, 
                      iis.SiteGuid, iis.AreaGuid, iis.RowStatus, iis.Status, iis.StatusNote, iis.Note, iis.IsSpareParts, iis.Keeper, iis.MaintenanceCycle, iis.CycleUnit, iis.DistinguishID, iis.DeviceType, iis.BeginUseDate, 
                      iis.ManageNumber, iis.DeptGuid, iis.Room, info.InstrumentName AS InstrumentInfoName, info.InstrumentType, info.SpecificationModel, info.Application, info.ReferencePrice, info.Manufacturer, 
                      info.CheckFactor, info.Range, info.ControlMeasures, info.UseConditions, info.UseMeasures, info.AllCount, info.WarningCount, info.QualityDate AS InfoQualityDate, info.QualityType
                      FROM dbo.TB_OMMP_InstrumentInstance AS iis LEFT OUTER JOIN
                      dbo.TB_OMMP_InstrumentInfo AS info ON iis.InfoGuid = info.RowGuid
                      WHERE (iis.RowStatus = 1) AND (info.RowStatus = 1) ";
            if (!string.IsNullOrEmpty(SiteGuid))
            {
                strsql += " and (" + SiteGuid + ")";
            }
            string strorder = " ORDER BY info.ID DESC, iis.ID DESC";
            DataTable dtNew = g_DatabaseHelper.ExecuteDataTable(strsql + strorder, ConnectionString1);
            dtNew.Columns.Add("PointId", typeof(int));
            for (int j = 0; j < dtNew.Rows.Count; j++)
            {
                string guid = dtNew.Rows[j]["SiteGuid"].ToString();
                DataRow[] dr = dt.Select("RowGuid='" + guid + "'");
                if (dr.Length > 0)
                {
                    dtNew.Rows[j]["InstanceName"] = dr[0]["PointName"] + "/" + dtNew.Rows[j]["InstanceName"];
                    dtNew.Rows[j]["PointId"] = Convert.ToInt32(dr[0]["PointId"]);
                }
            }
            DataView dv = dtNew.DefaultView;
            dv.Sort = "PointId ASC";
            return dv.ToTable();

        }
        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataTable GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" id,AbnormalName,AbnormalItemType ");
            strSql.Append(" FROM TB_AbnormalConfig ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), ConnectionString);
        }

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        private string ConnectionString
        {
            get
            {
                return "AMS_BaseDataConnection";
            }
        }
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        private string ConnectionString1
        {
            get
            {
                return "Frame_Connection";
            }
        }
        #endregion  Method
    }
}
