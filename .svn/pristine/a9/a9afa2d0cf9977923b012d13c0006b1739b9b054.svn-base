using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.MonitoringBusiness.Water
{
    /// <summary>
    /// 蓝藻数据
    /// </summary>
    public class BlueAlgaeDAL
    {
        /// <summary>
        /// 获取站点信息
        /// </summary>
        /// <param name="PointType">点位类型</param>
        /// <returns></returns>
        public string GetPortInfo(string userguid)
        {
            if (string.IsNullOrWhiteSpace(userguid))
            {
                userguid = System.Configuration.ConfigurationManager.AppSettings["UserGuid"];
                if (string.IsNullOrWhiteSpace(userguid))
                {
                    userguid = "fa5400b1-2472-42ee-8856-9bb63815b7ac";
                }
            }
            string result = string.Empty;
            IList<string> list = new List<string>();
            string strWhere = " where 1=1 ";
            strWhere += " and a.ApplicationUid='watrwatr-watr-watr-watr-watrwatrwatr' ";
            if (userguid != "")
            {
                strWhere += "  and UserGuid='" + userguid + "'";
            }
            string sqlPort = @"  select a.PointId,a.monitoringPointName, BaiduX as x, BaiduY as y,itemValue,itemText,c.IsOnline,a.CustomX,a.CustomY,
                                case a.ApplicationUid when 'airaaira-aira-aira-aira-airaairaaira' then '22' 
	                                when 'watrwatr-watr-watr-watr-watrwatrwatr' then '21' end as ST 
                                from dbo.V_Point_UserConfig as d left join 
                                 MPInfo.TB_MonitoringPoint as a  on a.PointId=d.PortId
                                inner join dbo.SY_View_CodeMainItem as b on a.BlueAlgaeRegionUid=b.itemGuid and a.EnableOrNot=1 
                                 left join dbo.SY_DataOnline as c on a.MonitoringPointUid=c.MonitoringPointUid and  c.[DataTypeUid]='1b6367f1-5287-4c14-b120-7a35bd176db1'
                                ";
            string orderby = " order by a.BluedOrderNUm  ";
            DataView dvPort = CreatDataView(sqlPort + strWhere + orderby, "AMS_BaseDataConnection");
            if (dvPort.Table.Rows.Count > 0)
            {
                for (int i = 0; i < dvPort.Table.Rows.Count; i++)
                {
                    string AlgaeDensity = string.Empty;
                    string portId = dvPort.Table.Rows[i][0].ToString();
                    string portName = dvPort.Table.Rows[i][1].ToString();
                    string portType = dvPort.Table.Rows[i]["ItemText"].ToString();
                    string attentionType = dvPort.Table.Rows[i]["itemValue"].ToString();
                    string siteTypeText = dvPort.Table.Rows[i]["ItemText"].ToString();
                    string ST = dvPort.Table.Rows[i]["ST"].ToString();
                    string Level = "--";
                    string EvaluateFactor = "--";
                    //取得查询行转列字段拼接
                    string sqlStr = " SELECT ";
                    string tableNameReport = " From dbo.SY_Water_InfectantBy60";
                    string factorSql = string.Empty;
                    factorSql += string.Format(",dbo.F_Round(MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ),(select MAX(DecimalDigit) from Standard.TB_PollutantCode where PollutantCode='{0}' ))  AS [{0}] ", "w19011");
                    string fieldName = " top 1 PointId,Tstamp" + factorSql;
                    string groupBy = " Group By PointId,Tstamp";
                    string whereStr = string.Format(" where PointId = {0}", portId);
                    sqlStr += fieldName + tableNameReport + whereStr + groupBy + " order by PointId ,Tstamp desc";
                    DataTable dvLevel = CreatDataView(sqlStr, "AMS_BaseDataConnection").Table;
                    if (dvLevel.Rows.Count > 0)
                    {
                        string pollutantValue = dvLevel.Rows[0]["w19011"].ToString();//污染物浓度
                        if (!string.IsNullOrEmpty(pollutantValue))
                        {
                            decimal value = Convert.ToDecimal(pollutantValue);
                            if (value <= 30)
                            {
                                pollutantValue = "30";
                            }
                            Level = GetBlueWQL(value);
                            EvaluateFactor = "藻密度";
                        }
                        AlgaeDensity = pollutantValue;
                    }
                    string IsOnline = "1";
                    if (siteTypeText == "人工监测点")
                        portType = "2";
                    string x = dvPort.Table.Rows[i]["x"].ToString();
                    string y = dvPort.Table.Rows[i]["y"].ToString();
                    string CustomX = dvPort.Table.Rows[i]["CustomX"].ToString();
                    string CustomY = dvPort.Table.Rows[i]["CustomY"].ToString();
                    if (Level == "")
                    {
                        Level = "--";
                    }
                    if (EvaluateFactor == "")
                    {
                        EvaluateFactor = "--";
                    }
                    //arr[i] = "{\"PortId\":\"" + portId + "\",\"PortName\":\"" + portName + "\",\"ST\":\"" + ST + "\",\"PortType\":\"" + portType + "\",\"attentionType\":\"" + attentionType + "\",\"X\":\"" + x + "\",\"Y\":\"" + y + "\",\"isShowName\":\"1\"}";
                    list.Add("{\"PortId\":\"" + portId + "\",\"PortName\":\"" + portName + "\",\"ST\":\"" + ST + "\",\"PortType\":\"" + portType + "\",\"attentionType\":\"" + attentionType + "\",\"X\":\"" + x + "\",\"Y\":\"" + y + "\",\"isShowName\":\"1\",\"IsOnline\":\"" + IsOnline + "\",\"CustomX\":\"" + CustomX + "\",\"CustomY\":\"" + CustomY + "\",\"Level\":\"" + Level + "\",\"EvaluateFactor\":\"" + EvaluateFactor + "\",\"AlgaeDensity\":\"" + AlgaeDensity + "\"}");
                }
            }
            result = "{\"PortInfo\":" + ToListStringNew(list) + "}";//ToArrayStringNew(list)
            return result;
        }
        /// <summary>
        /// 获取藻密度小时图
        /// </summary>
        /// <param name="userguid"></param>
        /// <returns></returns>
        public DataTable GetAlgaeImg(string userguid)
        {
            if (string.IsNullOrWhiteSpace(userguid))
            {
                userguid = System.Configuration.ConfigurationManager.AppSettings["UserGuid"];
                if (string.IsNullOrWhiteSpace(userguid))
                {
                    userguid = "fa5400b1-2472-42ee-8856-9bb63815b7ac";
                }
            }
            string sql = string.Format(@"with tableHour as (
                                                select b.MonitoringPointUid as PointGuid , PortId as PointID  , b.MonitoringPointName as PointName ,c.Tstamp,c.PollutantValue
                                                 from [dbo].[V_Point_UserConfig] as a left join [MPInfo].[TB_MonitoringPoint] as b on a.PortId=b.PointId 
                                                 left join  (select a.PointId,a.Tstamp,b.PollutantValue  from
                                                (SELECT [PointId],MAX([Tstamp]) as [Tstamp] FROM dbo.SY_Water_InfectantBy60 where PollutantCode='w19011' group by PointId) as a 
                                                    left join dbo.SY_Water_InfectantBy60 as b 
                                                    on a.PointId=b.PointId and a.Tstamp=b.Tstamp
                                                    where PollutantCode='w19011' 
                                                 group by a.PointId,a.Tstamp,b.PollutantValue) as c
                                                  on a.PortId=c.PointId
                                                  where UserGuid='{0}' and a.[ApplicationUid]='watrwatr-watr-watr-watr-watrwatrwatr')
                                                  select  newid() as guid
                                                         ,Max(Tstamp) as Tstamp
                                                         ,COUNT(PointID) as portCount
                                                         ,sum(case when [PollutantValue]<200  then 1 else 0 end) as '<200' 
                                                         ,sum(case when [PollutantValue]<500 and [PollutantValue]>=200 then 1 else 0 end) as '≥200' 
                                                         ,sum(case when [PollutantValue]<1000 and [PollutantValue]>=500 then 1 else 0 end) as '≥500' 
                                                         ,sum(case when [PollutantValue]<3000 and [PollutantValue]>=1000  then 1 else 0 end) as '≥1000'
                                                        ,sum(case when [PollutantValue]<5000 and [PollutantValue]>=3000  then 1 else 0 end) as '≥3000'
                                                        ,sum(case when [PollutantValue]<8000 and [PollutantValue]>=5000  then 1 else 0 end) as '≥5000'
                                                        ,sum(case when [PollutantValue]<10000 and [PollutantValue]>=8000  then 1 else 0 end) as '≥8000' 
                                                         ,sum(case when [PollutantValue]>=10000   then 1 else 0 end) as '≥10000' 
                                                    from  tableHour", userguid);
            DataTable dt = CreatDataView(sql, "AMS_BaseDataConnection").Table;
            return dt;
        }
        /// <summary>
        /// 获取日均值变化浓度
        /// </summary>
        /// <param name="PointId"></param>
        /// <param name="dtstart"></param>
        /// <param name="dtend"></param>
        /// <returns></returns>
        public DataTable GetDayAvg(string PointId, string dtstart, string dtend)
        {
            DateTime StartTime = DateTime.Now.Date.AddDays(-7);
            DateTime EndTime = DateTime.Now.Date;
            if (DateTime.TryParse(dtstart, out StartTime) && DateTime.TryParse(dtend, out EndTime))
            {
                StartTime = DateTime.Parse(dtstart);
                EndTime = DateTime.Parse(dtend);
            }
            else if (DateTime.TryParse(dtend, out EndTime))
            {
                EndTime = DateTime.Parse(dtend);
                StartTime = EndTime.Date.AddDays(-7);
            }
            else if (DateTime.TryParse(dtstart, out StartTime))
            {
                StartTime = DateTime.Parse(dtstart);
                EndTime = StartTime.Date.AddDays(+7);
            }
            else
            {
                StartTime = DateTime.Now.Date.AddDays(-7);
                EndTime = DateTime.Now.Date;
            }
            string sql = string.Format(@"SELECT [DateTime]
                                                ,dbo.F_Round([PollutantValue],(select MAX(DecimalDigit) from dbo.SY_PollutantCode where PollutantCode='w19011' )) as PollutantValue
                                            FROM [WaterReport].[TB_DayReport]
                                            where PointId={0} and PollutantCode='w19011' and DateTime>='{1}'
                                                    and DateTime<='{2}'
                                            order by DateTime ", PointId, StartTime, EndTime);
            DataTable dt = CreatDataView(sql, "AMS_MonitoringBusinessConnection").Table;
            return dt;
        }
        public DataTable GetWQLDayReport(string factorcode, string pointids)
        {
            //try
            //{
            string UserGuid = string.Empty;
            string sqlWhere = string.Empty;
            string portWhere = string.Empty;
            if (string.IsNullOrWhiteSpace(pointids))
            {
                UserGuid = System.Configuration.ConfigurationManager.AppSettings["UserGuid"];
                if (string.IsNullOrWhiteSpace(UserGuid))
                {
                    UserGuid = "fa5400b1-2472-42ee-8856-9bb63815b7ac";
                }
                sqlWhere = string.Format(@" where UserGuid='{0}' ", UserGuid);
            }
            else
            {
                portWhere = string.Format(@" where PointId={0} ", pointids);
            }
            string jsonStr = string.Empty;
            string message = string.Empty;
            bool isSucceed = false;
            DateTime dtstart = DateTime.Now.AddHours(-24);
            DateTime dtend = DateTime.Now;
            string sql = string.Empty;
            if (factorcode == "w19011")
            {
                sql = string.Format(@"with tableHour as (select [PortId] as PointId,Value_Avg,Value_Max,Value_Min,d.BluedOrderNUm as BluedOrderNUm
                                            FROM [dbo].[V_Point_UserConfig] as a
                                            left join MPInfo.TB_MonitoringPoint as d on a.ParameterUid=d.MonitoringPointUid 
                                            left join 
                                            ( select PointId, dbo.F_Round(AVG(PollutantValue),(select MAX(DecimalDigit) from Standard.TB_PollutantCode where PollutantCode='w19011' ))  as Value_Avg
                                            , dbo.F_Round(MAX(PollutantValue),(select MAX(DecimalDigit) from Standard.TB_PollutantCode where PollutantCode='w19011' ))  as Value_Max
                                            ,  dbo.F_Round(MIN(PollutantValue),(select MAX(DecimalDigit) from Standard.TB_PollutantCode where PollutantCode='w19011' ))  as Value_Min 
                                            from dbo.SY_Water_InfectantBy60
                                            where PollutantCode='{0}' and Tstamp>='{1}' and Tstamp<='{2}'  and PollutantValue>1 and PollutantValue<>7999 and Status='N'and PollutantValue<300000000
                                             group by PointId )as b on a.PortId=b.PointId
                                              {3})
                                             select * from tableHour {4}  group by PointId,Value_Avg,Value_Max,Value_Min,BluedOrderNUm  order by BluedOrderNUm", factorcode, dtstart, dtend, sqlWhere, portWhere);
            }
            else if (factorcode == "w01010")
            {
                sql = string.Format(@"with tableHour as (select [PortId] as PointId,Value_Avg,Value_Max,Value_Min,d.BluedOrderNUm as BluedOrderNUm
                                            FROM [dbo].[V_Point_UserConfig] as a
                                            left join MPInfo.TB_MonitoringPoint as d on a.ParameterUid=d.MonitoringPointUid 
                                            left join 
                                            ( select PointId, dbo.F_Round(AVG(PollutantValue),(select MAX(DecimalDigit) from Standard.TB_PollutantCode where PollutantCode='{0}' ))  as Value_Avg
                                            ,dbo.F_Round(MAX(PollutantValue),(select MAX(DecimalDigit) from Standard.TB_PollutantCode where PollutantCode='{0}' ))  as Value_Max
                                            , dbo.F_Round(MIN(PollutantValue),(select MAX(DecimalDigit) from Standard.TB_PollutantCode where PollutantCode='{0}' ))  as Value_Min 
                                            from dbo.SY_Water_InfectantBy60
                                            where PollutantCode='{0}' and Tstamp>='{1}' and Tstamp<='{2}'
                                             group by PointId )as b on a.PortId=b.PointId
                                              {3})
                                             select * from tableHour {4}  group by PointId,Value_Avg,Value_Max,Value_Min,BluedOrderNUm  order by BluedOrderNUm", factorcode, dtstart, dtend, sqlWhere, portWhere);
            }
            else
            {
                sql = string.Format(@"with tableHour as (select [PortId] as PointId,Value_Avg,Value_Max,Value_Min,d.BluedOrderNUm as BluedOrderNUm
                                            FROM [dbo].[V_Point_UserConfig] as a
                                            left join MPInfo.TB_MonitoringPoint as d on a.ParameterUid=d.MonitoringPointUid 
                                            left join 
                                            (select PointId, dbo.F_Round(AVG(PollutantValue),(select MAX(DecimalDigit) from Standard.TB_PollutantCode where PollutantCode='{0}' ))  as Value_Avg
                                            , dbo.F_Round(MAX(PollutantValue),(select MAX(DecimalDigit) from Standard.TB_PollutantCode where PollutantCode='{0}' ))  as Value_Max
                                            , dbo.F_Round(MIN(PollutantValue),(select MAX(DecimalDigit) from Standard.TB_PollutantCode where PollutantCode='{0}' ))  as Value_Min 
                                            from dbo.SY_Water_InfectantBy60
                                            where PollutantCode='{0}' and Tstamp>='{1}' and Tstamp<='{2}' and PollutantValue>0  
                                             group by PointId )as b on a.PortId=b.PointId
                                              {3})
                                             select * from tableHour {4}  group by PointId,Value_Avg,Value_Max,Value_Min,BluedOrderNUm  order by BluedOrderNUm", factorcode, dtstart, dtend, sqlWhere, portWhere);
            }
            DataTable dvLevel = CreatDataView(sql, "AMS_BaseDataConnection").Table;
            //if (dvLevel != null && dvLevel.Rows.Count > 0)
            //{
            //    jsonStr = dvLevel.ToJsonBySerialize();
            //    isSucceed = true;
            //}
            return dvLevel;
            //}
            //catch (Exception ex)
            //{
            //    return "{\"PortInfo\":{\"IsSuccess\":\"false\",\"Msg\":\"" + ex.Message + "\",\"Data\":[]}}";
            //}
        }
        /// <summary>
        /// 藻密度 水质
        /// </summary>
        /// <param name="BlueC"></param>
        /// <returns></returns>
        public string GetBlueWQL(decimal BlueC)
        {
            string level = string.Empty;
            if (BlueC < 200)
            {
                level = "1";
            }
            else if (BlueC < 1000)
            {
                level = "2";
            }
            else if (BlueC < 5000)
            {
                level = "3";
            }
            else if (BlueC < 10000)
            {
                level = "4";
            }
            else
            {
                level = "5";
            }
            return level;
        }
        /// <summary>   
        /// 普通集合转换Json   
        /// </summary>   
        /// <param name="list">集合对象</param>   
        /// <returns>Json字符串</returns>   
        public static string ToListStringNew(IList<string> list)
        {
            string jsonString = "[";
            for (int i = 0; i < list.Count; i++)
            {
                if (i == 0)
                {
                    jsonString += list[i];
                }
                else
                {
                    jsonString += "," + list[i];
                }

            }
            return jsonString + "]";
        }

        /// <summary>
        /// 获取数据集
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <param name="strConnectString">数据库链接字符串</param>
        /// <returns></returns>
        public DataView CreatDataView(String strSql, String strConnectString)
        {
            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings[strConnectString].ConnectionString);
            SqlDataAdapter myCommand = new SqlDataAdapter(strSql, myConn);
            try
            {
                if (ConfigurationManager.AppSettings["SqlCmdTimeOut"] != null && ConfigurationManager.AppSettings["SqlCmdTimeOut"].ToString() != "")
                {
                    myCommand.SelectCommand.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["SqlCmdTimeOut"]);
                }
                else
                {
                    myCommand.SelectCommand.CommandTimeout = 600;
                }
                DataTable dt = new DataTable();
                myCommand.Fill(dt);
                return dt.DefaultView;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                myCommand.Dispose();
                myConn.Close();
                myConn.Dispose();
            }

        }
    }
}
