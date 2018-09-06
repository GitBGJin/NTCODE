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
    /// 名称：UrbanRiverCourseInspectDAL.cs
    /// 创建人：王秉晟
    /// 创建日期：2015-11-03
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 城区河道巡检表数据库处理
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class UrbanRiverCourseInspectDAL
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
        /// 数据库表名
        /// </summary>
        private string tableName = "dbo.TB_UrbanRiverCourseInspect";
        #endregion

        #region << 构造函数 >>
        /// <summary>
        /// 构造函数
        /// </summary>
        public UrbanRiverCourseInspectDAL()
        { }
        #endregion

        #region  << 方法 >>
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(UrbanRiverCourseInspectEntity model)
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
                if (model.ActionID != null)
                {
                    strSql1.Append("ActionID,");
                    strSql2.Append("'" + model.ActionID + "',");
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
                if (model.PointStatus != null)
                {
                    strSql1.Append("PointStatus,");
                    strSql2.Append("'" + model.PointStatus + "',");
                }
                if (model.TaskGuid != null)
                {
                    strSql1.Append("TaskGuid,");
                    strSql2.Append("'" + Guid.NewGuid().ToString() + "',");
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
                if (model.Purpose != null)
                {
                    strSql1.Append("Purpose,");
                    strSql2.Append("'" + model.Purpose + "',");
                }
                if (model.Weather != null)
                {
                    strSql1.Append("Weather,");
                    strSql2.Append("'" + model.Weather + "',");
                }
                if (model.Temperature != null)
                {
                    strSql1.Append("Temperature,");
                    strSql2.Append("" + model.Temperature + ",");
                }
                if (model.RiverFlowVelocity != null)
                {
                    strSql1.Append("RiverFlowVelocity,");
                    strSql2.Append("'" + model.RiverFlowVelocity + "',");
                }
                if (model.WaterQualityStatus != null)
                {
                    strSql1.Append("WaterQualityStatus,");
                    strSql2.Append("'" + model.WaterQualityStatus + "',");
                }
                if (model.OtherConditions != null)
                {
                    strSql1.Append("OtherConditions,");
                    strSql2.Append("'" + model.OtherConditions + "',");
                }
                if (model.TaskCode != null)
                {
                    strSql1.Append("TaskCode,");
                    strSql2.Append("'" + model.TaskCode + "',");
                }
                strSql.Append("insert into TB_UrbanRiverCourseInspect(");
                strSql.Append(strSql1.ToString().Remove(strSql1.Length - 1));
                strSql.Append(")");
                strSql.Append(" values (");
                strSql.Append(strSql2.ToString().Remove(strSql2.Length - 1));
                strSql.Append(")");
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
        /// 批量增加数据
        /// </summary>
        public int AddBatch(params UrbanRiverCourseInspectEntity[] models)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                foreach (UrbanRiverCourseInspectEntity model in models)
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
                        strSql2.Append("" + model.PointId + ",");
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
                    if (model.TaskGuid != null)
                    {
                        strSql1.Append("TaskGuid,");
                        strSql2.Append("'" + Guid.NewGuid().ToString() + "',");
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
                    if (model.Purpose != null)
                    {
                        strSql1.Append("Purpose,");
                        strSql2.Append("'" + model.Purpose + "',");
                    }
                    if (model.Weather != null)
                    {
                        strSql1.Append("Weather,");
                        strSql2.Append("'" + model.Weather + "',");
                    }
                    if (model.Temperature != null)
                    {
                        strSql1.Append("Temperature,");
                        strSql2.Append("" + model.Temperature + ",");
                    }
                    if (model.RiverFlowVelocity != null)
                    {
                        strSql1.Append("RiverFlowVelocity,");
                        strSql2.Append("'" + model.RiverFlowVelocity + "',");
                    }
                    if (model.WaterQualityStatus != null)
                    {
                        strSql1.Append("WaterQualityStatus,");
                        strSql2.Append("'" + model.WaterQualityStatus + "',");
                    }
                    if (model.OtherConditions != null)
                    {
                        strSql1.Append("OtherConditions,");
                        strSql2.Append("'" + model.OtherConditions + "',");
                    }
                    if (model.TaskCode != null)
                    {
                        strSql1.Append("TaskCode,");
                        strSql2.Append("'" + model.TaskCode + "',");
                    }
                    strSql.Append(" insert into TB_UrbanRiverCourseInspect(");
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
        public int Update(UrbanRiverCourseInspectEntity model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update TB_UrbanRiverCourseInspect set ");
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
                if (model.PointStatus != null)
                {
                    strSql.Append("PointStatus='" + model.PointStatus + "',");
                }
                else
                {
                    strSql.Append("PointStatus= null ,");
                }
                if (model.TaskGuid != null)
                {
                    strSql.Append("TaskGuid='" + model.TaskGuid + "',");
                }
                else
                {
                    strSql.Append("TaskGuid= null ,");
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
                if (model.Purpose != null)
                {
                    strSql.Append("Purpose='" + model.Purpose + "',");
                }
                else
                {
                    strSql.Append("Purpose= null ,");
                }
                if (model.Weather != null)
                {
                    strSql.Append("Weather='" + model.Weather + "',");
                }
                else
                {
                    strSql.Append("Weather= null ,");
                }
                if (model.Temperature != null)
                {
                    strSql.Append("Temperature=" + model.Temperature + ",");
                }
                else
                {
                    strSql.Append("Temperature= null ,");
                }
                if (model.RiverFlowVelocity != null)
                {
                    strSql.Append("RiverFlowVelocity='" + model.RiverFlowVelocity + "',");
                }
                else
                {
                    strSql.Append("RiverFlowVelocity= null ,");
                }
                if (model.WaterQualityStatus != null)
                {
                    strSql.Append("WaterQualityStatus='" + model.WaterQualityStatus + "',");
                }
                else
                {
                    strSql.Append("WaterQualityStatus= null ,");
                }
                if (model.OtherConditions != null)
                {
                    strSql.Append("OtherConditions='" + model.OtherConditions + "',");
                }
                else
                {
                    strSql.Append("OtherConditions= null ,");
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
                strSql.Append(" where id=" + model.id + "; ");
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
        public int Delete(int id)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from TB_UrbanRiverCourseInspect ");
                strSql.Append(" where id=" + id + "; ");
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
        public int DeleteList(string idlist)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from TB_UrbanRiverCourseInspect ");
                strSql.Append(" where id in (" + idlist + ");  ");
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
        /// 得到一个对象实体
        /// </summary>
        public UrbanRiverCourseInspectEntity GetModel(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1  ");
            strSql.Append(" id,MissionID,ActionID,PointId,PointName,PointStatus,TaskGuid,PollingPeople,PollingDate,Purpose,Weather,Temperature,RiverFlowVelocity,WaterQualityStatus,OtherConditions,TaskCode ");
            strSql.Append(" from TB_UrbanRiverCourseInspect ");
            strSql.Append(" where id=" + id + "");
            UrbanRiverCourseInspectEntity model = new UrbanRiverCourseInspectEntity();
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
        public UrbanRiverCourseInspectEntity DataRowToModel(DataRow row)
        {
            UrbanRiverCourseInspectEntity model = new UrbanRiverCourseInspectEntity();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = int.Parse(row["id"].ToString());
                }
                if (row["MissionID"] != null)
                {
                    model.MissionID = row["MissionID"].ToString();
                }
                if (row["ActionID"] != null)
                {
                    model.ActionID = row["ActionID"].ToString();
                }
                if (row["PointId"] != null && row["PointId"].ToString() != "")
                {
                    model.PointId = int.Parse(row["PointId"].ToString());
                }
                if (row["PointName"] != null)
                {
                    model.PointName = row["PointName"].ToString();
                }
                if (row["PointStatus"] != null)
                {
                    model.PointStatus = row["PointStatus"].ToString();
                }
                if (row["TaskGuid"] != null && row["TaskGuid"].ToString() != "")
                {
                    model.TaskGuid = new Guid(row["TaskGuid"].ToString());
                }
                if (row["PollingPeople"] != null)
                {
                    model.PollingPeople = row["PollingPeople"].ToString();
                }
                if (row["PollingDate"] != null && row["PollingDate"].ToString() != "")
                {
                    model.PollingDate = DateTime.Parse(row["PollingDate"].ToString());
                }
                if (row["Purpose"] != null)
                {
                    model.Purpose = row["Purpose"].ToString();
                }
                if (row["Weather"] != null)
                {
                    model.Weather = row["Weather"].ToString();
                }
                if (row["Temperature"] != null && row["Temperature"].ToString() != "")
                {
                    model.Temperature = decimal.Parse(row["Temperature"].ToString());
                }
                if (row["RiverFlowVelocity"] != null)
                {
                    model.RiverFlowVelocity = row["RiverFlowVelocity"].ToString();
                }
                if (row["WaterQualityStatus"] != null)
                {
                    model.WaterQualityStatus = row["WaterQualityStatus"].ToString();
                }
                if (row["OtherConditions"] != null)
                {
                    model.OtherConditions = row["OtherConditions"].ToString();
                }
                if (row["TaskCode"] != null)
                {
                    model.TaskCode = row["TaskCode"].ToString();
                }
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,MissionID,ActionID,PointId,PointName,PointStatus,TaskGuid,PollingPeople,PollingDate,Purpose,Weather,Temperature,RiverFlowVelocity,WaterQualityStatus,OtherConditions,TaskCode ");
            strSql.Append(" FROM TB_UrbanRiverCourseInspect ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), connection);
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
            strSql.Append(" id,MissionID,ActionID,PointId,PointName,PointStatus,TaskGuid,PollingPeople,PollingDate,Purpose,Weather,Temperature,RiverFlowVelocity,WaterQualityStatus,OtherConditions,TaskCode ");
            strSql.Append(" FROM TB_UrbanRiverCourseInspect ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), connection);
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM TB_UrbanRiverCourseInspect ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
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
            strSql.Append(")AS Row, T.*  from TB_UrbanRiverCourseInspect T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), connection);
        }
        #endregion
    }
}

