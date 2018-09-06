using SmartEP.Core.Generic;
using SmartEP.Data.SqlServer.MonitoringBusiness.Water;
using SmartEP.DomainModel.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.MonitoringBusinessRepository.Water
{
    /// <summary>
    /// 名称：OM_ReagentInBillItemRepository.cs
    /// 创建人：刘长敏
    /// 创建日期：2016-08-30
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 地表水发布：标液配置表仓储层
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class OM_ReagentInBillItemRepository
    {
        OM_ReagentInBillItemDAL d_ReagentInBillItemDAL = Singleton<OM_ReagentInBillItemDAL>.GetInstance();

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns>返回插入条数</returns>
        public int Add(OM_ReagentInBillItemEntiy model)
        {
            if (model != null)
            {
                return d_ReagentInBillItemDAL.Add(model);
            }
            return 0;
        }
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns>返回插入条数</returns>
        public int AddBill(OM_ReagentInBillEntiy model)
        {
            if (model != null)
            {
                return d_ReagentInBillItemDAL.AddBill(model);
            }
            return 0;
        }
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns>返回插入条数</returns>
        public int AddDetail(OM_ReagentInItemDetailEntiy model)
        {
            if (model != null)
            {
                return d_ReagentInBillItemDAL.AddDetail(model);
            }
            return 0;
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int AddStyle(OM_ReagentTypeEntiy model)
        {
            if (model != null)
            {
                return d_ReagentInBillItemDAL.AddStyle(model);
            }
            return 0;
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int AddRea(OM_ReagentEntiy model)
        {
            if (model != null)
            {
                return d_ReagentInBillItemDAL.AddRea(model);
            }
            return 0;
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns>返回是否更新成功</returns>
        public bool Update(string[] model)
        {
            if (model != null)
            {
                return d_ReagentInBillItemDAL.Update(model);
            }
            return false;
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns>返回是否更新成功</returns>
        public bool Update(string model, string moveVolume)
        {
            if (model != null)
            {
                return d_ReagentInBillItemDAL.Update(model, moveVolume);
            }
            return false;
        }
        /// <summary>
        /// 获取系统编号数字部分
        /// </summary>
        /// <returns></returns>
        public string Count()
        {
            return d_ReagentInBillItemDAL.Count();
        }
        /// <summary>
        /// 插入空数据
        /// </summary>
        /// <param name="BillItemGuid"></param>
        /// <param name="SysNum"></param>
        public void InsertNullData(Guid BillItemGuid, string SysNum)
        {
            d_ReagentInBillItemDAL.InsertNullData(BillItemGuid, SysNum);
        }
        /// <summary>
        /// 查询母溶液的稀释倍数与浓度
        /// </summary>
        /// <param name="RowGuids"></param>
        /// <returns></returns>
        public DataTable SelectMultipleAndConcentration(string RowGuids)
        {
            return d_ReagentInBillItemDAL.SelectMultipleAndConcentration(RowGuids);
        }
        /// <summary>
        /// 查询母溶液的浓度
        /// </summary>
        /// <param name="RowGuids"></param>
        /// <returns></returns>
        public DataTable SelectConcentration(string RowGuids, string ProNum)
        {
            return d_ReagentInBillItemDAL.SelectConcentration(RowGuids, ProNum);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="IDs">IDs</param>
        /// <returns></returns>
        public bool Delete(int IDs)
        {
            return d_ReagentInBillItemDAL.Delete(IDs);
        }

        /// <summary>
        /// 显示数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetList(string strWhere)
        {
            return d_ReagentInBillItemDAL.GetList(strWhere);
        }
        /// <summary>
        /// 显示数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetLists(string strWhere)
        {
            return d_ReagentInBillItemDAL.GetLists(strWhere);
        }
        /// <summary>
        /// 显示数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetListStyle(string strWhere)
        {
            return d_ReagentInBillItemDAL.GetListStyle(strWhere);
        }
        /// <summary>
        /// 显示数据
        /// </summary>
        /// <returns>获取符合条件的数据</returns> 
        public string rowGuid(string strWhere)
        {
            return d_ReagentInBillItemDAL.rowGuid(strWhere);
        }
    }
}
