using SmartEP.Core.Generic;
using SmartEP.Utilities.AdoData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.MonitoringBusiness.Air
{
    /// <summary>
    /// 名称：SuperStationInterfaceDAL.cs
    /// 创建人：蒋月
    /// 创建日期：2017-2-15
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 超级站接口
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class SuperStationInterfaceDAL
    {
        /// <summary>
        /// 数据库表名
        /// </summary>
        private string tableName = "dbo.TB_SuperStationInterface";
        /// <summary>
        /// 数据库处理类
        /// </summary>
        DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connection = "AMS_MonitoringBusinessConnection";

        public SuperStationInterfaceDAL() { }

        #region 获取超级站接口
        public DataView GetSSInterface()
        {
            try
            {
                string sql = string.Format(@"select * from {0}", tableName);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion

        #region 修改超级站接口密码
        public void ModifyPassword(string password)
        {
            try
            {
                string sql = string.Format(@"update {0} set password = '{1}'", tableName, password);
                g_DatabaseHelper.ExecuteNonQuery(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion

        #region 更新状态
        public void ModifyStatus(int communicateStatus, int operateStatus, string interfaceName)
        {
            try
            {
                string sql = string.Format(@"update {0} set CommunicateStatus = {1}, OperateStatus= {2} where InterfaceName='{3}'", tableName, communicateStatus, operateStatus, interfaceName);
                g_DatabaseHelper.ExecuteNonQuery(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion


    }
}
