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
    /// 名称：ActionConfigDAL.cs
    /// 创建人：窦曙健
    /// 创建日期：2015-10-13
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 工作点配置数据访问类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class ActionConfigDAL
    {
    /// <summary>
        /// 数据库处理类
        /// </summary>
        protected DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();
        public ActionConfigDAL() { }
        #region  Method

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(ActionConfigEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            StringBuilder strSql1 = new StringBuilder();
            StringBuilder strSql2 = new StringBuilder();
            if (model.MissionId != null)
            {
                strSql1.Append("MissionId,");
                strSql2.Append("'" + Guid.NewGuid().ToString() + "',");
            }
            if (model.ActionID != null)
            {
                strSql1.Append("ActionID,");
                strSql2.Append("'" + Guid.NewGuid().ToString() + "',");
            }
            if (model.ItemID != null)
            {
                strSql1.Append("ItemID,");
                strSql2.Append("'" + Guid.NewGuid().ToString() + "',");
            }
            if (model.ItemName != null)
            {
                strSql1.Append("ItemName,");
                strSql2.Append("'" + model.ItemName + "',");
            }
            if (model.ItemType != null)
            {
                strSql1.Append("ItemType,");
                strSql2.Append("'" + model.ItemType + "',");
            }
            if (model.ClassType != null)
            {
                strSql1.Append("ClassType,");
                strSql2.Append("'" + model.ClassType + "',");
            }
            if (model.ActionUrl != null)
            {
                strSql1.Append("ActionUrl,");
                strSql2.Append("'" + model.ActionUrl + "',");
            }
            strSql.Append("insert into TB_ActionConfig(");
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
        public bool Update(ActionConfigEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update TB_ActionConfig set ");
            if (model.MissionId != null)
            {
                strSql.Append("MissionId='" + model.MissionId + "',");
            }
            else
            {
                strSql.Append("MissionId= null ,");
            }
            if (model.ActionID != null)
            {
                strSql.Append("ActionID='" + model.ActionID + "',");
            }
            else
            {
                strSql.Append("ActionID= null ,");
            }
            if (model.ItemID != null)
            {
                strSql.Append("ItemID='" + model.ItemID + "',");
            }
            else
            {
                strSql.Append("ItemID= null ,");
            }
            if (model.ItemName != null)
            {
                strSql.Append("ItemName='" + model.ItemName + "',");
            }
            else
            {
                strSql.Append("ItemName= null ,");
            }
            if (model.ItemType != null)
            {
                strSql.Append("ItemType='" + model.ItemType + "',");
            }
            else
            {
                strSql.Append("ItemType= null ,");
            }
            if (model.ClassType != null)
            {
                strSql.Append("ClassType='" + model.ClassType + "',");
            }
            else
            {
                strSql.Append("ClassType= null ,");
            }
            if (model.ActionUrl != null)
            {
                strSql.Append("ActionUrl='" + model.ActionUrl + "',");
            }
            else
            {
                strSql.Append("ActionUrl= null ,");
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
            strSql.Append("delete from TB_ActionConfig ");
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
        }		/// <summary>

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,MissionId,ActionID,ItemID,ItemName,ItemType,ClassType,ActionUrl ");
            strSql.Append(" FROM TB_ActionConfig ");
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