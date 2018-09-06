using SmartEP.BaseInfoRepository.Alarm;
using SmartEP.DomainModel.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.Service.BaseData.Alarm
{
    /// <summary>
    /// 名称：NotifySendService.cs
    /// 创建人：季柯
    /// 创建日期：2015-09-02
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：提供通知信息服务
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class NotifySendService
    {
        //短信发送仓储层
        NotifySendRepository notifySendRepository = new NotifySendRepository();
        //通知策略服务
        NotifyStrategyService notifyStrategyService = new NotifyStrategyService();
        //生成报警服务
        CreateAlarmService alarmService = new CreateAlarmService();
        //通知号码服务
        NotifyNumbersService notifyNumberService = new NotifyNumbersService();
        #region 增删改
        /// <summary>
        /// 增加通知信息
        /// </summary>
        /// <param name="notifySend">实体</param>
        public void Add(NotifySendEntity notifySend)
        {
            notifySendRepository.Add(notifySend);
        }

        /// <summary>
        /// 更新通知信息
        /// </summary>
        /// <param name="notifySend">实体</param>
        public void Update(NotifySendEntity notifySend)
        {
            notifySendRepository.Update(notifySend);
        }

        /// <summary>
        /// 删除通知信息
        /// </summary>
        /// <param name="notifySend">实体</param>
        public void Delete(NotifySendEntity notifySend)
        {
            notifySendRepository.Delete(notifySend);
        }

        /// <summary>
        /// 批量删除通知信息
        /// </summary>
        /// <param name="notifySend">实体</param>
        public void BatchDelete(List<NotifySendEntity> notifySends)
        {
            notifySendRepository.BatchDelete(notifySends);
        }
        #endregion

        /// <summary>
        /// 通知查询
        /// </summary>
        /// <param name="applicationUid"></param>
        /// <param name="notifyTypeUid"></param>
        /// <param name="sendContent"></param>
        /// <param name="sendStartTime"></param>
        /// <param name="sendEndTime"></param>
        /// <param name="isHandle"></param>
        /// <returns></returns>
        public IQueryable<NotifySendEntity> RetrieveList(string applicationUid, string notifyTypeUid, string sendContent, string receiveUser,DateTime sendStartTime, DateTime sendEndTime, bool isHandle)
        {
            IQueryable<NotifySendEntity> notifySendList = notifySendRepository.Retrieve(p => p.ApplicationUid == applicationUid && p.SendDateTime >= sendStartTime && p.SendDateTime <= sendEndTime && p.HandleFinishOrNot == isHandle);
            if (!string.IsNullOrEmpty(notifyTypeUid)) notifySendList.Where(p => p.NotifyTypeUid == notifyTypeUid);
            if (!string.IsNullOrEmpty(receiveUser)) notifySendList.Where(p => p.ReceiveUserNames.Contains(receiveUser));
            if (!string.IsNullOrEmpty(sendContent)) notifySendList.Where(p => p.SendContent.Contains(sendContent));
            return notifySendList;
        }

        /// <summary>
        /// 根据Uid数组获取通知发送信息列表
        /// </summary>
        /// <param name="notifySendUids"></param>
        /// <returns></returns>
        public IQueryable<NotifySendEntity> RetrieveListByUids(string[] notifySendUids)
        {
            return notifySendRepository.Retrieve(p=>notifySendUids.Contains(p.NotifySendUid));
        }

        /// <summary>
        /// 根据Uid获取通知发送信息
        /// </summary>
        /// <param name="notifySendUids"></param>
        /// <returns></returns>
        public NotifySendEntity RetrieveEntity(string notifySendUid)
        {
            return notifySendRepository.RetrieveFirstOrDefault(p => p.NotifySendUid == notifySendUid);
        }

        public void NotifySend(string applicationUid,string dataTypeUid)
        {
            //①更新当前短信发送次数【可用数据库作业执行】
            //只要当前时间不在配置的有效时间范围内的，都更新当前短信发送次数为0

            //②获取所有启用通知配置，循环
            DateTime startTime, endTime;
            //当前时间与上次发送间隔分钟数
            double lastTimeSpanMin;
            TimeSpan lastTimeSpan;
            List<CreatAlarmEntity> alarmList;
            string smsContent = string.Empty;
            NotifySendEntity nofitySendEntify;
            foreach (var notify in notifyStrategyService.RetrieveAll(applicationUid).ToList())
            {
                //②满足发送条件的，去报警信息表里筛选满足条件的报警信息
                //满足：1、当前时间在配置有效时间范围内
                //      2、当前发送次数<当天限制发送次数
                //      3、当前时间与上次发送时间间隔>配置的发送间隔分钟数
                startTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " " + notify.BeginTime);
                endTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " " + notify.EndTime);
                lastTimeSpan = (TimeSpan)(DateTime.Now - notify.LastNotifyTime);
                lastTimeSpanMin = lastTimeSpan.TotalMinutes;

                if ((DateTime.Now > startTime) && (DateTime.Now < endTime) && notify.CurrNotifyCount < notify.NotifyCount && lastTimeSpanMin > Int32.Parse(notify.NotifySpan))
                {
                    //筛选满足条件的报警信息
                    alarmList = alarmService.RetrieveList(notify.AlarmEventUid, notify.DataTypeUid, applicationUid, notify.EffectSubject.Split(';')).ToList();
                    foreach(var alarmEntity in alarmList)
                    {
                        smsContent += alarmEntity.Content + ";";
                    }
                }

                if (!string.IsNullOrEmpty(smsContent))
                {
                    //生成到短信待发送列表
                    nofitySendEntify = new NotifySendEntity();
                    nofitySendEntify.NotifySendUid = Guid.NewGuid().ToString();
                    nofitySendEntify.ApplicationUid = applicationUid;
                    nofitySendEntify.NotifyTypeUid = "f887af1c-e287-4d29-bf56-006ce8da87ac";//通知类型为短信
                    nofitySendEntify.CreatDateTime = DateTime.Now;
                    nofitySendEntify.CreatUser = "system";
                    nofitySendEntify.SendFinishOrNot = false;//新生成短信为未发送状态
                    nofitySendEntify.ReceiveUserAddresses = notify.NotifyNumbers;
                    nofitySendEntify.ReceiveUserNames =SmartEP.Utilities.DataTypes.ExtensionMethods.StringExtensions.GetArrayStrNoEmpty(notifyNumberService.RetrieveNameArrayByUids(notify.NotifyNumberUids.Split(';')).ToList(), ";");
                    nofitySendEntify.ReceiveUserUids = notify.NotifyNumberUids;
                    nofitySendEntify.SendContent = smsContent;

                    //增加发送短信实体
                    Add(nofitySendEntify);

                    //更新短信发送配置
                    notify.CurrNotifyCount = notify.CurrNotifyCount + 1;
                    notify.LastNotifyTime = DateTime.Now;
                    notify.UpdateUser = "system";
                    notify.UpdateDateTime = DateTime.Now;
                    notifyStrategyService.Update(notify);

                }
            }    
        }
    }
}
