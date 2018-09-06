using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.Data.SqlServer.QualityControlOperation.Water;
using SmartEP.DomainModel.MonitoringBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.MonitoringBusinessRepository.QualityControlOperation.Water
{
    /// <summary>
    /// 名称：TaskConfigRepository.cs
    /// 创建人：窦曙健
    /// 创建日期：2015-10-13
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 地表水发布：任务配置仓储层
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class TaskConfigRepository : BaseGenericRepository<MonitoringBusinessModel, TaskConfigEntity>
    {
        /// <summary>
        /// 根据key主键判断记录是否存在
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public override bool IsExist(string strKey)
        {
            return RetrieveCount(x => x.Id.Equals(Convert.ToInt32(strKey))) == 0 ? false : true;
        }

        #region << ADO.NET >>
        /// <summary>
        /// 任务配置DAL
        /// </summary>
        TaskConfigDAL d_TaskConfigDAL = Singleton<TaskConfigDAL>.GetInstance();

        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="pointId">站点编号</param>
        /// <param name="pollingDate">巡检时间</param>
        /// <returns></returns>
        public DataView GetDataPager(string[] taskIDs, string type, int pageSize, int pageNo, out int recordTotal, string orderBy = "id")
        {
            recordTotal = 0;
            return d_TaskConfigDAL.GetDataPager(taskIDs, type, pageSize, pageNo, out recordTotal, orderBy);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataTable GetList(string strWhere)
        {
            return d_TaskConfigDAL.GetList(strWhere);
        }
        /// <summary>
        /// 获得MissionName
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public string[] GetName(string strWhere)
        {
            //return Retrieve(p => p.MissionID == strWhere);
            IQueryable<TaskConfigEntity> q = Retrieve(p => p.MissionID == strWhere);
            IQueryable<string> x = q.Select(p => p.MissionName);
            string[] name = x.ToArray();
            return name;
        }
        #endregion
    }
}
