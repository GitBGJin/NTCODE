using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using SmartEP.Data.SqlServer.BaseData;
using SmartEP.DomainModel.BaseData;
using SmartEP.Core.Interfaces;

namespace SmartEP.BaseInfoRepository.BusinessRule
{
    public class PersonalizedSetRepository : BaseGenericRepository<BaseDataModel, PersonalizedSettingEntity>
    {
        private PersonalizedSetDAL dal = new PersonalizedSetDAL();

        public override bool IsExist(string strKey)
        {
            return Retrieve(x => x.PersonalizedSettingUid.Equals(strKey)).Count() == 0 ? false : true;
        }

        /// <summary>
        /// 根据用户GUID获取授权设置
        /// </summary>
        /// <param name="userGuid">用户Guid</param>
        /// <param name="appUid">系统应用类型Uid</param>
        /// <returns></returns>
        public DataTable GetPersonalizedSetByUserGuid(string userGuid, string appUid)
        {
            return dal.GetPersonalizedSetByUserGuid(userGuid, appUid);
        }

        /// <summary>
        /// 根据用户GUID获取测点分组设置
        /// </summary>
        /// <param name="userGuid">用户Guid</param>
        /// <param name="appUid">系统应用类型Uid</param>
        /// <returns></returns>
        public DataView GetPersonalizedSetPointGroupByUserGuid(string userGuid, string appUid)
        {
            return dal.GetPersonalizedSetPointGroupByUserGuid(userGuid, appUid);
        }

        /// <summary>
        /// 根据用户GUID获取授权测点分组
        /// </summary>
        /// <param name="userGuid">用户Guid</param>
        /// <param name="appUid">系统应用类型Uid</param>
        /// <returns></returns>
        public DataView GetAuthPointGroupByUserGuid(string userGuid, string appUid)
        {
            return dal.GetAuthPointGroupByUserGuid(userGuid, appUid);
        }

        /// <summary>
        /// 根据用户GUID获取因子分组设置
        /// </summary>
        /// <param name="userGuid">用户Guid</param>
        /// <param name="pollutantTypeUid">系统应用类型Uid</param>
        /// <returns></returns>
        public DataView GetPersonalizedSetPollutantGroupByUserGuid(string userGuid, string pollutantTypeUid)
        {
            return dal.GetPersonalizedSetPollutantGroupByUserGuid(userGuid, pollutantTypeUid);
        }

        /// <summary>
        /// 根据用户GUID获取授权因子分组
        /// </summary>
        /// <param name="userGuid">用户Guid</param>
        /// <param name="appUid">系统因子类型Uid</param>
        /// <returns></returns>
        public DataView GetAuthPollutantGroupByUserGuid(string userGuid, string pollutantTypeUid)
        {
            return dal.GetAuthPollutantGroupByUserGuid(userGuid, pollutantTypeUid);
        }

        /// <summary>
        ///根据用户GUID删除授权设置
        /// </summary>
        /// <param name="userGuid">用户Guid</param>
        /// <param name="appUid">系统因子类型Uid</param>
        /// <param name="paramType">个性化类型（port：站点、pollutant：因子）</param>
        /// <param name="notDelParamUid">id1','id2','id3</param>
        public void DelPersonalizedSetByUserGuid(string userGuid, string appUid, string paramType, string notDelParamUid = null)
        {
            dal.DelPersonalizedSetByUserGuid(userGuid, appUid, paramType, notDelParamUid);
        }

        /// <summary>
        /// 更新用户授权设置
        /// </summary>
        /// <param name="userGuid">用户Guid</param>
        /// <param name="paramUid">参数Uid</param>
        /// <param name="paramType">个性化类型（port：站点、pollutant：因子）</param>
        /// <param name="enable">是否启用（1：启用、0：禁用）</param>
        public void UpdatePersonalizedSetByUserGuid(string userGuid, string paramUid,
                                                    string paramType, string enable)
        {
            dal.UpdatePersonalizedSetByUserGuid(userGuid, paramUid, paramType, enable);
        }

        /// <summary>
        /// 添加用户授权设置
        /// </summary>
        /// <param name="userGuid">用户Guid</param>
        /// <param name="appUid">系统因子类型Uid</param>
        /// <param name="paramType">个性化类型（port：站点、pollutant：因子）</param>
        /// <param name="paramUids">参数Uid列表</param>
        public void AddPersonalizedSet(string userGuid, string appUid,
                                       string paramType, string[] paramUids)
        {
            dal.AddPersonalizedSet(userGuid, appUid, paramType, paramUids);
        }
    }
}
