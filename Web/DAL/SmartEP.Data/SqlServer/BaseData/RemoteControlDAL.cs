using SmartEP.Core.Generic;
using SmartEP.Utilities.AdoData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.BaseData
{
    /// <summary>
    /// 名称：RemoteControlDAL.cs
    /// 创建人：樊垂贺
    /// 创建日期：2015-11-4
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：远程控制数据访问层
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class RemoteControlDAL
    {
        /// <summary>
        /// 数据库处理类
        /// </summary>
        DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();

        /// <summary>
        /// 获取远程控制命令类型
        /// </summary>
        /// <returns></returns>
        public DataView GetCommandType()
        {
            string Sql = "SELECT  distinct commandType from V_Command_Mode_Type where commandMode='请求命令' and isUseOrNot=1";
            return g_DatabaseHelper.ExecuteDataView(Sql, "AMS_BaseDataConnection");
        }

        /// <summary>
        /// 获取远程控制命令
        /// </summary>
        /// <returns></returns>
        public DataView GetCommandList()
        {
            string Sql = "SELECT commandType, commandNumber,ISNULL(LTRIM(STR(commandNumber)), '') + ISNULL(commandName, '') AS commandName from V_Command_Mode_Type  where commandMode='请求命令' and isUseOrNot=1 ORDER BY commandNumber";
            return g_DatabaseHelper.ExecuteDataView(Sql, "AMS_BaseDataConnection");
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * ");
            strSql.Append(" FROM RTD_OriginalPacketSend ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), "AMS_BaseDataConnection");
        }
    }
}
