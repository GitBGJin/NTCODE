using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NTAnalysisApplication
{
    static class Program
    {

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //一个程序只能执行一个(修改程序名字后依然能执行)
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);
            foreach (Process process in processes)
            {
                if (process.Id != current.Id)
                {
                    if (process.MainModule.FileName
                    == current.MainModule.FileName)
                    {
                        MessageBox.Show("程序已经运行！", Application.ProductName,
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
            }
            //处理未捕获的异常  
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            //处理UI线程异常   
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            //处理非UI线程异常   
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            Application.Run(new Form1());
        }

        /// <summary>
        /// 处理UI线程异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            ILog log = LogManager.GetLogger("App.Logging");//获取一个日志记录器


            string str = "";
            string strDateInfo = "出现应用程序未处理的异常：" + DateTime.Now.ToString() + "\r\n";
            Exception error = e.Exception as Exception;
            if (error != null)
            {
                str = string.Format(strDateInfo + "异常类型：{0}\r\n异常消息：{1}\r\n异常信息：{2}\r\n",
                     error.GetType().Name, error.Message, error.StackTrace);
            }
            else
            {
                str = string.Format("应用程序线程错误:{0}", e);
            }
            Thread.Sleep(2 * 1000);//交出线程占用CPU时间2秒钟。
            log.Fatal(str);
            DialogResult dr = MessageBox.Show("程序发生错误，请及时重启软件！", "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (dr == DialogResult.OK)
            {
                System.Environment.Exit(0); 
            }
            else
            { 
                
            }
        }
        /// <summary>
        /// 处理非UI线程异常 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ILog log = LogManager.GetLogger("App.Logging");//获取一个日志记录器

            string str = "";
            Exception error = e.ExceptionObject as Exception;
            string strDateInfo = "出现应用程序未处理的异常：" + DateTime.Now.ToString() + "\r\n";
            if (error != null)
            {
                str = string.Format(strDateInfo + "Application UnhandledException:{0};\n\r堆栈信息:{1}", error.Message, error.StackTrace);
            }
            else
            {
                str = string.Format("Application UnhandledError:{0}", e);
            }

            log.Fatal(str);
            DialogResult dr = MessageBox.Show("程序发生错误，请及时重启软件！", "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (dr == DialogResult.OK)
            {
                System.Environment.Exit(0);
            }
            else
            {

            }
        }

    }
}
