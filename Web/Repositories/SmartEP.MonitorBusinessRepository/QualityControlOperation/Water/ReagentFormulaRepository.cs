using SmartEP.Core.Generic;
using SmartEP.Data.SqlServer.QualityControlOperation.Water;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.MonitoringBusinessRepository.QualityControlOperation.Water
{
    /// <summary>
    /// 名称：ReagentFormulaRepository.cs
    /// 创建人：樊垂贺
    /// 创建日期：2016-08-30
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 试剂配置数据
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class ReagentFormulaRepository
    {
        ReagentFormulaDAL g_ReagentFormulaDAL = Singleton<ReagentFormulaDAL>.GetInstance();
        /// <summary>
        /// 获取试剂类型
        /// </summary>
        /// <returns></returns>
        public DataTable GetReagentKind()
        {
            return g_ReagentFormulaDAL.GetReagentKind();
        }
        /// <summary>
        /// 根据仪器类型获取试剂类型
        /// </summary>
        /// <param name="InstanceGuid"></param>
        /// <returns></returns>
        public DataTable GetReagentKind(string InstanceGuid)
        {
            return g_ReagentFormulaDAL.GetReagentKind(InstanceGuid);
        }
        /// <summary>
        /// 获取仪器类型
        /// </summary>
        /// <returns></returns>
        public DataTable GetInstanceKind()
        {
            return g_ReagentFormulaDAL.GetInstanceKind();
        }

        /// <summary>
        /// 根据试剂类型获取化学品
        /// </summary>
        /// <param name="ReagentGuid"></param>
        /// <returns></returns>
        public DataTable GetReagentFormula(string ReagentGuid)
        {
            return g_ReagentFormulaDAL.GetReagentFormula(ReagentGuid);
        }

        /// <summary>
        /// 根据仪器类型和试剂类型获取化学品
        /// </summary>
        /// <param name="ReagentGuid"></param>
        /// <param name="InstanceGuid"></param>
        /// <returns></returns>
        public DataTable GetReagentFormula(string ReagentGuid, string InstanceGuid)
        {
            return g_ReagentFormulaDAL.GetReagentFormula(ReagentGuid, InstanceGuid);
        }
        /// <summary>
        /// 删除试剂
        /// </summary>
        /// <param name="billGuid"></param>
        /// <param name="billItemGuid"></param>
        /// <param name="FormulaCode"></param>
        /// <param name="ReagentGuid"></param>
        /// <param name="DetailGuid"></param>
        public void deleteFormula(string billGuid, string billItemGuid, string FormulaCode, string ReagentGuid, string DetailGuid)
        {
            g_ReagentFormulaDAL.deleteFormula(billGuid, billItemGuid, FormulaCode, ReagentGuid, DetailGuid);
        }
        /// <summary>
        /// 更新入库表和配方记录表
        /// </summary>
        /// <param name="sqlBill"></param>
        /// <param name="sqlFormula"></param>
        public bool InsertTable(string sqlBill, string sqlFormula, out string msg)
        {
            return g_ReagentFormulaDAL.InsertTable(sqlBill, sqlFormula, out msg);
        }
        /// <summary>
        /// 获取试剂配方
        /// </summary>
        /// <returns></returns>
        public DataTable getFormulaInstance()
        {
            return g_ReagentFormulaDAL.getFormulaInstance();
        }
        /// <summary>
        /// 获取标液类型
        /// </summary>
        /// <returns></returns>
        public DataTable GetReagentType()
        {
            return g_ReagentFormulaDAL.GetReagentType();
        }
        /// <summary>
        /// 删除有证物质
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public bool DeleteData(string sql)
        {
            return g_ReagentFormulaDAL.DeleteData(sql);
        }
        /// <summary>
        /// 添加有证物质
        /// </summary>
        /// <param name="sqlBillItem"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool AddItem(string sqlBillItem, out string msg)
        {
            return g_ReagentFormulaDAL.AddItem(sqlBillItem, out msg);
        }
        /// <summary>
        /// 试剂配置默认人
        /// </summary>
        /// <returns></returns>
        public string GetDefaultPerson()
        {
            return g_ReagentFormulaDAL.GetDefaultPerson();
        }
        /// <summary>
        /// 获取系统编号
        /// </summary>
        /// <returns></returns>
        public string GetProductSN()
        {
            return g_ReagentFormulaDAL.GetProductSN();
        }
        /// <summary>
        /// 插入空数据
        /// </summary>
        /// <param name="BillItemGuid"></param>
        /// <param name="SysNum"></param>
        public void InsertNullData(Guid BillItemGuid, string SysNum)
        {
            g_ReagentFormulaDAL.InsertNullData(BillItemGuid, SysNum);
        }
        /// <summary>
        /// 根据系统编号获取RowGuid
        /// </summary>
        /// <param name="SysNum"></param>
        /// <returns></returns>
        public Guid GetGuid(string SysNum)
        {
            return g_ReagentFormulaDAL.GetGuid(SysNum);
        }
    }
}
