using SmartEP.BaseInfoRepository.Channel;
using SmartEP.Core.Enums;
using SmartEP.DomainModel.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartEP.WebControl.CbxRsm;
using System.Linq.Expressions;

namespace SmartEP.Service.BaseData.Channel
{
    /// <summary>
    /// 名称：AirPollutantService.cs
    /// 创建人：季柯
    /// 创建日期：2015-08-18
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 空气污染物基础信息服务
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class WaterPollutantService : IPollutant
    {
        //通道仓储层
        private PollutantCodeRepository g_Repository = new PollutantCodeRepository();
        //水污染物类型值
        private string pollutantTypeValue = SmartEP.Core.Enums.EnumMapping.GetPollutantTypeValue(PollutantTypeValue.Water);
        private string applicationUid = SmartEP.Core.Enums.EnumMapping.GetApplicationValue(ApplicationValue.Water);
        #region 增删改
        /// <summary>
        /// 增加通道
        /// </summary>
        /// <param name="channel">通道实体</param>
        public void Add(PollutantCodeEntity channel)
        {
            g_Repository.Add(channel);
        }
        /// <summary>
        /// 更新通道
        /// </summary>
        /// <param name="channel">通道实体</param>
        public void Delete(PollutantCodeEntity channel)
        {
            g_Repository.Delete(channel);
        }
        /// <summary>
        /// 删除通道
        /// </summary>
        /// <param name="channel">通道实体</param>
        public void Update(PollutantCodeEntity channel)
        {
            g_Repository.Update(channel);
        }
        #endregion

        /// <summary>
        /// 获取所有空气通道
        /// </summary>
        /// <returns></returns>
        public IQueryable<PollutantCodeEntity> RetrieveList()
        {
            return g_Repository.Retrieve(p => p.PollutantTypeUid == pollutantTypeValue);
        }

        /// <summary>
        /// 根据是否启动获取所有空气通道
        /// </summary>
        /// <returns></returns>
        public IQueryable<PollutantCodeEntity> RetrieveListByUseOrNot(bool isUseOrNot)
        {
            return g_Repository.Retrieve(p => p.PollutantTypeUid == pollutantTypeValue && p.IsUseOrNot == isUseOrNot);
        }
        /// <summary>
        /// 根据通道Uid获取通道
        /// </summary>
        /// <param name="channelUid"></param>
        /// <returns></returns>
        public PollutantCodeEntity RetrieveEntityByUid(string channelUid)
        {
            return g_Repository.RetrieveFirstOrDefault(p => p.PollutantUid == channelUid);
        }
        /// <summary>
        /// 根据通道编码获取通道
        /// </summary>
        /// <param name="channelCode">通道编码</param>
        /// <returns></returns>
        public  PollutantCodeEntity RetrieveEntityByCode(string channelCode)
        {
            return g_Repository.RetrieveFirstOrDefault(p => p.PollutantCode == channelCode);
        }
        /// <summary>
        /// 根据通道名获取通道
        /// </summary>
        /// <param name="channelName"></param>
        /// <returns></returns>
        public PollutantCodeEntity RetrieveEntityByName(string channelName)
        {
            return g_Repository.RetrieveFirstOrDefault(p => p.PollutantTypeUid == pollutantTypeValue && p.PollutantName == channelName);
        }

        /// <summary>
        /// 地表水通道查询
        /// </summary>
        /// <param name="channelName">通道名称</param>
        /// <param name="channelCode">通道编码</param>
        /// <param name="indicatorUid">污染物指标Uid</param>
        /// <param name="categoryUid">因子种类Uid</param>
        /// <param name="sourceTypeUid">通道来源Uid</param>
        /// <param name="typeUid">通道类型Uid</param>
        /// <returns></returns>
        public IQueryable<PollutantCodeEntity> RetrieveList(string channelName, string channelCode, string indicatorUid, string categoryUid, string sourceTypeUid, string typeUid)
        {
            IQueryable<PollutantCodeEntity> pollutantEntities = g_Repository.Retrieve(p => p.PollutantTypeUid == pollutantTypeValue);
            if (!string.IsNullOrEmpty(channelName)) pollutantEntities = pollutantEntities.Where(p => p.PollutantName.Contains(channelName));
            if (!string.IsNullOrEmpty(channelCode)) pollutantEntities = pollutantEntities.Where(p => p.PollutantCode == channelCode);
            if (!string.IsNullOrEmpty(indicatorUid)) pollutantEntities = pollutantEntities.Where(p => p.IndicatorUid == indicatorUid);
            if (!string.IsNullOrEmpty(categoryUid)) pollutantEntities = pollutantEntities.Where(p => p.CategoryUid == categoryUid);
            if (!string.IsNullOrEmpty(sourceTypeUid)) pollutantEntities = pollutantEntities.Where(p => p.SourceTypeUid == sourceTypeUid);
            if (!string.IsNullOrEmpty(typeUid)) pollutantEntities = pollutantEntities.Where(p => p.TypeUid == typeUid);

            return pollutantEntities;
        }

        /// <summary>
        /// 获取因子选择SiteMap绑定数据源
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        public IQueryable<V_Factor_SiteMap_UserConfigEntity> RetrieveSiteMapList(string userGuid)
        {
            return g_Repository.Context.GetAll<V_Factor_SiteMap_UserConfigEntity>().Where(p => p.ApplicationUid == applicationUid && p.UserGuid == userGuid);
        }

        /// <summary>
        /// 根据因子编码获取因子其他信息
        /// </summary>
        /// <param name="channelCode"></param>
        /// <returns></returns>
        public SmartEP.Core.Interfaces.IPollutant GetPollutantInfo(string channelCode)
        {
            PollutantCodeEntity pollutant = g_Repository.RetrieveFirstOrDefault(p => p.PollutantCode == channelCode);
            V_FactorEntity factor = (from fac in g_Repository.Context.GetAll<V_FactorEntity>()
                                     where fac.PollutantUid.ToUpper().Equals(pollutant.PollutantUid.ToUpper())
                                     select fac).FirstOrDefault();
            return new RsmFactor(factor.PollutantName, factor.PollutantCode, factor.DecimalDigit.ToString(), factor.Unit, factor.PollutantUid);
        }

        /// <summary>
        /// 获取指定因子列表
        /// </summary>
        /// <param name="factors"></param>
        /// <returns></returns>
        public List<SmartEP.Core.Interfaces.IPollutant> GetDefaultFactors(string[] factors)
        {
            List<SmartEP.Core.Interfaces.IPollutant> pollList = new List<SmartEP.Core.Interfaces.IPollutant>();
            pollList = (from fac in g_Repository.Context.GetAll<V_FactorEntity>()
                                   where factors.Contains(fac.PollutantCode)
                        select new RsmFactor(fac.PollutantName, fac.PollutantCode, fac.DecimalDigit.Value.ToString(),fac.Unit,fac.PollutantUid)).ToList<SmartEP.Core.Interfaces.IPollutant>();
            return pollList;

        }

        /// <summary>
        /// 根据lambda表达式要求，返回所有对象
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual IQueryable<PollutantCodeEntity> Retrieve(Expression<Func<PollutantCodeEntity, bool>> predicate)
        {
            return g_Repository.Retrieve(predicate);
        }
    }
}
