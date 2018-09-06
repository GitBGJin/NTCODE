using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmartEP.Service.ReportLibrary.Air;

namespace SmartEP.WebUI.Pages.EnvAir.Report
{
    public partial class EvaluationReport : SmartEP.WebUI.Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitControl();//初始化控件
            }
        }

        #region 初始化
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            dtpBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.Date.AddMonths(-1).ToString("yyyy-MM-01"));//初始化时间
            dtpEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.Date.ToString("yyyy-MM-01")).AddDays(-1);//初始化时间
        }
        #endregion

        #region 方法
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            BindReport();
            RegisterScript(" SetHeigth();");
        }

        private void BindReport()
        {
            string[] portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);

            Telerik.Reporting.InstanceReportSource instanceReportSource = new Telerik.Reporting.InstanceReportSource();
            EvaluationReportService report = new EvaluationReportService();
            report.ReportParameters.Add("BeginTime", Telerik.Reporting.ReportParameterType.DateTime, Convert.ToDateTime(dtpBegin.SelectedDate.Value.Date));
            report.ReportParameters.Add("EndTime", Telerik.Reporting.ReportParameterType.DateTime,  Convert.ToDateTime(dtpEnd.SelectedDate.Value.Date));
            report.ReportParameters.Add("portId", Telerik.Reporting.ReportParameterType.String, string.Join(",",portIds));

            instanceReportSource.ReportDocument = report;
            this.ReportViewer1.ReportSource = instanceReportSource;
        }
        #endregion

        #region 事件
        protected void ReportViewer1_Load(object sender, EventArgs e)
        {
            BindReport();
        }
        #endregion
    }
}