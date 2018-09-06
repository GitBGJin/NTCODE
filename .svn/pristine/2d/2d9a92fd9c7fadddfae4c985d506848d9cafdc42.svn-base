using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.Channel;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.DataAnalyze.Air;
using SmartEP.Service.DataAnalyze.Air.AQIReport;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Utilities.Web.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartEP.WebUI.Pages.EnvAir.Chart
{
    /// <summary>
    /// 名称：AirQualityChart.aspx.cs
    /// 创建人：徐阳
    /// 创建日期：2017-06-02
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 评价数据画图
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class AirQualityChart : SmartEP.WebUI.Common.BasePage
    {
        private DayAQIService m_DayAQIService = new DayAQIService();
        private HourAQIService m_HourAQIService = new HourAQIService();
        private MonitoringPointAirService pointAirService = new MonitoringPointAirService();
        private AQICalculateService m_AQICalculateService = new AQICalculateService();
        MonitoringPointAirService monitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bind();
            }
        }

        /// <summary>
        /// 判断是否满足条件，决定是查表还是动态计算
        /// </summary>
        /// <returns></returns>
        private bool CalOrCheck(string[] points)
        {
            if (points.Length != 4)
            {
                return false;
            }
            else
            {
                List<string> list = ConfigurationManager.AppSettings["NTRegionPointId"].ToString().Split(',').ToList<string>();
                for (int i = 0; i < points.Length; i++)
                {
                    if (!list.Contains(points[i]))
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public void bind()
        {
            string pointIds = PageHelper.GetQueryString("pointIds");
            string chartType = PageHelper.GetQueryString("chartType");
            string chartContent = PageHelper.GetQueryString("chartContent");
            string DSType = PageHelper.GetQueryString("DSType");
            string factor = PageHelper.GetQueryString("factor");
            DateTime dtStart = DateTime.TryParse(PageHelper.GetQueryString("dtBegion"), out dtStart) ? dtStart : DateTime.Now.AddMonths(-1);
            DateTime dtEnd = DateTime.TryParse(PageHelper.GetQueryString("dtEnd"), out dtEnd) ? dtEnd : DateTime.Now;

            string flag = PageHelper.GetQueryString("flag");    //实时AQI绘图处理
            if (flag == "rt")
            {
                string isCheck = PageHelper.GetQueryString("isCheck");  //判断是否勾选市辖区选项
                string orderByRT = "PointId,DateTime Asc";
                DateTime dt2 = Convert.ToDateTime(dtStart.ToString("yyyy-MM-dd 00:00:00"));
                //DataView dataViewRT = m_HourAQIService.GetAirQualityOriRTReport(pointIds.Split(','), dt2, dtStart, 99999, 0, out recordTotalRT, orderByRT);
                DataView dataViewRT = m_HourAQIService.GetOriRTData(pointIds.Split(','), dt2, dtStart, orderByRT, isCheck);
                //if (isCheck == "1" && chartContent == "primaryValue")
                //{
                    
                //    int iaqi = 0;
                //    int iaqiMax = 0;
                //    foreach (DataRowView dr in dataViewRT)
                //    {
                //        string primaryPollutant = string.Empty;
                //        dr["DateTime"] = Convert.ToDateTime(Convert.ToDateTime(dr["DateTime"]).ToString("yyyy-MM-dd HH:00:00"));
                //        if (dr["PointId"].ToString() == "999")
                //        {
                //            foreach (DataColumn dc in dataViewRT.Table.Columns)
                //            {
                //                if (dc.ColumnName == "SO2_IAQI" || dc.ColumnName == "NO2_IAQI" || dc.ColumnName == "PM10_IAQI" || dc.ColumnName == "CO_IAQI" || dc.ColumnName == "O3_IAQI" || dc.ColumnName == "PM25_IAQI")
                //                {
                //                    if (dr[dc.ColumnName] != DBNull.Value && Convert.ToInt32(dr[dc.ColumnName].ToString()) > 50)
                //                    {
                //                        iaqi = Convert.ToInt32(dr[dc.ColumnName].ToString());
                //                        if (iaqi >= iaqiMax)
                //                        {
                //                            iaqiMax = iaqi;
                //                        }
                //                    }
                //                }
                //            }
                //            foreach (DataColumn dc in dataViewRT.Table.Columns)
                //            {
                //                if (dc.ColumnName == "SO2" || dc.ColumnName == "NO2" || dc.ColumnName == "PM10" || dc.ColumnName == "CO" || dc.ColumnName == "O3" || dc.ColumnName == "PM25")
                //                {
                //                    if (dr[dc.ColumnName + "_IAQI"] != DBNull.Value && Convert.ToInt32(dr[dc.ColumnName + "_IAQI"].ToString()) == iaqiMax)
                //                    {
                //                        primaryPollutant += dc.ColumnName == "PM25" ? "PM2.5" : dc.ColumnName + ",";
                //                    }
                //                }
                //            }
                //            string pripol = primaryPollutant.TrimEnd(',');
                //            dr["PrimaryPollutant"] = pripol != "" ? pripol : "--";
                //        }
                //    }
                //}
                string pid = string.Empty;
                if (isCheck.Equals("1"))
                {
                    pid = pointIds + ",999";
                }
                else
                {
                    pid = pointIds;
                }
                if (chartContent == "primaryAQI" || chartContent == "primaryValue")
                {
                    string2series(dataViewRT, pid.Split(','), chartContent, "H");
                }
                if (chartContent == "factorValue" || chartContent == "factorIAQI")
                {
                    string2seriesFactor(dataViewRT, pid.Split(','), chartContent, factor, "H");
                }
            }

            //将绘图类型存入隐藏域（折线、柱形）
            hdchartType.Value = chartType;
            //将图表类型存入隐藏域
            hdchartContent.Value = chartContent;

            string[] pointIdArr = pointIds.Split(',');
            string[] qty = { "优", "良", "轻度污染", "中度污染", "重度污染", "严重污染", "无效天" };
            int recordTotal = 0;
            int pageSize = 999999;
            int pageNo = 0;
            string orderBy = "PointId,DateTime Asc";
            DataView dataView = new DataView();

            //测点审核日数据
            if (DSType == "PDayAudit")
            {
                dataView = m_DayAQIService.GetAirQualityDayReportNew(pointIdArr, dtStart, dtEnd, qty, pageSize, pageNo, out recordTotal, orderBy);
                //画AQI的图和首要污染物
                if (chartContent == "primaryAQI" || chartContent == "primaryValue")
                {
                    string2series(dataView, pointIdArr, chartContent, "D");
                }
                //画六参数图
                if (chartContent == "factorValue" || chartContent == "factorIAQI")
                {
                    string2seriesFactor(dataView, pointIdArr, chartContent, factor, "D");
                }
            }
            //测点原始日数据
            if (DSType == "PDayOri")
            {
                dataView = m_DayAQIService.GetAirQualityOriDayReportNew(pointIdArr, dtStart, dtEnd, qty, pageSize, pageNo, out recordTotal, orderBy);
                if (chartContent == "primaryAQI" || chartContent == "primaryValue")
                {
                    string2series(dataView, pointIdArr, chartContent, "D");
                }
                if (chartContent == "factorValue" || chartContent == "factorIAQI")
                {
                    string2seriesFactor(dataView, pointIdArr, chartContent, factor, "D");
                }
            }
            //测点原始小时数据
            if (DSType == "PHourOri")
            {
                orderBy = "PointId,DateTime Asc";
                dataView = m_HourAQIService.GetAirQualityOriRTReport(pointIdArr, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
                if (chartContent == "primaryAQI" || chartContent == "primaryValue")
                {
                    string2series(dataView, pointIdArr, chartContent, "H");
                }
                if (chartContent == "factorValue" || chartContent == "factorIAQI")
                {
                    string2seriesFactor(dataView, pointIdArr, chartContent, factor, "H");
                }
            }
            //测点审核小时数据
            if (DSType == "PHourAudit")
            {
                orderBy = "PointId,DateTime Asc";
                dataView = m_HourAQIService.GetAirQualityRTReport(pointIdArr, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
                if (chartContent == "primaryAQI" || chartContent == "primaryValue")
                {
                    string2series(dataView, pointIdArr, chartContent, "H");
                }
                if (chartContent == "factorValue" || chartContent == "factorIAQI")
                {
                    string2seriesFactor(dataView, pointIdArr, chartContent, factor, "H");
                }
            }
            #region 区域审核小时数据
            if (DSType == "RHourAudit")
            {
                DataView dvRegion = GetRegionByPointId(pointIdArr);
                if (CalOrCheck(pointIdArr))
                {
                    orderBy = "DateTime Desc";
                    dataView = m_HourAQIService.GetAirQualityRegionRTReport(dtStart, dtEnd, pageSize, pageNo, "Audit", out recordTotal, orderBy).AsDataView();
                }
                else
                {
                    dataView = m_AQICalculateService.GetRegionAQI(pointIdArr, dtStart, dtEnd, 1, "2").AsDataView();
                }
                if (chartContent == "primaryAQI" || chartContent == "primaryValue")
                {
                    string2seriesR(dataView, dvRegion, chartContent, "H");
                }
                if (chartContent == "factorValue" || chartContent == "factorIAQI")
                {
                    string2seriesFactor(dataView, dvRegion, chartContent, factor, "H");
                }
            }
            #endregion

            #region 区域原始小时数据
            if (DSType == "RHourOri")
            {
                DataView dvRegion = GetRegionByPointId(pointIdArr);
                if (CalOrCheck(pointIdArr))
                {
                    orderBy = "DateTime Desc";
                    dataView = m_HourAQIService.GetAirQualityRegionRTReport(dtStart, dtEnd, pageSize, pageNo, "Ori", out recordTotal, orderBy).AsDataView();
                }
                else
                {
                    dataView = m_AQICalculateService.GetRegionAQI(pointIdArr, dtStart, dtEnd, 1, "1").AsDataView();
                }
                if (chartContent == "primaryAQI" || chartContent == "primaryValue")
                {
                    string2seriesR(dataView, dvRegion, chartContent, "H");
                }
                if (chartContent == "factorValue" || chartContent == "factorIAQI")
                {
                    string2seriesFactor(dataView, dvRegion, chartContent, factor, "H");
                }
            }
            #endregion

            #region 区域审核日数据
            if (DSType == "RDayAudit")
            {
                DataView dvRegion = GetRegionByPointId(pointIdArr);
                if (CalOrCheck(pointIdArr))
                {
                    string[] typeArr = { "优", "良", "轻度污染", "中度污染", "重度污染", "严重污染", "无效天" };
                    orderBy = "DateTime Desc";
                    dataView = m_DayAQIService.GetAirQualityRegionDayReportNew(dtStart, dtEnd, typeArr, pageSize, pageNo, "Audit", out recordTotal, orderBy);
                }
                else
                {
                    dataView = m_AQICalculateService.GetRegionAQI(pointIdArr, dtStart, dtEnd, 24, "2").AsDataView();
                }
                if (chartContent == "primaryAQI" || chartContent == "primaryValue")
                {
                    string2seriesR(dataView, dvRegion, chartContent, "D");
                }
                if (chartContent == "factorValue" || chartContent == "factorIAQI")
                {
                    string2seriesFactor(dataView, dvRegion, chartContent, factor, "D");
                }
            }
            #endregion

            #region 区域原始日数据
            if (DSType == "RDayOri")
            {
                DataView dvRegion = GetRegionByPointId(pointIdArr);
                if (CalOrCheck(pointIdArr))
                {
                    string[] typeArr = { "优", "良", "轻度污染", "中度污染", "重度污染", "严重污染", "无效天" };
                    orderBy = "DateTime Desc";
                    dataView = m_DayAQIService.GetAirQualityRegionDayReportNew(dtStart, dtEnd, typeArr, pageSize, pageNo, "Ori", out recordTotal, orderBy);
                }
                else
                {
                    dataView = m_AQICalculateService.GetRegionAQI(pointIdArr, dtStart, dtEnd, 24, "1").AsDataView();
                }
                if (chartContent == "primaryAQI" || chartContent == "primaryValue")
                {
                    string2seriesR(dataView, dvRegion, chartContent, "D");
                }
                if (chartContent == "factorValue" || chartContent == "factorIAQI")
                {
                    string2seriesFactor(dataView, dvRegion, chartContent, factor, "D");
                }
            }
            #endregion
        }

        #region 数据转换成json数据(因子)
        /// <summary>
        /// 测点数据转换成json数据
        /// </summary>
        /// <param name="dv"></param>
        /// <param name="pointids"></param>
        public void string2seriesFactor(DataView dataView, string[] pointIdArr, string chartContent, string factor, string dayOrHour)
        {
            AirPollutantService m_AirPollutantService = new AirPollutantService();
            MonitoringPointEntity monitorPoint = new MonitoringPointEntity();
            string pollutantCode = "";
            int DecimalNum = -999;
            if (chartContent == "factorValue")
            {
                string title = string.Empty;
                if (factor == "PM25")
                {
                    title = "PM2.5浓度值";
                }
                else if (factor == "Max8HourO3")
                {
                    title = "臭氧最大8小时浓度值";
                }
                else
                {
                    title = factor + "浓度值";
                }
                hdtitle.Value = title;
                string unit = "";
                switch (factor)
                {
                    case "PM25":
                        pollutantCode = "a34004";
                        break;
                    case "PM10":
                        pollutantCode = "a34002";
                        break;
                    case "NO2":
                        pollutantCode = "a21004";
                        break;
                    case "SO2":
                        pollutantCode = "a21026";
                        break;
                    case "CO":
                        pollutantCode = "a21005";
                        break;
                    case "O3":
                        pollutantCode = "a05024";
                        break;
                    case "Max8HourO3":
                        pollutantCode = "a05024";
                        break;

                }

                DecimalNum = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo(pollutantCode).PollutantDecimalNum);
                unit = m_AirPollutantService.GetPollutantInfo(pollutantCode).PollutantMeasureUnit;
                hdunit.Value = unit;
            }
            if (chartContent == "factorIAQI")
            {
                string title = string.Empty;
                if (factor == "PM25_IAQI")
                {
                    title = "PM2.5_IAQI分指数";
                }
                else if (factor == "Max8HourO3_IAQI")
                {
                    title = "臭氧最大8小时_IAQI分指数";
                }
                else
                {
                    title = factor + "分指数";
                }
                hdtitle.Value = title;
            }
            string data = "[";
            for (int j = 0; j < pointIdArr.Length; j++)
            {
                string pointId = pointIdArr[j];
                dataView.RowFilter = "PointId='" + pointIdArr[j] + "'";
                data += "{";
                if (pointId.Equals("999"))
                {
                    data += string.Format(" name: '平均值',data:[ ");
                }
                else
                {
                    monitorPoint = monitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(pointId));
                    data += string.Format(" name: '{0}',data:[ ", monitorPoint.MonitoringPointName);
                }
                int m = 0;
                foreach (DataRowView drv in dataView)
                {
                    m++;
                    DateTime tstamp = new DateTime();
                    tstamp = Convert.ToDateTime(Convert.ToDateTime(drv["DateTime"]).ToString("yyyy-MM-dd HH:00:00"));
                    string time = string.Empty;
                    if (dayOrHour == "D")
                    {
                        time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                    }
                    if (dayOrHour == "H")
                    {
                        time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                    }
                    if (m != dataView.Count)
                    {
                        if (drv[factor] != DBNull.Value && drv[factor].ToString().Trim() != "")
                        {
                            if (chartContent == "factorValue")
                            {
                                if (factor == "CO")
                                {
                                    data += "[" + time + "," + DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv[factor]), DecimalNum).ToString() + "],";
                                }
                                else
                                {
                                    data += "[" + time + "," + (DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv[factor]), DecimalNum) * 1000).ToString("G0") + "],";
                                }
                            }
                            else
                            {
                                data += "[" + time + "," + drv[factor].ToString() + "],";
                            }
                        }
                        else
                        {
                            data += "[" + time + ",null],";
                        }
                    }
                    else
                    {
                        if (drv[factor] != DBNull.Value && drv[factor].ToString().Trim() != "")
                        {
                            if (chartContent == "factorValue")
                            {
                                if (factor == "CO")
                                {
                                    data += "[" + time + "," + DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv[factor]), DecimalNum).ToString() + "],";
                                }
                                else
                                {
                                    data += "[" + time + "," + (DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv[factor]), DecimalNum) * 1000).ToString("G0") + "],";
                                }
                            }
                            else
                            {
                                data += "[" + time + "," + drv[factor].ToString() + "],";
                            }
                        }
                        else
                        {
                            data += "[" + time + ",null]";
                        }
                    }

                }
                data += "]},";
            }
            if (data != "")
            {
                data = data.Substring(0, data.Length - 1);
            }
            data += "]";
            hdjsonData.Value = data;

            RegisterScript("generate();");
        }
        #endregion

        #region 区域数据转换成json数据(因子)
        /// <summary>
        /// 区域数据转换成json数据
        /// </summary>
        /// <param name="dv"></param>
        /// <param name="pointids"></param>
        public void string2seriesFactor(DataView dataView, DataView dvR, string chartContent, string factor, string dayOrHour)
        {
            string pollutantCode = "";
            AirPollutantService m_AirPollutantService = new AirPollutantService();
            int DecimalNum = -999;
            string unit = "";
            if (chartContent == "factorValue")
            {
                string title = string.Empty;
                if (factor == "PM25")
                {
                    title = "PM2.5浓度值";
                }
                else if (factor == "Max8HourO3")
                {
                    title = "臭氧最大8小时浓度值";
                }
                else
                {
                    title = factor + "浓度值";
                }
                hdtitle.Value = title;

                switch (factor)
                {
                    case "PM25":
                        pollutantCode = "a34004";
                        break;
                    case "PM10":
                        pollutantCode = "a34002";
                        break;
                    case "NO2":
                        pollutantCode = "a21004";
                        break;
                    case "SO2":
                        pollutantCode = "a21026";
                        break;
                    case "CO":
                        pollutantCode = "a21005";
                        break;
                    case "O3":
                        pollutantCode = "a05024";
                        break;
                    case "Max8HourO3":
                        pollutantCode = "a05024";
                        break;
                }
                DecimalNum = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo(pollutantCode).PollutantDecimalNum);
                unit = m_AirPollutantService.GetPollutantInfo(pollutantCode).PollutantMeasureUnit;
                hdunit.Value = unit;
            }
            if (chartContent == "factorIAQI")
            {
                string title = string.Empty;
                if (factor == "PM25_IAQI")
                {
                    title = "PM2.5_IAQI分指数";
                }
                else if (factor == "Max8HourO3_IAQI")
                {
                    title = "臭氧最大8小时_IAQI分指数";
                }
                else
                {
                    title = factor + "分指数";
                }
                hdtitle.Value = title;
            }
            List<string> regionName = dvR.ToTable().AsEnumerable().Select(t => t.Field<string>("Region")).ToList();
            IEnumerable<string> names = regionName.Distinct();

            string data = "[";
            foreach (string name in names)
            {
                dataView.RowFilter = "PointId='" + name + "'";
                data += "{";
                data += string.Format(" name: '{0}',data:[ ", name);
                int m = 0;
                foreach (DataRowView drv in dataView)
                {
                    m++;
                    DateTime tstamp = Convert.ToDateTime(Convert.ToDateTime(drv["DateTime"]).ToString("yyyy-MM-dd HH:00:00"));
                    string time = string.Empty;
                    if (dayOrHour == "D")
                    {
                        time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                    }
                    if (dayOrHour == "H")
                    {
                        time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                    }
                    if (m != dataView.Count)
                    {
                        if (drv[factor] != DBNull.Value && drv[factor].ToString().Trim() != "")
                        {
                            if (chartContent == "factorIAQI")
                            {
                                data += "[" + time + "," + drv[factor].ToString() + "],";
                            }
                            else
                            {
                                if (factor == "CO")
                                {
                                    data += "[" + time + "," + DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv[factor]), DecimalNum).ToString() + "],";
                                }
                                else
                                {
                                    data += "[" + time + "," + (DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv[factor]), DecimalNum) * 1000).ToString("G0") + "],";
                                }
                            }
                        }
                        else
                        {
                            data += "[" + time + ",null],";
                        }
                    }
                    else
                    {
                        if (drv[factor] != DBNull.Value && drv[factor].ToString().Trim() != "")
                        {
                            if (chartContent == "factorIAQI")
                            {
                                data += "[" + time + "," + drv[factor].ToString() + "],";
                            }
                            else
                            {
                                if (factor == "CO")
                                {
                                    data += "[" + time + "," + DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv[factor]), DecimalNum).ToString() + "],";
                                }
                                else
                                {
                                    data += "[" + time + "," + (DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv[factor]), DecimalNum) * 1000).ToString("G0") + "],";
                                }
                            }
                        }
                        else
                        {
                            data += "[" + time + ",null]";
                        }
                    }

                }
                data += "]},";
            }
            if (data != "")
            {
                data = data.Substring(0, data.Length - 1);
            }
            data += "]";
            hdjsonData.Value = data;

            RegisterScript("generate();");
        }
        #endregion

        #region 区域数据转换成json数据
        /// <summary>
        /// 区域数据转换成json数据
        /// </summary>
        /// <param name="dv"></param>
        /// <param name="pointids"></param>
        public void string2seriesR(DataView dataView, DataView dvR, string chartContent, string dayOrHour)
        {
            AirPollutantService m_AirPollutantService = new AirPollutantService();
            List<string> regionName = dvR.ToTable().AsEnumerable().Select(t => t.Field<string>("Region")).ToList();
            IEnumerable<string> names = regionName.Distinct();

            if (chartContent == "primaryAQI")
            {
                string data = "[";
                foreach (string name in names)
                {
                    dataView.RowFilter = "PointId='" + name + "'";
                    data += "{";
                    data += string.Format(" name: '{0}',data:[ ", name);
                    int m = 0;
                    foreach (DataRowView drv in dataView)
                    {
                        m++;
                        //DateTime tstamp = Convert.ToDateTime(drv["DateTime"]);
                        DateTime tstamp = Convert.ToDateTime(Convert.ToDateTime(drv["DateTime"]).ToString("yyyy-MM-dd HH:00:00"));
                        string time = string.Empty;
                        if (dayOrHour == "D")
                        {
                            time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                        }
                        if (dayOrHour == "H")
                        {
                            time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                        }
                        if (m != dataView.Count)
                        {
                            if (drv["AQIValue"] != DBNull.Value && drv["AQIValue"].ToString().Trim() != "")
                            {
                                data += "[" + time + "," + drv["AQIValue"].ToString() + "],";
                            }
                            else
                            {
                                data += "[" + time + ",null],";
                            }
                        }
                        else
                        {
                            if (drv["AQIValue"] != DBNull.Value && drv["AQIValue"].ToString().Trim() != "")
                            {
                                data += "[" + time + "," + drv["AQIValue"].ToString() + "],";
                            }
                            else
                            {
                                data += "[" + time + ",null]";
                            }
                        }

                    }
                    data += "]},";
                }
                if (data != "")
                {
                    data = data.Substring(0, data.Length - 1);
                }
                data += "]";
                hdjsonData.Value = data;
            }
            if (chartContent == "primaryValue")
            {
                string data = "[";
                foreach (string name in names)
                {
                    dataView.RowFilter = "PointId='" + name + "'";
                    data += "{";
                    data += string.Format(" name: '{0}',data:[ ", name);
                    int m = 0;
                    foreach (DataRowView drv in dataView)
                    {
                        m++;
                        //DateTime tstamp = Convert.ToDateTime(drv["DateTime"]);
                        DateTime tstamp = Convert.ToDateTime(Convert.ToDateTime(drv["DateTime"]).ToString("yyyy-MM-dd HH:00:00"));
                        string time = string.Empty;
                        if (dayOrHour == "D")
                        {
                            time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                        }
                        if (dayOrHour == "H")
                        {
                            time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                        }
                        if (m != dataView.Count)
                        {
                            if (drv["PrimaryPollutant"].ToString() != "--" && drv["PrimaryPollutant"] != DBNull.Value)
                            {
                                string pri = drv["PrimaryPollutant"].ToString();
                                if (pri.Contains(','))
                                {
                                    string[] priAll = pri.Split(',');
                                    for (int i = 0; i < priAll.Length; i++)
                                    {
                                        string prif = priAll[i].ToString();
                                        string timeI = string.Empty;
                                        if (dayOrHour == "D")
                                        {
                                            timeI = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                                        }
                                        if (dayOrHour == "H")
                                        {
                                            timeI = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.AddMinutes(i).Year, tstamp.AddMinutes(i).Month - 1, tstamp.AddMinutes(i).Day, tstamp.AddMinutes(i).Hour, tstamp.AddMinutes(i).Minute, tstamp.AddMinutes(i).Second, tstamp.AddMinutes(i).Millisecond);
                                        }
                                        if (prif == "O3")
                                        {
                                            if (dataView.Table.Columns.Contains("Max8HourO3"))
                                            {
                                                int DecimalNum = GetDecimalNum("O3");
                                                if (drv["Max8HourO3"] != null && drv["Max8HourO3"].ToString().Trim() != "")
                                                    data += "{x:" + timeI + ",y:" + (DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv["Max8HourO3"]), DecimalNum) * 1000).ToString("G0") + ",u:'" + GetUnit("O3") + "',z:'" + prif + "'},";
                                            }
                                            if (dataView.Table.Columns.Contains("O3"))
                                            {
                                                int DecimalNum = GetDecimalNum("O3");
                                                if (drv["O3"] != null && drv["O3"].ToString().Trim() != "")
                                                    data += "{x:" + timeI + ",y:" + (DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv["O3"]), DecimalNum) * 1000).ToString("G0") + ",u:'" + GetUnit("O3") + "',z:'" + prif + "'},";
                                            }
                                        }
                                        else if (prif == "PM2.5")
                                        {
                                            int DecimalNum = GetDecimalNum("PM25");
                                            if (drv["PM25"] != null && drv["PM25"].ToString().Trim() != "")
                                                data += "{x:" + timeI + ",y:" + (DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv["PM25"]), DecimalNum) * 1000).ToString("G0") + ",u:'" + GetUnit("PM25") + "',z:'" + prif + "'},";
                                        }
                                        else
                                        {
                                            if (prif == "CO")
                                            {
                                                int DecimalNum = GetDecimalNum("CO");
                                                if (drv[prif] != null && drv[prif].ToString().Trim() != "")
                                                    data += "{x:" + timeI + ",y:" + (DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv[prif]), DecimalNum)).ToString() + GetUnit("CO") + ",u:'" + GetUnit(prif) + "',z:'" + prif + "'},";
                                            }
                                            else
                                            {
                                                int DecimalNum = GetDecimalNum(prif);
                                                if (drv[prif] != null && drv[prif].ToString().Trim() != "")
                                                    data += "{x:" + timeI + ",y:" + (DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv[prif]), DecimalNum) * 1000).ToString("G0") + ",u:'" + GetUnit(prif) + "',z:'" + prif + "'},";
                                            }
                                        }
                                    }
                                }
                                else if (pri == "O3")
                                {
                                    if (dataView.Table.Columns.Contains("Max8HourO3"))
                                    {
                                        int DecimalNum = GetDecimalNum("O3");
                                        if (drv["Max8HourO3"] != null && drv["Max8HourO3"].ToString().Trim() != "")
                                            data += "{x:" + time + ",y:" + (DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv["Max8HourO3"]), DecimalNum) * 1000).ToString("G0") + ",u:'" + GetUnit("O3") + "',z:'" + pri + "'},";
                                    }
                                    if (dataView.Table.Columns.Contains("O3"))
                                    {
                                        int DecimalNum = GetDecimalNum("O3");
                                        if (drv["O3"] != null && drv["O3"].ToString().Trim() != "")
                                            data += "{x:" + time + ",y" + (DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv["O3"]), DecimalNum) * 1000).ToString("G0") + ",u:'" + GetUnit("O3") + "',z:'" + pri + "'},";
                                    }
                                }
                                else if (pri == "PM2.5")
                                {
                                    int DecimalNum = GetDecimalNum("PM25");
                                    if (drv["PM25"] != null && drv["PM25"].ToString().Trim() != "")
                                        data += "{x:" + time + ",y:" + (DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv["PM25"]), DecimalNum) * 1000).ToString("G0") + ",u:'" + GetUnit("PM25") + "',z:'" + pri + "'},";
                                }
                                else
                                {
                                    if (pri == "CO")
                                    {
                                        int DecimalNum = GetDecimalNum("CO");
                                        if (drv[pri] != null && drv[pri].ToString().Trim() != "")
                                            data += "{x:" + time + ",y:" + (DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv[pri]), DecimalNum)).ToString() + ",u:'" + GetUnit(pri) + "',z:'" + pri + "'},";
                                    }
                                    else
                                    {
                                        int DecimalNum = GetDecimalNum(pri);
                                        if (drv[pri] != null && drv[pri].ToString().Trim() != "")
                                            data += "{x:" + time + ",y:" + (DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv[pri]), DecimalNum) * 1000).ToString("G0") + ",u:'" + GetUnit(pri) + "',z:'" + pri + "'},";
                                    }
                                }
                            }
                            else
                            {
                                data += "[" + time + ",null],";
                            }
                        }
                        else
                        {
                            if (drv["PrimaryPollutant"].ToString() != "--" && drv["PrimaryPollutant"] != DBNull.Value)
                            {
                                string pri = drv["PrimaryPollutant"].ToString();
                                if (pri.Contains(','))
                                {
                                    string[] priAll = pri.Split(',');
                                    for (int i = 0; i < priAll.Length; i++)
                                    {
                                        string prif = priAll[i].ToString();
                                        string timeI = string.Empty;
                                        if (dayOrHour == "D")
                                        {
                                            timeI = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                                        }
                                        if (dayOrHour == "H")
                                        {
                                            timeI = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.AddMinutes(i).Year, tstamp.AddMinutes(i).Month - 1, tstamp.AddMinutes(i).Day, tstamp.AddMinutes(i).Hour, tstamp.AddMinutes(i).Minute, tstamp.AddMinutes(i).Second, tstamp.AddMinutes(i).Millisecond);
                                        }
                                        if (prif == "O3")
                                        {
                                            if (dataView.Table.Columns.Contains("Max8HourO3"))
                                            {
                                                int DecimalNum = GetDecimalNum("O3");
                                                if (drv["Max8HourO3"] != null && drv["Max8HourO3"].ToString().Trim() != "")
                                                    data += "{x:" + timeI + ",y:" + (DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv["Max8HourO3"]), DecimalNum) * 1000).ToString("G0") + ",u:'" + GetUnit("O3") + "',z:'" + prif + "'},";
                                            }
                                            if (dataView.Table.Columns.Contains("O3"))
                                            {
                                                int DecimalNum = GetDecimalNum("O3");
                                                if (drv["O3"] != null && drv["O3"].ToString().Trim() != "")
                                                    data += "{x:" + timeI + ",y:" + (DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv["O3"]), DecimalNum) * 1000).ToString("G0") + ",u:'" + GetUnit("O3") + "',z:'" + prif + "'},";
                                            }
                                        }
                                        else if (prif == "PM2.5")
                                        {
                                            int DecimalNum = GetDecimalNum("PM25");
                                            if (drv["PM25"] != null && drv["PM25"].ToString().Trim() != "")
                                                data += "{x:" + timeI + ",y:" + (DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv["PM25"]), DecimalNum) * 1000).ToString("G0") + ",u:'" + GetUnit("PM25") + "',z:'" + prif + "'},";
                                        }
                                        else
                                        {
                                            if (prif == "CO")
                                            {
                                                int DecimalNum = GetDecimalNum("CO");
                                                if (drv[prif] != null && drv[prif].ToString().Trim() != "")
                                                    data += "{x:" + timeI + ",y:" + (DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv[prif]), DecimalNum)).ToString() + GetUnit("CO") + ",u:'" + GetUnit(prif) + "',z:'" + prif + "'},";
                                            }
                                            else
                                            {
                                                int DecimalNum = GetDecimalNum(prif);
                                                if (drv[prif] != null && drv[prif].ToString().Trim() != "")
                                                    data += "{x:" + timeI + ",y:" + (DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv[prif]), DecimalNum) * 1000).ToString("G0") + ",u:'" + GetUnit(prif) + "',z:'" + prif + "'},";
                                            }
                                        }
                                    }
                                }
                                else if (pri == "O3")
                                {
                                    if (dataView.Table.Columns.Contains("Max8HourO3"))
                                    {
                                        int DecimalNum = GetDecimalNum("O3");
                                        if (drv["Max8HourO3"] != null && drv["Max8HourO3"].ToString().Trim() != "")
                                            data += "{x:" + time + ",y:" + (DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv["Max8HourO3"]), DecimalNum) * 1000).ToString("G0") + ",u:'" + GetUnit("O3") + "',z:'" + pri + "'},";
                                    }
                                    if (dataView.Table.Columns.Contains("O3"))
                                    {
                                        int DecimalNum = GetDecimalNum("O3");
                                        if (drv["O3"] != null && drv["O3"].ToString().Trim() != "")
                                            data += "{x:" + time + ",y:" + (DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv["O3"]), DecimalNum) * 1000).ToString("G0") + ",u:'" + GetUnit("O3") + "',z:'" + pri + "'},";
                                    }
                                }
                                else if (pri == "PM2.5")
                                {
                                    int DecimalNum = GetDecimalNum("PM25");
                                    if (drv["PM25"] != null && drv["PM25"].ToString().Trim() != "")
                                        data += "{x:" + time + ",y:" + (DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv["PM25"]), DecimalNum) * 1000).ToString("G0") + ",u:'" + GetUnit("PM25") + "',z:'" + pri + "'},";
                                }
                                else
                                {
                                    if (pri == "CO")
                                    {
                                        int DecimalNum = GetDecimalNum("CO");
                                        if (drv[pri] != null && drv[pri].ToString().Trim() != "")
                                            data += "{x:" + time + ",y:" + (DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv[pri]), DecimalNum)).ToString() + ",u:'" + GetUnit(pri) + "',z:'" + pri + "'},";
                                    }
                                    else
                                    {
                                        int DecimalNum = GetDecimalNum(pri);
                                        if (drv[pri] != null && drv[pri].ToString().Trim() != "")
                                            data += "{x:" + time + ",y:" + (DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv[pri]), DecimalNum) * 1000).ToString("G0") + ",u:'" + GetUnit(pri) + "',z:'" + pri + "'},";
                                    }
                                }
                            }
                            else
                            {
                                data += "[" + time + ",null]";
                            }
                        }

                    }
                    data += "]},";
                }
                if (data != "")
                {
                    data = data.Substring(0, data.Length - 1);
                }
                data += "]";
                hdjsonData.Value = data;
            }

            RegisterScript("generate();");
        }
        #endregion

        #region 测点数据转换成json数据
        /// <summary>
        /// 测点数据转换成json数据
        /// </summary>
        /// <param name="dv"></param>
        /// <param name="pointids"></param>
        public void string2series(DataView dataView, string[] pointIdArr, string chartContent, string dayOrHour)
        {
            MonitoringPointEntity monitorPoint = new MonitoringPointEntity();
            if (chartContent == "primaryAQI")
            {
                string data = "[";
                for (int j = 0; j < pointIdArr.Length; j++)
                {
                    string pointId = pointIdArr[j];
                    dataView.RowFilter = "PointId='" + pointIdArr[j] + "'";
                    data += "{";
                    if (pointId.Equals("999"))
                    {
                        data += string.Format(" name: '平均值',data:[ ");
                    }
                    else
                    {
                        monitorPoint = monitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(pointId));
                        data += string.Format(" name: '{0}',data:[ ", monitorPoint.MonitoringPointName);
                    }
                    int m = 0;
                    foreach (DataRowView drv in dataView)
                    {
                        m++;
                        //DateTime tstamp = new DateTime();
                        //tstamp = Convert.ToDateTime(drv["DateTime"]);
                        DateTime tstamp = Convert.ToDateTime(Convert.ToDateTime(drv["DateTime"]).ToString("yyyy-MM-dd HH:00:00"));
                        string time = string.Empty;
                        if (dayOrHour == "D")
                        {
                            time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                        }
                        if (dayOrHour == "H")
                        {
                            time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                        }
                        if (m != dataView.Count)
                        {
                            if (drv["AQIValue"] != DBNull.Value && drv["AQIValue"].ToString().Trim() != "")
                            {
                                data += "[" + time + "," + drv["AQIValue"].ToString() + "],";
                            }
                            else
                            {
                                data += "[" + time + ",null],";
                            }
                        }
                        else
                        {
                            if (drv["AQIValue"] != DBNull.Value && drv["AQIValue"].ToString().Trim() != "")
                            {
                                data += "[" + time + "," + drv["AQIValue"].ToString() + "],";
                            }
                            else
                            {
                                data += "[" + time + ",null]";
                            }
                        }

                    }
                    data += "]},";
                }
                if (data != "")
                {
                    data = data.Substring(0, data.Length - 1);
                }
                data += "]";
                hdjsonData.Value = data;
            }
            if (chartContent == "primaryValue")
            {
                string data = "[";
                for (int j = 0; j < pointIdArr.Length; j++)
                {
                    string pointId = pointIdArr[j];
                    //MonitoringPointEntity monitorPoint = monitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(pointId));
                    dataView.RowFilter = "PointId='" + pointIdArr[j] + "'";
                    data += "{";
                    if (pointId.Equals("999"))
                    {
                        data += string.Format(" name: '平均值',data:[ ");
                    }
                    else
                    {
                        monitorPoint = monitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(pointId));
                        data += string.Format(" name: '{0}',data:[ ", monitorPoint.MonitoringPointName);
                    }
                    int m = 0;
                    foreach (DataRowView drv in dataView)
                    {
                        m++;
                        //DateTime tstamp = new DateTime();
                        //tstamp = Convert.ToDateTime(drv["DateTime"]);
                        DateTime tstamp = Convert.ToDateTime(Convert.ToDateTime(drv["DateTime"]).ToString("yyyy-MM-dd HH:00:00"));
                        string time = string.Empty;
                        if (dayOrHour == "D")
                        {
                            time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                        }
                        if (dayOrHour == "H")
                        {
                            time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                        }
                        if (m != dataView.Count)
                        {
                            if (drv["PrimaryPollutant"].ToString() != "--" && drv["PrimaryPollutant"] != DBNull.Value)
                            {
                                string pri = drv["PrimaryPollutant"].ToString();
                                if (pri.Contains(','))
                                {
                                    string[] priAll = pri.Split(',');
                                    for (int i = 0; i < priAll.Length; i++)
                                    {
                                        string prif = priAll[i].ToString();
                                        string timeI = string.Empty;
                                        if (dayOrHour == "D")
                                        {
                                            timeI = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                                        }
                                        if (dayOrHour == "H")
                                        {
                                            timeI = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.AddMinutes(i).Year, tstamp.AddMinutes(i).Month - 1, tstamp.AddMinutes(i).Day, tstamp.AddMinutes(i).Hour, tstamp.AddMinutes(i).Minute, tstamp.AddMinutes(i).Second, tstamp.AddMinutes(i).Millisecond);
                                        }
                                        if (prif == "O3")
                                        {
                                            if (dataView.Table.Columns.Contains("Max8HourO3"))
                                            {
                                                int DecimalNum = GetDecimalNum("O3");
                                                if (drv["Max8HourO3"] != null && drv["Max8HourO3"].ToString().Trim() != "")
                                                    data += "{x:" + timeI + ",y:" + (DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv["Max8HourO3"]), DecimalNum) * 1000).ToString("G0") + ",u:'" + GetUnit("O3") + "',z:'" + prif + "'},";
                                            }
                                            if (dataView.Table.Columns.Contains("O3"))
                                            {
                                                int DecimalNum = GetDecimalNum("O3");
                                                if (drv["O3"] != null && drv["O3"].ToString().Trim() != "")
                                                    data += "{x:" + timeI + ",y:" + (DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv["O3"]), DecimalNum) * 1000).ToString("G0") + ",u:'" + GetUnit("O3") + "',z:'" + prif + "'},";
                                            }
                                        }
                                        else if (prif == "PM2.5")
                                        {
                                            int DecimalNum = GetDecimalNum("PM25");
                                            if (drv["PM25"] != null && drv["PM25"].ToString().Trim() != "")
                                                data += "{x:" + timeI + ",y:" + (DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv["PM25"]), DecimalNum) * 1000).ToString("G0") + ",u:'" + GetUnit("PM25") + "',z:'" + prif + "'},";
                                        }
                                        else
                                        {
                                            if (prif == "CO")
                                            {
                                                int DecimalNum = GetDecimalNum("CO");
                                                if (drv[prif] != null && drv[prif].ToString().Trim() != "")
                                                    data += "{x:" + timeI + ",y:" + (DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv[prif]), DecimalNum)).ToString() + GetUnit("CO") + ",u:'" + GetUnit(prif) + "',z:'" + prif + "'},";
                                            }
                                            else
                                            {
                                                int DecimalNum = GetDecimalNum(prif);
                                                if (drv[prif] != null && drv[prif].ToString().Trim() != "")
                                                    data += "{x:" + timeI + ",y:" + (DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv[prif]), DecimalNum) * 1000).ToString("G0") + ",u:'" + GetUnit(prif) + "',z:'" + prif + "'},";
                                            }
                                        }
                                    }
                                }
                                else if (pri == "O3")
                                {
                                    if (dataView.Table.Columns.Contains("Max8HourO3"))
                                    {
                                        int DecimalNum = GetDecimalNum("O3");
                                        if (drv["Max8HourO3"] != null && drv["Max8HourO3"].ToString().Trim() != "")
                                            data += "{x:" + time + ",y:" + (DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv["Max8HourO3"]), DecimalNum) * 1000).ToString("G0") + ",u:'" + GetUnit("O3") + "',z:'" + pri + "'},";
                                    }
                                    if (dataView.Table.Columns.Contains("O3"))
                                    {
                                        int DecimalNum = GetDecimalNum("O3");
                                        if (drv["O3"] != null && drv["O3"].ToString().Trim() != "")
                                            data += "{x:" + time + ",y:" + (DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv["O3"]), DecimalNum) * 1000).ToString("G0") + ",u:'" + GetUnit("O3") + "',z:'" + pri + "'},";
                                    }
                                }
                                else if (pri == "PM2.5")
                                {
                                    int DecimalNum = GetDecimalNum("PM25");
                                    if (drv["PM25"] != null && drv["PM25"].ToString().Trim() != "")
                                        data += "{x:" + time + ",y:" + (DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv["PM25"]), DecimalNum) * 1000).ToString("G0") + ",u:'" + GetUnit("PM25") + "',z:'" + pri + "'},";
                                }
                                else
                                {
                                    if (pri == "CO")
                                    {
                                        int DecimalNum = GetDecimalNum("CO");
                                        if (drv[pri] != null && drv[pri].ToString().Trim() != "")
                                            data += "{x:" + time + ",y:" + (DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv[pri]), DecimalNum)).ToString() + ",u:'" + GetUnit(pri) + "',z:'" + pri + "'},";
                                    }
                                    else
                                    {
                                        int DecimalNum = GetDecimalNum(pri);
                                        if (drv[pri] != null && drv[pri].ToString().Trim() != "")
                                            data += "{x:" + time + ",y:" + (DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv[pri]), DecimalNum) * 1000).ToString("G0") + ",u:'" + GetUnit(pri) + "',z:'" + pri + "'},";
                                    }
                                }
                            }
                            else
                            {
                                data += "[" + time + ",null],";
                            }
                        }
                        else
                        {
                            if (drv["PrimaryPollutant"].ToString() != "--" && drv["PrimaryPollutant"] != DBNull.Value)
                            {
                                string pri = drv["PrimaryPollutant"].ToString();
                                if (pri.Contains(','))
                                {
                                    string[] priAll = pri.Split(',');
                                    for (int i = 0; i < priAll.Length; i++)
                                    {
                                        string prif = priAll[i].ToString();
                                        string timeI = string.Empty;
                                        if (dayOrHour == "D")
                                        {
                                            timeI = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                                        }
                                        if (dayOrHour == "H")
                                        {
                                            timeI = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.AddMinutes(i).Year, tstamp.AddMinutes(i).Month - 1, tstamp.AddMinutes(i).Day, tstamp.AddMinutes(i).Hour, tstamp.AddMinutes(i).Minute, tstamp.AddMinutes(i).Second, tstamp.AddMinutes(i).Millisecond);
                                        }
                                        if (prif == "O3")
                                        {
                                            if (dataView.Table.Columns.Contains("Max8HourO3"))
                                            {
                                                int DecimalNum = GetDecimalNum("O3");
                                                if (drv["Max8HourO3"] != null && drv["Max8HourO3"].ToString().Trim() != "")
                                                    data += "{x:" + timeI + ",y:" + (DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv["Max8HourO3"]), DecimalNum) * 1000).ToString("G0") + ",u:'" + GetUnit("O3") + "',z:'" + prif + "'},";
                                            }
                                            if (dataView.Table.Columns.Contains("O3"))
                                            {
                                                int DecimalNum = GetDecimalNum("O3");
                                                if (drv["O3"] != null && drv["O3"].ToString().Trim() != "")
                                                    data += "{x:" + timeI + ",y:" + (DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv["O3"]), DecimalNum) * 1000).ToString("G0") + ",u:'" + GetUnit("O3") + "',z:'" + prif + "'},";
                                            }
                                        }
                                        else if (prif == "PM2.5")
                                        {
                                            int DecimalNum = GetDecimalNum("PM25");
                                            if (drv["PM25"] != null && drv["PM25"].ToString().Trim() != "")
                                                data += "{x:" + timeI + ",y:" + (DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv["PM25"]), DecimalNum) * 1000).ToString("G0") + ",u:'" + GetUnit("PM25") + "',z:'" + prif + "'},";
                                        }
                                        else
                                        {
                                            if (prif == "CO")
                                            {
                                                int DecimalNum = GetDecimalNum("CO");
                                                if (drv[prif] != null && drv[prif].ToString().Trim() != "")
                                                    data += "{x:" + timeI + ",y:" + (DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv[prif]), DecimalNum)).ToString() + GetUnit("CO") + ",u:'" + GetUnit(prif) + "',z:'" + prif + "'},";
                                            }
                                            else
                                            {
                                                int DecimalNum = GetDecimalNum(prif);
                                                if (drv[prif] != null && drv[prif].ToString().Trim() != "")
                                                    data += "{x:" + timeI + ",y:" + (DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv[prif]), DecimalNum) * 1000).ToString("G0") + ",u:'" + GetUnit(prif) + "',z:'" + prif + "'},";
                                            }
                                        }
                                    }
                                }
                                else if (pri == "O3")
                                {
                                    if (dataView.Table.Columns.Contains("Max8HourO3"))
                                    {
                                        int DecimalNum = GetDecimalNum("O3");
                                        if (drv["Max8HourO3"] != null && drv["Max8HourO3"].ToString().Trim() != "")
                                            data += "{x:" + time + ",y:" + (DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv["Max8HourO3"]), DecimalNum) * 1000).ToString("G0") + ",u:'" + GetUnit("O3") + "',z:'" + pri + "'},";
                                    }
                                    if (dataView.Table.Columns.Contains("O3"))
                                    {
                                        int DecimalNum = GetDecimalNum("O3");
                                        if (drv["O3"] != null && drv["O3"].ToString().Trim() != "")
                                            data += "{x:" + time + ",y:" + (DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv["O3"]), DecimalNum) * 1000).ToString("G0") + ",u:'" + GetUnit("O3") + "',z:'" + pri + "'},";
                                    }
                                }
                                else if (pri == "PM2.5")
                                {
                                    int DecimalNum = GetDecimalNum("PM25");
                                    if (drv["PM25"] != null && drv["PM25"].ToString().Trim() != "")
                                        data += "{x:" + time + ",y:" + (DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv["PM25"]), DecimalNum) * 1000).ToString("G0") + ",u:'" + GetUnit("PM25") + "',z:'" + pri + "'},";
                                }
                                else
                                {
                                    if (pri == "CO")
                                    {
                                        int DecimalNum = GetDecimalNum("CO");
                                        if (drv[pri] != null && drv[pri].ToString().Trim() != "")
                                            data += "{x:" + time + ",y:" + (DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv[pri]), DecimalNum)).ToString() + ",u:'" + GetUnit(pri) + "',z:'" + pri + "'},";
                                    }
                                    else
                                    {
                                        int DecimalNum = GetDecimalNum(pri);
                                        if (drv[pri] != null && drv[pri].ToString().Trim() != "")
                                            data += "{x:" + time + ",y:" + (DecimalExtension.GetPollutantValue(Convert.ToDecimal(drv[pri]), DecimalNum) * 1000).ToString("G0") + ",u:'" + GetUnit(pri) + "',z:'" + pri + "'},";
                                    }
                                }
                            }
                            else
                            {
                                data += "[" + time + ",null]";
                            }
                        }

                    }
                    data += "]},";
                }
                if (data != "")
                {
                    data = data.Substring(0, data.Length - 1);
                }
                data += "]";
                hdjsonData.Value = data;
            }

            RegisterScript("generate();");
        }
        #endregion

        public int GetDecimalNum(string factor)
        {
            string pollutantCode = "";
            switch (factor)
            {
                case "PM25":
                    pollutantCode = "a34004";
                    break;
                case "PM10":
                    pollutantCode = "a34002";
                    break;
                case "NO2":
                    pollutantCode = "a21004";
                    break;
                case "SO2":
                    pollutantCode = "a21026";
                    break;
                case "CO":
                    pollutantCode = "a21005";
                    break;
                case "O3":
                    pollutantCode = "a05024";
                    break;
                case "Max8HourO3":
                    pollutantCode = "a05024";
                    break;
            }
            AirPollutantService m_AirPollutantService = new AirPollutantService();
            int DecimalNum = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo(pollutantCode).PollutantDecimalNum);
            return DecimalNum;
        }

        public string GetUnit(string factor)
        {
            string pollutantCode = "";
            switch (factor)
            {
                case "PM25":
                    pollutantCode = "a34004";
                    break;
                case "PM10":
                    pollutantCode = "a34002";
                    break;
                case "NO2":
                    pollutantCode = "a21004";
                    break;
                case "SO2":
                    pollutantCode = "a21026";
                    break;
                case "CO":
                    pollutantCode = "a21005";
                    break;
                case "O3":
                    pollutantCode = "a05024";
                    break;
                case "Max8HourO3":
                    pollutantCode = "a05024";
                    break;
            }
            AirPollutantService m_AirPollutantService = new AirPollutantService();
            string unit = m_AirPollutantService.GetPollutantInfo(pollutantCode).PollutantMeasureUnit;
            return unit;
        }

        /// <summary>
        /// 根据所选站点ID获取相应区域信息
        /// </summary>
        /// <param name="pointIds"></param>
        /// <returns></returns>
        public DataView GetRegionByPointId(string[] pointIds)
        {
            return pointAirService.GetRegionByPointId(pointIds);
        }

        public bool IsOrNotNumber(string a)
        {
            decimal d = 0;
            if (decimal.TryParse(a, out d) == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool IsNumeric(string text)
        {
            return Regex.IsMatch(text, @"^[+-]?\d*[.]?\d*$");
        }

        /// <summary>   
        /// Datatable转换为Json   
        /// </summary>   
        /// <param name="table">Datatable对象</param>   
        /// <returns>Json字符串</returns>   
        public static string ToJson(DataTable dt)
        {
            StringBuilder jsonString = new StringBuilder();
            jsonString.Append("[");
            DataRowCollection drc = dt.Rows;
            for (int i = 0; i < drc.Count; i++)
            {
                jsonString.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string strKey = dt.Columns[j].ColumnName;
                    string strValue = drc[i][j].ToString();
                    Type type = dt.Columns[j].DataType;
                    jsonString.Append("\"" + strKey + "\":");
                    strValue = StringFormat(strKey, strValue, type);
                    if (j < dt.Columns.Count - 1)
                    {
                        jsonString.Append(strValue + ",");
                    }
                    else
                    {
                        jsonString.Append(strValue);
                    }
                }
                jsonString.Append("},");
            }
            if (jsonString.ToString() != "[") jsonString.Remove(jsonString.Length - 1, 1);
            jsonString.Append("]");
            return jsonString.ToString();
        }
        /// <summary>  
        /// 格式化字符型、日期型、布尔型  
        /// </summary>  
        /// <param name="str"></param>  
        /// <param name="type"></param>  
        /// <returns></returns>  
        private static string StringFormat(string key, string str, Type type)
        {
            if (key.Equals("Tstamp", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(str))
            {
                str = ConvertDateTimeToInt(Convert.ToDateTime(str)).ToString();
            }
            if (key.Equals("DateTime", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(str))
            {
                str = ConvertDateTimeToInt(Convert.ToDateTime(str)).ToString();
            }
            if (type == typeof(string))
            {
                str = String2Json(str);
                str = "\"" + str + "\"";
            }
            else if (type == typeof(DateTime))
            {
                str = "\"" + str + "\"";
            }
            else if (type == typeof(bool))
            {
                str = str.ToLower();
            }
            else if (string.IsNullOrEmpty(str))
            {
                str = "null";
            }
            return str;
        }
        /// <summary>  
        /// 过滤特殊字符  
        /// </summary>  
        /// <param name="s"></param>  
        /// <returns></returns>  
        private static string String2Json(String s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s.ToCharArray()[i];
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\""); break;
                    case '\\':
                        sb.Append("\\\\"); break;
                    case '/':
                        sb.Append("\\/"); break;
                    case '\b':
                        sb.Append("\\b"); break;
                    case '\f':
                        sb.Append("\\f"); break;
                    case '\n':
                        sb.Append("\\n"); break;
                    case '\r':
                        sb.Append("\\r"); break;
                    case '\t':
                        sb.Append("\\t"); break;
                    default:
                        sb.Append(c); break;
                }
            }
            return sb.ToString();
        }
        /// <summary>  
        ///DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time">时间</param>  
        /// <returns>long</returns>  
        public static long ConvertDateTimeToInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.AddHours(-8).Ticks) / 10000;   //除10000调整为13位      
            return t;
        }
    }
}