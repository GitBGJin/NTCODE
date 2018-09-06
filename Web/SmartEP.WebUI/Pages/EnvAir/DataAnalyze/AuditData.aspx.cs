﻿using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.BusinessRule;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.Core.Enums;
using SmartEP.Service.DataAnalyze.Air.DataQuery;
using SmartEP.Service.Frame;
using SmartEP.Utilities.Calendar;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Utilities.Office;
using SmartEP.Utilities.Web.UI;
//using SmartEP.WebControl.CbxRsm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SmartEP.Service.AutoMonitoring.Air;
using log4net;

namespace SmartEP.WebUI.Pages.EnvAir.DataAnalyze
{
    /// <summary>
    /// 名称：AuditData.aspx.cs
    /// 创建人：刘长敏
    /// 创建日期：2015-08-14
    /// 维护人员：徐阳、刘晋
    /// 最新维护人员：刘晋
    /// 最新维护日期：2017-05-23
    /// 功能摘要：环境空气审核数据(小时数据、日数据、周数据、月数据、年数据)
    /// 虚拟分页类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class AuditData : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        InfectantBy1Service m_Min1Data = Singleton<InfectantBy1Service>.GetInstance();
        InfectantBy5Service m_Min5Data = Singleton<InfectantBy5Service>.GetInstance();
        InfectantBy60Service m_Min60Data = Singleton<InfectantBy60Service>.GetInstance();
        InfectantByDayService m_DayOriData = Singleton<InfectantByDayService>.GetInstance();
        InfectantByMonthService m_MonthOriData = Singleton<InfectantByMonthService>.GetInstance();
        DataQueryByHourService m_HourData = Singleton<DataQueryByHourService>.GetInstance();
        DataQueryByDayService m_DayData = Singleton<DataQueryByDayService>.GetInstance();
        DataQueryByWeekService m_WeekData = Singleton<DataQueryByWeekService>.GetInstance();
        DataQueryByMonthService m_MonthData = Singleton<DataQueryByMonthService>.GetInstance();
        DataQueryBySeasonService m_SeasonData = Singleton<DataQueryBySeasonService>.GetInstance();
        DataQueryByYearService m_YearData = Singleton<DataQueryByYearService>.GetInstance();
        InstrumentChannelService m_InstrumentChannelService = Singleton<InstrumentChannelService>.GetInstance();
        string LZSfactorName = string.Empty;
        string LZSPfactor = string.Empty;

        //因子
        SmartEP.Service.BaseData.Channel.AirPollutantService m_AirPollutantService = Singleton<SmartEP.Service.BaseData.Channel.AirPollutantService>.GetInstance();

        //获取因子上下限数据处理服务
        ExcessiveSettingService m_ExcessiveSettingService = new ExcessiveSettingService();
        //代码项服务层
        DictionaryService dicService = new DictionaryService();
        /// <summary>
        /// 选择因子
        /// </summary>
        private IList<IPollutant> factors = null;
        /// <summary>
        /// 选择站点
        /// </summary>
        private IList<IPoint> points = null;
        /// <summary>
        /// 统计
        /// </summary>
        DataView dvStatistical = null;
        /// <summary>
        /// 国家标记位
        /// </summary>
        IQueryable<V_CodeMainItemEntity> siteTypeEntites = null;
        /// <summary>
        /// 默认是否常规站字段为空
        /// </summary>
        string isAudit = string.Empty;
        static DateTime dtms = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00"));
        /// <summary>
        /// 默认是否超级站字段为空
        /// </summary>
        string isSuper = string.Empty;
        ILog log = LogManager.GetLogger("FileLogging");//获取一个日志记录器
        /// <summary>
        /// 设置站点控件选中超级站
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreInit(EventArgs e)
        {
            string pageType = PageHelper.GetQueryString("type");
            isSuper = PageHelper.GetQueryString("superOrNot");
            if (pageType == "htfxy" || pageType == "yjtystfxy" || pageType == "gfxy" || pageType == "sbdljpy" || pageType == "wsqt" || pageType == "lzspy" || pageType == "owdd" || pageType == "tyfsy" || pageType == "ljpy")
            {
                pointCbxRsm.isSuper(isSuper);
                pointCbxRsm.DefaultAllSelected = true;
            }
            isAudit = PageHelper.GetQueryString("auditOrNot");
            if (isAudit != null && isAudit != "")
            {
                factorCbxRsm.isAudit(isAudit);
            }
        }
        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.ViewState["type"] = PageHelper.GetQueryString("type");
                this.ViewState["flag"] = PageHelper.GetQueryString("flag");
                this.ViewState["Files"] = "审核数据";
                InitControl();
            }
        }

        #region 页面控件初始化
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {

            string pageType = PageHelper.GetQueryString("type");
            if (pageType == "lzspy" || pageType == "yjtystfxy")
            {
                //数据类型
                if (this.ViewState["flag"].ToString() == "0")
                {
                    radlDataType.Items.Add(new ListItem("小时数据", PollutantDataType.Hour.ToString()));
                    radlDataType.Items.Add(new ListItem("日数据", PollutantDataType.Day.ToString()));
                }
                else
                {
                    radlDataType.Items.Add(new ListItem("小时数据", PollutantDataType.Hour.ToString()));
                    radlDataType.Items.Add(new ListItem("日数据", PollutantDataType.Day.ToString()));
                    radlDataType.Items.Add(new ListItem("周数据", PollutantDataType.Week.ToString()));
                    radlDataType.Items.Add(new ListItem("月数据", PollutantDataType.Month.ToString()));
                    radlDataType.Items.Add(new ListItem("季数据", PollutantDataType.Season.ToString()));
                    radlDataType.Items.Add(new ListItem("年数据", PollutantDataType.Year.ToString()));

                    //radlDataTypeOri.Items.Add(new ListItem("分钟数据", PollutantDataType.Min1.ToString()));
                    //radlDataTypeOri.Items.Add(new ListItem("五分钟数据", PollutantDataType.Min5.ToString()));
                    radlDataTypeOri.Items.Add(new ListItem("小时数据", PollutantDataType.Min60.ToString()));
                    radlDataTypeOri.Items.Add(new ListItem("日数据", PollutantDataType.OriDay.ToString()));
                    radlDataTypeOri.Items.Add(new ListItem("月数据", PollutantDataType.OriMonth.ToString()));
                }
            }
            else if (pageType == "tyfsy")
            {
                //数据类型
                if (this.ViewState["flag"].ToString() == "0")
                {
                    radlDataType.Items.Add(new ListItem("小时数据", PollutantDataType.Hour.ToString()));
                    radlDataType.Items.Add(new ListItem("日数据", PollutantDataType.Day.ToString()));
                }
                else
                {
                    radlDataType.Items.Add(new ListItem("小时数据", PollutantDataType.Hour.ToString()));
                    radlDataType.Items.Add(new ListItem("日数据", PollutantDataType.Day.ToString()));
                    radlDataType.Items.Add(new ListItem("周数据", PollutantDataType.Week.ToString()));
                    radlDataType.Items.Add(new ListItem("月数据", PollutantDataType.Month.ToString()));
                    radlDataType.Items.Add(new ListItem("季数据", PollutantDataType.Season.ToString()));
                    radlDataType.Items.Add(new ListItem("年数据", PollutantDataType.Year.ToString()));

                    //radlDataTypeOri.Items.Add(new ListItem("分钟数据", PollutantDataType.Min1.ToString()));
                    radlDataTypeOri.Items.Add(new ListItem("十分钟数据", PollutantDataType.Min5.ToString()));
                    radlDataTypeOri.Items.Add(new ListItem("小时数据", PollutantDataType.Min60.ToString()));
                    radlDataTypeOri.Items.Add(new ListItem("日数据", PollutantDataType.OriDay.ToString()));
                    radlDataTypeOri.Items.Add(new ListItem("月数据", PollutantDataType.OriMonth.ToString()));
                }
            }
            else if (pageType == "yssh")
            {
                //数据类型
                if (this.ViewState["flag"].ToString() == "0")
                {
                    radlDataType.Items.Add(new ListItem("小时数据", PollutantDataType.Hour.ToString()));
                    radlDataType.Items.Add(new ListItem("日数据", PollutantDataType.Day.ToString()));
                }
                else
                {
                    radlDataType.Items.Add(new ListItem("小时数据", PollutantDataType.Hour.ToString()));
                    radlDataType.Items.Add(new ListItem("日数据", PollutantDataType.Day.ToString()));
                    radlDataType.Items.Add(new ListItem("周数据", PollutantDataType.Week.ToString()));
                    radlDataType.Items.Add(new ListItem("月数据", PollutantDataType.Month.ToString()));
                    radlDataType.Items.Add(new ListItem("季数据", PollutantDataType.Season.ToString()));
                    radlDataType.Items.Add(new ListItem("年数据", PollutantDataType.Year.ToString()));

                    radlDataTypeOri.Items.Add(new ListItem("小时数据", PollutantDataType.Min60.ToString()));
                    radlDataTypeOri.Items.Add(new ListItem("日数据", PollutantDataType.OriDay.ToString()));
                    radlDataTypeOri.Items.Add(new ListItem("月数据", PollutantDataType.OriMonth.ToString()));
                }
                //区域查询功能可见
                selectYs.Visible = true;
                foreach (Telerik.Web.UI.GridCommandItem item in gridAudit.Items)
                {
                    //标记位说明可见
                    RadButton button = (RadButton)item.FindControl("RadButton1");
                    button.Visible = true;
                }
            }
            else if (pageType == "htfxy")
            {
                //数据类型
                if (this.ViewState["flag"].ToString() == "0")
                {
                    radlDataType.Items.Add(new ListItem("小时数据", PollutantDataType.Hour.ToString()));
                    radlDataType.Items.Add(new ListItem("日数据", PollutantDataType.Day.ToString()));
                }
                else
                {
                    radlDataType.Items.Add(new ListItem("小时数据", PollutantDataType.Hour.ToString()));
                    radlDataType.Items.Add(new ListItem("日数据", PollutantDataType.Day.ToString()));
                    radlDataType.Items.Add(new ListItem("周数据", PollutantDataType.Week.ToString()));
                    radlDataType.Items.Add(new ListItem("月数据", PollutantDataType.Month.ToString()));
                    radlDataType.Items.Add(new ListItem("季数据", PollutantDataType.Season.ToString()));
                    radlDataType.Items.Add(new ListItem("年数据", PollutantDataType.Year.ToString()));

                    radlDataTypeOri.Items.Add(new ListItem("五分钟数据", PollutantDataType.Min5.ToString()));
                    radlDataTypeOri.Items.Add(new ListItem("小时数据", PollutantDataType.Min60.ToString()));
                    radlDataTypeOri.Items.Add(new ListItem("日数据", PollutantDataType.OriDay.ToString()));
                    radlDataTypeOri.Items.Add(new ListItem("月数据", PollutantDataType.OriMonth.ToString()));
                }
            }
            else
            {
                //数据类型
                if (this.ViewState["flag"].ToString() == "0")
                {
                    radlDataType.Items.Add(new ListItem("小时数据", PollutantDataType.Hour.ToString()));
                    radlDataType.Items.Add(new ListItem("日数据", PollutantDataType.Day.ToString()));
                }
                else
                {
                    radlDataType.Items.Add(new ListItem("小时数据", PollutantDataType.Hour.ToString()));
                    radlDataType.Items.Add(new ListItem("日数据", PollutantDataType.Day.ToString()));
                    radlDataType.Items.Add(new ListItem("周数据", PollutantDataType.Week.ToString()));
                    radlDataType.Items.Add(new ListItem("月数据", PollutantDataType.Month.ToString()));
                    radlDataType.Items.Add(new ListItem("季数据", PollutantDataType.Season.ToString()));
                    radlDataType.Items.Add(new ListItem("年数据", PollutantDataType.Year.ToString()));

                    radlDataTypeOri.Items.Add(new ListItem("分钟数据", PollutantDataType.Min1.ToString()));
                    radlDataTypeOri.Items.Add(new ListItem("五分钟数据", PollutantDataType.Min5.ToString()));
                    radlDataTypeOri.Items.Add(new ListItem("小时数据", PollutantDataType.Min60.ToString()));
                    radlDataTypeOri.Items.Add(new ListItem("日数据", PollutantDataType.OriDay.ToString()));
                    radlDataTypeOri.Items.Add(new ListItem("月数据", PollutantDataType.OriMonth.ToString()));
                }
            }

            radlDataType.SelectedValue = PollutantDataType.Hour.ToString();
            radlDataTypeOri.SelectedValue = PollutantDataType.Min60.ToString();

            dtpHour.Visible = true;
            dtpDay.Visible = false;
            dtpMonth.Visible = false;
            dbtHour.Visible = false;
            dbtDay.Visible = false;
            dbtMonth.Visible = false;
            dbtSeason.Visible = false;
            dbtYear.Visible = false;
            dbtWeek.Visible = false;



            string PointName = PageHelper.GetQueryString("PointName");
            string DateBegin = PageHelper.GetQueryString("DTBegin");
            string DateEnd = PageHelper.GetQueryString("DTEnd");
            string factors = PageHelper.GetQueryString("Factors");
            string ddlSel = PageHelper.GetQueryString("ddlSel");

            hourBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddHours(-47).ToString("yyyy-MM-dd HH:00"));
            hourEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00"));
            dtpBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddHours(-24).ToString("yyyy-MM-dd HH:mm"));
            dtpEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));

            dtpDayBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd"));
            dtpDayEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));

            dtpMonthBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddYears(-1).ToString("yyyy-MM"));
            dtpMonthEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToString("yyyy-MM"));

            dayBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd"));
            dayEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
            monthBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddYears(-1).ToString("yyyy-MM"));
            monthEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToString("yyyy-MM"));
            seasonBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"));
            seasonEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"));
            weekBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"));
            weekEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"));
            yearBegin.SelectedDate = DateTime.Now.AddYears(-5);
            yearEnd.SelectedDate = DateTime.Now.AddYears(-1);

            BindWeekFComboBox();
            BindWeekTComboBox();

            //时间框初始化
            //hourBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddHours(-47));
            //hourEnd.SelectedDate = DateTime.Now;
            if (PointName != "")
            {
                pointCbxRsm.SetPointValuesFromNames(PointName);
                factorCbxRsm.SetFactorValuesFromNames(factors);
                if (ddlSel == "1")
                {
                    radlDataTypeOri.Visible = false;
                    radlDataType.Visible = true;

                    hourBegin.SelectedDate = Convert.ToDateTime(DateBegin);
                    hourEnd.SelectedDate = Convert.ToDateTime(DateEnd);

                    hourBegin.DateInput.DateFormat = "yyyy-MM-dd HH:00";
                    hourEnd.DateInput.DateFormat = "yyyy-MM-dd HH:00";
                    dtpHour.Visible = false;
                    dtpDay.Visible = false;
                    dtpMonth.Visible = false;
                    dbtHour.Visible = true;
                    dbtDay.Visible = false;
                    dbtMonth.Visible = false;
                    dbtSeason.Visible = false;
                    dbtYear.Visible = false;
                    dbtWeek.Visible = false;
                }
                else
                {
                    dtpBegin.SelectedDate = Convert.ToDateTime(DateBegin);
                    dtpEnd.SelectedDate = Convert.ToDateTime(DateEnd);
                }

                ddlDataSource.SelectedIndex = Convert.ToInt32(ddlSel);
            }

            else
            {
                //string pageType = this.ViewState["type"].ToString();
                if (pageType == "htfxy")//黑炭分析仪
                {
                    //pointCbxRsm.SetPointValuesFromNames("南门");
                    string PollutantName = "";
                    string PollutantCode = "";
                    BindFactors("e5b6d666-24d1-473a-b15a-33a36245d44f", out PollutantName, out PollutantCode);
                    factorCbxRsm.SetFactorValuesFromCodes(PollutantCode.TrimEnd(';'));

                    this.ViewState["Files"] = "黑碳分析仪";
                }
                else if (pageType == "yjtystfxy")//有机碳，元素碳分析仪
                {
                    //pointCbxRsm.SetPointValuesFromNames("南门");
                    string PollutantName = "";
                    string PollutantCode = "";
                    BindFactors("14b38adf-d899-4362-99ff-6a9e9dd35485", out PollutantName, out PollutantCode);
                    //string PollutantName = BindFactors("14b38adf-d899-4362-99ff-6a9e9dd35485");
                    factorCbxRsm.SetFactorValuesFromCodes(PollutantCode.TrimEnd(';'));
                    this.ViewState["Files"] = "ECOC";
                }
                else if (pageType == "gfxy")//汞分析仪
                {
                    pointCbxRsm.SetPointValuesFromNames("南门");
                    //pointCbxRsm.SetPointValuesFromNames("南门");
                    string PollutantName = "";
                    string PollutantCode = "";
                    BindFactors("c50a2fc0-0832-42b0-be17-640503c9de70", out PollutantName, out PollutantCode);
                    //string PollutantName = BindFactors("c50a2fc0-0832-42b0-be17-640503c9de70");
                    factorCbxRsm.SetFactorValuesFromCodes(PollutantCode.TrimEnd(';'));
                    this.ViewState["Files"] = "汞分析仪";
                }
                else if (pageType == "sbdljpy")//三波段粒径谱
                {
                    //pointCbxRsm.SetPointValuesFromNames("监测站");
                    string PollutantName = "";
                    string PollutantCode = "";
                    BindFactors("59f02681-093f-48f0-9cac-ac59acd7038f", out PollutantName, out PollutantCode);
                    //string PollutantName = BindFactors("c219f214-df1c-481c-80a5-d4934e0a27c8");
                    factorCbxRsm.SetFactorValuesFromCodes(PollutantCode.TrimEnd(';'));
                    this.ViewState["Files"] = "浊度仪";
                }
                else if (pageType == "wsqt")//温室气体
                {
                    //pointCbxRsm.SetPointValuesFromNames("南门");
                    //pointCbxRsm.SetPointValuesFromNames("监测站");
                    string PollutantName = "";
                    string PollutantCode = "";
                    BindFactors("40f1064d-dd43-45e3-9eb4-8ff3805931c7", out PollutantName, out PollutantCode);
                    //string PollutantName = BindFactors("40f1064d-dd43-45e3-9eb4-8ff3805931c7");
                    factorCbxRsm.SetFactorValuesFromCodes(PollutantCode + "a34004");
                    this.ViewState["Files"] = "温室气体";
                }
                else if (pageType == "lzspy")//离子色谱
                {
                    //pointCbxRsm.SetPointValuesFromNames("南门");
                    string PollutantName = "";
                    string PollutantCode = "";
                    BindFactors("5575a0e1-d948-4566-9dcd-4b4767688add", out PollutantName, out PollutantCode);
                    factorCbxRsm.SetFactorValuesFromCodes(PollutantCode.TrimEnd(';'));
                    this.ViewState["Files"] = "离子色谱仪";
                }
                else if (pageType == "tyfsy")//太阳辐射仪
                {
                    //pointCbxRsm.SetPointValuesFromNames("南门");
                    string PollutantName = "";
                    string PollutantCode = "";
                    BindFactors("aabe91e0-29a4-427c-becc-0b29f1224422", out PollutantName, out PollutantCode);
                    factorCbxRsm.SetFactorValuesFromCodes(PollutantCode);
                    this.ViewState["Files"] = "太阳辐射仪";
                }
                else if (pageType == "owdd")//Opsis稳定度
                {
                    //pointCbxRsm.SetPointValuesFromNames("监测站");
                    string PollutantName = "";
                    string PollutantCode = "";
                    BindFactors("83f24b18-85d1-409d-b77d-377863c39a93", out PollutantName, out PollutantCode);
                    //string PollutantName = BindFactors("83f24b18-85d1-409d-b77d-377863c39a93");
                    factorCbxRsm.SetFactorValuesFromCodes(PollutantCode.TrimEnd(';'));
                    this.ViewState["Files"] = "Opsis稳定度";
                }
                else if (pageType == "ljpy")//粒径谱仪
                {
                    //pointCbxRsm.SetPointValuesFromNames("监测站");
                    string PollutantName = "";
                    string PollutantCode = "";

                    BindFactors("da92c7c1-4932-4007-a6d5-2866aa8c63f1", out PollutantName, out PollutantCode);
                    //string PollutantName = BindFactors("83f24b18-85d1-409d-b77d-377863c39a93");
                    factorCbxRsm.SetFactorValuesFromCodes(PollutantCode.TrimEnd(';'));
                    this.ViewState["Files"] = "粒径谱仪";
                }
                else
                {
                    //国控点，对照点，路边站
                    MonitoringPointAirService m_MonitoringPointAirService = Singleton<MonitoringPointAirService>.GetInstance();
                    string strpointName = "";
                    IQueryable<MonitoringPointEntity> EnableOrNotports = m_MonitoringPointAirService.RetrieveAirMPListByEnable();
                    string[] EnableOrNotportsarry = EnableOrNotports.Where(p => p.ContrlUid == "6fadff52-2338-4319-9f1d-7317823770ad" || p.ContrlUid == "c1158eb6-4d69-4846-a963-d16b9d2794ca").Select(p => p.MonitoringPointName).ToArray();
                    foreach (string point in EnableOrNotportsarry)
                    {
                        strpointName += point + ";";
                    }
                    pointCbxRsm.SetPointValuesFromNames(strpointName);
                    //因子
                    //SmartEP.Service.BaseData.Channel.AirPollutantService m_AirPollutantService = Singleton<SmartEP.Service.BaseData.Channel.AirPollutantService>.GetInstance();
                    //IQueryable<PollutantCodeEntity> Pollutant = m_AirPollutantService.RetrieveListByCalAQI();
                    //string PollutantName = "";
                    //string[] pollutantarry = Pollutant.Select(p => p.PollutantName).ToArray();
                    //foreach (string strName in pollutantarry)
                    //{
                    //    PollutantName += strName + ";";
                    //}
                    //factorCbxRsm.SetFactorValuesFromNames(PollutantName);
                    string pollutantName = System.Configuration.ConfigurationManager.AppSettings["AirPollutant2"];
                    factorCbxRsm.SetFactorValuesFromCodes(pollutantName);
                    PointFactor.Visible = true;
                    PointFactor.SelectedIndex = 0;
                    HiddenPointFactor.Value = "factor";
                    this.ViewState["Files"] = "常规参数";
                }

            }
        }
        #endregion

        /// <summary>
        /// 对粒径谱仪因子分类
        /// </summary>
        public void groupLJPY()
        {
            factors = factorCbxRsm.GetFactors();
            StringBuilder sbMin = new StringBuilder();
            StringBuilder sbMid = new StringBuilder();
            StringBuilder sbMax = new StringBuilder();
            //string[] arr1 = new string[] { "a51008", "a51009", "a51010", "a51011" };
            //List<string> list1 = arr1.ToList();
            //string[] arr2 = new string[] { "a51012", "a51013", "a51014", "a51015", "a51016", "a51017", "a51018", "a51019", "a51020", "a51021", "a51022" };
            //List<string> list2 = arr2.ToList();
            foreach (IPollutant ip in factors)
            {
                double num = Convert.ToDouble(ip.PollutantName.Trim('m').Trim('μ'));
                if (num > 0 && num <= 0.35)
                {
                    sbMin.Append(ip.PollutantCode + ";");
                }
                else if (num <= 2)
                {
                    sbMid.Append(ip.PollutantCode + ";");
                }
                else
                {
                    sbMax.Append(ip.PollutantCode + ";");
                }
                //if (list1.Contains(ip.PollutantCode))
                //{
                //    sbMin.Append(ip.PollutantCode + ";");
                //}
                //else if (list2.Contains(ip.PollutantCode))
                //{
                //    sbMid.Append(ip.PollutantCode + ";");
                //}
                //else
                //{
                //    sbMax.Append(ip.PollutantCode + ";");
                //}
            }
            hdGroupFac.Value = string.Join(";", factors.Select(p => p.PollutantCode).ToArray()) + "|" + sbMin.ToString().Trim(';') + "|" + sbMid.ToString().Trim(';') + "|" + sbMax.ToString().Trim(';');
            hdGroupName.Value = "0-32μm|0-0.35μm|0.35μm-2μm|2μm-32μm";
            //hdIsLJPY.Value = "1";
        }
        /// <summary>
        /// 对离子色谱仪因子分类
        /// </summary>
        public void groupLZSPY()
        {
            factors = factorCbxRsm.GetFactors();
            StringBuilder sbLZ = new StringBuilder();   //离子因子
            StringBuilder sbQT = new StringBuilder();   //气体因子
            List<string> list = new List<string>();
            list.Add("a21024");
            list.Add("a51006");
            list.Add("a51005");
            list.Add("a21028");
            list.Add("a21001");
            foreach (IPollutant ip in factors)
            {
                //string num = ip.PollutantName.Substring(ip.PollutantName.Length - 1, 1);
                //if (num.Equals("⁺") || num.Equals("⁻"))
                //{
                //    sbLZ.Append(ip.PollutantCode + ";");
                //}
                //else
                //{
                //    sbQT.Append(ip.PollutantCode + ";");
                //}

                if (list.Contains(ip.PollutantCode))
                {
                    sbQT.Append(ip.PollutantCode + ";");
                }
                else
                {
                    sbLZ.Append(ip.PollutantCode + ";");
                }
            }
            hdGroupFac.Value = sbLZ.ToString().Trim(';') + "|" + sbQT.ToString().Trim(';');
            hdGroupName.Value = "阴阳离子|气体污染物";
        }
        /// <summary>
        /// 对其他因子分类
        /// </summary>
        public void groupOthers(string pageType)
        {
            if (pageType == "sbdljpy")
            {
                hdGroupFac.Value = string.Join(";", factors.Select(p => p.PollutantCode).ToArray());
                hdGroupName.Value = "散射系数";
            }
            else if (pageType == "tyfsy")
            {
                hdGroupFac.Value = string.Join(";", factors.Select(p => p.PollutantCode).ToArray());
                hdGroupName.Value = "太阳辐射";
            }
            else if (pageType == "htfxy")
            {
                hdGroupFac.Value = string.Join(";", factors.Select(p => p.PollutantCode).ToArray());
                hdGroupName.Value = "黑碳气溶胶";
            }
            else if (pageType == "yjtystfxy")
            {
                hdGroupFac.Value = string.Join(";", factors.Select(p => p.PollutantCode).ToArray());
                hdGroupName.Value = "EC/OC";
            }
            else if (pageType == "yssh")
            {
                hdGroupFac.Value = "";
                hdGroupName.Value = "";
            }
        }

        #region 绑定因子
        public void BindFactors(string CategoryUid, out string Name, out string code, string type = "name")
        {
            IQueryable<PollutantCodeEntity> Pollutant = m_AirPollutantService.RetrieveList().Where(x => x.CategoryUid == CategoryUid);
            string PollutantName = "";
            string PollutantCode = "";
            //if (type == "name")
            //{
            string[] pollutantarry = Pollutant.Select(p => p.PollutantName).ToArray();
            foreach (string strName in pollutantarry)
            {
                PollutantName += strName + ";";
            }
            //}
            //else
            //{
            string[] pollutantCodearry = Pollutant.Select(p => p.PollutantCode).ToArray();
            foreach (string strName in pollutantCodearry)
            {
                PollutantCode += strName + ";";
            }
            //}
            Name = PollutantName;
            code = PollutantCode;
        }
        #endregion
        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {
            //if (!IsPostBack)
            //{
            //    pointCbxRsm_SelectedChanged();
            //}
            try
            {
                BindFactors("979f05bb-f730-4285-8aee-fafdce1360e2", out LZSfactorName, out LZSPfactor);

                string[] factorCodes = null;

                DateTime dtBegion = DateTime.Now;
                DateTime dtEnd = DateTime.Now;
                points = pointCbxRsm.GetPoints();
                string[] portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
                if (PageHelper.GetQueryString("type") == "tyfsy")
                {
                    factors = factorCbxRsm.GetFactors();
                    factors = RemoveO8Code(factors);
                    if (factors.Select(p => p.PollutantCode).ToArray().Contains("a90162"))
                    {
                        IPollutant oldO3 = m_AirPollutantService.GetPollutantInfo("a90162");
                        //factors.Remove(oldO3);
                        factors.RemoveAt(factors.Count - 1);
                        //factors.remo
                        IPollutant newO3 = m_AirPollutantService.GetPollutantInfo("a05024");
                        factors.Add(newO3);
                        factorCodes = factors.Select(p => p.PollutantCode).ToArray();

                    }
                    else
                    {
                        factorCodes = factors.Select(p => p.PollutantCode).ToArray();

                    }
                }
                else
                {
                    factors = factorCbxRsm.GetFactors();
                    factors = RemoveO8Code(factors);
                    factorCodes = factorCbxRsm.GetFactorValues(SmartEP.Core.Enums.CbxRsmReturnType.Code);
                    factorCodes = RemoveO8CodeReturnArray(factorCodes);

                }


                //生成RadGrid的绑定列
                dvStatistical = null;

                #region 是否显示统计行
                if (IsStatistical.Checked)
                {
                    if (portIds != null)
                    {
                        if (radlDataType.SelectedValue == "Hour")
                        {
                            dtBegion = hourBegin.SelectedDate.Value;
                            dtEnd = hourEnd.SelectedDate.Value;
                            gridAudit.ShowFooter = true;
                            dvStatistical = m_HourData.GetHourStatisticalData(portIds, factors, dtBegion, dtEnd);
                        }
                        else if (radlDataType.SelectedValue == "Day")
                        {
                            dtBegion = dayBegin.SelectedDate.Value;
                            dtEnd = dayEnd.SelectedDate.Value;
                            gridAudit.ShowFooter = true;
                            dvStatistical = m_DayData.GetDayStatisticalData(portIds, factors, dtBegion, dtEnd);
                        }
                        else if (radlDataType.SelectedValue == "Month")
                        {
                            int monthB = monthBegin.SelectedDate.Value.Year;
                            int monthE = monthEnd.SelectedDate.Value.Year;
                            int monthF = monthBegin.SelectedDate.Value.Month;
                            int monthT = monthEnd.SelectedDate.Value.Month;
                            gridAudit.ShowFooter = true;
                            dvStatistical = m_MonthData.GetMonthStatisticalData(portIds, factors, monthB, monthF, monthE, monthT);
                        }
                        else if (radlDataType.SelectedValue == "Season")
                        {
                            int seasonB = seasonBegin.SelectedDate.Value.Year;
                            int seasonE = seasonEnd.SelectedDate.Value.Year;
                            int seasonF = Convert.ToInt32(seasonFrom.SelectedValue);
                            int seasonT = Convert.ToInt32(seasonTo.SelectedValue);
                            gridAudit.ShowFooter = true;
                            dvStatistical = m_SeasonData.GetSeasonStatisticalData(portIds, factors, seasonB, seasonF, seasonE, seasonT);
                        }
                        else if (radlDataType.SelectedValue == "Year")
                        {
                            int yearB = yearBegin.SelectedDate.Value.Year;
                            int yearE = yearEnd.SelectedDate.Value.Year;
                            gridAudit.ShowFooter = true;
                            dvStatistical = m_YearData.GetYearStatisticalData(portIds, factors, yearB, yearE);
                        }
                        else if (radlDataType.SelectedValue == "Week")
                        {
                            int weekB = weekBegin.SelectedDate.Value.Year;
                            int weekE = weekEnd.SelectedDate.Value.Year;
                            int newyear = DateTime.ParseExact(weekFrom.SelectedValue, "yyyy-MM-dd", null).AddDays(6).Year;
                            int nyear = DateTime.ParseExact(weekTo.SelectedValue, "yyyy-MM-dd", null).Year;
                            int weekF = 0;
                            int weekT = 0;
                            if (newyear > weekB)
                            {
                                weekF = ChinaDate.WeekOfYear(DateTime.ParseExact(weekFrom.SelectedValue, "yyyy-MM-dd", null));
                            }
                            else
                                weekF = ChinaDate.WeekOfYear(DateTime.ParseExact(weekFrom.SelectedValue, "yyyy-MM-dd", null).AddDays(6));

                            if (weekE > nyear)
                            {
                                weekT = ChinaDate.WeekOfYear(DateTime.ParseExact(weekTo.SelectedValue, "yyyy-MM-dd", null).AddDays(6));
                            }
                            else
                                weekT = ChinaDate.WeekOfYear(DateTime.ParseExact(weekTo.SelectedValue, "yyyy-MM-dd", null));

                            gridAudit.ShowFooter = true;
                            dvStatistical = m_WeekData.GetWeekStatisticalData(portIds, factors, weekB, weekF, weekE, weekT);
                        }
                    }
                    else
                    {
                        dvStatistical = new DataView();
                    }

                }
                else
                {
                    gridAudit.ShowFooter = false;
                }
                #endregion

                //每页显示数据个数            
                int pageSize = gridAudit.PageSize;
                //当前页的序号
                int pageNo = gridAudit.CurrentPageIndex;
                string pageType = PageHelper.GetQueryString("type");
                //数据总行数
                int recordTotal = 0;

                SetHiddenRegionData(portIds);
                if (portIds != null)
                {
                    //审核数据
                    if (ddlDataSource.SelectedIndex == 1)
                    {
                        if (pageType == "yssh")
                        {
                            this.ViewState["Files"] = "审核";
                            if (radlDataType.SelectedValue == "Hour")
                            {
                                string[] factorCodeSelected = factorCbxRsm.GetFactorValues(SmartEP.Core.Enums.CbxRsmReturnType.Code);
                                if (factorCodeSelected.Contains("a05027"))
                                {
                                    //factors = AddO8Code(factors);
                                    //factorCodes = AddO8CodeReturnArray(factorCodes);
                                    factors = factorCbxRsm.GetFactors();
                                    factorCodes = factorCodeSelected;
                                }
                            }
                        }
                        if (radlDataType.SelectedValue == "Hour" && pageType == "yssh")
                        {
                            dtBegion = Convert.ToDateTime(hourBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:00:00"));
                            dtEnd = Convert.ToDateTime(hourEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:59:59"));
                            //【给隐藏域赋值，用于显示Chart】
                            if (rbtnlType.SelectedValue == "CityProper")
                            {
                                SetHiddenData(portIds, factors, dtBegion, dtEnd);
                                groupOthers(pageType);
                                string orderBy = "sortNumber desc,Tstamp desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderBy = "sortNumber desc,Tstamp asc";
                                var auditData = m_HourData.GetNewHourDataPagerWidthRegionO8(portIds, factors.Select(p => p.PollutantCode).ToArray(), dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy);

                                gridAudit.DataSource = auditData;

                                //热力图所需数据
                                string staT = hourBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                                string endT = hourEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                                string pointidorregionguid = string.Join(",", portIds);
                                string factorCode = string.Join(",", factors.Select(p => p.PollutantCode).ToArray());
                                hdstaT.Value = staT;
                                hdendT.Value = endT;
                                hdPoint.Value = pointidorregionguid;
                                hdFactor.Value = factorCode;
                                hdFlag.Value = "Hour";
                            }
                            else
                            {
                                SetHiddenData(portIds, factors, dtBegion, dtEnd);
                                groupOthers(pageType);
                                string orderBy = "PointId asc,Tstamp desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderBy = "PointId asc,Tstamp asc";
                                var auditData = m_HourData.GetNewHourDataPagerWidthO8(portIds, factors.Select(p => p.PollutantCode).ToArray(), dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy);

                                gridAudit.DataSource = auditData;

                                //热力图所需数据
                                string staT = hourBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                                string endT = hourEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                                string pointidorregionguid = string.Join(",", portIds);
                                string factorCode = string.Join(",", factors.Select(p => p.PollutantCode).ToArray());
                                hdstaT.Value = staT;
                                hdendT.Value = endT;
                                hdPoint.Value = pointidorregionguid;
                                hdFactor.Value = factorCode;
                                hdFlag.Value = "Hour";
                            }
                        }
                        //小时数据
                        if (radlDataType.SelectedValue == "Hour" && pageType != "yssh")
                        {
                            dtBegion = Convert.ToDateTime(hourBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:00:00"));
                            dtEnd = Convert.ToDateTime(hourEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:59:59"));
                            //【给隐藏域赋值，用于显示Chart】
                            SetHiddenData(portIds, factors, dtBegion, dtEnd);
                            if (pageType == "ljpy")
                            {
                                groupLJPY();
                            }
                            else if (pageType == "lzspy")
                            {
                                groupLZSPY();
                            }
                            else
                            {
                                groupOthers(pageType);
                            }
                            string orderBy = "PointId asc,Tstamp desc";
                            if (TimeSort.SelectedValue == "时间升序")
                                orderBy = "PointId asc,Tstamp asc";
                            var auditData = m_HourData.GetNewHourDataPager(portIds, factors.Select(p => p.PollutantCode).ToArray(), dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy);

                            gridAudit.DataSource = auditData;

                            //热力图所需数据
                            string staT = hourBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                            string endT = hourEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                            string pointidorregionguid = string.Join(",", portIds);
                            string factorCode = string.Join(",", factors.Select(p => p.PollutantCode).ToArray());
                            hdstaT.Value = staT;
                            hdendT.Value = endT;
                            hdPoint.Value = pointidorregionguid;
                            hdFactor.Value = factorCode;
                            hdFlag.Value = "Hour";
                        }
                        //日数据
                        else if (radlDataType.SelectedValue == "Day")
                        {
                            dtBegion = dayBegin.SelectedDate.Value;
                            dtEnd = dayEnd.SelectedDate.Value;
                            var auditData = new DataView();
                            //【给隐藏域赋值，用于显示Chart】
                            if (pageType == "yssh" && rbtnlType.SelectedValue == "CityProper")
                            {
                                SetHiddenData(portIds, factors, dtBegion, dtEnd);
                                if (pageType == "ljpy")
                                {
                                    groupLJPY();
                                }
                                else if (pageType == "lzspy")
                                {
                                    groupLZSPY();
                                }
                                else
                                {
                                    groupOthers(pageType);
                                }
                                string orderBy = "sortNumber desc,DateTime desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderBy = "sortNumber desc,DateTime asc";
                                auditData = m_DayData.GetDayDataRegionPager(portIds, factors, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy);

                            }
                            else
                            {
                                SetHiddenData(portIds, factors, dtBegion, dtEnd);
                                if (pageType == "ljpy")
                                {
                                    groupLJPY();
                                }
                                else if (pageType == "lzspy")
                                {
                                    groupLZSPY();
                                }
                                else
                                {
                                    groupOthers(pageType);
                                }
                                string orderBy = "PointId asc,DateTime desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderBy = "PointId asc,DateTime asc";
                                auditData = m_DayData.GetDayDataPager(portIds, factors, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
                            }
                            gridAudit.DataSource = auditData;

                            //热力图所需数据
                            string staT = dayBegin.SelectedDate.Value.ToString("yyyy-MM-dd 00:00:00");
                            string endT = dayEnd.SelectedDate.Value.ToString("yyyy-MM-dd 23:59:59");
                            string pointidorregionguid = string.Join(",", portIds);
                            string factorCode = string.Join(",", factors.Select(p => p.PollutantCode).ToArray());
                            hdstaT.Value = staT;
                            hdendT.Value = endT;
                            hdPoint.Value = pointidorregionguid;
                            hdFactor.Value = factorCode;
                            hdFlag.Value = "Day";
                        }
                        //月数据
                        else if (radlDataType.SelectedValue == "Month")
                        {
                            int monthB = monthBegin.SelectedDate.Value.Year;
                            int monthE = monthEnd.SelectedDate.Value.Year;
                            int monthF = monthBegin.SelectedDate.Value.Month;
                            int monthT = monthEnd.SelectedDate.Value.Month;
                            if (pageType == "yssh" && rbtnlType.SelectedValue == "CityProper")
                            {
                                //【给隐藏域赋值，用于显示Chart】
                                SetHiddenData(portIds, factors, monthB + ";" + monthF + ";" + monthE + ";" + monthT);
                                if (pageType == "ljpy")
                                {
                                    groupLJPY();
                                }
                                else if (pageType == "lzspy")
                                {
                                    groupLZSPY();
                                }
                                else
                                {
                                    groupOthers(pageType);
                                }
                                string orderBy = "sortNumber desc,Year desc,MonthOfYear desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderBy = "sortNumber desc,Year asc,MonthOfYear asc";
                                var auditData = m_MonthData.GetDataPagerRegion(portIds, factors, monthB, monthF, monthE, monthT, pageSize, pageNo, out recordTotal, orderBy);
                                gridAudit.DataSource = auditData;

                                //热力图数据
                                hdFlag.Value = "";
                            }
                            else
                            {
                                //【给隐藏域赋值，用于显示Chart】
                                SetHiddenData(portIds, factors, monthB + ";" + monthF + ";" + monthE + ";" + monthT);
                                if (pageType == "ljpy")
                                {
                                    groupLJPY();
                                }
                                else if (pageType == "lzspy")
                                {
                                    groupLZSPY();
                                }
                                else
                                {
                                    groupOthers(pageType);
                                }
                                string orderBy = "PointId asc,Year desc,MonthOfYear desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderBy = "PointId asc,Year asc,MonthOfYear asc";
                                var auditData = m_MonthData.GetMonthDataPager(portIds, factors, monthB, monthF, monthE, monthT, pageSize, pageNo, out recordTotal, orderBy);
                                gridAudit.DataSource = auditData;

                                //热力图数据
                                hdFlag.Value = "";
                            }
                        }
                        //季数据
                        else if (radlDataType.SelectedValue == "Season")
                        {
                            int seasonB = seasonBegin.SelectedDate.Value.Year;
                            int seasonE = seasonEnd.SelectedDate.Value.Year;
                            int seasonF = Convert.ToInt32(seasonFrom.SelectedValue);
                            int seasonT = Convert.ToInt32(seasonTo.SelectedValue);

                            if (pageType == "yssh" && rbtnlType.SelectedValue == "CityProper")
                            {
                                //【给隐藏域赋值，用于显示Chart】
                                SetHiddenData(portIds, factors, seasonB + ";" + seasonF + ";" + seasonE + ";" + seasonT);
                                if (pageType == "ljpy")
                                {
                                    groupLJPY();
                                }
                                else if (pageType == "lzspy")
                                {
                                    groupLZSPY();
                                }
                                else
                                {
                                    groupOthers(pageType);
                                }
                                string orderBy = "sortNumber desc,Year desc,SeasonOfYear desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderBy = "sortNumber desc,Year asc,SeasonOfYear asc";
                                var auditData = m_SeasonData.GetSeasonDataPagerRegion(portIds, factors, seasonB, seasonF, seasonE, seasonT, pageSize, pageNo, out recordTotal, orderBy);
                                gridAudit.DataSource = auditData;

                                //热力图数据
                                hdFlag.Value = "";
                            }
                            else
                            {
                                //【给隐藏域赋值，用于显示Chart】
                                SetHiddenData(portIds, factors, seasonB + ";" + seasonF + ";" + seasonE + ";" + seasonT);
                                if (pageType == "ljpy")
                                {
                                    groupLJPY();
                                }
                                else if (pageType == "lzspy")
                                {
                                    groupLZSPY();
                                }
                                else
                                {
                                    groupOthers(pageType);
                                }
                                string orderBy = "PointId asc,Year desc,SeasonOfYear desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderBy = "PointId asc,Year asc,SeasonOfYear asc";
                                var auditData = m_SeasonData.GetSeasonDataPager(portIds, factors, seasonB, seasonF, seasonE, seasonT, pageSize, pageNo, out recordTotal, orderBy);
                                gridAudit.DataSource = auditData;

                                //热力图数据
                                hdFlag.Value = "";
                            }
                        }
                        //年数据
                        else if (radlDataType.SelectedValue == "Year")
                        {

                            int yearB = yearBegin.SelectedDate.Value.Year;
                            int yearE = yearEnd.SelectedDate.Value.Year;
                            if (pageType == "yssh" && rbtnlType.SelectedValue == "CityProper")
                            {
                                //【给隐藏域赋值，用于显示Chart】
                                SetHiddenData(portIds, factors, yearB + ";" + yearE);
                                if (pageType == "ljpy")
                                {
                                    groupLJPY();
                                }
                                else if (pageType == "lzspy")
                                {
                                    groupLZSPY();
                                }
                                else
                                {
                                    groupOthers(pageType);
                                }
                                string orderBy = "sortNumber desc,Year desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderBy = "sortNumber desc,Year asc";
                                var auditData = m_YearData.GetYearDataPagerRegion(portIds, factors, yearB, yearE, pageSize, pageNo, out recordTotal, orderBy);
                                gridAudit.DataSource = auditData;

                                //热力图数据
                                hdFlag.Value = "";
                            }
                            else
                            {
                                //【给隐藏域赋值，用于显示Chart】
                                SetHiddenData(portIds, factors, yearB + ";" + yearE);
                                if (pageType == "ljpy")
                                {
                                    groupLJPY();
                                }
                                else if (pageType == "lzspy")
                                {
                                    groupLZSPY();
                                }
                                else
                                {
                                    groupOthers(pageType);
                                }
                                string orderBy = "PointId asc,Year desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderBy = "PointId asc,Year asc";
                                var auditData = m_YearData.GetYearDataPager(portIds, factors, yearB, yearE, pageSize, pageNo, out recordTotal, orderBy);
                                gridAudit.DataSource = auditData;

                                //热力图数据
                                hdFlag.Value = "";
                            }
                        }
                        //周数据
                        else if (radlDataType.SelectedValue == "Week")
                        {
                            int weekB = weekBegin.SelectedDate.Value.Year;
                            int weekE = weekEnd.SelectedDate.Value.Year;
                            int newyear = DateTime.ParseExact(weekFrom.SelectedValue, "yyyy-MM-dd", null).AddDays(6).Year;
                            int nyear = DateTime.ParseExact(weekTo.SelectedValue, "yyyy-MM-dd", null).Year;
                            int weekF = 0;
                            int weekT = 0;
                            if (newyear > weekB)
                            {
                                weekF = ChinaDate.WeekOfYear(DateTime.ParseExact(weekFrom.SelectedValue, "yyyy-MM-dd", null));
                            }
                            else
                                weekF = ChinaDate.WeekOfYear(DateTime.ParseExact(weekFrom.SelectedValue, "yyyy-MM-dd", null).AddDays(6));

                            if (weekE > nyear)
                            {
                                weekT = ChinaDate.WeekOfYear(DateTime.ParseExact(weekTo.SelectedValue, "yyyy-MM-dd", null).AddDays(6));
                            }
                            else
                                weekT = ChinaDate.WeekOfYear(DateTime.ParseExact(weekTo.SelectedValue, "yyyy-MM-dd", null));
                            var auditData = new DataView();
                            if (pageType == "yssh" && rbtnlType.SelectedValue == "CityProper")
                            {
                                SetHiddenData(portIds, factors, weekB + ";" + weekF + ";" + weekE + ";" + weekT);
                                if (pageType == "ljpy")
                                {
                                    groupLJPY();
                                }
                                else if (pageType == "lzspy")
                                {
                                    groupLZSPY();
                                }
                                else
                                {
                                    groupOthers(pageType);
                                }
                                string orderBy = "sortNumber desc,Year desc,WeekOfYear desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderBy = "sortNumber desc,Year asc,WeekOfYear asc";
                                auditData = m_WeekData.GetDataPagerRegion(portIds, factors, weekB, weekF, weekE, weekT, pageSize, pageNo, out recordTotal, orderBy);
                                gridAudit.DataSource = auditData;

                                //热力图数据
                                hdFlag.Value = "";
                            }
                            else
                            {
                                //【给隐藏域赋值，用于显示Chart】
                                SetHiddenData(portIds, factors, weekB + ";" + weekF + ";" + weekE + ";" + weekT);
                                if (pageType == "ljpy")
                                {
                                    groupLJPY();
                                }
                                else if (pageType == "lzspy")
                                {
                                    groupLZSPY();
                                }
                                else
                                {
                                    groupOthers(pageType);
                                }
                                string orderBy = "PointId asc,Year desc,WeekOfYear desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderBy = "PointId asc,Year asc,WeekOfYear asc";
                                auditData = m_WeekData.GetWeekDataPager(portIds, factors, weekB, weekF, weekE, weekT, pageSize, pageNo, out recordTotal, orderBy);
                                gridAudit.DataSource = auditData;

                                //热力图数据
                                hdFlag.Value = "";
                            }
                        }
                    }
                    //原始数据
                    else if (ddlDataSource.SelectedIndex == 0)
                    {
                        if (pageType == "yssh")
                        {
                            this.ViewState["Files"] = "原始";
                            if (radlDataTypeOri.SelectedValue == "Min60" || radlDataTypeOri.SelectedValue == "OriDay")
                            {
                                string[] factorCodeSelected = factorCbxRsm.GetFactorValues(SmartEP.Core.Enums.CbxRsmReturnType.Code);
                                if (factorCodeSelected.Contains("a05027"))
                                {
                                    //factors = AddO8Code(factors);
                                    //factorCodes = AddO8CodeReturnArray(factorCodes);
                                    factors = factorCbxRsm.GetFactors();
                                    factorCodes = factorCodeSelected;
                                }
                            }
                        }
                        if (radlDataTypeOri.SelectedValue == "Min1")
                        {
                            string orderBy = "PointId asc,Tstamp desc";
                            dtBegion = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:00"));
                            dtEnd = Convert.ToDateTime(dtpEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:00"));
                            if (TimeSort.SelectedValue == "时间升序")
                                orderBy = "PointId asc,Tstamp asc";
                            var auditData = m_Min1Data.GetDataPager(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy);//原始一分钟数据查询
                            gridAudit.DataSource = auditData;
                            //【给隐藏域赋值，用于显示Chart】
                            SetHiddenData(portIds, factors, dtBegion, dtEnd);
                            if (pageType == "ljpy")
                            {
                                groupLJPY();
                            }
                            else if (pageType == "lzspy")
                            {
                                groupLZSPY();
                            }
                            else
                            {
                                groupOthers(pageType);
                            }
                        }
                        //原始数据五分钟数据
                        else if (radlDataTypeOri.SelectedValue == "Min5")
                        {
                            string orderBy = "PointId asc,Tstamp desc";
                            dtBegion = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:00"));
                            dtEnd = Convert.ToDateTime(dtpEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:00"));
                            if (TimeSort.SelectedValue == "时间升序")
                                orderBy = "PointId asc,Tstamp asc";
                            var auditData = new DataView();
                            if (pageType == "tyfsy")
                            {
                                auditData = m_Min5Data.GetDataPagersWithO3(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy);//原始日数据类型按 原始日数据查询
                                auditData.Sort = orderBy;
                            }
                            else
                            {
                                auditData = m_Min5Data.GetDataPager(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy);//原始五分钟数据查询
                            }
                            int index = 0;
                            if (factorCodes.Contains("a05024") || factorCodes.Contains("a05040") || factorCodes.Contains("a05041"))
                            {
                                for (int i = 0; i < factorCodes.Length; i++)
                                {
                                    if (factorCodes[i].Equals("a05024") || factorCodes[i].Equals("a05040") || factorCodes[i].Equals("a05041"))
                                    {
                                        index = i;
                                    }
                                }
                                auditData.Table.Columns[factorCodes[index]].SetOrdinal(index + 2);
                            }
                            gridAudit.DataSource = auditData;
                            //【给隐藏域赋值，用于显示Chart】
                            SetHiddenData(portIds, factors, dtBegion, dtEnd);
                            if (pageType == "ljpy")
                            {
                                groupLJPY();
                            }
                            else if (pageType == "lzspy")
                            {
                                groupLZSPY();
                            }
                            else
                            {
                                groupOthers(pageType);
                            }
                        }
                        //原始审核页面原始小时数据的查询
                        else if (radlDataTypeOri.SelectedValue == "Min60" && pageType == "yssh")
                        {
                            //gridAudit.AllowCustomPaging = false;
                            string orderBy = "PointId asc,Tstamp desc";
                            dtBegion = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:00:00"));
                            dtEnd = Convert.ToDateTime(dtpEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:59:59"));
                            if (rbtnlType.SelectedValue == "CityProper")//区域
                            {
                                orderBy = "sortNumber desc,Tstamp desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderBy = "sortNumber desc,Tstamp asc";
                                //orderBy = "time.tstamp ";
                                DataView auditData = null;
                                //NT O3早一小时做处理
                                if (isSuper == "1")
                                {
                                    //auditData = m_Min60Data.GetDataPagerForO3(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy);//原始小时数据查询
                                    if (pageType == "lzspy")//离子色谱仪数据晚两个小时
                                    {
                                        auditData = m_Min60Data.GetDataPagerForO3AllTimeLZSPY(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy);//原始小时数据查询

                                    }
                                    else
                                    {
                                        //除离子色谱仪之外的其他仪器数据绑定
                                        auditData = m_Min60Data.GetDataPagerForO3AllTime(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy);//原始小时数据查询
                                    }

                                }
                                else
                                {
                                    //常规站
                                    dtBegion = Convert.ToDateTime(dtBegion.ToString("yyyy-MM-dd HH:59:59"));
                                    //auditData = m_Min60Data.GetDataPagerAllTimeWithO8(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy);//原始小时数据查询
                                    auditData = m_Min60Data.GetDataPagerAllTimeWithO8Region(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy);//原始小时数据查询
                                }
                                gridAudit.DataSource = auditData;
                                //【给隐藏域赋值，用于显示Chart】
                                SetHiddenData(portIds, factors, dtBegion, dtEnd);
                                groupOthers(pageType);
                            }
                            else//站点
                            {
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderBy = "PointId asc,Tstamp asc";
                                //orderBy = "time.tstamp ";
                                DataView auditData = null;
                                //NT O3早一小时做处理
                                if (isSuper == "1")
                                {
                                    //auditData = m_Min60Data.GetDataPagerForO3(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy);//原始小时数据查询
                                    if (pageType == "lzspy")//离子色谱仪数据晚两个小时
                                    {
                                        auditData = m_Min60Data.GetDataPagerForO3AllTimeLZSPY(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy);//原始小时数据查询

                                    }
                                    else
                                    {
                                        //除离子色谱仪之外的其他仪器数据绑定
                                        auditData = m_Min60Data.GetDataPagerForO3AllTime(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy);//原始小时数据查询
                                    }

                                }
                                else
                                {
                                    //常规站
                                    dtBegion = Convert.ToDateTime(dtBegion.ToString("yyyy-MM-dd HH:59:59"));
                                    auditData = m_Min60Data.GetDataPagerAllTimeWithO8(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy);//原始小时数据查询
                                }
                                gridAudit.DataSource = auditData;
                                //【给隐藏域赋值，用于显示Chart】
                                SetHiddenData(portIds, factors, dtBegion, dtEnd);
                                groupOthers(pageType);
                            }
                        }
                        //超级站原始数据小时数据
                        else if (radlDataTypeOri.SelectedValue == "Min60" && pageType != "yssh")
                        {
                            //gridAudit.AllowCustomPaging = false;
                            //string orderBy = "time.tstamp desc";

                            string orderBy = "PointId asc,Tstamp desc";
                            dtBegion = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:00:00"));
                            dtEnd = Convert.ToDateTime(dtpEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:59:59"));
                            if (TimeSort.SelectedValue == "时间升序")
                                orderBy = "PointId asc,Tstamp asc";
                            //orderBy = "time.tstamp ";
                            DataView auditData = null;
                            //NT O3早一小时做处理
                            if (isSuper == "1")
                            {
                                //auditData = m_Min60Data.GetDataPagerForO3(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy);//原始小时数据查询
                                if (pageType == "lzspy")//离子色谱仪数据晚两个小时
                                {
                                    auditData = m_Min60Data.GetDataPagerForO3AllTimeLZSPY(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy);//原始小时数据查询

                                }
                                else
                                {
                                    //除离子色谱仪之外的其他仪器数据绑定
                                    auditData = m_Min60Data.GetDataPagerForO3AllTime(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy);//原始小时数据查询
                                }

                            }
                            else
                            {
                                //常规站
                                dtBegion = Convert.ToDateTime(dtBegion.ToString("yyyy-MM-dd HH:59:59"));
                                auditData = m_Min60Data.GetDataPagerAllTime(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy);//原始小时数据查询
                            }
                            gridAudit.DataSource = auditData;
                            //【给隐藏域赋值，用于显示Chart】
                            SetHiddenData(portIds, factors, dtBegion, dtEnd);
                            if (pageType == "ljpy")
                            {
                                groupLJPY();
                            }
                            else if (pageType == "lzspy")
                            {
                                groupLZSPY();
                            }
                            else
                            {
                                groupOthers(pageType);
                            }
                        }
                        //原始数据日数据
                        else if (radlDataTypeOri.SelectedValue == "OriDay")
                        {
                            if (pageType == "yssh" && rbtnlType.SelectedValue == "CityProper")
                            {
                                string orderBy = "sortNumber desc,DateTime desc";
                                dtBegion = dtpDayBegin.SelectedDate.Value;
                                dtEnd = dtpDayEnd.SelectedDate.Value;
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderBy = "sortNumber desc,DateTime asc";
                                var auditData = new DataView();
                                if (pageType == "tyfsy")
                                {
                                    auditData = m_DayOriData.GetDataPagersWithMax(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy);//原始日数据类型按 原始日数据查询
                                }
                                else
                                {
                                    //auditData = m_DayOriData.GetDataPagerForAllTime(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
                                    auditData = m_DayOriData.GetDataPagerForAllTimeRegion(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
                                    //auditData = m_DayOriData.GetDataPagers(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy);//原始日数据类型按 原始日数据查询
                                }
                                int index = 0;
                                if (factorCodes.Contains("a05024") || factorCodes.Contains("a05040") || factorCodes.Contains("a05041"))
                                {
                                    for (int i = 0; i < factorCodes.Length; i++)
                                    {
                                        if (factorCodes[i].Equals("a05024") || factorCodes[i].Equals("a05040") || factorCodes[i].Equals("a05041"))
                                        {
                                            index = i;
                                        }
                                    }
                                    auditData.Table.Columns[factorCodes[index]].SetOrdinal(index + 3);
                                }
                                gridAudit.DataSource = auditData;
                                //【给隐藏域赋值，用于显示Chart】
                                SetHiddenData(portIds, factors, dtBegion, dtEnd);
                                if (pageType == "ljpy")
                                {
                                    groupLJPY();
                                }
                                else if (pageType == "lzspy")
                                {
                                    groupLZSPY();
                                }
                                else
                                {
                                    groupOthers(pageType);
                                }
                            }
                            else
                            {
                                string orderBy = "PointId asc,DateTime desc";
                                dtBegion = dtpDayBegin.SelectedDate.Value;
                                dtEnd = dtpDayEnd.SelectedDate.Value;
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderBy = "PointId asc,DateTime asc";
                                var auditData = new DataView();
                                if (pageType == "tyfsy")
                                {
                                    auditData = m_DayOriData.GetDataPagersWithMax(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy);//原始日数据类型按 原始日数据查询
                                }
                                else
                                {
                                    auditData = m_DayOriData.GetDataPagerForNTO3AllTime(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
                                    //auditData = m_DayOriData.GetDataPagers(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy);//原始日数据类型按 原始日数据查询
                                }
                                int index = 0;
                                if (factorCodes.Contains("a05024") || factorCodes.Contains("a05040") || factorCodes.Contains("a05041"))
                                {
                                    for (int i = 0; i < factorCodes.Length; i++)
                                    {
                                        if (factorCodes[i].Equals("a05024") || factorCodes[i].Equals("a05040") || factorCodes[i].Equals("a05041"))
                                        {
                                            index = i;
                                        }
                                    }
                                    auditData.Table.Columns[factorCodes[index]].SetOrdinal(index + 3);
                                }
                                gridAudit.DataSource = auditData;
                                //【给隐藏域赋值，用于显示Chart】
                                SetHiddenData(portIds, factors, dtBegion, dtEnd);
                                if (pageType == "ljpy")
                                {
                                    groupLJPY();
                                }
                                else if (pageType == "lzspy")
                                {
                                    groupLZSPY();
                                }
                                else
                                {
                                    groupOthers(pageType);
                                }
                            }
                        }
                        //原始数据月数据
                        else if (radlDataTypeOri.SelectedValue == "OriMonth")
                        {
                            if (pageType == "yssh" && rbtnlType.SelectedValue == "CityProper")
                            {
                                string orderBy = "sortNumber desc,Year desc,MonthOfYear desc";
                                dtBegion = dtpMonthBegin.SelectedDate.Value;
                                dtEnd = dtpMonthEnd.SelectedDate.Value;
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderBy = "sortNumber desc,Year asc,MonthOfYear asc";

                                var auditData = m_MonthOriData.GetOriDataPagerRegion(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy);//原始月数据类型按 原始月数据查询
                                gridAudit.DataSource = auditData;
                                //【给隐藏域赋值，用于显示Chart】
                                SetHiddenData(portIds, factors, dtBegion, dtEnd);
                                if (pageType == "ljpy")
                                {
                                    groupLJPY();
                                }
                                else if (pageType == "lzspy")
                                {
                                    groupLZSPY();
                                }
                                else
                                {
                                    groupOthers(pageType);
                                }
                            }
                            else
                            {
                                string orderBy = "PointId asc,Year desc,MonthOfYear desc";
                                dtBegion = dtpMonthBegin.SelectedDate.Value;
                                dtEnd = dtpMonthEnd.SelectedDate.Value;
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderBy = "PointId asc,Year asc,MonthOfYear asc";
                                var auditData = m_MonthOriData.GetOriDataPager(portIds, factorCodes, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, orderBy);//原始月数据类型按 原始月数据查询
                                gridAudit.DataSource = auditData;
                                //【给隐藏域赋值，用于显示Chart】
                                SetHiddenData(portIds, factors, dtBegion, dtEnd);
                                if (pageType == "ljpy")
                                {
                                    groupLJPY();
                                }
                                else if (pageType == "lzspy")
                                {
                                    groupLZSPY();
                                }
                                else
                                {
                                    groupOthers(pageType);
                                }
                            }
                        }
                    }
                }
                else
                {
                    gridAudit.DataSource = new DataTable();
                }
                //数据总行数
                gridAudit.VirtualItemCount = recordTotal;
                //国家数据标记位
                siteTypeEntites = dicService.RetrieveList(DictionaryType.AMS, "国家数据标记");
            }
            catch (Exception ex) { }
        }
        /// <summary>
        /// 给区域隐藏值赋值
        /// </summary>
        /// <param name="portIds"></param>
        private void SetHiddenRegionData(string[] portIds)
        {
            if (rbtnlType.SelectedValue == "CityProper")
            {
                HiddenDataRegionOrPoint.Value = "City";
            }
            else
            {
                HiddenDataRegionOrPoint.Value = "Point";
            }
            DataTable dtRegion = m_HourData.GetRegionWithPointId(portIds);
            List<string> listRegion = new List<string>();
            if (dtRegion != null && dtRegion.Rows.Count > 0)
            {
                for (int i = 0; i < dtRegion.Rows.Count; i++)
                {
                    listRegion.Add(dtRegion.Rows[i]["Region"].ToString());
                }
                HiddenDataRegionValue.Value = string.Join(";", listRegion.ToArray());
            }


        }
        /// <summary>
        /// 在因子数组中加上O3-8这个因子
        /// </summary>
        /// <param name="factorCodes"></param>
        /// <returns></returns>
        private string[] AddO8CodeReturnArray(string[] factorCodes)
        {
            try
            {
                List<string> factorList = new List<string>();
                if (factorCodes != null && factorCodes.Length > 0)
                {

                    factorList = factorCodes.ToList();
                    if (!factorList.Contains("a05027"))
                    {
                        factorList.Add("a05027");
                    }

                }
                return factorList.ToArray();
            }
            catch (Exception ex)
            {
                log.Error("---------------------------------AddO8CodeReturnArray方法异常");
                throw ex;
            }
        }
        /// <summary>
        /// 在因子集合中加上O3-8这个因子
        /// </summary>
        /// <param name="factors"></param>
        /// <returns></returns>
        private IList<IPollutant> AddO8Code(IList<IPollutant> factorList)
        {
            try
            {
                IPollutant O8 = m_AirPollutantService.GetPollutantInfo("a05027");
                if (factorList != null && factorList.Count > 0)
                {


                    factorList.Add(O8);


                }
                return factorList;
            }
            catch (Exception ex)
            {
                log.Error("---------------------------------AddO8Code方法异常");
                throw ex;
            }
        }
        /// <summary>
        /// 去除因子数组中的O3-8因子
        /// </summary>
        /// <param name="factorCodes"></param>
        /// <returns></returns>
        private string[] RemoveO8CodeReturnArray(string[] factorCodes)
        {
            try
            {
                List<string> factorList = new List<string>();
                if (factorCodes != null && factorCodes.Length > 0)
                {

                    factorList = factorCodes.ToList();
                    factorList.Remove("a05027");

                }
                return factorList.ToArray();
            }
            catch (Exception ex)
            {
                log.Error("---------------------------------RemoveO8CodeReturnArray方法异常");
                throw ex;
            }
        }
        /// <summary>
        /// 去除集合因子中的O3-8因子
        /// </summary>
        /// <param name="factorCodes">所有选中的因子</param>
        private IList<IPollutant> RemoveO8Code(IList<IPollutant> factorList)
        {
            try
            {

                if (factorList != null && factorList.Count > 0)
                {

                    foreach (IPollutant it in factorList)
                    {
                        if ("a05027".Equals(it.PollutantCode))
                        {
                            factorList.Remove(it);
                            break;
                        }
                    }

                }
                return factorList;
            }
            catch (Exception ex)
            {
                log.Error("---------------------------------RemoveO8Code方法异常");
                throw ex;
            }
        }

        /// <summary>
        /// 生成RadGrid的绑定列
        /// </summary>
        /// <param sender></param>
        /// <param e></param>
        protected void gridAudit_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            try
            {
                if (e.Column.ColumnType.Equals("GridExpandColumn"))
                    return;
                //追加站点
                GridBoundColumn col = (GridBoundColumn)e.Column;
                string a = col.DataField;
                if (col.DataField == "PointId")
                {
                    col.HeaderText = "站点";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterText = "平均值<br>最大值<br>最小值";
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                //追加日期
                else if (col.DataField == "Tstamp")
                {
                    col = (GridDateTimeColumn)e.Column;
                    string tstcolformat = "{0:MM-dd HH:mm}";
                    if ((SmartEP.Core.Enums.EnumMapping.ParseEnum<PollutantDataType>(radlDataType.SelectedValue) == PollutantDataType.Hour && ddlDataSource.SelectedValue == "AuditData") || (SmartEP.Core.Enums.EnumMapping.ParseEnum<PollutantDataType>(radlDataTypeOri.SelectedValue) == PollutantDataType.Min60 && ddlDataSource.SelectedValue == "OriData"))
                        tstcolformat = "{0:MM-dd HH:00}";
                    col.HeaderText = "日期";
                    col.DataFormatString = tstcolformat;
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(120);
                    col.ItemStyle.Width = Unit.Pixel(120);
                }
                else if (col.DataField == "DateTime")
                {
                    col = (GridDateTimeColumn)e.Column;
                    string tstcolformat = "{0:yyyy-MM-dd}";
                    if (SmartEP.Core.Enums.EnumMapping.ParseEnum<PollutantDataType>(radlDataType.SelectedValue) == PollutantDataType.Day)
                        tstcolformat = "{0:yyyy-MM-dd}";
                    col.HeaderText = "日期";
                    col.DataFormatString = tstcolformat;
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(120);
                    col.ItemStyle.Width = Unit.Pixel(120);
                }
                else if (col.DataField == "Year")
                {
                    col = (GridNumericColumn)e.Column;
                    col.HeaderText = "年份";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(120);
                    col.ItemStyle.Width = Unit.Pixel(120);
                }
                else if (col.DataField == "WeekOfYear")
                {
                    col = (GridNumericColumn)e.Column;
                    col.HeaderText = "周";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(120);
                    col.ItemStyle.Width = Unit.Pixel(120);
                }
                else if (col.DataField == "MonthOfYear")
                {
                    col = (GridNumericColumn)e.Column;
                    col.HeaderText = "月份";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(120);
                    col.ItemStyle.Width = Unit.Pixel(120);
                }
                else if (col.DataField == "SeasonOfYear")
                {
                    col = (GridNumericColumn)e.Column;
                    col.HeaderText = "季";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(120);
                    col.ItemStyle.Width = Unit.Pixel(120);
                }
                //追加因子
                else if (factors.Select(x => x.PollutantCode).Contains(col.DataField))
                {
                    //去除太阳辐射仪的最大紫外辐射和最大太阳辐射
                    if (radlDataTypeOri.SelectedValue != "OriDay")
                    {
                        if (col.DataField.Equals("a05041") || col.DataField.Equals("a05040"))
                        {
                            e.Column.Visible = false;
                        }
                    }

                    int radGridColWidthValue = int.Parse(radGridColWidth.Value.ToString());
                    IPollutant factor = factors.FirstOrDefault(x => x.PollutantCode.Equals(col.DataField));
                    string strName = factor.PollutantName;
                    if (strName == "PM2.5")
                    {
                        strName = "PM<sub>2.5</sub>";
                    }
                    if (strName == "PM10")
                    {
                        strName = "PM<sub>10</sub>";
                    }
                    string strUnit = factor.PollutantMeasureUnit;
                    //if (strUnit.Contains("m³"))
                    //{
                    //    strUnit = strUnit.Replace("m³", "m<sup>3</sup>");
                    //}
                    //if (strUnit == "mg/m3")
                    //{
                    //    strUnit = "mg/m<sup>3</sup>";
                    //}
                    //if (strUnit == "μg/m³")
                    //{
                    //    strUnit = "μg/m<sup>3</sup>";
                    //}
                    if (strUnit.Trim() == "")
                    {
                        col.HeaderText = string.Format("{0}<br>{1}", strName, strUnit);
                    }
                    else
                    {
                        col.HeaderText = string.Format("{0}<br>({1})", strName, strUnit);
                    }
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    //col.HeaderStyle.Width = Unit.Pixel(radGridColWidthValue);
                    //col.ItemStyle.Width = Unit.Pixel(radGridColWidthValue);
                    col.HeaderStyle.Width = Unit.Pixel(150);
                    col.ItemStyle.Width = Unit.Pixel(150);
                    SetGridFooterText(col);
                }
                else if (col.DataField == "blankspaceColumn") { }
                else
                {
                    e.Column.Visible = false;
                }

            }
            catch (Exception ex) { }
        }
        /// <summary>
        /// 平均大小值
        /// </summary>
        /// <param name="col"></param>
        public void SetGridFooterText(GridBoundColumn col)
        {
            //统计行
            if (dvStatistical != null)
            {
                string avg = string.Empty;
                string max = string.Empty;
                string min = string.Empty;
                dvStatistical.RowFilter = string.Format("PollutantCode='{0}'", col.DataField);
                if (dvStatistical.Count > 0)
                {
                    avg = dvStatistical[0]["Value_Avg"] != DBNull.Value ? dvStatistical[0]["Value_Avg"].ToString() : "--";
                    max = dvStatistical[0]["Value_Max"] != DBNull.Value ? dvStatistical[0]["Value_Max"].ToString() : "--";
                    min = dvStatistical[0]["Value_Min"] != DBNull.Value ? dvStatistical[0]["Value_Min"].ToString() : "--";
                    if (factors != null)
                    {
                        IPollutant factor = factors[0];
                        if (avg != "--")
                        {
                            decimal AVG = Convert.ToDecimal(avg);
                            avg = DecimalExtension.GetPollutantValue(AVG, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum)).ToString();
                        }
                        if (max != "--")
                        {
                            decimal MAX = Convert.ToDecimal(max);
                            max = DecimalExtension.GetPollutantValue(MAX, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum)).ToString();
                        }
                        if (min != "--")
                        {
                            decimal MIN = Convert.ToDecimal(min);
                            min = DecimalExtension.GetPollutantValue(MIN, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum)).ToString();
                        }
                    }

                }
                col.FooterText = string.Format("{0}<br>{1}<br>{2}", avg, max, min);
                col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
            }
        }

        /// <summary>
        /// 页面隐藏域控件赋值（小时、日），将数据需要的参数放入隐藏域，各个参数间用‘|’分割，每个参数内部用‘；’分割
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtBegin"></param>
        /// <param name="dtEnd"></param>
        private void SetHiddenData(string[] portIds, IList<IPollutant> factors, DateTime dtBegin, DateTime dtEnd)
        {
            string pageType = PageHelper.GetQueryString("type");
            if (pageType == "yssh")
            {
                if (ddlDataSource.SelectedValue == "OriData")
                {
                    if (rbtnlType.SelectedValue == "CityProper")
                    {
                        HiddenData.Value = string.Join(";", portIds) + "|" + string.Join(";", factors.Select(p => p.PollutantCode).ToArray())
                                         + "|" + dtBegin + "|" + dtEnd + "|" + radlDataTypeOri.SelectedValue + "City" + "|Air1";
                        HiddenChartType.Value = ChartType.SelectedValue;
                    }
                    else
                    {
                        HiddenData.Value = string.Join(";", portIds) + "|" + string.Join(";", factors.Select(p => p.PollutantCode).ToArray())
                                         + "|" + dtBegin + "|" + dtEnd + "|" + radlDataTypeOri.SelectedValue + "|Air1";
                        HiddenChartType.Value = ChartType.SelectedValue;
                    }
                }
                if (ddlDataSource.SelectedValue == "AuditData")
                {
                    if (rbtnlType.SelectedValue == "CityProper")
                    {
                        HiddenData.Value = string.Join(";", portIds) + "|" + string.Join(";", factors.Select(p => p.PollutantCode).ToArray())
                                        + "|" + dtBegin + "|" + dtEnd + "|" + radlDataType.SelectedValue + "City" + "|Air1";
                    }
                    else
                    {
                        HiddenData.Value = string.Join(";", portIds) + "|" + string.Join(";", factors.Select(p => p.PollutantCode).ToArray())
                                         + "|" + dtBegin + "|" + dtEnd + "|" + radlDataType.SelectedValue + "|Air1";
                    }
                    HiddenChartType.Value = ChartType.SelectedValue;
                }
            }
            else
            {
                if (ddlDataSource.SelectedValue == "OriData")
                {
                    HiddenData.Value = string.Join(";", portIds) + "|" + string.Join(";", factors.Select(p => p.PollutantCode).ToArray())
                                     + "|" + dtBegin + "|" + dtEnd + "|" + radlDataTypeOri.SelectedValue + "|Air";
                    HiddenChartType.Value = ChartType.SelectedValue;
                }
                if (ddlDataSource.SelectedValue == "AuditData")
                {
                    HiddenData.Value = string.Join(";", portIds) + "|" + string.Join(";", factors.Select(p => p.PollutantCode).ToArray())
                                     + "|" + dtBegin + "|" + dtEnd + "|" + radlDataType.SelectedValue + "|Air";
                    HiddenChartType.Value = ChartType.SelectedValue;
                }
            }

        }

        /// <summary>
        /// 页面隐藏域控件赋值,(周、月、季、年)
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="timeStr"></param>
        private void SetHiddenData(string[] portIds, IList<IPollutant> factors, string timeStr)
        {
            string pageType = PageHelper.GetQueryString("type");
            if (ddlDataSource.SelectedValue == "OriData")
            {
                if (pageType == "yssh" && rbtnlType.SelectedValue == "CityProper")
                {
                    HiddenData.Value = string.Join(";", portIds) + "|" + string.Join(";", factors.Select(p => p.PollutantCode).ToArray())
                                     + "|" + timeStr + "|" + "|" + radlDataTypeOri.SelectedValue + "City" + "|Air";
                    HiddenChartType.Value = ChartType.SelectedValue;
                }
                else
                {
                    HiddenData.Value = string.Join(";", portIds) + "|" + string.Join(";", factors.Select(p => p.PollutantCode).ToArray())
                                     + "|" + timeStr + "|" + "|" + radlDataTypeOri.SelectedValue + "|Air";
                    HiddenChartType.Value = ChartType.SelectedValue;
                }
            }
            if (ddlDataSource.SelectedValue == "AuditData")
            {

                if (pageType == "yssh" && rbtnlType.SelectedValue == "CityProper")
                {
                    HiddenData.Value = string.Join(";", portIds) + "|" + string.Join(";", factors.Select(p => p.PollutantCode).ToArray())
                                    + "|" + timeStr + "|" + "|" + radlDataType.SelectedValue + "City" + "|Air";
                    HiddenChartType.Value = ChartType.SelectedValue;
                }
                else
                {
                    HiddenData.Value = string.Join(";", portIds) + "|" + string.Join(";", factors.Select(p => p.PollutantCode).ToArray())
                                     + "|" + timeStr + "|" + "|" + radlDataType.SelectedValue + "|Air";
                    HiddenChartType.Value = ChartType.SelectedValue;
                }
            }
        }

        #endregion

        #region 绑定图表
        private void BindChart()
        {
            RegisterScript("InitGroupChart();");

            if (ShowType.Text.Equals("分屏"))
            {
                RegisterScript("InitGroupChart();");
            }
            else if (ShowType.Text.Equals("合并"))
            {
                RegisterScript("InitTogetherChart();");
            }
        }
        #endregion

        #region 绑定热力图
        private void BindHighChart()
        {

            RegisterScript("GetHighChart();");

        }
        #endregion

        #region 服务器端控件事件处理
        /// <summary>
        /// 绑定数据源（用于分页、排序）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridAudit_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }

        /// <summary>
        /// 数据行绑定处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridAudit_ItemDataBound(object sender, GridItemEventArgs e)
        {
            string DataTypeUid = "1b6367f1-5287-4c14-b120-7a35bd176db1";
            string[] portIds = pointCbxRsm.GetPointValues(CbxRsmReturnType.ID);
            string[] factorCodes = factorCbxRsm.GetFactorValues(SmartEP.Core.Enums.CbxRsmReturnType.Code);
            IQueryable<ExcessiveSettingInfo> Excessive = m_ExcessiveSettingService.RetrieveListByPointAndFactor(ApplicationValue.Air, factorCodes, portIds, DataTypeUid);
            DataTable dtExcessive = ConvertToDataTable(Excessive);
            GridDataItem item = e.Item as GridDataItem;
            DataRowView drv = e.Item.DataItem as DataRowView;
            string pageType = this.ViewState["type"].ToString();

            //原始审核数据查询
            if (pageType == "yssh")
            {
                if (e.Item is GridDataItem)
                {



                    if (rbtnlType.SelectedValue == "Port")
                    {
                        if (item["PointId"] != null)
                        {
                            GridTableCell pointCell = (GridTableCell)item["PointId"];
                            IPoint point = points.FirstOrDefault(x => x.PointID.Equals(pointCell.Text.Trim()));
                            if (points != null)
                                pointCell.Text = point.PointName;
                        }
                    }
                    if ((radlDataTypeOri.SelectedValue == "Min1" || radlDataTypeOri.SelectedValue == "Min60" || radlDataTypeOri.SelectedValue == "Min5") && ddlDataSource.SelectedValue == "OriData")
                    {
                        for (int iRow = 0; iRow < factors.Count; iRow++)
                        {
                            string siteTypeName = "--";//标记位名称
                            IPollutant factor = factors[iRow];
                            GridTableCell cell = (GridTableCell)item[factor.PollutantCode];
                            string factorStatus = drv[factor.PollutantCode + "_Status"] != DBNull.Value ? drv[factor.PollutantCode + "_Status"].ToString() : string.Empty;
                            string factorDataFlag = drv[factor.PollutantCode + "_DataFlag"] != DBNull.Value ? drv[factor.PollutantCode + "_DataFlag"].ToString() : string.Empty;
                            string factorAuditFlag = drv[factor.PollutantCode + "_AuditFlag"] != DBNull.Value ? drv[factor.PollutantCode + "_AuditFlag"].ToString() : string.Empty;
                            decimal value = 0M;
                            if (decimal.TryParse(cell.Text, out value))
                            {
                                if (factor.PollutantMeasureUnit == "μg/m³" && !LZSPfactor.Contains(factor.PollutantCode))
                                {
                                    value = DecimalExtension.GetPollutantValue(value * 1000, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 0 : Convert.ToInt32(factor.PollutantDecimalNum) - 3);
                                }
                                else
                                {
                                    value = DecimalExtension.GetPollutantValue(value, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum));
                                }
                                cell.Text = value.ToString("");
                                if (((factorStatus != "N" && factorStatus != "MF" && !string.IsNullOrEmpty(factorStatus)) || (factorDataFlag != "N" && factorDataFlag != "MF" && !string.IsNullOrEmpty(factorDataFlag)) || (factorAuditFlag != "N" && factorAuditFlag != "MF" && !string.IsNullOrEmpty(factorAuditFlag))))
                                {
                                    string markContent = string.Empty;
                                    if ((factorStatus != "N" && factorStatus != "MF" && !string.IsNullOrEmpty(factorStatus)) || (factorDataFlag != "N" && factorDataFlag != "MF" && !string.IsNullOrEmpty(factorDataFlag)) || (factorAuditFlag != "N" && factorAuditFlag != "MF" && !string.IsNullOrEmpty(factorAuditFlag)))
                                    {
                                        if (!string.IsNullOrEmpty(factorStatus))
                                        {
                                            markContent += factorStatus + ",";
                                        }
                                        if (!string.IsNullOrEmpty(factorDataFlag))
                                        {
                                            markContent += factorDataFlag + ",";
                                        }
                                        if (!string.IsNullOrEmpty(factorAuditFlag))
                                        {
                                            markContent += factorAuditFlag + ",";
                                        }
                                        //markContent += factorStatus + factorDataFlag + factorAuditFlag + "|";
                                        siteTypeName = factorStatus + factorDataFlag + factorAuditFlag + "(" + siteTypeEntites.Where(t => t.ItemValue.Equals(factorStatus))
                        .Select(t => t.ItemText).FirstOrDefault() + siteTypeEntites.Where(t => t.ItemValue.Equals(factorDataFlag))
                        .Select(t => t.ItemText).FirstOrDefault() + siteTypeEntites.Where(t => t.ItemValue.Equals(factorAuditFlag))
                        .Select(t => t.ItemText).FirstOrDefault() + ")" + "|";//标记位名称
                                    }
                                    markContent = markContent.TrimEnd(',');
                                    cell.Text = cell.Text + "(" + markContent + ")";
                                    cell.ForeColor = System.Drawing.Color.Red;
                                    cell.Font.Bold = true;
                                }
                                if (dtExcessive.DefaultView.Count > 0)
                                {
                                    if (item["PointId"] != null)
                                    {
                                        GridTableCell pointCell = (GridTableCell)item["PointId"];
                                        IPoint point = points.FirstOrDefault(x => x.PointName.Equals(pointCell.Text.Trim()));
                                        if (point != null)
                                        {
                                            DataRow[] drExcessive = dtExcessive.Select("PointID='" + point.PointID + "' and PollutantCode='" + factor.PollutantCode + "'");
                                            if (drExcessive.Count() > 0)
                                            {
                                                cell.ToolTip = "上限：" + drExcessive[0]["excessiveUpper"] + "\n下限：" + drExcessive[0]["excessiveLow"] + "\n标记位：" + siteTypeName.TrimEnd('|');
                                            }
                                            else
                                            {
                                                cell.ToolTip = "上限：-- \n下限：--" + "\n标记位：" + siteTypeName.TrimEnd('|');
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    cell.ToolTip = "上限：--\n下限：--";
                                }
                            }
                        }
                    }
                    else if (radlDataType.SelectedValue == "Hour" && ddlDataSource.SelectedValue == "AuditData")
                    {
                        for (int iRow = 0; iRow < factors.Count; iRow++)
                        {
                            string siteTypeName = "--";//标记位名称
                            IPollutant factor = factors[iRow];
                            GridTableCell cell = (GridTableCell)item[factor.PollutantCode];
                            string factorStatus = drv[factor.PollutantCode + "_Status"] != DBNull.Value ? drv[factor.PollutantCode + "_Status"].ToString() : string.Empty;
                            //string factorDataFlag = drv[factor.PollutantCode + "_DataFlag"] != DBNull.Value ? drv[factor.PollutantCode + "_DataFlag"].ToString() : string.Empty;
                            //string factorAuditFlag = drv[factor.PollutantCode + "_AuditFlag"] != DBNull.Value ? drv[factor.PollutantCode + "_AuditFlag"].ToString() : string.Empty;
                            decimal value = 0M;
                            if (decimal.TryParse(cell.Text, out value))
                            {
                                if (factor.PollutantMeasureUnit == "μg/m³" && !LZSPfactor.Contains(factor.PollutantCode))
                                {
                                    value = DecimalExtension.GetPollutantValue(value * 1000, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 0 : Convert.ToInt32(factor.PollutantDecimalNum) - 3);
                                }
                                else
                                {
                                    value = DecimalExtension.GetPollutantValue(value, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum));
                                }
                                cell.Text = value.ToString("");
                                if (((factorStatus != "N" && factorStatus != "MF" && !string.IsNullOrEmpty(factorStatus))))
                                {
                                    string markContent = string.Empty;
                                    if ((factorStatus != "N" && factorStatus != "MF" && !string.IsNullOrEmpty(factorStatus)))
                                    {
                                        markContent += factorStatus + "|";
                                        siteTypeName = factorStatus + "(" + siteTypeEntites.Where(t => t.ItemValue.Equals(factorStatus))
                        .Select(t => t.ItemText).FirstOrDefault() + ")" + "|";//标记位名称
                                    }
                                    markContent = markContent.TrimEnd('|');
                                    cell.Text = cell.Text + "(" + markContent + ")";
                                    cell.ForeColor = System.Drawing.Color.Red;
                                    cell.Font.Bold = true;
                                }
                                if (dtExcessive.DefaultView.Count > 0)
                                {
                                    if (item["PointId"] != null)
                                    {
                                        GridTableCell pointCell = (GridTableCell)item["PointId"];
                                        IPoint point = points.FirstOrDefault(x => x.PointName.Equals(pointCell.Text.Trim()));
                                        if (point != null)
                                        {
                                            DataRow[] drExcessive = dtExcessive.Select("PointID='" + point.PointID + "' and PollutantCode='" + factor.PollutantCode + "'");
                                            if (drExcessive.Count() > 0)
                                            {
                                                cell.ToolTip = "上限：" + drExcessive[0]["excessiveUpper"] + "\n下限：" + drExcessive[0]["excessiveLow"] + "\n标记位：" + siteTypeName.TrimEnd('|');
                                            }
                                            else
                                            {
                                                cell.ToolTip = "上限：-- \n下限：--" + "\n标记位：" + siteTypeName.TrimEnd('|');
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    cell.ToolTip = "上限：--\n下限：--";
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int iRow = 0; iRow < factors.Count; iRow++)
                        {
                            IPollutant factor = factors[iRow];
                            GridTableCell cell = (GridTableCell)item[factor.PollutantCode];
                            decimal value = 0M;
                            if (decimal.TryParse(cell.Text, out value))
                            {
                                if (factor.PollutantMeasureUnit == "μg/m³" && !LZSPfactor.Contains(factor.PollutantCode))
                                {
                                    value = DecimalExtension.GetPollutantValue(value * 1000, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 0 : Convert.ToInt32(factor.PollutantDecimalNum) - 3);
                                }
                                else
                                {
                                    value = DecimalExtension.GetPollutantValue(value, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum));
                                }

                                //value = DecimalExtension.GetPollutantValue(value, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum));
                                cell.Text = value.ToString("");

                                if (dtExcessive.DefaultView.Count > 0)
                                {
                                    if (item["PointId"] != null)
                                    {
                                        GridTableCell pointCell = (GridTableCell)item["PointId"];
                                        IPoint point = points.FirstOrDefault(x => x.PointName.Equals(pointCell.Text.Trim()));
                                        if (point != null)
                                        {
                                            DataRow[] drExcessive = dtExcessive.Select("PointID='" + point.PointID + "' and PollutantCode='" + factor.PollutantCode + "'");
                                            if (drExcessive.Count() > 0)
                                            {
                                                cell.ToolTip = "上限：" + drExcessive[0]["excessiveUpper"] + "\n下限：" + drExcessive[0]["excessiveLow"];
                                            }
                                            else
                                            {
                                                cell.ToolTip = "上限：--\n下限：--";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
                if (e.Item is GridCommandItem)
                {
                    GridCommandItem commandItem = e.Item as GridCommandItem;
                    //绑定标题
                    if (commandItem.FindControl("RadButton1") != null)
                    {
                        //标记位说明可见
                        RadButton button = (RadButton)commandItem.FindControl("RadButton1");
                        button.Visible = true;

                    }
                }
            }
            //不是常规参数
            else
            {
                if (e.Item is GridDataItem)
                {
                    if (item["PointId"] != null)
                    {
                        GridTableCell pointCell = (GridTableCell)item["PointId"];
                        IPoint point = points.FirstOrDefault(x => x.PointID.Equals(pointCell.Text.Trim()));
                        if (points != null)
                            pointCell.Text = point.PointName;
                    }
                    if ((radlDataTypeOri.SelectedValue == "Min60" || radlDataTypeOri.SelectedValue == "Min1" || radlDataTypeOri.SelectedValue == "Min5") && ddlDataSource.SelectedValue == "OriData")
                    {
                        for (int iRow = 0; iRow < factors.Count; iRow++)
                        {
                            string siteTypeName = "--";//标记位名称
                            IPollutant factor = factors[iRow];
                            GridTableCell cell = (GridTableCell)item[factor.PollutantCode];
                            string factorStatus = drv[factor.PollutantCode + "_Status"] != DBNull.Value ? drv[factor.PollutantCode + "_Status"].ToString() : string.Empty;
                            string factorDataFlag = drv[factor.PollutantCode + "_DataFlag"] != DBNull.Value ? drv[factor.PollutantCode + "_DataFlag"].ToString() : string.Empty;
                            string factorAuditFlag = drv[factor.PollutantCode + "_AuditFlag"] != DBNull.Value ? drv[factor.PollutantCode + "_AuditFlag"].ToString() : string.Empty;
                            decimal value = 0M;
                            if (decimal.TryParse(cell.Text, out value))
                            {
                                if (factor.PollutantMeasureUnit == "μg/m³" && !LZSPfactor.Contains(factor.PollutantCode))
                                {
                                    value = DecimalExtension.GetPollutantValue(value * 1000, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 0 : Convert.ToInt32(factor.PollutantDecimalNum) - 3);
                                }
                                else
                                {
                                    value = DecimalExtension.GetPollutantValue(value, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum));
                                }
                                cell.Text = value.ToString("");
                                if (((factorStatus != "N" && factorStatus != "MF" && !string.IsNullOrEmpty(factorStatus)) || (factorDataFlag != "N" && factorDataFlag != "MF" && !string.IsNullOrEmpty(factorDataFlag)) || (factorAuditFlag != "N" && factorAuditFlag != "MF" && !string.IsNullOrEmpty(factorAuditFlag))))
                                {
                                    string markContent = string.Empty;
                                    if ((factorStatus != "N" && factorStatus != "MF" && !string.IsNullOrEmpty(factorStatus)) || (factorDataFlag != "N" && factorDataFlag != "MF" && !string.IsNullOrEmpty(factorDataFlag)) || (factorAuditFlag != "N" && factorAuditFlag != "MF" && !string.IsNullOrEmpty(factorAuditFlag)))
                                    {
                                        if (!string.IsNullOrEmpty(factorStatus))
                                        {
                                            markContent += factorStatus + ",";
                                        }
                                        if (!string.IsNullOrEmpty(factorDataFlag))
                                        {
                                            markContent += factorDataFlag + ",";
                                        }
                                        if (!string.IsNullOrEmpty(factorAuditFlag))
                                        {
                                            markContent += factorAuditFlag + ",";
                                        }
                                        //markContent += factorStatus + factorDataFlag + factorAuditFlag + "|";
                                        siteTypeName = factorStatus + factorDataFlag + factorAuditFlag + "(" + siteTypeEntites.Where(t => t.ItemValue.Equals(factorStatus))
                        .Select(t => t.ItemText).FirstOrDefault() + siteTypeEntites.Where(t => t.ItemValue.Equals(factorDataFlag))
                        .Select(t => t.ItemText).FirstOrDefault() + siteTypeEntites.Where(t => t.ItemValue.Equals(factorAuditFlag))
                        .Select(t => t.ItemText).FirstOrDefault() + ")" + "|";//标记位名称
                                    }
                                    markContent = markContent.TrimEnd(',');
                                    cell.Text = (pageType == "tyfsy" && factor.PollutantCode == "a05024") ? cell.Text : cell.Text + "(" + markContent + ")";
                                    cell.ForeColor = (pageType == "tyfsy" && factor.PollutantCode == "a05024") ? System.Drawing.Color.Black : System.Drawing.Color.Red;
                                    cell.Font.Bold = (pageType == "tyfsy" && factor.PollutantCode == "a05024") ? false : true;
                                }
                                if (dtExcessive.DefaultView.Count > 0)
                                {
                                    if (item["PointId"] != null)
                                    {
                                        GridTableCell pointCell = (GridTableCell)item["PointId"];
                                        IPoint point = points.FirstOrDefault(x => x.PointName.Equals(pointCell.Text.Trim()));
                                        DataRow[] drExcessive = dtExcessive.Select("PointID='" + point.PointID + "' and PollutantCode='" + factor.PollutantCode + "'");
                                        if (drExcessive.Count() > 0)
                                        {
                                            cell.ToolTip = "上限：" + drExcessive[0]["excessiveUpper"] + "\n下限：" + drExcessive[0]["excessiveLow"] + "\n标记位：" + siteTypeName.TrimEnd('|');
                                        }
                                        else
                                        {
                                            cell.ToolTip = "上限：-- \n下限：--" + "\n标记位：" + siteTypeName.TrimEnd('|');
                                        }
                                    }
                                }
                                else
                                {
                                    cell.ToolTip = "上限：--\n下限：--";
                                }
                            }
                        }
                    }
                    else if (radlDataType.SelectedValue == "Hour" && ddlDataSource.SelectedValue == "AuditData")
                    {
                        for (int iRow = 0; iRow < factors.Count; iRow++)
                        {
                            string siteTypeName = "--";//标记位名称
                            IPollutant factor = factors[iRow];
                            GridTableCell cell = (GridTableCell)item[factor.PollutantCode];
                            string factorStatus = drv[factor.PollutantCode + "_Status"] != DBNull.Value ? drv[factor.PollutantCode + "_Status"].ToString() : string.Empty;
                            //string factorDataFlag = drv[factor.PollutantCode + "_DataFlag"] != DBNull.Value ? drv[factor.PollutantCode + "_DataFlag"].ToString() : string.Empty;
                            //string factorAuditFlag = drv[factor.PollutantCode + "_AuditFlag"] != DBNull.Value ? drv[factor.PollutantCode + "_AuditFlag"].ToString() : string.Empty;
                            decimal value = 0M;
                            if (decimal.TryParse(cell.Text, out value))
                            {
                                if (factor.PollutantMeasureUnit == "μg/m³" && !LZSPfactor.Contains(factor.PollutantCode))
                                {
                                    value = DecimalExtension.GetPollutantValue(value * 1000, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 0 : Convert.ToInt32(factor.PollutantDecimalNum) - 3);
                                }
                                else
                                {
                                    value = DecimalExtension.GetPollutantValue(value, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum));
                                }
                                cell.Text = value.ToString("");
                                if (((factorStatus != "N" && factorStatus != "MF" && !string.IsNullOrEmpty(factorStatus))))
                                {
                                    string markContent = string.Empty;
                                    if ((factorStatus != "N" && factorStatus != "MF" && !string.IsNullOrEmpty(factorStatus)))
                                    {
                                        markContent += factorStatus + "|";
                                        siteTypeName = factorStatus + "(" + siteTypeEntites.Where(t => t.ItemValue.Equals(factorStatus))
                        .Select(t => t.ItemText).FirstOrDefault() + ")" + "|";//标记位名称
                                    }
                                    markContent = markContent.TrimEnd('|');
                                    cell.Text = (pageType == "tyfsy" && factor.PollutantCode == "a05024") ? cell.Text : cell.Text + "(" + markContent + ")";
                                    cell.ForeColor = (pageType == "tyfsy" && factor.PollutantCode == "a05024") ? System.Drawing.Color.Black : System.Drawing.Color.Red;
                                    cell.Font.Bold = (pageType == "tyfsy" && factor.PollutantCode == "a05024") ? false : true;
                                }
                                if (dtExcessive.DefaultView.Count > 0)
                                {
                                    if (item["PointId"] != null)
                                    {
                                        GridTableCell pointCell = (GridTableCell)item["PointId"];
                                        IPoint point = points.FirstOrDefault(x => x.PointName.Equals(pointCell.Text.Trim()));
                                        DataRow[] drExcessive = dtExcessive.Select("PointID='" + point.PointID + "' and PollutantCode='" + factor.PollutantCode + "'");
                                        if (drExcessive.Count() > 0)
                                        {
                                            cell.ToolTip = "上限：" + drExcessive[0]["excessiveUpper"] + "\n下限：" + drExcessive[0]["excessiveLow"] + "\n标记位：" + siteTypeName.TrimEnd('|');
                                        }
                                        else
                                        {
                                            cell.ToolTip = "上限：-- \n下限：--" + "\n标记位：" + siteTypeName.TrimEnd('|');
                                        }
                                    }
                                }
                                else
                                {
                                    cell.ToolTip = "上限：--\n下限：--";
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int iRow = 0; iRow < factors.Count; iRow++)
                        {
                            IPollutant factor = factors[iRow];
                            GridTableCell cell = (GridTableCell)item[factor.PollutantCode];
                            decimal value = 0M;
                            if (decimal.TryParse(cell.Text, out value))
                            {
                                if (factor.PollutantMeasureUnit == "μg/m³" && !LZSPfactor.Contains(factor.PollutantCode))
                                {
                                    value = DecimalExtension.GetPollutantValue(value * 1000, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 0 : Convert.ToInt32(factor.PollutantDecimalNum) - 3);
                                }
                                else
                                {
                                    value = DecimalExtension.GetPollutantValue(value, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum));
                                }

                                //value = DecimalExtension.GetPollutantValue(value, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum));
                                cell.Text = value.ToString("");

                                if (dtExcessive.DefaultView.Count > 0)
                                {
                                    if (item["PointId"] != null)
                                    {
                                        GridTableCell pointCell = (GridTableCell)item["PointId"];
                                        IPoint point = points.FirstOrDefault(x => x.PointName.Equals(pointCell.Text.Trim()));
                                        DataRow[] drExcessive = dtExcessive.Select("PointID='" + point.PointID + "' and PollutantCode='" + factor.PollutantCode + "'");
                                        if (drExcessive.Count() > 0)
                                        {
                                            cell.ToolTip = "上限：" + drExcessive[0]["excessiveUpper"] + "\n下限：" + drExcessive[0]["excessiveLow"];
                                        }
                                        else
                                        {
                                            cell.ToolTip = "上限：--\n下限：--";
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
            }
        }

        /// <summary>
        /// 把IQueryable转化为DataTable
        /// </summary>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public DataTable ConvertToDataTable(IEnumerable enumerable)
        {
            var dataTable = new DataTable();
            foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(typeof(ExcessiveSettingInfo)))
            {
                dataTable.Columns.Add(pd.Name);
            }
            foreach (ExcessiveSettingInfo item in enumerable)
            {
                var Row = dataTable.NewRow();

                foreach (PropertyDescriptor dp in TypeDescriptor.GetProperties(typeof(ExcessiveSettingInfo)))
                {
                    Row[dp.Name] = dp.GetValue(item);
                }
                dataTable.Rows.Add(Row);
            }
            return dataTable;
        }

        /// <summary>
        /// 查询按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gridAudit.CurrentPageIndex = 0;
            if (dtms == Convert.ToDateTime(dtpEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:00")))
            {
                dtpEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00"));
            }
            gridAudit.Rebind();


            if (tabStrip.SelectedTab.Text == "图表")
            {
                BindChart();
                SecondLoadChart.Value = "1";
            }
            else if (tabStrip.SelectedTab.Text == "热力图")
            {
                BindHighChart();
                FirstLoadChart.Value = "1";
            }
            else
            {
                SecondLoadChart.Value = "1";
                FirstLoadChart.Value = "1";
            }
        }


        /// <summary>
        /// 图表类型选择（折线图、柱形图、点状图）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ChartType_SelectedIndexChanged(object sender, EventArgs e)
        {
            HiddenChartType.Value = ChartType.SelectedValue;
            RegisterScript("ChartTypeChanged('" + ChartType.SelectedValue + "');");
        }

        /// <summary>
        /// ToolBar事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridRTB_ButtonClick(object sender, RadToolBarEventArgs e)
        {
            Telerik.Web.UI.RadToolBarButton button = (Telerik.Web.UI.RadToolBarButton)e.Item;
            if (button.CommandName == "ExportToExcel")
            {
                BindFactors("979f05bb-f730-4285-8aee-fafdce1360e2", out LZSfactorName, out LZSPfactor);
                string[] portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
                if (PageHelper.GetQueryString("type") == "tyfsy")
                {
                    factors = factorCbxRsm.GetFactors();
                    factors = RemoveO8Code(factors);
                    if (factors.Select(p => p.PollutantCode).ToArray().Contains("a90162"))
                    {
                        IPollutant oldO3 = m_AirPollutantService.GetPollutantInfo("a90162");
                        factors.RemoveAt(factors.Count - 1);
                        IPollutant newO3 = m_AirPollutantService.GetPollutantInfo("a05024");
                        factors.Add(newO3);
                    }
                    else
                    {
                        factors = factorCbxRsm.GetFactors();
                    }
                }
                else
                {
                    factors = factorCbxRsm.GetFactors();
                    factors = RemoveO8Code(factors);
                }
                //factors = factorCbxRsm.GetFactors();
                factors = RemoveO8Code(factors);
                DateTime dateBegion = DateTime.Now;
                DateTime dateEnd = DateTime.Now;
                DataTable dt = new DataTable();

                //每页显示数据个数            
                int pageSize = 999999;
                string pageType = PageHelper.GetQueryString("type");
                //当前页的序号
                int pageNo = 0;

                //数据总行数
                int recordTotal = 0;

                if (portIds != null)
                {
                    //审核数据
                    if (ddlDataSource.SelectedIndex == 1)
                    {
                        if (pageType == "yssh")
                        {
                            this.ViewState["Files"] = "审核";
                            if (radlDataType.SelectedValue == "Hour")
                            {
                                string[] factorCodeSelected = factorCbxRsm.GetFactorValues(SmartEP.Core.Enums.CbxRsmReturnType.Code);
                                if (factorCodeSelected.Contains("a05027"))
                                {
                                    factors = factorCbxRsm.GetFactors();

                                }
                            }
                        }
                        if (radlDataType.SelectedValue == "Hour" && pageType != "yssh")
                        {
                            dateBegion = Convert.ToDateTime(hourBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:00:00"));
                            dateEnd = Convert.ToDateTime(hourEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:59:59"));
                            if (IsStatistical.Checked && factors.Count != 0)
                            {
                                dvStatistical = m_HourData.GetHourStatisticalData(portIds, factors, dateBegion, dateEnd);
                            }
                            string orderBy = "PointId asc,Tstamp desc";
                            if (TimeSort.SelectedValue == "时间升序")
                                orderBy = "PointId asc,Tstamp asc";
                            DataView dv = m_HourData.GetNewHourExportData(portIds, factors, dateBegion, dateEnd, orderBy);
                            dt = UpdateExportColumnName(dv, dvStatistical);

                        }
                        else if (radlDataType.SelectedValue == "Hour" && pageType == "yssh")
                        {
                            if (pageType == "yssh" && rbtnlType.SelectedValue == "CityProper")
                            {
                                dateBegion = Convert.ToDateTime(hourBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:00:00"));
                                dateEnd = Convert.ToDateTime(hourEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:59:59"));
                                if (IsStatistical.Checked && factors.Count != 0)
                                {
                                    dvStatistical = m_HourData.GetHourStatisticalData(portIds, factors, dateBegion, dateEnd);
                                }
                                string orderBy = "sortNumber desc,Tstamp desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderBy = "sortNumber desc,Tstamp asc";
                                DataView dv = m_HourData.GetNewHourDataPagerWidthRegionO8(portIds, factors.Select(p => p.PollutantCode).ToArray(), dateBegion, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
                                dt = UpdateExportColumnName(dv, dvStatistical);
                            }
                            else
                            {
                                dateBegion = Convert.ToDateTime(hourBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:00:00"));
                                dateEnd = Convert.ToDateTime(hourEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:59:59"));
                                if (IsStatistical.Checked && factors.Count != 0)
                                {
                                    dvStatistical = m_HourData.GetHourStatisticalData(portIds, factors, dateBegion, dateEnd);
                                }
                                string orderBy = "PointId asc,Tstamp desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderBy = "PointId asc,Tstamp asc";
                                DataView dv = m_HourData.GetNewHourDataPagerWidthO8(portIds, factors.Select(p => p.PollutantCode).ToArray(), dateBegion, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
                                dt = UpdateExportColumnName(dv, dvStatistical);
                            }
                        }
                        else if (radlDataType.SelectedValue == "Day")
                        {
                            if (pageType == "yssh" && rbtnlType.SelectedValue == "CityProper")
                            {
                                dateBegion = dayBegin.SelectedDate.Value;
                                dateEnd = dayEnd.SelectedDate.Value;
                                if (IsStatistical.Checked && factors.Count != 0)
                                {
                                    dvStatistical = m_DayData.GetDayStatisticalData(portIds, factors, dateBegion, dateBegion);
                                }
                                string orderBy = "sortNumber desc,DateTime desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderBy = "sortNumber desc,DateTime asc";
                                DataView dv = m_DayData.GetDayDataRegionPager(portIds, factors, dateBegion, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
                                dt = UpdateExportColumnName(dv, dvStatistical);
                            }
                            else
                            {
                                dateBegion = dayBegin.SelectedDate.Value;
                                dateEnd = dayEnd.SelectedDate.Value;
                                if (IsStatistical.Checked && factors.Count != 0)
                                {
                                    dvStatistical = m_DayData.GetDayStatisticalData(portIds, factors, dateBegion, dateBegion);
                                }
                                string orderBy = "PointId asc,DateTime desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderBy = "PointId asc,DateTime asc";
                                DataView dv = m_DayData.GetDayExportData(portIds, factors, dateBegion, dateEnd, orderBy);
                                dt = UpdateExportColumnName(dv, dvStatistical);
                            }
                        }
                        else if (radlDataType.SelectedValue == "Month")
                        {
                            if (pageType == "yssh" && rbtnlType.SelectedValue == "CityProper")
                            {
                                int monthB = monthBegin.SelectedDate.Value.Year;
                                int monthE = monthEnd.SelectedDate.Value.Year;
                                int monthF = monthBegin.SelectedDate.Value.Month;
                                int monthT = monthEnd.SelectedDate.Value.Month;
                                if (IsStatistical.Checked && factors.Count != 0)
                                {
                                    dvStatistical = m_MonthData.GetMonthStatisticalData(portIds, factors, monthB, monthF, monthE, monthT);
                                }
                                string orderBy = "sortNumber desc,Year desc,MonthOfYear desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderBy = "sortNumber desc,Year asc,MonthOfYear asc";
                                DataView dv = m_MonthData.GetDataPagerRegion(portIds, factors, monthB, monthF, monthE, monthT, pageSize, pageNo, out recordTotal, orderBy);
                                dt = UpdateExportColumnName(dv, dvStatistical);
                            }
                            else
                            {
                                int monthB = monthBegin.SelectedDate.Value.Year;
                                int monthE = monthEnd.SelectedDate.Value.Year;
                                int monthF = monthBegin.SelectedDate.Value.Month;
                                int monthT = monthEnd.SelectedDate.Value.Month;
                                if (IsStatistical.Checked && factors.Count != 0)
                                {
                                    dvStatistical = m_MonthData.GetMonthStatisticalData(portIds, factors, monthB, monthF, monthE, monthT);
                                }
                                string orderBy = "PointId asc,Year desc,MonthOfYear desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderBy = "PointId asc,Year asc,MonthOfYear asc";
                                DataView dv = m_MonthData.GetMonthExportData(portIds, factors, monthB, monthF, monthE, monthT, orderBy);
                                dt = UpdateExportColumnName(dv, dvStatistical);
                            }

                        }
                        else if (radlDataType.SelectedValue == "Season")
                        {
                            if (pageType == "yssh" && rbtnlType.SelectedValue == "CityProper")
                            {
                                int seasonB = seasonBegin.SelectedDate.Value.Year;
                                int seasonE = seasonEnd.SelectedDate.Value.Year;
                                int seasonF = Convert.ToInt32(seasonFrom.SelectedValue);
                                int seasonT = Convert.ToInt32(seasonTo.SelectedValue);
                                if (IsStatistical.Checked && factors.Count != 0)
                                {
                                    dvStatistical = m_SeasonData.GetSeasonStatisticalData(portIds, factors, seasonB, seasonF, seasonE, seasonT);
                                }
                                string orderBy = "sortNumber desc,Year desc,SeasonOfYear desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderBy = "sortNumber desc,Year asc,SeasonOfYear asc";
                                DataView dv = m_SeasonData.GetSeasonDataPagerRegion(portIds, factors, seasonB, seasonF, seasonE, seasonT, pageSize, pageNo, out recordTotal, orderBy);
                                dt = UpdateExportColumnName(dv, dvStatistical);
                            }
                            else
                            {
                                int seasonB = seasonBegin.SelectedDate.Value.Year;
                                int seasonE = seasonEnd.SelectedDate.Value.Year;
                                int seasonF = Convert.ToInt32(seasonFrom.SelectedValue);
                                int seasonT = Convert.ToInt32(seasonTo.SelectedValue);
                                if (IsStatistical.Checked && factors.Count != 0)
                                {
                                    dvStatistical = m_SeasonData.GetSeasonStatisticalData(portIds, factors, seasonB, seasonF, seasonE, seasonT);
                                }
                                string orderBy = "PointId asc,Year desc,SeasonOfYear desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderBy = "PointId asc,Year asc,SeasonOfYear asc";
                                DataView dv = m_SeasonData.GetSeasonExportData(portIds, factors, seasonB, seasonF, seasonE, seasonT, orderBy);
                                dt = UpdateExportColumnName(dv, dvStatistical);
                            }
                        }
                        else if (radlDataType.SelectedValue == "Year")
                        {
                            if (pageType == "yssh" && rbtnlType.SelectedValue == "CityProper")
                            {
                                int yearB = yearBegin.SelectedDate.Value.Year;
                                int yearE = yearEnd.SelectedDate.Value.Year;
                                if (IsStatistical.Checked && factors.Count != 0)
                                {
                                    dvStatistical = m_YearData.GetYearStatisticalData(portIds, factors, yearB, yearE);
                                }
                                string orderBy = "sortNumber desc,Year desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderBy = "sortNumber desc,Year asc";
                                DataView dv = m_YearData.GetYearDataPagerRegion(portIds, factors, yearB, yearE, pageSize, pageNo, out recordTotal, orderBy);
                                dt = UpdateExportColumnName(dv, dvStatistical);
                            }
                            else
                            {
                                int yearB = yearBegin.SelectedDate.Value.Year;
                                int yearE = yearEnd.SelectedDate.Value.Year;
                                if (IsStatistical.Checked && factors.Count != 0)
                                {
                                    dvStatistical = m_YearData.GetYearStatisticalData(portIds, factors, yearB, yearE);
                                }
                                string orderBy = "PointId asc,Year desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderBy = "PointId asc,Year asc";
                                DataView dv = m_YearData.GetYearExportData(portIds, factors, yearB, yearE, orderBy);
                                dt = UpdateExportColumnName(dv, dvStatistical);
                            }
                        }
                        else if (radlDataType.SelectedValue == "Week")
                        {
                            if (pageType == "yssh" && rbtnlType.SelectedValue == "CityProper")
                            {
                                int weekB = weekBegin.SelectedDate.Value.Year;
                                int weekE = weekEnd.SelectedDate.Value.Year;
                                int newyear = DateTime.ParseExact(weekFrom.SelectedValue, "yyyy-MM-dd", null).AddDays(6).Year;
                                int nyear = DateTime.ParseExact(weekTo.SelectedValue, "yyyy-MM-dd", null).Year;
                                int weekF = 0;
                                int weekT = 0;
                                if (newyear > weekB)
                                {
                                    weekF = ChinaDate.WeekOfYear(DateTime.ParseExact(weekFrom.SelectedValue, "yyyy-MM-dd", null));
                                }
                                else
                                    weekF = ChinaDate.WeekOfYear(DateTime.ParseExact(weekFrom.SelectedValue, "yyyy-MM-dd", null).AddDays(6));

                                if (weekE > nyear)
                                {
                                    weekT = ChinaDate.WeekOfYear(DateTime.ParseExact(weekTo.SelectedValue, "yyyy-MM-dd", null).AddDays(6));
                                }
                                else
                                    weekT = ChinaDate.WeekOfYear(DateTime.ParseExact(weekTo.SelectedValue, "yyyy-MM-dd", null));

                                if (IsStatistical.Checked && factors.Count != 0)
                                {
                                    dvStatistical = m_WeekData.GetWeekStatisticalData(portIds, factors, weekB, weekF, weekE, weekT);
                                }
                                string orderBy = "sortNumber desc,Year desc,WeekOfYear desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderBy = "sortNumber desc,Year asc,WeekOfYear asc";
                                DataView dv = m_WeekData.GetDataPagerRegion(portIds, factors, weekB, weekF, weekE, weekT, pageSize, pageNo, out recordTotal, orderBy);
                                dt = UpdateExportColumnName(dv, dvStatistical);
                            }
                            else
                            {
                                int weekB = weekBegin.SelectedDate.Value.Year;
                                int weekE = weekEnd.SelectedDate.Value.Year;
                                int newyear = DateTime.ParseExact(weekFrom.SelectedValue, "yyyy-MM-dd", null).AddDays(6).Year;
                                int nyear = DateTime.ParseExact(weekTo.SelectedValue, "yyyy-MM-dd", null).Year;
                                int weekF = 0;
                                int weekT = 0;
                                if (newyear > weekB)
                                {
                                    weekF = ChinaDate.WeekOfYear(DateTime.ParseExact(weekFrom.SelectedValue, "yyyy-MM-dd", null));
                                }
                                else
                                    weekF = ChinaDate.WeekOfYear(DateTime.ParseExact(weekFrom.SelectedValue, "yyyy-MM-dd", null).AddDays(6));

                                if (weekE > nyear)
                                {
                                    weekT = ChinaDate.WeekOfYear(DateTime.ParseExact(weekTo.SelectedValue, "yyyy-MM-dd", null).AddDays(6));
                                }
                                else
                                    weekT = ChinaDate.WeekOfYear(DateTime.ParseExact(weekTo.SelectedValue, "yyyy-MM-dd", null));

                                if (IsStatistical.Checked && factors.Count != 0)
                                {
                                    dvStatistical = m_WeekData.GetWeekStatisticalData(portIds, factors, weekB, weekF, weekE, weekT);
                                }
                                string orderBy = "PointId asc,Year desc,WeekOfYear desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderBy = "PointId asc,Year asc,WeekOfYear asc";
                                DataView dv = m_WeekData.GetWeekExportData(portIds, factors, weekB, weekF, weekE, weekT, pageSize, orderBy);
                                dt = UpdateExportColumnName(dv, dvStatistical);
                            }
                        }
                    }
                    //原始数据
                    if (ddlDataSource.SelectedIndex == 0)
                    {
                        if (pageType == "yssh")
                        {
                            this.ViewState["Files"] = "原始";
                            if (radlDataTypeOri.SelectedValue == "Min60")
                            {
                                string[] factorCodeSelected = factorCbxRsm.GetFactorValues(SmartEP.Core.Enums.CbxRsmReturnType.Code);
                                if (factorCodeSelected.Contains("a05027"))
                                {
                                    factors = factorCbxRsm.GetFactors();

                                }
                            }
                        }
                        if (radlDataTypeOri.SelectedValue == "Min1")
                        {
                            string orderBy = "PointId asc,Tstamp desc";
                            if (TimeSort.SelectedValue == "时间升序")
                                orderBy = "PointId asc,Tstamp asc";
                            dateBegion = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:00"));
                            dateEnd = Convert.ToDateTime(dtpEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:00"));
                            DataView dv = m_Min1Data.GetDataPager(portIds, factors, dateBegion, dateEnd, pageSize, pageNo, out recordTotal, orderBy);//原始一分钟数据查询
                            for (int i = dv.Table.Columns.Count - 1; i >= 0; i--)
                            {
                                DataColumn dc = dv.Table.Columns[i];
                                if (dc.ColumnName.ToString().Contains("Status"))
                                {
                                    dv.Table.Columns.Remove(dc.ColumnName.ToString());
                                }
                            }
                            dt = UpdateExportColumnName(dv, dvStatistical);
                        }
                        //原始数据五分钟数据
                        else if (radlDataTypeOri.SelectedValue == "Min5")
                        {
                            string orderBy = "PointId asc,Tstamp desc";
                            if (TimeSort.SelectedValue == "时间升序")
                                orderBy = "PointId asc,Tstamp asc";
                            dateBegion = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:00"));
                            dateEnd = Convert.ToDateTime(dtpEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:00"));
                            DataView dv = m_Min5Data.GetDataPager(portIds, factors, dateBegion, dateEnd, pageSize, pageNo, out recordTotal, orderBy);//原始五分钟数据查询
                            for (int i = dv.Table.Columns.Count - 1; i >= 0; i--)
                            {
                                DataColumn dc = dv.Table.Columns[i];
                                if (dc.ColumnName.ToString().Contains("Status"))
                                {
                                    dv.Table.Columns.Remove(dc.ColumnName.ToString());
                                }
                            }
                            dt = UpdateExportColumnName(dv, dvStatistical);
                        }
                        //原始小时数据包含O3-8因子
                        else if (radlDataTypeOri.SelectedValue == "Min60" && pageType == "yssh")
                        {
                            if (pageType == "yssh" && rbtnlType.SelectedValue == "CityProper")
                            {
                                string[] factorCodes = factorCbxRsm.GetFactorValues(SmartEP.Core.Enums.CbxRsmReturnType.Code);
                                string orderBy = "sortNumber desc,Tstamp desc";
                                //string orderBy = "time.tstamp desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                    //orderBy = "time.tstamp ";
                                    orderBy = "sortNumber desc,Tstamp asc";
                                dateBegion = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:00:00"));
                                dateEnd = Convert.ToDateTime(dtpEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:59:59"));
                                DataView dv = null;
                                //NT O3早一小时做处理
                                if (isSuper == "1")
                                {
                                    if (pageType == "lzspy")//离子色谱仪数据晚两个小时
                                    {
                                        dv = m_Min60Data.GetDataPagerForO3AllTimeLZSPY(portIds, factorCodes, dateBegion, dateEnd, pageSize, pageNo, out recordTotal, orderBy);//原始小时数据查询
                                    }
                                    else//其他仪器数据晚一个小时
                                    {
                                        if (pageType == "tyfsy" && factorCodes.Contains("a90162"))
                                        {
                                            for (int i = 0; i < factorCodes.Length; i++)
                                            {
                                                if (factorCodes[i] == "a90162")
                                                {
                                                    factorCodes[i] = "a05024";
                                                }
                                            }
                                        }
                                        dv = m_Min60Data.GetDataPagerForO3AllTime(portIds, factorCodes, dateBegion, dateEnd, pageSize, pageNo, out recordTotal, orderBy);//原始小时数据查询
                                    }
                                    //dv = m_Min60Data.GetDataPagerForO3(portIds, factorCodes, dateBegion, dateEnd, pageSize, pageNo, out recordTotal, orderBy);//原始小时数据查询
                                }
                                else
                                {
                                    //常规站
                                    dateBegion = Convert.ToDateTime(dateBegion.ToString("yyyy-MM-dd HH:59:59"));
                                    dv = m_Min60Data.GetDataPagerAllTimeWithO8Region(portIds, factorCodes, dateBegion, dateEnd, pageSize, pageNo, out recordTotal, orderBy);//原始小时数据查询

                                    //dv = m_Min60Data.GetDataPager(portIds, factorCodes, dateBegion, dateEnd, pageSize, pageNo, out recordTotal, orderBy);//原始小时数据查询
                                }
                                //DataView dv = m_Min60Data.GetDataPager(portIds, factors, dateBegion, dateEnd, pageSize, pageNo, out recordTotal, orderBy);//原始小时数据查询
                                for (int i = dv.Table.Columns.Count - 1; i >= 0; i--)
                                {
                                    DataColumn dc = dv.Table.Columns[i];
                                    if (dc.ColumnName.ToString().Contains("Status") || dc.ColumnName.ToString().Contains("rows") || dc.ColumnName.ToString().Contains("sortNumber"))
                                    {
                                        dv.Table.Columns.Remove(dc.ColumnName.ToString());
                                    }
                                }
                                dt = UpdateExportColumnName(dv, dvStatistical);
                            }
                            else
                            {
                                string[] factorCodes = factorCbxRsm.GetFactorValues(SmartEP.Core.Enums.CbxRsmReturnType.Code);
                                string orderBy = "PointId asc,Tstamp desc";
                                //string orderBy = "time.tstamp desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                    //orderBy = "time.tstamp ";
                                    orderBy = "PointId asc,Tstamp asc";
                                dateBegion = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:00:00"));
                                dateEnd = Convert.ToDateTime(dtpEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:59:59"));
                                DataView dv = null;
                                //NT O3早一小时做处理
                                if (isSuper == "1")
                                {
                                    if (pageType == "lzspy")//离子色谱仪数据晚两个小时
                                    {
                                        dv = m_Min60Data.GetDataPagerForO3AllTimeLZSPY(portIds, factorCodes, dateBegion, dateEnd, pageSize, pageNo, out recordTotal, orderBy);//原始小时数据查询
                                    }
                                    else//其他仪器数据晚一个小时
                                    {
                                        if (pageType == "tyfsy" && factorCodes.Contains("a90162"))
                                        {
                                            for (int i = 0; i < factorCodes.Length; i++)
                                            {
                                                if (factorCodes[i] == "a90162")
                                                {
                                                    factorCodes[i] = "a05024";
                                                }
                                            }
                                        }
                                        dv = m_Min60Data.GetDataPagerForO3AllTime(portIds, factorCodes, dateBegion, dateEnd, pageSize, pageNo, out recordTotal, orderBy);//原始小时数据查询
                                    }
                                    //dv = m_Min60Data.GetDataPagerForO3(portIds, factorCodes, dateBegion, dateEnd, pageSize, pageNo, out recordTotal, orderBy);//原始小时数据查询
                                }
                                else
                                {
                                    //常规站
                                    dateBegion = Convert.ToDateTime(dateBegion.ToString("yyyy-MM-dd HH:59:59"));
                                    dv = m_Min60Data.GetDataPagerAllTimeWithO8(portIds, factorCodes, dateBegion, dateEnd, pageSize, pageNo, out recordTotal, orderBy);//原始小时数据查询

                                    //dv = m_Min60Data.GetDataPager(portIds, factorCodes, dateBegion, dateEnd, pageSize, pageNo, out recordTotal, orderBy);//原始小时数据查询
                                }
                                //DataView dv = m_Min60Data.GetDataPager(portIds, factors, dateBegion, dateEnd, pageSize, pageNo, out recordTotal, orderBy);//原始小时数据查询
                                for (int i = dv.Table.Columns.Count - 1; i >= 0; i--)
                                {
                                    DataColumn dc = dv.Table.Columns[i];
                                    if (dc.ColumnName.ToString().Contains("Status") || dc.ColumnName.ToString().Contains("rows"))
                                    {
                                        dv.Table.Columns.Remove(dc.ColumnName.ToString());
                                    }
                                }
                                dt = UpdateExportColumnName(dv, dvStatistical);
                            }
                        }
                        //原始数据小时数据
                        else if (radlDataTypeOri.SelectedValue == "Min60" && pageType != "yssh")
                        {
                            string[] factorCodes = factorCbxRsm.GetFactorValues(SmartEP.Core.Enums.CbxRsmReturnType.Code);
                            string orderBy = "PointId asc,Tstamp desc";
                            //string orderBy = "time.tstamp desc";
                            if (TimeSort.SelectedValue == "时间升序")
                                //orderBy = "time.tstamp ";
                                orderBy = "PointId asc,Tstamp asc";
                            dateBegion = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:00:00"));
                            dateEnd = Convert.ToDateTime(dtpEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:59:59"));
                            DataView dv = null;
                            //NT O3早一小时做处理
                            if (isSuper == "1")
                            {
                                if (pageType == "lzspy")//离子色谱仪数据晚两个小时
                                {
                                    dv = m_Min60Data.GetDataPagerForO3AllTimeLZSPY(portIds, factorCodes, dateBegion, dateEnd, pageSize, pageNo, out recordTotal, orderBy);//原始小时数据查询

                                }
                                else//其他仪器数据晚一个小时
                                {
                                    if (pageType == "tyfsy" && factorCodes.Contains("a90162"))
                                    {
                                        for (int i = 0; i < factorCodes.Length; i++)
                                        {
                                            if (factorCodes[i] == "a90162")
                                            {
                                                factorCodes[i] = "a05024";
                                            }
                                        }
                                    }
                                    dv = m_Min60Data.GetDataPagerForO3AllTime(portIds, factorCodes, dateBegion, dateEnd, pageSize, pageNo, out recordTotal, orderBy);//原始小时数据查询
                                }
                                //dv = m_Min60Data.GetDataPagerForO3(portIds, factorCodes, dateBegion, dateEnd, pageSize, pageNo, out recordTotal, orderBy);//原始小时数据查询
                            }
                            else
                            {
                                //常规站
                                dateBegion = Convert.ToDateTime(dateBegion.ToString("yyyy-MM-dd HH:59:59"));
                                dv = m_Min60Data.GetDataPagerAllTime(portIds, factorCodes, dateBegion, dateEnd, pageSize, pageNo, out recordTotal, orderBy);//原始小时数据查询

                                //dv = m_Min60Data.GetDataPager(portIds, factorCodes, dateBegion, dateEnd, pageSize, pageNo, out recordTotal, orderBy);//原始小时数据查询
                            }
                            //DataView dv = m_Min60Data.GetDataPager(portIds, factors, dateBegion, dateEnd, pageSize, pageNo, out recordTotal, orderBy);//原始小时数据查询
                            for (int i = dv.Table.Columns.Count - 1; i >= 0; i--)
                            {
                                DataColumn dc = dv.Table.Columns[i];
                                if (dc.ColumnName.ToString().Contains("Status") || dc.ColumnName.ToString().Contains("rows"))
                                {
                                    dv.Table.Columns.Remove(dc.ColumnName.ToString());
                                }
                            }
                            dt = UpdateExportColumnName(dv, dvStatistical);
                        }
                        //原始数据日数据
                        else if (radlDataTypeOri.SelectedValue == "OriDay")
                        {
                            if (pageType == "yssh" && rbtnlType.SelectedValue == "CityProper")
                            {
                                List<string> list = new List<string>();
                                foreach (IPollutant factor in factors)
                                {
                                    list.Add(factor.PollutantCode);
                                }
                                string[] fac = list.ToArray();
                                string orderBy = "sortNumber desc,DateTime desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderBy = "sortNumber desc,DateTime asc";
                                dateBegion = dtpDayBegin.SelectedDate.Value;
                                dateEnd = dtpDayEnd.SelectedDate.Value;
                                DataView dv = null;
                                if (pageType == "tyfsy")
                                {
                                    dv = m_DayOriData.GetDataPagersWithMax(portIds, fac, dateBegion, dateEnd, pageSize, pageNo, out recordTotal, orderBy);//原始日数据类型按 原始日数据查询
                                }
                                else
                                {
                                    dv = m_DayOriData.GetDataPagerForAllTimeRegion(portIds, fac, dateBegion, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
                                    //dv = m_DayOriData.GetDataPagers(portIds, fac, dateBegion, dateEnd, pageSize, pageNo, out recordTotal, orderBy);//原始日数据类型按 原始日数据查询
                                }
                                int index = 0;
                                if (fac.Contains("a05024") || fac.Contains("a05040") || fac.Contains("a05041"))
                                {
                                    for (int i = 0; i < fac.Length; i++)
                                    {
                                        if (fac[i].Equals("a05024") || fac[i].Equals("a05040") || fac[i].Equals("a05041"))
                                        {
                                            index = i;
                                        }
                                    }
                                    dv.Table.Columns[fac[index]].SetOrdinal(index + 3);
                                }
                                //DataView dv = m_DayOriData.GetDataPagers(portIds, fac, dateBegion, dateEnd, pageSize, pageNo, out recordTotal, orderBy);//原始日数据类型按 原始日数据查询
                                for (int i = dv.Table.Columns.Count - 1; i >= 0; i--)
                                {
                                    DataColumn dc = dv.Table.Columns[i];
                                    if (dc.ColumnName.ToString().Contains("Status") || dc.ColumnName.ToString().Contains("sortNumber"))
                                    {
                                        dv.Table.Columns.Remove(dc.ColumnName.ToString());
                                    }
                                }
                                dt = UpdateExportColumnName(dv, dvStatistical);
                            }
                            else
                            {
                                List<string> list = new List<string>();
                                foreach (IPollutant factor in factors)
                                {
                                    list.Add(factor.PollutantCode);
                                }
                                string[] fac = list.ToArray();
                                string orderBy = "PointId asc,DateTime desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderBy = "PointId asc,DateTime asc";
                                dateBegion = dtpDayBegin.SelectedDate.Value;
                                dateEnd = dtpDayEnd.SelectedDate.Value;
                                DataView dv = null;
                                if (pageType == "tyfsy")
                                {
                                    dv = m_DayOriData.GetDataPagersWithMax(portIds, fac, dateBegion, dateEnd, pageSize, pageNo, out recordTotal, orderBy);//原始日数据类型按 原始日数据查询
                                }
                                else
                                {
                                    dv = m_DayOriData.GetDataPagerForNTO3AllTime(portIds, fac, dateBegion, dateEnd, pageSize, pageNo, out recordTotal, orderBy);
                                    //dv = m_DayOriData.GetDataPagers(portIds, fac, dateBegion, dateEnd, pageSize, pageNo, out recordTotal, orderBy);//原始日数据类型按 原始日数据查询
                                }
                                int index = 0;
                                if (fac.Contains("a05024") || fac.Contains("a05040") || fac.Contains("a05041"))
                                {
                                    for (int i = 0; i < fac.Length; i++)
                                    {
                                        if (fac[i].Equals("a05024") || fac[i].Equals("a05040") || fac[i].Equals("a05041"))
                                        {
                                            index = i;
                                        }
                                    }
                                    dv.Table.Columns[fac[index]].SetOrdinal(index + 3);
                                }
                                //DataView dv = m_DayOriData.GetDataPagers(portIds, fac, dateBegion, dateEnd, pageSize, pageNo, out recordTotal, orderBy);//原始日数据类型按 原始日数据查询
                                for (int i = dv.Table.Columns.Count - 1; i >= 0; i--)
                                {
                                    DataColumn dc = dv.Table.Columns[i];
                                    if (dc.ColumnName.ToString().Contains("Status"))
                                    {
                                        dv.Table.Columns.Remove(dc.ColumnName.ToString());
                                    }
                                }
                                dt = UpdateExportColumnName(dv, dvStatistical);
                            }
                        }
                        //原始数据月数据
                        else if (radlDataTypeOri.SelectedValue == "OriMonth")
                        {
                            if (pageType == "yssh" && rbtnlType.SelectedValue == "CityProper")
                            {
                                List<string> list = new List<string>();
                                foreach (IPollutant factor in factors)
                                {
                                    list.Add(factor.PollutantCode);
                                }
                                string[] fac = list.ToArray();
                                string orderBy = "sortNumber desc,Year desc,MonthOfYear desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderBy = "sortNumber desc,Year asc,MonthOfYear asc";
                                dateBegion = dtpMonthBegin.SelectedDate.Value;
                                dateEnd = dtpMonthEnd.SelectedDate.Value;
                                DataView dv = m_MonthOriData.GetOriDataPagerRegion(portIds, fac, dateBegion, dateEnd, pageSize, pageNo, out recordTotal, orderBy);//原始月数据类型按 原始月数据查询
                                for (int i = dv.Table.Columns.Count - 1; i >= 0; i--)
                                {
                                    DataColumn dc = dv.Table.Columns[i];
                                    if (dc.ColumnName.ToString().Contains("Status") || dc.ColumnName.ToString().Contains("sortNumber"))
                                    {
                                        dv.Table.Columns.Remove(dc.ColumnName.ToString());
                                    }
                                }
                                dt = UpdateExportColumnName(dv, dvStatistical);
                            }
                            else
                            {
                                List<string> list = new List<string>();
                                foreach (IPollutant factor in factors)
                                {
                                    list.Add(factor.PollutantCode);
                                }
                                string[] fac = list.ToArray();
                                string orderBy = "PointId asc,Year desc,MonthOfYear desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderBy = "PointId asc,Year asc,MonthOfYear asc";
                                dateBegion = dtpMonthBegin.SelectedDate.Value;
                                dateEnd = dtpMonthEnd.SelectedDate.Value;
                                DataView dv = m_MonthOriData.GetOriDataPager(portIds, fac, dateBegion, dateEnd, pageSize, pageNo, out recordTotal, orderBy);//原始月数据类型按 原始月数据查询
                                for (int i = dv.Table.Columns.Count - 1; i >= 0; i--)
                                {
                                    DataColumn dc = dv.Table.Columns[i];
                                    if (dc.ColumnName.ToString().Contains("Status"))
                                    {
                                        dv.Table.Columns.Remove(dc.ColumnName.ToString());
                                    }
                                }
                                dt = UpdateExportColumnName(dv, dvStatistical);
                            }
                        }
                    }
                    decimal value = 0M;
                    //for (int i = 0; i < dt.Rows.Count; i++)
                    //{
                    //    if (dt.Columns.Contains("日期") && dt.Rows[i]["日期"] != DBNull.Value)
                    //    {
                    //        dt.Rows[i]["日期"] = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Parse(dt.Rows[i]["日期"].ToString()));
                    //    }
                    //    //for (int j = 0; j < dt.Columns.Count; j++)
                    //    //{
                    //    //    if (dt.Columns[j].ColumnName.Contains("μg/m³") && !LZSfactorName.Contains(dt.Columns[j].ColumnName.Replace("(μg/m³)", "")))
                    //    //    {
                    //    //        if (!string.IsNullOrWhiteSpace(dt.Rows[i][j].ToString()))
                    //    //        {
                    //    //            if (decimal.TryParse(dt.Rows[i][j].ToString(), out value))
                    //    //            {
                    //    //                dt.Rows[i][j] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                    //    //            }
                    //    //        }

                    //    //    }
                    //    //    else
                    //    //    {
                    //    //        if (!string.IsNullOrWhiteSpace(dt.Rows[i][j].ToString()))
                    //    //        {
                    //    //            if (decimal.TryParse(dt.Rows[i][j].ToString(), out value))
                    //    //            {
                    //    //                //dt.Rows[i][j] = DecimalExtension.GetPollutantValue(value, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum));
                    //    //               dt.Rows[i][j] = DecimalExtension.GetPollutantValue(value, 3);
                    //    //            }
                    //    //        }
                    //    //    }
                    //    //}
                    //}
                    log.Info("导出开始------------");
                    ExcelHelper.DataTableToExcel(dt, this.ViewState["Files"].ToString(), this.ViewState["Files"].ToString(), this.Page);
                }
            }
        }

        /// <summary>
        /// 修改要导出的数据表中的列名
        /// </summary>
        /// <param name="dv">原始数据表</param>
        /// <returns></returns>
        private DataTable UpdateExportColumnName(DataView dv, DataView dvStatistical)
        {
            string pageType = PageHelper.GetQueryString("type");
            DataTable dtNew = dv.ToTable();

            DataTable dtnew = dtNew.Clone();
            //原始数据
            if (ddlDataSource.SelectedIndex == 0)
            {
                if (radlDataTypeOri.SelectedValue == "OriDay")
                {
                    dtnew.Columns["DateTime"].DataType = typeof(string);
                }
            }
            else
            {
                if (radlDataType.SelectedValue == "Day")
                {
                    dtnew.Columns["日期"].DataType = typeof(string);
                }
            }

            for (int i = 0; i < dtNew.Rows.Count; i++)
            {
                DataRow drNew = dtNew.Rows[i];
                DataRow drnew = dtnew.NewRow();
                foreach (DataColumn dcNew in dtNew.Columns)
                {
                    if (!string.IsNullOrWhiteSpace(drNew[dcNew].ToString()))
                    {
                        drnew[dcNew.ColumnName] = drNew[dcNew].ToString().Replace("<br/>", " \r\n");
                    }

                }
                dtnew.Rows.Add(drnew);
            }

            dtnew.Columns.Add("站点", typeof(string)).SetOrdinal(0);
            points = pointCbxRsm.GetPoints();
            for (int i = 0; i < dtnew.Rows.Count; i++)
            {
                DataRow drNew = dtnew.Rows[i];
                drNew["站点"] = (points.Count(t => t.PointID == drNew["PointId"].ToString()) > 0)
                    ? points.Where(t => t.PointID == drNew["PointId"].ToString()).Select(t => t.PointName).FirstOrDefault()
                    : drNew["PointId"].ToString();
                //原始数据
                if (ddlDataSource.SelectedIndex == 0)
                {
                    if (radlDataTypeOri.SelectedValue == "OriDay")
                    {
                        drNew["DateTime"] = string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(drNew["DateTime"].ToString()).ToString("yyyy-MM-dd"));
                    }
                }
                else
                {
                    if (radlDataType.SelectedValue == "Day")
                    {
                        drNew["日期"] = string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(drNew["日期"].ToString()).ToString("yyyy-MM-dd"));
                    }
                }
            }
            if (ddlDataSource.SelectedIndex == 0 && radlDataTypeOri.SelectedValue == "OriDay" && pageType == "tyfsy")
            {
                for (int i = 0; i < dtnew.Columns.Count; i++)
                {
                    DataColumn dcNew = dtnew.Columns[i];
                    //追加日期
                    if (dcNew.ColumnName == "Tstamp")
                    {
                        string tstcolformat = "{0:MM-dd HH:mm}";
                        dcNew.ColumnName = "日期";
                    }
                    else if (dcNew.ColumnName == "DateTime")
                    {
                        string tstcolformat = "{0:yyyy-MM-dd}";
                        dcNew.ColumnName = "日期";
                    }
                    else if (dcNew.ColumnName == "Year")
                    {
                        dcNew.ColumnName = "年份";
                    }
                    else if (dcNew.ColumnName == "WeekOfYear")
                    {
                        dcNew.ColumnName = "周";
                    }
                    else if (dcNew.ColumnName == "MonthOfYear")
                    {
                        dcNew.ColumnName = "月份";
                    }
                    else if (dcNew.ColumnName == "SeasonOfYear")
                    {
                        dcNew.ColumnName = "季";
                    }
                    //对因子列进行处理
                    else if (isFactor(dcNew.ColumnName))
                    {
                        decimal value = 0M;
                        IPollutant factor;
                        if (dcNew.ColumnName == "a05024")
                        {
                            factor = m_AirPollutantService.GetPollutantInfo("a05024");
                        }
                        else
                        {
                            factor = factorCbxRsm.GetFactors().FirstOrDefault(x => x.PollutantCode.Equals(dcNew.ColumnName));
                        }

                        if (factor != null)
                        {
                            string name = factor.PollutantName;
                            string unit = factor.PollutantMeasureUnit;
                            if (unit.Trim() == "")
                            {
                                dcNew.ColumnName = name;
                            }
                            else
                            {
                                if (unit.Equals("μg/m<sup>3</sup>"))
                                {
                                    unit = "μg/m3";
                                }
                                dcNew.ColumnName = name + "(" + unit + ")";
                            }
                            for (int j = 0; j < dtnew.Rows.Count; j++)
                            {
                                //if (dcNew.ColumnName.Contains("μg/m³") && !LZSfactorName.Contains(dcNew.ColumnName.Replace("(μg/m³)", "")))
                                if (factor.PollutantCode == "a21026" || factor.PollutantCode == "a21004" || factor.PollutantCode == "a05024" || factor.PollutantCode == "a05027" || factor.PollutantCode == "a34002" || factor.PollutantCode == "a34004")
                                {
                                    if (!string.IsNullOrWhiteSpace(dtnew.Rows[j][dcNew.ColumnName].ToString()))
                                    {
                                        if (decimal.TryParse(dtnew.Rows[j][dcNew.ColumnName].ToString(), out value))
                                        {
                                            dtnew.Rows[j][dcNew.ColumnName] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                        }
                                    }

                                }
                                else
                                {
                                    if (!string.IsNullOrWhiteSpace(dtnew.Rows[j][dcNew.ColumnName].ToString()))
                                    {
                                        if (decimal.TryParse(dtnew.Rows[j][dcNew.ColumnName].ToString(), out value))
                                        {
                                            dtnew.Rows[j][dcNew.ColumnName] = DecimalExtension.GetPollutantValue(value, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum));
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (dcNew.ColumnName == "序号" || dcNew.ColumnName == "PointId" || dcNew.ColumnName.Contains("_Status") || dcNew.ColumnName.Contains("_DataFlag") || dcNew.ColumnName.Contains("_AuditFlag") || dcNew.ColumnName.Equals("rows"))
                    {
                        dtnew.Columns.Remove(dcNew);
                        i--;
                    }
                }
            }
            else
            {
                for (int i = 0; i < dtnew.Columns.Count; i++)
                {
                    DataColumn dcNew = dtnew.Columns[i];
                    //追加日期
                    if (dcNew.ColumnName == "Tstamp")
                    {
                        string tstcolformat = "{0:MM-dd HH:mm}";
                        dcNew.ColumnName = "日期";
                    }
                    else if (dcNew.ColumnName == "DateTime")
                    {
                        string tstcolformat = "{0:yyyy-MM-dd}";
                        dcNew.ColumnName = "日期";
                    }
                    else if (dcNew.ColumnName == "Year")
                    {
                        dcNew.ColumnName = "年份";
                    }
                    else if (dcNew.ColumnName == "WeekOfYear")
                    {
                        dcNew.ColumnName = "周";
                    }
                    else if (dcNew.ColumnName == "MonthOfYear")
                    {
                        dcNew.ColumnName = "月份";
                    }
                    else if (dcNew.ColumnName == "SeasonOfYear")
                    {
                        dcNew.ColumnName = "季";
                    }
                    //对因子列进行处理
                    else if (isFactor(dcNew.ColumnName) && dcNew.ColumnName != "a05041" && dcNew.ColumnName != "a05040")
                    {
                        decimal value = 0M;
                        IPollutant factor;
                        if (dcNew.ColumnName == "a05024")
                        {
                            factor = m_AirPollutantService.GetPollutantInfo("a05024");
                        }
                        else
                        {
                            factor = factorCbxRsm.GetFactors().FirstOrDefault(x => x.PollutantCode.Equals(dcNew.ColumnName));
                        }

                        if (factor != null)
                        {
                            string name = factor.PollutantName;
                            string unit = factor.PollutantMeasureUnit;
                            if (unit.Trim() == "")
                            {
                                dcNew.ColumnName = name;
                            }
                            else
                            {
                                if (unit.Equals("μg/m<sup>3</sup>"))
                                {
                                    unit = "μg/m3";
                                }
                                dcNew.ColumnName = name + "(" + unit + ")";
                            }
                            for (int j = 0; j < dtnew.Rows.Count; j++)
                            {
                                //if (dcNew.ColumnName.Contains("μg/m³") && !LZSfactorName.Contains(dcNew.ColumnName.Replace("(μg/m³)", "")))
                                if (factor.PollutantCode == "a21026" || factor.PollutantCode == "a21004" || factor.PollutantCode == "a05024" || factor.PollutantCode == "a05027" || factor.PollutantCode == "a34002" || factor.PollutantCode == "a34004")
                                {
                                    if (!string.IsNullOrWhiteSpace(dtnew.Rows[j][dcNew.ColumnName].ToString()))
                                    {
                                        if (decimal.TryParse(dtnew.Rows[j][dcNew.ColumnName].ToString(), out value))
                                        {
                                            dtnew.Rows[j][dcNew.ColumnName] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                        }
                                    }

                                }
                                else
                                {
                                    if (!string.IsNullOrWhiteSpace(dtnew.Rows[j][dcNew.ColumnName].ToString()))
                                    {
                                        if (decimal.TryParse(dtnew.Rows[j][dcNew.ColumnName].ToString(), out value))
                                        {
                                            dtnew.Rows[j][dcNew.ColumnName] = DecimalExtension.GetPollutantValue(value, string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum));
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (dcNew.ColumnName == "序号" || dcNew.ColumnName == "a05041" || dcNew.ColumnName == "a05040" || dcNew.ColumnName == "PointId" || dcNew.ColumnName.Contains("_Status") || dcNew.ColumnName.Contains("_DataFlag") || dcNew.ColumnName.Contains("_AuditFlag") || dcNew.ColumnName.Equals("rows"))
                    {
                        dtnew.Columns.Remove(dcNew);
                        i--;
                    }
                }
            }
            
            if (dvStatistical != null && dvStatistical.Table.Rows.Count > 0)
            {
                DataTable dtStatistical = dvStatistical.Table;
                DataRow drMaxRow = dtnew.NewRow();
                drMaxRow["站点"] = "最大值";
                DataRow drMinRow = dtnew.NewRow();
                drMinRow["站点"] = "最小值";
                DataRow drAvgRow = dtnew.NewRow();
                drAvgRow["站点"] = "平均值";
                for (int i = 0; i < dtStatistical.Rows.Count; i++)
                {
                    DataRow drStatistical = dtStatistical.Rows[i];
                    if (drStatistical["PollutantCode"] != DBNull.Value && drStatistical["PollutantCode"].ToString() != "")
                    {
                        IPollutant factor = factorCbxRsm.GetFactors().FirstOrDefault(x => x.PollutantCode.Equals(drStatistical["PollutantCode"].ToString()));
                        int pdn = 0;
                        if (factor != null)
                        {
                            pdn = Convert.ToInt32(string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum));
                        }
                        if (dtnew.Columns.Contains(factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"))
                        {
                            drMaxRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"] = drStatistical["Value_Max"] != DBNull.Value ? drStatistical["Value_Max"] : "--";
                            drMinRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"] = drStatistical["Value_Min"] != DBNull.Value ? drStatistical["Value_Min"] : "--";
                            drAvgRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"] = drStatistical["Value_Avg"] != DBNull.Value ? drStatistical["Value_Avg"] : "--";

                            if (drMaxRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"].ToString() != "--")
                            {
                                decimal AVG = Convert.ToDecimal(drMaxRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"]);
                                drMaxRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"] = DecimalExtension.GetPollutantValue(AVG, pdn).ToString();
                            }
                            if (drMinRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"].ToString() != "--")
                            {
                                decimal MAX = Convert.ToDecimal(drMinRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"]);
                                drMinRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"] = DecimalExtension.GetPollutantValue(MAX, pdn).ToString();
                            }
                            if (drAvgRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"].ToString() != "--")
                            {
                                decimal MIN = Convert.ToDecimal(drAvgRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"]);
                                drAvgRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"] = DecimalExtension.GetPollutantValue(MIN, pdn).ToString();
                            }
                        }
                    }
                }
                dtnew.Rows.Add(drAvgRow);
                dtnew.Rows.Add(drMaxRow);
                dtnew.Rows.Add(drMinRow);
            }
            return dtnew;
        }

        /// <summary>
        /// 判断一个字符串是否为因子编码
        /// </summary>
        /// <param name="factorCode"></param>
        /// <returns></returns>
        public bool isFactor(string factorCode)
        {
            string reg = "^[A-Za-z]+[0-9]{4,7}$";
            if (Regex.IsMatch(factorCode, reg))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 分屏、合并切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ShowType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindChart();
        }
        #endregion

        /// <summary>
        /// 站点因子关联
        /// </summary>
        protected void pointCbxRsm_SelectedChanged()
        {
        }


        /// <summary>
        /// 绑定周
        /// </summary>
        private void BindWeekFComboBox()
        {
            if (weekBegin.SelectedDate > System.DateTime.Now)
            {
                Alert("选择时间必须小于等于当前时间！");
                return;
            }
            weekFrom.DataValueField = "value";
            weekFrom.DataTextField = "text";
            weekFrom.DataSource = ChinaDate.GetWeekOfMonth(weekBegin.SelectedDate.Value);
            weekFrom.DataBind();
            SetLiteral();
        }

        private void BindWeekTComboBox()
        {
            if (weekEnd.SelectedDate > System.DateTime.Now)
            {
                Alert("选择时间必须小于等于当前时间！");
                return;
            }
            weekTo.DataValueField = "value";
            weekTo.DataTextField = "text";
            weekTo.DataSource = ChinaDate.GetWeekOfMonth(weekEnd.SelectedDate.Value);
            weekTo.DataBind();
            SetLiteralT();
        }
        /// <summary>
        /// 显示所选周的日期
        /// </summary>
        private void SetLiteral()
        {
            txtweekF.Text = string.Format("从{0:yyyy-MM-dd}", weekFrom.SelectedValue);
        }
        private void SetLiteralT()
        {
            DateTime endDate = DateTime.ParseExact(weekTo.SelectedValue, "yyyy-MM-dd", null).AddDays(6);
            if (endDate > System.DateTime.Now)
                endDate = System.DateTime.Now;
            txtweekT.Text = string.Format("到{0:yyyy-MM-dd}", endDate);
        }
        #region 周数据更新日期范围
        protected void weekBegin_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            BindWeekFComboBox();
        }

        protected void weekFrom_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            SetLiteral();
        }

        protected void weekEnd_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            BindWeekTComboBox();
        }

        protected void weekTo_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            SetLiteralT();
        }
        #endregion

        /// <summary>
        /// 数据来源选项变化，数据类型选项相应变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlDataSource_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            if (ddlDataSource.SelectedIndex == 0)
            {
                radlDataTypeOri.Visible = true;
                radlDataType.Visible = false;
                radlDataTypeOri.SelectedValue = "Min60";
                //dtpBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddHours(-2).ToString("yyyy-MM-dd HH:00"));
                //dtpEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00"));
                //dtpBegin.DateInput.DateFormat = "yyyy-MM-dd HH:00";
                //dtpEnd.DateInput.DateFormat = "yyyy-MM-dd HH:00";
                dtpHour.Visible = true;
                dtpDay.Visible = false;
                dtpMonth.Visible = false;
                dbtHour.Visible = false;
                dbtDay.Visible = false;
                dbtMonth.Visible = false;
                dbtSeason.Visible = false;
                dbtYear.Visible = false;
                dbtWeek.Visible = false;
            }
            else
            {
                radlDataTypeOri.Visible = false;
                radlDataType.Visible = true;
                radlDataType.SelectedIndex = 0;
                //hourBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddHours(-47).ToString("yyyy-MM-dd HH:00"));
                //hourEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00"));
                //hourBegin.DateInput.DateFormat = "yyyy-MM-dd HH:00";
                //hourEnd.DateInput.DateFormat = "yyyy-MM-dd HH:00";
                dtpHour.Visible = false;
                dtpDay.Visible = false;
                dtpMonth.Visible = false;
                dbtHour.Visible = true;
                dbtDay.Visible = false;
                dbtMonth.Visible = false;
                dbtSeason.Visible = false;
                dbtYear.Visible = false;
                dbtWeek.Visible = false;
            }
        }

        /// <summary>
        /// 因子、站点类型图表选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PointFactor_SelectedIndexChanged(object sender, EventArgs e)
        {
            HiddenPointFactor.Value = PointFactor.SelectedValue;
            RegisterScript("PointFactor('" + PointFactor.SelectedValue + "');");
        }

        /// <summary>
        /// 原始数据数据类型时间框选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void radlDataTypeOri_SelectedIndexChanged(object sender, EventArgs e)
        {
            gridAudit.CurrentPageIndex = 0;
            //一分钟数据
            if (radlDataTypeOri.SelectedValue == "Min1")
            {
                dtpBegin.SelectedDate = DateTime.Now.AddHours(-1);
                dtpEnd.SelectedDate = DateTime.Now;
                dtpHour.Visible = true;
                dtpDay.Visible = false;
                dtpMonth.Visible = false;
                dbtHour.Visible = false;
                dbtDay.Visible = false;
                dbtMonth.Visible = false;
                dbtSeason.Visible = false;
                dbtYear.Visible = false;
                dbtWeek.Visible = false;
                dtpBegin.DateInput.DateFormat = "yyyy-MM-dd HH:mm";
                dtpEnd.DateInput.DateFormat = "yyyy-MM-dd HH:mm";
            }
            //五分钟数据
            if (radlDataTypeOri.SelectedValue == "Min5")
            {
                dtpBegin.SelectedDate = DateTime.Now.AddHours(-1);
                dtpEnd.SelectedDate = DateTime.Now;
                dtpHour.Visible = true;
                dtpDay.Visible = false;
                dtpMonth.Visible = false;
                dbtHour.Visible = false;
                dbtDay.Visible = false;
                dbtMonth.Visible = false;
                dbtSeason.Visible = false;
                dbtYear.Visible = false;
                dbtWeek.Visible = false;
                dtpBegin.DateInput.DateFormat = "yyyy-MM-dd HH:mm";
                dtpEnd.DateInput.DateFormat = "yyyy-MM-dd HH:mm";
            }
            //小时数据
            if (radlDataTypeOri.SelectedValue == "Min60")
            {
                dtpHour.Visible = true;
                dtpDay.Visible = false;
                dtpMonth.Visible = false;
                dtpDay.Visible = false;
                dbtHour.Visible = false;
                dbtDay.Visible = false;
                dbtMonth.Visible = false;
                dbtSeason.Visible = false;
                dbtYear.Visible = false;
                dbtWeek.Visible = false;
                dtpBegin.DateInput.DateFormat = "yyyy-MM-dd HH:00";
                dtpEnd.DateInput.DateFormat = "yyyy-MM-dd HH:00";
            }
            //日数据
            if (radlDataTypeOri.SelectedValue == "OriDay")
            {
                dtpHour.Visible = false;
                dtpDay.Visible = true;
                dtpMonth.Visible = false;
                dbtHour.Visible = false;
                dbtDay.Visible = false;
                dbtMonth.Visible = false;
                dbtSeason.Visible = false;
                dbtYear.Visible = false;
                dbtWeek.Visible = false;
                //dtpBegin.DateInput.DateFormat = "yyyy-MM-dd";
                //dtpEnd.DateInput.DateFormat = "yyyy-MM-dd";
            }
            //月数据
            if (radlDataTypeOri.SelectedValue == "OriMonth")
            {
                dtpHour.Visible = false;
                dtpDay.Visible = false;
                dtpMonth.Visible = true;
                dbtHour.Visible = false;
                dbtDay.Visible = false;
                dbtMonth.Visible = false;
                dbtSeason.Visible = false;
                dbtYear.Visible = false;
                dbtWeek.Visible = false;
                //dtpBegin.DateInput.DateFormat = "yyyy-MM";
                //dtpEnd.DateInput.DateFormat = "yyyy-MM";
            }
        }
        /// <summary>
        /// 审核数据数据类型时间框选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void radlDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            gridAudit.CurrentPageIndex = 0;
            //小时数据

            if (radlDataType.SelectedValue == "Hour")
            {
                dtpHour.Visible = false;
                dtpDay.Visible = false;
                dtpMonth.Visible = false;
                dbtHour.Visible = true;
                dbtDay.Visible = false;
                dbtMonth.Visible = false;
                dbtYear.Visible = false;
                dbtWeek.Visible = false;
                dbtSeason.Visible = false;


            }
            //日数据
            else if (radlDataType.SelectedValue == "Day")
            {
                dtpHour.Visible = false;
                dtpDay.Visible = false;
                dtpMonth.Visible = false;
                dbtDay.Visible = true;
                dbtHour.Visible = false;
                dbtMonth.Visible = false;
                dbtYear.Visible = false;
                dbtWeek.Visible = false;
                dbtSeason.Visible = false;
            }
            //周数据
            else if (radlDataType.SelectedValue == "Week")
            {
                dtpHour.Visible = false;
                dtpDay.Visible = false;
                dtpMonth.Visible = false;
                dbtWeek.Visible = true;
                dbtYear.Visible = false;
                dbtMonth.Visible = false;
                dbtDay.Visible = false;
                dbtHour.Visible = false;
                dbtSeason.Visible = false;
            }
            //月数据
            else if (radlDataType.SelectedValue == "Month")
            {
                dtpHour.Visible = false;
                dtpDay.Visible = false;
                dtpMonth.Visible = false;
                dbtMonth.Visible = true;
                dbtDay.Visible = false;
                dbtHour.Visible = false;
                dbtYear.Visible = false;
                dbtWeek.Visible = false;
                dbtSeason.Visible = false;

            }
            //季数据
            else if (radlDataType.SelectedValue == "Season")
            {
                dtpHour.Visible = false;
                dtpDay.Visible = false;
                dtpMonth.Visible = false;
                dbtSeason.Visible = true;
                dbtMonth.Visible = false;
                dbtDay.Visible = false;
                dbtHour.Visible = false;
                dbtYear.Visible = false;
                dbtWeek.Visible = false;

            }
            //年数据
            else if (radlDataType.SelectedValue == "Year")
            {
                dtpHour.Visible = false;
                dtpDay.Visible = false;
                dtpMonth.Visible = false;
                dbtYear.Visible = true;
                dbtMonth.Visible = false;
                dbtDay.Visible = false;
                dbtHour.Visible = false;
                dbtWeek.Visible = false;
                dbtSeason.Visible = false;
            }
        }
    }
}