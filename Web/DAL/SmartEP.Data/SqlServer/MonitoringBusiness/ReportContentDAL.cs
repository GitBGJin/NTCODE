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
    /// 名称：ReportContentDAL.cs
    /// 创建人：樊垂贺
    /// 创建日期：2016-2-28
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 环境空气发布：报表内容管理
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class ReportContentDAL
    {
        /// <summary>
        /// 数据库表名
        /// </summary>
        private string tableName = "dbo.TB_ReportContent";
        /// <summary>
        /// 数据库处理类
        /// </summary>
        DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connection = "AMS_MonitoringBusinessConnection";

        #region 插入数据
        public void insertTable(Dictionary<string, string> pcontent, string[] ptitle, string pageid, DateTime starttime, DateTime endtime)
        {
            string strptitle = "-9999";
            strptitle = "'" + string.Join("','", ptitle) + "'";
            List<CommandInfo> sqllist = new List<CommandInfo>();
            if (ptitle == null || ptitle.Length == 0)
                return;
            else
                strptitle = "'" + string.Join("','", ptitle) + "'";
            StringBuilder strdaySql = new StringBuilder();
            strdaySql.Append("delete from ");
            strdaySql.Append(tableName);
            strdaySql.Append(" where starttime='");
            strdaySql.Append(starttime);
            strdaySql.Append("' and ");
            strdaySql.Append("endtime='");
            strdaySql.Append(endtime);
            strdaySql.Append("' and pageid='");
            strdaySql.Append(pageid);
            strdaySql.Append("' and ptitle in (");
            strdaySql.Append(strptitle);
            strdaySql.Append(")");
            g_DatabaseHelper.ExecuteNonQuery(strdaySql.ToString(), connection);
            foreach (string titleitem in ptitle)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into ");
                strSql.Append(tableName);
                strSql.Append("(pageid,starttime,endtime,ptitle,pcontent,CreatDateTime)");
                strSql.Append("values(@pageid,@starttime,@endtime,@ptitle,@pcontent,@CreatDateTime)");
                SqlParameter[] parameters = { 
                                new SqlParameter("@pageid", SqlDbType.NVarChar,50),
                                new SqlParameter("@starttime", SqlDbType.DateTime),
                                new SqlParameter("@endtime", SqlDbType.DateTime),
                                new SqlParameter("@ptitle", SqlDbType.NVarChar,50),
                                new SqlParameter("@pcontent", SqlDbType.Text),
                                new SqlParameter("@CreatDateTime", SqlDbType.DateTime)};
                parameters[0].Value = pageid;
                parameters[1].Value = starttime;
                parameters[2].Value = endtime;
                parameters[3].Value = titleitem;
                parameters[4].Value = pcontent[titleitem];
                parameters[5].Value = DateTime.Now;
                CommandInfo cmd = new CommandInfo(strSql.ToString(), parameters);
                sqllist.Add(cmd);
            }
            g_DatabaseHelper.ExecuteSqlTranWithIndentity(sqllist, connection);
        }
        #endregion
        #region 查询数据
        public DataView GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select *  FROM ");
            strSql.Append(tableName);
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return g_DatabaseHelper.ExecuteDataView(strSql.ToString(), connection);
        }
        #endregion
    }
}
