using SmartEP.Core.Generic;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.MPInfo;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.DataAnalyze
{  
    /// <summary>
    /// 空气的查看图片页面
    /// </summary>
    public partial class MyImage : SmartEP.WebUI.Common.BasePage
    {
        private string m_MN;
        private DateTime m_DT;
        /// <summary>
        /// 图片路径
        /// </summary>
        private static string ImageShowUrl = System.Configuration.ConfigurationManager.AppSettings["ImageUrl"];//获得根目录的图片路径,配置在根目录            下面      
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string MN = Request.QueryString["MN"].ToString();//获得MN号
                //string MN = "77777777777777";
                DateTime datetime = Convert.ToDateTime(Request.QueryString["DT"]);//获得日期
                //DateTime datetime = Convert.ToDateTime("2016-12-01 00:00:00");
                this.m_MN = MN;
                this.m_DT = datetime;
                InitControl();
            }
        }
        #region 页面控件初始化
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            BindImageGallery();
        }
        #endregion
        #region 绑定RadImageGallery
        /// <summary>
        /// 绑定RadImageGallery
        /// </summary>
        public void BindImageGallery()
        {
            string _path = ImageShowUrl + "Air" + "/" + m_MN + "/" + m_DT.Date.ToString("yyyy") + "/" + m_DT.Date.ToString("MM") + "/" + m_DT.Date.ToString("dd") + "/" + m_DT.Date.ToString("HHmmss");    //拼接路径，图片日期精确到秒。
            RadImageGallery2.ImagesFolderPath = _path;
         }
        #endregion
      }
    }
