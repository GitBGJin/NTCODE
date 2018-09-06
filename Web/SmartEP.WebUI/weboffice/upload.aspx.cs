using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartEP.WebUI.weboffice
{
    public partial class upload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            string filename = System.Web.HttpUtility.UrlDecode(Request.Params["DocName"]);
            string projectpath = System.Web.HttpUtility.UrlDecode(Request.Params["DocPath"]);
            string filepath = Server.MapPath("~/" + projectpath);
            if (Request.Files.Count > 0)
            {
                HttpPostedFile upfile = Request.Files[0];
                //删除重复文件
                if (File.Exists(filepath))
                {
                    File.Delete(filepath);
                }
                upfile.SaveAs(filepath);
                Response.Write("success");
            }
            else
            {
                Response.Write("error");
            }
            Response.End();
        }
    }
}