﻿using SmartEP.Core.Generic;
using SmartEP.MonitoringBusinessRepository.Air;
using SmartEP.Service.AutoMonitoring.Air;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.DataAnalyze.Air.DataQuery
{
    public class GranuleSpecialService
    {
        /// <summary>
        /// 空气时数据
        /// </summary>
        GranuleSpecialRepository m_GranuleSpecial = Singleton<GranuleSpecialRepository>.GetInstance();
        InfectantBy1Service m_Min1Data = Singleton<InfectantBy1Service>.GetInstance();
        InfectantBy5Service m_Min5Data = Singleton<InfectantBy5Service>.GetInstance();
        InfectantBy60Service m_Min60Data = Singleton<InfectantBy60Service>.GetInstance();
        InfectantByDayService m_DayOriData = Singleton<InfectantByDayService>.GetInstance();
        InfectantByMonthService m_MonthOriData = Singleton<InfectantByMonthService>.GetInstance();
        DataQueryByHourService m_HourData = Singleton<DataQueryByHourService>.GetInstance();
        DataQueryByDayService m_DayData = Singleton<DataQueryByDayService>.GetInstance();
        DataQueryByWeekService m_WeekData = Singleton<DataQueryByWeekService>.GetInstance();
        DataQueryByMonthService m_MonthData = Singleton<DataQueryByMonthService>.GetInstance();
        DataQueryBySeasonService m_SeasonData = Singleton<DataQueryBySeasonService>.GetInstance();
        DataQueryByYearService m_YearData = Singleton<DataQueryByYearService>.GetInstance();
        /// <summary>
        /// 空气日数据
        /// </summary>
        DayReportRepository DayData = Singleton<DayReportRepository>.GetInstance();
        /// <summary>
        /// 空气季数据
        /// </summary>
        SeasonReportRepository SeasonData = Singleton<SeasonReportRepository>.GetInstance();
        /// <summary>
        /// 空气年数据
        /// </summary>
        YearReportRepository YearData = Singleton<YearReportRepository>.GetInstance();

        public DataView GetOriHourData(string portIds, string[] factors, DateTime dtStart, DateTime dtEnd, string type)
        {
            DataTable dvWind = m_GranuleSpecial.GetOriHourData(portIds, factors, dtStart, dtEnd, type).ToTable();
            string factorCodeWindDir = "a01008";//风向
            dvWind.Columns.Add("Wind", typeof(string));
            if (dvWind.Rows.Count > 0)
            {
                string windtype = "Sixteen";
                foreach (DataRow dr in dvWind.Rows)
                {
                    if (windtype == "Sixteen")
                    {
                        if (dr[factorCodeWindDir] != DBNull.Value)
                        {
                            dr["Wind"] = (SixteenGetWindName(Convert.ToDouble(dr[factorCodeWindDir])));
                        }
                    }
                    else
                    {
                        if (dr[factorCodeWindDir] != DBNull.Value)
                        {
                            dr["Wind"] = EightGetWindName(Convert.ToDouble(dr[factorCodeWindDir]));
                        }
                    }
                    foreach (string factor in factors)
                    {
                        if ((factor == "a34004" || factor == "a34002" || factor == "a21026" || factor == "a21004" || factor == "a05024") && dr[factor].ToString() != "")
                            dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]) * 1000, 0);
                        if (factor == "a21005" && dr[factor].ToString() != "")
                            dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]), 1);
                    }
                }
            }
            return dvWind.DefaultView;
        }
        public DataView GetOriHourDataNew(string [] pointIds,string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd, string type)
        {
            DataTable dvWind = new DataTable();
            DataTable dvW = new DataTable();
            List<string> factorW =new List<string>();
            string[] factorB = new string[] { };
            factorW.Add("a01001");
            factorW.Add("a01002");
            factorW.Add("a01006");
            factorW.Add("a01007");
            factorW.Add("a01008");
            factorB = factorW.ToArray();
            int recordTotal = 0;
            
            if(type=="Min1")
            {
                dvWind = m_Min1Data.GetAvgDataPager(pointIds, factors, dtStart, dtEnd, int.MaxValue, 0, out recordTotal, "PointId asc,Tstamp asc").ToTable();
                dvW = m_Min1Data.GetAvgDataPager(portIds, factorB, dtStart, dtEnd, int.MaxValue, 0, out recordTotal, "PointId asc,Tstamp asc").ToTable();
            }
            else if (type == "Min5")
            {
                dvWind = m_Min5Data.GetAvgDataPager(pointIds, factors, dtStart, dtEnd, int.MaxValue, 0, out recordTotal, "PointId asc,Tstamp asc").ToTable();
                dvW = m_Min5Data.GetAvgDataPager(portIds, factorB, dtStart, dtEnd, int.MaxValue, 0, out recordTotal, "PointId asc,Tstamp asc").ToTable();
            }
            else
            {
                dvWind = m_Min60Data.GetAvgDataPager(pointIds, factors, dtStart, dtEnd, int.MaxValue, 0, out recordTotal, "PointId asc,Tstamp asc").ToTable();
                dvW = m_Min60Data.GetAvgDataPager(portIds, factorB, dtStart, dtEnd, int.MaxValue, 0, out recordTotal, "PointId asc,Tstamp asc").ToTable();
            }

            //for (int i = 0; i < factors.Length; i++)
            //{
            //    dvW.Columns.Add(factors[i], typeof(System.String));
            //}
            //for (int i = 0; i < dvW.Rows.Count; i++)
            //{
            //    //dt.Rows[i]["DateTime"] = dt.Rows[i]["Year"] + "/" + dt.Rows[i]["MonthOfYear"];
            //    for (int j = 0; j < factors.Length; j++)
            //    {
            //        dvW.Rows[i][factors[j]] = dvWind.Rows[i][factors[j]];
            //    }
            //}
            string factorCodeWindDir = "a01008";//风向
            dvWind.Columns.Add("Wind", typeof(string));
            if (factors.Contains("a01008"))
            {
                if (dvWind.Rows.Count > 0)
                {
                    string windtype = "Sixteen";
                    foreach (DataRow dr in dvWind.Rows)
                    {
                        if (dr["Tstamp"].ToString() != "")
                            dr["Tstamp"] = Convert.ToDateTime(dr["Tstamp"]).ToString("MM/dd HH时");
                        if (windtype == "Sixteen")
                        {
                            if (dr[factorCodeWindDir].IsNotNullOrDBNull())
                            {
                                dr["Wind"] = (SixteenGetWindName(Convert.ToDouble(dr[factorCodeWindDir])));
                            }
                        }
                        else
                        {
                            if (dr[factorCodeWindDir] != DBNull.Value)
                            {
                                dr["Wind"] = EightGetWindName(Convert.ToDouble(dr[factorCodeWindDir]));
                            }
                        }
                        foreach (string factor in factors)
                        {
                            if ((factor == "a34004" || factor == "a34002" || factor == "a21026" || factor == "a21004" || factor == "a05024") && dr[factor].ToString() != "")
                                dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]) * 1000, 0);
                            if (factor == "a21005" && dr[factor].ToString() != "")
                                dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]), 1);
                        }
                        //foreach (string factor in factorB)
                        //{
                        //    if ((factor == "a01001" || factor == "a01002" || factor == "a01006" || factor == "a01007" || factor == "a01008") && dr[factor].ToString() != "")
                        //        dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]), 1);
                        //}
                    }
                }
            }
            else
            {
                if (dvWind.Rows.Count > 0)
                {
                    foreach (DataRow dr in dvWind.Rows)
                    {
                        if (dr["Tstamp"].ToString() != "")
                            dr["Tstamp"] = Convert.ToDateTime(dr["Tstamp"]).ToString("MM/dd HH时");
                        
                        foreach (string factor in factors)
                        {
                            if ((factor == "a34004" || factor == "a34002" || factor == "a21026" || factor == "a21004" || factor == "a05024") && dr[factor].ToString() != "")
                                dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]) * 1000, 0);
                            if ( (factor == "a01001" || factor == "a01002" || factor == "a01006" || factor == "a01007" || factor == "a01008"||factor == "a21005") && dr[factor].ToString() != "")
                                dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]), 1);
                        }
                    }
                }
            }
            return dvWind.DefaultView;
        }
        #region 16角度转换
        public string SixteenGetWindName(double degree)
        {
            string[] windName = {   "北风", 
                                    "东北偏北风", 
                                    "东北风", 
                                    "东北偏东风", 
                                    "东风",
                                    "东南偏东风",
                                    "东南风", 
                                    "东南偏南风", 
                                    "南风",
                                    "西南偏南风",
                                    "西南风",
                                    "西南偏西风", 
                                    "西风", 
                                    "西北偏西风", 
                                    "西北风", 
                                    "西北偏北风"
                                };
            double i = Math.Floor(degree % 360 / 11.25);
            if (i == 31) i = -1;
            i++;
            return windName[(int)Math.Floor(i / 2)];
        }
        #endregion

        #region 8角度转换
        public string EightGetWindName(double degree)
        {
            string[] windName = {   "北风", 
                                    "东北风", 
                                    "东风",
                                    "东南风", 
                                    "南风",
                                    "西南风",
                                    "西风", 
                                    "西北风", 
                                };
            double i = Math.Floor(degree % 360 / 22.5);
            if (i == 15) i = -1;
            i++;
            return windName[(int)Math.Floor(i / 2)];
        }
        #endregion
        /// <summary>
        /// 原始数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataView GetOriDayData(string portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {
            DataTable dt = m_GranuleSpecial.GetOriDayData(portIds, factors, dtStart, dtEnd).ToTable();

            dt.Columns.Add("Tstamp", typeof(string));
            dt.Columns.Add("Wind", typeof(string));
            for (int i = 0; i < dt.Rows.Count; i++)
            {

            }
            string factorCodeWindDir = "a01008";//风向

            if (dt.Rows.Count > 0)
            {
                string windtype = "Sixteen";
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["DateTime"].ToString() != "")
                        dr["Tstamp"] = Convert.ToDateTime(dr["DateTime"]).ToString("yyyy-MM-dd");

                    if (windtype == "Sixteen")
                    {
                        if (dr[factorCodeWindDir] != null)
                        {
                            dr["Wind"] = (SixteenGetWindName(Convert.ToDouble(dr[factorCodeWindDir])));
                        }
                    }
                    else
                    {
                        if (dr[factorCodeWindDir] != null)
                        {
                            dr["Wind"] = EightGetWindName(Convert.ToDouble(dr[factorCodeWindDir]));
                        }
                    }
                    foreach (string factor in factors)
                    {
                        if ((factor == "a34004" || factor == "a34002" || factor == "a21026" || factor == "a21004" || factor == "a05024") && dr[factor].ToString() != "")
                            dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]) * 1000, 0);
                        if (factor == "a21005" && dr[factor].ToString() != "")
                            dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]), 1);
                    }
                }
            }
            return dt.DefaultView;
        }
        /// <summary>
        /// 原始数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataView GetOriDayDataNew(string[] pointIds, string [] portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {

            DataTable dvWind = new DataTable();
            int recordTotal = 0;
            //风向
            List<string> factorW = new List<string>();
            string[] factorB = new string[] { };
            factorW.Add("a01001");
            factorW.Add("a01002");
            factorW.Add("a01006");
            factorW.Add("a01007");
            factorW.Add("a01008");
            factorB = factorW.ToArray();

            dvWind = m_DayOriData.GetAvgDataPagers(pointIds, factors, dtStart, dtEnd, int.MaxValue, 0, out recordTotal, "PointId asc,DateTime asc").ToTable();
            //风向
            //m_DayOriData.GetAvgDataPagers(portIdsCG, pfactors.Select(p => p.PollutantCode).ToArray(), dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,DateTime asc");
            DataTable dt = m_DayOriData.GetAvgDataPagers(portIds, factorB, dtStart, dtEnd, int.MaxValue, 0, out recordTotal, "PointId asc,DateTime asc").ToTable(); ;

            //for (int i = 0; i < factors.Length; i++)
            //{
            //    dt.Columns.Add(factors[i], typeof(System.String));
            //}
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    //dt.Rows[i]["DateTime"] = dt.Rows[i]["Year"] + "/" + dt.Rows[i]["MonthOfYear"];
            //    for (int j = 0; j < factors.Length; j++)
            //    {
            //        dt.Rows[i][factors[j]] = dvWind.Rows[i][factors[j]];
            //    }
            //}
            dvWind.Columns.Add("Wind", typeof(string));
            dvWind.Columns["DateTime"].ColumnName = "Tstamp";
            string factorCodeWindDir = "a01008";//风向
            if (factors.Contains("a01008"))
            {
                if (dvWind.Rows.Count > 0)
                {
                    string windtype = "Sixteen";
                    foreach (DataRow dr in dvWind.Rows)
                    {
                        if (dr["Tstamp"].ToString() != "")
                            dr["Tstamp"] = Convert.ToDateTime(dr["Tstamp"]).ToString("yyyy/MM/dd");

                        if (windtype == "Sixteen")
                        {
                            if (dr[factorCodeWindDir].IsNotNullOrDBNull())
                            {
                                dr["Wind"] = (SixteenGetWindName(Convert.ToDouble(dr[factorCodeWindDir])));
                            }
                        }
                        else
                        {
                            if (dr[factorCodeWindDir] != null)
                            {
                                dr["Wind"] = EightGetWindName(Convert.ToDouble(dr[factorCodeWindDir]));
                            }
                        }
                        foreach (string factor in factors)
                        {
                            if ((factor == "a34004" || factor == "a34002" || factor == "a21026" || factor == "a21004" || factor == "a05024") && dr[factor].ToString() != "")
                                dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]) * 1000, 0);
                            if (factor == "a21005" && dr[factor].ToString() != "")
                                dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]), 1);
                        }
                    }
                }
            }
            else
            {
                if (dvWind.Rows.Count > 0)
                {
                    string windtype = "Sixteen";
                    foreach (DataRow dr in dvWind.Rows)
                    {
                        if (dr["Tstamp"].ToString() != "")
                            dr["Tstamp"] = Convert.ToDateTime(dr["Tstamp"]).ToString("yyyy/MM/dd");

                        foreach (string factor in factors)
                        {
                            if ((factor == "a34004" || factor == "a34002" || factor == "a21026" || factor == "a21004" || factor == "a05024") && dr[factor].ToString() != "")
                                dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]) * 1000, 0);
                            if (factor == "a21005" && dr[factor].ToString() != "")
                                dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]), 1);
                        }
                    }
                }
            }
            
            return dvWind.DefaultView;
        }
        /// <summary>
        /// 原始数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataView GetOriMonthDataNew(string[] pointIds, string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {
            DataTable dvWind = new DataTable();
            int recordTotal = 0;
            //风向
            List<string> factorW = new List<string>();
            string[] factorB = new string[] { };
            factorW.Add("a01001");
            factorW.Add("a01002");
            factorW.Add("a01006");
            factorW.Add("a01007");
            factorW.Add("a01008");
            factorB = factorW.ToArray();
            //dvWind = m_GranuleSpecial.GetOriMonthDataNew(pointIds, factors, dtStart, dtEnd).ToTable();
            dvWind = m_MonthOriData.GetOriAvgDataPager(pointIds, factors, dtStart, dtEnd, int.MaxValue, 0, out recordTotal, "PointId asc,Year asc,MonthOfYear asc").ToTable();
            
            //DataTable dt = m_GranuleSpecial.GetOriMonthData(portIds, factorB, dtStart, dtEnd).ToTable();

            DataTable dt = m_MonthOriData.GetOriAvgDataPager(portIds, factorB, dtStart, dtEnd, int.MaxValue, 0, out recordTotal, "PointId asc,Year asc,MonthOfYear asc").ToTable();
            dvWind.Columns.Add("Tstamp", typeof(string));
            //for (int i = 0; i < factors.Length;i++ )
            //{
            //    dt.Columns.Add(factors[i], typeof(System.String));
            //}
            for (int i = 0; i < dvWind.Rows.Count; i++)
            {
                dvWind.Rows[i]["Tstamp"] = dvWind.Rows[i]["Year"] + "/" + dvWind.Rows[i]["MonthOfYear"];
                //for (int j = 0; j < factors.Length; j++)
                //{
                //    dt.Rows[i][factors[j]] = dvWind.Rows[i][factors[j]];
                //}
            }
            //dt.Columns.Add("Tstamp", typeof(string));
            dvWind.Columns.Add("Wind", typeof(string));
            dvWind.Columns.Remove("Year");
            dvWind.Columns.Remove("MonthOfYear");
            if (dvWind != null)
            {
                bool a = dvWind.Columns.Contains("Tstamp");
                if (a)
                {
                    if (dvWind.Columns.Count > 2)
                    {
                        dvWind.Columns["Tstamp"].SetOrdinal(0);
                    }
                }
                bool b = dvWind.Columns.Contains("DateTime");
                if (b)
                {
                    if (dvWind.Columns.Count > 2)
                    {
                        dvWind.Columns["DateTime"].SetOrdinal(0);
                    }
                }
            }
            
            string factorCodeWindDir = "a01008";//风向
            if (factors.Contains("a01008"))
            {
                if (dvWind.Rows.Count > 0)
                {
                    string windtype = "Sixteen";
                    foreach (DataRow dr in dvWind.Rows)
                    {
                        if (dr["Tstamp"].ToString() != "")
                            dr["Tstamp"] = Convert.ToDateTime(dr["Tstamp"]).ToString("yyyy/MM");
                        if (windtype == "Sixteen")
                        {
                            if (dr[factorCodeWindDir] != null)
                            {
                                dr["Wind"] = (SixteenGetWindName(Convert.ToDouble(dr[factorCodeWindDir])));
                            }
                        }
                        else
                        {
                            if (dr[factorCodeWindDir] != null)
                            {
                                dr["Wind"] = EightGetWindName(Convert.ToDouble(dr[factorCodeWindDir]));
                            }
                        }
                        foreach (string factor in factors)
                        {
                            if ((factor == "a34004" || factor == "a34002" || factor == "a21026" || factor == "a21004" || factor == "a05024") && dr[factor].ToString() != "")
                                dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]) * 1000, 0);
                            if (factor == "a21005" && dr[factor].ToString() != "")
                                dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]), 1);
                        }
                    }
                }
            }
            else
            {
                if (dvWind.Rows.Count > 0)
                {
                    //string windtype = "Sixteen";
                    foreach (DataRow dr in dvWind.Rows)
                    {
                        if (dr["Tstamp"].ToString() != "")
                            dr["Tstamp"] = Convert.ToDateTime(dr["Tstamp"]).ToString("yyyy/MM");
                        foreach (string factor in factors)
                        {
                            if ((factor == "a34004" || factor == "a34002" || factor == "a21026" || factor == "a21004" || factor == "a05024") && dr[factor].ToString() != "")
                                dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]) * 1000, 0);
                            if (factor == "a21005" && dr[factor].ToString() != "")
                                dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]), 1);
                        }
                    }
                }
            }
            return dvWind.DefaultView;

        }
        public DataView GetAuditHourDataNew(string[] pointIds, string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {
            DataTable dvWind = new DataTable();
            int recordTotal = 0;
            //风向
            List<string> factorW = new List<string>();
            string[] factorB = new string[] { };
            factorW.Add("a01001");
            factorW.Add("a01002");
            factorW.Add("a01006");
            factorW.Add("a01007");
            factorW.Add("a01008");
            factorB = factorW.ToArray();

            dvWind = m_HourData.GetNewHourDataPagerAvg(pointIds, factors, dtStart, dtEnd, int.MaxValue, 0, out recordTotal, "PointId asc,Tstamp asc").ToTable();
            //m_HourData.GetNewHourDataPagerAvg(portIdsCG, pfactors.Select(p => p.PollutantCode).ToArray(), dtBegion, dtEnd, pageSize, pageNo, out recordTotal, "PointId asc,Tstamp asc");

            //DataTable dt = m_HourData.GetNewHourDataPagerAvg(portIds, factorB, dtStart, dtEnd, int.MaxValue, 0, out recordTotal, "PointId asc,Tstamp asc").ToTable();
            //for (int i = 0; i < factors.Length; i++)
            //{
            //    dt.Columns.Add(factors[i], typeof(System.String));
            //}
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    //dt.Rows[i]["Tstamp"] = dt.Rows[i]["Year"] + "/" + dt.Rows[i]["MonthOfYear"];
            //    for (int j = 0; j < factors.Length; j++)
            //    {
            //        dt.Rows[i][factors[j]] = dvWind.Rows[i][factors[j]];
            //    }
            //}
            dvWind.Columns.Add("Wind", typeof(string));
            
            string factorCodeWindDir = "a01008";//风向
            if (factors.Contains("a01008"))
            {
                if (dvWind.Rows.Count > 0)
                {
                    string windtype = "Sixteen";
                    foreach (DataRow dr in dvWind.Rows)
                    {
                        if (dr["Tstamp"].ToString() != "")
                            dr["Tstamp"] = Convert.ToDateTime(dr["Tstamp"]).ToString("MM/dd HH时");
                        if (windtype == "Sixteen")
                        {
                            if (dr[factorCodeWindDir].IsNotNullOrDBNull())
                            {
                                dr["Wind"] = (SixteenGetWindName(Convert.ToDouble(dr[factorCodeWindDir])));
                            }
                        }
                        else
                        {
                            if (dr[factorCodeWindDir] != null)
                            {
                                dr["Wind"] = EightGetWindName(Convert.ToDouble(dr[factorCodeWindDir]));
                            }
                        }
                        foreach (string factor in factors)
                        {
                            if ((factor == "a34004" || factor == "a34002" || factor == "a21026" || factor == "a21004" || factor == "a05024") && dr[factor].ToString() != "")
                                dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]) * 1000, 0);
                            if (factor == "a21005" && dr[factor].ToString() != "")
                                dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]), 1);
                        }
                    }
                }
            }
            else
            {
                if (dvWind.Rows.Count > 0)
                {
                    foreach (DataRow dr in dvWind.Rows)
                    {
                        if (dr["Tstamp"].ToString() != "")
                            dr["Tstamp"] = Convert.ToDateTime(dr["Tstamp"]).ToString("MM/dd HH时");
                        foreach (string factor in factors)
                        {
                            if ((factor == "a34004" || factor == "a34002" || factor == "a21026" || factor == "a21004" || factor == "a05024") && dr[factor].ToString() != "")
                                dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]) * 1000, 0);
                            if (factor == "a21005" && dr[factor].ToString() != "")
                                dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]), 1);
                        }
                    }
                }
            }
            
            return dvWind.DefaultView;

        }
        /// <summary>
        /// 原始数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataView GetAuditDayDataNew(string[] pointIds, string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {
            DataTable dvWind = new DataTable();
            int recordTotal = 0;
            //风向
            List<string> factorW = new List<string>();
            string[] factorB = new string[] { };
            factorW.Add("a01001");
            factorW.Add("a01002");
            factorW.Add("a01006");
            factorW.Add("a01007");
            factorW.Add("a01008");
            factorB = factorW.ToArray();
            dvWind = DayData.GetAvgDataPager(pointIds, factors, dtStart, dtEnd, int.MaxValue, 0, out recordTotal, "PointId asc,DateTime asc").ToTable();
            //DataTable dt = DayData.GetAvgDataPager(portIds, factorB, dtStart, dtEnd, int.MaxValue, 0, out recordTotal, "PointId asc,DateTime asc").ToTable();
            //for (int i = 0; i < factors.Length; i++)
            //{
            //    dt.Columns.Add(factors[i], typeof(System.String));
            //}
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    //dt.Rows[i]["Tstamp"] = dt.Rows[i]["Year"] + "/" + dt.Rows[i]["MonthOfYear"];
            //    for (int j = 0; j < factors.Length; j++)
            //    {
            //        dt.Rows[i][factors[j]] = dvWind.Rows[i][factors[j]];
            //    }
            //}
            dvWind.Columns.Add("Wind", typeof(string));
            dvWind.Columns["DateTime"].ColumnName = "Tstamp";
            string factorCodeWindDir = "a01008";//风向
            if (factors.Contains("a01008"))
            {
                if (dvWind.Rows.Count > 0)
                {
                    string windtype = "Sixteen";
                    foreach (DataRow dr in dvWind.Rows)
                    {
                        if (dr["Tstamp"].ToString() != "")
                            dr["Tstamp"] = Convert.ToDateTime(dr["Tstamp"]).ToString("yyyy/MM/dd");
                        if (windtype == "Sixteen")
                        {
                            if (dr[factorCodeWindDir].IsNotNullOrDBNull())
                            {
                                dr["Wind"] = (SixteenGetWindName(Convert.ToDouble(dr[factorCodeWindDir])));
                            }
                        }
                        else
                        {
                            if (dr[factorCodeWindDir] != null)
                            {
                                dr["Wind"] = EightGetWindName(Convert.ToDouble(dr[factorCodeWindDir]));
                            }
                        }
                        foreach (string factor in factors)
                        {
                            if ((factor == "a34004" || factor == "a34002" || factor == "a21026" || factor == "a21004" || factor == "a05024") && dr[factor].ToString() != "")
                                dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]) * 1000, 0);
                            if (factor == "a21005" && dr[factor].ToString() != "")
                                dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]), 1);
                        }
                    }
                }
            }
            else
            {
                if (dvWind.Rows.Count > 0)
                {
                    string windtype = "Sixteen";
                    foreach (DataRow dr in dvWind.Rows)
                    {
                        if (dr["Tstamp"].ToString() != "")
                            dr["Tstamp"] = Convert.ToDateTime(dr["Tstamp"]).ToString("yyyy/MM/dd");
                        foreach (string factor in factors)
                        {
                            if ((factor == "a34004" || factor == "a34002" || factor == "a21026" || factor == "a21004" || factor == "a05024") && dr[factor].ToString() != "")
                                dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]) * 1000, 0);
                            if (factor == "a21005" && dr[factor].ToString() != "")
                                dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]), 1);
                        }
                    }
                }
            }
            return dvWind.DefaultView;
        }
        /// <summary>
        /// 原始数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataView GetAuditMonthDataNew(string[] pointIds, string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {
            DataTable dvWind = new DataTable();
            int recordTotal = 0;
            //风向
            List<string> factorW = new List<string>();
            string[] factorB = new string[] { };
            factorW.Add("a01001");
            factorW.Add("a01002");
            factorW.Add("a01006");
            factorW.Add("a01007");
            factorW.Add("a01008");
            factorB = factorW.ToArray();
            dvWind = m_MonthData.GetMonthDataPagerAvg(pointIds, factors, dtStart, dtEnd, int.MaxValue, 0, out recordTotal, "PointId asc,Year asc,MonthOfYear asc").ToTable();
            //DataTable dt = m_MonthData.GetMonthDataPagerAvg(portIds, factorB, dtStart, dtEnd, int.MaxValue, 0, out recordTotal, "PointId asc,Year asc,MonthOfYear asc").ToTable();

            dvWind.Columns.Add("Tstamp", typeof(string));
            //for (int i = 0; i < factors.Length; i++)
            //{
            //    dt.Columns.Add(factors[i], typeof(System.String));
            //}
            for (int i = 0; i < dvWind.Rows.Count; i++)
            {
                dvWind.Rows[i]["Tstamp"] = dvWind.Rows[i]["Year"] + "/" + dvWind.Rows[i]["MonthOfYear"];
                //for (int j = 0; j < factors.Length; j++)
                //{
                //    dt.Rows[i][factors[j]] = dvWind.Rows[i][factors[j]];
                //}
            }
            //dt.Columns.Add("Tstamp", typeof(string));
            dvWind.Columns.Add("Wind", typeof(string));
            dvWind.Columns.Remove("Year");
            dvWind.Columns.Remove("MonthOfYear");
            if (dvWind != null)
            {
                bool a = dvWind.Columns.Contains("Tstamp");
                if (a)
                {
                    if (dvWind.Columns.Count > 2)
                    {
                        dvWind.Columns["Tstamp"].SetOrdinal(0);
                    }
                }
                bool b = dvWind.Columns.Contains("DateTime");
                if (b)
                {
                    if (dvWind.Columns.Count > 2)
                    {
                        dvWind.Columns["DateTime"].SetOrdinal(0);
                    }
                }
            }
            string factorCodeWindDir = "a01008";//风向
            if (factors.Contains("a01008"))
            {
                if (dvWind.Rows.Count > 0)
                {
                    string windtype = "Sixteen";
                    foreach (DataRow dr in dvWind.Rows)
                    {
                        if (dr["Tstamp"].ToString() != "")
                            dr["Tstamp"] = Convert.ToDateTime(dr["Tstamp"]).ToString("yyyy/MM");
                        if (windtype == "Sixteen")
                        {
                            if (dr[factorCodeWindDir].IsNotNullOrDBNull())
                            {
                                dr["Wind"] = (SixteenGetWindName(Convert.ToDouble(dr[factorCodeWindDir])));
                            }
                        }
                        else
                        {
                            if (dr[factorCodeWindDir] != null)
                            {
                                dr["Wind"] = EightGetWindName(Convert.ToDouble(dr[factorCodeWindDir]));
                            }
                        }
                        foreach (string factor in factors)
                        {
                            if ((factor == "a34004" || factor == "a34002" || factor == "a21026" || factor == "a21004" || factor == "a05024") && dr[factor].ToString() != "")
                                dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]) * 1000, 0);
                            if (factor == "a21005" && dr[factor].ToString() != "")
                                dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]), 1);
                        }
                    }
                }
            }
            else
            {
                if (dvWind.Rows.Count > 0)
                {
                    string windtype = "Sixteen";
                    foreach (DataRow dr in dvWind.Rows)
                    {
                        if (dr["Tstamp"].ToString() != "")
                            dr["Tstamp"] = Convert.ToDateTime(dr["Tstamp"]).ToString("yyyy/MM");
                        foreach (string factor in factors)
                        {
                            if ((factor == "a34004" || factor == "a34002" || factor == "a21026" || factor == "a21004" || factor == "a05024") && dr[factor].ToString() != "")
                                dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]) * 1000, 0);
                            if (factor == "a21005" && dr[factor].ToString() != "")
                                dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]), 1);
                        }
                    }
                }
            }
            
            return dvWind.DefaultView;

        }
        /// <summary>
        /// 原始数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataView GetAuditSeasonDataNew(string[] pointIds, string [] portIds, string[] factors, int yearFrom, int seasonOfYearFrom, int yearTo, int seasonOfYearTo)
        {
            DataTable dvWind = new DataTable();
            int recordTotal = 0;
            //风向
            List<string> factorW = new List<string>();
            string[] factorB = new string[] { };
            factorW.Add("a01001");
            factorW.Add("a01002");
            factorW.Add("a01006");
            factorW.Add("a01007");
            factorW.Add("a01008");
            factorB = factorW.ToArray();
            dvWind = SeasonData.GetDataPagerAvg(pointIds, factors, yearFrom, seasonOfYearFrom, yearTo, seasonOfYearTo, int.MaxValue, 0, out recordTotal, "PointId asc,Year asc,SeasonOfYear asc").ToTable();

            //DataTable dt = SeasonData.GetDataPagerAvg(portIds, factorB, yearFrom, seasonOfYearFrom, yearTo, seasonOfYearTo, int.MaxValue, 0, out recordTotal, "PointId asc,Year asc,SeasonOfYear asc").ToTable();
            dvWind.Columns.Add("Wind", typeof(string));
            dvWind.Columns.Add("Tstamp", typeof(string));
            //for (int i = 0; i < factors.Length; i++)
            //{
            //    dt.Columns.Add(factors[i], typeof(System.String));
            //}
            for (int i = 0; i < dvWind.Rows.Count; i++)
            {
                dvWind.Rows[i]["Tstamp"] = dvWind.Rows[i]["Year"] + "年第" + dvWind.Rows[i]["SeasonOfYear"] + "季";
                //for (int j = 0; j < factors.Length; j++)
                //{
                //    dt.Rows[i][factors[j]] = dvWind.Rows[i][factors[j]];
                //}
            }
            if (dvWind != null)
            {
                bool a = dvWind.Columns.Contains("Tstamp");
                if (a)
                {
                    if (dvWind.Columns.Count > 2)
                    {
                        dvWind.Columns["Tstamp"].SetOrdinal(0);
                    }
                }
                bool b = dvWind.Columns.Contains("DateTime");
                if (b)
                {
                    if (dvWind.Columns.Count > 2)
                    {
                        dvWind.Columns["DateTime"].SetOrdinal(0);
                    }
                }
            }
            dvWind.Columns.Remove("Year");
            dvWind.Columns.Remove("SeasonOfYear");
            string factorCodeWindDir = "a01008";//风向
            if (factors.Contains("a01008"))
            {
                if (dvWind.Rows.Count > 0)
                {
                    string windtype = "Sixteen";
                    foreach (DataRow dr in dvWind.Rows)
                    {

                        if (windtype == "Sixteen")
                        {
                            if (dr[factorCodeWindDir] != null)
                            {
                                dr["Wind"] = (SixteenGetWindName(Convert.ToDouble(dr[factorCodeWindDir])));
                            }
                        }
                        else
                        {
                            if (dr[factorCodeWindDir] != null)
                            {
                                dr["Wind"] = EightGetWindName(Convert.ToDouble(dr[factorCodeWindDir]));
                            }
                        }
                        foreach (string factor in factors)
                        {
                            if ((factor == "a34004" || factor == "a34002" || factor == "a21026" || factor == "a21004" || factor == "a05024") && dr[factor].ToString() != "")
                                dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]) * 1000, 0);
                            if (factor == "a21005" && dr[factor].ToString() != "")
                                dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]), 1);
                        }
                    }
                }
            }
            else
            {
                if (dvWind.Rows.Count > 0)
                {
                    //string windtype = "Sixteen";
                    foreach (DataRow dr in dvWind.Rows)
                    {
                        foreach (string factor in factors)
                        {
                            if ((factor == "a34004" || factor == "a34002" || factor == "a21026" || factor == "a21004" || factor == "a05024") && dr[factor].ToString() != "")
                                dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]) * 1000, 0);
                            if (factor == "a21005" && dr[factor].ToString() != "")
                                dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]), 1);
                        }
                    }
                }
            }
            
            return dvWind.DefaultView;

        }
        /// <summary>
        /// 原始数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataView GetAuditYearDataNew(string[] pointIds, string[] portIds, string[] factors, int yearFrom, int yearTo)
        {
            DataTable dvWind = new DataTable();
            int recordTotal = 0;
            //风向
            List<string> factorW = new List<string>();
            string[] factorB = new string[] { };
            factorW.Add("a01001");
            factorW.Add("a01002");
            factorW.Add("a01006");
            factorW.Add("a01007");
            factorW.Add("a01008");
            factorB = factorW.ToArray();
            dvWind = YearData.GetDataPagerAvg(pointIds, factors, yearFrom, yearTo, int.MaxValue, 0, out recordTotal, "PointId asc,Year asc").ToTable();

            //DataTable dt = YearData.GetDataPagerAvg(portIds, factorB, yearFrom, yearTo, int.MaxValue, 0, out recordTotal, "PointId asc,Year asc").ToTable();
            dvWind.Columns.Add("Tstamp", typeof(string));
            dvWind.Columns.Add("Wind", typeof(string));
            //for (int i = 0; i < factors.Length; i++)
            //{
            //    dt.Columns.Add(factors[i], typeof(System.String));
            //}
            for (int i = 0; i < dvWind.Rows.Count; i++)
            {
                dvWind.Rows[i]["Tstamp"] = dvWind.Rows[i]["Year"] + "年";
                //for (int j = 0; j < factors.Length; j++)
                //{
                //    dt.Rows[i][factors[j]] = dvWind.Rows[i][factors[j]];
                //}
            }
            if (dvWind != null)
            {
                bool a = dvWind.Columns.Contains("Tstamp");
                if (a)
                {
                    if (dvWind.Columns.Count > 2)
                    {
                        dvWind.Columns["Tstamp"].SetOrdinal(0);
                    }
                }
                bool b = dvWind.Columns.Contains("DateTime");
                if (b)
                {
                    if (dvWind.Columns.Count > 2)
                    {
                        dvWind.Columns["DateTime"].SetOrdinal(0);
                    }
                }
            }
            dvWind.Columns.Remove("Year");
            
            string factorCodeWindDir = "a01008";//风向
            if (factors.Contains("a01008"))
            {
                if (dvWind.Rows.Count > 0)
                {
                    string windtype = "Sixteen";
                    foreach (DataRow dr in dvWind.Rows)
                    {

                        if (windtype == "Sixteen")
                        {
                            if (dr[factorCodeWindDir] != null)
                            {
                                dr["Wind"] = (SixteenGetWindName(Convert.ToDouble(dr[factorCodeWindDir])));
                            }
                        }
                        else
                        {
                            if (dr[factorCodeWindDir] != null)
                            {
                                dr["Wind"] = EightGetWindName(Convert.ToDouble(dr[factorCodeWindDir]));
                            }
                        }
                        foreach (string factor in factors)
                        {
                            if ((factor == "a34004" || factor == "a34002" || factor == "a21026" || factor == "a21004" || factor == "a05024") && dr[factor].ToString() != "")
                                dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]) * 1000, 0);
                            if (factor == "a21005" && dr[factor].ToString() != "")
                                dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]), 1);
                        }
                    }
                }
            }
            else
            {
                if (dvWind.Rows.Count > 0)
                {
                    //string windtype = "Sixteen";
                    foreach (DataRow dr in dvWind.Rows)
                    {
                        foreach (string factor in factors)
                        {
                            if ((factor == "a34004" || factor == "a34002" || factor == "a21026" || factor == "a21004" || factor == "a05024") && dr[factor].ToString() != "")
                                dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]) * 1000, 0);
                            if (factor == "a21005" && dr[factor].ToString() != "")
                                dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]), 1);
                        }
                    }
                }
            }
            
            return dvWind.DefaultView;

        }
        /// <summary>
        /// 原始数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataView GetAuditWeekDataNew(string[] pointIds, string portIds, string[] factors, int yearFrom, int weekOfYearFrom, int yearTo, int weekOfYearTo)
        {
            DataTable dvWind = new DataTable();

            //风向
            List<string> factorW = new List<string>();
            string[] factorB = new string[] { };

            if (factors.Length != 0 && factors[0] != "")
            {
                dvWind = m_GranuleSpecial.GetAuditWeekDataNew(pointIds, factors, yearFrom, weekOfYearFrom, yearTo, weekOfYearTo).ToTable();
                //风向
                factorW = factors.ToList();
                factorW.Add("a01001");
                factorW.Add("a01002");
                factorW.Add("a01006");
                factorW.Add("a01007");
                factorW.Add("a01008");
                factorB = factorW.ToArray();
            }
            else
            {
                factorW.Add("a01001");
                factorW.Add("a01002");
                factorW.Add("a01006");
                factorW.Add("a01007");
                factorW.Add("a01008");
                factorB = factorW.ToArray();
            }
            //复制表结构
            IEnumerable<DateTime> list = dvWind.AsEnumerable().Select(t => t.Field<DateTime>("WeekOfYear")).Distinct();

            //DataTable dt = m_GranuleSpecial.GetAuditWeekData(portIds, factorB, yearFrom, weekOfYearFrom, yearTo, weekOfYearTo).ToTable();
            dvWind.Columns.Add("Tstamp", typeof(string));
            dvWind.Columns.Add("Wind", typeof(string));
            
            if (dvWind.Rows.Count > 0)
            {
                for (int d = 0; d < list.Count(); d++)
                {

                    for (int i = 0; i < factors.Length; i++)
                    {
                        //总值的除数
                        int x = 0;
                        //平均值
                        decimal all = 0;
                        for (int j = 0; j < pointIds.Length; j++)
                        {
                            if (dvWind.Rows[j * list.Count() + d][factors[i]] != null && dvWind.Rows[j * list.Count() + d][factors[i]].ToString() != "" && (dvWind.Rows[j * list.Count() + d][factors[i]]).IsNotNullOrDBNull())
                            {
                                x += 1;
                                all += Convert.ToDecimal(dvWind.Rows[j * list.Count() + d][factors[i]]);

                            }
                            else
                            {
                                all += 0;
                            }
                        }
                        if (x == 0)
                        {
                            dvWind.Rows[d][factors[i]] = 0;
                        }
                        else
                        {
                            dvWind.Rows[d][factors[i]] = all / x;
                        }
                    }
                }
            }
            if (dvWind != null)
            {
                bool a = dvWind.Columns.Contains("Tstamp");
                if (a)
                {
                    if (dvWind.Columns.Count > 2)
                    {
                        dvWind.Columns["Tstamp"].SetOrdinal(0);
                    }
                }
                bool b = dvWind.Columns.Contains("DateTime");
                if (b)
                {
                    if (dvWind.Columns.Count > 2)
                    {
                        dvWind.Columns["DateTime"].SetOrdinal(0);
                    }
                }
            }
            
            string factorCodeWindDir = "a01008";//风向
            if (factors.Contains("a01008"))
            {
                if (dvWind.Rows.Count > 0)
                {
                    string windtype = "Sixteen";
                    foreach (DataRow dr in dvWind.Rows)
                    {
                        if (dr["WeekOfYear"].ToString() != "")
                            dr["Tstamp"] = dr["Year"].ToString() + "年" + dr["WeekOfYear"].ToString() + "周";
                        if (windtype == "Sixteen")
                        {
                            if (dr[factorCodeWindDir] != null)
                            {
                                dr["Wind"] = (SixteenGetWindName(Convert.ToDouble(dr[factorCodeWindDir])));
                            }
                        }
                        else
                        {
                            if (dr[factorCodeWindDir] != null)
                            {
                                dr["Wind"] = EightGetWindName(Convert.ToDouble(dr[factorCodeWindDir]));
                            }
                        }
                        foreach (string factor in factors)
                        {
                            if ((factor == "a34004" || factor == "a34002" || factor == "a21026" || factor == "a21004" || factor == "a05024") && dr[factor].ToString() != "")
                                dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]) * 1000, 0);
                            if (factor == "a21005" && dr[factor].ToString() != "")
                                dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]), 1);
                        }
                    }
                }
            }
            else
            {
                if (dvWind.Rows.Count > 0)
                {
                    //string windtype = "Sixteen";
                    foreach (DataRow dr in dvWind.Rows)
                    {
                        if (dr["WeekOfYear"].ToString() != "")
                            dr["Tstamp"] = dr["Year"].ToString() + "年" + dr["WeekOfYear"].ToString() + "周";
                        foreach (string factor in factors)
                        {
                            if ((factor == "a34004" || factor == "a34002" || factor == "a21026" || factor == "a21004" || factor == "a05024") && dr[factor].ToString() != "")
                                dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]) * 1000, 0);
                            if (factor == "a21005" && dr[factor].ToString() != "")
                                dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]), 1);
                        }
                    }
                }
            }
            
            dvWind.Columns.Remove("Year");
            dvWind.Columns.Remove("WeekOfYear");
            return dvWind.DefaultView;

        }
        /// <summary>
        /// 原始数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataView GetOriMonthData(string portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {
            DataTable dt = m_GranuleSpecial.GetOriMonthData(portIds, factors, dtStart, dtEnd).ToTable();
            dt.Columns.Add("Tstamp", typeof(string));
            dt.Columns.Add("Wind", typeof(string));
            for (int i = 0; i < dt.Rows.Count; i++)
            {

            }
            string factorCodeWindDir = "a01008";//风向

            if (dt.Rows.Count > 0)
            {
                string windtype = "Sixteen";
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["DateTime"].ToString() != "")
                        dr["Tstamp"] = Convert.ToDateTime(dr["DateTime"]).ToString("yyyy年MM月");
                    if (windtype == "Sixteen")
                    {
                        if (dr[factorCodeWindDir] != null)
                        {
                            dr["Wind"] = (SixteenGetWindName(Convert.ToDouble(dr[factorCodeWindDir])));
                        }
                    }
                    else
                    {
                        if (dr[factorCodeWindDir] != null)
                        {
                            dr["Wind"] = EightGetWindName(Convert.ToDouble(dr[factorCodeWindDir]));
                        }
                    }
                    foreach (string factor in factors)
                    {
                        if ((factor == "a34004" || factor == "a34002" || factor == "a21026" || factor == "a21004" || factor == "a05024") && dr[factor].ToString() != "")
                            dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]) * 1000, 0);
                        if (factor == "a21005" && dr[factor].ToString() != "")
                            dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]), 1);
                    }
                }
            }
            return dt.DefaultView;

        }
        
        public DataView GetAuditHourData(string portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {
            DataTable dt = m_GranuleSpecial.GetAuditHourData(portIds, factors, dtStart, dtEnd).ToTable();
            dt.Columns.Add("Wind", typeof(string));
            for (int i = 0; i < dt.Rows.Count; i++)
            {

            }
            string factorCodeWindDir = "a01008";//风向

            if (dt.Rows.Count > 0)
            {
                string windtype = "Sixteen";
                foreach (DataRow dr in dt.Rows)
                {
                    if (windtype == "Sixteen")
                    {
                        if (dr[factorCodeWindDir] != null)
                        {
                            dr["Wind"] = (SixteenGetWindName(Convert.ToDouble(dr[factorCodeWindDir])));
                        }
                    }
                    else
                    {
                        if (dr[factorCodeWindDir] != null)
                        {
                            dr["Wind"] = EightGetWindName(Convert.ToDouble(dr[factorCodeWindDir]));
                        }
                    }
                    foreach (string factor in factors)
                    {
                        if ((factor == "a34004" || factor == "a34002" || factor == "a21026" || factor == "a21004" || factor == "a05024") && dr[factor].ToString() != "")
                            dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]) * 1000, 0);
                        if (factor == "a21005" && dr[factor].ToString() != "")
                            dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]), 1);
                    }
                }
            }
            return dt.DefaultView;

        }
        /// <summary>
        /// 原始数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataView GetAuditDayData(string portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {
            DataTable dt = m_GranuleSpecial.GetAuditDayData(portIds, factors, dtStart, dtEnd).ToTable();
            dt.Columns.Add("Tstamp", typeof(string));
            dt.Columns.Add("Wind", typeof(string));
            for (int i = 0; i < dt.Rows.Count; i++)
            {

            }
            string factorCodeWindDir = "a01008";//风向

            if (dt.Rows.Count > 0)
            {
                string windtype = "Sixteen";
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["DateTime"].ToString() != "")
                        dr["Tstamp"] = Convert.ToDateTime(dr["DateTime"]).ToString("yyyy-MM-dd");
                    if (windtype == "Sixteen")
                    {
                        if (dr[factorCodeWindDir] != null)
                        {
                            dr["Wind"] = (SixteenGetWindName(Convert.ToDouble(dr[factorCodeWindDir])));
                        }
                    }
                    else
                    {
                        if (dr[factorCodeWindDir] != null)
                        {
                            dr["Wind"] = EightGetWindName(Convert.ToDouble(dr[factorCodeWindDir]));
                        }
                    }
                    foreach (string factor in factors)
                    {
                        if ((factor == "a34004" || factor == "a34002" || factor == "a21026" || factor == "a21004" || factor == "a05024") && dr[factor].ToString() != "")
                            dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]) * 1000, 0);
                        if (factor == "a21005" && dr[factor].ToString() != "")
                            dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]), 1);
                    }
                }
            }
            return dt.DefaultView;
        }
        /// <summary>
        /// 原始数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataView GetAuditMonthData(string portIds, string[] factors, DateTime dtStart, DateTime dtEnd)
        {
            DataTable dt = m_GranuleSpecial.GetAuditMonthData(portIds, factors, dtStart, dtEnd).ToTable();
            dt.Columns.Add("Tstamp", typeof(string));
            dt.Columns.Add("Wind", typeof(string));
            for (int i = 0; i < dt.Rows.Count; i++)
            {

            }
            string factorCodeWindDir = "a01008";//风向

            if (dt.Rows.Count > 0)
            {
                string windtype = "Sixteen";
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["DateTime"].ToString() != "")
                        dr["Tstamp"] = Convert.ToDateTime(dr["DateTime"]).ToString("yyyy年MM月");
                    if (windtype == "Sixteen")
                    {
                        if (dr[factorCodeWindDir] != null)
                        {
                            dr["Wind"] = (SixteenGetWindName(Convert.ToDouble(dr[factorCodeWindDir])));
                        }
                    }
                    else
                    {
                        if (dr[factorCodeWindDir] != null)
                        {
                            dr["Wind"] = EightGetWindName(Convert.ToDouble(dr[factorCodeWindDir]));
                        }
                    }
                    foreach (string factor in factors)
                    {
                        if ((factor == "a34004" || factor == "a34002" || factor == "a21026" || factor == "a21004" || factor == "a05024") && dr[factor].ToString() != "")
                            dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]) * 1000, 0);
                        if (factor == "a21005" && dr[factor].ToString() != "")
                            dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]), 1);
                    }
                }
            }
            return dt.DefaultView;

        }
        /// <summary>
        /// 原始数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataView GetAuditWeekData(string portIds, string[] factors, int yearFrom, int weekOfYearFrom, int yearTo, int weekOfYearTo)
        {
            DataTable dt = m_GranuleSpecial.GetAuditWeekData(portIds, factors, yearFrom, weekOfYearFrom, yearTo, weekOfYearTo).ToTable();
            dt.Columns.Add("Tstamp", typeof(string));
            dt.Columns.Add("Wind", typeof(string));
            for (int i = 0; i < dt.Rows.Count; i++)
            {

            }
            string factorCodeWindDir = "a01008";//风向

            if (dt.Rows.Count > 0)
            {
                string windtype = "Sixteen";
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["WeekOfYear"].ToString() != "")
                        dr["Tstamp"] = dr["Year"].ToString() + "年" + dr["WeekOfYear"].ToString() + "周";
                    if (windtype == "Sixteen")
                    {
                        if (dr[factorCodeWindDir] != null)
                        {
                            dr["Wind"] = (SixteenGetWindName(Convert.ToDouble(dr[factorCodeWindDir])));
                        }
                    }
                    else
                    {
                        if (dr[factorCodeWindDir] != null)
                        {
                            dr["Wind"] = EightGetWindName(Convert.ToDouble(dr[factorCodeWindDir]));
                        }
                    }
                    foreach (string factor in factors)
                    {
                        if ((factor == "a34004" || factor == "a34002" || factor == "a21026" || factor == "a21004" || factor == "a05024") && dr[factor].ToString() != "")
                            dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]) * 1000, 0);
                        if (factor == "a21005" && dr[factor].ToString() != "")
                            dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]), 1);
                    }
                }
            }
            return dt.DefaultView;

        }
        /// <summary>
        /// 原始数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataView GetAuditSeasonData(string portIds, string[] factors, int yearFrom, int seasonOfYearFrom, int yearTo, int seasonOfYearTo)
        {
            DataTable dt = m_GranuleSpecial.GetAuditSeasonData(portIds, factors, yearFrom, seasonOfYearFrom, yearTo, seasonOfYearTo).ToTable();
            dt.Columns.Add("Tstamp", typeof(string));
            dt.Columns.Add("Wind", typeof(string));
            for (int i = 0; i < dt.Rows.Count; i++)
            {

            }
            string factorCodeWindDir = "a01008";//风向

            if (dt.Rows.Count > 0)
            {
                string windtype = "Sixteen";
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["SeasonOfYear"].ToString() != "")
                        dr["Tstamp"] = dr["Year"].ToString() + "年" + dr["SeasonOfYear"].ToString() + "季";
                    if (windtype == "Sixteen")
                    {
                        if (dr[factorCodeWindDir] != null)
                        {
                            dr["Wind"] = (SixteenGetWindName(Convert.ToDouble(dr[factorCodeWindDir])));
                        }
                    }
                    else
                    {
                        if (dr[factorCodeWindDir] != null)
                        {
                            dr["Wind"] = EightGetWindName(Convert.ToDouble(dr[factorCodeWindDir]));
                        }
                    }
                    foreach (string factor in factors)
                    {
                        if ((factor == "a34004" || factor == "a34002" || factor == "a21026" || factor == "a21004" || factor == "a05024") && dr[factor].ToString() != "")
                            dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]) * 1000, 0);
                        if (factor == "a21005" && dr[factor].ToString() != "")
                            dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]), 1);
                    }
                }
            }
            return dt.DefaultView;

        }
        /// <summary>
        /// 原始数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataView GetAuditYearData(string portIds, string[] factors, int yearFrom, int yearTo)
        {
            DataTable dt = m_GranuleSpecial.GetAuditYearData(portIds, factors, yearFrom, yearTo).ToTable();
            dt.Columns.Add("Tstamp", typeof(string));
            dt.Columns.Add("Wind", typeof(string));
            for (int i = 0; i < dt.Rows.Count; i++)
            {

            }
            string factorCodeWindDir = "a01008";//风向

            if (dt.Rows.Count > 0)
            {
                string windtype = "Sixteen";
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["Year"].ToString() != "")
                        dr["Tstamp"] = dr["Year"].ToString() + "年";
                    if (windtype == "Sixteen")
                    {
                        if (dr[factorCodeWindDir] != null)
                        {
                            dr["Wind"] = (SixteenGetWindName(Convert.ToDouble(dr[factorCodeWindDir])));
                        }
                    }
                    else
                    {
                        if (dr[factorCodeWindDir] != null)
                        {
                            dr["Wind"] = EightGetWindName(Convert.ToDouble(dr[factorCodeWindDir]));
                        }
                    }
                    foreach (string factor in factors)
                    {
                        if ((factor == "a34004" || factor == "a34002" || factor == "a21026" || factor == "a21004" || factor == "a05024") && dr[factor].ToString() != "")
                            dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]) * 1000, 0);
                        if (factor == "a21005" && dr[factor].ToString() != "")
                            dr[factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dr[factor]), 1);
                    }
                }
            }
            return dt.DefaultView;

        }
    }
}
