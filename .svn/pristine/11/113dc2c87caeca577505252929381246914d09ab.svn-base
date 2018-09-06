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
    /// 名称：DrySeasonReportService.cs
    /// 创建人：朱佳伟
    /// 创建日期：2015-12-23
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：枯水期监测快报相关数据
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class DrySeasonReportService
    {
        /// <summary>
        /// 枯水期监测快报数据接口
        /// </summary>
        DrySeasonReportRepository g_DrySeasonReportRepository = Singleton<DrySeasonReportRepository>.GetInstance();

        /// <summary>
        /// 根据lambda表达式要求，返回所有对象
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IQueryable<DrySeasonReportEntity> Retrieve(Expression<Func<DrySeasonReportEntity, bool>> predicate)
        {
            return g_DrySeasonReportRepository.Retrieve(predicate);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="data">数据实体</param>
        public void Add(DrySeasonReportEntity data)
        {
            g_DrySeasonReportRepository.Add(data);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="DataLists">实体数组</param>
        public void BatchDelete(List<DrySeasonReportEntity> DataLists)
        {
            g_DrySeasonReportRepository.BatchDelete(DataLists);
        }
    }
}
