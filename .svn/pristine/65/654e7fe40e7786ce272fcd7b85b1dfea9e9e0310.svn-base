using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel;
using SmartEP.DomainModel.BaseData;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.Core.Enums;
using SmartEP.Service.Frame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.AutoMonitoring.Air
{
    /// <summary>
    /// 名称：AirDataSamplingRateNewService.cs
    /// 创建人：尤红兵
    /// 创建日期：2015-12-18
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 原始数据服务层
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class AirDataSamplingRateNewService : BaseGenericRepository<MonitoringBusinessModel, ReportSamplingRateByDayEntity>
    {
        public override bool IsExist(string strKey)
        {
            return true;
        }
        MonitoringPointAirService m_MonitoringPointAirService = new MonitoringPointAirService();
        /// <summary>
        /// 获取水的捕获率数据
        /// </summary>
        /// <param name="portIds">测点组</param>
        /// <param name="factors">因子组</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">结束时间</param>
        /// <returns></returns>
        public DataView DataSamplingRateRetrieve(string[] portIds, string[] factors, DateTime dtmStart, DateTime dtmEnd,
            int pageSize, int pageNo, out int recordTotal, out DataView dvStatistical, string orderBy = "PointId,Tstamp")
        {
            DataTable dtDataSamplingRate = ConvertToDataTable(Retrieve(it => it.ReportDateTime >= dtmStart && it.ReportDateTime <= dtmEnd && portIds.Contains(it.PointId.ToString()) && factors.Contains(it.PollutantCode) && it.ApplicationUid == "airaaira-aira-aira-aira-airaairaaira"));
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
                    DataRow[] drDataSamplingRate = dtDataSamplingRate.Select(string.Format("PointId='{0}' and PollutantCode='{1}'", point, factor));

                    if (drDataSamplingRate != null && drDataSamplingRate.Count() > 0)
                    {
                        decimal actualValue = 0;
                        decimal sampleValue = 0;
                        string percentageValue = string.Empty;
                        foreach (DataRow dr in drDataSamplingRate)
                        {
                            if (dr["ActualCollectionNumber"] != DBNull.Value)
                            {
                                actualValue += Convert.ToDecimal(dr["ActualCollectionNumber"]);//实际测试值
                            }
                            if (dr["SamplingNumber"] != DBNull.Value)
                            {
                                sampleValue += Convert.ToDecimal(dr["SamplingNumber"]);//应该测试值
                            }
                        }

                        //总的应该测试值不能为0 因为是分母
                        if (sampleValue != 0)
                        {
                            //捕获率
                            percentageValue = string.Format("{0:N2}", (actualValue / sampleValue) * 100);
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
                    drTemporaryTable["TotalValue"] = string.Format("{0:N2}%<br/>{1}:{2}", (totalActualValue / totalSampleValue) * 100, totalActualValue, totalSampleValue);

                }
                dtTemporaryTable.Rows.Add(drTemporaryTable);
            }

            //获取合计值
            decimal tailTotalActualValue = 0;//尾部总的实际测试值
            decimal tailTotalSampleValue = 0;//尾部总的应该测试值
            foreach (string factor in factors)
            {
                DataRow[] drDataSamplingRate = dtDataSamplingRate.Select(string.Format("PollutantCode='{0}'", factor));

                if (drDataSamplingRate != null && drDataSamplingRate.Count() > 0)
                {
                    DataRow drTailTotalTable = dtTailTotalTable.NewRow();

                    drTailTotalTable["PollutantCode"] = factor.ToString();

                    decimal actualValue = 0;
                    decimal sampleValue = 0;
                    string percentageValue = string.Empty;
                    foreach (DataRow dr in drDataSamplingRate)
                    {
                        if (dr["ActualCollectionNumber"] != DBNull.Value)
                        {
                            actualValue += Convert.ToDecimal(dr["ActualCollectionNumber"]);//实际测试值
                        }
                        if (dr["SamplingNumber"] != DBNull.Value)
                        {
                            sampleValue += Convert.ToDecimal(dr["SamplingNumber"]);//应该测试值
                        }
                    }

                    //总的应该测试值不能为0 因为是分母
                    if (sampleValue != 0)
                    {
                        //捕获率
                        percentageValue = string.Format("{0:N2}", (actualValue / sampleValue) * 100);
                    }

                    if (!string.IsNullOrWhiteSpace(percentageValue))
                    {
                        drTailTotalTable["PollutantTotal"] = string.Format("{0}%<br/>{1}:{2}", percentageValue, actualValue, sampleValue);
                    }

                    tailTotalActualValue += actualValue;
                    tailTotalSampleValue += sampleValue;

                    dtTailTotalTable.Rows.Add(drTailTotalTable);
                }
            }

            DataRow drTailTotal = dtTailTotalTable.NewRow();//最后添加一行总的统计行
            drTailTotal["PollutantCode"] = "TotalValue";//默认赋值
            if (tailTotalSampleValue != 0)
            {
                drTailTotal["PollutantTotal"] = string.Format("{0:N2}%<br/>{1}:{2}", (tailTotalActualValue / tailTotalSampleValue) * 100, tailTotalActualValue, tailTotalSampleValue);
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
            foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(typeof(ReportSamplingRateByDayEntity)))
            {
                dataTable.Columns.Add(pd.Name);
            }
            foreach (ReportSamplingRateByDayEntity item in enumerable)
            {
                var Row = dataTable.NewRow();

                foreach (PropertyDescriptor dp in TypeDescriptor.GetProperties(typeof(ReportSamplingRateByDayEntity)))
                {
                    Row[dp.Name] = dp.GetValue(item);
                }
                dataTable.Rows.Add(Row);
            }
            return dataTable;
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
        /// 数据捕获率详细信息
        /// </summary>
        /// <param name="portIds">测点组</param>
        /// <param name="factors">因子组</param>
        /// <param name="dtmStart">开始时间</param>
        /// <param name="dtmEnd">结束时解决</param>
        /// <returns></returns>
        public DataView GetSamplingDetailData(string[] portIds, string[] factors, DateTime dtmStart, DateTime dtmEnd)
        {
            Dictionary<string, string> pointInstrumentList = GetInstrumentNamesByPointIds(portIds);//根据测点Id数组获取测点Id和集成商名称的对应关系列 
            DataTable dtSamplingDetailData = ConvertToDataTable(Retrieve(it => it.ReportDateTime >= dtmStart && it.ReportDateTime <= dtmEnd && portIds.Contains(it.PointId.ToString()) && factors.Contains(it.PollutantCode) && it.ApplicationUid == "airaaira-aira-aira-aira-airaairaaira"));
            DataTable dtNew = dtSamplingDetailData.Clone();
            if (dtSamplingDetailData != null && dtSamplingDetailData.Rows.Count > 0)
            {
                MonitoringPointEntity entity = m_MonitoringPointAirService.RetrieveEntityByPointId(int.Parse(portIds[0]));//根据测点Id获取测点名称
                string pointName = (entity != null) ? entity.MonitoringPointName : string.Empty;

                dtNew.Columns.Add("IntegratorName", typeof(string));//集成商名称
                dtNew.Columns.Add("Days", typeof(int));//日期
                dtNew.Columns.Add("PointName", typeof(string));//测点名称
                dtNew.Columns.Add("NewTime", typeof(DateTime));//时间
                for (int i = 0; i < dtSamplingDetailData.Rows.Count; i++)
                {
                    DataRow drSamplingDetailData = dtSamplingDetailData.Rows[i];
                    DataRow drNew = dtNew.NewRow();
                    drNew["IntegratorName"] = pointInstrumentList[portIds[0].ToString()];
                    drNew["PointName"] = pointName;
                    foreach (DataColumn dcOld in dtSamplingDetailData.Columns)
                    {
                        if (!string.IsNullOrWhiteSpace(drSamplingDetailData[dcOld].ToString()))
                        {
                            if (dcOld.ColumnName.Equals("SamplingRate"))
                            {
                                drNew[dcOld.ColumnName] = string.Format("{0:N2}", Convert.ToDecimal(drSamplingDetailData[dcOld]) * 100) + "%";
                            }
                            else
                            {
                                drNew[dcOld.ColumnName] = drSamplingDetailData[dcOld].ToString();
                                if (dcOld.ColumnName.Equals("ReportDateTime"))
                                {
                                    if (drSamplingDetailData[dcOld] != null)
                                    {
                                        DateTime dtTempNew = DateTime.Now.AddDays(1);
                                        DateTime dtTemp = DateTime.TryParse(drSamplingDetailData[dcOld].ToString(), out dtTemp) ? dtTemp : dtTempNew;
                                        if (dtTemp != dtTempNew)
                                        {
                                            drNew["NewTime"] = dtTemp;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    dtNew.Rows.Add(drNew);
                }
            }
            return dtNew.DefaultView;
        }
        /// <summary>
        /// 根据测点Id数组获取测点Id和集成商名称的对应关系列 
        /// </summary>
        /// <param name="pointIds"></param>
        /// <returns></returns>
        private Dictionary<string, string> GetInstrumentNamesByPointIds(string[] pointIds)
        {
            DictionaryService dictionaryService = Singleton<DictionaryService>.GetInstance();
            Dictionary<string, string> pointInstrumentList = new Dictionary<string, string>();
            IQueryable<MonitoringPointEntity> monitorPointQueryable = m_MonitoringPointAirService.RetrieveListByPointIds(pointIds);//根据站点ID数组获取站点
            IQueryable<V_CodeMainItemEntity> codeMainItemQueryable = dictionaryService.RetrieveList(DictionaryType.AMS, "集成商类型");//获取城市类型
            foreach (MonitoringPointEntity monitoringPointEntity in monitorPointQueryable)
            {
                if (!pointInstrumentList.ContainsKey(monitoringPointEntity.PointId.ToString()))
                {
                    string instrumentName = codeMainItemQueryable.Where(t => t.ItemGuid.Equals(monitoringPointEntity.InstrumentIntegratorUid))
                                                .Select(t => t.ItemText).FirstOrDefault();//集成商名称
                    pointInstrumentList.Add(monitoringPointEntity.PointId.ToString(), instrumentName);
                }
            }
            return pointInstrumentList;
        }
    }
}
