using SmartEP.BaseInfoRepository.Alarm;
using SmartEP.Core.Enums;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.BusinessRule;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.Frame;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.BaseData.Alarm
{
    /// <summary>
    /// 名称：OfflineAlarmService.cs
    /// 创建人：季柯
    /// 创建日期：2015-08-21
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 离线规则配置服务
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class OfflineAlarmService
    {
        #region << 变量 >>
        //离线配置服务
        OfflineSettingService offlineSettingService = new OfflineSettingService();
        //字典服务
        DictionaryService dictionaryService = new DictionaryService();
        //生成报警服务
        CreateAlarmService creatAlarmService = new CreateAlarmService();

         /// <summary>
        /// 应用程序值字典
        /// </summary>
        private ApplicationType _applicationType;
        /// <summary>
        /// 数据类型值字典
        /// </summary>
        private PollutantDataType _dataType;
        /// <summary>
        /// 应用程序Uid
        /// </summary>
        private string _applicationUid;
        /// <summary>
        /// 用途Uid
        /// </summary>
        private string _userForUid;
        /// <summary>
        /// 数据类型Uid
        /// </summary>
        private string _dataTypeUid;
        /// <summary>
        /// 报警时间Uid
        /// </summary>
        private string _alarmEventUid;

        #endregion
        #region << 构造函数 >>
        public OfflineAlarmService(ApplicationType applicationType, PollutantDataType dataType)
        {
            _applicationType = applicationType;
            _dataType = dataType;
            _applicationUid = SmartEP.Core.Enums.EnumMapping.GetApplicationValue(applicationType);
            _userForUid = dictionaryService.GetValueByText(Core.Enums.DictionaryType.AMS, "规则用途类型", "报警");
            _dataTypeUid = dictionaryService.GetValueByCode(Core.Enums.DictionaryType.AMS, "数据类型", dataType.ToString());
            _alarmEventUid = dictionaryService.GetValueByText(Core.Enums.DictionaryType.AMS, "报警类型", "数据缺失");
        }
        #endregion
        #region << 方法 >>
        public void CheckAlarm()
         {
             //获取离线配置信息
             IQueryable<OfflineSettingEntity> offlineSetEntities = offlineSettingService.RetrieveListByDataType(_applicationUid, _userForUid, _dataTypeUid);
             //获取配置下所有点位
             IQueryable<MonitoringPointEntity> pointEntities = offlineSettingService.RetrievePointList(_applicationUid, _userForUid, _dataTypeUid);
             //获取所有点位id的集合
             List<string> lstPointId = pointEntities.Select(p => p.PointId.ToString()).ToList();
             //获取每个点的最新数据时间
             DataView latestDataTimeView = creatAlarmService.GetLatestDataTime(_applicationType, _dataType, lstPointId);
             //循环点位，比较离线判定时间与实际断线时间
             OfflineSettingEntity entity;
             //判定离线分钟数
             int offlineSpan;
             DateTime recordDateTime;
             //最新数据时间到现在为止的间隔分钟数
             int dataTimeSpan;
             foreach (var point in pointEntities)
             {
                 latestDataTimeView.RowFilter = "PointId = " + point.PointId;
                 if (latestDataTimeView!=null)
                 {
                     recordDateTime = Convert.ToDateTime(latestDataTimeView.ToTable().Rows[0]["Tstamp"]);
                     entity = offlineSetEntities.FirstOrDefault(p => p.MonitoringPointUid == point.MonitoringPointUid);
                     //获取判定离线分钟数和实际离线分钟数
                     if (Int32.TryParse(entity.OffLineTimeSpan.ToString(), out offlineSpan) && Int32.TryParse(Math.Round((DateTime.Now - recordDateTime).TotalMinutes,0).ToString(),out dataTimeSpan))
                     {
                         //实际数据缺失分钟数大于离线判定分钟数,并且之前无重复报警，则触发离线报警
                         if (dataTimeSpan > offlineSpan && !creatAlarmService.IsExist(_alarmEventUid,_dataTypeUid,point.MonitoringPointUid,recordDateTime))
                         {
                             creatAlarmService.Add(new CreatAlarmEntity()
                             {
                                 AlarmUid = Guid.NewGuid().ToString(),
                                 ApplicationUid = _applicationUid,
                                 MonitoringPointUid = point.MonitoringPointUid,
                                 RecordDateTime = recordDateTime,
                                 AlarmEventUid = _alarmEventUid,
                                 AlarmGradeUid = entity.NotifyGradeUid,
                                 DataTypeUid = entity.DataTypeUid,
                                 Content = string.Format("{0}从{1}到{2}缺失数据", point.MonitoringPointName, recordDateTime.ToString("MM-dd HH:00"), DateTime.Now.ToString("MM-dd HH:00")),
                                 CreatUser = "system",
                                 CreatDateTime = DateTime.Now
                             });
                         }
                     }
                 }
             }
         }
        #endregion
    }
}
