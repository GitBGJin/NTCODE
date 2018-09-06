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
    /// 名称：ReportContentRepository.cs
    /// 创建人：樊垂贺
    /// 创建日期：2016-2-28
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 环境空气发布：报表内容管理
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class ReportContentRepository
    {

        /// <summary>
        /// 数据库处理类
        /// </summary>
        ReportContentDAL g_ReportContentDAL = Singleton<ReportContentDAL>.GetInstance();
        public void insertTable(Dictionary<string, string> pcontent, string[] ptitle, string pageid, DateTime starttime, DateTime endtime)
        {

            g_ReportContentDAL.insertTable(pcontent, ptitle, pageid, starttime, endtime);
        }
        #region 查询数据
        public DataView GetList(string strWhere)
        {
            return g_ReportContentDAL.GetList(strWhere);
        }
        #endregion
    }
}
