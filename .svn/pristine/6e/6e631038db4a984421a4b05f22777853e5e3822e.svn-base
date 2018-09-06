using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AQIWS.Common
{
    public class DBHelper
    {
        public static string GetConnectionString(string connectionstring)
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings[connectionstring].ConnectionString;
        }

        public static System.Data.DataTable GetDataView(string cmdtext, System.Data.SqlClient.SqlParameter[] paras, string connectionstring)
        {

            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmdtext, connectionstring);
            da.SelectCommand.CommandType = System.Data.CommandType.Text;
            if (paras != null)
                da.SelectCommand.Parameters.AddRange(paras);
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return new System.Data.DataTable();
            }
        }

        public static object  GetEffect(string cmdtext, System.Data.SqlClient.SqlParameter[] paras, string connectionstring)
        {
            object objScalar = new object();
            using (SqlConnection myConn = new SqlConnection(connectionstring))
            {
                using (SqlCommand myCommand = new SqlCommand(cmdtext, myConn))
                {
                    try
                    {
                        if (ConfigurationManager.AppSettings["SqlCmdTimeOut"] != null && ConfigurationManager.AppSettings["SqlCmdTimeOut"].ToString().Trim() != "")
                        {
                            myCommand.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["SqlCmdTimeOut"]);
                        }
                        // 组装Sql参数
                        //PopulateParams(myCommand, paras);

                        if (myConn.State == ConnectionState.Closed)
                            myConn.Open();
                        objScalar = myCommand.ExecuteNonQuery();
                        return objScalar;
                    }
                    catch (Exception ex) { return null; }
                    finally
                    {
                        myConn.Close();
                    }
                }
            }
        }

        public static string GetRightData(object obj)
        {
            string val = string.Format("{0}", obj);
            if (val == "" || val == "&nbsp;" || val == "-99999" || val.StartsWith("-"))
            {
                return "--";
            }
            try
            {
                decimal dval = System.Convert.ToDecimal(val);
                return string.Format("{0}", Math.Ceiling(dval * 1000));
            }
            catch
            {
                return val;
            }
        }
    }
}