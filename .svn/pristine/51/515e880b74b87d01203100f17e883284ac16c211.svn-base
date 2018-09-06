using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using SmartEP.Service.AutoMonitoring.Air;
using SmartEP.Service.AutoMonitoring.Interfaces;
using SmartEP.Core.Enums;
using SmartEP.Core.Interfaces;
using SmartEP.Service.BaseData.BusinessRule;
//using SmartEP.DomainModel.BaseData;
using Newtonsoft.Json;
using SmartEP.WebControl.CbxRsm;

namespace SmartEP.WebUI.Pages.EnvAir.ChartAjaxRequest
{
    /// <summary>
    /// 名称：DayAvg.ashx.cs
    /// 创建人：
    /// 创建日期：2016-07-09
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：对照环境分析【Chart】
    /// 虚拟分页类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class DayAvg : IHttpHandler
    {
        private IInfectantDALService g_IInfectantDALService = null;
        private HighChartJsonData jsonData = new HighChartJsonData();
        private ExcessiveSettingService excessiveService = new ExcessiveSettingService();
        public string ChartType = "spline";
        public static DataView allData = new DataView();
        public static int FirstLoad = 0;
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string[] PointID = context.Request["PointID"] != null ? context.Request["PointID"].ToString().Split(';') : null;
                string[] AllPointID = context.Request["AllPointID"] != null ? context.Request["AllPointID"].ToString().Split(';') : null;
                string strFactorCodeS = context.Request["FactorCode"] != null ? context.Request["FactorCode"].ToString() : null;

                IList<IPollutant> listFactors = new List<IPollutant>();
                IList<RsmFactor> listRsmFactors = new List<RsmFactor>();
                listRsmFactors = JsonConvert.DeserializeObject<IList<RsmFactor>>(strFactorCodeS);
                foreach (RsmFactor obj in listRsmFactors)
                    listFactors.Add(obj);
                string[] FactorCode = listFactors.Select(p => p.PollutantCode).ToArray();
                //DateTime DtBegin = context.Request["DtBegin"] != null ? Convert.ToDateTime(context.Request["DtBegin"]) : DateTime.Now;
                //DateTime DtEnd = context.Request["DtEnd"] != null ? Convert.ToDateTime(context.Request["DtEnd"]) : DateTime.Now;
                string radlDataType = context.Request["radlDataType"] != null ? context.Request["radlDataType"].ToString() : "";
                string pageType = context.Request["pageType"] != null ? context.Request["pageType"].ToString() : "";
                int PageSize = context.Request["PageSize"] != null ? Convert.ToInt32(context.Request["PageSize"].ToString()) : 100;
                DateTime currentDate = context.Request["currentDate"] != null ? Convert.ToDateTime(context.Request["currentDate"]) : DateTime.Now;
                decimal aRatio = context.Request["aRatio"] != null ? Convert.ToDecimal(context.Request["aRatio"].ToString()) : (Decimal)0.5;
                decimal bRatio = context.Request["bRatio"] != null ? Convert.ToDecimal(context.Request["bRatio"].ToString()) : (Decimal)0.2;
                //int PageNo = context.Request["PageNo"] != null ? Convert.ToInt32(context.Request["PageNo"].ToString()) : 0;
                ChartType = context.Request["ChartType"] != null ? context.Request["ChartType"].ToString() : "spline";
                context.Response.ContentType = "text/plain";


                context.Response.Write(GetJsonData(PointID, listFactors, currentDate, aRatio, bRatio, pageType, AllPointID));
            }
            catch (Exception ex)
            {
                var var1 = ex.ToString();
            }
        }

        /// <summary>
        /// 获取实时数据Json格式
        /// </summary>
        /// <param name="PointID"></param>
        /// <param name="FactorCode"></param>
        /// <param name="DtBegin"></param>
        /// <param name="DtEnd"></param>
        /// <param name="radlDataType"></param>
        /// <param name="pageType"></param>
        /// <returns></returns>
        private string GetJsonData(string[] PointID, IList<IPollutant> factors, DateTime currentDate, decimal aRatio, decimal bRatio, string pageType, string[] allPointID)
        {
            string[] FactorCode = (from obj in factors select obj.PollutantCode).ToArray();
            string data = "";

            int total = 0;
            string fomatter = "formatter:formatterHourData";
            IQueryable<ExcessiveSettingInfo> excessiveList = null;//超标限值
            try
            {
                excessiveList = excessiveService.RetrieveListByFactor(ApplicationValue.Air, FactorCode, PointID);
            }
            catch
            {
            }
            DataView dv = new DataView();
            try
            {
                if (FirstLoad == 0)
                {
                    //if (radlDataType.Equals("Min5") || radlDataType.Equals("Min1") || radlDataType.Equals("RealTime")) fomatter = "formatter:formatterMinuteData";
                    //g_IInfectantDALService = MonitoringDataAir.GetInfectantDALService(SmartEP.Core.Enums.EnumMapping.ParseEnum<PollutantDataType>(radlDataType));
                    //allData = g_IInfectantDALService.GetDataPager(AllPointID, FactorCode, DtBegin, DtEnd, PageSize, PageNo, out total);

                    //allData = g_IInfectantDALService.GetDataPager(AllPointID, FactorCode, DtBegin, DtEnd, PageSize, PageNo, out total);
                    allData = GetDataSource(allPointID, factors, currentDate, aRatio, bRatio);
                    DataTable allDataDt = allData.Table;
                    //删除状态列
                    foreach (IPollutant item in factors)
                    {
                        allDataDt.Columns.Remove(item.PollutantCode + "_Status");
                        allDataDt.Columns.Remove(item.PollutantCode + "_Mark");
                    }
                    allData = allDataDt.DefaultView;
                    allData.RowFilter = "";
                    allData.RowFilter = "PointID=" + PointID[0];
                    dv = allData;
                }
                else
                {
                    allData.RowFilter = "";
                    allData.RowFilter = "PointID=" + PointID[0];
                    dv = allData;
                }
                FirstLoad++;
                if (FirstLoad == allPointID.Length) FirstLoad = 0;
            }
            catch
            {
                //dv = g_IInfectantDALService.GetDataPager(PointID, FactorCode, DtBegin, DtEnd, PageSize, PageNo, out total);
            }
            data = jsonData.GetChartData(dv, FactorCode, "Tstamp", fomatter, ChartType, pageType, fomatter + "_XLabel", FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999);

            return data;

            #region 注释
            //string data = "";
            //if (!radlDataType.Equals(""))
            //{
            //    int total = 0;
            //    string fomatter = "formatter:formatterHourData";
            //    DataView allData = new DataView();
            //    IQueryable<ExcessiveSettingInfo> excessiveList = excessiveService.RetrieveListByFactor(ApplicationValue.Air, FactorCode, PointID);
            //    DataView dv = new DataView();
            //    try
            //    {
            //        if (radlDataType.Equals("Min5") || radlDataType.Equals("Min1") || radlDataType.Equals("RealTime")) fomatter = "formatter:formatterMinuteData";
            //        g_IInfectantDALService = MonitoringDataAir.GetInfectantDALService(SmartEP.Core.Enums.EnumMapping.ParseEnum<PollutantDataType>(radlDataType));
            //        allData = g_IInfectantDALService.GetDataPager(PointID, FactorCode, DtBegin, DtEnd, PageSize, PageNo, out total);
            //    }
            //    catch
            //    {
            //    }
            //    data = jsonData.GetChartData(allData, FactorCode, "Tstamp", fomatter, ChartType, pageType, fomatter + "_XLabel", FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999);
            //}
            //return data;       
            #endregion
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 取得数据源
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtBegion"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        private DataView GetDataSource(string[] portIds, IList<IPollutant> factors, DateTime currentDate, decimal aRatio, decimal bRatio)
        {
            SmartEP.Service.AutoMonitoring.Air.InfectantBy60Service InfectantBy60BLL = new InfectantBy60Service();
            DataView currentYearDv = new DataView(); // 存储今年当前日期,各个测点7天的日均值数据
            DataView lastYearStatisticsDv = new DataView(); // 存储去年当前日期,前后各七天, 合计15天的统计数据
            DataView currentYearStatisticsDv = new DataView(); // 存储今年当前日期,各个测点7天的统计数据
            DateTime dtBegion = currentDate.AddDays(-6);
            DateTime dtEnd = currentDate.AddDays(1).AddSeconds(-1);

            currentYearDv = InfectantBy60BLL.GetDayAvgData(portIds, factors, dtBegion, dtEnd);

            currentYearStatisticsDv = InfectantBy60BLL.GetStatisticalData(portIds, factors, dtBegion, dtEnd);
            lastYearStatisticsDv = InfectantBy60BLL.GetStatisticalData(portIds, factors, dtBegion.AddYears(-1).AddDays(-1), dtEnd.AddYears(-1).AddDays(7));
            //开始计算各站点, 各因子的明天预测值的上限和下限
            //2016-7-8下限 = （同比数据*a+环比数据*（1-a））*（1-b）
            //2016-7-8上限 = （同比数据*a+环比数据*（1-a））*（1+b）

            //最终结果是一个范围，如：6.23~6.89
            //同比数据计算方式=2015-6-30~2015-7-14天的小时数据的均值（同比的前后7天+当天数据）
            //环比数据计算方式=2016-7-1~2016-7-7的小时数据的均值（前7天数据）

            //a默认0.5
            //b默认0.2
            DataTable currentYearDt = currentYearDv.Table;
            //decimal aRatio = Convert.ToDecimal(txt_a.Text); //计算用的系数
            //decimal bRatio = Convert.ToDecimal(txt_b.Text);//计算用的系数
            foreach (string strPortId in portIds)
            {
                int intPortId = Convert.ToInt32(strPortId);
                DataRow dr = currentYearDt.NewRow();
                DataRow dr2 = currentYearDt.NewRow();
                dr["PointId"] = intPortId;
                dr2["PointId"] = intPortId;
                dr["Tstamp"] = currentDate.AddDays(1);
                dr2["Tstamp"] = currentDate.AddDays(1);
                decimal? lastYearAvg = null; //同比数据
                decimal? currentYearAvg = null; //环比数据
                decimal? upperLimit = null; //明天预测值的上限
                decimal? lowwerLimit = null;//明天预测值的下限
                foreach (IPollutant iPollutant in factors)
                {
                    lastYearAvg = null;
                    currentYearAvg = null;
                    upperLimit = null;
                    lowwerLimit = null;
                    lastYearStatisticsDv.RowFilter = string.Format("PointId={0} AND PollutantCode='{1}'", intPortId, iPollutant.PollutantCode);
                    if (lastYearStatisticsDv.Count > 0 && lastYearStatisticsDv[0]["Value_Avg"] != DBNull.Value)
                    {
                        lastYearAvg = Convert.ToDecimal(lastYearStatisticsDv[0]["Value_Avg"].ToString());
                    }

                    currentYearStatisticsDv.RowFilter = string.Format("PointId={0} AND PollutantCode='{1}'", intPortId, iPollutant.PollutantCode);
                    if (currentYearStatisticsDv.Count > 0 && currentYearStatisticsDv[0]["Value_Avg"] != DBNull.Value)
                    {
                        currentYearAvg = Convert.ToDecimal(lastYearStatisticsDv[0]["Value_Avg"].ToString());
                    }
                    if (lastYearAvg != null)
                    {
                        //2016-7-8下限 = （同比数据*a+环比数据*（1-a））*（1-b）
                        //2016-7-8上限 = （同比数据*a+环比数据*（1-a））*（1+b）
                        upperLimit = (lastYearAvg * aRatio + currentYearAvg * (1 - aRatio)) * (1 - bRatio);
                        lowwerLimit = (lastYearAvg * aRatio + currentYearAvg * (1 - aRatio)) * (1 + bRatio);
                    }
                    dr[iPollutant.PollutantCode] = upperLimit;
                    dr2[iPollutant.PollutantCode] = lowwerLimit;
                }
                currentYearDt.Rows.Add(dr);
                currentYearDt.Rows.Add(dr2);
            }
            DataView myDv = currentYearDt.DefaultView;
            myDv.Sort = "PointId,Tstamp";
            return myDv;
        }
    }
}