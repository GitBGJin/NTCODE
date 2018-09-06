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
    public partial class AvgDayOfMonthreport : SmartEP.WebUI.Common.BasePage
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

            //国控点，对照点，路边站
            MonitoringPointAirService m_MonitoringPointAirService = Singleton<MonitoringPointAirService>.GetInstance();
            string strpointName = "";
            IQueryable<MonitoringPointEntity> EnableOrNotports = m_MonitoringPointAirService.RetrieveAirMPListByEnable();
            string[] EnableOrNotportsarry = EnableOrNotports.Where(p => p.ContrlUid == "6fadff52-2338-4319-9f1d-7317823770ad").Select(p => p.MonitoringPointName).ToArray();
            foreach (string point in EnableOrNotportsarry)
            {
                strpointName += point + ";";
            }
            pointCbxRsm.SetPointValuesFromNames(strpointName);

            rmypMonthTime.SelectedDate = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy年MM月"));
            GetDateRange();
            lblRange.Text = beginTime.ToString("yyyy-MM-dd") + " ~ " + endTime.ToString("yyyy-MM-dd");

            //根据页面ID、水或气的类别获取数据
            var customData = customDataService.CustomDatumRetrieve("AvgDayOfMonthreport", 1);

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
            string[] pointIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
            //因子编码
            string factorCode = System.Configuration.ConfigurationManager.AppSettings["AirPollutantCode"];
            string[] factorCodes = factorCode.Trim(';').Split(';');

            string factor = System.Configuration.ConfigurationManager.AppSettings["AirPollutantId"];
            string[] factors = factor.Trim(';').Split(';');
            Document doc = new Document(System.IO.Path.Combine(MapPath("DocumentsTemplet"), "AvgDayOfMonthreport.doc"));
            DocumentBuilder builder = new DocumentBuilder(doc);

            builder.MoveToMergeField("M1");
            //builder.Font.ClearFormatting();
            for (int i = 0; i < pointIds.Length; i++)
            {
                builder.Font.ClearFormatting();
                DataTable dt = GetMonthReportContent(pointIds[i], factorCodes, factors, beginTime, endTime);
                DataTable dtNew = GetAvgDayOfMonthData(pointIds[i], factorCodes, factors, beginTime, endTime);
                MoveToMF3(builder, dt, dtNew, pointIds[i], factorCodes, i);

            }

            doc.MailMerge.DeleteFields();
            //doc.Save("AvgDayOfMonthreport" + ".doc", Aspose.Words.SaveFormat.Doc, SaveType.OpenInWord, this.Response);

            doc.Save(this.Response, "AvgDayOfMonthreport" + ".doc", ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
            save(doc);
            Response.End();


        }

        /// <summary>
        /// 填入子站月报表数据
        /// </summary>
        /// <param name="dt"></param>
        private void MoveToMF3(DocumentBuilder builder, DataTable dt, DataTable dtNew, string pointId, string[] factorCodes, int Count)
        {
            builder.ParagraphFormat.Alignment = Aspose.Words.ParagraphAlignment.Left;
            string pointName = monitoringPointWaterService.RetrieveEntityByPointId(Convert.ToInt16(pointId)).MonitoringPointName;
            //builder.Write("全部日均值月报表\n");
            //builder.ParagraphFormat.Alignment = Aspose.Words.ParagraphAlignment.Center;
            string writeIn = "子站名称：" + pointName;
            string dtTime = rmypMonthTime.SelectedDate.Value.ToString("yyyy-MM");
            //builder.Write(writeIn + "                                                                          " + dtTime);
            //builder.Font.Name = "楷体";
            //builder.Font.Size= 9;
            //builder.Bold = true;
            //builder.ParagraphFormat.Alignment = Aspose.Words.ParagraphAlignment.Center;

            //builder.Font.ClearFormatting();
            builder.ParagraphFormat.ClearFormatting();
            if (Count > 0)
            {
                builder.ParagraphFormat.PageBreakBefore = true;//段前分页
            }
            builder.Font.Size = 12;
            builder.Font.Name = "宋体";
            builder.Font.Bold = true;
            builder.Write("SEC\\QMS04-149-01\n");
            // builder.EndRow();
            builder.ParagraphFormat.ClearFormatting();
            builder.Font.ClearFormatting();
            builder.Font.Size = 14;
            builder.Font.Name = "宋体";
            builder.Font.Bold = true;
            builder.ParagraphFormat.Alignment = Aspose.Words.ParagraphAlignment.Center;
            // builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            // builder.RowFormat.Alignment = Word.WdRowAlignment.wdAlignRowCenter;
            builder.Write("全 部 日 均 值 月 报 表\n");

            builder.Font.ClearFormatting();
            builder.Font.Size = 9;
            builder.Font.Name = "楷体_GB2312";
            builder.Font.Bold = true;
            int lenX = 105;
            builder.Write(writeIn + getSpace(lenX - 3 - pointName.Length) + dtTime);
            builder.Font.ClearFormatting();

            //因子编码
            string factor = System.Configuration.ConfigurationManager.AppSettings["AirPollutantName"];
            string[] factorNames = factor.Trim(';').Split(';');
            //站点Id
            string[] pointIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
            int day = 0;
            for (int i = 0; i < 10 + rmypMonthTime.SelectedDate.Value.DaysInMonth(); i++)
            {
                string name = "";
                if (i == 2 + rmypMonthTime.SelectedDate.Value.DaysInMonth())
                    name = "运行的有效天数";
                if (i == 3 + rmypMonthTime.SelectedDate.Value.DaysInMonth())
                    name = "月(日)平均值";
                if (i == 4 + rmypMonthTime.SelectedDate.Value.DaysInMonth())
                    name = "最大日均值";
                if (i == 5 + rmypMonthTime.SelectedDate.Value.DaysInMonth())
                    name = "最大超标倍数";
                if (i == 6 + rmypMonthTime.SelectedDate.Value.DaysInMonth())
                    name = "日均值超标率(%)";
                if (i == 7 + rmypMonthTime.SelectedDate.Value.DaysInMonth())
                    name = "最小日均值";
                if (i == 8 + rmypMonthTime.SelectedDate.Value.DaysInMonth())
                    name = "全月最大一次值";
                if (i == 9 + rmypMonthTime.SelectedDate.Value.DaysInMonth())
                    name = "全月运行小时数";
                for (int j = 0; j < 1 + factorCodes.Length; j++)
                {
                    builder.InsertCell();
                    builder.CellFormat.Borders.LineStyle = LineStyle.Single;
                    builder.CellFormat.Borders.Color = System.Drawing.Color.Black;
                    builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                    builder.CellFormat.VerticalMerge = CellMerge.None;
                    // builder.CellFormat.Width = 100;

                    if (i == 0)
                    {

                        if (j == 0)
                        {
                            builder.Font.ClearFormatting();
                            builder.Font.Size = 9;
                            builder.Font.Name = "楷体_GB2312";
                            builder.Font.Bold = true;
                            builder.Font.ClearFormatting();
                            builder.RowFormat.Height = 30.0;
                            builder.CellFormat.VerticalMerge = CellMerge.First;
                            builder.Write("日期");
                            //  builder.CellFormat.Width = 150;
                        }
                        else
                        {
                            builder.Font.ClearFormatting();
                            builder.Font.Size = 9;
                            builder.Font.Name = "楷体_GB2312";
                            builder.Font.Bold = true;
                            builder.Font.ClearFormatting();
                            builder.RowFormat.Height = 30.0;
                            builder.CellFormat.Width = 21.6;	//0.4"
                            builder.CellFormat.VerticalMerge = CellMerge.None;
                            string factorName = "";
                            if (factorNames[j - 1] == "二氧化硫")
                                factorName = "SO<SUB>2</SUB>";
                            if (factorNames[j - 1] == "二氧化氮")
                                factorName = "NO<SUB>2</SUB>";
                            if (factorNames[j - 1] == "一氧化碳")
                                factorName = "CO";
                            if (factorNames[j - 1] == "臭氧1小时")
                                factorName = "O<SUB>3</SUB>-1h";
                            if (factorNames[j - 1] == "臭氧8小时")
                                factorName = "O<SUB>3</SUB>-8h";
                            if (factorNames[j - 1] == "PM10")
                                factorName = "PM<SUB>10</SUB>";
                            if (factorNames[j - 1] == "PM2.5")
                                factorName = "PM<SUB>2.5</SUB>";
                            if (factorNames[j - 1] == "温度")
                                factorName = "Temp";
                            if (factorNames[j - 1] == "湿度")
                                factorName = "RH";
                            if (factorNames[j - 1] == "气压")
                                factorName = "Press";
                            if (factorNames[j - 1] == "风向")
                                factorName = "Wd";
                            if (factorNames[j - 1] == "风速")
                                factorName = "Ws";
                            builder.InsertHtml(factorName);
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
                            builder.Font.ClearFormatting();
                            builder.RowFormat.Height = 30.0 / 2;
                            builder.CellFormat.Width = 21.6;	//0.4"
                            builder.CellFormat.VerticalMerge = CellMerge.None;
                            string factorUnit = "";
                            factorUnit = m_WaterPollutantService.GetPollutantInfo(factorCodes[j - 1]).PollutantMeasureUnit != null ? m_WaterPollutantService.GetPollutantInfo(factorCodes[j - 1]).PollutantMeasureUnit : "--";
                            if (factorUnit == "μg/m3")
                                factorUnit = "μg/m<SUP>3</SUP>";
                            else if (factorUnit == "mg/m3")
                                factorUnit = "mg/m<SUP>3</SUP>";
                            builder.InsertHtml(factorUnit);
                        }
                    }
                    else if (i > 1 && i < 2 + rmypMonthTime.SelectedDate.Value.DaysInMonth())
                    {
                        builder.Font.ClearFormatting();
                        builder.Font.Size = 9;
                        builder.Font.Name = "楷体_GB2312";
                        //   builder.Font.Bold = true;
                        builder.Font.ClearFormatting();
                        builder.RowFormat.Height = 15.0;
                        if (j == 0)
                        {
                            // builder.CellFormat.Width = 150;
                            builder.Write(rmypMonthTime.SelectedDate.Value.AddDays(i - 2).Day.ToString());
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
                                        string a = dt.Rows[k1]["PointId"].ToString();
                                        string b = dt.Rows[k1]["DA"].ToString();
                                        string c = rmypMonthTime.SelectedDate.Value.AddDays(i - 2).Day.ToString();
                                        string d = dt.Columns[k2].ColumnName;
                                        string factorName = "";
                                        if (factorNames[j - 1] == "二氧化硫")
                                            factorName = "SO2";
                                        if (factorNames[j - 1] == "二氧化氮")
                                            factorName = "NO2";
                                        if (factorNames[j - 1] == "一氧化碳")
                                            factorName = "CO";
                                        if (factorNames[j - 1] == "臭氧1小时")
                                            factorName = "O3-1h";
                                        if (factorNames[j - 1] == "臭氧8小时")
                                            factorName = "O3-8h";
                                        if (factorNames[j - 1] == "PM10")
                                            factorName = "PM10";
                                        if (factorNames[j - 1] == "PM2.5")
                                            factorName = "PM2.5";
                                        if (factorNames[j - 1] == "温度")
                                        {
                                            factorName = "Temp";
                                            num = 1;
                                        }
                                        if (factorNames[j - 1] == "湿度")
                                        {
                                            factorName = "RH";
                                            num = 0;
                                        }
                                        if (factorNames[j - 1] == "气压")
                                        {
                                            factorName = "Press";
                                            num = 0;
                                        }
                                        if (factorNames[j - 1] == "风向")
                                        {
                                            factorName = "Wd";
                                            num = 0;
                                        }
                                        if (factorNames[j - 1] == "风速")
                                        {
                                            factorName = "Ws";
                                            num = 1;
                                        }
                                        if (dt.Rows[k1]["PointId"].ToString() == pointId && dt.Rows[k1]["DA"].ToString() == rmypMonthTime.SelectedDate.Value.AddDays(i - 2).Day.ToString() && dt.Columns[k2].ColumnName == factorName)
                                        {
                                            if (dt.Rows[k1]["O3-8h"].ToString() == "" && factorName == "O3-1h")
                                            {
                                                value = "";
                                                if (dt.Rows[k1]["O3-1h"].ToString() != "")
                                                    day++;
                                            }
                                            else
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
                                        string a = dtNew.Rows[k1]["PointId"].ToString();
                                        string b = dtNew.Rows[k1]["DA"].ToString();
                                        string d = dtNew.Columns[k2].ColumnName;
                                        string factorName = "";
                                        if (factorNames[j - 1] == "二氧化硫")
                                            factorName = "SO2";
                                        if (factorNames[j - 1] == "二氧化氮")
                                            factorName = "NO2";
                                        if (factorNames[j - 1] == "一氧化碳")
                                            factorName = "CO";
                                        if (factorNames[j - 1] == "臭氧1小时")
                                            factorName = "O3-1h";
                                        if (factorNames[j - 1] == "臭氧8小时")
                                            factorName = "O3-8h";
                                        if (factorNames[j - 1] == "PM10")
                                            factorName = "PM10";
                                        if (factorNames[j - 1] == "PM2.5")
                                            factorName = "PM2.5";
                                        if (factorNames[j - 1] == "温度")
                                        {
                                            factorName = "Temp";
                                            num = 1;
                                        }
                                        if (factorNames[j - 1] == "湿度")
                                        {
                                            factorName = "RH";
                                            num = 0;
                                        }
                                        if (factorNames[j - 1] == "气压")
                                        {
                                            factorName = "Press";
                                            num = 0;
                                        }
                                        if (factorNames[j - 1] == "风向")
                                        {
                                            factorName = "Wd";
                                            num = 0;
                                        }
                                        if (factorNames[j - 1] == "风速")
                                        {
                                            factorName = "Ws";
                                            num = 1;
                                        }
                                        if (dtNew.Rows[k1]["PointId"].ToString() == pointId && dtNew.Rows[k1]["DA"].ToString() == name && dtNew.Columns[k2].ColumnName == factorName)
                                        {
                                            if (factorName == "O3-1h" && name == "运行的有效天数")
                                            {
                                                value = (dtNew.Rows[k1][k2] == DBNull.Value ? "" : (Convert.ToInt32(dtNew.Rows[k1][k2]) - day).ToString());
                                            }
                                            else
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
            //if (Count != pointIds.Length - 1)
            //{
            //    if (rmypMonthTime.SelectedDate.Value.DaysInMonth() == 31)
            //        builder.Write("\n");
            //    if (rmypMonthTime.SelectedDate.Value.DaysInMonth() == 30)
            //        builder.Write("\n\n");
            //    if (rmypMonthTime.SelectedDate.Value.DaysInMonth() == 29)
            //        builder.Write("\n\n\n");
            //    if (rmypMonthTime.SelectedDate.Value.DaysInMonth() == 28)
            //        builder.Write("\n\n\n\n");
            //}
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
        private DataTable GetMonthReportContent(string pointId, string[] factorCodes, string[] factors, DateTime beginTime, DateTime endTime)
        {
            string[] portId = new string[1] { pointId };

            int recordCount = 0;
            DataTable dt = m_DayData.GetDayOfMonthDataPager(portId, factorCodes, factors, beginTime, endTime, 99999, 0, out recordCount).ToTable();

            return dt;
        }
        /// <summary>
        /// 子站月报表
        /// </summary>
        /// <returns></returns>
        private DataTable GetAvgDayOfMonthData(string pointId, string[] factorCodes, string[] factors, DateTime beginTime, DateTime endTime)
        {
            string[] portId = new string[1] { pointId };
            DataTable dtNew = m_DayData.GetAvgDayOfMonthData(portId, factorCodes, factors, beginTime, endTime).ToTable();

            return dtNew;
        }
        /// <summary>
        /// 保存记录
        /// </summary>
        protected void save(Document doc)
        {
            string[] strPointCodes = pointCbxRsm.GetPoints().Select(t => t.PointID).ToArray();//站点Code
            string pointCodes = string.Join(";", strPointCodes);
            string[] strPointNames = pointCbxRsm.GetPoints().Select(t => t.PointName).ToArray();//站点名称
            string pointNames = string.Join(";", strPointNames);
            //因子编码
            string factorCode = System.Configuration.ConfigurationManager.AppSettings["AirPollutantCode"];
            //因子名称
            string factorName = System.Configuration.ConfigurationManager.AppSettings["AirPollutantName"];

            string[] strFactorsName = factorName.Trim(';').Split(';');//因子名称
            string factorsName = string.Join(";", strFactorsName);
            string[] strFactorCodes = factorCode.Trim(';').Split(';'); //因子Code
            string factorCodes = string.Join(";", strFactorCodes);

            string filename = "(" + dt.ToString("yyyyMMddHHmmssffff") + ")" + "AvgDayOfMonthreport" + ".doc";
            string strTarget = Server.MapPath("../../../Pages/EnvWater/Report/ReportFile/AvgDayOfMonthreport/" + rmypMonthTime.SelectedDate.Value.Year + "/" + rmypMonthTime.SelectedDate.Value.Month + "/" + filename);
            //添加实体类对象
            ReportLogEntity customDatum = new ReportLogEntity();
            customDatum.PointIds = pointCodes;//测点Code
            customDatum.PointNames = pointNames;//测点名称
            customDatum.FactorCodes = factorCodes;//因子Code
            customDatum.FactorsNames = factorsName;//因子名称
            customDatum.DateTimeRange = beginTime.ToString("yyyy-MM-dd") + " ~ " + endTime.ToString("yyyy-MM-dd");
            customDatum.WaterOrAirType = 1;//水或气 0：水，1：气
            customDatum.PageTypeID = "AvgDayOfMonthreport";//页面ID
            customDatum.StartDateTime = Convert.ToDateTime(beginTime);
            customDatum.EndDateTime = Convert.ToDateTime(endTime);
            customDatum.CreatUser = this.ViewState["DisplayPerson"].ToString();
            customDatum.ExportName = "市区日均值月报表" + string.Format("{0:yyyyMM}", endTime);
            customDatum.ReportName = ("../../../Pages/EnvWater/Report/ReportFile/AvgDayOfMonthreport/" + rmypMonthTime.SelectedDate.Value.Year + "/" + rmypMonthTime.SelectedDate.Value.Month + "/" + filename).ToString();
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
            string[] pointIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
            //因子编码
            string factorCode = System.Configuration.ConfigurationManager.AppSettings["AirPollutantCode"];
            string[] factorCodes = factorCode.Trim(';').Split(';');

            string factor = System.Configuration.ConfigurationManager.AppSettings["AirPollutantId"];
            string[] factors = factor.Trim(';').Split(';');

            Document doc = new Document(System.IO.Path.Combine(MapPath("DocumentsTemplet"), "AvgDayOfMonthreport.doc"));
            DocumentBuilder builder = new DocumentBuilder(doc);
            builder.MoveToMergeField("M1");
            for (int i = 0; i < pointIds.Length; i++)
            {
                builder.Font.ClearFormatting();
                DataTable dt = GetMonthReportContent(pointIds[i], factorCodes, factors, beginTime, endTime);
                DataTable dtNew = GetAvgDayOfMonthData(pointIds[i], factorCodes, factors, beginTime, endTime);
                MoveToMF3(builder, dt, dtNew, pointIds[i], factorCodes, i);
            }

            doc.MailMerge.DeleteFields();
            save(doc);
            Response.End();
        }

        protected void btnExport2_Click(object sender, ImageClickEventArgs e)
        {
            GetDateRange();

            //站点Id
            string[] pointIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
            //因子编码
            string factorCode = System.Configuration.ConfigurationManager.AppSettings["AirPollutantCode"];
            string[] factorCodes = factorCode.Trim(';').Split(';');
            string factor = System.Configuration.ConfigurationManager.AppSettings["AirPollutantId"];
            string[] factors = factor.Trim(';').Split(';');
            Document doc = new Document(System.IO.Path.Combine(MapPath("DocumentsTemplet"), "AvgDayOfMonthreport.doc"));
            DocumentBuilder builder = new DocumentBuilder(doc);
            builder.MoveToMergeField("M1");

            for (int i = 0; i < pointIds.Length; i++)
            {
                builder.Font.ClearFormatting();
                DataTable dt = GetMonthReportContent(pointIds[i], factorCodes, factors, beginTime, endTime);
                DataTable dtNew = GetAvgDayOfMonthData(pointIds[i], factorCodes, factors, beginTime, endTime);
                MoveToMF3(builder, dt, dtNew, pointIds[i], factorCodes, i);
            }

            doc.MailMerge.DeleteFields();
            save(doc);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "reload", "<script type=\"text/javascript\">self.parent.location.reload();</script>", false);
        }
    }
}