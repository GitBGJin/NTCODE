using SmartEP.BaseInfoRepository.Alarm;
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
    /// 名称：ExcessiveAlarmService.cs
    /// 创建人：邱奇
    /// 创建日期：2015-08-22
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：提供超标报警信息服务
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class ExcessiveAlarmService
    {
        #region << 变量 >>
        //监测通道服务
        public static InstrumentChannelService instrumentChannelService = new InstrumentChannelService();
        //超标配置服务
        ExcessiveSettingService excessiveSettingService = new ExcessiveSettingService();
        //字典服务
        DictionaryService dicionaryService = new DictionaryService();
        //报警服务
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
        public ExcessiveAlarmService(ApplicationType applicationType, PollutantDataType dataType)
        {
            _applicationType = applicationType;
            _dataType = dataType;
            _applicationUid = SmartEP.Core.Enums.EnumMapping.GetApplicationValue(applicationType);
            _userForUid = dicionaryService.GetValueByText(Core.Enums.DictionaryType.AMS, "规则用途类型", "报警");
            _dataTypeUid = dicionaryService.GetValueByCode(Core.Enums.DictionaryType.AMS, "数据类型", dataType.ToString());
            _alarmEventUid = dicionaryService.GetValueByText(Core.Enums.DictionaryType.AMS, "报警类型", "超标");
        }
        #endregion
        #region << 方法 >>
        public void CheckAlarm()
        {
            IQueryable<ExcessiveSettingEntity> excessiveSetEntities = excessiveSettingService.RetrieveListByDataType(_applicationUid, _userForUid, _dataTypeUid);
            //因子编码、因子名称
            string factorCode = string.Empty, factorName = string.Empty, portId = string.Empty, portName = string.Empty;
 
            //循环点位，比较超标判定时间与实际断线时间
            DateTime recordDateTime;
            //污染物因子值
            decimal pollutantValue;
            InstrumentChannelEntity instrChannel;
            MonitoringPointEntity pointEntity;
            foreach (var entity in excessiveSetEntities)
            {
                //获取因子编码
                instrChannel = instrumentChannelService.RetrieveEntityByUid(entity.InstrumentChannelsUid);
                factorCode = instrChannel.PollutantCode;
                factorName = instrChannel.PollutantName;
                //获取站点id
                pointEntity = new MonitoringPointService().RetrieveEntityByUid(entity.MonitoringPointUid);
                portId = pointEntity.PointId.ToString();
                //获取每个点的最新数据时间
                DataView latestDataView = creatAlarmService.GetLatestData(_applicationType, _dataType, portId, factorCode);
                if (latestDataView.Count > 0)
                {
                    pollutantValue = Convert.ToDecimal(latestDataView[0]["PollutantValue"]);
                    recordDateTime = Convert.ToDateTime(latestDataView[0]["Tstamp"]);
                    if (pollutantValue > entity.ExcessiveUpper || pollutantValue < entity.ExcessiveLow)
                    {
                        //实际数据缺失分钟数大于超标判定分钟数,并且之前无重复报警，则触发超标报警
                        if (!creatAlarmService.IsExist(_alarmEventUid, _dataTypeUid, entity.MonitoringPointUid, recordDateTime,factorCode))
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
                                Content = string.Format("{0}{1}{2}[{3}]超标", pointEntity.MonitoringPointName, recordDateTime.ToString("MM-dd HH:00"), factorName, pollutantValue),
                                ItemName = factorCode,
                                ItemValue = pollutantValue.ToString(),
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
