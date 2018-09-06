using SmartEP.Core.Generic;
using SmartEP.MonitoringBusinessRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.DataAnalyze.Water
{
    public class ReportContentService
    {
        /// <summary>
        /// 数据库处理类
        /// </summary>
        ReportContentRepository g_ReportContentRepository = Singleton<ReportContentRepository>.GetInstance();
        public void insertTable(Dictionary<string, string> pcontent, string[] ptitle, string pageid, DateTime starttime, DateTime endtime)
        {
            g_ReportContentRepository.insertTable(pcontent, ptitle, pageid, starttime, endtime);
        }
        #region 查询数据
        public DataView getList(string[] ptitle, DateTime dtbegion, DateTime dtend, string pageid, string name)
        {
            string strWhere = " 1=1 ";
            string strptitle = "'" + string.Join("','", ptitle) + "','" + name + "'";
            if (ptitle != null)
            {
                strWhere += string.Format(" and ptitle in ({0}) ", strptitle);
            }
            if (!string.IsNullOrWhiteSpace(pageid))
            {
                strWhere += string.Format(" and pageid ='{0}' ", pageid);
            }
            if (dtbegion != null)
            {
                strWhere += string.Format(" and starttime >='{0}' ", dtbegion.Date);
            }
            if (dtend != null)
            {
                strWhere += string.Format(" and endtime <='{0}' ", dtend.Date);
            }
            return g_ReportContentRepository.GetList(strWhere);
        }
        #endregion
    }
}
