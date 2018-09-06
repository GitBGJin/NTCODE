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
    public class RealSamplesCheckService : BaseGenericRepository<MonitoringBusinessModel, RealSampleEntity>
    {
        public override bool IsExist(string strKey)
        {
            return true;
        }

        /// <summary>
        /// 根据时间查询数据(标液核查情况)
        /// </summary>
        /// <param name="dtBegin">时间</param>
        /// <returns></returns>
        public DataTable ComparativeAnalysisRetrieve(DateTime dtBegin, string[] FactorCodes)
        {
            return ConvertToDataTable(Retrieve(it => it.Datetime == dtBegin &&it.PointId=="51"&& FactorCodes.Contains(it.CompareItem)));
        }

        /// <summary>
        /// 把IQueryable转化为DataTable
        /// </summary>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public DataTable ConvertToDataTable(IEnumerable enumerable)
        {
            var dataTable = new DataTable();
            foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(typeof(RealSampleEntity)))
            {
                dataTable.Columns.Add(pd.Name);
            }
            foreach (RealSampleEntity item in enumerable)
            {
                var Row = dataTable.NewRow();

                foreach (PropertyDescriptor dp in TypeDescriptor.GetProperties(typeof(RealSampleEntity)))
                {
                    Row[dp.Name] = dp.GetValue(item);
                }
                dataTable.Rows.Add(Row);
            }
            return dataTable;
        }

    }
}
