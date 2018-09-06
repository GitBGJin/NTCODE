using SmartEP.Core.Generic;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.MPInfo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Service.DataAnalyze.Water
{
    /// <summary>
    /// 名称：WaterAuditStateRemindService.cs
    /// 创建人：王秉晟
    /// 创建日期：2015-09-15
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：地表水审核状态提醒类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class WaterAuditStateRemindService
    {
        /// <summary>
        /// 测点类
        /// </summary>
        MonitoringPointWaterService g_MonitoringPointWater = Singleton<MonitoringPointWaterService>.GetInstance();

        #region 获取实时在线状态信息
        /// <summary>
        /// 获取审核状态提醒数据
        /// </summary>
        /// <param name="siteTypeUids">站点类型Uid数组</param>
        /// <param name="dtmDate">日期</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="orderBy">排序方式（字段：PointId）</param>
        /// <returns>返回的结果集
        /// 返回的列名
        /// PointId：站点Id
        /// DateTime：日期
        /// AuditStatus：审核状况（未审核、部分审核）
        /// AbnormalCondition：异常情况
        /// </returns>
        public DataView GetAuditStateRemindDataPager(string[] siteTypeUids, DateTime dtmDate, int pageSize,
             int pageNo, out int recordTotal, string orderBy = "PointId")
        {
            DataTable dtNew = new DataTable();
            dtNew = CreateNewDataTable(dtNew);
            dtNew.Rows.Add("41", dtmDate, "未审核", "超标5 离群2 无效1");
            dtNew.Rows.Add("43", dtmDate, "部分审核", "离群2 无效5");
            dtNew.Rows.Add("97", dtmDate, "未审核", "无效3");
            recordTotal = dtNew.Rows.Count;
            return dtNew.AsDataView();
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 根据站点类型Uid数组站点Id列表
        /// </summary>
        /// <param name="siteTypeUids">站点类型Uid数组</param>
        /// <returns></returns>
        private IList<string> GetPointIdsByContrlUid(string[] siteTypeUids)
        {
            IList<MonitoringPointEntity> pointList = new List<MonitoringPointEntity>();
            siteTypeUids = siteTypeUids.Distinct().ToArray();
            foreach (string siteTypeUid in siteTypeUids)
            {
                IQueryable<MonitoringPointEntity> pointQueryable = g_MonitoringPointWater.RetrieveListBySiteTypeName(siteTypeUid);//根据控制类型获取点位列表
                pointList = pointList.Union(pointQueryable).ToList();
            }
            return pointList.Select(t => t.PointId.ToString()).ToArray();
        }

        /// <summary>
        /// 根据原始表的列为新表生成列
        /// </summary>
        /// <param name="dtOld">原始表</param>
        /// <returns></returns>
        private DataTable CreateNewDataTable(DataTable dtOld)
        {
            DataTable dtNew = null;
            if (dtOld != null)
            {
                dtNew = new DataTable();
                dtNew.Columns.Add("PointId", typeof(string));//站点Id
                dtNew.Columns.Add("DateTime", typeof(DateTime));//日期
                dtNew.Columns.Add("AuditStatus", typeof(string));//审核状况
                dtNew.Columns.Add("AbnormalCondition", typeof(string));//异常情况
                //if (dtNew.Columns.Contains("blankspaceColumn"))
                //{
                //    dtNew.Columns.Remove("blankspaceColumn");
                //}
                //dtNew.Columns.Add("blankspaceColumn", typeof(string));//空白列
            }
            return dtNew;
        }

        /// <summary>
        /// 获取分页后的数据
        /// </summary>
        /// <param name="dtOld">原始表</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="orderBy">排序方式（如：PointId,Tstamp）</param>
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
        /// 根据测点Id数组和站点类型列表增加空白数据行 
        /// </summary>
        /// <param name="pointIds">测点Id数组</param>
        /// <param name="siteTypeByPointIdsList">站点类型列表</param>
        /// <param name="dt">表</param>
        private void AddDataRowsByPointSiteTypes(string[] pointIds, Dictionary<string, string> siteTypeByPointIdsList, DataTable dt)
        {
            if (pointIds == null || pointIds.Length == 0 || dt == null)
            {
                return;
            }
            pointIds = pointIds.Distinct().ToArray();
            for (int i = 0; i < pointIds.Length; i++)
            {
                string pointId = pointIds[i];
                if (dt.Select(string.Format("PointId='{0}'", pointId)).Length == 0)
                {
                    DataRow dr = dt.NewRow();
                    dr["PointId"] = pointIds[i];
                    dr["PointTypeUid"] = (siteTypeByPointIdsList.ContainsKey(pointId))
                        ? siteTypeByPointIdsList[pointId] : string.Empty;//站点类型名称
                    dt.Rows.Add(dr);
                }
            }
        }
        #endregion
    }
}
