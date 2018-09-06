using SmartEP.Core.Generic;
using SmartEP.Data.SqlServer.QualityControlOperation.Water;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.MonitoringBusinessRepository.QualityControlOperation.Water
{
      
    public partial class MaterialRigisterRepository
    {
        MaterialRigisterDAL g_MaterialRigisterDAL = Singleton<MaterialRigisterDAL>.GetInstance();
        /// <summary>
        /// 获取标液类型
        /// </summary>
        /// <returns></returns>
        public DataTable GetReagentType()
        {
            return g_MaterialRigisterDAL.GetReagentType();
        }
        /// <summary>
        /// 删除有证物质
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public bool DeleteData(string sql)
        {
            return g_MaterialRigisterDAL.DeleteData(sql);
        }
        /// <summary>
        /// 添加有证物质
        /// </summary>
        /// <param name="sqlBillItem"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool AddItem(string sqlBillItem, out string msg)
        {
            return g_MaterialRigisterDAL.AddItem(sqlBillItem, out msg);
        }

        /// <summary>
        /// 显示数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetList(string strWhere)
        {
            return g_MaterialRigisterDAL.GetList(strWhere);
        }
        /// <summary>
        /// 显示数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetLists(string strWhere)
        {
            return g_MaterialRigisterDAL.GetLists(strWhere);
        }
        /// <summary>
        /// 显示数据
        /// </summary>
        /// <returns></returns>
        /// <returns>获取符合条件的标液类型数据</returns>
        public DataTable GetListStyle(string strWhere)
        {
            return g_MaterialRigisterDAL.GetListStyle(strWhere);
        }

        /// <summary>
        /// 根据标液号带出数据
        /// </summary>
        /// <param name="ProductionSN"></param>
        /// <returns></returns>
        public DataTable GetData(string ProductionSN)
        {
            return g_MaterialRigisterDAL.GetData(ProductionSN);
        }

        //根据系统编号查出数据，主要获取打印专递的参数
        public DataTable GetNum(string sysNum)
        {
            return g_MaterialRigisterDAL.GetNum(sysNum);
        }

        /// <summary>
        /// 根据RowGuid获取系统编号
        /// </summary>
        /// <param name="rowID"></param>
        /// <returns></returns>
        public DataTable GetSysNum(string rowID)
        {
            return g_MaterialRigisterDAL.GetSysNum(rowID);
        }
    }
}
