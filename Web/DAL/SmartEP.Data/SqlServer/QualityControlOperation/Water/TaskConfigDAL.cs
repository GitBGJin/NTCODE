using SmartEP.Core.Generic;
using SmartEP.Data.Enums;
using SmartEP.Data.SqlServer.Common.GridView;
using SmartEP.Utilities.AdoData;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.QualityControlOperation.Water
{
    /// <summary>
    /// 名称：TaskConfigDAL.cs
    /// 创建人：王秉晟
    /// 创建日期：2015-10-14
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 任务配置数据库处理
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class TaskConfigDAL
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
        private string tableName = "dbo.TB_TaskConfig";
        #endregion

        #region << 构造函数 >>
        /// <summary>
        /// 构造函数
        /// </summary>
        public TaskConfigDAL()
        {
            //connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(ApplicationType.Water, PollutantDataType.Hour);
        }
        #endregion

        #region << 方法 >>
        /// <summary>
        /// 取得主表的虚拟分页查询数据和总行数
        /// </summary>
        /// <param name="taskIDs">任务ID</param>
        /// <param name="type">类别（气Air、水Water、所有null或空）</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetDataPager(string[] taskIDs, string type, int pageSize, int pageNo, out int recordTotal, string orderBy = "id")
        {
            recordTotal = 0;
            try
            {
                string taskIDsStr = StringExtensions.GetArrayStrNoEmpty(taskIDs.ToList<string>(), "','");
                string typeStr = string.Empty;
                string keyName = "RowGuid";
                string fieldName = "id,RowGuid,MissionID,MissionName,MissionUrl,Type,CreateDate,CreateUser,RowStatus";
                string where = string.Empty;//查询条件拼接

                if (taskIDs.Length == 1 && !string.IsNullOrEmpty(taskIDs[0]))
                {
                    taskIDsStr = " AND MissionID ='" + taskIDsStr + "'";
                }
                else if (!string.IsNullOrEmpty(taskIDsStr))
                {
                    taskIDsStr = " AND MissionID IN('" + taskIDsStr + "')";
                }
                if (!string.IsNullOrWhiteSpace(type))
                {
                    typeStr = string.Format(" AND  Type='{0}'", type);
                }

                orderBy = string.IsNullOrEmpty(orderBy) ? "id" : orderBy;
                where = string.Format(" 1=1 {0} {1} ", taskIDsStr, typeStr);
                return g_GridViewPager.GetGridViewPager(tableName, fieldName, keyName, pageSize, pageNo, orderBy, where, connection, out recordTotal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 取得主表导出数据
        /// </summary>
        /// <param name="taskIDs">任务ID</param>
        /// <param name="type">类别（Air气、Water水）</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public DataView GetExportData(string[] taskIDs, string type, string orderBy = "id")
        {
            try
            {
                StringBuilder sqlStringBuilder = new StringBuilder();
                string taskIDsStr = StringExtensions.GetArrayStrNoEmpty(taskIDs.ToList<string>(), "','");
                string typeStr = string.Empty;
                //string keyName = "id";
                string fieldName = "id,MissionID,MissionName,MissionUrl,Type,CreateDate,CreateUser";
                string where = string.Empty;//查询条件拼接

                if (taskIDs.Length == 1 && !string.IsNullOrEmpty(taskIDs[0]))
                {
                    taskIDsStr = " AND MissionID ='" + taskIDsStr + "'";
                }
                else if (!string.IsNullOrEmpty(taskIDsStr))
                {
                    taskIDsStr = " AND MissionID IN('" + taskIDsStr + "')";
                }
                if (!string.IsNullOrWhiteSpace(type))
                {
                    typeStr = string.Format(" AND  Type='{0}'", type);
                }
                orderBy = string.IsNullOrEmpty(orderBy) ? "id" : orderBy;
                where = string.Format(" 1=1 {0} {1} ", taskIDsStr, typeStr);
                sqlStringBuilder.AppendFormat(@"SELECT {0} FROM {1} WHERE {2} ORDER BY {3}", fieldName, tableName, where, orderBy);
                return g_DatabaseHelper.ExecuteDataView(sqlStringBuilder.ToString(), connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataTable GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,MissionID,MissionName,MissionUrl,Type,CreatUser,CreatDateTime ");
            strSql.Append(" FROM dbo.TB_TaskConfig ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), connection);
        }
        /// <summary>
        /// 获得MissionName
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataTable GetName(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select MissionName");
            strSql.Append(" FROM dbo.TB_TaskConfig ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where MissionId=" + strWhere);
            }
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), connection);
        }
        #endregion  Method
    }
}
