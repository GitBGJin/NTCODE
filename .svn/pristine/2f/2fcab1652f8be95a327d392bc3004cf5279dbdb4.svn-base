using SmartEP.Core.Generic;
using SmartEP.DomainModel.Framework;
using SmartEP.MonitoringBusinessRepository.Water;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.DataAnalyze.Water
{
    /// <summary>
    /// 名称：OM_ReagentInBillItemSevice.cs
    /// 创建人：刘长敏
    /// 创建日期：2016-08-30
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    ///标液配置服务层类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class OM_ReagentInBillItemSevice
    {
        /// <summary>
        /// 空气质量预报表仓储层
        /// </summary>
        OM_ReagentInBillItemRepository r_ReagentInBillItem = Singleton<OM_ReagentInBillItemRepository>.GetInstance();
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="AQIForecast">质量预报实体</param>
        /// <returns>成功返回1，失败返回0，实体数组空返回2</returns>
        public int Add(OM_ReagentInBillItemEntiy[] abnormal)
        {
            int num = 0;
            for (int i = 0; i < abnormal.Length; i++)
            {
                num += r_ReagentInBillItem.Add(abnormal[i]);
            }
            //成功返回1，失败返回0
            if (num == abnormal.Length)
            {
                return abnormal.Length == 0 ? 2 : 1;
            }
            else
            {
                return 0;
            }

            //return r_ReagentInBillItem.Add(abnormal);
        }
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="AQIForecast">质量预报实体</param>
        /// <returns>成功返回1，失败返回0，实体数组空返回2</returns>
        public int AddBill(OM_ReagentInBillEntiy[] abnormal)
        {
            int num = 0;
            for (int i = 0; i < abnormal.Length; i++)
            {
                num += r_ReagentInBillItem.AddBill(abnormal[i]);
            }
            //成功返回1，失败返回0
            if (num == abnormal.Length)
            {
                return abnormal.Length == 0 ? 2 : 1;
            }
            else
            {
                return 0;
            }

            //return r_ReagentInBillItem.Add(abnormal);
        }
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="AQIForecast">质量预报实体</param>
        /// <returns>成功返回1，失败返回0，实体数组空返回2</returns>
        public int AddDetail(OM_ReagentInItemDetailEntiy[] abnormal)
        {
            int num = 0;
            for (int i = 0; i < abnormal.Length; i++)
            {
                num += r_ReagentInBillItem.AddDetail(abnormal[i]);
            }
            //成功返回1，失败返回0
            if (num == abnormal.Length)
            {
                return abnormal.Length == 0 ? 2 : 1;
            }
            else
            {
                return 0;
            }

        }
              /// <summary>
        /// 增加一条数据
        /// </summary>
        public int AddStyle(OM_ReagentTypeEntiy[] model)
        {
            int num = 0;
            for (int i = 0; i < model.Length; i++)
            {
                num += r_ReagentInBillItem.AddStyle(model[i]);
            }
            //成功返回1，失败返回0
            if (num == model.Length)
            {
                return model.Length == 0 ? 2 : 1;
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int AddRea(OM_ReagentEntiy[] model)
        {
            int num = 0;
            for (int i = 0; i < model.Length; i++)
            {
                num += r_ReagentInBillItem.AddRea(model[i]);
            }
            //成功返回1，失败返回0
            if (num == model.Length)
            {
                return model.Length == 0 ? 2 : 1;
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="AQIForecast">质量预报实体</param>
        /// <returns>成功返回true，失败返回false</returns>
        public bool Update(string[] abnormal)
        {
            return r_ReagentInBillItem.Update(abnormal);
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="AQIForecast">质量预报实体</param>
        /// <returns>成功返回true，失败返回false</returns>
        public bool Update(string abnormal, string moveVolume)
        {
            return r_ReagentInBillItem.Update(abnormal, moveVolume);
        }
        /// <summary>
        /// 获取系统编号数字部分
        /// </summary>
        /// <returns></returns>
        public string Count()
        {
            return r_ReagentInBillItem.Count();
        }
        /// <summary>
        /// 插入空数据
        /// </summary>
        /// <param name="BillItemGuid"></param>
        /// <param name="SysNum"></param>
        public void InsertNullData(Guid BillItemGuid, string SysNum)
        {
            r_ReagentInBillItem.InsertNullData(BillItemGuid, SysNum);
        }
        /// <summary>
        /// 查询母溶液的稀释倍数与浓度
        /// </summary>
        /// <param name="RowGuids"></param>
        public DataTable SelectMultipleAndConcentration(string RowGuids)
        {
            return r_ReagentInBillItem.SelectMultipleAndConcentration(RowGuids);
        }
        /// <summary>
        /// 查询母溶液的与浓度
        /// </summary>
        /// <param name="RowGuids"></param>
        public DataTable SelectConcentration(string RowGuids,string ProNum)
        {
            return r_ReagentInBillItem.SelectConcentration(RowGuids, ProNum);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>成功返回true，失败返回false</returns>
        public bool Delete(int IDs)
        {
            return r_ReagentInBillItem.Delete(IDs);
        }

        /// <summary>
        /// 显示数据
        /// </summary>
        /// <returns>获取符合条件的数据</returns>
        public DataTable GetList(string strWhere)
        {
            return r_ReagentInBillItem.GetList(strWhere);
        }
        /// <summary>
        /// 显示数据
        /// </summary>
        /// <returns>获取符合条件的数据</returns>
        public DataTable GetLists(string strWhere)
        {
            return r_ReagentInBillItem.GetLists(strWhere);
        }

        /// <summary>
        /// 显示数据
        /// </summary>
        /// <returns>获取符合条件的数据</returns>
        public DataTable GetListStyle(string strWhere)
        {
            return r_ReagentInBillItem.GetListStyle(strWhere);
        }

        /// <summary>
        /// 显示数据
        /// </summary>
        /// <returns>获取符合条件的数据</returns> 
        public string rowGuid(string strWhere)
        {
            return r_ReagentInBillItem.rowGuid(strWhere);
        }
    }
}
