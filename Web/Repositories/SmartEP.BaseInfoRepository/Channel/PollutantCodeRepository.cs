using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.BaseInfoRepository.Channel
{
    public class PollutantCodeRepository : BaseGenericRepository<BaseDataModel, PollutantCodeEntity>
    {
        public override bool IsExist(string strKey)
        {
            return Retrieve(x => x.PollutantTypeUid.Equals(strKey)).Count() == 0 ? false : true;
        }
    }
}
