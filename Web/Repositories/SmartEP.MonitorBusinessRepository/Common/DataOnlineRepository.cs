using SmartEP.Core.Enums;
using SmartEP.Data.SqlServer.MonitoringBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.MonitoringBusinessRepository.Common
{
    /// <summary>
    /// 名称：DataOnlineRepository.cs
    /// 创建人：窦曙健
    /// 创建日期：2016-5-8
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 数据在线
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class DataOnlineRepository
    {
        DataOnlineDAL dal = new DataOnlineDAL();

        public DataTable GetDataOnline(ApplicationType applicationType, string[] pointIds, PollutantDataType pollutantDataType)
        {
            return dal.GetDataOnline(applicationType, pointIds, pollutantDataType);
        }
        /// <summary>
        /// 获取站点在线情况
        /// </summary>
        /// <param name="applicationType">应用类型</param>
        /// <param name="pointIds">站点</param>
        /// <param name="factors">因子</param>
        /// <param name="pollutantDataType">枚举类型</param>
        /// <returns></returns>
        public DataTable GetOnlineInfo(ApplicationType applicationType, string[] pointIds, string[] factors, PollutantDataType pollutantDataType)
        {
            return dal.GetOnlineInfo(applicationType, pointIds, factors, pollutantDataType);
        }
        /// <summary>
        /// 获取站点在线情况
        /// </summary>
        /// <param name="InstrumentUids"></param>
        /// <param name="pollutantDataType"></param>
        /// <param name="online"></param>
        /// <returns></returns>
        public DataTable GetInstrumentOnlineInfo(string[] InstrumentUids, PollutantDataType pollutantDataType, string online)
        {
            return dal.GetInstrumentOnlineInfo(InstrumentUids, pollutantDataType, online);
        }
        /// <summary>
        /// 获得站点以及其所属运营商和在线情况
        /// </summary>
        /// <param name="applicationType"></param>
        /// <param name="portIds"></param>
        /// <param name="pollutantDataType"></param>
        /// <returns></returns>
        public DataTable GetOperatorOnlineInfo(ApplicationType applicationType, string[] portIds, PollutantDataType pollutantDataType)
        {
            return dal.GetOperatorOnlineInfo(applicationType, portIds, pollutantDataType);
        }
        /// <summary>
        /// 获取所有选中的站点所属的运营商
        /// </summary>
        /// <param name="applicationType"></param>
        /// <param name="portIds"></param>
        /// <param name="pollutantDataType"></param>
        /// <returns></returns>
        public DataTable GetOperatorInfo(ApplicationType applicationType, string[] portIds, PollutantDataType pollutantDataType)
        {
            return dal.GetOperatorInfo(applicationType, portIds, pollutantDataType);
        }
        /// <summary>
        /// 根据运营商的id去获取运营商的名字
        /// </summary>
        /// <param name="operas">运营商的id数组</param>
        /// <returns></returns>
        public DataTable GetOperatorName(string[] operas)
        {
            return dal.GetOperatorName(operas);
        }
        /// <summary>
        /// 获取仪器
        /// </summary>
        /// <returns></returns>
        public DataTable GetInstrumentInfo()
        {
            return dal.GetInstrumentInfo();
        }
    }
}
