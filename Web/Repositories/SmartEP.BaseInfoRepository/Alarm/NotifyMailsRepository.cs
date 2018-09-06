using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.BaseInfoRepository.Alarm
{
    public class NotifyMailsRepository : BaseGenericRepository<BaseDataModel, V_NotifyMaliEntity>
    {
        public override bool IsExist(string strKey)
        {
            return Retrieve(x => x.RowGuid.Equals(strKey)).Count() == 0 ? false : true;
        }
    }
}
