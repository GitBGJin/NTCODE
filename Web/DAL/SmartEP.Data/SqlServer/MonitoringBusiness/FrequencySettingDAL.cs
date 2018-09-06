using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Data.Enums;
using SmartEP.Utilities.AdoData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.MonitoringBusiness
{
    public class FrequencySettingDAL
    {
        /// <summary>
        /// 数据库处理类
        /// </summary>
        DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.MonitoringBusiness);
        /// <summary>
        /// 数据库表名
        /// </summary>
        private string tableName = "dbo.FrequencySetting";
        /// <summary>
        /// 更新配置数据
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ApplicationUid"></param>
        public void insertTable(DataTable dt, string ApplicationUid, string factorcode)
        {
            try
            {
                List<CommandInfo> sqllist = new List<CommandInfo>();
                StringBuilder strdaySql = new StringBuilder();
                strdaySql.Append("delete from ");
                strdaySql.Append(tableName);
                strdaySql.Append(" where ApplicationUid='");
                strdaySql.Append(ApplicationUid);
                strdaySql.Append("'  and PollutantCode='");
                strdaySql.Append(factorcode);
                strdaySql.Append("'");
                g_DatabaseHelper.ExecuteNonQuery(strdaySql.ToString(), connection);
                foreach (DataRow dr in dt.Rows)
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("insert into ");
                    strSql.Append(tableName);
                    strSql.Append("([ApplicationUid],[Upper],[Lower],[Range],[OederNum],[PollutantCode],[PollutantName])");
                    strSql.Append("values(@ApplicationUid,@Upper,@Lower,@Range,@OederNum,@PollutantCode,@PollutantName)");
                    SqlParameter[] parameters = { 
                                new SqlParameter("@ApplicationUid", SqlDbType.NVarChar,50),
                                new SqlParameter("@Upper", SqlDbType.Decimal),
                                new SqlParameter("@Lower", SqlDbType.Decimal),
                                new SqlParameter("@Range", SqlDbType.NVarChar,50),
                                new SqlParameter("@OederNum", SqlDbType.Int),
                                new SqlParameter("@PollutantCode", SqlDbType.NVarChar,50),
                                new SqlParameter("@PollutantName", SqlDbType.NVarChar,50)};
                    parameters[0].Value = dr["ApplicationUid"].ToString();
                    decimal? upperOrNUll = null;
                    decimal upper;
                    if (decimal.TryParse(dr["Upper"].ToString(), out upper))
                    {
                        upperOrNUll = upper;
                    }
                    else
                    {
                        upperOrNUll = null;
                    }
                    parameters[1].Value = upperOrNUll;
                    decimal? lowerOrNUll = null;
                    decimal lower;
                    if (decimal.TryParse(dr["Lower"].ToString(), out lower))
                    {
                        lowerOrNUll = lower;
                    }
                    else
                    {
                        lowerOrNUll = null;
                    }
                    parameters[2].Value = lowerOrNUll;
                    parameters[3].Value = dr["Range"].ToString();
                    parameters[4].Value = int.Parse(dr["OederNum"].ToString());
                    parameters[5].Value = dr["PollutantCode"].ToString();
                    parameters[6].Value = dr["PollutantName"].ToString();
                    CommandInfo cmd = new CommandInfo(strSql.ToString(), parameters);
                    sqllist.Add(cmd);
                }
                g_DatabaseHelper.ExecuteSqlTranWithIndentity(sqllist, connection);
            }
            catch (Exception ex)
            {
            }
        }
        /// <summary>
        /// 获取频数分布配置信息
        /// </summary>
        /// <param name="ApplicationUid"></param>
        /// <returns></returns>
        public DataView GetSetData(string ApplicationUid, string factorcode)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select OederNum as Num,Upper,Lower,Range,PollutantCode,PollutantName  FROM ");
                strSql.Append(tableName);
                strSql.Append(" where ApplicationUid='" + ApplicationUid + "' and  PollutantCode='" + factorcode + "'");
                strSql.Append("  order by OederNum");
                return g_DatabaseHelper.ExecuteDataView(strSql.ToString(), connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
