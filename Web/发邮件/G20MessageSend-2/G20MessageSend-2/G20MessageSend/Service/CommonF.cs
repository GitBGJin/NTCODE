using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace Service
{
    public class CommonF
    {
        public DataView CreatDataView(String strSql, String strConnectString)
        {
            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings[strConnectString].ConnectionString);
            SqlDataAdapter myCommand = new SqlDataAdapter(strSql, myConn);
            if (ConfigurationManager.AppSettings["AuditCmdTimeOut"] != null && ConfigurationManager.AppSettings["AuditCmdTimeOut"].ToString().Trim() != "")
            {
                myCommand.SelectCommand.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["AuditCmdTimeOut"]);
            }

            DataTable dt = new DataTable();
            try
            {
                myCommand.Fill(dt);
            }
            catch (Exception e)
            {
                WriteErrorLog("Execute (" + strSql + ") Error: " + e.Message);
            }
            finally
            {
                myCommand.Dispose();
                myConn.Close();
                myConn.Dispose();
            }
            return dt.DefaultView;
        }

        //<summary>
        //合并多个结构相同的表的方法
        //<param name="DataSet"></param>
        //</summary>
        //<returns>DataTable</returns>
        public DataTable MergeDataTable(DataSet ds)
        {
            DataTable newDataTable = ds.Tables[0].Clone();                //创建新表 克隆以有表的架构。
            object[] objArray = new object[newDataTable.Columns.Count];   //定义与表列数相同的对象数组 存放表的一行的值。
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                for (int j = 0; j < ds.Tables[i].Rows.Count; j++)
                {
                    ds.Tables[i].Rows[j].ItemArray.CopyTo(objArray, 0);    //将表的一行的值存放数组中。
                    newDataTable.Rows.Add(objArray);                       //将数组的值添加到新表中。
                }
            }
            return newDataTable;                                           //返回新表。
        }

        /// <summary>
        /// 获取DataTable
        /// </summary>
        /// <param name="strSql">SQL 语句</param>
        /// <param name="strConnectString">连接字符串名称</param>
        /// <returns></returns>
        public DataTable CreatDataTable(String strSql, String strConnectString)
        {

            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings[strConnectString].ConnectionString);
            SqlDataAdapter myCommand = new SqlDataAdapter(strSql, myConn);
            if (ConfigurationManager.AppSettings["AuditCmdTimeOut"] != null && ConfigurationManager.AppSettings["AuditCmdTimeOut"].ToString().Trim() != "")
            {
                myCommand.SelectCommand.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["AuditCmdTimeOut"]);
            }
            DataTable dt = new DataTable();
            try
            {
                myCommand.Fill(dt);
            }
            catch (Exception e)
            {
                WriteErrorLog("Execute (" + strSql + ") Error: " + e.Message);
            }
            finally
            {
                myCommand.Dispose();
                myConn.Close();
                myConn.Dispose();
            }
            return dt;
        }

        /// <summary>
        /// 获取DataSet
        /// </summary>
        /// <param name="strSql">SQL 语句</param>
        /// <param name="strConnectString">连接字符串名称</param>
        /// <returns></returns>
        public DataSet CreatDataSet(String strSql, String strConnectString)
        {
            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings[strConnectString].ConnectionString);
            SqlDataAdapter myCommand = new SqlDataAdapter(strSql, myConn);
            if (ConfigurationManager.AppSettings["AuditCmdTimeOut"] != null && ConfigurationManager.AppSettings["AuditCmdTimeOut"].ToString().Trim() != "")
            {
                myCommand.SelectCommand.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["AuditCmdTimeOut"]);
            }
            DataSet ds = new DataSet();
            try
            {
                myCommand.Fill(ds);
            }
            catch (Exception e)
            {
                WriteErrorLog("Execute (" + strSql + ") Error: " + e.Message);
            }
            finally
            {
                myCommand.Dispose();
                myConn.Close();
                myConn.Dispose();
            }
            return ds;
        }

        public void ExcuteSQL(string strSql, string strConnectString)
        {
            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings[strConnectString].ConnectionString);
            SqlCommand myCommand = new SqlCommand(strSql, myConn);

            try
            {
                if (myConn.State == ConnectionState.Closed) myConn.Open();
                myCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                WriteErrorLog("Execute (" + strSql + ") Error: " + e.Message);
            }
            finally
            {
                myCommand.Dispose();
                myConn.Close();
                myConn.Dispose();
            }
        }
        public object ExecuteScalar(string strSql, string strConnectString)
        {
            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings[strConnectString].ConnectionString);
            SqlCommand myCommand = new SqlCommand(strSql, myConn);

            try
            {
                if (myConn.State == ConnectionState.Closed) myConn.Open();
                return myCommand.ExecuteScalar();
            }
            catch (Exception e)
            {
                WriteErrorLog("Execute (" + strSql + ") Error: " + e.Message);
                return null;
            }
            finally
            {
                myCommand.Dispose();
                myConn.Close();
                myConn.Dispose();
            }
        }

        public String SafeJsCode(String message)
        {
            message = message.Trim('\\').Trim('r').Trim('\\').Trim('n');
            message = message.Replace("\r\n", "\\r\\n");
            message = message.Replace("'", "\\'");
            return message;
        }

        public string GetConnectString(string sqlType, string strServerName, string strUser, string strPsw, string strDataBase)
        {
            string strConnectionString = "";
            switch (sqlType)
            {
                case "SQL Server":
                    strConnectionString = "server=" + strServerName + ";uid=" + strUser + ";pwd=" + strPsw + ";database=" + strDataBase;
                    break;
                case "Access":
                    strConnectionString = "Provider=Microsoft.JET.OLEDB.4.0;data source=" + (strDataBase == "" ? "@FilePath" : strDataBase);
                    break;
                case "Excel":
                    strConnectionString = "Provider=Microsoft.JET.OLEDB.4.0;Extended Properties=Excel 8.0;data source=" + (strDataBase == "" ? "@FilePath" : strDataBase);
                    break;
                case "Paradox":
                    strConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Extended Properties=Paradox 5.x;Data Source=" + (strDataBase == "" ? "@FilePath" : strDataBase) + ";";
                    break;
                case "Oracle":
                    strConnectionString = "server=" + strServerName + ";User ID=" + strUser + ";Password=" + strPsw + ";";
                    break;
                case "DBF":
                    strConnectionString = "Driver={Microsoft Visual FoxPro Driver};SourceType=DBF;SourceDB=" + (strDataBase == "" ? "@FilePath" : strDataBase);
                    break;
                case "Sybase":
                    strConnectionString = "Data Source=" + strServerName + ";UID=" + strUser + ";PWD=" + strPsw + ";Database=" + strDataBase;//;Port=5000
                    break;
            }
            return strConnectionString;
        }

        public static void WriteInfoLog(string message)
        {
            WriteLog("INFO", message);
        }

        public static void WriteErrorLog(string message)
        {
            WriteLog("ERROR", message);
        }

        /// <summary>
        /// 日志输出,按天生成文件
        /// </summary>
        /// <param name="logLevel">日志级别</param>
        /// <param name="message">日志信息</param>
        private static void WriteLog(string logLevel, string message)
        {
            //日志路径
            String logPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Logs\\";
            string logFile = logPath + "ImpData_" + DateTime.Today.ToString("yyyy-MM-dd") + ".log";

            if (!Directory.Exists(logPath))
            {// 目录不存在先创建目录
                Directory.CreateDirectory(logPath);
            }

            if (!File.Exists(logFile))
            {// 文件不存在先创建文件
                File.Create(logFile).Close();
            }

            StreamWriter sw = File.AppendText(logFile);
            sw.WriteLine(string.Format(@"{0}|{1}|{2}"
                                       , DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                                       , logLevel
                                       , message));
            sw.Close();
            sw.Dispose();
        }
    }
}
