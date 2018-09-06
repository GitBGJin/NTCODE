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
using SmartEP.DomainModel.BaseData;
using SmartEP.Data.SqlServer.BaseData;

namespace SmartEP.Service.DataAnalyze.Water.WaterReport
{
    public class RiverSectionService
    {
        /// <summary>
        /// 地表水日数据
        /// </summary>
        DayReportRepository DayData = Singleton<DayReportRepository>.GetInstance();

        WaterAnalyzeDAL m_WaterAnalyzeDAL = new WaterAnalyzeDAL();

        /// <summary>
        /// 测点类
        /// </summary>
        MonitoringPointWaterService g_MonitoringPointWater = new MonitoringPointWaterService();

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
        public DataView GetDayDataPager(string[] portIds, IList<IPollutant> factors, DateTime dtStart, DateTime dtEnd
            , int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime")
        {
            recordTotal = 0;
            if (factors.IsNotNullOrDBNull())
                return DayData.GetDataPager(portIds, factors.Select(p => p.PollutantCode).ToArray(), dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
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
        public DataView GetDayDataPager(string[] portIds, string[] factors, DateTime dtStart, DateTime dtEnd
            , int pageSize, int pageNo, out int recordTotal, string orderBy = "PointId,DateTime")
        {
            recordTotal = 0;

            DataView dtv = m_WaterAnalyzeDAL.GetWaterAnalyzeData(portIds);
            DataTable dtNew = dtv.ToTable();
            if (factors.IsNotNullOrDBNull())
            {
                DataView dv = new DataView();

                dv = DayData.GetDataPager(portIds, factors, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
                DataTable newdtb = new DataTable();
                newdtb.Columns.Add("PointName", typeof(string));
                newdtb.Columns.Add("Year", typeof(int));
                newdtb.Columns.Add("Month", typeof(int));
                newdtb.Columns.Add("Day", typeof(int));
                newdtb.Columns.Add("w01001", typeof(decimal));
                newdtb.Columns.Add("w01009", typeof(decimal));
                newdtb.Columns.Add("w01019", typeof(decimal));
                newdtb.Columns.Add("w21003", typeof(decimal));
                newdtb.Columns.Add("w21011", typeof(decimal));
                newdtb.Columns.Add("blank", typeof(string));
                newdtb.Columns.Add("w01001a", typeof(decimal));
                newdtb.Columns.Add("w01009a", typeof(decimal));
                newdtb.Columns.Add("w01019a", typeof(decimal));
                newdtb.Columns.Add("w21003a", typeof(decimal));
                newdtb.Columns.Add("w21011a", typeof(decimal));
                DataTable dt = dv.ToTable();
                DataRow[] Rowdt;

                for (int i = 0; i < portIds.Length; i++)
                {
                    string PointName = "";
                    int id = Convert.ToInt32(portIds[i]);
                    MonitoringPointEntity MonitoringPointEntity = g_MonitoringPointWater.RetrieveEntityByPointId(id);
                    if (MonitoringPointEntity.IsNotNullOrDBNull())
                    {
                        PointName = MonitoringPointEntity.MonitoringPointName;
                    }
                    else
                    {
                        PointName = "/";
                    }

                    Rowdt = dt.Select("PointId='" + portIds[i] + "'");

                    for (int j = 0; j < Rowdt.Length; j++)
                    {
                        DataRow newRow = newdtb.NewRow();
                        if (Rowdt[j]["w01001"].IsNotNullOrDBNull())
                        {
                            newRow["w01001"] = DecimalExtension.GetPollutantValue(decimal.Parse(Rowdt[j]["w01001"].ToString()), 2);
                            newRow["w01001a"] = decimal.Parse(Rowdt[j]["w01001"].ToString());
                        }
                        if (Rowdt[j]["w01009"].IsNotNullOrDBNull())
                        {
                            newRow["w01009"] = DecimalExtension.GetPollutantValue(decimal.Parse(Rowdt[j]["w01009"].ToString()), 2);
                            newRow["w01009a"] = decimal.Parse(Rowdt[j]["w01009"].ToString());
                        }
                        if (Rowdt[j]["w01019"].IsNotNullOrDBNull())
                        {
                            newRow["w01019"] = DecimalExtension.GetPollutantValue(decimal.Parse(Rowdt[j]["w01019"].ToString()), 1);
                            newRow["w01019a"] = decimal.Parse(Rowdt[j]["w01019"].ToString());
                        }
                        if (Rowdt[j]["w21003"].IsNotNullOrDBNull())
                        {
                            newRow["w21003"] = DecimalExtension.GetPollutantValue(decimal.Parse(Rowdt[j]["w21003"].ToString()), 2);
                            newRow["w21003a"] = decimal.Parse(Rowdt[j]["w21003"].ToString());
                        }
                        if (Rowdt[j]["w21011"].IsNotNullOrDBNull())
                        {
                            newRow["w21011"] = DecimalExtension.GetPollutantValue(decimal.Parse(Rowdt[j]["w21011"].ToString()), 3);
                            newRow["w21011a"] = decimal.Parse(Rowdt[j]["w21011"].ToString());
                        }
                        newRow["PointName"] = PointName;
                        newRow["Year"] = DateTime.Parse(Rowdt[j]["DateTime"].ToString()).Year;
                        newRow["Month"] = DateTime.Parse(Rowdt[j]["DateTime"].ToString()).Month;
                        newRow["Day"] = DateTime.Parse(Rowdt[j]["DateTime"].ToString()).Day;

                        newdtb.Rows.Add(newRow);
                    }
                    //if (dtNew.Rows[i]["WatersName"].ToString().IsNotNullOrDBNull())
                    //{
                    //    newRow["PointName"] = dtNew.Rows[i]["WatersName"].ToString();
                    //}
                    //else
                    //{
                    //    newRow["PointName"] = "/";
                    //}

                }
                DataView newdv = new DataView(newdtb);
                return newdv;
            }
            return null;
        }



        /// <summary>
        /// 取得虚拟分页查询数据(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetDayExportData(string[] portIds, IList<IPollutant> factors, DateTime dateStart, DateTime dateEnd, string orderBy = "PointId,DateTime")
        {
            if (DayData != null)
                return DayData.GetExportData(portIds, factors, dateStart, dateEnd, orderBy);
            return null;
        }

        /// <summary>
        /// 取得行转列数据总行数
        /// </summary>
        /// <param name="portIds">测点数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <returns></returns>
        public int GetDayAllDataCount(string[] portIds, DateTime dateStart, DateTime dateEnd)
        {
            if (DayData != null)
                return DayData.GetAllDataCount(portIds, dateStart, dateStart);
            return 0;
        }
    }
}

