using Aspose.Words;
using Aspose.Words.Tables;
using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.BaseData;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.Service.BaseData.Channel;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.DataAnalyze.Air.DataQuery;
using SmartEP.Service.DataAnalyze.Report;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Utilities.Web.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.Report
{
    public partial class RunningMonthReport : SmartEP.WebUI.Common.BasePage
    {
        AirPollutantService AirPollutantService = Singleton<AirPollutantService>.GetInstance();
        MonitoringPointAirService MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
        DataQueryByDayService m_DataQueryByDayService = new DataQueryByDayService();
        ReportLogService ReportLogService = new ReportLogService();
        DateTime dt = DateTime.Now;
        static string strPointIds = "";
        static string strPointNames = "";
        static string strPollutantCodes = "";
        static string strPollutantNames = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.ViewState["DisplayPerson"] = PageHelper.GetQueryString("DisplayPerson");
                InitControl();
            }
        }
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            rmypMonth.SelectedDate = DateTime.Now;
            ////取得国控点
            //MonitoringPointAirService m_MonitoringPointAirService = Singleton<MonitoringPointAirService>.GetInstance();
            //string strpointName = "";
            //IQueryable<MonitoringPointEntity> EnableOrNotports = m_MonitoringPointAirService.RetrieveAirMPListByEnable();
            //string[] EnableOrNotportsarry = EnableOrNotports.Where(p => p.ContrlUid == "6fadff52-2338-4319-9f1d-7317823770ad" || p.ContrlUid == "bdf0837a-eb59-4c4a-a05f-c774a17f3077" || p.ContrlUid == "c1158eb6-4d69-4846-a963-d16b9d2794ca").Select(p => p.MonitoringPointName).ToArray();
            //foreach (string point in EnableOrNotportsarry)
            //{
            //    strpointName += point + ";";
            //}
            //pointCbxRsm.SetPointValuesFromNames(strpointName);


            //IQueryable<MonitoringPointEntity> ports = MonitoringPointAir.RetrieveAirMPListByCountryControlled();
            //pointCbx.DataValueField = "PointId";
            //pointCbx.DataTextField = "MonitoringPointName";
            //pointCbx.DataSource = ports;
            //pointCbx.DataBind();
            for (int i = 0; i < pointCbx.Items.Count; i++)
            {
                pointCbx.Items[i].Checked = true;
            }
            //取得常规六参数
            //IQueryable<PollutantCodeEntity> factors = AirPollutantService.RetrieveListByCalAQI();
            //factorCbx.DataTextField = "PollutantName";
            //factorCbx.DataValueField = "PollutantCode";
            //factorCbx.DataSource = factors;
            //factorCbx.DataBind();
            for (int i = 0; i < factorCbx.Items.Count; i++)
            {
                factorCbx.Items[i].Checked = true;
            }
        }

        public string GetMonthDays(DateTime begin)
        {
            string strRetValue = "";
            int intDays = begin.Day;
            strRetValue = intDays.ToString() + "(" + intDays * 24 + ")";
            return strRetValue;
        }
        protected void btnMonthReport_Click(object sender, EventArgs e)
        {
            PMChange();
            //Response.Write("<div id='mydiv' >");
            //Response.Write("_");
            //Response.Write("</div>");
            //Response.Write("<script>mydiv.innerText = '';</script>");
            //Response.Write("<script language=javascript>;");
            //Response.Write("var dots = 0;var dotmax = 10;function ShowWait()");
            //Response.Write("{var output; output = '正在装载页面';dots++;if(dots>=dotmax)dots=1;");
            //Response.Write("for(var x = 0;x < dots;x++){output += '·';}mydiv.innerText = output;}");
            //Response.Write("function StartShowWait(){mydiv.style.visibility = 'visible'; ");
            //Response.Write("window.setInterval('ShowWait()',1000);}");
            //Response.Write("function HideWait(){mydiv.style.display = 'none';");
            //Response.Write("window.clearInterval();}");
            //Response.Write("StartShowWait();</script>");
            //Response.Flush();
            //Thread.Sleep(10000);
            DateTime dtStart = Convert.ToDateTime(rmypMonth.SelectedDate.Value + "-1");
            string strMonthDays = GetMonthDays(dtStart.AddMonths(1).AddMilliseconds(-1));

            string[] portIds = strPointIds.Split(';');
            string[] factorCodes = strPollutantCodes.Split(';');
            List<SmartEP.Core.Interfaces.IPollutant> factors = AirPollutantService.GetDefaultFactors(factorCodes);
            DateTime dtBegion = new DateTime(rmypMonth.SelectedDate.Value.Year, rmypMonth.SelectedDate.Value.Month, 1);
            DateTime dtEnd = Convert.ToDateTime(dtBegion.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59"));
            int yearFrom = dtBegion.Year;
            int monthOfYearFrom = dtBegion.Month;
            int yearTo = dtEnd.Year;
            int monthOfYearTo = dtEnd.Month;
            //每页显示数据个数            
            int pageSize = 99999999;
            //当前页的序号
            int pageNo = 0;

            //数据总行数
            int recordTotal = 0;
            DataView MonthRun = m_DataQueryByDayService.GetMonthRun(portIds, dtStart);
            Document doc = new Document(System.IO.Path.Combine(MapPath("DocumentsTemplet"), "RunReport.doc"));
            DocumentBuilder builder = new DocumentBuilder(doc);

            builder.MoveToMergeField("MonthTime");
            //小三、居中
            builder.Font.ClearFormatting();
            builder.Font.Size = 15;
            builder.Font.Name = "楷体_GB2312";
            builder.Font.Bold = true;
            builder.ParagraphFormat.Alignment = Aspose.Words.ParagraphAlignment.Center;
            builder.Writeln(dtStart.ToString("yyyy") + "年" + dtStart.Month.ToString() + "月");
            builder.Font.ClearFormatting();
            builder.ParagraphFormat.ClearFormatting();
            builder.MoveToMergeField("RunReport");
            //   builder.Writeln(dtStart.ToString("yyyy") + "年" + dtStart.Month.ToString() + "月");
            BuildRunReport(builder, MonthRun);
            builder.MoveToMergeField("TJReport");
            BuildTJReport(builder, portIds, factors, yearFrom, monthOfYearFrom, yearTo, monthOfYearTo);
            builder.MoveToMergeField("DataDate");
            builder.Font.ClearFormatting();
            builder.Font.Size = 12;
            builder.Font.Name = "楷体_GB2312";
            builder.Font.Bold = true;
            builder.Write(DateTime.Now.ToString("yyyy") + "年" + DateTime.Now.Month.ToString() + "月" + DateTime.Now.Day.ToString() + "日");
            // builder.Write(dtStart.ToString("yyyy") + "年" + dtStart.Month.ToString() + "月\n");

            doc.MailMerge.DeleteFields();

            string filename = "(" + dt.ToString("yyyyMMddHHmmssffff") + ")" + "RunningMonthReport" + ".doc";
            string strTarget = Server.MapPath("../../../Pages/EnvAir/Report/ReportFile/RunningMonthReport/" + rmypMonth.SelectedDate.Value.Year + "/" + rmypMonth.SelectedDate.Value.Month + "/" + filename);
            doc.Save(strTarget);
            Response.Write("<script>closeWin()</script>");
            doc.Save(this.Response, "RunningMonthReport" + ".doc", ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Docx));//+ Convert.ToDateTime(ViewState["FromDate"].ToString()).ToString("yyyyMMdd") + "-" + Convert.ToDateTime(ViewState["ToDate"].ToString()).ToString("yyyyMMdd")
            ReportLogEntity customDatum = new ReportLogEntity();
            customDatum.PointIds = strPointIds;//测点Code
            customDatum.PointNames = strPollutantNames;//测点名称
            customDatum.FactorCodes = strPollutantCodes;//因子Code
            customDatum.FactorsNames = strPollutantNames;//因子名称
            customDatum.DateTimeRange = rmypMonth.SelectedDate.Value.Year.ToString() + "年" + rmypMonth.SelectedDate.Value.Month.ToString() + "月";
            customDatum.WaterOrAirType = 1;//水或气 0：水，1：气
            customDatum.PageTypeID = "RunningMonthReport";//页面ID
            customDatum.StartDateTime = dtBegion;
            customDatum.EndDateTime = dtEnd;
            customDatum.CreatUser = this.ViewState["DisplayPerson"].ToString();
            customDatum.ExportName = "运行月报表" + string.Format("{0:yyyyMM}", rmypMonth.SelectedDate.Value);
            customDatum.ReportName = ("../../../Pages/EnvAir/Report/ReportFile/RunningMonthReport/" + rmypMonth.SelectedDate.Value.Year + "/" + rmypMonth.SelectedDate.Value.Month + "/" + filename).ToString();
            customDatum.CreatDateTime = DateTime.Now;
            //添加数据
            ReportLogService.ReportLogAdd(customDatum);
            Response.End();

        }
        public void BuildRunReport(DocumentBuilder builder, DataView dvCollect)
        {
            PMChange();
            string[] strHead = { "子站名称", "本月天数(累计时)", "有效日天数(累计时)", "污染物数据(个)", "气象数据(个)" };
            string[] arrPortsValue = strPointIds.Split(';');
            string[] portNames = strPointNames.Split(';');
            string strMonthDays = "", strValidMonthDays = "", strPollute = "", strWeather = "";
            int intMonthDays = 0, intMonthTimes = 0, intValidMonthDays = 0, intValidMonthTimes = 0, intPollute = 0, intPollute2 = 0, intWeather = 0;
            //    string[] arrPortsName = portName.Trim(',').Split(',');
            int intY = arrPortsValue.Length + 2;
            int intX = strHead.Length;

            builder.RowFormat.Height = 0.3 * 72;	//0.4"
            builder.RowFormat.HeightRule = HeightRule.Exactly;
            builder.CellFormat.Width = 1.6 * 72;	//0.4"
            // double lenX0 =2 * 72;
            //double lenX1 = 2 * 72;
            double lenY0 = 0.5 * 72;
            double lenY1 = 0.2 * 72;
            string nullStr = "--";
            builder.ParagraphFormat.Alignment = Aspose.Words.ParagraphAlignment.Center;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;

            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.CellFormat.Borders.LineStyle = LineStyle.Single;
            builder.CellFormat.Borders.LineWidth = 1;
            builder.CellFormat.Borders.Color = System.Drawing.Color.Black;

            for (int y = 0; y < intY; y++)
            {
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                for (int x = 0; x < intX; x++)
                {
                    builder.InsertCell();
                    #region 表头
                    if (y == 0)
                    {
                        //黑体10号
                        builder.Font.ClearFormatting();
                        builder.Font.Size = 10;
                        builder.Font.Name = "宋体";
                        builder.Font.Bold = true;
                        builder.RowFormat.Height = lenY0;
                        builder.CellFormat.VerticalMerge = CellMerge.First;
                        builder.Write(strHead[x].ToString());
                        builder.Font.ClearFormatting();
                    }

                    #endregion

                    #region 数据区
                    if (y > 0 && y < intY - 1)
                    {
                        builder.Font.ClearFormatting();
                        builder.Font.Size = 10;
                        builder.Font.Name = "宋体";
                        //builder.Font.Bold = true;
                        builder.RowFormat.Height = lenY1;
                        if (x == 0)
                        {

                            builder.Write(portNames[y - 1].ToString());
                        }
                        else
                        {
                            dvCollect.RowFilter = "PointId='" + arrPortsValue[y - 1] + "'";
                            switch (x)
                            {
                                case 1: strMonthDays = dvCollect[0]["MonthDays"].ToString() + "(" + dvCollect[0]["MonthHours"].ToString() + ")";
                                    builder.Write(strMonthDays);
                                    intMonthDays += Convert.ToInt32(dvCollect[0]["MonthDays"]);
                                    intMonthTimes += Convert.ToInt32(dvCollect[0]["MonthHours"]);
                                    break;
                                case 2: strValidMonthDays = dvCollect[0]["ValidMonthDays"].ToString() + "(" + dvCollect[0]["ValidMonthHours"].ToString() + ")";
                                    builder.Write(strValidMonthDays);
                                    if (dvCollect[0]["ValidMonthDays"].IsNotNullOrDBNull())
                                    {
                                        intValidMonthDays += Convert.ToInt32(dvCollect[0]["ValidMonthDays"]);
                                    }
                                    if (dvCollect[0]["ValidMonthHours"].IsNotNullOrDBNull())
                                    {
                                        intValidMonthTimes += Convert.ToInt32(dvCollect[0]["ValidMonthHours"]);
                                    }
                                    break;
                                case 3: strPollute = dvCollect[0]["intPollute"].ToString();
                                    if (strPollute != "0" && dvCollect[0]["intPollute"].IsNotNullOrDBNull())
                                    {
                                        builder.Write(strPollute);
                                        intPollute += Convert.ToInt32(strPollute);
                                    }
                                    else
                                    {
                                        builder.Write(nullStr);
                                    }

                                    break;
                                case 4:
                                    strWeather = dvCollect[0]["intWeather"].ToString();
                                    if (strWeather != "0" && dvCollect[0]["intWeather"].IsNotNullOrDBNull())
                                    {
                                        builder.Write(strWeather);
                                        intPollute2 += Convert.ToInt32(strWeather);
                                    }
                                    else
                                    {
                                        builder.Write(nullStr);
                                    }

                                    break;
                                default: builder.Write(nullStr); break;
                            }
                        }
                        builder.Font.ClearFormatting();
                    }
                    #endregion
                    #region   表尾
                    if (y == intY - 1)
                    {
                        builder.Font.ClearFormatting();
                        builder.Font.Size = 10;
                        builder.Font.Name = "宋体";
                        //builder.Font.Bold = true;
                        builder.RowFormat.Height = lenY1;
                        if (x == 0)
                        {

                            builder.Write("合计");
                        }
                        else
                        {
                            switch (x)
                            {
                                case 1:
                                    builder.Write(intMonthDays.ToString() + "(" + intMonthTimes + ")"); break;
                                case 2:
                                    builder.Write(intValidMonthDays.ToString() + "(" + intValidMonthTimes + ")"); break;
                                case 3:
                                    builder.Write(intPollute.ToString()); break;
                                case 4:
                                    builder.Write(intPollute2.ToString()); break;

                                default: builder.Write(nullStr); break;
                            }
                        }
                        builder.Font.ClearFormatting();
                    }
                    #endregion

                }
                builder.EndRow();
            }

            builder.EndTable();

        }
        public void BuildTJReport(DocumentBuilder builder, string[] portIds, IList<SmartEP.Core.Interfaces.IPollutant> factors, int yearFrom, int monthOfYearFrom
            , int yearTo, int monthOfYearTo)
        {
            //获取因子信息接口实例化
            AirPollutantService m_AirPollutantService = new AirPollutantService();
            var dvCollect = m_DataQueryByDayService.GetMonthDataPager(portIds, factors, yearFrom, monthOfYearFrom, yearTo, monthOfYearTo);
            string[] arrPortsValue = portIds;
            string[] arrPortsName = strPointNames.Split(';');

            string[] arryfactor = strPollutantCodes.Split(';');
            string strfactor = "";
            string strFactorName = "";
            foreach (string factor in arryfactor)
            {
                strfactor += factor + ",";
                if (factor != "" && factor == "a21026")
                {
                    strFactorName += "SO<sub>2</sub>,";
                }
                if (factor != "" && factor == "a21004")
                {
                    strFactorName += "NO<sub>2</sub>,";
                }
                if (factor != "" && factor == "a21005")
                {
                    strFactorName += "CO,";
                }
                if (factor == "a05024")
                {
                    strFactorName += "O<sub>3</sub>-1h, O<sub>3</sub>-8h,";
                }
                if (factor != "" && factor == "a34002")
                {
                    strFactorName += "PM<sub>10</sub>,";
                }
                if (factor != "" && factor == "a34004")
                {
                    strFactorName += "PM<sub>2.5</sub>,";
                }
            }
            string[] arrFactorValue = strfactor.Replace("a05024,", "MaxOneHourO3,Max8HourO3,").Trim(',').Split(',');

            string[] strHead = strFactorName.Trim(',').Split(',');
            //  string[] strHead = { "SO<sub>2</sub>", "NO<sub>x</sub>", "NO<sub>2</sub>", "CO", "O<sub>3</sub>-1", "O<sub>3</sub>-8", "PM<sub>10</sub>", "PM<sub>2.5</sub>" };
            int intY = strHead.Length + 1;
            int intX = portIds.Length + 4;

            builder.RowFormat.Height = 0.3 * 72;	//0.4"
            builder.RowFormat.HeightRule = HeightRule.Exactly;
            builder.CellFormat.Width = 0.7 * 82;	//0.4"
            double lenY0 = 0.8 * 72;
            double lenY1 = 0.2 * 72;
            string nullStr = "--";
            //   builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;

            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.CellFormat.Borders.LineStyle = LineStyle.Single;
            builder.CellFormat.Borders.LineWidth = 1;
            builder.CellFormat.Borders.Color = System.Drawing.Color.Black;
            for (int y = 0; y < intY; y++)
            {
                double avgDataAmount = 0;
                int avgDataCount = 0;
                double avgDataAmount2 = 0;
                int avgDataCount2 = 0;

                builder.CellFormat.HorizontalMerge = CellMerge.None;
                for (int x = 0; x < intX; x++)
                {

                    builder.InsertCell();
                    #region 表头
                    if (y == 0)
                    {
                        builder.RowFormat.Height = lenY0;
                        builder.CellFormat.Width = 0.7 * 82;	//0.4"
                        //黑体10号
                        builder.Font.ClearFormatting();
                        builder.Font.Size = 10;
                        builder.Font.Name = "宋体";
                        builder.Font.Bold = true;
                        if (x == 0)
                        {
                            builder.Write("监测项");
                        }
                        if (x == 1)
                        {
                            builder.Write("类 别");
                        }
                        if (x >= 2 && x < intX - 2)
                        {
                            builder.CellFormat.VerticalMerge = CellMerge.First;
                            builder.Write(arrPortsName[x - 2].ToString());
                        }
                        if (x == intX - 2)
                        {
                            builder.Write("全市均值");

                        }
                        if (x == intX - 1)
                        {
                            builder.Write("剔除对照点\n后市均值");
                        }
                        builder.Font.ClearFormatting();
                    }

                    #endregion

                    #region 数据区
                    if (y > 0)
                    {
                        builder.Font.ClearFormatting();
                        builder.Font.Size = 9;
                        builder.Font.Name = "宋体";
                        //builder.Font.Bold = true;
                        builder.RowFormat.Height = lenY1;
                        builder.CellFormat.Width = 0.7 * 82;	//0.4"
                        if (x == 0)
                        {
                            builder.InsertHtml(strHead[y - 1].ToString());
                        }
                        else
                        {

                            if (x == 1)
                            {
                                builder.Write("有效日");
                            }
                            if (x >= 2 && x < intX - 2)
                            {
                                dvCollect.RowFilter = "PointId='" + arrPortsValue[x - 2] + "'";
                                if (dvCollect.Count > 0 && dvCollect[0][arrFactorValue[y - 1]] != null && dvCollect[0][arrFactorValue[y - 1]].ToString() != "")
                                {
                                    //int DecimalNum = 3;
                                    //if (arrFactorValue[y - 1] == "MaxOneHourO3" || arrFactorValue[y - 1] == "Max8HourO3")
                                    //{
                                    //    DecimalNum = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo("a05024").PollutantDecimalNum);
                                    //}
                                    //else
                                    //{
                                    //    DecimalNum = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo(arrFactorValue[y - 1]).PollutantDecimalNum);
                                    //}
                                    if (dvCollect[0][arrFactorValue[y - 1]].IsNotNullOrDBNull() && dvCollect[0][arrFactorValue[y - 1]].ToString() != "-1")
                                    {
                                        if (arrFactorValue[y - 1] != "a21005")
                                            builder.Write(DecimalExtension.GetPollutantValue(Convert.ToDecimal(dvCollect[0][arrFactorValue[y - 1]]) * 1000, 0).ToString());
                                        else
                                            builder.Write(DecimalExtension.GetPollutantValue(Convert.ToDecimal(dvCollect[0][arrFactorValue[y - 1]]), 1).ToString());
                                    }
                                    else if (dvCollect[0][arrFactorValue[y - 1]].ToString() == "-1")
                                    {
                                        builder.Write(dvCollect[0][arrFactorValue[y - 1]].ToString());
                                    }
                                    else
                                    {
                                        builder.Write(nullStr);
                                    }
                                    //if (x == 2)
                                    if (arrPortsValue[x - 2] == "8")
                                    {
                                        if (dvCollect[0][arrFactorValue[y - 1]].IsNotNullOrDBNull() && dvCollect[0][arrFactorValue[y - 1]].ToString() != "-1")
                                        {
                                            avgDataAmount2 += Convert.ToDouble(dvCollect[0][arrFactorValue[y - 1]].ToString());
                                            avgDataCount2++;
                                        }
                                    }
                                    else
                                    {
                                        if (dvCollect[0][arrFactorValue[y - 1]].IsNotNullOrDBNull() && dvCollect[0][arrFactorValue[y - 1]].ToString() != "-1")
                                        {
                                            avgDataAmount2 += Convert.ToDouble(dvCollect[0][arrFactorValue[y - 1]].ToString());
                                            avgDataCount2++;
                                            avgDataAmount += Convert.ToDouble(dvCollect[0][arrFactorValue[y - 1]].ToString());
                                            avgDataCount++;
                                        }
                                    }

                                }
                                else
                                {
                                    builder.Write(nullStr);
                                }
                            }

                            if (x == intX - 2)//全市均值
                            {
                                if (avgDataCount2 > 0)
                                {
                                    double avgData2 = avgDataAmount2 / (double)avgDataCount2;
                                    if (arrFactorValue[y - 1] != "a21005")
                                        builder.Write(DecimalExtension.GetPollutantValue(Convert.ToDecimal(avgData2) * 1000, 0).ToString());
                                    else
                                        builder.Write(DecimalExtension.GetPollutantValue(Convert.ToDecimal(avgData2), 1).ToString());
                                }
                                else
                                {
                                    builder.Write(nullStr);
                                }
                            }
                            if (x == intX - 1)//剔除对照
                            {
                                if (avgDataCount > 0)
                                {
                                    double avgData = avgDataAmount / (double)avgDataCount;
                                    if (arrFactorValue[y - 1] != "a21005")
                                        builder.Write(DecimalExtension.GetPollutantValue(Convert.ToDecimal(avgData) * 1000, 0).ToString());
                                    else
                                        builder.Write(DecimalExtension.GetPollutantValue(Convert.ToDecimal(avgData), 1).ToString());
                                }
                                else
                                {
                                    builder.Write(nullStr);
                                }
                            }


                        }
                        builder.Font.ClearFormatting();
                    }
                    #endregion
                    #region   表尾

                    #endregion

                }
                builder.EndRow();
            }

            builder.EndTable();
        }

        protected void btnExport_Click(object sender, ImageClickEventArgs e)
        {
            PMChange();
            DateTime dtStart = Convert.ToDateTime(rmypMonth.SelectedDate.Value + "-1");
            string strMonthDays = GetMonthDays(dtStart.AddMonths(1).AddMilliseconds(-1));
            string[] portIds = strPointIds.Split(';');
            string[] factorCodes = strPollutantCodes.Split(';');
            IList<SmartEP.Core.Interfaces.IPollutant> factors = AirPollutantService.GetDefaultFactors(factorCodes);
            DateTime dtBegion = new DateTime(rmypMonth.SelectedDate.Value.Year, rmypMonth.SelectedDate.Value.Month, 1);
            DateTime dtEnd = Convert.ToDateTime(dtBegion.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59"));
            int yearFrom = dtBegion.Year;
            int monthOfYearFrom = dtBegion.Month;
            int yearTo = dtEnd.Year;
            int monthOfYearTo = dtEnd.Month;
            DataView MonthRun = m_DataQueryByDayService.GetMonthRun(portIds, dtStart);
            Document doc = new Document(System.IO.Path.Combine(MapPath("DocumentsTemplet"), "RunReport.doc"));
            DocumentBuilder builder = new DocumentBuilder(doc);

            string pointCodes = strPointIds;
            string pointNames = strPointNames;
            string factorsName = strPollutantNames;
            string factorCode = strPollutantCodes;
            builder.MoveToMergeField("PointCount");
            //小三、居中
            builder.Font.ClearFormatting();
            builder.Font.Size = 12;
            builder.Font.Name = "楷体_GB2312";
            builder.Font.Bold = true;
            //builder.ParagraphFormat.Alignment = Aspose.Words.ParagraphAlignment.Center;
            builder.Writeln("本月份系统运行正常，" + portIds.Length.ToString() + "个子站运行天数均达到并超过了《环境空气自动监测技术规范》的要求,满足日报需要。");
            builder.Font.ClearFormatting();
            //builder.ParagraphFormat.ClearFormatting();
            builder.MoveToMergeField("MonthTime");
            //小三、居中
            builder.Font.ClearFormatting();
            builder.Font.Size = 15;
            builder.Font.Name = "楷体_GB2312";
            builder.Font.Bold = true;
            builder.ParagraphFormat.Alignment = Aspose.Words.ParagraphAlignment.Center;
            builder.Writeln(dtStart.ToString("yyyy") + "年" + dtStart.Month.ToString() + "月");
            builder.Font.ClearFormatting();
            builder.ParagraphFormat.ClearFormatting();
            builder.MoveToMergeField("RunReport");
            //   builder.Writeln(dtStart.ToString("yyyy") + "年" + dtStart.Month.ToString() + "月");
            BuildRunReport(builder, MonthRun);
            builder.MoveToMergeField("TJReport");
            BuildTJReport(builder, portIds, factors, yearFrom, monthOfYearFrom, yearTo, monthOfYearTo);
            builder.MoveToMergeField("DataDate");
            builder.Font.ClearFormatting();
            builder.Font.Size = 12;
            builder.Font.Name = "楷体_GB2312";
            builder.Font.Bold = true;
            builder.Write(DateTime.Now.ToString("yyyy") + "年" + DateTime.Now.Month.ToString() + "月" + DateTime.Now.Day.ToString() + "日");
            // builder.Write(dtStart.ToString("yyyy") + "年" + dtStart.Month.ToString() + "月\n");

            doc.MailMerge.DeleteFields();
            string filename = "(" + dt.ToString("yyyyMMddHHmmssffff") + ")" + "RunningMonthReport" + ".doc";
            string strTarget = Server.MapPath("../../../Pages/EnvAir/Report/ReportFile/RunningMonthReport/" + rmypMonth.SelectedDate.Value.Year + "/" + rmypMonth.SelectedDate.Value.Month + "/" + filename);
            doc.Save(strTarget);
            //doc.Save(this.Response, "RunningMonthReport" + ".doc", ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Docx));//+ Convert.ToDateTime(ViewState["FromDate"].ToString()).ToString("yyyyMMdd") + "-" + Convert.ToDateTime(ViewState["ToDate"].ToString()).ToString("yyyyMMdd")
            ReportLogEntity customDatum = new ReportLogEntity();
            customDatum.PointIds = pointCodes;//测点Code
            customDatum.PointNames = pointNames;//测点名称
            customDatum.FactorCodes = factorCode;//因子Code
            customDatum.FactorsNames = factorsName;//因子名称
            customDatum.DateTimeRange = rmypMonth.SelectedDate.Value.Year.ToString() + "年" + rmypMonth.SelectedDate.Value.Month.ToString() + "月";
            customDatum.WaterOrAirType = 1;//水或气 0：水，1：气
            customDatum.PageTypeID = "RunningMonthReport";//页面ID
            customDatum.StartDateTime = dtBegion;
            customDatum.EndDateTime = dtEnd;
            customDatum.CreatUser = this.ViewState["DisplayPerson"].ToString();
            customDatum.ExportName = "运行月报表" + string.Format("{0:yyyyMM}", rmypMonth.SelectedDate.Value);
            customDatum.ReportName = ("../../../Pages/EnvAir/Report/ReportFile/RunningMonthReport/" + rmypMonth.SelectedDate.Value.Year + "/" + rmypMonth.SelectedDate.Value.Month + "/" + filename).ToString();
            customDatum.CreatDateTime = DateTime.Now;
            //添加数据
            ReportLogService.ReportLogAdd(customDatum);
            if (!Directory.Exists(strTarget))
            {


                Alert("保存成功！");
                //Directory.CreateDirectory(strTarget);//创建新路径
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "reload", "<script type=\"text/javascript\">self.parent.location.reload();</script>", false);
            //Response.End();


        }

        /// <summary>
        /// 点位因子触发事件
        /// </summary>
        private void PMChange()
        {
            strPointIds = "";
            strPointNames = "";
            strPollutantCodes = "";
            strPollutantNames = "";
            foreach (RadComboBoxItem item in pointCbx.CheckedItems)
            {
                strPointIds += item.Value + ";";
                strPointNames += item.Text + ";";
            }
            strPointIds = strPointIds.TrimEnd(';');
            strPointNames = strPointNames.TrimEnd(';');
            foreach (RadComboBoxItem item in factorCbx.CheckedItems)
            {
                strPollutantCodes += item.Value + ";";
                strPollutantNames += item.Text + ";";
            }
            strPollutantCodes = strPollutantCodes.TrimEnd(';');
            strPollutantNames = strPollutantNames.TrimEnd(';');
        }
    }
}