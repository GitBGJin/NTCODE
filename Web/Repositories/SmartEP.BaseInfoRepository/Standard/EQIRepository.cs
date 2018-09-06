﻿using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.BaseInfoRepository.Standard
{
    public class EQIRepository : BaseGenericRepository<BaseDataModel, EQIEntity>
    {
        public override bool IsExist(string strKey)
        {
            return Retrieve(x => x.EQIUid.Equals(strKey)).Count() == 0 ? false : true;
        }
    }
}
