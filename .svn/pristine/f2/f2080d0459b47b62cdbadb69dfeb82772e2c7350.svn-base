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
    /// 名称：StatusDAL.cs
    /// 创建人：窦曙健
    /// 创建日期：2015-09-23
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 状态数据访问类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class StatusDAL
    {
        /// <summary>
        /// 数据库处理类
        /// </summary>
        protected DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();
        public StatusDAL() { }
        #region  Method

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(StatusEntity model)
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
            if (model.StatusType != null)
            {
                strSql1.Append("StatusType,");
                strSql2.Append("'" + model.StatusType + "',");
            }
            if (model.MonitorItem != null)
            {
                strSql1.Append("MonitorItem,");
                strSql2.Append("'" + model.MonitorItem + "',");
            }
            if (model.MonitorValue != null)
            {
                strSql1.Append("MonitorValue,");
                strSql2.Append("" + model.MonitorValue + ",");
            }
            if (model.ValueType != null)
            {
                strSql1.Append("ValueType,");
                strSql2.Append("'" + model.ValueType + "',");
            }
            if (model.PollingPeople != null)
            {
                strSql1.Append("PollingPeople,");
                strSql2.Append("'" + model.PollingPeople + "',");
            }
            if (model.PollingDate != null)
            {
                strSql1.Append("PollingDate,");
                strSql2.Append("'" + model.PollingDate + "',");
            }
            if (model.TaskCode != null)
            {
                strSql1.Append("TaskCode,");
                strSql2.Append("'" + model.TaskCode + "',");
            }
            strSql.Append("insert into TB_Status(");
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
        public int AddBatch(params StatusEntity[] models)
        {
            StringBuilder strSql = new StringBuilder();
            foreach (StatusEntity model in models)
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
                if (model.StatusType != null)
                {
                    strSql1.Append("StatusType,");
                    strSql2.Append("'" + model.StatusType + "',");
                }
                if (model.MonitorItem != null)
                {
                    strSql1.Append("MonitorItem,");
                    strSql2.Append("'" + model.MonitorItem + "',");
                }
                if (model.MonitorValue != null)
                {
                    strSql1.Append("MonitorValue,");
                    strSql2.Append("" + model.MonitorValue + ",");
                }
                if (model.ValueType != null)
                {
                    strSql1.Append("ValueType,");
                    strSql2.Append("'" + model.ValueType + "',");
                }
                if (model.PollingPeople != null)
                {
                    strSql1.Append("PollingPeople,");
                    strSql2.Append("'" + model.PollingPeople + "',");
                }
                if (model.PollingDate != null)
                {
                    strSql1.Append("PollingDate,");
                    strSql2.Append("'" + model.PollingDate + "',");
                }
                if (model.TaskCode != null)
                {
                    strSql1.Append("TaskCode,");
                    strSql2.Append("'" + model.TaskCode + "',");
                }
                strSql.Append(" insert into TB_Status(");
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
        public bool Update(StatusEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update TB_Status set ");
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
            if (model.StatusType != null)
            {
                strSql.Append("StatusType='" + model.StatusType + "',");
            }
            else
            {
                strSql.Append("StatusType= null ,");
            }
            if (model.MonitorItem != null)
            {
                strSql.Append("MonitorItem='" + model.MonitorItem + "',");
            }
            else
            {
                strSql.Append("MonitorItem= null ,");
            }
            if (model.MonitorValue != null)
            {
                strSql.Append("MonitorValue=" + model.MonitorValue + ",");
            }
            else
            {
                strSql.Append("MonitorValue= null ,");
            }
            if (model.ValueType != null)
            {
                strSql.Append("ValueType='" + model.ValueType + "',");
            }
            else
            {
                strSql.Append("ValueType= null ,");
            }
            if (model.PollingPeople != null)
            {
                strSql.Append("PollingPeople='" + model.PollingPeople + "',");
            }
            else
            {
                strSql.Append("PollingPeople= null ,");
            }
            if (model.PollingDate != null)
            {
                strSql.Append("PollingDate='" + model.PollingDate + "',");
            }
            else
            {
                strSql.Append("PollingDate= null ,");
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
        /// 删除一条数据
        /// </summary>
        public bool Delete(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from TB_Status ");
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
            strSql.Append("select id,MissionID,ActionID,PointId,PointName,StatusType,MonitorItem,MonitorValue,ValueType,PollingPeople,PollingDate,TaskCode ");
            strSql.Append(" FROM TB_Status ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
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
