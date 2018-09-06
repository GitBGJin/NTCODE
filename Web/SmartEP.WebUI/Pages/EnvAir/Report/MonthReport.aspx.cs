using Aspose.Words;
using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.Data.SqlServer.BaseData;
using SmartEP.DomainModel.BaseData;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.DataAnalyze.Air.AQIReport;
using SmartEP.Service.DataAnalyze.Report;
using SmartEP.Utilities.Office;
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
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using Aspose.Words.Tables;

namespace SmartEP.WebUI.Pages.EnvAir.Report
{
    public partial class MonthReport : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        private MonthAQIService m_DayAQIService;
        ReportLogService ReportLogService = new ReportLogService();
        /// <summary>
        /// 选择站点
        /// </summary>
        private IList<IPoint> points = null;
        DateTime dt = DateTime.Now;
        protected void Page_Load(object sender, EventArgs e)
        {
            m_DayAQIService = new MonthAQIService();
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
            //国控点，对照点，路边站
            //MonitoringPointAirService m_MonitoringPointAirService = Singleton<MonitoringPointAirService>.GetInstance();
            //string strpointName = "";
            //IQueryable<MonitoringPointEntity> EnableOrNotports = m_MonitoringPointAirService.RetrieveAirMPListByEnable();
            //string[] EnableOrNotportsarry = EnableOrNotports.Where(p => p.ContrlUid == "6fadff52-2338-4319-9f1d-7317823770ad").Select(p => p.MonitoringPointName).ToArray();
            //foreach (string point in EnableOrNotportsarry)
            //{
            //    if (point != "上方山")
            //        strpointName += point + ";";
            //}
            //pointCbxRsm.SetPointValuesFromNames(strpointName);

            dtpBegin.SelectedDate = DateTime.Now;
            DateTime dtBegin = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-01"));  //本月第一天
            hdPDate.Value = dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd");   //本月当天
        }
        #endregion


        #region 服务器端控件事件处理
        /// <summary>
        /// ToolBar事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataTable dtnew = new DataTable();

            if (dtpBegin.SelectedDate == null)
            {
                return;
            }
            DateTime mBegion = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd")).FirstDayOfMonth();
            string titleText = string.Empty;
            System.DateTime mEnd = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd")).LastDayOfMonth();
            if (mEnd > System.DateTime.Now)
                mEnd = System.DateTime.Now;

            //points = pointCbxRsm.GetPoints();

            var dataViewM = new DataView();

            dataViewM = m_DayAQIService.GetDataMonthPager(mBegion, mEnd);
            dtnew = dataViewM.ToTable();

            DateTime dtStart = Convert.ToDateTime(dtpBegin.SelectedDate + "-1");
            Document doc = new Document(System.IO.Path.Combine(MapPath("DocumentsTemplet"), "MonthReport.doc"));
            DocumentBuilder builder = new DocumentBuilder(doc);
            builder.MoveToMergeField("TimeReport");

            builder.Writeln(dtStart.ToString("yyyy") + "年" + dtStart.Month.ToString() + "月");

            builder.MoveToMergeField("DateReport");

            builder.Write(DateTime.Now.ToString("yyyy") + "年" + DateTime.Now.Month.ToString() + "月" + DateTime.Now.Day.ToString() + "日");

            if (dtnew.Rows.Count > 0)
            {
                builder.MoveToMergeField("M1");
                builder.Write(dtnew.Rows[0][0].ToString());

                builder.MoveToMergeField("M2");
                builder.Write(dtnew.Rows[0][1].ToString());
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.MoveToMergeField("M3");
                builder.Write(dtnew.Rows[0][2].ToString());
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.MoveToMergeField("M4");
                builder.Write(dtnew.Rows[0][3].ToString());
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.MoveToMergeField("M5");
                builder.Write(dtnew.Rows[0][4].ToString());
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.MoveToMergeField("M6");
                builder.Write(dtnew.Rows[0][5].ToString());
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.MoveToMergeField("M7");
                builder.Write(dtnew.Rows[0][6].ToString());
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.MoveToMergeField("M8");
                builder.Write(dtnew.Rows[0][7].ToString());
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.MoveToMergeField("M9");
                builder.Write(dtnew.Rows[0][8].ToString());
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.MoveToMergeField("Ma");
                builder.Write(dtnew.Rows[0][9].ToString());
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.MoveToMergeField("Mb");
                builder.Write(dtnew.Rows[0][10].ToString());
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.MoveToMergeField("Mc");
                builder.Write(dtnew.Rows[0][11].ToString());
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                string day = "共有" + dtnew.Rows[0]["Str"].ToString() + "天数据无效。";
                builder.MoveToMergeField("Day");
                builder.Write(day);
                doc.MailMerge.DeleteFields();

                //string[] strPointCodes = pointCbxRsm.GetPoints().Select(t => t.PointID).ToArray();//站点Code
                //string pointCodes = string.Join(";", strPointCodes);
                //string[] strPointNames = pointCbxRsm.GetPoints().Select(t => t.PointName).ToArray();//站点名称
                //string pointNames = string.Join(";", strPointNames);
                //添加实体类对象
                string filename = "(" + dt.ToString("yyyyMMddHHmmssffff") + ")" + "MonthReport" + ".doc";
                ReportLogEntity customDatum = new ReportLogEntity();
                customDatum.PointIds = "7e05b94c-bbd4-45c3-919c-42da2e63fd43";//测点Code
                customDatum.PointNames = "苏州市区";//测点名称
                customDatum.FactorCodes = "";//因子Code
                customDatum.FactorsNames = "";//因子名称
                customDatum.DateTimeRange = dtpBegin.SelectedDate.Value.Year.ToString() + "年" + dtpBegin.SelectedDate.Value.Month.ToString() + "月";
                customDatum.WaterOrAirType = 1;//水或气 0：水，1：气
                customDatum.PageTypeID = "MonthReport";//页面ID
                customDatum.StartDateTime = mBegion;
                customDatum.EndDateTime = mEnd;
                customDatum.CreatUser = this.ViewState["DisplayPerson"].ToString();
                customDatum.ExportName = "苏州市区环境空气质量月报表" + string.Format("{0:yyyyMM}", dtpBegin.SelectedDate.Value);
                customDatum.ReportName = ("../../../Pages/EnvAir/Report/ReportFile/MonthReport/" + dtpBegin.SelectedDate.Value.Year + "/" + dtpBegin.SelectedDate.Value.Month + "/" + filename).ToString();
                customDatum.CreatDateTime = DateTime.Now;
                //添加数据
                ReportLogService.ReportLogAdd(customDatum);


                string strTarget = Server.MapPath("../../../Pages/EnvAir/Report/ReportFile/MonthReport/" + dtpBegin.SelectedDate.Value.Year + "/" + dtpBegin.SelectedDate.Value.Month + "/" + filename);
                doc.Save(strTarget);
                doc.Save(this.Response, "MonthReport.doc", Aspose.Words.ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(Aspose.Words.SaveFormat.Docx));
                Response.End();
            }
        }
        #endregion

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            if (dtpBegin.SelectedDate == null)
            {
                return;
            }
            DataTable dtnew = new DataTable();

            if (dtpBegin.SelectedDate == null)
            {
                return;
            }

            DateTime mBegion = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd")).FirstDayOfMonth();
            string titleText = string.Empty;
            System.DateTime mEnd = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd")).LastDayOfMonth();
            if (mEnd > System.DateTime.Now)
                mEnd = System.DateTime.Now;

            //points = pointCbxRsm.GetPoints();
            var dataViewM = new DataView();

            dataViewM = m_DayAQIService.GetDataMonthPager(mBegion, mEnd);
            dtnew = dataViewM.ToTable();

            DateTime dtStart = Convert.ToDateTime(dtpBegin.SelectedDate + "-1");
            Document doc = new Document(System.IO.Path.Combine(MapPath("DocumentsTemplet"), "MonthReport.doc"));
            DocumentBuilder builder = new DocumentBuilder(doc);
            builder.MoveToMergeField("TimeReport");

            builder.Writeln(dtStart.ToString("yyyy") + "年" + dtStart.Month.ToString() + "月");

            builder.MoveToMergeField("DateReport");

            builder.Write(DateTime.Now.ToString("yyyy") + "年" + DateTime.Now.Month.ToString() + "月" + DateTime.Now.Day.ToString() + "日");

            builder.MoveToMergeField("M1");

            if (dtnew.Rows.Count > 0)
            {
                builder.Write(dtnew.Rows[0][0].ToString());
                builder.MoveToMergeField("M2");
                builder.Write(dtnew.Rows[0][1].ToString());
                builder.MoveToMergeField("M3");
                builder.Write(dtnew.Rows[0][2].ToString());
                builder.MoveToMergeField("M4");
                builder.Write(dtnew.Rows[0][3].ToString());
                builder.MoveToMergeField("M5");
                builder.Write(dtnew.Rows[0][4].ToString());
                builder.MoveToMergeField("M6");
                builder.Write(dtnew.Rows[0][5].ToString());
                builder.MoveToMergeField("M7");
                builder.Write(dtnew.Rows[0][6].ToString());
                builder.MoveToMergeField("M8");
                builder.Write(dtnew.Rows[0][7].ToString());
                builder.MoveToMergeField("M9");
                builder.Write(dtnew.Rows[0][8].ToString());
                builder.MoveToMergeField("Ma");
                builder.Write(dtnew.Rows[0][9].ToString());
                builder.MoveToMergeField("Mb");
                builder.Write(dtnew.Rows[0][10].ToString());
                builder.MoveToMergeField("Mc");
                builder.Write(dtnew.Rows[0][11].ToString());
                string day = "共有" + dtnew.Rows[0]["Str"].ToString() + "天数据无效。";
                builder.MoveToMergeField("Day");
                builder.Write(day);
                doc.MailMerge.DeleteFields();

                //string[] strPointCodes = pointCbxRsm.GetPoints().Select(t => t.PointID).ToArray();//站点Code
                //string pointCodes = string.Join(";", strPointCodes);
                //string[] strPointNames = pointCbxRsm.GetPoints().Select(t => t.PointName).ToArray();//站点名称
                //string pointNames = string.Join(";", strPointNames);
                string filename = "(" + dt.ToString("yyyyMMddHHmmssffff") + ")" + "MonthReport" + ".doc";
                //添加实体类对象
                ReportLogEntity customDatum = new ReportLogEntity();
                customDatum.PointIds = "7e05b94c-bbd4-45c3-919c-42da2e63fd43";//测点Code
                customDatum.PointNames = "苏州市区";//测点名称
                customDatum.FactorCodes = "a21026;a21004;a34002;a34004;a21005;a05024";//因子Code
                customDatum.FactorsNames = "二氧化硫;二氧化氮;PM10;PM2.5;一氧化碳;臭氧";//因子名称
                customDatum.DateTimeRange = dtpBegin.SelectedDate.Value.Year.ToString() + "年" + dtpBegin.SelectedDate.Value.Month.ToString() + "月";
                customDatum.WaterOrAirType = 1;//水或气 0：水，1：气
                customDatum.PageTypeID = "MonthReport";//页面ID
                customDatum.StartDateTime = mBegion;
                customDatum.EndDateTime = mEnd;
                customDatum.CreatUser = this.ViewState["DisplayPerson"].ToString();
                customDatum.ExportName = "苏州市区环境空气质量月报表" + string.Format("{0:yyyyMM}", dtpBegin.SelectedDate.Value);
                customDatum.ReportName = ("../../../Pages/EnvAir/Report/ReportFile/MonthReport/" + dtpBegin.SelectedDate.Value.Year + "/" + dtpBegin.SelectedDate.Value.Month + "/" + filename).ToString();
                customDatum.CreatDateTime = DateTime.Now;
                //添加数据
                ReportLogService.ReportLogAdd(customDatum);

                string strTarget = Server.MapPath("../../../Pages/EnvAir/Report/ReportFile/MonthReport/" + dtpBegin.SelectedDate.Value.Year + "/" + dtpBegin.SelectedDate.Value.Month + "/" + filename);
                doc.Save(strTarget);
                //Response.End();
                if (!Directory.Exists(strTarget))
                {
                    Alert("保存成功！");
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "reload", "<script type=\"text/javascript\">self.parent.location.reload();</script>", false);

            }

        }
    }
}