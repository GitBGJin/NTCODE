using SmartEP.Core.Generic;
using SmartEP.DomainModel.BaseData;
using SmartEP.MonitoringBusinessRepository.QualityControlOperation.Water;
using SmartEP.Service.BaseData.MPInfo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.OperatingMaintenance.Water
{
    /// <summary>
    /// 名称：QC_QueryService.cs
    /// 创建人：樊垂贺
    /// 创建日期：2016-05-29
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 状态表服务层类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class QC_QueryService
    {
        //标液考核仓储层
        StandardSolutionCheckRepository r_StandardSolutionCheckRepository = Singleton<StandardSolutionCheckRepository>.GetInstance();
        //站点信息
        MonitoringPointWaterService s_MonitoringPointWaterService = Singleton<MonitoringPointWaterService>.GetInstance();
        /// <summary>
        /// 获取质控任务合格率
        /// </summary>
        /// <param name="pointIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public DataTable getQCData(string[] pointIds, string[] factors, DateTime dtStart, DateTime dtEnd, string[] type)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("pointName", typeof(string));   //测点
            if (type.Contains("BiaoTest"))
            {
                dt.Columns.Add("BiaoTest", typeof(string));    //标液考核
            }
            if (type.Contains("MangTest"))
            {
                dt.Columns.Add("MangTest", typeof(string));    //盲样考核
            }
            if (type.Contains("JiaStandard"))
            {
                dt.Columns.Add("JiaStandard", typeof(string)); //加标回收
            }
            if (type.Contains("ShiSampling"))
            {
                dt.Columns.Add("ShiSampling", typeof(string)); //实样比对
            }
            if (type.Contains("BiaoTest"))
            {
                foreach (string factor in factors)
                {
                    dt.Columns.Add("Biao" + factor, typeof(string));  //标液考核因子
                }
            }
            if (type.Contains("MangTest"))
            {
                foreach (string factor in factors)
                {
                    dt.Columns.Add("Mang" + factor, typeof(string));  //盲样考核因子
                }
            }
            if (type.Contains("JiaStandard"))
            {
                foreach (string factor in factors)
                {
                    dt.Columns.Add("Jia" + factor, typeof(string));   //加标回收因子
                }
            }
            if (type.Contains("ShiSampling"))
            {
                foreach (string factor in factors)
                {
                    dt.Columns.Add("Shi" + factor, typeof(string));   //实样比对因子
                }
            }
            dt.Columns.Add("Total", typeof(string));
            dt.Columns.Add("factorTotal", typeof(string));
            try
            {
                DataView dtBiaoTest = null;
                DataView dtMangTest = null;
                DataView dtJiaTest = null;
                DataView dtShiTest = null;
                if (type.Contains("BiaoTest"))
                {
                    dtBiaoTest = getQualityData("5E469A23-76F9-4F53-9D98-F63D8DF199F0", pointIds, dtStart, dtEnd);//标液考核数据
                }
                if (type.Contains("MangTest"))
                {
                    dtMangTest = getQualityData("7D32FAB8-07CE-41A1-807C-C0B8A161322D", pointIds, dtStart, dtEnd);//盲样考核数据
                }
                if (type.Contains("JiaStandard"))
                {
                    dtJiaTest = getQualityData("6F183194-3B3E-4337-A57F-5D17845544A7", pointIds, dtStart, dtEnd);//加标回收数据
                }
                if (type.Contains("ShiSampling"))
                {
                    dtShiTest = getQualityData("8897D4CF-EA8C-450B-9009-149232DF2985", pointIds, dtStart, dtEnd);//实样比对数据
                }
                DataView[] dvArry = new DataView[] { dtBiaoTest, dtMangTest, dtJiaTest, dtShiTest };

                IQueryable<MonitoringPointEntity> IQueryablePointEntity = s_MonitoringPointWaterService.RetrieveWaterMPListByEnable();
                foreach (string pointid in pointIds)
                {
                    string[] pointName = IQueryablePointEntity.Where(t => t.PointId == int.Parse(pointid)).Select(t => t.MonitoringPointName).ToArray();
                    DataRow dtNew = dt.NewRow();
                    if (pointName.Length > 0)
                    {
                        dtNew["pointName"] = pointName[0];
                    }
                    if (type.Contains("BiaoTest"))
                    {
                        dtNew["BiaoTest"] = getQualifiedRate(dtBiaoTest, pointid, "");
                    }
                    if (type.Contains("MangTest"))
                    {
                        dtNew["MangTest"] = getQualifiedRate(dtMangTest, pointid, "");
                    }
                    if (type.Contains("JiaStandard"))
                    {
                        dtNew["JiaStandard"] = getQualifiedRate(dtJiaTest, pointid, "");
                    }
                    if (type.Contains("ShiSampling"))
                    {
                        dtNew["ShiSampling"] = getQualifiedRate(dtShiTest, pointid, "");
                    }
                    foreach (string factor in factors)
                    {
                        if (type.Contains("BiaoTest"))
                        {
                            dtNew["Biao" + factor] = getQualifiedRate(dtBiaoTest, pointid, factor);
                        }
                        if (type.Contains("MangTest"))
                        {
                            dtNew["Mang" + factor] = getQualifiedRate(dtMangTest, pointid, factor);
                        }
                        if (type.Contains("JiaStandard"))
                        {
                            dtNew["Jia" + factor] = getQualifiedRate(dtJiaTest, pointid, factor);
                        }
                        if (type.Contains("ShiSampling"))
                        {
                            dtNew["Shi" + factor] = getQualifiedRate(dtShiTest, pointid, factor);
                        }
                    }
                    dtNew["Total"] = getTotalRate(dvArry, pointid, null);
                    dtNew["factorTotal"] = getTotalRate(dvArry, pointid, factors);
                    dt.Rows.Add(dtNew);
                }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }
        public DataView getQualityData(string missionID, string[] pointIds, DateTime dtStart, DateTime dtEnd)
        {
            string strWhere = " 1=1 ";
            if (pointIds != null && pointIds.Length > 0)
            {
                string strPointIds = string.Join("','", pointIds);
                strWhere += string.Format(" and PointId in ('{0}') ", strPointIds);
            }
            if (!string.IsNullOrWhiteSpace(missionID))
            {
                strWhere += string.Format(" and MissionID ='{0}' ", missionID);
            }
            if (dtStart != null)
            {
                strWhere += string.Format(" and Tstamp <='{0}' ", dtEnd);
            }
            if (dtEnd != null)
            {
                strWhere += string.Format(" and Tstamp >='{0}' ", dtStart);
            }
            return r_StandardSolutionCheckRepository.GetList(strWhere).DefaultView;

        }
        public string getTotalRate(DataView[] dvArry, string pointId, string[] factor)
        {
            string totalRate = "";
            if (factor == null)
            {
                int totalCount = 0;
                int totalEvaluate = 0;
                foreach (DataView dvItem in dvArry)
                {
                    if (dvItem != null)
                    {
                        DataView dv = new DataView(dvItem.ToTable());

                        dv.RowFilter = "PointId=" + pointId;
                        int biaoCount = dv.Count;
                        dv.RowFilter = "PointId=" + pointId + " and Evaluate='合格'";
                        int biaoEvaluate = dv.Count;
                        totalCount += biaoCount;
                        totalEvaluate += biaoEvaluate;
                    }

                }
                if (totalCount > 0)
                {
                    string biaoRate = Math.Round(Double.Parse(totalEvaluate.ToString()) / Double.Parse(totalCount.ToString()) * 100, 1) + "%";
                    totalRate = biaoRate + " <br />" + totalEvaluate + ":" + totalCount;
                }
            }
            else
            {
                int totalFactorCount = 0;
                int totalFactorEvaluate = 0;
                string strfactor = string.Join("','", factor);
                foreach (DataView dvItem in dvArry)
                {
                    if (dvItem != null)
                    {
                        DataView dv = new DataView(dvItem.ToTable());
                        dv.RowFilter = "PointId=" + pointId + " and PollutantCode in ('" + strfactor + "')";
                        int biaoFactorCount = dv.Count;
                        dv.RowFilter = "PointId=" + pointId + " and Evaluate='合格'" + " and PollutantCode in ('" + strfactor + "')";
                        int biaoFactorEvaluate = dv.Count;
                        totalFactorCount += biaoFactorCount;
                        totalFactorEvaluate += biaoFactorEvaluate;
                    }
                }
                if (totalFactorCount > 0)
                {
                    string biaoRate = Math.Round(Double.Parse(totalFactorEvaluate.ToString()) / Double.Parse(totalFactorCount.ToString()) * 100, 1) + "%";
                    totalRate = biaoRate + " <br />" + totalFactorEvaluate + ":" + totalFactorCount;
                }
            }
            return totalRate;
        }
        /// <summary>
        /// 计算质控任务合格率
        /// </summary>
        /// <param name="dv"></param>
        /// <param name="pointId"></param>
        /// <param name="factor"></param>
        /// <returns></returns>
        public string getQualifiedRate(DataView dvTest, string pointId, string factor)
        {
            DataView dv = new DataView(dvTest.ToTable());
            string QualifiedRate = "";
            if (factor == "")
            {
                dv.RowFilter = "PointId=" + pointId;
                int biaoCount = dv.Count;
                dv.RowFilter = "PointId=" + pointId + " and Evaluate='合格'";
                int biaoEvaluate = dv.Count;
                if (biaoCount > 0)
                {
                    string biaoRate = Math.Round(Double.Parse(biaoEvaluate.ToString()) / Double.Parse(biaoCount.ToString()) * 100, 1) + "%";
                    QualifiedRate = biaoRate + " <br />" + biaoEvaluate + ":" + biaoCount;
                }
            }
            else
            {
                dv.RowFilter = "PointId=" + pointId + " and PollutantCode='" + factor + "'";
                int biaoFactorCount = dv.Count;
                dv.RowFilter = "PointId=" + pointId + " and Evaluate='合格'" + " and PollutantCode='" + factor + "'";
                int biaoFactorEvaluate = dv.Count;
                if (biaoFactorCount > 0)
                {
                    string biaoFactorRate = Math.Round(Double.Parse(biaoFactorEvaluate.ToString()) / Double.Parse(biaoFactorCount.ToString()) * 100, 1) + "%";
                    QualifiedRate = biaoFactorRate + " <br />" + biaoFactorEvaluate + ":" + biaoFactorCount;
                }
            }
            return QualifiedRate;
        }
    }
}
