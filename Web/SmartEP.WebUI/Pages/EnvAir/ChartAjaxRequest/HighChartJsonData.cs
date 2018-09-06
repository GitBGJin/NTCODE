﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using SmartEP.Service.BaseData.Channel;
//using SmartEP.DomainModel.BaseData;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Core.Interfaces;
//using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.BusinessRule;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Core.Generic;
using SmartEP.DomainModel.BaseData;

namespace SmartEP.WebUI.Pages.EnvAir.ChartAjaxRequest
{
    /// <summary>
    /// 名称：HighChartJsonData.cs
    /// 创建人：徐龙超
    /// 创建日期：2015-09-29
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：实时小时数据【Chart】
    /// 虚拟分页类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    /// 
    public class HighChartJsonData
    {
        string factorY = "a21005";//环境空气一氧化碳新增Y轴

        /// <summary>
        /// 修改by xuyang 2017-08-30
        /// </summary>
        /// <param name="dv"></param>
        /// <param name="FactorCode"></param>
        /// <param name="XName"></param>
        /// <param name="XLabel"></param>
        /// <param name="tooltip"></param>
        /// <param name="chartType"></param>
        /// <param name="pageType"></param>
        /// <param name="excessiveList"></param>
        /// <param name="pointid"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetChartDataMonthNTCom(DataView dv, string[] FactorCode, string[] XName, string[] XLabel, string tooltip, string chartType, string pageType, IQueryable<ExcessiveSettingInfo> excessiveList = null, int pointid = -9999, string name = "")
        {
            string FactorsCode = BindFactors("979f05bb-f730-4285-8aee-fafdce1360e2", "code");
            dv = dv.ToTable(true).DefaultView;//去除重复数据
            StringBuilder sb = new StringBuilder();
            string categories0 = "";
            string categories1 = "";
            List<SmartEP.Core.Interfaces.IPollutant> pollutantList = new List<SmartEP.Core.Interfaces.IPollutant>(); ;
            decimal max = -1;
            string plotLines = "";
            sb.Append("{");
            string pt = string.Empty;
            if (pageType == "Air1")
            {
                pageType = "Air";
                pt = "Air1";
            }
            #region series
            sb.Append("series:[");
            for (int i = 0; i < FactorCode.Length; i++)
            {
                for (int m = 0; m <= 1; m++)
                {
                    string factor = FactorCode[i];
                    SmartEP.Core.Interfaces.IPollutant pollutant = GetPollutantName(pageType, factor);
                    string factorName = pollutant.PollutantName;
                    //用于显示多Y轴
                    if (pollutantList.Count == 0 || !pollutantList.Where(x => !x.PollutantCode.Equals(factorY)).Select(x => x.PollutantMeasureUnit).ToArray().Contains(pollutant.PollutantMeasureUnit) || factor.Equals(factorY))
                        pollutantList.Add(pollutant);
                    //if (!unit.Contains(pollutant.PollutantMeasureUnit)) unit += i == 0 ? pollutant.PollutantMeasureUnit : ";" + pollutant.PollutantMeasureUnit;
                    sb.Append("{");
                    #region 对应Y轴
                    int ynum = -1;
                    if (factor.Equals(factorY))
                        ynum = pollutantList.IndexOf(pollutant);
                    else
                        ynum = pollutantList.IndexOf(pollutantList.Where(x => !x.PollutantCode.Equals(factorY) && x.PollutantMeasureUnit == pollutant.PollutantMeasureUnit).FirstOrDefault());
                    sb.AppendFormat("connectNulls:true,yAxis: {0},", ynum < 0 ? 0 : ynum);//Name属性 
                    #endregion

                    #region Name
                    sb.AppendFormat("name:'{0}',", m == 0 ? factorName + "(基准)" : factorName + "(比对)");//Name属性
                    #endregion

                    #region Data
                    string data = "";
                    if (m == 0)
                    {
                        dv.RowFilter = "Type ='审核(基准)' ";
                    }
                    else
                    {
                        dv.RowFilter = "Type ='审核(比对)' ";
                    }
                    foreach (DataRowView row in dv)
                    {
                        #region categories
                        if (i == 0)
                        {
                            if (m == 0)
                            {
                                string str = "";
                                for (int j = 0; j < XName.Length; j++)
                                {
                                    str += row[XName[j]].ToString() + XLabel[j];
                                }
                                categories0 += "'" + str + "',";
                            }
                            if (m == 1)
                            {
                                string str = "";
                                for (int j = 0; j < XName.Length; j++)
                                {
                                    str += row[XName[j]].ToString() + XLabel[j];
                                }
                                categories1 += "'" + str + "',";
                            }
                        }
                        #endregion

                        #region X、Y值
                        if (row[factor] != DBNull.Value && Convert.ToDecimal(row[factor]) != -10)
                        {
                            if (pollutant.PollutantMeasureUnit == "μg/m³")
                            {
                                data += (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[factor]) * 1000, 0).ToString()) + ",";
                            }
                            else
                            {
                                data += (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[factor]), Convert.ToInt32(pollutant.PollutantDecimalNum)).ToString()) + ",";
                            }
                            //data += (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[factor]), Convert.ToInt32(pollutant.PollutantDecimalNum)).ToString()) + ",";
                        }
                        else
                        {
                            data += "null,";
                        }
                        #endregion
                    }
                    sb.AppendFormat("data:[{0}]", data.Equals("") ? "" : data.Substring(0, data.Length - 1) + "");
                    #endregion

                    #region 是否显示
                    DataTable dt = dv.Table;
                    sb.AppendFormat(",visible:{0}", Convert.ToInt32(dt.Compute("Count(" + factor + ")", "")) > 0 ? "true" : "false");
                    #endregion
                    if (m == 1)
                    {
                        sb.AppendFormat(",xAxis: 1");
                    }
                    else
                    {
                        sb.AppendFormat(",xAxis: 0");
                    }
                    #region ToolTip
                    sb.Append(",tooltip: { valueSuffix: '" + (pollutant.PollutantMeasureUnit == "mg/m3" ? "mg/m<sup>3</sup>" : pollutant.PollutantMeasureUnit) + "' }");//Name属性                   
                    #endregion
                    sb.Append("},");
                    dv.RowFilter = "";
                }
            }
            if (sb.ToString().Substring(sb.ToString().Length - 1, 1).Trim().Equals(","))
                sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));//去除最后一个逗号
            sb.Append("]");
            #endregion

            #region title
            if (pt == "Air1")
            {
                if (pointid != -9999) sb.Append(",titleText:" + "'" + GetPointName(pageType, pointid) + name + "'");
                //if (pointid != -9999) sb.Append(",titleText:" + "'" + name + "'");
                else if (dv.Count > 0 && dv.ToTable().Columns.Contains("portName") && dv[0]["portName"] != DBNull.Value)
                    //sb.Append(",titleText:" + "'" + dv[0]["portName"] + name + "' ");
                    sb.Append(",titleText:" + "'" + name + "' ");
                else
                    sb.Append(",titleText:" + "''");
            }
            else
            {
                //if (pointid != -9999) sb.Append(",titleText:" + "'" + GetPointName(pageType, pointid) + name + "'");
                if (pointid != -9999) sb.Append(",titleText:" + "'" + name + "'");
                else if (dv.Count > 0 && dv.ToTable().Columns.Contains("portName") && dv[0]["portName"] != DBNull.Value)
                    //sb.Append(",titleText:" + "'" + dv[0]["portName"] + name + "' ");
                    sb.Append(",titleText:" + "'" + name + "' ");
                else
                    sb.Append(",titleText:" + "''");
            }


            #endregion

            #region tooltip
            sb.Append(",tooltip:{" + tooltip + "}");
            #endregion

            #region categories
            if (!categories0.Equals(""))
            {
                categories0 = categories0.ToString().Substring(0, categories0.ToString().Length - 1);//去除最后一个逗号
                categories0 = categories0.Replace("年", "/");//修改时间格式
                categories0 = categories0.Replace("月", "");
            }
            sb.Append(",categories:[" + categories0 + "]");
            if (!categories1.Equals(""))
            {
                categories1 = categories1.ToString().Substring(0, categories1.ToString().Length - 1);//去除最后一个逗号
                categories1 = categories1.Replace("年", "/");//修改时间格式
                categories1 = categories1.Replace("月", "");
            }
            sb.Append(",categories:[" + categories1 + "]");
            #endregion

            #region xAxis
            sb.Append(",xAxisData:[{categories:[" + categories0 + "],labels:{rotation:-30},opposite: true},{categories:[" + categories1 + "],labels:{rotation:-30},opposite: false}]");
            //sb.Append(",xAxisData:{type:'datetime',labels:{formatter:formatterMonthData}}");
            #endregion

            #region YAxis
            sb.Append(",yAxis:[");
            if (pollutantList != null)
            {
                int num = 0;
                foreach (SmartEP.Core.Interfaces.IPollutant poll in pollutantList)
                {
                    sb.Append("{");
                    #region title属性
                    sb.Append("title:{");
                    sb.AppendFormat("text: \"{0}\"", poll.PollutantMeasureUnit == "mg/m3" ? "mg/m<sup>3</sup>" : poll.PollutantMeasureUnit);
                    sb.Append("}");
                    #endregion
                    #region 对数轴
                    //sb.Append(",endOnTick: false,type: 'logarithmic'");
                    #endregion
                    #region 超标等级线
                    plotLines = "";
                    if (excessiveList != null)
                    {
                        if (poll.PollutantMeasureUnit == "μg/m³" && !FactorsCode.Contains(poll.PollutantCode))
                        {
                            max = dv.ToTable().Compute("Max(" + poll.PollutantCode + ")", "") != DBNull.Value ? Convert.ToDecimal(dv.ToTable().Compute("Max(" + poll.PollutantCode + ")", "")) * 1000 : -1;
                        }
                        else
                        {
                            max = dv.ToTable().Compute("Max(" + poll.PollutantCode + ")", "") != DBNull.Value ? Convert.ToDecimal(dv.ToTable().Compute("Max(" + poll.PollutantCode + ")", "")) : -1;
                        }

                        //max = dv.ToTable().Compute("Max(" + poll.PollutantCode + ")", "") != DBNull.Value ? Convert.ToDecimal(dv.ToTable().Compute("Max(" + poll.PollutantCode + ")", "")) : -1;
                        foreach (ExcessiveSettingInfo excess in excessiveList)
                        {
                            if (excess.PollutantGuid.Equals(poll.PollutantGuid))
                            {
                                max = max == -1 || max < excess.excessiveUpper ? excess.excessiveUpper : max;
                                #region 下限
                                //plotLines += "{";
                                //plotLines += string.Format("value:{0}", excess.excessiveLow);
                                //plotLines += string.Format(",color:\"{0}\"", "red");
                                //plotLines += string.Format(",width:{0}", 2);
                                //plotLines += string.Format(",dashStyle:'{0}'", "shortdot");
                                //#region Label属性
                                //plotLines += ",label: {";
                                //plotLines += string.Format("text:\"{0}\"", "超下限");
                                //plotLines += string.Format(",align:\"{0}\"", "left");
                                //plotLines += "}";
                                //#endregion
                                //plotLines += "},";
                                #endregion

                                #region 上限
                                plotLines += "{";
                                plotLines += string.Format("value:{0}", excess.excessiveUpper);
                                plotLines += string.Format(",color:\"{0}\"", "red");
                                plotLines += string.Format(",width:{0}", 2);
                                plotLines += string.Format(",dashStyle:'{0}'", "shortdot");
                                #region Label属性
                                plotLines += ",label: {";
                                plotLines += string.Format("text:\"{0}\"", "超标限(" + excess.excessiveUpper + ")");
                                plotLines += string.Format(",align:\"{0}\"", "left");
                                plotLines += "}";
                                #endregion
                                plotLines += "},";
                                #endregion
                            }
                        }
                        #region 等级线
                        sb.Append(",plotLines: [" + (!plotLines.Equals("") ? plotLines.Substring(0, plotLines.Length - 1) : "") + "]");
                        #endregion
                        #region 最大值、最小值
                        sb.Append(",min:0");
                        if (max != -1)
                            sb.Append(",max:" + max);
                        #endregion
                    }
                    #endregion

                    #region Y轴左右位置
                    if (num % 2 == 0)
                        sb.Append(",opposite: false");
                    else
                        sb.Append(",opposite: true");
                    num++;
                    #endregion

                    //#region Y轴线条
                    //sb.Append(",lineWidth: 1");
                    //#endregion
                    sb.Append("},");
                }
                if (sb.ToString().Substring(sb.ToString().Length - 1, 1).Trim().Equals(","))
                    sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));//去除最后一个逗号
            }
            else
            {
                sb.Append("{title: {text: '浓度'}}");
            }
            sb.Append("]");
            #endregion

            #region 图表type
            if (!chartType.Equals("")) sb.AppendFormat(",chartType:'{0}'", chartType);
            #endregion

            sb.Append("}");

            return sb.ToString().Replace("\r\n", "");
        }

        /// <summary>
        /// 构造Chart JSon数据比对数据【小时、日数据】
        /// </summary>
        /// <param name="dv">数据源</param>
        /// <param name="FactorCode">因子编码</param>
        /// <param name="XName">X轴字段名</param>
        /// <param name="tooltip">Chart节点对应的js悬停样式</param>
        /// <param name="chartType">数据类型</param>
        /// <param name="pageType">应用程序类型</param>
        /// <returns></returns>
        public string GetChartDataCom(DataView dv, string[] FactorCode, string XName, string tooltip, string chartType, string pageType, string XLabelfomatter = "", IQueryable<ExcessiveSettingInfo> excessiveList = null, int pointid = -9999, string name = "")
        {
            string FactorsCode = BindFactors("979f05bb-f730-4285-8aee-fafdce1360e2", "code");
            dv = dv.ToTable(true).DefaultView;//去除重复数据
            dv.Sort = XName + " ASC";
            StringBuilder sb = new StringBuilder();
            List<SmartEP.Core.Interfaces.IPollutant> pollutantList = new List<SmartEP.Core.Interfaces.IPollutant>(); ;
            decimal max = -1;
            string plotLines = "";
            sb.Append("{");
            string pt = string.Empty;
            if (pageType == "Air1")
            {
                pageType = "Air";
                pt = "Air1";
            }
            #region series
            sb.Append("series:[");
            for (int i = 0; i < FactorCode.Length; i++)
            {
                try
                {
                    for (int m = 0; m <= 1; m++)
                    {
                        string factor = FactorCode[i];
                        SmartEP.Core.Interfaces.IPollutant pollutant = GetPollutantName(pageType, factor);
                        string factorName = pollutant.PollutantName;
                        //用于显示多Y轴
                        if (pollutantList.Count == 0 || !pollutantList.Where(x => !x.PollutantCode.Equals(factorY)).Select(x => x.PollutantMeasureUnit).ToArray().Contains(pollutant.PollutantMeasureUnit) || factor.Equals(factorY))
                            pollutantList.Add(pollutant);
                        //if (!unit.Contains(pollutant.PollutantMeasureUnit)) unit += i == 0 ? pollutant.PollutantMeasureUnit : ";" + pollutant.PollutantMeasureUnit;
                        sb.Append("{");
                        #region 对应Y轴
                        int ynum = -1;
                        if (factor.Equals(factorY))
                            ynum = pollutantList.IndexOf(pollutant);
                        else
                            ynum = pollutantList.IndexOf(pollutantList.Where(x => !x.PollutantCode.Equals(factorY) && x.PollutantMeasureUnit == pollutant.PollutantMeasureUnit).FirstOrDefault());
                        sb.AppendFormat("connectNulls:true,yAxis: {0},", ynum < 0 ? 0 : ynum);//Name属性 
                        #endregion

                        #region Name
                        sb.AppendFormat("name:'{0}',", m == 0 ? factorName + "(基准)" : factorName + "(比对)");//Name属性
                        #endregion

                        #region Data
                        string data = "";

                        if (m == 0)
                        {
                            dv.RowFilter = "Type ='审核(基准)' ";
                        }
                        else
                        {
                            dv.RowFilter = "Type ='审核(比对)' ";
                        }
                        foreach (DataRowView row in dv)
                        {
                            DateTime tstamp = Convert.ToDateTime(row[XName]);
                            string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);

                            #region X、Y值
                            if (row[factor] != DBNull.Value && Convert.ToDecimal(row[factor]) != -10)
                            {
                                if (pollutant.PollutantMeasureUnit == "μg/m³" && !FactorsCode.Contains(pollutant.PollutantCode))
                                {
                                    data += "[" + time + "," + (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[factor]) * 1000, Convert.ToInt32(pollutant.PollutantDecimalNum) - 3).ToString()) + "],";
                                }
                                else
                                {
                                    data += "[" + time + "," + (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[factor]), Convert.ToInt32(pollutant.PollutantDecimalNum)).ToString()) + "],";
                                }

                            }
                            else
                            {
                                data += "[" + time + ",null],";
                            }
                            #endregion

                        }
                        sb.AppendFormat("data:[{0}]", data.Equals("") ? "" : data.Substring(0, data.Length - 1) + "");
                        #endregion

                        #region 是否显示
                        DataTable dt = dv.Table;
                        sb.AppendFormat(",visible:{0}", Convert.ToInt32(dt.Compute("Count(" + factor + ")", "")) > 0 ? "true" : "false");
                        #endregion

                        if (m == 1)
                        {
                            sb.AppendFormat(",xAxis: 1");
                        }
                        else
                        {
                            sb.AppendFormat(",xAxis: 0");
                        }

                        #region ToolTip
                        sb.Append(",tooltip: { valueSuffix: '" + (pollutant.PollutantMeasureUnit == "mg/m3" ? "mg/m<sup>3</sup>" : pollutant.PollutantMeasureUnit) + "' }");//Name属性                   
                        #endregion
                        sb.Append("},");
                        dv.RowFilter = "";
                    }
                }
                catch
                {
                }
            }
            if (sb.ToString().Substring(sb.ToString().Length - 1, 1).Trim().Equals(","))
                sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));//去除最后一个逗号
            sb.Append("]");
            #endregion

            #region title
            if (pt == "Air1")
            {
                if (pointid != -9999) sb.Append(",titleText:" + "'" + GetPointName(pageType, pointid) + "'");
                //if (pointid != -9999) sb.Append(",titleText:" + "'" + name + "'");
                else if (dv.Count > 0 && dv.ToTable().Columns.Contains("portName") && dv[0]["portName"] != DBNull.Value)
                    //sb.Append(",titleText:" + "'" + dv[0]["portName"] + name +"' ");
                    sb.Append(",titleText:" + "'" + name + "' ");
                else
                    sb.Append(",titleText:" + "''");
            }
            else
            {
                //if (pointid != -9999) sb.Append(",titleText:" + "'" + GetPointName(pageType, pointid) + name + "'");
                if (pointid != -9999) sb.Append(",titleText:" + "'" + name + "'");
                else if (dv.Count > 0 && dv.ToTable().Columns.Contains("portName") && dv[0]["portName"] != DBNull.Value)
                    //sb.Append(",titleText:" + "'" + dv[0]["portName"] + name +"' ");
                    sb.Append(",titleText:" + "'" + name + "' ");
                else
                    sb.Append(",titleText:" + "''");
            }

            #endregion

            #region tooltip
            //sb.Append(",tooltip:{" + tooltip + "}");//修改by lvyun 2017-08-21
            if (!string.IsNullOrWhiteSpace(tooltip))
            {
                sb.Append(",tooltip:{formatter: function () {return Highcharts.dateFormat('" + tooltip + "', this.x) + '<br/>' + this.series.name+':' + this.y + ' '+this.series.tooltipOptions.valueSuffix;}}");

            }
            else
            {
                sb.Append(",tooltip:{formatter: function () {return Highcharts.dateFormat('%m/%d日 %H时',this.x)+'<br/>'+ this.series.name+':'+this.y+' '+this.series.tooltipOptions.valueSuffix;}}");

            }
            #endregion

            #region xAxis
            if (!XLabelfomatter.Equals(""))
            {
                //sb.Append(",xAxisData:{type:'datetime',labels:{" + XLabelfomatter + "}}");
                sb.Append(",xAxisData:[{type:'datetime',labels:{formatter: function () {return Highcharts.dateFormat('" + XLabelfomatter + "',this.value);}},opposite: true},{type:'datetime',labels:{formatter: function () {return Highcharts.dateFormat('" + XLabelfomatter + "',this.value);}},opposite: false}]");//修改by lvyun 2017-08-21
            }
            else
            {
                sb.Append(",xAxisData:[{type:'datetime',labels:{formatter: function () {return Highcharts.dateFormat('%m/%d %H时',this.value);}},opposite: true},{type:'datetime',labels:{formatter: function () {return Highcharts.dateFormat('%m/%d %H时',this.value);}},opposite: false}]");//修改by lvyun 2017-08-21
            }

            #endregion

            #region YAxis
            sb.Append(",yAxis:[");
            if (pollutantList != null)
            {
                int num = 0;
                foreach (SmartEP.Core.Interfaces.IPollutant poll in pollutantList)
                {
                    sb.Append("{");
                    #region title属性
                    sb.Append("title:{");
                    sb.AppendFormat("text: \"{0}\"", poll.PollutantMeasureUnit == "mg/m3" ? "mg/m<sup>3</sup>" : poll.PollutantMeasureUnit);
                    #region title下的style属性
                    //sb.Append("style:{");
                    //if (yAxis.TitleColor != null && !yAxis.TitleColor.Trim().Equals(""))
                    //{
                    //    sb.AppendFormat("color:\"{0}\"", yAxis.TitleColor);
                    //}
                    //sb.Append("}");
                    #endregion
                    sb.Append("}");
                    #endregion
                    sb.Append(",labels: {format: '{value:.,0f}'}");
                    #region 对数轴
                    //sb.Append(",endOnTick: false,type: 'logarithmic'");
                    #endregion
                    #region 超标等级线
                    plotLines = "";
                    if (excessiveList != null)
                    {
                        if (poll.PollutantMeasureUnit == "μg/m³" && !FactorsCode.Contains(poll.PollutantCode))
                        {
                            max = dv.ToTable().Compute("Max(" + poll.PollutantCode + ")", "") != DBNull.Value ? Convert.ToDecimal(dv.ToTable().Compute("Max(" + poll.PollutantCode + ")", "")) * 1000 : -1;
                        }
                        else
                        {
                            max = dv.ToTable().Compute("Max(" + poll.PollutantCode + ")", "") != DBNull.Value ? Convert.ToDecimal(dv.ToTable().Compute("Max(" + poll.PollutantCode + ")", "")) : -1;
                        }
                        //max = dv.ToTable().Compute("Max(" + poll.PollutantCode + ")", "") != DBNull.Value ? Convert.ToDecimal(dv.ToTable().Compute("Max(" + poll.PollutantCode + ")", "")) : -1;
                        foreach (ExcessiveSettingInfo excess in excessiveList)
                        {
                            if (excess.PollutantGuid.Equals(poll.PollutantGuid))
                            {
                                max = max == -1 || max < excess.excessiveUpper ? excess.excessiveUpper : max;
                                #region 下限
                                //plotLines += "{";
                                //plotLines += string.Format("value:{0}", excess.excessiveLow);
                                //plotLines += string.Format(",color:\"{0}\"", "red");
                                //plotLines += string.Format(",width:{0}", 2);
                                //plotLines += string.Format(",dashStyle:'{0}'", "shortdot");
                                //#region Label属性
                                //plotLines += ",label: {";
                                //plotLines += string.Format("text:\"{0}\"", "超下限");
                                //plotLines += string.Format(",align:\"{0}\"", "left");
                                //plotLines += "}";
                                //#endregion
                                //plotLines += "},";
                                #endregion

                                #region 上限
                                plotLines += "{";
                                plotLines += string.Format("value:{0}", excess.excessiveUpper);
                                plotLines += string.Format(",color:\"{0}\"", "red");
                                plotLines += string.Format(",width:{0}", 2);
                                plotLines += string.Format(",dashStyle:'{0}'", "shortdot");
                                #region Label属性
                                plotLines += ",label: {";
                                plotLines += string.Format("text:\"{0}\"", "超标限(" + excess.excessiveUpper + ")");
                                plotLines += string.Format(",align:\"{0}\"", "left");
                                plotLines += "}";
                                #endregion
                                plotLines += "},";
                                #endregion
                            }
                        }
                        #region 等级线
                        sb.Append(",plotLines: [" + (!plotLines.Equals("") ? plotLines.Substring(0, plotLines.Length - 1) : "") + "]");
                        #endregion
                        #region 最大值、最小值
                        sb.Append(",min:0");
                        if (max != -1)
                            sb.Append(",max:" + max);
                        #endregion
                    }
                    #endregion

                    #region Y轴左右位置
                    if (num % 2 == 0)
                        sb.Append(",opposite: false");
                    else
                        sb.Append(",opposite: true");
                    num++;
                    #endregion

                    sb.Append("},");
                    #region 注释
                    //#region Y轴线条
                    //sb.Append(",lineWidth: 1");
                    //#endregion

                    //#region 最小值属性
                    //if (yAxis.Min != null && !yAxis.Min.Trim().Equals(""))
                    //    sb.Append("min:" + yAxis.Min + ",");
                    //#endregion

                    //#region 最大值属性
                    //if (yAxis.Max != null && !yAxis.Max.Trim().Equals(""))
                    //    sb.Append("max:" + yAxis.Max + ",");
                    //#endregion

                    //#region 等级域属性
                    //sb.Append("plotBands: [");
                    //if (plotList != null && plotList.Count > 0)
                    //{
                    //    foreach (PlotBands plotbands in plotList)
                    //    {
                    //        sb.Append("{");
                    //        if (plotbands.BandsColor != null && !plotbands.BandsColor.Trim().Equals(""))
                    //            sb.AppendFormat("color:\"{0}\",", plotbands.BandsColor);
                    //        if (plotbands.From != null && !plotbands.From.Trim().Equals(""))
                    //            sb.AppendFormat("from:\"{0}\",", plotbands.From);
                    //        if (plotbands.To != null && !plotbands.To.Trim().Equals(""))
                    //            sb.AppendFormat("to:\"{0}\",", plotbands.To);
                    //        #region Label属性
                    //        sb.Append("label: {");
                    //        if (plotbands.LabelText != null && !plotbands.LabelText.Trim().Equals(""))
                    //            sb.AppendFormat("text:\"{0}\",", plotbands.LabelText);
                    //        if (plotbands.LabelALign != null && !plotbands.LabelALign.Trim().Equals(""))
                    //            sb.AppendFormat("align:\"{0}\"", plotbands.LabelALign);
                    //        sb.Append("}");
                    //        #endregion
                    //        sb.Append("},");
                    //    }
                    //    sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));//去除最后一个逗号
                    //}
                    //sb.Append("],");
                    //#endregion
                    #endregion
                }
                if (sb.ToString().Substring(sb.ToString().Length - 1, 1).Trim().Equals(","))
                    sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));//去除最后一个逗号
            }
            else
            {
                sb.Append("{title: {text: '浓度'}}");
            }
            sb.Append("]");
            #endregion

            #region 图表type
            if (!chartType.Equals("")) sb.AppendFormat(",chartType:'{0}'", chartType);
            #endregion

            sb.Append("}");

            return sb.ToString().Replace("\r\n", "");
        }

        /// <summary>
        /// 构造Chart JSon数据【单参数数据】
        /// </summary>
        /// <param name="dv">数据源</param>
        /// <param name="FactorCode">因子编码</param>
        /// <param name="PointIds">站点ID</param>
        /// <param name="XName">X轴字段名</param>
        /// <param name="tooltip">Chart节点对应的js悬停样式</param>
        /// <param name="chartType">数据类型</param>
        /// <param name="pageType">应用程序类型</param>
        /// <returns></returns>
        public string GetChartDataSample(DataView dv, string[] FactorCode, string[] PointIds, string XName, string tooltip, string chartType, string pageType, string XLabelfomatter = "", IQueryable<ExcessiveSettingInfo> excessiveList = null, int pointid = -9999)
        {
            dv = dv.ToTable(true).DefaultView;//去除重复数据
            dv.Sort = XName + " ASC";
            SmartEP.Core.Interfaces.IPollutant Ifactor = GetPollutantName(pageType, FactorCode[0]);
            string DecimalNum = Ifactor.PollutantDecimalNum;
            StringBuilder sb = new StringBuilder();
            sb.Append("{chartType:'spline',");
            sb.Append(" xAxis: {type: 'datetime', dateTimeLabelFormats: { day: '%d日%H点'  } },");
            sb.Append(" yAxis: {title: { text: '" + (Ifactor.PollutantMeasureUnit == "mg/m3" ? "mg/m<sup>3</sup>" : Ifactor.PollutantMeasureUnit) + "' },  min: 0 },");
            //sb.Append(" yAxis: {title: { text: '" + (Ifactor.PollutantMeasureUnit == "mg/L" ? "mg/m<sup>3</sup>" : Ifactor.PollutantMeasureUnit) + "' },  min: 0 },");
            sb.Append(" tooltip: {formatter: function() {  return '<b>'+ this.series.name +'</b><br/>'+ Highcharts.dateFormat('%d日%H点%M分', this.x) +':'+ this.y +' " + (Ifactor.PollutantMeasureUnit == "mg/m3" ? "mg/m<sup>3</sup>" : Ifactor.PollutantMeasureUnit) + "';    } },series: [");
            string data = "";
            for (int i = 0; i < PointIds.Length; i++)
            {
                int pointTemp = int.TryParse(PointIds[i].ToString(), out pointTemp) ? pointTemp : 0;
                string pointName = GetPointName("Air", pointTemp);
                dv.RowFilter = "PointId='" + PointIds[i] + "'";
                data += "{";
                data += string.Format(" name: '{0}',data:[ ", pointName);
                int m = 0;
                foreach (DataRowView row in dv)
                {
                    m++;
                    DateTime tstamp = Convert.ToDateTime(row["Tstamp"]);
                    string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                    if (m != dv.Count)
                    {
                        if (row[FactorCode[0]] != DBNull.Value && FactorCode[0].ToString() != "a21005")
                        {

                            data += "[" + time + "," + (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[FactorCode[0]]) * 1000, 0).ToString()) + "],";
                        }
                        else if (row[FactorCode[0]] != DBNull.Value && FactorCode[0].ToString() == "a21005")
                        {

                            data += "[" + time + "," + (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[FactorCode[0]]), Convert.ToInt32(DecimalNum)).ToString()) + "],";
                        }
                        else
                        {
                            data += "[" + time + ",null],";
                        }
                    }
                    else
                    {
                        if (row[FactorCode[0]] != DBNull.Value && FactorCode[0].ToString() != "a21005")
                        {

                            data += "[" + time + "," + (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[FactorCode[0]]) * 1000, 0).ToString()) + "]";
                        }
                        else if (row[FactorCode[0]] != DBNull.Value && FactorCode[0].ToString() == "a21005")
                        {

                            data += "[" + time + "," + (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[FactorCode[0]]), Convert.ToInt32(DecimalNum)).ToString()) + "]";
                        }
                        else
                        {
                            data += "[" + time + ",null]";
                        }
                    }
                }
                data += "]},";
            }
            sb.AppendFormat("{0}", data.Equals("") ? "" : data.Substring(0, data.Length - 1) + "");
            sb.Append("]  } ");
            return sb.ToString().Replace("\r\n", "");
        }
        /// <summary>
        /// 构造Chart JSon数据【小时、日数据】
        /// </summary>
        /// <param name="dv1">数据源</param>
        /// <param name="FactorCode">因子编码</param>
        /// <param name="XName">X轴字段名</param>
        /// <param name="tooltip">Chart节点对应的js悬停样式</param>
        /// <param name="chartType">数据类型</param>
        /// <param name="pageType">应用程序类型</param>
        /// <returns></returns>
        public string GetXYChartData(DataView dv1, DataView dv2, string[] FactorCode, string XName, string tooltip, string chartType, string pageType, string XLabelfomatter = "", IQueryable<ExcessiveSettingInfo> excessiveList = null, int pointid = -9999, string name = "")
        {
            string FactorsCode = BindFactors("979f05bb-f730-4285-8aee-fafdce1360e2", "code");
            dv1 = dv1.ToTable(true).DefaultView;//去除重复数据
            dv1.Sort = XName + " ASC";
            dv2 = dv2.ToTable(true).DefaultView;//去除重复数据
            dv2.Sort = XName + " ASC";
            StringBuilder sb = new StringBuilder();
            List<SmartEP.Core.Interfaces.IPollutant> pollutantList = new List<SmartEP.Core.Interfaces.IPollutant>(); ;
            decimal max = -1;
            string plotLines = "";
            sb.Append("{");
            string pt = string.Empty;
            if (pageType == "Air1")
            {
                pageType = "Air";
                pt = "Air1";
            }
            #region series
            sb.Append("series:[");
            for (int i = 0; i < FactorCode.Length; i++)
            {
                try
                {
                    string factor = FactorCode[i];
                    SmartEP.Core.Interfaces.IPollutant pollutant = GetPollutantName(pageType, factor);
                    string factorName = pollutant.PollutantName;
                    //用于显示多Y轴
                    if (pollutantList.Count == 0 || !pollutantList.Where(x => !x.PollutantCode.Equals(factorY)).Select(x => x.PollutantMeasureUnit).ToArray().Contains(pollutant.PollutantMeasureUnit) || factor.Equals(factorY))
                        pollutantList.Add(pollutant);
                    //if (!unit.Contains(pollutant.PollutantMeasureUnit)) unit += i == 0 ? pollutant.PollutantMeasureUnit : ";" + pollutant.PollutantMeasureUnit;
                    sb.Append("{");

                    #region 对应Y轴
                    int ynum = -1;
                    if (factor.Equals(factorY))
                        ynum = pollutantList.IndexOf(pollutant);
                    else
                        ynum = pollutantList.IndexOf(pollutantList.Where(x => !x.PollutantCode.Equals(factorY) && x.PollutantMeasureUnit == pollutant.PollutantMeasureUnit).FirstOrDefault());
                    sb.AppendFormat("yAxis: {0},", ynum < 0 ? 0 : ynum);//Name属性 
                    #endregion

                    #region Name
                    sb.AppendFormat("name:'{0}',", factorName);//Name属性
                    #endregion

                    #region Data
                    string data = "";

                    foreach (DataRowView row in dv1)
                    {
                        DateTime tstamp = Convert.ToDateTime(row[XName]);
                        string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);

                        #region X、Y值
                        if (row[factor] != DBNull.Value)
                        {
                            if (pollutant.PollutantMeasureUnit == "μg/m³" && !FactorsCode.Contains(pollutant.PollutantCode))
                            {
                                data += "[" + time + "," + (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[factor]) * 1000, Convert.ToInt32(pollutant.PollutantDecimalNum) - 3).ToString()) + "],";
                            }
                            else
                            {
                                data += "[" + time + "," + (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[factor]), Convert.ToInt32(pollutant.PollutantDecimalNum)).ToString()) + "],";
                            }

                        }
                        else
                        {
                            data += "[" + time + ",null],";
                        }
                        #endregion

                    }
                    sb.AppendFormat("data:[{0}]", data.Equals("") ? "" : data.Substring(0, data.Length - 1) + "");
                    #endregion

                    #region 是否显示
                    DataTable dt = dv1.ToTable();
                    sb.AppendFormat(",visible:{0}", Convert.ToInt32(dt.Compute("Count(" + factor + ")", "")) > 0 ? "true" : "false");
                    #endregion

                    #region ToolTip
                    sb.Append(",tooltip: { valueSuffix: '" + (pollutant.PollutantMeasureUnit == "mg/m3" ? "mg/m<sup>3</sup>" : pollutant.PollutantMeasureUnit) + "' }");//Name属性                   
                    #endregion
                    sb.Append("},");
                }
                catch
                {
                }
            }
            if (sb.ToString().Substring(sb.ToString().Length - 1, 1).Trim().Equals(","))
                sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));//去除最后一个逗号
            sb.Append("]");
            #endregion

            #region title
            if (pt == "Air1")
            {
                if (pointid != -9999) sb.Append(",titleText:" + "'" + GetPointName(pageType, pointid) + "'");
                //if (pointid != -9999) sb.Append(",titleText:" + "'" + name + "'");
                else if (dv1.Count > 0 && dv1.ToTable().Columns.Contains("portName") && dv1[0]["portName"] != DBNull.Value)
                    //sb.Append(",titleText:" + "'" + dv1[0]["portName"] + name +"' ");
                    sb.Append(",titleText:" + "'" + name + "' ");
                else
                    sb.Append(",titleText:" + "'多测点均值'");
            }
            else
            {
                //if (pointid != -9999) sb.Append(",titleText:" + "'" + GetPointName(pageType, pointid) + name + "'");
                if (pointid != -9999) sb.Append(",titleText:" + "'" + name + "'");
                else if (dv1.Count > 0 && dv1.ToTable().Columns.Contains("portName") && dv1[0]["portName"] != DBNull.Value)
                    //sb.Append(",titleText:" + "'" + dv1[0]["portName"] + name +"' ");
                    sb.Append(",titleText:" + "'" + name + "' ");
                else
                    sb.Append(",titleText:" + "'多测点均值'");
            }

            #endregion

            #region tooltip
            sb.Append(",tooltip:{" + tooltip + "}");
            #endregion

            #region xAxis
            if (!XLabelfomatter.Equals(""))
                sb.Append(",xAxisData:{type:'datetime',labels:{" + XLabelfomatter + "}}");
            #endregion

            #region YAxis
            sb.Append(",yAxis:[");
            if (pollutantList != null)
            {
                int num = 0;
                foreach (SmartEP.Core.Interfaces.IPollutant poll in pollutantList)
                {
                    sb.Append("{");
                    #region title属性
                    sb.Append("title:{");
                    sb.AppendFormat("text: \"{0}\"", poll.PollutantMeasureUnit == "mg/m3" ? "mg/m<sup>3</sup>" : poll.PollutantMeasureUnit);
                    #region title下的style属性
                    //sb.Append("style:{");
                    //if (yAxis.TitleColor != null && !yAxis.TitleColor.Trim().Equals(""))
                    //{
                    //    sb.AppendFormat("color:\"{0}\"", yAxis.TitleColor);
                    //}
                    //sb.Append("}");
                    #endregion
                    sb.Append("}");
                    #endregion
                    sb.Append(",labels: {format: '{value:.,0f}'}");
                    #region 对数轴
                    //sb.Append(",endOnTick: false,type: 'logarithmic'");
                    #endregion
                    #region 超标等级线
                    plotLines = "";
                    if (excessiveList != null)
                    {
                        if (poll.PollutantMeasureUnit == "μg/m³" && !FactorsCode.Contains(poll.PollutantCode))
                        {
                            max = dv1.ToTable().Compute("Max(" + poll.PollutantCode + ")", "") != DBNull.Value ? Convert.ToDecimal(dv1.ToTable().Compute("Max(" + poll.PollutantCode + ")", "")) * 1000 : -1;
                        }
                        else
                        {
                            max = dv1.ToTable().Compute("Max(" + poll.PollutantCode + ")", "") != DBNull.Value ? Convert.ToDecimal(dv1.ToTable().Compute("Max(" + poll.PollutantCode + ")", "")) : -1;
                        }
                        //max = dv1.ToTable().Compute("Max(" + poll.PollutantCode + ")", "") != DBNull.Value ? Convert.ToDecimal(dv1.ToTable().Compute("Max(" + poll.PollutantCode + ")", "")) : -1;
                        foreach (ExcessiveSettingInfo excess in excessiveList)
                        {
                            if (excess.PollutantGuid.Equals(poll.PollutantGuid))
                            {
                                max = max == -1 || max < excess.excessiveUpper ? excess.excessiveUpper : max;
                                #region 下限
                                //plotLines += "{";
                                //plotLines += string.Format("value:{0}", excess.excessiveLow);
                                //plotLines += string.Format(",color:\"{0}\"", "red");
                                //plotLines += string.Format(",width:{0}", 2);
                                //plotLines += string.Format(",dashStyle:'{0}'", "shortdot");
                                //#region Label属性
                                //plotLines += ",label: {";
                                //plotLines += string.Format("text:\"{0}\"", "超下限");
                                //plotLines += string.Format(",align:\"{0}\"", "left");
                                //plotLines += "}";
                                //#endregion
                                //plotLines += "},";
                                #endregion

                                #region 上限
                                plotLines += "{";
                                plotLines += string.Format("value:{0}", excess.excessiveUpper);
                                plotLines += string.Format(",color:\"{0}\"", "red");
                                plotLines += string.Format(",width:{0}", 2);
                                plotLines += string.Format(",dashStyle:'{0}'", "shortdot");
                                #region Label属性
                                plotLines += ",label: {";
                                plotLines += string.Format("text:\"{0}\"", "超标限(" + excess.excessiveUpper + ")");
                                plotLines += string.Format(",align:\"{0}\"", "left");
                                plotLines += "}";
                                #endregion
                                plotLines += "},";
                                #endregion
                            }
                        }
                        #region 等级线
                        sb.Append(",plotLines: [" + (!plotLines.Equals("") ? plotLines.Substring(0, plotLines.Length - 1) : "") + "]");
                        #endregion
                        #region 最大值、最小值
                        sb.Append(",min:0");
                        if (max != -1)
                            sb.Append(",max:" + max);
                        #endregion
                    }
                    #endregion

                    #region Y轴左右位置
                    if (num % 2 == 0)
                        sb.Append(",opposite: false");
                    else
                        sb.Append(",opposite: true");
                    num++;
                    #endregion

                    sb.Append("},");
                    #region 注释
                    //#region Y轴线条
                    //sb.Append(",lineWidth: 1");
                    //#endregion

                    //#region 最小值属性
                    //if (yAxis.Min != null && !yAxis.Min.Trim().Equals(""))
                    //    sb.Append("min:" + yAxis.Min + ",");
                    //#endregion

                    //#region 最大值属性
                    //if (yAxis.Max != null && !yAxis.Max.Trim().Equals(""))
                    //    sb.Append("max:" + yAxis.Max + ",");
                    //#endregion

                    //#region 等级域属性
                    //sb.Append("plotBands: [");
                    //if (plotList != null && plotList.Count > 0)
                    //{
                    //    foreach (PlotBands plotbands in plotList)
                    //    {
                    //        sb.Append("{");
                    //        if (plotbands.BandsColor != null && !plotbands.BandsColor.Trim().Equals(""))
                    //            sb.AppendFormat("color:\"{0}\",", plotbands.BandsColor);
                    //        if (plotbands.From != null && !plotbands.From.Trim().Equals(""))
                    //            sb.AppendFormat("from:\"{0}\",", plotbands.From);
                    //        if (plotbands.To != null && !plotbands.To.Trim().Equals(""))
                    //            sb.AppendFormat("to:\"{0}\",", plotbands.To);
                    //        #region Label属性
                    //        sb.Append("label: {");
                    //        if (plotbands.LabelText != null && !plotbands.LabelText.Trim().Equals(""))
                    //            sb.AppendFormat("text:\"{0}\",", plotbands.LabelText);
                    //        if (plotbands.LabelALign != null && !plotbands.LabelALign.Trim().Equals(""))
                    //            sb.AppendFormat("align:\"{0}\"", plotbands.LabelALign);
                    //        sb.Append("}");
                    //        #endregion
                    //        sb.Append("},");
                    //    }
                    //    sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));//去除最后一个逗号
                    //}
                    //sb.Append("],");
                    //#endregion
                    #endregion
                }
                if (sb.ToString().Substring(sb.ToString().Length - 1, 1).Trim().Equals(","))
                    sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));//去除最后一个逗号
            }
            else
            {
                sb.Append("{title: {text: '浓度'}}");
            }
            sb.Append("]");
            #endregion

            #region 图表type
            if (!chartType.Equals("")) sb.AppendFormat(",chartType:'{0}'", chartType);
            #endregion

            sb.Append("}");

            return sb.ToString().Replace("\r\n", "");
        }
        /// <summary>
        /// 构造Chart JSon数据【小时、日数据】
        /// </summary>
        /// <param name="dv">数据源</param>
        /// <param name="FactorCode">因子编码</param>
        /// <param name="XName">X轴字段名</param>
        /// <param name="tooltip">Chart节点对应的js悬停样式</param>
        /// <param name="chartType">数据类型</param>
        /// <param name="pageType">应用程序类型</param>
        /// <returns></returns>
        public string GetChartData(DataView dv, string[] FactorCode, string XName, string tooltip, string chartType, string pageType, string XLabelfomatter = "", IQueryable<ExcessiveSettingInfo> excessiveList = null, int pointid = -9999, string name = "")
        {
            string FactorsCode = BindFactors("979f05bb-f730-4285-8aee-fafdce1360e2", "code");
            dv = dv.ToTable(true).DefaultView;//去除重复数据
            dv.Sort = XName + " ASC";
            StringBuilder sb = new StringBuilder();
            List<SmartEP.Core.Interfaces.IPollutant> pollutantList = new List<SmartEP.Core.Interfaces.IPollutant>(); ;
            decimal max = -1;
            string plotLines = "";
            sb.Append("{");
            string pt = string.Empty;
            if (pageType == "Air1")
            {
                pageType = "Air";
                pt = "Air1";
            }
            #region series
            sb.Append("series:[");
            for (int i = 0; i < FactorCode.Length; i++)
            {
                try
                {
                    string factor = FactorCode[i];
                    SmartEP.Core.Interfaces.IPollutant pollutant = GetPollutantName(pageType, factor);
                    string factorName = pollutant.PollutantName;
                    //用于显示多Y轴
                    if (pollutantList.Count == 0 || !pollutantList.Where(x => !x.PollutantCode.Equals(factorY)).Select(x => x.PollutantMeasureUnit).ToArray().Contains(pollutant.PollutantMeasureUnit) || factor.Equals(factorY))
                        pollutantList.Add(pollutant);
                    //if (!unit.Contains(pollutant.PollutantMeasureUnit)) unit += i == 0 ? pollutant.PollutantMeasureUnit : ";" + pollutant.PollutantMeasureUnit;
                    sb.Append("{connectNulls:true,");

                    #region 对应Y轴
                    int ynum = -1;
                    if (factor.Equals(factorY))
                        ynum = pollutantList.IndexOf(pollutant);
                    else
                        ynum = pollutantList.IndexOf(pollutantList.Where(x => !x.PollutantCode.Equals(factorY) && x.PollutantMeasureUnit == pollutant.PollutantMeasureUnit).FirstOrDefault());
                    sb.AppendFormat("yAxis: {0},", ynum < 0 ? 0 : ynum);//Name属性 
                    #endregion

                    #region Name
                    sb.AppendFormat("name:'{0}',", factorName);//Name属性
                    #endregion

                    #region Data
                    string data = "";

                    foreach (DataRowView row in dv)
                    {
                        DateTime tstamp = Convert.ToDateTime(row[XName]);
                        string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);

                        #region X、Y值
                        if (row[factor] != DBNull.Value)
                        {
                            if (pollutant.PollutantMeasureUnit == "μg/m³" && !FactorsCode.Contains(pollutant.PollutantCode))
                            {
                                data += "[" + time + "," + (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[factor]) * 1000, Convert.ToInt32(pollutant.PollutantDecimalNum) - 3).ToString()) + "],";
                            }
                            else
                            {
                                data += "[" + time + "," + (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[factor]), Convert.ToInt32(pollutant.PollutantDecimalNum)).ToString()) + "],";
                            }

                        }
                        else
                        {
                            data += "[" + time + ",null],";
                        }
                        #endregion

                    }
                    sb.AppendFormat("data:[{0}]", data.Equals("") ? "" : data.Substring(0, data.Length - 1) + "");
                    #endregion

                    #region 是否显示
                    DataTable dt = dv.ToTable();
                    sb.AppendFormat(",visible:{0}", Convert.ToInt32(dt.Compute("Count(" + factor + ")", "")) > 0 ? "true" : "false");
                    #endregion

                    #region ToolTip
                    sb.Append(",tooltip: { valueSuffix: '" + (pollutant.PollutantMeasureUnit == "mg/m3" ? "mg/m<sup>3</sup>" : pollutant.PollutantMeasureUnit) + "' }");//Name属性                   
                    #endregion
                    sb.Append("},");
                }
                catch
                {
                }
            }
            if (sb.ToString().Substring(sb.ToString().Length - 1, 1).Trim().Equals(","))
                sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));//去除最后一个逗号
            sb.Append("]");
            #endregion

            #region title
            if (pt == "Air1")
            {
                if (pointid != -9999) sb.Append(",titleText:" + "'" + GetPointName(pageType, pointid) + "'");
                //if (pointid != -9999) sb.Append(",titleText:" + "'" + name + "'");
                else if (dv.Count > 0 && dv.ToTable().Columns.Contains("portName") && dv[0]["portName"] != DBNull.Value)
                    //sb.Append(",titleText:" + "'" + dv[0]["portName"] + name +"' ");
                    sb.Append(",titleText:" + "'" + name + "' ");
                else
                    sb.Append(",titleText:" + "''");
            }
            else
            {
                //if (pointid != -9999) sb.Append(",titleText:" + "'" + GetPointName(pageType, pointid) + name + "'");
                if (pointid != -9999) sb.Append(",titleText:" + "'" + name + "'");
                else if (dv.Count > 0 && dv.ToTable().Columns.Contains("portName") && dv[0]["portName"] != DBNull.Value)
                    //sb.Append(",titleText:" + "'" + dv[0]["portName"] + name +"' ");
                    sb.Append(",titleText:" + "'" + name + "' ");
                else
                    sb.Append(",titleText:" + "''");
            }

            #endregion

            #region tooltip
            //sb.Append(",tooltip:{" + tooltip + "}");//修改by lvyun 2017-08-21
            if (!string.IsNullOrWhiteSpace(tooltip))
            {
                sb.Append(",tooltip:{formatter: function () {return Highcharts.dateFormat('" + tooltip + "', this.x) + '<br/>' + this.series.name+':' + this.y + ' '+this.series.tooltipOptions.valueSuffix;}}");

            }
            else
            {
                sb.Append(",tooltip:{formatter: function () {return Highcharts.dateFormat('%m/%d日 %H时',this.x)+'<br/>'+ this.series.name+':'+this.y+' '+this.series.tooltipOptions.valueSuffix;}}");

            }
            #endregion

            #region xAxis
            if (!XLabelfomatter.Equals(""))
            {
                //sb.Append(",xAxisData:{type:'datetime',labels:{" + XLabelfomatter + "}}");
                sb.Append(",xAxisData:{type:'datetime',labels:{step:2,formatter: function () {return Highcharts.dateFormat('" + XLabelfomatter + "',this.value);}}}");//修改by lvyun 2017-08-21
            }
            else
            {
                sb.Append(",xAxisData:{type:'datetime',labels:{step:2,formatter: function () {return Highcharts.dateFormat('%m/%d %H时',this.value);}}}");//修改by lvyun 2017-08-21
            }

            #endregion

            #region YAxis
            sb.Append(",yAxis:[");
            if (pollutantList != null)
            {
                int num = 0;
                foreach (SmartEP.Core.Interfaces.IPollutant poll in pollutantList)
                {
                    sb.Append("{");
                    #region title属性
                    sb.Append("title:{");
                    sb.AppendFormat("text: \"{0}\"", poll.PollutantMeasureUnit == "mg/m3" ? "mg/m<sup>3</sup>" : poll.PollutantMeasureUnit);
                    #region title下的style属性
                    //sb.Append("style:{");
                    //if (yAxis.TitleColor != null && !yAxis.TitleColor.Trim().Equals(""))
                    //{
                    //    sb.AppendFormat("color:\"{0}\"", yAxis.TitleColor);
                    //}
                    //sb.Append("}");
                    #endregion
                    sb.Append("}");
                    #endregion
                    sb.Append(",labels: {format: '{value:.,0f}'}");
                    #region 对数轴
                    //sb.Append(",endOnTick: false,type: 'logarithmic'");
                    #endregion
                    #region 超标等级线
                    plotLines = "";
                    if (excessiveList != null)
                    {
                        if (poll.PollutantMeasureUnit == "μg/m³" && !FactorsCode.Contains(poll.PollutantCode))
                        {
                            max = dv.ToTable().Compute("Max(" + poll.PollutantCode + ")", "") != DBNull.Value ? Convert.ToDecimal(dv.ToTable().Compute("Max(" + poll.PollutantCode + ")", "")) * 1000 : -1;
                        }
                        else
                        {
                            max = dv.ToTable().Compute("Max(" + poll.PollutantCode + ")", "") != DBNull.Value ? Convert.ToDecimal(dv.ToTable().Compute("Max(" + poll.PollutantCode + ")", "")) : -1;
                        }
                        //max = dv.ToTable().Compute("Max(" + poll.PollutantCode + ")", "") != DBNull.Value ? Convert.ToDecimal(dv.ToTable().Compute("Max(" + poll.PollutantCode + ")", "")) : -1;
                        foreach (ExcessiveSettingInfo excess in excessiveList)
                        {
                            if (excess.PollutantGuid.Equals(poll.PollutantGuid))
                            {
                                max = max == -1 || max < excess.excessiveUpper ? excess.excessiveUpper : max;
                                #region 下限
                                //plotLines += "{";
                                //plotLines += string.Format("value:{0}", excess.excessiveLow);
                                //plotLines += string.Format(",color:\"{0}\"", "red");
                                //plotLines += string.Format(",width:{0}", 2);
                                //plotLines += string.Format(",dashStyle:'{0}'", "shortdot");
                                //#region Label属性
                                //plotLines += ",label: {";
                                //plotLines += string.Format("text:\"{0}\"", "超下限");
                                //plotLines += string.Format(",align:\"{0}\"", "left");
                                //plotLines += "}";
                                //#endregion
                                //plotLines += "},";
                                #endregion

                                #region 上限
                                plotLines += "{";
                                plotLines += string.Format("value:{0}", excess.excessiveUpper);
                                plotLines += string.Format(",color:\"{0}\"", "red");
                                plotLines += string.Format(",width:{0}", 2);
                                plotLines += string.Format(",dashStyle:'{0}'", "shortdot");
                                #region Label属性
                                plotLines += ",label: {";
                                plotLines += string.Format("text:\"{0}\"", "超标限(" + excess.excessiveUpper + ")");
                                plotLines += string.Format(",align:\"{0}\"", "left");
                                plotLines += "}";
                                #endregion
                                plotLines += "},";
                                #endregion
                            }
                        }
                        #region 等级线
                        sb.Append(",plotLines: [" + (!plotLines.Equals("") ? plotLines.Substring(0, plotLines.Length - 1) : "") + "]");
                        #endregion
                        #region 最大值、最小值
                        sb.Append(",min:0");
                        if (max != -1)
                            sb.Append(",max:" + max);
                        #endregion
                    }
                    #endregion

                    #region Y轴左右位置
                    if (num % 2 == 0)
                        sb.Append(",opposite: false");
                    else
                        sb.Append(",opposite: true");
                    num++;
                    #endregion

                    sb.Append("},");
                    #region 注释
                    //#region Y轴线条
                    //sb.Append(",lineWidth: 1");
                    //#endregion

                    //#region 最小值属性
                    //if (yAxis.Min != null && !yAxis.Min.Trim().Equals(""))
                    //    sb.Append("min:" + yAxis.Min + ",");
                    //#endregion

                    //#region 最大值属性
                    //if (yAxis.Max != null && !yAxis.Max.Trim().Equals(""))
                    //    sb.Append("max:" + yAxis.Max + ",");
                    //#endregion

                    //#region 等级域属性
                    //sb.Append("plotBands: [");
                    //if (plotList != null && plotList.Count > 0)
                    //{
                    //    foreach (PlotBands plotbands in plotList)
                    //    {
                    //        sb.Append("{");
                    //        if (plotbands.BandsColor != null && !plotbands.BandsColor.Trim().Equals(""))
                    //            sb.AppendFormat("color:\"{0}\",", plotbands.BandsColor);
                    //        if (plotbands.From != null && !plotbands.From.Trim().Equals(""))
                    //            sb.AppendFormat("from:\"{0}\",", plotbands.From);
                    //        if (plotbands.To != null && !plotbands.To.Trim().Equals(""))
                    //            sb.AppendFormat("to:\"{0}\",", plotbands.To);
                    //        #region Label属性
                    //        sb.Append("label: {");
                    //        if (plotbands.LabelText != null && !plotbands.LabelText.Trim().Equals(""))
                    //            sb.AppendFormat("text:\"{0}\",", plotbands.LabelText);
                    //        if (plotbands.LabelALign != null && !plotbands.LabelALign.Trim().Equals(""))
                    //            sb.AppendFormat("align:\"{0}\"", plotbands.LabelALign);
                    //        sb.Append("}");
                    //        #endregion
                    //        sb.Append("},");
                    //    }
                    //    sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));//去除最后一个逗号
                    //}
                    //sb.Append("],");
                    //#endregion
                    #endregion
                }
                if (sb.ToString().Substring(sb.ToString().Length - 1, 1).Trim().Equals(","))
                    sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));//去除最后一个逗号
            }
            else
            {
                sb.Append("{title: {text: '浓度'}}");
            }
            sb.Append("]");
            #endregion

            #region 图表type
            if (!chartType.Equals("")) sb.AppendFormat(",chartType:'{0}'", chartType);
            #endregion

            sb.Append("}");

            return sb.ToString().Replace("\r\n", "");
        }
        /// <summary>
        /// 构造Chart JSon数据【小时、日数据】
        /// </summary>
        /// <param name="dv">数据源</param>
        /// <param name="FactorCode">因子编码</param>
        /// <param name="XName">X轴字段名</param>
        /// <param name="tooltip">Chart节点对应的js悬停样式</param>
        /// <param name="chartType">数据类型</param>
        /// <param name="pageType">应用程序类型</param>
        /// <returns></returns>
        public string GetChartDataRegion(DataView dv, string[] FactorCode, string XName, string tooltip, string chartType, string pageType, string XLabelfomatter = "", IQueryable<ExcessiveSettingInfo> excessiveList = null, string region="",string name = "")
        {
          string FactorsCode = BindFactors("979f05bb-f730-4285-8aee-fafdce1360e2", "code");
          dv = dv.ToTable(true).DefaultView;//去除重复数据
          dv.Sort = XName + " ASC";
          StringBuilder sb = new StringBuilder();
          List<SmartEP.Core.Interfaces.IPollutant> pollutantList = new List<SmartEP.Core.Interfaces.IPollutant>(); ;
          decimal max = -1;
          string plotLines = "";
          sb.Append("{");
          string pt = string.Empty;
          if (pageType == "Air1")
          {
            pageType = "Air";
            pt = "Air1";
          }
          #region series
          sb.Append("series:[");
          for (int i = 0; i < FactorCode.Length; i++)
          {
            try
            {
              string factor = FactorCode[i];
              SmartEP.Core.Interfaces.IPollutant pollutant = GetPollutantName(pageType, factor);
              string factorName = pollutant.PollutantName;
              //用于显示多Y轴
              if (pollutantList.Count == 0 || !pollutantList.Where(x => !x.PollutantCode.Equals(factorY)).Select(x => x.PollutantMeasureUnit).ToArray().Contains(pollutant.PollutantMeasureUnit) || factor.Equals(factorY))
                pollutantList.Add(pollutant);
              //if (!unit.Contains(pollutant.PollutantMeasureUnit)) unit += i == 0 ? pollutant.PollutantMeasureUnit : ";" + pollutant.PollutantMeasureUnit;
              sb.Append("{connectNulls:true,");

              #region 对应Y轴
              int ynum = -1;
              if (factor.Equals(factorY))
                ynum = pollutantList.IndexOf(pollutant);
              else
                ynum = pollutantList.IndexOf(pollutantList.Where(x => !x.PollutantCode.Equals(factorY) && x.PollutantMeasureUnit == pollutant.PollutantMeasureUnit).FirstOrDefault());
              sb.AppendFormat("yAxis: {0},", ynum < 0 ? 0 : ynum);//Name属性 
              #endregion

              #region Name
              sb.AppendFormat("name:'{0}',", factorName);//Name属性
              #endregion

              #region Data
              string data = "";

              foreach (DataRowView row in dv)
              {
                DateTime tstamp = Convert.ToDateTime(row[XName]);
                string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);

                #region X、Y值
                if (row[factor] != DBNull.Value)
                {
                  if (pollutant.PollutantMeasureUnit == "μg/m³" && !FactorsCode.Contains(pollutant.PollutantCode))
                  {
                    data += "[" + time + "," + (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[factor]) * 1000, Convert.ToInt32(pollutant.PollutantDecimalNum) - 3).ToString()) + "],";
                  }
                  else
                  {
                    data += "[" + time + "," + (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[factor]), Convert.ToInt32(pollutant.PollutantDecimalNum)).ToString()) + "],";
                  }

                }
                else
                {
                  data += "[" + time + ",null],";
                }
                #endregion

              }
              sb.AppendFormat("data:[{0}]", data.Equals("") ? "" : data.Substring(0, data.Length - 1) + "");
              #endregion

              #region 是否显示
              DataTable dt = dv.ToTable();
              sb.AppendFormat(",visible:{0}", Convert.ToInt32(dt.Compute("Count(" + factor + ")", "")) > 0 ? "true" : "false");
              #endregion

              #region ToolTip
              sb.Append(",tooltip: { valueSuffix: '" + (pollutant.PollutantMeasureUnit == "mg/m3" ? "mg/m<sup>3</sup>" : pollutant.PollutantMeasureUnit) + "' }");//Name属性                   
              #endregion
              sb.Append("},");
            }
            catch
            {
            }
          }
          if (sb.ToString().Substring(sb.ToString().Length - 1, 1).Trim().Equals(","))
            sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));//去除最后一个逗号
          sb.Append("]");
          #endregion

          #region title
          if (pt == "Air1")
          {
            sb.Append(",titleText:" + "'" + region + "'");

          }
          else
          {
            sb.Append(",titleText:" + "'" + region + "'");
          }

          #endregion

          #region tooltip
          //sb.Append(",tooltip:{" + tooltip + "}");//修改by lvyun 2017-08-21
          if (!string.IsNullOrWhiteSpace(tooltip))
          {
            sb.Append(",tooltip:{formatter: function () {return Highcharts.dateFormat('" + tooltip + "', this.x) + '<br/>' + this.series.name+':' + this.y + ' '+this.series.tooltipOptions.valueSuffix;}}");

          }
          else
          {
            sb.Append(",tooltip:{formatter: function () {return Highcharts.dateFormat('%m/%d日 %H时',this.x)+'<br/>'+ this.series.name+':'+this.y+' '+this.series.tooltipOptions.valueSuffix;}}");

          }
          #endregion

          #region xAxis
          if (!XLabelfomatter.Equals(""))
          {
            //sb.Append(",xAxisData:{type:'datetime',labels:{" + XLabelfomatter + "}}");
            sb.Append(",xAxisData:{type:'datetime',labels:{step:2,formatter: function () {return Highcharts.dateFormat('" + XLabelfomatter + "',this.value);}}}");//修改by lvyun 2017-08-21
          }
          else
          {
            sb.Append(",xAxisData:{type:'datetime',labels:{step:2,formatter: function () {return Highcharts.dateFormat('%m/%d %H时',this.value);}}}");//修改by lvyun 2017-08-21
          }

          #endregion

          #region YAxis
          sb.Append(",yAxis:[");
          if (pollutantList != null)
          {
            int num = 0;
            foreach (SmartEP.Core.Interfaces.IPollutant poll in pollutantList)
            {
              sb.Append("{");
              #region title属性
              sb.Append("title:{");
              sb.AppendFormat("text: \"{0}\"", poll.PollutantMeasureUnit == "mg/m3" ? "mg/m<sup>3</sup>" : poll.PollutantMeasureUnit);
              #region title下的style属性
              //sb.Append("style:{");
              //if (yAxis.TitleColor != null && !yAxis.TitleColor.Trim().Equals(""))
              //{
              //    sb.AppendFormat("color:\"{0}\"", yAxis.TitleColor);
              //}
              //sb.Append("}");
              #endregion
              sb.Append("}");
              #endregion
              sb.Append(",labels: {format: '{value:.,0f}'}");
              #region 对数轴
              //sb.Append(",endOnTick: false,type: 'logarithmic'");
              #endregion
              #region 超标等级线
              plotLines = "";
              if (excessiveList != null)
              {
                if (poll.PollutantMeasureUnit == "μg/m³" && !FactorsCode.Contains(poll.PollutantCode))
                {
                  max = dv.ToTable().Compute("Max(" + poll.PollutantCode + ")", "") != DBNull.Value ? Convert.ToDecimal(dv.ToTable().Compute("Max(" + poll.PollutantCode + ")", "")) * 1000 : -1;
                }
                else
                {
                  max = dv.ToTable().Compute("Max(" + poll.PollutantCode + ")", "") != DBNull.Value ? Convert.ToDecimal(dv.ToTable().Compute("Max(" + poll.PollutantCode + ")", "")) : -1;
                }
                //max = dv.ToTable().Compute("Max(" + poll.PollutantCode + ")", "") != DBNull.Value ? Convert.ToDecimal(dv.ToTable().Compute("Max(" + poll.PollutantCode + ")", "")) : -1;
                foreach (ExcessiveSettingInfo excess in excessiveList)
                {
                  if (excess.PollutantGuid.Equals(poll.PollutantGuid))
                  {
                    max = max == -1 || max < excess.excessiveUpper ? excess.excessiveUpper : max;
                    #region 下限
                    //plotLines += "{";
                    //plotLines += string.Format("value:{0}", excess.excessiveLow);
                    //plotLines += string.Format(",color:\"{0}\"", "red");
                    //plotLines += string.Format(",width:{0}", 2);
                    //plotLines += string.Format(",dashStyle:'{0}'", "shortdot");
                    //#region Label属性
                    //plotLines += ",label: {";
                    //plotLines += string.Format("text:\"{0}\"", "超下限");
                    //plotLines += string.Format(",align:\"{0}\"", "left");
                    //plotLines += "}";
                    //#endregion
                    //plotLines += "},";
                    #endregion

                    #region 上限
                    plotLines += "{";
                    plotLines += string.Format("value:{0}", excess.excessiveUpper);
                    plotLines += string.Format(",color:\"{0}\"", "red");
                    plotLines += string.Format(",width:{0}", 2);
                    plotLines += string.Format(",dashStyle:'{0}'", "shortdot");
                    #region Label属性
                    plotLines += ",label: {";
                    plotLines += string.Format("text:\"{0}\"", "超标限(" + excess.excessiveUpper + ")");
                    plotLines += string.Format(",align:\"{0}\"", "left");
                    plotLines += "}";
                    #endregion
                    plotLines += "},";
                    #endregion
                  }
                }
                #region 等级线
                sb.Append(",plotLines: [" + (!plotLines.Equals("") ? plotLines.Substring(0, plotLines.Length - 1) : "") + "]");
                #endregion
                #region 最大值、最小值
                sb.Append(",min:0");
                if (max != -1)
                  sb.Append(",max:" + max);
                #endregion
              }
              #endregion

              #region Y轴左右位置
              if (num % 2 == 0)
                sb.Append(",opposite: false");
              else
                sb.Append(",opposite: true");
              num++;
              #endregion

              sb.Append("},");
              #region 注释
              //#region Y轴线条
              //sb.Append(",lineWidth: 1");
              //#endregion

              //#region 最小值属性
              //if (yAxis.Min != null && !yAxis.Min.Trim().Equals(""))
              //    sb.Append("min:" + yAxis.Min + ",");
              //#endregion

              //#region 最大值属性
              //if (yAxis.Max != null && !yAxis.Max.Trim().Equals(""))
              //    sb.Append("max:" + yAxis.Max + ",");
              //#endregion

              //#region 等级域属性
              //sb.Append("plotBands: [");
              //if (plotList != null && plotList.Count > 0)
              //{
              //    foreach (PlotBands plotbands in plotList)
              //    {
              //        sb.Append("{");
              //        if (plotbands.BandsColor != null && !plotbands.BandsColor.Trim().Equals(""))
              //            sb.AppendFormat("color:\"{0}\",", plotbands.BandsColor);
              //        if (plotbands.From != null && !plotbands.From.Trim().Equals(""))
              //            sb.AppendFormat("from:\"{0}\",", plotbands.From);
              //        if (plotbands.To != null && !plotbands.To.Trim().Equals(""))
              //            sb.AppendFormat("to:\"{0}\",", plotbands.To);
              //        #region Label属性
              //        sb.Append("label: {");
              //        if (plotbands.LabelText != null && !plotbands.LabelText.Trim().Equals(""))
              //            sb.AppendFormat("text:\"{0}\",", plotbands.LabelText);
              //        if (plotbands.LabelALign != null && !plotbands.LabelALign.Trim().Equals(""))
              //            sb.AppendFormat("align:\"{0}\"", plotbands.LabelALign);
              //        sb.Append("}");
              //        #endregion
              //        sb.Append("},");
              //    }
              //    sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));//去除最后一个逗号
              //}
              //sb.Append("],");
              //#endregion
              #endregion
            }
            if (sb.ToString().Substring(sb.ToString().Length - 1, 1).Trim().Equals(","))
              sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));//去除最后一个逗号
          }
          else
          {
            sb.Append("{title: {text: '浓度'}}");
          }
          sb.Append("]");
          #endregion

          #region 图表type
          if (!chartType.Equals("")) sb.AppendFormat(",chartType:'{0}'", chartType);
          #endregion

          sb.Append("}");

          return sb.ToString().Replace("\r\n", "");
        }
        /// <summary>
        /// 构造Chart JSon数据【小时、日数据】
        /// </summary>
        /// <param name="dv">数据源</param>
        /// <param name="FactorCode">因子编码</param>
        /// <param name="XName">X轴字段名</param>
        /// <param name="tooltip">Chart节点对应的js悬停样式</param>
        /// <param name="chartType">数据类型</param>
        /// <param name="pageType">应用程序类型</param>
        /// <returns></returns>
        public string GetChartDataNew(DataView dv, string[] FactorCode, string XName, string tooltip, string chartType, string pageType, string XLabelfomatter = "", IQueryable<ExcessiveSettingInfo> excessiveList = null, int pointid = -9999, string name = "")
        {
            string FactorsCode = BindFactors("979f05bb-f730-4285-8aee-fafdce1360e2", "code");
            dv = dv.ToTable(true).DefaultView;//去除重复数据
            dv.Sort = XName + " ASC";
            StringBuilder sb = new StringBuilder();
            List<SmartEP.Core.Interfaces.IPollutant> pollutantList = new List<SmartEP.Core.Interfaces.IPollutant>(); ;
            decimal max = -1;
            string plotLines = "";
            sb.Append("{");

            #region series
            sb.Append("series:[");
            for (int i = 0; i < FactorCode.Length; i++)
            {
                try
                {
                    string factor = FactorCode[i];
                    SmartEP.Core.Interfaces.IPollutant pollutant = GetPollutantName(pageType, factor);
                    string factorName = pollutant.PollutantName;
                    //用于显示多Y轴
                    if (pollutantList.Count == 0 || !pollutantList.Where(x => !x.PollutantCode.Equals(factorY)).Select(x => x.PollutantMeasureUnit).ToArray().Contains(pollutant.PollutantMeasureUnit) || factor.Equals(factorY))
                        pollutantList.Add(pollutant);
                    //if (!unit.Contains(pollutant.PollutantMeasureUnit)) unit += i == 0 ? pollutant.PollutantMeasureUnit : ";" + pollutant.PollutantMeasureUnit;
                    sb.Append("{");

                    #region 对应Y轴
                    int ynum = -1;
                    if (factor.Equals(factorY))
                        ynum = pollutantList.IndexOf(pollutant);
                    else
                        ynum = pollutantList.IndexOf(pollutantList.Where(x => !x.PollutantCode.Equals(factorY) && x.PollutantMeasureUnit == pollutant.PollutantMeasureUnit).FirstOrDefault());
                    sb.AppendFormat("yAxis: {0},", ynum < 0 ? 0 : ynum);//Name属性 
                    #endregion

                    #region Name
                    sb.AppendFormat("name:'{0}',", factorName);//Name属性
                    #endregion

                    #region Data
                    string data = "";

                    foreach (DataRowView row in dv)
                    {
                        DateTime tstamp = Convert.ToDateTime(row[XName]);
                        string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);

                        #region X、Y值
                        if (row[factor] != DBNull.Value)
                        {
                            if (pollutant.PollutantMeasureUnit == "μg/m³" && !FactorsCode.Contains(pollutant.PollutantCode))
                            {
                                data += "[" + time + "," + (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[factor]) * 1000, Convert.ToInt32(pollutant.PollutantDecimalNum) - 3).ToString()) + "],";
                            }
                            else
                            {
                                data += "[" + time + "," + (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[factor]), Convert.ToInt32(pollutant.PollutantDecimalNum)).ToString()) + "],";
                            }

                        }
                        else
                        {
                            data += "[" + time + ",null],";
                        }
                        #endregion

                    }
                    sb.AppendFormat("data:[{0}]", data.Equals("") ? "" : data.Substring(0, data.Length - 1) + "");
                    #endregion

                    #region 是否显示
                    DataTable dt = dv.ToTable();
                    sb.AppendFormat(",visible:{0}", Convert.ToInt32(dt.Compute("Count(" + factor + ")", "")) > 0 ? "true" : "false");
                    #endregion

                    #region ToolTip
                    sb.Append(",tooltip: { valueSuffix: '" + (pollutant.PollutantMeasureUnit == "mg/m3" ? "mg/m<sup>3</sup>" : pollutant.PollutantMeasureUnit) + "' }");//Name属性                   
                    #endregion
                    sb.Append("},");
                }
                catch
                {
                }
            }
            if (sb.ToString().Substring(sb.ToString().Length - 1, 1).Trim().Equals(","))
                sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));//去除最后一个逗号
            sb.Append("]");
            #endregion

            #region title
            if (pointid != -9999) sb.Append(",titleText:" + "'" + GetPointName(pageType, pointid) + name + "'");
            //if (pointid != -9999) sb.Append(",titleText:" + "'" + name + "'");
            else if (dv.Count > 0 && dv.ToTable().Columns.Contains("portName") && dv[0]["portName"] != DBNull.Value)
                //sb.Append(",titleText:" + "'" + dv[0]["portName"] + name +"' ");
                sb.Append(",titleText:" + "'" + name + "' ");
            else
                sb.Append(",titleText:" + "''");
            #endregion

            #region tooltip
            sb.Append(",tooltip:{" + tooltip + "}");
            #endregion

            #region xAxis
            if (!XLabelfomatter.Equals(""))
                sb.Append(",xAxisData:{type:'datetime',labels:{" + XLabelfomatter + "}}");
            #endregion

            #region YAxis
            sb.Append(",yAxis:[");
            if (pollutantList != null)
            {
                int num = 0;
                foreach (SmartEP.Core.Interfaces.IPollutant poll in pollutantList)
                {
                    sb.Append("{");
                    #region title属性
                    sb.Append("title:{");
                    sb.AppendFormat("text: \"{0}\"", poll.PollutantMeasureUnit == "mg/m3" ? "mg/m<sup>3</sup>" : poll.PollutantMeasureUnit);
                    #region title下的style属性
                    //sb.Append("style:{");
                    //if (yAxis.TitleColor != null && !yAxis.TitleColor.Trim().Equals(""))
                    //{
                    //    sb.AppendFormat("color:\"{0}\"", yAxis.TitleColor);
                    //}
                    //sb.Append("}");
                    #endregion
                    sb.Append("}");
                    #endregion
                    sb.Append(",labels: {format: '{value:.,0f}'}");
                    #region 对数轴
                    //sb.Append(",endOnTick: false,type: 'logarithmic'");
                    #endregion
                    #region 超标等级线
                    plotLines = "";
                    if (excessiveList != null)
                    {
                        if (poll.PollutantMeasureUnit == "μg/m³" && !FactorsCode.Contains(poll.PollutantCode))
                        {
                            max = dv.ToTable().Compute("Max(" + poll.PollutantCode + ")", "") != DBNull.Value ? Convert.ToDecimal(dv.ToTable().Compute("Max(" + poll.PollutantCode + ")", "")) * 1000 : -1;
                        }
                        else
                        {
                            max = dv.ToTable().Compute("Max(" + poll.PollutantCode + ")", "") != DBNull.Value ? Convert.ToDecimal(dv.ToTable().Compute("Max(" + poll.PollutantCode + ")", "")) : -1;
                        }
                        //max = dv.ToTable().Compute("Max(" + poll.PollutantCode + ")", "") != DBNull.Value ? Convert.ToDecimal(dv.ToTable().Compute("Max(" + poll.PollutantCode + ")", "")) : -1;
                        foreach (ExcessiveSettingInfo excess in excessiveList)
                        {
                            if (excess.PollutantGuid.Equals(poll.PollutantGuid))
                            {
                                max = max == -1 || max < excess.excessiveUpper ? excess.excessiveUpper : max;
                                #region 下限
                                //plotLines += "{";
                                //plotLines += string.Format("value:{0}", excess.excessiveLow);
                                //plotLines += string.Format(",color:\"{0}\"", "red");
                                //plotLines += string.Format(",width:{0}", 2);
                                //plotLines += string.Format(",dashStyle:'{0}'", "shortdot");
                                //#region Label属性
                                //plotLines += ",label: {";
                                //plotLines += string.Format("text:\"{0}\"", "超下限");
                                //plotLines += string.Format(",align:\"{0}\"", "left");
                                //plotLines += "}";
                                //#endregion
                                //plotLines += "},";
                                #endregion

                                #region 上限
                                plotLines += "{";
                                plotLines += string.Format("value:{0}", excess.excessiveUpper);
                                plotLines += string.Format(",color:\"{0}\"", "red");
                                plotLines += string.Format(",width:{0}", 2);
                                plotLines += string.Format(",dashStyle:'{0}'", "shortdot");
                                #region Label属性
                                plotLines += ",label: {";
                                plotLines += string.Format("text:\"{0}\"", "超标限(" + excess.excessiveUpper + ")");
                                plotLines += string.Format(",align:\"{0}\"", "left");
                                plotLines += "}";
                                #endregion
                                plotLines += "},";
                                #endregion
                            }
                        }
                        #region 等级线
                        sb.Append(",plotLines: [" + (!plotLines.Equals("") ? plotLines.Substring(0, plotLines.Length - 1) : "") + "]");
                        #endregion
                        #region 最大值、最小值
                        sb.Append(",min:0");
                        if (max != -1)
                            sb.Append(",max:" + max);
                        #endregion
                    }
                    #endregion

                    #region Y轴左右位置
                    if (num % 2 == 0)
                        sb.Append(",opposite: false");
                    else
                        sb.Append(",opposite: true");
                    num++;
                    #endregion

                    sb.Append("},");
                    #region 注释
                    //#region Y轴线条
                    //sb.Append(",lineWidth: 1");
                    //#endregion

                    //#region 最小值属性
                    //if (yAxis.Min != null && !yAxis.Min.Trim().Equals(""))
                    //    sb.Append("min:" + yAxis.Min + ",");
                    //#endregion

                    //#region 最大值属性
                    //if (yAxis.Max != null && !yAxis.Max.Trim().Equals(""))
                    //    sb.Append("max:" + yAxis.Max + ",");
                    //#endregion

                    //#region 等级域属性
                    //sb.Append("plotBands: [");
                    //if (plotList != null && plotList.Count > 0)
                    //{
                    //    foreach (PlotBands plotbands in plotList)
                    //    {
                    //        sb.Append("{");
                    //        if (plotbands.BandsColor != null && !plotbands.BandsColor.Trim().Equals(""))
                    //            sb.AppendFormat("color:\"{0}\",", plotbands.BandsColor);
                    //        if (plotbands.From != null && !plotbands.From.Trim().Equals(""))
                    //            sb.AppendFormat("from:\"{0}\",", plotbands.From);
                    //        if (plotbands.To != null && !plotbands.To.Trim().Equals(""))
                    //            sb.AppendFormat("to:\"{0}\",", plotbands.To);
                    //        #region Label属性
                    //        sb.Append("label: {");
                    //        if (plotbands.LabelText != null && !plotbands.LabelText.Trim().Equals(""))
                    //            sb.AppendFormat("text:\"{0}\",", plotbands.LabelText);
                    //        if (plotbands.LabelALign != null && !plotbands.LabelALign.Trim().Equals(""))
                    //            sb.AppendFormat("align:\"{0}\"", plotbands.LabelALign);
                    //        sb.Append("}");
                    //        #endregion
                    //        sb.Append("},");
                    //    }
                    //    sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));//去除最后一个逗号
                    //}
                    //sb.Append("],");
                    //#endregion
                    #endregion
                }
                if (sb.ToString().Substring(sb.ToString().Length - 1, 1).Trim().Equals(","))
                    sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));//去除最后一个逗号
            }
            else
            {
                sb.Append("{title: {text: '浓度'}}");
            }
            sb.Append("]");
            #endregion

            #region 图表type
            if (!chartType.Equals("")) sb.AppendFormat(",chartType:'{0}'", chartType);
            #endregion

            sb.Append("}");

            return sb.ToString().Replace("\r\n", "");
        }
        /// <summary>
        /// 获取因子
        /// </summary>
        /// <param name="CategoryUid"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public string BindFactors(string CategoryUid, string type = "name")
        {
            SmartEP.Service.BaseData.Channel.AirPollutantService m_AirPollutantService = Singleton<SmartEP.Service.BaseData.Channel.AirPollutantService>.GetInstance();
            IQueryable<PollutantCodeEntity> Pollutant = m_AirPollutantService.RetrieveList().Where(x => x.CategoryUid == CategoryUid);
            string PollutantName = "";
            if (type == "name")
            {
                string[] pollutantarry = Pollutant.Select(p => p.PollutantName).ToArray();
                foreach (string strName in pollutantarry)
                {
                    PollutantName += strName + ";";
                }
            }
            else
            {
                string[] pollutantarry = Pollutant.Select(p => p.PollutantCode).ToArray();
                foreach (string strName in pollutantarry)
                {
                    PollutantName += strName + ";";
                }
            }
            return PollutantName;
        }
        /// <summary>
        /// 构造Chart JSon数据【小时、日数据】
        /// </summary>
        /// <param name="dv">数据源</param>
        /// <param name="FactorCode">因子编码</param>
        /// <param name="XName">X轴字段名</param>
        /// <param name="tooltip">Chart节点对应的js悬停样式</param>
        /// <param name="chartType">数据类型</param>
        /// <param name="pageType">应用程序类型</param>
        /// <returns></returns>
        public string GetChart3DData(DataView dv, string[] FactorCode, string XName, string tooltip, string chartType, string pageType, string XLabelfomatter = "", IQueryable<ExcessiveSettingInfo> excessiveList = null, int pointid = -9999)
        {
            dv = dv.ToTable(true).DefaultView;//去除重复数据
            dv.Sort = XName + " ASC";
            StringBuilder sb = new StringBuilder();
            List<SmartEP.Core.Interfaces.IPollutant> pollutantList = new List<SmartEP.Core.Interfaces.IPollutant>(); ;
            decimal max = -1;
            string plotLines = "";
            sb.Append("{");

            #region 图表type
            if (!chartType.Equals("") && chartType == "columnD")
            {
                sb.Append(" var" + "  " + "chart =  new" + "  " + "  Highcharts.Chart({chart: { renderTo: 'container',  type: 'column', margin: 75,");
                sb.Append("options3d: {enabled: true, alpha: 15, beta: 15, depth: 50,viewDistance: 25}},");
            }
            #endregion

            #region series
            sb.Append("series:[");
            for (int i = 0; i < FactorCode.Length; i++)
            {
                try
                {
                    string factor = FactorCode[i];
                    SmartEP.Core.Interfaces.IPollutant pollutant = GetPollutantName(pageType, factor);
                    string factorName = pollutant.PollutantName;
                    //用于显示多Y轴
                    if (pollutantList.Count == 0 || !pollutantList.Where(x => !x.PollutantCode.Equals(factorY)).Select(x => x.PollutantMeasureUnit).ToArray().Contains(pollutant.PollutantMeasureUnit) || factor.Equals(factorY))
                        pollutantList.Add(pollutant);
                    //if (!unit.Contains(pollutant.PollutantMeasureUnit)) unit += i == 0 ? pollutant.PollutantMeasureUnit : ";" + pollutant.PollutantMeasureUnit;
                    sb.Append("{");

                    #region 对应Y轴
                    int ynum = -1;
                    if (factor.Equals(factorY))
                        ynum = pollutantList.IndexOf(pollutant);
                    else
                        ynum = pollutantList.IndexOf(pollutantList.Where(x => !x.PollutantCode.Equals(factorY) && x.PollutantMeasureUnit == pollutant.PollutantMeasureUnit).FirstOrDefault());
                    sb.AppendFormat("yAxis: {0},", ynum < 0 ? 0 : ynum);//Name属性 
                    #endregion

                    #region Name
                    sb.AppendFormat("name:'{0}',", factorName);//Name属性
                    #endregion

                    #region Data
                    string data = "";

                    foreach (DataRowView row in dv)
                    {
                        DateTime tstamp = Convert.ToDateTime(row[XName]);
                        string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);

                        #region X、Y值
                        if (row[factor] != DBNull.Value)
                        {

                            data += "[" + time + "," + (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[factor]), Convert.ToInt32(pollutant.PollutantDecimalNum)).ToString()) + "],";
                        }
                        else
                        {
                            data += "[" + time + ",null],";
                        }
                        #endregion

                    }
                    sb.AppendFormat("data:[{0}]", data.Equals("") ? "" : data.Substring(0, data.Length - 1) + "");
                    #endregion

                    #region 是否显示
                    DataTable dt = dv.ToTable();
                    sb.AppendFormat(",visible:{0}", Convert.ToInt32(dt.Compute("Count(" + factor + ")", "")) > 0 ? "true" : "false");
                    #endregion

                    #region ToolTip
                    sb.Append(",tooltip: { valueSuffix: '" + (pollutant.PollutantMeasureUnit == "mg/m3" ? "mg/m<sup>3</sup>" : pollutant.PollutantMeasureUnit) + "' }");//Name属性                   
                    #endregion
                    sb.Append("},");
                }
                catch
                {
                }
            }
            if (sb.ToString().Substring(sb.ToString().Length - 1, 1).Trim().Equals(","))
                sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));//去除最后一个逗号
            sb.Append("]");
            #endregion

            #region title
            if (pointid != -9999) sb.Append(",titleText:" + "'" + GetPointName(pageType, pointid) + "'");
            else if (dv.Count > 0 && dv.ToTable().Columns.Contains("portName") && dv[0]["portName"] != DBNull.Value)
                sb.Append(",titleText:" + "'" + dv[0]["portName"] + "'");
            else
                sb.Append(",titleText:" + "''");
            #endregion

            //#region tooltip
            //sb.Append(",tooltip:{" + tooltip + "}");
            //#endregion

            #region xAxis
            if (!XLabelfomatter.Equals(""))
                sb.Append(",xAxisData:{type:'datetime',labels:{" + XLabelfomatter + "}}");
            #endregion

            #region YAxis
            sb.Append(",yAxis:[");
            if (pollutantList != null)
            {
                int num = 0;
                foreach (SmartEP.Core.Interfaces.IPollutant poll in pollutantList)
                {
                    sb.Append("{");
                    #region title属性
                    sb.Append("title:{");
                    sb.AppendFormat("text: \"{0}\"", poll.PollutantMeasureUnit == "mg/m3" ? "mg/m<sup>3</sup>" : poll.PollutantMeasureUnit);
                    #region title下的style属性
                    //sb.Append("style:{");
                    //if (yAxis.TitleColor != null && !yAxis.TitleColor.Trim().Equals(""))
                    //{
                    //    sb.AppendFormat("color:\"{0}\"", yAxis.TitleColor);
                    //}
                    //sb.Append("}");
                    #endregion
                    sb.Append("}");
                    #endregion

                    #region 超标等级线
                    plotLines = "";
                    if (excessiveList != null)
                    {
                        max = dv.ToTable().Compute("Max(" + poll.PollutantCode + ")", "") != DBNull.Value ? Convert.ToDecimal(dv.ToTable().Compute("Max(" + poll.PollutantCode + ")", "")) : -1;
                        foreach (ExcessiveSettingInfo excess in excessiveList)
                        {
                            if (excess.PollutantGuid.Equals(poll.PollutantGuid))
                            {
                                max = max == -1 || max < excess.excessiveUpper ? excess.excessiveUpper : max;
                                #region 下限
                                //plotLines += "{";
                                //plotLines += string.Format("value:{0}", excess.excessiveLow);
                                //plotLines += string.Format(",color:\"{0}\"", "red");
                                //plotLines += string.Format(",width:{0}", 2);
                                //plotLines += string.Format(",dashStyle:'{0}'", "shortdot");
                                //#region Label属性
                                //plotLines += ",label: {";
                                //plotLines += string.Format("text:\"{0}\"", "超下限");
                                //plotLines += string.Format(",align:\"{0}\"", "left");
                                //plotLines += "}";
                                //#endregion
                                //plotLines += "},";
                                #endregion

                                #region 上限
                                plotLines += "{";
                                plotLines += string.Format("value:{0}", excess.excessiveUpper);
                                plotLines += string.Format(",color:\"{0}\"", "red");
                                plotLines += string.Format(",width:{0}", 2);
                                plotLines += string.Format(",dashStyle:'{0}'", "shortdot");
                                #region Label属性
                                plotLines += ",label: {";
                                plotLines += string.Format("text:\"{0}\"", "超标限(" + excess.excessiveUpper + ")");
                                plotLines += string.Format(",align:\"{0}\"", "left");
                                plotLines += "}";
                                #endregion
                                plotLines += "},";
                                #endregion
                            }
                        }
                        #region 等级线
                        sb.Append(",plotLines: [" + (!plotLines.Equals("") ? plotLines.Substring(0, plotLines.Length - 1) : "") + "]");
                        #endregion
                        #region 最大值、最小值
                        sb.Append(",min:0");
                        if (max != -1)
                            sb.Append(",max:" + max);
                        #endregion
                    }
                    #endregion

                    #region Y轴左右位置
                    if (num % 2 == 0)
                        sb.Append(",opposite: false");
                    else
                        sb.Append(",opposite: true");
                    num++;
                    #endregion

                    sb.Append("},");
                    #region 注释
                    #endregion
                }
                if (sb.ToString().Substring(sb.ToString().Length - 1, 1).Trim().Equals(","))
                    sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));//去除最后一个逗号
            }
            else
            {
                sb.Append("{title: {text: '浓度'}}");
            }
            sb.Append("]});");
            #endregion


            sb.Append("}");

            return sb.ToString().Replace("\r\n", "");
        }
        /// <summary>
        /// 构造Chart JSon数据【小时、日数据】
        /// </summary>
        /// <param name="dv">数据源</param>
        /// <param name="FactorCode">因子编码</param>
        /// <param name="XName">X轴字段名</param>
        /// <param name="tooltip">Chart节点对应的js悬停样式</param>
        /// <param name="chartType">数据类型</param>
        /// <param name="pageType">应用程序类型</param>
        /// <returns></returns>
        public string GetChartDataSearch(DataView dv, string[] FactorCode, string XName, string tooltip, string chartType, string pageType, string XLabelfomatter = "", IQueryable<ExcessiveSettingInfo> excessiveList = null, int pointid = -9999)
        {
            dv = dv.ToTable(true).DefaultView;//去除重复数据
            dv.Sort = XName + " ASC";
            StringBuilder sb = new StringBuilder();
            sb.Append("{chartType:'spline',");
            sb.Append("title: {text: 'Monthly Average Temperature',x: -20},");
            sb.Append(" xAxis: {type: 'datetime', dateTimeLabelFormats: { day: '%d日%H点'  } },");
            sb.Append(" yAxis: {title: { text: 'mg/m<sup>3</sup>' },  min: 0 },");
            sb.Append(" tooltip: {formatter: function() {  return '<b>'+ this.series.name +'</b><br/>'+ Highcharts.dateFormat('%d日%H点%M分', this.x) +':'+ this.y +' " + "';    } },series: [");
            string data = "";
            for (int i = 0; i < FactorCode.Length; i++)
            {
                string factorName = FactorCode[i];
                data += "{";
                data += string.Format(" name: '{0}',data:[ ", factorName);
                foreach (DataRowView row in dv)
                {
                    if (row[XName].ToString() != "")
                    {
                        DateTime tstamp = Convert.ToDateTime(row[XName]);
                        string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                        #region X、Y值
                        if (row[factorName] != DBNull.Value && row[factorName].ToString() != "")
                        {

                            data += "[" + time + "," + (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[factorName]), 3).ToString()) + "],";
                        }
                        else
                        {
                            data += "[" + time + ",null],";
                        }
                        #endregion
                    }
                }
                data += "]},";
            }
            sb.AppendFormat("{0}", data.Equals("") ? "" : data.Substring(0, data.Length - 1) + "");
            sb.Append("]  } ");
            return sb.ToString().Replace("\r\n", "");
        }
        /// <summary>
        /// 构造Chart JSon数据【小时、日数据】
        /// </summary>
        /// <param name="dv">数据源</param>
        /// <param name="FactorCode">因子编码</param>
        /// <param name="XName">X轴字段名</param>
        /// <param name="tooltip">Chart节点对应的js悬停样式</param>
        /// <param name="chartType">数据类型</param>
        /// <param name="pageType">应用程序类型</param>
        /// <returns></returns>
        public string GetChartFactorSearch(DataView dv, string[] FactorCode, string XName, string Column, string tooltip, string chartType, string pageType, string XLabelfomatter = "", IQueryable<ExcessiveSettingInfo> excessiveList = null, int pointid = -9999)
        {
            dv = dv.ToTable(true).DefaultView;//去除重复数据
            dv.Sort = XName + " ASC";

            StringBuilder sb = new StringBuilder();
            sb.Append("{chartType:'spline',");
            sb.Append("title: {text: 'Monthly Average Temperature',x: -20},");
            sb.Append(" xAxis: {type: 'datetime', dateTimeLabelFormats: { day: '%d日%H点'  } },");
            sb.Append(" yAxis: {title: { text: 'mg/m<sup>3</sup>' },  min: 0 },");
            sb.Append(" tooltip: {formatter: function() {  return '<b>'+ this.series.name +'</b><br/>'+ Highcharts.dateFormat('%d日%H点%M分', this.x) +':'+ this.y +' " + "';    } },series: [");
            string data = "";
            for (int i = 0; i < FactorCode.Length; i++)
            {
                string factorName = FactorCode[i];
                DataView newdv = new DataView(dv.ToTable());
                newdv.RowFilter = "Code='" + factorName + "'";
                data += "{";
                data += string.Format(" name: '{0}',data:[ ", factorName);
                foreach (DataRowView row in newdv)
                {
                    if (row[XName].ToString() != "")
                    {
                        DateTime tstamp = Convert.ToDateTime(row[XName]);
                        string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                        #region X、Y值
                        if (row[Column] != DBNull.Value && row[Column].ToString() != "")
                        {

                            data += "[" + time + "," + (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[Column]), 3).ToString()) + "],";
                        }
                        else
                        {
                            data += "[" + time + ",null],";
                        }
                        #endregion
                    }
                }
                data += "]},";
            }
            sb.AppendFormat("{0}", data.Equals("") ? "" : data.Substring(0, data.Length - 1) + "");
            sb.Append("]  } ");
            return sb.ToString().Replace("\r\n", "");
        }

        /// <summary>
        /// 构造Chart JSon数据【小时、日数据】
        /// </summary>
        /// <param name="dv">数据源</param>
        /// <param name="FactorCode">因子编码</param>
        /// <param name="XName">X轴字段名</param>
        /// <param name="tooltip">Chart节点对应的js悬停样式</param>
        /// <param name="chartType">数据类型</param>
        /// <param name="pageType">应用程序类型</param>
        /// <returns></returns>
        public string GetChartWeiboDataSearch(DataView dv, string[] FactorCode, string XName, string tooltip, string chartType, string pageType, string XLabelfomatter = "", IQueryable<ExcessiveSettingInfo> excessiveList = null, int pointid = -9999)
        {
            dv = dv.ToTable(true).DefaultView;//去除重复数据
            DataTable dt = dv.ToTable();
            dv.Sort = XName + " ASC";
            StringBuilder sb = new StringBuilder();
            string factor = "";
            for (int i = 0; i < FactorCode.Length; i++)
            {
                string factorName = FactorCode[i];
                if (factorName == "401")
                    factor = "温度";
                if (factorName == "402")
                    factor = "蒸汽密度";
                if (factorName == "404")
                    factor = "相对湿度";
            }
            sb.Append("{ chart: {type: 'spline'},");
            sb.Append("title: {text: 'Monthly Average Temperature',x: -20},");
            sb.Append(" xAxis: { reversed: false,type: {enabled: true,text: 'km'}},");
            sb.Append(" yAxis: {title: { text: 'Km' },  min: 0 },");
            //sb.Append(" tooltip: {formatter: function() {  return '<b>'+ this.series.name +'</b><br/>'+ this.x +':'+ this.y +' " + "';    } },series: [");
            sb.Append(" tooltip: {headerFormat: '<b>{series.name}</b><br/>', pointFormat: '{point.x}: {point.y}'},series: [");
            string data = "";
            data += "{";
            data += string.Format(" name: '{0}',data:[ ", factor);
            for (int j = 3; j < dt.Columns.Count; j++)
            {
                string factorId = dt.Columns[j].ColumnName;
                #region X、Y值
                if (dt.Rows[0][factorId] != DBNull.Value)
                {

                    data += "[" + factorId + "," + (DecimalExtension.GetRoundValue(Convert.ToDecimal(dt.Rows[0][factorId]), 3).ToString()) + "],";
                }
                else
                {
                    data += "[" + factorId + ",null],";
                }
                #endregion
            }
            data += "]},";

            sb.AppendFormat("{0}", data.Equals("") ? "" : data.Substring(0, data.Length - 1) + "");
            sb.Append("]  } ");
            return sb.ToString().Replace("\r\n", "");
        }
        /// <summary>
        /// 构造Chart JSon数据【原始审核比对】
        /// </summary>
        /// <param name="dv">数据源</param>
        /// <param name="FactorCode">因子编码</param>
        /// <param name="XName">X轴字段名</param>
        /// <param name="tooltip">Chart节点对应的js悬停样式</param>
        /// <param name="chartType">数据类型</param>
        /// <param name="pageType">应用程序类型</param>
        /// <returns></returns>
        public string GetChartDataHour(DataView dv, string[] FactorCode, string XName, string tooltip, string chartType, string pageType, string XLabelfomatter = "", IQueryable<ExcessiveSettingInfo> excessiveList = null, int pointid = -9999)
        {
            dv = dv.ToTable(true).DefaultView;//去除重复数据
            dv.Sort = XName + " ASC";
            StringBuilder sb = new StringBuilder();
            List<SmartEP.Core.Interfaces.IPollutant> pollutantList = new List<SmartEP.Core.Interfaces.IPollutant>(); ;
            decimal max = -1;
            string plotLines = "";
            string factorName = "";
            string[] Type = { "原始", "审核" };
            sb.Append("{");
            if (pageType == "Air1")
            {
                pageType = "Air";
            }
            #region series
            sb.Append("series:[");
            for (int i = 0; i < FactorCode.Length; i++)
            {
                string factor = FactorCode[i];
                SmartEP.Core.Interfaces.IPollutant pollutant = GetPollutantName(pageType, factor);
                factorName = pollutant.PollutantName;
                //用于显示多Y轴
                if (pollutantList.Count == 0 || !pollutantList.Select(x => x.PollutantMeasureUnit).ToArray().Contains(pollutant.PollutantMeasureUnit))
                    pollutantList.Add(pollutant);
                //if (!unit.Contains(pollutant.PollutantMeasureUnit)) unit += i == 0 ? pollutant.PollutantMeasureUnit : ";" + pollutant.PollutantMeasureUnit;

                for (int n = 0; n < Type.Length; n++)
                {
                    sb.Append("{");

                    #region 对应Y轴
                    int ynum = Array.IndexOf(pollutantList.Select(x => x.PollutantMeasureUnit).ToArray(), pollutant.PollutantMeasureUnit);
                    sb.AppendFormat("yAxis: {0},", ynum < 0 ? 0 : ynum);//Name属性 
                    #endregion


                    #region Data
                    string type = pollutant.PollutantName + Type[n];
                    dv.RowFilter = "type='" + Type[n] + "'";
                    #region Name
                    sb.AppendFormat("name:'{0}',", type);//Name属性
                    #endregion
                    string data = "";
                    foreach (DataRowView row in dv)
                    {

                        DateTime tstamp = Convert.ToDateTime(row[XName]);
                        string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);

                        #region X、Y值
                        if (row[factor] != DBNull.Value && decimal.Parse(row[factor].ToString()) != -10)
                        {
                            try
                            {
                                if (pollutant.PollutantMeasureUnit == "μg/m³" || pollutant.PollutantCode == "a34004" || pollutant.PollutantName.Contains("PM2.5"))
                                {
                                    data += "[" + time + "," + (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[factor]) * 1000, Convert.ToInt32(pollutant.PollutantDecimalNum) - 3).ToString()) + "],";
                                }
                                else
                                {
                                    data += "[" + time + "," + (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[factor]), Convert.ToInt32(pollutant.PollutantDecimalNum)).ToString()) + "],";
                                }
                            }
                            catch (Exception ex) { }

                            //data += "[" + time + "," + (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[factor]), Convert.ToInt32(pollutant.PollutantDecimalNum)).ToString()) + "],";
                        }
                        else
                        {
                            data += "[" + time + ",null],";
                        }
                        #endregion

                    }

                    sb.AppendFormat("data:[{0}]", data.Equals("") ? "" : data.Substring(0, data.Length - 1) + "");
                    #endregion

                    #region 是否显示
                    DataTable dt = dv.ToTable();
                    sb.AppendFormat(",visible:{0}", Convert.ToInt32(dt.Compute("Count(" + factor + ")", "")) > 0 ? "true" : "false");
                    #endregion

                    #region ToolTip
                    sb.Append(",tooltip: { valueSuffix: '" + (pollutant.PollutantMeasureUnit == "mg/m3" ? "mg/m<sup>3</sup>" : pollutant.PollutantMeasureUnit) + "' }");//Name属性                   
                    #endregion
                    sb.Append("},");
                }
            }
            if (sb.ToString().Substring(sb.ToString().Length - 1, 1).Trim().Equals(","))
                sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));//去除最后一个逗号
            sb.Append("]");
            #endregion

            #region title
            if (pointid != -9999) sb.Append(",titleText:" + "'" + GetPointName(pageType, pointid) + "'");
            else if (dv.Count > 0 && dv.ToTable().Columns.Contains("portName") && dv[0]["portName"] != DBNull.Value)
                sb.Append(",titleText:" + "'" + dv[0]["portName"] + "(" + factorName + ")" + "'");
            else
                sb.Append(",titleText:" + "''");
            #endregion

            #region tooltip
            sb.Append(",tooltip:{" + tooltip + "}");
            #endregion

            #region xAxis
            if (!XLabelfomatter.Equals(""))
                sb.Append(",xAxisData:{type:'datetime',labels:{" + XLabelfomatter + "}}");
            #endregion

            #region YAxis
            sb.Append(",yAxis:[");
            if (pollutantList != null)
            {
                int num = 0;
                foreach (SmartEP.Core.Interfaces.IPollutant poll in pollutantList)
                {
                    sb.Append("{");
                    #region title属性
                    sb.Append("title:{");
                    sb.AppendFormat("text: \"{0}\"", poll.PollutantMeasureUnit == "mg/m3" ? "mg/m<sup>3</sup>" : poll.PollutantMeasureUnit);
                    #region title下的style属性
                    //sb.Append("style:{");
                    //if (yAxis.TitleColor != null && !yAxis.TitleColor.Trim().Equals(""))
                    //{
                    //    sb.AppendFormat("color:\"{0}\"", yAxis.TitleColor);
                    //}
                    //sb.Append("}");
                    #endregion
                    sb.Append("}");
                    #endregion

                    #region 超标等级线
                    plotLines = "";
                    if (excessiveList != null)
                    {
                        if (poll.PollutantMeasureUnit == "μg/m³")
                        {
                            max = dv.ToTable().Compute("Max(" + poll.PollutantCode + ")", "") != DBNull.Value ? Convert.ToDecimal(dv.ToTable().Compute("Max(" + poll.PollutantCode + ")", "")) * 1000 : -1;
                        }
                        else
                        {
                            max = dv.ToTable().Compute("Max(" + poll.PollutantCode + ")", "") != DBNull.Value ? Convert.ToDecimal(dv.ToTable().Compute("Max(" + poll.PollutantCode + ")", "")) : -1;
                        }
                        //max = dv.ToTable().Compute("Max(" + poll.PollutantCode + ")", "") != DBNull.Value ? Convert.ToDecimal(dv.ToTable().Compute("Max(" + poll.PollutantCode + ")", "")) : -1;
                        foreach (ExcessiveSettingInfo excess in excessiveList)
                        {
                            if (excess.PollutantGuid.Equals(poll.PollutantGuid))
                            {
                                max = max == -1 || max < excess.excessiveUpper ? excess.excessiveUpper : max;
                                #region 下限
                                //plotLines += "{";
                                //plotLines += string.Format("value:{0}", excess.excessiveLow);
                                //plotLines += string.Format(",color:\"{0}\"", "red");
                                //plotLines += string.Format(",width:{0}", 2);
                                //plotLines += string.Format(",dashStyle:'{0}'", "shortdot");
                                //#region Label属性
                                //plotLines += ",label: {";
                                //plotLines += string.Format("text:\"{0}\"", "超下限");
                                //plotLines += string.Format(",align:\"{0}\"", "left");
                                //plotLines += "}";
                                //#endregion
                                //plotLines += "},";
                                #endregion

                                #region 上限
                                plotLines += "{";
                                plotLines += string.Format("value:{0}", excess.excessiveUpper);
                                plotLines += string.Format(",color:\"{0}\"", "red");
                                plotLines += string.Format(",width:{0}", 2);
                                plotLines += string.Format(",dashStyle:'{0}'", "shortdot");
                                #region Label属性
                                plotLines += ",label: {";
                                plotLines += string.Format("text:\"{0}\"", "超标限(" + excess.excessiveUpper + ")");
                                plotLines += string.Format(",align:\"{0}\"", "left");
                                plotLines += "}";
                                #endregion
                                plotLines += "},";
                                #endregion
                            }
                        }
                        #region 等级线
                        sb.Append(",plotLines: [" + (!plotLines.Equals("") ? plotLines.Substring(0, plotLines.Length - 1) : "") + "]");
                        #endregion
                        #region 最大值、最小值
                        sb.Append(",min:0");
                        if (max != -1)
                            sb.Append(",max:" + max);
                        #endregion
                    }
                    #endregion

                    #region Y轴左右位置
                    if (num % 2 == 0)
                        sb.Append(",opposite: false");
                    else
                        sb.Append(",opposite: true");
                    num++;
                    #endregion

                    sb.Append("},");
                    #region 注释
                    //#region Y轴线条
                    //sb.Append(",lineWidth: 1");
                    //#endregion

                    //#region 最小值属性
                    //if (yAxis.Min != null && !yAxis.Min.Trim().Equals(""))
                    //    sb.Append("min:" + yAxis.Min + ",");
                    //#endregion

                    //#region 最大值属性
                    //if (yAxis.Max != null && !yAxis.Max.Trim().Equals(""))
                    //    sb.Append("max:" + yAxis.Max + ",");
                    //#endregion

                    //#region 等级域属性
                    //sb.Append("plotBands: [");
                    //if (plotList != null && plotList.Count > 0)
                    //{
                    //    foreach (PlotBands plotbands in plotList)
                    //    {
                    //        sb.Append("{");
                    //        if (plotbands.BandsColor != null && !plotbands.BandsColor.Trim().Equals(""))
                    //            sb.AppendFormat("color:\"{0}\",", plotbands.BandsColor);
                    //        if (plotbands.From != null && !plotbands.From.Trim().Equals(""))
                    //            sb.AppendFormat("from:\"{0}\",", plotbands.From);
                    //        if (plotbands.To != null && !plotbands.To.Trim().Equals(""))
                    //            sb.AppendFormat("to:\"{0}\",", plotbands.To);
                    //        #region Label属性
                    //        sb.Append("label: {");
                    //        if (plotbands.LabelText != null && !plotbands.LabelText.Trim().Equals(""))
                    //            sb.AppendFormat("text:\"{0}\",", plotbands.LabelText);
                    //        if (plotbands.LabelALign != null && !plotbands.LabelALign.Trim().Equals(""))
                    //            sb.AppendFormat("align:\"{0}\"", plotbands.LabelALign);
                    //        sb.Append("}");
                    //        #endregion
                    //        sb.Append("},");
                    //    }
                    //    sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));//去除最后一个逗号
                    //}
                    //sb.Append("],");
                    //#endregion
                    #endregion
                }
                if (sb.ToString().Substring(sb.ToString().Length - 1, 1).Trim().Equals(","))
                    sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));//去除最后一个逗号
            }
            else
            {
                sb.Append("{title: {text: '浓度'}}");
            }
            sb.Append("]");
            #endregion

            #region 图表type
            if (!chartType.Equals("")) sb.AppendFormat(",chartType:'{0}'", chartType);
            #endregion

            sb.Append("}");

            return sb.ToString().Replace("\r\n", "");
        }

        /// <summary>
        /// 构造Chart JSon数据【单参数数据】
        /// </summary>
        /// <param name="dv">数据源</param>
        /// <param name="FactorCode">因子编码</param>
        /// <param name="PointIds">站点ID</param>
        /// <param name="XName">X轴字段名</param>
        /// <param name="tooltip">Chart节点对应的js悬停样式</param>
        /// <param name="chartType">数据类型</param>
        /// <param name="pageType">应用程序类型</param>
        /// <returns></returns>
        public string GetChartDataX(DataView dv, string[] FactorCode, string[] PointIds, string XName, string tooltip, string chartType, string pageType, string XLabelfomatter = "", IQueryable<ExcessiveSettingInfo> excessiveList = null, int pointid = -9999)
        {
            dv = dv.ToTable(true).DefaultView;//去除重复数据
            dv.Sort = XName + " ASC";
            if (pageType == "Air1")
            {
                pageType = "Air";
            }
            SmartEP.Core.Interfaces.IPollutant Ifactor = GetPollutantName(pageType, FactorCode[0]);
            string DecimalNum = Ifactor.PollutantDecimalNum;
            string factorName = Ifactor.PollutantName;
            StringBuilder sb = new StringBuilder();
            sb.Append("{chartType:" + "'" + chartType + "',");
            sb.Append("titleText:" + "'" + factorName + "',");

            //sb.Append(" xAxis: {type: 'datetime', dateTimeLabelFormats: { day: '%d日%H点'  } },");
            //修改by lvyun 2017-08-21
            string xAxis = "";
            if (!string.IsNullOrWhiteSpace(XLabelfomatter))
            {
                xAxis = " xAxisData: {type: 'datetime', labels: {step:2,formatter: function () {return Highcharts.dateFormat('" + XLabelfomatter + "',this.value);}} },";
            }
            else
            {
                xAxis = " xAxisData: {type: 'datetime', labels: {step:2,formatter: function () {return Highcharts.dateFormat('%d日%H点%M分',this.value);}} },";
            }
            sb.Append(xAxis);
            sb.Append(" yAxis: {title: { text: '" + (Ifactor.PollutantMeasureUnit == "mg/m3" ? "mg/m<sup>3</sup>" : Ifactor.PollutantMeasureUnit) + "' },  min: 0 },");
            if (!string.IsNullOrWhiteSpace(tooltip))
            {
                sb.Append(" tooltip: {formatter: function() {  return '<b>'+ this.series.name +'</b><br/>'+ Highcharts.dateFormat('" + tooltip + "', this.x) +':'+ this.y +' " + (Ifactor.PollutantMeasureUnit == "mg/m3" ? "mg/m<sup>3</sup>" : Ifactor.PollutantMeasureUnit) + "';    } },series: [");

            }
            else
            {
                sb.Append(" tooltip: {formatter: function() {  return '<b>'+ this.series.name +'</b><br/>'+ Highcharts.dateFormat('%d日%H点%M分', this.x) +':'+ this.y +' " + (Ifactor.PollutantMeasureUnit == "mg/m3" ? "mg/m<sup>3</sup>" : Ifactor.PollutantMeasureUnit) + "';    } },series: [");

            }
            string data = "";
            for (int i = 0; i < PointIds.Length; i++)
            {
                SmartEP.Core.Interfaces.IPollutant pollutant = GetPollutantName(pageType, FactorCode[0].ToString());
                int pointTemp = int.TryParse(PointIds[i].ToString(), out pointTemp) ? pointTemp : 0;
                string pointName = GetPointName("Air", pointTemp);
                dv.RowFilter = "PointId='" + PointIds[i] + "'";
                data += "{";
                data += string.Format(" name: '{0}',data:[ ", pointName);
                int m = 0;
                foreach (DataRowView row in dv)
                {

                    DateTime tstamp = Convert.ToDateTime(row[XName]);
                    string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                    if (m != dv.Count)
                    {
                        if (row[FactorCode[0]] != DBNull.Value)
                        {
                            if (pollutant.PollutantMeasureUnit == "μg/m³")
                            {
                                data += "[" + time + "," + (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[FactorCode[0]]) * 1000, Convert.ToInt32(pollutant.PollutantDecimalNum) - 3).ToString()) + "],";
                            }
                            else
                            {
                                data += "[" + time + "," + (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[FactorCode[0]]), Convert.ToInt32(pollutant.PollutantDecimalNum)).ToString()) + "],";
                            }
                            //data += "[" + time + "," + (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[FactorCode[0]]), Convert.ToInt32(DecimalNum)).ToString()) + "],";
                        }
                        else
                        {
                            data += "[" + time + ",null],";
                        }
                    }
                    else
                    {
                        if (row[FactorCode[0]] != DBNull.Value)
                        {

                            data += "[" + time + "," + (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[FactorCode[0]]), Convert.ToInt32(DecimalNum)).ToString()) + "]";
                        }
                        else
                        {
                            data += "[" + time + ",null]";
                        }
                    }
                    m++;
                }
                data += "]},";
            }
            sb.AppendFormat("{0}", data.Equals("") ? "" : data.Substring(0, data.Length - 1) + "");
            sb.Append("]  } ");
            return sb.ToString().Replace("\r\n", "");
        }
        /// <summary>
        /// 构造Chart JSon数据(区域)【单参数数据】
        /// </summary>
        /// <param name="dv">数据源</param>
        /// <param name="FactorCode">因子编码</param>
        /// <param name="PointIds">站点ID</param>
        /// <param name="XName">X轴字段名</param>
        /// <param name="tooltip">Chart节点对应的js悬停样式</param>
        /// <param name="chartType">数据类型</param>
        /// <param name="pageType">应用程序类型</param>
        /// <returns></returns>
        public string GetChartDataXRegion(DataView dv,DataTable dtRegion,string[] FactorCode, string[] PointIds, string XName, string tooltip, string chartType, string pageType, string XLabelfomatter = "", IQueryable<ExcessiveSettingInfo> excessiveList = null, int pointid = -9999)
        {
          dv = dv.ToTable(true).DefaultView;//去除重复数据
          dv.Sort = XName + " ASC";
          if (pageType == "Air1")
          {
            pageType = "Air";
          }
          SmartEP.Core.Interfaces.IPollutant Ifactor = GetPollutantName(pageType, FactorCode[0]);
          string DecimalNum = Ifactor.PollutantDecimalNum;
          string factorName = Ifactor.PollutantName;
          StringBuilder sb = new StringBuilder();
          sb.Append("{chartType:" + "'" + chartType + "',");
          sb.Append("titleText:" + "'" + factorName + "',");

          //sb.Append(" xAxis: {type: 'datetime', dateTimeLabelFormats: { day: '%d日%H点'  } },");
          //修改by lvyun 2017-08-21
          string xAxis = "";
          if (!string.IsNullOrWhiteSpace(XLabelfomatter))
          {
            xAxis = " xAxisData: {type: 'datetime', labels: {step:2,formatter: function () {return Highcharts.dateFormat('" + XLabelfomatter + "',this.value);}} },";
          }
          else
          {
            xAxis = " xAxisData: {type: 'datetime', labels: {step:2,formatter: function () {return Highcharts.dateFormat('%d日%H点%M分',this.value);}} },";
          }
          sb.Append(xAxis);
          sb.Append(" yAxis: {title: { text: '" + (Ifactor.PollutantMeasureUnit == "mg/m3" ? "mg/m<sup>3</sup>" : Ifactor.PollutantMeasureUnit) + "' },  min: 0 },");
          if (!string.IsNullOrWhiteSpace(tooltip))
          {
            sb.Append(" tooltip: {formatter: function() {  return '<b>'+ this.series.name +'</b><br/>'+ Highcharts.dateFormat('" + tooltip + "', this.x) +':'+ this.y +' " + (Ifactor.PollutantMeasureUnit == "mg/m3" ? "mg/m<sup>3</sup>" : Ifactor.PollutantMeasureUnit) + "';    } },series: [");

          }
          else
          {
            sb.Append(" tooltip: {formatter: function() {  return '<b>'+ this.series.name +'</b><br/>'+ Highcharts.dateFormat('%d日%H点%M分', this.x) +':'+ this.y +' " + (Ifactor.PollutantMeasureUnit == "mg/m3" ? "mg/m<sup>3</sup>" : Ifactor.PollutantMeasureUnit) + "';    } },series: [");

          }
          string data = "";
        
            SmartEP.Core.Interfaces.IPollutant pollutant = GetPollutantName(pageType, FactorCode[0].ToString());
            //int pointTemp = int.TryParse(PointIds[i].ToString(), out pointTemp) ? pointTemp : 0;
            //string pointName = GetPointName("Air", pointTemp);
            //dv.RowFilter = "PointId='" + PointIds[i] + "'";
            if (dtRegion != null && dtRegion.Rows.Count > 0)
            {
              for (int i = 0; i < dtRegion.Rows.Count; i++)
              {
                string pointName = dtRegion.Rows[i]["Region"].ToString();
                dv.RowFilter = "PointId='" + pointName + "'";
                data += "{";
                data += string.Format(" name: '{0}',data:[ ", pointName);
                int m = 0;
                foreach (DataRowView row in dv)
                {
                 
                  DateTime tstamp = Convert.ToDateTime(row[XName]);
                  string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                  if (m != dv.Count)
                  {
                    if (row[FactorCode[0]] != DBNull.Value)
                    {
                      if (pollutant.PollutantMeasureUnit == "μg/m³")
                      {
                        data += "[" + time + "," + (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[FactorCode[0]]) * 1000, Convert.ToInt32(pollutant.PollutantDecimalNum) - 3).ToString()) + "],";
                      }
                      else
                      {
                        data += "[" + time + "," + (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[FactorCode[0]]), Convert.ToInt32(pollutant.PollutantDecimalNum)).ToString()) + "],";
                      }
                      //data += "[" + time + "," + (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[FactorCode[0]]), Convert.ToInt32(DecimalNum)).ToString()) + "],";
                    }
                    else
                    {
                      data += "[" + time + ",null],";
                    }
                  }
                  else
                  {
                    if (row[FactorCode[0]] != DBNull.Value)
                    {

                      data += "[" + time + "," + (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[FactorCode[0]]), Convert.ToInt32(DecimalNum)).ToString()) + "]";
                    }
                    else
                    {
                      data += "[" + time + ",null]";
                    }
                  }
                  m++;
                }
                data += "]},";
              }
            }
          sb.AppendFormat("{0}", data.Equals("") ? "" : data.Substring(0, data.Length - 1) + "");
          sb.Append("]  } ");
          return sb.ToString().Replace("\r\n", "");
        }
        /// <summary>
        /// 构造Chart JSon数据【单参数数据周、月、季、年】
        /// </summary>
        /// <param name="dv">数据源</param>
        /// <param name="FactorCode">因子编码</param>
        /// <param name="PointIds">站点ID</param>
        /// <param name="XName">X轴字段名</param>
        /// <param name="tooltip">Chart节点对应的js悬停样式</param>
        /// <param name="chartType">数据类型</param>
        /// <param name="pageType">应用程序类型</param>
        /// <returns></returns>
        public string GetChartDataMonthX(DataView dv, string[] FactorCode, string[] PointIds, string[] XName, string[] XLabel, string tooltip, string chartType, string pageType, IQueryable<ExcessiveSettingInfo> excessiveList = null, int pointid = -9999)
        {

            string factorName = "";
            dv = dv.ToTable(true).DefaultView;//去除重复数据
            if (pageType == "Air1")
            {
                pageType = "Air";
            }
            SmartEP.Core.Interfaces.IPollutant Ifactor = GetPollutantName(pageType, FactorCode[0]);
            string DecimalNum = Ifactor.PollutantDecimalNum;
            factorName = Ifactor.PollutantName;
            StringBuilder sb = new StringBuilder();
            string categories = "";
            sb.Append("{chartType:" + "'" + chartType + "',");
            sb.Append("titleText:" + "'" + factorName + "',");

            sb.Append(" tooltip: { valueSuffix: '" + (Ifactor.PollutantMeasureUnit == "mg/m3" ? "mg/m<sup>3</sup>" : Ifactor.PollutantMeasureUnit) + "' },series: [");
            string data = "";
            for (int i = 0; i < PointIds.Length; i++)
            {
                int pointTemp = int.TryParse(PointIds[i].ToString(), out pointTemp) ? pointTemp : 0;
                string pointName = GetPointName("Air", pointTemp);
                dv.RowFilter = "PointId='" + PointIds[i] + "'";
                data += "{";
                data += string.Format(" name: '{0}',data:[ ", pointName);
                int m = 0;
                foreach (DataRowView row in dv)
                {

                    #region categories
                    if (i == 0)
                    {
                        string str = "";
                        for (int j = 0; j < XName.Length; j++)
                        {
                            str += row[XName[j]].ToString() + XLabel[j];
                        }
                        categories += "'" + str + "',";
                    }
                    #endregion
                    if (m != dv.Count)
                    {
                        if (row[FactorCode[0]] != DBNull.Value)
                        {

                            data += (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[FactorCode[0]]), Convert.ToInt32(DecimalNum)).ToString()) + ",";
                        }
                        else
                        {
                            data += "null,";
                        }
                    }
                    else
                    {
                        if (row[FactorCode[0]] != DBNull.Value)
                        {

                            data += (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[FactorCode[0]]), Convert.ToInt32(DecimalNum)).ToString()) + ",";
                        }
                        else
                        {
                            data += "null,";
                        }
                    }
                    m++;
                }
                data += "]},";
            }
            sb.AppendFormat("{0}", data.Equals("") ? "" : data.Substring(0, data.Length - 1) + "");
            sb.Append("],");

            #region categories
            //sb.Append(" xAxis: {");
            if (!categories.Equals(""))
            {
                categories = categories.ToString().Substring(0, categories.ToString().Length - 1);//去除最后一个逗号
            }
            sb.Append("categories:[" + categories + "]");
            #endregion
            #region X、Y
            sb.Append(",xAxisData:{categories:[" + categories + "],labels:{rotation:-30}},");
            //sb.Append(",dateTimeLabelFormats: { month: '%Y-%m' }");
            //sb.Append("},");
            sb.Append(" yAxis: {title: { text: '" + (Ifactor.PollutantMeasureUnit == "mg/m3" ? "mg/m<sup>3</sup>" : Ifactor.PollutantMeasureUnit) + "' },  min: 0 }");
            #endregion
            sb.Append("} ");
            return sb.ToString().Replace("\r\n", "");
        }
        /// <summary>
        /// 构造Chart JSon数据【单参数数据周、月、季、年】
        /// </summary>
        /// <param name="dv">数据源</param>
        /// <param name="FactorCode">因子编码</param>
        /// <param name="PointIds">站点ID</param>
        /// <param name="XName">X轴字段名</param>
        /// <param name="tooltip">Chart节点对应的js悬停样式</param>
        /// <param name="chartType">数据类型</param>
        /// <param name="pageType">应用程序类型</param>
        /// <returns></returns>
        public string GetChartDataMonthXRegion(DataView dv, string[] FactorCode, DataTable dtRegion, string[] XName, string[] XLabel, string tooltip, string chartType, string pageType, IQueryable<ExcessiveSettingInfo> excessiveList = null, int pointid = -9999)
        {

          string factorName = "";
          dv = dv.ToTable(true).DefaultView;//去除重复数据
          if (pageType == "Air1")
          {
            pageType = "Air";
          }
          SmartEP.Core.Interfaces.IPollutant Ifactor = GetPollutantName(pageType, FactorCode[0]);
          string DecimalNum = Ifactor.PollutantDecimalNum;
          factorName = Ifactor.PollutantName;
          string unit = Ifactor.PollutantMeasureUnit;
          StringBuilder sb = new StringBuilder();
          string categories = "";
          sb.Append("{chartType:" + "'" + chartType + "',");
          sb.Append("titleText:" + "'" + factorName + "',");

          sb.Append(" tooltip: { valueSuffix: '" + (Ifactor.PollutantMeasureUnit == "mg/m3" ? "mg/m<sup>3</sup>" : Ifactor.PollutantMeasureUnit) + "' },series: [");
          string data = "";
          if (dtRegion != null && dtRegion.Rows.Count > 0)
          {
            for (int i = 0; i < dtRegion.Rows.Count; i++)
            {
              string pointName = dtRegion.Rows[i]["Region"].ToString();
              dv.RowFilter = "PointId='" + pointName + "'";
              data += "{";
              data += string.Format(" name: '{0}',data:[ ", pointName);
              int m = 0;
              foreach (DataRowView row in dv)
              {

                #region categories
                if (i == 0)
                {
                  string str = "";
                  for (int j = 0; j < XName.Length; j++)
                  {
                    str += row[XName[j]].ToString() + XLabel[j];
                  }
                  categories += "'" + str + "',";
                }
                #endregion
                if (m != dv.Count)
                {
                  if (row[FactorCode[0]] != DBNull.Value)
                  {
                    if (unit == "μg/m³")
                    {
                      data += (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[FactorCode[0]])*1000, Convert.ToInt32(DecimalNum)-3).ToString()) + ",";
                    }
                    else
                    {
                      data += (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[FactorCode[0]]), Convert.ToInt32(DecimalNum)).ToString()) + ",";
                    }
                  }
                  else
                  {
                    data += "null,";
                  }
                }
                else
                {
                  if (row[FactorCode[0]] != DBNull.Value)
                  {

                    data += (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[FactorCode[0]]), Convert.ToInt32(DecimalNum)).ToString()) + ",";
                  }
                  else
                  {
                    data += "null,";
                  }
                }
                m++;
              }
              data += "]},";
            }
          }
          sb.AppendFormat("{0}", data.Equals("") ? "" : data.Substring(0, data.Length - 1) + "");
          sb.Append("],");

          #region categories
          //sb.Append(" xAxis: {");
          if (!categories.Equals(""))
          {
            categories = categories.ToString().Substring(0, categories.ToString().Length - 1);//去除最后一个逗号
          }
          sb.Append("categories:[" + categories + "]");
          #endregion
          #region X、Y
          sb.Append(",xAxisData:{categories:[" + categories + "],labels:{rotation:-30}},");
          //sb.Append(",dateTimeLabelFormats: { month: '%Y-%m' }");
          //sb.Append("},");
          sb.Append(" yAxis: {title: { text: '" + (Ifactor.PollutantMeasureUnit == "mg/m3" ? "mg/m<sup>3</sup>" : Ifactor.PollutantMeasureUnit) + "' },  min: 0 }");
          #endregion
          sb.Append("} ");
          return sb.ToString().Replace("\r\n", "");
        }
        /// <summary>
        /// 构造Chart JSon数据【单参数数据月】修改by lvyun 2017-08-22
        /// </summary>
        /// <param name="dv"></param>
        /// <param name="FactorCode"></param>
        /// <param name="PointIds"></param>
        /// <param name="XName"></param>
        /// <param name="XLabel"></param>
        /// <param name="tooltip"></param>
        /// <param name="chartType"></param>
        /// <param name="pageType"></param>
        /// <param name="excessiveList"></param>
        /// <param name="pointid"></param>
        /// <returns></returns>
        public string GetChartDataMonthXNT(DataView dv, string[] FactorCode, string[] PointIds, string[] XName, string[] XLabel, string tooltip, string chartType, string pageType, IQueryable<ExcessiveSettingInfo> excessiveList = null, int pointid = -9999)
        {

            string factorName = "";
            dv = dv.ToTable(true).DefaultView;//去除重复数据
            if (pageType == "Air1")
            {
                pageType = "Air";
            }
            SmartEP.Core.Interfaces.IPollutant Ifactor = GetPollutantName(pageType, FactorCode[0]);
            string DecimalNum = Ifactor.PollutantDecimalNum;
            factorName = Ifactor.PollutantName;
            StringBuilder sb = new StringBuilder();
            string categories = "";
            sb.Append("{chartType:" + "'" + chartType + "',");
            sb.Append("titleText:" + "'" + factorName + "',");

            sb.Append(" tooltip: { valueSuffix: '" + (Ifactor.PollutantMeasureUnit == "mg/m3" ? "mg/m<sup>3</sup>" : Ifactor.PollutantMeasureUnit) + "' },series: [");
            string data = "";
            for (int i = 0; i < PointIds.Length; i++)
            {
                int pointTemp = int.TryParse(PointIds[i].ToString(), out pointTemp) ? pointTemp : 0;
                string pointName = GetPointName("Air", pointTemp);
                dv.RowFilter = "PointId='" + PointIds[i] + "'";
                data += "{";
                data += string.Format(" name: '{0}',data:[ ", pointName);
                int m = 0;
                foreach (DataRowView row in dv)
                {

                    #region categories
                    if (i == 0)
                    {
                        string str = "";
                        for (int j = 0; j < XName.Length; j++)
                        {
                            str += row[XName[j]].ToString() + XLabel[j];
                        }
                        categories += "'" + str + "',";
                    }
                    #endregion
                    if (m != dv.Count)
                    {
                        if (row[FactorCode[0]] != DBNull.Value)
                        {

                            data += (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[FactorCode[0]]), Convert.ToInt32(DecimalNum)).ToString()) + ",";
                        }
                        else
                        {
                            data += "null,";
                        }
                    }
                    else
                    {
                        if (row[FactorCode[0]] != DBNull.Value)
                        {

                            data += (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[FactorCode[0]]), Convert.ToInt32(DecimalNum)).ToString()) + ",";
                        }
                        else
                        {
                            data += "null,";
                        }
                    }
                    m++;
                }
                data += "]},";
            }
            sb.AppendFormat("{0}", data.Equals("") ? "" : data.Substring(0, data.Length - 1) + "");
            sb.Append("],");

            #region categories
            //sb.Append(" xAxis: {");
            if (!categories.Equals(""))
            {
                categories = categories.ToString().Substring(0, categories.ToString().Length - 1);//去除最后一个逗号
                categories = categories.Replace("年", "/");//修改时间格式
                categories = categories.Replace("月", "");
            }
            sb.Append("categories:[" + categories + "]");
            #endregion
            #region X、Y
            sb.Append(",xAxisData:{categories:[" + categories + "],labels:{rotation:-30}},");
            //sb.Append(",dateTimeLabelFormats: { month: '%Y-%m' }");
            //sb.Append("},");
            sb.Append(" yAxis: {title: { text: '" + (Ifactor.PollutantMeasureUnit == "mg/m3" ? "mg/m<sup>3</sup>" : Ifactor.PollutantMeasureUnit) + "' },  min: 0 }");
            #endregion
            sb.Append("} ");
            return sb.ToString().Replace("\r\n", "");
        }
        /// <summary>
        /// 构造Chart JSon数据【单参数数据月】修改by lvyun 2017-08-22
        /// </summary>
        /// <param name="dv"></param>
        /// <param name="FactorCode"></param>
        /// <param name="PointIds"></param>
        /// <param name="XName"></param>
        /// <param name="XLabel"></param>
        /// <param name="tooltip"></param>
        /// <param name="chartType"></param>
        /// <param name="pageType"></param>
        /// <param name="excessiveList"></param>
        /// <param name="pointid"></param>
        /// <returns></returns>
        public string GetChartDataMonthXNTRegion(DataView dv, string[] FactorCode, DataTable dtRegion, string[] XName, string[] XLabel, string tooltip, string chartType, string pageType, IQueryable<ExcessiveSettingInfo> excessiveList = null, int pointid = -9999)
        {

          string factorName = "";
          dv = dv.ToTable(true).DefaultView;//去除重复数据
          if (pageType == "Air1")
          {
            pageType = "Air";
          }
          SmartEP.Core.Interfaces.IPollutant Ifactor = GetPollutantName(pageType, FactorCode[0]);
          string DecimalNum = Ifactor.PollutantDecimalNum;
          factorName = Ifactor.PollutantName;
          string unit=Ifactor.PollutantMeasureUnit;
          StringBuilder sb = new StringBuilder();
          string categories = "";
          sb.Append("{chartType:" + "'" + chartType + "',");
          sb.Append("titleText:" + "'" + factorName + "',");

          sb.Append(" tooltip: { valueSuffix: '" + (Ifactor.PollutantMeasureUnit == "mg/m3" ? "mg/m<sup>3</sup>" : Ifactor.PollutantMeasureUnit) + "' },series: [");
          string data = "";
          if (dtRegion != null && dtRegion.Rows.Count > 0)
          {
            for (int i = 0; i < dtRegion.Rows.Count; i++)
            {
              string pointName = dtRegion.Rows[i]["Region"].ToString();
              dv.RowFilter = "PointId='" + pointName + "'";
              data += "{";
              data += string.Format(" name: '{0}',data:[ ", pointName);
              int m = 0;
              foreach (DataRowView row in dv)
              {

                #region categories
                if (i == 0)
                {
                  string str = "";
                  for (int j = 0; j < XName.Length; j++)
                  {
                    str += row[XName[j]].ToString() + XLabel[j];
                  }
                  categories += "'" + str + "',";
                }
                #endregion
                if (m != dv.Count)
                {
                  if (row[FactorCode[0]] != DBNull.Value)
                  {
                    if (unit == "μg/m³")
                    {
                      data += (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[FactorCode[0]])*1000, Convert.ToInt32(DecimalNum)-3).ToString()) + ",";
                    }
                    else
                    {
                      data += (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[FactorCode[0]]), Convert.ToInt32(DecimalNum)).ToString()) + ",";
                    }
                  }
                  else
                  {
                    data += "null,";
                  }
                }
                else
                {
                  if (row[FactorCode[0]] != DBNull.Value)
                  {

                    data += (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[FactorCode[0]]), Convert.ToInt32(DecimalNum)).ToString()) + ",";
                  }
                  else
                  {
                    data += "null,";
                  }
                }
                m++;
              }
              data += "]},";
            }
          }
          sb.AppendFormat("{0}", data.Equals("") ? "" : data.Substring(0, data.Length - 1) + "");
          sb.Append("],");

          #region categories
          //sb.Append(" xAxis: {");
          if (!categories.Equals(""))
          {
            categories = categories.ToString().Substring(0, categories.ToString().Length - 1);//去除最后一个逗号
            categories = categories.Replace("年", "/");//修改时间格式
            categories = categories.Replace("月", "");
          }
          sb.Append("categories:[" + categories + "]");
          #endregion
          #region X、Y
          sb.Append(",xAxisData:{categories:[" + categories + "],labels:{rotation:-30}},");
          //sb.Append(",dateTimeLabelFormats: { month: '%Y-%m' }");
          //sb.Append("},");
          sb.Append(" yAxis: {title: { text: '" + (Ifactor.PollutantMeasureUnit == "mg/m3" ? "mg/m<sup>3</sup>" : Ifactor.PollutantMeasureUnit) + "' },  min: 0 }");
          #endregion
          sb.Append("} ");
          return sb.ToString().Replace("\r\n", "");
        }

        /// <summary>
        /// 构造Chart JSon数据【周、月、季、年】
        /// </summary>
        /// <param name="dv">数据源</param>
        /// <param name="FactorCode">因子编码</param>
        /// <param name="XName">X轴字段名</param>
        /// <param name="tooltip">Chart节点对应的js悬停样式</param>
        /// <param name="chartType">数据类型</param>
        /// <param name="pageType">应用程序类型</param>
        /// <returns></returns>
        public string GetChartDataMonth(DataView dv, string[] FactorCode, string[] XName, string[] XLabel, string tooltip, string chartType, string pageType, IQueryable<ExcessiveSettingInfo> excessiveList = null, int pointid = -9999, string name = "")
        {
            dv = dv.ToTable(true).DefaultView;//去除重复数据
            StringBuilder sb = new StringBuilder();
            string categories = "";
            List<SmartEP.Core.Interfaces.IPollutant> pollutantList = new List<SmartEP.Core.Interfaces.IPollutant>(); ;
            decimal max = -1;
            string plotLines = "";
            sb.Append("{");
            string pt = string.Empty;
            if (pageType == "Air1")
            {
                pageType = "Air";
                pt = "Air1";
            }
            #region series
            sb.Append("series:[");
            for (int i = 0; i < FactorCode.Length; i++)
            {
                string factor = FactorCode[i];
                SmartEP.Core.Interfaces.IPollutant pollutant = GetPollutantName(pageType, factor);
                string factorName = pollutant.PollutantName;
                //用于显示多Y轴
                if (pollutantList.Count == 0 || !pollutantList.Where(x => !x.PollutantCode.Equals(factorY)).Select(x => x.PollutantMeasureUnit).ToArray().Contains(pollutant.PollutantMeasureUnit) || factor.Equals(factorY))
                    pollutantList.Add(pollutant);
                //if (!unit.Contains(pollutant.PollutantMeasureUnit)) unit += i == 0 ? pollutant.PollutantMeasureUnit : ";" + pollutant.PollutantMeasureUnit;
                sb.Append("{");
                #region 对应Y轴
                int ynum = -1;
                if (factor.Equals(factorY))
                    ynum = pollutantList.IndexOf(pollutant);
                else
                    ynum = pollutantList.IndexOf(pollutantList.Where(x => !x.PollutantCode.Equals(factorY) && x.PollutantMeasureUnit == pollutant.PollutantMeasureUnit).FirstOrDefault());
                sb.AppendFormat("yAxis: {0},", ynum < 0 ? 0 : ynum);//Name属性 
                #endregion

                #region Name
                sb.AppendFormat("name:'{0}',", factorName);//Name属性
                #endregion

                #region Data
                string data = "";
                foreach (DataRowView row in dv)
                {
                    #region categories
                    if (i == 0)
                    {
                        string str = "";
                        for (int j = 0; j < XName.Length; j++)
                        {
                            str += row[XName[j]].ToString() + XLabel[j];
                        }
                        categories += "'" + str + "',";
                    }
                    #endregion

                    #region X、Y值
                    if (row[factor] != DBNull.Value)
                    {
                        if (pollutant.PollutantMeasureUnit == "μg/m³")
                        {
                            data += (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[factor]) * 1000, 0).ToString()) + ",";
                        }
                        else
                        {
                            data += (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[factor]), Convert.ToInt32(pollutant.PollutantDecimalNum)).ToString()) + ",";
                        }
                        //data += (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[factor]), Convert.ToInt32(pollutant.PollutantDecimalNum)).ToString()) + ",";
                    }
                    else
                    {
                        data += "null,";
                    }
                    #endregion
                }
                sb.AppendFormat("data:[{0}]", data.Equals("") ? "" : data.Substring(0, data.Length - 1) + "");
                #endregion

                #region 是否显示
                DataTable dt = dv.ToTable();
                sb.AppendFormat(",visible:{0}", Convert.ToInt32(dt.Compute("Count(" + factor + ")", "")) > 0 ? "true" : "false");
                #endregion

                #region ToolTip
                sb.Append(",tooltip: { valueSuffix: '" + (pollutant.PollutantMeasureUnit == "mg/m3" ? "mg/m<sup>3</sup>" : pollutant.PollutantMeasureUnit) + "' }");//Name属性                   
                #endregion
                sb.Append("},");
            }
            if (sb.ToString().Substring(sb.ToString().Length - 1, 1).Trim().Equals(","))
                sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));//去除最后一个逗号
            sb.Append("]");
            #endregion

            #region title
            if (pt == "Air1")
            {
                if (pointid != -9999) sb.Append(",titleText:" + "'" + GetPointName(pageType, pointid) + name + "'");
                //if (pointid != -9999) sb.Append(",titleText:" + "'" + name + "'");
                else if (dv.Count > 0 && dv.ToTable().Columns.Contains("portName") && dv[0]["portName"] != DBNull.Value)
                    //sb.Append(",titleText:" + "'" + dv[0]["portName"] + name + "' ");
                    sb.Append(",titleText:" + "'" + name + "' ");
                else
                    sb.Append(",titleText:" + "''");
            }
            else
            {
                //if (pointid != -9999) sb.Append(",titleText:" + "'" + GetPointName(pageType, pointid) + name + "'");
                if (pointid != -9999) sb.Append(",titleText:" + "'" + name + "'");
                else if (dv.Count > 0 && dv.ToTable().Columns.Contains("portName") && dv[0]["portName"] != DBNull.Value)
                    //sb.Append(",titleText:" + "'" + dv[0]["portName"] + name + "' ");
                    sb.Append(",titleText:" + "'" + name + "' ");
                else
                    sb.Append(",titleText:" + "''");
            }


            #endregion

            #region tooltip
            sb.Append(",tooltip:{" + tooltip + "}");
            #endregion

            #region categories
            if (!categories.Equals(""))
            {
                categories = categories.ToString().Substring(0, categories.ToString().Length - 1);//去除最后一个逗号
            }
            sb.Append(",categories:[" + categories + "]");
            #endregion

            #region xAxis
            sb.Append(",xAxisData:{categories:[" + categories + "],labels:{rotation:-30}}");
            //sb.Append(",xAxisData:{type:'datetime',labels:{formatter:formatterMonthData}}");
            #endregion

            #region YAxis
            sb.Append(",yAxis:[");
            if (pollutantList != null)
            {
                int num = 0;
                foreach (SmartEP.Core.Interfaces.IPollutant poll in pollutantList)
                {
                    sb.Append("{");
                    #region title属性
                    sb.Append("title:{");
                    sb.AppendFormat("text: \"{0}\"", poll.PollutantMeasureUnit == "mg/m3" ? "mg/m<sup>3</sup>" : poll.PollutantMeasureUnit);
                    sb.Append("}");
                    #endregion
                    #region 对数轴
                    //sb.Append(",endOnTick: false,type: 'logarithmic'");
                    #endregion
                    #region 超标等级线
                    plotLines = "";
                    if (excessiveList != null)
                    {
                        max = dv.ToTable().Compute("Max(" + poll.PollutantCode + ")", "") != DBNull.Value ? Convert.ToDecimal(dv.ToTable().Compute("Max(" + poll.PollutantCode + ")", "")) : -1;
                        foreach (ExcessiveSettingInfo excess in excessiveList)
                        {
                            if (excess.PollutantGuid.Equals(poll.PollutantGuid))
                            {
                                max = max == -1 || max < excess.excessiveUpper ? excess.excessiveUpper : max;
                                #region 下限
                                //plotLines += "{";
                                //plotLines += string.Format("value:{0}", excess.excessiveLow);
                                //plotLines += string.Format(",color:\"{0}\"", "red");
                                //plotLines += string.Format(",width:{0}", 2);
                                //plotLines += string.Format(",dashStyle:'{0}'", "shortdot");
                                //#region Label属性
                                //plotLines += ",label: {";
                                //plotLines += string.Format("text:\"{0}\"", "超下限");
                                //plotLines += string.Format(",align:\"{0}\"", "left");
                                //plotLines += "}";
                                //#endregion
                                //plotLines += "},";
                                #endregion

                                #region 上限
                                plotLines += "{";
                                plotLines += string.Format("value:{0}", excess.excessiveUpper);
                                plotLines += string.Format(",color:\"{0}\"", "red");
                                plotLines += string.Format(",width:{0}", 2);
                                plotLines += string.Format(",dashStyle:'{0}'", "shortdot");
                                #region Label属性
                                plotLines += ",label: {";
                                plotLines += string.Format("text:\"{0}\"", "超标限(" + excess.excessiveUpper + ")");
                                plotLines += string.Format(",align:\"{0}\"", "left");
                                plotLines += "}";
                                #endregion
                                plotLines += "},";
                                #endregion
                            }
                        }
                        #region 等级线
                        sb.Append(",plotLines: [" + (!plotLines.Equals("") ? plotLines.Substring(0, plotLines.Length - 1) : "") + "]");
                        #endregion
                        #region 最大值、最小值
                        sb.Append(",min:0");
                        if (max != -1)
                            sb.Append(",max:" + max);
                        #endregion
                    }
                    #endregion

                    #region Y轴左右位置
                    if (num % 2 == 0)
                        sb.Append(",opposite: false");
                    else
                        sb.Append(",opposite: true");
                    num++;
                    #endregion

                    //#region Y轴线条
                    //sb.Append(",lineWidth: 1");
                    //#endregion
                    sb.Append("},");
                }
                if (sb.ToString().Substring(sb.ToString().Length - 1, 1).Trim().Equals(","))
                    sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));//去除最后一个逗号
            }
            else
            {
                sb.Append("{title: {text: '浓度'}}");
            }
            sb.Append("]");
            #endregion

            #region 图表type
            if (!chartType.Equals("")) sb.AppendFormat(",chartType:'{0}'", chartType);
            #endregion

            sb.Append("}");

            return sb.ToString().Replace("\r\n", "");
        }

        /// <summary>
        /// 构造Chart JSon数据【周、月、季、年】
        /// </summary>
        /// <param name="dv">数据源</param>
        /// <param name="FactorCode">因子编码</param>
        /// <param name="XName">X轴字段名</param>
        /// <param name="tooltip">Chart节点对应的js悬停样式</param>
        /// <param name="chartType">数据类型</param>
        /// <param name="pageType">应用程序类型</param>
        /// <returns></returns>
        public string GetChartDataMonthRegion(DataView dv, string[] FactorCode, string[] XName, string[] XLabel, string tooltip, string chartType, string pageType, IQueryable<ExcessiveSettingInfo> excessiveList = null, string region="", string name = "")
        {
          dv = dv.ToTable(true).DefaultView;//去除重复数据
          StringBuilder sb = new StringBuilder();
          string categories = "";
          List<SmartEP.Core.Interfaces.IPollutant> pollutantList = new List<SmartEP.Core.Interfaces.IPollutant>(); ;
          decimal max = -1;
          string plotLines = "";
          sb.Append("{");
          string pt = string.Empty;
          if (pageType == "Air1")
          {
            pageType = "Air";
            pt = "Air1";
          }
          #region series
          sb.Append("series:[");
          for (int i = 0; i < FactorCode.Length; i++)
          {
            string factor = FactorCode[i];
            SmartEP.Core.Interfaces.IPollutant pollutant = GetPollutantName(pageType, factor);
            string factorName = pollutant.PollutantName;
            //用于显示多Y轴
            if (pollutantList.Count == 0 || !pollutantList.Where(x => !x.PollutantCode.Equals(factorY)).Select(x => x.PollutantMeasureUnit).ToArray().Contains(pollutant.PollutantMeasureUnit) || factor.Equals(factorY))
              pollutantList.Add(pollutant);
            //if (!unit.Contains(pollutant.PollutantMeasureUnit)) unit += i == 0 ? pollutant.PollutantMeasureUnit : ";" + pollutant.PollutantMeasureUnit;
            sb.Append("{");
            #region 对应Y轴
            int ynum = -1;
            if (factor.Equals(factorY))
              ynum = pollutantList.IndexOf(pollutant);
            else
              ynum = pollutantList.IndexOf(pollutantList.Where(x => !x.PollutantCode.Equals(factorY) && x.PollutantMeasureUnit == pollutant.PollutantMeasureUnit).FirstOrDefault());
            sb.AppendFormat("yAxis: {0},", ynum < 0 ? 0 : ynum);//Name属性 
            #endregion

            #region Name
            sb.AppendFormat("name:'{0}',", factorName);//Name属性
            #endregion

            #region Data
            string data = "";
            foreach (DataRowView row in dv)
            {
              #region categories
              if (i == 0)
              {
                string str = "";
                for (int j = 0; j < XName.Length; j++)
                {
                  str += row[XName[j]].ToString() + XLabel[j];
                }
                categories += "'" + str + "',";
              }
              #endregion

              #region X、Y值
              if (row[factor] != DBNull.Value)
              {
                if (pollutant.PollutantMeasureUnit == "μg/m³")
                {
                  data += (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[factor]) * 1000, 0).ToString()) + ",";
                }
                else
                {
                  data += (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[factor]), Convert.ToInt32(pollutant.PollutantDecimalNum)).ToString()) + ",";
                }
                //data += (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[factor]), Convert.ToInt32(pollutant.PollutantDecimalNum)).ToString()) + ",";
              }
              else
              {
                data += "null,";
              }
              #endregion
            }
            sb.AppendFormat("data:[{0}]", data.Equals("") ? "" : data.Substring(0, data.Length - 1) + "");
            #endregion

            #region 是否显示
            DataTable dt = dv.ToTable();
            sb.AppendFormat(",visible:{0}", Convert.ToInt32(dt.Compute("Count(" + factor + ")", "")) > 0 ? "true" : "false");
            #endregion

            #region ToolTip
            sb.Append(",tooltip: { valueSuffix: '" + (pollutant.PollutantMeasureUnit == "mg/m3" ? "mg/m<sup>3</sup>" : pollutant.PollutantMeasureUnit) + "' }");//Name属性                   
            #endregion
            sb.Append("},");
          }
          if (sb.ToString().Substring(sb.ToString().Length - 1, 1).Trim().Equals(","))
            sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));//去除最后一个逗号
          sb.Append("]");
          #endregion

          #region title
          if (pt == "Air1")
          {
            sb.Append(",titleText:" + "'" + region + "'");

          }
          else
          {
            sb.Append(",titleText:" + "'" + region + "'");
          }


          #endregion

          #region tooltip
          sb.Append(",tooltip:{" + tooltip + "}");
          #endregion

          #region categories
          if (!categories.Equals(""))
          {
            categories = categories.ToString().Substring(0, categories.ToString().Length - 1);//去除最后一个逗号
          }
          sb.Append(",categories:[" + categories + "]");
          #endregion

          #region xAxis
          sb.Append(",xAxisData:{categories:[" + categories + "],labels:{rotation:-30}}");
          //sb.Append(",xAxisData:{type:'datetime',labels:{formatter:formatterMonthData}}");
          #endregion

          #region YAxis
          sb.Append(",yAxis:[");
          if (pollutantList != null)
          {
            int num = 0;
            foreach (SmartEP.Core.Interfaces.IPollutant poll in pollutantList)
            {
              sb.Append("{");
              #region title属性
              sb.Append("title:{");
              sb.AppendFormat("text: \"{0}\"", poll.PollutantMeasureUnit == "mg/m3" ? "mg/m<sup>3</sup>" : poll.PollutantMeasureUnit);
              sb.Append("}");
              #endregion
              #region 对数轴
              //sb.Append(",endOnTick: false,type: 'logarithmic'");
              #endregion
              #region 超标等级线
              plotLines = "";
              if (excessiveList != null)
              {
                max = dv.ToTable().Compute("Max(" + poll.PollutantCode + ")", "") != DBNull.Value ? Convert.ToDecimal(dv.ToTable().Compute("Max(" + poll.PollutantCode + ")", "")) : -1;
                foreach (ExcessiveSettingInfo excess in excessiveList)
                {
                  if (excess.PollutantGuid.Equals(poll.PollutantGuid))
                  {
                    max = max == -1 || max < excess.excessiveUpper ? excess.excessiveUpper : max;
                    #region 下限
                    //plotLines += "{";
                    //plotLines += string.Format("value:{0}", excess.excessiveLow);
                    //plotLines += string.Format(",color:\"{0}\"", "red");
                    //plotLines += string.Format(",width:{0}", 2);
                    //plotLines += string.Format(",dashStyle:'{0}'", "shortdot");
                    //#region Label属性
                    //plotLines += ",label: {";
                    //plotLines += string.Format("text:\"{0}\"", "超下限");
                    //plotLines += string.Format(",align:\"{0}\"", "left");
                    //plotLines += "}";
                    //#endregion
                    //plotLines += "},";
                    #endregion

                    #region 上限
                    plotLines += "{";
                    plotLines += string.Format("value:{0}", excess.excessiveUpper);
                    plotLines += string.Format(",color:\"{0}\"", "red");
                    plotLines += string.Format(",width:{0}", 2);
                    plotLines += string.Format(",dashStyle:'{0}'", "shortdot");
                    #region Label属性
                    plotLines += ",label: {";
                    plotLines += string.Format("text:\"{0}\"", "超标限(" + excess.excessiveUpper + ")");
                    plotLines += string.Format(",align:\"{0}\"", "left");
                    plotLines += "}";
                    #endregion
                    plotLines += "},";
                    #endregion
                  }
                }
                #region 等级线
                sb.Append(",plotLines: [" + (!plotLines.Equals("") ? plotLines.Substring(0, plotLines.Length - 1) : "") + "]");
                #endregion
                #region 最大值、最小值
                sb.Append(",min:0");
                if (max != -1)
                  sb.Append(",max:" + max);
                #endregion
              }
              #endregion

              #region Y轴左右位置
              if (num % 2 == 0)
                sb.Append(",opposite: false");
              else
                sb.Append(",opposite: true");
              num++;
              #endregion

              //#region Y轴线条
              //sb.Append(",lineWidth: 1");
              //#endregion
              sb.Append("},");
            }
            if (sb.ToString().Substring(sb.ToString().Length - 1, 1).Trim().Equals(","))
              sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));//去除最后一个逗号
          }
          else
          {
            sb.Append("{title: {text: '浓度'}}");
          }
          sb.Append("]");
          #endregion

          #region 图表type
          if (!chartType.Equals("")) sb.AppendFormat(",chartType:'{0}'", chartType);
          #endregion

          sb.Append("}");

          return sb.ToString().Replace("\r\n", "");
        }
        /// <summary>
        /// 修改by lvyun 2017-08-22
        /// </summary>
        /// <param name="dv"></param>
        /// <param name="FactorCode"></param>
        /// <param name="XName"></param>
        /// <param name="XLabel"></param>
        /// <param name="tooltip"></param>
        /// <param name="chartType"></param>
        /// <param name="pageType"></param>
        /// <param name="excessiveList"></param>
        /// <param name="pointid"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetChartDataMonthNT(DataView dv, string[] FactorCode, string[] XName, string[] XLabel, string tooltip, string chartType, string pageType, IQueryable<ExcessiveSettingInfo> excessiveList = null, int pointid = -9999, string name = "")
        {
            dv = dv.ToTable(true).DefaultView;//去除重复数据
            StringBuilder sb = new StringBuilder();
            string categories = "";
            List<SmartEP.Core.Interfaces.IPollutant> pollutantList = new List<SmartEP.Core.Interfaces.IPollutant>(); ;
            decimal max = -1;
            string plotLines = "";
            sb.Append("{");
            string pt = string.Empty;
            if (pageType == "Air1")
            {
                pageType = "Air";
                pt = "Air1";
            }
            #region series
            sb.Append("series:[");
            for (int i = 0; i < FactorCode.Length; i++)
            {
                string factor = FactorCode[i];
                SmartEP.Core.Interfaces.IPollutant pollutant = GetPollutantName(pageType, factor);
                string factorName = pollutant.PollutantName;
                //用于显示多Y轴
                if (pollutantList.Count == 0 || !pollutantList.Where(x => !x.PollutantCode.Equals(factorY)).Select(x => x.PollutantMeasureUnit).ToArray().Contains(pollutant.PollutantMeasureUnit) || factor.Equals(factorY))
                    pollutantList.Add(pollutant);
                //if (!unit.Contains(pollutant.PollutantMeasureUnit)) unit += i == 0 ? pollutant.PollutantMeasureUnit : ";" + pollutant.PollutantMeasureUnit;
                sb.Append("{");
                #region 对应Y轴
                int ynum = -1;
                if (factor.Equals(factorY))
                    ynum = pollutantList.IndexOf(pollutant);
                else
                    ynum = pollutantList.IndexOf(pollutantList.Where(x => !x.PollutantCode.Equals(factorY) && x.PollutantMeasureUnit == pollutant.PollutantMeasureUnit).FirstOrDefault());
                sb.AppendFormat("yAxis: {0},", ynum < 0 ? 0 : ynum);//Name属性 
                #endregion

                #region Name
                sb.AppendFormat("name:'{0}',", factorName);//Name属性
                #endregion

                #region Data
                string data = "";
                foreach (DataRowView row in dv)
                {
                    #region categories
                    if (i == 0)
                    {
                        string str = "";
                        for (int j = 0; j < XName.Length; j++)
                        {
                            str += row[XName[j]].ToString() + XLabel[j];
                        }
                        categories += "'" + str + "',";
                    }
                    #endregion

                    #region X、Y值
                    if (row[factor] != DBNull.Value)
                    {
                        if (pollutant.PollutantMeasureUnit == "μg/m³")
                        {
                            data += (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[factor]) * 1000, 0).ToString()) + ",";
                        }
                        else
                        {
                            data += (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[factor]), Convert.ToInt32(pollutant.PollutantDecimalNum)).ToString()) + ",";
                        }
                        //data += (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[factor]), Convert.ToInt32(pollutant.PollutantDecimalNum)).ToString()) + ",";
                    }
                    else
                    {
                        data += "null,";
                    }
                    #endregion
                }
                sb.AppendFormat("data:[{0}]", data.Equals("") ? "" : data.Substring(0, data.Length - 1) + "");
                #endregion

                #region 是否显示
                DataTable dt = dv.ToTable();
                sb.AppendFormat(",visible:{0}", Convert.ToInt32(dt.Compute("Count(" + factor + ")", "")) > 0 ? "true" : "false");
                #endregion

                #region ToolTip
                sb.Append(",tooltip: { valueSuffix: '" + (pollutant.PollutantMeasureUnit == "mg/m3" ? "mg/m<sup>3</sup>" : pollutant.PollutantMeasureUnit) + "' }");//Name属性                   
                #endregion
                sb.Append("},");
            }
            if (sb.ToString().Substring(sb.ToString().Length - 1, 1).Trim().Equals(","))
                sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));//去除最后一个逗号
            sb.Append("]");
            #endregion

            #region title
            if (pt == "Air1")
            {
                if (pointid != -9999) sb.Append(",titleText:" + "'" + GetPointName(pageType, pointid) + name + "'");
                //if (pointid != -9999) sb.Append(",titleText:" + "'" + name + "'");
                else if (dv.Count > 0 && dv.ToTable().Columns.Contains("portName") && dv[0]["portName"] != DBNull.Value)
                    //sb.Append(",titleText:" + "'" + dv[0]["portName"] + name + "' ");
                    sb.Append(",titleText:" + "'" + name + "' ");
                else
                    sb.Append(",titleText:" + "''");
            }
            else
            {
                //if (pointid != -9999) sb.Append(",titleText:" + "'" + GetPointName(pageType, pointid) + name + "'");
                if (pointid != -9999) sb.Append(",titleText:" + "'" + name + "'");
                else if (dv.Count > 0 && dv.ToTable().Columns.Contains("portName") && dv[0]["portName"] != DBNull.Value)
                    //sb.Append(",titleText:" + "'" + dv[0]["portName"] + name + "' ");
                    sb.Append(",titleText:" + "'" + name + "' ");
                else
                    sb.Append(",titleText:" + "''");
            }


            #endregion

            #region tooltip
            sb.Append(",tooltip:{" + tooltip + "}");
            #endregion

            #region categories
            if (!categories.Equals(""))
            {
                categories = categories.ToString().Substring(0, categories.ToString().Length - 1);//去除最后一个逗号
                categories = categories.Replace("年", "/");//修改时间格式
                categories = categories.Replace("月", "");
            }
            sb.Append(",categories:[" + categories + "]");
            #endregion

            #region xAxis
            sb.Append(",xAxisData:{categories:[" + categories + "],labels:{rotation:-30}}");
            //sb.Append(",xAxisData:{type:'datetime',labels:{formatter:formatterMonthData}}");
            #endregion

            #region YAxis
            sb.Append(",yAxis:[");
            if (pollutantList != null)
            {
                int num = 0;
                foreach (SmartEP.Core.Interfaces.IPollutant poll in pollutantList)
                {
                    sb.Append("{");
                    #region title属性
                    sb.Append("title:{");
                    sb.AppendFormat("text: \"{0}\"", poll.PollutantMeasureUnit == "mg/m3" ? "mg/m<sup>3</sup>" : poll.PollutantMeasureUnit);
                    sb.Append("}");
                    #endregion
                    #region 对数轴
                    //sb.Append(",endOnTick: false,type: 'logarithmic'");
                    #endregion
                    #region 超标等级线
                    plotLines = "";
                    if (excessiveList != null)
                    {
                        max = dv.ToTable().Compute("Max(" + poll.PollutantCode + ")", "") != DBNull.Value ? Convert.ToDecimal(dv.ToTable().Compute("Max(" + poll.PollutantCode + ")", "")) : -1;
                        foreach (ExcessiveSettingInfo excess in excessiveList)
                        {
                            if (excess.PollutantGuid.Equals(poll.PollutantGuid))
                            {
                                max = max == -1 || max < excess.excessiveUpper ? excess.excessiveUpper : max;
                                #region 下限
                                //plotLines += "{";
                                //plotLines += string.Format("value:{0}", excess.excessiveLow);
                                //plotLines += string.Format(",color:\"{0}\"", "red");
                                //plotLines += string.Format(",width:{0}", 2);
                                //plotLines += string.Format(",dashStyle:'{0}'", "shortdot");
                                //#region Label属性
                                //plotLines += ",label: {";
                                //plotLines += string.Format("text:\"{0}\"", "超下限");
                                //plotLines += string.Format(",align:\"{0}\"", "left");
                                //plotLines += "}";
                                //#endregion
                                //plotLines += "},";
                                #endregion

                                #region 上限
                                plotLines += "{";
                                plotLines += string.Format("value:{0}", excess.excessiveUpper);
                                plotLines += string.Format(",color:\"{0}\"", "red");
                                plotLines += string.Format(",width:{0}", 2);
                                plotLines += string.Format(",dashStyle:'{0}'", "shortdot");
                                #region Label属性
                                plotLines += ",label: {";
                                plotLines += string.Format("text:\"{0}\"", "超标限(" + excess.excessiveUpper + ")");
                                plotLines += string.Format(",align:\"{0}\"", "left");
                                plotLines += "}";
                                #endregion
                                plotLines += "},";
                                #endregion
                            }
                        }
                        #region 等级线
                        sb.Append(",plotLines: [" + (!plotLines.Equals("") ? plotLines.Substring(0, plotLines.Length - 1) : "") + "]");
                        #endregion
                        #region 最大值、最小值
                        sb.Append(",min:0");
                        if (max != -1)
                            sb.Append(",max:" + max);
                        #endregion
                    }
                    #endregion

                    #region Y轴左右位置
                    if (num % 2 == 0)
                        sb.Append(",opposite: false");
                    else
                        sb.Append(",opposite: true");
                    num++;
                    #endregion

                    //#region Y轴线条
                    //sb.Append(",lineWidth: 1");
                    //#endregion
                    sb.Append("},");
                }
                if (sb.ToString().Substring(sb.ToString().Length - 1, 1).Trim().Equals(","))
                    sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));//去除最后一个逗号
            }
            else
            {
                sb.Append("{title: {text: '浓度'}}");
            }
            sb.Append("]");
            #endregion

            #region 图表type
            if (!chartType.Equals("")) sb.AppendFormat(",chartType:'{0}'", chartType);
            #endregion

            sb.Append("}");

            return sb.ToString().Replace("\r\n", "");
        }
        /// <summary>
        /// 修改by lvyun 2017-08-22
        /// </summary>
        /// <param name="dv"></param>
        /// <param name="FactorCode"></param>
        /// <param name="XName"></param>
        /// <param name="XLabel"></param>
        /// <param name="tooltip"></param>
        /// <param name="chartType"></param>
        /// <param name="pageType"></param>
        /// <param name="excessiveList"></param>
        /// <param name="pointid"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetChartDataMonthNTRegion(DataView dv, string[] FactorCode, string[] XName, string[] XLabel, string tooltip, string chartType, string pageType, IQueryable<ExcessiveSettingInfo> excessiveList = null, string region="", string name = "")
        {
          dv = dv.ToTable(true).DefaultView;//去除重复数据
          StringBuilder sb = new StringBuilder();
          string categories = "";
          List<SmartEP.Core.Interfaces.IPollutant> pollutantList = new List<SmartEP.Core.Interfaces.IPollutant>(); ;
          decimal max = -1;
          string plotLines = "";
          sb.Append("{");
          string pt = string.Empty;
          if (pageType == "Air1")
          {
            pageType = "Air";
            pt = "Air1";
          }
          #region series
          sb.Append("series:[");
          for (int i = 0; i < FactorCode.Length; i++)
          {
            string factor = FactorCode[i];
            SmartEP.Core.Interfaces.IPollutant pollutant = GetPollutantName(pageType, factor);
            string factorName = pollutant.PollutantName;
            //用于显示多Y轴
            if (pollutantList.Count == 0 || !pollutantList.Where(x => !x.PollutantCode.Equals(factorY)).Select(x => x.PollutantMeasureUnit).ToArray().Contains(pollutant.PollutantMeasureUnit) || factor.Equals(factorY))
              pollutantList.Add(pollutant);
            //if (!unit.Contains(pollutant.PollutantMeasureUnit)) unit += i == 0 ? pollutant.PollutantMeasureUnit : ";" + pollutant.PollutantMeasureUnit;
            sb.Append("{");
            #region 对应Y轴
            int ynum = -1;
            if (factor.Equals(factorY))
              ynum = pollutantList.IndexOf(pollutant);
            else
              ynum = pollutantList.IndexOf(pollutantList.Where(x => !x.PollutantCode.Equals(factorY) && x.PollutantMeasureUnit == pollutant.PollutantMeasureUnit).FirstOrDefault());
            sb.AppendFormat("yAxis: {0},", ynum < 0 ? 0 : ynum);//Name属性 
            #endregion

            #region Name
            sb.AppendFormat("name:'{0}',", factorName);//Name属性
            #endregion

            #region Data
            string data = "";
            foreach (DataRowView row in dv)
            {
              #region categories
              if (i == 0)
              {
                string str = "";
                for (int j = 0; j < XName.Length; j++)
                {
                  str += row[XName[j]].ToString() + XLabel[j];
                }
                categories += "'" + str + "',";
              }
              #endregion

              #region X、Y值
              if (row[factor] != DBNull.Value)
              {
                if (pollutant.PollutantMeasureUnit == "μg/m³")
                {
                  data += (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[factor]) * 1000, 0).ToString()) + ",";
                }
                else
                {
                  data += (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[factor]), Convert.ToInt32(pollutant.PollutantDecimalNum)).ToString()) + ",";
                }
                //data += (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[factor]), Convert.ToInt32(pollutant.PollutantDecimalNum)).ToString()) + ",";
              }
              else
              {
                data += "null,";
              }
              #endregion
            }
            sb.AppendFormat("data:[{0}]", data.Equals("") ? "" : data.Substring(0, data.Length - 1) + "");
            #endregion

            #region 是否显示
            DataTable dt = dv.ToTable();
            sb.AppendFormat(",visible:{0}", Convert.ToInt32(dt.Compute("Count(" + factor + ")", "")) > 0 ? "true" : "false");
            #endregion

            #region ToolTip
            sb.Append(",tooltip: { valueSuffix: '" + (pollutant.PollutantMeasureUnit == "mg/m3" ? "mg/m<sup>3</sup>" : pollutant.PollutantMeasureUnit) + "' }");//Name属性                   
            #endregion
            sb.Append("},");
          }
          if (sb.ToString().Substring(sb.ToString().Length - 1, 1).Trim().Equals(","))
            sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));//去除最后一个逗号
          sb.Append("]");
          #endregion

          #region title
          if (pt == "Air1")
          {
            sb.Append(",titleText:" + "'" + region + "'");

          }
          else
          {
            sb.Append(",titleText:" + "'" + region + "'");
          }


          #endregion

          #region tooltip
          sb.Append(",tooltip:{" + tooltip + "}");
          #endregion

          #region categories
          if (!categories.Equals(""))
          {
            categories = categories.ToString().Substring(0, categories.ToString().Length - 1);//去除最后一个逗号
            categories = categories.Replace("年", "/");//修改时间格式
            categories = categories.Replace("月", "");
          }
          sb.Append(",categories:[" + categories + "]");
          #endregion

          #region xAxis
          sb.Append(",xAxisData:{categories:[" + categories + "],labels:{rotation:-30}}");
          //sb.Append(",xAxisData:{type:'datetime',labels:{formatter:formatterMonthData}}");
          #endregion

          #region YAxis
          sb.Append(",yAxis:[");
          if (pollutantList != null)
          {
            int num = 0;
            foreach (SmartEP.Core.Interfaces.IPollutant poll in pollutantList)
            {
              sb.Append("{");
              #region title属性
              sb.Append("title:{");
              sb.AppendFormat("text: \"{0}\"", poll.PollutantMeasureUnit == "mg/m3" ? "mg/m<sup>3</sup>" : poll.PollutantMeasureUnit);
              sb.Append("}");
              #endregion
              #region 对数轴
              //sb.Append(",endOnTick: false,type: 'logarithmic'");
              #endregion
              #region 超标等级线
              plotLines = "";
              if (excessiveList != null)
              {
                max = dv.ToTable().Compute("Max(" + poll.PollutantCode + ")", "") != DBNull.Value ? Convert.ToDecimal(dv.ToTable().Compute("Max(" + poll.PollutantCode + ")", "")) : -1;
                foreach (ExcessiveSettingInfo excess in excessiveList)
                {
                  if (excess.PollutantGuid.Equals(poll.PollutantGuid))
                  {
                    max = max == -1 || max < excess.excessiveUpper ? excess.excessiveUpper : max;
                    #region 下限
                    //plotLines += "{";
                    //plotLines += string.Format("value:{0}", excess.excessiveLow);
                    //plotLines += string.Format(",color:\"{0}\"", "red");
                    //plotLines += string.Format(",width:{0}", 2);
                    //plotLines += string.Format(",dashStyle:'{0}'", "shortdot");
                    //#region Label属性
                    //plotLines += ",label: {";
                    //plotLines += string.Format("text:\"{0}\"", "超下限");
                    //plotLines += string.Format(",align:\"{0}\"", "left");
                    //plotLines += "}";
                    //#endregion
                    //plotLines += "},";
                    #endregion

                    #region 上限
                    plotLines += "{";
                    plotLines += string.Format("value:{0}", excess.excessiveUpper);
                    plotLines += string.Format(",color:\"{0}\"", "red");
                    plotLines += string.Format(",width:{0}", 2);
                    plotLines += string.Format(",dashStyle:'{0}'", "shortdot");
                    #region Label属性
                    plotLines += ",label: {";
                    plotLines += string.Format("text:\"{0}\"", "超标限(" + excess.excessiveUpper + ")");
                    plotLines += string.Format(",align:\"{0}\"", "left");
                    plotLines += "}";
                    #endregion
                    plotLines += "},";
                    #endregion
                  }
                }
                #region 等级线
                sb.Append(",plotLines: [" + (!plotLines.Equals("") ? plotLines.Substring(0, plotLines.Length - 1) : "") + "]");
                #endregion
                #region 最大值、最小值
                sb.Append(",min:0");
                if (max != -1)
                  sb.Append(",max:" + max);
                #endregion
              }
              #endregion

              #region Y轴左右位置
              if (num % 2 == 0)
                sb.Append(",opposite: false");
              else
                sb.Append(",opposite: true");
              num++;
              #endregion

              //#region Y轴线条
              //sb.Append(",lineWidth: 1");
              //#endregion
              sb.Append("},");
            }
            if (sb.ToString().Substring(sb.ToString().Length - 1, 1).Trim().Equals(","))
              sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));//去除最后一个逗号
          }
          else
          {
            sb.Append("{title: {text: '浓度'}}");
          }
          sb.Append("]");
          #endregion

          #region 图表type
          if (!chartType.Equals("")) sb.AppendFormat(",chartType:'{0}'", chartType);
          #endregion

          sb.Append("}");

          return sb.ToString().Replace("\r\n", "");
        }
        /// <summary>
        /// 污染物分析数据构造
        /// </summary>
        /// <param name="dv"></param>
        /// <param name="FactorCode"></param>
        /// <param name="FactorName"></param>
        /// <param name="tooltip"></param>
        /// <param name="chartType"></param>
        /// <param name="pageType"></param>
        /// <returns></returns>
        public string GetChartData(DataView dv, string[] FactorCode, string[] FactorName, string[] FactorUnit, string XName, string tooltip, string chartType, string pageType, IQueryable<ExcessiveSettingInfo> excessiveList = null, int pointid = -9999)
        {
            dv = dv.ToTable(true).DefaultView;//去除重复数据
            dv.Sort = XName + " ASC";
            StringBuilder sb = new StringBuilder();
            decimal max = -1;
            string plotLines = "";
            sb.Append("{");

            #region series
            sb.Append("series:[");
            try
            {
                for (int i = 0; i < FactorCode.Length; i++)
                {
                    string factor = FactorCode[i];
                    string factorName = FactorName[i];
                    sb.Append("{");
                    #region 对应Y轴
                    if (factor.Equals("CO"))
                        sb.AppendFormat("yAxis:1,");
                    else
                        sb.AppendFormat("yAxis:0,");
                    #endregion

                    #region Name
                    sb.AppendFormat("name:'{0}'", factorName);//Name属性
                    #endregion

                    #region Data
                    string data = "";
                    foreach (DataRowView row in dv)
                    {
                        DateTime tstamp = Convert.ToDateTime(row[XName]);
                        string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour, tstamp.Minute, tstamp.Second, tstamp.Millisecond);

                        #region X、Y值
                        if (row[factor] != DBNull.Value)
                        {
                            try
                            {
                                if (factor != "CO")
                                {
                                    data += "[" + time + "," + (Convert.ToDecimal(row[factor].ToString()) * 1000).ToString() + "],";
                                }
                                else
                                {
                                    data += "[" + time + "," + row[factor] + "],";
                                }
                            }
                            catch (Exception ex) { }

                            //data += "[" + time + "," + row[factor] + "],";
                        }
                        else
                        {
                            data += "[" + time + ",null],";
                        }
                        #endregion
                        #region 注释
                        //data += "{";
                        //#region X、Y值
                        //if (row[factor] != DBNull.Value)
                        //{

                        //    data += "x:" + time + ",y:" + row[factor] + ",name:'" + factorName + "'";
                        //}
                        //else
                        //{
                        //    data += "x:" + time + ",y:null,name:'" + factorName + "'";
                        //}
                        //#endregion
                        //data += "},";
                        #endregion
                    }
                    sb.AppendFormat(",data:[{0}]", data.Equals("") ? "" : data.Substring(0, data.Length - 1) + "");
                    #endregion

                    #region 是否显示
                    DataTable dt = dv.ToTable();
                    sb.AppendFormat(",visible:{0}", Convert.ToInt32(dt.Compute("Count(" + factor + ")", "")) > 0 ? "true" : "false");
                    #endregion

                    #region ToolTip

                    if (factorName == "一氧化碳")
                    {
                        sb.Append(",tooltip: { valueSuffix: '" + "mg/m<sup>3</sup>" + "' }");//Name属性
                    }
                    else
                    {
                        sb.Append(",tooltip: { valueSuffix: '" + "μg/m<sup>3</sup>" + "' }");//Name属性
                    }
                    //sb.Append(",tooltip: { valueSuffix: '" + "mg/m<sup>3</sup>" + "' }");//Name属性                   
                    #endregion
                    sb.Append("},");
                }
            }
            catch
            {
            }
            if (sb.ToString().Substring(sb.ToString().Length - 1, 1).Trim().Equals(","))
                sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));//去除最后一个逗号
            sb.Append("]");
            #endregion

            #region title
            if (pointid != -9999) sb.Append(",titleText:" + "'" + GetPointName(pageType, pointid) + "'");
            else
                sb.Append(",titleText:" + "''");
            #endregion

            #region tooltip
            sb.Append(",tooltip:{" + tooltip + "}");
            #endregion

            #region YAxis
            sb.Append(",yAxis:[");
            if (FactorUnit != null && FactorUnit.Length > 0)
            {
                int num = 0;
                foreach (string measureUnit in FactorUnit)
                {
                    sb.Append("{");
                    #region title属性
                    sb.Append("title:{");
                    //if (measureUnit == "mg/m3")
                    //{
                    //    sb.AppendFormat("text: \"{0}\"", measureUnit == "mg/m3" ? "mg/m<sup>3</sup>" : measureUnit);
                    //}
                    //if (measureUnit == "μg/m³")
                    //{
                    //    sb.AppendFormat("text: \"{0}\"", measureUnit == "μg/m³" ? "μg/m<sup>3</sup>" : measureUnit);
                    //}
                    sb.AppendFormat("text: \"{0}\"", measureUnit == "μg/m³" ? "μg/m<sup>3</sup>" : measureUnit);
                    sb.Append("}");
                    #endregion

                    #region 超标等级线
                    plotLines = "";
                    if (excessiveList != null)
                    {
                        foreach (ExcessiveSettingInfo excess in excessiveList)
                        {
                            if (excess.PollutantCode != "a21005")
                            {
                                max = dv.ToTable().Compute("Max(" + excess.PollutantCode + ")", "") != DBNull.Value ? Convert.ToDecimal(dv.ToTable().Compute("Max(" + excess.PollutantCode + ")", "")) * 1000 : -1;
                            }
                            else
                            {
                                max = dv.ToTable().Compute("Max(" + excess.PollutantCode + ")", "") != DBNull.Value ? Convert.ToDecimal(dv.ToTable().Compute("Max(" + excess.PollutantCode + ")", "")) : -1;
                            }
                            //max = dv.ToTable().Compute("Max(" + excess.PollutantCode + ")", "") != DBNull.Value ? Convert.ToDecimal(dv.ToTable().Compute("Max(" + excess.PollutantCode + ")", "")) : -1;
                            max = max == -1 || max < excess.excessiveUpper ? excess.excessiveUpper : max;

                            #region 上限
                            plotLines += "{";
                            plotLines += string.Format("value:{0}", excess.excessiveUpper);
                            plotLines += string.Format(",color:\"{0}\"", "red");
                            plotLines += string.Format(",width:{0}", 2);
                            plotLines += string.Format(",dashStyle:'{0}'", "shortdot");
                            #region Label属性
                            plotLines += ",label: {";
                            plotLines += string.Format("text:\"{0}\"", "超标限(" + excess.excessiveUpper + ")");
                            plotLines += string.Format(",align:\"{0}\"", "left");
                            plotLines += "}";
                            #endregion
                            plotLines += "},";
                            #endregion
                        }
                        #region 等级线
                        sb.Append(",plotLines: [" + (!plotLines.Equals("") ? plotLines.Substring(0, plotLines.Length - 1) : "") + "]");
                        #endregion

                        #region 最大值、最小值
                        sb.Append(",min:0");
                        if (max != -1)
                            sb.Append(",max:" + max);
                        #endregion
                    }
                    #endregion

                    #region Y轴左右位置
                    if (num % 2 == 0)
                        sb.Append(",opposite: false");
                    else
                        sb.Append(",opposite: true");
                    num++;
                    #endregion

                    //#region Y轴线条
                    //sb.Append(",lineWidth: 1");
                    //#endregion
                    sb.Append("},");
                }
                if (sb.ToString().Substring(sb.ToString().Length - 1, 1).Trim().Equals(","))
                    sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));//去除最后一个逗号
            }
            else
            {
                sb.Append("{title: {text: '浓度'}}");
            }
            sb.Append("]");
            #endregion

            #region 图表type
            if (!chartType.Equals("")) sb.AppendFormat(",chartType:'{0}'", chartType);
            #endregion

            sb.Append("}");

            return sb.ToString().Replace("\r\n", "");
        }

        /// <summary>
        /// 根据因子编码获取污染物名称
        /// </summary>
        /// <param name="pageType">应用程序类型</param>
        /// <param name="pollutantCode">因子编码</param>
        /// <returns></returns>
        private SmartEP.Core.Interfaces.IPollutant GetPollutantName(string pageType, string pollutantCode)
        {
            try
            {
                if (pageType.Equals("Air"))
                {
                    AirPollutantService airService = new AirPollutantService();
                    return airService.GetPollutantInfo(pollutantCode);
                }
                else if (pageType.Equals("Water"))
                {
                    WaterPollutantService waterService = new WaterPollutantService();
                    return waterService.GetPollutantInfo(pollutantCode);
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }

        }

        /// <summary>
        /// 获取点位名称
        /// </summary>
        /// <param name="pageType"></param>
        /// <param name="pointid"></param>
        /// <returns></returns>
        private string GetPointName(string pageType, int pointid)
        {
            try
            {
                if (pageType.Equals("Air"))
                {
                    MonitoringPointAirService g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
                    return g_MonitoringPointAir.RetrieveEntityByPointId(pointid).MonitoringPointName;
                }
                else if (pageType.Equals("Water"))
                {
                    MonitoringPointWaterService g_MonitoringPointWater = Singleton<MonitoringPointWaterService>.GetInstance();
                    return g_MonitoringPointWater.RetrieveEntityByPointId(pointid).MonitoringPointName;
                }
                else
                {
                    return "";
                }
            }
            catch
            {
                return "";
            }

        }
    }
}