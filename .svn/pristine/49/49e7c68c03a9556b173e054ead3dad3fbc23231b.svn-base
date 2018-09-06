using SmartEP.Core.Generic;
using SmartEP.Utilities.AdoData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.QualityControlOperation.Water
{
    
    public partial class MaterialRigisterDAL
    {
        /// <summary>
        /// 数据库处理类
        /// </summary>
        DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string Frame_Connection = "Frame_Connection";
        private string Base_Connection = "AMS_BaseDataConnection";
        /// <summary>
        /// 获取试剂类型
        /// </summary>
        /// <returns></returns>
        public DataTable GetReagentType()
        {
            try
            {
                string sql = string.Format(@"SELECT [RowGuid],[TypeName1]
                                              FROM {0}
                                              where RowStatus=1 ", "TB_OMMP_ReagentInBillItem");
                return g_DatabaseHelper.ExecuteDataTable(sql, Frame_Connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //删除数据
        public bool DeleteData(string sql)
        {
            try
            {
                g_DatabaseHelper.ExecuteNonQuery(sql, Frame_Connection);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 添加有证物质
        /// </summary>
        /// <param name="sqlBillItem"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool AddItem(string sqlBillItem, out string msg)
        {
            try
            {
                msg = "";
                g_DatabaseHelper.ExecuteNonQuery(sqlBillItem, Frame_Connection);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * ");
            strSql.Append(" FROM TB_OMMP_ReagentType ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where (1=1) " + strWhere + "order by SortNum desc");
            }
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), ConnectionString);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetLists(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select *");
            strSql.Append(" FROM TB_OMMP_ReagentInBillItem ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where (1=1) " + strWhere);
            }
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), ConnectionString);
        }
        /// <summary>
        /// <returns>获取符合条件的标液类型数据</returns>
        /// </summary>
        public DataTable GetListStyle(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select distinct TypeName,SortNum ");
            strSql.Append(" FROM TB_OMMP_ReagentType ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where (1=1) and ReagentName='" + strWhere + "' order by SortNum desc ");
            }
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), ConnectionString);
        }
        /// <summary>
        /// 根据标液号带出数据
        /// </summary>
        /// <param name="ProductionSN"></param>
        /// <returns></returns>
        public DataTable GetData(string ProductionSN)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * ");
            strSql.Append(" FROM TB_OMMP_ReagentType ");
            if (ProductionSN.Trim() != "")
            {
                strSql.Append(" where (1=1) and ProductSN='" + ProductionSN + "'order by SortNum desc ");
            }
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), ConnectionString);
        }

        /// <summary>
        /// 根据RowGuid获取系统编号
        /// </summary>
        /// <param name="rowID"></param>
        /// <returns></returns>
        public DataTable GetSysNum(string rowID)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from TB_OMMP_ReagentInBillItem");//ProductSN
            if (rowID.Trim() != "")
            {
                sql.Append(" where (1=1) and RowGuid='" + rowID + "'");
            }
            return g_DatabaseHelper.ExecuteDataTable(sql.ToString(), ConnectionString);

        }

        //根据rowID查出数据，主要获取打印专递的参数
        public DataTable GetNum(string sysNum)
        {
           
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * ");
            strSql.Append(" FROM TB_OMMP_ReagentInItemDetail");
            if (sysNum.Trim() != "")
            {
                strSql.Append(" where (1=1) and FixCode='" + sysNum + "'");
            }
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), ConnectionString);
        }
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        private string ConnectionString
        {
            get
            {
                return "Frame_Connection";
            }
        }



     
    }
}
