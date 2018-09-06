using SmartEP.Core.Generic;
using SmartEP.DomainModel.MonitoringBusiness;
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
    /// 名称：AbnormalDAL.cs
    /// 创建人：窦曙健
    /// 创建日期：2015-09-23
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 异常表数据访问类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class AbnormalDAL
    {
        #region  Method
        public AbnormalDAL()
        { }
        protected DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(AbnormalEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            StringBuilder strSql1 = new StringBuilder();
            StringBuilder strSql2 = new StringBuilder();
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
            if (model.DateTime != null)
            {
                strSql1.Append("DateTime,");
                strSql2.Append("'" + model.DateTime + "',");
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
            if (model.AbnormalItem != null)
            {
                strSql1.Append("AbnormalItem,");
                strSql2.Append("'" + model.AbnormalItem + "',");
            }
            if (model.AbnormalGuid != null)
            {
                strSql1.Append("AbnormalGuid,");
                strSql2.Append("'" + model.AbnormalGuid + "',");
            }
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
            if (model.AbnormalDescription != null)
            {
                strSql1.Append("AbnormalDescription,");
                strSql2.Append("'" + model.AbnormalDescription + "',");
            }
            if (model.DutyMan != null)
            {
                strSql1.Append("DutyMan,");
                strSql2.Append("'" + model.DutyMan + "',");
            }
            if (model.TaskCode != null)
            {
                strSql1.Append("TaskCode,");
                strSql2.Append("'" + model.TaskCode + "',");
            }
            strSql.Append("insert into TB_Abnormal(");
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
        /// 批量增加数据
        /// </summary>
        public int AddBatch(params AbnormalEntity[] models)
        {
            StringBuilder strSql = new StringBuilder();
            foreach (AbnormalEntity model in models)
            {
                StringBuilder strSql1 = new StringBuilder();
                StringBuilder strSql2 = new StringBuilder();
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
                if (model.DateTime != null)
                {
                    strSql1.Append("DateTime,");
                    strSql2.Append("'" + model.DateTime + "',");
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
                if (model.AbnormalItem != null)
                {
                    strSql1.Append("AbnormalItem,");
                    strSql2.Append("'" + model.AbnormalItem + "',");
                }
                if (model.AbnormalGuid != null)
                {
                    strSql1.Append("AbnormalGuid,");
                    strSql2.Append("'" + model.AbnormalGuid + "',");
                }
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
                if (model.AbnormalDescription != null)
                {
                    strSql1.Append("AbnormalDescription,");
                    strSql2.Append("'" + model.AbnormalDescription + "',");
                }
                if (model.DutyMan != null)
                {
                    strSql1.Append("DutyMan,");
                    strSql2.Append("'" + model.DutyMan + "',");
                }
                if (model.TaskCode != null)
                {
                    strSql1.Append("TaskCode,");
                    strSql2.Append("'" + model.TaskCode + "',");
                }
                strSql.Append(" insert into TB_Abnormal(");
                strSql.Append(strSql1.ToString().Remove(strSql1.Length - 1));
                strSql.Append(")");
                strSql.Append(" values (");
                strSql.Append(strSql2.ToString().Remove(strSql2.Length - 1));
                strSql.Append("); ");
            }
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
        public bool Update(AbnormalEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update TB_Abnormal set ");
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
            if (model.DateTime != null)
            {
                strSql.Append("DateTime='" + model.DateTime + "',");
            }
            else
            {
                strSql.Append("DateTime= null ,");
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
            if (model.AbnormalItem != null)
            {
                strSql.Append("AbnormalItem='" + model.AbnormalItem + "',");
            }
            else
            {
                strSql.Append("AbnormalItem= null ,");
            }
            if (model.AbnormalGuid != null)
            {
                strSql.Append("AbnormalGuid='" + model.AbnormalGuid + "',");
            }
            else
            {
                strSql.Append("AbnormalGuid= null ,");
            }
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
            if (model.AbnormalDescription != null)
            {
                strSql.Append("AbnormalDescription='" + model.AbnormalDescription + "',");
            }
            else
            {
                strSql.Append("AbnormalDescription= null ,");
            }
            if (model.DutyMan != null)
            {
                strSql.Append("DutyMan='" + model.DutyMan + "',");
            }
            else
            {
                strSql.Append("DutyMan= null ,");
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
        /// 更新一条数据
        /// </summary>
        /// <param name="model">仪器使用记录实体</param>
        /// <returns></returns>
        public int Update(DutyEntity model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update TB_Duty set ");
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
                if (model.DateTime != null)
                {
                    strSql.Append("DateTime='" + model.DateTime + "',");
                }
                if (model.ComSituation != null)
                {
                    strSql.Append("ComSituation='" + model.ComSituation + "',");
                }
                else
                {
                    strSql.Append("ComSituation= null ,");
                }
                if (model.DatAcquisitionStatus != null)
                {
                    strSql.Append("DatAcquisitionStatus='" + model.DatAcquisitionStatus + "',");
                }
                else
                {
                    strSql.Append("DatAcquisitionStatus= null ,");
                }
                if (model.InstrumentCase != null)
                {
                    strSql.Append("InstrumentCase='" + model.InstrumentCase + "',");
                }
                else
                {
                    strSql.Append("InstrumentCase= null ,");
                }
                if (model.SystemRunStatus != null)
                {
                    strSql.Append("SystemRunStatus='" + model.SystemRunStatus + "',");
                }
                else
                {
                    strSql.Append("SystemRunStatus= null ,");
                }
                if (model.Description != null)
                {
                    strSql.Append("Description='" + model.Description + "',");
                }
                else
                {
                    strSql.Append("Description= null ,");
                }
                if (model.DutyMan != null)
                {
                    strSql.Append("DutyMan='" + model.DutyMan + "',");
                }
                else
                {
                    strSql.Append("DutyMan= null ,");
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
                strSql.Append(" where DutyId='" + model.DutyId + "' ");
                strSql.Append(";select @@ROWCOUNT");
                //g_DatabaseHelper.ExecuteNonQuery(strSql.ToString(), connection);
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
        /// 删除一条数据
        /// </summary>
        public bool Delete(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from TB_Abnormal ");
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
            strSql.Append("select id,MissionID,ActionID,DateTime,PointId,PointName,AbnormalItem,AbnormalGuid,AbnormalName,AbnormalItemType,AbnormalDescription,DutyMan,TaskCode ");
            strSql.Append(" FROM TB_Abnormal ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), ConnectionString);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetListDuty(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,MissionID,ActionID,DateTime,PointId,PointName,AbnormalItem,AbnormalGuid,AbnormalName,AbnormalItemType,AbnormalDescription,DutyMan,TaskCode ");
            strSql.Append(" FROM TB_Abnormal ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), ConnectionString);
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
            strSql.Append(" id,MissionID,ActionID,DateTime,PointId,PointName,AbnormalItem,AbnormalGuid,AbnormalName,AbnormalItemType,AbnormalDescription,DutyMan,TaskCode ");
            strSql.Append(" FROM TB_Abnormal ");
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
                return "AMS_MonitoringBusinessConnection";
            }
        }
        #endregion  Method
    }
}
