using SmartEP.Core.Generic;
using SmartEP.Service.DataAnalyze.Air.AQIReport;
using SmartEP.Service.DataAnalyze.Water.DataQuery;
using SmartEP.Utilities.Web.UI;
using SmartEP.WebUI.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartEP.WebUI.Pages.EnvAir.DataAnalyze
{
    public partial class DataAvgDayCharts : BasePage
    {
        protected void Page_Prerender(object sender, EventArgs e)
        {
            Bind();

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
        public void Bind()
        {
            string[] factors = PageHelper.GetQueryString("factors").Split(',');
            string AppllicationType = PageHelper.GetQueryString("Appllication");
            string type = PageHelper.GetQueryString("Type");
            string flag = PageHelper.GetQueryString("flag");
            hdFlag.Value = flag;
            string[] pointIds = PageHelper.GetQueryString("pointIds").Split(',');
            DateTime dtStart = DateTime.TryParse(PageHelper.GetQueryString("dtStart"), out dtStart) ? dtStart : DateTime.Now.AddMonths(-1);
            DateTime dtEnd = DateTime.TryParse(PageHelper.GetQueryString("dtEnd"), out dtEnd) ? dtEnd : DateTime.Now;
            if (AppllicationType == "water")
            {
                DataQueryByHourService m_DataQueryByHourService = Singleton<DataQueryByHourService>.GetInstance();
                DataView waterData = m_DataQueryByHourService.getMedianData(pointIds, factors, dtStart, dtEnd);
                if (waterData != null)
                {
                    DataTable dt2 = waterData.ToTable(true, "RegionName");
                    DataTable dt3 = waterData.ToTable(true, "FactorName");
                    hdpointiddata.Value = ToJson(waterData.ToTable());
                    hdPointNames.Value = ToJson(dt2);
                    hdFactorName.Value = ToJson(dt3);
                }
            }
            else
            {
                DayAQIService m_DayAQIService = Singleton<DayAQIService>.GetInstance();
                DataView AvgDayData = null;
                //画K线图
                if (flag == "0" || flag == "")
                {
                    //点位
                    if (type == "Port")
                    {
                        AvgDayData = m_DayAQIService.GetAvgDayData(pointIds, factors, dtStart, dtEnd);
                    }
                    //南通市区、市区均值
                    else if (type == "CityProper" || type == "CityAvg")
                    {
                        AvgDayData = m_DayAQIService.GetAllYearDataNew(pointIds, factors, dtStart, dtEnd);

                    }
                    if (AvgDayData != null)
                    {
                        DataTable dt2 = AvgDayData.ToTable(true, "RegionName");
                        DataTable dt3 = AvgDayData.ToTable(true, "FactorName");
                        hdpointiddata.Value = ToJson(AvgDayData.ToTable());
                        hdPointNames.Value = ToJson(dt2);
                        hdFactorName.Value = ToJson(dt3);
                    }
                }
                //画其他图
                else
                {
                    //点位
                    if (type == "Port")
                    {
                        AvgDayData = m_DayAQIService.GetAvgDayData(pointIds, factors, dtStart, dtEnd, "");
                    }
                    //南通市区、市区均值
                    else if (type == "CityProper" || type == "CityAvg")
                    {
                        AvgDayData = m_DayAQIService.GetAllYearDataNew(pointIds, factors, dtStart, dtEnd, "");
                    }
                    if (AvgDayData != null)
                    {
                        DataTable dt2 = AvgDayData.ToTable(true, "RegionName");
                        DataTable dt3 = AvgDayData.ToTable(true, "FactorName");
                        hdpointiddata.Value = ToJson(AvgDayData.ToTable());
                        hdPointNames.Value = ToJson(dt2);
                        hdFactorName.Value = ToJson(dt3);

                        string factor = string.Empty;
                        string factorName = dt3.Rows[0]["FactorName"].ToString();
                        factor = getfactor(factorName);
                        hdFactor.Value = factor;

                        if (flag == "1")
                        {
                            StringBuilder sbjson = new StringBuilder();
                            sbjson.Append("[");
                            //拼接超标天数
                            string strname = "{" + string.Format("type:'column',name:'{0}',", "超标天数");
                            StringBuilder sb = new StringBuilder();
                            sb.Append("data:[");
                            foreach (DataRow dr2 in dt2.Rows)
                            {
                                //AvgDayData.RowFilter = "FactorName = '" + dr3["FactorName"].ToString() + "' and RegionName = '" + dr2["RegionName"].ToString() + "'";
                                AvgDayData.RowFilter = "RegionName = '" + dr2["RegionName"].ToString() + "'";
                                string strdata = string.Format("{0}", AvgDayData[0]["OutDays"]);
                                sb.Append(strdata + ",");
                            }
                            sb.Append("],yAxis: 0},");
                            sbjson.Append(strname + sb.ToString());
                            //拼接有效天数
                            strname = "{" + string.Format("type:'column',name:'{0}',", "有效天数");
                            StringBuilder sbMonitorDays = new StringBuilder();
                            sbMonitorDays.Append("data:[");
                            foreach (DataRow dr2 in dt2.Rows)
                            {
                                //AvgDayData.RowFilter = "FactorName = '" + dr3["FactorName"].ToString() + "' and RegionName = '" + dr2["RegionName"].ToString() + "'";
                                AvgDayData.RowFilter = "RegionName = '" + dr2["RegionName"].ToString() + "'";
                                string strdata = string.Format("{0}", AvgDayData[0]["MonitorDays"]);
                                sbMonitorDays.Append(strdata + ",");
                            }
                            sbMonitorDays.Append("],yAxis: 0},");
                            sbjson.Append(strname + sbMonitorDays.ToString());
                            //拼接超标率
                            strname = "{" + string.Format("type:'spline',name:'{0}',", "超标率");
                            StringBuilder sbOutRate = new StringBuilder();
                            sbOutRate.Append("data:[");
                            foreach (DataRow dr2 in dt2.Rows)
                            {
                                //AvgDayData.RowFilter = "FactorName = '" + dr3["FactorName"].ToString() + "' and RegionName = '" + dr2["RegionName"].ToString() + "'";
                                AvgDayData.RowFilter = "RegionName = '" + dr2["RegionName"].ToString() + "'";
                                string strdata = string.Format("{0}", AvgDayData[0]["OutRate"]).TrimEnd('%');
                                sbOutRate.Append(strdata + ",");
                            }
                            sbOutRate.Append("],yAxis: 1},");
                            sbjson.Append(strname + sbOutRate.ToString());
                            sbjson.Append("]");
                            hdjsonData.Value = sbjson.ToString();
                        }
                        if (flag == "2")
                        {
                            StringBuilder sbjson = new StringBuilder();
                            sbjson.Append("[");
                            //拼接百分位数浓度
                            string strname = "{" + string.Format("type:'column',name:'{0}',", "百分位数浓度");
                            StringBuilder sb = new StringBuilder();
                            sb.Append("data:[");
                            foreach (DataRow dr2 in dt2.Rows)
                            {
                                //AvgDayData.RowFilter = "FactorName = '" + dr3["FactorName"].ToString() + "' and RegionName = '" + dr2["RegionName"].ToString() + "'";
                                AvgDayData.RowFilter = "RegionName = '" + dr2["RegionName"].ToString() + "'";
                                string strdata = string.Format("{0}", AvgDayData[0]["YearPercent"]);
                                sb.Append(strdata + ",");
                            }
                            sb.Append("],yAxis: 0},");
                            sbjson.Append(strname + sb.ToString());
                            //拼接百分位数超标倍数
                            strname = "{" + string.Format("type:'spline',name:'{0}',", "百分位数超标倍数");
                            StringBuilder sbYearPerOutRate = new StringBuilder();
                            sbYearPerOutRate.Append("data:[");
                            foreach (DataRow dr2 in dt2.Rows)
                            {
                                //AvgDayData.RowFilter = "FactorName = '" + dr3["FactorName"].ToString() + "' and RegionName = '" + dr2["RegionName"].ToString() + "'";
                                AvgDayData.RowFilter = "RegionName = '" + dr2["RegionName"].ToString() + "'";
                                string strdata = string.Format("{0}", AvgDayData[0]["YearPerOutRate"].ToString() == "/" ? "null" : AvgDayData[0]["YearPerOutRate"]);
                                sbYearPerOutRate.Append(strdata + ",");
                            }
                            sbYearPerOutRate.Append("],yAxis: 1},");
                            sbjson.Append(strname + sbYearPerOutRate.ToString());
                            sbjson.Append("]");
                            hdjsonData.Value = sbjson.ToString();
                        }
                    }
                }
            }
        }
        public string getfactor(string factorName)
        {
            string factor = string.Empty;
            if (factorName.Contains("PM2.5"))
            {
                factor = "PM2.5(μg/m3)";
            }
            else if (factorName.Contains("PM10"))
            {
                factor = "PM10(μg/m3)";
            }
            else if (factorName.Contains("NO2"))
            {
                factor = "NO2(μg/m3)";
            }
            else if (factorName.Contains("CO"))
            {
                factor = "CO(mg/m3)";
            }
            else if (factorName.Contains("SO2"))
            {
                factor = "SO2(μg/m3)";
            }
            else
            {
                factor = "O3-8(μg/m3)";
            }
            return factor;
        }

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            Bind();
        }

    }
}