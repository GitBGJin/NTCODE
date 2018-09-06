using SmartEP.AMSRepository.Water;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.WaterAutoMonitoring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.Service.AutoMonitoring.Water
{
    public class OriginalPacketRequestService
    {
        /// <summary>
        /// 1分钟数据仓储层
        /// </summary>
        OriginalPacketRequestRepository g_OriginalPacketRequestRepository = Singleton<OriginalPacketRequestRepository>.GetInstance();

        /// <summary>
        /// 获取设备监测数据
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">截止时间</param>
        /// <returns></returns>
        public IQueryable<OriginalPacketRequestEntity> RetrieveMonitoringData(DateTime startTime, DateTime endTime)
        {
            return g_OriginalPacketRequestRepository.Retrieve(x => x.OperaterTime >= startTime && x.OperaterTime <= endTime);
        }

        /// <summary>
        /// 追加反控数据
        /// </summary>
        /// <param name="baseEntity">命令请求实体</param>
        public void Save(IBaseEntityProperty baseEntity)
        {
            if (g_OriginalPacketRequestRepository != null)
                g_OriginalPacketRequestRepository.Add(baseEntity);
        }

        public bool IsExit(string strQN)
        {
            IQueryable<IBaseEntityProperty> entity = g_OriginalPacketRequestRepository.Retrieve(p => p.Qn == strQN);
            return entity.Count() > 0 ? true : false;
        }
    }
}
