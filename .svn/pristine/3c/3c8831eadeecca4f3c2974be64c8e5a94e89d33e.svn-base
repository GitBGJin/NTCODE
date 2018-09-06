using SmartEP.DomainModel.MonitoringBusiness;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.AutoMonitoring.Water
{
    public class OperationNewService
    {
        WaterDataEffectRateNewService waterDataEffectRateNew = new WaterDataEffectRateNewService();
        WaterDataSamplingRateNewService m_WaterDataSamplingRateNewService=new WaterDataSamplingRateNewService();
        /// <summary>
        /// 获取有效率查询的数据（中心平台）
        /// </summary>
        /// <param name="portIds">测点组</param>
        /// <param name="factors">因子组</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">结束时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="dvStatistical">返回底部统计</param>
        /// <param name="orderBy">排序方式（字段：PointId,Tstamp）</param>
        /// <returns></returns>
        public DataView GetPointEffectRateDetailNew(string[] portIds, string[] factors, DateTime dtmStart, DateTime dtmEnd,
            int pageSize, int pageNo, out int recordTotal, out DataView dvStatistical, string orderBy = "PointId,Tstamp")
        {
            DataTable dtDataEffectRate = waterDataEffectRateNew.GetdtDataEffectRate(portIds, factors, dtmStart, dtmEnd,orderBy);
            DataTable dtDataSamplingRate = m_WaterDataSamplingRateNewService.GetdtDataSamplingRate(portIds, factors, dtmStart, dtmEnd, orderBy);
            DataTable dtTemporaryTable = CreateSequentialByFactor(factors);//生成一个临时表 保存计算的捕获率数据
            DataTable dtTailTotalTable = CreateTailTotalSequential();//生成一个临时表 计算尾部合计数据
            //按站点和因子计算数据
            foreach (string point in portIds)
            {
                DataRow drTemporaryTable = dtTemporaryTable.NewRow();
                drTemporaryTable["PointId"] = point.ToString();
                decimal totalActualValue = 0;
                decimal totalSampleValue = 0;
                foreach (string factor in factors)
                {
                    DataRow[] drDataEffectRate = dtDataEffectRate.Select(string.Format("PointId='{0}' and PollutantCode='{1}'", point, factor));
                    DataRow[] drDataSamplingRate = dtDataSamplingRate.Select(string.Format("PointId='{0}' and PollutantCode='{1}'", point, factor));
                    if (drDataEffectRate != null && drDataEffectRate.Count() > 0 && drDataSamplingRate != null && drDataSamplingRate.Count() > 0)
                    {
                        decimal actualValue = 0;
                        decimal sampleValue = 0;
                        string percentageValue = string.Empty;
                        foreach (DataRow dr in drDataEffectRate)
                        {
                            if (dr["QualifiedNumber"] != DBNull.Value)
                            {
                                actualValue += Convert.ToDecimal(dr["QualifiedNumber"]);//合格数据条数
                            }
                        }
                        foreach (DataRow dr in drDataSamplingRate)
                        {
                            if (dr["SamplingNumber"] != DBNull.Value)
                            {
                                sampleValue += Convert.ToDecimal(dr["SamplingNumber"]);//应该测试值
                            }
                        }
                        
                        //总的应该测试值不能为0 因为是分母
                        if (sampleValue != 0)
                        {
                            if ((actualValue / sampleValue) * 100 >= 100)
                            {
                                percentageValue = "100";
                            }
                            else
                            {
                                //捕获率
                                percentageValue = string.Format("{0:N2}", (actualValue / sampleValue) * 100);
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(percentageValue))
                        {
                            drTemporaryTable[factor] = string.Format("{0}%<br/>{1}:{2}", percentageValue, actualValue, sampleValue);
                        }

                        totalActualValue += actualValue;//每一行总的实际测试值
                        totalSampleValue += sampleValue;//每一行总的应该测试值
                    }
                }
                if (totalSampleValue != 0)
                {
                    if ((totalActualValue / totalSampleValue) * 100 >= 100)
                    {
                        drTemporaryTable["TotalValue"] = string.Format("100%<br/>{0}:{1}", totalActualValue, totalSampleValue);
                    }
                    else
                    {
                        drTemporaryTable["TotalValue"] = string.Format("{0:N2}%<br/>{1}:{2}", (totalActualValue / totalSampleValue) * 100, totalActualValue, totalSampleValue);
                    }
                    
                }
                else
                {
                    drTemporaryTable["TotalValue"] = "0";
                }
                dtTemporaryTable.Rows.Add(drTemporaryTable);
            }

            //获取合计值
            decimal tailTotalActualValue = 0;//尾部总的实际测试值
            decimal tailTotalSampleValue = 0;//尾部总的应该测试值
            foreach (string factor in factors)
            {
                DataRow[] drDataEffectRate = dtDataEffectRate.Select(string.Format("PollutantCode='{0}'", factor));
                DataRow[] drDataSamplingRate = dtDataSamplingRate.Select(string.Format("PollutantCode='{0}'", factor));
                if (drDataEffectRate != null && drDataEffectRate.Count() > 0 && drDataSamplingRate != null && drDataSamplingRate.Count() > 0)
                {
                    DataRow drTailTotalTable = dtTailTotalTable.NewRow();

                    drTailTotalTable["PollutantCode"] = factor.ToString();

                    decimal actualValue = 0;
                    decimal sampleValue = 0;
                    string percentageValue = string.Empty;
                    foreach (DataRow dr in drDataEffectRate)
                    {
                        if (dr["QualifiedNumber"] != DBNull.Value)
                        {
                            actualValue += Convert.ToDecimal(dr["QualifiedNumber"]);//合格数据条数
                        }
                    }
                    foreach (DataRow dr in drDataSamplingRate)
                    {
                        if (dr["SamplingNumber"] != DBNull.Value)
                        {
                            sampleValue += Convert.ToDecimal(dr["SamplingNumber"]);//应该测试值（采样数据条数）
                        }
                    }
                    tailTotalActualValue += actualValue;
                    tailTotalSampleValue += sampleValue;
                    //总的应该测试值不能为0 因为是分母
                    if (sampleValue != 0)
                    {
                        if ((actualValue / sampleValue) * 100 >= 100)
                        {
                            percentageValue = "100";
                        }
                        else 
                        {
                            //捕获率
                            percentageValue = string.Format("{0:N2}", (actualValue / sampleValue) * 100);
                        }
                        
                    }
                    if (!string.IsNullOrWhiteSpace(percentageValue))
                    {

                        drTailTotalTable["PollutantTotal"] = string.Format("{0}%<br/>{1}:{2}", percentageValue, actualValue, sampleValue);
                    }
                    dtTailTotalTable.Rows.Add(drTailTotalTable);
                }
            }

            DataRow drTailTotal = dtTailTotalTable.NewRow();//最后添加一行总的统计行
            drTailTotal["PollutantCode"] = "TotalValue";//默认赋值
            if (tailTotalSampleValue != 0)
            {
                if ((tailTotalActualValue / tailTotalSampleValue) * 100>=100)
                {
                    drTailTotal["PollutantTotal"] = string.Format("100%<br/>{0}:{1}", tailTotalActualValue, tailTotalSampleValue);
                }
                else
                {
                    drTailTotal["PollutantTotal"] = string.Format("{0:N2}%<br/>{1}:{2}", (tailTotalActualValue / tailTotalSampleValue) * 100, tailTotalActualValue, tailTotalSampleValue);
                }
                
            }
            else
            {
                drTailTotal["PollutantTotal"] = "0";
            }
            dtTailTotalTable.Rows.Add(drTailTotal);

            dvStatistical = dtTailTotalTable.DefaultView;

            recordTotal = dtTemporaryTable.Rows.Count;

            DataTable dtData = GetDataPagerByPageSize(dtTemporaryTable, pageSize, pageNo, "PointId");

            return dtData.DefaultView;
        }
        /// <summary>
        /// 把IQueryable转化为DataTable
        /// </summary>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public DataTable ConvertToDataTable(IEnumerable enumerable)
        {
            var dataTable = new DataTable();
            foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(typeof(ReportQualifiedRateByDayEntity)))
            {
                dataTable.Columns.Add(pd.Name);
            }
            foreach (ReportQualifiedRateByDayEntity item in enumerable)
            {
                var Row = dataTable.NewRow();

                foreach (PropertyDescriptor dp in TypeDescriptor.GetProperties(typeof(ReportQualifiedRateByDayEntity)))
                {
                    Row[dp.Name] = dp.GetValue(item);
                }
                dataTable.Rows.Add(Row);
            }
            return dataTable;
        }

        /// <summary>
        /// 获取分页后的数据
        /// </summary>
        /// <param name="dtOld">原始表</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="orderBy">排序方式（如：PointId）</param>
        /// <returns></returns>
        private DataTable GetDataPagerByPageSize(DataTable dtOld, int pageSize, int pageNo, string orderBy)
        {
            if (dtOld != null)
            {
                int startIndex = pageSize * pageNo;//要返回的数据的开始位置(从0开始)
                int endIndex = pageSize * (pageNo + 1);//要返回的数据的结束位置(数据取到小于这个值)
                DataTable dtReturn = dtOld.Clone();
                endIndex = (endIndex > dtOld.Rows.Count) ? dtOld.Rows.Count : endIndex;//如果结束位置大于最大行数，则改为最大行数的值
                orderBy = (orderBy != null) ? orderBy : string.Empty;
                string[] orderByValues = orderBy.Split(',');
                bool isOrderByContains = true;
                foreach (string orderByValue in orderByValues)
                {
                    isOrderByContains = dtOld.Columns.Contains(orderByValue);
                }
                if (isOrderByContains)
                {
                    dtOld.DefaultView.Sort = orderBy;
                    dtOld = dtOld.DefaultView.ToTable();
                }
                for (int i = startIndex; i < endIndex; i++)
                {
                    DataRow dr = dtOld.Rows[i];
                    dtReturn.Rows.Add(dr.ItemArray);
                }
                return dtReturn;
            }
            return null;
        }
        /// <summary>
        /// 生成临时表(绑定数据)
        /// </summary>
        /// <returns></returns>
        private DataTable CreateSequentialByFactor(string[] pollutantCodes)
        {
            DataTable dtNew = new DataTable();
            dtNew.Columns.Add("PointId", typeof(string));
            //拼接字段
            for (int i = 0; i < pollutantCodes.Length; i++)
            {
                dtNew.Columns.Add(pollutantCodes[i], typeof(string));//各个因子 拼接成为列
            }
            dtNew.Columns.Add("TotalValue", typeof(string));

            dtNew.Columns.Add("blankspaceColumn");//空白列

            return dtNew;
        }

        /// <summary>
        /// 生成临时表(绑定尾部合计数据)
        /// </summary>
        /// <returns></returns>
        private DataTable CreateTailTotalSequential()
        {
            DataTable dtNew = new DataTable();
            dtNew.Columns.Add("PollutantCode", typeof(string));
            dtNew.Columns.Add("PollutantTotal", typeof(string));

            return dtNew;
        }
    }
}
