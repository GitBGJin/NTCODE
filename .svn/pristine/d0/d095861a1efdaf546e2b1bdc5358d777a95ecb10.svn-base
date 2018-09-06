using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Data.Enums;
using SmartEP.Utilities.AdoData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.MonitoringBusiness
{
    /// <summary>
    /// 名称：DataSamplingConditionDal.cs
    /// 创建人：朱佳伟
    /// 创建日期：2015-12-11
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：测点实时在线统计
    /// 环境发布：
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class DataSamplingConditionDal
    {
        #region 变量
        /// <summary>
        /// 数据库处理类
        /// </summary>
        DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.MonitoringBusiness);
        #endregion

        #region 方法
        /// <summary>
        /// 获取测点在线状态实时统计数据
        /// </summary>
        /// <param name="applicationType">应用程序类型</param>
        /// <returns></returns>
        public DataTable GetSamplingConditionData(ApplicationType applicationType)
        {
            string ApplicationUid = SmartEP.Core.Enums.EnumMapping.GetDesc(applicationType);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * ");
            //strSql.Append(",ROUND(onLineCount*100/allCount,0) as onLineRate ");
            strSql.Append(",(case allCount when 0 then 0 else ROUND(onLineCount*100/allCount,0) end) as onLineRate ");
            strSql.Append("from dbo.GetRealTime_DataSamplingCondition('" + ApplicationUid + "') ");
            strSql.Append("order by SortNumber desc");
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), connection);
        }

        /// <summary>
        /// 获取测点离线状态信息
        /// </summary>
        /// <param name="pointIds">点位ID</param>
        /// <param name="applicationType">应用程序类型</param>
        /// <returns></returns>
        public DataTable GetOfflinePointInfo(string applicationUid, string[] pointIds)
        {
            string pointId = string.Empty;
            foreach (string id in pointIds)
            {
                pointId += id + ",";
            }
            pointId = pointId.Trim(',');
            string strSql = string.Format(@"select p.monitoringpointname,v.itemtext,v.SortNumber,* from dbo.TB_DataSamplingCondition as d
                                        inner join dbo.SY_AcquisitionInstrument as a on a.AcquisitionUid = d.AcquisitionUid
                                        inner join dbo.SY_MonitoringPoint as p on p.MonitoringpointUid = a.monitoringpointUid
                                        inner join dbo.SY_View_CodeMainItem as v on v.itemGuid = p.ContrlUid
                                        where d.ApplicationUid = '{1}'
                                        and StatusCode = 0
                                        and p.PointId in ({0}) order by v.SortNumber desc", pointId, applicationUid);
            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), connection);
        }
        #endregion
    }
}
