using Aspose.Words;
using SmartEP.DomainModel.BaseData;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.Service.BaseData.Channel;
using SmartEP.Service.DataAnalyze.Air.AQIReport;
using SmartEP.Service.DataAnalyze.Report;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Reporting;
using Telerik.Web.UI;
using SmartEP.Service.ReportLibrary.Air;
using SmartEP.Utilities.Web.UI;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Core.Generic;
using System.Web.UI.HtmlControls;

namespace SmartEP.WebUI.Pages.EnvAir.Report
{
    public partial class AQI_SECDayReportNew : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        private DayAQIService m_DayAQIService = new DayAQIService();
        ReportLogService ReportLogService = new ReportLogService();
        AirPollutantService m_AirPollutantService = new AirPollutantService();
        MonitoringPointAirService MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();

        DateTime CurrentDateTime = System.DateTime.Now;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.ViewState["DisplayPerson"] = PageHelper.GetQueryString("DisplayPerson");
                ReportDate.SelectedDate = CurrentDateTime.AddDays(-1).Date;
                //取得国控点
                IQueryable<MonitoringPointEntity> ports = MonitoringPointAir.RetrieveAirMPListByCountryControlled();
                for (int i = 1; i < 9; i++)
                {
                    RadComboBox PointName = this.FindControl("txtPointName" + i) as RadComboBox;
                    PointName.DataValueField = "PointId";
                    PointName.DataTextField = "MonitoringPointName";
                    PointName.DataSource = ports;
                    PointName.DataBind();
                }
                InitControl();
            }
        }

        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            tdReportDate.InnerText = CurrentDateTime.ToString("yyyy年M月d日");
            tdWeek.InnerText = "星期" + GetWeek(CurrentDateTime).ToString();
            txtTime.Value = CurrentDateTime.ToString("HH:mm:ss");
            DataTable dt = GetReportYesterday();
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                AQIValue.InnerText = dr["AQIValue"] != DBNull.Value ? dr["AQIValue"].ToString() : "--";
                if (dr["PrimaryPollutant"] != DBNull.Value)
                {
                    string[] strPP = dr["PrimaryPollutant"].ToString().Split(',');
                    string strPE = "";
                    string strPC = "";
                    string strInner = "";
                    if (strPP.Length != 1)
                    {
                        for (int i = 0; i < strPP.Length; i++)
                        {
                            strPE = strPP[i].Trim();
                            switch (strPE)
                            {
                                case "SO2": strPC = "二氧化硫"; break;
                                case "NO2": strPC = "二氧化氮"; break;
                                case "PM10": strPC = "可吸入颗粒物"; break;
                                case "PM2.5": strPC = "细颗粒物"; break;
                                case "CO": strPC = "一氧化碳"; break;
                                case "O3": strPC = "臭氧8小时"; strPE = "O3-8h"; break;
                            }

                            strInner += strPC + "(" + strPE + ")</br>";
                        }
                    }
                    else
                    {
                        strPE = strPP[0].Trim();
                        switch (strPE)
                        {
                            case "SO2": strPC = "二氧化硫"; break;
                            case "NO2": strPC = "二氧化氮"; break;
                            case "PM10": strPC = "可吸入颗粒物"; break;
                            case "PM2.5": strPC = "细颗粒物"; break;
                            case "CO": strPC = "一氧化碳"; break;
                            case "O3": strPC = "臭氧8小时"; strPE = "O3-8h"; break;
                        }
                        if (strPE != "--")
                            strInner = strPC + "</br>(" + strPE + ")";
                        else
                            strInner = strPE;
                    }
                    PrimaryPollutant.InnerHtml = strInner;
                }
                else
                    PrimaryPollutant.InnerHtml = "--";
                QualityClass.InnerText = dr["Class"] != DBNull.Value ? dr["Class"].ToString() : "--";
                SO2.InnerText = dr["SO2"] != DBNull.Value ? (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["SO2_IAQI"]), 0)).ToString() : "--";
                NO2.InnerText = dr["NO2"] != DBNull.Value ? (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["NO2_IAQI"]), 0)).ToString() : "--";
                PM10.InnerText = dr["PM10"] != DBNull.Value ? (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["PM10_IAQI"]), 0)).ToString() : "--";
                CO.InnerText = dr["CO"] != DBNull.Value ? (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["CO_IAQI"]), 0)).ToString() : "--";
                O38h.InnerText = dr["Max8HourO3"] != DBNull.Value ? (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["Max8HourO3_IAQI"]), 0)).ToString() : "--";
                PM25.InnerText = dr["PM25"] != DBNull.Value ? (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["PM25_IAQI"]), 0)).ToString() : "--";
            }
            else
            {
                AQIValue.InnerText = "--";
                PrimaryPollutant.InnerText = "--";
                QualityClass.InnerText = "--";
                SO2.InnerText = "--";
                NO2.InnerText = "--";
                PM10.InnerText = "--";
                CO.InnerText = "--";
                O38h.InnerText = "--";
                PM25.InnerText = "--";
            }
        }

        /// <summary>
        /// 初始化子站控件
        /// </summary>
        private void InitPointsCombo()
        {
            string ControlUid = "6fadff52-2338-4319-9f1d-7317823770ad";
            MonitoringPointAirService s = Singleton<MonitoringPointAirService>.GetInstance();
            IQueryable<MonitoringPointEntity> arryEntity = s.RetrieveListByControlUid(ControlUid);
            int[] pointArry = arryEntity.Select(p => p.PointId).ToArray();
            for (int i = 1; i < 9; i++)
            {

                RadComboBox PointName = this.FindControl("txtPointName" + i) as RadComboBox;
                PointName.Items.Clear();
                for (int j = 0; j < pointArry.Length; j++)
                {
                    RadComboBoxItem item = new RadComboBoxItem();
                    item.Value = pointArry[j].ToString();
                    item.Text = MonitoringPointAir.RetrieveEntityByPointId(pointArry[j]).MonitoringPointName;
                    PointName.Items.Add(item);
                }
            }
        }

        protected string GetWeek(DateTime dt)
        {
            int i = (int)dt.DayOfWeek;
            string[] WeekDays = { "日", "一", "二", "三", "四", "五", "六" };
            return WeekDays[i];
        }

        /// <summary>
        /// 保存日报
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            Document doc = new Document(System.IO.Path.Combine(MapPath("DocumentsTemplet"), "AQI-SECDayReportTemplete.doc"));
            DocumentBuilder builder = new DocumentBuilder(doc);
            builder.MoveToMergeField("ReportDate");
            builder.Write(CurrentDateTime.ToString("yyyy年M月d日"));
            builder.MoveToMergeField("TheWeek");
            builder.Write(GetWeek(CurrentDateTime));
            builder.MoveToMergeField("ReportTime");
            builder.Write(CurrentDateTime.ToString("HH:mm:ss"));
            builder.MoveToMergeField("Temp");
            builder.Write(txtTemp.Value);
            builder.MoveToMergeField("Weather");
            //builder.Write(cboWeather.SelectedItem.Text);
            builder.Write(cboWeather.Value);
            builder.MoveToMergeField("Author");
            if (txtAuthor.Value == "")
                builder.Write("       ");
            else
                builder.Write(txtAuthor.Value);
            builder.MoveToMergeField("Auditor");
            if (txtAuditor.Value == "")
                builder.Write("       ");
            else
                builder.Write(txtAuditor.Value);
            builder.MoveToMergeField("Singer");
            if (txtSinger.Value == "")
                builder.Write("        ");
            else
                builder.Write(txtSinger.Value);
            ExportYesterdayData(builder);
            string[,] Ids = new string[,] { { "4", "cboSendResult" }, { "4", "cboSendTime" }, { "8", "txtPointName" }, { "8", "cboExecPara" }, { "8", "cboEliminateTime" }, { "8", "cboEliminateRemark" }, { "12", "taRemark" } };
            for (int i = 0; i < Ids.GetLength(0); i++)
            {
                ExportReportData(builder, Ids[i, 1], Convert.ToInt16(Ids[i, 0]));
            }

            doc.MailMerge.DeleteFields();
            save(doc);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "reload", "<script type=\"text/javascript\">self.parent.location.reload();</script>", false);
        }

        /// <summary>
        /// 保存记录
        /// </summary>
        protected void save(Document doc)
        {
            string filename = "(" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ")" + "AQI-SECDayReport" + ".doc";
            string strTarget = Server.MapPath("../../../Pages/EnvAir/Report/ReportFile/AQI-SECDayReport/" + ReportDate.SelectedDate.Value.Year + "/" + ReportDate.SelectedDate.Value.Month + "/" + filename);
            //添加实体类对象
            ReportLogEntity customDatum = new ReportLogEntity();
            customDatum.PointNames = "苏州市区";//测点名称
            customDatum.FactorsNames = "AQI;首要污染物;空气质量类别;SO2;NO2;PM10;CO;O3-8h;PM2.5";
            customDatum.DateTimeRange = ReportDate.SelectedDate.Value.ToString("yyyy年MM月dd日");
            customDatum.WaterOrAirType = 1;//水或气 0：水，1：气
            customDatum.PageTypeID = "AQI-SECDayReportNew";//页面ID
            customDatum.StartDateTime = ReportDate.SelectedDate.Value;
            customDatum.EndDateTime = ReportDate.SelectedDate.Value;
            customDatum.CreatUser = this.ViewState["DisplayPerson"].ToString();
            customDatum.ReportName = ("../../../Pages/EnvAir/Report/ReportFile/AQI-SECDayReport/" + ReportDate.SelectedDate.Value.Year + "/" + ReportDate.SelectedDate.Value.Month + "/" + filename).ToString();
            customDatum.CreatDateTime = DateTime.Now;
            customDatum.ExportName = "AQI-SEC日报" + CurrentDateTime.AddDays(-1).ToString("yyyy年M月d日");
            //添加数据
            ReportLogService.ReportLogAdd(customDatum);
            doc.Save(strTarget);
            if (!Directory.Exists(strTarget))
            {
                Alert("保存成功！");
            }
        }

        /// <summary>
        /// 导出昨日数据
        /// </summary>
        /// <param name="builder"></param>
        private void ExportYesterdayData(DocumentBuilder builder)
        {
            DataTable dt = GetReportYesterday();
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                builder.MoveToMergeField("AQI");
                builder.Write(dr["AQIValue"] != DBNull.Value ? dr["AQIValue"].ToString() : "--");
                builder.MoveToMergeField("PrimaryPollutant");
                string strInner = "";
                if (dr["PrimaryPollutant"] != DBNull.Value)
                {
                    string[] strPP = dr["PrimaryPollutant"].ToString().Split(',');
                    string strPE = "";
                    string strPC = "";
                    if (strPP.Length != 1)
                    {
                        for (int i = 0; i < strPP.Length; i++)
                        {
                            strPE = strPP[i].Trim();
                            switch (strPE)
                            {
                                case "SO2": strPC = "二氧化硫"; strPE = "SO<SUB>2</SUB>"; break;
                                case "NO2": strPC = "二氧化氮"; strPE = "NO<SUB>2</SUB>"; break;
                                case "PM10": strPC = "可吸入颗粒物"; strPE = "PM<SUB>10</SUB>"; break;
                                case "PM2.5": strPC = "细颗粒物"; strPE = "PM<SUB>2.5</SUB>"; break;
                                case "CO": strPC = "一氧化碳"; break;
                                case "O3": strPC = "臭氧8小时"; strPE = "O<SUB>3</SUB>-8h"; break;
                            }
                            strInner += strPC + "(" + strPE + ")</br>";
                        }
                    }
                    else
                    {
                        strPE = strPP[0].Trim();
                        switch (strPE)
                        {
                            case "SO2": strPC = "二氧化硫"; strPE = "SO<SUB>2</SUB>"; break;
                            case "NO2": strPC = "二氧化氮"; strPE = "NO<SUB>2</SUB>"; break;
                            case "PM10": strPC = "可吸入颗粒物"; strPE = "PM<SUB>10</SUB>"; break;
                            case "PM2.5": strPC = "细颗粒物"; strPE = "PM<SUB>2.5</SUB>"; break;
                            case "CO": strPC = "一氧化碳"; break;
                            case "O3": strPC = "臭氧8小时"; strPE = "O<SUB>3</SUB>-8h"; break;
                        }
                        if (strPE != "--")
                            strInner = strPC + "</br>(" + strPE + ")";
                        else
                            strInner = strPE;
                    }
                }
                else
                    strInner = "--";
                builder.InsertHtml("<b><font style='font-size:9pt'>" + strInner + "</font></b>");
                builder.MoveToMergeField("QualityClass");
                builder.Write(dr["Class"] != DBNull.Value ? dr["Class"].ToString() : "--");
                builder.MoveToMergeField("SO2");
                builder.Write(dr["SO2"] != DBNull.Value ? (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["SO2_IAQI"]), 0)).ToString() : "--");
                builder.MoveToMergeField("NO2");
                builder.Write(dr["NO2"] != DBNull.Value ? (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["NO2_IAQI"]), 0)).ToString() : "--");
                builder.MoveToMergeField("PM10");
                builder.Write(dr["PM10"] != DBNull.Value ? (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["PM10_IAQI"]), 0)).ToString() : "--");
                builder.MoveToMergeField("CO");
                builder.Write(dr["CO"] != DBNull.Value ? (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["CO_IAQI"]), 0)).ToString() : "--");
                builder.MoveToMergeField("O3-8h");
                builder.Write(dr["Max8HourO3"] != DBNull.Value ? (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["Max8HourO3_IAQI"]), 0)).ToString() : "--");
                builder.MoveToMergeField("PM2.5");
                builder.Write(dr["PM25"] != DBNull.Value ? (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["PM25_IAQI"]), 0)).ToString() : "--");
            }
            else
            {
                builder.MoveToMergeField("AQI");
                builder.Write("--");
                builder.MoveToMergeField("PrimaryPollutant");
                builder.Write("--");
                builder.MoveToMergeField("QualityClass");
                builder.Write("--");
                builder.MoveToMergeField("SO2");
                builder.Write("--");
                builder.MoveToMergeField("NO2");
                builder.Write("--");
                builder.MoveToMergeField("PM10");
                builder.Write("--");
                builder.MoveToMergeField("CO");
                builder.Write("--");
                builder.MoveToMergeField("O3-8h");
                builder.Write("--");
                builder.MoveToMergeField("PM2.5");
                builder.Write("--");
            }
        }

        /// <summary>
        /// 导出报表数据
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="ctId"></param>
        /// <param name="count"></param>
        private void ExportReportData(DocumentBuilder builder, string ctId, int count)
        {
            for (int i = 1; i <= count; i++)
            {
                switch (ctId)
                {
                    case "taRemark":
                        HtmlTextArea TaRemark = this.FindControl("taRemark" + i) as HtmlTextArea;
                        builder.MoveToMergeField("taRemark" + i);
                        builder.Write(TaRemark.Value);
                        break;
                    case "txtPointName":
                        RadComboBox PointName = this.FindControl("txtPointName" + i) as RadComboBox;
                        string strPointName = "";
                        foreach (RadComboBoxItem item in PointName.CheckedItems)
                        {
                            strPointName += item.Text + ";";
                        }
                        builder.MoveToMergeField("txtPointName" + i);
                        builder.Write(strPointName.TrimEnd(';'));
                        break;
                    case "cboSendResult":
                        RadComboBox CboSendResult = this.FindControl("cboSendResult" + i) as RadComboBox;
                        builder.MoveToMergeField("cboSendResult" + i);
                        builder.Write(CboSendResult.SelectedItem.Text);
                        break;
                    case "cboSendTime":
                        RadComboBox CboSendTime = this.FindControl("cboSendTime" + i) as RadComboBox;
                        builder.MoveToMergeField("cboSendTime" + i);
                        builder.Write(CboSendTime.SelectedItem.Text);
                        break;
                    case "cboExecPara":
                        RadComboBox CboExecPara = this.FindControl("cboExecPara" + i) as RadComboBox;
                        builder.MoveToMergeField("cboExecPara" + i);
                        string nameStr = "";
                        foreach (RadComboBoxItem item in CboExecPara.CheckedItems)
                        {
                            string name = item.Text;
                            switch (name)
                            {
                                case "SO2": name = "SO<SUB>2</SUB>"; break;
                                case "NO2": name = "NO<SUB>2</SUB>"; break;
                                case "PM10": name = "PM<SUB>10</SUB>"; break;
                                case "PM2.5": name = "PM<SUB>2.5</SUB>"; break;
                                case "O3-8h": name = "O<SUB>3</SUB>-8h"; break;
                            }
                            nameStr += name + ",";
                        }
                        builder.InsertHtml("<b><font style='font-size:9pt'>" + nameStr.TrimEnd(',') + "</font></b>");
                        break;
                    case "cboEliminateRemark":
                        RadComboBox CboEliminateRemark = this.FindControl("cboEliminateRemark" + i) as RadComboBox;
                        builder.MoveToMergeField("cboEliminateRemark" + i);
                        if (CboEliminateRemark.SelectedValue != "")
                            builder.Write(CboEliminateRemark.SelectedItem.Text);
                        break;
                    case "cboEliminateTime":
                        HtmlInputText CboEliminateTime = this.FindControl("cboEliminateTime" + i) as HtmlInputText;
                        builder.MoveToMergeField("cboEliminateTime" + i);
                        builder.Write(CboEliminateTime.Value);
                        break;
                }
            }
        }

        /// <summary>
        /// 获取昨日日报数据
        /// </summary>
        /// <returns></returns>
        protected DataTable GetReportYesterday()
        {
            int recordTotal = 0;  //数据总行数
            string[] regionUids = new string[1] { "7e05b94c-bbd4-45c3-919c-42da2e63fd43" }; //苏州市区Uid
            DateTime ReportTime = ReportDate.SelectedDate.Value;
            DataTable dt = m_DayAQIService.GetRegionAirQualityDayReport(regionUids, ReportTime, ReportTime, 10, 0, out recordTotal).ToTable();
            return dt;
        }

        /// <summary>
        /// 时间触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ReportDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            InitControl();
        }

        /// <summary>
        /// 点位触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PointName1_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadComboBox ExecPara = this.FindControl("cboExecPara1") as RadComboBox;
            RadComboBox EliminateRemark = this.FindControl("cboEliminateRemark1") as RadComboBox;
            ExecPara.Visible = false;
            EliminateRemark.Visible = false;
            if (txtPointName1.CheckedItems.Count > 0)
            {
                ExecPara.Visible = true;
                EliminateRemark.Visible = true;
            }
        }

        protected void PointName2_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadComboBox ExecPara = this.FindControl("cboExecPara2") as RadComboBox;
            RadComboBox EliminateRemark = this.FindControl("cboEliminateRemark2") as RadComboBox;
            ExecPara.Visible = false;
            EliminateRemark.Visible = false;
            if (txtPointName1.CheckedItems.Count > 0)
            {
                ExecPara.Visible = true;
                EliminateRemark.Visible = true;
            }
        }

        protected void PointName3_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadComboBox ExecPara = this.FindControl("cboExecPara3") as RadComboBox;
            RadComboBox EliminateRemark = this.FindControl("cboEliminateRemark3") as RadComboBox;
            ExecPara.Visible = false;
            EliminateRemark.Visible = false;
            if (txtPointName1.CheckedItems.Count > 0)
            {
                ExecPara.Visible = true;
                EliminateRemark.Visible = true;
            }
        }

        protected void PointName4_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadComboBox ExecPara = this.FindControl("cboExecPara4") as RadComboBox;
            RadComboBox EliminateRemark = this.FindControl("cboEliminateRemark4") as RadComboBox;
            ExecPara.Visible = false;
            EliminateRemark.Visible = false;
            if (txtPointName1.CheckedItems.Count > 0)
            {
                ExecPara.Visible = true;
                EliminateRemark.Visible = true;
            }
        }

        protected void PointName5_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadComboBox ExecPara = this.FindControl("cboExecPara5") as RadComboBox;
            RadComboBox EliminateRemark = this.FindControl("cboEliminateRemark5") as RadComboBox;
            ExecPara.Visible = false;
            EliminateRemark.Visible = false;
            if (txtPointName1.CheckedItems.Count > 0)
            {
                ExecPara.Visible = true;
                EliminateRemark.Visible = true;
            }
        }

        protected void PointName6_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadComboBox ExecPara = this.FindControl("cboExecPara6") as RadComboBox;
            RadComboBox EliminateRemark = this.FindControl("cboEliminateRemark6") as RadComboBox;
            ExecPara.Visible = false;
            EliminateRemark.Visible = false;
            if (txtPointName1.CheckedItems.Count > 0)
            {
                ExecPara.Visible = true;
                EliminateRemark.Visible = true;
            }
        }

        protected void PointName7_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadComboBox ExecPara = this.FindControl("cboExecPara7") as RadComboBox;
            RadComboBox EliminateRemark = this.FindControl("cboEliminateRemark7") as RadComboBox;
            ExecPara.Visible = false;
            EliminateRemark.Visible = false;
            if (txtPointName1.CheckedItems.Count > 0)
            {
                ExecPara.Visible = true;
                EliminateRemark.Visible = true;
            }
        }

        protected void PointName8_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadComboBox ExecPara = this.FindControl("cboExecPara8") as RadComboBox;
            RadComboBox EliminateRemark = this.FindControl("cboEliminateRemark8") as RadComboBox;
            ExecPara.Visible = false;
            EliminateRemark.Visible = false;
            if (txtPointName1.CheckedItems.Count > 0)
            {
                ExecPara.Visible = true;
                EliminateRemark.Visible = true;
            }
        }

    }
}