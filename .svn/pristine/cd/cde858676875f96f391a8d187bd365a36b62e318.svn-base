using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using SmartEP.BaseInfoRepository.BusinessRule;
using SmartEP.DomainModel.BaseData;

namespace SmartEP.Service.BaseData.BusinessRule
{
    public class PersonalizedSetService
    {
        private PersonalizedSetRepository repository = new PersonalizedSetRepository();

        /// <summary>
        /// 根据用户GUID获取授权设置
        /// </summary>
        /// <param name="userGuid">用户Guid</param>
        /// <param name="appUid">系统应用类型Uid</param>
        /// <returns></returns>
        public DataTable GetPersonalizedSetByUserGuid(string userGuid, string appUid)
        {
            return repository.GetPersonalizedSetByUserGuid(userGuid, appUid);
        }

        /// <summary>
        /// 根据用户GUID获取测点分组设置
        /// </summary>
        /// <param name="userGuid">用户Guid</param>
        /// <param name="appUid">系统应用类型Uid</param>
        /// <returns></returns>
        public DataView GetPersonalizedSetPointGroupByUserGuid(string userGuid, string appUid)
        {
            return repository.GetPersonalizedSetPointGroupByUserGuid(userGuid, appUid);
        }

        /// <summary>
        /// 根据用户GUID获取授权测点分组
        /// </summary>
        /// <param name="userGuid">用户Guid</param>
        /// <param name="appUid">系统应用类型Uid</param>
        /// <returns></returns>
        public DataView GetAuthPointGroupByUserGuid(string userGuid, string appUid)
        {
            return repository.GetAuthPointGroupByUserGuid(userGuid, appUid);
        }

        /// <summary>
        /// 根据用户GUID获取因子分组设置
        /// </summary>
        /// <param name="userGuid">用户Guid</param>
        /// <param name="pollutantTypeUid">系统应用类型Uid</param>
        /// <returns></returns>
        public DataView GetPersonalizedSetPollutantGroupByUserGuid(string userGuid, string pollutantTypeUid)
        {
            return repository.GetPersonalizedSetPollutantGroupByUserGuid(userGuid, pollutantTypeUid);
        }

        /// <summary>
        /// 根据用户GUID获取授权因子分组
        /// </summary>
        /// <param name="userGuid">用户Guid</param>
        /// <param name="appUid">系统因子类型Uid</param>
        /// <returns></returns>
        public DataView GetAuthPollutantGroupByUserGuid(string userGuid, string pollutantTypeUid)
        {
            return repository.GetAuthPollutantGroupByUserGuid(userGuid, pollutantTypeUid);
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
            repository.DelPersonalizedSetByUserGuid(userGuid, appUid, paramType, notDelParamUid);
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
            repository.UpdatePersonalizedSetByUserGuid(userGuid, paramUid, paramType, enable);
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
            repository.AddPersonalizedSet(userGuid, appUid, paramType, paramUids);
        }

        /// <summary>
        /// 取得个性化的站点
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        public IQueryable<PersonalizedSettingEntity> GetPersonalizedPoint(string userGuid)
        {
            return repository.Retrieve(x => x.UserUid.Equals(userGuid) && x.ParameterType == "port" && x.EnableCustomOrNot != null && x.EnableCustomOrNot.Value == true);
        }

        /// <summary>
        /// 取得个性化的因子
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        public IQueryable<PersonalizedSettingEntity> GetPersonalizedPollutant(string userGuid)
        {
            return repository.Retrieve(x => x.UserUid.Equals(userGuid) && x.ParameterType == "pollutant" && x.EnableCustomOrNot != null && x.EnableCustomOrNot.Value == true);
        }
    }
}
