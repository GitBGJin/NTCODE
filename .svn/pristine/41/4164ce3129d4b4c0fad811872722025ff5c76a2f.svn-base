using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace SmartEP.Utilities.AdoData
{
    public class BaseDAHelper
    {
        /// <summary>
        /// System.Data.SqlClient.SqlCommand 的参数列表
        /// </summary>
        private ArrayList ProcedureParameters = new ArrayList();	// 存储过程使用的参数

        public BaseDAHelper()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        #region 成员方法

        public void ClearParameters()
        {
            ProcedureParameters.Clear();
        }

        public void SetProcedureParameters(SqlParameter param, object paramValue)
        {
            param.Value = paramValue;
            this.ProcedureParameters.Add(param);
        }

        public void SetProcedureParameters(SqlParameter param)
        {
            this.ProcedureParameters.Add(param);
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="ConnectinString"></param>
        public void ExecuteProcNonQuery(string procName, string ConnectinString)
        {
            DatabaseHelper data = new DatabaseHelper();
            SqlParameter[] prams = new SqlParameter[ProcedureParameters.Count];
            for (int i = 0; i < ProcedureParameters.Count; i++)
            {
                SqlParameter param = (SqlParameter)ProcedureParameters[i];
                prams[i] = param;
            }
            data.RunProc(procName, ConnectinString, prams);
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="ConnectinString"></param>
        /// <returns></returns>
        public DataView ExecuteProc(string procName, string ConnectinString)
        {
            try
            {
                DataView myview = null;
                DatabaseHelper data = new DatabaseHelper();
                SqlParameter[] prams = new SqlParameter[ProcedureParameters.Count];
                for (int i = 0; i < ProcedureParameters.Count; i++)
                {
                    SqlParameter param = (SqlParameter)ProcedureParameters[i];
                    prams[i] = param;
                }
                myview = data.RunProcDataView(procName, ConnectinString, prams);
                return myview;
            }
            catch (Exception ex)
            {
                WriteTextLog("GetGridViewPager" + "执行存储过程", ex.Message, DateTime.Now);
                return null;
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
            string path = AppDomain.CurrentDomain.BaseDirectory + @"System\Log\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fileFullPath = path + time.ToString("yyyy-MM-dd") + ".System.txt";
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
        #endregion 成员方法
    }
}
