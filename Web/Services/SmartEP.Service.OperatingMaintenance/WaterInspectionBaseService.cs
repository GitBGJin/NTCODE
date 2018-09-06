using SmartEP.Core.Generic;
using SmartEP.DomainModel.WaterQualityControlOperation;
using SmartEP.MonitoringBusinessRepository.QualityControlOperation.Water;
using SmartEP.Utilities.AdoData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SmartEP.Service.OperatingMaintenance
{
    /// <summary>
    /// 名称：WaterInspectionBaseService.cs
    /// 创建人：王秉晟
    /// 创建日期：2015-10-07
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 任务记录表服务
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class WaterInspectionBaseService
    {
        /// <summary>
        /// 任务记录表仓储层
        /// </summary>
        WaterInspectionBaseRepository r_waterInspectionBase = Singleton<WaterInspectionBaseRepository>.GetInstance();

        #region 任务记录表操作
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="waterInspectionBase">自动巡检基础实体</param>
        /// <returns>成功返回1，失败返回0，实体数组空返回2</returns>
        public int Add(WaterInspectionBaseEntity[] waterInspectionBase)
        {
            int num = 0;
            for (int i = 0; i < waterInspectionBase.Length; i++)
            {
                num += r_waterInspectionBase.Add(waterInspectionBase[i]);
            }
            //成功返回1，失败返回0
            if (num == waterInspectionBase.Length)
            {
                return waterInspectionBase.Length == 0 ? 2 : 1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="waterInspectionBase2">状态实体</param>
        /// <returns>成功返回true，失败返回false</returns>
        public bool Update(WaterInspectionBaseEntity waterInspectionBase)
        {
            return r_waterInspectionBase.Update(waterInspectionBase);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>成功返回true，失败返回false</returns>
        public bool Delete(int id)
        {
            return r_waterInspectionBase.Delete(id);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="taskCode"></param>
        /// <param name="missionID"></param>
        /// <param name="dtimeStart"></param>
        /// <param name="dtimeEnd"></param>
        /// <returns></returns>
        public DataTable GetList(string taskCode, string missionID = null, DateTime? dtimeStart = null, DateTime? dtimeEnd = null)
        {
            string strWhere = " 1=1 ";
            if (!string.IsNullOrWhiteSpace(taskCode))
            {
                strWhere += string.Format(" and TaskCode ='{0}' ", taskCode);
            }
            if (!string.IsNullOrWhiteSpace(missionID))
            {
                strWhere += string.Format(" and MissionID ='{0}' ", missionID);
            }
            if (dtimeStart != null)
            {
                strWhere += string.Format(" and ActionDate <='{0}' ", dtimeStart);
            }
            if (dtimeEnd != null)
            {
                strWhere += string.Format(" and FinishDate >='{0}' ", dtimeEnd);
            }
            if (strWhere == " 1=1 ")
            {
                strWhere += " and TaskCode='' ";
            }
            return r_waterInspectionBase.GetList(strWhere);
        }
        #endregion
    }
}
