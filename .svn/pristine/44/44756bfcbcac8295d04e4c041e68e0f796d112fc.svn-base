using Aspose.Cells;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.Service.DataAnalyze.Air.AQIReport;
using SmartEP.Service.DataAnalyze.Report;
using SmartEP.Utilities.Web.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.Report
{
    public partial class MonthReportForcast : SmartEP.WebUI.Common.BasePage
    {
        /// 数据处理服务
        /// </summary>
        private MonthAQIService m_MonthAQIService = new MonthAQIService();
        ReportLogService ReportLogService = new ReportLogService();

        DateTime dt = DateTime.Now;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.ViewState["DisplayPerson"] = PageHelper.GetQueryString("DisplayPerson");
                InitControl();
            }
        }

        #region 页面控件初始化
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            for (int i = 0; i < comboCityProper.Items.Count; i++)
            {
                comboCityProper.Items[i].Checked = true;
            }
            dtpBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-01"));
            dtpEnd.SelectedDate = DateTime.Now;
            DateTime dtBegin = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-01"));  //本月第一天
            DateTime dtEnd = Convert.ToDateTime(dtpEnd.SelectedDate.Value.ToString("yyyy-MM-dd"));   //本月当天
            txtDateF.Text = dtBegin.ToString("yyyy年MM月dd日");
            txtDateT.Text = dtEnd.ToString("yyyy年MM月dd日");
        }
        #endregion


        #region 服务器端控件事件处理
        /// <summary>
        /// 下载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExport_Click(object sender, EventArgs e)
        {
            if (dtpBegin.SelectedDate == null || dtpEnd.SelectedDate == null)
            {
                Alert("开始时间或者终止时间，不能为空！");
                return;
            }
            else if (dtpBegin.SelectedDate > dtpEnd.SelectedDate)
            {
                Alert("开始时间不能大于终止时间！");
                return;
            }
            DateTime imBegion = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-01"));   //本期第一天
            DateTime imEnd = Convert.ToDateTime(dtpEnd.SelectedDate.Value.ToString("yyyy-MM-dd"));   //本期当天

            string regionGuid = "";
            foreach (RadComboBoxItem item in comboCityProper.CheckedItems)
            {
                regionGuid += (item.Value.ToString() + ";");
            }
            string[] regionGuids = regionGuid.Trim(';').Split(';');
            string regionName = "";
            foreach (RadComboBoxItem item in comboCityProper.CheckedItems)
            {
                regionName += (item.Text.ToString() + ";");  //区域名称
            }
            int pageSize = 99999;  //每页显示数据个数  
            int pageNo = 0;   //当前页的序号
            int recordTotal = 0;  //数据总行数
            var dv = new DataView();
            dv = m_MonthAQIService.GetAreaDataPager(regionGuids, imBegion, imEnd, pageSize, pageNo, out recordTotal);

            string FileName = "(" + dt.ToString("yyyyMMddHHmmssffff") + ")" + "MonthReportForcast" + ".xls";
            //添加实体类对象
            ReportLogEntity customDatum = new ReportLogEntity();
            customDatum.PointIds = regionGuid;//测点Code
            customDatum.PointNames = regionName;//测点名称
            customDatum.FactorCodes = "a34004";//因子Code
            customDatum.FactorsNames = "PM2.5";//因子名称
            customDatum.DateTimeRange = txtDateF.Text + "~" + txtDateT.Text;
            customDatum.WaterOrAirType = 1;//水或气 0：水，1：气
            customDatum.PageTypeID = "MonthReportForcast";//页面ID
            customDatum.StartDateTime = Convert.ToDateTime(txtDateF.Text);
            customDatum.EndDateTime = Convert.ToDateTime(txtDateT.Text);
            customDatum.CreatUser = this.ViewState["DisplayPerson"].ToString();
            customDatum.ReportName = ("../../../Pages/EnvAir/Report/ReportFile/MonthReportForcast/" + dtpEnd.SelectedDate.Value.Year + "/" + dtpEnd.SelectedDate.Value.Month + "/" + FileName).ToString();
            customDatum.CreatDateTime = DateTime.Now;
            //添加数据
            ReportLogService.ReportLogAdd(customDatum);
            DataTableToExcel(dv, "大市月报", "大市月报", 1);
        }

        /// <summary>
        /// 导出空气质量实时报
        /// </summary>
        /// <param name="dv">原始数据表</param>
        /// <returns></returns>
        private void DataTableToExcel(DataView dv, string fileName, string sheetName, int m)
        {
            DataTable dtNew = dv.ToTable();
            DateTime imBegion = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-01"));   //本期第一天
            DateTime imEnd = Convert.ToDateTime(txtDateT.Text);   //本期当天
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
            //第一行
            cells[0, 0].PutValue("地区");
            cells.Merge(0, 0, 3, 1);
            cells[0, 1].PutValue("PM2.5浓度(μg/m³)");
            cells.Merge(0, 1, 1, 3);
            cells[0, 4].PutValue("达标天数比例");
            cells.Merge(0, 4, 1, 3);


            //第二行
            cells[1, 1].PutValue(imBegion.AddYears(-1).Year.ToString() + "年");
            cells.Merge(1, 1, 1, 1);
            cells[1, 2].PutValue(imBegion.Year.ToString() + "年");
            cells.Merge(1, 2, 1, 1);
            cells[1, 3].PutValue("与2013年" + imBegion.Month.ToString() + "月~" + imEnd.Month.ToString() + "月");
            cells.Merge(1, 3, 1, 1);
            cells[1, 4].PutValue("2013年");
            cells.Merge(1, 4, 1, 1);
            cells[1, 5].PutValue(imBegion.Year.ToString() + "年");
            cells.Merge(1, 5, 1, 1);
            cells[1, 6].PutValue("与"+imBegion.AddYears(-1).Year.ToString() +"年" + imBegion.Month.ToString() + "月~" + imEnd.Month.ToString() + "月");
            cells.Merge(1, 6, 1, 1);

            //第三行
            cells[2, 1].PutValue(imBegion.Month.ToString() + "~" + imEnd.Month.ToString() + "月");
            cells[2, 2].PutValue(imBegion.Month.ToString() + "~" + imEnd.Month.ToString() + "月");
            cells[2, 3].PutValue("同期比较");
            cells[2, 4].PutValue(imBegion.Month.ToString() + "~" + imEnd.Month.ToString() + "月");
            cells[2, 5].PutValue(imBegion.Month.ToString() + "~" + imEnd.Month.ToString() + "月");
            cells[2, 6].PutValue("同期比较(百分点)");
            cells.SetRowHeight(0, 15);//设置行高
            cells.SetRowHeight(1, 30);//设置行高
            cells.SetRowHeight(2, 30);//设置行高
            cells.SetColumnWidth(0, 15);//设置列宽
            for (int i = 1; i <= 6; i++)
            {
                cells.SetColumnWidth(i, 12);//设置列宽
            }
            #endregion


            for (int i = 0; i < dtNew.Rows.Count; i++)
            {
                DataRow drNew = dtNew.Rows[i];
                int rowIndex = i + 3;
                cells[rowIndex, 0].PutValue(drNew["regionName"].ToString());
                cells[rowIndex, 1].PutValue(drNew["2014_PM25"].ToString());
                cells[rowIndex, 2].PutValue(drNew["2015_PM25"].ToString());
                cells[rowIndex, 3].PutValue(drNew["2013_PM25"].ToString());
                cells[rowIndex, 4].PutValue(drNew["2013_Db"].ToString());
                cells[rowIndex, 5].PutValue(drNew["2015_Db"].ToString());
                cells[rowIndex, 6].PutValue(drNew["2014_Db"].ToString());
            }
            foreach (Cell cell in cells)
            {
                if (!cell.IsStyleSet)
                {
                    cell.SetStyle(cellStyle);
                }
            }
            string FileName = "(" + dt.ToString("yyyyMMddHHmmssffff") + ")" + "MonthReportForcast" + ".xls";
            string strTarget = Server.MapPath("../../../Pages/EnvAir/Report/ReportFile/MonthReportForcast/" + dtpEnd.SelectedDate.Value.Year + "/" + dtpEnd.SelectedDate.Value.Month + "/" + FileName);
            string path = Server.MapPath("../../../Pages/EnvAir/Report/ReportFile/MonthReportForcast/" + dtpEnd.SelectedDate.Value.Year + "/" + dtpEnd.SelectedDate.Value.Month + "/");
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
        #endregion

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            if (dtpBegin.SelectedDate == null || dtpEnd.SelectedDate == null)
            {
                Alert("开始时间或者终止时间，不能为空！");
                return;
            }
            else if (dtpBegin.SelectedDate > dtpEnd.SelectedDate)
            {
                Alert("开始时间不能大于终止时间！");
                return;
            }
            DateTime dtBegin = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-01"));  //本月第一天
            DateTime dtEnd = Convert.ToDateTime(txtDateT.Text);   //本月当天

            string regionGuid = "";
            foreach (RadComboBoxItem item in comboCityProper.CheckedItems)
            {
                regionGuid += (item.Value.ToString() + ";");
            }
            string[] regionGuids = regionGuid.Trim(';').Split(';');
            string regionName = "";
            foreach (RadComboBoxItem item in comboCityProper.CheckedItems)
            {
                regionName += (item.Text.ToString() + ";");  //区域名称
            }
            int pageSize = 99999;  //每页显示数据个数  
            int pageNo = 0;   //当前页的序号
            int recordTotal = 0;  //数据总行数
            string FileName = "(" + dt.ToString("yyyyMMddHHmmssffff") + ")" + "MonthReportForcast" + ".xls";
            var dv = new DataView();
            dv = m_MonthAQIService.GetAreaDataPager(regionGuids, dtBegin, dtEnd, pageSize, pageNo, out recordTotal);
            //添加实体类对象
            ReportLogEntity customDatum = new ReportLogEntity();
            customDatum.PointIds = regionGuid;//测点Code
            customDatum.PointNames = regionName;//测点
            customDatum.FactorCodes = "a34004";//因子Code
            customDatum.FactorsNames = "PM2.5";//因子名称
            customDatum.DateTimeRange = txtDateF.Text + "~" + txtDateT.Text;
            customDatum.WaterOrAirType = 1;//水或气 0：水，1：气
            customDatum.PageTypeID = "MonthReportForcast";//页面ID
            customDatum.StartDateTime = Convert.ToDateTime(txtDateF.Text);
            customDatum.EndDateTime = Convert.ToDateTime(txtDateT.Text);
            customDatum.CreatUser = this.ViewState["DisplayPerson"].ToString();
            customDatum.ReportName = ("../../../Pages/EnvAir/Report/ReportFile/MonthReportForcast/" + dtpEnd.SelectedDate.Value.Year + "/" + dtpEnd.SelectedDate.Value.Month + "/" + FileName).ToString();
            customDatum.CreatDateTime = DateTime.Now;
            //添加数据
            ReportLogService.ReportLogAdd(customDatum);
            DataTableToExcel(dv, "大市月报", "大市月报", 0);
        }

        protected void dtpBegin_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            int yearF = dtpBegin.SelectedDate.Value.Year;
            int yearT = dtpEnd.SelectedDate.Value.Year;
            if (yearF != yearT)
            {
                int sub = yearF - yearT;
                dtpEnd.SelectedDate = dtpEnd.SelectedDate.Value.AddYears(+sub);
            }
            if (dtpBegin.SelectedDate.ToString() != "")
            {
                DateTime dtBegin = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-01"));  //本月第一天
                DateTime dtEnd = Convert.ToDateTime(dtpEnd.SelectedDate.Value.Date.ToString("yyyy-MM-dd"));   //本月当天
                txtDateF.Text = dtBegin.ToString("yyyy年MM月dd日");
                txtDateT.Text = dtEnd.ToString("yyyy年MM月dd日");
            }
            else
            {
                Alert("时间不能为空");
                return;
            }
        }

        protected void dtpEnd_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            int yearF = dtpBegin.SelectedDate.Value.Year;
            int yearT = dtpEnd.SelectedDate.Value.Year;
            if (yearF != yearT)
            {
                int sub = yearT - yearF;
                dtpBegin.SelectedDate = dtpBegin.SelectedDate.Value.AddYears(+sub);
            }
            if (dtpBegin.SelectedDate.ToString() != "")
            {
                DateTime dtBegin = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-01"));  //本月第一天
                DateTime dtEnd = DateTime.Now;
                switch (dtpEnd.SelectedDate.Value.Month)
                {
                    case 1:
                    case 3:
                    case 5:
                    case 7:
                    case 8:
                    case 10:
                    case 12:
                        dtEnd = Convert.ToDateTime(dtpEnd.SelectedDate.Value.ToString("yyyy-MM-31"));   //本月当天
                        break;
                    case 4:
                    case 6:
                    case 9:
                    case 11:
                        dtEnd = Convert.ToDateTime(dtpEnd.SelectedDate.Value.ToString("yyyy-MM-30"));   //本月当天
                        break;
                    case 2:
                        if (((dtpEnd.SelectedDate.Value.Year % 4) == 0 && (dtpEnd.SelectedDate.Value.Year % 100) != 0) || (dtpEnd.SelectedDate.Value.Year % 400) == 0)
                        {
                            dtEnd = Convert.ToDateTime(dtpEnd.SelectedDate.Value.ToString("yyyy-MM-29"));   //本月当天
                        }
                        else
                            dtEnd = Convert.ToDateTime(dtpEnd.SelectedDate.Value.ToString("yyyy-MM-28"));   //本月当天
                        break;
                }

                txtDateF.Text = dtBegin.ToString("yyyy年MM月dd日");
                txtDateT.Text = dtEnd.ToString("yyyy年MM月dd日");
            }
            else
            {
                Alert("时间不能为空");
                return;
            }
        }
    }
}