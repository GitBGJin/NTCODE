using SmartEP.DomainModel.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.Service.BaseData.Channel
{
    /// <summary>
    /// 名称：IChannel.cs
    /// 创建人：季柯
    /// 创建日期：2015-08-16
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 通道信息接口
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
     public interface IChannel
     {

         #region 方法
         void Add(PollutantCodeEntity channel);
         void Delete(PollutantCodeEntity channel);
         void Update(PollutantCodeEntity channel);

        IQueryable<PollutantCodeEntity> RetrieveList();
        PollutantCodeEntity RetrieveEntityByUid(string channelUid);
        PollutantCodeEntity RetrieveEntityByCode(string channelCode);
        PollutantCodeEntity RetrieveEntityByName(string channelName);
         #endregion

     }
}
