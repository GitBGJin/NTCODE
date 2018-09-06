using SmartEP.Core.Generic;
using SmartEP.Utilities.AdoData;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.BaseData
{
    /// <summary>
    /// 名称：WaterAnalyzeDAL.cs
    /// 创建人：窦曙健
    /// 创建日期：2015-10-20
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：水质分析数据访问层
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class WaterAnalyzeDAL
    {
        /// <summary>
        /// 数据库处理类
        /// </summary>
        DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();
       
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connection = "AMS_BaseDataConnection";
     
        /// <summary>
        /// 根据站点获取水质分析数据
        /// </summary>
        /// <param name="portIds">站点</param>
        /// <returns></returns>
        public DataView GetWaterAnalyzeData(string[] portIds)
        {
            //站点处理
            string portIdsStr = string.Empty;
            if (portIds.Length == 1 && !string.IsNullOrEmpty(portIds[0]))
            {
                portIdsStr = " PointId =" + portIds[0];
            }
            else
            {
                portIdsStr = " PointId IN(" + StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), ",") + ")";
            }
            string sql = @"select x.*,codeitem.ItemText as CalEQIType from
(select b.*,ci.ItemText from
                        (select a.monitoringPointUid,a.PointId,a.MonitoringPointName,a.RegionUid,a.Valley,a.WatersName,a.CalEQITypeUid,e.Class,e.IEQI
                         from
                        (SELECT w.monitoringPointUid,w.Valley,w.WatersName,w.EQIUid,w.CalEQITypeUid,mp.PointId,mp.MonitoringPointName,mp.RegionUid
                         from [MPInfo].[TB_MonitoringPointExtensionForEQMSWater] w
                         left join [MPInfo].[TB_MonitoringPoint] mp
                         on w.monitoringPointUid=mp.MonitoringPointUid
                         where mp.ApplicationUid='watrwatr-watr-watr-watr-watrwatrwatr') a
                         left join [Standard].[TB_EQI] e
                         on a.EQIUid=e.EQIUid) b
                         left join [SY_Frame_Code_Item] ci
                         on b.RegionUid=ci.RowGuid) x
                         left join [SY_Frame_Code_Item] codeitem
                         on x.CalEQITypeUid=codeitem.RowGuid
                         where " + portIdsStr;
            return g_DatabaseHelper.ExecuteDataView(sql, connection);
        }

    }
}
