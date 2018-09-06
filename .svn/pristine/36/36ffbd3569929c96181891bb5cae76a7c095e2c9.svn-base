using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.Data.SqlServer.Common.WebControl;
using SmartEP.Service.AutoMonitoring.Water;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Utilities.AdoData;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SmartEP.Service.DataAnalyze.Water.DataQuery
{
    /// <summary>
    /// 名称：DataExportService.cs
    /// 创建人：刘长敏
    /// 创建日期：2016-01-05
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：批量数据导出
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class DataExportService
    {
        /// <summary>
        /// 审核小时数据服务层
        /// </summary>
        DataQueryByHourService auditHourService = null;

        /// <summary>
        /// 审核日数据服务层
        /// </summary>
        DataQueryByDayService auditDayService = null;

        /// <summary>
        /// 审核周数据服务层
        /// </summary>
        DataQueryByWeekService auditWeekService = null;

        /// <summary>
        /// 审核月数据服务层
        /// </summary>
        DataQueryByMonthService auditMonthService = null;

        /// <summary>
        /// 审核季数据服务层
        /// </summary>
        DataQueryBySeasonService auditSeasonService = null;

        /// <summary>
        /// 审核年数据服务层
        /// </summary>
        DataQueryByYearService auditYearService = null;

        /// <summary>
        /// 原始小时数据服务层
        /// </summary>
        InfectantBy60Service originalHourService = null;

        /// <summary>
        /// 站点类
        /// </summary>
        MonitoringPointWaterService g_MonitoringPointWater = null;
        #region 小时数据
        /// <summary>
        /// 获取原始小时数据
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="factors">因子</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>小时数据的比对视图</returns>
        public DataView GetHourCompareOriginal(string[] portIds, string[] factors, DateTime dtBegin, DateTime dtEnd, string orderBy = "Tstamp,PointId")
        {
            g_MonitoringPointWater = Singleton<MonitoringPointWaterService>.GetInstance();
            originalHourService = Singleton<InfectantBy60Service>.GetInstance();
            DataTable dtOriginalData = originalHourService.GetExportData(portIds, factors, dtBegin, dtEnd, orderBy).Table;//获取原始小时数据
            //dtOriginalData.Columns.Add("portName", typeof(string)).SetOrdinal(0);

            //for (int k = 0; k < dtOriginalData.Rows.Count; k++)//给DataType、portName字段赋值
            //{
            //    int pointId = Convert.ToInt32(dtOriginalData.Rows[k]["PointId"]);
            //    dtOriginalData.Rows[k]["portName"] = g_MonitoringPointWater.RetrieveEntityByPointId(pointId).MonitoringPointName;//获取站点名称
            //}
            return dtOriginalData.DefaultView;
        }


        /// <summary>
        /// 获得小时审核原因
        /// </summary>
        /// <param name="pointId"></param>
        /// <param name="tstamp"></param>
        /// <returns></returns>
        public DataView GetHourReason(string pointId, string tstamp)
        {

            auditHourService = Singleton<DataQueryByHourService>.GetInstance();
            g_MonitoringPointWater = Singleton<MonitoringPointWaterService>.GetInstance();
            return auditHourService.GetHourReason(pointId, tstamp);
        }
        /// <summary>
        /// 获得小时审核前数据
        /// </summary>
        /// <param name="pointId"></param>
        /// <param name="tstamp"></param>
        /// <returns></returns>
        public DataView GetHourValue(string pointId, string tstamp)
        {
            auditHourService = Singleton<DataQueryByHourService>.GetInstance();
            g_MonitoringPointWater = Singleton<MonitoringPointWaterService>.GetInstance();
            return auditHourService.GetHourValue(pointId, tstamp);

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
            auditHourService = Singleton<DataQueryByHourService>.GetInstance();
            g_MonitoringPointWater = Singleton<MonitoringPointWaterService>.GetInstance();
            return auditHourService.GetCompareReason(tstamp, factorCode);
        }

        /// <summary>
        /// 获取审核小时数据
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="factors">因子</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>小时数据的比对视图</returns>
        public DataView GetHourCompareAudit(string[] portIds, string[] factors, DateTime dtBegin, DateTime dtEnd, string orderBy = "Tstamp,PointId")
        {
            auditHourService = Singleton<DataQueryByHourService>.GetInstance();
            g_MonitoringPointWater = Singleton<MonitoringPointWaterService>.GetInstance();
            DataTable dtAuditData = auditHourService.GetHourExportData(portIds, factors, dtBegin, dtEnd, orderBy).Table;//获取审核小时数据
            //dtAuditData.Columns.Add("portName", typeof(string)).SetOrdinal(0);

            //for (int j = 0; j < dtAuditData.Rows.Count; j++)//给DataType、portName字段赋值
            //{
            //    int pointId = Convert.ToInt32(dtAuditData.Rows[j]["PointId"]);
            //    dtAuditData.Rows[j]["portName"] = g_MonitoringPointWater.RetrieveEntityByPointId(pointId).MonitoringPointName;//获取站点名称
            //}

            return dtAuditData.DefaultView;
        }

        public DataView GetHourCompareAudit(string[] portIds, string[] factors, DateTime dtBegin, DateTime dtEnd, string lv, string orderBy = "Tstamp,PointId")
        {
            auditHourService = Singleton<DataQueryByHourService>.GetInstance();
            g_MonitoringPointWater = Singleton<MonitoringPointWaterService>.GetInstance();
            DataTable dtAuditData = auditHourService.GetHourExportData(portIds, factors, dtBegin, dtEnd, lv, orderBy).Table;//获取审核小时数据
            //dtAuditData.Columns.Add("portName", typeof(string)).SetOrdinal(0);

            //for (int j = 0; j < dtAuditData.Rows.Count; j++)//给DataType、portName字段赋值
            //{
            //    int pointId = Convert.ToInt32(dtAuditData.Rows[j]["PointId"]);
            //    dtAuditData.Rows[j]["portName"] = g_MonitoringPointWater.RetrieveEntityByPointId(pointId).MonitoringPointName;//获取站点名称
            //}

            return dtAuditData.DefaultView;
        }

        #endregion




        #region 日数据
        /// <summary>
        /// 获取日数据
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="factors">因子</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>日数据的比对视图</returns>
        public DataView GetDayCompare(string[] portIds, string[] factors, DateTime dtBegin, DateTime dtEnd, string orderBy = "PointId,DateTime")
        {
            auditDayService = Singleton<DataQueryByDayService>.GetInstance();
            g_MonitoringPointWater = Singleton<MonitoringPointWaterService>.GetInstance();
            DataTable dtAuditData = auditDayService.GetDayExportData(portIds, factors, dtBegin, dtEnd, orderBy).Table;//获取审核小时数据
            //dtAuditData.Columns.Add("portName", typeof(string)).SetOrdinal(0);

            //for (int j = 0; j < dtAuditData.Rows.Count; j++)//给DataType、portName字段赋值
            //{
            //    int pointId = Convert.ToInt32(dtAuditData.Rows[j]["PointId"]);
            //    dtAuditData.Rows[j]["portName"] = g_MonitoringPointWater.RetrieveEntityByPointId(pointId).MonitoringPointName;//获取站点名称
            //}
            return dtAuditData.DefaultView;
        }

        public DataView GetDayCompare(string[] portIds, string[] factors, DateTime dtBegin, DateTime dtEnd, string LVTable, string orderBy = "PointId,DateTime")
        {
            auditDayService = Singleton<DataQueryByDayService>.GetInstance();
            g_MonitoringPointWater = Singleton<MonitoringPointWaterService>.GetInstance();
            DataTable dtAuditData = auditDayService.GetDayExportData(portIds, factors, dtBegin, dtEnd, LVTable, orderBy).Table;//获取审核小时数据
            //dtAuditData.Columns.Add("portName", typeof(string)).SetOrdinal(0);

            //for (int j = 0; j < dtAuditData.Rows.Count; j++)//给DataType、portName字段赋值
            //{
            //    int pointId = Convert.ToInt32(dtAuditData.Rows[j]["PointId"]);
            //    dtAuditData.Rows[j]["portName"] = g_MonitoringPointWater.RetrieveEntityByPointId(pointId).MonitoringPointName;//获取站点名称
            //}
            return dtAuditData.DefaultView;
        }
        #endregion

        #region 周数据
        /// <summary>
        /// 获取周数据
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="factors">因子</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>周数据的比对视图</returns>
        public DataView GetWeekCompare(string[] portIds, string[] factors, int yearFrom, int weekOfYearFrom, int yearTo, int weekOfYearTo, string orderBy = "PointId,Year,WeekOfYear")
        {
            auditWeekService = Singleton<DataQueryByWeekService>.GetInstance();
            g_MonitoringPointWater = Singleton<MonitoringPointWaterService>.GetInstance();
            DataTable dtAuditData = auditWeekService.GetWeekExportData(portIds, factors, yearFrom, weekOfYearFrom, yearTo, weekOfYearTo, orderBy).Table;//获取审核周数据
            //dtAuditData.Columns.Add("portName", typeof(string)).SetOrdinal(0);

            //for (int j = 0; j < dtAuditData.Rows.Count; j++)//给DataType、portName字段赋值
            //{
            //    int pointId = Convert.ToInt32(dtAuditData.Rows[j]["PointId"]);
            //    dtAuditData.Rows[j]["portName"] = g_MonitoringPointWater.RetrieveEntityByPointId(pointId).MonitoringPointName;//获取站点名称
            //}
            return dtAuditData.DefaultView;
        }
        #endregion

        #region 月数据
        /// <summary>
        /// 获取月数据
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="factors">因子</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>月数据的比对视图</returns>
        public DataView GetMonthCompare(string[] portIds, string[] factors, int yearFrom, int monthOfYearFrom, int yearTo, int monthOfYearTo, string orderBy = "PointId,Year,MonthOfYear")
        {
            auditMonthService = Singleton<DataQueryByMonthService>.GetInstance();
            g_MonitoringPointWater = Singleton<MonitoringPointWaterService>.GetInstance();
            DataTable dtAuditData = auditMonthService.GetMonthExportData(portIds, factors, yearFrom, monthOfYearFrom, yearTo, monthOfYearTo, orderBy).Table;//获取审核周数据
            //dtAuditData.Columns.Add("portName", typeof(string)).SetOrdinal(0);

            //for (int j = 0; j < dtAuditData.Rows.Count; j++)//给portName字段赋值
            //{
            //    int pointId = Convert.ToInt32(dtAuditData.Rows[j]["PointId"]);
            //    dtAuditData.Rows[j]["portName"] = g_MonitoringPointWater.RetrieveEntityByPointId(pointId).MonitoringPointName;//获取站点名称
            //}
            return dtAuditData.DefaultView;
        }
        #endregion

        #region 季数据
        /// <summary>
        /// 获取季数据的比对
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="factors">因子</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>季数据的比对视图</returns>
        public DataView GetSeasonCompare(string[] portIds, string[] factors, int yearFrom, int seasonOfYearFrom, int yearTo, int seasonOfYearTo, string orderBy = "PointId,Year,SeasonOfYear")
        {
            auditSeasonService = Singleton<DataQueryBySeasonService>.GetInstance();
            g_MonitoringPointWater = Singleton<MonitoringPointWaterService>.GetInstance();
            DataTable dtAuditData = auditSeasonService.GetSeasonExportData(portIds, factors, yearFrom, seasonOfYearFrom, yearTo, seasonOfYearTo, orderBy).Table;//获取审核季数据
            //dtAuditData.Columns.Add("portName", typeof(string)).SetOrdinal(0);

            //for (int j = 0; j < dtAuditData.Rows.Count; j++)//给DataType、portName字段赋值
            //{
            //    int pointId = Convert.ToInt32(dtAuditData.Rows[j]["PointId"]);
            //    dtAuditData.Rows[j]["portName"] = g_MonitoringPointWater.RetrieveEntityByPointId(pointId).MonitoringPointName;//获取站点名称
            //}

            return dtAuditData.DefaultView;
        }
        #endregion

        #region 年数据
        /// <summary>
        /// 获取年数据
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="factors">因子</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>年数据的比对视图</returns>
        public DataView GetYearCompare(string[] portIds, string[] factors, int yearFrom, int yearTo, string orderBy = "PointId,Year")
        {
            auditYearService = Singleton<DataQueryByYearService>.GetInstance();
            g_MonitoringPointWater = Singleton<MonitoringPointWaterService>.GetInstance();
            DataTable dtAuditData = auditYearService.GetYearExportData(portIds, factors, yearFrom, yearTo, orderBy).Table;//获取审核年数据
            //dtAuditData.Columns.Add("portName", typeof(string)).SetOrdinal(0);

            //for (int j = 0; j < dtAuditData.Rows.Count; j++)//给DataType、portName字段赋值
            //{
            //    int pointId = Convert.ToInt32(dtAuditData.Rows[j]["PointId"]);
            //    dtAuditData.Rows[j]["portName"] = g_MonitoringPointWater.RetrieveEntityByPointId(pointId).MonitoringPointName;//获取站点名称
            //}
            return dtAuditData.DefaultView;
        }
        #endregion
    }
}
