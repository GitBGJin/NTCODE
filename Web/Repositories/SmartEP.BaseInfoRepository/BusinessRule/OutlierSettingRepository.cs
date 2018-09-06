using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.BaseInfoRepository.BusinessRule
{
    public class OutlierSettingRepository : BaseGenericRepository<BaseDataModel, OutlierSettingEntity>
    {
        public override bool IsExist(string strKey)
        {
            return Retrieve(x => x.OutlierUid.Equals(strKey)).Count() == 0 ? false : true;
        }
    }
}
