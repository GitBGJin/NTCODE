using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.BaseInfoRepository.BusinessRule
{
    public class ExcessiveSettingRepository : BaseGenericRepository<BaseDataModel, ExcessiveSettingEntity>
    {
        public override bool IsExist(string strKey)
        {
            return Retrieve(x => x.ExcessiveUid.Equals(strKey)).Count() == 0 ? false : true;
        }

    }
}
