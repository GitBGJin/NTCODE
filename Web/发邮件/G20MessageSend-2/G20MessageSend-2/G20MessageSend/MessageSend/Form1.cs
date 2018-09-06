﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Configuration;
using Service;

namespace G20MessageSend
{
    public partial class Form1 : Form
    {
        string timeFrom = ConfigurationManager.AppSettings["timeFrom"];
        string timeTo = ConfigurationManager.AppSettings["timeTo"];
        string startTime = ConfigurationManager.AppSettings["startTime"];
        string endTime = ConfigurationManager.AppSettings["endTime"];
        string startTimeAQI = ConfigurationManager.AppSettings["startTimeAQI"];
        string endTimeAQI = ConfigurationManager.AppSettings["endTimeAQI"];
        string day = string.Empty;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ThreadStart threadStart = new ThreadStart(thread);
            Thread sendThread = new Thread(threadStart);
            sendThread.IsBackground = true;
            sendThread.Start();
        }

        private void thread()
        {
            while (true)
            {
                //Send.WriteTextLog("执行循环开始时间", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now);
                DateTime dt = DateTime.Now;

                //间隔时间
                int time = Convert.ToInt32(ConfigurationManager.AppSettings["time"]);
                //string dts = Convert.ToDateTime(startTime).ToString("yyyy-MM-dd HH:mm:ss");
                //if (dt >= Convert.ToDateTime(timeFrom) && dt <= Convert.ToDateTime(timeTo).AddDays(1)
                //    && dt >= Convert.ToDateTime(startTimeAQI) && dt <= Convert.ToDateTime(endTimeAQI))
                //{
                //    try
                //    {
                //        Send send = new Send();
                //        send.SendAQIMessage();
                //    }
                //    catch (Exception ex)
                //    {
                //        Send.WriteTextLog("执行循环报错", ex.ToString(), DateTime.Now);
                //    }
                //}
                //DateTime dtssds = Convert.ToDateTime(startTime);
                //DateTime dtsds = Convert.ToDateTime(endTime);
                if (dt >= Convert.ToDateTime(startTime) && dt <= Convert.ToDateTime(endTime))
                {
                    try
                    {
                        Send send = new Send();
                        send.SendMessage();
                    }
                    catch (Exception ex)
                    {
                        Send.WriteTextLog("执行循环报错", ex.ToString(), DateTime.Now);
                    }
                }

                //间隔一段时间后再执行
                TimeSpan waitTime = new TimeSpan(0, 0, time, 0);
                Thread.CurrentThread.Join(waitTime);
                //Send.WriteTextLog("执行循环结束时间", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }
    }
}
