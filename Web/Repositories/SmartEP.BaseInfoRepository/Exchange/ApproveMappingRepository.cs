using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.BaseData;
using System.Linq;

namespace SmartEP.BaseInfoRepository.Exchange
{
    /// <summary>
    /// 名称：ApproveMappingRepository.cs
    /// 创建人：李飞
    /// 创建日期：2015-12-20
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// FTP处理
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class ApproveMappingRepository : BaseGenericRepository<BaseDataModel, DT_ApproveMappingEntity>
    {
        public override bool IsExist(string strKey)
        {
            return Retrieve(x => x.RowGuid.Equals(strKey)).Count() == 0 ? false : true;
        }
    }
}
