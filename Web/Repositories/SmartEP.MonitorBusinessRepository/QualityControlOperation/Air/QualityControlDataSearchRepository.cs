using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.Data.SqlServer.MonitoringBusiness.Air;
using SmartEP.DomainModel.MonitoringBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.MonitoringBusinessRepository.QualityControlOperation.Air
{
    public class QualityControlDataSearchRepository : BaseGenericRepository<MonitoringBusinessModel, QC_Report_PMSharp5030FlowCheckCaliEntity>
    {
        public override bool IsExist(string strKey)
        {
            return true;
        }
        /// <summary>
        /// 数据处理DAL类
        /// </summary>
        QualityControlDataSearchDAL m_DataSearchDAL = Singleton<QualityControlDataSearchDAL>.GetInstance();

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="PointId">测点</param>
        /// <param name="dtBegin">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <returns></returns>
        public IQueryable<QC_Report_PMSharp5030FlowCheckCaliEntity> GetPages(string[] AnaDevSN, DateTime dtBegin, DateTime dtEnd)
        {
            return Retrieve(p => AnaDevSN.Contains(p.AnaDevSN.ToString()) && p.CreatDateTime >= dtBegin && p.CreatDateTime <= dtEnd);
        }
        /// <summary>
        /// 根据主键DutyID获取数据
        /// </summary>
        /// <param name="PointId">测点</param>
        /// <param name="dtBegin">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <returns></returns>
        public IQueryable<QC_Report_PMSharp5030FlowCheckCaliEntity> GetList(Guid guid)
        {
            return Retrieve(p => p.TaskGuid == guid);
        }
        /// <summary>
        /// 取得导出数据(行转列数据)
        /// </summary>
        /// <returns></returns>
        public DataView GetDataBasePager(string a)
        {
            return m_DataSearchDAL.GetDataBasePager();
        }
        /// <summary>
        /// 获取站点信息
        /// </summary>
        /// <param name="ObjectType">测点类型：1水2气</param>
        /// <returns></returns>
        public DataView GetDataPointGuid(string[] rowGuid)
        {
            return m_DataSearchDAL.GetDataPointGuid(rowGuid);
        }
        /// <summary>
        /// 获取站点信息
        /// </summary>
        /// <param name="ObjectType">测点类型：1水2气</param>
        /// <returns></returns>
        public DataView GetDataPointName(string[] PointNames)
        {
            return m_DataSearchDAL.GetDataPointName(PointNames);
        }
        /// <summary>
        /// 取得导出数据(行转列数据)
        /// </summary>
        /// <returns></returns>
        public DataView GetDataPager(string objectType)
        {
            return m_DataSearchDAL.GetDataPager(objectType);
        }
        /// <summary>
        /// 取得导出数据(行转列数据)
        /// </summary>
        /// <returns></returns>
        public DataView GetDataByOperations()
        {
            return m_DataSearchDAL.GetDataByOperations();
        }
        /// <summary>
        /// 取得导出数据(行转列数据)
        /// </summary>
        /// <returns></returns>
        public DataView GetDataPointGuidPager(string objectType, string pointGuid)
        {
            return m_DataSearchDAL.GetDataPointGuidPager(objectType, pointGuid);
        }
        /// <summary>
        /// 取得导出数据(行转列数据)
        /// </summary>
        /// <returns></returns>
        public DataView GetDataPointPager(string objectType, string[] rowGuid)
        {
            return m_DataSearchDAL.GetDataPointPager(objectType, rowGuid);
        }
        /// <summary>
        /// 仪器信息查询
        /// </summary>
        /// <param name="pointId">站点ID</param>
        /// <returns></returns>
        public DataView GetAllDataPager(string[] pointIds, string instrumentName, string[] SNType, string[] inState, string[] Operators, DateTime dtBegin, DateTime dtEnd)
        {
            return m_DataSearchDAL.GetAllDataPager(pointIds, instrumentName, SNType, inState, Operators, dtBegin, dtEnd);
        }
        /// <summary>
        /// 获取仪器信息
        /// </summary>
        /// <param name="ObjectType">测点类型：1水2气</param>
        /// <returns></returns>
        public DataView GetDataByObjectType(string ObjectType)
        {
            return m_DataSearchDAL.GetDataByObjectType(ObjectType);
        }
        /// <summary>
        /// 获取仪器实例信息
        /// </summary>
        /// <param name="ObjectType">测点类型：1水2气</param>
        /// <returns></returns>
        public DataView GetInstanceDataByObjectType(string ObjectType, string IsSpareParts)
        {
            return m_DataSearchDAL.GetInstanceDataByObjectType(ObjectType, IsSpareParts);
        }
        /// <summary>
        ///仪器状态信息
        /// </summary>
        /// <returns></returns>
        public DataView GetDataStatePager()
        {
            return m_DataSearchDAL.GetDataStatePager();
        }
        /// <summary>
        ///维护人信息
        /// </summary>
        /// <returns></returns>
        public DataView GetDataUserPager()
        {
            return m_DataSearchDAL.GetDataUserPager();
        }
        /// <summary>
        ///Sharp5030颗粒物检查校准记录表
        /// </summary>
        /// <returns></returns>
        public DataView GetPMSharpDataPager(string[] SN, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            return m_DataSearchDAL.GetPMSharpDataPager(SN, dtBegin, dtEnd, MissionName);
        }
        /// <summary>
        ///Thermo1400、1405颗粒物检查校准记录表
        /// </summary>
        /// <returns></returns>
        public DataView GetPMTeomSharpDataPager(string[] SN, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            return m_DataSearchDAL.GetPMTeomSharpDataPager(SN, dtBegin, dtEnd, MissionName);
        }
        /// <summary>
        ///标准流量计检定核查记录
        /// </summary>
        /// <returns></returns>
        public DataView GetStdFlowMeterDataPager(string[] SN, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            return m_DataSearchDAL.GetStdFlowMeterDataPager(SN, dtBegin, dtEnd, MissionName);
        }
        /// <summary>
        ///臭氧校准仪校准记录
        /// </summary>
        /// <returns></returns>
        public DataView GetO3HappenDevDataPager(string[] SN, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            return m_DataSearchDAL.GetO3HappenDevDataPager(SN, dtBegin, dtEnd, MissionName);
        }
        /// <summary>
        ///氮氧化物分析仪动态校准记录
        /// </summary>
        /// <returns></returns>
        public DataView GetNOxDevDataPager(string[] SN, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            return m_DataSearchDAL.GetNOxDevDataPager(SN, dtBegin, dtEnd, MissionName);
        }
        /// <summary>
        ///动态校准仪流量（标准气/稀释气）检查记录表
        /// </summary>
        /// <returns></returns>
        public DataView GetCaliDevDataPager(string[] SN, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            return m_DataSearchDAL.GetCaliDevDataPager(SN, dtBegin, dtEnd, MissionName);
        }
        /// <summary>
        ///零气纯度检查记录
        /// </summary>
        /// <returns></returns>
        public DataView GetZeroGasDataPager(string[] SN, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            return m_DataSearchDAL.GetZeroGasDataPager(SN, dtBegin, dtEnd, MissionName);
        }
        /// <summary>
        ///气体分析仪精密度检查记录
        /// </summary>
        /// <returns></returns>
        public DataView GetAnaDevPrecisionDataPager(string[] SN, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            return m_DataSearchDAL.GetAnaDevPrecisionDataPager(SN, dtBegin, dtEnd, MissionName);
        }
        /// <summary>
        ///气体分析仪零点、跨度检查与调节记录
        /// </summary>
        /// <returns></returns>
        public DataView GetZeroAndSpanDataPager(string[] SN, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            return m_DataSearchDAL.GetZeroAndSpanDataPager(SN, dtBegin, dtEnd, MissionName);
        }
        /// <summary>
        ///气体分析仪零漂、标漂检查记录
        /// </summary>
        /// <returns></returns>
        public DataView GetAnaDevDriftDataPager(string[] SN, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            return m_DataSearchDAL.GetAnaDevDriftDataPager(SN, dtBegin, dtEnd, MissionName);
        }
        /// <summary>
        ///气体分析仪准确度审核记录
        /// </summary>
        /// <returns></returns>
        public DataView GetAnaDevDataPager(string[] SN, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            return m_DataSearchDAL.GetAnaDevDataPager(SN, dtBegin, dtEnd, MissionName);
        }
        /// <summary>
        ///多点线性校准记录
        /// </summary>
        /// <returns></returns>
        public DataView GetMultiPointDataPager(string[] SN, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            return m_DataSearchDAL.GetMultiPointDataPager(SN, dtBegin, dtEnd, MissionName);
        }
        /// <summary>
        /// 获取区域信息
        /// </summary>
        /// <returns></returns>
        public DataView GetDataAreaPager()
        {
            return m_DataSearchDAL.GetDataAreaPager();
        }
        /// <summary>
        /// 获取站点信息
        /// </summary>
        /// <returns></returns>
        public DataView GetDataPointPager()
        {
            return m_DataSearchDAL.GetDataPointPager();
        }
        #region new
        /// <summary>
        ///Sharp5030颗粒物检查校准记录表
        /// </summary>
        /// <returns></returns>
        public DataView GetPMSharpDataNewPager(string[] SN, string[] pointIds, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            return m_DataSearchDAL.GetPMSharpDataNewPager(SN, pointIds, dtBegin, dtEnd, MissionName);
        }
        /// <summary>
        ///Thermo1400、1405颗粒物检查校准记录表
        /// </summary>
        /// <returns></returns>
        public DataView GetPMTeomSharpDataNewPager(string[] SN, string[] pointIds, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            return m_DataSearchDAL.GetPMTeomSharpDataNewPager(SN, pointIds, dtBegin, dtEnd, MissionName);
        }
        /// <summary>
        ///标准流量计检定核查记录
        /// </summary>
        /// <returns></returns>
        public DataView GetStdFlowMeterDataNewPager(string[] SN, string[] pointIds, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            return m_DataSearchDAL.GetStdFlowMeterDataNewPager(SN, pointIds, dtBegin, dtEnd, MissionName);
        }
        /// <summary>
        ///臭氧校准仪校准记录
        /// </summary>
        /// <returns></returns>
        public DataView GetO3HappenDevDataNewPager(string[] SN, string[] pointIds, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            return m_DataSearchDAL.GetO3HappenDevDataNewPager(SN, pointIds, dtBegin, dtEnd, MissionName);
        }
        /// <summary>
        ///氮氧化物分析仪动态校准记录
        /// </summary>
        /// <returns></returns>
        public DataView GetNOxDevDataNewPager(string[] SN, string[] pointIds, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            return m_DataSearchDAL.GetNOxDevDataNewPager(SN, pointIds, dtBegin, dtEnd, MissionName);
        }
        /// <summary>
        ///动态校准仪流量（标准气/稀释气）检查记录表
        /// </summary>
        /// <returns></returns>
        public DataView GetCaliDevDataNewPager(string[] SN, string[] pointIds, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            return m_DataSearchDAL.GetCaliDevDataNewPager(SN, pointIds, dtBegin, dtEnd, MissionName);
        }
        /// <summary>
        ///零气纯度检查记录
        /// </summary>
        /// <returns></returns>
        public DataView GetZeroGasDataNewPager(string[] SN, string[] pointIds, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            return m_DataSearchDAL.GetZeroGasDataNewPager(SN, pointIds, dtBegin, dtEnd, MissionName);
        }
        /// <summary>
        ///气体分析仪精密度检查记录
        /// </summary>
        /// <returns></returns>
        public DataView GetAnaDevPrecisionDataNewPager(string[] SN, string[] pointIds, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            return m_DataSearchDAL.GetAnaDevPrecisionDataNewPager(SN, pointIds, dtBegin, dtEnd, MissionName);
        }
        /// <summary>
        ///气体分析仪零点、跨度检查与调节记录
        /// </summary>
        /// <returns></returns>
        public DataView GetZeroAndSpanDataNewPager(string[] SN, string[] pointIds, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            return m_DataSearchDAL.GetZeroAndSpanDataNewPager(SN, pointIds, dtBegin, dtEnd, MissionName);
        }
        /// <summary>
        ///气体分析仪零漂、标漂检查记录
        /// </summary>
        /// <returns></returns>
        public DataView GetAnaDevDriftDataNewPager(string[] SN, string[] pointIds, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            return m_DataSearchDAL.GetAnaDevDriftDataNewPager(SN, pointIds, dtBegin, dtEnd, MissionName);
        }
        /// <summary>
        ///气体分析仪准确度审核记录
        /// </summary>
        /// <returns></returns>
        public DataView GetAnaDevDataNewPager(string[] SN, string[] pointIds, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            return m_DataSearchDAL.GetAnaDevDataNewPager(SN, pointIds, dtBegin, dtEnd, MissionName);
        }
        /// <summary>
        ///多点线性校准记录
        /// </summary>
        /// <returns></returns>
        public DataView GetMultiPointDataNewPager(string[] SN, string[] pointIds, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            return m_DataSearchDAL.GetMultiPointDataNewPager(SN, pointIds, dtBegin, dtEnd, MissionName);
        }
        #endregion
    }
}
