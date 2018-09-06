using SmartEP.Core.Generic;
using SmartEP.Utilities.AdoData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace SmartEP.Data.SqlServer.BaseData
{
    public class AccessInfoDAL
    {
        public int InserAccessInfo(string pointId, string cardNumber, string registerName, string stationWay, DateTime stationDate)
        {
            string tableName = "dbo.TB_AccessInformation";
            DatabaseHelper g_DatabaseHelper = new DatabaseHelper();
            List<CommandInfo> sqllist = new List<CommandInfo>();
            StringBuilder strSql = new StringBuilder();
            string connection = "AMS_MonitoringBusinessConnection";
            strSql.Append("insert into ");
            strSql.Append(tableName);
            strSql.Append("(PointId,CardNumber,RegisterName,StationWay,StationDate)");
            strSql.Append("values(@pointId,@cardNumber,@registerName,@stationWay,@stationDate)");
            SqlParameter[] parameters = { 
                                            new SqlParameter("@pointId", SqlDbType.Int),
                                            new SqlParameter("@cardNumber", SqlDbType.NVarChar,50),
                                            new SqlParameter("@registerName", SqlDbType.NVarChar,50),
                                            new SqlParameter("@stationWay", SqlDbType.NVarChar,50),
                                            new SqlParameter("@stationDate", SqlDbType.DateTime),
                                            
                                         };
            parameters[0].Value = pointId;
            parameters[1].Value = cardNumber;
            parameters[2].Value = registerName;
            parameters[3].Value = stationWay;
            parameters[4].Value = stationDate;
            CommandInfo cmd = new CommandInfo(strSql.ToString(), parameters);
            sqllist.Add(cmd);
            
            string sql2 = @"select count(*) FROM TB_AccessInformation 
                            where PointId=" + pointId + " and cardNumber='" + cardNumber + "'AND stationDate='" + stationDate + "'";
            int cou;
            try
            {
                 cou = System.Convert.ToInt32(g_DatabaseHelper.ExecuteScalar(sql2, connection));
            }
            catch (Exception)
            {
               cou = -1;
            }
            if (cou >= 1)
            {
                 return 2;//重复
            }
            else
            {
                 g_DatabaseHelper.ExecuteSqlTranWithIndentity(sqllist, connection);
                 return 1;//插入成功
            }

        }

     }  
}
