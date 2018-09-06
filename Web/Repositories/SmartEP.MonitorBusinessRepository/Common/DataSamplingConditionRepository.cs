using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.Data.SqlServer.MonitoringBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartEP.Core.Generic;
using SmartEP.Core.Enums;
using System.Data;

namespace SmartEP.MonitoringBusinessRepository.Common
{
    /// <summary>
    /// 名称：DataSamplingConditionRepository.cs
    /// 创建人：李飞
    /// 创建日期：2015-08-17
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 监测点数据采集情况(包括在线状况、采集情况等信息)
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class DataSamplingConditionRepository : BaseGenericRepository<MonitoringBusinessModel, DataSamplingConditionEntity>
    {
        /// <summary>
        /// 数据处理借口
        /// </summary>
        DataSamplingConditionDal g_DataSamplingConditionDal = Singleton<DataSamplingConditionDal>.GetInstance();

        /// <summary>
        /// 根据数采仪Guid判断记录是否存在
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public override bool IsExist(string strKey)
        {
            return RetrieveCount(x => x.AcquisitionUid.Equals(strKey)) == 0 ? false : true;
        }

        /// <summary>
        /// 获取测点在线状态实时统计数据
        /// </summary>
        /// <param name="applicationType">应用程序类型</param>
        /// <returns></returns>
        public DataTable GetSamplingConditionData(ApplicationType applicationType)
        {
            return g_DataSamplingConditionDal.GetSamplingConditionData(applicationType);
        }

        /// <summary>
        /// 获取测点离线状态信息
        /// </summary>
        /// <param name="pointIds">点位ID</param>
        /// <param name="applicationType">应用程序类型</param>
        /// <returns></returns>
        public DataTable GetOfflinePointInfo(string applicationUid, string[] pointIds)
        {
            return g_DataSamplingConditionDal.GetOfflinePointInfo(applicationUid, pointIds);
        }
    }
}
