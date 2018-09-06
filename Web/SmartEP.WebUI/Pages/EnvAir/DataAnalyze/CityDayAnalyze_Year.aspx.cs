using Aspose.Cells;
using SmartEP.Core.Generic;
using SmartEP.DomainModel;
using SmartEP.Service.BaseData.BusinessRule;
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
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.DataAnalyze
{
    /// <summary>
    /// 名称：CityDayAnalyze_Year.aspx.cs
    /// 创建人：樊垂贺
    /// 创建日期：2015-08-19
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：全市年数据统计
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class CityDayAnalyze_Year : BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        DayAQIService m_DayAQIService = Singleton<DayAQIService>.GetInstance();
        DictionaryService dicService = new DictionaryService();

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
            for (int i = 0; i < 20; i++)
            {
                ddlYear.Items.Add(new DropDownListItem((DateTime.Now.Year - i).ToString(), (DateTime.Now.Year - i).ToString()));
                if (DateTime.Now.Year - i <= 2009)
                {
                    break;
                }
            }
            //comboCityProper.DataSource = dicService.RetrieveCityList();
            //comboCityProper.DataTextField = "ItemText";
            //comboCityProper.DataValueField = "ItemGuid";
            //comboCityProper.DataBind();
            //for (int i = 0; i < comboCityProper.Items.Count; i++)
            //{
            //    comboCityProper.Items[i].Checked = true;
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
            foreach (RadComboBoxItem item in comboCityProper.CheckedItems)
            {
                regionGuid += (item.Value.ToString() + ",");
            };
            string[] regionGuids = regionGuid.Trim(',').Split(',');
            DateTime dateStart = Convert.ToDateTime(ddlYear.SelectedText.ToString() + "-1-1 00:00:00");
            DateTime dateEnd = Convert.ToDateTime(ddlYear.SelectedText.ToString() + "-12-31 23:59:59");

            DataView yearDate = m_DayAQIService.GetAllYearData(regionGuids, dateStart, dateEnd);
            if (yearDate != null)
            {
                //foreach (DataRowView drv in yearDate)
                //{
                //    string a = drv["OutBiggestFactor"].ToString().Trim();
                //    if (Convert.ToDecimal(a) < 0)
                //    {
                //        drv["OutBiggestFactor"] = "/";
                //    }
                //}
                grdCityYear.DataSource = yearDate;//dataView;
                grdCityYear.VirtualItemCount = yearDate.Count;
            }
            else
            {
                grdCityYear.DataSource = new DataTable();
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
            grdCityYear.Rebind();
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
            grdCityYear.Rebind();
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
                };
                string[] regionGuids = regionGuid.Trim(',').Split(',');
                DateTime dateStart = Convert.ToDateTime(ddlYear.SelectedText.ToString() + "-1-1 00:00:00");
                DateTime dateEnd = Convert.ToDateTime(ddlYear.SelectedText.ToString() + "-12-31 23:59:59");

                DataView yearDate = m_DayAQIService.GetAllYearData(regionGuids, dateStart, dateEnd);
                DataTableToExcel(yearDate, ddlYear.SelectedText.ToString() + "年数据统计", ddlYear.SelectedText.ToString() + "年数据统计");
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

            //第一行
            cells[0, 0].PutValue("地区");
            cells.Merge(0, 0, 2, 1);
            cells[0, 1].PutValue("监测指标");
            cells.Merge(0, 1, 2, 1);
            cells[0, 2].PutValue("日均值");
            cells.Merge(0, 2, 1, 6);
            cells[0, 8].PutValue("年均值");
            cells.Merge(0, 8, 1, 4);

            //第二行
            cells[1, 2].PutValue("最小值");
            cells.Merge(1, 2, 1, 1);
            cells[1, 3].PutValue("最大值");
            cells.Merge(1, 3, 1, 1);
            cells[1, 4].PutValue("超标天数");
            cells.Merge(1, 4, 1, 1);
            cells[1, 5].PutValue("监测天数");
            cells.Merge(1, 5, 1, 1);
            cells[1, 6].PutValue("超标率");
            cells.Merge(1, 6, 1, 1);
            cells[1, 7].PutValue("最大超标倍数");
            cells.Merge(1, 7, 1, 1);
            cells[1, 8].PutValue("年均值浓度");
            cells.Merge(1, 8, 1, 1);
            cells[1, 9].PutValue("年均值超标倍数");
            cells.Merge(1, 9, 1, 1);
            cells[1, 10].PutValue("百分位数浓度");
            cells.Merge(1, 10, 1, 1);
            cells[1, 11].PutValue("百分位数超标倍数");
            cells.Merge(1, 11, 1, 1);
            for (int i = 0; i < dtNew.Rows.Count; i++)
            {
                DataRow drNew = dtNew.Rows[i];
                int rowIndex = i + 2;
                cells[rowIndex, 0].PutValue(drNew["RegionName"].ToString());
                cells[rowIndex, 1].PutValue(drNew["FactorName"].ToString());
                cells[rowIndex, 2].PutValue(drNew["DayMinValue"].ToString());
                cells[rowIndex, 3].PutValue(drNew["DayMaxValue"].ToString());
                cells[rowIndex, 4].PutValue(drNew["OutDays"].ToString());
                cells[rowIndex, 5].PutValue(drNew["MonitorDays"].ToString());
                cells[rowIndex, 6].PutValue(drNew["OutRate"].ToString());
                cells[rowIndex, 7].PutValue(drNew["OutBiggestFactor"].ToString());
                cells[rowIndex, 8].PutValue(drNew["YearAverage"].ToString());
                cells[rowIndex, 9].PutValue(drNew["YearOutRate"].ToString());
                cells[rowIndex, 10].PutValue(drNew["YearPercent"].ToString());
                cells[rowIndex, 11].PutValue(drNew["YearPerOutRate"].ToString());
            }
            cells.SetColumnWidth(1, 15);//设置列宽
            cells.SetColumnWidth(4, 10);//设置列宽
            cells.SetColumnWidth(5, 10);//设置列宽
            cells.SetColumnWidth(7, 15);//设置列宽
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
    }
}