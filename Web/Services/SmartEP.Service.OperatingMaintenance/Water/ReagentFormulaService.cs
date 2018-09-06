using SmartEP.Core.Generic;
using SmartEP.MonitoringBusinessRepository.QualityControlOperation.Water;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.OperatingMaintenance.Water
{
    /// <summary>
    /// 名称：ReagentFormulaService.cs
    /// 创建人：樊垂贺
    /// 创建日期：2016-08-30
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 试剂配置数据
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class ReagentFormulaService
    {
        ReagentFormulaRepository g_ReagentFormulaRepository = Singleton<ReagentFormulaRepository>.GetInstance();
        /// <summary>
        /// 获取试剂类型
        /// </summary>
        /// <returns></returns>
        public DataTable GetReagentKind()
        {
            return g_ReagentFormulaRepository.GetReagentKind();
        }
        /// <summary>
        /// 根据仪器类型获取试剂类型
        /// </summary>
        /// <param name="InstanceGuid"></param>
        /// <returns></returns>
        public DataTable GetReagentKind(string InstanceGuid)
        {
            return g_ReagentFormulaRepository.GetReagentKind(InstanceGuid);
        }
        /// <summary>
        /// 获取仪器类型
        /// </summary>
        /// <returns></returns>
        public DataTable GetInstanceKind()
        {
            return g_ReagentFormulaRepository.GetInstanceKind();
        }

        /// <summary>
        /// 根据试剂类型获取化学品
        /// </summary>
        /// <param name="ReagentGuid"></param>
        /// <returns></returns>
        public DataTable GetReagentFormula(string ReagentGuid)
        {
            return g_ReagentFormulaRepository.GetReagentFormula(ReagentGuid);
        }

        /// <summary>
        /// 根据仪器类型和试剂类型获取化学品
        /// </summary>
        /// <param name="ReagentGuid"></param>
        /// <param name="InstanceGuid"></param>
        /// <returns></returns>
        public DataTable GetReagentFormula(string ReagentGuid,string InstanceGuid)
        {
            return g_ReagentFormulaRepository.GetReagentFormula(ReagentGuid, InstanceGuid);
        }
        /// <summary>
        /// 更新入库表和配方记录表
        /// </summary>
        /// <param name="sqlBill"></param>
        /// <param name="sqlFormula"></param>
        public bool InsertTable(string sqlBill, string sqlFormula, out string msg)
        {
            return g_ReagentFormulaRepository.InsertTable(sqlBill, sqlFormula, out msg);
        }
        /// <summary>
        /// 试剂删除
        /// </summary>
        /// <param name="billGuid"></param>
        /// <param name="billItemGuid"></param>
        /// <param name="FormulaCode"></param>
        /// <param name="ReagentGuid"></param>
        /// <param name="DetailGuid"></param>
        public void deleteFormula(string billGuid, string billItemGuid, string FormulaCode, string ReagentGuid, string DetailGuid)
        {
            g_ReagentFormulaRepository.deleteFormula(billGuid, billItemGuid, FormulaCode, ReagentGuid, DetailGuid);
        }
        /// <summary>
        /// 获取试剂配方
        /// </summary>
        /// <returns></returns>
        public DataTable getFormulaInstance()
        {
            return g_ReagentFormulaRepository.getFormulaInstance();
        }

        /// <summary>
        /// 获取标液类型
        /// </summary>
        /// <returns></returns>
        public DataTable GetReagentType()
        {
            return g_ReagentFormulaRepository.GetReagentType();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        public bool DeleteData(string sql)
        {
            return g_ReagentFormulaRepository.DeleteData(sql);
        }
        /// <summary>
        /// 增加数据
        /// </summary>
        /// <param name="sqlBillItem"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool AddItem(string sqlBillItem, out string msg)
        {
            return g_ReagentFormulaRepository.AddItem(sqlBillItem, out msg);
        }
        /// <summary>
        /// 获取试剂配制默认人
        /// </summary>
        /// <returns></returns>
        public string GetDefaultPerson()
        {
            return g_ReagentFormulaRepository.GetDefaultPerson();
        }

        /// <summary>
        /// 获取系统编号
        /// </summary>
        /// <returns></returns>
        public string GetProductSN()
        {
            return g_ReagentFormulaRepository.GetProductSN();
        }
        /// <summary>
        /// 插入空数据
        /// </summary>
        /// <param name="BillItemGuid"></param>
        /// <param name="SysNum"></param>
        public void InsertNullData(Guid BillItemGuid, string SysNum)
        {
            g_ReagentFormulaRepository.InsertNullData(BillItemGuid, SysNum);
        }
        /// <summary>
        /// 根据系统编号获取RowGuid
        /// </summary>
        /// <param name="SysNum"></param>
        /// <returns></returns>
        public Guid GetGuid(string SysNum)
        {
            return g_ReagentFormulaRepository.GetGuid(SysNum);
        }
    }
}
