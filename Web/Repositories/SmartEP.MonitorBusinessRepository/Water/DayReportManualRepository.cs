using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.Data.SqlServer.MonitoringBusiness.Water;
using SmartEP.DomainModel.MonitoringBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.MonitoringBusinessRepository.Water
{
    /// <summary>
    /// 名称：DayReportManualRepository.cs
    /// 创建人：朱佳伟
    /// 创建日期：2015-12-23
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：人工点数据采集
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class DayReportManualRepository : BaseGenericRepository<MonitoringBusinessModel, DayReportManualEntity>
    {
        /// <summary>
        /// 数据接口
        /// </summary>
        DayReportManualDAL g_DayReportManualDAL = Singleton<DayReportManualDAL>.GetInstance();

        public override bool IsExist(string strKey)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 生成等级数据
        /// </summary>
        /// <param name="CalEQITypeUid">评价类型（湖泊、河流）</param>
        /// 河流：d8197909-568e-4319-874c-3ad7cbc92a7e
        /// 湖库：e82cd86f-71ba-4f87-8e5c-6ac7ca055a6b
        /// <param name="IEQI">评价水质类别</param>
        /// <param name="PointUids">站点列表，以“,”分割</param>
        /// <param name="PollutantCodes">评价因子,以“,”分割</param>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool GenerateData(string CalEQITypeUid, int IEQI, string PointUids, string PollutantCodes, DateTime dateStart, DateTime dateEnd, out String errMsg)
        {
            return g_DayReportManualDAL.GenerateData(CalEQITypeUid, IEQI, PointUids, PollutantCodes, dateStart, dateEnd, out errMsg);
        }

        /// <summary>
        /// 获取测点相关基础信息及相关因子日数据及等级
        /// </summary>
        /// <param name="PointUids">测点Uid数组</param>
        /// <param name="PollutantCodes">因子Code数组</param>
        /// <param name="StartDate">监测开始日期</param>
        /// <param name="EndDate">监测截止日期</param>
        /// <returns>DataTable</returns>
        public DataTable GetWaterIEQI(List<string> PointUids, List<string> PollutantCodes, DateTime StartDate, DateTime EndDate)
        {
            return g_DayReportManualDAL.GetWaterIEQI(PointUids, PollutantCodes, StartDate, EndDate);
        }
    }
}
