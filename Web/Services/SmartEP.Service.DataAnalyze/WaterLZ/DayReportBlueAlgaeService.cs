using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartEP.MonitoringBusinessRepository.WaterLZ;
using SmartEP.Core.Generic;
using System.Data;
using SmartEP.Utilities.DataTypes.ExtensionMethods;

namespace SmartEP.Service.DataAnalyze.WaterLZ
{
    /// <summary>
    /// 名称：DayReportBlueAlgaeService.cs
    /// 创建人：吕云
    /// 创建日期：2016-7-17
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 蓝藻预警发布：日数据处理
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class DayReportBlueAlgaeService
    {
        DayReportBlueAlgaeRepository m_DayReportBlueAlgaeRepository = Singleton<DayReportBlueAlgaeRepository>.GetInstance();
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="portIds">测点数据</param>
        /// <param name="factors">因子数据</param>
        /// <param name="dateStart">开始时间</param>
        /// <param name="dateEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId,DateTime）</param>
        /// <returns></returns>
        public DataView GetDataPager(string[] portIds, string[] factors
            , DateTime dateStart, DateTime dateEnd, int pageSize, int pageNo, out int recordTotal)
        {
            DataTable dt = new DataTable();

            dt= m_DayReportBlueAlgaeRepository.GetDataPager(portIds, factors, dateStart, dateEnd, pageSize, pageNo, out recordTotal).Table;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                foreach (string factor in factors)
                {
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

    }
}
