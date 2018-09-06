using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Com.Sinoyd;
using SmartEP.ReportLibrary;


namespace SmartEP.WebUI.Pages.EnvAir.Report
{
    public partial class ShowVillageWeekReport : System.Web.UI.Page
    {
        private DateTime beginTime;
        private DateTime endTime;
        private string titleText;
        private string factorName;
        private string factorCode;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["beginTime"] != null)
                    beginTime = DateTime.Parse(Request.QueryString["beginTime"].ToString());
                if (Request.QueryString["endTime"] != null)
                    endTime = DateTime.Parse(Request.QueryString["endTime"].ToString());
                if (Request.QueryString["titleText"] != null)
                    titleText = Request.QueryString["titleText"].ToString();
                if (Request.QueryString["factorName"] != null)
                    factorName = Request.QueryString["factorName"].ToString();
                if (Request.QueryString["factorCode"] != null)
                    factorCode = Request.QueryString["factorCode"].ToString();

                string topTitle = "各市、区" + factorName + "浓度及比较情况统计表(周报)";

                string column1 = factorName + "浓度";
                string subColumn1 = beginTime.AddYears(-1).Year + "年";
                string subColumn2 = beginTime.Year + "年";
                string subColumn3 = "与" + beginTime.AddYears(-1).Year + "年比较";

                VillageWeekRep rv = new VillageWeekRep();
                rv.ReportParameters.Add("beginTime", Telerik.Reporting.ReportParameterType.DateTime, beginTime);
                rv.ReportParameters.Add("endTime", Telerik.Reporting.ReportParameterType.DateTime, endTime);
                rv.ReportParameters.Add("titleText", Telerik.Reporting.ReportParameterType.String, titleText);
                rv.ReportParameters.Add("factorName", Telerik.Reporting.ReportParameterType.String, factorName);
                rv.ReportParameters.Add("factorCode", Telerik.Reporting.ReportParameterType.String, factorCode);
                rv.ReportParameters.Add("top", Telerik.Reporting.ReportParameterType.String, topTitle);
                rv.ReportParameters.Add("column1", Telerik.Reporting.ReportParameterType.String, column1);
                rv.ReportParameters.Add("subColumn1", Telerik.Reporting.ReportParameterType.String, subColumn1);
                rv.ReportParameters.Add("subColumn2", Telerik.Reporting.ReportParameterType.String, subColumn2);
                rv.ReportParameters.Add("subColumn3", Telerik.Reporting.ReportParameterType.String, subColumn3);

                this.ReportViewer1.ReportSource = rv;
                ReportViewer1.Report.DocumentName = "各市、区" + factorName + "浓度及比较情况统计表(周报)";
                ReportViewer1.ShowPrintButton = false;
                ReportViewer1.ShowPrintPreviewButton = false;





            }
        }

    }
}