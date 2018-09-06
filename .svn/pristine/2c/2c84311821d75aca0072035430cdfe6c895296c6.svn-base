using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.Data.SqlServer.Common.GridView;
using SmartEP.Utilities.AdoData;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.MonitoringBusiness.Water
{
    /// <summary>
    /// 名称：WeekReportDAL.cs
    /// 创建人：邱奇
    /// 创建日期：2015-12-14
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 水质自动监测月度小结数据处理
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class AutoMonitoringMonthSummaryDAL
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
        private string tableName = "TB_AutoMonitoringMonthSummary";
        #endregion

        #region << 构造函数 >>
        ///// <summary>
        ///// 构造函数
        ///// </summary>
        //public AutoMonitoringMonthSummaryDAL()
        //{
        //    connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(ApplicationType.Water, PollutantDataType.Hour);
        //}
        #endregion

        #region << 方法 >>
        /// <summary>
        /// 取得水质自动监测月度小结数据源
        /// </summary>
        /// <returns></returns>
        public DataView GetAllDataList(string recordGuid)
        {
            string sql = string.Format("select * from TB_AutoMonitoringMonthSummary where recordGuid = '{0}'",recordGuid);
            return g_DatabaseHelper.ExecuteDataView(sql, connection);
        }
        #endregion
    }
}
