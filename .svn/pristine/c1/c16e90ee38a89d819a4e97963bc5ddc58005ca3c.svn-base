using SmartEP.BaseInfoRepository.Alarm;
using SmartEP.DomainModel.BaseData;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.Service.BaseData.Alarm
{
    public class NotifyNumbersService
    {
        NotifyNumbersRepository notifyNumbersRepository = new NotifyNumbersRepository();
        #region 增删改
        /// <summary>
        /// 增加通知信息
        /// </summary>
        /// <param name="notifySend">实体</param>
        public void Add(V_NotifyNumberEntity notifyNumber)
        {
            notifyNumbersRepository.Add(notifyNumber);
        }

        /// <summary>
        /// 更新通知信息
        /// </summary>
        /// <param name="notifySend">实体</param>
        public void Update(V_NotifyNumberEntity notifyNumber)
        {
            notifyNumbersRepository.Update(notifyNumber);
        }

        /// <summary>
        /// 删除通知信息
        /// </summary>
        /// <param name="notifySend">实体</param>
        public void Delete(V_NotifyNumberEntity notifySend)
        {
            notifyNumbersRepository.Delete(notifySend);
        }
 
        #endregion

        /// <summary>
        /// 返回所有通知短信号码
        /// </summary>
        /// <returns></returns>
        public IQueryable<V_NotifyNumberEntity> RetrieveList()
        {
            return notifyNumbersRepository.RetrieveAll();
        }

        /// <summary>
        /// 返回所有通知短信号码
        /// </summary>
        /// <returns></returns>
        public V_NotifyNumberEntity RetrieveEntityByUid(string rowGuid)
        {
            return notifyNumbersRepository.RetrieveFirstOrDefault(p=>p.RowGuid == rowGuid);
        }

        /// <summary>
        /// 根据Uid数组获取通知短信号码
        /// </summary>
        /// <returns></returns>
        public  IQueryable<V_NotifyNumberEntity> RetrieveEntityByUids(string[] rowGuids)
        {
            return notifyNumbersRepository.Retrieve(p => rowGuids.Contains(p.RowGuid));
        }

        /// <summary>
        /// 根据Uid数组获取通知短信号码的数组
        /// </summary>
        /// <returns></returns>
        public string[] RetrieveNumberArrayByUids(string[] rowGuids)
        {
            return notifyNumbersRepository.Retrieve(p => rowGuids.Contains(p.RowGuid)).Select(p=>p.Number).ToArray();
        }

        /// <summary>
        /// 根据Uid数组获取通知短信号码的字符串
        /// </summary>
        /// <returns></returns>
        public string RetrieveNumbersByUids(string[] rowGuids)
        {
            return StringExtensions.GetArrayStrNoEmpty(notifyNumbersRepository.Retrieve(p => rowGuids.Contains(p.RowGuid)).Select(p => p.Number).ToList(),";");
        }

        /// <summary>
        /// 根据Uid数组获取通知短信号码的人员数组
        /// </summary>
        /// <returns></returns>
        public string[] RetrieveNameArrayByUids(string[] rowGuids)
        {
            return notifyNumbersRepository.Retrieve(p => rowGuids.Contains(p.RowGuid)).Select(p => p.Name).ToArray();
        }
    }
}
