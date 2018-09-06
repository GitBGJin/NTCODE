using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.MonitoringBusiness;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.OperatingMaintenance.Water
{
    /// <summary>
    /// 名称：StandardSolutionCheckService.cs
    /// 创建人：尤红兵
    /// 创建日期：2015-10-23
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：质控月报表类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class StandardSolutionService : BaseGenericRepository<MonitoringBusinessModel, StandardSolutionCheckEntity>
    {
        public override bool IsExist(string strKey)
        {
            return true;
        }
        /// <summary>
        /// 根据时间查询数据(加标)
        /// </summary>
        /// <param name="dtBegin">时间</param>
        /// <param name="FactorCodes">因子的Codes</param>
        /// <returns></returns>
        public DataTable StandardAddRetrieve(DateTime dtBegin, string[] FactorCodes)
        {
            return ConvertToDataTable(Retrieve(it => it.Tstamp == dtBegin && it.PointId == 51 && FactorCodes.Contains(it.PollutantCode)));
        }

        /// <summary>
        /// 根据时间查询数据(标样考核数据)
        /// </summary>
        /// <param name="dtBegin">时间</param>
        /// <param name="FactorCodes">因子的Codes</param>
        /// <returns></returns>
        public DataTable SampleEvaluationRetrieve(DateTime dtBegin,string[] FactorCodes)
        {
            return ConvertToDataTable(Retrieve(it => it.Tstamp == dtBegin && it.PointId == 51 && FactorCodes.Contains(it.PollutantCode)));
        }

        /// <summary>
        /// 根据时间查询数据(标液核查情况)
        /// </summary>
        /// <param name="dtBegin">时间</param>
        /// <param name="FactorCodes">因子的Codes</param>
        /// <returns></returns>
        public DataTable CalibrationVerificationRetrieve(DateTime dtBegin, string[] FactorCodes)
        {
            return ConvertToDataTable(Retrieve(it => it.Tstamp == dtBegin && it.PointId == 51 && FactorCodes.Contains(it.PollutantCode)));
        }

        /// <summary>
        /// 根据时间查询数据(标液核查情况)
        /// </summary>
        /// <param name="dtBegin">时间</param>
        /// <param name="FactorCodes">因子的Codes</param>
        /// <returns></returns>
        public DataTable ComparativeAnalysisRetrieve(DateTime dtBegin, string[] FactorCodes)
        {
            return ConvertToDataTable(Retrieve(it => it.CreatDateTime == dtBegin && it.PointId == 51 && FactorCodes.Contains(it.PollutantCode)));
        }

        /// <summary>
        /// 根据页面missionID、actionID获取数据
        /// </summary>
        /// <param name="missionID">任务ID</param>
        /// <param name="actionID">工作ID</param>
        /// <returns></returns>
        public DataTable StandardSolutionCheckRetrieve(string missionID, string actionID)
        {
            return ConvertToDataTable( Retrieve(it => it.MissionID==missionID&&it.ActionID==actionID));
        }

        /// <summary>
        /// 把IQueryable转化为DataTable
        /// </summary>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public DataTable ConvertToDataTable(IEnumerable enumerable)
        {
            var dataTable = new DataTable();
            foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(typeof(StandardSolutionCheckEntity)))
            {
                dataTable.Columns.Add(pd.Name);
            }
            foreach (StandardSolutionCheckEntity item in enumerable)
            {
                var Row = dataTable.NewRow();

                foreach (PropertyDescriptor dp in TypeDescriptor.GetProperties(typeof(StandardSolutionCheckEntity)))
                {
                    Row[dp.Name] = dp.GetValue(item);
                }
                dataTable.Rows.Add(Row);
            }
            return dataTable;
        }

    }
}
