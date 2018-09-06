using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.MonitoringBusinessRepository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.Service.DataAnalyze.Report
{
    /// <summary>
    /// 名称：ReportLogService.cs
    /// 创建人：樊垂贺
    /// 创建日期：2015-11-30
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 监测指标
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class ReportLogService
    {
        ReportLogRepository r_ReportLog = new ReportLogRepository();

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="reportLog">实体类对象</param>
        /// <returns></returns>
        public void ReportLogAdd(ReportLogEntity reportLog)
        {
            r_ReportLog.ReportLogAdd(reportLog);
        }

        /// <summary>
        /// 根据页面pageTypeID、waterOrAirType水或气的类型条件查询数据
        /// </summary>
        /// <param name="pageTypeID">页面ID</param>
        /// <param name="waterOrAirType">水或气的类型0：水，1：气</param>
        /// <returns></returns>
        public IQueryable<ReportLogEntity> CustomDatumRetrieve(string pageTypeID, int waterOrAirType)
        {
            return r_ReportLog.CustomDatumRetrieve(pageTypeID, waterOrAirType);
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="customDatum">实体类对象</param>
        public void ReportLogUpdate(object customDatum)
        {
            r_ReportLog.ReportLogUpdate(customDatum);
        }
        /// <summary>
        /// 根据主键id获取数据
        /// </summary>
        /// <param name="customID"></param>
        /// <returns></returns>
        public IQueryable<ReportLogEntity> ReportLogRetrieveByid(int id)
        {
            return r_ReportLog.ReportLogRetrieveByid(id);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="actionID">工作ID</param>
        /// <param name="pointId">测点Id</param>
        /// <param name="pollutantCode">因子代码</param>
        /// <param name="tstamp">时间戳</param>
        /// <returns></returns>

        public void Delete(int id)
        {
            ReportLogEntity model;
            model = r_ReportLog.RetrieveFirstOrDefault(p => p.Id == id);
            r_ReportLog.Delete(model);
        }
    }
}
