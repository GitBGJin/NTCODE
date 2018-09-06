using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.MonitoringBusinessRepository.Water;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.BaseData.Standard;
using SmartEP.Service.Core.Enums;
using SmartEP.DomainModel.BaseData;
using SmartEP.BaseInfoRepository.Channel;
using SmartEP.Service.Frame;
using SmartEP.DomainModel;
using SmartEP.Data.SqlServer.BaseData;
using System.Collections;
using System.ComponentModel;

namespace SmartEP.Service.DataAnalyze.Water.DataQuery
{
    /// <summary>
    /// 名称：DataQueryByHourService.cs
    /// 创建人：窦曙健
    /// 创建日期：2015-08-25
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：审核小时数据
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class DataQueryByHourService
    {
        /// <summary>
        /// 地表水小时数据
        /// </summary>
        HourReportRepository HourData = Singleton<HourReportRepository>.GetInstance();
        EQIConcentrationService EQIService = new EQIConcentrationService();
        WaterAnalyzeDAL m_WaterAnalyzeDAL = new WaterAnalyzeDAL();

        /// <summary>
        /// 获取因子小数位 channelCode 因子代码 例：CO的code是a21005  （ channelCode=a21005）
        /// </summary>
        SmartEP.Service.BaseData.Channel.WaterPollutantService m_WaterPollutantService = new SmartEP.Service.BaseData.Channel.WaterPollutantService();

        /// <summary>
        /// 测点类
        /// </summary>
        MonitoringPointWaterService g_MonitoringPointWater = null;

        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetHourDataPager(string[] portIds, IList<IPollutant> factors, DateTime dtStart, DateTime dtEnd
            , int pageSize, int pageNo, out int recordTotal, string orderBy = "Tstamp,PointId")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return HourData.GetDataPager(portIds, factors.Select(p => p.PollutantCode).ToArray(), dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
            return null;
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetNewHourDataPager(string[] portIds, IList<IPollutant> factors, DateTime dtStart, DateTime dtEnd
            , int pageSize, int pageNo, out int recordTotal, string orderBy = "Tstamp,PointId")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return HourData.GetNewDataPager(portIds, factors.Select(p => p.PollutantCode).ToArray(), dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
            return null;
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetHourDataPager(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd
            , int pageSize, int pageNo, out int recordTotal, string orderBy = "Tstamp,PointId")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return HourData.GetDataPager(portIds, factors, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
            return null;
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetDataHourPager(string[] portIds, string[] factors, DateTime dtBegin, DateTime dtEnd,
            DateTime dtFrom, DateTime dtTo, string type, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,tstamp desc")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return HourData.GetDataHourPager(portIds, factors, dtBegin, dtEnd, dtFrom, dtTo, type, pageSize, pageNo, out recordTotal, orderBy);
            return null;
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetHourData(string[] portIds, string[] factors, DateTime dtBegin, DateTime dtEnd,
            DateTime dtFrom, DateTime dtTo, string type, int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,tstamp desc,Type")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return HourData.GetDataPager(portIds, factors, dtBegin, dtEnd, dtFrom, dtTo, type, pageSize, pageNo, out recordTotal, orderBy);
            return null;
        }
        /// <summary>
        /// 取得统计数据（最大值、最小值、平均值）
        /// </summary>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetHourStatisticalData(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd)
        {
            if (factors.IsNotNullOrDBNull())
            {
                g_MonitoringPointWater = Singleton<MonitoringPointWaterService>.GetInstance();
                DataTable dt = dt = HourData.GetStatisticalData(portIds, factors, dateStart, dateEnd).Table;
                dt.Columns.Add("portName", typeof(string));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int pointId = Convert.ToInt32(dt.Rows[i]["PointId"]);
                    dt.Rows[i]["portName"] = g_MonitoringPointWater.RetrieveEntityByPointId(pointId).MonitoringPointName;
                }
                return dt.DefaultView;
            }
            return null;
        }
        public DataView GetHourStatisticalDataNew(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd)
        {
            if (factors.IsNotNullOrDBNull())
            {
                g_MonitoringPointWater = Singleton<MonitoringPointWaterService>.GetInstance();
                DataTable dt = dt = HourData.GetStatisticalDataNew(portIds, factors, dateStart, dateEnd).Table;
                dt.Columns.Add("portName", typeof(string));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int pointId = Convert.ToInt32(dt.Rows[i]["PointId"]);
                    dt.Rows[i]["portName"] = g_MonitoringPointWater.RetrieveEntityByPointId(pointId).MonitoringPointName;
                }
                return dt.DefaultView;
            }
            return null;
        }
        public DataTable GetCityDayReport(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd, Dictionary<string, string> pointKey)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("pointid", typeof(string));
                dt.Columns.Add("portName", typeof(string));
                dt.Columns.Add("datetime", typeof(DateTime));
                foreach (string factoritem in factors)
                {
                    dt.Columns.Add(factoritem, typeof(string));
                }
                dt.Columns.Add("video", typeof(string));
                dt.Columns.Add("description", typeof(string));
                DataView dv = HourData.GetStatisticalData(portIds, factors, dateStart, dateEnd);
                if (dv.Count > 0)
                {
                    foreach (string pointitem in portIds)
                    {
                        DataRow drNew = dt.NewRow();
                        drNew["pointid"] = pointitem;
                        int pointId = Convert.ToInt32(pointitem);
                        drNew["portName"] = pointKey[pointitem];
                        drNew["datetime"] = DateTime.Parse(dateStart.ToString("yyyy-MM-dd"));
                        foreach (string factoritem in factors)
                        {
                            dv.RowFilter = string.Format("PointId='{0}' and PollutantCode='{1}' ", pointitem, factoritem);
                            if (dv.Count > 0)
                            {
                                string max = dv[0]["Value_Max"] != DBNull.Value ? double.Parse(DecimalExtension.GetPollutantValue(decimal.Parse(dv[0]["Value_Max"].ToString()), 2).ToString()).ToString() : "/";
                                string min = dv[0]["Value_Min"] != DBNull.Value ? double.Parse(DecimalExtension.GetPollutantValue(decimal.Parse(dv[0]["Value_Min"].ToString()), 2).ToString()).ToString() : "/";
                                if (max != "/" && min != "/")
                                    drNew[factoritem] = min + "-" + max;
                            }
                        }
                        dt.Rows.Add(drNew);
                    }
                }
                return dt;
            }
            catch (Exception ex) { return null; }
        }
        /// <summary>
        /// 地表水综合统计中位数据
        /// </summary>
        /// <param name="pointIds"></param>
        /// <param name="factors"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataView getMedianData(string[] pointIds, string[] factors, DateTime dateStart, DateTime dateEnd)
        {
            MonitoringPointWaterService s_MonitoringPointWaterService = new MonitoringPointWaterService();
            SmartEP.Service.BaseData.Channel.WaterPollutantService s_WaterPollutantService = new SmartEP.Service.BaseData.Channel.WaterPollutantService();
            DataTable dt = new DataTable();
            dt.Columns.Add("RegionName", typeof(string));
            dt.Columns.Add("FactorName", typeof(string));
            dt.Columns.Add("MedianUpper", typeof(string));
            dt.Columns.Add("MedianLower", typeof(string));
            dt.Columns.Add("MedianValueu", typeof(string));
            dt.Columns.Add("lower", typeof(string));
            dt.Columns.Add("upper", typeof(string));
            int recordTotal = 0;
            DataView dvHourData = GetHourDataPager(pointIds, factors, dateStart, dateEnd, 99999, 0, out recordTotal);
            foreach (string pointId in pointIds)
            {
                if (pointId != "")
                {
                    string RegionName = s_MonitoringPointWaterService.RetrieveEntityByID(int.Parse(pointId)).MonitoringPointName;
                    foreach (string factorItem in factors)
                    {
                        PollutantCodeEntity factor = s_WaterPollutantService.RetrieveEntityByCode(factorItem);
                        DataRow dr = dt.NewRow();
                        dvHourData.RowFilter = string.Format(@"{0} is not null  and PointId={1}", factor.PollutantCode, pointId);
                        dr["RegionName"] = RegionName;
                        dr["FactorName"] = factor.PollutantName;
                        List<decimal> strFactor = dvHourData.ToTable().AsEnumerable().Select(r => r.Field<decimal>(factor.PollutantCode)).ToList();
                        //List<decimal> decFactor = new List<decimal>(strFactor.Select(x => decimal.Parse(x)));
                        Dictionary<string, decimal> ListMedian = Median(strFactor);
                        if (ListMedian.Keys.Contains("median"))
                        {
                            dr["MedianValueu"] = ListMedian["median"];
                        }
                        if (ListMedian.Keys.Contains("median1"))
                        {
                            dr["MedianLower"] = ListMedian["median1"];
                        }
                        if (ListMedian.Keys.Contains("median3"))
                        {
                            dr["MedianUpper"] = ListMedian["median3"];
                        } if (ListMedian.Keys.Contains("lower"))
                        {
                            dr["lower"] = ListMedian["lower"];
                        } if (ListMedian.Keys.Contains("upper"))
                        {
                            dr["upper"] = ListMedian["upper"];
                        }
                        dt.Rows.Add(dr);
                    }
                }
            }
            return dt.DefaultView;
        }
        /// <summary>
        /// 计算中位值，四分位值
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public Dictionary<string, decimal> Median(IList<decimal> array)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            return Median(array.ToArray());
        }

        public Dictionary<string, decimal> Median(decimal[] array)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            Dictionary<string, decimal> listmedian = new Dictionary<string, decimal>();

            if (array.Length > 0)
            {
                int endIndex = array.Length / 2;

                for (int i = 0; i < array.Length - 1; i++)
                {
                    for (int j = 0; j < array.Length - i - 1; j++)
                    {
                        if (array[j + 1] < array[j])
                        {
                            decimal temp = array[j + 1];
                            array[j + 1] = array[j];
                            array[j] = temp;
                        }
                    }
                }
                //if (array.Length % 2 != 0)
                //{
                //    return array[endIndex];
                //}
                int mid1 = endIndex / 2;
                int mid3 = array.Length % 2 == 0 ? (endIndex + mid1) : (endIndex + mid1 + 1);
                decimal median = array.Length % 2 == 0 ? (array[endIndex] + array[endIndex - 1]) / 2 : array[endIndex];
                decimal median1 = endIndex % 2 == 0 && endIndex != 0 ? (array[mid1] + array[mid1 - 1]) / 2 : array[mid1];
                decimal median3 = endIndex % 2 == 0 && endIndex != 0 ? (array[mid3] + array[mid3 - 1]) / 2 : array[mid3];
                if (array.Length == 1)
                {
                    mid3 = 0;
                }
                decimal lower = median1 - Decimal.Parse("1.5") * (median3 - median1);
                decimal upper = median3 + Decimal.Parse("1.5") * (median3 - median1); ;
                listmedian.Add("median", median);
                listmedian.Add("median1", median1);
                listmedian.Add("median3", median3);
                listmedian.Add("lower", lower);
                listmedian.Add("upper", upper);
            }
            return listmedian;
        }
        /// <summary>
        /// 取得统计数据（最大值、最小值、平均值、最大超标倍数、最大超标日期、实测范围）
        /// </summary>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="parameters">类型</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetStatisticalData(string[] portIds, string[] factors, string[] parameters, DateTime dateStart, DateTime dateEnd, int pageSize, int pageNo,
            out int recordTotal, string orderBy = "PointId")
        {
            DataTable dtMaxMinAvgData = new DataTable();//最大值、最小值、平均值
            DataTable dtMaxMultipleNew = new DataTable();//最大超标倍数
            DataTable dtStandardDateNew = new DataTable();//最大超标日期
            DataTable dtFoundRange = new DataTable();//实测范围
            DataTable dtOverStandard = new DataTable();//超标率
            List<string> strList = new List<string>();
            string[] strParameters = { "Value_Avg", "Value_Max", "Value_Min" };
            var z2 = parameters.Intersect(strParameters);//parameters为总的数据类型，strParameters自定义为最大值、最小值、平均值的数据类型
            foreach (var i in z2)
            {
                strList.Add(i.ToString());
            }

            g_MonitoringPointWater = Singleton<MonitoringPointWaterService>.GetInstance();
            portIds = portIds.Distinct().ToArray();
            Dictionary<string, string> siteTypeByPointIdsList = GetSiteTypeByPointIds(portIds);//根据站点Id数组获取站点Id和站点类型对应键值对

            DataTable dtFactorNew = CreateSequentialByFactor(factors);//按因子生成综合数据表 此表作为通用表结构
            DataView auditData = new DataView();
            if (parameters.Contains("Value_Range") || parameters.Contains("Value_Over"))
            {
                auditData = GetHourDataPager(portIds, factors, dateStart, dateEnd, int.MaxValue, 0, out recordTotal, orderBy);
            }
            #region 最大值、最小值、平均值
            DataTable dt = HourData.GetStatisticalData(portIds, factors, dateStart, dateEnd).Table;//最大值、最小值、平均值的数据源

            DataTable dtNew = dt.Clone();//数据源复制到新的dtNew表中 因为同一个测点和因子有多个数据要多个数据相加   同一个测点个个因子的平均值、最大值、最小值要相加最为最后因子的真实数据
            foreach (string point in portIds)
            {
                foreach (string factor in factors)
                {
                    DataRow drNew = dtNew.NewRow();
                    drNew["PointId"] = point.ToString();
                    drNew["PollutantCode"] = factor.ToString();
                    decimal avgValue = 0;
                    decimal maxValue = 0;
                    decimal minValue = 0;
                    DataRow[] drs = dt.Select(string.Format("PointId='{0}' and PollutantCode='{1}'", point, factor));
                    if (drs.Length > 0)//有数据  数据相加
                    {
                        for (int i = 0; i < drs.Length; i++)
                        {
                            if (drs[i]["Value_Avg"] != DBNull.Value)
                            {
                                avgValue += Convert.ToDecimal(drs[i]["Value_Avg"]);
                            }
                            if (drs[i]["Value_Max"] != DBNull.Value)
                            {
                                maxValue += Convert.ToDecimal(drs[i]["Value_Max"]);
                            }
                            if (drs[i]["Value_Min"] != DBNull.Value)
                            {
                                minValue += Convert.ToDecimal(drs[i]["Value_Min"]);
                            }
                        }

                        drNew["Value_Avg"] = avgValue.ToString();
                        drNew["Value_Max"] = maxValue.ToString();
                        drNew["Value_Min"] = minValue.ToString();
                    }
                    else//没数据 默认为空
                    {
                        drNew["Value_Avg"] = DBNull.Value;
                        drNew["Value_Max"] = DBNull.Value;
                        drNew["Value_Min"] = DBNull.Value;
                    }
                    dtNew.Rows.Add(drNew);

                }
            }
            if (strList.Count > 0)
            {
                dtMaxMinAvgData = AddNewRowToDataTableByFactor(portIds, factors, strList.ToArray(), dtFactorNew, dtNew, siteTypeByPointIdsList);//最大值 最小值 平均值数据源
            }
            #endregion

            #region 最大超标倍数
            if (parameters.Contains("Value_Max_Standard"))
            {
                string[] strMaxValue = { "Value_Max" };
                dtMaxMultipleNew = GetMaxMultipleNewDataTable(dtFactorNew, strMaxValue, portIds, factors, dateStart, dateEnd, dtNew, siteTypeByPointIdsList);
            }
            #endregion

            #region 最大值超标日期
            if (parameters.Contains("Value_Max_Date"))
            {
                dtStandardDateNew = GetStandardDateNewDataTable(dtFactorNew, siteTypeByPointIdsList, portIds, factors, dateStart, dateEnd);
            }
            #endregion

            #region 实测范围
            if (parameters.Contains("Value_Range"))
            {
                dtFoundRange = GetFoundRangeNewDataTable(dtFactorNew, siteTypeByPointIdsList, portIds, factors, auditData);
            }
            #endregion

            #region 超标率
            if (parameters.Contains("Value_Over"))
            {
                dtOverStandard = GetOverStandardNewDataTable(dtFactorNew, siteTypeByPointIdsList, portIds, factors, auditData);
            }
            #endregion

            DataTable dtAllData = UnionDataTable(dtMaxMinAvgData, dtMaxMultipleNew, dtStandardDateNew, dtFoundRange, dtOverStandard);//拼接数据表

            recordTotal = dtAllData.Rows.Count;


            DataTable dtData = GetDataPagerByPageSize(dtAllData, pageSize, pageNo, "PointId");

            return dtData.DefaultView;

        }

        /// <summary>
        /// 生成综合统计表 
        /// </summary>
        /// <param name="pollutantCodes">因子数据</param>
        /// <returns>返回的表的列如：SiteTypeName、PointId、Type、w01001、w01009.....</returns>
        private DataTable CreateSequentialByFactor(string[] pollutantCodes)
        {
            DataTable dtNew = new DataTable();

            dtNew.Columns.Add("SiteTypeName", typeof(string));
            dtNew.Columns.Add("PointId", typeof(int));
            dtNew.Columns.Add("Type", typeof(string));
            //拼接字段
            for (int i = 0; i < pollutantCodes.Length; i++)
            {
                dtNew.Columns.Add(pollutantCodes[i], typeof(string));//各个因子 拼接成为列
            }

            dtNew.Columns.Add("blankspaceColumn");//空白列
            return dtNew;
        }


        /// <summary>
        /// 最大超标倍数数据源
        /// </summary>
        /// <param name="dtFactorNew">表结构</param>
        /// <param name="parameters">参数类型</param>
        /// <param name="portIds">测点组</param>
        /// <param name="factors">因子组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <param name="dtNew">最大值、最小值、平均值的数据表结构</param>
        /// <param name="siteTypeByPointIdsList">获取测点类型数组字典</param>
        /// <returns></returns>
        private DataTable GetMaxMultipleNewDataTable(DataTable dtFactorNew, string[] parameters, string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd, DataTable dtNew, Dictionary<string, string> siteTypeByPointIdsList)
        {

            DataTable dtMaxMultiple1 = dtFactorNew.Clone();//返回最大值的数据源
            DataTable dtMaxMultipleNew = dtFactorNew.Clone(); //最大超标倍数表结构
            DataTable dtMaxMultiple = AddNewRowToDataTableByFactor(portIds, factors, parameters, dtMaxMultiple1, dtNew, siteTypeByPointIdsList);//最大值数据源

            if (dtMaxMultiple.DefaultView.Count > 0)
            {
                DataTable dtQualityOriginal = GetQualityNewDataTable(dtFactorNew, portIds, factors);//最大超标倍数数据源
                foreach (DataRow dr in dtMaxMultiple.Rows)
                {
                    DataRow drQualityNew = dtMaxMultipleNew.NewRow();
                    drQualityNew["SiteTypeName"] = dr["SiteTypeName"].ToString();
                    drQualityNew["PointId"] = int.Parse(dr["PointId"].ToString());
                    drQualityNew["Type"] = "最大超标倍数";
                    foreach (string factorName in factors)
                    {
                        if (dtMaxMultipleNew.Columns.Contains(factorName))
                        {
                            DataRow[] drQualityOriginal = dtQualityOriginal.Select(string.Format("PointId='{0}'", dr["PointId"]));
                            if (drQualityOriginal.Length > 0)
                            {
                                //最大值数据的判断
                                if (dr[factorName] != DBNull.Value && dr[factorName].ToString() != "0")
                                {
                                    //上限 下限数据的判断
                                    if (drQualityOriginal[0][factorName] != DBNull.Value && drQualityOriginal[0][factorName].ToString() != "" && drQualityOriginal[0][factorName].ToString() != "0")
                                    {
                                        //最大值-超标限值（上限）溶解氧是下限
                                        drQualityNew[factorName] = System.Math.Abs((Convert.ToDecimal(dr[factorName]) - Convert.ToDecimal(drQualityOriginal[0][factorName])) / Convert.ToDecimal(drQualityOriginal[0][factorName]) * Convert.ToDecimal(0.01)).ToString();
                                    }
                                    //else
                                    //{
                                    //    drQualityNew[factorName] = 0;
                                    //}
                                }
                                //else
                                //{
                                //    drQualityNew[factorName] = 0;
                                //}
                            }
                        }
                    }

                    dtMaxMultipleNew.Rows.Add(drQualityNew);

                }

            }

            #region 因子保留小数位

            //因子保留小数位

            string[] FactorCodes = factors;
            for (int i = 0; i < dtMaxMultipleNew.Rows.Count; i++)
            {
                foreach (string factorCode in FactorCodes)
                {
                    dtMaxMultipleNew.Rows[i][factorCode].ToString();
                    int DecimalNum = 3;
                    if (m_WaterPollutantService.GetPollutantInfo(factorCode) != null)
                    {
                        if (!string.IsNullOrWhiteSpace(m_WaterPollutantService.GetPollutantInfo(factorCode).PollutantDecimalNum))
                        {
                            DecimalNum = Convert.ToInt32(m_WaterPollutantService.GetPollutantInfo(factorCode).PollutantDecimalNum);
                        }

                    }
                    //value 需要进行小数位处理的数据 类型Decimal
                    if (dtMaxMultipleNew.Rows[i][factorCode] != DBNull.Value)
                    {
                        dtMaxMultipleNew.Rows[i][factorCode] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dtMaxMultipleNew.Rows[i][factorCode]), DecimalNum);
                    }
                }
            }

            #endregion

            return dtMaxMultipleNew;

        }
        /// <summary>
        /// 最大超标倍数数据
        /// </summary>
        /// <param name="dtFactorNew">要填充数据的表</param>
        /// <param name="portIds">测点</param>
        /// <param name="factors">因子</param>
        /// <returns></returns>
        private DataTable GetQualityNewDataTable(DataTable dtFactorNew, string[] portIds, string[] factors)
        {
            DataTable dtQualityNew = dtFactorNew.Clone();//上限 下限值和最大值 最小值 平均值是同一个字段的表

            WaterQualityClass enumQualityType = WaterQualityClass.Three;//默认是三类
            WaterPointCalWQType enumCalWQType = WaterPointCalWQType.River;//默认河流

            DataView waterQuality = m_WaterAnalyzeDAL.GetWaterAnalyzeData(portIds);//获取站点对应的水质类型数据（几类水、是湖泊还是河流）

            DataTable dtWaterQuality = waterQuality.ToTable();
            foreach (DataRow drWaterQuality in dtWaterQuality.Rows)
            {

                DataRow drQualityNew = dtQualityNew.NewRow();//新建表填充数据行
                //判断类别
                if (drWaterQuality["IEQI"] != DBNull.Value)
                {
                    string qiType = drWaterQuality["IEQI"].ToString();
                    string calEQIType = string.Empty;
                    switch (qiType)
                    {
                        case "1": enumQualityType = WaterQualityClass.One; break;
                        case "2": enumQualityType = WaterQualityClass.Two; break;
                        case "3": enumQualityType = WaterQualityClass.Three; break;
                        case "4": enumQualityType = WaterQualityClass.Four; break;
                        case "5": enumQualityType = WaterQualityClass.Five; break;
                        case "6": enumQualityType = WaterQualityClass.BadFive; break;
                        default: enumQualityType = WaterQualityClass.Three; break;
                    }
                }
                else
                {
                    enumQualityType = WaterQualityClass.Three;
                }

                //判断是河流还是 湖泊
                if (drWaterQuality["CalEQIType"] != DBNull.Value)
                {
                    string calEQIType = drWaterQuality["CalEQIType"].ToString();
                    switch (calEQIType)
                    {
                        case "河流": enumCalWQType = WaterPointCalWQType.River; break;
                        case "湖泊": enumCalWQType = WaterPointCalWQType.Lake; break;
                        default: enumCalWQType = WaterPointCalWQType.River; break;
                    }

                }
                else
                {
                    enumCalWQType = WaterPointCalWQType.River;
                }

                IQueryable<EQIConcentrationLimitEntity> limit = EQIService.RetrieveWaterConcentrationList(enumQualityType, factors, enumCalWQType);
                DataTable dtLimit = ConvertToDataTable(limit);//上限、下限的数据源

                #region 新表填充数据

                drQualityNew["PointId"] = drWaterQuality["PointId"].ToString();

                foreach (string factorName in factors)
                {
                    if (dtQualityNew.Columns.Contains(factorName))
                    {
                        DataRow[] drLimt = dtLimit.Select(string.Format("pollutantCode='{0}'", factorName));//从查询的上限、下限的数据源检索数据

                        if (drLimt.Length > 0)
                        {
                            //除了溶解氧是下限其它的因子都是上限
                            if (!factorName.Equals("溶解氧"))
                            {
                                drQualityNew[factorName] = drLimt[0]["Upper"].ToString();
                            }
                            else
                            {
                                drQualityNew[factorName] = drLimt[0]["Low"].ToString();
                            }
                        }
                    }
                }

                dtQualityNew.Rows.Add(drQualityNew);

                #endregion
            }

            return dtQualityNew;

        }

        /// <summary>
        /// 最大值超标日期
        /// </summary>
        /// <param name="dtFactorNew">表结构</param>
        /// <param name="siteTypeByPointIdsList">获取测点类型数组字典</param>
        /// <param name="portIds">测点组</param>
        /// <param name="factors">因子组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        private DataTable GetStandardDateNewDataTable(DataTable dtFactorNew, Dictionary<string, string> siteTypeByPointIdsList, string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd)
        {
            DataTable dtStandardDateNew = dtFactorNew.Clone();//最大值超标日期表结构
            DataTable dtStandardDate = HourData.GetStatisticalMaxTstampData(portIds, factors, dateStart, dateEnd).Table;//最大值、最小值、平均值的数据源
            foreach (string point in portIds)
            {
                DataRow drStandardDateNew = dtStandardDateNew.NewRow();
                drStandardDateNew["SiteTypeName"] = (siteTypeByPointIdsList.ContainsKey(point))
                          ? siteTypeByPointIdsList[point] : string.Empty;//站点类型名称??地表水 类型
                drStandardDateNew["PointId"] = int.Parse(point);
                drStandardDateNew["Type"] = "最大超标日期";
                foreach (string pollutantCode in factors)//因子
                {
                    if (dtStandardDateNew.Columns.Contains(pollutantCode))
                    {
                        DataRow[] drNe = dtStandardDate.Select(string.Format("PointId='{0}' and PollutantCode='{1}'", point, pollutantCode));
                        if (drNe.Length > 0)
                        {
                            if (drNe[0]["PollutantValue"] != DBNull.Value)
                            {
                                drStandardDateNew[pollutantCode] = drNe[0]["Tstamp"].ToString();//时间值

                            }
                        }
                    }

                }

                dtStandardDateNew.Rows.Add(drStandardDateNew);//添加新行

            }

            return dtStandardDateNew;

        }

        /// <summary>
        /// 实测范围数据
        /// </summary>
        /// <param name="dtFactorNew">表结构</param>
        /// <param name="siteTypeByPointIdsList">获取测点类型数组字典</param>
        /// <param name="portIds">测点组</param>
        /// <param name="factors">因子组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        private DataTable GetFoundRangeNewDataTable(DataTable dtFactorNew, Dictionary<string, string> siteTypeByPointIdsList, string[] portIds, string[] factors, DataView auditData)
        {

            DataTable dtFoundRange = dtFactorNew.Clone();//实测范围表结构

            foreach (string point in portIds)//测点
            {
                DataRow drFoundRange = dtFoundRange.NewRow();
                drFoundRange["SiteTypeName"] = (siteTypeByPointIdsList.ContainsKey(point))
                          ? siteTypeByPointIdsList[point] : string.Empty;//站点类型名称??地表水 类型
                drFoundRange["PointId"] = int.Parse(point);
                drFoundRange["Type"] = "实测范围";

                DataRow[] drNe = auditData.ToTable().Select(string.Format("PointId='{0}'", point));
                foreach (string pollutantCode in factors)//因子
                {
                    if (dtFoundRange.Columns.Contains(pollutantCode))
                    {
                        if (drNe.Length > 0)
                        {
                            DateTime valueStarTime = DateTime.TryParse(drNe[drNe.Length - 1]["Tstamp"].ToString(), out valueStarTime) ? valueStarTime : DateTime.Now;
                            DateTime valueEndTime = DateTime.TryParse(drNe[0]["Tstamp"].ToString(), out valueEndTime) ? valueEndTime : DateTime.Now;

                            //因为时间是按降序的 所以第一个是最大的时间
                            drFoundRange[pollutantCode] = string.Format("{0:MM/dd HH:mm}", valueStarTime) + "<br/>~<br/>" + string.Format("{0:MM-dd HH:mm}", valueEndTime);//时间值
                        }

                    }
                }

                dtFoundRange.Rows.Add(drFoundRange);
            }

            return dtFoundRange;

        }
        /// <summary>
        /// 超标率数据
        /// </summary>
        /// <param name="dtFactorNew">表结构</param>
        /// <param name="siteTypeByPointIdsList">获取测点类型数组字典</param>
        /// <param name="portIds">测点组</param>
        /// <param name="factors">因子组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        private DataTable GetOverStandardNewDataTable(DataTable dtFactorNew, Dictionary<string, string> siteTypeByPointIdsList, string[] portIds, string[] factors, DataView auditData)
        {

            DataTable dtFoundRange = dtFactorNew.Clone();//超标率表结构
            foreach (string point in portIds)//测点
            {
                DataRow drFoundRange = dtFoundRange.NewRow();
                drFoundRange["SiteTypeName"] = (siteTypeByPointIdsList.ContainsKey(point))
                          ? siteTypeByPointIdsList[point] : string.Empty;//站点类型名称??地表水 类型
                drFoundRange["PointId"] = int.Parse(point);
                drFoundRange["Type"] = "超标率";

                //DataRow[] drNe = auditData.ToTable().Select(string.Format("PointId='{0}'", point));
                foreach (string pollutantCode in factors)//因子
                {
                    if (dtFoundRange.Columns.Contains(pollutantCode))
                    {
                        DataRow[] drNew = auditData.ToTable().Select(string.Format("PointId='{0}' and {1} is not null", point, pollutantCode));
                        DataRow[] drAudit = auditData.ToTable().Select(string.Format("PointId='{0}' and {1} ='H' and {2} is not null", point, pollutantCode + "_AuditFlag", pollutantCode));
                        decimal auditCount = Convert.ToDecimal(drAudit.Count());
                        if (drNew.Length > 0)
                        {
                            decimal Total = Convert.ToDecimal(drNew.Count());
                            if (auditCount != 0)
                            {
                                drFoundRange[pollutantCode] = (auditCount / Total * 100).ToString("0.0") + "%";
                            }
                            else
                            {
                                drFoundRange[pollutantCode] = "0%";
                            }
                        }

                    }
                }

                dtFoundRange.Rows.Add(drFoundRange);
            }

            return dtFoundRange;

        }
        /// <summary>
        /// 为表添加数据行
        /// </summary>
        /// <param name="pointIds">测点数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="parameters">时间周期数组</param>
        /// <param name="dtNew">要添加数据行的表</param>
        /// <param name="dtReport">提供数据的表</param>
        /// <param name="siteTypeByPointIdsList">获取测点类型数组字典</param>
        private DataTable AddNewRowToDataTableByFactor(string[] pointIds, string[] factors, string[] parameters,
            DataTable dtNew, DataTable dtReport, Dictionary<string, string> siteTypeByPointIdsList)
        {
            pointIds = pointIds.Distinct().ToArray();
            //Dictionary<string, string> siteTypeByPointIdsList = GetSiteTypeByPointIds(pointIds);//根据站点Id数组获取站点Id和站点类型对应键值对

            for (int i = 0; i < pointIds.Length; i++)
            {
                foreach (string pa in parameters)//拼接的数组 平均值 最大值 最小值
                {
                    //string pointId = pointIds[i];
                    int pointId = int.Parse(pointIds[i]);

                    DataRow drNew = dtNew.NewRow();
                    DataRow[] drsReport = dtReport.Select(string.Format("PointId='{0}'", pointId));//查询制定测点的数据
                    DataTable dtNewReport = drsReport.CopyToDataTable();//转化为表数据
                    drNew["SiteTypeName"] = (siteTypeByPointIdsList.ContainsKey(pointId.ToString()))
                          ? siteTypeByPointIdsList[pointId.ToString()] : string.Empty;//站点类型名称??地表水 类型
                    drNew["PointId"] = pointId;
                    if (pa.Equals("Value_Avg"))
                    {
                        drNew["Type"] = "平均值";
                    }
                    else if (pa.Equals("Value_Max"))
                    {
                        drNew["Type"] = "最大值";

                    }
                    else if (pa.Equals("Value_Min"))
                    {
                        drNew["Type"] = "最小值";
                    }

                    foreach (string pollutantCode in factors)//因子
                    {
                        if (dtNew.Columns.Contains(pollutantCode))
                        {
                            DataRow[] drNe = dtNewReport.Select(string.Format("PollutantCode='{0}'", pollutantCode));
                            if (drNe.Length > 0)
                            {
                                if (drNe[0][pa] != DBNull.Value)
                                {
                                    drNew[pollutantCode] = drNe[0][pa].ToString();//浓度值

                                }
                            }
                        }

                    }

                    dtNew.Rows.Add(drNew);//添加新行
                }
            }

            #region 因子保留小数位

            //因子保留小数位

            string[] FactorCodes = factors;
            for (int i = 0; i < dtNew.Rows.Count; i++)
            {
                foreach (string factorCode in FactorCodes)
                {
                    dtNew.Rows[i][factorCode].ToString();
                    int DecimalNum = 3;
                    if (m_WaterPollutantService.GetPollutantInfo(factorCode) != null)
                    {
                        if (!string.IsNullOrWhiteSpace(m_WaterPollutantService.GetPollutantInfo(factorCode).PollutantDecimalNum))
                        {
                            DecimalNum = Convert.ToInt32(m_WaterPollutantService.GetPollutantInfo(factorCode).PollutantDecimalNum);
                        }

                    }
                    //value 需要进行小数位处理的数据 类型Decimal
                    if (dtNew.Rows[i][factorCode] != DBNull.Value)
                    {
                        dtNew.Rows[i][factorCode] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dtNew.Rows[i][factorCode]), DecimalNum);
                    }
                }
            }

            #endregion

            return dtNew;

        }

        /// <summary>
        /// 获取分页后的数据
        /// </summary>
        /// <param name="dtOld">原始表</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="orderBy">排序方式（如：PointId）</param>
        /// <returns></returns>
        private DataTable GetDataPagerByPageSize(DataTable dtOld, int pageSize, int pageNo, string orderBy)
        {
            if (dtOld != null)
            {
                int startIndex = pageSize * pageNo;//要返回的数据的开始位置(从0开始)
                int endIndex = pageSize * (pageNo + 1);//要返回的数据的结束位置(数据取到小于这个值)
                DataTable dtReturn = dtOld.Clone();
                endIndex = (endIndex > dtOld.Rows.Count) ? dtOld.Rows.Count : endIndex;//如果结束位置大于最大行数，则改为最大行数的值
                orderBy = (orderBy != null) ? orderBy : string.Empty;
                string[] orderByValues = orderBy.Split(',');
                bool isOrderByContains = true;
                foreach (string orderByValue in orderByValues)
                {
                    isOrderByContains = dtOld.Columns.Contains(orderByValue);
                }
                if (isOrderByContains)
                {
                    dtOld.DefaultView.Sort = orderBy;
                    dtOld = dtOld.DefaultView.ToTable();
                }
                for (int i = startIndex; i < endIndex; i++)
                {
                    DataRow dr = dtOld.Rows[i];
                    dtReturn.Rows.Add(dr.ItemArray);
                }
                return dtReturn;
            }
            return null;
        }


        /// <summary>
        /// 根据站点Id数组获取站点Id和站点类型对应键值对
        /// </summary>
        /// <param name="pointIds">站点Id数组</param>
        /// <returns></returns>
        private Dictionary<string, string> GetSiteTypeByPointIds(string[] pointIds)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            if (pointIds == null || pointIds.Length == 0)
            {
                return dictionary;
            }
            DictionaryService dictionaryService = Singleton<DictionaryService>.GetInstance();
            IQueryable<MonitoringPointEntity> monitorPointQueryable = g_MonitoringPointWater.RetrieveListByPointIds(pointIds); //根据站点ID数组获取站点
            IQueryable<V_CodeMainItemEntity> codeMainItemQueryable = dictionaryService.RetrieveList(DictionaryType.Water, "地表水站点类型");//获取城市类型
            foreach (MonitoringPointEntity monitorPointEntity in monitorPointQueryable)
            {
                if (!dictionary.ContainsKey(monitorPointEntity.PointId.ToString()))
                {
                    string siteTypeName = codeMainItemQueryable.Where(t => t.ItemGuid.Equals(monitorPointEntity.SiteTypeUid))
                            .Select(t => t.ItemText).FirstOrDefault();//站点类型名称
                    dictionary.Add(monitorPointEntity.PointId.ToString(), siteTypeName);
                }
            }
            return dictionary;
        }

        /// <summary>
        /// 取得全部查询数据(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetHourExportData(string[] portIds, IList<IPollutant> factors, DateTime dateStart, DateTime dateEnd, string orderBy = "Tstamp,PointId")
        {
            if (HourData != null)
                return HourData.GetExportData(portIds, factors, dateStart, dateEnd, orderBy);
            return null;
        }
        /// <summary>
        /// 取得全部查询数据(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetHourExportData(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd, string orderBy = "Tstamp,PointId")
        {
            if (HourData != null)
                return HourData.GetExportData(portIds, factors, dateStart, dateEnd, orderBy);
            return null;
        }
        /// <summary>
        /// 获得小时审核前数据
        /// </summary>
        /// <param name="pointId"></param>
        /// <param name="tstamp"></param>
        /// <returns></returns>
        public DataView GetHourValue(string pointId, string tstamp)
        {
            if (HourData != null)
                return HourData.GetHourValue(pointId, tstamp);
            return null;
        }
        /// <summary>
        /// 获得小时审核原因
        /// </summary>
        /// <param name="pointId"></param>
        /// <param name="tstamp"></param>
        /// <returns></returns>
        public DataView GetHourReason(string pointId, string tstamp)
        {
            if (HourData != null)
                return HourData.GetHourReason(pointId, tstamp);
            return null;
        }

	public DataView GetHourExportData(string[] portIds, string[] factors, DateTime dateStart, DateTime dateEnd, string lv, string orderBy = "Tstamp,PointId")
        {
            if (HourData != null)
                return HourData.GetExportData(portIds, factors, dateStart, dateEnd, lv, orderBy);
            return null;
        }


        /// <summary>
        /// 获得数据比对分析审核原因
        /// </summary>
        /// <param name="pointId"></param>
        /// <param name="tstamp"></param>
        /// <param name="factorCode"></param>
        /// <returns></returns>
        public DataView GetCompareReason(string tstamp, string factorCode)
        {
            if (HourData != null)
                return HourData.GetCompareReason(tstamp, factorCode);
            return null;
        }

        /// <summary>
        /// 报表数据
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetHourExportDataReport(string[] portIds, IList<IPollutant> factors, DateTime dateStart, DateTime dateEnd, string orderBy = "Tstamp,PointId")
        {
            if (HourData != null)
            {
                g_MonitoringPointWater = Singleton<MonitoringPointWaterService>.GetInstance();
                DataTable dt = HourData.GetExportDataReport(portIds, factors, dateStart, dateEnd, orderBy).ToTable();
                dt.Columns.Add("PortName", typeof(string));
                foreach (DataRow row in dt.Rows)
                {
                    int pointId = Convert.ToInt32(row["PointId"]);
                    row["PortName"] = g_MonitoringPointWater.RetrieveEntityByPointId(pointId).MonitoringPointName;
                }
                return dt.DefaultView;
            }
            return null;
        }

        /// <summary>
        /// 取得行转列数据总行数
        /// </summary>
        /// <param name="portIds">测点数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <returns></returns>
        public int GetHourAllDataCount(string[] portIds, DateTime dateStart, DateTime dateEnd)
        {
            if (HourData != null)
                return HourData.GetAllDataCount(portIds, dateStart, dateStart);
            return 0;
        }

        /// <summary>
        /// 蓝藻日报预警（小时数据前一天8点到第二天12点数据平均值）
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetBlueAlgaeDayData(string[] portIds, string[] factors
    , DateTime dtStart, DateTime dtEnd)
        {
            PollutantCodeRepository g_Repository = new PollutantCodeRepository();
            g_MonitoringPointWater = Singleton<MonitoringPointWaterService>.GetInstance();
            EQIConcentrationLimitEntity limitTree = null;

            DataTable dt = HourData.GetBlueAlgaeDayData(portIds, factors, dtStart, dtEnd).ToTable();
            dt.Columns.Add("DataNo", typeof(int));
            dt.Columns.Add("PortName", typeof(string));
            DataTable limitDT = new DataTable();
            limitDT = dt.Copy();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int pointId = Convert.ToInt32(dt.Rows[i]["PointId"]);
                limitDT.Rows[i]["PortName"] = g_MonitoringPointWater.RetrieveEntityByPointId(pointId).MonitoringPointName;
                limitDT.Rows[i]["DataNo"] = i + 1;

                DataRow row = limitDT.NewRow();
                row["DataNo"] = i + 1;
                row["PointID"] = pointId;
                row["portName"] = "超标倍数";
                for (int j = 0; j < factors.Length; j++)
                {
                    decimal value = dt.Rows[i][factors[j]] != DBNull.Value ? Convert.ToDecimal(dt.Rows[i][factors[j]]) : -99999;
                    if (value != -99999)
                    {
                        PollutantCodeEntity pollutant = g_Repository.RetrieveFirstOrDefault(p => p.PollutantCode == factors[j]);
                        limitDT.Rows[i][factors[j]] = DecimalExtension.GetRoundValue(value, pollutant.DecimalDigit.Value);
                        //if (i == 0)
                        //{
                        limitTree = EQIService.RetrieveWaterConcentration(WaterQualityClass.Three, factors[j], WaterPointCalWQType.River);
                        //}
                        if (limitTree != null && limitTree.Low.Value != 0)
                            row[factors[j]] = DecimalExtension.GetRoundValue((value - limitTree.Low.Value) / limitTree.Low.Value, pollutant.DecimalDigit.Value);
                    }
                }
                limitDT.Rows.Add(row);
            }
            return limitDT.DefaultView;
        }

        /// <summary>
        /// 水源地蓝藻日报预警上报数据（小时数据前一天8点到第二天12点数据平均值）
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetUploadBlueAlgaeDayData(int PointType, string[] factors
    , DateTime dtStart, DateTime dtEnd)
        {
            PollutantCodeRepository g_Repository = new PollutantCodeRepository();
            g_MonitoringPointWater = Singleton<MonitoringPointWaterService>.GetInstance();
            string[] portIds = null;

            //0是水源地；1是浮标站
            if (PointType == 0)
                portIds = g_MonitoringPointWater.RetrieveMPListByWaterSource().Select(x => x.PointId.ToString()).ToArray();
            else if (PointType == 1) portIds = g_MonitoringPointWater.RetrieveMPListByFloat().Select(x => x.PointId.ToString()).ToArray();
            if (portIds != null)
            {
                DataTable dt = HourData.GetBlueAlgaeDayData(portIds, factors, dtStart, dtEnd).ToTable();//数据源
                dt.Columns.Add("PortCode1", typeof(string)).SetOrdinal(0);
                dt.Columns.Add("PortCode2", typeof(string)).SetOrdinal(1);
                dt.Columns.Add("PortName", typeof(string)).SetOrdinal(2);
                dt.Columns.Add("EditUser", typeof(string)).SetOrdinal(dt.Columns.Count - 1);
                dt.Columns.Add("PhoneNum", typeof(string)).SetOrdinal(dt.Columns.Count - 1);
                dt.Columns.Add("AuditUser", typeof(string)).SetOrdinal(dt.Columns.Count - 1);
                dt.Columns.Add("Detail", typeof(string)).SetOrdinal(dt.Columns.Count - 1);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int pointId = Convert.ToInt32(dt.Rows[i]["PointId"]);
                    dt.Rows[i]["PortCode1"] = "320500";
                    dt.Rows[i]["PortCode2"] = PointType == 0 ? "YYS" + (i < 10 ? "0" + (i + 1) : (i + 1).ToString()) : "FB" + (i + 1);
                    dt.Rows[i]["PortName"] = g_MonitoringPointWater.RetrieveEntityByPointId(pointId).MonitoringPointName;
                    dt.Rows[i]["EditUser"] = "徐诗琴";
                    dt.Rows[i]["PhoneNum"] = "68338036";
                    dt.Rows[i]["AuditUser"] = "吕清";
                    dt.Rows[i]["Detail"] = "";
                }
                return dt.DefaultView;
            }
            //dt.Columns.
            else
                return new DataView();
        }
        /// <summary>
        /// 水源地蓝藻日报预警（小时数据前一天8点到第二天12点数据平均值）
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetBlueAlgaeDayData(int PointType, string[] portIds, string[] factors
    , DateTime dtStart, DateTime dtEnd, string Name)
        {
            PollutantCodeRepository g_Repository = new PollutantCodeRepository();
            g_MonitoringPointWater = Singleton<MonitoringPointWaterService>.GetInstance();

            //0是水源地；1是浮标站
            if (portIds != null)
            {
                DataTable dt = HourData.GetBlueAlgaeDayData(portIds, factors, dtStart, dtEnd).ToTable();//数据源
                dt.Columns.Add("PortCode1", typeof(string)).SetOrdinal(0);
                dt.Columns.Add("PortCode2", typeof(string)).SetOrdinal(1);
                dt.Columns.Add("PortName", typeof(string)).SetOrdinal(2);
                dt.Columns.Add("EditUser", typeof(string)).SetOrdinal(dt.Columns.Count - 1);
                dt.Columns.Add("PhoneNum", typeof(string)).SetOrdinal(dt.Columns.Count - 1);
                dt.Columns.Add("AuditUser", typeof(string)).SetOrdinal(dt.Columns.Count - 1);
                dt.Columns.Add("Detail", typeof(string)).SetOrdinal(dt.Columns.Count - 1);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int pointId = Convert.ToInt32(dt.Rows[i]["PointId"]);
                    dt.Rows[i]["PortCode1"] = "320500";
                    dt.Rows[i]["PortCode2"] = PointType == 0 ? "YYS" + (i < 10 ? "0" + (i + 1) : (i + 1).ToString()) : "FB" + (i + 1);
                    dt.Rows[i]["PortName"] = g_MonitoringPointWater.RetrieveEntityByPointId(pointId).MonitoringPointName;
                    dt.Rows[i]["EditUser"] = Name;
                    dt.Rows[i]["PhoneNum"] = "68338036";
                    dt.Rows[i]["AuditUser"] = "吕清";
                    dt.Rows[i]["Detail"] = "";
                }
                return dt.DefaultView;
            }
            //dt.Columns.
            else
                return new DataView();
        }
        /// <summary>
        /// 把IQueryable转化为DataTable
        /// </summary>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        private DataTable ConvertToDataTable(IEnumerable enumerable)
        {
            var dataTable = new DataTable();
            foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(typeof(EQIConcentrationLimitEntity)))
            {
                dataTable.Columns.Add(pd.Name);
            }
            foreach (EQIConcentrationLimitEntity item in enumerable)
            {
                var Row = dataTable.NewRow();

                foreach (PropertyDescriptor dp in TypeDescriptor.GetProperties(typeof(EQIConcentrationLimitEntity)))
                {
                    Row[dp.Name] = dp.GetValue(item);
                }
                dataTable.Rows.Add(Row);
            }
            return dataTable;
        }

        /// <summary>
        /// 拼接数据表
        /// </summary>
        /// <param name="dtArray">表数组</param>
        /// <returns></returns>
        private DataTable UnionDataTable(params DataTable[] dtArray)
        {
            DataTable dtAll = null;
            if (dtArray == null || dtArray.Length == 0)
            {
                return dtAll;
            }
            dtAll = dtArray[0];
            for (int i = 1; i < dtArray.Length; i++)
            {
                DataTable dt = dtArray[i];
                if (dtAll == null || dtAll.Rows.Count == 0)
                {
                    dtAll = dt;
                    continue;
                }
                else if (dt != null && dt.Rows.Count > 0)
                {
                    dtAll = dtAll.AsEnumerable().Union(dt.AsEnumerable()).CopyToDataTable();
                }
            }
            return dtAll;
        }
    }
}
