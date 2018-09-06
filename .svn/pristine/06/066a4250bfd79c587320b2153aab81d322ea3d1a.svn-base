using SmartEP.Core.Interfaces;
using SmartEP.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.BaseInfoRepository.Dictionary
{
    public class CodeDictionaryRepository : BaseGenericRepository<FrameworkModel, V_CodeDictionaryEntity>
    {
        public override bool IsExist(string strKey)
        {
            return Retrieve(x => x.ItemGuid.Equals(strKey)).Count() == 0 ? false : true;
        }
    }
}
