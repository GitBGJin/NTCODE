﻿using log4net;
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

namespace NTDataProcessApplication
{
    public partial class Form1 : Form
    {
        //操作类
        Handle h = new Handle();

        public Form1()
        {
            InitializeComponent();
        }
        ILog log = LogManager.GetLogger("App.Logging");//获取一个日志记录器
        private void Form1_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now.AddDays(-1);
            dateTimePicker2.Value = DateTime.Now;
            //常规站数据接入（因为接口无分钟数据故已合并到小时线程中）
            //ThreadPool.QueueUserWorkItem(new WaitCallback(this.Access));
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.historyVOC));
            ////数据计算处理
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.ThreadBy1));
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.ThreadBy60));
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.ThreadBy60ShangHai));
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.ThreadByVOC));
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.ThreadByDay));
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.ThreadByMonth));
        }

        #region 线程方法
        /// <summary>
        /// 常规数据接入
        /// </summary>
        /// <param name="o"></param>
        private void Access(object o)
        {
            while (true)
            {
                ////每天1点10分自动补前一月数据
                string FillTime = ConfigurationManager.AppSettings["FillTime"];
                if (DateTime.Now >= Convert.ToDateTime(FillTime) && DateTime.Now < Convert.ToDateTime(FillTime).AddMinutes(5))
                {
                    DateTime startTime = Convert.ToDateTime(DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd 00:00:00"));//获取前上月0点
                    DateTime endTime = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 23:59:59"));//获取昨天23点
                    while(startTime <= endTime)
                    {
                        h.fillhistorydata(startTime, 1);
                        startTime = startTime.AddDays(1);
                    }
                }
                //间隔时间
                int time = Convert.ToInt32(ConfigurationManager.AppSettings["SpanBy1"]);
                //间隔一段时间后再执行
                TimeSpan waitTime = new TimeSpan(0, 0, time, 0);
                Thread.CurrentThread.Join(waitTime);
            }
        }
        /// <summary>
        /// 常规数据接入
        /// </summary>
        /// <param name="o"></param>
        private void historyVOC(object o)
        {
            while (true)
            {
                ////每天1点10分自动补前一月数据
                string FillTimeVOC = ConfigurationManager.AppSettings["FillTimeVOC"];
                if (DateTime.Now >= Convert.ToDateTime(FillTimeVOC) && DateTime.Now < Convert.ToDateTime(FillTimeVOC).AddMinutes(5))
                {
                    h.FillVOCStatisticData(DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd HH:00:00"), DateTime.Now.ToString("yyyy-MM-dd HH:00:00"));
                }

                //间隔时间
                int time = Convert.ToInt32(ConfigurationManager.AppSettings["SpanBy60"]);
                //间隔一段时间后再执行
                TimeSpan waitTime = new TimeSpan(0, 0, time, 0);
                Thread.CurrentThread.Join(waitTime);
            }
        }
        /// <summary>
        /// 手动同步历史数据
        /// </summary>
        /// <param name="o"></param>
        private void fillhistorydata(DateTime startTime,DateTime endTime)
        {
            while (startTime <= endTime)
            {
                //每小时固定分钟开始同步
                string FillHistoryTime = ConfigurationManager.AppSettings["FillHistoryTime"];
                DateTime sTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:") + FillHistoryTime + ":00");

                //每次同步几天数据
                //int FillDays = (endTime.Day - startTime.Day) + 1;
                int FillDays = new TimeSpan(endTime.Ticks - startTime.Ticks).Days;
                //int FillDays = Convert.ToInt32(ConfigurationManager.AppSettings["FillDays"]);

                if (DateTime.Now >= sTime && DateTime.Now < sTime.AddMinutes(1))//允许一分钟误差内执行
                {
                    h.fillhistorydata(startTime, FillDays);

                    startTime = startTime.AddDays(FillDays);
                }
                //h.fillhistorydata(startTime);

                //startTime = startTime.AddDays(FillDays);
                //间隔时间
                int time = Convert.ToInt32(ConfigurationManager.AppSettings["SpanBy60"]);
                //间隔一段时间后再执行
                TimeSpan waitTime = new TimeSpan(0, 0, time, 0);
                Thread.CurrentThread.Join(waitTime);
            }

        }
        /// <summary>
        /// 分钟线程
        /// </summary>
        /// <param name="o"></param>
        private void ThreadBy1(object o)
        {
            while (true)
            {
                //每分钟固定秒开始计算
                string startTime = ConfigurationManager.AppSettings["StartTime"];
                DateTime sTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm") + ":" + startTime);
                if (DateTime.Now >= sTime && DateTime.Now < sTime.AddSeconds(1))//允许一秒钟误差内执行
                {
                    h.RunBy1();
                    if (DateTime.Now.Minute % 5 == 0)
                    {
                        h.RunBy5();
                    }
                }
                //间隔时间
                int time = Convert.ToInt32(ConfigurationManager.AppSettings["SpanBy1"]);
                //间隔一段时间后再执行
                TimeSpan waitTime = new TimeSpan(0, 0, 0, time);
                Thread.CurrentThread.Join(waitTime);
            }
        }
        /// <summary>
        /// 小时线程
        /// </summary>
        /// <param name="o"></param>
        private void ThreadBy60(object o)
        {
            while (true)
            {
                //每小时固定分开始计算
                string startTime = ConfigurationManager.AppSettings["StartTime"];
                DateTime sTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:") + startTime + ":00");
                int startMin = Convert.ToInt32(startTime);
                if ((DateTime.Now >= sTime && DateTime.Now < sTime.AddMinutes(1)) || DateTime.Now.Minute <45 && DateTime.Now.Minute > startMin)//允许一分钟误差内执行,前10分钟增加解析频率
                {
                    h.RunBy60();
                }
                //h.RunBy60();
                #region 周边环境评价数据解析(放入新线程中执行)
                //string nearbySTime = ConfigurationManager.AppSettings["NearbySTime"];
                //DateTime NearbySTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:") + nearbySTime + ":00");

                //if (DateTime.Now >= NearbySTime && DateTime.Now < NearbySTime.AddMinutes(1))//允许一分钟误差内执行
                //{
                //    h.ShangHaiRealAQI();
                //}
                #endregion

                //间隔时间
                int time = Convert.ToInt32(ConfigurationManager.AppSettings["SpanBy60"]);
                //间隔一段时间后再执行
                TimeSpan waitTime = new TimeSpan(0, 0, time, 0);
                Thread.CurrentThread.Join(waitTime);
            }
        }
        /// <summary>
        /// 小时线程
        /// </summary>
        /// <param name="o"></param>
        private void ThreadBy60ShangHai(object o)
        {
            while (true)
            {
                #region 周边环境评价数据解析
                string nearbySTime = ConfigurationManager.AppSettings["NearbySTime"];
                DateTime NearbySTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:") + nearbySTime + ":00");
                ////测试，待删
                //h.ShangHaiRealAQI();

                if (DateTime.Now >= NearbySTime && DateTime.Now < NearbySTime.AddMinutes(1))//允许一分钟误差内执行
                {
                    h.ShangHaiRealAQI();
                }
                #endregion
            }
        }
        /// <summary>
        /// 小时线程
        /// </summary>
        /// <param name="o"></param>
        private void ThreadByVOC(object o)
        {
            while (true)
            {
                #region VOC汇总数据计算解析
                string vocSTime = ConfigurationManager.AppSettings["VOCSTime"];
                DateTime VOCSTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:") + vocSTime + ":00");

                if (DateTime.Now >= VOCSTime &&  DateTime.Now.Minute < 20)//允许一分钟误差内执行
                {
                    string svTime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss");
                    string evTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    h.FillVOCStatisticData(svTime, evTime);
                }
                #endregion
                //间隔时间
                int time = Convert.ToInt32(ConfigurationManager.AppSettings["SpanBy60"]);
                //间隔一段时间后再执行
                TimeSpan waitTime = new TimeSpan(0, 0, time, 0);
                Thread.CurrentThread.Join(waitTime);
            }
        }
        /// <summary>
        /// 日线程
        /// </summary>
        /// <param name="o"></param>
        private void ThreadByDay(object o)
        {
            while (true)
            {
                //每天定时开始计算
                string startTime = ConfigurationManager.AppSettings["DayStartTime"];
                DateTime sTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + startTime );
                if (DateTime.Now >= sTime && DateTime.Now < sTime.AddHours(1))//允许一小时误差内执行
                {
                    h.RunByDay();

                    //每天定时补前一天数据
                    DateTime fillDay = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
                    h.fillhistorydata(fillDay, 1);
                }
                //每天定时补前一天数据
                //DateTime fillDay = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
                //h.fillhistorydata(fillDay, 1);
                //h.RunByDay();
                //间隔时间
                int time = Convert.ToInt32(ConfigurationManager.AppSettings["SpanByDay"]);
                //间隔一段时间后再执行
                TimeSpan waitTime = new TimeSpan(0, time, 0, 0);
                Thread.CurrentThread.Join(waitTime);
            }
        }
        /// <summary>
        /// 月线程
        /// </summary>
        /// <param name="o"></param>
        private void ThreadByMonth(object o)
        {
            while (true)
            {
                //每月1日定时开始计算
                string startTime = ConfigurationManager.AppSettings["DayStartTime"];
                DateTime sTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + startTime);
                if (DateTime.Now.Day == 1)
                {
                    if (DateTime.Now >= sTime && DateTime.Now < sTime.AddHours(1))//允许一小时误差内执行
                    {
                        h.RunByMonth();
                    }
                }
                //h.RunByMonth();
                //间隔时间
                int time = Convert.ToInt32(ConfigurationManager.AppSettings["SpanByMonth"]);
                //间隔一段时间后再执行
                TimeSpan waitTime = new TimeSpan(time, 0, 0, 0);
                Thread.CurrentThread.Join(waitTime);
            }
        }
        #endregion

        #region 窗口事件
        /// <summary>
        /// 点击最小化到托盘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized && notifyIcon1.Visible==false)
            {
                this.Hide();   //隐藏窗体
                notifyIcon1.Visible = true; //使托盘图标可见
                notifyIcon1.ShowBalloonTip(3000, "程序最小化提示",
                    "图标已经缩小到托盘，打开窗口请双击图标即可。",
                    ToolTipIcon.Info);
            }
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
        /// <summary>
        /// 双击托盘图标重新显示程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Visible = true;

                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.Visible = false;
                this.WindowState = FormWindowState.Minimized;
            }
        }

        #endregion
        /// <summary>
        /// 同步历史数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要同步当前日期数据？", "手动同步", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                this.Hide();   //隐藏窗体
                notifyIcon1.Visible = true; //使托盘图标可见
                notifyIcon1.ShowBalloonTip(3000, "程序最小化提示",
                    "图标已经缩小到托盘，打开窗口请双击图标即可。",
                    ToolTipIcon.Info);
                //MessageBox.Show("分钟");

                DateTime dts = DateTime.Now;
                string sTime = dateTimePicker1.Text;
                string eTime = dateTimePicker2.Text;

                fillhistorydata(Convert.ToDateTime(sTime), Convert.ToDateTime(eTime));

                DateTime dte = DateTime.Now;
                TimeSpan ts = dte - dts;
                var minutes = ts.Minutes;
                MessageBox.Show("同步成功！时长：" + minutes+"分钟");
            }
        }
        /// <summary>
        /// 同步历史AQI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要同步当前日期AQI？", "同步AQI", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                DateTime dts = DateTime.Now;
                string sTime = dateTimePicker1.Text;
                string eTime = dateTimePicker2.Text;
                h.CalculateBy60(Convert.ToDateTime(sTime), Convert.ToDateTime(eTime));
                DateTime dte = DateTime.Now;
                TimeSpan ts = dte - dts;
                var minutes = ts.Minutes;
                MessageBox.Show("AQI同步成功！时长：" + minutes + "分钟");
            }
        }
        /// <summary>
        /// 同步历史voc汇总数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定同步当前日期VOC汇总数据？", "同步VOC汇总数据", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                DateTime dts = DateTime.Now;
                string sTime = dateTimePicker1.Text;
                string eTime = dateTimePicker2.Text;
                h.FillVOCStatisticData(sTime, eTime);
                DateTime dte = DateTime.Now;
                TimeSpan ts = dte - dts;
                var minutes = ts.Minutes;
                MessageBox.Show("VOC汇总数据同步成功！时长：" + minutes + "分钟");
            }
        }

       

       
    }
}
