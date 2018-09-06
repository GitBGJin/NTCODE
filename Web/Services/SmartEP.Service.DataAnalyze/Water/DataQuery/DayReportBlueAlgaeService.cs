using SmartEP.Core.Generic;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.MonitoringBusinessRepository.Water;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SmartEP.Service.DataAnalyze.Water.DataQuery
{
    /// <summary>
    /// 名称：DayReportBlueAlgaeService.cs
    /// 创建人：朱佳伟
    /// 创建日期：2015-12-28
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：蓝藻预警监测人工点数据采集
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class DayReportBlueAlgaeService
    {
        /// <summary>
        /// 人工点日数据
        /// </summary>
        DayReportBlueAlgaeRepository g_DayReportBlueAlgaeRepository = Singleton<DayReportBlueAlgaeRepository>.GetInstance();

        /// <summary>
        /// 根据lambda表达式要求，返回所有对象
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IQueryable<DayReportBlueAlgaeEntity> Retrieve(Expression<Func<DayReportBlueAlgaeEntity, bool>> predicate)
        {
            return g_DayReportBlueAlgaeRepository.Retrieve(predicate);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="DataList">实体数组</param>
        public void BatchAdd(List<DayReportBlueAlgaeEntity> DataList)
        {
            g_DayReportBlueAlgaeRepository.BatchAdd(DataList);
        }
    }
}
