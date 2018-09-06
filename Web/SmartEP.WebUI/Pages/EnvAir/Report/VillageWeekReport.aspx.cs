using Aspose.Cells;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.ReportLibrary;
using SmartEP.Service.BaseData.BusinessRule;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.Core.Enums;
using SmartEP.Service.DataAnalyze.Air.AQIReport;
using SmartEP.Service.DataAnalyze.Air.DataQuery;
using SmartEP.Service.DataAnalyze.Enums;
using SmartEP.Service.DataAnalyze.Report;
using SmartEP.Service.Frame;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Utilities.Office;
using SmartEP.Utilities.Web.UI;
using SmartEP.WebControl.CbxRsm;
using SmartEP.WebUI.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Reporting;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.Report
{
    /// <summary>
    /// 名称：VillageWeekReport.cs
    /// 创建人：邱奇
    /// 创建日期：2015-10-14
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：市区、区县周报
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class VillageWeekReport : SmartEP.WebUI.Common.BasePage
    {
        /// 数据处理服务
        /// </summary>
        private MonthAQIService m_MonthAQIService = new MonthAQIService();
        const string myConnName = "ConnStrEqmsAir";
        static String myConnStr = "Conn_Air";
        ReportLogService ReportLogService = new ReportLogService();
        DataQueryByDayService m_DataQueryByDayService = Singleton<DataQueryByDayService>.GetInstance();
        DateTime dt = DateTime.Now;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            // 绑定信息
            if (!IsPostBack)
            {

                #region 初使化时间及表
                RadMonthYearPicker1.SelectedDate = System.DateTime.Now.AddDays(-1);
                BindWeekComboBox(true);
                YearBegin.Items.Clear();//年开始
                BindType();
                if (YearBegin.Items.Count > 0)
                    YearBegin.Items[0].Checked = true;
                this.ViewState["DisplayPerson"] = PageHelper.GetQueryString("DisplayPerson");
                #endregion
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
            System.DateTime endDate = Convert.ToDateTime(RadMonthYearPicker1.SelectedDate.Value); ;
            if (endDate > System.DateTime.Now)
                endDate = System.DateTime.Now;
            string timeRange = string.Format("{0:yyyy-01-01}到{1:yyyy-MM-dd}", endDate, endDate);
            titleText = endDate.Year + "年第" + WeekOfYear(endDate) + "期(" + timeRange + ")";
            ViewState["BeginDT"] = Convert.ToDateTime(endDate.Year + "-01-01 0:00:00");
            ViewState["EndDT"] = endDate.ToString();
            string factorCode = cbxFactor.SelectedValue;
            string factorName = cbxFactor.SelectedItem.Text;
            string titleDan = "微克/立方米";
            if (factorName == "一氧化碳")
                titleDan = "毫克/立方米";
            DateTime beginTime = DateTime.Parse(ViewState["BeginDT"].ToString());
            DateTime endTime = DateTime.Parse(ViewState["EndDT"].ToString());
            //string url = string.Format("ShowVillageWeekReport.aspx?beginTime={0}&endTime={1}&titleText={2}&factorName={3}&factorCode={4}", ViewState["BeginDT"], ViewState["EndDT"], titleText, factorName, factorCode);
            //RunScript("IFrameReHeigth('" + url + "');");
            string topTitle = "各市、区" + factorName + "浓度及比较情况统计表(周报)";
            string year = YearBegin.SelectedValue;
            string column1 = factorName + "浓度";
            string subColumn1 = beginTime.AddYears(-1).Year + "年";
            string subColumn2 = beginTime.Year + "年";
            string subColumn3 = "与" + beginTime.AddYears(-1).Year + "年比较";

            InstanceReportSource instanceReportSource = new InstanceReportSource();
            VillageWeekRep rv = new VillageWeekRep();
            rv.ReportParameters.Add("beginTime", Telerik.Reporting.ReportParameterType.DateTime, beginTime);
            rv.ReportParameters.Add("endTime", Telerik.Reporting.ReportParameterType.DateTime, endTime);
            rv.ReportParameters.Add("year", Telerik.Reporting.ReportParameterType.String, year);
            rv.ReportParameters.Add("titleText", Telerik.Reporting.ReportParameterType.String, titleText);
            rv.ReportParameters.Add("titleDan", Telerik.Reporting.ReportParameterType.String, titleDan);
            rv.ReportParameters.Add("factorName", Telerik.Reporting.ReportParameterType.String, factorName);
            rv.ReportParameters.Add("factorCode", Telerik.Reporting.ReportParameterType.String, factorCode);
            rv.ReportParameters.Add("top", Telerik.Reporting.ReportParameterType.String, topTitle);
            rv.ReportParameters.Add("column1", Telerik.Reporting.ReportParameterType.String, column1);
            rv.ReportParameters.Add("subColumn1", Telerik.Reporting.ReportParameterType.String, subColumn1);
            rv.ReportParameters.Add("subColumn2", Telerik.Reporting.ReportParameterType.String, subColumn2);
            rv.ReportParameters.Add("subColumn3", Telerik.Reporting.ReportParameterType.String, subColumn3);

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

        protected void RadMonthYearPicker1_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            BindWeekComboBox(false);
        }

        //protected void cmboWeeks_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        //{
        //    SetLiteral();
        //}

        private void BindWeekComboBox(bool isChange)
        {
            //System.DateTime cuMonth = RadMonthYearPicker1.SelectedDate ?? System.DateTime.Now;
            System.DateTime cuMonth = Convert.ToDateTime(RadMonthYearPicker1.SelectedDate);
            //cmboWeeks.DataValueField = "value";
            //cmboWeeks.DataTextField = "text";
            System.Collections.Generic.List<object> weeks = GetWeekOfMonth(cuMonth);

            if (Convert.ToDateTime(RadMonthYearPicker1.SelectedDate) < DateTime.Now)
            {
                //cmboWeeks.DataSource = weeks;
                //cmboWeeks.DataBind();
                //默认显示上周  
                if (isChange)
                {
                    for (int i = weeks.Count - 1; i >= 0; i--)
                    {
                        DateTime lastWeek = Convert.ToDateTime(DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd"));
                        string str = weeks[i].ToString().Split(',')[0].Trim().Replace("{", "").Replace("value", "").Replace("=", "").Trim();
                        DateTime weekTime = Convert.ToDateTime(weeks[i].ToString().Split(',')[0].Trim().Replace("{", "").Replace("value", "").Replace("=", "").Trim());
                        if (lastWeek >= weekTime)
                        {
                            //object obj = weeks[i];

                            //cmboWeeks.SelectedIndex = i;
                            //for (int j = i; j > 0; j--)
                            //{

                            //    weeks[j] = weeks[j - 1];
                            //}
                            //weeks[0] = obj;
                            break;
                        }
                    }
                }

                SetLiteral();
            }
            else
            {
                ShowMessage("时间请小于当前时间！");

            }
        }

        private void SetLiteral()
        {
            if (!RadMonthYearPicker1.SelectedDate.Value.Equals("") && Convert.ToDateTime(RadMonthYearPicker1.SelectedDate) < DateTime.Now)
            {
                System.DateTime endDate = Convert.ToDateTime(RadMonthYearPicker1.SelectedDate);
                if (endDate > System.DateTime.Now)
                    endDate = System.DateTime.Now;

                //Literal1.Text = string.Format("时间：从{0:yyyy-MM-dd}到{1:yyyy-MM-dd}", cmboWeeks.SelectedValue, endDate);
            }
            else
            {
                ShowMessage("时间请小于当前时间！");

            }
        }

        protected void RadMonthYearPicker1_ViewCellCreated(object sender, MonthYearViewCellCreatedEventArgs e)
        {
            //Telerik.Web.UI.Calendar.MonthYearViewCellType
            if (e.Cell.CellType == Telerik.Web.UI.Calendar.MonthYearViewCellType.MonthCell)
            {
                try
                {
                    System.Web.UI.WebControls.HyperLink month = e.Cell.Controls[0] as System.Web.UI.WebControls.HyperLink;
                    month.Text = (month.Text);
                }
                catch { }

            }
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
        /// 返回每个月所有周的开始时间及周数
        /// </summary>
        /// <param name="cuMonth"></param>
        /// <returns></returns>
        public static System.Collections.Generic.List<object> GetWeekOfMonth(System.DateTime cuMonth)
        {
            cuMonth = System.DateTime.ParseExact(cuMonth.ToString("yyyy-MM-01"), "yyyy-MM-dd", null);
            System.DateTime nextMonth = cuMonth.AddMonths(1);//.AddDays(7);
            System.Collections.Generic.List<object> weeks = new List<object>();
            int startDayOfWeek = (int)cuMonth.DayOfWeek;
            if (startDayOfWeek == 0) startDayOfWeek = 7;
            cuMonth = cuMonth.AddDays(-startDayOfWeek + 1);

            int i = 1;
            while (System.DateTime.Compare(cuMonth, nextMonth) < 0 && System.DateTime.Compare(cuMonth, System.DateTime.Now) < 0)
            {
                string week = string.Format("第{0}周", i);
                if (!weeks.Contains(week))
                {
                    weeks.Add(new { value = cuMonth.ToString("yyyy-MM-dd"), text = week });
                    i++;
                }
                cuMonth = cuMonth.AddDays(7);

            }
            return weeks;
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
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            if (RadMonthYearPicker1.SelectedDate == null)
            {
                ShowMessage("时间不能为空!");
                return;
            }
            if (Convert.ToDateTime(RadMonthYearPicker1.SelectedDate) < DateTime.Now)
            {
                LoadingReport();
            }
            else
            {
                ShowMessage("时间请小于当前时间！");

            }
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            if (RadMonthYearPicker1.SelectedDate == null)
            {
                ShowMessage("时间不能为空!");
                return;
            }
            if (Convert.ToDateTime(RadMonthYearPicker1.SelectedDate) > DateTime.Now)
            {
                ShowMessage("时间请小于当前时间！");
            }

            string titleText = string.Empty;
            System.DateTime endDate = Convert.ToDateTime(RadMonthYearPicker1.SelectedDate);
            if (endDate > System.DateTime.Now)
                endDate = System.DateTime.Now;
            string timeRange = string.Format("{0:yyyy-01-01}到{1:yyyy-MM-dd}", endDate, endDate);
            titleText = endDate.Year + "年第" + WeekOfYear(endDate) + "期(" + timeRange + ")";
            ViewState["BeginDT"] = Convert.ToDateTime(endDate.Year + "-01-01 0:00:00");
            ViewState["EndDT"] = endDate.ToString();
            string factorCode = cbxFactor.SelectedValue;
            string factorName = cbxFactor.SelectedItem.Text;
            DateTime beginTime = DateTime.Parse(ViewState["BeginDT"].ToString());
            DateTime endTime = DateTime.Parse(ViewState["EndDT"].ToString());
            //string url = string.Format("ShowVillageWeekReport.aspx?beginTime={0}&endTime={1}&titleText={2}&factorName={3}&factorCode={4}", ViewState["BeginDT"], ViewState["EndDT"], titleText, factorName, factorCode);
            //RunScript("IFrameReHeigth('" + url + "');");
            string topTitle = "各市、区" + factorName + "浓度及比较情况统计表(周报)";

            string column1 = factorName + "浓度";
            string subColumn1 = beginTime.AddYears(-1).Year + "年";
            string subColumn2 = beginTime.Year + "年";
            string subColumn3 = "与" + beginTime.AddYears(-1).Year + "年比较";
            string year = YearBegin.SelectedValue;
            string regionName = "市区均值;张家港;常熟;太仓市;昆山市;吴江区";

            string FileName = "(" + dt.ToString("yyyyMMddHHmmssffff") + ")" + "VillageWeekReport" + ".xls";
            var dv = new DataView();
            dv = m_MonthAQIService.GetWeekData(beginTime, endTime, year, factorName, factorCode);
            //添加实体类对象
            ReportLogEntity customDatum = new ReportLogEntity();
            customDatum.PointIds = "";//测点Code
            customDatum.PointNames = regionName;//测点
            customDatum.FactorCodes = "a34004";//因子Code
            customDatum.FactorsNames = "PM2.5";//因子名称
            customDatum.DateTimeRange = string.Format("{0:yyyy年MM月dd日}~{1:yyyy年MM月dd日}", beginTime, endDate);
            customDatum.WaterOrAirType = 1;//水或气 0：水，1：气
            customDatum.PageTypeID = "VillageWeekReport";//页面ID
            customDatum.StartDateTime = beginTime;
            customDatum.EndDateTime = endDate;

            customDatum.CreatUser = this.ViewState["DisplayPerson"].ToString();
            customDatum.ExportName = "各市、区PM2.5浓度及比较情况统计表周报(" + string.Format("{0:yyyyMMdd}-{1:yyyyMMdd}", beginTime, endDate) + ")";
            customDatum.ReportName = ("../../../Pages/EnvAir/Report/ReportFile/VillageWeekReport/" + endDate.Year + "/" + endDate.Month + "/" + FileName).ToString();
            customDatum.CreatDateTime = DateTime.Now;
            //添加数据
            ReportLogService.ReportLogAdd(customDatum);
            DataTableToExcel(dv, topTitle, topTitle, 0);
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
            System.DateTime endDate = Convert.ToDateTime(RadMonthYearPicker1.SelectedDate);
            if (endDate > System.DateTime.Now)
                endDate = System.DateTime.Now;
            string timeRange = string.Format("{0:yyyy-01-01}到{1:yyyy-MM-dd}", endDate, endDate);
            titleText = endDate.Year + "年第" + WeekOfYear(endDate) + "期(" + timeRange + ")";
            ViewState["BeginDT"] = Convert.ToDateTime(endDate.Year + "-01-01 0:00:00");
            ViewState["EndDT"] = endDate.ToString();
            string factorCode = cbxFactor.SelectedValue;
            string factorName = cbxFactor.SelectedItem.Text;
            DateTime beginTime = DateTime.Parse(ViewState["BeginDT"].ToString());
            DateTime endTime = DateTime.Parse(ViewState["EndDT"].ToString());
            //string url = string.Format("ShowVillageWeekReport.aspx?beginTime={0}&endTime={1}&titleText={2}&factorName={3}&factorCode={4}", ViewState["BeginDT"], ViewState["EndDT"], titleText, factorName, factorCode);
            //RunScript("IFrameReHeigth('" + url + "');");
            string column1 = factorName + "浓度";
            string title = "微克/立方米";
            if (factorName == "一氧化碳")
                title = "毫克/立方米";
            //    column1 = factorName + "浓度(μg/m³)(mg/m³)";

            string subColumn1 = beginTime.AddYears(-1).Year + "年";
            string subColumn2 = beginTime.Year + "年";
            string subColumn3 = "与" + beginTime.AddYears(-1).Year + "年比较";
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

            //备注
            Aspose.Cells.Style remarkStyle = workbook.Styles[workbook.Styles.Add()];
            remarkStyle.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
            remarkStyle.Font.Size = 11;
            remarkStyle.Font.Name = "黑体";
            //标题
            Aspose.Cells.Style titleStyle = workbook.Styles[workbook.Styles.Add()];
            titleStyle.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
            titleStyle.Font.Size = 18;
            titleStyle.Font.IsBold = true;
            titleStyle.Font.Name = "宋体";


            #region 表头

            cells[0, 0].PutValue("各市、区PM2.5浓度及比较情况统计表(周报)");
            cells.Merge(0, 0, 1, 11);
            cells[1, 0].PutValue(titleText + "                                                        " + title);
            cells.Merge(1, 0, 1, 11);
            //第一行
            cells[2, 0].PutValue("地区");
            cells.Merge(2, 0, 2, 1);
            cells[2, 1].PutValue(column1);
            cells.Merge(2, 1, 1, 5);
            cells[2, 6].PutValue("达标天数比例");
            cells.Merge(2, 6, 1, 5);

            //第二行
            cells[3, 1].PutValue("同期考核基数");
            cells.Merge(3, 1, 1, 1);
            cells[3, 2].PutValue(subColumn1);
            cells.Merge(3, 2, 1, 1);
            cells[3, 3].PutValue(subColumn2);
            cells.Merge(3, 3, 1, 1);
            cells[3, 4].PutValue("与同期考核基数比较");
            cells.Merge(3, 4, 1, 1);
            cells[3, 5].PutValue(subColumn3);
            cells.Merge(3, 5, 1, 1);
            cells[3, 6].PutValue("同期考核基数");
            cells.Merge(3, 6, 1, 1);
            cells[3, 7].PutValue(subColumn1);
            cells.Merge(3, 7, 1, 1);
            cells[3, 8].PutValue(subColumn2);
            cells.Merge(3, 8, 1, 1);
            cells[3, 9].PutValue("与同期考核基数比较");
            cells.Merge(3, 9, 1, 1);
            cells[3, 10].PutValue(subColumn3);
            cells.Merge(3, 10, 1, 1);

            cells.SetRowHeight(0, 30);//设置行高
            cells.SetRowHeight(1, 20);//设置行高
            cells.SetRowHeight(2, 30);//设置行高
            cells.SetRowHeight(3, 30);//设置行高
            cells.SetColumnWidth(0, 15);//设置列宽
            cells[1, 0].SetStyle(remarkStyle);
            cells[0, 0].SetStyle(titleStyle);

            for (int i = 1; i <= 10; i++)
            {
                cells.SetColumnWidth(i, 12);//设置列宽
            }
            for (int i = 4; i <= 13; i++)
            {
                cells.SetRowHeight(i, 20);//设置列宽
            }

            #endregion


            for (int i = 0; i < dtNew.Rows.Count; i++)
            {
                DataRow drNew = dtNew.Rows[i];
                int rowIndex = i + 4;
                cells[rowIndex, 0].PutValue(drNew["regionName"].ToString());
                cells[rowIndex, 1].PutValue(drNew["baseValue"].ToString());
                cells[rowIndex, 2].PutValue(drNew["perValue"].ToString());
                cells[rowIndex, 3].PutValue(drNew["curValue"].ToString());
                cells[rowIndex, 4].PutValue(drNew["compareBase"].ToString());
                cells[rowIndex, 5].PutValue(drNew["comparePer"].ToString());
                cells[rowIndex, 6].PutValue(drNew["baseRate"].ToString());
                cells[rowIndex, 7].PutValue(drNew["perRate"].ToString());
                cells[rowIndex, 8].PutValue(drNew["curRate"].ToString());
                cells[rowIndex, 9].PutValue(drNew["compareBase2"].ToString());
                cells[rowIndex, 10].PutValue(drNew["comparePer2"].ToString());

                cells[rowIndex, 0].SetStyle(cellStyle);
                cells[rowIndex, 1].SetStyle(cellStyle);
                cells[rowIndex, 2].SetStyle(cellStyle);
                cells[rowIndex, 3].SetStyle(cellStyle);
                cells[rowIndex, 4].SetStyle(cellStyle);
                cells[rowIndex, 5].SetStyle(cellStyle);
                cells[rowIndex, 6].SetStyle(cellStyle);
                cells[rowIndex, 7].SetStyle(cellStyle);
                cells[rowIndex, 8].SetStyle(cellStyle);
                cells[rowIndex, 9].SetStyle(cellStyle);
                cells[rowIndex, 10].SetStyle(cellStyle);

            }
            cells[2, 0].SetStyle(cellStyle);
            cells[2, 1].SetStyle(cellStyle);
            cells[2, 2].SetStyle(cellStyle);
            cells[2, 3].SetStyle(cellStyle);
            cells[2, 4].SetStyle(cellStyle);
            cells[2, 5].SetStyle(cellStyle);
            cells[2, 6].SetStyle(cellStyle);
            cells[2, 7].SetStyle(cellStyle);
            cells[2, 8].SetStyle(cellStyle);
            cells[2, 9].SetStyle(cellStyle);
            cells[2, 10].SetStyle(cellStyle);
            cells[3, 0].SetStyle(cellStyle);
            cells[3, 1].SetStyle(cellStyle);
            cells[3, 2].SetStyle(cellStyle);
            cells[3, 3].SetStyle(cellStyle);
            cells[3, 4].SetStyle(cellStyle);
            cells[3, 5].SetStyle(cellStyle);
            cells[3, 6].SetStyle(cellStyle);
            cells[3, 7].SetStyle(cellStyle);
            cells[3, 8].SetStyle(cellStyle);
            cells[3, 9].SetStyle(cellStyle);
            cells[3, 10].SetStyle(cellStyle);
            //foreach (Cell cell in cells)
            //{
            //    if (!cell.IsStyleSet)
            //    {
            //        cell.SetStyle(cellStyle);
            //    }
            //}
            string FileName = "(" + dt.ToString("yyyyMMddHHmmssffff") + ")" + "VillageWeekReport" + ".xls";
            string strTarget = Server.MapPath("../../../Pages/EnvAir/Report/ReportFile/VillageWeekReport/" + endTime.Year + "/" + endTime.Month + "/" + FileName);
            string path = Server.MapPath("../../../Pages/EnvAir/Report/ReportFile/VillageWeekReport/" + endTime.Year + "/" + endTime.Month + "/");
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

        protected void btnExport_Click(object sender, EventArgs e)
        {
            if (RadMonthYearPicker1.SelectedDate == null)
            {
                ShowMessage("时间不能为空!");
                return;
            }
            if (Convert.ToDateTime(RadMonthYearPicker1.SelectedDate) > DateTime.Now)
            {
                ShowMessage("时间请小于当前时间！");
            }

            string titleText = string.Empty;
            System.DateTime endDate = Convert.ToDateTime(RadMonthYearPicker1.SelectedDate);
            if (endDate > System.DateTime.Now)
                endDate = System.DateTime.Now;
            string timeRange = string.Format("{0:yyyy-01-01}到{1:yyyy-MM-dd}", endDate, endDate);
            titleText = endDate.Year + "年第" + WeekOfYear(endDate) + "期(" + timeRange + ")";
            ViewState["BeginDT"] = Convert.ToDateTime(endDate.Year + "-01-01 0:00:00");
            ViewState["EndDT"] = endDate.ToString();
            string factorCode = cbxFactor.SelectedValue;
            string factorName = cbxFactor.SelectedItem.Text;
            DateTime beginTime = DateTime.Parse(ViewState["BeginDT"].ToString());
            DateTime endTime = DateTime.Parse(ViewState["EndDT"].ToString());
            //string url = string.Format("ShowVillageWeekReport.aspx?beginTime={0}&endTime={1}&titleText={2}&factorName={3}&factorCode={4}", ViewState["BeginDT"], ViewState["EndDT"], titleText, factorName, factorCode);
            //RunScript("IFrameReHeigth('" + url + "');");
            string topTitle = "各市、区" + factorName + "浓度及比较情况统计表(周报)";

            string column1 = factorName + "浓度";
            string subColumn1 = beginTime.AddYears(-1).Year + "年";
            string subColumn2 = beginTime.Year + "年";
            string subColumn3 = "与" + beginTime.AddYears(-1).Year + "年比较";
            string year = YearBegin.SelectedValue;
            string regionName = "市区均值;张家港;常熟;太仓市;昆山市;吴江区";

            string FileName = "(" + dt.ToString("yyyyMMddHHmmssffff") + ")" + "VillageWeekReport" + ".xls";
            var dv = new DataView();
            dv = m_MonthAQIService.GetWeekData(beginTime, endTime, year, factorName, factorCode);
            //添加实体类对象
            ReportLogEntity customDatum = new ReportLogEntity();
            customDatum.PointIds = "";//测点Code
            customDatum.PointNames = regionName;//测点
            customDatum.FactorCodes = "a34004";//因子Code
            customDatum.FactorsNames = "PM2.5";//因子名称
            customDatum.DateTimeRange = string.Format("{0:yyyy年MM月dd日}~{1:yyyy年MM月dd日}", beginTime, endDate);
            customDatum.WaterOrAirType = 1;//水或气 0：水，1：气
            customDatum.PageTypeID = "VillageWeekReport";//页面ID
            customDatum.StartDateTime = beginTime;
            customDatum.EndDateTime = endDate;

            customDatum.CreatUser = this.ViewState["DisplayPerson"].ToString();
            customDatum.ExportName = "各市、区PM2.5浓度及比较情况统计表周报(" + string.Format("{0:yyyyMMdd}-{1:yyyyMMdd}", beginTime, endDate)+")";
            customDatum.ReportName = ("../../../Pages/EnvAir/Report/ReportFile/VillageWeekReport/" + endDate.Year + "/" + endDate.Month + "/" + FileName).ToString();
            customDatum.CreatDateTime = DateTime.Now;
            //添加数据
            ReportLogService.ReportLogAdd(customDatum);
            DataTableToExcel(dv, topTitle, topTitle, 0);
        }


    }
}