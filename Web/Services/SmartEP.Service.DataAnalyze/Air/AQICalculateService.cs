using log4net;
using SmartEP.Data.SqlServer.MonitoringBusiness.Air;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.DataAnalyze.Air
{
    /// <summary>
    /// 名称：AQIService.aspx.cs
    /// 创建人：吕云
    /// 创建日期：2017-5-18
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：AQI计算
    /// 虚拟分页类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class AQICalculateService
    {
        #region 变量
        ILog log = LogManager.GetLogger("FileLogging");//获取一个日志记录器

        AQICalculateDAL d_AQIDAL = new AQICalculateDAL();

        #endregion

        #region 方法
        /// <summary>
        /// 计算空气质量分指数
        /// </summary>
        /// <param name="PollutantCode">因子编码</param>
        /// <param name="PollutantValue">浓度值</param>
        /// <param name="TimeType">时间类型：1小时，8小时，24小时</param>
        /// 公式：IAQIP=(IAQIH-IAQIL)/(BPH-BPL)*(CP-BPL)+IAQIL
        /// IAQIP:空气质量指数
        /// CP:污染物浓度
        /// BPL:小于或等于CP的浓度限值
        /// BPH:大于或等于CP的浓度限值
        /// IAQIL:对应于BPL的指数限值
        /// IAQIH:对应于BPH的指数限值
        /// <returns></returns>
        public int? GetIAQI(string PollutantCode, double? PollutantValue, int TimeType)
        {
            try
            {
                if (PollutantValue == null)
                {
                    return null;
                }
                DataTable dtFactor = GetPollutantUnit(PollutantCode);
                int decimalNum = Convert.ToInt32(dtFactor.Rows[0]["DecimalDigit"]);

                decimal CP = Convert.ToDecimal(DecimalExtension.GetPollutantValue(Convert.ToDecimal(PollutantValue), decimalNum));//浓度值
                string TimeTypeUid;//时间类型Uid
                decimal IAQIH = 500;
                decimal IAQIL = 0;
                decimal BPH = 9999;
                decimal BPL = 0;
                int? IAQIP;
                //int index;//标记行索引
                Boolean flag = true;//标记是否超过浓度限值
                switch (TimeType)
                {
                    case 1:
                        TimeTypeUid = "7c67a857-d602-4f90-a26d-edd3e9f4d36c";
                        break;
                    case 8:
                        TimeTypeUid = "1cc20274-210c-4c22-8fe8-553b46ddf112";
                        break;
                    case 24:
                        TimeTypeUid = "a7056afa-9c7f-4876-8853-9c95c5d7e2b3";
                        break;
                    default:
                        TimeTypeUid = "7c67a857-d602-4f90-a26d-edd3e9f4d36c";
                        break;

                }
                DataTable dtLimitAndIAQI = d_AQIDAL.GetFactorLimitAndIAQI(PollutantCode, TimeTypeUid);
                for (int i = 0; i < dtLimitAndIAQI.Rows.Count; i++)
                {
                    BPH = Convert.ToDecimal(dtLimitAndIAQI.Rows[i]["Upper"]);
                    BPL = Convert.ToDecimal(dtLimitAndIAQI.Rows[i]["Low"]);
                    if ((CP >= BPL && CP <= BPH))
                    {
                        IAQIH = Convert.ToDecimal(dtLimitAndIAQI.Rows[i]["IAQI"].ToString());
                        IAQIL = i > 0 ? Convert.ToDecimal(dtLimitAndIAQI.Rows[i - 1]["IAQI"].ToString()) : 0;
                        //index = i;
                        flag = false;//因子浓度未超过最大浓度限值
                        break;
                    }
                }
                if (flag == true && dtLimitAndIAQI.Rows.Count > 0)//因子浓度超过最大浓度限值
                {
                    //取最大限值和最小限值
                    BPH = Convert.ToDecimal(dtLimitAndIAQI.Rows[0]["Upper"]);
                    BPL = Convert.ToDecimal(dtLimitAndIAQI.Rows[dtLimitAndIAQI.Rows.Count - 1]["Upper"]);
                    IAQIH = Convert.ToDecimal(dtLimitAndIAQI.Rows[0]["IAQI"].ToString());
                    IAQIL = Convert.ToDecimal(dtLimitAndIAQI.Rows[dtLimitAndIAQI.Rows.Count - 1]["IAQI"].ToString());
                }
                IAQIP = Convert.ToInt32(Math.Ceiling((IAQIH - IAQIL) * (CP - BPL) / (BPH - BPL) + IAQIL));//先乘后除防止精度丢失
                if (IAQIP > 500)
                    IAQIP = 500;
                if (IAQIP <= 0)
                    IAQIP = null;
                return IAQIP;
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 获取AQI/首要污染物
        /// </summary>
        /// <param name="AQI_SO2"></param>
        /// <param name="AQI_NO2"></param>
        /// <param name="AQI_PM10"></param>
        /// <param name="AQI_CO"></param>
        /// <param name="AQI_O3_8"></param>
        /// <param name="AQI_PM25"></param>
        /// <param name="ReturnType">返回信息，C:Code,N:名称,V:AQI值,S:中文名</param>
        /// <returns></returns>
        public string GetAQI_Max_CNV(int? AQI_SO2, int? AQI_NO2, int? AQI_PM10, int? AQI_CO, int? AQI_O3, int? AQI_PM25, string ReturnType)
        {
            try
            {
                int? AQI_MaxValue = -9999;
                string AQI_Max_FactorCode = string.Empty;
                string AQI_Max_FactorName = string.Empty;
                string AQI_Max_FactorSpName = string.Empty;
                string ReturnStr = string.Empty;

                if (AQI_SO2 != null)
                {
                    if (AQI_MaxValue < AQI_SO2)
                    {
                        AQI_MaxValue = AQI_SO2;
                        AQI_Max_FactorCode = "a21026";
                        AQI_Max_FactorName = "SO2";
                        AQI_Max_FactorSpName = "二氧化硫";
                    }
                }
                if (AQI_NO2 != null)
                {
                    if (AQI_MaxValue < AQI_NO2)
                    {
                        AQI_MaxValue = AQI_NO2;
                        AQI_Max_FactorCode = "a21004";
                        AQI_Max_FactorName = "NO2";
                        AQI_Max_FactorSpName = "二氧化氮";
                    }
                    else if (AQI_MaxValue == AQI_NO2)
                    {
                        AQI_Max_FactorCode = AQI_Max_FactorCode + ",a21004";
                        AQI_Max_FactorName = AQI_Max_FactorName + ",NO2";
                        AQI_Max_FactorSpName = AQI_Max_FactorSpName + ",二氧化氮";
                    }
                }
                if (AQI_PM10!=null)
                {
                    if (AQI_MaxValue < AQI_PM10)
                    {
                        AQI_MaxValue = AQI_PM10;
                        AQI_Max_FactorCode = "a34002";
                        AQI_Max_FactorName = "PM10";
                        AQI_Max_FactorSpName = "可吸入颗粒物";
                    }
                    else if (AQI_MaxValue == AQI_PM10)
                    {
                        AQI_Max_FactorCode = AQI_Max_FactorCode + ",a34002";
                        AQI_Max_FactorName = AQI_Max_FactorName + ",PM10";
                        AQI_Max_FactorSpName = AQI_Max_FactorSpName + ",可吸入颗粒物";
                    }
                }
                if (AQI_CO != null)
                {
                    if (AQI_MaxValue < AQI_CO)
                    {
                        AQI_MaxValue = AQI_CO;
                        AQI_Max_FactorCode = "a21005";
                        AQI_Max_FactorName = "CO";
                        AQI_Max_FactorSpName = "一氧化碳";
                    }
                    else if (AQI_MaxValue == AQI_CO)
                    {
                        AQI_Max_FactorCode = AQI_Max_FactorCode + ",a21005";
                        AQI_Max_FactorName = AQI_Max_FactorName + ",CO";
                        AQI_Max_FactorSpName = AQI_Max_FactorSpName + ",一氧化碳";
                    }
                }
                if (AQI_O3!=null)
                {
                    if (AQI_MaxValue < AQI_O3)
                    {
                        AQI_MaxValue = AQI_O3;
                        AQI_Max_FactorCode = "a05024";
                        AQI_Max_FactorName = "O3";
                        AQI_Max_FactorSpName = "臭氧";
                    }
                    else if (AQI_MaxValue == AQI_O3)
                    {
                        AQI_Max_FactorCode = AQI_Max_FactorCode + ",a05024";
                        AQI_Max_FactorName = AQI_Max_FactorName + ",O3";
                        AQI_Max_FactorSpName = AQI_Max_FactorSpName + ",臭氧";
                    }
                }
                if (AQI_PM25 != null)
                {
                    if (AQI_MaxValue < AQI_PM25)
                    {
                        AQI_MaxValue = AQI_PM25;
                        AQI_Max_FactorCode = "a34004";
                        AQI_Max_FactorName = "PM2.5";
                        AQI_Max_FactorSpName = "细微颗粒物";
                    }
                    else if (AQI_MaxValue == AQI_PM25)
                    {
                        AQI_Max_FactorCode = AQI_Max_FactorCode + ",a34004";
                        AQI_Max_FactorName = AQI_Max_FactorName + ",PM2.5";
                        AQI_Max_FactorSpName = AQI_Max_FactorSpName + ",细微颗粒物";
                    }
                }

                if (AQI_MaxValue == -9999)
                {
                    AQI_MaxValue = null;
                    AQI_Max_FactorCode = null;
                    AQI_Max_FactorName = null;
                    AQI_Max_FactorSpName = null;
                }
                else if (AQI_MaxValue <= 50)
                {
                    AQI_Max_FactorCode = "--";
                    AQI_Max_FactorName = "--";
                    AQI_Max_FactorSpName = "--";
                }
                else
                {
                    
                }
                #region 参与AQI计算的因子有无效数据 如果其他有效因子有超标 则当天超标 如果有效因子不超标  则当天无效
                ArrayList AQIList = new ArrayList();
                AQIList.Add(AQI_SO2);
                AQIList.Add(AQI_NO2);
                AQIList.Add(AQI_PM10);
                AQIList.Add(AQI_CO);
                AQIList.Add(AQI_O3);
                AQIList.Add(AQI_PM25);
                foreach (int? AQI in AQIList)
                {
                    if (AQI == null && AQI_MaxValue <= 100)
                    {
                        AQI_MaxValue = null;
                        AQI_Max_FactorCode = null;
                        AQI_Max_FactorName = null;
                        AQI_Max_FactorSpName = null;
                        break;
                    }
                }
                #endregion
                if (ReturnType.ToUpper() == "C")
                {
                    ReturnStr = AQI_Max_FactorCode;
                }
                else if (ReturnType.ToUpper() == "N")
                {
                    ReturnStr = AQI_Max_FactorName;
                }
                else if (ReturnType.ToUpper() == "V")
                {
                    if (AQI_MaxValue != null)
                        ReturnStr = AQI_MaxValue.ToString();
                    else
                        ReturnStr = null;
                }
                else if (ReturnType.ToUpper() == "S")
                {
                    ReturnStr = AQI_Max_FactorSpName;
                }
                return ReturnStr;
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 区域因子浓度计算值
        /// </summary>
        /// <param name="PointIds">站点</param>
        /// <param name="PollutantCode">因子编码</param>
        /// <param name="Tstamp">时间</param>
        /// <param name="TimeType">时间类型：1小时均值，8小时均值，24小时均值</param>
        /// <param name="Type">原始：1，审核：2</param>
        /// <returns></returns>
        public decimal? GetRegionValue(string[] PointIds, string PollutantCode, DateTime Tstamp, int TimeType,string Type)
        {
            try
            {
                PointIds = PointIds.Distinct().ToArray();
                int count = PointIds.Count();
                DataTable dtFactor = GetPollutantUnit(PollutantCode);
                int decimalNum = Convert.ToInt32(dtFactor.Rows[0]["DecimalDigit"]);

                decimal? value = null;
                switch (TimeType)
                {
                    case 1:
                        {
                            #region 1小时均值
                            DataTable dt = d_AQIDAL.GetHourRegionValue(PointIds, PollutantCode, Tstamp, Type);
                            int EffectCount = dt.Rows.Count;
                            //log.Info("有效测点数量:" + EffectCount + "选中测点数量：" + count + "有效测点占比：" + EffectCount * 1.0 / count);
                            if (count>0&&count < 4)
                            {
                                if (EffectCount * 1.0 / count >= 0.5)
                                {
                                    value = Convert.ToDecimal(dt.Compute("Sum(" + "PollutantValue" + ")", "true"))/dt.Rows.Count;
                                    //log.Info("浓度值：" + value);
                                }
                                //else if (EffectCount > 0)
                                //{
                                //    value = Convert.ToDecimal(dt.Compute("Max(PollutantValue)", "true"));
                                //}
                                else
                                {
                                    value = null;
                                }
                            }
                            else if (count >= 4)
                            {
                                if (EffectCount * 1.0 / count >= 0.75)
                                {
                                    
                                    value = Convert.ToDecimal(dt.Compute("Sum(" + "PollutantValue" + ")", "true")) / dt.Rows.Count;
                                    //log.Info("浓度值：" + value);
                                }
                                //else if (EffectCount > 0)
                                //{
                                //    value = Convert.ToDecimal(dt.Compute("Max(PollutantValue)", "true"));
                                //}
                                else
                                {
                                    value = null;
                                }
                            }
                            #endregion
                            break;
                        }
                    case 8:
                        {
                            if (PollutantCode == "a05027")
                            {
                                #region 臭氧最近8小时均值（可跨天）
                                DataTable dt = d_AQIDAL.GetO3_8NTRegionValue(PointIds, Tstamp,Type);
                                int EffectCount = dt.Rows.Count;
                                if (count < 4)
                                {
                                    if (EffectCount * 1.0 / count >= 0.5)
                                    {
                                        value = (dt.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<string>("Recent8HoursO3NT"))).Sum()) / dt.Rows.Count;
                                    }
                                    else if (EffectCount > 0)
                                    {
                                        value = dt.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<string>("Recent8HoursO3NT"))).Max();
                                    }
                                    else
                                    {
                                        value = null;
                                    }
                                }
                                else if (count >= 4)
                                {
                                    if (EffectCount * 1.0 / count >= 0.75)
                                    {

                                        value = (dt.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<string>("Recent8HoursO3NT"))).Sum()) / dt.Rows.Count;
                                        //log.Info("浓度值：" + value);
                                    }
                                    else if (EffectCount > 0)
                                    {
                                        value = dt.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<string>("Recent8HoursO3NT"))).Max();
                                        //log.Info("浓度值：" + value);
                                    }
                                    else
                                    {
                                        value = null;
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                #region 臭氧日最大8小时均值
                                DataTable dt = d_AQIDAL.GetO3_8RegionValue(PointIds, Tstamp, Type);
                                int EffectCount = dt.Rows.Count;
                                if (count < 4)
                                {
                                    if (EffectCount * 1.0 / count >= 0.5)
                                    {
                                        value = (dt.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<string>("Max8HourO3"))).Sum()) / dt.Rows.Count;
                                        //log.Info("浓度值：" + value);
                                    }
                                    else if (EffectCount > 0)
                                    {
                                        value = dt.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<string>("Max8HourO3"))).Max();
                                        //log.Info("浓度值：" + value);
                                    }
                                    else
                                    {
                                        value = null;
                                    }
                                }
                                else if (count >= 4)
                                {
                                    if (EffectCount * 1.0 / count >= 0.75)
                                    {
                                        value = (dt.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<string>("Max8HourO3"))).Sum()) / dt.Rows.Count;
                                        //log.Info("浓度值：" + value);
                                    }
                                    else if (EffectCount > 0)
                                    {
                                        value = dt.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<string>("Max8HourO3"))).Max();
                                        //log.Info("浓度值：" + value);
                                    }
                                    else
                                    {
                                        value = null;
                                    }
                                }
                                #endregion
                            }
                            break;
                        }
                    case 24:
                        {
                            #region 24小时均值
                            DataTable dt = d_AQIDAL.GetDayRegionValue(PointIds, PollutantCode, Tstamp,Type);
                            int EffectCount = dt.Rows.Count;
                            if (count < 4)
                            {
                                if (EffectCount * 1.0 / count >= 0.5)
                                {
                                    value = Convert.ToDecimal(dt.Compute("Sum(" + "PollutantValue" + ")", "true")) / dt.Rows.Count;
                                    //log.Info("浓度值：" + value);
                                }
                                else if (EffectCount > 0)
                                {
                                    value = Convert.ToDecimal(dt.Compute("Max(PollutantValue)", "true"));
                                    //log.Info("浓度值：" + value);
                                }
                                else
                                {
                                    value = null;
                                }
                            }
                            else if (count >= 4)
                            {
                                if (EffectCount * 1.0 / count >= 0.75)
                                {
                                    value = Convert.ToDecimal(dt.Compute("Sum(" + "PollutantValue" + ")", "true")) / dt.Rows.Count;
                                    //log.Info("浓度值：" + value);
                                }
                                else if (EffectCount > 0)
                                {
                                    value = Convert.ToDecimal(dt.Compute("Max(PollutantValue)", "true"));
                                    //log.Info("浓度值：" + value);
                                }
                                else
                                {
                                    value = null;
                                }
                            }
                            #endregion
                            break;
                        }
                }
                //log.Info("最终返回的浓度值：" + value);
                return value;
                
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 获取区域AQI
        /// </summary>
        /// <param name="PointIds">站点</param>
        /// <param name="StartTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <param name="TimeType">时间类型：1：小时AQI,24：日AQI</param>
        /// <param name="Type">数据类型：1：原始，2审核</param>
        /// <returns></returns>
        public DataTable GetRegionAQI(string[] PointIds, DateTime StartTime, DateTime EndTime, int TimeType, string Type)
        {
            try 
            {
                PointIds = PointIds.Distinct().ToArray();
                DataTable dt=new DataTable();
                switch (TimeType)
                {
                    case 1:
                        {
                            string tableName = string.Empty;
                            if (Type == "2")
                            {
                                tableName = "AirRelease.TB_HourAQI";
                            }
                            else
                            {
                                tableName = "dbo.SY_OriHourAQI";
                            }
                            dt = d_AQIDAL.GetRegionHourData(PointIds, StartTime, EndTime, tableName);

                            break;
                        }
                    case 24:
                        {
                            string tableName = string.Empty;
                            if (Type == "2")
                            {
                                tableName = "AirRelease.TB_DayAQI";
                            }
                            else
                            {
                                tableName = "dbo.SY_OriDayAQI";
                            }
                            dt = d_AQIDAL.GetRegionDayData(PointIds, StartTime, EndTime, tableName);

                            break;
                        }

                }
                return dt;
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 区域因子浓度时间均值(用于时段均值计算)
        /// </summary>
        /// <param name="PointIds">站点</param>
        /// <param name="PollutantCode">因子编码</param>
        /// <param name="Tstamp">时间</param>
        /// <param name="TimeType">时间类型：小时均值，日均值</param>
        /// <param name="Type">原始：1，审核：2</param>
        /// <returns></returns>
        public decimal? GetRegionValueByTime(string[] PointIds, string PollutantCode, DateTime StartTime,DateTime EndTime, int TimeType,string Type)
        {
            try
            {
                PointIds = PointIds.Distinct().ToArray();
                int count = PointIds.Count();
                decimal? value = null;
                switch (TimeType)
                {
                    case 1:
                        {
                            if (PollutantCode == "a05024")
                            {
                                #region 小时时段均值臭氧算法（不可跨天）
                                DataTable dt = d_AQIDAL.GetO3_8RegionValueByHours(PointIds, StartTime, EndTime, Type);
                                int EffectCount = dt.Rows.Count;
                                if (count < 4)
                                {
                                    if (EffectCount * 1.0 / count >= 0.5)
                                    {
                                        value = (dt.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<decimal>("Recent8HoursO3"))).Sum()) / dt.Rows.Count;
                                    }
                                    else if (EffectCount > 0)
                                    {
                                        value = dt.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<decimal>("Recent8HoursO3"))).Max();
                                    }
                                    else
                                    {
                                        value = null;
                                    }
                                }
                                else if (count >= 4)
                                {
                                    if (EffectCount * 1.0 / count >= 0.75)
                                    {
                                        value = (dt.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<decimal>("Recent8HoursO3"))).Sum()) / dt.Rows.Count;
                                        //log.Info("浓度值：" + value);
                                    }
                                    else if (EffectCount > 0)
                                    {
                                        value = dt.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<decimal>("Recent8HoursO3"))).Max();
                                        //log.Info("浓度值：" + value);
                                    }
                                    else
                                    {
                                        value = null;
                                    }
                                }
                                #endregion
                            }
                            else if (PollutantCode == "a05027")
                            {
                                #region 小时时段均值臭氧算法（可跨天）
                                DataTable dt = d_AQIDAL.GetNTO3_8RegionValueByHours(PointIds, StartTime, EndTime, Type);
                                int EffectCount = dt.Rows.Count;
                                if (count < 4)
                                {
                                    if (EffectCount * 1.0 / count >= 0.5)
                                    {
                                        value = (dt.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<decimal>("Recent8HoursO3"))).Sum()) / dt.Rows.Count;
                                    }
                                    else if (EffectCount > 0)
                                    {
                                        value = dt.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<decimal>("Recent8HoursO3"))).Max();
                                    }
                                    else
                                    {
                                        value = null;
                                    }
                                }
                                else if (count >= 4)
                                {
                                    if (EffectCount * 1.0 / count >= 0.75)
                                    {
                                        value = (dt.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<decimal>("Recent8HoursO3"))).Sum()) / dt.Rows.Count;
                                        //log.Info("浓度值：" + value);
                                    }
                                    else if (EffectCount > 0)
                                    {
                                        value = dt.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<decimal>("Recent8HoursO3"))).Max();
                                        //log.Info("浓度值：" + value);
                                    }
                                    else
                                    {
                                        value = null;
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                #region 小时均值
                                DataTable dt = d_AQIDAL.GetHourRegionValueByTime(PointIds, PollutantCode, StartTime, EndTime, Type);
                                int EffectCount = dt.Rows.Count;
                                if (count < 4)
                                {
                                    if (EffectCount * 1.0 / count >= 0.5)
                                    {
                                        value = Convert.ToDecimal(dt.Compute("Sum(" + "PollutantValue" + ")", "true")) / dt.Rows.Count;
                                    }
                                    else if (EffectCount > 0)
                                    {
                                        value = Convert.ToDecimal(dt.Compute("Max(PollutantValue)", "true"));
                                    }
                                    else
                                    {
                                        value = null;
                                    }
                                }
                                else if (count >= 4)
                                {
                                    if (EffectCount * 1.0 / count >= 0.75)
                                    {
                                        value = Convert.ToDecimal(dt.Compute("Sum(" + "PollutantValue" + ")", "true")) / dt.Rows.Count;
                                    }
                                    else if (EffectCount > 0)
                                    {
                                        value = Convert.ToDecimal(dt.Compute("Max(PollutantValue)", "true"));
                                    }
                                    else
                                    {
                                        value = null;
                                    }
                                }
                                #endregion
                            }
                            break;
                        }
                    case 8:
                        {
                            if (PollutantCode == "a05027")
                            {
                                #region 臭氧最近8小时均值（可跨天）
                                DataTable dt = d_AQIDAL.GetO3_8NTRegionValueByTime(PointIds, StartTime, EndTime,Type);
                                int EffectCount = dt.Rows.Count;
                                if (count < 4)
                                {
                                    if (EffectCount * 1.0 / count >= 0.5)
                                    {
                                        value = (dt.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<string>("Recent8HoursO3NT"))).Sum()) / dt.Rows.Count;
                                    }
                                    else if (EffectCount > 0)
                                    {
                                        value = dt.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<string>("Recent8HoursO3NT"))).Max();
                                    }
                                    else
                                    {
                                        value = null;
                                    }
                                }
                                else if (count >= 4)
                                {
                                    if (EffectCount * 1.0 / count >= 0.75)
                                    {
                                        value = (dt.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<string>("Recent8HoursO3NT"))).Sum()) / dt.Rows.Count;
                                        //log.Info("浓度值：" + value);
                                    }
                                    else if (EffectCount > 0)
                                    {
                                        value = dt.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<string>("Recent8HoursO3NT"))).Max();
                                        //log.Info("浓度值：" + value);
                                    }
                                    else
                                    {
                                        value = null;
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                #region 臭氧日最大8小时均值
                                DataTable dt = d_AQIDAL.GetO3_8RegionValueByTime(PointIds, StartTime, EndTime,Type);
                                int EffectCount = dt.Rows.Count;
                                if (count < 4)
                                {
                                    if (EffectCount * 1.0 / count >= 0.5)
                                    {
                                        value = (dt.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<string>("Max8HourO3"))).Sum()) / dt.Rows.Count;
                                    }
                                    else if (EffectCount > 0)
                                    {
                                        value = dt.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<string>("Max8HourO3"))).Max();
                                    }
                                    else
                                    {
                                        value = null;
                                    }
                                }
                                else if (count >= 4)
                                {
                                    if (EffectCount * 1.0 / count >= 0.75)
                                    {
                                        value = (dt.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<string>("Max8HourO3"))).Sum()) / dt.Rows.Count;
                                    }
                                    else if (EffectCount > 0)
                                    {
                                        value = dt.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<string>("Max8HourO3"))).Max();
                                    }
                                    else
                                    {
                                        value = null;
                                    }
                                }
                                #endregion
                            }
                            break;
                        }
                    case 24:
                        {
                            #region 日均值
                            DataTable dt = d_AQIDAL.GetDayRegionValueByTime(PointIds, PollutantCode, StartTime, EndTime,Type);
                            int EffectCount = dt.Rows.Count;
                            if (count < 4)
                            {
                                if (EffectCount * 1.0 / count >= 0.5)
                                {
                                    value = Convert.ToDecimal(dt.Compute("Sum(" + "PollutantValue" + ")", "true")) / dt.Rows.Count;
                                }
                                else if (EffectCount > 0)
                                {
                                    value = Convert.ToDecimal(dt.Compute("Max(PollutantValue)", "true"));
                                }
                                else
                                {
                                    value = null;
                                }
                            }
                            else if (count >= 4)
                            {
                                if (EffectCount * 1.0 / count >= 0.75)
                                {
                                    value = Convert.ToDecimal(dt.Compute("Sum(" + "PollutantValue" + ")", "true")) / dt.Rows.Count;
                                }
                                else if (EffectCount > 0)
                                {
                                    value = Convert.ToDecimal(dt.Compute("Max(PollutantValue)", "true"));
                                }
                                else
                                {
                                    value = null;
                                }
                            }
                            #endregion
                            break;
                        }
                }
                return value;
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        /// 计算AQI空气质量等级相关数据
        /// </summary>
        /// <param name="AQI_MaxValue">AQI</param>
        /// <param name="ReturnType">返回类型：指数等级范围(Range),空气质量级别(Grade),空气质量级别罗马字符(Roman),空气质量类别(Class),空气质量状况(Condition),对应颜色(Color),对应颜色代码(RGBValue)</param>
        /// <returns></returns>
        public string GetAQI_Grade(int AQI_MaxValue, string ReturnType)
        {
            try
            {
                string Range = string.Empty;//指数等级范围
                string Grade = string.Empty;//空气质量级别
                string Roman = string.Empty;//空气质量级别罗马字符
                string Class = string.Empty;//空气质量类别
                string Condition = string.Empty;//空气质量状况
                string Color = string.Empty;//对应颜色
                string RGBValue = string.Empty;//对应颜色代码
                string Alabo = string.Empty;//空气质量级别阿拉伯字符
                string HealthEffect = string.Empty;//对健康的影响
                string TakeStep = string.Empty;//建议采取措施

                string ReturnStr = string.Empty;
                if (AQI_MaxValue >= 0 && AQI_MaxValue <= 50)
                {
                    Range = "0~50";
                    Grade = "一级";
                    Roman = "I";
                    Class = "优";
                    Condition = "优(I)";
                    Color = "#00e400";
                    RGBValue = "0,228,0";
                    Alabo = "1";
                    HealthEffect = "空气质量令人满意，基本无空气污染";
                    TakeStep = "各类人群可正常活动";
                }
                else if (AQI_MaxValue >= 51 && AQI_MaxValue <= 100)
                {
                    Range = "51~100";
                    Grade = "二级";
                    Roman = "II";
                    Class = "良";
                    Condition = "良(II)";
                    Color = "#ffff00";
                    RGBValue = "255,255,0";
                    Alabo = "2";
                    HealthEffect = "空气质量可接受，但某些污染物可能对极少数异常敏感人群健康有较弱影响";
                    TakeStep = "极少数异常敏感人群应减少户外活动";

                }
                else if (AQI_MaxValue >= 101 && AQI_MaxValue <= 150)
                {
                    Range = "101~150";
                    Grade = "三级";
                    Roman = "III";
                    Class = "轻度污染";
                    Condition = "轻度污染(III)";
                    Color = "#ff7e00";
                    RGBValue = "255,126,0";
                    Alabo = "3";
                    HealthEffect = "易感人群症状有轻度加剧，健康人群出现刺激症状";
                    TakeStep = "儿童、老年人及心脏病、呼吸系统疾病患者应减少长时间、高强度的户外锻炼";

                }
                else if (AQI_MaxValue >= 151 && AQI_MaxValue <= 200)
                {
                    Range = "151~200";
                    Grade = "四级";
                    Roman = "IV";
                    Class = "中度污染";
                    Condition = "中度污染(IV)";
                    Color = "#ff0000";
                    RGBValue = "255,0,0";
                    Alabo = "4";
                    HealthEffect = "进一步加剧易感人群症状，可能对 健康人群心脏、呼吸系统有影响";
                    TakeStep = "儿童、老年人及心脏病、呼吸系统 疾病患者避免长时间、高强度的户外锻练，一般人群适量减少户外运动";

                }
                else if (AQI_MaxValue >= 201 && AQI_MaxValue <= 300)
                {
                    Range = "201~300";
                    Grade = "五级";
                    Roman = "V";
                    Class = "重度污染";
                    Condition = "重度污染(V)";
                    Color = "#99004c";
                    RGBValue = "153,0,76";
                    Alabo = "5";
                    HealthEffect = "心脏病和肺病患者症状显著加剧，运动耐受力降低，健康人群普遍出 现症状";
                    TakeStep = "儿童、老年人和心脏病、肺病患者应停留在室内，停止户外运动，一般人群减少户外运动";

                }
                else if (AQI_MaxValue >= 301)
                {
                    Range = ">300";
                    Grade = "六级";
                    Roman = "VI";
                    Class = "严重污染";
                    Condition = "严重污染(VI)";
                    Color = "#7e0023";
                    RGBValue = "126,0,35";
                    Alabo = "6";
                    HealthEffect = "健康人群运动耐受力降低，有明显 强烈症状，提前出现某些疾病";
                    TakeStep = "老年人和病人应当留在室内，避免体力消耗，一般人群应避免户外活动";

                }

                if (ReturnType.ToUpper() == "RANGE")
                {
                    ReturnStr = Range;
                }
                if (ReturnType.ToUpper() == "GRADE")
                {
                    ReturnStr = Grade;
                }
                if (ReturnType.ToUpper() == "ROMAN")
                {
                    ReturnStr = Roman;
                }
                if (ReturnType.ToUpper() == "CLASS")
                {
                    ReturnStr = Class;
                }
                if (ReturnType.ToUpper() == "CONDITION")
                {
                    ReturnStr = Condition;
                }
                if (ReturnType.ToUpper() == "COLOR")
                {
                    ReturnStr = Color;
                }
                if (ReturnType.ToUpper() == "RGBVALUE")
                {
                    ReturnStr = RGBValue;
                }
                if (ReturnType.ToUpper() == "ALABO")
                {
                    ReturnStr = Alabo;
                }
                if (ReturnType.ToUpper() == "HEALTHEFFECT")
                {
                    ReturnStr = HealthEffect;
                }
                if (ReturnType.ToUpper() == "TAKESTEP")
                {
                    ReturnStr = TakeStep;
                }
                return ReturnStr;
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 获取因子相关配置信息
        /// </summary>
        /// <param name="PollutantCode"></param>
        /// <returns></returns>
        public DataTable GetPollutantUnit(string PollutantCode)
        {
            return d_AQIDAL.GetPollutantUnit(PollutantCode);
        }
        /// <summary>
        /// 把为无效的值变为null
        /// </summary>
        /// <param name="Param">参数值</param>
        /// <param name="Status">标记位</param>
        /// <param name="sparator">分隔符</param>
        /// <returns></returns>
        public decimal? ValidValueByFlag(decimal Param, string Flag, string sparator)
        {
            try
            {
                string[] FlagStr = Flag.Split(Convert.ToChar(sparator));
                DataTable dtFlag = d_AQIDAL.GetFlag(FlagStr);

                if (dtFlag.Rows.Count > 0)
                {
                    return null;
                }
                else
                {
                    return Param;
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }
        #endregion
    }
}
