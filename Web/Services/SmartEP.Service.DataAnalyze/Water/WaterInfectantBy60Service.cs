using SmartEP.Core.Interfaces;
using SmartEP.Data.SqlServer.AutoMonitoring;
using SmartEP.DomainModel.WaterAutoMonitoring;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.DataAnalyze.Water
{
    public class WaterInfectantBy60Service : BaseGenericRepository<WaterAutoMonitoringModel, InfectantBy60Entity>
    {
        public override bool IsExist(string strKey)
        {
            return true;
        }

        /// <summary>
        /// 获取最接近当前时间的因子数据
        /// </summary>
        /// <param name="pointID">测点</param>
        /// <param name="factors">因子组</param>
        /// <returns></returns>
        public DataTable GetInfectantBy60DataPager(string[] portIds, string[] factors)
        {
            Infectant60WaterDAL infectant60Water = new Infectant60WaterDAL();
            infectant60Water.GetWaterRecentTimeData(portIds, factors);

            DataTable dtDataOnlineState = ConvertToDataTable(Retrieve(it => portIds.Contains(it.PointId.ToString()) && factors.Contains(it.PollutantCode)));
            return new DataTable();
        }

        /// <summary>
        /// 把IQueryable转化为DataTable
        /// </summary>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public DataTable ConvertToDataTable(IEnumerable enumerable)
        {
            var dataTable = new DataTable();
            foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(typeof(InfectantBy60Entity)))
            {
                dataTable.Columns.Add(pd.Name);
            }
            foreach (InfectantBy60Entity item in enumerable)
            {
                var Row = dataTable.NewRow();

                foreach (PropertyDescriptor dp in TypeDescriptor.GetProperties(typeof(InfectantBy60Entity)))
                {
                    Row[dp.Name] = dp.GetValue(item);
                }
                dataTable.Rows.Add(Row);
            }
            return dataTable;
        }

    }
}
