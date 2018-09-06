using SmartEP.Core.Generic;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.MonitoringBusinessRepository.QualityControlOperation.Water;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.Service.OperatingMaintenance.Water
{
    
   public class TaskConfigService
    {
       /// <summary>
        /// 任务配置仓储层
       /// </summary>
       TaskConfigRepository g_TaskConfigRepository = Singleton<TaskConfigRepository>.GetInstance();

       /// <summary>
       /// 获得MissionName
       /// </summary>
       /// <param name="monitoringPointUid"></param>
       /// <returns></returns>
       //public List<TaskConfigEntity> GetName(string strWhere)
       //{
       //    return g_TaskConfigRepository.GetName(strWhere).ToList<TaskConfigEntity>();
       //}
       public string[] GetName(string strWhere)
       {
           return g_TaskConfigRepository.GetName(strWhere);
       }
    }
}
