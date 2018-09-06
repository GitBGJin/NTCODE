using SmartEP.Core.Generic;
using SmartEP.Service.Core.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace SmartEP.WebUI
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            try
            {
                //Exception ex = Server.GetLastError();
                //string errorUrl = Request.Url.ToString();
                //SysLog.WriteErrorLog(errorUrl + ":" + ex.Message);
                //HttpException ev = ex as HttpException;

                //Response.Redirect(string.Format(@"{0}/Portal/Error.aspx?ErrorCode={1}", Request.ApplicationPath, 404));
            }
            catch { }
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}