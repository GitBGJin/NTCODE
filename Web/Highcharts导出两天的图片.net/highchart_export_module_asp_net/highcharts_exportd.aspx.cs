﻿using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using Svg;
using System.Drawing.Imaging;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text;
using System.Threading;
using log4net;

namespace highchart_export_module_asp_net
{
    public partial class highcharts_exportd : System.Web.UI.Page
    {
        public string tSvg;
        ILog log = LogManager.GetLogger("App.Logging");//获取一个日志记录器
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                //log.Info("保存图片开始");
                if (DateTime.Now.Minute >= 1 && DateTime.Now.Minute < 20)
                {
                    log.Info("保存退偏图开始");
                    saveImage();
                }
                
            }
        }
        /// <summary>
        /// 保存图像
        /// </summary>
        public void saveImage()
        {
            try
            {
                if (Request.Form["type"] != null && Request.Form["svg"] != null && Request.Form["filename"] != null)
                {
                    string tType = Request.Form["type"].ToString();
                    string tSvg = Request.Form["svg"].ToString();
                    //log.Info("字符串：///////////"+tSvg+"////////////");
                    string tFileName = Request.Form["filename"].ToString();
                    if (tFileName == "")
                    {
                        tFileName = "chart" + DateTime.Now.ToString("yyyyMMddHH")+"-tuipian";
                    }
                    else
                    {
                        tFileName = tFileName + DateTime.Now.ToString("yyyyMMddHH") + "-tuipian";
                    }

                    string savePath = Server.MapPath("image/");

                    if (!Directory.Exists(savePath))
                    {
                        Directory.CreateDirectory(savePath);
                    }
                    savePath += tFileName + ".svg";
                    if (!File.Exists(savePath))
                    {
                        System.IO.StreamWriter sw = new System.IO.StreamWriter(savePath, true, Encoding.UTF8);
                        sw.WriteLine(tSvg);
                        sw.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
        }
    }
}