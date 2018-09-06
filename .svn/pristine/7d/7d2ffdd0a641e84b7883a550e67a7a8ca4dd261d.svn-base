using SmartEP.BaseInfoRepository.Standard;
using SmartEP.Core.Enums;
using SmartEP.DomainModel.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.Service.BaseData.Standard
{
    /// <summary>
    /// 名称：EQIService.cs
    /// 创建人：季柯
    /// 创建日期：2015-08-18
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：提供污染等级服务（如空气质量指数、水质等级）
    /// 版权所有(C)：江苏远大信息股份有限公司
    public class EQIService : IEQI
    {
        private EQIRepository eqiRepository = new EQIRepository();
        private string applicationValue = SmartEP.Core.Enums.EnumMapping.GetApplicationValue(ApplicationValue.Air);

        #region << 构造函数 >>
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="applicationType">应用类型（空气、地表水、噪声）</param>
        public EQIService(ApplicationType applicationType)
        {
            applicationValue = SmartEP.Core.Enums.EnumMapping.GetApplicationValue(applicationType);
        }
        #endregion

        #region 增删改
        /// <summary>
        /// 增加等级标准
        /// </summary>
        /// <param name="InstrumentChannel">实体</param>
        public void Add(EQIEntity eqi)
        {
            eqiRepository.Add(eqi);
        }

        /// <summary>
        /// 更新等级标准
        /// </summary>
        /// <param name="InstrumentChannel">实体</param>
        public void Update(EQIEntity eqi)
        {
            eqiRepository.Update(eqi);
        }

        /// <summary>
        /// 删除等级标准
        /// </summary>
        /// <param name="monitoringPoint">实体</param>
        public void Delete(EQIEntity eqi)
        {
            eqiRepository.Delete(eqi);
        }
        #endregion

        /// <summary>
        /// 获取空气质量等级标准列表
        /// </summary>
        /// <returns></returns>
        public IQueryable<EQIEntity> RetrieveList()
        {
            return eqiRepository.Retrieve(a => a.ApplicationUid == applicationValue);
        }

        /// <summary>
        /// 根据EQIUid获取空气质量等级
        /// </summary>
        /// <param name="eqiUid"></param>
        /// <returns></returns>
        public EQIEntity RetrieveByUid(string eqiUid)
        {
            return RetrieveList().FirstOrDefault(a => a.EQIUid == eqiUid);
        }


    }
}
