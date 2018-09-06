using SmartEP.Core.Generic;
using SmartEP.Service.DataAnalyze.Air;
using SmartEP.Utilities.Web.UI;
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
    public partial class AirFrequencyDistributionChart : System.Web.UI.Page
    {
        AirFrequencyService g_AirFrequencyService = Singleton<AirFrequencyService>.GetInstance();
        protected void Page_Prerender(object sender, EventArgs e)
        {
            Bind();
        }
        public void Bind()
        {
            string factorname = PageHelper.GetQueryString("factorname");
            string factors = PageHelper.GetQueryString("factor");
            string[] pointIds = PageHelper.GetQueryString("pointIds").Split(';');
            DateTime dtStart = DateTime.TryParse(PageHelper.GetQueryString("dtStart"), out dtStart) ? dtStart : DateTime.Now.AddMonths(-1);
            DateTime dtEnd = DateTime.TryParse(PageHelper.GetQueryString("dtEnd"), out dtEnd) ? dtEnd : DateTime.Now;
            DataTable dt = g_AirFrequencyService.getAirFrequencyAllData(pointIds, factors, dtStart, dtEnd);
            hdFrequencyData.Value = ToJson(dt);
            DataTable dtName = new DataTable();
            dtName.Columns.Add("RegionName", typeof(string));
            foreach (DataColumn dc in dt.Columns)
            {
                DataRow dr = dtName.NewRow();
                dr["RegionName"] = dc.ColumnName;
                dtName.Rows.Add(dr);
            }
            hdFrequencyRange.Value = ToJson(dtName);
            hdFactorName.Value = factorname;
        }
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
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
    }
}