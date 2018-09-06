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
    /// 名称：MessageTextDAL.cs
    /// 创建人：樊垂贺
    /// 创建日期：2016-3-13
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 环境空气发布：短信内容管理
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class MessageTextDAL
    {
        /// <summary>
        /// 数据库表名
        /// </summary>
        private string tableName = "dbo.TB_MessageText";
        /// <summary>
        /// 数据库处理类
        /// </summary>
        DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connection = "AMS_MonitoringBusinessConnection";
        private string basecon = "AMS_BaseDataConnection";
        #region 获得发送人员手机号
        public DataView getTelNum(string messageType)
        {
            try
            {
                string sql = string.Format(@"SELECT  * FROM [AMS_BaseData].[dbo].[TB_MessageTel] where PageType='{0}'", messageType);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
        #region 插入数据
        public void insertTable(string messagetype, DateTime? datetime, DateTime dtbegin, DateTime dtend, string messagetext, string daterange, string username)
        {
            List<CommandInfo> sqllist = new List<CommandInfo>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ");
            strSql.Append(tableName);
            strSql.Append("(messagetype,datetime,begintime,endtime,messagetext,daterange,CreatUser,CreatDateTime,text2)");
            strSql.Append("values(@messagetype,@datetime,@begintime,@endtime,@messagetext,@daterange,@CreatUser,@CreatDateTime,@text2)");
            SqlParameter[] parameters = { 
                                            new SqlParameter("@messagetype", SqlDbType.NVarChar,50),
                                            new SqlParameter("@datetime", SqlDbType.DateTime),
                                            new SqlParameter("@begintime", SqlDbType.DateTime),
                                            new SqlParameter("@endtime", SqlDbType.DateTime),
                                            new SqlParameter("@messagetext", SqlDbType.Text),
                                            new SqlParameter("@daterange", SqlDbType.NVarChar,50),
                                            new SqlParameter("@CreatUser", SqlDbType.NVarChar,50),
                                            new SqlParameter("@CreatDateTime", SqlDbType.DateTime),
                                            new SqlParameter("@text2", SqlDbType.NVarChar,50)
                                         };
            parameters[0].Value = messagetype;
            parameters[1].Value = datetime;
            parameters[2].Value = dtbegin;
            parameters[3].Value = dtend;
            parameters[4].Value = messagetext;
            parameters[5].Value = daterange;
            parameters[6].Value = username;
            parameters[7].Value = DateTime.Now;
            parameters[8].Value = "未发送";
            CommandInfo cmd = new CommandInfo(strSql.ToString(), parameters);
            sqllist.Add(cmd);
            g_DatabaseHelper.ExecuteSqlTranWithIndentity(sqllist, connection);
        }
        #endregion
        #region 获取数据
        public DataView GetMessageText(string strWhere)
        {
            try
            {
                string dayTable = tableName;
                string sql = string.Format(@"select * from {0} where {1} order by datetime desc", dayTable, strWhere);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion
        #region 更新数据
        public void updatedata(string strWhere, string messagetext, string username)
        {
            try
            {
                string dayTable = tableName;
                string sql = string.Format(@"update {0} set messagetext='{2}',UpdateUser='{3}',UpdateDateTime='{4}'  where {1}",
                    dayTable, strWhere, messagetext, username, DateTime.Now);
                g_DatabaseHelper.ExecuteNonQuery(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion
        #region 更新发送状态
        public void upSendStatus(string strWhere, string messagetext, string username)
        {
            try
            {
                string dayTable = tableName;
                string sql = string.Format(@"update {0} set text2='已提交发送'  where {1}",
                    dayTable, strWhere, messagetext, username, DateTime.Now);
                g_DatabaseHelper.ExecuteNonQuery(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion
    }
}
