using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.Data.SqlServer.MonitoringBusiness.Air;
using SmartEP.DomainModel.MonitoringBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.MonitoringBusinessRepository.Air
{
    /// <summary>
    /// 名称：HourReportForUpLoadRepository.cs
    /// 创建人：朱佳伟
    /// 创建日期：2016-01-18
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：上传审核数据到临时表
    /// 环境空气发布：
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class HourReportForUpLoadRepository : BaseGenericRepository<MonitoringBusinessModel, AirHourReportForUpLoadEntity>
    {
        /// <summary>
        /// 数据处理接口
        /// </summary>
        HourReportForUpLoadDAL g_HourReportForUpLoadDAL = Singleton<HourReportForUpLoadDAL>.GetInstance();

        public override bool IsExist(string strKey)
        {
            throw new NotImplementedException();
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
            return g_HourReportForUpLoadDAL.GetUpLoadData(PointIds, PollutantCodes, DataType, UserGuid);
        }

        /// <summary>
        /// 删除用户上传数据
        /// </summary>
        /// <param name="DataType">数据类型</param>
        /// <param name="UserGuid">上传用户Uid</param>
        public void DeleteByUser(int DataType, string UserGuid)
        {
            g_HourReportForUpLoadDAL.DeleteByUser(DataType, UserGuid);
        }

        /// <summary>
        /// 批量导入
        /// </summary>
        /// <param name="models">导入的数据实体数组</param>
        public void InsertAll(List<AirHourReportForUpLoadEntity> models)
        {
            g_HourReportForUpLoadDAL.InsertAll(models);
        }

        /// <summary>
        /// 执行存储过程，批量上报区县数据
        /// </summary>
        /// <param name="UserGuid">操作用户标识</param>
        /// <param name="DataType">数据类型</param>
        /// <param name="ApplicationUid">系统Uid</param>
        public void BatchAddAirHourReport(string UserGuid, int DataType, string ApplicationUid)
        {
            g_HourReportForUpLoadDAL.BatchAddAirHourReport(UserGuid, DataType, ApplicationUid);
        }
    }
}
