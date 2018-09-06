﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartEP.Core.Generic;
using SmartEP.Data.SqlServer.BaseData;
using System.Data;
using SmartEP.WebService.Internal.Common;
using SmartEP.Data.SqlServer.MonitoringBusiness.Air;
using System.Text;
using SmartEP.Service.BaseData.Channel;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.Frame;
using SmartEP.Service.Core.Enums;
using SmartEP.DomainModel;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Utilities.AdoData;
using SmartEP.Service.DataAnalyze.Air;
using SmartEP.Service.DataAnalyze.Air.AQIReport;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Service.AutoMonitoring.Air;
using System.Net;
using System.Configuration;
using System.Text.RegularExpressions;
using System.IO;


namespace SmartEP.WebService
{
    /// <summary>
    /// 名称:GetDataClass.cs
    /// 创建人:徐阳
    /// 创建日期:2017-07-03
    /// 维护人员:
    /// 最新维护人员:
    /// 最新维护日期:
    /// 功能摘要:获取数据类
    /// 版权所有(C):江苏远大信息股份有限公司
    /// </summary>
    public class GetDataClass
    {
        //WebServiceForOutData g_ServiceForOutData = new WebServiceForOutData();
        
        DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();
        //private String monitorRegionUid = System.Configuration.ConfigurationManager.AppSettings["CityAQI_MonitoringRegionUid"].ToString();
        private String monitorRegionUid = "b6e983c4-4f92-4be3-bbac-d9b71c470640";
        /// <summary>
        /// 获取测点数据
        /// </summary>
        /// <returns>
        /// </returns>
        public string GetMonitoringPointJson()
        {
            MonitoringPointDAL monitoringPointDAL = new MonitoringPointDAL();
            DataTable dt = new DataTable();
            dt = monitoringPointDAL.GetMonitoringPointDataTableForData();
            //for (int i = 0; i < dt.Columns.Count; i++)
            //{
            //    DataColumn dc = dt.Columns[i];
            //    if (dc.ColumnName != "PointId" && dc.ColumnName != "MonitoringPointUid" && dc.ColumnName != "MonitoringPointName" && dc.ColumnName != "X" && dc.ColumnName != "Y" && dc.ColumnName != "PName")
            //    {
            //        dt.Columns.Remove(dc);
            //        i--;
            //    }
            //}
            StringBuilder sb = new StringBuilder();
            sb.Append("[{\"PortInfo\":[{\"state\":0,\"label\":\"环境空气\",\"id\":\"000\",\"children\":[");
            DataTable dtCtr = monitoringPointDAL.GetPortTypes();
            string PortTypeName = string.Empty;     //控制等级
            string PortTypeValue = string.Empty;    //控制Guid
            string str = string.Empty;
            string json = string.Empty;
            for (int i = 0; i < dtCtr.Rows.Count; i++)
            {
                PortTypeName = dtCtr.Rows[i]["PortTypeName"].ToString();
                PortTypeValue = dtCtr.Rows[i]["PortTypeValue"].ToString();
                //str = string.Format("{\"state\":0,\"lable\":\"{0}\",\"id\":\"{1}\",\"children\":", PortTypeName, PortTypeValue);
                str = "{\"state\":0,\"label\":\"" + PortTypeName + "\",\"id\":\"" + PortTypeValue + "\",\"children\":[";
                sb.Append(str);
                DataView dv = dt.AsDataView();
                dv.RowFilter = "PName = '" + PortTypeName + "'";
                foreach (DataRowView drv in dv)
                {
                    string str2 = "{\"state\":0,\"label\":\"" + drv["MonitoringPointName"] + "\",\"id\":\"" + drv["PointId"] + "\"},";
                    sb.Append(str2);
                }
                string strForsb = sb.ToString().TrimEnd(',');
                sb.Clear().Append(strForsb);
                sb.Append("]},");
                dv.RowFilter = "";
            }
            string result = sb.ToString().TrimEnd(',') + "]}]}]";
            return result;
        }

        /// <summary>
        /// 获取测点信息
        /// </summary>
        /// <returns>
        /// </returns>
        public string GetPortMessage(string[] pointId, string type, string fac)
        {
            MonitoringPointDAL monitoringPointDAL = new MonitoringPointDAL();
            DataTable dt = new DataTable();
            dt = monitoringPointDAL.GetPortMessageForData(pointId, type, fac);
            StringBuilder sb = new StringBuilder();
            sb.Append("[{\"PortMessages\":");
            string json = dt.ToJsonBySerialize();
            sb.Append(json + "}]");
            return sb.ToString();
        }

        /// <summary>
        /// 获取测点信息
        /// </summary>
        /// <returns>
        /// </returns>
        public string GetPortMessage(string[] pointId, string type, string fac, DateTime dt1, DateTime dt2)
        {
            MonitoringPointDAL monitoringPointDAL = new MonitoringPointDAL();
            DataTable dt = new DataTable();
            dt = monitoringPointDAL.GetPortMessageForData(pointId, type, fac, dt1, dt2);
            StringBuilder sb = new StringBuilder();
            sb.Append("[{\"PortMessages\":");
            string json = dt.ToJsonBySerialize();
            sb.Append(json + "}]");
            return sb.ToString();
        }

        /// <summary>
        /// 获取周边城市测点信息
        /// </summary>
        /// <returns>
        /// </returns>
        //public string GetAroundPortMessage(string pointId, string type, DateTime dtime)
        //{
        //    MonitoringPointDAL monitoringPointDAL = new MonitoringPointDAL();
        //    DataTable dt = new DataTable();
        //    dt = monitoringPointDAL.GetAroundPortMessageForData(pointId, type, dtime);
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("[{\"PortMessages\":");
        //    string json = dt.ToJsonBySerialize();
        //    sb.Append(json + "}]");
        //    return sb.ToString();
        //}

        /// <summary>
        /// 获取周边城市测点信息排序
        /// </summary>
        /// <returns>
        /// </returns>
        public string GetAroundPortMessageOrder(string order, string type)
        {
            MonitoringPointDAL monitoringPointDAL = new MonitoringPointDAL();
            DataTable dt = new DataTable();
            dt = monitoringPointDAL.GetAroundPortMessageForDataOrder(order, type);
            StringBuilder sb = new StringBuilder();
            sb.Append("[{\"PortMessages\":");
            string json = string.Empty;
            if (dt == null)
            {
                json = "";
            }
            else
            {
                json = dt.ToJsonBySerialize();
            }
            sb.Append(json + "}]");
            return sb.ToString();
        }

        /// <summary>
        /// 获取周边城市测点信息排序
        /// </summary>
        /// <returns>
        /// </returns>
        public string GetAroundPortData(string order, string type)
        {
            MonitoringPointDAL monitoringPointDAL = new MonitoringPointDAL();
            DataTable dt = new DataTable();
            dt = monitoringPointDAL.GetAroundPortData(order, type);
            StringBuilder sb = new StringBuilder();
            sb.Append("[{\"PortMessages\":");
            string json = string.Empty;
            if (dt == null)
            {
                json = "";
            }
            else
            {
                json = dt.ToJsonBySerialize();
            }
            sb.Append(json + "}]");
            return sb.ToString();
        }

        /// <summary>
        /// 获取所有站点分类
        /// </summary>
        /// <returns>
        /// </returns>
        public string GetPortTypes()
        {
            MonitoringPointDAL monitoringPointDAL = new MonitoringPointDAL();
            DataTable dt = new DataTable();
            dt = monitoringPointDAL.GetPortTypes();
            StringBuilder sb = new StringBuilder();
            sb.Append("[{\"PortTypes\":");
            string json = "[{\"PortTypeName\":\"所有选项\",\"PortTypeValue\":\"ALL\"}," + dt.ToJsonBySerialize().TrimStart('[');
            sb.Append(json + "}]");
            return sb.ToString();
        }

        /// <summary>
        /// 获取因子信息
        /// </summary>
        /// <returns>
        /// </returns>
        public string GetPollutantCodeDataTable()
        {
            DictionaryService dicService = new DictionaryService();
            string guid = dicService.GetValueByText(DictionaryType.AMS, "通道类型", "状态");
            IList<V_CodeMainItemEntity> codeMainItemList = dicService.RetrieveList(DictionaryType.AMS, "计量单位").ToList();//根据字典类型、字典名获取字典项列表
            DataTable dt = new DataTable("PollutantData");
            dt.Columns.Add("PollutantCode");
            dt.Columns.Add("PollutantName");
            dt.Columns.Add("Unit");
            MonitoringPointAirService monitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();//空气测点类
            IList<MonitoringPointEntity> monitoringPointAirList = monitoringPointAir.RetrieveAirMPListByEnable().ToList();//获取所有启用的空气点位列表
            IList<PollutantCodeEntity> pollutantCodeAirList = GetPollutantDataByPointList(monitoringPointAirList);
            pollutantCodeAirList = pollutantCodeAirList.Where(t => t.TypeUid != guid).ToList();
            foreach (PollutantCodeEntity pollutantCodeEntity in pollutantCodeAirList)
            {
                DataRow dr = dt.NewRow();
                dr["PollutantCode"] = pollutantCodeEntity.PollutantCode;
                dr["PollutantName"] = pollutantCodeEntity.PollutantName;
                dr["Unit"] = codeMainItemList.Where(t => t.ItemGuid == pollutantCodeEntity.MeasureUnitUid)
                                             .Select(t => t.ItemText).FirstOrDefault();
                dt.Rows.Add(dr);
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("[{\"FactorList\":");
            string json = dt.ToJsonBySerialize();
            sb.Append(json + "}]");
            return sb.ToString();
        }

        /// <summary>
        /// 获取参与AQI计算因子数据
        /// </summary>
        /// <returns>
        /// </returns>
        public string GetPollutantByCalAQI()
        {
            AirPollutantService airPollutantService = new AirPollutantService();
            List<PollutantCodeEntity> list = airPollutantService.RetrieveListByCalAQI().ToList();
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            sb.Append("[{\"ApiFactor\":[");
            foreach (PollutantCodeEntity pce in list)
            {
                sb.Append("{\"Name\":\"" + pce.PollutantName + "\",\"EName\":\"" + pce.ChemicalSymbol + "\",\"Code\":\"" + pce.PollutantCode + "\"},");
            }
            string json = sb.ToString().TrimEnd(',');
            sb.Clear().Append(json + "]}]");
            return sb.ToString();
        }

        /// <summary>
        /// 获取优良数据统计(默认显示南通市区，可选测点)
        /// </summary>
        /// <returns>
        /// </returns>
        public string GetCountData(string[] ports, DateTime dtStart, DateTime dtEnd)
        {
            DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();
            AQICalculateService m_AQICalculateService = new AQICalculateService();
            DayAQIService m_DayAQIService = Singleton<DayAQIService>.GetInstance();
            if (ports[0] == "Default")
            {
                string PointTableName = "V_Point_Air_SiteMap_Region";
                string sqlPoint = string.Format(@"SELECT B.PID
                                        FROM {0} A
                                        inner join {0} B
                                        on A.PName='{1}' and A.CGuid = B.PGuid", PointTableName, "南通市区");
                DataTable dvPoint = g_DatabaseHelper.ExecuteDataTable(sqlPoint, "AMS_BaseDataConnection");
                string[] arr = dvPoint.AsEnumerable().Select(d => Convert.ToString(d.Field<int>("PID"))).ToArray();
                string[] regionGuids = { "cda2fe50-94b2-4176-bd41-32cb90584b70" };
                string[] fac = { };
                string[] years = { };
                Dictionary<string, string> regionName = new Dictionary<string, string>();
                regionName.Add("cda2fe50-94b2-4176-bd41-32cb90584b70", "南通市区");
                int recordTotal = 0;
                DataTable dt = m_DayAQIService.GetAreaOverDaysList(arr, regionGuids, fac, dtStart, dtEnd, "质量类别", "", 100, 99999, 0, years, regionName, out recordTotal);
                return dt.ToJsonBySerialize();
            }
            else
            {
                string[] fac = { };
                string[] years = { };
                int recordTotal = 0;
                DataTable dt = m_DayAQIService.GetOverDaysList(ports, fac, dtStart, dtEnd, "质量类别", "", 100, 99999, 0, years, out recordTotal);
                return dt.ToJsonBySerialize();
            }
        }

        /// <summary>
        /// 获取区域综合分析数据
        /// </summary>
        /// <returns>
        /// </returns>
        public string GetAreaDataAnalyze(DateTime dtStart, DateTime dtEnd)
        {
            DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();
            AQICalculateService m_AQICalculateService = new AQICalculateService();
            string PointTableName = "V_Point_Air_SiteMap_Region";
            string sqlRegion = string.Format(@"SELECT PName FROM {0} WHERE PGuid is null", PointTableName);
            DataView dvRegion = g_DatabaseHelper.ExecuteDataView(sqlRegion, "AMS_BaseDataConnection");
            Dictionary<string, string[]> dic = new Dictionary<string, string[]>();
            //            if (RegionName != "")
            //            {
            //                string sqlPoint = string.Format(@"SELECT B.PID
            //                                        FROM {0} A
            //                                        inner join {0} B
            //                                        on A.PName='{1}' and A.CGuid = B.PGuid", PointTableName, RegionName);
            //                DataTable dvPoint = g_DatabaseHelper.ExecuteDataTable(sqlPoint, "AMS_BaseDataConnection");
            //                string[] arr = dvPoint.AsEnumerable().Select(d => Convert.ToString(d.Field<int>("PID"))).ToArray();
            //                dic.Add(RegionName, arr);
            //            }
            //            else
            //            {
            foreach (DataRowView drv in dvRegion)
            {
                string sqlPoint = string.Format(@"SELECT B.PID
                                        FROM {0} A
                                        inner join {0} B
                                        on A.PName='{1}' and A.CGuid = B.PGuid", PointTableName, drv["PName"]);
                DataTable dvPoint = g_DatabaseHelper.ExecuteDataTable(sqlPoint, "AMS_BaseDataConnection");
                string[] arr = dvPoint.AsEnumerable().Select(d => Convert.ToString(d.Field<int>("PID"))).ToArray();
                dic.Add(drv["PName"].ToString(), arr);
            }
            //}
            DataTable dtForAQI = new DataTable();
            dtForAQI.Columns.Add("RegionName", typeof(string));
            dtForAQI.Columns.Add("DateTime", typeof(DateTime));
            dtForAQI.Columns.Add("PM25", typeof(Decimal));
            dtForAQI.Columns.Add("PM25_IAQI", typeof(Int32));
            dtForAQI.Columns.Add("PM10", typeof(Decimal));
            dtForAQI.Columns.Add("PM10_IAQI", typeof(Int32));
            dtForAQI.Columns.Add("NO2", typeof(Decimal));
            dtForAQI.Columns.Add("NO2_IAQI", typeof(Int32));
            dtForAQI.Columns.Add("SO2", typeof(Decimal));
            dtForAQI.Columns.Add("SO2_IAQI", typeof(Int32));
            dtForAQI.Columns.Add("CO", typeof(Decimal));
            dtForAQI.Columns.Add("CO_IAQI", typeof(Int32));
            dtForAQI.Columns.Add("Max8HourO3", typeof(Decimal));
            dtForAQI.Columns.Add("Max8HourO3_IAQI", typeof(Int32));
            dtForAQI.Columns.Add("AQIValue", typeof(Int32));
            int dayNum = Convert.ToInt32((dtEnd.Subtract(dtStart)).TotalDays);
            for (int i = 0; i < dayNum; i++)
            {
                foreach (string name in dic.Keys)
                {
                    DateTime dayNew = dtStart.AddDays(i);
                    decimal? PM25PollutantValue = m_AQICalculateService.GetRegionValue(dic[name], "a34004", dayNew, 24, "1");
                    decimal? PM10PollutantValue = m_AQICalculateService.GetRegionValue(dic[name], "a34002", dayNew, 24, "1");
                    decimal? NO2PollutantValue = m_AQICalculateService.GetRegionValue(dic[name], "a21004", dayNew, 24, "1");
                    decimal? SO2PollutantValue = m_AQICalculateService.GetRegionValue(dic[name], "a21026", dayNew, 24, "1");
                    decimal? COPollutantValue = m_AQICalculateService.GetRegionValue(dic[name], "a21005", dayNew, 24, "1");
                    decimal? Max8HourO3PollutantValue = m_AQICalculateService.GetRegionValue(dic[name], "a05024", dayNew, 8, "1");
                    int? PM25Value = m_AQICalculateService.GetIAQI("a34004", Convert.ToDouble(PM25PollutantValue), 24);
                    int? PM10Value = m_AQICalculateService.GetIAQI("a34002", Convert.ToDouble(PM10PollutantValue), 24);
                    int? NO2Value = m_AQICalculateService.GetIAQI("a21004", Convert.ToDouble(NO2PollutantValue), 24);
                    int? SO2Value = m_AQICalculateService.GetIAQI("a21026", Convert.ToDouble(SO2PollutantValue), 24);
                    int? COValue = m_AQICalculateService.GetIAQI("a21005", Convert.ToDouble(COPollutantValue), 24);
                    int? Max8HourO3Value = m_AQICalculateService.GetIAQI("a05024", Convert.ToDouble(Max8HourO3PollutantValue), 8);
                    string AQIValue = m_AQICalculateService.GetAQI_Max_CNV(SO2Value, NO2Value, PM10Value, COValue, Max8HourO3Value, PM25Value, "V");
                    DataRow dr = dtForAQI.NewRow();
                    dr["RegionName"] = name;
                    dr["DateTime"] = dayNew;
                    if (PM25PollutantValue != null)
                        dr["PM25"] = PM25PollutantValue;
                    if (PM25Value != null)
                        dr["PM25_IAQI"] = PM25Value;
                    if (PM10PollutantValue != null)
                        dr["PM10"] = PM10PollutantValue;
                    if (PM10Value != null)
                        dr["PM10_IAQI"] = PM10Value;
                    if (NO2PollutantValue != null)
                        dr["NO2"] = NO2PollutantValue;
                    if (NO2Value != null)
                        dr["NO2_IAQI"] = NO2Value;
                    if (SO2PollutantValue != null)
                        dr["SO2"] = SO2PollutantValue;
                    if (SO2Value != null)
                        dr["SO2_IAQI"] = SO2Value;
                    if (COPollutantValue != null)
                        dr["CO"] = COPollutantValue;
                    if (COValue != null)
                        dr["CO_IAQI"] = COValue;
                    if (Max8HourO3PollutantValue != null)
                        dr["Max8HourO3"] = Max8HourO3PollutantValue;
                    if (Max8HourO3Value != null)
                        dr["Max8HourO3_IAQI"] = Max8HourO3Value;
                    if (AQIValue != null && AQIValue.Trim() != "")
                        dr["AQIValue"] = Convert.ToInt32(AQIValue);
                    dtForAQI.Rows.Add(dr);
                }
            }

            DataView CountValues = new DataView();                            //存放统计值
            DataTable dtCountValues = new DataTable();
            dtCountValues.Columns.Add("RegionName", typeof(string));
            dtCountValues.Columns.Add("X", typeof(string));
            dtCountValues.Columns.Add("Y", typeof(string));
            dtCountValues.Columns.Add("ExcellentCount", typeof(int));
            dtCountValues.Columns.Add("GoodCount", typeof(int));
            dtCountValues.Columns.Add("LightCount", typeof(int));
            dtCountValues.Columns.Add("MiddleCount", typeof(int));
            dtCountValues.Columns.Add("SevereCount", typeof(int));
            dtCountValues.Columns.Add("SeriesCount", typeof(int));
            dtCountValues.Columns.Add("EGRate", typeof(string));
            dtCountValues.Columns.Add("ExcellentRate", typeof(string));
            dtCountValues.Columns.Add("GoodRate", typeof(string));
            dtCountValues.Columns.Add("LightRate", typeof(string));
            dtCountValues.Columns.Add("MiddleRate", typeof(string));
            dtCountValues.Columns.Add("SevereRate", typeof(string));
            dtCountValues.Columns.Add("SeriesRate", typeof(string));
            dtCountValues.Columns.Add("PM25_Max", typeof(string));
            dtCountValues.Columns.Add("PM10_Max", typeof(string));
            dtCountValues.Columns.Add("NO2_Max", typeof(string));
            dtCountValues.Columns.Add("SO2_Max", typeof(string));
            dtCountValues.Columns.Add("CO_Max", typeof(string));
            dtCountValues.Columns.Add("Max8HourO3_Max", typeof(string));
            dtCountValues.Columns.Add("PM25_Min", typeof(string));
            dtCountValues.Columns.Add("PM10_Min", typeof(string));
            dtCountValues.Columns.Add("NO2_Min", typeof(string));
            dtCountValues.Columns.Add("SO2_Min", typeof(string));
            dtCountValues.Columns.Add("CO_Min", typeof(string));
            dtCountValues.Columns.Add("Max8HourO3_Min", typeof(string));
            dtCountValues.Columns.Add("PM25_Avg", typeof(string));
            dtCountValues.Columns.Add("PM10_Avg", typeof(string));
            dtCountValues.Columns.Add("NO2_Avg", typeof(string));
            dtCountValues.Columns.Add("SO2_Avg", typeof(string));
            dtCountValues.Columns.Add("CO_Avg", typeof(string));
            dtCountValues.Columns.Add("Max8HourO3_Avg", typeof(string));

            foreach (string name in dic.Keys)
            {
                int dayCount = dtForAQI.Select("RegionName='" + name + "' and AQIValue is not null").Count();         //某区域总天数

                DataRow drCountValues = dtCountValues.NewRow();
                DataTable dtXY = GetXY(name);
                drCountValues["RegionName"] = name;
                drCountValues["X"] = dtXY.Rows[0]["X"].ToString();
                drCountValues["Y"] = dtXY.Rows[0]["Y"].ToString();
                int ExcellentDays = dtForAQI.Select("RegionName='" + name + "' and AQIValue > 0 and AQIValue <= 50").Count();
                drCountValues["ExcellentCount"] = ExcellentDays;
                int GoodDays = dtForAQI.Select("RegionName='" + name + "' and AQIValue > 50 and AQIValue <= 100").Count();
                drCountValues["GoodCount"] = GoodDays;
                int LightDays = dtForAQI.Select("RegionName='" + name + "' and AQIValue > 100 and AQIValue <= 150").Count();
                drCountValues["LightCount"] = LightDays;
                int MiddleDays = dtForAQI.Select("RegionName='" + name + "' and AQIValue > 150 and AQIValue <= 200").Count();
                drCountValues["MiddleCount"] = MiddleDays;
                int SevereDays = dtForAQI.Select("RegionName='" + name + "' and AQIValue > 200 and AQIValue <= 300").Count();
                drCountValues["SevereCount"] = SevereDays;
                int SeriesDays = dtForAQI.Select("RegionName='" + name + "' and AQIValue > 300").Count();
                drCountValues["SeriesCount"] = SeriesDays;
                drCountValues["EGRate"] = GetPollutantValue(decimal.Parse((GoodDays + ExcellentDays).ToString()) / dayCount * 100, 1).ToString() + "%";
                drCountValues["ExcellentRate"] = GetPollutantValue(decimal.Parse((ExcellentDays).ToString()) / dayCount * 100, 1).ToString() + "%";
                drCountValues["GoodRate"] = GetPollutantValue(decimal.Parse((GoodDays).ToString()) / dayCount * 100, 1).ToString() + "%";
                drCountValues["LightRate"] = GetPollutantValue(decimal.Parse((LightDays).ToString()) / dayCount * 100, 1).ToString() + "%";
                drCountValues["MiddleRate"] = GetPollutantValue(decimal.Parse((MiddleDays).ToString()) / dayCount * 100, 1).ToString() + "%";
                drCountValues["SevereRate"] = GetPollutantValue(decimal.Parse((SevereDays).ToString()) / dayCount * 100, 1).ToString() + "%";
                drCountValues["SeriesRate"] = GetPollutantValue(decimal.Parse((SeriesDays).ToString()) / dayCount * 100, 1).ToString() + "%";
                drCountValues["PM25_Max"] = GetPollutantValue(Convert.ToDecimal(dtForAQI.Compute("MAX(PM25)", "RegionName='" + name + "' and PM25 is not null").ToString()) * 1000, 0).ToString() + "μg / m3";
                drCountValues["PM10_Max"] = GetPollutantValue(Convert.ToDecimal(dtForAQI.Compute("MAX(PM10)", "RegionName='" + name + "' and PM10 is not null").ToString()) * 1000, 0) + "μg / m3";
                drCountValues["NO2_Max"] = GetPollutantValue(Convert.ToDecimal(dtForAQI.Compute("MAX(NO2)", "RegionName='" + name + "' and NO2 is not null").ToString()) * 1000, 0) + "μg / m3";
                drCountValues["SO2_Max"] = GetPollutantValue(Convert.ToDecimal(dtForAQI.Compute("MAX(SO2)", "RegionName='" + name + "' and SO2 is not null").ToString()) * 1000, 0) + "μg / m3";
                drCountValues["CO_Max"] = GetPollutantValue(Convert.ToDecimal(dtForAQI.Compute("MAX(CO)", "RegionName='" + name + "' and CO is not null").ToString()), 0) + "mg/m3";
                drCountValues["Max8HourO3_Max"] = GetPollutantValue(Convert.ToDecimal(dtForAQI.Compute("MAX(Max8HourO3)", "RegionName='" + name + "' and Max8HourO3 is not null").ToString()) * 1000, 0) + "μg / m3";
                drCountValues["PM25_Min"] = GetPollutantValue(Convert.ToDecimal(dtForAQI.Compute("Min(PM25)", "RegionName='" + name + "' and PM25 is not null").ToString()) * 1000, 0) + "μg / m3";
                drCountValues["PM10_Min"] = GetPollutantValue(Convert.ToDecimal(dtForAQI.Compute("Min(PM10)", "RegionName='" + name + "' and PM10 is not null").ToString()) * 1000, 0) + "μg / m3";
                drCountValues["NO2_Min"] = GetPollutantValue(Convert.ToDecimal(dtForAQI.Compute("Min(NO2)", "RegionName='" + name + "' and NO2 is not null").ToString()) * 1000, 0) + "μg / m3";
                drCountValues["SO2_Min"] = GetPollutantValue(Convert.ToDecimal(dtForAQI.Compute("Min(SO2)", "RegionName='" + name + "' and SO2 is not null").ToString()) * 1000, 0) + "μg / m3";
                drCountValues["CO_Min"] = GetPollutantValue(Convert.ToDecimal(dtForAQI.Compute("Min(CO)", "RegionName='" + name + "' and CO is not null").ToString()), 0) + "mg/m3";
                drCountValues["Max8HourO3_Min"] = GetPollutantValue(Convert.ToDecimal(dtForAQI.Compute("Min(Max8HourO3)", "RegionName='" + name + "' and Max8HourO3 is not null").ToString()) * 1000, 0) + "μg / m3";
                drCountValues["Max8HourO3_Max"] = GetPollutantValue(Convert.ToDecimal(dtForAQI.Compute("MAX(Max8HourO3)", "RegionName='" + name + "' and Max8HourO3 is not null").ToString()) * 1000, 0) + "μg / m3";
                drCountValues["PM25_Avg"] = GetPollutantValue(Convert.ToDecimal(dtForAQI.Compute("AVG(PM25)", "RegionName='" + name + "' and PM25 is not null").ToString()) * 1000, 0) + "μg / m3";
                drCountValues["PM10_Avg"] = GetPollutantValue(Convert.ToDecimal(dtForAQI.Compute("AVG(PM10)", "RegionName='" + name + "' and PM10 is not null").ToString()) * 1000, 0) + "μg / m3";
                drCountValues["NO2_Avg"] = GetPollutantValue(Convert.ToDecimal(dtForAQI.Compute("AVG(NO2)", "RegionName='" + name + "' and NO2 is not null").ToString()) * 1000, 0) + "μg / m3";
                drCountValues["SO2_Avg"] = GetPollutantValue(Convert.ToDecimal(dtForAQI.Compute("AVG(SO2)", "RegionName='" + name + "' and SO2 is not null").ToString()) * 1000, 0) + "μg / m3";
                drCountValues["CO_Avg"] = GetPollutantValue(Convert.ToDecimal(dtForAQI.Compute("AVG(CO)", "RegionName='" + name + "' and CO is not null").ToString()), 0) + "mg/m3";
                drCountValues["Max8HourO3_Avg"] = GetPollutantValue(Convert.ToDecimal(dtForAQI.Compute("AVG(Max8HourO3)", "RegionName='" + name + "' and Max8HourO3 is not null").ToString()) * 1000, 0) + "μg / m3";
                dtCountValues.Rows.Add(drCountValues);
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("[{\"AreaInfo\":");
            string json = dtCountValues.ToJsonBySerialize();
            sb.Append(json + "}]");
            return sb.ToString();
        }

        private static DataTable GetXY(string name)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("X", typeof(string));
            dt.Columns.Add("Y", typeof(string));
            DataRow dr = dt.NewRow();
            if (name.Contains("通州区"))
            {
                dr["X"] = "121.0922";
                dr["Y"] = "32.0666";
            }
            else if (name.Contains("如皋市"))
            {
                dr["X"] = "120.5708";
                dr["Y"] = "32.3812";
            }
            else if (name.Contains("海安县"))
            {
                dr["X"] = "120.4590";
                dr["Y"] = "32.5430";
            }
            else if (name.Contains("如东县"))
            {
                dr["X"] = "121.1747";
                dr["Y"] = "32.2981";
            }
            else if (name.Contains("通州湾示范区"))
            {
                dr["X"] = "121.2963";
                dr["Y"] = "32.1283";
            }
            else if (name.Contains("海门市"))
            {
                dr["X"] = "121.1584";
                dr["Y"] = "31.8947";
            }
            else if (name.Contains("南通市区"))
            {
                dr["X"] = "120.8580";
                dr["Y"] = "32.0039";
            }
            else if (name.Contains("启东市"))
            {
                dr["X"] = "121.6595";
                dr["Y"] = "31.8178";
            }
            dt.Rows.Add(dr);
            return dt;
        }

        /// <summary>
        /// 控制小数位
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="decimalNum">小数位数</param>
        /// <returns></returns>
        public static decimal GetPollutantValue(decimal value, int decimalNum)
        {
            if (decimalNum < 0)
                return value;
            decimal valuePow = value * Convert.ToInt32(Math.Pow(10, decimalNum));
            if (valuePow - Convert.ToDecimal(Math.Floor(valuePow)) == 0M)
                return Math.Round(value, decimalNum);
            else
                return Math.Round(value, decimalNum, MidpointRounding.ToEven);
        }

        /// <summary>
        /// 根据测点实体数组获取因子数据列
        /// </summary>
        /// <param name="monitoringPointList">测点实体数组</param>
        /// <returns></returns>
        private IList<PollutantCodeEntity> GetPollutantDataByPointList(IList<MonitoringPointEntity> monitoringPointList)
        {
            IList<PollutantCodeEntity> pollutantCodeList = new List<PollutantCodeEntity>();
            InstrumentChannelService instrumentChannelService = Singleton<InstrumentChannelService>.GetInstance();
            foreach (MonitoringPointEntity monitoringPointEntity in monitoringPointList)
            {
                IQueryable<PollutantCodeEntity> pollutantCodeQueryable =
                            instrumentChannelService.RetrieveChannelListByPointUid(monitoringPointEntity.MonitoringPointUid);
                pollutantCodeList = pollutantCodeList.Union(pollutantCodeQueryable).ToList();
            }
            return pollutantCodeList;
        }

        /// <summary>
        /// 根据站点类型获取某个监测因子的最新1小时IAQI分指数情况
        /// </summary>
        /// <returns>
        /// </returns>
        public string GetIAQIByFactorAndPointType(string[] pointType, string factor, string AreaGuid)
        {
            MonitoringPointDAL monitoringPointDAL = new MonitoringPointDAL();
            PortHourAQIDAL m_PortAQIDAL = new PortHourAQIDAL();
            AirPollutantService m_AirPollutantService = new AirPollutantService();
            PollutantCodeEntity pe = m_AirPollutantService.RetrieveEntityByCode(factor);  //根据因子code获取实体
            //Dictionary<string, string> dic = new Dictionary<string, string>();
            //DataView dv = monitoringPointDAL.GetMonitoringPointDataTableForData().AsDataView();
            //if (pointType[0] == "ALL")
            //{
            //    foreach (DataRowView drv in dv)
            //    {
            //        string pointId = drv["PointId"].ToString();
            //        string monitoringPointName = drv["MonitoringPointName"].ToString();
            //        dic.Add(pointId, monitoringPointName);
            //    }
            //}
            //else
            //{
            //    StringBuilder sbForPT = new StringBuilder();
            //    for (int i = 0; i < pointType.Length; i++)
            //    {
            //        sbForPT.Append("'" + pointType[i] + "',");
            //    }
            //    dv.RowFilter = "CGuid in (" + sbForPT.ToString().Substring(0, sbForPT.Length - 1) + ")";
            //    foreach (DataRowView drv in dv)
            //    {
            //        string pointId = drv["PointId"].ToString();
            //        string monitoringPointName = drv["MonitoringPointName"].ToString();
            //        dic.Add(pointId, monitoringPointName);
            //    }
            //}
            StringBuilder sb = new StringBuilder();
            sb.Append("[{\"PortIAQIBySiteType\":");
            DataTable dtForResult;
            if (factor == "a05027")
            {
                dtForResult = m_PortAQIDAL.GetAirQualityNewestOriRTReportForData("Recent8HoursO3NT").Table;
            }
            else if (factor == "a34004")
            {
                dtForResult = m_PortAQIDAL.GetAirQualityNewestOriRTReportForData("PM25").Table;
            }
            else
            {
                dtForResult = m_PortAQIDAL.GetAirQualityNewestOriRTReportForData(pe.ChemicalSymbol).Table;
            }
            dtForResult.Columns.Add("Unit", typeof(string));
            for (int i = 0; i < dtForResult.Rows.Count; i++)
            {
                dtForResult.Rows[i]["Unit"] = m_AirPollutantService.GetPollutantInfo(factor).PollutantMeasureUnit;
                if (!factor.Equals("a21005"))
                {
                    dtForResult.Rows[i][1] = (dtForResult.Rows[i][1] != DBNull.Value && dtForResult.Rows[i][1] != "") ? (Convert.ToDecimal(dtForResult.Rows[i][1].ToString()) * 1000).ToString("G0") : dtForResult.Rows[i][1];
                }
                foreach (DataColumn dc in dtForResult.Columns)
                {
                    if (dtForResult.Rows[i][dc.ColumnName] == DBNull.Value || dtForResult.Rows[i][dc.ColumnName] == "")
                    {
                        dtForResult.Rows[i][dc.ColumnName] = "--";
                    }
                }
            }
            string pids = GetPointIdByRegion(AreaGuid);
            DataView dvs = dtForResult.AsDataView();
            dvs.RowFilter = "PointId in (" + pids + ")";
            string json = dvs.ToTable().ToJsonBySerialize();
            sb.Append(json + "}]");
            return sb.ToString();
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public string GetIHourOrDayData(string portId, string factor, string type, DateTime dt1, DateTime dt2)
        {
            DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();
            if (type == "1")    //小时
            {
                InfectantBy60Service m_Min60Data = Singleton<InfectantBy60Service>.GetInstance();
                DateTime dtBegion = Convert.ToDateTime(dt1.ToString("yyyy-MM-dd HH:00:00"));
                DateTime dtEnd = Convert.ToDateTime(dt2.ToString("yyyy-MM-dd HH:59:59"));
                //NT O3早一小时做处理
                if (portId == "204")
                {
                    if (factor == "a05024")
                    {
                        string sql = string.Format(@"select a.PointId,CONVERT(varchar(13),dateadd(DAY,1,Tstamp),121) as Tstamp,PollutantValue,b.MonitoringPointName,b.X,b.Y,C.PollutantName,(CASE C.PollutantCode when 'a21005' then 'mg/m3' else 'μg/m3' end) as unit
                                                    from [Air].[TB_InfectantBy60] as a,[dbo].[SY_MonitoringPoint] as b,[dbo].[SY_PollutantCode] as c
                                                    where Tstamp >= '{0}' and Tstamp <= '{1}' and a.PointId = {2} and a.PollutantCode = '{3}' and a.PointId = b.PointId and a.PollutantCode = c.PollutantCode", dt1.AddHours(-1), dt2.AddHours(-1), portId, factor);
                        DataTable dt = g_DatabaseHelper.ExecuteDataTable(sql, "AMS_AirAutoMonitorConnection");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i]["PollutantValue"] != DBNull.Value && dt.Rows[i]["PollutantValue"].ToString().Trim() != "")
                            {
                                dt.Rows[i]["PollutantValue"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["PollutantValue"]), 1);
                            }
                        }
                        StringBuilder sb = new StringBuilder();
                        sb.Append("[{\"Data\":");
                        string json = dt.ToJsonBySerialize();
                        sb.Append(json + "}]");
                        return sb.ToString();
                    }
                    else
                    {
                        string sql = string.Format(@"select a.PointId,CONVERT(varchar(13),Tstamp,121) Tstamp,PollutantValue,b.MonitoringPointName,b.X,b.Y,C.PollutantName,(CASE C.PollutantCode when 'a21005' then 'mg/m3' else 'μg/m3' end) as unit
                                                    from [Air].[TB_InfectantBy60] as a,[dbo].[SY_MonitoringPoint] as b,[dbo].[SY_PollutantCode] as c
                                                    where Tstamp >= '{0}' and Tstamp <= '{1}' and a.PointId = {2} and a.PollutantCode = '{3}' and a.PointId = b.PointId and a.PollutantCode = c.PollutantCode", dt1, dt2, portId, factor);
                        DataTable dt = g_DatabaseHelper.ExecuteDataTable(sql, "AMS_AirAutoMonitorConnection");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (factor == "a21005")
                            {
                                if (dt.Rows[i]["PollutantValue"] != DBNull.Value && dt.Rows[i]["PollutantValue"].ToString().Trim() != "")
                                {
                                    dt.Rows[i]["PollutantValue"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["PollutantValue"]), 1);
                                }
                            }
                            else
                            {
                                if (dt.Rows[i]["PollutantValue"] != DBNull.Value && dt.Rows[i]["PollutantValue"].ToString().Trim() != "")
                                {
                                    dt.Rows[i]["PollutantValue"] = Convert.ToDecimal((DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["PollutantValue"]), 3) * 1000).ToString("G0"));
                                }
                            }
                        }
                        StringBuilder sb = new StringBuilder();
                        sb.Append("[{\"Data\":");
                        string json = dt.ToJsonBySerialize();
                        sb.Append(json + "}]");
                        return sb.ToString();
                    }
                }
                else
                {
                    string sql = string.Format(@"select a.PointId,CONVERT(varchar(13),Tstamp,121) Tstamp,PollutantValue,b.MonitoringPointName,b.X,b.Y,C.PollutantName,(CASE C.PollutantCode when 'a21005' then 'mg/m3' else 'μg/m3' end) as unit
                                                    from [Air].[TB_InfectantBy60] as a,[dbo].[SY_MonitoringPoint] as b,[dbo].[SY_PollutantCode] as c
                                                    where Tstamp >= '{0}' and Tstamp <= '{1}' and a.PointId = {2} and a.PollutantCode = '{3}' and a.PointId = b.PointId and a.PollutantCode = c.PollutantCode", dt1, dt2, portId, factor);
                    DataTable dt = g_DatabaseHelper.ExecuteDataTable(sql, "AMS_AirAutoMonitorConnection");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (factor == "a21005")
                        {
                            if (dt.Rows[i]["PollutantValue"] != DBNull.Value && dt.Rows[i]["PollutantValue"].ToString().Trim() != "")
                            {
                                dt.Rows[i]["PollutantValue"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["PollutantValue"]), 1);
                            }
                        }
                        else
                        {
                            if (dt.Rows[i]["PollutantValue"] != DBNull.Value && dt.Rows[i]["PollutantValue"].ToString().Trim() != "")
                            {
                                dt.Rows[i]["PollutantValue"] = Convert.ToDecimal((DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["PollutantValue"]), 3) * 1000).ToString("G0"));
                            }
                        }
                    }
                    StringBuilder sb = new StringBuilder();
                    sb.Append("[{\"Data\":");
                    string json = dt.ToJsonBySerialize();
                    sb.Append(json + "}]");
                    return sb.ToString();
                }
            }
            if (type == "2")
            {
                string sql = string.Format(@"select a.PointId,CONVERT(varchar(10),DateTime,121) DateTime,PollutantValue,b.MonitoringPointName,b.X,b.Y,C.PollutantName,(CASE C.PollutantCode when 'a21005' then 'mg/m3' else 'μg/m3' end) as unit
                                                    from [Air].[TB_InfectantByDay] as a,[dbo].[SY_MonitoringPoint] as b,[dbo].[SY_PollutantCode] as c
                                                    where DateTime >= '{0}' and DateTime <'{1}' and a.PointId = {2} and a.PollutantCode = '{3}'  and a.PointId = b.PointId and a.PollutantCode = c.PollutantCode", dt1, dt2.AddDays(1), portId, factor);
                DataTable dt = g_DatabaseHelper.ExecuteDataTable(sql, "AMS_AirAutoMonitorConnection");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (factor == "a21005")
                    {
                        if (dt.Rows[i]["PollutantValue"] != DBNull.Value && dt.Rows[i]["PollutantValue"].ToString().Trim() != "")
                        {
                            dt.Rows[i]["PollutantValue"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["PollutantValue"]), 1);
                        }
                    }
                    else
                    {
                        if (dt.Rows[i]["PollutantValue"] != DBNull.Value && dt.Rows[i]["PollutantValue"].ToString().Trim() != "")
                        {
                            dt.Rows[i]["PollutantValue"] = Convert.ToDecimal((DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i]["PollutantValue"]), 3) * 1000).ToString("G0"));
                        }
                    }
                }
                StringBuilder sb = new StringBuilder();
                sb.Append("[{\"Data\":");
                string json = dt.ToJsonBySerialize();
                sb.Append(json + "}]");
                return sb.ToString();
            }
            return null;
        }

        /// <summary>
        /// 根据站点Id获取最新1小时AQI数据
        /// </summary>
        /// <returns>
        /// </returns>
        public string GetAQIByPointId(string arraypointTypeRegion, string[] pointType, string AreaGuid)
        {
            MonitoringPointDAL monitoringPointDAL = new MonitoringPointDAL();
            PortHourAQIDAL m_PortAQIDAL = new PortHourAQIDAL();
            DataView dv = monitoringPointDAL.GetMonitoringPointDataTableForData().AsDataView();
            StringBuilder sb = new StringBuilder();
            sb.Append("[{\"PortIAQIBySiteType\":");
            Dictionary<string, string> dic = new Dictionary<string, string>();
            DataTable dtForResult = new DataTable();
            if (arraypointTypeRegion != "ALLR")
            {
                if (pointType[0] == "ALL")
                {
                    dv.RowFilter = "CGuid = '" + arraypointTypeRegion + "'";
                    List<string> list = new List<string>();
                    foreach (DataRowView drv in dv)
                    {
                        string pointId = drv["PointId"].ToString();
                        string monitoringPointName = drv["MonitoringPointName"].ToString() + "," + drv["PName"].ToString();
                        dic.Add(pointId, monitoringPointName);
                        list.Add(drv["PointId"].ToString());
                    }
                    dtForResult = m_PortAQIDAL.GetAirQualityNewestOriRTReportWithOutFac(list.ToArray()).Table;
                }
                else
                {
                    StringBuilder sb1 = new StringBuilder();
                    List<string> list = new List<string>();
                    foreach (string s in pointType)
                    {
                        sb1.Append("'" + s + "',");
                    }
                    dv.RowFilter = "CGuid = '" + arraypointTypeRegion + "' and PointId in (" + sb1.ToString().Trim(',') + ")";
                    foreach (DataRowView drv in dv)
                    {
                        string pointId = drv["PointId"].ToString();
                        string monitoringPointName = drv["MonitoringPointName"].ToString() + "," + drv["PName"].ToString();
                        dic.Add(pointId, monitoringPointName);
                        list.Add(drv["PointId"].ToString());
                    }
                    dtForResult = m_PortAQIDAL.GetAirQualityNewestOriRTReportWithOutFac(list.ToArray()).Table;
                }
            }
            if (arraypointTypeRegion == "ALLR")
            {
                dtForResult = m_PortAQIDAL.GetAirQualityNewestOriRTReportWithOutFac(new string[] { "ALL" }).Table;
                foreach (DataRowView drv in dv)
                {
                    string pointId = drv["PointId"].ToString();
                    string monitoringPointName = drv["MonitoringPointName"].ToString() + "," + drv["PName"].ToString();
                    dic.Add(pointId, monitoringPointName);
                }
            }
            dtForResult.Columns.Add("PortName", typeof(string));
            dtForResult.Columns.Add("PortType", typeof(string));
            for (int i = 0; i < dtForResult.Rows.Count; i++)
            {
                //dtForResult.Rows[i]["DateTime"] = Convert.ToDateTime(Convert.ToDateTime(dtForResult.Rows[i]["DateTime"]).ToString("yyyy-MM-dd HH:00:00"));
                dtForResult.Rows[i]["PortName"] = dic[dtForResult.Rows[i]["PointId"].ToString()].Split(',')[0];
                dtForResult.Rows[i]["PortType"] = dic[dtForResult.Rows[i]["PointId"].ToString()].Split(',')[1];
                foreach (DataColumn dc in dtForResult.Columns)
                {
                    if (dc.ColumnName.Equals("AQIValue") || dc.ColumnName.Equals("Class") || dc.ColumnName.Equals("Grade") || dc.ColumnName.Equals("HealthEffect") || dc.ColumnName.Equals("TakeStep") || dc.ColumnName.Equals("PrimaryPollutant"))
                        if (dtForResult.Rows[i][dc.ColumnName] == DBNull.Value || dtForResult.Rows[i][dc.ColumnName].ToString() == "")
                        {
                            dtForResult.Rows[i][dc.ColumnName] = "--";
                        }
                }
            }
            string pids = GetPointIdByRegion(AreaGuid);
            DataView dvs = dtForResult.AsDataView();
            if (pointType[0] != "ALL")
            {
                dvs.RowFilter = "PointId in (" + pids + ") and PointId in ('" + string.Join("','", pointType) + "')";
            }
            else
            {
                dvs.RowFilter = "PointId in (" + pids + ")";
            }

            string json = dvs.ToTable().ToJsonBySerialize();
            sb.Append(json + "}]");
            return sb.ToString();
        }


        /// <summary>
        /// 根据站点Id获取最新1小时AQI数据
        /// </summary>
        /// <returns>
        /// </returns>
        public string GetAQIByPointIdNew(string arraypointTypeRegion, string[] pointType, string AreaGuid)
        {
            MonitoringPointDAL monitoringPointDAL = new MonitoringPointDAL();
            PortHourAQIDAL m_PortAQIDAL = new PortHourAQIDAL();
            DataView dv = monitoringPointDAL.GetMonitoringPointDataTableForData().AsDataView();
            StringBuilder sb = new StringBuilder();
            sb.Append("[{\"PortIAQIBySiteType\":");
            Dictionary<string, string> dic = new Dictionary<string, string>();
            DataTable dtForResult = new DataTable();
            #region 非ALLR
            if (arraypointTypeRegion != "ALLR")
            {
                if (pointType[0] == "ALL")
                {
                    dv.RowFilter = "CGuid = '" + arraypointTypeRegion + "'";
                    List<string> list = new List<string>();
                    foreach (DataRowView drv in dv)
                    {
                        string pointId = drv["PointId"].ToString();
                        string monitoringPointName = drv["MonitoringPointName"].ToString() + "," + drv["PName"].ToString();
                        dic.Add(pointId, monitoringPointName);
                        list.Add(drv["PointId"].ToString());
                    }
                    dtForResult = m_PortAQIDAL.GetAirQualityNewestOriRTReportWithOutFac(list.ToArray()).Table;
                }
                else
                {
                    StringBuilder sb1 = new StringBuilder();
                    List<string> list = new List<string>();
                    foreach (string s in pointType)
                    {
                        sb1.Append("'" + s + "',");
                    }
                    dv.RowFilter = "CGuid = '" + arraypointTypeRegion + "' and PointId in (" + sb1.ToString().Trim(',') + ")";
                    foreach (DataRowView drv in dv)
                    {
                        string pointId = drv["PointId"].ToString();
                        string monitoringPointName = drv["MonitoringPointName"].ToString() + "," + drv["PName"].ToString();
                        dic.Add(pointId, monitoringPointName);
                        list.Add(drv["PointId"].ToString());
                    }
                    dtForResult = m_PortAQIDAL.GetAirQualityNewestOriRTReportWithOutFac(list.ToArray()).Table;
                }
            }
            #endregion
            if (arraypointTypeRegion == "ALLR")
            {
                dtForResult = m_PortAQIDAL.GetAirQualityNewestOriRTReportWithOutFac(new string[] { "ALL" }).Table;
                foreach (DataRowView drv in dv)
                {
                    string pointId = drv["PointId"].ToString();
                    string monitoringPointName = drv["MonitoringPointName"].ToString() + "," + drv["PName"].ToString();
                    dic.Add(pointId, monitoringPointName);
                }
            }
            dtForResult.Columns.Add("PortName", typeof(string));
            dtForResult.Columns.Add("PortType", typeof(string));
            for (int i = 0; i < dtForResult.Rows.Count; i++)
            {
                dtForResult.Rows[i]["DateTime"] = Convert.ToDateTime(dtForResult.Rows[i]["DateTime"]+":00:00").ToString("yyyy-MM-dd HH:00");
                dtForResult.Rows[i]["PortName"] = dic[dtForResult.Rows[i]["PointId"].ToString()].Split(',')[0];
                dtForResult.Rows[i]["PortType"] = dic[dtForResult.Rows[i]["PointId"].ToString()].Split(',')[1];
                foreach (DataColumn dc in dtForResult.Columns)
                {
                    if (dc.ColumnName.Equals("AQIValue") || dc.ColumnName.Equals("Class") || dc.ColumnName.Equals("Grade") || dc.ColumnName.Equals("HealthEffect") || dc.ColumnName.Equals("TakeStep") || dc.ColumnName.Equals("PrimaryPollutant"))
                        if (dtForResult.Rows[i][dc.ColumnName] == DBNull.Value || dtForResult.Rows[i][dc.ColumnName].ToString() == "")
                        {
                            dtForResult.Rows[i][dc.ColumnName] = "--";
                        }
                }
            }
            string pids = GetPointIdByRegion(AreaGuid);
            for (int k = 0; k < dtForResult.Columns.Count;k++ )
            {
                if (dtForResult.Columns[k].ColumnName == "PointId")
                {
                    dtForResult.Columns[k].ColumnName = "PortId";
                }
            }
            DataView dvs = dtForResult.AsDataView();

            if (pointType[0] != "ALL")
            {
                dvs.RowFilter = "PortId in (" + pids + ") and PortId in ('" + string.Join("','", pointType) + "')";
            }
            else
            {
                dvs.RowFilter = "PortId in (" + pids + ")";
            }

            string json = dvs.ToTable().ToJsonBySerialize();
            sb.Append(json + "}]");
            return sb.ToString();
        }


        /// <summary>
        /// 根据站点Id获取最新1小时因子数据
        /// </summary>
        /// <returns>
        /// </returns>
        public string GetFactorByPointId(string[] pointType)
        {
            MonitoringPointDAL monitoringPointDAL = new MonitoringPointDAL();
            PortHourAQIDAL m_PortAQIDAL = new PortHourAQIDAL();
            AirPollutantService m_AirPollutantService = new AirPollutantService();
            DataView dv = monitoringPointDAL.GetMonitoringPointDataTableForData().AsDataView();
            StringBuilder sb = new StringBuilder();
            sb.Append("[{\"PortFactor\":");
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (DataRowView drv in dv)
            {
                string pointId = drv["PointId"].ToString();
                string monitoringPointName = drv["MonitoringPointName"].ToString() + "," + drv["PName"].ToString();
                dic.Add(pointId, monitoringPointName);
            }
            DataTable dtForResult = m_PortAQIDAL.GetFactorByPointId(pointType).Table;
            dtForResult.Columns.Add("PortName", typeof(string));
            dtForResult.Columns.Add("PortType", typeof(string));
            dtForResult.Columns.Add("factorName", typeof(string));
            dtForResult.Columns.Add("factorUnit", typeof(string));
            for (int i = 0; i < dtForResult.Rows.Count; i++)
            {
                if (dtForResult.Rows[i]["PollutantCode"].ToString().Equals("a21005"))
                {
                    dtForResult.Rows[i]["PollutantValue"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dtForResult.Rows[i]["PollutantValue"] != DBNull.Value ? dtForResult.Rows[i]["PollutantValue"] : 0), 1);
                }
                else
                {
                    dtForResult.Rows[i]["PollutantValue"] = Convert.ToDecimal((DecimalExtension.GetPollutantValue(Convert.ToDecimal(dtForResult.Rows[i]["PollutantValue"] != DBNull.Value ? dtForResult.Rows[i]["PollutantValue"] : 0), 3) * 1000).ToString("G0"));
                }
                dtForResult.Rows[i]["Tstamp"] = Convert.ToDateTime(Convert.ToDateTime(dtForResult.Rows[i]["Tstamp"]).ToString("yyyy-MM-dd HH:00:00"));
                dtForResult.Rows[i]["PortName"] = dic[dtForResult.Rows[i]["PointId"].ToString()].Split(',')[0];
                dtForResult.Rows[i]["PortType"] = dic[dtForResult.Rows[i]["PointId"].ToString()].Split(',')[1];
                dtForResult.Rows[i]["factorName"] = m_AirPollutantService.GetPollutantInfo(dtForResult.Rows[i]["PollutantCode"].ToString()).PollutantName;
                dtForResult.Rows[i]["factorUnit"] = m_AirPollutantService.GetPollutantInfo(dtForResult.Rows[i]["PollutantCode"].ToString()).PollutantMeasureUnit;
            }
            string json = dtForResult.ToJsonBySerialize();
            sb.Append(json + "}]");
            return sb.ToString();
        }

        /// <summary>
        /// 获取空气质量日报数据
        /// </summary>
        /// <param name="portType">站点类型</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="qualityType">空气质量类别(优,良,轻度污染,中度污染,重度污染,严重污染)</param>
        /// <returns></returns>
        public string GetDayAQIJson(string[] portType, DateTime dtStart, DateTime dtEnd, string[] qualityType, string AreaGuid)
        {
            int totalRecords = 0;
            PortDayAQIDAL m_PortAQIDAL = Singleton<PortDayAQIDAL>.GetInstance();
            MonitoringPointDAL monitoringPointDAL = new MonitoringPointDAL();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            DataView dv = monitoringPointDAL.GetMonitoringPointDataTableForData().AsDataView();
            if (portType[0] == "ALL")
            {
                foreach (DataRowView drv in dv)
                {
                    string pointId = drv["PointId"].ToString();
                    string monitoringPointName = drv["MonitoringPointName"].ToString() + "," + drv["PName"].ToString();
                    dic.Add(pointId, monitoringPointName);
                }
            }
            else
            {
                StringBuilder sbForPT = new StringBuilder();
                for (int i = 0; i < portType.Length; i++)
                {
                    sbForPT.Append("'" + portType[i] + "',");
                }
                dv.RowFilter = "CGuid in (" + sbForPT.ToString().Substring(0, sbForPT.Length - 1) + ")";
                foreach (DataRowView drv in dv)
                {
                    string pointId = drv["PointId"].ToString();
                    string monitoringPointName = drv["MonitoringPointName"].ToString() + "," + drv["PName"].ToString();
                    dic.Add(pointId, monitoringPointName);
                }
            }
            DataTable dt = new DataTable();
            dt = m_PortAQIDAL.GetOriDataForData(dic.Keys.ToArray(), dtStart, dtEnd, GetClassByNum(qualityType), 99999, 0, out totalRecords).ToTable();
            dt.Columns.Add("PortName", typeof(string));
            dt.Columns.Add("PortType", typeof(string));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //dt.Rows[i]["DateTime"] = Convert.ToDateTime(dt.Rows[i]["DateTime"]).ToString("yyyy-MM-dd");
                dt.Rows[i]["PortName"] = dic[dt.Rows[i]["PointId"].ToString()].Split(',')[0];
                dt.Rows[i]["PortType"] = dic[dt.Rows[i]["PointId"].ToString()].Split(',')[1];
                foreach (DataColumn dc in dt.Columns)
                {
                    if (dc.ColumnName.Equals("AQIValue") || dc.ColumnName.Equals("Class") || dc.ColumnName.Equals("Grade") || dc.ColumnName.Equals("PrimaryPollutant"))
                    {
                        if (dt.Rows[i][dc.ColumnName] == DBNull.Value || dt.Rows[i][dc.ColumnName].ToString().Trim().Equals(""))
                        {
                            dt.Rows[i][dc.ColumnName] = "--";
                        }
                    }
                }
            }
            string pids = GetPointIdByRegion(AreaGuid);
            DataView dvs = dt.AsDataView();
            dvs.RowFilter = "PointId in (" + pids + ")";
            StringBuilder sb = new StringBuilder();
            sb.Append("[{\"DayAQI\":");
            string json = dvs.ToTable().ToJsonBySerialize();
            sb.Append(json + "}]");
            return sb.ToString(); ;
        }

        /// <summary>
        /// 根据测点获取空气质量日报数据
        /// </summary>
        /// <param name="portType">站点类型</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="qualityType">空气质量类别(优,良,轻度污染,中度污染,重度污染,严重污染)</param>
        /// <returns></returns>
        public string GetDayAQIByPointIdJson(string[] portType, DateTime dtStart, DateTime dtEnd, string[] qualityType)
        {
            int totalRecords = 0;
            PortDayAQIDAL m_PortAQIDAL = Singleton<PortDayAQIDAL>.GetInstance();
            MonitoringPointDAL monitoringPointDAL = new MonitoringPointDAL();

            DataView dv = monitoringPointDAL.GetMonitoringPointDataTableForData().AsDataView();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (DataRowView drv in dv)
            {
                string pointId = drv["PointId"].ToString();
                string monitoringPointName = drv["MonitoringPointName"].ToString() + "," + drv["PName"].ToString();
                dic.Add(pointId, monitoringPointName);
            }

            DataTable dt = new DataTable();
            dt = m_PortAQIDAL.GetOriDataForData(portType, dtStart, dtEnd, GetClassByNum(qualityType), 99999, 0, out totalRecords).ToTable();
            dt.Columns.Add("PortName", typeof(string));
            dt.Columns.Add("PortType", typeof(string));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["PortName"] = dic[dt.Rows[i]["PointId"].ToString()].Split(',')[0]; ;
                dt.Rows[i]["PortType"] = dic[dt.Rows[i]["PointId"].ToString()].Split(',')[1];
            }
            string json = "[{\"PortIAQIBySiteType\":" + dt.ToJsonBySerialize() + "}]";
            return json;
        }

        /// <summary>
        /// 根据数字获取类别
        /// </summary>
        /// <param name="qualityType"></param>
        /// <returns></returns>
        public string[] GetClassByNum(string[] qualityType)
        {
            if (qualityType.Contains("优") || qualityType.Contains("良") || qualityType.Contains("轻度污染") || qualityType.Contains("中度污染") || qualityType.Contains("重度污染") || qualityType.Contains("严重污染"))
            {
                return qualityType;
            }
            else
            {
                List<string> list = new List<string>();
                for (int i = 0; i < qualityType.Length; i++)
                {
                    switch (qualityType[i])
                    {
                        case "1":
                            list.Add("优");
                            break;
                        case "2":
                            list.Add("良");
                            break;
                        case "3":
                            list.Add("轻度污染");
                            break;
                        case "4":
                            list.Add("中度污染");
                            break;
                        case "5":
                            list.Add("重度污染");
                            break;
                        case "6":
                            list.Add("严重污染");
                            break;
                    }
                }
                return list.ToArray();
            }
        }

        /// <summary>
        /// 获取空气质量小时数据
        /// </summary>
        /// <param name="portIds">站点Id</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <returns></returns>
        public string GetHourAQIJson(string[] portIds, DateTime dtStart, DateTime dtEnd)
        {
            int totalRecords = 0;
            PortHourAQIDAL m_PortAQIDAL = Singleton<PortHourAQIDAL>.GetInstance();
            DataTable dt = new DataTable();
            dt = m_PortAQIDAL.GetOriDataPager(portIds, dtStart, dtEnd, 99999, 0, out totalRecords).ToTable();
            string json = dt.ToJsonBySerialize();
            return json;
        }





        /// <summary>
        /// 获取周边环境点位分布
        /// </summary>
        /// <returns>
        /// </returns>
        public string GetPortMessageAround(string pointId, string type, string fac)
        {
            DataTable dt = new DataTable();
            MonitoringPointDAL monitoringPointDAL = new MonitoringPointDAL();
            string[] pointIds=new string[]{"186"};
            if (pointId != "")
            {
                if (!pointId.Contains(";"))
                {
                    //if (pointId == "3" && type == "AQI")
                    //{
                    //    dt = monitoringPointDAL.GetPortMessageForData(pointIds, type, fac);
                    //}
                    //else
                    //{
                        dt = GetArroundTableIAQI(pointId, type, fac);
                    //}
                }
                else
                {
                    //pointId.Contains("186;188;187;189;190;205;206;204;199;200;201;198;202;203;191;192;193;194;195;196;197")
                    if (pointId.Contains("186") && pointId.Contains("187") && pointId.Contains("188") && pointId.Contains("189") && pointId.Contains("190") && pointId.Contains("191") && pointId.Contains("192") && pointId.Contains("193") && pointId.Contains("194") && pointId.Contains("195") && pointId.Contains("196") && pointId.Contains("197") && pointId.Contains("198") && pointId.Contains("199") && pointId.Contains("200") && pointId.Contains("201") && pointId.Contains("202") && pointId.Contains("203") && pointId.Contains("204") && pointId.Contains("205") && pointId.Contains("206"))
                    {
                        string[] pointArr = new string[]{"0","1","2","3"};
                        foreach (string point in pointArr)
                        {
                            DataTable dtForConcat = GetArroundTableIAQI(point, type, fac);
                            dt.Merge(dtForConcat);
                        }
                    }
                    else
                    {
                        string[] pointArr = pointId.Split(';');
                        foreach (string point in pointArr)
                        {
                            DataTable dtForConcat = GetArroundTableIAQI(point, type, fac);
                            dt.Merge(dtForConcat);
                        }
                    }
                }
            }
            else
            {
                string[] pointArr = "1;2;3;0".Split(';');
                foreach (string point in pointArr)
                {
                    DataTable dtForConcat = GetArroundTableIAQI(point, type, fac);
                    dt.Merge(dtForConcat);
                }
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("[{\"PortMessagesAround\":");
            string json = dt.ToJsonBySerialize();
            sb.Append(json + "}]");
            return sb.ToString();
        }

        /// <summary>
        /// 为接口提供数据
        /// </summary>
        /// <param name="pointId"></param>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <returns></returns>      
        public DataTable GetArroundTableIAQI(string pointId, string type, string fac)
        {
            string data = string.Empty;
            string data2 = string.Empty;
            DataTable dtjson = new DataTable();
            dtjson.Columns.Add("PointId", typeof(string));
            dtjson.Columns.Add("MonitoringPointName", typeof(string));
            dtjson.Columns.Add("DateTime", typeof(string));
            dtjson.Columns.Add("Value", typeof(string));
            dtjson.Columns.Add("IAQI", typeof(string));
            dtjson.Columns.Add("level", typeof(string));
            dtjson.Columns.Add("PrimaryPollutant", typeof(string));
            dtjson.Columns.Add("X", typeof(string));
            dtjson.Columns.Add("Y", typeof(string));
            //苏州
            if (pointId == "1")
            {
                //string url = string.Format("http://222.92.77.251:8083/MobileWs/AQI.asmx/Get24HoursFactorDataAir?portId={0}&factorName={1}", 0, fac);
                //HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                //System.Net.WebResponse response = request.GetResponse();
                //System.IO.Stream respStream = response.GetResponseStream();
                //using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, Encoding.GetEncoding("UTF-8")))
                //{
                //    data = reader.ReadToEnd();
                //}
                string url2 = string.Format("http://222.92.77.251:8083/MobileWs/AQI.asmx/GetLatestHourAQI?portId={0}", 0);
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url2);
                System.Net.WebResponse response = request.GetResponse();
                System.IO.Stream respStream = response.GetResponseStream();
                using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, Encoding.GetEncoding("UTF-8")))
                {
                    data2 = reader.ReadToEnd();
                }
                //DataTable dt = data.JsonToDataSet().Tables[0];
                DataTable dt2 = data2.JsonToDataSet().Tables[0];
                DataRow dr = dtjson.NewRow();
                dr["PointId"] = pointId;
                dr["MonitoringPointName"] = "苏州市";
                dr["X"] = "120.585";
                dr["Y"] = "31.299";
                dr["DateTime"] = dt2.Rows[0]["DateTime"].ToString();
                //DataView dv = dt.AsDataView();
                //dv.RowFilter = "DateTime = '" + dt2.Rows[0]["DateTime"].ToString() + "'";
                //dr["Value"] = dv[0]["value"].ToString();
                dr["Value"] = "";
                if (type.Equals("IAQI"))
                {
                    dr["IAQI"] = dt2.Rows[0][fac + "_IAQI"].ToString();
                    string level = string.Empty;
                    if (dt2.Rows[0][fac + "_IAQI"].ToString() != "")
                    {
                        int iaqi = Convert.ToInt32(dt2.Rows[0][fac + "_IAQI"].ToString());
                        if (iaqi <= 50 && iaqi > 0)
                        {
                            level = "优";
                        }
                        else if (iaqi <= 100)
                        {
                            level = "良";
                        }
                        else if (iaqi <= 150)
                        {
                            level = "轻度污染";
                        }
                        else if (iaqi <= 200)
                        {
                            level = "中度污染";
                        }
                        else if (iaqi <= 300)
                        {
                            level = "重度污染";
                        }
                        else if (iaqi > 300)
                        {
                            level = "严重污染";
                        }
                        else
                            level = "无效天";
                    }
                    dr["level"] = level;
                }
                if (type.Equals("AQI"))
                {
                    dr["IAQI"] = dt2.Rows[0]["AQI"].ToString();
                    dr["level"] = dt2.Rows[0]["Class"].ToString();
                    dr["PrimaryPollutant"] = dt2.Rows[0]["PrimaryPollutant"].ToString();
                }
                if (type.Equals(""))
                {
                    dr["IAQI"] = dt2.Rows[0]["AQI"].ToString();
                    dr["level"] = "";
                }
                dtjson.Rows.Add(dr);
                return dtjson;
            }
            //张家港
            else if (pointId == "2")
            {
                ServiceReference1.GISServiceSoapClient client = new ServiceReference1.GISServiceSoapClient();
                string zjgData = string.Empty;
                DataTable dtt = new DataTable();
                if (type.Equals("AQI"))
                {
                    zjgData = client.GetPortALLDetailZJG("hour", "1").TrimStart('[').TrimEnd(']');
                    dtt = zjgData.JsonToDataSet().Tables[0];
                    DataRow dr = dtjson.NewRow();
                    dr["PointId"] = pointId;
                    dr["MonitoringPointName"] = "张家港市";
                    dr["X"] = "120.55";
                    dr["Y"] = "31.87";
                    dr["DateTime"] = dtt.Rows[0]["DateTime"].ToString();
                    dr["Value"] = "";
                    dr["IAQI"] = dtt.Rows[0]["AQI"].ToString();
                    dr["level"] = dtt.Rows[0]["Class"].ToString();
                    dr["PrimaryPollutant"] = dtt.Rows[0]["PrimaryPollutant"].ToString();
                    dtjson.Rows.Add(dr);
                }
                if (type.Equals("IAQI"))
                {
                    string factor = string.Empty;
                    switch (fac)
                    {
                        case "SO2":
                            factor = "SO2";
                            break;
                        case "NO2":
                            factor = "NO2";
                            break;
                        case "PM10":
                            factor = "PM10_1";
                            break;
                        case "CO":
                            factor = "CO";
                            break;
                        case "O3":
                            factor = "O3_1";
                            break;
                        case "PM2.5":
                            factor = "PM25_1";
                            break;
                    }
                    zjgData = client.GetHourDataByZJG("1", factor).TrimStart('[').TrimEnd(']');
                    dtt = zjgData.JsonToDataSet().Tables[0];
                    DataRow dr = dtjson.NewRow();
                    dr["PointId"] = pointId;
                    dr["MonitoringPointName"] = "张家港市";
                    dr["X"] = "120.55";
                    dr["Y"] = "31.87";
                    dr["DateTime"] = dtt.Rows[0]["DateTime"].ToString();
                    dr["Value"] = dtt.Rows[0]["FactorValue"].ToString();
                    dr["IAQI"] = dtt.Rows[0]["IAQI"].ToString();
                    string level = string.Empty;
                    int iaqi = Convert.ToInt32(dtt.Rows[0]["IAQI"].ToString());
                    if (iaqi <= 50 && iaqi > 0)
                    {
                        level = "优";
                    }
                    else if (iaqi <= 100)
                    {
                        level = "良";
                    }
                    else if (iaqi <= 150)
                    {
                        level = "轻度污染";
                    }
                    else if (iaqi <= 200)
                    {
                        level = "中度污染";
                    }
                    else if (iaqi <= 300)
                    {
                        level = "重度污染";
                    }
                    else if (iaqi > 300)
                    {
                        level = "严重污染";
                    }
                    else
                        level = "无效天";
                    dr["level"] = level;
                    dtjson.Rows.Add(dr);
                }
                if (type.Equals(""))
                {
                    zjgData = client.GetPortALLDetailZJG("hour", "1").TrimStart('[').TrimEnd(']');
                    dtt = zjgData.JsonToDataSet().Tables[0];
                    DataRow dr = dtjson.NewRow();
                    dr["PointId"] = pointId;
                    dr["MonitoringPointName"] = "张家港市";
                    dr["X"] = "120.55";
                    dr["Y"] = "31.87";
                    dr["DateTime"] = dtt.Rows[0]["DateTime"].ToString();
                    dr["Value"] = "";
                    dr["IAQI"] = dtt.Rows[0]["AQI"].ToString();
                    dr["level"] = "";
                    dtjson.Rows.Add(dr);
                }
                return dtjson;
            }
            //南通
            else if (pointId == "3")
            {
                string tableName = "AMS_AirAutoMonitor.Air.TB_OriRegionHourAQI";

                string sql = string.Format(@"select {0} PortName,
                                                Convert(nvarchar(16),dateadd(hh,0,[datetime]),21) DateTime,
                                                ISNULL(SO2_IAQI,'--') SO2_IAQI,
                                                ISNULL(NO2_IAQI,'--') NO2_IAQI,
                                                ISNULL(PM10_IAQI,'--') PM10_IAQI,
                                                ISNULL(CO_IAQI,'--') CO_IAQI,
                                                ISNULL(O3_IAQI,'--') O3_IAQI,
                                                ISNULL(Recent8HoursO3_IAQI,'--') Recent8HoursO3_IAQI,
                                                ISNULL([PM25_IAQI],'--') [PM25_IAQI],
                                                ISNULL(AQIValue,'--') AQI,
                                                ISNULL(PrimaryPollutant,'--') PrimaryPollutant,
                                                ISNULL(Class,'--') Class, 
                                                ISNULL(Grade,'--') Grade,
                                                ISNULL(RGBValue,'--') RGBValue,
                                                ISNULL(HealthEffect,'--') HealthEffect,
                                                ISNULL(TakeStep,'--') TakeStep ,
ISNULL(SO2,'--') SO2 ,
ISNULL(NO2,'--') NO2 ,
ISNULL(PM10,'--') PM10 ,
ISNULL(CO,'--') CO ,
ISNULL(O3,'--') O3 ,
ISNULL(PM25,'--') PM25
                                           from {1} AS v 
                                                {2} 
                                          where v.[dateTime] = (  
                                                                select MAX([dateTime])
                                                                  from {1} 
                                                                 where {3} [dateTime] > DATEADD(DAY, -7, GETDATE())
                                                                   and [dateTime] < GETDATE()
                                                                ) 
                                             {4}"
                                                  , "'3' AS PortId, '南通市' AS"
                                                  , tableName
                                                  , ""
                                                  , string.Format(" MonitoringRegionUid = '{0}' and ", monitorRegionUid)
                                                  , string.Format("and v.MonitoringRegionUid = '{0}'", monitorRegionUid)
                                                  );
                DataTable dt = g_DatabaseHelper.ExecuteDataTable(sql, "AMS_AirAutoMonitorConnection");
                if (type.Equals("AQI"))
                {
                    string[] pointIds = new string[] { "186", "187", "188", "189", "190","198","191","194","202","201","193","200","192","196","195","203","197","199","204","206","205" };
                    string field = string.Format(@"B.*,C.MonitoringPointName,C.X,C.Y,
                                               Class as level, (CONVERT(varchar(13),B.DateTime,121)+':00:00') DateeTime", fac, fac + "_IAQI");
                    string sqls = string.Format(@"select {1} from 
(select pointid,max(DateTime) DateTime from {0} group by pointid) A
left join {0} B 
on B.DateTime = A.DateTime and B.PointId = A.pointid
left join [dbo].[SY_MonitoringPoint] C on A.PointId = C.PointId where A.PointId in ({2})", "[AMS_AirAutoMonitor].[Air].[TB_OriHourAQI]", field, "'" + string.Join("','", pointIds) + "'");
                    dt = g_DatabaseHelper.ExecuteDataTable(sqls, "AMS_MonitoringBusinessConnection");

                    //DataRow dr = dtjson.NewRow();
                    //dr["PointId"] = pointId;
                    //dr["MonitoringPointName"] = "南通市";
                    //dr["X"] = "120.86";
                    //dr["Y"] = "32.01";
                    //dr["DateTime"] = dt.Rows[0]["DateTime"].ToString();
                    //dr["Value"] = "";
                    //dr["IAQI"] = dt.Rows[0]["AQI"].ToString();
                    //dr["level"] = dt.Rows[0]["Class"].ToString();
                    //dr["PrimaryPollutant"] = dt.Rows[0]["PrimaryPollutant"].ToString();
                    //dtjson.Rows.Add(dr);
                    for (int k = 0; k < pointIds.Length;k++ )
                    {
                        DataRow dr = dtjson.NewRow();
                        dr["PointId"] = dt.Rows[k]["PointId"].ToString();
                        //dr["PointId"] = "3";
                        dr["MonitoringPointName"] = dt.Rows[k]["MonitoringPointName"].ToString();
                        dr["X"] = dt.Rows[k]["X"].ToString();
                        dr["Y"] = dt.Rows[k]["Y"].ToString();
                        dr["DateTime"] = dt.Rows[k]["DateeTime"].ToString();
                        dr["Value"] = "";
                        dr["IAQI"] = dt.Rows[k]["AQIValue"].ToString();
                        dr["level"] = dt.Rows[k]["Class"].ToString();
                        dr["PrimaryPollutant"] = dt.Rows[k]["PrimaryPollutant"].ToString();
                        dtjson.Rows.Add(dr);
                    }
                }
                if (type.Equals("IAQI"))
                {
                    string factor = string.Empty;
                    switch (fac)
                    {
                        case "SO2":
                            factor = "SO2";
                            break;
                        case "NO2":
                            factor = "NO2";
                            break;
                        case "PM10":
                            factor = "PM10";
                            break;
                        case "CO":
                            factor = "CO";
                            break;
                        case "O3":
                            factor = "O3";
                            break;
                        case "PM2.5":
                            factor = "PM25";
                            break;
                    }
                    string iAQI = dt.Rows[0][factor + "_IAQI"].ToString();
                    DataRow dr = dtjson.NewRow();
                    dr["PointId"] = pointId;
                    dr["MonitoringPointName"] = "南通市";
                    dr["X"] = "120.86";
                    dr["Y"] = "32.01";
                    dr["DateTime"] = dt.Rows[0]["DateTime"].ToString();
                    dr["Value"] = dt.Rows[0][factor].ToString();
                    dr["IAQI"] = iAQI;
                    string level = string.Empty;
                    if (iAQI != "" || iAQI != "--")
                    {
                        int iaqi = Convert.ToInt32(iAQI);
                        if (iaqi <= 50 && iaqi > 0)
                        {
                            level = "优";
                        }
                        else if (iaqi <= 100)
                        {
                            level = "良";
                        }
                        else if (iaqi <= 150)
                        {
                            level = "轻度污染";
                        }
                        else if (iaqi <= 200)
                        {
                            level = "中度污染";
                        }
                        else if (iaqi <= 300)
                        {
                            level = "重度污染";
                        }
                        else if (iaqi > 300)
                        {
                            level = "严重污染";
                        }
                        else
                            level = "无效天";
                        dr["level"] = level;
                        dtjson.Rows.Add(dr);
                    }
                }
                if (type.Equals(""))
                {
                    DataRow dr = dtjson.NewRow();
                    dr["PointId"] = pointId;
                    dr["MonitoringPointName"] = "南通市";
                    dr["X"] = "120.86";
                    dr["Y"] = "32.01";
                    dr["DateTime"] = dt.Rows[0]["DateTime"].ToString();
                    dr["Value"] = "";
                    dr["IAQI"] = dt.Rows[0]["AQI"].ToString();
                    dr["level"] = "";
                    dtjson.Rows.Add(dr);
                }
                return dtjson;
            }
            //上海
            else if (pointId == "0")
            {
                DataTable dtt = SHrealTimeAQI();
                if (type.Equals("AQI"))
                {
                    DataRow dr = dtjson.NewRow();
                    dr["PointId"] = pointId;
                    dr["MonitoringPointName"] = "上海市";
                    dr["X"] = "121.4";
                    dr["Y"] = "31.2";
                    dr["DateTime"] = dtt.Rows[0]["DateTime"].ToString();
                    dr["Value"] = "";
                    dr["IAQI"] = dtt.Rows[0]["AQIValue"].ToString();
                    dr["level"] = dtt.Rows[0]["Class"].ToString();
                    dr["PrimaryPollutant"] = dtt.Rows[0]["PrimaryPollutant"].ToString();
                    dtjson.Rows.Add(dr);
                }
                if (type.Equals("IAQI"))
                {
                    DataRow dr = dtjson.NewRow();
                    dr["PointId"] = pointId;
                    dr["MonitoringPointName"] = "上海市";
                    dr["X"] = "121.4";
                    dr["Y"] = "31.2";
                    dr["DateTime"] = dtt.Rows[0]["DateTime"].ToString();
                    dr["Value"] = "";
                    dr["IAQI"] = "";
                    dr["level"] = dtt.Rows[0]["Class"].ToString();
                    dtjson.Rows.Add(dr);
                }
                if (type.Equals(""))
                {
                    DataRow dr = dtjson.NewRow();
                    dr["PointId"] = pointId;
                    dr["MonitoringPointName"] = "上海市";
                    dr["X"] = "121.4";
                    dr["Y"] = "31.2";
                    dr["DateTime"] = dtt.Rows[0]["DateTime"].ToString();
                    dr["Value"] = "";
                    dr["IAQI"] = dtt.Rows[0]["AQIValue"].ToString();
                    dr["level"] = "";
                    dtjson.Rows.Add(dr);
                }
                return dtjson;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取周边环境点位分布
        /// </summary>
        /// <returns>
        /// </returns>                      
        public string GetPortMessageAround(string pointId, string type, string fac, DateTime dt1, DateTime dt2)
        {
            DataTable dt = new DataTable();
            if (pointId != "")
            {
                if (!pointId.Contains(";"))
                {
                    dt = GetArroundTable(pointId, dt1, dt2);
                }
                else
                {
                    string[] pointArr = pointId.Split(';');
                    foreach (string point in pointArr)
                    {
                        DataTable dtForConcat = GetArroundTable(point, dt1, dt2);
                        dt.Merge(dtForConcat);
                    }
                }
            }
            else
            {
                string[] pointArr = "1;2;3;0".Split(';');
                foreach (string point in pointArr)
                {
                    DataTable dtForConcat = GetArroundTable(point, dt1, dt2);
                    dt.Merge(dtForConcat);
                }
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("[{\"PortMessagesAround\":");
            string json = dt.ToJsonBySerialize();
            sb.Append(json + "}]");
            return sb.ToString();
        }

        /// <summary>
        /// 为接口提供数据
        /// </summary>
        /// <param name="pointId"></param>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <returns></returns>      
        public DataTable GetArroundTable(string pointId, DateTime dt1, DateTime dt2)
        {
            string data = string.Empty;
            DataTable dt = new DataTable();
            //南通
            if (pointId == "3")
            {
                string tableName = "AirReport.TB_RegionDayAQIReport";
                string sql = string.Format(@" 
                                        SELECT Convert(nvarchar(10),{0},21) as DateTime,
                                               AQIValue AQI,
                                               PrimaryPollutant,
                                               RGBValue,
                                               Class,
                                               Grade,
                                               HealthEffect,
                                               TakeStep 
                                         FROM {1}
                                        WHERE {0} >= '{2}' 
                                          and {0} < '{3}' 
                                          {4} 
                                        order by {0}"
                                           , "ReportDateTime"
                                           , tableName
                                           , dt1.AddDays(-1)
                                           , dt2
                                           , string.Format(" and MonitoringRegionUid = '{0}'", monitorRegionUid));

                dt = g_DatabaseHelper.ExecuteDataTable(sql, "AMS_MonitoringBusinessConnection");
                foreach (DataColumn dc in dt.Columns)
                {
                    if (dc.ColumnName.Equals("Class"))
                    {
                        dc.ColumnName = "level";
                    }
                }
            }
            //苏州
            if (pointId == "1")
            {
                string url = string.Format("http://222.92.77.251:8083/MobileWs/AQI.asmx/GetDayAQIByDatetime?portId={0}&startDay={1}&endDay={2}", 0, dt1.AddDays(-2), dt2);
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                System.Net.WebResponse response = request.GetResponse();
                System.IO.Stream respStream = response.GetResponseStream();
                using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, Encoding.GetEncoding("UTF-8")))
                {
                    data = reader.ReadToEnd();
                }
                dt = data.JsonToDataSet().Tables[0];
            }
            //张家港
            if (pointId == "2")
            {
                ServiceReference1.GISServiceSoapClient client = new ServiceReference1.GISServiceSoapClient();
                string zjgData = client.GetPortALLDetailZJG("day", "1").TrimStart('[').TrimEnd(']');
                DataTable dtt = zjgData.JsonToDataSet().Tables[0];
                dt.Columns.Add("DateTime", typeof(string));
                dt.Columns.Add("AQI", typeof(string));
                dt.Columns.Add("PrimaryPollutant", typeof(string));
                dt.Columns.Add("RGBValue", typeof(string));
                dt.Columns.Add("Class", typeof(string));
                dt.Columns.Add("Grade", typeof(string));
                dt.Columns.Add("HealthEffect", typeof(string));
                dt.Columns.Add("TakeStep", typeof(string));
                DataRow dr = dt.NewRow();
                dr["DateTime"] = dtt.Rows[0]["DateTime"].ToString();
                dr["AQI"] = dtt.Rows[0]["AQI"].ToString();
                dr["PrimaryPollutant"] = dtt.Rows[0]["PrimaryPollutant"].ToString();
                dr["RGBValue"] = dtt.Rows[0]["RGBValue"].ToString();
                dr["Class"] = dtt.Rows[0]["Class"].ToString();
                dr["Grade"] = dtt.Rows[0]["Grade"].ToString();
                dr["HealthEffect"] = dtt.Rows[0]["HealthEffect"].ToString();
                dr["TakeStep"] = dtt.Rows[0]["TakeStep"].ToString();
                dt.Rows.Add(dr);
            }
            //上海
            if (pointId == "0")
            {
                DataTable dtt = SHdayTimeAQI();
                dt.Columns.Add("DateTime", typeof(string));
                dt.Columns.Add("AQI", typeof(string));
                dt.Columns.Add("PrimaryPollutant", typeof(string));
                dt.Columns.Add("RGBValue", typeof(string));
                dt.Columns.Add("Class", typeof(string));
                dt.Columns.Add("Grade", typeof(string));
                dt.Columns.Add("HealthEffect", typeof(string));
                dt.Columns.Add("TakeStep", typeof(string));
                DataRow dr = dt.NewRow();
                if (dtt.Rows.Count > 0)
                {
                    dr["DateTime"] = dtt.Rows[0]["DateTime"].ToString();
                    dr["AQI"] = dtt.Rows[0]["AQIValue"].ToString();
                    dr["PrimaryPollutant"] = dtt.Rows[0]["PrimaryPollutant"].ToString();
                    dr["RGBValue"] = dtt.Rows[0]["RGBValue"].ToString();
                    dr["Class"] = dtt.Rows[0]["Class"].ToString();
                    dr["Grade"] = dtt.Rows[0]["Grade"].ToString();
                    dr["HealthEffect"] = "--";
                    dr["TakeStep"] = "--";
                    dt.Rows.Add(dr);
                }
            }
            dt.Columns.Add("PointId", typeof(string));
            dt.Columns.Add("MonitoringPointName", typeof(string));
            dt.Columns.Add("X", typeof(string));
            dt.Columns.Add("Y", typeof(string));
            foreach (DataColumn dc in dt.Columns)
            {
                if (dc.ColumnName.Equals("Class"))
                {
                    dc.ColumnName = "level";
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dc.ColumnName.Equals("PointId"))
                    {
                        switch (pointId)
                        {
                            case "1":
                                dt.Rows[i][dc.ColumnName] = "1";
                                break;
                            case "2":
                                dt.Rows[i][dc.ColumnName] = "2";
                                break;
                            case "0":
                                dt.Rows[i][dc.ColumnName] = "0";
                                break;
                            case "3":
                                dt.Rows[i][dc.ColumnName] = "3";
                                break;
                        }
                    }
                    if (dc.ColumnName.Equals("MonitoringPointName"))
                    {
                        switch (pointId)
                        {
                            case "1":
                                dt.Rows[i][dc.ColumnName] = "苏州市";
                                break;
                            case "2":
                                dt.Rows[i][dc.ColumnName] = "张家港市";
                                break;
                            case "0":
                                dt.Rows[i][dc.ColumnName] = "上海市";
                                break;
                            case "3":
                                dt.Rows[i][dc.ColumnName] = "南通市";
                                break;
                        }
                    }
                    if (dc.ColumnName.Equals("X"))
                    {
                        switch (pointId)
                        {
                            case "1":
                                dt.Rows[i][dc.ColumnName] = "120.585";
                                break;
                            case "2":
                                dt.Rows[i][dc.ColumnName] = "120.55";
                                break;
                            case "0":
                                dt.Rows[i][dc.ColumnName] = "121.4";
                                break;
                            case "3":
                                dt.Rows[i][dc.ColumnName] = "120.86";
                                break;
                        }
                    }
                    if (dc.ColumnName.Equals("Y"))
                    {
                        switch (pointId)
                        {
                            case "1":
                                dt.Rows[i][dc.ColumnName] = "31.299";
                                break;
                            case "2":
                                dt.Rows[i][dc.ColumnName] = "31.87";
                                break;
                            case "0":
                                dt.Rows[i][dc.ColumnName] = "31.2";
                                break;
                            case "3":
                                dt.Rows[i][dc.ColumnName] = "32.01";
                                break;
                        }
                    }
                }
            }
            return dt;
        }

        /// <summary>
        /// 获取因子信息
        /// </summary>
        /// <returns>
        /// </returns>
        public string GetFactorByType(string pointId, string id)
        {
            if (pointId == "1")
            {
                //string factors = ConfigurationManager.AppSettings[id];
                //double result = 0;
                //string[] factorArr = factors.Split(',');
                //foreach (string factor in factorArr)
                //{
                //    if (over.Contains(factorName))
                //    {
                //        result = Convert.ToDouble(over.Split(':')[1]);
                //    }
                //}
                //return result;

                if (id == "")
                    id = "204";
                DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();
                string sql = string.Format(@"SELECT TOP 1000 
      A.[PointId]
      ,[MonitoringPointName]
      ,A.[PollutantCode]
      ,A.[PollutantName]
      ,ItemText Unit
  FROM [AMS_BaseData].[dbo].[V_Point_InstrumentChannels] A,[dbo].[SY_View_CodeMainItem] B,[AMS_BaseData].[Standard].[TB_PollutantCode] C
  where 
  InstrumentName in('常规参数','气象五参数分析仪','超级站常规参数') and 
	A.PointId = {0} and A.PollutantCode in ({1}) and B.ItemGuid = C.MeasureUnitUid and A.PollutantCode = C.PollutantCode", id, "'a34004','a05024','a21005','a34002','a21026','a21004'");
                DataTable dt = g_DatabaseHelper.ExecuteDataTable(sql, "AMS_BaseDataConnection");
                StringBuilder sb = new StringBuilder();
                sb.Append("[{\"FactorList\":");
                string json = dt.ToJsonBySerialize();
                sb.Append(json + "}]");
                return sb.ToString();
            }
            if (pointId == "2")
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("PointId", typeof(string));
                dt.Columns.Add("MonitoringPointName", typeof(string));
                dt.Columns.Add("PollutantCode", typeof(string));
                dt.Columns.Add("PollutantName", typeof(string));
                dt.Columns.Add("Unit", typeof(string));
                for (int i = 0; i < 6; i++)
                {
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                }
                string name = string.Empty;
                switch (id)
                {
                    case "0":
                        name = "上海市";
                        break;
                    case "1":
                        name = "苏州市";
                        break;
                    case "2":
                        name = "张家港市";
                        break;
                }
                dt.Rows[2]["PointId"] = id;
                dt.Rows[2]["MonitoringPointName"] = name;
                dt.Rows[2]["PollutantCode"] = "a21004";
                dt.Rows[2]["PollutantName"] = "NO₂";
                dt.Rows[2]["Unit"] = "μg/m³";
                dt.Rows[5]["PointId"] = id;
                dt.Rows[5]["MonitoringPointName"] = name;
                dt.Rows[5]["PollutantCode"] = "a05024";
                dt.Rows[5]["PollutantName"] = "O₃";
                dt.Rows[5]["Unit"] = "μg/m³";
                dt.Rows[4]["PointId"] = id;
                dt.Rows[4]["MonitoringPointName"] = name;
                dt.Rows[4]["PollutantCode"] = "a21005";
                dt.Rows[4]["PollutantName"] = "CO";
                dt.Rows[4]["Unit"] = "mg/m³";
                dt.Rows[1]["PointId"] = id;
                dt.Rows[1]["MonitoringPointName"] = name;
                dt.Rows[1]["PollutantCode"] = "a34002";
                dt.Rows[1]["PollutantName"] = "PM₁₀";
                dt.Rows[1]["Unit"] = "μg/m³";
                dt.Rows[3]["PointId"] = id;
                dt.Rows[3]["MonitoringPointName"] = name;
                dt.Rows[3]["PollutantCode"] = "a21026";
                dt.Rows[3]["PollutantName"] = "SO₂";
                dt.Rows[3]["Unit"] = "μg/m³";
                dt.Rows[0]["PointId"] = id;
                dt.Rows[0]["MonitoringPointName"] = name;
                dt.Rows[0]["PollutantCode"] = "a34004";
                dt.Rows[0]["PollutantName"] = "PM₂.₅";
                dt.Rows[0]["Unit"] = "μg/m³";
                StringBuilder sb = new StringBuilder();
                sb.Append("[{\"FactorList\":");
                string json = dt.ToJsonBySerialize();
                sb.Append(json + "}]");
                return sb.ToString();
            }
            return null;
        }


        /// <summary>
        /// 获取实时浓度
        /// </summary>
        /// <returns>
        /// </returns>       
        public string GetLastHourDataByType(string type, string pointId, string AreaGuid)
        {
            //南通
            if (type == "1")
            {
                DataTable dt = new DataTable();
                if (pointId != "")
                {
                    dt = GetDataForHour(type, pointId);
                    string pids = GetPointIdByRegion(AreaGuid);
                    DataView dvs = dt.AsDataView();
                    dvs.RowFilter = "PointId in (" + pids + ")";
                    StringBuilder sb = new StringBuilder();
                    sb.Append("[{\"HistoryHourData\":");
                    string json = dvs.ToTable().ToJsonBySerialize();
                    sb.Append(json + "}]");
                    return sb.ToString();
                }
                else
                {
                    string[] arr = new string[] { "186", "187", "188", "189", "190", "198", "191", "194", "202", "201", "193", "200", "192", "196", "195", "203", "197", "199", "204", "206", "205" };
                    foreach (string pid in arr)
                    {
                        DataTable dtt = GetDataForHour(type, pid);
                        dt.Merge(dtt);
                    }
                    string pids = GetPointIdByRegion(AreaGuid);
                    DataView dvs = dt.AsDataView();
                    dvs.RowFilter = "PointId in (" + pids + ")";
                    StringBuilder sb = new StringBuilder();
                    sb.Append("[{\"HistoryHourData\":");
                    string json = dvs.ToTable().ToJsonBySerialize();
                    sb.Append(json + "}]");
                    return sb.ToString();
                }
            }
            //除了南通
            if (type == "2")
            {
                DataTable dt = new DataTable();
                if (pointId != "")
                {
                    dt = GetDataForHour(type, pointId);
                    StringBuilder sb = new StringBuilder();
                    sb.Append("[{\"HistoryHourData\":");
                    string json = dt.ToJsonBySerialize();
                    sb.Append(json + "}]");
                    return sb.ToString();
                }
                else
                {
                    string[] arr = new string[] { "0", "1", "2"};
                    //186;188;187;189;190;205;206;204;199;200;201;198;202;203;191;192;193;194;195;196;197
                    string[] arrr = new string[] { "186", "188", "187", "189", "190", "205", "206","204", "199", "200", "201", "198", "202", "203", "191", "192", "193", "194", "195", "196", "197"};
                    foreach (string pid in arr)
                    {
                        DataTable dtt = GetDataForHour(type, pid);
                        dt.Merge(dtt);
                    }
                    foreach (string pid in arrr)
                    {
                        DataTable dtt = GetDataForHour("1", pid);
                        dt.Merge(dtt);
                    }
                    StringBuilder sb = new StringBuilder();
                    sb.Append("[{\"HistoryHourData\":");
                    string json = dt.ToJsonBySerialize();
                    sb.Append(json + "}]");
                    return sb.ToString();
                }
            }
            return null;
        }

        /// <summary>
        /// 获取数据给实时浓度
        /// </summary>
        /// <returns></returns>
        public DataTable GetDataForHour(string type, string pointId)
        {
            if (type == "1")
            {
                DateTime dtstart = Convert.ToDateTime((DateTime.Now.ToString("yyyy-MM-dd HH:00:00"))).AddHours(-1);
                DateTime dtend = Convert.ToDateTime((DateTime.Now.ToString("yyyy-MM-dd HH:00:00")));
                string fieldName = "Tstamp,PointId,PollutantCode,PollutantValue";
                DataTable dtjson = new DataTable();
                dtjson.Columns.Add("PointId", typeof(string));
                dtjson.Columns.Add("MonitoringPointName", typeof(string));
                dtjson.Columns.Add("DateTime", typeof(string));
                DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();
                string sql = string.Format(@"SELECT TOP 1000 
      [PointId]
      ,[MonitoringPointName]
      ,A.[PollutantCode]
      ,A.[PollutantName]
      ,ItemText Unit
  FROM [AMS_BaseData].[dbo].[V_Point_InstrumentChannels] A,[AMS_BaseData].[Standard].[TB_PollutantCode] B,[EQMS_Framework].[dbo].[TB_Frame_CodeItem] C
  where 
  InstrumentName in('常规参数','气象五参数分析仪','超级站常规参数') and A.[PollutantCode] = B.[PollutantCode] and B.MeasureUnitUid = C.RowGuid and 
	PointId = {0} and A.PollutantCode in ({1})", pointId, "'a34004','a05024','a21005','a34002','a21026','a21004'");
                DataTable dt = g_DatabaseHelper.ExecuteDataTable(sql, "AMS_BaseDataConnection");
                string[] factors = dt.AsEnumerable().Select(t => t.Field<string>("PollutantCode")).ToArray();
                foreach (string f in factors)
                {
                    dtjson.Columns.Add(f + "_Name", typeof(string));
                    dtjson.Columns.Add(f + "_Value", typeof(string));
                    dtjson.Columns.Add(f + "_Unit", typeof(string));
                    dtjson.Columns.Add(f + "_IsExceeded", typeof(string));
                    dtjson.Columns.Add(f + "_ExcessiveUpper", typeof(string));
                    dtjson.Columns.Add(f + "_ExcessiveLow", typeof(string));
                }
                string sql2 = string.Format(@"select {2} from {0} where PointId ={1} and PollutantCode in ({5}) and Tstamp <= '{4}' and Tstamp >= '{3}'", "Air.TB_InfectantBy60", pointId, fieldName, dtstart, dtend, "'" + string.Join("','", factors) + "'");
                DataTable dt2 = g_DatabaseHelper.ExecuteDataTable(sql2, "AMS_AirAutoMonitorConnection");
                DataRow dr = dtjson.NewRow();
                dtjson.Rows.Add(dr);
                dtjson.Rows[0]["PointId"] = pointId;
                dtjson.Rows[0]["MonitoringPointName"] = dt.Rows[0]["MonitoringPointName"].ToString();
                dtjson.Rows[0]["DateTime"] = dt2.Rows.Count > 0 ? Convert.ToDateTime(dt2.Rows[0]["Tstamp"]).ToString("yyyy-MM-dd HH:00") : "--";
                DataView dv = dt.AsDataView();
                DataView dv2 = dt2.AsDataView();
                bool isOver = false;
                double alertUpper = 0;
                double alertLower = 0;
                string value = string.Empty;
                foreach (string f in factors)
                {
                    if (dv2.Count > 0)
                    {
                        dv.RowFilter = "PollutantCode = '" + f + "'";
                        dv2.RowFilter = "PollutantCode = '" + f + "'";
                        dtjson.Rows[0][f + "_Name"] = dv[0]["PollutantName"];
                        value = dv2[0]["PollutantValue"].ToString();
                        //dtjson.Rows[0][f + "_Value"] = dv2.Count > 0 ? dv2[0]["PollutantValue"].ToString() : "--";
                        if (f == "a21005")
                        {
                            if (value != "" && value != "--")
                                value = DecimalExtension.GetPollutantValue(Convert.ToDecimal(value), 1).ToString();
                        }
                        else
                        {
                            if (value != "" && value != "--")
                                value = Convert.ToDecimal(DecimalExtension.GetPollutantValue(Convert.ToDecimal(value), 3) * 1000).ToString("G0");
                        }
                        dtjson.Rows[0][f + "_Value"] = value;
                        dtjson.Rows[0][f + "_Unit"] = dv[0]["Unit"];
                        alertUpper = GetAirAlertUpper(f);
                        alertLower = GetAirAlertLower(f);
                        if (dv2.Count > 0 && value != "--" && value != "" && Convert.ToDouble(value) > alertUpper)
                            isOver = true;
                        else isOver = false;
                        dtjson.Rows[0][f + "_IsExceeded"] = isOver.ToString();
                        dtjson.Rows[0][f + "_ExcessiveUpper"] = alertUpper.ToString();
                        dtjson.Rows[0][f + "_ExcessiveLow"] = alertLower.ToString();
                    }
                }
                return dtjson;
            }
            else
            {
                string[] factors = new string[] { "a34004", "a34002", "a21004", "a21026", "a21005", "a05024" };
                string fac = string.Empty;
                string data = string.Empty;
                bool isOver = false;
                double alertUpper = 0;
                double alertLower = 0;
                DataTable dtjson = new DataTable();
                dtjson.Columns.Add("PointId", typeof(string));
                dtjson.Columns.Add("MonitoringPointName", typeof(string));
                dtjson.Columns.Add("DateTime", typeof(string));
                foreach (string f in factors)
                {
                    switch (f)
                    {
                        case "a34002":
                            fac = "PM10";
                            break;
                        case "a34004":
                            fac = "PM25";
                            break;
                        case "a21005":
                            fac = "CO";
                            break;
                        case "a21004":
                            fac = "NO2";
                            break;
                        case "a05024":
                            fac = "O3";
                            break;
                        case "a21026":
                            fac = "SO2";
                            break;
                    }
                    dtjson.Columns.Add(f + "_Name", typeof(string));
                    dtjson.Columns.Add(f + "_Value", typeof(string));
                    dtjson.Columns.Add(f + "_Unit", typeof(string));
                    dtjson.Columns.Add(f + "_IsExceeded", typeof(string));
                    dtjson.Columns.Add(f + "_ExcessiveUpper", typeof(string));
                    dtjson.Columns.Add(f + "_ExcessiveLow", typeof(string));
                }
                if (pointId == "0")
                {
                    return new DataTable();
                }
                else if (pointId == "1")
                {
                    DataRow dr = dtjson.NewRow();
                    dtjson.Rows.Add(dr);
                    foreach (string f in factors)
                    {
                        switch (f)
                        {
                            case "a34002":
                                fac = "PM10";
                                break;
                            case "a34004":
                                fac = "PM25";
                                break;
                            case "a21005":
                                fac = "CO";
                                break;
                            case "a21004":
                                fac = "NO2";
                                break;
                            case "a05024":
                                fac = "O3";
                                break;
                            case "a21026":
                                fac = "SO2";
                                break;
                        }
                        string url = string.Format("http://222.92.77.251:8083/MobileWs/AQI.asmx/Get24HoursFactorDataAir?portId={0}&factorName={1}", 0, fac);
                        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                        System.Net.WebResponse response = request.GetResponse();
                        System.IO.Stream respStream = response.GetResponseStream();
                        using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, Encoding.GetEncoding("UTF-8")))
                        {
                            data = reader.ReadToEnd();
                        }
                        DataView dv = data.JsonToDataSet().Tables[0].AsDataView();
                        if (dv.Count == 0)
                        {
                            dtjson.Rows[0][f + "_Value"] = "";
                            dtjson.Rows[0]["DateTime"] = "";
                            isOver = false;
                        }
                        else
                        {
                            dv.Sort = "DateTime desc";
                            dtjson.Rows[0]["DateTime"] = dv[0]["DateTime"].ToString();
                            dtjson.Rows[0][f + "_Value"] = dv[0]["value"].ToString();
                            if (dv[0]["value"].ToString() != "" && dv[0]["value"].ToString() != "--" && dv[0]["value"].ToString() != "--" && Convert.ToDouble(dv[0]["value"].ToString()) > alertUpper)
                                isOver = true;
                            else isOver = false;
                        }
                        
                        dtjson.Rows[0]["PointId"] = pointId;
                        dtjson.Rows[0]["MonitoringPointName"] = "苏州市";
                        
                        dtjson.Rows[0][f + "_Name"] = fac == "PM25" ? "PM2.5" : fac;
                        
                        dtjson.Rows[0][f + "_Unit"] = fac == "CO" ? "mg/m3" : "μg/m3";
                        alertUpper = GetAirAlertUpper(fac);
                        alertLower = GetAirAlertLower(fac);
                        
                        dtjson.Rows[0][f + "_IsExceeded"] = isOver.ToString();
                        dtjson.Rows[0][f + "_ExcessiveUpper"] = alertUpper.ToString();
                        dtjson.Rows[0][f + "_ExcessiveLow"] = alertLower.ToString();
                    }
                    return dtjson;
                }
                else if (pointId == "2")
                {
                    string url = string.Format("http://222.92.77.251:8083/V02WebServiceForOutSZ/WebServiceForDayData.asmx/GetZJGRealData");
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                    System.Net.WebResponse response = request.GetResponse();
                    System.IO.Stream respStream = response.GetResponseStream();
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, Encoding.GetEncoding("UTF-8")))
                    {
                        data = reader.ReadToEnd();
                    }
                    DataTable dt = data.JsonToDataSet().Tables[0];
                    DataView dv = dt.AsDataView();

                    //ServiceReference1.GISServiceSoapClient client = new ServiceReference1.GISServiceSoapClient();
                    //string zjgData = string.Empty;
                    if (dv.Count > 0)
                    {
                        DataRow dr = dtjson.NewRow();
                        dtjson.Rows.Add(dr);
                        foreach (string f in factors)
                        {
                            switch (f)
                            {
                                case "a34002":
                                    fac = "PM10";
                                    break;
                                case "a34004":
                                    fac = "PM2.5";
                                    break;
                                case "a21005":
                                    fac = "CO";
                                    break;
                                case "a21004":
                                    fac = "NO2";
                                    break;
                                case "a05024":
                                    fac = "O3";
                                    break;
                                case "a21026":
                                    fac = "SO2";
                                    break;
                            }
                            //zjgData = client.GetHourDataByZJG("1", fac).TrimStart('[').TrimEnd(']');
                            //DataTable dtt = zjgData.JsonToDataSet().Tables[0];
                            dtjson.Rows[0]["PointId"] = pointId;
                            dtjson.Rows[0]["MonitoringPointName"] = "张家港市";
                            dtjson.Rows[0]["DateTime"] = dt.Rows[0]["Tstamp"].ToString();
                            dtjson.Rows[0][f + "_Name"] = fac == "PM25" ? "PM2.5" : fac;
                            dv.RowFilter = "FactorName = '" + fac + "'";
                            string value = dv[0]["FactorValue"].ToString();
                            //if (f == "a21005")
                            //{
                            //    if (value != "" && value != "--")
                            //        value = DecimalExtension.GetPollutantValue(Convert.ToDecimal(value), 1).ToString();
                            //}
                            dtjson.Rows[0][f + "_Value"] = value;
                            dtjson.Rows[0][f + "_Unit"] = fac == "CO" ? "mg/m3" : "μg/m3";
                            alertUpper = GetAirAlertUpper(fac);
                            alertLower = GetAirAlertLower(fac);
                            if (value != "--" && Convert.ToDouble(value) > alertUpper)
                                isOver = true;
                            else isOver = false;
                            dtjson.Rows[0][f + "_IsExceeded"] = isOver.ToString();
                            dtjson.Rows[0][f + "_ExcessiveUpper"] = alertUpper.ToString();
                            dtjson.Rows[0][f + "_ExcessiveLow"] = alertLower.ToString();
                        }
                    }
                    return dtjson;
                }
                else if (pointId == "3")
                {
                    string tableName = "AMS_AirAutoMonitor.Air.TB_OriRegionHourAQI";

                    string sql = string.Format(@"select {0} PortName,
                                                Convert(nvarchar(16),dateadd(hh,0,[datetime]),21) DateTime,
                                                ISNULL(SO2_IAQI,'--') SO2_IAQI,
                                                ISNULL(NO2_IAQI,'--') NO2_IAQI,
                                                ISNULL(PM10_IAQI,'--') PM10_IAQI,
                                                ISNULL(CO_IAQI,'--') CO_IAQI,
                                                ISNULL(O3_IAQI,'--') O3_IAQI,
                                                ISNULL(Recent8HoursO3_IAQI,'--') Recent8HoursO3_IAQI,
                                                ISNULL([PM25_IAQI],'--') [PM2.5_IAQI],
                                                ISNULL(AQIValue,'--') AQI,
                                                ISNULL(PrimaryPollutant,'--') PrimaryPollutant,
                                                ISNULL(Class,'--') Class, 
                                                ISNULL(Grade,'--') Grade,
                                                ISNULL(RGBValue,'--') RGBValue,
                                                ISNULL(HealthEffect,'--') HealthEffect,
                                                ISNULL(TakeStep,'--') TakeStep ,
ISNULL(SO2,'--') SO2 ,
ISNULL(NO2,'--') NO2 ,
ISNULL(PM10,'--') PM10 ,
ISNULL(CO,'--') CO ,
ISNULL(O3,'--') O3 ,
ISNULL(PM25,'--') PM25
                                           from {1} AS v 
                                                {2} 
                                          where v.[dateTime] = (  
                                                                select MAX([dateTime])
                                                                  from {1} 
                                                                 where {3} [dateTime] > DATEADD(DAY, -7, GETDATE())
                                                                   and [dateTime] < GETDATE()
                                                                ) 
                                             {4}"
                                                      , "'3' AS PortId, '南通市' AS"
                                                      , tableName
                                                      , ""
                                                      , string.Format(" MonitoringRegionUid = '{0}' and ", monitorRegionUid)
                                                      , string.Format("and v.MonitoringRegionUid = '{0}'", monitorRegionUid)
                                                      );
                    DataTable dt = g_DatabaseHelper.ExecuteDataTable(sql, "AMS_AirAutoMonitorConnection");



                    DataRow dr = dtjson.NewRow();
                    dtjson.Rows.Add(dr);
                    foreach (string f in factors)
                    {
                        switch (f)
                        {
                            case "a34002":
                                fac = "PM10";
                                break;
                            case "a34004":
                                fac = "PM25";
                                break;
                            case "a21005":
                                fac = "CO";
                                break;
                            case "a21004":
                                fac = "NO2";
                                break;
                            case "a05024":
                                fac = "O3";
                                break;
                            case "a21026":
                                fac = "SO2";
                                break;
                        }
                        dtjson.Rows[0]["PointId"] = pointId;
                        dtjson.Rows[0]["MonitoringPointName"] = "南通市";
                        dtjson.Rows[0]["DateTime"] = Convert.ToDateTime(dt.Rows[0]["DateTime"].ToString()).ToString("yyyy-MM-dd HH:00");
                        dtjson.Rows[0][f + "_Name"] = fac == "PM25" ? "PM2.5" : fac;
                        string value = dt.Rows[0][fac].ToString();
                        if (value != "" && value != "--")
                        {
                            if (f == "a21005")
                                value = DecimalExtension.GetPollutantValue(Convert.ToDecimal(value), 1).ToString();
                            else
                                value = Convert.ToDecimal(DecimalExtension.GetPollutantValue(Convert.ToDecimal(value), 3) * 1000).ToString("G0");
                        }
                        dtjson.Rows[0][f + "_Value"] = value;
                        dtjson.Rows[0][f + "_Unit"] = fac == "CO" ? "mg/m3" : "μg/m3";
                        alertUpper = GetAirAlertUpper(fac);
                        alertLower = GetAirAlertLower(fac);
                        if (dt.Rows[0][fac].ToString() != "" && dt.Rows[0][fac].ToString() != "--" && Convert.ToDouble(dt.Rows[0][fac].ToString()) > alertUpper)
                            isOver = true;
                        else isOver = false;
                        dtjson.Rows[0][f + "_IsExceeded"] = isOver.ToString();
                        dtjson.Rows[0][f + "_ExcessiveUpper"] = alertUpper.ToString();
                        dtjson.Rows[0][f + "_ExcessiveLow"] = alertLower.ToString();
                    }
                    return dtjson;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 获取数据给日浓度
        /// </summary>
        /// <returns></returns>
        public DataTable GetDataForDay(string type, string pointId)
        {
            if (type == "3")
            {
                DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();
                DataTable dt = new DataTable();
                DateTime dtstart = Convert.ToDateTime((DateTime.Now.ToString("yyyy-MM-dd 00:00:00"))).AddDays(-1);
                DateTime dtend = Convert.ToDateTime((DateTime.Now.ToString("yyyy-MM-dd 00:00:00")));
                string sql = string.Format(@"SELECT A.[PointId]
		,B.MonitoringPointName
      ,convert(nvarchar(10),[DateTime],21) [DateTime]
      ,[SO2]
      ,[NO2]
      ,[PM10]
      ,[CO]
      ,[Max8HourO3]
      ,[PM25]
  FROM [AMS_AirAutoMonitor].[Air].[TB_OriDayAQI] A inner join [dbo].[SY_MonitoringPoint] B
  on A.PointId = B.PointId
  where datetime >='{0}' and datetime <'{1}'
  order by PointId", dtstart, dtend);
                DataView dtdata = g_DatabaseHelper.ExecuteDataView(sql, "AMS_AirAutoMonitorConnection");
                string[] factors = new string[] { "a34004", "a34002", "a21004", "a21026", "a21005", "a05024" };
                dt.Columns.Add("PointId", typeof(string));
                dt.Columns.Add("MonitoringPointName", typeof(string));
                dt.Columns.Add("DateTime", typeof(string));
                foreach (string f in factors)
                {
                    dt.Columns.Add(f + "_Name", typeof(string));
                    dt.Columns.Add(f + "_Value", typeof(string));
                    dt.Columns.Add(f + "_Unit", typeof(string));
                    dt.Columns.Add(f + "_IsExceeded", typeof(string));
                    dt.Columns.Add(f + "_ExcessiveUpper", typeof(string));
                    dt.Columns.Add(f + "_ExcessiveLow", typeof(string));
                }
                bool isOver = false;
                double alertUpper = 0;
                double alertLower = 0;
                string fac = string.Empty;
                for (int i = 0; i < dtdata.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["PointId"] = dtdata[i]["PointId"];
                    dr["MonitoringPointName"] = dtdata[i]["MonitoringPointName"];
                    dr["DateTime"] = dtdata[i]["DateTime"];
                    foreach (string f in factors)
                    {
                        switch (f)
                        {
                            case "a34002":
                                fac = "PM10";
                                break;
                            case "a34004":
                                fac = "PM25";
                                break;
                            case "a21005":
                                fac = "CO";
                                break;
                            case "a21004":
                                fac = "NO2";
                                break;
                            case "a05024":
                                fac = "Max8HourO3";
                                break;
                            case "a21026":
                                fac = "SO2";
                                break;
                        }
                        dr[f + "_Name"] = fac == "Max8HourO3" ? "O3" : fac;
                        string value = dtdata[i][fac].ToString();
                        if (value != "")
                        {
                            if (f == "a21005")
                                value = DecimalExtension.GetPollutantValue(Convert.ToDecimal(value), 1).ToString();
                            else
                                value = Convert.ToDecimal(DecimalExtension.GetPollutantValue(Convert.ToDecimal(value), 3) * 1000).ToString("G0");
                        }
                        dr[f + "_Value"] = value != "" ? value : "--";
                        dr[f + "_Unit"] = f == "a21005" ? "mg/m3" : "μg/m3";
                        alertUpper = GetAirAlertUpper(fac == "Max8HourO3" ? "O3" : fac);
                        alertLower = GetAirAlertLower(fac == "Max8HourO3" ? "O3" : fac);
                        if (value != "" && Convert.ToDouble(value) > alertUpper)
                            isOver = true;
                        else isOver = false;
                        dr[f + "_IsExceeded"] = isOver.ToString();
                        dr[f + "_ExcessiveUpper"] = alertUpper.ToString();
                        dr[f + "_ExcessiveLow"] = alertLower.ToString();
                    }
                    dt.Rows.Add(dr);
                }
                return dt;



                //                string fieldName = "DateTime,PointId,PollutantCode,PollutantValue";
                //                DataTable dtjson = new DataTable();
                //                dtjson.Columns.Add("PointId", typeof(string));
                //                dtjson.Columns.Add("MonitoringPointName", typeof(string));
                //                dtjson.Columns.Add("DateTime", typeof(string));

                //                string sql = string.Format(@"SELECT TOP 1000 
                //      [PointId]
                //      ,[MonitoringPointName]
                //      ,A.[PollutantCode]
                //      ,A.[PollutantName]
                //      ,ItemText Unit
                //  FROM [AMS_BaseData].[dbo].[V_Point_InstrumentChannels] A,[AMS_BaseData].[Standard].[TB_PollutantCode] B,[EQMS_Framework].[dbo].[TB_Frame_CodeItem] C
                //  where 
                //  InstrumentName in('常规参数','气象五参数分析仪','超级站常规参数') and A.[PollutantCode] = B.[PollutantCode] and B.MeasureUnitUid = C.RowGuid and 
                //	PointId = {0} and A.PollutantCode in ({1})", pointId, "'a34004','a05024','a21005','a34002','a21026','a21004'");
                //                DataTable dt = g_DatabaseHelper.ExecuteDataTable(sql, "AMS_BaseDataConnection");
                //                string[] factors = dt.AsEnumerable().Select(t => t.Field<string>("PollutantCode")).ToArray();
                //                foreach (string f in factors)
                //                {
                //                    dtjson.Columns.Add(f + "_Name", typeof(string));
                //                    dtjson.Columns.Add(f + "_Value", typeof(string));
                //                    dtjson.Columns.Add(f + "_Unit", typeof(string));
                //                    dtjson.Columns.Add(f + "_IsExceeded", typeof(string));
                //                    dtjson.Columns.Add(f + "_ExcessiveUpper", typeof(string));
                //                    dtjson.Columns.Add(f + "_ExcessiveLow", typeof(string));
                //                }
                //                string sql2 = string.Format(@"select {2} from {0} where PointId ={1} and PollutantCode in ({5}) and DateTime < '{4}' and DateTime >= '{3}'", "[AMS_AirAutoMonitor].[Air].[TB_InfectantByDay]", pointId, fieldName, dtstart, dtend, "'" + string.Join("','", factors) + "'");
                //                DataTable dt2 = g_DatabaseHelper.ExecuteDataTable(sql2, "AMS_AirAutoMonitorConnection");
                //                DataRow dr = dtjson.NewRow();
                //                dtjson.Rows.Add(dr);
                //                dtjson.Rows[0]["PointId"] = pointId;
                //                dtjson.Rows[0]["MonitoringPointName"] = dt.Rows[0]["MonitoringPointName"].ToString();
                //                dtjson.Rows[0]["DateTime"] = dt2.Rows.Count > 0 ? Convert.ToDateTime(dt2.Rows[0]["DateTime"]).ToString("yyyy-MM-dd") : "--";
                //                DataView dv = dt.AsDataView();
                //                DataView dv2 = dt2.AsDataView();
                //                bool isOver = false;
                //                double alertUpper = 0;
                //                double alertLower = 0;
                //                foreach (string f in factors)
                //                {
                //                    if (dv2.Count > 0)
                //                    {
                //                        dv.RowFilter = "PollutantCode = '" + f + "'";
                //                        dv2.RowFilter = "PollutantCode = '" + f + "'";
                //                        dtjson.Rows[0][f + "_Name"] = dv[0]["PollutantName"];
                //                        string value = dv2[0]["PollutantValue"].ToString();
                //                        if (value != "")
                //                        {
                //                            if (f == "a21005")
                //                                value = DecimalExtension.GetPollutantValue(Convert.ToDecimal(value), 1).ToString();
                //                            else
                //                                value = Convert.ToDecimal(DecimalExtension.GetPollutantValue(Convert.ToDecimal(value), 3) * 1000).ToString("G0");
                //                        }
                //                        dtjson.Rows[0][f + "_Value"] = value != "" ? value : "--";
                //                        dtjson.Rows[0][f + "_Unit"] = dv[0]["Unit"];
                //                        alertUpper = GetAirAlertUpper(f);
                //                        alertLower = GetAirAlertLower(f);
                //                        if (value != "" && Convert.ToDouble(value) > alertUpper)
                //                            isOver = true;
                //                        else isOver = false;
                //                        dtjson.Rows[0][f + "_IsExceeded"] = isOver.ToString();
                //                        dtjson.Rows[0][f + "_ExcessiveUpper"] = alertUpper.ToString();
                //                        dtjson.Rows[0][f + "_ExcessiveLow"] = alertLower.ToString();
                //                    }
                //                }
                //                return dtjson;
            }
            else
            {
                DataTable dt = new DataTable();
                //上海
                if (pointId == "0")
                {
                    string[] factors = new string[] { "a34004", "a34002", "a21004", "a21026", "a21005", "a05024" };
                    dt.Columns.Add("PointId", typeof(string));
                    dt.Columns.Add("MonitoringPointName", typeof(string));
                    dt.Columns.Add("DateTime", typeof(string));
                    foreach (string f in factors)
                    {
                        dt.Columns.Add(f + "_Name", typeof(string));
                        dt.Columns.Add(f + "_Value", typeof(string));
                        dt.Columns.Add(f + "_Unit", typeof(string));
                        dt.Columns.Add(f + "_IsExceeded", typeof(string));
                        dt.Columns.Add(f + "_ExcessiveUpper", typeof(string));
                        dt.Columns.Add(f + "_ExcessiveLow", typeof(string));
                    }
                    DataView data = SHdayValue().AsDataView();
                    if (data.Count > 0)
                    {
                        DataRow dr = dt.NewRow();
                        dr["PointId"] = pointId;
                        dr["MonitoringPointName"] = "上海市";
                        dr["DateTime"] = Convert.ToDateTime(data[0]["Tstamp"].ToString()).ToString("yyyy-MM-dd");
                        string factorname = string.Empty;
                        string value = string.Empty;
                        bool isOver = false;
                        double alertUpper = 0;
                        double alertLower = 0;
                        foreach (string f in factors)
                        {
                            data.RowFilter = "PollutantCode = '" + f + "'";
                            factorname = data[0]["factorName"].ToString();
                            value = data[0]["PollutantValue"].ToString();
                            dr[f + "_Name"] = factorname;
                            dr[f + "_Value"] = value;
                            dr[f + "_Unit"] = data[0]["factorUnit"].ToString();
                            alertUpper = GetAirAlertUpper(factorname == "PM2.5" ? "PM25" : factorname);
                            alertLower = GetAirAlertLower(factorname == "PM2.5" ? "PM25" : factorname);
                            if (value != "" && value != "--" && Convert.ToDouble(value) > alertUpper)
                                isOver = true;
                            else isOver = false;
                            dr[f + "_IsExceeded"] = isOver.ToString();
                            dr[f + "_ExcessiveUpper"] = alertUpper.ToString();
                            dr[f + "_ExcessiveLow"] = alertLower.ToString();
                        }
                        dt.Rows.Add(dr);
                    }
                }
                //苏州或张家港
                else if (pointId == "1" || pointId == "2")
                {
                    string cityUid = pointId == "1" ? "7e05b94c-bbd4-45c3-919c-42da2e63fd43" : "22";
                    string data = string.Empty;
                    string url = string.Format("http://222.92.77.251:8083/V02WebServiceForOutSZ/WebServiceForDayData.asmx/GetLastDayDataByPortIdForArount?MonitoringRegionUid={0}", cityUid);
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                    System.Net.WebResponse response = request.GetResponse();
                    System.IO.Stream respStream = response.GetResponseStream();
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, Encoding.GetEncoding("UTF-8")))
                    {
                        data = "{\"DayData\":" + reader.ReadToEnd() + "}";
                    }
                    DataTable dt2 = data.JsonToDataSet().Tables[0];
                    string[] factors = new string[] { "a34004", "a34002", "a21004", "a21026", "a21005", "a05024" };
                    dt.Columns.Add("PointId", typeof(string));
                    dt.Columns.Add("MonitoringPointName", typeof(string));
                    dt.Columns.Add("DateTime", typeof(string));
                    foreach (string f in factors)
                    {
                        dt.Columns.Add(f + "_Name", typeof(string));
                        dt.Columns.Add(f + "_Value", typeof(string));
                        dt.Columns.Add(f + "_Unit", typeof(string));
                        dt.Columns.Add(f + "_IsExceeded", typeof(string));
                        dt.Columns.Add(f + "_ExcessiveUpper", typeof(string));
                        dt.Columns.Add(f + "_ExcessiveLow", typeof(string));
                    }
                    DataRow dr = dt.NewRow();
                    dr["PointId"] = pointId;
                    dr["MonitoringPointName"] = pointId == "1" ? "苏州市" : "张家港市";
                    dr["DateTime"] = pointId == "1" ? Convert.ToDateTime(dt2.Rows[0]["ReportDateTime"].ToString()).ToString("yyyy-MM-dd") : Convert.ToDateTime(dt2.Rows[0]["DateTime"].ToString()).ToString("yyyy-MM-dd");
                    bool isOver = false;
                    double alertUpper = 0;
                    double alertLower = 0;
                    string fac = string.Empty;
                    foreach (string f in factors)
                    {
                        switch (f)
                        {
                            case "a34002":
                                fac = "PM10";
                                break;
                            case "a34004":
                                fac = "PM25";
                                break;
                            case "a21005":
                                fac = "CO";
                                break;
                            case "a21004":
                                fac = "NO2";
                                break;
                            case "a05024":
                                fac = "Max8HourO3";
                                break;
                            case "a21026":
                                fac = "SO2";
                                break;
                        }
                        if (dt2.Rows.Count > 0)
                        {
                            dr[f + "_Name"] = fac == "Max8HourO3" ? "O3" : fac;
                            string value = dt2.Rows[0][fac].ToString();
                            if (value != "")
                            {
                                if (f == "a21005")
                                    value = DecimalExtension.GetPollutantValue(Convert.ToDecimal(value), 1).ToString();
                                else
                                    value = Convert.ToDecimal(DecimalExtension.GetPollutantValue(Convert.ToDecimal(value), 3) * 1000).ToString("G0");
                            }
                            dr[f + "_Value"] = value != "" ? value : "--";
                            dr[f + "_Unit"] = f == "a21005" ? "mg/m3" : "μg/m3";
                            alertUpper = GetAirAlertUpper(fac == "Max8HourO3" ? "O3" : fac);
                            alertLower = GetAirAlertLower(fac == "Max8HourO3" ? "O3" : fac);
                            if (value != "" && Convert.ToDouble(value) > alertUpper)
                                isOver = true;
                            else isOver = false;
                            dr[f + "_IsExceeded"] = isOver.ToString();
                            dr[f + "_ExcessiveUpper"] = alertUpper.ToString();
                            dr[f + "_ExcessiveLow"] = alertLower.ToString();
                        }
                    }
                    dt.Rows.Add(dr);

                }
                //南通日数据
                else
                {
                    string[] factors = new string[] { "a34004", "a34002", "a21004", "a21026", "a21005", "a05024" };
                    dt.Columns.Add("PointId", typeof(string));
                    dt.Columns.Add("MonitoringPointName", typeof(string));
                    dt.Columns.Add("DateTime", typeof(string));
                    foreach (string f in factors)
                    {
                        dt.Columns.Add(f + "_Name", typeof(string));
                        dt.Columns.Add(f + "_Value", typeof(string));
                        dt.Columns.Add(f + "_Unit", typeof(string));
                        dt.Columns.Add(f + "_IsExceeded", typeof(string));
                        dt.Columns.Add(f + "_ExcessiveUpper", typeof(string));
                        dt.Columns.Add(f + "_ExcessiveLow", typeof(string));
                    }
                    DataRow dr = dt.NewRow();
                    string dt1 = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 00:00:00");
                    string dt2 = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                    string sql = string.Format(@"select '3' PointId
		,convert(nvarchar(10),[ReportDateTime],21) DateTime
      ,[SO2]
      ,[SO2_IAQI]
      ,[NO2]
      ,[NO2_IAQI]
      ,[PM10]
      ,[PM10_IAQI]
      ,[CO]
      ,[CO_IAQI]
      ,[MaxOneHourO3]
      ,[MaxOneHourO3_IAQI]
      ,[Max8HourO3]
      ,[Max8HourO3_IAQI]
      ,[PM25]
      ,[PM25_IAQI]
      ,[AQIValue]
      ,[PrimaryPollutant]
      ,[Range]
      ,[RGBValue]
      ,[Class]
      ,[Grade]
      ,[HealthEffect]
      ,[TakeStep] from [AMS_AirAutoMonitor].[Air].[TB_OriRegionDayAQIReport] where MonitoringRegionUid = '{0}' and ReportDateTime >= '{1}' and ReportDateTime < '{2}'", monitorRegionUid, dt1, dt2);
                    DataTable dtt = g_DatabaseHelper.ExecuteDataTable(sql, "AMS_AirAutoMonitorConnection");
                    if (dtt.Rows.Count > 0)
                    {
                        dr["PointId"] = "3";
                        dr["MonitoringPointName"] = "南通市";
                        dr["DateTime"] = dtt.Rows[0]["DateTime"].ToString();
                        bool isOver = false;
                        double alertUpper = 0;
                        double alertLower = 0;
                        string fac = string.Empty;
                        foreach (string f in factors)
                        {
                            switch (f)
                            {
                                case "a34002":
                                    fac = "PM10";
                                    break;
                                case "a34004":
                                    fac = "PM25";
                                    break;
                                case "a21005":
                                    fac = "CO";
                                    break;
                                case "a21004":
                                    fac = "NO2";
                                    break;
                                case "a05024":
                                    fac = "Max8HourO3";
                                    break;
                                case "a21026":
                                    fac = "SO2";
                                    break;
                            }
                            if (dtt.Rows.Count > 0)
                            {
                                dr[f + "_Name"] = fac == "Max8HourO3" ? "O3" : fac;
                                string value = dtt.Rows[0][fac].ToString();
                                if (value != "")
                                {
                                    if (f == "a21005")
                                        value = DecimalExtension.GetPollutantValue(Convert.ToDecimal(value), 1).ToString();
                                    else
                                        value = Convert.ToDecimal(DecimalExtension.GetPollutantValue(Convert.ToDecimal(value), 3) * 1000).ToString("G0");
                                }
                                dr[f + "_Value"] = value != "" ? value : "--";
                                dr[f + "_Unit"] = f == "a21005" ? "mg/m3" : "μg/m3";
                                alertUpper = GetAirAlertUpper(fac == "Max8HourO3" ? "O3" : fac);
                                alertLower = GetAirAlertLower(fac == "Max8HourO3" ? "O3" : fac);
                                if (value != "" && Convert.ToDouble(value) > alertUpper)
                                    isOver = true;
                                else isOver = false;
                                dr[f + "_IsExceeded"] = isOver.ToString();
                                dr[f + "_ExcessiveUpper"] = alertUpper.ToString();
                                dr[f + "_ExcessiveLow"] = alertLower.ToString();
                            }
                        }
                        dt.Rows.Add(dr);
                    }
                }
                return dt;
            }
        }

        /// <summary>
        /// 获取日浓度
        /// </summary>
        /// <returns>
        /// </returns>
        public string GetLastDayDataByType(string pointId, string AreaGuid)
        {
            if (pointId == "3")
            {
                //DataTable dt = new DataTable();
                //string[] arr = new string[] { "186", "187", "188", "189", "190", "198", "191", "194", "202", "201", "193", "200", "192", "196", "195", "203", "197", "199", "204", "206", "205" };

                //foreach (string pid in arr)
                //{
                //    DataTable dtt = GetDataForDay(pointId, pid);
                //    dt.Merge(dtt);
                //}
                DataTable dt = GetDataForDay(pointId, "");
                string pids = GetPointIdByRegion(AreaGuid);
                DataView dvs = dt.AsDataView();
                dvs.RowFilter = "PointId in (" + pids + ")";
                StringBuilder sb = new StringBuilder();
                sb.Append("[{\"HistoryDayData\":");
                string json = dvs.ToTable().ToJsonBySerialize();
                sb.Append(json + "}]");
                return sb.ToString();
            }
            else
            {
                DataTable dt = new DataTable();
                if (pointId == "")
                {
                    string[] pointIdArr = "0;1;2;4".Split(';');
                    foreach (string pid in pointIdArr)
                    {
                        DataTable dtt = GetDataForDay("0", pid);
                        dt.Merge(dtt);
                    }
                }
                else
                {
                    dt = GetDataForDay("0", pointId);
                }

                StringBuilder sb = new StringBuilder();
                sb.Append("[{\"HistoryDayData\":");
                string json = dt.ToJsonBySerialize();
                sb.Append(json + "}]");
                return sb.ToString();
            }
        }

        /// <summary>
        /// 获取周边城市的日AQI
        /// </summary>
        /// <returns>
        /// </returns>
        public string GetDayAQIForArount(string pointId, string[] arrType, DateTime dt1, DateTime dt2, string order)
        {
            DataTable dt = new DataTable();
            if (pointId != "")
            {
                dt = GetDataForDayAQI(pointId, arrType, dt1, dt2);
                DataView dv = dt.AsDataView();
                dv.Sort = order == "1" ? "AQIValue asc" : "AQIValue desc";
                string[] arr = GetClassByNum(arrType);
                string strType = "'" + string.Join("','", arr) + "'";
                dv.RowFilter = "Class in (" + strType + ")";
                StringBuilder sb = new StringBuilder();
                sb.Append("[{\"HistoryDayData\":");
                string json = dv.ToTable().ToJsonBySerialize();
                sb.Append(json + "}]");
                return sb.ToString();
            }
            else
            {
                string[] arr = new string[] { "1", "2", "3", "0" };
                foreach (string pid in arr)
                {
                    DataTable dtt = GetDataForDayAQI(pid, arrType, dt1, dt2);
                    dt.Merge(dtt);
                }
                DataView dv = dt.AsDataView();
                dv.Sort = order == "1" ? "AQIValue asc" : "AQIValue desc";
                string[] arrT = GetClassByNum(arrType);
                string strType = "'" + string.Join("','", arrT) + "'";
                dv.RowFilter = "Class in (" + strType + ")";
                StringBuilder sb = new StringBuilder();
                sb.Append("[{\"HistoryDayData\":");
                string json = dv.ToTable().ToJsonBySerialize();
                sb.Append(json + "}]");
                return sb.ToString();
            }
        }

        /// <summary>
        /// 为日AQI获取数据
        /// </summary>
        /// <param name="portType"></param>
        /// <returns></returns>
        public DataTable GetDataForDayAQI(string pointId, string[] arrType, DateTime dt1, DateTime dt2)
        {
            if (pointId == "0")
            {
                DataTable dt = SHdayTimeAQI();
                return dt;
            }
            if (pointId == "1")
            {
                string data = string.Empty;
                string url = string.Format("http://222.92.77.251:8083/MobileWs/AQI.asmx/GetDayAQIByDatetime?portId={0}&factorName={1}&startDay={1}&endDay={2}", 0, dt1.AddDays(-1), dt2);
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                System.Net.WebResponse response = request.GetResponse();
                System.IO.Stream respStream = response.GetResponseStream();
                using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, Encoding.GetEncoding("UTF-8")))
                {
                    data = reader.ReadToEnd();
                }
                DataTable dt = data.JsonToDataSet().Tables[0];
                dt.Columns.Add("PointId", typeof(string));
                dt.Columns.Add("PortName", typeof(string));
                foreach (DataRow dr in dt.Rows)
                {
                    dr["PointId"] = pointId;
                    dr["PortName"] = "苏州市";
                }
                foreach (DataColumn dc in dt.Columns)
                {
                    if (dc.ColumnName.Equals("AQI"))
                    {
                        dc.ColumnName = "AQIValue";
                    }
                }
                return dt;
            }
            if (pointId == "2")//张家港
            {
                DataTable dt = new DataTable();
                ServiceReference1.GISServiceSoapClient client = new ServiceReference1.GISServiceSoapClient();
                string zjgData = client.GetPortALLDetailZJG("day", "2").TrimStart('[').TrimEnd(']');//张家港监测站：1，张家港城北小学：2
                DataTable dtt = zjgData.JsonToDataSet().Tables[0];
                dt.Columns.Add("DateTime", typeof(string));
                dt.Columns.Add("AQIValue", typeof(string));
                dt.Columns.Add("PrimaryPollutant", typeof(string));
                dt.Columns.Add("RGBValue", typeof(string));
                dt.Columns.Add("Class", typeof(string));
                dt.Columns.Add("Grade", typeof(string));
                dt.Columns.Add("HealthEffect", typeof(string));
                dt.Columns.Add("TakeStep", typeof(string));
                dt.Columns.Add("PointId", typeof(string));
                dt.Columns.Add("PortName", typeof(string));
                DataRow dr = dt.NewRow();
                dr["DateTime"] = dtt.Rows[0]["DateTime"].ToString();
                dr["AQIValue"] = dtt.Rows[0]["AQI"].ToString();
                dr["PrimaryPollutant"] = dtt.Rows[0]["PrimaryPollutant"].ToString().Equals("O3-8") ? "O3" : dtt.Rows[0]["PrimaryPollutant"].ToString();
                dr["RGBValue"] = dtt.Rows[0]["RGBValue"].ToString();
                dr["Class"] = dtt.Rows[0]["Class"].ToString();
                dr["Grade"] = dtt.Rows[0]["Grade"].ToString();
                dr["HealthEffect"] = dtt.Rows[0]["HealthEffect"].ToString();
                dr["TakeStep"] = dtt.Rows[0]["TakeStep"].ToString();
                dr["PointId"] = pointId;
                dr["PortName"] = "张家港市";
                dt.Rows.Add(dr);
                return dt;
            }
            if (pointId == "3")
            {
                string tableName = "[AMS_MonitorBusiness].[AirReport].[TB_RegionDayAQIReport]";

                string sql = string.Format(@" 
                                        SELECT Convert(nvarchar(10),{0},21) as DateTime,
                                               AQIValue AQIValue,
                                               PrimaryPollutant,
                                               RGBValue,
                                               Class,
                                               Grade,
                                               HealthEffect,
                                               TakeStep 
                                         FROM {1}
                                        WHERE {0} >= '{2}' 
                                          and {0} <= '{3}' 
                                          {4} 
                                        order by {0}"
                                           , "ReportDateTime"
                                           , tableName
                                           , dt1
                                           , dt2
                                           , string.Format(" and MonitoringRegionUid = '{0}'", monitorRegionUid));

                DataTable dt = g_DatabaseHelper.ExecuteDataTable(sql, "AMS_MonitoringBusinessConnection");
                dt.Columns.Add("PointId", typeof(string));
                dt.Columns.Add("PortName", typeof(string));
                foreach (DataRow dr in dt.Rows)
                {
                    dr["PointId"] = pointId;
                    dr["PortName"] = "南通市";
                }
                return dt;
            }
            return null;
        }

        /// <summary>
        /// 获取日数据信息
        /// </summary>
        /// <returns>
        /// </returns>
        public string GetLastDayDataByPortIdForArount(string pointId)
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            sb.Append("[{\"HistoryDayData\":");
            string json = dt.ToJsonBySerialize();
            sb.Append(json + "}]");
            return sb.ToString();
        }

        /// <summary>
        /// 获取周边城市的实时AQI
        /// </summary>
        /// <returns>
        /// </returns>
        public string GetHourAQIForArount(string pointId, string order)
        {
            string data = string.Empty;
            string data2 = string.Empty;
            DataTable dtresult = new DataTable();
            if (pointId == "")
            {
                string[] idArr = "1;2;0".Split(';');
                foreach (string id in idArr)
                {
                    DataTable dt = GetDataForArount(id);
                    dtresult.Merge(dt);
                }
                DataView dv = dtresult.AsDataView();
                dv.Sort = order == "1" ? "AQIValue asc" : "AQIValue desc";
                StringBuilder sb = new StringBuilder();
                sb.Append("[{\"PortIAQIBySiteType\":");
                string json = dv.ToTable().ToJsonBySerialize().TrimEnd(']');
                sb.Append(json + "," + GetAQIByPointIdNew("ALLR", new string[] { "ALL" }, "").Substring(24, GetAQIByPointIdNew("ALLR", new string[] { "ALL" }, "").Length - 24));
                return sb.ToString();
                
            }
            else
            {
                if (pointId == "3")
                {
                    return GetAQIByPointId("ALLR",new string[]{"ALL"},"");
                }
                else
                {
                    if (pointId == "2" || pointId == "1" || pointId == "0")
                    {
                        DataTable dt = GetDataForArount(pointId);
                        DataView dv = dt.AsDataView();
                        dv.Sort = order == "1" ? "AQIValue asc" : "AQIValue desc";
                        StringBuilder sb = new StringBuilder();
                        sb.Append("[{\"PortIAQIBySiteType\":");
                        string json = dv.ToTable().ToJsonBySerialize();
                        sb.Append(json + "}]");
                        return sb.ToString();
                    }
                    else
                    {
                        return GetAQIByPointId("ALLR", pointId.Split(','), "");
                    }
                    
                }
                
                
            }
            return null;
        }

        public DataTable GetDataForArount(string id)
        {
            string data = string.Empty;
            string data2 = string.Empty;
            if (id == "0")
            {
                DataTable dt = SHrealTimeAQI();
                return dt;
            }
            if (id == "1")
            {
                string url2 = string.Format("http://222.92.77.251:8083/MobileWs/AQI.asmx/GetLatestHourAQI?portId={0}", 0);
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url2);
                System.Net.WebResponse response = request.GetResponse();
                System.IO.Stream respStream = response.GetResponseStream();
                using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, Encoding.GetEncoding("UTF-8")))
                {
                    data2 = reader.ReadToEnd();
                }
                DataTable dt2 = data2.JsonToDataSet().Tables[0];
                dt2.Rows[0]["PortId"] = id;
                string[] factors = new string[] { "SO2", "NO2", "PM10", "CO", "O3", "PM2.5" };
                foreach (string f in factors)
                {
                    dt2.Columns.Add(f, typeof(string));
                }
                string dtime = dt2.Rows[0]["DateTime"].ToString();
                foreach (string f in factors)
                {
                    url2 = string.Format("http://222.92.77.251:8083/MobileWs/AQI.asmx/Get24HoursFactorDataAir?portId={0}&factorName={1}", 0, f == "PM2.5" ? "PM25" : f);
                    request = (HttpWebRequest)HttpWebRequest.Create(url2);
                    response = request.GetResponse();
                    respStream = response.GetResponseStream();
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, Encoding.GetEncoding("UTF-8")))
                    {
                        data = reader.ReadToEnd();
                    }
                    DataView dv = data.JsonToDataSet().Tables[0].AsDataView();
                    string value = string.Empty;
                    if (dv.Count == 0)
                    {
                        value="";
                    }
                    else
                    {
                        dv.RowFilter = "DateTime = '" + dtime + "'";
                        value = dv[0]["value"].ToString();
                    }
                    
                    
                    dt2.Rows[0][f] = value;
                }
                foreach (DataColumn dc in dt2.Columns)
                {
                    if (dc.ColumnName.Equals("AQI"))
                    {
                        dc.ColumnName = "AQIValue";
                    }
                }
                return dt2;
            }
            if (id == "2")
            {
                string url = string.Format("http://218.4.71.46:8000/V02ZJG.AQIWS/ZhangjgAQI.asmx/GetLatestHourAQINew?portId={0}", 2);//张家港区域改成城北小学站点
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                System.Net.WebResponse response = request.GetResponse();
                System.IO.Stream respStream = response.GetResponseStream();
                using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, Encoding.GetEncoding("UTF-8")))
                {
                    data2 = reader.ReadToEnd();
                }
                DataTable dt2 = data2.JsonToDataSet().Tables[0];
                if (dt2.Rows.Count > 0)
                {
                    dt2.Rows[0]["PortId"] = id;
                    dt2.Rows[0]["PortName"] = "张家港市";
                    //string[] factors = new string[] { "SO2", "NO2", "PM10", "CO", "O3", "PM2.5" };
                    //foreach (string f in factors)
                    //{
                    //    dt2.Columns.Add(f, typeof(string));
                    //}
                    //string dtime = dt2.Rows[0]["DateTime"].ToString();
                    //string fac = string.Empty;
                    //string zjgData = string.Empty;
                    //ServiceReference1.GISServiceSoapClient client = new ServiceReference1.GISServiceSoapClient();
                    //foreach (string f in factors)
                    //{
                    //    switch (f)
                    //    {
                    //        case "PM10":
                    //            fac = "PM10_1";
                    //            break;
                    //        case "PM2.5":
                    //            fac = "PM25_1";
                    //            break;
                    //        case "CO":
                    //            fac = "CO";
                    //            break;
                    //        case "NO2":
                    //            fac = "NO2";
                    //            break;
                    //        case "O3":
                    //            fac = "O3_1";
                    //            break;
                    //        case "SO2":
                    //            fac = "SO2";
                    //            break;
                    //    }
                    //    //优化速度，此处给空值
                    //    zjgData = client.GetHourDataByZJG("1", fac).TrimStart('[').TrimEnd(']');
                    //    DataTable dtt = zjgData.JsonToDataSet().Tables[0];
                    //    dt2.Rows[0][f] = dtt.Rows[0]["FactorValue"];
                    //    dt2.Rows[0][f] = "";
                    //}
                    foreach (DataColumn dc in dt2.Columns)
                    {
                        if (dc.ColumnName.Equals("AQI"))
                        {
                            dc.ColumnName = "AQIValue";
                        }
                    }
                }
                return dt2;
            }
            if (id == "3")
            {
                string tableName = "AMS_AirAutoMonitor.Air.TB_OriRegionHourAQI";

                string sql = string.Format(@"select {0} PortName,
                                                Convert(nvarchar(13),dateadd(hh,0,[datetime]),21)+':00' DateTime,
                                                ISNULL(SO2_IAQI,'--') SO2_IAQI,
                                                ISNULL(NO2_IAQI,'--') NO2_IAQI,
                                                ISNULL(PM10_IAQI,'--') PM10_IAQI,
                                                ISNULL(CO_IAQI,'--') CO_IAQI,
                                                ISNULL(O3_IAQI,'--') O3_IAQI,
                                                ISNULL(Recent8HoursO3_IAQI,'--') Recent8HoursO3_IAQI,
                                                ISNULL([PM25_IAQI],'--') [PM2.5_IAQI],
                                                ISNULL(AQIValue,'--') AQIValue,
                                                ISNULL(PrimaryPollutant,'--') PrimaryPollutant,
                                                ISNULL(Class,'--') Class, 
                                                ISNULL(Grade,'--') Grade,
                                                ISNULL(RGBValue,'--') RGBValue,
                                                ISNULL(HealthEffect,'--') HealthEffect,
                                                ISNULL(TakeStep,'--') TakeStep ,
ISNULL(SO2,'--') SO2 ,
ISNULL(NO2,'--') NO2 ,
ISNULL(PM10,'--') PM10 ,
ISNULL(CO,'--') CO ,
ISNULL(O3,'--') O3 ,
ISNULL(PM25,'--') 'PM2.5'
                                           from {1} AS v 
                                                {2} 
                                          where v.[dateTime] = (  
                                                                select MAX([dateTime])
                                                                  from {1} 
                                                                 where {3} [dateTime] > DATEADD(DAY, -7, GETDATE())
                                                                   and [dateTime] < GETDATE()
                                                                ) 
                                             {4}"
                                                  , "'3' AS PortId, '南通市' AS"
                                                  , tableName
                                                  , ""
                                                  , string.Format(" MonitoringRegionUid = '{0}' and ", monitorRegionUid)
                                                  , string.Format("and v.MonitoringRegionUid = '{0}'", monitorRegionUid)
                                                  );
                DataTable dt = g_DatabaseHelper.ExecuteDataTable(sql, "AMS_AirAutoMonitorConnection");
                return dt;
            }
            return null;
        }

        /// <summary>
        /// 获取点位因子浓度
        /// </summary>
        /// <returns>
        /// </returns>
        public string GetFactorByPortId(string type, string pointId)
        {
            if (type == "1")
            {
                DateTime dtstart = Convert.ToDateTime((DateTime.Now.ToString("yyyy-MM-dd HH:00:00"))).AddHours(-1);
                DateTime dtend = Convert.ToDateTime((DateTime.Now.ToString("yyyy-MM-dd HH:00:00")));
                string fieldName = "Tstamp,PointId,PollutantCode,PollutantValue";
                DataTable dtjson = new DataTable();
                dtjson.Columns.Add("PointId", typeof(string));
                dtjson.Columns.Add("MonitoringPointName", typeof(string));
                dtjson.Columns.Add("DateTime", typeof(string));
                DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();
                string sql = string.Format(@"SELECT TOP 1000 
      [PointId]
      ,[MonitoringPointName]
      ,A.[PollutantCode]
      ,A.[PollutantName]
      ,ItemText Unit
  FROM [AMS_BaseData].[dbo].[V_Point_InstrumentChannels] A,[AMS_BaseData].[Standard].[TB_PollutantCode] B,[EQMS_Framework].[dbo].[TB_Frame_CodeItem] C
  where 
  InstrumentName in('常规参数','气象五参数分析仪','超级站常规参数') and A.[PollutantCode] = B.[PollutantCode] and B.MeasureUnitUid = C.RowGuid and 
	PointId = {0} and A.PollutantCode in ({1})", pointId, "'a34004','a05024','a21005','a34002','a21026','a21004'");
                DataTable dt = g_DatabaseHelper.ExecuteDataTable(sql, "AMS_BaseDataConnection");
                string[] factors = dt.AsEnumerable().Select(t => t.Field<string>("PollutantCode")).ToArray();
                string sql2 = string.Format(@"select {2} from {0} where PointId ={1} and PollutantCode in ({5}) and Tstamp <= '{4}' and Tstamp >= '{3}'", "Air.TB_InfectantBy60", pointId, fieldName, dtstart, dtend, "'" + string.Join("','", factors) + "'");
                DataTable dt2 = g_DatabaseHelper.ExecuteDataTable(sql2, "AMS_AirAutoMonitorConnection");
                dt2.Columns.Add("PortName", typeof(string));
                dt2.Columns.Add("factorName", typeof(string));
                dt2.Columns.Add("factorUnit", typeof(string));
                DataView dvv = dt.AsDataView();
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    dvv.RowFilter = "PointId = '" + dt2.Rows[i]["PointId"] + "'";
                    dt2.Rows[i]["PortName"] = dvv[0]["MonitoringPointName"];
                    dvv.RowFilter = "PollutantCode = '" + dt2.Rows[i]["PollutantCode"] + "'";
                    dt2.Rows[i]["factorName"] = dvv[0]["PollutantName"];
                    dt2.Rows[i]["factorUnit"] = dvv[0]["Unit"];
                }
                StringBuilder sb = new StringBuilder();
                sb.Append("[{\"HistoryHourData\":");
                string json = dt2.ToJsonBySerialize();
                sb.Append(json + "}]");
                return sb.ToString();
            }
            if (type == "2")
            {
                if (pointId == "0")
                {
                    DataTable dt = new DataTable();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("[{\"HistoryHourData\":");
                    string json = dt.ToJsonBySerialize();
                    sb.Append(json + "}]");
                    return sb.ToString();
                }
                if (pointId == "1")
                {
                    string[] factors = new string[] { "a34004", "a34002", "a21004", "a21026", "a21005", "a05024" };
                    string fac = string.Empty;
                    string data = string.Empty;
                    DataTable dtjson = new DataTable();
                    dtjson.Columns.Add("PointId", typeof(string));
                    dtjson.Columns.Add("PortName", typeof(string));
                    dtjson.Columns.Add("Tstamp", typeof(string));
                    dtjson.Columns.Add("PollutantCode", typeof(string));
                    dtjson.Columns.Add("FactorValue", typeof(string));
                    dtjson.Columns.Add("FactorName", typeof(string));
                    dtjson.Columns.Add("factorUnit", typeof(string));
                    foreach (string f in factors)
                    {
                        switch (f)
                        {
                            case "a34002":
                                fac = "PM10";
                                break;
                            case "a34004":
                                fac = "PM25";
                                break;
                            case "a21005":
                                fac = "CO";
                                break;
                            case "a21004":
                                fac = "NO2";
                                break;
                            case "a05024":
                                fac = "O3";
                                break;
                            case "a21026":
                                fac = "SO2";
                                break;
                        }
                        string url = string.Format("http://222.92.77.251:8083/MobileWs/AQI.asmx/Get24HoursFactorDataAir?portId={0}&factorName={1}", 0, fac);
                        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                        System.Net.WebResponse response = request.GetResponse();
                        System.IO.Stream respStream = response.GetResponseStream();
                        using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, Encoding.GetEncoding("UTF-8")))
                        {
                            data = reader.ReadToEnd();
                        }
                        DataView dv = data.JsonToDataSet().Tables[0].AsDataView();
                        if (dv.Count == 0)
                        {
                            DataRow dr = dtjson.NewRow();
                            dr["PointId"] = pointId;
                            dr["PortName"] = "苏州市";
                            dr["Tstamp"] = "";
                            dr["PollutantCode"] = f;
                            dr["FactorName"] = fac == "PM25" ? "PM2.5" : fac;
                            dr["FactorValue"] = "";
                            dr["factorUnit"] = fac == "CO" ? "mg/m3" : "μg/m3";
                            dtjson.Rows.Add(dr);
                        }
                        else 
                        {
                            foreach (DataRowView drv in dv)
                            {
                                DataRow dr = dtjson.NewRow();
                                dr["PointId"] = pointId;
                                dr["PortName"] = "苏州市";
                                dr["Tstamp"] = drv["DateTime"].ToString();
                                dr["PollutantCode"] = f;
                                dr["FactorName"] = fac == "PM25" ? "PM2.5" : fac;
                                dr["FactorValue"] = drv["value"].ToString();
                                dr["factorUnit"] = fac == "CO" ? "mg/m3" : "μg/m3";
                                dtjson.Rows.Add(dr);
                            }
                        }
                        
                    }
                    DataView dvv = dtjson.AsDataView();
                    dvv.Sort = "Tstamp desc";
                    string t = dvv[0]["Tstamp"].ToString();
                    dvv.RowFilter = "Tstamp = '" + t + "'";
                    StringBuilder sb = new StringBuilder();
                    sb.Append("[{\"HistoryHourData\":");
                    string json = dvv.ToTable().ToJsonBySerialize();
                    sb.Append(json + "}]");
                    return sb.ToString();
                }
                if (pointId == "2")//张家港
                {
                    //string[] factors = new string[] { "a34004", "a34002", "a21004", "a21026", "a21005", "a05024" };
                    //ServiceReference1.GISServiceSoapClient client = new ServiceReference1.GISServiceSoapClient();
                    //string zjgData = string.Empty;
                    //string fac = string.Empty;
                    //DataTable dtjson = new DataTable();
                    //foreach (string f in factors)
                    //{
                    //    switch (f)
                    //    {
                    //        case "a34002":
                    //            fac = "PM10_1";
                    //            break;
                    //        case "a34004":
                    //            fac = "PM25_1";
                    //            break;
                    //        case "a21005":
                    //            fac = "CO";
                    //            break;
                    //        case "a21004":
                    //            fac = "NO2";
                    //            break;
                    //        case "a05024":
                    //            fac = "O3_1";
                    //            break;
                    //        case "a21026":
                    //            fac = "SO2";
                    //            break;
                    //    }
                    //    zjgData = client.GetHourDataByZJG("1", fac).TrimStart('[').TrimEnd(']');
                    //    DataTable dt = zjgData.JsonToDataSet().Tables[0];
                    //    dtjson.Merge(dt);
                    //}
                    //dtjson.Columns.Add("factorUnit", typeof(string));
                    //for (int i = 0; i < dtjson.Columns.Count; i++)
                    //{
                    //    if (dtjson.Columns[i].ColumnName.Equals("PortId"))
                    //    {
                    //        dtjson.Columns[i].ColumnName = "PointId";
                    //    }
                    //    if (dtjson.Columns[i].ColumnName.Equals("DateTime"))
                    //    {
                    //        dtjson.Columns[i].ColumnName = "Tstamp";
                    //    }
                    //}
                    //string codeName = string.Empty;
                    //string name = string.Empty;
                    //string value = string.Empty;
                    //for (int i = 0; i < dtjson.Rows.Count; i++)
                    //{
                    //    dtjson.Rows[i]["PointId"] = pointId;
                    //    dtjson.Rows[i]["PortName"] = "张家港市";
                    //    codeName = dtjson.Rows[i]["FactorName"].ToString();
                    //    switch (codeName)
                    //    {
                    //        case "PM10 1小时":
                    //            name = "PM10";
                    //            break;
                    //        case "PM2.5 1小时":
                    //            name = "PM2.5";
                    //            break;
                    //        case "一氧化碳":
                    //            name = "CO";
                    //            break;
                    //        case "二氧化氮":
                    //            name = "NO2";
                    //            break;
                    //        case "臭氧1小时":
                    //            name = "O3";
                    //            break;
                    //        case "二氧化硫":
                    //            name = "SO2";
                    //            break;
                    //    }
                    //    dtjson.Rows[i]["FactorName"] = name;
                    //    dtjson.Rows[i]["factorUnit"] = name == "CO" ? "mg/m3" : "μg/m3";
                    //    value = dtjson.Rows[i]["FactorValue"].ToString();
                    //    if (name == "CO")
                    //    {
                    //        if (value != "--" && value.Trim() != "")
                    //        {
                    //            value = DecimalExtension.GetPollutantValue(Convert.ToDecimal(value), 1).ToString();
                    //        }
                    //    }
                    //    //else
                    //    //{
                    //    //    if (value != "--" && value.Trim() != "")
                    //    //    {
                    //    //        value = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(value), 3) * 1000).ToString("G0");
                    //    //    }
                    //    //}
                    //    dtjson.Rows[i]["FactorValue"] = value;
                    //}
                    string data = string.Empty;
                    string url = string.Format("http://222.92.77.251:8083/V02WebServiceForOutSZ/WebServiceForDayData.asmx/GetZJGRealData");
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                    System.Net.WebResponse response = request.GetResponse();
                    System.IO.Stream respStream = response.GetResponseStream();
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, Encoding.GetEncoding("UTF-8")))
                    {
                        data = reader.ReadToEnd();
                    }
                    DataTable dt = data.JsonToDataSet().Tables[0];
                    StringBuilder sb = new StringBuilder();
                    sb.Append("[{\"HistoryHourData\":");
                    string json = dt.ToJsonBySerialize();
                    sb.Append(json + "}]");
                    return sb.ToString();
                }
                if (pointId == "3")
                {
                    string tableName = "AMS_AirAutoMonitor.Air.TB_OriRegionHourAQI";

                    string sql = string.Format(@"select {0} PortName,
                                                Convert(nvarchar(16),dateadd(hh,0,[datetime]),21) DateTime,
                                                ISNULL(SO2_IAQI,'--') SO2_IAQI,
                                                ISNULL(NO2_IAQI,'--') NO2_IAQI,
                                                ISNULL(PM10_IAQI,'--') PM10_IAQI,
                                                ISNULL(CO_IAQI,'--') CO_IAQI,
                                                ISNULL(O3_IAQI,'--') O3_IAQI,
                                                ISNULL(Recent8HoursO3_IAQI,'--') Recent8HoursO3_IAQI,
                                                ISNULL([PM25_IAQI],'--') [PM2.5_IAQI],
                                                ISNULL(AQIValue,'--') AQIValue,
                                                ISNULL(PrimaryPollutant,'--') PrimaryPollutant,
                                                ISNULL(Class,'--') Class, 
                                                ISNULL(Grade,'--') Grade,
                                                ISNULL(RGBValue,'--') RGBValue,
                                                ISNULL(HealthEffect,'--') HealthEffect,
                                                ISNULL(TakeStep,'--') TakeStep ,
ISNULL(SO2,'--') SO2 ,
ISNULL(NO2,'--') NO2 ,
ISNULL(PM10,'--') PM10 ,
ISNULL(CO,'--') CO ,
ISNULL(O3,'--') O3 ,
ISNULL(PM25,'--') 'PM2.5'
                                           from {1} AS v 
                                                {2} 
                                          where v.[dateTime] = (  
                                                                select MAX([dateTime])
                                                                  from {1} 
                                                                 where {3} [dateTime] > DATEADD(DAY, -7, GETDATE())
                                                                   and [dateTime] < GETDATE()
                                                                ) 
                                             {4}"
                                                      , "'3' AS PortId, '南通市' AS"
                                                      , tableName
                                                      , ""
                                                      , string.Format(" MonitoringRegionUid = '{0}' and ", monitorRegionUid)
                                                      , string.Format("and v.MonitoringRegionUid = '{0}'", monitorRegionUid)
                                                      );
                    DataTable dtt = g_DatabaseHelper.ExecuteDataTable(sql, "AMS_AirAutoMonitorConnection");

                    string[] factors = new string[] { "a34004", "a34002", "a21004", "a21026", "a21005", "a05024" };
                    string fac = string.Empty;
                    DataTable dtjson = new DataTable();
                    dtjson.Columns.Add("PointId", typeof(string));
                    dtjson.Columns.Add("PortName", typeof(string));
                    dtjson.Columns.Add("Tstamp", typeof(string));
                    dtjson.Columns.Add("PollutantCode", typeof(string));
                    dtjson.Columns.Add("PollutantValue", typeof(string));
                    dtjson.Columns.Add("factorName", typeof(string));
                    dtjson.Columns.Add("factorUnit", typeof(string));
                    foreach (string f in factors)
                    {
                        switch (f)
                        {
                            case "a34002":
                                fac = "PM10";
                                break;
                            case "a34004":
                                fac = "PM2.5";
                                break;
                            case "a21005":
                                fac = "CO";
                                break;
                            case "a21004":
                                fac = "NO2";
                                break;
                            case "a05024":
                                fac = "O3";
                                break;
                            case "a21026":
                                fac = "SO2";
                                break;
                        }
                        DataRow dr = dtjson.NewRow();
                        dr["PointId"] = pointId;
                        dr["PortName"] = "南通市";
                        dr["Tstamp"] = dtt.Rows[0]["DateTime"];
                        dr["PollutantCode"] = f;
                        string value = dtt.Rows[0][fac].ToString();
                        if (f == "a21005")
                        {
                            if (value != "--" && value.Trim() != "")
                            {
                                value = DecimalExtension.GetPollutantValue(Convert.ToDecimal(value), 1).ToString();
                            }
                        }
                        else
                        {
                            if (value != "--" && value.Trim() != "")
                            {
                                value = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(value), 3) * 1000).ToString("G0");
                            }
                        }
                        dr["PollutantValue"] = value;
                        dr["factorName"] = fac;
                        dr["factorUnit"] = fac == "CO" ? "mg/m3" : "μg/m3";
                        dtjson.Rows.Add(dr);
                    }
                    StringBuilder sb = new StringBuilder();
                    sb.Append("[{\"HistoryHourData\":");
                    string json = dtjson.ToJsonBySerialize();
                    sb.Append(json + "}]");
                    return sb.ToString();
                }
            }
            return null;
        }

        /// <summary>
        /// 获取点位因子浓度
        /// </summary>
        /// <returns>
        /// </returns>
        public string GetFactorByPortIdNew(string type, string pointId)
        {
            if (type == "1")
            {
                DateTime dtstart = Convert.ToDateTime((DateTime.Now.ToString("yyyy-MM-dd HH:00:00"))).AddHours(-1);
                DateTime dtend = Convert.ToDateTime((DateTime.Now.ToString("yyyy-MM-dd HH:00:00")));
                string fieldName = "Tstamp,PointId,PollutantCode,PollutantValue as FactorValue";
                DataTable dtjson = new DataTable();
                dtjson.Columns.Add("PointId", typeof(string));
                dtjson.Columns.Add("MonitoringPointName", typeof(string));
                dtjson.Columns.Add("DateTime", typeof(string));
                DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();
                string sql = string.Format(@"SELECT TOP 1000 
      [PointId]
      ,[MonitoringPointName]
      ,A.[PollutantCode]
      ,A.[PollutantName]
      ,ItemText Unit
  FROM [AMS_BaseData].[dbo].[V_Point_InstrumentChannels] A,[AMS_BaseData].[Standard].[TB_PollutantCode] B,[EQMS_Framework].[dbo].[TB_Frame_CodeItem] C
  where 
  InstrumentName in('常规参数','气象五参数分析仪','超级站常规参数') and A.[PollutantCode] = B.[PollutantCode] and B.MeasureUnitUid = C.RowGuid and 
	PointId = {0} and A.PollutantCode in ({1})", pointId, "'a34004','a05024','a21005','a34002','a21026','a21004'");
                DataTable dt = g_DatabaseHelper.ExecuteDataTable(sql, "AMS_BaseDataConnection");
                string[] factors = dt.AsEnumerable().Select(t => t.Field<string>("PollutantCode")).ToArray();
                string sql2 = string.Format(@"select {2} from {0} where PointId ={1} and PollutantCode in ({5}) and Tstamp <= '{4}' and Tstamp >= '{3}'", "Air.TB_InfectantBy60", pointId, fieldName, dtstart, dtend, "'" + string.Join("','", factors) + "'");
                DataTable dt2 = g_DatabaseHelper.ExecuteDataTable(sql2, "AMS_AirAutoMonitorConnection");
                dt2.Columns.Add("PortName", typeof(string));
                dt2.Columns.Add("FactorName", typeof(string));
                dt2.Columns.Add("factorUnit", typeof(string));
                DataView dvv = dt.AsDataView();
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    dvv.RowFilter = "PointId = '" + dt2.Rows[i]["PointId"] + "'";
                    dt2.Rows[i]["PortName"] = dvv[0]["MonitoringPointName"];
                    dvv.RowFilter = "PollutantCode = '" + dt2.Rows[i]["PollutantCode"] + "'";
                    dt2.Rows[i]["factorName"] = dvv[0]["PollutantName"];
                    dt2.Rows[i]["factorUnit"] = dvv[0]["Unit"];
                }
                for (int k = 0; k < dt2.Rows.Count;k++ )
                {
                    if (dt2.Rows[k]["factorUnit"].ToString() == "μg/m³")
                    {
                        dt2.Rows[k]["FactorValue"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt2.Rows[k]["FactorValue"]) * 1000,0).ToString();
                    }
                    else if (dt2.Rows[k]["factorUnit"].ToString() == "mg/m³")
                    {
                        dt2.Rows[k]["FactorValue"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt2.Rows[k]["FactorValue"]), 1).ToString();
                    }
                    
                }
                StringBuilder sb = new StringBuilder();
                sb.Append("[{\"HistoryHourData\":");
                string json = dt2.ToJsonBySerialize();
                sb.Append(json + "}]");
                return sb.ToString();
            }
            if (type == "2")
            {
                if (pointId == "0")
                {
                    DataTable dt = new DataTable();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("[{\"HistoryHourData\":");
                    string json = dt.ToJsonBySerialize();
                    sb.Append(json + "}]");
                    return sb.ToString();
                }
                if (pointId == "1")
                {
                    string[] factors = new string[] { "a34004", "a34002", "a21004", "a21026", "a21005", "a05024" };
                    string fac = string.Empty;
                    string data = string.Empty;
                    DataTable dtjson = new DataTable();
                    dtjson.Columns.Add("PointId", typeof(string));
                    dtjson.Columns.Add("PortName", typeof(string));
                    dtjson.Columns.Add("Tstamp", typeof(string));
                    dtjson.Columns.Add("PollutantCode", typeof(string));
                    dtjson.Columns.Add("FactorValue", typeof(string));
                    dtjson.Columns.Add("FactorName", typeof(string));
                    dtjson.Columns.Add("factorUnit", typeof(string));
                    foreach (string f in factors)
                    {
                        switch (f)
                        {
                            case "a34002":
                                fac = "PM10";
                                break;
                            case "a34004":
                                fac = "PM25";
                                break;
                            case "a21005":
                                fac = "CO";
                                break;
                            case "a21004":
                                fac = "NO2";
                                break;
                            case "a05024":
                                fac = "O3";
                                break;
                            case "a21026":
                                fac = "SO2";
                                break;
                        }
                        string url = string.Format("http://222.92.77.251:8083/MobileWs/AQI.asmx/Get24HoursFactorDataAir?portId={0}&factorName={1}", 0, fac);
                        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                        System.Net.WebResponse response = request.GetResponse();
                        System.IO.Stream respStream = response.GetResponseStream();
                        using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, Encoding.GetEncoding("UTF-8")))
                        {
                            data = reader.ReadToEnd();
                        }
                        DataView dv = data.JsonToDataSet().Tables[0].AsDataView();
                        foreach (DataRowView drv in dv)
                        {
                            DataRow dr = dtjson.NewRow();
                            dr["PointId"] = pointId;
                            dr["PortName"] = "苏州市";
                            dr["Tstamp"] = drv["DateTime"].ToString();
                            dr["PollutantCode"] = f;
                            dr["FactorName"] = fac == "PM25" ? "PM2.5" : fac;
                            dr["FactorValue"] = drv["value"].ToString();
                            dr["factorUnit"] = fac == "CO" ? "mg/m3" : "μg/m3";
                            dtjson.Rows.Add(dr);
                        }
                    }
                    DataView dvv = dtjson.AsDataView();
                    dvv.Sort = "Tstamp desc";
                    string t = dvv[0]["Tstamp"].ToString();
                    dvv.RowFilter = "Tstamp = '" + t + "'";
                    StringBuilder sb = new StringBuilder();
                    sb.Append("[{\"HistoryHourData\":");
                    string json = dvv.ToTable().ToJsonBySerialize();
                    sb.Append(json + "}]");
                    return sb.ToString();
                }
                if (pointId == "2")
                {
                    //string[] factors = new string[] { "a34004", "a34002", "a21004", "a21026", "a21005", "a05024" };
                    //ServiceReference1.GISServiceSoapClient client = new ServiceReference1.GISServiceSoapClient();
                    //string zjgData = string.Empty;
                    //string fac = string.Empty;
                    //DataTable dtjson = new DataTable();
                    //foreach (string f in factors)
                    //{
                    //    switch (f)
                    //    {
                    //        case "a34002":
                    //            fac = "PM10_1";
                    //            break;
                    //        case "a34004":
                    //            fac = "PM25_1";
                    //            break;
                    //        case "a21005":
                    //            fac = "CO";
                    //            break;
                    //        case "a21004":
                    //            fac = "NO2";
                    //            break;
                    //        case "a05024":
                    //            fac = "O3_1";
                    //            break;
                    //        case "a21026":
                    //            fac = "SO2";
                    //            break;
                    //    }
                    //    zjgData = client.GetHourDataByZJG("1", fac).TrimStart('[').TrimEnd(']');
                    //    DataTable dt = zjgData.JsonToDataSet().Tables[0];
                    //    dtjson.Merge(dt);
                    //}
                    //dtjson.Columns.Add("factorUnit", typeof(string));
                    //for (int i = 0; i < dtjson.Columns.Count; i++)
                    //{
                    //    if (dtjson.Columns[i].ColumnName.Equals("PortId"))
                    //    {
                    //        dtjson.Columns[i].ColumnName = "PointId";
                    //    }
                    //    if (dtjson.Columns[i].ColumnName.Equals("DateTime"))
                    //    {
                    //        dtjson.Columns[i].ColumnName = "Tstamp";
                    //    }
                    //}
                    //string codeName = string.Empty;
                    //string name = string.Empty;
                    //string value = string.Empty;
                    //for (int i = 0; i < dtjson.Rows.Count; i++)
                    //{
                    //    dtjson.Rows[i]["PointId"] = pointId;
                    //    dtjson.Rows[i]["PortName"] = "张家港市";
                    //    codeName = dtjson.Rows[i]["FactorName"].ToString();
                    //    switch (codeName)
                    //    {
                    //        case "PM10 1小时":
                    //            name = "PM10";
                    //            break;
                    //        case "PM2.5 1小时":
                    //            name = "PM2.5";
                    //            break;
                    //        case "一氧化碳":
                    //            name = "CO";
                    //            break;
                    //        case "二氧化氮":
                    //            name = "NO2";
                    //            break;
                    //        case "臭氧1小时":
                    //            name = "O3";
                    //            break;
                    //        case "二氧化硫":
                    //            name = "SO2";
                    //            break;
                    //    }
                    //    dtjson.Rows[i]["FactorName"] = name;
                    //    dtjson.Rows[i]["factorUnit"] = name == "CO" ? "mg/m3" : "μg/m3";
                    //    value = dtjson.Rows[i]["FactorValue"].ToString();
                    //    if (name == "CO")
                    //    {
                    //        if (value != "--" && value.Trim() != "")
                    //        {
                    //            value = DecimalExtension.GetPollutantValue(Convert.ToDecimal(value), 1).ToString();
                    //        }
                    //    }
                    //    //else
                    //    //{
                    //    //    if (value != "--" && value.Trim() != "")
                    //    //    {
                    //    //        value = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(value), 3) * 1000).ToString("G0");
                    //    //    }
                    //    //}
                    //    dtjson.Rows[i]["FactorValue"] = value;
                    //}
                    string data = string.Empty;
                    string url = string.Format("http://222.92.77.251:8083/V02WebServiceForOutSZ/WebServiceForDayData.asmx/GetZJGRealData");
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                    System.Net.WebResponse response = request.GetResponse();
                    System.IO.Stream respStream = response.GetResponseStream();
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, Encoding.GetEncoding("UTF-8")))
                    {
                        data = reader.ReadToEnd();
                    }
                    DataTable dt = data.JsonToDataSet().Tables[0];
                    StringBuilder sb = new StringBuilder();
                    sb.Append("[{\"HistoryHourData\":");
                    string json = dt.ToJsonBySerialize();
                    sb.Append(json + "}]");
                    return sb.ToString();
                }
                if (pointId == "3")
                {
                    string tableName = "AMS_AirAutoMonitor.Air.TB_OriRegionHourAQI";

                    string sql = string.Format(@"select {0} PortName,
                                                Convert(nvarchar(16),dateadd(hh,0,[datetime]),21) DateTime,
                                                ISNULL(SO2_IAQI,'--') SO2_IAQI,
                                                ISNULL(NO2_IAQI,'--') NO2_IAQI,
                                                ISNULL(PM10_IAQI,'--') PM10_IAQI,
                                                ISNULL(CO_IAQI,'--') CO_IAQI,
                                                ISNULL(O3_IAQI,'--') O3_IAQI,
                                                ISNULL(Recent8HoursO3_IAQI,'--') Recent8HoursO3_IAQI,
                                                ISNULL([PM25_IAQI],'--') [PM2.5_IAQI],
                                                ISNULL(AQIValue,'--') AQIValue,
                                                ISNULL(PrimaryPollutant,'--') PrimaryPollutant,
                                                ISNULL(Class,'--') Class, 
                                                ISNULL(Grade,'--') Grade,
                                                ISNULL(RGBValue,'--') RGBValue,
                                                ISNULL(HealthEffect,'--') HealthEffect,
                                                ISNULL(TakeStep,'--') TakeStep ,
ISNULL(SO2,'--') SO2 ,
ISNULL(NO2,'--') NO2 ,
ISNULL(PM10,'--') PM10 ,
ISNULL(CO,'--') CO ,
ISNULL(O3,'--') O3 ,
ISNULL(PM25,'--') 'PM2.5'
                                           from {1} AS v 
                                                {2} 
                                          where v.[dateTime] = (  
                                                                select MAX([dateTime])
                                                                  from {1} 
                                                                 where {3} [dateTime] > DATEADD(DAY, -7, GETDATE())
                                                                   and [dateTime] < GETDATE()
                                                                ) 
                                             {4}"
                                                      , "'3' AS PortId, '南通市' AS"
                                                      , tableName
                                                      , ""
                                                      , string.Format(" MonitoringRegionUid = '{0}' and ", monitorRegionUid)
                                                      , string.Format("and v.MonitoringRegionUid = '{0}'", monitorRegionUid)
                                                      );
                    DataTable dtt = g_DatabaseHelper.ExecuteDataTable(sql, "AMS_AirAutoMonitorConnection");

                    string[] factors = new string[] { "a34004", "a34002", "a21004", "a21026", "a21005", "a05024" };
                    string fac = string.Empty;
                    DataTable dtjson = new DataTable();
                    dtjson.Columns.Add("PointId", typeof(string));
                    dtjson.Columns.Add("PortName", typeof(string));
                    dtjson.Columns.Add("Tstamp", typeof(string));
                    dtjson.Columns.Add("PollutantCode", typeof(string));
                    dtjson.Columns.Add("PollutantValue", typeof(string));
                    dtjson.Columns.Add("factorName", typeof(string));
                    dtjson.Columns.Add("factorUnit", typeof(string));
                    foreach (string f in factors)
                    {
                        switch (f)
                        {
                            case "a34002":
                                fac = "PM10";
                                break;
                            case "a34004":
                                fac = "PM2.5";
                                break;
                            case "a21005":
                                fac = "CO";
                                break;
                            case "a21004":
                                fac = "NO2";
                                break;
                            case "a05024":
                                fac = "O3";
                                break;
                            case "a21026":
                                fac = "SO2";
                                break;
                        }
                        DataRow dr = dtjson.NewRow();
                        dr["PointId"] = pointId;
                        dr["PortName"] = "南通市";
                        dr["Tstamp"] = dtt.Rows[0]["DateTime"];
                        dr["PollutantCode"] = f;
                        string value = dtt.Rows[0][fac].ToString();
                        if (f == "a21005")
                        {
                            if (value != "--" && value.Trim() != "")
                            {
                                value = DecimalExtension.GetPollutantValue(Convert.ToDecimal(value), 1).ToString();
                            }
                        }
                        else
                        {
                            if (value != "--" && value.Trim() != "")
                            {
                                value = (DecimalExtension.GetPollutantValue(Convert.ToDecimal(value), 3) * 1000).ToString("G0");
                            }
                        }
                        dr["PollutantValue"] = value;
                        dr["factorName"] = fac;
                        dr["factorUnit"] = fac == "CO" ? "mg/m3" : "μg/m3";
                        dtjson.Rows.Add(dr);
                    }
                    StringBuilder sb = new StringBuilder();
                    sb.Append("[{\"HistoryHourData\":");
                    string json = dtjson.ToJsonBySerialize();
                    sb.Append(json + "}]");
                    return sb.ToString();
                }
            }
            return null;
        }
        /// <summary>
        /// 获取周边城市分类
        /// </summary>
        /// <returns>
        /// </returns>
        public string GetCityType()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("TypeName", typeof(string));
            dt.Columns.Add("TypeValue", typeof(string));
            dt.Columns.Add("TypeNum", typeof(string));
            for (int i = 0; i < 5; i++)
            {
                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);
            }
            dt.Rows[0]["TypeName"] = "所有选项";
            dt.Rows[0]["TypeValue"] = "ALL";
            dt.Rows[0]["TypeNum"] = "0";
            dt.Rows[1]["TypeName"] = "上海市";
            dt.Rows[1]["TypeValue"] = "46E2CC36-D425-4150-A3F7-F8B78FF8D972";
            dt.Rows[1]["TypeNum"] = "0";
            dt.Rows[2]["TypeName"] = "苏州市";
            dt.Rows[2]["TypeValue"] = "3FB1F58B-89C3-4BCC-ABE8-EF475B8DBE9C";
            dt.Rows[2]["TypeNum"] = "1";
            dt.Rows[3]["TypeName"] = "张家港市";
            dt.Rows[3]["TypeValue"] = "FCAF4350-5218-4D9C-850E-FB4ACEC2A769";
            dt.Rows[3]["TypeNum"] = "2";
            dt.Rows[4]["TypeName"] = "南通市";
            dt.Rows[4]["TypeValue"] = "b6e983c4-4f92-4be3-bbac-d9b71c470640";
            dt.Rows[4]["TypeNum"] = "3";
            StringBuilder sb = new StringBuilder();
            sb.Append("[{\"CityTypes\":");
            string json = dt.ToJsonBySerialize();
            sb.Append(json + "}]");
            return sb.ToString();
        }

        /// <summary>
        /// 获得点位因子日浓度均值
        /// </summary>
        /// <returns>
        /// </returns>
        public string GetDayFactorByPointId(string poingId)
        {
            DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();
            DateTime dtstart = Convert.ToDateTime((DateTime.Now.ToString("yyyy-MM-dd 00:00:00"))).AddDays(-1);
            DateTime dtend = Convert.ToDateTime((DateTime.Now.ToString("yyyy-MM-dd 00:00:00")));
            string sql2 = string.Format(@"SELECT [PointId]
      ,[DateTime]
      ,[Max8HourO3]
  FROM [AMS_AirAutoMonitor].[Air].[TB_OriDayAQI]    
  where datetime >='{0}' and datetime <'{1}' {2} ", dtstart, dtend, poingId == "" ? "" : "and PointId=" + poingId);
            DataView dtdata = g_DatabaseHelper.ExecuteDataView(sql2, "AMS_AirAutoMonitorConnection");

            string factors = "'a34004','a05024','a21005','a34002','a21026','a21004'";
            string dt1 = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 00:00:00");
            string dt2 = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string sql = string.Format(@"select Convert(nvarchar(10),[DateTime],21) Tstamp,
		                    A.PointId,
		                    A.PollutantCode,
		                    PollutantValue,
		                    MonitoringPointName PortName,
		                    PollutantName factorName,
		                    D.ItemText factorUnit
                      FROM [AMS_AirAutoMonitor].[Air].[TB_InfectantByDay] A
                      inner join [dbo].[SY_MonitoringPoint] B
                      on A.PointId = B.PointId 
                      inner join [dbo].[SY_PollutantCode] C	
                      on C.PollutantCode = A.PollutantCode
                      inner join [dbo].[SY_View_CodeMainItem] D
                      on D.ItemGuid = C.MeasureUnitUid where a.PollutantCode in ({0}) and [DateTime] >='{1}' and [DateTime] <'{2}' {3}", factors, dt1, dt2, poingId == "" ? "" : "and A.PointId=" + poingId);
            DataTable dt = g_DatabaseHelper.ExecuteDataTable(sql, "AMS_AirAutoMonitorConnection");
            foreach (DataRow dr in dt.Rows)
            {
                string factor = dr["PollutantCode"].ToString();
                string pointId = dr["PointId"].ToString();
                if (factor == "a21005")
                {
                    if (dr["PollutantValue"] != DBNull.Value && dr["PollutantValue"].ToString().Trim() != "")
                    {
                        dr["PollutantValue"] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["PollutantValue"]), 1);
                    }
                }
                else if (factor == "a05024")
                {
                    dtdata.RowFilter = "PointId = '" + pointId + "'";
                    string value = dtdata[0]["Max8HourO3"].ToString();
                    if (value.Trim() != "" && value != "--")
                    {
                        dr["PollutantValue"] = Convert.ToDecimal((DecimalExtension.GetPollutantValue(Convert.ToDecimal(value), 3) * 1000).ToString("G0"));
                    }
                }
                else
                {
                    if (dr["PollutantValue"] != DBNull.Value && dr["PollutantValue"].ToString().Trim() != "")
                    {
                        dr["PollutantValue"] = Convert.ToDecimal((DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr["PollutantValue"]), 3) * 1000).ToString("G0"));
                    }
                }
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("[{\"PortFactor\":");
            string json = dt.ToJsonBySerialize();
            sb.Append(json + "}]");
            return sb.ToString();
        }

        /// <summary>
        /// 获得区域
        /// </summary>
        /// <returns>
        /// </returns>
        public string GetArea()
        {
            string sql = @"SELECT 
      distinct [RegionUid] ItemValue,
      [ItemText],
      [SortNumber]
  FROM [AMS_BaseData].[MPInfo].[TB_MonitoringPoint] A
  left join [dbo].[SY_View_CodeMainItem] B
  on A.[RegionUid] = B.[ItemGuid]
  order by [SortNumber] desc";
            DataTable dt = g_DatabaseHelper.ExecuteDataTable(sql, "AMS_BaseDataConnection");
            DataRow dr = dt.NewRow();
            dr["ItemValue"] = "ALL";
            dr["ItemText"] = "所有选项";
            dr["SortNumber"] = "99999";
            dt.Rows.Add(dr);
            DataView dvvv = dt.AsDataView();
            dvvv.Sort = "SortNumber desc";
            StringBuilder sb = new StringBuilder();
            sb.Append("[{\"AreaInfo\":");
            string json = dvvv.ToTable().ToJsonBySerialize();
            sb.Append(json + "}]");
            return sb.ToString();
        }

        //判断是否超标
        public double GetAirAlertUpper(string factorName)
        {
            string overData = ConfigurationManager.AppSettings["AirOverData"];
            double result = 0;
            string[] overList = overData.Split(',');
            foreach (var over in overList)
            {
                if (over.Contains(factorName))
                {
                    result = Convert.ToDouble(over.Split(':')[1]);
                }
            }
            return result;
        }
        //判断是否超标
        public double GetAirAlertLower(string factorName)
        {
            string overData = ConfigurationManager.AppSettings["AirLowerData"];
            double result = 0;
            string[] overList = overData.Split(',');
            foreach (var over in overList)
            {
                if (over.Contains(factorName))
                {
                    result = Convert.ToDouble(over.Split(':')[1]);
                }
            }
            return result;
        }

        /// <summary>
        /// 根据区域获取所属站点
        /// </summary>
        /// <returns></returns>
        public string GetPointIdByRegion(string areaUid)
        {
            if (areaUid == "" || areaUid == "All")
            {
                string sql = "select distinct PointId from [AMS_BaseData].[MPInfo].[TB_MonitoringPoint]";
                DataTable dt = g_DatabaseHelper.ExecuteDataTable(sql, "AMS_BaseDataConnection");
                string[] arr = dt.AsEnumerable().Select(t => Convert.ToString(t.Field<int>("PointId"))).ToArray();
                return "'" + string.Join("','", arr) + "'";
            }
            else
            {
                string sql = string.Format("select distinct PointId from [AMS_BaseData].[MPInfo].[TB_MonitoringPoint] where RegionUid = '{0}'", areaUid);
                DataTable dt = g_DatabaseHelper.ExecuteDataTable(sql, "AMS_BaseDataConnection");
                string[] arr = dt.AsEnumerable().Select(t => Convert.ToString(t.Field<int>("PointId"))).ToArray();
                return "'" + string.Join("','", arr) + "'";
            }
        }

        /// <summary>
        /// 获取周边城市点位因子日均值浓度
        /// </summary>
        /// <returns></returns>
        public string GetDayFactorByPortId(string pointId)
        {
            DataTable dt = new DataTable();
            if (pointId == "")
            {
                string[] pointIdArr = "1;2;3;0".Split(';');
                foreach (string pid in pointIdArr)
                {
                    DataTable dtt = GetDataForDay(pid);
                    dt.Merge(dtt);
                }
            }
            else
            {
                dt = GetDataForDay(pointId);
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("[{\"HistoryDayData\":");
            string json = dt.ToJsonBySerialize();
            sb.Append(json + "}]");
            return sb.ToString();
        }

        /// <summary>
        /// 获取数据给日浓度
        /// </summary>
        /// <returns></returns>
        public DataTable GetDataForDay(string pointId)
        {
            DataTable dt = new DataTable();
            if (pointId == "0")
            {
                dt = SHdayValue();
            }
            //苏州或张家港
            else if (pointId == "1" || pointId == "2")
            {
                string cityUid = pointId == "1" ? "7e05b94c-bbd4-45c3-919c-42da2e63fd43" : "22";//张家港区域Guid换成张家港城北小学站点
                string data = string.Empty;
                string url = string.Format("http://222.92.77.251:8083/V02WebServiceForOutSZ/WebServiceForDayData.asmx/GetLastDayDataByPortIdForArount?MonitoringRegionUid={0}", cityUid);
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                System.Net.WebResponse response = request.GetResponse();
                System.IO.Stream respStream = response.GetResponseStream();
                using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, Encoding.GetEncoding("UTF-8")))
                {
                    data = "{\"DayData\":" + reader.ReadToEnd() + "}";
                }
                DataTable dt2 = data.JsonToDataSet().Tables[0];
                string[] factors = new string[] { "a34004", "a34002", "a21004", "a21026", "a21005", "a05024" };
                dt.Columns.Add("PointId", typeof(string));
                dt.Columns.Add("PortName", typeof(string));
                dt.Columns.Add("Tstamp", typeof(string));
                dt.Columns.Add("PollutantCode", typeof(string));
                dt.Columns.Add("PollutantValue", typeof(string));
                dt.Columns.Add("factorName", typeof(string));
                dt.Columns.Add("factorUnit", typeof(string));
                string fac = string.Empty;
                string value = string.Empty;
                foreach (string f in factors)
                {
                    switch (f)
                    {
                        case "a34002":
                            fac = "PM10";
                            break;
                        case "a34004":
                            fac = "PM2.5";
                            break;
                        case "a21005":
                            fac = "CO";
                            break;
                        case "a21004":
                            fac = "NO2";
                            break;
                        case "a05024":
                            fac = "Max8HourO3";
                            break;
                        case "a21026":
                            fac = "SO2";
                            break;
                    }
                    value = dt2.Rows[0][fac == "PM2.5" ? "PM25" : fac].ToString();
                    DataRow dr = dt.NewRow();
                    dr["PointId"] = pointId;
                    dr["PortName"] = pointId == "1" ? "苏州市" : "张家港市";
                    dr["Tstamp"] = pointId == "1" ? Convert.ToDateTime(dt2.Rows[0]["ReportDateTime"].ToString()).ToString("yyyy-MM-dd") : Convert.ToDateTime(dt2.Rows[0]["DateTime"].ToString()).ToString("yyyy-MM-dd");
                    dr["PollutantCode"] = f;
                    if (value != "" && value != "--")
                    {
                        if (f == "a21005")
                            value = DecimalExtension.GetPollutantValue(Convert.ToDecimal(value), 1).ToString();
                        else
                            value = Convert.ToDecimal(DecimalExtension.GetPollutantValue(Convert.ToDecimal(value), 3) * 1000).ToString("G0");
                    }
                    dr["PollutantValue"] = value;
                    dr["factorName"] = fac == "Max8HourO3" ? "O3" : fac;
                    dr["factorUnit"] = f == "a21005" ? "mg/m3" : "μg/m3";
                    dt.Rows.Add(dr);
                }
            }
            //南通日数据
            else
            {
                string dt1 = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 00:00:00");
                string dt2 = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                string sql = string.Format(@"select '3' PointId
		,convert(nvarchar(10),[ReportDateTime],21) DateTime
      ,[SO2]
      ,[SO2_IAQI]
      ,[NO2]
      ,[NO2_IAQI]
      ,[PM10]
      ,[PM10_IAQI]
      ,[CO]
      ,[CO_IAQI]
      ,[MaxOneHourO3]
      ,[MaxOneHourO3_IAQI]
      ,[Max8HourO3]
      ,[Max8HourO3_IAQI]
      ,[PM25]
      ,[PM25_IAQI]
      ,[AQIValue]
      ,[PrimaryPollutant]
      ,[Range]
      ,[RGBValue]
      ,[Class]
      ,[Grade]
      ,[HealthEffect]
      ,[TakeStep] from [AMS_AirAutoMonitor].[Air].[TB_OriRegionDayAQIReport] where MonitoringRegionUid = '{0}' and ReportDateTime >= '{1}' and ReportDateTime < '{2}'", monitorRegionUid, dt1, dt2);
                DataTable dtt = g_DatabaseHelper.ExecuteDataTable(sql, "AMS_AirAutoMonitorConnection");
                string[] factors = new string[] { "a34004", "a34002", "a21004", "a21026", "a21005", "a05024" };
                dt.Columns.Add("PointId", typeof(string));
                dt.Columns.Add("PortName", typeof(string));
                dt.Columns.Add("Tstamp", typeof(string));
                dt.Columns.Add("PollutantCode", typeof(string));
                dt.Columns.Add("PollutantValue", typeof(string));
                dt.Columns.Add("factorName", typeof(string));
                dt.Columns.Add("factorUnit", typeof(string));
                string fac = string.Empty;
                string value = string.Empty;
                foreach (string f in factors)
                {
                    switch (f)
                    {
                        case "a34002":
                            fac = "PM10";
                            break;
                        case "a34004":
                            fac = "PM2.5";
                            break;
                        case "a21005":
                            fac = "CO";
                            break;
                        case "a21004":
                            fac = "NO2";
                            break;
                        case "a05024":
                            fac = "Max8HourO3";
                            break;
                        case "a21026":
                            fac = "SO2";
                            break;
                    }
                    value = dtt.Rows[0][fac == "PM2.5" ? "PM25" : fac].ToString();
                    DataRow dr = dt.NewRow();
                    dr["PointId"] = pointId;
                    dr["PortName"] = "南通市";
                    dr["Tstamp"] = Convert.ToDateTime(dtt.Rows[0]["DateTime"].ToString()).ToString("yyyy-MM-dd");
                    dr["PollutantCode"] = f;
                    if (value != "" && value != "--")
                    {
                        if (f == "a21005")
                            value = DecimalExtension.GetPollutantValue(Convert.ToDecimal(value), 1).ToString();
                        else
                            value = Convert.ToDecimal(DecimalExtension.GetPollutantValue(Convert.ToDecimal(value), 3) * 1000).ToString("G0");
                    }
                    dr["PollutantValue"] = value;
                    dr["factorName"] = fac == "Max8HourO3" ? "O3" : fac;
                    dr["factorUnit"] = f == "a21005" ? "mg/m3" : "μg/m3";
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        #region 抓取上海数据
        public static DataTable SHrealTimeAQI() //实时AQI
        {
            //WebClient MyWebClient = new WebClient();
            //string Mydata = "";
            //string Mydata2 = "";
            //string data = "";
            //string Real = DateTime.Now.ToString("yyyy-MM-dd HH:00");

            //MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据

            //Byte[] pageData = MyWebClient.DownloadData("http://www.semc.gov.cn/aqi/home/Index.aspx"); //从指定网站下载数据

            //string pageHtml = Encoding.UTF8.GetString(pageData); //如果获取网站页面采用的是UTF-8，则使用这句  "D:\\项目\\南通超级站\\代码\\CatchData\\CatchData\\Files\\ouput.html"
            //List<string> list = new List<string>();
            //Regex regexArea = new Regex(@"(?<=实时空气质量状况)([\s\S]*?)(?=过去24小时AQI)", RegexOptions.Multiline | RegexOptions.ExplicitCapture);//构造解析从“实时空气质量状况”到“过去24小时AQI”之间字符串的正则表达式
            //MatchCollection mcArea = regexArea.Matches(pageHtml); //执行匹配
            //foreach (Match mArea in mcArea)
            //{
            //    Regex regexR = new Regex(@"(?<=<tr>)([\s\S]*?)(?=</tr>)");//构造解析表格行数据的正则表达式
            //    MatchCollection mcR = regexR.Matches(mArea.Groups[0].ToString()); //执行匹配
            //    //int c = 0;
            //    foreach (Match mr in mcR)
            //    {
            //        Regex regexD = new Regex(@"(?<=<td[^>]*>[\s]*?)([\S]*)(?=[\s]*?</td>)");//构造解析表格列数据的正则表达式
            //        Regex regexSpan = new Regex(@"(?<=<span[^>]*>[\s]*?)([\S]*)(?=[\s]*?</span>)");//正则表达式获取SPAN (?i)(?<=<span.*?id=""s1"".*?>)[^<]+(?=</span>)
            //        MatchCollection mcD = regexD.Matches(mr.Groups[0].ToString()); //执行匹配
            //        MatchCollection mcS = regexSpan.Matches(mr.Groups[0].ToString());
            //        for (int i = 0; i < mcD.Count; i++)
            //        {
            //            Mydata += mcD[i].Value + "/";
            //        }


            //        for (int k = 0; k < mcS.Count; k++)
            //        {
            //            data += mcS[k].Value + ",";
            //        }
            //    }
            //}
            //string[] dt = data.Trim(',').Split(',');
            //for (int i = 0; i < dt.Length; i++)
            //{
            //    if (i == 1 || i == 3)
            //    {
            //        Mydata2 += dt[i] + "/";
            //    }
            //}
            //Mydata += Mydata2;

            DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();
            //string sql = string.Format("select top 1 DateTime ,data from [AMS_AirAutoMonitor].[dbo].[TB_RealAQINearby] where City =  'shanghai' and DateTime >= '{0}' order by DateTime desc", DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd"));
            string sql = string.Format("select top 1 DateTime ,data from [AMS_AirAutoMonitor].[dbo].[TB_RealAQINearby] where City =  'shanghai' order by DateTime desc");
            DataTable dt = g_DatabaseHelper.ExecuteDataTable(sql, "AMS_AirAutoMonitorConnection");
            string Mydata = dt.Rows[0]["data"].ToString();

            DataTable dtt = new DataTable();
            dtt.Columns.Add("PortId", typeof(string));
            dtt.Columns.Add("PortName", typeof(string));
            dtt.Columns.Add("DateTime", typeof(string));
            dtt.Columns.Add("PrimaryPollutant", typeof(string));
            dtt.Columns.Add("AQIValue", typeof(string));
            dtt.Columns.Add("Class", typeof(string));
            dtt.Columns.Add("Grade", typeof(string));
            dtt.Columns.Add("HealthEffect", typeof(string));
            dtt.Columns.Add("TakeStep", typeof(string));
            DataRow dr = dtt.NewRow();
            if (Mydata == "" || Mydata.IsNullOrDBNull())
            {
                dr["PortId"] = "0";
                dr["PortName"] = "上海市";
                dr["DateTime"] = Convert.ToDateTime(dt.Rows[0]["DateTime"].ToString()).ToString("yyyy-MM-dd HH:00");
                dr["PrimaryPollutant"] = "";
                dr["AQIValue"] = "";
                dr["Class"] = "";
                dr["Grade"] = "";
                dr["HealthEffect"] = "";
                dr["TakeStep"] = "";
            }
            else
            {
                string classA = Mydata.Contains("sub") ? Mydata.Split('/')[5].ToString() : (Mydata.Contains("无") ? Mydata.Split('/')[4].ToString() : Mydata.Split('/')[3].ToString());
                dr["PortId"] = "0";
                dr["PortName"] = "上海市";
                dr["DateTime"] = Convert.ToDateTime(dt.Rows[0]["DateTime"].ToString()).ToString("yyyy-MM-dd HH:00");
                dr["PrimaryPollutant"] = Mydata.Contains("sub") ? Mydata.Split('/')[0].TrimEnd('<').Replace("<sub>", "").ToString() : "--";
                dr["AQIValue"] = Mydata.Contains("sub") ? Mydata.Split('/')[4].ToString() : (Mydata.Contains("无") ? Mydata.Split('/')[3].ToString() : Mydata.Split('/')[2]);
                dr["Class"] = classA;
                string grade = string.Empty;
                switch (classA)
                {
                    case "优":
                        grade = "一级";
                        break;
                    case "良":
                        grade = "二级";
                        break;
                    case "轻度污染":
                        grade = "三级";
                        break;
                    case "中度污染":
                        grade = "四级";
                        break;
                    case "重度污染":
                        grade = "五级";
                        break;
                    case "严重污染":
                        grade = "六级";
                        break;
                    default:
                        grade = "--";
                        break;
                }
                dr["Grade"] = grade;
                dr["HealthEffect"] = Mydata.Contains("sub") ? Mydata.Split('/')[2].ToString() : (Mydata.Contains("无") ? Mydata.Split('/')[1].ToString() : Mydata.Split('/')[0]);
                dr["TakeStep"] = Mydata.Contains("sub") ? Mydata.Split('/')[3].ToString() : (Mydata.Contains("无") ? Mydata.Split('/')[2].ToString() : Mydata.Split('/')[1]);
            
            }
            dtt.Rows.Add(dr);

            return dtt;

        }

        private static DataTable SHdayTimeAQI()
        {
            try
            {

                string rl;
                string url = "http://www.semc.gov.cn/aqi/home/DayData.aspx";
                string aqidata = "AQI:";
                //string valuedata = "";
                DateTime Time = DateTime.Now;
                string beg = Time.AddDays(-1).ToString("yyyy-MM-dd");
                string end = Time.ToString("yyyy-MM-dd");

                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("__EVENTTARGET", "");
                dic.Add("__EVENTARGUMENT", "");
                dic.Add("__LASTFOCUS", "");
                dic.Add("__VIEWSTATE", "bYp3ODmFSoS%2FlXDkqiD1qxve5%2FoAdkylW624jFowUtw8G%2BaWnYCRrUSzfeLPBYK3QiJZk5jHTfhBp4NEDZBDi01uF%2FUJY%2BexPiNn9dvM0yw69eaNcjvNUSnp2NT55ogvJJzTPRbHsaOmAeOT9YCnuDx32Z9JUnHrh9%2B0d2A82PjvDWkYYXBZXPSqoHkoqzwaIn4v7UvpImNJ%2FLJOFdonzq5eOlJMMl8CL%2B3RUs6slb%2FuAT3vNObNp1IMXrTuExo2YNGMEMpXQHwWVyKFDgK7%2B1D5lmO7zIZSDRL8xQ3havfNPga0mRtjHq6X1cMGRZ5FGi6N31nIvagrxO%2FYy%2Fl1G%2BAuLnUcPX9Yp20FNYwUJfR9hWkmd04pEaC59QWSq3sA4m%2BOmvDtLVKDC9u6UuBE%2FTbGYHXiC0KXm3fSfwwx0Al%2F%2BWs%2F12WdHWLEcM3uLSsKY530%2BkUfRebVIgw9oG8ZxMfjKsbp5nDHNFdbcvtoFQSnZVzseLJJnWmpI9B5BJUhl777nO%2F9M62oj6Gshgb1js4%2FVioSXFIkpg9QzHCQYqsG1hv5s6feTCqzeyd1cNt0pHFTCPDjj5TOsFEb40fA5LR4sgzLRlhEoBBDrBR6D%2BuccdyPuDbOmDSqaARVvrEORc2Lqh4WLJTJ2GBy0irSMp6Z%2BAUcVpSw0i7kc%2FGaqf5CD9XlkRflqmxGFer2FeuIpo8qytvqN1HHlPsDZb5Sn1gcPYgWQ%2BMOgNNB%2B2jZZRYe73uaVfmPO6UFPFiufwDXTUdB3ziR0cQxNAq4CdYQkuHOIrA6l8TjWVxx48LdtPSdXlGhshuBM0a%2Bo%2BzyXI1hObG5oCjeC9AzbN0KwcC9cA3E%2FsnfH%2FqJaexdrj9239g74M%2F2%2BBIlBWCwxSkku%2FadodHBzajZlAFLIimYGwY8TnkX2aBkAeirI99XgKe8DPRnRO3FuhHof3MpIAFllyBbPDxOh7Xm2LIVHO3dXnuvc%2Bo6dbLx%2FmeXAj71KqTk8tpkoxS5A3qKCPGnGGFFWK2EchRviO5B6GlPhwpSw4uCbeY4Boz%2BdfSgXIIDsnFgHGolIKuIde%2BMHLpw7Tb0HTgF4R5N7jAJ4JJu%2FBzna6N%2B5%2BB%2BPJnirgv5urUsjCWesTVgOUYS2wsTM8hU2KuPKBwmksq%2BPka2gfpVW%2Bm%2BcH6%2FcFyZZUSKHahl%2BTFMJBYgsxrNzVUFuXSEpcGwiZA%2FoNa9Dx%2FRagkxGvtonfADT7VlVjH%2FEs3VT%2Bj%2B6nLbnT2lTk5r759r2JwLUNAdxeJP%2BhXL09oOqNyhs0WQfWK8lABiQvekMkRnoMRHwqBGd%2B%2FnjRCXD9MfXshJAnVxXj%2BUbBzSFIth2UF%2BT4eE9vT40IpBJzNg6QI%3D");
                dic.Add("__VIEWSTATEGENERATOR", "288FB3DC");
                dic.Add("__EVENTVALIDATION", "%2BSWxVzGJrl3h43K6L8YC2glqHgCC%2F5zkvTQxv9F4Gxb5mdCPRvgXqMhrPiIKY%2Fnokd98iYxPOqdzhC1w48qirvRQMylNP9NrsZq%2BCvWlFFzYuM4MmXXny9JBsPUb8Kx7KeutyGpSiDVju1PYtLl5w3GQtMgoayeJVeA05Q%3D%3D");
                dic.Add("startdate", beg);
                dic.Add("enddate", end);
                dic.Add("btnresf", "%E6%9F%A5%E8%AF%A2");
                dic.Add("a", "rdoaqi");
                dic.Add("txtCurrentPage", "1");
                string post = Post(url, dic);

                Regex regexArea = new Regex(@"(?<=未经环保部审核。)([\s\S]*?)(?=共)");//构造解析从“实时空气质量状况”到“过去24小时AQI”之间字符串的正则表达式
                MatchCollection mcArea = regexArea.Matches(post);
                foreach (Match mArea in mcArea)
                {
                    Regex regexR = new Regex(@"(?<=<tr>)([\s\S]*?)(?=</tr>)");//构造解析表格行数据的正则表达式
                    MatchCollection mcR = regexR.Matches(mArea.Groups[0].ToString()); //执行匹配
                    int m = 0;
                    foreach (Match mr in mcR)
                    {
                        m++;
                        if (m == 3)
                        {
                            Regex regexD = new Regex(@"(?<=<td[^>]*>[\s]*?)([\S]*)(?=[\s]*?</td>)");
                            MatchCollection mcD = regexD.Matches(mr.Groups[0].ToString()); //执行匹配
                            for (int d = 0; d < mcD.Count; d++)
                            {
                                aqidata += mcD[d].Value + "/";
                            }
                        }
                    }
                }
                string[] aqi = aqidata.Trim('/').Split('/');
                string dt1 = "";
                for (int a = 0; a < aqi.Length; a++)
                {
                    if (a == 0)
                    {
                        dt1 += aqi[a] + ",";
                    }
                    else if (a == 1)
                    {
                        dt1 += "PM2.5_IAQI:" + aqi[a] + ",";
                    }
                    else if (a == 2)
                    {
                        dt1 += "PM10_IAQI:" + aqi[a] + ",";
                    }
                    else if (a == 3)
                    {
                        dt1 += "O3_IAQI:" + aqi[a] + ",";
                    }
                    else if (a == 4)
                    {
                        dt1 += "SO2_IAQI:" + aqi[a] + ",";
                    }
                    else if (a == 5)
                    {
                        dt1 += "NO2_IAQI:" + aqi[a] + ",";
                    }
                    else if (a == 6)
                    {
                        dt1 += "C0_IAQI:" + aqi[a] + ",";
                    }
                    else if (a == 7)
                    {
                        dt1 += "AQI:" + aqi[a] + ",";
                    }
                    else if (a == 8)
                    {
                        dt1 += "Class:" + aqi[a] + ",";
                    }
                    else if (a == 9)
                    {
                        dt1 += "PrimaryPollutant:" + aqi[a];
                    }
                }
                if (dt1 == "AQI:,")
                    return new DataTable();
                else
                {
                    string classA = dt1.Split(',')[8].Split(':')[1].ToString();
                    string grade = string.Empty;
                    string healthEffect = string.Empty;
                    string takeStep = string.Empty;
                    switch (classA)
                    {
                        case "优":
                            grade = "一级";
                            healthEffect = "空气质量令人满意，基本无空气污染";
                            takeStep = "各类人群可正常活动";
                            break;
                        case "良":
                            grade = "二级";
                            healthEffect = "空气质量可接受，但某些污染物可能对极少数异常敏感人群健康有较弱影响";
                            takeStep = "极少数异常敏感人群应减少户外活动";
                            break;
                        case "轻度污染":
                            grade = "三级";
                            healthEffect = "易感人群症状有轻度加剧，健康人群出现刺激症状";
                            takeStep = "儿童、老年人及心脏病、呼吸系统疾病患者应减少长时间、高强度的户外锻炼";
                            break;
                        case "中度污染":
                            grade = "四级";
                            healthEffect = "进一步加剧易感人群症状，可能对 健康人群心脏、呼吸系统有影响";
                            takeStep = "儿童、老年人及心脏病、呼吸系统 疾病患者避免长时间、高强度的户外锻练，一般人群适量减少户外运动";
                            break;
                        case "重度污染":
                            grade = "五级";
                            healthEffect = "心脏病和肺病患者症状显著加剧，运动耐受力降低，健康人群普遍出 现症状";
                            takeStep = "儿童、老年人和心脏病、肺病患者应停留在室内，停止户外运动，一般人群减少户外运动";
                            break;
                        case "严重污染":
                            grade = "六级";
                            healthEffect = "健康人群运动耐受力降低，有明显 强烈症状，提前出现某些疾病";
                            takeStep = "老年人和病人应当留在室内，避免体力消耗，一般人群应避免户外活动";
                            break;
                        default:
                            grade = "--";
                            healthEffect = "--";
                            takeStep = "--";
                            break;
                    }
                    DataTable dtt = new DataTable();
                    dtt.Columns.Add("DateTime", typeof(string));
                    dtt.Columns.Add("AQIValue", typeof(string));
                    dtt.Columns.Add("PrimaryPollutant", typeof(string));
                    dtt.Columns.Add("RGBValue", typeof(string));
                    dtt.Columns.Add("Class", typeof(string));
                    dtt.Columns.Add("Grade", typeof(string));
                    dtt.Columns.Add("PointId", typeof(string));
                    dtt.Columns.Add("PortName", typeof(string));
                    dtt.Columns.Add("HealthEffect", typeof(string));
                    dtt.Columns.Add("TakeStep", typeof(string));
                    DataRow dr = dtt.NewRow();
                    dr["DateTime"] = dt1.Split(',')[0].Split(':')[1].ToString();
                    dr["AQIValue"] = dt1.Split(',')[7].Split(':')[1].ToString();
                    dr["PrimaryPollutant"] = dt1.Split(',')[9].Split(':')[1].ToString();
                    dr["HealthEffect"] = healthEffect;
                    dr["TakeStep"] = takeStep;
                    dr["RGBValue"] = "--";
                    dr["Class"] = classA;
                    dr["Grade"] = grade;
                    dr["PointId"] = "0";
                    dr["PortName"] = "上海市";
                    dtt.Rows.Add(dr);
                    return dtt;
                }
            }catch(Exception ex)
            {
                return new DataTable();
            }
        }


        private static DataTable SHdayValue()
        {
            try
            {
                string rl;
                string url = "http://www.semc.gov.cn/aqi/home/DayData.aspx";
                //string aqidata = "";
                string valuedata = "浓度:";
                DateTime Time = DateTime.Now;
                string beg = Time.AddDays(-1).ToString("yyyy-MM-dd");
                string end = Time.ToString("yyyy-MM-dd");

                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("__EVENTTARGET", "");
                dic.Add("__EVENTARGUMENT", "");
                dic.Add("__LASTFOCUS", "");
                dic.Add("__VIEWSTATE", "bYp3ODmFSoS%2FlXDkqiD1qxve5%2FoAdkylW624jFowUtw8G%2BaWnYCRrUSzfeLPBYK3QiJZk5jHTfhBp4NEDZBDi01uF%2FUJY%2BexPiNn9dvM0yw69eaNcjvNUSnp2NT55ogvJJzTPRbHsaOmAeOT9YCnuDx32Z9JUnHrh9%2B0d2A82PjvDWkYYXBZXPSqoHkoqzwaIn4v7UvpImNJ%2FLJOFdonzq5eOlJMMl8CL%2B3RUs6slb%2FuAT3vNObNp1IMXrTuExo2YNGMEMpXQHwWVyKFDgK7%2B1D5lmO7zIZSDRL8xQ3havfNPga0mRtjHq6X1cMGRZ5FGi6N31nIvagrxO%2FYy%2Fl1G%2BAuLnUcPX9Yp20FNYwUJfR9hWkmd04pEaC59QWSq3sA4m%2BOmvDtLVKDC9u6UuBE%2FTbGYHXiC0KXm3fSfwwx0Al%2F%2BWs%2F12WdHWLEcM3uLSsKY530%2BkUfRebVIgw9oG8ZxMfjKsbp5nDHNFdbcvtoFQSnZVzseLJJnWmpI9B5BJUhl777nO%2F9M62oj6Gshgb1js4%2FVioSXFIkpg9QzHCQYqsG1hv5s6feTCqzeyd1cNt0pHFTCPDjj5TOsFEb40fA5LR4sgzLRlhEoBBDrBR6D%2BuccdyPuDbOmDSqaARVvrEORc2Lqh4WLJTJ2GBy0irSMp6Z%2BAUcVpSw0i7kc%2FGaqf5CD9XlkRflqmxGFer2FeuIpo8qytvqN1HHlPsDZb5Sn1gcPYgWQ%2BMOgNNB%2B2jZZRYe73uaVfmPO6UFPFiufwDXTUdB3ziR0cQxNAq4CdYQkuHOIrA6l8TjWVxx48LdtPSdXlGhshuBM0a%2Bo%2BzyXI1hObG5oCjeC9AzbN0KwcC9cA3E%2FsnfH%2FqJaexdrj9239g74M%2F2%2BBIlBWCwxSkku%2FadodHBzajZlAFLIimYGwY8TnkX2aBkAeirI99XgKe8DPRnRO3FuhHof3MpIAFllyBbPDxOh7Xm2LIVHO3dXnuvc%2Bo6dbLx%2FmeXAj71KqTk8tpkoxS5A3qKCPGnGGFFWK2EchRviO5B6GlPhwpSw4uCbeY4Boz%2BdfSgXIIDsnFgHGolIKuIde%2BMHLpw7Tb0HTgF4R5N7jAJ4JJu%2FBzna6N%2B5%2BB%2BPJnirgv5urUsjCWesTVgOUYS2wsTM8hU2KuPKBwmksq%2BPka2gfpVW%2Bm%2BcH6%2FcFyZZUSKHahl%2BTFMJBYgsxrNzVUFuXSEpcGwiZA%2FoNa9Dx%2FRagkxGvtonfADT7VlVjH%2FEs3VT%2Bj%2B6nLbnT2lTk5r759r2JwLUNAdxeJP%2BhXL09oOqNyhs0WQfWK8lABiQvekMkRnoMRHwqBGd%2B%2FnjRCXD9MfXshJAnVxXj%2BUbBzSFIth2UF%2BT4eE9vT40IpBJzNg6QI%3D");
                dic.Add("__VIEWSTATEGENERATOR", "288FB3DC");
                dic.Add("__EVENTVALIDATION", "%2BSWxVzGJrl3h43K6L8YC2glqHgCC%2F5zkvTQxv9F4Gxb5mdCPRvgXqMhrPiIKY%2Fnokd98iYxPOqdzhC1w48qirvRQMylNP9NrsZq%2BCvWlFFzYuM4MmXXny9JBsPUb8Kx7KeutyGpSiDVju1PYtLl5w3GQtMgoayeJVeA05Q%3D%3D");
                dic.Add("startdate", beg);
                dic.Add("enddate", end);
                dic.Add("btnresf", "%E6%9F%A5%E8%AF%A2");
                dic.Add("a", "rdovalue");
                dic.Add("txtCurrentPage", "1");
                string post = Post(url, dic);

                //string str = post;
                Regex regexArea = new Regex(@"(?<=未经环保部审核。)([\s\S]*?)(?=共)");//构造解析从“实时空气质量状况”到“过去24小时AQI”之间字符串的正则表达式
                MatchCollection mcArea = regexArea.Matches(post);
                foreach (Match mArea in mcArea)
                {
                    Regex regexR = new Regex(@"(?<=<tr>)([\s\S]*?)(?=</tr>)");//构造解析表格行数据的正则表达式
                    MatchCollection mcR = regexR.Matches(mArea.Groups[0].ToString()); //执行匹配
                    int m = 0;
                    foreach (Match mr in mcR)
                    {
                        m++;
                        if (m == 3)
                        {
                            Regex regexD = new Regex(@"(?<=<td[^>]*>[\s]*?)([\S]*)(?=[\s]*?</td>)");
                            MatchCollection mcD = regexD.Matches(mr.Groups[0].ToString()); //执行匹配
                            for (int d = 0; d < mcD.Count; d++)
                            {
                                valuedata += mcD[d].Value + "/";
                            }
                        }
                    }
                }
                string[] value = valuedata.Trim('/').Split('/');
                string dt2 = "";
                for (int b = 0; b < value.Length; b++)
                {
                    if (b == 0)
                    {
                        dt2 += value[b] + ",";
                    }
                    else if (b == 1)
                    {
                        dt2 += "PM2.5:" + value[b] + ",";
                    }
                    else if (b == 2)
                    {
                        dt2 += "PM10:" + value[b] + ",";
                    }
                    else if (b == 3)
                    {
                        dt2 += "O3:" + value[b] + ",";
                    }
                    else if (b == 4)
                    {
                        dt2 += "SO2:" + value[b] + ",";
                    }
                    else if (b == 5)
                    {
                        dt2 += "NO2:" + value[b] + ",";
                    }
                    else if (b == 6)
                    {
                        dt2 += "CO:" + value[b] + ",";
                    }
                }
                if (dt2 == "浓度:,")
                    return new DataTable();
                else
                {
                    DataTable dtt = new DataTable();
                    dtt.Columns.Add("PointId", typeof(string));
                    dtt.Columns.Add("PortName", typeof(string));
                    dtt.Columns.Add("Tstamp", typeof(string));
                    dtt.Columns.Add("PollutantCode", typeof(string));
                    dtt.Columns.Add("PollutantValue", typeof(string));
                    dtt.Columns.Add("factorName", typeof(string));
                    dtt.Columns.Add("factorUnit", typeof(string));
                    string tm = dt2.Split(',')[0].Split(':')[1].ToString();
                    string[] factorys = new string[] { "a34004", "a34002", "a21004", "a21026", "a21005", "a05024" };
                    string factoryname = string.Empty;
                    string factoryvalue = string.Empty;
                    foreach (string f in factorys)
                    {
                        switch (f)
                        {
                            case "a34004":
                                factoryname = "PM2.5";
                                factoryvalue = dt2.Split(',')[1].Split(':')[1].ToString(); ;
                                break;
                            case "a34002":
                                factoryname = "PM10";
                                factoryvalue = dt2.Split(',')[2].Split(':')[1].ToString(); ;
                                break;
                            case "a21004":
                                factoryname = "NO2";
                                factoryvalue = dt2.Split(',')[5].Split(':')[1].ToString(); ;
                                break;
                            case "a21026":
                                factoryname = "SO2";
                                factoryvalue = dt2.Split(',')[4].Split(':')[1].ToString(); ;
                                break;
                            case "a21005":
                                factoryname = "CO";
                                factoryvalue = dt2.Split(',')[6].Split(':')[1].ToString(); ;
                                break;
                            case "a05024":
                                factoryname = "O3";
                                factoryvalue = dt2.Split(',')[3].Split(':')[1].ToString(); ;
                                break;
                        }
                        DataRow dr = dtt.NewRow();
                        dr["PointId"] = "0";
                        dr["PortName"] = "上海市";
                        dr["Tstamp"] = tm;
                        dr["PollutantCode"] = f;
                        if (f == "a21005")
                        {
                            decimal a = 0;
                            dr["PollutantValue"] = Decimal.TryParse(factoryvalue, out a) ? DecimalExtension.GetPollutantValue(Convert.ToDecimal(factoryvalue), 1).ToString() : factoryvalue;
                        }
                        else
                        {
                            dr["PollutantValue"] = factoryvalue;
                        }
                        dr["factorName"] = factoryname;
                        dr["factorUnit"] = f == "a21005" ? "mg/m3" : "μg/m3";
                        dtt.Rows.Add(dr);
                    }
                    return dtt;
                }
            }
            catch (Exception ex)
            {
                return new DataTable();
            }
        }

        public static string Post(string url, Dictionary<string, string> dic)
        {
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            //req.ContentType = "text/xml";
            #region 添加Post 参数
            StringBuilder builder = new StringBuilder();
            int i = 0;
            foreach (var item in dic)
            {
                if (i > 0)
                    builder.Append("&");
                builder.AppendFormat("{0}={1}", item.Key, item.Value);
                i++;
            }
            byte[] data = Encoding.UTF8.GetBytes(builder.ToString());
            req.ContentLength = data.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            #endregion
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            //获取响应内容  
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }
        #endregion
    }
}