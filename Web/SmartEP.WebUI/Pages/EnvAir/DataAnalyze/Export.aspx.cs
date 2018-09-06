using Aspose.Cells;
using Aspose.Words;
using Aspose.Words.Tables;
using log4net;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.BaseData;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.Core.Enums;
using SmartEP.Service.DataAnalyze.Air.AQIReport;
using SmartEP.Service.DataAnalyze.Air.DataQuery;
using SmartEP.Service.DataAnalyze.Report;
using SmartEP.Service.Frame;
using SmartEP.Utilities.AdoData;
using SmartEP.Utilities.Office;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Microsoft.CSharp;
using ICSharpCode.SharpZipLib.Zip;
using System.Text;

//using ICSharpCode.SharpZipLib.Zip;

namespace SmartEP.WebUI.Pages.EnvAir.DataAnalyze
{
    public partial class Export : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 选择因子
        /// </summary>
        private IList<IPollutant> factors = null;
        /// <summary>
        /// 数据库处理类
        /// </summary>
        DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string MonitoringBusinessConnection = "AMS_MonitoringBusinessConnection";
        /// <summary>
        /// 总站报表模板路径
        /// </summary>
        private string path = System.Configuration.ConfigurationManager.AppSettings["ExcelKey"];
        /// <summary>
        /// 总站报表模板路径
        /// </summary>
        private string strTarget = System.Configuration.ConfigurationManager.AppSettings["ExcelRord"];
        /// <summary>
        /// 选择站点
        /// </summary>
        private IList<IPoint> points = null;
        ILog log = LogManager.GetLogger("FileLogging");//获取一个日志记录器

        static List<string[]> files;
        static string root = "";
        /// <summary>
        /// 所有空目录缓存
        /// </summary>
        static List<string[]> paths;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitControl();
            }
        }



        #region 初始化控件
        private void InitControl()
        {

        }
        #endregion

        #region 事件
        protected void auditLogGrid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }

        /// <summary>
        /// 查询按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //string path = "";
            string filename = "";
            Document doc =new Document();
            DataTable dt = new DataTable();
            string tableName1 = "dbo.TB_Test_ActualSampleCompare";
            string tableName2 = "dbo.TB_Test_SamplesCheck";
            string tableName3 = "dbo.TB_Test_ZeroPointDrift";
            string tableName4 = "dbo.TB_Test_RangeCorrectionCheck";
            string tableBase = "[AMS_BaseDataZZ].[MPInfo].[TB_MonitoringPoint]";
            foreach (Object j in auditLogGrid.SelectedIndexes)
            {
                int i = Convert.ToInt32(j);
                if (i == 0)
                {
                    string sql = string.Format("select Tstamp,CodMonitorValue AS MonitorValue,CodStandardValue AS StandardValue,CodRelativeError AS RelativeError,b.MonitoringPointName as PointId from {0} a left join {1} b on a.PointId=b.PointId", tableName1, tableBase);//,CodQualified AS Qualified
                    dt = g_DatabaseHelper.ExecuteDataTable(sql, MonitoringBusinessConnection);
                    doc = new Document(System.IO.Path.Combine(path, "高锰酸盐指数实际水样比对测试结果表.docx"));
                    filename = "高锰酸盐指数实际水样比对测试结果表.docx";
                    string update = string.Format("update [dbo].[Export] set State='生成中' where ImpName='高锰酸盐指数实际水样比对测试结果表'");
                    g_DatabaseHelper.ExecuteNonQuery(update, MonitoringBusinessConnection);
                }
                if (i == 1)
                {
                    string sql = string.Format("select Tstamp,NHMonitorValue AS MonitorValue,NHStandardValue AS StandardValue,NHRelativeError AS RelativeError,b.MonitoringPointName as PointId from {0} a left join {1} b on a.PointId=b.PointId", tableName1, tableBase);//,NHQualified AS Qualified
                    dt = g_DatabaseHelper.ExecuteDataTable(sql, MonitoringBusinessConnection);
                    doc = new Document(System.IO.Path.Combine(path, "氨氮实际水样比对测试结果表.docx"));
                    filename = "氨氮实际水样比对测试结果表.docx";
                    string update = string.Format("update [dbo].[Export] set State='生成中' where ImpName='氨氮实际水样比对测试结果表'");
                    g_DatabaseHelper.ExecuteNonQuery(update, MonitoringBusinessConnection);
                }
                if (i == 2)
                {
                    string sql = string.Format("select Tstamp,TPMonitorValue AS MonitorValue,TPStandardValue AS StandardValue,TPRelativeError AS RelativeError,b.MonitoringPointName as PointId from {0} a left join {1} b on a.PointId=b.PointId", tableName1, tableBase);//,TPQualified AS Qualified
                    dt = g_DatabaseHelper.ExecuteDataTable(sql, MonitoringBusinessConnection);
                    doc = new Document(System.IO.Path.Combine(path, "总磷实际水样比对测试结果表.docx"));
                    filename = "总磷实际水样比对测试结果表.docx";
                    string update = string.Format("update [dbo].[Export] set State='生成中' where ImpName='总磷实际水样比对测试结果表'");
                    g_DatabaseHelper.ExecuteNonQuery(update, MonitoringBusinessConnection);
                }
                if (i == 3)
                {
                    string sql = string.Format("select Tstamp,TNMonitorValue AS MonitorValue,TNStandardValue AS StandardValue,TNRelativeError AS RelativeError,b.MonitoringPointName as PointId from {0} a left join {1} b on a.PointId=b.PointId", tableName1, tableBase);//,TNQualified AS Qualified
                    dt = g_DatabaseHelper.ExecuteDataTable(sql, MonitoringBusinessConnection);
                    doc = new Document(System.IO.Path.Combine(path, "总氮实际水样比对测试结果表.docx"));
                    filename = "总氮实际水样比对测试结果表.docx";
                    string update = string.Format("update [dbo].[Export] set State='生成中' where ImpName='总氮实际水样比对测试结果表'");
                    g_DatabaseHelper.ExecuteNonQuery(update, MonitoringBusinessConnection);
                }
                if (i == 4)
                {
                    string sql = string.Format("select PointName as PointId,ReportDateTime,PHMonitorValue AS MonitorValue,PHSampleValue AS StandardValue,PHError AS RelativeError from {0}", tableName2);//,CodQualified AS Qualified
                    dt = g_DatabaseHelper.ExecuteDataTable(sql, MonitoringBusinessConnection);
                    doc = new Document(System.IO.Path.Combine(path, "pH样品核查测试结果表.docx"));
                    filename = "pH样品核查测试结果表.docx";
                    string update = string.Format("update [dbo].[Export] set State='生成中' where ImpName='pH样品核查测试结果表'");
                    g_DatabaseHelper.ExecuteNonQuery(update, MonitoringBusinessConnection);
                }
                if (i == 5)
                {
                    string sql = string.Format("select PointName as PointId,ReportDateTime,DOMonitorValue AS MonitorValue,DOSampleValue AS StandardValue,DOError AS RelativeError from {0}", tableName2);//,NHQualified AS Qualified
                    dt = g_DatabaseHelper.ExecuteDataTable(sql, MonitoringBusinessConnection);
                    doc = new Document(System.IO.Path.Combine(path, "溶解氧样品核查测试结果表.docx"));
                    filename = "溶解氧样品核查测试结果表.docx";
                    string update = string.Format("update [dbo].[Export] set State='生成中' where ImpName='溶解氧样品核查测试结果表'");
                    g_DatabaseHelper.ExecuteNonQuery(update, MonitoringBusinessConnection);
                }
                if (i == 6)
                {
                    string sql = string.Format("select PointName as PointId,ReportDateTime,ECMonitorValue AS MonitorValue,ECSampleValue AS StandardValue,ECError AS RelativeError from {0}", tableName2);//,TPQualified AS Qualified
                    dt = g_DatabaseHelper.ExecuteDataTable(sql, MonitoringBusinessConnection);
                    doc = new Document(System.IO.Path.Combine(path, "电导率样品核查测试结果表.docx"));
                    filename = "电导率样品核查测试结果表.docx";
                    string update = string.Format("update [dbo].[Export] set State='生成中' where ImpName='电导率样品核查测试结果表'");
                    g_DatabaseHelper.ExecuteNonQuery(update, MonitoringBusinessConnection);
                }
                if (i == 7)
                {
                    string sql = string.Format("select PointName as PointId,ReportDateTime,TBMonitorValue AS MonitorValue,TBSampleValue AS StandardValue,TBError AS RelativeError from {0}", tableName2);//,TNQualified AS Qualified
                    dt = g_DatabaseHelper.ExecuteDataTable(sql, MonitoringBusinessConnection);
                    doc = new Document(System.IO.Path.Combine(path, "浊度样品核查测试结果表.docx"));
                    filename = "浊度样品核查测试结果表.docx";
                    string update = string.Format("update [dbo].[Export] set State='生成中' where ImpName='浊度样品核查测试结果表'");
                    g_DatabaseHelper.ExecuteNonQuery(update, MonitoringBusinessConnection);
                }
                if (i == 8)
                {
                    string sql = string.Format(@"select c.MonitoringPointName as PointId,CONVERT(varchar(100), a.Tstamp, 111) as Tstamp,a.[CodMonitorValue] as ZMonitorValue
      ,[CodStandardValue] as ZStandardValue
      ,[CodAbsoluteError] as ZAbsoluteError
	  ,b.[CodMonitorValue] as LMonitorValue
      ,[CodStandValue] as LStandValue
      ,[CodRelativeErrorValue] as LErrorValue
from {0} a
left join {1} b
on a.PointId=b.PointId and a.Tstamp =b.Tstamp left join {2} c on a.PointId=c.PointId
order by PointId,Tstamp", tableName3,tableName4,tableBase);//,CodQualified AS Qualified
                    dt = g_DatabaseHelper.ExecuteDataTable(sql, MonitoringBusinessConnection);
                    doc = new Document(System.IO.Path.Combine(path, "高锰酸盐指数零点和量程校正液核查测定结果表.docx"));
                    filename = "高锰酸盐指数零点和量程校正液核查测定结果表.docx";
                    string update = string.Format("update [dbo].[Export] set State='生成中' where ImpName='高锰酸盐指数零点和量程校正液核查测定结果表'");
                    g_DatabaseHelper.ExecuteNonQuery(update, MonitoringBusinessConnection);
                }
                if (i == 9)
                {
                    string sql = string.Format(@"select c.MonitoringPointName as PointId,CONVERT(varchar(100), a.Tstamp, 111) as Tstamp,a.[NHMonitorValue] as ZMonitorValue
      ,[NHStandardValue] as ZStandardValue
      ,[NHAbsoluteError] as ZAbsoluteError
	  ,b.[NHMonitorValue] as LMonitorValue
      ,[NHStandValue] as LStandValue
      ,[NHRelativeErrorValue] as LErrorValue
from {0} a
left join {1} b
on a.PointId=b.PointId and a.Tstamp =b.Tstamp left join {2} c on a.PointId=c.PointId
order by PointId,Tstamp", tableName3, tableName4,tableBase);
                    dt = g_DatabaseHelper.ExecuteDataTable(sql, MonitoringBusinessConnection);
                    doc = new Document(System.IO.Path.Combine(path, "氨氮零点和量程校正液核查测定结果表.docx"));
                    filename = "氨氮零点和量程校正液核查测定结果表.docx";
                    string update = string.Format("update [dbo].[Export] set State='生成中' where ImpName='氨氮零点和量程校正液核查测定结果表'");
                    g_DatabaseHelper.ExecuteNonQuery(update, MonitoringBusinessConnection);
                }
                if (i == 10)
                {
                    string sql = string.Format(@"select c.MonitoringPointName as PointId,CONVERT(varchar(100), a.Tstamp, 111) as Tstamp,a.[TPMonitorValue] as ZMonitorValue
      ,[TPStandardValue] as ZStandardValue
      ,[TPAbsoluteError] as ZAbsoluteError
	  ,b.[TPMonitorValue] as LMonitorValue
      ,[TPStandValue] as LStandValue
      ,[TPRelativeErrorValue] as LErrorValue
from {0} a
left join {1} b
on a.PointId=b.PointId and a.Tstamp =b.Tstamp left join {2} c on a.PointId=c.PointId
order by PointId,Tstamp", tableName3, tableName4,tableBase); 
                    dt = g_DatabaseHelper.ExecuteDataTable(sql, MonitoringBusinessConnection);
                    doc = new Document(System.IO.Path.Combine(path, "总磷零点和量程校正液核查测定结果表.docx"));
                    filename = "总磷零点和量程校正液核查测定结果表.docx";
                    string update = string.Format("update [dbo].[Export] set State='生成中' where ImpName='总磷零点和量程校正液核查测定结果表'");
                    g_DatabaseHelper.ExecuteNonQuery(update, MonitoringBusinessConnection);
                }
                if (i == 11)
                {
                    string sql = string.Format(@"select c.MonitoringPointName as PointId,CONVERT(varchar(100), a.Tstamp, 111) as Tstamp,a.[TNMonitorValue] as ZMonitorValue
      ,[TNStandardValue] as ZStandardValue
      ,[TNAbsoluteError] as ZAbsoluteError
	  ,b.[TNMonitorValue] as LMonitorValue
      ,[TNStandValue] as LStandValue
      ,[TNRelativeErrorValue] as LErrorValue
from {0} a
left join {1} b
on a.PointId=b.PointId and a.Tstamp =b.Tstamp left join {2} c on a.PointId=c.PointId
order by PointId,Tstamp", tableName3, tableName4,tableBase); 
                    dt = g_DatabaseHelper.ExecuteDataTable(sql, MonitoringBusinessConnection);
                    doc = new Document(System.IO.Path.Combine(path, "总氮零点和量程校正液核查测定结果表.docx"));
                    filename = "总氮零点和量程校正液核查测定结果表.docx";
                    string update = string.Format("update [dbo].[Export] set State='生成中' where ImpName='总氮零点和量程校正液核查测定结果表'");
                    g_DatabaseHelper.ExecuteNonQuery(update, MonitoringBusinessConnection);
                }
                if (i == 12)
                {
                    string sql = string.Format(@"select c.MonitoringPointName as PointId,CONVERT(varchar(100), a.Tstamp, 111) as Tstamp,a.[CodMonitorValue] as ZMonitorValue
      ,[CodLastMonitorValue] as ZStandardValue
      ,[CodLastRelativeError] as ZAbsoluteError
	  ,b.[TNMonitorValue] as LMonitorValue
      ,[CodLastDayMonitorValue] as LStandValue
      ,[Cod24HRelativeErrorValue] as LErrorValue
from {0} a
left join {1} b
on a.PointId=b.PointId and a.Tstamp =b.Tstamp left join {2} c on a.PointId=c.PointId
order by PointId,Tstamp", tableName3, tableName4,tableBase); 
                    dt = g_DatabaseHelper.ExecuteDataTable(sql, MonitoringBusinessConnection);
                    doc = new Document(System.IO.Path.Combine(path, "高锰酸盐指数24小时零点和量程漂移测定结果表.docx"));
                    filename = "高锰酸盐指数24小时零点和量程漂移测定结果表.docx";
                    string update = string.Format("update [dbo].[Export] set State='生成中' where ImpName='高锰酸盐指数24小时零点和量程漂移测定结果表'");
                    g_DatabaseHelper.ExecuteNonQuery(update, MonitoringBusinessConnection);
                }
                if (i == 13)
                {
                    string sql = string.Format(@"select c.MonitoringPointName as PointId,CONVERT(varchar(100), a.Tstamp, 111) as Tstamp,a.[NHMonitorValue] as ZMonitorValue
      ,[NHLastMonitorValue] as ZStandardValue
      ,[NHLastRelativeError] as ZAbsoluteError
	  ,b.[NHMonitorValue] as LMonitorValue
      ,[NHLastDayMonitorValue] as LStandValue
      ,[NH24HRelativeErrorValue] as LErrorValue
from {0} a
left join {1} b
on a.PointId=b.PointId and a.Tstamp =b.Tstamp left join {2} c on a.PointId=c.PointId
order by PointId,Tstamp", tableName3, tableName4,tableBase); 
                    dt = g_DatabaseHelper.ExecuteDataTable(sql, MonitoringBusinessConnection);
                    doc = new Document(System.IO.Path.Combine(path, "氨氮24小时零点和量程漂移测定结果表.docx"));
                    filename = "氨氮24小时零点和量程漂移测定结果表.docx";
                    string update = string.Format("update [dbo].[Export] set State='生成中' where ImpName='氨氮24小时零点和量程漂移测定结果表'");
                    g_DatabaseHelper.ExecuteNonQuery(update, MonitoringBusinessConnection);
                }
                if (i == 14)
                {
                    string sql = string.Format(@"select c.MonitoringPointName as PointId,CONVERT(varchar(100), a.Tstamp, 111) as Tstamp,a.[TPMonitorValue] as ZMonitorValue
      ,[TPLastMonitorValue] as ZStandardValue
      ,[TPLastRelativeError] as ZAbsoluteError
	  ,b.[TPMonitorValue] as LMonitorValue
      ,[TPLastDayMonitorValue] as LStandValue
      ,[TP24HRelativeErrorValue] as LErrorValue
from {0} a
left join {1} b
on a.PointId=b.PointId and a.Tstamp =b.Tstamp left join {2} c on a.PointId=c.PointId
order by PointId,Tstamp", tableName3, tableName4,tableBase); 
                    dt = g_DatabaseHelper.ExecuteDataTable(sql, MonitoringBusinessConnection);
                    doc = new Document(System.IO.Path.Combine(path, "总磷24小时零点和量程漂移测定结果表.docx"));
                    filename = "总磷24小时零点和量程漂移测定结果表.docx";
                    string update = string.Format("update [dbo].[Export] set State='生成中' where ImpName='总磷24小时零点和量程漂移测定结果表'");
                    g_DatabaseHelper.ExecuteNonQuery(update, MonitoringBusinessConnection);
                }
                if (i == 15)
                {
                    string sql = string.Format(@"select c.MonitoringPointName as PointId,CONVERT(varchar(100), a.Tstamp, 111) as Tstamp,a.[TNMonitorValue] as ZMonitorValue
      ,[TNLastMonitorValue] as ZStandardValue
      ,[TNLastRelativeError] as ZAbsoluteError
	  ,b.[TNMonitorValue] as LMonitorValue
      ,[TNLastDayMonitorValue] as LStandValue
      ,[TN24HRelativeErrorValue] as LErrorValue
from {0} a
left join {1} b
on a.PointId=b.PointId and a.Tstamp =b.Tstamp left join {2} c on a.PointId=c.PointId
order by PointId,Tstamp", tableName3, tableName4,tableBase); 
                    dt = g_DatabaseHelper.ExecuteDataTable(sql, MonitoringBusinessConnection);
                    doc = new Document(System.IO.Path.Combine(path, "总氮24小时零点和量程漂移测定结果表.docx"));
                    filename = "总氮24小时零点和量程漂移测定结果表.docx";
                    string update = string.Format("update [dbo].[Export] set State='生成中' where ImpName='总氮24小时零点和量程漂移测定结果表'");
                    g_DatabaseHelper.ExecuteNonQuery(update, MonitoringBusinessConnection);
                }
                auditLogGrid.Rebind();
                DocumentBuilder builder = new DocumentBuilder(doc);
                builder.MoveToMergeField("MF2");
                if (i >= 0 && i < 4)
                {
                    MoveToMF2One(builder, dt);
                }
                else if (i == 4)
                {
                    MoveToMF2TwoPH(builder, dt);
                }
                else if (i == 5)
                {
                    MoveToMF2TwoRJ(builder, dt);
                }
                else if (i == 6)
                {
                    MoveToMF2TwoDD(builder, dt);
                }
                else if (i == 7)
                {
                    MoveToMF2TwoZD(builder, dt);
                }
                else if (i == 8)
                {
                    MoveToMF2ThreeGM(builder, dt);
                }
                else if (i == 9)
                {
                    MoveToMF2ThreeAD(builder, dt);
                }
                else if (i == 10)
                {
                    MoveToMF2ThreeZL(builder, dt);
                }
                else if (i == 11)
                {
                    MoveToMF2ThreeZD(builder, dt);
                }
                else if (i >= 12 && i < 16)
                {
                    MoveToMF2ThreeAD(builder, dt);
                }
                doc.MailMerge.DeleteFields();
                string strTargets = strTarget + filename;
                //strTarget = Server.MapPath("../" + filename);
                doc.Save(strTargets);
            }
            
        }
        #region 将文件打包压缩，并导出压缩后的压缩包
        protected void btnSearch1_Click(object sender, ImageClickEventArgs e)
        {
            string path = strTarget.Substring(0, strTarget.Length-2);
            string filename = "";
            List<string> fils = new List<string> { };
            Document doc = new Document();
            string error = "";
            //int[] intarr;
            foreach (Object i in auditLogGrid.SelectedIndexes)
            {
                if(i.ToString()=="0")
                {
                    doc = new Document(System.IO.Path.Combine(path, "高锰酸盐指数实际水样比对测试结果表.docx"));
                    filename = "高锰酸盐指数实际水样比对测试结果表.docx";
                    fils.Add(path + "\\" + filename);
                }
                if (i.ToString() == "1")
                {
                    doc = new Document(System.IO.Path.Combine(path, "氨氮实际水样比对测试结果表.docx"));
                    filename = "氨氮实际水样比对测试结果表.docx";
                    fils.Add(path + "\\" + filename);
                }
                if (i.ToString() == "2")
                {
                    doc = new Document(System.IO.Path.Combine(path, "总磷实际水样比对测试结果表.docx"));
                    filename = "总磷实际水样比对测试结果表.docx";
                    fils.Add(path + "\\" + filename);
                }
                if (i.ToString() == "3")
                {
                    doc = new Document(System.IO.Path.Combine(path, "总氮实际水样比对测试结果表.docx"));
                    filename = "总氮实际水样比对测试结果表.docx";
                    fils.Add(path + "\\" + filename);
                }
                if (i.ToString() == "4")
                {
                    doc = new Document(System.IO.Path.Combine(path, "pH样品核查测试结果表.docx"));
                    filename = "pH样品核查测试结果表.docx";
                    fils.Add(path + "\\" + filename);
                }
                if (i.ToString() == "5")
                {
                    doc = new Document(System.IO.Path.Combine(path, "溶解氧样品核查测试结果表.docx"));
                    filename = "溶解氧样品核查测试结果表.docx";
                    fils.Add(path + "\\" + filename);
                }
                if (i.ToString() == "6")
                {
                    doc = new Document(System.IO.Path.Combine(path, "电导率样品核查测试结果表.docx"));
                    filename = "电导率样品核查测试结果表.docx";
                    fils.Add(path + "\\" + filename);
                }
                if (i.ToString() == "7")
                {
                    doc = new Document(System.IO.Path.Combine(path, "浊度样品核查测试结果表.docx"));
                    filename = "浊度样品核查测试结果表.docx";
                    fils.Add(path + "\\" + filename);
                }
                if (i.ToString() == "8")
                {
                    doc = new Document(System.IO.Path.Combine(path, "高锰酸盐指数零点和量程校正液核查测定结果表.docx"));
                    filename = "高锰酸盐指数零点和量程校正液核查测定结果表.docx";
                    fils.Add(path + "\\" + filename);
                }
                if (i.ToString() == "9")
                {
                    doc = new Document(System.IO.Path.Combine(path, "氨氮零点和量程校正液核查测定结果表.docx"));
                    filename = "氨氮零点和量程校正液核查测定结果表.docx";
                    fils.Add(path + "\\" + filename);
                }
                if (i.ToString() == "10")
                {
                    doc = new Document(System.IO.Path.Combine(path, "总磷零点和量程校正液核查测定结果表.docx"));
                    filename = "总磷零点和量程校正液核查测定结果表.docx";
                    fils.Add(path + "\\" + filename);
                }
                if (i.ToString() == "11")
                {
                    doc = new Document(System.IO.Path.Combine(path, "总氮零点和量程校正液核查测定结果表.docx"));
                    filename = "总氮零点和量程校正液核查测定结果表.docx";
                    fils.Add(path + "\\" + filename);
                }
                if (i.ToString() == "12")
                {
                    doc = new Document(System.IO.Path.Combine(path, "高锰酸盐指数24小时零点和量程漂移测定结果表.docx"));
                    filename = "高锰酸盐指数24小时零点和量程漂移测定结果表.docx";
                    fils.Add(path + "\\" + filename);
                }
                if (i.ToString() == "13")
                {
                    doc = new Document(System.IO.Path.Combine(path, "氨氮24小时零点和量程漂移测定结果表.docx"));
                    filename = "氨氮24小时零点和量程漂移测定结果表.docx";
                    fils.Add(path + "\\" + filename);
                }
                if (i.ToString() == "14")
                {
                    doc = new Document(System.IO.Path.Combine(path, "总磷24小时零点和量程漂移测定结果表.docx"));
                    filename = "总磷24小时零点和量程漂移测定结果表.docx";
                    fils.Add(path + "\\" + filename);
                }
                if (i.ToString() == "15")
                {
                    doc = new Document(System.IO.Path.Combine(path, "总氮24小时零点和量程漂移测定结果表.docx"));
                    filename = "总氮24小时零点和量程漂移测定结果表.docx";
                    fils.Add(path +"\\"+ filename);
                }
            }
            string[] File = fils.ToArray();
            if (Pack(File, path, 9, "123456", out error))
            {

                FileInfo file = new FileInfo(path+"\\File.zip");//创建一个文件对象  
                Response.Clear();//清除所有缓存区的内容  
                Response.Charset = "GB2312";//定义输出字符集  
                Response.ContentEncoding = Encoding.Default;//输出内容的编码为默认编码  
                Response.AddHeader("Content-Disposition", "attachment;filename=" + file.Name);
                //添加头信息。为“文件下载/另存为”指定默认文件名称  
                Response.AddHeader("Content-Length", file.Length.ToString());
                //添加头文件，指定文件的大小，让浏览器显示文件下载的速度   
                Response.WriteFile(file.FullName);// 把文件流发送到客户端  
                Response.End();  
            }
        }
        /// <summary>
        ///  将多个文件或文件夹打包
        /// </summary>
        /// <param name="filesOrDirectoriesPaths">被打包文件的路径</param>
        /// <param name="strZipPath">打包后存放的路径</param>
        /// <param name="intZipLevel">打包压缩级别0-9(0为不压缩)</param>
        /// <param name="strPassword">打包密码</param>
        /// <param name="error">打包过程中的错误信息</param>
        /// <returns>是否打包成功</returns>
        public bool Pack(string[] filesOrDirectoriesPaths, string strZipPath, int intZipLevel, string strPassword, out string error)
        {
            files = new List<string[]>();
            root = "";
            foreach (string filename in filesOrDirectoriesPaths)
            {
                if (File.Exists(filename))
                {
                    files.Add(new string[] { filename, filename.Substring(0, filename.LastIndexOf("\\") + 1) });
                }
                //else if (Directory.Exists(filename))
                //{
                //    root = filename.Substring(0, filename.LastIndexOf("\\") + 1);
                //    GetAllDirectories(filename);
                //}
                else
                {
                    error = "请检查文件路径，某些文件不存在！！！";
                    return false;
                }
            }
            ZipOutputStream zipOutputStream = new ZipOutputStream(File.Create(strZipPath+"\\File.zip"));
            zipOutputStream.SetLevel(intZipLevel);
            //zipOutputStream.Password = strPassword;
            foreach (string[] strFile in files)
            {
                try
                {
                    FileStream fs = File.OpenRead(strFile[0]);
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    string strFileName = strFile[0].Replace(strFile[1], String.Empty);

                    ZipEntry entry = new ZipEntry(strFileName);
                    entry.DateTime = DateTime.Now;
                    zipOutputStream.PutNextEntry(entry);
                    zipOutputStream.Write(buffer, 0, buffer.Length);
                    fs.Close();
                    fs.Dispose();
                }
                catch
                {
                    error = "文件读取错误！";
                    return false;
                }
            }
            files.Clear();
            zipOutputStream.Finish();
            zipOutputStream.Close();
            error = "";
            return true;
        }

        /// <summary>
        /// 取得目录下所有文件及文件夹，分别存入files及paths
        /// </summary>
        /// <param name="rootPath">根目录</param>
        private static void GetAllDirectories(string rootPath)
        {
            string[] subPaths = Directory.GetDirectories(rootPath);//得到所有子目录
            foreach (string path in subPaths)
            {
                GetAllDirectories(path);//对每一个字目录做与根目录相同的操作：即找到子目录并将当前目录的文件名存入List
            }
            string[] filess = Directory.GetFiles(rootPath);
            foreach (string file in filess)
            {
                files.Add(new string[] { file, root });//将当前目录中的所有文件全名存入文件List               
            }
            if (subPaths.Length == filess.Length && filess.Length == 0)//如果是空目录
            {
                //记录空目录
                paths.Add(new string[] { rootPath, root });
            }
        }
        #endregion
        protected void btnSearch2_Click(object sender, ImageClickEventArgs e)
        {

        }

        #region 实际水样比对测试结果表
        /// <summary>
        /// 水质自动监测子站运行情况表
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="pointId"></param>
        /// <param name="factors"></param>
        public void MoveToMF2One(DocumentBuilder builder, DataTable dt)
        {
            try
            {
                builder.Font.ClearFormatting();
                builder.CellFormat.Borders.LineStyle = LineStyle.Single;
                builder.CellFormat.Borders.Color = System.Drawing.Color.Black;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                builder.Font.Size = 10;
                List<double> widthList = new List<double>();
                double width, widths = 0;
                width = 70;
                widths = 45.380;
                DataView dv = dt.DefaultView;
                List<string> pointId = dt.AsEnumerable().Select(t => Convert.ToString(t.Field<Int32>("PointId"))).ToList();
                IEnumerable<string> names = pointId.Distinct();
                foreach (string name in names)
                {
                    dv.RowFilter = "PointId='" + name + "'";
                    for (int i = 0; i < dv.ToTable().Rows.Count + 2; i++)
                    {
                        if (i == 0)
                        {
                            builder.ParagraphFormat.Alignment = ParagraphAlignment.Right;
                            builder.Writeln("工位号" + name);
                            builder.Font.Bold = true;
                            for (int j = 0; j < 7; j++)
                            {
                                builder.InsertCell();
                                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                builder.CellFormat.HorizontalMerge = CellMerge.None;
                                builder.CellFormat.VerticalMerge = CellMerge.None;
                                builder.CellFormat.Width = width;
                                switch (j)
                                {
                                    case 0:
                                        builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("序号");
                                        break;
                                    case 1:
                                        builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("日期");
                                        break;
                                    case 2:
                                        builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("采样时间");
                                        break;
                                    case 3:
                                        builder.CellFormat.HorizontalMerge = CellMerge.First;
                                        builder.Write("测定结果(mg/L)");
                                        break;
                                    case 4:
                                        builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                                        //builder.Write("测定结果(mg/L)");
                                        break;
                                    case 5:
                                        builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("相对误差");
                                        break;
                                    case 6:
                                        //builder.CellFormat.Width = 100;
                                        //builder.CellFormat.VerticalMerge = CellMerge.First;

                                        builder.Write("相对误差范围");
                                        break;
                                }
                            }
                            builder.EndRow();
                        }
                        else if (i == 1)
                        {
                            builder.Font.Bold = true;
                            for (int j = 0; j < 7; j++)
                            {
                                builder.InsertCell();
                                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                builder.CellFormat.HorizontalMerge = CellMerge.None;
                                builder.CellFormat.VerticalMerge = CellMerge.None;
                                builder.CellFormat.Width = width;
                                switch (j)
                                {
                                    case 0:
                                        builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        //builder.Write("序号");
                                        break;
                                    case 1:
                                        builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        //builder.Write("日期");
                                        break;
                                    case 2:
                                        builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        //builder.Write("采样时间");
                                        break;
                                    case 3:
                                        //builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("系统");
                                        break;
                                    case 4:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.First;
                                        builder.Write("标准分析");
                                        break;
                                    case 5:
                                        builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        //builder.Write("相对误差");
                                        break;
                                    case 6:
                                        //builder.CellFormat.Width = 100;
                                        //builder.CellFormat.VerticalMerge = CellMerge.Previous;

                                        builder.Write("相对误差范围");
                                        break;
                                }
                            }
                            builder.EndRow();
                        }
                        else
                        {
                            builder.Font.Bold = true;
                            for (int j = 0; j < 7; j++)
                            {
                                switch (j)
                                {
                                    case 0:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write((i - 1).ToString());
                                        break;
                                    case 1:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(Convert.ToDateTime(dv.ToTable().Rows[i - 2][j - 1].ToString()).ToString("MM月yy日"));
                                        break;
                                    case 2:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(Convert.ToDateTime(dv.ToTable().Rows[i - 2][j - 2].ToString()).ToString("HH:00"));
                                        break;
                                    case 3:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(dv.ToTable().Rows[i - 2][j - 2].ToString());
                                        break;
                                    case 4:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(dv.ToTable().Rows[i - 2][j - 2].ToString());
                                        break;
                                    case 5:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(dv.ToTable().Rows[i - 2][j - 2].ToString());
                                        break;
                                    case 6:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        //builder.CellFormat.Width = 100;
                                        builder.Write("≤±20%（固定式水站）;≤±30%（浮船式水站）");
                                        break;
                                }
                            }
                            builder.EndRow();
                        }
                    }
                    builder.EndTable();
                }
                
                
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
        }
        #endregion
        #region 样品核查测试结果表PH
        /// <summary>
        /// 水质自动监测子站运行情况表
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="pointId"></param>
        /// <param name="factors"></param>
        public void MoveToMF2TwoPH(DocumentBuilder builder, DataTable dt)
        {
            try
            {
                builder.Font.ClearFormatting();
                builder.CellFormat.Borders.LineStyle = LineStyle.Single;
                builder.CellFormat.Borders.Color = System.Drawing.Color.Black;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                builder.Font.Size = 10;
                List<double> widthList = new List<double>();
                double width, widths = 0;
                width = 70;
                widths = 45.380;
                DataView dv = dt.DefaultView;
                List<string> pointId = dt.AsEnumerable().Select(t => t.Field<String>("PointId")).ToList();
                IEnumerable<string> names = pointId.Distinct();
                foreach (string name in names)
                {
                    dv.RowFilter = "PointId='" + name + "'";
                    for (int i = 0; i < dv.ToTable().Rows.Count + 2; i++)
                    {
                        if (i == 0)
                        {
                            builder.ParagraphFormat.Alignment = ParagraphAlignment.Right;
                            builder.Writeln("工位号" + name);
                            builder.Font.Bold = true;
                            for (int j = 0; j < 7; j++)
                            {
                                builder.InsertCell();
                                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                builder.CellFormat.HorizontalMerge = CellMerge.None;
                                builder.CellFormat.VerticalMerge = CellMerge.None;
                                builder.CellFormat.Width = width;
                                switch (j)
                                {
                                    case 0:
                                        builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("序号");
                                        break;
                                    case 1:
                                        builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("日期");
                                        break;
                                    case 2:
                                        builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("采样时间");
                                        break;
                                    case 3:
                                        builder.CellFormat.HorizontalMerge = CellMerge.First;
                                        builder.Write("测定结果(无量纲)");
                                        break;
                                    case 4:
                                        builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                                        //builder.Write("测定结果(无量纲)");
                                        break;
                                    case 5:
                                        builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("绝对误差");
                                        break;
                                    case 6:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                                        builder.CellFormat.Width = 100;
                                        builder.Write("绝对误差范围");
                                        break;
                                }
                            }
                            builder.EndRow();
                        }
                        else if (i == 1)
                        {
                            builder.Font.Bold = true;
                            for (int j = 0; j < 7; j++)
                            {
                                builder.InsertCell();
                                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                builder.CellFormat.HorizontalMerge = CellMerge.None;
                                builder.CellFormat.VerticalMerge = CellMerge.None;
                                builder.CellFormat.Width = width;
                                switch (j)
                                {
                                    case 0:
                                        builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        //builder.Write("序号");
                                        break;
                                    case 1:
                                        builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        //builder.Write("日期");
                                        break;
                                    case 2:
                                        builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        //builder.Write("采样时间");
                                        break;
                                    case 3:
                                        //builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("系统");
                                        break;
                                    case 4:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.First;
                                        builder.Write("样品值");
                                        break;
                                    case 5:
                                        builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        //builder.Write("绝对误差");
                                        break;
                                    case 6:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                                        builder.CellFormat.Width = 100;
                                        builder.Write("绝对误差范围");
                                        break;
                                }
                            }
                            builder.EndRow();
                        }
                        else
                        {
                            builder.Font.Bold = true;
                            for (int j = 0; j < 7; j++)
                            {
                                switch (j)
                                {
                                    case 0:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write((i - 1).ToString());
                                        break;
                                    case 1:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(Convert.ToDateTime(dv.ToTable().Rows[i - 2][j].ToString()).ToString("MM月yy日"));
                                        break;
                                    case 2:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(Convert.ToDateTime(dv.ToTable().Rows[i - 2][j - 1].ToString()).ToString("HH:00"));
                                        break;
                                    case 3:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(dv.ToTable().Rows[i - 2][j - 1].ToString());
                                        break;
                                    case 4:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(dv.ToTable().Rows[i - 2][j - 1].ToString());
                                        break;
                                    case 5:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(dv.ToTable().Rows[i - 2][j - 1].ToString());
                                        break;
                                    case 6:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = 100;
                                        builder.Write("≤±0.10pH");
                                        break;
                                }
                            }
                            builder.EndRow();
                        }
                    }

                    builder.EndTable();
                }
                
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
        }
        #endregion
        #region 样品核查测试结果表溶解氧
        /// <summary>
        /// 水质自动监测子站运行情况表
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="pointId"></param>
        /// <param name="factors"></param>
        public void MoveToMF2TwoRJ(DocumentBuilder builder, DataTable dt)
        {
            try
            {
                builder.Font.ClearFormatting();
                builder.CellFormat.Borders.LineStyle = LineStyle.Single;
                builder.CellFormat.Borders.Color = System.Drawing.Color.Black;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                builder.Font.Size = 10;
                List<double> widthList = new List<double>();
                double width, widths = 0;
                width = 70;
                widths = 45.380;
                DataView dv = dt.DefaultView;
                List<string> pointId = dt.AsEnumerable().Select(t => t.Field<String>("PointId")).ToList();
                IEnumerable<string> names = pointId.Distinct();
                foreach (string name in names)
                {
                    dv.RowFilter = "PointId='" + name + "'";
                    for (int i = 0; i < dv.ToTable().Rows.Count + 2; i++)
                    {
                        if (i == 0)
                        {
                            builder.ParagraphFormat.Alignment = ParagraphAlignment.Right;
                            builder.Writeln("工位号" + name);
                            builder.Font.Bold = true;
                            for (int j = 0; j < 7; j++)
                            {
                                builder.InsertCell();
                                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                builder.CellFormat.HorizontalMerge = CellMerge.None;
                                builder.CellFormat.VerticalMerge = CellMerge.None;
                                builder.CellFormat.Width = width;
                                switch (j)
                                {
                                    case 0:
                                        builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("序号");
                                        break;
                                    case 1:
                                        builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("日期");
                                        break;
                                    case 2:
                                        builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("采样时间");
                                        break;
                                    case 3:
                                        builder.CellFormat.HorizontalMerge = CellMerge.First;
                                        builder.Write("测定结果(mg/L)");
                                        break;
                                    case 4:
                                        builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                                        //builder.Write("测定结果(无量纲)");
                                        break;
                                    case 5:
                                        builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("相对误差");
                                        break;
                                    case 6:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                                        builder.CellFormat.Width = 100;
                                        builder.Write("相对误差范围");
                                        break;
                                }
                            }
                            builder.EndRow();
                        }
                        else if (i == 1)
                        {
                            builder.Font.Bold = true;
                            for (int j = 0; j < 7; j++)
                            {
                                builder.InsertCell();
                                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                builder.CellFormat.HorizontalMerge = CellMerge.None;
                                builder.CellFormat.VerticalMerge = CellMerge.None;
                                builder.CellFormat.Width = width;
                                switch (j)
                                {
                                    case 0:
                                        builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        //builder.Write("序号");
                                        break;
                                    case 1:
                                        builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        //builder.Write("日期");
                                        break;
                                    case 2:
                                        builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        //builder.Write("采样时间");
                                        break;
                                    case 3:
                                        //builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("系统");
                                        break;
                                    case 4:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.First;
                                        builder.Write("样品值");
                                        break;
                                    case 5:
                                        builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        //builder.Write("绝对误差");
                                        break;
                                    case 6:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                                        builder.CellFormat.Width = 100;
                                        builder.Write("相对误差范围");
                                        break;
                                }
                            }
                            builder.EndRow();
                        }
                        else
                        {
                            builder.Font.Bold = true;
                            for (int j = 0; j < 7; j++)
                            {
                                switch (j)
                                {
                                    case 0:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write((i - 1).ToString());
                                        break;
                                    case 1:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(Convert.ToDateTime(dv.ToTable().Rows[i - 2][j].ToString()).ToString("MM月yy日"));
                                        break;
                                    case 2:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(Convert.ToDateTime(dv.ToTable().Rows[i - 2][j - 1].ToString()).ToString("HH:00"));
                                        break;
                                    case 3:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(dv.ToTable().Rows[i - 2][j - 1].ToString());
                                        break;
                                    case 4:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(dv.ToTable().Rows[i - 2][j - 1].ToString());
                                        break;
                                    case 5:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(dv.ToTable().Rows[i - 2][j - 1].ToString());
                                        break;
                                    case 6:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = 100;
                                        builder.Write("≤±10%");
                                        break;
                                }
                            }
                            builder.EndRow();
                        }
                    }

                    builder.EndTable();
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
        }
        #endregion
        #region 样品核查测试结果表电导率
        /// <summary>
        /// 水质自动监测子站运行情况表
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="pointId"></param>
        /// <param name="factors"></param>
        public void MoveToMF2TwoDD(DocumentBuilder builder, DataTable dt)
        {
            try
            {
                builder.Font.ClearFormatting();
                builder.CellFormat.Borders.LineStyle = LineStyle.Single;
                builder.CellFormat.Borders.Color = System.Drawing.Color.Black;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                builder.Font.Size = 10;
                List<double> widthList = new List<double>();
                double width, widths = 0;
                width = 70;
                widths = 45.380;
                DataView dv = dt.DefaultView;
                List<string> pointId = dt.AsEnumerable().Select(t => t.Field<String>("PointId")).ToList();
                IEnumerable<string> names = pointId.Distinct();
                foreach (string name in names)
                {
                    dv.RowFilter = "PointId='" + name + "'";
                    for (int i = 0; i < dv.ToTable().Rows.Count + 2; i++)
                    {
                        if (i == 0)
                        {
                            builder.ParagraphFormat.Alignment = ParagraphAlignment.Right;
                            builder.Writeln("工位号" + name);
                            builder.Font.Bold = true;
                            for (int j = 0; j < 7; j++)
                            {
                                builder.InsertCell();
                                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                builder.CellFormat.HorizontalMerge = CellMerge.None;
                                builder.CellFormat.VerticalMerge = CellMerge.None;
                                builder.CellFormat.Width = width;
                                switch (j)
                                {
                                    case 0:
                                        builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("序号");
                                        break;
                                    case 1:
                                        builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("日期");
                                        break;
                                    case 2:
                                        builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("采样时间");
                                        break;
                                    case 3:
                                        builder.CellFormat.HorizontalMerge = CellMerge.First;
                                        builder.Write("测定结果(μS/cm)");
                                        break;
                                    case 4:
                                        builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                                        //builder.Write("测定结果(无量纲)");
                                        break;
                                    case 5:
                                        builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("相对误差");
                                        break;
                                    case 6:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                                        builder.CellFormat.Width = 100;
                                        builder.Write("相对误差范围");
                                        break;
                                }
                            }
                            builder.EndRow();
                        }
                        else if (i == 1)
                        {
                            builder.Font.Bold = true;
                            for (int j = 0; j < 7; j++)
                            {
                                builder.InsertCell();
                                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                builder.CellFormat.HorizontalMerge = CellMerge.None;
                                builder.CellFormat.VerticalMerge = CellMerge.None;
                                builder.CellFormat.Width = width;
                                switch (j)
                                {
                                    case 0:
                                        builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        //builder.Write("序号");
                                        break;
                                    case 1:
                                        builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        //builder.Write("日期");
                                        break;
                                    case 2:
                                        builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        //builder.Write("采样时间");
                                        break;
                                    case 3:
                                        //builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("系统");
                                        break;
                                    case 4:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.First;
                                        builder.Write("样品值");
                                        break;
                                    case 5:
                                        builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        //builder.Write("绝对误差");
                                        break;
                                    case 6:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                                        builder.CellFormat.Width = 100;
                                        builder.Write("相对误差范围");
                                        break;
                                }
                            }
                            builder.EndRow();
                        }
                        else
                        {
                            builder.Font.Bold = true;
                            for (int j = 0; j < 7; j++)
                            {
                                switch (j)
                                {
                                    case 0:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write((i - 1).ToString());
                                        break;
                                    case 1:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(Convert.ToDateTime(dv.ToTable().Rows[i - 2][j].ToString()).ToString("MM月yy日"));
                                        break;
                                    case 2:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(Convert.ToDateTime(dv.ToTable().Rows[i - 2][j - 1].ToString()).ToString("HH:00"));
                                        break;
                                    case 3:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(dv.ToTable().Rows[i - 2][j - 1].ToString());
                                        break;
                                    case 4:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(dv.ToTable().Rows[i - 2][j - 1].ToString());
                                        break;
                                    case 5:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(dv.ToTable().Rows[i - 2][j - 1].ToString());
                                        break;
                                    case 6:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = 100;
                                        builder.Write("≤±5%");
                                        break;
                                }
                            }
                            builder.EndRow();
                        }
                    }

                    builder.EndTable();
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
        }
        #endregion
        #region 样品核查测试结果表浊度
        /// <summary>
        /// 水质自动监测子站运行情况表
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="pointId"></param>
        /// <param name="factors"></param>
        public void MoveToMF2TwoZD(DocumentBuilder builder, DataTable dt)
        {
            try
            {
                builder.Font.ClearFormatting();
                builder.CellFormat.Borders.LineStyle = LineStyle.Single;
                builder.CellFormat.Borders.Color = System.Drawing.Color.Black;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                builder.Font.Size = 10;
                List<double> widthList = new List<double>();
                double width, widths = 0;
                width = 70;
                widths = 45.380;
                DataView dv = dt.DefaultView;
                List<string> pointId = dt.AsEnumerable().Select(t => t.Field<String>("PointId")).ToList();
                IEnumerable<string> names = pointId.Distinct();
                foreach (string name in names)
                {
                    dv.RowFilter = "PointId='" + name + "'";
                    for (int i = 0; i < dv.ToTable().Rows.Count + 2; i++)
                    {
                        if (i == 0)
                        {
                            builder.ParagraphFormat.Alignment = ParagraphAlignment.Right;
                            builder.Writeln("工位号" + name);
                            builder.Font.Bold = true;
                            for (int j = 0; j < 7; j++)
                            {
                                builder.InsertCell();
                                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                builder.CellFormat.HorizontalMerge = CellMerge.None;
                                builder.CellFormat.VerticalMerge = CellMerge.None;
                                builder.CellFormat.Width = width;
                                switch (j)
                                {
                                    case 0:
                                        builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("序号");
                                        break;
                                    case 1:
                                        builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("日期");
                                        break;
                                    case 2:
                                        builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("采样时间");
                                        break;
                                    case 3:
                                        builder.CellFormat.HorizontalMerge = CellMerge.First;
                                        builder.Write("测定结果(NTU)");
                                        break;
                                    case 4:
                                        builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                                        //builder.Write("测定结果(无量纲)");
                                        break;
                                    case 5:
                                        builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("相对误差");
                                        break;
                                    case 6:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                                        builder.CellFormat.Width = 100;
                                        builder.Write("相对误差范围");
                                        break;
                                }
                            }
                            builder.EndRow();
                        }
                        else if (i == 1)
                        {
                            builder.Font.Bold = true;
                            for (int j = 0; j < 7; j++)
                            {
                                builder.InsertCell();
                                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                builder.CellFormat.HorizontalMerge = CellMerge.None;
                                builder.CellFormat.VerticalMerge = CellMerge.None;
                                builder.CellFormat.Width = width;
                                switch (j)
                                {
                                    case 0:
                                        builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        //builder.Write("序号");
                                        break;
                                    case 1:
                                        builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        //builder.Write("日期");
                                        break;
                                    case 2:
                                        builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        //builder.Write("采样时间");
                                        break;
                                    case 3:
                                        //builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("系统");
                                        break;
                                    case 4:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.First;
                                        builder.Write("样品值");
                                        break;
                                    case 5:
                                        builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        //builder.Write("绝对误差");
                                        break;
                                    case 6:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                                        builder.CellFormat.Width = 100;
                                        builder.Write("相对误差范围");
                                        break;
                                }
                            }
                            builder.EndRow();
                        }
                        else
                        {
                            builder.Font.Bold = true;
                            for (int j = 0; j < 7; j++)
                            {
                                switch (j)
                                {
                                    case 0:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write((i - 1).ToString());
                                        break;
                                    case 1:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(Convert.ToDateTime(dv.ToTable().Rows[i - 2][j].ToString()).ToString("MM月yy日"));
                                        break;
                                    case 2:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(Convert.ToDateTime(dv.ToTable().Rows[i - 2][j - 1].ToString()).ToString("HH:00"));
                                        break;
                                    case 3:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(dv.ToTable().Rows[i - 2][j - 1].ToString());
                                        break;
                                    case 4:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(dv.ToTable().Rows[i - 2][j - 1].ToString());
                                        break;
                                    case 5:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(dv.ToTable().Rows[i - 2][j - 1].ToString());
                                        break;
                                    case 6:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = 100;
                                        builder.Write("≤±10%");
                                        break;
                                }
                            }
                            builder.EndRow();
                        }
                    }

                    builder.EndTable();
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
        }
        #endregion
        #region 零点和量程校正液核查测定结果表高锰酸盐
        /// <summary>
        /// 水质自动监测子站运行情况表
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="pointId"></param>
        /// <param name="factors"></param>
        public void MoveToMF2ThreeGM(DocumentBuilder builder, DataTable dt)
        {
            try
            {
                builder.Font.ClearFormatting();
                builder.CellFormat.Borders.LineStyle = LineStyle.Single;
                builder.CellFormat.Borders.Color = System.Drawing.Color.Black;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                builder.Font.Size = 10;
                List<double> widthList = new List<double>();
                double width, widths = 0;
                width = 70;
                widths = 45.380;
                DataView dv = dt.DefaultView;
                List<string> pointId = dt.AsEnumerable().Select(t => t.Field<String>("PointId")).ToList();
                IEnumerable<string> names = pointId.Distinct();
                foreach (string name in names)
                {
                    for (int i = 0; i < dt.Rows.Count + 3; i++)
                    {
                        if (i == 0)
                        {
                            builder.Font.Bold = true;
                            for (int j = 0; j < 10; j++)
                            {
                                builder.InsertCell();
                                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                builder.CellFormat.HorizontalMerge = CellMerge.None;
                                builder.CellFormat.VerticalMerge = CellMerge.None;
                                builder.CellFormat.Width = width;
                                switch (j)
                                {
                                    case 0:
                                        //builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("序号");
                                        break;
                                    case 1:
                                        //builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("日期");
                                        break;
                                    case 2:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.First;
                                        builder.Write("零点校正液核查");
                                        break;
                                    case 3:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.None;
                                        builder.Write("零点校正液核查");
                                        break;
                                    case 4:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.None;
                                        builder.Write("零点校正液核查");
                                        break;
                                    case 5:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                                        builder.Write("零点校正液核查");
                                        break;
                                    case 6:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.First;
                                        builder.Write("量程校正液核查");
                                        break;
                                    case 7:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.None;
                                        builder.Write("量程校正液核查");
                                        break;
                                    case 8:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.None;
                                        builder.Write("量程校正液核查");
                                        break;
                                    case 9:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                                        builder.Write("量程校正液核查");
                                        break;
                                }
                            }
                            builder.EndRow();
                        }
                        else if (i == 1)
                        {
                            builder.Font.Bold = true;
                            for (int j = 0; j < 10; j++)
                            {
                                builder.InsertCell();
                                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                builder.CellFormat.HorizontalMerge = CellMerge.None;
                                builder.CellFormat.VerticalMerge = CellMerge.None;
                                builder.CellFormat.Width = width;
                                switch (j)
                                {
                                    case 0:
                                        //builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        builder.Write("序号");
                                        break;
                                    case 1:
                                        //builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        builder.Write("日期");
                                        break;
                                    case 2:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.First;
                                        builder.Write("测定结果");
                                        break;
                                    case 3:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                                        builder.Write("测定结果(mg/L)");
                                        break;
                                    case 4:
                                        //builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("绝对误差");
                                        break;
                                    case 5:
                                        //builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("绝对误差范围");
                                        break;
                                    case 6:
                                        builder.CellFormat.HorizontalMerge = CellMerge.First;
                                        builder.Write("测定结果");
                                        break;
                                    case 7:
                                        builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                                        break;
                                    case 8:
                                        //builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("相对误差");
                                        break;
                                    case 9:
                                        //builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("相对误差范围");
                                        break;
                                }
                            }
                            builder.EndRow();
                        }
                        else if (i == 2)
                        {
                            builder.Font.Bold = true;
                            for (int j = 0; j < 10; j++)
                            {
                                builder.InsertCell();
                                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                builder.CellFormat.HorizontalMerge = CellMerge.None;
                                builder.CellFormat.VerticalMerge = CellMerge.None;
                                builder.CellFormat.Width = width;
                                switch (j)
                                {
                                    case 0:
                                        //builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        builder.Write("序号");
                                        break;
                                    case 1:
                                        //builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        builder.Write("日期");
                                        break;
                                    case 2:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.First;
                                        builder.Write("系统");
                                        break;
                                    case 3:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.None;
                                        builder.Write("零点校正液浓度");
                                        break;
                                    case 4:
                                        //builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        builder.Write("绝对误差");
                                        break;
                                    case 5:
                                        //builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        builder.Write("相对误差");
                                        break;
                                    case 6:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.First;
                                        builder.Write("系统");
                                        break;
                                    case 7:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.None;
                                        builder.Write("量程校正液浓度");
                                        break;
                                    case 8:
                                        //builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        builder.Write("相对误差");
                                        break;
                                    case 9:
                                        //builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        builder.Write("相对误差范围");
                                        break;
                                }
                            }
                            builder.EndRow();
                        }
                        else
                        {
                            builder.Font.Bold = true;
                            for (int j = 0; j < 10; j++)
                            {
                                switch (j)
                                {
                                    case 0:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write((i - 2).ToString());
                                        break;
                                    case 1:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(Convert.ToDateTime(dt.Rows[i - 3][j].ToString()).ToString("MM月yy日"));
                                        break;
                                    case 2:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(dt.Rows[i - 3][j].ToString());
                                        break;
                                    case 3:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(dt.Rows[i - 3][j].ToString());
                                        break;
                                    case 4:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write("0.0");
                                        break;
                                    case 5:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write("≤±1.5mg/L");
                                        break;
                                    case 6:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        //builder.CellFormat.Width = 100;
                                        builder.Write(dt.Rows[i - 3][j - 1].ToString());
                                        break;
                                    case 7:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(dt.Rows[i - 3][j - 1].ToString());
                                        break;
                                    case 8:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(dt.Rows[i - 3][j - 1].ToString());
                                        break;
                                    case 9:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        //builder.CellFormat.Width = 100;
                                        builder.Write("≤±10%");
                                        break;
                                }
                            }
                            builder.EndRow();
                        }
                    }
                    builder.EndTable();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
        }
        #endregion
        #region 零点和量程校正液核查测定结果表氨氮
        /// <summary>
        /// 水质自动监测子站运行情况表
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="pointId"></param>
        /// <param name="factors"></param>
        public void MoveToMF2ThreeAD(DocumentBuilder builder, DataTable dt)
        {
            try
            {
                builder.Font.ClearFormatting();
                builder.CellFormat.Borders.LineStyle = LineStyle.Single;
                builder.CellFormat.Borders.Color = System.Drawing.Color.Black;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                builder.Font.Size = 10;
                List<double> widthList = new List<double>();
                double width, widths = 0;
                width = 70;
                widths = 45.380;
                DataView dv = dt.DefaultView;
                List<string> pointId = dt.AsEnumerable().Select(t => t.Field<String>("PointId")).ToList();
                IEnumerable<string> names = pointId.Distinct();
                foreach (string name in names)
                {
                    for (int i = 0; i < dt.Rows.Count + 3; i++)
                    {
                        if (i == 0)
                        {
                            builder.Font.Bold = true;
                            for (int j = 0; j < 10; j++)
                            {
                                builder.InsertCell();
                                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                builder.CellFormat.HorizontalMerge = CellMerge.None;
                                builder.CellFormat.VerticalMerge = CellMerge.None;
                                builder.CellFormat.Width = width;
                                switch (j)
                                {
                                    case 0:
                                        //builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("序号");
                                        break;
                                    case 1:
                                        //builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("日期");
                                        break;
                                    case 2:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.First;
                                        builder.Write("零点校正液核查");
                                        break;
                                    case 3:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.None;
                                        builder.Write("零点校正液核查");
                                        break;
                                    case 4:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.None;
                                        builder.Write("零点校正液核查");
                                        break;
                                    case 5:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                                        builder.Write("零点校正液核查");
                                        break;
                                    case 6:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.First;
                                        builder.Write("量程校正液核查");
                                        break;
                                    case 7:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.None;
                                        builder.Write("量程校正液核查");
                                        break;
                                    case 8:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.None;
                                        builder.Write("量程校正液核查");
                                        break;
                                    case 9:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                                        builder.Write("量程校正液核查");
                                        break;
                                }
                            }
                            builder.EndRow();
                        }
                        else if (i == 1)
                        {
                            builder.Font.Bold = true;
                            for (int j = 0; j < 10; j++)
                            {
                                builder.InsertCell();
                                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                builder.CellFormat.HorizontalMerge = CellMerge.None;
                                builder.CellFormat.VerticalMerge = CellMerge.None;
                                builder.CellFormat.Width = width;
                                switch (j)
                                {
                                    case 0:
                                        //builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        builder.Write("序号");
                                        break;
                                    case 1:
                                        //builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        builder.Write("日期");
                                        break;
                                    case 2:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.First;
                                        builder.Write("测定结果");
                                        break;
                                    case 3:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                                        builder.Write("测定结果(mg/L)");
                                        break;
                                    case 4:
                                        //builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("绝对误差");
                                        break;
                                    case 5:
                                        //builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("绝对误差范围");
                                        break;
                                    case 6:
                                        builder.CellFormat.HorizontalMerge = CellMerge.First;
                                        builder.Write("测定结果");
                                        break;
                                    case 7:
                                        builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                                        break;
                                    case 8:
                                        //builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("相对误差");
                                        break;
                                    case 9:
                                        //builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("相对误差范围");
                                        break;
                                }
                            }
                            builder.EndRow();
                        }
                        else if (i == 2)
                        {
                            builder.Font.Bold = true;
                            for (int j = 0; j < 10; j++)
                            {
                                builder.InsertCell();
                                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                builder.CellFormat.HorizontalMerge = CellMerge.None;
                                builder.CellFormat.VerticalMerge = CellMerge.None;
                                builder.CellFormat.Width = width;
                                switch (j)
                                {
                                    case 0:
                                        //builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        builder.Write("序号");
                                        break;
                                    case 1:
                                        //builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        builder.Write("日期");
                                        break;
                                    case 2:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.First;
                                        builder.Write("系统");
                                        break;
                                    case 3:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.None;
                                        builder.Write("零点校正液浓度");
                                        break;
                                    case 4:
                                        //builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        builder.Write("绝对误差");
                                        break;
                                    case 5:
                                        //builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        builder.Write("相对误差");
                                        break;
                                    case 6:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.First;
                                        builder.Write("系统");
                                        break;
                                    case 7:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.None;
                                        builder.Write("量程校正液浓度");
                                        break;
                                    case 8:
                                        //builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        builder.Write("相对误差");
                                        break;
                                    case 9:
                                        //builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        builder.Write("相对误差范围");
                                        break;
                                }
                            }
                            builder.EndRow();
                        }
                        else
                        {
                            builder.Font.Bold = true;
                            for (int j = 0; j < 10; j++)
                            {
                                switch (j)
                                {
                                    case 0:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write((i - 2).ToString());
                                        break;
                                    case 1:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(Convert.ToDateTime(dt.Rows[i - 3][j].ToString()).ToString("MM月yy日"));
                                        break;
                                    case 2:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(dt.Rows[i - 3][j].ToString());
                                        break;
                                    case 3:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write("0.00");
                                        break;
                                    case 4:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(dt.Rows[i - 3][j].ToString());
                                        break;
                                    case 5:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write("≤±0.20mg/L");
                                        break;
                                    case 6:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        //builder.CellFormat.Width = 100;
                                        builder.Write(dt.Rows[i - 3][j - 1].ToString());
                                        break;
                                    case 7:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(dt.Rows[i - 3][j - 1].ToString());
                                        break;
                                    case 8:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(dt.Rows[i - 3][j - 1].ToString());
                                        break;
                                    case 9:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        //builder.CellFormat.Width = 100;
                                        builder.Write("≤±10%");
                                        break;
                                }
                            }
                            builder.EndRow();
                        }
                    }
                    builder.EndTable();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
        }
        #endregion
        #region 零点和量程校正液核查测定结果表总磷
        /// <summary>
        /// 水质自动监测子站运行情况表
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="pointId"></param>
        /// <param name="factors"></param>
        public void MoveToMF2ThreeZL(DocumentBuilder builder, DataTable dt)
        {
            try
            {
                builder.Font.ClearFormatting();
                builder.CellFormat.Borders.LineStyle = LineStyle.Single;
                builder.CellFormat.Borders.Color = System.Drawing.Color.Black;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                builder.Font.Size = 10;
                List<double> widthList = new List<double>();
                double width, widths = 0;
                width = 70;
                widths = 45.380;
                DataView dv = dt.DefaultView;
                List<string> pointId = dt.AsEnumerable().Select(t => t.Field<String>("PointId")).ToList();
                IEnumerable<string> names = pointId.Distinct();
                foreach (string name in names)
                {
                    for (int i = 0; i < dt.Rows.Count + 3; i++)
                    {
                        if (i == 0)
                        {
                            builder.Font.Bold = true;
                            for (int j = 0; j < 10; j++)
                            {
                                builder.InsertCell();
                                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                builder.CellFormat.HorizontalMerge = CellMerge.None;
                                builder.CellFormat.VerticalMerge = CellMerge.None;
                                builder.CellFormat.Width = width;
                                switch (j)
                                {
                                    case 0:
                                        //builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("序号");
                                        break;
                                    case 1:
                                        //builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("日期");
                                        break;
                                    case 2:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.First;
                                        builder.Write("零点校正液核查");
                                        break;
                                    case 3:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.None;
                                        builder.Write("零点校正液核查");
                                        break;
                                    case 4:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.None;
                                        builder.Write("零点校正液核查");
                                        break;
                                    case 5:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                                        builder.Write("零点校正液核查");
                                        break;
                                    case 6:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.First;
                                        builder.Write("量程校正液核查");
                                        break;
                                    case 7:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.None;
                                        builder.Write("量程校正液核查");
                                        break;
                                    case 8:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.None;
                                        builder.Write("量程校正液核查");
                                        break;
                                    case 9:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                                        builder.Write("量程校正液核查");
                                        break;
                                }
                            }
                            builder.EndRow();
                        }
                        else if (i == 1)
                        {
                            builder.Font.Bold = true;
                            for (int j = 0; j < 10; j++)
                            {
                                builder.InsertCell();
                                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                builder.CellFormat.HorizontalMerge = CellMerge.None;
                                builder.CellFormat.VerticalMerge = CellMerge.None;
                                builder.CellFormat.Width = width;
                                switch (j)
                                {
                                    case 0:
                                        //builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        builder.Write("序号");
                                        break;
                                    case 1:
                                        //builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        builder.Write("日期");
                                        break;
                                    case 2:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.First;
                                        builder.Write("测定结果");
                                        break;
                                    case 3:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                                        builder.Write("测定结果(mg/L)");
                                        break;
                                    case 4:
                                        //builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("绝对误差");
                                        break;
                                    case 5:
                                        //builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("绝对误差范围");
                                        break;
                                    case 6:
                                        builder.CellFormat.HorizontalMerge = CellMerge.First;
                                        builder.Write("测定结果");
                                        break;
                                    case 7:
                                        builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                                        break;
                                    case 8:
                                        //builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("相对误差");
                                        break;
                                    case 9:
                                        //builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("相对误差范围");
                                        break;
                                }
                            }
                            builder.EndRow();
                        }
                        else if (i == 2)
                        {
                            builder.Font.Bold = true;
                            for (int j = 0; j < 10; j++)
                            {
                                builder.InsertCell();
                                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                builder.CellFormat.HorizontalMerge = CellMerge.None;
                                builder.CellFormat.VerticalMerge = CellMerge.None;
                                builder.CellFormat.Width = width;
                                switch (j)
                                {
                                    case 0:
                                        //builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        builder.Write("序号");
                                        break;
                                    case 1:
                                        //builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        builder.Write("日期");
                                        break;
                                    case 2:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.First;
                                        builder.Write("系统");
                                        break;
                                    case 3:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.None;
                                        builder.Write("零点校正液浓度");
                                        break;
                                    case 4:
                                        //builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        builder.Write("绝对误差");
                                        break;
                                    case 5:
                                        //builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        builder.Write("相对误差");
                                        break;
                                    case 6:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.First;
                                        builder.Write("系统");
                                        break;
                                    case 7:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.None;
                                        builder.Write("量程校正液浓度");
                                        break;
                                    case 8:
                                        //builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        builder.Write("相对误差");
                                        break;
                                    case 9:
                                        //builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        builder.Write("相对误差范围");
                                        break;
                                }
                            }
                            builder.EndRow();
                        }
                        else
                        {
                            builder.Font.Bold = true;
                            for (int j = 0; j < 10; j++)
                            {
                                switch (j)
                                {
                                    case 0:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write((i - 2).ToString());
                                        break;
                                    case 1:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(Convert.ToDateTime(dt.Rows[i - 3][j].ToString()).ToString("MM月yy日"));
                                        break;
                                    case 2:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(dt.Rows[i - 3][j].ToString());
                                        break;
                                    case 3:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write("0.000");
                                        break;
                                    case 4:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(dt.Rows[i - 3][j].ToString());
                                        break;
                                    case 5:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write("≤±0.030mg/L");
                                        break;
                                    case 6:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        //builder.CellFormat.Width = 100;
                                        builder.Write(dt.Rows[i - 3][j - 1].ToString());
                                        break;
                                    case 7:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(dt.Rows[i - 3][j - 1].ToString());
                                        break;
                                    case 8:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(dt.Rows[i - 3][j - 1].ToString());
                                        break;
                                    case 9:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        //builder.CellFormat.Width = 100;
                                        builder.Write("≤±10%");
                                        break;
                                }
                            }
                            builder.EndRow();
                        }
                    }
                    builder.EndTable();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
        }
        #endregion
        #region 零点和量程校正液核查测定结果表总氮
        /// <summary>
        /// 水质自动监测子站运行情况表
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="pointId"></param>
        /// <param name="factors"></param>
        public void MoveToMF2ThreeZD(DocumentBuilder builder, DataTable dt)
        {
            try
            {
                builder.Font.ClearFormatting();
                builder.CellFormat.Borders.LineStyle = LineStyle.Single;
                builder.CellFormat.Borders.Color = System.Drawing.Color.Black;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                builder.Font.Size = 10;
                List<double> widthList = new List<double>();
                double width, widths = 0;
                width = 70;
                widths = 45.380;
                DataView dv = dt.DefaultView;
                List<string> pointId = dt.AsEnumerable().Select(t => t.Field<String>("PointId")).ToList();
                IEnumerable<string> names = pointId.Distinct();
                foreach (string name in names)
                {
                    for (int i = 0; i < dt.Rows.Count + 3; i++)
                    {
                        if (i == 0)
                        {
                            builder.Font.Bold = true;
                            for (int j = 0; j < 10; j++)
                            {
                                builder.InsertCell();
                                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                builder.CellFormat.HorizontalMerge = CellMerge.None;
                                builder.CellFormat.VerticalMerge = CellMerge.None;
                                builder.CellFormat.Width = width;
                                switch (j)
                                {
                                    case 0:
                                        //builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("序号");
                                        break;
                                    case 1:
                                        //builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("日期");
                                        break;
                                    case 2:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.First;
                                        builder.Write("零点校正液核查");
                                        break;
                                    case 3:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.None;
                                        builder.Write("零点校正液核查");
                                        break;
                                    case 4:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.None;
                                        builder.Write("零点校正液核查");
                                        break;
                                    case 5:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                                        builder.Write("零点校正液核查");
                                        break;
                                    case 6:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.First;
                                        builder.Write("量程校正液核查");
                                        break;
                                    case 7:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.None;
                                        builder.Write("量程校正液核查");
                                        break;
                                    case 8:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.None;
                                        builder.Write("量程校正液核查");
                                        break;
                                    case 9:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                                        builder.Write("量程校正液核查");
                                        break;
                                }
                            }
                            builder.EndRow();
                        }
                        else if (i == 1)
                        {
                            builder.Font.Bold = true;
                            for (int j = 0; j < 10; j++)
                            {
                                builder.InsertCell();
                                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                builder.CellFormat.HorizontalMerge = CellMerge.None;
                                builder.CellFormat.VerticalMerge = CellMerge.None;
                                builder.CellFormat.Width = width;
                                switch (j)
                                {
                                    case 0:
                                        //builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        builder.Write("序号");
                                        break;
                                    case 1:
                                        //builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        builder.Write("日期");
                                        break;
                                    case 2:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.First;
                                        builder.Write("测定结果");
                                        break;
                                    case 3:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                                        builder.Write("测定结果(mg/L)");
                                        break;
                                    case 4:
                                        //builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("绝对误差");
                                        break;
                                    case 5:
                                        //builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("绝对误差范围");
                                        break;
                                    case 6:
                                        builder.CellFormat.HorizontalMerge = CellMerge.First;
                                        builder.Write("测定结果");
                                        break;
                                    case 7:
                                        builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                                        break;
                                    case 8:
                                        //builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("相对误差");
                                        break;
                                    case 9:
                                        //builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("相对误差范围");
                                        break;
                                }
                            }
                            builder.EndRow();
                        }
                        else if (i == 2)
                        {
                            builder.Font.Bold = true;
                            for (int j = 0; j < 10; j++)
                            {
                                builder.InsertCell();
                                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                builder.CellFormat.HorizontalMerge = CellMerge.None;
                                builder.CellFormat.VerticalMerge = CellMerge.None;
                                builder.CellFormat.Width = width;
                                switch (j)
                                {
                                    case 0:
                                        //builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        builder.Write("序号");
                                        break;
                                    case 1:
                                        //builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        builder.Write("日期");
                                        break;
                                    case 2:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.First;
                                        builder.Write("系统");
                                        break;
                                    case 3:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.None;
                                        builder.Write("零点校正液浓度");
                                        break;
                                    case 4:
                                        //builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        builder.Write("绝对误差");
                                        break;
                                    case 5:
                                        //builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        builder.Write("相对误差");
                                        break;
                                    case 6:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.First;
                                        builder.Write("系统");
                                        break;
                                    case 7:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.None;
                                        builder.Write("量程校正液浓度");
                                        break;
                                    case 8:
                                        //builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        builder.Write("相对误差");
                                        break;
                                    case 9:
                                        //builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        builder.Write("相对误差范围");
                                        break;
                                }
                            }
                            builder.EndRow();
                        }
                        else
                        {
                            builder.Font.Bold = true;
                            for (int j = 0; j < 10; j++)
                            {
                                switch (j)
                                {
                                    case 0:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write((i - 2).ToString());
                                        break;
                                    case 1:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(Convert.ToDateTime(dt.Rows[i - 3][j].ToString()).ToString("MM月yy日"));
                                        break;
                                    case 2:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(dt.Rows[i - 3][j].ToString());
                                        break;
                                    case 3:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write("0.00");
                                        break;
                                    case 4:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(dt.Rows[i - 3][j].ToString());
                                        break;
                                    case 5:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write("≤±0.30mg/L");
                                        break;
                                    case 6:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        //builder.CellFormat.Width = 100;
                                        builder.Write(dt.Rows[i - 3][j - 1].ToString());
                                        break;
                                    case 7:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(dt.Rows[i - 3][j - 1].ToString());
                                        break;
                                    case 8:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(dt.Rows[i - 3][j - 1].ToString());
                                        break;
                                    case 9:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        //builder.CellFormat.Width = 100;
                                        builder.Write("≤±10%");
                                        break;
                                }
                            }
                            builder.EndRow();
                        }
                    }
                    builder.EndTable();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
        }
        #endregion
        #region 24小时零点和量程漂移测定结果表
        /// <summary>
        /// 水质自动监测子站运行情况表
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="pointId"></param>
        /// <param name="factors"></param>
        public void MoveToMF2Four(DocumentBuilder builder, DataTable dt)
        {
            try
            {
                builder.Font.ClearFormatting();
                builder.CellFormat.Borders.LineStyle = LineStyle.Single;
                builder.CellFormat.Borders.Color = System.Drawing.Color.Black;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                builder.Font.Size = 10;
                List<double> widthList = new List<double>();
                double width, widths = 0;
                width = 70;
                widths = 45.380;
                DataView dv = dt.DefaultView;
                List<string> pointId = dt.AsEnumerable().Select(t => t.Field<String>("PointId")).ToList();
                IEnumerable<string> names = pointId.Distinct();
                foreach (string name in names)
                {
                    for (int i = 0; i < dt.Rows.Count + 3; i++)
                    {
                        if (i == 0)
                        {
                            builder.Font.Bold = true;
                            for (int j = 0; j < 10; j++)
                            {
                                builder.InsertCell();
                                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                builder.CellFormat.HorizontalMerge = CellMerge.None;
                                builder.CellFormat.VerticalMerge = CellMerge.None;
                                builder.CellFormat.Width = width;
                                switch (j)
                                {
                                    case 0:
                                        //builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("序号");
                                        break;
                                    case 1:
                                        //builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("日期");
                                        break;
                                    case 2:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.First;
                                        builder.Write("24小时零点漂移");
                                        break;
                                    case 3:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.None;
                                        builder.Write("24小时零点漂移");
                                        break;
                                    case 4:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.None;
                                        builder.Write("24小时零点漂移");
                                        break;
                                    case 5:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                                        builder.Write("24小时零点漂移");
                                        break;
                                    case 6:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.First;
                                        builder.Write("24小时量程漂移");
                                        break;
                                    case 7:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.None;
                                        builder.Write("24小时量程漂移");
                                        break;
                                    case 8:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.None;
                                        builder.Write("24小时量程漂移");
                                        break;
                                    case 9:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                                        builder.Write("24小时量程漂移");
                                        break;
                                }
                            }
                            builder.EndRow();
                        }
                        else if (i == 1)
                        {
                            builder.Font.Bold = true;
                            for (int j = 0; j < 10; j++)
                            {
                                builder.InsertCell();
                                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                builder.CellFormat.HorizontalMerge = CellMerge.None;
                                builder.CellFormat.VerticalMerge = CellMerge.None;
                                builder.CellFormat.Width = width;
                                switch (j)
                                {
                                    case 0:
                                        //builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        builder.Write("序号");
                                        break;
                                    case 1:
                                        //builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        builder.Write("日期");
                                        break;
                                    case 2:
                                        builder.CellFormat.HorizontalMerge = CellMerge.First;
                                        builder.Write("测定结果(mg/L)");
                                        break;
                                    case 3:
                                        builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                                        //builder.Write("测定结果(mg/L)");
                                        break;
                                    case 4:
                                        //builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("相对误差");
                                        break;
                                    case 5:
                                        //builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("相对误差范围");
                                        break;
                                    case 6:
                                        builder.CellFormat.HorizontalMerge = CellMerge.First;
                                        builder.Write("测定结果(mg/L)");
                                        break;
                                    case 7:
                                        builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                                        break;
                                    case 8:
                                        //builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("相对误差");
                                        break;
                                    case 9:
                                        //builder.CellFormat.VerticalMerge = CellMerge.First;
                                        builder.Write("相对误差范围");
                                        break;
                                }
                            }
                            builder.EndRow();
                        }
                        else if (i == 2)
                        {
                            builder.Font.Bold = true;
                            for (int j = 0; j < 10; j++)
                            {
                                builder.InsertCell();
                                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                builder.CellFormat.HorizontalMerge = CellMerge.None;
                                builder.CellFormat.VerticalMerge = CellMerge.None;
                                builder.CellFormat.Width = width;
                                switch (j)
                                {
                                    case 0:
                                        //builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        builder.Write("序号");
                                        break;
                                    case 1:
                                        //builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        builder.Write("日期");
                                        break;
                                    case 2:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.First;
                                        builder.Write("当日");
                                        break;
                                    case 3:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.None;
                                        builder.Write("前一日");
                                        break;
                                    case 4:
                                        //builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        builder.Write("相对误差");
                                        break;
                                    case 5:
                                        //builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        builder.Write("相对误差范围");
                                        break;
                                    case 6:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.First;
                                        builder.Write("当日");
                                        break;
                                    case 7:
                                        //builder.CellFormat.HorizontalMerge = CellMerge.None;
                                        builder.Write("前一日");
                                        break;
                                    case 8:
                                        //builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        builder.Write("相对误差");
                                        break;
                                    case 9:
                                        //builder.CellFormat.VerticalMerge = CellMerge.Previous;
                                        builder.Write("相对误差范围");
                                        break;
                                }
                            }
                            builder.EndRow();
                        }
                        else
                        {
                            builder.Font.Bold = true;
                            for (int j = 0; j < 10; j++)
                            {
                                switch (j)
                                {
                                    case 0:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write((i - 2).ToString());
                                        break;
                                    case 1:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(Convert.ToDateTime(dt.Rows[i - 3][j].ToString()).ToString("MM月yy日"));
                                        break;
                                    case 2:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(dt.Rows[i - 3][j].ToString());
                                        break;
                                    case 3:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(dt.Rows[i - 3][j].ToString());
                                        break;
                                    case 4:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(dt.Rows[i - 3][j].ToString());
                                        break;
                                    case 5:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write("≤±1.5mg/L");
                                        break;
                                    case 6:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        //builder.CellFormat.Width = 100;
                                        builder.Write(dt.Rows[i - 3][j - 1].ToString());
                                        break;
                                    case 7:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(dt.Rows[i - 3][j - 1].ToString());
                                        break;
                                    case 8:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        builder.CellFormat.Width = width;
                                        builder.Write(dt.Rows[i - 3][j - 1].ToString());
                                        break;
                                    case 9:
                                        builder.InsertCell();
                                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                        //builder.CellFormat.Width = 100;
                                        builder.Write("≤±10%");
                                        break;
                                }
                            }
                            builder.EndRow();
                        }
                    }
                    builder.EndTable();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
        }
        #endregion
        #region 方法
        #region Grid绑定
        private void BindGrid()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ImpName", typeof(string));
            dt.Columns.Add("State", typeof(string));
            dt.Columns.Add("CreatTime", typeof(DateTime));
            dt.Columns.Add("Download", typeof(string));
            string tablename = System.Configuration.ConfigurationManager.AppSettings["ExportName"];
            string sql = string.Format(@"select * from {0}", tablename);
            dt = g_DatabaseHelper.ExecuteDataTable(sql, MonitoringBusinessConnection);

            auditLogGrid.DataSource = dt;

            
        }
        #endregion

        /// <summary>
        /// 站点因子联动
        /// </summary>
        protected void pointCbxRsm_SelectedChanged()
        {
            
        }
        #endregion

        /// <summary>
        /// 数据导出格式化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void auditLogGrid_GridExporting(object sender, GridExportingArgs e)
        {
            if (e.ExportType == ExportType.Excel || e.ExportType == ExportType.Word)
            {
                string css = "<style> td { border:solid 0.1pt #000000; }</style>";
                e.ExportOutput = e.ExportOutput.Replace("</head>", css + "</head>");
            }
        }

        protected void auditLogGrid_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                
            }
            catch
            {
            }
        }
        protected void gridRTB_ButtonClick(object sender, RadToolBarEventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                //log.Error(ex.ToString());
            }
        }
        #endregion

    }
}