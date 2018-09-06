using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Service.BaseData.Channel;
using SmartEP.Core.Enums;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Core.Generic;
using SmartEP.Service.DataAuditing.AuditBaseInfo;

namespace SmartEP.Service.DataAuditing.AuditInterfaces
{
    /// <summary>
    /// 名称：DataDealService.cs
    /// 创建人：徐龙超
    /// 创建日期：2015-08-16
    /// 维护人员：
    /// 最新维护人员：吕云
    /// 最新维护日期：2017-6-20
    /// 功能摘要：
    /// 审核站点抽象类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class DataDealService
    {
        AuditDataService auditDataService = new AuditDataService();
        string factorY = "a21005";//环境空气一氧化碳新增Y轴
        string y1 = "w21003;w21011";//氨氮;总磷
        #region 获取审核小时数据Chart的JSON数据(单点多因子)
        public string AuditMultiFacotr(string applicationUID, string[] portId, string[] factor, DateTime startTime, DateTime endTime, bool isAllDate)
        {
            string IsShowScroll = "false";
            StringBuilder sb = new StringBuilder();
            string factorName = "";
            int num = 0;
            List<SmartEP.Core.Interfaces.IPollutant> pollutantList = new List<SmartEP.Core.Interfaces.IPollutant>(); ;
            DataView dv = auditDataService.RetrieveAuditHourData(applicationUID, portId, factor, startTime, endTime, isAllDate);
            string category = "";
            sb.Append("{");
            #region series
            sb.Append("seriesList:[");
            if (dv.Count > 48) IsShowScroll = "true";
            foreach (string pointID in portId)
            {
                for (int i = 0; i < factor.Length; i++)
                {
                    string fac = factor[i];
                    if (!pointID.Equals("") && !fac.Equals(""))
                    {
                        SmartEP.Core.Interfaces.IPollutant pollutant = GetPollutantInfo(applicationUID, fac);
                        if (num == 0)
                        {
                            factorName += factorName.Equals("") ? pollutant.PollutantName : ";" + pollutant.PollutantName;
                            //用于显示多Y轴
                            if (pollutantList.Count == 0 || !pollutantList.Where(x => !x.PollutantCode.Equals(factorY)).Select(x => x.PollutantMeasureUnit).ToArray().Contains(pollutant.PollutantMeasureUnit) || fac.Equals(factorY))
                                pollutantList.Add(pollutant);
                        }
                        sb.Append("{");
                        #region 对应Y轴
                        int ynum = -1;
                        if (fac.Equals(factorY))
                            ynum = pollutantList.IndexOf(pollutant);
                        else
                            ynum = pollutantList.IndexOf(pollutantList.Where(x => !x.PollutantCode.Equals(factorY) && x.PollutantMeasureUnit == pollutant.PollutantMeasureUnit).FirstOrDefault());
                        sb.AppendFormat("yAxisIndex: {0},", ynum < 0 ? 0 : ynum);//Name属性 
                        #endregion

                        #region Series 基本设置
                        sb.AppendFormat(@"name: '{0}', type: 'line', showAllSymbol: true,", pollutant.PollutantName);
                        #endregion

                        #region Data
                        string str = "";
                        foreach (DataRowView row in dv)
                        {
                            DateTime tstamp = Convert.ToDateTime(row["DataDateTime"]);
                            //string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour - 7, tstamp.Minute, tstamp.Second, tstamp.Millisecond);

                            if (i == 0)
                            {
                                if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(applicationUID))
                                    category += tstamp + ",";
                                else
                                    category += tstamp.AddHours(1) + ",";
                            }
                            str += "{";
                            #region 节点样式
                            if (row[fac + "_AuditFlag"] != DBNull.Value && row[fac + "_AuditFlag"].ToString().Contains("RM"))
                            {

                                str += "itemStyle: {normal: {color: '" + "red" + "', label: { show: true,  position: 'inside', formatter: '" + "RM" + "' } } }, symbol: 'pin', symbolSize: 12,";
                            }

                            #endregion

                            #region X、Y值
                            int DecimalNum = pollutant.PollutantDecimalNum != null ? Convert.ToInt32(pollutant.PollutantDecimalNum) : 3;
                            str += "value:";
                            if (row[fac] != DBNull.Value)
                            {
                                str += (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[fac]), DecimalNum));
                            }
                            else
                            {
                                //str += time + ",null" + "," + row["PointId"];
                                str += "'-'";
                            }
                            //int DecimalNum = pollutant.PollutantDecimalNum != null ? Convert.ToInt32(pollutant.PollutantDecimalNum) : 3;
                            //str += "value:[";
                            //if (row[fac] != DBNull.Value)
                            //{
                            //    str += time + "," + (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[fac]), DecimalNum)) + "," + row["PointId"];
                            //}
                            //else
                            //{
                            //    //str += time + ",null" + "," + row["PointId"];
                            //    str += time + ",'-'" + "," + row["PointId"];
                            //}
                            //str += "]";
                            #endregion

                            str += "},";
                        }
                        if (str.Substring(str.ToString().Length - 1, 1).Trim().Equals(","))
                            str = str.ToString().Substring(0, str.ToString().Length - 1);//去除最后一个逗号
                        sb.AppendFormat("data:[{0}]", str);
                        #endregion
                        sb.Append("},");
                    }
                }
                num++;
            }
            if (sb.ToString().Substring(sb.ToString().Length - 1, 1).Trim().Equals(","))
                sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));//去除最后一个逗号
            else
            {
                sb.Append("{data:[]}");
            }
            sb.Append("]");
            #endregion

            #region category
            sb.Append(",category:[");
            sb.Append("'" + category.Replace(",", "','") + "'");
            sb.Append("]");
            #endregion

            #region 图例
            sb.Append(",legend:['" + string.Join("','", factorName.Split(';')) + "']");//图例
            #endregion

            #region 滚动条
            sb.Append(",dataZoomShow:" + IsShowScroll);//是否显示滚动条
            #endregion

            #region YAxis
            sb.Append(",yAxis:[");
            if (pollutantList != null)
            {
                num = 0;
                foreach (SmartEP.Core.Interfaces.IPollutant poll in pollutantList)
                {
                    sb.Append("{");
                    #region title属性
                    sb.AppendFormat("name: \"{0}\"", poll.PollutantMeasureUnit == "mg/L" ? "mg/m^3" : poll.PollutantMeasureUnit);
                    #endregion

                    #region Y轴左右位置
                    //if (num % 2 == 0)
                    //    sb.Append(",opposite: false");
                    //else
                    //    sb.Append(",opposite: true");
                    //num++;
                    #endregion

                    #region Type
                    sb.Append(", type: 'value'");
                    #endregion

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

            sb.Append("}");
            return sb.ToString().Replace("\r\n", "");
        }
        #endregion

        #region 获取审核小时数据Chart的JSON数据(多点多因子)
        public string AuditMultiPointFacotr(string applicationUID, string[] portId, string[] factor, DateTime startTime, DateTime endTime, bool isAllDate, IList<PointPollutantInfo> pList, string PointType = "0")
        {
            string IsShowScroll = "false";
            bool isShowTotal = false;
            StringBuilder sb = new StringBuilder();
            string factorName = "";
            int num = 0;
            List<SmartEP.Core.Interfaces.IPollutant> pollutantList = new List<SmartEP.Core.Interfaces.IPollutant>(); ;
            DataView dv = auditDataService.RetrieveAuditHourData(applicationUID, portId, factor, startTime, endTime, isAllDate,isShowTotal, PointType);
            string category = "";
            DateTime[] timeCategory = null;
            string pointName = "";
            string legend = "";
            sb.Append("{");
            #region series
            sb.Append("seriesList:[");
            //if (dv.Count > 48) IsShowScroll = "true";

            //if (!category.Split(',').Contains(tstamp.ToString()))
            //    category += tstamp + ",";
            if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(applicationUID))//水不是模式24条，有多少显示多少
            {
                timeCategory = dv.ToTable().AsEnumerable().Select(d => d.Field<DateTime>("DataDateTime")).Distinct().OrderByDescending(d => d).ToArray();
                category = string.Join(",", timeCategory);

            }
            else
            {
                //timeCategory = dv.ToTable().AsEnumerable().Select(d => d.Field<DateTime>("DataDateTime").AddHours(1)).Distinct().OrderBy(d => d).ToArray();
                timeCategory = dv.ToTable().AsEnumerable().Select(d => d.Field<DateTime>("DataDateTime").AddHours(0)).Distinct().OrderBy(d => d).ToArray();
                category = string.Join(",", timeCategory);
            }
            foreach (string pointID in portId)
            {
                if (pointID.Equals("")) continue;
                pointName = GetPointName(applicationUID, Convert.ToInt32(pointID));
                legend += pointName + ";";
                for (int i = 0; i < factor.Length; i++)
                {
                    string fac = factor[i];
                    if (!pointID.Equals("") && !fac.Equals(""))
                    {
                        dv.RowFilter = "";
                        dv.RowFilter = "PointID=" + pointID;
                        dv.Sort = "DataDateTime ASC";
                        //if (dv.Count <= 0) { num--; break; }
                        SmartEP.Core.Interfaces.IPollutant pollutant = GetPollutantInfo(applicationUID, fac);
                        if (num == 0)
                        {
                            factorName += factorName.Equals("") ? pollutant.PollutantName : ";" + pollutant.PollutantName;
                            //用于显示多Y轴
                            if (pollutantList.Count == 0 || !pollutantList.Where(x => !x.PollutantCode.Equals(factorY)).Select(x => x.PollutantMeasureUnit).ToArray().Contains(pollutant.PollutantMeasureUnit) || fac.Equals(factorY))
                                pollutantList.Add(pollutant);
                        }

                        #region series
                        #region 获取Data
                        string str = "";
                        if (dv.Count > 0)
                        {
                            if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(applicationUID))//水不是模式24条，有多少显示多少
                            {
                                str = DealDataWater(dv, num, fac, pollutant, pointID);
                                if (str.Equals("")) continue;
                            }
                            else
                            {
                                string isEdit = "0";
                                try
                                {
                                    if (pList != null)
                                    {
                                        PointPollutantInfo factorInfo = pList.Where(x => x.PID == fac).FirstOrDefault();
                                        if (factorInfo.Tag == null)
                                            isEdit = "0";
                                        else
                                            isEdit = factorInfo.Tag;
                                    }
                                    else
                                        isEdit = "0";
                                }
                                catch
                                {
                                }

                                str = DealData(dv, num, fac, pollutant, pointID, isEdit);
                            }

                            if (!str.Equals("") && str.Substring(str.ToString().Length - 1, 1).Trim().Equals(","))
                            {
                                str = str.ToString().Substring(0, str.ToString().Length - 1);//去除最后一个逗号
                            }
                        }
                        else
                        {
                            if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(applicationUID))//水不是模式24条，有多少显示多少
                                break;
                        }

                        #endregion

                        #region Series
                        sb.Append("{");
                        #region 对应Y轴
                        int ynum = 0;
                        if (fac.Equals(factorY))
                            ynum = pollutantList.IndexOf(pollutant);
                        else
                            ynum = pollutantList.IndexOf(pollutantList.Where(x => !x.PollutantCode.Equals(factorY) && x.PollutantMeasureUnit == pollutant.PollutantMeasureUnit).FirstOrDefault());
                        if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(applicationUID))
                        {
                            sb.AppendFormat("yAxisIndex: {0},", y1.Split(';').Contains(fac) ? 0 : (pollutantList.Count > 1 ? 1 : 0));//Name属性 
                        }
                        else
                            sb.AppendFormat("yAxisIndex: {0},", ynum < 0 || ynum >= 2 ? 0 : ynum);//Name属性 

                        #endregion

                        #region Series 基本设置
                        sb.AppendFormat(@"name: '{0}', type: 'line', showAllSymbol: true", pointName);
                        #endregion

                        #region Data
                        sb.AppendFormat(",data:[{0}]", str);
                        #endregion

                        sb.Append("},");
                        #endregion
                        #endregion
                    }
                }
                num++;
            }
            if (sb.ToString().Substring(sb.ToString().Length - 1, 1).Trim().Equals(","))
                sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));//去除最后一个逗号
            else
            {
                sb.Append("{data:[]}");
            }
            sb.Append("]");
            #endregion

            #region category
            sb.Append(",category:[");
            sb.Append("'" + category.Replace(",", "','") + "'");
            sb.Append("]");
            #endregion

            #region 图例
            if (legend.Length > 0)
                sb.Append(",legend:['" + string.Join("','", legend.Substring(0, legend.Length - 1).Split(';')) + "']");//图例
            #endregion

            #region 滚动条
            sb.Append(",dataZoomShow:" + IsShowScroll);//是否显示滚动条
            #endregion

            #region YAxis
            sb.Append(",yAxis:[");
            if (pollutantList != null)
            {
                num = 0;
                foreach (SmartEP.Core.Interfaces.IPollutant poll in pollutantList)
                {
                    if (num >= 2) break;
                    sb.Append("{");
                    #region title属性
                    //if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(applicationUID))
                    sb.AppendFormat("name: \"{0}\"", "浓度");
                    //else
                    //    sb.AppendFormat("name: \"{0}\"", poll.PollutantMeasureUnit == "mg/L" ? "mg/m^3" : poll.PollutantMeasureUnit);
                    #endregion

                    #region Y轴左右位置
                    //if (num % 2 == 0)
                    //    sb.Append(",opposite: false");
                    //else
                    //    sb.Append(",opposite: true");
                    num++;
                    #endregion

                    #region Type
                    sb.Append(", type: 'value'");
                    #endregion

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

            #region 系统类型
            sb.AppendFormat(@",applicationType:'{0}'", (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID) ? "Air" : "Water"));//图例
            #endregion

            sb.Append("}");
            return sb.ToString().Replace("\r\n", "");
        }
        #endregion

        #region 获取审核小时数据Chart的JSON数据(多点多因子)超级站
        public string AuditMultiPointFacotrSuper(string applicationUID, string[] portId, string[] factor, DateTime startTime, DateTime endTime, bool isAllDate, IList<PointPollutantInfo> pList)
        {
            string IsShowScroll = "false";
            StringBuilder sb = new StringBuilder();
            string factorName = "";
            int num = 0;
            List<SmartEP.Core.Interfaces.IPollutant> pollutantList = new List<SmartEP.Core.Interfaces.IPollutant>(); ;
            DataView dv = auditDataService.RetrieveAuditHourDataSuper(applicationUID, portId, factor, startTime, endTime, isAllDate,false,"1");
            string category = "";
            DateTime[] timeCategory = null;
            string pointName = "";
            string legend = "";
            sb.Append("{");
            #region series
            sb.Append("seriesList:[");
            //if (dv.Count > 48) IsShowScroll = "true";

            //if (!category.Split(',').Contains(tstamp.ToString()))
            //    category += tstamp + ",";
            if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(applicationUID))//水不是模式24条，有多少显示多少
            {
                timeCategory = dv.ToTable().AsEnumerable().Select(d => d.Field<DateTime>("DataDateTime")).Distinct().OrderByDescending(d => d).ToArray();
                category = string.Join(",", timeCategory);

            }
            else
            {
                timeCategory = dv.ToTable().AsEnumerable().Select(d => d.Field<DateTime>("DataDateTime")).Distinct().OrderBy(d => d).ToArray();
                category = string.Join(",", timeCategory);
            }
            foreach (string pointID in portId)
            {
                if (pointID.Equals("")) continue;
                pointName = GetPointName(applicationUID, Convert.ToInt32(pointID));
                legend += pointName + ";";
                for (int i = 0; i < factor.Length; i++)
                {
                    string fac = factor[i];
                    if (!pointID.Equals("") && !fac.Equals(""))
                    {
                        dv.RowFilter = "";
                        dv.RowFilter = "PointID=" + pointID;
                        dv.Sort = "DataDateTime ASC";
                        //if (dv.Count <= 0) { num--; break; }
                        SmartEP.Core.Interfaces.IPollutant pollutant = GetPollutantInfo(applicationUID, fac);
                        if (num == 0)
                        {
                            factorName += factorName.Equals("") ? pollutant.PollutantName : ";" + pollutant.PollutantName;
                            //用于显示多Y轴
                            if (pollutantList.Count == 0 || !pollutantList.Where(x => !x.PollutantCode.Equals(factorY)).Select(x => x.PollutantMeasureUnit).ToArray().Contains(pollutant.PollutantMeasureUnit) || fac.Equals(factorY))
                                pollutantList.Add(pollutant);
                        }

                        #region series
                        #region 获取Data
                        string str = "";
                        if (dv.Count > 0)
                        {
                            if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(applicationUID))//水不是模式24条，有多少显示多少
                            {
                                str = DealDataWater(dv, num, fac, pollutant, pointID);
                                if (str.Equals("")) continue;
                            }
                            else
                            {
                                string isEdit = "0";
                                try
                                {
                                    if (pList != null)
                                    {
                                        PointPollutantInfo factorInfo = pList.Where(x => x.PID == fac).FirstOrDefault();
                                        if (factorInfo.Tag == null)
                                            isEdit = "0";
                                        else
                                            isEdit = factorInfo.Tag;
                                    }
                                    else
                                        isEdit = "0";
                                }
                                catch
                                {
                                }

                                str = DealData(dv, num, fac, pollutant, pointID, isEdit);
                            }

                            if (!str.Equals("") && str.Substring(str.ToString().Length - 1, 1).Trim().Equals(","))
                            {
                                str = str.ToString().Substring(0, str.ToString().Length - 1);//去除最后一个逗号
                            }
                        }
                        else
                        {
                            if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(applicationUID))//水不是模式24条，有多少显示多少
                                break;
                        }

                        #endregion

                        #region Series
                        sb.Append("{");
                        #region 对应Y轴
                        int ynum = 0;
                        if (fac.Equals(factorY))
                            ynum = pollutantList.IndexOf(pollutant);
                        else
                            ynum = pollutantList.IndexOf(pollutantList.Where(x => !x.PollutantCode.Equals(factorY) && x.PollutantMeasureUnit == pollutant.PollutantMeasureUnit).FirstOrDefault());
                        if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(applicationUID))
                        {
                            sb.AppendFormat("yAxisIndex: {0},", y1.Split(';').Contains(fac) ? 0 : (pollutantList.Count > 1 ? 1 : 0));//Name属性 
                        }
                        else
                            sb.AppendFormat("yAxisIndex: {0},", ynum < 0 || ynum >= 2 ? 0 : ynum);//Name属性 

                        #endregion

                        #region Series 基本设置
                        sb.AppendFormat(@"name: '{0}', type: 'line', showAllSymbol: true", pointName);
                        #endregion

                        #region Data
                        sb.AppendFormat(",data:[{0}]", str);
                        #endregion

                        sb.Append("},");
                        #endregion
                        #endregion
                    }
                }
                num++;
            }
            if (sb.ToString().Substring(sb.ToString().Length - 1, 1).Trim().Equals(","))
                sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));//去除最后一个逗号
            else
            {
                sb.Append("{data:[]}");
            }
            sb.Append("]");
            #endregion

            #region category
            sb.Append(",category:[");
            sb.Append("'" + category.Replace(",", "','") + "'");
            sb.Append("]");
            #endregion

            #region 图例
            if (legend.Length > 0)
                sb.Append(",legend:['" + string.Join("','", legend.Substring(0, legend.Length - 1).Split(';')) + "']");//图例
            #endregion

            #region 滚动条
            sb.Append(",dataZoomShow:" + IsShowScroll);//是否显示滚动条
            #endregion

            #region YAxis
            sb.Append(",yAxis:[");
            if (pollutantList != null)
            {
                num = 0;
                foreach (SmartEP.Core.Interfaces.IPollutant poll in pollutantList)
                {
                    if (num >= 2) break;
                    sb.Append("{");
                    #region title属性
                    //if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(applicationUID))
                    sb.AppendFormat("name: \"{0}\"", "浓度");
                    //else
                    //    sb.AppendFormat("name: \"{0}\"", poll.PollutantMeasureUnit == "mg/L" ? "mg/m^3" : poll.PollutantMeasureUnit);
                    #endregion

                    #region Y轴左右位置
                    //if (num % 2 == 0)
                    //    sb.Append(",opposite: false");
                    //else
                    //    sb.Append(",opposite: true");
                    num++;
                    #endregion

                    #region Type
                    sb.Append(", type: 'value'");
                    #endregion

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

            #region 系统类型
            sb.AppendFormat(@",applicationType:'{0}'", (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID) ? "Air" : "Water"));//图例
            #endregion

            sb.Append("}");
            return sb.ToString().Replace("\r\n", "");
        }
        #endregion

        private string DealData(DataView dv, int num, string fac, SmartEP.Core.Interfaces.IPollutant pollutant, string pointID, string isEdit)
        {
            string str = "";
            foreach (DataRowView row in dv)
            {
                DateTime tstamp = Convert.ToDateTime(row["DataDateTime"]);
                //string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month - 1, tstamp.Day, tstamp.Hour - 7, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                str += "{";

                #region 节点样式
                if (row[fac + "_AuditFlag"] != DBNull.Value && row[fac + "_AuditFlag"].ToString().Contains("RM"))
                {

                    str += "itemStyle: {normal: {color: '" + "red" + "', label: { show: true,  position: 'inside', formatter: '" + "RM" + "' } } }, symbol: 'pin', symbolSize: 12,";
                }

                #endregion

                #region X、Y值
                int DecimalNum = pollutant.PollutantDecimalNum != null ? Convert.ToInt32(pollutant.PollutantDecimalNum) : 3;
                str += "value:";
                if (row[fac] != DBNull.Value)
                {
                    str += (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[fac]), DecimalNum));
                }
                else
                {
                    //str += time + ",null" + "," + row["PointId"];
                    str += "'-'";
                }
                #endregion

                #region name
                str += string.Format(@",name:'{0}'", pollutant.PollutantName + ";" + pollutant.PollutantCode + ";" + pointID + ";" + tstamp + ";" + isEdit);
                #endregion
                str += "},";

                //}
            }
            return str;
        }

        private string DealDataWater(DataView dv, int num, string fac, SmartEP.Core.Interfaces.IPollutant pollutant, string pointID)
        {
            string str = "";
            foreach (DataRowView row in dv)
            {
                if (row[fac] != DBNull.Value)
                {
                    DateTime tstamp = Convert.ToDateTime(row["DataDateTime"]);
                    string time = string.Format("new Date({0},{1},{2},{3},{4})", tstamp.Year, tstamp.Month, tstamp.Day, tstamp.Hour, tstamp.Minute);
                    //string time = string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", tstamp.Year, tstamp.Month , tstamp.Day, tstamp.Hour - 6, tstamp.Minute, tstamp.Second, tstamp.Millisecond);
                    str += "{";
                    #region category
                    //if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(applicationUID))//水不是模式24条，有多少显示多少
                    //{
                    //    //if (!timeCategory.Contains(tstamp))
                    //    //    str += "";
                    //}
                    //else
                    //{
                    //    if (num == 0)
                    //    {

                    //        category += tstamp.AddHours(1) + ",";
                    //    }
                    //}
                    #endregion

                    #region 节点样式 #f39292,#24bd2a,#1768fd
                    if (row[fac + "_AuditFlag"] != DBNull.Value && row[fac + "_AuditFlag"].ToString().Contains("RM"))
                    {

                        str += "itemStyle: {normal: {color: '" + "red" + "', label: { show: true,  position: 'inside', formatter: '" + "RM" + "' } } }, symbol: 'pin', symbolSize: 12,";
                    }
                    else if (row[fac + "_AuditFlag"] != DBNull.Value && row[fac + "_AuditFlag"].ToString().Contains("QC"))
                    {

                        str += "itemStyle: {normal: {color: '" + "#f39292" + "', label: { show: true,  position: 'inside', formatter: '" + "QC" + "' } } }, symbol: 'pin', symbolSize: 12,";
                    }
                    else if (row[fac + "_AuditFlag"] != DBNull.Value && row[fac + "_AuditFlag"].ToString().Contains("PF"))
                    {

                        str += "itemStyle: {normal: {color: '" + "#24bd2a" + "', label: { show: true,  position: 'inside', formatter: '" + "PF" + "' } } }, symbol: 'pin', symbolSize: 12,";
                    }

                    #endregion

                    #region X、Y值
                    int DecimalNum = pollutant.PollutantDecimalNum != null ? Convert.ToInt32(pollutant.PollutantDecimalNum) : 3;
                    str += "value:[" + time + ",";

                    str += (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[fac]), DecimalNum));

                    str += "]";
                    #endregion

                    #region 是否显示
                    if (row[fac] == DBNull.Value)
                        str += string.Format(@",ignore:true");

                    #endregion

                    #region name
                    str += string.Format(@",name:'{0}'", pollutant.PollutantName + ";" + pollutant.PollutantCode + ";" + pointID + ";" + tstamp + ";0");
                    #endregion
                    str += "},";

                }
                //else
                //{
                //    str += "'-',";
                //}
            }
            return str;
        }

        #region 获取审核小时数据Chart的JSON数据(多点单因子)
        public string AuditSingleFacotr(string applicationUID, string[] portId, string[] portName, string factor, DateTime startTime, DateTime endTime, bool isAllDate)
        {
            StringBuilder sb = new StringBuilder();
            string str = "";
            SmartEP.Core.Interfaces.IPollutant pollutant = GetPollutantInfo(applicationUID, factor);
            DataView dv = auditDataService.RetrieveAuditHourData(applicationUID, portId, factor.Split(','), startTime, endTime, isAllDate);
            foreach (string pointID in portId)
            {
                #region Data
                dv.RowFilter = "PointId=" + pointID;
                foreach (DataRowView row in dv)
                {
                    #region X、Y值
                    if (row[factor] != DBNull.Value)
                    {
                        str += (DecimalExtension.GetRoundValue(Convert.ToDecimal(row[factor]), Convert.ToInt32(pollutant.PollutantDecimalNum))) + ",";
                    }
                    else
                    {
                        str += "[],";
                    }
                    #endregion
                }
                dv.RowFilter = "";
                #endregion
            }
            if (str.Substring(str.ToString().Length - 1, 1).Trim().Equals(","))
                str = str.ToString().Substring(0, str.ToString().Length - 1);//去除最后一个逗号
            sb.Append("{");
            #region  Series
            sb.Append("seriesList:[{");
            sb.AppendFormat(@"name: '{0}', type: 'bar', showAllSymbol: true,", "监测值(柱状图)");
            sb.Append("itemStyle:{ normal:{ color:'#FFD700'}},");
            sb.AppendFormat("data:[{0}]", str);
            sb.Append("},");
            sb.Append("{");
            sb.AppendFormat(@"name: '{0}', type: 'line', showAllSymbol: true,", "监测值(折线图)");
            sb.Append("itemStyle:{ normal:{ color:'#F01687' }},");
            sb.AppendFormat("data:[{0}]", str);
            sb.Append("}]");
            #endregion
            #region category
            sb.Append(",category:[");
            sb.Append("'" + string.Join("','", portName) + "'");
            sb.Append("]");
            #endregion
            #region title
            DateTime time = startTime;
            if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID)) time = time.AddHours(1);
            sb.Append(",title:");
            sb.Append("'" + pollutant.PollutantName + "  " + (pollutant.PollutantMeasureUnit == "mg/L" ? "mg/m^3" : pollutant.PollutantMeasureUnit) + "(" + time + ")'");
            #endregion
            #region YAxis
            //sb.Append(",yAxis:");
            //sb.Append("{");
            //#region title属性
            //sb.AppendFormat("name: \"{0}\"", pollutant.PollutantMeasureUnit == "mg/L" ? "mg/m^3" : pollutant.PollutantMeasureUnit);
            //#endregion
            //#region Type
            //sb.Append(", type: 'value'");
            //#endregion
            //sb.Append("}");
            //sb.Append("");
            #endregion

            sb.Append("}");
            return sb.ToString().Replace("\r\n", "");
        }
        #endregion

        /// <summary>
        /// 根据因子编码获取污染物名称
        /// </summary>
        /// <param name="pageType">应用程序类型</param>
        /// <param name="pollutantCode">因子编码</param>
        /// <returns></returns>
        private SmartEP.Core.Interfaces.IPollutant GetPollutantInfo(string applicationUID, string pollutantCode)
        {
            if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID))//环境空气
            {
                AirPollutantService airService = new AirPollutantService();
                return airService.GetPollutantInfo(pollutantCode);
            }
            else if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(applicationUID))//地表水
            {
                WaterPollutantService waterService = new WaterPollutantService();
                return waterService.GetPollutantInfo(pollutantCode);
            }
            else
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
        private string GetPointName(string applicationUID, int pointid)
        {
            if (EnumMapping.GetApplicationValue(ApplicationValue.Air).Equals(applicationUID))//环境空气
            {
                MonitoringPointAirService g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
                return g_MonitoringPointAir.RetrieveEntityByPointId(pointid).MonitoringPointName;
            }
            else if (EnumMapping.GetApplicationValue(ApplicationValue.Water).Equals(applicationUID))//地表水
            {
                MonitoringPointWaterService g_MonitoringPointWater = Singleton<MonitoringPointWaterService>.GetInstance();
                return g_MonitoringPointWater.RetrieveEntityByPointId(pointid).MonitoringPointName;
            }
            else
            {
                return "";
            }

        }
    }
}
