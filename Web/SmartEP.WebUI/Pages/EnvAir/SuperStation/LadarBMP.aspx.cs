using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.SuperStation
{
    public partial class LadarBMP : SmartEP.WebUI.Common.BasePage
    {
        string _filepath = ConfigurationManager.AppSettings["Ladar"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //初始化控件
                rdpstartdate.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-01"));
                rdpenddate.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            }
        }

        /// <summary>
        /// 绑定数据源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RadImageGallery2_NeedDataSource(object sender, ImageGalleryNeedDataSourceEventArgs e)
        {
            RadImageGallery2.DataSource = GetDataTable();
        }

        /// <summary>
        /// 组装数据源
        /// </summary>
        /// <returns>DataView</returns>
        public DataView GetDataTable()
        {
            DateTime startdate = Convert.ToDateTime(rdpstartdate.SelectedDate);
            DateTime enddate = Convert.ToDateTime(rdpenddate.SelectedDate).AddDays(1);
            string filetype = cmbtype.SelectedValue;
            string _path = _filepath;//相对路径
            string FullPath = GetMapPath(_path);//获取绝对路径

            string[] filelist = GetFilesList(FullPath, startdate, enddate, filetype);

            DataTable dt = new DataTable();
            dt.Columns.Add("ImageUrl", typeof(string));
            dt.Columns.Add("Title", typeof(string));
            dt.Columns.Add("Description", typeof(string));

            if (filelist.Length > 0)
            {
                foreach (string str in filelist)
                {
                    DataRow dr = dt.NewRow();
                    dr["ImageUrl"] = _path + "/" + str;
                    dr["Title"] = GetDate(str.Split('.')[0].Substring(0, 8)).ToString("yyyy-MM-dd");
                    dr["Description"] = str;
                    dt.Rows.Add(dr);
                }
            }
            else
            {
                //错误图片需要等确认存放地址后再修改
                string str = "20150101error.bmp";
                DataRow dr = dt.NewRow();
                dr["ImageUrl"] = _path + "/" + str;
                dr["Title"] = "暂无上传图片";
                dr["Description"] = str;
                dt.Rows.Add(dr);
            }

            DataView dataView = dt.AsDataView();
            return dataView;
        }

        #region 文件相关操作

        /// <summary>
        /// 获得当前绝对路径
        /// </summary>
        /// <param name="strPath">指定的路径</param>
        /// <returns>绝对路径</returns>
        public static string GetMapPath(string strPath)
        {
            if (strPath.ToLower().StartsWith("http://"))
            {
                return strPath;
            }
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(strPath);
            }
            else //非web程序引用
            {
                strPath = strPath.Replace("/", "\\");
                if (strPath.StartsWith("\\"))
                {
                    strPath = strPath.Substring(strPath.IndexOf('\\', 1)).TrimStart('\\');
                }
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        }

        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <param name="_filepath">文件相对路径</param>
        /// <returns>bool</returns>
        public static bool FileExists(string _filepath)
        {
            string fullpath = GetMapPath(_filepath);
            if (File.Exists(fullpath))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取文件夹内指定时间段内的文件
        /// </summary>
        /// <param name="fullpath">文件绝对路径</param>
        /// <param name="startdate">开始日期</param>
        /// <param name="enddate">截止日期</param>
        /// <param name="filetype">文件类型</param>
        /// <returns></returns>
        public string[] GetFilesList(string fullpath, DateTime startdate, DateTime enddate, string filetype)
        {
            var query = (from f in Directory.GetFiles(fullpath)
                         let fi = new FileInfo(f)
                         //where fi.LastWriteTime >= startdate && fi.LastWriteTime <= enddate
                         where GetDate(fi.Name.Split('.')[0].Substring(0, 8)) >= startdate && GetDate(fi.Name.Split('.')[0].Substring(0, 8)) < enddate && fi.Name.Split('.')[0].Substring(8, 2) == filetype
                         orderby fi.CreationTime descending
                         select fi.Name);
            return query.ToArray();
        }

        /// <summary>
        /// 将图片转为24/16位BMP格式
        /// </summary>
        /// <param name="_filepath">文件路径</param>
        public void SaveImageBMP(string _filepath)
        {
            //判断文件是否存在
            if (FileExists(_filepath))
            {
                #region 需要文件转码
                //string filename = _filepath.Substring(_filepath.LastIndexOf(@"/") + 1);
                //string new_filename = "n_" + filename;
                //string n_filepath = _filepath.Replace(filename, new_filename);
                ////判断转码新文件是否存在
                //if (!FileExists(n_filepath))
                //{
                //    //开始转码，转换为24为bmp
                //    using (Bitmap source = new Bitmap(GetMapPath(_filepath)))
                //    {
                //        using (Bitmap bmp = new Bitmap(source.Width, source.Height, System.Drawing.Imaging.PixelFormat.Format16bppRgb555))//Format16bppRgb555
                //        {
                //            using (Graphics g = Graphics.FromImage(bmp))
                //            {
                //                g.DrawImage(source, new Rectangle(0, 0, source.Width, source.Height));
                //                bmp.Save(GetMapPath(n_filepath), System.Drawing.Imaging.ImageFormat.Bmp);
                //            }
                //        }
                //    }
                //}
                #endregion
            }
        }

        #endregion

        #region 查询事件
        /// <summary>
        /// 查询按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button1_Click(object sender, EventArgs e)
        {
            DateTime startdate = Convert.ToDateTime(rdpstartdate.SelectedDate);
            DateTime enddate = Convert.ToDateTime(rdpenddate.SelectedDate).AddDays(1);
            string filetype = cmbtype.SelectedValue;
            if (startdate.ToString("yyyy-MM-dd") == "1900-01-01")
            {
                Alert("请输入查询开始日期！");
                return;
            }
            if (enddate.ToString("yyyy-MM-dd") == "1900-01-01")
            {
                Alert("请输入查询截止日期！");
                return;
            }
            if (enddate < startdate)
            {
                Alert("请输入合法的查询开始、截止日期！");
                return;
            }

            RadImageGallery2.Rebind();
        }

        /// <summary>
        /// 将日期格式的字符串转为日期输出
        /// </summary>
        /// <param name="str">日期格式的字符串yyyyMMdd</param>
        /// <returns></returns>
        public DateTime GetDate(string str)
        {
            DateTime dt = DateTime.Parse("1900-01-01"); ;
            //验证8位数字的正则表达式
            Regex reg = new Regex("^\\d{8}$");
            if (reg.IsMatch(str) && str.Length == 8)
            {
                string year = str.Substring(0, 4);
                string month = str.Substring(4, 2);
                string day = str.Substring(6, 2);
                try
                {
                    dt = Convert.ToDateTime(year + "-" + month + "-" + day);
                }
                catch (Exception ex)
                {
                }
            }
            return dt;
        }
        #endregion
    }
}