using SmartEP.MonitoringBusinessRepository.Air;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.DataAnalyze.Interfaces;
using SmartEP.Service.Frame;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Core.Generic;
using SmartEP.Service.BaseData.Standard;
using SmartEP.Service.DataAnalyze.Air.DataQuery;
using SmartEP.Utilities.AdoData;
using SmartEP.BaseInfoRepository.MPInfo;
using SmartEP.Data.SqlServer.BaseData;

namespace SmartEP.Service.DataAnalyze.Air.AQIReport
{
    /// <summary>
    /// 名称：ConcentrationAQIService.cs
    /// 创建人：刘长敏
    /// 创建日期：2016-1-21
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：浓度_AQI综合查询
    /// 虚拟分页类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class ConcentrationAQIService : IDayAQI
    {
        /// <summary>
        /// 测点类
        /// </summary>
        MonitoringPointAirService g_MonitoringPointAir = null;
        private AQICalculateService m_AQICalculateService=new AQICalculateService();
        /// <summary>
        /// 数据库处理类
        /// </summary>
        DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();
        private string BaseDataConnection = "AMS_BaseDataConnection";
        /// <summary>
        /// 点位日AQI
        /// </summary>
        DayAQIRepository pointDayAQI = null;
        /// <summary>
        /// 空气站点信息服务
        /// </summary>
        private MonitoringPointAirService pointAirServices = new MonitoringPointAirService();
        /// <summary>
        /// 空气站点信息服务
        /// </summary>
        MonitoringPointAirService pointAirService = new MonitoringPointAirService();
        /// <summary>
        /// 区域日AQI
        /// </summary>
        RegionDayAQIRepository regionDayAQI = null;
        
        /// <summary>
        /// 区域名称
        /// </summary>
        DictionaryService g_DictionaryService = new DictionaryService();
        //获取因子小数位

        DayAQIService dayAQI = new DayAQIService();
        DataQueryByDayService m_DataQueryByDayService = Singleton<DataQueryByDayService>.GetInstance();


        MonitoringPointRepository g_Repository = new MonitoringPointRepository();
        /// <summary>
        /// 数据处理DAL类
        /// </summary>
        MonitoringPointDAL m_MonitoringPointDAC = new MonitoringPointDAL();
        /// <summary>
        /// 空气污染指数
        /// </summary>
        AQIService s_AQIService = new AQIService();

        #region 根据站点统计
        /// <summary>
        /// 根据key主键判断记录是否存在
        /// </summary>
        /// <param name="strKey">主键值</param>
        /// <returns></returns>
        public bool PIsExist(string strKey)
        {
            pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
            if (pointDayAQI != null)
                return pointDayAQI.IsExist(strKey);
            return false;
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页（从0开始）</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：MonitoringRegionUid）</param>
        /// <returns></returns>
        public DataView GetPortDataPager(string[] portIds, DateTime dtStart, DateTime dtEnd, int pageSize
            , int pageNo, out int recordTotal, string orderBy = "DateTime,MonitoringRegionUid")
        {
            pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
            recordTotal = 0;
            if (pointDayAQI != null)
                return pointDayAQI.GetDataPager(portIds, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
            return null;
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页（从0开始）</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetPortDataPagerDayAQI(string[] portIds, DateTime dtStart, DateTime dtEnd, int pageSize
            , int pageNo, out int recordTotal, string orderBy = "PointId")
        {
            pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
            recordTotal = 0;
            if (pointDayAQI != null)
                return pointDayAQI.GetConcentrationDay(portIds, dtStart, dtEnd, pageSize, pageNo, out recordTotal, orderBy);
            return null;
        }
        /// <summary>
        /// 取得导出数据（行转列数据）
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetPortExportData(string[] portIds, DateTime dtStart, DateTime dtEnd, string orderBy = "DateTime,PointId")
        {
            pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
            if (pointDayAQI != null)
                return pointDayAQI.GetExportData(portIds, dtStart, dtEnd, orderBy);
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="mBegion"></param>
        /// <param name="mEnd"></param>
        /// <param name="years"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <param name="recordTotal"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetAirQualityChangePager(string[] portIds, string reportType, DateTime mBegion, DateTime mEnd, string years, int pageSize
       , int pageNo, out int recordTotal, string orderBy = "PointId")
        {
            
            recordTotal = 0;
            decimal all, alls, Excellent, Excellents, Good, Goods, Severe, Severes;
            DataTable dt = new DataTable();
            string[] PointNames = new string[] { "南通市区", "崇川区", "港闸区", "开发区", "通州区", "通州湾示范区", "海安县", "如皋市", "如东县", "海门市", "启东市" };
            string[] Pointnames = new string[] { "南通市区", "通州区", "通州湾示范区", "海安县", "如皋市", "如东县", "海门市", "启东市" };

            string[] cc = new string[] { "187", "188" };
            string[] gz = new string[] { "190" };
            string[] kf = new string[] { "189" };
            #region 各县(市)、区达标率情况月报
            if (reportType == "Rate")
            {
                #region 创建表结构
                dt.Columns.Add("PointName", typeof(string));
                dt.Columns.Add("LastP", typeof(string));
                dt.Columns.Add("ThisP", typeof(string));
                dt.Columns.Add("PCompare", typeof(string));
                dt.Columns.Add("LastC", typeof(string));
                dt.Columns.Add("ThisC", typeof(string));
                dt.Columns.Add("CCompare", typeof(string));
                #endregion
                #region 常规县区及崇川区、港闸区、开发区数据源
                DataView dv = m_AQICalculateService.GetRegionAQI(portIds, mBegion, mEnd, 24, "2").AsDataView();
                DataView dvCC = m_AQICalculateService.GetRegionAQI(cc, mBegion, mEnd, 24, "2").AsDataView();
                DataView dvGZ = m_AQICalculateService.GetRegionAQI(gz, mBegion, mEnd, 24, "2").AsDataView();
                DataView dvKF = m_AQICalculateService.GetRegionAQI(kf, mBegion, mEnd, 24, "2").AsDataView();

                DataView dvs = m_AQICalculateService.GetRegionAQI(portIds, Convert.ToDateTime(years + "-" + mBegion.Month + "-" + mBegion.Day), Convert.ToDateTime(years + "-" + mEnd.Month + "-" + mEnd.Day), 24, "2").AsDataView();
                DataView dvCCs = m_AQICalculateService.GetRegionAQI(cc, Convert.ToDateTime(years + "-" + mBegion.Month + "-" + mBegion.Day), Convert.ToDateTime(years + "-" + mEnd.Month + "-" + mEnd.Day), 24, "2").AsDataView();
                DataView dvGZs = m_AQICalculateService.GetRegionAQI(gz, Convert.ToDateTime(years + "-" + mBegion.Month + "-" + mBegion.Day), Convert.ToDateTime(years + "-" + mEnd.Month + "-" + mEnd.Day), 24, "2").AsDataView();
                DataView dvKFs = m_AQICalculateService.GetRegionAQI(kf, Convert.ToDateTime(years + "-" + mBegion.Month + "-" + mBegion.Day), Convert.ToDateTime(years + "-" + mEnd.Month + "-" + mEnd.Day), 24, "2").AsDataView();
                #endregion
                #region dt添加行
                for (int i = 0; i < PointNames.Length; i++)
                {
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                    dt.Rows[i]["PointName"] = PointNames[i];
                }
                #endregion
                #region 往dt填数据
                for (int i = 0; i < PointNames.Length; i++)
                {
                    if (i == 1)
                    {
                        #region 崇川区
                        dvCC.RowFilter = "PointId='" + "南通市区" + "'";
                        all = dvCC.Count;
                        dvCC.RowFilter = "PointId='" + "南通市区" + "' and AQIValue <> '' and AQIValue<=50";
                        Excellent = dvCC.Count;
                        dvCC.RowFilter = "PointId='" + "南通市区" + "' and AQIValue <> '' and AQIValue<=100 and AQIValue>50";
                        Good = dvCC.Count;
                        dvCC.RowFilter = "PointId='" + "南通市区" + "' and AQIValue <> '' and AQIValue<=300 and AQIValue>200";
                        Severe = dvCC.Count;
                        //
                        dvCC.RowFilter = "PointId='" + "南通市区" + "'  and (AQIValue<0 or AQIValue is null)";
                        decimal None = dvCC.Count;

                        dvCCs.RowFilter = "PointId='" + "南通市区" + "'";
                        alls = dvCCs.Count;
                        dvCCs.RowFilter = "PointId='" + "南通市区" + "' and AQIValue <> '' and AQIValue<=50";
                        Excellents = dvCCs.Count;
                        dvCCs.RowFilter = "PointId='" + "南通市区" + "' and AQIValue <> '' and AQIValue<=100 and AQIValue>50";
                        Goods = dvCCs.Count;
                        dvCCs.RowFilter = "PointId='" + "南通市区" + "' and AQIValue <> '' and AQIValue<=300 and AQIValue>200";
                        Severes = dvCCs.Count;
                        //
                        dvCCs.RowFilter = "PointId='" + "南通市区" + "'  and (AQIValue<0 or AQIValue is null)";
                        decimal Nones = dvCCs.Count;

                        dt.Rows[i]["LastP"] = Severes.ToString();
                        dt.Rows[i]["ThisP"] = Severe.ToString();
                        if (Severe - Severes > 0)
                        {
                            dt.Rows[i]["PCompare"] = "↑" + (Severe - Severes).ToString();
                        }
                        else if (Severe - Severes < 0)
                        {
                            dt.Rows[i]["PCompare"] = "↓" + (Severes - Severe).ToString();
                        }
                        else
                        {
                            dt.Rows[i]["PCompare"] = "持平";
                        }
                        if (alls == 0)
                        {
                            dt.Rows[i]["LastC"] = 0;
                        }
                        else
                        {
                            dt.Rows[i]["LastC"] = Math.Round((Excellents + Goods) * 100 / (alls-Nones), 1, MidpointRounding.AwayFromZero).ToString();
                        }
                        if (all == 0)
                        {
                            dt.Rows[i]["LastC"] = 0;
                        }
                        else
                        {
                            dt.Rows[i]["ThisC"] = Math.Round((Excellent + Good) * 100 / (all-None), 1, MidpointRounding.AwayFromZero).ToString();
                        }

                        if (Math.Round((Excellent + Good) * 100 / (all - None), 1, MidpointRounding.AwayFromZero) - Math.Round((Excellents + Goods) * 100 / (alls - Nones), 1, MidpointRounding.AwayFromZero) > 0)
                        {
                            dt.Rows[i]["CCompare"] = "↑" + (Math.Round((Excellent + Good) * 100 / (all - None), 1, MidpointRounding.AwayFromZero) - Math.Round((Excellents + Goods) * 100 / (alls - Nones), 1, MidpointRounding.AwayFromZero)).ToString();
                        }
                        else if (Math.Round((Excellent + Good) * 100 / (all - None), 1, MidpointRounding.AwayFromZero) - Math.Round((Excellents + Goods) * 100 / (alls - Nones), 1, MidpointRounding.AwayFromZero) < 0)
                        {
                            dt.Rows[i]["CCompare"] = "↓" + (Math.Round((Excellents + Goods) * 100 / (alls - Nones), 1, MidpointRounding.AwayFromZero) - Math.Round((Excellent + Good) * 100 / (all - None), 1, MidpointRounding.AwayFromZero)).ToString();
                        }
                        else
                        {
                            dt.Rows[i]["CCompare"] = "持平";
                        }
                        #endregion
                    }
                    else if (i == 2)
                    {
                        #region 港闸区
                        dvGZ.RowFilter = "PointId='" + "南通市区" + "'";
                        all = dvGZ.Count;
                        dvGZ.RowFilter = "PointId='" + "南通市区" + "' and AQIValue <> '' and AQIValue<=50";
                        Excellent = dvGZ.Count;
                        dvGZ.RowFilter = "PointId='" + "南通市区" + "' and AQIValue <> '' and AQIValue<=100 and AQIValue>50";
                        Good = dvGZ.Count;
                        dvGZ.RowFilter = "PointId='" + "南通市区" + "' and AQIValue <> '' and AQIValue<=300 and AQIValue>200";
                        Severe = dvGZ.Count;
                        //
                        dvGZ.RowFilter = "PointId='" + "南通市区" + "'  and (AQIValue<0 or AQIValue is null)";
                        decimal None = dvGZ.Count;

                        dvGZs.RowFilter = "PointId='" + "南通市区" + "'";
                        alls = dvGZs.Count;
                        dvGZs.RowFilter = "PointId='" + "南通市区" + "' and AQIValue <> '' and AQIValue<=50";
                        Excellents = dvGZs.Count;
                        dvGZs.RowFilter = "PointId='" + "南通市区" + "' and AQIValue <> '' and AQIValue<=100 and AQIValue>50";
                        Goods = dvGZs.Count;
                        dvGZs.RowFilter = "PointId='" + "南通市区" + "' and AQIValue <> '' and AQIValue<=300 and AQIValue>200";
                        Severes = dvGZs.Count;
                        //
                        dvGZs.RowFilter = "PointId='" + "南通市区" + "'  and (AQIValue<0 or AQIValue is null)";
                        decimal Nones = dvGZs.Count;

                        dt.Rows[i]["LastP"] = Severes.ToString();
                        dt.Rows[i]["ThisP"] = Severe.ToString();
                        if (Severe - Severes > 0)
                        {
                            dt.Rows[i]["PCompare"] = "↑" + (Severe - Severes).ToString();
                        }
                        else if (Severe - Severes < 0)
                        {
                            dt.Rows[i]["PCompare"] = "↓" + (Severes - Severe).ToString();
                        }
                        else
                        {
                            dt.Rows[i]["PCompare"] = "持平";
                        }
                        if (alls == 0)
                        {
                            dt.Rows[i]["LastC"] = 0;
                        }
                        else
                        {
                            dt.Rows[i]["LastC"] = Math.Round((Excellents + Goods) * 100 / (alls-Nones), 1, MidpointRounding.AwayFromZero).ToString();
                        }
                        if (all == 0)
                        {
                            dt.Rows[i]["LastC"] = 0;
                        }
                        else
                        {
                            dt.Rows[i]["ThisC"] = Math.Round((Excellent + Good) * 100 / (all-None), 1, MidpointRounding.AwayFromZero).ToString();
                        }

                        if (Math.Round((Excellent + Good) * 100 / (all - None), 1, MidpointRounding.AwayFromZero) - Math.Round((Excellents + Goods) * 100 / (alls - Nones), 1, MidpointRounding.AwayFromZero) > 0)
                        {
                            dt.Rows[i]["CCompare"] = "↑" + (Math.Round((Excellent + Good) * 100 / (all - None), 1, MidpointRounding.AwayFromZero) - Math.Round((Excellents + Goods) * 100 / (alls - Nones), 1, MidpointRounding.AwayFromZero)).ToString();
                        }
                        else if (Math.Round((Excellent + Good) * 100 / (all - None), 1, MidpointRounding.AwayFromZero) - Math.Round((Excellents + Goods) * 100 / (alls - Nones), 1, MidpointRounding.AwayFromZero) < 0)
                        {
                            dt.Rows[i]["CCompare"] = "↓" + (Math.Round((Excellents + Goods) * 100 / (alls - Nones), 1, MidpointRounding.AwayFromZero) - Math.Round((Excellent + Good) * 100 / (all - None), 1, MidpointRounding.AwayFromZero)).ToString();
                        }
                        else
                        {
                            dt.Rows[i]["CCompare"] = "持平";
                        }
                        #endregion
                    }
                    else if (i == 3)
                    {
                        #region 开发区
                        dvKF.RowFilter = "PointId='" + "南通市区" + "'";
                        all = dvKF.Count;
                        dvKF.RowFilter = "PointId='" + "南通市区" + "' and AQIValue <> '' and AQIValue<=50";
                        Excellent = dvKF.Count;
                        dvKF.RowFilter = "PointId='" + "南通市区" + "' and AQIValue <> '' and AQIValue<=100 and AQIValue>50";
                        Good = dvKF.Count;
                        dvKF.RowFilter = "PointId='" + "南通市区" + "' and AQIValue <> '' and AQIValue<=300 and AQIValue>200";
                        Severe = dvKF.Count;
                        //
                        dvKF.RowFilter = "PointId='" + "南通市区" + "'  and (AQIValue<0 or AQIValue is null)";
                        decimal None = dvKF.Count;


                        dvKFs.RowFilter = "PointId='" + "南通市区" + "'";
                        alls = dvKFs.Count;
                        dvKFs.RowFilter = "PointId='" + "南通市区" + "' and AQIValue <> '' and AQIValue<=50";
                        Excellents = dvKFs.Count;
                        dvKFs.RowFilter = "PointId='" + "南通市区" + "' and AQIValue <> '' and AQIValue<=100 and AQIValue>50";
                        Goods = dvKFs.Count;
                        dvKFs.RowFilter = "PointId='" + "南通市区" + "' and AQIValue <> '' and AQIValue<=300 and AQIValue>200";
                        Severes = dvKFs.Count;
                        //
                        dvKFs.RowFilter = "PointId='" + "南通市区" + "'  and (AQIValue<0 or AQIValue is null)";
                        decimal Nones = dvKFs.Count;

                        dt.Rows[i]["LastP"] = Severes.ToString();
                        dt.Rows[i]["ThisP"] = Severe.ToString();
                        if (Severe - Severes > 0)
                        {
                            dt.Rows[i]["PCompare"] = "↑" + (Severe - Severes).ToString();
                        }
                        else if (Severe - Severes < 0)
                        {
                            dt.Rows[i]["PCompare"] = "↓" + (Severes - Severe).ToString();
                        }
                        else
                        {
                            dt.Rows[i]["PCompare"] = "持平";
                        }
                        if (alls == 0)
                        {
                            dt.Rows[i]["LastC"] = 0;
                        }
                        else
                        {
                            dt.Rows[i]["LastC"] = Math.Round((Excellents + Goods) * 100 / (alls-Nones), 1, MidpointRounding.AwayFromZero).ToString();
                        }
                        if (all == 0)
                        {
                            dt.Rows[i]["LastC"] = 0;
                        }
                        else
                        {
                            dt.Rows[i]["ThisC"] = Math.Round((Excellent + Good) * 100 / (all-None), 1, MidpointRounding.AwayFromZero).ToString();
                        }

                        if (Math.Round((Excellent + Good) * 100 / (all - None), 1, MidpointRounding.AwayFromZero) - Math.Round((Excellents + Goods) * 100 / (alls - Nones), 1, MidpointRounding.AwayFromZero) > 0)
                        {
                            dt.Rows[i]["CCompare"] = "↑" + (Math.Round((Excellent + Good) * 100 / (all - None), 1, MidpointRounding.AwayFromZero) - Math.Round((Excellents + Goods) * 100 / (alls - Nones), 1, MidpointRounding.AwayFromZero)).ToString();
                        }
                        else if (Math.Round((Excellent + Good) * 100 / (all - None), 1, MidpointRounding.AwayFromZero) - Math.Round((Excellents + Goods) * 100 / (alls - Nones), 1, MidpointRounding.AwayFromZero) < 0)
                        {
                            dt.Rows[i]["CCompare"] = "↓" + (Math.Round((Excellents + Goods) * 100 / (alls - Nones), 1, MidpointRounding.AwayFromZero) - Math.Round((Excellent + Good) * 100 / (all - None), 1, MidpointRounding.AwayFromZero)).ToString();
                        }
                        else
                        {
                            dt.Rows[i]["CCompare"] = "持平";
                        }
                        #endregion
                    }
                    else
                    {
                        #region 常规区县
                        dv.RowFilter = "PointId='" + PointNames[i] + "'";
                        all = dv.Count;
                        dv.RowFilter = "PointId='" + PointNames[i] + "' and AQIValue <> '' and AQIValue<=50";
                        Excellent = dv.Count;
                        dv.RowFilter = "PointId='" + PointNames[i] + "' and AQIValue <> '' and AQIValue<=100 and AQIValue>50";
                        Good = dv.Count;
                        dv.RowFilter = "PointId='" + PointNames[i] + "' and AQIValue <> '' and AQIValue<=300 and AQIValue>200";
                        Severe = dv.Count;
                        //
                        dv.RowFilter = "PointId='" + PointNames[i] + "'  and (AQIValue<0 or AQIValue is null)";
                        decimal None = dv.Count;
                        dvs.RowFilter = "PointId='" + PointNames[i] + "'";
                        alls = dvs.Count;
                        dvs.RowFilter = "PointId='" + PointNames[i] + "' and AQIValue <> '' and AQIValue<=50";
                        Excellents = dvs.Count;
                        dvs.RowFilter = "PointId='" + PointNames[i] + "' and AQIValue <> '' and AQIValue<=100 and AQIValue>50";
                        Goods = dvs.Count;
                        dvs.RowFilter = "PointId='" + PointNames[i] + "' and AQIValue <> '' and AQIValue<=300 and AQIValue>200";
                        Severes = dvs.Count;
                        //
                        dvs.RowFilter = "PointId='" + PointNames[i] + "'  and (AQIValue<0 or AQIValue is null)";
                        decimal Nones = dvs.Count;

                        dt.Rows[i]["LastP"] = Severes.ToString();
                        dt.Rows[i]["ThisP"] = Severe.ToString();
                        if (Severe - Severes > 0)
                        {
                            dt.Rows[i]["PCompare"] = "↑" + (Severe - Severes).ToString();
                        }
                        else if (Severe - Severes < 0)
                        {
                            dt.Rows[i]["PCompare"] = "↓" + (Severes - Severe).ToString();
                        }
                        else
                        {
                            dt.Rows[i]["PCompare"] = "持平";
                        }
                        if (alls == 0)
                        {
                            dt.Rows[i]["LastC"] = 0;
                        }
                        else
                        {
                            dt.Rows[i]["LastC"] = Math.Round((Excellents + Goods) * 100 / (alls-Nones), 1, MidpointRounding.AwayFromZero).ToString();
                        }
                        if (all == 0)
                        {
                            dt.Rows[i]["LastC"] = 0;
                        }
                        else
                        {
                            dt.Rows[i]["ThisC"] = Math.Round((Excellent + Good) * 100 / (all-None), 1, MidpointRounding.AwayFromZero).ToString();
                        }

                        if (Math.Round((Excellent + Good) * 100 / (all - None), 1, MidpointRounding.AwayFromZero) - Math.Round((Excellents + Goods) * 100 / (alls - Nones), 1, MidpointRounding.AwayFromZero) > 0)
                        {
                            dt.Rows[i]["CCompare"] = "↑" + (Math.Round((Excellent + Good) * 100 / (all - None), 1, MidpointRounding.AwayFromZero) - Math.Round((Excellents + Goods) * 100 / (alls - Nones), 1, MidpointRounding.AwayFromZero)).ToString();
                        }
                        else if (Math.Round((Excellent + Good) * 100 / (all - None), 1, MidpointRounding.AwayFromZero) - Math.Round((Excellents + Goods) * 100 / (alls - Nones), 1, MidpointRounding.AwayFromZero) < 0)
                        {
                            dt.Rows[i]["CCompare"] = "↓" + (Math.Round((Excellents + Goods) * 100 / (alls - Nones), 1, MidpointRounding.AwayFromZero) - Math.Round((Excellent + Good) * 100 / (all - None), 1, MidpointRounding.AwayFromZero)).ToString();
                        }
                        else
                        {
                            dt.Rows[i]["CCompare"] = "持平";
                        }
                        #endregion
                    }

                }
                #endregion 
            }
            #endregion
            #region 各县(市)、区空气质量变化情况
            else
            {
                #region 创建表结构
                dt.Columns.Add("PointName", typeof(string));    //城市
                dt.Columns.Add("Density", typeof(string));      //浓度值
                dt.Columns.Add("DCompare", typeof(string));     //同比变化情况
                dt.Columns.Add("Drop", typeof(string));         //降幅
                dt.Columns.Add("Proportion", typeof(string));   //比例
                dt.Columns.Add("PCompares", typeof(string));     //同比变化情况
                dt.Columns.Add("Increase", typeof(string));     //升幅
                dt.Columns.Add("Evaluation", typeof(string));   //综合评价

                #endregion
                #region 常规县区及崇川区、港闸区、开发区数据源
                DataView dvTarget = g_DatabaseHelper.ExecuteDataView("select CityName,ReductionTarget,IncreaseTarget from dbo.DT_AirQualityTarget", BaseDataConnection);
                DataView dv = m_AQICalculateService.GetRegionAQI(portIds, mBegion, mEnd, 24, "2").AsDataView();
                DataView dvCC = m_AQICalculateService.GetRegionAQI(cc, mBegion, mEnd, 24, "2").AsDataView();
                DataView dvGZ = m_AQICalculateService.GetRegionAQI(gz, mBegion, mEnd, 24, "2").AsDataView();
                DataView dvKF = m_AQICalculateService.GetRegionAQI(kf, mBegion, mEnd, 24, "2").AsDataView();

                DataView dvs = m_AQICalculateService.GetRegionAQI(portIds, Convert.ToDateTime(years + "-" + mBegion.Month + "-" + mBegion.Day), Convert.ToDateTime(years + "-" + mEnd.Month + "-" + mEnd.Day), 24, "2").AsDataView();
                DataView dvCCs = m_AQICalculateService.GetRegionAQI(cc, Convert.ToDateTime(years + "-" + mBegion.Month + "-" + mBegion.Day), Convert.ToDateTime(years + "-" + mEnd.Month + "-" + mEnd.Day), 24, "2").AsDataView();
                DataView dvGZs = m_AQICalculateService.GetRegionAQI(gz, Convert.ToDateTime(years + "-" + mBegion.Month + "-" + mBegion.Day), Convert.ToDateTime(years + "-" + mEnd.Month + "-" + mEnd.Day), 24, "2").AsDataView();
                DataView dvKFs = m_AQICalculateService.GetRegionAQI(kf, Convert.ToDateTime(years + "-" + mBegion.Month + "-" + mBegion.Day), Convert.ToDateTime(years + "-" + mEnd.Month + "-" + mEnd.Day), 24, "2").AsDataView();

                
                #endregion
                #region dt添加行
                for (int i = 0; i < PointNames.Length; i++)
                {
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                    dt.Rows[i]["PointName"] = PointNames[i];
                }
                #endregion
                #region 往dt填数据
                for (int i = 0; i < PointNames.Length; i++)
                {
                    if (i == 1)
                    {
                        #region 崇川区
                        decimal targetl = 0;
                        decimal targeth = 0;
                        dvCC.RowFilter = "PointId='" + "南通市区" + "'";
                        all = dvCC.Count;
                        dvCC.RowFilter = "PointId='" + "南通市区" + "' and AQIValue <> '' and AQIValue<=50";
                        Excellent = dvCC.Count;
                        dvCC.RowFilter = "PointId='" + "南通市区" + "' and AQIValue <> '' and AQIValue<=100 and AQIValue>50";
                        Good = dvCC.Count;
                        //dvCC.RowFilter = "PointId='" + "南通市区" + "' and AQIValue <> '' and AQIValue<=300 and AQIValue>200";
                        //Severe = dvCC.Count;
                        //
                        dvGZ.RowFilter = "PointId='" + "南通市区" + "'  and (AQIValue<0 or AQIValue is null)";
                        decimal None = dvGZ.Count;

                        dvCCs.RowFilter = "PointId='" + "南通市区" + "'";
                        alls = dvCCs.Count;
                        dvCCs.RowFilter = "PointId='" + "南通市区" + "' and AQIValue <> '' and AQIValue<=50";
                        Excellents = dvCCs.Count;
                        dvCCs.RowFilter = "PointId='" + "南通市区" + "' and AQIValue <> '' and AQIValue<=100 and AQIValue>50";
                        Goods = dvCCs.Count;
                        //dvCCs.RowFilter = "PointId='" + "南通市区" + "' and AQIValue <> '' and AQIValue<=300 and AQIValue>200";
                        //Severes = dvCCs.Count;
                        //
                        dvGZs.RowFilter = "PointId='" + "南通市区" + "'  and (AQIValue<0 or AQIValue is null)";
                        decimal Nones = dvGZs.Count;

                        //本期年份
                        dvCC.RowFilter = "PointId='" + "南通市区" + "'";
                        Severe = Math.Round(Convert.ToDecimal(dvCC.ToTable().AsEnumerable().Select(t => Convert.ToDecimal(t.Field<string>("PM25"))).Average()) * 1000, 1, MidpointRounding.AwayFromZero);

                        decimal Severee, Severees;
                        string[] ids=new string[]{"187","188"};

                        Severee = DecimalExtension.GetRoundValue(Convert.ToDecimal(m_AQICalculateService.GetRegionValueByTime(ids, "a34004", mBegion, mEnd, 24, "2")) * 1000, 1);
                        Severees = DecimalExtension.GetRoundValue(Convert.ToDecimal(m_AQICalculateService.GetRegionValueByTime(ids, "a34004", Convert.ToDateTime(years + "-" + mBegion.Month + "-" + mBegion.Day), Convert.ToDateTime(years + "-" + mEnd.Month + "-" + mEnd.Day), 24, "2")) * 1000, 1); 

                        //比对年份
                        dvCCs.RowFilter = "PointId='" + "南通市区" + "'";
                        Severes = Math.Round(Convert.ToDecimal(dvCCs.ToTable().AsEnumerable().Select(t => Convert.ToDecimal(t.Field<string>("PM25"))).Average()) * 1000, 1, MidpointRounding.AwayFromZero);

                        dt.Rows[i]["Density"] = Severee;
                        if (Severee - Severees > 0)
                        {
                            dt.Rows[i]["DCompare"] = "+" + Math.Round(((Severee - Severees) * 100 / Severees), 1, MidpointRounding.AwayFromZero).ToString();
                            targetl = Math.Round(((Severee - Severees) * 100 / Severees), 1, MidpointRounding.AwayFromZero);
                        }
                        else if (Severee - Severees < 0)
                        {
                            dt.Rows[i]["DCompare"] = "-" + Math.Round(((Severees - Severee) * 100 / Severees), 1, MidpointRounding.AwayFromZero).ToString();
                            targetl = 0 - Math.Round(((Severees - Severee) * 100 / Severees), 1, MidpointRounding.AwayFromZero);
                        }
                        else
                        {
                            dt.Rows[i]["DCompare"] = "持平";
                            targetl = 0;
                        }
                        dvTarget.RowFilter = "CityName='" + PointNames[i] + "'";
                        dt.Rows[i]["Drop"] = dvTarget.ToTable().Rows[0]["ReductionTarget"].ToString();
                        if (all == 0)
                        {
                            dt.Rows[i]["Proportion"] = 0;
                        }
                        else
                        {
                            dt.Rows[i]["Proportion"] = Math.Round((Excellent + Good) * 100 / (all - None), 1, MidpointRounding.AwayFromZero).ToString();
                        }
                        if (Math.Round((Excellent + Good) * 100 / (all - None), 1, MidpointRounding.AwayFromZero) - Math.Round((Excellents + Goods) * 100 / (alls - Nones), 1, MidpointRounding.AwayFromZero) > 0)
                        {
                            dt.Rows[i]["PCompares"] = "+" + (Math.Round((Excellent + Good) * 100 / (all - None), 1, MidpointRounding.AwayFromZero) - Math.Round((Excellents + Goods) * 100 / (alls - Nones), 1, MidpointRounding.AwayFromZero)).ToString();
                            targeth = Math.Round((Excellent + Good) * 100 / (all - None), 1, MidpointRounding.AwayFromZero) - Math.Round((Excellents + Goods) * 100 / (alls - Nones), 1, MidpointRounding.AwayFromZero);
                        }
                        else if (Math.Round((Excellent + Good) * 100 / (all - None), 1, MidpointRounding.AwayFromZero) - Math.Round((Excellents + Goods) * 100 / (alls - Nones), 1, MidpointRounding.AwayFromZero) < 0)
                        {
                            dt.Rows[i]["PCompares"] = "-" + (Math.Round((Excellents + Goods) * 100 / (alls - Nones), 1, MidpointRounding.AwayFromZero) - Math.Round((Excellent + Good) * 100 / (all - None), 1, MidpointRounding.AwayFromZero)).ToString();
                            targeth =0- Math.Round((Excellents + Goods) * 100 / (alls - Nones), 1, MidpointRounding.AwayFromZero) - Math.Round((Excellent + Good) * 100 / (all - None), 1, MidpointRounding.AwayFromZero);
                        }
                        else
                        {
                            dt.Rows[i]["PCompares"] = "持平";
                            targeth = 0;
                        }
                        dvTarget.RowFilter = "CityName='" + PointNames[i] + "'";
                        dt.Rows[i]["Increase"] = dvTarget.ToTable().Rows[0]["IncreaseTarget"].ToString();
                        if ((targetl <= Convert.ToDecimal(dvTarget.ToTable().Rows[0]["ReductionTarget"].ToString())) && (targeth >= Convert.ToDecimal(dvTarget.ToTable().Rows[0]["IncreaseTarget"].ToString())))
                        {
                            dt.Rows[i]["Evaluation"] = "达标";
                        }
                        else
                        {
                            dt.Rows[i]["Evaluation"] = "不达标";
                        }
                        #endregion
                    }
                    else if (i == 2)
                    {
                        #region 港闸区
                        decimal targetl = 0;
                        decimal targeth = 0;
                        dvGZ.RowFilter = "PointId='" + "南通市区" + "'";
                        all = dvGZ.Count;
                        dvGZ.RowFilter = "PointId='" + "南通市区" + "' and AQIValue <> '' and AQIValue<=50";
                        Excellent = dvGZ.Count;
                        dvGZ.RowFilter = "PointId='" + "南通市区" + "' and AQIValue <> '' and AQIValue<=100 and AQIValue>50";
                        Good = dvGZ.Count;
                        //
                        dvGZ.RowFilter = "PointId='" + "南通市区" + "'  and (AQIValue<0 or AQIValue is null)";
                        decimal None = dvGZ.Count;

                        dvGZs.RowFilter = "PointId='" + "南通市区" + "'";
                        alls = dvGZs.Count;
                        dvGZs.RowFilter = "PointId='" + "南通市区" + "' and AQIValue <> '' and AQIValue<=50";
                        Excellents = dvGZs.Count;
                        dvGZs.RowFilter = "PointId='" + "南通市区" + "' and AQIValue <> '' and AQIValue<=100 and AQIValue>50";
                        Goods = dvGZs.Count;
                        //
                        dvGZs.RowFilter = "PointId='" + "南通市区" + "'  and (AQIValue<0 or AQIValue is null)";
                        decimal Nones = dvGZs.Count;

                        dvGZ.RowFilter = "PointId='" + "南通市区" + "'";
                        Severe = Math.Round(Convert.ToDecimal(dvGZ.ToTable().AsEnumerable().Select(t => Convert.ToDecimal(t.Field<string>("PM25"))).Average()) * 1000, 1, MidpointRounding.AwayFromZero);

                        dvGZs.RowFilter = "PointId='" + "南通市区" + "'";
                        Severes = Math.Round(Convert.ToDecimal(dvGZs.ToTable().AsEnumerable().Select(t => Convert.ToDecimal(t.Field<string>("PM25"))).Average()) * 1000, 1, MidpointRounding.AwayFromZero);

                        decimal Severee, Severees;
                        string[] ids = new string[] { "190" };
                        Severee = DecimalExtension.GetRoundValue(Convert.ToDecimal(m_AQICalculateService.GetRegionValueByTime(ids, "a34004", mBegion, mEnd, 24, "2")) * 1000, 1);
                        Severees = DecimalExtension.GetRoundValue(Convert.ToDecimal(m_AQICalculateService.GetRegionValueByTime(ids, "a34004", Convert.ToDateTime(years + "-" + mBegion.Month + "-" + mBegion.Day), Convert.ToDateTime(years + "-" + mEnd.Month + "-" + mEnd.Day), 24, "2")) * 1000, 1); 


                        dt.Rows[i]["Density"] = Severee;
                        if (Severee - Severees > 0)
                        {
                            dt.Rows[i]["DCompare"] = "+" + Math.Round(((Severee - Severees) * 100 / Severees), 1, MidpointRounding.AwayFromZero).ToString();
                            targetl = Math.Round(((Severee - Severees) * 100 / Severees), 1, MidpointRounding.AwayFromZero);
                        }
                        else if (Severee - Severees < 0)
                        {
                            dt.Rows[i]["DCompare"] = "-" + Math.Round(((Severees - Severee) * 100 / Severees), 1, MidpointRounding.AwayFromZero).ToString();
                            targetl = 0 - Math.Round(((Severees - Severee) * 100 / Severees), 1, MidpointRounding.AwayFromZero);
                        }
                        else
                        {
                            dt.Rows[i]["DCompare"] = "持平";
                            targetl = 0;
                        }
                        dvTarget.RowFilter = "CityName='" + PointNames[i] + "'";
                        dt.Rows[i]["Drop"] = dvTarget.ToTable().Rows[0]["ReductionTarget"].ToString();
                        if (all == 0)
                        {
                            dt.Rows[i]["Proportion"] = 0;
                        }
                        else
                        {
                            dt.Rows[i]["Proportion"] = Math.Round((Excellent + Good) * 100 / (all - None), 1, MidpointRounding.AwayFromZero).ToString();
                        }
                        if (Math.Round((Excellent + Good) * 100 / (all - None), 1, MidpointRounding.AwayFromZero) - Math.Round((Excellents + Goods) * 100 / (alls - Nones), 1, MidpointRounding.AwayFromZero) > 0)
                        {
                            dt.Rows[i]["PCompares"] = "+" + (Math.Round((Excellent + Good) * 100 / (all - None), 1, MidpointRounding.AwayFromZero) - Math.Round((Excellents + Goods) * 100 / (alls - Nones), 1, MidpointRounding.AwayFromZero)).ToString();
                            targeth = Math.Round((Excellent + Good) * 100 / (all - None), 1, MidpointRounding.AwayFromZero) - Math.Round((Excellents + Goods) * 100 / (alls - Nones), 1, MidpointRounding.AwayFromZero);
                        }
                        else if (Math.Round((Excellent + Good) * 100 / (all - None), 1, MidpointRounding.AwayFromZero) - Math.Round((Excellents + Goods) * 100 / (alls - Nones), 1, MidpointRounding.AwayFromZero) < 0)
                        {
                            dt.Rows[i]["PCompares"] = "-" + (Math.Round((Excellents + Goods) * 100 / (alls - Nones), 1, MidpointRounding.AwayFromZero) - Math.Round((Excellent + Good) * 100 / (all - None), 1, MidpointRounding.AwayFromZero)).ToString();
                            targeth =0- Math.Round((Excellents + Goods) * 100 / (alls - Nones), 1, MidpointRounding.AwayFromZero) - Math.Round((Excellent + Good) * 100 / (all - None), 1, MidpointRounding.AwayFromZero);
                        }
                        else
                        {
                            dt.Rows[i]["PCompares"] = "持平";
                            targeth = 0;
                        }
                        dvTarget.RowFilter = "CityName='" + PointNames[i] + "'";
                        dt.Rows[i]["Increase"] = dvTarget.ToTable().Rows[0]["IncreaseTarget"].ToString();
                        if ((targetl <= Convert.ToDecimal(dt.Rows[i]["Drop"])) && (targeth >= Convert.ToDecimal(dt.Rows[i]["Increase"])))
                        {
                            dt.Rows[i]["Evaluation"] = "达标";
                        }
                        else
                        {
                            dt.Rows[i]["Evaluation"] = "不达标";
                        }
                        #endregion
                    }
                    else if (i == 3)
                    {
                        #region 开发区
                        decimal targetl = 0;
                        decimal targeth = 0;
                        dvKF.RowFilter = "PointId='" + "南通市区" + "'";
                        all = dvKF.Count;
                        dvKF.RowFilter = "PointId='" + "南通市区" + "' and AQIValue <> '' and AQIValue<=50";
                        Excellent = dvKF.Count;
                        dvKF.RowFilter = "PointId='" + "南通市区" + "' and AQIValue <> '' and AQIValue<=100 and AQIValue>50";
                        Good = dvKF.Count;
                        //
                        dvKF.RowFilter = "PointId='" + "南通市区" + "'  and (AQIValue<0 or AQIValue is null)";
                        decimal None = dvKF.Count;

                        dvKFs.RowFilter = "PointId='" + "南通市区" + "'";
                        alls = dvKFs.Count;
                        dvKFs.RowFilter = "PointId='" + "南通市区" + "' and AQIValue <> '' and AQIValue<=50";
                        Excellents = dvKFs.Count;
                        dvKFs.RowFilter = "PointId='" + "南通市区" + "' and AQIValue <> '' and AQIValue<=100 and AQIValue>50";
                        Goods = dvKFs.Count;
                        //
                        dvKFs.RowFilter = "PointId='" + "南通市区" + "'  and (AQIValue<0 or AQIValue is null)";
                        decimal Nones = dvKFs.Count;

                        dvKF.RowFilter = "PointId='" + "南通市区" + "'";
                        Severe = Math.Round(Convert.ToDecimal(dvKF.ToTable().AsEnumerable().Select(t => Convert.ToDecimal(t.Field<string>("PM25"))).Average()) * 1000, 1, MidpointRounding.AwayFromZero);

                        dvKFs.RowFilter = "PointId='" + "南通市区" + "'";
                        Severes = Math.Round(Convert.ToDecimal(dvKFs.ToTable().AsEnumerable().Select(t => Convert.ToDecimal(t.Field<string>("PM25"))).Average()) * 1000, 1, MidpointRounding.AwayFromZero);

                        decimal Severee, Severees;
                        string[] ids = new string[] { "189" };
                        Severee = DecimalExtension.GetRoundValue(Convert.ToDecimal(m_AQICalculateService.GetRegionValueByTime(ids, "a34004", mBegion, mEnd, 24, "2")) * 1000, 1);
                        Severees = DecimalExtension.GetRoundValue(Convert.ToDecimal(m_AQICalculateService.GetRegionValueByTime(ids, "a34004", Convert.ToDateTime(years + "-" + mBegion.Month + "-" + mBegion.Day), Convert.ToDateTime(years + "-" + mEnd.Month + "-" + mEnd.Day), 24, "2")) * 1000, 1); 

                        dt.Rows[i]["Density"] = Severee;
                        if (Severee - Severees > 0)
                        {
                            dt.Rows[i]["DCompare"] = "+" + Math.Round(((Severee - Severees) * 100 / Severees), 1, MidpointRounding.AwayFromZero).ToString();
                            targetl = Math.Round(((Severee - Severees) * 100 / Severees), 1, MidpointRounding.AwayFromZero);
                        }
                        else if (Severee - Severees < 0)
                        {
                            dt.Rows[i]["DCompare"] = "-" + Math.Round(((Severees - Severee) * 100 / Severees), 1, MidpointRounding.AwayFromZero).ToString();
                            targetl = 0 - Math.Round(((Severees - Severee) * 100 / Severees), 1, MidpointRounding.AwayFromZero);
                        }
                        else
                        {
                            dt.Rows[i]["DCompare"] = "持平";
                            targetl = 0;
                        }
                        dvTarget.RowFilter = "CityName='" + PointNames[i] + "'";
                        dt.Rows[i]["Drop"] = dvTarget.ToTable().Rows[0]["ReductionTarget"].ToString();
                        if (all == 0)
                        {
                            dt.Rows[i]["Proportion"] = 0;
                        }
                        else
                        {
                            dt.Rows[i]["Proportion"] = Math.Round((Excellent + Good) * 100 / (all - None), 1, MidpointRounding.AwayFromZero).ToString();
                        }
                        if (Math.Round((Excellent + Good) * 100 / (all - None), 1, MidpointRounding.AwayFromZero) - Math.Round((Excellents + Goods) * 100 / (alls - Nones), 1, MidpointRounding.AwayFromZero) > 0)
                        {
                            dt.Rows[i]["PCompares"] = "+" + (Math.Round((Excellent + Good) * 100 / (all - None), 1, MidpointRounding.AwayFromZero) - Math.Round((Excellents + Goods) * 100 / (alls - Nones), 1, MidpointRounding.AwayFromZero)).ToString();
                            targeth = Math.Round((Excellent + Good) * 100 / (all - None), 1, MidpointRounding.AwayFromZero) - Math.Round((Excellents + Goods) * 100 / (alls - Nones), 1, MidpointRounding.AwayFromZero);
                        }
                        else if (Math.Round((Excellent + Good) * 100 / (all - None), 1, MidpointRounding.AwayFromZero) - Math.Round((Excellents + Goods) * 100 / (alls - Nones), 1, MidpointRounding.AwayFromZero) < 0)
                        {
                            dt.Rows[i]["PCompares"] = "-" + (Math.Round((Excellents + Goods) * 100 / (alls - Nones), 1, MidpointRounding.AwayFromZero) - Math.Round((Excellent + Good) * 100 / (all - None), 1, MidpointRounding.AwayFromZero)).ToString();
                            targeth =0- Math.Round((Excellents + Goods) * 100 / (alls - Nones), 1, MidpointRounding.AwayFromZero) - Math.Round((Excellent + Good) * 100 / (all - None), 1, MidpointRounding.AwayFromZero);
                        }
                        else
                        {
                            dt.Rows[i]["PCompares"] = "持平";
                            targeth = 0;
                        }
                        dvTarget.RowFilter = "CityName='" + PointNames[i] + "'";
                        dt.Rows[i]["Increase"] = dvTarget.ToTable().Rows[0]["IncreaseTarget"].ToString();
                        if ((targetl <= Convert.ToDecimal(dt.Rows[i]["Drop"])) && (targeth >= Convert.ToDecimal(dt.Rows[i]["Increase"])))
                        {
                            dt.Rows[i]["Evaluation"] = "达标";
                        }
                        else
                        {
                            dt.Rows[i]["Evaluation"] = "不达标";
                        }

                        #endregion
                    }
                    else
                    {
                        #region 常规区县
                        decimal targetl = 0;
                        decimal targeth = 0;
                        dv.RowFilter = "PointId='" + PointNames[i] + "'";
                        all = dv.Count;
                        dv.RowFilter = "PointId='" + PointNames[i] + "' and AQIValue <> '' and AQIValue<=50";
                        Excellent = dv.Count;
                        dv.RowFilter = "PointId='" + PointNames[i] + "' and AQIValue <> '' and AQIValue<=100 and AQIValue>50";
                        Good = dv.Count;
                        //
                        dv.RowFilter = "PointId='" + PointNames[i] + "'  and (AQIValue<0 or AQIValue is null)";
                        decimal None = dv.Count;

                        dvs.RowFilter = "PointId='" + PointNames[i] + "'";
                        alls = dvs.Count;
                        dvs.RowFilter = "PointId='" + PointNames[i] + "' and AQIValue <> '' and AQIValue<=50";
                        Excellents = dvs.Count;
                        dvs.RowFilter = "PointId='" + PointNames[i] + "' and AQIValue <> '' and AQIValue<=100 and AQIValue>50";
                        Goods = dvs.Count;
                        //
                        dvs.RowFilter = "PointId='" + PointNames[i] + "'  and (AQIValue<0 or AQIValue is null)";
                        decimal Nones = dvs.Count;

                        dv.RowFilter = "PointId='" + PointNames[i] + "'";
                        Severe = Math.Round(Convert.ToDecimal(dv.ToTable().AsEnumerable().Select(t => Convert.ToDecimal(t.Field<string>("PM25"))).Average()) * 1000, 1, MidpointRounding.AwayFromZero);

                        dvs.RowFilter = "PointId='" + PointNames[i] + "'";
                        Severes = Math.Round(Convert.ToDecimal(dvs.ToTable().AsEnumerable().Select(t => Convert.ToDecimal(t.Field<string>("PM25"))).Average()) * 1000, 1, MidpointRounding.AwayFromZero);

                        DataTable da = m_MonitoringPointDAC.GetPointIdByCityName(PointNames[i]);

                        decimal Severee, Severees;
                        string[] ids = dtToArr(da);
                        if(i==0)
                        {
                            ids=new string[]{"187","188","189","190"};
                        }
                        Severee = DecimalExtension.GetRoundValue(Convert.ToDecimal(m_AQICalculateService.GetRegionValueByTime(ids, "a34004", mBegion, mEnd, 24, "2")) * 1000, 1);
                        Severees = DecimalExtension.GetRoundValue(Convert.ToDecimal(m_AQICalculateService.GetRegionValueByTime(ids, "a34004", Convert.ToDateTime(years + "-" + mBegion.Month + "-" + mBegion.Day), Convert.ToDateTime(years + "-" + mEnd.Month + "-" + mEnd.Day), 24, "2")) * 1000, 1); 


                        dt.Rows[i]["Density"] = Severee;
                        if (Severee - Severees > 0)
                        {
                            dt.Rows[i]["DCompare"] = "+" + Math.Round(((Severee - Severees) * 100 / Severees), 1, MidpointRounding.AwayFromZero).ToString();
                            targetl = Math.Round(((Severee - Severees) * 100 / Severees), 1, MidpointRounding.AwayFromZero);
                        }
                        else if (Severee - Severees < 0)
                        {
                            dt.Rows[i]["DCompare"] = "-" + Math.Round(((Severees - Severee) * 100 / Severees), 1, MidpointRounding.AwayFromZero).ToString();
                            targetl = 0 - Math.Round(((Severees - Severee) * 100 / Severees), 1, MidpointRounding.AwayFromZero);
                        }
                        else
                        {
                            dt.Rows[i]["DCompare"] = "持平";
                            targetl = 0;
                        }
                        dvTarget.RowFilter="CityName='" + PointNames[i] + "'";
                        dt.Rows[i]["Drop"] = dvTarget.ToTable().Rows[0]["ReductionTarget"].ToString();
                        if (all == 0)
                        {
                            dt.Rows[i]["Proportion"] = 0;
                        }
                        else
                        {
                            dt.Rows[i]["Proportion"] = Math.Round((Excellent + Good) * 100 / (all-None), 1, MidpointRounding.AwayFromZero).ToString();
                        }
                        if (Math.Round((Excellent + Good) * 100 / (all - None), 1, MidpointRounding.AwayFromZero) - Math.Round((Excellents + Goods) * 100 / (alls-Nones), 1, MidpointRounding.AwayFromZero) > 0)
                        {
                            dt.Rows[i]["PCompares"] = "+" + (Math.Round((Excellent + Good) * 100 / (all - None), 1, MidpointRounding.AwayFromZero) - Math.Round((Excellents + Goods) * 100 / (alls - Nones), 1, MidpointRounding.AwayFromZero)).ToString();
                            targeth = Math.Round((Excellent + Good) * 100 / (all - None), 1, MidpointRounding.AwayFromZero) - Math.Round((Excellents + Goods) * 100 / (alls - Nones), 1, MidpointRounding.AwayFromZero);
                        }
                        else if (Math.Round((Excellent + Good) * 100 / (all - None), 1, MidpointRounding.AwayFromZero) - Math.Round((Excellents + Goods) * 100 / (alls - Nones), 1, MidpointRounding.AwayFromZero) < 0)
                        {
                            dt.Rows[i]["PCompares"] = "-" + (Math.Round((Excellents + Goods) * 100 / (alls - Nones), 1, MidpointRounding.AwayFromZero) - Math.Round((Excellent + Good) * 100 / (all - None), 1, MidpointRounding.AwayFromZero)).ToString();
                            targeth =0- Math.Round((Excellents + Goods) * 100 / (alls - Nones), 1, MidpointRounding.AwayFromZero) - Math.Round((Excellent + Good) * 100 / (all - None), 1, MidpointRounding.AwayFromZero);
                        }
                        else
                        {
                            dt.Rows[i]["PCompares"] = "持平";
                            targeth = 0;
                        }
                        dvTarget.RowFilter = "CityName='" + PointNames[i] + "'";
                        dt.Rows[i]["Increase"] = dvTarget.ToTable().Rows[0]["IncreaseTarget"].ToString();
                        if ((targetl <= Convert.ToDecimal(dt.Rows[i]["Drop"])) && (targeth >= Convert.ToDecimal(dt.Rows[i]["Increase"])))
                        {
                            dt.Rows[i]["Evaluation"] = "达标";
                        }
                        else
                        {
                            dt.Rows[i]["Evaluation"] = "不达标";
                        }
                        #endregion
                    }

                }
                #endregion 
            }
            #endregion
            
            recordTotal = dt.Rows.Count;

            return dt.DefaultView;
        }

        /// <summary>
        /// 空气质量区域比对报表
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factorCodes"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <param name="year"></param>
        /// <param name="years"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <param name="recordTotal"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetCityProperComparisonPager(string[] portIds, DateTime mBegion, DateTime mEnd, string[] years, int pageSize
        , int pageNo, out int recordTotal, string orderBy = "PointId")
        {
            pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
            recordTotal = 0;
            DataTable dt = new DataTable();
            decimal all, Excellent, Good, Light, Moderate, Severe;
            string tstamp, compliance, excellent, good, light, moderate, severe, a21026, a21004, a21005, a05024, a34002, a34004;
            #region dt加列名
            dt.Columns.Add("Tstamp", typeof(string));
            dt.Columns.Add("Compliance", typeof(string));
            dt.Columns.Add("Excellent", typeof(string));
            dt.Columns.Add("Good", typeof(string));
            dt.Columns.Add("Light", typeof(string));
            dt.Columns.Add("Moderate", typeof(string));
            dt.Columns.Add("Severe", typeof(string));
            dt.Columns.Add("SO2", typeof(string));
            dt.Columns.Add("NO2", typeof(string));
            dt.Columns.Add("CO", typeof(string));
            dt.Columns.Add("O3", typeof(string));
            dt.Columns.Add("PM10", typeof(string));
            dt.Columns.Add("PM2.5", typeof(string));
            #endregion
            //DataView dv = GetPortDataPagerDayAQI(portIds, mBegion, mEnd, pageSize, pageNo, out recordTotal, orderBy);  // 本期

            #region 区域日浓度均值
            decimal? PM25PollutantValue = m_AQICalculateService.GetRegionValueByTime(portIds, "a34004", mBegion, mEnd, 24, "2");
            decimal? PM10PollutantValue = m_AQICalculateService.GetRegionValueByTime(portIds, "a34002", mBegion, mEnd, 24, "2");
            decimal? NO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(portIds, "a21004", mBegion, mEnd, 24, "2");
            decimal? SO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(portIds, "a21026", mBegion, mEnd, 24, "2");
            decimal? COPollutantValue = m_AQICalculateService.GetRegionValueByTime(portIds, "a21005", mBegion, mEnd, 24, "2");
            decimal? Max8HourO3PollutantValue = m_AQICalculateService.GetRegionValueByTime(portIds, "a05024", mBegion, mEnd, 8, "2");
            DataTable dtForCity = new DataTable();
            dtForCity.Columns.Add("a21026", typeof(string));
            dtForCity.Columns.Add("a21004", typeof(string));
            dtForCity.Columns.Add("a21005", typeof(string));
            dtForCity.Columns.Add("a05024", typeof(string));
            dtForCity.Columns.Add("a34002", typeof(string));
            dtForCity.Columns.Add("a34004", typeof(string));
            DataRow drc = dtForCity.NewRow();
            //drc["MonitoringRegionUid"] = name;
            drc["a34004"] = PM25PollutantValue.ToString();
            drc["a34002"] = PM10PollutantValue.ToString();
            drc["a21004"] = NO2PollutantValue.ToString();
            drc["a21026"] = SO2PollutantValue.ToString();
            drc["a21005"] = COPollutantValue.ToString();
            drc["a05024"] = Max8HourO3PollutantValue.ToString();
            dtForCity.Rows.Add(drc);
            #endregion
            DataView dv = dtForCity.DefaultView;

            //DataView dvs = pointDayAQI.GetDataPager(portIds, mBegion, mEnd, int.MaxValue, 0, out recordTotal, "DateTime,PointId");
            DataView dvs = m_AQICalculateService.GetRegionAQI(portIds, mBegion, mEnd, 24, "2").AsDataView();
            #region 加行
            for (int i=0;i<years.Length*2+1;i++)
            {
                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);
            }
            #endregion
            #region 天数统计
            all = dvs.Count;
            dvs.RowFilter = "AQIValue <> '' and AQIValue<=50";
            Excellent = dvs.Count;
            dvs.RowFilter = "AQIValue <> '' and AQIValue<=100 and AQIValue>50";
            Good = dvs.Count;
            dvs.RowFilter = "AQIValue <> '' and AQIValue<=150 and AQIValue>100";
            Light = dvs.Count;
            dvs.RowFilter = "AQIValue <> '' and AQIValue<=200 and AQIValue>150";
            Moderate = dvs.Count;
            dvs.RowFilter = "AQIValue <> '' and AQIValue<=300 and AQIValue>200";
            Severe = dvs.Count;
            //dv.RowFilter = "AQIValue <> '' and AQIValue>300";
            //decimal SeverelyPolluted = dv.Count;
            #endregion
            decimal k = Convert.ToDecimal(dv.ToTable().Rows[0]["a21026"].ToString() == "" ? 0 : dv.ToTable().Rows[0]["a21026"]);
            #region 当前时间的浓度
            if (mBegion.Month < mEnd.Month)
            {
                dt.Rows[years.Length]["Tstamp"] = mBegion.Year+"年" + mBegion.Month + "-" + mEnd.Month+"月";
            }
            else
            {
                dt.Rows[years.Length]["Tstamp"] = mBegion.ToString("yyyy年MM月");
            }
            
            dt.Rows[years.Length]["Compliance"] = all != 0 ?Math.Round((Excellent + Good) * 100 / all,1,MidpointRounding.AwayFromZero)  : 0;
            dt.Rows[years.Length]["Excellent"] = Excellent.ToString();
            dt.Rows[years.Length]["Good"] = Good.ToString();
            dt.Rows[years.Length]["Light"] = Light.ToString();
            dt.Rows[years.Length]["Moderate"] = Moderate.ToString();
            dt.Rows[years.Length]["Severe"] = Severe.ToString();
            //DecimalExtension.GetRoundValue(Convert.ToDecimal(Rowdt[0][factor]) * 1000, 0);
            dt.Rows[years.Length]["SO2"] = dv.Count > 0 ? DecimalExtension.GetRoundValue((Convert.ToDecimal(dv.ToTable().Rows[0]["a21026"].ToString() == "" ? 0 : dv.ToTable().Rows[0]["a21026"]) * 1000),0).ToString() : "0";
            dt.Rows[years.Length]["NO2"] = dv.Count > 0 ? DecimalExtension.GetRoundValue((Convert.ToDecimal(dv.ToTable().Rows[0]["a21004"].ToString() == "" ? 0 : dv.ToTable().Rows[0]["a21004"]) * 1000),0).ToString() : "0";
            dt.Rows[years.Length]["CO"] = dv.Count > 0 ? (dv.ToTable().Rows[0]["a21005"].ToString() == "" ? "0" : DecimalExtension.GetRoundValue(Convert.ToDecimal(dv.ToTable().Rows[0]["a21005"].ToString()),1).ToString()) : "0";
            dt.Rows[years.Length]["O3"] = dv.Count > 0 ? DecimalExtension.GetRoundValue((Convert.ToDecimal(dv.ToTable().Rows[0]["a05024"].ToString() == "" ? 0 : dv.ToTable().Rows[0]["a05024"]) * 1000),0).ToString() : "0";
            dt.Rows[years.Length]["PM10"] = dv.Count > 0 ? DecimalExtension.GetRoundValue((Convert.ToDecimal(dv.ToTable().Rows[0]["a34002"].ToString() == "" ? 0 : dv.ToTable().Rows[0]["a34002"]) * 1000),0).ToString() : "0";
            dt.Rows[years.Length]["PM2.5"] = dv.Count > 0 ? DecimalExtension.GetRoundValue((Convert.ToDecimal(dv.ToTable().Rows[0]["a34004"].ToString() == "" ? 0 : dv.ToTable().Rows[0]["a34004"]) * 1000),0).ToString() : "0";
            #endregion

            #region 比对时间
            for (int j=0;j<years.Length;j++)
            {
                string s = years[j] + "-" + mBegion.ToString("MM-dd HH:mm:ss");
                //DataView dvNew = GetPortDataPagerDayAQI(portIds, Convert.ToDateTime(years[j] + "-" + mBegion.ToString("MM-dd HH:mm:ss")), Convert.ToDateTime(years[j] + "-" + mEnd.ToString("MM-dd HH:mm:ss")), pageSize, pageNo, out recordTotal, orderBy);  // 本期
                #region 区域日浓度均值
                PM25PollutantValue = m_AQICalculateService.GetRegionValueByTime(portIds, "a34004", Convert.ToDateTime(years[j] + "-" + mBegion.ToString("MM-dd HH:mm:ss")), Convert.ToDateTime(years[j] + "-" + mEnd.ToString("MM-dd HH:mm:ss")), 24, "2");
                PM10PollutantValue = m_AQICalculateService.GetRegionValueByTime(portIds, "a34002", Convert.ToDateTime(years[j] + "-" + mBegion.ToString("MM-dd HH:mm:ss")), Convert.ToDateTime(years[j] + "-" + mEnd.ToString("MM-dd HH:mm:ss")), 24, "2");
                NO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(portIds, "a21004", Convert.ToDateTime(years[j] + "-" + mBegion.ToString("MM-dd HH:mm:ss")), Convert.ToDateTime(years[j] + "-" + mEnd.ToString("MM-dd HH:mm:ss")), 24, "2");
                SO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(portIds, "a21026", Convert.ToDateTime(years[j] + "-" + mBegion.ToString("MM-dd HH:mm:ss")), Convert.ToDateTime(years[j] + "-" + mEnd.ToString("MM-dd HH:mm:ss")), 24, "2");
                COPollutantValue = m_AQICalculateService.GetRegionValueByTime(portIds, "a21005", Convert.ToDateTime(years[j] + "-" + mBegion.ToString("MM-dd HH:mm:ss")), Convert.ToDateTime(years[j] + "-" + mEnd.ToString("MM-dd HH:mm:ss")), 24, "2");
                Max8HourO3PollutantValue = m_AQICalculateService.GetRegionValueByTime(portIds, "a05024", Convert.ToDateTime(years[j] + "-" + mBegion.ToString("MM-dd HH:mm:ss")), Convert.ToDateTime(years[j] + "-" + mEnd.ToString("MM-dd HH:mm:ss")), 8, "2");
                DataTable dtForCityNew = new DataTable();
                dtForCityNew.Columns.Add("a21026", typeof(string));
                dtForCityNew.Columns.Add("a21004", typeof(string));
                dtForCityNew.Columns.Add("a21005", typeof(string));
                dtForCityNew.Columns.Add("a05024", typeof(string));
                dtForCityNew.Columns.Add("a34002", typeof(string));
                dtForCityNew.Columns.Add("a34004", typeof(string));
                DataRow drcn = dtForCityNew.NewRow();
                //drc["MonitoringRegionUid"] = name;
                drcn["a34004"] = PM25PollutantValue.ToString();
                drcn["a34002"] = PM10PollutantValue.ToString();
                drcn["a21004"] = NO2PollutantValue.ToString();
                drcn["a21026"] = SO2PollutantValue.ToString();
                drcn["a21005"] = COPollutantValue.ToString();
                drcn["a05024"] = Max8HourO3PollutantValue.ToString();
                dtForCityNew.Rows.Add(drcn);
                #endregion
                DataView dvNew = dtForCityNew.DefaultView;
                //DataView dvsNew = pointDayAQI.GetDataPager(portIds, Convert.ToDateTime(years[j] + "-" + mBegion.ToString("MM-dd HH:mm:ss")), Convert.ToDateTime(years[j] + "-" + mEnd.ToString("MM-dd HH:mm:ss")), int.MaxValue, 0, out recordTotal, "DateTime,PointId");
                DataView dvsNew = m_AQICalculateService.GetRegionAQI(portIds, Convert.ToDateTime(years[j] + "-" + mBegion.ToString("MM-dd HH:mm:ss")), Convert.ToDateTime(years[j] + "-" + mEnd.ToString("MM-dd HH:mm:ss")), 24, "2").AsDataView();
                #region 天数统计
                all = dvsNew.Count;
                dvsNew.RowFilter = "AQIValue <> '' and AQIValue<=50";
                Excellent = dvsNew.Count;
                dvsNew.RowFilter = "AQIValue <> '' and AQIValue<=100 and AQIValue>50";
                Good = dvsNew.Count;
                dvsNew.RowFilter = "AQIValue <> '' and AQIValue<=150 and AQIValue>100";
                Light = dvsNew.Count;
                dvsNew.RowFilter = "AQIValue <> '' and AQIValue<=200 and AQIValue>150";
                Moderate = dvsNew.Count;
                dvsNew.RowFilter = "AQIValue <> '' and AQIValue<=300 and AQIValue>200";
                Severe = dvsNew.Count;
                //dv.RowFilter = "AQIValue <> '' and AQIValue>300";
                //decimal SeverelyPolluted = dv.Count;
                #endregion
                #region 浓度
                
                if (Convert.ToDateTime(years[j] + "-" + mBegion.ToString("MM-dd HH:mm:ss")).Month < Convert.ToDateTime(years[j] + "-" + mEnd.ToString("MM-dd HH:mm:ss")).Month)
                {
                    dt.Rows[j]["Tstamp"] = Convert.ToDateTime(years[j] + "-" + mBegion.ToString("MM-dd HH:mm:ss")).Year+"年" + Convert.ToDateTime(years[j] + "-" + mBegion.ToString("MM-dd HH:mm:ss")).Month + "-" + Convert.ToDateTime(years[j] + "-" + mEnd.ToString("MM-dd HH:mm:ss")).Month + "月";
                }
                else
                {
                    dt.Rows[j]["Tstamp"] = Convert.ToDateTime(years[j] + "-" + mBegion.ToString("MM-dd HH:mm:ss")).ToString("yyyy年MM月");
                }
                dt.Rows[j]["Compliance"] = all != 0 ? Math.Round((Excellent + Good) * 100 / all, 2, MidpointRounding.AwayFromZero) : 0;
                dt.Rows[j]["Excellent"] = Excellent.ToString();
                dt.Rows[j]["Good"] = Good.ToString();
                dt.Rows[j]["Light"] = Light.ToString();
                dt.Rows[j]["Moderate"] = Moderate.ToString();
                dt.Rows[j]["Severe"] = Severe.ToString();
                
                dt.Rows[j]["SO2"] = dvNew.Count > 0 ? DecimalExtension.GetRoundValue((Convert.ToDecimal(dvNew.ToTable().Rows[0]["a21026"].ToString() == "" ? 0 : dvNew.ToTable().Rows[0]["a21026"]) * 1000),0).ToString() : "0";
                dt.Rows[j]["NO2"] = dvNew.Count > 0 ? DecimalExtension.GetRoundValue((Convert.ToDecimal(dvNew.ToTable().Rows[0]["a21004"].ToString() == "" ? 0 : dvNew.ToTable().Rows[0]["a21004"]) * 1000), 0).ToString() : "0";
                dt.Rows[j]["CO"] = dvNew.Count > 0 ? (dvNew.ToTable().Rows[0]["a21005"].ToString() == "" ? "0" : DecimalExtension.GetRoundValue(Convert.ToDecimal(dvNew.ToTable().Rows[0]["a21005"].ToString()), 1).ToString()) : "0";
                dt.Rows[j]["O3"] = dvNew.Count > 0 ? DecimalExtension.GetRoundValue((Convert.ToDecimal(dvNew.ToTable().Rows[0]["a05024"].ToString() == "" ? 0 : dvNew.ToTable().Rows[0]["a05024"]) * 1000), 0).ToString() : "0";
                dt.Rows[j]["PM10"] = dvNew.Count > 0 ? DecimalExtension.GetRoundValue((Convert.ToDecimal(dvNew.ToTable().Rows[0]["a34002"].ToString() == "" ? 0 : dvNew.ToTable().Rows[0]["a34002"]) * 1000), 0).ToString() : "0";
                dt.Rows[j]["PM2.5"] = dvNew.Count > 0 ? DecimalExtension.GetRoundValue((Convert.ToDecimal(dvNew.ToTable().Rows[0]["a34004"].ToString() == "" ? 0 : dvNew.ToTable().Rows[0]["a34004"]) * 1000), 0).ToString() : "0";
                #endregion

                #region 比较
                if(Convert.ToDecimal(dt.Rows[years.Length]["Compliance"]) - Convert.ToDecimal(dt.Rows[j]["Compliance"])>0)
                {
                    compliance = "↑"+(Convert.ToDecimal(dt.Rows[years.Length]["Compliance"]) - Convert.ToDecimal(dt.Rows[j]["Compliance"])).ToString();
                }
                else if(Convert.ToDecimal(dt.Rows[years.Length]["Compliance"]) - Convert.ToDecimal(dt.Rows[j]["Compliance"])<0)
                {
                    compliance = "↓"+(Convert.ToDecimal(dt.Rows[j]["Compliance"]) - Convert.ToDecimal(dt.Rows[years.Length]["Compliance"])).ToString();
                }
                else
                {
                    compliance = "持平";
                }
                if (Convert.ToDecimal(dt.Rows[years.Length]["Excellent"]) - Convert.ToDecimal(dt.Rows[j]["Excellent"]) > 0)
                {
                    excellent = "↑" + (Convert.ToDecimal(dt.Rows[years.Length]["Excellent"]) - Convert.ToDecimal(dt.Rows[j]["Excellent"])).ToString();
                }
                else if (Convert.ToDecimal(dt.Rows[years.Length]["Excellent"]) - Convert.ToDecimal(dt.Rows[j]["Excellent"]) < 0)
                {
                    excellent = "↓" + (Convert.ToDecimal(dt.Rows[j]["Excellent"]) - Convert.ToDecimal(dt.Rows[years.Length]["Excellent"])).ToString();
                }
                else
                {
                    excellent = "持平";
                }
                if (Convert.ToDecimal(dt.Rows[years.Length]["Good"]) - Convert.ToDecimal(dt.Rows[j]["Good"]) > 0)
                {
                    good = "↑" + (Convert.ToDecimal(dt.Rows[years.Length]["Good"]) - Convert.ToDecimal(dt.Rows[j]["Good"])).ToString();
                }
                else if (Convert.ToDecimal(dt.Rows[years.Length]["Good"]) - Convert.ToDecimal(dt.Rows[j]["Good"]) < 0)
                {
                    good = "↓" + (Convert.ToDecimal(dt.Rows[j]["Good"]) - Convert.ToDecimal(dt.Rows[years.Length]["Good"])).ToString();
                }
                else
                {
                    good = "持平";
                }
                if (Convert.ToDecimal(dt.Rows[years.Length]["Light"]) - Convert.ToDecimal(dt.Rows[j]["Light"]) > 0)
                {
                    light = "↑" + (Convert.ToDecimal(dt.Rows[years.Length]["Light"]) - Convert.ToDecimal(dt.Rows[j]["Light"])).ToString();
                }
                else if (Convert.ToDecimal(dt.Rows[years.Length]["Light"]) - Convert.ToDecimal(dt.Rows[j]["Light"]) < 0)
                {
                    light = "↓" + (Convert.ToDecimal(dt.Rows[j]["Light"]) - Convert.ToDecimal(dt.Rows[years.Length]["Light"])).ToString();
                }
                else
                {
                    light = "持平";
                }
                if (Convert.ToDecimal(dt.Rows[years.Length]["Moderate"]) - Convert.ToDecimal(dt.Rows[j]["Moderate"]) > 0)
                {
                    moderate = "↑" + (Convert.ToDecimal(dt.Rows[years.Length]["Moderate"]) - Convert.ToDecimal(dt.Rows[j]["Moderate"])).ToString();
                }
                else if (Convert.ToDecimal(dt.Rows[years.Length]["Moderate"]) - Convert.ToDecimal(dt.Rows[j]["Moderate"]) < 0)
                {
                    moderate = "↓" + (Convert.ToDecimal(dt.Rows[j]["Moderate"]) - Convert.ToDecimal(dt.Rows[years.Length]["Moderate"])).ToString();
                }
                else
                {
                    moderate = "持平";
                }
                if (Convert.ToDecimal(dt.Rows[years.Length]["Severe"]) - Convert.ToDecimal(dt.Rows[j]["Severe"]) > 0)
                {
                    severe = "↑" + (Convert.ToDecimal(dt.Rows[years.Length]["Severe"]) - Convert.ToDecimal(dt.Rows[j]["Severe"])).ToString();
                }
                else if (Convert.ToDecimal(dt.Rows[years.Length]["Severe"]) - Convert.ToDecimal(dt.Rows[j]["Severe"]) < 0)
                {
                    severe = "↓" + (Convert.ToDecimal(dt.Rows[j]["Severe"]) - Convert.ToDecimal(dt.Rows[years.Length]["Severe"])).ToString();
                }
                else
                {
                    severe = "持平";
                }
                if (Convert.ToDecimal(dt.Rows[years.Length]["SO2"]) - Convert.ToDecimal(dt.Rows[j]["SO2"]) > 0)
                {
                    a21026 = "↑" + Math.Round(((Convert.ToDecimal(dt.Rows[years.Length]["SO2"]) - Convert.ToDecimal(dt.Rows[j]["SO2"]))*100 / Convert.ToDecimal(dt.Rows[years.Length]["SO2"])), 1, MidpointRounding.AwayFromZero).ToString() + "%";
                }
                else if (Convert.ToDecimal(dt.Rows[years.Length]["SO2"]) - Convert.ToDecimal(dt.Rows[j]["SO2"]) < 0)
                {
                    a21026 = "↓" + Math.Round(((Convert.ToDecimal(dt.Rows[j]["SO2"]) - Convert.ToDecimal(dt.Rows[years.Length]["SO2"]))*100 / Convert.ToDecimal(dt.Rows[j]["SO2"])), 1, MidpointRounding.AwayFromZero).ToString() + "%";
                }
                else
                {
                    a21026 = "持平";
                }
                if (Convert.ToDecimal(dt.Rows[years.Length]["NO2"]) - Convert.ToDecimal(dt.Rows[j]["NO2"]) > 0)
                {
                    a21004 = "↑" + Math.Round(((Convert.ToDecimal(dt.Rows[years.Length]["NO2"]) - Convert.ToDecimal(dt.Rows[j]["NO2"]))*100 / Convert.ToDecimal(dt.Rows[years.Length]["NO2"])), 1, MidpointRounding.AwayFromZero).ToString() + "%";
                }
                else if (Convert.ToDecimal(dt.Rows[years.Length]["NO2"]) - Convert.ToDecimal(dt.Rows[j]["NO2"]) < 0)
                {
                    a21004 = "↓" + Math.Round(((Convert.ToDecimal(dt.Rows[j]["NO2"]) - Convert.ToDecimal(dt.Rows[years.Length]["NO2"]))*100 / Convert.ToDecimal(dt.Rows[j]["NO2"])), 1, MidpointRounding.AwayFromZero).ToString() + "%";
                }
                else
                {
                    a21004 = "持平";
                }
                if (Convert.ToDecimal(dt.Rows[years.Length]["CO"]) - Convert.ToDecimal(dt.Rows[j]["CO"]) > 0)
                {
                    a21005 = "↑" + Math.Round(((Convert.ToDecimal(dt.Rows[years.Length]["CO"]) - Convert.ToDecimal(dt.Rows[j]["CO"]))*100 / Convert.ToDecimal(dt.Rows[years.Length]["CO"])), 1, MidpointRounding.AwayFromZero).ToString() + "%";
                }
                else if (Convert.ToDecimal(dt.Rows[years.Length]["CO"]) - Convert.ToDecimal(dt.Rows[j]["CO"]) < 0)
                {
                    a21005 = "↓" + Math.Round(((Convert.ToDecimal(dt.Rows[j]["CO"]) - Convert.ToDecimal(dt.Rows[years.Length]["CO"]))*100 / Convert.ToDecimal(dt.Rows[j]["CO"])), 1, MidpointRounding.AwayFromZero).ToString() + "%";
                }
                else
                {
                    a21005 = "持平";
                }
                if (Convert.ToDecimal(dt.Rows[years.Length]["O3"]) - Convert.ToDecimal(dt.Rows[j]["O3"]) > 0)
                {
                    a05024 = "↑" + Math.Round(((Convert.ToDecimal(dt.Rows[years.Length]["O3"]) - Convert.ToDecimal(dt.Rows[j]["O3"]))*100 / Convert.ToDecimal(dt.Rows[years.Length]["O3"])), 1, MidpointRounding.AwayFromZero).ToString() + "%";
                }
                else if (Convert.ToDecimal(dt.Rows[years.Length]["O3"]) - Convert.ToDecimal(dt.Rows[j]["O3"]) < 0)
                {
                    a05024 = "↓" + Math.Round(((Convert.ToDecimal(dt.Rows[j]["O3"]) - Convert.ToDecimal(dt.Rows[years.Length]["O3"]))*100 / Convert.ToDecimal(dt.Rows[j]["O3"])), 1, MidpointRounding.AwayFromZero).ToString() + "%";
                }
                else
                {
                    a05024 = "持平";
                }
                if (Convert.ToDecimal(dt.Rows[years.Length]["PM10"]) - Convert.ToDecimal(dt.Rows[j]["PM10"]) > 0)
                {
                    a34002 = "↑" + Math.Round(((Convert.ToDecimal(dt.Rows[years.Length]["PM10"]) - Convert.ToDecimal(dt.Rows[j]["PM10"]))*100 / Convert.ToDecimal(dt.Rows[years.Length]["PM10"])), 1, MidpointRounding.AwayFromZero).ToString() + "%";
                }
                else if (Convert.ToDecimal(dt.Rows[years.Length]["PM10"]) - Convert.ToDecimal(dt.Rows[j]["PM10"]) < 0)
                {
                    a34002 = "↓" + Math.Round(((Convert.ToDecimal(dt.Rows[j]["PM10"]) - Convert.ToDecimal(dt.Rows[years.Length]["PM10"]))*100 / Convert.ToDecimal(dt.Rows[j]["PM10"])), 1, MidpointRounding.AwayFromZero).ToString() + "%";
                }
                else
                {
                    a34002 = "持平";
                }
                if (Convert.ToDecimal(dt.Rows[years.Length]["PM2.5"]) - Convert.ToDecimal(dt.Rows[j]["PM2.5"]) > 0)
                {
                    a34004 = "↑" + Math.Round(((Convert.ToDecimal(dt.Rows[years.Length]["PM2.5"]) - Convert.ToDecimal(dt.Rows[j]["PM2.5"]))*100 / Convert.ToDecimal(dt.Rows[years.Length]["PM2.5"])), 1, MidpointRounding.AwayFromZero).ToString() + "%";
                }
                else if (Convert.ToDecimal(dt.Rows[years.Length]["PM2.5"]) - Convert.ToDecimal(dt.Rows[j]["PM2.5"]) < 0)
                {
                    a34004 = "↓" + Math.Round(((Convert.ToDecimal(dt.Rows[j]["PM2.5"]) - Convert.ToDecimal(dt.Rows[years.Length]["PM2.5"]))*100 / Convert.ToDecimal(dt.Rows[j]["PM2.5"])), 1, MidpointRounding.AwayFromZero).ToString() + "%";
                }
                else
                {
                    a34004 = "持平";
                }
                dt.Rows[j + years.Length + 1]["Tstamp"] = "与"+years[j]+"年比较";
                dt.Rows[j + years.Length + 1]["Compliance"] = compliance;
                dt.Rows[j + years.Length + 1]["Excellent"] = excellent;
                dt.Rows[j + years.Length + 1]["Good"] = good;
                dt.Rows[j + years.Length + 1]["Light"] = light;
                dt.Rows[j + years.Length + 1]["Moderate"] = moderate;
                dt.Rows[j + years.Length + 1]["Severe"] = severe;
                dt.Rows[j + years.Length + 1]["SO2"] = a21026;
                dt.Rows[j + years.Length + 1]["NO2"] = a21004;
                dt.Rows[j + years.Length + 1]["CO"] = a21005;
                dt.Rows[j + years.Length + 1]["O3"] = a05024;
                dt.Rows[j + years.Length + 1]["PM10"] = a34002;
                dt.Rows[j + years.Length + 1]["PM2.5"] = a34004;
                #endregion
            }
            #endregion
            return dt.DefaultView;
        }
        /// <summary>
        /// 空气质量站点比对报表
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factorCodes"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <param name="year"></param>
        /// <param name="years"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <param name="recordTotal"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataView GetComparisonPager(string[] portIds, DateTime mBegion, DateTime mEnd, string[] years, int pageSize
        , int pageNo, out int recordTotal, string orderBy = "PointId")
        {
            pointDayAQI = Singleton<DayAQIRepository>.GetInstance();
            recordTotal = 0;
            DataTable dt = new DataTable();
            decimal all, Excellent, Good, Light, Moderate, Severe;
            string tstamp, compliance, excellent, good, light, moderate, severe, a21026, a21004, a21005, a05024, a34002, a34004;
            #region dt加列名
            dt.Columns.Add("Tstamp", typeof(string));
            dt.Columns.Add("Compliance", typeof(string));
            dt.Columns.Add("Excellent", typeof(string));
            dt.Columns.Add("Good", typeof(string));
            dt.Columns.Add("Light", typeof(string));
            dt.Columns.Add("Moderate", typeof(string));
            dt.Columns.Add("Severe", typeof(string));
            dt.Columns.Add("SO2", typeof(string));
            dt.Columns.Add("NO2", typeof(string));
            dt.Columns.Add("CO", typeof(string));
            dt.Columns.Add("O3", typeof(string));
            dt.Columns.Add("PM10", typeof(string));
            dt.Columns.Add("PM2.5", typeof(string));
            #endregion
            DataView dv = GetPortDataPagerDayAQI(portIds, mBegion, mEnd, pageSize, pageNo, out recordTotal, orderBy);  // 本期
            DataView dvs = pointDayAQI.GetDataPager(portIds, mBegion, mEnd, int.MaxValue, 0, out recordTotal, "DateTime,PointId");
            #region 加行
            for (int i = 0; i < years.Length * 2 + 1; i++)
            {
                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);
            }
            #endregion
            #region 天数统计
            all = dvs.Count;
            dvs.RowFilter = "AQIValue <> '' and AQIValue<=50";
            Excellent = dvs.Count;
            dvs.RowFilter = "AQIValue <> '' and AQIValue<=100 and AQIValue>50";
            Good = dvs.Count;
            dvs.RowFilter = "AQIValue <> '' and AQIValue<=150 and AQIValue>100";
            Light = dvs.Count;
            dvs.RowFilter = "AQIValue <> '' and AQIValue<=200 and AQIValue>150";
            Moderate = dvs.Count;
            dvs.RowFilter = "AQIValue <> '' and AQIValue<=300 and AQIValue>200";
            Severe = dvs.Count;
            //dv.RowFilter = "AQIValue <> '' and AQIValue>300";
            //decimal SeverelyPolluted = dv.Count;
            #endregion
            #region 当前时间的浓度
            if (mBegion.Month < mEnd.Month)
            {
                dt.Rows[years.Length]["Tstamp"] = mBegion.ToString("yyyy年") + mBegion.Month + "-" + mEnd.Month + "月";
            }
            else
            {
                dt.Rows[years.Length]["Tstamp"] = mBegion.ToString("yyyy年MM月");
            }
            //dt.Rows[years.Length]["Tstamp"] = mBegion.ToString("yyyy年MM月");
            dt.Rows[years.Length]["Compliance"] = all != 0 ? Math.Round((Excellent + Good) * 100 / all, 1, MidpointRounding.AwayFromZero) : 0;
            dt.Rows[years.Length]["Excellent"] = Excellent.ToString();
            dt.Rows[years.Length]["Good"] = Good.ToString();
            dt.Rows[years.Length]["Light"] = Light.ToString();
            dt.Rows[years.Length]["Moderate"] = Moderate.ToString();
            dt.Rows[years.Length]["Severe"] = Severe.ToString();
            
            dt.Rows[years.Length]["SO2"] = dv.Count > 0 ? DecimalExtension.GetRoundValue((Convert.ToDecimal(dv.ToTable().Rows[0]["a21026"].ToString() == "" ? 0 : dv.ToTable().Rows[0]["a21026"]) * 1000), 0).ToString() : "0";
            dt.Rows[years.Length]["NO2"] = dv.Count > 0 ? DecimalExtension.GetRoundValue((Convert.ToDecimal(dv.ToTable().Rows[0]["a21004"].ToString() == "" ? 0 : dv.ToTable().Rows[0]["a21004"]) * 1000), 0).ToString() : "0";
            dt.Rows[years.Length]["CO"] = dv.Count > 0 ? (dv.ToTable().Rows[0]["a21005"].ToString() == "" ? "0" : DecimalExtension.GetRoundValue(Convert.ToDecimal(dv.ToTable().Rows[0]["a21005"].ToString()), 1).ToString()) : "0";
            dt.Rows[years.Length]["O3"] = dv.Count > 0 ? DecimalExtension.GetRoundValue((Convert.ToDecimal(dv.ToTable().Rows[0]["a05024"].ToString() == "" ? 0 : dv.ToTable().Rows[0]["a05024"]) * 1000), 0).ToString() : "0";
            dt.Rows[years.Length]["PM10"] = dv.Count > 0 ? DecimalExtension.GetRoundValue((Convert.ToDecimal(dv.ToTable().Rows[0]["a34002"].ToString() == "" ? 0 : dv.ToTable().Rows[0]["a34002"]) * 1000), 0).ToString() : "0";
            dt.Rows[years.Length]["PM2.5"] = dv.Count > 0 ? DecimalExtension.GetRoundValue((Convert.ToDecimal(dv.ToTable().Rows[0]["a34004"].ToString() == "" ? 0 : dv.ToTable().Rows[0]["a34004"]) * 1000), 0).ToString() : "0";
            #endregion

            #region 比对时间
            for (int j = 0; j < years.Length; j++)
            {
                string s = years[j] + "-" + mBegion.ToString("MM-dd HH:mm:ss");
                DataView dvNew = GetPortDataPagerDayAQI(portIds, Convert.ToDateTime(years[j] + "-" + mBegion.ToString("MM-dd HH:mm:ss")), Convert.ToDateTime(years[j] + "-" + mEnd.ToString("MM-dd HH:mm:ss")), pageSize, pageNo, out recordTotal, orderBy);  // 本期
                DataView dvsNew = pointDayAQI.GetDataPager(portIds, Convert.ToDateTime(years[j] + "-" + mBegion.ToString("MM-dd HH:mm:ss")), Convert.ToDateTime(years[j] + "-" + mEnd.ToString("MM-dd HH:mm:ss")), int.MaxValue, 0, out recordTotal, "DateTime,PointId");
                #region 天数统计
                all = dvsNew.Count;
                dvsNew.RowFilter = "AQIValue <> '' and AQIValue<=50";
                Excellent = dvsNew.Count;
                dvsNew.RowFilter = "AQIValue <> '' and AQIValue<=100 and AQIValue>50";
                Good = dvsNew.Count;
                dvsNew.RowFilter = "AQIValue <> '' and AQIValue<=150 and AQIValue>100";
                Light = dvsNew.Count;
                dvsNew.RowFilter = "AQIValue <> '' and AQIValue<=200 and AQIValue>150";
                Moderate = dvsNew.Count;
                dvsNew.RowFilter = "AQIValue <> '' and AQIValue<=300 and AQIValue>200";
                Severe = dvsNew.Count;
                //dv.RowFilter = "AQIValue <> '' and AQIValue>300";
                //decimal SeverelyPolluted = dv.Count;
                #endregion
                #region 浓度
                
                if (Convert.ToDateTime(years[j] + "-" + mBegion.ToString("MM-dd HH:mm:ss")).Month < Convert.ToDateTime(years[j] + "-" + mEnd.ToString("MM-dd HH:mm:ss")).Month)
                {
                    dt.Rows[j]["Tstamp"] = Convert.ToDateTime(years[j] + "-" + mBegion.ToString("MM-dd HH:mm:ss")).ToString("yyyy年") + Convert.ToDateTime(years[j] + "-" + mBegion.ToString("MM-dd HH:mm:ss")).Month + "-" + Convert.ToDateTime(years[j] + "-" + mEnd.ToString("MM-dd HH:mm:ss")).Month + "月";
                }
                else
                {
                    dt.Rows[j]["Tstamp"] = Convert.ToDateTime(years[j] + "-" + mBegion.ToString("MM-dd HH:mm:ss")).ToString("yyyy年MM月");
                }
                dt.Rows[j]["Compliance"] = all != 0 ? Math.Round((Excellent + Good) * 100 / all, 2, MidpointRounding.AwayFromZero) : 0;
                dt.Rows[j]["Excellent"] = Excellent.ToString();
                dt.Rows[j]["Good"] = Good.ToString();
                dt.Rows[j]["Light"] = Light.ToString();
                dt.Rows[j]["Moderate"] = Moderate.ToString();
                dt.Rows[j]["Severe"] = Severe.ToString();
                dt.Rows[j]["SO2"] = dvNew.Count > 0 ? DecimalExtension.GetRoundValue((Convert.ToDecimal(dvNew.ToTable().Rows[0]["a21026"].ToString() == "" ? 0 : dvNew.ToTable().Rows[0]["a21026"]) * 1000), 0).ToString() : "0";
                dt.Rows[j]["NO2"] = dvNew.Count > 0 ? DecimalExtension.GetRoundValue((Convert.ToDecimal(dvNew.ToTable().Rows[0]["a21004"].ToString() == "" ? 0 : dvNew.ToTable().Rows[0]["a21004"]) * 1000), 0).ToString() : "0";
                dt.Rows[j]["CO"] = dvNew.Count > 0 ? (dvNew.ToTable().Rows[0]["a21005"].ToString() == "" ? "0" : DecimalExtension.GetRoundValue(Convert.ToDecimal(dvNew.ToTable().Rows[0]["a21005"].ToString()), 1).ToString()) : "0";
                dt.Rows[j]["O3"] = dvNew.Count > 0 ? DecimalExtension.GetRoundValue((Convert.ToDecimal(dvNew.ToTable().Rows[0]["a05024"].ToString() == "" ? 0 : dvNew.ToTable().Rows[0]["a05024"]) * 1000), 0).ToString() : "0";
                dt.Rows[j]["PM10"] = dvNew.Count > 0 ? DecimalExtension.GetRoundValue((Convert.ToDecimal(dvNew.ToTable().Rows[0]["a34002"].ToString() == "" ? 0 : dvNew.ToTable().Rows[0]["a34002"]) * 1000), 0).ToString() : "0";
                dt.Rows[j]["PM2.5"] = dvNew.Count > 0 ? DecimalExtension.GetRoundValue((Convert.ToDecimal(dvNew.ToTable().Rows[0]["a34004"].ToString() == "" ? 0 : dvNew.ToTable().Rows[0]["a34004"]) * 1000), 0).ToString() : "0";
                #endregion

                #region 比较
                if (Convert.ToDecimal(dt.Rows[years.Length]["Compliance"]) - Convert.ToDecimal(dt.Rows[j]["Compliance"]) > 0)
                {
                    compliance = "↑" + (Convert.ToDecimal(dt.Rows[years.Length]["Compliance"]) - Convert.ToDecimal(dt.Rows[j]["Compliance"])).ToString();
                }
                else if (Convert.ToDecimal(dt.Rows[years.Length]["Compliance"]) - Convert.ToDecimal(dt.Rows[j]["Compliance"]) < 0)
                {
                    compliance = "↓" + (Convert.ToDecimal(dt.Rows[j]["Compliance"]) - Convert.ToDecimal(dt.Rows[years.Length]["Compliance"])).ToString();
                }
                else
                {
                    compliance = "持平";
                }
                if (Convert.ToDecimal(dt.Rows[years.Length]["Excellent"]) - Convert.ToDecimal(dt.Rows[j]["Excellent"]) > 0)
                {
                    excellent = "↑" + (Convert.ToDecimal(dt.Rows[years.Length]["Excellent"]) - Convert.ToDecimal(dt.Rows[j]["Excellent"])).ToString();
                }
                else if (Convert.ToDecimal(dt.Rows[years.Length]["Excellent"]) - Convert.ToDecimal(dt.Rows[j]["Excellent"]) < 0)
                {
                    excellent = "↓" + (Convert.ToDecimal(dt.Rows[j]["Excellent"]) - Convert.ToDecimal(dt.Rows[years.Length]["Excellent"])).ToString();
                }
                else
                {
                    excellent = "持平";
                }
                if (Convert.ToDecimal(dt.Rows[years.Length]["Good"]) - Convert.ToDecimal(dt.Rows[j]["Good"]) > 0)
                {
                    good = "↑" + (Convert.ToDecimal(dt.Rows[years.Length]["Good"]) - Convert.ToDecimal(dt.Rows[j]["Good"])).ToString();
                }
                else if (Convert.ToDecimal(dt.Rows[years.Length]["Good"]) - Convert.ToDecimal(dt.Rows[j]["Good"]) < 0)
                {
                    good = "↓" + (Convert.ToDecimal(dt.Rows[j]["Good"]) - Convert.ToDecimal(dt.Rows[years.Length]["Good"])).ToString();
                }
                else
                {
                    good = "持平";
                }
                if (Convert.ToDecimal(dt.Rows[years.Length]["Light"]) - Convert.ToDecimal(dt.Rows[j]["Light"]) > 0)
                {
                    light = "↑" + (Convert.ToDecimal(dt.Rows[years.Length]["Light"]) - Convert.ToDecimal(dt.Rows[j]["Light"])).ToString();
                }
                else if (Convert.ToDecimal(dt.Rows[years.Length]["Light"]) - Convert.ToDecimal(dt.Rows[j]["Light"]) < 0)
                {
                    light = "↓" + (Convert.ToDecimal(dt.Rows[j]["Light"]) - Convert.ToDecimal(dt.Rows[years.Length]["Light"])).ToString();
                }
                else
                {
                    light = "持平";
                }
                if (Convert.ToDecimal(dt.Rows[years.Length]["Moderate"]) - Convert.ToDecimal(dt.Rows[j]["Moderate"]) > 0)
                {
                    moderate = "↑" + (Convert.ToDecimal(dt.Rows[years.Length]["Moderate"]) - Convert.ToDecimal(dt.Rows[j]["Moderate"])).ToString();
                }
                else if (Convert.ToDecimal(dt.Rows[years.Length]["Moderate"]) - Convert.ToDecimal(dt.Rows[j]["Moderate"]) < 0)
                {
                    moderate = "↓" + (Convert.ToDecimal(dt.Rows[j]["Moderate"]) - Convert.ToDecimal(dt.Rows[years.Length]["Moderate"])).ToString();
                }
                else
                {
                    moderate = "持平";
                }
                if (Convert.ToDecimal(dt.Rows[years.Length]["Severe"]) - Convert.ToDecimal(dt.Rows[j]["Severe"]) > 0)
                {
                    severe = "↑" + (Convert.ToDecimal(dt.Rows[years.Length]["Severe"]) - Convert.ToDecimal(dt.Rows[j]["Severe"])).ToString();
                }
                else if (Convert.ToDecimal(dt.Rows[years.Length]["Severe"]) - Convert.ToDecimal(dt.Rows[j]["Severe"]) < 0)
                {
                    severe = "↓" + (Convert.ToDecimal(dt.Rows[j]["Severe"]) - Convert.ToDecimal(dt.Rows[years.Length]["Severe"])).ToString();
                }
                else
                {
                    severe = "持平";
                }
                //if (Convert.ToDecimal(dt.Rows[years.Length]["SO2"]) - Convert.ToDecimal(dt.Rows[j]["SO2"]) > 0)
                //{
                //    a21026 = "↑" + (Convert.ToDecimal(dt.Rows[years.Length]["SO2"]) - Convert.ToDecimal(dt.Rows[j]["SO2"])).ToString();
                //}
                //else if (Convert.ToDecimal(dt.Rows[years.Length]["SO2"]) - Convert.ToDecimal(dt.Rows[j]["SO2"]) < 0)
                //{
                //    a21026 = "↓" + (Convert.ToDecimal(dt.Rows[j]["SO2"]) - Convert.ToDecimal(dt.Rows[years.Length]["SO2"])).ToString();
                //}
                //else
                //{
                //    a21026 = "持平";
                //}

                //if (Convert.ToDecimal(dt.Rows[years.Length]["NO2"]) - Convert.ToDecimal(dt.Rows[j]["NO2"]) > 0)
                //{
                //    a21004 = "↑" + (Convert.ToDecimal(dt.Rows[years.Length]["NO2"]) - Convert.ToDecimal(dt.Rows[j]["NO2"])).ToString();
                //}
                //else if (Convert.ToDecimal(dt.Rows[years.Length]["NO2"]) - Convert.ToDecimal(dt.Rows[j]["NO2"]) < 0)
                //{
                //    a21004 = "↓" + (Convert.ToDecimal(dt.Rows[j]["NO2"]) - Convert.ToDecimal(dt.Rows[years.Length]["NO2"])).ToString();
                //}
                //else
                //{
                //    a21004 = "持平";
                //}
                //if (Convert.ToDecimal(dt.Rows[years.Length]["CO"]) - Convert.ToDecimal(dt.Rows[j]["CO"]) > 0)
                //{
                //    a21005 = "↑" + (Convert.ToDecimal(dt.Rows[years.Length]["CO"]) - Convert.ToDecimal(dt.Rows[j]["CO"])).ToString();
                //}
                //else if (Convert.ToDecimal(dt.Rows[years.Length]["CO"]) - Convert.ToDecimal(dt.Rows[j]["CO"]) < 0)
                //{
                //    a21005 = "↓" + (Convert.ToDecimal(dt.Rows[j]["CO"]) - Convert.ToDecimal(dt.Rows[years.Length]["CO"])).ToString();
                //}
                //else
                //{
                //    a21005 = "持平";
                //}
                //if (Convert.ToDecimal(dt.Rows[years.Length]["O3"]) - Convert.ToDecimal(dt.Rows[j]["O3"]) > 0)
                //{
                //    a05024 = "↑" + (Convert.ToDecimal(dt.Rows[years.Length]["O3"]) - Convert.ToDecimal(dt.Rows[j]["O3"])).ToString();
                //}
                //else if (Convert.ToDecimal(dt.Rows[years.Length]["O3"]) - Convert.ToDecimal(dt.Rows[j]["O3"]) < 0)
                //{
                //    a05024 = "↓" + (Convert.ToDecimal(dt.Rows[j]["O3"]) - Convert.ToDecimal(dt.Rows[years.Length]["O3"])).ToString();
                //}
                //else
                //{
                //    a05024 = "持平";
                //}
                //if (Convert.ToDecimal(dt.Rows[years.Length]["PM10"]) - Convert.ToDecimal(dt.Rows[j]["PM10"]) > 0)
                //{
                //    a34002 = "↑" + (Convert.ToDecimal(dt.Rows[years.Length]["PM10"]) - Convert.ToDecimal(dt.Rows[j]["PM10"])).ToString();
                //}
                //else if (Convert.ToDecimal(dt.Rows[years.Length]["PM10"]) - Convert.ToDecimal(dt.Rows[j]["PM10"]) < 0)
                //{
                //    a34002 = "↓" + (Convert.ToDecimal(dt.Rows[j]["PM10"]) - Convert.ToDecimal(dt.Rows[years.Length]["PM10"])).ToString();
                //}
                //else
                //{
                //    a34002 = "持平";
                //}
                //if (Convert.ToDecimal(dt.Rows[years.Length]["PM2.5"]) - Convert.ToDecimal(dt.Rows[j]["PM2.5"]) > 0)
                //{
                //    a34004 = "↑" + (Convert.ToDecimal(dt.Rows[years.Length]["PM2.5"]) - Convert.ToDecimal(dt.Rows[j]["PM2.5"])).ToString();
                //}
                //else if (Convert.ToDecimal(dt.Rows[years.Length]["PM2.5"]) - Convert.ToDecimal(dt.Rows[j]["PM2.5"]) < 0)
                //{
                //    a34004 = "↓" + (Convert.ToDecimal(dt.Rows[j]["PM2.5"]) - Convert.ToDecimal(dt.Rows[years.Length]["PM2.5"])).ToString();
                //}
                //else
                //{
                //    a34004 = "持平";
                //}
                if (Convert.ToDecimal(dt.Rows[years.Length]["SO2"]) - Convert.ToDecimal(dt.Rows[j]["SO2"]) > 0)
                {
                    a21026 = "↑" + Math.Round(((Convert.ToDecimal(dt.Rows[years.Length]["SO2"]) - Convert.ToDecimal(dt.Rows[j]["SO2"]))*100 / Convert.ToDecimal(dt.Rows[years.Length]["SO2"])), 1, MidpointRounding.AwayFromZero).ToString() + "%";
                }
                else if (Convert.ToDecimal(dt.Rows[years.Length]["SO2"]) - Convert.ToDecimal(dt.Rows[j]["SO2"]) < 0)
                {
                    a21026 = "↓" + Math.Round(((Convert.ToDecimal(dt.Rows[j]["SO2"]) - Convert.ToDecimal(dt.Rows[years.Length]["SO2"]))*100 / Convert.ToDecimal(dt.Rows[j]["SO2"])), 1, MidpointRounding.AwayFromZero).ToString() + "%";
                }
                else
                {
                    a21026 = "持平";
                }
                if (Convert.ToDecimal(dt.Rows[years.Length]["NO2"]) - Convert.ToDecimal(dt.Rows[j]["NO2"]) > 0)
                {
                    a21004 = "↑" + Math.Round(((Convert.ToDecimal(dt.Rows[years.Length]["NO2"]) - Convert.ToDecimal(dt.Rows[j]["NO2"]))*100 / Convert.ToDecimal(dt.Rows[years.Length]["NO2"])), 1, MidpointRounding.AwayFromZero).ToString() + "%";
                }
                else if (Convert.ToDecimal(dt.Rows[years.Length]["NO2"]) - Convert.ToDecimal(dt.Rows[j]["NO2"]) < 0)
                {
                    a21004 = "↓" + Math.Round(((Convert.ToDecimal(dt.Rows[j]["NO2"]) - Convert.ToDecimal(dt.Rows[years.Length]["NO2"]))*100 / Convert.ToDecimal(dt.Rows[j]["NO2"])), 1, MidpointRounding.AwayFromZero).ToString() + "%";
                }
                else
                {
                    a21004 = "持平";
                }
                if (Convert.ToDecimal(dt.Rows[years.Length]["CO"]) - Convert.ToDecimal(dt.Rows[j]["CO"]) > 0)
                {
                    a21005 = "↑" + Math.Round(((Convert.ToDecimal(dt.Rows[years.Length]["CO"]) - Convert.ToDecimal(dt.Rows[j]["CO"]))*100 / Convert.ToDecimal(dt.Rows[years.Length]["CO"])), 1, MidpointRounding.AwayFromZero).ToString() + "%";
                }
                else if (Convert.ToDecimal(dt.Rows[years.Length]["CO"]) - Convert.ToDecimal(dt.Rows[j]["CO"]) < 0)
                {
                    a21005 = "↓" + Math.Round(((Convert.ToDecimal(dt.Rows[j]["CO"]) - Convert.ToDecimal(dt.Rows[years.Length]["CO"]))*100 / Convert.ToDecimal(dt.Rows[j]["CO"])), 1, MidpointRounding.AwayFromZero).ToString() + "%";
                }
                else
                {
                    a21005 = "持平";
                }
                if (Convert.ToDecimal(dt.Rows[years.Length]["O3"]) - Convert.ToDecimal(dt.Rows[j]["O3"]) > 0)
                {
                    a05024 = "↑" + Math.Round(((Convert.ToDecimal(dt.Rows[years.Length]["O3"]) - Convert.ToDecimal(dt.Rows[j]["O3"]))*100 / Convert.ToDecimal(dt.Rows[years.Length]["O3"])), 1, MidpointRounding.AwayFromZero).ToString() + "%";
                }
                else if (Convert.ToDecimal(dt.Rows[years.Length]["O3"]) - Convert.ToDecimal(dt.Rows[j]["O3"]) < 0)
                {
                    a05024 = "↓" + Math.Round(((Convert.ToDecimal(dt.Rows[j]["O3"]) - Convert.ToDecimal(dt.Rows[years.Length]["O3"]))*100 / Convert.ToDecimal(dt.Rows[j]["O3"])), 1, MidpointRounding.AwayFromZero).ToString() + "%";
                }
                else
                {
                    a05024 = "持平";
                }
                if (Convert.ToDecimal(dt.Rows[years.Length]["PM10"]) - Convert.ToDecimal(dt.Rows[j]["PM10"]) > 0)
                {
                    a34002 = "↑" + Math.Round(((Convert.ToDecimal(dt.Rows[years.Length]["PM10"]) - Convert.ToDecimal(dt.Rows[j]["PM10"]))*100 / Convert.ToDecimal(dt.Rows[years.Length]["PM10"])), 1, MidpointRounding.AwayFromZero).ToString() + "%";
                }
                else if (Convert.ToDecimal(dt.Rows[years.Length]["PM10"]) - Convert.ToDecimal(dt.Rows[j]["PM10"]) < 0)
                {
                    a34002 = "↓" + Math.Round(((Convert.ToDecimal(dt.Rows[j]["PM10"]) - Convert.ToDecimal(dt.Rows[years.Length]["PM10"]))*100 / Convert.ToDecimal(dt.Rows[j]["PM10"])), 1, MidpointRounding.AwayFromZero).ToString() + "%";
                }
                else
                {
                    a34002 = "持平";
                }
                if (Convert.ToDecimal(dt.Rows[years.Length]["PM2.5"]) - Convert.ToDecimal(dt.Rows[j]["PM2.5"]) > 0)
                {
                    a34004 = "↑" + Math.Round(((Convert.ToDecimal(dt.Rows[years.Length]["PM2.5"]) - Convert.ToDecimal(dt.Rows[j]["PM2.5"]))*100 / Convert.ToDecimal(dt.Rows[years.Length]["PM2.5"])), 1, MidpointRounding.AwayFromZero).ToString() + "%";
                }
                else if (Convert.ToDecimal(dt.Rows[years.Length]["PM2.5"]) - Convert.ToDecimal(dt.Rows[j]["PM2.5"]) < 0)
                {
                    a34004 = "↓" + Math.Round(((Convert.ToDecimal(dt.Rows[j]["PM2.5"]) - Convert.ToDecimal(dt.Rows[years.Length]["PM2.5"]))*100 / Convert.ToDecimal(dt.Rows[j]["PM2.5"])), 1, MidpointRounding.AwayFromZero).ToString() + "%";
                }
                else
                {
                    a34004 = "持平";
                }
                dt.Rows[j + years.Length + 1]["Tstamp"] = "与" + years[j] + "年比较";
                dt.Rows[j + years.Length + 1]["Compliance"] = compliance;
                dt.Rows[j + years.Length + 1]["Excellent"] = excellent;
                dt.Rows[j + years.Length + 1]["Good"] = good;
                dt.Rows[j + years.Length + 1]["Light"] = light;
                dt.Rows[j + years.Length + 1]["Moderate"] = moderate;
                dt.Rows[j + years.Length + 1]["Severe"] = severe;
                dt.Rows[j + years.Length + 1]["SO2"] = a21026;
                dt.Rows[j + years.Length + 1]["NO2"] = a21004;
                dt.Rows[j + years.Length + 1]["CO"] = a21005;
                dt.Rows[j + years.Length + 1]["O3"] = a05024;
                dt.Rows[j + years.Length + 1]["PM10"] = a34002;
                dt.Rows[j + years.Length + 1]["PM2.5"] = a34004;
                #endregion
            }
            #endregion
            return dt.DefaultView;
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页（从0开始）</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetConcentrationDataPager(string[] portIds, string[] factorCodes, DateTime dtStart, DateTime dtEnd, string[] year, string[] years, int pageSize
         , int pageNo, out int recordTotal, string COnAQI, string orderBy = "PointId")
        {
            recordTotal = 0;
            DateTime mBegion = Convert.ToDateTime(dtStart.ToString("yyyy-MM-dd"));  //本期第一天
            DateTime mEnd = Convert.ToDateTime(dtEnd.ToString("yyyy-MM-dd"));   //本期当天
            string monthB = mBegion.ToString("MM-dd");
            string monthE = mEnd.ToString("MM-dd");

            g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            DataQueryByDayService m_DataQueryByDayService = Singleton<DataQueryByDayService>.GetInstance();

            DataView dv = new DataView();
            DataView dvT = new DataView();
            DataView dvN = new DataView();

            DataTable newdtb = new DataTable();
            newdtb.Columns.Add("PointId", typeof(int));
            newdtb.Columns.Add("PointName", typeof(string));
            string cf = mBegion.Year.ToString() + "年" + mBegion.Month.ToString() + "月" + mBegion.Day.ToString() + "日" + "~" + mEnd.Month.ToString() + "月" + mEnd.Day.ToString() + "日";
            if (COnAQI == "1")
            {
                foreach (string factor in factorCodes)
                {

                    newdtb.Columns.Add(factor + cf, typeof(string));
                    for (int i = 0; i < years.Length; i++)
                    {
                        if (years[i] != "" && Convert.ToInt32(years[i]) != mBegion.Year)
                        {
                            string str = years[i] + "年" + mBegion.Month.ToString() + "月" + mBegion.Day.ToString() + "日" + "~" + mEnd.Month.ToString() + "月" + mEnd.Day.ToString() + "日";
                            newdtb.Columns.Add(factor + str, typeof(string));
                        }
                    }
                    for (int m = 0; m < year.Length; m++)
                    {
                        if (year[m] != "")
                            newdtb.Columns.Add(factor + year[m] + "考核基数", typeof(string));

                    }
                    for (int j = 0; j < years.Length; j++)
                    {
                        if (years[j] != "" && Convert.ToInt32(years[j]) != mBegion.Year)
                        {
                            newdtb.Columns.Add(factor + "与" + years[j].ToString() + "年比较", typeof(string));
                        }
                    }

                    for (int m = 0; m < year.Length; m++)
                    {
                        if (year[m] != "")
                            newdtb.Columns.Add(factor + "与" + year[m] + "考核基数比较", typeof(string));

                    }
                }
            }
            else
            {
                newdtb.Columns.Add(cf, typeof(string));
                for (int i = 0; i < years.Length; i++)
                {
                    if (years[i] != "" && Convert.ToInt32(years[i]) != mBegion.Year)
                    {
                        string str = years[i] + "年" + mBegion.Month.ToString() + "月" + mBegion.Day.ToString() + "日" + "~" + mEnd.Month.ToString() + "月" + mEnd.Day.ToString() + "日";
                        newdtb.Columns.Add(str, typeof(string));
                    }
                }
                for (int m = 0; m < year.Length; m++)
                {
                    if (year[m] != "")
                        newdtb.Columns.Add(year[m] + "考核基数", typeof(string));

                }
                for (int j = 0; j < years.Length; j++)
                {
                    if (years[j] != "" && Convert.ToInt32(years[j]) != mBegion.Year)
                    {
                        newdtb.Columns.Add("与" + years[j].ToString() + "年比较", typeof(string));
                    }
                }

                for (int m = 0; m < year.Length; m++)
                {
                    if (year[m] != "")
                        newdtb.Columns.Add("与" + year[m] + "考核基数比较", typeof(string));

                }
            }
            dv = GetPortDataPagerDayAQI(portIds, mBegion, mEnd, pageSize, pageNo, out recordTotal, orderBy);  // 本期

            DataTable dt = dv.ToTable();   //本期
            DataRow[] Rowdt;
            for (int i = 0; i < portIds.Length; i++)
            {
                string PointName = "";
                decimal CurrentFactorCon = -1000;
                decimal SameFactorCon = -1000;
                decimal BaseFactorCon = -1000;
                decimal PM25CurrentFactorCon = -1000;
                decimal PM25SameFactorCon = -1000;
                decimal PM25BaseFactorCon = -1000;
                decimal PM10CurrentFactorCon = -1000;
                decimal PM10SameFactorCon = -1000;
                decimal PM10BaseFactorCon = -1000;
                decimal SO2CurrentFactorCon = -1000;
                decimal SO2SameFactorCon = -1000;
                decimal SO2BaseFactorCon = -1000;
                decimal NO2CurrentFactorCon = -1000;
                decimal NO2SameFactorCon = -1000;
                decimal NO2BaseFactorCon = -1000;
                decimal COCurrentFactorCon = -1000;
                decimal COSameFactorCon = -1000;
                decimal COBaseFactorCon = -1000;
                decimal O3CurrentFactorCon = -1000;
                decimal O3SameFactorCon = -1000;
                decimal O3BaseFactorCon = -1000;
                DataRow newRow = newdtb.NewRow();
                PointName = g_MonitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(portIds[i])).MonitoringPointName;


                Rowdt = dt.Select("PointId='" + portIds[i] + "'");   //2015

                if (Rowdt.Length > 0)
                {

                    if (COnAQI == "1")
                    {
                        foreach (string factor in factorCodes)
                        {
                            if (Rowdt[0][factor].IsNotNullOrDBNull())
                            {
                                switch (factor)
                                {
                                    case "a34004":
                                        PM25CurrentFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(Rowdt[0][factor]) * 1000, 0);
                                        break;
                                    case "a34002":
                                        PM10CurrentFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(Rowdt[0][factor]) * 1000, 0);
                                        break;
                                    case "a21004":
                                        NO2CurrentFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(Rowdt[0][factor]) * 1000, 0);
                                        break;
                                    case "a21026":
                                        SO2CurrentFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(Rowdt[0][factor]) * 1000, 0);
                                        break;
                                    case "a21005":
                                        COCurrentFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(Rowdt[0][factor]), 1);
                                        break;
                                    case "a05024":
                                        O3CurrentFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(Rowdt[0][factor]) * 1000, 0);
                                        break;
                                }
                            }

                        }
                    }
                    else
                    {
                        for (int j = 1; j < 6; j++)
                        {
                            string factors = dt.Columns[j].ColumnName;
                            int count = 24;
                            if (factors == "a05024")
                            {
                                count = 8;
                            }
                            if (Rowdt[0][j].IsNotNullOrDBNull())
                            {
                                decimal Con = DecimalExtension.GetRoundValue(Convert.ToDecimal(Rowdt[0][j]), 4);
                                decimal temp = DecimalExtension.GetRoundValue(s_AQIService.GetAQI(factors, Con, count), 0);
                                if (CurrentFactorCon < temp)
                                {
                                    CurrentFactorCon = temp;
                                }
                            }
                        }
                    }
                }
                for (int j = 0; j < years.Length; j++)
                {
                    DataRow[] RowdtN;
                    if (years[j] != "" && Convert.ToInt32(years[j]) != mBegion.Year)
                    {
                        DateTime smBegion = Convert.ToDateTime(years[j] + "-" + monthB);   //基数第一天
                        DateTime smEnd = Convert.ToDateTime(years[j] + "-" + monthE);   //基数当天
                        //  同期
                        dvT = GetPortDataPagerDayAQI(portIds, smBegion, smEnd, pageSize, pageNo, out recordTotal, orderBy);
                        DataTable dtN = dvT.ToTable();   //同期
                        RowdtN = dtN.Select("PointId='" + portIds[i] + "'");   //2014
                        if (RowdtN.Length > 0)
                        {
                            if (COnAQI == "1")
                            {
                                foreach (string factor in factorCodes)
                                {
                                    if (RowdtN[0][factor].IsNotNullOrDBNull())
                                    {
                                        switch (factor)
                                        {
                                            case "a34004":
                                                PM25SameFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(RowdtN[0][factor]) * 1000, 0);
                                                break;
                                            case "a34002":
                                                PM10SameFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(RowdtN[0][factor]) * 1000, 0);
                                                break;
                                            case "a21004":
                                                NO2SameFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(RowdtN[0][factor]) * 1000, 0);
                                                break;
                                            case "a21026":
                                                SO2SameFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(RowdtN[0][factor]) * 1000, 0);
                                                break;
                                            case "a21005":
                                                COSameFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(RowdtN[0][factor]), 1);
                                                break;
                                            case "a05024":
                                                O3SameFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(RowdtN[0][factor]) * 1000, 0);
                                                break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                for (int n = 1; n < 6; n++)
                                {
                                    string factors = dtN.Columns[n].ColumnName;
                                    int count = 24;
                                    if (factors == "a05024")
                                    {
                                        count = 8;
                                    }
                                    if (RowdtN[0][n].IsNotNullOrDBNull())
                                    {
                                        decimal Con = DecimalExtension.GetRoundValue(Convert.ToDecimal(RowdtN[0][n]), 4);
                                        decimal temp = DecimalExtension.GetRoundValue(s_AQIService.GetAQI(factors, Con, count), 0);
                                        if (SameFactorCon < temp)
                                        {
                                            SameFactorCon = temp;
                                        }
                                    }
                                }
                            }
                        }
                        if (RowdtN.Length == 0)
                            ;
                        else
                        {
                            string str = years[j] + "年" + mBegion.Month.ToString() + "月" + mBegion.Day.ToString() + "日" + "~" + mEnd.Month.ToString() + "月" + mEnd.Day.ToString() + "日";
                            string sr = "与" + years[j].ToString() + "年比较";

                            if (COnAQI == "1")
                            {
                                foreach (string factor in factorCodes)
                                {
                                    switch (factor)
                                    {
                                        case "a34004":
                                            if (PM25SameFactorCon != -1000)
                                                newRow[factor + str] = PM25SameFactorCon.ToString();

                                            if (PM25SameFactorCon != 0 && PM25SameFactorCon != -1000 && PM25CurrentFactorCon != -1000)
                                            {
                                                newRow[factor + sr] = DecimalExtension.GetRoundValue((PM25CurrentFactorCon - PM25SameFactorCon) / PM25SameFactorCon * 100, 1) + "%";
                                            }
                                            break;
                                        case "a34002":
                                            if (PM10SameFactorCon != -1000)
                                                newRow[factor + str] = PM10SameFactorCon.ToString();

                                            if (PM10SameFactorCon != 0 && PM10SameFactorCon != -1000 && PM10CurrentFactorCon != -1000)
                                            {
                                                newRow[factor + sr] = DecimalExtension.GetRoundValue((PM10CurrentFactorCon - PM10SameFactorCon) / PM10SameFactorCon * 100, 1) + "%";
                                            }
                                            break;
                                        case "a21004":
                                            if (NO2SameFactorCon != -1000)
                                                newRow[factor + str] = NO2SameFactorCon.ToString();

                                            if (NO2SameFactorCon != 0 && NO2SameFactorCon != -1000 && NO2CurrentFactorCon != -1000)
                                            {
                                                newRow[factor + sr] = DecimalExtension.GetRoundValue((NO2CurrentFactorCon - NO2SameFactorCon) / NO2SameFactorCon * 100, 1) + "%";
                                            }
                                            break;
                                        case "a21026":
                                            if (SO2SameFactorCon != -1000)
                                                newRow[factor + str] = SO2SameFactorCon.ToString();

                                            if (SO2SameFactorCon != 0 && SO2SameFactorCon != -1000 && SO2CurrentFactorCon != -1000)
                                            {
                                                newRow[factor + sr] = DecimalExtension.GetRoundValue((SO2CurrentFactorCon - SO2SameFactorCon) / SO2SameFactorCon * 100, 1) + "%";
                                            }
                                            break;
                                        case "a21005":
                                            if (COSameFactorCon != -1000)
                                                newRow[factor + str] = COSameFactorCon.ToString();

                                            if (COSameFactorCon != 0 && COSameFactorCon != -1000 && COCurrentFactorCon != -1000)
                                            {
                                                newRow[factor + sr] = DecimalExtension.GetRoundValue((COCurrentFactorCon - COSameFactorCon) / COSameFactorCon * 100, 1) + "%";
                                            }
                                            break;
                                        case "a05024":
                                            if (O3SameFactorCon != -1000)
                                                newRow[factor + str] = O3SameFactorCon.ToString();

                                            if (O3SameFactorCon != 0 && O3SameFactorCon != -1000 && O3CurrentFactorCon != -1000)
                                            {
                                                newRow[factor + sr] = DecimalExtension.GetRoundValue((O3CurrentFactorCon - O3SameFactorCon) / O3SameFactorCon * 100, 1) + "%";
                                            }
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (SameFactorCon != -1000)
                                    newRow[str] = SameFactorCon.ToString();

                                if (SameFactorCon != 0 && SameFactorCon != -1000 && CurrentFactorCon != -1000)
                                {
                                    newRow[sr] = DecimalExtension.GetRoundValue((CurrentFactorCon - SameFactorCon) / SameFactorCon * 100, 1) + "%";
                                }
                            }
                        }
                    }

                }

                for (int j = 0; j < year.Length; j++)
                {
                    if (year[j] != "")
                    {
                        DataRow[] RowdtNew;
                        //基数
                        dvN = m_DataQueryByDayService.GetConcentrationDay(portIds, dtStart, dtEnd, year[j]);
                        DataTable dtNew = dvN.ToTable();  //基数
                        RowdtNew = dtNew.Select("PointId='" + portIds[i] + "'");   //2013               
                        if (RowdtNew.Length > 0)
                        {
                            if (COnAQI == "1")
                            {
                                foreach (string factor in factorCodes)
                                {
                                    if (RowdtNew[0][factor].IsNotNullOrDBNull())
                                    {
                                        switch (factor)
                                        {
                                            case "a34004":
                                                PM25BaseFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(RowdtNew[0][factor]) * 1000, 0);
                                                break;
                                            case "a34002":
                                                PM10BaseFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(RowdtNew[0][factor]) * 1000, 0);
                                                break;
                                            case "a21004":
                                                NO2BaseFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(RowdtNew[0][factor]) * 1000, 0);
                                                break;
                                            case "a21026":
                                                SO2BaseFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(RowdtNew[0][factor]) * 1000, 0);
                                                break;
                                            case "a21005":
                                                COBaseFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(RowdtNew[0][factor]), 1);
                                                break;
                                            case "a05024":
                                                O3BaseFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(RowdtNew[0][factor]) * 1000, 0);
                                                break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                for (int m = 1; m < 6; m++)
                                {
                                    string factors = dtNew.Columns[m].ColumnName;
                                    int count = 24;
                                    if (factors == "a05024")
                                    {
                                        count = 8;
                                    }
                                    if (RowdtNew[0][m].IsNotNullOrDBNull())
                                    {
                                        decimal Con = DecimalExtension.GetRoundValue(Convert.ToDecimal(RowdtNew[0][m]), 4);
                                        decimal temp = DecimalExtension.GetRoundValue(s_AQIService.GetAQI(factors, Con, count), 0);
                                        if (BaseFactorCon < temp)
                                        {
                                            BaseFactorCon = temp;
                                        }
                                    }
                                }
                            }
                        }
                        if (RowdtNew.Length == 0)
                            ;
                        else
                        {
                            if (COnAQI == "1")
                            {
                                foreach (string factor in factorCodes)
                                {
                                    switch (factor)
                                    {
                                        case "a34004":
                                            if (PM25BaseFactorCon != -1000)
                                                newRow[factor + year[j] + "考核基数"] = PM25BaseFactorCon.ToString();
                                            if (PM25BaseFactorCon != 0 && PM25BaseFactorCon != -1000 && PM25CurrentFactorCon != -1000)
                                            {
                                                newRow[factor + "与" + year[j] + "考核基数比较"] = DecimalExtension.GetRoundValue((PM25CurrentFactorCon - PM25BaseFactorCon) / PM25BaseFactorCon * 100, 1) + "%";
                                            }
                                            break;
                                        case "a34002":
                                            if (PM10BaseFactorCon != -1000)
                                                newRow[factor + year[j] + "考核基数"] = PM10BaseFactorCon.ToString();
                                            if (PM10BaseFactorCon != 0 && PM10BaseFactorCon != -1000 && PM10CurrentFactorCon != -1000)
                                            {
                                                newRow[factor + "与" + year[j] + "考核基数比较"] = DecimalExtension.GetRoundValue((PM10CurrentFactorCon - PM10BaseFactorCon) / PM10BaseFactorCon * 100, 1) + "%";
                                            }
                                            break;
                                        case "a21004":
                                            if (NO2BaseFactorCon != -1000)
                                                newRow[factor + year[j] + "考核基数"] = NO2BaseFactorCon.ToString();
                                            if (NO2BaseFactorCon != 0 && NO2BaseFactorCon != -1000 && NO2CurrentFactorCon != -1000)
                                            {
                                                newRow[factor + "与" + year[j] + "考核基数比较"] = DecimalExtension.GetRoundValue((NO2CurrentFactorCon - NO2BaseFactorCon) / NO2BaseFactorCon * 100, 1) + "%";
                                            }
                                            break;
                                        case "a21026":
                                            if (SO2BaseFactorCon != -1000)
                                                newRow[factor + year[j] + "考核基数"] = SO2BaseFactorCon.ToString();
                                            if (SO2BaseFactorCon != 0 && SO2BaseFactorCon != -1000 && SO2CurrentFactorCon != -1000)
                                            {
                                                newRow[factor + "与" + year[j] + "考核基数比较"] = DecimalExtension.GetRoundValue((SO2CurrentFactorCon - SO2BaseFactorCon) / SO2BaseFactorCon * 100, 1) + "%";
                                            }
                                            break;
                                        case "a21005":
                                            if (COBaseFactorCon != -1000)
                                                newRow[factor + year[j] + "考核基数"] = COBaseFactorCon.ToString();
                                            if (COBaseFactorCon != 0 && COBaseFactorCon != -1000 && COCurrentFactorCon != -1000)
                                            {
                                                newRow[factor + "与" + year[j] + "考核基数比较"] = DecimalExtension.GetRoundValue((COCurrentFactorCon - COBaseFactorCon) / COBaseFactorCon * 100, 1) + "%";
                                            }
                                            break;
                                        case "a05024":
                                            if (O3BaseFactorCon != -1000)
                                                newRow[factor + year[j] + "考核基数"] = O3BaseFactorCon.ToString();
                                            if (O3BaseFactorCon != 0 && O3BaseFactorCon != -1000 && O3CurrentFactorCon != -1000)
                                            {
                                                newRow[factor + "与" + year[j] + "考核基数比较"] = DecimalExtension.GetRoundValue((O3CurrentFactorCon - O3BaseFactorCon) / O3BaseFactorCon * 100, 1) + "%";
                                            }
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (BaseFactorCon != -1000)
                                    newRow[year[j] + "考核基数"] = BaseFactorCon.ToString();
                                if (BaseFactorCon != 0 && BaseFactorCon != -1000 && CurrentFactorCon != -1000)
                                {
                                    newRow["与" + year[j] + "考核基数比较"] = DecimalExtension.GetRoundValue((CurrentFactorCon - BaseFactorCon) / BaseFactorCon * 100, 1) + "%";
                                }
                            }
                        }

                    }
                    if (Rowdt.Length == 0)
                    {
                        newRow["PointId"] = Convert.ToInt32(portIds[i]);
                        newRow["PointName"] = PointName;
                        newdtb.Rows.Add(newRow);
                    }
                    else
                    {
                        newRow["PointId"] = Convert.ToInt32(portIds[i]);
                        newRow["PointName"] = PointName;
                        if (COnAQI == "1")
                        {
                            foreach (string factor in factorCodes)
                            {
                                switch (factor)
                                {
                                    case "a34004":
                                        if (PM25CurrentFactorCon != -1000)
                                            newRow[factor + cf] = PM25CurrentFactorCon.ToString();
                                        break;
                                    case "a34002":
                                        if (PM10CurrentFactorCon != -1000)
                                            newRow[factor + cf] = PM10CurrentFactorCon.ToString();
                                        break;
                                    case "a21004":
                                        if (NO2CurrentFactorCon != -1000)
                                            newRow[factor + cf] = NO2CurrentFactorCon.ToString();
                                        break;
                                    case "a21026":
                                        if (SO2CurrentFactorCon != -1000)
                                            newRow[factor + cf] = SO2CurrentFactorCon.ToString();
                                        break;
                                    case "a21005":
                                        if (COCurrentFactorCon != -1000)
                                            newRow[factor + cf] = COCurrentFactorCon.ToString();
                                        break;
                                    case "a05024":
                                        if (O3CurrentFactorCon != -1000)
                                            newRow[factor + cf] = O3CurrentFactorCon.ToString();
                                        break;
                                }
                            }
                        }
                        else
                            newRow[cf] = CurrentFactorCon.ToString();

                        newdtb.Rows.Add(newRow);
                    }
                }
            }
            return newdtb.DefaultView;
        }



        #endregion
        #region 根据区域统计
        /// <summary>
        /// 根据key主键判断记录是否存在
        /// </summary>
        /// <param name="strKey">主键值</param>
        /// <returns></returns>
        public bool RIsExist(string strKey)
        {
            regionDayAQI = Singleton<RegionDayAQIRepository>.GetInstance();
            if (regionDayAQI != null)
                return regionDayAQI.IsExist(strKey);
            return false;
        }

        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页（从0开始）</param> 
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns> 
        public DataView GetAreaDataPager(string[] regionGuids, string[] factorCodes, DateTime dtStart, DateTime dtEnd, string[] year, string[] years, int pageSize
        , int pageNo, out int recordTotal, string COnAQI)
        {
            recordTotal = 0;
            DateTime mBegion = Convert.ToDateTime(dtStart.ToString("yyyy-MM-dd"));  //本期第一天
            DateTime mEnd = Convert.ToDateTime(dtEnd.ToString("yyyy-MM-dd"));   //本期当天
            string monthB = mBegion.ToString("MM-dd");
            string monthE = mEnd.ToString("MM-dd");

            regionDayAQI = Singleton<RegionDayAQIRepository>.GetInstance();

            DataView dv = new DataView();
            DataView dvT = new DataView();
            DataView dvN = new DataView();

            DataTable newdtb = new DataTable();
            newdtb.Columns.Add("PointId", typeof(int));
            newdtb.Columns.Add("PointName", typeof(string));
            string cf = mBegion.Year.ToString() + "年" + mBegion.Month.ToString() + "月" + mBegion.Day.ToString() + "日" + "~" + mEnd.Month.ToString() + "月" + mEnd.Day.ToString() + "日";
            if (COnAQI == "1")
            {
                foreach (string factor in factorCodes)
                {

                    newdtb.Columns.Add(factor + cf, typeof(string));
                    for (int i = 0; i < years.Length; i++)
                    {
                        if (years[i] != "" && Convert.ToInt32(years[i]) != mBegion.Year)
                        {
                            string str = years[i] + "年" + mBegion.Month.ToString() + "月" + mBegion.Day.ToString() + "日" + "~" + mEnd.Month.ToString() + "月" + mEnd.Day.ToString() + "日";
                            newdtb.Columns.Add(factor + str, typeof(string));
                        }
                    }
                    for (int m = 0; m < year.Length; m++)
                    {
                        if (year[m] != "")
                            newdtb.Columns.Add(factor + year[m] + "考核基数", typeof(string));

                    }
                    for (int j = 0; j < years.Length; j++)
                    {
                        if (years[j] != "" && Convert.ToInt32(years[j]) != mBegion.Year)
                        {
                            newdtb.Columns.Add(factor + "与" + years[j].ToString() + "年比较", typeof(string));
                        }
                    }

                    for (int m = 0; m < year.Length; m++)
                    {
                        if (year[m] != "")
                            newdtb.Columns.Add(factor + "与" + year[m] + "考核基数比较", typeof(string));
                    }
                }
            }
            else
            {
                newdtb.Columns.Add(cf, typeof(string));
                for (int i = 0; i < years.Length; i++)
                {
                    if (years[i] != "" && Convert.ToInt32(years[i]) != mBegion.Year)
                    {
                        string str = years[i] + "年" + mBegion.Month.ToString() + "月" + mBegion.Day.ToString() + "日" + "~" + mEnd.Month.ToString() + "月" + mEnd.Day.ToString() + "日";
                        newdtb.Columns.Add(str, typeof(string));
                    }
                }
                for (int m = 0; m < year.Length; m++)
                {
                    if (year[m] != "")
                        newdtb.Columns.Add(year[m] + "考核基数", typeof(string));

                }
                for (int j = 0; j < years.Length; j++)
                {
                    if (years[j] != "" && Convert.ToInt32(years[j]) != mBegion.Year)
                    {
                        newdtb.Columns.Add("与" + years[j].ToString() + "年比较", typeof(string));
                    }
                }

                for (int m = 0; m < year.Length; m++)
                {
                    if (year[m] != "")
                        newdtb.Columns.Add("与" + year[m] + "考核基数比较", typeof(string));

                }
            }
            dv = regionDayAQI.GetAreaDataPager(regionGuids, mBegion, mEnd, pageSize, pageNo, out recordTotal);  // 本期
            DataTable dt = dv.ToTable();   //本期         
            DataRow[] Rowdt;



            string quanshi = "";
            for (int i = 0; i < regionGuids.Length; i++)
            {

                string PointName = "";
                decimal CurrentFactorCon = -1000;
                decimal SameFactorCon = -1000;
                decimal BaseFactorCon = -1000;
                decimal PM25CurrentFactorCon = -1000;
                decimal PM25SameFactorCon = -1000;
                decimal PM25BaseFactorCon = -1000;
                decimal PM10CurrentFactorCon = -1000;
                decimal PM10SameFactorCon = -1000;
                decimal PM10BaseFactorCon = -1000;
                decimal SO2CurrentFactorCon = -1000;
                decimal SO2SameFactorCon = -1000;
                decimal SO2BaseFactorCon = -1000;
                decimal NO2CurrentFactorCon = -1000;
                decimal NO2SameFactorCon = -1000;
                decimal NO2BaseFactorCon = -1000;
                decimal COCurrentFactorCon = -1000;
                decimal COSameFactorCon = -1000;
                decimal COBaseFactorCon = -1000;
                decimal O3CurrentFactorCon = -1000;
                decimal O3SameFactorCon = -1000;
                decimal O3BaseFactorCon = -1000;

                DataRow newRow = newdtb.NewRow();
                PointName = g_DictionaryService.GetCodeDictionaryTextByValue(regionGuids[i]);


                Rowdt = dt.Select("MonitoringRegionUid='" + regionGuids[i] + "'");   //2015

                if (Rowdt.Length > 0)
                {
                    if (COnAQI == "1")
                    {
                        foreach (string factor in factorCodes)
                        {
                            if (Rowdt[0][factor].IsNotNullOrDBNull())
                            {
                                switch (factor)
                                {
                                    case "a34004":
                                        PM25CurrentFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(Rowdt[0][factor]) * 1000, 0);
                                        break;
                                    case "a34002":
                                        PM10CurrentFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(Rowdt[0][factor]) * 1000, 0);
                                        break;
                                    case "a21004":
                                        NO2CurrentFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(Rowdt[0][factor]) * 1000, 0);
                                        break;
                                    case "a21026":
                                        SO2CurrentFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(Rowdt[0][factor]) * 1000, 0);
                                        break;
                                    case "a21005":
                                        COCurrentFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(Rowdt[0][factor]), 1);
                                        break;
                                    case "a05024":
                                        O3CurrentFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(Rowdt[0][factor]) * 1000, 0);
                                        break;
                                }
                            }

                        }
                    }
                    else
                    {
                        for (int j = 1; j < 7; j++)
                        {
                            string factors = dt.Columns[j].ColumnName;
                            int count = 24;
                            if (factors == "a05024")
                            {
                                count = 8;
                            }
                            if (Rowdt[0][j].IsNotNullOrDBNull())
                            {
                                decimal Con = DecimalExtension.GetRoundValue(Convert.ToDecimal(Rowdt[0][j]), 4);
                                decimal temp = DecimalExtension.GetRoundValue(s_AQIService.GetAQI(factors, Con, count), 0);
                                if (CurrentFactorCon < temp)
                                {
                                    CurrentFactorCon = temp;
                                }
                            }
                        }
                    }
                }

                for (int j = 0; j < years.Length; j++)
                {
                    DataRow[] RowdtN;
                    if (years[j] != "" && Convert.ToInt32(years[j]) != mBegion.Year)
                    {
                        DateTime smBegion = Convert.ToDateTime(years[j] + "-" + monthB);   //基数第一天
                        DateTime smEnd = DateTime.Parse((years[j].ToString() + "-" + monthE).ToString());    //基数当天
                        //  同期
                        dvT = regionDayAQI.GetAreaDataPager(regionGuids, smBegion, smEnd, pageSize, pageNo, out recordTotal);  //  同期
                        DataTable dtN = dvT.ToTable();   //同期
                        RowdtN = dtN.Select("MonitoringRegionUid='" + regionGuids[i] + "'");   //2014
                        if (RowdtN.Length > 0)
                        {
                            if (COnAQI == "1")
                            {
                                foreach (string factor in factorCodes)
                                {
                                    if (RowdtN[0][factor].IsNotNullOrDBNull())
                                    {
                                        switch (factor)
                                        {
                                            case "a34004":
                                                PM25SameFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(RowdtN[0][factor]) * 1000, 0);
                                                break;
                                            case "a34002":
                                                PM10SameFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(RowdtN[0][factor]) * 1000, 0);
                                                break;
                                            case "a21004":
                                                NO2SameFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(RowdtN[0][factor]) * 1000, 0);
                                                break;
                                            case "a21026":
                                                SO2SameFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(RowdtN[0][factor]) * 1000, 0);
                                                break;
                                            case "a21005":
                                                COSameFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(RowdtN[0][factor]), 1);
                                                break;
                                            case "a05024":
                                                O3SameFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(RowdtN[0][factor]) * 1000, 0);
                                                break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                for (int n = 1; n < 7; n++)
                                {
                                    string factors = dtN.Columns[n].ColumnName;
                                    int count = 24;
                                    if (factors == "a05024")
                                    {
                                        count = 8;
                                    }
                                    if (RowdtN[0][n].IsNotNullOrDBNull())
                                    {
                                        decimal Con = DecimalExtension.GetRoundValue(Convert.ToDecimal(RowdtN[0][n]), 4);
                                        decimal temp = DecimalExtension.GetRoundValue(s_AQIService.GetAQI(factors, Con, count), 0);
                                        if (SameFactorCon < temp)
                                        {
                                            SameFactorCon = temp;
                                        }
                                    }
                                }
                            }
                        }

                        if (RowdtN.Length == 0)
                            ;
                        else
                        {

                            string str = years[j] + "年" + mBegion.Month.ToString() + "月" + mBegion.Day.ToString() + "日" + "~" + mEnd.Month.ToString() + "月" + mEnd.Day.ToString() + "日";
                            string sr = "与" + years[j].ToString() + "年比较";
                            if (COnAQI == "1")
                            {
                                foreach (string factor in factorCodes)
                                {
                                    switch (factor)
                                    {
                                        case "a34004":
                                            if (PM25SameFactorCon != -1000)
                                                newRow[factor + str] = PM25SameFactorCon.ToString();

                                            if (PM25SameFactorCon != 0 && PM25SameFactorCon != -1000 && PM25CurrentFactorCon != -1000)
                                            {
                                                newRow[factor + sr] = DecimalExtension.GetRoundValue((PM25CurrentFactorCon - PM25SameFactorCon) / PM25SameFactorCon * 100, 1) + "%";
                                            }
                                            break;
                                        case "a34002":
                                            if (PM10SameFactorCon != -1000)
                                                newRow[factor + str] = PM10SameFactorCon.ToString();

                                            if (PM10SameFactorCon != 0 && PM10SameFactorCon != -1000 && PM10CurrentFactorCon != -1000)
                                            {
                                                newRow[factor + sr] = DecimalExtension.GetRoundValue((PM10CurrentFactorCon - PM10SameFactorCon) / PM10SameFactorCon * 100, 1) + "%";
                                            }
                                            break;
                                        case "a21004":
                                            if (NO2SameFactorCon != -1000)
                                                newRow[factor + str] = NO2SameFactorCon.ToString();

                                            if (NO2SameFactorCon != 0 && NO2SameFactorCon != -1000 && NO2CurrentFactorCon != -1000)
                                            {
                                                newRow[factor + sr] = DecimalExtension.GetRoundValue((NO2CurrentFactorCon - NO2SameFactorCon) / NO2SameFactorCon * 100, 1) + "%";
                                            }
                                            break;
                                        case "a21026":
                                            if (SO2SameFactorCon != -1000)
                                                newRow[factor + str] = SO2SameFactorCon.ToString();

                                            if (SO2SameFactorCon != 0 && SO2SameFactorCon != -1000 && SO2CurrentFactorCon != -1000)
                                            {
                                                newRow[factor + sr] = DecimalExtension.GetRoundValue((SO2CurrentFactorCon - SO2SameFactorCon) / SO2SameFactorCon * 100, 1) + "%";
                                            }
                                            break;
                                        case "a21005":
                                            if (COSameFactorCon != -1000)
                                                newRow[factor + str] = COSameFactorCon.ToString();

                                            if (COSameFactorCon != 0 && COSameFactorCon != -1000 && COCurrentFactorCon != -1000)
                                            {
                                                newRow[factor + sr] = DecimalExtension.GetRoundValue((COCurrentFactorCon - COSameFactorCon) / COSameFactorCon * 100, 1) + "%";
                                            }
                                            break;
                                        case "a05024":
                                            if (O3SameFactorCon != -1000)
                                                newRow[factor + str] = O3SameFactorCon.ToString();

                                            if (O3SameFactorCon != 0 && O3SameFactorCon != -1000 && O3CurrentFactorCon != -1000)
                                            {
                                                newRow[factor + sr] = DecimalExtension.GetRoundValue((O3CurrentFactorCon - O3SameFactorCon) / O3SameFactorCon * 100, 1) + "%";
                                            }
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (SameFactorCon != -1000)
                                    newRow[str] = SameFactorCon.ToString();
                                if (SameFactorCon != 0 && SameFactorCon != -1000 && CurrentFactorCon != -1000)
                                {
                                    newRow[sr] = DecimalExtension.GetRoundValue((CurrentFactorCon - SameFactorCon) / SameFactorCon * 100, 1) + "%";
                                }
                            }
                        }
                    }
                }
                for (int j = 0; j < year.Length; j++)
                {
                    decimal Base = 0;
                    if (year[j] != "")
                    {
                        DataRow[] RowdtNew;
                        //基数
                        dvN = m_DataQueryByDayService.GetRegionConcentrationDay(regionGuids, dtStart, dtEnd, year[j]);   //  基数
                        DataTable dtNew = dvN.ToTable();  //基数
                        RowdtNew = dtNew.Select("MonitoringRegionUid='" + regionGuids[i] + "'");   //2013
                        if (RowdtNew.Length > 0)
                        {
                            if (COnAQI == "1")
                            {
                                foreach (string factor in factorCodes)
                                {
                                    if (RowdtNew[0][factor].IsNotNullOrDBNull())
                                    {
                                        switch (factor)
                                        {
                                            case "a34004":
                                                PM25BaseFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(RowdtNew[0][factor]) * 1000, 0);
                                                break;
                                            case "a34002":
                                                PM10BaseFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(RowdtNew[0][factor]) * 1000, 0);
                                                break;
                                            case "a21004":
                                                NO2BaseFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(RowdtNew[0][factor]) * 1000, 0);
                                                break;
                                            case "a21026":
                                                SO2BaseFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(RowdtNew[0][factor]) * 1000, 0);
                                                break;
                                            case "a21005":
                                                COBaseFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(RowdtNew[0][factor]), 1);
                                                break;
                                            case "a05024":
                                                O3BaseFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(RowdtNew[0][factor]) * 1000, 0);
                                                break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                for (int m = 1; m < 7; m++)
                                {
                                    string factors = dtNew.Columns[m].ColumnName;
                                    int count = 24;
                                    if (factors == "a05024")
                                    {
                                        count = 8;
                                    }
                                    if (RowdtNew[0][m].IsNotNullOrDBNull())
                                    {
                                        decimal Con = DecimalExtension.GetRoundValue(Convert.ToDecimal(RowdtNew[0][m]), 4);
                                        decimal temp = DecimalExtension.GetRoundValue(s_AQIService.GetAQI(factors, Con, count), 0);
                                        if (BaseFactorCon < temp)
                                        {
                                            BaseFactorCon = temp;
                                        }
                                    }
                                }
                            }
                        }
                        if (RowdtNew.Length == 0)
                            ;
                        else
                        {
                            if (COnAQI == "1")
                            {
                                foreach (string factor in factorCodes)
                                {
                                    switch (factor)
                                    {
                                        case "a34004":
                                            if (PM25BaseFactorCon != -1000)
                                                newRow[factor + year[j] + "考核基数"] = PM25BaseFactorCon.ToString();
                                            if (PM25BaseFactorCon != 0 && PM25BaseFactorCon != -1000 && PM25CurrentFactorCon != -1000)
                                            {
                                                newRow[factor + "与" + year[j] + "考核基数比较"] = DecimalExtension.GetRoundValue((PM25CurrentFactorCon - PM25BaseFactorCon) / PM25BaseFactorCon * 100, 1) + "%";
                                            }
                                            break;
                                        case "a34002":
                                            if (PM10BaseFactorCon != -1000)
                                                newRow[factor + year[j] + "考核基数"] = PM10BaseFactorCon.ToString();
                                            if (PM10BaseFactorCon != 0 && PM10BaseFactorCon != -1000 && PM10CurrentFactorCon != -1000)
                                            {
                                                newRow[factor + "与" + year[j] + "考核基数比较"] = DecimalExtension.GetRoundValue((PM10CurrentFactorCon - PM10BaseFactorCon) / PM10BaseFactorCon * 100, 1) + "%";
                                            }
                                            break;
                                        case "a21004":
                                            if (NO2BaseFactorCon != -1000)
                                                newRow[factor + year[j] + "考核基数"] = NO2BaseFactorCon.ToString();
                                            if (NO2BaseFactorCon != 0 && NO2BaseFactorCon != -1000 && NO2CurrentFactorCon != -1000)
                                            {
                                                newRow[factor + "与" + year[j] + "考核基数比较"] = DecimalExtension.GetRoundValue((NO2CurrentFactorCon - NO2BaseFactorCon) / NO2BaseFactorCon * 100, 1) + "%";
                                            }
                                            break;
                                        case "a21026":
                                            if (SO2BaseFactorCon != -1000)
                                                newRow[factor + year[j] + "考核基数"] = SO2BaseFactorCon.ToString();
                                            if (SO2BaseFactorCon != 0 && SO2BaseFactorCon != -1000 && SO2CurrentFactorCon != -1000)
                                            {
                                                newRow[factor + "与" + year[j] + "考核基数比较"] = DecimalExtension.GetRoundValue((SO2CurrentFactorCon - SO2BaseFactorCon) / SO2BaseFactorCon * 100, 1) + "%";
                                            }
                                            break;
                                        case "a21005":
                                            if (COBaseFactorCon != -1000)
                                                newRow[factor + year[j] + "考核基数"] = COBaseFactorCon.ToString();
                                            if (COBaseFactorCon != 0 && COBaseFactorCon != -1000 && COCurrentFactorCon != -1000)
                                            {
                                                newRow[factor + "与" + year[j] + "考核基数比较"] = DecimalExtension.GetRoundValue((COCurrentFactorCon - COBaseFactorCon) / COBaseFactorCon * 100, 1) + "%";
                                            }
                                            break;
                                        case "a05024":
                                            if (O3BaseFactorCon != -1000)
                                                newRow[factor + year[j] + "考核基数"] = O3BaseFactorCon.ToString();
                                            if (O3BaseFactorCon != 0 && O3BaseFactorCon != -1000 && O3CurrentFactorCon != -1000)
                                            {
                                                newRow[factor + "与" + year[j] + "考核基数比较"] = DecimalExtension.GetRoundValue((O3CurrentFactorCon - O3BaseFactorCon) / O3BaseFactorCon * 100, 1) + "%";
                                            }
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (BaseFactorCon != -1000)
                                    newRow[year[j] + "考核基数"] = BaseFactorCon.ToString();
                                if (BaseFactorCon != 0 && BaseFactorCon != -1000 && CurrentFactorCon != -1000)
                                {
                                    newRow["与" + year[j] + "考核基数比较"] = DecimalExtension.GetRoundValue((CurrentFactorCon - BaseFactorCon) / BaseFactorCon * 100, 1) + "%";
                                }
                            }

                        }
                    }
                }
                if (Rowdt.Length == 0)
                {
                    newRow["PointName"] = PointName;
                    newdtb.Rows.Add(newRow);
                }
                else
                {
                    newRow["PointName"] = PointName;
                    if (COnAQI == "1")
                    {
                        foreach (string factor in factorCodes)
                        {
                            switch (factor)
                            {
                                case "a34004":
                                    if (PM25CurrentFactorCon != -1000)
                                        newRow[factor + cf] = PM25CurrentFactorCon.ToString();
                                    break;
                                case "a34002":
                                    if (PM10CurrentFactorCon != -1000)
                                        newRow[factor + cf] = PM10CurrentFactorCon.ToString();
                                    break;
                                case "a21004":
                                    if (NO2CurrentFactorCon != -1000)
                                        newRow[factor + cf] = NO2CurrentFactorCon.ToString();
                                    break;
                                case "a21026":
                                    if (SO2CurrentFactorCon != -1000)
                                        newRow[factor + cf] = SO2CurrentFactorCon.ToString();
                                    break;
                                case "a21005":
                                    if (COCurrentFactorCon != -1000)
                                        newRow[factor + cf] = COCurrentFactorCon.ToString();
                                    break;
                                case "a05024":
                                    if (O3CurrentFactorCon != -1000)
                                        newRow[factor + cf] = O3CurrentFactorCon.ToString();
                                    break;
                            }
                        }
                    }
                    else
                        newRow[cf] = CurrentFactorCon.ToString();
                    newdtb.Rows.Add(newRow);
                }

            }
            DataView newdv = new DataView();
            newdv = new DataView(newdtb);
            return newdv;
        }
        /// <summary>
        /// 根据所选站点ID获取相应区域信息
        /// </summary>
        /// <param name="pointIds"></param>
        /// <returns></returns>
        public DataView GetRegionByPointId(string[] pointIds)
        {
            return pointAirService.GetRegionByPointId(pointIds);
        }
        
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页（从0开始）</param> 
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns> 
        public DataView GetAreaDataPagerNew(string[] portIds, string[] factorCodes, DateTime dtStart, DateTime dtEnd, string[] year, string[] years, int pageSize
        , int pageNo, out int recordTotal, string COnAQI)
        {
            recordTotal = 0;
            DateTime mBegion = Convert.ToDateTime(dtStart.ToString("yyyy-MM-dd"));  //本期第一天
            DateTime mEnd = Convert.ToDateTime(dtEnd.ToString("yyyy-MM-dd"));   //本期当天
            string monthB = mBegion.ToString("MM-dd");
            string monthE = mEnd.ToString("MM-dd");
            DataView dvRegion = GetRegionByPointId(portIds);
            /// <summary>
            /// 区域Uid集合
            /// </summary>
            List<string> listRegionUids = new List<string>();
            foreach (DataRowView dr in dvRegion)
            {
                string regionUid = dr["RegionUid"].ToString();
                listRegionUids.Add(regionUid);
            }
            string[] regionUids = listRegionUids.ToArray();

            //新建一个新的datatable,存放区域数据信息
            DataTable dtForAQI = new DataTable();
            DataTable dtForAQIT = new DataTable();
            DataTable dtForAQIS = new DataTable();
            regionDayAQI = Singleton<RegionDayAQIRepository>.GetInstance();

            DataView dv = new DataView();
            DataView dvT = new DataView();
            DataView dvN = new DataView();

            DataTable newdtb = new DataTable();
            newdtb.Columns.Add("PointId", typeof(int));
            newdtb.Columns.Add("PointName", typeof(string));
            string cf = mBegion.Year.ToString() + "年" + mBegion.Month.ToString() + "月" + mBegion.Day.ToString() + "日" + "~" + mEnd.Month.ToString() + "月" + mEnd.Day.ToString() + "日";
            if (COnAQI == "1")
            {
                foreach (string factor in factorCodes)
                {

                    newdtb.Columns.Add(factor + cf, typeof(string));
                    for (int i = 0; i < years.Length; i++)
                    {
                        if (years[i] != "" && Convert.ToInt32(years[i]) != mBegion.Year)
                        {
                            string str = years[i] + "年" + mBegion.Month.ToString() + "月" + mBegion.Day.ToString() + "日" + "~" + mEnd.Month.ToString() + "月" + mEnd.Day.ToString() + "日";
                            newdtb.Columns.Add(factor + str, typeof(string));
                        }
                    }
                    for (int m = 0; m < year.Length; m++)
                    {
                        if (year[m] != "")
                            newdtb.Columns.Add(factor + year[m] + "考核基数", typeof(string));

                    }
                    for (int j = 0; j < years.Length; j++)
                    {
                        if (years[j] != "" && Convert.ToInt32(years[j]) != mBegion.Year)
                        {
                            newdtb.Columns.Add(factor + "与" + years[j].ToString() + "年比较", typeof(string));
                        }
                    }

                    for (int m = 0; m < year.Length; m++)
                    {
                        if (year[m] != "")
                            newdtb.Columns.Add(factor + "与" + year[m] + "考核基数比较", typeof(string));
                    }
                }
            }
            else
            {
                newdtb.Columns.Add(cf, typeof(string));
                for (int i = 0; i < years.Length; i++)
                {
                    if (years[i] != "" && Convert.ToInt32(years[i]) != mBegion.Year)
                    {
                        string str = years[i] + "年" + mBegion.Month.ToString() + "月" + mBegion.Day.ToString() + "日" + "~" + mEnd.Month.ToString() + "月" + mEnd.Day.ToString() + "日";
                        newdtb.Columns.Add(str, typeof(string));
                    }
                }
                for (int m = 0; m < year.Length; m++)
                {
                    if (year[m] != "")
                        newdtb.Columns.Add(year[m] + "考核基数", typeof(string));

                }
                for (int j = 0; j < years.Length; j++)
                {
                    if (years[j] != "" && Convert.ToInt32(years[j]) != mBegion.Year)
                    {
                        newdtb.Columns.Add("与" + years[j].ToString() + "年比较", typeof(string));
                    }
                }

                for (int m = 0; m < year.Length; m++)
                {
                    if (year[m] != "")
                        newdtb.Columns.Add("与" + year[m] + "考核基数比较", typeof(string));

                }
            }
            List<string> regionName = dvRegion.ToTable().AsEnumerable().Select(t => t.Field<string>("Region")).ToList();
            IEnumerable<string> names = regionName.Distinct();
            //dv = regionDayAQI.GetAreaDataPager(regionUids, mBegion, mEnd, pageSize, pageNo, out recordTotal);  // 本期
            //DataTable dt = dv.ToTable();   //本期         
            DataRow[] Rowdt;
            //给datatable增加列
            dtForAQI.Columns.Add("MonitoringRegionUid", typeof(string));
            dtForAQI.Columns.Add("a34004", typeof(string));
            dtForAQI.Columns.Add("a34002", typeof(string));
            dtForAQI.Columns.Add("a21004", typeof(string));
            dtForAQI.Columns.Add("a21026", typeof(string));
            dtForAQI.Columns.Add("a21005", typeof(string));
            dtForAQI.Columns.Add("a05024", typeof(string));
            dtForAQI.Columns.Add("Max_AQI", typeof(string));
            //给datatable增加列
            dtForAQIT.Columns.Add("MonitoringRegionUid", typeof(string));
            dtForAQIT.Columns.Add("a34004", typeof(string));
            dtForAQIT.Columns.Add("a34002", typeof(string));
            dtForAQIT.Columns.Add("a21004", typeof(string));
            dtForAQIT.Columns.Add("a21026", typeof(string));
            dtForAQIT.Columns.Add("a21005", typeof(string));
            dtForAQIT.Columns.Add("a05024", typeof(string));
            dtForAQIT.Columns.Add("Max_AQI", typeof(string));
            foreach (string name in names)
            {
                List<string> list = new List<string>();
                string[] ids = { };
                DataRow[] drs = dvRegion.ToTable().Select("Region='" + name + "'").ToArray<DataRow>();
                for (int j = 0; j < drs.Length; j++)
                {
                    list.Add(drs[j]["PortId"].ToString());
                }
                ids = list.ToArray();

                decimal? PM25PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34004", mBegion, mEnd, 24, "2");
                decimal? PM10PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34002", mBegion, mEnd, 24, "2");
                decimal? NO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21004", mBegion, mEnd, 24, "2");
                decimal? SO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21026", mBegion, mEnd, 24, "2");
                decimal? COPollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21005", mBegion, mEnd, 24, "2");
                decimal? Max8HourO3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05024", mBegion, mEnd, 8, "2");
                int? PM25Value = m_AQICalculateService.GetIAQI("a34004", Convert.ToDouble(PM25PollutantValue), 24);
                int? PM10Value = m_AQICalculateService.GetIAQI("a34002", Convert.ToDouble(PM10PollutantValue), 24);
                int? NO2Value = m_AQICalculateService.GetIAQI("a21004", Convert.ToDouble(NO2PollutantValue), 24);
                int? SO2Value = m_AQICalculateService.GetIAQI("a21026", Convert.ToDouble(SO2PollutantValue), 24);
                int? COValue = m_AQICalculateService.GetIAQI("a21005", Convert.ToDouble(COPollutantValue), 24);
                int? Max8HourO3Value = m_AQICalculateService.GetIAQI("a05024", Convert.ToDouble(Max8HourO3PollutantValue), 8);
                string AQIValue = m_AQICalculateService.GetAQI_Max_CNV(PM25Value, PM10Value, NO2Value, SO2Value, COValue, Max8HourO3Value, "V");
                

                DataRow dr = dtForAQI.NewRow();
                dr["MonitoringRegionUid"] = name;
                dr["a34004"] = PM25PollutantValue.ToString();
                dr["a34002"] = PM10PollutantValue.ToString();
                dr["a21004"] = NO2PollutantValue.ToString();
                dr["a21026"] = SO2PollutantValue.ToString();
                dr["a21005"] = COPollutantValue.ToString();
                dr["a05024"] = Max8HourO3PollutantValue.ToString();
                dr["Max_AQI"] = AQIValue;
                dtForAQI.Rows.Add(dr);
            }
            //}
            //dataView = dtForAQI.AsDataView();
            DataTable dt = dtForAQI;

            string quanshi = "";
            foreach (string name in names)
            {

                string PointName = "";
                decimal CurrentFactorCon = -1000;
                decimal SameFactorCon = -1000;
                decimal BaseFactorCon = -1000;
                decimal PM25CurrentFactorCon = -1000;
                decimal PM25SameFactorCon = -1000;
                decimal PM25BaseFactorCon = -1000;
                decimal PM10CurrentFactorCon = -1000;
                decimal PM10SameFactorCon = -1000;
                decimal PM10BaseFactorCon = -1000;
                decimal SO2CurrentFactorCon = -1000;
                decimal SO2SameFactorCon = -1000;
                decimal SO2BaseFactorCon = -1000;
                decimal NO2CurrentFactorCon = -1000;
                decimal NO2SameFactorCon = -1000;
                decimal NO2BaseFactorCon = -1000;
                decimal COCurrentFactorCon = -1000;
                decimal COSameFactorCon = -1000;
                decimal COBaseFactorCon = -1000;
                decimal O3CurrentFactorCon = -1000;
                decimal O3SameFactorCon = -1000;
                decimal O3BaseFactorCon = -1000;

                DataRow newRow = newdtb.NewRow();
                PointName = name;


                Rowdt = dt.Select("MonitoringRegionUid='" + name + "'");   //2015

                if (Rowdt.Length > 0)
                {
                    if (COnAQI == "1")
                    {
                        foreach (string factor in factorCodes)
                        {
                            if (Rowdt[0][factor].IsNotNullOrDBNull() && Rowdt[0][factor].ToString()!="")
                            {
                                switch (factor)
                                {
                                    case "a34004":
                                        PM25CurrentFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(Rowdt[0][factor]) * 1000, 0);
                                        break;
                                    case "a34002":
                                        PM10CurrentFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(Rowdt[0][factor]) * 1000, 0);
                                        break;
                                    case "a21004":
                                        NO2CurrentFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(Rowdt[0][factor]) * 1000, 0);
                                        break;
                                    case "a21026":
                                        SO2CurrentFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(Rowdt[0][factor]) * 1000, 0);
                                        break;
                                    case "a21005":
                                        COCurrentFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(Rowdt[0][factor]), 1);
                                        break;
                                    case "a05024":
                                        O3CurrentFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(Rowdt[0][factor]) * 1000, 0);
                                        break;
                                }
                            }

                        }
                    }
                    else
                    {
                        for (int j = 1; j < 7; j++)
                        {
                            string factors = dt.Columns[j].ColumnName;
                            int count = 24;
                            if (factors == "a05024")
                            {
                                count = 8;
                            }
                            if (Rowdt[0][j].IsNotNullOrDBNull() && Rowdt[0][j].ToString()!="")
                            {
                                decimal Con = DecimalExtension.GetRoundValue(Convert.ToDecimal(Rowdt[0][j]), 4);
                                decimal temp = DecimalExtension.GetRoundValue(s_AQIService.GetAQI(factors, Con, count), 0);
                                if (CurrentFactorCon < temp)
                                {
                                    CurrentFactorCon = temp;
                                }
                            }
                        }
                    }
                }

                for (int j = 0; j < years.Length; j++)
                {
                    DataRow[] RowdtN;
                    if (years[j] != "" && Convert.ToInt32(years[j]) != mBegion.Year)
                    {
                        DateTime smBegion = Convert.ToDateTime(years[j] + "-" + monthB);   //基数第一天
                        DateTime smEnd = DateTime.Parse((years[j].ToString() + "-" + monthE).ToString());    //基数当天
                        //  同期
                        foreach (string nameT in names)
                        {
                            List<string> list = new List<string>();
                            string[] ids = { };
                            DataRow[] drs = dvRegion.ToTable().Select("Region='" + name + "'").ToArray<DataRow>();
                            for (int k = 0; k < drs.Length; k++)
                            {
                                list.Add(drs[k]["PortId"].ToString());
                            }
                            ids = list.ToArray();

                            decimal? PM25PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34004", smBegion, smEnd, 24, "2");
                            decimal? PM10PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34002", smBegion, smEnd, 24, "2");
                            decimal? NO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21004", smBegion, smEnd, 24, "2");
                            decimal? SO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21026", smBegion, smEnd, 24, "2");
                            decimal? COPollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21005", smBegion, smEnd, 24, "2");
                            decimal? Max8HourO3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05024", smBegion, smEnd, 8, "2");
                            int? PM25Value = m_AQICalculateService.GetIAQI("a34004", Convert.ToDouble(PM25PollutantValue), 24);
                            int? PM10Value = m_AQICalculateService.GetIAQI("a34002", Convert.ToDouble(PM10PollutantValue), 24);
                            int? NO2Value = m_AQICalculateService.GetIAQI("a21004", Convert.ToDouble(NO2PollutantValue), 24);
                            int? SO2Value = m_AQICalculateService.GetIAQI("a21026", Convert.ToDouble(SO2PollutantValue), 24);
                            int? COValue = m_AQICalculateService.GetIAQI("a21005", Convert.ToDouble(COPollutantValue), 24);
                            int? Max8HourO3Value = m_AQICalculateService.GetIAQI("a05024", Convert.ToDouble(Max8HourO3PollutantValue), 8);
                            string AQIValue = m_AQICalculateService.GetAQI_Max_CNV(PM25Value, PM10Value, NO2Value, SO2Value, COValue, Max8HourO3Value, "V");


                            DataRow dr = dtForAQIT.NewRow();
                            dr["MonitoringRegionUid"] = name;
                            dr["a34004"] = PM25PollutantValue.ToString();
                            dr["a34002"] = PM10PollutantValue.ToString();
                            dr["a21004"] = NO2PollutantValue.ToString();
                            dr["a21026"] = SO2PollutantValue.ToString();
                            dr["a21005"] = COPollutantValue.ToString();
                            dr["a05024"] = Max8HourO3PollutantValue.ToString();
                            dr["Max_AQI"] = AQIValue;
                            dtForAQIT.Rows.Add(dr);
                        }
                        //dvT = regionDayAQI.GetAreaDataPager(regionUids, smBegion, smEnd, pageSize, pageNo, out recordTotal);  //  同期
                        DataTable dtN = dtForAQIT;   //同期 

                        RowdtN = dtN.Select("MonitoringRegionUid='" + name + "'");   //2014
                        if (RowdtN.Length > 0)
                        {
                            if (COnAQI == "1")
                            {
                                foreach (string factor in factorCodes)
                                {
                                    if (RowdtN[0][factor].IsNotNullOrDBNull() && RowdtN[0][factor].ToString()!="")
                                    {
                                        switch (factor)
                                        {
                                            case "a34004":
                                                PM25SameFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(RowdtN[0][factor]) * 1000, 0);
                                                break;
                                            case "a34002":
                                                PM10SameFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(RowdtN[0][factor]) * 1000, 0);
                                                break;
                                            case "a21004":
                                                NO2SameFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(RowdtN[0][factor]) * 1000, 0);
                                                break;
                                            case "a21026":
                                                SO2SameFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(RowdtN[0][factor]) * 1000, 0);
                                                break;
                                            case "a21005":
                                                COSameFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(RowdtN[0][factor]), 1);
                                                break;
                                            case "a05024":
                                                O3SameFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(RowdtN[0][factor]) * 1000, 0);
                                                break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                for (int n = 1; n < 7; n++)
                                {
                                    string factors = dtN.Columns[n].ColumnName;
                                    int count = 24;
                                    if (factors == "a05024")
                                    {
                                        count = 8;
                                    }
                                    if (RowdtN[0][n].IsNotNullOrDBNull() && RowdtN[0][n].ToString()!="")
                                    {
                                        decimal Con = DecimalExtension.GetRoundValue(Convert.ToDecimal(RowdtN[0][n]), 4);
                                        decimal temp = DecimalExtension.GetRoundValue(s_AQIService.GetAQI(factors, Con, count), 0);
                                        if (SameFactorCon < temp)
                                        {
                                            SameFactorCon = temp;
                                        }
                                    }
                                }
                            }
                        }

                        if (RowdtN.Length == 0)
                            ;
                        else
                        {

                            string str = years[j] + "年" + mBegion.Month.ToString() + "月" + mBegion.Day.ToString() + "日" + "~" + mEnd.Month.ToString() + "月" + mEnd.Day.ToString() + "日";
                            string sr = "与" + years[j].ToString() + "年比较";
                            if (COnAQI == "1")
                            {
                                foreach (string factor in factorCodes)
                                {
                                    switch (factor)
                                    {
                                        case "a34004":
                                            if (PM25SameFactorCon != -1000)
                                                newRow[factor + str] = PM25SameFactorCon.ToString();

                                            if (PM25SameFactorCon != 0 && PM25SameFactorCon != -1000 && PM25CurrentFactorCon != -1000)
                                            {
                                                newRow[factor + sr] = DecimalExtension.GetRoundValue((PM25CurrentFactorCon - PM25SameFactorCon) / PM25SameFactorCon * 100, 1) + "%";
                                            }
                                            break;
                                        case "a34002":
                                            if (PM10SameFactorCon != -1000)
                                                newRow[factor + str] = PM10SameFactorCon.ToString();

                                            if (PM10SameFactorCon != 0 && PM10SameFactorCon != -1000 && PM10CurrentFactorCon != -1000)
                                            {
                                                newRow[factor + sr] = DecimalExtension.GetRoundValue((PM10CurrentFactorCon - PM10SameFactorCon) / PM10SameFactorCon * 100, 1) + "%";
                                            }
                                            break;
                                        case "a21004":
                                            if (NO2SameFactorCon != -1000)
                                                newRow[factor + str] = NO2SameFactorCon.ToString();

                                            if (NO2SameFactorCon != 0 && NO2SameFactorCon != -1000 && NO2CurrentFactorCon != -1000)
                                            {
                                                newRow[factor + sr] = DecimalExtension.GetRoundValue((NO2CurrentFactorCon - NO2SameFactorCon) / NO2SameFactorCon * 100, 1) + "%";
                                            }
                                            break;
                                        case "a21026":
                                            if (SO2SameFactorCon != -1000)
                                                newRow[factor + str] = SO2SameFactorCon.ToString();

                                            if (SO2SameFactorCon != 0 && SO2SameFactorCon != -1000 && SO2CurrentFactorCon != -1000)
                                            {
                                                newRow[factor + sr] = DecimalExtension.GetRoundValue((SO2CurrentFactorCon - SO2SameFactorCon) / SO2SameFactorCon * 100, 1) + "%";
                                            }
                                            break;
                                        case "a21005":
                                            if (COSameFactorCon != -1000)
                                                newRow[factor + str] = COSameFactorCon.ToString();

                                            if (COSameFactorCon != 0 && COSameFactorCon != -1000 && COCurrentFactorCon != -1000)
                                            {
                                                newRow[factor + sr] = DecimalExtension.GetRoundValue((COCurrentFactorCon - COSameFactorCon) / COSameFactorCon * 100, 1) + "%";
                                            }
                                            break;
                                        case "a05024":
                                            if (O3SameFactorCon != -1000)
                                                newRow[factor + str] = O3SameFactorCon.ToString();

                                            if (O3SameFactorCon != 0 && O3SameFactorCon != -1000 && O3CurrentFactorCon != -1000)
                                            {
                                                newRow[factor + sr] = DecimalExtension.GetRoundValue((O3CurrentFactorCon - O3SameFactorCon) / O3SameFactorCon * 100, 1) + "%";
                                            }
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (SameFactorCon != -1000)
                                    newRow[str] = SameFactorCon.ToString();
                                if (SameFactorCon != 0 && SameFactorCon != -1000 && CurrentFactorCon != -1000)
                                {
                                    newRow[sr] = DecimalExtension.GetRoundValue((CurrentFactorCon - SameFactorCon) / SameFactorCon * 100, 1) + "%";
                                }
                            }
                        }
                    }
                }
                for (int j = 0; j < year.Length; j++)
                {
                    decimal Base = 0;
                    if (year[j] != "")
                    {
                        DataRow[] RowdtNew;
                        //基数
                        //  同期
                        foreach (string nameT in names)
                        {
                            List<string> list = new List<string>();
                            string[] ids = { };
                            DataRow[] drs = dvRegion.ToTable().Select("Region='" + name + "'").ToArray<DataRow>();
                            for (int k = 0; k < drs.Length; k++)
                            {
                                list.Add(drs[k]["PortId"].ToString());
                            }
                            ids = list.ToArray();

                            decimal? PM25PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34004", dtStart, dtEnd, 24, "2");
                            decimal? PM10PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a34002", dtStart, dtEnd, 24, "2");
                            decimal? NO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21004", dtStart, dtEnd, 24, "2");
                            decimal? SO2PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21026", dtStart, dtEnd, 24, "2");
                            decimal? COPollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a21005", dtStart, dtEnd, 24, "2");
                            decimal? Max8HourO3PollutantValue = m_AQICalculateService.GetRegionValueByTime(ids, "a05024", dtStart, dtEnd, 8, "2");
                            int? PM25Value = m_AQICalculateService.GetIAQI("a34004", Convert.ToDouble(PM25PollutantValue), 24);
                            int? PM10Value = m_AQICalculateService.GetIAQI("a34002", Convert.ToDouble(PM10PollutantValue), 24);
                            int? NO2Value = m_AQICalculateService.GetIAQI("a21004", Convert.ToDouble(NO2PollutantValue), 24);
                            int? SO2Value = m_AQICalculateService.GetIAQI("a21026", Convert.ToDouble(SO2PollutantValue), 24);
                            int? COValue = m_AQICalculateService.GetIAQI("a21005", Convert.ToDouble(COPollutantValue), 24);
                            int? Max8HourO3Value = m_AQICalculateService.GetIAQI("a05024", Convert.ToDouble(Max8HourO3PollutantValue), 8);
                            string AQIValue = m_AQICalculateService.GetAQI_Max_CNV(PM25Value, PM10Value, NO2Value, SO2Value, COValue, Max8HourO3Value, "V");


                            DataRow dr = dtForAQIS.NewRow();
                            dr["MonitoringRegionUid"] = name;
                            dr["a34004"] = PM25PollutantValue.ToString();
                            dr["a34002"] = PM10PollutantValue.ToString();
                            dr["a21004"] = NO2PollutantValue.ToString();
                            dr["a21026"] = SO2PollutantValue.ToString();
                            dr["a21005"] = COPollutantValue.ToString();
                            dr["a05024"] = Max8HourO3PollutantValue.ToString();
                            dr["Max_AQI"] = AQIValue;
                            dtForAQIT.Rows.Add(dr);
                        }
                        //dvT = regionDayAQI.GetAreaDataPager(regionUids, smBegion, smEnd, pageSize, pageNo, out recordTotal);  //  同期
                        //DataTable dtN = dtForAQIT;   //同期 

                        //RowdtN = dtN.Select("MonitoringRegionUid='" + name + "'");   //2014
                        //dvN = m_DataQueryByDayService.GetRegionConcentrationDay(regionUids, dtStart, dtEnd, year[j]);   //  基数
                        DataTable dtNew = dtForAQIS;  //基数
                        RowdtNew = dtNew.Select("MonitoringRegionUid='" + name + "'");   //2013
                        if (RowdtNew.Length > 0)
                        {
                            if (COnAQI == "1")
                            {
                                foreach (string factor in factorCodes)
                                {
                                    if (RowdtNew[0][factor].IsNotNullOrDBNull() && RowdtNew[0][factor].ToString()!="")
                                    {
                                        switch (factor)
                                        {
                                            case "a34004":
                                                PM25BaseFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(RowdtNew[0][factor]) * 1000, 0);
                                                break;
                                            case "a34002":
                                                PM10BaseFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(RowdtNew[0][factor]) * 1000, 0);
                                                break;
                                            case "a21004":
                                                NO2BaseFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(RowdtNew[0][factor]) * 1000, 0);
                                                break;
                                            case "a21026":
                                                SO2BaseFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(RowdtNew[0][factor]) * 1000, 0);
                                                break;
                                            case "a21005":
                                                COBaseFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(RowdtNew[0][factor]), 1);
                                                break;
                                            case "a05024":
                                                O3BaseFactorCon = DecimalExtension.GetRoundValue(Convert.ToDecimal(RowdtNew[0][factor]) * 1000, 0);
                                                break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                for (int m = 1; m < 7; m++)
                                {
                                    string factors = dtNew.Columns[m].ColumnName;
                                    int count = 24;
                                    if (factors == "a05024")
                                    {
                                        count = 8;
                                    }
                                    if (RowdtNew[0][m].IsNotNullOrDBNull() && RowdtNew[0][m].ToString()!="")
                                    {
                                        decimal Con = DecimalExtension.GetRoundValue(Convert.ToDecimal(RowdtNew[0][m]), 4);
                                        decimal temp = DecimalExtension.GetRoundValue(s_AQIService.GetAQI(factors, Con, count), 0);
                                        if (BaseFactorCon < temp)
                                        {
                                            BaseFactorCon = temp;
                                        }
                                    }
                                }
                            }
                        }
                        if (RowdtNew.Length == 0)
                            ;
                        else
                        {
                            if (COnAQI == "1")
                            {
                                foreach (string factor in factorCodes)
                                {
                                    switch (factor)
                                    {
                                        case "a34004":
                                            if (PM25BaseFactorCon != -1000)
                                                newRow[factor + year[j] + "考核基数"] = PM25BaseFactorCon.ToString();
                                            if (PM25BaseFactorCon != 0 && PM25BaseFactorCon != -1000 && PM25CurrentFactorCon != -1000)
                                            {
                                                newRow[factor + "与" + year[j] + "考核基数比较"] = DecimalExtension.GetRoundValue((PM25CurrentFactorCon - PM25BaseFactorCon) / PM25BaseFactorCon * 100, 1) + "%";
                                            }
                                            break;
                                        case "a34002":
                                            if (PM10BaseFactorCon != -1000)
                                                newRow[factor + year[j] + "考核基数"] = PM10BaseFactorCon.ToString();
                                            if (PM10BaseFactorCon != 0 && PM10BaseFactorCon != -1000 && PM10CurrentFactorCon != -1000)
                                            {
                                                newRow[factor + "与" + year[j] + "考核基数比较"] = DecimalExtension.GetRoundValue((PM10CurrentFactorCon - PM10BaseFactorCon) / PM10BaseFactorCon * 100, 1) + "%";
                                            }
                                            break;
                                        case "a21004":
                                            if (NO2BaseFactorCon != -1000)
                                                newRow[factor + year[j] + "考核基数"] = NO2BaseFactorCon.ToString();
                                            if (NO2BaseFactorCon != 0 && NO2BaseFactorCon != -1000 && NO2CurrentFactorCon != -1000)
                                            {
                                                newRow[factor + "与" + year[j] + "考核基数比较"] = DecimalExtension.GetRoundValue((NO2CurrentFactorCon - NO2BaseFactorCon) / NO2BaseFactorCon * 100, 1) + "%";
                                            }
                                            break;
                                        case "a21026":
                                            if (SO2BaseFactorCon != -1000)
                                                newRow[factor + year[j] + "考核基数"] = SO2BaseFactorCon.ToString();
                                            if (SO2BaseFactorCon != 0 && SO2BaseFactorCon != -1000 && SO2CurrentFactorCon != -1000)
                                            {
                                                newRow[factor + "与" + year[j] + "考核基数比较"] = DecimalExtension.GetRoundValue((SO2CurrentFactorCon - SO2BaseFactorCon) / SO2BaseFactorCon * 100, 1) + "%";
                                            }
                                            break;
                                        case "a21005":
                                            if (COBaseFactorCon != -1000)
                                                newRow[factor + year[j] + "考核基数"] = COBaseFactorCon.ToString();
                                            if (COBaseFactorCon != 0 && COBaseFactorCon != -1000 && COCurrentFactorCon != -1000)
                                            {
                                                newRow[factor + "与" + year[j] + "考核基数比较"] = DecimalExtension.GetRoundValue((COCurrentFactorCon - COBaseFactorCon) / COBaseFactorCon * 100, 1) + "%";
                                            }
                                            break;
                                        case "a05024":
                                            if (O3BaseFactorCon != -1000)
                                                newRow[factor + year[j] + "考核基数"] = O3BaseFactorCon.ToString();
                                            if (O3BaseFactorCon != 0 && O3BaseFactorCon != -1000 && O3CurrentFactorCon != -1000)
                                            {
                                                newRow[factor + "与" + year[j] + "考核基数比较"] = DecimalExtension.GetRoundValue((O3CurrentFactorCon - O3BaseFactorCon) / O3BaseFactorCon * 100, 1) + "%";
                                            }
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (BaseFactorCon != -1000)
                                    newRow[year[j] + "考核基数"] = BaseFactorCon.ToString();
                                if (BaseFactorCon != 0 && BaseFactorCon != -1000 && CurrentFactorCon != -1000)
                                {
                                    newRow["与" + year[j] + "考核基数比较"] = DecimalExtension.GetRoundValue((CurrentFactorCon - BaseFactorCon) / BaseFactorCon * 100, 1) + "%";
                                }
                            }

                        }
                    }
                }
                if (Rowdt.Length == 0)
                {
                    newRow["PointName"] = PointName;
                    newdtb.Rows.Add(newRow);
                }
                else
                {
                    newRow["PointName"] = PointName;
                    if (COnAQI == "1")
                    {
                        foreach (string factor in factorCodes)
                        {
                            switch (factor)
                            {
                                case "a34004":
                                    if (PM25CurrentFactorCon != -1000)
                                        newRow[factor + cf] = PM25CurrentFactorCon.ToString();
                                    break;
                                case "a34002":
                                    if (PM10CurrentFactorCon != -1000)
                                        newRow[factor + cf] = PM10CurrentFactorCon.ToString();
                                    break;
                                case "a21004":
                                    if (NO2CurrentFactorCon != -1000)
                                        newRow[factor + cf] = NO2CurrentFactorCon.ToString();
                                    break;
                                case "a21026":
                                    if (SO2CurrentFactorCon != -1000)
                                        newRow[factor + cf] = SO2CurrentFactorCon.ToString();
                                    break;
                                case "a21005":
                                    if (COCurrentFactorCon != -1000)
                                        newRow[factor + cf] = COCurrentFactorCon.ToString();
                                    break;
                                case "a05024":
                                    if (O3CurrentFactorCon != -1000)
                                        newRow[factor + cf] = O3CurrentFactorCon.ToString();
                                    break;
                            }
                        }
                    }
                    else
                        newRow[cf] = CurrentFactorCon.ToString();
                    newdtb.Rows.Add(newRow);
                }

            }
            DataView newdv = new DataView();
            newdv = new DataView(newdtb);
            return newdv;
        }

        /// <summary>
        /// 获取指定日期内日数据均值数据
        /// </summary>
        /// <param name="regionGuids">区域Guid</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>
        /// 返回字段：
        /// MonitoringRegionUid：区域ID
        /// PM25：PM25浓度
        /// PM10：PM10浓度
        /// NO2：NO2浓度
        /// SO2：SO2浓度
        /// CO：CO浓度
        /// MaxOneHourO3：MaxOneHourO3浓度
        /// Max8HourO3：Max8HourO3浓度
        /// </returns>
        public DataView GetRegionsAvgValue(string[] regionGuids, DateTime dateStart, DateTime dateEnd)
        {
            regionDayAQI = Singleton<RegionDayAQIRepository>.GetInstance();
            if (regionDayAQI != null)
                return regionDayAQI.GetAvgValue(regionGuids, dateStart, dateEnd);
            return null;
        }
        #endregion

        #region 接口实现
        /// <summary>
        /// 获取某段时间内某一监测点AQI
        /// </summary>
        /// <returns></returns>
        public int GetTimePortAQI()
        {
            return 0;
        }

        /// <summary>
        /// 获取某段时间内苏州市区所有监测点AQI
        /// </summary>
        /// <returns></returns>
        public int GetTimeSzPortsAQI()
        {
            return 0;
        }

        /// <summary>
        /// 获取某段时间内某一城区所有监测点AQI
        /// </summary>
        /// <returns></returns>
        public int GetTimeAreaPortsAQI()
        {
            return 0;
        }

        /// <summary>
        /// 获取某段时间内某一监测点AQI污染等级统计
        /// </summary>
        /// <returns></returns>
        public int GetTimePortAQIGrade()
        {
            return 0;
        }

        /// <summary>
        /// 获取某段时间内苏州市区所有监测点AQI污染等级统计
        /// </summary>
        /// <returns></returns>
        public int GetTimeSzPortsAQIGrade()
        {
            return 0;
        }

        /// <summary>
        /// 获取某段时间内某一城区所有监测点AQI污染等级统计
        /// </summary>
        /// <returns></returns>
        public int GetTimeAreaPortsAQIGrade()
        {
            return 0;
        }

        /// <summary>
        /// 获取某段时间内某一监测点首要污染物统计
        /// </summary>
        /// <returns></returns>
        public int GetTimePortPrimaryPollutant()
        {
            return 0;
        }

        /// <summary>
        /// 获取某段时间内苏州市区所有监测点首要污染物统计
        /// </summary>
        /// <returns></returns>
        public int GetTimeSzPortsPrimaryPollutant()
        {
            return 0;
        }

        /// <summary>
        /// 获取某段时间内某一城区所有监测点首要污染物统计
        /// </summary>
        /// <returns></returns>
        public int GetTimeAreaPortsPrimaryPollutant()
        {
            return 0;
        }

        /// <summary>
        /// 获取某段时间内某一监测点超标天数统计
        /// </summary>
        /// <returns></returns>
        public int GetTimePortOutDay()
        {
            return 0;
        }

        /// <summary>
        /// 获取某段时间内苏州市区所有监测点超标天数统计
        /// </summary>
        /// <returns></returns>
        public int GetTimeSzPortsOutDay()
        {
            return 0;
        }

        /// <summary>
        /// 获取某段时间内某一城区所有监测点超标天数统计
        /// </summary>
        /// <returns></returns>
        public int GetTimeAreaPortsOutDay()
        {
            return 0;
        }
        #endregion
        /// <summary>
        /// DataTable转换为一维字符串数组
        /// </summary>
        /// <returns></returns>
        public static string[] dtToArr(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return new string[0];
            }
            else
            {
                string[] sr = new string[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.IsDBNull(dt.Rows[i][0]))
                    {
                        sr[i] = "";
                    }
                    else
                    {
                        sr[i] = dt.Rows[i][0] + "";
                    }
                }
                return sr;
            }
        }
    }
}
