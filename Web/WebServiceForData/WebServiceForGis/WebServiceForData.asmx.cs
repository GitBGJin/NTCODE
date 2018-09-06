﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Net;
using System.IO;
using System.Xml;
using System.Web.Script.Serialization;
using System.Web.Services.Description;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Linq;
using System.Web.Services.Protocols;
using System.Globalization;

namespace WebServiceForData
{
    /// <summary>
    /// WebServiceForGis 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScE:\workplace\WebServiceForGis\WebServiceForGis\Web.configriptService]
    public class WebServiceForData : System.Web.Services.WebService
    {
     


      [WebMethod(Description = @"定义：获取所有启用的仪器菜单")]
      public void GetAllUsingInstruments()
      {
        try
        {
          string msg = "";
          
          string jsonsb = string.Empty;
          string sql = string.Empty;
         
         
          StringBuilder sbs = new StringBuilder();
         
          string tableName="[InstrInfo].[TB_Instruments]";
       
          sql = string.Format(@"select RowGuid,InstrumentName from {0}  where  ShowInMenu=1 order by orderByNum desc", tableName);
          DataTable dt = CreatDataTable(sql, "AMS_BaseDataConnection");

          dt.TableName = "InstrumentData";
          jsonsb = DataTableToJson(dt);
          ReturnJson(jsonsb);
        }
        catch (Exception e)
        {
          WriteTextLog("数据异常", e.ToString(), DateTime.Now);
          ReturnJson(ReturnErrorStr("获取启用菜单异常！"));
        }
      }
      [WebMethod(Description = @"定义：获取粒径普仪的24小时分段数据 startTime：查询数据的日期 日期格式:yyyy-MM-dd  kind：分段的层,0:所有数据，0-32um;1:0-0.35um;2:0.35-2um;3:2-32um")]
      public void  GetParticlesizeOfSpectrometerData(string startTime, string kind)
      {
        try
        {
          kind=kind.Trim();
          string LZSPYGuid = ConfigurationManager.AppSettings["LZSPYGuid"];
          // string imageGuidStr = ConfigurationManager.AppSettings["CityImageGuid"];
          // string imageJGLDGuidStr = ConfigurationManager.AppSettings["JGLDImageGuid"];

          // string _filepath = ConfigurationManager.AppSettings["UrbanPhoto"].ToString();
          //string _JGLDFilePath=ConfigurationManager.AppSettings["NTJGLDXG"].ToString();
          List<string> listLZSPY = new List<string>();
          string sqlLZSPY = string.Format(@"select  t1.PollutantUid,t1.PollutantCode,t2.PollutantName,t2.DecimalDigit,t3.itemValue as measureUnit from InstrInfo.TB_InstrumentChannels as t1 left join [Standard].[TB_PollutantCode] as t2 on t1.PollutantCode=t2.PollutantCode   left join [dbo].[TB_Frame_CodeItem] as t3 on t2.MeasureUnitUid=t3.RowGuid where t1.InstrumentUid='{0}' order by t2.OrderByNum desc,t1.PollutantCode", LZSPYGuid);
          DataTable dtLZSPY = CreatDataTable(sqlLZSPY, "AMS_BaseDataConnection");
          if (dtLZSPY != null && dtLZSPY.Rows.Count > 0)
          {
            for (int i = 0; i < dtLZSPY.Rows.Count; i++)
            {
              string pollutantCode = dtLZSPY.Rows[i]["PollutantCode"].ToString();
              if (!listLZSPY.Contains(pollutantCode))
              {
                listLZSPY.Add(pollutantCode);
              }
            }
          }
          if(string.IsNullOrEmpty(kind))
          {
            ReturnJson( ReturnErrorStr("分段层不能为空！"));
            return;
          }
          string instrumentUid = ConfigurationManager.AppSettings["LJPYGuid"];
          startTime = startTime.Trim();
          string low;
          string upper;
          string[] numbers = null;
          switch (kind)
          {
            case "0":
              numbers = ConfigurationManager.AppSettings["ljAll"].Split(',');
              low = numbers[0];
              upper = numbers[1];
              break;
            case "1":
               numbers = ConfigurationManager.AppSettings["ljFirst"].Split(',');
               low = numbers[0];
              upper = numbers[1];
              break;
            case "2":
              numbers = ConfigurationManager.AppSettings["ljSecond"].Split(',');
                low = numbers[0];
              upper = numbers[1];
              break;
            case "3":
               numbers = ConfigurationManager.AppSettings["ljThird"].Split(',');
                low = numbers[0];
              upper = numbers[1];
              break;
            default:
             ReturnJson(ReturnErrorStr());
             return;
          }
          if (string.IsNullOrEmpty(startTime))
          {
            startTime = DateTime.Now.ToString("yyyy-MM-dd");
          }
          string msg = "";
          string jsonsb = string.Empty;
          string sql = string.Empty;

          sql = string.Format(@"select  t1.PollutantUid,t1.PollutantCode,t2.PollutantName,t2.DecimalDigit,t3.ItemText as measureUnit 
from InstrInfo.TB_InstrumentChannels as t1 
left join [Standard].[TB_PollutantCode] as t2 on t1.PollutantCode=t2.PollutantCode  
left join dbo.SY_Frame_Code_Item as t3 on t2.MeasureUnitUid=t3.RowGuid 
where t1.InstrumentUid='{0}' and t2.PollutantCode>='{1}' and t2.PollutantCode<='{2}' 
order by t2.OrderByNum desc,t1.PollutantCode", instrumentUid, low, upper);
          DataTable dtFactor = CreatDataTable(sql, "AMS_BaseDataConnection");
         
          List<string> listFactor = new List<string>();
          if (dtFactor != null && dtFactor.Rows.Count > 0)
          {
            for (int i = 0; i < dtFactor.Rows.Count; i++)
            {
              string factorCode = dtFactor.Rows[i]["PollutantCode"].ToString();
              if (!listFactor.Contains(factorCode))
              {
                listFactor.Add(factorCode);
              }
            }
          }
          StringBuilder sb = new StringBuilder();
          DataTable dtOriginalData = GetHourData(listFactor, startTime, dtFactor, instrumentUid);
          if (dtOriginalData != null && dtOriginalData.Rows.Count > 0)
          {
            for (int i = 0; i < dtOriginalData.Rows.Count; i++)
            {
              if (sb.Length > 0)
              {
                sb.Append(",");
              }
              StringBuilder sb1 = new StringBuilder();
              for (int j = 2; j < dtOriginalData.Columns.Count; j++)
              {
                string factor = dtOriginalData.Columns[j].ColumnName;
                if (!string.IsNullOrEmpty(factor) && !factor.Contains("_Status") && !factor.Contains("_DataFlag") && !factor.Contains("_AuditFlag"))
                {
                  string factorStatus = dtOriginalData.Rows[i][factor + "_Status"] != DBNull.Value ? dtOriginalData.Rows[i][factor + "_Status"].ToString() : string.Empty;
                  string factorDataFlag = dtOriginalData.Rows[i][factor + "_DataFlag"] != DBNull.Value ? dtOriginalData.Rows[i][factor + "_DataFlag"].ToString() : string.Empty;
                  string factorAuditFlag = dtOriginalData.Rows[i][factor + "_AuditFlag"] != DBNull.Value ? dtOriginalData.Rows[i][factor + "_AuditFlag"].ToString() : string.Empty;
                  string markContent = string.Empty;
                  markContent=TrimChart(factorStatus, factorDataFlag, factorAuditFlag);
                  
                  //if (((factorStatus != "N" && factorStatus != "d" && factorStatus != "MF" && !string.IsNullOrEmpty(factorStatus)) || (factorDataFlag != "N" && factorDataFlag != "d" && factorDataFlag != "MF" && !string.IsNullOrEmpty(factorDataFlag)) || (factorAuditFlag != "N" && factorAuditFlag != "d" && factorAuditFlag != "MF" && !string.IsNullOrEmpty(factorAuditFlag))))
                  //{
                  //  markContent = string.Empty;
                  //  if ((factorStatus != "N" && factorStatus != "d" && factorStatus != "MF" && !string.IsNullOrEmpty(factorStatus)) || (factorDataFlag != "N" && factorDataFlag != "d" && factorDataFlag != "MF" && !string.IsNullOrEmpty(factorDataFlag)) || (factorAuditFlag != "N" && factorAuditFlag != "d" && factorAuditFlag != "MF" && !string.IsNullOrEmpty(factorAuditFlag)))
                  //  {
                  //    markContent += factorStatus + "," + factorDataFlag + "," + factorAuditFlag;
                  //    markContent = TrimChart(markContent);
                  //  }
                  //}
                  string valueStr = string.Empty;
                  decimal value;
                  DataRow[] dr = dtFactor.Select("PollutantCode='" + factor + "'");
                  string factorName = string.Empty;
                  int digit;
                  string measureUnit = string.Empty;
                  if (factor == "a05024")
                  {
                    factorName = "O₃";
                    digit = 3 - 3;
                    if (Convert.IsDBNull(dtOriginalData.Rows[i][dtOriginalData.Columns[j].ColumnName]))
                    {
                      valueStr = "--";
                    }
                    else
                    {
                      valueStr = dtOriginalData.Rows[i][dtOriginalData.Columns[j].ColumnName].ToString();
                      if (decimal.TryParse(valueStr, out value))
                      {
                        value = GetPollutantValue(value * 1000, digit);
                        valueStr = value.ToString();
                      }
                    }
                  }
                  else
                  {
                    factorName = dr[0]["PollutantName"].ToString();
                    measureUnit = dr[0]["measureUnit"].ToString();
                    string digitStr = dr[0]["DecimalDigit"].ToString();
                    if ("ug/m3".Equals(measureUnit) && !listLZSPY.Contains(factor))
                    {
                      digit = string.IsNullOrEmpty(digitStr) ? 0 : Convert.ToInt32(digitStr) - 3;
                      if (Convert.IsDBNull(dtOriginalData.Rows[i][dtOriginalData.Columns[j].ColumnName]))
                      {
                        valueStr = "--";
                      }
                      else
                      {
                        valueStr = dtOriginalData.Rows[i][dtOriginalData.Columns[j].ColumnName].ToString();
                        if (decimal.TryParse(valueStr, out value))
                        {
                          value = GetPollutantValue(value * 1000, digit);
                          valueStr = value.ToString();
                        }
                      }
                    }
                    else
                    {
                      digit = string.IsNullOrEmpty(digitStr) ? 3 : Convert.ToInt32(digitStr);
                      if (Convert.IsDBNull(dtOriginalData.Rows[i][dtOriginalData.Columns[j].ColumnName]))
                      {
                        valueStr = "--";
                      }
                      else
                      {
                        valueStr = dtOriginalData.Rows[i][dtOriginalData.Columns[j].ColumnName].ToString();
                        if (decimal.TryParse(valueStr, out value))
                        {
                          value = GetPollutantValue(value, digit);
                          valueStr = value.ToString();
                        }
                      }
                    }
                  }
                 //标记位暂时隐藏
                  markContent = string.Empty;
                  sb1.AppendFormat(@"{{""factor"": ""{0}"",""factorName"": ""{1}"",""value"": ""{2}"",""flag"": ""{3}"",""measureUnit"":""{4}""}},", dtOriginalData.Columns[j].ColumnName, factorName, valueStr, markContent, measureUnit.Replace("<sup>3</sup>", "³"));
                }
              }
                sb.AppendFormat(@"{{""Tstamp"":""{0}"",""Value"":[{1}]}}", dtOriginalData.Rows[i]["Tstamp"].ToString(), sb1.ToString().TrimEnd(','));
              }
            

          }
          msg = (string.Format(@"{{""HistoryData"":[{0}]}}", sb));
          ReturnJson(msg);
        }
        catch (Exception e)
        {
          WriteTextLog("数据异常", e.ToString(), DateTime.Now);
          ReturnJson(ReturnErrorStr());
        }
      }

      private string TrimChart(string factorStatus, string factorDataFlag, string factorAuditFlag)
      {
        string markUse = string.Empty;
        
        
              if (!string.IsNullOrEmpty(factorStatus) && factorStatus != "N" && factorStatus != "d" && factorStatus != "MF")
              {
                markUse += factorStatus;
                markUse += ",";
              }
              if (!string.IsNullOrEmpty(factorDataFlag) && factorStatus != "N" && factorDataFlag != "d" && factorDataFlag != "MF")
              {
                markUse += factorDataFlag;
                markUse += ",";
              }
              if (!string.IsNullOrEmpty(factorAuditFlag) && factorAuditFlag != "N" && factorAuditFlag != "d" && factorAuditFlag != "MF")
              {
                markUse += factorAuditFlag;
                markUse += ",";
              }
        markUse = markUse.TrimEnd(',');
        return markUse;
      }
      /// <summary>
      /// 去除符号
      /// </summary>
      /// <param name="markContent"></param>
      /// <returns></returns>
      private string TrimChart(string markContent)
      {
        string markUse = string.Empty;
        if (!string.IsNullOrEmpty(markContent))
        {
          string[] marks = markContent.Split(',');
          if (marks != null && marks.Count() > 0)
          {
            for (int i = 0; i < marks.Count(); i++)
            {
              string s = marks[0];
              if (!string.IsNullOrEmpty(s) && s!="N" && s!="d" && s!="MF")
              {
                markUse += s;
                markUse += ",";
              }
            }
          }
        }
        markUse = markUse.TrimEnd(',');
        return markUse;
      }
      [WebMethod(Description = @"定义：获取粒径普仪的分段")]
      public void GetParticlesizeOfSpectrometerKind()
      {
        try
        {
          DataTable dt = new DataTable();
          dt.TableName = "ParticlesizeData";
          dt.Columns.Add("Kind", typeof(Int32));
          dt.Columns.Add("DepartFloor", typeof(string));
          
          string ljAll = ConfigurationManager.AppSettings["ljAll"];
          FillDataRow(dt, ljAll,0);
          string ljFirst = ConfigurationManager.AppSettings["ljFirst"];
          FillDataRow(dt, ljFirst,1);
          string ljSecond = ConfigurationManager.AppSettings["ljSecond"];
          FillDataRow(dt, ljSecond,2);
          string ljThird = ConfigurationManager.AppSettings["ljThird"];
          FillDataRow(dt, ljThird,3);
          ReturnJson(DataTableToJson(dt));
         
           
          
        }
        catch (Exception e)
        {
          WriteTextLog("数据异常", e.ToString(), DateTime.Now);
          ReturnJson(ReturnErrorStr());
        }
      }
      [WebMethod(Description = @"定义：获取Voc因子的类别1:一级类；2：二级类；3：分因子")]
      public void GetVocClass()
      {
        try
        {
          DataTable dt = new DataTable();
          dt.TableName = "VOCType";
          dt.Columns.Add("VOCType", typeof(string));
          dt.Columns.Add("VOCValue", typeof(string));
          string VOCTypes = ConfigurationManager.AppSettings["VOCType"];
          string[] vocArr = VOCTypes.Split(';');
          if (vocArr != null && vocArr.Count() > 0)
          {
            for (int i = 1; i <= vocArr.Count(); i++)
            {
              DataRow dr = dt.NewRow();
              dr["VOCType"] = vocArr[i - 1];
              dr["VOCValue"] = i;
              dt.Rows.Add(dr);
            }
          }
          ReturnJson(DataTableToJson(dt));
        }
        catch (Exception e)
        {
          WriteTextLog("数据异常", e.ToString(), DateTime.Now);
          ReturnJson(ReturnErrorStr(e.ToString()));
        }
      }
      [WebMethod(Description = @"定义：获取24小时VOC的数据 startTime：查询数据的日期 日期格式：yyyy-MM-dd  type：voc的分类,1:一级类(ppb);2:二级类(ppb);3:分因子(ppb);4:一级类(ppb);5:二级类(ppb);6:分因子(ppb)")]
      public void GetVocData(string startTime, string type)
      {
        try
        {
          DataTable dtAll = new DataTable();
          DataTable dtFactor = new DataTable();
          type = type.Trim();
          if (string.IsNullOrEmpty(type))
          {
            ReturnJson(ReturnErrorStr("VOC不能为空！"));
            return;
          }
        
          startTime = startTime.Trim();

          if (string.IsNullOrEmpty(startTime))
          {
            startTime = DateTime.Now.ToString("yyyy-MM-dd");
          }

          StringBuilder sb = new StringBuilder();

          if (type == "1")
          {
            string firstClass = ConfigurationManager.AppSettings["FirstClass"];
            string[] firstClassArr = firstClass.Split(';');
            List<string> listFirst = firstClassArr.ToList<string>();
            DataTable dt = new DataTable();
            #region 旧方法(取单因子小时数据计算)
            //dt.Columns.Add("PointId", typeof(System.String));//污染物名称 参数
            //dt.Columns.Add("Tstamp", typeof(System.DateTime));//时间戳
            //foreach (string colName in listFirst)
            //{
            //    dt.Columns.Add(colName, typeof(System.String));//污染物值
            //}
            //if (listFirst.Contains("非甲烷碳氢化合物"))
            //{
            //    PutInDt("非甲烷碳氢化合物", startTime, dt);
            //}
            //if (listFirst.Contains("卤代烃类"))
            //{
            //    PutInDt("卤代烃类", startTime, dt);
            //}
            //if (listFirst.Contains("含氧（氮）类"))
            //{
            //    PutInDt("含氧（氮）类", startTime, dt);
            //}
            //if (listFirst.Contains("TVOC"))
            //{
            //    PutInDt("TVOC", startTime, dt);
            //}
            #endregion
            #region 新方法(直接取总值表)
            DateTime current = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
            string dtEnd;
            if (current == Convert.ToDateTime(startTime))
            {
                dtEnd = DateTime.Now.ToString("yyyy-MM-dd HH:00:00");
                startTime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:00:00");
            }
            else
            {
                dtEnd = Convert.ToDateTime(startTime).ToString("yyyy-MM-dd 23:00:00");
            }
            dt = GetVOCDataAll("ppb", listFirst.ToArray(), startTime, dtEnd);
            #endregion
            dtAll = dt;
          }
          else if (type == "2")
          {
              string secondClass = ConfigurationManager.AppSettings["SecondClass"];
              string[] secondClassArr = secondClass.Split(',');
              List<string> listSecond = secondClassArr.ToList<string>();

              
              DataTable dt = new DataTable();
              #region 旧方法(取单因子小时数据计算)
              // dt.Columns.Add("PointId", typeof(System.String));//污染物名称 参数
             //dt.Columns.Add("Tstamp", typeof(System.DateTime));//时间戳
             // foreach (string colName in listSecond)
             // {
             //     dt.Columns.Add(colName, typeof(System.String));//污染物值
             // }
             
             // if (listSecond.Contains("醚类有机物"))
             // {
             //     string factorType = "e5f83fd9-0b77-4d1b-935f-1826fddcc343";
             //     PutInDtSecond("醚类有机物", factorType, startTime, dt);
             // }
             // if (listSecond.Contains("酮类有机物"))
             // {
             //   string factorType = "8c9ce5f3-4716-485e-95e1-72608b2843ce";
             //   PutInDtSecond("酮类有机物", factorType, startTime, dt);
             // }
             // if (listSecond.Contains("醛类有机物"))
             // {
             //   string factorType = "8198b6fc-7a77-427d-8e3e-9c9228ac168c";
             //   PutInDtSecond("醛类有机物", factorType, startTime, dt);
             // }
             // if (listSecond.Contains("含氮有机物"))
             // {
             //   string factorType = "3bbe4b30-53e4-48a8-a884-c3b38a03b705";
             //   PutInDtSecond("含氮有机物", factorType, startTime, dt);
             // }
             // if (listSecond.Contains("卤代烷烃"))
             // {
             //   string factorType = "a0bad5d7-9eec-4fa4-9c36-828aad78041d";
             //   PutInDtSecond("卤代烷烃", factorType, startTime, dt);
             // }
             // if (listSecond.Contains("卤代烯烃"))
             // {
             //   string factorType = "21de4143-2c28-4256-b71e-6cb5ce63e417";
             //   PutInDtSecond("卤代烯烃", factorType, startTime, dt);
             // }
             // if (listSecond.Contains("卤代芳香烃"))
             // {
             //   string factorType = "1eaac416-0d69-48b9-aca1-9ff7904907bb";
             //   PutInDtSecond("卤代芳香烃", factorType, startTime, dt);
             // }
             // if (listSecond.Contains("氟利昂"))
             // {
             //   string factorType = "053c74fd-d1ae-4341-b258-1788079970bd";
             //   PutInDtSecond("氟利昂", factorType, startTime, dt);
             // }
             // if (listSecond.Contains("低碳烷烃C2-C5"))
             // {
             //   string factorType = "fb1fc34b-770f-4141-b75a-015919725e0b";
             //   PutInDtSecond("低碳烷烃C2-C5", factorType, startTime, dt);
             // }
             // if (listSecond.Contains("苯系物"))
             // {
             //   string factorType = "e9607fce-75dc-4134-9a8d-af2a1eb4a7bf";
             //   PutInDtSecond("苯系物", factorType, startTime, dt);
             // }
             // if (listSecond.Contains("低碳烯烃C2-C5"))
             // {
             //   string factorType = "a41db3c8-0fb4-4b0e-b1b0-e5950f04eff6";
             //   PutInDtSecond("低碳烯烃C2-C5", factorType, startTime, dt);
             // }
             // if (listSecond.Contains("高碳烯烃C6-C12"))
             // {
             //   string factorType = "7bc5f3d8-7cb3-4273-a646-1ed7df60cdd8";
             //   PutInDtSecond("高碳烯烃C6-C12", factorType, startTime, dt);
             // }
             // if (listSecond.Contains("炔烃"))
             // {
             //   string factorType = "5b1918b9-7c92-477a-8e23-64cbae6477f6";
             //   PutInDtSecond("炔烃", factorType, startTime, dt);
             // }
             // if (listSecond.Contains("高碳烷烃C6-C12"))
             // {
             //   string factorType = "06a02408-6eab-4188-b442-86dd8e97654c";
             //   PutInDtSecond("高碳烷烃C6-C12", factorType, startTime, dt);
              // }
              #endregion

              #region 新方法(直接取总值表)
              DateTime current = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
              string dtEnd;
              if (current == Convert.ToDateTime(startTime))
              {
                  dtEnd = DateTime.Now.ToString("yyyy-MM-dd HH:00:00");
                  startTime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:00:00");
              }
              else
              {
                  dtEnd = Convert.ToDateTime(startTime).ToString("yyyy-MM-dd 23:00:00");
              }
              dt = GetVOCDataAll("ppb", listSecond.ToArray(), startTime, dtEnd);
              #endregion
              dtAll = dt;

          }
          else if (type == "3")
          {
              string sql1 = string.Empty;
              sql1 = string.Format(@" select t1.PollutantCode,t2.PollutantName from  V_Factor_Air_SiteMap as t3 left join [dbo].[DT_VOC3Type]  as t1 
on t3.PID=t1.PollutantCode join [Standard].[TB_PollutantCode] as t2 on t1.PollutantCode=t2.PollutantCode
where  t1.PollutantCode in (SELECT  PollutantCode FROM [Standard].[TB_PollutantCode] where VOCType!='2' and IsUseOrNot=1) order by  POrder desc,COrder desc"
          );
              dtFactor = CreatDataTable(sql1, "AMS_BaseDataConnection");
              string[] listFactor = dtFactor.AsEnumerable().Select(p => p.Field<string>("PollutantCode")).ToArray();
              DataTable auditData = GetVOCsKQYDataPagerNew(listFactor, startTime, dtFactor);
              dtAll = auditData;
          }
          else if (type == "4")
          {
              string firstClass = ConfigurationManager.AppSettings["FirstClass"];
              string[] firstClassArr = firstClass.Split(';');
              List<string> listFirst = firstClassArr.ToList<string>();
              DataTable dt = new DataTable();
              #region 旧方法(取单因子小时数据计算)
              //dt.Columns.Add("PointId", typeof(System.String));//污染物名称 参数
              //dt.Columns.Add("Tstamp", typeof(System.DateTime));//时间戳
              //foreach (string colName in listFirst)
              //{
              //    dt.Columns.Add(colName, typeof(System.String));//污染物值
              //}
              //if (listFirst.Contains("非甲烷碳氢化合物"))
              //{
              //    PutInDt("非甲烷碳氢化合物", startTime, dt);
              //}
              //if (listFirst.Contains("卤代烃类"))
              //{
              //    PutInDt("卤代烃类", startTime, dt);
              //}
              //if (listFirst.Contains("含氧（氮）类"))
              //{
              //    PutInDt("含氧（氮）类", startTime, dt);
              //}
              //if (listFirst.Contains("TVOC"))
              //{
              //    PutInDt("TVOC", startTime, dt);
              //}
              #endregion
              #region 新方法(直接取总值表)
              DateTime current = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
              string dtEnd;
              if (current == Convert.ToDateTime(startTime))
              {
                  dtEnd = DateTime.Now.ToString("yyyy-MM-dd HH:00:00");
                  startTime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:00:00");
              }
              else
              {
                  dtEnd = Convert.ToDateTime(startTime).ToString("yyyy-MM-dd 23:00:00");
              }
              dt = GetVOCDataAll("μg/m³", listFirst.ToArray(), startTime, dtEnd);
              #endregion
              dtAll = dt;
          }
          else if (type == "5")
          {
              string secondClass = ConfigurationManager.AppSettings["SecondClass"];
              string[] secondClassArr = secondClass.Split(',');
              List<string> listSecond = secondClassArr.ToList<string>();


              DataTable dt = new DataTable();
              #region 旧方法(取单因子小时数据计算)
              // dt.Columns.Add("PointId", typeof(System.String));//污染物名称 参数
              //dt.Columns.Add("Tstamp", typeof(System.DateTime));//时间戳
              // foreach (string colName in listSecond)
              // {
              //     dt.Columns.Add(colName, typeof(System.String));//污染物值
              // }

              // if (listSecond.Contains("醚类有机物"))
              // {
              //     string factorType = "e5f83fd9-0b77-4d1b-935f-1826fddcc343";
              //     PutInDtSecond("醚类有机物", factorType, startTime, dt);
              // }
              // if (listSecond.Contains("酮类有机物"))
              // {
              //   string factorType = "8c9ce5f3-4716-485e-95e1-72608b2843ce";
              //   PutInDtSecond("酮类有机物", factorType, startTime, dt);
              // }
              // if (listSecond.Contains("醛类有机物"))
              // {
              //   string factorType = "8198b6fc-7a77-427d-8e3e-9c9228ac168c";
              //   PutInDtSecond("醛类有机物", factorType, startTime, dt);
              // }
              // if (listSecond.Contains("含氮有机物"))
              // {
              //   string factorType = "3bbe4b30-53e4-48a8-a884-c3b38a03b705";
              //   PutInDtSecond("含氮有机物", factorType, startTime, dt);
              // }
              // if (listSecond.Contains("卤代烷烃"))
              // {
              //   string factorType = "a0bad5d7-9eec-4fa4-9c36-828aad78041d";
              //   PutInDtSecond("卤代烷烃", factorType, startTime, dt);
              // }
              // if (listSecond.Contains("卤代烯烃"))
              // {
              //   string factorType = "21de4143-2c28-4256-b71e-6cb5ce63e417";
              //   PutInDtSecond("卤代烯烃", factorType, startTime, dt);
              // }
              // if (listSecond.Contains("卤代芳香烃"))
              // {
              //   string factorType = "1eaac416-0d69-48b9-aca1-9ff7904907bb";
              //   PutInDtSecond("卤代芳香烃", factorType, startTime, dt);
              // }
              // if (listSecond.Contains("氟利昂"))
              // {
              //   string factorType = "053c74fd-d1ae-4341-b258-1788079970bd";
              //   PutInDtSecond("氟利昂", factorType, startTime, dt);
              // }
              // if (listSecond.Contains("低碳烷烃C2-C5"))
              // {
              //   string factorType = "fb1fc34b-770f-4141-b75a-015919725e0b";
              //   PutInDtSecond("低碳烷烃C2-C5", factorType, startTime, dt);
              // }
              // if (listSecond.Contains("苯系物"))
              // {
              //   string factorType = "e9607fce-75dc-4134-9a8d-af2a1eb4a7bf";
              //   PutInDtSecond("苯系物", factorType, startTime, dt);
              // }
              // if (listSecond.Contains("低碳烯烃C2-C5"))
              // {
              //   string factorType = "a41db3c8-0fb4-4b0e-b1b0-e5950f04eff6";
              //   PutInDtSecond("低碳烯烃C2-C5", factorType, startTime, dt);
              // }
              // if (listSecond.Contains("高碳烯烃C6-C12"))
              // {
              //   string factorType = "7bc5f3d8-7cb3-4273-a646-1ed7df60cdd8";
              //   PutInDtSecond("高碳烯烃C6-C12", factorType, startTime, dt);
              // }
              // if (listSecond.Contains("炔烃"))
              // {
              //   string factorType = "5b1918b9-7c92-477a-8e23-64cbae6477f6";
              //   PutInDtSecond("炔烃", factorType, startTime, dt);
              // }
              // if (listSecond.Contains("高碳烷烃C6-C12"))
              // {
              //   string factorType = "06a02408-6eab-4188-b442-86dd8e97654c";
              //   PutInDtSecond("高碳烷烃C6-C12", factorType, startTime, dt);
              // }
              #endregion

              #region 新方法(直接取总值表)
              DateTime current = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
              string dtEnd;
              if (current == Convert.ToDateTime(startTime))
              {
                  dtEnd = DateTime.Now.ToString("yyyy-MM-dd HH:00:00");
                  startTime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:00:00");
              }
              else
              {
                  dtEnd = Convert.ToDateTime(startTime).ToString("yyyy-MM-dd 23:00:00");
              }
              dt = GetVOCDataAll("μg/m³", listSecond.ToArray(), startTime, dtEnd);
              #endregion
              dtAll = dt;

          }
          else if (type == "6")
          {
              string sql1 = string.Empty;
              sql1 = string.Format(@" select t1.PollutantCode,t2.PollutantName from  V_Factor_Air_SiteMap as t3 left join [dbo].[DT_VOC3Type]  as t1 
on t3.PID=t1.PollutantCode join [Standard].[TB_PollutantCode] as t2 on t1.PollutantCode=t2.PollutantCode
where  t1.PollutantCode in (SELECT  PollutantCode FROM [Standard].[TB_PollutantCode] where VOCType!='2' and IsUseOrNot=0) order by  POrder desc,COrder desc"
          );
              dtFactor = CreatDataTable(sql1, "AMS_BaseDataConnection");
              string[] listFactor = dtFactor.AsEnumerable().Select(p => p.Field<string>("PollutantCode")).ToArray();
              DataTable auditData = GetVOCsKQYDataPagerNew(listFactor, startTime, dtFactor);
              dtAll = auditData;
          }
          else
          {
              ReturnJson(ReturnErrorStr("类型填写不正确！"));
              return;
          }
          if (dtAll != null && dtAll.Rows.Count > 0)
          {
              #region datatable转json
              if (type == "3" || type == "6")
              {
                  for (int i = 0; i < dtAll.Rows.Count; i++)
                  {
                      if (sb.Length > 0)
                      {
                          sb.Append(",");
                      }
                      StringBuilder sb1 = new StringBuilder();

                      for (int j = 2; j < dtAll.Columns.Count;j=j+4)
                      {
                        string factor = dtAll.Columns[j].ColumnName;
                        if (!string.IsNullOrEmpty(factor) && !factor.Contains("_Status") && !factor.Contains("_DataFlag") && !factor.Contains("_AuditFlag"))
                        {
                          string factorStatus = dtAll.Rows[i][factor + "_Status"] != DBNull.Value ? dtAll.Rows[i][factor + "_Status"].ToString() : string.Empty;
                          string factorDataFlag = dtAll.Rows[i][factor + "_DataFlag"] != DBNull.Value ? dtAll.Rows[i][factor + "_DataFlag"].ToString() : string.Empty;
                          string factorAuditFlag = dtAll.Rows[i][factor + "_AuditFlag"] != DBNull.Value ? dtAll.Rows[i][factor + "_AuditFlag"].ToString() : string.Empty;
                          string markContent = string.Empty;
                          markContent = TrimChart(factorStatus, factorDataFlag, factorAuditFlag);
                          
                          DataRow[] dr = dtFactor.Select("PollutantCode='" + factor + "'");
                          string factorName = dr[0]["PollutantName"].ToString();
                          string valueStr = string.Empty;
                          int digit = GetDecimalDigit(factor);
                          decimal value = 0M;
                          if (Convert.IsDBNull(dtAll.Rows[i][dtAll.Columns[j].ColumnName]))
                          {
                            valueStr = "--";
                          }
                          else
                          {
                            valueStr = dtAll.Rows[i][dtAll.Columns[j].ColumnName].ToString();
                            if (decimal.TryParse(valueStr, out value))
                            {
                              value = GetPollutantValue(value, digit);
                              valueStr = value.ToString();
                            }
                          }
                          markContent = string.Empty;
                          sb1.AppendFormat(@"{{""factor"": ""{0}"",""factorName"": ""{1}"",""value"": ""{2}"",""flag"": ""{3}"",""measureUnit"":""ppb""}},", dtAll.Columns[j].ColumnName, factorName, valueStr, markContent);
                        }
                      }
                      sb.AppendFormat(@"{{""Tstamp"":""{0}"",""Value"":[{1}]}}", dtAll.Rows[i]["Tstamp"].ToString(), sb1.ToString().TrimEnd(','));
                  }
                  //DateTime etime = DateTime.Now;
                  //TimeSpan sec = etime - stime;
                  //WriteTextLog("表转json时长", sec.Seconds.ToString(), DateTime.Now);
              }
              #endregion
              else if(type=="1" || type=="2")
              {
                  for (int i = 0; i < dtAll.Rows.Count; i++)
                  {
                      if (sb.Length > 0)
                      {
                          sb.Append(",");
                      }
                      StringBuilder sb1 = new StringBuilder();
                      for (int j = 2; j < dtAll.Columns.Count; j++)
                      {
                        string valueStr=string.Empty;
                         int digit=3;
                        decimal value=0M;
                        if (Convert.IsDBNull(dtAll.Rows[i][dtAll.Columns[j].ColumnName]))
                        {
                          valueStr = "--";
                        }
                        else
                        {
                          valueStr = dtAll.Rows[i][dtAll.Columns[j].ColumnName].ToString();
                          if (decimal.TryParse(valueStr, out value))
                          {
                            value = GetPollutantValue(value, digit);
                            valueStr = value.ToString();
                          }
                        }
                          string factorName = dtAll.Columns[j].ColumnName;

                          sb1.AppendFormat(@"{{""factor"": ""{0}"",""factorName"": ""{1}"",""value"": ""{2}"",""flag"": ""{3}"",""measureUnit"":""ppb""}},", dtAll.Columns[j].ColumnName, factorName, valueStr, string.Empty);
                      }
                      sb.AppendFormat(@"{{""Tstamp"":""{0}"",""Value"":[{1}]}}", dtAll.Rows[i]["Tstamp"].ToString(), sb1.ToString().TrimEnd(','));
                  }
              }
              else if (type == "4" || type == "5")
              {
                  for (int i = 0; i < dtAll.Rows.Count; i++)
                  {
                      if (sb.Length > 0)
                      {
                          sb.Append(",");
                      }
                      StringBuilder sb1 = new StringBuilder();
                      for (int j = 2; j < dtAll.Columns.Count; j++)
                      {
                          string valueStr = string.Empty;
                          int digit = 3;
                          decimal value = 0M;
                          if (Convert.IsDBNull(dtAll.Rows[i][dtAll.Columns[j].ColumnName]))
                          {
                              valueStr = "--";
                          }
                          else
                          {
                              valueStr = dtAll.Rows[i][dtAll.Columns[j].ColumnName].ToString();
                              if (decimal.TryParse(valueStr, out value))
                              {
                                  value = GetPollutantValue(value, digit);
                                  valueStr = value.ToString();
                              }
                          }
                          string factorName = dtAll.Columns[j].ColumnName;

                          sb1.AppendFormat(@"{{""factor"": ""{0}"",""factorName"": ""{1}"",""value"": ""{2}"",""flag"": ""{3}"",""measureUnit"":""ppb""}},", dtAll.Columns[j].ColumnName, factorName, valueStr, string.Empty);
                      }
                      sb.AppendFormat(@"{{""Tstamp"":""{0}"",""Value"":[{1}]}}", dtAll.Rows[i]["Tstamp"].ToString(), sb1.ToString().TrimEnd(','));
                  }
              }
          }
          string msg = (string.Format(@"{{""HistoryData"":[{0}]}}", sb));
          ReturnJson(msg);
        }
        catch (Exception e)
        {
          WriteTextLog("数据异常", e.ToString(), DateTime.Now);
          ReturnJson(ReturnErrorStr(e.ToString()));
        }
      }

      private DataTable GetVOCsKQYDataPagerNew(string[] listFactor, string startTime, DataTable dtFactor)
      {
        string factorSql = string.Empty;
        string factorWhere = string.Empty;
        string pointWhere = string.Empty;
        string tableName = "Air.TB_InfectantBy60";
       
        DateTime StTime = Convert.ToDateTime(startTime);
        DateTime dtStart = Convert.ToDateTime(StTime.ToString("yyyy-MM-dd 00:00:00"));
        DateTime current = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
        if (dtStart == current)
        {
          DateTime dtEnd = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00:00"));
          dtStart = dtEnd.AddHours(-26);
          string factorFieldSql = string.Empty;
          string factorFlagSql = string.Empty;
          string factorDataFlagSql = string.Empty;
          string factorAuditFlagSql = string.Empty;
          string sql = string.Empty;
          string allHoursSql = string.Empty;
          foreach (string factor in listFactor)
          {

            string factorFlag = string.Empty;
            string factorDataFlag = string.Empty;
            string factorAuditFlag = string.Empty;
            factorFlag = factor + "_Status";
            factorDataFlag = factor + "_DataFlag";
            factorAuditFlag = factor + "_AuditFlag";
            factorSql += string.Format(",MAX(CASE (PollutantCode) WHEN '{0}' THEN CASE WHEN Datename(hour,Tstamp)=0 THEN Null ELSE PollutantValue END END)  AS [{0}] ", factor);
            factorFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN CASE WHEN Datename(hour,Tstamp)=0 THEN Null ELSE Status END END) AS [{1}] ", factor, factorFlag);
            factorDataFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN CASE WHEN Datename(hour,Tstamp)=0 THEN Null ELSE DataFlag END END) AS [{1}] ", factor, factorDataFlag);
            factorAuditFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN  CASE WHEN Datename(hour,Tstamp)=0 THEN Null ELSE AuditFlag END END) AS [{1}] ", factor, factorAuditFlag);
            factorFieldSql += "," + factor;
           
            factorFieldSql += "," + factorFlag;
            factorFieldSql += "," + factorDataFlag;
            factorFieldSql += "," + factorAuditFlag;
            factorWhere += "'" + factor + "',";
          }
          if (factorWhere.Length > 0)
          {
            factorWhere = factorWhere.Remove(factorWhere.Length - 1, 1);
          }
          pointWhere = " AND PointId=204";
          string where = string.Format("  Tstamp>='{0}' and Tstamp<='{1}'  AND PollutantCode in ({2})", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + pointWhere;
          string fieldName = "PointId,Tstamp" + factorSql + factorFlagSql + factorDataFlagSql + factorAuditFlagSql;
          string orderBy = "time.PointId asc,time.Tstamp desc";
          string groupBy = "PointId,Tstamp";
          string dataSql = string.Format("SELECT  {0} FROM {1} WHERE {2} GROUP BY {3}  ", fieldName, tableName, where, groupBy);
         
          DateTime AllHourdtEndNew = Convert.ToDateTime(dtEnd.ToString("yyyy-MM-dd HH:00")) >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00")) ?
                  DateTime.Now.AddHours(-1) : dtEnd;
            
            allHoursSql += string.Format(@"select top 24 time.PointId,time.Tstamp{0} from dbo.SY_F_GetAllDataByHour('{1}',',','{2}','{3}') time", factorFieldSql, "204", dtStart, AllHourdtEndNew);

            sql += string.Format(@"{0} left join ({1}) data on convert(varchar(13),time.tstamp,120)=convert(varchar(13),data.tstamp,120) and time.pointId=data.pointId ORDER BY {2}"
                , allHoursSql, dataSql, orderBy);
          DataTable dt = CreatDataTable(sql, "AMS_AirAutoMonitorConnection");
          return dt;
        }
        else
        {
          DateTime dtEnd = Convert.ToDateTime(StTime.ToString("yyyy-MM-dd 23:59:59"));

          string factorFieldSql = string.Empty;
          string factorFlagSql = string.Empty;
          string factorDataFlagSql = string.Empty;
          string factorAuditFlagSql = string.Empty;
          string allHoursSql = string.Empty;
          string sql = string.Empty;
          foreach (string factor in listFactor)
          {

            string factorFlag = string.Empty;
            string factorDataFlag = string.Empty;
            string factorAuditFlag = string.Empty;
            factorFlag = factor + "_Status";
            factorDataFlag = factor + "_DataFlag";
            factorAuditFlag = factor + "_AuditFlag";
            factorSql += string.Format(",MAX(CASE (PollutantCode) WHEN '{0}' THEN CASE WHEN Datename(hour,Tstamp)=0 THEN Null ELSE PollutantValue END END)  AS [{0}] ", factor);
            factorFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN Status END) AS [{1}] ", factor, factorFlag);
            factorDataFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN DataFlag END) AS [{1}] ", factor, factorDataFlag);
            factorAuditFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN AuditFlag END) AS [{1}] ", factor, factorAuditFlag);
            factorFieldSql += "," + factor;
            factorFieldSql += "," + factorFlag;
            factorFieldSql += "," + factorDataFlag;
            factorFieldSql += "," + factorAuditFlag;
            factorWhere += "'" + factor + "',";
          }
          if (factorWhere.Length > 0)
          {
            factorWhere = factorWhere.Remove(factorWhere.Length - 1, 1);
          }
          pointWhere = " AND PointId=204";
          string where = string.Format("  Tstamp>='{0}' AND Tstamp<='{1}'  AND PollutantCode in ({2})", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + pointWhere;
          string fieldName = "PointId,Tstamp" + factorSql + factorFlagSql + factorDataFlagSql + factorAuditFlagSql;
          string orderBy = "time.PointId asc,time.Tstamp desc";
          string groupBy = "PointId,Tstamp";
          string dataSql = string.Format("SELECT top 24 {0} FROM {1} WHERE {2} GROUP BY {3}  ", fieldName, tableName, where, groupBy, orderBy);
          DateTime AllHourdtEndNew = Convert.ToDateTime(dtEnd.ToString("yyyy-MM-dd HH:00")) >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00")) ?
                DateTime.Now.AddHours(-1) : dtEnd;

          allHoursSql += string.Format(@"select time.PointId,time.Tstamp{0} from dbo.SY_F_GetAllDataByHour('{1}',',','{2}','{3}') time", factorFieldSql, "204", dtStart, AllHourdtEndNew);

          sql += string.Format(@"{0} left join ({1}) data on convert(varchar(13),time.tstamp,120)=convert(varchar(13),data.tstamp,120) and time.pointId=data.pointId ORDER BY {2}"
              , allHoursSql, dataSql, orderBy);
          DataTable dt = CreatDataTable(sql, "AMS_AirAutoMonitorConnection");
          return dt;
        }
      }
/// <summary>
/// 填充二级类表格
/// </summary>
/// <param name="p"></param>
/// <param name="startTime"></param>
/// <param name="dt"></param>
      private void PutInDtSecond(string name,string factorType, string startTime, DataTable dt)
      {
          

          string sql1 = string.Empty;
         sql1 = string.Format(@"select PollutantCode from [dbo].[DT_VOC3Type] 
                                                    where VOC2TypeGuid='{0}' and PollutantCode in (SELECT  PollutantCode FROM [Standard].[TB_PollutantCode] where VOCType!='2' and IsUseOrNot=1)"
                , factorType);
          
          DataTable dtFactor = CreatDataTable(sql1, "AMS_BaseDataConnection");
          List<string> listFactor = new List<string>();
          if (dtFactor != null && dtFactor.Rows.Count > 0)
          {
              for (int i = 0; i < dtFactor.Rows.Count; i++)
              {
                  string factorCode = dtFactor.Rows[i]["PollutantCode"].ToString();
                  if (!listFactor.Contains(factorCode))
                  {
                      listFactor.Add(factorCode);
                  }
              }
          }
          DataTable auditData = GetVOCsKQYDataPager(listFactor, startTime, dtFactor);
          if (auditData.Rows.Count > 0)
          {
              GetDataTable(name, auditData, dt);
          }
      }
      /// <summary>
      /// 区分单位的VOC一级类总和数据（非零点）
      /// </summary>
      /// <param name="unit"></param>
      /// <param name="TypeNames"></param>
      /// <param name="ds"></param>
      /// <param name="de"></param>
      /// <returns></returns>
      public DataTable GetVOCDataAll(string unit, string[] TypeNames, string ds, string de)
      {
          try
          {
              string selectsql = "select time.PointId,time.Tstamp";
              string factorSql = string.Empty;
              foreach (string TypeName in TypeNames)
              {
                  selectsql += string.Format(@",[{0}] ", TypeName);//sql防止列名中含有中文括号需要[]表示列名
                  factorSql += string.Format(",MAX(CASE TypeName WHEN '{0}' THEN CASE WHEN Datename(hour,Tstamp)=0 THEN Null ELSE  PollutantValue END END) AS [{0}] ", TypeName);
              }
              DateTime AllHourdtEndNew = Convert.ToDateTime(de) >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00")) ?
                      DateTime.Now.AddHours(-1) : Convert.ToDateTime(de);
              selectsql += string.Format(@"from dbo.SY_F_GetAllDataByHour('204',',','{0}','{1}') time left join (", ds, AllHourdtEndNew.ToString("yyyy-MM-dd HH:00:00"));
              string sql = string.Format(@"{0} SELECT '204' AS PointId ,Tstamp {1} FROM [Air].[TB_VOCStatistics] WHERE Tstamp>='{2}' AND Tstamp<='{3}'  and Description='{4}'  GROUP BY Tstamp) data 
ON convert(varchar(13),time.tstamp,120)=convert(varchar(13),data.tstamp,120) and time.pointId=data.pointId order by Tstamp  desc", selectsql, factorSql, ds, de, unit);
              return CreatDataTable(sql, "AMS_AirAutoMonitorConnection");
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }
/// <summary>
/// 按分类计算值并填充数据到DataTable中
/// </summary>
/// <param name="p"></param>
      private void PutInDt(string name, string startTime,DataTable dt)
      {


          string factorWan = name;

              string sql1 = string.Empty;
              if ("TVOC".Equals(name))
              {
                  sql1 = string.Format(@"select PollutantCode from [dbo].[DT_VOC3Type]
where VOC1TypeGuid IN (SELECT RowGuid from [dbo].[DT_VOC1Type] ) and  PollutantCode in (SELECT  PollutantCode FROM [Standard].[TB_PollutantCode] where VOCType!='2' and IsUseOrNot=1) ");
               
              }
              else
              {
                  sql1 = string.Format(@"select PollutantCode from [dbo].[DT_VOC3Type]
where VOC1TypeGuid IN (SELECT RowGuid from [dbo].[DT_VOC1Type] where VOC1Type in ('{0}')) and  PollutantCode in (SELECT  PollutantCode FROM [Standard].[TB_PollutantCode] where VOCType!='2' and IsUseOrNot=1) "
      , factorWan);
              }
           
           DataTable dtFactor = CreatDataTable(sql1, "AMS_BaseDataConnection");
              List<string> listFactor = new List<string>();
              if (dtFactor != null && dtFactor.Rows.Count > 0)
              {
                  for (int i = 0; i < dtFactor.Rows.Count; i++)
                  {
                      string factorCode = dtFactor.Rows[i]["PollutantCode"].ToString();
                      if (!listFactor.Contains(factorCode))
                      {
                          listFactor.Add(factorCode);
                      }
                  }
              }
              DataTable auditData = GetVOCsKQYDataPager(listFactor, startTime, dtFactor);
              if (auditData.Rows.Count > 0)
              {
                  GetDataTable(name, auditData, dt);
              }
      }
      [WebMethod(Description = @"定义：获取城市摄影和激光雷达的图片地址<br/> instrumentUid:仪器的guid；DateTime:查询数据的时间，如果不填则默认当前时间。时间格式(yyyy-MM-dd HH:00:00)")]
      public void GetImageUrl(string instrumentUid, string startTime)
      {
        try
        {
          instrumentUid = instrumentUid.Trim();
          startTime = startTime.Trim();
          string imageGuidStr = ConfigurationManager.AppSettings["CityImageGuid"];
          string imageJGLDGuidStr = ConfigurationManager.AppSettings["JGLDImageGuid"];

          string _filepath = ConfigurationManager.AppSettings["UrbanPhoto"].ToString();
          string _JGLDFilePath = ConfigurationManager.AppSettings["NTJGLDXG"].ToString();
          if (string.IsNullOrEmpty(startTime))
          {
            startTime = DateTime.Now.ToString("yyyy-MM-dd HH:00:00");
          }
          string msg = "";
          string jsonsb = string.Empty;
          string sql = string.Empty;
          if (imageGuidStr.Equals(instrumentUid))
          {

            DateTime dtStart = Convert.ToDateTime(startTime);
            dtStart = Convert.ToDateTime(dtStart.ToString("yyyy-MM-dd"));
            DateTime dtEnd = dtStart.AddDays(1);
            
            string _path = "http://218.91.209.251:1117/CSYC/";
            string[] childfilelist = GetChildFilesList(_filepath, dtStart, dtEnd).Distinct().ToArray();
            DataTable dt = new DataTable();
            dt.TableName = "CityImage";
            dt.Columns.Add("ImageUrl", typeof(string));
            dt.Columns.Add("ImageDate", typeof(string));
            if (childfilelist.Length > 0)
            {

              foreach (string str in childfilelist)
              {
                DataRow dr = dt.NewRow();
               
                dr["ImageUrl"] = _path +str;
                dr["ImageDate"] = GetCityDate(str.Substring(13, 19)).ToString("yyyy-MM-dd HH:mm:ss");

                dt.Rows.Add(dr);
              }
              
            }
            msg = DataTableToJson(dt);

            ReturnJson(msg);
          }
          else if (imageJGLDGuidStr.Equals(instrumentUid))
          {
            DateTime dtStart = Convert.ToDateTime(startTime);
            dtStart = Convert.ToDateTime(dtStart.ToString("yyyy-MM-dd HH:00:00"));
            DateTime dtEnd = dtStart;
            dtStart = dtStart.AddHours(-1);
            string _path = "http://218.91.209.251:1117/NTJGLDXG/image/";
            DataTable dt = new DataTable();
            dt.TableName = "JGLDImage";
            dt.Columns.Add("ImageUrl", typeof(string));
            dt.Columns.Add("ImageDate", typeof(string));
            string[] filelist = GetFilesList(_JGLDFilePath, dtStart, dtEnd);
            if (filelist.Length > 0)
            {
              foreach (string str in filelist)
              {

                DataRow dr = dt.NewRow();

                dr["ImageUrl"] = _path + str;
                dr["ImageDate"] = GetDate(str.Substring(5, 10)).ToString("yyyy-MM-dd HH:00:00");

                dt.Rows.Add(dr);
              }
              
            }
            msg = DataTableToJson(dt);
            ReturnJson(msg);
          }
        }
        catch (Exception e)
        {
          WriteTextLog("数据异常", e.ToString(), DateTime.Now);
          ReturnJson(ReturnErrorStr());
        }
      }
      [WebMethod(Description = @"定义：获取所有启用的仪器菜单(除了VOC和粒径普仪)内24小时因子的小时数据<br/> instrumentUid:仪器的guid；DateTime:查询数据的日期，如果不填则默认当前日期。时间格式(yyyy-MM-dd)")]
      public void GetInstrumentsFactorsData(string instrumentUid, string startTime)
      {
        try{
         
        instrumentUid = instrumentUid.Trim();
        startTime = startTime.Trim();
        string tyfsyGuid = ConfigurationManager.AppSettings["TYFSYGuid"];
        string LZSPYGuid = ConfigurationManager.AppSettings["LZSPYGuid"];
       // string imageGuidStr = ConfigurationManager.AppSettings["CityImageGuid"];
       // string imageJGLDGuidStr = ConfigurationManager.AppSettings["JGLDImageGuid"];
       
       // string _filepath = ConfigurationManager.AppSettings["UrbanPhoto"].ToString();
       //string _JGLDFilePath=ConfigurationManager.AppSettings["NTJGLDXG"].ToString();
//        List<string> listLZSPY = new List<string>();
//        string sqlLZSPY = string.Format(@"select  t1.PollutantUid,t1.PollutantCode,t2.PollutantName,t2.DecimalDigit,t3.[ItemText] as measureUnit 
//from InstrInfo.TB_InstrumentChannels as t1 
//left join [Standard].[TB_PollutantCode] as t2 on t1.PollutantCode=t2.PollutantCode   
//left join dbo.SY_Frame_Code_Item as t3 on t2.MeasureUnitUid=t3.RowGuid 
//where t1.InstrumentUid='{0}' order by t2.OrderByNum desc,t1.PollutantCode", LZSPYGuid);
//        DataTable dtLZSPY = CreatDataTable(sqlLZSPY, "AMS_BaseDataConnection");
//        if (dtLZSPY != null && dtLZSPY.Rows.Count > 0)
//        {
//          for(int i=0;i<dtLZSPY.Rows.Count;i++)
//          {
//            string pollutantCode = dtLZSPY.Rows[i]["PollutantCode"].ToString();
//            if (!listLZSPY.Contains(pollutantCode))
//            {
//              listLZSPY.Add(pollutantCode);
//            }
//          }
//        }
        if (string.IsNullOrEmpty(startTime))
        {
          startTime = DateTime.Now.ToString("yyyy-MM-dd");
        }
        string msg = "";
        string jsonsb = string.Empty;
        string sql = string.Empty;

        sql = string.Format(@"select dd.* from  ( select 
                       t2.PollutantUid
                       ,t2.PollutantCode
                       ,t2.PollutantName
                       ,t2.DecimalDigit
                       ,t3.ItemText as measureUnit
                        ,POrder
                        ,COrder
                        ,allInfo.PGuid
                    from V_Factor_Air_SiteMap as allInfo
                    
                   join [Standard].[TB_PollutantCode] as t2
                  
                  on allInfo.CGuid=t2.PollutantUid
                  
                    left join dbo.SY_Frame_Code_Item as t3
                    on t2.MeasureUnitUid=t3.RowGuid
                    left join
                    (
                        SELECT DISTINCT PollutantUid
                        FROM InstrInfo.TB_InstrumentChannels
                        WHERE InstrumentUid='{0}'
                            
                    ) as insCh
                        on allInfo.CGuid = insCh.PollutantUid
                    where    (insCh.PollutantUid IS NOT NULL) )  as dd left join ( select * from  V_Factor_Air_SiteMap where PGuid is null) as ff on dd.PGuid=ff.CGuid order by ff.Porder desc,dd.Corder desc
", instrumentUid);
        //sql = string.Format(@"select  t1.PollutantUid,t1.PollutantCode,t2.PollutantName,t2.DecimalDigit,t3.itemValue as measureUnit from InstrInfo.TB_InstrumentChannels as t1 left join [Standard].[TB_PollutantCode] as t2 on t1.PollutantCode=t2.PollutantCode  left join [dbo].[TB_Frame_CodeItem] as t3 on t2.MeasureUnitUid=t3.RowGuid left join V_Factor_Air_SiteMap as t4 on t4.Cguid=t2.PollutantUid where t1.InstrumentUid='{0}' order by t4.Porder desc,t4.COrder desc", instrumentUid);
        DataTable dtFactor = CreatDataTable(sql, "AMS_BaseDataConnection");
        List<string> listFactor = new List<string>();
        if (dtFactor != null && dtFactor.Rows.Count > 0)
        {
          for (int i = 0; i < dtFactor.Rows.Count; i++)
          {
            string factorCode = dtFactor.Rows[i]["PollutantCode"].ToString();
            if (!listFactor.Contains(factorCode))
            {
              listFactor.Add(factorCode);
            }
          }
        }
        if (tyfsyGuid.Equals(instrumentUid))
        {
          listFactor.Add("a05024");
          listFactor.Remove("a05040");
          listFactor.Remove("a05041");
          listFactor.Remove("a90162");
        }
        StringBuilder sb = new StringBuilder();
        DataTable dtOriginalData=GetHourData(listFactor, startTime,dtFactor, instrumentUid);
        if (dtOriginalData != null && dtOriginalData.Rows.Count > 0)
        {
          for (int i = 0; i < dtOriginalData.Rows.Count; i++)
          {
            if (sb.Length > 0)
            {
              sb.Append(",");
            }
            StringBuilder sb1 = new StringBuilder();
            for (int j = 2; j < dtOriginalData.Columns.Count; j++)
            {
              string factor = dtOriginalData.Columns[j].ColumnName;
              if (!string.IsNullOrEmpty(factor) && !factor.Contains("_Status") && !factor.Contains("_DataFlag") && !factor.Contains("_AuditFlag"))
              {
                string factorStatus = dtOriginalData.Rows[i][factor + "_Status"] != DBNull.Value ? dtOriginalData.Rows[i][factor + "_Status"].ToString() : string.Empty;
                string factorDataFlag = dtOriginalData.Rows[i][factor + "_DataFlag"] != DBNull.Value ? dtOriginalData.Rows[i][factor + "_DataFlag"].ToString() : string.Empty;
                string factorAuditFlag = dtOriginalData.Rows[i][factor + "_AuditFlag"] != DBNull.Value ? dtOriginalData.Rows[i][factor + "_AuditFlag"].ToString() : string.Empty;
                string markContent = string.Empty;
                markContent = TrimChart(factorStatus, factorDataFlag, factorAuditFlag);
                //if (((factorStatus != "N" && factorStatus != "d" && factorStatus != "MF" && !string.IsNullOrEmpty(factorStatus)) || (factorDataFlag != "N" && factorDataFlag != "d" && factorDataFlag != "MF" && !string.IsNullOrEmpty(factorDataFlag)) || (factorAuditFlag != "N" && factorAuditFlag != "d" && factorAuditFlag != "MF" && !string.IsNullOrEmpty(factorAuditFlag))))
                //{
                //  markContent = string.Empty;
                //  if ((factorStatus != "N" && factorStatus != "d" && factorStatus != "MF" && !string.IsNullOrEmpty(factorStatus)) || (factorDataFlag != "N" && factorDataFlag != "d" && factorDataFlag != "MF" && !string.IsNullOrEmpty(factorDataFlag)) || (factorAuditFlag != "N" && factorAuditFlag != "d" && factorAuditFlag != "MF" && !string.IsNullOrEmpty(factorAuditFlag)))
                //  {
                //    markContent += factorStatus + "," + factorDataFlag + "," + factorAuditFlag;
                //    markContent = markContent.TrimEnd(',').TrimEnd(',');
                //  }
                //}
                string valueStr = string.Empty;
                decimal value;
                DataRow[] dr = dtFactor.Select("PollutantCode='" + factor + "'");
                string factorName = string.Empty;
                int digit;
                string measureUnit = string.Empty;
                if (factor == "a05024")
                {
                  factorName = "O₃";
                  digit = 3 - 3;
                  measureUnit = "μg/m³";
                  if (Convert.IsDBNull(dtOriginalData.Rows[i][dtOriginalData.Columns[j].ColumnName]))
                  {
                    valueStr = "--";
                  }
                  else
                  {
                    valueStr = dtOriginalData.Rows[i][dtOriginalData.Columns[j].ColumnName].ToString();
                    if (decimal.TryParse(valueStr, out value))
                    {
                      value = GetPollutantValue(value * 1000, digit);
                      valueStr = value.ToString();
                    }
                  }
                }
                else
                {
                  factorName = dr[0]["PollutantName"].ToString();
                  measureUnit = dr[0]["measureUnit"].ToString();
                  string digitStr = dr[0]["DecimalDigit"].ToString();
                  if ("μg/m³".Equals(measureUnit))
                  {
                    digit = string.IsNullOrEmpty(digitStr) ? 0 : Convert.ToInt32(digitStr) - 3;
                    if (Convert.IsDBNull(dtOriginalData.Rows[i][dtOriginalData.Columns[j].ColumnName]))
                    {
                      valueStr = "--";
                    }
                    else
                    {
                      valueStr = dtOriginalData.Rows[i][dtOriginalData.Columns[j].ColumnName].ToString();
                      if (decimal.TryParse(valueStr, out value))
                      {
                        value = GetPollutantValue(value * 1000, digit);
                        valueStr = value.ToString();
                      }
                    }
                  }
                  else
                  {
                    digit = string.IsNullOrEmpty(digitStr) ? 3 : Convert.ToInt32(digitStr);
                    if (Convert.IsDBNull(dtOriginalData.Rows[i][dtOriginalData.Columns[j].ColumnName]))
                    {
                      valueStr = "--";
                    }
                    else
                    {
                      valueStr = dtOriginalData.Rows[i][dtOriginalData.Columns[j].ColumnName].ToString();
                      if (decimal.TryParse(valueStr, out value))
                      {
                        value = GetPollutantValue(value, digit);
                        valueStr = value.ToString();
                      }
                    }
                  }
                }

                markContent = string.Empty;
                sb1.AppendFormat(@"{{""factor"": ""{0}"",""factorName"": ""{1}"",""value"": ""{2}"",""flag"": ""{3}"",""measureUnit"":""{4}""}},", dtOriginalData.Columns[j].ColumnName, factorName, valueStr, markContent, measureUnit.Replace("<sup>3</sup>", "³"));
              }
            }
            sb.AppendFormat(@"{{""Tstamp"":""{0}"",""Value"":[{1}]}}", dtOriginalData.Rows[i]["Tstamp"].ToString(), sb1.ToString().TrimEnd(','));
          }
         
        }
        msg=(string.Format(@"{{""HistoryData"":[{0}]}}", sb));
        ReturnJson(msg);
        }   
        catch (Exception e)
        {
          WriteTextLog("数据异常", e.ToString(), DateTime.Now);
          ReturnJson(ReturnErrorStr());
        }
      }
      /// <summary>
      /// 获取数据源 日数据
      /// </summary>
      public void GetDataTable(string columnName, DataTable auDT, DataTable dt)
      {
          if (dt.Rows.Count > 0)
          {
              foreach (DataRow dr in auDT.Rows)
              {
                  decimal wanValue = 0;
                  foreach (DataColumn dc in auDT.Columns)
                  {
                      if (dc.ColumnName != "PointId" && dc.ColumnName != "Tstamp")
                      {
                        wanValue += dr[dc.ColumnName] != DBNull.Value ? GetPollutantValue(Convert.ToDecimal(dr[dc.ColumnName]), 3) : 0;
                      }
                  }

                  DataRow[] drs = dt.Select(string.Format("Tstamp='{0}' and PointId={1}", dr["Tstamp"], dr["PointId"]));

                  if (drs.Length > 0)
                  {
                      if (wanValue == 0)
                      {
                          drs[0][columnName] =DBNull.Value;
                      }
                      else
                      {
                          drs[0][columnName] = wanValue.ToString();
                      }
                  }
                  //else
                  //{
                  //    DataRow drh = dt.NewRow();
                  //    drh["PointId"] = dr["PointId"].ToString();
                  //    drh["Tstamp"] = dr["Tstamp"].ToString();
                  //    if (wanValue == 0)
                  //    {
                  //        drh[columnName] = DBNull.Value;
                  //    }
                  //    else
                  //    {
                  //        drh[columnName] = wanValue.ToString();
                  //    }
                  //    dt.Rows.Add(drh);
                  //}
                  
              }
          }
          else
          {
              foreach (DataRow dr in auDT.Rows)
              {
                  decimal wanValue = 0;
                  foreach (DataColumn dc in auDT.Columns)
                  {
                      if (dc.ColumnName != "PointId" && dc.ColumnName != "Tstamp")
                      {
                          wanValue += dr[dc.ColumnName] != DBNull.Value ? GetPollutantValue(Convert.ToDecimal(dr[dc.ColumnName]),3) : 0;
                      }
                  }
                  DataRow drh = dt.NewRow();
                  drh["PointId"] = dr["PointId"].ToString();
                  drh["Tstamp"] = dr["Tstamp"].ToString();
                  if (wanValue == 0)
                  {
                    drh[columnName] = DBNull.Value;
                  }
                  else
                  {
                    drh[columnName] = wanValue.ToString();
                    
                  }
                  dt.Rows.Add(drh);
                  

                 
              }
          }
          
       
          
        
      }
      /// <summary>
      /// 获取文件夹内指定时间段内的文件
      /// </summary>
      /// <param name="fullpath">文件绝对路径</param>
      /// <param name="startdate">开始日期</param>
      /// <param name="enddate">截止日期</param>
      /// <returns></returns>
      public string[] GetFilesList(string fullpath, DateTime startdate, DateTime enddate)
      {
        var query = (from f in Directory.GetFiles(fullpath)
                     let fi = new FileInfo(f)
                     where IsOrNotImg(fi.Name) && GetDate(fi.Name.Substring(5, 10)) > startdate && GetDate(fi.Name.Substring(5, 10)) <=enddate
                     orderby GetDate(fi.Name.Substring(5, 10)) descending
                     select fi.Name);
        return query.ToArray();
      }
      /// <summary>
      /// 将日期格式的字符串转为日期输出
      /// </summary>
      /// <param name="str">日期格式的字符串yyyyMMdd</param>
      /// <returns></returns>
      public DateTime GetDate(string str)
      {
        DateTime dt = DateTime.Parse("1900-01-01");
        if (str.Length == 10)
        {
          //IFormatProvider format = new System.Globalization.CultureInfo("zh-CN");
          string FormatStr = "yyyyMMddHH";
          dt = DateTime.ParseExact(str, FormatStr, CultureInfo.InvariantCulture);
        }
        return dt;
      }
      /// <summary>
      /// 想DataTable中填充数据
      /// </summary>
      /// <param name="dt"></param>
      private void FillDataRow(DataTable dt, string str, int kind)
      {
        DataRow dr = dt.NewRow();
        string name = GetDepartName(str);
        dr["Kind"] = kind;
        dr["DepartFloor"] = name;
        dt.Rows.Add(dr);
      }
      /// <summary>
      /// 获取层的名字
      /// </summary>
      /// <param name="str"></param>
      private string GetDepartName(string str)
      {
        string valueName = string.Empty;
        if (!string.IsNullOrEmpty(str))
        {
          //string[] numbers = str.Split(',');
          //valueName = numbers[0] + "~" + numbers[1] + "um";
          if (str == "a51008,a51038")
          {
              valueName = ">0.25um";
          }
          if (str == "a51008,a51011")
          {
              valueName = "0.25um-0.4um";
          }
          if (str == "a51011,a51022")
          {
              valueName = "0.4um-2.5um";
          }
          if (str == "a51022,a51038")
          {
              valueName = ">2.5um";
          }
        }
        return valueName;
      }
      /// <summary>
      /// 获取voc全部因子的数据
      /// </summary>
      /// <param name="listFactor"></param>
      /// <param name="startTime"></param>
      /// <param name="dtFactor"></param>
      /// <returns></returns>
      private DataTable GetVOCsKQYDataPager(List<string> listFactor, string startTime, DataTable dtFactor)
      {
        string factorSql = string.Empty;
        string factorWhere = string.Empty;
        string pointWhere = string.Empty;
        string tableName = "Air.TB_InfectantBy60";
      
        DateTime StTime = Convert.ToDateTime(startTime);
        DateTime dtStart = Convert.ToDateTime(StTime.ToString("yyyy-MM-dd 00:00:00"));
        DateTime current=Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
        if (current == dtStart)
        {
          DateTime dtEnd = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00:00"));
          dtStart = dtEnd.AddHours(-26);
          string factorFieldSql = string.Empty;
          string allHoursSql = string.Empty;
          string sql = string.Empty;
          foreach (string factor in listFactor)
          {

            factorSql += string.Format(",MAX(CASE (PollutantCode) WHEN '{0}' THEN CASE WHEN Datename(hour,Tstamp)=0 THEN Null ELSE PollutantValue END END)  AS [{0}] ", factor);
            factorFieldSql += "," + factor;

            factorWhere += "'" + factor + "',";
          }
          if (factorWhere.Length > 0)
          {
            factorWhere = factorWhere.Remove(factorWhere.Length - 1, 1);
          }
          pointWhere = " AND PointId=204";
          string where = string.Format("  Tstamp>='{0}' and Tstamp<='{1}'  AND PollutantCode in ({2})", dtStart.ToString("yyyy-MM-dd HH:00:00"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + pointWhere;
          string fieldName = "PointId,Tstamp" + factorSql;
          string orderBy = "time.PointId asc,time.Tstamp desc";
          string groupBy = "PointId,Tstamp";
          string dataSql = string.Format("SELECT  {0} FROM {1} WHERE {2} GROUP BY {3}  ", fieldName, tableName, where, groupBy, orderBy);
          DateTime AllHourdtEndNew = Convert.ToDateTime(dtEnd.ToString("yyyy-MM-dd HH:00")) >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00")) ?
                DateTime.Now.AddHours(-1) : dtEnd;

          allHoursSql += string.Format(@"select top 24 time.PointId,time.Tstamp{0} from dbo.SY_F_GetAllDataByHour('{1}',',','{2}','{3}') time", factorFieldSql, "204", dtStart, AllHourdtEndNew);

          sql += string.Format(@"{0} left join ({1}) data on convert(varchar(13),time.tstamp,120)=convert(varchar(13),data.tstamp,120) and time.pointId=data.pointId ORDER BY {2}"
              , allHoursSql, dataSql, orderBy);
          DataTable dt = CreatDataTable(sql, "AMS_AirAutoMonitorConnection");
          return dt;
        }
        else
        {
          DateTime dtEnd = Convert.ToDateTime(StTime.ToString("yyyy-MM-dd 23:59:59"));

          string factorFieldSql = string.Empty;
          string sql = string.Empty;
          string allHoursSql = string.Empty;
          foreach (string factor in listFactor)
          {

            factorSql += string.Format(",MAX(CASE (PollutantCode) WHEN '{0}' THEN CASE WHEN Datename(hour,Tstamp)=0 THEN Null ELSE PollutantValue END END)  AS [{0}] ", factor);
            factorFieldSql += "," + factor;
            factorWhere += "'" + factor + "',";
          }
          if (factorWhere.Length > 0)
          {
            factorWhere = factorWhere.Remove(factorWhere.Length - 1, 1);
          }
          pointWhere = " AND PointId=204";
          string where = string.Format("  Tstamp>='{0}' AND Tstamp<='{1}'  AND PollutantCode in ({2})", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + pointWhere;
          string fieldName = "PointId,Tstamp" + factorSql;
          string orderBy = "time.PointId asc,time.Tstamp desc";
          string groupBy = "PointId,Tstamp";
          string dataSql = string.Format("SELECT {0} FROM {1} WHERE {2} GROUP BY {3} ", fieldName, tableName, where, groupBy, orderBy);
          DateTime AllHourdtEndNew = Convert.ToDateTime(dtEnd.ToString("yyyy-MM-dd HH:00")) >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00")) ?
              DateTime.Now.AddHours(-1) : dtEnd;

          allHoursSql += string.Format(@"select  time.PointId,time.Tstamp{0} from dbo.SY_F_GetAllDataByHour('{1}',',','{2}','{3}') time", factorFieldSql, "204", dtStart, AllHourdtEndNew);

          sql += string.Format(@"{0} left join ({1}) data on convert(varchar(13),time.tstamp,120)=convert(varchar(13),data.tstamp,120) and time.pointId=data.pointId ORDER BY {2}"
              , allHoursSql, dataSql, orderBy);
          DataTable dt = CreatDataTable(sql, "AMS_AirAutoMonitorConnection");
          return dt;
        }
      }
      /// <summary>
      /// 得到该日期当天的数据
      /// </summary>
      /// <param name="listFactor"></param>
      /// <param name="startTime"></param>
      private DataTable GetHourData(List<string> listFactor, string startTime, DataTable dtFactor, string instrumentUid)
      {
        string LZSPYGuid = ConfigurationManager.AppSettings["LZSPYGuid"];
        string factorSql = string.Empty;
        string factorWhere = string.Empty;
        string pointWhere = string.Empty;
        string tableName = "Air.TB_InfectantBy60";
        string factorFlagSql = string.Empty;
        string factorDataFlagSql = string.Empty;
        string factorAuditFlagSql = string.Empty;
        DateTime StTime = Convert.ToDateTime(startTime);

        DateTime dtStart = Convert.ToDateTime(StTime.ToString("yyyy-MM-dd 00:00:00"));
        DateTime current = Convert.ToDateTime(DateTime.Now.ToString(("yyyy-MM-dd 00:00:00")));
        if (current == dtStart)
        {
          DateTime dtEnd =Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00:00"));
          dtStart = dtEnd.AddHours(-26);
          string factorFieldSql = string.Empty;
          string sql = string.Empty;
          if (listFactor.Contains("a05024"))
          {
            StringBuilder sb = new StringBuilder();
            sb.Append("select  A.*,B.a05024,B.[a05024_Status],[a05024_DataFlag],B.[a05024_AuditFlag] from (select ");
            DataRow[] drs = dtFactor.Select("PollutantCode='a05024'");


            foreach (string factor in listFactor)
            {
              string factorFlag = string.Empty;
              string factorDataFlag = string.Empty;
              string factorAuditFlag = string.Empty;
              if (factor != "a05024")
              {
                factorFlag = factor + "_Status";
                factorDataFlag = factor + "_DataFlag";
                factorAuditFlag = factor + "_AuditFlag";
                DataRow[] dr = dtFactor.Select("PollutantCode='" + factor + "'");


                factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                factorFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN Status END) AS [{1}] ", factor, factorFlag);
                factorDataFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN DataFlag END) AS [{1}] ", factor, factorDataFlag);
                factorAuditFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN AuditFlag END) AS [{1}] ", factor, factorAuditFlag);

                factorWhere += "'" + factor + "',";
              }
              factorFieldSql += "," + factor;
              factorFieldSql += "," + factor + "_Status";
              factorFieldSql += "," + factor + "_DataFlag";
              factorFieldSql += "," + factor + "_AuditFlag";
            }
            factorWhere = factorWhere + "'a05024'";

            pointWhere = " AND PointId=204";
            string where = string.Format("  Tstamp>='{0}' and Tstamp<='{1}'  AND PollutantCode in ({2})", dtStart.ToString("yyyy-MM-dd HH:00:00"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + pointWhere;
            string sqlForO3 = string.Format(@"select PointId,Tstamp
                                                    ,MAX(CASE(PollutantCode) WHEN 'a05024' THEN PollutantValue END )  AS [a05024] 
                                                    ,MAX(CASE(PollutantCode) WHEN 'a05024' THEN Status END) AS [a05024_Status] 
                                                    ,MAX(CASE(PollutantCode) WHEN 'a05024' THEN DataFlag END) AS [a05024_DataFlag] 
                                                    ,MAX(CASE(PollutantCode) WHEN 'a05024' THEN AuditFlag END) AS [a05024_AuditFlag] 
                                                    from {0}  WHERE Tstamp>='{1}' and Tstamp<='{2}' {3} group by PointId,Tstamp) B ", tableName, dtStart.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss"), pointWhere);

            string fieldName = "PointId,Tstamp" + factorSql + factorFlagSql + factorDataFlagSql + factorAuditFlagSql;
            string orderBy = "time.Tstamp desc";
            string groupBy = "PointId,Tstamp";
            sb.Append(fieldName + " from " + tableName + " where " + where + " group by " + groupBy + ") A left join (" + sqlForO3 + "on A.Tstamp = DATEADD(hour,1, B.Tstamp) and A.PointId=B.PointId  ");
            string dataSql = sb.ToString();
            string allHoursSql = string.Empty;
            //由于实时数据通常晚1小时到数据库（离子色谱仪晚两个小时），补数据时间需要剔除当前小时及之后的小时
            DateTime AllHourdtEndNew = Convert.ToDateTime(dtEnd.ToString("yyyy-MM-dd HH:00")) >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00")) ?
                DateTime.Now.AddHours(-1) : dtEnd;

            allHoursSql += string.Format(@"select top 24 time.PointId,time.Tstamp{0} from dbo.SY_F_GetAllDataByHour('{1}',',','{2}','{3}') time", factorFieldSql, "204", dtStart, AllHourdtEndNew);

            sql += string.Format(@"{0} left join ({1}) data on convert(varchar(13),time.tstamp,120)=convert(varchar(13),data.tstamp,120) and time.pointId=data.pointId ORDER BY {2}"
                , allHoursSql, dataSql, orderBy);
          }
          else
          {
            foreach (string factor in listFactor)
            {
              string factorFlag = factor + "_Status";
              string factorDataFlag = factor + "_DataFlag";
              string factorAuditFlag = factor + "_AuditFlag";
              DataRow[] dr = dtFactor.Select("PollutantCode='" + factor + "'");
              string factorName = dr[0]["PollutantName"].ToString();


              factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
              factorFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN Status END) AS [{1}] ", factor, factorFlag);
              factorDataFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN DataFlag END) AS [{1}] ", factor, factorDataFlag);
              factorAuditFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN AuditFlag END) AS [{1}] ", factor, factorAuditFlag);
              factorFieldSql += "," + factor;
              factorFieldSql += "," + factorFlag;
              factorFieldSql += "," + factorDataFlag;
              factorFieldSql += "," + factorAuditFlag;
              factorWhere += "'" + factor + "',";
            }
            if (factorWhere.Length > 0)
            {
              factorWhere = factorWhere.Remove(factorWhere.Length - 1, 1);
            }
            pointWhere = " AND PointId=204";
            string where = string.Format("  Tstamp>='{0}' and Tstamp<='{1}'  AND PollutantCode in ({2})", dtStart.ToString("yyyy-MM-dd HH:00:00"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + pointWhere;
            string fieldName = "PointId,Tstamp" + factorSql + factorFlagSql + factorDataFlagSql + factorAuditFlagSql;
            string orderBy = "time.Tstamp desc";
            string groupBy = "PointId,Tstamp";
            string dataSql = string.Format("SELECT {0} FROM {1} WHERE {2} GROUP BY {3} ", fieldName, tableName, where, groupBy);
            string allHoursSql = string.Empty;
            DateTime AllHourdtEndNew = new DateTime();
            //由于实时数据通常晚1小时到数据库（离子色谱仪晚两个小时），补数据时间需要剔除当前小时及之后的小时
            if (LZSPYGuid.Equals(instrumentUid))
            {
              AllHourdtEndNew = Convert.ToDateTime(dtEnd.ToString("yyyy-MM-dd HH:00")) >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00")) ?
                 DateTime.Now.AddHours(-2) : dtEnd;
            }
            else
            {
              AllHourdtEndNew = Convert.ToDateTime(dtEnd.ToString("yyyy-MM-dd HH:00")) >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00")) ?
                  DateTime.Now.AddHours(-1) : dtEnd;
            }
            allHoursSql += string.Format(@"select top 24 time.PointId,time.Tstamp{0} from dbo.SY_F_GetAllDataByHour('{1}',',','{2}','{3}') time", factorFieldSql, "204", dtStart, AllHourdtEndNew);

            sql += string.Format(@"{0} left join ({1}) data on convert(varchar(13),time.tstamp,120)=convert(varchar(13),data.tstamp,120) and time.pointId=data.pointId ORDER BY {2}"
                , allHoursSql, dataSql, orderBy);
          }

          DataTable dt = CreatDataTable(sql, "AMS_AirAutoMonitorConnection");
          return dt;
        }
        else
        {

          DateTime dtEnd = Convert.ToDateTime(StTime.ToString("yyyy-MM-dd 23:59:59"));
          string factorFieldSql = string.Empty;
          string sql = string.Empty;
          if (listFactor.Contains("a05024"))
          {
            StringBuilder sb = new StringBuilder();
            sb.Append("select A.*,B.a05024,B.[a05024_Status],[a05024_DataFlag],B.[a05024_AuditFlag] from (select ");
            DataRow[] drs = dtFactor.Select("PollutantCode='a05024'");


            foreach (string factor in listFactor)
            {
              string factorFlag = string.Empty;
              string factorDataFlag = string.Empty;
              string factorAuditFlag = string.Empty;
              if (factor != "a05024")
              {
                factorFlag = factor + "_Status";
                factorDataFlag = factor + "_DataFlag";
                factorAuditFlag = factor + "_AuditFlag";
                DataRow[] dr = dtFactor.Select("PollutantCode='" + factor + "'");


                factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
                factorFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN Status END) AS [{1}] ", factor, factorFlag);
                factorDataFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN DataFlag END) AS [{1}] ", factor, factorDataFlag);
                factorAuditFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN AuditFlag END) AS [{1}] ", factor, factorAuditFlag);

                factorWhere += "'" + factor + "',";
              }
              factorFieldSql += "," + factor;
              factorFieldSql += "," + factor + "_Status";
              factorFieldSql += "," + factor + "_DataFlag";
              factorFieldSql += "," + factor + "_AuditFlag";
            }
            factorWhere = factorWhere + "'a05024'";

            pointWhere = " AND PointId=204";
            string where = string.Format("  Tstamp>='{0}' AND Tstamp<='{1}'  AND PollutantCode in ({2})", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + pointWhere;
            string sqlForO3 = string.Format(@"select PointId,Tstamp
                                                    ,MAX(CASE(PollutantCode) WHEN 'a05024' THEN PollutantValue END )  AS [a05024] 
                                                    ,MAX(CASE(PollutantCode) WHEN 'a05024' THEN Status END) AS [a05024_Status] 
                                                    ,MAX(CASE(PollutantCode) WHEN 'a05024' THEN DataFlag END) AS [a05024_DataFlag] 
                                                    ,MAX(CASE(PollutantCode) WHEN 'a05024' THEN AuditFlag END) AS [a05024_AuditFlag] 
                                                    from {0}  WHERE Tstamp>='{1}' AND Tstamp<='{2}' {3} group by PointId,Tstamp) B ", tableName, dtStart.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss"), pointWhere);

            string fieldName = "PointId,Tstamp" + factorSql + factorFlagSql + factorDataFlagSql + factorAuditFlagSql;
            string orderBy = "time.Tstamp desc";
            string groupBy = "PointId,Tstamp";
            sb.Append(fieldName + " from " + tableName + " where " + where + " group by " + groupBy + ") A left join (" + sqlForO3 + "on A.Tstamp = DATEADD(hour,1, B.Tstamp) and A.PointId=B.PointId  ");
            string dataSql = sb.ToString();
            string allHoursSql = string.Empty;
            //由于实时数据通常晚1小时到数据库（离子色谱仪晚两个小时），补数据时间需要剔除当前小时及之后的小时
            DateTime AllHourdtEndNew = Convert.ToDateTime(dtEnd.ToString("yyyy-MM-dd HH:00")) >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00")) ?
                DateTime.Now.AddHours(-1) : dtEnd;

            allHoursSql += string.Format(@"select time.PointId,time.Tstamp{0} from dbo.SY_F_GetAllDataByHour('{1}',',','{2}','{3}') time", factorFieldSql, "204", dtStart, AllHourdtEndNew);

            sql += string.Format(@"{0} left join ({1}) data on convert(varchar(13),time.tstamp,120)=convert(varchar(13),data.tstamp,120) and time.pointId=data.pointId ORDER BY {2}"
                , allHoursSql, dataSql, orderBy);
          }
          else
          {
            foreach (string factor in listFactor)
            {
              string factorFlag = factor + "_Status";
              string factorDataFlag = factor + "_DataFlag";
              string factorAuditFlag = factor + "_AuditFlag";
              DataRow[] dr = dtFactor.Select("PollutantCode='" + factor + "'");
              string factorName = dr[0]["PollutantName"].ToString();


              factorSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN PollutantValue END ) AS [{0}] ", factor);
              factorFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN Status END) AS [{1}] ", factor, factorFlag);
              factorDataFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN DataFlag END) AS [{1}] ", factor, factorDataFlag);
              factorAuditFlagSql += string.Format(",MAX(CASE(PollutantCode) WHEN '{0}' THEN AuditFlag END) AS [{1}] ", factor, factorAuditFlag);
              factorFieldSql += "," + factor;
              factorFieldSql += "," + factorFlag;
              factorFieldSql += "," + factorDataFlag;
              factorFieldSql += "," + factorAuditFlag;
              factorWhere += "'" + factor + "',";
            }
            if (factorWhere.Length > 0)
            {
              factorWhere = factorWhere.Remove(factorWhere.Length - 1, 1);
            }
            pointWhere = " AND PointId=204";
            string where = string.Format("  Tstamp>='{0}' AND Tstamp<='{1}'  AND PollutantCode in ({2})", dtStart.ToString("yyyy-MM-dd HH:mm:ss"), dtEnd.ToString("yyyy-MM-dd HH:mm:ss"), factorWhere) + pointWhere;
            string fieldName = "PointId,Tstamp" + factorSql + factorFlagSql + factorDataFlagSql + factorAuditFlagSql;
            string orderBy = "time.Tstamp desc";
            string groupBy = "PointId,Tstamp";
            string dataSql = string.Format("SELECT {0} FROM {1} WHERE {2} GROUP BY {3} ", fieldName, tableName, where, groupBy);
            string allHoursSql = string.Empty;
            DateTime AllHourdtEndNew = new DateTime();
            //由于实时数据通常晚1小时到数据库（离子色谱仪晚两个小时），补数据时间需要剔除当前小时及之后的小时
            if (LZSPYGuid.Equals(instrumentUid))
            {
              AllHourdtEndNew = Convert.ToDateTime(dtEnd.ToString("yyyy-MM-dd HH:00")) >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00")) ?
                 DateTime.Now.AddHours(-2) : dtEnd;
            }
            else
            {
              AllHourdtEndNew = Convert.ToDateTime(dtEnd.ToString("yyyy-MM-dd HH:00")) >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00")) ?
                  DateTime.Now.AddHours(-1) : dtEnd;
            }
            allHoursSql += string.Format(@"select time.PointId,time.Tstamp{0} from dbo.SY_F_GetAllDataByHour('{1}',',','{2}','{3}') time", factorFieldSql, "204", dtStart, AllHourdtEndNew);

            sql += string.Format(@"{0} left join ({1}) data on convert(varchar(13),time.tstamp,120)=convert(varchar(13),data.tstamp,120) and time.pointId=data.pointId ORDER BY {2}"
                , allHoursSql, dataSql, orderBy);
          }

          DataTable dt = CreatDataTable(sql, "AMS_AirAutoMonitorConnection");
          return dt;
        }
      }
      /// <summary>
      /// 获取因子的小数位
      /// </summary>
      /// <param name="factor"></param>
      /// <returns></returns>
      private int GetDecimalDigit(string factor)
      {
        int i = 3;
        string sql = "select decimaldigit from [Standard].[TB_PollutantCode] where PollutantCode='" + factor + "'";
        DataTable dt = CreatDataTable(sql, "AMS_BaseDataConnection");
        if (dt != null && dt.Rows.Count > 0)
        {
          string decimalStr = dt.Rows[0]["DecimalDigit"].ToString();
          if (!string.IsNullOrEmpty(decimalStr))
          {
            i = Convert.ToInt32(decimalStr);
          }
        }
        return i;
      }

     

     

        #region 通用方法
      /// <summary>
      /// 取得数据库中取得的因子浓度实际值
      /// 排除直接用银行家算法时因数据库小数位多、补零而导致的数据异常
      /// </summary>
      /// <param name="value">因子浓度</param>
      /// <param name="decimalNum">小数位</param>
      /// <returns></returns>
      public static decimal GetPollutantValue(decimal value, int decimalNum)
      {
        if (decimalNum < 0)
          return value;
        decimal valuePow = value * Convert.ToInt32(Math.Pow(10, decimalNum));
        if (valuePow - Convert.ToDecimal(Math.Floor(valuePow)) == 0M)
          return Math.Round(value, decimalNum);
        else
          return Math.Round(value, decimalNum, MidpointRounding.ToEven);
      }
      /// <summary>
      /// 将DataTable转成json字符串
      /// </summary>
      /// <param name="dt"></param>
       private string  DataTableToJson(DataTable dt)
       {
          StringBuilder jsonBuilder = new StringBuilder();
        jsonBuilder.Append("{\"");
        jsonBuilder.Append(dt.TableName);
        jsonBuilder.Append("\":[");
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            jsonBuilder.Append("{");
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                jsonBuilder.Append("\"");
                jsonBuilder.Append(dt.Columns[j].ColumnName);
                jsonBuilder.Append("\":\"");
                jsonBuilder.Append(dt.Rows[i][j].ToString());
                jsonBuilder.Append("\",");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("},");
        }
        if (dt != null && dt.Rows.Count > 0)
        {
          jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
        }
        jsonBuilder.Append("]");
        jsonBuilder.Append("}");
        return jsonBuilder.ToString();
       }
      /// <summary>
      /// DatTable按照一定的格式重新生成
      /// </summary>
      /// <param name="dts"></param>
      /// <returns></returns>
      private DataTable DataTableToFormat(DataTable dts,DataTable dtFactor)
      {
        DataTable dt = new DataTable();
        dt.Columns.Add("PointID",typeof(string));
        dt.Columns.Add("PointName", typeof(string));
        dt.Columns.Add("PollutantCode", typeof(string));
        dt.Columns.Add("PollutantName", typeof(string));
        dt.Columns.Add("PollutantValue", typeof(string));
        dt.Columns.Add("Date", typeof(DateTime));
        if (dtFactor != null && dtFactor.Rows.Count > 0)
        {
          for (int i = 0; i < dtFactor.Rows.Count; i++)
          {
            string PollutantCode = dtFactor.Rows[i]["factorcode"].ToString();
            string PollutantName = dtFactor.Rows[i]["factorName"].ToString();
            string MappingColumn = dtFactor.Rows[i]["mappingColumn"].ToString();
            if (dts != null && dts.Rows.Count > 0)
            {
              
              for (int j = 0; j < dts.Rows.Count; j++)
              {
                DataRow dr = dt.NewRow();
                dr["PointID"] = dts.Rows[j]["portId"].ToString();
                dr["PointName"] = dts.Rows[j]["monitoringPointName"].ToString();
                dr["PollutantCode"] = PollutantCode;
                dr["PollutantName"] = PollutantName;
                string num = dts.Rows[j][MappingColumn].ToString();
                if (!string.IsNullOrEmpty(num))
                {
                  dr["PollutantValue"] = GetPollutantValue(Convert.ToDecimal(dts.Rows[j][MappingColumn].ToString()), 4);
                }
                else
                {
                  dr["PollutantValue"] = num;
                }
                dr["Date"] = dts.Rows[j]["tstamp"].ToString();
                dt.Rows.Add(dr);
              }
            }
          }
        }
        return dt;
      }
      /// <summary>
      /// DatTable按照一定的格式重新生成
      /// </summary>
      /// <param name="dts"></param>
      /// <returns></returns>
      private DataTable DataTableToFormat2(DataTable dts, DataTable dtFactor)
      {
        DataTable dt = new DataTable();
        dt.Columns.Add("PointID", typeof(string));
        dt.Columns.Add("PointName", typeof(string));
        dt.Columns.Add("PointType", typeof(string));
        dt.Columns.Add("PollutantCode", typeof(string));
        dt.Columns.Add("PollutantName", typeof(string));
        dt.Columns.Add("PollutantValue", typeof(string));
        dt.Columns.Add("Date", typeof(DateTime));
        if (dtFactor != null && dtFactor.Rows.Count > 0)
        {
          for (int i = 0; i < dtFactor.Rows.Count; i++)
          {
            string PollutantCode = dtFactor.Rows[i]["factorcode"].ToString();
            string PollutantName = dtFactor.Rows[i]["factorName"].ToString();
            string MappingColumn = dtFactor.Rows[i]["mappingColumn"].ToString();
            
            if (dts != null && dts.Rows.Count > 0)
            {

              for (int j = 0; j < dts.Rows.Count; j++)
              {
                DataRow dr = dt.NewRow();
                dr["PointID"] = dts.Rows[j]["portId"].ToString();
                dr["PointName"] = dts.Rows[j]["monitoringPointName"].ToString();
                dr["PointType"] = dts.Rows[j]["PointType"].ToString();
                dr["PollutantCode"] = PollutantCode;
                dr["PollutantName"] = PollutantName;
                string num = dts.Rows[j][MappingColumn].ToString();
                if (!string.IsNullOrEmpty(num))
                {
                  dr["PollutantValue"] = GetPollutantValue(Convert.ToDecimal(dts.Rows[j][MappingColumn].ToString()), 4);
                }
                else
                {
                  dr["PollutantValue"] = num;
                }
                dr["Date"] = dts.Rows[j]["tstamp"].ToString();
                dt.Rows.Add(dr);
              }
            }
          }
        }
        return dt;
      }
      /// <summary>
      /// 获取两个数组中的交集
      /// </summary>
      /// <param name="pointIds"></param>
      /// <param name="pointAfter"></param>
      /// <returns></returns>
      private string[] GetInfection(string[] ary1, string[] ary2)
      {
        List<string> list=new List<string>();
        if (ary1 != null && ary2 != null)
        {
          List<string> list1 = ary1.ToList<string>();
          List<string> list2 = ary2.ToList<string>();
          foreach (string em1 in list1)
          {
            if (!string.IsNullOrEmpty(em1) && list2.Contains(em1))
            {
              list.Add(em1);
            }
          }
        }
        return list.ToArray();
      }
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
      /// <summary>
      /// 有错误时返回的json字符串
      /// </summary>
      /// <returns></returns>
      private string ReturnErrorStr()
      {
        string errorStr = string.Empty;
        errorStr = "{\"Message\":[{\"IsSuccess\":\"false\",\"Reason\":\"调用接口失败\"}]}";
        return errorStr;
      }
      /// <summary>
      /// 有错误时返回的json字符串
      /// </summary>
      /// <returns></returns>
      private string ReturnErrorStr(string msg)
      {
        string errorStr = string.Empty;
        errorStr = "{\"Message\":[{\"IsSuccess\":\"false\",\"Reason\":\""+msg+"\"}]}";
        return errorStr;
      }
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
        /// 获取数据集
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <param name="strConnectString">数据库链接字符串</param>
        /// <returns></returns>
        public DataTable CreatDataTable(String strSql, String strConnectString)
        {

          using (SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings[strConnectString].ConnectionString))
          {
            using (SqlDataAdapter myCommand = new SqlDataAdapter(strSql, myConn))
            {
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
                return dt;
              }
              catch (Exception ex)
              {
                return null;
              }
              finally
              {
                myConn.Close();
                myConn.Dispose();
              }
            }
          }
        }
        /// <summary>  
        /// 写入日志到文本文件  
        /// </summary>  
        /// <param name="action">动作</param>  
        /// <param name="strMessage">日志内容</param>  
        /// <param name="time">时间</param>  
        public static void WriteTextLog(string action, string strMessage, DateTime time)
        {
          string path = AppDomain.CurrentDomain.BaseDirectory + @"System\Log\";
          if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

          string fileFullPath = path + time.ToString("yyyy-MM-dd") + ".System.txt";
          StringBuilder str = new StringBuilder();
          str.Append("Time:    " + time.ToString() + "\r\n");
          str.Append("Action:  " + action + "\r\n");
          str.Append("Message: " + strMessage + "\r\n");
          str.Append("-----------------------------------------------------------\r\n");
          StreamWriter sw;
          if (!File.Exists(fileFullPath))
          {
            sw = File.CreateText(fileFullPath);
          }
          else
          {
            sw = File.AppendText(fileFullPath);
          }
          sw.WriteLine(str.ToString());
          sw.Close();
        }
        /// <summary>
        /// 返回一个dataview，带参数
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="strConName"></param>
        /// <param name="prams"></param>
        /// <returns></returns>
        public DataView RunProcDataView(string procName, String strConName, SqlParameter[] prams = null)
        {
            using (SqlConnection myConn = new SqlConnection(GetSqlConnection(strConName)))
            {
                using (SqlDataAdapter myCommand = new SqlDataAdapter(procName, myConn))
                {
                    myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;
                    PopulateProcParams(myCommand, prams);
                    if (ConfigurationManager.AppSettings["SqlCmdTimeOut"] != null && ConfigurationManager.AppSettings["SqlCmdTimeOut"].ToString() != "")
                    {
                        myCommand.SelectCommand.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["SqlCmdTimeOut"]);
                    }
                    else
                    {
                        myCommand.SelectCommand.CommandTimeout = 600;
                    }
                    try
                    {
                        DataSet ds = new DataSet();
                        myCommand.Fill(ds, "mytable");
                        DataView mydataview = ds.Tables["mytable"].DefaultView;
                        return mydataview;
                    }
                    catch (Exception e)
                    {
                        throw new Exception("执行脚本命令的时候产生了一个错误-> " + e.ToString());
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

        /// <summary>
        /// 创建存储过程DataAdapter
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="strConName"></param>
        /// <param name="prams"></param>
        /// <returns></returns>
        private SqlDataAdapter PopulateProcParams(SqlDataAdapter myCommand, SqlParameter[] prams)
        {
            //SqlConnection myConn = new SqlConnection(GetSqlConnection(strConName));
            //SqlDataAdapter cmd = new SqlDataAdapter(procName, myConn);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            // add proc parameters
            if (prams != null)
            {
                foreach (SqlParameter parameter in prams)
                    myCommand.SelectCommand.Parameters.Add(parameter);
            }

            // return param
            myCommand.SelectCommand.Parameters.Add(
                new SqlParameter("ReturnValue", SqlDbType.Int, 4,
                ParameterDirection.ReturnValue, false, 0, 0,
                string.Empty, DataRowVersion.Default, null));

            return myCommand;
        }
        /// <summary>
        /// 获取文件夹内指定时间段内的子文件下的文件
        /// </summary>
        /// <param name="fullpath">文件绝对路径</param>
        /// <param name="startdate">开始日期</param>
        /// <param name="enddate">截止日期</param>
        /// <returns></returns>
        public string[] GetChildFilesList(string fullpath, DateTime dtStart, DateTime dtEnd)
        {
          
          var query = from a in
                        (from f in Directory.GetDirectories(fullpath)
                         let fi = new FileInfo(f)
                         where Convert.ToDateTime(fi.Name) >= dtStart && Convert.ToDateTime(fi.Name) < dtEnd
                         //orderby GetDate(fi.Name) descending
                         select fi.Name)
                      from mm in Directory.GetFiles(fullpath + "/" + a)
                      let mfi = new FileInfo(mm)
                      where IsOrNotImg(mfi.Name) 
                      orderby GetCityDate(mfi.Name.Substring(2, 19)) descending
                      select  a + "/" + mfi.Name
                         ;
          return query.ToArray();
        }

        private DateTime GetCityDate(string str)
        {
          DateTime dt = DateTime.Parse("1900-01-01 00:00:00");
          if (str.Length == 19)
          {
            //IFormatProvider format = new System.Globalization.CultureInfo("zh-CN");
            string FormatStr = "yyyy-MM-dd HH-mm-ss";
            dt = DateTime.ParseExact(str, FormatStr, CultureInfo.InvariantCulture);
          }
          return dt;
          
         
        }
        protected bool IsOrNotImg(string str)
        {
          if (str.ToLower().Contains("bmp") || str.ToLower().Contains("jpeg") || str.ToLower().Contains("jpg") || str.ToLower().Contains("png") || str.ToLower().Contains("svg"))
          {
            return true;
          }
          else
          {
            return false;
          }
        }
        /// <summary>
        /// 取得连接字符串
        /// </summary>
        /// <param name="appSettingsName"></param>
        /// <returns></returns>
        private string GetSqlConnection(string appSettingsName)
        {
            return ConfigurationManager.ConnectionStrings[appSettingsName].ConnectionString;
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
        /// <param name="array">集合对象</param>   
        /// <returns>Json字符串</returns>   
        public static string ToArrayStringNew(String[] array)
        {
            StringBuilder sb = new StringBuilder();
            string jsonString = "[";
            sb.Append(jsonString);
            for (int i = 0; i < array.Length; i++)
            {
                if (i == 0)
                {
                    sb.Append(array[i]);
                    //jsonString += array[i];
                }
                else
                {
                    sb.Append("," + array[i]);
                    //jsonString += "," + array[i];
                }

            }
            return sb.ToString() + "]";
        }

        #endregion
        #region 暂未使用
        //[WebMethod(Description = " [图表]获取空气日报图表（日期：yyyy-MM-DD）")]
        //public string GetAQIPieByDate(string siteType, string date)
        //{
        //      string result = string.Empty;
        //    result = "[{\"PortAQIPieByDate\":[";
        //    switch (siteType)
        //    {
        //        case "ALL":
        //            result += "{\"PieName\":\"优\",\"PieValue\":\"40\"}";
        //            result += ",{\"PieName\":\"良\",\"PieValue\":\"40\"}";
        //            result += ",{\"PieName\":\"中度污染\",\"PieValue\":\"20\"}";
        //            break;
        //        case "City":
        //             result += "{\"PieName\":\"优\",\"PieValue\":\"66.67\"}";
        //            result += ",{\"PieName\":\"中度污染\",\"PieValue\":\"33.33\"}";
        //            break;
        //        case "District":
        //            result += "{\"PieName\":\"良\",\"PieValue\":\"100\"}";
        //            break;
        //        case "Town":
        //            result += "{\"PieName\":\"良\",\"PieValue\":\"100\"}";
        //            break;
        //        default:
        //             result += "{\"PieName\":\"优\",\"PieValue\":\"40\"}";
        //            result += ",{\"PieName\":\"良\",\"PieValue\":\"40\"}";
        //            result += ",{\"PieName\":\"中度污染\",\"PieValue\":\"20\"}";
        //            break;
        //    }

        //    result += "]}]";
        //    return result;
        //}

        //[WebMethod(Description = " [API查询（昆山）]获取站点空气质量日报API")]
        //public string GetPortAQIByDate(string siteType, string date, string classes)
        //{
        //    string result = string.Empty;
        //    string sql = " SELECT a.id ,a.monitoringPointName,null as  value, ";
        //    sql += " CEILING(AVG( cast(AQIValue as int))) as AQIValue, ";
        //    sql += " dbo.F_GetAPI_Grade(CEILING(AVG(cast(AQIValue as int))),'Grade') as 'Grade', ";
        //    sql += " dbo.F_GetAPI_Grade(CEILING(AVG(cast(AQIValue as int))),'Class') as 'Class', ";
        //    sql += "  dbo.[F_GetAPI_Max_CNV](CEILING(AVG( cast(SO2_IAQI as int))),CEILING(AVG( cast(NO2_IAQI as int))),CEILING(AVG( cast(PM10_IAQI as int))),-99999,-99999,'S') as 'MainPollute' ";
        //    sql += " FROM STA_MonitoringPoint a inner join  dbo.SY_View_CodeMainItem c on a.siteTypeCode=c.ItemGuid LEFT OUTER JOIN RTD_HourAQI b ON a.id = b.portId ";
        //    if (siteType !="City")
        //    sql += " and ItemValue = '" + siteType + "'";
        //    sql += " AND  datediff(dd,[dateTime],'"+date+"')=0 ";
        //    sql += "  group by a.id,a.monitoringPointName  ";

        //    DataView dv = CreatDataView(sql, "ConnStrEqmsAir");
        //    if (dv.Table.Rows.Count > 0)
        //    {
        //        DataView newDV = new DataView(dv.Table);
        //        if (classes != "")
        //            newDV.RowFilter = " Class like '%" + classes + "%'";
        //        String[] arr = new string[newDV.ToTable().Rows.Count];

        //        for (int i = 0; i < newDV.ToTable().Rows.Count; i++)
        //        {
        //            string portId = string.Empty, portName = string.Empty;
        //            string factorValue = string.Empty, AQI = string.Empty;
        //            string level = string.Empty, state = string.Empty;
        //            string mainPollute = string.Empty;

        //            if (!Convert.IsDBNull(newDV.ToTable().Rows[i][0]))
        //                portId = newDV.ToTable().Rows[i][0].ToString();
        //            else portId = "--";
        //            if (!Convert.IsDBNull(newDV.ToTable().Rows[i][1]))
        //                portName = newDV.ToTable().Rows[i][1].ToString();
        //            else portName = "--";
        //            if (!Convert.IsDBNull(newDV.ToTable().Rows[i][2]))
        //                factorValue = newDV.ToTable().Rows[i][2].ToString();
        //            else factorValue = "--";
        //            if (!Convert.IsDBNull(newDV.ToTable().Rows[i][3]))
        //                AQI = newDV.ToTable().Rows[i][3].ToString();
        //            else AQI="--";
        //            if (!Convert.IsDBNull(newDV.ToTable().Rows[i][4]))
        //                level = newDV.ToTable().Rows[i][4].ToString();
        //            else level="--";
        //            if (!Convert.IsDBNull(newDV.ToTable().Rows[i][5]))
        //                state = newDV.ToTable().Rows[i][5].ToString();
        //            else state="--";
        //            if (!Convert.IsDBNull(newDV.ToTable().Rows[i][6]))
        //                mainPollute = newDV.ToTable().Rows[i][6].ToString();
        //            else mainPollute="--";
        //            arr[i] = "{\"PortId\":\""+portId+"\",\"PortName\":\""+portName+"\",\"FactorValue\":\""+factorValue+"\",\"AQI\":\"" +AQI+"\",\"Level\":\""+level+"\",\"State\":\""+state +"\",\"MainPollute\":\""+mainPollute + "\"}";
        //        }
        //        result = "[{\"PortAQIByDate\":" + ToArrayStringNew(arr) + "}]";
        //    }
        //    return result;

        //}

        //[WebMethod(Description = " [新]获取站点水质周报")]
        //public string GetPortWaterQualityByWeek(string siteType, string year, string week)
        //{
        //    string result = string.Empty;
        //    result = "[{\"PortWeekWaterQuality\":[";
        //    switch (siteType)
        //    {
        //        case "City":
        //            result += "{\"PortId\":\"31\",\"PortName\":\"张家港三水厂\",\"Level\":\"III类\",\"MainPollute\":\"总磷\"}";
        //            result += ",{\"PortId\":\"32\",\"PortName\":\"常熟三水厂\",\"Level\":\"II类\",\"MainPollute\":\"总磷\"}";
        //            result += ",{\"PortId\":\"33\",\"PortName\":\"昆山二水厂\",\"Level\":\"I类\",\"MainPollute\":\"高锰酸盐指数\"}";
        //            result += ",{\"PortId\":\"35\",\"PortName\":\"吴江净水厂\",\"Level\":\"IV类\",\"MainPollute\":\"氨氮\"}";
        //            break;
        //        case "District":
        //            result += "{\"PortId\":\"36\",\"PortName\":\"市区望亭\",\"Level\":\"III类\",\"MainPollute\":\"总氮\"}";
        //            break;
        //        case "Town":
        //            result += "{\"PortId\":\"34\",\"PortName\":\"太仓二水厂\",\"Level\":\"III类\",\"MainPollute\":\"总磷\"}";
        //            break;
        //        default:
        //            result += "{\"PortId\":\"31\",\"PortName\":\"张家港三水厂\",\"Level\":\"III类\",\"MainPollute\":\"总磷\"}";
        //            result += ",{\"PortId\":\"32\",\"PortName\":\"常熟三水厂\",\"Level\":\"II类\",\"MainPollute\":\"总磷\"}";
        //            result += ",{\"PortId\":\"33\",\"PortName\":\"昆山二水厂\",\"Level\":\"I类\",\"MainPollute\":\"高锰酸盐指数\"}";
        //            result += ",{\"PortId\":\"34\",\"PortName\":\"太仓二水厂\",\"Level\":\"III类\",\"MainPollute\":\"总磷\"}";
        //            result += ",{\"PortId\":\"35\",\"PortName\":\"吴江净水厂\",\"Level\":\"IV类\",\"MainPollute\":\"氨氮\"}";
        //            result += ",{\"PortId\":\"36\",\"PortName\":\"市区望亭\",\"Level\":\"III类\",\"MainPollute\":\"总氮\"}";
        //            break;

        //    }
        //    result += "]}]";
        //    return result;
        //}


        //[WebMethod(Description = " [24小时AQI]根据站点类型获取某个监测因子的最新24小时AQI")]
        //public string GetPortAQIBy24HoursAndSiteType(string siteType)
        //{
        //    string result = string.Empty;

        //    string sql = " SELECT a.id ,a.monitoringPointName,null as  value, ";
        //    sql += " CEILING(AVG( cast(AQIValue as int))) as AQIValue, ";
        //    sql += " dbo.F_GetAPI_Grade(CEILING(AVG(cast(AQIValue as int))),'Grade') as 'Grade', ";
        //    sql += " dbo.F_GetAPI_Grade(CEILING(AVG(cast(AQIValue as int))),'Class') as 'Class' ";
        //    sql += " FROM STA_MonitoringPoint a inner join  dbo.SY_View_CodeMainItem c on a.siteTypeCode=c.ItemGuid LEFT OUTER JOIN RTD_HourAQI b ON a.id = b.portId ";

        //    sql += " AND  [dateTime]>=DATEADD(hour,-25,getdate())  AND  [dateTime]<=DATEADD(hour,-1,getdate())  ";
        //    if (siteType != "City")
        //        sql += " where  ItemValue = '" + siteType + "'";
        //    sql += "  group by a.id,a.monitoringPointName  ";

        //    DataView dv = CreatDataView(sql, "ConnStrEqmsAir");
        //    if (dv.Table.Rows.Count > 0)
        //    {
        //        String[] arr = new string[dv.Table.Rows.Count];
        //        for (int i = 0; i < dv.Table.Rows.Count; i++)
        //        {
        //            string portId = string.Empty, portName = string.Empty;
        //            string factorValue = string.Empty, AQI = string.Empty;
        //            string level = string.Empty, state = string.Empty;

        //            if (!Convert.IsDBNull(dv.Table.Rows[i][0]))
        //                portId = dv.Table.Rows[i][0].ToString();
        //            else portId = "--";
        //            if (!Convert.IsDBNull(dv.Table.Rows[i][1]))
        //                portName = dv.Table.Rows[i][1].ToString();
        //            else portName = "--";
        //            if (!Convert.IsDBNull(dv.Table.Rows[i][2]))
        //                factorValue = dv.Table.Rows[i][2].ToString();
        //            else factorValue = "--";
        //            if (!Convert.IsDBNull(dv.Table.Rows[i][3]))
        //            {
        //                if (dv.Table.Rows[i][3].ToString() != "-99999")
        //                    AQI = dv.Table.Rows[i][3].ToString();
        //                else AQI = "--";

        //            }
        //            else AQI = "--";
        //            if (!Convert.IsDBNull(dv.Table.Rows[i][4]))
        //                level = dv.Table.Rows[i][4].ToString();
        //            else level = "--";
        //            if (!Convert.IsDBNull(dv.Table.Rows[i][5]))
        //                state = dv.Table.Rows[i][5].ToString();
        //            else state = "--";

        //            arr[i] = "{\"PortId\":\"" + portId + "\",\"PortName\":\"" + portName + "\",\"FactorValue\":\"" + factorValue + "\",\"AQI\":\"" + AQI + "\",\"Level\":\"" + level + "\",\"State\":\"" + state + "\"}";
        //        }
        //        result = "[{\"PortAQI24HoursBySiteType\":" + ToArrayStringNew(arr) + "}]";
        //    }
        //    return result;
        //}

        //[WebMethod(Description = " [新]获取站点水质日报")]
        //public string GetPortWaterQualityByDate(string siteType, string fromDate, string toDate, string classes)
        //{
        //    switch (classes)
        //    {
        //        case "I": classes = "Ⅰ";
        //            break;
        //        case "II": classes = "Ⅱ";
        //            break;
        //        case "III": classes = "Ⅲ";
        //            break;
        //        case "IV": classes = "Ⅳ";
        //            break;
        //        case "V": classes = "Ⅴ";
        //            break;

        //    }

        //    string result = string.Empty;
        //    //result = "[{\"PortDayWaterQuality\":[";

        //    string sql = " select b.id as portid ,b.monitoringPointName,CONVERT(varchar(10),tstamp,120) as tstamp, ";
        //    sql += " MAX_Pollute,MAX_WQ from RPT_DayWaterQuality as a ";
        //    sql += " inner join dbo.V_Point as b on a.portid=b.id ";
        //    sql += " where b.siteTypeCode='" + siteType + "' ";
        //    sql += " and tstamp>='" + fromDate + "' and tstamp <='" + toDate + "' ";
        //    sql += " order by portid,tstamp desc ";

        //    DataView dv = CreatDataView(sql, "ConnStrEqmsWater");
        //    if (dv.Table.Rows.Count > 0)
        //    {
        //        string portid = string.Empty;
        //        string portName = string.Empty, mainPollute = string.Empty;
        //        string classStr = string.Empty, date = string.Empty;
        //        DataView newDV = new DataView(dv.Table);
        //        if (classes != "")
        //            newDV.RowFilter = " MAX_WQ ='" + classes + "'";

        //        if (newDV.Table.Rows.Count > 0)
        //        {
        //            String[] arr = new string[newDV.ToTable().Rows.Count];

        //            for (int i = 0; i < newDV.ToTable().Rows.Count; i++)
        //            {
        //                if (!Convert.IsDBNull(newDV.ToTable().Rows[i][0]))
        //                    portid = newDV.ToTable().Rows[i][0].ToString();
        //                else portid = "--";
        //                if (!Convert.IsDBNull(newDV.ToTable().Rows[i][1]))
        //                    portName = newDV.ToTable().Rows[i][1].ToString();
        //                else portName = "--";
        //                if (!Convert.IsDBNull(newDV.ToTable().Rows[i][2]))
        //                    date = newDV.ToTable().Rows[i][2].ToString();
        //                else date = "--";
        //                if (!Convert.IsDBNull(newDV.ToTable().Rows[i][3]))
        //                    mainPollute = newDV.ToTable().Rows[i][3].ToString().Trim(';');
        //                else mainPollute = "--";
        //                if (!Convert.IsDBNull(newDV.ToTable().Rows[i][4]))
        //                    classStr = newDV.ToTable().Rows[i][4].ToString();
        //                else classStr = "--";
        //                arr[i] = "{\"PortId\":\"" + portid + "\",\"PortName\":\"" + portName + "\",\"MonitorTime\":\"" + date + "\",\"Level\":\"" + classStr + "\",\"MainPollute\":\"" + mainPollute + "\"}";
        //            }
        //            result = "[{\"PortDayWaterQuality\":" + ToArrayStringNew(arr) + "}]";
        //        }

        //    }
        //    return result;
        //}





        #endregion

    }
}
