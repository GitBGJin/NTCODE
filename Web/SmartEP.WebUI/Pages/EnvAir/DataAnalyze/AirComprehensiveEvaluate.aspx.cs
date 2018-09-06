using Aspose.Cells;
using SmartEP.Core.Generic;
using SmartEP.Service.DataAnalyze.Air.AQIReport;
using SmartEP.Utilities.Office;
using SmartEP.WebUI.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.DataAnalyze
{
    public partial class AirComprehensiveEvaluate : BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        DayAQIService m_DayAQIService = Singleton<DayAQIService>.GetInstance();

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
            dtpBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-01-01"));
            dtpEnd.SelectedDate = DateTime.Now.AddDays(-1);
        }
        #endregion

        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {
            string regionGuid = "";
            foreach (RadComboBoxItem item in comboCity.CheckedItems)
            {
                regionGuid += (item.Value.ToString() + ",");
            }
            string[] regionGuids = regionGuid.Trim(',').Split(',');
            string year = Year.SelectedValue;
            DateTime dtBegion = dtpBegin.SelectedDate.Value;
            DateTime dtEnd = dtpEnd.SelectedDate.Value;
            var analyzeDate = new DataView();
            if (regionGuids != null)
            {
                analyzeDate = m_DayAQIService.GetComprehensiveData(regionGuids, dtBegion, dtEnd, year);
            }
            if (analyzeDate != null)
            {
                grdComprehensive.DataSource = analyzeDate;//dataView;
                grdComprehensive.VirtualItemCount = analyzeDate.Count;
            }
            else
            {
                grdComprehensive.DataSource = new DataTable();
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
            grdComprehensive.Rebind();
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
                string year = Year.SelectedValue;
                string years = dtpEnd.SelectedDate.Value.AddYears(-1).Year.ToString();
                GridTableCell cell = (GridTableCell)item["PM2.5浓度与" + year + "年同期比较(%)"];
                string PM25Status = drv["PM25_Status"] != DBNull.Value ? drv["PM25_Status"].ToString() : string.Empty;

                GridTableCell Dcell = (GridTableCell)item["达标率与" + years + "年同期比较(百分点)"];
                string DabiaoStatus = drv["Dabiao_Status"] != DBNull.Value ? drv["Dabiao_Status"].ToString() : string.Empty;

                GridTableCell DsDcell = (GridTableCell)item["重污染天数" + years + "年同期比较(天)"];
                string DaysStatus = drv["Days_Status"] != DBNull.Value ? drv["Days_Status"].ToString() : string.Empty;

                GridTableCell CDcell = (GridTableCell)item["综合考评"];
                string CompreStatus = drv["Compre_Status"] != DBNull.Value ? drv["Compre_Status"].ToString() : string.Empty;

                if (PM25Status == "N")
                {
                    cell.Text = cell.Text;
                    cell.ForeColor = System.Drawing.Color.Red;
                    cell.Font.Bold = true;
                }
                if (DabiaoStatus == "N")
                {
                    Dcell.Text = Dcell.Text;
                    Dcell.ForeColor = System.Drawing.Color.Red;
                    Dcell.Font.Bold = true;
                }
                if (DaysStatus == "N")
                {
                    DsDcell.Text = DsDcell.Text;
                    DsDcell.ForeColor = System.Drawing.Color.Red;
                    DsDcell.Font.Bold = true;
                }
                if (CompreStatus == "N")
                {
                    CDcell.Text = CDcell.Text;
                    CDcell.ForeColor = System.Drawing.Color.Red;
                    CDcell.Font.Bold = true;
                }
            }
        }

        /// <summary>
        /// 查询按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            grdComprehensive.Rebind();
        }
        protected void grdComprehensive_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
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
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "PM25_Status")
                {
                    col.HeaderText = string.Empty;
                    col.Visible = false;
                }
                else if (col.DataField == "Dabiao_Status")
                {
                    col.HeaderText = string.Empty;
                    col.Visible = false;
                }
                else if (col.DataField == "Days_Status")
                {
                    col.HeaderText = string.Empty;
                    col.Visible = false;
                }
                else if (col.DataField == "Compre_Status")
                {
                    col.HeaderText = string.Empty;
                    col.Visible = false;
                }
                else
                {
                    if (col.DataField.Contains("PM2.5"))
                    {
                        col.HeaderText = col.DataField.Replace("PM2.5", "PM<sub>2.5</sub>");
                    }
                    if (col.DataField.Contains("O3"))
                    {
                        col.HeaderText = col.DataField.Replace("O3", "O<sub>3</sub>");
                    }
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    if (col.DataField == "综合考评")
                    {
                        col.HeaderStyle.Width = Unit.Pixel(90);
                        col.ItemStyle.Width = Unit.Pixel(90);
                    }
                    else
                    {
                        col.HeaderStyle.Width = Unit.Pixel(180);
                        col.ItemStyle.Width = Unit.Pixel(180);
                    }
                }
            }
            catch (Exception ex) { }
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
                foreach (RadComboBoxItem item in comboCity.CheckedItems)
                {
                    regionGuid += (item.Value.ToString() + ",");
                }
                string[] regionGuids = regionGuid.Trim(',').Split(',');
                DateTime dtBegion = dtpBegin.SelectedDate.Value;
                dtBegion = new DateTime(dtBegion.Year, dtBegion.Month, 1);
                DateTime dtEnd = dtpEnd.SelectedDate.Value;
                DateTime dtEndA = new DateTime(dtEnd.Year, dtEnd.Month, 1);
                string year = Year.SelectedValue;
                var analyzeDate = m_DayAQIService.GetComprehensiveData(regionGuids, dtBegion, dtEnd, year);
                DataTable dtNew = analyzeDate.ToTable();
                DataTableToExcel(dtNew.DefaultView, "各市、区域考评比较情况统计表（周报）", "各市、区域考评比较情况统计表（周报）");
            }
        }

        #endregion

        /// <summary>
        /// 导出空气质量实时报
        /// </summary>
        /// <param name="dv">原始数据表</param>
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
            cellStyle.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            cellStyle.Borders[BorderType.TopBorder].Color = Color.Black;
            cellStyle.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
            cellStyle.Borders[BorderType.BottomBorder].Color = Color.Black;
            cellStyle.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            cellStyle.Borders[BorderType.LeftBorder].Color = Color.Black;
            cellStyle.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            cellStyle.Borders[BorderType.RightBorder].Color = Color.Black;

            Aspose.Cells.Style cellStyle1 = workbook.Styles[workbook.Styles.Add()];
            cellStyle1.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center; // 设置居中 
            cellStyle1.Font.Name = "宋体"; //文字字体
            cellStyle1.Font.Size = 12;//文字大小
            cellStyle1.Font.Color = Color.Red;
            cellStyle1.IsLocked = false; //单元格解锁
            cellStyle1.IsTextWrapped = true; //单元格内容自动换行
            cellStyle1.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            cellStyle1.Borders[BorderType.TopBorder].Color = Color.Black;
            cellStyle1.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
            cellStyle1.Borders[BorderType.BottomBorder].Color = Color.Black;
            cellStyle1.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            cellStyle1.Borders[BorderType.LeftBorder].Color = Color.Black;
            cellStyle1.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            cellStyle1.Borders[BorderType.RightBorder].Color = Color.Black;

            Aspose.Cells.Style titleStyle = workbook.Styles[workbook.Styles.Add()];
            titleStyle.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center; // 设置居中 
            titleStyle.Font.Name = "宋体"; //文字字体
            titleStyle.Font.Size = 20;//文字大小
            titleStyle.Font.IsBold = false;
            titleStyle.IsLocked = false; //单元格解锁
            titleStyle.IsTextWrapped = true; //单元格内容自动换行
            titleStyle.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            titleStyle.Borders[BorderType.TopBorder].Color = Color.White;
            titleStyle.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
            titleStyle.Borders[BorderType.BottomBorder].Color = Color.White;
            titleStyle.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            titleStyle.Borders[BorderType.LeftBorder].Color = Color.White;
            titleStyle.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            titleStyle.Borders[BorderType.RightBorder].Color = Color.White;

            Aspose.Cells.Style titleStyle1 = workbook.Styles[workbook.Styles.Add()];
            titleStyle1.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center; // 设置居中 
            titleStyle1.Font.Name = "宋体"; //文字字体
            titleStyle1.Font.Size = 14;//文字大小
            titleStyle1.IsLocked = false; //单元格解锁
            titleStyle1.IsTextWrapped = true; //单元格内容自动换行

            string year = Year.SelectedValue;
            string years = dtpEnd.SelectedDate.Value.AddYears(-1).Year.ToString();

            string titleText = string.Empty;
            System.DateTime endDate = Convert.ToDateTime(dtpEnd.SelectedDate.Value); ;
            if (endDate > System.DateTime.Now)
                endDate = System.DateTime.Now;
            string timeRange = string.Format("{0:0101}~{1:MMdd}", endDate, endDate);
            titleText = endDate.Year + "年第" + WeekOfYear(endDate) + "期(" + timeRange + ")";

            #region 标题
            cells[0, 0].PutValue("各市、区域考评比较情况统计表（周报）");
            cells.Merge(0, 0, 1, 7);
            cells[1, 0].PutValue(titleText);
            cells.Merge(1, 0, 1, 7);
            for (int i = 0; i < 7; i++)
            {
                cells[0, i].SetStyle(titleStyle);
                cells[1, i].SetStyle(titleStyle1);
            }
            cells.SetRowHeight(0, 60);//设置行高
            cells.SetRowHeight(1, 30);//设置行高
            #endregion

            #region 表头
            //第一行
            cells[2, 0].PutValue("区域");
            cells.Merge(2, 0, 1, 1);
            cells[2, 1].PutValue(dtpEnd.SelectedDate.Value.Year + "年PM2.5浓度(微克/立方米)");
            cells.Merge(2, 1, 1, 1);
            cells[2, 2].PutValue("PM2.5浓度与" + year + "年同期比较(%)");
            cells.Merge(2, 2, 1, 1);
            cells[2, 3].PutValue("达标率与" + years + "年同期比较(百分点)");
            cells.Merge(2, 3, 1, 1);
            cells[2, 4].PutValue("重污染天数" + years + "年同期比较(天)");
            cells.Merge(2, 4, 1, 1);
            cells[2, 5].PutValue("综合考评");
            cells.Merge(2, 5, 1, 1);
            cells[2, 6].PutValue("O3百分位(微克/立方米)");
            cells.Merge(2, 6, 1, 1);

            cells.SetRowHeight(2, 50);//设置行高
            for (int i = 0; i <= 6; i++)
            {
                cells.SetColumnWidth(i, 20);//设置列宽
                cells[2, i].SetStyle(cellStyle);
            }
            #endregion
            for (int i = 0; i < dtNew.Rows.Count; i++)
            {
                DataRow drNew = dtNew.Rows[i];
                int rowIndex = i + 3;
                cells.SetRowHeight(rowIndex, 25);//设置行高
                cells[rowIndex, 0].PutValue(drNew["RegionName"].ToString());
                cells[rowIndex, 0].SetStyle(cellStyle);
                cells[rowIndex, 1].PutValue(drNew[dtpEnd.SelectedDate.Value.Year + "年PM2.5浓度(微克/立方米)"].ToString());
                cells[rowIndex, 1].SetStyle(cellStyle);

                cells[rowIndex, 2].PutValue(drNew["PM2.5浓度与" + year + "年同期比较(%)"].ToString());
                cells[rowIndex, 2].SetStyle(cellStyle);
                if (drNew["PM25_Status"].ToString() == "N")
                {
                    cells[rowIndex, 2].SetStyle(cellStyle1);
                }
                cells[rowIndex, 3].PutValue(drNew["达标率与" + years + "年同期比较(百分点)"].ToString());
                cells[rowIndex, 3].SetStyle(cellStyle);
                if (drNew["Dabiao_Status"].ToString() == "N")
                {
                    cells[rowIndex, 3].SetStyle(cellStyle1);
                }
                cells[rowIndex, 4].PutValue(drNew["重污染天数" + years + "年同期比较(天)"].ToString());
                cells[rowIndex, 4].SetStyle(cellStyle);
                if (drNew["Days_Status"].ToString() == "N")
                {
                    cells[rowIndex, 4].SetStyle(cellStyle1);
                }
                cells[rowIndex, 5].PutValue(drNew["综合考评"].ToString());
                cells[rowIndex, 5].SetStyle(cellStyle);
                if (drNew["Compre_Status"].ToString() == "N")
                {
                    cells[rowIndex, 5].SetStyle(cellStyle1);
                }
                cells[rowIndex, 6].PutValue(drNew["O3百分位(微克/立方米)"].ToString());
                cells[rowIndex, 6].SetStyle(cellStyle);
            }
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", string.Format("attachment;filename={0}.xls", HttpUtility.UrlEncode(fileName) + DateTime.Now.ToString("yyyyMMddHHmmss")));
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.ContentType = "application/ms-excel";
            Response.BinaryWrite(workbook.SaveToStream().ToArray());
            Response.End();
        }
        /// <summary>
        /// 返回该年第几周
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public static int WeekOfYear(DateTime day)
        {
            int weeknum;
            DateTime fDt = DateTime.Parse(day.Year.ToString() + "-01-01");
            int k = Convert.ToInt32(fDt.DayOfWeek);//得到该年的第一天是周几 
            if (k == 0)
            {
                k = 7;
            }
            int l = Convert.ToInt32(day.DayOfYear);//得到当天是该年的第几天 
            l = l - (7 - k + 1);
            //l = l - (7 - k);
            if (l <= 0)
            {
                weeknum = 1;
            }
            else
            {
                if (l % 7 == 0)
                {
                    weeknum = l / 7 + 1;
                }
                else
                {
                    weeknum = l / 7 + 2;//不能整除的时候要加上前面的一周和后面的一周 
                }
            }
            return weeknum - 1;
        }
    }
}