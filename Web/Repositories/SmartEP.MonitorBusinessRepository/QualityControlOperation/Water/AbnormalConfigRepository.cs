using SmartEP.Core.Generic;
using SmartEP.Data.SqlServer.QualityControlOperation.Water;
using SmartEP.DomainModel.WaterQualityControlOperation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.MonitoringBusinessRepository.QualityControlOperation.Water
{
    /// <summary>
    /// 名称：AbnormalConfigRepository.cs
    /// 创建人：窦曙健
    /// 创建日期：2015-09-23
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 地表水发布：异常配置表仓储层
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class AbnormalConfigRepository
    {
        AbnormalConfigDAL d_AbnormalConfigDAL = Singleton<AbnormalConfigDAL>.GetInstance();

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns>返回插入条数</returns>
        public int Add(AbnormalConfigEntity model)
        {
            if (model != null)
            {
                return d_AbnormalConfigDAL.Add(model);
            }
            return 0;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns>返回是否更新成功</returns>
        public bool Update(AbnormalConfigEntity model)
        {
            if (model != null)
            {
                return d_AbnormalConfigDAL.Update(model);
            }
            return false;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public bool SingleDelete(int id)
        {
            return d_AbnormalConfigDAL.Delete(id);
        }

        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="pointId">站点编号</param>
        /// <param name="pollingDate">巡检时间</param>
        /// <returns></returns>
        public DataTable GetList(string strWhere)
        {
            return d_AbnormalConfigDAL.GetList(strWhere);
        }
        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="pointId">站点编号</param>
        /// <param name="pollingDate">巡检时间</param>
        /// <returns></returns>
        public DataTable GetInstrumentNew(string[] strWhere)
        {
            return d_AbnormalConfigDAL.GetInstrumentNew(strWhere);
        }
        /// <summary>
        /// 获取前几条数据
        /// </summary>
        /// <param name="Top"></param>
        /// <param name="pointId"></param>
        /// <param name="ChangeDate"></param>
        /// <param name="filedOrder"></param>
        /// <returns></returns>
        public DataTable GetList(int Top, string strWhere, string filedOrder)
        {
            return d_AbnormalConfigDAL.GetList(Top, strWhere, filedOrder);
        }
    }
}
