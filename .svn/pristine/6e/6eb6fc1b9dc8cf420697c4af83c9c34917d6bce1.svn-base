using SmartEP.Core.Generic;
using SmartEP.MonitoringBusinessRepository;
using SmartEP.MonitoringBusinessRepository.Air;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.DataAnalyze.Air
{
    public class AirFrequencyService
    {
        /// <summary>
        /// 数据库处理类
        /// </summary>
        DayAQIRepository g_DayAQIRepository = Singleton<DayAQIRepository>.GetInstance();
        FrequencyRepository g_FrequencyRepository = Singleton<FrequencyRepository>.GetInstance();

        /// <summary>
        /// 获取统计数据（按站点）
        /// </summary>
        /// <param name="pointIds"></param>
        /// <param name="factorCode"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataTable getAirFrequencyData(string[] pointIds, string factorCode, DateTime dtStart, DateTime dtEnd)
        {
            TimeSpan ts = dtEnd - dtStart;
            int dayTotal = ts.Days;
            DataView dv = g_FrequencyRepository.GetSetData(SmartEP.Core.Enums.EnumMapping.GetDesc(SmartEP.Core.Enums.ApplicationType.Air), factorCode);
            if (dv == null || dv.Count == 0)
            {
                return null;
            }
            int recordCount = 0;
            DataView dayReport = g_DayAQIRepository.GetDataPager(pointIds, dtStart, dtEnd, 99999, 0, out recordCount);
            DataTable dt = new DataTable();
            dt.Columns.Add("pointIds", typeof(string));
            for (int i = 0; i < dv.Count; i++)
            {
                if (dv[i]["Range"] != DBNull.Value)
                {
                    dt.Columns.Add(dv[i]["Range"].ToString(), typeof(string));
                }
            }
            dt.Columns.Add("volidDays", typeof(string));
            foreach (string pointItem in pointIds)
            {
                dayReport.RowFilter = "PointId=" + pointItem + " and AQIValue>0";
                int dvCount = dayReport.Count;
                DataRow dr = dt.NewRow();
                dr["pointIds"] = pointItem;
                if (dayTotal - dvCount >= 0)
                {
                    dr["volidDays"] = dayTotal - dvCount;
                }
                for (int i = 0; i < dv.Count; i++)
                {
                    if (dv[i]["Range"] != DBNull.Value)
                    {
                        decimal? upperOrNUll = null;
                        decimal upper;
                        if (decimal.TryParse(dv[i]["Upper"].ToString(), out upper))
                        {
                            if (factorCode != "a05024")
                            {
                                upperOrNUll = upper / 1000;
                            }
                            else
                            {
                                upperOrNUll = upper;
                            }
                        }
                        else
                        {
                            upperOrNUll = null;
                        }
                        decimal? lowerOrNUll = null;
                        decimal lower;
                        if (decimal.TryParse(dv[i]["Lower"].ToString(), out lower))
                        {
                            lowerOrNUll = lower / 1000;
                        }
                        else
                        {
                            lowerOrNUll = null;
                        }
                        string strRowFilter = string.Empty;
                        strRowFilter += "PointId=" + pointItem + " and AQIValue>0";
                        factorCode = getPollutantEN(factorCode);
                        if (upperOrNUll != null)
                        {
                            strRowFilter += " and  " + factorCode + "<" + upperOrNUll;
                        }
                        if (lowerOrNUll != null)
                        {
                            strRowFilter += " and  " + factorCode + ">=" + lowerOrNUll;
                        }
                        dayReport.RowFilter = strRowFilter;
                        dr[dv[i]["Range"].ToString()] = dayReport.Count;
                        dayReport.RowFilter = string.Empty;
                    }
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
        /// <summary>
        /// 获取统计数据（合计）
        /// </summary>
        /// <param name="pointIds"></param>
        /// <param name="factorCode"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataTable getAirFrequencyAllData(string[] pointIds, string factorCode, DateTime dtStart, DateTime dtEnd)
        {
            try
            {
                DataView dv = g_FrequencyRepository.GetSetData(SmartEP.Core.Enums.EnumMapping.GetDesc(SmartEP.Core.Enums.ApplicationType.Air), factorCode);
                int recordCount = 0;
                DataView dayReport = g_DayAQIRepository.GetDataPager(pointIds, dtStart, dtEnd, 99999, 0, out recordCount);
                DataTable dt = new DataTable();
                for (int i = 0; i < dv.Count; i++)
                {
                    dt.Columns.Add(dv[i]["Range"].ToString(), typeof(string));
                }
                DataRow dr = dt.NewRow();
                for (int i = 0; i < dv.Count; i++)
                {
                    dayReport.RowFilter = "AQIValue>0";
                    decimal dvCount = dayReport.Count;
                    if (dv[i]["Range"] != DBNull.Value)
                    {
                        decimal? upperOrNUll = null;
                        decimal upper;
                        if (decimal.TryParse(dv[i]["Upper"].ToString(), out upper))
                        {
                            if (factorCode != "a05024")
                            {
                                upperOrNUll = upper / 1000;
                            }
                            else
                            {
                                upperOrNUll = upper;
                            }
                        }
                        else
                        {
                            upperOrNUll = null;
                        }
                        decimal? lowerOrNUll = null;
                        decimal lower;
                        if (decimal.TryParse(dv[i]["Lower"].ToString(), out lower))
                        {
                            lowerOrNUll = lower / 1000;
                        }
                        else
                        {
                            lowerOrNUll = null;
                        }
                        string strRowFilter = "AQIValue>0";
                        factorCode = getPollutantEN(factorCode);
                        if (upperOrNUll != null)
                        {
                            strRowFilter += " and  " + factorCode + "<" + upperOrNUll;
                        }
                        if (lowerOrNUll != null)
                        {
                            strRowFilter += " and  " + factorCode + ">=" + lowerOrNUll;
                        }
                        if (upperOrNUll != null || lowerOrNUll != null)
                        {
                            dayReport.RowFilter = strRowFilter;
                            decimal rangeCount = dayReport.Count;
                            dr[dv[i]["Range"].ToString()] = Math.Round(rangeCount / dvCount * 100, 1);
                            dayReport.RowFilter = string.Empty;
                        }
                    }
                }
                dt.Rows.Add(dr);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 根据因子code获取数据列
        /// </summary>
        /// <param name="PollutangCode"></param>
        /// <returns></returns>
        private string getPollutantEN(string PollutangCode)
        {
            string PollutantEN = string.Empty;
            if (PollutangCode == "a34004")
            {
                PollutantEN = "PM25";
            }
            else if (PollutangCode == "a34002")
            {
                PollutantEN = "PM10";
            }
            else if (PollutangCode == "a21004")
            {
                PollutantEN = "NO2";
            }
            else if (PollutangCode == "a21026")
            {
                PollutantEN = "SO2";
            }
            else if (PollutangCode == "a21005")
            {
                PollutantEN = "CO";
            }
            else if (PollutangCode == "a05024")
            {
                PollutantEN = "Max8HourO3";
            }
            else
            {
                PollutantEN = PollutangCode;
            }
            return PollutantEN;
        }
    }
}
