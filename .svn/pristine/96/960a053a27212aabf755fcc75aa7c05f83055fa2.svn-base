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
    /// 名称：StandardSolutionChangeDAL.cs
    /// 创建人：窦曙健
    /// 创建日期：2015-09-23
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 试剂标液更换数据访问类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class StandardSolutionChangeDAL
    {
        #region  Method
        /// <summary>
        /// 数据库处理类
        /// </summary>
        protected DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();
        public StandardSolutionChangeDAL() { }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(StandardSolutionChangeEntity model)
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
                strSql2.Append("'" + Guid.NewGuid().ToString() + "',");
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
            if (model.InstrumentId != null)
            {
                strSql1.Append("InstrumentId,");
                strSql2.Append("'" + model.InstrumentId + "',");
            }
            if (model.InstrumentName != null)
            {
                strSql1.Append("InstrumentName,");
                strSql2.Append("'" + model.InstrumentName + "',");
            }
            if (model.StandardSolutionIdID != null)
            {
                strSql1.Append("StandardSolutionIdID,");
                strSql2.Append("'" + model.StandardSolutionIdID + "',");
            }
            if (model.StandardSolutionIdName != null)
            {
                strSql1.Append("StandardSolutionIdName,");
                strSql2.Append("'" + model.StandardSolutionIdName + "',");
            }
            if (model.Purpose != null)
            {
                strSql1.Append("Purpose,");
                strSql2.Append("'" + model.Purpose + "',");
            }
            if (model.ChangeReason != null)
            {
                strSql1.Append("ChangeReason,");
                strSql2.Append("'" + model.ChangeReason + "',");
            }
            if (model.OldNumber != null)
            {
                strSql1.Append("OldNumber,");
                strSql2.Append("'" + model.OldNumber + "',");
            }
            if (model.NewNumber != null)
            {
                strSql1.Append("NewNumber,");
                strSql2.Append("'" + model.NewNumber + "',");
            }
            if (model.ChangeDate != null)
            {
                strSql1.Append("ChangeDate,");
                strSql2.Append("'" + model.ChangeDate + "',");
            }
            if (model.LastChangeDate != null)
            {
                strSql1.Append("LastChangeDate,");
                strSql2.Append("'" + model.LastChangeDate + "',");
            }
            if (model.isTest != null)
            {
                strSql1.Append("isTest,");
                strSql2.Append("" + (model.isTest ? 1 : 0) + ",");
            }
            if (model.ChangePeople != null)
            {
                strSql1.Append("ChangePeople,");
                strSql2.Append("'" + model.ChangePeople + "',");
            }
            if (model.TaskCode != null)
            {
                strSql1.Append("TaskCode,");
                strSql2.Append("'" + model.TaskCode + "',");
            }
            strSql.Append("insert into TB_StandardSolutionChange(");
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
        public int AddBatch(params StandardSolutionChangeEntity[] models)
        {
            StringBuilder strSql = new StringBuilder();
            foreach (StandardSolutionChangeEntity model in models)
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
                    strSql2.Append("'" + Guid.NewGuid().ToString() + "',");
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
                if (model.InstrumentId != null)
                {
                    strSql1.Append("InstrumentId,");
                    strSql2.Append("'" + model.InstrumentId + "',");
                }
                if (model.InstrumentName != null)
                {
                    strSql1.Append("InstrumentName,");
                    strSql2.Append("'" + model.InstrumentName + "',");
                }
                if (model.StandardSolutionIdID != null)
                {
                    strSql1.Append("StandardSolutionIdID,");
                    strSql2.Append("'" + model.StandardSolutionIdID + "',");
                }
                if (model.StandardSolutionIdName != null)
                {
                    strSql1.Append("StandardSolutionIdName,");
                    strSql2.Append("'" + model.StandardSolutionIdName + "',");
                }
                if (model.Purpose != null)
                {
                    strSql1.Append("Purpose,");
                    strSql2.Append("'" + model.Purpose + "',");
                }
                if (model.ChangeReason != null)
                {
                    strSql1.Append("ChangeReason,");
                    strSql2.Append("'" + model.ChangeReason + "',");
                }
                if (model.OldNumber != null)
                {
                    strSql1.Append("OldNumber,");
                    strSql2.Append("'" + model.OldNumber + "',");
                }
                if (model.NewNumber != null)
                {
                    strSql1.Append("NewNumber,");
                    strSql2.Append("'" + model.NewNumber + "',");
                }
                if (model.ChangeDate != null)
                {
                    strSql1.Append("ChangeDate,");
                    strSql2.Append("'" + model.ChangeDate + "',");
                }
                if (model.LastChangeDate != null)
                {
                    strSql1.Append("LastChangeDate,");
                    strSql2.Append("'" + model.LastChangeDate + "',");
                }
                if (model.isTest != null)
                {
                    strSql1.Append("isTest,");
                    strSql2.Append("" + (model.isTest ? 1 : 0) + ",");
                }
                if (model.ChangePeople != null)
                {
                    strSql1.Append("ChangePeople,");
                    strSql2.Append("'" + model.ChangePeople + "',");
                }
                if (model.TaskCode != null)
                {
                    strSql1.Append("TaskCode,");
                    strSql2.Append("'" + model.TaskCode + "',");
                }
                strSql.Append(" insert into TB_StandardSolutionChange(");
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
        public bool Update(StandardSolutionChangeEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update TB_StandardSolutionChange set ");
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
            if (model.InstrumentId != null)
            {
                strSql.Append("InstrumentId='" + model.InstrumentId + "',");
            }
            else
            {
                strSql.Append("InstrumentId= null ,");
            }
            if (model.InstrumentName != null)
            {
                strSql.Append("InstrumentName='" + model.InstrumentName + "',");
            }
            else
            {
                strSql.Append("InstrumentName= null ,");
            }
            if (model.StandardSolutionIdID != null)
            {
                strSql.Append("StandardSolutionIdID='" + model.StandardSolutionIdID + "',");
            }
            else
            {
                strSql.Append("StandardSolutionIdID= null ,");
            }
            if (model.StandardSolutionIdName != null)
            {
                strSql.Append("StandardSolutionIdName='" + model.StandardSolutionIdName + "',");
            }
            else
            {
                strSql.Append("StandardSolutionIdName= null ,");
            }
            if (model.Purpose != null)
            {
                strSql.Append("Purpose='" + model.Purpose + "',");
            }
            else
            {
                strSql.Append("Purpose= null ,");
            }
            if (model.ChangeReason != null)
            {
                strSql.Append("ChangeReason='" + model.ChangeReason + "',");
            }
            else
            {
                strSql.Append("ChangeReason= null ,");
            }
            if (model.OldNumber != null)
            {
                strSql.Append("OldNumber='" + model.OldNumber + "',");
            }
            else
            {
                strSql.Append("OldNumber= null ,");
            }
            if (model.NewNumber != null)
            {
                strSql.Append("NewNumber='" + model.NewNumber + "',");
            }
            else
            {
                strSql.Append("NewNumber= null ,");
            }
            if (model.ChangeDate != null)
            {
                strSql.Append("ChangeDate='" + model.ChangeDate + "',");
            }
            else
            {
                strSql.Append("ChangeDate= null ,");
            }
            strSql.Append("LastChangeDate= ChangeDate ,");
            //if (model.LastChangeDate != null)
            //{
            //    strSql.Append("LastChangeDate='" + model.LastChangeDate + "',");
            //}
            //else
            //{
            //    strSql.Append("LastChangeDate= null ,");
            //}
            if (model.isTest != null)
            {
                strSql.Append("isTest=" + (model.isTest ? 1 : 0) + ",");
            }
            else
            {
                strSql.Append("isTest= null ,");
            }
            if (model.ChangePeople != null)
            {
                strSql.Append("ChangePeople='" + model.ChangePeople + "',");
            }
            else
            {
                strSql.Append("ChangePeople= null ,");
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
            strSql.Append("delete from TB_StandardSolutionChange ");
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
            strSql.Append("select id,MissionID,ActionID,PointId,PointName,InstrumentId,InstrumentName,StandardSolutionIdID,StandardSolutionIdName,Purpose,ChangeReason,OldNumber,NewNumber,ChangeDate,LastChangeDate,isTest,ChangePeople,TaskCode ");
            strSql.Append(" FROM TB_StandardSolutionChange ");
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
            strSql.Append(" id,MissionID,ActionID,PointId,PointName,InstrumentId,InstrumentName,StandardSolutionIdID,StandardSolutionIdName,Purpose,ChangeReason,OldNumber,NewNumber,ChangeDate,LastChangeDate,isTest,ChangePeople,TaskCode ");
            strSql.Append(" FROM TB_StandardSolutionChange ");
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
