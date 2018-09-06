using SmartEP.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SmartEP.Core.Enums;
using SmartEP.Service.Core.Enums;
using SmartEP.Service.AutoMonitoring.Air;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Core.Generic;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.MonitoringBusinessRepository.Air;
using SmartEP.DomainModel.BaseData;

namespace SmartEP.Service.DataAnalyze.Air.DataQuery
{
    public class DataCompare
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
        DataQueryByYearService auditYearService = new DataQueryByYearService();

        /// <summary>
        /// 原始小时数据服务层
        /// </summary>
        InfectantBy60Service originalHourService = null;

        /// <summary>
        /// 站点类
        /// </summary>
        MonitoringPointAirService g_MonitoringPointAir = null;

        #region 小时数据
        /// <summary>
        /// 取得小时数据（最大值、最小值、平均值）
        /// </summary>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetHourStatisticalData(string[] portIds, IList<IPollutant> factors, DateTime[,] dateTime, bool isNeedOriginalData = false)
        {
            auditHourService = Singleton<DataQueryByHourService>.GetInstance();
            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            DataTable newDataTable = null;
            List<DataTable> dtList = new List<DataTable>();

            for (int i = 0; i < dateTime.GetLength(0); i++)
            {
                DataTable dtStatisticalData = auditHourService.GetHourStatisticalData(portIds, factors, dateTime[i, 0], dateTime[i, 1]).Table;
                dtStatisticalData.Columns.Add("DataType", typeof(string)).SetOrdinal(1);
                dtStatisticalData.Columns.Add("DateTimeRange", typeof(string)).SetOrdinal(2);
                dtStatisticalData.Columns.Add("rowMack", typeof(int)).SetOrdinal(3);
                for (int j = 0; j < dtStatisticalData.Rows.Count; j++)
                {
                    if (i == 0)
                    {
                        dtStatisticalData.Rows[j]["DataType"] = "审核(基准)";
                        dtStatisticalData.Rows[j]["rowMack"] = j;
                    }
                    else
                    {
                        dtStatisticalData.Rows[j]["DataType"] = "审核(比对)";
                        dtStatisticalData.Rows[j]["rowMack"] = j;
                    }
                    dtStatisticalData.Rows[j]["DateTimeRange"] = dateTime[i, 0].ToString() + "至" + dateTime[i, 1].ToString();
                }
                dtList.Add(dtStatisticalData);

                if (isNeedOriginalData)//判断时候需要原始数据
                {
                    originalHourService = Singleton<InfectantBy60Service>.GetInstance();
                    DataTable dtOriginalData = originalHourService.GetStatisticalData(portIds, factors, dateTime[i, 0], dateTime[i, 1]).Table;
                    dtOriginalData.Columns.Add("DataType", typeof(string)).SetOrdinal(1);
                    dtOriginalData.Columns.Add("DateTimeRange", typeof(string)).SetOrdinal(2);
                    dtOriginalData.Columns.Add("rowMack", typeof(int)).SetOrdinal(3);
                    for (int k = 0; k < dtOriginalData.Rows.Count; k++)//给DataType、portName字段赋值
                    {
                        if (i == 0)
                        {
                            dtOriginalData.Rows[k]["DataType"] = "原始(基准)";
                            dtOriginalData.Rows[k]["rowMack"] = k;
                        }
                        else
                        {
                            dtOriginalData.Rows[k]["DataType"] = "原始(比对)";
                            dtOriginalData.Rows[k]["rowMack"] = k;
                        }
                        dtOriginalData.Rows[k]["DateTimeRange"] = dateTime[i, 0].ToString() + "至" + dateTime[i, 1].ToString();
                    }
                    dtList.Add(dtOriginalData);
                }
                if (newDataTable == null)
                {
                    newDataTable = dtStatisticalData.Clone();
                }
            }
            object[] obj = new object[newDataTable.Columns.Count];
            for (int i = 0; i < dtList.Count; i++)
            {
                DataTable dt = dtList[i];
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    dt.Rows[j].ItemArray.CopyTo(obj, 0);
                    newDataTable.Rows.Add(obj);
                }
            }
            newDataTable.DefaultView.Sort = "rowMack,DateTimeRange,DataType";
            return newDataTable.DefaultView;
        }

        /// <summary>
        /// 获取小时数据的比对
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="factors">因子</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>小时数据的比对视图</returns>
        public DataView GetHourCompareView(string[] portIds, string[] factors, DateTime[,] dateTime, int pageSize, int pageNo, out int recordTotal, int tabIndex, string orderBy = "Tstamp,PointId", bool isNeedOriginalData = false)
        {
            recordTotal = 0;
            auditHourService = Singleton<DataQueryByHourService>.GetInstance();
            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            DataTable dtAuditData = null;
            for (int i = 0; i < dateTime.GetLength(0); i++)
            {
                dtAuditData = auditHourService.GetHourDataPager(portIds, factors, dateTime[i, 0], dateTime[i, 1], pageSize, pageNo, out recordTotal, orderBy).Table;//获取审核小时数据
                dtAuditData.Columns.Add("portName", typeof(string)).SetOrdinal(0);

                for (int j = 0; j < dtAuditData.Rows.Count; j++)//给portName字段赋值
                {
                    int pointId = Convert.ToInt32(dtAuditData.Rows[j]["PointId"]);
                    dtAuditData.Rows[j]["portName"] = g_MonitoringPointAir.RetrieveEntityByPointId(pointId).MonitoringPointName;//获取站点名称
                }
            }
            return dtAuditData.DefaultView;
        }
        /// <summary>
        /// 获取原始审核小时数据的比对
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="factors">因子</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>小时数据的比对视图</returns>
        public DataView GetHourCompare(string[] portIds, string[] factors, DateTime dtBegin, DateTime dtEnd, DateTime dtFrom, DateTime dtTo, string type, int pageSize, int pageNo, out int recordTotal, int tabIndex, string orderBy = "PointId,tstamp desc")
        {
            recordTotal = 0;
            auditHourService = Singleton<DataQueryByHourService>.GetInstance();
            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            DataTable dtAuditData = null;

            dtAuditData = auditHourService.GetDataHourPager(portIds, factors, dtBegin, dtEnd, dtFrom, dtTo, type, pageSize, pageNo, out recordTotal, orderBy).Table;//获取审核小时数据
            //dtAuditData.Columns.Add("portName", typeof(string)).SetOrdinal(0);
            //dtAuditData.Columns.Add("rowMack", typeof(int)).SetOrdinal(1);

            //for (int j = 0; j < dtAuditData.Rows.Count; j++)//给DataType、portName字段赋值
            //{
            //    int pointId = Convert.ToInt32(dtAuditData.Rows[j]["PointId"]);
            //    dtAuditData.Rows[j]["portName"] = g_MonitoringPointAir.RetrieveEntityByPointId(pointId).MonitoringPointName;//获取站点名称
            //    dtAuditData.Rows[j]["rowMack"] = 1;
            //}
            return dtAuditData.DefaultView;
        }
        /// <summary>
        /// 获取小时数据的比对
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="factors">因子</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>小时数据的比对视图</returns>
        public DataView GetHourOtherCompare(string[] portIds, string[] factors, DateTime dtBegin, DateTime dtEnd, DateTime dtFrom, DateTime dtTo, string type, int pageSize, int pageNo, out int recordTotal, int tabIndex, string orderBy = "PointId,tstamp desc,Type")
        {
            recordTotal = 0;
            auditHourService = Singleton<DataQueryByHourService>.GetInstance();
            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            DataTable dtAuditData = null;
            DataTable dt1 = null;
            DataTable dt2 = null;

            if (dtFrom == dtTo)
            {
                dtAuditData = auditHourService.GetHourDataDF(portIds, factors, dtBegin, dtEnd, type, int.MaxValue, 0, orderBy).Table;//获取审核小时数据
            }
            else 
            {
                dtAuditData = auditHourService.GetHourDataDF(portIds, factors, dtFrom, dtTo, type, int.MaxValue, 0, orderBy).Table;//获取审核小时数据
            }
            dt2 = auditHourService.GetHourDataDF(portIds, factors, dtBegin, dtEnd, type, int.MaxValue, 0,  orderBy).Table;//获取审核小时数据
            
            for (int m = 0; m < dt2.Rows.Count; m++)
            {
                if (dt2.Rows[m]["Type"].ToString() == "审核")
                {
                    dt2.Rows[m]["Type"] = "审核(比对)";
                }
                if (dt2.Rows[m]["Type"].ToString() == "原始")
                {
                    dt2.Rows[m]["Type"] = "原始(比对)";
                }
            }

            for (int m = 0; m < dtAuditData.Rows.Count; m++)
            {
                if (dtAuditData.Rows[m]["Type"].ToString() == "审核")
                {
                    dtAuditData.Rows[m]["Type"] = "审核(基准)";
                }
                if (dtAuditData.Rows[m]["Type"].ToString() == "原始")
                {
                    dtAuditData.Rows[m]["Type"] = "原始(基准)";
                }
            }
            if (dtFrom == dtTo)
            {
                dtAuditData = dt2;
            }
            else 
            {
                dtAuditData.Merge(dt2, true);
            }
            
            recordTotal = dtAuditData.Rows.Count;
            //dtAuditData = auditHourService.GetHourData(portIds, factors, dtBegin, dtEnd, dtFrom, dtTo, type, pageSize, pageNo, out recordTotal, orderBy).Table;//获取审核小时数据
            //dtAuditData.Columns.Add("portName", typeof(string)).SetOrdinal(0);
            //dtAuditData.Columns.Add("rowMack", typeof(int)).SetOrdinal(1);

            //for (int j = 0; j < dtAuditData.Rows.Count; j++)//给DataType、portName字段赋值
            //{
            //    int pointId = Convert.ToInt32(dtAuditData.Rows[j]["PointId"]);
            //    dtAuditData.Rows[j]["portName"] = g_MonitoringPointAir.RetrieveEntityByPointId(pointId).MonitoringPointName;//获取站点名称
            //    dtAuditData.Rows[j]["rowMack"] = 1;
            //}
            //if (dtFrom != new DateTime() && dtTo != new DateTime())
            //{
            //    int a = 0;
            //    DataRow[] dtRow = dtAuditData.Select("tstamp>='" + dtBegin + "' and tstamp<='" + dtEnd + "'");   //2015
            //    DataRow[] drs = dtAuditData.Select("tstamp>='" + dtFrom + "' and tstamp<='" + dtTo + "'");   //2015
            //    for (int m = 0; m < dtRow.Length; m++)
            //    {
            //        if (dtRow[m]["Type"].ToString() == "审核")
            //        {
            //            dtRow[m]["Type"] = "审核(比对)";
            //        }
            //        if (dtRow[m]["Type"].ToString() == "原始")
            //        {
            //            dtRow[m]["Type"] = "原始(比对)";
            //        }
            //        a = m;
            //    }
            //    for (int i = 0; i < drs.Length; i++)
            //    {
            //        if (drs[i]["Type"].ToString() == "审核")
            //        {
            //            drs[i]["Type"] = "审核(基准)";
            //        }
            //        if (drs[i]["Type"].ToString() == "原始")
            //        {
            //            drs[i]["Type"] = "原始(基准)";
            //        }
            //    }
            //}
            return dtAuditData.DefaultView;
        }
        /// <summary>
        /// 获取小时数据的比对
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="factors">因子</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>小时数据的比对视图</returns>
        public DataView GetHourOtherCompareNew(string[] portIds, string[] factors, DateTime dtBegin, DateTime dtEnd, DateTime dtFrom, DateTime dtTo, string type, int pageSize, int pageNo, out int recordTotal, int tabIndex, string orderBy = "PointId,tstamp desc,Type")
        {
            recordTotal = 0;
            auditHourService = Singleton<DataQueryByHourService>.GetInstance();
            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            DataTable dtAuditData = null;
            DataTable dt1 = null;
            DataTable dt2 = null;

            //if (dtFrom == dtTo)
            //{
            //    dtAuditData = auditHourService.GetHourDataDF(portIds, factors, dtBegin, dtEnd, type, int.MaxValue, 0, orderBy).Table;//获取审核小时数据
            //}
            //else
            //{
            //    dtAuditData = auditHourService.GetHourDataDF(portIds, factors, dtFrom, dtTo, type, int.MaxValue, 0, orderBy).Table;//获取审核小时数据
            //}
            //dt2 = auditHourService.GetHourDataDF(portIds, factors, dtBegin, dtEnd, type, int.MaxValue, 0, orderBy).Table;//获取审核小时数据

            //for (int m = 0; m < dt2.Rows.Count; m++)
            //{
            //    if (dt2.Rows[m]["Type"].ToString() == "审核")
            //    {
            //        dt2.Rows[m]["Type"] = "审核(比对)";
            //    }
            //    if (dt2.Rows[m]["Type"].ToString() == "原始")
            //    {
            //        dt2.Rows[m]["Type"] = "原始(比对)";
            //    }
            //}

            //for (int m = 0; m < dtAuditData.Rows.Count; m++)
            //{
            //    if (dtAuditData.Rows[m]["Type"].ToString() == "审核")
            //    {
            //        dtAuditData.Rows[m]["Type"] = "审核(基准)";
            //    }
            //    if (dtAuditData.Rows[m]["Type"].ToString() == "原始")
            //    {
            //        dtAuditData.Rows[m]["Type"] = "原始(基准)";
            //    }
            //}
            //if (dtFrom == dtTo)
            //{
            //    dtAuditData = dt2;
            //}
            //else
            //{
            //    dtAuditData.Merge(dt2, true);
            //}

            //recordTotal = dtAuditData.Rows.Count;
            dtAuditData = auditHourService.GetHourData(portIds, factors, dtBegin, dtEnd, dtFrom, dtTo, type, pageSize, pageNo, out recordTotal, orderBy).Table;//获取审核小时数据
            dtAuditData.Columns.Add("portName", typeof(string)).SetOrdinal(0);
            dtAuditData.Columns.Add("rowMack", typeof(int)).SetOrdinal(1);

            for (int j = 0; j < dtAuditData.Rows.Count; j++)//给DataType、portName字段赋值
            {
                int pointId = Convert.ToInt32(dtAuditData.Rows[j]["PointId"]);
                dtAuditData.Rows[j]["portName"] = g_MonitoringPointAir.RetrieveEntityByPointId(pointId).MonitoringPointName;//获取站点名称
                dtAuditData.Rows[j]["rowMack"] = 1;
            }
            if (dtFrom != new DateTime() && dtTo != new DateTime())
            {
                int a = 0;
                DataRow[] dtRow = dtAuditData.Select("tstamp>='" + dtBegin + "' and tstamp<='" + dtEnd + "'");   //2015
                DataRow[] drs = dtAuditData.Select("tstamp>='" + dtFrom + "' and tstamp<='" + dtTo + "'");   //2015
                for (int m = 0; m < dtRow.Length; m++)
                {
                    if (dtRow[m]["Type"].ToString() == "审核")
                    {
                        dtRow[m]["Type"] = "审核(比对)";
                    }
                    if (dtRow[m]["Type"].ToString() == "原始")
                    {
                        dtRow[m]["Type"] = "原始(比对)";
                    }
                    a = m;
                }
                for (int i = 0; i < drs.Length; i++)
                {
                    if (drs[i]["Type"].ToString() == "审核")
                    {
                        drs[i]["Type"] = "审核(基准)";
                    }
                    if (drs[i]["Type"].ToString() == "原始")
                    {
                        drs[i]["Type"] = "原始(基准)";
                    }
                }
            }
            return dtAuditData.DefaultView;
        }
        #endregion

        #region 日数据
        /// <summary>
        /// 取得日数据（最大值、最小值、平均值）
        /// </summary>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetDayStatisticalData(string[] portIds, IList<IPollutant> factors, DateTime[,] dateTime, bool isNeedOriginalData = false)
        {
            auditDayService = Singleton<DataQueryByDayService>.GetInstance();
            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            DataTable newDataTable = null;
            List<DataTable> dtList = new List<DataTable>();

            for (int i = 0; i < dateTime.GetLength(0); i++)
            {
                DataTable dtStatisticalData = auditDayService.GetDayStatisticalData(portIds, factors, dateTime[i, 0], dateTime[i, 1]).Table;
                dtStatisticalData.Columns.Add("DataType", typeof(string)).SetOrdinal(1);
                dtStatisticalData.Columns.Add("DateTimeRange", typeof(string)).SetOrdinal(2);
                dtStatisticalData.Columns.Add("rowMack", typeof(string)).SetOrdinal(3);
                for (int j = 0; j < dtStatisticalData.Rows.Count; j++)
                {
                    if (i == 0)
                    {
                        dtStatisticalData.Rows[j]["DataType"] = "审核(基准)";
                        dtStatisticalData.Rows[j]["rowMack"] = j;
                    }
                    else
                    {
                        dtStatisticalData.Rows[j]["DataType"] = "审核(比对)";
                        dtStatisticalData.Rows[j]["rowMack"] = j;
                    }
                    dtStatisticalData.Rows[j]["DateTimeRange"] = dateTime[i, 0].ToString() + "至" + dateTime[i, 1].ToString();
                }
                dtList.Add(dtStatisticalData);

                if (newDataTable == null)
                {
                    newDataTable = dtStatisticalData.Clone();
                }
            }
            object[] obj = new object[newDataTable.Columns.Count];
            for (int i = 0; i < dtList.Count; i++)
            {
                DataTable dt = dtList[i];
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    dt.Rows[j].ItemArray.CopyTo(obj, 0);
                    newDataTable.Rows.Add(obj);
                }
            }
            newDataTable.DefaultView.Sort = "rowMack,DateTimeRange,DataType";
            return newDataTable.DefaultView;
        }

        /// <summary>
        /// 获取日数据的比对
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="factors">因子</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>日数据的比对视图</returns>
        public DataView GetDayCompareView(string[] portIds, string[] factors, DateTime[,] dateTime, int pageSize, int pageNo, out int recordTotal, int tabIndex, string orderBy = "PointId,DateTime", bool isNeedOriginalData = false)
        {
            recordTotal = 0;
            auditDayService = Singleton<DataQueryByDayService>.GetInstance();
            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            DataTable dtAuditData = new DataTable();
            for (int i = 0; i < dateTime.GetLength(0); i++)
            {
                dtAuditData = auditDayService.GetDayData(portIds, factors, dateTime[i, 0], dateTime[i, 1], pageSize, pageNo, out recordTotal, orderBy).Table;//获取审核天数据
                dtAuditData.Columns.Add("portName", typeof(string)).SetOrdinal(0);
                for (int j = 0; j < dtAuditData.Rows.Count; j++)//给portName字段赋值
                {
                    int pointId = Convert.ToInt32(dtAuditData.Rows[j]["PointId"]);
                    dtAuditData.Rows[j]["portName"] = g_MonitoringPointAir.RetrieveEntityByPointId(pointId).MonitoringPointName;//获取站点名称
                }

            }
            return dtAuditData.DefaultView;
        }
        /// <summary>
        /// 获取日数据的比对
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="factors">因子</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>日数据的比对视图</returns>
        public DataView GetDayCompare(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd, DateTime dtFrom, DateTime dtTo, int pageSize, int pageNo, out int recordTotal, int tabIndex, string orderBy = "PointId,DateTime desc", bool isNeedOriginalData = false)
        {
            recordTotal = 0;
            auditDayService = Singleton<DataQueryByDayService>.GetInstance();
            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();

            DataTable dtAuditData = null;
            DataTable dt2 = null;

            dtAuditData = auditDayService.GetDayDataDF(portIds, factors, dtStart, dtEnd, int.MaxValue, 0,  orderBy).Table;//获取审核日数据
            dt2 = auditDayService.GetDayDataDF(portIds, factors, dtFrom, dtTo, int.MaxValue, 0, orderBy).Table;//获取审核日数据

            //dtAuditData = auditDayService.GetDayData(portIds, factors, dtStart, dtEnd, dtFrom, dtTo, pageSize, pageNo, out recordTotal, orderBy).Table;//获取审核日数据

            dtAuditData.Columns.Add("portName", typeof(string)).SetOrdinal(0);
            dtAuditData.Columns.Add("rowMack", typeof(int)).SetOrdinal(1);
            IQueryable<MonitoringPointEntity> entity = g_MonitoringPointAir.RetrieveListByPointIds(portIds);
            for (int j = 0; j < dtAuditData.Rows.Count; j++)//给DataType、portName字段赋值
            {
                int pointId = Convert.ToInt32(dtAuditData.Rows[j]["PointId"]);
                //dtAuditData.Rows[j]["portName"] = g_MonitoringPointAir.RetrieveEntityByPointId(pointId).MonitoringPointName;//获取站点名称
                dtAuditData.Rows[j]["portName"] = entity.Where(x => x.PointId == pointId).Select(t => t.MonitoringPointName).FirstOrDefault();
                dtAuditData.Rows[j]["rowMack"] = 1;
            }

            dt2.Columns.Add("portName", typeof(string)).SetOrdinal(0);
            dt2.Columns.Add("rowMack", typeof(int)).SetOrdinal(1);
            for (int j = 0; j < dt2.Rows.Count; j++)//给DataType、portName字段赋值
            {
                int pointId = Convert.ToInt32(dt2.Rows[j]["PointId"]);
                //dtAuditData.Rows[j]["portName"] = g_MonitoringPointAir.RetrieveEntityByPointId(pointId).MonitoringPointName;//获取站点名称
                dt2.Rows[j]["portName"] = entity.Where(x => x.PointId == pointId).Select(t => t.MonitoringPointName).FirstOrDefault();
                dt2.Rows[j]["rowMack"] = 1;
            }

            for (int m = 0; m < dt2.Rows.Count; m++)
            {
                if (dt2.Rows[m]["Type"].ToString() == "审核")
                {
                    dt2.Rows[m]["Type"] = "审核(比对)";
                }
                if (dt2.Rows[m]["Type"].ToString() == "原始")
                {
                    dt2.Rows[m]["Type"] = "原始(比对)";
                }
            }

            for (int m = 0; m < dtAuditData.Rows.Count; m++)
            {
                if (dtAuditData.Rows[m]["Type"].ToString() == "审核")
                {
                    dtAuditData.Rows[m]["Type"] = "审核(基准)";
                }
                if (dtAuditData.Rows[m]["Type"].ToString() == "原始")
                {
                    dtAuditData.Rows[m]["Type"] = "原始(基准)";
                }
            }
            dtAuditData.Merge(dt2, true);
            recordTotal = dtAuditData.Rows.Count;

            //if (dtFrom != new DateTime() && dtTo != new DateTime())
            //{
            //    DataRow[] dtRow = dtAuditData.Select("DateTime>='" + dtStart + "' and DateTime<='" + dtEnd + "'");   //2015
            //    DataRow[] drs = dtAuditData.Select("DateTime>='" + dtFrom + "' and DateTime<='" + dtTo + "'");   //2015
            //    for (int m = 0; m < dtRow.Length; m++)
            //    {
            //        if (dtRow[m]["Type"].ToString() == "审核")
            //        {
            //            dtRow[m]["Type"] = "审核(基准)";
            //            dtRow[m]["rowMack"] = m;
            //        }
            //    }
            //    for (int i = 0; i < drs.Length; i++)
            //    {
            //        if (drs[i]["Type"].ToString() == "审核")
            //        {
            //            drs[i]["Type"] = "审核(比对)";
            //            drs[i]["rowMack"] = i;
            //        }
            //    }
            //}
            return dtAuditData.DefaultView;
        }
        #endregion

        #region 周数据
        /// <summary>
        /// 取得周数据（最大值、最小值、平均值）
        /// </summary>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="weekOfYearFrom">开始周数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="weekOfYearTo">结束周数</param>
        /// <returns></returns>
        public DataView GetWeekStatisticalData(string[] portIds, IList<IPollutant> factors, int[,] dateTime)
        {
            auditWeekService = Singleton<DataQueryByWeekService>.GetInstance();
            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            DataTable newDataTable = null;
            List<DataTable> dtList = new List<DataTable>();

            for (int i = 0; i < dateTime.GetLength(0); i++)
            {
                DataTable dtStatisticalData = auditWeekService.GetWeekStatisticalData(portIds, factors, dateTime[i, 0], dateTime[i, 1], dateTime[i, 2], dateTime[i, 3]).Table;
                dtStatisticalData.Columns.Add("DataType", typeof(string)).SetOrdinal(1);
                dtStatisticalData.Columns.Add("DateTimeRange", typeof(string)).SetOrdinal(2);
                dtStatisticalData.Columns.Add("rowMack", typeof(string));
                for (int j = 0; j < dtStatisticalData.Rows.Count; j++)
                {
                    if (i == 0)
                    {
                        dtStatisticalData.Rows[j]["DataType"] = "审核(基准)";
                        dtStatisticalData.Rows[j]["rowMack"] = j;
                    }
                    else
                    {
                        dtStatisticalData.Rows[j]["DataType"] = "审核(比对)";
                        dtStatisticalData.Rows[j]["rowMack"] = j;
                    }
                    dtStatisticalData.Rows[j]["DateTimeRange"] = dateTime[i, 0].ToString() + "年第" + dateTime[i, 1].ToString() + "周至" + dateTime[i, 2].ToString() + "年第" + dateTime[i, 3].ToString() + "周";
                }
                dtList.Add(dtStatisticalData);

                if (newDataTable == null)
                {
                    newDataTable = dtStatisticalData.Clone();
                }
            }
            object[] obj = new object[newDataTable.Columns.Count];
            for (int i = 0; i < dtList.Count; i++)
            {
                DataTable dt = dtList[i];
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    dt.Rows[j].ItemArray.CopyTo(obj, 0);
                    newDataTable.Rows.Add(obj);
                }
            }
            newDataTable.DefaultView.Sort = "rowMack,DateTimeRange,DataType";
            return newDataTable.DefaultView;
        }

        /// <summary>
        /// 获取周数据的比对
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="factors">因子</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>周数据的比对视图</returns>
        public DataView GetWeekCompareView(string[] portIds, string[] factors, int[,] dateTime, int pageSize, int pageNo, out int recordTotal, int tabIndex, string orderBy = "PointId,Year,WeekOfYear", bool isNeedOriginalData = false)
        {
            recordTotal = 0;
            auditWeekService = Singleton<DataQueryByWeekService>.GetInstance();
            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            DataTable dtAuditData = null;

            for (int i = 0; i < dateTime.GetLength(0); i++)
            {
                dtAuditData = auditWeekService.GetWeekDataPager(portIds, factors, dateTime[i, 0], dateTime[i, 1], dateTime[i, 2], dateTime[i, 3],
                   pageSize, pageNo, out recordTotal, orderBy).Table;//获取审核周数据
                dtAuditData.Columns.Add("portName", typeof(string)).SetOrdinal(0);
                for (int j = 0; j < dtAuditData.Rows.Count; j++)//给portName字段赋值
                {
                    int pointId = Convert.ToInt32(dtAuditData.Rows[j]["PointId"]);
                    dtAuditData.Rows[j]["portName"] = g_MonitoringPointAir.RetrieveEntityByPointId(pointId).MonitoringPointName;//获取站点名称
                }

            }
            return dtAuditData.DefaultView;
        }
        /// <summary>
        /// 获取周数据的比对
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="factors">因子</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>周数据的比对视图</returns>
        public DataView GetWeekCompare(string[] portIds, string[] factors, int weekB, int weekF, int weekE, int weekT,
            int dtweekB, int dtweekF, int dtweekE, int dtweekT, int pageSize, int pageNo, out int recordTotal, int tabIndex, string orderBy = "PointId,Year,WeekOfYear", bool isNeedOriginalData = false)
        {
            recordTotal = 0;
            auditWeekService = Singleton<DataQueryByWeekService>.GetInstance();
            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();

            //DataTable dtAuditData = auditWeekService.GetWeekDataPager(portIds, factors, weekB, weekF, weekE, weekT, dtweekB, dtweekF, dtweekE, dtweekT,
            //    pageSize, pageNo, out recordTotal, orderBy).Table;//获取审核周数据

            DataTable dtAuditData = auditWeekService.GetWeekDataPagerDF(portIds, factors, weekB, weekF, weekE, weekT, 
                int.MaxValue, 0,  orderBy).Table;//获取审核周数据
            DataTable dt2 = auditWeekService.GetWeekDataPagerDF(portIds, factors,  dtweekB, dtweekF, dtweekE, dtweekT,
                int.MaxValue, 0,  orderBy).Table;//获取审核周数据

            dtAuditData.Columns.Add("portName", typeof(string)).SetOrdinal(0);
            dtAuditData.Columns.Add("rowMack", typeof(int)).SetOrdinal(1);

            for (int j = 0; j < dtAuditData.Rows.Count; j++)//给DataType、portName字段赋值
            {
                int pointId = Convert.ToInt32(dtAuditData.Rows[j]["PointId"]);
                dtAuditData.Rows[j]["portName"] = g_MonitoringPointAir.RetrieveEntityByPointId(pointId).MonitoringPointName;//获取站点名称
                dtAuditData.Rows[j]["rowMack"] = 1;
            }

            dt2.Columns.Add("portName", typeof(string)).SetOrdinal(0);
            dt2.Columns.Add("rowMack", typeof(int)).SetOrdinal(1);

            for (int j = 0; j < dt2.Rows.Count; j++)//给DataType、portName字段赋值
            {
                int pointId = Convert.ToInt32(dt2.Rows[j]["PointId"]);
                dt2.Rows[j]["portName"] = g_MonitoringPointAir.RetrieveEntityByPointId(pointId).MonitoringPointName;//获取站点名称
                dt2.Rows[j]["rowMack"] = 1;
            }

            for (int m = 0; m < dt2.Rows.Count; m++)
            {
                if (dt2.Rows[m]["Type"].ToString() == "审核")
                {
                    dt2.Rows[m]["Type"] = "审核(比对)";
                }
                if (dt2.Rows[m]["Type"].ToString() == "原始")
                {
                    dt2.Rows[m]["Type"] = "原始(比对)";
                }
            }

            for (int m = 0; m < dtAuditData.Rows.Count; m++)
            {
                if (dtAuditData.Rows[m]["Type"].ToString() == "审核")
                {
                    dtAuditData.Rows[m]["Type"] = "审核(基准)";
                }
                if (dtAuditData.Rows[m]["Type"].ToString() == "原始")
                {
                    dtAuditData.Rows[m]["Type"] = "原始(基准)";
                }
            }
            dtAuditData.Merge(dt2, true);
            recordTotal = dtAuditData.Rows.Count;
            //if (dtweekB > 0 && dtweekE > 0)
            //{
            //    int weekFrom = weekB * 1000 + weekF;
            //    int weekTo = weekE * 1000 + weekT;

            //    int weekFromB = dtweekB * 1000 + dtweekF;
            //    int weekToB = dtweekE * 1000 + dtweekT;
            //    string where = string.Format(" (Year*1000 + WeekOfYear)>= {0} AND (Year*1000 + WeekOfYear)<={1} ", weekFrom, weekTo);
            //    string whereStr = string.Format(" (Year*1000 + WeekOfYear)>= {0} AND (Year*1000 + WeekOfYear)<={1} ", weekFromB, weekToB);
            //    DataRow[] dtRow = dtAuditData.Select(where);   //2015
            //    DataRow[] drs = dtAuditData.Select(whereStr);   //2015
            //    for (int m = 0; m < dtRow.Length; m++)
            //    {
            //        if (dtRow[m]["Type"].ToString() == "审核")
            //        {
            //            dtRow[m]["Type"] = "审核(基准)";
            //            dtRow[m]["rowMack"] = m;
            //        }
            //    }
            //    for (int i = 0; i < drs.Length; i++)
            //    {
            //        if (drs[i]["Type"].ToString() == "审核")
            //        {
            //            drs[i]["Type"] = "审核(比对)";
            //            drs[i]["rowMack"] = i;
            //        }
            //    }
            //}
            return dtAuditData.DefaultView;
        }
        #endregion

        #region 月数据
        /// <summary>
        /// 取得月数据（最大值、最小值、平均值）
        /// </summary>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="monthOfYearFrom">开始月数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="monthOfYearTo">结束月数</param>
        /// <returns></returns>
        public DataView GetMonthStatisticalData(string[] portIds, IList<IPollutant> factors, int[,] dateTime)
        {
            auditMonthService = Singleton<DataQueryByMonthService>.GetInstance();
            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            DataTable newDataTable = null;
            List<DataTable> dtList = new List<DataTable>();

            for (int i = 0; i < dateTime.GetLength(0); i++)
            {
                DataTable dtStatisticalData = auditMonthService.GetMonthStatisticalData(portIds, factors, dateTime[i, 0], dateTime[i, 1], dateTime[i, 2], dateTime[i, 3]).Table;
                dtStatisticalData.Columns.Add("DataType", typeof(string)).SetOrdinal(1);
                dtStatisticalData.Columns.Add("DateTimeRange", typeof(string)).SetOrdinal(2);
                dtStatisticalData.Columns.Add("rowMack", typeof(string)).SetOrdinal(3);
                for (int j = 0; j < dtStatisticalData.Rows.Count; j++)
                {
                    if (i == 0)
                    {
                        dtStatisticalData.Rows[j]["DataType"] = "审核(基准)";
                        dtStatisticalData.Rows[j]["rowMack"] = j;
                    }
                    else
                    {
                        dtStatisticalData.Rows[j]["DataType"] = "审核(比对)";
                        dtStatisticalData.Rows[j]["rowMack"] = j;
                    }
                    dtStatisticalData.Rows[j]["DateTimeRange"] = dateTime[i, 0].ToString() + "年" + dateTime[i, 1].ToString() + "月至" + dateTime[i, 2].ToString() + "年" + dateTime[i, 3].ToString() + "月";
                }
                dtList.Add(dtStatisticalData);

                if (newDataTable == null)
                {
                    newDataTable = dtStatisticalData.Clone();
                }
            }
            object[] obj = new object[newDataTable.Columns.Count];
            for (int i = 0; i < dtList.Count; i++)
            {
                DataTable dt = dtList[i];
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    dt.Rows[j].ItemArray.CopyTo(obj, 0);
                    newDataTable.Rows.Add(obj);
                }
            }
            newDataTable.DefaultView.Sort = "rowMack,DateTimeRange,DataType";
            return newDataTable.DefaultView;
        }

        /// <summary>
        /// 获取月数据的比对
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="factors">因子</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>月数据的比对视图</returns>
        public DataView GetMonthCompareView(string[] portIds, string[] factors, int[,] dateTime, int pageSize, int pageNo, out int recordTotal, int tabIndex, string orderBy = "PointId,Year,MonthOfYear")
        {
            recordTotal = 0;
            auditMonthService = Singleton<DataQueryByMonthService>.GetInstance();
            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            DataTable dtAuditData = null;

            for (int i = 0; i < dateTime.GetLength(0); i++)
            {
                dtAuditData = auditMonthService.GetMonthDataPager(portIds, factors, dateTime[i, 0], dateTime[i, 1], dateTime[i, 2], dateTime[i, 3],
                     pageSize, pageNo, out recordTotal, orderBy).Table;//获取审核月数据
                dtAuditData.Columns.Add("portName", typeof(string)).SetOrdinal(0);
                for (int j = 0; j < dtAuditData.Rows.Count; j++)//给portName字段赋值
                {
                    int pointId = Convert.ToInt32(dtAuditData.Rows[j]["PointId"]);
                    dtAuditData.Rows[j]["portName"] = g_MonitoringPointAir.RetrieveEntityByPointId(pointId).MonitoringPointName;//获取站点名称
                }
            }

            return dtAuditData.DefaultView;
        }
        /// <summary>
        /// 获取月数据的比对
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="factors">因子</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>月数据的比对视图</returns>
        public DataView GetMonthCompare(string[] portIds, string[] factors, int monthB, int monthF, int monthE, int monthT, int dtmonthB, int dtmonthF, int dtmonthE, int dtmonthT, int pageSize, int pageNo, out int recordTotal, int tabIndex, string orderBy = "PointId,Year desc,MonthOfYear desc")
        {
            recordTotal = 0;
            auditMonthService = Singleton<DataQueryByMonthService>.GetInstance();
            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            DataTable dtAuditData = null;
            DataTable dt2 = null;

            dtAuditData = auditMonthService.GetMonthDataPagerDF(portIds, factors, monthB, monthF, monthE, monthT,  int.MaxValue, 0,  orderBy).Table;//获取审核月数据
            dt2 = auditMonthService.GetMonthDataPagerDF(portIds, factors, dtmonthB, dtmonthF, dtmonthE, dtmonthT, int.MaxValue, 0, orderBy).Table;//获取审核月数据

            dtAuditData.Columns.Add("portName", typeof(string)).SetOrdinal(0);
            dtAuditData.Columns.Add("rowMack", typeof(int)).SetOrdinal(1);

            for (int j = 0; j < dtAuditData.Rows.Count; j++)//给DataType、portName字段赋值
            {
                int pointId = Convert.ToInt32(dtAuditData.Rows[j]["PointId"]);
                dtAuditData.Rows[j]["portName"] = g_MonitoringPointAir.RetrieveEntityByPointId(pointId).MonitoringPointName;//获取站点名称
                dtAuditData.Rows[j]["rowMack"] = 1;
            }

            dt2.Columns.Add("portName", typeof(string)).SetOrdinal(0);
            dt2.Columns.Add("rowMack", typeof(int)).SetOrdinal(1);

            for (int j = 0; j < dt2.Rows.Count; j++)//给DataType、portName字段赋值
            {
                int pointId = Convert.ToInt32(dt2.Rows[j]["PointId"]);
                dt2.Rows[j]["portName"] = g_MonitoringPointAir.RetrieveEntityByPointId(pointId).MonitoringPointName;//获取站点名称
                dt2.Rows[j]["rowMack"] = 1;
            }

            for (int m = 0; m < dt2.Rows.Count; m++)
            {
                if (dt2.Rows[m]["Type"].ToString() == "审核")
                {
                    dt2.Rows[m]["Type"] = "审核(比对)";
                }
                if (dt2.Rows[m]["Type"].ToString() == "原始")
                {
                    dt2.Rows[m]["Type"] = "原始(比对)";
                }
            }

            for (int m = 0; m < dtAuditData.Rows.Count; m++)
            {
                if (dtAuditData.Rows[m]["Type"].ToString() == "审核")
                {
                    dtAuditData.Rows[m]["Type"] = "审核(基准)";
                }
                if (dtAuditData.Rows[m]["Type"].ToString() == "原始")
                {
                    dtAuditData.Rows[m]["Type"] = "原始(基准)";
                }
            }
            dtAuditData.Merge(dt2, true);
            recordTotal = dtAuditData.Rows.Count;
            //if (dtmonthB > 0 && dtmonthE > 0)
            //{
            //    string where = string.Format(" ReportDateTime>='{0}-{1}-01' and ReportDateTime<='{2}-{3}-01'", monthB, monthF, monthE, monthT);
            //    string whereStr = string.Format(" ReportDateTime>='{0}-{1}-01' and ReportDateTime<='{2}-{3}-01' ", dtmonthB, dtmonthF, dtmonthE, dtmonthT);
            //    DataRow[] dtRow = dtAuditData.Select(where);   //2015
            //    DataRow[] drs = dtAuditData.Select(whereStr);   //2015
            //    for (int m = 0; m < dtRow.Length; m++)
            //    {
            //        if (dtRow[m]["Type"].ToString() == "审核")
            //        {
            //            dtRow[m]["Type"] = "审核(基准)";
            //            dtRow[m]["rowMack"] = m;
            //        }
            //    }
            //    for (int i = 0; i < drs.Length; i++)
            //    {
            //        if (drs[i]["Type"].ToString() == "审核")
            //        {
            //            drs[i]["Type"] = "审核(比对)";
            //            drs[i]["rowMack"] = i;
            //        }
            //    }
            //}
            return dtAuditData.DefaultView;
        }
        #endregion

        #region 季数据
        /// <summary>
        /// 取得季数据（最大值、最小值、平均值）
        /// </summary>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="seasonOfYearFrom">开始季数</param>
        /// <param name="yearTo">结束年</param>
        /// <param name="seasonOfYearTo">结束季数</param>
        /// <returns></returns>
        public DataView GetSeasonStatisticalData(string[] portIds, IList<IPollutant> factors, int[,] dateTime)
        {
            auditSeasonService = Singleton<DataQueryBySeasonService>.GetInstance();
            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            DataTable newDataTable = null;
            List<DataTable> dtList = new List<DataTable>();

            for (int i = 0; i < dateTime.GetLength(0); i++)
            {
                DataTable dtStatisticalData = auditSeasonService.GetSeasonStatisticalData(portIds, factors, dateTime[i, 0], dateTime[i, 1], dateTime[i, 2], dateTime[i, 3]).Table;
                dtStatisticalData.Columns.Add("DataType", typeof(string)).SetOrdinal(1);
                dtStatisticalData.Columns.Add("DateTimeRange", typeof(string)).SetOrdinal(2);
                dtStatisticalData.Columns.Add("rowMack", typeof(string)).SetOrdinal(3);
                for (int j = 0; j < dtStatisticalData.Rows.Count; j++)
                {
                    if (i == 0)
                    {
                        dtStatisticalData.Rows[j]["DataType"] = "审核(基准)";
                        dtStatisticalData.Rows[j]["rowMack"] = j;
                    }
                    else
                    {
                        dtStatisticalData.Rows[j]["DataType"] = "审核(比对)";
                        dtStatisticalData.Rows[j]["rowMack"] = j;
                    }
                    dtStatisticalData.Rows[j]["DateTimeRange"] = dateTime[i, 0].ToString() + "年第" + dateTime[i, 1].ToString() + "季度至" + dateTime[i, 2].ToString() + "年第" + dateTime[i, 3].ToString() + "季度";
                }
                dtList.Add(dtStatisticalData);

                if (newDataTable == null)
                {
                    newDataTable = dtStatisticalData.Clone();
                }
            }
            object[] obj = new object[newDataTable.Columns.Count];
            for (int i = 0; i < dtList.Count; i++)
            {
                DataTable dt = dtList[i];
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    dt.Rows[j].ItemArray.CopyTo(obj, 0);
                    newDataTable.Rows.Add(obj);
                }
            }
            newDataTable.DefaultView.Sort = "rowMack,DateTimeRange,DataType";
            return newDataTable.DefaultView;
        }

        /// <summary>
        /// 获取季数据的比对
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="factors">因子</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>季数据的比对视图</returns>
        public DataView GetSeasonCompareView(string[] portIds, string[] factors, int[,] dateTime, int pageSize, int pageNo, out int recordTotal, int tabIndex, string orderBy = "PointId,Year,SeasonOfYear")
        {
            recordTotal = 0;
            auditSeasonService = Singleton<DataQueryBySeasonService>.GetInstance();
            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            DataTable dtAuditData = null;

            for (int i = 0; i < dateTime.GetLength(0); i++)
            {
                dtAuditData = auditSeasonService.GetSeasonDataPager(portIds, factors, dateTime[i, 0], dateTime[i, 1], dateTime[i, 2], dateTime[i, 3],
                      pageSize, pageNo, out recordTotal, orderBy).Table;//获取审核季数据
                dtAuditData.Columns.Add("portName", typeof(string)).SetOrdinal(0);
                for (int j = 0; j < dtAuditData.Rows.Count; j++)//给portName字段赋值
                {
                    int pointId = Convert.ToInt32(dtAuditData.Rows[j]["PointId"]);
                    dtAuditData.Rows[j]["portName"] = g_MonitoringPointAir.RetrieveEntityByPointId(pointId).MonitoringPointName;//获取站点名称
                }

            }
            return dtAuditData.DefaultView;
        }
        /// <summary>
        /// 获取季数据的比对
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="factors">因子</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>季数据的比对视图</returns>
        public DataView GetSeasonCompare(string[] portIds, string[] factors, int seasonB, int seasonF, int seasonE, int seasonT,
            int dtseasonB, int dtseasonF, int dtseasonE, int dtseasonT, int pageSize, int pageNo, out int recordTotal, int tabIndex, string orderBy = "PointId,Year,SeasonOfYear")
        {
            recordTotal = 0;
            auditSeasonService = Singleton<DataQueryBySeasonService>.GetInstance();
            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();

         //   DataTable dtAuditData = auditSeasonService.GetSeasonDataPager(portIds, factors, seasonB, seasonF, seasonE, seasonT,
         //dtseasonB, dtseasonF, dtseasonE, dtseasonT, pageSize, pageNo, out recordTotal, orderBy).Table;//获取审核周数据
            DataTable dtAuditData = auditSeasonService.GetSeasonDataPagerDF(portIds, factors, seasonB, seasonF, seasonE, seasonT,
          int.MaxValue, 0, orderBy).Table;//获取审核季数据
            DataTable dt2 = auditSeasonService.GetSeasonDataPagerDF(portIds, factors, dtseasonB, dtseasonF, dtseasonE, dtseasonT,
          int.MaxValue, 0,  orderBy).Table;//获取审核季数据

            dtAuditData.Columns.Add("portName", typeof(string)).SetOrdinal(0);
            dtAuditData.Columns.Add("rowMack", typeof(int)).SetOrdinal(1);

            IQueryable<MonitoringPointEntity> entity = g_MonitoringPointAir.RetrieveListByPointIds(portIds);
            for (int j = 0; j < dtAuditData.Rows.Count; j++)//给DataType、portName字段赋值
            {
                int pointId = Convert.ToInt32(dtAuditData.Rows[j]["PointId"]);
                //dtAuditData.Rows[j]["portName"] = g_MonitoringPointAir.RetrieveEntityByPointId(pointId).MonitoringPointName;//获取站点名称
                dtAuditData.Rows[j]["portName"] = entity.Where(x => x.PointId == pointId).Select(t => t.MonitoringPointName).FirstOrDefault();
                dtAuditData.Rows[j]["rowMack"] = 1;
            }

            dt2.Columns.Add("portName", typeof(string)).SetOrdinal(0);
            dt2.Columns.Add("rowMack", typeof(int)).SetOrdinal(1);

            for (int j = 0; j < dt2.Rows.Count; j++)//给DataType、portName字段赋值
            {
                int pointId = Convert.ToInt32(dt2.Rows[j]["PointId"]);
                //dtAuditData.Rows[j]["portName"] = g_MonitoringPointAir.RetrieveEntityByPointId(pointId).MonitoringPointName;//获取站点名称
                dt2.Rows[j]["portName"] = entity.Where(x => x.PointId == pointId).Select(t => t.MonitoringPointName).FirstOrDefault();
                dt2.Rows[j]["rowMack"] = 1;
            }

            for (int m = 0; m < dt2.Rows.Count; m++)
            {
                if (dt2.Rows[m]["Type"].ToString() == "审核")
                {
                    dt2.Rows[m]["Type"] = "审核(比对)";
                }
                if (dt2.Rows[m]["Type"].ToString() == "原始")
                {
                    dt2.Rows[m]["Type"] = "原始(比对)";
                }
            }

            for (int m = 0; m < dtAuditData.Rows.Count; m++)
            {
                if (dtAuditData.Rows[m]["Type"].ToString() == "审核")
                {
                    dtAuditData.Rows[m]["Type"] = "审核(基准)";
                }
                if (dtAuditData.Rows[m]["Type"].ToString() == "原始")
                {
                    dtAuditData.Rows[m]["Type"] = "原始(基准)";
                }
            }
            dtAuditData.Merge(dt2, true);
            recordTotal = dtAuditData.Rows.Count;
            //if (dtseasonB > 0 && dtseasonE > 0)
            //{
            //    int seasonFrom = seasonB * 1000 + seasonF;
            //    int seasonTo = seasonE * 1000 + seasonT;

            //    int seasonFromB = dtseasonB * 1000 + dtseasonF;
            //    int seasonToB = dtseasonE * 1000 + dtseasonT;
            //    string where = string.Format("  (Year*1000 + SeasonOfYear)>= {0} AND (Year*1000 + SeasonOfYear)<={1}  ", seasonFrom, seasonTo);
            //    string whereStr = string.Format("  (Year*1000 + SeasonOfYear)>= {0} AND (Year*1000 + SeasonOfYear)<={1} ", seasonFromB, seasonToB);
            //    DataRow[] dtRow = dtAuditData.Select(where);   //2015
            //    DataRow[] drs = dtAuditData.Select(whereStr);   //2015
            //    for (int m = 0; m < dtRow.Length; m++)
            //    {
            //        if (dtRow[m]["Type"].ToString() == "审核")
            //        {
            //            dtRow[m]["Type"] = "审核(基准)";
            //            dtRow[m]["rowMack"] = m;
            //        }
            //    }
            //    for (int i = 0; i < drs.Length; i++)
            //    {
            //        if (drs[i]["Type"].ToString() == "审核")
            //        {
            //            drs[i]["Type"] = "审核(比对)";
            //            drs[i]["rowMack"] = i;
            //        }
            //    }
            //}
            return dtAuditData.DefaultView;
        }
        #endregion

        #region 年数据
        /// <summary>
        /// 取得年数据（最大值、最小值、平均值）
        /// </summary>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="yearFrom">开始年</param>
        /// <param name="yearTo">结束年</param>
        /// <returns></returns>
        public DataView GetYearStatisticalData(string[] portIds, IList<IPollutant> factors, int[,] dateTime)
        {
            auditYearService = Singleton<DataQueryByYearService>.GetInstance();
            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            DataTable newDataTable = null;
            List<DataTable> dtList = new List<DataTable>();

            for (int i = 0; i < dateTime.GetLength(0); i++)
            {
                DataTable dtStatisticalData = auditYearService.GetYearStatisticalData(portIds, factors, dateTime[i, 0], dateTime[i, 1]).Table;
                dtStatisticalData.Columns.Add("DataType", typeof(string)).SetOrdinal(1);
                dtStatisticalData.Columns.Add("DateTimeRange", typeof(string)).SetOrdinal(2);
                dtStatisticalData.Columns.Add("rowMack", typeof(string)).SetOrdinal(3);
                for (int j = 0; j < dtStatisticalData.Rows.Count; j++)
                {
                    if (i == 0)
                    {
                        dtStatisticalData.Rows[j]["DataType"] = "审核(基准)";
                        dtStatisticalData.Rows[j]["rowMack"] = j;
                    }
                    else
                    {
                        dtStatisticalData.Rows[j]["DataType"] = "审核(比对)";
                        dtStatisticalData.Rows[j]["rowMack"] = j;
                    }
                    dtStatisticalData.Rows[j]["DateTimeRange"] = dateTime[i, 0].ToString() + "年至" + dateTime[i, 1].ToString() + "年";
                }
                dtList.Add(dtStatisticalData);

                if (newDataTable == null)
                {
                    newDataTable = dtStatisticalData.Clone();
                }
            }
            object[] obj = new object[newDataTable.Columns.Count];
            for (int i = 0; i < dtList.Count; i++)
            {
                DataTable dt = dtList[i];
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    dt.Rows[j].ItemArray.CopyTo(obj, 0);
                    newDataTable.Rows.Add(obj);
                }
            }
            newDataTable.DefaultView.Sort = "rowMack,DateTimeRange,DataType";
            return newDataTable.DefaultView;
        }

        /// <summary>
        /// 获取年数据的比对
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="factors">因子</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>年数据的比对视图</returns>
        public DataView GetYearCompareView(string[] portIds, string[] factors, int[,] dateTime, int pageSize, int pageNo, out int recordTotal, int tabIndex, string orderBy = "PointId,Year")
        {
            recordTotal = 0;
            auditYearService = Singleton<DataQueryByYearService>.GetInstance();
            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            DataTable dtAuditData = null;

            for (int i = 0; i < dateTime.GetLength(0); i++)
            {
                dtAuditData = auditYearService.GetYearDataPager(portIds, factors, dateTime[i, 0], dateTime[i, 1],
                     pageSize, pageNo, out recordTotal, orderBy).Table;//获取审核年数据
                dtAuditData.Columns.Add("portName", typeof(string)).SetOrdinal(0);
                for (int j = 0; j < dtAuditData.Rows.Count; j++)//给portName字段赋值
                {
                    int pointId = Convert.ToInt32(dtAuditData.Rows[j]["PointId"]);
                    dtAuditData.Rows[j]["portName"] = g_MonitoringPointAir.RetrieveEntityByPointId(pointId).MonitoringPointName;//获取站点名称
                }

            }
            return dtAuditData.DefaultView;
        }
        /// <summary>
        /// 获取年数据的比对
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <param name="factors">因子</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>年数据的比对视图</returns>
        public DataView GetYearCompare(string[] portIds, string[] factors, int yearB, int yearE, int dtyearB, int dtyearE, int pageSize, int pageNo, out int recordTotal, int tabIndex, string orderBy = "PointId,Year")
        {
            recordTotal = 0;
            auditYearService = Singleton<DataQueryByYearService>.GetInstance();
            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            //DataTable dtAuditData = auditYearService.GetYearDataPager(portIds, factors, yearB, yearE, dtyearB, dtyearE,
            //    pageSize, pageNo, out recordTotal, orderBy).Table;//获取审核年数据

            DataTable dtAuditData = auditYearService.GetYearDataPagerDF(portIds, factors, yearB, yearE, 
                int.MaxValue, 0,  orderBy).Table;//获取审核年数据
            DataTable dt2 = auditYearService.GetYearDataPagerDF(portIds, factors,  dtyearB, dtyearE,
                int.MaxValue, 0,  orderBy).Table;//获取审核年数据

            dtAuditData.Columns.Add("portName", typeof(string)).SetOrdinal(0);
            dtAuditData.Columns.Add("rowMack", typeof(int)).SetOrdinal(1);

            for (int j = 0; j < dtAuditData.Rows.Count; j++)//给DataType、portName字段赋值
            {
                int pointId = Convert.ToInt32(dtAuditData.Rows[j]["PointId"]);
                dtAuditData.Rows[j]["portName"] = g_MonitoringPointAir.RetrieveEntityByPointId(pointId).MonitoringPointName;//获取站点名称
                dtAuditData.Rows[j]["rowMack"] = 1;
            }

            dt2.Columns.Add("portName", typeof(string)).SetOrdinal(0);
            dt2.Columns.Add("rowMack", typeof(int)).SetOrdinal(1);

            for (int j = 0; j < dt2.Rows.Count; j++)//给DataType、portName字段赋值
            {
                int pointId = Convert.ToInt32(dt2.Rows[j]["PointId"]);
                dt2.Rows[j]["portName"] = g_MonitoringPointAir.RetrieveEntityByPointId(pointId).MonitoringPointName;//获取站点名称
                dt2.Rows[j]["rowMack"] = 1;
            }

            for (int m = 0; m < dt2.Rows.Count; m++)
            {
                if (dt2.Rows[m]["Type"].ToString() == "审核")
                {
                    dt2.Rows[m]["Type"] = "审核(比对)";
                }
                if (dt2.Rows[m]["Type"].ToString() == "原始")
                {
                    dt2.Rows[m]["Type"] = "原始(比对)";
                }
            }

            for (int m = 0; m < dtAuditData.Rows.Count; m++)
            {
                if (dtAuditData.Rows[m]["Type"].ToString() == "审核")
                {
                    dtAuditData.Rows[m]["Type"] = "审核(基准)";
                }
                if (dtAuditData.Rows[m]["Type"].ToString() == "原始")
                {
                    dtAuditData.Rows[m]["Type"] = "原始(基准)";
                }
            }
            dtAuditData.Merge(dt2, true);
            recordTotal = dtAuditData.Rows.Count;
            //if (dtyearB > 0 && dtyearE > 0)
            //{
            //    string where = string.Format(" Year>='{0}' AND Year<='{1}' ", yearB, yearE);
            //    string whereStr = string.Format(" Year>='{0}' AND Year<='{1}' ", dtyearB, dtyearE);
            //    DataRow[] dtRow = dtAuditData.Select(where);   //2015
            //    DataRow[] drs = dtAuditData.Select(whereStr);   //2015
            //    for (int m = 0; m < dtRow.Length; m++)
            //    {
            //        if (dtRow[m]["Type"].ToString() == "审核")
            //        {
            //            dtRow[m]["Type"] = "审核(基准)";
            //            dtRow[m]["rowMack"] = m;
            //        }
            //    }
            //    for (int i = 0; i < drs.Length; i++)
            //    {
            //        if (drs[i]["Type"].ToString() == "审核")
            //        {
            //            drs[i]["Type"] = "审核(比对)";
            //            drs[i]["rowMack"] = i;
            //        }
            //    }
            //}
            return dtAuditData.DefaultView;
        }
        #endregion
    }
}
