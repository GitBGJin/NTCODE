using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace SmartEP.Utilities.AdoData
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
                    try
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
                    catch (Exception ex) { }
                    finally
                    {
                        myConn.Close();
                    }
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
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = strSql;
                    comm.ExecuteNonQuery();
                    trans.Commit();
                }
                catch
                {
                    trans.Rollback();
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
                    try
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
                    catch (Exception ex) { return null; }
                    finally
                    {
                        myConn.Close();
                    }
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
                    try
                    {
                        if (ConfigurationManager.AppSettings["SqlCmdTimeOut"] != null && ConfigurationManager.AppSettings["SqlCmdTimeOut"].ToString().Trim() != "")
                        {
                            myCommand.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["SqlCmdTimeOut"]);
                        }
                        // 组装Sql参数
                        PopulateParams(myCommand, TableParameters);
                        SqlParameter returnParam = myCommand.Parameters.Add(new SqlParameter("@thisId", SqlDbType.Int));
                        returnParam.Direction = ParameterDirection.Output;
                        int returnId = 0;
                        if (myConn.State == ConnectionState.Closed)
                            myConn.Open();
                        myCommand.ExecuteScalar();
                        returnId = (int)myCommand.Parameters["@thisId"].Value;
                        return returnId;
                    }
                    catch (Exception ex) { return 0; }
                    finally
                    {
                        myConn.Close();
                    }
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
                        try
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
                        catch (Exception ex) { return null; }
                        finally
                        {
                            myConn.Close();
                        }
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
                        try
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
                        catch (Exception ex) { return null; }
                        finally
                        {
                            myConn.Close();
                        }
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
                    try
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
                    catch (Exception ex) { return null; }
                    finally
                    {
                        myConn.Close();
                    }
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
                    try
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
                    catch (Exception ex) { return null; }
                    finally
                    {
                        myConn.Close();
                    }
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
                    try
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        PopulateProcParams(myCommand, null);
                        if (myConn.State == ConnectionState.Closed)
                            myConn.Open();
                        myCommand.ExecuteNonQuery();
                        return (int)myCommand.Parameters["ReturnValue"].Value;
                    }
                    catch (Exception ex) { return 0; }
                    finally
                    {
                        myConn.Close();
                    }
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
                    try
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
                    catch (Exception ex)
                    {
                        WriteTextLog("执行存储过程：" + procName + "，出现错误：" + ex.Message, strConName, DateTime.Now);
                        return 0;
                    }
                    finally
                    {
                        myConn.Close();
                    }
                }
            }
        }
        /// <summary>  
        /// 写入日志到文本文件  
        /// </summary>  
        /// <param name="action">动作</param>  
        /// <param name="strMessage">日志内容</param>  
        /// <param name="time">时间</param>  
        public static void WriteTextLog(string action, string strMessage, DateTime time)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"SystemRunProc\LogRunProc\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fileFullPath = path + time.ToString("yyyy-MM-dd") + ".SystemRunProc.txt";
            StringBuilder str = new StringBuilder();
            str.Append("Time:    " + time.ToString() + "\r\n");
            str.Append("Action:  " + action + "\r\n");
            str.Append("Message: " + strMessage + "\r\n");
            str.Append("-----------------------------------------------------------\r\n\r\n");
            StreamWriter sw;
            if (!File.Exists(fileFullPath))
            {
                sw = File.CreateText(fileFullPath);
            }
            else
            {
                sw = File.AppendText(fileFullPath);
            }
            sw.WriteLine(str.ToString());
            sw.Close();
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
                    try
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
                    catch (Exception ex) { return null; }
                    finally
                    {
                        myConn.Close();
                    }
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
                    try
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
                    catch (Exception ex) { return null; }
                    finally
                    {
                        myConn.Close();
                    }
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
                    try
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
                    catch (Exception ex) { return null; }
                    finally
                    {
                        myConn.Close();
                    }
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

        #region << 私有方法，执行多条SQL语句，实现数据库事务，创建人：朱佳伟，创建日期：2016-01-08 >>

        #region 执行多条SQL语句，实现数据库事务。
        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>
        public void ExecuteSqlTranWithIndentity(System.Collections.Generic.List<CommandInfo> SQLStringList, String strConName)
        {
            using (SqlConnection conn = new SqlConnection(GetSqlConnection(strConName)))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        try
                        {
                            int indentity = 0;
                            //循环
                            foreach (CommandInfo myDE in SQLStringList)
                            {
                                string cmdText = myDE.CommandText;
                                SqlParameter[] cmdParms = (SqlParameter[])myDE.Parameters;
                                foreach (SqlParameter q in cmdParms)
                                {
                                    if (q.Direction == ParameterDirection.InputOutput)
                                    {
                                        q.Value = indentity;
                                    }
                                }
                                PrepareCommand(cmd, conn, trans, CommandType.Text, cmdText, cmdParms);
                                int val = cmd.ExecuteNonQuery();
                                foreach (SqlParameter q in cmdParms)
                                {
                                    if (q.Direction == ParameterDirection.Output)
                                    {
                                        indentity = Convert.ToInt32(q.Value);
                                    }
                                }
                                cmd.Parameters.Clear();
                            }
                            trans.Commit();
                        }
                        catch
                        {
                            trans.Rollback();
                            throw;
                        }
                        finally { conn.Close(); }
                    }
                }
            }
        }
        #endregion

        #region 为执行命令准备参数
        /// <summary>
        /// 为执行命令准备参数
        /// </summary>
        /// <param name="cmd">SqlCommand 命令</param>
        /// <param name="conn">已经存在的数据库连接</param>
        /// <param name="trans">数据库事物处理</param>
        /// <param name="cmdType">SqlCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
        /// <param name="cmdText">Command text，T-SQL语句 例如 Select * from Products</param>
        /// <param name="cmdParms">返回带参数的命令</param>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            //判断是否需要事物处理
            if (trans != null)
            {
                cmd.Transaction = trans;
            }
            cmd.CommandType = cmdType;
            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                {
                    if ((parm.Direction == ParameterDirection.InputOutput || parm.Direction == ParameterDirection.Input) && (parm.Value == null))
                    {
                        parm.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parm);
                }
            }
        }
        #endregion

        #endregion
    }
}
