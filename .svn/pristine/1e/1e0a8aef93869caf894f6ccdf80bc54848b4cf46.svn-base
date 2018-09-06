using SmartEP.Core.Generic;
using SmartEP.MonitoringBusinessRepository.QualityControlOperation.Water;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.OperatingMaintenance.Water
{
    
    public partial class MaterialRigisterService
    {
        MaterialRigisterRepository g_MaterialRigisterRepository = Singleton<MaterialRigisterRepository>.GetInstance();
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        public bool DeleteData(string sql)
        {
            return g_MaterialRigisterRepository.DeleteData(sql);
        }
        /// <summary>
        /// 增加数据
        /// </summary>
        /// <param name="sqlBillItem"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool AddItem(string sqlBillItem, out string msg)
        {
            return g_MaterialRigisterRepository.AddItem(sqlBillItem, out msg);
        }

        /// <summary>
        /// 获取标液类型
        /// </summary>
        /// <returns></returns>
        public DataTable GetReagentType()
        {
            return g_MaterialRigisterRepository.GetReagentType();
        }

        /// <summary>
        /// 显示数据
        /// </summary>
        /// <returns>获取符合条件的标液数据</returns>
        public DataTable GetList(string strWhere)
        {
            return g_MaterialRigisterRepository.GetList(strWhere);
        }
        /// <summary>
        /// 显示数据
        /// </summary>
        /// <returns>获取符合条件的数据</returns>
        public DataTable GetLists(string strWhere)
        {
            return g_MaterialRigisterRepository.GetLists(strWhere);
        }

        /// <summary>
        /// 显示数据
        /// </summary>
        /// <returns>获取符合条件的标液类型数据</returns>
        public DataTable GetListStyle(string strWhere)
        {
            return g_MaterialRigisterRepository.GetListStyle(strWhere);
        }
        /// <summary>
        /// 根据标液号带出数据
        /// </summary>
        /// <param name="ProductionSN"></param>
        /// <returns></returns>
        public DataTable GetData(string ProductionSN)
        {
            return g_MaterialRigisterRepository.GetData(ProductionSN);
        }
        //根据系统编号查出数据，主要获取打印专递的参数
        public DataTable GetNum(string sysNum)
        {
            return g_MaterialRigisterRepository.GetNum(sysNum);
        }

        /// <summary>
        /// 根据RowGuid获取系统编号
        /// </summary>
        /// <param name="rowID"></param>
        /// <returns></returns>
        public DataTable GetSysNum(string rowID)
        {
            return g_MaterialRigisterRepository.GetSysNum(rowID);
        }
    }
}
