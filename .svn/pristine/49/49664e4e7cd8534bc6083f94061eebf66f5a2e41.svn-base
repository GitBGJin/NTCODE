using SmartEP.AMSRepository.Water;
using SmartEP.Core.Generic;
using SmartEP.MonitoringBusinessRepository.QualityControlOperation.Water;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.DataAnalyze.Water
{
    public class InstrumentParameterAnalyzeService
    {
        public DataTable GetList(string portId, string factor, string[] numTypes, DateTime dtStart, DateTime dtEnd, int DecimalNum, string orderBy = "PointId")
        {
            int recordTotal = 0;
            string[] portIds = { portId };
            string[] factors = { factor };
            DataTable dt = new DataTable();
            dt.Columns.Add("DateTime", typeof(DateTime));//时间
            dt.Columns.Add("numType", typeof(string));//数据类型
            dt.Columns.Add("PollutantValue", typeof(string));//测定值
            dt.Columns.Add("StandardValue", typeof(string));//标准值
            dt.Columns.Add("addValue", typeof(string));//水样值
            dt.Columns.Add("RelativeOffset", typeof(string));//相对误差
            dt.Columns.Add("AbsoluteOffset", typeof(string));//绝对误差
            dt.Columns.Add("Recovery", typeof(string));//加标回收
            if (numTypes.Contains("realtime"))
            {
                /// <summary>
                /// 60分钟数据仓储层
                /// </summary>
                InfectantBy60Repository g_InfectantBy60Repository = Singleton<InfectantBy60Repository>.GetInstance();
                DataView dvOriginal = g_InfectantBy60Repository.GetDataPager(portIds, factors, dtStart, dtEnd, int.MaxValue, 0, out recordTotal, orderBy);
                for (int i = 0; i < dvOriginal.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["DateTime"] = dvOriginal[i]["Tstamp"];
                    dr["numType"] = "正常水样";
                    decimal PollutantValue = 0;
                    if (decimal.TryParse(dvOriginal[i][factor].ToString(), out PollutantValue))
                    {
                        PollutantValue = Convert.ToDecimal(dvOriginal[i][factor].ToString());
                        dr["PollutantValue"] = DecimalExtension.GetPollutantValue(PollutantValue, DecimalNum).ToString();
                    }
                    dt.Rows.Add(dr);
                }
            }
            if (numTypes.Length > 1 || (numTypes.Length == 1 && !numTypes.Contains("realtime")))
            {
                /// <summary>
                /// 质控数据仓储类
                /// </summary>
                StandardSolutionCheckRepository g_StandardSolutionCheckRepository = Singleton<StandardSolutionCheckRepository>.GetInstance();
                string strWhere = " 1=1 ";
                if (!string.IsNullOrWhiteSpace(portId))
                {
                    strWhere += string.Format(" and PointId ='{0}' ", portId);
                }
                if (!string.IsNullOrWhiteSpace(factor))
                {
                    strWhere += string.Format(" and PollutantCode ='{0}' ", factor);
                }
                if (dtStart != null)
                {
                    strWhere += string.Format(" and Tstamp >='{0}' ", dtStart);
                }
                if (dtEnd != null)
                {
                    strWhere += string.Format(" and Tstamp <='{0}' ", dtEnd);
                }
                DataTable qualityData = g_StandardSolutionCheckRepository.GetList(strWhere);
                if (qualityData.DefaultView.Count > 0)
                {
                    if (numTypes.Contains("8897D4CF-EA8C-450B-9009-149232DF2985"))
                    {
                        DataRow[] drSample = qualityData.Select("MissionID='8897D4CF-EA8C-450B-9009-149232DF2985'");
                        foreach (DataRow dritem in drSample)
                        {
                            DataRow dr = dt.NewRow();
                            dr["DateTime"] = dritem["Tstamp"];
                            dr["numType"] = "实样比对";
                            decimal PollutantValue = 0;
                            if (decimal.TryParse(dritem["PollutantValue"].ToString(), out PollutantValue))
                            {
                                PollutantValue = Convert.ToDecimal(dritem["PollutantValue"].ToString());
                                dr["PollutantValue"] = DecimalExtension.GetPollutantValue(PollutantValue, DecimalNum).ToString();
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                    if (numTypes.Contains("5E469A23-76F9-4F53-9D98-F63D8DF199F0"))
                    {
                        DataRow[] drStandard = qualityData.Select("MissionID='5E469A23-76F9-4F53-9D98-F63D8DF199F0'");
                        foreach (DataRow dritem in drStandard)
                        {
                            DataRow dr = dt.NewRow();
                            dr["DateTime"] = dritem["Tstamp"];
                            dr["numType"] = "标样考核";
                            decimal PollutantValue = 0;
                            if (decimal.TryParse(dritem["PollutantValue"].ToString(), out PollutantValue))
                            {
                                PollutantValue = Convert.ToDecimal(dritem["PollutantValue"].ToString());
                                dr["PollutantValue"] = DecimalExtension.GetPollutantValue(PollutantValue, DecimalNum).ToString();
                            }//测定值
                            decimal StandardValue = 0;
                            if (decimal.TryParse(dritem["StandardValue"].ToString(), out StandardValue))
                            {
                                StandardValue = Convert.ToDecimal(dritem["StandardValue"].ToString());
                                dr["StandardValue"] = DecimalExtension.GetPollutantValue(StandardValue, DecimalNum).ToString();
                            }//标准值
                            decimal RelativeOffset = 0;
                            if (decimal.TryParse(dritem["RelativeOffset"].ToString(), out RelativeOffset))
                            {
                                RelativeOffset = Convert.ToDecimal(dritem["RelativeOffset"].ToString());
                                dr["RelativeOffset"] = Math.Round(RelativeOffset, 1);//相对误差
                            }
                            decimal AbsoluteOffset = 0;
                            if (decimal.TryParse(dritem["AbsoluteOffset"].ToString(), out AbsoluteOffset))
                            {
                                AbsoluteOffset = Convert.ToDecimal(dritem["AbsoluteOffset"].ToString());
                                dr["AbsoluteOffset"] = Math.Round(AbsoluteOffset, 2);//相对误差
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                    if (numTypes.Contains("7D32FAB8-07CE-41A1-807C-C0B8A161322D"))
                    {
                        DataRow[] drBlind = qualityData.Select("MissionID='7D32FAB8-07CE-41A1-807C-C0B8A161322D'");
                        foreach (DataRow dritem in drBlind)
                        {
                            DataRow dr = dt.NewRow();
                            dr["DateTime"] = dritem["Tstamp"];
                            dr["numType"] = "盲样考核";
                            decimal PollutantValue = 0;
                            if (decimal.TryParse(dritem["PollutantValue"].ToString(), out PollutantValue))
                            {
                                PollutantValue = Convert.ToDecimal(dritem["PollutantValue"].ToString());
                                dr["PollutantValue"] = DecimalExtension.GetPollutantValue(PollutantValue, DecimalNum).ToString();
                            }//测定值
                            decimal StandardValue = 0;
                            if (decimal.TryParse(dritem["StandardValue"].ToString(), out StandardValue))
                            {
                                StandardValue = Convert.ToDecimal(dritem["StandardValue"].ToString());
                                dr["StandardValue"] = DecimalExtension.GetPollutantValue(StandardValue, DecimalNum).ToString();
                            }//标准值
                            decimal RelativeOffset = 0;
                            if (decimal.TryParse(dritem["RelativeOffset"].ToString(), out RelativeOffset))
                            {
                                RelativeOffset = Convert.ToDecimal(dritem["RelativeOffset"].ToString());
                                dr["RelativeOffset"] = Math.Round(RelativeOffset, 1);//相对误差
                            }
                            decimal AbsoluteOffset = 0;
                            if (decimal.TryParse(dritem["AbsoluteOffset"].ToString(), out AbsoluteOffset))
                            {
                                AbsoluteOffset = Convert.ToDecimal(dritem["AbsoluteOffset"].ToString());
                                dr["AbsoluteOffset"] = Math.Round(AbsoluteOffset, 2);//相对误差
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                    if (numTypes.Contains("6F183194-3B3E-4337-A57F-5D17845544A7"))
                    {
                        DataRow[] drAdd = qualityData.Select("MissionID='6F183194-3B3E-4337-A57F-5D17845544A7'");
                        foreach (DataRow dritem in drAdd)
                        {
                            DataRow dr = dt.NewRow();
                            dr["DateTime"] = dritem["Tstamp"];
                            dr["numType"] = "加标回收";
                            decimal PollutantValue = 0;
                            if (decimal.TryParse(dritem["PollutantValue"].ToString(), out PollutantValue))
                            {
                                PollutantValue = Convert.ToDecimal(dritem["PollutantValue"].ToString());
                                dr["PollutantValue"] = DecimalExtension.GetPollutantValue(PollutantValue, DecimalNum).ToString();
                            }//测定值
                            decimal StandardValue = 0;
                            if (decimal.TryParse(dritem["PollutantValueAdd"].ToString(), out StandardValue))
                            {
                                StandardValue = Convert.ToDecimal(dritem["PollutantValueAdd"].ToString());
                                dr["StandardValue"] = DecimalExtension.GetPollutantValue(StandardValue, DecimalNum).ToString();
                            }//标准值
                            decimal addValue = 0;
                            if (decimal.TryParse(dritem["StandardValue"].ToString(), out addValue))
                            {
                                addValue = Convert.ToDecimal(dritem["StandardValue"].ToString());
                                dr["addValue"] = DecimalExtension.GetPollutantValue(addValue, DecimalNum).ToString();
                            }//水样值
                            decimal RelativeOffset = 0;
                            if (decimal.TryParse(dritem["RelativeOffset"].ToString(), out RelativeOffset))
                            {
                                RelativeOffset = Convert.ToDecimal(dritem["RelativeOffset"].ToString());
                                dr["RelativeOffset"] = Math.Round(RelativeOffset, 1);//相对误差
                            }
                            decimal AbsoluteOffset = 0;
                            if (decimal.TryParse(dritem["AbsoluteOffset"].ToString(), out AbsoluteOffset))
                            {
                                AbsoluteOffset = Convert.ToDecimal(dritem["AbsoluteOffset"].ToString());
                                dr["AbsoluteOffset"] = Math.Round(AbsoluteOffset, 2);//相对误差
                            }
                            decimal Recovery = 0;
                            if (decimal.TryParse(dritem["UniversalValue3"].ToString(), out Recovery))
                            {
                                Recovery = Convert.ToDecimal(dritem["UniversalValue3"].ToString());
                                dr["Recovery"] = Math.Round(Recovery, 1);//加标回收率
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                }
            }
            DataView dv = dt.DefaultView;
            dv.Sort = "DateTime DESC";
            dt = dv.Table;
            return dt;
        }
    }
}
