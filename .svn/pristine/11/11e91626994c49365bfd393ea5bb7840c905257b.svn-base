using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.ReportLibrary.Air;
using SmartEP.Service.BaseData.Channel;
using System.Data;
using SmartEP.Service.DataAnalyze.Air.DataQuery;
using Aspose.Cells;
using System.IO;
using SmartEP.Core.Generic;
using SmartEP.Utilities.IO;
using SmartEP.DomainModel.BaseData;
using System.Drawing.Imaging;
using Aspose.Cells.Rendering;
using Aspose.Words;
using Aspose.Words.Saving;
using Aspose.Words.Drawing;
using SmartEP.Service.DataAnalyze.Air.AQIReport;
using SmartEP.Core.Enums;
using System.Collections;
using SmartEP.Service.BaseData.Standard;
using SmartEP.Service.Core.Enums;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.Service.DataAnalyze.Report;
using Telerik.Web.UI;
using SmartEP.Service.DataAnalyze.Air.MonthReport;
using System.Configuration;
using System.Text;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System.Drawing;

namespace SmartEP.WebUI.Pages.EnvAir.Report
{
    public partial class AirMonthReport : SmartEP.WebUI.Common.BasePage
    {
        #region 属性
        MonitoringPointAirService pointAirService = new MonitoringPointAirService();
        AirQualityAutoMonthReportService reportService = new AirQualityAutoMonthReportService();
        AirPollutantService airPollutantService = new AirPollutantService();
        DataQueryByHourService dataByHourService = new DataQueryByHourService();
        MonitoringPointAirService g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
        DayAQIService dayAQI = new DayAQIService();
        DataQueryByDayService dayQuery = new DataQueryByDayService();
        DataQueryByHourService hourQuery = new DataQueryByHourService();
        DataQueryByMonthService monthQuery = new DataQueryByMonthService();
        MonthAQIService monthAQI = new MonthAQIService();
        EQIConcentrationService EQIService = new EQIConcentrationService();
        ReportLogService ReportLogService = new ReportLogService();
        DustHazeDayService dustService = new DustHazeDayService();
        string[] factors = { "二氧化硫", "二氧化氮", "氮氧化物", "可吸入颗粒", "细粒子", "臭氧8小时", "一氧化碳" };
        string[] factorCode = { "a21026", "a21004", "a21002", "a34002", "a34004", "a05024", "a21005" };

        string[] factorName = ("SO2;SO2;NO2;PM10;PM25;Max8HourO3;CO").Split(';');//解决单指标SO2图表显示乱的问题
        string[] factorNameCN = { "二氧化硫", "二氧化硫", "二氧化氮", "可吸入颗粒", "细粒子", "臭氧8小时", "一氧化碳" };//解决单指标SO2图表显示乱的问题
        string[] factorCodeChart = { "a21026", "a21026", "a21004", "a34002", "a34004", "a05024", "a21005" };

        string[] huimaiPointids = { "1", "2" };
        string[] huimaiFactor = { "a01020" };//能见度
        //string[] sheetName = ("ClassAnalyze;RegionAQI;DayData_SO2;DayData_NO2;DayData_PM10;DayData_PM25;DayData_O3;DayData_CO;VisibilityDayData;HuiMai").Split(';');
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitControl();//初始化控件
                //LoadingReport();//加载report
            }
        }

        #region 初始化

        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            dtpBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.Date.AddMonths(-1).ToString("yyyy-MM-01"));//初始化时间
            ViewState["pageTypeID"] = "MonthReportCollection";
            BindingPoint();//绑定测点
            GetGroupPoints();//获取点位分组情况
        }

        /// <summary>
        /// 绑定测点
        /// </summary>
        private void BindingPoint()
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
        }

        private void GetGroupPoints()
        {
            string[] portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
            string result = "";
            string replacePoint = "";
            string groups = ConfigurationManager.AppSettings["GroupPoints"] != null ? ConfigurationManager.AppSettings["GroupPoints"].ToString() : "";
            if (groups.Equals("")) ViewState["groupPoint"] = string.Join(",", portIds);
            else
            {
                foreach (string group in groups.Split(';'))
                {
                    replacePoint = "";
                    foreach (string portid in portIds)
                    {
                        if (group.Split(',').Contains(portid))
                            result += portid + ",";
                        else replacePoint += portid + ",";
                    }
                    result = result.Trim(',') + ";";
                    if (!replacePoint.Equals(""))
                        portIds = replacePoint.Trim(',').Split(',');
                    else break;
                }
                ViewState["groupPoint"] = result.Trim(';') + (!replacePoint.Equals("") ? ";" + replacePoint.Trim(',') : "");
            }
        }
        #endregion

        #region 事件
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void uploadData_Click(object sender, EventArgs e)
        {
            //RadScriptManager.RegisterStartupScript(this, GetType(), "ShowWebOffice", "<script>ShowWebOffice(escape('" + "空气月报201511.doc" + "'));</script>", false);

            DateTime timeBegin = dtpBegin.SelectedDate.Value.Date;
            string SavePath = Server.MapPath("../../../Files/TempFile/Word/AirMonthReport/DownLoad");
            string SaveFileName = "空气月报" + timeBegin.ToString("yyyyMM") + ".doc";
            if (File.Exists(System.IO.Path.Combine(SavePath + "/" + SaveFileName)))
            {
                Document doc = new Document(SavePath + "/" + SaveFileName);
                var docStream = new MemoryStream();
                doc.Save(this.Response, "AirMonthReport.doc", Aspose.Words.ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(Aspose.Words.SaveFormat.Doc));
            }
            else
            {
                string[] strPointCodes = pointCbxRsm.GetPoints().Select(t => t.PointID).ToArray();//站点Code
                StringBuilder fieldNames = new StringBuilder();
                StringBuilder fieldValues = new StringBuilder();
                DataTableToExcel(Server.MapPath("../../../Files/TempFile/Word/AirMonthReport/AirMonthTemp.xls"), Server.MapPath("../../../Files/TempFile/Word/AirMonthReport/"), "temp.xls", strPointCodes, out fieldNames, out fieldValues);
                DataTableToWord(strPointCodes, false, SavePath, SaveFileName, fieldNames, fieldValues);
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            DateTime timeBegin = dtpBegin.SelectedDate.Value.Date;
            if (ReportLogService.CustomDatumRetrieve("AirMonthReport", 1).Where(x => x.StartDateTime.Value >= timeBegin && x.EndDateTime.Value <= timeBegin.AddMonths(1).AddDays(-1)).Count() <= 0)
            {
                string[] strPointCodes = pointCbxRsm.GetPoints().Select(t => t.PointID).ToArray();//站点Code
                string pointCodes = string.Join(";", strPointCodes);
                string[] strPointNames = pointCbxRsm.GetPoints().Select(t => t.PointName).ToArray();//站点名称
                string pointNames = string.Join(";", strPointNames);
                StringBuilder fieldNames = new StringBuilder();
                StringBuilder fieldValues = new StringBuilder();

                string SavePath = Server.MapPath("../../../Files/TempFile/Word/AirMonthReport/DownLoad");
                string SaveFileName = "空气月报" + timeBegin.ToString("yyyyMM") + ".doc";

                DataTableToExcel(Server.MapPath("../../../Files/TempFile/Word/AirMonthReport/AirMonthTemp.xls"), Server.MapPath("../../../Files/TempFile/Word/AirMonthReport/"), "aa.xls", strPointCodes, out fieldNames, out fieldValues);
                DataTableToWord(strPointCodes, true, SavePath, SaveFileName, fieldNames, fieldValues);
                SaveToReport(pointCodes, pointNames, SaveFileName);
                Alert("保存成功！");
            }
            else
                Alert("该文件已经存在！");
        }

        /// <summary>
        /// 测点选择
        /// </summary>
        protected void pointCbxRsm_SelectedChanged()
        {
            GetGroupPoints();
        }
        #endregion

        #region 方法
        /// <summary>
        /// 保存报表实体
        /// </summary>
        private void SaveToReport(string pointCodes, string pointNames, string fileName)
        {
            DateTime timeBegin = dtpBegin.SelectedDate.Value.Date;
            DateTime timeEnd = timeBegin.AddMonths(1).AddDays(-1);

            //添加实体类对象
            ReportLogEntity customDatum = new ReportLogEntity();
            customDatum.PointIds = pointCodes;//测点Code
            customDatum.PointNames = pointNames;//测点名称
            customDatum.FactorCodes = string.Join(";", factorCode);//因子Code
            customDatum.FactorsNames = string.Join(";", factors);//因子名称
            customDatum.DateTimeRange = timeBegin.Year.ToString() + "年" + timeBegin.Month.ToString() + "月";
            customDatum.WaterOrAirType = 1;//水或气 0：水，1：气
            customDatum.PageTypeID = "AirMonthReport";//页面ID
            customDatum.StartDateTime = timeBegin;
            customDatum.EndDateTime = timeEnd;
            customDatum.CreatUser = Session["DisplayName"] != null ? Session["DisplayName"].ToString() : "";
            customDatum.ReportName = ("../../../Files/TempFile/Word/AirMonthReport/DownLoad/" + fileName).ToString();
            customDatum.CreatDateTime = DateTime.Now;
            //添加数据
            ReportLogService.ReportLogAdd(customDatum);
        }

        //<summary>
        //创建Excel导出文件
        //</summary>
        //<param name="portIds"></param>
        //<param name="dtData"></param>
        //<param name="CommonField"></param>
        //<param name="TempPath"></param>
        //<param name="SavePath"></param>
        //<param name="SaveFileName"></param>
        //<param name="IsBackup"></param>
        public void DataTableToExcel(string TempPath, string SavePath, string SaveFileName, string[] portIds, out StringBuilder fieldNames, out StringBuilder fieldValues)
        {
            DateTime timeBegin = dtpBegin.SelectedDate.Value.Date;
            DateTime timeEnd = timeBegin.AddMonths(1).AddDays(-1);

            fieldNames = new StringBuilder();
            fieldValues = new StringBuilder();
            //string Names = "";//存放数据以及图片
            //string Values = "";

            System.Web.HttpContext.Current.Response.Charset = "utf-8";
            System.Web.HttpContext.Current.Response.Buffer = true;
            System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
            //创建一个workbookdesigner对象
            WorkbookDesigner designer = new WorkbookDesigner();

            //制定报表模板
            string path = System.IO.Path.Combine(TempPath);
            designer.Open(path);

            if (designer.Workbook.Worksheets.Count > 0)
            {
                for (int i = 0; i < designer.Workbook.Worksheets.Count; i++)
                {
                    Worksheet sheet = designer.Workbook.Worksheets[i];
                    DataTable dataDT = new DataTable();

                    if (sheet.Name.Equals("ClassAnalyze"))//优良天数统计
                    {
                        #region 优良天数统计
                        #region 数据源
                        dataDT = dayAQI.GetGradeStatisticsMutilPoint(IAQIType.AQIValue, portIds, timeBegin, timeEnd).ToTable();
                        DataTable lastMonthData = dayAQI.GetGradeStatisticsMutilPoint(IAQIType.AQIValue, portIds, timeBegin.AddMonths(-1), timeEnd.AddMonths(-1)).ToTable();
                        dataDT.TableName = sheet.Name;
                        designer.SetDataSource(dataDT);//小时数据 
                        #endregion

                        if (dataDT.Rows.Count > 0)
                        {
                            #region Cell处理
                            int num = 0;
                            string level = "";
                            for (int j = 1; j < dataDT.Columns.Count; j++)
                            {
                                string columnName = dataDT.Columns[j].ColumnName;
                                if (columnName.Contains("Level1")) columnName = "优";
                                else if (columnName.Contains("Level2")) columnName = "良";
                                else if (columnName.Contains("Level3")) columnName = "轻度污染";
                                else if (columnName.Contains("Level4")) columnName = "中度污染";
                                else if (columnName.Contains("Level5")) columnName = "重度污染";
                                else if (columnName.Contains("Level6")) columnName = "严重污染";
                                else continue;
                                level += columnName + Convert.ToInt32(dataDT.Rows[0][j]) + "天、";

                                if (Convert.ToInt32(dataDT.Rows[0][j]) > 0)
                                {
                                    Cells cells = sheet.Cells;
                                    cells[0, num].PutValue(columnName);
                                    cells[1, num].PutValue("&=[" + sheet.Name + "]." + dataDT.Columns[j].ColumnName);
                                    num++;
                                }
                            }
                            if (num > 0)
                            {
                                sheet.Charts[0].NSeries[0].Values = sheet.Name + "!A2:" + GetCode((num + 64)) + "2";
                                sheet.Charts[0].NSeries[0].XValues = sheet.Name + "!A1:" + GetCode((num + 64)) + "1";
                                sheet.Charts[0].NSeries[0].DataLabels.Position = Aspose.Cells.Charts.LabelPositionType.OutsideEnd;
                            }
                            #endregion

                            #region 文字内容数据绑定
                            fieldNames.Append("MonthDate|");//月份
                            fieldValues.Append(timeBegin.ToString("yyyy年MM月") + "|");

                            fieldNames.Append(sheet.Name + "_PointCount|");//点位数
                            fieldValues.Append(portIds.Length + "|");

                            fieldNames.Append(sheet.Name + "_FineCount|");//达标天数
                            fieldValues.Append(dataDT.Rows[0]["FineCount"] + "|");

                            fieldNames.Append(sheet.Name + "_OverCount|");//超标天数
                            fieldValues.Append(dataDT.Rows[0]["OverCount"] + "|");

                            fieldNames.Append(sheet.Name + "_ComparePer|");//与上月对比达标比例  
                            int fineCount = Convert.ToInt32(dataDT.Rows[0]["FineCount"]);
                            int lastFineCount = Convert.ToInt32(lastMonthData.Rows[0]["FineCount"]);
                            string compare = "";
                            if (fineCount > lastFineCount)
                                compare = "上升";
                            if (fineCount < lastFineCount)
                                compare = "下降";
                            if (fineCount != lastFineCount)
                                fieldValues.Append((lastFineCount == 0 ? compare + "100" : compare + (Math.Abs(Convert.ToDecimal(fineCount - lastFineCount)) / lastFineCount).ToString("0.0")) + "个百分点|");
                            else
                                fieldValues.Append("持平|");

                            fieldNames.Append(sheet.Name + "_LevelDays|");//优良天数统计
                            fieldValues.Append(level.Trim('、') + "|");
                            fieldNames.Append(sheet.Name + "|");//超标图片
                            fieldValues.Append(SavePath + sheet.Name + ".png|");
                            #endregion
                        }

                        #region 生成图片
                        //根据数据源处理生成报表内容
                        designer.Process();
                        if (sheet.Charts.Count > 0)
                            sheet.Charts[0].ToImage(SavePath + sheet.Name + ".png", ImageFormat.Png);
                        #endregion
                        #endregion
                    }
                    else if (sheet.Name.Equals("RegionAQI"))//整体AQI
                    {
                        #region 整体AQI
                        #region 数据源
                        dataDT = dayAQI.GetMutilPointAQIData(portIds, timeBegin, timeEnd).ToTable();
                        dataDT.TableName = sheet.Name;
                        designer.SetDataSource(dataDT);
                        #endregion

                        #region Cell处理
                        Cells cells = sheet.Cells;
                        cells[1, 0].PutValue("&=[" + sheet.Name + "].DateTime");
                        cells[1, 1].PutValue("&=[" + sheet.Name + "].AQIValue");
                        sheet.Charts[0].NSeries[0].Values = sheet.Name + "!B2:B" + (dataDT.Rows.Count + 1);
                        sheet.Charts[0].NSeries[0].XValues = sheet.Name + "!A2:A" + (dataDT.Rows.Count + 1);
                        #endregion

                        #region 生成图片
                        //根据数据源处理生成报表内容
                        designer.Process();
                        if (sheet.Charts.Count > 0)
                            sheet.Charts[0].ToImage(SavePath + sheet.Name + ".png", ImageFormat.Png);
                        fieldNames.Append(sheet.Name + "|");//AQI图片
                        fieldValues.Append(SavePath + sheet.Name + ".png|");
                        #endregion
                        #endregion
                    }
                    else if (sheet.Name.Equals("DayData"))// 单选指标日均值
                    {
                        #region 单选指标日均值
                        string color = ConfigurationManager.AppSettings["SchedulerColors"] != null ? ConfigurationManager.AppSettings["SchedulerColors"].ToString() : "";
                        //数据源
                        dataDT = dayAQI.GetPortAllData(portIds, timeBegin, timeEnd).ToTable();
                        DataView dv = dataDT.DefaultView;
                        int index = 1;
                        foreach (string group in ViewState["groupPoint"].ToString().Split(';'))
                        {
                            foreach (string po in group.Split(','))
                            {
                                string pointName = g_MonitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(po)).MonitoringPointName;

                                #region 数据源处理
                                dv.RowFilter = "";
                                dv.RowFilter = "Pointid=" + po;
                                DataTable dt = dv.ToTable();
                                dt.TableName = sheet.Name + po;
                                designer.SetDataSource(dt);
                                #endregion

                                #region Cell处理
                                Cells cells = sheet.Cells;
                                //表头
                                cells[index, 0].PutValue("时间");
                                cells[index, 1].PutValue(pointName);
                                cells[index, 2].PutValue(pointName);
                                cells[index, 3].PutValue(pointName);
                                cells[index, 4].PutValue(pointName);
                                cells[index, 5].PutValue(pointName);
                                cells[index, 6].PutValue(pointName);
                                cells[index, 7].PutValue(pointName);

                                //数据
                                cells[index + 1, 0].PutValue("&=[" + dt.TableName + "].DateTime", true);
                                Aspose.Cells.Style style = cells[index + 1, 0].GetStyle();
                                //style.Number = 16;
                                //style.Custom = "yyyy/MM/d";
                                cells[index + 1, 0].SetStyle(style);

                                cells[index + 1, 1].PutValue("&=[" + dt.TableName + "].SO2");//SO2
                                cells[index + 1, 2].PutValue("&=[" + dt.TableName + "].SO2");//SO2
                                cells[index + 1, 3].PutValue("&=[" + dt.TableName + "].NO2");//NO2
                                cells[index + 1, 4].PutValue("&=[" + dt.TableName + "].PM10");//PM10
                                cells[index + 1, 5].PutValue("&=[" + dt.TableName + "].PM25");//PM2.5
                                cells[index + 1, 6].PutValue("&=[" + dt.TableName + "].Max8HourO3");//O3
                                cells[index + 1, 7].PutValue("&=[" + dt.TableName + "].CO");//CO                                               
                                #endregion
                                index = index + 2;
                            }
                        }
                        //根据数据源处理生成报表内容
                        designer.Process();

                        for (int j = 0; j < factorName.Length; j++)
                        {
                            index = 1;
                            int chartIndex = 1;
                            int groupIndex = 1;

                            #region 文字内容处理
                            if (j > 0)
                            {
                                DataView dvAvg = dayAQI.GetPointsAvgValue(portIds, timeBegin, timeEnd);
                                DataView dvAvgLastMonth = dayAQI.GetPointsAvgValue(portIds, timeBegin, timeEnd);
                                DataView dvAvgLastYear = dayAQI.GetPointsAvgValue(portIds, timeBegin, timeEnd);
                                string content = GetPollutantInfo(portIds.Length, factorName[j], factorCodeChart[j], j, ViewState["groupPoint"].ToString().Split(';').Length, dataDT, dvAvg.ToTable(), dvAvgLastMonth.ToTable(), dvAvgLastYear.ToTable());
                                fieldNames.Append("RangeDay_" + factorName[j] + "|");
                                fieldValues.Append(content.Split('|')[0] + "|");
                                fieldNames.Append("RangeMonth_" + factorName[j] + "|");
                                fieldValues.Append(content.Split('|')[1] + "|");
                                fieldNames.Append("Compare_" + factorName[j] + "|");
                                fieldValues.Append(content.Split('|')[2] + "|");
                            }
                            #endregion

                            #region 图表处理
                            foreach (string group in ViewState["groupPoint"].ToString().Split(';'))
                            {
                                int num = 0;
                                sheet.Charts[0].NSeries.Clear();
                                foreach (string po in group.Split(','))
                                {
                                    string pointName = g_MonitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(po)).MonitoringPointName;

                                    #region Series处理
                                    int count = (timeEnd - timeBegin).Days + 1;
                                    sheet.Charts[0].NSeries.Add(sheet.Name + "!" + "A" + (chartIndex + 1) + ":" + "A" + (chartIndex + 1) + "," + GetCode(j + 2 + 64) + (chartIndex + 1) + ":" + GetCode(j + 2 + 64) + (chartIndex + 1), false);
                                    sheet.Charts[0].NSeries[num].Name = pointName;
                                    sheet.Charts[0].NSeries[num].Values = sheet.Name + "!" + GetCode(j + 2 + 64) + (chartIndex + 2) + ":" + GetCode(j + 2 + 64) + (count + chartIndex + 1);
                                    if (!color.Equals("")&&color.Split(',').Length>num)
                                    {
                                        sheet.Charts[0].NSeries[num].Line.Color = ReturnColorFromString(color.Split(',')[num]);
                                        sheet.Charts[0].NSeries[num].MarkerBackgroundColor = ReturnColorFromString(color.Split(',')[num]);
                                        sheet.Charts[0].NSeries[num].MarkerForegroundColor = ReturnColorFromString(color.Split(',')[num]);
                                    }
                                    num++;
                                    index = index + 2;
                                    chartIndex = chartIndex + count + 1;
                                    #endregion
                                }
                                sheet.Charts[0].NSeries.CategoryData = sheet.Name + "!A3:A32";

                                #region 生成图片
                                //sheet.Charts[0].NSeries.CategoryData = Category.Trim(',');
                                sheet.Charts[0].Legend.Position = Aspose.Cells.Charts.LegendPositionType.Bottom;

                                if (sheet.Charts.Count > 0 && j > 0)
                                    sheet.Charts[0].ToImage(SavePath + sheet.Name + "_" + factorName[j] + "_" + groupIndex + ".png", ImageFormat.Png);
                                groupIndex++;
                                #endregion
                            }
                            #endregion
                        }
                        #endregion
                    }
                    else if (sheet.Name.Equals("VisibilityDayData")) //能见度
                    {
                        #region 能见度
                        DataView dvDay = dayQuery.GetDayStatisticalData(huimaiPointids, huimaiFactor, timeBegin, timeEnd);//日数据统计
                        DataView dvMonth = monthQuery.GetMonthStatisticalData(huimaiPointids, huimaiFactor, timeBegin.Year, timeBegin.Month, timeBegin.Year, timeBegin.Month);//月数据统计

                        for (int j = 0; j < huimaiPointids.Length; j++)
                        {
                            string pointName = g_MonitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(huimaiPointids[j])).MonitoringPointName;

                            #region 数据源
                            dataDT = hourQuery.GetHourStatisticalDataByDay(huimaiPointids[j].Split(','), huimaiFactor, timeBegin, timeEnd).ToTable();
                            dataDT.TableName = sheet.Name;
                            designer.ClearDataSource();
                            designer.SetDataSource(dataDT);
                            #endregion

                            #region 文字内容处理
                            dvDay.RowFilter = "";
                            dvDay.RowFilter = "Pointid=" + huimaiPointids[j];
                            dvMonth.RowFilter = "";
                            dvMonth.RowFilter = "Pointid=" + huimaiPointids[j];
                            string content = GetVisiblityInfo(pointName, dvDay.ToTable(), dvMonth.ToTable(), dataDT, timeBegin, timeEnd);
                            fieldNames.Append(sheet.Name + "_content_" + huimaiPointids[j] + "|");
                            fieldValues.Append(content + "|");
                            #endregion

                            #region Cell处理
                            Cells cells = sheet.Cells;
                            cells[1, 0].PutValue("&=[" + sheet.Name + "].Tstamp");
                            cells[1, 1].PutValue("&=[" + sheet.Name + "].Value_Max");
                            cells[1, 2].PutValue("&=[" + sheet.Name + "].Value_Min");
                            cells[1, 3].PutValue("&=[" + sheet.Name + "].Value_Avg");

                            //根据数据源处理生成报表内容
                            designer.Process();

                            sheet.Charts[0].NSeries[0].XValues = sheet.Name + "!A2:A" + (dataDT.Rows.Count + 1);
                            sheet.Charts[0].NSeries[0].Values = sheet.Name + "!B2:B" + (dataDT.Rows.Count + 1);
                            sheet.Charts[0].NSeries[1].XValues = sheet.Name + "!A2:A" + (dataDT.Rows.Count + 1);
                            sheet.Charts[0].NSeries[1].Values = sheet.Name + "!C2:C" + (dataDT.Rows.Count + 1);
                            sheet.Charts[0].NSeries[2].XValues = sheet.Name + "!A2:A" + (dataDT.Rows.Count + 1);
                            sheet.Charts[0].NSeries[2].Values = sheet.Name + "!D2:D" + (dataDT.Rows.Count + 1);
                            #endregion

                            #region 生成图片

                            if (sheet.Charts.Count > 0)
                                sheet.Charts[0].ToImage(SavePath + sheet.Name + "_" + huimaiPointids[j] + ".png", ImageFormat.Png);
                            #endregion
                        }
                        #endregion
                    }
                    else if (sheet.Name.Equals("HuiMai")) //灰霾
                    {
                        #region 灰霾
                        DataView dvMonth = dustService.GetDustHazeDayStatistical(huimaiPointids, timeBegin, timeEnd);
                        DataView dvLastMonth = dustService.GetDustHazeDayStatistical(huimaiPointids, timeBegin.AddMonths(-1), timeEnd.AddMonths(-1));

                        for (int j = 0; j < huimaiPointids.Length; j++)
                        {
                            string pointName = g_MonitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(huimaiPointids[j])).MonitoringPointName;

                            #region 数据源
                            dvMonth.RowFilter = "";
                            dvMonth.RowFilter = "Pointid=" + huimaiPointids[j];
                            dvLastMonth.RowFilter = "";
                            dvLastMonth.RowFilter = "Pointid=" + huimaiPointids[j];
                            dataDT = dvMonth.ToTable();
                            dataDT.TableName = sheet.Name;
                            //designer.ClearDataSource();
                            //designer.SetDataSource(dataDT);
                            #endregion

                            #region 文字内容处理
                            string content = GetHuiMaiInfo(pointName, dvMonth.ToTable(), dvLastMonth.ToTable(), timeBegin, timeEnd);
                            fieldNames.Append(sheet.Name + "_content_" + huimaiPointids[j] + "|");
                            fieldValues.Append(content + "|");
                            #endregion

                            #region Cell处理
                            if (dataDT.Rows.Count > 0)
                            {
                                int num = 0;
                                for (int k = 1; k < dataDT.Columns.Count; k++)
                                {
                                    string columnName = dataDT.Columns[k].ColumnName;
                                    if (columnName.Equals("NODustHaze")) columnName = "非灰霾天";
                                    else if (columnName.Equals("Grade1")) columnName = "轻微灰霾";
                                    else if (columnName.Equals("Grade2")) columnName = "轻度灰霾";
                                    else if (columnName.Equals("Grade3")) columnName = "中度灰霾";
                                    else if (columnName.Equals("Grade4")) columnName = "重度灰霾";
                                    else continue;
                                    if (Convert.ToInt32(dataDT.Rows[0][k]) > 0)
                                    {
                                        Cells cells = sheet.Cells;
                                        cells[0, num].PutValue(columnName);
                                        //cells[1, num].PutValue("&=[" + sheet.Name + "]." + dataDT.Columns[k].ColumnName);
                                        cells[1, num].PutValue(Convert.ToInt32(dataDT.Rows[0][k]));
                                        num++;
                                    }
                                }
                                if (num > 0)
                                {
                                    sheet.Charts[0].NSeries[0].Values = sheet.Name + "!A2:" + GetCode((num + 64)) + "2";
                                    sheet.Charts[0].NSeries[0].XValues = sheet.Name + "!A1:" + GetCode((num + 64)) + "1";
                                    sheet.Charts[0].NSeries[0].DataLabels.Position = Aspose.Cells.Charts.LabelPositionType.OutsideEnd;
                                }
                                else
                                {
                                    sheet.Charts[0].NSeries.Clear();
                                }
                            }
                            #endregion

                            #region 生成图片
                            //designer.Process();
                            if (sheet.Charts.Count > 0)
                            {
                                if (dataDT.Rows.Count <= 0) sheet.Charts[0].NSeries.Clear();
                                sheet.Charts[0].ToImage(SavePath + sheet.Name + "_" + huimaiPointids[j] + ".png", ImageFormat.Png);
                            }
                            fieldNames.Append(sheet.Name + "_" + huimaiPointids[j] + "|");//AQI图片
                            fieldValues.Append(SavePath + sheet.Name + "_" + huimaiPointids[j] + ".png|");
                            #endregion
                        }
                        #endregion
                    }

                    //fieldNames += Names;
                    //fieldValues += Values;
                }

                fieldNames = new StringBuilder(fieldNames.ToString().Trim('|'));
                fieldValues = new StringBuilder(fieldValues.ToString().Trim('|'));
            }

            #region 保存Excel文件
            string fileToSave = System.IO.Path.Combine(SavePath + "/" + SaveFileName);
            if (File.Exists(fileToSave))
            {
                File.Delete(fileToSave);
            }
            else
            {
                FileManager.CreateFile(SaveFileName, SavePath);
            }
            designer.Save(fileToSave, FileFormatType.Excel2003);
            #endregion
        }

        /// <summary>
        /// 获取单项指标文字说明
        /// </summary>
        /// <param name="pointCount"></param>
        /// <param name="factor"></param>
        /// <param name="factorCode"></param>
        /// <param name="factorIndex"></param>
        /// <param name="groupNum"></param>
        /// <param name="dtDay"></param>
        /// <param name="dtMonth"></param>
        /// <param name="dtLastMonth"></param>
        /// <param name="dtlastYear"></param>
        /// <returns></returns>
        public string GetPollutantInfo(int pointCount, string factor, string factorCode, int factorIndex, int groupNum, DataTable dtDay, DataTable dtMonth, DataTable dtLastMonth, DataTable dtlastYear)
        {
            string content = "";
            EQIConcentrationLimitEntity limitTwo = new EQIConcentrationLimitEntity();
            if (!factor.Equals("Max8HourO3"))
                limitTwo = EQIService.RetrieveAirConcentration(AQIClass.Moderate, factorCode, EQITimeType.TwentyFour);//超标限值
            else
                limitTwo = EQIService.RetrieveAirConcentration(AQIClass.Moderate, factorCode, EQITimeType.Eight);//超标限值

            #region 日均值
            string max = dtDay.Compute("Max(" + factor + ")", "") != DBNull.Value ? dtDay.Compute("Max(" + factor + ")", "").ToString() : "";
            string min = dtDay.Compute("Min(" + factor + ")", "") != DBNull.Value ? dtDay.Compute("Min(" + factor + ")", "").ToString() : "";
            string maxinfo = "";
            string over = "";
            string imageNO = "";
            if (!max.Equals(""))
            {
                #region 获取最高值出现的点位、日期
                DataRow[] rows = dtDay.Select(factor + "=" + max);
                foreach (DataRow row in rows)
                {
                    string pointName = g_MonitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(row["Pointid"])).MonitoringPointName;
                    maxinfo += Convert.ToDateTime(row["Datetime"]).ToString("dd日") + pointName + "、";
                }
                if (!maxinfo.Equals("")) maxinfo = ",最高值出现在" + maxinfo.Trim('、');
                #endregion

                #region 获取超标倍数
                decimal baseValue = 0;
                if (limitTwo != null)
                {
                    if (Convert.ToDecimal(max) < limitTwo.Low)
                        baseValue = limitTwo.Low.Value;
                    else if (Convert.ToDecimal(max) > limitTwo.Upper)
                        baseValue = limitTwo.Upper.Value;
                }
                if (baseValue != 0)
                {
                    over = ", 超标" + System.Math.Abs(((Convert.ToDecimal(Convert.ToDouble(max)) - baseValue) / baseValue)).ToString("0.0") + "倍";
                }
                #endregion
            }

            #region 所见图表名称
            for (int i = 0; i < groupNum; i++)
            {
                imageNO += "图" + factorIndex + GetCode(i + 97) + "、";
            }
            #endregion
            if (!factor.Equals("Max8HourO3"))
                content += string.Format(@"{0}个测点{1}日均浓度范围为{2}～{3} {4}{5} {6}。"
                    , pointCount, factor.Replace("Max8HourO3", "O3"), min, max, maxinfo, !over.Equals("") ? over : ",各测点日均浓度均达标", "(见" + imageNO.Trim('、') + ")");
            else
                content += string.Format(@"{0}个测点臭氧8小时滚动最大值范围为{2}～{3} {4}{5} {6}。"
               , pointCount, factor.Replace("Max8HourO3", "O3"), min, max, maxinfo, !over.Equals("") ? over : ",各测点日均浓度均达标", "(见" + imageNO.Trim('、') + ")");
            #endregion

            if (!factor.Equals("Max8HourO3"))
            {
                #region 月均值
                max = dtMonth.Compute("Max(" + factor + ")", "") != DBNull.Value ? DecimalExtension.GetPollutantValue(Convert.ToDecimal(dtMonth.Compute("Max(" + factor + ")", "")), 3).ToString() : "";
                min = dtMonth.Compute("Min(" + factor + ")", "") != DBNull.Value ? DecimalExtension.GetPollutantValue(Convert.ToDecimal(dtMonth.Compute("Min(" + factor + ")", "")), 3).ToString() : "";
                maxinfo = "";
                over = "";
                if (!max.Equals(""))
                {
                    #region 获取最高值出现的点位、日期
                    DataRow[] rows = dtMonth.Select(factor + "=" + max);
                    foreach (DataRow row in rows)
                    {
                        string pointName = g_MonitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(row["Pointid"])).MonitoringPointName;
                        maxinfo += pointName + "、";
                    }
                    if (!maxinfo.Equals("")) maxinfo = ",最高值出现在" + maxinfo.Trim('、');
                    #endregion

                    #region 获取超标点位
                    if (limitTwo != null)
                    {
                        if (Convert.ToDecimal(max) >= limitTwo.Low && Convert.ToDecimal(max) < limitTwo.Upper || !min.Equals("") && Convert.ToDecimal(min) >= limitTwo.Low && Convert.ToDecimal(min) < limitTwo.Upper)
                        {
                        }
                        else
                        {
                            string notoverPoint = "";
                            foreach (DataRow row in dtMonth.Rows)
                            {
                                if (row[factor] != DBNull.Value)
                                {
                                    if (Convert.ToDecimal(row[factor]) >= limitTwo.Low || Convert.ToDecimal(row[factor]) < limitTwo.Upper)
                                    {
                                        string pointName = g_MonitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(row["Pointid"])).MonitoringPointName;
                                        notoverPoint += pointName + ";";
                                    }
                                }
                                notoverPoint = notoverPoint.Trim(';');
                                if (!notoverPoint.Equals("") && notoverPoint.Split(';').Length == dtMonth.Rows.Count)
                                    over = "，各测点月均浓度均达标";
                                else if (!notoverPoint.Equals(""))
                                    over = string.Format("，除{0}外，其余均超标", notoverPoint.Replace(";", "、"));
                                else
                                    over = "，各测点月均浓度均未达标";
                            }
                        }
                    }
                    else over = "，各测点月均浓度均达标";

                    #endregion
                }
                content += "|" + string.Format(@"{0}个测点{1}月均浓度范围为{2}～{3} {4} 。"
                    , pointCount, factor.Replace("Max8HourO3", "O3"), min, max, maxinfo, over);
                #endregion
            }
            else
            {
                #region 日均值
                limitTwo = EQIService.RetrieveAirConcentration(AQIClass.Moderate, factorCode, EQITimeType.One);//超标限值
                max = dtDay.Compute("Max(" + factor + ")", "") != DBNull.Value ? dtDay.Compute("Max(" + factor + ")", "").ToString() : "";
                min = dtDay.Compute("Min(" + factor + ")", "") != DBNull.Value ? dtDay.Compute("Min(" + factor + ")", "").ToString() : "";
                maxinfo = "";
                over = "";
                imageNO = "";
                if (!max.Equals(""))
                {
                    #region 获取最高值出现的点位、日期
                    DataRow[] rows = dtDay.Select(factor + "=" + max);
                    foreach (DataRow row in rows)
                    {
                        string pointName = g_MonitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(row["Pointid"])).MonitoringPointName;
                        maxinfo += Convert.ToDateTime(row["Datetime"]).ToString("dd日") + pointName + "、";
                    }
                    if (!maxinfo.Equals("")) maxinfo = ",最高值出现在" + maxinfo.Trim('、');
                    #endregion

                    #region 获取超标倍数
                    decimal baseValue = 0;
                    if (limitTwo != null)
                    {
                        if (Convert.ToDecimal(max) < limitTwo.Low)
                            baseValue = limitTwo.Low.Value;
                        else if (Convert.ToDecimal(max) > limitTwo.Upper)
                            baseValue = limitTwo.Upper.Value;
                    }
                    if (baseValue != 0)
                    {
                        over = ", 超标" + System.Math.Abs(((Convert.ToDecimal(Convert.ToDouble(max)) - baseValue) / baseValue)).ToString("0.0") + "倍";
                    }
                    #endregion
                }
                content += "|" + string.Format(@"臭氧1小时最大值范围为{0}～{1} {2}{3}。"
                    , min, max, maxinfo, !over.Equals("") ? over : ",各测点日均浓度均达标");
                #endregion
            }

            #region 比较日均值、月均值
            string monthPointASC = "";
            string monthPointDESC = "";
            string yearPointASC = "";
            string yearPointDESC = "";
            string monthContent = "";
            string yearContent = "";
            for (int i = 0; i < dtMonth.Rows.Count; i++)
            {
                string pointName = g_MonitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(dtMonth.Rows[i]["Pointid"])).MonitoringPointName;
                if (dtLastMonth.Rows[i][factor] != DBNull.Value && dtMonth.Rows[i][factor] != DBNull.Value)
                {
                    if (Convert.ToDouble(dtMonth.Rows[i][factor]) > Convert.ToDouble(dtLastMonth.Rows[i][factor]))
                        monthPointASC += pointName + "、";
                    else if (Convert.ToDouble(dtMonth.Rows[i][factor]) < Convert.ToDouble(dtLastMonth.Rows[i][factor]))
                        monthPointDESC += pointName + "、";
                }

                if (dtlastYear.Rows[i][factor] != DBNull.Value && dtMonth.Rows[i][factor] != DBNull.Value)
                {
                    if (Convert.ToDouble(dtMonth.Rows[i][factor]) > Convert.ToDouble(dtlastYear.Rows[i][factor]))
                        yearPointASC += pointName + "、";
                    else if (Convert.ToDouble(dtMonth.Rows[i][factor]) < Convert.ToDouble(dtlastYear.Rows[i][factor]))
                        yearPointDESC += pointName + "、";
                }
            }
            monthPointASC = monthPointASC.Trim('、');
            monthPointDESC = monthPointDESC.Trim('、');
            yearPointASC = yearPointASC.Trim('、');
            yearPointDESC = yearPointDESC.Trim('、');
            #region 月文字说明
            if (monthPointASC.Equals("") && monthPointDESC.Equals(""))
                monthContent = "所有点位持平";
            else if (!monthPointASC.Equals("") && !monthPointDESC.Equals(""))
            {
                if (monthPointASC.Split('、').Length + monthPointDESC.Split('、').Length == dtMonth.Rows.Count)
                    monthContent = "," + monthPointASC.Trim('、') + "有所上升，" + monthPointDESC.Trim('、') + "有所下降";
                else
                    monthContent = "," + monthPointASC.Trim('、') + "有所上升，" + monthPointDESC.Trim('、') + "有所下降，其他点位持平";

            }
            else if (!monthPointASC.Equals(""))
            {
                if (monthPointASC.Split('、').Length + monthPointDESC.Split('、').Length == dtMonth.Rows.Count)
                    monthContent = ",所有点位均有所上升";
                else
                    monthContent = "," + monthPointASC.Trim('、') + "有所上升，其他点位持平";
            }
            else if (!monthPointDESC.Equals(""))
            {
                if (monthPointASC.Split('、').Length + monthPointDESC.Split('、').Length == dtMonth.Rows.Count)
                    monthContent = ",所有点位均有所下降";
                else
                    monthContent = "," + monthPointDESC.Trim('、') + "有所下降，其他点位持平";
            }
            #endregion

            #region 年文字说明
            if (yearPointASC.Equals("") && yearPointDESC.Equals(""))
                yearContent = "所有点位持平";
            else if (!yearPointASC.Equals("") && !yearPointDESC.Equals(""))
            {
                if (yearPointASC.Split('、').Length + yearPointDESC.Split('、').Length == dtMonth.Rows.Count)
                    yearContent = "," + yearPointASC.Trim('、') + "有所上升，" + yearPointDESC.Trim('、') + "有所下降";
                else
                    yearContent = "," + yearPointASC.Trim('、') + "有所上升，" + yearPointDESC.Trim('、') + "有所下降，其他点位持平";

            }
            else if (!yearPointASC.Equals(""))
            {
                if (yearPointASC.Split('、').Length + yearPointDESC.Split('、').Length == dtMonth.Rows.Count)
                    yearContent = ",所有点位均有所上升";
                else
                    yearContent = "," + yearPointDESC.Trim('、') + "有所上升，其他点位持平";
            }
            else if (!monthPointDESC.Equals(""))
            {
                if (yearPointASC.Split('、').Length + yearPointDESC.Split('、').Length == dtMonth.Rows.Count)
                    yearContent = ",所有点位均有所下降";
                else
                    yearContent = "," + yearPointDESC.Trim('、') + "有所下降，其他点位持平";
            }
            #endregion
            content += "|" + string.Format(@"与上月相比{0}{1}"
               , monthContent, yearContent);
            #endregion
            return content;
        }

        /// <summary>
        /// 能见度文字处理
        /// </summary>
        /// <param name="PointName"></param>
        /// <param name="dtDay"></param>
        /// <param name="dtMonth"></param>
        /// <param name="dtHour"></param>
        /// <param name="timeBegin"></param>
        /// <param name="timeEnd"></param>
        /// <returns></returns>
        public string GetVisiblityInfo(string PointName, DataTable dtDay, DataTable dtMonth, DataTable dtHour, DateTime timeBegin, DateTime timeEnd)
        {
            string content = "";

            #region 日均值最小值日期
            string mininfo = "";
            if (dtHour.Compute("min(Value_Avg)", "") != DBNull.Value)
            {
                decimal min = Convert.ToDecimal(dtHour.Compute("min(Value_Avg)", ""));
                DataRow[] rows = dtHour.Select("Value_Avg=" + min);
                foreach (DataRow row in rows)
                {
                    mininfo += Convert.ToDateTime(row["Tstamp"]).ToString("MM月dd日") + "、";
                }
            }
            #endregion

            #region 能见度统计
            int count1 = 0;
            int count2 = 0;
            int count3 = 0;
            if (dtHour.Compute("count(Tstamp)", "Value_Avg<5000") != DBNull.Value)
            {
                count1 = Convert.ToInt32(dtHour.Compute("count(Tstamp)", "Value_Avg<5000"));
            }
            if (dtHour.Compute("count(Tstamp)", "Value_Avg>=5000 and Value_Avg<=10000") != DBNull.Value)
            {
                count2 = Convert.ToInt32(dtHour.Compute("count(Tstamp)", "Value_Avg>=5000 and Value_Avg<=10000"));
            }
            if (dtHour.Compute("count(Tstamp)", "Value_Avg>10000") != DBNull.Value)
            {
                count3 = Convert.ToInt32(dtHour.Compute("count(Tstamp)", "Value_Avg>10000"));
            }
            #endregion

            content = string.Format(@"{0}本月能见度有效监测天数为{1}天，月均能见度为{2}，日均能见度数值范围为{3}，最小值出现在{4}。从整月能见度变化趋势看，{5}月份能见度整体较差。整月中，日均能见度小于5000米的有{6}天，介于5000、10000米之间的{7}天，大于10000米的{8}天。"
                         , PointName
                         , (timeEnd - timeBegin).Days + 1
                         , dtMonth.Rows.Count > 0 && dtMonth.Rows[0]["Value_Avg"] != DBNull.Value ? DecimalExtension.GetPollutantValue(Convert.ToDecimal(dtMonth.Rows[0]["Value_Avg"].ToString()), 1) + "米" : "--"
                         , dtDay.Rows.Count > 0 && dtDay.Rows[0]["Value_Min"] != DBNull.Value && dtDay.Rows[0]["Value_Max"] != DBNull.Value ? DecimalExtension.GetPollutantValue(Convert.ToDecimal(dtDay.Rows[0]["Value_Min"].ToString()), 0) + "~" + DecimalExtension.GetPollutantValue(Convert.ToDecimal(dtDay.Rows[0]["Value_Max"].ToString()), 0) + "米" : "--"
                         , !mininfo.Equals("") ? mininfo.Trim('、') : "--"
                         , timeBegin.Month
                         , count1
                         , count2
                         , count3);
            return content;
        }

        /// <summary>
        /// 灰霾文字处理
        /// </summary>
        /// <param name="PointName"></param>
        /// <param name="dtMonth"></param>
        /// <param name="dtLastMonth"></param>
        /// <param name="timeBegin"></param>
        /// <param name="timeEnd"></param>
        /// <returns></returns>
        public string GetHuiMaiInfo(string PointName, DataTable dtMonth, DataTable dtLastMonth, DateTime timeBegin, DateTime timeEnd)
        {
            string content = "";

            double IsDustHaze = dtMonth.Rows.Count > 0 && dtMonth.Rows[0]["IsDustHaze"] != DBNull.Value ? Convert.ToDouble(dtMonth.Rows[0]["IsDustHaze"].ToString()) : 0;
            #region 灰霾等级
            string grades = "";
            if (dtMonth.Rows.Count > 0)
            {
                if (dtMonth.Rows[0]["grade1"] != DBNull.Value && Convert.ToInt32(dtMonth.Rows[0]["grade1"]) != 0)
                    grades += dtMonth.Rows[0]["grade1"] + "天轻微灰霾,";
                if (dtMonth.Rows[0]["grade2"] != DBNull.Value && Convert.ToInt32(dtMonth.Rows[0]["grade2"]) != 0)
                    grades += dtMonth.Rows[0]["grade2"] + "天轻度灰霾,";
                if (dtMonth.Rows[0]["grade3"] != DBNull.Value && Convert.ToInt32(dtMonth.Rows[0]["grade3"]) != 0)
                    grades += dtMonth.Rows[0]["grade3"] + "天中度灰霾,";
                if (dtMonth.Rows[0]["grade4"] != DBNull.Value && Convert.ToInt32(dtMonth.Rows[0]["grade4"]) != 0)
                    grades += dtMonth.Rows[0]["grade4"] + "天重度灰霾,";
                if (!grades.Equals("")) grades = "根据灰霾等级统计，本月灰霾天有" + grades.Trim(',') + "。";
            }
            #endregion
            #region 与上月相比
            string compare = "";
            if (dtMonth.Rows.Count > 0 && dtLastMonth.Rows.Count > 0)
            {
                if (dtMonth.Rows[0]["IsDustHaze"] != DBNull.Value && dtLastMonth.Rows[0]["IsDustHaze"] != DBNull.Value)
                {
                    if (Convert.ToInt32(dtMonth.Rows[0]["IsDustHaze"]) > Convert.ToInt32(dtLastMonth.Rows[0]["IsDustHaze"]))
                        compare = "与上月相比，本月灰霾天数上升。";
                    else if (Convert.ToInt32(dtMonth.Rows[0]["IsDustHaze"]) < Convert.ToInt32(dtLastMonth.Rows[0]["IsDustHaze"]))
                        compare = "与上月相比，本月灰霾天数下降。";
                    else
                        compare = "与上月相比，本月灰霾天数持平。";
                }
            }
            #endregion

            content = string.Format(@"{0}本月灰霾有效监测天数{1}天。根据监测结果，本月非灰霾天{2}天，灰霾天{3}天，占有效监测天数的{4}。{5}{6}"
                         , PointName
                         , (timeEnd - timeBegin).Days + 1
                         , dtMonth.Rows.Count > 0 && dtMonth.Rows[0]["NODustHaze"] != DBNull.Value ? dtMonth.Rows[0]["NODustHaze"].ToString() : "0"
                         , dtMonth.Rows.Count > 0 && dtMonth.Rows[0]["IsDustHaze"] != DBNull.Value ? dtMonth.Rows[0]["IsDustHaze"].ToString() : "0"
                         , (IsDustHaze / ((timeEnd - timeBegin).Days) * 100).ToString("0.0") + "%"
                         , grades, compare);
            return content;
        }


        /// <summary>
        /// 根据ASCII码获取字母
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private string GetCode(int num)
        {
            byte[] array = new byte[1];
            array[0] = (byte)(num); //ASCII码强制转换二进制
            string code = Convert.ToString(System.Text.Encoding.ASCII.GetString(array));
            return code;
        }

        public void DataTableToWord(string[] portIds, bool IsBackup, string SavePath, string SaveFileName, StringBuilder fieldNames, StringBuilder fieldValues)
        {
            Document doc = new Document(System.IO.Path.Combine(MapPath("../../../Files/TempFile/Word/AirMonthReport/"), "AirMonthTemp.doc"));


            DocumentBuilder builder = new DocumentBuilder(doc);

            ToMonthAnalazy(builder, doc, portIds);//月均值word数据源处理
            GetDataEffectRate(builder, doc, portIds);//有效率统计
            GetPointGeneral(builder, doc, portIds);//监测点概况

            doc.MailMerge.Execute(fieldNames.ToString().Split('|'), fieldValues.ToString().Split('|'));//文字描述
            ImageToWord(builder, doc);//单指标图片加载

            //合并模版，相当于页面的渲染
            //doc.MailMerge.ExecuteWithRegions(dt);
            var docStream = new MemoryStream();
            if (IsBackup)
            {
                //保存Excel文件
                string fileToSave = System.IO.Path.Combine(SavePath + "/" + SaveFileName);
                if (File.Exists(fileToSave))
                {
                    File.Delete(fileToSave);
                }
                else
                {
                    FileManager.CreateFile(SaveFileName, SavePath);
                }
                doc.Save(SavePath + "/" + SaveFileName);
                //designer.Save(fileToSave, FileFormatType.Excel2003);
            }
            else
            {
                doc.Save(this.Response, SaveFileName, Aspose.Words.ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(Aspose.Words.SaveFormat.Doc));
            }
        }

        /// <summary>
        /// 月均值word数据源处理
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="doc"></param>
        private void ToMonthAnalazy(DocumentBuilder builder, Document doc, string[] portIds)
        {
            int num = 0;
            DateTime timeBegin = dtpBegin.SelectedDate.Value.Date;
            DateTime timeEnd = timeBegin.AddMonths(1).AddDays(-1);

            #region 数据源
            DataTable dt = monthAQI.GetMonthAvgData(portIds, timeBegin, timeEnd).ToTable();
            dt.TableName = "MonthAvgDT";
            #endregion

            #region 追加域、设定数据源
            builder.MoveToMergeField("MonthAvg");
            //Aspose.Words.Tables.Table table = (Aspose.Words.Tables.Table)doc.GetChild(NodeType.Table, 2, true);
            for (int j = 0; j < 2; j++)
            {
                num = 0;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    #region 创建单元格
                    if (i < dt.Columns.Count - 1)
                    {
                        builder.MoveToMergeField("MonthAvg");
                        builder.InsertCell();
                        if (j == 0)
                        {
                            builder.CellFormat.Borders.Top.LineStyle = LineStyle.Double;
                            builder.CellFormat.Borders.Bottom.LineStyle = LineStyle.Double;
                            builder.CellFormat.Shading.BackgroundPatternColor = System.Drawing.Color.FromArgb(217, 217, 217);
                        }
                        else
                        {
                            builder.CellFormat.Borders.Top.LineStyle = LineStyle.Single;
                            builder.CellFormat.Borders.Bottom.LineStyle = LineStyle.Single;
                            builder.CellFormat.Shading.BackgroundPatternColor = System.Drawing.Color.Transparent;
                        }
                    }
                    #endregion

                    #region 添加域以及表头
                    if (!dt.Columns[i].ColumnName.Equals("DateID") && !dt.Columns[i].ColumnName.Equals("FactorID"))
                    {
                        #region 表头
                        if (j == 0)
                        {
                            builder.MoveToCell(0, 0, num + 1, 0);
                            builder.Write(dt.Columns[i].ColumnName);
                        }
                        #endregion

                        #region 数据
                        if (j == 1)
                        {
                            builder.MoveToCell(0, 1, num + 1, 0);
                            if (num == 0)
                            {
                                builder.InsertField("MERGEFIELD TableStart:MonthAvgDT");
                                builder.InsertField("MERGEFIELD 项目");
                            }
                            else if (i == dt.Columns.Count - 1)
                            {

                                builder.InsertField("MERGEFIELD " + dt.Columns[i].ColumnName);
                                builder.InsertField("MERGEFIELD TableEnd:MonthAvgDT");
                            }
                            else
                                builder.InsertField("MERGEFIELD " + dt.Columns[i].ColumnName);
                        }
                        #endregion
                        builder.CellFormat.VerticalAlignment = Aspose.Words.Tables.CellVerticalAlignment.Center;
                        num++;
                    }
                    #endregion
                }
                builder.EndRow();
            }

            builder.EndTable();//表格操作结束
            doc.MailMerge.ExecuteWithRegions(dt);//设定数据源
            #endregion

            #region 单元格合并
            builder.MoveToMergeField("MonthAvg");
            Aspose.Words.Tables.Table table = (Aspose.Words.Tables.Table)doc.GetChild(NodeType.Table, 2, true);
            for (int i = 1; i < 7; i++)
            {
                #region 列合并
                if (i == 1)
                {
                    table.Rows[i].Cells[0].CellFormat.VerticalMerge = Aspose.Words.Tables.CellMerge.First;
                    table.Rows[i + 6].Cells[0].CellFormat.VerticalMerge = Aspose.Words.Tables.CellMerge.First;
                    table.Rows[i + 12].Cells[0].CellFormat.VerticalMerge = Aspose.Words.Tables.CellMerge.First;
                    //for (int j = 0; j < table.Rows[i].Cells.Count; j++)
                    //{
                    //    if (j == 1)
                    //        table.Rows[0].Cells[j].CellFormat.HorizontalMerge = Aspose.Words.Tables.CellMerge.Previous;
                    //    else
                    //        table.Rows[0].Cells[j].CellFormat.HorizontalMerge = Aspose.Words.Tables.CellMerge.First;
                    //}
                }
                else
                {
                    table.Rows[i].Cells[0].CellFormat.VerticalMerge = Aspose.Words.Tables.CellMerge.Previous;
                    table.Rows[i + 6].Cells[0].CellFormat.VerticalMerge = Aspose.Words.Tables.CellMerge.Previous;
                    table.Rows[i + 12].Cells[0].CellFormat.VerticalMerge = Aspose.Words.Tables.CellMerge.Previous;
                }

                #endregion

                #region 对齐格式
                table.Rows[i].Cells[0].CellFormat.VerticalAlignment = Aspose.Words.Tables.CellVerticalAlignment.Center;
                table.Rows[i + 6].Cells[0].CellFormat.VerticalAlignment = Aspose.Words.Tables.CellVerticalAlignment.Center;
                table.Rows[i + 12].Cells[0].CellFormat.VerticalAlignment = Aspose.Words.Tables.CellVerticalAlignment.Center;
                #endregion
            }

            #region 行合并

            table.Rows[0].Cells[0].CellFormat.HorizontalMerge = Aspose.Words.Tables.CellMerge.First;
            table.Rows[0].Cells[1].CellFormat.HorizontalMerge = Aspose.Words.Tables.CellMerge.First;
            //table.Rows[0].Cells[2].CellFormat.HorizontalMerge = Aspose.Words.Tables.CellMerge.First;
            #endregion

            #region 合并名称
            builder.MoveToMergeField("MonthAvg");
            builder.MoveToCell(0, 1, 0, 0);
            builder.Write(dtpBegin.SelectedDate.Value.ToString("yyyy年MM月") + "\r\n月均浓度\r\n（毫克/立方米）");
            builder.MoveToCell(0, 7, 0, 0);
            builder.Write("与上月相\r\n比变化值\r\n（毫克/立方米）");
            builder.MoveToCell(0, 13, 0, 0);
            builder.Write("与去年同期\r\n相比变化值\r\n（毫克/立方米）");

            builder.MoveToCell(0, 0, 0, 0);
            builder.Write("项目");
            #endregion
            #endregion

        }

        /// <summary>
        /// 获取数据有效率
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="doc"></param>
        private void GetDataEffectRate(DocumentBuilder builder, Document doc, string[] portIds)
        {
            DateTime timeBegin = dtpBegin.SelectedDate.Value.Date;
            DataTable dataDt = new MonthReportService().GetQualifiedRate(portIds, factorCode, timeBegin.ToString("yyyy-MM"), timeBegin.ToString("yyyy-MM"));
            dataDt.TableName = "EffectRate";
            doc.MailMerge.ExecuteWithRegions(dataDt);//设定数据源
            Aspose.Words.Tables.Table table = (Aspose.Words.Tables.Table)doc.GetChild(NodeType.Table, 0, true);
            for (int i = 0; i < dataDt.Rows.Count; i++)
            {
                if (i % 2 == 0)
                    table.Rows[i + 1].Cells[0].CellFormat.VerticalMerge = Aspose.Words.Tables.CellMerge.First;
                else
                    table.Rows[i + 1].Cells[0].CellFormat.VerticalMerge = Aspose.Words.Tables.CellMerge.Previous;
                table.Rows[i + 1].Cells[0].CellFormat.VerticalAlignment = Aspose.Words.Tables.CellVerticalAlignment.Center;
            }
        }

        /// <summary>
        /// 获取监测点概况
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="doc"></param>
        /// <param name="portIds"></param>
        private void GetPointGeneral(DocumentBuilder builder, Document doc, string[] portIds)
        {
            DateTime timeBegin = dtpBegin.SelectedDate.Value.Date;
            DateTime timeEnd = timeBegin.AddMonths(1).AddDays(-1);
            DataTable dataDt = new MonthReportService().GetPointGeneral(portIds, timeBegin.ToString("yyyy-MM-dd"), timeEnd.ToString("yyyy-MM-dd"));
            dataDt.TableName = "PointGeneral";
            doc.MailMerge.ExecuteWithRegions(dataDt);//设定数据源
        }

        private void ImageToWord(DocumentBuilder builder, Document doc)
        {
            int groupCount = ViewState["groupPoint"].ToString().Split(';').Length;
            Shape shape = null;
            #region 污染物指标
            for (int i = 1; i < factorName.Length; i++)
            {
                builder.MoveToMergeField("DayData_" + factorName[i]);
                for (int j = 0; j < groupCount; j++)
                {
                    try
                    {
                        shape = builder.InsertImage(Server.MapPath("../../../Files/TempFile/Word/AirMonthReport/") + "DayData_" + factorName[i] + "_" + (j + 1) + ".png");
                        shape.Width = 446;
                        shape.Height = 267;
                        builder.Write(string.Format("\r\n图{0}:各点位{1}日均浓度变化趋势", i + GetCode(j + 97), factorNameCN[i]));

                    }
                    catch
                    {
                    }
                }
            }
            #endregion

            #region 能见度
            for (int i = 0; i < huimaiPointids.Length; i++)
            {
                builder.MoveToMergeField("VisibilityDayData_" + huimaiPointids[i]);
                shape = builder.InsertImage(Server.MapPath("../../../Files/TempFile/Word/AirMonthReport/") + "VisibilityDayData_" + huimaiPointids[i] + ".png");
                shape.Width = 446;
                shape.Height = 267;
            }
            #endregion
        }

        /// <summary>
        /// 颜色转换
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public Color ReturnColorFromString(string color)
        {
            color = color.Trim().Substring(1, color.Length - 1).Trim();
            string red = color.Substring(0, 2);
            string green = color.Substring(2, 2);
            string blue = color.Substring(4, 2);

            byte redByte = Convert.ToByte(red, 16);
            byte greenByte = Convert.ToByte(green, 16);
            byte blueByte = Convert.ToByte(blue, 16);
            return Color.FromArgb(255, redByte, greenByte, blueByte);
        }
        #endregion
    }
}