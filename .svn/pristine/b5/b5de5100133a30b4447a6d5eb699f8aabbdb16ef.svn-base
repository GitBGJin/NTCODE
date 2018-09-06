using SmartEP.Core.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.MonitoringBusiness
{
    /// <summary>
    /// 名称：DataOnlineDAL.cs
    /// 创建人：窦曙健
    /// 创建日期：2016-5-8
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 数据在线
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class DataOnlineDAL
    {
        /// <summary>
        /// 数据库处理类
        /// </summary>
        SmartEP.Utilities.AdoData.DatabaseHelper g_DatabaseHelper = SmartEP.Core.Generic.Singleton<SmartEP.Utilities.AdoData.DatabaseHelper>.GetInstance();

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(Enums.DataConnectionType.MonitoringBusiness);

        /// <summary>
        /// 根据应用类型和测点Id获取所有的数据
        /// </summary>
        /// <param name="applicationType">应用类型</param>
        /// <param name="pointIds">测点Id</param>
        /// <returns></returns>
        public DataTable GetDataOnline(ApplicationType applicationType, string[] pointIds, PollutantDataType pollutantDataType)
        {
            string ApplicationUid = SmartEP.Core.Enums.EnumMapping.GetDesc(applicationType);
            string strPoints = "";
            foreach (string pointId in pointIds)
            {
                strPoints += pointId.ToString() + ",";
            }
            strPoints = strPoints.Remove(strPoints.Length - 1, 1);
            StringBuilder strSql = new StringBuilder();

            switch (pollutantDataType)
            {
                case PollutantDataType.Min5:
                    strSql.Clear();
                    strSql.Append("SELECT * FROM [AMS_MonitorBusiness].[dbo].[DataOnline] where   IsOnline in (1,0) and [ApplicationUid]='" + ApplicationUid + "' and [PointId] in (" + strPoints + ") and DataTypeUid='7a894b1f-e990-4cc3-87bb-be1e431c46bf'");
                    strSql.Append(" order by PointId ");
                    break;
                case PollutantDataType.Min60:
                    strSql.Clear();
                    strSql.Append("SELECT * FROM [AMS_MonitorBusiness].[dbo].[DataOnline] where  IsOnline in (1,0) and [ApplicationUid]='" + ApplicationUid + "' and [PointId] in (" + strPoints + ")  and DataTypeUid='1b6367f1-5287-4c14-b120-7a35bd176db1'");
                    strSql.Append(" order by PointId ");
                    break;
                default:
                    strSql.Clear();
                    strSql.Append("SELECT * FROM [AMS_MonitorBusiness].[dbo].[DataOnline] where  IsOnline in (1,0) and [ApplicationUid]='" + ApplicationUid + "' and [PointId] in (" + strPoints + ")  and DataTypeUid='1b6367f1-5287-4c14-b120-7a35bd176db1'");
                    strSql.Append(" order by PointId ");
                    break;
            }

            return g_DatabaseHelper.ExecuteDataTable(strSql.ToString(), connection);
        }
        /// <summary>
        /// 获取站点在线情况
        /// </summary>
        /// <param name="applicationType">应用类型</param>
        /// <param name="pointIds">站点</param>
        /// <param name="factors">因子</param>
        /// <param name="pollutantDataType">枚举类型</param>
        /// <returns></returns>
        public DataTable GetOnlineInfo(ApplicationType applicationType, string[] pointIds, string[] factors, PollutantDataType pollutantDataType)
        {
            try
            {
                string tableName = "";
                if (applicationType == ApplicationType.Air)
                {
                    if (pollutantDataType == PollutantDataType.Min5)
                    {
                        tableName = "dbo.SY_Air_InfectantBy5";
                    }
                    else
                    {
                        tableName = "dbo.SY_Air_InfectantBy60";
                    }
                }
                else
                {
                    if (pollutantDataType == PollutantDataType.Min5)
                    {
                        tableName = "dbo.SY_Water_InfectantBy5";
                    }
                    else
                    {
                        tableName = "dbo.SY_Water_InfectantBy60";
                    }
                }
                string ApplicationUid = SmartEP.Core.Enums.EnumMapping.GetDesc(applicationType);
                //取得查询行转列字段拼接
                string factorSql = string.Empty;
                string factorFlagSql = string.Empty;
                string factorMarkSql = string.Empty;
                string factorWhere = string.Empty;
                string dataTypeUid = string.Empty;
                switch (pollutantDataType)
                {
                    case PollutantDataType.Min5:
                        dataTypeUid = "7a894b1f-e990-4cc3-87bb-be1e431c46bf";
                        break;
                    case PollutantDataType.Min60:
                        dataTypeUid = "1b6367f1-5287-4c14-b120-7a35bd176db1";
                        break;
                    default:
                        dataTypeUid = "1b6367f1-5287-4c14-b120-7a35bd176db1";
                        break;
                }
                string strPoints = "";
                foreach (string pointId in pointIds)
                {
                    strPoints += pointId.ToString() + ",";
                }
                strPoints = strPoints.Remove(strPoints.Length - 1, 1);
                foreach (string factor in factors)
                {
                    string factorFlag = factor + "_Status";
                    string factorMark = factor + "_Mark";
                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                    factorFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN Status END) AS [{1}] ", factor, factorFlag);
                    factorMarkSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN Mark END) AS [{1}] ", factor, factorMark);
                    factorWhere += "'" + factor + "',";
                }
                factorWhere = factorWhere.Remove(factorWhere.Length - 1, 1);
                string sql = string.Format(@"select online.PointId
                                                    ,[IsOnline] as NetWorking
                                                    ,MAX(case when [IsOnline]=1 then '在线' else case when [IsOnline]=0 then '离线'+cast( (OffLineTime/60) as nvarchar) +'小时'+ cast( (OffLineTime%60)as nvarchar) +'分' else '在线'  end end) as NetWorkInfo 
                                                    ,NewDataTime as Tstamp {0} {1} {2}
                                                    FROM [dbo].[DataOnline] as online left join {6} as auto on online.PointId=auto.PointId and online.NewDataTime=auto.tstamp
                                                    inner join [dbo].[SY_MonitoringPoint] mt on mt.PointId = online.PointId inner join dbo.SY_View_CodeMainItem svcRG on mt.RegionUid = svcRG.ItemGuid
                                                    where DataTypeUid='{3}' and online.PointId in ({4}) and  online.ApplicationUid='{5}'  and IsOnline in (1,0)
                                                    group by online.PointId,[IsOnline],NewDataTime,svcRG.sortNumber,mt.OrderByNum order by svcRG.sortNumber desc,mt.OrderByNum desc"
                    , factorSql, factorFlagSql, factorMarkSql, dataTypeUid, strPoints, ApplicationUid, tableName);
                return g_DatabaseHelper.ExecuteDataTable(sql.ToString(), connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取站点在线情况
        /// </summary>
        /// <param name="InstrumentUids"></param>
        /// <param name="pollutantDataType"></param>
        /// <param name="online"></param>
        /// <returns></returns>
        public DataTable GetInstrumentOnlineInfo(string[] InstrumentUids, PollutantDataType pollutantDataType, string online)
        {
            try
            {
                string tableName = "[dbo].[InstrumentDataOnline]";
                string dataTypeUid = string.Empty;
                switch (pollutantDataType)
                {
                    case PollutantDataType.Min1:
                        dataTypeUid = "c36398ef-2bec-49be-8fca-b491fecaa359";
                        break;
                    case PollutantDataType.Min5:
                        dataTypeUid = "7a894b1f-e990-4cc3-87bb-be1e431c46bf";
                        break;
                    case PollutantDataType.Min60:
                        dataTypeUid = "1b6367f1-5287-4c14-b120-7a35bd176db1";
                        break;
                    default:
                        dataTypeUid = "1b6367f1-5287-4c14-b120-7a35bd176db1";
                        break;
                }
                string strInstrumentUids = "";
                foreach (string InstrumentUid in InstrumentUids)
                {
                    strInstrumentUids += "'" + InstrumentUid.ToString() + "',";
                }
                strInstrumentUids = strInstrumentUids.Remove(strInstrumentUids.Length - 1, 1);
                string IsOnline = "0,1";
                if (online == "2")
                    IsOnline = "1";
                else if (online == "3")
                    IsOnline = "0";
                string sql = string.Format(@"SELECT Ins.[InstrumentName]
									  ,[IsOnline] NetWorking
									   ,MAX(case when [IsOnline]=1 then '在线' else case when [IsOnline]=0 then '离线'+cast( (OffLineTime/60) as nvarchar) +'小时'+ cast( (OffLineTime%60)as nvarchar) +'分' else '在线'  end end) as NetWorkInfo 
									  ,[NewDataTime] Tstamp
								  FROM {3} line
                                  left join [AMS_BaseData].[InstrInfo].[TB_Instruments] Ins
                                  on line.[InstrumentUid] =Ins.RowGuid
                                  where DataTypeUid='{0}' and line.InstrumentUid in ({1}) and IsOnline in ({2})
								 group by Ins.[InstrumentName],[IsOnline],NewDataTime,Ins.OrderByNum order by Ins.OrderByNum desc"
                    , dataTypeUid, strInstrumentUids, IsOnline, tableName);
                return g_DatabaseHelper.ExecuteDataTable(sql.ToString(), connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获得站点以及其所属运营商和在线情况
        /// </summary>
        /// <param name="applicationType"></param>
        /// <param name="portIds"></param>
        /// <param name="pollutantDataType"></param>
        /// <returns></returns>
        public DataTable GetOperatorOnlineInfo(ApplicationType applicationType, string[] portIds, PollutantDataType pollutantDataType)
        {
            try
            {
                string tableName = "[AMS_BaseData].[dbo].[V_Point_Water_SiteMap_Business]";

                string ApplicationUid = SmartEP.Core.Enums.EnumMapping.GetDesc(applicationType);
                //取得查询行转列字段拼接

                string dataTypeUid = string.Empty;
                switch (pollutantDataType)
                {
                    case PollutantDataType.Min5:
                        dataTypeUid = "7a894b1f-e990-4cc3-87bb-be1e431c46bf";
                        break;
                    case PollutantDataType.Min60:
                        dataTypeUid = "1b6367f1-5287-4c14-b120-7a35bd176db1";
                        break;
                    default:
                        dataTypeUid = "1b6367f1-5287-4c14-b120-7a35bd176db1";
                        break;
                }
                string strPoints = "";
                foreach (string pointId in portIds)
                {
                    strPoints += pointId.ToString() + ",";
                }
                strPoints = strPoints.Remove(strPoints.Length - 1, 1);

                string sql = string.Format(@"select online.MonitoringPointUid
                                                    ,[IsOnline] as NetWorking
                                                    ,auto.PGuid as PGuid
                                                   
                                                    FROM [dbo].[DataOnline] as online inner join {0} as auto on online.MonitoringPointUid=auto.GID 
                                                    where DataTypeUid='{1}' and online.PointId in ({2}) and  ApplicationUid='{3}'  and IsOnline in (1,0)", tableName, dataTypeUid, strPoints, ApplicationUid);
                return g_DatabaseHelper.ExecuteDataTable(sql.ToString(), connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获取所有选中的站点所属的运营商
        /// </summary>
        /// <param name="applicationType"></param>
        /// <param name="portIds"></param>
        /// <param name="pollutantDataType"></param>
        /// <returns></returns>
        public DataTable GetOperatorInfo(ApplicationType applicationType, string[] portIds, PollutantDataType pollutantDataType)
        {
            try
            {
                string tableName = "[AMS_BaseData].[dbo].[V_Point_Water_SiteMap_Business]";

                string ApplicationUid = SmartEP.Core.Enums.EnumMapping.GetDesc(applicationType);
                //取得查询行转列字段拼接

                string dataTypeUid = string.Empty;
                switch (pollutantDataType)
                {
                    case PollutantDataType.Min5:
                        dataTypeUid = "7a894b1f-e990-4cc3-87bb-be1e431c46bf";
                        break;
                    case PollutantDataType.Min60:
                        dataTypeUid = "1b6367f1-5287-4c14-b120-7a35bd176db1";
                        break;
                    default:
                        dataTypeUid = "1b6367f1-5287-4c14-b120-7a35bd176db1";
                        break;
                }
                string strPoints = "";
                foreach (string pointId in portIds)
                {
                    strPoints += pointId.ToString() + ",";
                }
                strPoints = strPoints.Remove(strPoints.Length - 1, 1);

                string sql = string.Format(@"select PGuid from(select online.MonitoringPointUid
                                                    ,[IsOnline] as NetWorking
                                                    ,auto.PGuid as PGuid
                                                   
                                                    FROM [dbo].[DataOnline] as online inner join {0} as auto on online.MonitoringPointUid=auto.GID 
                                                    where DataTypeUid='{1}' and online.PointId in ({2}) and  ApplicationUid='{3}'  and IsOnline in (1,0) ) as hh group by PGuid"
                    , tableName, dataTypeUid, strPoints, ApplicationUid);
                return g_DatabaseHelper.ExecuteDataTable(sql.ToString(), connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 根据运营商的id去获取运营商的名字
        /// </summary>
        /// <param name="operas">运营商的id数组</param>
        /// <returns></returns>
        public DataTable GetOperatorName(string[] operas)
        {
            string tableName = "SY_View_CodeMainItem";
            string strOperas = "";
            foreach (string opera in operas)
            {
                strOperas += "'" + opera.ToString() + "',";
            }
            strOperas = strOperas.Remove(strOperas.Length - 1, 1);
            string sql = string.Format(@"select ItemGuid,ItemText from {0} where ItemGuid in ({1})"
                , tableName, strOperas);
            return g_DatabaseHelper.ExecuteDataTable(sql.ToString(), connection);
        }
        /// <summary>
        /// 获取仪器
        /// </summary>
        /// <returns></returns>
        public DataTable GetInstrumentInfo()
        {
            try
            {
                string sql = string.Format(@"select RowGuid as ItemGuid,InstrumentName as ItemText from [AMS_BaseData].[InstrInfo].[TB_Instruments] where ApplyTypeUid='3b5ac81c-cefb-4db8-b19f-6c4c2f41eb03' and EnableOrNot=1 and ShowInMenu=1 order by OrderByNum desc");
                return g_DatabaseHelper.ExecuteDataTable(sql.ToString(), connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
