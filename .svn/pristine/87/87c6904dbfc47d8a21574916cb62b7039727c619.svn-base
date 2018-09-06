using SmartEP.Core.Generic;
using SmartEP.Data.SqlServer.Common.GridView;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.Utilities.AdoData;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.QualityControlOperation.Air
{
    /// <summary>
    /// 名称：OM_TaskItemDataDAL.cs
    /// 创建人：王秉晟
    /// 创建日期：2016-03-09
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 运维任务项数据记录表数据访问类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class OM_TaskItemDataDAL
    {
        #region << 变量 >>
        /// <summary>
        /// 数据库处理类
        /// </summary>
        DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();

        /// <summary>
        /// 虚拟分页类
        /// </summary>
        GridViewPagerDAL g_GridViewPager = Singleton<GridViewPagerDAL>.GetInstance();

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connection = "AMS_MonitoringBusinessConnection";

        /// <summary>
        /// 数据库表名
        /// </summary>
        private string tableName = "dbo.TB_OM_TaskItemData";

        /// <summary>
        /// 工作ID
        /// </summary>
        private string m_ActionID = string.Empty;
        #endregion

        #region << 构造函数 >>
        /// <summary>
        /// 构造函数
        /// </summary>
        public OM_TaskItemDataDAL()
        {
            //connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(ApplicationType.Water, PollutantDataType.Hour);
        }
        #endregion

        #region  << 方法 >>
        /// <summary>
        /// 获得数据列表
        /// </summary>
        /// <param name="strWhere">查询条件（例如：1=1）</param>
        /// <returns></returns>
        public DataTable GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * ");
            strSql.AppendFormat(" FROM {0} ", tableName);
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), connection);
        }
        /// <summary>
        /// 下位数据传入中心平台
        /// </summary>
        /// <param name="ds">数据源</param>
        /// <returns></returns>
        public void SubmitAirQCTaskData(DataSet ds)
        {
            SqlConnection conn = new SqlConnection(GetSqlConnection(connection));
            conn.Open();
            SqlBulkCopy sbc = new SqlBulkCopy(conn);
            foreach (DataTable table in ds.Tables)
            {
                sbc.DestinationTableName = table.TableName;
                //将数据集合和目标服务器的字段对应
                for (int q = 0; q < table.Columns.Count; q++)
                {
                    sbc.ColumnMappings.Add(table.Columns[q].ColumnName, table.Columns[q].ColumnName);
                }
                try
                {
                    sbc.WriteToServer(table);
                    sbc.ColumnMappings.Clear();
                }
                catch (Exception ex)
                {
                    throw ex;
                    //continue;
                }
            }
            ds.Dispose();
            conn.Close();
            sbc.Close();
        }
        /// <summary>
        /// 取得连接字符串
        /// </summary>
        /// <param name="appSettingsName"></param>
        /// <returns></returns>
        private string GetSqlConnection(string appSettingsName)
        {
            return ConfigurationManager.ConnectionStrings[appSettingsName].ConnectionString;
        }
        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataTable GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" * ");
            strSql.AppendFormat(" FROM {0} ", tableName);
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), connection);
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select count(1) FROM {0} ", tableName);
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object obj = g_DatabaseHelper.ExecuteScalar(strSql.ToString(), connection);
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
        /// 分页获取数据列表
        /// </summary>
        public DataTable GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T.id desc");
            }
            strSql.AppendFormat(")AS Row, T.*  from {0} T ", tableName);
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), connection);
        }
        #endregion
        /// <summary>
        /// 更新任务项值
        /// </summary>
        public void UpdateTable(OM_TaskItemDatumEntity[] OM_TaskItemDatumEntityArry, int taskGuid, string taskCode)
        {
            List<CommandInfo> sqllist = new List<CommandInfo>();
            for (int i = 0; i < OM_TaskItemDatumEntityArry.Length; i++)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update ");
                strSql.Append(tableName);
                strSql.Append(" set TaskGuid=@TaskGuid,TaskCode=@TaskCode,TaskItemGuid=@TaskItemGuid,ItemValue=@ItemValue,ItemRecordDate=@ItemRecordDate");
                strSql.Append(",MaintainceUser=@MaintainceUser,MaintainceDate=@MaintainceDate,Remark=@Remark,ItemFactor=@ItemFactor,RowGuid=@RowGuid");
                strSql.Append(",UniversalValue1=@UniversalValue1,UniversalValue2=@UniversalValue2,UniversalValue3=@UniversalValue3,UniversalValue4=@UniversalValue4 ");
                strSql.Append(" where taskGuid=");
                strSql.Append(taskGuid);
                strSql.Append(" and TaskCode='");
                strSql.Append(taskCode);
                strSql.Append("' and TaskItemGuid='");
                strSql.Append(OM_TaskItemDatumEntityArry[i].TaskItemGuid);
                strSql.Append("';");
                SqlParameter[] parameters = { 
                                new SqlParameter("@TaskGuid", SqlDbType.Int),
                                new SqlParameter("@TaskCode", SqlDbType.NVarChar,100),
                                new SqlParameter("@TaskItemGuid", SqlDbType.UniqueIdentifier),
                                new SqlParameter("@ItemValue", SqlDbType.NVarChar, 100),
                                new SqlParameter("@ItemRecordDate", SqlDbType.DateTime),
                                new SqlParameter("@MaintainceUser", SqlDbType.NVarChar,50),
                                new SqlParameter("@MaintainceDate", SqlDbType.DateTime),
                                new SqlParameter("@Remark", SqlDbType.NVarChar,200),
                                new SqlParameter("@ItemFactor", SqlDbType.NVarChar,50),
                                new SqlParameter("@RowGuid", SqlDbType.UniqueIdentifier),
                                new SqlParameter("@UniversalValue1", SqlDbType.NVarChar, 100),
                                new SqlParameter("@UniversalValue2", SqlDbType.NVarChar, 100),
                                new SqlParameter("@UniversalValue3", SqlDbType.NVarChar, 100),
                                new SqlParameter("@UniversalValue4", SqlDbType.NVarChar, 100)};
                parameters[0].Value = OM_TaskItemDatumEntityArry[i].TaskGuid;
                parameters[1].Value = OM_TaskItemDatumEntityArry[i].TaskCode;
                parameters[2].Value = OM_TaskItemDatumEntityArry[i].TaskItemGuid;
                parameters[3].Value = OM_TaskItemDatumEntityArry[i].ItemValue;
                parameters[4].Value = OM_TaskItemDatumEntityArry[i].ItemRecordDate;
                parameters[5].Value = OM_TaskItemDatumEntityArry[i].MaintainceUser;
                parameters[6].Value = OM_TaskItemDatumEntityArry[i].MaintainceDate;
                parameters[7].Value = OM_TaskItemDatumEntityArry[i].Remark;
                parameters[8].Value = OM_TaskItemDatumEntityArry[i].ItemFactor;
                parameters[9].Value = Guid.NewGuid();
                parameters[10].Value = OM_TaskItemDatumEntityArry[i].UniversalValue1;
                parameters[11].Value = OM_TaskItemDatumEntityArry[i].UniversalValue2;
                parameters[12].Value = OM_TaskItemDatumEntityArry[i].UniversalValue3;
                parameters[13].Value = OM_TaskItemDatumEntityArry[i].UniversalValue4;
                CommandInfo cmd = new CommandInfo(strSql.ToString(), parameters);
                sqllist.Add(cmd);
            }
            g_DatabaseHelper.ExecuteSqlTranWithIndentity(sqllist, connection);
        }
        /// <summary>
        /// 插入任务项值
        /// </summary>
        public void insertTable(OM_TaskItemDatumEntity[] OM_TaskItemDatumEntityArry, int taskGuid, string taskCode)
        {
            List<CommandInfo> sqllist = new List<CommandInfo>();
            //StringBuilder strdaySql = new StringBuilder();
            //strdaySql.Append("delete from ");
            //strdaySql.Append(tableName);
            //strdaySql.Append(" where taskGuid=");
            //strdaySql.Append(taskGuid);
            //strdaySql.Append(" and TaskCode='");
            //strdaySql.Append(taskCode);
            //strdaySql.Append("';");
            //g_DatabaseHelper.ExecuteNonQuery(strdaySql.ToString(), connection);
            for (int i = 0; i < OM_TaskItemDatumEntityArry.Length; i++)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into ");
                strSql.Append(tableName);
                strSql.Append("(TaskGuid,TaskCode,TaskItemGuid,ItemValue,ItemRecordDate,MaintainceUser,MaintainceDate,Remark,ItemFactor,RowGuid,UniversalValue1,UniversalValue2,UniversalValue3,UniversalValue4)");
                strSql.Append("values(@TaskGuid,@TaskCode,@TaskItemGuid,@ItemValue,@ItemRecordDate,@MaintainceUser,@MaintainceDate,@Remark,@ItemFactor,@RowGuid,@UniversalValue1,@UniversalValue2,@UniversalValue3,@UniversalValue4);");
                SqlParameter[] parameters = { 
                                new SqlParameter("@TaskGuid", SqlDbType.Int),
                                new SqlParameter("@TaskCode", SqlDbType.NVarChar,100),
                                new SqlParameter("@TaskItemGuid", SqlDbType.UniqueIdentifier),
                                new SqlParameter("@ItemValue", SqlDbType.NVarChar, 100),
                                new SqlParameter("@ItemRecordDate", SqlDbType.DateTime),
                                new SqlParameter("@MaintainceUser", SqlDbType.NVarChar,50),
                                new SqlParameter("@MaintainceDate", SqlDbType.DateTime),
                                new SqlParameter("@Remark", SqlDbType.NVarChar,200),
                                new SqlParameter("@ItemFactor", SqlDbType.NVarChar,50),
                                new SqlParameter("@RowGuid", SqlDbType.UniqueIdentifier),
                                new SqlParameter("@UniversalValue1", SqlDbType.NVarChar, 100),
                                new SqlParameter("@UniversalValue2", SqlDbType.NVarChar, 100),
                                new SqlParameter("@UniversalValue3", SqlDbType.NVarChar, 100),
                                new SqlParameter("@UniversalValue4", SqlDbType.NVarChar, 100)};
                parameters[0].Value = OM_TaskItemDatumEntityArry[i].TaskGuid;
                parameters[1].Value = OM_TaskItemDatumEntityArry[i].TaskCode;
                parameters[2].Value = OM_TaskItemDatumEntityArry[i].TaskItemGuid;
                parameters[3].Value = OM_TaskItemDatumEntityArry[i].ItemValue;
                parameters[4].Value = OM_TaskItemDatumEntityArry[i].ItemRecordDate;
                parameters[5].Value = OM_TaskItemDatumEntityArry[i].MaintainceUser;
                parameters[6].Value = OM_TaskItemDatumEntityArry[i].MaintainceDate;
                parameters[7].Value = OM_TaskItemDatumEntityArry[i].Remark;
                parameters[8].Value = OM_TaskItemDatumEntityArry[i].ItemFactor;
                parameters[9].Value = Guid.NewGuid();
                parameters[10].Value = OM_TaskItemDatumEntityArry[i].UniversalValue1;
                parameters[11].Value = OM_TaskItemDatumEntityArry[i].UniversalValue2;
                parameters[12].Value = OM_TaskItemDatumEntityArry[i].UniversalValue3;
                parameters[13].Value = OM_TaskItemDatumEntityArry[i].UniversalValue4;
                CommandInfo cmd = new CommandInfo(strSql.ToString(), parameters);
                sqllist.Add(cmd);
            }
            g_DatabaseHelper.ExecuteSqlTranWithIndentity(sqllist, connection);
        }
    }
}
