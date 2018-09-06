using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartEP.WebUI.Pages.EnvAir.BaseData
{
    public partial class ExternalLinks : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string url = Request["url"].ToString();
            RedirectAndPOST(this.Page, url);
        }

        private static void RedirectAndPOST(Page page,string url)
        {
            string strForm = PreparePOSTForm(url);
            page.Controls.Add(new LiteralControl(strForm));
        }

        private static String PreparePOSTForm(string url)
        {
            //Set a name for the form
            string formID = "PostForm";
            //Build the form using the specified data to be posted.
            StringBuilder strForm = new StringBuilder();
            strForm.Append("<form id=\"" + formID + "\" name=\"" +
                           formID + "\" action=\"" + url +
                           "\" target=\"_blank\">");

            strForm.Append("</form>");
            StringBuilder strScript = new StringBuilder();
            strScript.Append("<script language='javascript'>");
            strScript.Append("var v" + formID + " = document." +
                             formID + ";");
            strScript.Append("v" + formID + ".submit();");
            strScript.Append("</script>");
            return strForm.ToString() + strScript.ToString();
        }
    }
}