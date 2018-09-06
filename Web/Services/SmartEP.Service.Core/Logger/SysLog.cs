using SmartEP.Core.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace SmartEP.Service.Core.Logger
{
    /// <summary>
    /// 名称：SysLog.aspx.cs
    /// 创建人：李飞
    /// 创建日期：2015-10-29
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：系统日志
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class SysLog
    {
        /// <summary>
        /// 追加错误日志
        /// </summary>
        /// <param name="message"></param>
        public static void WriteErrorLog(string message)
        {
            WriteLog(SysLogMode.Error, message);
        }

        /// <summary>
        /// 日志输出,按天生成文件
        /// </summary>
        /// <param name="logLevel">日志级别</param>
        /// <param name="message">日志信息</param>
        private static void WriteLog(SysLogMode logLevel, string message)
        {
            try
            {
                //日志路径
                String logPath = System.IO.Path.Combine(HttpRuntime.AppDomainAppPath, "Files\\Logs\\");
                string logFile = logPath + "SystemLog_" + DateTime.Today.ToString("yyyy-MM-dd") + ".log";

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
                                           , SmartEP.Core.Enums.EnumMapping.GetDesc(logLevel)
                                           , message));
                sw.Close();
                sw.Dispose();
            }
            catch { }
        }
    }
}
