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

namespace SmartEP.WebUI.Pages.EnvAir.ChartAjaxRequest
{
    /// <summary>
    /// 名称：RealTimeData.ashx.cs
    /// 创建人：徐龙超
    /// 创建日期：2015-09-29
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：实时小时数据【Chart】
    /// 虚拟分页类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class RealTimeData : IHttpHandler
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
                string[] FactorCode = context.Request["FactorCode"] != null ? context.Request["FactorCode"].ToString().Split(';') : null;
                DateTime DtBegin = context.Request["DtBegin"] != null ? Convert.ToDateTime(context.Request["DtBegin"]) : DateTime.Now;
                DateTime DtEnd = context.Request["DtEnd"] != null ? Convert.ToDateTime(context.Request["DtEnd"]) : DateTime.Now;
                string radlDataType = context.Request["radlDataType"] != null ? context.Request["radlDataType"].ToString() : "";
                string pageType = context.Request["pageType"] != null ? context.Request["pageType"].ToString() : "";
                int PageSize = context.Request["PageSize"] != null ? Convert.ToInt32(context.Request["PageSize"].ToString()) : 100;
                int PageNo = context.Request["PageNo"] != null ? Convert.ToInt32(context.Request["PageNo"].ToString()) : 0;
                ChartType = context.Request["ChartType"] != null ? context.Request["ChartType"].ToString() : "spline";
                context.Response.ContentType = "text/plain";
                context.Response.Write(GetJsonData(PointID, FactorCode, DtBegin, DtEnd, radlDataType, pageType, PageSize, PageNo, AllPointID));
            }
            catch
            {

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
        private string GetJsonData(string[] PointID, string[] FactorCode, DateTime DtBegin, DateTime DtEnd, string radlDataType, string pageType, int PageSize, int PageNo, string[] AllPointID)
        {
            string data = "";
            if (!radlDataType.Equals(""))
            {
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
                        if (radlDataType.Equals("Min5") || radlDataType.Equals("Min1") || radlDataType.Equals("RealTime")) fomatter = "formatter:formatterMinuteData";
                        g_IInfectantDALService = MonitoringDataAir.GetInfectantDALService(SmartEP.Core.Enums.EnumMapping.ParseEnum<PollutantDataType>(radlDataType));
                        allData = g_IInfectantDALService.GetDataPager(AllPointID, FactorCode, DtBegin, DtEnd, PageSize, PageNo, out total);
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
                    if (FirstLoad == AllPointID.Length) FirstLoad = 0;
                }
                catch
                {
                    //dv = g_IInfectantDALService.GetDataPager(PointID, FactorCode, DtBegin, DtEnd, PageSize, PageNo, out total);
                }
                data = jsonData.GetChartData(dv, FactorCode, "Tstamp", fomatter, ChartType, pageType, fomatter + "_XLabel", FactorCode.Length == 1 ? excessiveList : null, PointID != null ? Convert.ToInt32(PointID[0]) : -9999);
            }
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
    }
}