using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.Service.AutoMonitoring.Air;
using SmartEP.Service.BaseData.Channel;
using SmartEP.Service.DataAnalyze.Air.DataQuery;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
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
    public partial class DayDevelementChart : SmartEP.WebUI.Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bind();
            }
        }

        public void bind()
        {
            string pointIds = PageHelper.GetQueryString("pointIds");
            string factorIds = PageHelper.GetQueryString("factorIds");
            DateTime dtStart = DateTime.TryParse(PageHelper.GetQueryString("dtbegin"), out dtStart) ? dtStart : Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 00:00:00"));
            DateTime dtEnd = DateTime.TryParse(PageHelper.GetQueryString("dtend"), out dtEnd) ? dtEnd : Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 23:59:59"));
            int days = (dtEnd - dtStart).Days + 1;
            string[] portIds = pointIds.Split(',');
            string[] factorCodes = factorIds.Split(',');
            AirPollutantService airPollutantService = new AirPollutantService();
            InfectantBy60Service m_HourOriData = Singleton<InfectantBy60Service>.GetInstance();
            DataQueryByHourService m_HourData = Singleton<DataQueryByHourService>.GetInstance();
            List<SmartEP.Core.Interfaces.IPollutant> factors = airPollutantService.GetDefaultFactors(factorCodes);
            int recordTotal = 0;
            string flag = PageHelper.GetQueryString("flag");
            CorL.Value = PageHelper.GetQueryString("CorL");
            if (flag == "Ori")
            {
                DataTable dt = new DataTable();
                string orderby = "Tstamp asc";
                var monitorData = m_HourOriData.GetHourAvgData(portIds, factorCodes, dtStart, dtEnd,"Min60", 99999, 0, out recordTotal, orderby);
                dt.Columns.Add("PointId", typeof(int));
                dt.Columns.Add("Hours", typeof(string));
                foreach (string fac in factorCodes)
                {
                    dt.Columns.Add(fac, typeof(string));
                }
                for (int i = 0; i < 24; i++)
                {
                    dt.Rows.Add(dt.NewRow());
                }
                foreach (string fac in factorCodes)
                {
                    for (int i = 0; i < 24; i++)
                    {
                        int daycount = (dtEnd - dtStart).Days + 1;
                        decimal d = 0;  //某因子某整点总和计数器
                        for (int j = 0; j < days; j++)
                        {
                            monitorData.RowFilter = "Tstamp>='" + dtStart.AddDays(j).AddHours(i) + "' and Tstamp<'" + dtStart.AddDays(j).AddHours(i + 1) + "'";
                            string facValue = "";
                            if (monitorData.Count <= 0)
                            {
                                facValue = "0";
                            }
                            else
                            {
                                //facValue = (monitorData[j * 24 + i][fac] != null && monitorData[j * 24 + i][fac].ToString().Trim() != "") ? monitorData[j * 24 + i][fac].ToString() : "0";
                                facValue = (monitorData[0][fac] != null && monitorData[0][fac].ToString().Trim() != "") ? monitorData[0][fac].ToString() : "0";
                            }
                            if (facValue == "0") daycount--;
                            d += Convert.ToDecimal(facValue);
                            monitorData.RowFilter = "";
                        }
                        decimal? avg = 0;
                        if (daycount > 0)
                        {
                            avg = d / daycount;
                            dt.Rows[i][fac] = avg.ToString();
                        }
                        else
                        {
                            avg = null;
                        }
                    }
                }
                for (int i = 0; i < 24; i++)
                {
                    if (portIds.Length > 1)
                        dt.Rows[i]["PointId"] = "0";
                    else
                        dt.Rows[i]["PointId"] = portIds[0].ToString();
                    dt.Rows[i]["Hours"] = i + "时";
                }
                StringBuilder sb = new StringBuilder();
                sb.Append("[");
                foreach (SmartEP.Core.Interfaces.IPollutant ip in factors)
                {
                    string str = string.Empty;
                    str += "{name:'" + ip.PollutantName + "',tooltip:{valueSuffix:'" + ip.PollutantMeasureUnit + "'},data:[";
                    for (int i = 0; i < 24; i++)
                    {
                        string value = "[]";
                        if (dt.Rows[i][ip.PollutantCode] != DBNull.Value && dt.Rows[i][ip.PollutantCode].ToString().Trim() != "")
                        {
                            //if (ip.PollutantMeasureUnit == "μg/m3")
                            if (ip.PollutantCode == "a21026" || ip.PollutantCode == "a21004" || ip.PollutantCode == "a05024" || ip.PollutantCode == "a34002" || ip.PollutantCode == "a34004")
                            {
                                decimal a = Convert.ToDecimal(dt.Rows[i][ip.PollutantCode]);
                                value = DecimalExtension.GetPollutantValue(a * 1000, string.IsNullOrEmpty(ip.PollutantDecimalNum) ? 0 : Convert.ToInt32(ip.PollutantDecimalNum) - 3).ToString();
                            }
                            else
                            {
                                decimal a = Convert.ToDecimal(dt.Rows[i][ip.PollutantCode]);
                                value = DecimalExtension.GetPollutantValue(a, string.IsNullOrEmpty(ip.PollutantDecimalNum) ? 3 : Convert.ToInt32(ip.PollutantDecimalNum)).ToString();
                            }
                        }
                        str += value + ",";
                    }
                    str += "]},";
                    sb.Append(str);
                }
                sb.Append("]");
                hdjsonData.Value = sb.ToString();
                RegisterScript("generate();");
            }
            if (flag == "Audit")
            {
                DataTable dt = new DataTable();
                string orderby = "PointId,Tstamp asc";
                var monitorData = m_HourData.GetHourDataAvg(portIds, factors.Select(p => p.PollutantCode).ToArray(), dtStart, dtEnd, 99999, 0, out recordTotal, orderby);
                dt.Columns.Add("PointId", typeof(int));
                dt.Columns.Add("Hours", typeof(string));
                foreach (string fac in factorCodes)
                {
                    dt.Columns.Add(fac, typeof(string));
                }
                for (int i = 0; i < 24; i++)
                {
                    dt.Rows.Add(dt.NewRow());
                }
                foreach (string fac in factorCodes)
                {
                    for (int i = 0; i < 24; i++)
                    {
                        int daycount = (dtEnd - dtStart).Days + 1;
                        decimal d = 0;  //某因子某整点总和计数器
                        for (int j = 0; j < days; j++)
                        {
                            monitorData.RowFilter = "Tstamp>='" + dtStart.AddDays(j).AddHours(i) + "' and Tstamp<'" + dtStart.AddDays(j).AddHours(i + 1) + "'";
                            string facValue = "";
                            if (monitorData.Count <= 0)
                            {
                                facValue = "0";
                            }
                            else
                            {
                                //facValue = (monitorData[j * 24 + i][fac] != null && monitorData[j * 24 + i][fac].ToString().Trim() != "") ? monitorData[j * 24 + i][fac].ToString() : "0";
                                facValue = (monitorData[0][fac] != null && monitorData[0][fac].ToString().Trim() != "") ? monitorData[0][fac].ToString() : "0";
                            }
                            if (facValue == "0") daycount--;
                            d += Convert.ToDecimal(facValue);
                            monitorData.RowFilter = "";
                        }
                        decimal? avg = 0;
                        if (daycount > 0)
                        {
                            avg = d / daycount;
                            dt.Rows[i][fac] = avg.ToString();
                        }
                        else
                        {
                            avg = null;
                        }
                    }
                }
                for (int i = 0; i < 24; i++)
                {
                    if (portIds.Length > 1)
                        dt.Rows[i]["PointId"] = "0";
                    else
                        dt.Rows[i]["PointId"] = portIds[0].ToString();
                    dt.Rows[i]["Hours"] = i + "时";
                }
                StringBuilder sb = new StringBuilder();
                sb.Append("[");
                foreach (SmartEP.Core.Interfaces.IPollutant ip in factors)
                {
                    string str = string.Empty;
                    str += "{name:'" + ip.PollutantName + "',tooltip:{valueSuffix:'" + ip.PollutantMeasureUnit + "'},data:[";
                    for (int i = 0; i < 24; i++)
                    {
                        string value = "[]";
                        if (dt.Rows[i][ip.PollutantCode] != DBNull.Value && dt.Rows[i][ip.PollutantCode].ToString().Trim() != "")
                        {
                            //if (ip.PollutantMeasureUnit == "μg/m3")
                            if (ip.PollutantCode == "a21026" || ip.PollutantCode == "a21004" || ip.PollutantCode == "a05024" || ip.PollutantCode == "a34002" || ip.PollutantCode == "a34004")
                            {
                                decimal a = Convert.ToDecimal(dt.Rows[i][ip.PollutantCode]);
                                value = DecimalExtension.GetPollutantValue(a * 1000, string.IsNullOrEmpty(ip.PollutantDecimalNum) ? 0 : Convert.ToInt32(ip.PollutantDecimalNum) - 3).ToString();
                            }
                            else
                            {
                                decimal a = Convert.ToDecimal(dt.Rows[i][ip.PollutantCode]);
                                value = DecimalExtension.GetPollutantValue(a, string.IsNullOrEmpty(ip.PollutantDecimalNum) ? 3 : Convert.ToInt32(ip.PollutantDecimalNum)).ToString();
                            }
                        }
                        str += value + ",";
                    }
                    str += "]},";
                    sb.Append(str);
                }
                sb.Append("]");
                hdjsonData.Value = sb.ToString();
                RegisterScript("generate();");
            }

        }

    }
}