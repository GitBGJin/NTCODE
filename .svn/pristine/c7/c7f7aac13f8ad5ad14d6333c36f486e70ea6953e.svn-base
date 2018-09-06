using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.MonitoringBusinessRepository.Air;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SmartEP.Service.DataAnalyze.Air.DataQuery
{
    /// <summary>
    /// 名称：DataQueryByHourForUpLoadService.cs
    /// 创建人：朱佳伟
    /// 创建日期：2016-01-18
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：上传审核数据到临时表
    /// 环境空气发布：
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class DataQueryByHourForUpLoadService
    {
        /// <summary>
        /// 接口
        /// </summary>
        HourReportForUpLoadRepository g_HourReportForUpLoadRepository = Singleton<HourReportForUpLoadRepository>.GetInstance();

        /// <summary>
        /// 根据lambda表达式要求，返回所有对象
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IQueryable<AirHourReportForUpLoadEntity> Retrieve(Expression<Func<AirHourReportForUpLoadEntity, bool>> predicate)
        {
            return g_HourReportForUpLoadRepository.Retrieve(predicate);
        }

        /// <summary>
        /// 获取用户上传的数据
        /// </summary>
        /// <param name="PointIds">监测点Id数组</param>
        /// <param name="PollutantCodes">监测因子Code数组</param>
        /// <param name="DataType">数据类型</param>
        /// <param name="UserGuid">上传用户Uid</param>
        /// <returns>DataTable</returns>
        public DataTable GetUpLoadData(List<int> PointIds, List<string> PollutantCodes, int DataType, string UserGuid)
        {
            return g_HourReportForUpLoadRepository.GetUpLoadData(PointIds, PollutantCodes, DataType, UserGuid);
        }

        /// <summary>
        /// 删除用户上传数据
        /// </summary>
        /// <param name="DataType">数据类型</param>
        /// <param name="UserGuid">上传用户Uid</param>
        public void DeleteByUser(int DataType, string UserGuid)
        {
            g_HourReportForUpLoadRepository.DeleteByUser(DataType, UserGuid);
        }

        /// <summary>
        /// 批量导入
        /// </summary>
        /// <param name="models">导入的数据实体数组</param>
        public void InsertAll(List<AirHourReportForUpLoadEntity> models)
        {
            g_HourReportForUpLoadRepository.InsertAll(models);
        }

        /// <summary>
        /// 执行存储过程，批量上报区县数据
        /// </summary>
        /// <param name="UserGuid">操作用户标识</param>
        /// <param name="DataType">数据类型</param>
        /// <param name="ApplicationUid">系统类型</param>
        public void BatchAddAirHourReport(string UserGuid, int DataType, ApplicationType Application)
        {
            string ApplicationUid = SmartEP.Core.Enums.EnumMapping.GetDesc(Application);
            g_HourReportForUpLoadRepository.BatchAddAirHourReport(UserGuid, DataType, ApplicationUid);
        }
    }
}
