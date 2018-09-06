﻿using Aspose.Cells;
using log4net;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.Channel;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.DataAnalyze.Air;
using SmartEP.Service.DataAnalyze.Air.AQIReport;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.DataAnalyze
{
    /// <summary>
    /// 名称：AirQualityDayReport.cs
    /// 创建人：徐阳
    /// 创建日期：2017-06-29
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：站点实时AQI信息
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class RealTimeAQI_NT : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        private HourAQIService m_HourAQIService;

        /// <summary>
        /// 空气站点信息服务
        /// </summary>
        private MonitoringPointAirService pointAirService;

        /// <summary>
        /// 选择站点
        /// </summary>
        private IList<IPoint> points = null;

        /// <summary>
        /// 选择因子
        /// </summary>
        private IList<PollutantCodeEntity> factors = null;

        /// <summary>
        /// 区域Uid集合
        /// </summary>
        List<string> listRegionUids = new List<string>();

        static DateTime dtms = Convert.ToDateTime(DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:00"));
        /// <summary>
        /// 区域信息
        /// </summary>
        DataView dvRegion = new DataView();

        /// <summary>
        /// 页脚信息
        /// </summary>
        DataTable dtAvg = new DataTable();

        ILog log = LogManager.GetLogger("FileLogging");//获取一个日志记录器

        protected void Page_Load(object sender, EventArgs e)
        {
            m_HourAQIService = new HourAQIService();
            pointAirService = new MonitoringPointAirService();

            if (!IsPostBack)
            {
                InitControl();
                gridRealTimeAQI.CurrentPageIndex = 0;
                gridRealTimeAQI.Rebind();
                gridHoursAQI.CurrentPageIndex = 0;
                gridHoursAQI.DataSource = new DataTable();
                //timer.Enabled = true;

            }
        }
        //protected void timer_Tick(object sender, EventArgs e)
        //{
        //    gridRealTimeAQI.CurrentPageIndex = 0;
        //    gridRealTimeAQI.Rebind();
        //    gridHoursAQI.CurrentPageIndex = 0;
        //    gridHoursAQI.Rebind();
        //    timer.Enabled = false;
        //}
        #region 页面控件初始化
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            //dtpBegin.SelectedDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:00"));


            string names = ConfigurationManager.AppSettings["NT_RTPointName"].ToString();    //从配置文件获取默认站点
            pointCbxRsm.SetPointValuesFromNames(names);

            //从数据库取最新数据的时间并赋值给控件
            points = pointCbxRsm.GetPoints();
            DateTime time = m_HourAQIService.GetOriAQINewestTime(points.Select(t => t.PointID).ToArray());
            dtpBegin.SelectedDate = Convert.ToDateTime(time.ToString("yyyy-MM-dd HH:00:00"));
        }
        #endregion

        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {
            try
            {
                #region
                points = pointCbxRsm.GetPoints();
                factors = GetPollutantListByCalAQI();//获取参与评价AQI的常规6因子
                int pageSizeHour = gridRealTimeAQI.PageSize;  //小时每页显示数据个数  
                int pageNoHour = gridRealTimeAQI.CurrentPageIndex;   //小时当前页的序号
                int recordTotal = 0;  //数据总行数
                var dataViewHour = new DataView();
                string orderBy = "";
                DateTime dt = dtpBegin.SelectedDate.Value;
                if (points != null && points.Count > 0)
                {
                    if (tabStrip.SelectedIndex == 0)
                    {
                        orderBy = "PointId";
                        dataViewHour = m_HourAQIService.GetAirQualityNewestOriRTReport(points.Select(t => t.PointID).ToArray(), pageSizeHour, pageNoHour, out recordTotal, dt.AddHours(1).AddSeconds(-1), orderBy);

                        AirPollutantService m_AirPollutantService = new AirPollutantService();
                        AQICalculateService m_AQICalculateService = new AQICalculateService();
                        //DateTime dtMax = dataViewHour.Table.AsEnumerable().Select(d => d.Field<DateTime>("DateTime")).Max();
                        Double? SO2 = null;
                        Double? NO2 = null;
                        Double? PM10 = null;
                        Double? PM25 = null;
                        Double? CO = null;
                        Double? O3 = null;
                        if (dataViewHour.Table.Select("SO2 is not null and SO2 <>''").Count() > 0)
                        {
                            SO2 = Convert.ToDouble((dataViewHour.Table.AsEnumerable().Select(d => Convert.ToDecimal((d.Field<string>("SO2") != "" && d.Field<object>("SO2") != DBNull.Value) ? d.Field<string>("SO2") : "0")).Sum()) / (dataViewHour.Table.Select("SO2 is not null and SO2 <>''").Count()));
                        }
                        if (dataViewHour.Table.Select("NO2 is not null and NO2 <>''").Count() > 0)
                        {
                            NO2 = Convert.ToDouble((dataViewHour.Table.AsEnumerable().Select(d => Convert.ToDecimal((d.Field<string>("NO2") != "" && d.Field<object>("NO2") != DBNull.Value) ? d.Field<string>("NO2") : "0")).Sum()) / (dataViewHour.Table.Select("NO2 is not null and NO2 <>''").Count()));
                        }
                        if (dataViewHour.Table.Select("PM10 is not null and PM10 <>''").Count() > 0)
                        {
                            PM10 = Convert.ToDouble((dataViewHour.Table.AsEnumerable().Select(d => Convert.ToDecimal((d.Field<string>("PM10") != "" && d.Field<object>("PM10") != DBNull.Value) ? d.Field<string>("PM10") : "0")).Sum()) / (dataViewHour.Table.Select("PM10 is not null and PM10 <>''").Count()));
                        }
                        if (dataViewHour.Table.Select("PM25 is not null and PM25 <>''").Count() > 0)
                        {
                            PM25 = Convert.ToDouble((dataViewHour.Table.AsEnumerable().Select(d => Convert.ToDecimal((d.Field<string>("PM25") != "" && d.Field<object>("PM25") != DBNull.Value) ? d.Field<string>("PM25") : "0")).Sum()) / (dataViewHour.Table.Select("PM25 is not null and PM25 <>''").Count()));
                        }
                        if (dataViewHour.Table.Select("CO is not null and CO <>''").Count() > 0)
                        {
                            CO = Convert.ToDouble((dataViewHour.Table.AsEnumerable().Select(d => Convert.ToDecimal((d.Field<string>("CO") != "" && d.Field<object>("CO") != DBNull.Value) ? d.Field<string>("CO") : "0")).Sum()) / (dataViewHour.Table.Select("CO is not null and CO <>''").Count()));
                        }
                        if (dataViewHour.Table.Select("O3 is not null and O3 <>''").Count() > 0)
                        {
                            O3 = Convert.ToDouble((dataViewHour.Table.AsEnumerable().Select(d => Convert.ToDecimal((d.Field<string>("O3") != "" && d.Field<object>("O3") != DBNull.Value) ? d.Field<string>("O3") : "0")).Sum()) / (dataViewHour.Table.Select("O3 is not null and O3 <>''").Count()));
                        }
                        int? PM25Value = m_AQICalculateService.GetIAQI("a34004", PM25, 24);
                        int? PM10Value = m_AQICalculateService.GetIAQI("a34002", PM10, 24);
                        int? NO2Value = m_AQICalculateService.GetIAQI("a21004", NO2, 1);
                        int? SO2Value = m_AQICalculateService.GetIAQI("a21026", SO2, 1);
                        int? COValue = m_AQICalculateService.GetIAQI("a21005", CO, 1);
                        int? O3Value = m_AQICalculateService.GetIAQI("a05024", O3, 1);
                        string AQIValue = m_AQICalculateService.GetAQI_Max_CNV(SO2Value, NO2Value, PM10Value, COValue, O3Value, PM25Value, "V");
                        string primaryPollutant = m_AQICalculateService.GetAQI_Max_CNV(SO2Value, NO2Value, PM10Value, COValue, O3Value, PM25Value, "N");
                        string grade = m_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Grade");
                        string class_AQI = m_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Class");
                        string color = m_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
                        dataViewHour.RowFilter = "";
                        DataTable dtH = dataViewHour.ToTable();
                        DataRow drv = dtH.NewRow();
                        //drv["PointId"] = "平均值";
                        drv["DateTime"] = dtpBegin.SelectedDate;
                        drv["PM25"] = PM25;
                        drv["PM25_IAQI"] = PM25Value;
                        drv["PM10"] = PM10;
                        drv["PM10_IAQI"] = PM10Value;
                        drv["NO2"] = NO2;
                        drv["NO2_IAQI"] = NO2Value;
                        drv["SO2"] = SO2;
                        drv["SO2_IAQI"] = SO2Value;
                        drv["CO"] = CO;
                        drv["CO_IAQI"] = COValue;
                        drv["O3"] = O3;
                        drv["O3_IAQI"] = O3Value;
                        drv["AQIValue"] = AQIValue;
                        drv["PrimaryPollutant"] = primaryPollutant;
                        drv["Grade"] = grade;
                        drv["Class"] = class_AQI;
                        drv["RGBValue"] = color;
                        dtH.Rows.Add(drv);

                        gridRealTimeAQI.DataSource = dtH.AsDataView();
                        gridRealTimeAQI.VirtualItemCount = recordTotal + 1;

                        //if (IsStatistical.Checked)
                        //{
                        //    AirPollutantService m_AirPollutantService = new AirPollutantService();
                        //    AQICalculateService m_AQICalculateService = new AQICalculateService();
                        //    //dtAvg = new DataTable();
                        //    //dtAvg.Columns.Add("SO2", typeof(string));
                        //    //dtAvg.Columns.Add("SO2_IAQI", typeof(string));
                        //    //dtAvg.Columns.Add("NO2", typeof(string));
                        //    //dtAvg.Columns.Add("NO2_IAQI", typeof(string));
                        //    //dtAvg.Columns.Add("PM10", typeof(string));
                        //    //dtAvg.Columns.Add("PM10_IAQI", typeof(string));
                        //    //dtAvg.Columns.Add("PM25", typeof(string));
                        //    //dtAvg.Columns.Add("PM25_IAQI", typeof(string));
                        //    //dtAvg.Columns.Add("CO", typeof(string));
                        //    //dtAvg.Columns.Add("CO_IAQI", typeof(string));
                        //    //dtAvg.Columns.Add("O3", typeof(string));
                        //    //dtAvg.Columns.Add("O3_IAQI", typeof(string));
                        //    //dtAvg.Columns.Add("Recent8HoursO3NT", typeof(string));
                        //    //dtAvg.Columns.Add("Recent8HoursO3NT_IAQI", typeof(string));

                        //    Double SO2 = Convert.ToDouble((dataViewHour.Table.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<string>("SO2") != "" ? d.Field<string>("SO2") : "0")).Sum()) / dataViewHour.Count);
                        //    Double NO2 = Convert.ToDouble((dataViewHour.Table.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<string>("NO2") != "" ? d.Field<string>("NO2") : "0")).Sum()) / dataViewHour.Count);
                        //    Double PM10 = Convert.ToDouble((dataViewHour.Table.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<string>("PM10") != "" ? d.Field<string>("PM10") : "0")).Sum()) / dataViewHour.Count);
                        //    Double PM25 = Convert.ToDouble((dataViewHour.Table.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<string>("PM25") != "" ? d.Field<string>("PM25") : "0")).Sum()) / dataViewHour.Count);
                        //    Double CO = Convert.ToDouble((dataViewHour.Table.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<string>("CO") != "" ? d.Field<string>("CO") : "0")).Sum()) / dataViewHour.Count);
                        //    Double O3 = Convert.ToDouble((dataViewHour.Table.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<string>("O3") != "" ? d.Field<string>("O3") : "0")).Sum()) / dataViewHour.Count);
                        //    Double Recent8HoursO3NT = Convert.ToDouble((dataViewHour.Table.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<string>("Recent8HoursO3NT"))).Sum()) / dataViewHour.Count);
                        //    int? PM25Value = m_AQICalculateService.GetIAQI("a34004", PM25, 24);
                        //    int? PM10Value = m_AQICalculateService.GetIAQI("a34002", PM10, 24);
                        //    int? NO2Value = m_AQICalculateService.GetIAQI("a21004", NO2, 1);
                        //    int? SO2Value = m_AQICalculateService.GetIAQI("a21026", SO2, 1);
                        //    int? COValue = m_AQICalculateService.GetIAQI("a21005", CO, 1);
                        //    int? O3Value = m_AQICalculateService.GetIAQI("a05024", O3, 1);
                        //    int? Max8HourO3NTValue = m_AQICalculateService.GetIAQI("a05024", Recent8HoursO3NT, 8);

                        //    //DataRow dr = dtAvg.NewRow();
                        //    //dr["SO2"] = SO2.ToString();
                        //    //dr["SO2_IAQI"] = SO2Value.ToString();
                        //    //dr["NO2"] = NO2.ToString();
                        //    //dr["NO2_IAQI"] = NO2Value.ToString();
                        //    //dr["PM10"] = PM10.ToString();
                        //    //dr["PM10_IAQI"] = PM10Value.ToString();
                        //    //dr["PM25"] = PM25.ToString();
                        //    //dr["PM25_IAQI"] = PM25Value.ToString();
                        //    //dr["CO"] = CO.ToString();
                        //    //dr["CO_IAQI"] = COValue.ToString();
                        //    //dr["O3"] = O3.ToString();
                        //    //dr["O3_IAQI"] = O3Value.ToString();
                        //    //dr["Recent8HoursO3NT"] = Recent8HoursO3NT.ToString();
                        //    //dr["Recent8HoursO3NT_IAQI"] = Max8HourO3NTValue.ToString();
                        //    //dtAvg.Rows.Add(dr);

                        //    gridRealTimeAQI.ShowFooter = true;
                        //    gridRealTimeAQI.Columns[1].FooterText = "平均值";
                        //    gridRealTimeAQI.Columns[1].FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                        //    gridRealTimeAQI.Columns[3].FooterText = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(PM25), Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a34004").PollutantDecimalNum)) * 1000).ToString("G0");
                        //    gridRealTimeAQI.Columns[4].FooterText = PM25Value.ToString();
                        //    gridRealTimeAQI.Columns[5].FooterText = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(PM10), Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a34002").PollutantDecimalNum)) * 1000).ToString("G0");
                        //    gridRealTimeAQI.Columns[6].FooterText = PM10Value.ToString();
                        //    gridRealTimeAQI.Columns[7].FooterText = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(NO2), Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a21004").PollutantDecimalNum)) * 1000).ToString("G0");
                        //    gridRealTimeAQI.Columns[8].FooterText = NO2Value.ToString();
                        //    gridRealTimeAQI.Columns[9].FooterText = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(SO2), Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a21026").PollutantDecimalNum)) * 1000).ToString("G0");
                        //    gridRealTimeAQI.Columns[10].FooterText = SO2Value.ToString();
                        //    gridRealTimeAQI.Columns[11].FooterText = DecimalExtension.GetPollutantValue(Convert.ToDecimal(CO), Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a21005").PollutantDecimalNum)).ToString();
                        //    gridRealTimeAQI.Columns[12].FooterText = COValue.ToString();
                        //    gridRealTimeAQI.Columns[13].FooterText = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(O3), Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a05024").PollutantDecimalNum)) * 1000).ToString("G0");
                        //    gridRealTimeAQI.Columns[14].FooterText = O3Value.ToString();
                        //    gridRealTimeAQI.Columns[15].FooterText = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(Recent8HoursO3NT), Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a05024").PollutantDecimalNum)) * 1000).ToString("G0");
                        //    gridRealTimeAQI.Columns[16].FooterText = Max8HourO3NTValue.ToString();
                        //    for (int i = 3; i < 17; i++)
                        //    {
                        //        gridRealTimeAQI.Columns[i].FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                        //    }
                        //}
                        //else
                        //{
                        //    gridRealTimeAQI.ShowFooter = false;
                        //}

                        string[] pointIds = points.Select(t => t.PointID).ToArray();
                        string dtBegion = dt.AddHours(1).AddSeconds(-1).ToString();
                        if (IsCheck.Checked == true)
                        {
                            hdIsCheck.Value = "1";
                        }
                        else
                        {
                            hdIsCheck.Value = "0";
                        }
                        hddt.Value = dtBegion;
                        hdPointId.Value = string.Join(",", pointIds);
                    }
                    else if (tabStrip.SelectedIndex == 2)
                    {
                        string[] pointIds = points.Select(t => t.PointID).ToArray();
                        string dtBegion = dt.AddHours(1).AddSeconds(-1).ToString();
                        if (IsCheck.Checked == true)
                        {
                            hdIsCheck.Value = "1";
                        }
                        else
                        {
                            hdIsCheck.Value = "0";
                        }
                        hddt.Value = dtBegion;
                        hdPointId.Value = string.Join(",", pointIds);
                    }
                }
                else
                {
                    dataViewHour = null;
                    gridRealTimeAQI.DataSource = new DataTable();
                    gridHoursAQI.DataSource = new DataTable();
                }
                #endregion

            }
            catch (Exception ex)
            {
                log.Info(ex.ToString());
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
                GridBoundColumn col = e.Column as GridBoundColumn;
                if (col == null)
                    return;
                if (col.DataField.Equals("DateTime"))
                {
                    //col = (GridDateTimeColumn)e.Column;
                    //string tstcolformat = "{0:HH:00}";
                    col.HeaderText = "时间";
                    //col.DataFormatString = tstcolformat;
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(120);
                    col.ItemStyle.Width = Unit.Pixel(120);
                }
                else if (points.Select(x => x.PointID).Contains(col.DataField))
                {
                    //IPollutant factor = factors.FirstOrDefault(x => x.PollutantCode.Equals(col.DataField));
                    IPoint ip = points.FirstOrDefault(x => x.PointID.Equals(col.DataField));
                    col.HeaderText = string.Format("{0}", ip.PointName);
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                }
                else if (col.DataField.Equals("AQIValue"))
                {
                    col.HeaderText = "平均值";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                }
                else
                {
                    e.Column.Visible = false;
                }

            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// 获取根据单位转换浓度值后的新数据视图
        /// </summary>
        /// <param name="dv"></param>
        /// <returns></returns>
        private DataView GetNewViewByTurnData(DataView dv)
        {
            DataView dvNew = new DataView();
            DataTable dt = dv.ToTable();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];

                //mg/m3转成μg/m3的浓度值
                if (!string.IsNullOrWhiteSpace(dr["SO2"].ToString()))
                {
                    dr["SO2"] = (decimal.Parse(dr["SO2"].ToString()) * 1000).ToString("G0");
                }
                if (!string.IsNullOrWhiteSpace(dr["NO2"].ToString()))
                {
                    dr["NO2"] = (decimal.Parse(dr["NO2"].ToString()) * 1000).ToString("G0");
                }
                if (!string.IsNullOrWhiteSpace(dr["PM10"].ToString()))
                {
                    dr["PM10"] = (decimal.Parse(dr["PM10"].ToString()) * 1000).ToString("G0");
                }
                if (!string.IsNullOrWhiteSpace(dr["PM25"].ToString()))
                {
                    dr["PM25"] = (decimal.Parse(dr["PM25"].ToString()) * 1000).ToString("G0");
                }
                if (!string.IsNullOrWhiteSpace(dr["Max8HourO3"].ToString()))
                {
                    dr["Max8HourO3"] = (decimal.Parse(dr["Max8HourO3"].ToString()) * 1000).ToString("G0");
                }
                if (!string.IsNullOrWhiteSpace(dr["MaxOneHourO3"].ToString()))
                {
                    dr["MaxOneHourO3"] = (decimal.Parse(dr["MaxOneHourO3"].ToString()) * 1000).ToString("G0");
                }
            }
            dvNew = dt.AsDataView();
            return dvNew;
        }
        #endregion

        #region 服务器端控件事件处理
        /// <summary>
        /// 绑定数据源（用于分页、排序）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }


        protected void gridHoursAQI_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                DataRowView drv = e.Item.DataItem as DataRowView;
                if (item["DateTime"] != null)
                {
                    GridTableCell pointCell = (GridTableCell)item["DateTime"];
                    pointCell.Text = pointCell.Text + " 时";
                }
            }
        }

        /// <summary>
        /// 小时AQI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridHoursAQI_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindGridAQI();
        }

        public void BindGridAQI()
        {
            points = pointCbxRsm.GetPoints();
            DateTime dt = dtpBegin.SelectedDate.Value;
            DateTime dt2 = Convert.ToDateTime(dt.ToString("yyyy-MM-dd 00:00:00"));
            string orderBy = "PointId";
            DataView hoursAQI = m_HourAQIService.GetOriRTAQI(points.Select(t => t.PointID).ToArray(), dt2, dt.AddHours(1).AddSeconds(-1), orderBy);
            gridHoursAQI.DataSource = hoursAQI;
            gridHoursAQI.VirtualItemCount = hoursAQI.Count;
        }

        /// <summary>
        /// 小时数据行绑定处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grid_ItemDataBoundHour(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                DataRowView drv = e.Item.DataItem as DataRowView;
                if (item["PointId"] != null)
                {
                    GridTableCell pointCell = (GridTableCell)item["PointId"];
                    //pointCbxRsm.GetAllPoints();
                    IPoint point = points.FirstOrDefault(x => x.PointID.Equals(drv["PointId"].ToString().Trim()));

                    if (point != null)
                    {
                        pointCell.Text = string.Format("<a href='#' onclick='RowClick()'>{0}</a>", point.PointName);
                    }
                    else
                    {
                        pointCell.Text = "平均值";
                    }
                }
                if (item["DateTime"] != null)
                {
                    DateTime dt;
                    if (dtpBegin.SelectedDate == null)
                    {
                        dt = DateTime.Now;
                    }
                    else
                    {
                        dt = (DateTime)dtpBegin.SelectedDate;
                    }
                    GridTableCell dateTimeCell = (GridTableCell)item["DateTime"];
                    dateTimeCell.Text = dt.ToString("MM-dd HH时");
                    //GridTableCell dateTimeCell = (GridTableCell)item["DateTime"];
                    //DateTime dateTime;
                    //if (DateTime.TryParse(dateTimeCell.Text, out dateTime))
                    //{
                    //    dateTimeCell.Text = dateTime.ToString("MM-dd HH时");
                    //}
                }
                if (item["RGBValue"] != null)
                {
                    GridTableCell cell = item["RGBValue"] as GridTableCell;
                    cell.Style.Add("background-color", cell.Text);
                    cell.Text = string.Empty;
                }
                for (int i = 0; i < factors.Count; i++)
                {
                    string[] uniqueNames = GetUniqueNameByPollutantNameHour(factors[i].PollutantCode);
                    foreach (string uniqueName in uniqueNames)
                    {
                        if (drv.DataView.Table.Columns.Contains(uniqueName) && item[uniqueName] != null)
                        {
                            GridTableCell factorCell = (GridTableCell)item[uniqueName];
                            decimal pollutantValue;

                            if (decimal.TryParse(factorCell.Text, out pollutantValue))
                            {
                                //获取因子小数位,channelCode 因子代码 例：CO的code是a21005  （ channelCode=a21005）
                                AirPollutantService m_AirPollutantService = new AirPollutantService();
                                int DecimalNum = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo(factors[i].PollutantCode).PollutantDecimalNum);
                                //保留小数位数,value 需要进行小数位处理的数据 类型Decimal
                                if (uniqueName == "CO")
                                {
                                    factorCell.Text = DecimalExtension.GetPollutantValue(pollutantValue, DecimalNum).ToString();
                                }
                                else
                                {
                                    factorCell.Text = (DecimalExtension.GetPollutantValue(pollutantValue, DecimalNum) * 1000).ToString("G0");
                                }
                            }
                        }
                    }
                }
            }
        }

        //小时数据获取因子
        private string[] GetUniqueNameByPollutantNameHour(string pollutantName)
        {
            string[] returnValues = new string[0];
            switch (pollutantName)
            {
                case "a21026":
                    returnValues = new string[] { "SO2" };
                    break;
                case "a21004":
                    returnValues = new string[] { "NO2" };
                    break;
                case "a34002":
                    returnValues = new string[] { "PM10" };
                    break;
                case "a21005":
                    returnValues = new string[] { "CO" };
                    break;
                case "a05024":
                    returnValues = new string[] { "O3" };
                    break;
                case "a34004":
                    returnValues = new string[] { "PM25" };
                    break;
                default: break;
            }
            return returnValues;
        }

        /// <summary>
        /// 查询按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            //BindGrid();
            //gridRealTimeAQI.CurrentPageIndex = 0;
            //gridRealTimeAQI.Rebind();
            //gridHoursAQI.CurrentPageIndex = 0;
            //gridHoursAQI.Rebind();
            
            if (tabStrip.SelectedTab.Text == "列表")
            {
                gridRealTimeAQI.CurrentPageIndex = 0;
                if (dtms == Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:00")))
                {
                    dtpBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:00"));
                }
                gridRealTimeAQI.Rebind();
                FirstLoadChart.Value = "1";
            }
            if (tabStrip.SelectedTab.Text == "当日小时AQI")
            {
                gridHoursAQI.CurrentPageIndex = 0;
                gridHoursAQI.Rebind();
                FirstLoadChart.Value = "1";
            }
            if (tabStrip.SelectedTab.Text == "图表")
            {
                BindGrid();
                BindChart();
            }
            else
            {
                FirstLoadChart.Value = "1";
            }
        }

        /// <summary>
        /// 绑定图表
        /// </summary>
        private void BindChart()
        {
            RegisterScript("CreatChart();");
        }

        /// <summary>
        /// 小时数据ToolBar事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridRTB_ButtonClickHour(object sender, RadToolBarEventArgs e)
        {
            Telerik.Web.UI.RadToolBarButton button = (Telerik.Web.UI.RadToolBarButton)e.Item;
            if (button.CommandName == "ExportToExcel")
            {
                points = pointCbxRsm.GetPoints();
                int pageSizeHour = 9999;  //小时每页显示数据个数  
                int pageNoHour = 0;   //小时当前页的序号
                int recordTotal = 0;  //数据总行数
                var dataViewHour = new DataView();
                string orderBy = "";
                DateTime dt = dtpBegin.SelectedDate.Value;
                if (points != null && points.Count > 0)
                {
                    orderBy = "PointId";
                    //dataViewHour = m_HourAQIService.GetAirQualityOriRTReport(points.Select(t => t.PointID).ToArray(), dtmhourBegin, dtmhourEnd, pageSizeHour, pageNoHour, out recordTotal, orderBy);
                    dataViewHour = m_HourAQIService.GetAirQualityNewestOriRTReport(points.Select(t => t.PointID).ToArray(), pageSizeHour, pageNoHour, out recordTotal, dt.AddHours(1).AddSeconds(-1), orderBy);
                    AirPollutantService m_AirPollutantService = new AirPollutantService();
                    AQICalculateService m_AQICalculateService = new AQICalculateService();
                    //DateTime dtMax = dataViewHour.Table.AsEnumerable().Select(d => d.Field<DateTime>("DateTime")).Max();
                    //dataViewHour.RowFilter = "DateTime = '" + dtMax + "'";
                    Double? SO2 = null;
                    Double? NO2 = null;
                    Double? PM10 = null;
                    Double? PM25 = null;
                    Double? CO = null;
                    Double? O3 = null;
                    if (dataViewHour.Table.Select("SO2 is not null and SO2 <>''").Count() > 0)
                    {
                        SO2 = Convert.ToDouble((dataViewHour.Table.AsEnumerable().Select(d => Convert.ToDecimal((d.Field<string>("SO2") != "" && d.Field<object>("SO2") != DBNull.Value) ? d.Field<string>("SO2") : "0")).Sum()) / (dataViewHour.Table.Select("SO2 is not null and SO2 <>''").Count()));
                    }
                    if (dataViewHour.Table.Select("NO2 is not null and NO2 <>''").Count() > 0)
                    {
                        NO2 = Convert.ToDouble((dataViewHour.Table.AsEnumerable().Select(d => Convert.ToDecimal((d.Field<string>("NO2") != "" && d.Field<object>("NO2") != DBNull.Value) ? d.Field<string>("NO2") : "0")).Sum()) / (dataViewHour.Table.Select("NO2 is not null and NO2 <>''").Count()));
                    }
                    if (dataViewHour.Table.Select("PM10 is not null and PM10 <>''").Count() > 0)
                    {
                        PM10 = Convert.ToDouble((dataViewHour.Table.AsEnumerable().Select(d => Convert.ToDecimal((d.Field<string>("PM10") != "" && d.Field<object>("PM10") != DBNull.Value) ? d.Field<string>("PM10") : "0")).Sum()) / (dataViewHour.Table.Select("PM10 is not null and PM10 <>''").Count()));
                    }
                    if (dataViewHour.Table.Select("PM25 is not null and PM25 <>''").Count() > 0)
                    {
                        PM25 = Convert.ToDouble((dataViewHour.Table.AsEnumerable().Select(d => Convert.ToDecimal((d.Field<string>("PM25") != "" && d.Field<object>("PM25") != DBNull.Value) ? d.Field<string>("PM25") : "0")).Sum()) / (dataViewHour.Table.Select("PM25 is not null and PM25 <>''").Count()));
                    }
                    if (dataViewHour.Table.Select("CO is not null and CO <>''").Count() > 0)
                    {
                        CO = Convert.ToDouble((dataViewHour.Table.AsEnumerable().Select(d => Convert.ToDecimal((d.Field<string>("CO") != "" && d.Field<object>("CO") != DBNull.Value) ? d.Field<string>("CO") : "0")).Sum()) / (dataViewHour.Table.Select("CO is not null and CO <>''").Count()));
                    }
                    if (dataViewHour.Table.Select("O3 is not null and O3 <>''").Count() > 0)
                    {
                        O3 = Convert.ToDouble((dataViewHour.Table.AsEnumerable().Select(d => Convert.ToDecimal((d.Field<string>("O3") != "" && d.Field<object>("O3") != DBNull.Value) ? d.Field<string>("O3") : "0")).Sum()) / (dataViewHour.Table.Select("O3 is not null and O3 <>''").Count()));
                    }
                    int? PM25Value = m_AQICalculateService.GetIAQI("a34004", PM25, 24);
                    int? PM10Value = m_AQICalculateService.GetIAQI("a34002", PM10, 24);
                    int? NO2Value = m_AQICalculateService.GetIAQI("a21004", NO2, 1);
                    int? SO2Value = m_AQICalculateService.GetIAQI("a21026", SO2, 1);
                    int? COValue = m_AQICalculateService.GetIAQI("a21005", CO, 1);
                    int? O3Value = m_AQICalculateService.GetIAQI("a05024", O3, 1);
                    string AQIValue = m_AQICalculateService.GetAQI_Max_CNV(SO2Value, NO2Value, PM10Value, COValue, O3Value, PM25Value, "V");
                    string primaryPollutant = m_AQICalculateService.GetAQI_Max_CNV(SO2Value, NO2Value, PM10Value, COValue, O3Value, PM25Value, "N");
                    string grade = m_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Grade");
                    string class_AQI = m_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Class");
                    string color = m_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQIValue), "Color");
                    dataViewHour.RowFilter = "";
                    DataTable dtH = dataViewHour.ToTable();
                    DataRow drv = dtH.NewRow();
                    //drv["PointId"] = "平均值";
                    drv["DateTime"] = dtpBegin.SelectedDate;
                    drv["PM25"] = PM25;
                    drv["PM25_IAQI"] = PM25Value;
                    drv["PM10"] = PM10;
                    drv["PM10_IAQI"] = PM10Value;
                    drv["NO2"] = NO2;
                    drv["NO2_IAQI"] = NO2Value;
                    drv["SO2"] = SO2;
                    drv["SO2_IAQI"] = SO2Value;
                    drv["CO"] = CO;
                    drv["CO_IAQI"] = COValue;
                    drv["O3"] = O3;
                    drv["O3_IAQI"] = O3Value;
                    drv["AQIValue"] = AQIValue;
                    drv["PrimaryPollutant"] = primaryPollutant;
                    drv["Grade"] = grade;
                    drv["Class"] = class_AQI;
                    drv["RGBValue"] = color;
                    dtH.Rows.Add(drv);
                    dataViewHour = dtH.AsDataView();
                }
                else
                {
                    dataViewHour = null;
                }
                DataTableToExcelHour(dataViewHour, "空气质量实时报", "空气质量实时报");
            }
        }

        /// <summary>
        /// 导出空气质量实时报
        /// </summary>
        /// <param name="dv">原始数据表</param>
        /// <returns></returns>
        private void DataTableToExcelHour(DataView dv, string fileName, string sheetName)
        {
            DataTable dtNew = dv.ToTable();
            points = pointCbxRsm.GetPoints();
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

            #region 数据修改
            AirPollutantService m_AirPollutantService = new AirPollutantService();
            factors = GetPollutantListByCalAQI();//获取参与评价AQI的常规6因子
            if (!dtNew.Columns.Contains("PointName"))
            {
                dtNew.Columns.Add("PointName");
            }
            for (int i = 0; i < dtNew.Rows.Count; i++)
            {
                DataRow drNew = dtNew.Rows[i];

                //drNew["PointName"] = (points.Count(t => t.PointID == drNew["PointId"].ToString()) > 0)
                // ? points.Where(t => t.PointID == drNew["PointId"].ToString()).Select(t => t.PointName).FirstOrDefault()
                // : drNew["PointId"].ToString();

                drNew["PointName"] = (points.Count(t => t.PointID == drNew["PointId"].ToString()) > 0)
                 ? points.Where(t => t.PointID == drNew["PointId"].ToString()).Select(t => t.PointName).FirstOrDefault()
                 : "平均值";

                for (int j = 0; j < factors.Count; j++)
                {
                    string[] uniqueNames = GetUniqueNameByPollutantNameHour(factors[j].PollutantCode);
                    foreach (string uniqueName in uniqueNames)
                    {
                        if (dtNew.Columns.Contains(uniqueName) && !string.IsNullOrWhiteSpace(drNew[uniqueName].ToString()))
                        {
                            //获取因子小数位,channelCode 因子代码 例：CO的code是a21005  （ channelCode=a21005）
                            int DecimalNum = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo(factors[j].PollutantCode).PollutantDecimalNum);
                            decimal pollutantValue = decimal.TryParse(drNew[uniqueName].ToString(), out pollutantValue) ? pollutantValue : 0;

                            //保留小数位数,value 需要进行小数位处理的数据 类型Decimal
                            if (uniqueName == "CO")
                            {
                                drNew[uniqueName] = DecimalExtension.GetPollutantValue(pollutantValue, DecimalNum).ToString();
                            }
                            else
                            {
                                drNew[uniqueName] = (DecimalExtension.GetPollutantValue(pollutantValue, DecimalNum) * 1000).ToString("G0");
                            }
                        }
                    }
                }
            }
            #endregion

            #region 表头
            //第一行
            cells[0, 0].PutValue("监测点位名称");
            cells.Merge(0, 0, 3, 1);
            cells[0, 1].PutValue("日期");
            cells.Merge(0, 1, 3, 1);
            cells[0, 2].PutValue("污染物浓度及空气质量分指数（IAQI）");
            cells.Merge(0, 2, 1, 12);
            cells[0, 14].PutValue("空气质量指数(AQI)");
            cells.Merge(0, 14, 3, 1);
            cells[0, 15].PutValue("首要污染物");
            cells.Merge(0, 15, 3, 1);
            cells[0, 16].PutValue("空气质量指数级别");
            cells.Merge(0, 16, 3, 1);
            cells[0, 17].PutValue("空气质量指数类别");
            cells.Merge(0, 17, 2, 2);

            //第二行
            cells[1, 2].PutValue("PM2.5 1小时平均");
            cells.Merge(1, 2, 1, 2);
            cells[1, 4].PutValue("PM10 1小时平均");
            cells.Merge(1, 4, 1, 2);
            cells[1, 6].PutValue("二氧化氮(NO2)1小时平均");
            cells.Merge(1, 6, 1, 2);
            cells[1, 8].PutValue("二氧化硫(SO2)1小时平均");
            cells.Merge(1, 8, 1, 2);
            cells[1, 10].PutValue("一氧化碳(CO)1小时平均");
            cells.Merge(1, 10, 1, 2);
            cells[1, 12].PutValue("臭氧(O3)1小时平均");
            cells.Merge(1, 12, 1, 2);


            //第三行
            cells[2, 2].PutValue("浓度/(μg/m3)");
            cells[2, 3].PutValue("分指数");
            cells[2, 4].PutValue("浓度/(μg/m3)");
            cells[2, 5].PutValue("分指数");
            cells[2, 6].PutValue("浓度/(μg/m3)");
            cells[2, 7].PutValue("分指数");
            cells[2, 8].PutValue("浓度/(μg/m3)");
            cells[2, 9].PutValue("分指数");
            cells[2, 10].PutValue("浓度/(mg/m3)");
            cells[2, 11].PutValue("分指数");
            cells[2, 12].PutValue("浓度/(μg/m3)");
            cells[2, 13].PutValue("分指数");
            cells[2, 17].PutValue("类别");
            cells[2, 18].PutValue("颜色");
            cells.SetRowHeight(0, 20);//设置行高
            cells.SetRowHeight(1, 30);//设置行高
            cells.SetColumnWidth(0, 20);//设置列宽
            cells.SetColumnWidth(1, 13);//设置列宽
            cells.SetColumnWidth(17, 10);//设置列宽
            cells.SetColumnWidth(17, 10);//设置列宽
            cells.SetColumnWidth(18, 10);//设置列宽
            for (int i = 2; i <= 15; i++)
            {
                cells.SetColumnWidth(i, 10);//设置列宽
            }
            #endregion

            for (int i = 0; i < dtNew.Rows.Count; i++)
            {
                DataRow drNew = dtNew.Rows[i];
                int rowIndex = i + 3;
                cells[rowIndex, 0].PutValue(drNew["PointName"].ToString());
                if (dtNew.Columns.Contains("DateTime"))
                {
                    DateTime dateTime = DateTime.Parse(drNew["DateTime"].ToString()).AddHours(0);
                    cells[rowIndex, 1].PutValue(string.Format("{0:yyyy-MM-dd HH时}", dateTime));
                }
                else
                {
                    DateTime reportDateTime = DateTime.Parse(drNew["ReportDateTime"].ToString()).AddHours(0);
                    cells[rowIndex, 1].PutValue(string.Format("{0:yyyy-MM-dd HH时}", reportDateTime));
                }
                cells[rowIndex, 2].PutValue(drNew["PM25"].ToString() != "" ? drNew["PM25"].ToString() : "--");
                cells[rowIndex, 3].PutValue(drNew["PM25_IAQI"].ToString() != "" ? drNew["PM25_IAQI"].ToString() : "--");
                cells[rowIndex, 4].PutValue(drNew["PM10"].ToString() != "" ? drNew["PM10"].ToString() : "--");
                cells[rowIndex, 5].PutValue(drNew["PM10_IAQI"].ToString() != "" ? drNew["PM10_IAQI"].ToString() : "--");
                cells[rowIndex, 6].PutValue(drNew["NO2"].ToString() != "" ? drNew["NO2"].ToString() : "--");
                cells[rowIndex, 7].PutValue(drNew["NO2_IAQI"].ToString() != "" ? drNew["NO2_IAQI"].ToString() : "--");
                cells[rowIndex, 8].PutValue(drNew["SO2"].ToString() != "" ? drNew["SO2"].ToString() : "--");
                cells[rowIndex, 9].PutValue(drNew["SO2_IAQI"].ToString() != "" ? drNew["SO2_IAQI"].ToString() : "--");
                cells[rowIndex, 10].PutValue(drNew["CO"].ToString() != "" ? drNew["CO"].ToString() : "--");
                cells[rowIndex, 11].PutValue(drNew["CO_IAQI"].ToString() != "" ? drNew["CO_IAQI"].ToString() : "--");
                cells[rowIndex, 12].PutValue(drNew["O3"].ToString() != "" ? drNew["O3"].ToString() : "--");
                cells[rowIndex, 13].PutValue(drNew["O3_IAQI"].ToString() != "" ? drNew["O3_IAQI"].ToString() : "--");
                cells[rowIndex, 14].PutValue(drNew["AQIValue"].ToString() != "" ? drNew["AQIValue"].ToString() : "--");
                cells[rowIndex, 15].PutValue(drNew["PrimaryPollutant"].ToString() != "" ? drNew["PrimaryPollutant"].ToString() : "--");
                cells[rowIndex, 16].PutValue(drNew["Grade"].ToString() != "" ? drNew["Grade"].ToString() : "--");
                cells[rowIndex, 17].PutValue(drNew["Class"].ToString() != "" ? drNew["Class"].ToString() : "--");
                cells[rowIndex, 18].PutValue("");
                if (drNew["RGBValue"].ToString() != "")
                {
                    Aspose.Cells.Style styleTemp = cells[rowIndex, 18].GetStyle();
                    styleTemp.ForegroundColor = System.Drawing.ColorTranslator.FromHtml(drNew["RGBValue"].ToString());//设置背景色
                    styleTemp.Pattern = BackgroundType.Solid;//设置背景样式
                    cells[rowIndex, 18].SetStyle(styleTemp);
                }
            }
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
            //Response.AppendHeader("Content-Disposition", string.Format("attachment;filename={0}.xls", HttpUtility.UrlEncode(fileName) + DateTime.Now.ToString("yyyyMMddHHmmss")));
            Response.AppendHeader("Content-Disposition", string.Format("attachment;filename={0}.xls", HttpUtility.UrlEncode(fileName)));
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.ContentType = "application/ms-excel";
            Response.BinaryWrite(workbook.SaveToStream().ToArray());
            Response.End();
        }


        #endregion

        /// <summary>
        /// 根据测点Id数组获取因子列
        /// </summary>
        /// <param name="pointIds">测点Id数组</param>
        /// <returns></returns>
        private IList<PollutantCodeEntity> GetPollutantCodesByPointIds(string[] pointIds)
        {
            MonitoringPointAirService monitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            IQueryable<MonitoringPointEntity> monitorPointQueryable = monitoringPointAir.RetrieveListByPointIds(pointIds);//根据站点ID数组获取站点
            IList<PollutantCodeEntity> pollutantList = new List<PollutantCodeEntity>();
            InstrumentChannelService instrumentChannelService = Singleton<InstrumentChannelService>.GetInstance();//提供仪器通道信息服务
            foreach (MonitoringPointEntity monitoringPointEntity in monitorPointQueryable)
            {
                IQueryable<PollutantCodeEntity> pollutantCodeQueryable =
                    instrumentChannelService.RetrieveChannelListByPointUid(monitoringPointEntity.MonitoringPointUid);//根据站点Uid获取所有监测通道
                pollutantList = pollutantList.Union(pollutantCodeQueryable).ToList();
            }
            return pollutantList;
        }

        /// <summary>
        /// 获取参与评价AQI的常规6因子
        /// </summary>
        /// <returns></returns>
        private IList<PollutantCodeEntity> GetPollutantListByCalAQI()
        {
            AirPollutantService airPollutantService = new AirPollutantService();
            return airPollutantService.RetrieveListByCalAQI().ToList();
        }

        protected void rcbCityProper_ItemCreated(object sender, RadComboBoxItemEventArgs e)
        {
            e.Item.Checked = true;
        }

        /// <summary>
        /// 图表类型变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ChartContent_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            string Pic = ChartContent.SelectedValue;
            hdChartContent.Value = Pic;
            RegisterScript("PointFactor('" + Pic + "');");
        }

        /// <summary>
        /// 绘图类型变化(折线、柱形)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ChartType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string PicType = ChartType.SelectedValue;
            hdChartType.Value = PicType;
            RegisterScript("PointFactor('" + PicType + "');");
        }

    }
}