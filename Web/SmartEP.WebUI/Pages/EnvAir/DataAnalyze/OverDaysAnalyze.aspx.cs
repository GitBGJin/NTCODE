﻿using SmartEP.Core.Generic;
using SmartEP.Service.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmartEP.Service.Core.Enums;
using Telerik.Web.UI;
using SmartEP.Core.Enums;
using SmartEP.Utilities.Calendar;
using SmartEP.Service.DataAnalyze.Air.AQIReport;
using System.Data;
using SmartEP.Core.Interfaces;
using SmartEP.Service.BaseData.MPInfo;
using Aspose.Cells;
using SmartEP.Utilities.Office;
using SmartEP.DomainModel;
using System.Configuration;
using SmartEP.Utilities.IO;

namespace SmartEP.WebUI.Pages.EnvAir.DataAnalyze
{
    /// <summary>
    /// 名称：OverDaysAnalyze.aspx.cs
    /// 创建人：樊垂贺
    /// 创建日期：2016-01-21
    /// 维护人员：
    /// 最新维护人员：徐阳
    /// 最新维护日期：2017-06-13
    /// 功能摘要：超标天数统计
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class OverDaysAnalyze : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        DictionaryService dicService = Singleton<DictionaryService>.GetInstance();
        DayAQIService m_DayAQIService = Singleton<DayAQIService>.GetInstance();
        private MonitoringPointAirService pointAirService = Singleton<MonitoringPointAirService>.GetInstance();

        /// <summary>
        /// 站点服务
        /// </summary>
        MonitoringPointAirService m_MonitoringPointAirService = Singleton<MonitoringPointAirService>.GetInstance();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //string[] strname = m_MonitoringPointAirService.RetrieveAirMPListByCountryControlled().Select(t => t.MonitoringPointName).ToArray();
                //string names = "";
                //foreach (string item in strname)
                //{
                //    names += item + ";";
                //}
                string names = ConfigurationManager.AppSettings["NTRegionPointName"].ToString();    //从配置文件获取默认站点
                pointCbxRsm.SetPointValuesFromNames(names);
                InitControl();
            }
        }
        #region 页面控件初始化
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            for (int i = 0; i < DateTime.Now.Year - 2009; i++)
            {
                compareYear.Items.Add(new RadComboBoxItem((DateTime.Now.Year - i).ToString(), (DateTime.Now.Year - i).ToString()));
            }
            for (int i = 0; i < compareYear.Items.Count; i++)
            {
                if (int.Parse(compareYear.Items[i].Value) == DateTime.Now.AddYears(-1).Year)
                {
                    compareYear.Items[i].Checked = true;
                }
            }
            dayBegin.SelectedDate = DateTime.Now.AddMonths(-1);
            dayEnd.SelectedDate = DateTime.Now;
            dvCb.Visible = false;
            dvFatext.Visible = false;
            rcbFactors.Visible = false;
        }

        #endregion       

        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {
            string[] portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
            string BrandNum = "[";
            string HdPortId = "[";

            string rate = "";

            decimal? del = 0;

            IList<IPoint> points = null;
            points = pointCbxRsm.GetPoints();

            DateTime dtBegion = Convert.ToDateTime(dayBegin.SelectedDate.Value.ToString("yyyy-MM-dd 00:00:00"));
            DateTime dtEnd = Convert.ToDateTime(dayEnd.SelectedDate.Value.ToString("yyyy-MM-dd 23:59:59"));
            //每页显示数据个数            
            int pageSize = int.MaxValue;
            //当前页的序号
            int currentPageIndex = 0;

            int recordTotal = 0;
            string orderby = "";
            string rcbFactor = "";
            foreach (RadComboBoxItem item in rcbFactors.CheckedItems)
            {
                rcbFactor += (item.Value.ToString() + ",");
            }
            string[] factors = rcbFactor.Trim(',').Split(',');
            string rcbYear = "";
            foreach (RadComboBoxItem item in compareYear.CheckedItems)
            {
                rcbYear += (item.Value.ToString() + ",");
            }
            string[] rcbYears = { };
            if (rcbYear != "")
            {
                rcbYears = rcbYear.Trim(',').Split(',');
            }
            string TotalType = rdlType.SelectedText;
            string cblFactor = "";
            for (int j = 0; j < cbList.Items.Count; j++)
            {
                if (cbList.Items[j].Selected)
                {
                    cblFactor += cbList.Items[j].Text + ";";
                }
            }
            if (rbtnlType.SelectedValue == "CityProper")
            {
                //DataTable dtOver = m_DayAQIService.GetAreaOverDaysList(portIds,regionGuids, factors, dtBegion, dtEnd, TotalType, cblFactor, 100, pageSize, currentPageIndex, rcbYears, regionName, out recordTotal);
                DataTable dtOver = m_DayAQIService.GetAreaOverDaysList(portIds, factors, dtBegion, dtEnd, TotalType, cblFactor, 100, pageSize, currentPageIndex, rcbYears, out recordTotal);
                gridOverDays.DataSource = dtOver;
                if (dtOver != null)
                {
                    decimal? count = 0;
                    gridOverDays.VirtualItemCount = dtOver.Rows.Count;
                    DataTable dtPie = m_DayAQIService.GetAreaOverDaysList(portIds, factors, dtBegion, dtEnd, TotalType, cblFactor, 100, int.MaxValue, 0, rcbYears, out recordTotal);

                    for (int i = 0; i < dtPie.Rows.Count; i++)
                    {
                        rate += dtPie.Rows[i]["StandardDaysRate"] + ",";
                        //BrandNum += (Convert.ToDecimal(dtPie.Rows[i]["Good"]) + Convert.ToDecimal(dtPie.Rows[i]["Moderate"]))
                        //    / (Convert.ToDecimal(dtPie.Rows[i]["Good"]) + Convert.ToDecimal(dtPie.Rows[i]["Moderate"]) 
                        //    + Convert.ToDecimal(dtPie.Rows[i]["LightlyPolluted"]) + Convert.ToDecimal(dtPie.Rows[i]["ModeratelyPolluted"])
                        //    + Convert.ToDecimal(dtPie.Rows[i]["HeavilyPolluted"]) + Convert.ToDecimal(dtPie.Rows[i]["SeverelyPolluted"]));
                    }
                    rate = rate.TrimEnd(',');

                    for (int i = 0; i < dtPie.Columns.Count; i++)
                    {
                        DataColumn dcNew = dtPie.Columns[i];
                        if (!dcNew.ColumnName.Contains("RegionName") && !dcNew.ColumnName.Contains("Good") && !dcNew.ColumnName.Contains("Moderate") && !dcNew.ColumnName.Contains("LightlyPolluted") && !dcNew.ColumnName.Contains("ModeratelyPolluted") && !dcNew.ColumnName.Contains("HeavilyPolluted") && !dcNew.ColumnName.Contains("SeverelyPolluted"))
                        {
                            dtPie.Columns.Remove(dcNew);
                            i--;
                        }
                    }
                    for (int k = 1; k < dtPie.Columns.Count; k++)
                    {
                        string KindName = string.Empty;
                        if (dtPie.Columns[k].ColumnName == "Good")
                        {
                            KindName = "优";
                            BrandNum += "{type:'column',name:'" + KindName + "',color:'#7DC733',data:[";
                        }
                        else if (dtPie.Columns[k].ColumnName == "Moderate")
                        {
                            KindName = "良";
                            BrandNum += "{type:'column',name:'" + KindName + "',color:'#FFFF00',data:[";
                        }
                        else if (dtPie.Columns[k].ColumnName == "LightlyPolluted")
                        {
                            KindName = "轻度污染";
                            BrandNum += "{type:'column',name:'" + KindName + "',color:'#FF7E00',data:[";
                        }
                        else if (dtPie.Columns[k].ColumnName == "ModeratelyPolluted")
                        {
                            KindName = "中度污染";
                            BrandNum += "{type:'column',name:'" + KindName + "',color:'#FF0000',data:[";
                        }
                        else if (dtPie.Columns[k].ColumnName == "HeavilyPolluted")
                        {
                            KindName = "重度污染";
                            BrandNum += "{type:'column',name:'" + KindName + "',color:'#99004c',data:[";
                        }
                        else
                        {
                            KindName = "严重污染";
                            BrandNum += "{type:'column',name:'" + KindName + "',color:'#7e0023',data:[";
                        }
                        for (int i = 0; i < dtPie.Rows.Count; i++)
                        {
                            BrandNum += Convert.ToDecimal(dtPie.Rows[i][dtPie.Columns[k].ColumnName.ToString()]) + ",";
                        }
                        BrandNum = BrandNum.TrimEnd(',');
                        BrandNum += "]},";
                    }
                    BrandNum += "{type: 'spline',name: '达标率',data: [" + rate + "],marker: {lineWidth: 2,lineColor: Highcharts.getOptions().colors[3],fillColor: 'white'}}";
                    BrandNum += "]";
                    //BrandNum = BrandNum.TrimEnd(',');
                    //BrandNum += "]";
                    for (int n = 0; n < dtPie.Rows.Count; n++)
                    {
                        HdPortId +="'"+ dtPie.Rows[n]["RegionName"].ToString() + "',";
                    }
                    HdPortId = HdPortId.TrimEnd(',');
                    HdPortId += "]";
                }
                else
                {
                    BrandNum += "]";
                    HdPortId += "]";
                }
                HiddenData.Value = BrandNum;
                HiddenPortId.Value = HdPortId;
            }
            else
            {
                DataTable dtOver = m_DayAQIService.GetOverDaysList(portIds, factors, dtBegion, dtEnd, TotalType, cblFactor, 100, pageSize, currentPageIndex, rcbYears, out recordTotal);
                gridOverDays.DataSource = dtOver;
                gridOverDays.VirtualItemCount = dtOver.Rows.Count;
                if (dtOver != null)
                {
                    decimal? count = 0;
                    DataTable dtPie = m_DayAQIService.GetOverDaysList(portIds, factors, dtBegion, dtEnd, TotalType, cblFactor, 100, int.MaxValue, 0, rcbYears, out recordTotal);
                    for (int i = 0; i < dtPie.Rows.Count; i++)
                    {
                        rate += dtPie.Rows[i]["StandardDaysRate"] + ",";
                        //BrandNum += (Convert.ToDecimal(dtPie.Rows[i]["Good"]) + Convert.ToDecimal(dtPie.Rows[i]["Moderate"]))
                        //    / (Convert.ToDecimal(dtPie.Rows[i]["Good"]) + Convert.ToDecimal(dtPie.Rows[i]["Moderate"]) 
                        //    + Convert.ToDecimal(dtPie.Rows[i]["LightlyPolluted"]) + Convert.ToDecimal(dtPie.Rows[i]["ModeratelyPolluted"])
                        //    + Convert.ToDecimal(dtPie.Rows[i]["HeavilyPolluted"]) + Convert.ToDecimal(dtPie.Rows[i]["SeverelyPolluted"]));
                    }
                    rate = rate.TrimEnd(',');
                    for (int i = 0; i < dtPie.Columns.Count; i++)
                    {
                        DataColumn dcNew = dtPie.Columns[i];
                        if (!dcNew.ColumnName.Contains("portIds") && !dcNew.ColumnName.Contains("Good") && !dcNew.ColumnName.Contains("Moderate") && !dcNew.ColumnName.Contains("LightlyPolluted") && !dcNew.ColumnName.Contains("ModeratelyPolluted") && !dcNew.ColumnName.Contains("HeavilyPolluted") && !dcNew.ColumnName.Contains("SeverelyPolluted"))
                        {
                            dtPie.Columns.Remove(dcNew);
                            i--;
                        }
                    }
                    for (int k = 1; k < dtPie.Columns.Count;k++ )
                    {
                        string KindName=string.Empty;
                        if (dtPie.Columns[k].ColumnName == "Good")
                        {
                            KindName = "优";
                            BrandNum += "{type:'column',name:'" + KindName + "',color:'#7DC733',data:[";
                        }
                        else if (dtPie.Columns[k].ColumnName == "Moderate")
                        {
                            KindName = "良";
                            BrandNum += "{type:'column',name:'" + KindName + "',color:'#FFFF00',data:[";
                        }
                        else if (dtPie.Columns[k].ColumnName == "LightlyPolluted")
                        {
                            KindName = "轻度污染";
                            BrandNum += "{type:'column',name:'" + KindName + "',color:'#FF7E00',data:[";
                        }
                        else if (dtPie.Columns[k].ColumnName == "ModeratelyPolluted")
                        {
                            KindName = "中度污染";
                            BrandNum += "{type:'column',name:'" + KindName + "',color:'#FF0000',data:[";
                        }
                        else if (dtPie.Columns[k].ColumnName == "HeavilyPolluted")
                        {
                            KindName = "重度污染";
                            BrandNum += "{type:'column',name:'" + KindName + "',color:'#99004c',data:[";
                        }
                        else
                        {
                            KindName = "严重污染";
                            BrandNum += "{type:'column',name:'" + KindName + "',color:'#7e0023',data:[";
                        }
                        //BrandNum += "{name:'" + KindName + "',data:[";
                        for (int i = 0; i < dtPie.Rows.Count; i++)
                        {
                            BrandNum += Convert.ToDecimal(dtPie.Rows[i][dtPie.Columns[k].ColumnName.ToString()]) + ",";
                        }
                        BrandNum = BrandNum.TrimEnd(',');
                        BrandNum += "]},";
                    }
                    BrandNum += "{type: 'spline',name: '达标率',data: ["+rate+"],marker: {lineWidth: 2,lineColor: Highcharts.getOptions().colors[3],fillColor: 'white'}}";
                    BrandNum += "]";
                    for (int n = 0; n < dtPie.Rows.Count; n++)
                    {
                        IPoint point = points.FirstOrDefault(x => x.PointID.Equals(dtPie.Rows[n]["portIds"].ToString()));
                        HdPortId += "'"+point.PointName + "',";
                    }
                    HdPortId = HdPortId.TrimEnd(',');
                    HdPortId += "]";
                }
                else
                {
                    BrandNum += "]";
                    HdPortId += "]";
                }
                HiddenData.Value = BrandNum;
                HiddenPortId.Value = HdPortId;
            }
        }
        #endregion

        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            gridOverDays.CurrentPageIndex = 0;
            gridOverDays.Rebind();
            if (tabStrip.SelectedTab.Text == "图表")
            {
                BindChart();
                FirstLoadChart.Value = "1";
            }
        }
        #region 绑定图表
        private void BindChart()
        {
            RegisterScript("InitGroupChart();");
        }
        #endregion
        protected void rdlType_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            if (rdlType.SelectedValue == "1")
            {
                dvCb.Visible = true;
                dvFatext.Visible = true;
                rcbFactors.Visible = true;
            }
            else
            {
                dvCb.Visible = false;
                dvFatext.Visible = false;
                rcbFactors.Visible = false;
            }
        }

        protected void gridOverDays_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }

        protected void gridOverDays_ItemDataBound(object sender, GridItemEventArgs e)
        {
            DateTime dtBegion = Convert.ToDateTime(dayBegin.SelectedDate.Value.ToString("yyyy-MM-dd 00:00:00"));
            DateTime dtEnd = Convert.ToDateTime(dayEnd.SelectedDate.Value.ToString("yyyy-MM-dd 23:59:59"));
            GridDataItem item = e.Item as GridDataItem;
            DataRowView drv = e.Item.DataItem as DataRowView;
            if (rdlType.SelectedValue == "1")
            {
                switch (rbtnlType.SelectedValue)
                {
                    case "Port":
                        /// <summary>
                        /// 选择站点
                        /// </summary>
                        IList<IPoint> points = null;
                        points = pointCbxRsm.GetPoints();

                        if (e.Item is GridDataItem)
                        {
                            if (item["portIds"] != null)
                            {
                                GridTableCell pointCell = (GridTableCell)item["portIds"];
                                IPoint point = points.FirstOrDefault(x => x.PointID.Equals(pointCell.Text.Trim()));
                                if (points != null)
                                {
                                    pointCell.Text = string.Format("<a href='#' onclick='RowClick(\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\")'>{5}</a>", drv["portIds"], point.PointName, "Port", dtBegion, dtEnd, point.PointName);

                                }
                            }

                        }
                        break;
                    case "CityProper":
                        if (e.Item is GridDataItem)
                        {
                            if (item["RegionName"] != null)
                            {
                                GridTableCell RegionNameCell = (GridTableCell)item["RegionName"];
                                IList<IPoint> pointsAll =  pointCbxRsm.GetPoints();
                                List<string> pointNameRegion = pointCbxRsm.GetPointNameByRegion(drv["RegionName"].ToString());
                                string pointNamesAll = string.Join(";",pointsAll.Select(t => t.PointName).Where(x => pointNameRegion.Contains(x)).ToArray());
                                RegionNameCell.Text = string.Format("<a href='#' onclick='RowClick(\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\")'>{5}</a>", pointNamesAll, drv["RegionName"], "CityProper", dtBegion, dtEnd, drv["RegionName"]);
                            }
                        }
                        break;
                }
            }
            if (rdlType.SelectedValue == "2"){
                switch (rbtnlType.SelectedValue)
                {
                    case "Port":
                        /// <summary>
                        /// 选择站点
                        /// </summary>
                        IList<IPoint> points = null;
                        points = pointCbxRsm.GetPoints();

                        if (e.Item is GridDataItem)
                        {
                            if (item["portIds"] != null)
                            {
                                GridTableCell pointCell = (GridTableCell)item["portIds"];
                                IPoint point = points.FirstOrDefault(x => x.PointID.Equals(pointCell.Text.Trim()));
                                if (points != null)
                                {
                                    pointCell.Text = string.Format("<a href='#' onclick='RowClick(\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\")'>{5}</a>", drv["portIds"], point.PointName, "Port", dtBegion, dtEnd, point.PointName);

                                    GridTableCell StandardDaysCell = (GridTableCell)item["StandardDays"];
                                    string days0 = "StandardDays";
                                    StandardDaysCell.Text = string.Format("<a href='#' onclick='RowClick2(\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\")'>{6}</a>", drv["portIds"], point.PointName, "Port", dtBegion, dtEnd, days0, drv["StandardDays"]);
                                    GridTableCell OverDaysCell = (GridTableCell)item["OverDays"];
                                    string days1 = "OverDays";
                                    OverDaysCell.Text = string.Format("<a href='#' onclick='RowClick2(\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\")'>{6}</a>", drv["portIds"], point.PointName, "Port", dtBegion, dtEnd, days1, drv["OverDays"]);
                                    GridTableCell InvalidDaysCell = (GridTableCell)item["InvalidDays"];
                                    string days2 = "InvalidDays";
                                    InvalidDaysCell.Text = string.Format("<a href='#' onclick='RowClick2(\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\")'>{6}</a>", drv["portIds"], point.PointName, "Port", dtBegion, dtEnd, days2, drv["InvalidDays"]);
                                    GridTableCell GoodCell = (GridTableCell)item["Good"];
                                    string days3 = "Good";
                                    GoodCell.Text = string.Format("<a href='#' onclick='RowClick2(\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\")'>{6}</a>", drv["portIds"], point.PointName, "Port", dtBegion, dtEnd, days3, drv["Good"]);
                                    GridTableCell ModerateCell = (GridTableCell)item["Moderate"];
                                    string days4 = "Moderate";
                                    ModerateCell.Text = string.Format("<a href='#' onclick='RowClick2(\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\")'>{6}</a>", drv["portIds"], point.PointName, "Port", dtBegion, dtEnd, days4, drv["Moderate"]);
                                    GridTableCell LightlyPollutedCell = (GridTableCell)item["LightlyPolluted"];
                                    string days5 = "LightlyPolluted";
                                    LightlyPollutedCell.Text = string.Format("<a href='#' onclick='RowClick2(\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\")'>{6}</a>", drv["portIds"], point.PointName, "Port", dtBegion, dtEnd, days5, drv["LightlyPolluted"]);
                                    GridTableCell ModeratelyPollutedCell = (GridTableCell)item["ModeratelyPolluted"];
                                    string days6 = "ModeratelyPolluted";
                                    ModeratelyPollutedCell.Text = string.Format("<a href='#' onclick='RowClick2(\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\")'>{6}</a>", drv["portIds"], point.PointName, "Port", dtBegion, dtEnd, days6, drv["ModeratelyPolluted"]);
                                    GridTableCell HeavilyPollutedCell = (GridTableCell)item["HeavilyPolluted"];
                                    string days7 = "HeavilyPolluted";
                                    HeavilyPollutedCell.Text = string.Format("<a href='#' onclick='RowClick2(\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\")'>{6}</a>", drv["portIds"], point.PointName, "Port", dtBegion, dtEnd, days7, drv["HeavilyPolluted"]);
                                    GridTableCell SeverelyPollutedCell = (GridTableCell)item["SeverelyPolluted"];
                                    string days8 = "SeverelyPolluted";
                                    SeverelyPollutedCell.Text = string.Format("<a href='#' onclick='RowClick2(\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\")'>{6}</a>", drv["portIds"], point.PointName, "Port", dtBegion, dtEnd, days8, drv["SeverelyPolluted"]);
                                }
                            }

                        }
                        break;
                    case "CityProper":
                        if (e.Item is GridDataItem)
                        {
                            if (item["RegionName"] != null)
                            {
                                IList<IPoint> pointsAll = pointCbxRsm.GetPoints();
                                List<string> pointNameRegion = pointCbxRsm.GetPointNameByRegion(drv["RegionName"].ToString());
                                string pointNamesAll = string.Join(";", pointsAll.Select(t => t.PointName).Where(x => pointNameRegion.Contains(x)).ToArray());
                                GridTableCell RegionNameCell = (GridTableCell)item["RegionName"];
                                RegionNameCell.Text = string.Format("<a href='#' onclick='RowClick(\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\")'>{5}</a>", pointNamesAll, drv["RegionName"], "CityProper", dtBegion, dtEnd, drv["RegionName"]);

                                GridTableCell StandardDaysCell = (GridTableCell)item["StandardDays"];
                                string days0 = "StandardDays";
                                StandardDaysCell.Text = string.Format("<a href='#' onclick='RowClick2(\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\")'>{6}</a>", pointNamesAll, drv["RegionName"], "CityProper", dtBegion, dtEnd, days0, drv["StandardDays"]);
                                GridTableCell OverDaysCell = (GridTableCell)item["OverDays"];
                                string days1 = "OverDays";
                                OverDaysCell.Text = string.Format("<a href='#' onclick='RowClick2(\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\")'>{6}</a>", pointNamesAll, drv["RegionName"], "CityProper", dtBegion, dtEnd, days1, drv["OverDays"]);
                                GridTableCell InvalidDaysCell = (GridTableCell)item["InvalidDays"];
                                string days2 = "InvalidDays";
                                InvalidDaysCell.Text = string.Format("<a href='#' onclick='RowClick2(\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\")'>{6}</a>", pointNamesAll, drv["RegionName"], "CityProper", dtBegion, dtEnd, days2, drv["InvalidDays"]);
                                GridTableCell GoodCell = (GridTableCell)item["Good"];
                                string days3 = "Good";
                                GoodCell.Text = string.Format("<a href='#' onclick='RowClick2(\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\")'>{6}</a>", pointNamesAll, drv["RegionName"], "CityProper", dtBegion, dtEnd, days3, drv["Good"]);
                                GridTableCell ModerateCell = (GridTableCell)item["Moderate"];
                                string days4 = "Moderate";
                                ModerateCell.Text = string.Format("<a href='#' onclick='RowClick2(\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\")'>{6}</a>", pointNamesAll, drv["RegionName"], "CityProper", dtBegion, dtEnd, days4, drv["Moderate"]);
                                GridTableCell LightlyPollutedCell = (GridTableCell)item["LightlyPolluted"];
                                string days5 = "LightlyPolluted";
                                LightlyPollutedCell.Text = string.Format("<a href='#' onclick='RowClick2(\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\")'>{6}</a>", pointNamesAll, drv["RegionName"], "CityProper", dtBegion, dtEnd, days5, drv["LightlyPolluted"]);
                                GridTableCell ModeratelyPollutedCell = (GridTableCell)item["ModeratelyPolluted"];
                                string days6 = "ModeratelyPolluted";
                                ModeratelyPollutedCell.Text = string.Format("<a href='#' onclick='RowClick2(\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\")'>{6}</a>", pointNamesAll, drv["RegionName"], "CityProper", dtBegion, dtEnd, days6, drv["ModeratelyPolluted"]);
                                GridTableCell HeavilyPollutedCell = (GridTableCell)item["HeavilyPolluted"];
                                string days7 = "HeavilyPolluted";
                                HeavilyPollutedCell.Text = string.Format("<a href='#' onclick='RowClick2(\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\")'>{6}</a>", pointNamesAll, drv["RegionName"], "CityProper", dtBegion, dtEnd, days7, drv["HeavilyPolluted"]);
                                GridTableCell SeverelyPollutedCell = (GridTableCell)item["SeverelyPolluted"];
                                string days8 = "SeverelyPolluted";
                                SeverelyPollutedCell.Text = string.Format("<a href='#' onclick='RowClick2(\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\")'>{6}</a>", pointNamesAll, drv["RegionName"], "CityProper", dtBegion, dtEnd, days8, drv["SeverelyPolluted"]);

                            }
                        }
                        break;
                }
            }
        }

        protected void gridOverDays_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            try
            {
                GridBoundColumn col = e.Column as GridBoundColumn;
                if (col == null)
                {
                    return;
                }
                if (col.DataField == "RegionName")
                {
                    col.HeaderText = "区域";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "portIds")
                {
                    col.HeaderText = "测点";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "StandardDays")
                {
                    col.HeaderText = "达标天数";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "OverDays")
                {
                    col.HeaderText = "超标天数";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "InvalidDays")
                {
                    col.HeaderText = "无效天数";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "StandardDaysRate")
                {
                    col.HeaderText = "达标率（%）";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (compareYear.CheckedItems.Select(x => x.Value).Contains(col.DataField))
                {
                    col.HeaderText = col.DataField + "年达标率（%）";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "Good")
                {
                    col.HeaderText = "优";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "Moderate")
                {
                    col.HeaderText = "良";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "LightlyPolluted")
                {
                    col.HeaderText = "轻度污染";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "ModeratelyPolluted")
                {
                    col.HeaderText = "中度污染";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "HeavilyPolluted")
                {
                    col.HeaderText = "重度污染";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "SeverelyPolluted")
                {
                    col.HeaderText = "严重污染";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "OverSO2")
                {
                    col.HeaderText = "SO2";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ColumnGroupName = "OverDays";
                    col.HeaderStyle.Width = Unit.Pixel(80);
                    col.ItemStyle.Width = Unit.Pixel(80);
                }
                else if (col.DataField == "OverNO2")
                {
                    col.HeaderText = "NO2";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ColumnGroupName = "OverDays";
                    col.HeaderStyle.Width = Unit.Pixel(80);
                    col.ItemStyle.Width = Unit.Pixel(80);
                }
                else if (col.DataField == "OverPM25")
                {
                    col.HeaderText = "PM25";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ColumnGroupName = "OverDays";
                    col.HeaderStyle.Width = Unit.Pixel(80);
                    col.ItemStyle.Width = Unit.Pixel(80);
                }
                else if (col.DataField == "OverPM10")
                {
                    col.HeaderText = "PM10";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ColumnGroupName = "OverDays";
                    col.HeaderStyle.Width = Unit.Pixel(80);
                    col.ItemStyle.Width = Unit.Pixel(80);
                }
                else if (col.DataField == "OverCO")
                {
                    col.HeaderText = "CO";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ColumnGroupName = "OverDays";
                    col.HeaderStyle.Width = Unit.Pixel(80);
                    col.ItemStyle.Width = Unit.Pixel(80);
                }
                else if (col.DataField == "OverO3")
                {
                    col.HeaderText = "O3-1";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ColumnGroupName = "OverDays";
                    col.HeaderStyle.Width = Unit.Pixel(80);
                    col.ItemStyle.Width = Unit.Pixel(80);
                }
                else if (col.DataField == "OverRecent8HoursO3")
                {
                    col.HeaderText = "O3-8";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ColumnGroupName = "OverDays";
                    col.HeaderStyle.Width = Unit.Pixel(80);
                    col.ItemStyle.Width = Unit.Pixel(80);
                }
                else if (col.DataField == "StandSO2")
                {
                    col.HeaderText = "SO2";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ColumnGroupName = "StandDays";
                    col.HeaderStyle.Width = Unit.Pixel(80);
                    col.ItemStyle.Width = Unit.Pixel(80);
                }
                else if (col.DataField == "StandNO2")
                {
                    col.HeaderText = "NO2";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ColumnGroupName = "StandDays";
                    col.HeaderStyle.Width = Unit.Pixel(80);
                    col.ItemStyle.Width = Unit.Pixel(80);
                }
                else if (col.DataField == "StandPM25")
                {
                    col.HeaderText = "PM25";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ColumnGroupName = "StandDays";
                    col.HeaderStyle.Width = Unit.Pixel(80);
                    col.ItemStyle.Width = Unit.Pixel(80);
                }
                else if (col.DataField == "StandPM10")
                {
                    col.HeaderText = "PM10";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ColumnGroupName = "StandDays";
                    col.HeaderStyle.Width = Unit.Pixel(80);
                    col.ItemStyle.Width = Unit.Pixel(80);
                }
                else if (col.DataField == "StandCO")
                {
                    col.HeaderText = "CO";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ColumnGroupName = "StandDays";
                    col.HeaderStyle.Width = Unit.Pixel(80);
                    col.ItemStyle.Width = Unit.Pixel(80);
                }
                else if (col.DataField == "StandO3")
                {
                    col.HeaderText = "O3-1";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ColumnGroupName = "StandDays";
                    col.HeaderStyle.Width = Unit.Pixel(80);
                    col.ItemStyle.Width = Unit.Pixel(80);
                }
                else if (col.DataField == "StandRecent8HoursO3")
                {
                    col.HeaderText = "O3-8";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ColumnGroupName = "StandDays";
                    col.HeaderStyle.Width = Unit.Pixel(80);
                    col.ItemStyle.Width = Unit.Pixel(80);
                }
                else if (col.DataField == "PrimarySO2")
                {
                    col.HeaderText = "SO2";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ColumnGroupName = "PrimaryDays";
                    col.HeaderStyle.Width = Unit.Pixel(80);
                    col.ItemStyle.Width = Unit.Pixel(80);
                }
                else if (col.DataField == "PrimaryNO2")
                {
                    col.HeaderText = "NO2";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ColumnGroupName = "PrimaryDays";
                    col.HeaderStyle.Width = Unit.Pixel(80);
                    col.ItemStyle.Width = Unit.Pixel(80);
                }
                else if (col.DataField == "PrimaryPM25")
                {
                    col.HeaderText = "PM25";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ColumnGroupName = "PrimaryDays";
                    col.HeaderStyle.Width = Unit.Pixel(80);
                    col.ItemStyle.Width = Unit.Pixel(80);
                }
                else if (col.DataField == "PrimaryPM10")
                {
                    col.HeaderText = "PM10";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ColumnGroupName = "PrimaryDays";
                    col.HeaderStyle.Width = Unit.Pixel(80);
                    col.ItemStyle.Width = Unit.Pixel(80);
                }
                else if (col.DataField == "PrimaryCO")
                {
                    col.HeaderText = "CO";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ColumnGroupName = "PrimaryDays";
                    col.HeaderStyle.Width = Unit.Pixel(80);
                    col.ItemStyle.Width = Unit.Pixel(80);
                }
                else if (col.DataField == "PrimaryO3")
                {
                    col.HeaderText = "O3-8";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ColumnGroupName = "PrimaryDays";
                    col.HeaderStyle.Width = Unit.Pixel(80);
                    col.ItemStyle.Width = Unit.Pixel(80);
                }
                else
                {
                    e.Column.Visible = false;
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void gridRTB_ButtonClick(object sender, RadToolBarEventArgs e)
        {
            Telerik.Web.UI.RadToolBarButton button = (Telerik.Web.UI.RadToolBarButton)e.Item;
            if (button.CommandName == "ExportToExcel")
            {
                try
                {
                    string[] portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
                    //string regionGuid = "";
                    //Dictionary<string, string> regionName = new Dictionary<string, string>();
                    //DataView dvRegion = GetRegionByPointId(portIds);
                    //IEnumerable<string> names = dvRegion.ToTable().AsEnumerable().Select(t => t.Field<string>("Region")).ToList().Distinct();
                    //foreach (string name in names)
                    //{
                    //    DataRow dr = dvRegion.ToTable().Select("Region='" + name + "'").FirstOrDefault();
                    //    regionGuid += (dr["RegionUid"].ToString() + ",");
                    //    regionName.Add(dr["RegionUid"].ToString(), dr["Region"].ToString());
                    //}
                    //string[] regionGuids = regionGuid.Trim(',').Split(',');
                    DateTime dtBegion = Convert.ToDateTime(dayBegin.SelectedDate.Value.ToString("yyyy-MM-dd 00:00:00"));
                    DateTime dtEnd = Convert.ToDateTime(dayEnd.SelectedDate.Value.ToString("yyyy-MM-dd 23:59:59"));
                    //每页显示数据个数            
                    int pageSize = int.MaxValue;
                    //当前页的序号
                    int currentPageIndex = 0;

                    int recordTotal = 0;
                    string rcbFactor = "";
                    foreach (RadComboBoxItem item in rcbFactors.CheckedItems)
                    {
                        rcbFactor += (item.Value.ToString() + ",");
                    }
                    string[] factors = rcbFactor.Trim(',').Split(',');
                    string TotalType = rdlType.SelectedText;
                    string cblFactor = "";
                    for (int j = 0; j < cbList.Items.Count; j++)
                    {
                        if (cbList.Items[j].Selected)
                        {
                            cblFactor += cbList.Items[j].Text + ";";
                        }
                    }
                    string rcbYear = "";
                    foreach (RadComboBoxItem item in compareYear.CheckedItems)
                    {
                        rcbYear += (item.Value.ToString() + ",");
                    }
                    string[] rcbYears = rcbYear.Trim(',').Split(',');
                    if (rbtnlType.SelectedValue == "CityProper")
                    {
                        //DataTable dtOver = m_DayAQIService.GetAreaOverDaysList(portIds, regionGuids, factors, dtBegion, dtEnd, TotalType, cblFactor, 100, pageSize, currentPageIndex, rcbYears, regionName, out recordTotal);
                        DataTable dtOver = m_DayAQIService.GetAreaOverDaysList(portIds, factors, dtBegion, dtEnd, TotalType, cblFactor, 100, pageSize, currentPageIndex, rcbYears, out recordTotal);
                        dtOver.Columns["RegionName"].SetOrdinal(0);
                        if (TotalType == "质量类别")
                        {
                            DataTable dt = UpdateExportColumnName(dtOver, rbtnlType.SelectedValue);
                            ExcelHelper.DataTableToExcel(dt, "达标率及天数类别(区域)", "质量类别", this.Page);
                        }
                        else
                        {
                            try
                            {
                                DataTableToExcel(dtOver, "达标率及天数类别(区域)", "因子统计", rbtnlType.SelectedValue, cblFactor, factors, rcbYears);
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                    else
                    {
                        DataTable dtOver = m_DayAQIService.GetOverDaysList(portIds, factors, dtBegion, dtEnd, TotalType, cblFactor, 100, pageSize, currentPageIndex, rcbYears, out recordTotal);
                        dtOver.Columns.Add("RegionName", typeof(string));
                        foreach (DataRow dr in dtOver.Rows)
                        {
                            if (dr["portIds"] != DBNull.Value)
                            {
                                string pointid = dr["portIds"].ToString();
                                IPoint point = pointCbxRsm.GetPoints().FirstOrDefault(x => x.PointID.Equals(pointid));
                                dr["RegionName"] = point.PointName;
                            }
                        }
                        dtOver.Columns["RegionName"].SetOrdinal(0);
                        if (TotalType == "质量类别")
                        {
                            DataTable dt = UpdateExportColumnName(dtOver, rbtnlType.SelectedValue);
                            ExcelHelper.DataTableToExcel(dt, "达标率及天数类别(测点)", "质量类别", this.Page);
                        }
                        else
                        {
                            DataTableToExcel(dtOver, "达标率及天数类别(测点)", "因子统计", rbtnlType.SelectedValue, cblFactor, factors, rcbYears);
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
        /// <summary>
        /// 修改要导出的数据表中的列名
        /// </summary>
        /// <param name="dv">原始数据表</param>
        /// <param name="dvStatistical">合计行数据表</param>
        /// <returns></returns>
        private DataTable UpdateExportColumnName(DataTable dt, string type)
        {
            DataTable dtNew = dt;
            for (int i = 0; i < dtNew.Columns.Count; i++)
            {
                if (dtNew.Columns[i].ColumnName == "RegionName")
                {
                    if (type == "CityProper")
                    {
                        dtNew.Columns[i].ColumnName = "区域";
                    }
                    else
                    {
                        dtNew.Columns[i].ColumnName = "测点";
                    }
                }
                else if (dtNew.Columns[i].ColumnName == "StandardDays")
                {
                    dtNew.Columns[i].ColumnName = "达标天数";
                }
                else if (dtNew.Columns[i].ColumnName == "OverDays")
                {
                    dtNew.Columns[i].ColumnName = "超标天数";
                }
                else if (dtNew.Columns[i].ColumnName == "InvalidDays")
                {
                    dtNew.Columns[i].ColumnName = "无效天数";
                }
                else if (dtNew.Columns[i].ColumnName == "StandardDaysRate")
                {
                    dtNew.Columns[i].ColumnName = "达标率（%）";
                }
                else if (compareYear.CheckedItems.Select(x => x.Value).Contains(dtNew.Columns[i].ColumnName))
                {
                    string year = dtNew.Columns[i].ColumnName;
                    dtNew.Columns[i].ColumnName = year + "年达标率（%）";
                }
                else if (dtNew.Columns[i].ColumnName == "Good")
                {
                    dtNew.Columns[i].ColumnName = "优";
                }
                else if (dtNew.Columns[i].ColumnName == "Moderate")
                {
                    dtNew.Columns[i].ColumnName = "良";
                }
                else if (dtNew.Columns[i].ColumnName == "LightlyPolluted")
                {
                    dtNew.Columns[i].ColumnName = "轻度污染";
                }
                else if (dtNew.Columns[i].ColumnName == "ModeratelyPolluted")
                {
                    dtNew.Columns[i].ColumnName = "中度污染";
                }
                else if (dtNew.Columns[i].ColumnName == "HeavilyPolluted")
                {
                    dtNew.Columns[i].ColumnName = "重度污染";
                }
                else if (dtNew.Columns[i].ColumnName == "SeverelyPolluted")
                {
                    dtNew.Columns[i].ColumnName = "严重污染";
                }
                else
                {
                    dtNew.Columns.Remove(dtNew.Columns[i].ColumnName);
                    i--;
                }
            }
            //foreach (DataColumn dc in dtNew.Columns)
            //{
            //    if (dc.ColumnName == "RegionName")
            //    {
            //        if (type == "CityProper")
            //        {
            //            dc.ColumnName = "区域";
            //        }
            //        else
            //        {
            //            dc.ColumnName = "测点";
            //        }
            //    }
            //    else if (dc.ColumnName == "StandardDays")
            //    {
            //        dc.ColumnName = "达标天数";
            //    }
            //    else if (dc.ColumnName == "OverDays")
            //    {
            //        dc.ColumnName = "超标天数";
            //    }
            //    else if (dc.ColumnName == "StandardDaysRate")
            //    {
            //        dc.ColumnName = "达标天数比例（%）";
            //    }
            //    else if (dc.ColumnName == "Good")
            //    {
            //        dc.ColumnName = "优";
            //    }
            //    else if (dc.ColumnName == "Moderate")
            //    {
            //        dc.ColumnName = "良";
            //    }
            //    else if (dc.ColumnName == "LightlyPolluted")
            //    {
            //        dc.ColumnName = "轻度污染";
            //    }
            //    else if (dc.ColumnName == "ModeratelyPolluted")
            //    {
            //        dc.ColumnName = "中度污染";
            //    }
            //    else if (dc.ColumnName == "HeavilyPolluted")
            //    {
            //        dc.ColumnName = "重度污染";
            //    }
            //    else if (dc.ColumnName == "SeverelyPolluted")
            //    {
            //        dc.ColumnName = "严重污染";
            //    }
            //    else
            //    {
            //        dtNew.Columns.Remove(dc.ColumnName);
            //    }
            //}
            return dtNew;
        }
        /// <summary>
        /// 导出超标天数统计
        /// </summary>
        /// <param name="dv">超标天数统计</param>
        /// <returns></returns>
        private void DataTableToExcel(DataTable dt, string fileName, string sheetName, string type, string cblFactor, string[] factors, string[] years)
        {
            DataTable dtNew = dt;
            Workbook workbook = new Workbook();
            Worksheet sheet = workbook.Worksheets[0];
            Cells cells = sheet.Cells;
            Aspose.Cells.Style cellStyle = workbook.Styles[workbook.Styles.Add()];
            workbook.FileName = fileName;
            sheet.Name = sheetName;
            cellStyle.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center; // 设置居中 
            cellStyle.Font.Name = "宋体"; //文字字体
            cellStyle.Font.Size = 12;//文字大小
            cellStyle.IsLocked = false; //单元格解锁
            cellStyle.IsTextWrapped = true; //单元格内容自动换行
            //第一行
            if (type == "CityProper")
            {
                cells[0, 0].PutValue("区域");
                cells.Merge(0, 0, 2, 1);
            }
            else
            {
                cells[0, 0].PutValue("测点");
                cells.Merge(0, 0, 2, 1);
            }
            cells[0, 1].PutValue("达标天数");
            cells.Merge(0, 1, 2, 1);
            cells[0, 2].PutValue("超标天数");
            cells.Merge(0, 2, 2, 1);
            cells[0, 3].PutValue("无效天数");
            cells.Merge(0, 3, 2, 1);
            cells[0, 4].PutValue("达标率（%）");
            cells.Merge(0, 4, 2, 1);
            int mRow = 5;
            foreach (string year in years)
            {
                cells[0, mRow].PutValue(year+"年达标率（%）");
                cells.Merge(0, mRow, 2, 1);
                mRow++;
            }
            if (cblFactor.Contains("超标"))
            {
                cells[0, mRow].PutValue("超标天数");
                cells.Merge(0, mRow, 1, factors.Length);
                foreach (string itemFactor in factors)
                {
                    if (itemFactor == "RecentoneHoursO3")
                    {
                        cells[1, mRow].PutValue("O3-1");
                    }
                    else if (itemFactor == "Recent8HoursO3")
                    {
                        cells[1, mRow].PutValue("O3-8");
                    }
                    else
                    {
                        cells[1, mRow].PutValue(itemFactor);
                    }
                    mRow++;
                }
            }
            if (cblFactor.Contains("达标"))
            {
                cells[0, mRow].PutValue("达标天数");
                cells.Merge(0, mRow, 1, factors.Length);
                foreach (string itemFactor in factors)
                {
                    if (itemFactor == "RecentoneHoursO3")
                    {
                        cells[1, mRow].PutValue("O3-1");
                    }
                    else if (itemFactor == "Recent8HoursO3")
                    {
                        cells[1, mRow].PutValue("O3-8");
                    }
                    else
                    {
                        cells[1, mRow].PutValue(itemFactor);
                    }
                    mRow++;
                }
            }
            
            if (cblFactor.Contains("首要污染物"))
            {
                cells[0, mRow].PutValue("首要污染物");
                cells.Merge(0, mRow, 1, factors.Length);
                foreach (string itemFactor in factors)
                {
                    if (itemFactor == "Recent8HoursO3")
                    {
                        cells[1, mRow].PutValue("O3-8");
                    }
                    else if (itemFactor == "RecentoneHoursO3")
                    {
                        mRow--;
                    }
                    else
                    {
                        cells[1, mRow].PutValue(itemFactor);
                    }
                    mRow++;
                }
            }
            for (int i = 0; i < dtNew.Rows.Count; i++)
            {
                int dataRow = 5;
                DataRow drNew = dtNew.Rows[i];
                int rowIndex = i + 2;
                cells[rowIndex, 0].PutValue(drNew["RegionName"].ToString());
                cells[rowIndex, 1].PutValue(drNew["StandardDays"].ToString());
                cells[rowIndex, 2].PutValue(drNew["OverDays"].ToString());
                cells[rowIndex, 3].PutValue(drNew["InvalidDays"].ToString());
                cells[rowIndex, 4].PutValue(drNew["StandardDaysRate"].ToString());
                foreach (string year in years)
                {
                    cells[rowIndex, dataRow].PutValue(drNew[year].ToString());
                    dataRow++;
                }
                int m = dataRow;
                if (cblFactor.Contains("超标"))
                {
                    foreach (string itemFactor in factors)
                    {
                        if (itemFactor == "RecentoneHoursO3")
                        {
                            cells[rowIndex, m].PutValue(drNew["OverO3"].ToString());
                        }
                        else
                        {
                            cells[rowIndex, m].PutValue(drNew["Over" + itemFactor].ToString());
                        }
                        m++;
                    }
                }
                if (cblFactor.Contains("达标"))
                {
                    foreach (string itemFactor in factors)
                    {
                        if (itemFactor == "RecentoneHoursO3")
                        {
                            cells[rowIndex, m].PutValue(drNew["StandO3"].ToString());
                        }
                        else
                        {
                            cells[rowIndex, m].PutValue(drNew["Stand" + itemFactor].ToString());
                        }
                        m++;
                    }
                }
                
                if(cblFactor.Contains("首要污染物"))
                {
                    foreach (string itemFactor in factors)
                    {
                        //if (factors.Length==7)
                        //{
                        //    if (itemFactor == "RecentoneHoursO3")
                        //    {
                        //        cells[rowIndex, m].PutValue(drNew["PrimaryO3"].ToString());
                        //    }
                        //    else
                        //    {
                        //        cells[rowIndex, m].PutValue(drNew["Primary" + itemFactor].ToString());
                        //    }
                        //}
                        //else
                        //{
                        //    if (itemFactor == "RecentoneHoursO3" || itemFactor == "Recent8HoursO3")
                        //    {
                        //        cells[rowIndex, m].PutValue(drNew["PrimaryO3"].ToString());
                        //    }
                        //    else
                        //    {
                        //        cells[rowIndex, m].PutValue(drNew["Primary" + itemFactor].ToString());
                        //    }
                        //}
                        if (itemFactor == "Recent8HoursO3")
                        {
                            cells[rowIndex, m].PutValue(drNew["PrimaryO3"].ToString());
                        }
                        else if (itemFactor == "RecentoneHoursO3")
                        {
                            m--;
                        }
                        else
                        {
                            cells[rowIndex, m].PutValue(drNew["Primary" + itemFactor].ToString());
                        }
                        
                        m++;
                    }
                }
                
            }
            //if (cblFactor.Contains("超标") && cblFactor.Contains("达标"))
            //{
            //    int j = mRow;
            //    cells[0, j].PutValue("超标天数");
            //    cells.Merge(0, j, 1, factors.Length);
            //    foreach (string itemFactor in factors)
            //    {
            //        if (itemFactor == "RecentoneHoursO3")
            //        {
            //            cells[1, j].PutValue("O3-1");
            //        }
            //        else if (itemFactor == "Recent8HoursO3")
            //        {
            //            cells[1, j].PutValue("O3-8");
            //        }
            //        else
            //        {
            //            cells[1, j].PutValue(itemFactor);
            //        }
            //        j++;
            //    }
            //    cells[0, j].PutValue("达标天数");
            //    cells.Merge(0, j, 1, factors.Length);
            //    foreach (string itemFactor in factors)
            //    {
            //        if (itemFactor == "RecentoneHoursO3")
            //        {
            //            cells[1, j].PutValue("O3-1");
            //        }
            //        else if (itemFactor == "Recent8HoursO3")
            //        {
            //            cells[1, j].PutValue("O3-8");
            //        }
            //        else
            //        {
            //            cells[1, j].PutValue(itemFactor);
            //        }
            //        j++;
            //    }
            //    for (int i = 0; i < dtNew.Rows.Count; i++)
            //    {
            //        int datarow = 5;
            //        DataRow drNew = dtNew.Rows[i];
            //        int rowIndex = i + 2;
            //        cells[rowIndex, 0].PutValue(drNew["RegionName"].ToString());
            //        cells[rowIndex, 1].PutValue(drNew["StandardDays"].ToString());
            //        cells[rowIndex, 2].PutValue(drNew["OverDays"].ToString());
            //        cells[rowIndex, 3].PutValue(drNew["InvalidDays"].ToString());
            //        cells[rowIndex, 4].PutValue(drNew["StandardDaysRate"].ToString());
            //        foreach (string year in years)
            //        {
            //            cells[rowIndex, datarow].PutValue(drNew[year].ToString());
            //            datarow++;
            //        }
            //        int m = datarow;
            //        foreach (string itemFactor in factors)
            //        {
            //            if (itemFactor == "RecentoneHoursO3")
            //            {
            //                cells[rowIndex, m].PutValue(drNew["OverO3"].ToString());
            //            }
            //            else
            //            {
            //                cells[rowIndex, m].PutValue(drNew["Over" + itemFactor].ToString());
            //            }
            //            m++;
            //        }
            //        foreach (string itemFactor in factors)
            //        {
            //            if (itemFactor == "RecentoneHoursO3")
            //            {
            //                cells[rowIndex, m].PutValue(drNew["StandO3"].ToString());
            //            }
            //            else
            //            {
            //                cells[rowIndex, m].PutValue(drNew["Stand" + itemFactor].ToString());
            //            }
            //            m++;
            //        }
            //    }
            //}
            //else if (cblFactor.Contains("超标"))
            //{
            //    int j = mRow;
            //    cells[0, j].PutValue("超标天数");
            //    cells.Merge(0, j, 1, factors.Length);
            //    foreach (string itemFactor in factors)
            //    {
            //        if (itemFactor == "RecentoneHoursO3")
            //        {
            //            cells[1, j].PutValue("O3-1");
            //        }
            //        else if (itemFactor == "Recent8HoursO3")
            //        {
            //            cells[1, j].PutValue("O3-8");
            //        }
            //        else
            //        {
            //            cells[1, j].PutValue(itemFactor);
            //        }
            //        j++;
            //    }
            //    for (int i = 0; i < dtNew.Rows.Count; i++)
            //    {
            //        int dataRow = 5;
            //        DataRow drNew = dtNew.Rows[i];
            //        int rowIndex = i + 2;
            //        cells[rowIndex, 0].PutValue(drNew["RegionName"].ToString());
            //        cells[rowIndex, 1].PutValue(drNew["StandardDays"].ToString());
            //        cells[rowIndex, 2].PutValue(drNew["OverDays"].ToString());
            //        cells[rowIndex, 3].PutValue(drNew["InvalidDays"].ToString());
            //        cells[rowIndex, 4].PutValue(drNew["StandardDaysRate"].ToString());
            //        foreach (string year in years)
            //        {
            //            cells[rowIndex, dataRow].PutValue(drNew[year].ToString());
            //            dataRow++;
            //        }
            //        int m = dataRow;
            //        foreach (string itemFactor in factors)
            //        {
            //            if (itemFactor == "RecentoneHoursO3")
            //            {
            //                cells[rowIndex, m].PutValue(drNew["OverO3"].ToString());
            //            }
            //            else
            //            {
            //                cells[rowIndex, m].PutValue(drNew["Over" + itemFactor].ToString());
            //            }
            //            m++;
            //        }
            //    }
            //}
            //else if (cblFactor.Contains("达标"))
            //{
            //    int j = mRow;
            //    cells[0, j].PutValue("达标天数");
            //    cells.Merge(0, j, 1, factors.Length);
            //    foreach (string itemFactor in factors)
            //    {
            //        if (itemFactor == "RecentoneHoursO3")
            //        {
            //            cells[1, j].PutValue("O3-1");
            //        }
            //        else if (itemFactor == "Recent8HoursO3")
            //        {
            //            cells[1, j].PutValue("O3-8");
            //        }
            //        else
            //        {
            //            cells[1, j].PutValue(itemFactor);
            //        }
            //        j++;
            //    }
            //    for (int i = 0; i < dtNew.Rows.Count; i++)
            //    {
            //        int dataRow = 5;
            //        DataRow drNew = dtNew.Rows[i];
            //        int rowIndex = i + 2;
            //        cells[rowIndex, 0].PutValue(drNew["RegionName"].ToString());
            //        cells[rowIndex, 1].PutValue(drNew["StandardDays"].ToString());
            //        cells[rowIndex, 2].PutValue(drNew["OverDays"].ToString());
            //        cells[rowIndex, 3].PutValue(drNew["InvalidDays"].ToString());
            //        cells[rowIndex, 4].PutValue(drNew["StandardDaysRate"].ToString());
            //        foreach (string year in years)
            //        {
            //            cells[rowIndex, dataRow].PutValue(drNew[year].ToString());
            //            dataRow++;
            //        }
            //        int m = dataRow;
            //        foreach (string itemFactor in factors)
            //        {
            //            if (itemFactor == "RecentoneHoursO3")
            //            {
            //                cells[rowIndex, m].PutValue(drNew["StandO3"].ToString());
            //            }
            //            else
            //            {
            //                cells[rowIndex, m].PutValue(drNew["Stand" + itemFactor].ToString());
            //            }
            //            m++;
            //        }
            //    }
            //}
            //else if (cblFactor.Contains("首要污染物"))
            //{
            //    int j = mRow;
            //    cells[0, j].PutValue("首要污染物天数");
            //    cells.Merge(0, j, 1, factors.Length);
            //    foreach (string itemFactor in factors)
            //    {
            //        if (itemFactor == "RecentoneHoursO3")
            //        {
            //            cells[1, j].PutValue("O3-1");
            //        }
            //        else if (itemFactor == "Recent8HoursO3")
            //        {
            //            cells[1, j].PutValue("O3-8");
            //        }
            //        else
            //        {
            //            cells[1, j].PutValue(itemFactor);
            //        }
            //        j++;
            //    }
            //    for (int i = 0; i < dtNew.Rows.Count; i++)
            //    {
            //        int dataRow = 5;
            //        DataRow drNew = dtNew.Rows[i];
            //        int rowIndex = i + 2;
            //        cells[rowIndex, 0].PutValue(drNew["RegionName"].ToString());
            //        cells[rowIndex, 1].PutValue(drNew["StandardDays"].ToString());
            //        cells[rowIndex, 2].PutValue(drNew["OverDays"].ToString());
            //        cells[rowIndex, 3].PutValue(drNew["InvalidDays"].ToString());
            //        cells[rowIndex, 4].PutValue(drNew["StandardDaysRate"].ToString());
            //        foreach (string year in years)
            //        {
            //            cells[rowIndex, dataRow].PutValue(drNew[year].ToString());
            //            dataRow++;
            //        }
            //        int m = dataRow;
            //        foreach (string itemFactor in factors)
            //        {
            //            if (itemFactor == "RecentoneHoursO3")
            //            {
            //                cells[rowIndex, m].PutValue(drNew["StandO3"].ToString());
            //            }
            //            else
            //            {
            //                cells[rowIndex, m].PutValue(drNew["Stand" + itemFactor].ToString());
            //            }
            //            m++;
            //        }
            //    }
            //}
            cells.SetRowHeight(1, 20);//设置行高
            cells.SetColumnWidth(0, 20);//设置列宽
            cells.SetColumnWidth(1, 20);//设置列宽
            cells.SetColumnWidth(2, 20);//设置列宽
            cells.SetColumnWidth(3, 20);//设置列宽
            cells.SetColumnWidth(4, 20);//设置列宽
            foreach (Cell cell in cells)
            {
                if (!cell.IsStyleSet)
                {
                    cell.SetStyle(cellStyle);
                }
            }
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", string.Format("attachment;filename={0}.xls", HttpUtility.UrlEncode(fileName)));
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.ContentType = "application/ms-excel";
            Response.BinaryWrite(workbook.SaveToStream().ToArray());
            Response.End();
        }

        /// <summary>
        /// 根据所选站点ID获取相应区域信息
        /// </summary>
        /// <param name="pointIds"></param>
        /// <returns></returns>
        public DataView GetRegionByPointId(string[] pointIds)
        {
            return pointAirService.GetRegionByPointId(pointIds);
        }
    }
}