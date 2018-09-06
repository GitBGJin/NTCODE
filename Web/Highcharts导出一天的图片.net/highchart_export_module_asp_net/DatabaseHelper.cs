using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace highchart_export_module_asp_net
{
    public class DatabaseHelper
    {
        #region << 执行SQL >>
        /// <summary>
        /// 执行sql脚本
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="strConName"></param>
        /// <param name="TableParameters"></param>
        public void ExecuteNonQuery(string strSql, String strConName, ArrayList TableParameters = null)
        {
            using (SqlConnection myConn = new SqlConnection(GetSqlConnection(strConName)))
            {
                using (SqlCommand myCommand = new SqlCommand(strSql, myConn))
                {
                    if (ConfigurationManager.AppSettings["SqlCmdTimeOut"] != null && ConfigurationManager.AppSettings["SqlCmdTimeOut"].ToString().Trim() != "")
                    {
                        myCommand.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["SqlCmdTimeOut"]);
                    }
                    // 组装Sql参数
                    PopulateParams(myCommand, TableParameters);

                    if (myConn.State == ConnectionState.Closed)
                        myConn.Open();
                    int rowCount = myCommand.ExecuteNonQuery();	// return sql 影响的行数
                }
            }
        }

        /// <summary>
        /// 执行Sql脚本，使用事务处理
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="strConName"></param>
        public void ExecuteWithTransaction(string strSql, String strConName)
        {
            if (strSql == null || strSql == "")
                throw new Exception("");

            string ConnectionString = GetSqlConnection(strConName);
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                //可以在事务中创建一个保存点，同时回滚到保存点
                SqlTransaction trans;
                trans = conn.BeginTransaction();
                SqlCommand comm = new SqlCommand();
                if (ConfigurationManager.AppSettings["SqlCmdTimeOut"] != null && ConfigurationManager.AppSettings["SqlCmdTimeOut"].ToString().Trim() != "")
                {
                    comm.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["SqlCmdTimeOut"]);
                }
                comm.Connection = conn;
                comm.Transaction = trans;
                try
                {
                    conn.Open();
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = strSql;
                    comm.ExecuteNonQuery();
                    trans.Commit();
                }
                catch
                {
                    //trans.Rollback();//不用指明trans.Rollback()，如果有异常的话，事物会自动回滚的。
                }
                finally
                {
                    trans.Dispose();
                    conn.Close();
                    comm.Dispose();
                }
            }
        }
        #endregion

        #region << 执行SQL，返回值 >>

        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。忽略额外的列或行。
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="strConName"></param>
        /// <param name="TableParameters"></param>
        /// <returns></returns>
        public object ExecuteScalar(string strSql, String strConName, ArrayList TableParameters = null)
        {
            object objScalar = new object();
            using (SqlConnection myConn = new SqlConnection(GetSqlConnection(strConName)))
            {
                using (SqlCommand myCommand = new SqlCommand(strSql, myConn))
                {
                    if (ConfigurationManager.AppSettings["SqlCmdTimeOut"] != null && ConfigurationManager.AppSettings["SqlCmdTimeOut"].ToString().Trim() != "")
                    {
                        myCommand.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["SqlCmdTimeOut"]);
                    }
                    // 组装Sql参数
                    PopulateParams(myCommand, TableParameters);

                    if (myConn.State == ConnectionState.Closed)
                        myConn.Open();
                    objScalar = myCommand.ExecuteScalar();
                    return objScalar;
                }
            }
        }


        /// <summary>
        /// 执行插入
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="strConName"></param>
        /// <param name="TableParameters"></param>
        /// <returns></returns>
        public int ExecuteInsert(string strSql, String strConName, ArrayList TableParameters = null)
        {
            using (SqlConnection myConn = new SqlConnection(GetSqlConnection(strConName)))
            {
                using (SqlCommand myCommand = new SqlCommand(strSql, myConn))
                {
                    if (ConfigurationManager.AppSettings["SqlCmdTimeOut"] != null && ConfigurationManager.AppSettings["SqlCmdTimeOut"].ToString().Trim() != "")
                    {
                        myCommand.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["SqlCmdTimeOut"]);
                    }
                    // 组装Sql参数
                    PopulateParams(myCommand, TableParameters);
                    SqlParameter returnParam = myCommand.Parameters.Add(new SqlParameter("@thisId", SqlDbType.Int));
                    returnParam.Direction = ParameterDirection.ReturnValue;
                    int returnId = 0;
                    if (myConn.State == ConnectionState.Closed)
                        myConn.Open();
                    myCommand.ExecuteScalar();
                    returnId = (int)myCommand.Parameters["@thisId"].Value;
                    return returnId;
                }
            }
        }

        /// <summary>
        /// 执行查询，返回查询结果DataView
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="strConName"></param>
        /// <param name="TableParameters"></param>
        /// <returns></returns>
        public DataView ExecuteDataView(string strSql, String strConName, ArrayList TableParameters = null)
        {
            using (SqlConnection myConn = new SqlConnection(GetSqlConnection(strConName)))
            {
                using (SqlCommand myCommand = new SqlCommand(strSql, myConn))
                {
                    using (SqlDataAdapter myAdapter = new SqlDataAdapter())
                    {
                        if (ConfigurationManager.AppSettings["SqlCmdTimeOut"] != null && ConfigurationManager.AppSettings["SqlCmdTimeOut"].ToString().Trim() != "")
                        {
                            myCommand.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["SqlCmdTimeOut"]);
                        }

                        myCommand.Connection = myConn;
                        if (myConn.State == ConnectionState.Closed)
                            myConn.Open();
                        myCommand.CommandType = CommandType.Text;
                        myCommand.CommandText = strSql;
                        myAdapter.SelectCommand = myCommand;
                        // 组装Sql参数
                        PopulateParams(myCommand, TableParameters);
                        DataSet ds = new DataSet();
                        myAdapter.Fill(ds, "BaseTable");
                        return ds.Tables["BaseTable"].DefaultView;
                    }
                }
            }
        }

        /// <summary>
        /// 执行查询，返回查询结果DataTable
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="strConName"></param>
        /// <param name="TableParameters"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string strSql, String strConName, ArrayList TableParameters = null)
        {
            using (SqlConnection myConn = new SqlConnection(GetSqlConnection(strConName)))
            {
                using (SqlCommand myCommand = new SqlCommand(strSql, myConn))
                {
                    using (SqlDataAdapter myAdapter = new SqlDataAdapter())
                    {
                        if (ConfigurationManager.AppSettings["SqlCmdTimeOut"] != null && ConfigurationManager.AppSettings["SqlCmdTimeOut"].ToString().Trim() != "")
                        {
                            myCommand.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["SqlCmdTimeOut"]);
                        }

                        myCommand.Connection = myConn;
                        if (myConn.State == ConnectionState.Closed)
                            myConn.Open();
                        myCommand.CommandType = CommandType.Text;
                        myCommand.CommandText = strSql;
                        myAdapter.SelectCommand = myCommand;
                        // 组装Sql参数
                        PopulateParams(myCommand, TableParameters);
                        DataSet ds = new DataSet();
                        myAdapter.Fill(ds, "BaseTable");
                        return ds.Tables["BaseTable"];
                    }
                }
            }
        }

        /// <summary>
        /// 执行查询，返回实体类
        /// </summary>
        /// <param name="model"></param>
        /// <param name="strSql"></param>
        /// <param name="strConName"></param>
        /// <param name="TableParameters"></param>
        public object ExecuteModel(object model, string strSql, String strConName, ArrayList TableParameters = null)
        {
            using (SqlConnection myConn = new SqlConnection(GetSqlConnection(strConName)))
            {
                using (SqlCommand myCommand = new SqlCommand(strSql, myConn))
                {
                    if (ConfigurationManager.AppSettings["SqlCmdTimeOut"] != null && ConfigurationManager.AppSettings["SqlCmdTimeOut"].ToString().Trim() != "")
                    {
                        myCommand.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["SqlCmdTimeOut"]);
                    }

                    if (myConn.State == ConnectionState.Closed)
                        myConn.Open();
                    // 组装Sql参数
                    PopulateParams(myCommand, TableParameters);
                    SqlDataReader myReader = myCommand.ExecuteReader();
                    if (myReader.Read())
                    {
                        ReaderToObject(myReader, model);
                    }
                    return model;
                }
            }
        }

        /// <summary>
        /// 执行查询，返回实体类
        /// </summary>
        /// <param name="t"></param>
        /// <param name="strSql"></param>
        /// <param name="strConName"></param>
        /// <param name="TableParameters"></param>
        /// <returns></returns>
        public object ExecuteModel(Type t, string strSql, String strConName, ArrayList TableParameters = null)
        {
            object model = Activator.CreateInstance(t);

            using (SqlConnection myConn = new SqlConnection(GetSqlConnection(strConName)))
            {
                using (SqlCommand myCommand = new SqlCommand(strSql, myConn))
                {
                    if (ConfigurationManager.AppSettings["SqlCmdTimeOut"] != null && ConfigurationManager.AppSettings["SqlCmdTimeOut"].ToString().Trim() != "")
                    {
                        myCommand.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["SqlCmdTimeOut"]);
                    }

                    if (myConn.State == ConnectionState.Closed)
                        myConn.Open();
                    // 组装Sql参数
                    PopulateParams(myCommand, TableParameters);
                    SqlDataReader myReader = myCommand.ExecuteReader();
                    if (myReader.Read())
                    {
                        ReaderToObject(myReader, model);
                    }
                    else
                    {
                        model = null;
                    }
                    return model;
                }
            }
        }
        /// <summary>
        /// 利用反射将数据读入实体类
        /// 前提是实体类中的Field必须和数据库中的一样
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="targetObj"></param>
        private static void ReaderToObject(IDataReader reader, object targetObj)
        {
            System.Reflection.FieldInfo[] fieldInfos = targetObj.GetType().GetFields();
            foreach (System.Reflection.FieldInfo info in fieldInfos)
            {
                if (reader[info.Name] != DBNull.Value)
                {
                    if (info.FieldType.IsEnum)
                    {
                        info.SetValue(targetObj, Enum.ToObject(info.FieldType, reader[info.Name]));
                    }
                    else
                    {
                        info.SetValue(targetObj, reader[info.Name]);
                    }
                }
            }
        }
        #endregion

        #region << 存储过程 >>
        /// <summary>
        /// Run stored procedure.
        /// </summary>
        /// <param name="procName">Name of stored procedure.</param>
        /// <returns>Stored procedure return value.</returns>
        public int RunProc(string procName, String strConName)
        {
            using (SqlConnection myConn = new SqlConnection(GetSqlConnection(strConName)))
            {
                using (SqlCommand myCommand = new SqlCommand(procName, myConn))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    PopulateProcParams(myCommand, null);
                    if (myConn.State == ConnectionState.Closed)
                        myConn.Open();
                    myCommand.ExecuteNonQuery();
                    return (int)myCommand.Parameters["ReturnValue"].Value;
                }
            }
        }

        /// <summary>
        /// Run stored procedure.
        /// </summary>
        /// <param name="procName">Name of stored procedure.</param>
        /// <param name="prams">Stored procedure params.</param>
        /// <returns>Stored procedure return value.</returns>
        public int RunProc(string procName, String strConName, SqlParameter[] prams = null)
        {
            using (SqlConnection myConn = new SqlConnection(GetSqlConnection(strConName)))
            {
                using (SqlCommand myCommand = new SqlCommand(procName, myConn))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    PopulateProcParams(myCommand, prams);
                    if (ConfigurationManager.AppSettings["SqlCmdTimeOut"] != null && ConfigurationManager.AppSettings["SqlCmdTimeOut"].ToString() != "")
                    {
                        myCommand.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["SqlCmdTimeOut"]);
                    }
                    if (myConn.State == ConnectionState.Closed)
                        myConn.Open();
                    myCommand.ExecuteNonQuery();
                    int ret = (int)myCommand.Parameters["ReturnValue"].Value;
                    myCommand.Parameters.Clear();
                    return ret;
                }
            }
        }

        /// <summary>
        /// Run stored procedure.
        /// </summary>
        /// <param name="procName">Name of stored procedure.</param>
        /// <param name="dataReader">Return result of procedure.</param>
        public SqlDataReader RunProcDataReader(string procName, String strConName = null)
        {
            using (SqlConnection myConn = new SqlConnection(GetSqlConnection(strConName)))
            {
                using (SqlCommand myCommand = new SqlCommand(procName, myConn))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    PopulateProcParams(myCommand, null);
                    if (ConfigurationManager.AppSettings["SqlCmdTimeOut"] != null && ConfigurationManager.AppSettings["SqlCmdTimeOut"].ToString() != "")
                    {
                        myCommand.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["SqlCmdTimeOut"]);
                    }
                    if (myConn.State == ConnectionState.Closed)
                        myConn.Open();
                    SqlDataReader dataReader = myCommand.ExecuteReader();
                    return dataReader;
                }
            }
        }

        /// <summary>
        /// Run stored procedure.
        /// </summary>
        /// <param name="procName">Name of stored procedure.</param>
        /// <param name="prams">Stored procedure params.</param>
        /// <param name="dataReader">Return result of procedure.</param>
        public SqlDataReader RunProcDataReader(string procName, String strConName, SqlParameter[] prams = null)
        {
            using (SqlConnection myConn = new SqlConnection(GetSqlConnection(strConName)))
            {
                using (SqlCommand myCommand = new SqlCommand(procName, myConn))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    PopulateProcParams(myCommand, prams);
                    if (ConfigurationManager.AppSettings["SqlCmdTimeOut"] != null && ConfigurationManager.AppSettings["SqlCmdTimeOut"].ToString() != "")
                    {
                        myCommand.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["SqlCmdTimeOut"]);
                    }
                    if (myConn.State == ConnectionState.Closed)
                        myConn.Open();
                    SqlDataReader dataReader = myCommand.ExecuteReader();
                    return dataReader;
                }
            }
        }

        /// <summary>
        /// 返回一个dataview，无参数
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="strConName"></param>
        /// <returns></returns>
        public DataView RunProcDataView(string procName, String strConName = null)
        {
            using (SqlConnection myConn = new SqlConnection(GetSqlConnection(strConName)))
            {
                using (SqlDataAdapter myCommand = new SqlDataAdapter(procName, myConn))
                {
                    myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;
                    if (ConfigurationManager.AppSettings["SqlCmdTimeOut"] != null && ConfigurationManager.AppSettings["SqlCmdTimeOut"].ToString() != "")
                    {
                        myCommand.SelectCommand.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["SqlCmdTimeOut"]);
                    }
                    DataSet ds = new DataSet();
                    myCommand.Fill(ds, "mytable");
                    DataView mydataview = ds.Tables["mytable"].DefaultView;
                    return mydataview;
                }
            }
        }

        /// <summary>
        /// 返回一个dataview，带参数
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="strConName"></param>
        /// <param name="prams"></param>
        /// <returns></returns>
        public DataView RunProcDataView(string procName, String strConName, SqlParameter[] prams = null)
        {
            using (SqlConnection myConn = new SqlConnection(GetSqlConnection(strConName)))
            {
                using (SqlDataAdapter myCommand = new SqlDataAdapter(procName, myConn))
                {
                    myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;
                    PopulateProcParams(myCommand, prams);
                    if (ConfigurationManager.AppSettings["SqlCmdTimeOut"] != null && ConfigurationManager.AppSettings["SqlCmdTimeOut"].ToString() != "")
                    {
                        myCommand.SelectCommand.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["SqlCmdTimeOut"]);
                    }
                    try
                    {
                        DataSet ds = new DataSet();
                        myCommand.Fill(ds, "mytable");
                        DataView mydataview = ds.Tables["mytable"].DefaultView;
                        return mydataview;
                    }
                    catch (Exception e)
                    {
                        throw new Exception("执行脚本命令的时候产生了一个错误-> " + e.ToString());
                    }
                    finally
                    {
                        myCommand.Dispose();
                        myConn.Close();
                        myConn.Dispose();
                    }
                }
            }
        }
        #endregion

        #region << 公共方法 >>
        /// <summary>
        /// 取得连接字符串
        /// </summary>
        /// <param name="appSettingsName"></param>
        /// <returns></returns>
        private string GetSqlConnection(string appSettingsName)
        {
            return ConfigurationManager.ConnectionStrings[appSettingsName].ConnectionString;
        }

        /// <summary>
        /// 组装Sql参数
        /// </summary>
        /// <param name="myCommand"></param>
        /// <param name="TableParameters"></param>
        private void PopulateParams(SqlCommand myCommand, ArrayList TableParameters)
        {
            myCommand.Parameters.Clear();
            if (TableParameters != null)
            {
                foreach (object obj in TableParameters)
                {
                    SqlParameter param = (SqlParameter)obj;
                    if (!myCommand.Parameters.Contains(param))
                    {
                        myCommand.Parameters.Add(param);
                    }
                }
            }
        }

        /// <summary>
        /// 创建存储过程Command
        /// </summary>
        /// <param name="myCommand"></param>
        /// <param name="prams"></param>
        /// <returns></returns>
        private SqlCommand PopulateProcParams(SqlCommand myCommand, SqlParameter[] prams)
        {
            myCommand.CommandType = CommandType.StoredProcedure;

            // add proc parameters
            if (prams != null)
            {
                foreach (SqlParameter parameter in prams)
                {
                    myCommand.Parameters.Add(parameter);
                }
            }

            // return param
            myCommand.Parameters.Add(
                new SqlParameter("ReturnValue", SqlDbType.Int, 4,
                ParameterDirection.ReturnValue, false, 0, 0,
                string.Empty, DataRowVersion.Default, null));

            return myCommand;
        }

        /// <summary>
        /// 创建存储过程DataAdapter
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="strConName"></param>
        /// <param name="prams"></param>
        /// <returns></returns>
        private SqlDataAdapter PopulateProcParams(SqlDataAdapter myCommand, SqlParameter[] prams)
        {
            //SqlConnection myConn = new SqlConnection(GetSqlConnection(strConName));
            //SqlDataAdapter cmd = new SqlDataAdapter(procName, myConn);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            // add proc parameters
            if (prams != null)
            {
                foreach (SqlParameter parameter in prams)
                {
                    myCommand.SelectCommand.Parameters.Add(parameter);
                }
            }

            // return param
            myCommand.SelectCommand.Parameters.Add(
                new SqlParameter("ReturnValue", SqlDbType.Int, 4,
                ParameterDirection.ReturnValue, false, 0, 0,
                string.Empty, DataRowVersion.Default, null));

            return myCommand;
        }

        /// <summary>
        /// 创建输入参数
        /// </summary>
        /// <param name="ParamName">Name of param.</param>
        /// <param name="DbType">Param type.</param>
        /// <param name="Size">Param size.</param>
        /// <param name="Value">Param value.</param>
        /// <returns>New parameter.</returns>
        public static SqlParameter MakeInParam(string ParamName, SqlDbType DbType, int Size, object Value)
        {
            return MakeParam(ParamName, DbType, Size, ParameterDirection.Input, Value);
        }

        /// <summary>
        /// 创建输出参数
        /// </summary>
        /// <param name="ParamName">Name of param.</param>
        /// <param name="DbType">Param type.</param>
        /// <param name="Size">Param size.</param>
        /// <returns>New parameter.</returns>
        public static SqlParameter MakeOutParam(string ParamName, SqlDbType DbType, int Size)
        {
            return MakeParam(ParamName, DbType, Size, ParameterDirection.Output, null);
        }

        /// <summary>
        /// Make stored procedure param.
        /// </summary>
        /// <param name="ParamName">Name of param.</param>
        /// <param name="DbType">Param type.</param>
        /// <param name="Size">Param size.</param>
        /// <param name="Direction">Parm direction.</param>
        /// <param name="Value">Param value.</param>
        /// <returns>New parameter.</returns>
        public static SqlParameter MakeParam(string ParamName, SqlDbType DbType, Int32 Size, ParameterDirection Direction, object Value)
        {
            SqlParameter param;

            if (Size > 0)
                param = new SqlParameter(ParamName, DbType, Size);
            else
                param = new SqlParameter(ParamName, DbType);

            param.Direction = Direction;
            if (!(Direction == ParameterDirection.Output && Value == null))
                param.Value = Value;

            return param;
        }
        #endregion
    }
}