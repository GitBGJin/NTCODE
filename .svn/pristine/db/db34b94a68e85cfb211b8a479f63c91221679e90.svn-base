using SmartEP.Core.Generic;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.MonitoringBusinessRepository.Water;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SmartEP.Service.ReportLibrary.Water
{
    /// <summary>
    /// 名称：AutoMonitoringMonthSummaryService.cs
    /// 创建人：朱佳伟
    /// 创建日期：2015-12-30
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：水质自动监测月度小结
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class AutoMonitoringMonthSummaryService
    {
        /// <summary>
        /// 数据接口
        /// </summary>
        AutoMonitoringMonthSummaryRepository g_AutoMonitoringMonthSummaryRepository = Singleton<AutoMonitoringMonthSummaryRepository>.GetInstance();

        /// <summary>
        /// 根据lambda表达式要求，返回所有对象
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IQueryable<AutoMonitoringMonthSummaryEntity> Retrieve(Expression<Func<AutoMonitoringMonthSummaryEntity, bool>> predicate)
        {
            return g_AutoMonitoringMonthSummaryRepository.Retrieve(predicate);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="DataList">实体数组</param>
        public void BatchAdd(List<AutoMonitoringMonthSummaryEntity> DataList)
        {
            g_AutoMonitoringMonthSummaryRepository.BatchAdd(DataList);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="Data">实体</param>
        public void Add(AutoMonitoringMonthSummaryEntity Data)
        {
            g_AutoMonitoringMonthSummaryRepository.Add(Data);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="DataList">实体数组</param>
        public void BatchDelete(List<AutoMonitoringMonthSummaryEntity> DataList)
        {
            g_AutoMonitoringMonthSummaryRepository.BatchDelete(DataList);
        }
    }
}
