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
    /// 名称：StandardizationDAL.cs
    /// 创建人：窦曙健
    /// 创建日期：2015-09-23
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 标定数据访问类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class StandardizationDAL
    {
        #region  Method
        /// <summary>
        /// 数据库处理类
        /// </summary>
        protected DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();
        public StandardizationDAL()
        { }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(StandardizationEntity model)
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
            if (model.FactorId != null)
            {
                strSql1.Append("FactorId,");
                strSql2.Append("'" + model.FactorId + "',");
            }
            if (model.FactorName != null)
            {
                strSql1.Append("FactorName,");
                strSql2.Append("'" + model.FactorName + "',");
            }
            if (model.StandardValue != null)
            {
                strSql1.Append("StandardValue,");
                strSql2.Append("'" + model.StandardValue + "',");
            }
            if (model.StandardizationValue != null)
            {
                strSql1.Append("StandardizationValue,");
                strSql2.Append("'" + model.StandardizationValue + "',");
            }
            if (model.ComparisonValue != null)
            {
                strSql1.Append("ComparisonValue,");
                strSql2.Append("" + model.ComparisonValue + ",");
            }
            if (model.StandardizationPeople != null)
            {
                strSql1.Append("StandardizationPeople,");
                strSql2.Append("'" + model.StandardizationPeople + "',");
            }
            if (model.StandardizationDate != null)
            {
                strSql1.Append("StandardizationDate,");
                strSql2.Append("'" + model.StandardizationDate + "',");
            }
            if (model.IsQualified != null)
            {
                strSql1.Append("IsQualified,");
                strSql2.Append("'" + model.IsQualified + "',");
            }
            if (model.TaskCode != null)
            {
                strSql1.Append("TaskCode,");
                strSql2.Append("'" + model.TaskCode + "',");
            }
            strSql.Append("insert into TB_Standardization(");
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
        public int AddBatch(params StandardizationEntity[] models)
        {
            StringBuilder strSql = new StringBuilder();
            foreach (StandardizationEntity model in models)
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
                if (model.FactorId != null)
                {
                    strSql1.Append("FactorId,");
                    strSql2.Append("'" + model.FactorId + "',");
                }
                if (model.FactorName != null)
                {
                    strSql1.Append("FactorName,");
                    strSql2.Append("'" + model.FactorName + "',");
                }
                if (model.StandardValue != null)
                {
                    strSql1.Append("StandardValue,");
                    strSql2.Append("'" + model.StandardValue + "',");
                }
                if (model.StandardizationValue != null)
                {
                    strSql1.Append("StandardizationValue,");
                    strSql2.Append("'" + model.StandardizationValue + "',");
                }
                if (model.ComparisonValue != null)
                {
                    strSql1.Append("ComparisonValue,");
                    strSql2.Append("" + model.ComparisonValue + ",");
                }
                if (model.StandardizationPeople != null)
                {
                    strSql1.Append("StandardizationPeople,");
                    strSql2.Append("'" + model.StandardizationPeople + "',");
                }
                if (model.StandardizationDate != null)
                {
                    strSql1.Append("StandardizationDate,");
                    strSql2.Append("'" + model.StandardizationDate + "',");
                }
                if (model.IsQualified != null)
                {
                    strSql1.Append("IsQualified,");
                    strSql2.Append("'" + model.IsQualified + "',");
                }
                if (model.TaskCode != null)
                {
                    strSql1.Append("TaskCode,");
                    strSql2.Append("'" + model.TaskCode + "',");
                }
                strSql.Append(" insert into TB_Standardization(");
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
        public bool Update(StandardizationEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update TB_Standardization set ");
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
            if (model.FactorId != null)
            {
                strSql.Append("FactorId='" + model.FactorId + "',");
            }
            else
            {
                strSql.Append("FactorId= null ,");
            }
            if (model.FactorName != null)
            {
                strSql.Append("FactorName='" + model.FactorName + "',");
            }
            else
            {
                strSql.Append("FactorName= null ,");
            }
            if (model.StandardValue != null)
            {
                strSql.Append("StandardValue='" + model.StandardValue + "',");
            }
            else
            {
                strSql.Append("StandardValue= null ,");
            }
            if (model.StandardizationValue != null)
            {
                strSql.Append("StandardizationValue='" + model.StandardizationValue + "',");
            }
            else
            {
                strSql.Append("StandardizationValue= null ,");
            }
            if (model.ComparisonValue != null)
            {
                strSql.Append("ComparisonValue=" + model.ComparisonValue + ",");
            }
            else
            {
                strSql.Append("ComparisonValue= null ,");
            }
            if (model.StandardizationPeople != null)
            {
                strSql.Append("StandardizationPeople='" + model.StandardizationPeople + "',");
            }
            else
            {
                strSql.Append("StandardizationPeople= null ,");
            }
            if (model.StandardizationDate != null)
            {
                strSql.Append("StandardizationDate='" + model.StandardizationDate + "',");
            }
            else
            {
                strSql.Append("StandardizationDate= null ,");
            }
            if (model.IsQualified != null)
            {
                strSql.Append("IsQualified='" + model.IsQualified + "',");
            }
            else
            {
                strSql.Append("IsQualified= null ,");
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
            strSql.Append(" where id=" + model.id + " ");
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
            strSql.Append("delete from TB_Standardization ");
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
            strSql.Append("select id,MissionID,ActionID,PointId,PointName,FactorId,FactorName,StandardValue,StandardizationValue,ComparisonValue,StandardizationPeople,StandardizationDate,IsQualified,TaskCode ");
            strSql.Append(" FROM TB_Standardization ");
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
            strSql.Append(" id,MissionID,ActionID,PointId,PointName,FactorId,FactorName,StandardizationValue,ComparisonValue,StandardizationPeople,StandardizationDate,IsQualified,TaskCode ");
            strSql.Append(" FROM TB_Standardization ");
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
