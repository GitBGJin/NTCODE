using SmartEP.WebUI.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartEP.WebUI.Pages.EnvAir.Report
{
    public partial class AirMonthReportEdit : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        #region 判断文件是否存在
        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <returns>success/error</returns>
        public string FileExists()
        {
            string filename = System.Web.HttpUtility.UrlDecode(Convert.ToString(Request["filename"]));
            //string filename = "空气月报201511.doc";
            string _filepath = "Files/TempFile/Word/AirMonthReport/DownLoad/" + filename;
            string fullpath = Server.MapPath("~/" + _filepath);
            if (File.Exists(fullpath))
            {
                return fullpath.Replace("\\", "\\\\");
            }
            return "";
        }
        #endregion
    }
}