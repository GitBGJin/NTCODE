﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using AQIWS.Common;
using System.Text;
using System.Xml;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using System.Configuration;
using System.Web.Security;
using SmartEP.Utilities.DataTypes.ExtensionMethods;

namespace AQIWS
{
    /// <summary>
    /// AQI 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://218.91.209.251:1117")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    [System.Web.Script.Services.ScriptService]
    public class AQI : System.Web.Services.WebService
    {
        private string AMS_BaseData_Conn = "AMS_BaseData_Conn";

        private const string AMS_Framework = "FrameWork_Conn";

        private const string AMS_AirAutoMonitor_Conn = "AMS_AirAutoMonitor_Conn";

        private const string AMS_WaterAutoMonitor_Conn = "AMS_WaterAutoMonitor_Conn";

        private const string AMS_MonitorBusiness_Conn = "AMS_MonitorBusiness_Conn";

        /// <summary>
        /// 气站点类型标识
        /// </summary>
        private const string APP_UID_AIR = "airaaira-aira-aira-aira-airaairaaira";

        /// <summary>
        /// 水站点类型标识
        /// </summary>
        private const string APP_UID_WATER = "watrwatr-watr-watr-watr-watrwatrwatr";

        /// <summary>
        /// 气因子类型标识
        /// </summary>
        private const string POLLUTANT_UID_AIR = "8D89B62D-36E1-4F05-B00D-3A585F6A90D7";

        /// <summary>
        /// 水因子类型标识
        /// </summary>
        private const string POLLUTANT_UID_WATER = "80CA99DE-3B78-422F-9BAA-D47F23324231";

        /// <summary>
        /// 气请求类型标识
        /// </summary>
        private const string SYS_TYPE_AIR = "air";

        /// <summary>
        /// 水请求类型标识
        /// </summary>
        private const string SYS_TYPE_WATER = "water";

        /// <summary>
        /// 苏州市AQI数据UID配置项
        /// </summary>
        private String monitorRegionUid = System.Configuration.ConfigurationManager.AppSettings["CityAQI_MonitoringRegionUid"].ToString();

        #region 环境质量移动平台专业版服务

        /// <summary>
        /// {"PortInfo":[{"PortId":"0","PortName":"吴江区","X":"0","Y":"0","PortType":"吴江区","RegionType":"--","orderNumber":"--"},{"PortId":"2","PortName":"吴江区环保局","X":"120.6459","Y":"31.1436","PortType":"监测点","RegionType":"松陵镇","orderNumber":"1"},{"PortId":"4","PortName":"教师进修学校","X":"120.6386","Y":"31.1675","PortType":"监测点","RegionType":"松陵镇","orderNumber":"2"},{"PortId":"5","PortName":"开发区","X":"120.6643","Y":"31.1630","PortType":"监测点","RegionType":"松陵镇","orderNumber":"3"}]}
        /// </summary>
        /// <param name="sysType"></param>
        [WebMethod(Description = @"【站点信息】获取站点信息<br />参数说明：<br />sysType为系统类型，air为环境空气、water为地表水<br />LoginId为登录人账号:admin")]
        //        public void GetPortInfoBySysType(string sysType, string LoginId)
        //        {
        //            string appUid = SYS_TYPE_AIR.Equals(sysType.ToLower()) ? APP_UID_AIR : APP_UID_WATER;

        //            string jsonStr = string.Empty;

        //            try
        //            {
        //                string querySql = string.Format(@"
        //                            select PointId as PortId,
        //                                  mt.MonitoringPointName as PortName,
        //                                   X,
        //                                   Y,
        //                                   svcST.ItemText as PortType,
        //                                   svcRG.ItemText as RegionType, 
        //                                   mt.OrderByNum as orderNumber 
        //                              from [MPInfo].[TB_MonitoringPoint] mt
        //                             inner join dbo.SY_View_CodeMainItem svcST
        //                                on mt.SiteTypeUid = svcST.ItemGuid
        //                             inner join dbo.SY_View_CodeMainItem svcRG
        //                                on mt.RegionUid = svcRG.ItemGuid
        //                             inner join dbo.V_Point_UserConfig as d
        //                                  on mt.PointId=d.PortId
        //                             where mt.ApplicationUid = '{0}'
        //                               and mt.EnableOrNot = 1 and LoginID='{1}'
        //                             order by mt.OrderByNum desc"
        //                             , appUid, LoginId);

        //                DataTable portDt = DBHelper.GetDataView(querySql, null, DBHelper.GetConnectionString(AMS_BaseData_Conn));

        //                portDt.Columns.Add("AQI", typeof(string));
        //                portDt.Columns.Add("Grade", typeof(string));
        //                foreach (DataRow dr in portDt.Rows)
        //                {
        //                    string pid = dr["PortId"].ToString();
        //                    DataView dv = GetMsg(pid);
        //                    if (dv.Count > 0)
        //                    {
        //                        dv.Sort = "DateTime desc";
        //                        dr["AQI"] = dv[0]["AQI"];
        //                        dr["Grade"] = dv[0]["Class"];
        //                    }

        //                }
        //                if (SYS_TYPE_AIR.Equals(sysType.ToLower()))
        //                {
        //                    DataRow newRow = portDt.NewRow();
        //                    newRow["PortId"] = "0";
        //                    newRow["PortName"] = "南通市";
        //                    newRow["X"] = 0;
        //                    newRow["Y"] = 0;
        //                    newRow["PortType"] = "南通市";

        //                    portDt.Rows.InsertAt(newRow, 0);
        //                }

        //                StringBuilder jsonSb = new StringBuilder();

        //                foreach (System.Data.DataRowView row in portDt.DefaultView)
        //                {
        //                    if (jsonSb.Length != 0)
        //                    {
        //                        jsonSb.Append(",");
        //                    }

        //                    StringBuilder colSb = new StringBuilder();
        //                    foreach (System.Data.DataColumn col in portDt.Columns)
        //                    {
        //                        if (colSb.Length != 0)
        //                        {
        //                            colSb.Append(",");
        //                        }

        //                        colSb.AppendFormat(@"""{0}"":""{1}""", col.ColumnName, System.Convert.IsDBNull(row[col.ColumnName]) ? "--" : row[col.ColumnName]);
        //                    }

        //                    jsonSb.AppendFormat("{{{0}}}", colSb);
        //                }

        //                jsonStr = string.Format(@"{{""PortInfo"":[{0}]}}", jsonSb);
        //            }
        //            catch (Exception e)
        //            {
        //                CommonFunction.WriteErrorLog("When GetPortInfoBySysType SysType=[" + sysType + "] Error: " + e.Message);
        //            }

        //            CommonFunction.WriteInfoLog("Return GetPortInfoBySysType SysType=[" + sysType + "] Result=[" + jsonStr + "]");

        //            ReturnJson(jsonStr);
        //        }
        public void GetPortInfoBySysType(string sysType, string LoginId)
        {
            string appUid = SYS_TYPE_AIR.Equals(sysType.ToLower()) ? APP_UID_AIR : APP_UID_WATER;

            string jsonStr = string.Empty;

            try
            {
                string querySql = string.Format(@"
                            select PointId as PortId,
                                  mt.MonitoringPointName as PortName,
                                   X,
                                   Y,
                                   svcST.ItemText as PortType,
                                   svcRG.ItemText as RegionType, 
                                   mt.OrderByNum as orderNumber 
                              from [MPInfo].[TB_MonitoringPoint] mt
                             inner join dbo.SY_View_CodeMainItem svcST
                                on mt.SiteTypeUid = svcST.ItemGuid
                             inner join dbo.SY_View_CodeMainItem svcRG
                                on mt.RegionUid = svcRG.ItemGuid
                             inner join dbo.V_Point_UserConfig as d
                                  on mt.PointId=d.PortId
                             where mt.ApplicationUid = '{0}'
                               and mt.EnableOrNot = 1 and LoginID='{1}'
                             order by svcRG.sortNumber desc,mt.OrderByNum desc"
                             , appUid, LoginId);

                DataTable portDt = DBHelper.GetDataView(querySql, null, DBHelper.GetConnectionString(AMS_BaseData_Conn));

                portDt.Columns.Add("AQI", typeof(string));
                portDt.Columns.Add("PrimaryPollutant", typeof(string));
                portDt.Columns.Add("Grade", typeof(string));
                foreach (DataRow dr in portDt.Rows)
                {
                    string pid = dr["PortId"].ToString();
                    DataView dv = GetMsg(pid);
                    if (dv.Count > 0)
                    {
                        dv.Sort = "DateTime desc";
                        dr["AQI"] = dv[0]["AQI"];
                        dr["Grade"] = dv[0]["Class"];
                    }

                }
                if (SYS_TYPE_AIR.Equals(sysType.ToLower()))
                {
                    DataView dv = GetMsg("0");
                    DataRow newRow = portDt.NewRow();
                    newRow["PortId"] = "0";
                    newRow["PortName"] = "南通市";
                    newRow["X"] = 0;
                    newRow["Y"] = 0;
                    newRow["PortType"] = "南通市";
                    newRow["RegionType"] = "南通市";
                    newRow["AQI"] = dv[dv.Count - 1]["AQI"];
                    newRow["PrimaryPollutant"] = dv[dv.Count - 1]["PrimaryPollutant"];
                    newRow["Grade"] = dv[dv.Count - 1]["Class"];
                    portDt.Rows.InsertAt(newRow, 0);
                }

                StringBuilder jsonSb = new StringBuilder();

                foreach (System.Data.DataRowView row in portDt.DefaultView)
                {
                    if (jsonSb.Length != 0)
                    {
                        jsonSb.Append(",");
                    }

                    StringBuilder colSb = new StringBuilder();
                    foreach (System.Data.DataColumn col in portDt.Columns)
                    {
                        if (colSb.Length != 0)
                        {
                            colSb.Append(",");
                        }

                        colSb.AppendFormat(@"""{0}"":""{1}""", col.ColumnName, System.Convert.IsDBNull(row[col.ColumnName]) ? "--" : row[col.ColumnName]);
                    }

                    jsonSb.AppendFormat("{{{0}}}", colSb);
                }

                jsonStr = string.Format(@"{{""PortInfo"":[{0}]}}", jsonSb);
            }
            catch (Exception e)
            {
                CommonFunction.WriteErrorLog("When GetPortInfoBySysType SysType=[" + sysType + "] Error: " + e.Message);
            }

            CommonFunction.WriteInfoLog("Return GetPortInfoBySysType SysType=[" + sysType + "] Result=[" + jsonStr + "]");

            ReturnJson(jsonStr);
        }


        public DataView GetMsg(string portId)
        {
            string jsonStr = string.Empty;
            string topN = "24";
            string tableName = "[AMS_AirAutoMonitor].[Air].[TB_OriHourAQI]";
            if ("0".Equals(portId))
            {
                tableName = "[AMS_AirAutoMonitor].[Air].[TB_OriRegionHourAQI]";
            }
            string sql = string.Format(@"
                                                 select dateadd(hh,0,datetime) DateTime,
                                                        AQIValue AQI,
                                                        PrimaryPollutant,
                                                        RGBValue,
                                                        Class,
                                                        Grade,
                                                        HealthEffect,
                                                        TakeStep 
                                                   from {0} 
                                                  where {1} 
                                                    and dateTime > DATEADD(HOUR, -({2}), GETDATE())
                                                  order by dateTime asc"
                                       , tableName
                                       , "0".Equals(portId)
                                              ? string.Format("MonitoringRegionUid = '{0}' ", monitorRegionUid)
                                              : string.Format("PointId = {0} ", portId)
                                       , topN);
            DataTable dt = DBHelper.GetDataView(sql, null, DBHelper.GetConnectionString(AMS_MonitorBusiness_Conn));
            return dt.AsDataView();
        }

        /// <summary>
        /// {"FactorList":[{"name":"PM2.5"},{"name":"PM10"},{"name":"O3"},{"name":"NO2"},{"name":"SO2"},{"name":"CO"}]}
        /// </summary>
        /// <param name="sysType"></param>
        [WebMethod(Description = @"【空气实况/实时水质】获取因子可选列表<br />sysType:系统类型<br /> air：空气/water：地表水<br />LoginId为登录人账号:admin")]
        public void GetFactorList(string sysType, string LoginId)
        {
            string appUid = SYS_TYPE_AIR.Equals(sysType.ToLower()) ? APP_UID_AIR : APP_UID_WATER;
            string jsonStr = string.Empty;

            try
            {
                StringBuilder jsonSb = new System.Text.StringBuilder();
                //                string querySql = string.Format(@"
                //                                                   select pc.PollutantCode
                //                                             ,[ChemicalSymbol] as PollutantName
                //                                              from [Standard].[TB_PollutantCode] pc
                //                                              inner join [BasicInfo].[TB_PersonalizedSettings]  as ps
                //                                                  on pc.PollutantUid=ps.ParameterUid 
                //                                                  left join [EQMS_Framework].[dbo].[VI_UserInfoAll] Us
                //                                                  on ps.UserUid=Us.RowGuid
                //                                             where ps.EnableCustomOrNot=1 and ps.ParameterType='pollutant' and pc.IsCalEQIOrNot =1
                //                                              and ps.ApplicationUid='{0}' and LoginID = '{1}'
                //                                               Order by pc.OrderByNum desc"
                //                            , appUid, LoginId);
                //此处需要增加臭氧8小时，故不增加权限控制
                string querySql = @"
SELECT 
[PollutantCode]
,(case [PollutantCode] when 'a05026' then 'O3_8' else [ChemicalSymbol] end) PollutantName
FROM [AMS_BaseData].[Standard].[TB_PollutantCode]
where PollutantCode in ('a21026','a21004','a21005','a05024','a34002','a34004','a05026')
Order by OrderByNum desc";
                if (sysType == "water")
                    querySql = string.Format(@"
                                              select pc.PollutantCode
                                             ,pc.PollutantName 
                                             ,[ChemicalSymbol]  
                                              from [Standard].[TB_PollutantCode] pc
                                               left join [BasicInfo].[TB_PersonalizedSettings]  as ps
                                                  on pc.PollutantUid=ps.ParameterUid 
                                                  left join [EQMS_Framework].[dbo].[VI_UserInfoAll] Us
                                                  on ps.UserUid=Us.RowGuid
                                             where ps.EnableCustomOrNot=1 and ps.ParameterType='pollutant'
                                              and ps.ApplicationUid='{0}'
                                               Order by pc.OrderByNum desc"
                             , appUid, LoginId);



                DataTable portDt = DBHelper.GetDataView(querySql, null, DBHelper.GetConnectionString(AMS_BaseData_Conn));

                foreach (System.Data.DataRowView row in portDt.DefaultView)
                {
                    if (jsonSb.Length != 0)
                    {
                        jsonSb.Append(",");
                    }

                    jsonSb.AppendFormat(@"{{""name"":""{0}""}}", row["PollutantName"]);
                }

                jsonStr = string.Format(@"{{""FactorList"":[{0}]}}", jsonSb);
            }
            catch (Exception e)
            {
                //CommonFunction.WriteErrorLog("When GetFactorList SysType=[" + sysType + "] Error: " + e.Message);
                CommonFunction.WriteErrorLog("Error: " + e.Message);
            }

            //CommonFunction.WriteInfoLog("Return GetFactorList SysType=[" + sysType + "] Result=[" + jsonStr + "]");

            ReturnJson(jsonStr);
        }

        /// <summary>
        /// {"OnlineCategary":[{"name":"常规站在线信息","categary":"Common"},{"name":"超级站在线信息","categary":"Super"}]}
        /// </summary>
        /// <param name="sysType"></param>
        [WebMethod(Description = @"【在线信息分类】获取在线信息列表<br />")]
        public void GetOnlineCategary()
        {
            string jsonStr = string.Empty;

            try
            {
                StringBuilder jsonSb = new System.Text.StringBuilder();
                DataTable dt = new DataTable();
                dt.Columns.Add("name",typeof(string));
                dt.Columns.Add("categary", typeof(string));
                DataRow dr = dt.NewRow();
                dr["name"] = "常规站在线信息";
                dr["categary"] = "Common";
                dt.Rows.Add(dr);
                DataRow dr2 = dt.NewRow();
                dr2["name"] = "超级站在线信息";
                dr2["categary"] = "Super";
                dt.Rows.Add(dr2);


                foreach (System.Data.DataRowView row in dt.DefaultView)
                {
                    if (jsonSb.Length != 0)
                    {
                        jsonSb.Append(",");
                    }

                    jsonSb.AppendFormat(@"{{""name"":""{0}"",""categary"":""{1}""}}", row["name"], row["categary"]);
                }

                jsonStr = string.Format(@"{{""OnlineCategary"":[{0}]}}", jsonSb);
            }
            catch (Exception e)
            {
                CommonFunction.WriteErrorLog("Error: " + e.Message);
            }

            ReturnJson(jsonStr);
        }

        /// <summary>
        /// {"PortHourAQI":[{"PortId":"2","PortName":"吴江区环保局","DateTime":"2015-11-25 10:00","SO2_IAQI":"3","NO2_IAQI":"19","PM10_IAQI":"40","CO_IAQI":"14","O3_IAQI":"7","PM2.5_IAQI":"46","AQI":"46","PrimaryPollutant":"--","Class":"优","Grade":"一级","RGBValue":"#00e400","HealthEffect":"空气质量令人满意，基本无空气污染","TakeStep":"各类人群可正常活动","PrimaryPollutantValue":"--"}]}
        /// </summary>
        /// <param name="portId"></param>
        [WebMethod(Description = @"【AQI】获取最新小时AQI空气质量状况<br />portId: 站点信息， portId=0代表全市，不填显示全部点位的信息。")]
        public void GetLatestHourAQI(string portId)
        {
            StringBuilder jsonSb = new StringBuilder();

            try
            {
                if (string.IsNullOrEmpty(portId))
                {
                    jsonSb.Append(GetLatestHourAQIJson("0"));
                    if (jsonSb.Length > 0)
                    {
                        jsonSb.Append(",");
                    }
                    jsonSb.Append(GetLatestHourAQIJson(""));
                }
                else
                {
                    jsonSb.Append(GetLatestHourAQIJson(portId));
                }
            }
            catch (Exception e)
            {
                // CommonFunction.WriteErrorLog("When GetLatestHourAQI PortId=[" + portId + "] Error: " + e.Message);
                CommonFunction.WriteErrorLog("Error: " + e.Message);
            }

            string jsonStr = string.Format(@"{{""PortHourAQI"":[{0}]}}", jsonSb);

            // CommonFunction.WriteInfoLog("Return GetLatestHourAQI PortId=[" + portId + "] Result=[" + jsonStr + "]");

            ReturnJson(jsonStr);
        }

        /// <summary>
        /// {"HoursFactorData":[{"DateTime":"2015-11-24 15:00","value":"2"},{"DateTime":"2015-11-24 16:00","value":"1"},{"DateTime":"2015-11-24 17:00","value":"2"},{"DateTime":"2015-11-24 18:00","value":"1"},{"DateTime":"2015-11-24 19:00","value":"1"},{"DateTime":"2015-11-24 20:00","value":"1"},{"DateTime":"2015-11-24 21:00","value":"2"},{"DateTime":"2015-11-24 22:00","value":"2"},{"DateTime":"2015-11-24 23:00","value":"4"},{"DateTime":"2015-11-25 00:00","value":"4"},{"DateTime":"2015-11-25 01:00","value":"4"},{"DateTime":"2015-11-25 02:00","value":"4"},{"DateTime":"2015-11-25 03:00","value":"4"},{"DateTime":"2015-11-25 04:00","value":"4"},{"DateTime":"2015-11-25 05:00","value":"5"},{"DateTime":"2015-11-25 06:00","value":"6"},{"DateTime":"2015-11-25 07:00","value":"6"},{"DateTime":"2015-11-25 08:00","value":"6"},{"DateTime":"2015-11-25 09:00","value":"7"},{"DateTime":"2015-11-25 10:00","value":"8"},{"DateTime":"2015-11-25 11:00","value":"9"},{"DateTime":"2015-11-25 12:00","value":"8"},{"DateTime":"2015-11-25 13:00","value":"8"},{"DateTime":"2015-11-25 14:00","value":"6"}]}
        /// </summary>
        /// <param name="portId"></param>
        /// <param name="factorName"></param>
        [WebMethod(Description = @"【AQI】获取污染物最近24小时因子数据<br />参数说明：<br />portId为站点ID，portId=0代表全市；FactorName为污染物因子名称：PM25/PM10/O3/NO2/SO2/CO/Recent8HoursO3")]
        public void Get24HoursFactorDataAir(string portId, string factorName)
        {
            string jsonStr = string.Empty;

            //string tableName = "AirRelease.TB_HourAQI";

            //南通超级站审核无数据，数据源变更为原始
            string tableName = "AMS_AirAutoMonitor.Air.TB_OriHourAQI";

            if ("0".Equals(portId))
            {
                //tableName = "AirRelease.TB_RegionHourAQI";
                tableName = "AMS_AirAutoMonitor.Air.TB_OriRegionHourAQI";
            }

            try
            {
                factorName = ("PM2.5".Equals(factorName.ToUpper())) ? "PM25" : (("O3_8".Equals(factorName.ToUpper())) ? "Recent8HoursO3" : factorName);

                string sql = string.Format(@"
                                          select dateadd(hh,0,[DateTime]) DateTime,
                                                 isnull([{0}],'--') [{0}] 
                                            from {1} 
                                           where {2} 
                                             and dateTime > DATEADD(HOUR, -24, GETDATE()) 
                                           order by dateTime asc"
                                          , factorName
                                          , tableName
                                          , "0".Equals(portId)
                                                   ? string.Format("MonitoringRegionUid = '{0}' ", monitorRegionUid)
                                                   : string.Format("PointId = {0} ", portId));

                //DataTable dt = DBHelper.GetDataView(sql, null, DBHelper.GetConnectionString(AMS_MonitorBusiness_Conn));
                DataTable dt = DBHelper.GetDataView(sql, null, DBHelper.GetConnectionString("AMS_AirAutoMonitor_Conn"));

                StringBuilder jsonSb = new StringBuilder();
                foreach (System.Data.DataRowView row in dt.DefaultView)
                {
                    if (jsonSb.Length != 0)
                    {
                        jsonSb.Append(",");
                    }

                    StringBuilder colSb = new StringBuilder();
                    foreach (System.Data.DataColumn col in dt.Columns)
                    {
                        if (colSb.Length != 0)
                        {
                            colSb.Append(",");
                        }

                        if (col.ColumnName.ToLower() == "datetime")
                        {
                            colSb.AppendFormat(@"""{0}"":""{1}""", col.ColumnName, string.Format("{0:yyyy-MM-dd HH:00}", row[col.ColumnName]));
                        }
                        else if (col.ColumnName.ToUpper() == "CO")
                        {
                            colSb.AppendFormat(@"""{0}"":""{1}""", "value", row[col.ColumnName].ToString() == "--" ? "--" : Math.Round(Convert.ToDecimal(row[col.ColumnName].ToString()), 1).ToString());
                        }
                        else
                        {
                            colSb.AppendFormat(@"""{0}"":""{1}""", "value", row[col.ColumnName].ToString() == "--" ? "--" : Math.Round(Convert.ToDecimal(row[col.ColumnName].ToString()) * 1000, 0).ToString());
                        }
                    }
                    jsonSb.AppendFormat("{{{0}}}", colSb);
                }

                jsonStr = string.Format(@"{{""HoursFactorData"":[{0}]}}", jsonSb);
            }
            catch (Exception e)
            {
                //CommonFunction.WriteErrorLog("When Get24HoursFactorDataAir PortId=[" + portId
                //                           + "] FactorName=[" + factorName + "] Error: " + e.Message);
                CommonFunction.WriteErrorLog("Error: " + e.Message);
            }

            //CommonFunction.WriteInfoLog("Return Get24HoursFactorDataAir PortId=[" + portId
            //                          + "] FactorName=[" + factorName + "] Result=[" + jsonStr + "]");

            ReturnJson(jsonStr);
        }

        [WebMethod(Description = @"常规因子获取单位<br />参数说明：FactorName为污染物因子名称：PM25/PM10/O3/NO2/SO2/CO/Recent8HoursO3")]
        public void GetMeasureUnit(string factorName)
        {
            try
            {
                string jsonStr = string.Empty;
                factorName = ("PM25".Equals(factorName.ToUpper())) ? "PM2.5" : (("O3_8".Equals(factorName.ToUpper())) ? "O3" : ((("Recent8HoursO3".Equals(factorName)) ? "O3" : factorName)));

                string sql = string.Format(@"select top 1 [MeasureUnitName]  from [dbo].[V_Factor_Air_SiteMap] WHERE PNAME='{0}'", factorName);

                DataTable dt = DBHelper.GetDataView(sql, null, DBHelper.GetConnectionString("AMS_BaseData_Conn"));

                jsonStr = string.Format(@"{{""measureUnit"":""{0}""}}", dt.Rows[0]["MeasureUnitName"].ToString());
                ReturnJson(jsonStr);
            }
            catch (Exception e)
            {
                CommonFunction.WriteErrorLog("Error: " + e.Message);
            }
        }


        /// <summary>
        /// {"HoursFactorData":[{"DateTime":"2015-11-24 15:00","value":"2"},{"DateTime":"2015-11-24 16:00","value":"1"},{"DateTime":"2015-11-24 17:00","value":"2"},{"DateTime":"2015-11-24 18:00","value":"1"},{"DateTime":"2015-11-24 19:00","value":"1"},{"DateTime":"2015-11-24 20:00","value":"1"},{"DateTime":"2015-11-24 21:00","value":"2"},{"DateTime":"2015-11-24 22:00","value":"2"},{"DateTime":"2015-11-24 23:00","value":"4"},{"DateTime":"2015-11-25 00:00","value":"4"},{"DateTime":"2015-11-25 01:00","value":"4"},{"DateTime":"2015-11-25 02:00","value":"4"},{"DateTime":"2015-11-25 03:00","value":"4"},{"DateTime":"2015-11-25 04:00","value":"4"},{"DateTime":"2015-11-25 05:00","value":"5"},{"DateTime":"2015-11-25 06:00","value":"6"},{"DateTime":"2015-11-25 07:00","value":"6"},{"DateTime":"2015-11-25 08:00","value":"6"},{"DateTime":"2015-11-25 09:00","value":"7"},{"DateTime":"2015-11-25 10:00","value":"8"},{"DateTime":"2015-11-25 11:00","value":"9"},{"DateTime":"2015-11-25 12:00","value":"8"},{"DateTime":"2015-11-25 13:00","value":"8"},{"DateTime":"2015-11-25 14:00","value":"6"}]}
        /// </summary>
        /// <param name="portId"></param>
        /// <param name="factorName"></param>
        [WebMethod(Description = @"【AQI】获取污染物最近24小时因子数据<br />参数说明：<br />portId为站点ID，portId=0代表全市；FactorName为污染物因子名称：PM25/PM10/O3/NO2/SO2/CO/Recent8HoursO3")]
        public void Get24HoursFactorDataAir_ForAndorid(string portId, string factorName)
        {
            string jsonStr = string.Empty;

            //string tableName = "AirRelease.TB_HourAQI";

            //南通超级站审核无数据，数据源变更为原始
            string tableName = "AMS_AirAutoMonitor.Air.TB_OriHourAQI";

            if ("0".Equals(portId))
            {
                //tableName = "AirRelease.TB_RegionHourAQI";
                tableName = "AMS_AirAutoMonitor.Air.TB_OriRegionHourAQI";
            }

            try
            {
                factorName = ("PM2.5".Equals(factorName.ToUpper())) ? "PM25" : (("O3_8".Equals(factorName.ToUpper())) ? "Recent8HoursO3" : factorName);

                string sql = string.Format(@"
                                          select dateadd(hh,0,[DateTime]) DateTime,
                                                 isnull([{0}],'--') [{0}] 
                                            from {1} 
                                           where {2} 
                                             and dateTime > DATEADD(HOUR, -24, GETDATE()) 
                                           order by dateTime desc"
                                          , factorName
                                          , tableName
                                          , "0".Equals(portId)
                                                   ? string.Format("MonitoringRegionUid = '{0}' ", monitorRegionUid)
                                                   : string.Format("PointId = {0} ", portId));

                //DataTable dt = DBHelper.GetDataView(sql, null, DBHelper.GetConnectionString(AMS_MonitorBusiness_Conn));
                DataTable dt = DBHelper.GetDataView(sql, null, DBHelper.GetConnectionString("AMS_AirAutoMonitor_Conn"));

                StringBuilder jsonSb = new StringBuilder();
                foreach (System.Data.DataRowView row in dt.DefaultView)
                {
                    if (jsonSb.Length != 0)
                    {
                        jsonSb.Append(",");
                    }

                    StringBuilder colSb = new StringBuilder();
                    foreach (System.Data.DataColumn col in dt.Columns)
                    {
                        if (colSb.Length != 0)
                        {
                            colSb.Append(",");
                        }

                        if (col.ColumnName.ToLower() == "datetime")
                        {
                            colSb.AppendFormat(@"""{0}"":""{1}""", col.ColumnName, string.Format("{0:yyyy-MM-dd HH:00}", row[col.ColumnName]));
                        }
                        else if (col.ColumnName.ToUpper() == "CO")
                        {
                            colSb.AppendFormat(@"""{0}"":""{1}""", "value", row[col.ColumnName].ToString() == "--" ? "--" : Math.Round(Convert.ToDecimal(row[col.ColumnName].ToString()), 1).ToString());
                        }
                        else
                        {
                            colSb.AppendFormat(@"""{0}"":""{1}""", "value", row[col.ColumnName].ToString() == "--" ? "--" : Math.Round(Convert.ToDecimal(row[col.ColumnName].ToString()) * 1000, 0).ToString());
                        }
                    }
                    jsonSb.AppendFormat("{{{0}}}", colSb);
                }

                jsonStr = string.Format(@"{{""HoursFactorData"":[{0}]}}", jsonSb);
            }
            catch (Exception e)
            {
                //CommonFunction.WriteErrorLog("When Get24HoursFactorDataAir PortId=[" + portId
                //                           + "] FactorName=[" + factorName + "] Error: " + e.Message);
                CommonFunction.WriteErrorLog("Error: " + e.Message);
            }

            //CommonFunction.WriteInfoLog("Return Get24HoursFactorDataAir PortId=[" + portId
            //                          + "] FactorName=[" + factorName + "] Result=[" + jsonStr + "]");

            ReturnJson(jsonStr);
        }

        /// <summary>
        /// {"HourConcentration":[{"DateTime":"2015-11-25 14:00","Value":[{"factor": "PM2.5","type": "1小时","value": "16μg/m3"},{"factor": "O3","type": "1小时","value": "39μg/m3"},{"factor": "CO","type": "1小时","value": "1.0mg/m3"},{"factor": "PM10","type": "1小时","value": "31μg/m3"},{"factor": "SO2","type": "1小时","value": "6μg/m3"},{"factor": "NO2","type": "1小时","value": "31μg/m3"}]}]}
        /// </summary>
        /// <param name="portId"></param>
        [WebMethod(Description = @"【浓度】获取小时浓度值<br />参数说明：portId:站点ID，portId=0代表全市")]
        public void GetHourConcentration(string portId)
        {
            string jsonStr = string.Empty;

            string tableName = "AirRelease.TB_HourAQI";

            if ("0".Equals(portId))
            {
                tableName = "AirRelease.TB_RegionHourAQI";
            }

            try
            {
                string sql = string.Format(@"select {0} PortName,
                                                dateadd(hh,0,[datetime]) DateTime,
                                                [PM25],
                                                [Recent24HoursPM25],
                                                O3,
                                                Recent8HoursO3,
                                                CO,
                                                PM10,
                                                Recent24HoursPM10,
                                                SO2,
                                                NO2
                                           from {1} as V 
                                                {2}
                                          where V.[dateTime] = (  
                                                                select MAX([dateTime]) 
                                                                  from {1} 
                                                                 where {3} [dateTime] > DATEADD(DAY, -7, GETDATE())
                                                                   and [dateTime] < GETDATE()) 
                                                {4}"
                                               , "0".Equals(portId) ? "0 AS PortId,'南通市' as" : "v.PointId as PortId, p.MonitoringPointName as"
                                               , tableName
                                               , "0".Equals(portId) ? "" : "inner join [dbo].[SY_MonitoringPoint] as P on v.PointId = p.PointId "
                                               , "0".Equals(portId)
                                                         ? string.Format("MonitoringRegionUid = '{0}' and ", monitorRegionUid)
                                                         : "PointId = V.PointId and "
                                               , "0".Equals(portId)
                                                         ? string.Format(" and v.MonitoringRegionUid = '{0}'", monitorRegionUid)
                                                         : string.Format(" and v.PointId={0}", portId));

                StringBuilder jsonSb = new StringBuilder();

                DataTable dt = DBHelper.GetDataView(sql, null, DBHelper.GetConnectionString(AMS_MonitorBusiness_Conn));

                if (dt != null && dt.Rows.Count > 0)
                {
                    StringBuilder factorsInfoSb = new StringBuilder();
                    DataRow dr = dt.Rows[0];
                    factorsInfoSb.AppendFormat(@"{{""factor"": ""PM2.5"",""type"": ""1小时"",""value"": ""{0}μg/m3""}},", Convert.IsDBNull(dr["PM25"]) ? "--" : Math.Round(Convert.ToDecimal(dr["PM25"].ToString()) * 1000, 0).ToString());

                    factorsInfoSb.AppendFormat(@"{{""factor"": ""O3"",""type"": ""1小时"",""value"": ""{0}μg/m3""}},", Convert.IsDBNull(dr["O3"]) ? "--" : Math.Round(Convert.ToDecimal(dr["O3"].ToString()) * 1000, 0).ToString());

                    factorsInfoSb.AppendFormat(@"{{""factor"": ""CO"",""type"": ""1小时"",""value"": ""{0}mg/m3""}},", Convert.IsDBNull(dr["CO"]) ? "--" : Math.Round(Convert.ToDecimal(dr["CO"].ToString()), 1).ToString());

                    factorsInfoSb.AppendFormat(@"{{""factor"": ""PM10"",""type"": ""1小时"",""value"": ""{0}μg/m3""}},", Convert.IsDBNull(dr["PM10"]) ? "--" : Math.Round(Convert.ToDecimal(dr["PM10"].ToString()) * 1000, 0).ToString());

                    factorsInfoSb.AppendFormat(@"{{""factor"": ""SO2"",""type"": ""1小时"",""value"": ""{0}μg/m3""}},", Convert.IsDBNull(dr["SO2"]) ? "--" : Math.Round(Convert.ToDecimal(dr["SO2"].ToString()) * 1000, 0).ToString());

                    factorsInfoSb.AppendFormat(@"{{""factor"": ""NO2"",""type"": ""1小时"",""value"": ""{0}μg/m3""}}", Convert.IsDBNull(dr["NO2"]) ? "--" : Math.Round(Convert.ToDecimal(dr["NO2"].ToString()) * 1000, 0).ToString());

                    jsonSb.AppendFormat(@"{{""DateTime"":""{0}"",""Value"":[{1}]}}", Convert.ToDateTime(dr["DateTime"]).ToString("yyyy-MM-dd HH:00"), factorsInfoSb.ToString());
                }
                else
                {
                    //CommonFunction.WriteInfoLog("When GetHourConcentration PortId=[" + portId + "] Not Query Data....");
                }

                jsonStr = string.Format(@"{{""HourConcentration"":[{0}]}}", jsonSb.ToString());
            }
            catch (Exception e)
            {
                //CommonFunction.WriteErrorLog("When GetHourConcentration PortId=[" + portId + "] Error: " + e.Message);
                CommonFunction.WriteErrorLog("Error: " + e.Message);
            }

            //CommonFunction.WriteInfoLog("Return GetHourConcentration PortId=[" + portId + "] Result=[" + jsonStr + "]");

            ReturnJson(jsonStr);
        }

        /// <summary>
        /// {"HourAQI":[{"DateTime":"2015-11-25 13:00","AQI":"44","PrimaryPollutant":"--","RGBValue":"#00e400","Class":"优","Grade":"一级","HealthEffect":"空气质量令人满意，基本无空气污染","TakeStep":"各类人群可正常活动"},{"DateTime":"2015-11-25 14:00","AQI":"31","PrimaryPollutant":"--","RGBValue":"#00e400","Class":"优","Grade":"一级","HealthEffect":"空气质量令人满意，基本无空气污染","TakeStep":"各类人群可正常活动"},{"DateTime":"2015-11-25 15:00","AQI":"36","PrimaryPollutant":"--","RGBValue":"#00e400","Class":"优","Grade":"一级","HealthEffect":"空气质量令人满意，基本无空气污染","TakeStep":"各类人群可正常活动"}]}
        /// </summary>
        /// <param name="portId"></param>
        /// <param name="topN"></param>
        [WebMethod(Description = @"【气_趋势】获取最近N小时AQI情况<br />参数说明：<br />portId为站点ID，portId=0代表全市；<br />topN表示最近N小时（例：填写24代表获取最近24小时AQI）")]
        public void GetTopNHourAQI(string portId, string topN)
        {
            string jsonStr = string.Empty;

            //string tableName = "AirRelease.TB_HourAQI";

            //if ("0".Equals(portId))
            //{
            //    tableName = "AirRelease.TB_RegionHourAQI";
            //}
            string AMS_MonitorBusiness_Con = "AMS_AirAutoMonitor_Conn";
            string tableName = "Air.TB_OriHourAQI";
            //string tableName = "AirRelease.TB_HourAQI";

            if ("0".Equals(portId))
            {
                tableName = "AirRelease.TB_RegionHourAQI";
                AMS_MonitorBusiness_Con = "AMS_MonitorBusiness_Conn";
            }
            try
            {
                string sql = string.Format(@"
                                                 select dateadd(hh,0,datetime) DateTime,
                                                        AQIValue AQI,
                                                        PrimaryPollutant,
                                                        RGBValue,
                                                        Class,
                                                        Grade,
                                                        HealthEffect,
                                                        TakeStep 
                                                   from {0} 
                                                  where {1} 
                                                    and dateTime > DATEADD(HOUR, -({2}), GETDATE())
                                                  order by dateTime asc"
                                           , tableName
                                           , "0".Equals(portId)
                                                  ? string.Format("MonitoringRegionUid = '{0}' ", monitorRegionUid)
                                                  : string.Format("PointId = {0} ", portId)
                                           , topN);

                DataTable dt = DBHelper.GetDataView(sql, null, DBHelper.GetConnectionString(AMS_MonitorBusiness_Con));
                if (dt != null && dt.Rows.Count > 0)
                {
                    StringBuilder jsonSb = new StringBuilder();
                    foreach (System.Data.DataRowView row in dt.DefaultView)
                    {
                        if (jsonSb.Length != 0)
                        {
                            jsonSb.Append(",");
                        }

                        StringBuilder colSb = new StringBuilder();
                        foreach (DataColumn col in dt.Columns)
                        {
                            if (colSb.Length != 0)
                            {
                                colSb.Append(",");
                            }

                            if (col.ColumnName.ToLower() == "datetime")
                                colSb.AppendFormat(@"""{0}"":""{1}""", col.ColumnName, string.Format("{0:yyyy-MM-dd HH:00}", row[col.ColumnName]));
                            else
                                colSb.AppendFormat(@"""{0}"":""{1}""", col.ColumnName, System.Convert.IsDBNull(row[col.ColumnName]) ? "--" : row[col.ColumnName]);
                        }

                        jsonSb.AppendFormat("{{{0}}}", colSb);
                    }

                    jsonStr = string.Format(@"{{""HourAQI"":[{0}]}}", jsonSb);
                }
                else
                {
                    jsonStr = string.Format(@"{{""HourAQI"":[{0}]}}", "");
                }
            }
            catch (Exception e)
            {
                CommonFunction.WriteErrorLog("When GetTopNHourAQI PortId=[" + portId + "] topNum=[" + topN + "] Error: " + e.Message);
            }

            //CommonFunction.WriteInfoLog("Return GetTopNHourAQI PortId=[" + portId + "] topNum=[" + topN + "] Result=[" + jsonStr + "]");

            ReturnJson(jsonStr);
        }

        /// <summary>
        /// {"HourAQI":[{"DateTime":"2015-11-25 13:00","AQI":"44","PrimaryPollutant":"--","RGBValue":"#00e400","Class":"优","Grade":"一级","HealthEffect":"空气质量令人满意，基本无空气污染","TakeStep":"各类人群可正常活动"},{"DateTime":"2015-11-25 14:00","AQI":"31","PrimaryPollutant":"--","RGBValue":"#00e400","Class":"优","Grade":"一级","HealthEffect":"空气质量令人满意，基本无空气污染","TakeStep":"各类人群可正常活动"},{"DateTime":"2015-11-25 15:00","AQI":"36","PrimaryPollutant":"--","RGBValue":"#00e400","Class":"优","Grade":"一级","HealthEffect":"空气质量令人满意，基本无空气污染","TakeStep":"各类人群可正常活动"}]}
        /// </summary>
        /// <param name="portId"></param>
        /// <param name="topN"></param>
        [WebMethod(Description = @"【气_趋势】获取最近N小时AQI情况<br />参数说明：<br />portId为站点ID，portId=0代表全市；<br />topN表示最近N小时（例：填写24代表获取最近24小时AQI）")]
        public void GetTopNHourAQI_ForAndorid(string portId, string topN)
        {
            string jsonStr = string.Empty;
            string AMS_MonitorBusiness_Con = "AMS_AirAutoMonitor_Conn";
            string tableName = "Air.TB_OriHourAQI";
            //string tableName = "AirRelease.TB_HourAQI";

            if ("0".Equals(portId))
            {
                tableName = "AirRelease.TB_RegionHourAQI";
                AMS_MonitorBusiness_Con = "AMS_MonitorBusiness_Conn";
            }
            //string tableName = "AirRelease.TB_HourAQI";

            //if ("0".Equals(portId))
            //{
            //    tableName = "AirRelease.TB_RegionHourAQI";
            //}

            try
            {
                string sql = string.Format(@"
                                                 select dateadd(hh,0,datetime) DateTime,
                                                        AQIValue AQI,
                                                        PrimaryPollutant,
                                                        RGBValue,
                                                        Class,
                                                        Grade,
                                                        HealthEffect,
                                                        TakeStep 
                                                   from {0} 
                                                  where {1} 
                                                    and dateTime > DATEADD(HOUR, -({2}), GETDATE())
                                                  order by dateTime desc"
                                           , tableName
                                           , "0".Equals(portId)
                                                  ? string.Format("MonitoringRegionUid = '{0}' ", monitorRegionUid)
                                                  : string.Format("PointId = {0} ", portId)
                                           , topN);

                DataTable dt = DBHelper.GetDataView(sql, null, DBHelper.GetConnectionString(AMS_MonitorBusiness_Con));
                if (dt != null && dt.Rows.Count > 0)
                {
                    StringBuilder jsonSb = new StringBuilder();
                    foreach (System.Data.DataRowView row in dt.DefaultView)
                    {
                        if (jsonSb.Length != 0)
                        {
                            jsonSb.Append(",");
                        }

                        StringBuilder colSb = new StringBuilder();
                        foreach (DataColumn col in dt.Columns)
                        {
                            if (colSb.Length != 0)
                            {
                                colSb.Append(",");
                            }

                            if (col.ColumnName.ToLower() == "datetime")
                                colSb.AppendFormat(@"""{0}"":""{1}""", col.ColumnName, string.Format("{0:yyyy-MM-dd HH:00}", row[col.ColumnName]));
                            else
                                colSb.AppendFormat(@"""{0}"":""{1}""", col.ColumnName, System.Convert.IsDBNull(row[col.ColumnName]) ? "--" : row[col.ColumnName]);
                        }

                        jsonSb.AppendFormat("{{{0}}}", colSb);
                    }

                    jsonStr = string.Format(@"{{""HourAQI"":[{0}]}}", jsonSb);
                }
                else
                {
                    jsonStr = string.Format(@"{{""HourAQI"":[{0}]}}", "");
                }
            }
            catch (Exception e)
            {
                CommonFunction.WriteErrorLog("When GetTopNHourAQI PortId=[" + portId + "] topNum=[" + topN + "] Error: " + e.Message);
            }

            //CommonFunction.WriteInfoLog("Return GetTopNHourAQI PortId=[" + portId + "] topNum=[" + topN + "] Result=[" + jsonStr + "]");

            ReturnJson(jsonStr);
        }

        /// <summary>
        /// {"DayAQI":[{"DateTime":"2015-11-23","AQI":"49","PrimaryPollutant":"--","RGBValue":"#00e400","Class":"优","Grade":"一级","HealthEffect":"空气质量令人满意，基本无空气污染","TakeStep":"各类人群可正常活动"},{"DateTime":"2015-11-24","AQI":"35","PrimaryPollutant":"--","RGBValue":"#00e400","Class":"优","Grade":"一级","HealthEffect":"空气质量令人满意，基本无空气污染","TakeStep":"各类人群可正常活动"},{"DateTime":"2015-11-25","AQI":"--","PrimaryPollutant":"--","RGBValue":"--","Class":"--","Grade":"--","HealthEffect":"--","TakeStep":"--"}]}
        /// </summary>
        /// <param name="portId"></param>
        /// <param name="topN"></param>
        [WebMethod(Description = @"【气_趋势】获取最近N天AQI情况<br />参数说明：<br />portId为站点ID，portId=0代表全市；<br />topN表示最近N天（例：填写30代表获取最近30天日AQI）")]
        public void GetTopNDayAQI(string portId, string topN)
        {
            string jsonStr = string.Empty;

            //string tableName = "AirRelease.TB_DayAQI";

            //if ("0".Equals(portId))
            //{
            //    tableName = "AirReport.TB_RegionDayAQIReport";
            //}
            string AMS_MonitorBusiness_Con = "AMS_AirAutoMonitor_Conn";
            string tableName = "Air.TB_OriDayAQI";
            //string tableName = "AirRelease.TB_HourAQI";

            if ("0".Equals(portId))
            {
                tableName = "AirReport.TB_RegionDayAQIReport";
                AMS_MonitorBusiness_Con = "AMS_MonitorBusiness_Conn";
            }
            try
            {
                string sql = string.Format(@" 
                                               select {0} as DateTime,
                                                      AQIValue AQI,
                                                      PrimaryPollutant,
                                                      RGBValue,
                                                      Class,
                                                      Grade,
                                                      HealthEffect,
                                                      TakeStep 
                                                 from {1} 
                                                where {2} 
                                                  and {0} > DATEADD(DAY, -({3}), GETDATE()) 
                                                order by {0} asc"
                                                  , "0".Equals(portId) ? "ReportDateTime" : "DateTime"
                                                  , tableName
                                                  , "0".Equals(portId)
                                                        ? string.Format("MonitoringRegionUid = '{0}' ", monitorRegionUid)
                                                        : string.Format("PointId = {0} ", portId)
                                                  , Convert.ToInt32(topN) + 1);

                DataTable dt = DBHelper.GetDataView(sql, null, DBHelper.GetConnectionString(AMS_MonitorBusiness_Con));
                if (DateTime.Now.Hour > 20)
                {
                    dt.Rows.RemoveAt(0);
                }
                else
                {
                    dt.Rows.RemoveAt(dt.Rows.Count - 1);
                }

                StringBuilder jsonSb = new StringBuilder();
                foreach (System.Data.DataRowView row in dt.DefaultView)
                {
                    if (jsonSb.Length != 0)
                    {
                        jsonSb.Append(",");
                    }

                    StringBuilder colSb = new StringBuilder();

                    foreach (DataColumn col in dt.Columns)
                    {
                        if (colSb.Length != 0) colSb.Append(",");

                        if (col.ColumnName.ToLower() == "datetime")
                            colSb.AppendFormat(@"""{0}"":""{1}""", col.ColumnName, string.Format("{0:yyyy-MM-dd}", row[col.ColumnName]));
                        else
                            colSb.AppendFormat(@"""{0}"":""{1}""", col.ColumnName, System.Convert.IsDBNull(row[col.ColumnName]) ? "--" : row[col.ColumnName]);
                    }
                    jsonSb.AppendFormat("{{{0}}}", colSb);
                }

                jsonStr = string.Format(@"{{""DayAQI"":[{0}]}}", jsonSb);
            }
            catch (Exception e)
            {
                CommonFunction.WriteErrorLog("When GetTopNDayAQI PortId=[" + portId + "] topNum=[" + topN + "] Error: " + e.Message);
            }

            //CommonFunction.WriteInfoLog("Return GetTopNDayAQI PortId=[" + portId + "] topNum=[" + topN + "] Result=[" + jsonStr + "]");

            ReturnJson(jsonStr);
        }

        /// <summary>
        /// {"OnlineInfo":[{"portId":"2","PortName":"吴江区环保局","IsOnline":"True","RecordCount":"24","LastestTime":"2015/11/25 14:00:00"},{"portId":"4","PortName":"教师进修学校","IsOnline":"True","RecordCount":"24","LastestTime":"2015/11/25 14:00:00"},{"portId":"5","PortName":"开发区","IsOnline":"True","RecordCount":"24","LastestTime":"2015/11/25 14:00:00"}]}
        /// </summary>
        /// <param name="sysType"></param>
        [WebMethod(Description = @"【在线情况】获取在线情况信息<br />参数说明：<br />sysType:系统类型<br /> air：空气/water：地表水/air_Super：空气超级站")]
        public void GetOnlineInfo(string sysType)
        {
            string jsonStr = string.Empty;

            ///仪器状态
            if (sysType == "air_Super")
            {
                try
                {
                    string sql = @"SELECT Ins.[InstrumentName]
,case isNull(IsOnline, 0) when 1 then 'True' else 'False' end [IsOnline]
,MAX(case when [IsOnline]=1 then '在线' else case when [IsOnline]=0 then '离线'+cast( (OffLineTime/60) as nvarchar) +'小时'+ cast( (OffLineTime%60)as nvarchar) +'分' else '在线'  end end) as NetWorkInfo 
,[NewDataTime] LastestTime
FROM [dbo].[InstrumentDataOnline] line
left join [AMS_BaseData].[InstrInfo].[TB_Instruments] Ins
on line.[InstrumentUid] =Ins.RowGuid
where DataTypeUid='1b6367f1-5287-4c14-b120-7a35bd176db1'
and Ins.ApplyTypeUid='3b5ac81c-cefb-4db8-b19f-6c4c2f41eb03' and EnableOrNot=1 and ShowInMenu=1  
 and IsOnline in (0,1)
group by Ins.[InstrumentName],[IsOnline],NewDataTime,Ins.[OrderByNum]
order by Ins.[OrderByNum] desc";
                    //and line.InstrumentUid in ('1589850e-0df1-4d9d-b508-4a77def158ba','9ef57f3c-8cce-4fe3-980f-303bbcfde260','a6b3d80c-8281-4bc6-af47-f0febf568a5c','6e4aa38a-f68b-490b-9cd7-3b92c7805c2d','56dd6e9b-4c8f-4e67-a70f-b6a277cb44d7','da4f968f-cc6e-4fec-8219-6167d100499d','b925b850-72cc-4b35-a15c-b3ec4c531069','3745f768-a789-4d58-9578-9e41fde5e5f0')

                    DataTable dt = DBHelper.GetDataView(sql, null, DBHelper.GetConnectionString(AMS_MonitorBusiness_Conn));
                    StringBuilder jsonSb = new StringBuilder();
                    foreach (DataRowView row in dt.DefaultView)
                    {
                        if (jsonSb.Length != 0)
                            jsonSb.Append(",");
                        row["LastestTime"] = Convert.ToDateTime(Convert.ToDateTime(row["LastestTime"]).ToString("yyyy-MM-dd HH:00:00"));
                        StringBuilder colSb = new StringBuilder();
                        foreach (System.Data.DataColumn col in dt.Columns)
                        {
                            if (colSb.Length != 0) colSb.Append(",");
                            colSb.AppendFormat(@"""{0}"":""{1}""", col.ColumnName, System.Convert.IsDBNull(row[col.ColumnName]) ? "--" : row[col.ColumnName]);
                        }
                        jsonSb.AppendFormat("{{{0}}}", colSb);
                    }

                    jsonStr = string.Format(@"{{""OnlineInfo"":[{0}]}}", jsonSb);
                }
                catch (Exception e)
                {
                    CommonFunction.WriteErrorLog("When GetOnlineInfo SysType=[" + sysType + "] Error: " + e.Message);
                }

                ReturnJson(jsonStr);
            }
            else
            {
                string appUid = SYS_TYPE_AIR.Equals(sysType.ToLower()) ? APP_UID_AIR : APP_UID_WATER;

                try
                {
                    string sql = string.Format(@"
                                            SELECT  online.PointId as portId, smp.monitoringPointName as  PortName, 
                                            case isNull(IsOnline, 0) when 1 then 'True' when 8 then 'True' when 0 then 'False' else 'False' end as IsOnline,
                                            Recent24HourRecords RecordCount,NewDataTime LastestTime 
                                            FROM [dbo].[DataOnline] as online   left join  [dbo].[SY_MonitoringPoint] smp on online.PointId=smp.PointId
inner join [dbo].[SY_MonitoringPoint] mt on mt.PointId = online.PointId inner join dbo.SY_View_CodeMainItem svcRG on mt.RegionUid = svcRG.ItemGuid
                                            where online.ApplicationUid= '{0}' and [DataTypeUid]='1b6367f1-5287-4c14-b120-7a35bd176db1'
order by svcRG.sortNumber desc,mt.OrderByNum desc
                                            ", appUid);

                    DataTable dt = DBHelper.GetDataView(sql, null, DBHelper.GetConnectionString(AMS_MonitorBusiness_Conn));
                    StringBuilder jsonSb = new StringBuilder();
                    foreach (DataRowView row in dt.DefaultView)
                    {
                        if (jsonSb.Length != 0)
                            jsonSb.Append(",");
                        row["LastestTime"] = Convert.ToDateTime(Convert.ToDateTime(row["LastestTime"]).ToString("yyyy-MM-dd HH:00:00"));
                        StringBuilder colSb = new StringBuilder();
                        foreach (System.Data.DataColumn col in dt.Columns)
                        {
                            if (colSb.Length != 0) colSb.Append(",");
                            colSb.AppendFormat(@"""{0}"":""{1}""", col.ColumnName, System.Convert.IsDBNull(row[col.ColumnName]) ? "--" : row[col.ColumnName]);
                        }
                        jsonSb.AppendFormat("{{{0}}}", colSb);
                    }

                    jsonStr = string.Format(@"{{""OnlineInfo"":[{0}]}}", jsonSb);
                }
                catch (Exception e)
                {
                    CommonFunction.WriteErrorLog("When GetOnlineInfo SysType=[" + sysType + "] Error: " + e.Message);
                }

                ReturnJson(jsonStr);
            }
        }

        /// <summary>
        /// {"ClassInfo":[{"Class":"优","Value":"0","Percentage":"非数字"},{"Class":"良","Value":"0","Percentage":"非数字"},{"Class":"轻度污染","Value":"0","Percentage":"非数字"},{"Class":"中度污染","Value":"0","Percentage":"非数字"},{"Class":"重度污染","Value":"0","Percentage":"非数字"},{"Class":"严重污染","Value":"0","Percentage":"非数字"}]}
        /// </summary>
        /// <param name="portId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        [WebMethod(Description = @"【气_优良天数自定义统计】")]
        public void GetAirClassDayStruct(string portId, string startTime, string endTime)
        {
            string jsonStr = string.Empty;

            string tableName = "AirRelease.TB_DayAQI";

            if ("0".Equals(portId))
            {
                tableName = "AirReport.TB_RegionDayAQIReport";
            }

            //if ("0".Equals(portId))
            //{
            //    tableName = "[AMS_AirAutoMonitor].[Air].[TB_OriRegionDayAQIReport]";
            //}

            try
            {
                string sql = string.Format(@"
                                          select sum(1) as Value,
                                                 [Class] 
                                            from {0} 
                                           where AQIValue is not null 
                                             and [Class] is not null
                                             and {1} >= '{2}' 
                                             and {1} <= '{3}' 
                                             {4} 
                                           group by [Class] 
                                           order by Value desc"
                                             , tableName
                                             , "0".Equals(portId) ? "ReportDateTime" : "DateTime"
                                             , startTime
                                             , endTime
                                             , "0".Equals(portId)
                                                   ? string.Format("and MonitoringRegionUid = '{0}'", monitorRegionUid)
                                                   : "and PointId=" + portId);

                DataTable dt = DBHelper.GetDataView(sql, null, DBHelper.GetConnectionString(AMS_MonitorBusiness_Conn));

                int sumCount = 0;

                if (dt != null && dt.Rows.Count > 0)
                {
                    sumCount = int.Parse(dt.Compute("Sum(Value)", "").ToString());
                }

                StringBuilder jsonSb = new StringBuilder();

                foreach (System.Data.DataRowView row in dt.DefaultView)
                {
                    if (jsonSb.Length != 0)
                        jsonSb.Append(",");

                    StringBuilder colSb = new StringBuilder();
                    foreach (DataColumn col in dt.Columns)
                    {
                        if (colSb.Length != 0)
                        {
                            colSb.Append(",");
                        }

                        colSb.AppendFormat(@"""{0}"":""{1}""", col.ColumnName, System.Convert.IsDBNull(row[col.ColumnName]) ? "--" : row[col.ColumnName]);
                    }

                    colSb.AppendFormat(@",""Percentage"":""{0}""", Math.Round(Convert.ToDouble(row["Value"]) * 100 / sumCount, 2));

                    jsonSb.AppendFormat("{{{0}}}", colSb);
                }

                jsonStr = string.Format(@"{{""ClassInfo"":[{0}]}}", jsonSb);
            }
            catch (Exception e)
            {
                //CommonFunction.WriteErrorLog("When GetAirClassDayStruct PortId=[" + portId + "] Error: " + e.Message);
                CommonFunction.WriteErrorLog("Error: " + e.Message);
            }

            //CommonFunction.WriteInfoLog("Return GetAirClassDayStruct PortId=[" + portId
            //                          + "] StartTime=[" + startTime + "] EndTime=[" + endTime
            //                          + "] Result=[" + jsonStr + "]");

            ReturnJson(jsonStr);
        }

        /// <summary>
        /// {"ClassInfo":[{"Month":"2014-11","Class1":"2","Class2":"21","Class3":"5","Class4":"1","Class5":"0","Class6":"0"},{"Month":"2014-12","Class1":"0","Class2":"19","Class3":"12","Class4":"0","Class5":"0","Class6":"0"},{"Month":"2015-01","Class1":"2","Class2":"9","Class3":"13","Class4":"5","Class5":"2","Class6":"0"},{"Month":"2015-02","Class1":"6","Class2":"14","Class3":"4","Class4":"1","Class5":"2","Class6":"0"},{"Month":"2015-03","Class1":"2","Class2":"20","Class3":"7","Class4":"0","Class5":"0","Class6":"0"},{"Month":"2015-04","Class1":"2","Class2":"17","Class3":"11","Class4":"0","Class5":"0","Class6":"0"},{"Month":"2015-05","Class1":"2","Class2":"20","Class3":"8","Class4":"0","Class5":"0","Class6":"0"},{"Month":"2015-06","Class1":"8","Class2":"15","Class3":"4","Class4":"0","Class5":"0","Class6":"0"},{"Month":"2015-07","Class1":"4","Class2":"22","Class3":"3","Class4":"1","Class5":"0","Class6":"0"},{"Month":"2015-08","Class1":"6","Class2":"10","Class3":"12","Class4":"2","Class5":"0","Class6":"0"},{"Month":"2015-09","Class1":"3","Class2":"21","Class3":"6","Class4":"0","Class5":"0","Class6":"0"},{"Month":"2015-10","Class1":"4","Class2":"18","Class3":"8","Class4":"1","Class5":"0","Class6":"0"},{"Month":"2015-11","Class1":"6","Class2":"12","Class3":"4","Class4":"1","Class5":"0","Class6":"0"}]}
        /// </summary>
        /// <param name="portId"></param>
        /// <param name="topN"></param>
        [WebMethod(Description = @"【气_优良天数月统计】")]
        public void GetAirClassMonthStruct(string portId, string topN)
        {
            string jsonStr = string.Empty;

            string tableName = "AirRelease.TB_DayAQI";

            if (portId == "0")
            {
                tableName = "AirReport.TB_RegionDayAQIReport";
            }

            int preMonth = 12;
            try
            {
                preMonth = Convert.ToInt32(topN);
            }
            catch (Exception e)
            {
                //CommonFunction.WriteErrorLog("When GetAirClassMonthStruct PortId=[" + portId
                //                           + "] Parse PreMonth=[" + topN + "] Error, Use Default Value="
                //                           + preMonth + " ErrInfo: " + e.Message);
                CommonFunction.WriteErrorLog("ErrInfo: " + e.Message);
                preMonth = 12;
            }

            //数据库里DateTime字段存放时间比实际大一天
            DateTime startTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-preMonth);
            DateTime endTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1);

            try
            {
                string sql = string.Format(@"
                                        select * 
                                          from (select convert(varchar(7), dateadd(dd, 0, {0}), 120) as [Month],
                                                       [Class],
                                                       isNull(sum(1),0) DayCount
                                                 from {1} 
                                                where AQIValue is not null 
                                                  and {0} >= '{2}' 
                                                  and {0} < '{3}' 
                                                  {4}
                                                group by convert(varchar(7),dateadd(dd,0,{0}),120), [Class]) as a
                                          pivot(Sum(DayCount) for [Class] in(优, 良, 轻度污染, 中度污染, 重度污染, 严重污染)) as p "
                                          , "0".Equals(portId) ? "ReportDateTime" : "DateTime"
                                          , tableName
                                          , startTime
                                          , endTime
                                          , "0".Equals(portId)
                                               ? string.Format("and MonitoringRegionUid = '{0}'", monitorRegionUid)
                                               : "and PointId=" + portId);

                StringBuilder jsonSb = new StringBuilder();
                DataTable dt = DBHelper.GetDataView(sql, null, DBHelper.GetConnectionString(AMS_MonitorBusiness_Conn));
                DataView dv = dt.DefaultView;
                for (DateTime day = startTime; day < endTime; day = day.AddMonths(1))
                {
                    dv.RowFilter = "[Month] = '" + day.ToString("yyyy-MM") + "'";
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        if (jsonSb.Length != 0)
                            jsonSb.Append(",");

                        StringBuilder colSb = new StringBuilder();
                        foreach (DataColumn col in dt.Columns)
                        {
                            if (colSb.Length != 0) colSb.Append(",");

                            if (col.ColumnName == "Month")
                                colSb.AppendFormat(@"""{0}"":""{1}""", col.ColumnName, Convert.IsDBNull(dv.ToTable().Rows[0][col.ColumnName]) ? "0" : dv.ToTable().Rows[0][col.ColumnName]);
                            else
                                colSb.AppendFormat(@"""{0}"":""{1}""", GetClassNumName(col.ColumnName), Convert.IsDBNull(dv.ToTable().Rows[0][col.ColumnName]) ? "0" : dv.ToTable().Rows[0][col.ColumnName]);
                        }

                        jsonSb.AppendFormat("{{{0}}}", colSb);
                    }
                }

                jsonStr = string.Format(@"{{""ClassInfo"":[{0}]}}", jsonSb);
            }
            catch (Exception e)
            {
                //CommonFunction.WriteErrorLog("When GetAirClassMonthStruct PortId=[" + portId + "] Error: " + e.Message);
                CommonFunction.WriteErrorLog("Error: " + e.Message);
            }

            //CommonFunction.WriteInfoLog("Return GetAirClassMonthStruct PortId=[" + portId
            //                          + "] PreMonth=[" + topN + "] Result=[" + jsonStr + "]");

            ReturnJson(jsonStr);
        }

        [WebMethod(Description = @"【版本】获取安卓公众版更新信息")]
        public void GetVersion()
        {
            string verName = ConfigurationManager.AppSettings["verName"];
            string verCode = ConfigurationManager.AppSettings["verCode"];
            string verDesc = ConfigurationManager.AppSettings["verDesc"];
            string verPath = ConfigurationManager.AppSettings["verPath"];
            ReturnJson("{\"Version\":[{\"verName\":\"" + verName + "\",\"verCode\":\"" + verCode + "\",\"verDesc\":\"" + verDesc + "\",\"verPath\":\"" + verPath + "\"}]}");
        }

        [WebMethod(Description = @"【版本】获取安卓专业版更新信息")]
        public void GetVersionForPro()
        {
            string verName = ConfigurationManager.AppSettings["verNamePro"];
            string verCode = ConfigurationManager.AppSettings["verCodePro"];
            string verDesc = ConfigurationManager.AppSettings["verDescPro"];
            string verPath = ConfigurationManager.AppSettings["verPathPro"];
            ReturnJson("{\"Version\":[{\"verName\":\"" + verName + "\",\"verCode\":\"" + verCode + "\",\"verDesc\":\"" + verDesc + "\",\"verPath\":\"" + verPath + "\"}]}");
        }

        [WebMethod(Description = @"【版本】获取IOS公众版更新信息")]
        public void GetVersionForIOS()
        {
            string verName = ConfigurationManager.AppSettings["verNameIOS"];
            string verCode = ConfigurationManager.AppSettings["verCodeIOS"];
            string verDesc = ConfigurationManager.AppSettings["verDescIOS"];
            string verPath = ConfigurationManager.AppSettings["verPathIOS"];
            ReturnJson("{\"Version\":[{\"verName\":\"" + verName + "\",\"verCode\":\"" + verCode + "\",\"verDesc\":\"" + verDesc + "\",\"verPath\":\"" + verPath + "\"}]}");
        }

        [WebMethod(Description = @"【版本】获取IOS专业版更新信息")]
        public void GetVersionForIOSPro()
        {
            string verName = ConfigurationManager.AppSettings["verNameIOSPro"];
            string verCode = ConfigurationManager.AppSettings["verCodeIOSPro"];
            string verDesc = ConfigurationManager.AppSettings["verDescIOSPro"];
            string verPath = ConfigurationManager.AppSettings["verPathIOSPro"];
            ReturnJson("{\"Version\":[{\"verName\":\"" + verName + "\",\"verCode\":\"" + verCode + "\",\"verDesc\":\"" + verDesc + "\",\"verPath\":\"" + verPath + "\"}]}");
        }

        /// <summary>
        /// {"HourAQI":[{"DateTime":"2015-11-25 14:00","AQI":"31","PrimaryPollutant":"--","RGBValue":"#00e400","Class":"优","Grade":"一级","HealthEffect":"空气质量令人满意，基本无空气污染","AQI1":"各类人群可正常活动"},{"DateTime":"2015-11-25 15:00","AQI":"36","PrimaryPollutant":"--","RGBValue":"#00e400","Class":"优","Grade":"一级","HealthEffect":"空气质量令人满意，基本无空气污染","AQI1":"各类人群可正常活动"},{"DateTime":"2015-11-25 16:00","AQI":"51","PrimaryPollutant":"PM10","RGBValue":"#ffff00","Class":"良","Grade":"二级","HealthEffect":"空气质量可接受，但某些污染物可能对极少数异常敏感人群健康有较弱影响","AQI1":"极少数异常敏感人群应减少户外活动"},{"DateTime":"2015-11-25 17:00","AQI":"58","PrimaryPollutant":"PM2.5","RGBValue":"#ffff00","Class":"良","Grade":"二级","HealthEffect":"空气质量可接受，但某些污染物可能对极少数异常敏感人群健康有较弱影响","AQI1":"极少数异常敏感人群应减少户外活动"},{"DateTime":"2015-11-25 18:00","AQI":"75","PrimaryPollutant":"PM2.5","RGBValue":"#ffff00","Class":"良","Grade":"二级","HealthEffect":"空气质量可接受，但某些污染物可能对极少数异常敏感人群健康有较弱影响","AQI1":"极少数异常敏感人群应减少户外活动"},{"DateTime":"2015-11-25 19:00","AQI":"67","PrimaryPollutant":"PM2.5","RGBValue":"#ffff00","Class":"良","Grade":"二级","HealthEffect":"空气质量可接受，但某些污染物可能对极少数异常敏感人群健康有较弱影响","AQI1":"极少数异常敏感人群应减少户外活动"},{"DateTime":"2015-11-25 20:00","AQI":"62","PrimaryPollutant":"PM2.5","RGBValue":"#ffff00","Class":"良","Grade":"二级","HealthEffect":"空气质量可接受，但某些污染物可能对极少数异常敏感人群健康有较弱影响","AQI1":"极少数异常敏感人群应减少户外活动"}]}
        /// </summary>
        /// <param name="portId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        [WebMethod(Description = @"【气_历史】获取时间区间内小时AQI情况<br />参数说明：<br />portId为站点ID，portId=0代表全市；<br />startTime为开始时间，endTime为结束时间（时间格式：yyyy-MM-dd HH:00）")]
        public void GetHourAQIByDatetime(string portId, string startTime, string endTime)
        {
            string jsonStr = string.Empty;
            string AMS_MonitorBusiness_Con = "AMS_AirAutoMonitor_Conn";
            string tableName = "Air.TB_OriHourAQI";
            //string tableName = "AirRelease.TB_HourAQI";

            if ("0".Equals(portId))
            {
                tableName = "AirRelease.TB_RegionHourAQI";
                AMS_MonitorBusiness_Con = "AMS_MonitorBusiness_Conn";
            }

            try
            {
                string sql = string.Format(@" 
                                       select dateadd(hh,0,datetime) DateTime,
                                              AQIValue AQI,
                                              PrimaryPollutant,
                                              RGBValue,
                                              Class,
                                              Grade,
                                              HealthEffect,
                                              TakeStep 
                                         from {0}
                                        where dateTime >= '{1}' 
                                          and dateTime <= '{2}' 
                                          {3} 
                                        order by dateTime"
                                        , tableName
                                        , startTime
                                        , endTime
                                        , "0".Equals(portId)
                                             ? string.Format(" and MonitoringRegionUid = '{0}'", monitorRegionUid)
                                             : string.Format(" and PointId = {0} ", portId));

                DataTable dt = DBHelper.GetDataView(sql, null, DBHelper.GetConnectionString(AMS_MonitorBusiness_Con));

                StringBuilder jsonSb = new StringBuilder();
                foreach (System.Data.DataRowView row in dt.DefaultView)
                {
                    if (jsonSb.Length != 0)
                        jsonSb.Append(",");

                    StringBuilder colSb = new StringBuilder();
                    foreach (System.Data.DataColumn col in dt.Columns)
                    {
                        if (colSb.Length != 0) colSb.Append(",");

                        if (col.ColumnName.ToLower() == "datetime")
                            colSb.AppendFormat(@"""{0}"":""{1}""", col.ColumnName, string.Format("{0:yyyy-MM-dd HH:00}", row[col.ColumnName]));
                        else
                            colSb.AppendFormat(@"""{0}"":""{1}""", col.ColumnName, System.Convert.IsDBNull(row[col.ColumnName]) ? "--" : row[col.ColumnName]);
                    }

                    jsonSb.AppendFormat("{{{0}}}", colSb);
                }

                jsonStr = string.Format(@"{{""HourAQI"":[{0}]}}", jsonSb);
            }
            catch (Exception e)
            {
                //CommonFunction.WriteErrorLog("When GetHourAQIByDatetime PortId=[" + portId + "] Error: " + e.Message);
                CommonFunction.WriteErrorLog("Error: " + e.Message);
            }

            //CommonFunction.WriteInfoLog("Return GetHourAQIByDatetime PortId=[" + portId
            //                          + "] StartTime=[" + startTime + "] EndTime=[" + endTime
            //                          + "] Result=[" + jsonStr + "]");

            ReturnJson(jsonStr);
        }

        /// <summary>
        /// {"DayAQI":[{"DateTime":"2015-11-24","AQI":"35","PrimaryPollutant":"--","RGBValue":"#00e400","Class":"优","Grade":"一级","HealthEffect":"空气质量令人满意，基本无空气污染","TakeStep":"各类人群可正常活动"},{"DateTime":"2015-11-25","AQI":"--","PrimaryPollutant":"--","RGBValue":"--","Class":"--","Grade":"--","HealthEffect":"--","TakeStep":"--"}]}
        /// </summary>
        /// <param name="portId"></param>
        /// <param name="startDay"></param>
        /// <param name="endDay"></param>
        [WebMethod(Description = @"【气_历史】获取时间区间内日AQI情况<br />参数说明：<br />portId为站点ID，portId=0代表全市，<br />startDay为开始日期，endDay为结束日期（时间格式：yyyy-MM-dd）")]
        public void GetDayAQIByDatetime(string portId, string startDay, string endDay)
        {
            string jsonStr = string.Empty;

            //string tableName = "AirRelease.TB_DayAQI";

            //if ("0".Equals(portId))
            //{
            //    tableName = "AirReport.TB_RegionDayAQIReport";
            //}
            string AMS_MonitorBusiness_Con = "AMS_AirAutoMonitor_Conn";
            string tableName = "Air.TB_OriDayAQI";
            //string tableName = "AirRelease.TB_HourAQI";

            if ("0".Equals(portId))
            {
                tableName = "AirReport.TB_RegionDayAQIReport";
                AMS_MonitorBusiness_Con = "AMS_MonitorBusiness_Conn";
            }
            try
            {
                startDay = Convert.ToDateTime(startDay).AddDays(0).ToString();
                endDay = Convert.ToDateTime(endDay).AddDays(0).ToString();

                string sql = string.Format(@" 
                                        SELECT {0} as DateTime,
                                               AQIValue AQI,
                                               PrimaryPollutant,
                                               RGBValue,
                                               Class,
                                               Grade,
                                               HealthEffect,
                                               TakeStep 
                                         FROM {1}
                                        WHERE {0} >= '{2}' 
                                          and {0} <= '{3}' 
                                          {4} 
                                        order by {0}"
                                           , "0".Equals(portId) ? "ReportDateTime" : "DateTime"
                                           , tableName
                                           , startDay
                                           , endDay
                                           , "0".Equals(portId)
                                               ? string.Format(" and MonitoringRegionUid = '{0}'", monitorRegionUid)
                                               : string.Format(" and PointId = {0} ", portId));

                DataTable dt = DBHelper.GetDataView(sql, null, DBHelper.GetConnectionString(AMS_MonitorBusiness_Con));

                StringBuilder jsonSb = new StringBuilder();
                foreach (System.Data.DataRowView row in dt.DefaultView)
                {
                    if (jsonSb.Length != 0)
                        jsonSb.Append(",");

                    StringBuilder colSb = new StringBuilder();
                    foreach (System.Data.DataColumn col in dt.Columns)
                    {
                        if (colSb.Length != 0) colSb.Append(",");

                        if (col.ColumnName.ToLower() == "datetime")
                            colSb.AppendFormat(@"""{0}"":""{1}""", col.ColumnName, string.Format("{0:yyyy-MM-dd}", row[col.ColumnName]));
                        else
                            colSb.AppendFormat(@"""{0}"":""{1}""", col.ColumnName, System.Convert.IsDBNull(row[col.ColumnName]) ? "--" : row[col.ColumnName]);
                    }

                    jsonSb.AppendFormat("{{{0}}}", colSb);
                }

                jsonStr = string.Format(@"{{""DayAQI"":[{0}]}}", jsonSb);
            }
            catch (Exception e)
            {
                //CommonFunction.WriteErrorLog("When GetDayAQIByDatetime PortId=[" + portId + "] Error: " + e.Message);
                CommonFunction.WriteErrorLog("Error: " + e.Message);
            }

            //CommonFunction.WriteInfoLog("Return GetDayAQIByDatetime PortId=[" + portId
            //                          + "] StartDay=[" + startDay + "] EndDay=[" + endDay
            //                          + "] Result=[" + jsonStr + "]");

            ReturnJson(jsonStr);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="portId">站点ID</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="timeType">数据类型:Hour-小时数据;Day-日数据</param>
        [WebMethod(Description = @"获取首要污染物及浓度情况<br />参数说明：<br />portId为站点ID，portId=0代表全市，<br />timeType为数据类型：Hour-小时数据;Day-日数据，<br />startTime为开始日期，endTime为结束日期（小时数据时间格式：yyyy-MM-dd HH:mm:ss;日数据时间格式：yyyy-MM-dd）")]
        public void GetPrimaryPollutant(string portId, string startTime, string endTime, string timeType)
        {
            string sql = string.Empty;
            string tableName = string.Empty;
            
            //string result = "[";
            string result = "{"+string.Format(@"""reason"":""成功的返回"",""data"":[");
            string PrimaryPollutant = string.Empty;
            string polluted = string.Empty;
            string factor=string.Empty;
            try 
            {
                if (timeType == "Hour")
                {
                    tableName = "AirRelease.TB_HourAQI";
                    if ("0".Equals(portId))
                    {
                        tableName = "AirRelease.TB_RegionHourAQI";
                        if (startTime == "" && endTime=="")
                        {
                            sql = string.Format("select * from {0} where MonitoringRegionUid='{1}' and DateTime>='{2}' and DateTime<='{3}' order by DateTime", tableName, monitorRegionUid, DateTime.Now.AddHours(-24).ToString("yyyy-MM-dd HH:00:00"), DateTime.Now.ToString("yyyy-MM-dd HH:00:00"));
                        }
                        else
                        {
                            sql = string.Format("select * from {0} where MonitoringRegionUid='{1}' and DateTime>='{2}' and DateTime<='{3}' order by DateTime", tableName, monitorRegionUid, startTime, endTime);
                        }
                        
                    }
                    else
                    {
                        if (startTime == "" && endTime == "")
                        {
                            sql = string.Format("select * from {0} where PointId='{1}' and DateTime>='{2}' and DateTime<='{3}' order by DateTime", tableName, portId, DateTime.Now.AddHours(-24).ToString("yyyy-MM-dd HH:00:00"), DateTime.Now.ToString("yyyy-MM-dd HH:00:00"));
                        }
                        else
                        {
                            sql = string.Format("select * from {0} where PointId='{1}' and DateTime>='{2}' and DateTime<='{3}' order by DateTime", tableName, portId, startTime, endTime);
                        }
                        
                    }

                }
                else if (timeType == "Day")
                {
                    tableName = "AirRelease.TB_DayAQI";
                    if ("0".Equals(portId))
                    {
                        tableName = "AirReport.TB_RegionDayAQIReport";
                        if (startTime == "" && endTime == "")
                        {
                            sql = string.Format("select * from {0} where MonitoringRegionUid='{1}' and ReportDateTime>='{2}' and ReportDateTime<='{3}' order by ReportDateTime", tableName, monitorRegionUid, DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd"), DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
                        }
                        else
                        {
                            sql = string.Format("select * from {0} where MonitoringRegionUid='{1}' and ReportDateTime>='{2}' and ReportDateTime<='{3}' order by ReportDateTime", tableName, monitorRegionUid, startTime, endTime);
                        }
                        
                    }
                    else
                    {
                        if (startTime == "" && endTime == "")
                        {
                            sql = string.Format("select * from {0} where PointId='{1}' and DateTime>='{2}' and DateTime<='{3}' order by DateTime", tableName, portId, DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"));
                        }
                        else
                        {
                            sql = string.Format("select * from {0} where PointId='{1}' and DateTime>='{2}' and DateTime<='{3}' order by DateTime", tableName, portId, startTime, endTime);
                        }
                        
                    }
                }
                DataTable dt = DBHelper.GetDataView(sql, null, DBHelper.GetConnectionString(AMS_MonitorBusiness_Conn));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string[] pollutant = dt.Rows[i]["PrimaryPollutant"].ToString().Split(',');
                    for (int k = 0; k < pollutant.Length; k++)
                    {
                        PrimaryPollutant = "";
                        if (pollutant[k] == "PM2.5")
                        {
                            polluted = pollutant[k];
                            factor = DecimalExtension.GetPollutantValue((Convert.ToDecimal(dt.Rows[i]["PM25"].ToString()) * 1000),0).ToString();
                        }
                        else
                        {
                            if (pollutant[k] == "O3")
                            {
                                polluted = pollutant[k];
                                if (timeType == "Day")
                                {
                                    factor = DecimalExtension.GetPollutantValue((Convert.ToDecimal(dt.Rows[i]["Max8HourO3"].ToString()) * 1000),0).ToString();
                                }
                                else
                                {
                                    factor = DecimalExtension.GetPollutantValue((Convert.ToDecimal(dt.Rows[i][pollutant[k]].ToString()) * 1000),0).ToString();
                                }

                            }
                            else if (pollutant[k] == "CO")
                            {
                                polluted = pollutant[k];
                                factor = dt.Rows[i][pollutant[k]].ToString();
                            }
                            else if (pollutant[k] == "" || pollutant[k] == "--")
                            {
                                polluted = "--";
                                factor = "--";
                            }
                            else
                            {
                                polluted = pollutant[k];
                                factor = DecimalExtension.GetPollutantValue((Convert.ToDecimal(dt.Rows[i][pollutant[k]].ToString()) * 1000),0).ToString();
                            }
                        }
                        PrimaryPollutant += "{" + string.Format(@"""factor"":""{0}"",""factorValue"":""{1}""", polluted, factor) + "},";
                    }
                    if (timeType == "Hour")
                    {
                        result += string.Format(@"{{""DateTime"":""{0}"",""Value"":[{1}]}}", Convert.ToDateTime(dt.Rows[i]["DateTime"].ToString()).ToString("yyyy-MM-dd HH:00:00"), PrimaryPollutant.TrimEnd(',')) + ",";
                    }
                    else
                    {
                        if ("0".Equals(portId))
                        {
                            result += string.Format(@"{{""DateTime"":""{0}"",""Value"":[{1}]}}", Convert.ToDateTime(dt.Rows[i]["ReportDateTime"].ToString()).ToString("yyyy-MM-dd"), PrimaryPollutant.TrimEnd(',')) + ",";
                        }
                        else
                        {
                            result += string.Format(@"{{""DateTime"":""{0}"",""Value"":[{1}]}}", Convert.ToDateTime(dt.Rows[i]["DateTime"].ToString()).ToString("yyyy-MM-dd"), PrimaryPollutant.TrimEnd(',')) + ",";
                        }

                    }
                }
            }
            catch(Exception ex)
            {
                CommonFunction.WriteErrorLog("Error: " + ex.Message);
            }
            ReturnJson(result.TrimEnd(',')+"]}");
        }
        /// <summary>
        /// {"HistoryData":[{"DateTime":"11-26 08:00","Value":[{"factor": "PM2.5","value": "0.043","isExceeded":"False"},{"factor": "PM10","value": "0.061","isExceeded":"False"},{"factor": "CO","value": "--","isExceeded":"False"},{"factor": "O3","value": "--","isExceeded":"False"},{"factor": "SO2","value": "--","isExceeded":"False"},{"factor": "NO2","value": "--","isExceeded":"False"}]},{"DateTime":"11-26 07:00","Value":[{"factor": "PM2.5","value": "0.043","isExceeded":"False"},{"factor": "PM10","value": "0.057","isExceeded":"False"},{"factor": "CO","value": "1.227","isExceeded":"False"},{"factor": "O3","value": "0.040","isExceeded":"False"},{"factor": "SO2","value": "0.020","isExceeded":"False"},{"factor": "NO2","value": "0.025","isExceeded":"False"}]},{"DateTime":"11-26 06:00","Value":[{"factor": "PM2.5","value": "0.043","isExceeded":"False"},{"factor": "PM10","value": "0.055","isExceeded":"False"},{"factor": "CO","value": "1.189","isExceeded":"False"},{"factor": "O3","value": "0.044","isExceeded":"False"},{"factor": "SO2","value": "0.020","isExceeded":"False"},{"factor": "NO2","value": "0.022","isExceeded":"False"}]}]}
        /// </summary>
        /// <param name="portId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="userUid"></param>
        [WebMethod(Description = @"【气_监测数据】获取时间区间内小时浓度值<br />参数说明：portId:站点ID")]
        public void GetHourDataByDatetime(string portId, string startTime, string endTime, string userUid)
        {
            string result = string.Empty;

            if ("0".Equals(portId))
            {
                #region [全市AQI小时数据]
                bool isOver = false;
                double alertUpper = 0;
                                                      //                PM25 as [PM2.5],
                                                      //PM10,
                                                      //CO as 一氧化碳,
                                                      //O3 as 臭氧,
                                                      //SO2 as 二氧化硫,
                                                      //NO2 as 二氧化氮 
                try
                {
                    string sqlCity = string.Format(@"
                                               select dateTime,SO2,NO2,CO,O3,PM10,PM25 as [PM2.5]
                                                 from AirRelease.TB_RegionHourAQI
                                                where MonitoringRegionUid = '{0}'
                                                  and dateTime >= '{1}' 
                                                  and dateTime <='{2}'
                                                order by dateTime desc"
                                                 , monitorRegionUid
                                                 , startTime
                                                 , endTime);

                    DataTable dtCityData = DBHelper.GetDataView(sqlCity, null, DBHelper.GetConnectionString(AMS_MonitorBusiness_Conn));
                    for (int i = 0; i < dtCityData.Columns.Count; i++)
                    {
                        if (dtCityData.Columns[i].ColumnName == "dateTime")
                        {
                            for (int j = 0; j < dtCityData.Rows.Count; j++)
                            {
                                DateTime dt = Convert.ToDateTime(dtCityData.Rows[j][i].ToString());
                                dtCityData.Rows[j][i] = Convert.ToDateTime(dt.ToString("yyyy-MM-dd HH:00:00"));
                            }
                        }
                        else if (dtCityData.Columns[i].ColumnName == "PM2.5" || dtCityData.Columns[i].ColumnName == "PM10" || dtCityData.Columns[i].ColumnName == "SO2" || dtCityData.Columns[i].ColumnName == "O3" || dtCityData.Columns[i].ColumnName == "NO2")
                        {
                            for (int j = 0; j < dtCityData.Rows.Count; j++)
                            {
                                int DecimalNum = 3;
                                decimal pollutantValue = decimal.TryParse(dtCityData.Rows[j][i].ToString(), out pollutantValue) ? pollutantValue : 0;
                                dtCityData.Rows[j][i] = ((DecimalExtension.GetPollutantValue(pollutantValue, DecimalNum)) * 1000).ToString("G0");
                            }
                        }
                        else if (dtCityData.Columns[i].ColumnName == "CO")
                        {
                            for (int j = 0; j < dtCityData.Rows.Count; j++)
                            {
                                int DecimalNum = 1;
                                decimal pollutantValue = decimal.TryParse(dtCityData.Rows[j][i].ToString(), out pollutantValue) ? pollutantValue : 0;
                                dtCityData.Rows[j][i] = (DecimalExtension.GetPollutantValue(pollutantValue, DecimalNum)).ToString();
                            }
                        }
                    }
                    StringBuilder jsonSb = new StringBuilder();
                    foreach (System.Data.DataRowView row in dtCityData.DefaultView)
                    {
                        if (jsonSb.Length != 0)
                        {
                            jsonSb.Append(",");
                        }

                        StringBuilder colSb = new StringBuilder();

                        for (int i = 1; i < dtCityData.Columns.Count; i++)
                        {
                            if (!Convert.IsDBNull(row[dtCityData.Columns[i].ColumnName]))
                            {
                                alertUpper = GetAirAlertUpper(dtCityData.Columns[i].ColumnName);
                                if (Convert.ToDouble(row[dtCityData.Columns[i].ColumnName]) > alertUpper)
                                    isOver = true;
                                else isOver = false;
                            }
                            else isOver = false;

                            colSb.AppendFormat(@"{{""factor"": ""{0}"",""value"": ""{1}"",""isExceeded"":""{2}"",""measureUnit"":""{3}""}},", dtCityData.Columns[i].ColumnName, Convert.IsDBNull(row[dtCityData.Columns[i].ColumnName]) ? "--" : Math.Round(Convert.ToDecimal(row[dtCityData.Columns[i].ColumnName]), 3).ToString(), isOver.ToString(), dtCityData.Columns[i].ColumnName == "CO" ? "mg/m³" : "μg / m³");
                        }

                        jsonSb.AppendFormat(@"{{""DateTime"":""{0:MM-dd HH:mm}"",""Value"":[{1}]}}", row["dateTime"], colSb.ToString().TrimEnd(','));

                    }

                    result = string.Format(@"{{""HistoryData"":[{0}]}}", jsonSb);
                }
                catch (Exception e)
                {
                    //CommonFunction.WriteErrorLog("When GetHourDataByDatetime PortId=[" + portId + "] Error: " + e.Message);
                    CommonFunction.WriteErrorLog("Error: " + e.Message);
                }

                //CommonFunction.WriteInfoLog("Return GetHourDataByDatetime PortId=[" + portId
                //                          + "] StartTime=[" + startTime + "] EndTime=[" + endTime
                //                          + "] Result=[" + result + "]");

                ReturnJson(result);

                #endregion
            }
            else
            {
                #region 【气实时小时数据】
                try
                {
                    string queryTstampSql = string.Format(@"
                                            select Tstamp 
                                              from Air.TB_InfectantBy60
                                             where PointId = {0}
                                               and Tstamp >= '{1}'
                                               and Tstamp <= '{2}'
                                             group by Tstamp 
                                             order by Tstamp desc
                                           ", portId
                                            , startTime
                                            , endTime);

                    DataTable tstampDt = DBHelper.GetDataView(queryTstampSql, null, DBHelper.GetConnectionString(AMS_AirAutoMonitor_Conn));

                    if (tstampDt == null || tstampDt.Rows.Count == 0)
                    {
                        CommonFunction.WriteInfoLog("When GetHourDataByDatetime Not Query Data.....");
                    }
                    else
                    {
                        string queryDataSql = null;
                        DataTable dataDt = null;
                        StringBuilder jsonSb = new StringBuilder();
                        StringBuilder tstampFactorsSb = null;
                        string userStr = "";
                        if (userUid != "")
                            userStr = string.Format(@" and syps.UserUid = '{0}'", userUid);
                        foreach (DataRow tstampDr in tstampDt.Rows)
                        {
                            queryDataSql = string.Format(@"

                                       select distinct ati.PollutantCode,
										syp.ChemicalSymbol PollutantName,
										dbo.F_Round(ati.PollutantValue, syp.DecimalDigit) as PollutantValue,
										(case when CHARINDEX('HSp', Mark) > 0 then 'True' when CHARINDEX('LSp', Mark) > 0 then 'True' else 'False' end) as IsExceeded, 
										syp.OrderByNum 
										from  Air.TB_InfectantBy60 ati
										left join dbo.SY_PollutantCode syp
										on ati.PollutantCode = syp.PollutantCode
                                        and ati.PollutantCode in ('a34004','a34002','a21005','a05024','a21026','a21004')
										and ati.PointId = {2}
										and ati.Tstamp = '{3}'
										and ati.id = (select MAX(id) 
										from Air.TB_InfectantBy60 temp
										where temp.PointId = ati.PointId
										and temp.PollutantCode = ati.PollutantCode 
										and temp.Tstamp = ati.Tstamp) 
										inner join    dbo.SY_PersonalizedSettings syps
										on syps.ParameterUid = syp.PollutantUid
                                        {0}
										and syps.ApplicationUid = '{1}' 
										and syps.EnableCustomOrNot = 1 
										order by syp.OrderByNum desc
                                           ", userStr
                                            , APP_UID_AIR
                                            , portId
                                            , tstampDr["Tstamp"]);

                            dataDt = DBHelper.GetDataView(queryDataSql, null, DBHelper.GetConnectionString(AMS_AirAutoMonitor_Conn));
                            for (int i = 0; i < dataDt.Rows.Count; i++)
                            {
                                if (dataDt.Rows[i]["PollutantCode"].ToString() == "a21026" || dataDt.Rows[i]["PollutantCode"].ToString() == "a21004" || dataDt.Rows[i]["PollutantCode"].ToString() == "a05024" || dataDt.Rows[i]["PollutantCode"].ToString() == "a34002" || dataDt.Rows[i]["PollutantCode"].ToString() == "a34004")
                                {
                                    int DecimalNum = 3;
                                    decimal pollutantValue = decimal.TryParse(dataDt.Rows[i]["PollutantValue"].ToString(), out pollutantValue) ? pollutantValue : 0;
                                    dataDt.Rows[i]["PollutantValue"] = ((DecimalExtension.GetPollutantValue(pollutantValue, DecimalNum)) * 1000).ToString("G0");
                                }
                                else if (dataDt.Rows[i]["PollutantCode"].ToString() == "a21005")
                                {
                                    int DecimalNum = 1;
                                    decimal pollutantValue = decimal.TryParse(dataDt.Rows[i]["PollutantValue"].ToString(), out pollutantValue) ? pollutantValue : 0;
                                    dataDt.Rows[i]["PollutantValue"] = (DecimalExtension.GetPollutantValue(pollutantValue, DecimalNum)).ToString();
                                }
                            }
                            tstampFactorsSb = new StringBuilder();

                            foreach (DataRow dataDr in dataDt.Rows)
                            {

                                tstampFactorsSb.AppendFormat(@"{{""factor"": ""{0}"",""value"": ""{1}"",""isExceeded"":""{2}"",""measureUnit"":""{3}""}},"
                                                                    , dataDr["PollutantName"].ToString()
                                                                    , (dataDr["PollutantValue"] == DBNull.Value) ? "--" : dataDr["PollutantValue"].ToString()
                                                                    , dataDr["IsExceeded"].ToString()
                                                                    , dataDr["PollutantName"].ToString() == "CO" ? "mg/m³" : "μg / m³");
                            }

                            jsonSb.AppendFormat(@"{{""DateTime"":""{0:MM-dd HH:00}"",""Value"":[{1}]}},"
                                               , tstampDr["Tstamp"]
                                               , tstampFactorsSb.ToString().TrimEnd(','));
                        }

                        //删除最后一个，
                        jsonSb.Remove((jsonSb.Length - 1), 1);

                        result = string.Format(@"{{""HistoryData"":[{0}]}}", jsonSb.ToString());
                    }
                }
                catch (Exception e)
                {
                    CommonFunction.WriteErrorLog("When GetHourDataByDatetime PortId=[" + portId + "] Error: " + e.Message);
                }

                CommonFunction.WriteInfoLog("Return GetHourDataByDatetime PortId=[" + portId
                                          + "] StartTime=[" + startTime + "] EndTime=[" + endTime
                                          + "] UserUid=[" + userUid + "] Result=[" + result + "]");

                ReturnJson(result);

                #endregion
            }
        }

        /// <summary>
        /// {"PortHourAQI":[{"PortId":"2","PortName":"吴江区环保局","DateTime":"2015-11-26 10:00","SO2_IAQI":"1","NO2_IAQI":"13","PM10_IAQI":"53","CO_IAQI":"--","O3_IAQI":"13","PM2.5_IAQI":"55","AQI":"--","PrimaryPollutant":"--","Class":"--","Grade":"--","RGBValue":"--","HealthEffect":"--","TakeStep":"--"}]}
        /// </summary>
        /// <param name="portId"></param>
        [WebMethod(Description = @"【气_GIS】获取所有点位小时空气质量状况<br />")]
        public void GetLatestHourAQIByAll(string portId)
        {
            string jsonStr = string.Empty;

            string tableName = "AirRelease.TB_HourAQI";

            if ("0".Equals(portId))
            {
                tableName = "AirRelease.TB_RegionHourAQI";
            }

            try
            {
                string sql = string.Format(@"
                                      select {0} PortName,
                                             dateadd(hh,0,[datetime]) DateTime,
                                             ISNULL(SO2_IAQI,'--') SO2_IAQI,
                                             ISNULL(NO2_IAQI,'--') NO2_IAQI,
                                             ISNULL(PM10_IAQI,'--') PM10_IAQI,
                                             ISNULL(CO_IAQI,'--') CO_IAQI,
                                             ISNULL(O3_IAQI,'--') O3_IAQI,
                                             ISNULL([PM25_IAQI],'--') [PM25_IAQI],
                                             ISNULL(AQIValue,'--') AQI,
                                             ISNULL(PrimaryPollutant,'--') PrimaryPollutant,
                                             ISNULL(Class,'--') Class, 
                                             ISNULL(Grade,'--') Grade,
                                             ISNULL(RGBValue,'--') RGBValue,
                                             ISNULL(HealthEffect,'--') HealthEffect,
                                             ISNULL(TakeStep,'--') TakeStep 
                                        from {1}  AS v 
                                        {2} 
                                       where v.[dateTime] = (  
                                                       select MAX([dateTime]) 
                                                         from {1} 
                                                        where {3} [dateTime] > DATEADD(DAY, -7, GETDATE())
                                                          and [dateTime] < GETDATE()
                                                            ) 
                                         {4}"
                                         , "0".Equals(portId) ? "0 AS PortId,'南通市' as" : "v.PointId as PortId, p.MonitoringPointName as"
                                         , tableName
                                         , "0".Equals(portId) ? "" : "inner join [dbo].[SY_MonitoringPoint] as P on v.PointId = p.PointId  "
                                         , "0".Equals(portId)
                                               ? string.Format(" MonitoringRegionUid = '{0}' and ", monitorRegionUid)
                                               : " PointId = V.PointId and "
                                               , string.IsNullOrEmpty(portId)?"":("0".Equals(portId)
                                               ? string.Format("and v.MonitoringRegionUid = '{0}'", monitorRegionUid)
                                               : string.Format("and v.PointId={0}", portId)));
                                         //, string.IsNullOrEmpty(portId) || "0".Equals(portId)
                                         //      ? string.Format("and v.MonitoringRegionUid = '{0}'", monitorRegionUid)
                                         //      : string.Format("and v.PointId={0}", portId));

                DataTable dt = DBHelper.GetDataView(sql, null, DBHelper.GetConnectionString(AMS_MonitorBusiness_Conn));

                StringBuilder jsonSb = new StringBuilder();
                foreach (System.Data.DataRowView row in dt.DefaultView)
                {
                    if (jsonSb.Length != 0)
                        jsonSb.Append(",");

                    StringBuilder colSb = new StringBuilder();
                    foreach (System.Data.DataColumn col in dt.Columns)
                    {
                        if (colSb.Length != 0) colSb.Append(",");

                        if (col.ColumnName.ToLower() == "datetime")
                            colSb.AppendFormat(@"""{0}"":""{1}""", col.ColumnName, string.Format("{0:yyyy-MM-dd HH:00}", row[col.ColumnName]));
                        else
                            colSb.AppendFormat(@"""{0}"":""{1}""", col.ColumnName, System.Convert.IsDBNull(row[col.ColumnName]) ? "--" : row[col.ColumnName]);
                    }

                    jsonSb.AppendFormat("{{{0}}}", colSb);
                }

                jsonStr = string.Format(@"{{""PortHourAQI"":[{0}]}}", jsonSb);
            }
            catch (Exception e)
            {
                //CommonFunction.WriteErrorLog("When GetLatestHourAQIByAll PortId=[" + portId + "] Error: " + e.Message);
                CommonFunction.WriteErrorLog("Error: " + e.Message);
            }

            //CommonFunction.WriteInfoLog("Return GetLatestHourAQIByAll PortId=[" + portId + "] Result=[" + jsonStr + "]");

            ReturnJson(jsonStr);
        }

        /// <summary>
        /// {"PortHourWQ":[{"PortId":"2","DateTime":"2015-11-26 08:00","Value":[{"factor": "pH值","WQ": "1","value": "7.69"},{"factor": "溶解氧","WQ": "4","value": "3.65"},{"factor": "高锰酸盐指数","WQ": "3","value": "4.80"},{"factor": "氨氮","WQ": "4","value": "1.08"},{"factor": "总磷","WQ": "6","value": "1.637"}]}]}
        /// </summary>
        /// <param name="portId"></param>
        [WebMethod(Description = @"【水_实时水质】获取所有点位小时水质状况<br />")]
        public void GetLatestHourWQ(string portId)
        {
            string jsonStr = string.Empty;

            try
            {
                string queryDataSql = string.Format(@"
                                            select distinct wti.Tstamp, 
                                                   wti.PollutantCode, 
                                                   syp.PollutantName, 
                                                   dbo.F_Round(wti.PollutantValue, syp.DecimalDigit) as PollutantValue, 
                                                   dbo.F_GetPointSinglePollutantHourWQ_LEVEL(wti.PointId, wti.PollutantCode, wti.Tstamp) as WQ
                                              from dbo.SY_Water_InfectantBy60 wti
                                             inner join dbo.SY_PollutantCode syp
                                                on wti.PollutantCode = syp.PollutantCode and syp.IsCalEQIOrNot=1
                                             where wti.PointId = {0} and  dbo.F_GetPointSinglePollutantHourWQ_LEVEL(wti.PointId, wti.PollutantCode, wti.Tstamp)>0
                                               and wti.Tstamp = (select MAX(Tstamp) 
                                                                   from dbo.SY_Water_InfectantBy60 
                                                                  where PointId = {0} 
                                                                    and Tstamp > DATEADD(DAY, -7, GETDATE())
                                                                    and Tstamp < GETDATE()
                                                                 )
                                               and wti.id = (select MAX(id) 
                                                               from dbo.SY_Water_InfectantBy60 temp
                                                              where temp.PointId = wti.PointId
                                                                and temp.PollutantCode = wti.PollutantCode 
                                                                and temp.Tstamp = wti.Tstamp)
                                             order by wti.PollutantCode asc", portId);

                DataTable dataDt = DBHelper.GetDataView(queryDataSql, null, DBHelper.GetConnectionString(AMS_MonitorBusiness_Conn));

                if (dataDt == null || dataDt.Rows.Count == 0)
                {
                    CommonFunction.WriteInfoLog("When GetLatestHourWQ PortId=[" + portId + "] Not Query data....");

                    jsonStr = string.Format(@"{{""PortHourWQ"":[]}}");
                }
                else
                {
                    StringBuilder pollutantSb = new StringBuilder();

                    foreach (DataRow dataDr in dataDt.Rows)
                    {
                        pollutantSb.AppendFormat(@"{{""factor"": ""{0}"",""WQ"": ""{1}"",""value"": ""{2}""}},"
                                                    , dataDr["PollutantName"].ToString()
                                                    , dataDr["WQ"].ToString()
                                                    , (dataDr["PollutantValue"] == DBNull.Value) ? "--" : dataDr["PollutantValue"].ToString());
                    }

                    // 删除最后一个,
                    pollutantSb.Remove(pollutantSb.Length - 1, 1);

                    string tstamp = string.Format("{0:yyyy-MM-dd HH:mm}", dataDt.Rows[0]["Tstamp"]);

                    StringBuilder jsonSb = new StringBuilder();
                    jsonSb.AppendFormat(@"{{""PortHourWQ"":[{{""PortId"":""{0}"",""DateTime"":""{1}"",""Value"":[{2}]}}]}}"
                                         , portId
                                         , tstamp
                                         , pollutantSb.ToString());

                    jsonStr = jsonSb.ToString();
                }

            }
            catch (Exception e)
            {
                CommonFunction.WriteErrorLog("When GetLatestHourWQ PortId=[" + portId + "] Error: " + e.Message);
            }

            CommonFunction.WriteInfoLog("Return GetLatestHourWQ PortId=[" + portId + "] Result=[" + jsonStr + "]");

            ReturnJson(jsonStr);
        }

        /// <summary>
        /// {"HoursFactorData":[{"DateTime":"2015-11-25 12:00","value":"7.66"},{"DateTime":"2015-11-25 16:00","value":"7.64"},{"DateTime":"2015-11-25 20:00","value":"7.81"},{"DateTime":"2015-11-26 00:00","value":"7.66"},{"DateTime":"2015-11-26 04:00","value":"7.65"},{"DateTime":"2015-11-26 08:00","value":"7.69"}]}
        /// </summary>
        /// <param name="portId"></param>
        /// <param name="factorName"></param>
        [WebMethod(Description = @"【水_实时水质】获取最新24小时污染物浓度趋势<br />")]
        public void hoursHoursFactorDataWater(string portId, string factorName)
        {
            string result = string.Empty;
            try
            {
                string queryPollutantSql = string.Format(@" 
                                                  select [PollutantCode],
                                                         [PollutantName]
                                                    from [Standard].[TB_PollutantCode] st
                                                   where [PollutantTypeUid] = '80CA99DE-3B78-422F-9BAA-D47F23324231' 
                                                     and (st.[PollutantName] = '{0}' 
                                                      or st.[PollutantCode] = '{0}')", factorName);

                DataTable pollDt = DBHelper.GetDataView(queryPollutantSql, null, DBHelper.GetConnectionString(AMS_BaseData_Conn));

                if (pollDt.Rows.Count > 0)
                {// 查询到存在此因子
                    DataRow pollDr = pollDt.Rows[0];
                    string pollutantCode = pollDr["PollutantCode"].ToString();

                    string querySql = string.Format(@"
                                                    select distinct wti.[Tstamp],
                                                           dbo.F_Round(wti.PollutantValue, syp.DecimalDigit) as [PollutantValue] 
                                                      from Water.TB_InfectantBy60 wti
                                                     inner join dbo.SY_PollutantCode syp
                                                        on wti.PollutantCode = syp.PollutantCode 
                                                     where wti.PointId = {0} 
                                                       and wti.[PollutantCode] = '{1}' 
                                                       and datediff(hh, [Tstamp], getdate()) <= 24 and datediff(hh, [Tstamp], getdate()) >= 0 
                                                       and wti.id = (select MAX(id) 
                                                                       from Water.TB_InfectantBy60 temp
                                                                      where temp.PointId = wti.PointId
                                                                        and temp.PollutantCode = wti.PollutantCode 
                                                                        and temp.Tstamp = wti.Tstamp)
                                                     order by wti.[Tstamp] asc"
                                                    , portId
                                                    , pollutantCode);

                    DataTable valueDt = DBHelper.GetDataView(querySql, null, DBHelper.GetConnectionString(AMS_WaterAutoMonitor_Conn));

                    StringBuilder jsonSb = new StringBuilder();
                    jsonSb.Append("{\"HoursFactorData\":[");

                    if (valueDt.Rows.Count > 0)
                    {// 查询到因子数据
                        foreach (DataRow dataDr in valueDt.Rows)
                        {
                            jsonSb.AppendFormat(@"{{""DateTime"":""{0}"",""value"":""{1}""}},"
                                                 , string.Format("{0:yyyy-MM-dd HH:mm}", dataDr["Tstamp"])
                                                 , dataDr["PollutantValue"].ToString());
                        }

                        // 删除最后一个,
                        jsonSb.Remove(jsonSb.Length - 1, 1);
                    }
                    else
                    {
                        CommonFunction.WriteInfoLog("When Get24HoursFactorDataWater PortId=[" + portId
                                                  + "] FactorName=[" + factorName + "] Not Query History Hour Data Record...");
                    }

                    result = jsonSb.Append("]}").ToString();
                }
                else
                {
                    CommonFunction.WriteInfoLog("When Get24HoursFactorDataWater PortId=[" + portId
                                              + "] FactorName=[" + factorName + "] Not Found This Pollutant..");
                }

            }
            catch (Exception e)
            {
                CommonFunction.WriteErrorLog("When Get24HoursFactorDataWater PortId=[" + portId + "] Error: " + e.Message);
            }

            CommonFunction.WriteInfoLog("Return Get24HoursFactorDataWater PortId=[" + portId
                                      + "] FactorName=[" + factorName + "] Result=[" + result + "]");

            ReturnJson(result);
        }

        /// <summary>
        /// {"DayWQ":[{"dateTime":"2015-11-22","WQ":"4","Class":"轻度污染","MainPollute":"氨氮,溶解氧"},{"dateTime":"2015-11-23","WQ":"5","Class":"中度污染","MainPollute":"氨氮"},{"dateTime":"2015-11-24","WQ":"6","Class":"重度污染","MainPollute":"氨氮"},{"dateTime":"2015-11-25","WQ":"6","Class":"重度污染","MainPollute":"氨氮"}]}
        /// </summary>
        /// <param name="portId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        [WebMethod(Description = @"【水_历史】获取时间区间内日水质情况<br />参数说明：<br />portId为站点ID；<br />startTime为开始时间，endTime为结束时间（时间格式：yyyy-MM-dd）")]
        public void GetDayWQByDatetime(string portId, string startTime, string endTime)
        {
            string result = string.Empty;

            try
            {
                DataTable dataDt = GetWaterDayWQ(portId, startTime, endTime);

                StringBuilder jsonSb = new StringBuilder();

                foreach (DataRowView row in dataDt.DefaultView)
                {
                    if (jsonSb.Length != 0)
                        jsonSb.Append(",");

                    StringBuilder colSb = new StringBuilder();

                    foreach (DataColumn col in dataDt.Columns)
                    {
                        if (colSb.Length != 0)
                        {
                            colSb.Append(",");
                        }

                        if (col.ColumnName.ToLower() == "datetime")
                            colSb.AppendFormat(@"""{0}"":""{1}""", col.ColumnName, string.Format("{0:yyyy-MM-dd}", row[col.ColumnName]));
                        else if (col.ColumnName.ToLower() == "mainpollute")
                            colSb.AppendFormat(@"""{0}"":""{1}""", col.ColumnName, row[col.ColumnName].ToString().TrimStart(',').TrimEnd(','));
                        else
                            colSb.AppendFormat(@"""{0}"":""{1}""", col.ColumnName, System.Convert.IsDBNull(row[col.ColumnName]) ? "--" : row[col.ColumnName]);
                    }

                    jsonSb.AppendFormat("{{{0}}}", colSb);
                }

                result = string.Format(@"{{""DayWQ"":[{0}]}}", jsonSb);
            }
            catch (Exception e)
            {
                CommonFunction.WriteErrorLog("When GetDayWQByDatetime PortId=[" + portId + "] Error: " + e.Message);
            }

            CommonFunction.WriteInfoLog("Return GetDayWQByDatetime PortId=[" + portId
                                      + "] StartTime=[" + startTime + "] EndTime=[" + endTime + "] Result=[" + result + "]");

            ReturnJson(result);
        }

        /// <summary>
        /// {"DayWQ":[{"dateTime":"2015-11-29","WQ":"4","Class":"轻度污染","MainPollute":"氨氮,溶解氧"},{"dateTime":"2015-11-30","WQ":"4","Class":"轻度污染","MainPollute":"氨氮,溶解氧"},{"dateTime":"2015-12-01","WQ":"4","Class":"轻度污染","MainPollute":"氨氮,溶解氧"},{"dateTime":"2015-12-02","WQ":"4","Class":"轻度污染","MainPollute":"氨氮,溶解氧"},{"dateTime":"2015-12-03","WQ":"4","Class":"轻度污染","MainPollute":"氨氮,溶解氧"}]}
        /// </summary>
        /// <param name="portId"></param>
        /// <param name="topN"></param>
        [WebMethod(Description = @"【水_历史】获取最近N天内日水质情况<br />参数说明")]
        public void GetTopNDayWQ(string portId, string topN)
        {
            int top = int.Parse(topN);
            string startTime = DateTime.Now.AddDays(-top).ToString("yyyy-MM-dd");
            string endTime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");

            CommonFunction.WriteInfoLog("Begin GetTopNDayWQ PortId=[" + portId
                                      + "] topN=[" + topN + "]");
            GetDayWQByDatetime(portId, startTime, endTime);
            CommonFunction.WriteInfoLog("End GetTopNDayWQ PortId=[" + portId
                                      + "] topN=[" + topN + "]");
        }

        /// <summary>
        /// {"HistoryData":[{"DateTime":"11-26 08:12","Value":[{"factor": "水温","value": "15.0","isExceeded":"False"},{"factor": "pH值","value": "7.69","isExceeded":"False"},{"factor": "溶解氧","value": "3.65","isExceeded":"True"},{"factor": "高锰酸盐指数","value": "4.80","isExceeded":"False"},{"factor": "氨氮","value": "1.08","isExceeded":"True"},{"factor": "总磷","value": "1.637","isExceeded":"True"},{"factor": "总氮","value": "30.00","isExceeded":"True"},{"factor": "电导率","value": "700.00","isExceeded":"False"},{"factor": "浑浊度","value": "15.2","isExceeded":"False"},{"factor": "总有机碳","value": "8.22","isExceeded":"False"},{"factor": "蓝绿藻","value": "933.50","isExceeded":"False"},{"factor": "叶绿素","value": "20.13","isExceeded":"False"}]},{"DateTime":"11-26 04:12","Value":[{"factor": "水温","value": "14.0","isExceeded":"False"},{"factor": "pH值","value": "7.65","isExceeded":"False"},{"factor": "溶解氧","value": "3.77","isExceeded":"True"},{"factor": "高锰酸盐指数","value": "5.20","isExceeded":"False"},{"factor": "氨氮","value": "0.86","isExceeded":"False"},{"factor": "总磷","value": "1.637","isExceeded":"True"},{"factor": "总氮","value": "30.00","isExceeded":"True"},{"factor": "电导率","value": "707.00","isExceeded":"False"},{"factor": "浑浊度","value": "15.3","isExceeded":"False"},{"factor": "总有机碳","value": "8.87","isExceeded":"False"},{"factor": "蓝绿藻","value": "946.57","isExceeded":"False"},{"factor": "叶绿素","value": "20.49","isExceeded":"False"}]},{"DateTime":"11-26 00:12","Value":[{"factor": "水温","value": "15.0","isExceeded":"False"},{"factor": "pH值","value": "7.66","isExceeded":"False"},{"factor": "溶解氧","value": "3.92","isExceeded":"True"},{"factor": "高锰酸盐指数","value": "4.00","isExceeded":"False"},{"factor": "氨氮","value": "1.02","isExceeded":"True"},{"factor": "总磷","value": "0.913","isExceeded":"True"},{"factor": "总氮","value": "30.00","isExceeded":"True"},{"factor": "电导率","value": "705.00","isExceeded":"False"},{"factor": "浑浊度","value": "15.1","isExceeded":"False"},{"factor": "总有机碳","value": "6.86","isExceeded":"False"},{"factor": "蓝绿藻","value": "942.84","isExceeded":"False"},{"factor": "叶绿素","value": "20.97","isExceeded":"False"}]}]}
        /// </summary>
        /// <param name="portId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="userUid"></param>
        [WebMethod(Description = @"【水_历史】获取地表水监测数据数据")]
        public void GetWaterHistoryDatas(string portId, string startTime, string endTime, string userUid)
        {
            string result = string.Empty;
            try
            {
                string queryTstampSql = string.Format(@"
                                            select Tstamp  
                                              from Water.TB_InfectantBy60
                                             where PointId = {0}
                                               and Tstamp >= '{1}'
                                               and Tstamp <= '{2}'
                                             group by Tstamp 
                                             order by Tstamp desc
                                           ", portId
                                            , startTime
                                            , endTime);

                DataTable tstampDt = DBHelper.GetDataView(queryTstampSql, null, DBHelper.GetConnectionString(AMS_WaterAutoMonitor_Conn));

                if (tstampDt == null || tstampDt.Rows.Count == 0)
                {
                    CommonFunction.WriteInfoLog("When GetWaterHistoryDatas Not Query Data.....");
                }
                else
                {
                    string queryDataSql = null;
                    DataTable dataDt = null;
                    StringBuilder jsonSb = new StringBuilder();
                    StringBuilder tstampFactorsSb = null;
                    string userStr = "";
                    if (userUid != "")
                        userStr = string.Format(@" and syps.UserUid = '{0}'", userUid);
                    foreach (DataRow tstampDr in tstampDt.Rows)
                    {
                        queryDataSql = string.Format(@"
                                                     select distinct wti.PollutantCode,
													syp.PollutantName,
													dbo.F_Round(wti.PollutantValue, syp.DecimalDigit) as PollutantValue,
													(case when CHARINDEX('HSp', Mark) > 0 then 'True' when CHARINDEX('LSp', Mark) > 0 then 'True' else 'False' end) as IsExceeded, 
													syp.OrderByNum 
													from Water.TB_InfectantBy60 wti 
													left join dbo.SY_PollutantCode syp
													on wti.PollutantCode = syp.PollutantCode
													and wti.PointId = {2}
													and wti.Tstamp = '{3}'
													and wti.id = (select MAX(id) 
													from Water.TB_InfectantBy60 temp
													where temp.PointId = wti.PointId
													and temp.PollutantCode = wti.PollutantCode 
													and temp.Tstamp = wti.Tstamp)
													inner join dbo.SY_PersonalizedSettings syps
													on syps.ParameterUid = syp.PollutantUid
													{0}
													and syps.ApplicationUid = '{1}' 
													and syps.EnableCustomOrNot = 1 
													order by syp.OrderByNum desc
                                           ", userStr
                                            , APP_UID_WATER
                                            , portId
                                            , tstampDr["Tstamp"]);

                        dataDt = DBHelper.GetDataView(queryDataSql, null, DBHelper.GetConnectionString(AMS_WaterAutoMonitor_Conn));

                        tstampFactorsSb = new StringBuilder();

                        foreach (DataRow dataDr in dataDt.Rows)
                        {
                            tstampFactorsSb.AppendFormat(@"{{""factor"": ""{0}"",""value"": ""{1}"",""isExceeded"":""{2}""}},"
                                                         , dataDr["PollutantName"].ToString()
                                                         , (dataDr["PollutantValue"] == DBNull.Value) ? "--" : dataDr["PollutantValue"].ToString()
                                                         , dataDr["IsExceeded"].ToString());
                        }

                        jsonSb.AppendFormat(@"{{""DateTime"":""{0:MM-dd HH:mm}"",""Value"":[{1}]}},"
                                           , tstampDr["Tstamp"]
                                           , tstampFactorsSb.ToString().TrimEnd(','));
                    }

                    //删除最后一个，
                    jsonSb.Remove((jsonSb.Length - 1), 1);

                    result = string.Format(@"{{""HistoryData"":[{0}]}}", jsonSb.ToString());
                }
            }
            catch (Exception ex)
            {
                CommonFunction.WriteErrorLog("When GetWaterHistoryDatas PortId=[" + portId + "] Error: " + ex.Message);
            }

            CommonFunction.WriteInfoLog("Return GetWaterHistoryDatas PortId=[" + portId
                                      + "] StartTime=[" + startTime + "] EndTime=[" + endTime
                                      + "] UserUid=[" + userUid + "] Result=[" + result + "]");

            ReturnJson(result);
        }

        /// <summary>
        /// {"Login":"False"}
        /// </summary>
        /// <param name="loginId"></param>
        /// <param name="pwd"></param>
        [WebMethod(Description = @"【修改密码】修改密码")]
        public void GetModifyPassword(string loginId, string useruid, string pwd, string pwdnew)
        {
            string jsonStr = string.Format(@"{{""Update"":""0"",""UserId"":""""}}");

            try
            {
                if (!ContainSqlChar(loginId) && !ContainSqlChar(pwd))
                {
                    string md5Pwd = FormsAuthentication.HashPasswordForStoringInConfigFile(pwd, "SHA1");//使用MD5加密
                    string md5Pwdnew = FormsAuthentication.HashPasswordForStoringInConfigFile(pwdnew, "SHA1");

                    string sqlUpdate = string.Format(@"update dbo.TB_Frame_User set Password = '{0}' where LoginID='{1}' and Password='{2}' and RowGuid = '{3}'", md5Pwdnew, loginId, md5Pwd, useruid);
                    object dtsqlLogin = DBHelper.GetEffect(sqlUpdate, null, DBHelper.GetConnectionString(AMS_Framework));
                    if (dtsqlLogin != null)
                    {
                        jsonStr = string.Format(@"{{""Update"":""{0}"",""UserUid"":""""}}", dtsqlLogin.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                CommonFunction.WriteErrorLog("When UpdatePwd LoginId=[" + loginId + "] Error: " + e.Message);
            }

            CommonFunction.WriteInfoLog("Return UpdatePwd loginId=[" + loginId + "] Result=[" + jsonStr + "]");

            ReturnJson(jsonStr);
        }

        /// <summary>
        /// {"Login":"False"}
        /// </summary>
        /// <param name="loginId"></param>
        /// <param name="pwd"></param>
        [WebMethod(Description = @"【登录】验证专业版登录用户名和密码")]
        public void GetLoginInfo(string loginId, string pwd)
        {
            string jsonStr = string.Format(@"{{""Login"":""False"",""UserId"":""""}}");

            try
            {
                if (!ContainSqlChar(loginId) && !ContainSqlChar(pwd))
                {
                    string md5Pwd = FormsAuthentication.HashPasswordForStoringInConfigFile(pwd, "SHA1");//使用MD5加密

                    string sqlLogin = string.Format(@"
                                                  select RowGuid, 
                                                         LoginID, 
                                                         DisplayName 
                                                    from dbo.TB_Frame_User 
                                                   where LoginID='{0}' 
                                                     and Password='{1}'"
                                                       , loginId
                                                       , md5Pwd);
                    DataTable dtsqlLogin = DBHelper.GetDataView(sqlLogin, null, DBHelper.GetConnectionString(AMS_Framework));

                    if (dtsqlLogin.Rows.Count > 0)
                    {
                        jsonStr = string.Format(@"{{""Login"":""True"",""UserUid"":""{0}"",""Alias"":""{1}""}}", dtsqlLogin.Rows[0]["RowGuid"].ToString(), loginId);
                    }
                }
            }
            catch (Exception e)
            {
                CommonFunction.WriteErrorLog("When GetLoginInfo LoginId=[" + loginId + "] Error: " + e.Message);
            }

            CommonFunction.WriteInfoLog("Return GetLoginInfo loginId=[" + loginId + "] Result=[" + jsonStr + "]");

            ReturnJson(jsonStr);
        }
        //[SoapDocumentMethod("http://tempuri.org/CheckAuthorization", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        //public string CheckAuthorization(string requestXml)
        //{
        //    object[] array = base.Invoke("CheckAuthorization", new object[]
        //    {
        //        requestXml
        //    });
        //    return (string)array[0];
        //}



        /// <summary>
        /// {"Login":"False"}
        /// </summary>
        /// <param name="loginId"></param>
        /// <param name="pwd"></param>
        [WebMethod(Description = @"【登录】验证专业版登录用户名和密码<br />用户名：admin 密码：sz2015!<br />用户名：zhaoli 密码：11111<br />用户名：zuoning 密码：11111")]
        public void GetLoginInfoNew(string loginId, string pwd)
        {
            string jsonStr = string.Format(@"{{""IsSuccess"":""false"",""Data"":""""}}");
            string Number = string.Empty;
            if (loginId == "admin")
                Number = "1111111111111";
            if (loginId == "zhaoli")
                Number = "1111000111100";
            try
            {
                if (!ContainSqlChar(loginId) && !ContainSqlChar(pwd))
                {
                    string md5Pwd = FormsAuthentication.HashPasswordForStoringInConfigFile(pwd, "SHA1");//使用MD5加密
                    StringBuilder jsonSb = new StringBuilder();
                    string sqlLogin = string.Format(@"
                                                  select RowGuid, 
                                                         LoginID, 
                                                         DisplayName 
                                                    from dbo.TB_Frame_User 
                                                   where LoginID='{0}' 
                                                     and Password='{1}'"
                                                       , loginId
                                                       , md5Pwd);
                    DataTable dtsqlLogin = DBHelper.GetDataView(sqlLogin, null, DBHelper.GetConnectionString(AMS_Framework));

                    if (dtsqlLogin.Rows.Count > 0)
                    {
                        if (Number != "")
                        {
                            jsonSb.AppendFormat(@"{{""ErrorInfo"":"""",""MenuInfo"":""{0}"",""UserGuid"":""{1}""}}"
                                             , Number, dtsqlLogin.Rows[0]["RowGuid"].ToString());
                            jsonStr = string.Format(@"{{""IsSuccess"":""true"",""Data"":{0}}}", jsonSb);
                        }
                        else
                        {
                            jsonSb.AppendFormat(@"{{""ErrorInfo"":""没有权限"",""MenuInfo"":"""",""UserGuid"":""{0}""}}", dtsqlLogin.Rows[0]["RowGuid"].ToString());
                            jsonStr = string.Format(@"{{""IsSuccess"":""false"",""Data"":{0}}}", jsonSb);
                        }
                    }
                    else
                    {
                        jsonSb.AppendFormat(@"{{""ErrorInfo"":""密码错误"",""MenuInfo"":"""",""UserGuid"":""""}}");
                        jsonStr = string.Format(@"{{""IsSuccess"":""false"",""Data"":{0}}}", jsonSb);
                    }
                }
            }
            catch (Exception e)
            {
                CommonFunction.WriteErrorLog("When GetLoginInfo LoginId=[" + loginId + "] Error: " + e.Message);
            }

            CommonFunction.WriteInfoLog("Return GetLoginInfo loginId=[" + loginId + "] Result=[" + jsonStr + "]");

            ReturnJson(jsonStr);
        }
        /// <summary>
        /// {"Login":"False"}
        /// </summary>
        /// <param name="loginId"></param>
        /// <param name="pwd"></param>
        [WebMethod(Description = @"【登录】验证管理维度手机号码；Number：18888888888")]
        public void GetLeaderInfo(string Number)
        {
            string jsonStr = string.Format(@"{{""Login"":""False"",""UserId"":""""}}");

            try
            {
                if (!ContainSqlChar(Number))
                {
                    string sqlLogin = string.Format(@"
                                                  select RowGuid, 
                                                         MobilePhone, 
                                                         Name 
                                                    from dbo.TB_LeaderNumber
                                                   where MobilePhone='{0}'"
                                                       , Number);
                    DataTable dtsqlLogin = DBHelper.GetDataView(sqlLogin, null, DBHelper.GetConnectionString(AMS_BaseData_Conn));

                    if (dtsqlLogin.Rows.Count > 0)
                    {
                        jsonStr = string.Format(@"{{""Login"":""True"",""UserUid"":""{0}""}}", dtsqlLogin.Rows[0]["RowGuid"].ToString());
                    }
                }
            }
            catch (Exception e)
            {
                CommonFunction.WriteErrorLog("When GetLoginInfo LoginId=[" + Number + "] Error: " + e.Message);
            }

            CommonFunction.WriteInfoLog("Return GetLoginInfo loginId=[" + Number + "] Result=[" + jsonStr + "]");

            ReturnJson(jsonStr);
        }
        #endregion

        #region 公用方法
        /// <summary>
        /// 直接返回Json格式
        /// </summary>
        /// <param name="json">字符串</param>
        public void ReturnJson(string json)
        {

            var response = HttpContext.Current.Response;
            response.ContentType = "application/json";
            try
            {
                response.Charset = "UTF-8";
                response.BinaryWrite(System.Text.Encoding.UTF8.GetBytes(json));
                //response.BinaryWrite(System.Text.Encoding.GetEncoding("gb2312").GetBytes(json));
            }
            catch (Exception ex)
            {
                response.Write(System.Text.Encoding.UTF8.GetBytes(ex.Message));
            }

        }

        public double GetAirAlertUpper(string factorName)
        {
            string overData = ConfigurationManager.AppSettings["AirOverData2"];
            double result = 0;
            string[] overList = overData.Split(',');
            foreach (var over in overList)
            {
                if (over.Contains(factorName))
                {
                    result = Convert.ToDouble(over.Split(':')[1]);
                }

            }
            return result;

        }

        public double GetWaterAlertUpper(string factorName)
        {
            string overData = ConfigurationManager.AppSettings["WaterOverData"];
            double result = 0;
            string[] overList = overData.Split(',');
            foreach (var over in overList)
            {
                if (over.Contains(factorName))
                {
                    result = Convert.ToDouble(over.Split(':')[1]);
                }

            }
            return result;

        }
        /// <summary>
        /// 获取最新AQI信息
        /// </summary>
        /// <param name="portId">0为全市</param>
        /// <returns></returns>
        private string GetLatestHourAQIJson(string portId)
        {
            //string tableName = "AirRelease.TB_HourAQI";

            //南通超级站审核无数据，数据源变更为原始
            string tableName = "AMS_AirAutoMonitor.Air.TB_OriHourAQI";
            if (portId == "0")
            {
                //tableName = "AirRelease.TB_RegionHourAQI";
                tableName = "AMS_AirAutoMonitor.Air.TB_OriRegionHourAQI";
            }

            string sql = string.Format(@"select {0} PortName,
                                                dateadd(hh,0,[datetime]) DateTime,
                                                ISNULL(SO2_IAQI,'--') SO2_IAQI,
                                                ISNULL(NO2_IAQI,'--') NO2_IAQI,
                                                ISNULL(PM10_IAQI,'--') PM10_IAQI,
                                                ISNULL(CO_IAQI,'--') CO_IAQI,
                                                ISNULL(O3_IAQI,'--') O3_IAQI,
                                                ISNULL(Recent8HoursO3_IAQI,'--') Recent8HoursO3_IAQI,
                                                ISNULL([PM25_IAQI],'--') [PM2.5_IAQI],
                                                ISNULL(AQIValue,'--') AQI,
                                                ISNULL(PrimaryPollutant,'--') PrimaryPollutant,
                                                ISNULL(Class,'--') Class, 
                                                ISNULL(Grade,'--') Grade,
                                                ISNULL(RGBValue,'--') RGBValue,
                                                ISNULL(HealthEffect,'--') HealthEffect,
                                                ISNULL(TakeStep,'--') TakeStep 
                                           from {1} AS v 
                                                {2} 
                                          where v.[dateTime] = (  
                                                                select MAX([dateTime])
                                                                  from {1} 
                                                                 where {3} [dateTime] > DATEADD(DAY, -7, GETDATE())
                                                                   and [dateTime] < GETDATE()
                                                                ) 
                                             {4}"
                                              , "0".Equals(portId) ? "0 AS PortId, '南通市' AS" : "v.PointId as PortId, p.MonitoringPointName as"
                                              , tableName
                                              , "0".Equals(portId) ? "" : "inner join [dbo].[SY_MonitoringPoint] as P on v.PointId = p.PointId "
                                              , "0".Equals(portId)
                                                   ? string.Format(" MonitoringRegionUid = '{0}' and ", monitorRegionUid)
                                                   : string.Format(" PointId = {0} and", portId)
                                              , string.IsNullOrEmpty(portId) || "0".Equals(portId)
                                                   ? string.Format("and v.MonitoringRegionUid = '{0}'", monitorRegionUid)
                                                   : string.Format("and v.PointId = {0}", portId)
                                              );

            //DataTable dt = DBHelper.GetDataView(sql, null, DBHelper.GetConnectionString(AMS_MonitorBusiness_Conn));
            DataTable dt = DBHelper.GetDataView(sql, null, DBHelper.GetConnectionString("AMS_AirAutoMonitor_Conn"));

            StringBuilder jsonSb = new StringBuilder();
            foreach (System.Data.DataRowView row in dt.DefaultView)
            {
                if (jsonSb.Length != 0)
                {
                    jsonSb.Append(",");
                }

                StringBuilder colSb = new StringBuilder();
                foreach (System.Data.DataColumn col in dt.Columns)
                {
                    if (col.ColumnName == "Recent8HoursO3_IAQI")
                    {
                        continue;
                    }

                    if (colSb.Length != 0)
                    {
                        colSb.Append(",");
                    }

                    if (col.ColumnName.ToLower() == "datetime")
                    {
                        colSb.AppendFormat(@"""{0}"":""{1}""", col.ColumnName, string.Format("{0:yyyy-MM-dd HH:00}", row[col.ColumnName]));
                    }
                    else
                    {
                        colSb.AppendFormat(@"""{0}"":""{1}""", col.ColumnName, System.Convert.IsDBNull(row[col.ColumnName]) ? "--" : row[col.ColumnName]);
                    }
                }

                if (portId != "0")
                    portId = row["PortId"].ToString();
                else
                    portId = "0";

                //获取污染因子的浓度值
                string primaryPollutant = row["PrimaryPollutant"].ToString();

                if (primaryPollutant == "--")
                {
                    colSb.AppendFormat(@",""{0}"":""{1}""", "PrimaryPollutantValue", "--");
                }
                else
                {
                    primaryPollutant = primaryPollutant.Split(',')[0];

                    // 需要将PM2.5转换为列名PM25
                    primaryPollutant = "PM2.5".Equals(primaryPollutant.ToUpper()) ? "PM25" : primaryPollutant;

                    string sqlValue = string.Format(@" 
                                                     select TOP 1 isnull([{0}],'--') [{0}] 
                                                       from {1} 
                                                      where 1=1 {2} 
                                                      order by [dateTime] desc"
                                                      , primaryPollutant
                                                      , tableName
                                                      , portId == "0"
                                                               ? string.Format(" and MonitoringRegionUid = '{0}'", monitorRegionUid)
                                                               : string.Format(" and PointId = {0}", portId));

                    DataTable dtValue = DBHelper.GetDataView(sqlValue, null, DBHelper.GetConnectionString(AMS_MonitorBusiness_Conn));

                    if (primaryPollutant.ToUpper() == "CO")
                    {
                        colSb.AppendFormat(@",""{0}"":""{1}""", "PrimaryPollutantValue", dtValue.Rows[0][0].ToString() == "--" ? "--" : Math.Round(Convert.ToDecimal(dtValue.Rows[0][0]), 1).ToString());
                    }
                    else
                    {
                        colSb.AppendFormat(@",""{0}"":""{1}""", "PrimaryPollutantValue", dtValue.Rows[0][0].ToString() == "--" ? "--" : Math.Round(Convert.ToDecimal(dtValue.Rows[0][0]) * 1000, 0).ToString());
                    }
                }

                jsonSb.AppendFormat("{{{0}}}", colSb);
            }

            return jsonSb.ToString();
        }

        public DataTable GetWaterDayWQ(string portId, string startTime, string endTime)
        {
            string sql = string.Format(@"
                                select dateTime,
                                       WQ,
                                       sye.Grade as Class, 
                                       MainPollute  
                                  from (
                                      select distinct wtd.DateTime, 
                                                   dbo.F_GetPointAllPollutantDayWQ_LEVEL(wtd.PointId, wtd.DateTime) as WQ ,
                                                    wtd.Description as MainPollute 
                                              from WaterReport.TB_DayReport wtd
                                             inner join dbo.SY_PollutantCode syp
                                                on wtd.PollutantCode = syp.PollutantCode and syp.IsCalEQIOrNot=1
                                             where wtd.PointId = {0}
                                                    and DateTime >= '{1}'
                                                    and DateTime <= '{2}'
		                                ) as WaterLI 
                                 inner join dbo.SY_EQI sye
                                    on WaterLI.wq = sye.IEQI
                                   and sye.ApplicationUid = '{3}'
                                 order by DateTime asc"
                                 , portId
                                 , startTime
                                 , endTime
                                 , APP_UID_WATER);

            return DBHelper.GetDataView(sql, null, DBHelper.GetConnectionString(AMS_MonitorBusiness_Conn));
        }

        #region  【报警信息完成】
        /// <summary>
        /// 【报警信息查看】查看报警信息
        /// </summary>
        /// <param name="startTime">开始时间,格式为2015-03-04</param>
        /// <param name="endTime">结束时间,格式为2015-03-09</param>
        /// <returns>返回成功：{"AlarmInfo": {"IsSuccess":"true","Data":[{"PortName": "吴江区环保局1","AlarmTime": "03-18 08:00", "AlarmContent": "报警内容"},
        ///                                                              {"PortName": "吴江区环保局2","AlarmTime": "03-18 08:00","AlarmContent": "报警内容"},
        ///                                                              {"PortName": "吴江区环保局3","AlarmTime": "03-18 08:00","AlarmContent": "报警内容"}]}}</returns>
        /// <returns>返回失败：{"AlarmInfo": {"IsSuccess":"false","Data":"程序中出错，请联系开发人员"}}</returns>
        /// 说明：PortName     站点名称
        ///       AlarmTime    报警时间,格式只需要03-18 08:00
        ///       AlarmContent 报警内容
        [WebMethod(Description = @"【报警信息查看】查看报警信息<br />
                                 <p>sysType   为系统类型，air为环境空气、water为地表水</p>
                                 <p>startTime 为开始时间（时间格式：yyyy-MM-dd HH:mm）</p>
                                 <p>endTime   为结束时间（时间格式：yyyy-MM-dd HH:mm）</p>")]
        public void GetAlarmInfo(string sysType, string startTime, string endTime)
        {
            //CommonFunction.WriteInfoLog("Begin Execute GetAlarmInfo Param Info: SysType=[" + sysType
            //                          + "] StartTime=[" + startTime + "] EndTime=[" + endTime + "]");
            CommonFunction.WriteInfoLog("EndTime=[" + endTime + "]");

            string appUid = SYS_TYPE_AIR.Equals(sysType.ToLower()) ? APP_UID_AIR : APP_UID_WATER;

            string result = string.Empty;
            StringBuilder allPortInfostr = new StringBuilder();
            allPortInfostr.Append("{\"AlarmInfo\":{\"IsSuccess\":\"true\",\"Data\":[");
            String portInfoStr = string.Empty;
            //            string sqlAlarmInfo = string.Format(@"select a.[MonitoringPointName] as PortName,
            //                                                         b.[CreatDateTime] as AlarmTime,
            //                                                         b.[Content] as AlarmContent, 
            //                                                         b.[ItemValue] as pollutantValue,
            //                                                         (case when b.[dealFlag] = 1 then '1' else '0' end) as AlarmProcess,
            //                                                         b.[UpdateDateTime] as UpdateTime  
            //                                                    from [MPInfo].[TB_MonitoringPoint] a, 
            //                                                         [AlarmNotify].[TB_CreatAlarm] b
            //                                                   where a.[MonitoringPointUid] = b.[MonitoringPointUid] 
            //                                                     and a.[ApplicationUid] ='{0}' 
            //                                                     and b.[CreatDateTime] >= '{1}' 
            //                                                     and b.[CreatDateTime] <= '{2}'
            //                                                   order by AlarmTime desc, UpdateTime desc"
            //                                                   , appUid
            //                                                   , startTime
            //                                                   , endTime);
            string where = @"  ApplicationUid = 'airaaira-aira-aira-aira-airaairaaira'  AND 
                            MonitoringPointUid in ('f2369c1f-95e1-4f5d-9045-9c185ce8727a','d979dcb6-7707-4def-9a2d-be5cb5270376','261a801c-516b-4ab4-bad3-a289b80582ee','adcbd9ba-7acc-4eec-824b-932625f5f6b3','4bba2c6b-c7a3-4c6d-9a86-7376d7cea40b','d820a83a-7a97-44da-82de-0ae0d96b1ba3','dd027da6-43a2-4650-9117-eca7221bab75','f4145a15-5bd5-4f9c-9783-99fca706e4d7','cc8b584a-273f-4938-80e1-1141c222ae98','f8fb840d-cff4-4d9d-8a6a-6472f9af38d5','8f71c355-3cf0-4a62-8ef6-62fd380d5189','c0b0fa1a-006e-4780-8b77-00d794033d2f','dc57b6d2-57aa-4aba-97cf-11ba69c73b6e','87002985-b3dd-4044-aba9-26bbc1b0017e','41dfbfa6-2664-42ae-bf20-53cee5f4c237','27f9f30c-95a0-435a-8b63-085e28a6b9d0','3ea588ce-11b6-4799-b837-4020da3e3883','2832a4c9-58b5-4ad7-b049-fcf7183ca4dc','af716151-3483-4e0a-82e1-cdb9dbf1e41b','f88e3bce-bf94-42d3-8dad-dd101d17b07b','dc0799c7-e8bb-41c6-8cb4-e610961e3519')";
            string sqlAlarmInfo = string.Format(@"  select b.MonitoringPointName AS PortName,b.CreatDateTime AS AlarmTime,
	b.Content AS AlarmContent ,b.[ItemValue] as pollutantValue,(case when b.[dealFlag] = 1 then '已处理' else '未处理' end) as AlarmProcess from
                                (select ApplicationUid,MonitoringPointUid,MAX(RecordDateTime) as RecordDateTime,MAX(CreatDateTime) as CreatDateTime,AlarmEventUid ,ItemName FROM [dbo].[V_AlarmHandle]
                                where 1=1 and {0} and CreatDateTime >='{1}' and CreatDateTime <='{2}'
                                 group by ApplicationUid,MonitoringPointUid,AlarmEventUid,ItemName) as a inner join  [dbo].[V_AlarmHandle]  as b 
                                on a.AlarmEventUid=b.AlarmEventUid and a.ApplicationUid=b.ApplicationUid and a.MonitoringPointUid=b.MonitoringPointUid  and a.RecordDateTime=b.RecordDateTime
                                and a.CreatDateTime=b.CreatDateTime and a.ItemName=b.ItemName    order by b.CreatDateTime desc", where, startTime, endTime);
            DataTable dtAlarmInfo = DBHelper.GetDataView(sqlAlarmInfo, null, DBHelper.GetConnectionString(AMS_BaseData_Conn));
            if (dtAlarmInfo.Rows.Count == 0)
            {
                result = "{\"AlarmInfo\": {\"IsSuccess\":\"false\",\"Data\":\"未查询到符合条件的数据！\"}}";
            }
            else
            {
                try
                {
                    for (int i = 0; i < dtAlarmInfo.Rows.Count; i++)
                    {
                        DataRow dr = dtAlarmInfo.Rows[i];
                        portInfoStr = "{\"PortName\":\"" + dr["PortName"].ToString() + "\",\"AlarmTime\":\"" + Convert.ToDateTime(dr["AlarmTime"]).ToString("yyyy-MM-dd HH:mm:ss").Substring(0, 16) + "\",\"AlarmContent\":\"" + dr["AlarmContent"].ToString() + "\",\"AlarmProcess\":\"" + dr["AlarmProcess"].ToString() + "\"}";
                        allPortInfostr.Append(portInfoStr);
                        allPortInfostr.Append(",");
                    }
                    result = allPortInfostr.Replace(",", "", allPortInfostr.Length - 1, 1).Append("]}}").ToString(); //这里是去掉最后一个多余的逗号（,）
                }
                catch (Exception e)
                {
                    //CommonFunction.WriteErrorLog("Execute GetAlarmInfo SysType=[" + sysType + "] Error: " + e.Message);
                    CommonFunction.WriteErrorLog("Error: " + e.Message);

                    result = "{\"AlarmInfo\": {\"IsSuccess\":\"false\",\"Data\":\"程序中出错，请联系开发人员\"}}";
                }
            }

            //CommonFunction.WriteInfoLog("End Execute GetAlarmInfo SysType=[" + sysType + "] Return Info=[" + result + "]");

            ReturnJson(result);

        }
        #endregion

        /// <summary>
        /// 优良天数等级名称转换
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetClassNumName(string name)
        {
            string result = "";
            switch (name)
            {
                case "优": result = "Class1"; break;
                case "良": result = "Class2"; break;
                case "轻度污染": result = "Class3"; break;
                case "中度污染": result = "Class4"; break;
                case "重度污染": result = "Class5"; break;
                case "严重污染": result = "Class6"; break;
            }
            return result;
        }

        /// <summary>
        ///SQL注入过滤
        /// </summary>
        /// <param name="InText">要过滤的字符串</param>
        /// <returns>如果参数存在不安全字符，则返回true</returns>
        public static bool ContainSqlChar(string InText)
        {
            string word = "and|exec|insert|select|delete|update|chr|mid|master|or|truncate|char|declare|join|'";
            if (InText == null)
                return false;
            foreach (string str_t in word.Split('|'))
            {
                if ((InText.ToLower().IndexOf(str_t + " ") > -1) || (InText.ToLower().IndexOf(" " + str_t) > -1) || (InText.ToLower().IndexOf(str_t) > -1))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 根据因子类型获取因子集合字典
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> getPollutantDic(string pollutantUid)
        {
            Dictionary<string, string> pollutantDic = new Dictionary<string, string>();

            string querySql = string.Format(@"
                                         select distinct [PollutantCode],
                                                [PollutantName] 
                                           from [Standard].[TB_PollutantCode] 
                                          where [PollutantTypeUid] = '{0}'", pollutantUid);
            DataTable pollutantDt = DBHelper.GetDataView(querySql, null, DBHelper.GetConnectionString(AMS_BaseData_Conn));
            foreach (DataRow dr in pollutantDt.Rows)
            {
                pollutantDic.Add(dr["PollutantCode"].ToString(), dr["PollutantName"].ToString());
            }

            return pollutantDic;
        }

        #endregion
    }
}
