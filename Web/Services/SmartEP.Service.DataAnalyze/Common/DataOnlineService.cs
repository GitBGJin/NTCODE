using SmartEP.Core.Enums;
using SmartEP.MonitoringBusinessRepository.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.DataAnalyze.Common
{
    /// <summary>
    /// 名称：DataOnlineService.cs
    /// 创建人：窦曙健
    /// 创建日期：2016-5-8
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 数据在线
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class DataOnlineService
    {
        DataOnlineRepository rep = new DataOnlineRepository();

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <param name="applicationType"></param>
        /// <param name="pointIds"></param>
        /// <returns></returns>
        public DataTable GetDataOnline(ApplicationType applicationType, string[] pointIds, PollutantDataType pollutantDataType)
        {
            return rep.GetDataOnline(applicationType, pointIds, pollutantDataType);
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
            return rep.GetOnlineInfo(applicationType, pointIds, factors, pollutantDataType);
        }

        /// <summary>
        /// 获取仪器在线情况
        /// </summary>
        /// <param name="InstrumentUids"></param>
        /// <param name="pollutantDataType"></param>
        /// <param name="Online"></param>
        /// <returns></returns>
        public DataTable GetInstrumentOnlineInfo(string[] InstrumentUids, PollutantDataType pollutantDataType, string Online)
        {
            return rep.GetInstrumentOnlineInfo(InstrumentUids, pollutantDataType, Online);
        }
        /// <summary>
        /// 获取在线数、离线数和总数
        /// </summary>
        /// <param name="applicationType"></param>
        /// <param name="pointIds"></param>
        /// <returns></returns>
        public DataTable GetTotalCount(ApplicationType applicationType, string[] pointIds, PollutantDataType pollutantDataType)
        {
            DataTable dt = GetDataOnline(applicationType, pointIds, pollutantDataType);

            DataTable dtNew = new DataTable();
            dtNew.Columns.Add("OnlineCount", typeof(string));
            dtNew.Columns.Add("OfflineCount", typeof(string));
            dtNew.Columns.Add("TotalCount", typeof(string));

            DataRow dr = dtNew.NewRow();
            dr["OnlineCount"] = dt.Select("IsOnline=1").Length.ToString();
            dr["OfflineCount"] = dt.Select("IsOnline=0").Length.ToString();
            dr["TotalCount"] = dt.Rows.Count.ToString();

            dtNew.Rows.Add(dr);

            return dtNew;
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
            return rep.GetOperatorOnlineInfo(applicationType, portIds, pollutantDataType);
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
            return rep.GetOperatorInfo(applicationType, portIds, pollutantDataType);
        }
        /// <summary>
        /// 根据运营商的id去获取运营商的名字
        /// </summary>
        /// <param name="operas">运营商的id数组</param>
        /// <returns></returns>
        public DataTable GetOperaterName(string[] operas)
        {
            return rep.GetOperatorName(operas);
        }
        /// <summary>
        /// 获取仪器
        /// </summary>
        /// <returns></returns>
        public DataTable GetInstrumentInfo()
        {
            return rep.GetInstrumentInfo();
        }
    }
}
