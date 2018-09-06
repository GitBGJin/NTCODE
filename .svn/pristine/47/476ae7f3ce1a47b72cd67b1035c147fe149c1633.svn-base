using SmartEP.Core.Generic;
using SmartEP.Utilities.AdoData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.MonitoringBusiness.Water
{
    /// <summary>
    /// 名称：SuperStationInterfaceDAL.cs
    /// 创建人：蒋月
    /// 创建日期：2017-2-22
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 水质自动监测站运行维护文件管理
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class MaintainFileManageDAL
    {
        #region << 变量 >>
        /// <summary>
        /// 数据库表名
        /// </summary>
        private string tableName = "dbo.TB_MaintainFileManage";
        /// <summary>
        /// 数据库处理类
        /// </summary>
        DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connection = "AMS_MonitoringBusinessConnection";
        #endregion

        #region << 构造函数 >>
        public MaintainFileManageDAL() { }
        #endregion

        #region << 方法 >>
        /// <summary>
        /// 获取水质自动监测站运行维护文件数据
        /// </summary>
        /// <returns></returns>
        public DataView GetMaintainFile(string startTime, string endTime)
        {
            try
            {
                string sql = string.Format(@"select * from {0} where CreateTime >= '{1}' and CreateTime <= '{2}'", tableName, startTime, endTime);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 获取水质自动监测站运行维护文件数据
        /// </summary>
        /// <returns></returns>
        public DataView GetMaintainFile(int id)
        {
            try
            {
                string sql = string.Format(@"select * from {0} where ID={1}", tableName, id);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 更新doc文件路径和html文件路径
        /// </summary>
        /// <param name="DocRoute"></param>
        /// <param name="HtmlRoute"></param>
        public void ModifyRoute(string DocRoute, string HtmlRoute, string IsUpload, int id)
        {
            try
            {
                string sql = string.Format(@"update {0} set DocRoute = '{1}', HtmlRoute = '{2}', IsUpload = '{3}' where ID = {4}", tableName, DocRoute, HtmlRoute, IsUpload, id);
                g_DatabaseHelper.ExecuteNonQuery(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="createTime"></param>
        /// <param name="docRoute"></param>
        /// <param name="htmlRoute"></param>
        /// <param name="isUpload"></param>
        /// <param name="person"></param>
        public void AddFile(string fileName, string createTime, string docRoute, string htmlRoute, string isUpload, string person)
        {
            try
            {
                string sql = string.Format(@"insert into {0} values('{1}','{2}','{3}','{4}','{5}','{6}')", tableName, fileName, createTime, docRoute, htmlRoute, isUpload, person);
                g_DatabaseHelper.ExecuteNonQuery(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }

        public void DeleteFile(int id)
        {
            try
            {
                string sql = string.Format(@"delete from {0} where ID={1}", tableName, id);
                g_DatabaseHelper.ExecuteNonQuery(sql, connection);
            }
            catch (Exception ex) { throw ex; }
        }

        #endregion
    }
}
