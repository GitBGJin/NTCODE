﻿using Aspose.Cells;
using SmartEP.Core.Generic;
using SmartEP.DomainModel;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.BusinessRule;
using SmartEP.Service.BaseData.Channel;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.Core.Enums;
using SmartEP.Service.DataAnalyze.Air.AQIReport;
using SmartEP.Service.Frame;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Utilities.Office;
using SmartEP.WebControl.CbxRsm;
using SmartEP.WebUI.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.DataAnalyze
{
    public partial class DataAvgDayAnalyze : BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        DayAQIService m_DayAQIService = Singleton<DayAQIService>.GetInstance();
        DictionaryService dicService = new DictionaryService();
        /// <summary>
        /// 类型数据
        /// </summary>
        Dictionary<string, string> dicList = null;
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

            string names = ConfigurationManager.AppSettings["NTRegionPointName"].ToString();    //从配置文件获取默认站点
            pointCbxRsm.SetPointValuesFromNames(names);

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
            //if (comboCityModel.Items.Count > 0)
            //{
            //    comboCityModel.Items[0].Checked = true;
            //}

            dicList = new Dictionary<string, string>();
            dicList.Add("DayMinValue", "最小值");
            dicList.Add("DayMaxValue", "最大值");
            dicList.Add("DayAvgValue", "平均值");
            dicList.Add("OutDays", "超标天数");
            dicList.Add("MonitorDays", "有效天数");
            dicList.Add("OutRate", "超标率");
            dicList.Add("OutBiggestFactor", "最大超标倍数");
            dicList.Add("OutDate", "最大超标日期");
            dicList.Add("DataRange", "浓度范围");
            dicList.Add("YearPercent", "百分位数浓度");
            dicList.Add("YearPerOutRate", "百分位数超标倍数");
            cbType.Items.Clear();
            foreach (KeyValuePair<string, string> kv in dicList)
            {
                RadComboBoxItem cmbItemcbType = new RadComboBoxItem();
                cmbItemcbType.Text = kv.Value;
                cmbItemcbType.Value = kv.Key;

                cmbItemcbType.Checked = true;
                cbType.Items.Add(cmbItemcbType);
            }
            cbType.DataBind();
        }
        #endregion

        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {
            string pointidorregionguid = "";
            string factorCode = "";
            string regionGuid = "";
            string[] portIds = null;
            //switch (rbtnlType.SelectedValue)
            //{
            //    case "Port":
            //        portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
            //        pointidorregionguid = string.Join(",", portIds);
            //        break;
            //    case "CityProper":
            //        foreach (RadComboBoxItem item in comboCity.CheckedItems)
            //        {
            //            regionGuid += (item.Value.ToString() + ",");
            //        }
            //        pointidorregionguid = regionGuid;
            //        break;
            //}
            //string[] regionGuids = regionGuid.Trim(',').Split(',');
            portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
            pointidorregionguid = string.Join(",", portIds);

            string factor = "";
            foreach (RadComboBoxItem item in rcbfactor.CheckedItems)
            {
                factor += (item.Value.ToString() + ",");
            }
            factorCode = factor;
            string[] factors = factor.Trim(',').Split(',');
            DateTime dateStart = Convert.ToDateTime(rdpBegin.SelectedDate.Value.ToString("yyyy-MM-dd 00:00:00"));
            DateTime dateEnd = Convert.ToDateTime(rdpEnd.SelectedDate.Value.ToString("yyyy-MM-dd 23:59:59"));
            hdPointIds.Value = pointidorregionguid;
            hdFactors.Value = factorCode.TrimEnd(',');
            hdStartdt.Value = dateStart.ToString("yyyy-MM-dd HH:mm:ss");
            hdEnddt.Value = dateEnd.ToString("yyyy-MM-dd HH:mm:ss");
            hdPointType.Value = rbtnlType.SelectedValue;
            hdChartType.Value = ChartContent.SelectedValue;
            hdFlag.Value = ChartContent.SelectedValue == "K_Value" ? "0" : (ChartContent.SelectedValue == "OutRate" ? "1" : "2");
            
            //Decimal x= DecimalExtension.GetPollutantValue(,0);

            if (tabStrip.SelectedTab.Text == "图表")
            {
                //iframeOCM.Attributes.Add("src", "DataAvgDayCharts.aspx?pointIds=" + pointidorregionguid + "&factors=" + factorCode + "&Type=" + rbtnlType.SelectedValue
                //    + "&dtStart=" + dateStart.ToString("yyyy-MM-dd HH:mm:ss") + "&dtEnd=" + dateEnd.ToString("yyyy-MM-dd HH:mm:ss"));
                FirstLoadChart.Value = "1";
            }
            //生成RadGrid的绑定列
            //每页显示数据个数            
            int pageSize = grdAvgDay.PageSize;
            //当前页的序号
            int pageNo = grdAvgDay.CurrentPageIndex;

            var AvgDayData = new DataView();
            DataTable dt = new DataTable();
            //点位
            if (rbtnlType.SelectedValue == "Port")
            {
                if (portIds != null)
                {
                    AvgDayData = m_DayAQIService.GetAvgDayData(portIds, factors, dateStart, dateEnd);

                }
            }
            //南通市区、市区均值
            else if (rbtnlType.SelectedValue == "CityProper")
            {
                if (portIds != null)
                {
                    AvgDayData = m_DayAQIService.GetAllYearDataNew(portIds, factors, dateStart, dateEnd);
                }

            }

            if (AvgDayData != null)
            {
                grdAvgDay.DataSource = AvgDayData;//dataView;
                grdAvgDay.VirtualItemCount = AvgDayData.Count;
            }
            else
            {
                grdAvgDay.DataSource = new DataTable();
            }
        }

        #endregion

        /// <summary>   
        /// Datatable转换为Json   
        /// </summary>   
        /// <param name="table">Datatable对象</param>   
        /// <returns>Json字符串</returns>   
        public static string ToJson(DataTable dt)
        {
            StringBuilder jsonString = new StringBuilder();
            jsonString.Append("[");
            DataRowCollection drc = dt.Rows;
            for (int i = 0; i < drc.Count; i++)
            {
                jsonString.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string strKey = dt.Columns[j].ColumnName;
                    string strValue = drc[i][j].ToString();
                    Type type = dt.Columns[j].DataType;
                    jsonString.Append("\"" + strKey + "\":");
                    strValue = StringFormat(strValue, type);
                    if (j < dt.Columns.Count - 1)
                    {
                        jsonString.Append(strValue + ",");
                    }
                    else
                    {
                        jsonString.Append(strValue);
                    }
                }
                jsonString.Append("},");
            }
            if (jsonString.ToString() != "[") jsonString.Remove(jsonString.Length - 1, 1);
            jsonString.Append("]");
            return jsonString.ToString();
        }
        /// <summary>  
        /// 格式化字符型、日期型、布尔型  
        /// </summary>  
        /// <param name="str"></param>  
        /// <param name="type"></param>  
        /// <returns></returns>  
        private static string StringFormat(string str, Type type)
        {
            if (type == typeof(string))
            {
                str = String2Json(str);
                str = "\"" + str + "\"";
            }
            else if (type == typeof(DateTime))
            {
                str = "\"" + str + "\"";
            }
            else if (type == typeof(bool))
            {
                str = str.ToLower();
            }
            else if (string.IsNullOrEmpty(str))
            {
                str = "null";
            }
            return str;
        }
        /// <summary>  
        /// 过滤特殊字符  
        /// </summary>  
        /// <param name="s"></param>  
        /// <returns></returns>  
        private static string String2Json(String s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s.ToCharArray()[i];
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\""); break;
                    case '\\':
                        sb.Append("\\\\"); break;
                    case '/':
                        sb.Append("\\/"); break;
                    case '\b':
                        sb.Append("\\b"); break;
                    case '\f':
                        sb.Append("\\f"); break;
                    case '\n':
                        sb.Append("\\n"); break;
                    case '\r':
                        sb.Append("\\r"); break;
                    case '\t':
                        sb.Append("\\t"); break;
                    default:
                        sb.Append(c); break;
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            grdAvgDay.CurrentPageIndex = 0;
            grdAvgDay.Rebind();
            if (tabStrip.SelectedTab.Text == "图表")
            {

                RegisterScript("SearchData();");
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
        protected void grdAvgDay_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                //AirPollutantService m_AirPollutantService = new AirPollutantService();
                if (e.Item is GridDataItem)
                {
                    GridDataItem item = e.Item as GridDataItem;
                    DataRowView drv = e.Item.DataItem as DataRowView;

                    //if (item["FactorName"] != null)
                    //{
                    //    GridTableCell factorCell = (GridTableCell)item["FactorName"];
                    //    factorCell.Text += m_AirPollutantService.GetPollutantInfo(item["FactorName"].ToString()).PollutantMeasureUnit;
                    //}
                }
            }
            catch (Exception ex)
            {
            }
        }

        ///// <summary>
        ///// 查询按纽事件
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void btnSearch_Click(object sender, EventArgs e)
        //{
        //    grdAvgDay.Rebind();
        //}

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
                string pointidorregionguid = "";
                string factorCode = "";
                string regionGuid = "";
                string[] portIds = null;
                //switch (rbtnlType.SelectedValue)
                //{
                //    case "Port":
                //        portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
                //        pointidorregionguid = string.Join(",", portIds);
                //        break;
                //    case "CityProper":
                //        foreach (RadComboBoxItem item in comboCity.CheckedItems)
                //        {
                //            regionGuid += (item.Value.ToString() + ",");
                //        }
                //        pointidorregionguid = regionGuid;
                //        break;
                //}
                //string[] regionGuids = regionGuid.Trim(',').Split(',');
                portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);

                string factor = "";
                foreach (RadComboBoxItem item in rcbfactor.CheckedItems)
                {
                    factor += (item.Value.ToString() + ",");
                }
                factorCode = factor;
                string[] factors = factor.Trim(',').Split(',');
                DataView yearDate = new DataView();
                DateTime dateStart = Convert.ToDateTime(rdpBegin.SelectedDate.Value.ToString("yyyy-MM-dd 00:00:00"));
                DateTime dateEnd = Convert.ToDateTime(rdpEnd.SelectedDate.Value.ToString("yyyy-MM-dd 23:59:59"));
                if (rbtnlType.SelectedValue == "Port")
                {
                    if (portIds != null)
                    {
                        yearDate = m_DayAQIService.GetAvgDayData(portIds, factors, dateStart, dateEnd);
                    }
                }
                //南通市区、市区均值
                else if (rbtnlType.SelectedValue == "CityProper")
                {
                    if (portIds != null)
                    {
                        yearDate = m_DayAQIService.GetAllYearDataNew(portIds, factors, dateStart, dateEnd);
                    }

                }
                DataTableToExcel(yearDate, "日均值统计", "日均值统计");
            }
        }

        /// <summary>
        /// 导出全市年数据统计
        /// </summary>
        /// <param name="dv">全市年数据统计表</param>
        /// <returns></returns>
        private void DataTableToExcel(DataView dv, string fileName, string sheetName)
        {
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
            string unit = "";
            for (int i = 0; i < dtNew.Rows.Count; i++)
            {
                switch (dtNew.Rows[i]["FactorName"].ToString())
                {
                    case "CO (mg/m<sup>3</sup>)":
                        unit = "CO (mg/m3)";
                        break;
                    case "O3-8 (μg/m<sup>3</sup>)":
                        unit = "O3-8 (μg/m3)";
                        break;
                    case "PM2.5 (μg/m<sup>3</sup>)":
                        unit = "PM2.5 (μg/m3)";
                        break;
                    case "PM10 (μg/m<sup>3</sup>)":
                        unit = "PM10 (μg/m3)";
                        break;
                    case "NO2 (μg/m<sup>3</sup>)":
                        unit = "NO2 (μg/m3)";
                        break;
                    case "SO2 (μg/m<sup>3</sup>)":
                        unit = "SO2 (μg/m3)";
                        break;
                }
                dtNew.Rows[i]["FactorName"] = unit;
            }

            //第一行
            cells[0, 0].PutValue("地区");
            cells.Merge(0, 0, 1, 1);
            cells[0, 1].PutValue("监测指标");
            cells.Merge(0, 1, 1, 1);
            int n = 2;
            foreach (RadComboBoxItem item in cbType.CheckedItems)
            {
                cells[0, n].PutValue(item.Text);
                cells.Merge(1, n, 1, 1);
                n++;
            }
            for (int i = 0; i < dtNew.Rows.Count; i++)
            {
                DataRow drNew = dtNew.Rows[i];
                int rowIndex = i + 1;
                cells.SetRowHeight(rowIndex, 20);//设置列宽
                cells[rowIndex, 0].PutValue(drNew["RegionName"].ToString());
                cells[rowIndex, 1].PutValue(drNew["FactorName"].ToString());

                int m = 2;
                foreach (RadComboBoxItem item in cbType.CheckedItems)
                {
                    cells[rowIndex, m].PutValue(drNew[item.Value].ToString());
                    m++;
                }
            }
            cells.SetColumnWidth(0, 15);//设置列宽
            //cells.SetColumnWidth(4, 10);//设置列宽
            //cells.SetColumnWidth(5, 10);//设置列宽
            //cells.SetColumnWidth(5, 10);//设置列宽
            //cells.SetColumnWidth(5, 10);//设置列宽
            for (int i = 1; i <= 7; i++)
                cells.SetColumnWidth(i, 10);//设置列宽
            for (int i = 8; i <= 12; i++)
                cells.SetColumnWidth(i, 15);//设置列宽
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

        protected void grdAvgDay_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            try
            {
                GridBoundColumn col = e.Column as GridBoundColumn;
                if (col == null)
                    return;
                if (col.DataField == "RegionName")
                {
                    col.HeaderText = "地区";
                    col.EmptyDataText = "/";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                    col.AllowSorting = false;
                }
                else if (col.DataField == "FactorName")
                {
                    col.HeaderText = "监测指标";
                    col.EmptyDataText = "/";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                    col.AllowSorting = false;
                }
                else if (cbType.CheckedItems.Count(t => (col.DataField).Contains(t.Value)) > 0)
                {
                    col.Visible = true;
                    col.HeaderText = cbType.CheckedItems.Where(x => x.Value.Contains(col.DataField)).Select(t => t.Text).FirstOrDefault();
                    
                    col.EmptyDataText = "/";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                    col.AllowSorting = false;
                    if (col.HeaderText=="")
                    {
                        col.Visible = false;
                    }
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

        protected void ChartContent_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            string value = ChartContent.SelectedValue;
            hdChartType.Value = value;
            if (ChartContent.SelectedValue == "K_Value")
            {
                hdFlag.Value = "0";
            }
            else if (ChartContent.SelectedValue == "OutRate")
            {
                hdFlag.Value = "1";
            }
            else
            {
                hdFlag.Value = "2";
            }
            RegisterScript("PointFactor();");
        }

    }
}