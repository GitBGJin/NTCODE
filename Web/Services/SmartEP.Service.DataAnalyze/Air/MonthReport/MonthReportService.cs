using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SmartEP.MonitoringBusinessRepository.Air;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Core.Generic;
using SmartEP.Service.Frame;
using SmartEP.Service.DataAuditing.AuditBaseInfo;
using SmartEP.Service.Core.Enums;

namespace SmartEP.Service.DataAnalyze.Air.MonthReport
{
    /// <summary>
    /// 名称：MonthReportService.cs
    /// 创建人：李飞
    /// 创建日期：2015-08-22
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 月报服务层
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class MonthReportService
    {
        private MonthReportRepository monthReportRep = new MonthReportRepository();
        MonitoringPointAirService g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
        DictionaryService dicService = new DictionaryService();
        AuditMonitoringPointPollutantService auditPollutant = new AuditMonitoringPointPollutantService();
        /// <summary>
        /// 按月统计站点月报数据有效率
        /// </summary>
        /// <param name="portIds">站点列表</param>
        /// <param name="factorCodes">因子编码列表</param>
        /// <param name="startTime">开始时间(格式：yyyy-MM)</param>
        /// <param name="endTime">结束时间(格式：yyyy-MM)</param>
        /// <returns>PointName、Tstamp(yyyy-MM)、Factor_QualifiedNum、Factor_QualifiedRate</returns>
        public DataTable GetQualifiedRate(string[] portIds, string[] factorCodes, string startTime, string endTime)
        {
            return monthReportRep.GetQualifiedRate(portIds, factorCodes, startTime, endTime);
        }

        /// <summary>
        /// 获取监测点概况
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public DataTable GetPointGeneral(string[] portIds, string startTime, string endTime)
        {
            DataTable dt = monthReportRep.GetPointGeneral(portIds, startTime, endTime);
            dt.Columns.Add("region");
            dt.Columns.Add("typeName");
            dt.Columns.Add("factors");
            foreach (DataRow row in dt.Rows)
            {
                MonitoringPointExtensionForEQMSAirEntity extensionEntity = g_MonitoringPointAir.RetrieveAirExtensionPointListByPointUids(row["MonitoringPointUid"].ToString().Split(';')).FirstOrDefault();
                string region = dicService.GetTextByGuid(DictionaryType.AMS, "行政区划", row["RegionUid"].ToString());
                string typeName = dicService.GetTextByGuid(DictionaryType.Air, "空气站点属性类型", row["ContrlUid"].ToString());
                row["PointName"] = row["PointName"].ToString() + "(" + extensionEntity.Stcode + ")";
                row["region"] = region;
                row["typeName"] = typeName;
                string[] factors = auditPollutant.RetrievePollutantListByPointUid(row["MonitoringPointUid"].ToString()).Select(x => x.PollutantName).ToArray();
                if (factors != null)
                {                   
                    row["factors"] = string.Join("、", factors);
                }
                row["content"] = row["content"]!=DBNull.Value?row["content"].ToString().Replace("|","\r\n\r\n"):"";
            }
            return dt;
        }


    }
}
