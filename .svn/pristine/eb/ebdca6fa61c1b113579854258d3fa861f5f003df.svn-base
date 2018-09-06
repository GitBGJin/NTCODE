using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.BaseInfoRepository.MPInfo
{
    public class MonitoringPointExtensionForEQMSWaterRepository : BaseGenericRepository<BaseDataModel, MonitoringPointExtensionForEQMSWaterEntity>
    {
        public override bool IsExist(string strKey)
        {
            return Retrieve(x => x.MonitoringPointUid.Equals(strKey)).Count() == 0 ? false : true;
        }
    }
}
