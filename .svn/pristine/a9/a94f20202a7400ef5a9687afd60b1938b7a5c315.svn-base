using SmartEP.Core.Enums;
using SmartEP.Core.Interfaces;
using SmartEP.Data.SqlServer.BaseData;
using SmartEP.DomainModel.BaseData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.BaseInfoRepository.Alarm
{
    public class CreatAlarmRepository : BaseGenericRepository<BaseDataModel, CreatAlarmEntity>
    {
        public override bool IsExist(string strKey)
        {
            return Retrieve(x => x.AlarmUid.Equals(strKey)).Count() == 0 ? false : true;
        }

        #region << ADO.NET >>

        AlarmDAL m_AlarmDAL = new AlarmDAL();
        public DataView GetLatestData(ApplicationType applicationType, PollutantDataType pollutantDataType, string portId, string factorCode)
        {
            return m_AlarmDAL.GetLatestData(applicationType, pollutantDataType, portId, factorCode);
        }

        public DataView GetLatestData(ApplicationType applicationType, PollutantDataType pollutantDataType, List<string> listPortIds, List<string> listFactorCodes)
        {
            return m_AlarmDAL.GetLatestData(applicationType, pollutantDataType, listPortIds, listFactorCodes);
        }

        public DataView GetLatestDataTime(ApplicationType applicationType, PollutantDataType pollutantDataType, List<string> portIds)
        {
            return m_AlarmDAL.GetLatestDataTime(applicationType, pollutantDataType, portIds);
        }

        public DataView GetCompareBeforeData(ApplicationType applicationType, PollutantDataType pollutantDataType, string portId, DateTime tstamp, string factorCode, int compareBeforeGroups, bool isContainThisRecord)
        {
            return m_AlarmDAL.GetCompareBeforeData(applicationType, pollutantDataType, portId, tstamp, factorCode, compareBeforeGroups, isContainThisRecord);
        }

        public bool IsRepeatData(ApplicationType applicationType, PollutantDataType pollutantDataType, string portId, DateTime tstamp, string factorCode, int repeatNumber)
        {
            return m_AlarmDAL.IsRepeatData(applicationType, pollutantDataType, portId, tstamp, factorCode, repeatNumber);
        }

        /// <summary>
        /// 取得虚拟分页数据和总行数
        /// </summary>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="where">WHERE条件</param>
        /// <param name="recordTotal">数据总行数</param>
        /// <returns></returns>
        public DataView GetGridViewPager(int pageSize, int pageNo, string where, out int recordTotal)
        {
            return m_AlarmDAL.GetGridViewPager(pageSize, pageNo, where, out recordTotal);
        }

        /// <summary>
        /// 取得导出报警信息
        /// </summary>
        /// <param name="where">查询条件，不带where</param>
        /// <param name="orderBy">排序，不带order by</param>
        /// <returns></returns>
        public DataView GetExportData(string where, string orderBy)
        {
            return m_AlarmDAL.GetExportData(where, orderBy);
        }
        #endregion
        /// <summary>
        /// 根据用户名获取联系方式
        /// </summary>
        /// <param name="Name">用户名</param>
        /// <returns></returns>
        public DataView GetNumberByName(string Name)
        {
            return m_AlarmDAL.GetNumberByName(Name);
        }



        /// <summary>
        /// 批量查询报警信息  by  zhuwei  2016-7-14
        /// </summary>
        /// <returns></returns>
        public DataView GetPLAlarmInfo(string applicationUid, string monitoringPointUid, string alarmEventUid, string itemName, string pLHandleid, DateTime? recordDateTime)
        {
            return m_AlarmDAL.GetPLAlarmInfo(applicationUid, monitoringPointUid, alarmEventUid, itemName, pLHandleid,recordDateTime);
        }

        /// <summary>
        /// 批量查询报警信息  by  zhuwei  2016-7-14
        /// </summary>
        /// <returns></returns>
        public DataView GetAuditPLAlarmInfo(string applicationUid, string monitoringPointUid, string alarmEventUid, string itemName, string pLHandleid, DateTime? recordDateTime)
        {
            return m_AlarmDAL.GetAuditPLAlarmInfo(applicationUid, monitoringPointUid, alarmEventUid, itemName, pLHandleid, recordDateTime);
        }
    }
}
