using SmartEP.AMSRepository.Water;
using SmartEP.BaseInfoRepository.Dictionary;
using SmartEP.BaseInfoRepository.MPInfo;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.Data.SqlServer.BaseData;
using SmartEP.Data.SqlServer.MonitoringBusiness.Water;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.BaseData.Standard;
using SmartEP.Service.Core.Enums;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.DataAnalyze.Water.DataQuery
{
    /// <summary>
    /// 名称：RealTimeWaterQualityStatus.cs
    /// 创建人：窦曙健
    /// 创建日期：2015-09-1
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：实时水质分析
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class RealTimeWaterQualityStatus
    {
        /// <summary>
        /// 实时水质
        /// </summary>
        /// <param name="portType">站点类型</param>
        /// <param name="portIds">站点编号</param>
        /// <param name="factors">评价因子</param>
        /// <param name="dateStart"开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public DataView RealTimeWaterQuality(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd)
        {
            int recordTotal = 0;
            string portType = "";
            WaterAnalyzeDAL d_WaterAnalyze = new WaterAnalyzeDAL();
            DataTable dt_WaterAnalyze = d_WaterAnalyze.GetWaterAnalyzeData(portIds).Table;//获取测点功能水质评价
            WaterQualityService s_WaterQuality = new WaterQualityService();
            MonitoringPointWaterService s_MonitoringPointWater = new MonitoringPointWaterService();
            CodeMainItemRepository r_CodeMainItem = new CodeMainItemRepository();
            InfectantBy60Repository g_InfectantBy60Repository = Singleton<InfectantBy60Repository>.GetInstance();
            DataTable dt = g_InfectantBy60Repository.GetDataPager(portIds, factors, dateStart, dateEnd, 1000, 0, out recordTotal).Table;
            dt.Columns.Add("portType", typeof(string)).SetOrdinal(0);
            dt.Columns.Add("Grade", typeof(string));
            dt.Columns.Add("Range", typeof(string));
            string EvaluateFactorCodes = "";
            foreach (string factor in factors)
            {
                dt.Columns.Remove(factor + "_Status");
                dt.Columns.Remove(factor + "_Mark");
                dt.Columns.Add("WQI_" + factor, typeof(string));//分指数
                dt.Columns.Add("WQL_" + factor, typeof(string));//等级
                EvaluateFactorCodes += factor + ";";
            }
            EvaluateFactorCodes = EvaluateFactorCodes.Substring(0, EvaluateFactorCodes.Length - 1);//获取评价因子
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Dictionary<string, Int32> WQLValues = new Dictionary<string, int>();
                    int pointId = Convert.ToInt32(dt.Rows[i]["PointId"]);//获取站点Id
                    DataRow[] dr = dt_WaterAnalyze.Select("PointId=" + pointId);
                    if (dr.Length > 0)
                    {
                        string EQI = dr[0]["IEQI"].ToString();
                        string CalEQIType = dr[0]["CalEQIType"].ToString();
                        foreach (string factor in factors)
                        {
                            string pollutantValue = dt.Rows[i][factor].ToString();
                            decimal WQI;
                            string WQL;
                            if (!string.IsNullOrEmpty(pollutantValue))
                            {
                                #region 获取分指数
                                switch (EQI)
                                {
                                    case "1":
                                        if (CalEQIType == "湖泊")
                                            WQI = s_WaterQuality.GetWQI(factor, Convert.ToDecimal(pollutantValue), WaterQualityClass.One, EQITimeType.One, WaterPointCalWQType.Lake);
                                        else
                                            WQI = s_WaterQuality.GetWQI(factor, Convert.ToDecimal(pollutantValue), WaterQualityClass.One, EQITimeType.One, WaterPointCalWQType.River);
                                        break;
                                    case "2":
                                        if (CalEQIType == "湖泊")
                                            WQI = s_WaterQuality.GetWQI(factor, Convert.ToDecimal(pollutantValue), WaterQualityClass.Two, EQITimeType.One, WaterPointCalWQType.Lake);
                                        else
                                            WQI = s_WaterQuality.GetWQI(factor, Convert.ToDecimal(pollutantValue), WaterQualityClass.Two, EQITimeType.One, WaterPointCalWQType.River);
                                        break;
                                    case "3":
                                        if (CalEQIType == "湖泊")
                                            WQI = s_WaterQuality.GetWQI(factor, Convert.ToDecimal(pollutantValue), WaterQualityClass.Three, EQITimeType.One, WaterPointCalWQType.Lake);
                                        else
                                            WQI = s_WaterQuality.GetWQI(factor, Convert.ToDecimal(pollutantValue), WaterQualityClass.Three, EQITimeType.One, WaterPointCalWQType.River);
                                        break;
                                    case "4":
                                        if (CalEQIType == "湖泊")
                                            WQI = s_WaterQuality.GetWQI(factor, Convert.ToDecimal(pollutantValue), WaterQualityClass.Four, EQITimeType.One, WaterPointCalWQType.Lake);
                                        else
                                            WQI = s_WaterQuality.GetWQI(factor, Convert.ToDecimal(pollutantValue), WaterQualityClass.Four, EQITimeType.One, WaterPointCalWQType.River);
                                        break;
                                    case "5":
                                        if (CalEQIType == "湖泊")
                                            WQI = s_WaterQuality.GetWQI(factor, Convert.ToDecimal(pollutantValue), WaterQualityClass.Five, EQITimeType.One, WaterPointCalWQType.Lake);
                                        else
                                            WQI = s_WaterQuality.GetWQI(factor, Convert.ToDecimal(pollutantValue), WaterQualityClass.Five, EQITimeType.One, WaterPointCalWQType.River);
                                        break;
                                    case "6":
                                        if (CalEQIType == "湖泊")
                                            WQI = s_WaterQuality.GetWQI(factor, Convert.ToDecimal(pollutantValue), WaterQualityClass.BadFive, EQITimeType.One, WaterPointCalWQType.Lake);
                                        else
                                            WQI = s_WaterQuality.GetWQI(factor, Convert.ToDecimal(pollutantValue), WaterQualityClass.BadFive, EQITimeType.One, WaterPointCalWQType.River);
                                        break;
                                    default:
                                        if (CalEQIType == "湖泊")
                                            WQI = s_WaterQuality.GetWQI(factor, Convert.ToDecimal(pollutantValue), WaterQualityClass.BadFive, EQITimeType.One, WaterPointCalWQType.Lake);
                                        else
                                            WQI = s_WaterQuality.GetWQI(factor, Convert.ToDecimal(pollutantValue), WaterQualityClass.BadFive, EQITimeType.One, WaterPointCalWQType.River);
                                        break;
                                }
                                #endregion
                                #region 获取等级
                                switch (CalEQIType)
                                {
                                    case "湖泊":
                                        WQL = s_WaterQuality.GetWQL(factor, Convert.ToDecimal(pollutantValue), EQITimeType.One, WaterPointCalWQType.Lake, EQIReurnType.Level);
                                        break;
                                    case "河流":
                                        WQL = s_WaterQuality.GetWQL(factor, Convert.ToDecimal(pollutantValue), EQITimeType.One, WaterPointCalWQType.River, EQIReurnType.Level);
                                        break;
                                    default:
                                        WQL = s_WaterQuality.GetWQL(factor, Convert.ToDecimal(pollutantValue), EQITimeType.One, WaterPointCalWQType.River, EQIReurnType.Level);
                                        break;
                                }
                                #endregion
                                dt.Rows[i]["WQI_" + factor] = WQI.ToString();
                                dt.Rows[i]["WQL_" + factor] = WQL;

                                if (!string.IsNullOrEmpty(WQL.ToString()))
                                {
                                    WQLValues.Add(factor, Convert.ToInt32(WQL));
                                }
                            }
                        }
                        #region 获取水质级别
                        string Grade = s_WaterQuality.GetWQL_Max(EQIReurnType.Value, EvaluateFactorCodes, WQLValues);
                        dt.Rows[i]["Grade"] = Grade;
                        if (!string.IsNullOrEmpty(Grade) & !string.IsNullOrEmpty(EQI))
                        {
                            if (Convert.ToDouble(Grade) <= Convert.ToDouble(EQI))
                            {
                                dt.Rows[i]["Range"] = "达标";
                            }
                            else
                            {
                                dt.Rows[i]["Range"] = "未达标";
                            }
                        }
                        else
                        {
                            dt.Rows[i]["Range"] = "/";
                        }
                        #endregion
                        #region 获取站点类型
                        string SiteTypeUid = s_MonitoringPointWater.RetrieveEntityByPointId(pointId).SiteTypeUid;//获取站点类型Uid
                        if (!string.IsNullOrEmpty(SiteTypeUid))
                        {
                            portType = r_CodeMainItem.RetrieveFirstOrDefault(p => p.ItemGuid == SiteTypeUid).ItemText;//根据站点类型Uid获取站点类型名称
                        }
                        else
                        {
                            portType = "/";
                        }
                        #endregion
                        dt.Rows[i]["portType"] = portType;
                    }
                }
            }
            return dt.DefaultView;
        }

        /// <summary>
        /// 当前整体水质评价
        /// </summary>
        /// <param name="portType"></param>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataView WaterAllAnalysis(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Grade", typeof(string));
            dt.Columns.Add("PrimaryPollutant", typeof(string));
            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("Range", typeof(string));
            dt.Columns.Add("Type", typeof(string));

            DataRow dr = dt.NewRow();
            dr["Date"] = dateEnd.ToString("yyyy-MM-dd");

            #region 获取功能区水质
            WaterAnalyzeDAL d_WaterAnalyze = new WaterAnalyzeDAL();
            DataTable dt_WaterAnalyzeData = d_WaterAnalyze.GetWaterAnalyzeData(portIds).Table;
            string EQI = dt_WaterAnalyzeData.Rows[0]["IEQI"].ToString();//获取功能水质EQI
            string CalEQIType = dt_WaterAnalyzeData.Rows[0]["CalEQIType"].ToString();//获取功能水质水质类别
            #endregion
            #region 获取质量级别和首要污染物
            WaterQualityService s_WaterQuality = new WaterQualityService();
            #region 获取参与评价因子
            string EvaluateFactorCodes = "";
            for (int i = 0; i < factors.Length; i++)
            {
                EvaluateFactorCodes += factors[i] + ";";
            }
            EvaluateFactorCodes = EvaluateFactorCodes.Substring(0, EvaluateFactorCodes.Length - 1);
            #endregion
            #region 因子指数列表
            Dictionary<string, Int32> WQIValues = new Dictionary<string, int>();
            InfectantBy60Repository s_DataQueryByHour = new InfectantBy60Repository();
            DataTable dt_Values = s_DataQueryByHour.GetStatisticalData(portIds, factors, dateStart, dateEnd).Table;//获取均值数据
            for (int i = 0; i < dt_Values.Rows.Count; i++)
            {
                decimal WQI;
                string WQL;
                string pollutantCode = dt_Values.Rows[i]["PollutantCode"].ToString();//污染物code
                string pollutantValue = dt_Values.Rows[i]["Value_Avg"].ToString();//污染物浓度
                if (!string.IsNullOrEmpty(pollutantValue))
                {
                    decimal value = Convert.ToDecimal(pollutantValue);
                    #region 获取分指数
                    //switch (EQI)
                    //{
                    //    case "1":
                    //        if (CalEQIType == "湖泊")
                    //            WQI = s_WaterQuality.GetWQI(pollutantCode, Convert.ToDecimal(pollutantValue), WaterQualityClass.One, EQITimeType.One, WaterPointCalWQType.Lake);
                    //        else
                    //            WQI = s_WaterQuality.GetWQI(pollutantCode, Convert.ToDecimal(pollutantValue), WaterQualityClass.One, EQITimeType.One, WaterPointCalWQType.River);
                    //        break;
                    //    case "2":
                    //        if (CalEQIType == "湖泊")
                    //            WQI = s_WaterQuality.GetWQI(pollutantCode, Convert.ToDecimal(pollutantValue), WaterQualityClass.Two, EQITimeType.One, WaterPointCalWQType.Lake);
                    //        else
                    //            WQI = s_WaterQuality.GetWQI(pollutantCode, Convert.ToDecimal(pollutantValue), WaterQualityClass.Two, EQITimeType.One, WaterPointCalWQType.River);
                    //        break;
                    //    case "3":
                    //        if (CalEQIType == "湖泊")
                    //            WQI = s_WaterQuality.GetWQI(pollutantCode, Convert.ToDecimal(pollutantValue), WaterQualityClass.Three, EQITimeType.One, WaterPointCalWQType.Lake);
                    //        else
                    //            WQI = s_WaterQuality.GetWQI(pollutantCode, Convert.ToDecimal(pollutantValue), WaterQualityClass.Three, EQITimeType.One, WaterPointCalWQType.River);
                    //        break;
                    //    case "4":
                    //        if (CalEQIType == "湖泊")
                    //            WQI = s_WaterQuality.GetWQI(pollutantCode, Convert.ToDecimal(pollutantValue), WaterQualityClass.Four, EQITimeType.One, WaterPointCalWQType.Lake);
                    //        else
                    //            WQI = s_WaterQuality.GetWQI(pollutantCode, Convert.ToDecimal(pollutantValue), WaterQualityClass.Four, EQITimeType.One, WaterPointCalWQType.River);
                    //        break;
                    //    case "5":
                    //        if (CalEQIType == "湖泊")
                    //            WQI = s_WaterQuality.GetWQI(pollutantCode, Convert.ToDecimal(pollutantValue), WaterQualityClass.Five, EQITimeType.One, WaterPointCalWQType.Lake);
                    //        else
                    //            WQI = s_WaterQuality.GetWQI(pollutantCode, Convert.ToDecimal(pollutantValue), WaterQualityClass.Five, EQITimeType.One, WaterPointCalWQType.River);
                    //        break;
                    //    case "6":
                    //        if (CalEQIType == "湖泊")
                    //            WQI = s_WaterQuality.GetWQI(pollutantCode, Convert.ToDecimal(pollutantValue), WaterQualityClass.BadFive, EQITimeType.One, WaterPointCalWQType.Lake);
                    //        else
                    //            WQI = s_WaterQuality.GetWQI(pollutantCode, Convert.ToDecimal(pollutantValue), WaterQualityClass.BadFive, EQITimeType.One, WaterPointCalWQType.River);
                    //        break;
                    //    default:
                    //        if (CalEQIType == "湖泊")
                    //            WQI = s_WaterQuality.GetWQI(pollutantCode, Convert.ToDecimal(pollutantValue), WaterQualityClass.BadFive, EQITimeType.One, WaterPointCalWQType.Lake);
                    //        else
                    //            WQI = s_WaterQuality.GetWQI(pollutantCode, Convert.ToDecimal(pollutantValue), WaterQualityClass.BadFive, EQITimeType.One, WaterPointCalWQType.River);
                    //        break;
                    //}
                    #endregion
                    #region 获取等级
                    switch (CalEQIType)
                    {
                        case "湖泊":
                            WQL = s_WaterQuality.GetWQL(pollutantCode, Convert.ToDecimal(pollutantValue), EQITimeType.One, WaterPointCalWQType.Lake, EQIReurnType.Level);
                            break;
                        case "河流":
                            WQL = s_WaterQuality.GetWQL(pollutantCode, Convert.ToDecimal(pollutantValue), EQITimeType.One, WaterPointCalWQType.River, EQIReurnType.Level);
                            break;
                        default:
                            WQL = s_WaterQuality.GetWQL(pollutantCode, Convert.ToDecimal(pollutantValue), EQITimeType.One, WaterPointCalWQType.River, EQIReurnType.Level);
                            break;
                    }
                    #endregion
                    if (!string.IsNullOrEmpty(WQL.ToString()))
                        WQIValues.Add(pollutantCode, Convert.ToInt32(WQL));
                }
            }
            #endregion
            //质量级别
            string Grade = s_WaterQuality.GetWQL_Max(EQIReurnType.Roman, EvaluateFactorCodes, WQIValues);
            dr["Grade"] = Grade;
            //首要污染物
            string PrimaryPollutant = s_WaterQuality.GetWQL_Max(EQIReurnType.Name, EvaluateFactorCodes, WQIValues);
            dr["PrimaryPollutant"] = PrimaryPollutant;
            #endregion
            //获取实时水质数据
            DataTable dt_WaterQuality = RealTimeWaterQuality(portIds, factors, dateStart, dateEnd).Table;
            if (dt_WaterQuality.Rows.Count > 0)
            {
                dr["Type"] = dt_WaterQuality.Rows[0]["portType"].ToString();
                decimal total = dt_WaterQuality.Rows.Count;
                decimal Standard = 0;
                for (int i = 0; i < dt_WaterQuality.Rows.Count; i++)
                {
                    string grade = dt_WaterQuality.Rows[i]["Grade"].ToString();
                    if (!string.IsNullOrEmpty(grade) && !string.IsNullOrEmpty(EQI))
                    {
                        if (Convert.ToInt32(grade) <= Convert.ToInt32(EQI))
                        {
                            Standard++;
                        }
                    }
                }
                decimal RangeValue = (Standard / total) * 100;
                string Range = Math.Round(RangeValue, 0).ToString() + "%";
                if (!string.IsNullOrEmpty(EQI))
                    dr["Range"] = Range;
                else
                    dr["Range"] = "/";
            }
            dt.Rows.Add(dr);
            return dt.DefaultView;
        }

        /// <summary>
        /// 类别统计
        /// </summary>
        /// <param name="portType">站点类型</param>
        /// <param name="portIds">站点编号</param>
        /// <param name="factors">评价因子</param>
        /// <param name="dateStart"开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public DataView WaterQualityGradeStatistical(string portType, string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd)
        {
            HourReportDAL hourReportDAL = new HourReportDAL();
            DataTable dt = RealTimeWaterQuality(portIds, factors, dateStart, dateEnd).Table;
            DataTable dtNew = new DataTable();
            int WQL1 = 0;
            int WQL2 = 0;
            int WQL3 = 0;
            int WQL4 = 0;
            int WQL5 = 0;
            int WQL6 = 0;
            dt.Columns["portType"].SetOrdinal(0);
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    dt.Rows[i]["portType"] = portType;
            //}
            dtNew.Columns.Add("Grade");
            dtNew.Columns.Add("Count");
            dtNew.Columns.Add("Rate");
            dtNew.Columns.Add("Color");
            if (dt.Rows.Count > 0)
            {
                if (portIds.Length == 1)
                {
                    #region 单侧点，多因子
                    for (int m = 0; m < dt.DefaultView.Count; m++)
                    {
                        if (!string.IsNullOrEmpty(dt.Rows[m]["Grade"].ToString()))
                        {
                            int curWQL = int.TryParse(dt.Rows[m]["Grade"].ToString(), out curWQL) ? curWQL : 0;
                            switch (curWQL)
                            {
                                case 0: break;
                                case 1:
                                    WQL1++;
                                    break;
                                case 2:
                                    WQL2++;
                                    break;
                                case 3:
                                    WQL3++;
                                    break;
                                case 4:
                                    WQL4++;
                                    break;
                                case 5:
                                    WQL5++;
                                    break;
                                case 6:
                                    WQL6++;
                                    break;
                                default:
                                    WQL6++;
                                    break;
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    #region 多测点

                    #region
                    //for (int i = 0; i < dt.Rows.Count; i++)
                    //{
                    //    int maxWQL = 0;
                    //    foreach (string factor in factors)
                    //    {
                    //        if (!string.IsNullOrEmpty(dt.Rows[i]["WQL_" + factor].ToString()))
                    //        {
                    //            int curWQL = int.TryParse(dt.Rows[i]["WQL_" + factor].ToString(), out curWQL) ? curWQL : 0;
                    //            maxWQL = (maxWQL > curWQL) ? maxWQL : curWQL;
                    //        }
                    //    }
                    //    switch (maxWQL)
                    //    {
                    //        case 0: break;
                    //        case 1:
                    //            WQL1++;
                    //            break;
                    //        case 2:
                    //            WQL2++;
                    //            break;
                    //        case 3:
                    //            WQL3++;
                    //            break;
                    //        case 4:
                    //            WQL4++;
                    //            break;
                    //        case 5:
                    //            WQL5++;
                    //            break;
                    //        case 6:
                    //            WQL6++;
                    //            break;
                    //        default:
                    //            WQL6++;
                    //            break;
                    //    }
                    //}
                    #endregion

                    #region
                    foreach (string pointId in portIds)
                    {
                        DataTable dtMaxWQL = WaterAllAnalysis(new string[] { pointId }, factors, dateStart, dateEnd).Table;
                        if (dtMaxWQL.Rows.Count > 0)
                        {
                            int maxWQL = int.TryParse(dtMaxWQL.Rows[0]["WaterLevel"].ToString(), out maxWQL) ? maxWQL : 0;
                            switch (maxWQL)
                            {
                                case 0: break;
                                case 1:
                                    WQL1++;
                                    break;
                                case 2:
                                    WQL2++;
                                    break;
                                case 3:
                                    WQL3++;
                                    break;
                                case 4:
                                    WQL4++;
                                    break;
                                case 5:
                                    WQL5++;
                                    break;
                                case 6:
                                    WQL6++;
                                    break;
                                default:
                                    WQL6++;
                                    break;
                            }
                        }
                    }
                    #endregion
                    #endregion
                }
            }
            for (int i = 1; i <= 6; i++)
            {
                DataRow drNew = dtNew.NewRow();
                string grade = string.Empty;
                string color = string.Empty;
                int count = 0;
                decimal rate = 0;
                switch (i)
                {
                    case 1:
                        grade = "Ⅰ";
                        count = WQL1;
                        color = "#00ffff";
                        break;
                    case 2:
                        grade = "Ⅱ";
                        count = WQL2;
                        color = "#00ffff";
                        break;
                    case 3:
                        grade = "Ⅲ";
                        count = WQL3;
                        color = "green";
                        break;
                    case 4:
                        grade = "Ⅳ";
                        count = WQL4;
                        color = "yellow";
                        break;
                    case 5:
                        grade = "Ⅴ";
                        count = WQL5;
                        color = "orange";
                        break;
                    case 6:
                        grade = "劣Ⅴ";
                        count = WQL6;
                        color = "red";
                        break;
                    default: break;
                }
                if (WQL1 + WQL2 + WQL3 + WQL4 + WQL5 + WQL6 > 0)
                {
                    rate = Math.Round((decimal)count * 100 / (WQL1 + WQL2 + WQL3 + WQL4 + WQL5 + WQL6), 2);
                }
                drNew["Grade"] = grade;
                drNew["Count"] = count;
                drNew["Rate"] = rate;
                drNew["Color"] = color;
                dtNew.Rows.Add(drNew);
            }
            return dtNew.DefaultView;
        }
    }
}
