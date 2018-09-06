using SmartEP.Core.Generic;
using SmartEP.DomainModel.WaterQualityControlOperation;
using SmartEP.Utilities.AdoData;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.QualityControlOperation.Water
{
    /// <summary>
    /// 名称：WaterInspectionBaseDAL.cs
    /// 创建人：窦曙健
    /// 创建日期：2015-10-13
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 任务记录表数据访问类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class WaterInspectionBaseDAL
    {
        /// <summary>
        /// 数据库处理类
        /// </summary>
        protected DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();
        public WaterInspectionBaseDAL() { }

        #region  Method
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns></returns>
        public int Add(WaterInspectionBaseEntity model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                StringBuilder strSql1 = new StringBuilder();
                StringBuilder strSql2 = new StringBuilder();
                if (model.MissionID != null)
                {
                    strSql1.Append("MissionID,");
                    strSql2.Append("'" + model.MissionID + "',");
                }
                if (model.MissionName != null)
                {
                    strSql1.Append("MissionName,");
                    strSql2.Append("'" + model.MissionName + "',");
                }
                if (model.PointId != null)
                {
                    strSql1.Append("PointId,");
                    strSql2.Append("'" + model.PointId + "',");
                }
                if (model.PointName != null)
                {
                    strSql1.Append("PointName,");
                    strSql2.Append("'" + model.PointName + "',");
                }
                if (model.StartDate != null)
                {
                    strSql1.Append("StartDate,");
                    strSql2.Append("'" + model.StartDate + "',");
                }
                if (model.EndDate != null)
                {
                    strSql1.Append("EndDate,");
                    strSql2.Append("'" + model.EndDate + "',");
                }
                if (model.Team != null)
                {
                    strSql1.Append("Team,");
                    strSql2.Append("'" + model.Team + "',");
                }
                if (model.ActionDate != null)
                {
                    strSql1.Append("ActionDate,");
                    strSql2.Append("'" + model.ActionDate + "',");
                }
                if (model.FinishDate != null)
                {
                    strSql1.Append("FinishDate,");
                    strSql2.Append("'" + model.FinishDate + "',");
                }
                if (model.Status != null)
                {
                    strSql1.Append("Status,");
                    strSql2.Append("'" + model.Status + "',");
                }
                if (model.TaskCode != null)
                {
                    strSql1.Append("TaskCode,");
                    strSql2.Append("'" + model.TaskCode + "',");
                }
                if (model.Type != null)
                {
                    strSql1.Append("Type,");
                    strSql2.Append("'" + model.Type + "',");
                }
                if (model.TaskType != null)
                {
                    strSql1.Append("TaskType,");
                    strSql2.Append("'" + model.TaskType + "',");
                }
                if (model.FileName != null)
                {
                    strSql1.Append("FileName,");
                    strSql2.Append("'" + model.FileName + "',");
                }
                if (model.FilePath != null)
                {
                    strSql1.Append("FilePath,");
                    strSql2.Append("'" + model.FilePath + "',");
                }
                if (model.FileUpLoadDate != null)
                {
                    strSql1.Append("FileUpLoadDate,");
                    strSql2.Append("'" + model.FileUpLoadDate + "',");
                }
                if (model.ActionUserName != null)
                {
                    strSql1.Append("ActionUserName,");
                    strSql2.Append("'" + model.ActionUserName + "',");
                }
                //if (model.FileStrems != null)
                //{
                //    strSql1.Append("FileStrems,");
                //    strSql2.Append("'" + model.FileStrems + "',");
                //}
                if (model.TempTaskID != null)
                {
                    strSql1.Append("TempTaskID,");
                    strSql2.Append("'" + model.TempTaskID + "',");
                }
                if (model.ReportCode != null)
                {
                    strSql1.Append("ReportCode,");
                    strSql2.Append("'" + model.ReportCode + "',");
                }
                if (model.ReportName != null)
                {
                    strSql1.Append("ReportName,");
                    strSql2.Append("'" + model.ReportName + "',");
                }
                if (model.FormCode != null)
                {
                    strSql1.Append("FormCode,");
                    strSql2.Append("'" + model.FormCode + "',");
                }
                if (model.MN != null)
                {
                    strSql1.Append("MN,");
                    strSql2.Append("'" + model.MN + "',");
                }
                if (model.ReportCreateDate != null)
                {
                    strSql1.Append("ReportCreateDate,");
                    strSql2.Append("'" + model.ReportCreateDate + "',");
                }
                if (model.ReportUpdateDate != null)
                {
                    strSql1.Append("ReportUpdateDate,");
                    strSql2.Append("'" + model.ReportUpdateDate + "',");
                }
                if (model.ReportUpLoadDate != null)
                {
                    strSql1.Append("ReportUpLoadDate,");
                    strSql2.Append("'" + model.ReportUpLoadDate + "',");
                }
                if (model.Remark != null)
                {
                    strSql1.Append("Remark,");
                    strSql2.Append("'" + model.Remark + "',");
                }
                if (model.OperateStatus != null)
                {
                    strSql1.Append("OperateStatus,");
                    strSql2.Append("'" + model.OperateStatus + "',");
                }
                if (model.TaskGuid != null)
                {
                    strSql1.Append("TaskGuid,");
                    strSql2.Append("'" + model.TaskGuid + "',");
                }
                strSql.Append("insert into TB_WaterInspectionBase(");
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
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 批量新增数据
        /// </summary>
        /// <param name="models">实体类数组</param>
        /// <returns></returns>
        public int AddBatch(params WaterInspectionBaseEntity[] models)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                foreach (WaterInspectionBaseEntity model in models)
                {
                    StringBuilder strSql1 = new StringBuilder();
                    StringBuilder strSql2 = new StringBuilder();
                    if (model.MissionID != null)
                    {
                        strSql1.Append("MissionID,");
                        strSql2.Append("'" + model.MissionID + "',");
                    }
                    if (model.MissionName != null)
                    {
                        strSql1.Append("MissionName,");
                        strSql2.Append("'" + model.MissionName + "',");
                    }
                    if (model.PointId != null)
                    {
                        strSql1.Append("PointId,");
                        strSql2.Append("'" + model.PointId + "',");
                    }
                    if (model.PointName != null)
                    {
                        strSql1.Append("PointName,");
                        strSql2.Append("'" + model.PointName + "',");
                    }
                    if (model.StartDate != null)
                    {
                        strSql1.Append("StartDate,");
                        strSql2.Append("'" + model.StartDate + "',");
                    }
                    if (model.EndDate != null)
                    {
                        strSql1.Append("EndDate,");
                        strSql2.Append("'" + model.EndDate + "',");
                    }
                    if (model.Team != null)
                    {
                        strSql1.Append("Team,");
                        strSql2.Append("'" + model.Team + "',");
                    }
                    if (model.ActionDate != null)
                    {
                        strSql1.Append("ActionDate,");
                        strSql2.Append("'" + model.ActionDate + "',");
                    }
                    if (model.FinishDate != null)
                    {
                        strSql1.Append("FinishDate,");
                        strSql2.Append("'" + model.FinishDate + "',");
                    }
                    if (model.Status != null)
                    {
                        strSql1.Append("Status,");
                        strSql2.Append("'" + model.Status + "',");
                    }
                    if (model.TaskCode != null)
                    {
                        strSql1.Append("TaskCode,");
                        strSql2.Append("'" + model.TaskCode + "',");
                    }
                    if (model.Type != null)
                    {
                        strSql1.Append("Type,");
                        strSql2.Append("'" + model.Type + "',");
                    }
                    if (model.TaskType != null)
                    {
                        strSql1.Append("TaskType,");
                        strSql2.Append("'" + model.TaskType + "',");
                    }
                    if (model.FileName != null)
                    {
                        strSql1.Append("FileName,");
                        strSql2.Append("'" + model.FileName + "',");
                    }
                    if (model.FilePath != null)
                    {
                        strSql1.Append("FilePath,");
                        strSql2.Append("'" + model.FilePath + "',");
                    }
                    if (model.FileUpLoadDate != null)
                    {
                        strSql1.Append("FileUpLoadDate,");
                        strSql2.Append("'" + model.FileUpLoadDate + "',");
                    }
                    if (model.ActionUserName != null)
                    {
                        strSql1.Append("ActionUserName,");
                        strSql2.Append("'" + model.ActionUserName + "',");
                    }
                    //if (model.FileStrems != null)
                    //{
                    //    strSql1.Append("FileStrems,");
                    //    strSql2.Append("'" + model.FileStrems + "',");
                    //}
                    if (model.TempTaskID != null)
                    {
                        strSql1.Append("TempTaskID,");
                        strSql2.Append("'" + model.TempTaskID + "',");
                    }
                    if (model.ReportCode != null)
                    {
                        strSql1.Append("ReportCode,");
                        strSql2.Append("'" + model.ReportCode + "',");
                    }
                    if (model.ReportName != null)
                    {
                        strSql1.Append("ReportName,");
                        strSql2.Append("'" + model.ReportName + "',");
                    }
                    if (model.FormCode != null)
                    {
                        strSql1.Append("FormCode,");
                        strSql2.Append("'" + model.FormCode + "',");
                    }
                    if (model.MN != null)
                    {
                        strSql1.Append("MN,");
                        strSql2.Append("'" + model.MN + "',");
                    }
                    if (model.ReportCreateDate != null)
                    {
                        strSql1.Append("ReportCreateDate,");
                        strSql2.Append("'" + model.ReportCreateDate + "',");
                    }
                    if (model.ReportUpdateDate != null)
                    {
                        strSql1.Append("ReportUpdateDate,");
                        strSql2.Append("'" + model.ReportUpdateDate + "',");
                    }
                    if (model.ReportUpLoadDate != null)
                    {
                        strSql1.Append("ReportUpLoadDate,");
                        strSql2.Append("'" + model.ReportUpLoadDate + "',");
                    }
                    if (model.Remark != null)
                    {
                        strSql1.Append("Remark,");
                        strSql2.Append("'" + model.Remark + "',");
                    }
                    if (model.OperateStatus != null)
                    {
                        strSql1.Append("OperateStatus,");
                        strSql2.Append("'" + model.OperateStatus + "',");
                    }
                    if (model.TaskGuid != null)
                    {
                        strSql1.Append("TaskGuid,");
                        strSql2.Append("'" + model.TaskGuid + "',");
                    }
                    strSql.Append(" insert into TB_WaterInspectionBase(");
                    strSql.Append(strSql1.ToString().Remove(strSql1.Length - 1));
                    strSql.Append(")");
                    strSql.Append(" values (");
                    strSql.Append(strSql2.ToString().Remove(strSql2.Length - 1));
                    strSql.Append("); ");
                }
                strSql.Append(";select @@ROWCOUNT");
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
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns></returns>
        public bool Update(WaterInspectionBaseEntity model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update TB_WaterInspectionBase set ");
                if (model.MissionID != null)
                {
                    strSql.Append("MissionID='" + model.MissionID + "',");
                }
                else
                {
                    strSql.Append("MissionID= null ,");
                }
                if (model.MissionName != null)
                {
                    strSql.Append("MissionName='" + model.MissionName + "',");
                }
                else
                {
                    strSql.Append("MissionName= null ,");
                }
                if (model.PointId != null)
                {
                    strSql.Append("PointId='" + model.PointId + "',");
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
                if (model.StartDate != null)
                {
                    strSql.Append("StartDate='" + model.StartDate + "',");
                }
                else
                {
                    strSql.Append("StartDate= null ,");
                }
                if (model.EndDate != null)
                {
                    strSql.Append("EndDate='" + model.EndDate + "',");
                }
                else
                {
                    strSql.Append("EndDate= null ,");
                }
                if (model.Team != null)
                {
                    strSql.Append("Team='" + model.Team + "',");
                }
                else
                {
                    strSql.Append("Team= null ,");
                }
                if (model.ActionDate != null)
                {
                    strSql.Append("ActionDate='" + model.ActionDate + "',");
                }
                else
                {
                    strSql.Append("ActionDate= null ,");
                }
                if (model.FinishDate != null)
                {
                    strSql.Append("FinishDate='" + model.FinishDate + "',");
                }
                else
                {
                    strSql.Append("FinishDate= null ,");
                }
                if (model.Status != null)
                {
                    strSql.Append("Status='" + model.Status + "',");
                }
                else
                {
                    strSql.Append("Status= null ,");
                }
                if (model.TaskCode != null)
                {
                    strSql.Append("TaskCode='" + model.TaskCode + "',");
                }
                else
                {
                    strSql.Append("TaskCode= null ,");
                }
                if (model.Type != null)
                {
                    strSql.Append("Type='" + model.Type + "',");
                }
                else
                {
                    strSql.Append("Type= null ,");
                }
                if (model.TaskType != null)
                {
                    strSql.Append("TaskType='" + model.TaskType + "',");
                }
                else
                {
                    strSql.Append("TaskType= null ,");
                }
                if (model.FileName != null)
                {
                    strSql.Append("FileName='" + model.FileName + "',");
                }
                else
                {
                    strSql.Append("FileName= null ,");
                }
                if (model.FilePath != null)
                {
                    strSql.Append("FilePath='" + model.FilePath + "',");
                }
                else
                {
                    strSql.Append("FilePath= null ,");
                }
                if (model.FileUpLoadDate != null)
                {
                    strSql.Append("FileUpLoadDate='" + model.FileUpLoadDate + "',");
                }
                else
                {
                    strSql.Append("FileUpLoadDate= null ,");
                }
                if (model.ActionUserName != null)
                {
                    strSql.Append("ActionUserName='" + model.ActionUserName + "',");
                }
                else
                {
                    strSql.Append("ActionUserName= null ,");
                }
                //if (model.FileStrems != null)
                //{
                //    strSql.Append("FileStrems='" + model.FileStrems + "',");
                //}
                //else
                //{
                //    strSql.Append("FileStrems= null ,");
                //}
                if (model.TempTaskID != null)
                {
                    strSql.Append("TempTaskID='" + model.TempTaskID + "',");
                }
                else
                {
                    strSql.Append("TempTaskID= null ,");
                }

                if (model.ReportCode != null)
                {
                    strSql.Append("ReportCode='" + model.ReportCode + "',");
                }
                else
                {
                    strSql.Append("ReportCode= null ,");
                }
                if (model.ReportName != null)
                {
                    strSql.Append("ReportName='" + model.ReportName + "',");
                }
                else
                {
                    strSql.Append("ReportName= null ,");
                }
                if (model.FormCode != null)
                {
                    strSql.Append("FormCode='" + model.FormCode + "',");
                }
                else
                {
                    strSql.Append("FormCode= null ,");
                }
                if (model.MN != null)
                {
                    strSql.Append("MN='" + model.MN + "',");
                }
                else
                {
                    strSql.Append("MN= null ,");
                }
                if (model.ReportCreateDate != null)
                {
                    strSql.Append("ReportCreateDate='" + model.ReportCreateDate + "',");
                }
                else
                {
                    strSql.Append("ReportCreateDate= null ,");
                }
                if (model.ReportUpdateDate != null)
                {
                    strSql.Append("ReportUpdateDate='" + model.ReportUpdateDate + "',");
                }
                else
                {
                    strSql.Append("ReportUpdateDate= null ,");
                }
                if (model.ReportUpLoadDate != null)
                {
                    strSql.Append("ReportUpLoadDate='" + model.ReportUpLoadDate + "',");
                }
                else
                {
                    strSql.Append("ReportUpLoadDate= null ,");
                }
                if (model.Remark != null)
                {
                    strSql.Append("Remark='" + model.Remark + "',");
                }
                else
                {
                    strSql.Append("Remark= null ,");
                }
                if (model.OperateStatus != null)
                {
                    strSql.Append("OperateStatus='" + model.OperateStatus + "',");
                }
                else
                {
                    strSql.Append("OperateStatus= null ,");
                }
                if (model.TaskGuid != null)
                {
                    strSql.Append("TaskGuid='" + model.TaskGuid + "',");
                }
                else
                {
                    strSql.Append("TaskGuid= null ,");
                }
                int n = strSql.ToString().LastIndexOf(",");
                strSql.Remove(n, 1);
                if (!string.IsNullOrWhiteSpace(model.TaskCode))
                {
                    strSql.Append(" where TaskCode='" + model.TaskCode + "'");
                }
                else
                {
                    strSql.Append(" where id='" + model.id + "'");
                }
                strSql.Append(";select @@ROWCOUNT");
                object obj = g_DatabaseHelper.ExecuteScalar(strSql.ToString(), ConnectionString);
                if (obj == null)
                {
                    return false;
                }
                else
                {
                    return (Convert.ToInt32(obj) > 0);
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 新增或更新数据（根据TaskCode判断）
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns></returns>
        public bool AddOrUpdateByTaskCode(WaterInspectionBaseEntity model)
        {
            if (string.IsNullOrWhiteSpace(model.TaskCode))
            {
                return false;
            }

            DataTable dt = GetList("TaskCode='" + model.TaskCode + "'");
            bool isSucceed = false;
            if (dt == null || dt.Rows.Count == 0)
            {
                isSucceed = Add(model) > 0;
            }
            else
            {
                isSucceed = Update(model);
            }
            return isSucceed;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="pointId">站点编号</param>
        /// <param name="pollingDate">巡检时间</param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from TB_WaterInspectionBase ");
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
        /// 显示数据
        /// </summary>
        /// <param name="pointId">站点编号</param>
        /// <param name="pollingDate">巡检时间</param>
        /// <returns></returns>
        public DataTable GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@" SELECT * ");
            strSql.Append(" FROM TB_WaterInspectionBase ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), ConnectionString);
        }

        /// <summary>
        /// 根据TID更新表单中对应的TaskCode值
        /// </summary>
        /// <param name="taskCode">任务编号</param>
        /// <param name="TID">下发任务（和TaskCode值一样），临时任务（生成的Guid）</param>
        /// <param name="formCode">表单编号</param>
        /// <returns></returns>
        public int UpdateTaskCodeOfFormByTID(string taskCode, string TID, string formCode)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                string[] formTableNames = GetTableNameByFormCode(formCode);
                int count = 0;

                if (formTableNames == null || formTableNames.Length == 0)
                {
                    return 0;
                }
                for (int i = 0; i < formTableNames.Length; i++)
                {
                    string formTableName = formTableNames[i];
                    string sqlChild = string.Empty;

                    if (string.IsNullOrWhiteSpace(formTableName))
                    {
                        continue;
                    }
                    sqlChild = string.Format(" Update {0} Set TaskCode='{1}' Where TaskCode='{2}'; ", formTableName, taskCode, TID);
                    sql.Append(sqlChild);
                }
                if (sql.Length > 0)
                {
                    sql.Append(";select @@ROWCOUNT");
                    object obj = g_DatabaseHelper.ExecuteScalar(sql.ToString(), ConnectionString);
                    if (obj != null)
                    {
                        count = Convert.ToInt32(obj);
                    }
                }

                return count;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// 根据TempTaskID更新表单中对应的TaskCode值
        /// </summary>
        /// <param name="taskCode">任务编号</param>
        /// <param name="formCode">表单编号</param>
        /// <returns></returns>
        public int UpdateTaskCodeOfFormByTempTaskID(string taskCode, string formCode)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                string[] formTableNames = GetTableNameByFormCode(formCode);
                int count = 0;

                if (formTableNames == null || formTableNames.Length == 0)
                {
                    return 0;
                }
                for (int i = 0; i < formTableNames.Length; i++)
                {
                    string formTableName = formTableNames[i];
                    string sqlChild = string.Empty;

                    if (string.IsNullOrWhiteSpace(formTableName))
                    {
                        continue;
                    }
                    sqlChild = string.Format(@" Update {0} Set TaskCode=(select top 1 TaskCode from TB_WaterInspectionBase
                                                                   where TempTaskID='{1}' order by id desc) 
                                                Where TaskCode='{1}' ", formTableName, taskCode);
                    sql.Append(sqlChild);
                }
                if (sql.Length > 0)
                {
                    sql.Append(";select @@ROWCOUNT");
                    object obj = g_DatabaseHelper.ExecuteScalar(sql.ToString(), ConnectionString);
                    if (obj != null)
                    {
                        count = Convert.ToInt32(obj);
                    }
                }

                return count;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        private string[] GetTableNameByFormCode(string formCode)
        {
            string[] formTableNames = null;
            switch (formCode)
            {
                case "01"://每周巡检维护
                    break;
                case "02"://月巡检
                    break;
                case "03"://季巡检
                    break;
                case "04"://年巡检
                    break;
                case "05"://空气子站巡检
                    break;
                case "06"://零点和跨度检查
                    break;
                case "07"://动态校准仪校准
                    break;
                case "08"://标准气体验证报告
                    break;
                case "09"://多点线性检查
                    break;
                case "10"://氮氧化物分析仪动态校准
                    break;
                case "11"://质控数据查询
                    break;
                case "12"://标气有效期查询
                    break;
                case "13"://水质自动站巡检
                    formTableNames = new string[] { "TB_WaterInspectionBase2", "TB_Status", "TB_PartChange", 
                                                    "TB_StandardSolutionChange", "TB_Standardization", "TB_Abnormal" };
                    break;
                case "14"://试剂标液更换表单
                    formTableNames = new string[] { "TB_StandardSolutionChange" };
                    break;
                case "15"://异常表单
                    formTableNames = new string[] { "TB_Abnormal" };
                    break;
                case "16"://新增备品备件更换表单
                    formTableNames = new string[] { "TB_PartChange" };
                    break;
                case "17"://新增标定表单
                    formTableNames = new string[] { "TB_Standardization" };
                    break;
                case "18"://新增采样记录
                    formTableNames = new string[] { "TB_SamplingRecord" };
                    break;
                case "19"://标准样品考核
                    formTableNames = new string[] { "" };
                    break;
                case "20"://实样比对考核
                    formTableNames = new string[] { "TB_StandardSolutionCheck", "TB_RealSamples" };
                    break;
                case "21"://准确度和精密度考核
                    formTableNames = new string[] { "TB_StandardSolutionCheck" };
                    break;
                case "22"://加标回收考核
                    formTableNames = new string[] { "TB_StandardSolutionCheck" };
                    break;
                case "23"://量程漂移考核
                    formTableNames = new string[] { "TB_StandardSolutionCheck" };
                    break;
                case "24"://零点漂移考核
                    formTableNames = new string[] { "TB_StandardSolutionCheck" };
                    break;
                case "25"://盲样考核
                    formTableNames = new string[] { "TB_StandardSolutionCheck" };
                    break;
                case "26"://标液考核
                    formTableNames = new string[] { "TB_StandardSolutionCheck" };
                    break;
                case "27"://检出限考核
                    formTableNames = new string[] { "TB_StandardSolutionCheck" };
                    break;
                case "28"://空白值考核
                    formTableNames = new string[] { "TB_StandardSolutionCheck" };
                    break;
                case "29"://标准曲线考核
                    formTableNames = new string[] { "TB_StandardSolutionCheck" };
                    break;
                case "30"://重复性误差考核
                    formTableNames = new string[] { "TB_StandardSolutionCheck" };
                    break;
                case "31"://性能考核
                    formTableNames = new string[] { "TB_WaterInspectionBase2", "TB_StandardSolutionCheck" };
                    break;
                case "32"://城区河道巡检
                    formTableNames = new string[] { "TB_UrbanRiverCourseInspect", "TB_Status", "TB_ElectrodeCalibration",
                                                    "TB_Standardization", "TB_Abnormal","TB_TaskFileInfo" };
                    break;
                default: break;
            }
            return formTableNames;
        }

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        private string ConnectionString
        {
            get
            {
                return "AMS_MonitoringBusinessConnection";
            }
        }

        #endregion  Method
    }
}
