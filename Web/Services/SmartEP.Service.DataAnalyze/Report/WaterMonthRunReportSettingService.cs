using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartEP.MonitoringBusinessRepository.Common;
using SmartEP.DomainModel.MonitoringBusiness;

namespace SmartEP.Service.DataAnalyze.Report
{
    /// <summary>
    /// 名称：WaterMonthRunReportSettingService.cs
    /// 创建人：徐龙超
    /// 创建日期：2015-11-26
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：审核日数据
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class WaterMonthRunReportSettingService
    {
        WaterMonthRunReportSettingRepository reportRep = new WaterMonthRunReportSettingRepository();

        /// <summary>
        /// 根据时间获取运行月报配置信息
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public WaterMonthRunReportSettingEntity GetMonthReportSettingByTime(DateTime time)
        {
            WaterMonthRunReportSettingEntity setting = new WaterMonthRunReportSettingEntity();
            return reportRep.Retrieve(x => x.ReportTime == time).FirstOrDefault();
        }

        /// <summary>
        /// 更新报表配置
        /// </summary>
        /// <param name="time"></param>
        /// <param name="Portids"></param>
        /// <param name="factors"></param>
        /// <param name="User"></param>
        public void UpdateSeting(DateTime time, string Portids, string factors, string result, string memo, string User)
        {
            WaterMonthRunReportSettingEntity setting = reportRep.Retrieve(x => x.ReportTime == time).FirstOrDefault();
            if (setting != null)//更新
            {
                setting.PointIDs = Portids;
                setting.FactorCodes = factors;
                setting.Result = result;
                setting.Memo = memo;
                setting.UpdateDateTime = DateTime.Now;
                setting.UpdateUser = "";
                reportRep.Update(setting);
            }
            else           //插入
            {
                setting = new WaterMonthRunReportSettingEntity();
                setting.PointIDs = Portids;
                setting.FactorCodes = factors;
                setting.ReportTime = time;
                setting.Result = result;
                setting.Memo = memo;
                setting.CreatDateTime = DateTime.Now;
                setting.CreatUser = "";
                setting.UpdateDateTime = DateTime.Now;
                setting.UpdateUser = "";
                reportRep.Add(setting);
            }
        }

        /// <summary>
        /// 删除报表配置
        /// </summary>
        /// <param name="time"></param>
        public void DeleteSettingByTime(DateTime time)
        {
            IQueryable<WaterMonthRunReportSettingEntity> query = reportRep.Retrieve(x => x.ReportTime == time);
            reportRep.BatchDelete(query.ToList<WaterMonthRunReportSettingEntity>());
        }
    }
}
