using SmartEP.Utilities.AdoData;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.AutoMonitoring
{
    public class DaoZhanLvTongJi
    {
        /// <summary>
        /// 数据库出库类
        /// </summary>
        DatabaseHelper g_DatabaseHelper = new DatabaseHelper();
        /// <summary>
        /// 连接字符串
        /// </summary>
        private string connection = "AMS_MonitoringBusinessConnection";

        /// <summary>
        /// 获得门禁信息表的信息
        /// </summary>
        /// <param name="TeamGuid"> 运维小组的Guid</param>
        /// <param name="beginDate"> 开始时间</param>
        /// <param name="endDate"> 结束时间</param>
        /// <param name="sysType"> 系统类型</param>
        /// <returns></returns>

        public DataTable GetAccessInformation(String[] TeamGuid, String beginDate, String endDate,
            String sysType = "")
        {

            string RowGuid = StringExtensions.GetArrayStrNoEmpty(TeamGuid.ToList<string>(), "','");

            RowGuid = "'" + RowGuid + "'";

            string sql = string.Empty;
            sql = String.Format(@"
                  SELECT tmpMain.TeamGuid, tmpMain.TeamName, a.MN, CONVERT(VARCHAR(20),accInfo.DT,120) as DT, tmpMain.PointUid, tmpMain.PointId, tmpMain.PointName, cardNum.CodeNum
                 ,ISNULL(SignCount2,0) AS N1,ISNULL(SignCount1,0) AS N2  FROM (
	             SELECT mTeam.RowGuid AS TeamGuid, TeamName,tmpPoint.PointUid,tmpPoint.PointId,tmpPoint.PointName   
	             FROM (SELECT * FROM SY_OMMP_MaintenanceTeam WHERE RowGuid  IN ({1})  ) AS mTeam 	
	             CROSS JOIN (
		         SELECT MonitoringPointUid AS PointUid, PointId, MonitoringPointName AS PointName FROM [SY_MonitoringPoint] 
		         WHERE ApplicationUid='{0}' 
               	) AS tmpPoint
                ) AS tmpMain
               LEFT JOIN TB_CardNumber AS cardNum ON tmpMain.TeamGuid=cardNum.TeamUid
               LEFT JOIN (
	          SELECT A.TeamGuid, B.ObjectID AS PointUid, CONVERT(NVARCHAR(19), A.SignInTime,120) AS DT, COUNT(A.TeamGuid) AS SignCount1
						 FROM dbo.V_Water_AccessInformation_PadTimes AS A 
               LEFT JOIN SY_OMMP_MaintenanceObject AS B ON A.MaintenanceObjectGuid=B.RowGuid
	           WHERE A.SigninTime>=CONVERT(DATETIME,'{2}',120) AND A.SigninTime<=CONVERT(DATETIME,'{3}',120) AND A.SigninTime IS NOT NULL
	           GROUP BY A.TeamGuid, B.ObjectID, CONVERT(NVARCHAR(19), A.SigninTime,120)
               ) AS attIObj ON tmpMain.TeamGuid=attIObj.TeamGuid
               inner  JOIN (
	           SELECT CardNumber, PointId, TeamGuid ,CONVERT(NVARCHAR(19), StationDate,120) AS DT,COUNT(TeamGuid) AS SignCount2 FROM            
               dbo.V_Water_AccessInformation_SignTimes

               WHERE StationDate>=CONVERT(DATETIME,'{2}',120) AND StationDate<=CONVERT(DATETIME,'{3}',120) AND StationDate IS NOT NULL 
	           GROUP BY CONVERT(NVARCHAR(19), StationDate,120), CardNumber, PointId, TeamGuid 
               ) AS accInfo ON cardNum.CodeNum=accInfo.CardNumber
               LEFT JOIN (
               SELECT mp.[MonitoringPointUid],[PointId],ai.MN,[ApplicationUid] ,[MonitoringPointName]
               FROM [AMS_BaseData].[MPInfo].[TB_MonitoringPoint] mp
               left join [AMS_BaseData].[MPInfo].[TB_AcquisitionInstrument] ai
               on mp.MonitoringPointUid=ai.MonitoringPointUid
               ) AS   a  group by
              tmpMain.TeamGuid,tmpMain.PointName,tmpMain.TeamName,a.MN, accInfo.DT,
              tmpMain.PointUid, tmpMain.PointId, tmpMain.PointName, cardNum.CodeNum,
              ISNULL(SignCount2,0) ,ISNULL(SignCount1,0)", sysType, RowGuid, beginDate, endDate);
            return g_DatabaseHelper.ExecuteDataTable(sql, connection);
        }


        /// <summary>
        /// 获得水的门禁信息表的信息
        /// </summary>
        /// <param name="TeamGuid"> 运维小组的Guid</param>
        /// <param name="beginDate"> 开始时间</param>
        /// <param name="endDate"> 结束时间</param>
        /// <param name="sysType"> 系统类型</param>
        /// <returns></returns>

        public DataTable GetWaterAccessInformation(String[] TeamGuid, String[] portIds, String beginDate, String endDate,
            String sysType = "")
        {

            string RowGuid2 = StringExtensions.GetArrayStrNoEmpty(TeamGuid.ToList<string>(), "','");
            string portId = StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), "','");
            RowGuid2 = "'" + RowGuid2 + "'";
            portId = "'" + portId + "'";
            string sql = string.Empty;
//            sql = string.Format(@"select '刷卡' type , T2.TeamGuid,T2.TeamName,T2.PointId,T2.MonitoringPointUid,T2.DT,T2.PointName,T2.MN,T2.CodeNum,0 AS N0,
//ISNULL(sum(T2.CardCnt),0) AS N1,ISNULL(sum(T2.SignCnt),0) AS N2
//from ( 
// select T.ObjectID,T.TeamGuid,T.TeamName,CONVERT(NVARCHAR(19), T.SignInTime,23) as DT,T.MonitoringPointUid,T.PointId,T.PointName,T.mn,T.CodeNum 
//  , 0 AS CardCnt,
//  (select ISNULL(sum(1),0) from V_Water_AccessInformation_PadTimes where TeamGuid=T.TeamGuid and PointId=T.PointId 
//and CONVERT(NVARCHAR(19),SignInTime,23)=CONVERT(NVARCHAR(19),T.SignInTime,23)
//  ) SignCnt
//  FROM(
//select a.ObjectID,b.TeamGuid,b.SignInTime,  c.MonitoringPointUid,c.PointId,c.MonitoringPointName PointName,d.mn,e.CodeNum,f.TeamName from 
//    EQMS_Framework.dbo.TB_OMMP_MaintenanceObject a inner join
//    EQMS_Framework.dbo.TB_OMMP_AttendanceInObject b
//    on a.RowGuid=b.MaintenanceObjectGuid
//   inner join AMS_BaseData.MPInfo.TB_MonitoringPoint c
//   on c.MonitoringPointUid=a.objectid 
//   inner join [AMS_BaseData].[MPInfo].[TB_AcquisitionInstrument] d
//    on c.MonitoringPointUid=d.MonitoringPointUid
//    inner join [AMS_MonitorBusiness].[dbo].[TB_CardNumber] e on 
//    b.TeamGuid=e.TeamUid
//    inner join [EQMS_Framework].dbo.TB_OMMP_MaintenanceTeam f
//    on  b.TeamGuid=f.RowGuid
//    where c.ApplicationUid='{0}' 
//    ) T 
//    where T.TeamGuid IN ({1}) 
//     and T.SigninTime>=CONVERT(DATETIME,'{2}',120) AND T.SigninTime<=CONVERT(DATETIME,'{3}',120) AND T.SigninTime IS NOT NULL
//    and T.PointId IN ({4})
//     group by T.ObjectID,T.TeamName,T.TeamGuid,CONVERT(NVARCHAR(19),
//      T.SignInTime,23),T.MonitoringPointUid,T.PointId,T.PointName,T.mn,T.CodeNum 
//    union all  
// select T.ObjectID,T.TeamUid,T.TeamName, CONVERT(NVARCHAR(19), T.StationDate,23) as DT,T.MonitoringPointUid,T.PointId,T.PointName,T.mn,T.CodeNum 
//  ,
//  (select ISNULL(sum(1),0)from V_Water_AccessInformation_SignTimes where TeamGuid=T.TeamUid and PointId=T.PointId 
//and CONVERT(NVARCHAR(19),StationDate,23)=CONVERT(NVARCHAR(19),T.StationDate,23)
//  ) CardCnt,0 AS SignCnt
//  FROM(
//select c.MonitoringPointUid ObjectID,e.TeamUid,F.StationDate, c.MonitoringPointUid,c.PointId,c.MonitoringPointName PointName,d.mn,e.CodeNum,g.TeamName from 
//
//   AMS_BaseData.MPInfo.TB_MonitoringPoint c
//   inner join [AMS_BaseData].[MPInfo].[TB_AcquisitionInstrument] d
//    on c.MonitoringPointUid=d.MonitoringPointUid
//    
//    INNER JOIN V_Water_AccessInformation_SignTimes F
//    ON F.PointId=C.PointId
//      inner join [AMS_MonitorBusiness].[dbo].[TB_CardNumber] e on 
//    F.CardNumber=e.CodeNum
//     inner join [EQMS_Framework].dbo.TB_OMMP_MaintenanceTeam g
//    on  e.TeamUid=g.RowGuid
//    where c.ApplicationUid='{0}' 
//    ) T 
//    where T.TeamUid IN ({1})
//   and T.StationDate >=CONVERT(DATETIME,'{2}',120) AND StationDate<=CONVERT(DATETIME,'{3}',120) AND T.StationDate IS NOT NULL 
//    and T.PointId IN ({4})
//     group by T.ObjectID,T.TeamName,T.TeamUid,CONVERT(NVARCHAR(19),
//      T.StationDate,23),T.MonitoringPointUid,T.PointId,T.PointName,T.mn,T.CodeNum ) T2
//      GROUP BY T2.TeamGuid,T2.TeamName,T2.PointId,T2.MonitoringPointUid,T2.PointName,T2.DT,T2.PointName,T2.MN,T2.CodeNum", sysType, RowGuid, beginDate, endDate, portId);

//            string sql2 = string.Empty;
//            sql2 = string.Format(@"select '密码' type , '' as TeamGuid,RegisterName as TeamName,B.PointId as PointId,B.MonitoringPointUid as MonitoringPointUid,CONVERT(NVARCHAR(19),StationDate,23) as DT,B.MonitoringPointName as PointName,d.MN as MN,  CardNumber as CodeNum,COUNT(CardNumber) as N0,0 as N1,0 as N2
//from [AMS_MonitorBusiness].[dbo].[TB_AccessInformation] A,[AMS_BaseData].[MPInfo].[TB_MonitoringPoint] B  left join [AMS_BaseData].[MPInfo].[TB_AcquisitionInstrument] d
//    on B.MonitoringPointUid=d.MonitoringPointUid
//where A.PointId = B.PointId and RegisterName='{4}' and StationDate >=CONVERT(DATETIME,'{1}',120) AND StationDate<=CONVERT(DATETIME,'{2}',120) AND StationDate IS NOT NULL 
//    and A.PointId IN ({3}) group by RegisterName,b.PointId,B.MonitoringPointUid,CONVERT(NVARCHAR(19),StationDate,23),B.MonitoringPointName ,d.MN,CardNumber", sysType, beginDate, endDate, portId, rcbMIma);

//            string sql3 = string.Empty;
//            sql3 = string.Format(@"select '刷卡' type , '' as TeamGuid,RegisterName as TeamName,B.PointId as PointId,B.MonitoringPointUid as MonitoringPointUid,CONVERT(NVARCHAR(19),StationDate,23) as DT,B.MonitoringPointName as PointName,d.MN as MN,  CardNumber as CodeNum,0 as N0,COUNT(CardNumber) as N1,0 as N2
//from [AMS_MonitorBusiness].[dbo].[TB_AccessInformation] A,[AMS_BaseData].[MPInfo].[TB_MonitoringPoint] B  left join [AMS_BaseData].[MPInfo].[TB_AcquisitionInstrument] d
//    on B.MonitoringPointUid=d.MonitoringPointUid
//where A.PointId = B.PointId and RegisterName='{4}' and StationDate >=CONVERT(DATETIME,'{1}',120) AND StationDate<=CONVERT(DATETIME,'{2}',120) AND StationDate IS NOT NULL 
//    and A.PointId IN ({3}) group by RegisterName,b.PointId,B.MonitoringPointUid,CONVERT(NVARCHAR(19),StationDate,23),B.MonitoringPointName ,d.MN,CardNumber", sysType, beginDate, endDate, portId, rcbWeizhi);

//            return g_DatabaseHelper.ExecuteDataTable("select * from (" + sql + " union all " + sql2 + " union all " + sql3 + ") Tmp order by DT desc", connection);

            sql = string.Format(@"SELECT RegisterName,MonitoringPointName,StationWay,StationDate,CardNumber,密码登陆,刷卡次数,签到次数,MN,CONVERT(NVARCHAR(19), StationDate,23) as DT
                                FROM
                                (SELECT *
                                FROM
                                (SELECT A.PointId,A.RegisterName,B.MonitoringPointName,B.MonitoringPointUid,A.StationWay,A.StationDate,A.CardNumber,SUM(CASE 密码登录 WHEN 1 THEN 1 ELSE 0 END) 密码登陆,SUM(CASE 刷卡次数 WHEN 1 THEN 1 ELSE 0 END) 刷卡次数,0 签到次数
                                FROM (SELECT StationWay,PointId,RegisterName,CardNumber,CONVERT(NVARCHAR(19),StationDate,23) StationDate,(CASE StationWay WHEN '密码' THEN 1 ELSE 0 END) 密码登录,(CASE StationWay WHEN '刷卡' THEN 1 ELSE 0 END) 刷卡次数 
                                from [AMS_MonitorBusiness].[dbo].[TB_AccessInformation]) A left join [AMS_BaseData].[MPInfo].[TB_MonitoringPoint] B on A.PointId =B.PointId 
                                GROUP BY A.CardNumber,A.StationDate,A.RegisterName,B.MonitoringPointName,A.StationWay,A.PointId,B.MonitoringPointUid
                                union all
                                select G.PointId,D.TeamName RegisterName,G.MonitoringPointName,G.MonitoringBussinessUid,'Pad签到' StationWay,CONVERT(NVARCHAR(19),C.SignInTime,23) StationDate,E.CodeNum CardNumber,0 密码登录,0 刷卡次数,COUNT(*) 签到次数
                                from [EQMS_Framework].[dbo].[TB_OMMP_AttendanceInObject] C left join [EQMS_Framework].[dbo].[TB_OMMP_MaintenanceTeam] D on C.TeamGuid = D.RowGuid 
	                                left join [AMS_MonitorBusiness].[dbo].[TB_CardNumber] E on E.TeamUid = C.TeamGuid left join [EQMS_Framework].[dbo].[TB_OMMP_MaintenanceObject] F on C.MaintenanceObjectGuid = F.RowGuid
	                                left join [AMS_BaseData].[MPInfo].[TB_MonitoringPoint] G on G.MonitoringPointUid = F.ObjectID
                                group by CONVERT(NVARCHAR(19),C.SignInTime,23),D.TeamName,E.CodeNum,G.PointId,G.MonitoringPointName,G.MonitoringBussinessUid) M ) N
                                left join [AMS_BaseData].[MPInfo].[TB_AcquisitionInstrument] H on N.MonitoringPointUid=H.MonitoringPointUid
                                WHERE StationDate >=CONVERT(DATETIME,'{0}',120) AND StationDate<=CONVERT(DATETIME,'{1}',120) AND CardNumber IN ({3}) AND PointId IN ({2})
                                ORDER BY StationDate DESC,CardNumber", beginDate, endDate, portId, RowGuid2);
            return g_DatabaseHelper.ExecuteDataTable(sql, connection);
        }

        /// <summary>
        /// 返回DataView
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="strConnectString"></param>
        /// <returns></returns>
        public DataView CreatDataView(String strSql, String strConnectString)
        {
            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings[strConnectString].ConnectionString);
            SqlDataAdapter myCommand = new SqlDataAdapter(strSql, myConn);
            DataTable dt = new DataTable();
            myCommand.Fill(dt);
            return dt.DefaultView;
        }
    }
}
