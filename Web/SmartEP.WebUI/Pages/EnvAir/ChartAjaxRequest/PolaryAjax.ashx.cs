﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using SmartEP.Core.Enums;
using SmartEP.Service.DataAnalyze.Air.PolaryWind;
using SmartEP.Service.DataAnalyze.Air.DataQuery;
using SmartEP.Core.Generic;
using SmartEP.Service.BaseData.Channel;
using System.Collections.ObjectModel;
using SmartEP.Service.AutoMonitoring.Interfaces;
using SmartEP.Service.AutoMonitoring.Air;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.DomainModel.BaseData;

namespace SmartEP.WebUI.Pages.EnvAir.ChartAjaxRequest
{
    /// <summary>
    /// PolaryWindDirectionAjax 的摘要说明
    /// </summary>
    public class PolaryWindDirectionAjax : IHttpHandler
    {
        DataQueryByHourService m_HourData = Singleton<DataQueryByHourService>.GetInstance();
        DataQueryByDayService m_DayData = Singleton<DataQueryByDayService>.GetInstance();
        AirPollutantService airPollutantService = new AirPollutantService();
        PolaryWindService windService = new PolaryWindService();
        private IInfectantDALService g_IInfectantDALService = null;
        string factorCodeWindDir = "a01008";//风向
        string factorCodeWindSpeed = "a01007";//风速
        public void ProcessRequest(HttpContext context)
        {
            #region 获取参数
            string[] portid = context.Request["PointID"].ToString().Split(';');
            string[] factorCode = context.Request["FactorCode"].ToString().Split(';');
            DateTime dtBegin = Convert.ToDateTime(context.Request["dtBegin"].ToString());
            DateTime dtEnd = Convert.ToDateTime(context.Request["dtEnd"].ToString());
            string radlDataType = context.Request["radlDataType"].ToString();
            string WindDir = context.Request["WindDir"].ToString();
            string PolaryType = context.Request["PolaryType"].ToString();
            string flag = context.Request["flag"].ToString();
            #endregion

            List<PolaryData> list = GetDataSource(portid, factorCode, radlDataType, dtBegin, dtEnd, WindDir, PolaryType, flag);//获取数据源
            if (list != null)
            {
                string jsonStr = CreatePolaryJsonData(list, factorCode, WindDir, PolaryType);//封装json格式
                context.Response.ContentType = "text/plain";
                context.Response.Write(jsonStr);//返回json数据
            }
            else
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("Without Wind");//返回json数据
            }
        }

        #region 方法
        /// <summary>
        /// 玫瑰图绘图更改,创建json数据
        /// </summary>
        /// <param name="list"></param>
        /// <param name="factorCode"></param>
        /// <param name="WindDir"></param>
        /// <param name="ChartType"></param>
        /// <returns></returns>
        private string CreatePolaryJsonData(List<PolaryData> list, string[] factorCode, string WindDir, string ChartType)
        {
            string windName = "";
            if (WindDir.Equals("Sixteen"))
                windName = "北风;东北偏北风;东北风;东北偏东风;东风;东南偏东风;东南风;东南偏南风;南风;西南偏南风;西南风;西南偏西风;西风;西北偏西风;西北风;西北偏北风";
            else
                windName = "北风;东北风;东风;东南风;南风;西南风;西风;西北风";
            StringBuilder sb = new StringBuilder();
            string factorName = "";
            int num = 0;
            string indicator = "";

            sb.Append("{");
            #region Data
            sb.Append("series:[");
            foreach (string fac in factorCode)
            {
                if (fac != factorCodeWindDir && fac != factorCodeWindSpeed)
                {
                    SmartEP.Core.Interfaces.IPollutant pollutant = airPollutantService.GetPollutantInfo(fac);
                    factorName += pollutant.PollutantName + ",";

                    sb.Append("{");
                    if (fac != "a21005")
                    {
                        sb.AppendFormat(@"name: '{0}',yAxis: 0,type:'{1}',pointPlacement: '{2}'", pollutant.PollutantName + " 浓度", "column", "on");
                    }
                    else if (fac == "a21005")
                    {
                        sb.AppendFormat(@"name: '{0}',yAxis: 3,type:'{1}',pointPlacement: '{2}',color:'yellow'", pollutant.PollutantName + " 浓度", "column", "on");
                    }
                    string str = "";
                    foreach (string name in windName.Split(';'))
                    {
                        //foreach (PolaryData polary in list.Where(x => x.FactorCoce.Equals(fac)))
                        //{
                        PolaryData polary = list.Where(x => x.FactorCoce.Equals(fac) && x.FX.Equals(name)).FirstOrDefault();

                        #region value
                        if (polary != null)
                        {
                            if (polary.ND != null)
                            {
                                str += polary.ND + ",";
                            }
                            else
                            {
                                str += "[],";
                            }
                        }
                        else
                        {
                            str += "[],";
                        }
                        #endregion

                        #region indicator
                        if (num == 0)
                            indicator += " '" + name + "',";
                        //indicator += "{ text: '" + name + "', max: 360 },";
                        #endregion
                        //}
                    }
                    if (str.Substring(str.ToString().Length - 1, 1).Trim().Equals(","))
                        str = str.ToString().Substring(0, str.ToString().Length - 1);//去除最后一个逗号
                    sb.AppendFormat(",data:[{0}]", str);
                    sb.Append("},");
                    num++;
                }
            }

            sb.Append("{");
            sb.AppendFormat(@"name: '{0}',yAxis: 1,type:'{1}',pointPlacement: '{2}',color:'blue'", "次数", "line", "on");
            string strCount = "";
            foreach (string name in windName.Split(';'))
            {
                //foreach (PolaryData polary in list.Where(x => x.FactorCoce.Equals(fac)))
                //{
                PolaryData polary = list.Where(x => x.FX.Equals(name)).FirstOrDefault();

                #region value
                if (polary != null)
                {
                    if (polary.FactorCount != null)
                    {
                        strCount += polary.FactorCount + ",";
                    }
                    else
                    {
                        strCount += "[],";
                    }
                }
                else
                {
                    strCount += "[],";
                }
                #endregion
                //}
            }
            if (strCount.Substring(strCount.ToString().Length - 1, 1).Trim().Equals(","))
                strCount = strCount.ToString().Substring(0, strCount.ToString().Length - 1);//去除最后一个逗号
            sb.AppendFormat(",data:[{0}]", strCount);
            sb.Append("},");

            sb.Append("{");
            sb.AppendFormat(@"name: '{0}',yAxis: 2,type:'{1}',pointPlacement: '{2}',color:'green'", "风速", "line", "on");
            string strSpeed = "";
            foreach (string name in windName.Split(';'))
            {
                //foreach (PolaryData polary in list.Where(x => x.FactorCoce.Equals(fac)))
                //{
                PolaryData polary = list.Where(x => x.FX.Equals(name)).FirstOrDefault();

                #region value
                if (polary != null)
                {
                    if (polary.FactorCount != null)
                    {
                        strSpeed += polary.FS + ",";
                    }
                    else
                    {
                        strSpeed += "[],";
                    }
                }
                else
                {
                    strSpeed += "[],";
                }
                #endregion
                //}
            }
            if (strSpeed.Substring(strSpeed.ToString().Length - 1, 1).Trim().Equals(","))
                strSpeed = strSpeed.ToString().Substring(0, strSpeed.ToString().Length - 1);//去除最后一个逗号
            sb.AppendFormat(",data:[{0}]", strSpeed);
            sb.Append("},");

            if (sb.ToString().Substring(sb.ToString().Length - 1, 1).Trim().Equals(","))
                sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));//去除最后一个逗号
            else
            {
                sb.Append("{series:[]}");
            }
            sb.Append("]");
            #endregion

            #region indicator
            if (!indicator.Equals(""))
                sb.Append(",indicator:[" + indicator.ToString().Substring(0, indicator.ToString().Length - 1) + "]");
            else
                sb.Append(",indicator:[]");
            #endregion

            #region 图例
            if (!factorName.Equals("")) factorName = factorName.Substring(0, factorName.Length - 1);
            sb.Append(",legend:['" + factorName.Replace(",", "','") + "']");//图例
            sb.Append(",legendShow:" + (factorCode.Length > 1 ? "true" : "false"));//图例显示    
            #endregion

            #region Title
            string title = "";
            if (ChartType.Equals("WindDirection"))
                title = "风向玫瑰图";
            else if (ChartType.Equals("WindSpeed"))
                title = "风速玫瑰图";
            else
                title = "污染物分布玫瑰图";
            sb.Append(",titleText:'" + title + "'");//Title
            #endregion

            sb.Append("}");
            return sb.ToString().Replace("\r\n", "");
        }

        /// <summary>
        /// 创建json数据
        /// </summary>
        /// <param name="list"></param>
        /// <param name="factorCode"></param>
        /// <param name="WindDir"></param>
        /// <param name="ChartType"></param>
        /// <returns></returns>
        //private string CreatePolaryJsonData(List<PolaryData> list, string[] factorCode, string WindDir, string ChartType)
        //{
        //    string windName = "";
        //    if (WindDir.Equals("Sixteen"))
        //        windName = "北风;西北偏北风;西北风;西北偏西风;西风;西南偏西;西南风;西南偏南;南风;东南偏南风;东南风;东南偏东风;东风;东北偏东风;东北风;东北偏北风";
        //    else
        //        windName = "北风;西北风;西风;西南风;南风;东南风;东风;东北风";
        //    StringBuilder sb = new StringBuilder();
        //    string factorName = "";
        //    int num = 0;
        //    string indicator = "";

        //    sb.Append("{");
        //    #region Data
        //    sb.Append("data:[");
        //    foreach (string fac in factorCode)
        //    {

        //        SmartEP.Core.Interfaces.IPollutant pollutant = airPollutantService.GetPollutantInfo(fac);
        //        factorName += pollutant.PollutantName + ",";

        //        sb.Append("{");
        //        sb.AppendFormat(@"name: '{0}'", pollutant.PollutantName);
        //        string str = "";
        //        foreach (string name in windName.Split(';'))
        //        {
        //            //foreach (PolaryData polary in list.Where(x => x.FactorCoce.Equals(fac)))
        //            //{
        //            PolaryData polary = list.Where(x => x.FactorCoce.Equals(fac) && x.FX.Equals(name)).FirstOrDefault();

        //            #region value
        //            if (polary != null)
        //            {
        //                if (polary.ND != null)
        //                {
        //                    str += polary.ND + ",";
        //                }
        //                else
        //                {
        //                    str += "[],";
        //                }
        //            }
        //            else
        //            {
        //                str += "[],";
        //            }
        //            #endregion

        //            #region indicator
        //            if (num == 0)
        //                indicator += "{ text: '" + name + "'},";
        //            //indicator += "{ text: '" + name + "', max: 360 },";
        //            #endregion
        //            //}
        //        }
        //        if (str.Substring(str.ToString().Length - 1, 1).Trim().Equals(","))
        //            str = str.ToString().Substring(0, str.ToString().Length - 1);//去除最后一个逗号
        //        sb.AppendFormat(",value:[{0}]", str);
        //        sb.Append("},");
        //        num++;
        //    }

        //    if (sb.ToString().Substring(sb.ToString().Length - 1, 1).Trim().Equals(","))
        //        sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));//去除最后一个逗号
        //    else
        //    {
        //        sb.Append("{data:[]}");
        //    }
        //    sb.Append("]");
        //    #endregion

        //    #region indicator
        //    if (!indicator.Equals(""))
        //        sb.Append(",indicator:[" + indicator.ToString().Substring(0, indicator.ToString().Length - 1) + "]");
        //    else
        //        sb.Append(",indicator:[]");
        //    #endregion

        //    #region 图例
        //    if (!factorName.Equals("")) factorName = factorName.Substring(0, factorName.Length - 1);
        //    sb.Append(",legend:['" + factorName.Replace(",", "','") + "']");//图例
        //    sb.Append(",legendShow:" + (factorCode.Length > 1 ? "true" : "false"));//图例显示    
        //    #endregion

        //    #region Title
        //    string title = "";
        //    if (ChartType.Equals("WindDirection"))
        //        title = "风向玫瑰图";
        //    else if (ChartType.Equals("WindSpeed"))
        //        title = "风速玫瑰图";
        //    else
        //        title = "污染物分布玫瑰图";
        //    sb.Append(",titleText:'" + title + "'");//Title
        //    #endregion

        //    sb.Append("}");
        //    return sb.ToString().Replace("\r\n", "");
        //}

        ///// <summary>
        ///// 获取数据源
        ///// </summary>
        ///// <param name="portIds"></param>
        ///// <param name="factorCode"></param>
        ///// <param name="radlDataType"></param>
        ///// <param name="dtBegin"></param>
        ///// <param name="dtEnd"></param>
        ///// <param name="windtype"></param>
        ///// <param name="PolaryType"></param>
        ///// <returns></returns>
        //public List<PolaryData> GetDataSource(string[] portIds, string[] factorCode, string radlDataType, DateTime dtBegin, DateTime dtEnd, string windtype, string PolaryType)
        //{
        //    List<PolaryData> windData = new List<PolaryData>();

        //    if (PolaryType.Equals("WindDirection"))
        //        windData = GetDataSourceWindDir(portIds, factorCode, radlDataType, dtBegin, dtEnd, windtype);
        //    else if (PolaryType.Equals("WindSpeed"))
        //        windData = GetDataSourceWindDir(portIds, factorCode, radlDataType, dtBegin, dtEnd, windtype);
        //    else
        //        windData = GetDataSourceWindDir(portIds, factorCode, radlDataType, dtBegin, dtEnd, windtype);
        //    return windData;
        //}

        /// <summary>
        /// 获取风向数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factorCode"></param>
        /// <param name="radlDataType"></param>
        /// <param name="dtBegin"></param>
        /// <param name="dtEnd"></param>
        /// <param name="windtype"></param>
        /// <returns></returns>
        public List<PolaryData> GetDataSource(string[] portIds, string[] factorCode, string radlDataType, DateTime dtBegin, DateTime dtEnd, string windtype, string PolaryType, string pointId)
        {
            string[] arrWind = new string[] { pointId };
            List<SmartEP.Core.Interfaces.IPollutant> factorList = airPollutantService.GetDefaultFactors(factorCode);
            List<SmartEP.Core.Interfaces.IPollutant> factorListWind = airPollutantService.GetDefaultFactors(new string[] { factorCodeWindDir, factorCodeWindSpeed });
            //ObservableCollection<decimal?> degree = new ObservableCollection<decimal?>();
            List<PolaryData> windData = new List<PolaryData>();
            DataView dv = new DataView();
            DataView dvWind = new DataView();
            int pointCount = portIds.Length;
            int dayOrHour = 0;
            if (radlDataType == "Hour")//审核小时
            {
                dv = m_HourData.GetAQAutoMonthReportExportData(portIds, factorList, dtBegin, dtEnd.AddHours(1).AddSeconds(-1));
                dvWind = m_HourData.GetAQAutoMonthReportExportData(arrWind, factorListWind, dtBegin, dtEnd.AddHours(1).AddSeconds(-1));
                dayOrHour = 0;
            }
            else if (radlDataType == "Min60")//原始小时数据
            {
                int total = 0;
                g_IInfectantDALService = MonitoringDataAir.GetInfectantDALService(SmartEP.Core.Enums.EnumMapping.ParseEnum<PollutantDataType>(radlDataType));
                dv = g_IInfectantDALService.GetDataPager(portIds, factorList, dtBegin, dtEnd.AddHours(1).AddSeconds(-1), 100000, 0, out total);
                dvWind = g_IInfectantDALService.GetDataPager(arrWind, factorListWind, dtBegin, dtEnd.AddHours(1).AddSeconds(-1), 100000, 0, out total);
                dayOrHour = 0;
            }
            else if (radlDataType == "Day")//日数据
            {
                dv = m_DayData.GetAQRoutineMonthReportExportData(portIds, factorList, dtBegin, dtEnd);
                dvWind = m_DayData.GetAQRoutineMonthReportExportData(arrWind, factorListWind, dtBegin, dtEnd);
                dayOrHour = 1;
            }
            else if (radlDataType == "OriDay")//原始日数据
            {
                //dv = m_DayData.GetOriAQRoutineMonthReportExportData(portIds, factorList, dtBegin, dtEnd);
                int total = 0;
                InfectantByDayService m_DayOriData = Singleton<InfectantByDayService>.GetInstance();
                dv = m_DayOriData.GetDataPagers(portIds, factorCode, dtBegin, dtEnd, 10000, 0, out total);
                dvWind = m_DayOriData.GetDataPagers(arrWind, new string[] { factorCodeWindDir, factorCodeWindSpeed }, dtBegin, dtEnd, 10000, 0, out total);
                dayOrHour = 1;
            }
            if (dvWind.Count > 0)
            {
                for (int i = 0; i < dv.Table.Rows.Count; i++)
                {
                    if (dv.Table.Columns.Contains("Tstamp"))
                    {
                        dv.Table.Rows[i]["Tstamp"] = Convert.ToDateTime(Convert.ToDateTime(dv.Table.Rows[i]["Tstamp"]).ToString("yyyy-MM-dd HH:00:00"));
                    }
                }
                for (int i = 0; i < dvWind.Table.Rows.Count; i++)
                {
                    if (dvWind.Table.Columns.Contains("Tstamp"))
                    {
                        dvWind.Table.Rows[i]["Tstamp"] = Convert.ToDateTime(Convert.ToDateTime(dvWind.Table.Rows[i]["Tstamp"]).ToString("yyyy-MM-dd HH:00:00"));
                    }
                }
                foreach (string fac in factorCode)
                {
                    SmartEP.Core.Interfaces.IPollutant pollutant = factorList.Where(x => x.PollutantCode.Equals(fac)).FirstOrDefault();
                    ObservableCollection<PolaryData> data = new ObservableCollection<PolaryData>();
                    if (fac != factorCodeWindDir && fac != factorCodeWindSpeed)
                    {
                        data = windService.GetPolaryAllData(dv, pointCount, dvWind, fac, pointId, windtype, dayOrHour);
                    }
                    List<PolaryData> polList = (from d in data select new PolaryData { FactorCoce = fac, FactorName = pollutant.PollutantName, ND = d.ND, FX = d.FX, FactorCount = d.FactorCount, FS = d.FS }).ToList<PolaryData>();
                    windData.AddRange(polList);
                }
                return windData;
            }
            else
            {
                return null;
            }
        }
        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}