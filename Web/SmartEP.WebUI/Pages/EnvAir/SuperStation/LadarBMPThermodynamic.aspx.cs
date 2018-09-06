﻿using log4net;
using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.AutoMonitoring.Air;
using SmartEP.Service.BaseData.BusinessRule;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.Core.Enums;
using SmartEP.Service.DataAnalyze.Air.DataQuery;
using SmartEP.Service.Frame;
using SmartEP.Utilities.AdoData;
using SmartEP.Utilities.Calendar;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Utilities.Office;
using SmartEP.Utilities.Web.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.SuperStation
{
    /// <summary>
    /// 名称：LadarBMPThermodynamic.cs
    /// 创建人：刘晋
    /// 创建日期：2017-07-01
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：激光雷达页面
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class LadarBMPThermodynamic : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        InfectantBy1Service m_Min1Data = Singleton<InfectantBy1Service>.GetInstance();
        InfectantBy5Service m_Min5Data = Singleton<InfectantBy5Service>.GetInstance();
        InfectantBy60Service m_Min60Data = Singleton<InfectantBy60Service>.GetInstance();
        InfectantByDayService m_DayOriData = Singleton<InfectantByDayService>.GetInstance();
        InfectantByMonthService m_MonthOriData = Singleton<InfectantByMonthService>.GetInstance();
        DataQueryByHourService m_HourData = Singleton<DataQueryByHourService>.GetInstance();
        DataQueryByDayService m_DayData = Singleton<DataQueryByDayService>.GetInstance();
        DataQueryByWeekService m_WeekData = Singleton<DataQueryByWeekService>.GetInstance();
        DataQueryByMonthService m_MonthData = Singleton<DataQueryByMonthService>.GetInstance();
        DataQueryBySeasonService m_SeasonData = Singleton<DataQueryBySeasonService>.GetInstance();
        DataQueryByYearService m_YearData = Singleton<DataQueryByYearService>.GetInstance();
        InstrumentChannelService m_InstrumentChannelService = Singleton<InstrumentChannelService>.GetInstance();
        MonitoringPointAirService monitoringPointAir = new MonitoringPointAirService();
        //获取因子上下限数据处理服务
        ExcessiveSettingService m_ExcessiveSettingService = new ExcessiveSettingService();
        /// <summary>
        /// 数据库处理类
        /// </summary>
        DatabaseHelper g_DatabaseHelper = Singleton<DatabaseHelper>.GetInstance();
        //代码项服务层
        DictionaryService dicService = new DictionaryService();
        //获取因子小数位
        // channelCode 因子代码 例：CO的code是a21005  （ channelCode=a21005）
        SmartEP.Service.BaseData.Channel.AirPollutantService m_AirPollutantService = new SmartEP.Service.BaseData.Channel.AirPollutantService();
        string LZSPfactor = string.Empty;
        private int scriptFlag;
        /// <summary>
        /// 选择因子
        /// </summary>
        private IList<IPollutant> factors = null;
        private string[] strFactors = null;
        string LZSfactorName = string.Empty;
        /// <summary>
        /// 选择站点
        /// </summary>
        private IList<IPoint> points = null;
        private string[] portIds = null;
        /// <summary>
        /// 统计
        /// </summary>
        DataView dvStatistical = null;
        /// <summary>
        /// 全局日数据表 List
        /// </summary>
        DataTable dt = null;
        /// <summary>
        /// 国家标记位
        /// </summary>
        IQueryable<V_CodeMainItemEntity> siteTypeEntites = null;
        /// <summary>
        /// 配置城市摄影存放图片路径
        /// </summary>
        string _filepath = ConfigurationManager.AppSettings["NTJGLDXG"].ToString();
        string _path = ConfigurationManager.AppSettings["_path"].ToString();
        /// <summary>
        /// 全局日数据表 图标
        /// </summary>
        DataTable dtIcon = null;
        /// <summary>
        /// 全局周数据表 List
        /// </summary>
        DataTable dtWeek = null;


        static DateTime dtms = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:00"));
        static DateTime dtme = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00"));

        /// <summary>
        /// 全局周数据表 图标
        /// </summary>
        DataTable dtWeekIcon = null;
        /// <summary>
        /// 全局日数据表 List
        /// </summary>
        DataTable dtMonth = null;
        //static int first = 0;
        /// <summary>
        /// 全局日数据表 图标
        /// </summary>
        DataTable dtMonthIcon = null;
        ILog log = LogManager.GetLogger("FileLogging");//获取一个日志记录器
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                //hdPointName.Value = cbPoint.SelectedText;
                InitControl();
                //gridAudit.DataSource = new DataTable();
                //timer.Enabled = true;
                BindCharts();
            }
        }
        #region 页面控件初始化
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {

            ////初始化测点
            string FactorNames = System.Configuration.ConfigurationManager.AppSettings["AirPollutant"];
            //int recordTotal = 0;
            //时间框初始化
            DateTime dt = DateTime.Now;
            DateTime dtt = DateTime.Now.AddDays(-1);
            dt.ToLongTimeString().ToString();
            dtt.ToLongTimeString().ToString();
            dtpBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd HH:00"));
            dtpEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00"));

            //dtpBegin.SelectedDate = Convert.ToDateTime("2017-07-01 00:00:00");
            //dtpEnd.SelectedDate = Convert.ToDateTime("2017-07-02 00:00:00");
            //Quality.Value = "extin532";
            //DtStart.Value = Convert.ToDateTime(dtpBegin.SelectedDate).ToString("yyyy-MM-dd HH:mm:ss");
            //DtEnd.Value = Convert.ToDateTime(dtpEnd.SelectedDate).ToString("yyyy-MM-dd HH:mm:ss");
            //DataView dv1 = m_Min1Data.GetLadarData(Quality.Value, int.MaxValue, 0, out recordTotal, DtStart.Value, DtEnd.Value);

            //dtIcon = dv1.ToTable();
            //DataTable dts = new DataTable();
            //dts.Columns.Add("xValue", typeof(string));
            //dts.Columns.Add("yValue", typeof(string));
            //dts.Columns.Add("zValue", typeof(string));
            //foreach (DataRow dr in dtIcon.Rows)
            //{
            //    DataRow drNew = dts.NewRow();
            //    drNew["xValue"] = dr["DateTime"];
            //    //if(dr[])
            //    drNew["yValue"] = dr["Height"].ToString().Substring(0, dr["Height"].ToString().Length - 2);
            //    drNew["zValue"] = dr["Number"];
            //    dts.Rows.Add(drNew);
            //}
            //HiddenData.Value = ToJson(dts);
            
        }
        #endregion

        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid(string type)
        {
            if (!IsPostBack)
            {

            }
            try
            {
                divImg.Visible = false;
                //每页显示数据个数            
                int pageSize = gridAudit.PageSize;
                //当前页的序号
                int pageNo = gridAudit.CurrentPageIndex;
                //数据总行数
                int recordTotal = 0;

                string sqlch = string.Empty;
                DataTable dvch = null;
                string[] stch = null;


                DataView dv1 = new DataView();
                DtStart.Value = Convert.ToDateTime(dtpBegin.SelectedDate).ToString("yyyy-MM-dd HH:mm:ss");
                DtEnd.Value = Convert.ToDateTime(dtpEnd.SelectedDate).ToString("yyyy-MM-dd HH:mm:ss");

                if (ddlDataSource.SelectedValue != "height")
                {
                    sqlch = string.Format("select Hight,Min,Max,HightMin from [dbo].[DT_JGLDColorHight] where DataType='{0}'", ddlDataSource.SelectedValue);
                    dvch = g_DatabaseHelper.ExecuteDataTable(sqlch, "AMS_BaseDataConnection");
                    stch = dtToArr(dvch);
                    hdMin.Value = stch[1];
                    hdMax.Value = stch[2];
                    
                }
                
                if (ddlDataSource.SelectedValue == "extin532")
                {
                    Quality.Value = "extin532";
                    dv1 = m_Min1Data.GetLadarData(Quality.Value, pageSize, pageNo, out recordTotal, DtStart.Value, DtEnd.Value, stch[0], stch[3]);
                }
                else if (ddlDataSource.SelectedValue == "extin355")
                {
                    Quality.Value = "extin355";
                    dv1 = m_Min1Data.GetLadarData(Quality.Value, pageSize, pageNo, out recordTotal, DtStart.Value, DtEnd.Value, stch[0], stch[3]);
                }
                else if (ddlDataSource.SelectedValue == "height")
                {
                    divImg.Visible = false;
                    ChartContainer.Visible = true;
                    Quality.Value = "height";
                    dv1 = m_Min1Data.GetLadarBorder(Quality.Value, pageSize, pageNo, out recordTotal, DtStart.Value, DtEnd.Value);
                }
                else if (ddlDataSource.SelectedValue == "depol")
                {
                    Quality.Value = "depol";
                    dv1 = m_Min1Data.GetLadarData(Quality.Value, pageSize, pageNo, out recordTotal, DtStart.Value, DtEnd.Value, stch[0], stch[3]);
                }

                
                int hours = Convert.ToInt32((Convert.ToDateTime(DtEnd.Value) - Convert.ToDateTime(DtStart.Value)).TotalHours);
                int minutes = (Convert.ToDateTime(DtEnd.Value) - Convert.ToDateTime(DtStart.Value)).Minutes;
                int seconds = (Convert.ToDateTime(DtEnd.Value) - Convert.ToDateTime(DtStart.Value)).Seconds;
                if (hours == 48 && minutes == 0 && seconds == 0 && Convert.ToDateTime(DtEnd.Value).Minute == 0 && Convert.ToDateTime(DtStart.Value).Minute == 0)
                {
                    string FullPath = _filepath;
                    string[] filelist = GetFilesList(FullPath, Convert.ToDateTime(DtStart.Value), Convert.ToDateTime(DtEnd.Value));
                    if (filelist.Length >= 4)
                    {
                        string fileName = _path;
                        if (ddlDataSource.SelectedValue == "extin355")
                        {
                            fileName += filelist[0];
                        }
                        else if (ddlDataSource.SelectedValue == "extin532")
                        {
                            fileName += filelist[1];
                        }
                        else if (ddlDataSource.SelectedValue == "height")
                        {
                            fileName += filelist[2];
                        }
                        else if (ddlDataSource.SelectedValue == "depol")
                        {
                            fileName += filelist[3];
                        }
                        divImg.Style.Value = "height:500px;text-align:center";
                        divImg.Visible = true;
                        ChartContainer.Visible = false;
                        divImg.Src = fileName;
                    }
                    else
                    {
                        divImg.Controls.Clear();
                        divImg.Visible = false;
                        ChartContainer.Visible = true;
                        if (ddlDataSource.SelectedValue == "height")
                        {
                            if (dv1.ToTable().Rows.Count > 0)
                            {
                                DataTable dtTime = dv1.ToTable();
                                dtTime.Columns.Remove("Number");
                                hdCount.Value = dtTime.Rows.Count.ToString();
                                DataTable dtBorder = dv1.ToTable();
                                dtBorder.Columns.Remove("DateTime");
                                hdTime.Value = ToStringNew(dtTime);
                                hdBorder.Value = ToString(dtBorder);
                            }
                            else
                            {
                                hdCount.Value = "1";
                                hdTime.Value = "[]";
                                hdBorder.Value = "[]";
                            }
                        }
                        else
                        {
                            dtIcon = dv1.ToTable();
                            DataTable dts = new DataTable();
                            dts.Columns.Add("xValue", typeof(string));
                            dts.Columns.Add("yValue", typeof(string));
                            dts.Columns.Add("zValue", typeof(string));
                            foreach (DataRow dr in dtIcon.Rows)
                            {
                                DataRow drNew = dts.NewRow();
                                drNew["xValue"] = Convert.ToDateTime(dr["DateTime"].ToString()).ToString("MM/dd HH:mm:ss");
                                drNew["yValue"] = dr["Height"].ToString().Substring(0, dr["Height"].ToString().Length - 2);
                                drNew["zValue"] = dr["Number"];
                                dts.Rows.Add(drNew);
                            }
                            HiddenData.Value = ToJson(dts);
                        }
                    }
                }
                else
                {
                    divImg.Controls.Clear();
                    divImg.Visible = false;
                    ChartContainer.Visible = true;
                    if (ddlDataSource.SelectedValue == "height")
                    {
                        if (dv1.ToTable().Rows.Count > 0)
                        {
                            DataTable dtTime = dv1.ToTable();
                            dtTime.Columns.Remove("Number");
                            hdCount.Value = dtTime.Rows.Count.ToString();
                            DataTable dtBorder = dv1.ToTable();
                            dtBorder.Columns.Remove("DateTime");
                            hdTime.Value = ToStringNew(dtTime);
                            hdBorder.Value = ToString(dtBorder);
                        }
                        else
                        {
                            hdCount.Value = "1";
                            hdTime.Value = "[]";
                            hdBorder.Value = "[]";
                        }
                    }
                    else
                    {
                        dtIcon = dv1.ToTable();
                        DataTable dts = new DataTable();
                        dts.Columns.Add("xValue", typeof(string));
                        dts.Columns.Add("yValue", typeof(string));
                        dts.Columns.Add("zValue", typeof(string));
                        foreach (DataRow dr in dtIcon.Rows)
                        {
                            DataRow drNew = dts.NewRow();
                            drNew["xValue"] = Convert.ToDateTime(dr["DateTime"].ToString()).ToString("MM/dd HH:mm:ss");
                            drNew["yValue"] = dr["Height"].ToString().Substring(0, dr["Height"].ToString().Length - 2);
                            drNew["zValue"] = dr["Number"];
                            dts.Rows.Add(drNew);
                        }
                        HiddenData.Value = ToJson(dts);
                    }
                }
                if (type == "列表")
                {
                    DataView dv = new DataView();
                    if (ddlDataSource.SelectedValue == "extin532")
                    {
                        Quality.Value = "extin532";
                        dv = m_Min1Data.GetLadarDataNew(Quality.Value, pageSize, pageNo, out recordTotal, DtStart.Value, DtEnd.Value);
                    }
                    else if (ddlDataSource.SelectedValue == "extin355")
                    {
                        Quality.Value = "extin355";
                        dv = m_Min1Data.GetLadarDataNew(Quality.Value, pageSize, pageNo, out recordTotal, DtStart.Value, DtEnd.Value);
                    }
                    else if (ddlDataSource.SelectedValue == "height")
                    {
                        divImg.Visible = false;
                        ChartContainer.Visible = true;
                        Quality.Value = "height";
                        dv = m_Min1Data.GetLadarBorder(Quality.Value, pageSize, pageNo, out recordTotal, DtStart.Value, DtEnd.Value);
                    }
                    else if (ddlDataSource.SelectedValue == "depol")
                    {
                        Quality.Value = "depol";
                        dv = m_Min1Data.GetLadarDataNew(Quality.Value, pageSize, pageNo, out recordTotal, DtStart.Value, DtEnd.Value);
                    }
                    dv.Sort = "DateTime desc";
                    //BindCharts();
                    dt = dv.ToTable();
                    if (dt.Rows.Count >= 0)
                    {
                        gridAudit.DataSource = dt.DefaultView;
                    }
                    else
                    {
                        gridAudit.DataSource = new DataTable();
                    }
                    //数据总行数
                    gridAudit.VirtualItemCount = recordTotal;
                }
                
            }
            catch(Exception e)
            {
                log.Error(e.ToString());
                throw e;
            }
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
                         where trueOrFalse(fi.Name) && IsOrNotImg(fi.Name) && GetDate(fi.Name.Substring(5, 10)) >= startdate && GetDate(fi.Name.Substring(5, 10)) <= enddate
                         orderby GetDate(fi.Name.Substring(5, 10)) descending
                         select fi.Name);
            return query.ToArray();
        }
        protected bool IsOrNotImg(string str)
        {
            if (str.ToLower().Contains("bmp") || str.ToLower().Contains("jpeg") || str.ToLower().Contains("jpg") || str.ToLower().Contains("png") || str.ToLower().Contains("svg"))
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
                string FormatStr = "yyyyMMddHH";
                dt = DateTime.ParseExact(str, FormatStr, CultureInfo.InvariantCulture);
            }
            return dt;
        }
        #endregion
        /// <summary>
        /// 返回值为true
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        protected bool trueOrFalse(string str)
        {
            return true;

        }
        /// 读文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="fileType">.doc / .docx / .txt</param>
        /// <returns>文件内容</returns>
        public string ReadFile(string filePath, string fileType)
        {
            try
            {
                string fileName = filePath;
                string content = "文件格式不支持检索";
                filePath = HttpContext.Current.Server.MapPath(filePath);
                if (fileType == ".doc" || fileType == ".docx")
                {
                    Aspose.Words.Document doc = new Aspose.Words.Document(filePath);
                    content = doc.GetText();
                }
                else if (fileType == ".txt")
                {
                    StreamReader sr = new StreamReader(filePath, System.Text.Encoding.GetEncoding("UTF-8"));
                    content = sr.ReadToEnd().ToString();
                    sr.Close();
                }
                return content;
            }
            catch (Exception e)
            {
                return "对不起，本文没有发现！可能是从服务器上删除的。";
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
            //if (key.Equals("Tstamp", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(str))
            //{
            //    str = ConvertDateTimeToInt(Convert.ToDateTime(str)).ToString();
            //}
            //if (key.Equals("DateTime", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(str))
            //{
            //    str = ConvertDateTimeToInt(Convert.ToDateTime(str)).ToString();
            //}
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
        #region 绑定图表
        

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
            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "JS", Text, false);
        }
        #endregion
        /// <summary>
        /// 查找按钮事件
        /// </summary>
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            
            //BindGrid();
            gridAudit.CurrentPageIndex = 0;
            gridAudit.Rebind();
            if (tabStrip.SelectedTab.Text=="图谱")
            {
                BindCharts();
            }
            
                
        }
        /// <summary>
        /// DataTable转换为一维字符串数组
        /// </summary>
        /// <returns></returns>
        public static string[] dtToArr(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return new string[0];
            }
            else
            {
                string[] sr = new string[dt.Columns.Count];
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (Convert.IsDBNull(dt.Rows[0][i]))
                    {
                        sr[i] = "";
                    }
                    else
                    {
                        sr[i] = dt.Rows[0][i] + "";
                    }
                }
                return sr;
            }
        }
        public static string ToString(DataTable dt)
        {
            StringBuilder jsonString = new StringBuilder();
            jsonString.Append("[");
            DataRowCollection drc = dt.Rows;
            for (int i = 0; i < drc.Count; i++)
            {
                string strValue = drc[i][0].ToString();
                
                jsonString.Append(strValue + ",");
                
            }
            
            if (jsonString.ToString() != "[") jsonString.Remove(jsonString.Length - 1, 1);
            jsonString.Append("]");
            return jsonString.ToString();
        }
        public static string ToStringNew(DataTable dt)
        {
            StringBuilder jsonString = new StringBuilder();
            jsonString.Append("[");
            DataRowCollection drc = dt.Rows;
            for (int i = 0; i < drc.Count; i++)
            {
                string strValue = Convert.ToDateTime(drc[i][0].ToString()).ToString("MM/dd HH:mm:ss");

                jsonString.Append("\""+strValue + "\",");

            }

            if (jsonString.ToString() != "[") jsonString.Remove(jsonString.Length - 1, 1);
            jsonString.Append("]");
            return jsonString.ToString();
        }
        /// <summary>
        /// DataTable
        /// </summary>
        /// <param name="dt">DataTable对象</param>
        /// <returns>Json字符串</returns>
        public static string ToJson(DataTable dt)
        {
            StringBuilder jsonString = new StringBuilder();
            jsonString.Append("[");
            DataRowCollection drc = dt.Rows;
            for (int i = 0; i < drc.Count; i++)
            {
                jsonString.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string strKey = dt.Columns[j].ColumnName;
                    string strValue = drc[i][j].ToString();
                    Type type = dt.Columns[j].DataType;
                    jsonString.Append("\"" + strKey + "\":");
                    strValue = StringFormat(strKey, strValue, type);
                    if (j < dt.Columns.Count - 1)
                    {
                        jsonString.Append(strValue + ",");
                    }
                    else
                    {
                        jsonString.Append(strValue);
                    }
                }
                jsonString.Append("},");
            }
            if (jsonString.ToString() != "[") jsonString.Remove(jsonString.Length - 1, 1);
            jsonString.Append("]");
            return jsonString.ToString();
        }
        /// <summary>
        /// 把IQueryable转化为DataTable
        /// </summary>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public DataTable ConvertToDataTable(IEnumerable enumerable)
        {
            var dataTable = new DataTable();
            foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(typeof(ExcessiveSettingInfo)))
            {
                dataTable.Columns.Add(pd.Name);
            }
            foreach (ExcessiveSettingInfo item in enumerable)
            {
                var Row = dataTable.NewRow();

                foreach (PropertyDescriptor dp in TypeDescriptor.GetProperties(typeof(ExcessiveSettingInfo)))
                {
                    Row[dp.Name] = dp.GetValue(item);
                }
                dataTable.Rows.Add(Row);
            }
            return dataTable;
        }

        protected void gridAudit_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (tabStrip.SelectedTab.Text == "列表")
            {
                BindGrid("列表");
            }
            else
            {
                BindGrid("图谱");
            }
            
            
        }

        protected void gridAudit_ItemDataBound(object sender, GridItemEventArgs e)
        {

        }

        protected void gridAudit_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            try
            {
                if (e.Column.ColumnType.Equals("GridExpandColumn"))
                    return;
                //追加测点
                GridBoundColumn col = (GridBoundColumn)e.Column;
                string a = col.DataField;

                if (col.DataField == "Height")
                {
                    col.HeaderText = "高度(Km)";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterText = "平均值<br>最大值<br>最小值";
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "Number")
                {
                    col.HeaderText = ddlDataSource.SelectedText;
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "DateTime")
                {
                    col = (GridDateTimeColumn)e.Column;
                    string tstcolformat = "{0:yyyy-MM-dd HH:mm:ss}";
                    col.HeaderText = "时间";
                    col.DataFormatString = tstcolformat;
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(120);
                    col.ItemStyle.Width = Unit.Pixel(120);
                }
                
            }
            catch (Exception ex) { }
        }

        protected void gridRTB_ButtonClick(object sender, RadToolBarEventArgs e)
        {

        }

        public int GetAuditDays()
        {
            int days = -1;
            try
            {
                days = ConfigurationManager.AppSettings["AuditDays"] != null && !ConfigurationManager.AppSettings["AuditDays"].ToString().Equals("") ? Convert.ToInt32(ConfigurationManager.AppSettings["AuditDays"].ToString()) : -1;
            }
            catch
            {
                days = -1;
            }
            return days;
        }

        protected void tabStrip_TabClick(object sender, RadTabStripEventArgs e)
        {
            if (tabStrip.SelectedTab.Text == "列表")
            {
                BindGrid("列表");
            }
            else
            {
                BindGrid("图谱");
                BindCharts();
            }
        }
    }
}