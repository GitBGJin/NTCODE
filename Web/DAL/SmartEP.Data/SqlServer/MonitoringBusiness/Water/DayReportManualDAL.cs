using SmartEP.Core.Generic;
using SmartEP.Data.Enums;
using SmartEP.Utilities.AdoData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.MonitoringBusiness.Water
{
    /// <summary>
    /// 名称：DayReportManualDAL.cs
    /// 创建人：朱佳伟
    /// 创建日期：2015-12-17
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 地表水发布：枯水期环境监测快报日数据处理
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class DayReportManualDAL
    {
        /// <summary>
        /// 数据库处理类
        /// </summary>
        DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();

        /// <summary>
        /// 存储过程处理类
        /// </summary>
        BaseDAHelper g_DBBiz = Singleton<BaseDAHelper>.GetInstance();

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.MonitoringBusiness);

        #region 取水口配置信息
        /// <summary>
        /// 取水口站点配置
        /// </summary>
        private string QSK_Point_CodeName = "取水口站点配置";
        /// <summary>
        /// 取水口监测因子配置
        /// </summary>
        private string QSK_Pollutant_CodeName = "取水口监测因子配置";
        /// <summary>
        /// 取水口评价监测因子配置
        /// </summary>
        private string QSK_AuditPollutant_CodeName = "取水口评价监测因子配置";
        #endregion

        #region 河流配置信息
        /// <summary>
        /// 河流站点配置
        /// </summary>
        private string HL_Point_CodeName = "河流站点配置";
        /// <summary>
        /// 河流监测因子配置
        /// </summary>
        private string HL_Pollutant_CodeName = "河流监测因子配置";
        /// <summary>
        /// 河流评价监测因子配置
        /// </summary>
        private string HL_AuditPollutant_CodeName = "河流评价监测因子配置";
        #endregion

        /// <summary>
        /// 获取取水口日数据监测结果
        /// </summary>
        /// <param name="PointUids">配置站点数组</param>
        /// <param name="PollutantCodes"></param>
        /// <param name="Date"></param>
        /// <returns></returns>
        public DataTable GetDataByQSK(List<string> PointUids, List<string> PollutantCodes, DateTime Date)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("");



            DataTable dt = new DataTable();
            return dt;
        }

        #region 生成人工点日数据评价水质等级
        /// <summary>
        /// 生成等级数据
        /// </summary>
        /// <param name="CalEQITypeUid">评价类型（湖泊、河流）</param>
        /// 河流：d8197909-568e-4319-874c-3ad7cbc92a7e
        /// 湖库：e82cd86f-71ba-4f87-8e5c-6ac7ca055a6b
        /// <param name="IEQI">评价水质类别</param>
        /// <param name="PointUids">站点列表，以“,”分割</param>
        /// <param name="PollutantCodes">评价因子,以“,”分割</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool GenerateData(string CalEQITypeUid, int IEQI, string PointUids, string PollutantCodes, DateTime dateStart, DateTime dateEnd, out String errMsg)
        {
            errMsg = string.Empty;
            dateStart = DateTime.Parse(dateStart.ToString("yyyy-MM-dd 00:00:00"));
            dateEnd = DateTime.Parse(dateEnd.ToString("yyyy-MM-dd 00:00:00"));
            try
            {
                if (PointUids == null || PointUids.Length == 0 || PollutantCodes == null || PollutantCodes.Length == 0)
                { return false; }
                g_DBBiz.ClearParameters();

                SqlParameter pramDateStart = new SqlParameter();
                pramDateStart = new SqlParameter();
                pramDateStart.SqlDbType = SqlDbType.DateTime;
                pramDateStart.ParameterName = "@m_begin";
                pramDateStart.Value = dateStart;
                g_DBBiz.SetProcedureParameters(pramDateStart);

                SqlParameter pramDateEnd = new SqlParameter();
                pramDateEnd = new SqlParameter();
                pramDateEnd.SqlDbType = SqlDbType.DateTime;
                pramDateEnd.ParameterName = "@m_end";
                pramDateEnd.Value = dateEnd;
                g_DBBiz.SetProcedureParameters(pramDateEnd);

                SqlParameter pramPointUids = new SqlParameter();
                pramPointUids = new SqlParameter();
                pramPointUids.SqlDbType = SqlDbType.NVarChar;
                pramPointUids.ParameterName = "@m_portlist";
                pramPointUids.Value = PointUids;
                g_DBBiz.SetProcedureParameters(pramPointUids);

                SqlParameter pramCalEQITypeUid = new SqlParameter();
                pramCalEQITypeUid = new SqlParameter();
                pramCalEQITypeUid.SqlDbType = SqlDbType.NVarChar;
                pramCalEQITypeUid.ParameterName = "@m_CalEQITypeUid";
                pramCalEQITypeUid.Value = CalEQITypeUid;
                g_DBBiz.SetProcedureParameters(pramCalEQITypeUid);

                SqlParameter pramIEQI = new SqlParameter();
                pramIEQI = new SqlParameter();
                pramIEQI.SqlDbType = SqlDbType.Int;
                pramIEQI.ParameterName = "@m_IEQI";
                pramIEQI.Value = IEQI;
                g_DBBiz.SetProcedureParameters(pramIEQI);

                SqlParameter pramPollutantCodes = new SqlParameter();
                pramPollutantCodes = new SqlParameter();
                pramPollutantCodes.SqlDbType = SqlDbType.NVarChar;
                pramPollutantCodes.ParameterName = "@m_EvaluateFactorList";
                pramPollutantCodes.Value = PollutantCodes;
                g_DBBiz.SetProcedureParameters(pramPollutantCodes);

                //执行存储过程
                g_DBBiz.ExecuteProcNonQuery("UP_WaterReport_Day_Port_Manual", connection);
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }
            return true;
        }
        #endregion

        #region
        /// <summary>
        /// 获取测点相关基础信息及相关因子日数据及等级
        /// </summary>
        /// <param name="PointUids">测点Uid数组</param>
        /// <param name="PollutantCodes">因子Code数组</param>
        /// <param name="StartDate">监测开始日期</param>
        /// <param name="EndDate">监测截止日期</param>
        /// <returns>DataTable</returns>
        public DataTable GetWaterIEQI(List<string> PointUids, List<string> PollutantCodes, DateTime StartDate, DateTime EndDate)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select water.Grade,MONTH(water.DateTime) as months,DAY(water.DateTime) as days,water.PointGuid,water.Description");
            strSql.Append(",item.ItemText as WatersName,item.ItemValue as PointName ");
            strSql.Append(",(select top 1 DateTime from dbo.TB_DrySeasonReport order by DateTime desc) as LastDate ");
            strSql.Append(",ISNULL((select top 1 Times from dbo.TB_DrySeasonReport order by DateTime desc),'0') as LastTimes ");
            strSql.Append(",ISNULL((select top 1 TotalTimes from dbo.TB_DrySeasonReport order by DateTime desc),'0') as LastTotalTimes ");
            if (PollutantCodes != null)
            {
                foreach (string code in PollutantCodes)
                {
                    strSql.Append(",(select top 1 PollutantValue from WaterReport.TB_DayReport_Manual ");
                    strSql.Append("where PollutantCode='" + code + "' ");
                    strSql.Append("and PointGuid=water.PointGuid and DateTime=water.DateTime) as '" + code + "' ");
                }
            }
            strSql.Append("from WaterReport.TB_DayReport_Manual water ");
            strSql.Append("inner join dbo.SY_View_CodeMainItem item on water.PointGuid=item.ItemGuid ");
            strSql.Append("where water.PollutantCode='WaterQuality' ");
            strSql.Append("and water.DateTime>=CONVERT(DATETIME,'" + StartDate + "') and water.DateTime<=CONVERT(DATETIME,'" + EndDate + "') ");
            if (PointUids.Count > 0)
            {
                if (PointUids.Count == 1)
                {
                    strSql.Append("and water.PointGuid='" + PointUids[0] + "' ");
                }
                else
                {
                    strSql.Append("and (");
                    for (int i = 0; i < PointUids.Count; i++)
                    {
                        if (i == 0)
                        {
                            strSql.Append(" water.PointGuid='" + PointUids[i] + "' ");
                        }
                        else
                        {
                            strSql.Append(" or water.PointGuid='" + PointUids[i] + "' ");
                        }
                    }
                    strSql.Append(")");
                }
            }
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), connection);
        }
        #endregion
    }
}
