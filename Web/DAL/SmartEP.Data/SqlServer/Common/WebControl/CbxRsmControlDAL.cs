﻿using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Utilities.AdoData;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.Common.WebControl
{
    /// <summary>
    /// 名称：CbxRsmControlDAL.cs
    /// 创建人：李飞
    /// 创建日期：2015-11-20
    /// 维护人员：徐阳、刘晋
    /// 最新维护人员：刘晋
    /// 最新维护日期：2017-05-23
    /// 功能描述：
    /// 自定义控件数据取得
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class CbxRsmControlDAL
    {
        /// <summary>
        /// 数据库处理类
        /// </summary>
        protected DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();
        /// <summary>
        /// 连接字符串
        /// </summary>
        string connection = SmartEP.Data.Enums.EnumMapping.GetConnectionName(Enums.DataConnectionType.BaseData);

        /// <summary>
        /// 取得自定义控件返回值
        /// </summary>
        /// <param name="applicationType"></param>
        /// <param name="rsmType"></param>
        /// <param name="pointType"></param>
        /// <param name="defaultStrList"></param>
        /// <param name="notIn"></param>
        /// <param name="userGuid"></param>
        /// <param name="IsCheckAll"></param>
        /// <param name="isAllSel"></param>
        /// <returns></returns>
        public DataView GetRsmData(ApplicationType applicationType, CbxRsmType rsmType, RsmPointMode pointType, String defaultStrList, String notIn, String userGuid, String _defaultAudit, String a, Boolean IsCheckAll = false, bool isAllSel = false)
        {

            string sql = string.Empty;
            switch (rsmType)
            {
                case CbxRsmType.Point:
                    sql = GetPointRsmSql(applicationType, pointType, notIn, userGuid, isAllSel);
                    break;
                case CbxRsmType.ChannelFactor:
                    sql = GetFactorRsmSql(applicationType, notIn, userGuid, _defaultAudit, a, isAllSel);
                    break;
            }
            return g_DatabaseHelper.ExecuteDataView(sql, connection);
        }

        /// <summary>
        /// 根据区域名称获取下属站点的名称
        /// </summary>
        /// <param name="regionName"></param>
        /// <returns></returns>
        public List<string> GetPointNameByRegion(string regionName)
        {
            string PointTableName = "V_Point_Air_SiteMap_Region";
            string sql = string.Format(@"SELECT B.PName
                                        FROM {0} A
                                        inner join {0} B
                                        on A.PName='{1}' and A.CGuid = B.PGuid", PointTableName, regionName);
            DataView dv = g_DatabaseHelper.ExecuteDataView(sql, connection);
            List<string> list = new List<string>();
            foreach (DataRowView dr in dv)
            {
                string pointName = dr["PName"].ToString();
                list.Add(pointName);
            }
            return list;
        }

        /// <summary>
        /// 因子SQL处理
        /// </summary>
        /// <param name="applicationType">应用程序类型</param>
        /// <param name="NotIn">不包含</param>
        /// <param name="UserGuid">用户Guid</param>
        /// <param name="_defaultAudit">是否常规站</param>
        /// <param name="a">区分同名方法字段</param>
        /// <param name="isAll">是否所有因子</param>
        /// <returns></returns>
        public string GetFactorRsmSql(ApplicationType applicationType, String NotIn, String UserGuid, String _defaultAudit, String a, bool isAll = false)
        {
            string PointTableName = "";
            switch (applicationType)
            {
                case ApplicationType.Air:
                    PointTableName = "V_Factor_Air_SiteMap";
                    break;
                case ApplicationType.Water:
                    PointTableName = "V_Factor_Water_SiteMap";
                    break;
                case ApplicationType.Noise:
                    PointTableName = "V_Factor_Air_SiteMap";
                    break;
            }

            string sql = string.Empty;
            string where = " (allInfo.PGuid is null or AuthInfo.ParameterUid is not null) ";
            if (!string.IsNullOrEmpty(NotIn))
            {
                where += string.Format(" AND allInfo.CGuid NOT IN ('{0}')", StringExtensions.GetArrayStrNoEmpty(NotIn.Split(',').ToList(), "','"));
            }
            if (isAll)
            {
                where = " 1=1 ";
            }

            string data = string.Empty;
            switch (_defaultAudit)
            {
                case "lzspy":
                    data = "5575a0e1-d948-4566-9dcd-4b4767688add";
                    break;
                case "ljpy":
                    data = "da92c7c1-4932-4007-a6d5-2866aa8c63f1";
                    break;
                case "zdy":
                    data = "59f02681-093f-48f0-9cac-ac59acd7038f";
                    break;
                case "ec":
                    data = "14b38adf-d899-4362-99ff-6a9e9dd35485";
                    break;
                case "htfxy":
                    data = "e5b6d666-24d1-473a-b15a-33a36245d44f";
                    break;
                case "tyfsy":
                    data = "aabe91e0-29a4-427c-becc-0b29f1224422";
                    break;
                case "vocs":
                    data = "fbc6dyta-d06c-678g-b5y0-89b12be5bda3";
                    break;
                case "vocsw":
                    data = "fbc6dyta-d06c-678g-b5y0-89b12be5bda3w";
                    break;
                case "vocskqy":
                    data = "fbc6dyta-d06c-678g-b5y0-89b12be5bda3kqy";
                    break;
                case "factorS":
                    data = "factorS";
                    break;
                case "superSR":
                    data = "superSR";
                    break;
                case "NTTJ":
                    data="NTTJ";
                    break;
                default:
                    data = "1";
                    break;
            }
            if (_defaultAudit == "1")
            {
                sql = string.Format(@"
                WITH RSMInfo AS (
                    select GID
                        ,PGuid
                        ,CGuid
                        ,CONVERT(NVARCHAR(20),CASE [PID] WHEN '' THEN NULL ELSE [PID] END)+':'
                            + CONVERT(NVARCHAR(50),DecimalDigit)+':'
                            + CONVERT(NVARCHAR(50),MeasureUnitName)+':'+CONVERT(NVARCHAR(50),[CGuid]) AS [RsmValue]
                        ,PName AS RsmName
                        ,POrder
                        ,COrder
                    from {0} as allInfo
                    left join dbo.F_GetUserAuthConfig('{1}','{2}','pollutant') as AuthInfo
                        on allInfo.CGuid = AuthInfo.ParameterUid
                    left join 
                    (
                        SELECT DISTINCT PollutantUid
                        FROM V_Point_InstrumentChannels
                        WHERE TypeUid='ae39f55e-5c43-4b4a-b224-0b925b5f3c9f'
                            and ApplicationUid = '{2}'
                    ) as insCh
                        on allInfo.CGuid = insCh.PollutantUid
                    where {3} AND (insCh.PollutantUid IS NOT NULL OR allInfo.PGuid is NULL)
                ) 
                SELECT RSMInfo.[GID] 
                    , RSMInfo.[PGuid]
                    , RSMInfo.[CGuid]
                    , RSMInfo.[RsmValue]
                    , RSMInfo.[RsmName]
                    , RSMInfo.[POrder]
                    , RSMInfo.[COrder]
                FROM RSMInfo
                WHERE PGuid IS NULL 
	                AND [CGuid] IN ('6acccea5-8178-4163-85e8-3c0f72c93657','339B72C4-7295-4D31-B9EB-23342CB3697E')
                UNION
                SELECT RSMInfo.[GID] 
                    , RSMInfo.[PGuid]
                    , RSMInfo.[CGuid]
                    , RSMInfo.[RsmValue]
                    , RSMInfo.[RsmName]
                    , RSMInfo.[POrder]
                    , RSMInfo.[COrder]
                FROM RSMInfo
                WHERE PGuid IS NOT NULL 
	                AND PGuid IN ('6acccea5-8178-4163-85e8-3c0f72c93657','339B72C4-7295-4D31-B9EB-23342CB3697E')
                order by POrder desc,COrder desc
                ", PointTableName, UserGuid, SmartEP.Core.Enums.EnumMapping.GetDesc(applicationType), where);
            }
            else if (_defaultAudit == "factorS")
            {
                sql = string.Format(@"
                WITH RSMInfo AS (
                    select GID
                        ,PGuid
                        ,CGuid
                        ,CONVERT(NVARCHAR(20),CASE [PID] WHEN '' THEN NULL ELSE [PID] END)+':'
                            + CONVERT(NVARCHAR(50),DecimalDigit)+':'
                            + CONVERT(NVARCHAR(50),MeasureUnitName)+':'+CONVERT(NVARCHAR(50),[CGuid]) AS [RsmValue]
                        ,PName AS RsmName
                        ,POrder
                        ,COrder
                    from {0} as allInfo
                    left join dbo.F_GetUserAuthConfig('{1}','{2}','pollutant') as AuthInfo
                        on allInfo.CGuid = AuthInfo.ParameterUid
                    left join 
                    (
                        SELECT DISTINCT PollutantUid
                        FROM V_Point_InstrumentChannels
                        WHERE TypeUid='ae39f55e-5c43-4b4a-b224-0b925b5f3c9f'
                            and ApplicationUid = '{2}'
                    ) as insCh
                        on allInfo.CGuid = insCh.PollutantUid
                    where {3} AND (insCh.PollutantUid IS NOT NULL OR allInfo.PGuid is NULL)
                ) 
                SELECT RSMInfo.[GID] 
                    , RSMInfo.[PGuid]
                    , RSMInfo.[CGuid]
                    , RSMInfo.[RsmValue]
                    , RSMInfo.[RsmName]
                    , RSMInfo.[POrder]
                    , RSMInfo.[COrder]
                FROM RSMInfo
                WHERE PGuid IS NULL 
	                AND [CGuid] IN ('5575a0e1-d948-4566-9dcd-4b4767688add','14b38adf-d899-4362-99ff-6a9e9dd35485','59f02681-093f-48f0-9cac-ac59acd7038f')
                UNION
                select RowGuid as GID
                ,LJPTypeGuid AS PGuid
                ,RowGuid as CGuid, CONVERT(NVARCHAR(20),CASE [PID] WHEN '' THEN NULL ELSE [PID] END)+':'
                            + CONVERT(NVARCHAR(50),DecimalDigit)+':'
                            + CONVERT(NVARCHAR(50),MeasureUnitName)+':'+CONVERT(NVARCHAR(50),[CGuid]) AS [RsmValue]
                            ,PName AS RsmName
                ,POrder
                ,COrder
                FROM dbo.DT_LJPYType2  AS v
                LEFT JOIN {0}  AS r ON r.PID=PollutantCode
                union 
                SELECT
                [RowGuid] AS GID
                ,Null  as PGuid
                ,[RowGuid] AS CGuid
                ,Null as RsmValue
                ,LJPYType as RsmName
                ,OrderByNum as POrder
                ,99999 as COrder
                FROM dbo.DT_LJPYType1
                union 
                SELECT RSMInfo.[GID] 
                    , RSMInfo.[PGuid]
                    , RSMInfo.[CGuid]
                    , RSMInfo.[RsmValue]
                    , RSMInfo.[RsmName]
                    , RSMInfo.[POrder]
                    , RSMInfo.[COrder]
                FROM RSMInfo
                WHERE PGuid IS NOT NULL 
	                AND PGuid IN ('5575a0e1-d948-4566-9dcd-4b4767688add','14b38adf-d899-4362-99ff-6a9e9dd35485','59f02681-093f-48f0-9cac-ac59acd7038f')
                order by POrder desc,COrder desc
                ", PointTableName, UserGuid, SmartEP.Core.Enums.EnumMapping.GetDesc(applicationType), where);
            }
            else if (_defaultAudit == "vocs")
            {
                sql = string.Format(@"
                SELECT [RowGuid] as GID
      
      ,[VOC2TypeGuid] AS PGuid
      ,[RowGuid] as CGuid
      ,CONVERT(NVARCHAR(20),CASE [PID] WHEN '' THEN NULL ELSE [PID] END)+':'
                            + CONVERT(NVARCHAR(50),DecimalDigit)+':'
                            + CONVERT(NVARCHAR(50),MeasureUnitName)+':'+CONVERT(NVARCHAR(50),[CGuid]) AS [RsmValue]
      ,PName AS RsmName
      ,POrder
      ,COrder
  FROM [dbo].[DT_VOC3Type] AS v
  LEFT JOIN {0}  AS r ON v.PollutantCode=r.PID
union 
SELECT
      [RowGuid] AS GID
      ,Null  as PGuid
      ,[RowGuid] AS CGuid
      ,Null as RsmValue
      ,VOC2Type as RsmName
      ,OrderByNum as POrder
      ,99999 as COrder
  FROM [dbo].[DT_VOC2Type]
  order by POrder desc,COrder desc
                ", PointTableName, UserGuid, SmartEP.Core.Enums.EnumMapping.GetDesc(applicationType), where);
            }
            else if (_defaultAudit == "superSR")
            {
                sql = string.Format(@"
                WITH RSMInfo AS (
                    select GID
                        ,PGuid
                        ,CGuid
                        ,CONVERT(NVARCHAR(20),CASE [PID] WHEN '' THEN NULL ELSE [PID] END)+':'
                            + CONVERT(NVARCHAR(50),DecimalDigit)+':'
                            + CONVERT(NVARCHAR(50),MeasureUnitName)+':'+CONVERT(NVARCHAR(50),[CGuid]) AS [RsmValue]
                        ,PName AS RsmName
                        ,POrder
                        ,COrder
                    from {0} as allInfo
                    left join dbo.F_GetUserAuthConfig('{1}','{2}','pollutant') as AuthInfo
                        on allInfo.CGuid = AuthInfo.ParameterUid
                    left join 
                    (
                        SELECT DISTINCT PollutantUid
                        FROM V_Point_InstrumentChannels
                        WHERE TypeUid='ae39f55e-5c43-4b4a-b224-0b925b5f3c9f'
                            and ApplicationUid = '{2}'
                    ) as insCh
                        on allInfo.CGuid = insCh.PollutantUid
                    where {3} AND (insCh.PollutantUid IS NOT NULL OR allInfo.PGuid is NULL) 
and [PID] not in ('a21026','a21004','a21005','a05024','a34002','a34004','a05040','a05041','a51004','a34005') 
and [PID] NOT IN (SELECT PollutantCode FROM [Standard].[TB_PollutantCode] where VOCType ='2')
and [PID] NOT IN (SELECT PollutantCode FROM [Standard].[TB_PollutantCode] where IsUseOrNot=0)
                ) 
                SELECT RSMInfo.[GID] 
                    , RSMInfo.[PGuid]
                    , RSMInfo.[CGuid]
                    , RSMInfo.[RsmValue]
                    , RSMInfo.[RsmName]
                    , RSMInfo.[POrder]
                    , RSMInfo.[COrder]
                FROM RSMInfo
                WHERE PGuid IS NULL 
	                AND [CGuid] IN ('aabe91e0-29a4-427c-becc-0b29f1224422','339B72C4-7295-4D31-B9EB-23342CB3697E')
                UNION
                SELECT [RowGuid] as GID
            ,[VOC2TypeGuid] AS PGuid
            ,[RowGuid] as CGuid
            ,CONVERT(NVARCHAR(20),CASE [PID] WHEN '' THEN NULL ELSE [PID] END)+':'
                            + CONVERT(NVARCHAR(50),DecimalDigit)+':'
                            + CONVERT(NVARCHAR(50),MeasureUnitName)+':'+CONVERT(NVARCHAR(50),[CGuid]) AS [RsmValue]
                ,PName AS RsmName
                ,POrder
                ,COrder
                FROM [dbo].[DT_VOC3Type] AS v
                LEFT JOIN {0}  AS r ON v.PollutantCode=r.PID
                 where  v.CreatDateTime<'2018-05-15'
        UNION
            SELECT
                [RowGuid] AS GID
                ,Null  as PGuid
                ,[RowGuid] AS CGuid
                ,Null as RsmValue
                ,VOC2Type as RsmName
                ,OrderByNum as POrder
                ,99999 as COrder
                FROM [dbo].[DT_VOC2Type]
                UNION
                SELECT RSMInfo.[GID] 
                    , RSMInfo.[PGuid]
                    , RSMInfo.[CGuid]
                    , RSMInfo.[RsmValue]
                    , RSMInfo.[RsmName]
                    , RSMInfo.[POrder]
                    , RSMInfo.[COrder]
                FROM RSMInfo
                WHERE PGuid IS NOT NULL 
	                AND PGuid IN ('aabe91e0-29a4-427c-becc-0b29f1224422','339B72C4-7295-4D31-B9EB-23342CB3697E')
                order by POrder,COrder desc
                ", PointTableName, UserGuid, SmartEP.Core.Enums.EnumMapping.GetDesc(applicationType), where);
            }
            else if (data == "aabe91e0-29a4-427c-becc-0b29f1224422")
            {
                sql = string.Format(@"
                WITH RSMInfo AS (
                    select GID
                        ,PGuid
                        ,CGuid
                        ,CONVERT(NVARCHAR(20),CASE [PID] WHEN '' THEN NULL ELSE [PID] END)+':'
                            + CONVERT(NVARCHAR(50),DecimalDigit)+':'
                            + CONVERT(NVARCHAR(50),MeasureUnitName)+':'+CONVERT(NVARCHAR(50),[CGuid]) AS [RsmValue]
                        ,PName AS RsmName
                        ,POrder
                        ,COrder
                    from {0} as allInfo
                    left join dbo.F_GetUserAuthConfig('{1}','{2}','pollutant') as AuthInfo
                        on allInfo.CGuid = AuthInfo.ParameterUid
                    left join 
                    (
                        SELECT DISTINCT PollutantUid
                        FROM V_Point_InstrumentChannels
                        WHERE TypeUid='ae39f55e-5c43-4b4a-b224-0b925b5f3c9f'
                            and ApplicationUid = '{2}'
                    ) as insCh
                        on allInfo.CGuid = insCh.PollutantUid
                    where {3} AND (insCh.PollutantUid IS NOT NULL OR allInfo.PGuid is NULL)
                ) 
                SELECT RSMInfo.[GID] 
                    , RSMInfo.[PGuid]
                    , RSMInfo.[CGuid]
                    , RSMInfo.[RsmValue]
                    , RSMInfo.[RsmName]
                    , RSMInfo.[POrder]
                    , RSMInfo.[COrder]
                FROM RSMInfo
                WHERE PGuid IS NULL 
	                AND [CGuid] ='{4}'
                UNION
                SELECT RSMInfo.[GID] 
                    , RSMInfo.[PGuid]
                    , RSMInfo.[CGuid]
                    , RSMInfo.[RsmValue]
                    , RSMInfo.[RsmName]
                    , RSMInfo.[POrder]
                    , RSMInfo.[COrder]
                FROM RSMInfo
                WHERE PGuid IS NOT NULL 
	                AND PGuid = '{4}'
                order by POrder desc,COrder desc
                ", PointTableName, UserGuid, SmartEP.Core.Enums.EnumMapping.GetDesc(applicationType), where, data);
            }
            else if (data == "fbc6dyta-d06c-678g-b5y0-89b12be5bda3w")
            {
                sql = string.Format(@"
                                SELECT [RowGuid] as GID
      
      ,[VOC2TypeGuid] AS PGuid
      ,[RowGuid] as CGuid
      ,CONVERT(NVARCHAR(20),CASE [PID] WHEN '' THEN NULL ELSE [PID] END)+':'
                            + CONVERT(NVARCHAR(50),DecimalDigit)+':'
                            + CONVERT(NVARCHAR(50),MeasureUnitName)+':'+CONVERT(NVARCHAR(50),[CGuid]) AS [RsmValue]
      ,PName AS RsmName
      ,POrder
      ,COrder
  FROM [dbo].[DT_VOC3Type] AS v
  LEFT JOIN {0}  AS r ON v.PollutantCode=r.PID
  where PName in (SELECT  PollutantName FROM [Standard].[TB_PollutantCode] where VOCType!='2') and v.CreatDateTime<'2018-05-15'
union 
SELECT
      b.[RowGuid] AS GID
      ,Null  as PGuid
      ,b.[RowGuid] AS CGuid
      ,Null as RsmValue
      ,b.VOC2Type as RsmName
      ,b.OrderByNum as POrder
      ,99999 as COrder
  FROM [dbo].[DT_VOC3Type] a
  left join [dbo].[DT_VOC2Type] b on a.VOC2TypeGuid=b.RowGuid
  where a.PollutantCode in (SELECT PollutantCode FROM [Standard].[TB_PollutantCode] where VOCType !='2' and IsUseOrNot=1)             
  order by POrder asc,COrder desc
                ", PointTableName, UserGuid, SmartEP.Core.Enums.EnumMapping.GetDesc(applicationType), where, data.Substring(0,data.Length-1));
            }
            else if (data == "fbc6dyta-d06c-678g-b5y0-89b12be5bda3kqy")
            {
                sql = string.Format(@"
                                SELECT [RowGuid] as GID
      ,[VOC2TypeGuid] AS PGuid
      ,[RowGuid] as CGuid
      ,CONVERT(NVARCHAR(20),CASE [PID] WHEN '' THEN NULL ELSE [PID] END)+':'
                            + CONVERT(NVARCHAR(50),DecimalDigit)+':'
                            + CONVERT(NVARCHAR(50),MeasureUnitName)+':'+CONVERT(NVARCHAR(50),[CGuid]) AS [RsmValue]
      ,PName AS RsmName
      ,POrder
      ,COrder
  FROM [dbo].[DT_VOC3Type] AS v
  LEFT JOIN V_Factor_Air_SiteMap  AS r ON v.PollutantCode=r.PID
  where PName in (SELECT  PollutantName FROM [Standard].[TB_PollutantCode] where VOCType!='2') and v.CreatDateTime<'2018-05-15'
union 
SELECT
      b.[RowGuid] AS GID
      ,Null  as PGuid
      ,b.[RowGuid] AS CGuid
      ,Null as RsmValue
      ,b.VOC2Type as RsmName
      ,b.OrderByNum as POrder
      ,99999 as COrder
  FROM [dbo].[DT_VOC3Type] a
  left join [dbo].[DT_VOC2Type] b on a.VOC2TypeGuid=b.RowGuid
  where a.PollutantCode in (SELECT PollutantCode FROM [Standard].[TB_PollutantCode] where VOCType!='2' and IsUseOrNot=1)             
  order by POrder asc,COrder desc
                ", PointTableName, UserGuid, SmartEP.Core.Enums.EnumMapping.GetDesc(applicationType), where, data.Substring(0, data.Length - 3));
            }
            else if (data == "NTTJ")
            {
                sql = string.Format(@"
                WITH RSMInfo AS (
                    select GID
                        ,PGuid
                        ,CGuid
                        ,CONVERT(NVARCHAR(20),CASE [PID] WHEN '' THEN NULL ELSE [PID] END)+':'
                            + CONVERT(NVARCHAR(50),DecimalDigit)+':'
                            + CONVERT(NVARCHAR(50),MeasureUnitName)+':'+CONVERT(NVARCHAR(50),[CGuid]) AS [RsmValue]
                        ,PName AS RsmName
                        ,POrder
                        ,COrder
                    from {0} as allInfo
                    left join dbo.F_GetUserAuthConfig('{1}','{2}','pollutant') as AuthInfo
                        on allInfo.CGuid = AuthInfo.ParameterUid
                    left join 
                    (
                        SELECT DISTINCT PollutantUid
                        FROM V_Point_InstrumentChannels
                        WHERE TypeUid='ae39f55e-5c43-4b4a-b224-0b925b5f3c9f'
                            and ApplicationUid = '{2}'
                    ) as insCh
                        on allInfo.CGuid = insCh.PollutantUid
                    where {3} AND (insCh.PollutantUid IS NOT NULL OR allInfo.PGuid is NULL)  AND Pid not in ('a05040','a05041')
                ) 
                SELECT RSMInfo.[GID] 
                    , RSMInfo.[PGuid]
                    , RSMInfo.[CGuid]
                    , RSMInfo.[RsmValue]
                    , RSMInfo.[RsmName]
                    , RSMInfo.[POrder]
                    , RSMInfo.[COrder]
                FROM RSMInfo
                WHERE PGuid IS NULL 
	                AND [CGuid] IN (SELECT DISTINCT PGuid FROM RSMInfo)
                UNION
                SELECT RSMInfo.[GID] 
                    , RSMInfo.[PGuid]
                    , RSMInfo.[CGuid]
                    , RSMInfo.[RsmValue]
                    , RSMInfo.[RsmName]
                    , RSMInfo.[POrder]
                    , RSMInfo.[COrder]
                FROM RSMInfo
                WHERE PGuid IS NOT NULL 
	                AND PGuid IN (SELECT DISTINCT CGuid FROM RSMInfo WHERE PGuid IS NULL)
                order by POrder desc,COrder desc
                ", PointTableName, UserGuid, SmartEP.Core.Enums.EnumMapping.GetDesc(applicationType), where);
            }
            else
            {
                sql = string.Format(@"
                WITH RSMInfo AS (
                    select GID
                        ,PGuid
                        ,CGuid
                        ,CONVERT(NVARCHAR(20),CASE [PID] WHEN '' THEN NULL ELSE [PID] END)+':'
                            + CONVERT(NVARCHAR(50),DecimalDigit)+':'
                            + CONVERT(NVARCHAR(50),MeasureUnitName)+':'+CONVERT(NVARCHAR(50),[CGuid]) AS [RsmValue]
                        ,PName AS RsmName
                        ,POrder
                        ,COrder
                    from {0} as allInfo
                    left join dbo.F_GetUserAuthConfig('{1}','{2}','pollutant') as AuthInfo
                        on allInfo.CGuid = AuthInfo.ParameterUid
                    left join 
                    (
                        SELECT DISTINCT PollutantUid
                        FROM V_Point_InstrumentChannels
                        WHERE TypeUid='ae39f55e-5c43-4b4a-b224-0b925b5f3c9f'
                            and ApplicationUid = '{2}'
                    ) as insCh
                        on allInfo.CGuid = insCh.PollutantUid
                    where {3} AND (insCh.PollutantUid IS NOT NULL OR allInfo.PGuid is NULL)
                ) 
                SELECT RSMInfo.[GID] 
                    , RSMInfo.[PGuid]
                    , RSMInfo.[CGuid]
                    , RSMInfo.[RsmValue]
                    , RSMInfo.[RsmName]
                    , RSMInfo.[POrder]
                    , RSMInfo.[COrder]
                FROM RSMInfo
                WHERE PGuid IS NULL 
	                AND [CGuid] ='{4}'
                UNION
                SELECT RSMInfo.[GID] 
                    , RSMInfo.[PGuid]
                    , RSMInfo.[CGuid]
                    , RSMInfo.[RsmValue]
                    , RSMInfo.[RsmName]
                    , RSMInfo.[POrder]
                    , RSMInfo.[COrder]
                FROM RSMInfo
                WHERE PGuid IS NOT NULL 
	                AND PGuid = '{4}'
                order by POrder desc,COrder desc
                ", PointTableName, UserGuid, SmartEP.Core.Enums.EnumMapping.GetDesc(applicationType), where, data);
            }
            return sql;
        }

        /// <summary>
        /// 取得自定义控件返回值
        /// </summary>
        /// <param name="applicationType"></param>
        /// <param name="rsmType"></param>
        /// <param name="pointType"></param>
        /// <param name="defaultStrList"></param>
        /// <param name="notIn"></param>
        /// <param name="userGuid"></param>
        /// <param name="IsCheckAll"></param>
        /// <param name="isAllSel"></param>
        /// <returns></returns>
        public DataView GetRsmData(ApplicationType applicationType, CbxRsmType rsmType, RsmPointMode pointType, String defaultStrList, String notIn, String userGuid, Boolean IsCheckAll = false, bool isAllSel = false)
        {

            string sql = string.Empty;
            switch (rsmType)
            {
                case CbxRsmType.Point:
                    sql = GetPointRsmSql(applicationType, pointType, notIn, userGuid, isAllSel);
                    break;
                case CbxRsmType.ChannelFactor:
                    sql = GetFactorRsmSql(applicationType, notIn, userGuid, isAllSel);
                    break;
            }
            return g_DatabaseHelper.ExecuteDataView(sql, connection);
        }

        /// <summary>
        /// 取得自定义控件超级站返回值
        /// </summary>
        /// <param name="applicationType"></param>
        /// <param name="rsmType"></param>
        /// <param name="pointType"></param>
        /// <param name="defaultStrList"></param>
        /// <param name="notIn"></param>
        /// <param name="userGuid"></param>
        /// <param name="IsCheckAll"></param>
        /// <param name="isAllSel"></param>
        /// <returns></returns>
        public DataView GetRsmData(ApplicationType applicationType, CbxRsmType rsmType, RsmPointMode pointType, String defaultStrList, String notIn, String userGuid, Boolean IsCheckAll = true, bool isAllSel = false, string _defaultSuper = "1")
        {

            string sql = string.Empty;
            switch (rsmType)
            {
                case CbxRsmType.Point:
                    sql = GetPointRsmSql(applicationType, pointType, notIn, userGuid, isAllSel, _defaultSuper);
                    break;
                case CbxRsmType.ChannelFactor:
                    sql = GetFactorRsmSql(applicationType, notIn, userGuid, isAllSel);
                    break;
            }
            return g_DatabaseHelper.ExecuteDataView(sql, connection);
        }

        /// <summary>
        /// 站点SQL处理
        /// </summary>
        /// <param name="applicationType">应用程序类型</param>
        /// <param name="PointType">测点筛选类型</param>
        /// <param name="NotIn">不包含</param>
        /// <param name="UserGuid">用户Guid</param>
        /// <param name="isAll">是否所有站点</param>
        /// <returns></returns>
        public string GetPointRsmSql(ApplicationType applicationType, RsmPointMode PointType, String NotIn, String UserGuid, bool isAll = false)
        {
            string PointTableName = "";
            switch (PointType)
            {
                case RsmPointMode.Class:
                    switch (applicationType)
                    {
                        case ApplicationType.Air:
                            PointTableName = "V_Point_Air_SiteMap_WaterDustProperty";
                            break;
                        case ApplicationType.Water:
                            PointTableName = "V_Point_Water_SiteMap_Property";
                            break;
                        case ApplicationType.Noise:
                            PointTableName = "V_Point_Noise_SiteMap_Property";
                            break;
                    }
                    break;
                case RsmPointMode.Type:
                    switch (applicationType)
                    {
                        case ApplicationType.Air:
                            PointTableName = "V_Point_Air_SiteMap_Type";
                            break;
                        case ApplicationType.Water:
                            PointTableName = "V_Point_Water_SiteMap_Type";
                            break;
                        case ApplicationType.Noise:
                            PointTableName = "V_Point_Noise_SiteMap_Type";
                            break;
                        case ApplicationType.BlueAlga:
                            PointTableName = "V_Point_Water_SiteMap_TypeLZ";
                            break;
                    }
                    break;
                case RsmPointMode.Region:
                    switch (applicationType)
                    {
                        case ApplicationType.Air:
                            PointTableName = "V_Point_Air_SiteMap_Region";
                            break;
                        case ApplicationType.Water:
                            PointTableName = "V_Point_Water_SiteMap_Region";
                            break;
                        case ApplicationType.Noise:
                            PointTableName = "V_Point_Noise_SiteMap_Region";
                            break;
                    }
                    break;
                case RsmPointMode.Business:
                    switch (applicationType)
                    {
                        case ApplicationType.Air:
                            PointTableName = "V_Point_Air_SiteMap_Region";
                            break;
                        case ApplicationType.Water:
                            PointTableName = "V_Point_Water_SiteMap_Business";
                            break;
                        case ApplicationType.Noise:
                            PointTableName = "V_Point_Noise_SiteMap_Region";
                            break;
                    }
                    break;
                case RsmPointMode.Property:
                    switch (applicationType)
                    {
                        case ApplicationType.Water:
                            PointTableName = "V_Point_Water_SiteMap_StationProperty";
                            break;
                    }
                    break;
                //case RsmPointMode.BlueAlga:
                //    PointTableName = "V_Point_Water_SiteMap_Region";
                //    break;
                default: PointTableName = "V_Point_Air_SiteMap_Type"; break;
            }
            #region << 旧代码 >>
            //            string Sql = String.Format(@"
            //                        SELECT [GID],[PGuid]
            //                            ,[CGuid]
            //                            ,CONVERT(NVARCHAR(20),CASE [PID] WHEN '' THEN NULL ELSE [PID] END)+':'
            //                                + CONVERT(NVARCHAR(50),CASE [CGuid] WHEN '' THEN NULL ELSE [CGuid] END)+':'
            //                                + CONVERT(NVARCHAR(20),CASE when [MN] IS null THEN '' ELSE [MN] END) AS [RsmValue]
            //                            ,[PName] AS [RsmName]
            //                            ,[POrder]
            //                            ,[COrder]
            //                            ,[UserGuid]
            //                            ,[IsUseOrNot]
            //                            ,[EnableCustomOrNot]
            //                        FROM {0} 
            //                        WHERE ([IsUseOrNot]=1) 
            //                            AND ([EnableCustomOrNot]=1) 
            //                            AND [UserGuid]='{1}' 
            //                            AND [EnableCustomOrNot]=1 {2}
            //                        ORDER BY [POrder],[COrder]"
            //                        , PointTableName, UserGuid, NotIn == "" ? "" : "AND [CGuid] NOT IN ('" + NotIn.Replace(",", "','") + "')");
            #endregion

            string sql = string.Empty;
            string where = " (allInfo.PGuid is null or AuthInfo.ParameterUid is not null) ";
            if (!string.IsNullOrEmpty(NotIn))
            {
                where += string.Format(" AND allInfo.CGuid NOT IN ('{0}')", StringExtensions.GetArrayStrNoEmpty(NotIn.Split(',').ToList(), "','"));
            }
            if (isAll)
            {
                where = " 1=1 ";
            }
            sql = string.Format(@"
                WITH RSMInfo AS (
                    select GID
                        ,PGuid
                        ,CGuid
                        ,CONVERT(NVARCHAR(20),CASE [PID] WHEN '' THEN NULL ELSE [PID] END)+':'
                            + CONVERT(NVARCHAR(50),[CGuid]) AS [RsmValue]
                        ,PName AS RsmName
                        ,POrder
                        ,COrder
                    from {0} as allInfo
                    left join dbo.F_GetUserAuthConfig('{1}','{2}','port') as AuthInfo
                        on allInfo.CGuid = AuthInfo.ParameterUid
                    where {3} 
                )
                SELECT RSMInfo.[GID] 
                    , RSMInfo.[PGuid]
                    , RSMInfo.[CGuid]
                    , RSMInfo.[RsmValue]
                    , RSMInfo.[RsmName]
                    , RSMInfo.[POrder]
                    , RSMInfo.[COrder]
                FROM RSMInfo
                WHERE PGuid IS NULL 
	                AND [CGuid] IN (SELECT DISTINCT PGuid FROM RSMInfo)
                UNION
                SELECT RSMInfo.[GID] 
                    , RSMInfo.[PGuid]
                    , RSMInfo.[CGuid]
                    , RSMInfo.[RsmValue]
                    , RSMInfo.[RsmName]
                    , RSMInfo.[POrder]
                    , RSMInfo.[COrder]
                FROM RSMInfo
                WHERE PGuid IS NOT NULL 
	                AND PGuid IN (SELECT DISTINCT CGuid FROM RSMInfo WHERE PGuid IS NULL)
                order by POrder desc,COrder desc ", PointTableName, UserGuid, SmartEP.Core.Enums.EnumMapping.GetDesc(applicationType), where);
            return sql;
            //            sql = string.Format(@"
            //                WITH RSMInfo AS (
            //                    select GID
            //                        ,PGuid
            //                        ,CGuid
            //                        ,CONVERT(NVARCHAR(20),CASE [PID] WHEN '' THEN NULL ELSE [PID] END)+':'
            //                            + CONVERT(NVARCHAR(50),[CGuid]) AS [RsmValue]
            //                        ,PName AS RsmName
            //                        ,POrder
            //                        ,COrder
            //                    from {0} as allInfo
            //                    left join dbo.F_GetUserAuthConfig('{1}','{2}','port') as AuthInfo
            //                        on allInfo.CGuid = AuthInfo.ParameterUid
            //                    where {3} 
            //                )
            //                SELECT A.* FROM 
            //                (SELECT RSMInfo.[GID] 
            //                    , RSMInfo.[PGuid]
            //                    , RSMInfo.[CGuid]
            //                    , RSMInfo.[RsmValue]
            //                    , RSMInfo.[RsmName]
            //                    , RSMInfo.[POrder]
            //                    , RSMInfo.[COrder]
            //                FROM RSMInfo
            //                WHERE PGuid IS NULL 
            //	                AND [CGuid] IN (SELECT DISTINCT PGuid FROM RSMInfo)) A,
            //                (SELECT RSMInfo.[GID] 
            //                    , RSMInfo.[PGuid]
            //                    , RSMInfo.[CGuid]
            //                    , RSMInfo.[RsmValue]
            //                    , RSMInfo.[RsmName]
            //                    , RSMInfo.[POrder]
            //                    , RSMInfo.[COrder]
            //                FROM RSMInfo
            //                WHERE PGuid IS NOT NULL 
            //	                AND PGuid IN (SELECT DISTINCT CGuid FROM RSMInfo WHERE PGuid IS NULL)
            //                AND CGuid IN((SELECT MonitoringPointUid FROM MPInfo.TB_MonitoringPoint WHERE EnableOrNot='1') INTERSECT (SELECT MonitoringPointUid FROM MPInfo.TB_MonitoringPointExtensionForEQMSAir WHERE SuperOrNot='0'))) B
            //                WHERE A.GID=B.PGuid
            //                UNION
            //                SELECT RSMInfo.[GID] 
            //                    , RSMInfo.[PGuid]
            //                    , RSMInfo.[CGuid]
            //                    , RSMInfo.[RsmValue]
            //                    , RSMInfo.[RsmName]
            //                    , RSMInfo.[POrder]
            //                    , RSMInfo.[COrder]
            //                FROM RSMInfo
            //                WHERE PGuid IS NOT NULL 
            //	                AND PGuid IN (SELECT DISTINCT CGuid FROM RSMInfo WHERE PGuid IS NULL)
            //                AND CGuid IN((SELECT MonitoringPointUid FROM MPInfo.TB_MonitoringPoint WHERE EnableOrNot='1') INTERSECT (SELECT MonitoringPointUid FROM MPInfo.TB_MonitoringPointExtensionForEQMSAir WHERE SuperOrNot='0'))
            //                order by POrder desc,COrder desc ", PointTableName, UserGuid, SmartEP.Core.Enums.EnumMapping.GetDesc(applicationType), where);
            //            return sql;
        }

        /// <summary>
        /// 超级站站点SQL处理
        /// </summary>
        /// <param name="applicationType">应用程序类型</param>
        /// <param name="PointType">测点筛选类型</param>
        /// <param name="NotIn">不包含</param>
        /// <param name="UserGuid">用户Guid</param>
        /// <param name="isAll">是否所有站点</param>
        /// <returns></returns>
        public string GetPointRsmSql(ApplicationType applicationType, RsmPointMode PointType, String NotIn, String UserGuid, bool isAll = false, string _defaultSuper = "1")
        {
            string PointTableName = "";
            switch (PointType)
            {
                case RsmPointMode.Class:
                    switch (applicationType)
                    {
                        case ApplicationType.Air:
                            PointTableName = "V_Point_Air_SiteMap_WaterDustProperty";
                            break;
                        case ApplicationType.Water:
                            PointTableName = "V_Point_Water_SiteMap_Property";
                            break;
                        case ApplicationType.Noise:
                            PointTableName = "V_Point_Noise_SiteMap_Property";
                            break;
                    }
                    break;
                case RsmPointMode.Type:
                    switch (applicationType)
                    {
                        case ApplicationType.Air:
                            PointTableName = "V_Point_Air_SiteMap_Type";
                            break;
                        case ApplicationType.Water:
                            PointTableName = "V_Point_Water_SiteMap_Type";
                            break;
                        case ApplicationType.Noise:
                            PointTableName = "V_Point_Noise_SiteMap_Type";
                            break;
                        case ApplicationType.BlueAlga:
                            PointTableName = "V_Point_Water_SiteMap_TypeLZ";
                            break;
                    }
                    break;
                case RsmPointMode.Region:
                    switch (applicationType)
                    {
                        case ApplicationType.Air:
                            PointTableName = "V_Point_Air_SiteMap_Region";
                            break;
                        case ApplicationType.Water:
                            PointTableName = "V_Point_Water_SiteMap_Region";
                            break;
                        case ApplicationType.Noise:
                            PointTableName = "V_Point_Noise_SiteMap_Region";
                            break;
                    }
                    break;
                case RsmPointMode.Business:
                    switch (applicationType)
                    {
                        case ApplicationType.Air:
                            PointTableName = "V_Point_Air_SiteMap_Region";
                            break;
                        case ApplicationType.Water:
                            PointTableName = "V_Point_Water_SiteMap_Business";
                            break;
                        case ApplicationType.Noise:
                            PointTableName = "V_Point_Noise_SiteMap_Region";
                            break;
                    }
                    break;
                case RsmPointMode.Property:
                    switch (applicationType)
                    {
                        case ApplicationType.Water:
                            PointTableName = "V_Point_Water_SiteMap_StationProperty";
                            break;
                    }
                    break;
                //case RsmPointMode.BlueAlga:
                //    PointTableName = "V_Point_Water_SiteMap_Region";
                //    break;
                default: PointTableName = "V_Point_Air_SiteMap_Type"; break;
            }
            #region << 旧代码 >>
            //            string Sql = String.Format(@"
            //                        SELECT [GID],[PGuid]
            //                            ,[CGuid]
            //                            ,CONVERT(NVARCHAR(20),CASE [PID] WHEN '' THEN NULL ELSE [PID] END)+':'
            //                                + CONVERT(NVARCHAR(50),CASE [CGuid] WHEN '' THEN NULL ELSE [CGuid] END)+':'
            //                                + CONVERT(NVARCHAR(20),CASE when [MN] IS null THEN '' ELSE [MN] END) AS [RsmValue]
            //                            ,[PName] AS [RsmName]
            //                            ,[POrder]
            //                            ,[COrder]
            //                            ,[UserGuid]
            //                            ,[IsUseOrNot]
            //                            ,[EnableCustomOrNot]
            //                        FROM {0} 
            //                        WHERE ([IsUseOrNot]=1) 
            //                            AND ([EnableCustomOrNot]=1) 
            //                            AND [UserGuid]='{1}' 
            //                            AND [EnableCustomOrNot]=1 {2}
            //                        ORDER BY [POrder],[COrder]"
            //                        , PointTableName, UserGuid, NotIn == "" ? "" : "AND [CGuid] NOT IN ('" + NotIn.Replace(",", "','") + "')");
            #endregion

            string sql = string.Empty;
            string where = " (allInfo.PGuid is null or AuthInfo.ParameterUid is not null) ";
            if (!string.IsNullOrEmpty(NotIn))
            {
                where += string.Format(" AND allInfo.CGuid NOT IN ('{0}')", StringExtensions.GetArrayStrNoEmpty(NotIn.Split(',').ToList(), "','"));
            }
            if (isAll)
            {
                where = " 1=1 ";
            }
            if (_defaultSuper=="AirDER")
            {
                sql = string.Format(@"
                WITH RSMInfo AS (
                    select GID
                        ,PGuid
                        ,CGuid
                        ,CONVERT(NVARCHAR(20),CASE [PID] WHEN '' THEN NULL ELSE [PID] END)+':'
                            + CONVERT(NVARCHAR(50),[CGuid]) AS [RsmValue]
                        ,PName AS RsmName
                        ,POrder
                        ,COrder
                    from {0} as allInfo
                    left join dbo.F_GetUserAuthConfig('{1}','{2}','port') as AuthInfo
                        on allInfo.CGuid = AuthInfo.ParameterUid
                    where {3} and GID !='f2369c1f-95e1-4f5d-9045-9c185ce8727a' 
                )
                SELECT RSMInfo.[GID] 
                    , RSMInfo.[PGuid]
                    , RSMInfo.[CGuid]
                    , RSMInfo.[RsmValue]
                    , RSMInfo.[RsmName]
                    , RSMInfo.[POrder]
                    , RSMInfo.[COrder]
                FROM RSMInfo
                WHERE PGuid IS NULL 
	                AND [CGuid] IN (SELECT DISTINCT PGuid FROM RSMInfo)
                UNION
                SELECT RSMInfo.[GID] 
                    , RSMInfo.[PGuid]
                    , RSMInfo.[CGuid]
                    , RSMInfo.[RsmValue]
                    , RSMInfo.[RsmName]
                    , RSMInfo.[POrder]
                    , RSMInfo.[COrder]
                FROM RSMInfo
                WHERE PGuid IS NOT NULL 
	                AND PGuid IN (SELECT DISTINCT CGuid FROM RSMInfo WHERE PGuid IS NULL)
                order by POrder desc,COrder desc ", PointTableName, UserGuid, SmartEP.Core.Enums.EnumMapping.GetDesc(applicationType), where);
            }
            else
            {
                sql = string.Format(@"
                WITH RSMInfo AS (
                    select GID
                        ,PGuid
                        ,CGuid
                        ,CONVERT(NVARCHAR(20),CASE [PID] WHEN '' THEN NULL ELSE [PID] END)+':'
                            + CONVERT(NVARCHAR(50),[CGuid]) AS [RsmValue]
                        ,PName AS RsmName
                        ,POrder
                        ,COrder
                    from {0} as allInfo
                    left join dbo.F_GetUserAuthConfig('{1}','{2}','port') as AuthInfo
                        on allInfo.CGuid = AuthInfo.ParameterUid
                    where {3} 
                )
                SELECT A.* FROM 
                (SELECT RSMInfo.[GID] 
                    , RSMInfo.[PGuid]
                    , RSMInfo.[CGuid]
                    , RSMInfo.[RsmValue]
                    , RSMInfo.[RsmName]
                    , RSMInfo.[POrder]
                    , RSMInfo.[COrder]
                FROM RSMInfo
                WHERE PGuid IS NULL 
	                AND [CGuid] IN (SELECT DISTINCT PGuid FROM RSMInfo)) A,
                (SELECT RSMInfo.[GID] 
                    , RSMInfo.[PGuid]
                    , RSMInfo.[CGuid]
                    , RSMInfo.[RsmValue]
                    , RSMInfo.[RsmName]
                    , RSMInfo.[POrder]
                    , RSMInfo.[COrder]
                FROM RSMInfo
                WHERE PGuid IS NOT NULL 
	                AND PGuid IN (SELECT DISTINCT CGuid FROM RSMInfo WHERE PGuid IS NULL)
                AND CGuid IN((SELECT MonitoringPointUid FROM MPInfo.TB_MonitoringPoint WHERE EnableOrNot='1') INTERSECT (SELECT MonitoringPointUid FROM MPInfo.TB_MonitoringPointExtensionForEQMSAir WHERE SuperOrNot='1'))) B
                WHERE A.GID=B.PGuid
                UNION
                SELECT RSMInfo.[GID] 
                    , RSMInfo.[PGuid]
                    , RSMInfo.[CGuid]
                    , RSMInfo.[RsmValue]
                    , RSMInfo.[RsmName]
                    , RSMInfo.[POrder]
                    , RSMInfo.[COrder]
                FROM RSMInfo
                WHERE PGuid IS NOT NULL 
	                AND PGuid IN (SELECT DISTINCT CGuid FROM RSMInfo WHERE PGuid IS NULL)
                AND CGuid IN((SELECT MonitoringPointUid FROM MPInfo.TB_MonitoringPoint WHERE EnableOrNot='1') INTERSECT (SELECT MonitoringPointUid FROM MPInfo.TB_MonitoringPointExtensionForEQMSAir WHERE SuperOrNot='1'))
                order by POrder desc,COrder desc ", PointTableName, UserGuid, SmartEP.Core.Enums.EnumMapping.GetDesc(applicationType), where);
            }
            
            return sql;
        }

        /// <summary>
        /// 因子SQL处理
        /// </summary>
        /// <param name="applicationType">应用程序类型</param>
        /// <param name="NotIn">不包含</param>
        /// <param name="UserGuid">用户Guid</param>
        /// <param name="isAll">是否所有因子</param>
        /// <returns></returns>
        public string GetFactorRsmSql(ApplicationType applicationType, String NotIn, String UserGuid, bool isAll = false)
        {
            string PointTableName = "";
            switch (applicationType)
            {
                case ApplicationType.Air:
                    PointTableName = "V_Factor_Air_SiteMap";
                    break;
                case ApplicationType.Water:
                    PointTableName = "V_Factor_Water_SiteMap";
                    break;
                case ApplicationType.Noise:
                    PointTableName = "V_Factor_Air_SiteMap";
                    break;
            }

            string sql = string.Empty;
            string where = " (allInfo.PGuid is null or AuthInfo.ParameterUid is not null) ";
            if (!string.IsNullOrEmpty(NotIn))
            {
                where += string.Format(" AND allInfo.CGuid NOT IN ('{0}')", StringExtensions.GetArrayStrNoEmpty(NotIn.Split(',').ToList(), "','"));
            }
            if (isAll)
            {
                where = " 1=1 ";
            }

            sql = string.Format(@"
                WITH RSMInfo AS (
                    select GID
                        ,PGuid
                        ,CGuid
                        ,CONVERT(NVARCHAR(20),CASE [PID] WHEN '' THEN NULL ELSE [PID] END)+':'
                            + CONVERT(NVARCHAR(50),DecimalDigit)+':'
                            + CONVERT(NVARCHAR(50),MeasureUnitName)+':'+CONVERT(NVARCHAR(50),[CGuid]) AS [RsmValue]
                        ,PName AS RsmName
                        ,POrder
                        ,COrder
                    from {0} as allInfo
                    left join dbo.F_GetUserAuthConfig('{1}','{2}','pollutant') as AuthInfo
                        on allInfo.CGuid = AuthInfo.ParameterUid
                    left join 
                    (
                        SELECT DISTINCT PollutantUid
                        FROM V_Point_InstrumentChannels
                        WHERE TypeUid='ae39f55e-5c43-4b4a-b224-0b925b5f3c9f'
                            and ApplicationUid = '{2}'
                    ) as insCh
                        on allInfo.CGuid = insCh.PollutantUid
                    where {3} AND (insCh.PollutantUid IS NOT NULL OR allInfo.PGuid is NULL)
                ) 
                SELECT RSMInfo.[GID] 
                    , RSMInfo.[PGuid]
                    , RSMInfo.[CGuid]
                    , RSMInfo.[RsmValue]
                    , RSMInfo.[RsmName]
                    , RSMInfo.[POrder]
                    , RSMInfo.[COrder]
                FROM RSMInfo
                WHERE PGuid IS NULL 
	                AND [CGuid] IN (SELECT DISTINCT PGuid FROM RSMInfo)
                UNION
                SELECT RSMInfo.[GID] 
                    , RSMInfo.[PGuid]
                    , RSMInfo.[CGuid]
                    , RSMInfo.[RsmValue]
                    , RSMInfo.[RsmName]
                    , RSMInfo.[POrder]
                    , RSMInfo.[COrder]
                FROM RSMInfo
                WHERE PGuid IS NOT NULL 
	                AND PGuid IN (SELECT DISTINCT CGuid FROM RSMInfo WHERE PGuid IS NULL)
                order by POrder desc,COrder desc
                ", PointTableName, UserGuid, SmartEP.Core.Enums.EnumMapping.GetDesc(applicationType), where);
            return sql;
        }
    }
}
