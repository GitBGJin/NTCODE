using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.DomainModel.BaseData;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.DataAnalyze.Air.DataQuery;
using SmartEP.Service.DataAnalyze.Report;
using SmartEP.Utilities.Web.UI;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.Report
{
    public partial class RoutineMonthReport : SmartEP.WebUI.Common.BasePage
    {
        DataQueryByDayService m_DataQueryByDayService = new DataQueryByDayService();
        CustomDataService customDataService = new CustomDataService();
        ReportLogService ReportLogService = new ReportLogService();
        DateTime dt = DateTime.Now;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                this.ViewState["DisplayPerson"] = PageHelper.GetQueryString("DisplayPerson");
                InitControl();
            }
        }
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            rmypMonth.SelectedDate = DateTime.Now;
            //根据页面ID、水或气的类别获取数据
            var customData = customDataService.CustomDatumRetrieve("RoutineMonthReport", 1);


            MonitoringPointAirService m_MonitoringPointAirService = Singleton<MonitoringPointAirService>.GetInstance();
            IQueryable<MonitoringPointEntity> ports = m_MonitoringPointAirService.RetrieveAirMPListByCountryControlled();
            string[] pointarry = ports.Select(p => p.MonitoringPointName).ToArray();
            string strpointName = "";
            foreach (string point in pointarry)
            {
                strpointName += point + ";";
            }
            pointCbxRsm.SetPointValuesFromNames(strpointName);
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DateTime dtStart = Convert.ToDateTime(rmypMonth.SelectedDate.Value + "-1");
            string[] portIds = pointCbxRsm.GetPointValues(CbxRsmReturnType.ID);
            string[] factors = { "a01007", "a01008", "a01001", "a01004", "a01006", "a01020", "a21002" };
            DateTime dtBegion = new DateTime(rmypMonth.SelectedDate.Value.Year, rmypMonth.SelectedDate.Value.Month, 1);
            DateTime dtEnd = Convert.ToDateTime(dtBegion.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59"));
            //每页显示数据个数            
            int pageSize = 99999999;
            //当前页的序号
            int pageNo = 0;

            //数据总行数
            int recordTotal = 0;


            string strGuid = "(" + dt.ToString("yyyyMMddHHmmssffff") + ")" + "RoutineMonthReport";
            string strSource = Server.MapPath("Template/air.mdb");
            string strTarget = Server.MapPath("../../../Pages/EnvAir/Report/ReportFile/RoutineMonthReport/" + strGuid + ".mdb");
            string path = Server.MapPath("../../../Pages/EnvAir/Report/ReportFile/RoutineMonthReport/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);//创建新路径
            }
            File.Copy(strSource, strTarget, true);

            string strConnectionString = "Provider=Microsoft.JET.OLEDB.4.0;data source=" + strTarget;

            var myView = m_DataQueryByDayService.GetDayDataPager(portIds, factors, dtBegion, dtEnd, pageSize, pageNo, out recordTotal);
            decimal[] myData = new decimal[28];
            for (int i = 0; i < myView.Count; i++)
            {

                for (int j = 0; j < 28; j++)
                {
                    if (myView[i][j] != null && myView[i][j].ToString() != "")
                    {
                        myData[j] = Convert.ToDecimal(myView[i][j].ToString());
                    }
                    else
                    {
                        myData[j] = -1;
                    }
                }

                access_Insert(strConnectionString, myData[0], myData[1], myData[2], myData[3], myData[4], myData[5], myData[6], myData[7], myData[8], myData[9], myData[10], myData[11], myData[12], myData[13], myData[14], myData[15], myData[16], myData[17], myData[18], myData[19], myData[20], myData[21], myData[22], myData[23], myData[24], myData[25], myData[26], myData[27]);
            }
            string pointCodes = string.Join(";", portIds);
            string[] strPointNames = pointCbxRsm.GetPoints().Select(t => t.PointName).ToArray();//站点名称
            string pointNames = string.Join(";", strPointNames);
            ReportLogEntity customDatum = new ReportLogEntity();
            customDatum.PointIds = pointCodes;//测点Code
            customDatum.PointNames = pointNames;//测点名称
            //customDatum.FactorCodes = factorCodes;//因子Code
            //customDatum.FactorsNames = factorsName;//因子名称
            customDatum.DateTimeRange = dtStart.Year.ToString() + "年" + dtStart.Month.ToString() + "月";
            customDatum.WaterOrAirType = 1;//水或气 0：水，1：气
            customDatum.PageTypeID = "RoutineMonthReport";//页面ID
            customDatum.StartDateTime = dtBegion;
            customDatum.EndDateTime = dtEnd;
            customDatum.CreatUser = this.ViewState["DisplayPerson"].ToString();
            customDatum.ReportName = ("../../../Pages/EnvAir/Report/ReportFile/RoutineMonthReport/" + strGuid + ".mdb").ToString();
            customDatum.CreatDateTime = DateTime.Now;
            //添加数据
            ReportLogService.ReportLogAdd(customDatum);
            FileDownload(strTarget, strGuid + ".mdb");
        }
        public void access_Insert(string accessConn, decimal STCODE, decimal YE, decimal MON, decimal DA, decimal PCODE, decimal SMO, decimal SDA, decimal SHO, decimal SMI, decimal EMO, decimal EDA, decimal EHO, decimal EMI, decimal SO2, decimal NOX, decimal NO2, decimal TSP, decimal CO, decimal PM10, decimal PM25, decimal O3_1, decimal O3_8, decimal WS, decimal WD, decimal TEMP, decimal RH, decimal PRESS, decimal VISIBILITY)
        {
            OleDbConnection myConn = new OleDbConnection(accessConn);
            OleDbCommand myCommand = new OleDbCommand("insert into [air] (STCODE,YE,MON,DA,PCODE,SMO,SDA,SHO,SMI,EMO,EDA,EHO,EMI,SO2,NOX,NO2,TSP,CO,PM10,PM2d5,O3_1,O3_8) values (@m_STCODE,@m_YE,@m_MON,@m_DA,@m_PCODE,@m_SMO,@m_SDA,@m_SHO,@m_SMI,@m_EMO,@m_EDA,@m_EHO,@m_EMI,@m_SO2,@m_NOX,@m_NO2,@m_TSP,@m_CO,@m_PM10,@m_PM2d5,@m_O3_1,@m_O3_8)", myConn);

            myCommand.Parameters.Add(new OleDbParameter("@m_STCODE", OleDbType.Decimal, 9)).Value = STCODE;
            myCommand.Parameters.Add(new OleDbParameter("@m_YE", OleDbType.Decimal, 9)).Value = YE;
            myCommand.Parameters.Add(new OleDbParameter("@m_MON", OleDbType.Decimal, 9)).Value = SMO;//新增
            myCommand.Parameters.Add(new OleDbParameter("@m_DA", OleDbType.Decimal, 9)).Value = SDA;//新增
            myCommand.Parameters.Add(new OleDbParameter("@m_PCODE", OleDbType.Decimal, 9)).Value = PCODE;
            myCommand.Parameters.Add(new OleDbParameter("@m_SMO", OleDbType.Decimal, 9)).Value = SMO;
            myCommand.Parameters.Add(new OleDbParameter("@m_SDA", OleDbType.Decimal, 9)).Value = SDA;
            myCommand.Parameters.Add(new OleDbParameter("@m_SHO", OleDbType.Decimal, 9)).Value = SHO;//6

            myCommand.Parameters.Add(new OleDbParameter("@m_SMI", OleDbType.Decimal, 9)).Value = SMI;
            myCommand.Parameters.Add(new OleDbParameter("@m_EMO", OleDbType.Decimal, 9)).Value = EMO;
            myCommand.Parameters.Add(new OleDbParameter("@m_EDA", OleDbType.Decimal, 9)).Value = EDA;
            myCommand.Parameters.Add(new OleDbParameter("@m_EHO", OleDbType.Decimal, 9)).Value = EHO;
            myCommand.Parameters.Add(new OleDbParameter("@m_EMI", OleDbType.Decimal, 9)).Value = EMI;//5

            myCommand.Parameters.Add(new OleDbParameter("@m_SO2", OleDbType.Decimal, 9)).Value = SO2;
            myCommand.Parameters.Add(new OleDbParameter("@m_NOX", OleDbType.Decimal, 9)).Value = NOX;
            myCommand.Parameters.Add(new OleDbParameter("@m_NO2", OleDbType.Decimal, 9)).Value = NO2;
            myCommand.Parameters.Add(new OleDbParameter("@m_TSP", OleDbType.Decimal, 9)).Value = TSP;
            myCommand.Parameters.Add(new OleDbParameter("@m_CO", OleDbType.Decimal, 9)).Value = CO;
            myCommand.Parameters.Add(new OleDbParameter("@m_PM10", OleDbType.Decimal, 9)).Value = PM10;//6
            myCommand.Parameters.Add(new OleDbParameter("@m_PM2d5", OleDbType.Decimal, 9)).Value = PM25;//新增

            myCommand.Parameters.Add(new OleDbParameter("@m_O3_1", OleDbType.Decimal, 9)).Value = O3_1;//新增
            myCommand.Parameters.Add(new OleDbParameter("@m_O3_8", OleDbType.Decimal, 9)).Value = O3_8;//新增
            //myCommand.Parameters.Add(new OleDbParameter("@m_WS", OleDbType.Decimal, 9)).Value = WS;
            //myCommand.Parameters.Add(new OleDbParameter("@m_WD", OleDbType.Decimal, 9)).Value = WD;
            //myCommand.Parameters.Add(new OleDbParameter("@m_TEMP", OleDbType.Decimal, 9)).Value = TEMP;
            //myCommand.Parameters.Add(new OleDbParameter("@m_RH", OleDbType.Decimal, 9)).Value = RH;//5

            //myCommand.Parameters.Add(new OleDbParameter("@m_PRESS", OleDbType.Decimal, 9)).Value = PRESS;
            //myCommand.Parameters.Add(new OleDbParameter("@m_VISIBILITY", OleDbType.Decimal, 9)).Value = VISIBILITY;

            try
            {
                myConn.Open();
                myCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception("添加记录时产生错误，错误代码为：" + e.ToString());
            }
            finally
            {
                myCommand.Dispose();
                myConn.Close();
                myConn.Dispose();
            }
        }
        ///   <summary>   
        ///   文件下载   
        ///   </summary>   
        ///   <param   name="FullFileName"></param>   
        private void FileDownload(string FullFileName, string FileName)
        {
            FileInfo DownloadFile = new FileInfo(FullFileName);
            Response.Clear();
            Response.ClearHeaders();
            Response.Buffer = false;
            Response.ContentType = "application/ms-access";
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(FileName, System.Text.Encoding.UTF8));
            Response.AppendHeader("Content-Length", DownloadFile.Length.ToString());
            Response.WriteFile(DownloadFile.FullName);
            Response.Flush();
            Response.End();
        }

        protected void btnMonthReport_Click1(object sender, EventArgs e)
        {
            DateTime dtStart = Convert.ToDateTime(rmypMonth.SelectedDate.Value + "-1");
            string[] portIds = pointCbxRsm.GetPointValues(CbxRsmReturnType.ID);
            string[] factors = { "a01007", "a01008", "a01001", "a01004", "a01006", "a01020", "a21002" };
            DateTime dtBegion = new DateTime(rmypMonth.SelectedDate.Value.Year, rmypMonth.SelectedDate.Value.Month, 1);
            DateTime dtEnd = Convert.ToDateTime(dtBegion.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59"));
            //每页显示数据个数            
            int pageSize = 99999999;
            //当前页的序号
            int pageNo = 0;

            //数据总行数
            int recordTotal = 0;


            string strGuid = "(" + dt.ToString("yyyyMMddHHmmssffff") + ")" + "RoutineMonthReport";
            string strSource = Server.MapPath("Template/air.mdb");
            string strTarget = Server.MapPath("../../../Pages/EnvAir/Report/ReportFile/RoutineMonthReport/" + strGuid + ".mdb");
            string path = Server.MapPath("../../../Pages/EnvAir/Report/ReportFile/RoutineMonthReport/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);//创建新路径
            }
            File.Copy(strSource, strTarget, true);

            string strConnectionString = "Provider=Microsoft.JET.OLEDB.4.0;data source=" + strTarget;

            var myView = m_DataQueryByDayService.GetDayDataPager(portIds, factors, dtBegion, dtEnd, pageSize, pageNo, out recordTotal);
            decimal[] myData = new decimal[28];
            for (int i = 0; i < myView.Count; i++)
            {

                for (int j = 0; j < 28; j++)
                {
                    if (myView[i][j] != null && myView[i][j].ToString() != "")
                    {
                        myData[j] = Convert.ToDecimal(myView[i][j].ToString());
                    }
                    else
                    {
                        myData[j] = -1;
                    }
                }

                access_Insert(strConnectionString, myData[0], myData[1], myData[2], myData[3], myData[4], myData[5], myData[6], myData[7], myData[8], myData[9], myData[10], myData[11], myData[12], myData[13], myData[14], myData[15], myData[16], myData[17], myData[18], myData[19], myData[20], myData[21], myData[22], myData[23], myData[24], myData[25], myData[26], myData[27]);
            }
            string pointCodes = string.Join(";", portIds);
            string[] strPointNames = pointCbxRsm.GetPoints().Select(t => t.PointName).ToArray();//站点名称
            string pointNames = string.Join(";", strPointNames);
            ReportLogEntity customDatum = new ReportLogEntity();
            customDatum.PointIds = pointCodes;//测点Code
            customDatum.PointNames = pointNames;//测点名称
            //customDatum.FactorCodes = factorCodes;//因子Code
            //customDatum.FactorsNames = factorsName;//因子名称
            customDatum.DateTimeRange = dtStart.Year.ToString() + "年" + dtStart.Month.ToString() + "月";
            customDatum.WaterOrAirType = 1;//水或气 0：水，1：气
            customDatum.PageTypeID = "RoutineMonthReport";//页面ID
            customDatum.StartDateTime = dtBegion;
            customDatum.EndDateTime = dtEnd;
            customDatum.CreatUser = this.ViewState["DisplayPerson"].ToString();
            customDatum.ExportName = "例行月报表" + string.Format("{0:yyyyMM}", dtStart);
            customDatum.ReportName = ("../../../Pages/EnvAir/Report/ReportFile/RoutineMonthReport/" + strGuid + ".mdb").ToString();
            customDatum.CreatDateTime = DateTime.Now;
            //添加数据
            ReportLogService.ReportLogAdd(customDatum);
        }
        public void bind()
        {
            string[] portIds = pointCbxRsm.GetPointValues(CbxRsmReturnType.ID);
            string[] factors = { "a01007", "a01008", "a01001", "a01004", "a01006", "a01020", "a21002" };
            DateTime dtBegion = new DateTime(rmypMonth.SelectedDate.Value.Year, rmypMonth.SelectedDate.Value.Month, 1);
            DateTime dtEnd = dtBegion.AddMonths(1).AddMilliseconds(-1);
            //每页显示数据个数            
            int pageSize = 99999999;
            //当前页的序号
            int pageNo = 0;

            //数据总行数
            int recordTotal = 0;
            var myView = m_DataQueryByDayService.GetDayDataPager(portIds, factors, dtBegion, dtEnd, pageSize, pageNo, out recordTotal);
            //gridReport.DataSource = myView;
        }

        protected void btnMonthReport_Click(object sender, ImageClickEventArgs e)
        {
            DateTime dtStart = Convert.ToDateTime(rmypMonth.SelectedDate.Value + "-1");
            string[] portIds = pointCbxRsm.GetPointValues(CbxRsmReturnType.ID);
            string[] factors = { "a01007", "a01008", "a01001", "a01004", "a01006", "a01020", "a21002" };
            DateTime dtBegion = new DateTime(rmypMonth.SelectedDate.Value.Year, rmypMonth.SelectedDate.Value.Month, 1);
            DateTime dtEnd = Convert.ToDateTime(dtBegion.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59"));
            //每页显示数据个数            
            int pageSize = 99999999;
            //当前页的序号
            int pageNo = 0;

            //数据总行数
            int recordTotal = 0;

            string factorNames = "二氧化硫;一氧化氮;二氧化氮;氮氧化物;可吸入颗粒物;细颗粒物;臭氧1小时;臭氧8小时;一氧化碳;氟化物;苯并(a)芘;汞;铅;总烃;非甲烷烃;挥发性有机污染物";
            string strGuid = "(" + dt.ToString("yyyyMMddHHmmssffff") + ")" + "RoutineMonthReport";
            string strSource = Server.MapPath("Template/air.mdb");
            string strTarget = Server.MapPath("../../../Pages/EnvAir/Report/ReportFile/RoutineMonthReport/" + rmypMonth.SelectedDate.Value.Year + "/" + rmypMonth.SelectedDate.Value.Month + "/" + strGuid + ".mdb");
            string path = Server.MapPath("../../../Pages/EnvAir/Report/ReportFile/RoutineMonthReport/" + rmypMonth.SelectedDate.Value.Year + "/" + rmypMonth.SelectedDate.Value.Month + "/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);//创建新路径
            }
            File.Copy(strSource, strTarget, true);

            string strConnectionString = "Provider=Microsoft.JET.OLEDB.4.0;data source=" + strTarget;

            var myView = m_DataQueryByDayService.GetDayDataPager(portIds, factors, dtBegion, dtEnd, pageSize, pageNo, out recordTotal);
            myView.Sort = "PCODE asc ,DA asc";
            decimal[] myData = new decimal[28];
            for (int i = 0; i < myView.Count; i++)
            {

                for (int j = 0; j < 28; j++)
                {
                    if (myView[i][j] != null && myView[i][j].ToString() != "")
                    {
                        myData[j] = Convert.ToDecimal(myView[i][j].ToString());
                    }
                    else
                    {
                        myData[j] = -1;
                    }
                }

                access_Insert(strConnectionString, myData[0], myData[1], myData[2], myData[3], myData[4], myData[5], myData[6], myData[7], myData[8], myData[9], myData[10], myData[11], myData[12], myData[13], myData[14], myData[15], myData[16], myData[17], myData[18], myData[19], myData[20], myData[21], myData[22], myData[23], myData[24], myData[25], myData[26], myData[27]);
            }
            string pointCodes = string.Join(";", portIds);
            string[] strPointNames = pointCbxRsm.GetPoints().Select(t => t.PointName).ToArray();//站点名称
            string pointNames = string.Join(";", strPointNames);
            ReportLogEntity customDatum = new ReportLogEntity();
            customDatum.PointIds = pointCodes;//测点Code
            customDatum.PointNames = pointNames;//测点名称
            //customDatum.FactorCodes = factorCodes;//因子Code
            customDatum.FactorsNames = factorNames;//因子名称
            customDatum.DateTimeRange = dtStart.Year.ToString() + "年" + dtStart.Month.ToString() + "月";
            customDatum.WaterOrAirType = 1;//水或气 0：水，1：气
            customDatum.PageTypeID = "RoutineMonthReport";//页面ID
            customDatum.StartDateTime = dtBegion;
            customDatum.EndDateTime = dtEnd;
            customDatum.CreatUser = this.ViewState["DisplayPerson"].ToString();
            customDatum.ExportName = "例行月报表" + string.Format("{0:yyyyMM}", dtStart);
            customDatum.ReportName = ("../../../Pages/EnvAir/Report/ReportFile/RoutineMonthReport/" + rmypMonth.SelectedDate.Value.Year + "/" + rmypMonth.SelectedDate.Value.Month + "/" + strGuid + ".mdb").ToString();
            customDatum.CreatDateTime = DateTime.Now;
            //添加数据
            ReportLogService.ReportLogAdd(customDatum);
            if (!Directory.Exists(strTarget))
            {


                Alert("保存成功！");
                //Directory.CreateDirectory(strTarget);//创建新路径
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "reload", "<script type=\"text/javascript\">self.parent.location.reload();</script>", false);

        }

        //protected void gridReport_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        //{
        //    try
        //    {
        //        GridBoundColumn col = e.Column as GridBoundColumn;
        //        if (col == null)
        //            return;

        //        col.HeaderText = col.DataField;
        //        col.EmptyDataText = "--";
        //        col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        //        col.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
        //        col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
        //        col.HeaderStyle.Width = Unit.Pixel(110);
        //        col.ItemStyle.Width = Unit.Pixel(110);

        //    }
        //    catch (Exception ex) { }
        //}
    }
}