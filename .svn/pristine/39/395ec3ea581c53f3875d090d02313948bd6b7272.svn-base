using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.Service.AutoMonitoring.Air;
using SmartEP.Service.DataAnalyze.Air.DataQuery;
using SmartEP.Utilities.Calendar;
using SmartEP.Utilities.IO;
using SmartEP.Utilities.Web.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartEP.WebUI.Pages.EnvAir.Special
{
    public partial class GranuleSpecial2 : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 日数据接口
        /// </summary>
        GranuleSpecialService g_GranuleSpecial = new GranuleSpecialService();
        InfectantBy1Service m_Min1Data = Singleton<InfectantBy1Service>.GetInstance();
        /// <summary>
        /// 选择因子
        /// </summary>
        private IList<IPollutant> pfactors = null;
        /// <summary>
        /// 配置城市摄影存放图片路径
        /// </summary>
        string _filepath = ConfigurationManager.AppSettings["UrbanPhoto"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitControl();
            }
        }

        ///// <summary>
        ///// 初始化控件
        ///// </summary>
        private void InitControl()
        {
            string factors = System.Configuration.ConfigurationManager.AppSettings["AirPollutantName"];
            factorCbxRsm.SetFactorValuesFromNames(factors);
            //数据类型
            radlDataType.Items.Add(new ListItem("小时", PollutantDataType.Hour.ToString()));
            radlDataType.Items.Add(new ListItem("日", PollutantDataType.Day.ToString()));
            radlDataType.Items.Add(new ListItem("月", PollutantDataType.Month.ToString()));

            radlDataTypeOri.Items.Add(new ListItem("一分钟", PollutantDataType.Min1.ToString()));
            radlDataTypeOri.Items.Add(new ListItem("五分钟", PollutantDataType.Min5.ToString()));
            radlDataTypeOri.Items.Add(new ListItem("小时", PollutantDataType.Min60.ToString()));
            radlDataTypeOri.Items.Add(new ListItem("日", PollutantDataType.OriDay.ToString()));
            radlDataTypeOri.Items.Add(new ListItem("月", PollutantDataType.OriMonth.ToString()));

            radlDataType.SelectedValue = PollutantDataType.Hour.ToString();
            radlDataTypeOri.SelectedValue = PollutantDataType.Min60.ToString();

            //时间框初始化
            hourBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddHours(-47));
            hourEnd.SelectedDate = DateTime.Now;
            dtpBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm"));
            dtpEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            dayBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd"));
            dayEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
            monthBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddYears(-1).ToString("yyyy-MM"));
            monthEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToString("yyyy-MM"));


            dtpHour.Visible = true;
            dbtHour.Visible = false;
            dbtDay.Visible = false;
            dbtMonth.Visible = false;
            BindData();


            ViewState["count"] = 0;
            ViewState["tableCount"] = 0;
            //初始化控件
            DateTime startdate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-01"));
            DateTime enddate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));

            //初始化数据源
            GetFilesData(startdate, enddate);
        
        }
        ///// <summary>
        ///// 绑定数据
        ///// </summary>
        private void BindData()
        {
            string WaterPortId = System.Configuration.ConfigurationManager.AppSettings["WeatherFactor"];
            string[] factors = WaterPortId.Split(',');
            string[] portIds = pointCbxRsm.GetPointValues(CbxRsmReturnType.ID);
            pfactors = factorCbxRsm.GetFactors();
            DataTable dtOneMin = new DataTable();
            if (portIds != null && factors != null)
            {
                string pointId = portIds[0];
                if (ddlDataSource.SelectedValue == "OriData")
                {
                    DateTime dtBegion = dtpBegin.SelectedDate.Value;
                    DateTime dtEnd = dtpEnd.SelectedDate.Value;
                    if (radlDataTypeOri.SelectedValue == "OriDay")
                    {
                        dtOneMin = g_GranuleSpecial.GetOriDayData(pointId, factors, dtBegion, dtEnd).ToTable();
                        SetHiddenData(portIds, pfactors, dtBegion, dtEnd);
                    }
                    if (radlDataTypeOri.SelectedValue == "OriMonth")
                    {
                        dtOneMin = g_GranuleSpecial.GetOriMonthData(pointId, factors, dtBegion, dtEnd).ToTable();
                        SetHiddenData(portIds, pfactors, dtBegion, dtEnd);
                    }
                    if (radlDataTypeOri.SelectedValue == "Min1" || radlDataTypeOri.SelectedValue == "Min5" || radlDataTypeOri.SelectedValue == "Min60")
                    {
                        dtOneMin = g_GranuleSpecial.GetOriHourData(pointId, factors, dtBegion, dtEnd, radlDataTypeOri.SelectedValue).ToTable();
                        SetHiddenData(portIds, pfactors, dtBegion, dtEnd);
                    }
                }
                if (ddlDataSource.SelectedValue == "AuditData")
                {
                    //小时数据
                    if (radlDataType.SelectedValue == "Hour")
                    {
                        DateTime dtBegion = Convert.ToDateTime(hourBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:00"));
                        DateTime dtEnd = Convert.ToDateTime(hourEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:00"));
                        dtOneMin = g_GranuleSpecial.GetAuditHourData(pointId, factors, dtBegion, dtEnd).ToTable();
                        SetHiddenData(portIds, pfactors, dtBegion, dtEnd);

                    }
                    //日数据
                    else if (radlDataType.SelectedValue == "Day")
                    {
                        DateTime dtBegion = dayBegin.SelectedDate.Value;
                        DateTime dtEnd = dayEnd.SelectedDate.Value;
                        dtOneMin = g_GranuleSpecial.GetAuditDayData(pointId, factors, dtBegion, dtEnd).ToTable();
                        SetHiddenData(portIds, pfactors, dtBegion, dtEnd);
                    }
                    //月数据
                    else if (radlDataType.SelectedValue == "Month")
                    {
                        int monthB = monthBegin.SelectedDate.Value.Year;
                        int monthE = monthEnd.SelectedDate.Value.Year;
                        int monthF = monthBegin.SelectedDate.Value.Month;
                        int monthT = monthEnd.SelectedDate.Value.Month;
                        DateTime dtBegion = monthBegin.SelectedDate.Value;
                        DateTime dtEnd = monthEnd.SelectedDate.Value;
                        dtOneMin = g_GranuleSpecial.GetAuditMonthData(pointId, factors, dtBegion, dtEnd).ToTable();
                        SetHiddenData(portIds, pfactors, dtBegion, dtEnd);
                    }
                }

            }
            hdAirWeather.Value = JsonHelper.ToJson(dtOneMin);
        }
        /// <summary>
        /// 页面隐藏域控件赋值（小时、日），将数据需要的参数放入隐藏域，各个参数间用‘|’分割，每个参数内部用‘；’分割
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtBegin"></param>
        /// <param name="dtEnd"></param>
        private void SetHiddenData(string[] portIds, IList<IPollutant> factors, DateTime dtBegin, DateTime dtEnd)
        {
            if (ddlDataSource.SelectedValue == "OriData")
            {
                HiddenData.Value = string.Join(";", portIds) + "|" + string.Join(";", factors.Select(p => p.PollutantCode).ToArray())
                                 + "|" + dtBegin + "|" + dtEnd + "|" + radlDataTypeOri.SelectedValue + "|Air";

            }
            if (ddlDataSource.SelectedValue == "AuditData")
            {
                HiddenData.Value = string.Join(";", portIds) + "|" + string.Join(";", factors.Select(p => p.PollutantCode).ToArray())
                                 + "|" + dtBegin + "|" + dtEnd + "|" + radlDataType.SelectedValue + "|Air";

            }
        }
        /// <summary>
        /// 审核数据数据类型时间框选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void radlDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //小时数据
            if (radlDataType.SelectedValue == "Hour")
            {
                dtpHour.Visible = false;
                dbtHour.Visible = true;
                dbtDay.Visible = false;
                dbtMonth.Visible = false;
            }
            //日数据
            else if (radlDataType.SelectedValue == "Day")
            {
                dtpHour.Visible = false;
                dbtDay.Visible = true;
                dbtHour.Visible = false;
                dbtMonth.Visible = false;
            }
            //周数据
            else if (radlDataType.SelectedValue == "Week")
            {
                dtpHour.Visible = false;
                dbtMonth.Visible = false;
                dbtDay.Visible = false;
                dbtHour.Visible = false;
            }
            //月数据
            else if (radlDataType.SelectedValue == "Month")
            {
                dtpHour.Visible = false;
                dbtMonth.Visible = true;
                dbtDay.Visible = false;
                dbtHour.Visible = false;
            }
            //季数据
            else if (radlDataType.SelectedValue == "Season")
            {
                dtpHour.Visible = false;
                dbtMonth.Visible = false;
                dbtDay.Visible = false;
                dbtHour.Visible = false;

            }
            //年数据
            else if (radlDataType.SelectedValue == "Year")
            {
                dtpHour.Visible = false;
                dbtMonth.Visible = false;
                dbtDay.Visible = false;
                dbtHour.Visible = false;
            }
        }
        /// <summary>
        /// 原始数据数据类型时间框选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void radlDataTypeOri_SelectedIndexChanged(object sender, EventArgs e)
        {
            //一分钟数据
            if (radlDataTypeOri.SelectedValue == "Min1")
            {
                dtpHour.Visible = true;
                dbtHour.Visible = false;
                dbtDay.Visible = false;
                dbtMonth.Visible = false;
                dtpBegin.DateInput.DateFormat = "yyyy-MM-dd HH:mm";
                dtpEnd.DateInput.DateFormat = "yyyy-MM-dd HH:mm";
            }
            //五分钟数据
            if (radlDataTypeOri.SelectedValue == "Min5")
            {
                dtpHour.Visible = true;
                dbtHour.Visible = false;
                dbtDay.Visible = false;
                dbtMonth.Visible = false;
                dtpBegin.DateInput.DateFormat = "yyyy-MM-dd HH:mm";
                dtpEnd.DateInput.DateFormat = "yyyy-MM-dd HH:mm";
            }
            //小时数据
            if (radlDataTypeOri.SelectedValue == "Min60")
            {
                dtpHour.Visible = true;
                dbtHour.Visible = false;
                dbtDay.Visible = false;
                dbtMonth.Visible = false;
                dtpBegin.DateInput.DateFormat = "yyyy-MM-dd HH:00";
                dtpEnd.DateInput.DateFormat = "yyyy-MM-dd HH:00";
            }
            //日数据
            if (radlDataTypeOri.SelectedValue == "OriDay")
            {
                dtpHour.Visible = true;
                dbtHour.Visible = false;
                dbtDay.Visible = false;
                dbtMonth.Visible = false;
                dtpBegin.DateInput.DateFormat = "yyyy-MM-dd";
                dtpEnd.DateInput.DateFormat = "yyyy-MM-dd";
            }
            //月数据
            if (radlDataTypeOri.SelectedValue == "OriMonth")
            {
                dtpHour.Visible = true;
                dbtHour.Visible = false;
                dbtDay.Visible = false;
                dbtMonth.Visible = false;
                dtpBegin.DateInput.DateFormat = "yyyy-MM";
                dtpEnd.DateInput.DateFormat = "yyyy-MM";
            }
        }
        /// <summary>
        /// 数据来源选项变化，数据类型选项相应变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlDataSource_SelectedIndexChanged(object sender, Telerik.Web.UI.DropDownListEventArgs e)
        {
            if (ddlDataSource.SelectedIndex == 0)
            {
                radlDataTypeOri.Visible = true;
                radlDataType.Visible = false;
                radlDataTypeOri.SelectedIndex = 0;
            }
            else
            {
                radlDataTypeOri.Visible = false;
                radlDataType.Visible = true;
                radlDataType.SelectedIndex = 0;
            }
        }


        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            BindData();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ShowECharts(); ", true);
            RegisterScript("RefreshChart();");
            ViewState["count"] = 0;
            DateTime startdate = DateTime.Now;
            DateTime enddate = DateTime.Now; ;
            if (ddlDataSource.SelectedValue == "OriData")
            {
                startdate = dtpBegin.SelectedDate.Value;
                enddate = dtpEnd.SelectedDate.Value;

            }
            if (ddlDataSource.SelectedValue == "AuditData")
            {
                //小时数据
                if (radlDataType.SelectedValue == "Hour")
                {
                    startdate = Convert.ToDateTime(hourBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:00"));
                    enddate = Convert.ToDateTime(hourEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:00"));

                }
                //日数据
                else if (radlDataType.SelectedValue == "Day")
                {
                    startdate = dayBegin.SelectedDate.Value;
                    enddate = dayEnd.SelectedDate.Value;
                }
                //月数据
                else if (radlDataType.SelectedValue == "Month")
                {
                    int monthB = monthBegin.SelectedDate.Value.Year;
                    int monthE = monthEnd.SelectedDate.Value.Year;
                    int monthF = monthBegin.SelectedDate.Value.Month;
                    int monthT = monthEnd.SelectedDate.Value.Month;
                    startdate = monthBegin.SelectedDate.Value;
                    enddate = monthEnd.SelectedDate.Value;
                }
            }
            GetFilesData(startdate, enddate);
            BindGrid();
        }
        //public string[] GetFilesList(string fullpath, DateTime startdate, DateTime enddate)
        //    {

        //        TimeSpan days = enddate.Subtract(startdate);
        //        IEnumerable<string> query = new List<string> { };
        //        int day = days.Days;
        //        int hour = days.Hours;
        //        if (day > 0)
        //        {
        //            for (int j = 0; j < day; j++)
        //            {
        //                string fullpaths = fullpath + "\\" + startdate.AddDays(j).ToString("yyyy-MM-dd");
        //                if (Directory.Exists(fullpaths))
        //                {
        //                    query = (from f in Directory.GetFiles(fullpaths)
        //                             let fi = new FileInfo(f)
        //                             where trueOrFalse(fi.Name) && IsOrNotImg(fi.Name) && GetDate(fi.Name.Substring(0, 10)) >= startdate && GetDate(fi.Name.Substring(0, 10)) <= enddate
        //                             orderby GetDate(fi.Name.Substring(0, 10)) descending
        //                             select fi.Name);
        //                }
        //            }
        //        }
        //        return query.ToArray();
        //    }
        #region 组装数据源
        /// <summary>
        /// 组装数据源
        /// </summary>
        public void GetFilesData(DateTime startdate, DateTime enddate)
        {
            //string _path = _filepath;//相对路径
            //string FullPath = GetMapPath(_path);//获取绝对路径
            string FullPath = _filepath;
            string[] filelist = GetFilesList(FullPath, startdate, enddate);
            string _path = "http://218.91.209.251:1117/CSYC/";
            string[] childfilelist = GetChildFilesList(FullPath, startdate, enddate);
            DataTable dt = new DataTable();
            dt.Columns.Add("FileName", typeof(string));
            dt.Columns.Add("ImageUrl", typeof(string));
            dt.Columns.Add("ImageDate", typeof(string));

            if (childfilelist.Length > 0)
            {
                foreach (string str in childfilelist)
                {
                    //if (str.ToLower().LastIndexOf(radlDataType.SelectedItem.Text) > -1 || str.ToLower().LastIndexOf(radlDataType.SelectedItem.Value) > -1)
                    //{
                    DataRow dr = dt.NewRow();
                    dr["FileName"] = str;
                    dr["ImageUrl"] = _path + str;
                    dr["ImageDate"] = str.Substring(13, 10);
                    //dr["ImageDate"] = GetDate(str.Substring(0, 8)).ToString("yyyy-MM-dd");
                    dt.Rows.Add(dr);
                    //}
                }
            }
            if (filelist.Length > 0)
            {
                foreach (string str in filelist)
                {

                    //if (str.ToLower().LastIndexOf(radlDataType.SelectedItem.Text) > -1 || str.ToLower().LastIndexOf(radlDataType.SelectedItem.Value) > -1)
                    //{
                    DataRow dr = dt.NewRow();
                    dr["FileName"] = str;
                    dr["ImageUrl"] = _path + "/" + str;
                    dr["ImageDate"] = str;
                    dt.Rows.Add(dr);
                    //}
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
                //if (i < dt2.Rows.Count - 1)
                //{
                //    if (Convert.ToDateTime(dt2.Rows[i]["ImageDate"]) == Convert.ToDateTime(dt.Rows[i + 1]["ImageDate"]))
                //    {
                //        dr["ImageUrl"] += ";" + Convert.ToString(dt2.Rows[i + 1]["ImageUrl"]);
                //        i++;
                //    }
                //}
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
            diVisibility.Controls.Clear();
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
            string[] a = dt.AsEnumerable().Select(t => t.Field<string>("ImageDate")).Distinct().ToArray();

            ViewState["tableCount"] = datecount;
            //开始循环加载数据
            str.Append("<tbody>");
            for (int i = 0; i < dt.Rows.Count; i++)
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
            str.Append("<tr><td>共 " + a.Length + " 天，" + filecount.ToString() + " 张图片</td></tr>");
            str.Append("</tbody>");
            str.Append("</table>");

            Control ct = ParseControl(str.ToString());
            diVisibility.Controls.Add(ct);
        }
        #endregion

        #region 初始化列表加载图片
        /// <summary>
        /// 初始化列表加载图片
        /// </summary>
        /// <param name="ImageUrl">图片文件路径</param>
        public void LoadImg(string ImageUrl)
        {

            //divXG.Style.Add("background", "url(" + ImageUrl.Replace("~", "../../..") + ")");
            //divXG.Style.Add("background-size", "100% 100%");
            divImg.Style.Add("background", "url(" + ImageUrl.Replace("~", "../../..") + ")");
            divImg.Style.Add("background-size", "100% 100%");
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
                             where Convert.ToDateTime(fi.Name) >= startdate && Convert.ToDateTime(fi.Name) <= enddate
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
            if (str.Length == 10)
            {
                //IFormatProvider format = new System.Globalization.CultureInfo("zh-CN");
                string FormatStr = "yyyyMMdd";
                dt = DateTime.ParseExact(str, FormatStr, CultureInfo.InvariantCulture);
            }
            return dt;
        }
        #endregion
        /// <summary>
        /// 间隔时间按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void timer_Tick(object sender, EventArgs e)
        {
            //DateTime startdate = Convert.ToDateTime(rdpstartdate.SelectedDate);
            //DateTime enddate = Convert.ToDateTime(rdpenddate.SelectedDate).AddDays(1);
            DateTime startdate = DateTime.Now;
            DateTime enddate = DateTime.Now; ;
            if (ddlDataSource.SelectedValue == "OriData")
            {
                startdate = dtpBegin.SelectedDate.Value;
                enddate = dtpEnd.SelectedDate.Value;

            }
            if (ddlDataSource.SelectedValue == "AuditData")
            {
                //小时数据
                if (radlDataType.SelectedValue == "Hour")
                {
                    startdate = Convert.ToDateTime(hourBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:00"));
                    enddate = Convert.ToDateTime(hourEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:00"));

                }
                //日数据
                else if (radlDataType.SelectedValue == "Day")
                {
                    startdate = dayBegin.SelectedDate.Value;
                    enddate = dayEnd.SelectedDate.Value;
                }
                //月数据
                else if (radlDataType.SelectedValue == "Month")
                {
                    int monthB = monthBegin.SelectedDate.Value.Year;
                    int monthE = monthEnd.SelectedDate.Value.Year;
                    int monthF = monthBegin.SelectedDate.Value.Month;
                    int monthT = monthEnd.SelectedDate.Value.Month;
                    startdate = monthBegin.SelectedDate.Value;
                    enddate = monthEnd.SelectedDate.Value;
                }
            }
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
        /// <summary>
        /// 返回值为true
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        protected bool trueOrFalse(string str)
        {
            return true;

        }
        /// <summary>
        /// 停止播放按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void stop_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
        }
        /// <summary>
        /// 开始播放按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void start_Click(object sender, EventArgs e)
        {
            string second = tbSecond.Text == "" ? "3" : tbSecond.Text;
            timer.Interval = int.Parse(second) * 1000;
            timer.Enabled = true;
        }

        #region 绑定BindGrid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {
            if (!IsPostBack)
            {

            }
            try
            {
                //每页显示数据个数            
                int pageSize = 24;
                //当前页的序号
                int pageNo = 0;
                //数据总行数
                int recordTotal = 0;
                if (ddlJiGuang.SelectedValue == "extin532")
                {
                    Quality.Value = "extin532";
                }
                else if (ddlJiGuang.SelectedValue == "extin355")
                {
                    Quality.Value = "extin355";
                }
                else if (ddlJiGuang.SelectedValue == "border")
                {
                    Quality.Value = "border";
                }
                else if (ddlJiGuang.SelectedValue == "depol")
                {
                    Quality.Value = "depol";
                }

                if (ddlJiGuang.SelectedValue != "border")
                {
                    sqlch = string.Format("select Hight,Min,Max from [dbo].[DT_JGLDColorBatch] where DataType='{0}'", ddlDataSource.SelectedValue);
                    dvch = g_DatabaseHelper.ExecuteDataTable(sqlch, "AMS_BaseDataConnection");
                    stch = dtToArr(dvch);
                }

                DtStart.Value = Convert.ToDateTime(dtpBegin.SelectedDate).ToString("yyyy-MM-dd HH:mm:ss");
                DtEnd.Value = Convert.ToDateTime(dtpEnd.SelectedDate).ToString("yyyy-MM-dd HH:mm:ss");
                DataView dv = m_Min1Data.GetLadarData(Quality.Value, pageSize, pageNo, out recordTotal, DtStart.Value, DtEnd.Value,stch[0]);

                DataTable dt = dv.ToTable();
                DataTable dts = new DataTable();
                dts.Columns.Add("xValue", typeof(string));
                dts.Columns.Add("yValue", typeof(string));
                dts.Columns.Add("zValue", typeof(string));
                foreach (DataRow dr in dt.Rows)
                {
                    DataRow drNew = dts.NewRow();
                    drNew["xValue"] = dr["DateTime"];
                    drNew["yValue"] = dr["Height"];
                    drNew["zValue"] = dr["Number"];
                    dts.Rows.Add(drNew);
                }
                HiddenDataNew.Value = JsonHelper.ToJson(dts);
                RegisterScript("CreatCharts();");
            }
            catch (Exception e)
            {

            }
        }


        /// <summary>  
        /// 格式化字符型、日期型、布尔型  
        /// </summary>  
        /// <param name="str"></param>  
        /// <param name="type"></param>  
        /// <returns></returns>  
        private static string StringFormat(string key, string str, Type type)
        {
            if (key.Equals("Tstamp", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(str))
            {
                str = ConvertDateTimeToInt(Convert.ToDateTime(str)).ToString();
            }
            if (key.Equals("DateTime", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(str))
            {
                str = ConvertDateTimeToInt(Convert.ToDateTime(str)).ToString();
            }
            if (type == typeof(string))
            {
                str = String2Json(str);
                str = "\"" + str + "\"";
            }
            else if (type == typeof(DateTime))
            {
                str = "\"" + str + "\"";
            }
            else if (type == typeof(bool))
            {
                str = str.ToLower();
            }
            else if (string.IsNullOrEmpty(str))
            {
                str = "null";
            }
            return str;
        }
        /// <summary>  
        ///DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time">时间</param>  
        /// <returns>long</returns>  
        public static long ConvertDateTimeToInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.AddHours(-8).Ticks) / 10000;   //除10000调整为13位      
            return t;
        }
        /// <summary>  
        /// 过滤特殊字符  
        /// </summary>  
        /// <param name="s"></param>  
        /// <returns></returns>  
        private static string String2Json(String s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s.ToCharArray()[i];
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\""); break;
                    case '\\':
                        sb.Append("\\\\"); break;
                    case '/':
                        sb.Append("\\/"); break;
                    case '\b':
                        sb.Append("\\b"); break;
                    case '\f':
                        sb.Append("\\f"); break;
                    case '\n':
                        sb.Append("\\n"); break;
                    case '\r':
                        sb.Append("\\r"); break;
                    case '\t':
                        sb.Append("\\t"); break;
                    default:
                        sb.Append(c); break;
                }
            }
            return sb.ToString();
        }
        #endregion
    }
}