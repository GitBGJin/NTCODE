using SmartEP.Core.Generic;
using SmartEP.DomainModel.WaterQualityControlOperation;
using SmartEP.MonitoringBusinessRepository.QualityControlOperation.Water;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.OperatingMaintenance.Water
{
    /// <summary>
    /// 名称：StatusService.cs
    /// 创建人：窦曙健
    /// 创建日期：2015-10-10
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 状态表服务层类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class StatusService
    {
        /// <summary>
        /// 状态表仓储层
        /// </summary>
        StatusRepository r_status = Singleton<StatusRepository>.GetInstance();

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="status">状态实体</param>
        /// <returns>成功返回1，失败返回0，实体数组空返回2</returns>
        public int Add(StatusEntity[] status)
        {
            int num = 0;
            for (int i = 0; i < status.Length; i++)
            {
                num += r_status.Add(status[i]);
            }
            //成功返回1，失败返回0
            if (num == status.Length)
            {
                return status.Length == 0 ? 2 : 1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="status">状态实体</param>
        /// <returns>成功返回true，失败返回false</returns>
        public bool Update(StatusEntity status)
        {
            return r_status.Update(status);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>成功返回true，失败返回false</returns>
        public bool Delete(int id)
        {
            return r_status.Delete(id);
        }

        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="pointId">站点编号</param>
        /// <param name="pollingDate">巡检时间</param>
        /// <returns>获取符合条件的数据</returns>
        public DataTable GetList(string taskCode, string missionID = null, DateTime? dtimeStart = null, DateTime? dtimeEnd = null)
        {
            //string strWhere = "PointId='" + pointId + "' and PollingDate>='" + datetime + "' and PollingDate<='" + endtime + "'";
            //if (MissionID != "" && taskCode != "")
            //{
            //    strWhere += " and MissionID='" + MissionID + "' and TaskCode='" + taskCode + "'";
            //}
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
                strWhere += string.Format(" and PollingDate <='{0}' ", dtimeStart);
            }
            if (dtimeEnd != null)
            {
                strWhere += string.Format(" and PollingDate >='{0}' ", dtimeEnd);
            }
            if (strWhere == " 1=1 ")
            {
                strWhere += " and TaskCode='' ";
            }
            return r_status.GetList(strWhere);
        }
    }
}
