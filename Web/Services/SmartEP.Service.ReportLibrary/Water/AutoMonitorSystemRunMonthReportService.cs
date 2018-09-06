﻿using Aspose.Words;
using Aspose.Words.Tables;
using SmartEP.DomainModel.BaseData;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.Infrastructure.Generic;
using SmartEP.Service.BaseData.Channel;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.BaseData.Standard;
using SmartEP.Service.Core.Enums;
using SmartEP.Service.DataAnalyze.Public;
using SmartEP.Service.DataAnalyze.Report;
using SmartEP.Service.DataAnalyze.Water;
using SmartEP.Service.DataAnalyze.Water.DataQuery;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Utilities.Web.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;


namespace SmartEP.Service.ReportLibrary.Water
{
    public class AutoMonitorSystemRunMonthReportService 
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        ReportLogService ReportLogService = new ReportLogService();
        FactorsConfigService configService = new FactorsConfigService();
        PublicReportService reportService = new PublicReportService();
        /// <summary>
        /// 数据库处理类
        /// </summary>
        ReportSummaryService g_ReportSummaryService = Singleton<ReportSummaryService>.GetInstance();
        //日数据
        DataQueryByDayService dataQueryByDayService = new DataQueryByDayService();
        //点位
        MonitoringPointWaterService monitoringPointWaterService = new MonitoringPointWaterService();
        //因子
        WaterPollutantService m_WaterPollutantService = new WaterPollutantService();
        EQIConcentrationService EQIService = new EQIConcentrationService();
        DateTime beginTime;
        DateTime endTime;
        string PageTypeId = "5abe35a9-a119-4506-a83a-c7372dc95211"; //页面类型Guid
        string VOCGuid = "41fb0a54-660b-4f8b-b298-47d48955cfe0"; //VOC
        string AlgaGuid = "4968e16e-f3cb-4824-b510-ba1b43f07996"; //藻类
        string TW = "w01029t"; //总水量因子
        string pageTypeID = "AutoMonitorSystemRunMonthReport";
        int waterOrAirType = 0; 
        

        //public void update(string pageid, string id, string PageTypeId)
        //{
        //    ReportLogEntity customDatumData = ReportLogService.CustomDatumRetrieve(pageTypeID, waterOrAirType).Where(it => it.Id == int.Parse(id)).FirstOrDefault();
        //    DateTime CurrentTime = System.DateTime.Now;
        //    GetDateRange(customDatumData);
        //    DataTable reportSummary = new DataTable();
        //    reportSummary.Columns.Add("PointId", typeof(string));
        //    reportSummary.Columns.Add("Tstamp", typeof(string));
        //    reportSummary.Columns.Add("Year", typeof(string));
        //    reportSummary.Columns.Add("MonthOfYear", typeof(string));
        //    reportSummary.Columns.Add("PollutantCode", typeof(string));
        //    reportSummary.Columns.Add("PollutantValue", typeof(string));
        //    DataTable reportSummaryItem = new DataTable();
        //    reportSummaryItem.Columns.Add("Tstamp", typeof(string));
        //    reportSummaryItem.Columns.Add("Year", typeof(string));
        //    reportSummaryItem.Columns.Add("MonthOfYear", typeof(string));
        //    reportSummaryItem.Columns.Add("MonitorData", typeof(string));
        //    reportSummaryItem.Columns.Add("QualifiedData", typeof(string));
        //    reportSummaryItem.Columns.Add("QualifiedRate", typeof(string));
        //    reportSummaryItem.Columns.Add("Summary", typeof(string));
        //    //点位因子小数位表
        //    DataTable dtPointsFactorNum = new DataTable();
        //    //dtPointsFactorNum.Columns.Add("PointId", typeof(string));
        //    //dtPointsFactorNum.Columns.Add("Pollutant", typeof(string));
        //    //dtPointsFactorNum.Columns.Add("DecimalDigit", typeof(string));
        //    //点位因子关系表
        //    DataTable dtPoints = GetFactorsRelation(out dtPointsFactorNum, customDatumData, PageTypeId);
        //    //点位Id
        //    //string[] PointIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID).OrderBy(x => SmartEP.Core.Enums.CbxRsmReturnType.ID).ToArray();
        //    string[] PointIds = customDatumData.PointIds.Split(';');
        //    List<int> PointsList = new List<int>();
        //    for (int i = 0; i < PointIds.Length; i++)
        //    {
        //        PointsList.Add(Convert.ToInt16(PointIds[i]));
        //    }
        //    //获取最大化因子
        //    DataTable dtMF = configService.GetMaxFactors(PageTypeId, PointsList).ToTable();
        //    List<string> FactorsList = new List<string>();
        //    for (int i = 0; i < dtMF.Rows.Count; i++)
        //    {
        //        FactorsList.Add(dtMF.Rows[i]["PollutantCodes"].ToString());
        //    }

        //    //string path = "D:\\ok\\标准代码库\\Src\\SmartEP.WebUI\\Pages\\EnvWater\\Report\\DocumentsTemplet";
        //    //string path = "../../..//Pages/EnvWater/Report/DocumentsTemplet";
        //    //Document doc = new Document(System.IO.Path.Combine(path, "AutoMonitorSystemRunMonthReportTemplete.doc"));
        //    //Document doc = new Document(System.IO.Path.Combine("..\\..\\..\\Pages\\EnvWater\\Report\\DocumentsTemplet", "AutoMonitorSystemRunMonthReportTemplete.doc"));
        //    Document doc = new Document(customDatumData.ReportName);
        //    DocumentBuilder builder = new DocumentBuilder(doc);

        //    builder.Font.ClearFormatting();
        //    builder.MoveToMergeField("reportDate");
        //    builder.Write(customDatumData.StartDateTime.Value.ToString("yyyy年MM月", System.Globalization.CultureInfo.GetCultureInfo("zh-CN").DateTimeFormat));
        //    builder.MoveToMergeField("StatisticDate");
        //    builder.Write(beginTime.ToString("yyyy-MM-dd") + "~" + endTime.ToString("yyyy-MM-dd"));
        //    builder.MoveToMergeField("MonitSummary");
        //    string strMonitSummary = "    本月份系统运行正常。" + "各子站的运行情况见表 1 所示。";
        //    builder.Write(strMonitSummary);
        //    builder.MoveToMergeField("MonitResult");
        //    string strMonitResult = "水质月均监测结果见表2~表" + (2 + dtPoints.Rows.Count) + "所示。";
        //    builder.Write(strMonitResult);
        //    builder.MoveToMergeField("MonitResultDesc");
        //    builder.Write("    本月" + InitMonitResult(PageTypeId, customDatumData) + "各子站日监测结果见附表。");
        //    builder.MoveToMergeField("MFRemark");
        //    builder.Write("1.本月份按相关管理要求，对水站每天进行日常运行和维护管理工作。\r\n\n2.各子站按质控要求实施“日监控、周检查、月比对”的质量管理制度。即每天至少两次数据调取监控，每周至少一次标准溶液检查测试，每月至少一次实际水样的实验室比对测试。\r\n\n3.金墅港子站由于系统运维调试，12月2日有一组数据缺失。五参数仪由于仪器传输故障导致数据不变，12月8日~15日期间共29组数据无效；氨氮测定仪由于校准异常导致12月11日~13日共13组数据无效，多台仪器故障导致本月金墅港子站数据总有效率仅为90.08%（低于95%达标率）。总氮、总磷测定仪由于性能测试12月21日、23日数据无效（不纳入有效率统计）。\r\n\n4.本月金墅港子站浊度月均值较去年同期升高，而总氮监测月均值却为下降趋势，主要原因为本年度仪器使用过滤器进行水样的预处理，从而减小浊度对测定的干扰。\r\n\n5.渔洋山子站生物毒性仪因仪器故障导致1天监测数据无效，单机数据有效率为91.40%；若去除毒性仪抑制率指标，渔洋山子站运维有效率为98.33%。氟化物仪因仪器故障导致12月11日~13日数据无效；总氮测定仪因性能测试12月23日数据无效（不纳入有效率统计）。\r\n\n6.针对子站监测月均值波动及超标现象，分别加密进行仪器校准及标准溶液核查工作。渔洋山子站本月针对总氮监测指标共进行了5次标准溶液核查工作，最大相对误差为5.1%；金墅港子站本月针对总氮监测指标进行了1次仪器校准、5次标准溶液核查工作，最大相对误差为10.0%；针对总磷监测指标进行了1次仪器校准、5次标准溶液核查工作，最大相对误差为7.0%，均在10%质控范围之内。\r\n\n7.本月针对pH监测指标进行了预防性维护和仪器性能考核，金墅港子站本月进行了1次仪器校准、5次标准核查工作，最大绝对误差为0.06；渔洋山子站本月进行了4次标准溶液核查工作，最大绝对误差为0.05，均在0.20质控范围之内。\r\n\n8.本月针对溶解氧监测指标进行了预防性维护和仪器性能考核，金墅港子站本月进行了4次饱和溶解氧核查工作，最大绝对误差为0.20mg/L；渔洋山子站本月进行了4次饱和溶解氧核查工作，最大绝对误差为0.20 mg/L，均在0.50 mg/L质控范围之内。\r\n\n9.本月针对藻类密度监测指标进行了预防性维护和仪器性能考核，金墅港子站本月进行了1次标准核查工作，最大相对误差为0.7%；渔洋山子站本月进行了1次标准核查工作，最大相对误差为4.7%，均在10%质控范围之内。\r\n\n10.龙塘港水文站水文监测日均数据显示，本月龙塘港河入湖天数为4天，入湖总水量1.88×105 m3；滞流（或关闸）（平均流速小于0.02m/s）天数为26天。");
        //    builder.MoveToMergeField("CYear");
        //    builder.Write(CurrentTime.Year.ToString());
        //    builder.MoveToMergeField("CMonth");
        //    builder.Write(CurrentTime.Month.ToString());
        //    builder.MoveToMergeField("CDay");
        //    builder.Write(CurrentTime.Day.ToString());

        //    builder.MoveToMergeField("MF1");
        //    DataTable MonitorStatistics = GetMonitorStatistics(dtPoints, beginTime, endTime);
        //    DataTable ShouldRecord = GetShouldRecord(dtPoints, beginTime, endTime);
        //    MoveToMF1(builder, MonitorStatistics, ShouldRecord);
        //    //MoveToMF1(builder, MonitorStatistics);
        //    builder.MoveToMergeField("MF2");
        //    DataTable dtMonthAvgContent = GetMonthAvgContent(PointsList, beginTime, endTime);
        //    MoveToMF2(builder, dtPoints, dtMonthAvgContent, dtPointsFactorNum, customDatumData, PageTypeId);
        //    builder.MoveToMergeField("MF3");
        //    DataTable dtDayData = dataQueryByDayService.GetRMDayData(PointsList, beginTime, endTime).ToTable();
        //    DataTable dtMonthAvg = dataQueryByDayService.GetRMStatisticalDataAvg(PointsList, beginTime, endTime).ToTable();
        //    DataTable dtBaseData = dataQueryByDayService.GetGradeOrLimit(PointsList, customDatumData.StartDateTime.Value.Year, customDatumData.StartDateTime.Value.Month).ToTable();
        //    MoveToMF3(builder, dtPoints, dtDayData, dtMonthAvg, dtBaseData, dtPointsFactorNum, customDatumData, PageTypeId);

        //    foreach (DataRow dmacr in dtMonthAvgContent.Rows)
        //    {
        //        DataRow drSummary = reportSummary.NewRow();
        //        drSummary["PointId"] = dmacr["PointId"].ToString();
        //        drSummary["Tstamp"] = customDatumData.CreatDateTime.Value.ToString("yyyy-MM");
        //        drSummary["Year"] = customDatumData.CreatDateTime.Value.Year;
        //        drSummary["MonthOfYear"] = customDatumData.CreatDateTime.Value.Month;
        //        drSummary["PollutantCode"] = dmacr["PollutantCode"].ToString();
        //        drSummary["PollutantValue"] = dmacr["Value_Avg"].ToString();
        //        reportSummary.Rows.Add(drSummary);
        //    }
        //    decimal CollectionCount = 0;
        //    decimal QualifiedCount = 0;
        //    foreach (string pointId in PointIds)
        //    {
        //        DataRow drMonitor = reportSummary.NewRow();
        //        DataRow drQualified = reportSummary.NewRow();
        //        DataRow drRate = reportSummary.NewRow();
        //        drMonitor["PointId"] = pointId;
        //        drMonitor["Tstamp"] = customDatumData.StartDateTime.Value.ToString("yyyy-MM");
        //        drMonitor["Year"] = customDatumData.StartDateTime.Value.Year;
        //        drMonitor["MonthOfYear"] = customDatumData.StartDateTime.Value.Month;
        //        drMonitor["PollutantCode"] = "MonitorData";
        //        drQualified["PointId"] = pointId;
        //        drQualified["Tstamp"] = customDatumData.StartDateTime.Value.ToString("yyyy-MM");
        //        drQualified["Year"] = customDatumData.StartDateTime.Value.Year;
        //        drQualified["MonthOfYear"] = customDatumData.StartDateTime.Value.Month;
        //        drQualified["PollutantCode"] = "QualifiedData";
        //        drRate["PointId"] = pointId;
        //        drRate["Tstamp"] = customDatumData.StartDateTime.Value.ToString("yyyy-MM");
        //        drRate["Year"] = customDatumData.StartDateTime.Value.Year;
        //        drRate["MonthOfYear"] = customDatumData.StartDateTime.Value.Month;
        //        drRate["PollutantCode"] = "QualifiedRate";
        //        decimal Collection = 0;
        //        decimal Qualified = 0;
        //        DataRow[] dr = MonitorStatistics.Select("PointId=" + pointId);
        //        if (dr.Count() > 0)
        //        {
        //            if (dr[0]["CollectionCount"] != null)
        //            {
        //                drMonitor["PollutantValue"] = dr[0]["CollectionCount"].ToString();
        //                Collection = decimal.Parse(dr[0]["CollectionCount"].ToString());
        //                CollectionCount += decimal.Parse(dr[0]["CollectionCount"].ToString());
        //            }
        //            if (dr[0]["QualifiedCount"] != null)
        //            {
        //                drQualified["PollutantValue"] = dr[0]["QualifiedCount"].ToString();
        //                Qualified = decimal.Parse(dr[0]["QualifiedCount"].ToString());
        //                QualifiedCount += decimal.Parse(dr[0]["QualifiedCount"].ToString());
        //            }
        //            if (Collection != 0)
        //            {
        //                drRate["PollutantValue"] = Math.Round(Qualified / Collection * 100, 2);
        //            }
        //        }
        //        reportSummary.Rows.Add(drMonitor);
        //        reportSummary.Rows.Add(drQualified);
        //        reportSummary.Rows.Add(drRate);

        //    }


        //    DataRow drSummaryItem = reportSummaryItem.NewRow();
        //    drSummaryItem["Tstamp"] = customDatumData.StartDateTime.Value.ToString("yyyy-MM");
        //    drSummaryItem["Year"] = customDatumData.StartDateTime.Value.Year;
        //    drSummaryItem["MonthOfYear"] = customDatumData.StartDateTime.Value.Month;
        //    drSummaryItem["MonitorData"] = CollectionCount;
        //    drSummaryItem["QualifiedData"] = QualifiedCount;
        //    if (CollectionCount != 0)
        //    {
        //        drSummaryItem["QualifiedRate"] = Math.Round(QualifiedCount / CollectionCount * 100, 2);
        //    }
        //    drSummaryItem["Summary"] = "    本月" + InitMonitResult(PageTypeId, customDatumData);

        //    reportSummaryItem.Rows.Add(drSummaryItem);

        //    try
        //    {
        //        g_ReportSummaryService.insertTable(reportSummary.DefaultView, customDatumData.StartDateTime.Value.Year, customDatumData.StartDateTime.Value.Month, PointIds, "AutoMonitorSystemRunMonthReport");
        //        g_ReportSummaryService.insertItemTable(reportSummaryItem.DefaultView, customDatumData.StartDateTime.Value.Year, customDatumData.StartDateTime.Value.Month, "AutoMonitorSystemRunMonthReport");
        //    }
        //    catch (Exception ex)
        //    {
        //    }

        //    doc.MailMerge.DeleteFields();
        //    save(doc, customDatumData);
        //}

        /// <summary>
        /// 获取点位因子关系表
        /// </summary>
        /// <returns></returns>
        public DataTable GetFactorsRelation(out DataTable dtNum, ReportLogEntity customDatumData, string PageTypeId, string str = "")
        {
            //点位Id
            string[] pointIds = customDatumData.PointIds.Split(';');
            List<int> pointList = new List<int>();
            for (int i = 0; i < pointIds.Length; i++)
            {
                pointList.Add(Convert.ToInt16(pointIds[i]));
            }
            //点位因子关系表
            DataTable dt = configService.GetFactorsById(PageTypeId, pointList).ToTable();
            DataTable dtCategory = dt.DefaultView.ToTable(true, "CategoryUid");
            DataTable dtTemp = new DataTable();
            dtTemp.Columns.Add("PointId", typeof(string));
            dtTemp.Columns.Add("PollutantCode", typeof(string));
            dtTemp.Columns.Add("DecimalDigit", typeof(string));
            DataTable dtNew = new DataTable();
            dtNew.Columns.Add("PointId");
            dtNew.Columns.Add("PollutantCodes");
            if (str == "C")
            {
                dtNew.Columns.Add("PollutantType");
                for (int i = 0; i < pointList.Count; i++)
                {
                    for (int j = 0; j < dtCategory.Rows.Count; j++)
                    {
                        DataRow drTemp = dtTemp.NewRow();
                        DataRow drNew = dtNew.NewRow();
                        drTemp["PointId"] = pointList[i];
                        drNew["PointId"] = pointList[i];
                        drNew["PollutantType"] = dtCategory.Rows[j]["CategoryUid"].ToString();
                        string strFactors = "";
                        DataRow[] drs = dt.Select("PointId=" + pointList[i] + " and CategoryUid='" + dtCategory.Rows[j]["CategoryUid"].ToString() + "'").OrderBy(x => x["OrderByNum"]).ToArray();
                        for (int k = 0; k < drs.Length; k++)
                        {
                            strFactors += drs[k]["PollutantCodes"].ToString() + ";";
                            drTemp["PollutantCode"] = drs[k]["PollutantCodes"];
                            drTemp["DecimalDigit"] = drs[k]["DecimalDigit"];
                        }
                        drNew["PollutantCodes"] = strFactors.TrimEnd(';');
                        dtNew.Rows.Add(drNew);
                        dtTemp.Rows.Add(drTemp);
                    }
                }
            }
            else
            {
                for (int i = 0; i < pointList.Count; i++)
                {

                    DataRow drNew = dtNew.NewRow();

                    drNew["PointId"] = pointList[i];
                    string strFactors = "";
                    DataRow[] drs = dt.Select("PointId=" + pointList[i] + "").OrderBy(x => x["OrderByNum"]).ToArray();
                    for (int j = 0; j < drs.Length; j++)
                    {
                        DataRow drTemp = dtTemp.NewRow();
                        drTemp["PointId"] = pointList[i];
                        strFactors += drs[j]["PollutantCodes"].ToString() + ";";
                        drTemp["PollutantCode"] = drs[j]["PollutantCodes"];
                        drTemp["DecimalDigit"] = drs[j]["DecimalDigit"];
                        dtTemp.Rows.Add(drTemp);
                    }
                    drNew["PollutantCodes"] = strFactors.TrimEnd(';');
                    dtNew.Rows.Add(drNew);

                }
            }
            DataView dvNew = dtNew.DefaultView;
            dvNew.Sort = "PointId";
            dtNum = dtTemp;
            return dvNew.ToTable();
        }

        /// <summary>
        /// 保存记录
        /// </summary>
        public void save(Document doc, ReportLogEntity customDatumData)
        {
            string filename = "(" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ")" + "AutoMonitorSystemRunMonthReportTemplete" + ".doc";
            //"D:\\ok\\标准代码库\\Src\\SmartEP.WebUI\\Pages\\EnvWater\\Report\\ReportFile\\AutoMonitorSystemRunMonthReportTemplete\\2016\\3\\(201703201421479293)AutoMonitorSystemRunMonthReportTemplete.doc"
            //string strTarget = Server.MapPath("../../../Pages/EnvWater/Report/ReportFile/AutoMonitorSystemRunMonthReportTemplete/" + customDatumData.StartDateTime.Value.Year + "/" + customDatumData.StartDateTime.Value.Month + "/" + filename);
            string strTarget = "D:\\ok\\标准代码库\\Src\\SmartEP.WebUI\\Pages\\EnvWater\\Report\\ReportFile\\AutoMonitorSystemRunMonthReportTemplete\\" + customDatumData.StartDateTime.Value.Year +"\\"+ customDatumData.StartDateTime.Value.Month+"\\"+filename;
            customDatumData.UpdateDateTime = DateTime.Now;
            customDatumData.ReportName = ("../../../Pages/EnvWater/Report/ReportFile/AutoMonitorSystemRunMonthReportTemplete/" + customDatumData.StartDateTime.Value.Year + "/" + customDatumData.StartDateTime.Value.Month + "/" + filename).ToString();
            //更新数据
            ReportLogService.ReportLogUpdate(customDatumData);
            doc.Save(strTarget);
            //if (!Directory.Exists(strTarget))
            //{
                
            //}
        }

        /// <summary>
        /// 获取月监测结果
        /// </summary>
        /// <returns></returns>
        public string InitMonitResult(string PageTypeId, ReportLogEntity customDatumData)
        {
            StringBuilder strReturn = new StringBuilder();
            DataTable dtPointFactorNum = new DataTable();
            //点位因子关系表
            DataTable dtPoints = GetFactorsRelation(out dtPointFactorNum, customDatumData, PageTypeId);
            //点位Id
            string[] PointIds = customDatumData.PointIds.Split(';');
            List<int> PointsList = new List<int>();
            for (int i = 0; i < PointIds.Length; i++)
            {
                PointsList.Add(Convert.ToInt16(PointIds[i]));
            }
            //获取最大化因子
            DataTable dtMF = configService.GetMaxFactors(PageTypeId, PointsList).ToTable();
            List<string> FactorsList = new List<string>();
            for (int i = 0; i < dtMF.Rows.Count; i++)
            {
                FactorsList.Add(dtMF.Rows[i]["PollutantCodes"].ToString());
            }
            //数据处理
            DataTable dtMonthContent = GetMonthAvgContent(PointsList, beginTime, endTime);
            for (int i = 0; i < dtPoints.Rows.Count; i++)
            {
                int pointId = Convert.ToInt16(dtPoints.Rows[i]["PointId"]);
                string PointName = monitoringPointWaterService.RetrieveEntityByPointId(pointId).MonitoringPointName;
                string[] FactorCodes = dtPoints.Rows[i]["PollutantCodes"].ToString().Split(';');
                strReturn.AppendFormat("{0}子站的", PointName);
                for (int j = 0; j < FactorCodes.Length; j++)
                {
                    string FactorName = m_WaterPollutantService.GetPollutantInfo(FactorCodes[j]).PollutantName;
                    int num = m_WaterPollutantService.GetPollutantInfo(FactorCodes[j]).PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo(FactorCodes[j]).PollutantDecimalNum) : 2;
                    DataRow[] dr = dtMonthContent.Select("PointId=" + pointId + " and PollutantCode='" + FactorCodes[j] + "'");
                    string LastMonthComp = "";
                    string LastYearMontComp = "";
                    string Comp = "";
                    if (dr.Length > 0)
                    {
                        if (dr[0]["Value_Avg"] != DBNull.Value)
                        {
                            if (dr[0]["LastMonthAvg"] != DBNull.Value && dr[0]["LastMonthAvg"].ToString() != "0")
                            {
                                Decimal value1 = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[0]["Value_Avg"]), num);
                                Decimal value2 = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[0]["LastMonthAvg"]), num);
                                if (value1 > value2)
                                    Comp = "上升";
                                else
                                    Comp = "下降";
                                if (value2 != 0 && 100 * ((value1 - value2) / value2) > 20)
                                    LastMonthComp = Comp + (Math.Abs(100 * ((value1 - value2) / value2))).ToString("0") + "%";
                            }
                            if (dr[0]["LastYearMonthAvg"] != DBNull.Value && dr[0]["LastYearMonthAvg"].ToString() != "0")
                            {
                                Decimal value1 = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[0]["Value_Avg"]), num);
                                Decimal value2 = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[0]["LastYearMonthAvg"]), num);
                                if (value1 > value2)
                                    Comp = "上升";
                                else
                                    Comp = "下降";
                                if (value2 != 0 && 100 * ((value1 - value2) / value2) > 20)
                                    LastYearMontComp = Comp + (Math.Abs(100 * (value1 - value2) / value2)).ToString("0") + "%";
                            }
                            if (LastMonthComp != "" && LastYearMontComp != "")
                                strReturn.AppendFormat("{0}监测月均值较上月及去年同期分别{1}、{2}，", FactorName, LastMonthComp, LastYearMontComp);
                            else if (LastMonthComp != "" && LastYearMontComp == "")
                                strReturn.AppendFormat("{0}监测月均值较上月{1}，", FactorName, LastMonthComp);
                            else if (LastMonthComp == "" && LastYearMontComp != "")
                                strReturn.AppendFormat("{0}监测月均值较去年同期{1}，", FactorName, LastYearMontComp);
                            else { }
                        }
                    }
                }
                strReturn.Remove(strReturn.Length - 1, 1);
                strReturn.Append("；");
            }
            strReturn.Remove(strReturn.Length - 1, 1);
            strReturn.Append("。");
            return strReturn.ToString();
        }

        /// <summary>
        /// 获取水质自动监测子站运行情况
        /// </summary>
        /// <param name="dtPoints"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        public DataTable GetMonitorStatistics(DataTable dtPoints, DateTime beginTime, DateTime endTime)
        {
            string ApplicationUid = "watrwatr-watr-watr-watr-watrwatrwatr";
            return reportService.GetRunningEffectRateByUncertainFactors(ApplicationUid, dtPoints, beginTime, endTime).ToTable();
        }

        /// <summary>
        /// 获取月均监测结果
        /// </summary>
        /// <returns></returns>
        public DataTable GetMonthAvgContent(List<int> PointIds, DateTime beginTime, DateTime endTime)
        {
            //本月数据
            DataTable dt = dataQueryByDayService.GetRMStatisticalData(PointIds, beginTime, endTime).ToTable();
            DataRow[] drs = dt.Select("PointId=53 and PollutantCode='w01029'");
            for (int i = 0; i < drs.Length; i++)
            {
                DataRow dr = dt.NewRow();
                dr["PointId"] = drs[i]["PointId"];
                dr["PollutantCode"] = TW;
                dr["Value_Max"] = drs[i]["Total_Max"];
                dr["Value_Min"] = drs[i]["Total_Min"];
                dr["Value_Avg"] = drs[i]["Total_Avg"];
                dr["CategoryUid"] = drs[i]["CategoryUid"];
                dr["F"] = drs[i]["F"];
                dt.Rows.Add(dr);
            }
            //上月数据
            DataTable dtLastMonth = dataQueryByDayService.GetRMStatisticalDataAvg(PointIds, beginTime.AddMonths(-1), Convert.ToDateTime(endTime.AddMonths(-1).ToString("yyyy-MM-") + endTime.AddMonths(-1).DaysInMonth() + " 23:59:59")).ToTable();
            DataRow[] drsLastMonth = dtLastMonth.Select("PointId=53 and PollutantCode='w01029'");
            for (int i = 0; i < drsLastMonth.Length; i++)
            {
                DataRow dr = dtLastMonth.NewRow();
                dr["PointId"] = drsLastMonth[i]["PointId"];
                dr["PollutantCode"] = TW;
                dr["Value_Avg"] = drsLastMonth[i]["Total_Avg"];
                dr["F"] = drsLastMonth[i]["F"];
                dtLastMonth.Rows.Add(dr);
            }
            //去年同期
            DataTable dtLastYearMonth = dataQueryByDayService.GetRMStatisticalDataAvg(PointIds, beginTime.AddYears(-1), Convert.ToDateTime(endTime.AddYears(-1).ToString("yyyy-MM-") + endTime.AddYears(-1).DaysInMonth() + " 23:59:59")).ToTable();
            DataRow[] drLastYearMonth = dtLastYearMonth.Select("PointId=53 and PollutantCode='w01029'");
            for (int i = 0; i < drLastYearMonth.Length; i++)
            {
                DataRow dr = dtLastYearMonth.NewRow();
                dr["PointId"] = drLastYearMonth[i]["PointId"];
                dr["PollutantCode"] = TW;
                dr["Value_Avg"] = drLastYearMonth[i]["Total_Avg"];
                dr["F"] = drLastYearMonth[i]["F"];
                dtLastYearMonth.Rows.Add(dr);
            }
            dt.Columns.Add("LastMonthAvg", typeof(decimal));
            dt.Columns.Add("LastYearMonthAvg", typeof(decimal));
            dt.Columns.Add("ClassBase", typeof(string));
            dt.Columns.Add("Upper", typeof(decimal));
            dt.Columns.Add("Low", typeof(decimal));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow[] dr = dtLastMonth.Select("PointId='" + dt.Rows[i]["PointId"].ToString() + "' and PollutantCode='" + dt.Rows[i]["PollutantCode"].ToString() + "' and F=" + dt.Rows[i]["F"].ToString() + "");
                DataRow[] drLastYear = dtLastYearMonth.Select("PointId='" + dt.Rows[i]["PointId"].ToString() + "' and PollutantCode='" + dt.Rows[i]["PollutantCode"].ToString() + "' and F=" + dt.Rows[i]["F"].ToString() + "");
                if (dr.Length > 0)
                    if (dr[0]["Value_Avg"] != DBNull.Value)
                        dt.Rows[i]["LastMonthAvg"] = Convert.ToDecimal(dr[0]["Value_Avg"]);
                if (drLastYear.Length > 0)
                    if (dr[0]["Value_Avg"] != DBNull.Value)
                        dt.Rows[i]["LastYearMonthAvg"] = Convert.ToDecimal(drLastYear[0]["Value_Avg"]);
                EQIConcentrationLimitEntity limit = EQIService.RetrieveWaterConcentration(WaterQualityClass.Three, dt.Rows[i]["PollutantCode"].ToString(), WaterPointCalWQType.River);
                if (limit != null)
                {
                    if (limit.Low != null && limit.Upper != null)
                    {
                        dt.Rows[i]["Upper"] = limit.Upper;
                        dt.Rows[i]["Low"] = limit.Low;
                        if (limit.Upper >= 9999)
                        {
                            dt.Rows[i]["ClassBase"] = "≥" + ((dt.Rows[i]["PollutantCode"].ToString() == "w21017" || dt.Rows[i]["PollutantCode"].ToString() == "w21001" || dt.Rows[i]["PollutantCode"].ToString() == "w21003") ? limit.Low.Value.ToString("0.0") : limit.Low.Value.ToString().TrimEnd('0').TrimEnd('.'));
                        }
                        else if (limit.Low <= 0)
                        {
                            dt.Rows[i]["ClassBase"] = "≤" + ((dt.Rows[i]["PollutantCode"].ToString() == "w21017" || dt.Rows[i]["PollutantCode"].ToString() == "w21001" || dt.Rows[i]["PollutantCode"].ToString() == "w21003") ? limit.Upper.Value.ToString("0.0") : limit.Upper.Value.ToString().TrimEnd('0').TrimEnd('.'));
                        }
                        else
                        {
                            dt.Rows[i]["ClassBase"] = ((dt.Rows[i]["PollutantCode"].ToString() == "w21017" || dt.Rows[i]["PollutantCode"].ToString() == "w21001" || dt.Rows[i]["PollutantCode"].ToString() == "w21003") ? limit.Low.Value.ToString("0.0") : limit.Low.Value.ToString().TrimEnd('0').TrimEnd('.')) + "~" + ((dt.Rows[i]["PollutantCode"].ToString() == "w21017" || dt.Rows[i]["PollutantCode"].ToString() == "w21001" || dt.Rows[i]["PollutantCode"].ToString() == "w21003") ? limit.Upper.Value.ToString("0.0") : limit.Upper.Value.ToString().TrimEnd('0').TrimEnd('.'));
                        }
                    }
                    else if (limit.Low != null)
                    {
                        dt.Rows[i]["Low"] = limit.Low;
                        dt.Rows[i]["ClassBase"] = "≥" + ((dt.Rows[i]["PollutantCode"].ToString() == "w21017" || dt.Rows[i]["PollutantCode"].ToString() == "w21001" || dt.Rows[i]["PollutantCode"].ToString() == "w21003") ? limit.Low.Value.ToString("0.0") : limit.Low.Value.ToString().TrimEnd('0').TrimEnd('.'));
                    }
                    else if (limit.Upper != null)
                    {
                        dt.Rows[i]["Upper"] = limit.Upper;
                        dt.Rows[i]["ClassBase"] = "≤" + ((dt.Rows[i]["PollutantCode"].ToString() == "w21017" || dt.Rows[i]["PollutantCode"].ToString() == "w21001" || dt.Rows[i]["PollutantCode"].ToString() == "w21003") ? limit.Upper.Value.ToString("0.0") : limit.Upper.Value.ToString().TrimEnd('0').TrimEnd('.'));
                    }
                }
            }
            return dt;
        }

        /// <summary>
        /// 水质自动监测子站运行情况表
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="pointId"></param>
        /// <param name="factors"></param>
        public void MoveToMF1(DocumentBuilder builder, DataTable dt, DataTable dt2)
        {
            builder.Font.ClearFormatting();
            builder.CellFormat.Borders.LineStyle = LineStyle.Single;
            builder.CellFormat.Borders.Color = System.Drawing.Color.Black;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            builder.Font.Size = 10;
            List<double> widthList = new List<double>();
            for (int i = 0; i < 10; i++)
            {
                double width = 0;
                width = 64.345;
                widthList.Add(width);
            }
            int RunningDaysCount = 0;  //统计运行天数
            int MonitorDatasCount = 0;  //统计监测数据
            int QualifiedDatasCount = 0;  //统计有效数据
            int ShouldDatasCount = 0;   //统计应测数据
            double buhuolv = 0.00;  //统计数据捕获率
            double youxiaolv = 0.00;    //统计数据有效率
            double yunxinglv = 0.00;    //统计有效运行率
            for (int i = 0; i <= dt.Rows.Count + 4; i++)
            {
                if (i == 0)
                {
                    builder.Font.Bold = true;
                    for (int j = 0; j < 10; j++)
                    {
                        builder.InsertCell();
                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                        builder.CellFormat.Width = widthList[j];
                        switch (j)
                        {
                            case 0:
                                builder.CellFormat.VerticalMerge = CellMerge.First;
                                builder.Write("序号");
                                break;
                            case 1:
                                builder.CellFormat.VerticalMerge = CellMerge.First;
                                builder.Write("子站名称");
                                break;
                            case 2:
                                builder.CellFormat.VerticalMerge = CellMerge.First;
                                builder.Write("本月天数");
                                break;
                            case 3:
                                builder.CellFormat.VerticalMerge = CellMerge.First;
                                builder.Write("运行天数");
                                break;
                            case 4:
                                //builder.CellFormat.VerticalMerge = CellMerge.None;
                                builder.CellFormat.HorizontalMerge = CellMerge.First;
                                builder.Write("监测数据");
                                break;
                            case 5:
                                //builder.CellFormat.VerticalMerge = CellMerge.None;
                                builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                                builder.Write("监测数据");
                                break;
                            case 6:
                                builder.CellFormat.VerticalMerge = CellMerge.First;
                                builder.CellFormat.HorizontalMerge = CellMerge.None;
                                builder.Write("有效数据");
                                break;
                            case 7:
                                builder.CellFormat.VerticalMerge = CellMerge.First;
                                builder.Write("数据捕获率");
                                break;
                            case 8:
                                builder.CellFormat.VerticalMerge = CellMerge.First;
                                builder.Write("数据有效率");
                                break;
                            case 9:
                                builder.CellFormat.VerticalMerge = CellMerge.First;
                                builder.Write("有效运行率");
                                break;
                        }
                    }
                    builder.EndRow();
                }
                else if (i == 1)
                {
                    builder.Font.Bold = true;
                    for (int j = 0; j < 10; j++)
                    {
                        builder.InsertCell();
                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                        builder.CellFormat.Width = widthList[j];
                        switch (j)
                        {
                            case 0:
                                builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                break;
                            case 1:
                                builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                break;
                            case 2:
                                builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                break;
                            case 3:
                                builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                break;
                            case 4:
                                builder.CellFormat.VerticalMerge = CellMerge.None;
                                builder.Write("应测");
                                break;
                            case 5:
                                builder.CellFormat.VerticalMerge = CellMerge.None;
                                builder.Write("实测");
                                break;
                            case 6:
                                builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                break;
                            case 7:
                                builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                break;
                            case 8:
                                builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                break;
                            case 9:
                                builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                break;
                        }
                    }
                    builder.EndRow();
                }
                else if (i == 2)
                {
                    builder.Font.Bold = true;
                    for (int j = 0; j < 10; j++)
                    {
                        builder.InsertCell();
                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                        builder.CellFormat.Width = widthList[j];
                        switch (j)
                        {
                            case 0:
                                builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                break;
                            case 1:
                                builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                break;
                            case 2:
                                builder.CellFormat.VerticalMerge = CellMerge.None;
                                builder.Write("(天)");
                                break;
                            case 3:
                                builder.CellFormat.VerticalMerge = CellMerge.None;
                                builder.Write("(天)");
                                break;
                            case 4:
                                builder.CellFormat.VerticalMerge = CellMerge.None;
                                builder.Write("(个)");
                                break;
                            case 5:
                                builder.CellFormat.VerticalMerge = CellMerge.None;
                                builder.Write("(个)");
                                break;
                            case 6:
                                builder.CellFormat.VerticalMerge = CellMerge.None;
                                builder.Write("(个)");
                                break;
                            case 7:
                                builder.CellFormat.VerticalMerge = CellMerge.None;
                                builder.Write("(%)");
                                break;
                            case 8:
                                builder.CellFormat.VerticalMerge = CellMerge.None;
                                builder.Write("(%)");
                                break;
                            case 9:
                                builder.CellFormat.VerticalMerge = CellMerge.None;
                                builder.Write("(%)");
                                break;
                        }
                    }
                    builder.EndRow();
                }
                else if (i > 2 && i <= dt.Rows.Count + 2)
                {
                    builder.Font.Bold = false;
                    for (int j = 0; j < 10; j++)
                    {
                        builder.InsertCell();
                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                        builder.CellFormat.Width = widthList[j];
                        switch (j)
                        {
                            case 0:
                                builder.CellFormat.VerticalMerge = CellMerge.None;
                                builder.Write((i - 2).ToString());
                                break;
                            case 1:
                                builder.CellFormat.VerticalMerge = CellMerge.None;
                                builder.Write(dt.Rows[i - 3]["MonitoringPointName"].ToString());
                                break;
                            case 2: builder.Write(beginTime.DaysInMonth().ToString()); break;
                            case 3:
                                int RD = dt.Rows[i - 3]["RunningDays"] != DBNull.Value ? Convert.ToInt32(dt.Rows[i - 3]["RunningDays"]) : 0;
                                builder.Write(RD.ToString());
                                RunningDaysCount += RD;
                                break;
                            case 4:
                                string id = dt.Rows[i - 3]["PointId"].ToString();
                                foreach (DataRow dr in dt2.Rows)
                                {
                                    if (dr["PointId"].ToString().Equals(id))
                                    {
                                        string ShouldCount = dr["ShouldCount"].ToString();
                                        ShouldDatasCount += Convert.ToInt32(dr["ShouldCount"]);
                                        builder.Write(ShouldCount);
                                    }
                                    //else
                                    //{
                                    //    ShouldDatasCount += 0;
                                    //    builder.Write("/");
                                    //}
                                }
                                //string ShouldCount = (beginTime.DaysInMonth() * 42 * 6).ToString();
                                //builder.Write(ShouldCount);
                                break;
                            case 5:
                                builder.Write(dt.Rows[i - 3]["CollectionCount"].ToString());
                                MonitorDatasCount += Convert.ToInt32(dt.Rows[i - 3]["CollectionCount"]);
                                break;
                            case 6:
                                builder.Write(dt.Rows[i - 3]["QualifiedCount"].ToString());
                                QualifiedDatasCount += Convert.ToInt32(dt.Rows[i - 3]["QualifiedCount"]);
                                break;
                            case 7:
                                double sc = double.Parse(dt.Rows[i - 3]["CollectionCount"].ToString());
                                //double yc = double.Parse((beginTime.DaysInMonth() * 42 * 6).ToString());
                                string id2 = dt.Rows[i - 3]["PointId"].ToString();
                                foreach (DataRow dr in dt2.Rows)
                                {
                                    if (dr["PointId"].ToString().Equals(id2))
                                    {
                                        double ShouldCount = double.Parse(dr["ShouldCount"].ToString());
                                        builder.Write(((sc / ShouldCount * 100).ToString().Substring(0, 5)) + "%");
                                        buhuolv += sc / ShouldCount * 100;
                                    }
                                    //else
                                    //{
                                    //    builder.Write("/");
                                    //    buhuolv += 0;
                                    //}
                                }
                                break;
                            case 8:
                                string effectRate = "";
                                if (dt.Rows[i - 3]["CollectionCount"] != DBNull.Value && dt.Rows[i - 3]["QualifiedCount"] != DBNull.Value && Convert.ToInt32(dt.Rows[i - 3]["CollectionCount"]) != 0)
                                    effectRate = (Convert.ToDecimal(dt.Rows[i - 3]["QualifiedCount"]) / Convert.ToDecimal(dt.Rows[i - 3]["CollectionCount"])).ToString("0.00%");
                                builder.Write(effectRate);
                                youxiaolv += (double.Parse((dt.Rows[i - 3]["QualifiedCount"]).ToString()) / double.Parse((dt.Rows[i - 3]["CollectionCount"]).ToString())) * 100;
                                break;
                            case 9:
                                double yx = double.Parse(dt.Rows[i - 3]["QualifiedCount"].ToString());
                                //double yc2 = double.Parse((beginTime.DaysInMonth() * 42 * 6).ToString());
                                string id3 = dt.Rows[i - 3]["PointId"].ToString();
                                foreach (DataRow dr in dt2.Rows)
                                {
                                    if (dr["PointId"].ToString().Equals(id3))
                                    {
                                        double ShouldCount = double.Parse(dr["ShouldCount"].ToString());
                                        builder.Write(((yx / ShouldCount * 100).ToString().Substring(0, 5)) + "%");
                                        yunxinglv += yx / ShouldCount * 100;
                                    }
                                    //else
                                    //{
                                    //    builder.Write("/");
                                    //    yunxinglv += 0;
                                    //}
                                }
                                break;
                        }
                    }
                    builder.EndRow();
                }
                else if (i == dt.Rows.Count + 3)
                {
                    builder.Font.Bold = false;
                    for (int j = 0; j < 10; j++)
                    {
                        builder.InsertCell();
                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                        builder.CellFormat.Width = widthList[j];
                        switch (j)
                        {
                            case 0:
                                builder.CellFormat.HorizontalMerge = CellMerge.First;
                                builder.Write("合计");
                                break;
                            case 1:
                                builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                                break;
                            case 2:
                                builder.CellFormat.HorizontalMerge = CellMerge.None;
                                builder.Write("/"); break;
                            case 3: builder.Write("/"); break;
                            //case 4:
                            //    builder.CellFormat.HorizontalMerge = CellMerge.None;
                            //    builder.Write((beginTime.DaysInMonth() * dt.Rows.Count).ToString());
                            //    break;
                            //case 5: builder.Write(RunningDaysCount.ToString()); break;
                            case 4:
                                builder.Write(ShouldDatasCount.ToString());
                                break;
                            case 5: builder.Write(MonitorDatasCount.ToString()); break;
                            case 6: builder.Write(QualifiedDatasCount.ToString()); break;
                            case 7:
                                builder.Write(((buhuolv / (dt.Rows.Count)).ToString().Substring(0, 5)) + "%"); break;
                            case 8:
                                builder.Write(((youxiaolv / (dt.Rows.Count)).ToString().Substring(0, 5)) + "%"); break;
                            case 9:
                                builder.Write(((yunxinglv / (dt.Rows.Count)).ToString().Substring(0, 5)) + "%"); break;
                        }
                    }
                    builder.EndRow();
                }

                //else
                //{
                //    builder.Font.Bold = false;
                //    for (int j = 0; j < 7; j++)
                //    {
                //        builder.InsertCell();
                //        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                //        builder.CellFormat.Width = widthList[j];
                //        switch (j)
                //        {
                //            case 0:
                //                builder.CellFormat.HorizontalMerge = CellMerge.First;
                //                builder.Write("平均值");
                //                break;
                //            case 1:
                //                builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                //                break;
                //            case 2:
                //                builder.CellFormat.HorizontalMerge = CellMerge.None;
                //                builder.Write(beginTime.DaysInMonth().ToString());
                //                break;
                //            case 3:
                //                if (dt.Rows.Count != 0)
                //                    builder.Write((Convert.ToDecimal(RunningDaysCount) / dt.Rows.Count).ToString("0"));
                //                break;
                //            case 4:
                //                if (dt.Rows.Count != 0)
                //                    builder.Write((Convert.ToDecimal(MonitorDatasCount) / dt.Rows.Count).ToString("0"));
                //                break;
                //            case 5:
                //                if (dt.Rows.Count != 0)
                //                    builder.Write((Convert.ToDecimal(QualifiedDatasCount) / dt.Rows.Count).ToString("0"));
                //                break;
                //            case 6:
                //                if (dt.Rows.Count != 0)
                //                    builder.Write((Convert.ToDecimal(QualifiedDatasCount / dt.Rows.Count) / Convert.ToDecimal(MonitorDatasCount / dt.Rows.Count)).ToString("0.00%"));
                //                break;
                //        }
                //    }
                //    builder.EndRow();
                //}
            }

            builder.EndTable();

        }

        #region MoveToMF2
        /// <summary>
        /// 各子站日监测结果
        /// </summary>
        /// <param name="dt"></param>
        public void MoveToMF2(DocumentBuilder builder, DataTable dtPoints, DataTable dtMonthAvgContent, DataTable dtFactorNum, ReportLogEntity customDatumData, string PageTypeId)
        {
            builder.ParagraphFormat.ClearFormatting();
            builder.Font.ClearFormatting();
            builder.Font.Bold = true;
            builder.ParagraphFormat.Alignment = Aspose.Words.ParagraphAlignment.Center;
            builder.CellFormat.Borders.LineStyle = LineStyle.Single;
            builder.CellFormat.Borders.Color = System.Drawing.Color.Black;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Font.Size = 10;

            int count = 2;
            for (int point = 0; point < dtPoints.Rows.Count; point++)
            {
                string pointId = dtPoints.Rows[point]["PointId"].ToString();
                string[] factorCodes = dtPoints.Rows[point]["PollutantCodes"].ToString().Split(';');
                string pointName = monitoringPointWaterService.RetrieveEntityByPointId(Convert.ToInt16(pointId)).MonitoringPointName;
                int[] Num = SettingTable(factorCodes, 7);
                string writeIn = "";
                if (pointId == "41")
                {
                    DataTable dtPFactorsNum = new DataTable();
                    DataTable dtPFactors = GetFactorsRelation(out dtPFactorsNum, customDatumData, PageTypeId, "C");
                    //常规因子
                    DataRow[] RegularFactors = dtPFactors.Select("PointId=41 and PollutantType not in('" + VOCGuid + "','" + AlgaGuid + "')");
                    string strRegularFactors = "";
                    for (int factor = 0; factor < RegularFactors.Length; factor++)
                    {
                        strRegularFactors += RegularFactors[factor]["PollutantCodes"] + ";";
                    }
                    string[] arrRegularFactors = strRegularFactors.TrimEnd(';').Split(';');
                    //非常规因子
                    DataRow[] IRRegularFactors = dtPFactors.Select("PointId=41 and PollutantType in('" + VOCGuid + "','" + AlgaGuid + "')");
                    //string strIRRegularFactors = "";
                    //for (int factor = 0; factor < IRRegularFactors.Length; factor++)
                    //{
                    //    strIRRegularFactors += IRRegularFactors[factor]["PollutantCodes"] + ";";
                    //}
                    //string[] arrIRRegularFactors = strIRRegularFactors.TrimEnd(';').Split(';');
                    //数据处理
                    DataRow[] Regular = dtMonthAvgContent.Select("PointId=41 and CategoryUid not in('" + VOCGuid + "','" + AlgaGuid + "')");
                    DataRow[] IRRegular = dtMonthAvgContent.Select("PointId=41 and CategoryUid in('" + VOCGuid + "','" + AlgaGuid + "')");
                    if (RegularFactors.Length > 0 && IRRegularFactors.Length == 0)
                    {
                        writeIn = "表" + count + "    " + pointName + "子站常规指标月均监测结果统计表";
                        builder.Write(writeIn);
                        int[] num = SettingTable(arrRegularFactors, 7);
                        FillMonthData(builder, dtMonthAvgContent, dtFactorNum, arrRegularFactors, pointId, num[0], num[1]);
                        count++;
                    }
                    else if (RegularFactors.Length == 0 && IRRegularFactors.Length > 0)
                    {
                        writeIn = "表" + count + "    " + pointName + "子站特征指标月均监测结果统计表";
                        builder.Write(writeIn);
                        for (int ir = 0; ir < IRRegularFactors.Length; ir++)
                        {
                            string[] arrIRRegularFactors2 = IRRegularFactors[ir]["PollutantCodes"].ToString().Split(';');
                            int[] num = SettingTable(arrIRRegularFactors2, 7);
                            FillMonthData(builder, dtMonthAvgContent, dtFactorNum, arrIRRegularFactors2, pointId, num[0], num[1]);
                        }
                        count++;
                    }
                    else if (RegularFactors.Length > 0 && IRRegularFactors.Length > 0)
                    {
                        writeIn = "表" + count + "    " + pointName + "子站常规指标月均监测结果统计表";
                        builder.Write(writeIn);
                        int[] num1 = SettingTable(arrRegularFactors, 7);
                        FillMonthData(builder, dtMonthAvgContent, dtFactorNum, arrRegularFactors, pointId, num1[0], num1[1]);
                        count++;
                        writeIn = "表" + count + "    " + pointName + "子站特征指标月均监测结果统计表";
                        builder.Write(writeIn);
                        int[] num2 = SettingTable(IRRegularFactors[0]["PollutantCodes"].ToString().Split(';'), 7);
                        for (int ir = 0; ir < IRRegularFactors.Length; ir++)
                        {
                            string[] arrIRRegularFactors2 = IRRegularFactors[ir]["PollutantCodes"].ToString().Split(';');
                            int[] num3 = SettingTable(IRRegularFactors[ir]["PollutantCodes"].ToString().Split(';'), 7);
                            FillMonthData2(builder, dtMonthAvgContent, dtFactorNum, arrIRRegularFactors2, pointId, num3[0], num2[1]);
                        }
                        builder.Write("\r\n");
                        count++;
                    }
                }
                else if (pointId == "53")
                {
                    writeIn = "表" + count + "    " + pointName + "子站月均监测结果统计表";
                    builder.Write(writeIn);
                    for (int i = 0; i < 9; i++)
                    {
                        for (int j = 0; j < factorCodes.Length + 5; j++)
                        {
                            builder.InsertCell();
                            builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                            builder.CellFormat.HorizontalMerge = CellMerge.None;
                            builder.CellFormat.VerticalMerge = CellMerge.None;
                            switch (i)
                            {
                                case 0:
                                    {
                                        builder.Font.Bold = true;
                                        if (j == 0)
                                        {
                                            builder.CellFormat.VerticalMerge = CellMerge.First;
                                            builder.CellFormat.Width = 80;
                                            builder.Write("监测项目");
                                        }
                                        else if (j == 1)
                                        {
                                            builder.CellFormat.HorizontalMerge = CellMerge.First;

                                            builder.Write("  流速  ");
                                        }
                                        else if (j == 2)
                                        {
                                            builder.CellFormat.HorizontalMerge = CellMerge.Previous;

                                        }
                                        else if (j == 3)
                                        {
                                            builder.CellFormat.HorizontalMerge = CellMerge.First;

                                            builder.Write("  流量  ");
                                        }
                                        else if (j == 4)
                                        {
                                            builder.CellFormat.HorizontalMerge = CellMerge.Previous;

                                        }
                                        else if (j == 5)
                                        {
                                            builder.CellFormat.HorizontalMerge = CellMerge.First;

                                            builder.Write("总水量");
                                        }
                                        else if (j == 6)
                                        {
                                            builder.CellFormat.HorizontalMerge = CellMerge.Previous;

                                        }
                                        else if (factorCodes[j - 5] != "w01028" || factorCodes[j - 5] != "w01029")
                                        {
                                            builder.CellFormat.VerticalMerge = CellMerge.First;
                                            builder.Write(m_WaterPollutantService.GetPollutantInfo(factorCodes[j - 5]).PollutantName);
                                        }
                                        break;
                                    }
                                case 1:
                                    {
                                        builder.Font.Bold = true;
                                        if (j == 0)
                                        {
                                            builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                            builder.CellFormat.Width = 80;
                                        }
                                        else if (j == 1)
                                        {

                                            builder.Write("出湖");
                                        }
                                        else if (j == 2)
                                        {

                                            builder.Write("入湖");
                                        }
                                        else if (j == 3)
                                        {

                                            builder.Write("出湖");
                                        }
                                        else if (j == 4)
                                        {

                                            builder.Write("入湖");
                                        }
                                        else if (j == 5)
                                        {

                                            builder.Write("出湖");
                                        }
                                        else if (j == 6)
                                        {

                                            builder.Write("入湖");
                                        }
                                        else if (factorCodes[j - 5] != "w01028" || factorCodes[j - 5] != "w01029")
                                        {
                                            builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        }
                                        break;
                                    }
                                case 2:
                                    {
                                        builder.Font.Bold = false;
                                        if (j == 0)
                                        {
                                            builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                            builder.CellFormat.Width = 80;
                                        }
                                        else if (j == 1)
                                        {

                                            builder.Write("m/s");
                                        }
                                        else if (j == 2)
                                        {

                                            builder.Write("m/s");
                                        }
                                        else if (j == 3)
                                        {

                                            builder.InsertHtml("<b>m<SUP>3</SUP>/s</b>");
                                        }
                                        else if (j == 4)
                                        {

                                            builder.InsertHtml("<b>m<SUP>3</SUP>/s</b>");
                                        }
                                        else if (j == 5)
                                        {

                                            builder.InsertHtml("<b>×10<SUP>5</SUP>m<SUP>3</SUP></b>");
                                        }
                                        else if (j == 6)
                                        {

                                            builder.InsertHtml("<b>×10<SUP>5</SUP>m<SUP>3</SUP></b>");
                                        }
                                        else if (factorCodes[j - 5] != "w01028" || factorCodes[j - 5] != "w01029")
                                        {
                                            string factorUnit = "";
                                            factorUnit = m_WaterPollutantService.GetPollutantInfo(factorCodes[j - 5]).PollutantMeasureUnit != null ? m_WaterPollutantService.GetPollutantInfo(factorCodes[j - 5]).PollutantMeasureUnit : "";
                                            builder.Write(factorUnit);
                                        }
                                        break;
                                    }
                                case 3:
                                    {
                                        builder.Font.Bold = false;
                                        FillMonthDataLTG(builder, dtMonthAvgContent, dtFactorNum, factorCodes, pointId, j, "最大值");
                                        break;
                                    }
                                case 4:
                                    {
                                        builder.Font.Bold = false;
                                        FillMonthDataLTG(builder, dtMonthAvgContent, dtFactorNum, factorCodes, pointId, j, "最小值");
                                        break;
                                    }
                                case 5:
                                    {
                                        builder.Font.Bold = false;
                                        FillMonthDataLTG(builder, dtMonthAvgContent, dtFactorNum, factorCodes, pointId, j, "平均值");
                                        break;
                                    }
                                case 6:
                                    {
                                        builder.Font.Bold = false;
                                        FillMonthDataLTG(builder, dtMonthAvgContent, dtFactorNum, factorCodes, pointId, j, "上月均值");
                                        break;
                                    }
                                case 7:
                                    {
                                        builder.Font.Bold = false;
                                        FillMonthDataLTG(builder, dtMonthAvgContent, dtFactorNum, factorCodes, pointId, j, "去年同期均值");
                                        break;
                                    }
                                case 8:
                                    {
                                        if (j == 0)
                                        {
                                            builder.Font.Bold = true;
                                            builder.Write("标准限值");
                                        }
                                        else if (j == 1 || j == 2)
                                        {
                                            builder.Font.Bold = false;
                                            DataRow[] dr = dtMonthAvgContent.Select("PointId='" + pointId + "' and PollutantCode='w01028'");
                                            builder.Write(dr.Length == 0 ? "/" : (dr[0]["ClassBase"] == DBNull.Value ? "/" : dr[0]["ClassBase"].ToString()));
                                        }
                                        else if (j == 3 || j == 4)
                                        {
                                            builder.Font.Bold = false;
                                            DataRow[] dr = dtMonthAvgContent.Select("PointId='" + pointId + "' and PollutantCode='w01029'");
                                            builder.Write(dr.Length == 0 ? "/" : (dr[0]["ClassBase"] == DBNull.Value ? "/" : dr[0]["ClassBase"].ToString()));
                                        }
                                        else if (j == 5 || j == 6)
                                        {
                                            builder.Font.Bold = false;
                                            builder.Write("/");
                                        }
                                        else if (factorCodes[j - 5] != "w01028" || factorCodes[j - 5] != "w01029")
                                        {
                                            builder.Font.Bold = false;
                                            DataRow[] dr = dtMonthAvgContent.Select("PointId='" + pointId + "' and PollutantCode='" + factorCodes[j - 5] + "'");
                                            builder.Write(dr.Length == 0 ? "/" : (dr[0]["ClassBase"] == DBNull.Value ? "/" : dr[0]["ClassBase"].ToString()));
                                        }
                                        break;
                                    }
                            }
                        }
                        builder.EndRow();
                    }
                    builder.EndTable();
                    count++;
                }
                else
                {
                    writeIn = "表" + count + "    " + pointName + "子站月均监测结果统计表";
                    builder.Write(writeIn);
                    FillMonthData(builder, dtMonthAvgContent, dtFactorNum, factorCodes, pointId, Num[0], Num[1]);
                    count++;
                }
            }
        }

        /// <summary>
        /// 填充表单
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="dtMonthAvgContent"></param>
        /// <param name="factorCodes"></param>
        /// <param name="pointId"></param>
        /// <param name="NumOfRow"></param>
        /// <param name="NumOfCol"></param>
        public void FillMonthData(DocumentBuilder builder, DataTable dtMonthAvgContent, DataTable dtFactorNum, string[] factorCodes, string pointId, int NumOfRow, int NumOfCol)
        {
            //填充表格
            for (int k = 0; k < NumOfRow; k++)
            {
                for (int i = 0; i < 8; i++)
                {
                    builder.Font.Color = System.Drawing.Color.Black;
                    for (int j = 0; j < NumOfCol + 1; j++)
                    {
                        builder.InsertCell();
                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                        builder.CellFormat.VerticalMerge = CellMerge.None;
                        switch (i)
                        {
                            case 0:
                                {
                                    if (j == 0)
                                    {
                                        builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Font.Bold = true;
                                        builder.CellFormat.Width = 80;
                                        builder.Write("监测项目");
                                    }
                                    else if (j > 0 && k * NumOfCol + j <= factorCodes.Length)
                                    {
                                        builder.Write(m_WaterPollutantService.GetPollutantInfo(factorCodes[k * NumOfCol + j - 1]).PollutantName);
                                    }
                                    break;
                                }
                            case 1:
                                {
                                    if (j == 0)
                                    {
                                        builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        builder.CellFormat.Width = 80;
                                    }
                                    else if (j > 0 && k * NumOfCol + j <= factorCodes.Length)
                                    {
                                        string factorUnit = "";
                                        factorUnit = m_WaterPollutantService.GetPollutantInfo(factorCodes[k * NumOfCol + j - 1]).PollutantMeasureUnit != null ? m_WaterPollutantService.GetPollutantInfo(factorCodes[k * NumOfCol + j - 1]).PollutantMeasureUnit : "";
                                        builder.Write(factorUnit);
                                    }
                                    break;
                                }
                            case 2:
                                {
                                    builder.Font.Bold = false;
                                    if (j == 0)
                                    {
                                        builder.CellFormat.Width = 80;
                                        builder.Write("最大值");
                                    }
                                    else if (j > 0 && k * NumOfCol + j <= factorCodes.Length)
                                    {
                                        int num = 2;
                                        DataRow[] drFactorNum = dtFactorNum.Select("PointId='" + pointId + "' and PollutantCode='" + factorCodes[k * NumOfCol + j - 1] + "'");
                                        if (drFactorNum.Length > 0)
                                        {
                                            if (drFactorNum[0]["DecimalDigit"] != DBNull.Value && int.TryParse(drFactorNum[0]["DecimalDigit"].ToString(), out num))
                                            {
                                                num = int.Parse(drFactorNum[0]["DecimalDigit"].ToString());
                                            }
                                            else
                                            {
                                                num = m_WaterPollutantService.GetPollutantInfo(factorCodes[k * NumOfCol + j - 1]).PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo(factorCodes[k * NumOfCol + j - 1]).PollutantDecimalNum) : 2;
                                            }
                                        }
                                        else
                                        {
                                            num = m_WaterPollutantService.GetPollutantInfo(factorCodes[k * NumOfCol + j - 1]).PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo(factorCodes[k * NumOfCol + j - 1]).PollutantDecimalNum) : 2;
                                        }

                                        DataRow[] dr = dtMonthAvgContent.Select("PointId='" + pointId + "' and PollutantCode='" + factorCodes[k * NumOfCol + j - 1] + "'");
                                        if (dr.Length != 0 && dr[0]["Value_Max"] != DBNull.Value)
                                        {
                                            decimal value = (factorCodes[k * NumOfCol + j - 1] == "w02023") ? Convert.ToDecimal(dr[0]["Value_Max"]) / 1000 : Convert.ToDecimal(dr[0]["Value_Max"]);
                                            if ((dr[0]["Upper"] != DBNull.Value && value > Convert.ToDecimal(dr[0]["Upper"])) || (dr[0]["Low"] != DBNull.Value && value < Convert.ToDecimal(dr[0]["Low"])))
                                                builder.Font.Color = System.Drawing.Color.Red;
                                            else
                                                builder.Font.Color = System.Drawing.Color.Black;
                                            builder.Write(DecimalExtension.GetPollutantValue(value, num).ToString());
                                        }
                                        else
                                        {
                                            builder.Write("/");
                                        }
                                    }
                                    break;
                                }
                            case 3:
                                {
                                    builder.Font.Bold = false;
                                    if (j == 0)
                                    {
                                        builder.CellFormat.Width = 80;
                                        builder.Write("最小值");
                                    }
                                    else if (j > 0 && k * NumOfCol + j <= factorCodes.Length)
                                    {
                                        int num = 2;
                                        DataRow[] drFactorNum = dtFactorNum.Select("PointId='" + pointId + "' and PollutantCode='" + factorCodes[k * NumOfCol + j - 1] + "'");
                                        if (drFactorNum.Length > 0)
                                        {
                                            if (drFactorNum[0]["DecimalDigit"] != DBNull.Value && int.TryParse(drFactorNum[0]["DecimalDigit"].ToString(), out num))
                                            {
                                                num = int.Parse(drFactorNum[0]["DecimalDigit"].ToString());
                                            }
                                            else
                                            {
                                                num = m_WaterPollutantService.GetPollutantInfo(factorCodes[k * NumOfCol + j - 1]).PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo(factorCodes[k * NumOfCol + j - 1]).PollutantDecimalNum) : 2;
                                            }
                                        }
                                        else
                                        {
                                            num = m_WaterPollutantService.GetPollutantInfo(factorCodes[k * NumOfCol + j - 1]).PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo(factorCodes[k * NumOfCol + j - 1]).PollutantDecimalNum) : 2;
                                        }

                                        DataRow[] dr = dtMonthAvgContent.Select("PointId='" + pointId + "' and PollutantCode='" + factorCodes[k * NumOfCol + j - 1] + "'");
                                        if (dr.Length != 0 && dr[0]["Value_Min"] != DBNull.Value)
                                        {
                                            decimal value = (factorCodes[k * NumOfCol + j - 1] == "w02023") ? Convert.ToDecimal(dr[0]["Value_Min"]) / 1000 : Convert.ToDecimal(dr[0]["Value_Min"]);
                                            if ((dr[0]["Upper"] != DBNull.Value && value > Convert.ToDecimal(dr[0]["Upper"])) || (dr[0]["Low"] != DBNull.Value && value < Convert.ToDecimal(dr[0]["Low"])))
                                                builder.Font.Color = System.Drawing.Color.Red;
                                            else
                                                builder.Font.Color = System.Drawing.Color.Black;
                                            builder.Write(DecimalExtension.GetPollutantValue(value, num).ToString());
                                        }
                                        else
                                        {
                                            builder.Write("/");
                                        }
                                    }
                                    break;
                                }
                            case 4:
                                {
                                    builder.Font.Bold = false;
                                    if (j == 0)
                                    {
                                        builder.CellFormat.Width = 80;
                                        builder.Write("平均值");
                                    }
                                    else if (j > 0 && k * NumOfCol + j <= factorCodes.Length)
                                    {
                                        int num = 2;
                                        DataRow[] drFactorNum = dtFactorNum.Select("PointId='" + pointId + "' and PollutantCode='" + factorCodes[k * NumOfCol + j - 1] + "'");
                                        if (drFactorNum.Length > 0)
                                        {
                                            if (drFactorNum[0]["DecimalDigit"] != DBNull.Value && int.TryParse(drFactorNum[0]["DecimalDigit"].ToString(), out num))
                                            {
                                                num = int.Parse(drFactorNum[0]["DecimalDigit"].ToString());
                                            }
                                            else
                                            {
                                                num = m_WaterPollutantService.GetPollutantInfo(factorCodes[k * NumOfCol + j - 1]).PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo(factorCodes[k * NumOfCol + j - 1]).PollutantDecimalNum) : 2;
                                            }
                                        }
                                        else
                                        {
                                            num = m_WaterPollutantService.GetPollutantInfo(factorCodes[k * NumOfCol + j - 1]).PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo(factorCodes[k * NumOfCol + j - 1]).PollutantDecimalNum) : 2;
                                        }

                                        DataRow[] dr = dtMonthAvgContent.Select("PointId='" + pointId + "' and PollutantCode='" + factorCodes[k * NumOfCol + j - 1] + "'");
                                        if (dr.Length != 0 && dr[0]["Value_Avg"] != DBNull.Value)
                                        {
                                            decimal value = (factorCodes[k * NumOfCol + j - 1] == "w02023") ? Convert.ToDecimal(dr[0]["Value_Avg"]) / 1000 : Convert.ToDecimal(dr[0]["Value_Avg"]);
                                            if ((dr[0]["Upper"] != DBNull.Value && value > Convert.ToDecimal(dr[0]["Upper"])) || (dr[0]["Low"] != DBNull.Value && value < Convert.ToDecimal(dr[0]["Low"])))
                                                builder.Font.Color = System.Drawing.Color.Red;
                                            else
                                                builder.Font.Color = System.Drawing.Color.Black;
                                            builder.Write(DecimalExtension.GetPollutantValue(value, num).ToString());
                                        }
                                        else
                                        {
                                            builder.Write("/");
                                        }
                                    }
                                    break;
                                }
                            case 5:
                                {
                                    builder.Font.Bold = false;
                                    if (j == 0)
                                    {
                                        builder.CellFormat.Width = 80;
                                        builder.Write("上月均值");
                                    }
                                    else if (j > 0 && k * NumOfCol + j <= factorCodes.Length)
                                    {
                                        int num = 2;
                                        DataRow[] drFactorNum = dtFactorNum.Select("PointId='" + pointId + "' and PollutantCode='" + factorCodes[k * NumOfCol + j - 1] + "'");
                                        if (drFactorNum.Length > 0)
                                        {
                                            if (drFactorNum[0]["DecimalDigit"] != DBNull.Value && int.TryParse(drFactorNum[0]["DecimalDigit"].ToString(), out num))
                                            {
                                                num = int.Parse(drFactorNum[0]["DecimalDigit"].ToString());
                                            }
                                            else
                                            {
                                                num = m_WaterPollutantService.GetPollutantInfo(factorCodes[k * NumOfCol + j - 1]).PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo(factorCodes[k * NumOfCol + j - 1]).PollutantDecimalNum) : 2;
                                            }
                                        }
                                        else
                                        {
                                            num = m_WaterPollutantService.GetPollutantInfo(factorCodes[k * NumOfCol + j - 1]).PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo(factorCodes[k * NumOfCol + j - 1]).PollutantDecimalNum) : 2;
                                        }

                                        DataRow[] dr = dtMonthAvgContent.Select("PointId='" + pointId + "' and PollutantCode='" + factorCodes[k * NumOfCol + j - 1] + "'");
                                        if (dr.Length != 0 && dr[0]["LastMonthAvg"] != DBNull.Value)
                                        {
                                            decimal value = (factorCodes[k * NumOfCol + j - 1] == "w02023") ? Convert.ToDecimal(dr[0]["LastMonthAvg"]) / 1000 : Convert.ToDecimal(dr[0]["LastMonthAvg"]);
                                            if ((dr[0]["Upper"] != DBNull.Value && value > Convert.ToDecimal(dr[0]["Upper"])) || (dr[0]["Low"] != DBNull.Value && value < Convert.ToDecimal(dr[0]["Low"])))
                                                builder.Font.Color = System.Drawing.Color.Red;
                                            else
                                                builder.Font.Color = System.Drawing.Color.Black;
                                            builder.Write(DecimalExtension.GetPollutantValue(value, num).ToString());
                                        }
                                        else
                                        {
                                            builder.Write("/");
                                        }
                                    }
                                    break;
                                }
                            case 6:
                                {
                                    builder.Font.Bold = false;
                                    if (j == 0)
                                    {
                                        builder.CellFormat.Width = 80;
                                        builder.Write("去年同期均值");
                                    }
                                    else if (j > 0 && k * NumOfCol + j <= factorCodes.Length)
                                    {
                                        int num = 2;
                                        DataRow[] drFactorNum = dtFactorNum.Select("PointId='" + pointId + "' and PollutantCode='" + factorCodes[k * NumOfCol + j - 1] + "'");
                                        if (drFactorNum.Length > 0)
                                        {
                                            if (drFactorNum[0]["DecimalDigit"] != DBNull.Value && int.TryParse(drFactorNum[0]["DecimalDigit"].ToString(), out num))
                                            {
                                                num = int.Parse(drFactorNum[0]["DecimalDigit"].ToString());
                                            }
                                            else
                                            {
                                                num = m_WaterPollutantService.GetPollutantInfo(factorCodes[k * NumOfCol + j - 1]).PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo(factorCodes[k * NumOfCol + j - 1]).PollutantDecimalNum) : 2;
                                            }
                                        }
                                        else
                                        {
                                            num = m_WaterPollutantService.GetPollutantInfo(factorCodes[k * NumOfCol + j - 1]).PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo(factorCodes[k * NumOfCol + j - 1]).PollutantDecimalNum) : 2;
                                        }

                                        DataRow[] dr = dtMonthAvgContent.Select("PointId='" + pointId + "' and PollutantCode='" + factorCodes[k * NumOfCol + j - 1] + "'");
                                        if (dr.Length != 0 && dr[0]["LastYearMonthAvg"] != DBNull.Value)
                                        {
                                            decimal value = (factorCodes[k * NumOfCol + j - 1] == "w02023") ? Convert.ToDecimal(dr[0]["LastYearMonthAvg"]) / 1000 : Convert.ToDecimal(dr[0]["LastYearMonthAvg"]);
                                            if ((dr[0]["Upper"] != DBNull.Value && value > Convert.ToDecimal(dr[0]["Upper"])) || (dr[0]["Low"] != DBNull.Value && value < Convert.ToDecimal(dr[0]["Low"])))
                                                builder.Font.Color = System.Drawing.Color.Red;
                                            else
                                                builder.Font.Color = System.Drawing.Color.Black;
                                            builder.Write(DecimalExtension.GetPollutantValue(value, num).ToString());
                                        }
                                        else
                                        {
                                            builder.Write("/");
                                        }
                                    }
                                    break;
                                }
                            case 7:
                                {
                                    if (j == 0)
                                    {
                                        builder.CellFormat.Width = 80;
                                        builder.Font.Bold = true;
                                        builder.Write("标准限值");
                                    }
                                    else if (j > 0 && k * NumOfCol + j <= factorCodes.Length)
                                    {
                                        builder.Font.Bold = false;
                                        DataRow[] dr = dtMonthAvgContent.Select("PointId='" + pointId + "' and PollutantCode='" + factorCodes[k * NumOfCol + j - 1] + "'");
                                        builder.Write(dr.Length == 0 ? "/" : (dr[0]["ClassBase"] == DBNull.Value ? "/" : dr[0]["ClassBase"].ToString()));
                                    }
                                    break;
                                }
                        }
                    }
                    builder.EndRow();
                }
            }

            builder.EndTable();
            builder.Write("\r\n");
        }

        public void FillMonthData2(DocumentBuilder builder, DataTable dtMonthAvgContent, DataTable dtFactorNum, string[] factorCodes, string pointId, int NumOfRow, int NumOfCol)
        {
            //填充表格
            for (int k = 0; k < NumOfRow; k++)
            {
                for (int i = 0; i < 8; i++)
                {
                    builder.Font.Color = System.Drawing.Color.Black;
                    for (int j = 0; j < NumOfCol + 1; j++)
                    {
                        builder.InsertCell();
                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                        builder.CellFormat.VerticalMerge = CellMerge.None;
                        switch (i)
                        {
                            case 0:
                                {
                                    if (j == 0)
                                    {
                                        builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Font.Bold = true;
                                        builder.CellFormat.Width = 80;
                                        builder.Write("监测项目");
                                    }
                                    else if (j > 0 && k * NumOfCol + j <= factorCodes.Length)
                                    {
                                        builder.Write(m_WaterPollutantService.GetPollutantInfo(factorCodes[k * NumOfCol + j - 1]).PollutantName);
                                    }
                                    break;
                                }
                            case 1:
                                {
                                    if (j == 0)
                                    {
                                        builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        builder.CellFormat.Width = 80;
                                    }
                                    else if (j > 0 && k * NumOfCol + j <= factorCodes.Length)
                                    {
                                        string factorUnit = "";
                                        factorUnit = m_WaterPollutantService.GetPollutantInfo(factorCodes[k * NumOfCol + j - 1]).PollutantMeasureUnit != null ? m_WaterPollutantService.GetPollutantInfo(factorCodes[k * NumOfCol + j - 1]).PollutantMeasureUnit : "";
                                        builder.Write(factorUnit);
                                    }
                                    break;
                                }
                            case 2:
                                {
                                    builder.Font.Bold = false;
                                    if (j == 0)
                                    {
                                        builder.CellFormat.Width = 80;
                                        builder.Write("最大值");
                                    }
                                    else if (j > 0 && k * NumOfCol + j <= factorCodes.Length)
                                    {
                                        int num = 2;
                                        DataRow[] drFactorNum = dtFactorNum.Select("PointId='" + pointId + "' and PollutantCode='" + factorCodes[k * NumOfCol + j - 1] + "'");
                                        if (drFactorNum.Length > 0)
                                        {
                                            if (drFactorNum[0]["DecimalDigit"] != DBNull.Value && int.TryParse(drFactorNum[0]["DecimalDigit"].ToString(), out num))
                                            {
                                                num = int.Parse(drFactorNum[0]["DecimalDigit"].ToString());
                                            }
                                            else
                                            {
                                                num = m_WaterPollutantService.GetPollutantInfo(factorCodes[k * NumOfCol + j - 1]).PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo(factorCodes[k * NumOfCol + j - 1]).PollutantDecimalNum) : 2;
                                            }
                                        }
                                        else
                                        {
                                            num = m_WaterPollutantService.GetPollutantInfo(factorCodes[k * NumOfCol + j - 1]).PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo(factorCodes[k * NumOfCol + j - 1]).PollutantDecimalNum) : 2;
                                        }

                                        DataRow[] dr = dtMonthAvgContent.Select("PointId='" + pointId + "' and PollutantCode='" + factorCodes[k * NumOfCol + j - 1] + "'");
                                        if (dr.Length != 0 && dr[0]["Value_Max"] != DBNull.Value)
                                        {
                                            decimal value = (factorCodes[k * NumOfCol + j - 1] == "w02023") ? Convert.ToDecimal(dr[0]["Value_Max"]) / 1000 : Convert.ToDecimal(dr[0]["Value_Max"]);
                                            if ((dr[0]["Upper"] != DBNull.Value && value > Convert.ToDecimal(dr[0]["Upper"])) || (dr[0]["Low"] != DBNull.Value && value < Convert.ToDecimal(dr[0]["Low"])))
                                                builder.Font.Color = System.Drawing.Color.Red;
                                            else
                                                builder.Font.Color = System.Drawing.Color.Black;
                                            builder.Write(DecimalExtension.GetPollutantValue(value, num).ToString());
                                        }
                                        else
                                        {
                                            builder.Write("/");
                                        }
                                    }
                                    break;
                                }
                            case 3:
                                {
                                    builder.Font.Bold = false;
                                    if (j == 0)
                                    {
                                        builder.CellFormat.Width = 80;
                                        builder.Write("最小值");
                                    }
                                    else if (j > 0 && k * NumOfCol + j <= factorCodes.Length)
                                    {
                                        int num = 2;
                                        DataRow[] drFactorNum = dtFactorNum.Select("PointId='" + pointId + "' and PollutantCode='" + factorCodes[k * NumOfCol + j - 1] + "'");
                                        if (drFactorNum.Length > 0)
                                        {
                                            if (drFactorNum[0]["DecimalDigit"] != DBNull.Value && int.TryParse(drFactorNum[0]["DecimalDigit"].ToString(), out num))
                                            {
                                                num = int.Parse(drFactorNum[0]["DecimalDigit"].ToString());
                                            }
                                            else
                                            {
                                                num = m_WaterPollutantService.GetPollutantInfo(factorCodes[k * NumOfCol + j - 1]).PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo(factorCodes[k * NumOfCol + j - 1]).PollutantDecimalNum) : 2;
                                            }
                                        }
                                        else
                                        {
                                            num = m_WaterPollutantService.GetPollutantInfo(factorCodes[k * NumOfCol + j - 1]).PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo(factorCodes[k * NumOfCol + j - 1]).PollutantDecimalNum) : 2;
                                        }

                                        DataRow[] dr = dtMonthAvgContent.Select("PointId='" + pointId + "' and PollutantCode='" + factorCodes[k * NumOfCol + j - 1] + "'");
                                        if (dr.Length != 0 && dr[0]["Value_Min"] != DBNull.Value)
                                        {
                                            decimal value = (factorCodes[k * NumOfCol + j - 1] == "w02023") ? Convert.ToDecimal(dr[0]["Value_Min"]) / 1000 : Convert.ToDecimal(dr[0]["Value_Min"]);
                                            if ((dr[0]["Upper"] != DBNull.Value && value > Convert.ToDecimal(dr[0]["Upper"])) || (dr[0]["Low"] != DBNull.Value && value < Convert.ToDecimal(dr[0]["Low"])))
                                                builder.Font.Color = System.Drawing.Color.Red;
                                            else
                                                builder.Font.Color = System.Drawing.Color.Black;
                                            builder.Write(DecimalExtension.GetPollutantValue(value, num).ToString());
                                        }
                                        else
                                        {
                                            builder.Write("/");
                                        }
                                    }
                                    break;
                                }
                            case 4:
                                {
                                    builder.Font.Bold = false;
                                    if (j == 0)
                                    {
                                        builder.CellFormat.Width = 80;
                                        builder.Write("平均值");
                                    }
                                    else if (j > 0 && k * NumOfCol + j <= factorCodes.Length)
                                    {
                                        int num = 2;
                                        DataRow[] drFactorNum = dtFactorNum.Select("PointId='" + pointId + "' and PollutantCode='" + factorCodes[k * NumOfCol + j - 1] + "'");
                                        if (drFactorNum.Length > 0)
                                        {
                                            if (drFactorNum[0]["DecimalDigit"] != DBNull.Value && int.TryParse(drFactorNum[0]["DecimalDigit"].ToString(), out num))
                                            {
                                                num = int.Parse(drFactorNum[0]["DecimalDigit"].ToString());
                                            }
                                            else
                                            {
                                                num = m_WaterPollutantService.GetPollutantInfo(factorCodes[k * NumOfCol + j - 1]).PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo(factorCodes[k * NumOfCol + j - 1]).PollutantDecimalNum) : 2;
                                            }
                                        }
                                        else
                                        {
                                            num = m_WaterPollutantService.GetPollutantInfo(factorCodes[k * NumOfCol + j - 1]).PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo(factorCodes[k * NumOfCol + j - 1]).PollutantDecimalNum) : 2;
                                        }

                                        DataRow[] dr = dtMonthAvgContent.Select("PointId='" + pointId + "' and PollutantCode='" + factorCodes[k * NumOfCol + j - 1] + "'");
                                        if (dr.Length != 0 && dr[0]["Value_Avg"] != DBNull.Value)
                                        {
                                            decimal value = (factorCodes[k * NumOfCol + j - 1] == "w02023") ? Convert.ToDecimal(dr[0]["Value_Avg"]) / 1000 : Convert.ToDecimal(dr[0]["Value_Avg"]);
                                            if ((dr[0]["Upper"] != DBNull.Value && value > Convert.ToDecimal(dr[0]["Upper"])) || (dr[0]["Low"] != DBNull.Value && value < Convert.ToDecimal(dr[0]["Low"])))
                                                builder.Font.Color = System.Drawing.Color.Red;
                                            else
                                                builder.Font.Color = System.Drawing.Color.Black;
                                            builder.Write(DecimalExtension.GetPollutantValue(value, num).ToString());
                                        }
                                        else
                                        {
                                            builder.Write("/");
                                        }
                                    }
                                    break;
                                }
                            case 5:
                                {
                                    builder.Font.Bold = false;
                                    if (j == 0)
                                    {
                                        builder.CellFormat.Width = 80;
                                        builder.Write("上月均值");
                                    }
                                    else if (j > 0 && k * NumOfCol + j <= factorCodes.Length)
                                    {
                                        int num = 2;
                                        DataRow[] drFactorNum = dtFactorNum.Select("PointId='" + pointId + "' and PollutantCode='" + factorCodes[k * NumOfCol + j - 1] + "'");
                                        if (drFactorNum.Length > 0)
                                        {
                                            if (drFactorNum[0]["DecimalDigit"] != DBNull.Value && int.TryParse(drFactorNum[0]["DecimalDigit"].ToString(), out num))
                                            {
                                                num = int.Parse(drFactorNum[0]["DecimalDigit"].ToString());
                                            }
                                            else
                                            {
                                                num = m_WaterPollutantService.GetPollutantInfo(factorCodes[k * NumOfCol + j - 1]).PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo(factorCodes[k * NumOfCol + j - 1]).PollutantDecimalNum) : 2;
                                            }
                                        }
                                        else
                                        {
                                            num = m_WaterPollutantService.GetPollutantInfo(factorCodes[k * NumOfCol + j - 1]).PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo(factorCodes[k * NumOfCol + j - 1]).PollutantDecimalNum) : 2;
                                        }

                                        DataRow[] dr = dtMonthAvgContent.Select("PointId='" + pointId + "' and PollutantCode='" + factorCodes[k * NumOfCol + j - 1] + "'");
                                        if (dr.Length != 0 && dr[0]["LastMonthAvg"] != DBNull.Value)
                                        {
                                            decimal value = (factorCodes[k * NumOfCol + j - 1] == "w02023") ? Convert.ToDecimal(dr[0]["LastMonthAvg"]) / 1000 : Convert.ToDecimal(dr[0]["LastMonthAvg"]);
                                            if ((dr[0]["Upper"] != DBNull.Value && value > Convert.ToDecimal(dr[0]["Upper"])) || (dr[0]["Low"] != DBNull.Value && value < Convert.ToDecimal(dr[0]["Low"])))
                                                builder.Font.Color = System.Drawing.Color.Red;
                                            else
                                                builder.Font.Color = System.Drawing.Color.Black;
                                            builder.Write(DecimalExtension.GetPollutantValue(value, num).ToString());
                                        }
                                        else
                                        {
                                            builder.Write("/");
                                        }
                                    }
                                    break;
                                }
                            case 6:
                                {
                                    builder.Font.Bold = false;
                                    if (j == 0)
                                    {
                                        builder.CellFormat.Width = 80;
                                        builder.Write("去年同期均值");
                                    }
                                    else if (j > 0 && k * NumOfCol + j <= factorCodes.Length)
                                    {
                                        int num = 2;
                                        DataRow[] drFactorNum = dtFactorNum.Select("PointId='" + pointId + "' and PollutantCode='" + factorCodes[k * NumOfCol + j - 1] + "'");
                                        if (drFactorNum.Length > 0)
                                        {
                                            if (drFactorNum[0]["DecimalDigit"] != DBNull.Value && int.TryParse(drFactorNum[0]["DecimalDigit"].ToString(), out num))
                                            {
                                                num = int.Parse(drFactorNum[0]["DecimalDigit"].ToString());
                                            }
                                            else
                                            {
                                                num = m_WaterPollutantService.GetPollutantInfo(factorCodes[k * NumOfCol + j - 1]).PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo(factorCodes[k * NumOfCol + j - 1]).PollutantDecimalNum) : 2;
                                            }
                                        }
                                        else
                                        {
                                            num = m_WaterPollutantService.GetPollutantInfo(factorCodes[k * NumOfCol + j - 1]).PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo(factorCodes[k * NumOfCol + j - 1]).PollutantDecimalNum) : 2;
                                        }

                                        DataRow[] dr = dtMonthAvgContent.Select("PointId='" + pointId + "' and PollutantCode='" + factorCodes[k * NumOfCol + j - 1] + "'");
                                        if (dr.Length != 0 && dr[0]["LastYearMonthAvg"] != DBNull.Value)
                                        {
                                            decimal value = (factorCodes[k * NumOfCol + j - 1] == "w02023") ? Convert.ToDecimal(dr[0]["LastYearMonthAvg"]) / 1000 : Convert.ToDecimal(dr[0]["LastYearMonthAvg"]);
                                            if ((dr[0]["Upper"] != DBNull.Value && value > Convert.ToDecimal(dr[0]["Upper"])) || (dr[0]["Low"] != DBNull.Value && value < Convert.ToDecimal(dr[0]["Low"])))
                                                builder.Font.Color = System.Drawing.Color.Red;
                                            else
                                                builder.Font.Color = System.Drawing.Color.Black;
                                            builder.Write(DecimalExtension.GetPollutantValue(value, num).ToString());
                                        }
                                        else
                                        {
                                            builder.Write("/");
                                        }
                                    }
                                    break;
                                }
                            case 7:
                                {
                                    if (j == 0)
                                    {
                                        builder.CellFormat.Width = 80;
                                        builder.Font.Bold = true;
                                        builder.Write("标准限值");
                                    }
                                    else if (j > 0 && k * NumOfCol + j <= factorCodes.Length)
                                    {
                                        builder.Font.Bold = false;
                                        DataRow[] dr = dtMonthAvgContent.Select("PointId='" + pointId + "' and PollutantCode='" + factorCodes[k * NumOfCol + j - 1] + "'");
                                        builder.Write(dr.Length == 0 ? "/" : (dr[0]["ClassBase"] == DBNull.Value ? "/" : dr[0]["ClassBase"].ToString()));
                                    }
                                    break;
                                }
                        }
                    }
                    builder.EndRow();
                }
            }

            builder.EndTable();
        }

        /// <summary>
        /// 设置表格显示样式
        /// </summary>
        /// <param name="factorCodes"></param>
        /// <param name="numOfRow"></param>
        /// <returns></returns>
        public int[] SettingTable(string[] factorCodes, int numOfRow)
        {
            //设置表格显示样式
            int NumOfCol = 0; //每行因子个数
            int NumOfRow = 0; //行数
            int NumOfLastRow = 0; //最后一行因子个数
            if (factorCodes.Length % numOfRow == 0)
            {
                NumOfCol = numOfRow;
                NumOfRow = factorCodes.Length / numOfRow;
            }
            else
            {
                NumOfRow = factorCodes.Length / numOfRow + 1;
                if (factorCodes.Length % NumOfRow == 0)
                    NumOfCol = factorCodes.Length / NumOfRow;
                else
                {
                    NumOfCol = factorCodes.Length / NumOfRow + 1;
                    NumOfLastRow = factorCodes.Length - NumOfCol * (NumOfRow - 1);
                }
            }
            int[] arrReturn = new int[] { NumOfRow, NumOfCol };
            return arrReturn;
        }

        public void FillMonthDataLTG(DocumentBuilder builder, DataTable dtMonthAvgContent, DataTable dtFactorNum, string[] factorCodes, string pointId, int j, string firstName)
        {
            string ColName = "";
            switch (firstName)
            {
                case "最大值": ColName = "Value_Max"; break;
                case "最小值": ColName = "Value_Min"; break;
                case "平均值": ColName = "Value_Avg"; break;
                case "上月均值": ColName = "LastMonthAvg"; break;
                case "去年同期均值": ColName = "LastYearMonthAvg"; break;
            }

            if (j == 0)
            {
                builder.CellFormat.Width = 80;
                builder.Write(firstName);
            }
            else if (j == 1)
            {
                int num = 3;
                DataRow[] drFactorNum = dtFactorNum.Select("PointId=53 and PollutantCode='w01028'");
                if (drFactorNum.Length > 0)
                {
                    if (drFactorNum[0]["DecimalDigit"] != DBNull.Value && int.TryParse(drFactorNum[0]["DecimalDigit"].ToString(), out num))
                    {
                        num = int.Parse(drFactorNum[0]["DecimalDigit"].ToString());
                    }
                    else
                    {
                        num = m_WaterPollutantService.GetPollutantInfo("w01028").PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo("w01028").PollutantDecimalNum) : 3;
                    }
                }
                else
                {
                    num = m_WaterPollutantService.GetPollutantInfo("w01028").PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo("w01028").PollutantDecimalNum) : 3;
                }

                //int num = m_WaterPollutantService.GetPollutantInfo("w01028").PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo("w01028").PollutantDecimalNum) : 3;
                DataRow[] dr = dtMonthAvgContent.Select("PointId=53 and PollutantCode='w01028' and F=1");
                builder.Write(dr.Length == 0 ? "/" : (dr[0][ColName] == DBNull.Value ? "/" : DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[0][ColName]), num).ToString()));
            }
            else if (j == 2)
            {
                int num = 3;
                DataRow[] drFactorNum = dtFactorNum.Select("PointId=53 and PollutantCode='w01028'");
                if (drFactorNum.Length > 0)
                {
                    if (drFactorNum[0]["DecimalDigit"] != DBNull.Value && int.TryParse(drFactorNum[0]["DecimalDigit"].ToString(), out num))
                    {
                        num = int.Parse(drFactorNum[0]["DecimalDigit"].ToString());
                    }
                    else
                    {
                        num = m_WaterPollutantService.GetPollutantInfo("w01028").PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo("w01028").PollutantDecimalNum) : 3;
                    }
                }
                else
                {
                    num = m_WaterPollutantService.GetPollutantInfo("w01028").PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo("w01028").PollutantDecimalNum) : 3;
                }
                //int num = m_WaterPollutantService.GetPollutantInfo("w01028").PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo("w01028").PollutantDecimalNum) : 3;
                DataRow[] dr = dtMonthAvgContent.Select("PointId=53 and PollutantCode='w01028' and F=-1");
                builder.Write(dr.Length == 0 ? "/" : (dr[0][ColName] == DBNull.Value ? "/" : System.Math.Abs(DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[0][ColName]), num)).ToString()));
            }
            else if (j == 3)
            {
                int num = 3;
                DataRow[] drFactorNum = dtFactorNum.Select("PointId=53 and PollutantCode='w01029'");
                if (drFactorNum.Length > 0)
                {
                    if (drFactorNum[0]["DecimalDigit"] != DBNull.Value && int.TryParse(drFactorNum[0]["DecimalDigit"].ToString(), out num))
                    {
                        num = int.Parse(drFactorNum[0]["DecimalDigit"].ToString());
                    }
                    else
                    {
                        num = m_WaterPollutantService.GetPollutantInfo("w01029").PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo("w01029").PollutantDecimalNum) : 3;
                    }
                }
                else
                {
                    num = m_WaterPollutantService.GetPollutantInfo("w01029").PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo("w01029").PollutantDecimalNum) : 3;
                }
                //int num = m_WaterPollutantService.GetPollutantInfo("w01029").PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo("w01029").PollutantDecimalNum) : 2;
                DataRow[] dr = dtMonthAvgContent.Select("PointId=53 and PollutantCode='w01029' and F=1");
                builder.Write(dr.Length == 0 ? "/" : (dr[0][ColName] == DBNull.Value ? "/" : DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[0][ColName]), num).ToString()));
            }
            else if (j == 4)
            {
                int num = 3;
                DataRow[] drFactorNum = dtFactorNum.Select("PointId=53 and PollutantCode='w01029'");
                if (drFactorNum.Length > 0)
                {
                    if (drFactorNum[0]["DecimalDigit"] != DBNull.Value && int.TryParse(drFactorNum[0]["DecimalDigit"].ToString(), out num))
                    {
                        num = int.Parse(drFactorNum[0]["DecimalDigit"].ToString());
                    }
                    else
                    {
                        num = m_WaterPollutantService.GetPollutantInfo("w01029").PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo("w01029").PollutantDecimalNum) : 3;
                    }
                }
                else
                {
                    num = m_WaterPollutantService.GetPollutantInfo("w01029").PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo("w01029").PollutantDecimalNum) : 3;
                }
                //int num = m_WaterPollutantService.GetPollutantInfo("w01029").PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo("w01029").PollutantDecimalNum) : 2;
                DataRow[] dr = dtMonthAvgContent.Select("PointId=53 and PollutantCode='w01029' and F=-1");
                builder.Write(dr.Length == 0 ? "/" : (dr[0][ColName] == DBNull.Value ? "/" : System.Math.Abs(DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[0][ColName]), num)).ToString()));
            }
            else if (j == 5)
            {

                DataRow[] dr = dtMonthAvgContent.Select("PointId=53 and PollutantCode='" + TW + "' and F=1");
                builder.Write(dr.Length == 0 ? "/" : (dr[0][ColName] == DBNull.Value ? "/" : DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[0][ColName]) / 100000, 2).ToString()));
            }
            else if (j == 6)
            {

                DataRow[] dr = dtMonthAvgContent.Select("PointId=53 and PollutantCode='" + TW + "' and F=-1");
                builder.Write(dr.Length == 0 ? "/" : (dr[0][ColName] == DBNull.Value ? "/" : System.Math.Abs(DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[0][ColName]) / 100000, 2)).ToString()));
            }
            else if (factorCodes[j - 5] != "w01028" || factorCodes[j - 5] != "w01029")
            {
                int num = 2;
                DataRow[] drFactorNum = dtFactorNum.Select("PointId='" + pointId + "' and PollutantCode='" + factorCodes[j - 5] + "'");
                if (drFactorNum.Length > 0)
                {
                    if (drFactorNum[0]["DecimalDigit"] != DBNull.Value && int.TryParse(drFactorNum[0]["DecimalDigit"].ToString(), out num))
                    {
                        num = int.Parse(drFactorNum[0]["DecimalDigit"].ToString());
                    }
                    else
                    {
                        num = m_WaterPollutantService.GetPollutantInfo(factorCodes[j - 5]).PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo(factorCodes[j - 5]).PollutantDecimalNum) : 2;
                    }
                }
                else
                {
                    num = m_WaterPollutantService.GetPollutantInfo(factorCodes[j - 5]).PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo(factorCodes[j - 5]).PollutantDecimalNum) : 2;
                }
                //int num = m_WaterPollutantService.GetPollutantInfo(factorCodes[j - 5]).PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo(factorCodes[j - 5]).PollutantDecimalNum) : 2;
                DataRow[] dr = dtMonthAvgContent.Select("PointId='" + pointId + "' and PollutantCode='" + factorCodes[j - 5] + "'");
                builder.Write(dr.Length == 0 ? "/" : (dr[0][ColName] == DBNull.Value ? "/" : DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[0][ColName]), num).ToString()));
            }
        }

        #endregion

        /// <summary>
        /// 各子站月报表数据
        /// </summary>
        /// <param name="dt"></param>
        public void MoveToMF3(DocumentBuilder builder, DataTable dtPoints, DataTable dtDayData, DataTable dtMonthAvg, DataTable dtBaseData, DataTable dtFactorNum, ReportLogEntity customDatumData, string PageTypeId)
        {
            builder.ParagraphFormat.ClearFormatting();
            builder.Font.ClearFormatting();
            builder.ParagraphFormat.Alignment = Aspose.Words.ParagraphAlignment.Center;
            builder.CellFormat.Borders.LineStyle = LineStyle.Single;
            builder.CellFormat.Borders.Color = System.Drawing.Color.Black;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.CellFormat.LeftPadding = 0;
            builder.CellFormat.RightPadding = 0;
            builder.Font.Size = 10;

            for (int point = 0; point < dtPoints.Rows.Count; point++)
            {
                string pointId = dtPoints.Rows[point]["PointId"].ToString();
                string[] factorCodes = dtPoints.Rows[point]["PollutantCodes"].ToString().Split(';');
                string pointName = monitoringPointWaterService.RetrieveEntityByPointId(Convert.ToInt16(pointId)).MonitoringPointName;
                string writeIn = "";
                if (pointId == "41")
                {
                    DataTable dtPfactorsNum = new DataTable();
                    DataTable dtPFactors = GetFactorsRelation(out dtPfactorsNum, customDatumData, PageTypeId, "C");
                    //常规因子
                    DataRow[] RegularFactors = dtPFactors.Select("PointId=41 and PollutantType not in('" + VOCGuid + "','" + AlgaGuid + "')");
                    string strRegularFactors = "";
                    for (int factor = 0; factor < RegularFactors.Length; factor++)
                    {
                        strRegularFactors += RegularFactors[factor]["PollutantCodes"] + ";";
                    }
                    string[] arrRegularFactors = strRegularFactors.TrimEnd(';').Split(';');
                    //非常规因子
                    DataRow[] IRRegularFactors = dtPFactors.Select("PointId=41 and PollutantType in('" + VOCGuid + "','" + AlgaGuid + "')");
                    string strIRRegularFactors = "";
                    for (int factor = 0; factor < IRRegularFactors.Length; factor++)
                    {
                        strIRRegularFactors += IRRegularFactors[factor]["PollutantCodes"] + ";";
                    }
                    string[] arrIRRegularFactors = strIRRegularFactors.TrimEnd(';').Split(';');
                    string strVOC = "";//VOC
                    DataRow[] VOC = dtPFactors.Select("PointId=41 and PollutantType='" + VOCGuid + "'");
                    if (VOC.Length > 0 && VOC[0]["PollutantCodes"] != DBNull.Value)
                        strVOC = VOC[0]["PollutantCodes"].ToString();
                    //数据处理
                    if (arrRegularFactors.Length > 0 && arrIRRegularFactors.Length == 0)
                    {
                        writeIn = "站点名称：" + pointName + "子站  常规指标";
                        builder.Write(writeIn);
                        FillDayData(customDatumData, builder, dtDayData, dtMonthAvg, dtBaseData, dtFactorNum, arrRegularFactors, pointId);
                    }
                    else if (arrRegularFactors.Length == 0 && arrIRRegularFactors.Length > 0)
                    {
                        writeIn = "站点名称：" + pointName + "子站  特征指标";
                        builder.Write(writeIn);
                        FillDayData(customDatumData, builder, dtDayData, dtMonthAvg, dtBaseData, dtFactorNum, arrIRRegularFactors, pointId, strVOC, "irregular");
                    }
                    else if (arrRegularFactors.Length > 0 && arrIRRegularFactors.Length > 0)
                    {
                        writeIn = "站点名称：" + pointName + "子站  常规指标";
                        builder.Write(writeIn);
                        FillDayData(customDatumData, builder, dtDayData, dtMonthAvg, dtBaseData, dtFactorNum, arrRegularFactors, pointId);
                        builder.InsertBreak(BreakType.PageBreak);
                        writeIn = "站点名称：" + pointName + "子站  特征指标";
                        builder.Write(writeIn);
                        FillDayData(customDatumData, builder, dtDayData, dtMonthAvg, dtBaseData, dtFactorNum, arrIRRegularFactors, pointId, strVOC, "irregular");
                    }
                }
                else if (pointId == "53")
                {
                    builder.InsertBreak(BreakType.PageBreak);
                    writeIn = "站点名称：" + pointName;
                    builder.Write(writeIn);
                    for (int i = 0; i < 3 + customDatumData.StartDateTime.Value.DaysInMonth(); i++)
                    {
                        for (int j = 0; j < factorCodes.Length + 6; j++)
                        {
                            builder.InsertCell();
                            builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                            builder.CellFormat.HorizontalMerge = CellMerge.None;
                            builder.CellFormat.VerticalMerge = CellMerge.None;
                            if (i == 0)
                            {
                                builder.Font.Bold = true;
                                if (j == 0)
                                {
                                    builder.CellFormat.VerticalMerge = CellMerge.First;
                                    builder.Write("序号");
                                }
                                else if (j == 1)
                                {
                                    builder.CellFormat.VerticalMerge = CellMerge.First;
                                    builder.Write("时间");
                                }
                                else if (j == 2)
                                {
                                    builder.CellFormat.HorizontalMerge = CellMerge.First;
                                    builder.Write("流速");
                                }
                                else if (j == 3)
                                {
                                    builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                                }
                                else if (j == 4)
                                {
                                    builder.CellFormat.HorizontalMerge = CellMerge.First;
                                    builder.Write("流量");
                                }
                                else if (j == 5)
                                {
                                    builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                                }
                                else if (j == 6)
                                {
                                    builder.CellFormat.HorizontalMerge = CellMerge.First;
                                    builder.Write("总水量");
                                }
                                else if (j == 7)
                                {
                                    builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                                }
                                else if (factorCodes[j - 6] != "w01028" || factorCodes[j - 5] != "w01029")
                                {
                                    builder.CellFormat.VerticalMerge = CellMerge.First;
                                    builder.Write(m_WaterPollutantService.GetPollutantInfo(factorCodes[j - 6]).PollutantName);
                                }
                            }
                            else if (i == 1)
                            {
                                builder.Font.Bold = true;
                                if (j == 0)
                                {
                                    builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                }
                                else if (j == 1)
                                {
                                    builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                }
                                else if (j == 2)
                                {
                                    builder.Write("出湖");
                                }
                                else if (j == 3)
                                {
                                    builder.Write("入湖");
                                }
                                else if (j == 4)
                                {
                                    builder.Write("出湖");
                                }
                                else if (j == 5)
                                {
                                    builder.Write("入湖");
                                }
                                else if (j == 6)
                                {
                                    builder.Write("出湖");
                                }
                                else if (j == 7)
                                {
                                    builder.Write("入湖");
                                }
                                else if (factorCodes[j - 6] != "w01028" || factorCodes[j - 6] != "w01029")
                                {
                                    builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                }
                            }
                            else if (i == 2)
                            {
                                builder.Font.Bold = true;
                                if (j == 0)
                                {
                                    builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                }
                                else if (j == 1)
                                {
                                    builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                }
                                else if (j == 2)
                                {
                                    builder.Write("m/s");
                                }
                                else if (j == 3)
                                {
                                    builder.Write("m/s");
                                }
                                else if (j == 4)
                                {
                                    builder.InsertHtml("<b>m<SUP>3</SUP>/s</b>");
                                }
                                else if (j == 5)
                                {
                                    builder.InsertHtml("<b>m<SUP>3</SUP>/s</b>");
                                }
                                else if (j == 6)
                                {
                                    builder.InsertHtml("<b>×10<SUP>5</SUP>m<SUP>3</SUP></b>");
                                }
                                else if (j == 7)
                                {
                                    builder.InsertHtml("<b>×10<SUP>5</SUP>m<SUP>3</SUP></b>");
                                }
                                else if (factorCodes[j - 6] != "w01028" || factorCodes[j - 6] != "w01029")
                                {
                                    string factorUnit = "";
                                    factorUnit = m_WaterPollutantService.GetPollutantInfo(factorCodes[j - 6]).PollutantMeasureUnit != null ? m_WaterPollutantService.GetPollutantInfo(factorCodes[j - 6]).PollutantMeasureUnit : "";
                                    builder.Write(factorUnit);
                                }
                            }
                            else
                            {
                                builder.Font.Bold = false;
                                FillDayDataLTG(builder, dtDayData, dtFactorNum, factorCodes, pointId, i, j);
                            }
                        }
                        builder.EndRow();
                    }
                    builder.EndTable();
                }
                else
                {
                    builder.InsertBreak(BreakType.PageBreak);
                    writeIn = "站点名称：" + pointName;
                    builder.Write(writeIn);
                    FillDayData(customDatumData, builder, dtDayData, dtMonthAvg, dtBaseData, dtFactorNum, factorCodes, pointId);
                }

            }
        }

        /// <summary>
        /// 填充表单
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="dtDayData"></param>
        /// <param name="dtBaseData"></param>
        /// <param name="factorCodes"></param>
        /// <param name="pointId"></param>
        public void FillDayData(ReportLogEntity customDatumData, DocumentBuilder builder, DataTable dtDayData, DataTable dtMonthAvg, DataTable dtBaseData, DataTable dtFactorNum, string[] factorCodes, string pointId, string VOC = "", string type = "")
        {
            int K = 1;
            int J;
            if (type == "irregular")
            {
                K = 2;
                J = factorCodes.Length % K == 0 ? factorCodes.Length / K : factorCodes.Length / K + 1;
            }
            else
            {
                J = factorCodes.Length;
            }
            for (int k = 0; k < K; k++)
            {
                for (int i = 0; i < 3 + customDatumData.StartDateTime.Value.DaysInMonth(); i++)
                {
                    for (int j = 0; j < 2 + J; j++)
                    {
                        builder.InsertCell();
                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                        builder.CellFormat.HorizontalMerge = CellMerge.None;
                        builder.CellFormat.VerticalMerge = CellMerge.None;
                        if (i == 0)
                        {
                            builder.Font.Bold = true;
                            if (j == 0)
                            {
                                builder.CellFormat.VerticalMerge = CellMerge.First;
                                builder.Write("序号");
                            }
                            else if (j == 1)
                            {
                                builder.CellFormat.VerticalMerge = CellMerge.First;
                                builder.Write("时间");
                            }
                            else if (j > 1 && k * J + j - 2 < factorCodes.Length)
                                builder.Write(m_WaterPollutantService.GetPollutantInfo(factorCodes[k * J + j - 2]).PollutantName);
                        }
                        else if (i == 1)
                        {
                            builder.Font.Bold = true;
                            if (j == 0)
                            {
                                builder.CellFormat.VerticalMerge = CellMerge.Previous;
                            }
                            else if (j == 1)
                            {
                                builder.CellFormat.VerticalMerge = CellMerge.Previous;
                            }
                            else if (j > 1 && k * J + j - 2 < factorCodes.Length)
                            {
                                string factorUnit = "";
                                factorUnit = m_WaterPollutantService.GetPollutantInfo(factorCodes[k * J + j - 2]).PollutantMeasureUnit != null ? m_WaterPollutantService.GetPollutantInfo(factorCodes[k * J + j - 2]).PollutantMeasureUnit : "";
                                builder.Write(factorUnit);
                            }
                        }
                        else if (i < 2 + customDatumData.StartDateTime.Value.DaysInMonth())
                        {
                            string dateTime = beginTime.AddDays(i - 2).ToString("yyyy-MM-dd");
                            builder.Font.Bold = false;
                            if (j == 0)
                            {
                                builder.Write((i - 1).ToString());
                            }
                            else if (j == 1)
                            {
                                builder.Write(dateTime);
                            }
                            else if (j > 1 && k * J + j - 2 < factorCodes.Length)
                            {
                                int num = 2;
                                DataRow[] drNum = dtFactorNum.Select("PointId=" + pointId + " and PollutantCode='" + factorCodes[k * J + j - 2] + "'");
                                if (drNum.Length > 0)
                                {
                                    if (drNum[0]["DecimalDigit"] != DBNull.Value && int.TryParse(drNum[0]["DecimalDigit"].ToString(), out num))
                                    {
                                        num = int.Parse(drNum[0]["DecimalDigit"].ToString());
                                    }
                                    else
                                    {
                                        num = m_WaterPollutantService.GetPollutantInfo(factorCodes[k * J + j - 2]).PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo(factorCodes[k * J + j - 2]).PollutantDecimalNum) : 2;
                                    }
                                }
                                else
                                {
                                    num = m_WaterPollutantService.GetPollutantInfo(factorCodes[k * J + j - 2]).PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo(factorCodes[k * J + j - 2]).PollutantDecimalNum) : 2;
                                }
                                DataRow[] dr = dtDayData.Select("PointId=" + pointId + " and PollutantCode='" + factorCodes[k * J + j - 2] + "' and DateTime='" + dateTime + "'");
                                if (dr.Length != 0 && dr[0]["PollutantValue"] != DBNull.Value)
                                {
                                    decimal value = (factorCodes[k * J + j - 2] == "w02023") ? Convert.ToDecimal(dr[0]["PollutantValue"]) / 1000 : Convert.ToDecimal(dr[0]["PollutantValue"]);
                                    builder.Write(DecimalExtension.GetPollutantValue(value, num).ToString());
                                }
                                else
                                {
                                    builder.Write("/");
                                }
                            }
                        }
                        else
                        {
                            builder.Font.Bold = false;
                            if (j == 0)
                            {
                                builder.CellFormat.HorizontalMerge = CellMerge.First;
                                if (type == "irregular")
                                    builder.Write("是否达标");
                                else
                                    builder.Write("水质类别");
                            }
                            else if (j == 1)
                            {
                                builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                            }
                            else if (j > 1 && k * J + j - 2 < factorCodes.Length)
                            {
                                string Class = "";
                                string F = "Grade";
                                if (VOC.IndexOf(factorCodes[k * J + j - 2]) > -1)
                                    F = "Limit";
                                DataRow[] dr = dtBaseData.Select("PointId=" + pointId + " and PollutantCode='" + factorCodes[k * J + j - 2] + "' and F='" + F + "'");
                                if (F == "Grade")
                                {
                                    int grade = dr.Length == 0 ? -1 : (dr[0]["Grade"] == DBNull.Value ? -1 : Convert.ToInt16(dr[0]["Grade"]));
                                    if (grade > 3)
                                        builder.Font.Color = System.Drawing.Color.Red;
                                    else
                                        builder.Font.Color = System.Drawing.Color.Black;
                                    switch (grade)
                                    {
                                        case 0:
                                        case -1: Class = "/"; break;
                                        case 1: Class = "Ⅰ类"; break;
                                        case 2: Class = "Ⅱ类"; break;
                                        case 3: Class = "Ⅲ类"; break;
                                        case 4: Class = "Ⅳ类"; break;
                                        case 5: Class = "Ⅴ类"; break;
                                        case 6: Class = "劣Ⅴ类"; break;
                                    }
                                }
                                else
                                {
                                    DataRow[] Avg = dtMonthAvg.Select("PointId=" + pointId + " and PollutantCode='" + factorCodes[k * J + j - 2] + "'");
                                    if (Avg.Length > 0 && Avg[0]["Value_Avg"] != DBNull.Value && dr.Length > 0)
                                    {
                                        if (dr[0]["UPPER"] != DBNull.Value && dr[0]["Low"] == DBNull.Value)
                                        {
                                            Class = Convert.ToDecimal(Avg[0]["Value_Avg"]) <= Convert.ToDecimal(dr[0]["UPPER"]) ? "是" : "否";
                                        }
                                        else if (dr[0]["UPPER"] == DBNull.Value && dr[0]["Low"] != DBNull.Value)
                                        {
                                            Class = Convert.ToDecimal(Avg[0]["Value_Avg"]) >= Convert.ToDecimal(dr[0]["Low"]) ? "是" : "否";
                                        }
                                        else if (dr[0]["UPPER"] != DBNull.Value && dr[0]["Low"] != DBNull.Value)
                                        {
                                            Class = Convert.ToDecimal(Avg[0]["Value_Avg"]) >= Convert.ToDecimal(dr[0]["Low"]) && Convert.ToDecimal(Avg[0]["Value_Avg"]) <= Convert.ToDecimal(dr[0]["UPPER"]) ? "是" : "否";
                                        }
                                        else
                                        {
                                            Class = "/";
                                        }
                                    }
                                    else
                                    {
                                        Class = "/";
                                    }
                                }
                                builder.Write(Class);
                            }
                        }
                    }
                    builder.EndRow();
                }
            }
            builder.EndTable();
            builder.Write("\r\n");
        }

        public void FillDayDataLTG(DocumentBuilder builder, DataTable dtDayData, DataTable dtFactorNum, string[] factorCodes, string pointId, int i, int j)
        {
            string strDateTime = beginTime.AddDays(i - 3).ToString("yyyy-MM-dd");
            if (j == 0)
            {
                builder.Write((i - 2).ToString());
            }
            else if (j == 1)
            {
                builder.Write(strDateTime);
            }
            else if (j == 2)
            {
                int num = 3;
                DataRow[] drNum = dtFactorNum.Select("PointId=53 and PollutantCode='w01028' ");
                if (drNum.Length > 0)
                {
                    if (drNum[0]["DecimalDigit"] != DBNull.Value && int.TryParse(drNum[0]["DecimalDigit"].ToString(), out num))
                    {
                        num = int.Parse(drNum[0]["DecimalDigit"].ToString());
                    }
                    else
                    {
                        num = m_WaterPollutantService.GetPollutantInfo("w01028").PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo("w01028").PollutantDecimalNum) : 3;
                    }
                }
                else
                {
                    num = m_WaterPollutantService.GetPollutantInfo("w01028").PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo("w01028").PollutantDecimalNum) : 3;
                }
                DataRow[] dr = dtDayData.Select("PointId=53 and PollutantCode='w01028' and DateTime='" + strDateTime + "' and F=1");
                if (dr.Length != 0 && dr[0]["PollutantValue"] != DBNull.Value)
                {
                    decimal value = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[0]["PollutantValue"]), num) >= 0.02m ? Convert.ToDecimal(dr[0]["PollutantValue"]) : 0.000m;
                    builder.Write(DecimalExtension.GetPollutantValue(value, num).ToString());
                }
                else
                {
                    builder.Write("/");
                }
            }
            else if (j == 3)
            {
                int num = 3;
                DataRow[] drNum = dtFactorNum.Select("PointId=53 and PollutantCode='w01028' ");
                if (drNum.Length > 0)
                {
                    if (drNum[0]["DecimalDigit"] != DBNull.Value && int.TryParse(drNum[0]["DecimalDigit"].ToString(), out num))
                    {
                        num = int.Parse(drNum[0]["DecimalDigit"].ToString());
                    }
                    else
                    {
                        num = m_WaterPollutantService.GetPollutantInfo("w01028").PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo("w01028").PollutantDecimalNum) : 3;
                    }
                }
                else
                {
                    num = m_WaterPollutantService.GetPollutantInfo("w01028").PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo("w01028").PollutantDecimalNum) : 3;
                }
                DataRow[] dr = dtDayData.Select("PointId=53 and PollutantCode='w01028' and DateTime='" + strDateTime + "'  and F=-1");
                if (dr.Length != 0 && dr[0]["PollutantValue"] != DBNull.Value)
                {
                    decimal value = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[0]["PollutantValue"]), num) <= -0.02m ? Convert.ToDecimal(dr[0]["PollutantValue"]) : 0.000m;
                    builder.Write(System.Math.Abs(DecimalExtension.GetPollutantValue(value, num)).ToString());
                }
                else
                {
                    builder.Write("/");
                }
            }
            else if (j == 4)
            {
                decimal flsm = 1;
                DataRow[] fls = dtDayData.Select("PointId=53 and PollutantCode='w01028' and DateTime='" + strDateTime + "' and F=1");
                if (fls.Length != 0 && fls[0]["PollutantValue"] != DBNull.Value)
                {
                    if (DecimalExtension.GetPollutantValue(Convert.ToDecimal(fls[0]["PollutantValue"]), 3) < 0.02m)
                        flsm = 0;
                }
                int num = 2;
                DataRow[] drNum = dtFactorNum.Select("PointId=53 and PollutantCode='w01029' ");
                if (drNum.Length > 0)
                {
                    if (drNum[0]["DecimalDigit"] != DBNull.Value && int.TryParse(drNum[0]["DecimalDigit"].ToString(), out num))
                    {
                        num = int.Parse(drNum[0]["DecimalDigit"].ToString());
                    }
                    else
                    {
                        num = m_WaterPollutantService.GetPollutantInfo("w01029").PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo("w01029").PollutantDecimalNum) : 2;
                    }
                }
                else
                {
                    num = m_WaterPollutantService.GetPollutantInfo("w01029").PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo("w01029").PollutantDecimalNum) : 2;
                }
                //int num = m_WaterPollutantService.GetPollutantInfo("w01029").PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo("w01029").PollutantDecimalNum) : 2;
                DataRow[] dr = dtDayData.Select("PointId=53 and PollutantCode='w01029' and DateTime='" + strDateTime + "' and F=1");
                if (dr.Length != 0 && dr[0]["PollutantValue"] != DBNull.Value)
                {
                    decimal value = flsm != 0 ? Convert.ToDecimal(dr[0]["PollutantValue"]) : 0.000m;
                    builder.Write(DecimalExtension.GetPollutantValue(value, num).ToString());
                }
                else
                {
                    builder.Write("/");
                }
            }
            else if (j == 5)
            {
                decimal flsm = -1;
                DataRow[] fls = dtDayData.Select("PointId=53 and PollutantCode='w01028' and DateTime='" + strDateTime + "' and F=-1");
                if (fls.Length != 0 && fls[0]["PollutantValue"] != DBNull.Value)
                {
                    if (DecimalExtension.GetPollutantValue(Convert.ToDecimal(fls[0]["PollutantValue"]), 3) > -0.02m)
                        flsm = 0;
                }
                int num = 2;
                DataRow[] drNum = dtFactorNum.Select("PointId=53 and PollutantCode='w01029' ");
                if (drNum.Length > 0)
                {
                    if (drNum[0]["DecimalDigit"] != DBNull.Value && int.TryParse(drNum[0]["DecimalDigit"].ToString(), out num))
                    {
                        num = int.Parse(drNum[0]["DecimalDigit"].ToString());
                    }
                    else
                    {
                        num = m_WaterPollutantService.GetPollutantInfo("w01029").PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo("w01029").PollutantDecimalNum) : 2;
                    }
                }
                else
                {
                    num = m_WaterPollutantService.GetPollutantInfo("w01029").PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo("w01029").PollutantDecimalNum) : 2;
                }
                //int num = m_WaterPollutantService.GetPollutantInfo("w01029").PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo("w01029").PollutantDecimalNum) : 2;
                DataRow[] dr = dtDayData.Select("PointId=53 and PollutantCode='w01029' and DateTime='" + strDateTime + "' and F=-1");
                if (dr.Length != 0 && dr[0]["PollutantValue"] != DBNull.Value)
                {
                    decimal value = flsm != 0 ? Convert.ToDecimal(dr[0]["PollutantValue"]) : 0.000m;
                    builder.Write(System.Math.Abs(DecimalExtension.GetPollutantValue(value, num)).ToString());
                }
                else
                {
                    builder.Write("/");
                }
            }
            else if (j == 6)
            {
                decimal flsm = 1;
                DataRow[] fls = dtDayData.Select("PointId=53 and PollutantCode='w01028' and DateTime='" + strDateTime + "' and F=1");
                if (fls.Length != 0 && fls[0]["PollutantValue"] != DBNull.Value)
                {
                    if (DecimalExtension.GetPollutantValue(Convert.ToDecimal(fls[0]["PollutantValue"]), 3) < 0.02m)
                        flsm = 0;
                }
                DataRow[] dr = dtDayData.Select("PointId=53 and PollutantCode='w01029' and DateTime='" + strDateTime + "' and F=1");
                if (dr.Length != 0 && dr[0]["TotalValue"] != DBNull.Value)
                {
                    decimal value = flsm != 0 ? Convert.ToDecimal(dr[0]["TotalValue"]) : 0.000m;
                    builder.Write(DecimalExtension.GetPollutantValue(value / 100000, 2).ToString());
                }
                else
                {
                    builder.Write("/");
                }
            }
            else if (j == 7)
            {
                decimal flsm = -1;
                DataRow[] fls = dtDayData.Select("PointId=53 and PollutantCode='w01028' and DateTime='" + strDateTime + "' and F=-1");
                if (fls.Length != 0 && fls[0]["PollutantValue"] != DBNull.Value)
                {
                    if (DecimalExtension.GetPollutantValue(Convert.ToDecimal(fls[0]["PollutantValue"]), 3) > -0.02m)
                        flsm = 0;
                }
                DataRow[] dr = dtDayData.Select("PointId=53 and PollutantCode='w01029' and DateTime='" + strDateTime + "' and F=-1");
                if (dr.Length != 0 && dr[0]["TotalValue"] != DBNull.Value)
                {
                    decimal value = flsm != 0 ? Convert.ToDecimal(dr[0]["TotalValue"]) : 0.000m;
                    builder.Write(System.Math.Abs(DecimalExtension.GetPollutantValue(value / 100000, 2)).ToString());
                }
                else
                {
                    builder.Write("/");
                }
            }
            else if (factorCodes[j - 6] != "w01028" || factorCodes[j - 6] != "w01029")
            {
                int num = 3;
                DataRow[] drNum = dtFactorNum.Select("PointId='" + pointId + "' and PollutantCode='" + factorCodes[j - 6] + "'  ");
                if (drNum.Length > 0)
                {
                    if (drNum[0]["DecimalDigit"] != DBNull.Value && int.TryParse(drNum[0]["DecimalDigit"].ToString(), out num))
                    {
                        num = int.Parse(drNum[0]["DecimalDigit"].ToString());
                    }
                    else
                    {
                        num = m_WaterPollutantService.GetPollutantInfo(factorCodes[j - 6]).PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo(factorCodes[j - 6]).PollutantDecimalNum) : 2;
                    }
                }
                else
                {
                    num = m_WaterPollutantService.GetPollutantInfo(factorCodes[j - 6]).PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo(factorCodes[j - 6]).PollutantDecimalNum) : 2;
                }
                //int num = m_WaterPollutantService.GetPollutantInfo(factorCodes[j - 6]).PollutantDecimalNum != "" ? Convert.ToInt16(m_WaterPollutantService.GetPollutantInfo(factorCodes[j - 6]).PollutantDecimalNum) : 2;
                DataRow[] dr = dtDayData.Select("PointId='" + pointId + "' and PollutantCode='" + factorCodes[j - 6] + "' and DateTime='" + strDateTime + "'");
                builder.Write(dr.Length == 0 ? "/" : (dr[0]["PollutantValue"] == DBNull.Value ? "/" : DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[0]["PollutantValue"]), num).ToString()));
            }
        }

        /// <summary>
        /// 获取水质自动监测子站应该测记录数
        /// </summary>
        /// <param name="dtPoints"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public DataTable GetShouldRecord(DataTable dtPoints, DateTime beginTime, DateTime endTime)
        {
            string ApplicationUid = "watrwatr-watr-watr-watr-watrwatrwatr";
            return reportService.GetShouldRecordByUncertainFactors(ApplicationUid, dtPoints, beginTime, endTime).ToTable();
        }

        /// <summary>
        /// 获取时间范围
        /// </summary>
        public void GetDateRange(ReportLogEntity customDatumData)
        {
            if (customDatumData.StartDateTime != null)
            {
                beginTime = customDatumData.StartDateTime.Value;
                endTime = customDatumData.EndDateTime.Value;
            }
        }

        
        
    }
}
