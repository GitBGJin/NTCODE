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
using SmartEP.Core.Generic;
using SmartEP.Service.Frame;
using SmartEP.Service.Core.Enums;
using SmartEP.Service.BaseData.Channel;
using Aspose.Cells;

namespace SmartEP.WebUI.Pages.EnvAir.DataAnalyze
{
    /// <summary>
    /// 名称：PrimaryPollutantAvgYearData.aspx.cs
    /// 创建人：樊垂贺
    /// 创建日期：2015-08-19
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：主要污染物年均值
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class PrimaryPollutantAvgYearData : BasePage
    {
        ///  /// <summary>
        /// 数据处理服务
        /// </summary>
        DayAQIService m_DayAQIService = Singleton<DayAQIService>.GetInstance();
        DictionaryService dicService = new DictionaryService();
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
            rdpBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd"));
            rdpEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
        }
        #endregion

        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {
            string regionGuid = "";
            foreach (RadComboBoxItem item in comboCityProper.CheckedItems)
            {
                regionGuid += (item.Value.ToString() + ",");
            }
            string[] regionGuids = regionGuid.Trim(',').Split(',');
            string factor = "";
            foreach (RadComboBoxItem item in rcbfactor.CheckedItems)
            {
                factor += (item.Value.ToString() + ",");
            }
            string[] factors = factor.Trim(',').Split(',');
            DateTime dateStart = Convert.ToDateTime(rdpBegin.SelectedDate.Value.ToString("yyyy-MM-dd") + " 00:00:00");
            DateTime dateEnd = Convert.ToDateTime(rdpEnd.SelectedDate.Value.ToString("yyyy-MM-dd") + " 23:59:59");

            DataView yearDate = m_DayAQIService.GetPrimaryPollutantAvgYearData(regionGuids, factors, dateStart, dateEnd);
            if (yearDate != null)
            {
                DataTable dt = yearDate.ToTable();
                int PM25Unit = (Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a34004").PollutantDecimalNum) - 3) < 0 ? 0 : (Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a34004").PollutantDecimalNum) - 3);
                int PM10Unit = (Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a34002").PollutantDecimalNum) - 3) < 0 ? 0 : (Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a34002").PollutantDecimalNum) - 3);
                int NO2Unit = (Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a21004").PollutantDecimalNum) - 3) < 0 ? 0 : (Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a21004").PollutantDecimalNum) - 3);
                int SO2Unit = (Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a21026").PollutantDecimalNum) - 3) < 0 ? 0 : (Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a21026").PollutantDecimalNum) - 3);
                int COUnit = (Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a21005").PollutantDecimalNum));
                int Max8HourO3Unit = (Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a05024").PollutantDecimalNum) - 3) < 0 ? 0 : (Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a05024").PollutantDecimalNum) - 3);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    foreach (string item in factors)
                    {
                        switch (item)
                        {
                            case "PM25":
                                if (dt.Rows[i]["PM25Concentration"] != DBNull.Value)
                                {
                                    dt.Rows[i]["PM25Concentration"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["PM25Concentration"]) * 1000, PM25Unit)).ToString();
                                }
                                if (dt.Rows[i]["PM25YearPercent"] != DBNull.Value)
                                {
                                    dt.Rows[i]["PM25YearPercent"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["PM25YearPercent"]) * 1000, PM25Unit)).ToString();
                                }
                                break;
                            case "PM10":
                                if (dt.Rows[i]["PM10Concentration"] != DBNull.Value)
                                {
                                    dt.Rows[i]["PM10Concentration"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["PM10Concentration"]) * 1000, PM10Unit)).ToString();
                                }
                                if (dt.Rows[i]["PM10YearPercent"] != DBNull.Value)
                                {
                                    dt.Rows[i]["PM10YearPercent"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["PM10YearPercent"]) * 1000, PM10Unit)).ToString();
                                }
                                break;
                            case "SO2":
                                if (dt.Rows[i]["SO2Concentration"] != DBNull.Value)
                                {
                                    dt.Rows[i]["SO2Concentration"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["SO2Concentration"]) * 1000, SO2Unit)).ToString(); ;
                                }
                                if (dt.Rows[i]["SO2YearPercent"] != DBNull.Value)
                                {
                                    dt.Rows[i]["SO2YearPercent"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["SO2YearPercent"]) * 1000, SO2Unit)).ToString(); ;
                                }
                                break;
                            case "NO2":
                                if (dt.Rows[i]["NO2Concentration"] != DBNull.Value)
                                {
                                    dt.Rows[i]["NO2Concentration"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["NO2Concentration"]) * 1000, NO2Unit)).ToString();
                                }
                                if (dt.Rows[i]["NO2YearPercent"] != DBNull.Value)
                                {
                                    dt.Rows[i]["NO2YearPercent"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["NO2YearPercent"]) * 1000, NO2Unit)).ToString();
                                }
                                break;
                            case "CO":
                                if (dt.Rows[i]["COConcentration"] != DBNull.Value)
                                {
                                    dt.Rows[i]["COConcentration"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["COConcentration"]), COUnit)).ToString();
                                }
                                if (dt.Rows[i]["COYearPercent"] != DBNull.Value)
                                {
                                    dt.Rows[i]["COYearPercent"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["COYearPercent"]), COUnit)).ToString();
                                }
                                break;
                            case "Max8HourO3":
                                if (dt.Rows[i]["Max8HourO3Concentration"] != DBNull.Value)
                                {
                                    dt.Rows[i]["Max8HourO3Concentration"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["Max8HourO3Concentration"]) * 1000, Max8HourO3Unit)).ToString();
                                }
                                if (dt.Rows[i]["Max8HourO3YearPercent"] != DBNull.Value)
                                {
                                    dt.Rows[i]["Max8HourO3YearPercent"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["Max8HourO3YearPercent"]) * 1000, Max8HourO3Unit)).ToString();
                                }
                                break;
                        }
                    }
                }
                grdAvgYearData.DataSource = dt.DefaultView;//dataView;
                DataView dv = dt.DefaultView;
                //数据分页的页数
                grdAvgYearData.VirtualItemCount = dv.Count;
            }
        }

        #endregion

        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            grdAvgYearData.Rebind();
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
        }

        /// <summary>
        /// 查询按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            grdAvgYearData.Rebind();
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
                foreach (RadComboBoxItem item in comboCityProper.CheckedItems)
                {
                    regionGuid += (item.Value.ToString() + ",");
                }
                string[] regionGuids = regionGuid.Trim(',').Split(',');
                string factor = "";
                foreach (RadComboBoxItem item in rcbfactor.CheckedItems)
                {
                    factor += (item.Value.ToString() + ",");
                }
                string[] factors = factor.Trim(',').Split(',');
                DateTime dateStart = Convert.ToDateTime(rdpBegin.SelectedDate.Value.ToString("yyyy-MM-dd") + " 00:00:00");
                DateTime dateEnd = Convert.ToDateTime(rdpEnd.SelectedDate.Value.ToString("yyyy-MM-dd") + " 23:59:59");

                DataView yearDate = m_DayAQIService.GetPrimaryPollutantAvgYearData(regionGuids, factors, dateStart, dateEnd);
                DataTable dt = yearDate.ToTable();
                int PM25Unit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a34004").PollutantDecimalNum) - 3;
                int PM10Unit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a34002").PollutantDecimalNum) - 3;
                int NO2Unit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a21004").PollutantDecimalNum) - 3;
                int SO2Unit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a21026").PollutantDecimalNum) - 3;
                int COUnit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a21005").PollutantDecimalNum);
                int Max8HourO3Unit = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a05024").PollutantDecimalNum) - 3;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    foreach (string item in factors)
                    {
                        switch (item)
                        {
                            case "PM25":
                                if (dt.Rows[i]["PM25Concentration"] != DBNull.Value)
                                {
                                    dt.Rows[i]["PM25Concentration"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["PM25Concentration"]) * 1000, PM25Unit)).ToString();
                                }
                                if (dt.Rows[i]["PM25YearPercent"] != DBNull.Value)
                                {
                                    dt.Rows[i]["PM25YearPercent"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["PM25YearPercent"]) * 1000, PM25Unit)).ToString();
                                }
                                if (dt.Rows[i]["PM25LimitValue"] != DBNull.Value)
                                {
                                    dt.Rows[i]["PM25LimitValue"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["PM25LimitValue"]) * 1000, PM25Unit)).ToString();
                                }
                                break;
                            case "PM10":
                                if (dt.Rows[i]["PM10Concentration"] != DBNull.Value)
                                {
                                    dt.Rows[i]["PM10Concentration"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["PM10Concentration"]) * 1000, PM10Unit)).ToString();
                                }
                                if (dt.Rows[i]["PM10YearPercent"] != DBNull.Value)
                                {
                                    dt.Rows[i]["PM10YearPercent"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["PM10YearPercent"]) * 1000, PM10Unit)).ToString();
                                }
                                if (dt.Rows[i]["PM10LimitValue"] != DBNull.Value)
                                {
                                    dt.Rows[i]["PM10LimitValue"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["PM10LimitValue"]) * 1000, PM10Unit)).ToString();
                                }
                                break;
                            case "SO2":
                                if (dt.Rows[i]["SO2Concentration"] != DBNull.Value)
                                {
                                    dt.Rows[i]["SO2Concentration"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["SO2Concentration"]) * 1000, SO2Unit)).ToString(); ;
                                }
                                if (dt.Rows[i]["SO2YearPercent"] != DBNull.Value)
                                {
                                    dt.Rows[i]["SO2YearPercent"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["SO2YearPercent"]) * 1000, SO2Unit)).ToString(); ;
                                }
                                if (dt.Rows[i]["SO2LimitValue"] != DBNull.Value)
                                {
                                    dt.Rows[i]["SO2LimitValue"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["SO2LimitValue"]) * 1000, SO2Unit)).ToString();
                                }
                                break;
                            case "NO2":
                                if (dt.Rows[i]["NO2Concentration"] != DBNull.Value)
                                {
                                    dt.Rows[i]["NO2Concentration"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["NO2Concentration"]) * 1000, NO2Unit)).ToString();
                                }
                                if (dt.Rows[i]["NO2YearPercent"] != DBNull.Value)
                                {
                                    dt.Rows[i]["NO2YearPercent"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["NO2YearPercent"]) * 1000, NO2Unit)).ToString();
                                }
                                if (dt.Rows[i]["NO2LimitValue"] != DBNull.Value)
                                {
                                    dt.Rows[i]["NO2LimitValue"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["NO2LimitValue"]) * 1000, NO2Unit)).ToString();
                                }
                                break;
                            case "CO":
                                if (dt.Rows[i]["COConcentration"] != DBNull.Value)
                                {
                                    dt.Rows[i]["COConcentration"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["COConcentration"]), COUnit)).ToString();
                                }
                                if (dt.Rows[i]["COYearPercent"] != DBNull.Value)
                                {
                                    dt.Rows[i]["COYearPercent"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["COYearPercent"]), COUnit)).ToString();
                                }
                                if (dt.Rows[i]["COLimitValue"] != DBNull.Value)
                                {
                                    dt.Rows[i]["COLimitValue"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["COLimitValue"]), COUnit)).ToString();
                                }
                                break;
                            case "Max8HourO3":
                                if (dt.Rows[i]["Max8HourO3Concentration"] != DBNull.Value)
                                {
                                    dt.Rows[i]["Max8HourO3Concentration"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["Max8HourO3Concentration"]) * 1000, Max8HourO3Unit)).ToString();
                                }
                                if (dt.Rows[i]["Max8HourO3YearPercent"] != DBNull.Value)
                                {
                                    dt.Rows[i]["Max8HourO3YearPercent"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["Max8HourO3YearPercent"]) * 1000, Max8HourO3Unit)).ToString();
                                }
                                if (dt.Rows[i]["Max8HourO3LimitValue"] != DBNull.Value)
                                {
                                    dt.Rows[i]["Max8HourO3LimitValue"] = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["Max8HourO3LimitValue"]) * 1000, Max8HourO3Unit)).ToString();
                                }
                                break;
                        }
                    }
                }

                DataTableToExcel(dt, "环境空气质量指数统计", "环境空气质量指数统计");
            }
        }
        /// <summary>
        /// 导出全市年数据统计
        /// </summary>
        /// <param name="dv">全市年数据统计表</param>
        /// <returns></returns>
        private void DataTableToExcel(DataTable dt, string fileName, string sheetName)
        {
            string factor = "";
            foreach (RadComboBoxItem item in rcbfactor.CheckedItems)
            {
                factor += (item.Value.ToString() + ",");
            }
            string[] factors = factor.Trim(',').Split(',');
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
            cells[0, 0].PutValue("区域");
            cells.Merge(0, 0, 2, 1);
            int Count = 1;
            foreach (string item in factors)
            {
                switch (item)
                {
                    case "SO2":
                        cells[0, Count].PutValue("SO2");
                        cells.Merge(0, Count, 1, 2);
                        cells[1, Count].PutValue("浓度（μg/m³）");
                        cells.Merge(1, Count, 1, 1);
                        cells[1, Count + 1].PutValue("百分位数");
                        cells.Merge(1, Count + 1, 1, 1);
                        cells[1, Count + 2].PutValue("单项指数");
                        cells.Merge(1, Count + 2, 1, 1);
                        break;
                    case "NO2":
                        cells[0, Count].PutValue("NO2");
                        cells.Merge(0, Count, 1, 2);
                        cells[1, Count].PutValue("浓度（μg/m³）");
                        cells.Merge(1, Count, 1, 1);
                        cells[1, Count + 1].PutValue("百分位数");
                        cells.Merge(1, Count + 1, 1, 1);
                        cells[1, Count + 2].PutValue("单项指数");
                        cells.Merge(1, Count + 2, 1, 1);
                        break;
                    case "PM10":
                        cells[0, Count].PutValue("PM10");
                        cells.Merge(0, Count, 1, 2);
                        cells[1, Count].PutValue("浓度（μg/m³）");
                        cells.Merge(1, Count, 1, 1);
                        cells[1, Count + 1].PutValue("百分位数");
                        cells.Merge(1, Count + 1, 1, 1);
                        cells[1, Count + 2].PutValue("单项指数");
                        cells.Merge(1, Count + 2, 1, 1);
                        break;
                    case "PM25":
                        cells[0, Count].PutValue("PM2.5");
                        cells.Merge(0, Count, 1, 2);
                        cells[1, Count].PutValue("浓度（μg/m³）");
                        cells.Merge(1, Count, 1, 1);
                        cells[1, Count + 1].PutValue("百分位数");
                        cells.Merge(1, Count + 1, 1, 1);
                        cells[1, Count + 2].PutValue("单项指数");
                        cells.Merge(1, Count + 2, 1, 1);
                        break;
                    case "CO":
                        cells[0, Count].PutValue("CO");
                        cells.Merge(0, Count, 1, 2);
                        cells[1, Count].PutValue("浓度（mg/m³）");
                        cells.Merge(1, Count, 1, 1);
                        cells[1, Count + 1].PutValue("百分位数");
                        cells.Merge(1, Count + 1, 1, 1);
                        cells[1, Count + 2].PutValue("单项指数");
                        cells.Merge(1, Count + 2, 1, 1);
                        break;
                    case "Max8HourO3":
                        cells[0, Count].PutValue("O3-8小时");
                        cells.Merge(0, Count, 1, 2);
                        cells[1, Count].PutValue("浓度（μg/m³）");
                        cells.Merge(1, Count, 1, 1);
                        cells[1, Count + 1].PutValue("百分位数");
                        cells.Merge(1, Count + 1, 1, 1);
                        cells[1, Count + 2].PutValue("单项指数");
                        cells.Merge(1, Count + 2, 1, 1);
                        break;
                }
                Count += 3;
            }
            cells[0, Count].PutValue("综合污染指数");
            cells.Merge(0, Count, 1, 1);
            cells.SetRowHeight(0, 20);//设置列宽
            cells.SetRowHeight(1, 40);//设置列宽

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow drNew = dt.Rows[i];
                int rowIndex = i + 2;
                cells[rowIndex, 0].PutValue(drNew["RegionName"].ToString());
                int n = 1;
                foreach (string item in factors)
                {
                    cells[rowIndex, n].PutValue(drNew[item + "Concentration"].ToString());
                    cells[rowIndex, ++n].PutValue(drNew[item + "YearPercent"].ToString());
                    cells[rowIndex, ++n].PutValue(drNew[item + "SI"].ToString());
                    n++;
                }
                cells[rowIndex, n].PutValue(drNew["CPI"].ToString());
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



        #endregion

        protected void grdAvgYearData_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            try
            {
                GridBoundColumn col = e.Column as GridBoundColumn;
                if (col == null)
                    return;
                if (col.DataField == "RegionName")
                {
                    col.HeaderText = "区域";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(70);
                    col.ItemStyle.Width = Unit.Pixel(70);
                    col.AllowSorting = false;
                }
                else if (col.DataField == "SO2Concentration")
                {
                    col.Visible = true;
                    col.HeaderText = "浓度(μg/m³)";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ColumnGroupName = "SO2";
                    //col.HeaderStyle.Width = Unit.Pixel(110);
                    //col.ItemStyle.Width = Unit.Pixel(110);
                    col.AllowSorting = false;
                }
                else if (col.DataField == "SO2YearPercent")
                {
                    col.Visible = true;
                    col.HeaderText = "百分位数";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ColumnGroupName = "SO2";
                    col.AllowSorting = false;
                }
                else if (col.DataField == "SO2SI")
                {
                    col.Visible = true;
                    col.HeaderText = "单项指数";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ColumnGroupName = "SO2";
                    col.AllowSorting = false;
                }
                else if (col.DataField == "NO2Concentration")
                {
                    col.Visible = true;
                    col.HeaderText = "浓度(μg/m³)";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ColumnGroupName = "NO2";
                    col.AllowSorting = false;
                }
                else if (col.DataField == "NO2YearPercent")
                {
                    col.Visible = true;
                    col.HeaderText = "百分位数";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ColumnGroupName = "NO2";
                    col.AllowSorting = false;
                }
                else if (col.DataField == "NO2SI")
                {
                    col.Visible = true;
                    col.HeaderText = "单项指数";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ColumnGroupName = "NO2";
                    col.AllowSorting = false;
                }
                else if (col.DataField == "PM10Concentration")
                {
                    col.Visible = true;
                    col.HeaderText = "浓度(μg/m³)";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ColumnGroupName = "PM10";
                    col.AllowSorting = false;
                }
                else if (col.DataField == "PM10YearPercent")
                {
                    col.Visible = true;
                    col.HeaderText = "百分位数";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ColumnGroupName = "PM10";
                    col.AllowSorting = false;
                }
                else if (col.DataField == "PM10SI")
                {
                    col.Visible = true;
                    col.HeaderText = "单项指数";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ColumnGroupName = "PM10";
                    col.AllowSorting = false;
                }
                else if (col.DataField == "PM25Concentration")
                {
                    col.Visible = true;
                    col.HeaderText = "浓度(μg/m³)";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ColumnGroupName = "PM25";
                    col.AllowSorting = false;
                }
                else if (col.DataField == "PM25YearPercent")
                {
                    col.Visible = true;
                    col.HeaderText = "百分位数";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ColumnGroupName = "PM25";
                    col.AllowSorting = false;
                }
                else if (col.DataField == "PM25SI")
                {
                    col.Visible = true;
                    col.HeaderText = "单项指数";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ColumnGroupName = "PM25";
                    col.AllowSorting = false;
                }
                else if (col.DataField == "COConcentration")
                {
                    col.Visible = true;
                    col.HeaderText = "浓度(mg/m³)";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ColumnGroupName = "CO";
                    col.AllowSorting = false;
                }
                else if (col.DataField == "COYearPercent")
                {
                    col.Visible = true;
                    col.HeaderText = "百分位数";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ColumnGroupName = "CO";
                    col.AllowSorting = false;
                }
                else if (col.DataField == "COSI")
                {
                    col.Visible = true;
                    col.HeaderText = "单项指数";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ColumnGroupName = "CO";
                    col.AllowSorting = false;
                }
                else if (col.DataField == "Max8HourO3Concentration")
                {
                    col.Visible = true;
                    col.HeaderText = "浓度(μg/m³)";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ColumnGroupName = "Max8HourO3";
                    col.AllowSorting = false;
                }
                else if (col.DataField == "Max8HourO3YearPercent")
                {
                    col.Visible = true;
                    col.HeaderText = "百分位数";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ColumnGroupName = "Max8HourO3";
                    col.AllowSorting = false;
                }
                else if (col.DataField == "Max8HourO3SI")
                {
                    col.Visible = true;
                    col.HeaderText = "单项指数";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ColumnGroupName = "Max8HourO3";
                    col.AllowSorting = false;
                }
                else if (col.DataField == "CPI")
                {
                    col.Visible = true;
                    col.HeaderText = "综合污染指数";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
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
    }
}