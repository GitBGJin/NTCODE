using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartEP.Utilities.AdoData;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using System.Data;
using SmartEP.Data.Enums;

namespace SmartEP.Data.SqlServer.MonitoringBusiness
{
    /// <summary>
    /// 名称：CalibrationReportDAL.cs
    /// 创建人：徐龙超
    /// 创建日期：2015-11-17
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 环境空气发布：校准报告
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class CalibrationReportDAL
    {
        #region << 变量 >>
        /// <summary>
        /// 数据库处理类
        /// </summary>
        DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();

        /// <summary>
        /// 存储过程处理类
        /// </summary>
        BaseDAHelper g_DBBiz = Singleton<BaseDAHelper>.GetInstance();

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(DataConnectionType.AirAutoMonitoring);

        private string tableName = "QC.TB_DeviationReport";
        #endregion


        #region << 方法 >>
        /// <summary>
        /// 按点位获取校准报告数据(按日统计)
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="pointid"></param>
        /// <returns></returns>
        public DataView GetCalibrationDayData(DateTime beginTime, DateTime endTime, string[] pointid, string[] factors)
        {
            //'a21026', 'a21003', 'a21005', 'a05024'
            //            string sql = string.Format(@"SELECT distinct ROW_NUMBER() over(order by A.portid) AS id,A.portid,MAX(endTime) as startTime,MAX(endTime) as endTime,B.monitoringPointName as [PointName],B.color as color
            //                                       , (select STUFF( (SELECT  ','+Convert(varchar(16),startTime,120)+' 至 '+Convert(varchar(16),endTime,120) +' '+REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE([calNameCode],'O3',''),'NOx',''),'NO',''),'NO2',''),'SO2',''),'CO','')+'('+[memo]+')'  FROM QC_DeviationReport  
            //                                         where startTime>DATEADD(HH,-3,CONVERT(varchar(10),max(A.endTime),120)) and startTime<DATEADD(HH,3,CONVERT(varchar(10),max(A.endTime),120)) and portID=A.portID
            //                                          and persistentTime>=5 and (calTypeCode='Z' or calTypeCode='S') and channelCode IN ('a21026', 'a21003', 'a21005', 'a05024') 
            //                                         order by startTime
            //                                         FOR XML PATH('')  ),1,1,'')   ) as [Description]                                       
            //                                       FROM QC_DeviationReport AS A join ({2}) AS B
            //                                       ON A.portID=B.id
            //                                       where  persistentTime>=5 and (calTypeCode='Z' OR calTypeCode='S') and channelCode IN ('a21026', 'a21003', 'a21005', 'a05024') 
            //                                        and 
            //                                       (startTime>DATEADD(HH,-4,'{0}')  and startTime<DATEADD(HH,4,'{1}') {3})
            //                                       group by CONVERT(varchar(10),DATEADD(HH,-4,A.startTime),120),CONVERT(varchar(10),DATEADD(HH,4,A.startTime),120),A.portid,B.monitoringPointName,B.color"
            //                                       , beginTime, endTime,pointid.Equals("") ? " and 1=0" : " and A.portID in(" + pointid + ")");

            string description = string.Format(@"select STUFF( (SELECT  ','+Convert(varchar(16),startTime,120)+' 至 '+Convert(varchar(16),endTime,120) +' '+REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE([calNameCode],'O3',''),'NOx',''),'NO',''),'NO2',''),'SO2',''),'CO','')+'('+[MEMO]+')'  FROM {1}  
                                         where startTime>DATEADD(HH,-3,CONVERT(varchar(10),max(A.endTime),120)) and startTime<DATEADD(HH,3,CONVERT(varchar(10),max(A.endTime),120)) and PointID=A.PointID
                                          and persistentTime>=5 and (calTypeCode='Z' or calTypeCode='S') {0} 
                                         order by startTime
                                         FOR XML PATH('')  ),1,1,'')"
                                         , factors.Length > 0 ? " and PollutantCode IN ('" + string.Join("','", factors) + "')" : " and 1=0", tableName);
            string sql = string.Format(@"SELECT distinct ROW_NUMBER() over(order by A.PointID) AS id,A.PointID,MAX(endTime) as startTime,MAX(endTime) as endTime
                                           ,({0}) as [Description]   FROM {5} AS A  
                                        where  persistentTime>=5 and (calTypeCode='Z' OR calTypeCode='S') 
                                        and (startTime>DATEADD(HH,-4,'{1}')  and startTime<DATEADD(HH,4,'{2}') {3})
                                        {4} 
                                        group by CONVERT(varchar(10),DATEADD(HH,-4,A.startTime),120),CONVERT(varchar(10),DATEADD(HH,4,A.startTime),120),A.PointID"
                                        , description, beginTime, endTime
                                        , pointid.Equals("") ? " and 1=0" : " and A.PointID in(" + string.Join(",", pointid) + ")"
                                        , factors.Length > 0 ? " and PollutantCode IN ('" + string.Join("','", factors) + "')" : " and 1=0"
                                        , tableName);
            return g_DatabaseHelper.ExecuteDataView(sql, connection);
        }

        /// <summary>
        /// 按点位获取校准报告数据(按小时统计)
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="pointid"></param>
        /// <returns></returns>
        public DataView GetCalibrationHourData(DateTime beginTime, DateTime endTime, string[] pointid, string[] factors)
        {
            string description = string.Format(@"select STUFF( (SELECT  ','+Convert(varchar(16),startTime,120)+' 至 '+Convert(varchar(16),endTime,120) +' '+REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE([calNameCode],'O3',''),'NOx',''),'NO',''),'NO2',''),'SO2',''),'CO','')+'('+[MEMO]+')'  FROM {1}  
                                         where startTime>DATEADD(HH,-3,CONVERT(varchar(10),max(A.endTime),120)) and startTime<DATEADD(HH,3,CONVERT(varchar(10),max(A.endTime),120)) and PointID=A.PointID
                                          and persistentTime>=5 and (calTypeCode='Z' or calTypeCode='S') {0} 
                                         order by startTime
                                         FOR XML PATH('')  ),1,1,'')"
                                         , factors.Length > 0 ? " and PollutantCode IN ('" + string.Join("','", factors) + "')" : " and 1=0", tableName);
            string sql = string.Format(@"SELECT distinct ROW_NUMBER() over(order by A.PointID) AS id,A.PointID,min(startTime) as startTime,MAX(endTime) as endTime
                                           ,({0}) as [Description]   FROM {5} AS A  
                                        where  persistentTime>=5 and (calTypeCode='Z' OR calTypeCode='S') 
                                        and (startTime>DATEADD(HH,-4,'{1}')  and startTime<DATEADD(HH,4,'{2}') {3})
                                        {4} 
                                        group by CONVERT(varchar(10),DATEADD(HH,-4,A.startTime),120),CONVERT(varchar(10),DATEADD(HH,4,A.startTime),120),A.PointID"
                                        , description, beginTime, endTime
                                        , pointid.Equals("") ? " and 1=0" : " and A.PointID in(" + string.Join(",", pointid) + ")"
                                        , factors.Length > 0 ? " and PollutantCode IN ('" + string.Join("','", factors) + "')" : " and 1=0"
                                        , tableName);
            return g_DatabaseHelper.ExecuteDataView(sql, connection);
        }
        #endregion
    }
}
