using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Reporting;
using SmartEP.ReportLibrary;
using SmartEP.Service.BaseData.MPInfo;
namespace SmartEP.WebUI.Pages.EnvAir.RemoteControl
{
    public partial class DeviationReport : SmartEP.WebUI.Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Report1 report = new Report1();
                string pointid = Request["portName"] != null && !Request["portName"].Equals("") ? GetPointID(HttpUtility.UrlDecode(Request["portName"])) : "2";
                Report1.pointId = pointid;
                Report1.searchtime = Convert.ToDateTime(Request["startTime"] != null && !Request["startTime"].Equals("") ? Request["startTime"] : DateTime.Now.ToString("yyyy-MM-dd 00:00"));
                InstanceReportSource reportSource = new InstanceReportSource();
                reportSource.ReportDocument = report;
                this.ReportViewer1.ReportSource = reportSource;
            }
        }

        public string GetPointID(string PortName)
        {
            MonitoringPointAirService pointAir = new MonitoringPointAirService();
            return pointAir.RetrieveEntityByName(PortName).PointId.ToString();
        }
    }
}