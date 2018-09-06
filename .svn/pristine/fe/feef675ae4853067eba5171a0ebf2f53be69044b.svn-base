using SmartEP.Core.Generic;
using SmartEP.DomainModel.WaterQualityControlOperation;
using SmartEP.Utilities.AdoData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.QualityControlOperation.Water
{
    /// <summary>
    /// 名称：WaterInspectionBase2DAL.cs
    /// 创建人：窦曙健
    /// 创建日期：2015-09-23
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 水质自动巡检基础数据访问类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class WaterInspectionBase2DAL
    {
        /// <summary>
        /// 数据库处理类
        /// </summary>
        protected DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();
        public WaterInspectionBase2DAL() { }
        #region  Method

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns></returns>
        public int Add(WaterInspectionBase2Entity model)
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
            if (model.PointStatus != null)
            {
                strSql1.Append("PointStatus,");
                strSql2.Append("'" + model.PointStatus + "',");
            }
            if (model.Temperature != null)
            {
                strSql1.Append("Temperature,");
                strSql2.Append("'" + model.Temperature + "',");
            }
            if (model.RH != null)
            {
                strSql1.Append("RH,");
                strSql2.Append("'" + model.RH + "',");
            }
            if (model.DataCommunication != null)
            {
                strSql1.Append("DataCommunication,");
                strSql2.Append("'" + model.DataCommunication + "',");
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
            strSql.Append("insert into TB_WaterInspectionBase2(");
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
        /// 批量新增数据
        /// </summary>
        /// <param name="models">实体类数组</param>
        /// <returns></returns>
        public int AddBatch(params WaterInspectionBase2Entity[] models)
        {
            StringBuilder strSql = new StringBuilder();
            foreach (WaterInspectionBase2Entity model in models)
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
                if (model.PointStatus != null)
                {
                    strSql1.Append("PointStatus,");
                    strSql2.Append("'" + model.PointStatus + "',");
                }
                if (model.Temperature != null)
                {
                    strSql1.Append("Temperature,");
                    strSql2.Append("'" + model.Temperature + "',");
                }
                if (model.RH != null)
                {
                    strSql1.Append("RH,");
                    strSql2.Append("'" + model.RH + "',");
                }
                if (model.DataCommunication != null)
                {
                    strSql1.Append("DataCommunication,");
                    strSql2.Append("'" + model.DataCommunication + "',");
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
                strSql.Append(" insert into TB_WaterInspectionBase2(");
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
        /// 更新数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns></returns>
        public bool Update(WaterInspectionBase2Entity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update TB_WaterInspectionBase2 set ");
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
            if (model.PointStatus != null)
            {
                strSql.Append("PointStatus='" + model.PointStatus + "',");
            }
            else
            {
                strSql.Append("PointStatus= null ,");
            }
            if (model.Temperature != null)
            {
                strSql.Append("Temperature='" + model.Temperature + "',");
            }
            else
            {
                strSql.Append("Temperature= null ,");
            }
            if (model.RH != null)
            {
                strSql.Append("RH='" + model.RH + "',");
            }
            else
            {
                strSql.Append("RH= null ,");
            }
            if (model.DataCommunication != null)
            {
                strSql.Append("DataCommunication='" + model.DataCommunication + "',");
            }
            else
            {
                strSql.Append("DataCommunication= null ,");
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
        /// <param name="pointId">站点编号</param>
        /// <param name="pollingDate">巡检时间</param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from TB_WaterInspectionBase2 ");
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
            strSql.Append("select id,MissionID,ActionID,PointId,PointName,PointStatus,Temperature,RH,DataCommunication,PollingPeople,PollingDate,TaskCode ");
            strSql.Append(" FROM TB_WaterInspectionBase2 ");
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
