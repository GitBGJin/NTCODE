using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Data.SqlServer.Common.GridView;
using SmartEP.DomainModel.WaterQualityControlOperation;
using SmartEP.Utilities.AdoData;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.QualityControlOperation.Water
{
    /// <summary>
    /// 名称：InstrumentUsedRecordDAL.cs
    /// 创建人：王秉晟
    /// 创建日期：2015-09-25
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 仪器使用记录数据库处理
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class InstrumentUsedRecordDAL
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
        private string tableName = "dbo.TB_InstrumentUsedRecord";
        #endregion

        #region << 构造函数 >>
        /// <summary>
        /// 构造函数
        /// </summary>
        public InstrumentUsedRecordDAL()
        {
            //connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(ApplicationType.Water, PollutantDataType.Hour);
        }
        #endregion

        #region << 方法 >>
        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model">仪器使用记录实体</param>
        /// <returns></returns>
        public int Add(InstrumentUsedRecordEntity model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                StringBuilder strSql1 = new StringBuilder();
                StringBuilder strSql2 = new StringBuilder();
                if (model.id != null)
                {
                    strSql1.Append("id,");
                    strSql2.Append("'" + Guid.NewGuid().ToString() + "',");
                }
                if (model.PointId != null)
                {
                    strSql1.Append("PointId,");
                    strSql2.Append("" + model.PointId + ",");
                }
                if (model.InstrumentNumber != null)
                {
                    strSql1.Append("InstrumentNumber,");
                    strSql2.Append("'" + model.InstrumentNumber + "',");
                }
                if (model.UsedUser != null)
                {
                    strSql1.Append("UsedUser,");
                    strSql2.Append("'" + model.UsedUser + "',");
                }
                if (model.UsedDate != null)
                {
                    strSql1.Append("UsedDate,");
                    strSql2.Append("'" + model.UsedDate + "',");
                }
                if (model.ShouldReturnDate != null)
                {
                    strSql1.Append("ShouldReturnDate,");
                    strSql2.Append("'" + model.ShouldReturnDate + "',");
                }
                if (model.RealReturnDate != null)
                {
                    strSql1.Append("RealReturnDate,");
                    strSql2.Append("'" + model.RealReturnDate + "',");
                }
                if (model.UseContent != null)
                {
                    strSql1.Append("UseContent,");
                    strSql2.Append("'" + model.UseContent + "',");
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
                strSql.Append("insert into TB_InstrumentUsedRecord(");
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
        /// 更新一条数据
        /// </summary>
        /// <param name="model">仪器使用记录实体</param>
        /// <returns></returns>
        public int Update(InstrumentUsedRecordEntity model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update TB_InstrumentUsedRecord set ");
                if (model.PointId != null)
                {
                    strSql.Append("PointId=" + model.PointId + ",");
                }
                else
                {
                    strSql.Append("PointId= null ,");
                }
                if (model.InstrumentNumber != null)
                {
                    strSql.Append("InstrumentNumber='" + model.InstrumentNumber + "',");
                }
                else
                {
                    strSql.Append("InstrumentNumber= null ,");
                }
                if (model.UsedUser != null)
                {
                    strSql.Append("UsedUser='" + model.UsedUser + "',");
                }
                else
                {
                    strSql.Append("UsedUser= null ,");
                }
                if (model.UsedDate != null)
                {
                    strSql.Append("UsedDate='" + model.UsedDate + "',");
                }
                if (model.ShouldReturnDate != null)
                {
                    strSql.Append("ShouldReturnDate='" + model.ShouldReturnDate + "',");
                }
                else
                {
                    strSql.Append("ShouldReturnDate= null ,");
                }
                if (model.RealReturnDate != null)
                {
                    strSql.Append("RealReturnDate='" + model.RealReturnDate + "',");
                }
                else
                {
                    strSql.Append("RealReturnDate= null ,");
                }
                if (model.UseContent != null)
                {
                    strSql.Append("UseContent='" + model.UseContent + "',");
                }
                else
                {
                    strSql.Append("UseContent= null ,");
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
                strSql.Append(" where id='" + model.id + "' ");
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
        /// <param name="id">id</param>
        /// <returns></returns>
        public int Delete(Guid id)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from TB_InstrumentUsedRecord ");
                strSql.Append(" where id='" + id + "' ");
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
        /// <param name="idlist">id数组</param>
        /// <returns></returns>
        public int DeleteList(string[] idlist)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                string sqlId = "'" + idlist.Aggregate((t, m) => t + "','" + m) + "'";
                strSql.Append("delete from TB_InstrumentUsedRecord ");
                strSql.Append(" where id in (" + sqlId + ")  ");
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
        /// <param name="id">id</param>
        /// <returns></returns>
        public InstrumentUsedRecordEntity GetModel(Guid id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1  ");
            strSql.Append(" id,PointId,InstrumentNumber,UsedUser,UsedDate,ShouldReturnDate,RealReturnDate,UseContent,CreatUser,CreatDateTime,UpdateUser,UpdateDateTime,TaskCode ");
            strSql.Append(" from TB_InstrumentUsedRecord ");
            strSql.Append(" where id='" + id + "' ");
            InstrumentUsedRecordEntity model = new InstrumentUsedRecordEntity();
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
        public InstrumentUsedRecordEntity DataRowToModel(DataRow row)
        {
            InstrumentUsedRecordEntity model = new InstrumentUsedRecordEntity();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = new Guid(row["id"].ToString());
                }
                if (row["PointId"] != null && row["PointId"].ToString() != "")
                {
                    model.PointId = int.Parse(row["PointId"].ToString());
                }
                if (row["InstrumentNumber"] != null)
                {
                    model.InstrumentNumber = row["InstrumentNumber"].ToString();
                }
                if (row["UsedUser"] != null)
                {
                    model.UsedUser = row["UsedUser"].ToString();
                }
                if (row["UsedDate"] != null && row["UsedDate"].ToString() != "")
                {
                    model.UsedDate = DateTime.Parse(row["UsedDate"].ToString());
                }
                if (row["ShouldReturnDate"] != null && row["ShouldReturnDate"].ToString() != "")
                {
                    model.ShouldReturnDate = DateTime.Parse(row["ShouldReturnDate"].ToString());
                }
                if (row["RealReturnDate"] != null && row["RealReturnDate"].ToString() != "")
                {
                    model.RealReturnDate = DateTime.Parse(row["RealReturnDate"].ToString());
                }
                if (row["UseContent"] != null)
                {
                    model.UseContent = row["UseContent"].ToString();
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
        /// 取得主表的虚拟分页查询数据和总行数
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetDataPager(string[] portIds,string[] Instruments,string[] Users, DateTime dtmStart, DateTime dtmEnd,
            int pageSize, int pageNo, out int recordTotal, string orderBy = "UsedDate")
        {
            recordTotal = 0;
            try
            {
                //string idsStr = StringExtensions.GetArrayStrNoEmpty(ids.ToList<string>(), ",");
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                string InstrumentStr = "";
                string UsersStr = "";
                for (int i = 0; i < Instruments.Length; i++)
                {
                    InstrumentStr += "'" + Instruments[i] + "'";
                    if (i != Instruments.Length - 1)
                    {
                        InstrumentStr += ",";
                    }

                }
                for (int i = 0; i < Users.Length; i++)
                {
                    UsersStr += "'" + Users[i] + "'";
                    if (i != Users.Length - 1)
                    {
                        UsersStr += ",";
                    }

                }
                string dateStr = string.Empty;
                string keyName = "id";
                string fieldName = "id,PointId,InstrumentNumber,UsedUser,UsedDate,ShouldReturnDate,RealReturnDate,UseContent,CreatUser,CreatDateTime,UpdateUser,UpdateDateTime,TaskCode ";
                string where = string.Empty;//查询条件拼接

                //if (ids.Length == 1 && !string.IsNullOrEmpty(ids[0]))
                //{
                //    idsStr = " AND id =" + idsStr;
                //}
                //else if (!string.IsNullOrEmpty(idsStr))
                //{
                //    idsStr = " AND id IN(" + idsStr + ")";
                //}
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND PointId =" + portIdsStr;
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = " AND PointId IN(" + portIdsStr + ")";
                }
                if (Instruments.Length == 1 && !string.IsNullOrEmpty(Instruments[0]))
                {
                    InstrumentStr = " AND InstrumentNumber ='" + InstrumentStr + "'";
                }
                else if (!string.IsNullOrEmpty(InstrumentStr))
                {
                    InstrumentStr = " AND InstrumentNumber IN(" + InstrumentStr + ")";
                }
                if (Users.Length == 1 && !string.IsNullOrEmpty(Users[0]))
                {
                    UsersStr = " AND UsedUser ='" + UsersStr + "'";
                }
                else if (!string.IsNullOrEmpty(UsersStr))
                {
                    UsersStr = " AND UsedUser IN(" + UsersStr + ")";
                }
                if (dtmStart != DateTime.MinValue)
                {
                    dateStr += string.Format(" AND  UsedDate>='{0}'", dtmStart.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                if (dtmEnd != DateTime.MinValue)
                {
                    dateStr += string.Format(" AND  UsedDate<='{0}'", dtmEnd.ToString("yyyy-MM-dd HH:mm:ss"));
                }

                orderBy = string.IsNullOrEmpty(orderBy) ? "UsedDate" : orderBy;
                where = string.Format(" 1=1 {0} {1} {2} {3}", portIdsStr,InstrumentStr,UsersStr, dateStr);
                return g_GridViewPager.GetGridViewPager(tableName, fieldName, keyName, pageSize, pageNo, orderBy, where, connection, out recordTotal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 取得主表导出数据
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">截止时间</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetExportData(string[] portIds, DateTime dtmStart, DateTime dtmEnd, string orderBy = "UsedDate")
        {
            try
            {
                StringBuilder sqlStringBuilder = new StringBuilder();
                //string idsStr = StringExtensions.GetArrayStrNoEmpty(ids.ToList<string>(), ",");
                string portIdsStr = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",");
                string dateStr = string.Empty;
                string keyName = "id";
                string fieldName = "id,PointId,InstrumentNumber,UsedUser,UsedDate,ShouldReturnDate,RealReturnDate,UseContent,CreatUser,CreatDateTime,UpdateUser,UpdateDateTime,TaskCode ";
                string where = string.Empty;//查询条件拼接

                //if (ids.Length == 1 && !string.IsNullOrEmpty(ids[0]))
                //{
                //    idsStr = " AND id =" + idsStr;
                //}
                //else if (!string.IsNullOrEmpty(idsStr))
                //{
                //    idsStr = " AND id IN(" + idsStr + ")";
                //}
                if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
                {
                    portIdsStr = " AND PointId =" + portIdsStr;
                }
                else if (!string.IsNullOrEmpty(portIdsStr))
                {
                    portIdsStr = " AND PointId IN(" + portIdsStr + ")";
                }
                if (dtmStart != DateTime.MinValue)
                {
                    dateStr += string.Format(" AND  UsedDate>='{0}'", dtmStart.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                if (dtmEnd != DateTime.MinValue)
                {
                    dateStr += string.Format(" AND  UsedDate<='{0}'", dtmEnd.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                orderBy = string.IsNullOrEmpty(orderBy) ? "UsedDate" : orderBy;
                where = string.Format(" 1=1 {0} {1} ", portIdsStr, dateStr);
                sqlStringBuilder.AppendFormat(@"SELECT {0} FROM {1} WHERE {2} ORDER BY {3}", fieldName, tableName, where, orderBy);
                return g_DatabaseHelper.ExecuteDataView(sqlStringBuilder.ToString(), connection);
            }
            catch (Exception ex)
            {
                throw ex;
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
            strSql.Append(")AS Row, T.*  from TB_InstrumentUsedRecord T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), connection);
        }
        #endregion  Method
    }
}
