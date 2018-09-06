using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.BaseInfoRepository.MPInfo
{
    public class AcquisitionInstrumentRepository : BaseGenericRepository<BaseDataModel, AcquisitionInstrumentEntity>
    {
        public override bool IsExist(string strKey)
        {
            return Retrieve(x => x.AcquisitionUid.Equals(strKey)).Count() == 0 ? false : true;
        }
    }
}
