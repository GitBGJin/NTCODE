using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.BaseInfoRepository.Alarm
{
    public class NotifyStrategyRepository : BaseGenericRepository<BaseDataModel, NotifyStrategyEntity>
    {
        public override bool IsExist(string strKey)
        {
            return Retrieve(x => x.NotifyStrategyUid.Equals(strKey)).Count() == 0 ? false : true;
        }
    }
}
