using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NTAnalysisApplication
{
    public partial class Form1 : Form
    {
        //操作类
        Handle h = new Handle();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.ThreadJ));
        }

        #region 线程方法
        /// <summary>
        ///激光雷达
        /// </summary>
        /// <param name="o"></param>
        private void ThreadJ(object o)
        {
            while (true)
            {
                //每分钟固定秒开始计算
                string startTime = ConfigurationManager.AppSettings["TimeStart"];
                DateTime sTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm") + ":" + startTime);
                if (DateTime.Now >= sTime && DateTime.Now < sTime.AddSeconds(10))//允许一秒钟误差内执行
                {
                    if (DateTime.Now.Minute % 5 == 0)
                    {
                        h.SynData();
                    }
                    
                }
                //h.SynData();
                //间隔时间
                int time = Convert.ToInt32(ConfigurationManager.AppSettings["SecTime"]);
                //间隔一段时间后再执行
                TimeSpan waitTime = new TimeSpan(0, 0, 0, time);
                Thread.CurrentThread.Join(waitTime);
            }
        }
        #endregion
        /// <summary>
        /// 点击最小化到托盘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();   //隐藏窗体
                notifyIcon1.Visible = true; //使托盘图标可见
                notifyIcon1.ShowBalloonTip(3000, "程序最小化提示",
                    "图标已经缩小到托盘，打开窗口请双击图标即可。",
                    ToolTipIcon.Info);
            }
        }
        /// <summary>
        /// 双击托盘图标重新显示程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
        }
        /// <summary>
        /// 关闭窗口事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("确定要退出程序吗？", "退出程序", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
            else
            {
                Dispose();
                System.Environment.Exit(0); //不管什么线程都被强制退出。 
            }
        }

    }
}
