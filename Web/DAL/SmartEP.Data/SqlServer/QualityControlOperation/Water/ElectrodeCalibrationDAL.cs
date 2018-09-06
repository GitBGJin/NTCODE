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
    /// 名称：ElectrodeCalibrationDAL.cs
    /// 创建人：王秉晟
    /// 创建日期：2015-11-03
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 电极校准数据库处理
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class ElectrodeCalibrationDAL
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
        private string tableName = "dbo.TB_ElectrodeCalibration";
        #endregion

        #region << 构造函数 >>
        /// <summary>
        /// 构造函数
        /// </summary>
        public ElectrodeCalibrationDAL()
        { }
        #endregion

        #region << 方法 >>
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(ElectrodeCalibrationEntity model)
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
                if (model.MonitorItemCode != null)
                {
                    strSql1.Append("MonitorItemCode,");
                    strSql2.Append("'" + model.MonitorItemCode + "',");
                }
                if (model.MonitorItemText != null)
                {
                    strSql1.Append("MonitorItemText,");
                    strSql2.Append("'" + model.MonitorItemText + "',");
                }
                if (model.CalibrationTemperature != null)
                {
                    strSql1.Append("CalibrationTemperature,");
                    strSql2.Append("" + model.CalibrationTemperature + ",");
                }
                if (model.CalibrationConcentration != null)
                {
                    strSql1.Append("CalibrationConcentration,");
                    strSql2.Append("" + model.CalibrationConcentration + ",");
                }
                if (model.BeforeConcentration != null)
                {
                    strSql1.Append("BeforeConcentration,");
                    strSql2.Append("" + model.BeforeConcentration + ",");
                }
                if (model.BeforeParameter != null)
                {
                    strSql1.Append("BeforeParameter,");
                    strSql2.Append("" + model.BeforeParameter + ",");
                }
                if (model.AfterConcentration != null)
                {
                    strSql1.Append("AfterConcentration,");
                    strSql2.Append("" + model.AfterConcentration + ",");
                }
                if (model.AfterParameter != null)
                {
                    strSql1.Append("AfterParameter,");
                    strSql2.Append("" + model.AfterParameter + ",");
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
                if (model.OrderByNum != null)
                {
                    strSql1.Append("OrderByNum,");
                    strSql2.Append("" + model.OrderByNum + ",");
                }
                if (model.Description != null)
                {
                    strSql1.Append("Description,");
                    strSql2.Append("'" + model.Description + "',");
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
                strSql.Append("insert into TB_ElectrodeCalibration(");
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
        public int AddBatch(params ElectrodeCalibrationEntity[] models)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                foreach (ElectrodeCalibrationEntity model in models)
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
                    if (model.MonitorItemCode != null)
                    {
                        strSql1.Append("MonitorItemCode,");
                        strSql2.Append("'" + model.MonitorItemCode + "',");
                    }
                    if (model.MonitorItemText != null)
                    {
                        strSql1.Append("MonitorItemText,");
                        strSql2.Append("'" + model.MonitorItemText + "',");
                    }
                    if (model.CalibrationTemperature != null)
                    {
                        strSql1.Append("CalibrationTemperature,");
                        strSql2.Append("" + model.CalibrationTemperature + ",");
                    }
                    if (model.CalibrationConcentration != null)
                    {
                        strSql1.Append("CalibrationConcentration,");
                        strSql2.Append("" + model.CalibrationConcentration + ",");
                    }
                    if (model.BeforeConcentration != null)
                    {
                        strSql1.Append("BeforeConcentration,");
                        strSql2.Append("" + model.BeforeConcentration + ",");
                    }
                    if (model.BeforeParameter != null)
                    {
                        strSql1.Append("BeforeParameter,");
                        strSql2.Append("" + model.BeforeParameter + ",");
                    }
                    if (model.AfterConcentration != null)
                    {
                        strSql1.Append("AfterConcentration,");
                        strSql2.Append("" + model.AfterConcentration + ",");
                    }
                    if (model.AfterParameter != null)
                    {
                        strSql1.Append("AfterParameter,");
                        strSql2.Append("" + model.AfterParameter + ",");
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
                    if (model.OrderByNum != null)
                    {
                        strSql1.Append("OrderByNum,");
                        strSql2.Append("" + model.OrderByNum + ",");
                    }
                    if (model.Description != null)
                    {
                        strSql1.Append("Description,");
                        strSql2.Append("'" + model.Description + "',");
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
                    strSql.Append(" insert into TB_ElectrodeCalibration(");
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
        public int Update(ElectrodeCalibrationEntity model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update TB_ElectrodeCalibration set ");
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
                if (model.MonitorItemCode != null)
                {
                    strSql.Append("MonitorItemCode='" + model.MonitorItemCode + "',");
                }
                else
                {
                    strSql.Append("MonitorItemCode= null ,");
                }
                if (model.MonitorItemText != null)
                {
                    strSql.Append("MonitorItemText='" + model.MonitorItemText + "',");
                }
                else
                {
                    strSql.Append("MonitorItemText= null ,");
                }
                if (model.CalibrationTemperature != null)
                {
                    strSql.Append("CalibrationTemperature=" + model.CalibrationTemperature + ",");
                }
                else
                {
                    strSql.Append("CalibrationTemperature= null ,");
                }
                if (model.CalibrationConcentration != null)
                {
                    strSql.Append("CalibrationConcentration=" + model.CalibrationConcentration + ",");
                }
                else
                {
                    strSql.Append("CalibrationConcentration= null ,");
                }
                if (model.BeforeConcentration != null)
                {
                    strSql.Append("BeforeConcentration=" + model.BeforeConcentration + ",");
                }
                else
                {
                    strSql.Append("BeforeConcentration= null ,");
                }
                if (model.BeforeParameter != null)
                {
                    strSql.Append("BeforeParameter=" + model.BeforeParameter + ",");
                }
                else
                {
                    strSql.Append("BeforeParameter= null ,");
                }
                if (model.AfterConcentration != null)
                {
                    strSql.Append("AfterConcentration=" + model.AfterConcentration + ",");
                }
                else
                {
                    strSql.Append("AfterConcentration= null ,");
                }
                if (model.AfterParameter != null)
                {
                    strSql.Append("AfterParameter=" + model.AfterParameter + ",");
                }
                else
                {
                    strSql.Append("AfterParameter= null ,");
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
                if (model.OrderByNum != null)
                {
                    strSql.Append("OrderByNum=" + model.OrderByNum + ",");
                }
                else
                {
                    strSql.Append("OrderByNum= null ,");
                }
                if (model.Description != null)
                {
                    strSql.Append("Description='" + model.Description + "',");
                }
                else
                {
                    strSql.Append("Description= null ,");
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
                strSql.Append("delete from TB_ElectrodeCalibration ");
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
                strSql.Append("delete from TB_ElectrodeCalibration ");
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
        public ElectrodeCalibrationEntity GetModel(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1  ");
            strSql.Append(" id,MissionID,ActionID,PointId,PointName,MonitorItemCode,MonitorItemText,CalibrationTemperature,CalibrationConcentration,BeforeConcentration,BeforeParameter,AfterConcentration,AfterParameter,PollingPeople,PollingDate,OrderByNum,Description,CreatUser,CreatDateTime,UpdateUser,UpdateDateTime,TaskCode ");
            strSql.Append(" from TB_ElectrodeCalibration ");
            strSql.Append(" where id=" + id + "");
            ElectrodeCalibrationEntity model = new ElectrodeCalibrationEntity();
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
        public ElectrodeCalibrationEntity DataRowToModel(DataRow row)
        {
            ElectrodeCalibrationEntity model = new ElectrodeCalibrationEntity();
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
                if (row["MonitorItemCode"] != null)
                {
                    model.MonitorItemCode = row["MonitorItemCode"].ToString();
                }
                if (row["MonitorItemText"] != null)
                {
                    model.MonitorItemText = row["MonitorItemText"].ToString();
                }
                if (row["CalibrationTemperature"] != null && row["CalibrationTemperature"].ToString() != "")
                {
                    model.CalibrationTemperature = decimal.Parse(row["CalibrationTemperature"].ToString());
                }
                if (row["CalibrationConcentration"] != null && row["CalibrationConcentration"].ToString() != "")
                {
                    model.CalibrationConcentration = decimal.Parse(row["CalibrationConcentration"].ToString());
                }
                if (row["BeforeConcentration"] != null && row["BeforeConcentration"].ToString() != "")
                {
                    model.BeforeConcentration = decimal.Parse(row["BeforeConcentration"].ToString());
                }
                if (row["BeforeParameter"] != null && row["BeforeParameter"].ToString() != "")
                {
                    model.BeforeParameter = decimal.Parse(row["BeforeParameter"].ToString());
                }
                if (row["AfterConcentration"] != null && row["AfterConcentration"].ToString() != "")
                {
                    model.AfterConcentration = decimal.Parse(row["AfterConcentration"].ToString());
                }
                if (row["AfterParameter"] != null && row["AfterParameter"].ToString() != "")
                {
                    model.AfterParameter = decimal.Parse(row["AfterParameter"].ToString());
                }
                if (row["PollingPeople"] != null)
                {
                    model.PollingPeople = row["PollingPeople"].ToString();
                }
                if (row["PollingDate"] != null && row["PollingDate"].ToString() != "")
                {
                    model.PollingDate = DateTime.Parse(row["PollingDate"].ToString());
                }
                if (row["OrderByNum"] != null && row["OrderByNum"].ToString() != "")
                {
                    model.OrderByNum = int.Parse(row["OrderByNum"].ToString());
                }
                if (row["Description"] != null)
                {
                    model.Description = row["Description"].ToString();
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
        /// 获得数据列表
        /// </summary>
        public DataTable GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,MissionID,ActionID,PointId,PointName,MonitorItemCode,MonitorItemText,CalibrationTemperature,CalibrationConcentration,BeforeConcentration,BeforeParameter,AfterConcentration,AfterParameter,PollingPeople,PollingDate,OrderByNum,Description,CreatUser,CreatDateTime,UpdateUser,UpdateDateTime,TaskCode ");
            strSql.Append(" FROM TB_ElectrodeCalibration ");
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
            strSql.Append(" id,MissionID,ActionID,PointId,PointName,MonitorItemCode,MonitorItemText,CalibrationTemperature,CalibrationConcentration,BeforeConcentration,BeforeParameter,AfterConcentration,AfterParameter,PollingPeople,PollingDate,OrderByNum,Description,CreatUser,CreatDateTime,UpdateUser,UpdateDateTime,TaskCode ");
            strSql.Append(" FROM TB_ElectrodeCalibration ");
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
            strSql.Append("select count(1) FROM TB_ElectrodeCalibration ");
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
            strSql.Append(")AS Row, T.*  from TB_ElectrodeCalibration T ");
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

