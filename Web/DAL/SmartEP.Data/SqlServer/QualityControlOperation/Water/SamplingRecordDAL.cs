using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartEP.Utilities.AdoData;
using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using System.Data;
using System.Data.SqlClient;
using SmartEP.Data.SqlServer.Common.GridView;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.WaterQualityControlOperation;

namespace SmartEP.Data.SqlServer.QualityControlOperation.Water
{
    /// <summary>
    /// 名称：SamplingRecordDAL.cs
    /// 创建人：王秉晟
    /// 创建日期：2015-09-23
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 采样记录数据库处理
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class SamplingRecordDAL
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
        /// 数据库表名（主表）
        /// </summary>
        private string tableNameMain = "dbo.TB_SamplingRecord";

        /// <summary>
        /// 数据库表名（详情表）
        /// </summary>
        private string tableNameDetail = "dbo.TB_SamplingRecordDetail";
        #endregion

        #region << 构造函数 >>
        /// <summary>
        /// 构造函数
        /// </summary>
        public SamplingRecordDAL()
        {
            //connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(ApplicationType.Water, PollutantDataType.Hour);
        }
        #endregion

        #region << 方法 >>
        /// <summary>
        /// 批量增加数据
        /// </summary>
        /// <param name="model">采样记录实体</param>
        /// <returns></returns>
        public int Add(params SamplingRecordEntity[] models)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                foreach (SamplingRecordEntity model in models)
                {
                    StringBuilder strSql1 = new StringBuilder();
                    StringBuilder strSql2 = new StringBuilder();
                    if (model.id != null)
                    {
                        strSql1.Append("id,");
                        strSql2.Append("'" + model.id + "',");
                    }
                    if (model.MissionID != null)
                    {
                        strSql1.Append("MissionID,");
                        strSql2.Append("'" + model.MissionID + "',");
                    }
                    if (model.ActionID != null)
                    {
                        strSql1.Append("ActionID,");
                        strSql2.Append("'" + model.ActionID + "',");
                    }
                    if (model.SamplingDate != null)
                    {
                        strSql1.Append("SamplingDate,");
                        strSql2.Append("'" + model.SamplingDate + "',");
                    }
                    if (model.SamplingGoal != null)
                    {
                        strSql1.Append("SamplingGoal,");
                        strSql2.Append("'" + model.SamplingGoal + "',");
                    }
                    if (model.AnalysisBatchNumber != null)
                    {
                        strSql1.Append("AnalysisBatchNumber,");
                        strSql2.Append("'" + model.AnalysisBatchNumber + "',");
                    }
                    if (model.PointId != null)
                    {
                        strSql1.Append("PointId,");
                        strSql2.Append("" + model.PointId + ",");
                    }
                    if (model.PointName != null)
                    {
                        strSql1.Append("PointName,");
                        strSql2.Append("'" + model.PointName + "',");
                    }
                    if (model.Description != null)
                    {
                        strSql1.Append("Description,");
                        strSql2.Append("'" + model.Description + "',");
                    }
                    if (model.SamplingUser != null)
                    {
                        strSql1.Append("SamplingUser,");
                        strSql2.Append("'" + model.SamplingUser + "',");
                    }
                    if (model.RecordUser != null)
                    {
                        strSql1.Append("RecordUser,");
                        strSql2.Append("'" + model.RecordUser + "',");
                    }
                    if (model.AuditUser != null)
                    {
                        strSql1.Append("AuditUser,");
                        strSql2.Append("'" + model.AuditUser + "',");
                    }
                    if (model.CreatUser != null)
                    {
                        strSql1.Append("CreatUser,");
                        strSql2.Append("'" + model.CreatUser + "',");
                    }
                    if (model.CreatDateTime != null)
                    {
                        strSql1.Append("CreatDateTime,");
                        strSql2.Append("'" + model.CreatDateTime + "',");
                    }
                    if (model.UpdateUser != null)
                    {
                        strSql1.Append("UpdateUser,");
                        strSql2.Append("'" + model.UpdateUser + "',");
                    }
                    if (model.UpdateDateTime != null)
                    {
                        strSql1.Append("UpdateDateTime,");
                        strSql2.Append("'" + model.UpdateDateTime + "',");
                    }
                    if (model.TaskCode != null)
                    {
                        strSql1.Append("TaskCode,");
                        strSql2.Append("'" + model.TaskCode + "',");
                    }
                    strSql.Append(" insert into TB_SamplingRecord(");
                    strSql.Append(strSql1.ToString().Remove(strSql1.Length - 1));
                    strSql.Append(")");
                    strSql.Append(" values (");
                    strSql.Append(strSql2.ToString().Remove(strSql2.Length - 1));
                    strSql.Append("); ");
                }
                strSql.Append(";select @@ROWCOUNT");
                //g_DatabaseHelper.ExecuteNonQuery(strSql.ToString(), connection);
                object obj = g_DatabaseHelper.ExecuteScalar(strSql.ToString(), connection);
                if (obj == null)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(obj);
                }
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model">采样记录实体</param>
        /// <returns></returns>
        public int Update(params SamplingRecordEntity[] models)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                foreach (SamplingRecordEntity model in models)
                {
                    strSql.Append(" update TB_SamplingRecord set ");
                    if (model.MissionID != null)
                    {
                        strSql.Append("MissionID='" + model.MissionID + "',");
                    }
                    else
                    {
                        strSql.Append("MissionID= null ,");
                    }
                    if (model.ActionID != null)
                    {
                        strSql.Append("ActionID='" + model.ActionID + "',");
                    }
                    else
                    {
                        strSql.Append("ActionID= null ,");
                    }
                    if (model.SamplingDate != null)
                    {
                        strSql.Append("SamplingDate='" + model.SamplingDate + "',");
                    }
                    else
                    {
                        strSql.Append("SamplingDate= null ,");
                    }
                    if (model.SamplingGoal != null)
                    {
                        strSql.Append("SamplingGoal='" + model.SamplingGoal + "',");
                    }
                    else
                    {
                        strSql.Append("SamplingGoal= null ,");
                    }
                    if (model.AnalysisBatchNumber != null)
                    {
                        strSql.Append("AnalysisBatchNumber='" + model.AnalysisBatchNumber + "',");
                    }
                    else
                    {
                        strSql.Append("AnalysisBatchNumber= null ,");
                    }
                    if (model.PointId != null)
                    {
                        strSql.Append("PointId=" + model.PointId + ",");
                    }
                    else
                    {
                        strSql.Append("PointId= null ,");
                    }
                    if (model.PointName != null)
                    {
                        strSql.Append("PointName='" + model.PointName + "',");
                    }
                    else
                    {
                        strSql.Append("PointName= null ,");
                    }
                    if (model.Description != null)
                    {
                        strSql.Append("Description='" + model.Description + "',");
                    }
                    else
                    {
                        strSql.Append("Description= null ,");
                    }
                    if (model.SamplingUser != null)
                    {
                        strSql.Append("SamplingUser='" + model.SamplingUser + "',");
                    }
                    else
                    {
                        strSql.Append("SamplingUser= null ,");
                    }
                    if (model.RecordUser != null)
                    {
                        strSql.Append("RecordUser='" + model.RecordUser + "',");
                    }
                    else
                    {
                        strSql.Append("RecordUser= null ,");
                    }
                    if (model.AuditUser != null)
                    {
                        strSql.Append("AuditUser='" + model.AuditUser + "',");
                    }
                    else
                    {
                        strSql.Append("AuditUser= null ,");
                    }
                    if (model.CreatUser != null)
                    {
                        strSql.Append("CreatUser='" + model.CreatUser + "',");
                    }
                    else
                    {
                        strSql.Append("CreatUser= null ,");
                    }
                    if (model.CreatDateTime != null)
                    {
                        strSql.Append("CreatDateTime='" + model.CreatDateTime + "',");
                    }
                    else
                    {
                        strSql.Append("CreatDateTime= null ,");
                    }
                    if (model.UpdateUser != null)
                    {
                        strSql.Append("UpdateUser='" + model.UpdateUser + "',");
                    }
                    else
                    {
                        strSql.Append("UpdateUser= null ,");
                    }
                    if (model.UpdateDateTime != null)
                    {
                        strSql.Append("UpdateDateTime='" + model.UpdateDateTime + "',");
                    }
                    else
                    {
                        strSql.Append("UpdateDateTime= null ,");
                    }
                    if (model.TaskCode != null)
                    {
                        strSql.Append("TaskCode='" + model.TaskCode + "',");
                    }
                    else
                    {
                        strSql.Append("TaskCode= null ,");
                    }
                    int n = strSql.ToString().LastIndexOf(",");
                    strSql.Remove(n, 1);
                    strSql.Append(" where id='" + model.id + "'; ");
                }
                strSql.Append(";select @@ROWCOUNT");
                //g_DatabaseHelper.ExecuteNonQuery(strSql.ToString(), connection);
                object obj = g_DatabaseHelper.ExecuteScalar(strSql.ToString(), connection);
                if (obj == null)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(obj);
                }
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public int Delete(Guid id)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(" delete from TB_SamplingRecordDetail ");
                strSql.Append(" where SamplingMainGuid='" + id + "'; ");
                strSql.Append(" delete from TB_SamplingRecord ");
                strSql.Append(" where id='" + id + "'; ");
                strSql.Append(";select @@ROWCOUNT");
                //g_DatabaseHelper.ExecuteNonQuery(strSql.ToString(), connection);
                object obj = g_DatabaseHelper.ExecuteScalar(strSql.ToString(), connection);
                if (obj == null)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(obj);
                }
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="idlist">id数组</param>
        /// <returns></returns>
        public int DeleteList(string[] idlist)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                string sqlId = "'" + idlist.Aggregate((t, m) => t + "','" + m) + "'";
                strSql.Append("delete from TB_SamplingRecord ");
                strSql.Append(" where id in (" + sqlId + ")  ");
                strSql.Append(";select @@ROWCOUNT");
                //g_DatabaseHelper.ExecuteNonQuery(strSql.ToString(), connection);
                object obj = g_DatabaseHelper.ExecuteScalar(strSql.ToString(), connection);
                if (obj == null)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(obj);
                }
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public DataTable GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@" select id,MissionID,ActionID,SamplingDate,SamplingGoal,AnalysisBatchNumber,PointId,PointName,Description,SamplingUser,RecordUser,AuditUser,CreatUser,CreatDateTime,UpdateUser,UpdateDateTime,TaskCode ");
            strSql.Append(" FROM TB_SamplingRecord ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), connection);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public SamplingRecordEntity GetModel(Guid id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1  ");
            strSql.Append(" id,MissionID,ActionID,SamplingDate,SamplingGoal,AnalysisBatchNumber,PointId,PointName,Description,SamplingUser,RecordUser,AuditUser,CreatUser,CreatDateTime,UpdateUser,UpdateDateTime,TaskCode ");
            strSql.Append(" from TB_SamplingRecord ");
            strSql.Append(" where id='" + id + "' ");
            SamplingRecordEntity model = new SamplingRecordEntity();
            DataTable dt = g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), connection);
            if (dt.Rows.Count > 0)
            {
                return DataRowToModel(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public SamplingRecordEntity DataRowToModel(DataRow row)
        {
            SamplingRecordEntity model = new SamplingRecordEntity();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = new Guid(row["id"].ToString());
                }
                if (row["MissionID"] != null && row["MissionID"].ToString() != "")
                {
                    model.MissionID = row["MissionID"].ToString();
                }
                if (row["ActionID"] != null && row["ActionID"].ToString() != "")
                {
                    model.ActionID = row["ActionID"].ToString();
                }
                if (row["SamplingDate"] != null && row["SamplingDate"].ToString() != "")
                {
                    model.SamplingDate = DateTime.Parse(row["SamplingDate"].ToString());
                }
                if (row["SamplingGoal"] != null)
                {
                    model.SamplingGoal = row["SamplingGoal"].ToString();
                }
                if (row["AnalysisBatchNumber"] != null)
                {
                    model.AnalysisBatchNumber = row["AnalysisBatchNumber"].ToString();
                }
                if (row["PointId"] != null && row["PointId"].ToString() != "")
                {
                    model.PointId = int.Parse(row["PointId"].ToString());
                }
                if (row["PointName"] != null)
                {
                    model.PointName = row["PointName"].ToString();
                }
                if (row["Description"] != null)
                {
                    model.Description = row["Description"].ToString();
                }
                if (row["SamplingUser"] != null)
                {
                    model.SamplingUser = row["SamplingUser"].ToString();
                }
                if (row["RecordUser"] != null)
                {
                    model.RecordUser = row["RecordUser"].ToString();
                }
                if (row["AuditUser"] != null)
                {
                    model.AuditUser = row["AuditUser"].ToString();
                }
                if (row["CreatUser"] != null)
                {
                    model.CreatUser = row["CreatUser"].ToString();
                }
                if (row["CreatDateTime"] != null && row["CreatDateTime"].ToString() != "")
                {
                    model.CreatDateTime = DateTime.Parse(row["CreatDateTime"].ToString());
                }
                if (row["UpdateUser"] != null)
                {
                    model.UpdateUser = row["UpdateUser"].ToString();
                }
                if (row["UpdateDateTime"] != null && row["UpdateDateTime"].ToString() != "")
                {
                    model.UpdateDateTime = DateTime.Parse(row["UpdateDateTime"].ToString());
                }
                if (row["TaskCode"] != null)
                {
                    model.TaskCode = row["TaskCode"].ToString();
                }
            }
            return model;
        }

        /// <summary>
        /// 取得主表的虚拟分页查询数据和总行数
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetDataPager(string[] portIds, DateTime dtmStart, DateTime dtmEnd,
            int pageSize, int pageNo, out int recordTotal, string orderBy = "SamplingDate")
        {
            recordTotal = 0;
            try
            {
                //string idsStr = StringExtensions.GetArrayStrNoEmpty(ids.ToList<string>(), ",");
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                string dateStr = string.Empty;
                string keyName = "id";
                string fieldName = "id,MissionID,ActionID,SamplingDate,SamplingGoal,AnalysisBatchNumber,PointId,PointName,Description,SamplingUser,RecordUser,AuditUser,CreatUser,CreatDateTime,UpdateUser,UpdateDateTime,TaskCode ";
                string where = string.Empty;//查询条件拼接

                //if (ids.Length == 1 && !string.IsNullOrEmpty(ids[0]))
                //{
                //    idsStr = " AND id =" + idsStr;
                //}
                //else if (!string.IsNullOrEmpty(idsStr))
                //{
                //    idsStr = " AND id IN(" + idsStr + ")";
                //}
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND PointId =" + portIdsStr;
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = " AND PointId IN(" + portIdsStr + ")";
                }
                if (dtmStart != DateTime.MinValue)
                {
                    dateStr += string.Format(" AND  SamplingDate>='{0}'", dtmStart.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                if (dtmEnd != DateTime.MinValue)
                {
                    dateStr += string.Format(" AND  SamplingDate<='{0}'", dtmEnd.ToString("yyyy-MM-dd HH:mm:ss"));
                }

                orderBy = string.IsNullOrEmpty(orderBy) ? "SamplingDate" : orderBy;
                where = string.Format(" 1=1 {0} {1} ", portIdsStr, dateStr);
                return g_GridViewPager.GetGridViewPager(tableNameMain, fieldName, keyName, pageSize, pageNo, orderBy, where, connection, out recordTotal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 取得主表的虚拟分页查询数据和总行数
        /// </summary>
        /// <param name="missionId">MissionID</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetData(string missionId,DateTime dtmStart, DateTime dtmEnd,
            int pageSize, int pageNo, out int recordTotal, string orderBy = "SamplingDate")
        {
            recordTotal = 0;
            try
            {
                string missionStr=missionId;
                string dateStr = string.Empty;
                string keyName = "id";
                string fieldName = "id,MissionID,ActionID,SamplingDate,SamplingGoal,AnalysisBatchNumber,PointId,PointName,Description,SamplingUser,RecordUser,AuditUser,CreatUser,CreatDateTime,UpdateUser,UpdateDateTime,TaskCode ";
                string where = string.Empty;//查询条件拼接
                if (missionStr != null)
                {
                    missionStr = " AND MissionID ='" + missionStr + "'";
                }
                if (dtmStart != DateTime.MinValue)
                {
                    dateStr += string.Format(" AND  SamplingDate>='{0}'", dtmStart.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                if (dtmEnd != DateTime.MinValue)
                {
                    dateStr += string.Format(" AND  SamplingDate<='{0}'", dtmEnd.ToString("yyyy-MM-dd HH:mm:ss"));
                }

                orderBy = string.IsNullOrEmpty(orderBy) ? "SamplingDate" : orderBy;
                where = string.Format(" 1=1 {0} {1} ",missionStr, dateStr);
                return g_GridViewPager.GetGridViewPager(tableNameMain, fieldName, keyName, pageSize, pageNo, orderBy, where, connection, out recordTotal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 取得详情表的查询数据和总行数
        /// </summary>
        /// <param name="ids">主键数据</param>
        /// <param name="portIds">测点数据</param>
        /// <param name="sampleNumbers">样品编号</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">截止时间</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetDetailTableDataPager(string[] ids, string[] portIds, string[] sampleNumbers,
            DateTime dtmStart, DateTime dtmEnd, out int recordTotal, int pageSize = int.MaxValue, int pageNo = 0,
             string orderBy = "SamplingDate")
        {
            try
            {
                string idsStr = StringExtensions.GetArrayStrNoEmpty(ids.ToList<string>(), "','");
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                string sampleNumbersStr = StringExtensions.GetArrayStrNoEmpty(sampleNumbers.ToList<string>(), ",");
                string dateStr = string.Empty;
                string keyName = "id";
                string fieldName = "id,MissionID,ActionID,SamplingDate,SamplingGoal,AnalysisBatchNumber,PointId,PointName,Description,SamplingUser,RecordUser,AuditUser,CreatUser,CreatDateTime,UpdateUser,UpdateDateTime";
                string where = string.Empty;//查询条件拼接

                if (ids.Length == 1 && !string.IsNullOrEmpty(ids[0]))
                {
                    idsStr = " AND id ='" + idsStr + "'";
                }
                else if (!string.IsNullOrEmpty(idsStr))
                {
                    idsStr = " AND id IN('" + idsStr + "')";
                }
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND PointId =" + portIdsStr;
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = " AND PointId IN(" + portIdsStr + ")";
                }
                if (dtmStart != DateTime.MinValue)
                {
                    dateStr += string.Format(" AND  CreatDateTime>='{0}'", dtmStart.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                if (dtmEnd != DateTime.MinValue)
                {
                    dateStr += string.Format(" AND  CreatDateTime<='{0}'", dtmEnd.ToString("yyyy-MM-dd HH:mm:ss"));
                }

                orderBy = string.IsNullOrEmpty(orderBy) ? "SamplingDate" : orderBy;
                where = string.Format(" 1=1 {0} {1} {2} ", idsStr, portIdsStr, dateStr);

                return g_GridViewPager.GetGridViewPager(tableNameMain, fieldName, keyName, pageSize, pageNo, orderBy, where, connection, out recordTotal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 取得主表导出数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">截止时间</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetExportData(string[] portIds, DateTime dtmStart, DateTime dtmEnd, string orderBy = "SamplingDate")
        {
            try
            {
                StringBuilder sqlStringBuilder = new StringBuilder();
                //string idsStr = StringExtensions.GetArrayStrNoEmpty(ids.ToList<string>(), ",");
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                string dateStr = string.Empty;
                string keyName = "id";
                string fieldName = "id,ActionID,SamplingDate,SamplingGoal,AnalysisBatchNumber,PointId,PointName,Description,SamplingUser,RecordUser,AuditUser,CreatUser,CreatDateTime,UpdateUser,UpdateDateTime,TaskCode ";
                string where = string.Empty;//查询条件拼接

                //if (ids.Length == 1 && !string.IsNullOrEmpty(ids[0]))
                //{
                //    idsStr = " AND id =" + idsStr;
                //}
                //else if (!string.IsNullOrEmpty(idsStr))
                //{
                //    idsStr = " AND id IN(" + idsStr + ")";
                //}
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND PointId =" + portIdsStr;
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = " AND PointId IN(" + portIdsStr + ")";
                }
                if (dtmStart != DateTime.MinValue)
                {
                    dateStr += string.Format(" AND  SamplingDate>='{0}'", dtmStart.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                if (dtmEnd != DateTime.MinValue)
                {
                    dateStr += string.Format(" AND  SamplingDate<='{0}'", dtmEnd.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                orderBy = string.IsNullOrEmpty(orderBy) ? "SamplingDate" : orderBy;
                where = string.Format(" 1=1 {0} {1} ", portIdsStr, dateStr);
                sqlStringBuilder.AppendFormat(@"SELECT {0} FROM {1} WHERE {2} ORDER BY {3}", fieldName, tableNameMain, where, orderBy);
                return g_DatabaseHelper.ExecuteDataView(sqlStringBuilder.ToString(), connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 取得详情表导出数据
        /// </summary>
        /// <param name="ids">主键数据</param>
        /// <param name="portIds">测点数据</param>
        /// <param name="sampleNumbers">样品编号</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">截止时间</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetDetailTableExportData(string[] ids, string[] portIds, string[] sampleNumbers,
            DateTime dtmStart, DateTime dtmEnd, string orderBy = "SamplingDate")
        {
            try
            {
                StringBuilder sqlStringBuilder = new StringBuilder();
                string idsStr = StringExtensions.GetArrayStrNoEmpty(ids.ToList<string>(), "','");
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                string sampleNumbersStr = StringExtensions.GetArrayStrNoEmpty(sampleNumbers.ToList<string>(), ",");
                string dateStr = string.Empty;
                string fieldName = "id,ActionID,SamplingDate,SamplingGoal,AnalysisBatchNumber,PointId,PointName,Description,SamplingUser,RecordUser,AuditUser,CreatUser,CreatDateTime,UpdateUser,UpdateDateTime";
                string where = string.Empty;//查询条件拼接

                if (ids.Length == 1 && !string.IsNullOrEmpty(ids[0]))
                {
                    idsStr = " AND id =" + idsStr + "'";
                }
                else if (!string.IsNullOrEmpty(idsStr))
                {
                    idsStr = " AND id IN('" + idsStr + "')";
                }
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND PointId =" + portIdsStr;
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = " AND PointId IN(" + portIdsStr + ")";
                }
                if (dtmStart != DateTime.MinValue)
                {
                    dateStr += string.Format(" AND  CreatDateTime>='{0}'", dtmStart.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                if (dtmEnd != DateTime.MinValue)
                {
                    dateStr += string.Format(" AND  CreatDateTime<='{0}'", dtmEnd.ToString("yyyy-MM-dd HH:mm:ss"));
                }

                orderBy = string.IsNullOrEmpty(orderBy) ? "SamplingDate" : orderBy;
                where = string.Format(" 1=1 {0} {1} {2} ", idsStr, portIdsStr, dateStr);

                return g_DatabaseHelper.ExecuteDataView(sqlStringBuilder.ToString(), connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //        /// <summary>
        //        /// 取得统计数据（最大值、最小值、平均值）
        //        /// </summary>
        //        /// <param name="autoMonitorType">数据类型</param>
        //        /// <param name="portIds">测点ID数组</param>
        //        /// <param name="factors">因子数组</param>
        //        /// <param name="dateStart">开始时间</param>
        //        /// <param name="dateEnd">结束时间</param>
        //        /// <returns></returns>
        //        public DataView GetStatisticalData(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd)
        //        {
        //            //拼接Where条件
        //            string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
        //            if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
        //            {
        //                portIdsStr = " AND PointId =" + portIdsStr;
        //            }
        //            else if (!string.IsNullOrEmpty(portIdsStr))
        //            {
        //                portIdsStr = " AND PointId IN(" + portIdsStr + ")";
        //            }
        //            string where = string.Format(" Tstamp>='{0}' AND Tstamp<='{1}' ", dateStart.ToString("yyyy-MM-dd HH:mm:ss"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss")) + portIdsStr;

        //            //拼接统计数据字串，使用UNION方式减少统计条数，提升统计速度
        //            string sql = string.Empty;
        //            for (int iRow = 0; iRow < factors.Length; iRow++)
        //            {
        //                if (iRow > 0)
        //                    sql += " UNION ";
        //                sql += string.Format(@"
        //                        SELECT PointId
        //                            ,PollutantCode='{1}'
        //	                        ,AVG(PollutantValue) AS Value_Avg
        //	                        ,MAX(PollutantValue) AS Value_Max
        //	                        ,MIN(PollutantValue) AS Value_Min
        //                        FROM {0}
        //                        WHERE {2}
        //                            AND PollutantCode='{1}'
        //                        GROUP BY PointId
        //                    ", tableNameMain, factors[iRow], where);
        //            }
        //            return g_DatabaseHelper.ExecuteDataView(sql, connection);
        //        }

        /// <summary>
        /// 取得数据总行数
        /// </summary>
        /// <param name="ids">主键数据</param>
        /// <param name="portIds">测点数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <returns></returns>
        public int GetAllDataCount(string[] ids, string[] portIds, DateTime dtmStart, DateTime dtmEnd)
        {
            string idsStr = StringExtensions.GetArrayStrNoEmpty(ids.ToList<string>(), "','");
            string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
            string dateStr = string.Empty;
            string where = string.Empty;//查询条件拼接

            if (ids.Length == 1 && !string.IsNullOrEmpty(ids[0]))
            {
                idsStr = " AND id ='" + idsStr + "'";
            }
            else if (!string.IsNullOrEmpty(idsStr))
            {
                idsStr = " AND id IN('" + idsStr + "')";
            }
            if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
            {
                portIdsStr = " AND PointId =" + portIdsStr;
            }
            else if (!string.IsNullOrEmpty(portIdsStr))
            {
                portIdsStr = " AND PointId IN(" + portIdsStr + ")";
            }
            if (dtmStart != DateTime.MinValue)
            {
                dateStr += string.Format(" AND  CreatDateTime>='{0}'", dtmStart.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            if (dtmEnd != DateTime.MinValue)
            {
                dateStr += string.Format(" AND  CreatDateTime<='{0}'", dtmEnd.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            where = string.Format(" 1=1 {0} {1} {2} ", idsStr, portIdsStr, dateStr);

            return g_GridViewPager.GetAllDataCount(tableNameMain, "SamplingDate", where, connection);
        }

        ///// <summary>
        ///// 获得数据列表
        ///// </summary>
        //public DataSet GetList(string strWhere)
        //{
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append("select id,ActionID,SamplingDate,SamplingGoal,AnalysisBatchNumber,PointId,PointName,Description,SamplingUser,RecordUser,AuditUser,CreatUser,CreatDateTime,UpdateUser,UpdateDateTime ");
        //    strSql.Append(" FROM TB_SamplingRecord ");
        //    if (strWhere.Trim() != "")
        //    {
        //        strSql.Append(" where " + strWhere);
        //    }
        //    return null;// g_DatabaseHelper.Query(strSql.ToString());
        //}

        ///// <summary>
        ///// 获得前几行数据
        ///// </summary>
        //public DataSet GetList(int Top, string strWhere, string filedOrder)
        //{
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append("select ");
        //    if (Top > 0)
        //    {
        //        strSql.Append(" top " + Top.ToString());
        //    }
        //    strSql.Append(" id,ActionID,SamplingDate,SamplingGoal,AnalysisBatchNumber,PointId,PointName,Description,SamplingUser,RecordUser,AuditUser,CreatUser,CreatDateTime,UpdateUser,UpdateDateTime ");
        //    strSql.Append(" FROM TB_SamplingRecord ");
        //    if (strWhere.Trim() != "")
        //    {
        //        strSql.Append(" where " + strWhere);
        //    }
        //    strSql.Append(" order by " + filedOrder);
        //    return null;// DbHelperSQL.Query(strSql.ToString());
        //}

        ///// <summary>
        ///// 获取记录总数
        ///// </summary>
        //public int GetRecordCount(string strWhere)
        //{
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append("select count(1) FROM TB_SamplingRecord ");
        //    if (strWhere.Trim() != "")
        //    {
        //        strSql.Append(" where " + strWhere);
        //    }
        //    object obj = 0;// DbHelperSQL.GetSingle(strSql.ToString());
        //    if (obj == null)
        //    {
        //        return 0;
        //    }
        //    else
        //    {
        //        return Convert.ToInt32(obj);
        //    }
        //}

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataTable GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T.id desc");
            }
            strSql.Append(")AS Row, T.*  from TB_SamplingRecord T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), connection);
        }
        #endregion  Method
    }
}

