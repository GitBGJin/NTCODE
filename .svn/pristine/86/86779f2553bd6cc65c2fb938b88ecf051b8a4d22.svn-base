using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.Data.SqlServer.MonitoringBusiness.Air;
using SmartEP.DomainModel.MonitoringBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.MonitoringBusinessRepository.Air
{
    /// <summary>
    /// 名称：HourReportForUpLoadSZRepository.cs
    /// 创建人：王秉晟
    /// 创建日期：2016-01-27
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：上传审核数据到临时表
    /// 环境空气发布：
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class HourReportForUpLoadSZRepository : BaseGenericRepository<MonitoringBusinessModel, AirHourReportForUpLoadSZEntity>
    {
        /// <summary>
        /// 数据处理接口
        /// </summary>
        HourReportForUpLoadSZDAL g_HourReportForUpLoadSZDAL = Singleton<HourReportForUpLoadSZDAL>.GetInstance();

        public override bool IsExist(string strKey)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 获取因子浓度限值
        /// </summary>
        /// <param name="PollutantCodes"></param>
        /// <returns></returns>
        public DataTable GetFactorLimt(List<string> PollutantCodes)
        {
            return g_HourReportForUpLoadSZDAL.GetFactorLimt(PollutantCodes);
        }
        /// <summary>
        /// 导入区县数据记录
        /// </summary>
        /// <param name="ProblemData"></param>
        /// <param name="PointGuid"></param>
        /// <param name="fileName"></param>
        public void insertQuXianData(string ProblemData, string PointGuid, string fileName)
        {
            g_HourReportForUpLoadSZDAL.insertQuXianData(ProblemData, PointGuid, fileName);
        }
        /// <summary>
        /// 导入区县数据记录
        /// </summary>
        /// <param name="ProblemData"></param>
        /// <param name="PointGuid"></param>
        /// <param name="fileName"></param>
        public void insertQuXianDataNew(string ProblemData,DateTime dtStart, DateTime dtEnd)
        {
            g_HourReportForUpLoadSZDAL.insertQuXianDataNew(ProblemData,dtStart, dtEnd);
        }
        /// <summary>
        /// 获取用户上传的数据
        /// </summary>
        /// <param name="PointIds">监测点Id数组</param>
        /// <param name="PollutantCodes">监测因子Code数组</param>
        /// <param name="DataType">数据类型</param>
        /// <param name="UserGuid">上传用户Uid</param>
        /// <returns>DataTable</returns>
        public DataTable GetUpLoadDataSZ(List<int> PointIds, List<string> PollutantCodes, int DataType, string UserGuid)
        {
            return g_HourReportForUpLoadSZDAL.GetUpLoadDataSZ(PointIds, PollutantCodes, DataType, UserGuid);
        }

        /// <summary>
        /// 删除用户上传数据
        /// </summary>
        /// <param name="DataType">数据类型</param>
        /// <param name="UserGuid">上传用户Uid</param>
        public void DeleteByUserSZ(int DataType)
        {
            g_HourReportForUpLoadSZDAL.DeleteByUserSZ(DataType);
        }

        /// <summary>
        /// 批量导入
        /// </summary>
        /// <param name="models">导入的数据实体数组</param>
        public void InsertAllSZ(List<AirHourReportForUpLoadSZEntity> models)
        {
            g_HourReportForUpLoadSZDAL.InsertAllSZ(models);
        }

        /// <summary>
        /// 执行存储过程，批量上报区县数据
        /// </summary>
        /// <param name="UserGuid">操作用户标识</param>
        /// <param name="DataType">数据类型</param>
        /// <param name="ApplicationUid">系统Uid</param>
        public void BatchAddAirHourReportSZ(string UserGuid, int DataType, string ApplicationUid)
        {
            g_HourReportForUpLoadSZDAL.BatchAddAirHourReportSZ(UserGuid, DataType, ApplicationUid);
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="DataType">数据类型</param>
        public DataView GetData(string pointId, string fileName)
        {
            return g_HourReportForUpLoadSZDAL.GetData(pointId, fileName);
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="DataType">数据类型</param>
        public DataView GetDataNew(DateTime dtStart, DateTime dtEnd)
        {
            return g_HourReportForUpLoadSZDAL.GetDataNew(dtStart, dtEnd);
        }
        /// <summary>
        /// 新增记录
        /// </summary>
        /// <param name="DataType">数据类型</param>
        public void GetAddData(string PointId, string PointName, DateTime dtTime, string User, string fileName)
        {
            g_HourReportForUpLoadSZDAL.GetAddData(PointId, PointName, dtTime, User, fileName);
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="DataType">数据类型</param>
        public void GetUpdateData(string PointId, string fileName, string ProblemData)
        {
            g_HourReportForUpLoadSZDAL.GetUpdateData(PointId, fileName, ProblemData);
        }
    }
}
