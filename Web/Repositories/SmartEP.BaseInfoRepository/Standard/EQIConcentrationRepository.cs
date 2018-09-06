using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.Data.SqlServer.BaseData;
using SmartEP.DomainModel.BaseData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.BaseInfoRepository.Standard
{
    public class EQIConcentrationRepository : BaseGenericRepository<BaseDataModel, EQIConcentrationLimitEntity>
    {
        /// <summary>
        /// 数据接口
        /// </summary>
        EQIConcentrationLimitDAL g_EQIConcentrationLimitDAL = Singleton<EQIConcentrationLimitDAL>.GetInstance();

        public override bool IsExist(string strKey)
        {
            return Retrieve(x => x.EQIConcentrationLimitUid.Equals(strKey)).Count() == 0 ? false : true;
        }

        #region 获取因子对应标准的评价范围
        /// <summary>
        /// 获取因子对应标准的评价范围
        /// </summary>
        /// <param name="IEQI">评价标准</param>
        /// <param name="PollutantCodes">因子Code数组</param>
        /// <param name="applicationtype">应用类型</param>
        /// <param name="waterPointCalWQType">水域类型</param>
        /// 河流"d8197909-568e-4319-874c-3ad7cbc92a7e"
        /// 湖、库"e82cd86f-71ba-4f87-8e5c-6ac7ca055a6b"
        /// <returns></returns>
        public DataTable GetIEQIByPollutantCodes(List<int> IEQI, List<string> PollutantCodes, ApplicationType applicationtype, string waterPointCalWQType)
        {
            return g_EQIConcentrationLimitDAL.GetIEQIByPollutantCodes(IEQI, PollutantCodes, applicationtype, waterPointCalWQType);
        }
        #endregion
    }
}
