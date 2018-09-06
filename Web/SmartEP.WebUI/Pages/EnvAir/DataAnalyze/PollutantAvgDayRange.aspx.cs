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
using SmartEP.Core.Enums;
using SmartEP.Service.DataAnalyze.Air.AQIReport;
using SmartEP.Core.Interfaces;
using SmartEP.Service.Frame;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.Core.Enums;
using Aspose.Cells;
using SmartEP.Core.Generic;
using SmartEP.DomainModel.BaseData;

namespace SmartEP.WebUI.Pages.EnvAir.DataAnalyze
{
    /// <summary>
    /// 名称：PollutantAvgDayRange.aspx.cs
    /// 创建人：刘长敏
    /// 创建日期：2015-08-20
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：各项污染物日均值浓度范围
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class PollutantAvgDayRange : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        private DayAQIService m_DayAQIService;
        DictionaryService dicService = new DictionaryService();
        MonitoringPointAirService pointAirService = new MonitoringPointAirService();
        /// <summary>
        /// 选择站点
        /// </summary>
        private IList<IPoint> points = null;
        /// <summary>
        /// 统计
        /// </summary>
        DataView dvStatistical = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            m_DayAQIService = new DayAQIService();
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
            rdpBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToString("yyyy-MM"));
            rdpEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"));

            //国控点，对照点，路边站
            MonitoringPointAirService m_MonitoringPointAirService = Singleton<MonitoringPointAirService>.GetInstance();
            string strpointName = "";
            IQueryable<MonitoringPointEntity> EnableOrNotports = m_MonitoringPointAirService.RetrieveAirMPListByEnable();
            string[] EnableOrNotportsarry = EnableOrNotports.Where(p => p.ContrlUid == "6fadff52-2338-4319-9f1d-7317823770ad" || p.ContrlUid == "bdf0837a-eb59-4c4a-a05f-c774a17f3077" || p.ContrlUid == "c1158eb6-4d69-4846-a963-d16b9d2794ca").Select(p => p.MonitoringPointName).ToArray();
            foreach (string point in EnableOrNotportsarry)
            {
                strpointName += point + ";";
            }
            pointCbxRsm.SetPointValuesFromNames(strpointName);

            ////苏州市区
            //comboCityProper.DataSource = dicService.RetrieveRegionList(CityType.SuZhou);
            //comboCityProper.DataTextField = "ItemText";
            //comboCityProper.DataValueField = "ItemGuid";
            //comboCityProper.DataBind();
            //for (int i = 0; i < comboCityProper.Items.Count; i++)
            //{
            //    comboCityProper.Items[i].Checked = true;
            //}

            ////城市均值
            //for (int i = 0; i < comboCity.Items.Count; i++)
            //{
            //    comboCity.Items[i].Checked = true;
            //}

            ////创模均值
            //comboCityModel.DataSource = dicService.RetrieveCityList();
            //comboCityModel.DataTextField = "ItemText";
            //comboCityModel.DataValueField = "ItemGuid";
            //comboCityModel.DataBind();
            //comboCityModel.DataSource = pointAirService.RetrieveAirMPListByCityModel(CityType.SuZhou);
            //comboCityModel.DataTextField = "MonitoringPointName";
            //comboCityModel.DataValueField = "PointId";
            //comboCityModel.DataBind();
            //if (comboCityModel.Items.Count > 0)
            //{
            //    comboCityModel.Items[0].Checked = true;
            //}

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
            points = pointCbxRsm.GetPoints();
            switch (rbtnlType.SelectedValue)
            {
                case "Port":
                    portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
                    break;
                case "CityProper":
                    foreach (RadComboBoxItem item in comboCity.CheckedItems)
                    {
                        regionGuid += (item.Value.ToString() + ",");
                    }
                    break;
            }
            string[] regionGuids = regionGuid.Trim(',').Split(',');

            string factor = "";
            foreach (RadComboBoxItem item in rcbfactor.CheckedItems)
            {
                factor += (item.Value.ToString() + ",");
            }
            string[] factors = factor.Trim(',').Split(',');

            DateTime dtBegion = DateTime.Now;
            DateTime dtEnd = DateTime.Now;
            dtBegion = rdpBegin.SelectedDate.Value;
            dtEnd = rdpEnd.SelectedDate.Value.AddMonths(+1).AddDays(-1);
            //生成RadGrid的绑定列
            dvStatistical = null;
            //是否显示统计行
            if (IsStatistical.Checked)
            {
                if (rbtnlType.SelectedValue == "Port")
                {
                    if (portIds != null && factors != null)
                    {
                        grdAvgDayRange.ShowFooter = true;
                        dvStatistical = m_DayAQIService.GetPointStatisticalRange(portIds, factors, dtBegion, dtEnd);
                    }
                    else
                    {
                        dvStatistical = new DataView();
                    }
                }
                else if (rbtnlType.SelectedValue == "CityProper")
                {
                    if (regionGuids != null && factors != null)
                    {
                        grdAvgDayRange.ShowFooter = true;
                        dvStatistical = m_DayAQIService.GetRegionStatisticalRange(regionGuids, factors, dtBegion, dtEnd);
                    }
                    else
                    {
                        dvStatistical = new DataView();
                    }
                }

            }
            else
            {
                grdAvgDayRange.ShowFooter = false;
            }
            //每页显示数据个数            
            int pageSize = grdAvgDayRange.PageSize;
            //当前页的序号
            int pageNo = grdAvgDayRange.CurrentPageIndex;

            var AvgDayData = new DataView();

            //点位
            if (rbtnlType.SelectedValue == "Port")
            {
                if (portIds != null)
                {
                    AvgDayData = m_DayAQIService.GetPointPollutantAvgDayRange(portIds, factors, dtBegion, dtEnd);
                    AvgDayData.Sort = "PortId asc";
                    grdAvgDayRange.DataSource = AvgDayData;
                    grdAvgDayRange.VirtualItemCount = AvgDayData.Count;
                }
                else
                {
                    grdAvgDayRange.DataSource = new DataTable();
                }
            }
            //苏州市区、市区均值
            else if (rbtnlType.SelectedValue == "CityProper")
            {
                if (regionGuids != null)
                {
                    AvgDayData = m_DayAQIService.GetRegionPollutantDayRange(regionGuids, factors, dtBegion, dtEnd);
                    AvgDayData.Sort = "Number";
                    grdAvgDayRange.DataSource = AvgDayData;
                    grdAvgDayRange.VirtualItemCount = AvgDayData.Count;
                }
                else
                {
                    grdAvgDayRange.DataSource = new DataTable();
                }
            }
            //数据总行数

        }

        #endregion

        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            if (rdpBegin.SelectedDate.Value.Year != rdpEnd.SelectedDate.Value.Year)
            {
                Alert("时间范围不能跨年！");
                return;
            }
            grdAvgDayRange.CurrentPageIndex = 0;
            grdAvgDayRange.Rebind();
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
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                DataRowView drv = e.Item.DataItem as DataRowView;
            }
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
                if (rdpBegin.SelectedDate == null || rdpEnd.SelectedDate == null)
                {
                    //Alert("开始时间或者终止时间，不能为空！");
                    return;
                }
                else if (rdpBegin.SelectedDate > rdpEnd.SelectedDate)
                {
                    //Alert("开始时间不能大于终止时间！");
                    return;
                }

                string[] pointIds = pointCbxRsm.GetPoints().Select(t => t.PointID).ToArray();
                string factor = "";
                foreach (RadComboBoxItem item in rcbfactor.CheckedItems)
                {
                    factor += (item.Value.ToString() + ",");
                }
                string[] factors = factor.Trim(',').Split(',');
                DateTime dtmBegion = rdpBegin.SelectedDate.Value;
                DateTime dtmEnd = rdpEnd.SelectedDate.Value;
                DataView dv = new DataView();
                if (rbtnlType.SelectedValue == "Port")
                {
                    if (pointIds != null)
                    {
                        dv = m_DayAQIService.GetPointPollutantAvgDayRange(pointIds, factors, dtmBegion, dtmEnd);
                        dv.Sort = "PortId asc";
                    }
                    if (IsStatistical.Checked)
                    {
                        dvStatistical = m_DayAQIService.GetPointStatisticalRange(pointIds, factors, dtmBegion, dtmEnd);
                    }
                }
                else if (rbtnlType.SelectedValue == "CityProper")
                {
                    string[] regionUids = { };
                    switch (rbtnlType.SelectedValue)
                    {
                        case "CityProper":
                            regionUids = comboCity.CheckedItems.Select(t => t.Value).ToArray();
                            break;
                        default: break;
                    }
                    if (regionUids != null)
                    {
                        dv = m_DayAQIService.GetRegionPollutantDayRange(regionUids, factors, dtmBegion, dtmEnd);
                        dv.Sort = "Number";

                    }
                    if (IsStatistical.Checked)
                    {
                        dvStatistical = m_DayAQIService.GetRegionStatisticalRange(regionUids, factors, dtmBegion, dtmEnd);
                    }
                }
                DataTable dtNew = dv.ToTable();
                if (IsStatistical.Checked)
                {
                    if (dvStatistical != null && dvStatistical.Table.Rows.Count > 0)
                    {
                        DataTable dtStatistical = dvStatistical.Table;
                        DataRow drNameRow = dtNew.NewRow();
                        drNameRow["RegionName"] = "区域/测点";
                        DataRow drRangeRow = dtNew.NewRow();
                        drRangeRow["RegionName"] = "浓度范围";
                        for (int i = 0; i < dtStatistical.Rows.Count; i++)
                        {
                            DataRow drStatistical = dtStatistical.Rows[i];
                            foreach (string factorName in factors)
                            {
                                drNameRow[factorName + "Range"] = drStatistical["RegionName"] != DBNull.Value ? drStatistical["RegionName"] : "--";
                                drRangeRow[factorName + "Range"] = drStatistical[factorName + "Range"] != DBNull.Value ? drStatistical[factorName + "Range"] : "--";
                            }
                        }
                        dtNew.Rows.Add(drNameRow);
                        dtNew.Rows.Add(drRangeRow);
                    }
                }

                DataTableToExcel(dtNew.DefaultView, "污染物日均值浓度统计", "污染物日均值浓度统计");
            }
        }

        /// <summary>
        /// 导出空气质量实时报
        /// </summary>
        /// <param name="dv">原始数据表</param>
        /// <returns></returns>
        private void DataTableToExcel(DataView dv, string fileName, string sheetName)
        {
            points = pointCbxRsm.GetPoints();
            string factor = "";
            foreach (RadComboBoxItem item in rcbfactor.CheckedItems)
            {
                factor += (item.Value.ToString() + ",");
            }
            string[] factors = factor.Trim(',').Split(',');
            DataTable dtNew = dv.ToTable();
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
            if (!dtNew.Columns.Contains("PointName"))
            {
                dtNew.Columns.Add("PointName");
            }
            for (int i = 0; i < dtNew.Rows.Count; i++)
            {
                DataRow drNew = dtNew.Rows[i];
                if (rbtnlType.SelectedValue == "Port")
                {
                    drNew["PointName"] = (points.Count(t => t.PointID == drNew["PointName"].ToString()) > 0)
                     ? points.Where(t => t.PointID == drNew["PointName"].ToString()).Select(t => t.PointName).FirstOrDefault()
                     : drNew["PointName"].ToString();
                }
                else if (rbtnlType.SelectedValue == "CityProper")
                {
                    RadComboBox comboBox = null;
                    switch (rbtnlType.SelectedValue)
                    {
                        case "CityProper":
                            comboBox = comboCity;
                            break;
                        default: break;
                    }
                    if (comboBox != null)
                    {
                        drNew["PointName"] = (comboBox.Items.Count(t => t.Value == drNew["RegionName"].ToString()) > 0)
                         ? comboBox.Items.Where(t => t.Value == drNew["RegionName"].ToString()).Select(t => t.Text).FirstOrDefault()
                         : drNew["RegionName"].ToString();
                    }
                }
            }

            //第一行
            cells[0, 0].PutValue("区域/测点");
            cells.Merge(0, 0, 2, 1);
            int count = 1;
            foreach (string factorName in factors)
            {
                switch (factorName)
                {
                    case "PM25":
                        cells[0, count].PutValue("PM2.5");
                        cells.Merge(0, count, 1, 1);
                        cells[1, count].PutValue("μg/m³");
                        cells.Merge(1, count, 1, 1);
                        break;
                    case "CO":
                        cells[0, count].PutValue(factorName);
                        cells.Merge(0, count, 1, 1);
                        cells[1, count].PutValue("mg/m³");
                        cells.Merge(1, count, 1, 1);
                        break;
                    case "Max8HourO3":
                        cells[0, count].PutValue("O3-8小时");
                        cells.Merge(0, count, 1, 1);
                        cells[1, count].PutValue("μg/m³");
                        cells.Merge(1, count, 1, 1);
                        break;
                    default:
                        cells[0, count].PutValue(factorName);
                        cells.Merge(0, count, 1, 1);
                        cells[1, count].PutValue("μg/m³");
                        cells.Merge(1, count, 1, 1);
                        break;
                }
                count++;
            }
            cells[0, count].PutValue("AQI");
            cells.Merge(0, count, 2, 1);
            cells.SetRowHeight(0, 20);//设置行高
            cells.SetRowHeight(1, 20);//设置行高
            //cells.SetColumnWidth(0, 15);//设置列宽
            //cells.SetColumnWidth(1, 15);//设置列宽
            //cells.SetColumnWidth(10, 20);//设置列宽
            for (int i = 0; i <= count; i++)
            {
                cells.SetColumnWidth(i, 13);//设置列宽
            }

            for (int i = 0; i < dtNew.Rows.Count; i++)
            {
                int j = 1;
                DataRow drNew = dtNew.Rows[i];
                int rowIndex = i + 2;
                cells[rowIndex, 0].PutValue(drNew["RegionName"].ToString());
                foreach (string factorName in factors)
                {
                    cells[rowIndex, j].PutValue(drNew[factorName + "Range"].ToString());
                    j++;
                }
                cells[rowIndex, j].PutValue(drNew["AQI"].ToString());
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
            Response.AppendHeader("Content-Disposition", string.Format("attachment;filename={0}.xls", HttpUtility.UrlEncode(fileName)));
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.ContentType = "application/ms-excel";
            Response.BinaryWrite(workbook.SaveToStream().ToArray());
            Response.End();
        }

        /// <summary>
        /// 查询类型切换事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rbtnlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            dvPoint.Style["display"] = "none";
            comboCity.Visible = false;
            switch (rbtnlType.SelectedValue)
            {
                case "CityProper":
                    comboCity.Visible = true;
                    break;
                case "Port":
                    dvPoint.Style["display"] = "normal";
                    break;
            }
        }
        #endregion

        protected void grdAvgDayRange_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            try
            {
                GridBoundColumn col = e.Column as GridBoundColumn;
                if (col == null)
                    return;
                if (col.DataField == "RegionName")
                {
                    col.HeaderText = "区域/测点";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterText = "区域/测点<br>浓度范围";
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                    col.AllowSorting = false;
                }
                else if (col.DataField == "AQI")
                {
                    col.HeaderText = "AQI";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                    col.AllowSorting = false;
                }
                else if (col.DataField.Contains("PM25") || col.DataField.Contains("PM10") || col.DataField.Contains("NO2") || col.DataField.Contains("SO2") || col.DataField.Contains("Max8HourO3"))
                {
                    col.Visible = true;
                    col.HeaderText = "μg/m³";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                    col.ColumnGroupName = col.DataField;
                    SetGridFooterText(col);
                    col.AllowSorting = false;
                }
                else if (col.DataField.Contains("CO"))
                {
                    col.Visible = true;
                    col.HeaderText = "mg/m³";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                    col.ColumnGroupName = "CO";
                    SetGridFooterText(col);
                    col.AllowSorting = false;
                }
                else if (col.DataField == "blankspaceColumn")
                {
                    col.HeaderText = string.Empty;
                    col.Visible = false;
                }
                else
                {
                    col.Visible = false;
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
                string Name = string.Empty;
                string Range = string.Empty;
                if (dvStatistical.Count > 0)
                {
                    Name = dvStatistical[0]["RegionName"] != DBNull.Value ? dvStatistical[0]["RegionName"].ToString() : "--";
                    Range = dvStatistical[0][col.DataField] != DBNull.Value ? dvStatistical[0][col.DataField].ToString() : "--";

                }
                col.FooterText = string.Format("{0}<br>{1}", Name, Range);
                col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
            }
        }
    }
}