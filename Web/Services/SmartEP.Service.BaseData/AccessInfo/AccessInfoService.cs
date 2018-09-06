using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SmartEP.Core.Generic;
using SmartEP.BaseInfoRepository.Access;
using SmartEP.Data;

namespace SmartEP.Service.BaseData.AccessInfo
{
    public class AccessInfoService
    {
        AccessInfoRepository g_AccessInfoRepository = new AccessInfoRepository();
        public int InserAccessInfo(string pointId, string cardNumber, string registerName, string stationWay, DateTime stationDate)
        {
            int num = g_AccessInfoRepository.InserAccessInfo(pointId, cardNumber, registerName, stationWay, stationDate);
            return num;
        }
    }
}
