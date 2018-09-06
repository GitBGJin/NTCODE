using SmartEP.BaseInfoRepository.Alarm;
using SmartEP.Core.Generic;
using SmartEP.DomainModel.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SmartEP.Service.BaseData.Alarm
{
    public class NotifyStrategyService
    {
        NotifyStrategyRepository notifyStrategyRepository = Singleton<NotifyStrategyRepository>.GetInstance();

        #region 增删改
        /// <summary>
        /// 增加通知策略
        /// </summary>
        /// <param name="notifySend">实体</param>
        public void Add(NotifyStrategyEntity notifySend)
        {
            notifyStrategyRepository.Add(notifySend);
        }

        /// <summary>
        /// 更新通知策略
        /// </summary>
        /// <param name="notifySend">实体</param>
        public void Update(NotifyStrategyEntity notifySend)
        {
            notifyStrategyRepository.Update(notifySend);
        }

        /// <summary>
        /// 删除通知策略
        /// </summary>
        /// <param name="notifySend">实体</param>
        public void Delete(NotifyStrategyEntity notifySend)
        {
            notifyStrategyRepository.Delete(notifySend);
        }

        /// <summary>
        /// 批量删除通知策略
        /// </summary>
        /// <param name="notifySend">实体</param>
        public void BatchDelete(List<NotifyStrategyEntity> notifySends)
        {
            notifyStrategyRepository.BatchDelete(notifySends);
        }
        #endregion

        /// <summary>
        /// 根据Uid数组获取通知策略列表
        /// </summary>
        /// <param name="notifySendUids"></param>
        /// <returns></returns>
        public IQueryable<NotifyStrategyEntity> RetrieveListByUids(string[] notifyStrategyUids)
        {
            return notifyStrategyRepository.Retrieve(p => notifyStrategyUids.Contains(p.NotifyStrategyUid));
        }

        /// <summary>
        /// 根据应用程序Uid获取所有启用的通知策略列表
        /// </summary>
        /// <param name="notifySendUids"></param>
        /// <returns></returns>
        public IQueryable<NotifyStrategyEntity> RetrieveAll(string applicationTypeUid)
        {
            return notifyStrategyRepository.Retrieve(p => p.ApplicationUid == applicationTypeUid && p.EnableOrNot != null && p.EnableOrNot == true);
        }

        /// <summary>
        /// 根据Uid获取通知策略
        /// </summary>
        /// <param name="notifySendUids"></param>
        /// <returns></returns>
        public NotifyStrategyEntity RetrieveEntity(string notifyStrategyUid)
        {
            return notifyStrategyRepository.RetrieveFirstOrDefault(p => p.NotifyStrategyUid == notifyStrategyUid);
        }

        /// <summary>
        /// 获取通知策略
        /// </summary>
        /// <param name="alarmEventUid">报警类型Uid</param>
        /// <param name="strategyName">报警策略名称</param>
        /// <param name="enabledOrNot">是否启用</param>
        /// <returns></returns>
        public IQueryable<NotifyStrategyEntity> RetrieveList(string applicationTypeUid, string alarmEventUid, string strategyName, bool enabledOrNot)
        {
            IQueryable<NotifyStrategyEntity> notifyStrategyList = notifyStrategyRepository.Retrieve(p => p.ApplicationUid == applicationTypeUid && p.EnableOrNot == enabledOrNot);
            if (!string.IsNullOrEmpty(alarmEventUid)) notifyStrategyList = notifyStrategyList.Where(p => p.AlarmEventUid == alarmEventUid);
            if (!string.IsNullOrEmpty(strategyName)) notifyStrategyList = notifyStrategyList.Where(p => p.NotifyStrategyName.Contains(strategyName));

            return notifyStrategyList;
        }

        /// <summary>
        /// 根据lambda表达式要求，返回所有对象
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IQueryable<NotifyStrategyEntity> Retrieve(Expression<Func<NotifyStrategyEntity, bool>> predicate)
        {
            return notifyStrategyRepository.Retrieve(predicate);
        }
    }
}
