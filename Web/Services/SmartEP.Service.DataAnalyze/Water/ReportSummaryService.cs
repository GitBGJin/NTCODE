using SmartEP.Core.Generic;
using SmartEP.MonitoringBusinessRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.DataAnalyze.Water
{
    /// <summary>
    /// 名称：ReportSummaryService.cs
    /// 创建人：樊垂贺
    /// 创建日期：2016-7-10
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 环境空气发布：月报汇总
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class ReportSummaryService
    {
        /// <summary>
        /// 数据库处理类
        /// </summary>
        ReportSummaryRepository g_ReportSummaryRepository = Singleton<ReportSummaryRepository>.GetInstance();
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
            g_ReportSummaryRepository.insertTable(dv, year, month, pointIds, pageType);
        }
        /// <summary>
        /// 插入月报汇总数据
        /// </summary>
        /// <param name="dv"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="pointIds"></param>
        /// <param name="pageType"></param>
        public void insertItemTable(DataView dv, int year, int month, string pageType)
        {
            g_ReportSummaryRepository.insertItemTable(dv, year, month, pageType);
        }
        public DataView getList(string PageType, string[] pointIds, DateTime startDate, DateTime endDate, string[] factorcodes)
        {
            string strWhere = " 1=1 ";
            string strPointId = string.Join(",", pointIds);
            if (pointIds != null)
            {
                strWhere += string.Format(" and PointId in ({0}) ", strPointId);
            }
            if (!string.IsNullOrWhiteSpace(PageType))
            {
                strWhere += string.Format(" and PageType ='{0}' ", PageType);
            }
            if (startDate != null)
            {
                strWhere += string.Format(" and Tstamp >='{0}' ", startDate.Date);
            }
            if (endDate != null)
            {
                strWhere += string.Format(" and Tstamp <='{0}' ", endDate.Date);
            }
            return g_ReportSummaryRepository.GetList(strWhere, factorcodes);
        }
        public DataView getListItem(string PageType, DateTime startDate, DateTime endDate)
        {
            string strWhere = " 1=1 ";
            if (!string.IsNullOrWhiteSpace(PageType))
            {
                strWhere += string.Format(" and PageType ='{0}' ", PageType);
            }
            if (startDate != null)
            {
                strWhere += string.Format(" and Tstamp >='{0}' ", startDate.Date);
            }
            if (endDate != null)
            {
                strWhere += string.Format(" and Tstamp <='{0}' ", endDate.Date);
            }
            return g_ReportSummaryRepository.GetListItem(strWhere);
        }
    }
}
