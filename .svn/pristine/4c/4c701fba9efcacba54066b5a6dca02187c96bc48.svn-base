using SmartEP.BaseInfoRepository.Exchange;
using SmartEP.Core.Generic;
using SmartEP.DomainModel.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.Service.BaseData.Exchange
{
    /// <summary>
    /// 名称：ApproveMappingService.cs
    /// 创建人：李飞
    /// 创建日期：2015-12-20
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// FTP处理
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class ApproveMappingService
    {
        /// <summary>
        /// FTP处理仓储层
        /// </summary>
        private ApproveMappingRepository approveMappingRepository = Singleton<ApproveMappingRepository>.GetInstance();

        #region 增删改
        /// <summary>
        /// 增加等级标准
        /// </summary>
        /// <param name="etity">实体</param>
        public void Add(DT_ApproveMappingEntity etity)
        {
            approveMappingRepository.Add(etity);
        }

        /// <summary>
        /// 更新等级标准
        /// </summary>
        /// <param name="etity">实体</param>
        public void Update(DT_ApproveMappingEntity etity)
        {
            approveMappingRepository.Update(etity);
        }

        /// <summary>
        /// 删除等级标准
        /// </summary>
        /// <param name="etity">实体</param>
        public void Delete(DT_ApproveMappingEntity etity)
        {
            approveMappingRepository.Delete(etity);
        }
        #endregion

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public IQueryable<DT_ApproveMappingEntity> RetrieveList()
        {
            return approveMappingRepository.RetrieveAll();
        }

        /// <summary>
        /// 根据rowGuid获取配置
        /// </summary>
        /// <param name="eqiUid"></param>
        /// <returns></returns>
        public DT_ApproveMappingEntity RetrieveByUid(string rowGuid)
        {
            return RetrieveList().FirstOrDefault(a => a.RowGuid == rowGuid);
        }
    }
}
