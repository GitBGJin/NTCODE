using log4net;
using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartEP.WebUI.Pages.EnvAir.SuperStation
{
    /// <summary>
    /// 名称：LadarBMP_new.aspx
    /// 创建人：刘晋
    /// 创建日期：2017-06-12
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：
    /// 激光雷达热力图
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class LadarBMP_new : SmartEP.WebUI.Common.BasePage
    {
        //获取一个日志记录器
        ILog log = LogManager.GetLogger("FileLogging");
        private int scriptFlag;
        /// <summary>
        /// 配置激光雷达存放图片路径
        /// </summary>
        string _filepath = ConfigurationManager.AppSettings["Ladar"].ToString();
        /// <summary>
        /// 消光关键词
        /// </summary>
        string[] key_xg = new string[] { "消光", "extin532", "XiaoGuang" };
        /// <summary>
        /// 退偏关键词
        /// </summary>
        string[] key_tp = new string[] { "退偏", "depol", "TuiPian" };
        /// <summary>
        /// 边界层高度关键词
        /// </summary>
        string[] key_bj = new string[] { "边界层","border","BianJie"};
        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                radlDataType.Items.Add(new ListItem("消光", "extin532"));
                radlDataType.Items.Add(new ListItem("退偏", "depol"));
                radlDataType.Items.Add(new ListItem("边界层", "border"));
                radlDataType.SelectedValue = "extin532";
                if (radlDataType.SelectedValue == "extin532")
                {
                    imgName.InnerText = "消光系数";
                }
                else if (radlDataType.SelectedValue == "border")
                {
                    imgName.InnerText = "边界层高度";
                }
                else if (radlDataType.SelectedValue == "depol")
                {
                    imgName.InnerText = "退偏振度";
                }
                ViewState["count"] = 0;
                ViewState["tableCount"] = 0;
                //初始化控件
                DateTime startdate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-01"));
                DateTime enddate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                rdpstartdate.SelectedDate = startdate;
                rdpenddate.SelectedDate = enddate;
                //初始化数据源
                GetFilesData(startdate, enddate);
            }
        }

        #region 查询事件
        /// <summary>
        /// 查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            if (radlDataType.SelectedValue == "extin532")
            {
                imgName.InnerText = "消光系数532";
                Quality.Value = "extin532";
            }
            else if (radlDataType.SelectedValue == "extin355")
            {
                imgName.InnerText = "消光系数355";
                Quality.Value = "extin355";
            }
            else if (radlDataType.SelectedValue == "border")
            {
                imgName.InnerText = "边界层高度";
                Quality.Value = "height";
            }
            else if (radlDataType.SelectedValue == "depol")
            {
                imgName.InnerText = "退偏振度";
                Quality.Value = "depol";
            }
            ViewState["count"] = 0;
            DateTime startdate = Convert.ToDateTime(rdpstartdate.SelectedDate);
            DateTime enddate = Convert.ToDateTime(rdpenddate.SelectedDate).AddDays(1);
            DtStart.Value=startdate.ToString("yyyy-MM-dd HH:mm:ss");
            DtEnd.Value = enddate.ToString("yyyy-MM-dd HH:mm:ss");
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

            BindCharts();
        }
        private void BindCharts()
        {
            RegisterScript("CreatCharts();");
        }
        /// <summary>
        /// 执行前台脚本
        /// </summary>
        /// <param name="script"></param>
        public void RegisterScript(string script)
        {
            string Text = "<script language=\"javascript\" type = \"text/javascript\">\n" + script + "\n</script>";
            scriptFlag++;
            //base.ClientScript.RegisterStartupScript(base.GetType(), scriptFlag.ToString(), Text);
            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "JS", Text, false);
        }
        #endregion

        #region 组装数据源
        /// <summary>
        /// 组装数据源
        /// </summary>
        public void GetFilesData(DateTime startdate, DateTime enddate)
        {
            string _path = _filepath;//相对路径
            string FullPath = GetMapPath(_path);//获取绝对路径
            string[] filelist = GetFilesList(FullPath, startdate, enddate);

            string[] childfilelist = GetChildFilesList(FullPath, startdate, enddate);
            DataTable dt = new DataTable();
            dt.Columns.Add("FileName", typeof(string));
            dt.Columns.Add("ImageUrl", typeof(string));
            dt.Columns.Add("ImageDate", typeof(string));

            if (childfilelist.Length > 0)
            {
                foreach (string str in childfilelist)
                {
                    if (str.ToLower().Contains("xiaoguang") || str.ToLower().Contains("tuipian") || str.ToLower().Contains("bianjie") || str.ToLower().Contains("extin532") || str.ToLower().Contains("depol") || str.ToLower().Contains("border") || str.ToLower().Contains("消光") || str.ToLower().Contains("退偏") || str.ToLower().Contains("边界层"))
                    {
                        if (str.ToLower().LastIndexOf(radlDataType.SelectedItem.Text) > -1 || str.ToLower().LastIndexOf(radlDataType.SelectedItem.Value) > -1)
                        {
                            DataRow dr = dt.NewRow();
                            dr["FileName"] = str;
                            dr["ImageUrl"] = _path + "/" + str;
                            dr["ImageDate"] = GetDate(str.Substring(0, 8)).ToString("yyyy-MM-dd");
                            dt.Rows.Add(dr);
                        }
                    }
                }
            }
            if (filelist.Length > 0)
            {
                foreach (string str in filelist)
                {
                    if (str.ToLower().Contains("xiaoguang") || str.ToLower().Contains("tuipian") || str.ToLower().Contains("bianjie") || str.ToLower().Contains("消光") || str.ToLower().Contains("退偏") || str.ToLower().Contains("边界层"))
                    {
                        if (radlDataType.SelectedValue == "depol")
                        {
                            if (str.ToLower().LastIndexOf("tuipian") > -1 || str.ToLower().LastIndexOf(radlDataType.SelectedItem.Text) > -1 || str.ToLower().LastIndexOf(radlDataType.SelectedItem.Value) > -1)
                            {
                                DataRow dr = dt.NewRow();
                                dr["FileName"] = str;
                                dr["ImageUrl"] = _path + "/" + str;
                                dr["ImageDate"] = GetDate(str.Substring(0, 8)).ToString("yyyy-MM-dd");
                                dt.Rows.Add(dr);
                            }
                        }
                        else if (radlDataType.SelectedValue == "extin532")
                        {
                            if (str.ToLower().LastIndexOf("xiaoguang") > -1 || str.ToLower().LastIndexOf(radlDataType.SelectedItem.Text) > -1 || str.ToLower().LastIndexOf(radlDataType.SelectedItem.Value) > -1)
                            {
                                DataRow dr = dt.NewRow();
                                dr["FileName"] = str;
                                dr["ImageUrl"] = _path + "/" + str;
                                dr["ImageDate"] = GetDate(str.Substring(0, 8)).ToString("yyyy-MM-dd");
                                dt.Rows.Add(dr);
                            }
                        }
                        else
                        {
                            if (str.ToLower().LastIndexOf("bianjie") > -1 || str.ToLower().LastIndexOf(radlDataType.SelectedItem.Text) > -1 || str.ToLower().LastIndexOf(radlDataType.SelectedItem.Value) > -1)
                            {
                                DataRow dr = dt.NewRow();
                                dr["FileName"] = str;
                                dr["ImageUrl"] = _path + "/" + str;
                                dr["ImageDate"] = GetDate(str.Substring(0, 8)).ToString("yyyy-MM-dd");
                                dt.Rows.Add(dr);
                            }
                        }
                    }
                }
            }
            DataView dv = dt.DefaultView;
            dv.Sort = "ImageDate DESC";
            DataTable dt2 = dv.ToTable();
            //对数据源重新组装，组合日期相同的文件
            DataTable Ndt = new DataTable();
            Ndt.Columns.Add("ImageDate", typeof(string));
            Ndt.Columns.Add("ImageUrl", typeof(string));
            //开始循环遍历
            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                DataRow dr = Ndt.NewRow();
                dr["ImageDate"] = Convert.ToString(dt2.Rows[i]["ImageDate"]);
                dr["ImageUrl"] = Convert.ToString(dt2.Rows[i]["ImageUrl"]);
                if (i < dt2.Rows.Count - 1)
                {
                    if (Convert.ToDateTime(dt2.Rows[i]["ImageDate"]) == Convert.ToDateTime(dt.Rows[i + 1]["ImageDate"]))
                    {
                        dr["ImageUrl"] += ";" + Convert.ToString(dt2.Rows[i + 1]["ImageUrl"]);
                        i++;
                    }
                }
                Ndt.Rows.Add(dr);
            }
            //开始组装Table
            GetTable(Ndt);
        }
        #endregion

        #region 组装Table
        /// <summary>
        /// 组装Table
        /// </summary>
        /// <param name="dt"></param>
        public void GetTable(DataTable dt)
        {
            div1.Controls.Clear();
            if (dt == null || dt.Rows.Count == 0)
            {
                return;
            }
            StringBuilder str = new StringBuilder();
            str.Append("<table id=\"maintable\" class=\"border-table\">");
            //表头
            str.Append("<thead><tr id=\"tr_title\"><th width=\"100%\">绘测日期</th></tr></thead>");
            //统计
            int filecount = 0, datecount = 0;
            datecount = dt.Rows.Count;
            ViewState["tableCount"] = datecount;
            //开始循环加载数据
            str.Append("<tbody>");
            for (int i = 0; i < datecount; i++)
            {
                if (i == 0)
                {
                    str.Append("<tr style=\"background-color: #FFA544;\" id=\"tr_" + i.ToString() + "\" onclick=\"GetSelect(this)\" >");
                }
                else
                {
                    str.Append("<tr id=\"tr_" + i.ToString() + "\" onclick=\"GetSelect(this)\">");
                }
                str.Append("<td><a href=\"#\" style=\"color: #000000;text-decoration: none;\">" + dt.Rows[i]["ImageDate"] + "</a>");
                //判断图片路径，是否有2张
                int imgcount = Convert.ToString(dt.Rows[i]["ImageUrl"]).Split(';').Length;
                filecount += imgcount;
                if (imgcount < 2)
                {
                    str.Append("（" + imgcount.ToString() + "张图片）");
                }
                if (imgcount > 0)
                {
                    str.Append("<input type=\"hidden\" id=\"img_" + i.ToString() + "0\" value=\"" + Convert.ToString(dt.Rows[i]["ImageUrl"]).Split(';')[0] + "\" />");
                    if (i == 0)
                    {
                        LoadImg(Convert.ToString(dt.Rows[i]["ImageUrl"]).Split(';')[0]);
                    }

                    if (imgcount == 2)
                    {
                        str.Append("<input type=\"hidden\" id=\"img_" + i.ToString() + "1\" value=\"" + Convert.ToString(dt.Rows[i]["ImageUrl"]).Split(';')[1] + "\" />");
                        if (i == 0)
                        {
                            LoadImg(Convert.ToString(dt.Rows[i]["ImageUrl"]).Split(';')[1]);
                        }
                    }
                }
                str.Append("</td>");
                str.Append("</tr>");
            }
            //结尾统计
            str.Append("<tr><td>共 " + datecount.ToString() + " 天，" + filecount.ToString() + " 张图片</td></tr>");
            str.Append("</tbody>");
            str.Append("</table>");

            Control ct = ParseControl(str.ToString());
            div1.Controls.Add(ct);
        }
        #endregion

        #region 初始化列表加载图片
        /// <summary>
        /// 初始化列表加载图片
        /// </summary>
        /// <param name="ImageUrl">图片文件路径</param>
        public void LoadImg(string ImageUrl)
        {
            string filename = GetFileName(ImageUrl).Split('.')[0];
            bool isxg = false, istp = false, isbj = false;
            //判断是否消光
            foreach (string str in key_xg)
            {
                if (filename.LastIndexOf(str) > -1)
                {
                    isxg = true;
                    break;
                }
            }
            //判断是否退偏
            foreach (string str in key_tp)
            {
                if (filename.LastIndexOf(str) > -1)
                {
                    istp = true;
                    break;
                }
            }
            foreach (string str in key_bj)
            {
                if (filename.LastIndexOf(str) > -1)
                {
                    isbj = true;
                    break;
                }
            }
            if (isxg)
            {
                
                divImg.Style.Add("background", "url(" + ImageUrl.Replace("~", "../../..") + ")");
                divImg.Style.Add("background-size", "100% 100%");
            }
            if (istp)
            {
                
                divImg.Style.Add("background", "url(" + ImageUrl.Replace("~", "../../..") + ")");
                divImg.Style.Add("background-size", "100% 100%");
            }
            if (isbj)
            {
                
                divImg.Style.Add("background", "url(" + ImageUrl.Replace("~", "../../..") + ")");
                divImg.Style.Add("background-size", "100% 100%");
            }
        }
        #endregion

        #region 获得当前绝对路径
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
        #endregion

        #region 返回文件名，不含路径
        /// <summary>
        /// 返回文件名，不含路径
        /// </summary>
        /// <param name="_filepath">文件相对路径</param>
        /// <returns>string</returns>
        public static string GetFileName(string _filepath)
        {
            return _filepath.Substring(_filepath.LastIndexOf(@"/") + 1);
        }
        #endregion

        #region 判断文件是否存在
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
        #endregion

        #region 获取文件夹内指定时间段内的文件
        /// <summary>
        /// 获取文件夹内指定时间段内的子文件下的文件
        /// </summary>
        /// <param name="fullpath">文件绝对路径</param>
        /// <param name="startdate">开始日期</param>
        /// <param name="enddate">截止日期</param>
        /// <returns></returns>
        public string[] GetChildFilesList(string fullpath, DateTime startdate, DateTime enddate)
        {
            var query = from a in
                            (from f in Directory.GetDirectories(fullpath)
                             let fi = new FileInfo(f)
                             where GetDate(fi.Name) >= startdate && GetDate(fi.Name) <= enddate
                             //orderby GetDate(fi.Name) descending
                             select fi.Name)
                        from mm in Directory.GetFiles(fullpath + "/" + a)
                        let mfi = new FileInfo(mm)
                        where IsOrNotImg(mfi.Name)
                        select a + "/" + mfi.Name
                           ;
            return query.ToArray();
        }
        /// <summary>
        /// 获取文件夹内指定时间段内的文件
        /// </summary>
        /// <param name="fullpath">文件绝对路径</param>
        /// <param name="startdate">开始日期</param>
        /// <param name="enddate">截止日期</param>
        /// <returns></returns>
        public string[] GetFilesList(string fullpath, DateTime startdate, DateTime enddate)
        {
            var query = (from f in Directory.GetFiles(fullpath)
                         let fi = new FileInfo(f)
                         where trueOrFalse(fi.Name) && IsOrNotImg(fi.Name) && GetDate(fi.Name.Substring(0, 8)) >= startdate && GetDate(fi.Name.Substring(0, 8)) <= enddate
                         orderby GetDate(fi.Name.Substring(0, 8)) descending
                         select fi.Name);
            return query.ToArray();
        }
        #endregion
        protected bool IsOrNotImg(string str)
        {
            if (str.ToLower().Contains("bmp") || str.ToLower().Contains("jpeg") || str.ToLower().Contains("jpg") || str.ToLower().Contains("png"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #region 将日期格式的字符串转为日期输出
        /// <summary>
        /// 将日期格式的字符串转为日期输出
        /// </summary>
        /// <param name="str">日期格式的字符串yyyyMMdd</param>
        /// <returns></returns>
        public DateTime GetDate(string str)
        {
            DateTime dt = DateTime.Parse("1900-01-01");
            if (str.Length == 8)
            {
                //IFormatProvider format = new System.Globalization.CultureInfo("zh-CN");
                string FormatStr = "yyyyMMdd";
                dt = DateTime.ParseExact(str, FormatStr, CultureInfo.InvariantCulture);
            }
            return dt;
        }
        #endregion

        protected void timer_Tick(object sender, EventArgs e)
        {
            DateTime startdate = Convert.ToDateTime(rdpstartdate.SelectedDate);
            DateTime enddate = Convert.ToDateTime(rdpenddate.SelectedDate).AddDays(1);
            GetFilesData(startdate, enddate);
            int tableCount = int.TryParse(ViewState["tableCount"].ToString(), out tableCount) ? int.Parse(ViewState["tableCount"].ToString()) : 0;
            int count = int.TryParse(ViewState["count"].ToString(), out count) ? int.Parse(ViewState["count"].ToString()) : 0;
            if (tableCount != 0)
            {
                if (count < tableCount)
                {
                    RegisterScript("onTimer(" + count + ")");
                    count++;
                }
                else
                {
                    count = 0;
                    RegisterScript("onTimer(" + count + ")");
                    count++;
                }
                ViewState["count"] = count;
            }
        }
        protected bool trueOrFalse(string str)
        {
            if (str.ToLower().Contains("xiaoguang") || str.ToLower().Contains("tuipian") || str.ToLower().Contains("bianjie") || str.ToLower().Contains("extin532") || str.ToLower().Contains("depol") || str.ToLower().Contains("border") || str.ToLower().Contains("消光") || str.ToLower().Contains("退偏") || str.ToLower().Contains("边界层"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        protected void stop_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
        }

        //protected void start_Click(object sender, EventArgs e)
        //{
        //    string second = tbSecond.Text == "" ? "3" : tbSecond.Text;
        //    timer.Interval = int.Parse(second) * 1000;
        //    timer.Enabled = true;
        //}
    }
}