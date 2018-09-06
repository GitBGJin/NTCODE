using SmartEP.DomainModel;
using SmartEP.Service.BaseData.BusinessRule;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Utilities.Office;
using SmartEP.WebControl.CbxRsm;
using SmartEP.WebUI.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SmartEP.Core.Interfaces;
using SmartEP.Service.DataAnalyze.Air.AQIReport;
using SmartEP.Core.Generic;
using SmartEP.Service.Frame;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.Core.Enums;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.Channel;

namespace SmartEP.WebUI.Pages.EnvAir.DataAnalyze
{
    /// <summary>
    /// 名称：PollutantAnalyze.aspx.cs
    /// 创建人：樊垂贺
    /// 创建日期：2015-08-20
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：污染物浓度变化分析
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class PollutantAnalyze : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        HourAQIService m_HourAQIService = Singleton<HourAQIService>.GetInstance();
        DictionaryService dicService = new DictionaryService();
        MonitoringPointAirService pointAirService = new MonitoringPointAirService();
        InstrumentChannelService m_InstrumentChannelService = new InstrumentChannelService();
        AirPollutantService m_AirPollutantService = new AirPollutantService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitControl();
            }
        }
        #region 页面控件初始化
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            dtpBegin.SelectedDate = DateTime.Now.AddMonths(-1);
            dtpEnd.SelectedDate = DateTime.Now;
            //ddlCityProper.DataSource = dicService.RetrieveCityList();
            //ddlCityProper.DataTextField = "ItemText";
            //ddlCityProper.DataValueField = "ItemGuid";
            //ddlCityProper.DataBind();
            //comboCity.DataSource = dicService.RetrieveCityList();
            //comboCity.DataTextField = "ItemText";
            //comboCity.DataValueField = "ItemGuid";
            //comboCity.DataBind();
            //comboCityModel.DataSource=pointAirService.RetrieveAirMPListByCityModel(CityType.SuZhou);
            //comboCityModel.DataTextField = "MonitoringPointName";
            //comboCityModel.DataValueField = "PointId";
            //comboCityModel.DataBind();
        }
        #endregion

        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {
            string regionGuid = "";
            string[] portIds = null;
            switch (rbtnlType.SelectedValue)
            {
                case "Port":
                    portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
                    break;
                case "CityProper":
                    regionGuid = ddlCityProper.SelectedValue;
                    break;
                //case "City":
                //    foreach (RadComboBoxItem item in comboCity.CheckedItems)
                //    {
                //        regionGuid += (item.Value.ToString() + ",");
                //    }
                //    break;
                //case "CityModel":
                //    foreach (RadComboBoxItem item in comboCityModel.CheckedItems)
                //    {
                //        regionGuid += (item.Value.ToString() + ",");
                //    }
                //    break;
            }
            string[] regionGuids = regionGuid.Trim(',').Split(',');
            DateTime dtBegion = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd 00:00:00"));
            DateTime dtEnd = Convert.ToDateTime(dtpEnd.SelectedDate.Value.ToString("yyyy-MM-dd 23:59:59"));
            //每页显示数据个数            
            int pageSize = grdPAnalyze.PageSize;
            //当前页的序号
            int currentPageIndex = grdPAnalyze.CurrentPageIndex;

            int recordTotal = 0;
            string orderby = "";
            //var dataView = new DataView();// g_OfflineBiz.GetGridViewPager(pageSize, currentPageIndex, GetWhereString(), out recordTotal);
            //if (dataView == null)
            //{
            //    dataView = new DataView();
            //}
            var analyzeDate = new DataView();
            DataTable dt = null;
            if (rbtnlType.SelectedValue == "Port")
            {
                //【给隐藏域赋值，用于显示Chart】
                SetHiddenData(portIds, dtBegion, dtEnd);

                orderby = "PointId,DateTime desc";
                if (portIds != null)
                {
                    analyzeDate = m_HourAQIService.GetPortDataPager(portIds, dtBegion, dtEnd, pageSize, currentPageIndex, out recordTotal, orderby);
                    //筛选数据源
                    dt = analyzeDate.ToTable(true, new string[] { "PointId", "DateTime", "PM25", "PM10", "NO2", "SO2", "CO", "O3", "Recent8HoursO3", "AQIValue", "PrimaryPollutant", "Class", "Grade" });
                }
            }
            else
            {
                //【给隐藏域赋值，用于显示Chart】
                SetHiddenData(regionGuids, dtBegion, dtEnd);

                orderby = "MonitoringRegionUid ,DateTime desc";
                analyzeDate = m_HourAQIService.GetRegionDataPager(regionGuids, dtBegion, dtEnd, pageSize, currentPageIndex, out recordTotal, orderby);
                dt = analyzeDate.ToTable(true, new string[] { "MonitoringRegionUid", "DateTime", "PM25", "PM10", "NO2", "SO2", "CO", "O3", "Recent8HoursO3", "AQIValue", "PrimaryPollutant", "Class", "Grade" });
            }
            if (dt != null)
            {
                int PM25Unit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a34004").PollutantDecimalNum);
                int PM10Unit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a34002").PollutantDecimalNum);
                int NO2Unit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a21004").PollutantDecimalNum);
                int SO2Unit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a21026").PollutantDecimalNum);
                int COUnit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a21005").PollutantDecimalNum);
                int O3Unit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a05024").PollutantDecimalNum);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["PM25"] != DBNull.Value)
                    {
                        //dt.Rows[i]["PM25"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["PM25"]), PM25Unit)).ToString();
                        dt.Rows[i]["PM25"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["PM25"]) * 1000, PM25Unit-3)).ToString();
                    }
                    if (dt.Rows[i]["PM10"] != DBNull.Value)
                    {
                        dt.Rows[i]["PM10"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["PM10"]) * 1000, PM10Unit-3)).ToString();
                        // dt.Rows[i]["PM10"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["PM10"]), PM10Unit)).ToString();
                    }
                    if (dt.Rows[i]["NO2"] != DBNull.Value)
                    {
                        dt.Rows[i]["NO2"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["NO2"]) * 1000, NO2Unit-3)).ToString();
                        //dt.Rows[i]["NO2"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["NO2"]), NO2Unit)).ToString();
                    }
                    if (dt.Rows[i]["SO2"] != DBNull.Value)
                    {
                        dt.Rows[i]["SO2"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["SO2"]) * 1000, SO2Unit-3)).ToString(); 
                        //dt.Rows[i]["SO2"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["SO2"]), SO2Unit)).ToString(); ;

                    }
                    if (dt.Rows[i]["CO"] != DBNull.Value)
                    {
                        dt.Rows[i]["CO"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["CO"]), COUnit)).ToString();

                    }
                    if (dt.Rows[i]["Recent8HoursO3"] != DBNull.Value)
                    {
                        dt.Rows[i]["Recent8HoursO3"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["Recent8HoursO3"]) * 1000, O3Unit-3)).ToString();
                        //dt.Rows[i]["Recent8HoursO3"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["Recent8HoursO3"]), O3Unit)).ToString();

                    }
                    if (dt.Rows[i]["O3"] != DBNull.Value)
                    {
                        dt.Rows[i]["O3"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["O3"]) * 1000, O3Unit-3)).ToString();
                        //dt.Rows[i]["O3"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["O3"]), O3Unit)).ToString();

                    }
                }
            }
            grdPAnalyze.DataSource = dt;//dataView;

            //数据分页的页数
            grdPAnalyze.VirtualItemCount = recordTotal;
        }

        #endregion

        /// <summary>
        /// 页面隐藏域控件赋值，将数据需要的参数放入隐藏域，各个参数间用‘|’分割，每个参数内部用‘；’分割
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtBegin"></param>
        /// <param name="dtEnd"></param>
        private void SetHiddenData(string[] portIds, DateTime dtBegin, DateTime dtEnd)
        {
            if (portIds != null)
            {
                //string[] factor = { "SO2", "CO", "NO2", "PM10", "O3", "Recent8HoursO3", "PM25" };
                string[] factor = rcbFactors.CheckedItems.Select(x => x.Value).ToArray();
                HiddenData.Value = string.Join(";", portIds) + "|" + string.Join(";", factor) + "|" + dtBegin + "|" + dtEnd + "|" + rbtnlType.SelectedValue + "|Air";
                HiddenChartType.Value = ChartType.SelectedValue;
            }
        }

        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            grdPAnalyze.Rebind();
            if (tabStrip.SelectedTab.Text == "图表")
            {
                RegisterScript("RefreshChart();");
            }
            else
            {
                FirstLoadChart.Value = "1";
            }
        }

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

        /// <summary>
        /// 数据行绑定处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grid_ItemDataBound(object sender, GridItemEventArgs e)
        {
            GridDataItem item = e.Item as GridDataItem;
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
                        if (item["PointId"] != null)
                        {
                            GridTableCell pointCell = (GridTableCell)item["PointId"];
                            IPoint point = points.FirstOrDefault(x => x.PointID.Equals(pointCell.Text.Trim()));
                            if (points != null)
                                pointCell.Text = point.PointName;
                        }

                    }
                    break;
                case "CityProper":
                    string regionGuid = "";
                    IList<string> list = new List<string>();
                    DictionaryService dictionary = new DictionaryService();
                    regionGuid = ddlCityProper.SelectedValue;
                    string[] regionGuids = regionGuid.Trim(',').Split(',');
                    if (item is GridDataItem)
                    {
                        if (item["MonitoringRegionUid"] != null)
                        {
                            GridTableCell regionCell = (GridTableCell)item["MonitoringRegionUid"];
                            for (int i = 0; i < regionGuids.Length; i++)
                            {
                                string RegionName = dictionary.GetCodeDictionaryTextByValue(regionGuids[i]);
                                regionCell.Text = RegionName;
                            }
                        }

                    }
                    break;
            }
        }

        /// <summary>
        /// 查询按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {

            grdPAnalyze.Rebind();
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
                string regionGuid = "";
                string[] portIds = null;
                switch (rbtnlType.SelectedValue)
                {
                    case "Port":
                        portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
                        break;
                    case "CityProper":
                        regionGuid = ddlCityProper.SelectedValue;
                        break;
                }
                string[] regionGuids = regionGuid.Trim(',').Split(',');
                DateTime dtBegion = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd 00:00:00"));
                DateTime dtEnd = Convert.ToDateTime(dtpEnd.SelectedDate.Value.ToString("yyyy-MM-dd 23:59:59"));
                //每页显示数据个数            
                int pageSize = 99999999;
                //当前页的序号
                int currentPageIndex = 0;

                int recordTotal = 0;
                string orderby = "";
                var analyzeDate = new DataView();
                DataTable dt = null;
                int PM25Unit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a34004").PollutantDecimalNum);
                int PM10Unit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a34002").PollutantDecimalNum);
                int NO2Unit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a21004").PollutantDecimalNum);
                int SO2Unit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a21026").PollutantDecimalNum);
                int COUnit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a21005").PollutantDecimalNum);
                int O3Unit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a05024").PollutantDecimalNum);

                if (rbtnlType.SelectedValue == "Port")
                {
                    orderby = "DateTime,PointId";
                    analyzeDate = m_HourAQIService.GetPortDataPager(portIds, dtBegion, dtEnd, pageSize, currentPageIndex, out recordTotal, orderby);
                    //筛选数据源
                    dt = analyzeDate.ToTable(true, new string[] { "PointId", "DateTime", "PM25", "PM10", "NO2", "SO2", "CO", "O3", "Recent8HoursO3", "AQIValue", "PrimaryPollutant", "Class", "Grade" });
                }
                else
                {
                    orderby = "DateTime,MonitoringRegionUid";
                    analyzeDate = m_HourAQIService.GetRegionDataPager(regionGuids, dtBegion, dtEnd, pageSize, currentPageIndex, out recordTotal, orderby);
                    dt = analyzeDate.ToTable(true, new string[] { "MonitoringRegionUid", "DateTime", "PM25", "PM10", "NO2", "SO2", "CO", "O3", "Recent8HoursO3", "AQIValue", "PrimaryPollutant", "Class", "Grade" });
                }
                dt.Columns.Add("DateTimeStr", typeof(string));
                foreach (DataRow row in dt.Rows)
                {
                    row["DateTimeStr"] = row["DateTime"].FormatToString("yyyy-MM-dd HH:mm");

                    if (row["PM25"] != DBNull.Value)
                    {
                        row["PM25"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(row["PM25"]) * 1000, 0)).ToString();
                        //row["PM25"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(row["PM25"]) * 1000, PM25Unit)).ToString();
                    }
                    if (row["PM10"] != DBNull.Value)
                    {
                        row["PM10"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(row["PM10"]) * 1000, 0)).ToString();
                        //row["PM10"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(row["PM10"]) , PM10Unit)).ToString();
                    }
                    if (row["NO2"] != DBNull.Value)
                    {
                        row["NO2"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(row["NO2"]) * 1000, 0)).ToString();
                        //row["NO2"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(row["NO2"]), NO2Unit)).ToString();
                    }
                    if (row["SO2"] != DBNull.Value)
                    {
                        row["SO2"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(row["SO2"]) * 1000, 0)).ToString();
                        //row["SO2"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(row["SO2"]) , SO2Unit)).ToString();
                    }
                    if (row["CO"] != DBNull.Value)
                    {
                        row["CO"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(row["CO"]), COUnit)).ToString();
                    }
                    if (row["Recent8HoursO3"] != DBNull.Value)
                    {
                        row["Recent8HoursO3"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(row["Recent8HoursO3"]) * 1000, 0)).ToString();
                        //row["Recent8HoursO3"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(row["Recent8HoursO3"]), O3Unit)).ToString();
                    }
                    if (row["O3"] != DBNull.Value)
                    {
                        row["O3"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(row["O3"]) * 1000, 0)).ToString();
                        //row["O3"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(row["O3"]), O3Unit)).ToString();
                    }
                }
                dt = UpdateExportColumnName(dt.DefaultView);
                ExcelHelper.DataTableToExcel(dt, "污染物变化分析", "污染物变化分析", this.Page);
            }
        }
        /// <summary>
        /// 修改要导出的数据表中的列名
        /// </summary>
        /// <param name="dv">污染物变化分析数据表</param>
        /// <returns></returns>
        private DataTable UpdateExportColumnName(DataView dv)
        {
            DataTable dtNew = dv.ToTable();
            if (!dtNew.Columns.Contains("PointName"))
            {
                dtNew.Columns.Add("PointName");
            }
            for (int i = 0; i < dtNew.Rows.Count; i++)
            {
                DataRow drNew = dtNew.Rows[i];
                if (rbtnlType.SelectedValue == "Port")
                {
                    IList<IPoint> points = pointCbxRsm.GetPoints();
                    drNew["PointName"] = (points.Count(t => t.PointID == drNew["PointId"].ToString()) > 0)
                     ? points.Where(t => t.PointID == drNew["PointId"].ToString()).Select(t => t.PointName).FirstOrDefault()
                     : drNew["PointId"].ToString();
                }
                else if (rbtnlType.SelectedValue == "CityProper")
                {
                    RadDropDownList comboBox = null;
                    comboBox = ddlCityProper;
                    if (comboBox != null)
                    {
                        drNew["PointName"] = (comboBox.Items.Count(t => t.Value == drNew["MonitoringRegionUid"].ToString()) > 0)
                         ? comboBox.Items.Where(t => t.Value == drNew["MonitoringRegionUid"].ToString()).Select(t => t.Text).FirstOrDefault()
                         : drNew["MonitoringRegionUid"].ToString();
                    }
                }
            }
            dtNew.Columns["PointName"].SetOrdinal(0);
            dtNew.Columns["DateTimeStr"].SetOrdinal(1);
            if (rbtnlType.SelectedValue == "Port")
            {
                dtNew.Columns.Remove("PointId");
            }
            else
            {
                dtNew.Columns.Remove("MonitoringRegionUid");
            }
            dtNew.Columns.Remove("DateTime");
            for (int i = 0; i < dtNew.Columns.Count; i++)
            {

                DataColumn dcNew = dtNew.Columns[i];
                if (dcNew.ColumnName == "PointName")
                {
                    if (rbtnlType.SelectedValue == "Port")
                    {
                        dcNew.ColumnName = "测点";
                    }
                    else if (rbtnlType.SelectedValue == "CityProper")
                    {
                        dcNew.ColumnName = "区域";
                    }
                }
                else if (dcNew.ColumnName == "DateTimeStr")
                {
                    dcNew.ColumnName = "日期";
                    dcNew.FormatToString("{0:yyyy-MM-dd HH:mm}");
                }
                else if (dcNew.ColumnName == "AQIValue")
                {
                    dcNew.ColumnName = "AQI";
                }
                else if (dcNew.ColumnName == "PrimaryPollutant")
                {
                    dcNew.ColumnName = "首要污染物";
                }
                else if (dcNew.ColumnName == "Class")
                {
                    dcNew.ColumnName = "类别";
                }
                else if (dcNew.ColumnName == "Grade")
                {
                    dcNew.ColumnName = "等级";
                }
                else if (dcNew.ColumnName == "DateTime")
                {
                    dtNew.Columns.Remove(dcNew);
                }
                foreach (RadComboBoxItem item in rcbFactors.Items)
                {
                    if (item.Value.Contains(dcNew.ColumnName))
                    {
                        dcNew.ColumnName = item.Text;
                    }
                }
            }
            return dtNew;
        }
        /// <summary>
        /// 查询类型切换事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rbtnlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            dvPoints.Style["display"] = "none";
            ddlCityProper.Visible = false;
            switch (rbtnlType.SelectedValue)
            {
                case "CityProper":
                    ddlCityProper.Visible = true;
                    break;
                case "Port":
                    //pointCbxRsm.Visible = true;
                    dvPoints.Style["display"] = "normal";
                    break;
            }
        }

        #endregion
        /// <summary>
        /// grid 创建列事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdPAnalyze_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            try
            {
                GridBoundColumn col = e.Column as GridBoundColumn;
                if (col == null)
                {
                    return;
                }
                if (col.DataField == "PointId")
                {
                    col.HeaderText = "测点";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "MonitoringRegionUid")
                {
                    col.HeaderText = "区域";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "DateTime")
                {
                    col = (GridDateTimeColumn)e.Column;
                    string tstcolformat = "{0:yyyy-MM-dd HH:mm}";
                    col.HeaderText = "日期";
                    col.DataFormatString = tstcolformat;
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(120);
                    col.ItemStyle.Width = Unit.Pixel(120);
                }
                else if (col.DataField == "AQIValue")
                {
                    col.HeaderText = "AQI";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                    col.HeaderStyle.Width = Unit.Pixel(60);
                    col.ItemStyle.Width = Unit.Pixel(60);
                }
                else if (col.DataField == "PrimaryPollutant")
                {
                    col.HeaderText = "首要污染物";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                    col.HeaderStyle.Width = Unit.Pixel(80);
                    col.ItemStyle.Width = Unit.Pixel(80);
                }
                else if (col.DataField == "Grade")
                {
                    col.HeaderText = "等级";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                    col.HeaderStyle.Width = Unit.Pixel(60);
                    col.ItemStyle.Width = Unit.Pixel(60);
                }
                else if (col.DataField == "Class")
                {
                    col.HeaderText = "类别";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                    col.HeaderStyle.Width = Unit.Pixel(80);
                    col.ItemStyle.Width = Unit.Pixel(80);
                }
                else
                    if (rcbFactors.CheckedItems.Count(t => t.Value.Contains(col.DataField)) > 0)
                    {
                        int radGridColWidthValue = int.Parse(radGridColWidth.Value.ToString());
                        //IPollutant factor = rcbFactors.FirstOrDefault(x => x.PollutantCode.Equals(e.Column.UniqueName));
                        //col.HeaderText = string.Format("{0}<br>({1})", factor.PollutantName, factor.PollutantMeasureUnit); 
                        col.HeaderText = rcbFactors.CheckedItems.Where(x => x.Value.Equals(e.Column.UniqueName)).Select(t => t.Text).FirstOrDefault();
                        col.EmptyDataText = "--";
                        col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                        col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                        col.HeaderStyle.Width = Unit.Pixel(radGridColWidthValue);
                        col.ItemStyle.Width = Unit.Pixel(radGridColWidthValue);
                        col.Visible = true;
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
    }
}