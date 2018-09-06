using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.MonitoringBusinessRepository.Air;
using SmartEP.Service.BaseData.MPInfo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SmartEP.Service.DataAnalyze.Air.DataQuery
{
    /// <summary>
    /// 名称：DataQueryByHourForUpLoadSZService.cs
    /// 创建人：王秉晟
    /// 创建日期：2016-01-27
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：上传审核数据到临时表
    /// 环境空气发布：
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class DataQueryByHourForUpLoadSZService
    {
        /// <summary>
        /// 接口（苏州）
        /// </summary>
        HourReportForUpLoadSZRepository g_HourReportForUpLoadSZRepository = Singleton<HourReportForUpLoadSZRepository>.GetInstance();

        /// <summary>
        /// 根据lambda表达式要求，返回所有对象
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IQueryable<AirHourReportForUpLoadSZEntity> RetrieveSZ(Expression<Func<AirHourReportForUpLoadSZEntity, bool>> predicate)
        {
            return g_HourReportForUpLoadSZRepository.Retrieve(predicate);
        }
        /// <summary>
        /// 获取因子浓度限值
        /// </summary>
        /// <param name="PollutantCodes"></param>
        /// <returns></returns>
        public DataTable GetFactorLimt(List<string> PollutantCodes)
        {
            return g_HourReportForUpLoadSZRepository.GetFactorLimt(PollutantCodes);
        }
        /// <summary>
        /// 导入区县数据记录
        /// </summary>
        /// <param name="ProblemData"></param>
        /// <param name="PointGuid"></param>
        /// <param name="fileName"></param>
        public void insertQuXianData(string ProblemData, string PointGuid, string fileName)
        {
            g_HourReportForUpLoadSZRepository.insertQuXianData(ProblemData, PointGuid, fileName);
        }
        /// <summary>
        /// 导入区县数据记录
        /// </summary>
        /// <param name="ProblemData"></param>
        /// <param name="PointGuid"></param>
        /// <param name="fileName"></param>
        public void insertQuXianDataNew(string ProblemData,DateTime dtStart, DateTime dtEnd)
        {
            g_HourReportForUpLoadSZRepository.insertQuXianDataNew(ProblemData,dtStart, dtEnd);
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
            return g_HourReportForUpLoadSZRepository.GetUpLoadDataSZ(PointIds, PollutantCodes, DataType, UserGuid);
        }

        /// <summary>
        /// 删除用户上传数据
        /// </summary>
        /// <param name="DataType">数据类型</param>
        /// <param name="UserGuid">上传用户Uid</param>
        public void DeleteByUserSZ(int DataType)
        {
            g_HourReportForUpLoadSZRepository.DeleteByUserSZ(DataType);
        }

        /// <summary>
        /// 批量导入
        /// </summary>
        /// <param name="models">导入的数据实体数组</param>
        public void InsertAllSZ(List<AirHourReportForUpLoadSZEntity> models)
        {
            g_HourReportForUpLoadSZRepository.InsertAllSZ(models);
        }

        /// <summary>
        /// 执行存储过程，批量上报区县数据
        /// </summary>
        /// <param name="UserGuid">操作用户标识</param>
        /// <param name="DataType">数据类型</param>
        /// <param name="ApplicationUid">系统类型</param>
        public void BatchAddAirHourReportSZ(string userGuid, int dataType, ApplicationType application)
        {
            string applicationUid = SmartEP.Core.Enums.EnumMapping.GetDesc(application);
            g_HourReportForUpLoadSZRepository.BatchAddAirHourReportSZ(userGuid, dataType, applicationUid);
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="DataType">数据类型</param>
        public DataView GetData(string pointId, string fileName)
        {
            return g_HourReportForUpLoadSZRepository.GetData(pointId, fileName);
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="DataType">数据类型</param>
        public DataView GetDataNew(DateTime dtStart, DateTime dtEnd)
        {
            return g_HourReportForUpLoadSZRepository.GetDataNew(dtStart, dtEnd);
        }
        /// <summary>
        /// 新增记录
        /// </summary>
        /// <param name="DataType">数据类型</param>
        public void GetAddData(string PointId, DateTime dtTime, string User, string fileName)
        {
            string PointName = "";
            if (PointId == "4296ce53-78d3-4741-9eda-6306e3e5b399")
                PointName = "张家港";
            if (PointId == "f7444783-a425-411c-a54b-f9fed72ec72e")
                PointName = "常熟";
            if (PointId == "d993d02f-fcc3-4ea6-b52b-9414fbd9b8e6")
                PointName = "太仓";
            if (PointId == "636775d8-091d-4754-9ed2-cd9dfef1f6ab")
                PointName = "昆山";
            if (PointId == "48d749e6-d07c-4764-8d50-50f170defe0b")
                PointName = "吴江";
            g_HourReportForUpLoadSZRepository.GetAddData(PointId, PointName, dtTime, User, fileName);
        }
        /// <summary>
        /// 新增记录
        /// </summary>
        /// <param name="DataType">数据类型</param>
        public void GetAddDataNew(string PointId, DateTime dtTime, string User, string fileName)
        {
            string PointName = "";
            if (PointId == "85C481F6-0D64-4A7A-AAE9-2D19637A4E1B")
                PointName = "文昌中学";
            if (PointId == "531A47F1-038E-4A96-BB12-1CC7207C2BC8")
                PointName = "昆山花桥";
            if (PointId == "5ED74863-F4B2-411E-89A3-4CDBC31AFDB4")
                PointName = "方洲公园";
            if (PointId == "A320AD97-443D-46FF-83A4-959FDF66EBFA")
                PointName = "东山";
            if (PointId == "47D8CC8C-8A0A-472D-B786-D3E401824E54")
                PointName = "拙政园";
            if (PointId == "f306ee6e-92c8-40e0-9484-4cf7cd4a0691")
                PointName = "常熟国土局子站";
            if (PointId == "df6d3480-18a0-475f-b9a3-9981b2445a2e")
                PointName = "凤凰站";
            if (PointId == "14278fc8-b823-4bf6-894c-c31bcdb72d6b")
                PointName = "香山";
            if (PointId == "62473106-c49a-45e7-bcaf-70bcc4eea094")
                PointName = "锦溪";
            if (PointId == "be43684c-36aa-4e31-a934-010d67a577fc")
                PointName = "湿地公园站";
            g_HourReportForUpLoadSZRepository.GetAddData(PointId, PointName, dtTime, User, fileName);
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="DataType">数据类型</param>
        public void GetUpdateData(string PointId, string fileName, string ProblemData)
        {
            g_HourReportForUpLoadSZRepository.GetUpdateData(PointId, fileName, ProblemData);
        }
    }
}
