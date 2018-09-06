using SmartEP.Core.Generic;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.MonitoringBusinessRepository.Water;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SmartEP.Service.DataAnalyze.Water
{
    /// <summary>
    /// 名称：BlueAlgaeEarlyWarningMonitoringService.cs
    /// 创建人：朱佳伟
    /// 创建日期：2015-12-29
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：蓝藻预警监测报告数据
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class BlueAlgaeEarlyWarningMonitoringService
    {
        /// <summary>
        /// 蓝藻预警监测报告数据接口
        /// </summary>
        BlueAlgaeEarlyWarningMonitoringRepository g_BlueAlgaeEarlyWarningMonitoringRepository = Singleton<BlueAlgaeEarlyWarningMonitoringRepository>.GetInstance();

        /// <summary>
        /// 根据lambda表达式要求，返回所有对象
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IQueryable<BlueAlgaeEarlyWarningMonitoringEntity> Retrieve(Expression<Func<BlueAlgaeEarlyWarningMonitoringEntity, bool>> predicate)
        {
            return g_BlueAlgaeEarlyWarningMonitoringRepository.Retrieve(predicate);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="DataList">实体数组</param>
        public void Add(BlueAlgaeEarlyWarningMonitoringEntity Data)
        {
            g_BlueAlgaeEarlyWarningMonitoringRepository.Add(Data);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="DataLists"></param>
        public void BatchDelete(List<BlueAlgaeEarlyWarningMonitoringEntity> DataLists)
        {
            g_BlueAlgaeEarlyWarningMonitoringRepository.BatchDelete(DataLists);
        }
    }
}
