using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Service.AutoMonitoring.Air;
using SmartEP.Service.AutoMonitoring.Interfaces;
using SmartEP.Service.DataAnalyze.Air;
using SmartEP.Service.DataAnalyze.Air.DataQuery;
using SmartEP.Utilities.Calendar;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Utilities.Web.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartEP.WebUI.Pages.EnvAir.Chart
{
    public partial class ChartFrameScatter : SmartEP.WebUI.Common.BasePage
    {
        private IInfectantDALService g_IInfectantDALService = null;
        InfectantBy60Service m_HourOriData = Singleton<InfectantBy60Service>.GetInstance();
        InfectantByDayService m_DayOriData = Singleton<InfectantByDayService>.GetInstance();
        InfectantByMonthService m_MonthOriData = Singleton<InfectantByMonthService>.GetInstance();
        DataQueryByHourService m_HourData = Singleton<DataQueryByHourService>.GetInstance();
        DataQueryByDayService m_DayData = Singleton<DataQueryByDayService>.GetInstance();
        DataQueryByWeekService m_WeekData = Singleton<DataQueryByWeekService>.GetInstance();
        DataQueryByMonthService m_MonthData = Singleton<DataQueryByMonthService>.GetInstance();
        DataQueryBySeasonService m_SeasonData = Singleton<DataQueryBySeasonService>.GetInstance();
        DataQueryByYearService m_YearData = Singleton<DataQueryByYearService>.GetInstance();
        AQICalculateService s_AQICalculateService = new AQICalculateService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string x = Session["X"].ToString();
                string y = Session["Y"].ToString();
                factorCbxRsmX.SetFactorValuesFromCodes(x != "" ? x : "a34004;");
                factorCbxRsmY.SetFactorValuesFromCodes(y != "" ? y : "a34002;");

                HiddenX.Value = factorCbxRsmX.GetFactors().FirstOrDefault().PollutantName + "(" + factorCbxRsmX.GetFactors().FirstOrDefault().PollutantMeasureUnit + ")";
                HiddenY.Value = factorCbxRsmY.GetFactors().FirstOrDefault().PollutantName + "(" + factorCbxRsmY.GetFactors().FirstOrDefault().PollutantMeasureUnit + ")";
                string PointID = PageHelper.GetQueryString("PointID");
                string dtBegin = PageHelper.GetQueryString("dtBegin");
                string dtEnd = PageHelper.GetQueryString("dtEnd");
                string Type = PageHelper.GetQueryString("Type");
                bind();
            }
        }
        public void bind()
        {
            if (factorCbxRsmX.GetFactors().FirstOrDefault().PollutantCode == factorCbxRsmY.GetFactors().FirstOrDefault().PollutantCode)
            {
                Alert("不能选择相同因子");
                return;
            }
            string PointID = PageHelper.GetQueryString("PointID");

            string Type = PageHelper.GetQueryString("Type");

            if (Type == "Min1" || Type == "Min5" || Type == "Min60")
            {
                g_IInfectantDALService = MonitoringDataAir.GetInfectantDALService(SmartEP.Core.Enums.EnumMapping.ParseEnum<PollutantDataType>(Type));
            }

            string[] portIds = PointID.Split(';');
            string[] factors = { factorCbxRsmX.GetFactors().FirstOrDefault().PollutantCode, factorCbxRsmY.GetFactors().FirstOrDefault().PollutantCode };
            //string[] factorCodes = factorCbxRsmX.GetFactorValues(SmartEP.Core.Enums.CbxRsmReturnType.Code);
            //每页显示数据个数            
            int pageSize = int.MaxValue;
            //当前页的序号
            int pageNo = 0;

            //【给隐藏域赋值，用于显示Chart】

            //数据总行数
            int recordTotal = 0;
            DataTable dt = new DataTable();
            var monitorData = new DataView();

            if (Type == "OriDay")
            {
                DateTime dtBegin = DateTime.Parse(PageHelper.GetQueryString("dtBegin"));
                DateTime dtEnd = DateTime.Parse(PageHelper.GetQueryString("dtEnd"));
                string orderby = "DateTime desc";
                monitorData = m_DayOriData.GetDayAvgData(portIds, factors, dtBegin, dtEnd, pageSize, pageNo, out recordTotal, orderby);
            }
            if (Type == "OriMonth")
            {
                DateTime dtBegin = DateTime.Parse(PageHelper.GetQueryString("dtBegin"));
                DateTime dtEnd = DateTime.Parse(PageHelper.GetQueryString("dtEnd"));
                string orderby = "Year desc,MonthOfYear desc";
                monitorData = m_MonthOriData.GetOriDataAvg(portIds, factors, dtBegin, dtEnd, pageSize, pageNo, out recordTotal, orderby);
            }
            if (Type == "Min1" || Type == "Min5" || Type == "Min60")
            {
                DateTime dtBegin = DateTime.Parse(PageHelper.GetQueryString("dtBegin"));
                DateTime dtEnd = DateTime.Parse(PageHelper.GetQueryString("dtEnd"));
                string orderby = "Tstamp desc";
                monitorData = m_HourOriData.GetHourAvgData(portIds, factors, dtBegin, dtEnd, Type, pageSize, pageNo, out recordTotal, orderby);
            }
            if (Type == "Hour")
            {
                DateTime dtBegin = DateTime.Parse(PageHelper.GetQueryString("dtBegin"));
                DateTime dtEnd = DateTime.Parse(PageHelper.GetQueryString("dtEnd"));
                string orderBy = "Tstamp desc";
                monitorData = m_HourData.GetHourDataAvg(portIds, factors, dtBegin, dtEnd, pageSize, pageNo, out recordTotal, orderBy);

            }
            //日数据
            if (Type == "Day")
            {
                DateTime dtBegin = DateTime.Parse(PageHelper.GetQueryString("dtBegin"));
                DateTime dtEnd = DateTime.Parse(PageHelper.GetQueryString("dtEnd"));
                string orderBy = "DateTime desc";
                monitorData = m_DayData.GetDayDataAvg(portIds, factors, dtBegin, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
            }
            //月数据
            if (Type == "Month")
            {
                string[] tmStr = PageHelper.GetQueryString("dtBegin").ToString().Split(';');

                int monthB = Convert.ToInt32(tmStr[0]);
                int monthE = Convert.ToInt32(tmStr[2]);
                int monthF = Convert.ToInt32(tmStr[1]);
                int monthT = Convert.ToInt32(tmStr[3]);
                string orderBy = "Year desc,MonthOfYear desc";
                monitorData = m_MonthData.GetMonthDataAvg(portIds, factors, monthB, monthF, monthE, monthT, pageSize, pageNo, out recordTotal, orderBy);
            }
            //季数据
            if (Type == "Season")
            {
                string[] tmStr = PageHelper.GetQueryString("dtBegin").ToString().Split(';');

                int seasonB = Convert.ToInt32(tmStr[0]);
                int seasonE = Convert.ToInt32(tmStr[2]);
                int seasonF = Convert.ToInt32(tmStr[1]);
                int seasonT = Convert.ToInt32(tmStr[3]);
                string orderBy = "Year desc,SeasonOfYear desc";
                monitorData = m_SeasonData.GetSeasonDataAvg(portIds, factors, seasonB, seasonF, seasonE, seasonT, pageSize, pageNo, out recordTotal, orderBy);
            }
            //年数据
            if (Type == "Year")
            {
                string[] tmStr = PageHelper.GetQueryString("dtBegin").ToString().Split(';');

                int yearB = Convert.ToInt32(tmStr[0]);
                int yearE = Convert.ToInt32(tmStr[1]);
                string orderBy = "Year desc";
                monitorData = m_YearData.GetYearDataAvg(portIds, factors, yearB, yearE, pageSize, pageNo, out recordTotal, orderBy);

            }
            //周数据
            if (Type == "Week")
            {
                string[] tmStr = PageHelper.GetQueryString("dtBegin").ToString().Split(';');

                int weekB = Convert.ToInt32(tmStr[0]);
                int weekE = Convert.ToInt32(tmStr[2]);
                int weekF = Convert.ToInt32(tmStr[1]);
                int weekT = Convert.ToInt32(tmStr[3]);
                string orderBy = "Year desc,WeekOfYear desc";
                monitorData = m_WeekData.GetWeekDataAvg(portIds, factors, weekB, weekF, weekE, weekT, pageSize, pageNo, out recordTotal, orderBy);
            }
            dt = monitorData.ToTable();
            if (monitorData.Count > 0)// by lvyun
            {
                string XCode = factorCbxRsmX.GetFactors().FirstOrDefault().PollutantCode;
                string YCode = factorCbxRsmY.GetFactors().FirstOrDefault().PollutantCode;
                //x轴小数位修约
                DataTable dtFactorX = s_AQICalculateService.GetPollutantUnit(XCode);
                DataTable dtFactorY = s_AQICalculateService.GetPollutantUnit(YCode);
                if (dtFactorX.Rows.Count > 0)
                {
                    int decimalNumX = Convert.ToInt32(dtFactorX.Rows[0]["DecimalDigit"]);
                    if (dtFactorX.Rows[0]["MeasureUnitName"].Equals("μg/m³"))
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (dr[XCode] != DBNull.Value)
                                dr[XCode] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[XCode]), decimalNumX) * 1000;
                        }

                    }
                }
                if (dtFactorY.Rows.Count > 0)
                {
                    int decimalNumY = Convert.ToInt32(dtFactorY.Rows[0]["DecimalDigit"]);
                    if (dtFactorY.Rows[0]["MeasureUnitName"].Equals("μg/m³"))
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (dr[YCode] != DBNull.Value)
                                dr[YCode] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[YCode]), decimalNumY) * 1000;
                        }
                    }
                }
            }

            dt.Columns[factorCbxRsmX.GetFactors().FirstOrDefault().PollutantCode].ColumnName = "Xvalue";
            dt.Columns[factorCbxRsmY.GetFactors().FirstOrDefault().PollutantCode].ColumnName = "Yvalue";
            DataTable dt1 = new DataTable();
            dt1.Columns.Add("xvalue", typeof(string));
            dt1.Columns.Add("yvalue", typeof(string));
            if (dt.Rows.Count > 0)
            {
                foreach (DataRowView dr in dt.AsDataView())
                {
                    if (dr["Xvalue"] == DBNull.Value || dr["Yvalue"] == DBNull.Value)
                    {
                        dr.Delete();
                    }
                }
                dt.AcceptChanges();
                if (dt.Rows.Count > 1)
                {
                    decimal AVGX = Convert.ToDecimal(dt.Compute("AVG(Xvalue)", "").ToString());
                    decimal AVGY = Convert.ToDecimal(dt.Compute("AVG(Yvalue)", "").ToString());
                    decimal exp1 = 0;   //公式分子
                    decimal exp2 = 0;   //公式分母
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["Xvalue"].ToString().Trim() == "")
                        {
                            dr["Xvalue"] = "0";
                        }
                        if (dr["Yvalue"].ToString().Trim() == "")
                        {
                            dr["Yvalue"] = "0";
                        }
                        exp1 += Convert.ToDecimal(dr["Xvalue"].ToString()) * Convert.ToDecimal(dr["Yvalue"].ToString());
                        exp2 += Convert.ToDecimal(dr["Xvalue"].ToString()) * Convert.ToDecimal(dr["Xvalue"].ToString());
                    }
                    exp1 -= dt.Rows.Count * AVGX * AVGY;
                    exp2 -= dt.Rows.Count * AVGX * AVGX;
                    decimal expb = exp1 / exp2;
                    decimal expa = AVGY - expb * AVGX;
                    //求点
                    decimal x1 = Convert.ToDecimal(dt.Compute("Min(Xvalue)", "").ToString());   //第一个点的x
                    decimal y1 = expb * x1 + expa;                                              //第一个点的y
                    decimal x2 = Convert.ToDecimal(dt.Compute("MAX(Xvalue)", "").ToString());   //第二个点的x
                    decimal y2 = expb * x2 + expa;                                              //第二个点的y

                    DataRow dr1 = dt1.NewRow();
                    DataRow dr2 = dt1.NewRow();
                    dr1["xvalue"] = x1.ToString();
                    dr1["yvalue"] = y1.ToString();
                    dr2["xvalue"] = x2.ToString();
                    dr2["yvalue"] = y2.ToString();
                    dt1.Rows.Add(dr1);
                    dt1.Rows.Add(dr2);
                }
                else
                {
                    Alert("获取线性回归关系至少需要两条数据！");
                }
            }
            HiddenValue.Value = ToJson(dt);
            HiddenValueNew.Value = ToJson(dt1);
        }

        protected void factorCbxRsm_SelectedChanged()
        {
            if (factorCbxRsmX.GetFactors().FirstOrDefault().PollutantCode == factorCbxRsmY.GetFactors().FirstOrDefault().PollutantCode)
            {
                Alert("不能选择相同因子");
            }
            string unit = string.Empty;
            if (factorCbxRsmX.GetFactors().FirstOrDefault().PollutantMeasureUnit.Equals("μg/m<sup>3</sup>") || factorCbxRsmY.GetFactors().FirstOrDefault().PollutantMeasureUnit.Equals("μg/m<sup>3</sup>"))
            {
                unit = "μg/m3";
            }
            if (factorCbxRsmX.GetFactors().FirstOrDefault().PollutantMeasureUnit.Equals("mg/m<sup>3</sup>") || factorCbxRsmY.GetFactors().FirstOrDefault().PollutantMeasureUnit.Equals("mg/m<sup>3</sup>"))
            {
                unit = "mg/m3";
            }
            HiddenX.Value = factorCbxRsmX.GetFactors().FirstOrDefault().PollutantName + "(" + unit + ")";
            HiddenY.Value = factorCbxRsmY.GetFactors().FirstOrDefault().PollutantName + "(" + unit + ")";
            bind();
        }

        //protected void factorCbxRsmY_SelectedChanged()
        //{
        //    if (factorCbxRsmX.GetFactors().FirstOrDefault().PollutantCode == factorCbxRsmY.GetFactors().FirstOrDefault().PollutantCode)
        //    {
        //        Alert("不能选择相同因子");
        //    }
        //    HiddenY.Value = factorCbxRsmY.GetFactors().FirstOrDefault().PollutantName + "(" + factorCbxRsmY.GetFactors().FirstOrDefault().PollutantMeasureUnit + ")";
        //    bind();
        //}
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
                    strValue = StringFormat(strValue, type);
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
        /// DataTable转成Json   
        /// </summary>  
        /// <param name="jsonName"></param>  
        /// <param name="dt"></param>  
        /// <returns></returns>  
        public static string ToJson(DataTable dt, string jsonName)
        {
            StringBuilder Json = new StringBuilder();
            if (string.IsNullOrEmpty(jsonName))
                jsonName = dt.TableName;
            Json.Append("{\"" + jsonName + "\":[");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Json.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        Type type = dt.Rows[i][j].GetType();
                        Json.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + StringFormat(dt.Rows[i][j].ToString(), type));
                        if (j < dt.Columns.Count - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < dt.Rows.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]}");
            return Json.ToString();
        }

        /// <summary>  
        /// 格式化字符型、日期型、布尔型  
        /// </summary>  
        /// <param name="str"></param>  
        /// <param name="type"></param>  
        /// <returns></returns>  
        private static string StringFormat(string str, Type type)
        {
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
    }
}