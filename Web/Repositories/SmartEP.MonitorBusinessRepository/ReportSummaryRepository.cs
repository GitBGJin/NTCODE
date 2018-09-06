using SmartEP.Core.Generic;
using SmartEP.Data.SqlServer.MonitoringBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.MonitoringBusinessRepository
{
    /// <summary>
    /// 名称：ReportSummaryRepository.cs
    /// 创建人：樊垂贺
    /// 创建日期：2016-7-10
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 环境空气发布：月报汇总
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class ReportSummaryRepository
    {
        /// <summary>
        /// 数据库处理类
        /// </summary>
        ReportSummaryDAL g_ReportSummaryDAL = Singleton<ReportSummaryDAL>.GetInstance();
        /// <summary>
        /// 插入月报汇总数据
        /// </summary>
        /// <param name="dv"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="pointIds"></param>
        /// <param name="pageType"></param>
        public void insertTable(DataView dv, int year, int month, string[] pointIds, string pageType)
        {
            g_ReportSummaryDAL.insertTable(dv, year, month, pointIds, pageType);
        }
        /// <summary>
        /// 插入月报汇总数据
        /// </summary>
        /// <param name="dv"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="pageType"></param>
        public void insertItemTable(DataView dv, int year, int month, string pageType)
        {
            g_ReportSummaryDAL.insertItemTable(dv, year, month, pageType);
        }
        /// <summary>
        /// 查询月报汇总数据
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataView GetList(string strWhere, string[] factorcodes)
        {
            return g_ReportSummaryDAL.GetList(strWhere, factorcodes);
        }
        /// <summary>
        /// 查询月报汇总数据
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataView GetListItem(string strWhere)
        {
            return g_ReportSummaryDAL.GetListItem(strWhere);
        }
    }
}
