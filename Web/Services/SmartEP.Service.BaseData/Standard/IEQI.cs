using SmartEP.DomainModel.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEP.Service.BaseData.Standard
{
    /// <summary>
    /// 名称：IEQI.cs
    /// 创建人：季柯
    /// 创建日期：2015-08-27
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：污染物等级标准接口
    /// 版权所有(C)：江苏远大信息股份有限公司
    public interface IEQI
    {
        #region 方法
        void Add(EQIEntity channel);
        void Delete(EQIEntity channel);
        void Update(EQIEntity channel);

        IQueryable<EQIEntity> RetrieveList();
        EQIEntity RetrieveByUid(string eqiUid);
        #endregion
    }
}
