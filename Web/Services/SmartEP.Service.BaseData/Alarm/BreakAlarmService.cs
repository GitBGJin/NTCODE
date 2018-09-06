using SmartEP.Core.Enums;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.BusinessRule;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.Frame;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SmartEP.Utilities.DataTypes.ExtensionMethods;

namespace SmartEP.Service.BaseData.Alarm
{
    /// <summary>
    /// 名称：BreakAlarmService.cs
    /// 创建人：季柯
    /// 创建日期：2015-08-22
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 突变报警服务
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class BreakAlarmService
    {
        #region << 变量 >>
        //突变配置服务
        BreakSettingSerivce breakSettingService = new BreakSettingSerivce();
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
        public BreakAlarmService(ApplicationType applicationType, PollutantDataType dataType)
        {
            _applicationType = applicationType;
            _dataType = dataType;
            _applicationUid = SmartEP.Core.Enums.EnumMapping.GetApplicationValue(applicationType);
            _userForUid = dicionaryService.GetValueByText(Core.Enums.DictionaryType.AMS, "规则用途类型", "报警");
            _dataTypeUid = dicionaryService.GetValueByCode(Core.Enums.DictionaryType.AMS, "数据类型", dataType.ToString());
            _alarmEventUid = dicionaryService.GetValueByText(Core.Enums.DictionaryType.AMS, "报警类型", "突变");
        }
        #endregion
        #region << 方法 >>
        public void CheckAlarm()
        {
            //获取重复限配置信息
            IQueryable<BreakSettingEntity> breakSetEntities = breakSettingService.RetrieveListByDataType(_applicationUid, _userForUid, _dataTypeUid);
            //因子代码、名称、站点id
            string factorCode = string.Empty, factorName = string.Empty, portId = string.Empty;
            DateTime recordDateTime;
            InstrumentChannelEntity instrChannel;
            MonitoringPointEntity pointEntity;
            DataView dvLast,beforeDataView;
            Decimal beforeAvgData,thisData;
            foreach (var entity in breakSetEntities)
            {
                //获取因子编码,因子名称
                instrChannel = instrumentChannelService.RetrieveEntityByUid(entity.InstrumentChannelsUid);
                factorCode = instrChannel.PollutantCode;
                factorName = instrChannel.PollutantName;
                //获取站点id
                pointEntity = new MonitoringPointService().RetrieveEntityByUid(entity.MonitoringPointUid);
                portId = pointEntity.PointId.ToString();
                //获取最新数据时间
                dvLast = creatAlarmService.GetLatestData(_applicationType, _dataType, portId, factorCode);
                if (TypeConversionExtensions.IsNotNull(dvLast))
                {
                    recordDateTime = Convert.ToDateTime(dvLast[0]["Tstamp"]);
                    thisData =  Convert.ToDecimal(dvLast[0]["PollutantValue"]);
                    beforeDataView = creatAlarmService.GetCompareBeforeData(_applicationType, _dataType, portId, recordDateTime, factorCode, Int32.Parse(entity.CompareBeforeGroups.ToString()), false);
                    if (TypeConversionExtensions.IsNotNull(beforeDataView))
                    {
                        beforeAvgData = Convert.ToDecimal(beforeDataView.ToTable().Compute("AVG(PollutantValue)", ""));
                        if (IsBreak(beforeAvgData,thisData,Convert.ToDecimal(entity.BreakMultipleForUpper)) && !creatAlarmService.IsExist(_alarmEventUid, _dataTypeUid, entity.MonitoringPointUid, recordDateTime,factorCode))
                        {
                            creatAlarmService.Add(new CreatAlarmEntity()
                            {
                                AlarmUid = Guid.NewGuid().ToString(),
                                ApplicationUid = _applicationUid,
                                MonitoringPointUid = entity.MonitoringPointUid,
                                RecordDateTime = recordDateTime,
                                AlarmEventUid = _alarmEventUid,
                                AlarmGradeUid = entity.GradeUid,
                                DataTypeUid = entity.DataTypeUid,
                                Content = string.Format("{0}{1}{2}[{3}]超前{4}组{5}倍", pointEntity.MonitoringPointName, recordDateTime.ToString("MM-dd HH:mm"), factorName, thisData.ToString(), entity.CompareBeforeGroups.ToString(), entity.BreakMultipleForUpper.ToString()),
                                ItemName = factorCode,
                                ItemValue = thisData.ToString(),
                                CreatUser = "system",
                                CreatDateTime = DateTime.Now
                            });
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 判定是否突变（当前数据>=前几组数据均值的N倍，则判定为突变）
        /// </summary>
        /// <param name="avgLastData">前面几组均值</param>
        /// <param name="thisData">当前数据</param>
        /// <param name="BreakMultiple">突变倍数</param>
        /// <returns></returns>
        public bool IsBreak(Decimal avgLastData, Decimal thisData, Decimal breakMultiple)
        {
            return (thisData >= avgLastData * breakMultiple) ? true : false;
        }
        #endregion
    }
}
