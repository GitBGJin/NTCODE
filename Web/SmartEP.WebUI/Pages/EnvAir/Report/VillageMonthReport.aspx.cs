using SmartEP.ReportLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Reporting;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.DomainModel.MonitoringBusiness;
using System.Data;
using SmartEP.Service.DataAnalyze.Air.AQIReport;
using SmartEP.Service.DataAnalyze.Report;
using Aspose.Cells;
using System.IO;
using SmartEP.Utilities.Web.UI;
using SmartEP.Service.DataAnalyze.Air.DataQuery;
using SmartEP.Core.Generic;


namespace SmartEP.WebUI.Pages.EnvAir.Report
{
    public partial class VillageMonthReport : SmartEP.WebUI.Common.BasePage
    {
        /// 数据处理服务
        /// </summary>
        private MonthAQIService m_MonthAQIService = new MonthAQIService();
        ReportLogService ReportLogService = new ReportLogService();
        DataQueryByDayService m_DataQueryByDayService = Singleton<DataQueryByDayService>.GetInstance();
        const string myConnName = "ConnStrEqmsAir";
        static String myConnStr = "Conn_Air";
        DateTime dt = DateTime.Now;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            // 绑定信息
            if (!IsPostBack)
            {

                #region 初使化时间及表
                monthBegin.DateInput.DisplayDateFormat = "yyyy-MM-dd";
                monthBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-01-01"));
                monthEnd.DateInput.DisplayDateFormat = "yyyy-MM-dd";
                monthEnd.SelectedDate = System.DateTime.Now;
                #endregion
                this.ViewState["DisplayPerson"] = PageHelper.GetQueryString("DisplayPerson");
                YearBegin.Items.Clear();//年开始
                BindType();

                if (YearBegin.Items.Count > 0)
                    YearBegin.Items[0].Checked = true;

                LoadingReport();

            }
        }
        #region 绑定基数类型
        public void BindType()
        {
            DataTable dvType = m_DataQueryByDayService.GetCheckRegionDataType();
            YearBegin.DataSource = dvType;
            YearBegin.DataTextField = "DataType";
            YearBegin.DataValueField = "DataType";
            YearBegin.DataBind();
        }
        #endregion
        /// <summary>
        /// 加载报表数据
        /// </summary>
        private void LoadingReport()
        {
            string titleText = string.Empty;
            System.DateTime endDate = Convert.ToDateTime(monthEnd.SelectedDate.Value.ToString("yyyy-MM-dd"));
            if (endDate > System.DateTime.Now)
                endDate = System.DateTime.Now;
            DateTime startDate = Convert.ToDateTime(monthBegin.SelectedDate.Value.ToString("yyyy-MM-dd"));
            string timeRange = string.Format("{0:yyyy年MM月dd日} 到 {1:yyyy年MM月dd日}", startDate, endDate);
            titleText = "(" + timeRange + ")";
            ViewState["BeginDT"] = startDate.ToString();
            ViewState["EndDT"] = endDate.ToString();
            string factorCode = "PM25";
            string factorName = "PM2.5";
            DateTime beginTime = DateTime.Parse(ViewState["BeginDT"].ToString());
            DateTime endTime = DateTime.Parse(ViewState["EndDT"].ToString());
            string year = YearBegin.SelectedValue;
            //  string topTitle = "各市、区环境空气质量改善情况统计表";

            string subColumn1 = beginTime.Year + "年" + beginTime.Month.ToString() + "月~" + endTime.Month.ToString() + "月";
            string subColumn2 = "与" + beginTime.AddYears(-1).Year + "年同期比较";
            InstanceReportSource instanceReportSource = new InstanceReportSource();
            VillageMonthRep rv = new VillageMonthRep();
            rv.ReportParameters.Add("beginTime", Telerik.Reporting.ReportParameterType.DateTime, beginTime);
            rv.ReportParameters.Add("endTime", Telerik.Reporting.ReportParameterType.DateTime, endTime);
            rv.ReportParameters.Add("year", Telerik.Reporting.ReportParameterType.String, year);
            rv.ReportParameters.Add("titleText", Telerik.Reporting.ReportParameterType.String, titleText);
            rv.ReportParameters.Add("factorName", Telerik.Reporting.ReportParameterType.String, factorName);
            rv.ReportParameters.Add("factorCode", Telerik.Reporting.ReportParameterType.String, factorCode);
            rv.ReportParameters.Add("subColumn1", Telerik.Reporting.ReportParameterType.String, subColumn1);
            rv.ReportParameters.Add("subColumn2", Telerik.Reporting.ReportParameterType.String, subColumn2);
            instanceReportSource.ReportDocument = rv;
            this.ReportViewer1.ReportSource = instanceReportSource;
            //this.ReportViewer1.ReportSource = rv;
            //ReportViewer1.Report.DocumentName = "各市、区" + factorName + "浓度及比较情况统计表(周报)";
            ReportViewer1.ShowPrintButton = false;
            ReportViewer1.ShowPrintPreviewButton = false;
            ReportViewer1.ShowExportGroup = false;

            RegisterScript("SetHeigth();");
        }
        protected void RunScript(String JsScrpit)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alarm", "<script type='text/javascript' language='javascript'>" + JsScrpit + "</script>", false);
        }


        protected void ShowMessage(String Msg)
        {
            RunJScript(String.Format("alert('{0}');", Msg));
        }

        protected void RunJScript(String JS)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "JS", String.Format("<script type='text/javascript' language='javascript'>{0}</script>", JS), false);
        }



        /// <summary>
        /// 解决RadMonthYearPicker控件的中文版的月不变的Bug
        /// 从月的英文缩写转换成中文
        /// </summary>
        /// <param name="cnName"></param>
        /// <returns></returns>
        public static string GetOtherMonthName(string cnName)
        {
            return new Dictionary<string, string>() { { "Jan", "1月" }, { "Feb", "2月" }, { "Mar", "3月" }, { "Apr", "4月" }, { "May", "5月" }, { "Jun", "6月" }, { "Jul", "7月" }, { "Aug", "8月" }, { "Sep", "9月" }, { "Oct", "10月" }, { "Nov", "11月" }, { "Dec", "12月" }, { "", "" } }[cnName];
        }

        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            if (monthBegin.SelectedDate == null || monthEnd.SelectedDate == null)
            {
                ShowMessage("时间不能为空!");
                return;
            }
            if (monthBegin.SelectedDate > monthEnd.SelectedDate)
            {
                ShowMessage("结束时间不能大于开始时间!");
                return;
            }
            if (monthBegin.SelectedDate.Value.Year != monthEnd.SelectedDate.Value.Year)
            {
                ShowMessage("时间范围不能跨年!");
                return;
            }
            if (Convert.ToDateTime(monthEnd.SelectedDate) < DateTime.Now)
            {
                LoadingReport();
            }
            else
            {
                ShowMessage("时间请小于当前时间！");

            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            if (monthBegin.SelectedDate == null || monthEnd.SelectedDate == null)
            {
                ShowMessage("时间不能为空!");
                return;
            }
            if (monthBegin.SelectedDate > monthEnd.SelectedDate)
            {
                ShowMessage("结束时间不能大于开始时间!");
                return;
            }
            if (monthBegin.SelectedDate.Value.Year != monthEnd.SelectedDate.Value.Year)
            {
                ShowMessage("时间范围不能跨年!");
                return;
            }

            string titleText = string.Empty;
            System.DateTime endDate = Convert.ToDateTime(monthEnd.SelectedDate.Value.ToString("yyyy-MM-dd"));
            if (endDate > System.DateTime.Now)
                endDate = System.DateTime.Now;
            DateTime startDate = Convert.ToDateTime(monthBegin.SelectedDate.Value.ToString("yyyy-MM-dd"));
            string timeRange = string.Format("{0:yyyy年MM月dd日} 到 {1:yyyy年MM月dd日}", startDate, endDate);
            titleText = "(" + timeRange + ")";
            ViewState["BeginDT"] = startDate.ToString();
            ViewState["EndDT"] = endDate.ToString();
            string factorCode = "PM25";
            string factorName = "PM2.5";
            DateTime beginTime = DateTime.Parse(ViewState["BeginDT"].ToString());
            DateTime endTime = DateTime.Parse(ViewState["EndDT"].ToString());
            string year = YearBegin.SelectedValue;
            string regionName = "市区均值;张家港;常熟;太仓市;昆山市;吴江区";

            string FileName = "(" + dt.ToString("yyyyMMddHHmmssffff") + ")" + "VillageMonthReport" + ".xls";
            var dv = new DataView();
            dv = m_MonthAQIService.GetData(beginTime, endTime, year, factorName, factorCode);
            //添加实体类对象
            ReportLogEntity customDatum = new ReportLogEntity();
            customDatum.PointIds = "";//测点Code
            customDatum.PointNames = regionName;//测点
            customDatum.FactorCodes = "a34004";//因子Code
            customDatum.FactorsNames = "PM2.5";//因子名称
            customDatum.DateTimeRange = string.Format("{0:yyyy年MM月dd日}~{1:yyyy年MM月dd日}", startDate, endDate);
            customDatum.WaterOrAirType = 1;//水或气 0：水，1：气
            customDatum.PageTypeID = "VillageMonthReport";//页面ID
            customDatum.StartDateTime = startDate;
            customDatum.EndDateTime = endDate;

            customDatum.CreatUser = this.ViewState["DisplayPerson"].ToString();
            customDatum.ExportName = "各市、区环境空气质量改善情况月报(" + string.Format("{0:yyyyMMdd}-{1:yyyyMMdd}", startDate, endDate)+")";
            customDatum.ReportName = ("../../../Pages/EnvAir/Report/ReportFile/VillageMonthReport/" + endDate.Year + "/" + endDate.Month + "/" + FileName).ToString();
            customDatum.CreatDateTime = DateTime.Now;
            //添加数据
            ReportLogService.ReportLogAdd(customDatum);
            DataTableToExcel(dv, "各市、区环境空气质量改善情况", "各市、区环境空气质量改善情况", 0);
        }

        /// <summary>
        /// 导出空气质量实时报
        /// </summary>
        /// <param name="dv">原始数据表</param>
        /// <returns></returns>
        private void DataTableToExcel(DataView dv, string fileName, string sheetName, int m)
        {
            DataTable dtNew = dv.ToTable();

            string titleText = string.Empty;
            System.DateTime endDate = Convert.ToDateTime(monthEnd.SelectedDate.Value.ToString("yyyy-MM-dd"));
            if (endDate > System.DateTime.Now)
                endDate = System.DateTime.Now;
            DateTime startDate = Convert.ToDateTime(monthBegin.SelectedDate.Value.ToString("yyyy-MM-dd"));
            string timeRange = string.Format("{0:yyyy年MM月dd日} 到 {1:yyyy年MM月dd日}", startDate, endDate);
            titleText = "(" + timeRange + ")";
            ViewState["BeginDT"] = startDate.ToString();
            ViewState["EndDT"] = endDate.ToString();

            DateTime beginTime = DateTime.Parse(ViewState["BeginDT"].ToString());
            DateTime endTime = DateTime.Parse(ViewState["EndDT"].ToString());
            string subColumn1 = beginTime.Year + "年" + beginTime.Month.ToString() + "月~" + endTime.Month.ToString() + "月";
            string subColumn2 = "与" + beginTime.AddYears(-1).Year + "年同期比较";

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
            cellStyle.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
            cellStyle.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            cellStyle.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            //标题
            Aspose.Cells.Style titleStyle = workbook.Styles[workbook.Styles.Add()];
            titleStyle.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
            titleStyle.Font.Size = 18;
            titleStyle.Font.IsBold = true;
            titleStyle.Font.Name = "宋体";

            #region 表头
            cells[0, 0].PutValue("各市、区环境空气质量改善情况统计表");
            cells.Merge(0, 0, 1, 6);
            //第一行
            cells[1, 0].PutValue("地区");
            cells.Merge(1, 0, 2, 1);
            cells[1, 1].PutValue("达标天数比例");
            cells.Merge(1, 1, 1, 2);
            cells[1, 3].PutValue("PM2.5浓度(μg/m³)");
            cells.Merge(1, 3, 1, 3);

            //第二行
            cells[2, 1].PutValue(subColumn1);
            cells.Merge(2, 1, 1, 1);
            cells[2, 2].PutValue(subColumn2);
            cells.Merge(2, 2, 1, 1);
            cells[2, 3].PutValue(subColumn1);
            cells.Merge(2, 3, 1, 1);
            cells[2, 4].PutValue(subColumn2);
            cells.Merge(2, 4, 1, 1);
            cells[2, 5].PutValue("与考核基数同期比较");
            cells.Merge(2, 5, 1, 1);

            cells.SetRowHeight(0, 30);//设置行高
            cells.SetRowHeight(1, 30);//设置行高
            cells.SetRowHeight(2, 30);//设置行高
            cells.SetColumnWidth(0, 15);//设置列宽

            cells[0, 0].SetStyle(titleStyle);
            for (int i = 1; i <= 6; i++)
            {
                cells.SetColumnWidth(i, 20);//设置列宽
            }
            for (int i = 3; i <= 13; i++)
            {
                cells.SetRowHeight(i, 20);//设置列宽
            }
            #endregion


            for (int i = 0; i < dtNew.Rows.Count; i++)
            {
                DataRow drNew = dtNew.Rows[i];
                int rowIndex = i + 3;
                cells[rowIndex, 0].PutValue(drNew["regionName"].ToString());
                cells[rowIndex, 1].PutValue(drNew["curRate"].ToString());
                cells[rowIndex, 2].PutValue(drNew["compare"].ToString());
                cells[rowIndex, 3].PutValue(drNew["curValue"].ToString());
                cells[rowIndex, 4].PutValue(drNew["comparePer"].ToString());
                cells[rowIndex, 5].PutValue(drNew["compareBase"].ToString());

                cells[rowIndex, 0].SetStyle(cellStyle);
                cells[rowIndex, 1].SetStyle(cellStyle);
                cells[rowIndex, 2].SetStyle(cellStyle);
                cells[rowIndex, 3].SetStyle(cellStyle);
                cells[rowIndex, 4].SetStyle(cellStyle);
                cells[rowIndex, 5].SetStyle(cellStyle);
            }

            cells[1, 0].SetStyle(cellStyle);
            cells[1, 1].SetStyle(cellStyle);
            cells[1, 2].SetStyle(cellStyle);
            cells[1, 3].SetStyle(cellStyle);
            cells[1, 4].SetStyle(cellStyle);
            cells[1, 5].SetStyle(cellStyle);
            cells[2, 0].SetStyle(cellStyle);
            cells[2, 1].SetStyle(cellStyle);
            cells[2, 2].SetStyle(cellStyle);
            cells[2, 3].SetStyle(cellStyle);
            cells[2, 4].SetStyle(cellStyle);
            cells[2, 5].SetStyle(cellStyle);
           
            //foreach (Cell cell in cells)
            //{
            //    if (!cell.IsStyleSet)
            //    {
            //        cell.SetStyle(cellStyle);
            //    }
            //}
            string FileName = "(" + dt.ToString("yyyyMMddHHmmssffff") + ")" + "VillageMonthReport" + ".xls";
            string strTarget = Server.MapPath("../../../Pages/EnvAir/Report/ReportFile/VillageMonthReport/" + endTime.Year + "/" + endTime.Month + "/" + FileName);
            string path = Server.MapPath("../../../Pages/EnvAir/Report/ReportFile/VillageMonthReport/" + endTime.Year + "/" + endTime.Month + "/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);//创建新路径
            }
            workbook.Save(strTarget);
            if (m == 1)
            {
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "utf-8";
                Response.AppendHeader("Content-Disposition", string.Format("attachment;filename={0}.xls", HttpUtility.UrlEncode(fileName)));
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.ContentType = "application/ms-excel";
                Response.BinaryWrite(workbook.SaveToStream().ToArray());
                Response.End();
            }
            if (!Directory.Exists(strTarget))
            {
                Alert("保存成功！");
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "reload", "<script type=\"text/javascript\">self.parent.location.reload();</script>", false);
        }

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            if (monthBegin.SelectedDate == null || monthEnd.SelectedDate == null)
            {
                ShowMessage("时间不能为空!");
                return;
            }
            if (monthBegin.SelectedDate > monthEnd.SelectedDate)
            {
                ShowMessage("结束时间不能大于开始时间!");
                return;
            }
            if (monthBegin.SelectedDate.Value.Year != monthEnd.SelectedDate.Value.Year)
            {
                ShowMessage("时间范围不能跨年!");
                return;
            }

            string titleText = string.Empty;
            System.DateTime endDate = Convert.ToDateTime(monthEnd.SelectedDate.Value.ToString("yyyy-MM-dd"));
            if (endDate > System.DateTime.Now)
                endDate = System.DateTime.Now;
            DateTime startDate = Convert.ToDateTime(monthBegin.SelectedDate.Value.ToString("yyyy-MM-dd"));
            string timeRange = string.Format("{0:yyyy年MM月dd日} 到 {1:yyyy年MM月dd日}", startDate, endDate);
            titleText = "(" + timeRange + ")";
            ViewState["BeginDT"] = startDate.ToString();
            ViewState["EndDT"] = endDate.ToString();
            string factorCode = "PM25";
            string factorName = "PM2.5";
            DateTime beginTime = DateTime.Parse(ViewState["BeginDT"].ToString());
            DateTime endTime = DateTime.Parse(ViewState["EndDT"].ToString());
            string year = YearBegin.SelectedValue;
            string regionName = "市区均值;张家港;常熟;太仓市;昆山市;吴江区";

            string FileName = "(" + dt.ToString("yyyyMMddHHmmssffff") + ")" + "VillageMonthReport" + ".xls";
            var dv = new DataView();
            dv = m_MonthAQIService.GetData(beginTime, endTime, year, factorName, factorCode);
            //添加实体类对象
            ReportLogEntity customDatum = new ReportLogEntity();
            customDatum.PointIds = "";//测点Code
            customDatum.PointNames = regionName;//测点
            customDatum.FactorCodes = "a34004";//因子Code
            customDatum.FactorsNames = "PM2.5";//因子名称
            customDatum.DateTimeRange = string.Format("{0:yyyy年MM月dd日}~{1:yyyy年MM月dd日}", startDate, endDate);
            customDatum.WaterOrAirType = 1;//水或气 0：水，1：气
            customDatum.PageTypeID = "VillageMonthReport";//页面ID
            customDatum.StartDateTime = startDate;
            customDatum.EndDateTime = endDate;

            customDatum.CreatUser = this.ViewState["DisplayPerson"].ToString();
            customDatum.ExportName = "各市、区环境空气质量改善情况月报(" + string.Format("{0:yyyyMMdd}-{1:yyyyMMdd}", startDate, endDate) + ")";
            customDatum.ReportName = ("../../../Pages/EnvAir/Report/ReportFile/VillageMonthReport/" + endDate.Year + "/" + endDate.Month + "/" + FileName).ToString();
            customDatum.CreatDateTime = DateTime.Now;
            //添加数据
            ReportLogService.ReportLogAdd(customDatum);
            DataTableToExcel(dv, "各市、区环境空气质量改善情况", "各市、区环境空气质量改善情况", 0);

        }
    }
}