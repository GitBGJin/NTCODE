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
using System.Text.RegularExpressions;

namespace SmartEP.Service.BaseData.Alarm
{
    /// <summary>
    /// 名称：ExcessiveAlarmService.cs
    /// 创建人：邱奇
    /// 创建日期：2015-08-22
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：提供离群报警信息服务(未完成，暂时不做)
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class OutlierAlarmService
    {
        //监测通道服务
        public static InstrumentChannelService g_instrumentChannelService = new InstrumentChannelService();
        //离群配置服务
        OutlierSettingService g_outlierService = new OutlierSettingService();
        //字典服务
        DictionaryService g_dicService = new DictionaryService();
        //报警服务
        CreateAlarmService g_creatAlarmService = new CreateAlarmService();
        public void CheckAlarm(ApplicationType application, PollutantDataType dataType)
        {
            //ApplicationValue applicationValue = (application == ApplicationType.Air) ? ApplicationValue.Air : ApplicationValue.Water;
            ////获取用途Uid，这边是报警
            //string userForUid = g_dicService.GetValueByText(Core.Enums.DictionaryType.AMS, "规则用途类型", "报警");
            ////获取数据类型Uid
            //string dataTypeUid = g_dicService.GetValueByCode(Core.Enums.DictionaryType.AMS, "数据类型", dataType.ToString());
            ////离群报警Uid
            //string alarmEventUid = g_dicService.GetValueByText(Core.Enums.DictionaryType.AMS, "报警类型", "离群报警");
            ////获取离群配置信息
            //IQueryable<OutlierSettingEntity> outlierSetEntities = g_outlierService.RetrieveList(dataTypeUid, userForUid, applicationValue);
            ////获取配置下多个点位Id
            //List<RsmPoint> rsmPoint = new List<RsmPoint>();
            //List<string> portIdsList = new List<string>();
            //List<RsmFactor> rsmFactor = new List<RsmFactor>();
            //List<string> factorsList = new List<string>();
            //GetPortIdList(applicationValue, outlierSetEntities, out portIdsList, out rsmPoint, out factorsList, out rsmFactor);
            ////循环点位，比较离群判定时间与实际断线时间
            //DateTime recordDateTime;
            ////污染物因子值
            //decimal maxPollutantValue;
            //decimal avgPollutantValue;
            //string factorCode = string.Empty, factorName = string.Empty, portId = string.Empty, pointName = string.Empty;
            //string pointId;
            //string comparePointIds;
            //List<int> pointIds = new List<int>();
            //foreach (var exEntity in outlierSetEntities)
            //{

            //    pointId = rsmPoint.FirstOrDefault(p => p.PointGuid == exEntity.MonitoringPointUid).PointID;
            //    pointName = rsmPoint.FirstOrDefault(p => p.PointGuid == exEntity.MonitoringPointUid).PointName;
            //    factorCode = g_instrumentChannelService.RetrieveListByInstrumentUid(exEntity.InstrumentChannelsUid).FirstOrDefault().PollutantCode;
            //    factorName = rsmFactor.FirstOrDefault(p => p.PollutantCode == factorCode).PollutantName;

            //    //获取每个点的最新数据时间
            //    DataView latestDataTimeView = g_creatAlarmService.GetLatestData(application, dataType, portIdsList, factorsList);
            //    if (latestDataTimeView.Count > 0)
            //    {
            //        //临时处理，将PointUid转换为Id后，将比较点位与主点位拼接在一起
            //        comparePointIds = "'1','2'";
            //        latestDataTimeView.RowFilter = "PollutantCode = '" + factorCode + "' and Point in (" + comparePointIds + ")";
            //        DataTable dt = latestDataTimeView.ToTable();
            //        maxPollutantValue = Convert.ToDecimal(dt.Compute("MAX(PollutantValue)", ""));
            //        latestDataTimeView.RowFilter = "PollutantValue = '" + maxPollutantValue + "' and PointId = '" + pointId + "'";
            //        if (latestDataTimeView.Count > 0)
            //        {
            //            //去除最大值
            //            recordDateTime = Convert.ToDateTime(latestDataTimeView[0]["Tstamp"]);
            //            dt.Rows.Remove(latestDataTimeView.ToTable().Rows[0]);
            //            avgPollutantValue = Convert.ToDecimal(dt.Compute("AVG(PollutantValue)", ""));
            //            if ((maxPollutantValue / avgPollutantValue) > exEntity.OutlierRate || maxPollutantValue > exEntity.ReferenceValue)
            //            {
            //                if (applicationValue == ApplicationValue.Air)
            //                {
            //                    MonitoringPointAirService pointService = new MonitoringPointAirService();
            //                    pointName = pointService.RetrieveEntityByUid(exEntity.MonitoringPointUid).MonitoringPointName;
            //                }
            //                else
            //                {
            //                    MonitoringPointWaterService pointService = new MonitoringPointWaterService();
            //                    pointName = pointService.RetrieveEntityByUid(exEntity.MonitoringPointUid).MonitoringPointName;
            //                }
            //                g_creatAlarmService.Add(new CreatAlarmEntity()
            //                {
            //                    AlarmUid = Guid.NewGuid().ToString(),
            //                    ApplicationUid = applicationValue.ToString(),
            //                    MonitoringPointUid = exEntity.MonitoringPointUid,
            //                    RecordDateTime = recordDateTime,
            //                    AlarmEventUid = alarmEventUid,
            //                    AlarmGradeUid = exEntity.NotifyGradeUid,
            //                    DataTypeUid = exEntity.DataTypeUid,
            //                    Content = string.Format("{0},{1}数据超标", pointName, recordDateTime.ToString("MM-dd HH:00")),
            //                    ItemName = factorCode,
            //                    ItemValue = maxPollutantValue.ToString(),
            //                    CreatUser = "system",
            //                    CreatDateTime = DateTime.Now
            //                });
            //            }
            //        }
            //    }
            }

        //public void GetPortIdList(ApplicationValue applicationValue, IQueryable<OutlierSettingEntity> outlierSetEntities, out  List<string> portIdsList, out List<RsmPoint> rsmPoint, out  List<string> factorsList, out List<RsmFactor> rsmFactor)
        //{
        //    portIdsList = new List<string>();
        //    factorsList = new List<string>();
        //    MonitoringPointEntity pointEntity;
        //    PollutantCodeEntity pollutantEntity;
        //    InstrumentChannelEntity channelEntity;
        //    rsmPoint = new List<RsmPoint>();
        //    rsmFactor = new List<RsmFactor>();
        //    if (applicationValue == ApplicationValue.Air)
        //    {
        //        MonitoringPointAirService airPointService = new MonitoringPointAirService();
        //        AirPollutantService airPollutantService = new AirPollutantService();
        //        InstrumentChannelService channelService = new InstrumentChannelService();
        //        foreach (var entity in outlierSetEntities)
        //        {
        //            pointEntity = airPointService.RetrieveEntityByUid(entity.MonitoringPointUid);
        //            channelEntity = channelService.RetrieveEntityByUid(entity.InstrumentChannelsUid);
        //            pollutantEntity = airPollutantService.RetrieveEntityByUid(channelEntity.PollutantUid);
        //            if (pointEntity != null)
        //            {
        //                rsmPoint.Add(new RsmPoint(pointEntity.MonitoringPointName, pointEntity.PointId.ToString(), entity.MonitoringPointUid));
        //                portIdsList.Add(pointEntity.PointId.ToString());
        //                rsmFactor.Add(new RsmFactor(pollutantEntity.PollutantName, pollutantEntity.PollutantCode.ToString(), pollutantEntity.DecimalDigit.ToString(), pollutantEntity.MeasureUnitUid.ToString(), entity.MonitoringPointUid));
        //                factorsList.Add(pollutantEntity.PollutantCode.ToString());
        //            }
        //        }
        //    }
        //    else if (applicationValue == ApplicationValue.Water)
        //    {
        //        MonitoringPointWaterService waterPointService = new MonitoringPointWaterService();
        //        WaterPollutantService waterPollutantService = new WaterPollutantService();
        //        InstrumentChannelService channelService = new InstrumentChannelService();
        //        foreach (var entity in outlierSetEntities)
        //        {
        //            pointEntity = waterPointService.RetrieveEntityByUid(entity.MonitoringPointUid);
        //            channelEntity = channelService.RetrieveEntityByUid(entity.InstrumentChannelsUid);
        //            pollutantEntity = waterPollutantService.RetrieveEntityByUid(channelEntity.PollutantUid);
        //            if (pointEntity != null)
        //            {
        //                rsmPoint.Add(new RsmPoint(pointEntity.MonitoringPointName, pointEntity.PointId.ToString(), entity.MonitoringPointUid));
        //                portIdsList.Add(pointEntity.PointId.ToString());
        //                rsmFactor.Add(new RsmFactor(pollutantEntity.PollutantName, pollutantEntity.PollutantCode.ToString(), pollutantEntity.DecimalDigit.ToString(), pollutantEntity.MeasureUnitUid.ToString(), entity.MonitoringPointUid));
        //                factorsList.Add(pollutantEntity.PollutantCode.ToString());
        //            }
        //        }
        //    }
        //}

        //临时处理，通过点位UID返回ID
        public int GetIdByUid(string uid)
        {
            return 1;
        }
    }
}
