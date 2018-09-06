using SmartEP.Core.Generic;
using SmartEP.Utilities.AdoData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.MonitoringBusiness
{
    /// <summary>
    /// 名称：ReportSummaryDAL.cs
    /// 创建人：樊垂贺
    /// 创建日期：2016-7-10
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 环境空气发布：月报汇总
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class ReportSummaryDAL
    {
        /// <summary>
        /// 数据库表名
        /// </summary>
        private string tableName = "dbo.TB_ReportSummary";
        private string tableName1 = "dbo.TB_ReportSummaryItem";
        /// <summary>
        /// 数据库处理类
        /// </summary>
        DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connection = "AMS_MonitoringBusinessConnection";
        #region 插入数据
        /// <summary>
        /// 插入月报汇总数据
        /// </summary>
        /// <param name="dv"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="pointIds"></param>
        /// <param name="pageType"></param>
        public void insertTable(DataView dv, int year, int month, string[] pointIds, string pageType)
        {
            List<CommandInfo> sqllist = new List<CommandInfo>();
            string pointid = "'" + string.Join("','", pointIds) + "'";
            StringBuilder strdaySql = new StringBuilder();
            strdaySql.Append("delete from ");
            strdaySql.Append(tableName);
            strdaySql.Append(" where Year=");
            strdaySql.Append(year);
            strdaySql.Append(" and ");
            strdaySql.Append("MonthOfYear=");
            strdaySql.Append(month);
            strdaySql.Append(" and PageType='");
            strdaySql.Append(pageType);
            strdaySql.Append("' and PointId in (");
            strdaySql.Append(pointid);
            strdaySql.Append(")");
            g_DatabaseHelper.ExecuteNonQuery(strdaySql.ToString(), connection);
            for (int i = 0; i < dv.Count; i++)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into ");
                strSql.Append(tableName);
                strSql.Append("(PageType,PointId,Tstamp,Year,MonthOfYear,PollutantCode,PollutantValue)");
                strSql.Append("values(@PageType,@PointId,@Tstamp,@Year,@MonthOfYear,@PollutantCode,@PollutantValue)");
                SqlParameter[] parameters = { 
                                new SqlParameter("@PageType", SqlDbType.NVarChar,50),
                                new SqlParameter("@PointId", SqlDbType.Int),
                                new SqlParameter("@Tstamp", SqlDbType.DateTime),
                                new SqlParameter("@Year", SqlDbType.Int),
                                new SqlParameter("@MonthOfYear", SqlDbType.Int),
                                new SqlParameter("@PollutantCode", SqlDbType.NVarChar,50),
                                new SqlParameter("@PollutantValue", SqlDbType.Decimal)};
                parameters[0].Value = pageType;
                parameters[1].Value = int.Parse(dv[i]["PointId"].ToString());
                parameters[2].Value = DateTime.Parse(dv[i]["Tstamp"].ToString());
                parameters[3].Value = int.Parse(dv[i]["Year"].ToString());
                parameters[4].Value = int.Parse(dv[i]["MonthOfYear"].ToString());
                parameters[5].Value = dv[i]["PollutantCode"].ToString();
                decimal pollutantvalue = 0;
                if (dv[i]["PollutantValue"] != null)
                {
                    pollutantvalue = decimal.Parse(dv[i]["PollutantValue"].ToString());
                    parameters[6].Value = dv[i]["PollutantValue"].ToString();
                }
                CommandInfo cmd = new CommandInfo(strSql.ToString(), parameters);
                sqllist.Add(cmd);
            }
            g_DatabaseHelper.ExecuteSqlTranWithIndentity(sqllist, connection);
        }
        /// <summary>
        /// 插入月报汇总数据
        /// </summary>
        /// <param name="dv"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="pointIds"></param>
        /// <param name="pageType"></param>
        public void insertItemTable(DataView dv, int year, int month, string pageType)
        {
            List<CommandInfo> sqllist = new List<CommandInfo>();
            StringBuilder strdaySql = new StringBuilder();
            strdaySql.Append("delete from ");
            strdaySql.Append(tableName1);
            strdaySql.Append(" where Year=");
            strdaySql.Append(year);
            strdaySql.Append(" and ");
            strdaySql.Append("MonthOfYear=");
            strdaySql.Append(month);
            strdaySql.Append(" and  PageType='");
            strdaySql.Append(pageType);
            strdaySql.Append("'");
            g_DatabaseHelper.ExecuteNonQuery(strdaySql.ToString(), connection);
            for (int i = 0; i < dv.Count; i++)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into ");
                strSql.Append(tableName1);
                strSql.Append("(PageType,Tstamp,Year,MonthOfYear,MonitorData,QualifiedData,QualifiedRate,Summary)");
                strSql.Append("values(@PageType,@Tstamp,@Year,@MonthOfYear,@MonitorData,@QualifiedData,@QualifiedRate,@Summary)");
                SqlParameter[] parameters = { 
                                new SqlParameter("@PageType", SqlDbType.NVarChar,50),
                                new SqlParameter("@Tstamp", SqlDbType.DateTime),
                                new SqlParameter("@Year", SqlDbType.Int),
                                new SqlParameter("@MonthOfYear", SqlDbType.Int),
                                new SqlParameter("@MonitorData", SqlDbType.Int),
                                new SqlParameter("@QualifiedData", SqlDbType.Int),
                                new SqlParameter("@QualifiedRate", SqlDbType.Decimal),
                                new SqlParameter("@Summary", SqlDbType.Text)};
                parameters[0].Value = pageType;
                parameters[1].Value = DateTime.Parse(dv[i]["Tstamp"].ToString());
                parameters[2].Value = int.Parse(dv[i]["Year"].ToString());
                parameters[3].Value = int.Parse(dv[i]["MonthOfYear"].ToString());
                parameters[4].Value = int.Parse(dv[i]["MonitorData"].ToString());
                parameters[5].Value = int.Parse(dv[i]["QualifiedData"].ToString());
                parameters[6].Value = decimal.Parse(dv[i]["QualifiedRate"].ToString());
                parameters[7].Value = dv[i]["Summary"].ToString();
                CommandInfo cmd = new CommandInfo(strSql.ToString(), parameters);
                sqllist.Add(cmd);
            }
            g_DatabaseHelper.ExecuteSqlTranWithIndentity(sqllist, connection);
        }
        #endregion
        #region 查询数据
        /// <summary>
        /// 查询月报汇总数据
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataView GetList(string strWhere, string[] factorcodes)
        {
            string factor = string.Join(",", factorcodes);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("with report as(select PageType,PointId,Tstamp,Year,MonthOfYear,[PollutantCode],[PollutantValue] from  ");
            strSql.Append(tableName);
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(") select * from   report pivot(max([PollutantValue]) for [PollutantCode] in (" + factor + "))a");

            return g_DatabaseHelper.ExecuteDataView(strSql.ToString(), connection);
        }
        /// <summary>
        /// 查询月报汇总数据
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataView GetListItem(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select *  FROM ");
            strSql.Append(tableName1);
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return g_DatabaseHelper.ExecuteDataView(strSql.ToString(), connection);
        }
        #endregion
    }
}
