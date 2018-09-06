using SmartEP.Core.Generic;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.Utilities.AdoData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.MonitoringBusiness.Air
{
    public class RPT_AQIForecastDAL
    {
        #region  Method
        public RPT_AQIForecastDAL()
        { }
        protected DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(RPT_AQIForecastEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            StringBuilder strSql1 = new StringBuilder();
            StringBuilder strSql2 = new StringBuilder();
            if (model.AQITimeA != null)
            {
                strSql1.Append("AQITimeA,");
                strSql2.Append("'" + model.AQITimeA + "',");
            }
            if (model.AQIClassA != null)
            {
                strSql1.Append("AQIClassA,");
                strSql2.Append("'" + model.AQIClassA + "',");
            }
            if (model.PrimaryPollutantA != null)
            {
                strSql1.Append("PrimaryPollutantA,");
                strSql2.Append("'" + model.PrimaryPollutantA + "',");
            }
            if (model.AQIA != null)
            {
                strSql1.Append("AQIA,");
                strSql2.Append("'" + model.AQIA + "',");
            }
            if (model.AQITimeB != null)
            {
                strSql1.Append("AQITimeB,");
                strSql2.Append("'" + model.AQITimeB + "',");
            }
            if (model.AQIClassB != null)
            {
                strSql1.Append("AQIClassB,");
                strSql2.Append("'" + model.AQIClassB + "',");
            }
            if (model.PrimaryPollutantB != null)
            {
                strSql1.Append("PrimaryPollutantB,");
                strSql2.Append("'" + model.PrimaryPollutantB + "',");
            }
            if (model.AQIB != null)
            {
                strSql1.Append("AQIB,");
                strSql2.Append("'" + model.AQIB + "',");
            }
            if (model.AQITimeC != null)
            {
                strSql1.Append("AQITimeC,");
                strSql2.Append("'" + model.AQITimeC + "',");
            }
            if (model.AQIClassC != null)
            {
                strSql1.Append("AQIClassC,");
                strSql2.Append("'" + model.AQIClassC + "',");
            }
            if (model.PrimaryPollutantC != null)
            {
                strSql1.Append("PrimaryPollutantC,");
                strSql2.Append("'" + model.PrimaryPollutantC + "',");
            }
            if (model.AQIC != null)
            {
                strSql1.Append("AQIC,");
                strSql2.Append("'" + model.AQIC + "',");
            }
            if (model.Description != null)
            {
                strSql1.Append("Description,");
                strSql2.Append("'" + model.Description + "',");
            }
            if (model.IssuedUnit != null)
            {
                strSql1.Append("IssuedUnit,");
                strSql2.Append("'" + model.IssuedUnit + "',");
            }
            if (model.IssuedTime != null)
            {
                strSql1.Append("IssuedTime,");
                strSql2.Append("'" + model.IssuedTime + "',");
            }
            if (model.IsIssued != null)
            {
                strSql1.Append("IsIssued,");
                strSql2.Append("'" + model.IsIssued + "',");
            }
            strSql.Append("insert into RPT_AQIForecast(");
            strSql.Append(strSql1.ToString().Remove(strSql1.Length - 1));
            strSql.Append(")");
            strSql.Append(" values (");
            strSql.Append(strSql2.ToString().Remove(strSql2.Length - 1));
            strSql.Append(")");
            strSql.Append(";select @@IDENTITY");
            object obj = g_DatabaseHelper.ExecuteScalar(strSql.ToString(), ConnectionString);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(RPT_AQIForecastEntity model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update RPT_AQIForecast set ");
            if (model.AQITimeA != null)
            {
                strSql.Append("AQITimeA='" + model.AQITimeA + "',");
            }
            else
            {
                strSql.Append("AQITimeA= null ,");
            }
            if (model.AQIClassA != null)
            {
                strSql.Append("AQIClassA='" + model.AQIClassA + "',");
            }
            else
            {
                strSql.Append("AQIClassA= null ,");
            }
            if (model.PrimaryPollutantA != null)
            {
                strSql.Append("PrimaryPollutantA='" + model.PrimaryPollutantA + "',");
            }
            else
            {
                strSql.Append("PrimaryPollutantA= null ,");
            }
            if (model.AQIA != null)
            {
                strSql.Append("AQIA='" + model.AQIA + "',");
            }
            else
            {
                strSql.Append("AQIA= null ,");
            }
            if (model.AQITimeB != null)
            {
                strSql.Append("AQITimeB='" + model.AQITimeB + "',");
            }
            else
            {
                strSql.Append("AQITimeB= null ,");
            }
            if (model.AQIClassB != null)
            {
                strSql.Append("AQIClassB='" + model.AQIClassB + "',");
            }
            else
            {
                strSql.Append("AQIClassB= null ,");
            }
            if (model.PrimaryPollutantB != null)
            {
                strSql.Append("PrimaryPollutantB='" + model.PrimaryPollutantB + "',");
            }
            else
            {
                strSql.Append("PrimaryPollutantB= null ,");
            }
            if (model.AQIB != null)
            {
                strSql.Append("AQIB='" + model.AQIB + "',");
            }
            else
            {
                strSql.Append("AQIB= null ,");
            }
            if (model.AQITimeC != null)
            {
                strSql.Append("AQITimeC='" + model.AQITimeC + "',");
            }
            else
            {
                strSql.Append("AQITimeC= null ,");
            }
            if (model.AQIClassC != null)
            {
                strSql.Append("AQIClassC='" + model.AQIClassC + "',");
            }
            else
            {
                strSql.Append("AQIClassC= null ,");
            }
            if (model.PrimaryPollutantC != null)
            {
                strSql.Append("PrimaryPollutantC='" + model.PrimaryPollutantC + "',");
            }
            else
            {
                strSql.Append("PrimaryPollutantC= null ,");
            }
            if (model.AQIC != null)
            {
                strSql.Append("AQIC='" + model.AQIC + "',");
            }
            else
            {
                strSql.Append("AQIC= null ,");
            }
            if (model.Description != null)
            {
                strSql.Append("Description='" + model.Description + "',");
            }
            else
            {
                strSql.Append("Description= null ,");
            }
            if (model.IssuedUnit != null)
            {
                strSql.Append("IssuedUnit='" + model.IssuedUnit + "',");
            }
            else
            {
                strSql.Append("IssuedUnit= null ,");
            }
            if (model.IssuedTime != null)
            {
                strSql.Append("IssuedTime='" + model.IssuedTime + "',");
            }
            else
            {
                strSql.Append("IssuedTime= null ,");
            }
            if (model.IsIssued != null)
            {
                strSql.Append("IsIssued='" + model.IsIssued + "',");
            }
            else
            {
                strSql.Append("IsIssued= null ,");
            }
            int n = strSql.ToString().LastIndexOf(",");
            strSql.Remove(n, 1);
            strSql.Append(" where ID=" + model.ID + "");
            int rowsAffected = Convert.ToInt32(g_DatabaseHelper.ExecuteScalar(strSql.ToString(), ConnectionString));
            if (rowsAffected > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string IDs)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from RPT_AQIForecast ");
            strSql.Append(" where ID IN (" + IDs + ")");
            int rowsAffected = Convert.ToInt32(g_DatabaseHelper.ExecuteScalar(strSql.ToString(), ConnectionString));
            if (rowsAffected > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select[ID]"
                + ",[AQITimeA],[AQIClassA],[PrimaryPollutantA],[AQIA]"
                + ",[AQITimeB],[AQIClassB],[PrimaryPollutantB],[AQIB]"
                + ",[AQITimeC],[AQIClassC],[PrimaryPollutantC],[AQIC]"
                + ",[Description],[IssuedUnit],[IssuedTime],[IsIssued]");
            strSql.Append(" FROM RPT_AQIForecast ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where (1=1) " + strWhere);
            }
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), ConnectionString);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAQIList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT TOP 1 [ID],[AQITimeA],[AQIClassA],[PrimaryPollutantA],[AQIA],[AQITimeB],[AQIClassB],[PrimaryPollutantB]"
                        + ",[AQIB],[AQITimeC],[AQIClassC],[PrimaryPollutantC],[AQIC],[Description],[IssuedUnit],[IssuedTime],[IsIssued]");
            strSql.Append(" FROM RPT_AQIForecast ");
            strSql.Append("WHERE [IsIssued]=1  ORDER BY [IssuedTime] DESC");
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), ConnectionString);
        }
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        private string ConnectionString
        {
            get
            {
                return "AMS_MonitoringBusinessConnection";
            }
        }
        #endregion  Method
    }
}
