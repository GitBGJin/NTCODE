﻿using SmartEP.Core.Generic;
using SmartEP.Utilities.AdoData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.Common.GridView
{
    /// <summary>
    /// 名称：GridViewPagerDAL.cs
    /// 创建人：李飞
    /// 创建日期：2015-08-11
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能描述：
    /// 1、虚拟分页类
    /// 2、取得行转列分页数据
    /// 3、取得数据总行数
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class GridViewPagerDAL
    {
        /// <summary>
        /// 存储过程处理类
        /// </summary>
        BaseDAHelper g_DBBiz = Singleton<BaseDAHelper>.GetInstance();

        /// <summary>
        /// 取得虚拟分页数据和总行数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fieldName">查询字段</param>
        /// <param name="keyName">索引字段</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="where">WHERE条件</param>
        /// <param name="connStr">连接字符串</param>
        /// <param name="recordTotal">数据总行数</param>
        /// <returns></returns>
        public DataView GetGridViewPager(string tableName, string fieldName, string keyName
            , int pageSize, int pageNo, string orderBy, string where, string connStr, out int recordTotal)
        {
            recordTotal = 0;
            try
            {
                g_DBBiz.ClearParameters();
                SqlParameter pramViewName = new SqlParameter();
                pramViewName = new SqlParameter();
                pramViewName.SqlDbType = SqlDbType.NVarChar;
                pramViewName.ParameterName = "@viewName";
                pramViewName.Value = tableName;
                g_DBBiz.SetProcedureParameters(pramViewName);

                SqlParameter pramFieldName = new SqlParameter();
                pramFieldName = new SqlParameter();
                pramFieldName.SqlDbType = SqlDbType.NVarChar;
                pramFieldName.ParameterName = "@fieldName";
                pramFieldName.Value = fieldName;
                g_DBBiz.SetProcedureParameters(pramFieldName);

                SqlParameter pramKeyName = new SqlParameter();
                pramKeyName = new SqlParameter();
                pramKeyName.SqlDbType = SqlDbType.NVarChar;
                pramKeyName.ParameterName = "@keyName";
                pramKeyName.Value = keyName;
                g_DBBiz.SetProcedureParameters(pramKeyName);

                SqlParameter pramPageSize = new SqlParameter();
                pramPageSize = new SqlParameter();
                pramPageSize.SqlDbType = SqlDbType.Int;
                pramPageSize.ParameterName = "@pageSize";
                pramPageSize.Value = pageSize;
                g_DBBiz.SetProcedureParameters(pramPageSize);

                SqlParameter pramPageNo = new SqlParameter();
                pramPageNo = new SqlParameter();
                pramPageNo.SqlDbType = SqlDbType.Int;
                pramPageNo.ParameterName = "@pageNo";
                pramPageNo.Value = pageNo;
                g_DBBiz.SetProcedureParameters(pramPageNo);

                SqlParameter pramOrder = new SqlParameter();
                pramOrder = new SqlParameter();
                pramOrder.SqlDbType = SqlDbType.NVarChar;
                pramOrder.ParameterName = "@orderString";
                pramOrder.Value = (orderBy.IndexOf("Order by") >= 0 || orderBy.IndexOf("order by") >= 0) ? orderBy : " Order by " + orderBy;
                g_DBBiz.SetProcedureParameters(pramOrder);

                SqlParameter pramWhere = new SqlParameter();
                pramWhere = new SqlParameter();
                pramWhere.SqlDbType = SqlDbType.NVarChar;
                pramWhere.ParameterName = "@whereString";
                pramWhere.Value = string.IsNullOrEmpty(where) ? "1=1" : where;
                g_DBBiz.SetProcedureParameters(pramWhere);

                SqlParameter pramTotal = new SqlParameter();
                pramTotal = new SqlParameter();
                pramTotal.SqlDbType = SqlDbType.Int;
                pramTotal.ParameterName = "@recordTotal";
                pramTotal.Direction = ParameterDirection.Output;
                g_DBBiz.SetProcedureParameters(pramTotal);

                var dv = g_DBBiz.ExecuteProc("UP_GridViewPager", connStr);
                recordTotal = (int)pramTotal.Value;
                return dv;
            }
            catch (Exception ex)
            {
                WriteTextLog("GetGridViewPager" + "分页", ex.Message, DateTime.Now);
                throw ex;

            }
        }
        /// <summary>
        /// 获取复杂查询语句虚拟分页数据和总行数
        /// </summary>
        /// <param name="ComplexSql">查询语句</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="connStr">连接字符串</param>
        /// <param name="recordTotal">数据总行数</param>
        /// <returns></returns>
        public DataView GridViewPagerCplexSql(string ComplexSql, int pageSize, int pageNo, string orderBy, string connStr, out int recordTotal)
        {
            recordTotal = 0;
            try
            {
                g_DBBiz.ClearParameters();
                SqlParameter pramComplexSql = new SqlParameter();
                pramComplexSql = new SqlParameter();
                pramComplexSql.SqlDbType = SqlDbType.NVarChar;
                pramComplexSql.ParameterName = "@ComplexSql";
                pramComplexSql.Value = ComplexSql;
                g_DBBiz.SetProcedureParameters(pramComplexSql);

                SqlParameter pramPageSize = new SqlParameter();
                pramPageSize = new SqlParameter();
                pramPageSize.SqlDbType = SqlDbType.Int;
                pramPageSize.ParameterName = "@pageSize";
                pramPageSize.Value = pageSize;
                g_DBBiz.SetProcedureParameters(pramPageSize);

                SqlParameter pramPageNo = new SqlParameter();
                pramPageNo = new SqlParameter();
                pramPageNo.SqlDbType = SqlDbType.Int;
                pramPageNo.ParameterName = "@pageNo";
                pramPageNo.Value = pageNo;
                g_DBBiz.SetProcedureParameters(pramPageNo);

                SqlParameter pramOrder = new SqlParameter();
                pramOrder = new SqlParameter();
                pramOrder.SqlDbType = SqlDbType.NVarChar;
                pramOrder.ParameterName = "@orderString";
                pramOrder.Value = (orderBy.IndexOf("Order by") >= 0 || orderBy.IndexOf("order by") >= 0) ? orderBy : " Order by " + orderBy;
                g_DBBiz.SetProcedureParameters(pramOrder);

                SqlParameter pramTotal = new SqlParameter();
                pramTotal = new SqlParameter();
                pramTotal.SqlDbType = SqlDbType.Int;
                pramTotal.ParameterName = "@recordTotal";
                pramTotal.Direction = ParameterDirection.Output;
                g_DBBiz.SetProcedureParameters(pramTotal);

                var dv = g_DBBiz.ExecuteProc("UP_GridViewPagerCplexSql", connStr);
                recordTotal = (int)pramTotal.Value;
                return dv;
            }
            catch (Exception ex)
            {
                WriteTextLog("GridViewPagerCplexSql" + "分页", ex.Message, DateTime.Now);
                throw ex;

            }
        }
        /// <summary>
        /// 获取复杂查询语句虚拟分页数据和总行数
        /// </summary>
        /// <param name="ComplexSql">查询语句</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="connStr">连接字符串</param>
        /// <param name="recordTotal">数据总行数</param>
        /// <returns></returns>
        public DataView GridViewPagerAllTimeSql(string ComplexSql, string TempTableSql, int pageSize, int pageNo, string orderBy, string connStr, out int recordTotal)
        {
            recordTotal = 0;
            try
            {
                g_DBBiz.ClearParameters();
                SqlParameter pramComplexSql = new SqlParameter();
                pramComplexSql = new SqlParameter();
                pramComplexSql.SqlDbType = SqlDbType.NVarChar;
                pramComplexSql.ParameterName = "@ComplexSql";
                pramComplexSql.Value = ComplexSql;
                g_DBBiz.SetProcedureParameters(pramComplexSql);

                SqlParameter pramTempTableSql = new SqlParameter();
                pramTempTableSql = new SqlParameter();
                pramTempTableSql.SqlDbType = SqlDbType.NVarChar;
                pramTempTableSql.ParameterName = "@TempTableSql";
                pramTempTableSql.Value = TempTableSql;
                g_DBBiz.SetProcedureParameters(pramTempTableSql);

                SqlParameter pramPageSize = new SqlParameter();
                pramPageSize = new SqlParameter();
                pramPageSize.SqlDbType = SqlDbType.Int;
                pramPageSize.ParameterName = "@pageSize";
                pramPageSize.Value = pageSize;
                g_DBBiz.SetProcedureParameters(pramPageSize);

                SqlParameter pramPageNo = new SqlParameter();
                pramPageNo = new SqlParameter();
                pramPageNo.SqlDbType = SqlDbType.Int;
                pramPageNo.ParameterName = "@pageNo";
                pramPageNo.Value = pageNo;
                g_DBBiz.SetProcedureParameters(pramPageNo);

                SqlParameter pramOrder = new SqlParameter();
                pramOrder = new SqlParameter();
                pramOrder.SqlDbType = SqlDbType.NVarChar;
                pramOrder.ParameterName = "@orderString";
                pramOrder.Value = (orderBy.IndexOf("Order by") >= 0 || orderBy.IndexOf("order by") >= 0) ? orderBy : " Order by " + orderBy;
                g_DBBiz.SetProcedureParameters(pramOrder);

                SqlParameter pramTotal = new SqlParameter();
                pramTotal = new SqlParameter();
                pramTotal.SqlDbType = SqlDbType.Int;
                pramTotal.ParameterName = "@recordTotal";
                pramTotal.Direction = ParameterDirection.Output;
                g_DBBiz.SetProcedureParameters(pramTotal);

                var dv = g_DBBiz.ExecuteProc("UP_GridViewPagerAllTimeSql", connStr);
                recordTotal = (int)pramTotal.Value;
                return dv;
            }
            catch (Exception ex)
            {
                WriteTextLog("GridViewPagerAllTimeSql" + "分页", ex.Message, DateTime.Now);
                throw ex;

            }
        }

        /// <summary>  
        /// 写入日志到文本文件  
        /// </summary>  
        /// <param name="action">动作</param>  
        /// <param name="strMessage">日志内容</param>  
        /// <param name="time">时间</param>  
        public static void WriteTextLog(string action, string strMessage, DateTime time)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"System\Log\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fileFullPath = path + time.ToString("yyyy-MM-dd") + ".System.txt";
            StringBuilder str = new StringBuilder();
            str.Append("Time:    " + time.ToString() + "\r\n");
            str.Append("Action:  " + action + "\r\n");
            str.Append("Message: " + strMessage + "\r\n");
            str.Append("-----------------------------------------------------------\r\n\r\n");
            StreamWriter sw;
            if (!File.Exists(fileFullPath))
            {
                sw = File.CreateText(fileFullPath);
            }
            else
            {
                sw = File.AppendText(fileFullPath);
            }
            sw.WriteLine(str.ToString());
            sw.Close();
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="tableName">数据表名</param>
        /// <param name="fieldName">查询字段</param>
        /// <param name="groupBy">分组(唯一性要求字段)字段（多字段以‘,’分割）</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="where">查询条件</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="connStr">数据库连接字串</param>
        /// <param name="recordTotal">输出记录总数</param>
        /// <returns></returns>
        public DataView GetPivotDataPager(string tableName, string fieldName, string groupBy, string orderBy, string where, int pageSize, int pageNo, string connStr, out int recordTotal)
        {
            try
            {
                g_DBBiz.ClearParameters();
                SqlParameter pageSizePara = new SqlParameter();
                pageSizePara = new SqlParameter();
                pageSizePara.SqlDbType = SqlDbType.Int;
                pageSizePara.ParameterName = "@pageSize";
                pageSizePara.Value = pageSize;
                g_DBBiz.SetProcedureParameters(pageSizePara);

                SqlParameter pageNoPara = new SqlParameter();
                pageNoPara = new SqlParameter();
                pageNoPara.SqlDbType = SqlDbType.Int;
                pageNoPara.ParameterName = "@pageNo";
                pageNoPara.Value = pageNo;
                g_DBBiz.SetProcedureParameters(pageNoPara);

                SqlParameter tableNamePara = new SqlParameter();
                tableNamePara = new SqlParameter();
                tableNamePara.SqlDbType = SqlDbType.NVarChar;
                tableNamePara.ParameterName = "@tableName";
                tableNamePara.Value = tableName;
                g_DBBiz.SetProcedureParameters(tableNamePara);

                SqlParameter fieldNamePara = new SqlParameter();
                fieldNamePara = new SqlParameter();
                fieldNamePara.SqlDbType = SqlDbType.NVarChar;
                fieldNamePara.ParameterName = "@fieldName";
                fieldNamePara.Value = fieldName;
                g_DBBiz.SetProcedureParameters(fieldNamePara);

                SqlParameter groupByPara = new SqlParameter();
                groupByPara = new SqlParameter();
                groupByPara.SqlDbType = SqlDbType.NVarChar;
                groupByPara.ParameterName = "@groupBy";
                groupByPara.Value = groupBy;
                g_DBBiz.SetProcedureParameters(groupByPara);

                SqlParameter orderByPara = new SqlParameter();
                orderByPara = new SqlParameter();
                orderByPara.SqlDbType = SqlDbType.NVarChar;
                orderByPara.ParameterName = "@orderBy";
                orderByPara.Value = orderBy; ;
                g_DBBiz.SetProcedureParameters(orderByPara);

                SqlParameter wherePara = new SqlParameter();
                wherePara = new SqlParameter();
                wherePara.SqlDbType = SqlDbType.NVarChar;
                wherePara.ParameterName = "@whereString";
                wherePara.Value = where;
                g_DBBiz.SetProcedureParameters(wherePara);

                SqlParameter totalPara = new SqlParameter();
                totalPara = new SqlParameter();
                totalPara.SqlDbType = SqlDbType.Int;
                totalPara.ParameterName = "@recordTotal";
                totalPara.Direction = ParameterDirection.Output;
                g_DBBiz.SetProcedureParameters(totalPara);

                DataView dv = g_DBBiz.ExecuteProc("UP_GetPivotPager", connStr);
                recordTotal = (int)totalPara.Value;
                return dv;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 取得虚拟分页查询总行数(行转列数据)
        /// </summary>
        /// <param name="tableName">数据表名</param>
        /// <param name="groupBy">分组(唯一性要求字段)字段（多字段以‘,’分割）</param>
        /// <param name="where">查询条件</param>
        /// <param name="connStr">数据库连接字串</param>
        /// <returns>总行数</returns>
        public int GetAllDataCount(string tableName, string groupBy, string where, string connStr)
        {
            int recordTotal = 0;
            try
            {
                g_DBBiz.ClearParameters();
                SqlParameter pramViewName = new SqlParameter();
                pramViewName = new SqlParameter();
                pramViewName.SqlDbType = SqlDbType.NVarChar;
                pramViewName.ParameterName = "@viewName";
                pramViewName.Value = tableName;
                g_DBBiz.SetProcedureParameters(pramViewName);

                SqlParameter pramKeyName = new SqlParameter();
                pramKeyName = new SqlParameter();
                pramKeyName.SqlDbType = SqlDbType.NVarChar;
                pramKeyName.ParameterName = "@keyName";
                pramKeyName.Value = groupBy;
                g_DBBiz.SetProcedureParameters(pramKeyName);

                SqlParameter pramWhere = new SqlParameter();
                pramWhere = new SqlParameter();
                pramWhere.SqlDbType = SqlDbType.NVarChar;
                pramWhere.ParameterName = "@whereString";
                pramWhere.Value = where;
                g_DBBiz.SetProcedureParameters(pramWhere);

                SqlParameter pramTotal = new SqlParameter();
                pramTotal = new SqlParameter();
                pramTotal.SqlDbType = SqlDbType.Int;
                pramTotal.ParameterName = "@recordTotal";
                pramTotal.Direction = ParameterDirection.Output;
                g_DBBiz.SetProcedureParameters(pramTotal);

                var dv = g_DBBiz.ExecuteProc("UP_GetAllDataCount", connStr);
                recordTotal = (int)pramTotal.Value;
                return recordTotal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

//        public List<TEntity> GetPagingList<TEntity>(string connectionString, string table, string condition, string orderBy, int pageIndex, int pageSize) where TEntity : class, new()
//        {
//            if (string.IsNullOrWhiteSpace(table) || string.IsNullOrWhiteSpace(orderBy))
//            {
//                return new List<TEntity>();
//            }

//            string sqlFormat = @"select *  
//                               from (select *, ROW_NUMBER() over (order by {2}) AS RowNumber  
//                                     from {0}  
//                                     where 1 = 1 {1}) T  
//                               where RowNumber between @skip and @end  
//                               order by {2}";

//            if (!string.IsNullOrEmpty(condition))
//            {
//                condition = "and " + condition;
//            }

//            int skip = (pageIndex - 1) * pageSize + 1;
//            int end = pageIndex * pageSize;

//            var parameters = new SqlParameter[]  
//           {  
//               new SqlParameter("@skip", skip),  
//               new SqlParameter("@end", end)
//           };

//            string sqlString = string.Format(sqlFormat, table, condition, orderBy);
//            return GetList<TEntity>(connectionString, sqlString, parameters);
//        }

    }
}
