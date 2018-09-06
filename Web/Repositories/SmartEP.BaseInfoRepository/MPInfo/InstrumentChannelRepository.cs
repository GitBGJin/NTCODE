using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.BaseInfoRepository.MPInfo
{
    public class InstrumentChannelRepository : BaseGenericRepository<BaseDataModel, InstrumentChannelEntity>
    {
        public override bool IsExist(string strKey)
        {
            return Retrieve(x => x.InstrumentChannelsUid.Equals(strKey)).Count() == 0 ? false : true;
        }
    }
}
