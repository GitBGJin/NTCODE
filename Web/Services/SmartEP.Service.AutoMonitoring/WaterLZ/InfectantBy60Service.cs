using SmartEP.AMSRepository.WaterLZ;
using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.WaterAutoMonitoring;
using SmartEP.Service.AutoMonitoring.Interfaces;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.AutoMonitoring.WaterLZ
{
    public class InfectantBy60Service
    {
        /// <summary>
        /// 60分钟数据仓储层
        /// </summary>
        InfectantBy60Repository g_InfectantBy60Repository = Singleton<InfectantBy60Repository>.GetInstance();

        #region << ADO.NET >>
        /// <summary>
        /// 取得蓝藻虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns></returns>
        public DataView GetLZDataPager(string[] portIds, IList<IPollutant> factors
            , DateTime dtStart, DateTime dtEnd, int pageSize, int pageNo, out int recordTotal)
        {
            recordTotal = 0;
            DataTable dt = new DataTable();
            if (factors.IsNotNullOrDBNull())
                dt= g_InfectantBy60Repository.GetLZDataPager(portIds, factors.Select(p => p.PollutantCode).ToArray(), dtStart, dtEnd, pageSize, pageNo, out recordTotal).ToTable();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                foreach (string factor in factors.Select(p => p.PollutantCode).ToArray())
                {
                    if (factor == "w19011")
                    {
                        if (dt.Rows[i][factor].IsNotNullOrDBNull() && (dt.Rows[i][factor].ToString() == "7999" || dt.Rows[i][factor].ToString() == "0" || Convert.ToDecimal(dt.Rows[i][factor].ToString()) < 30))
                        {
                            dt.Rows[i][factor] = 30;
                        }
                    }
                    else
                    {
                        if (dt.Rows[i][factor].IsNotNullOrDBNull() && (dt.Rows[i][factor].ToString() == "7999" || dt.Rows[i][factor].ToString() == "0" || Convert.ToDecimal(dt.Rows[i][factor].ToString()) < 0))
                        {
                            dt.Rows[i][factor] = DBNull.Value;
                        }
                    }
                    if (dt.Rows[i][factor] != DBNull.Value)
                    {
                        if (factor == "w19011")//藻密度
                        {
                            dt.Rows[i][factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i][factor]), 0);
                        }
                        else if (factor == "w01010" || factor == "w01003" || factor == "w01014")//水温&浊度&电导率
                        {
                            dt.Rows[i][factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i][factor]), 1);
                        }
                        else if (factor == "w01009" || factor == "w01001")//溶解氧&PH值
                        {
                            dt.Rows[i][factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i][factor]), 2);
                        }
                        else if (factor == "w01016")//叶绿素a
                        {
                            dt.Rows[i][factor] = DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[i][factor]), 4);
                        }
                    }
                }
            }

                return dt.DefaultView;
        }
        /// <summary>
        /// 取得统计数据（最大值、最小值、平均值）
        /// </summary>
        /// <param name="portIds">测点ID数组</param>
        /// <param name="factors">因子数组</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns></returns>
        public DataView GetStatisticalLZData(string[] portIds, IList<IPollutant> factors, DateTime dateStart, DateTime dateEnd)
        {
            DataTable dt = new DataTable();
            if (factors.IsNotNullOrDBNull())
                dt= g_InfectantBy60Repository.GetStatisticalLZData(portIds, factors.Select(p => p.PollutantCode).ToArray(), dateStart, dateEnd).ToTable();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 1; j < dt.Columns.Count; j++)
                {
                    if (dt.Rows[i][0].ToString() == "w19011")
                    {
                        if (dt.Rows[i][j].IsNotNullOrDBNull() && Convert.ToDecimal(dt.Rows[i][j].ToString()) < 30)
                        {
                            dt.Rows[i][j] = 30;
                        }
                    }
                    
                }
            }


            return dt.DefaultView;
        }

        #endregion
    }
}
