using log4net;
using SmartEP.Core.Generic;
using SmartEP.Utilities.AdoData;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SmartEP.Data.SqlServer.BaseData
{
    /// <summary>
    /// 名称：PortInfoDAL.aspx.cs
    /// 创建人：
    /// 创建日期：
    /// 维护人员：
    /// 最新维护人员：吕云
    /// 最新维护日期：2017-6-19
    /// 功能摘要：南通市实时空气质量
    /// 虚拟分页类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public class PortInfoDAL
    {
        /// <summary>
        /// 数据库处理类
        /// </summary>
        DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connection = "AMS_BaseDataConnection";
        ILog log = LogManager.GetLogger("FileLogging");//获取一个日志记录器

        /// <summary>
        /// 获取站点信息提供给接口
        /// </summary>
        /// <returns></returns>
        public DataTable GetPortInfoForData()
        {
            string result = string.Empty;
            IList<string> list = new List<string>();

            try
            {
                string sql = string.Format(@"
                                            SELECT  online.PointId as portId, smp.monitoringPointName as  PortName, 
                                            case isNull(IsOnline, 0) when 1 then 'True' when 8 then 'True' when 0 then 'False' else 'False' end as IsOnline,
                                            Recent24HourRecords RecordCount,NewDataTime LastestTime 
                                            FROM [dbo].[DataOnline] as online   left join  [dbo].[SY_MonitoringPoint] smp on online.PointId=smp.PointId
                                            where [DataTypeUid]='1b6367f1-5287-4c14-b120-7a35bd176db1'
                                            ");
                DataTable dt = CreatDataView(sql, "AMS_MonitoringBusinessConnection").Table;
                return dt;
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        /// 根据用户权限获取站点
        /// </summary>
        /// <param name="userguid"></param>
        /// <param name="datatype"></param>
        /// <returns></returns>
        public DataTable GetPointByUserGuid(string userguid, string datatype)
        {
            DataTable dt = new DataTable();
            string strwhere = " where 1=1 ";
            if (!string.IsNullOrWhiteSpace(userguid))
            {
                strwhere += " and UserGuid='" + userguid + "'";
            }
            if (datatype.ToUpper() == "WATER")
            {
                strwhere += " and a.ApplicationUid='watrwatr-watr-watr-watr-watrwatrwatr'";
            }
            else if (datatype.ToUpper() == "AIR")
            {
                strwhere += " and a.ApplicationUid='airaaira-aira-aira-aira-airaairaaira'";
            }
            string sql = string.Format(@"SELECT b.MonitoringPointUid as PortId,a.SiteTypeUid,siteType,b.MonitoringPointName
                                            FROM [dbo].[V_Point_UserConfig] as a left join MPInfo.TB_MonitoringPoint as b 
                                            on a.PortId=b.PointId
                                            {0}
                                            group by b.MonitoringPointUid,a.SiteTypeUid,siteType,b.MonitoringPointName,b.OrderByNum 
                                            order by b.OrderByNum desc ", strwhere);
            dt = CreatDataView(sql, "AMS_BaseDataConnection").Table;
            return dt;
        }

        /// <summary>
        /// 获取站点信息
        /// </summary>
        /// <param name="SYSType"></param>
        /// <param name="pointGuid"></param>
        /// <returns></returns>
        public string GetPortInfo(string SYSType, string pointGuid)//已更新的方法
        {
            string result = string.Empty;
            IList<string> list = new List<string>();

            try
            {
                string application = string.Empty;
                string strWhere = " where 1=1 ";
                if (SYSType == "water")
                {
                    if (pointGuid != "")
                        strWhere += " and a.ApplicationUid='watrwatr-watr-watr-watr-watrwatrwatr' and a.MonitoringPointUid='" + pointGuid + "'";
                    else
                        strWhere += " and a.ApplicationUid='watrwatr-watr-watr-watr-watrwatrwatr' ";
                }
                else if (SYSType == "air")
                {
                    if (pointGuid != "")
                        strWhere += " and a.ApplicationUid='airaaira-aira-aira-aira-airaairaaira'  and a.MonitoringPointUid='" + pointGuid + "'";
                    else
                        strWhere += " and a.ApplicationUid='airaaira-aira-aira-aira-airaairaaira' ";
                }
                else
                {
                    if (pointGuid != "")
                    {
                        strWhere += "  and a.MonitoringPointUid='" + pointGuid + "'";
                    }
                }
                string sqlPort = @" select a.MonitoringPointUid,a.PointId,monitoringPointName,x,y,itemValue,itemText,c.IsOnline,a.CustomX,a.CustomY,a.ApplicationUid,
                                case a.ApplicationUid when 'airaaira-aira-aira-aira-airaairaaira' then '22' 
	                                when 'watrwatr-watr-watr-watr-watrwatrwatr' then '21' end as ST 
                                from MPInfo.TB_MonitoringPoint as a 
                                inner join dbo.SY_View_CodeMainItem as b on a.SiteTypeUid=b.itemGuid and a.EnableOrNot=1 
                                 left join dbo.SY_DataOnline as c on a.MonitoringPointUid=c.MonitoringPointUid and  c.[DataTypeUid]='1b6367f1-5287-4c14-b120-7a35bd176db1'";
                DataView dvPort = CreatDataView(sqlPort + strWhere, "AMS_BaseDataConnection");

                if (dvPort.Table.Rows.Count > 0)
                {
                    //String[] arr = new string[dvPort.Table.Rows.Count];


                    for (int i = 0; i < dvPort.Table.Rows.Count; i++)
                    {
                        string ApplicationUid = dvPort.Table.Rows[i]["ApplicationUid"].ToString();
                        string portId = dvPort.Table.Rows[i]["PointId"].ToString();
                        string portName = dvPort.Table.Rows[i]["monitoringPointName"].ToString();
                        string MonitoringPointUid = dvPort[i]["MonitoringPointUid"].ToString();
                        string portType = "1";
                        string attentionType = dvPort.Table.Rows[i]["itemValue"].ToString();
                        string siteTypeText = dvPort.Table.Rows[i]["ItemText"].ToString();
                        string ST = dvPort.Table.Rows[i]["ST"].ToString();
                        string Level = "--";
                        string EvaluateFactor = "--";
                        if (ApplicationUid == "airaaira-aira-aira-aira-airaairaaira")
                        {
                            string sqlLevel = string.Format(@"select top 1 Class as Grade from AirRelease.TB_HourAQI where PointId={0} order by DateTime desc"
                                   , Convert.ToInt32(portId));
                            DataView dvLevel = CreatDataView(sqlLevel, "AMS_MonitoringBusinessConnection");
                            if (dvLevel.Count > 0)
                            {
                                if (dvLevel[0]["Grade"] != DBNull.Value)
                                {
                                    Level = dvLevel[0]["Grade"].ToString();
                                }
                            }
                        }
                        else if (ApplicationUid == "watrwatr-watr-watr-watr-watrwatrwatr")
                        {
                            string sql = string.Format(@"SELECT PointId,EQIUid,IEQI,CalEQITypeUid,EvaluateFactorList,b.ItemText as EQIType
                                 FROM AMS_BaseData.dbo.V_Point_Water a left join dbo.SY_Frame_Code_Item  b on a.CalEQITypeUid=b.RowGuid
                                 where PointId = {0}", portId);
                            DataView dvPoint = CreatDataView(sql, "AMS_BaseDataConnection");
                            string[] FactorCode = null;
                            string FactorCodes = string.Empty;
                            string EQI = string.Empty;
                            string CalEQIType = string.Empty;
                            string CalEQITypeUid = string.Empty;
                            if (dvPoint.Table.Rows.Count > 0)
                            {
                                FactorCode = dvPoint.Table.Rows[0]["EvaluateFactorList"].ToString() != null ? dvPoint.Table.Rows[0]["EvaluateFactorList"].ToString().Split(';') : null;
                                FactorCodes = dvPoint.Table.Rows[0]["EvaluateFactorList"].ToString() != null ? dvPoint.Table.Rows[0]["EvaluateFactorList"].ToString().Trim(';') : null;
                                EQI = dvPoint.Table.Rows[0]["IEQI"].ToString();//获取功能水质EQI
                                CalEQIType = dvPoint.Table.Rows[0]["EQIType"].ToString();//获取功能水质水质类别
                                CalEQITypeUid = dvPoint.Table.Rows[0]["CalEQITypeUid"].ToString();
                            }
                            //取得查询行转列字段拼接
                            string sqlStr = " SELECT ";
                            string tableNameReport = " From dbo.SY_Water_InfectantBy60";
                            string factorSql = string.Empty;
                            if (FactorCode.Length > 0 && FactorCode[0].ToString() != "")
                            {
                                foreach (string factor in FactorCode)
                                {
                                    factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                                }

                                string fieldName = " top 1 PointId,Tstamp" + factorSql;
                                string groupBy = " Group By PointId,Tstamp";
                                string whereStr = string.Format(" where PointId = {0}", portId);
                                sqlStr += fieldName + tableNameReport + whereStr + groupBy + " order by PointId ,Tstamp desc";
                                DataTable dvLevel = CreatDataView(sqlStr, "AMS_BaseDataConnection").Table;
                                Dictionary<string, Int32> WQIValues = new Dictionary<string, int>();
                                if (dvLevel.Rows.Count > 0)
                                {
                                    string WQL = string.Empty;
                                    foreach (string factor in FactorCode)
                                    {
                                        string pollutantValue = dvLevel.Rows[0][factor].ToString();//污染物浓度
                                        if (!string.IsNullOrEmpty(pollutantValue))
                                        {
                                            decimal value = Convert.ToDecimal(pollutantValue);
                                            #region 获取等级
                                            switch (CalEQIType)
                                            {
                                                case "湖泊":
                                                    string huSql = "SELECT dbo.F_GetWQL('" + factor + "','7c67a857-d602-4f90-a26d-edd3e9f4d36c'," + value + ",'" + CalEQITypeUid + "','Level')";
                                                    DataTable dvhu = CreatDataView(huSql, "AMS_MonitoringBusinessConnection").Table;
                                                    WQL = dvhu.Rows[0][0].ToString();
                                                    break;
                                                case "河流":
                                                    string heSql = "SELECT dbo.F_GetWQL('" + factor + "','7c67a857-d602-4f90-a26d-edd3e9f4d36c'," + value + ",'" + CalEQITypeUid + "','Level')";
                                                    DataTable dvhe = CreatDataView(heSql, "AMS_MonitoringBusinessConnection").Table;
                                                    WQL = dvhe.Rows[0][0].ToString();
                                                    break;
                                                default:
                                                    string OtherSql = "SELECT dbo.F_GetWQL('" + factor + "','7c67a857-d602-4f90-a26d-edd3e9f4d36c'," + value + ",'" + CalEQITypeUid + "','Level')";
                                                    DataTable dvOther = CreatDataView(OtherSql, "AMS_MonitoringBusinessConnection").Table;
                                                    WQL = dvOther.Rows[0][0].ToString();
                                                    break;
                                            }
                                            #endregion
                                            if (!string.IsNullOrEmpty(WQL.ToString()))
                                                WQIValues.Add(factor, Convert.ToInt32(WQL));
                                        }
                                    }
                                }
                                Level = GetWQL_Max("Roman", FactorCodes, WQIValues, out EvaluateFactor);
                            }
                        }
                        string IsOnline = dvPort.Table.Rows[i]["IsOnline"].ToString();
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
                        list.Add("{\"MonitoringPointUid\":\"" + MonitoringPointUid + "\",\"PortId\":\"" + portId + "\",\"PortName\":\"" + portName + "\",\"X\":\"" + x + "\",\"Y\":\"" + y + "\",\"IsOnline\":\"" + IsOnline + "\",\"CustomX\":\"" + CustomX + "\",\"CustomY\":\"" + CustomY + "\",\"Level\":\"" + Level + "\",\"EvaluateFactor\":\"" + EvaluateFactor + "\"}");
                    }
                }

                result = "{\"PortInfo\":" + ToListStringNew(list) + "}";//ToArrayStringNew(list)
            }
            catch (Exception ex)
            {
                result = "{\"ErrorInfo\":\"" + ex.Message + "\"}";
            }
            return result;
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
        /// 普通集合转换Json   
        /// </summary>   
        /// <param name="list">集合对象</param>   
        /// <returns>Json字符串</returns>   

        public string GetWQL_Max(string ReturnType, string EvaluateFactorCodes, Dictionary<string, Int32> WQIValues, out string EvaluateFactor)
        {
            EvaluateFactor = "";
            // @WQL_PH：pH值 w01001
            string WQL_PH = WQIValues.ContainsKey("w01001") ? WQIValues["w01001"].ToString() : "NULL";
            EvaluateFactor += WQIValues.ContainsKey("w01001") ? "pH值;".ToString() : "";
            // @WQL_NH3N：氨氮 w21003
            string WQL_NH3N = WQIValues.ContainsKey("w21003") ? WQIValues["w21003"].ToString() : "NULL";
            EvaluateFactor += WQIValues.ContainsKey("w21003") ? "氨氮;".ToString() : "";
            // @WQL_CODMN：高锰酸盐 w01019
            string WQL_CODMN = WQIValues.ContainsKey("w01019") ? WQIValues["w01019"].ToString() : "NULL";
            EvaluateFactor += WQIValues.ContainsKey("w01019") ? "高锰酸盐;".ToString() : "";
            // @WQL_DOX：溶解氧 w01009
            string WQL_DOX = WQIValues.ContainsKey("w01009") ? WQIValues["w01009"].ToString() : "NULL";
            EvaluateFactor += WQIValues.ContainsKey("w01009") ? "溶解氧;".ToString() : "";
            // @WQL_TP：总磷 w21011
            string WQL_TP = WQIValues.ContainsKey("w21011") ? WQIValues["w21011"].ToString() : "NULL";
            EvaluateFactor += WQIValues.ContainsKey("w21011") ? "总磷;".ToString() : "";
            // @WQL_TN：总氮 w21001
            string WQL_TN = WQIValues.ContainsKey("w21001") ? WQIValues["w21001"].ToString() : "NULL";
            EvaluateFactor += WQIValues.ContainsKey("w21001") ? "总氮;".ToString() : "";
            // @WQL_CODCR：化学需氧量（COD）w01018
            string WQL_CODCR = WQIValues.ContainsKey("w01018") ? WQIValues["w01018"].ToString() : "NULL";
            EvaluateFactor += WQIValues.ContainsKey("w01018") ? "化学需氧量;".ToString() : "";
            // @WQL_BOD5：五日生化需氧量w01017
            string WQL_BOD5 = WQIValues.ContainsKey("w01017") ? WQIValues["w01017"].ToString() : "NULL";
            EvaluateFactor += WQIValues.ContainsKey("w01017") ? "五日生化需氧量;".ToString() : "";
            // @WQL_CU：铜 w20122
            string WQL_CU = WQIValues.ContainsKey("w20122") ? WQIValues["w20122"].ToString() : "NULL";
            EvaluateFactor += WQIValues.ContainsKey("w20122") ? "铜;".ToString() : "";
            // @WQL_ZN：锌 w20123
            string WQL_ZN = WQIValues.ContainsKey("w20123") ? WQIValues["w20123"].ToString() : "NULL";
            EvaluateFactor += WQIValues.ContainsKey("w20123") ? "锌;".ToString() : "";
            // @WQL_F：氟化物 w21017
            string WQL_F = WQIValues.ContainsKey("w21017") ? WQIValues["w21017"].ToString() : "NULL";
            EvaluateFactor += WQIValues.ContainsKey("w21017") ? "氟化物;".ToString() : "";
            // @WQL_SE：硒 w20128
            string WQL_SE = WQIValues.ContainsKey("w20128") ? WQIValues["w20128"].ToString() : "NULL";
            EvaluateFactor += WQIValues.ContainsKey("w20128") ? "硒;".ToString() : "";
            // @WQL_ARS：砷 w20119
            string WQL_ARS = WQIValues.ContainsKey("w20119") ? WQIValues["w20119"].ToString() : "NULL";
            EvaluateFactor += WQIValues.ContainsKey("w20119") ? "砷;".ToString() : "";
            // @WQL_HG：汞 w20111
            string WQL_HG = WQIValues.ContainsKey("w20111") ? WQIValues["w20111"].ToString() : "NULL";
            EvaluateFactor += WQIValues.ContainsKey("w20111") ? "汞;".ToString() : "";
            // @WQL_CD：镉 w20115
            string WQL_CD = WQIValues.ContainsKey("w20115") ? WQIValues["w20115"].ToString() : "NULL";
            EvaluateFactor += WQIValues.ContainsKey("w20115") ? "镉;".ToString() : "";
            // @WQL_CR6：六价铬 w20117
            string WQL_CR6 = WQIValues.ContainsKey("w20117") ? WQIValues["w20117"].ToString() : "NULL";
            EvaluateFactor += WQIValues.ContainsKey("w20117") ? "六价铬;".ToString() : "";
            // @WQL_PB：铅 w20120
            string WQL_PB = WQIValues.ContainsKey("w20120") ? WQIValues["w20120"].ToString() : "NULL";
            EvaluateFactor += WQIValues.ContainsKey("w20120") ? "铅;".ToString() : "";
            // @WQL_CN：氰化物 w21016
            string WQL_CN = WQIValues.ContainsKey("w21016") ? WQIValues["w21016"].ToString() : "NULL";
            EvaluateFactor += WQIValues.ContainsKey("w21016") ? "氰化物;".ToString() : "";
            // @WQL_VLPH：挥发酚 w23002
            string WQL_VLPH = WQIValues.ContainsKey("w23002") ? WQIValues["w23002"].ToString() : "NULL";
            EvaluateFactor += WQIValues.ContainsKey("w23002") ? "挥发酚;".ToString() : "";
            // @WQL_S2：硫化物 w21019
            string WQL_S2 = WQIValues.ContainsKey("w21019") ? WQIValues["w21019"].ToString() : "NULL";
            EvaluateFactor += WQIValues.ContainsKey("w21019") ? "硫化物;".ToString() : "";
            // @WQL_OILS：石油类 w22001
            string WQL_OILS = WQIValues.ContainsKey("w22001") ? WQIValues["w22001"].ToString() : "NULL";
            EvaluateFactor += WQIValues.ContainsKey("w22001") ? "石油类;".ToString() : "";
            // @WQL_ASAA：阴离子表面活性剂 w19002
            string WQL_ASAA = WQIValues.ContainsKey("w19002") ? WQIValues["w19002"].ToString() : "NULL";
            EvaluateFactor += WQIValues.ContainsKey("w19002") ? "阴离子表面活性剂;".ToString() : "";
            // @WQL_FCG：粪大肠菌群（个/L）w02003
            string WQL_FCG = WQIValues.ContainsKey("w02003") ? WQIValues["w02003"].ToString() : "NULL";
            EvaluateFactor += WQIValues.ContainsKey("w02003") ? "粪大肠菌群;".ToString() : "";
            string sql = string.Format(@"SELECT dbo.F_GetWQL_Max('{0}','{1}',{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24})"
                , ReturnType, EvaluateFactorCodes, WQL_PH, WQL_NH3N, WQL_CODMN, WQL_DOX, WQL_TP, WQL_TN, WQL_CODCR, WQL_BOD5, WQL_CU
                , WQL_ZN, WQL_F, WQL_SE, WQL_ARS, WQL_HG, WQL_CD, WQL_CR6, WQL_PB, WQL_CN, WQL_VLPH, WQL_S2, WQL_OILS, WQL_ASAA, WQL_FCG);
            DataTable dt = CreatDataView(sql, "AMS_MonitoringBusinessConnection").Table;
            string ret = "--";
            if (dt.Rows.Count > 0)
            {
                ret = dt.Rows[0][0].ToString();
            }
            return ret.ToString();
        }
        #region 获取标液值信息
        /// <summary>
        /// 通过标液编号获取标液值
        /// </summary>
        /// <param name="StanSolutionSN">标液编号（固定资产编号）</param>
        /// <returns>返回成功：
        /// {"IsSuccess":"True","Msg":"",
        /// "Data":[{"FixCode":"","ReagentName":"","Concentration":"","UnitName":"","}]}
        /// 返回失败：{"IsSuccess":"False","Msg":"错误信息","Data":[]}
        /// </returns>
        /// FixCode  标液编号	
        /// Concentration	浓度	
        /// UnitName	浓度单位	
        public DataView GetStanSolutionInfoData(string StanSolutionSN)
        {
            try
            {
                string strsql = string.Format(@"select reagentD.FixCode,reagent.ReagentName,reagent.Concentration,Concent.UnitName 
								from TB_OMMP_ReagentInItemDetail as reagentD
								LEFT OUTER JOIN dbo.TB_OMMP_ReagentInBillItem  AS initem on reagentD.BillItemGuid=initem.RowGuid
								LEFT OUTER JOIN dbo.TB_OMMP_Reagent AS reagent ON initem.ReagentGuid=reagent.RowGuid 
								LEFT OUTER JOIN dbo.TB_OMMP_ConcentrationUnit AS Concent ON reagent.ConcentrationUnitGuid=Concent.RowGuid 
								where reagentD.FixCode='{0}'", StanSolutionSN);
                return CreatDataView(strsql, "Frame_Connection");
            }
            catch (Exception ex) { throw ex; }
        }
        # endregion
        /// <summary>
        /// 获取数据集
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <param name="strConnectString">数据库链接字符串</param>
        /// <returns></returns>
        public DataView CreatDataView(String strSql, String strConnectString)
        {
            try
            {
                SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings[strConnectString].ConnectionString);
                SqlDataAdapter myCommand = new SqlDataAdapter(strSql, myConn);
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

        }
        /// <summary>
        /// 根据站点Uid获取站点仪器因子类型配置信息
        /// </summary>
        /// <param name="PUid"></param>
        /// <returns></returns>
        public DataTable GetPoint_Category_Instrument(string PUid)
        {
            try
            {
                string sql = string.Format(@"  select *   FROM [AMS_BaseData].[dbo].[V_Point_Category_Instrument]
where [MonitoringPointUid]='{0}'", PUid);
                return g_DatabaseHelper.ExecuteDataTable(sql, connection);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                return null;
            }
        }
    }
}
