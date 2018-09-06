using log4net;
using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NTAnalysisApplication
{
    public class Handle
    {
        ILog log = LogManager.GetLogger("App.Logging");//获取一个日志记录器
        AirAutoMonitorModel AirAutoMonitorModel = new AirAutoMonitorModel();
        BaseDataModel BaseDataModel = new BaseDataModel();
        DAL d_DAL = new DAL();
        //文件路径
        string filePath = ConfigurationManager.AppSettings["FilePath"].ToString();
        string filePathL = ConfigurationManager.AppSettings["FilePathL"].ToString();
        //数组初始化
        string Array = ConfigurationManager.AppSettings["Array"].ToString();
        private string[] arrays = new string[3];
        private const string AMS_AirAutoMonitor_Conn = "AMS_AirAutoMonitorConnection";
        // 激光雷达表名
        string destTblName = "dbo.TB_SuperStation_jiguangleida";

        /// <summary>
        /// 激光雷达解析
        /// </summary>
        public void SynData()
        {
            try
            {
                
                DirectoryInfo TheFolder = new DirectoryInfo(filePath);
                //遍历文件
                foreach (FileInfo NextFile in TheFolder.GetFiles())
                {
                    try
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.Add("DateTime", typeof(string));
                        dt.Columns.Add("DataType", typeof(string));
                        dt.Columns.Add("Height", typeof(string));
                        dt.Columns.Add("Number", typeof(string));
                        dt.Columns.Add("CreatDateTime", typeof(string));

                        string str = NextFile.Name;
                        str = str.Substring(0, str.Length - 4);
                        DateTime date = DateTime.Now;
                        try
                        {
                            date = DateTime.ParseExact(str, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                        string preDelSql = string.Format(@"
                                               delete from {0} 
                                                where  DateTime = '{1}'"
                                      , destTblName
                                      , date.ToString("yyyy-MM-dd HH:mm:ss")
                                      );
                        //先删除表中数据避免重复插入
                        ExcuteSQL(preDelSql, AMS_AirAutoMonitor_Conn);
                        string filePaths = filePath + NextFile.Name;
                        var filepaths = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePaths);
                        if (File.Exists(filepaths))
                        {
                            using (StreamReader sw = new StreamReader(filepaths))
                            {
                                int i = 0;
                                var strLine = sw.ReadLine();
                                strLine = strLine.TrimEnd('\0');
                                while (!string.IsNullOrEmpty(strLine))
                                {
                                    i++;
                                    var array = strLine.Split(',');
                                    //第一行数据
                                    if (array.Length > 0 && i == 1)
                                    {
                                        DataRow dr = dt.NewRow();
                                        dr["DateTime"] = date.ToString("yyyy-MM-dd HH:mm:ss");
                                        dr["CreatDateTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                        dr["DataType"] = array[0];
                                        dr["Number"] = array[1];
                                        dt.Rows.Add(dr);
                                    }
                                    //第二行数据
                                    if (array.Length > 0 && i == 2)
                                    {
                                        arrays = new string[Int32.Parse(Array)];
                                        arrays = strLine.Split(',');
                                    }
                                    //第三行数据
                                    if (array.Length > 0 && i > 2)
                                    {
                                        for (int n = 1; n < array.Length; n++)
                                        {
                                            DataRow dr = dt.NewRow();
                                            dr["DateTime"] = date.ToString("yyyy-MM-dd HH:mm:ss");
                                            dr["CreatDateTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                            dr["DataType"] = arrays[n];
                                            dr["Height"] = array[0];
                                            dr["Number"] = array[n];
                                            dt.Rows.Add(dr);
                                        }
                                    }
                                    strLine = sw.ReadLine();
                                }
                            }
                        }
                        //如果不存在则创建文件夹
                        if (!Directory.Exists(filePathL))
                        {
                            Directory.CreateDirectory(filePathL);//创建目录
                        }
                        FileInfo fi = new FileInfo(filepaths);
                        fi.CopyTo(filePathL + NextFile.Name, true);
                        //File.Copy(filepaths, filePathL + NextFile.Name, true);

                        // 添加数据
                        string connStr = ConfigurationManager.ConnectionStrings[AMS_AirAutoMonitor_Conn].ConnectionString;
                        SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(connStr, SqlBulkCopyOptions.UseInternalTransaction);
                        sqlbulkcopy.BulkCopyTimeout = 0;
                        sqlbulkcopy.DestinationTableName = destTblName;//目标表表名
                        sqlbulkcopy.ColumnMappings.Add("[DateTime]", "[DateTime]");
                        sqlbulkcopy.ColumnMappings.Add("[DataType]", "[DataType]");
                        sqlbulkcopy.ColumnMappings.Add("[Height]", "[Height]");
                        sqlbulkcopy.ColumnMappings.Add("[Number]", "[Number]");
                        sqlbulkcopy.ColumnMappings.Add("[CreatDateTime]", "[CreatDateTime]");

                        sqlbulkcopy.WriteToServer(dt);
                        sqlbulkcopy.Close();

                        File.Delete(filepaths);
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
                
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }

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
            catch (Exception ex)
            {
                log.Error(ex.ToString());
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
