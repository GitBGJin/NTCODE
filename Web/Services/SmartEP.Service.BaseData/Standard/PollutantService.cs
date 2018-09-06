using SmartEP.BaseInfoRepository.Standard;
using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.DomainModel.BaseData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SmartEP.Service.BaseData.Standard
{
    /// <summary>
    /// 名称：PollutantService.cs
    /// 创建人：朱佳伟
    /// 创建日期：2015-11-10
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 环境发布：污染因子数据
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class PollutantService
    {
        /// <summary>
        /// 数据接口
        /// </summary>
        PollutantRepository g_PollutantRepository = Singleton<PollutantRepository>.GetInstance();

        #region 获取站点对应的所有因子
        /// <summary>
        /// 获取站点对应的所有因子
        /// </summary>
        /// <param name="PointUids">站点Uid数组</param>
        /// <param name="applicationType">应用类型</param>
        /// <returns></returns>
        public DataTable GetPollutantsByPointUid(string[] PointUids, ApplicationType applicationType)
        {
            if (PointUids == null || PointUids.Length == 0)
            {
                return null;
            }
            string ApplicationUid = SmartEP.Core.Enums.EnumMapping.GetDesc(applicationType);
            string PollutantTypeUid = null;
            switch (applicationType)
            {
                //数据来源[EQMS_Framework].[dbo].[V_CodeMainItem].[ItemGuid]
                //大气污染物
                case ApplicationType.Air: PollutantTypeUid = "8D89B62D-36E1-4F05-B00D-3A585F6A90D7"; break;
                //水污染物
                case ApplicationType.Water: PollutantTypeUid = "80CA99DE-3B78-422F-9BAA-D47F23324231"; break;
            }
            if (PollutantTypeUid == null)
            {
                return null;
            }
            V_Point_InstrumentChannelEntity[] query = g_PollutantRepository.Retrieve(x => PointUids.Contains(x.MonitoringPointUid) && x.PollutantTypeUid == PollutantTypeUid && x.TypeUid == "ae39f55e-5c43-4b4a-b224-0b925b5f3c9f").OrderByDescending(x => x.OrderByNum).ToArray();
            if (query.Length == 0)
            {
                return null;
            }
            DataTable dt = new DataTable();
            dt.Columns.Add("PollutantUid", typeof(string));
            dt.Columns.Add("PollutantCode", typeof(string));
            dt.Columns.Add("PollutantName", typeof(string));

            foreach (V_Point_InstrumentChannelEntity vp in query)
            {
                DataRow dr = dt.NewRow();
                dr["PollutantUid"] = vp.PollutantUid;
                dr["PollutantCode"] = vp.PollutantCode;
                dr["PollutantName"] = vp.PollutantName;
                dt.Rows.Add(dr);
            }

            DataView dv = new DataView(dt);
            //删除重复行
            return dv.ToTable(true, new string[] { "PollutantUid", "PollutantCode", "PollutantName" });

        }

        #endregion

        #region 获取代码项配置中的因子数据
        /// <summary>
        /// 获取代码项配置中的因子数据
        /// </summary>
        /// <param name="typeName">代码分类</param>
        /// <param name="codeName">代码名称</param>
        /// <returns>DataTable</returns>
        public DataTable GetPollutantDataByItem(string typeName, string codeName)
        {
            return g_PollutantRepository.GetPollutantDataByItem(typeName, codeName);
        }
        #endregion

        /// <summary>
        /// 根据lambda表达式要求，返回所有对象
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IQueryable<V_Point_InstrumentChannelEntity> Retrieve(Expression<Func<V_Point_InstrumentChannelEntity, bool>> predicate)
        {
            return g_PollutantRepository.Retrieve(predicate);
        }
    }
}
