﻿using log4net;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.Service.AutoMonitoring.Air;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.DataAnalyze.Air.DataQuery;
using SmartEP.Utilities.AdoData;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Utilities.Web.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartEP.WebUI.Pages.EnvAir.Chart
{
    public partial class OzonePrecursorChart : System.Web.UI.Page
    {
        /// <summary>
        /// 名称：OzonePrecursorChart.aspx
        /// 创建人：刘晋
        /// 创建日期：2017-06-12
        /// 维护人员：
        /// 最新维护人员：
        /// 最新维护日期：
        /// 功能摘要：
        /// TVOCs页面可下钻饼图
        /// 版权所有(C)：江苏远大信息股份有限公司
        /// </summary>
        //获取一个日志记录器
        ILog log = LogManager.GetLogger("FileLogging");
        /// <summary>
        /// 数据处理服务
        /// </summary>
        InfectantBy1Service m_Min1Data = Singleton<InfectantBy1Service>.GetInstance();
        InfectantBy5Service m_Min5Data = Singleton<InfectantBy5Service>.GetInstance();
        InfectantBy60Service m_Min60Data = Singleton<InfectantBy60Service>.GetInstance();
        InfectantByDayService m_DayOriData = Singleton<InfectantByDayService>.GetInstance();
        InfectantByMonthService m_MonthOriData = Singleton<InfectantByMonthService>.GetInstance();
        DataQueryByHourService m_HourData = Singleton<DataQueryByHourService>.GetInstance();
        DataQueryByDayService m_DayData = Singleton<DataQueryByDayService>.GetInstance();
        DataQueryByWeekService m_WeekData = Singleton<DataQueryByWeekService>.GetInstance();
        DataQueryByMonthService m_MonthData = Singleton<DataQueryByMonthService>.GetInstance();
        DataQueryBySeasonService m_SeasonData = Singleton<DataQueryBySeasonService>.GetInstance();
        DataQueryByYearService m_YearData = Singleton<DataQueryByYearService>.GetInstance();
        InstrumentChannelService m_InstrumentChannelService = Singleton<InstrumentChannelService>.GetInstance();
        MonitoringPointAirService monitoringPointAir = new MonitoringPointAirService();
        /// <summary>
        /// 数据库处理类
        /// </summary>
        DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();
        //每页显示数据个数            
        int pageSize = int.MaxValue;
        //当前页的序号
        int pageNo = 0;
        int recordTotal = 1;
        DataTable dt = null;
        DataTable dt1 = null;
        DataTable dt2 = null;
        decimal? all = 0;
        decimal? count = 0;
        decimal? del = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bind();
            }
        }
        /// <summary>
        /// 数据处理方法
        /// </summary>
        public void bind()
        {
            var auditData = new DataView();
            try 
            {
                string name = string.Empty;
                string TypeNames = PageHelper.GetQueryString("TypeNames");
                string[] typeName = TypeNames.TrimEnd(',').Split(',');
                string Points = PageHelper.GetQueryString("Points");
                string[] points = Points.TrimEnd(',').Split(',');
                string Begion = PageHelper.GetQueryString("Begion");
                string End = PageHelper.GetQueryString("End");
                string OrderBy = PageHelper.GetQueryString("OrderBy");
                string DataType = PageHelper.GetQueryString("DataType");
                string TimeType = PageHelper.GetQueryString("TimeType");

                string BrandNum = "Browser Version	Total Market Share \n ";
                string sql = string.Empty;
                if (TimeType == "Min60s")
                {
                    sql = string.Format(@"select PollutantCode from [dbo].[DT_VOC3Type]
                                        where VOC1TypeGuid IN (SELECT RowGuid from [dbo].[DT_VOC1Type] where VOC1Type = '{0}') and PollutantCode in (SELECT  PollutantCode FROM [Standard].[TB_PollutantCode] where VOCType!='2') order by VOC1TypeGuid"
                , typeName[0]);
                }
                else
                {
                    sql = string.Format(@"select PollutantCode from [dbo].[DT_VOC3Type]
                                        where VOC1TypeGuid IN (SELECT RowGuid from [dbo].[DT_VOC1Type] where VOC1Type = '{0}') order by VOC1TypeGuid"
                , typeName[0]);
                }
//                string sql = string.Format(@"select PollutantCode from [dbo].[DT_VOC3Type]
//                                        where VOC1TypeGuid IN (SELECT RowGuid from [dbo].[DT_VOC1Type] where VOC1Type = '{0}') order by VOC1TypeGuid"
//                , typeName[0]);
                dt = g_DatabaseHelper.ExecuteDataTable(sql, "AMS_BaseDataConnection");
                string[] factorCodes = dtToArr(dt);
                string sql1 = string.Format(@"select VOC2Type
                                        from [dbo].[DT_VOC2Type]
                                        where RowGuid in(
                                        select VOC2TypeGuid from [dbo].[DT_VOC3Type]
                                        where VOC1TypeGuid IN 
                                        (SELECT RowGuid from [dbo].[DT_VOC1Type] where VOC1Type in ('{0}')))", typeName[0]);
                dt1 = g_DatabaseHelper.ExecuteDataTable(sql1, "AMS_BaseDataConnection");
                string[] Type = dtToArr(dt1);
                if (DataType == "AuditData")
                {
                    if (TimeType == "Hour")
                    {
                        auditData = m_HourData.GetHourDataPagerNew(points, factorCodes, Convert.ToDateTime(Begion), Convert.ToDateTime(End), pageSize, pageNo, out recordTotal, OrderBy);//小时类型按 小时数据查询
                    }
                    if (TimeType == "Hourskqy")
                    {
                        auditData = m_HourData.GetVOCsKQYHourDataPagerNew(points, factorCodes, Convert.ToDateTime(Begion), Convert.ToDateTime(End), pageSize, pageNo, out recordTotal, OrderBy);//小时类型按 小时数据查询
                    }
                    else if (TimeType == "Day")
                    {
                        auditData = m_DayData.GetDayData(points, factorCodes, Convert.ToDateTime(Begion), Convert.ToDateTime(End), pageSize, pageNo, out recordTotal, OrderBy);//日类型按 日数据查询
                    }
                    else if (TimeType == "Week")
                    {
                        string [] begin=Begion.Split(',');
                        string[] end = End.Split(',');
                        int weekB = Convert.ToInt32(begin[0]);
                        int weekF = Convert.ToInt32(begin[1]);
                        int weekE = Convert.ToInt32(end[0]);
                        int weekT = Convert.ToInt32(end[1]);
                        auditData = m_WeekData.GetWeekDataPager(points, factorCodes, weekB, weekF, weekE, weekT, pageSize, pageNo, out recordTotal, OrderBy);
                    }
                    else if (TimeType == "Month")
                    {
                        string[] begin = Begion.Split(',');
                        string[] end = End.Split(',');
                        int monthB = Convert.ToInt32(begin[0]);
                        int monthF = Convert.ToInt32(begin[1]);
                        int monthE = Convert.ToInt32(end[0]);
                        int monthT = Convert.ToInt32(end[1]);
                        auditData = m_MonthData.GetMonthDataPager(points, factorCodes, monthB, monthF, monthE, monthT, pageSize, pageNo, out recordTotal, OrderBy);
                    }
                    else if (TimeType == "Season")
                    {
                        string[] begin = Begion.Split(',');
                        string[] end = End.Split(',');
                        int seasonB = Convert.ToInt32(begin[0]);
                        int seasonF = Convert.ToInt32(begin[1]);
                        int seasonE = Convert.ToInt32(end[0]);
                        int seasonT = Convert.ToInt32(end[1]);
                        auditData = m_SeasonData.GetSeasonDataPager(points, factorCodes, seasonB, seasonF, seasonE, seasonT, pageSize, pageNo, out recordTotal, OrderBy);
                    }
                    else if (TimeType == "Year")
                    {
                        auditData = m_YearData.GetYearDataPager(points, factorCodes, Convert.ToInt32(Begion), Convert.ToInt32(End), pageSize, pageNo, out recordTotal, OrderBy); //年类型 按年数据查询
                    }
                }
                else if (DataType == "OriData")
                {
                    if (TimeType=="Min1")
                    {
                        auditData = m_Min1Data.GetDataPager(points, factorCodes, Convert.ToDateTime(Begion), Convert.ToDateTime(End), pageSize, pageNo, out recordTotal, OrderBy);//一分钟类型按 一分钟数据查询
                    }
                    else if (TimeType=="Min5")
                    {
                        auditData = m_Min5Data.GetDataPager(points, factorCodes, Convert.ToDateTime(Begion), Convert.ToDateTime(End), pageSize, pageNo, out recordTotal, OrderBy);//五分钟类型按 五分钟数据查询
                    }
                    else if (TimeType=="Min60")
                    {
                        auditData = m_Min60Data.GetDataPagerNew(points, factorCodes, Convert.ToDateTime(Begion), Convert.ToDateTime(End), pageSize, pageNo, out recordTotal, OrderBy);//60分钟类型按 60分钟数据查询
                    }
                    else if (TimeType == "Min60s")
                    {
                        auditData = m_Min60Data.GetVOCWDataPager(points, factorCodes, Convert.ToDateTime(Begion), Convert.ToDateTime(End), pageSize, pageNo, out recordTotal, OrderBy);//60分钟类型按 60分钟数据查询
                    }
                    else if (TimeType == "Min60kqy")
                    {
                        auditData = m_Min60Data.GetVOCsKQYDataPager(points, factorCodes, Convert.ToDateTime(Begion), Convert.ToDateTime(End), pageSize, pageNo, out recordTotal, OrderBy);//60分钟类型按 60分钟数据查询
                    }
                    else if (TimeType == "OriDay")
                    {
                        auditData = m_DayOriData.GetDataPagers(points, factorCodes, Convert.ToDateTime(Begion), Convert.ToDateTime(End), pageSize, pageNo, out recordTotal, OrderBy);//原始日数据类型按 原始
                    }
                    else if (TimeType == "OriMonth")
                    {
                        auditData = m_MonthOriData.GetOriDataPager(points, factorCodes, Convert.ToDateTime(Begion), Convert.ToDateTime(End), pageSize, pageNo, out recordTotal, OrderBy);
                    }
                }
                if (auditData.ToTable().Rows.Count > 0)
                {
                    foreach (string a in factorCodes)
                    {
                        decimal? b = auditData.ToTable().AsEnumerable().Select(t => t.Field<decimal?>(a)).Sum();
                        all += auditData.ToTable().AsEnumerable().Select(t => t.Field<decimal?>(a)).Sum();
                    }
                }
                if(all>0)
                {
                    if (TimeType == "Min60s")
                    {
                        for (int i = 0; i < Type.Length; i++)
                        {
                            string sql3 = string.Format(@"select PollutantCode
                                    from [dbo].[DT_VOC3Type]
                                    where VOC2TypeGuid=(
                                    select RowGuid from [dbo].[DT_VOC2Type] where VOC2Type='{0}')", Type[i]);
                            dt2 = g_DatabaseHelper.ExecuteDataTable(sql3, "AMS_BaseDataConnection");
                            string[] pointId = dtToArr(dt2);
                            for (int j = 0; j < pointId.Length; j++)
                            {
                                IPollutant factor = factorCbxRsm.GetFactors().FirstOrDefault(x => x.PollutantCode.Equals(pointId[j]));
                                if (factor != null)
                                {
                                    name = factor.PollutantName;
                                }
                                count = auditData.ToTable().AsEnumerable().Select(t => t.Field<decimal?>(pointId[j])).Sum();
                                del = count * 100 / all;
                                BrandNum += Type[i] + "[" + name + "]	" + del + "% \n ";
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < Type.Length; i++)
                        {
                            string sql3 = string.Format(@"select PollutantCode
                                    from [dbo].[DT_VOC3Type]
                                    where VOC2TypeGuid=(
                                    select RowGuid from [dbo].[DT_VOC2Type] where VOC2Type='{0}')", Type[i]);
                            dt2 = g_DatabaseHelper.ExecuteDataTable(sql3, "AMS_BaseDataConnection");
                            string[] pointId = dtToArr(dt2);
                            for (int j = 0; j < pointId.Length; j++)
                            {
                                IPollutant factor = factorCbxRsm.GetFactors().FirstOrDefault(x => x.PollutantCode.Equals(pointId[j]));
                                if (factor != null)
                                {
                                    name = factor.PollutantName;
                                }
                                count = auditData.ToTable().AsEnumerable().Select(t => t.Field<decimal?>(pointId[j])).Sum();
                                del = count * 100 / all;
                                BrandNum += Type[i] + "[" + name + "]	" + del + "% \n ";
                            }
                        }
                    }
                    
                }
                hdBrandData.Value = BrandNum;
                
            }
            catch(Exception e)
            {
                log.Error(e.ToString());
            }
            
        }
        
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