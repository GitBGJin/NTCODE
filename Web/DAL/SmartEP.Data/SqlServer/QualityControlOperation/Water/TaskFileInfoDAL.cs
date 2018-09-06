using SmartEP.Core.Generic;
using SmartEP.Data.SqlServer.Common.GridView;
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
    /// 名称：TaskFileInfoDAL.cs
    /// 创建人：王秉晟
    /// 创建日期：2015-11-22
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 任务文件信息数据库处理
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class TaskFileInfoDAL
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
        private string tableName = "dbo.TB_TaskFileInfo";

        /// <summary>
        /// 工作ID
        /// </summary>
        private string m_ActionID = string.Empty;
        #endregion

        #region << 构造函数 >>
        /// <summary>
        /// 构造函数
        /// </summary>
        public TaskFileInfoDAL()
        {
            //connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(ApplicationType.Water, PollutantDataType.Hour);
        }
        #endregion

        #region  << 方法 >>
        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns></returns>
        public int Add(TaskFileInfoEntity model)
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
                if (model.FilePath != null)
                {
                    strSql1.Append("FilePath,");
                    strSql2.Append("'" + model.FilePath + "',");
                }
                if (model.FileName != null)
                {
                    strSql1.Append("FileName,");
                    strSql2.Append("'" + model.FileName + "',");
                }
                if (model.FileStreams != null)
                {
                    strSql1.Append("FileStreams,");
                    strSql2.Append("" + model.FileStreams + ",");
                }
                if (model.UpLoadUser != null)
                {
                    strSql1.Append("UpLoadUser,");
                    strSql2.Append("'" + model.UpLoadUser + "',");
                }
                if (model.UpLoadDate != null)
                {
                    strSql1.Append("UpLoadDate,");
                    strSql2.Append("'" + model.UpLoadDate + "',");
                }
                if (model.Remark != null)
                {
                    strSql1.Append("Remark,");
                    strSql2.Append("'" + model.Remark + "',");
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
                strSql.Append("insert into TB_TaskFileInfo(");
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
        /// <param name="models">实体类数组</param>
        public int AddBatch(params TaskFileInfoEntity[] models)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                foreach (TaskFileInfoEntity model in models)
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
                    if (model.FilePath != null)
                    {
                        strSql1.Append("FilePath,");
                        strSql2.Append("'" + model.FilePath + "',");
                    }
                    if (model.FileName != null)
                    {
                        strSql1.Append("FileName,");
                        strSql2.Append("'" + model.FileName + "',");
                    }
                    if (model.FileStreams != null)
                    {
                        strSql1.Append("FileStreams,");
                        strSql2.Append("" + model.FileStreams + ",");
                    }
                    if (model.UpLoadUser != null)
                    {
                        strSql1.Append("UpLoadUser,");
                        strSql2.Append("'" + model.UpLoadUser + "',");
                    }
                    if (model.UpLoadDate != null)
                    {
                        strSql1.Append("UpLoadDate,");
                        strSql2.Append("'" + model.UpLoadDate + "',");
                    }
                    if (model.Remark != null)
                    {
                        strSql1.Append("Remark,");
                        strSql2.Append("'" + model.Remark + "',");
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
                    strSql.Append(" insert into TB_TaskFileInfo(");
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
        /// <param name="model">实体类</param>
        /// <returns></returns>
        public int Update(TaskFileInfoEntity model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update TB_TaskFileInfo set ");
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
                if (model.FilePath != null)
                {
                    strSql.Append("FilePath='" + model.FilePath + "',");
                }
                else
                {
                    strSql.Append("FilePath= null ,");
                }
                if (model.FileName != null)
                {
                    strSql.Append("FileName='" + model.FileName + "',");
                }
                else
                {
                    strSql.Append("FileName= null ,");
                }
                if (model.FileStreams != null)
                {
                    strSql.Append("FileStreams=" + model.FileStreams + ",");
                }
                else
                {
                    strSql.Append("FileStreams= null ,");
                }
                if (model.UpLoadUser != null)
                {
                    strSql.Append("UpLoadUser='" + model.UpLoadUser + "',");
                }
                else
                {
                    strSql.Append("UpLoadUser= null ,");
                }
                if (model.UpLoadDate != null)
                {
                    strSql.Append("UpLoadDate='" + model.UpLoadDate + "',");
                }
                else
                {
                    strSql.Append("UpLoadDate= null ,");
                }
                if (model.Remark != null)
                {
                    strSql.Append("Remark='" + model.Remark + "',");
                }
                else
                {
                    strSql.Append("Remark= null ,");
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
                strSql.Append("delete from TB_TaskFileInfo ");
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
                strSql.Append("delete from TB_TaskFileInfo ");
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
        public TaskFileInfoEntity GetModel(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1  ");
            strSql.Append(" id,MissionID,ActionID,PointId,PointName,FilePath,FileName,FileStreams,UpLoadUser,UpLoadDate,Remark,OrderByNum,Description,CreatUser,CreatDateTime,UpdateUser,UpdateDateTime,TaskCode ");
            strSql.Append(" from TB_TaskFileInfo ");
            strSql.Append(" where id=" + id + "");
            TaskFileInfoEntity model = new TaskFileInfoEntity();
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
        public TaskFileInfoEntity DataRowToModel(DataRow row)
        {
            TaskFileInfoEntity model = new TaskFileInfoEntity();
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
                if (row["FilePath"] != null)
                {
                    model.FilePath = row["FilePath"].ToString();
                }
                if (row["FileName"] != null)
                {
                    model.FileName = row["FileName"].ToString();
                }
                if (row["FileStreams"] != null && row["FileStreams"].ToString() != "")
                {
                    model.FileStreams = (byte[])row["FileStreams"];
                }
                if (row["UpLoadUser"] != null)
                {
                    model.UpLoadUser = row["UpLoadUser"].ToString();
                }
                if (row["UpLoadDate"] != null && row["UpLoadDate"].ToString() != "")
                {
                    model.UpLoadDate = DateTime.Parse(row["UpLoadDate"].ToString());
                }
                if (row["Remark"] != null)
                {
                    model.Remark = row["Remark"].ToString();
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
                if (row["TaskCode"] != null && row["TaskCode"].ToString() != "")
                {
                    model.TaskCode = row["TaskCode"].ToString();
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
            strSql.Append("select id,MissionID,ActionID,PointId,PointName,FilePath,FileName,FileStreams,UpLoadUser,UpLoadDate,Remark,OrderByNum,Description,CreatUser,CreatDateTime,UpdateUser,UpdateDateTime,TaskCode ");
            strSql.Append(" FROM TB_TaskFileInfo ");
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
            strSql.Append(" id,MissionID,ActionID,PointId,PointName,FilePath,FileName,FileStreams,UpLoadUser,UpLoadDate,Remark,OrderByNum,Description,CreatUser,CreatDateTime,UpdateUser,UpdateDateTime,TaskCode ");
            strSql.Append(" FROM TB_TaskFileInfo ");
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
            strSql.Append("select count(1) FROM TB_TaskFileInfo ");
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
            strSql.Append(")AS Row, T.*  from TB_TaskFileInfo T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), connection);
        }

        /*
        */

        #endregion
    }
}
