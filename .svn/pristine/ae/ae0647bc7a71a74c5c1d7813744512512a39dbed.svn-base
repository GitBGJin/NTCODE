using Aspose.Words;
using Aspose.Words.Tables;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.BaseData;
using SmartEP.DomainModel.MonitoringBusiness;
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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace SmartEP.WebUI.Pages.EnvAir.Report
{
    public partial class AvgDayOfMonthreportNew : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        CustomDataService customDataService = new CustomDataService();
        MonitoringPointWaterService monitoringPointWaterService = new MonitoringPointWaterService();
        ReportLogService ReportLogService = new ReportLogService();

        //日数据
        DataQueryByDayService m_DayData = Singleton<DataQueryByDayService>.GetInstance();
        /// <summary>
        /// 选择站点
        /// </summary>
        private IList<IPoint> points = null;

        /// <summary>
        /// 因子
        /// </summary>
        SmartEP.Service.BaseData.Channel.WaterPollutantService m_WaterPollutantService = new SmartEP.Service.BaseData.Channel.WaterPollutantService();
        DateTime dt = DateTime.Now;
        /// <summary>
        /// 月报时间
        /// </summary>
        DateTime beginTime;
        DateTime endTime;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.ViewState["DisplayPerson"] = PageHelper.GetQueryString("DisplayPerson");
                InitControl();
                // pointCbxRsm_SelectedChanged();
            }
        }

        #region
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {

            ////国控点，对照点，路边站
            //MonitoringPointAirService m_MonitoringPointAirService = Singleton<MonitoringPointAirService>.GetInstance();
            //string strpointName = "";
            //IQueryable<MonitoringPointEntity> EnableOrNotports = m_MonitoringPointAirService.RetrieveAirMPListByEnable();
            //string[] EnableOrNotportsarry = EnableOrNotports.Where(p => p.ContrlUid == "c1158eb6-4d69-4846-a963-d16b9d2794ca").Select(p => p.MonitoringPointName).ToArray();
            //foreach (string point in EnableOrNotportsarry)
            //{
            //    strpointName += point + ";";
            //}
            //pointCbxRsm.SetPointValuesFromNames(strpointName);

            rmypMonthTime.SelectedDate = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy年MM月"));
            GetDateRange();
            lblRange.Text = beginTime.ToString("yyyy-MM-dd") + " ~ " + endTime.ToString("yyyy-MM-dd");

            //根据页面ID、水或气的类别获取数据
            var customData = customDataService.CustomDatumRetrieve("AvgDayOfMonthreportNew", 1);

        }
        #endregion

        /// <summary>
        /// 获取时间范围
        /// </summary>
        private void GetDateRange()
        {
            if (rmypMonthTime != null)
            {
                int daysInMonth = rmypMonthTime.SelectedDate.Value.DaysInMonth();
                beginTime = Convert.ToDateTime(rmypMonthTime.SelectedDate.Value.ToString("yyyy-MM") + "-01 00:00:00");
                endTime = Convert.ToDateTime(rmypMonthTime.SelectedDate.Value.ToString("yyyy-MM-") + daysInMonth.ToString() + " 23:59:59");
            }
        }

        /// <summary>
        /// 月报时间触发事件
        /// </summary>
        protected void rmypMonthTime_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            GetDateRange();
            lblRange.Text = beginTime.ToString("yyyy-MM-dd") + " ~ " + endTime.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 月报统计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExport_Click(object sender, EventArgs e)
        {
            GetDateRange();

            //站点Id
            // string[] pointIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
            string[] pointIds = comboCity.CheckedItems.Select(t => t.Value).ToArray();

            Document doc = new Document(System.IO.Path.Combine(MapPath("DocumentsTemplet"), "AvgDayOfMonthreportNew.doc"));
            DocumentBuilder builder = new DocumentBuilder(doc);

            builder.MoveToMergeField("M1");
            for (int i = 0; i < pointIds.Length; i++)
            {
                builder.Font.ClearFormatting();
                DataTable dt = GetMonthReportContent(pointIds[i], beginTime, endTime);
                DataTable dtNew = GetAvgDayOfMonthData(pointIds[i], beginTime, endTime);
                MoveToMF3(builder, dt, dtNew, pointIds[i], i);

            }

            doc.MailMerge.DeleteFields();
            //doc.Save("AvgDayOfMonthreportNew" + ".doc", Aspose.Words.SaveFormat.Doc, SaveType.OpenInWord, this.Response);

            doc.Save(this.Response, "AvgDayOfMonthreportNew" + ".doc", ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
            save(doc);
            Response.End();


        }

        /// <summary>
        /// 填入子站月报表数据
        /// </summary>
        /// <param name="dt"></param>
        private void MoveToMF3(DocumentBuilder builder, DataTable dt, DataTable dtNew, string pointId, int Count)
        {
            //builder.ParagraphFormat.Alignment = Aspose.Words.ParagraphAlignment.Left;
            //string pointName = monitoringPointWaterService.RetrieveEntityByPointId(Convert.ToInt32(pointId)).MonitoringPointName;
            string pointName = "";
            string factorCode = "";
            if (pointId == "9")
            {
                factorCode = System.Configuration.ConfigurationManager.AppSettings["AirPollutantCode1"];
                pointName = "表2 监测站（A Station）";
            }
            else if (pointId == "33")
            {
                factorCode = System.Configuration.ConfigurationManager.AppSettings["AirPollutantCode2"];
                pointName = "表3 拙政园（B2 Station）";
            }
            else if (pointId == "34")
            {
                factorCode = System.Configuration.ConfigurationManager.AppSettings["AirPollutantCode3"];
                pointName = "表4 苏州中学（C1 Station）";
            }
            else if (pointId == "38")
            {
                factorCode = System.Configuration.ConfigurationManager.AppSettings["AirPollutantCode3"];
                pointName = "表7 市政府（C4 Station）";
            }
            else if (pointId == "35")
            {
                factorCode = System.Configuration.ConfigurationManager.AppSettings["AirPollutantCode4"];
                pointName = "表5 苏州大学东校区（C2 Station）";
            }
            else if (pointId == "36")
            {
                factorCode = System.Configuration.ConfigurationManager.AppSettings["AirPollutantCode4"];
                pointName = "表6 工业园区市政公司（C3 Station）";
            }
            else if (pointId == "39")
            {
                factorCode = System.Configuration.ConfigurationManager.AppSettings["AirPollutantCode5"];
                pointName = "表8 渔洋山水厂（D Station）";
            }
            else if (pointId == "24")
            {
                factorCode = System.Configuration.ConfigurationManager.AppSettings["AirPollutantCode6"];
                pointName = "表9 光大环保监测站";
            }
            string[] factorCodes = factorCode.Trim(';').Split(';');

            builder.ParagraphFormat.ClearFormatting();
            if (Count > 0)
            {
                builder.ParagraphFormat.PageBreakBefore = true;//段前分页
            }
            //builder.Font.Size = 12;
            //builder.Font.Name = "宋体";
            //builder.Font.Bold = true;
            //builder.Write("");
            builder.ParagraphFormat.ClearFormatting();
            builder.Font.ClearFormatting();
            builder.Font.Size = 14;
            builder.Font.Name = "宋体";
            builder.Font.Bold = false;
            builder.ParagraphFormat.Alignment = Aspose.Words.ParagraphAlignment.Center;
            builder.Write("\n\n" + pointName + "日均值表\n\n");

            builder.Font.ClearFormatting();
            //builder.Font.Size = 9;
            //builder.Font.Name = "楷体_GB2312";
            //builder.Font.Bold = true;
            //int lenX = 105;
            //builder.Write("");
            //builder.Font.ClearFormatting();

            //站点Id
            // string[] pointIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
            string[] pointIds = comboCity.CheckedItems.Select(t => t.Value).ToArray();
            for (int i = 0; i < 4 + rmypMonthTime.SelectedDate.Value.DaysInMonth(); i++)
            {
                string name = "";
                if (i == 2 + rmypMonthTime.SelectedDate.Value.DaysInMonth())
                    name = "有效天";
                if (i == 3 + rmypMonthTime.SelectedDate.Value.DaysInMonth())
                    name = "月均值";
                for (int j = 0; j < 1 + factorCodes.Length; j++)
                {
                    builder.InsertCell();
                    builder.CellFormat.Borders.LineStyle = LineStyle.Single;
                    builder.CellFormat.Borders.Color = System.Drawing.Color.Black;
                    builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                    builder.CellFormat.VerticalMerge = CellMerge.None;
                    if (i == 0)
                    {

                        if (j == 0)
                        {
                            builder.Font.ClearFormatting();
                            builder.Font.Size = 9;
                            builder.Font.Name = "楷体_GB2312";
                            builder.Font.Bold = true;
                            builder.RowFormat.Height = 30.0;
                            builder.Font.ClearFormatting();
                            builder.CellFormat.VerticalMerge = CellMerge.First;
                            builder.Write("日期");

                        }
                        else
                        {
                            builder.Font.ClearFormatting();
                            builder.Font.Size = 9;
                            builder.Font.Name = "楷体_GB2312";
                            builder.Font.Bold = true;
                            builder.RowFormat.Height = 30.0;
                            builder.CellFormat.Width = 21.6;	//0.4"
                            builder.CellFormat.VerticalMerge = CellMerge.None;
                            string factorName = "";
                            if (factorCodes[j - 1] == "a21026")
                                factorName = "SO<SUB>2</SUB>";
                            if (factorCodes[j - 1] == "a21004")
                                factorName = "NO<SUB>2</SUB>";
                            if (factorCodes[j - 1] == "a21005")
                                factorName = "CO";
                            if (factorCodes[j - 1] == "a05024")
                                factorName = "O<SUB>3</SUB>";
                            if (factorCodes[j - 1] == "a34002")
                                factorName = "PM<SUB>10</SUB>";
                            if (factorCodes[j - 1] == "a34004")
                                factorName = "PM<SUB>2.5</SUB>";
                            if (factorCodes[j - 1] == "a90969")
                                factorName = "Natural";
                            if (factorCodes[j - 1] == "a21028")
                                factorName = "H<SUB>2</SUB>S";
                            if (factorCodes[j - 1] == "a21001")
                                factorName = "NH<SUB>3</SUB>";
                            builder.InsertHtml(factorName);
                            builder.Font.ClearFormatting();
                        }
                    }
                    else if (i == 1)
                    {
                        if (j == 0)
                            builder.CellFormat.VerticalMerge = CellMerge.Previous;
                        else
                        {
                            builder.Font.ClearFormatting();
                            builder.Font.Size = 9;
                            builder.Font.Name = "楷体_GB2312";
                            builder.Font.Bold = true;
                            builder.RowFormat.Height = 20;
                            builder.CellFormat.Width = 21.6;	//0.4"
                            builder.CellFormat.VerticalMerge = CellMerge.None;
                            string factorUnit = "";
                            factorUnit = m_WaterPollutantService.GetPollutantInfo(factorCodes[j - 1]).PollutantMeasureUnit != null ? m_WaterPollutantService.GetPollutantInfo(factorCodes[j - 1]).PollutantMeasureUnit : "--";
                            if (factorUnit == "μg/m3")
                                factorUnit = "mg/m<SUP>3</SUP>";
                            else if (factorUnit == "mg/m3")
                                factorUnit = "mg/m<SUP>3</SUP>";
                            builder.InsertHtml(factorUnit);
                            builder.Font.ClearFormatting();
                        }
                    }
                    else if (i > 1 && i < 2 + rmypMonthTime.SelectedDate.Value.DaysInMonth())
                    {
                        builder.Font.ClearFormatting();
                        builder.Font.Size = 9;
                        builder.Font.Name = "楷体_GB2312";
                        builder.Font.ClearFormatting();
                        builder.RowFormat.Height = 15.0;
                        if (j == 0)
                        {
                            builder.Write(rmypMonthTime.SelectedDate.Value.AddDays(i - 2).ToString("dd-MM-yyyy"));
                        }
                        else
                        {
                            string value = "";
                            int num = Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo(factorCodes[j - 1]).PollutantDecimalNum);
                            DataRow[] dr = dt.Select("");
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                for (int k1 = 0; k1 < dt.Rows.Count; k1++)
                                {
                                    for (int k2 = 0; k2 < 2 + factorCodes.Length; k2++)
                                    {
                                        string factorName = "";
                                        if (factorCodes[j - 1] == "a21026")
                                            factorName = "SO2";
                                        if (factorCodes[j - 1] == "a21004")
                                            factorName = "NO2";
                                        if (factorCodes[j - 1] == "a21005")
                                        {
                                            factorName = "CO";
                                            num = 3;
                                        }
                                        if (factorCodes[j - 1] == "a05024")
                                            factorName = "O3";
                                        if (factorCodes[j - 1] == "a34002")
                                            factorName = "PM10";
                                        if (factorCodes[j - 1] == "a34004")
                                            factorName = "PM2.5";
                                        if (factorCodes[j - 1] == "a90969")
                                        {
                                            factorName = "Natural";
                                            num = 0;
                                        }
                                        if (factorCodes[j - 1] == "a21028")
                                        {
                                            factorName = "H2S";
                                        }
                                        if (factorCodes[j - 1] == "a21001")
                                        {
                                            factorName = "NH3";
                                        }
                                        if (dt.Rows[k1]["PointId"].ToString() == pointId && dt.Rows[k1]["DateTime"].ToString() == rmypMonthTime.SelectedDate.Value.AddDays(i - 2).ToString("dd-MM-yyyy") && dt.Columns[k2].ColumnName == factorName)
                                        {
                                            value = dt.Rows[k1][k2].ToString();
                                        }
                                    }
                                }
                            }
                            builder.Write(value == "" ? "--" : DecimalExtension.GetPollutantValue(Convert.ToDecimal(value), num).ToString());
                            //builder.Write(value == "" ? "--" : DecimalExtension.GetPollutantValue(Convert.ToDecimal(value), Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo(factorCodes[j - 1]).PollutantDecimalNum)).ToString());
                        }
                    }
                    else if (i >= 2 + rmypMonthTime.SelectedDate.Value.DaysInMonth())
                    {
                        builder.Font.ClearFormatting();
                        builder.Font.Size = 9;
                        builder.Font.Name = "楷体_GB2312";
                        //   builder.Font.Bold = true;
                        builder.Font.ClearFormatting();
                        builder.RowFormat.Height = 15.0;
                        if (j == 0)
                        {
                            builder.Write(name);
                        }
                        else
                        {
                            string value = "";
                            int num = Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo(factorCodes[j - 1]).PollutantDecimalNum);
                            if (dtNew != null && dtNew.Rows.Count > 0)
                            {
                                for (int k1 = 0; k1 < dtNew.Rows.Count; k1++)
                                {
                                    for (int k2 = 0; k2 < 2 + factorCodes.Length; k2++)
                                    {
                                        string factorName = "";
                                        if (factorCodes[j - 1] == "a21026")
                                            factorName = "SO2";
                                        if (factorCodes[j - 1] == "a21004")
                                            factorName = "NO2";
                                        if (factorCodes[j - 1] == "a21005")
                                        {
                                            factorName = "CO";
                                            num = 3;
                                        }
                                        if (factorCodes[j - 1] == "a05024")
                                            factorName = "O3";
                                        if (factorCodes[j - 1] == "a34002")
                                            factorName = "PM10";
                                        if (factorCodes[j - 1] == "a34004")
                                            factorName = "PM2.5";
                                        if (factorCodes[j - 1] == "a90969")
                                        {
                                            factorName = "Natural";
                                            num = 0;
                                        }
                                        if (factorCodes[j - 1] == "a21028")
                                        {
                                            factorName = "H2S";
                                        }
                                        if (factorCodes[j - 1] == "a21001")
                                        {
                                            factorName = "NH3";
                                        }
                                        if (dtNew.Rows[k1]["PointId"].ToString() == pointId && dtNew.Rows[k1]["DateTime"].ToString() == name && dtNew.Columns[k2].ColumnName == factorName)
                                        {
                                            value = dtNew.Rows[k1][k2].ToString();
                                        }
                                    }
                                }
                            }
                            builder.Write(value == "" ? "--" : DecimalExtension.GetPollutantValue(Convert.ToDecimal(value), num).ToString());
                        }
                    }
                }
                builder.EndRow();
            }

            builder.EndTable();
            if (Count != pointIds.Length - 1)
            {
                if (rmypMonthTime.SelectedDate.Value.DaysInMonth() == 31)
                    builder.Write("\n\n");
                if (rmypMonthTime.SelectedDate.Value.DaysInMonth() == 30)
                    builder.Write("\n\n\n");
                if (rmypMonthTime.SelectedDate.Value.DaysInMonth() == 29)
                    builder.Write("\n\n\n\n");
                if (rmypMonthTime.SelectedDate.Value.DaysInMonth() == 28)
                    builder.Write("\n\n\n\n\n");
            }
        }
        public string getSpace(int spaceNo)
        {
            string mySpace = "";
            for (int i = 0; i < spaceNo; i++)
            {
                mySpace += " ";
            }
            return mySpace;
        }
        /// <summary>
        /// 子站月报表
        /// </summary>
        /// <returns></returns>
        private DataTable GetMonthReportContent(string pointId, DateTime beginTime, DateTime endTime)
        {
            string factorCode = "";
            if (pointId == "9")
            {
                factorCode = System.Configuration.ConfigurationManager.AppSettings["AirPollutantCode1"];
            }
            else if (pointId == "33")
            {
                factorCode = System.Configuration.ConfigurationManager.AppSettings["AirPollutantCode2"];
            }
            else if (pointId == "34" || pointId == "38")
            {
                factorCode = System.Configuration.ConfigurationManager.AppSettings["AirPollutantCode3"];
            }
            else if (pointId == "35" || pointId == "36")
            {
                factorCode = System.Configuration.ConfigurationManager.AppSettings["AirPollutantCode4"];
            }
            else if (pointId == "39")
            {
                factorCode = System.Configuration.ConfigurationManager.AppSettings["AirPollutantCode5"];

            }
            else if (pointId == "24")
            {
                factorCode = System.Configuration.ConfigurationManager.AppSettings["AirPollutantCode6"];

            }
            string[] factorCodes = factorCode.Trim(';').Split(';');

            string[] portId = new string[1] { pointId };

            int recordCount = 0;
            DataTable dt = m_DayData.GetDayOfMonthNewPager(portId, factorCodes, beginTime, endTime, 99999, 0, out recordCount).ToTable();

            return dt;
        }
        /// <summary>
        /// 子站月报表
        /// </summary>
        /// <returns></returns>
        private DataTable GetAvgDayOfMonthData(string pointId, DateTime beginTime, DateTime endTime)
        {
            string factorCode = "";
            if (pointId == "9")
            {
                factorCode = System.Configuration.ConfigurationManager.AppSettings["AirPollutantCode1"];
            }
            else if (pointId == "33")
            {
                factorCode = System.Configuration.ConfigurationManager.AppSettings["AirPollutantCode2"];
            }
            else if (pointId == "34" || pointId == "38")
            {
                factorCode = System.Configuration.ConfigurationManager.AppSettings["AirPollutantCode3"];
            }
            else if (pointId == "35" || pointId == "36")
            {
                factorCode = System.Configuration.ConfigurationManager.AppSettings["AirPollutantCode4"];
            }
            else if (pointId == "39")
            {
                factorCode = System.Configuration.ConfigurationManager.AppSettings["AirPollutantCode5"];

            }
            else if (pointId == "24")
            {
                factorCode = System.Configuration.ConfigurationManager.AppSettings["AirPollutantCode6"];

            }
            string[] factorCodes = factorCode.Trim(';').Split(';');

            string[] portId = new string[1] { pointId };
            DataTable dtNew = m_DayData.GetAvgDayOfMonthNewData(portId, factorCodes, beginTime, endTime).ToTable();

            return dtNew;
        }
        /// <summary>
        /// 保存记录
        /// </summary>
        protected void save(Document doc)
        {
            // string[] strPointCodes = pointCbxRsm.GetPoints().Select(t => t.PointID).ToArray();//站点Code
            string[] strPointCodes = comboCity.CheckedItems.Select(t => t.Value).ToArray();
            string pointCodes = string.Join(";", strPointCodes);
            // string[] strPointNames = pointCbxRsm.GetPoints().Select(t => t.PointName).ToArray();//站点名称
            string[] strPointNames = comboCity.CheckedItems.Select(t => t.Text).ToArray();
            string pointNames = string.Join(";", strPointNames);
            //因子编码
            string factorCode = "a21026;a21004;a21005;a05024;a34002;a34004;a90969;a21028;a21001";
            //因子名称
            string factorName = "二氧化硫;二氧化氮;一氧化碳;臭氧;PM10;PM2.5;Natural;硫化氢;氨";

            string[] strFactorsName = factorName.Trim(';').Split(';');//因子名称
            string factorsName = string.Join(";", strFactorsName);
            string[] strFactorCodes = factorCode.Trim(';').Split(';'); //因子Code
            string factorCodes = string.Join(";", strFactorCodes);

            string filename = "(" + dt.ToString("yyyyMMddHHmmssffff") + ")" + "AvgDayOfMonthreportNew" + ".doc";
            string strTarget = Server.MapPath("../../../Pages/EnvWater/Report/ReportFile/AvgDayOfMonthreportNew/" + rmypMonthTime.SelectedDate.Value.Year + "/" + rmypMonthTime.SelectedDate.Value.Month + "/" + filename);
            //添加实体类对象
            ReportLogEntity customDatum = new ReportLogEntity();
            customDatum.PointIds = pointCodes;//测点Code
            customDatum.PointNames = pointNames;//测点名称
            customDatum.FactorCodes = factorCodes;//因子Code
            customDatum.FactorsNames = factorsName;//因子名称
            customDatum.DateTimeRange = beginTime.ToString("yyyy-MM-dd") + " ~ " + endTime.ToString("yyyy-MM-dd");
            customDatum.WaterOrAirType = 1;//水或气 0：水，1：气
            customDatum.PageTypeID = "AvgDayOfMonthreportNew";//页面ID
            customDatum.StartDateTime = Convert.ToDateTime(beginTime);
            customDatum.EndDateTime = Convert.ToDateTime(endTime);
            customDatum.CreatUser = this.ViewState["DisplayPerson"].ToString();
            customDatum.ExportName = "中意项日均值月报表" + string.Format("{0:yyyyMM}", endTime);
            customDatum.ReportName = ("../../../Pages/EnvWater/Report/ReportFile/AvgDayOfMonthreportNew/" + rmypMonthTime.SelectedDate.Value.Year + "/" + rmypMonthTime.SelectedDate.Value.Month + "/" + filename).ToString();
            customDatum.CreatDateTime = DateTime.Now;
            //添加数据
            ReportLogService.ReportLogAdd(customDatum);
            doc.Save(strTarget);
            if (!Directory.Exists(strTarget))
            {


                Alert("保存成功！");
                //Directory.CreateDirectory(strTarget);//创建新路径
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            GetDateRange();


            //站点Id
            // string[] pointIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
            string[] pointIds = comboCity.CheckedItems.Select(t => t.Value).ToArray();
            string factor = System.Configuration.ConfigurationManager.AppSettings["AirPollutantId"];
            string[] factors = factor.Trim(';').Split(';');

            Document doc = new Document(System.IO.Path.Combine(MapPath("DocumentsTemplet"), "AvgDayOfMonthreportNew.doc"));
            DocumentBuilder builder = new DocumentBuilder(doc);
            builder.MoveToMergeField("M1");
            for (int i = 0; i < pointIds.Length; i++)
            {
                builder.Font.ClearFormatting();
                DataTable dt = GetMonthReportContent(pointIds[i], beginTime, endTime);
                DataTable dtNew = GetAvgDayOfMonthData(pointIds[i], beginTime, endTime);
                MoveToMF3(builder, dt, dtNew, pointIds[i], i);
            }

            doc.MailMerge.DeleteFields();
            save(doc);
            Response.End();
        }

        protected void btnExport2_Click(object sender, ImageClickEventArgs e)
        {
            GetDateRange();

            //站点Id
            // string[] pointIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
            string[] pointIds = comboCity.CheckedItems.Select(t => t.Value).ToArray();

            string factor = System.Configuration.ConfigurationManager.AppSettings["AirPollutantId"];
            string[] factors = factor.Trim(';').Split(';');
            Document doc = new Document(System.IO.Path.Combine(MapPath("DocumentsTemplet"), "AvgDayOfMonthreportNew.doc"));
            DocumentBuilder builder = new DocumentBuilder(doc);
            builder.MoveToMergeField("M1");

            for (int i = 0; i < pointIds.Length; i++)
            {
                builder.Font.ClearFormatting();
                DataTable dt = GetMonthReportContent(pointIds[i], beginTime, endTime);
                DataTable dtNew = GetAvgDayOfMonthData(pointIds[i], beginTime, endTime);
                MoveToMF3(builder, dt, dtNew, pointIds[i], i);
            }

            doc.MailMerge.DeleteFields();
            save(doc);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "reload", "<script type=\"text/javascript\">self.parent.location.reload();</script>", false);
        }
    }
}