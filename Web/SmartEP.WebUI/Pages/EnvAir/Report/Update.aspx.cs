﻿using log4net;
using NTDataProcessApplication;
using SmartEP.Utilities.Web.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartEP.WebUI.Pages.EnvAir.Report
{
    public partial class Update : SmartEP.WebUI.Common.BasePage
    {
        //操作类
        Handle h = new Handle();
        ILog log = LogManager.GetLogger("FileLogging");//获取一个日志记录器
        string isAudit = string.Empty;
        string MData = string.Empty;
        protected override void OnPreInit(EventArgs e)
        {
            isAudit = PageHelper.GetQueryString("auditOrNot");
            if (isAudit != null && isAudit != "")
            {
                factorCbxRsm.isAudit(isAudit);
            }
            //string isSuper = PageHelper.GetQueryString("superOrNot");
            //if (isAudit != null && isAudit != "")
            //{
            //    pointCbxRsm.isSuper(isSuper);
            //}
            pointCbxRsm.isSuper("AirDER");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitControl();
            }
        }
        /// <summary>
        /// 时间初始化
        /// </summary>
        private void InitControl()
        {
            dtpBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:00"));
            dtpEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00"));
        }
        /// <summary>
        /// 点击同步按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UpdateMessage_Click(object sender, EventArgs e)
        {
            try 
            {
                string MissData = string.Empty;
                if (dtpBegin.SelectedDate == null || dtpEnd.SelectedDate == null)
                {
                    Alert("日期不能为空!");
                    return;
                }
                DateTime startTime = (DateTime)dtpBegin.SelectedDate;
                DateTime endTime = (DateTime)dtpEnd.SelectedDate;
                if (startTime > endTime)
                {
                    Alert("开始时间不能大于结束时间!");
                    return;
                }
                if ((int)(endTime - startTime).TotalDays > 7)
                {
                    Alert("请选择一周以内进行同步!");
                    return;
                }
                string[] portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
                string[] factorCodes = factorCbxRsm.GetFactorValues(SmartEP.Core.Enums.CbxRsmReturnType.Code);
                fillhistorydata(startTime, endTime, portIds, factorCodes, out MissData);
                if (MissData == "接口该处无数据:")
                {
                    Alert("数据同步成功!");
                }
                else
                {
                    if (MissData.EndsWith(":"))
                    {
                        Alert(MissData.Substring(0, MissData.Length - 8));
                    }
                    else
                    {
                        Alert(MissData.TrimEnd(','));
                    }
                }
                

                
            }
            catch(Exception ex)
            {
                log.Error(ex.ToString());
            }
            
        }
        /// <summary>
        /// 同步历史数据
        /// </summary>
        /// <param name="o"></param>
        private void fillhistorydata(DateTime startTime, DateTime endTime, string[] portIds, string[] factorCodes, out string MissData)
        {
            MissData = string.Empty;
            try 
            {
                while (startTime <= endTime)
                {
                    //每小时固定分钟开始计算
                    string FillHistoryTime = ConfigurationManager.AppSettings["FillHistoryTime"];
                    DateTime sTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:") + FillHistoryTime + ":00");

                    int FillDays = Convert.ToInt32(ConfigurationManager.AppSettings["FillDays"]);

                    //if (DateTime.Now >= sTime && DateTime.Now < sTime.AddMinutes(1))//允许一分钟误差内执行
                    //{
                    //    h.fillhistorydata(startTime);

                    //    startTime = startTime.AddDays(FillDays);
                    //}
                    h.fillhistorydata(startTime, FillDays, portIds, factorCodes, out MData);
                    MissData += MData;
                    startTime = startTime.AddDays(FillDays);
                    //间隔时间
                    //int time = Convert.ToInt32(ConfigurationManager.AppSettings["SpanBy60"]);
                    //间隔一段时间后再执行
                    //TimeSpan waitTime = new TimeSpan(0, 0, time, 0);
                    //Thread.CurrentThread.Join(waitTime);
                }
            }
            catch(Exception ex)
            {
                log.Error(ex.ToString());
            }
            
        }
        /// <summary>
        /// 分钟线程
        /// </summary>
        /// <param name="o"></param>
        //private void ThreadBy1(object o)
        //{
        //    while (true)
        //    {
        //        //每分钟固定秒开始计算
        //        string startTime = ConfigurationManager.AppSettings["StartTime"];
        //        DateTime sTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm") + ":" + startTime);
        //        if (DateTime.Now >= sTime && DateTime.Now < sTime.AddSeconds(1))//允许一秒钟误差内执行
        //        {
        //            h.RunBy1();
        //            if (DateTime.Now.Minute % 5 == 0)
        //            {
        //                h.RunBy5();
        //            }
        //        }
        //        //TODO: 测试待删
        //        //h.RunBy1();
        //        //h.RunBy5();
        //        //间隔时间
        //        int time = Convert.ToInt32(ConfigurationManager.AppSettings["SpanBy1"]);
        //        //间隔一段时间后再执行
        //        TimeSpan waitTime = new TimeSpan(0, 0, 0, time);
        //        Thread.CurrentThread.Join(waitTime);
        //    }
        //}
        /// <summary>
        /// 小时线程
        /// </summary>
        /// <param name="o"></param>
        //private void ThreadBy60(object o)
        //{
        //    while (true)
        //    {
        //        //每小时固定分开始计算
        //        string startTime = ConfigurationManager.AppSettings["StartTime"];
        //        DateTime sTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:") + startTime + ":00");
        //        if (DateTime.Now >= sTime && DateTime.Now < sTime.AddMinutes(1))//允许一分钟误差内执行
        //        {
        //            h.RunBy60();
        //        }
        //        //TODO: 测试待删
        //        //h.RunBy60();
        //        //间隔时间
        //        int time = Convert.ToInt32(ConfigurationManager.AppSettings["SpanBy60"]);
        //        //间隔一段时间后再执行
        //        TimeSpan waitTime = new TimeSpan(0, 0, time, 0);
        //        Thread.CurrentThread.Join(waitTime);
        //    }
        //}
        /// <summary>
        /// 日线程
        /// </summary>
        /// <param name="o"></param>
        //private void ThreadByDay(object o)
        //{
        //    while (true)
        //    {
        //        //每天定时开始计算
        //        string startTime = ConfigurationManager.AppSettings["StartTime"];
        //        DateTime sTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + startTime + ":00:00");
        //        if (DateTime.Now >= sTime && DateTime.Now < sTime.AddHours(1))//允许一小时误差内执行
        //        {
        //            h.RunByDay();
        //        }
        //        //h.RunByDay();
        //        //间隔时间
        //        int time = Convert.ToInt32(ConfigurationManager.AppSettings["SpanByDay"]);
        //        //间隔一段时间后再执行
        //        TimeSpan waitTime = new TimeSpan(0, time, 0, 0);
        //        Thread.CurrentThread.Join(waitTime);
        //    }
        //}
        /// <summary>
        /// 月线程
        /// </summary>
        /// <param name="o"></param>
        //private void ThreadByMonth(object o)
        //{
        //    while (true)
        //    {
        //        //每月1日定时开始计算
        //        string startTime = ConfigurationManager.AppSettings["StartTime"];
        //        DateTime sTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd ") + startTime + ":00:00");
        //        if (DateTime.Now.Day == 1)
        //        {
        //            if (DateTime.Now >= sTime && DateTime.Now < sTime.AddHours(1))//允许一小时误差内执行
        //            {
        //                h.RunByMonth();
        //            }
        //        }
        //        //h.RunByMonth();
        //        //间隔时间
        //        int time = Convert.ToInt32(ConfigurationManager.AppSettings["SpanByMonth"]);
        //        //间隔一段时间后再执行
        //        TimeSpan waitTime = new TimeSpan(time, 0, 0, 0);
        //        Thread.CurrentThread.Join(waitTime);
        //    }
        //}
    }
}