using SmartEP.Core.Generic;
using SmartEP.DomainModel;
using SmartEP.BaseInfoRepository.BusinessRule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using SmartEP.DomainModel.BaseData;
using SmartEP.Core.Enums;
using SmartEP.BaseInfoRepository.Alarm;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Service.Core.Enums;
using SmartEP.Service.Frame;

namespace SmartEP.Service.BaseData.Alarm
{
    /// <summary>
    /// 名称：CreateAlarmService.cs
    /// 创建人：邱奇
    /// 创建日期：2015-08-20
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：提供报警信息存储服务
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class CreateAlarmService
    {
        CreatAlarmRepository g_Repository = new CreatAlarmRepository();
        #region 增删改
        /// <summary>
        /// 增加报警信息
        /// </summary>
        /// <param name="creatAlarm">实体</param>
        public void Add(CreatAlarmEntity creatAlarm)
        {
            g_Repository.Add(creatAlarm);
        }

        /// <summary>
        /// 更新报警信息
        /// </summary>
        /// <param name="creatAlarm">实体</param>
        public void Update(CreatAlarmEntity creatAlarm)
        {
            g_Repository.Update(creatAlarm);
        }

        /// <summary>
        /// 删除报警信息
        /// </summary>
        /// <param name="creatAlarm">实体</param>
        public void Delete(CreatAlarmEntity creatAlarm)
        {
            g_Repository.Delete(creatAlarm);
        }
        #endregion

        /// <summary>
        /// 查询指定报警信息
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public CreatAlarmEntity RetrieveFirstOrDefault(Expression<Func<CreatAlarmEntity, bool>> predicate)
        {
            return g_Repository.RetrieveFirstOrDefault(predicate);
        }

        //<summary>
        //报警信息查询（报警源Uid、报警事件Uid、报警等级Uid）
        //</summary>
        //<param name="alarmSourceUid">报警源Uid</param>
        //<param name="alarmEventUid">报警事件Uid</param>
        //<param name="alarmGradeUid">报警等级Uid</param>
        //<param name="applicationUid">应用程序Uid</param>
        //<returns></returns>
        public IQueryable<CreatAlarmEntity> RetrieveList(string alarmSourceUid, string alarmEventUid, string alarmGradeUid, string dataTypeUid, string applicationUid)
        {
            IQueryable<CreatAlarmEntity> creatAlarmEntity = g_Repository.RetrieveAll();
            if (!string.IsNullOrEmpty(alarmSourceUid)) creatAlarmEntity = creatAlarmEntity.Where(p => p.AlarmSourceUid == alarmSourceUid);
            if (!string.IsNullOrEmpty(alarmEventUid)) creatAlarmEntity = creatAlarmEntity.Where(p => p.AlarmEventUid == alarmEventUid);
            if (!string.IsNullOrEmpty(alarmGradeUid)) creatAlarmEntity = creatAlarmEntity.Where(p => p.AlarmGradeUid == alarmGradeUid);
            if (!string.IsNullOrEmpty(alarmGradeUid)) creatAlarmEntity = creatAlarmEntity.Where(p => p.ApplicationUid == applicationUid);
            if (!string.IsNullOrEmpty(alarmGradeUid)) creatAlarmEntity = creatAlarmEntity.Where(p => p.DataTypeUid == dataTypeUid);

            return creatAlarmEntity;
        }

        //<summary>
        //获取当前5分钟之内产生的最新报警信息查询（报警事件Uid、时间类型Uid，应用程序Uid，点位Guid数组）
        //</summary>
        //<param name="alarmSourceUid">报警源Uid</param>
        //<param name="alarmEventUid">报警事件Uid</param>
        //<param name="alarmGradeUid">报警等级Uid</param>
        //<param name="applicationUid">应用程序Uid</param>
        //<returns></returns>
        public IQueryable<CreatAlarmEntity> RetrieveList(string alarmEventUid, string dataTypeUid, string applicationUid, string[] pointGuids)
        {
            IQueryable<CreatAlarmEntity> creatAlarmEntity = g_Repository.Retrieve(p => p.AlarmEventUid == alarmEventUid && p.DataTypeUid == dataTypeUid && p.ApplicationUid == applicationUid && pointGuids.Contains(p.MonitoringPointUid) && p.CreatDateTime > DateTime.Now.AddMinutes(-5));
            return creatAlarmEntity;
        }

        /// <summary>
        /// 判断相同报警是否存在
        /// </summary>
        /// <param name="alarmEventUid">报警事件Uid</param>
        /// <param name="dataTypeUid">数据类型Uid</param>
        /// <param name="monitoringPointUid">站点Uid</param>
        /// <param name="tstamp">时间</param>
        /// <returns></returns>
        public bool IsExist(string alarmEventUid, string dataTypeUid, string monitoringPointUid, DateTime tstamp)
        {
            IQueryable<CreatAlarmEntity> creatAlarmEntity = g_Repository.Retrieve(p => p.AlarmEventUid == alarmEventUid && p.DataTypeUid == dataTypeUid && p.MonitoringPointUid == monitoringPointUid && p.RecordDateTime == tstamp);
            return creatAlarmEntity.Count() > 0 ? true : false;

        }

        /// <summary>
        /// 判断相同报警是否存在
        /// </summary>
        /// <param name="alarmEventUid">报警事件Uid</param>
        /// <param name="dataTypeUid">数据类型Uid</param>
        /// <param name="monitoringPointUid">站点Uid</param>
        /// <param name="tstamp">时间</param>
        /// <param name="factorCode">因子代码</param>
        /// <returns></returns>
        public bool IsExist(string alarmEventUid, string dataTypeUid, string monitoringPointUid, DateTime tstamp, string factorCode)
        {
            IQueryable<CreatAlarmEntity> creatAlarmEntity = g_Repository.Retrieve(p => p.AlarmEventUid == alarmEventUid && p.DataTypeUid == dataTypeUid && p.MonitoringPointUid == monitoringPointUid && p.ItemName == factorCode && p.RecordDateTime == tstamp);
            return creatAlarmEntity.Count() > 0 ? true : false;

        }

        #region << ADO.NET >>

        public DataView GetLatestData(ApplicationType applicationType, PollutantDataType pollutantDataType, string portId, string factorCode)
        {
            return g_Repository.GetLatestData(applicationType, pollutantDataType, portId, factorCode);
        }

        public DataView GetLatestData(ApplicationType applicationType, PollutantDataType pollutantDataType, List<string> listPortIds, List<string> listFactorCodes)
        {
            return g_Repository.GetLatestData(applicationType, pollutantDataType, listPortIds, listFactorCodes);
        }

        public DataView GetLatestDataTime(ApplicationType applicationType, PollutantDataType pollutantDataType, List<string> portIds)
        {
            return g_Repository.GetLatestDataTime(applicationType, pollutantDataType, portIds);
        }

        public DataView GetCompareBeforeData(ApplicationType applicationType, PollutantDataType pollutantDataType, string portId, DateTime tstamp, string factorCode, int compareBeforeGroups, bool isContainThisRecord)
        {
            return g_Repository.GetCompareBeforeData(applicationType, pollutantDataType, portId, tstamp, factorCode, compareBeforeGroups, isContainThisRecord);
        }

        public bool IsRepeatData(ApplicationType applicationType, PollutantDataType pollutantDataType, string portId, DateTime tstamp, string factorCode, int repeatNumber)
        {
            return g_Repository.IsRepeatData(applicationType, pollutantDataType, portId, tstamp, factorCode, repeatNumber);
        }

        /// <summary>
        /// 取得虚拟分页数据和总行数
        /// </summary>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="whereString">WHERE条件</param>
        /// <param name="recordTotal">数据总行数</param>
        /// <returns></returns>
        public DataView GetGridViewPager(int pageSize, int pageNo, string whereString, out int recordTotal)
        {
            return g_Repository.GetGridViewPager(pageSize, pageNo, whereString, out recordTotal);
        }

        /// <summary>
        /// 取得导出报警信息
        /// </summary>
        /// <param name="where">查询条件，不带where</param>
        /// <param name="orderBy">排序，不带order by</param>
        /// <returns></returns>
        public DataView GetExportData(string where, string orderBy)
        {
            return g_Repository.GetExportData(where, orderBy);
        }
        #endregion

        public DataTable GetAlarmInfo(string[] portIds, DateTime dtpBegin, DateTime dtpEnd, string ApplicationType, string[] Alarmtype)
        {
            DictionaryService g_dicService = Singleton<DictionaryService>.GetInstance();
            IQueryable<V_CodeMainItemEntity> alarmTypeEntites = g_dicService.RetrieveList(DictionaryType.AMS, "报警类型");
            DataTable dt = new DataTable();
            dt.Columns.Add("AlarmType", typeof(string));
            dt.Columns.Add("AlarmTotal", typeof(decimal));
            string where = string.Empty;
            int pageSize = int.MaxValue;
            int pageNo = 1;
            int recordTotal = 0;
            where = string.Format(" ApplicationUid = '{0}' ", ApplicationType);
            if (portIds.Length > 0)
            {
                where += string.Format(" AND MonitoringPointUid in ('{0}')", StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), "','"));
            }
            //报警类型
            if (Alarmtype.Length > 0)
            {
                string alarmTypes = StringExtensions.GetArrayStrNoEmpty(Alarmtype.ToList<string>(), "','");
                where += string.Format(" AND AlarmEventUid in ('{0}')", alarmTypes);
            }
            //时间范围
            if (dtpBegin != null)
                where += string.Format(" AND RecordDateTime >= '{0}'", dtpBegin.ToString("yyyy-MM-dd HH:mm:ss"));
            if (dtpEnd != null)
                where += string.Format(" AND RecordDateTime <= '{0}'", dtpEnd.ToString("yyyy-MM-dd HH:mm:ss"));

            DataView dv = GetGridViewPager(pageSize, pageNo, where, out recordTotal);
            if (dv != null)
            {
                foreach (string type in Alarmtype)
                {
                    DataRow drNew = dt.NewRow();
                    dv.RowFilter = "AlarmEventUid='" + type + "'";
                    drNew["AlarmType"] = alarmTypeEntites.Where(x => x.ItemGuid == type).Select(t => t.ItemText).FirstOrDefault();
                    drNew["AlarmTotal"] = dv.Count;
                    dt.Rows.Add(drNew);
                }
            }
            else
            {
                foreach (string type in Alarmtype)
                {
                    DataRow drNew = dt.NewRow();
                    drNew["AlarmType"] = alarmTypeEntites.Where(x => x.ItemGuid == type).Select(t => t.ItemText).FirstOrDefault();
                    drNew["AlarmTotal"] = 0;
                    dt.Rows.Add(drNew);
                }
            }
            return dt;
        }



        /// <summary>
        /// 批量查询报警信息  by  zhuwei  2016-7-14
        /// </summary>
        /// <returns></returns>
        public DataView GetPLAlarmInfo(string applicationUid, string monitoringPointUid, string alarmEventUid, string itemName, string pLHandleid, DateTime? recordDateTime)
        {
            return g_Repository.GetPLAlarmInfo(applicationUid, monitoringPointUid, alarmEventUid, itemName, pLHandleid,recordDateTime);
        }

        /// <summary>
        /// 批量查询审核的报警信息  by  zhuwei  2016-7-14
        /// </summary>
        /// <returns></returns>
        public DataView GetAuditPLAlarmInfo(string applicationUid, string monitoringPointUid, string alarmEventUid, string itemName, string pLHandleid, DateTime? recordDateTime)
        {
            return g_Repository.GetAuditPLAlarmInfo(applicationUid, monitoringPointUid, alarmEventUid, itemName, pLHandleid, recordDateTime);
        }
    }
}
