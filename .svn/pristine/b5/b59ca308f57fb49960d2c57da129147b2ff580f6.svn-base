using SmartEP.BaseInfoRepository.MPInfo;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.BaseData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SmartEP.Service.BaseData.MPInfo
{
    public class MonitoringPointService : AbstractMonitoringPoint
    {

        MonitoringPointRepository g_MonitoringPointRepository = Singleton<MonitoringPointRepository>.GetInstance();

        /// <summary>
        /// 根据点位ID获取点位名称信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetPointNameByID(string pointid)
        {
            return g_MonitoringPointRepository.GetPointNameByID(pointid);
        }

        /// <summary>
        /// 根据lambda表达式要求，返回所有对象
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IQueryable<MonitoringPointEntity> Retrieve(Expression<Func<MonitoringPointEntity, bool>> predicate)
        {
            return g_MonitoringPointRepository.Retrieve(predicate);
        }
    }
}
