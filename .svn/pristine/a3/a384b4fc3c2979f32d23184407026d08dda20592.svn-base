using SmartEP.Core.Generic;
using SmartEP.Data.Enums;
using SmartEP.Utilities.AdoData;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.MonitoringBusiness.Air
{
    public class QualityControlDataSearchDAL
    {
        #region << 变量 >>
        /// <summary>
        /// 数据库处理类
        /// </summary>
        protected DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();


        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        protected string connection = null;
        protected string connection1 = null;
        /// <summary>
        /// 数据库表名
        /// </summary>
        protected string tableName = null;

        #endregion

        #region << 构造函数 >>
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="aQIDataType">AQI数据类型</param>
        public QualityControlDataSearchDAL()
        {
            // tableName = SmartEP.Data.Enums.EnumMapping.GetTableName(AQIDataType.RegionDayAQI);
            connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.Frame);
            connection1 = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.MonitoringBusiness);
        }
        #endregion

        #region << 数据查询方法 >>
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <param name="orderBy">排序字段(MonitoringRegionUid,ReportDateTime)</param>
        /// <returns></returns>
        public DataView GetDataBasePager()
        {
            string tableName1 = "TB_OMMP_InstrumentInfo";
            try
            {
                string sql = string.Format(@"select id, [RowGuid],InstrumentName+'/'+SpecificationModel as InstrumentType ,SpecificationModel
                                             from {0} 
                                            where ObjectType=2 and RowStatus=1 and InfoType=1",
                                           tableName1);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }

        }
        /// <summary>
        /// 获取仪器信息
        /// </summary>
        /// <param name="ObjectType">测点类型：1水2气</param>
        /// <returns></returns>
        public DataView GetDataByObjectType(string ObjectType)
        {
            string tableName1 = "TB_OMMP_InstrumentInfo";
            try
            {
                string sql = string.Format(@"select id, [RowGuid],InstrumentName+'/'+SpecificationModel as InstrumentType ,SpecificationModel
                                             from {0} 
                                            where ObjectType={1} and RowStatus=1",
                                           tableName1, ObjectType);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }

        }
        /// <summary>
        /// 获取仪器信息
        /// </summary>
        /// <param name="ObjectType">测点类型：1水2气</param>
        /// <returns></returns>
        public DataView GetDataByOperations()
        {
            string tableName1 = "TB_OMMP_ManufacturerInfo";
            try
            {
                string sql = string.Format(@"select ManufacturerName as ItemText, RowGuid as ItemValue  from {0} where ManufacturerName is not null",
                                           tableName1);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }

        }
        /// <summary>
        /// 获取站点信息
        /// </summary>
        /// <param name="ObjectType">测点类型：1水2气</param>
        /// <returns></returns>
        public DataView GetDataPointGuid(string[] rowGuid)
        {
            string tableName1 = "TB_OMMP_MaintenanceObject";
            //测点
            string portStr = string.Empty;
            if (rowGuid != null && rowGuid.Length > 0)
                portStr = "AND ObjectID IN ('" + StringExtensions.GetArrayStrNoEmpty(rowGuid.ToList(), "','") + "')";
            try
            {
                string sql = string.Format(@"select *
                                             from {0} 
                                            where 1=1 {1}",
                                           tableName1, portStr);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }

        }
        /// <summary>
        /// 获取站点信息
        /// </summary>
        /// <param name="ObjectType">测点类型：1水2气</param>
        /// <returns></returns>
        public DataView GetDataPointName(string[] PointNames)
        {
            string tableName1 = "TB_OMMP_MaintenanceObject";
            //测点
            string portStr = string.Empty;
            if (PointNames != null && PointNames.Length > 0)
                portStr = "AND ObjectName IN ('" + StringExtensions.GetArrayStrNoEmpty(PointNames.ToList(), "','") + "')";
            try
            {
                string sql = string.Format(@"select *
                                             from {0} 
                                            where 1=1 {1}",
                                           tableName1, portStr);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }

        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <param name="orderBy">排序字段(MonitoringRegionUid,ReportDateTime)</param>
        /// <returns></returns>
        public DataView GetDataPager(string objectType)
        {
            string where = string.Empty;
            if (objectType.ToString() != "")
                where = "ObjectType=" + objectType + " AND";
            string tableName2 = "TB_OMMP_InstrumentInstance";
            try
            {
                string sql = string.Format(@"select id,RowGuid,Infoguid, InstanceName+'/'+FixedAssetNumber as InstrumentName,FixedAssetNumber
                                             from {0} 
                                            where {1} RowStatus=1 and InfoGuid is not null",
                                           tableName2, where);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }

        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <param name="orderBy">排序字段(MonitoringRegionUid,ReportDateTime)</param>
        /// <returns></returns>
        public DataView GetDataPointGuidPager(string objectType, string pointGuid)
        {
            string sqlStr = string.Empty;
            if (pointGuid != "")
                sqlStr = string.Format(@" AND SiteGuid=(select RowGuid from TB_OMMP_MaintenanceObject where ObjectID='{0}')", pointGuid);

            string where = string.Empty;
            if (objectType.ToString() != "")
                where = "ObjectType=" + objectType + " AND";
            string tableName2 = "TB_OMMP_InstrumentInstance";
            try
            {
                string sql = string.Format(@"select id,RowGuid,Infoguid, InstanceName+'/'+FixedAssetNumber as InstrumentName,FixedAssetNumber
                                             from {0} 
                                            where {1} RowStatus=1 and InfoGuid is not null {2}",
                                           tableName2, where, sqlStr);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }

        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <param name="orderBy">排序字段(MonitoringRegionUid,ReportDateTime)</param>
        /// <returns></returns>
        public DataView GetDataPointPager(string objectType, string[] rowGuids)
        {
            string where = string.Empty;
            if (objectType.ToString() != "")
                where = "ObjectType=" + objectType + " AND";
            //测点
            string portStr = string.Empty;
            if (rowGuids != null && rowGuids.Length > 0 && rowGuids[0].ToString() != "")
                portStr = " AND SiteGuid IN ('" + StringExtensions.GetArrayStrNoEmpty(rowGuids.ToList(), "','") + "')";
            string tableName2 = "TB_OMMP_InstrumentInstance";
            try
            {
                string sql = string.Format(@"select id,RowGuid,Infoguid, InstanceName+'/'+FixedAssetNumber as InstrumentName,FixedAssetNumber
                                             from {0} 
                                            where {1} RowStatus=1 and InfoGuid is not null {2}",
                                           tableName2, where, portStr);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }

        }
        /// <summary>
        /// 获取仪器实例信息
        /// </summary>
        /// <param name="ObjectType">测点类型：1水2气</param>
        /// <returns></returns>
        public DataView GetInstanceDataByObjectType(string ObjectType, string IsSpareParts)
        {
            string tableName2 = "TB_OMMP_InstrumentInstance";
            try
            {
                string sqlwhere = "";
                if (!string.IsNullOrWhiteSpace(IsSpareParts))
                {
                    sqlwhere += string.Format(@"  and IsSpareParts={0}", IsSpareParts);
                }
                string sql = string.Format(@"select id, Infoguid, InstanceName+'/'+FixedAssetNumber as InstrumentName,FixedAssetNumber
                                             from {0} 
                                            where ObjectType={1} and RowStatus=1 and InfoGuid is not null {2}",
                                           tableName2, ObjectType, sqlwhere);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }

        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <param name="orderBy">排序字段(MonitoringRegionUid,ReportDateTime)</param>
        /// <returns></returns>
        public DataView GetAllDataPager(string[] pointIds, string instrumentName, string[] SNType, string[] inState, string[] Operators, DateTime dtBegin, DateTime dtEnd)
        {
            string tableName1 = "TB_OMMP_InstrumentInfo";
            string tableName2 = "TB_OMMP_InstrumentInstance";
            string tableName3 = "TB_OMMP_MaintenanceObject";
            string tableName4 = "TB_OMMP_InstrumentInstanceState";
            string tableName5 = "TB_OMMP_ManufacturerInfo";
            try
            {
                //测点
                string portStr = string.Empty;
                if (pointIds != null && pointIds.Length > 0)
                    portStr = "ObjectID IN ('" + StringExtensions.GetArrayStrNoEmpty(pointIds.ToList(), "','") + "')";
                //仪器类别
                string snStr = string.Empty;
                if (SNType != null && SNType.Length > 0 && SNType[0].ToString() != "" && portStr != "")
                    snStr = " AND InfoGuid IN ('" + StringExtensions.GetArrayStrNoEmpty(SNType.ToList(), "','") + "')";
                else if (SNType != null && SNType.Length > 0 && SNType[0].ToString() != "")
                    snStr = "InfoGuid IN ('" + StringExtensions.GetArrayStrNoEmpty(SNType.ToList(), "','") + "')";
                //运维商
                string operStr = string.Empty;
                if (Operators != null && Operators.Length > 0 && Operators[0].ToString() != "" && (portStr != "" || snStr != ""))
                    operStr = " AND Manufacturer IN ('" + StringExtensions.GetArrayStrNoEmpty(Operators.ToList(), "','") + "')";
                else if (Operators != null && Operators.Length > 0 && Operators[0].ToString() != "")
                    operStr = "Manufacturer IN ('" + StringExtensions.GetArrayStrNoEmpty(Operators.ToList(), "','") + "')";
                //运行状态
                string stateStr = string.Empty;
                if (inState != null && inState.Length > 0 && inState[0].ToString() != "")
                    stateStr = " AND State IN ('" + StringExtensions.GetArrayStrNoEmpty(inState.ToList(), "','") + "')";
                //仪器编号
                string nameStr = string.Empty;
                if (instrumentName != "" && (portStr != "" || snStr != "" || operStr != ""))
                    nameStr = " AND FixedAssetNumber='" + instrumentName + "'";
                else if (instrumentName != "")
                    nameStr = "FixedAssetNumber='" + instrumentName + "'";

                string where = string.Empty;
                if (portStr.IsNotNullOrDBNull() || snStr.IsNotNullOrDBNull() || operStr.IsNotNullOrDBNull() || nameStr.IsNotNullOrDBNull())
                {
                    where = " AND " + portStr + snStr + operStr + nameStr;
                }
                string sql = string.Format(@"select a.[RowGuid],InstrumentName+'/'+SpecificationModel as InstrumentType,a.SpecificationModel ,a.Brand,a.Manufacturer,InstanceName+'/'+FixedAssetNumber as InstrumentName, b.FixedAssetNumber,d.State,b.BuyDate,c.ObjectName as pointName
									          ,a.[Dealers],a.[Operations],b.LastDate,b.NextDate,b.RecentDate,b.RegistrationDate,b.TeamGuid,TeamName,ManufacturerName
                                           from {0} as a left join {1} as b
									on a.[RowGuid]=b.InfoGuid  left join {2} as c on b.SiteGuid=c.RowGuid 
									left join {3} as d on b.Status=d.RowGuid 
	                                left join {8} as n on a.Manufacturer=n.RowGuid 
									where a.ObjectType=2 and b.ObjectType=2 and a.RowStatus=1 and b.RowStatus=1 
                                      {4} {5} AND BuyDate >= '{6}' and BuyDate <= '{7}' and a.InfoType=1",
                                           tableName1, tableName2, tableName3, tableName4, where, stateStr, dtBegin.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), tableName5);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }

        }
        /// <summary>
        ///仪器状态信息
        /// </summary>
        /// <returns></returns>
        public DataView GetDataStatePager()
        {
            string tableName1 = "TB_OMMP_InstrumentInstanceState";
            try
            {
                string sql = string.Format(@"select *
                                             from {0} ",
                                           tableName1);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }

        }
        /// <summary>
        ///维护人信息
        /// </summary>
        /// <returns></returns>
        public DataView GetDataUserPager()
        {
            string tableName1 = "[TB_OMMP_MaintenanceTeamUser]";
            try
            {
                string sql = string.Format(@"select *
                                             from {0} ",
                                           tableName1);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }

        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <param name="orderBy">排序字段(MonitoringRegionUid,ReportDateTime)</param>
        /// <returns></returns>
        public DataView GetPMSharpDataPager(string[] SN, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            string tableName1 = "TB_QC_Report_PMSharp5030FlowCheckCali";
            string tableName2 = "TB_QC_Report_PMSharp5030TestValue";
            string tableName3 = "TB_WaterInspectionBase";
            try
            {
                string snStr = string.Empty;
                if (SN != null && SN.Length > 0 && SN[0].ToString() != "")
                    snStr = " AND [AnaDevSN] IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "')";
                string StrName = string.Empty;
                if (MissionName != "")
                    StrName = " AND MissionName ='" + MissionName + "'";
                string sql = string.Format(@"select TaskCode as '任务编号'
											,[AnaDevSN] as '仪器编号'
											,c.CollectionDt as '日期' 
											,c.FlowCaliSetValue as '仪器设定流量'
											,c.FlowCaliViewValue as '仪器显示流量'
											,c.FlowCaliRefValue as '参考标准读值'
											,c.FlowCaliInputValue as '输入数据'
											,FlowCheckIsPass as '检定结果'
											,[Description] as '备注'
                                            from {0} as a left join {1} as b 
											on a.TaskGuid=b.TaskGuid left join {2} as c 
											on a.[RowGuid]=c.QCReportGuid 
											where ActionDate >= '{3}' and FinishDate <= '{4}' {5} {6}",
                                           tableName1, tableName3, tableName2, dtBegin.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), snStr, StrName);
                return g_DatabaseHelper.ExecuteDataView(sql, connection1);
            }
            catch (Exception ex) { throw ex; }

        }

        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <param name="orderBy">排序字段(MonitoringRegionUid,ReportDateTime)</param>
        /// <returns></returns>
        public DataView GetPMTeomSharpDataPager(string[] SN, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            string tableName1 = "TB_QC_Report_PMTEOMRPFlowCheckCali";
            string tableName2 = "TB_WaterInspectionBase";
            string tableName3 = "TB_QC_Report_PMTEOMRPTestValue";
            try
            {
                string snStr = string.Empty;
                if (SN != null && SN.Length > 0 && SN[0].ToString() != "")
                    snStr = " AND [AnaDevSN] IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "')";
                string StrName = string.Empty;
                if (MissionName != "")
                    StrName = " AND MissionName ='" + MissionName + "'";
                string sql = string.Format(@"select TaskCode as '任务编号'
											,[AnaDevSN] as '仪器编号'
											,c.CollectionDt as '日期' 
											,max(CASE(c.FlowType) WHEN 1 THEN c.FlowCaliSetValue END ) AS '主流量设定值'
											,max(CASE(c.FlowType) WHEN 1 THEN c.[FlowCaliRefValue] END ) as '参考标准读数流量'
											,max(CASE(c.FlowType) WHEN 1 THEN c.[FlowCaliInputValue] END ) as '主输入读数'
											,max(CASE(c.FlowType) WHEN 2 THEN c.FlowCaliSetValue END ) AS '辅流量设定值'
											,max(CASE(c.FlowType) WHEN 2 THEN c.[FlowCaliRefValue] END ) as '参考标准读数'
											,max(CASE(c.FlowType) WHEN 2 THEN c.[FlowCaliInputValue] END ) as '辅输入读数'
											,[Description] as '备注'
                                            from {0} as a left join {1} as b 
											on a.TaskGuid=b.TaskGuid left join {2} as c 
											on a.[RowGuid]=c.QCReportGuid 
											where ActionDate >= '{3}' and FinishDate <= '{4}' {5} {6}
											group by TaskCode,[AnaDevSN],[CollectionDt],[Description]",
                                           tableName1, tableName2, tableName3, dtBegin.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), snStr, StrName);
                return g_DatabaseHelper.ExecuteDataView(sql, connection1);
            }
            catch (Exception ex) { throw ex; }

        }

        /// <summary>
        ///标准流量计检定核查记录
        /// </summary>
        /// <returns></returns>
        public DataView GetStdFlowMeterDataPager(string[] SN, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            string tableName1 = "TB_QC_Report_StdFlowMeterCheck";
            string tableName2 = "TB_WaterInspectionBase";
            string tableName3 = "TB_QC_Report_StdFlowMeterTestValue";
            try
            {
                string snStr = string.Empty;
                if (SN != null && SN.Length > 0 && SN[0].ToString() != "")
                    snStr = " AND FlowMeterSN IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "')";
                string StrName = string.Empty;
                if (MissionName != "")
                    StrName = " AND MissionName ='" + MissionName + "'";
                string sql = string.Format(@"select TaskCode as '任务编号'
											,FlowMeterSN as '仪器编号'
											,c.CollectionDt as '日期' 
											,c.RecordRangePerc as '量程百分比'
											,c.RecordTobeCaliFlowTestValue as '待校准流量设备读数'
											,c.RecordStdFlowTestValue as '标准流量计实测值'
											,c.RecordStdFlowQsValue as '标准流量计质量流量'
											,[Description] as '备注'
											from {0} as a left join {1} as b 
											on a.TaskGuid=b.TaskGuid left join {2} as c 
											on a.[RowGuid]=c.QCReportGuid 
											where ActionDate>='{3}' and FinishDate<='{4}' {5} {6}",
                                          tableName1, tableName2, tableName3, dtBegin.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), snStr, StrName);
                return g_DatabaseHelper.ExecuteDataView(sql, connection1);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        ///臭氧校准仪校准记录
        /// </summary>
        /// <returns></returns>
        public DataView GetO3HappenDevDataPager(string[] SN, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            string tableName1 = "TB_QC_Report_O3HappenDevCali";
            string tableName2 = "TB_WaterInspectionBase";
            string tableName3 = "TB_QC_Report_O3HappenDevCaliTestValue";
            try
            {
                string snStr = string.Empty;
                if (SN != null && SN.Length > 0 && SN[0].ToString() != "")
                    snStr = " AND CaliDevSN IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "')";
                string StrName = string.Empty;
                if (MissionName != "")
                    StrName = " AND MissionName ='" + MissionName + "'";
                string sql = string.Format(@"select TaskCode as '任务编号'
											,CaliDevSN as '仪器编号'
											,c.CollectionDt as '日期'
											,c.ConveyStdConc as '传递标准浓度'
											,c.AfterChangeConc as '转换后浓度'
											,c.TobeCaliConc as '被校仪器浓度'
											,c.RespError as '响应误差'
											,ResultSlope as '斜率'
											,ResultIntercept '截距'
											,ResulCoeff as '相关系数'
											,Description as '备注'
											from {0} as a left join {1} as b 
											on a.TaskGuid=b.TaskGuid left join {2} as c 
											on a.[RowGuid]=c.QCReportGuid 
											where ActionDate>='{3}' and FinishDate<='{4}' {5} {6}",
                                           tableName1, tableName2, tableName3, dtBegin.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), snStr, StrName);
                return g_DatabaseHelper.ExecuteDataView(sql, connection1);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        ///氮氧化物分析仪动态校准记录
        /// </summary>
        /// <returns></returns>
        public DataView GetNOxDevDataPager(string[] SN, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            string tableName1 = "TB_QC_Report_NOxDevCali";
            string tableName2 = "TB_WaterInspectionBase";
            string tableName3 = "TB_QC_Report_NOxDevLineAnalyseTestValue";
            try
            {
                string snStr = string.Empty;
                if (SN != null && SN.Length > 0 && SN[0].ToString() != "")
                    snStr = " AND ([NOxDevSN] IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "')"
                             + " OR [CaliDevSN] IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "'))";
                string StrName = string.Empty;
                if (MissionName != "")
                    StrName = " AND MissionName ='" + MissionName + "'";
                string sql = string.Format(@"select TaskCode as '任务编号'
											,CaliDevSN as '仪器编号'
											,c.CollectionDt as '日期'
                                            ,max(CASE(c.CaliFactorName) WHEN 'NO' THEN c.RefConc END ) AS 'NO实际浓度'
                                            ,max(CASE(c.CaliFactorName) WHEN 'NO' THEN c.RespConc END ) AS 'NO仪器响应'
                                            ,max(CASE(c.CaliFactorName) WHEN 'NOx' THEN c.RefConc END ) AS 'NOx实际浓度'
                                            ,max(CASE(c.CaliFactorName) WHEN 'NOx' THEN c.RespConc END ) AS 'NOx仪器响应'
											,ResultNOxSlope as '斜率'
											,ResultNOxIntercept as '截距'
											,ResultNOxCoeff as '相关系数'
											from {0} as a left join {1} as b 
											on a.TaskGuid=b.TaskGuid left join {2} as c 
											on a.[RowGuid]=c.QCReportGuid 
											where ActionDate>='{3}' and FinishDate<='{4}' {5} {6}
											group by TaskCode,CaliDevSN,CollectionDt,ResultNOxSlope,ResultNOxIntercept,ResultNOxCoeff",
                                           tableName1, tableName2, tableName3, dtBegin.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), snStr, StrName);
                return g_DatabaseHelper.ExecuteDataView(sql, connection1);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        ///动态校准仪流量（标准气/稀释气）检查记录表
        /// </summary>
        /// <returns></returns>
        public DataView GetCaliDevDataPager(string[] SN, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            string tableName1 = "TB_QC_Report_CaliDevFlowCheck";
            string tableName2 = "TB_WaterInspectionBase";
            string tableName3 = "TB_QC_Report_CaliDevFlowTestValue";
            try
            {
                string snStr = string.Empty;
                if (SN != null && SN.Length > 0 && SN[0].ToString() != "")
                    snStr = " AND [CaliDevSN] IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "')";
                string StrName = string.Empty;
                if (MissionName != "")
                    StrName = " AND MissionName ='" + MissionName + "'";
                string sql = string.Format(@"select TaskCode as '任务编号'
											,CaliDevSN as '仪器编号'
											,c.CollectionDt as '日期' 
											,FlowMeterRange as'量程'
											,c.SetValue as '设定值'
											,c.CaliDevViewValue as '仪器读数'
											,c.FlowMeterValue as '流量计读数'
											,c.FlowMeterValue as '流量计修正读数'
										    ,c.FlowMeterQsValue as '输入校准仪值'
											,ReslutSlope as '斜率'
											,ReslutIntercept '截距'
											,ReslutCoeff as '相关系数'
                                            from {0}  as a left join {1} as b 
											on a.TaskGuid=b.TaskGuid left join {2} as c 
											on  a.[RowGuid]=c.QCReportGuid 
											where ActionDate >= '{3}' and FinishDate <= '{4}' {5} {6}",
                                           tableName1, tableName2, tableName3, dtBegin.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), snStr, StrName);
                return g_DatabaseHelper.ExecuteDataView(sql, connection1);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        ///零气纯度检查记录
        /// </summary>
        /// <returns></returns>
        public DataView GetZeroGasDataPager(string[] SN, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            string tableName1 = "TB_QC_Report_ZeroGasPurity";
            string tableName2 = "TB_WaterInspectionBase";
            string tableName3 = "TB_QC_Report_ZeroGasPurityTestValue";
            try
            {
                string snStr = string.Empty;
                if (SN != null && SN.Length > 0 && SN[0].ToString() != "")
                    snStr = " AND ([AnaDevSN] IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "')"
                             + " OR [CaliDevSN] IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "'))";
                string StrName = string.Empty;
                if (MissionName != "")
                    StrName = " AND MissionName ='" + MissionName + "'";
                string sql = string.Format(@"select TaskCode as '任务编号'
											,AnaDevSN as '仪器编号'
											,FinishDate as '日期'
											,max(CASE(c.ResultType) WHEN 0 THEN c.ZeroRespConc1 END ) AS '当日响应浓度1'
											,max(CASE(c.ResultType) WHEN 0 THEN c.ZeroRespConc2 END ) AS '当日响应浓度2'
											,max(CASE(c.ResultType) WHEN 0 THEN c.ZeroRespConc3 END ) AS '当日响应浓度3'
											,max(CASE(c.ResultType) WHEN 0 THEN c.ZeroRespAvg END ) AS '当日响应平均值'
											,max(CASE(c.ResultType) WHEN 0 THEN c.ZeroRespDiff END ) AS '当日响应偏差'
											,max(CASE(c.ResultType) WHEN 1 THEN c.ZeroRespConc1 END ) AS '次日响应浓度1'
											,max(CASE(c.ResultType) WHEN 1 THEN c.ZeroRespConc2 END ) AS '次日响应浓度2'
											,max(CASE(c.ResultType) WHEN 1 THEN c.ZeroRespConc3 END ) AS '次日响应浓度3'
											,max(CASE(c.ResultType) WHEN 1 THEN c.ZeroRespAvg END ) AS '次日响应平均值'
											,max(CASE(c.ResultType) WHEN 1 THEN c.ZeroRespDiff END ) AS '次日响应偏差'		
                                            from {0}  as a left join {1} as b 
											on a.TaskGuid=b.TaskGuid left join {2} as c 
											on  a.[RowGuid]=c.QCReportGuid 
											where ActionDate >= '{3}' and FinishDate <= '{4}' {5} {6}
											group by TaskCode,AnaDevSN,FinishDate",
                                           tableName1, tableName2, tableName3, dtBegin.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), snStr, StrName);
                return g_DatabaseHelper.ExecuteDataView(sql, connection1);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        ///气体分析仪精密度检查记录
        /// </summary>
        /// <returns></returns>
        public DataView GetAnaDevPrecisionDataPager(string[] SN, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            string tableName1 = "TB_QC_Report_AnaDevPrecisionCheck";
            string tableName2 = "TB_WaterInspectionBase";
            string tableName3 = "TB_QC_Report_AnaDevPrecisionTestValue";
            try
            {
                string snStr = string.Empty;
                if (SN != null && SN.Length > 0 && SN[0].ToString() != "")
                    if (SN != null && SN.Length > 0 && SN[0].ToString() != "")
                        snStr = " AND ([AnaDevSO2SN] IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "')"
                            + " OR [AnaDevCOSN] IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "')"
                            + " OR [AnaDevO3SN] IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "')"
                            + " OR [AnaDevNOxSN] IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "')"
                            + " OR [CaliDevSN] IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "'))";
                string StrName = string.Empty;
                if (MissionName != "")
                    StrName = " AND MissionName ='" + MissionName + "'";
                string sql = string.Format(@"select TaskCode as '任务编号'
											,[CaliDevSN] as '仪器编号'
											,FinishDate as '日期'
											,c.CaliFactorName as '污染物'
											,c.RefConc as '参考浓度'
											,c.RespConc as '响应浓度'
											,c.PercErr as '百分比误差'
                                            ,c.O3PumpState as '状态'
                                            from {0}  as a left join {1} as b 
											on a.TaskGuid=b.TaskGuid left join {2} as c 
											on  a.[RowGuid]=c.QCReportGuid 
											where ActionDate >= '{3}' and FinishDate <= '{4}' {5}  {6}",
                                          tableName1, tableName2, tableName3, dtBegin.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), snStr, StrName);
                return g_DatabaseHelper.ExecuteDataView(sql, connection1);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        ///气体分析仪零点、跨度检查与调节记录
        /// </summary>
        /// <returns></returns>
        public DataView GetZeroAndSpanDataPager(string[] SN, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            string tableName1 = "TB_QC_Report_GasZeroAndSpanCheck";
            string tableName2 = "TB_WaterInspectionBase";
            try
            {
                string snStr = string.Empty;
                if (SN != null && SN.Length > 0 && SN[0].ToString() != "")
                    snStr = " AND ([AnaDevSO2SN] IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "')"
                        + " OR [AnaDevCOSN] IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "')"
                        + " OR [AnaDevO3SN] IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "')"
                        + " OR [AnaDevNOxSN] IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "')"
                        + " OR [CaliDevSN] IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "'))";
                string StrName = string.Empty;
                if (MissionName != "")
                    StrName = " AND MissionName ='" + MissionName + "'";
                string sql = string.Format(@"select TaskCode
											  ,CaliDevSN
											  ,FinishDate 
											  ,[SO2Zero]
											  ,[SO2ZeroModifyBefore]
											  ,[SO2ZeroModifyAfter]
											  ,[SO2Span]
											  ,[SO2SpanModifyBefore]
											  ,[SO2SpanModifyAfter]
											  ,[COZero]
											  ,[COZeroModifyBefore]
											  ,[COZeroModifyAfter]
											  ,[COSpan]
											  ,[COSpanModifyBefore]
											  ,[COSpanModifyAfter]
											  ,[O3Zero]
											  ,[O3ZeroModifyBefore]
											  ,[O3ZeroModifyAfter]
											  ,[O3Span]
											  ,[O3SpanModifyBefore]
											  ,[O3SpanModifyAfter]
											  ,[NOZero]
											  ,[NOZeroModifyBefore]
											  ,[NOZeroModifyAfter]
											  ,[NOSpan]
											  ,[NOSpanModifyBefore]
											  ,[NOSpanModifyAfter]
											  ,[NOXZero]
											  ,[NOXZeroModifyBefore]
											  ,[NOXZeroModifyAfter]
											  ,[NOXSpan]
											  ,[NOXSpanModifyBefore]
											  ,[NOXSpanModifyAfter]
											  ,[SO2SlopeModifyBefore]
											  ,[SO2SlopeModifyAfter]
											  ,[SO2InterceptModifyBefore]
											  ,[SO2InterceptModifyAfter]
											  ,[SO2GainModifyBefore]
											  ,[SO2GainModifyAfter]
											  ,[COSlopeModifyBefore]
											  ,[COSlopeModifyAfter]
											  ,[COInterceptModifyBefore]
											  ,[COInterceptModifyAfter]
											  ,[COGainModifyBefore]
											  ,[COGainModifyAfter]
											  ,[O3SlopeModifyBefore]
											  ,[O3SlopeModifyAfter]
											  ,[O3InterceptModifyBefore]
											  ,[O3InterceptModifyAfter]
											  ,[O3GainModifyBefore]
											  ,[O3GainModifyAfter]
											  ,[NOSlopeModifyBefore]
											  ,[NOSlopeModifyAfter]
											  ,[NOInterceptModifyBefore]
											  ,[NOInterceptModifyAfter]
											  ,[NOGainModifyBefore]
											  ,[NOGainModifyAfter]
											  ,[NOXSlopeModifyBefore]
											  ,[NOXSlopeModifyAfter]
											  ,[NOXInterceptModifyBefore]
											  ,[NOXInterceptModifyAfter]
											  ,[NOXGainModifyBefore]
											  ,[NOXGainModifyAfter]
											  ,Memo 
                                              from {0}  as a left join {1} as b 
											  on a.TaskGuid=b.TaskGuid 
											  where ActionDate >= '{2}' and FinishDate <= '{3}' {4} {5}",
                                          tableName1, tableName2, dtBegin.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), snStr, StrName);
                return g_DatabaseHelper.ExecuteDataView(sql, connection1);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        ///气体分析仪零漂、标漂检查记录
        /// </summary>
        /// <returns></returns>
        public DataView GetAnaDevDriftDataPager(string[] SN, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            string tableName1 = "TB_QC_Report_AnaDevDriftCheck";
            string tableName2 = "TB_WaterInspectionBase";
            try
            {
                string snStr = string.Empty;
                if (SN != null && SN.Length > 0 && SN[0].ToString() != "")
                    snStr = " AND ([AnaDevSN] IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "')"
                             + " OR [CaliDevSN] IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "'))";
                string StrName = string.Empty;
                if (MissionName != "")
                    StrName = " AND MissionName ='" + MissionName + "'";
                string sql = string.Format(@"select TaskCode as '任务编号'
											,AnaDevSN as '仪器编号'
											,LastCaliDt as '日期' 
											,Zero24HStartResp as '零气初始响应'
											,Zero24HEndResp as '零气后响应'
											,Zero24HDrift as '零气漂移'
											,Span24HStartResp as '跨度初始响应'
											,Span24HEndResp as '跨度后响应'
											,Span24HDrift as '跨度漂移'
											from {0} as a left join {1} as b 
											on a.TaskGuid=b.TaskGuid 
											where ActionDate>='{2}' and FinishDate<='{3}' {4} {5}",
                                           tableName1, tableName2, dtBegin.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), snStr, StrName);
                return g_DatabaseHelper.ExecuteDataView(sql, connection1);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        ///气体分析仪准确度审核记录
        /// </summary>
        /// <returns></returns>
        public DataView GetAnaDevDataPager(string[] SN, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            string tableName1 = "TB_QC_Report_AnaDevAccuracyCheck";
            string tableName2 = "TB_WaterInspectionBase";
            string tableName3 = "TB_QC_Report_AnaDevAccuracyTestValue";
            try
            {
                string snStr = string.Empty;
                if (SN != null && SN.Length > 0 && SN[0].ToString() != "")
                    snStr = " AND ([AnaDevSN] IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "')"
                             + " OR [CaliDevSN] IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "'))";
                string StrName = string.Empty;
                if (MissionName != "")
                    StrName = " AND MissionName ='" + MissionName + "'";
                string sql = string.Format(@"select TaskCode as '任务编号'
											,AnaDevSN as '仪器编号'
											,c.CollectionDt as '日期'
											,c.RefConc as '测试气体浓度'
											,c.RespConc as '仪器响应浓度'
											,c.RespError as '响应误差'
											,ResultAvgError as '平均误差'
											,ResultStdDiff as '标准偏差'
											,ResultSlope as '斜率'
											,ResultIntercept '截距'
											,ResultCoeff as '相关系数'
											from {0} as a left join {1} as b 
											on a.TaskGuid=b.TaskGuid left join {2} as c 
											on a.[RowGuid]=c.QCReportGuid 
											where ActionDate>='{3}' and FinishDate<='{4}' {5} {6}",
                                           tableName1, tableName2, tableName3, dtBegin.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), snStr, StrName);
                return g_DatabaseHelper.ExecuteDataView(sql, connection1);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        ///多点线性校准记录
        /// </summary>
        /// <returns></returns>
        public DataView GetMultiPointDataPager(string[] SN, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            string tableName1 = "TB_QC_Report_MultiPointCali";
            string tableName2 = "TB_QC_Report_MultiPointCaliTestValue";
            string tableName3 = "TB_WaterInspectionBase";
            try
            {
                string snStr = string.Empty;
                if (SN != null && SN.Length > 0 && SN[0].ToString() != "")
                    snStr = " AND (b.AnaDevSN IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "')"
                         + " OR [CaliDevSN] IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "'))";
                string StrName = string.Empty;
                if (MissionName != "")
                    StrName = " AND MissionName ='" + MissionName + "'";
                string sql = string.Format(@"SELECT TaskCode as '任务编号'
                                            ,b. AnaDevSN as '仪器编号'
											,c.CollectionDt as '日期'
											,c.RefConc as '测试气体浓度'
											,c.RespConc as '仪器响应浓度'
											,c.RespError as '响应误差'
											,ResultAvgError as '平均误差'
											,ResultStdDiff as '标准偏差'
											,ResultSlope as '斜率'
											,ResultIntercept '截距'
											,ResultCoeff as '相关系数'
											,Description as '备注'
										  FROM {0} as a   left join {1} as b
										  on a.TaskGuid=b.TaskGuid left join {2} as c
										  on c.QCReportGuid=b.[RowGuid]
										  where [ActionDate]>='{3}'  and [FinishDate]<='{4}' {5} {6}
                                          order by TaskCode",
                                         tableName3, tableName1, tableName2, dtBegin.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), snStr, StrName);
                return g_DatabaseHelper.ExecuteDataView(sql, connection1);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <param name="orderBy">排序字段(MonitoringRegionUid,ReportDateTime)</param>
        /// <returns></returns>
        public DataView GetDataAreaPager()
        {
            string tableName1 = "TB_OMMP_MaintenanceArea";
            try
            {
                string sql = string.Format(@"select *
                                             from {0}
                                             order by SortNumber desc",
                                           tableName1);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }

        }
        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <param name="orderBy">排序字段(MonitoringRegionUid,ReportDateTime)</param>
        /// <returns></returns>
        public DataView GetDataPointPager()
        {
            string tableName1 = "TB_OMMP_MaintenanceObject";
            try
            {
                string sql = string.Format(@"select *
                                             from {0} 
                                             where AreaGuid is not null and TypeGuid='airaaira-aira-aira-aira-airaairaaira'",
                                           tableName1);
                return g_DatabaseHelper.ExecuteDataView(sql, connection);
            }
            catch (Exception ex) { throw ex; }

        }
        #endregion
        #region new
        public DataView GetPMSharpDataNewPager(string[] SN, string[] pointIds, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            string tableName1 = "TB_QC_Report_PMSharp5030FlowCheckCali";
            string tableName2 = "TB_QC_Report_PMSharp5030TestValue";
            string tableName3 = "TB_WaterInspectionBase";
            try
            {
                string snStr = string.Empty;
                if (SN != null && SN.Length > 0 && SN[0].ToString() != "")
                    snStr = " AND [AnaDevSN] IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "')";
                string pointStr = string.Empty;
                if (pointIds != null && pointIds.Length > 0 && pointIds[0].ToString() != "")
                    pointStr = " AND PointName IN ('" + StringExtensions.GetArrayStrNoEmpty(pointIds.ToList(), "','") + "')";
                string StrName = string.Empty;
                if (MissionName != "")
                    StrName = " AND MissionName ='" + MissionName + "'";
                string sql = string.Format(@"select PointName as '测点'
											,c.CollectionDt as '时间' 
											,c.FlowCaliSetValue as '仪器设定流量'
											,c.FlowCaliViewValue as '仪器显示流量'
											,c.FlowCaliRefValue as '参考标准读值'
											,c.FlowCaliInputValue as '输入数据'
											,FlowCheckIsPass as '检定结果'
											,AnaDevModel+'/'+AnaDevSN as '仪器编号'
                                            ,TaskCode as '任务编号'
											,ActionUserName as '执行人'
                                            from {0} as a left join {1} as b 
											on a.TaskGuid=b.TaskGuid left join {2} as c 
											on a.[RowGuid]=c.QCReportGuid 
											where ActionDate >= '{3}' and FinishDate <= '{4}' {5} {6} {7} AND b.TaskGuid is not null",
                                           tableName1, tableName3, tableName2, dtBegin.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), pointStr, snStr, StrName);
                return g_DatabaseHelper.ExecuteDataView(sql, connection1);
            }
            catch (Exception ex) { throw ex; }

        }

        /// <summary>
        /// 取得虚拟分页查询数据和总行数(行转列数据)
        /// </summary>
        /// <param name="regionGuids">区域</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">截止时间</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageNo">当前页(从0开始)</param>
        /// <param name="recordTotal">总行数</param>
        /// <param name="regionAQIStatisticalType">区域AQI统计类型（常规、创模点）</param>
        /// <param name="orderBy">排序字段(MonitoringRegionUid,ReportDateTime)</param>
        /// <returns></returns>
        public DataView GetPMTeomSharpDataNewPager(string[] SN, string[] pointIds, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            string tableName1 = "TB_QC_Report_PMTEOMRPFlowCheckCali";
            string tableName2 = "TB_WaterInspectionBase";
            string tableName3 = "TB_QC_Report_PMTEOMRPTestValue";
            try
            {
                string snStr = string.Empty;
                if (SN != null && SN.Length > 0 && SN[0].ToString() != "")
                    snStr = " AND [AnaDevSN] IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "')";
                string pointStr = string.Empty;
                if (pointIds != null && pointIds.Length > 0 && pointIds[0].ToString() != "")
                    pointStr = " AND PointName IN ('" + StringExtensions.GetArrayStrNoEmpty(pointIds.ToList(), "','") + "')";
                string StrName = string.Empty;
                if (MissionName != "")
                    StrName = " AND MissionName ='" + MissionName + "'";
                string sql = string.Format(@"select PointName as '测点'
											,c.CollectionDt as '时间' 
											,max(CASE(c.FlowType) WHEN 1 THEN c.FlowCaliSetValue END ) AS '主流量设定值'
											,max(CASE(c.FlowType) WHEN 1 THEN c.[FlowCaliRefValue] END ) as '参考标准读数流量'
											,max(CASE(c.FlowType) WHEN 1 THEN c.[FlowCaliInputValue] END ) as '主输入读数'
											,max(CASE(c.FlowType) WHEN 2 THEN c.FlowCaliSetValue END ) AS '辅流量设定值'
											,max(CASE(c.FlowType) WHEN 2 THEN c.[FlowCaliRefValue] END ) as '参考标准读数'
											,max(CASE(c.FlowType) WHEN 2 THEN c.[FlowCaliInputValue] END ) as '辅输入读数'
                                            ,AnaDevModel+'/'+AnaDevSN as '仪器编号'
                                            ,TaskCode as '任务编号'
											,ActionUserName as '执行人'
                                            from {0} as a left join {1} as b 
											on a.TaskGuid=b.TaskGuid left join {2} as c 
											on a.[RowGuid]=c.QCReportGuid 
											where ActionDate >= '{3}' and FinishDate <= '{4}' {5} {6} {7} AND b.TaskGuid is not null
											group by TaskCode,PointName,AnaDevModel,AnaDevSN,CollectionDt,ActionUserName",
                                           tableName1, tableName2, tableName3, dtBegin.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), pointStr, snStr, StrName);
                return g_DatabaseHelper.ExecuteDataView(sql, connection1);
            }
            catch (Exception ex) { throw ex; }

        }

        /// <summary>
        ///标准流量计检定核查记录
        /// </summary>
        /// <returns></returns>
        public DataView GetStdFlowMeterDataNewPager(string[] SN, string[] pointIds, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            string tableName1 = "TB_QC_Report_StdFlowMeterCheck";
            string tableName2 = "TB_WaterInspectionBase";
            string tableName3 = "TB_QC_Report_StdFlowMeterTestValue";
            try
            {
                string snStr = string.Empty;
                if (SN != null && SN.Length > 0 && SN[0].ToString() != "")
                    snStr = " AND FlowMeterSN IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "')";
                string pointStr = string.Empty;
                if (pointIds != null && pointIds.Length > 0 && pointIds[0].ToString() != "")
                    pointStr = " AND PointName IN ('" + StringExtensions.GetArrayStrNoEmpty(pointIds.ToList(), "','") + "')";
                string StrName = string.Empty;
                if (MissionName != "")
                    StrName = " AND MissionName ='" + MissionName + "'";
                string sql = string.Format(@"select PointName as '测点'
											,c.CollectionDt as '时间' 
											,c.RecordRangePerc as '量程百分比'
											,c.RecordTobeCaliFlowTestValue as '待校准流量设备读数'
											,c.RecordStdFlowTestValue as '标准流量计实测值'
											,c.RecordStdFlowQsValue as '标准流量计质量流量'
											,FlowMeterModel+'/'+FlowMeterSN as '仪器编号'
                                            ,TaskCode as '任务编号'
											,ActionUserName as '执行人'
											from {0} as a left join {1} as b 
											on a.TaskGuid=b.TaskGuid left join {2} as c 
											on a.[RowGuid]=c.QCReportGuid 
											where ActionDate>='{3}' and FinishDate<='{4}' {5} {6} {7} AND b.TaskGuid is not null",
                                          tableName1, tableName2, tableName3, dtBegin.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), pointStr, snStr, StrName);
                return g_DatabaseHelper.ExecuteDataView(sql, connection1);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        ///臭氧校准仪校准记录
        /// </summary>
        /// <returns></returns>
        public DataView GetO3HappenDevDataNewPager(string[] SN, string[] pointIds, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            string tableName1 = "TB_QC_Report_O3HappenDevCali";
            string tableName2 = "TB_WaterInspectionBase";
            string tableName3 = "TB_QC_Report_O3HappenDevCaliTestValue";
            try
            {
                string snStr = string.Empty;
                if (SN != null && SN.Length > 0 && SN[0].ToString() != "")
                    snStr = " AND CaliDevSN IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "')";
                string pointStr = string.Empty;
                if (pointIds != null && pointIds.Length > 0 && pointIds[0].ToString() != "")
                    pointStr = " AND PointName IN ('" + StringExtensions.GetArrayStrNoEmpty(pointIds.ToList(), "','") + "')";
                string StrName = string.Empty;
                if (MissionName != "")
                    StrName = " AND MissionName ='" + MissionName + "'";
                string sql = string.Format(@"select PointName as '测点'
											,c.CollectionDt as '时间'
											,c.ConveyStdConc as '传递标准浓度'
											,c.AfterChangeConc as '转换后浓度'
											,c.TobeCaliConc as '被校仪器浓度'
											,c.RespError as '响应误差'
											,CaliDevModel+'/'+CaliDevSN as '仪器编号'
                                            ,TaskCode as '任务编号'
                                            ,ActionUserName as '执行人'
											from {0} as a left join {1} as b 
											on a.TaskGuid=b.TaskGuid left join {2} as c 
											on a.[RowGuid]=c.QCReportGuid 
											where ActionDate>='{3}' and FinishDate<='{4}' {5} {6} {7} AND b.TaskGuid is not null",
                                           tableName1, tableName2, tableName3, dtBegin.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), pointStr, snStr, StrName);
                return g_DatabaseHelper.ExecuteDataView(sql, connection1);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        ///氮氧化物分析仪动态校准记录
        /// </summary>
        /// <returns></returns>
        public DataView GetNOxDevDataNewPager(string[] SN, string[] pointIds, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            string tableName1 = "TB_QC_Report_NOxDevCali";
            string tableName2 = "TB_WaterInspectionBase";
            string tableName3 = "TB_QC_Report_NOxDevLineAnalyseTestValue";
            try
            {
                string snStr = string.Empty;
                if (SN != null && SN.Length > 0 && SN[0].ToString() != "")
                    snStr = " AND ([NOxDevSN] IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "')"
                             + " OR [CaliDevSN] IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "'))";
                string pointStr = string.Empty;
                if (pointIds != null && pointIds.Length > 0 && pointIds[0].ToString() != "")
                    pointStr = " AND PointName IN ('" + StringExtensions.GetArrayStrNoEmpty(pointIds.ToList(), "','") + "')";
                string StrName = string.Empty;
                if (MissionName != "")
                    StrName = " AND MissionName ='" + MissionName + "'";
                string sql = string.Format(@"select PointName as '测点'
											,c.CollectionDt as '时间'
                                            ,max(CASE(c.CaliFactorName) WHEN 'NO' THEN c.RefConc END ) AS 'NO实际浓度'
                                            ,max(CASE(c.CaliFactorName) WHEN 'NO' THEN c.RespConc END ) AS 'NO仪器响应'
                                            ,max(CASE(c.CaliFactorName) WHEN 'NOx' THEN c.RefConc END ) AS 'NOx实际浓度'
                                            ,max(CASE(c.CaliFactorName) WHEN 'NOx' THEN c.RespConc END ) AS 'NOx仪器响应'
											,CaliDevModel+'/'+CaliDevSN as '仪器编号'
                                            ,TaskCode as '任务编号'
                                            ,ActionUserName as '执行人'
											from {0} as a left join {1} as b 
											on a.TaskGuid=b.TaskGuid left join {2} as c 
											on a.[RowGuid]=c.QCReportGuid 
											where ActionDate>='{3}' and FinishDate<='{4}' {5} {6} {7} AND b.TaskGuid is not null
											group by TaskCode,PointName,CaliDevModel,CaliDevSN,CollectionDt,ActionUserName",
                                           tableName1, tableName2, tableName3, dtBegin.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), pointStr, snStr, StrName);
                return g_DatabaseHelper.ExecuteDataView(sql, connection1);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        ///动态校准仪流量（标准气/稀释气）检查记录表
        /// </summary>
        /// <returns></returns>
        public DataView GetCaliDevDataNewPager(string[] SN, string[] pointIds, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            string tableName1 = "TB_QC_Report_CaliDevFlowCheck";
            string tableName2 = "TB_WaterInspectionBase";
            string tableName3 = "TB_QC_Report_CaliDevFlowTestValue";
            try
            {
                string snStr = string.Empty;
                if (SN != null && SN.Length > 0 && SN[0].ToString() != "")
                    snStr = " AND [CaliDevSN] IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "')";
                string pointStr = string.Empty;
                if (pointIds != null && pointIds.Length > 0 && pointIds[0].ToString() != "")
                    pointStr = " AND PointName IN ('" + StringExtensions.GetArrayStrNoEmpty(pointIds.ToList(), "','") + "')";
                string StrName = string.Empty;
                if (MissionName != "")
                    StrName = " AND MissionName ='" + MissionName + "'";
                string sql = string.Format(@"select PointName as '测点'
											,c.CollectionDt as '时间' 
											,FlowMeterRange as'量程'
											,c.SetValue as '设定值'
											,c.CaliDevViewValue as '仪器读数'
											,c.FlowMeterValue as '流量计读数'
											,c.FlowMeterValue as '流量计修正读数'
										    ,c.FlowMeterQsValue as '输入校准仪值'
											,CaliDevModel+'/'+CaliDevSN as '仪器编号'
                                            ,TaskCode as '任务编号'
                                            ,ActionUserName as '执行人'
                                            from {0}  as a left join {1} as b 
											on a.TaskGuid=b.TaskGuid left join {2} as c 
											on  a.[RowGuid]=c.QCReportGuid 
											where ActionDate >= '{3}' and FinishDate <= '{4}' {5} {6} {7} AND b.TaskGuid is not null",
                                           tableName1, tableName2, tableName3, dtBegin.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), pointStr, snStr, StrName);
                return g_DatabaseHelper.ExecuteDataView(sql, connection1);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        ///零气纯度检查记录
        /// </summary>
        /// <returns></returns>
        public DataView GetZeroGasDataNewPager(string[] SN, string[] pointIds, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            string tableName1 = "TB_QC_Report_ZeroGasPurity";
            string tableName2 = "TB_WaterInspectionBase";
            string tableName3 = "TB_QC_Report_ZeroGasPurityTestValue";
            try
            {
                string snStr = string.Empty;
                if (SN != null && SN.Length > 0 && SN[0].ToString() != "")
                    snStr = " AND ([AnaDevSN] IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "')"
                             + " OR [CaliDevSN] IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "'))";
                string pointStr = string.Empty;
                if (pointIds != null && pointIds.Length > 0 && pointIds[0].ToString() != "")
                    pointStr = " AND PointName IN ('" + StringExtensions.GetArrayStrNoEmpty(pointIds.ToList(), "','") + "')";
                string StrName = string.Empty;
                if (MissionName != "")
                    StrName = " AND MissionName ='" + MissionName + "'";
                string sql = string.Format(@"select PointName as '测点'
											,FinishDate as '时间'
											,max(CASE(c.ResultType) WHEN 0 THEN c.ZeroRespConc1 END ) AS '当日响应浓度1'
											,max(CASE(c.ResultType) WHEN 0 THEN c.ZeroRespConc2 END ) AS '当日响应浓度2'
											,max(CASE(c.ResultType) WHEN 0 THEN c.ZeroRespConc3 END ) AS '当日响应浓度3'
											,max(CASE(c.ResultType) WHEN 0 THEN c.ZeroRespAvg END ) AS '当日响应平均值'
											,max(CASE(c.ResultType) WHEN 0 THEN c.ZeroRespDiff END ) AS '当日响应偏差'
											,max(CASE(c.ResultType) WHEN 1 THEN c.ZeroRespConc1 END ) AS '次日响应浓度1'
											,max(CASE(c.ResultType) WHEN 1 THEN c.ZeroRespConc2 END ) AS '次日响应浓度2'
											,max(CASE(c.ResultType) WHEN 1 THEN c.ZeroRespConc3 END ) AS '次日响应浓度3'
											,max(CASE(c.ResultType) WHEN 1 THEN c.ZeroRespAvg END ) AS '次日响应平均值'
											,max(CASE(c.ResultType) WHEN 1 THEN c.ZeroRespDiff END ) AS '次日响应偏差'
											,AnaDevModel+'/'+AnaDevSN as '仪器编号'
                                            ,TaskCode as '任务编号'
                                            ,ActionUserName as '执行人'		
                                            from {0}  as a left join {1} as b 
											on a.TaskGuid=b.TaskGuid left join {2} as c 
											on  a.[RowGuid]=c.QCReportGuid 
											where ActionDate >= '{3}' and FinishDate <= '{4}' {5} {6} {7} AND b.TaskGuid is not null
											group by TaskCode,PointName,AnaDevModel,AnaDevSN,FinishDate,ActionUserName",
                                           tableName1, tableName2, tableName3, dtBegin.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), pointStr, snStr, StrName);
                return g_DatabaseHelper.ExecuteDataView(sql, connection1);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        ///气体分析仪精密度检查记录
        /// </summary>
        /// <returns></returns>
        public DataView GetAnaDevPrecisionDataNewPager(string[] SN, string[] pointIds, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            string tableName1 = "TB_QC_Report_AnaDevPrecisionCheck";
            string tableName2 = "TB_WaterInspectionBase";
            string tableName3 = "TB_QC_Report_AnaDevPrecisionTestValue";
            try
            {
                string snStr = string.Empty;
                if (SN != null && SN.Length > 0 && SN[0].ToString() != "")
                    if (SN != null && SN.Length > 0 && SN[0].ToString() != "")
                        snStr = " AND ([AnaDevSO2SN] IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "')"
                            + " OR [AnaDevCOSN] IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "')"
                            + " OR [AnaDevO3SN] IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "')"
                            + " OR [AnaDevNOxSN] IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "')"
                            + " OR [CaliDevSN] IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "'))";
                string pointStr = string.Empty;
                if (pointIds != null && pointIds.Length > 0 && pointIds[0].ToString() != "")
                    pointStr = " AND PointName IN ('" + StringExtensions.GetArrayStrNoEmpty(pointIds.ToList(), "','") + "')";
                string StrName = string.Empty;
                if (MissionName != "")
                    StrName = " AND MissionName ='" + MissionName + "'";
                string sql = string.Format(@"select PointName as '测点'
											,FinishDate as '时间'
											,c.CaliFactorName as '污染物'
											,c.RefConc as '参考浓度'
											,c.RespConc as '响应浓度'
											,c.PercErr as '百分比误差'
                                            ,c.O3PumpState as '状态'
											,CaliDevModel+'/'+CaliDevSN as '仪器编号'
                                            ,TaskCode as '任务编号'
                                            ,ActionUserName as '执行人'
                                            from {0}  as a left join {1} as b 
											on a.TaskGuid=b.TaskGuid left join {2} as c 
											on  a.[RowGuid]=c.QCReportGuid 
											where ActionDate >= '{3}' and FinishDate <= '{4}' {5} {6} {7} AND b.TaskGuid is not null",
                                          tableName1, tableName2, tableName3, dtBegin.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), pointStr, snStr, StrName);
                return g_DatabaseHelper.ExecuteDataView(sql, connection1);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        ///气体分析仪零点、跨度检查与调节记录
        /// </summary>
        /// <returns></returns>
        public DataView GetZeroAndSpanDataNewPager(string[] SN, string[] pointIds, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            string tableName1 = "TB_QC_Report_GasZeroAndSpanCheck";
            string tableName2 = "TB_WaterInspectionBase";
            try
            {
                string snStr = string.Empty;
                if (SN != null && SN.Length > 0 && SN[0].ToString() != "")
                    snStr = " AND ([AnaDevSO2SN] IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "')"
                        + " OR [AnaDevCOSN] IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "')"
                        + " OR [AnaDevO3SN] IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "')"
                        + " OR [AnaDevNOxSN] IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "')"
                        + " OR [CaliDevSN] IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "'))";
                string pointStr = string.Empty;
                if (pointIds != null && pointIds.Length > 0 && pointIds[0].ToString() != "")
                    pointStr = " AND PointName IN ('" + StringExtensions.GetArrayStrNoEmpty(pointIds.ToList(), "','") + "')";
                string StrName = string.Empty;
                if (MissionName != "")
                    StrName = " AND MissionName ='" + MissionName + "'";
                string sql = string.Format(@"select TaskCode
                                              ,PointName
											  ,CaliDevModel+'/'+CaliDevSN as CaliDevSN
											  ,FinishDate 
											  ,[SO2Zero]
											  ,[SO2ZeroModifyBefore]
											  ,[SO2ZeroModifyAfter]
											  ,[SO2Span]
											  ,[SO2SpanModifyBefore]
											  ,[SO2SpanModifyAfter]
											  ,[COZero]
											  ,[COZeroModifyBefore]
											  ,[COZeroModifyAfter]
											  ,[COSpan]
											  ,[COSpanModifyBefore]
											  ,[COSpanModifyAfter]
											  ,[O3Zero]
											  ,[O3ZeroModifyBefore]
											  ,[O3ZeroModifyAfter]
											  ,[O3Span]
											  ,[O3SpanModifyBefore]
											  ,[O3SpanModifyAfter]
											  ,[NOZero]
											  ,[NOZeroModifyBefore]
											  ,[NOZeroModifyAfter]
											  ,[NOSpan]
											  ,[NOSpanModifyBefore]
											  ,[NOSpanModifyAfter]
											  ,[NOXZero]
											  ,[NOXZeroModifyBefore]
											  ,[NOXZeroModifyAfter]
											  ,[NOXSpan]
											  ,[NOXSpanModifyBefore]
											  ,[NOXSpanModifyAfter]
											  ,[SO2SlopeModifyBefore]
											  ,[SO2SlopeModifyAfter]
											  ,[SO2InterceptModifyBefore]
											  ,[SO2InterceptModifyAfter]
											  ,[SO2GainModifyBefore]
											  ,[SO2GainModifyAfter]
											  ,[COSlopeModifyBefore]
											  ,[COSlopeModifyAfter]
											  ,[COInterceptModifyBefore]
											  ,[COInterceptModifyAfter]
											  ,[COGainModifyBefore]
											  ,[COGainModifyAfter]
											  ,[O3SlopeModifyBefore]
											  ,[O3SlopeModifyAfter]
											  ,[O3InterceptModifyBefore]
											  ,[O3InterceptModifyAfter]
											  ,[O3GainModifyBefore]
											  ,[O3GainModifyAfter]
											  ,[NOSlopeModifyBefore]
											  ,[NOSlopeModifyAfter]
											  ,[NOInterceptModifyBefore]
											  ,[NOInterceptModifyAfter]
											  ,[NOGainModifyBefore]
											  ,[NOGainModifyAfter]
											  ,[NOXSlopeModifyBefore]
											  ,[NOXSlopeModifyAfter]
											  ,[NOXInterceptModifyBefore]
											  ,[NOXInterceptModifyAfter]
											  ,[NOXGainModifyBefore]
											  ,[NOXGainModifyAfter]
                                              ,ActionUserName
                                              from {0}  as a left join {1} as b 
											  on a.TaskGuid=b.TaskGuid 
											  where ActionDate >= '{2}' and FinishDate <= '{3}' {4} {5} {6} AND b.TaskGuid is not null",
                                          tableName1, tableName2, dtBegin.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), pointStr, snStr, StrName);
                return g_DatabaseHelper.ExecuteDataView(sql, connection1);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        ///气体分析仪零漂、标漂检查记录
        /// </summary>
        /// <returns></returns>
        public DataView GetAnaDevDriftDataNewPager(string[] SN, string[] pointIds, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            string tableName1 = "TB_QC_Report_AnaDevDriftCheck";
            string tableName2 = "TB_WaterInspectionBase";
            try
            {
                string snStr = string.Empty;
                if (SN != null && SN.Length > 0 && SN[0].ToString() != "")
                    snStr = " AND ([AnaDevSN] IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "')"
                             + " OR [CaliDevSN] IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "'))";
                string pointStr = string.Empty;
                if (pointIds != null && pointIds.Length > 0 && pointIds[0].ToString() != "")
                    pointStr = " AND PointName IN ('" + StringExtensions.GetArrayStrNoEmpty(pointIds.ToList(), "','") + "')";
                string StrName = string.Empty;
                if (MissionName != "")
                    StrName = " AND MissionName ='" + MissionName + "'";
                string sql = string.Format(@"select PointName as '测点'
											,LastCaliDt as '时间' 
											,Zero24HStartResp as '零气初始响应'
											,Zero24HEndResp as '零气后响应'
											,Zero24HDrift as '零气漂移'
											,Span24HStartResp as '跨度初始响应'
											,Span24HEndResp as '跨度后响应'
											,Span24HDrift as '跨度漂移'
											,AnaDevModel+'/'+AnaDevSN as '仪器编号'
                                            ,TaskCode as '任务编号'
                                            ,ActionUserName as '执行人'
											from {0} as a left join {1} as b 
											on a.TaskGuid=b.TaskGuid 
											where ActionDate>='{2}' and FinishDate<='{3}' {4} {5} {6} AND b.TaskGuid is not null",
                                           tableName1, tableName2, dtBegin.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), pointStr, snStr, StrName);
                return g_DatabaseHelper.ExecuteDataView(sql, connection1);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        ///气体分析仪准确度审核记录
        /// </summary>
        /// <returns></returns>
        public DataView GetAnaDevDataNewPager(string[] SN, string[] pointIds, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            string tableName1 = "TB_QC_Report_AnaDevAccuracyCheck";
            string tableName2 = "TB_WaterInspectionBase";
            string tableName3 = "TB_QC_Report_AnaDevAccuracyTestValue";
            try
            {
                string snStr = string.Empty;
                if (SN != null && SN.Length > 0 && SN[0].ToString() != "")
                    snStr = " AND ([AnaDevSN] IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "')"
                             + " OR [CaliDevSN] IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "'))";
                string pointStr = string.Empty;
                if (pointIds != null && pointIds.Length > 0 && pointIds[0].ToString() != "")
                    pointStr = " AND PointName IN ('" + StringExtensions.GetArrayStrNoEmpty(pointIds.ToList(), "','") + "')";
                string StrName = string.Empty;
                if (MissionName != "")
                    StrName = " AND MissionName ='" + MissionName + "'";
                string sql = string.Format(@"select PointName as '测点'
											,c.CollectionDt as '时间'
											,c.RefConc as '测试气体浓度'
											,c.RespConc as '分析仪响应'
											,c.RespError as '响应误差'
											,AnaDevModel+'/'+AnaDevSN as '仪器编号'
                                            ,TaskCode as '任务编号'
                                            ,ActionUserName as '执行人'
											from {0} as a left join {1} as b 
											on a.TaskGuid=b.TaskGuid left join {2} as c 
											on a.[RowGuid]=c.QCReportGuid 
											where ActionDate>='{3}' and FinishDate<='{4}' {5} {6} {7} AND b.TaskGuid is not null",
                                           tableName1, tableName2, tableName3, dtBegin.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), pointStr, snStr, StrName);
                return g_DatabaseHelper.ExecuteDataView(sql, connection1);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        ///多点线性校准记录
        /// </summary>
        /// <returns></returns>
        public DataView GetMultiPointDataNewPager(string[] SN, string[] pointIds, DateTime dtBegin, DateTime dtEnd, string MissionName)
        {
            string tableName1 = "TB_QC_Report_MultiPointCali";
            string tableName2 = "TB_QC_Report_MultiPointCaliTestValue";
            string tableName3 = "TB_WaterInspectionBase";
            try
            {
                string snStr = string.Empty;
                if (SN != null && SN.Length > 0 && SN[0].ToString() != "")
                    snStr = " AND (b.AnaDevSN IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "')"
                         + " OR [CaliDevSN] IN ('" + StringExtensions.GetArrayStrNoEmpty(SN.ToList(), "','") + "'))";
                string pointStr = string.Empty;
                if (pointIds != null && pointIds.Length > 0 && pointIds[0].ToString() != "")
                    pointStr = " AND PointName IN ('" + StringExtensions.GetArrayStrNoEmpty(pointIds.ToList(), "','") + "')";
                string StrName = string.Empty;
                if (MissionName != "")
                    StrName = " AND MissionName ='" + MissionName + "'";
                string sql = string.Format(@"SELECT PointName as '测点'
											,c.CollectionDt as '时间'
											,c.RefConc as '测试气体浓度'
											,c.RespConc as '分析仪响应'
											,c.RespError as '响应误差'
											,AnaDevModel+'/'+AnaDevSN as '仪器编号'
                                            ,TaskCode as '任务编号'
                                            ,ActionUserName as '执行人'
										  FROM {0} as a   left join {1} as b
										  on a.TaskGuid=b.TaskGuid left join {2} as c
										  on c.QCReportGuid=b.[RowGuid]
										  where [ActionDate]>='{3}'  and [FinishDate]<='{4}' {5} {6} {7} AND b.TaskGuid is not null
                                          order by TaskCode",
                                         tableName3, tableName1, tableName2, dtBegin.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), pointStr, snStr, StrName);
                return g_DatabaseHelper.ExecuteDataView(sql, connection1);
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion
    }
}
