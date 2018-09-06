using SmartEP.Service.DataAuditing.AuditInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartEP.WebUI.Pages.EnvAir.Report
{
    /// <summary>
    /// ReportDBFHandler 的摘要说明
    /// </summary>
    public class ReportDBFHandler : IHttpHandler
    {
        AuditDataService auditDataService = new AuditDataService();
        public void ProcessRequest(HttpContext context)
        {
              

                    string[] PointID = context.Request["PointID"].ToString().Split(';');
                    DateTime BeginTime = Convert.ToDateTime(context.Request["StartTime"].ToString());
                    DateTime EndTime = Convert.ToDateTime(context.Request["EndTime"].ToString());
                    bool isSuccess = auditDataService.SubmitAudit( PointID, BeginTime, EndTime);
                    context.Response.ContentType = "text/plain";
                    context.Response.Write(isSuccess);
              
        }



        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}