using Aspose.Cells;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.Core.Enums;
using SmartEP.Service.DataAnalyze.Air.AQIReport;
using SmartEP.Service.DataAnalyze.Air.DataQuery;
using SmartEP.Service.Frame;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.Report
{
    /// <summary>
    /// 名称：ConcentrationAQIReport.aspx.cs
    /// 创建人：刘长敏
    /// 创建日期：2016-1-21
    /// 维护人员：
    /// 最新维护人员：刘晋
    /// 最新维护日期：2017-07-13
    /// 功能摘要：浓度_AQI综合查询
    /// 虚拟分页类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class ConcentrationAQIReport : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        private ConcentrationAQIService m_ConcentrationAQIService;
        DictionaryService dicService = new DictionaryService();
        MonitoringPointAirService pointAirService = new MonitoringPointAirService();
        DataQueryByDayService m_DataQueryByDayService = Singleton<DataQueryByDayService>.GetInstance();
        /// <summary>
        /// 选择站点
        /// </summary>
        private IList<IPoint> points = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            m_ConcentrationAQIService = new ConcentrationAQIService();
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

            YearBegin.Items.Clear();//年开始
            BindType();

            if (YearBegin.Items.Count > 0)
                YearBegin.Items[0].Checked = true;

            Year.Items.Clear();//年开始
            int yearNow = DateTime.Now.Year;
            int year = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["Year"]);
            for (int i = yearNow; i >= year; i--)
            {
                //年开始
                RadComboBoxItem cmbItemYearBegin = new RadComboBoxItem();
                cmbItemYearBegin.Text = i.ToString();
                cmbItemYearBegin.Value = i.ToString();
                if (i == yearNow - 1)
                    cmbItemYearBegin.Checked = true;
                Year.Items.Add(cmbItemYearBegin);
            }
            Year.DataBind();
            //国控点，对照点，路边站
            MonitoringPointAirService m_MonitoringPointAirService = Singleton<MonitoringPointAirService>.GetInstance();
            string strpointName = "";
            IQueryable<MonitoringPointEntity> EnableOrNotports = m_MonitoringPointAirService.RetrieveAirMPListByEnable();
            string[] EnableOrNotportsarry = EnableOrNotports.Where(p => p.ContrlUid == "6fadff52-2338-4319-9f1d-7317823770ad" || p.ContrlUid == "bdf0837a-eb59-4c4a-a05f-c774a17f3077" || p.ContrlUid == "c1158eb6-4d69-4846-a963-d16b9d2794ca").Select(p => p.MonitoringPointName).ToArray();
            foreach (string point in EnableOrNotportsarry)
            {
                strpointName += point + ";";
            }
            string names = ConfigurationManager.AppSettings["NTRegionPointName"].ToString();    //从配置文件获取默认站点
            pointCbxRsm.SetPointValuesFromNames(names);
            //pointCbxRsm.SetPointValuesFromNames(strpointName);


        }
        #endregion
        #region 绑定基数类型
        public void BindType()
        {
            if (rbtnlType.SelectedValue == "Port")
            {
                DataTable dvType = m_DataQueryByDayService.GetCheckDataType();
                YearBegin.DataSource = dvType;
                YearBegin.DataTextField = "DataType";
                YearBegin.DataValueField = "DataType";
                YearBegin.DataBind();
            }
            else
            {
                DataTable dvType = m_DataQueryByDayService.GetCheckRegionDataType();
                YearBegin.DataSource = dvType;
                YearBegin.DataTextField = "DataType";
                YearBegin.DataValueField = "DataType";
                YearBegin.DataBind();
            }
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

            string factor = "";
            foreach (RadComboBoxItem item in factorCbxRsm.CheckedItems)
            {
                factor += (item.Value.ToString() + ",");
            }
            string[] factors = factor.Trim(',').Split(',');

            points = pointCbxRsm.GetPoints();
            switch (rbtnlType.SelectedValue)
            {
                case "Port":
                    portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
                    break;
                case "CityProper":
                    //foreach (RadComboBoxItem item in comboCity.CheckedItems)
                    //{
                    //    regionGuid += (item.Value.ToString() + ",");
                    //}
                    portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
                    break;
            }
            string[] regionGuids = regionGuid.Trim(',').Split(',');
            DateTime dtBegion = rdpBegin.SelectedDate.Value;
            DateTime dtEnd = rdpEnd.SelectedDate.Value;

            DateTime mBegion = Convert.ToDateTime(dtBegion.ToString("yyyy-MM-dd"));  //本期第一天
            DateTime mEnd = Convert.ToDateTime(dtEnd.ToString("yyyy-MM-dd"));   //本期当天
            DateTime smBegion = Convert.ToDateTime(dtBegion.AddYears(-1).ToString("yyyy-MM-dd"));   //同期第一天
            DateTime smEnd = Convert.ToDateTime(dtEnd.AddYears(-1).ToString("yyyy-MM-dd"));      //同期当天
            string monthB = dtBegion.ToString("MM-dd");
            string monthE = dtEnd.ToString("MM-dd");

            string yearAll = "";
            foreach (RadComboBoxItem item in Year.CheckedItems)
            {
                yearAll += (item.Value.ToString() + ",");
            }
            string[] years = yearAll.Trim(',').Split(',');

            string yearBase = "";
            foreach (RadComboBoxItem item in YearBegin.CheckedItems)
            {
                yearBase += (item.Value.ToString() + ",");
            }
            string[] year = yearBase.Trim(',').Split(',');
            //生成RadGrid的绑定列
            //每页显示数据个数            
            int pageSize = grdAvgDayRange.PageSize;
            //当前页的序号
            int pageNo = grdAvgDayRange.CurrentPageIndex;
            var AvgDayData = new DataView();

            int recordTotal = 0;
            //点位
            if (rbtnlType.SelectedValue == "Port")
            {
                if (portIds != null && factors != null)
                {
                    AvgDayData = m_ConcentrationAQIService.GetConcentrationDataPager(portIds, factors, dtBegion, dtEnd, year, years, pageSize, pageNo, out recordTotal, COnAQI.SelectedValue);
                    AvgDayData.Sort = "PointId asc";
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
                    AvgDayData = m_ConcentrationAQIService.GetAreaDataPagerNew(portIds, factors, dtBegion, dtEnd, year, years, pageSize, pageNo, out recordTotal, COnAQI.SelectedValue);

                    grdAvgDayRange.DataSource = AvgDayData;
                    grdAvgDayRange.VirtualItemCount = AvgDayData.Count;
                }
                else
                {
                    grdAvgDayRange.DataSource = new DataTable();
                }
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
        /// grid生成列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdAvgDayRange_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            try
            {
                GridBoundColumn col = e.Column as GridBoundColumn;
                if (col == null)
                    return;

                if (col.DataField == "PointName")
                {
                    col.HeaderText = "测点";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "PointId")
                {
                    col.HeaderText = string.Empty;
                    col.Visible = false;
                }
                else if (col.DataField.Contains("a34004"))
                {
                    col.HeaderText = col.DataField.Remove(0, 6);
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(180);
                    col.ItemStyle.Width = Unit.Pixel(180);
                    col.ColumnGroupName = "PM25";
                }
                else if (col.DataField.Contains("a34002"))
                {
                    col.HeaderText = col.DataField.Remove(0, 6);
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(180);
                    col.ItemStyle.Width = Unit.Pixel(180);
                    col.ColumnGroupName = "PM10";
                }
                else if (col.DataField.Contains("a21004"))
                {
                    col.HeaderText = col.DataField.Remove(0, 6);
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(180);
                    col.ItemStyle.Width = Unit.Pixel(180);
                    col.ColumnGroupName = "NO2";
                }
                else if (col.DataField.Contains("a21026"))
                {
                    col.HeaderText = col.DataField.Remove(0, 6);
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(180);
                    col.ItemStyle.Width = Unit.Pixel(180);
                    col.ColumnGroupName = "SO2";
                }
                else if (col.DataField.Contains("a21005"))
                {
                    col.HeaderText = col.DataField.Remove(0, 6);
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(180);
                    col.ItemStyle.Width = Unit.Pixel(180);
                    col.ColumnGroupName = "CO";
                }
                else if (col.DataField.Contains("a05024"))
                {
                    col.HeaderText = col.DataField.Remove(0, 6);
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(180);
                    col.ItemStyle.Width = Unit.Pixel(180);
                    col.ColumnGroupName = "O3";
                }
                else
                {
                    col.HeaderText = col.DataField;
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(180);
                    col.ItemStyle.Width = Unit.Pixel(180);
                    col.ColumnGroupName = "AQI";
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
                DateTime dtBegion = rdpBegin.SelectedDate.Value;
                DateTime dtEnd = rdpEnd.SelectedDate.Value;
                string factor = "";
                foreach (RadComboBoxItem item in factorCbxRsm.CheckedItems)
                {
                    factor += (item.Value.ToString() + ",");
                }
                string[] factors = factor.Trim(',').Split(',');
                //string factor = factorCbxRsm.SelectedValue;
                string factorStr = "";
                if (factor != "")
                {
                    switch (factor)
                    {
                        case "a34004":
                            factorStr = "PM2.5";
                            break;
                        case "a34002":
                            factorStr = "PM10";
                            break;
                        case "a21005":
                            factorStr = "一氧化碳(CO)";
                            break;
                        case "a21004":
                            factorStr = "二氧化氮(NO2)";
                            break;
                        case "a21026":
                            factorStr = "二氧化硫(SO2)";
                            break;
                        case "a05024":
                            factorStr = "臭氧(8小时最值)";
                            break;
                    }
                }
                else
                {
                    Alert("请选择因子！");
                    return;
                }
                //每页显示数据个数            
                int pageSize = grdAvgDayRange.PageSize;
                //当前页的序号
                int pageNo = grdAvgDayRange.CurrentPageIndex;
                string yearAll = "";
                foreach (RadComboBoxItem item in Year.CheckedItems)
                {
                    yearAll += (item.Value.ToString() + ",");
                }
                string[] years = yearAll.Trim(',').Split(',');

                string yearBase = "";
                foreach (RadComboBoxItem item in YearBegin.CheckedItems)
                {
                    yearBase += (item.Value.ToString() + ",");
                }
                string[] year = yearBase.Trim(',').Split(',');
                int recordTotal = 0;
                DataView dv = new DataView();
                if (rbtnlType.SelectedValue == "Port")
                {
                    if (pointIds != null)
                    {
                        dv = m_ConcentrationAQIService.GetConcentrationDataPager(pointIds, factors, dtBegion, dtEnd, year, years, pageSize, pageNo, out recordTotal, COnAQI.SelectedValue);
                        dv.Sort = "PointId asc";
                    }
                }
                else if (rbtnlType.SelectedValue == "CityProper")
                {
                    string[] regionUids = { };
                    switch (rbtnlType.SelectedValue)
                    {
                        case "CityProper":
                            //regionUids = comboCity.CheckedItems.Select(t => t.Value).ToArray();
                            break;
                        default: break;
                    }
                    if (regionUids != null)
                    {
                        dv = m_ConcentrationAQIService.GetAreaDataPagerNew(pointIds, factors, dtBegion, dtEnd, year, years, pageSize, pageNo, out recordTotal, COnAQI.SelectedValue);
                    }
                }
                DataTableToExcel(dv, "浓度_AQI综合查询", "浓度_AQI综合查询", factor, factorStr, COnAQI.SelectedValue);
            }
        }

        /// <summary>
        /// 导出空气质量实时报
        /// </summary>
        /// <param name="dv">原始数据表</param>
        /// <returns></returns>
        private void DataTableToExcel(DataView dv, string fileName, string sheetName, string factor, string factorStr, string COnAQI)
        {
            DataTable dtNew = dv.ToTable();
            DateTime dtBegion = rdpBegin.SelectedDate.Value;
            DateTime dtEnd = rdpEnd.SelectedDate.Value;
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


            #region 表头

            //第二行
            string name = "";
            string names = "";
            int count = 0;
            foreach (DataColumn c in dtNew.Columns)
            {
                name = c.ColumnName;
                names += (name + ",");
                if (name != "PointId" && name != "PointName")
                {
                    ++count;
                    if (name.Contains("a34004"))
                    {
                        name = "PM2.5" + name.Substring(6, name.Length-6);
                    }
                    else if (name.Contains("a34002"))
                    {
                        name = "PM10" + name.Substring(6, name.Length-6);
                    }
                    else if (name.Contains("a21005"))
                    {
                        name = "一氧化碳(CO)" + name.Substring(6, name.Length-6);
                    }
                    else if (name.Contains("a21004"))
                    {
                        name = "二氧化氮(NO2)" + name.Substring(6, name.Length-6);
                    }
                    else if (name.Contains("a21026"))
                    {
                        name = "二氧化硫(SO2)" + name.Substring(6, name.Length-6);
                    }
                    else if (name.Contains("a05024"))
                    {
                        name = "臭氧(8小时最值)" + name.Substring(6, name.Length-6);
                    }
                    cells[1, count].PutValue(name);
                    cells.Merge(1, count, 1, 1);
                }
            }
            string[] nameAll = names.Trim(',').Split(',');

            //第一行
            cells[0, 0].PutValue("地区");
            cells.Merge(0, 0, 2, 1);
            if (COnAQI == "1")
            {
                if (factor == "a21005")
                    cells[0, 1].PutValue(factorStr + "平均浓度" + "(mg/m³)");
                else
                    cells[0, 1].PutValue(factorStr + "平均浓度" + "(μg/m³)");
            }
            else
            {
                cells[0, 1].PutValue("平均AQI");
            }
            cells.Merge(0, 1, 1, nameAll.Length - 2);


            cells.SetRowHeight(0, 30);//设置行高
            cells.SetRowHeight(1, 30);//设置行高
            cells.SetColumnWidth(0, 20);//设置列宽
            for (int i = 1; i <= nameAll.Length - 2; i++)
            {
                cells.SetColumnWidth(i, 15);//设置列宽
            }
            #endregion


            for (int i = 0; i < dtNew.Rows.Count; i++)
            {
                DataRow drNew = dtNew.Rows[i];
                int rowIndex = i + 2;
                cells[rowIndex, 0].PutValue(drNew["PointName"].ToString());
                for (int j = 1; j <= count; j++)
                {
                    int m = j + 1;
                    cells[rowIndex, j].PutValue(drNew[nameAll[m]].ToString());
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
            //comboCity.Visible = false;
            switch (rbtnlType.SelectedValue)
            {
                case "CityProper":
                    //comboCity.Visible = true;
                    dvPoint.Style["display"] = "normal";
                    break;
                case "Port":
                    dvPoint.Style["display"] = "normal";
                    break;
            }
            if (YearBegin.Items.Count > 0)
                YearBegin.Items[0].Checked = true;
        }
        #endregion
    }
}