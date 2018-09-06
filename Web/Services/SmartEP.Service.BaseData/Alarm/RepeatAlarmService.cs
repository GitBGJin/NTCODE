using SmartEP.Core.Enums;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.BusinessRule;
using SmartEP.Service.BaseData.Channel;
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
    /// 名称：RepeatAlarmService.cs
    /// 创建人：季柯
    /// 创建日期：2015-08-20
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 重复报警服务
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class RepeatAlarmService
    {
        #region << 变量 >>
        //离线配置服务
        RepeatLimitSettingService repeatSettingService = new RepeatLimitSettingService();
        //字典服务
        DictionaryService dicionaryService = new DictionaryService();
        //报警服务
        CreateAlarmService creatAlarmService = new CreateAlarmService();
        //监测通道服务
        public static InstrumentChannelService instrumentChannelService = new InstrumentChannelService();

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
        public RepeatAlarmService(ApplicationType applicationType, PollutantDataType dataType)
        {
            _applicationType = applicationType;
            _dataType = dataType;
            _applicationUid = SmartEP.Core.Enums.EnumMapping.GetApplicationValue(applicationType);
            _userForUid = dicionaryService.GetValueByText(Core.Enums.DictionaryType.AMS, "规则用途类型", "报警");
            _dataTypeUid = dicionaryService.GetValueByCode(Core.Enums.DictionaryType.AMS, "数据类型", dataType.ToString());
            _alarmEventUid = dicionaryService.GetValueByText(Core.Enums.DictionaryType.AMS, "报警类型", "重复");
        }
        #endregion
        #region << 方法 >>
        /// <summary>
        /// 执行重复报警
        /// </summary>
        /// <param name="application">应用程序类型</param>
        /// <param name="dataType">数据类型</param>
        public void CheckAlarm()
        {
            //获取重复限配置信息
            IQueryable<RepeatLimitSettingEntity> repeatSetEntities = repeatSettingService.RetrieveListByDataType(_applicationUid, _userForUid, _dataTypeUid);
            //因子编码、因子名称
            string factorCode = string.Empty,factorName = string.Empty,portId = string.Empty,portName= string.Empty;
            DateTime recordDateTime;
            InstrumentChannelEntity instrChannel;
            DataView dvLast;
            MonitoringPointEntity pointEntity;
            foreach (var entity in repeatSetEntities)
            {
                //获取因子编码、名称
                instrChannel = instrumentChannelService.RetrieveEntityByUid(entity.InstrumentChannelsUid);
                factorCode = instrChannel.PollutantCode;
                factorName = instrChannel.PollutantName;
                //获取站点id
                pointEntity = new MonitoringPointService().RetrieveEntityByUid(entity.MonitoringPointUid);
                portId = pointEntity.PointId.ToString();
                //获取最新数据时间
                dvLast = creatAlarmService.GetLatestData(_applicationType, _dataType, portId, factorCode);
                if (dvLast.Count > 0)
                {
                    recordDateTime = Convert.ToDateTime(dvLast[0]["Tstamp"]);//最新数据时间
                    if (creatAlarmService.IsRepeatData(_applicationType, _dataType, portId, recordDateTime, factorCode, Int32.Parse(entity.RepeatableNumber.ToString()))
                        && !creatAlarmService.IsExist(_alarmEventUid, _dataTypeUid, entity.MonitoringPointUid, recordDateTime, factorCode))
                    {
                        creatAlarmService.Add(new CreatAlarmEntity()
                        {
                            AlarmUid = Guid.NewGuid().ToString(),
                            ApplicationUid = _applicationUid,
                            MonitoringPointUid = entity.MonitoringPointUid,
                            RecordDateTime = recordDateTime,
                            AlarmEventUid = _alarmEventUid,
                            AlarmGradeUid = entity.NotifyGradeUid,
                            DataTypeUid = entity.DataTypeUid,
                            Content = string.Format("{0}{1}{2}[{3}]连续{4}组重复。", recordDateTime.ToString("MM-dd HH:mm"), pointEntity.MonitoringPointName, factorName, dvLast[0]["PollutantValue"].ToString(), entity.RepeatableNumber),
                            ItemName = factorCode,
                            ItemValue = dvLast[0]["PollutantValue"].ToString(),
                            CreatUser = "system",
                            CreatDateTime = DateTime.Now
                        });
                    }
                } 
            }
        }
        #endregion
    }
}
