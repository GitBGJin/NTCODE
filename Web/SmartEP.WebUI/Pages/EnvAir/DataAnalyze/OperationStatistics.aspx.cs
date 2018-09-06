using Aspose.Cells;
using SmartEP.Core.Generic;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.DataAnalyze.Air.DataQuery;
using SmartEP.Utilities.Office;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.DataAnalyze
{
    public partial class OperationStatistics : SmartEP.WebUI.Common.BasePage
    {
         /// <summary>
        /// 数据处理服务
        /// </summary>
        DataQueryByHourService m_HourData = Singleton<DataQueryByHourService>.GetInstance();
        /// <summary>
        /// 测点类
        /// </summary>
        MonitoringPointAirService g_MonitoringPointAir = null;
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
            monthBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddYears(-1).ToString("yyyy-MM"));
            monthEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToString("yyyy-MM"));

            //国控点，对照点，路边站
            MonitoringPointAirService m_MonitoringPointAirService = Singleton<MonitoringPointAirService>.GetInstance();
            string strpointName = "";
            IQueryable<MonitoringPointEntity> EnableOrNotports = m_MonitoringPointAirService.RetrieveAirMPListByEnable();
            string[] EnableOrNotportsarry = EnableOrNotports.Where(p => p.ContrlUid == "bdf0837a-eb59-4c4a-a05f-c774a17f3077" || p.MonitoringPointUid == "5C15471C-4FBE-4BD6-BA03-F1EDE5D5E270").Select(p => p.MonitoringPointName).ToArray();
            foreach (string point in EnableOrNotportsarry)
            {
                strpointName += point + ";";
            }
            pointCbxRsm.SetPointValuesFromNames(strpointName);
            string pollutantName = System.Configuration.ConfigurationManager.AppSettings["AirPollutant"];
            factorCbxRsm.SetFactorValuesFromNames(pollutantName);
        }
        #endregion
        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {
            string[] portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
            string[] factors = factorCbxRsm.GetFactorValues(SmartEP.Core.Enums.CbxRsmReturnType.Code);
            //DateTime dtBegion = Convert.ToDateTime(monthBegin.SelectedDate.Value.ToString("yyyy-MM-dd 00:00:00"));
            //DateTime dtEnd = Convert.ToDateTime(monthEnd.SelectedDate.Value.ToString("yyyy-MM-dd 23:59:59"));
            DateTime dtBegion = monthBegin.SelectedDate.Value;
            DateTime dtEnd = monthEnd.SelectedDate.Value.AddMonths(1).AddSeconds(-1);
            //生成RadGrid的绑定列
            //每页显示数据个数            
            int pageSize = grdDataCaptureRate.PageSize;
            //当前页的序号
            int pageNo = grdDataCaptureRate.CurrentPageIndex;
            var AvgDayData = new DataView();

            //点位
            if (portIds != null && factors != null)
            {
                AvgDayData = m_HourData.GetCaptureDataPager(portIds, factors, dtBegion, dtEnd);
                grdDataCaptureRate.DataSource = AvgDayData;
                grdDataCaptureRate.VirtualItemCount = AvgDayData.Count;
            }
            else
            {
                grdDataCaptureRate.DataSource = new DataTable();
            }
        }

        #endregion
        #region 服务器端控件事件处理
        /// <summary>
        /// 绑定数据源（用于分页、排序）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdDataCaptureRate_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }
        /// <summary>
        /// 数据行绑定处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdDataCaptureRate_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            GridDataItem item = e.Item as GridDataItem;
            DataRowView drv = e.Item.DataItem as DataRowView;
            string[] portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
            string[] factors = factorCbxRsm.GetFactorValues(SmartEP.Core.Enums.CbxRsmReturnType.Name);
            string factor = "";
            foreach (string strName in factors)
            {
                factor += strName + ";";
            }
            //DateTime dtBegion = Convert.ToDateTime(monthBegin.SelectedDate.Value.ToString("yyyy-MM-dd 00:00:00"));
            //DateTime dtEnd = Convert.ToDateTime(monthEnd.SelectedDate.Value.ToString("yyyy-MM-dd 23:59:59"));
            DateTime dtBegion = monthBegin.SelectedDate.Value;
            DateTime dtEnd = monthEnd.SelectedDate.Value.AddMonths(1).AddSeconds(-1);
            if (e.Item is GridDataItem)
            {
                for (int i = 0; i < portIds.Length; i++)
                {
                    string portName = g_MonitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(portIds[i])).MonitoringPointName;
                    if (item[portName] != null)
                    {
                        GridTableCell pointCell = (GridTableCell)item[portName];
                        pointCell.Text = string.Format("<a href='#' onclick='RowClick(\"{0}\",\"{1}\",\"{2}\",\"{3}\")'>{4}</a>", portName, dtBegion, dtEnd, factor, pointCell.Text);
                    }
                }
            }
        }
        /// <summary>
        /// 生成RadGrid的绑定列
        /// </summary>
        /// <param sender></param>
        /// <param e></param>
        protected void grdDataCaptureRate_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {
            try
            {
                GridBoundColumn col = e.Column as GridBoundColumn;
                if (col == null)
                    return;

                col.HeaderText = col.DataField;
                col.EmptyDataText = "--";
                col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                col.HeaderStyle.Width = Unit.Pixel(110);
                col.ItemStyle.Width = Unit.Pixel(110);

            }
            catch (Exception ex) { }
        }
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridRTB_ButtonClick(object sender, Telerik.Web.UI.RadToolBarEventArgs e)
        {
            try
            {
                Telerik.Web.UI.RadToolBarButton button = (Telerik.Web.UI.RadToolBarButton)e.Item;
                if (button.CommandName == "ExportToExcel")
                {
                    string[] portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
                    string[] factors = factorCbxRsm.GetFactorValues(SmartEP.Core.Enums.CbxRsmReturnType.Code);
                    //DateTime dtBegion = Convert.ToDateTime(monthBegin.SelectedDate.Value.ToString("yyyy-MM-dd 00:00:00"));
                    //DateTime dtEnd = Convert.ToDateTime(monthEnd.SelectedDate.Value.ToString("yyyy-MM-dd 23:59:59"));
                    DateTime dtBegion = monthBegin.SelectedDate.Value;
                    DateTime dtEnd = monthEnd.SelectedDate.Value;
                    //if (dtEnd >= dtBegion) 
                    //{
                    //    TimeSpan ts = dtEnd - dtBegion;
                    //    int day = ts.Days;
                    //    int i  = monthEnd.SelectedDate.Value - monthBegin.SelectedDate.Value;
                    Workbook workbook = new Workbook();
                    workbook.Worksheets.RemoveAt(0);
                    for (int i = 0; i<3; i ++)
                    {
                        if (i == 0)
                        {
                            DataTable dt = m_HourData.GetEffectiveDataNew(portIds, factors, dtBegion, dtEnd.AddMonths(1).AddSeconds(-1), true).Table;
                            DataTableToExcel1(workbook, dt, "子站数据运行率", factors);
                        }
                        if (i == 1)
                        {
                            DataTable dt = m_HourData.GetCaptureDataPagerNew(portIds, factors, dtBegion, dtEnd.AddMonths(1).AddSeconds(-1), true).Table;
                            DataTableToExcel2(workbook, dt, "有效数据捕获率", factors);
                        }
                        if (i == 2)
                        {
                            DataTable dt = m_HourData.GetCheckNew(portIds, dtBegion, dtEnd.AddMonths(1).AddSeconds(-1), true).Table;
                            DataTableToExcel3(workbook, dt, "Sheet1", portIds);
                        }

                    }
                    string now = DateTime.Now.ToString("yyyymmddhhmmss");
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "utf-8";
                    Response.AppendHeader("Content-Disposition", string.Format("attachment;filename={0}.xls", HttpUtility.UrlEncode("中意项目运维统计表" + now)));
                    Response.ContentEncoding = System.Text.Encoding.UTF8;
                    Response.ContentType = "application/ms-excel";
                    Response.BinaryWrite(workbook.SaveToStream().ToArray());
                    Response.End();
                    
                    //DataTable dt = m_HourData.GetCaptureDataPagerNew(portIds, factors, dtBegion, dtEnd, true).Table;
                    //ExcelHelper.DataTableToExcel(dt, "有效数据捕获率", "有效数据捕获率", this.Page);
                    //}
                    
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion
        /// <summary>
        /// 导出子站数据运行率
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="dt"></param>
        /// <param name="sheetName"></param>
        /// <param name="facto"></param>
        private void DataTableToExcel1(Workbook workbook, DataTable dt, string sheetName, string[] facto)
        {

            Worksheet sheet = workbook.Worksheets.Add(sheetName);
            Cells cells = sheet.Cells;
            Aspose.Cells.Style cellStyle = workbook.Styles[workbook.Styles.Add()];
            //workbook.FileName = fileName;
            //sheet.Name = sheetName;
            cellStyle.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center; // 设置居中 
            cellStyle.Font.Name = "宋体"; //文字字体
            cellStyle.Font.Size = 12;//文字大小
            cellStyle.IsLocked = false; //单元格解锁
            cellStyle.IsTextWrapped = true; //单元格内容自动换行
            DataTable dtNew = dt;
            DataView dv = new DataView(dtNew);
            ////第一行
            cells[0, 0].PutValue("子站数据运行率(%)");
            cells.Merge(0, 0, 1, dtNew.Columns.Count);
            cells.SetRowHeight(0, 20);//设置行高
            cells.SetColumnWidth(0, 25);//设置列宽
            //将DataTable的列名导入Excel表第一行
            int rowIndex = 1;
            int columnIndex = 0;
            
            foreach (DataColumn dc in dtNew.Columns)
            {
                
                cells[rowIndex, columnIndex].PutValue(dc.ColumnName.ToString());
                columnIndex++;
            }

            for (int n = 0; n < dtNew.Rows.Count; n++)
            {
                
                rowIndex = n + 2;
                for (int k = 0; k < dtNew.Columns.Count; k++)
                {
                    cells[rowIndex, k].PutValue(dv[n][k].ToString().Replace("%", ""));
                }
            }
            cells[dtNew.Rows.Count+2, 0].PutValue("合同要求");
            cells.Merge(dtNew.Rows.Count+2, 1, 1, dtNew.Columns.Count - 1);
            cells[dtNew.Rows.Count+3, 0].PutValue("备注");
            cells[dtNew.Rows.Count + 5, 0].PutValue("系统正常运行率=（系统运行总时数÷运行考核总时数）×100%，系统正常运行率以单站点计算，以系统正常运行率最低的站点作为托管项目的系统正常运行率。");
            //cells.Merge(dtNew.Rows.Count+3, 1, 1, dtNew.Columns.Count - 1);
            foreach (Cell cell in cells)
            {
                if (!cell.IsStyleSet)
                {
                    cell.SetStyle(cellStyle);
                }
            }
        }
        /// <summary>
        /// 导出有效数据捕获率
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="dt"></param>
        /// <param name="sheetName"></param>
        /// <param name="facto"></param>
        private void DataTableToExcel2(Workbook workbook, DataTable dt, string sheetName, string[] facto)
        {

            Worksheet sheet = workbook.Worksheets.Add(sheetName);
            Cells cells = sheet.Cells;
            Aspose.Cells.Style cellStyle = workbook.Styles[workbook.Styles.Add()];
            //workbook.FileName = fileName;
            //sheet.Name = sheetName;
            cellStyle.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center; // 设置居中 
            cellStyle.Font.Name = "宋体"; //文字字体
            cellStyle.Font.Size = 12;//文字大小
            cellStyle.IsLocked = false; //单元格解锁
            cellStyle.IsTextWrapped = true; //单元格内容自动换行
            DataTable dtNew = dt;

            //第一行
            cells[0, 0].PutValue("有效数据捕获率(%)");
            cells.Merge(0, 0, 1, dtNew.Columns.Count);
            
            cells.SetRowHeight(0, 20);//设置行高
            cells.SetColumnWidth(0, 25);//设置列宽

            //将DataTable的列名导入Excel表第一行
            int rowIndex = 1;
            int columnIndex = 0;
            foreach (DataColumn dc in dtNew.Columns)
            {

                cells[rowIndex, columnIndex].PutValue(dc.ColumnName.ToString());
                columnIndex++;
            }

            for (int n = 0; n < dtNew.Rows.Count; n++)
            {
                DataView dv = new DataView(dtNew);
                rowIndex = n + 2;
                for (int k = 0; k < dtNew.Columns.Count; k++)
                {
                    if (dv[n][k].ToString().Contains("%"))
                    {
                        cells[rowIndex, k].PutValue(dv[n][k].ToString().Replace("%",""));
                    }
                    else 
                    {
                        cells[rowIndex, k].PutValue(dv[n][k].ToString());
                    }
                    
                }
            }
            cells[dtNew.Rows.Count + 2, 0].PutValue("合同要求");
            cells.Merge(dtNew.Rows.Count + 2, 1, 1, dtNew.Columns.Count - 1);
            cells[dtNew.Rows.Count + 3, 0].PutValue("备注");
            cells[dtNew.Rows.Count + 5, 0].PutValue("注：有效数据捕获率=（有效运行时数÷运行考核总时数）×100%，数据有效捕获率以单台仪表计算，以数据有效捕获率最低的仪表作为托管项目的数据有效捕获率。");
            foreach (Cell cell in cells)
            {
                if (!cell.IsStyleSet)
                {
                    cell.SetStyle(cellStyle);
                }
            }
        }
        /// <summary>
        /// 导出Sheet1
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="dt"></param>
        /// <param name="sheetName"></param>
        /// <param name="facto"></param>
        private void DataTableToExcel3(Workbook workbook, DataTable dt, string sheetName, string[] portIds)
        {

            Worksheet sheet = workbook.Worksheets.Add(sheetName);
            Cells cells = sheet.Cells;
            Aspose.Cells.Style cellStyle = workbook.Styles[workbook.Styles.Add()];
            //workbook.FileName = fileName;
            //sheet.Name = sheetName;
            cellStyle.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center; // 设置居中 
            cellStyle.Font.Name = "宋体"; //文字字体
            cellStyle.Font.Size = 12;//文字大小
            cellStyle.IsLocked = false; //单元格解锁
            cellStyle.IsTextWrapped = true; //单元格内容自动换行
            DataTable dtNew = dt;


            //cells.SetRowHeight(0, 20);//设置行高
            
            ////第一行
            cells[0, 0].PutValue("站点");
            cells.Merge(0, 0, 1, 1);
            cells[0, 1].PutValue("考核科目");
            cells.SetColumnWidth(1, 40);//设置列宽
            cells[0, 2].PutValue("规范要求");
            cells.SetColumnWidth(2, 20);//设置列宽
            int n = 1;
            foreach (string portId in portIds)
            {
                cells.Merge(n,0,10,1);
                cells[n, 1].PutValue("巡检(次)");
                cells[n, 2].PutValue("每周");
                cells[n + 1, 1].PutValue("零/跨标准(次)");
                cells[n + 1, 2].PutValue("每周10％");
                cells[n + 2, 1].PutValue("精密度检查(次)");
                cells[n + 2, 2].PutValue("2周/10％、15％");
                cells[n + 3, 1].PutValue("多点线性检查(次)");
                cells[n + 3, 2].PutValue("每季或大修");
                cells[n + 4, 1].PutValue("钼炉转换率检查(次)");
                cells[n + 4, 2].PutValue("半年96～102％");
                cells[n + 5, 1].PutValue("流量校准(次)");
                cells[n + 5, 2].PutValue("每季7％、10％");
                cells[n + 6, 1].PutValue("采样支管清洗/更换/气密性检查(次)");
                cells[n + 6, 2].PutValue("每季");
                cells[n + 7, 1].PutValue("清洗采样总管及切割头/气密性检查(次)");
                cells[n + 7, 2].PutValue("每季");
                cells[n + 8, 1].PutValue("记录完整性");
                cells[n + 8, 2].PutValue("每月");
                cells[n + 9, 1].PutValue("标准物质溯源");
                cells[n + 9, 2].PutValue("计量认证");
                n += 10;
            }

            //cells.SetRowHeight(0, 20);//设置行高
            //cells.SetColumnWidth(0, 25);//设置列宽
            //将DataTable的列名导入Excel表第一行
            //int rowIndex = 1;
            //int columnIndex = 0;
            //foreach (DataColumn dc in dtNew.Columns)
            //{

            //    cells[rowIndex, columnIndex].PutValue(dc.ColumnName.ToString());
            //    columnIndex++;
            //}

            //for (int n = 0; n < dtNew.Rows.Count; n++)
            //{
            //    DataView dv = new DataView(dtNew);
            //    rowIndex = n + 2;
            //    for (int k = 0; k < dtNew.Columns.Count; k++)
            //    {
            //        cells[rowIndex, k].PutValue(dv[n][k].ToString());
            //    }
            //}

            foreach (Cell cell in cells)
            {
                if (!cell.IsStyleSet)
                {
                    cell.SetStyle(cellStyle);
                }
            }
        }
        /// <summary>
        /// 查询按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            grdDataCaptureRate.CurrentPageIndex = 0;
            grdDataCaptureRate.Rebind();
        }
    
    }
}