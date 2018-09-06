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
    public partial class ImageShow : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 站点服务
        /// </summary>
        MonitoringPointAirService m_MonitoringPointAirService = Singleton<MonitoringPointAirService>.GetInstance();
        /// <summary>
        /// 图片路径
        /// </summary>
        private static string ImageShowUrl = System.Configuration.ConfigurationManager.AppSettings["ImageShowUrl"];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitControl();
            }
        }
        #region 页面控件初始化
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            //IQueryable<MonitoringPointEntity> ports = m_MonitoringPointAirService.RetrieveAirMPListByCountryControlled().OrderBy(t => t.PointId);
            //rcbPoint.DataValueField = "PointId";
            //rcbPoint.DataTextField = "MonitoringPointName";
            //rcbPoint.DataSource = ports;
            //rcbPoint.DataBind();
            //if (rcbPoint.Items.Count > 0)
            //{
            //    rcbPoint.Items[0].Checked = true;
            //}
            //for (int i = 0; i < rcbPoint.Items.Count; i++)
            //{
            //    rcbPoint.Items[i].Checked = true;
            //}
            dayBegin.SelectedDate = DateTime.Now.AddDays(-1).Date;
            dayEnd.SelectedDate = DateTime.Now.Date;
            BindImageGallery();
        }
        #endregion
        #region 绑定RadImageGallery
        /// <summary>
        /// 绑定RadImageGallery
        /// </summary>
        public void BindImageGallery()
        {
            DateTime startdate = dayBegin.SelectedDate.Value.Date;
            DateTime enddate = dayEnd.SelectedDate.Value.Date;
            foreach (RadComboBoxItem item in rcbPoint.CheckedItems)
            {
                for (int i = 0; startdate.AddDays(+i) <= enddate; i++)
                {
                    string _path = ImageShowUrl + item.Value + "/" + startdate.AddDays(+i).Date.ToString("yyyy-MM-dd") + "/";
                    string[] filelist = GetFileList(_path).ToArray();
                    foreach (string fileItem in filelist)
                    {
                        if (fileItem.Contains(".jpg"))
                        {
                            ImageGalleryItem Galleryitem = new ImageGalleryItem();
                            Galleryitem.ImageUrl = @"" + _path + fileItem;
                            Galleryitem.Description = item.Text + fileItem;
                            RadImageGallery2.Items.Add(Galleryitem);
                        }
                    }
                }
            }
        }
        #endregion
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            RadImageGallery2.Rebind();
            RadImageGallery2.Items.Clear();
            RadImageGallery2.CurrentItemIndex = 0;
            BindImageGallery();
        }
        #region 获取文件夹内指定时间段内的文件
        private static List<String> GetFileList(string _path)
        {
            List<string> list = new List<string>();
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(_path);
                HttpWebResponse response = (HttpWebResponse)req.GetResponse();　　//获取响应，即发送请求
                Stream responseStream = response.GetResponseStream();
                using (StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8))
                {
                    string s = streamReader.ReadToEnd();
                    Regex reg = new Regex(@"(?is)<a(?:(?!href=).)*href=(['""]?)(?<url>[^""\s>]*)\1[^>]*>(?<text>(?:(?!</?a\b).)*)</a>");
                    MatchCollection mc = reg.Matches(s);

                    foreach (Match m in mc)
                    {
                        list.Add(m.Groups["text"].Value);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return list;
        }

        /// <summary>
        /// 获取文件夹内指定时间段内的文件
        /// </summary>
        /// <param name="fullpath">文件绝对路径</param>
        /// <param name="startdate">开始日期</param>
        /// <param name="enddate">截止日期</param>
        /// <returns></returns>
        //public string[] GetFilesList(string fullpath, DateTime startdate, DateTime enddate)
        //{
        //    var query = (from f in Directory.GetFiles(fullpath)
        //                 let fi = new FileInfo(f)
        //                 where GetDate(fi.Name.Substring(0, 8)) >= startdate && GetDate(fi.Name.Substring(0, 8)) <= enddate
        //                 orderby GetDate(fi.Name.Substring(0, 8)) descending
        //                 select fi.Name);
        //    return query.ToArray();
        //}
        #endregion
        #region 将日期格式的字符串转为日期输出
        /// <summary>
        /// 将日期格式的字符串转为日期输出
        /// </summary>
        /// <param name="str">日期格式的字符串yyyyMMdd</param>
        /// <returns></returns>
        public DateTime GetDate(string str)
        {
            DateTime dt = DateTime.Parse("1900-01-01");
            if (str.Length == 14)
            {
                //IFormatProvider format = new System.Globalization.CultureInfo("zh-CN");
                string FormatStr = "yyyyMMddHHmmss";
                dt = DateTime.ParseExact(str, FormatStr, CultureInfo.InvariantCulture);
            }
            return dt;
        }
        #endregion
    }
}