using SmartEP.Core.Generic;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.MonitoringBusinessRepository.QualityControlOperation.Air;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.OperatingMaintenance.Air
{
    /// <summary>
    /// 名称：OM_TaskItemDataService.cs
    /// 创建人：樊垂贺
    /// 创建日期：2016-04-02
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 运维任务项数据记录表服务层
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class OM_TaskItemDataService
    {
        /// <summary>
        /// 异常表仓储层
        /// </summary>
        OM_TaskItemDataRepository r_OM_TaskItemDataRepository = Singleton<OM_TaskItemDataRepository>.GetInstance();

        /// <summary>
        /// 下位数据传入中心平台
        /// </summary>
        /// <param name="ds">数据源</param>
        /// <returns></returns>
        public void SubmitAirQCTaskData(DataSet ds)
        {
            r_OM_TaskItemDataRepository.SubmitAirQCTaskData(ds);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        /// <param name="strWhere">查询条件（例如：1=1）</param>
        /// <returns></returns>
        public DataTable GetList(string TaskCode, string TaskGuid)
        {
            string strWhere = "1=1";
            if (!string.IsNullOrWhiteSpace(TaskGuid))
            {
                strWhere += string.Format(" and TaskGuid='{0}' ", TaskGuid);
            }
            if (!string.IsNullOrWhiteSpace(TaskCode))
            {
                strWhere += string.Format(" and TaskCode='{0}' ", TaskCode);
            }
            return r_OM_TaskItemDataRepository.GetList(strWhere);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        /// <param name="strWhere">查询条件（例如：1=1）</param>
        /// <returns></returns>
        public DataTable GetListData(string TaskItemGuid, DateTime dtBegin, DateTime dtEnd)
        {
            string strWhere = "1=1";
            if (!string.IsNullOrWhiteSpace(TaskItemGuid))
            {
                strWhere += string.Format(" and TaskItemGuid in ({0}) ", TaskItemGuid);
            }
            if (dtBegin != null)
            {
                strWhere += string.Format(" and ItemRecordDate >='{0}' ", dtBegin);
            }
            if (dtEnd != null)
            {
                strWhere += string.Format(" and ItemRecordDate <='{0}' ", dtEnd);
            }
            return r_OM_TaskItemDataRepository.GetList(strWhere);
        }
        /// <summary>
        /// 获的仪器检修数据列表
        /// </summary>
        /// <param name="TaskItemGuid">查询条件（例如：1=1）</param>
        /// <param name="dtBegin">查询条件（例如：1=1）</param>
        /// <param name="dtEnd">查询条件（例如：1=1）</param>
        /// <param name="FixedAssetNumbers">查询条件（例如：1=1）</param>
        ///  <param name="istrue">判断是否通过仪器编号查询</param>
        /// <returns></returns>
        public DataTable GetListDatabyDevice(string TaskItemGuid, DateTime dtBegin, DateTime dtEnd, string FixedAssetNumbers, string[] rcbTypes, bool istrue)
        {
            string strWhere = "1=1";
            if (!string.IsNullOrWhiteSpace(TaskItemGuid))
            {
                strWhere += string.Format(" and TaskItemGuid in ({0}) ", TaskItemGuid);
            }
            if (istrue)
            {
                strWhere += string.Format(" and UniversalValue3 in ({0}) ", FixedAssetNumbers);
            }
            if (dtBegin != null)
            {
                strWhere += string.Format(" and ItemRecordDate >='{0}' ", dtBegin);
            }
            if (dtEnd != null)
            {
                strWhere += string.Format(" and ItemRecordDate <='{0}' ", dtEnd);
            }
            if (rcbTypes != null && rcbTypes.Length > 0)
            {
                string strtypes = "'" + string.Join("','", rcbTypes) + "'";
                strWhere += string.Format(" and ItemValue in ({0})  ", strtypes);
            }
            return r_OM_TaskItemDataRepository.GetList(strWhere);
        }
        /// <summary>
        /// 更新任务项值
        /// </summary>
        public void UpdateTable(OM_TaskItemDatumEntity[] OM_TaskItemDatumEntityArry, int taskGuid, string taskCode)
        {
            r_OM_TaskItemDataRepository.UpdateTable(OM_TaskItemDatumEntityArry, taskGuid, taskCode);
        }
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="strWhere">查询条件（例如：1=1）</param>
        /// <returns></returns>
        public void insertTable(OM_TaskItemDatumEntity[] OM_TaskItemDatumEntityArry, int taskGuid, string taskCode)
        {
            r_OM_TaskItemDataRepository.insertTable(OM_TaskItemDatumEntityArry, taskGuid, taskCode);
        }
    }
}
