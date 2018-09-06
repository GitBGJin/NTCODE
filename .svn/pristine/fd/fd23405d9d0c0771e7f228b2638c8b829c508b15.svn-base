using SmartEP.Core.Generic;
using SmartEP.Data.SqlServer.QualityControlOperation.Air;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.MonitoringBusinessRepository.QualityControlOperation.Air
{
    public class MaintenanceRepository
    {
        MaintenanceDAL d_MaintenanceDAL = Singleton<MaintenanceDAL>.GetInstance();
        #region << 数据查询方法 >>
        /// <summary>
        /// 根据站点guid查询仪器
        /// </summary>
        /// <param name="pointGuids">站点guid</param>
        /// <param name="objectType">系统类型</param>
        /// <param name="IsSpareParts">是否备件</param>
        /// <returns></returns>
        public DataView GetInstanceByGuid(string[] pointGuids, string objectType, string IsSpareParts)
        {
            return d_MaintenanceDAL.GetInstanceByGuid(pointGuids, objectType, IsSpareParts);
        }
        /// <summary>
        /// 查询可用配件信息
        /// </summary>
        /// <param name="FittingName">配件名</param>
        /// <param name="ObjectType">系统类型</param>
        /// <returns></returns>
        public DataView GetFittingInstance(string FittingName, string ObjectType)
        {
            return d_MaintenanceDAL.GetFittingInstance(FittingName, ObjectType);
        }
        /// <summary>
        /// 查询仪器的配件具体信息
        /// </summary>
        /// <param name="InstrumentInstanceGuid">具体仪器实例GUID</param>
        /// <returns></returns>
        public DataView GetInstrumentFittingInstance(string InstrumentInstanceGuid, string FittingName)
        {
            return d_MaintenanceDAL.GetInstrumentFittingInstance(InstrumentInstanceGuid, FittingName);

        }

        /// <summary>
        /// 查询仪器的配件
        /// </summary>
        /// <param name="rowguid">仪器GUID</param>
        /// <returns></returns>
        public DataView GetFitting(string rowguid)
        {
            return d_MaintenanceDAL.GetFitting(rowguid);
        }
        /// <summary>
        /// 查询仪器清单
        /// </summary>
        /// <param name="FixedAssetNumbers">仪器编号</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetInstanceList(string[] FixedAssetNumbers, DateTime dtStart, DateTime dtEnd)
        {
            return d_MaintenanceDAL.GetInstanceList(FixedAssetNumbers, dtStart, dtEnd);
        }
        /// <summary>
        /// 获取仪器所在测点
        /// </summary>
        /// <param name="InstrumentInstanceGuid">仪器实例Guid</param>
        /// <returns></returns>
        public string GetInstanceSite(string InstrumentInstanceGuid)
        {
            return d_MaintenanceDAL.GetInstanceSite(InstrumentInstanceGuid);
        }
        /// <summary>
        /// 向出入库中添加仪器/备件的出入库信息
        /// </summary>
        /// <param name="status">出入库状态</param>
        /// <param name="note">提示内容</param>
        /// <param name="InstanceGuid">仪器/配件实例Guid</param>
        /// <returns></returns>
        public int InsertInstrumentInstanceSite(int status, string note, string InstanceGuid)
        {
            return d_MaintenanceDAL.InsertInstrumentInstanceSite(status, note, InstanceGuid);
        }
        /// <summary>
        /// 查看仪器是否已选择当前配件类型
        /// </summary>
        /// <param name="InstrumentInstanceGuid">仪器实例Guid</param>
        ///<param name="FittingGuid">配件类型Guid</param>
        /// <returns></returns>
        public string GetIsFitting(string InstrumentInstanceGuid, string FittingGuid)
        {
            return d_MaintenanceDAL.GetIsFitting(InstrumentInstanceGuid, FittingGuid);
        }
        /// <summary>
        /// 查看仪器已选择当前配件类型的配对RowGuid
        /// </summary>
        /// <param name="InstrumentInstanceGuid">仪器实例Guid</param>
        ///<param name="FittingGuid">配件类型Guid</param>
        /// <returns></returns>
        public string GetIsFittingKey(string InstrumentInstanceGuid, string FittingGuid)
        {
            return d_MaintenanceDAL.GetIsFittingKey(InstrumentInstanceGuid, FittingGuid);
        }
        /// <summary>
        /// 查询仪器出入记录
        /// </summary>
        /// <param name="FixedAssetNumbers">仪器编号</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetInstanceSite(string[] FixedAssetNumbers, DateTime dtStart, DateTime dtEnd)
        {
            return d_MaintenanceDAL.GetInstanceSite(FixedAssetNumbers, dtStart, dtEnd);
        }
        /// <summary>
        /// 查询仪器出入记录
        /// </summary>
        /// <param name="pointGuids">站点guid</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetInstanceSiteByPoint(string[] pointGuids, DateTime dtStart, DateTime dtEnd)
        {
            return d_MaintenanceDAL.GetInstanceSiteByPoint(pointGuids, dtStart, dtEnd);
        }
        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public DataTable GetList(string strWhere)
        {
            return d_MaintenanceDAL.GetList(strWhere);
        }
        /// <summary>
        /// 仪器使用信息
        /// </summary>
        /// <param name="FixedAssetNumbers">仪器编号</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <returns></returns>
        public DataTable GetListByInstrument(string[] FixedAssetNumbers, DateTime dtStart, DateTime dtEnd)
        {
            return d_MaintenanceDAL.GetListByInstrument(FixedAssetNumbers, dtStart, dtEnd);
        }
        #endregion
    }
}
