using SmartEP.Core.Generic;
using SmartEP.Service.DataAnalyze.Air.DataQuery;
using SmartEP.Utilities.Web.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace SmartEP.WebUI.Pages.EnvAir.Chart
{
    public partial class HighChartFrame : SmartEP.WebUI.Common.BasePage
    {
        DataQueryByHourService m_HourData = Singleton<DataQueryByHourService>.GetInstance();
        DataQueryByDayService m_DayData = Singleton<DataQueryByDayService>.GetInstance();
        DataQueryByWeekService m_WeekData = Singleton<DataQueryByWeekService>.GetInstance();
        DataQueryByMonthService m_MonthData = Singleton<DataQueryByMonthService>.GetInstance();
        DataQueryBySeasonService m_SeasonData = Singleton<DataQueryBySeasonService>.GetInstance();
        DataQueryByYearService m_YearData = Singleton<DataQueryByYearService>.GetInstance();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bind();
            }
        }

        public void bind()
        {
            
            string points = PageHelper.GetQueryString("pointIds");
            string[] factors = PageHelper.GetQueryString("factors").Split(',');
            DateTime dtStart = DateTime.TryParse(PageHelper.GetQueryString("dtStart"), out dtStart) ? dtStart : DateTime.Now.AddMonths(-1);
            DateTime dtEnd = DateTime.TryParse(PageHelper.GetQueryString("dtEnd"), out dtEnd) ? dtEnd : DateTime.Now;
            string flags = PageHelper.GetQueryString("flags");
            DataView HighChartData = new DataView();
            if (flags == "Hour")
            {
                HighChartData = m_HourData.GetNewHourDataPager(points, factors, dtStart, dtEnd);
                DataTable dtS = new DataTable();
                dtS.Columns.Add("xValue", typeof(string));
                dtS.Columns.Add("yValue", typeof(string));
                dtS.Columns.Add("zValue", typeof(string));
                foreach (DataRow dr in HighChartData.ToTable().Rows)
                {

                    DataRow drNew = dtS.NewRow();
                    drNew["xValue"] = dr["Tstamp"];
                    drNew["yValue"] = dr["PollutantName"];
                    drNew["zValue"] = dr["PollutantValue"];
                    dtS.Rows.Add(drNew);
                }
                string jsonData = ToJson(dtS);
                HiddenDataS.Value = jsonData;
            }
            if (flags == "Day")
            {
                HighChartData = m_DayData.GetDayDataPager(points, factors, dtStart, dtEnd);
                DataTable dtS = new DataTable();
                dtS.Columns.Add("xValue", typeof(string));
                dtS.Columns.Add("yValue", typeof(string));
                dtS.Columns.Add("zValue", typeof(string));
                foreach (DataRow dr in HighChartData.ToTable().Rows)
                {

                    DataRow drNew = dtS.NewRow();
                    drNew["xValue"] = dr["DateTime"];
                    drNew["yValue"] = dr["PollutantName"];
                    drNew["zValue"] = dr["PollutantValue"];
                    dtS.Rows.Add(drNew);
                }
                string jsonData = ToJson(dtS);
                HiddenDataS.Value = jsonData;
            }
            if (flags == "")
            {
                
            }
            DataView Name = m_HourData.GetPointName(points);
            if (Name.ToTable().Rows.Count > 0)
            {
                string pointName = Name.ToTable().Rows[0][1].ToString();
                HiddenDataR.Value = pointName;
            }
            
            //RegisterScript("HeatmapChart(" + point + "," + jsonData + ");");
            //RegisterScript("HeatmapChart();");
        }
        /// <summary>
        /// 正则判断列名
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsNumeric(string text)
        {
            return Regex.IsMatch(text, @"^\w?\d*$");
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