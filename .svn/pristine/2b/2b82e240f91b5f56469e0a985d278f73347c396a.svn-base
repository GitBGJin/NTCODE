using SmartEP.Core.Interfaces;
using SmartEP.Data.SqlServer.MonitoringBusiness.Air;
using SmartEP.DomainModel.AirAutoMonitoring;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.AMSRepository.Air
{
    /// <summary>
    /// 名称：AirCalibrationRepository.cs
    /// 创建人：李飞
    /// 创建日期：2015-08-17
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 校准记录仓储层
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class AirCalibrationRepository : BaseGenericRepository<AirAutoMonitoringModel, AirCalibrationEntity>
    {
        /// <summary>
        /// 校准数据接口
        /// </summary>
        AirCalibrationDAL g_AirCalibrationDAL = new AirCalibrationDAL();

        /// <summary>
        /// 根据主键Key判断记录是否存在
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public override bool IsExist(string strKey)
        {
            return RetrieveCount(x => x.CalibrationGuid.Equals(strKey)) == 0 ? false : true;
        }

        /// <summary>
        /// 校准数据
        /// </summary>
        /// <param name="PointIds">测点Id</param>
        /// <param name="StartDate">开始日期</param>
        /// <param name="EndDate">截止日期</param>
        /// <param name="CalTypeCodes">校准类型</param>
        /// <returns></returns>
        public DataTable GetData(List<int> PointIds, DateTime StartDate, DateTime EndDate, List<string> CalTypeCodes)
        {
            return g_AirCalibrationDAL.GetData(PointIds, StartDate, EndDate, CalTypeCodes);
        }
    }
}
