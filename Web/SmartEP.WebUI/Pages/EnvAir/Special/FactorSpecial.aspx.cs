﻿using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.Service.AutoMonitoring.Air;
using SmartEP.Service.DataAnalyze.Air.DataQuery;
using SmartEP.Utilities.Calendar;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Utilities.IO;
using SmartEP.Utilities.Web.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.Special
{
    public partial class FactorSpecial : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 日数据接口
        /// </summary>
        GranuleSpecialService g_GranuleSpecial = new GranuleSpecialService();
        SmartEP.Service.BaseData.Channel.AirPollutantService m_AirPollutantService = new SmartEP.Service.BaseData.Channel.AirPollutantService();
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
        /// <summary>
        /// 选择因子
        /// </summary>
        private IList<IPollutant> pfactors = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.ViewState["Type"] = PageHelper.GetQueryString("Type");
                InitControl();
            }
        }
        /// <summary>
        /// 默认是否常规站字段为空
        /// </summary>
        string isAudit = string.Empty;
        protected override void OnPreInit(EventArgs e)
        {
            isAudit = "factorS";
            if (isAudit != null && isAudit != "")
            {
                factorCbxRsm.isAudit(isAudit);
            }
        }
        ///// <summary>
        ///// 初始化控件
        ///// </summary>
        private void InitControl()
        {
            string factors = System.Configuration.ConfigurationManager.AppSettings["PollutantCode4"];
            //if (ddlLiJing.SelectedValue == "0")
            //{
            //    factors += System.Configuration.ConfigurationManager.AppSettings["PollutantCode1"];
            //}
            //if (ddlLiJing.SelectedValue == "1")
            //{
            //    factors += System.Configuration.ConfigurationManager.AppSettings["PollutantCode2"];
            //}
            //if (ddlLiJing.SelectedValue == "2")
            //{
            //    factors += System.Configuration.ConfigurationManager.AppSettings["PollutantCode3"];
            //}
            //factors += string.Join(",", factorCom.CheckedItems.Select(x => x.Value).ToArray());
            string points = System.Configuration.ConfigurationManager.AppSettings["SuperStationPointName"];
            pointCbxRsm.SetPointValuesFromNames(points);
            string Type = this.ViewState["Type"].ToString();
            string factorCode1 = System.Configuration.ConfigurationManager.AppSettings["PMCode"];
            if (Type == "PM")
            {
                factorCode1 = System.Configuration.ConfigurationManager.AppSettings["PMCode"];
            }
            else
            {
                factorCode1 = System.Configuration.ConfigurationManager.AppSettings["O3Code"];
            }
            //因子控件
            foreach (RadComboBoxItem item in factorCom.Items)
            {
                if (factorCode1.Contains(item.Value))
                {
                    item.Checked = true;
                }
            }


            factorCbxRsm.SetFactorValuesFromCodes(factors.Trim(';'));
            //数据类型
            radlDataType.Items.Add(new ListItem("小时", PollutantDataType.Hour.ToString()));
            radlDataType.Items.Add(new ListItem("日", PollutantDataType.Day.ToString()));
            radlDataType.Items.Add(new ListItem("周", PollutantDataType.Week.ToString()));
            radlDataType.Items.Add(new ListItem("月", PollutantDataType.Month.ToString()));
            radlDataType.Items.Add(new ListItem("季", PollutantDataType.Season.ToString()));
            radlDataType.Items.Add(new ListItem("年", PollutantDataType.Year.ToString()));

            //radlDataTypeOri.Items.Add(new ListItem("一分钟", PollutantDataType.Min1.ToString()));
            //radlDataTypeOri.Items.Add(new ListItem("五分钟", PollutantDataType.Min5.ToString()));
            radlDataTypeOri.Items.Add(new ListItem("小时", PollutantDataType.Min60.ToString()));
            radlDataTypeOri.Items.Add(new ListItem("日", PollutantDataType.OriDay.ToString()));
            radlDataTypeOri.Items.Add(new ListItem("月", PollutantDataType.OriMonth.ToString()));

            radlDataType.SelectedValue = PollutantDataType.Hour.ToString();
            radlDataTypeOri.SelectedValue = PollutantDataType.Min60.ToString();

            //时间框初始化
            hourBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddHours(-47));
            hourEnd.SelectedDate = DateTime.Now;
            dtpBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm"));
            dtpEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
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

            dtpHour.Visible = true;
            dbtHour.Visible = false;
            dbtDay.Visible = false;
            dbtMonth.Visible = false;
            dbtSeason.Visible = false;
            dbtYear.Visible = false;
            dbtWeek.Visible = false;
            BindData();
            RegisterScript("ReChart();");

        }
        ///// <summary>
        ///// 绑定数据
        ///// </summary>
        private void BindData()
        {
            DataView auditData = new DataView();
            string[] portIds = { "204" };
            //if (ddlLiJing.SelectedValue == "0")
            //{
            //    portIds
            //}
            List<string> pointIDD = new List<string>();

            DataTable dt = new DataTable();
            DataTable dtMonth = new DataTable();
            DataTable dtInstrumen = new DataTable();
            int pageSize = int.MaxValue;
            int pageNo = 0;
            int recordTotal = 0;
            List<string> listTypeName = new List<string>();
            string[] portIdsCG=null;
            if (pointCbxRsm.GetPointValues(CbxRsmReturnType.ID).Contains("204"))
            {
                portIdsCG = pointCbxRsm.GetPointValues(CbxRsmReturnType.ID);
            }
            else
            {
                for (int i = 0; i < pointCbxRsm.GetPointValues(CbxRsmReturnType.ID).Length;i++ )
                {
                    pointIDD.Add(pointCbxRsm.GetPointValues(CbxRsmReturnType.ID)[i]);
                }
                pointIDD.Add("204");
                portIdsCG = pointIDD.ToArray();
            }
             
            
            string [] fac = factorCom.CheckedItems.Select(x => x.Value).ToArray();
            pfactors = factorCbxRsm.GetFactors();
            IPollutant iFactorCode = null;
            for (int i = 0; i < fac.Length; i++)
            {
                iFactorCode = m_AirPollutantService.GetPollutantInfo(fac[i]);
                pfactors.Add(iFactorCode);
            }
            
            DataTable dtOneMin = new DataTable();
            if (portIds != null)
            {
                if (ddlDataSource.SelectedValue == "OriData")
                {
                    DateTime dtBegion = dtpBegin.SelectedDate.Value;
                    DateTime dtEnd = dtpEnd.SelectedDate.Value;
                    #region OriDay
                    if (radlDataTypeOri.SelectedValue == "OriDay")
                    {
                        this.ViewState.Add("dt", dt);
                        SetHiddenData(portIds, pfactors, dtBegion, dtEnd);
                        auditData = m_DayOriData.GetAvgDataPagers(portIdsCG, pfactors.Select(p => p.PollutantCode).ToArray(), dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                        dt = auditData.ToTable();
                        for (int i = 0; i < pfactors.Select(p => p.PollutantCode).ToArray().Length; i++)
                        {
                            dt.Columns[pfactors.Select(p => p.PollutantCode).ToArray()[i]].ColumnName = m_AirPollutantService.GetPollutantInfo(pfactors.Select(p => p.PollutantCode).ToArray()[i]).PollutantName;
                            listTypeName.Add(m_AirPollutantService.GetPollutantInfo(pfactors.Select(p => p.PollutantCode).ToArray()[i]).PollutantName);
                        }
                        dtInstrumen = dt;
                        if (dtInstrumen != null)
                        {
                            bool a = dtInstrumen.Columns.Contains("Tstamp");
                            if (a)
                            {
                                if (dtInstrumen.Columns.Count > 2)
                                {
                                    dtInstrumen.Columns["Tstamp"].SetOrdinal(0);
                                }
                            }
                            bool b = dtInstrumen.Columns.Contains("DateTime");
                            if (b)
                            {
                                if (dtInstrumen.Columns.Count > 2)
                                {
                                    dtInstrumen.Columns["DateTime"].SetOrdinal(0);
                                }
                            }
                        }
                        #region 数据*1000
                        if (dtInstrumen.Columns.Contains("O₃"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;

                                if (decimal.TryParse(dtInstrumen.Rows[i]["O₃"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["O₃"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("PM₁₀"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;

                                if (decimal.TryParse(dtInstrumen.Rows[i]["PM₁₀"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["PM₁₀"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("PM₂.₅"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["PM₂.₅"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["PM₂.₅"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("TSP"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["TSP"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["TSP"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("PM₁.₀"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["PM₁.₀"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["PM₁.₀"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NOᵧ"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NOᵧ"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NO"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NO"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NO"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NOₓ"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NOₓ"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NOₓ"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NOᵧ₁"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ₁"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NOᵧ₁"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NOᵧ₂"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ₂"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NOᵧ₂"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        #endregion
                        string Time = "[";
                        for (int i = 0; i < dtInstrumen.Rows.Count; i++)
                        {
                            Time += "'" + Convert.ToDateTime(dtInstrumen.Rows[i]["DateTime"].ToString()).ToString("MM/dd HH时") + "',";
                        }
                        Time = Time.TrimEnd(',');
                        Time += "]";
                        string data = "[{connectNulls:true,";
                        for (int i = 0; i < listTypeName.ToArray().Length; i++)
                        {
                            
                            if ("Thermal OC;Thermal EC;OptEC;OptOC;TC".Contains(dtInstrumen.Columns[i + 1].ColumnName))
                            {
                                data += "yAxis: 0,tooltip: { valueSuffix: 'μgC/m3' },smooth: true,name:'" + dtInstrumen.Columns[i + 1].ColumnName + "',data:[";
                            }
                            else if (dtInstrumen.Columns[i + 1].ColumnName.Contains("μm"))
                            {
                                data += "yAxis: 3,tooltip: { valueSuffix: '个/L' },smooth: true,name:'" + dtInstrumen.Columns[i + 1].ColumnName + "',data:[";
                            }
                            else if (dtInstrumen.Columns[i + 1].ColumnName == "CO")
                            {
                                data += "yAxis: 2,tooltip: { valueSuffix: 'mg/m3' },smooth: true,name:'" + dtInstrumen.Columns[i + 1].ColumnName + "',data:[";
                            }
                            else if (dtInstrumen.Columns[i + 1].ColumnName.Contains("nm"))
                            {
                                data += "yAxis: 4,tooltip: { valueSuffix: '' },smooth: true,name:'" + dtInstrumen.Columns[i + 1].ColumnName + "',data:[";
                            }
                            else
                            {
                                data += "yAxis: 1,tooltip: { valueSuffix: 'μg/m3' },smooth: true,name:'" + dtInstrumen.Columns[i + 1].ColumnName + "',data:[";
                                
                            }
                            for (int j = 0; j < dtInstrumen.Rows.Count; j++)
                            {
                                if (dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() == "" || dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() == "0")
                                {
                                    data += "null,";
                                }
                                else
                                {
                                    data += dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() + ",";
                                }
                            }
                            data += "]}, {connectNulls:true,";
                        }
                        data = data.Substring(0, data.Length - 21);
                        data += "]";
                        hdDate.Value = data;
                        hdDTime.Value = Time;
                    }
                    #endregion
                    #region OriMonth
                    if (radlDataTypeOri.SelectedValue == "OriMonth")
                    {
                        SetHiddenData(portIds, pfactors, dtBegion, dtEnd);
                        auditData = m_MonthOriData.GetOriAvgDataPager(portIdsCG, pfactors.Select(p => p.PollutantCode).ToArray(), dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Year asc,MonthOfYear asc");
                        dtMonth = auditData.ToTable();
                        //dt.Columns["旧列名"].ColumnName = "新的列名";
                        for (int i = 0; i < pfactors.Select(p => p.PollutantCode).ToArray().Length; i++)
                        {
                            dtMonth.Columns[pfactors.Select(p => p.PollutantCode).ToArray()[i]].ColumnName = m_AirPollutantService.GetPollutantInfo(pfactors.Select(p => p.PollutantCode).ToArray()[i]).PollutantName;
                            listTypeName.Add(m_AirPollutantService.GetPollutantInfo(pfactors.Select(p => p.PollutantCode).ToArray()[i]).PollutantName);
                        }
                        
                        dtInstrumen = dtMonth;
                        if (dtInstrumen != null)
                        {
                            bool a = dtInstrumen.Columns.Contains("Tstamp");
                            if (a)
                            {
                                if (dtInstrumen.Columns.Count > 2)
                                {
                                    dtInstrumen.Columns["Tstamp"].SetOrdinal(1);
                                }
                            }
                            bool b = dtInstrumen.Columns.Contains("DateTime");
                            if (b)
                            {
                                if (dtInstrumen.Columns.Count > 2)
                                {
                                    dtInstrumen.Columns["DateTime"].SetOrdinal(1);
                                }
                            }
                        }
                        #region 数据*1000
                        if (dtInstrumen.Columns.Contains("O₃"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;

                                if (decimal.TryParse(dtInstrumen.Rows[i]["O₃"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["O₃"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("PM₁₀"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;

                                if (decimal.TryParse(dtInstrumen.Rows[i]["PM₁₀"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["PM₁₀"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("PM₂.₅"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["PM₂.₅"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["PM₂.₅"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("TSP"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["TSP"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["TSP"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("PM₁.₀"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["PM₁.₀"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["PM₁.₀"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NOᵧ"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NOᵧ"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NO"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NO"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NO"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NOₓ"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NOₓ"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NOₓ"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NOᵧ₁"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ₁"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NOᵧ₁"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NOᵧ₂"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ₂"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NOᵧ₂"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        #endregion
                        string Time = "[";
                        for (int i = 0; i < dtInstrumen.Rows.Count; i++)
                        {
                            //Time += "'" + string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Year, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Month - 1, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Day, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Hour, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Minute, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Second, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Millisecond) + "',";
                            Time += "'" + dtInstrumen.Rows[i]["Year"].ToString() + "/" + dtInstrumen.Rows[i]["MonthOfYear"].ToString() + "',";
                        }
                        Time = Time.TrimEnd(',');
                        Time += "]";
                        string data = "[{connectNulls:true,";
                        for (int i = 0; i < listTypeName.ToArray().Length; i++)
                        {

                            if ("Thermal OC;Thermal EC;OptEC;OptOC;TC".Contains(dtInstrumen.Columns[i + 2].ColumnName))
                            {
                                data += "yAxis: 0,tooltip: { valueSuffix: 'μgC/m3' },smooth: true,name:'" + dtInstrumen.Columns[i + 2].ColumnName + "',data:[";
                            }
                            else if (dtInstrumen.Columns[i + 2].ColumnName.Contains("μm"))
                            {
                                data += "yAxis: 3,tooltip: { valueSuffix: '个/L' },smooth: true,name:'" + dtInstrumen.Columns[i + 2].ColumnName + "',data:[";
                            }
                            else if (dtInstrumen.Columns[i + 2].ColumnName == "CO")
                            {
                                data += "yAxis: 2,tooltip: { valueSuffix: 'mg/m3' },smooth: true,name:'" + dtInstrumen.Columns[i + 2].ColumnName + "',data:[";
                            }
                            else if (dtInstrumen.Columns[i + 2].ColumnName.Contains("nm"))
                            {
                                data += "yAxis: 4,tooltip: { valueSuffix: '' },smooth: true,name:'" + dtInstrumen.Columns[i + 2].ColumnName + "',data:[";
                            }
                            else
                            {
                                data += "yAxis: 1,tooltip: { valueSuffix: 'μg/m3' },smooth: true,name:'" + dtInstrumen.Columns[i + 2].ColumnName + "',data:[";

                            }
                            for (int j = 0; j < dtInstrumen.Rows.Count; j++)
                            {
                                if (dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() == "" || dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() == "0")
                                {
                                    data += "null,";
                                }
                                else
                                {
                                    data += dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() + ",";
                                }
                            }
                            data += "]}, {connectNulls:true,";
                        }
                        data = data.Substring(0, data.Length - 21);
                        data += "]";
                        hdDate.Value = data;
                        hdDTime.Value = Time;
                    }
                    #endregion
                    #region Min1
                    if (radlDataTypeOri.SelectedValue == "Min1")
                    {
                        this.ViewState.Add("dt", dt);
                        SetHiddenData(portIds, pfactors, dtBegion, dtEnd);
                        auditData = m_Min1Data.GetAvgDataPager(portIdsCG, pfactors.Select(p => p.PollutantCode).ToArray(), dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                        dt = auditData.ToTable();
                        //dt.Columns["旧列名"].ColumnName = "新的列名";
                        for (int i = 0; i < pfactors.Select(p => p.PollutantCode).ToArray().Length; i++)
                        {
                            dt.Columns[pfactors.Select(p => p.PollutantCode).ToArray()[i]].ColumnName = m_AirPollutantService.GetPollutantInfo(pfactors.Select(p => p.PollutantCode).ToArray()[i]).PollutantName;
                            listTypeName.Add(m_AirPollutantService.GetPollutantInfo(pfactors.Select(p => p.PollutantCode).ToArray()[i]).PollutantName);
                        }
                        
                        dtInstrumen = dt;
                        if (dtInstrumen != null)
                        {
                            bool a = dtInstrumen.Columns.Contains("Tstamp");
                            if (a)
                            {
                                if (dtInstrumen.Columns.Count > 2)
                                {
                                    dtInstrumen.Columns["Tstamp"].SetOrdinal(0);
                                }
                            }
                            bool b = dtInstrumen.Columns.Contains("DateTime");
                            if (b)
                            {
                                if (dtInstrumen.Columns.Count > 2)
                                {
                                    dtInstrumen.Columns["DateTime"].SetOrdinal(0);
                                }
                            }
                        }
                        #region 数据*1000
                        if (dtInstrumen.Columns.Contains("O₃"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;

                                if (decimal.TryParse(dtInstrumen.Rows[i]["O₃"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["O₃"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("PM₁₀"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;

                                if (decimal.TryParse(dtInstrumen.Rows[i]["PM₁₀"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["PM₁₀"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("PM₂.₅"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["PM₂.₅"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["PM₂.₅"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("TSP"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["TSP"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["TSP"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("PM₁.₀"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["PM₁.₀"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["PM₁.₀"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NOᵧ"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NOᵧ"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NO"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NO"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NO"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NOₓ"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NOₓ"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NOₓ"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NOᵧ₁"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ₁"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NOᵧ₁"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NOᵧ₂"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ₂"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NOᵧ₂"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        #endregion
                        string Time = "[";
                        for (int i = 0; i < dtInstrumen.Rows.Count; i++)
                        {
                            //Time += "'" + string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Year, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Month - 1, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Day, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Hour, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Minute, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Second, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Millisecond) + "',";
                            Time += "'" + Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).ToString("MM/dd HH时") + "',";
                        }
                        Time = Time.TrimEnd(',');
                        Time += "]";
                        string data = "[{connectNulls:true,";
                        for (int i = 0; i < listTypeName.ToArray().Length; i++)
                        {

                            if ("Thermal OC;Thermal EC;OptEC;OptOC;TC".Contains(dtInstrumen.Columns[i + 1].ColumnName))
                            {
                                data += "yAxis: 0,tooltip: { valueSuffix: 'μgC/m3' },smooth: true,name:'" + dtInstrumen.Columns[i + 1].ColumnName + "',data:[";
                            }
                            else if (dtInstrumen.Columns[i + 1].ColumnName.Contains("μm"))
                            {
                                data += "yAxis: 3,tooltip: { valueSuffix: '个/L' },smooth: true,name:'" + dtInstrumen.Columns[i + 1].ColumnName + "',data:[";
                            }
                            else if (dtInstrumen.Columns[i + 1].ColumnName == "CO")
                            {
                                data += "yAxis: 2,tooltip: { valueSuffix: 'mg/m3' },smooth: true,name:'" + dtInstrumen.Columns[i + 1].ColumnName + "',data:[";
                            }
                            else if (dtInstrumen.Columns[i + 1].ColumnName.Contains("nm"))
                            {
                                data += "yAxis: 4,tooltip: { valueSuffix: '' },smooth: true,name:'" + dtInstrumen.Columns[i + 1].ColumnName + "',data:[";
                            }
                            else
                            {
                                data += "yAxis: 1,tooltip: { valueSuffix: 'μg/m3' },smooth: true,name:'" + dtInstrumen.Columns[i + 2].ColumnName + "',data:[";

                            }
                            for (int j = 0; j < dtInstrumen.Rows.Count; j++)
                            {
                                if (dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() == "" || dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() == "0")
                                {
                                    data += "null,";
                                }
                                else
                                {
                                    data += dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() + ",";
                                }
                            }
                            data += "]}, {connectNulls:true,";
                        }
                        data = data.Substring(0, data.Length - 21);
                        data += "]";
                        hdDate.Value = data;
                        hdDTime.Value = Time;
                    }
                    #endregion
                    #region Min5
                    if (radlDataTypeOri.SelectedValue == "Min5" )
                    {
                        this.ViewState.Add("dt", dt);
                        SetHiddenData(portIds, pfactors, dtBegion, dtEnd);
                        auditData = m_Min5Data.GetAvgDataPager(portIdsCG, pfactors.Select(p => p.PollutantCode).ToArray(), dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                        dt = auditData.ToTable();
                        for (int i = 0; i < pfactors.Select(p => p.PollutantCode).ToArray().Length; i++)
                        {
                            dt.Columns[pfactors.Select(p => p.PollutantCode).ToArray()[i]].ColumnName = m_AirPollutantService.GetPollutantInfo(pfactors.Select(p => p.PollutantCode).ToArray()[i]).PollutantName;
                            listTypeName.Add(m_AirPollutantService.GetPollutantInfo(pfactors.Select(p => p.PollutantCode).ToArray()[i]).PollutantName);
                        }
                        
                        dtInstrumen = dt;
                        if (dtInstrumen != null)
                        {
                            bool a = dtInstrumen.Columns.Contains("Tstamp");
                            if (a)
                            {
                                if (dtInstrumen.Columns.Count > 2)
                                {
                                    dtInstrumen.Columns["Tstamp"].SetOrdinal(0);
                                }
                            }
                            bool b = dtInstrumen.Columns.Contains("DateTime");
                            if (b)
                            {
                                if (dtInstrumen.Columns.Count > 2)
                                {
                                    dtInstrumen.Columns["DateTime"].SetOrdinal(0);
                                }
                            }
                        }
                        #region 数据*1000
                        if (dtInstrumen.Columns.Contains("O₃"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;

                                if (decimal.TryParse(dtInstrumen.Rows[i]["O₃"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["O₃"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("PM₁₀"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;

                                if (decimal.TryParse(dtInstrumen.Rows[i]["PM₁₀"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["PM₁₀"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("PM₂.₅"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["PM₂.₅"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["PM₂.₅"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("TSP"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["TSP"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["TSP"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("PM₁.₀"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["PM₁.₀"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["PM₁.₀"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NOᵧ"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NOᵧ"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NO"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NO"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NO"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NOₓ"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NOₓ"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NOₓ"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NOᵧ₁"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ₁"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NOᵧ₁"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NOᵧ₂"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ₂"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NOᵧ₂"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        #endregion
                        string Time = "[";
                        for (int i = 0; i < dtInstrumen.Rows.Count; i++)
                        {
                            //Time += "'" + string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Year, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Month - 1, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Day, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Hour, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Minute, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Second, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Millisecond) + "',";
                            Time += "'" + Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).ToString("MM/dd HH时") + "',";
                        }
                        Time = Time.TrimEnd(',');
                        Time += "]";
                        string data = "[{connectNulls:true,";
                        for (int i = 0; i < listTypeName.ToArray().Length; i++)
                        {

                            if ("Thermal OC;Thermal EC;OptEC;OptOC;TC".Contains(dtInstrumen.Columns[i + 1].ColumnName))
                            {
                                data += "yAxis: 0,tooltip: { valueSuffix: 'μgC/m3' },smooth: true,name:'" + dtInstrumen.Columns[i + 1].ColumnName + "',data:[";
                            }
                            else if (dtInstrumen.Columns[i + 1].ColumnName.Contains("μm"))
                            {
                                data += "yAxis: 3,tooltip: { valueSuffix: '个/L' },smooth: true,name:'" + dtInstrumen.Columns[i + 1].ColumnName + "',data:[";
                            }
                            else if (dtInstrumen.Columns[i + 1].ColumnName == "CO")
                            {
                                data += "yAxis: 2,tooltip: { valueSuffix: 'mg/m3' },smooth: true,name:'" + dtInstrumen.Columns[i + 1].ColumnName + "',data:[";
                            }
                            else if (dtInstrumen.Columns[i + 1].ColumnName.Contains("nm"))
                            {
                                data += "yAxis: 4,tooltip: { valueSuffix: '' },smooth: true,name:'" + dtInstrumen.Columns[i + 1].ColumnName + "',data:[";
                            }
                            else
                            {
                                data += "yAxis: 1,tooltip: { valueSuffix: 'μg/m3' },smooth: true,name:'" + dtInstrumen.Columns[i + 1].ColumnName + "',data:[";

                            }
                            for (int j = 0; j < dtInstrumen.Rows.Count; j++)
                            {
                                if (dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() == "" || dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() == "0")
                                {
                                    data += "null,";
                                }
                                else
                                {
                                    data += dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() + ",";
                                }
                            }
                            data += "]}, {connectNulls:true,";
                        }
                        data = data.Substring(0, data.Length - 21);
                        data += "]";
                        hdDate.Value = data;
                        hdDTime.Value = Time;
                    }
                    #endregion
                    #region Min60

                    if (radlDataTypeOri.SelectedValue == "Min60")
                    {
                        this.ViewState.Add("dt", dt);
                        SetHiddenData(portIds, pfactors, dtBegion, dtEnd);
                        auditData = m_Min60Data.GetAvgDataPager(portIdsCG, pfactors.Select(p => p.PollutantCode).ToArray(), dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                        dt = auditData.ToTable();
                        for (int i = 0; i < pfactors.Select(p => p.PollutantCode).ToArray().Length; i++)
                        {
                            dt.Columns[pfactors.Select(p => p.PollutantCode).ToArray()[i]].ColumnName = m_AirPollutantService.GetPollutantInfo(pfactors.Select(p => p.PollutantCode).ToArray()[i]).PollutantName;
                            listTypeName.Add(m_AirPollutantService.GetPollutantInfo(pfactors.Select(p => p.PollutantCode).ToArray()[i]).PollutantName);
                        }
                        dtInstrumen = dt;
                        if (dtInstrumen != null)
                        {
                            bool a = dtInstrumen.Columns.Contains("Tstamp");
                            if (a)
                            {
                                if (dtInstrumen.Columns.Count > 2)
                                {
                                    dtInstrumen.Columns["Tstamp"].SetOrdinal(0);
                                }
                            }
                            bool b = dtInstrumen.Columns.Contains("DateTime");
                            if (b)
                            {
                                if (dtInstrumen.Columns.Count > 2)
                                {
                                    dtInstrumen.Columns["DateTime"].SetOrdinal(0);
                                }
                            }
                        }
                        #region 数据*1000
                        if (dtInstrumen.Columns.Contains("O₃"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;

                                if (decimal.TryParse(dtInstrumen.Rows[i]["O₃"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["O₃"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("PM₁₀"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;

                                if (decimal.TryParse(dtInstrumen.Rows[i]["PM₁₀"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["PM₁₀"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("PM₂.₅"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["PM₂.₅"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["PM₂.₅"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("TSP"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["TSP"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["TSP"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("PM₁.₀"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["PM₁.₀"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["PM₁.₀"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NOᵧ"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NOᵧ"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NO"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NO"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NO"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NOₓ"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NOₓ"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NOₓ"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NOᵧ₁"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ₁"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NOᵧ₁"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NOᵧ₂"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ₂"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NOᵧ₂"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        #endregion
                        string Time = "[";
                        for (int i = 0; i < dtInstrumen.Rows.Count; i++)
                        {
                            //Time += "'" + string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Year, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Month - 1, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Day, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Hour, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Minute, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Second, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Millisecond) + "',";
                            Time += "'" + Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).ToString("MM/dd HH时") + "',";
                        }
                        Time = Time.TrimEnd(',');
                        Time += "]";
                        string data = "[{connectNulls:true,";
                        for (int i = 0; i < listTypeName.ToArray().Length; i++)
                        {

                            if ("Thermal OC;Thermal EC;OptEC;OptOC;TC".Contains(dtInstrumen.Columns[i + 1].ColumnName))
                            {
                                data += "yAxis: 0,tooltip: { valueSuffix: 'μgC/m3' },smooth: true,name:'" + dtInstrumen.Columns[i + 1].ColumnName + "',data:[";
                            }
                            else if (dtInstrumen.Columns[i + 1].ColumnName.Contains("μm"))
                            {
                                data += "yAxis: 3,tooltip: { valueSuffix: '个/L' },smooth: true,name:'" + dtInstrumen.Columns[i + 1].ColumnName + "',data:[";
                            }
                            else if (dtInstrumen.Columns[i + 1].ColumnName == "CO")
                            {
                                data += "yAxis: 2,tooltip: { valueSuffix: 'mg/m3' },smooth: true,name:'" + dtInstrumen.Columns[i + 1].ColumnName + "',data:[";
                            }
                            else if (dtInstrumen.Columns[i + 1].ColumnName.Contains("nm"))
                            {
                                data += "yAxis: 4,tooltip: { valueSuffix: '' },smooth: true,name:'" + dtInstrumen.Columns[i + 1].ColumnName + "',data:[";
                            }
                            else
                            {
                                data += "yAxis: 1,tooltip: { valueSuffix: 'μg/m3' },smooth: true,name:'" + dtInstrumen.Columns[i + 1].ColumnName + "',data:[";

                            }
                            for (int j = 0; j < dtInstrumen.Rows.Count; j++)
                            {
                                if (dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() == "" || dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() == "0")
                                {
                                    data += "null,";
                                }
                                else
                                {
                                    data += dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() + ",";
                                }
                            }
                            data += "]}, {connectNulls:true,";
                        }
                        data = data.Substring(0, data.Length - 21);
                        data += "]";
                        hdDate.Value = data;
                        hdDTime.Value = Time;
                    }
                    #endregion
                }

                if (ddlDataSource.SelectedValue == "AuditData")
                {

                    #region//小时数据
                    if (radlDataType.SelectedValue == "Hour")
                    {
                        DateTime dtBegion = Convert.ToDateTime(hourBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:00"));
                        DateTime dtEnd = Convert.ToDateTime(hourEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:00"));
                        auditData = m_HourData.GetNewHourDataPagerAvg(portIdsCG, pfactors.Select(p => p.PollutantCode).ToArray(), dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");
                        SetHiddenData(portIds, pfactors, dtBegion, dtEnd);
                        dt = auditData.ToTable();
                        for (int i = 0; i < pfactors.Select(p => p.PollutantCode).ToArray().Length; i++)
                        {
                            dt.Columns[pfactors.Select(p => p.PollutantCode).ToArray()[i]].ColumnName = m_AirPollutantService.GetPollutantInfo(pfactors.Select(p => p.PollutantCode).ToArray()[i]).PollutantName;
                            listTypeName.Add(m_AirPollutantService.GetPollutantInfo(pfactors.Select(p => p.PollutantCode).ToArray()[i]).PollutantName);
                        }
                        dtInstrumen = dt;
                        if (dtInstrumen != null)
                        {
                            bool a = dtInstrumen.Columns.Contains("Tstamp");
                            if (a)
                            {
                                if (dtInstrumen.Columns.Count > 2)
                                {
                                    dtInstrumen.Columns["Tstamp"].SetOrdinal(0);
                                }
                            }
                            bool b = dtInstrumen.Columns.Contains("DateTime");
                            if (b)
                            {
                                if (dtInstrumen.Columns.Count > 2)
                                {
                                    dtInstrumen.Columns["DateTime"].SetOrdinal(0);
                                }
                            }
                        }
                        #region 数据*1000
                        if (dtInstrumen.Columns.Contains("O₃"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;

                                if (decimal.TryParse(dtInstrumen.Rows[i]["O₃"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["O₃"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("PM₁₀"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;

                                if (decimal.TryParse(dtInstrumen.Rows[i]["PM₁₀"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["PM₁₀"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("PM₂.₅"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["PM₂.₅"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["PM₂.₅"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("TSP"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["TSP"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["TSP"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("PM₁.₀"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["PM₁.₀"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["PM₁.₀"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NOᵧ"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NOᵧ"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NO"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NO"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NO"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NOₓ"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NOₓ"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NOₓ"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NOᵧ₁"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ₁"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NOᵧ₁"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NOᵧ₂"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ₂"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NOᵧ₂"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        #endregion
                        string Time = "[";
                        for (int i = 0; i < dtInstrumen.Rows.Count; i++)
                        {
                            //Time += "'" + string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Year, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Month - 1, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Day, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Hour, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Minute, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Second, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Millisecond) + "',";
                            Time += "'" + Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).ToString("MM/dd HH时") + "',";
                        }
                        Time = Time.TrimEnd(',');
                        Time += "]";
                        string data = "[{connectNulls:true,";
                        for (int i = 0; i < listTypeName.ToArray().Length; i++)
                        {

                            if ("Thermal OC;Thermal EC;OptEC;OptOC;TC".Contains(dtInstrumen.Columns[i + 1].ColumnName))
                            {
                                data += "yAxis: 0,tooltip: { valueSuffix: 'μgC/m3' },smooth: true,name:'" + dtInstrumen.Columns[i + 1].ColumnName + "',data:[";
                            }
                            else if (dtInstrumen.Columns[i + 1].ColumnName.Contains("μm"))
                            {
                                data += "yAxis: 3,tooltip: { valueSuffix: '个/L' },smooth: true,name:'" + dtInstrumen.Columns[i + 1].ColumnName + "',data:[";
                            }
                            else if (dtInstrumen.Columns[i + 1].ColumnName == "CO")
                            {
                                data += "yAxis: 2,tooltip: { valueSuffix: 'mg/m3' },smooth: true,name:'" + dtInstrumen.Columns[i + 1].ColumnName + "',data:[";
                            }
                            else if (dtInstrumen.Columns[i + 1].ColumnName.Contains("nm"))
                            {
                                data += "yAxis: 4,tooltip: { valueSuffix: '' },smooth: true,name:'" + dtInstrumen.Columns[i + 1].ColumnName + "',data:[";
                            }
                            else
                            {
                                data += "yAxis: 1,tooltip: { valueSuffix: 'μg/m3' },smooth: true,name:'" + dtInstrumen.Columns[i + 1].ColumnName + "',data:[";

                            }
                            for (int j = 0; j < dtInstrumen.Rows.Count; j++)
                            {
                                if (dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() == "" || dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() == "0")
                                {
                                    data += "null,";
                                }
                                else
                                {
                                    data += dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() + ",";
                                }
                            }
                            data += "]}, {connectNulls:true,";
                        }
                        data = data.Substring(0, data.Length - 21);
                        data += "]";
                        hdDate.Value = data;
                        hdDTime.Value = Time;
                    }
                    #endregion

                    #region //日数据
                    else if (radlDataType.SelectedValue == "Day")
                    {
                        DateTime dtBegion = dayBegin.SelectedDate.Value;
                        DateTime dtEnd = dayEnd.SelectedDate.Value;
                        auditData = m_DayData.GetAvgDayDataPager(portIdsCG, pfactors, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
                        SetHiddenData(portIds, pfactors, dtBegion, dtEnd);
                        dt = auditData.ToTable();
                        for (int i = 0; i < pfactors.Select(p => p.PollutantCode).ToArray().Length; i++)
                        {
                            dt.Columns[pfactors.Select(p => p.PollutantCode).ToArray()[i]].ColumnName = m_AirPollutantService.GetPollutantInfo(pfactors.Select(p => p.PollutantCode).ToArray()[i]).PollutantName;
                            listTypeName.Add(m_AirPollutantService.GetPollutantInfo(pfactors.Select(p => p.PollutantCode).ToArray()[i]).PollutantName);
                        }
                        dtInstrumen = dt;
                        if (dtInstrumen != null)
                        {
                            bool a = dtInstrumen.Columns.Contains("Tstamp");
                            if (a)
                            {
                                if (dtInstrumen.Columns.Count > 2)
                                {
                                    dtInstrumen.Columns["Tstamp"].SetOrdinal(0);
                                }
                            }
                            bool b = dtInstrumen.Columns.Contains("DateTime");
                            if (b)
                            {
                                if (dtInstrumen.Columns.Count > 2)
                                {
                                    dtInstrumen.Columns["DateTime"].SetOrdinal(0);
                                }
                            }
                        }
                        #region 数据*1000
                        if (dtInstrumen.Columns.Contains("O₃"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;

                                if (decimal.TryParse(dtInstrumen.Rows[i]["O₃"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["O₃"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("PM₁₀"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;

                                if (decimal.TryParse(dtInstrumen.Rows[i]["PM₁₀"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["PM₁₀"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("PM₂.₅"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["PM₂.₅"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["PM₂.₅"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("TSP"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["TSP"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["TSP"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("PM₁.₀"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["PM₁.₀"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["PM₁.₀"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NOᵧ"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NOᵧ"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NO"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NO"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NO"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NOₓ"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NOₓ"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NOₓ"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NOᵧ₁"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ₁"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NOᵧ₁"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NOᵧ₂"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ₂"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NOᵧ₂"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        #endregion
                        string Time = "[";
                        for (int i = 0; i < dtInstrumen.Rows.Count; i++)
                        {
                            //Time += "'" + string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Year, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Month - 1, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Day, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Hour, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Minute, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Second, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Millisecond) + "',";
                            Time += "'" + Convert.ToDateTime(dtInstrumen.Rows[i]["DateTime"].ToString()).ToString("yyyy/MM/dd") + "',";
                        }
                        Time = Time.TrimEnd(',');
                        Time += "]";
                        string data = "[{connectNulls:true,";
                        for (int i = 0; i < listTypeName.ToArray().Length; i++)
                        {

                            if ("Thermal OC;Thermal EC;OptEC;OptOC;TC".Contains(dtInstrumen.Columns[i + 1].ColumnName))
                            {
                                data += "yAxis: 0,tooltip: { valueSuffix: 'μgC/m3' },smooth: true,name:'" + dtInstrumen.Columns[i + 1].ColumnName + "',data:[";
                            }
                            else if (dtInstrumen.Columns[i + 1].ColumnName.Contains("μm"))
                            {
                                data += "yAxis: 3,tooltip: { valueSuffix: '个/L' },smooth: true,name:'" + dtInstrumen.Columns[i + 1].ColumnName + "',data:[";
                            }
                            else if (dtInstrumen.Columns[i + 1].ColumnName == "CO")
                            {
                                data += "yAxis: 2,tooltip: { valueSuffix: 'mg/m3' },smooth: true,name:'" + dtInstrumen.Columns[i + 1].ColumnName + "',data:[";
                            }
                            else if (dtInstrumen.Columns[i + 1].ColumnName.Contains("nm"))
                            {
                                data += "yAxis: 4,tooltip: { valueSuffix: '' },smooth: true,name:'" + dtInstrumen.Columns[i + 1].ColumnName + "',data:[";
                            }
                            else
                            {
                                data += "yAxis: 1,tooltip: { valueSuffix: 'μg/m3' },smooth: true,name:'" + dtInstrumen.Columns[i + 1].ColumnName + "',data:[";

                            }
                            for (int j = 0; j < dtInstrumen.Rows.Count; j++)
                            {
                                if (dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() == "" || dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() == "0")
                                {
                                    data += "null,";
                                }
                                else
                                {
                                    data += dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() + ",";
                                }
                            }
                            data += "]}, {connectNulls:true,";
                        }
                        data = data.Substring(0, data.Length - 21);
                        data += "]";
                        hdDate.Value = data;
                        hdDTime.Value = Time;
                    }
                    #endregion

                    #region//月数据
                    else if (radlDataType.SelectedValue == "Month")
                    {
                        DateTime dtBegion = monthBegin.SelectedDate.Value;
                        DateTime dtEnd = monthEnd.SelectedDate.Value;
                        int monthB = monthBegin.SelectedDate.Value.Year;
                        int monthE = monthEnd.SelectedDate.Value.Year;
                        int monthF = monthBegin.SelectedDate.Value.Month;
                        int monthT = monthEnd.SelectedDate.Value.Month;
                        //【给隐藏域赋值，用于显示Chart】
                        SetHiddenData(portIds, pfactors, monthB + ";" + monthF + ";" + monthE + ";" + monthT);
                        auditData = m_MonthData.GetMonthDataPagerAvg(portIdsCG, pfactors.Select(p => p.PollutantCode).ToArray(), dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Year asc,MonthOfYear asc");
                        
                        dt = auditData.ToTable();
                        for (int i = 0; i < pfactors.Select(p => p.PollutantCode).ToArray().Length; i++)
                        {
                            dt.Columns[pfactors.Select(p => p.PollutantCode).ToArray()[i]].ColumnName = m_AirPollutantService.GetPollutantInfo(pfactors.Select(p => p.PollutantCode).ToArray()[i]).PollutantName;
                            listTypeName.Add(m_AirPollutantService.GetPollutantInfo(pfactors.Select(p => p.PollutantCode).ToArray()[i]).PollutantName);
                        }
                        dtInstrumen = dt;
                        if (dtInstrumen != null)
                        {
                            bool a = dtInstrumen.Columns.Contains("Tstamp");
                            if (a)
                            {
                                if (dtInstrumen.Columns.Count > 2)
                                {
                                    dtInstrumen.Columns["Tstamp"].SetOrdinal(0);
                                }
                            }
                            bool b = dtInstrumen.Columns.Contains("DateTime");
                            if (b)
                            {
                                if (dtInstrumen.Columns.Count > 2)
                                {
                                    dtInstrumen.Columns["DateTime"].SetOrdinal(0);
                                }
                            }
                        }
                        #region 数据*1000
                        if (dtInstrumen.Columns.Contains("O₃"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;

                                if (decimal.TryParse(dtInstrumen.Rows[i]["O₃"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["O₃"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("PM₁₀"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;

                                if (decimal.TryParse(dtInstrumen.Rows[i]["PM₁₀"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["PM₁₀"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("PM₂.₅"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["PM₂.₅"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["PM₂.₅"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("TSP"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["TSP"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["TSP"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("PM₁.₀"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["PM₁.₀"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["PM₁.₀"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NOᵧ"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NOᵧ"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NO"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NO"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NO"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NOₓ"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NOₓ"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NOₓ"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NOᵧ₁"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ₁"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NOᵧ₁"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NOᵧ₂"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ₂"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NOᵧ₂"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        #endregion
                        string Time = "[";
                        for (int i = 0; i < dtInstrumen.Rows.Count; i++)
                        {
                            //Time += "'" + string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Year, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Month - 1, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Day, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Hour, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Minute, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Second, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Millisecond) + "',";
                            Time += "'" + dtInstrumen.Rows[i]["Year"].ToString() + "/" + dtInstrumen.Rows[i]["MonthOfYear"].ToString() + "',";
                        }
                        Time = Time.TrimEnd(',');
                        Time += "]";
                        string data = "[{connectNulls:true,";
                        for (int i = 0; i < listTypeName.ToArray().Length; i++)
                        {

                            if ("Thermal OC;Thermal EC;OptEC;OptOC;TC".Contains(dtInstrumen.Columns[i + 2].ColumnName))
                            {
                                data += "yAxis: 0,tooltip: { valueSuffix: 'μgC/m3' },smooth: true,name:'" + dtInstrumen.Columns[i + 2].ColumnName + "',data:[";
                            }
                            else if (dtInstrumen.Columns[i + 2].ColumnName.Contains("μm"))
                            {
                                data += "yAxis: 3,tooltip: { valueSuffix: '个/L' },smooth: true,name:'" + dtInstrumen.Columns[i + 2].ColumnName + "',data:[";
                            }
                            else if (dtInstrumen.Columns[i + 2].ColumnName == "CO")
                            {
                                data += "yAxis: 2,tooltip: { valueSuffix: 'mg/m3' },smooth: true,name:'" + dtInstrumen.Columns[i + 2].ColumnName + "',data:[";
                            }
                            else if (dtInstrumen.Columns[i + 2].ColumnName.Contains("nm"))
                            {
                                data += "yAxis: 4,tooltip: { valueSuffix: '' },smooth: true,name:'" + dtInstrumen.Columns[i + 2].ColumnName + "',data:[";
                            }
                            else
                            {
                                data += "yAxis: 1,tooltip: { valueSuffix: 'μg/m3' },smooth: true,name:'" + dtInstrumen.Columns[i + 2].ColumnName + "',data:[";

                            }
                            for (int j = 0; j < dtInstrumen.Rows.Count; j++)
                            {
                                if (dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() == "" || dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() == "0")
                                {
                                    data += "null,";
                                }
                                else
                                {
                                    data += dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() + ",";
                                }
                            }
                            data += "]}, {connectNulls:true,";
                        }
                        data = data.Substring(0, data.Length - 21);
                        data += "]";
                        hdDate.Value = data;
                        hdDTime.Value = Time;
                    }
                    #endregion

                    #region//季数据
                    else if (radlDataType.SelectedValue == "Season")
                    {
                        int seasonB = seasonBegin.SelectedDate.Value.Year;
                        int seasonE = seasonEnd.SelectedDate.Value.Year;
                        int seasonF = Convert.ToInt32(seasonFrom.SelectedValue);
                        int seasonT = Convert.ToInt32(seasonTo.SelectedValue);
                        //【给隐藏域赋值，用于显示Chart】
                        SetHiddenData(portIds, pfactors, seasonB + ";" + seasonF + ";" + seasonE + ";" + seasonT);
                        auditData = m_SeasonData.GetSeasonDataPagerAvg(portIdsCG, pfactors, seasonB, seasonF, seasonE, seasonT, pageSize, pageNo, out recordTotal, "PointId asc,Year asc,SeasonOfYear asc");
                        

                        dt = auditData.ToTable();
                        for (int i = 0; i < pfactors.Select(p => p.PollutantCode).ToArray().Length; i++)
                        {
                            dt.Columns[pfactors.Select(p => p.PollutantCode).ToArray()[i]].ColumnName = m_AirPollutantService.GetPollutantInfo(pfactors.Select(p => p.PollutantCode).ToArray()[i]).PollutantName;
                            listTypeName.Add(m_AirPollutantService.GetPollutantInfo(pfactors.Select(p => p.PollutantCode).ToArray()[i]).PollutantName);
                        }
                        dtInstrumen = dt;
                        if (dtInstrumen != null)
                        {
                            bool a = dtInstrumen.Columns.Contains("Tstamp");
                            if (a)
                            {
                                if (dtInstrumen.Columns.Count > 2)
                                {
                                    dtInstrumen.Columns["Tstamp"].SetOrdinal(0);
                                }
                            }
                            bool b = dtInstrumen.Columns.Contains("DateTime");
                            if (b)
                            {
                                if (dtInstrumen.Columns.Count > 2)
                                {
                                    dtInstrumen.Columns["DateTime"].SetOrdinal(0);
                                }
                            }
                        }
                        #region 数据*1000
                        if (dtInstrumen.Columns.Contains("O₃"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;

                                if (decimal.TryParse(dtInstrumen.Rows[i]["O₃"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["O₃"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("PM₁₀"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;

                                if (decimal.TryParse(dtInstrumen.Rows[i]["PM₁₀"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["PM₁₀"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("PM₂.₅"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["PM₂.₅"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["PM₂.₅"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("TSP"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["TSP"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["TSP"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("PM₁.₀"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["PM₁.₀"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["PM₁.₀"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NOᵧ"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NOᵧ"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NO"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NO"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NO"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NOₓ"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NOₓ"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NOₓ"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NOᵧ₁"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ₁"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NOᵧ₁"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NOᵧ₂"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ₂"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NOᵧ₂"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        #endregion
                        string Time = "[";
                        for (int i = 0; i < dtInstrumen.Rows.Count; i++)
                        {
                            //Time += "'" + string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Year, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Month - 1, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Day, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Hour, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Minute, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Second, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Millisecond) + "',";
                            Time += "'" + dtInstrumen.Rows[i]["Year"].ToString() + "年第" + dtInstrumen.Rows[i]["SeasonOfYear"].ToString() + "季',";
                        }
                        Time = Time.TrimEnd(',');
                        Time += "]";
                        string data = "[{connectNulls:true,";
                        for (int i = 0; i < listTypeName.ToArray().Length; i++)
                        {

                            if ("Thermal OC;Thermal EC;OptEC;OptOC;TC".Contains(dtInstrumen.Columns[i + 2].ColumnName))
                            {
                                data += "yAxis: 0,tooltip: { valueSuffix: 'μgC/m3' },smooth: true,name:'" + dtInstrumen.Columns[i + 2].ColumnName + "',data:[";
                            }
                            else if (dtInstrumen.Columns[i + 2].ColumnName.Contains("μm"))
                            {
                                data += "yAxis: 3,tooltip: { valueSuffix: '个/L' },smooth: true,name:'" + dtInstrumen.Columns[i + 2].ColumnName + "',data:[";
                            }
                            else if (dtInstrumen.Columns[i + 2].ColumnName == "CO")
                            {
                                data += "yAxis: 2,tooltip: { valueSuffix: 'mg/m3' },smooth: true,name:'" + dtInstrumen.Columns[i + 2].ColumnName + "',data:[";
                            }
                            else if (dtInstrumen.Columns[i + 2].ColumnName.Contains("nm"))
                            {
                                data += "yAxis: 4,tooltip: { valueSuffix: '' },smooth: true,name:'" + dtInstrumen.Columns[i + 2].ColumnName + "',data:[";
                            }
                            else
                            {
                                data += "yAxis: 1,tooltip: { valueSuffix: 'μg/m3' },smooth: true,name:'" + dtInstrumen.Columns[i + 2].ColumnName + "',data:[";

                            }
                            for (int j = 0; j < dtInstrumen.Rows.Count; j++)
                            {
                                if (dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() == "" || dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() == "0")
                                {
                                    data += "null,";
                                }
                                else
                                {
                                    data += dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() + ",";
                                }
                            }
                            data += "]}, {connectNulls:true,";
                        }
                        data = data.Substring(0, data.Length - 21);
                        data += "]";
                        hdDate.Value = data;
                        hdDTime.Value = Time;
                    }
                    #endregion

                    #region//年数据
                    else if (radlDataType.SelectedValue == "Year")
                    {

                        int yearB = yearBegin.SelectedDate.Value.Year;
                        int yearE = yearEnd.SelectedDate.Value.Year;
                        //【给隐藏域赋值，用于显示Chart】
                        SetHiddenData(portIds, pfactors, yearB + ";" + yearE);
                        auditData = m_YearData.GetYearDataPagerAvg(portIdsCG, pfactors, yearB, yearE, pageSize, pageNo, out recordTotal, "PointId asc,Year asc");
                        

                        dt = auditData.ToTable();
                        for (int i = 0; i < pfactors.Select(p => p.PollutantCode).ToArray().Length; i++)
                        {
                            dt.Columns[pfactors.Select(p => p.PollutantCode).ToArray()[i]].ColumnName = m_AirPollutantService.GetPollutantInfo(pfactors.Select(p => p.PollutantCode).ToArray()[i]).PollutantName;
                            listTypeName.Add(m_AirPollutantService.GetPollutantInfo(pfactors.Select(p => p.PollutantCode).ToArray()[i]).PollutantName);
                        }
                        dtInstrumen = dt;
                        if (dtInstrumen != null)
                        {
                            bool a = dtInstrumen.Columns.Contains("Tstamp");
                            if (a)
                            {
                                if (dtInstrumen.Columns.Count > 2)
                                {
                                    dtInstrumen.Columns["Tstamp"].SetOrdinal(0);
                                }
                            }
                            bool b = dtInstrumen.Columns.Contains("DateTime");
                            if (b)
                            {
                                if (dtInstrumen.Columns.Count > 2)
                                {
                                    dtInstrumen.Columns["DateTime"].SetOrdinal(0);
                                }
                            }
                        }
                        #region 数据*1000
                        if (dtInstrumen.Columns.Contains("O₃"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;

                                if (decimal.TryParse(dtInstrumen.Rows[i]["O₃"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["O₃"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("PM₁₀"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;

                                if (decimal.TryParse(dtInstrumen.Rows[i]["PM₁₀"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["PM₁₀"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("PM₂.₅"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["PM₂.₅"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["PM₂.₅"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("TSP"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["TSP"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["TSP"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("PM₁.₀"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["PM₁.₀"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["PM₁.₀"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NOᵧ"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NOᵧ"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NO"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NO"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NO"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NOₓ"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NOₓ"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NOₓ"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NOᵧ₁"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ₁"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NOᵧ₁"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NOᵧ₂"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ₂"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NOᵧ₂"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        #endregion
                        string Time = "[";
                        for (int i = 0; i < dtInstrumen.Rows.Count; i++)
                        {
                            //Time += "'" + string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Year, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Month - 1, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Day, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Hour, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Minute, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Second, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Millisecond) + "',";
                            Time += "'" + dtInstrumen.Rows[i]["Year"].ToString() + "年'," ;
                        }
                        Time = Time.TrimEnd(',');
                        Time += "]";
                        string data = "[{connectNulls:true,";
                        for (int i = 0; i < listTypeName.ToArray().Length; i++)
                        {

                            if ("Thermal OC;Thermal EC;OptEC;OptOC;TC".Contains(dtInstrumen.Columns[i + 1].ColumnName))
                            {
                                data += "yAxis: 0,tooltip: { valueSuffix: 'μgC/m3' },smooth: true,name:'" + dtInstrumen.Columns[i + 1].ColumnName + "',data:[";
                            }
                            else if (dtInstrumen.Columns[i + 1].ColumnName.Contains("μm"))
                            {
                                data += "yAxis: 3,tooltip: { valueSuffix: '个/L' },smooth: true,name:'" + dtInstrumen.Columns[i + 1].ColumnName + "',data:[";
                            }
                            else if (dtInstrumen.Columns[i + 1].ColumnName == "CO")
                            {
                                data += "yAxis: 2,tooltip: { valueSuffix: 'mg/m3' },smooth: true,name:'" + dtInstrumen.Columns[i + 1].ColumnName + "',data:[";
                            }
                            else if (dtInstrumen.Columns[i + 1].ColumnName.Contains("nm"))
                            {
                                data += "yAxis: 4,tooltip: { valueSuffix: '' },smooth: true,name:'" + dtInstrumen.Columns[i + 1].ColumnName + "',data:[";
                            }
                            else
                            {
                                data += "yAxis: 1,tooltip: { valueSuffix: 'μg/m3' },smooth: true,name:'" + dtInstrumen.Columns[i + 1].ColumnName + "',data:[";

                            }
                            for (int j = 0; j < dtInstrumen.Rows.Count; j++)
                            {
                                if (dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() == "" || dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() == "0")
                                {
                                    data += "null,";
                                }
                                else
                                {
                                    data += dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() + ",";
                                }
                            }
                            data += "]}, {connectNulls:true,";
                        }
                        data = data.Substring(0, data.Length - 21);
                        data += "]";
                        hdDate.Value = data;
                        hdDTime.Value = Time;

                    }
                    #endregion

                    #region//周数据
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
                        //【给隐藏域赋值，用于显示Chart】
                        SetHiddenData(portIds, pfactors, weekB + ";" + weekF + ";" + weekE + ";" + weekT);
                        auditData = m_WeekData.GetWeekDataPagerAvg(portIdsCG, pfactors, weekB, weekF, weekE, weekT, pageSize, pageNo, out recordTotal, "PointId asc,Year asc,WeekOfYear asc");
                        

                        dt = auditData.ToTable();
                        for (int i = 0; i < pfactors.Select(p => p.PollutantCode).ToArray().Length; i++)
                        {
                            dt.Columns[pfactors.Select(p => p.PollutantCode).ToArray()[i]].ColumnName = m_AirPollutantService.GetPollutantInfo(pfactors.Select(p => p.PollutantCode).ToArray()[i]).PollutantName;
                            listTypeName.Add(m_AirPollutantService.GetPollutantInfo(pfactors.Select(p => p.PollutantCode).ToArray()[i]).PollutantName);
                        }
                        dtInstrumen = dt;
                        if (dtInstrumen != null)
                        {
                            bool a = dtInstrumen.Columns.Contains("Tstamp");
                            if (a)
                            {
                                if (dtInstrumen.Columns.Count > 2)
                                {
                                    dtInstrumen.Columns["Tstamp"].SetOrdinal(0);
                                }
                            }
                            bool b = dtInstrumen.Columns.Contains("DateTime");
                            if (b)
                            {
                                if (dtInstrumen.Columns.Count > 2)
                                {
                                    dtInstrumen.Columns["DateTime"].SetOrdinal(0);
                                }
                            }
                        }
                        #region 数据*1000
                        if (dtInstrumen.Columns.Contains("O₃"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;

                                if (decimal.TryParse(dtInstrumen.Rows[i]["O₃"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["O₃"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("PM₁₀"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;

                                if (decimal.TryParse(dtInstrumen.Rows[i]["PM₁₀"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["PM₁₀"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("PM₂.₅"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["PM₂.₅"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["PM₂.₅"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("TSP"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["TSP"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["TSP"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("PM₁.₀"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["PM₁.₀"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["PM₁.₀"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NOᵧ"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NOᵧ"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NO"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NO"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NO"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NOₓ"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NOₓ"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NOₓ"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NOᵧ₁"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ₁"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NOᵧ₁"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        if (dtInstrumen.Columns.Contains("NOᵧ₂"))
                        {
                            for (int i = 0; i < dtInstrumen.Rows.Count; i++) //臭氧的数据要乘以1000
                            {
                                decimal value = 0M;
                                if (decimal.TryParse(dtInstrumen.Rows[i]["NOᵧ₂"].ToString(), out value))
                                {
                                    dtInstrumen.Rows[i]["NOᵧ₂"] = DecimalExtension.GetPollutantValue(value * 1000, 0);
                                }
                            }
                        }
                        #endregion
                        string Time = "[";
                        for (int i = 0; i < dtInstrumen.Rows.Count; i++)
                        {
                            //Time += "'" + string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Year, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Month - 1, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Day, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Hour, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Minute, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Second, Convert.ToDateTime(dtInstrumen.Rows[i]["Tstamp"].ToString()).Millisecond) + "',";
                            Time += "'" + dtInstrumen.Rows[i]["Year"].ToString() + "年第" + dtInstrumen.Rows[i]["WeekOfYear"].ToString() + "周',";
                        }
                        Time = Time.TrimEnd(',');
                        Time += "]";
                        string data = "[{connectNulls:true,";
                        for (int i = 0; i < listTypeName.ToArray().Length; i++)
                        {

                            if ("Thermal OC;Thermal EC;OptEC;OptOC;TC".Contains(dtInstrumen.Columns[i + 2].ColumnName))
                            {
                                data += "yAxis: 0,tooltip: { valueSuffix: 'μgC/m3' },smooth: true,name:'" + dtInstrumen.Columns[i + 2].ColumnName + "',data:[";
                            }
                            else if (dtInstrumen.Columns[i + 2].ColumnName.Contains("μm"))
                            {
                                data += "yAxis: 3,tooltip: { valueSuffix: '个/L' },smooth: true,name:'" + dtInstrumen.Columns[i + 2].ColumnName + "',data:[";
                            }
                            else if (dtInstrumen.Columns[i + 2].ColumnName == "CO")
                            {
                                data += "yAxis: 2,tooltip: { valueSuffix: 'mg/m3' },smooth: true,name:'" + dtInstrumen.Columns[i + 2].ColumnName + "',data:[";
                            }
                            else if (dtInstrumen.Columns[i + 2].ColumnName.Contains("nm"))
                            {
                                data += "yAxis: 4,tooltip: { valueSuffix: '' },smooth: true,name:'" + dtInstrumen.Columns[i + 2].ColumnName + "',data:[";
                            }
                            else
                            {
                                data += "yAxis: 1,tooltip: { valueSuffix: 'μg/m3' },smooth: true,name:'" + dtInstrumen.Columns[i + 2].ColumnName + "',data:[";

                            }
                            for (int j = 0; j < dtInstrumen.Rows.Count; j++)
                            {
                                if (dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() == "" || dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() == "0")
                                {
                                    data += "null,";
                                }
                                else
                                {
                                    data += dtInstrumen.Rows[j][listTypeName.ToArray()[i]].ToString() + ",";
                                }
                            }
                            data += "]}, {connectNulls:true,";
                        }
                        data = data.Substring(0, data.Length - 21);
                        data += "]";
                        hdDate.Value = data;
                        hdDTime.Value = Time;
                    }
                    #endregion
                }

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
            if (ddlDataSource.SelectedValue == "OriData")
            {
                HiddenData.Value = string.Join(";", portIds) + "|" + string.Join(";", factors.Select(p => p.PollutantCode).ToArray())
                                 + "|" + dtBegin + "|" + dtEnd + "|" + radlDataTypeOri.SelectedValue + "|Air";

            }
            if (ddlDataSource.SelectedValue == "AuditData")
            {
                HiddenData.Value = string.Join(";", portIds) + "|" + string.Join(";", factors.Select(p => p.PollutantCode).ToArray())
                                 + "|" + dtBegin + "|" + dtEnd + "|" + radlDataType.SelectedValue + "|Air";

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
            if (ddlDataSource.SelectedValue == "OriData")
            {
                HiddenData.Value = string.Join(";", portIds) + "|" + string.Join(";", factors.Select(p => p.PollutantCode).ToArray())
                                 + "|" + timeStr + "|" + "|" + radlDataTypeOri.SelectedValue + "|Air";
            }
            if (ddlDataSource.SelectedValue == "AuditData")
            {
                HiddenData.Value = string.Join(";", portIds) + "|" + string.Join(";", factors.Select(p => p.PollutantCode).ToArray())
                                 + "|" + timeStr + "|" + "|" + radlDataType.SelectedValue + "|Air";
            }
        }
        /// <summary>
        /// 原始数据数据类型时间框选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void radlDataTypeOri_SelectedIndexChanged(object sender, EventArgs e)
        {
            //一分钟数据
            if (radlDataTypeOri.SelectedValue == "Min1")
            {
                dtpHour.Visible = true;
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
                dtpHour.Visible = true;
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
                dtpHour.Visible = true;
                dbtHour.Visible = false;
                dbtDay.Visible = false;
                dbtMonth.Visible = false;
                dbtSeason.Visible = false;
                dbtYear.Visible = false;
                dbtWeek.Visible = false;
                dtpBegin.DateInput.DateFormat = "yyyy-MM-dd";
                dtpEnd.DateInput.DateFormat = "yyyy-MM-dd";
            }
            //月数据
            if (radlDataTypeOri.SelectedValue == "OriMonth")
            {
                dtpHour.Visible = true;
                dbtHour.Visible = false;
                dbtDay.Visible = false;
                dbtMonth.Visible = false;
                dbtSeason.Visible = false;
                dbtYear.Visible = false;
                dbtWeek.Visible = false;
                dtpBegin.DateInput.DateFormat = "yyyy-MM";
                dtpEnd.DateInput.DateFormat = "yyyy-MM";
            }
        }
        /// <summary>
        /// 审核数据数据类型时间框选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void radlDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //小时数据
            if (radlDataType.SelectedValue == "Hour")
            {
                dtpHour.Visible = false;
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
                dbtYear.Visible = true;
                dbtMonth.Visible = false;
                dbtDay.Visible = false;
                dbtHour.Visible = false;
                dbtWeek.Visible = false;
                dbtSeason.Visible = false;
            }
        }
        /// <summary>
        /// 数据来源选项变化，数据类型选项相应变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlDataSource_SelectedIndexChanged(object sender, Telerik.Web.UI.DropDownListEventArgs e)
        {
            if (ddlDataSource.SelectedIndex == 0)
            {
                radlDataTypeOri.Visible = true;
                radlDataType.Visible = false;
                radlDataTypeOri.SelectedIndex = 0;
                dtpHour.Visible = true;
                dbtHour.Visible = false;
                dbtDay.Visible = false;
                dbtMonth.Visible = false;
                dbtSeason.Visible = false;
                dbtYear.Visible = false;
                dbtWeek.Visible = false;
                dtpBegin.DateInput.DateFormat = "yyyy-MM-dd HH:00";
                dtpEnd.DateInput.DateFormat = "yyyy-MM-dd HH:00";
            }
            else
            {
                radlDataTypeOri.Visible = false;
                radlDataType.Visible = true;
                dtpHour.Visible = false;
                dbtHour.Visible = true;
                dbtDay.Visible = false;
                dbtMonth.Visible = false;
                dbtYear.Visible = false;
                dbtWeek.Visible = false;
                dbtSeason.Visible = false;
                radlDataType.SelectedIndex = 0;
            }
        }


        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            BindData();
            RegisterScript("ReChart();");

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

        protected void ddlLiJing_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            string factors = System.Configuration.ConfigurationManager.AppSettings["PollutantCode"];
            //if (ddlLiJing.SelectedValue == "0")
            //{
            //    factors += System.Configuration.ConfigurationManager.AppSettings["PollutantCode1"];
            //}
            //if (ddlLiJing.SelectedValue == "1")
            //{
            //    factors += System.Configuration.ConfigurationManager.AppSettings["PollutantCode2"];
            //}
            //if (ddlLiJing.SelectedValue == "2")
            //{
            //    factors += System.Configuration.ConfigurationManager.AppSettings["PollutantCode3"];
            //}
            factorCbxRsm.SetFactorValuesFromCodes(factors.Trim(';'));
        }
    }
}