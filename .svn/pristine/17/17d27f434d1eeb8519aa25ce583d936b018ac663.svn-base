using SmartEP.Core.Generic;
using SmartEP.Service.AutoMonitoring.Air;
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
    /// <summary>
    /// 名称：lijingpuChart.aspx.cs
    /// 创建人：徐阳
    /// 创建日期：2017-05-22
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 粒径谱数据
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class lijingpuChart : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 粒径谱数据服务层
        /// </summary>
        SuperStation_lijingpuService s_SuperStation_lijingpuService = Singleton<SuperStation_lijingpuService>.GetInstance();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bind();
            }
        }

        public void bind()
        {
            DataView dv = new DataView();

            string name = PageHelper.GetQueryString("name");
            hdName.Value = name;
            string points = PageHelper.GetQueryString("pointIds");
            string num = PageHelper.GetQueryString("num");
            hdnum.Value = num;

            string flag = PageHelper.GetQueryString("flag");
            if (flag == "1")
            {
                int dtB = Convert.ToInt32(PageHelper.GetQueryString("dtB"));
                int dtE = Convert.ToInt32(PageHelper.GetQueryString("dtE"));
                int dtF = Convert.ToInt32(PageHelper.GetQueryString("dtF"));
                int dtT = Convert.ToInt32(PageHelper.GetQueryString("dtT"));
                string type = PageHelper.GetQueryString("type");
                if (type == "Month" || type == "MonthOri" || type == "Week" || type == "Season" || type == "Year")
                {
                    dv = s_SuperStation_lijingpuService.GetOneData(type, points, dtB, dtE, dtF, dtT, num);
                }
            }
            else
            {
                string type = PageHelper.GetQueryString("type");
                DateTime dtStart = DateTime.TryParse(PageHelper.GetQueryString("dtStart"), out dtStart) ? dtStart : DateTime.Now.AddMonths(-1);
                DateTime dtEnd = DateTime.TryParse(PageHelper.GetQueryString("dtEnd"), out dtEnd) ? dtEnd : DateTime.Now;
                if (type == "Day" || type == "Hour" || type == "DayOri" || type == "HourOri" || type == "Min5" || type == "Min1")
                {
                    dv = s_SuperStation_lijingpuService.GetOneData(type, points, dtStart, dtEnd, num);
                }
            }

            //string tims = "";
            //if (dv != null && dv.Count > 0)
            //{
            //    tims = dv[0]["DateTime"].ToString();
            //}
            var dvLiJing = s_SuperStation_lijingpuService.getLiJingConfig();
            DataTable dt = dv.ToTable();
            for (int i = dt.Columns.Count - 1; i >= 0; i--)
            {
                DataColumn dc = dt.Columns[i];
                if (dc.ColumnName.ToString().Length > 4 && dc.ColumnName.ToString().Substring(0, 4) == "data")
                {
                    string dvName = dvLiJing.ToTable().Select("DataCount='" + dc.ColumnName.ToString() + "'").FirstOrDefault()["DataContent"].ToString();
                    if (dvName == null || dvName == "")
                    {
                        dt.Columns.Remove(dc.ColumnName.ToString());
                        continue;
                    }
                    dc.ColumnName = dvName;
                }
            }
            hdHeavyMetalMonitor.Value = ToJson(dt);
            string hidden = "";
            foreach (DataColumn dc in dt.Columns)
            {
                if (IsOrNotNumber(dc.ColumnName))
                {
                    hidden += dc.ColumnName + ";";
                }
            }
            hiddendiameter.Value = hidden;
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

        /// <summary>
        /// 点击上一张
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void preSearch_Click(object sender, EventArgs e)
        {
            DataView dv = new DataView();

            string name = PageHelper.GetQueryString("name");
            hdName.Value = name;
            string points = PageHelper.GetQueryString("pointIds");

            if (int.Parse(hdnum.Value) - 1 >= 0)
            {
                hdnum.Value = (int.Parse(hdnum.Value) - 1).ToString();
            }
            else
            {
                hdnum.Value = "0";
            }

            string num = hdnum.Value;

            string flag = PageHelper.GetQueryString("flag");
            if (flag == "1")
            {
                int dtB = Convert.ToInt32(PageHelper.GetQueryString("dtB"));
                int dtE = Convert.ToInt32(PageHelper.GetQueryString("dtE"));
                int dtF = Convert.ToInt32(PageHelper.GetQueryString("dtF"));
                int dtT = Convert.ToInt32(PageHelper.GetQueryString("dtT"));
                string type = PageHelper.GetQueryString("type");
                if (type == "Month" || type == "MonthOri" || type == "Week" || type == "Season" || type == "Year")
                {
                    dv = s_SuperStation_lijingpuService.GetOneData(type, points, dtB, dtE, dtF, dtT, num);
                }
            }
            else
            {
                string type = PageHelper.GetQueryString("type");
                DateTime dtStart = DateTime.TryParse(PageHelper.GetQueryString("dtStart"), out dtStart) ? dtStart : DateTime.Now.AddMonths(-1);
                DateTime dtEnd = DateTime.TryParse(PageHelper.GetQueryString("dtEnd"), out dtEnd) ? dtEnd : DateTime.Now;
                if (type == "Day" || type == "Hour" || type == "DayOri" || type == "HourOri" || type == "Min5" || type == "Min1")
                {
                    dv = s_SuperStation_lijingpuService.GetOneData(type, points, dtStart, dtEnd, num);
                }
            }
            var dvLiJing = s_SuperStation_lijingpuService.getLiJingConfig();
            DataTable dt = dv.ToTable();
            for (int i = dt.Columns.Count - 1; i >= 0; i--)
            {
                DataColumn dc = dt.Columns[i];
                if (dc.ColumnName.ToString().Length > 4 && dc.ColumnName.ToString().Substring(0, 4) == "data")
                {
                    string dvName = dvLiJing.ToTable().Select("DataCount='" + dc.ColumnName.ToString() + "'").FirstOrDefault()["DataContent"].ToString();
                    if (dvName == null || dvName == "")
                    {
                        dt.Columns.Remove(dc.ColumnName.ToString());
                        continue;
                    }
                    dc.ColumnName = dvName;
                }
            }
            hdHeavyMetalMonitor.Value = ToJson(dt);
            string hidden = "";
            foreach (DataColumn dc in dt.Columns)
            {
                if (IsOrNotNumber(dc.ColumnName))
                {
                    hidden += dc.ColumnName + ";";
                }
            }
            hiddendiameter.Value = hidden;

            RegisterScript("generate();");


        }

        /// <summary>
        /// 点击下一张
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void nextSearch_Click(object sender, EventArgs e)
        {
            DataView dv = new DataView();

            string name = PageHelper.GetQueryString("name");
            hdName.Value = name;
            string points = PageHelper.GetQueryString("pointIds");

            hdnum.Value = (int.Parse(hdnum.Value) + 1).ToString();

            string num = hdnum.Value;

            string flag = PageHelper.GetQueryString("flag");
            if (flag == "1")
            {
                int dtB = Convert.ToInt32(PageHelper.GetQueryString("dtB"));
                int dtE = Convert.ToInt32(PageHelper.GetQueryString("dtE"));
                int dtF = Convert.ToInt32(PageHelper.GetQueryString("dtF"));
                int dtT = Convert.ToInt32(PageHelper.GetQueryString("dtT"));
                string type = PageHelper.GetQueryString("type");
                if (type == "Month" || type == "MonthOri" || type == "Week" || type == "Season" || type == "Year")
                {
                    dv = s_SuperStation_lijingpuService.GetOneData(type, points, dtB, dtE, dtF, dtT, num);
                }
            }
            else
            {
                string type = PageHelper.GetQueryString("type");
                DateTime dtStart = DateTime.TryParse(PageHelper.GetQueryString("dtStart"), out dtStart) ? dtStart : DateTime.Now.AddMonths(-1);
                DateTime dtEnd = DateTime.TryParse(PageHelper.GetQueryString("dtEnd"), out dtEnd) ? dtEnd : DateTime.Now;
                if (type == "Day" || type == "Hour" || type == "DayOri" || type == "HourOri" || type == "Min5" || type == "Min1")
                {
                    dv = s_SuperStation_lijingpuService.GetOneData(type, points, dtStart, dtEnd, num);
                }
            }
            var dvLiJing = s_SuperStation_lijingpuService.getLiJingConfig();
            DataTable dt = dv.ToTable();
            for (int i = dt.Columns.Count - 1; i >= 0; i--)
            {
                DataColumn dc = dt.Columns[i];
                if (dc.ColumnName.ToString().Length > 4 && dc.ColumnName.ToString().Substring(0, 4) == "data")
                {
                    string dvName = dvLiJing.ToTable().Select("DataCount='" + dc.ColumnName.ToString() + "'").FirstOrDefault()["DataContent"].ToString();
                    if (dvName == null || dvName == "")
                    {
                        dt.Columns.Remove(dc.ColumnName.ToString());
                        continue;
                    }
                    dc.ColumnName = dvName;
                }
            }
            hdHeavyMetalMonitor.Value = ToJson(dt);
            string hidden = "";
            foreach (DataColumn dc in dt.Columns)
            {
                if (IsOrNotNumber(dc.ColumnName))
                {
                    hidden += dc.ColumnName + ";";
                }
            }
            hiddendiameter.Value = hidden;

            RegisterScript("generate();");
        }
    }
}