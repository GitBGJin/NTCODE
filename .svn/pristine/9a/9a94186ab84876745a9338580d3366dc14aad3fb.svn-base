using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.BaseInfoRepository.Alarm
{
    public class NotifySendRepository : BaseGenericRepository<BaseDataModel, NotifySendEntity>
    {
        public override bool IsExist(string strKey)
        {
            return Retrieve(x => x.NotifySendUid.Equals(strKey)).Count() == 0 ? false : true;
        }
    }
}
