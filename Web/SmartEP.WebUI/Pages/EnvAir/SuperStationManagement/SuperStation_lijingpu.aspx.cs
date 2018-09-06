using SmartEP.Core.Generic;
using SmartEP.Service.AutoMonitoring.Air;
using SmartEP.Utilities.Office;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SmartEP.Utilities.Web.UI;
using SmartEP.Utilities.Calendar;
using SmartEP.Core.Enums;
using SmartEP.Core.Interfaces;
using SmartEP.Utilities.DataTypes.ExtensionMethods;

namespace SmartEP.WebUI.Pages.EnvAir.SuperStationManagement
{
    /// <summary>
    /// 名称：SuperStation_lijingpu.aspx.cs
    /// 创建人：樊垂贺
    /// 创建日期：2016-05-13
    /// 维护人员：
    /// 最新维护人员：徐阳
    /// 最新维护日期：2017-05-19
    /// 功能摘要：
    /// 粒径谱数据
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class SuperStation_lijingpu : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 粒径谱数据服务层
        /// </summary>
        SuperStation_lijingpuService s_SuperStation_lijingpuService = Singleton<SuperStation_lijingpuService>.GetInstance();

        //获取选择测点
        private IList<IPoint> points = null;

        /// <summary>
        /// 作为判断是否为超级站的标记
        /// </summary>
        string isSuper = string.Empty;

        protected override void OnPreInit(EventArgs e)
        {
            isSuper = PageHelper.GetQueryString("superOrNot");
            pointCbxRsm.isSuper(isSuper);
        }

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
            //初始化测点
            //string connSuperStationPointName = System.Configuration.ConfigurationManager.AppSettings["SuperStationPointName"];
            //string[] strPointName = connSuperStationPointName.Split(',');
            //string connSuperStationPointId = System.Configuration.ConfigurationManager.AppSettings["SuperStationPointId"];
            //string[] strPointID = connSuperStationPointId.Split(',');
            //cbPoint.Items.Clear();
            //for (int i = 0; i < strPointName.Length; i++)
            //{
            //    cbPoint.Items.Add(new DropDownListItem(strPointName[i].ToString(), strPointID[i].ToString()));
            //}
            //cbPoint.DataBind();
            //rddlType.Items.Add(new DropDownListItem("3772L", "0"));
            //rddlType.Items.Add(new DropDownListItem("APS3321", "1"));

            //时间框初始化
            hourBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddHours(-47));
            hourEnd.SelectedDate = DateTime.Now;
            dtpBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm"));
            dtpEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            dayBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
            dayEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            monthBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToString("yyyy-MM"));
            monthEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"));
            seasonBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"));
            seasonEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"));
            weekBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"));
            weekEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"));
            yearBegin.SelectedDate = DateTime.Now.AddYears(-5);
            yearEnd.SelectedDate = DateTime.Now.AddYears(-1);
            BindWeekFComboBox();
            BindWeekTComboBox();
            //数据类型
            radlDataTypeOri.Items.Add(new ListItem("一分钟", PollutantDataType.Min1.ToString()));
            radlDataTypeOri.Items.Add(new ListItem("五分钟", PollutantDataType.Min5.ToString()));
            radlDataTypeOri.Items.Add(new ListItem("小时", PollutantDataType.Min60.ToString()));
            radlDataTypeOri.Items.Add(new ListItem("日", PollutantDataType.Day.ToString()));
            radlDataTypeOri.Items.Add(new ListItem("月", PollutantDataType.Month.ToString()));
            radlDataTypeOri.SelectedValue = PollutantDataType.Min60.ToString();

            radlDataType.Items.Add(new ListItem("小时", PollutantDataType.Hour.ToString()));
            radlDataType.Items.Add(new ListItem("日", PollutantDataType.Day.ToString()));
            radlDataType.Items.Add(new ListItem("周", PollutantDataType.Week.ToString()));
            radlDataType.Items.Add(new ListItem("月", PollutantDataType.Month.ToString()));
            radlDataType.Items.Add(new ListItem("季", PollutantDataType.Season.ToString()));
            radlDataType.Items.Add(new ListItem("年", PollutantDataType.Year.ToString()));
            radlDataType.SelectedValue = PollutantDataType.Hour.ToString();

            dtpHour.Visible = true;
            dbtHour.Visible = false;
            dbtDay.Visible = false;
            dbtMonth.Visible = false;
            dbtSeason.Visible = false;
            dbtYear.Visible = false;
            dbtWeek.Visible = false;
        }
        #endregion
        #region 绑定grid
        /// <summary>
        /// 绑定grid
        /// </summary>
        private void BindGrid()
        {
            string[] pointIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
            DateTime dtBegion = DateTime.Now;
            DateTime dtEnd = DateTime.Now;
            if (pointIds != null)
            {
                if (ddlDataSource.SelectedIndex == 1)
                {
                    //审核数据日数据
                    if (radlDataType.SelectedValue == "Day")
                    {
                        dtBegion = dayBegin.SelectedDate.Value;
                        dtEnd = dayEnd.SelectedDate.Value.AddDays(1).AddSeconds(-1);
                        var dv = s_SuperStation_lijingpuService.GetDataList("Day", pointIds, dtBegion, dtEnd);
                        gridAudit.DataSource = dv;
                        if (dv != null && dv.Count >= 0)
                        {
                            gridAudit.VirtualItemCount = dv.Count;
                        }

                        if (tabStrip.SelectedTab.Text == "图表")
                        {
                            BindChart();
                        }
                    }
                    //审核数据小时数据
                    else if (radlDataType.SelectedValue == "Hour")
                    {
                        dtBegion = Convert.ToDateTime(hourBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:00"));
                        dtEnd = Convert.ToDateTime(hourEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:00"));
                        var dv = s_SuperStation_lijingpuService.GetDataList("Hour", pointIds, dtBegion, dtEnd);
                        gridAudit.DataSource = dv;
                        if (dv != null && dv.Count >= 0)
                        {
                            gridAudit.VirtualItemCount = dv.Count;
                        }

                        if (tabStrip.SelectedTab.Text == "图表")
                        {
                            BindChart();
                        }
                    }
                    //审核数据月数据
                    else if (radlDataType.SelectedValue == "Month")
                    {
                        //DateTime mtBegin = monthBegin.SelectedDate.Value;
                        ////本月第一天时间 
                        //dtBegion = mtBegin.AddDays(-(mtBegin.Day) + 1);
                        //DateTime mtEnd = monthEnd.SelectedDate.Value;
                        ////将本月月数+1 
                        //DateTime dt2 = mtEnd.AddMonths(1);
                        ////本月最后一天时间 
                        //dtEnd = dt2.AddDays(-(mtEnd.Day)).AddDays(1).AddSeconds(-1);
                        //var dv = s_SuperStation_lijingpuService.GetDataList("Month", pointIds, dtBegion, dtEnd);
                        int monthB = monthBegin.SelectedDate.Value.Year;
                        int monthE = monthEnd.SelectedDate.Value.Year;
                        int monthF = monthBegin.SelectedDate.Value.Month;
                        int monthT = monthEnd.SelectedDate.Value.Month;
                        var dv = s_SuperStation_lijingpuService.GetDataList("Month", pointIds, monthB, monthE, monthF, monthT);
                        gridAudit.DataSource = dv;
                        if (dv != null && dv.Count >= 0)
                        {
                            gridAudit.VirtualItemCount = dv.Count;
                        }

                        if (tabStrip.SelectedTab.Text == "图表")
                        {
                            BindChart();
                        }
                    }
                    //审核数据周数据
                    else if (radlDataType.SelectedValue == "Week")
                    {
                        //dtBegion = Convert.ToDateTime(weekFrom.SelectedValue);
                        //dtEnd = DateTime.ParseExact(weekTo.SelectedValue, "yyyy-MM-dd", null).AddDays(6).AddDays(1).AddSeconds(-1);
                        //DataView dv = s_SuperStation_lijingpuService.GetDataList("Week", pointIds, dtBegion, dtEnd);
                        int weekB = weekBegin.SelectedDate.Value.Year;
                        int weekE = weekEnd.SelectedDate.Value.Year;
                        int newyear = DateTime.ParseExact(weekFrom.SelectedValue, "yyyy-MM-dd", null).AddDays(6).Year;
                        int nyear = DateTime.ParseExact(weekTo.SelectedValue, "yyyy-MM-dd", null).Year;
                        int weekF = 0;
                        int weekT = 0;
                        if (newyear > weekB)
                        {
                            weekF = ChinaDate.WeekOfYear(DateTime.ParseExact(weekFrom.SelectedValue, "yyyy-MM-dd", null));
                        }
                        else
                            weekF = ChinaDate.WeekOfYear(DateTime.ParseExact(weekFrom.SelectedValue, "yyyy-MM-dd", null).AddDays(6));

                        if (weekE > nyear)
                        {
                            weekT = ChinaDate.WeekOfYear(DateTime.ParseExact(weekTo.SelectedValue, "yyyy-MM-dd", null).AddDays(6));
                        }
                        else
                            weekT = ChinaDate.WeekOfYear(DateTime.ParseExact(weekTo.SelectedValue, "yyyy-MM-dd", null));
                        var dv = s_SuperStation_lijingpuService.GetDataList("Week", pointIds, weekB, weekE, weekF, weekT);
                        gridAudit.DataSource = dv;
                        if (dv != null && dv.Count >= 0)
                        {
                            gridAudit.VirtualItemCount = dv.Count;
                        }

                        if (tabStrip.SelectedTab.Text == "图表")
                        {
                            BindChart();
                        }
                    }
                    //审核数据季数据
                    else if (radlDataType.SelectedValue == "Season")
                    {
                        //int seasonF = Convert.ToInt32(seasonFrom.SelectedValue);
                        //int seasonT = Convert.ToInt32(seasonTo.SelectedValue);
                        //DateTime seaBegin = seasonBegin.SelectedDate.Value;
                        //dtBegion = seaBegin.AddMonths(seasonF * 3).AddMonths(-3).AddDays(1);
                        //DateTime seaEnd = seasonEnd.SelectedDate.Value;
                        //dtEnd = seaEnd.AddMonths(seasonT * 3);
                        //var dv = s_SuperStation_lijingpuService.GetDataList("Season", pointIds, dtBegion, dtEnd);
                        int seasonB = seasonBegin.SelectedDate.Value.Year;
                        int seasonE = seasonEnd.SelectedDate.Value.Year;
                        int seasonF = Convert.ToInt32(seasonFrom.SelectedValue);
                        int seasonT = Convert.ToInt32(seasonTo.SelectedValue);
                        var dv = s_SuperStation_lijingpuService.GetDataList("Season", pointIds, seasonB, seasonE, seasonF, seasonT);
                        gridAudit.DataSource = dv;
                        if (dv != null && dv.Count >= 0)
                        {
                            gridAudit.VirtualItemCount = dv.Count;
                        }

                        if (tabStrip.SelectedTab.Text == "图表")
                        {
                            BindChart();
                        }
                    }
                    //审核数据年数据
                    else if (radlDataType.SelectedValue == "Year")
                    {
                        int yearB = yearBegin.SelectedDate.Value.Year;
                        int yearE = yearEnd.SelectedDate.Value.Year;
                        var dv = s_SuperStation_lijingpuService.GetDataList("Year", pointIds, yearB, yearE, 0, 0);
                        gridAudit.DataSource = dv;
                        if (dv != null && dv.Count >= 0)
                        {
                            gridAudit.VirtualItemCount = dv.Count;
                        }

                        if (tabStrip.SelectedTab.Text == "图表")
                        {
                            BindChart();
                        }
                    }
                }
                else if (ddlDataSource.SelectedIndex == 0)
                {
                    //原始数据数据小时数据
                    if (radlDataTypeOri.SelectedValue == "Min60")
                    {
                        dtBegion = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:00"));
                        dtEnd = Convert.ToDateTime(dtpEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:00"));
                        var dv = s_SuperStation_lijingpuService.GetDataList("HourOri", pointIds, dtBegion, dtEnd);
                        gridAudit.DataSource = dv;
                        if (dv != null && dv.Count >= 0)
                        {
                            gridAudit.VirtualItemCount = dv.Count;
                        }

                        if (tabStrip.SelectedTab.Text == "图表")
                        {
                            BindChart();
                        }
                    }
                    //原始数据数据一分钟数据
                    else if (radlDataTypeOri.SelectedValue == "Min1")
                    {
                        dtBegion = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                        dtEnd = Convert.ToDateTime(dtpEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                        var dv = s_SuperStation_lijingpuService.GetDataList("Min1", pointIds, dtBegion, dtEnd);
                        gridAudit.DataSource = dv;
                        if (dv != null && dv.Count >= 0)
                        {
                            gridAudit.VirtualItemCount = dv.Count;
                        }

                        if (tabStrip.SelectedTab.Text == "图表")
                        {
                            BindChart();
                        }
                    }
                    //原始数据数据五分钟数据
                    else if (radlDataTypeOri.SelectedValue == "Min5")
                    {
                        dtBegion = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                        dtEnd = Convert.ToDateTime(dtpEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                        var dv = s_SuperStation_lijingpuService.GetDataList("Min5", pointIds, dtBegion, dtEnd);
                        gridAudit.DataSource = dv;
                        if (dv != null && dv.Count >= 0)
                        {
                            gridAudit.VirtualItemCount = dv.Count;
                        }

                        if (tabStrip.SelectedTab.Text == "图表")
                        {
                            BindChart();
                        }
                    }
                    //原始数据数据日数据
                    else if (radlDataTypeOri.SelectedValue == "Day")
                    {
                        dtBegion = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                        dtEnd = Convert.ToDateTime(dtpEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                        var dv = s_SuperStation_lijingpuService.GetDataList("DayOri", pointIds, dtBegion, dtEnd);
                        gridAudit.DataSource = dv;
                        if (dv != null && dv.Count >= 0)
                        {
                            gridAudit.VirtualItemCount = dv.Count;
                        }

                        if (tabStrip.SelectedTab.Text == "图表")
                        {
                            BindChart();
                        }
                    }
                    //原始数据数据月数据
                    else if (radlDataTypeOri.SelectedValue == "Month")
                    {
                        //dtBegion = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                        //dtEnd = Convert.ToDateTime(dtpEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                        //var dv = s_SuperStation_lijingpuService.GetDataList("MonthOri", pointIds, dtBegion, dtEnd);
                        int monthB = monthBegin.SelectedDate.Value.Year;
                        int monthE = monthEnd.SelectedDate.Value.Year;
                        int monthF = monthBegin.SelectedDate.Value.Month;
                        int monthT = monthEnd.SelectedDate.Value.Month;
                        var dv = s_SuperStation_lijingpuService.GetDataList("MonthOri", pointIds, monthB, monthE, monthF, monthT);
                        gridAudit.DataSource = dv;
                        if (dv != null && dv.Count >= 0)
                        {
                            gridAudit.VirtualItemCount = dv.Count;
                        }

                        if (tabStrip.SelectedTab.Text == "图表")
                        {
                            BindChart();
                        }
                    }
                }
            }
            else
            {
                gridAudit.DataSource = new DataTable();
            }

        }

        public void BindChart()
        {
            string num = HiddenNum.Value;
            string[] pointIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
            List<string> names = new List<string>();
            for (int i = 0; i < pointIds.Length; i++)
            {
                string pointName = pointCbxRsm.GetPoints().Where(t => t.PointID == pointIds[i]).FirstOrDefault().PointName;
                names.Add(pointName);
            }
            string[] namesArray = names.ToArray();
            hdName.Value = string.Join(",", namesArray);

            //审核数据
            if (ddlDataSource.SelectedIndex == 1)
            {
                if (radlDataType.SelectedValue == "Day")
                {
                    string dtBegion = dayBegin.SelectedDate.Value.ToString("yyyy-MM-dd 00:00:00");
                    string dtEnd = dayEnd.SelectedDate.Value.ToString("yyyy-MM-dd 23:59:59");

                    hdType.Value = "Day";
                    hdNum.Value = num;
                    hddtBegion.Value = dtBegion;
                    hddtEnd.Value = dtEnd;
                    hdPointId.Value = string.Join(",", pointIds);
                }
                if (radlDataType.SelectedValue == "Hour")
                {
                    string dtBegion = hourBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:00");
                    string dtEnd = hourEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:00");

                    hdType.Value = "Hour";
                    hdNum.Value = num;
                    hddtBegion.Value = dtBegion;
                    hddtEnd.Value = dtEnd;
                    hdPointId.Value = string.Join(",", pointIds);
                }
                if (radlDataType.SelectedValue == "Month")
                {
                    int monthB = monthBegin.SelectedDate.Value.Year;
                    int monthE = monthEnd.SelectedDate.Value.Year;
                    int monthF = monthBegin.SelectedDate.Value.Month;
                    int monthT = monthEnd.SelectedDate.Value.Month;

                    hdType.Value = "Month";
                    hdNum.Value = num;
                    hddtB.Value = monthB.ToString();
                    hddtE.Value = monthE.ToString();
                    hddtF.Value = monthF.ToString();
                    hddtT.Value = monthT.ToString();
                    hddtFlag.Value = "1";
                    hdPointId.Value = string.Join(",", pointIds);
                }
                if (radlDataType.SelectedValue == "Season")
                {
                    int seasonB = seasonBegin.SelectedDate.Value.Year;
                    int seasonE = seasonEnd.SelectedDate.Value.Year;
                    int seasonF = Convert.ToInt32(seasonFrom.SelectedValue);
                    int seasonT = Convert.ToInt32(seasonTo.SelectedValue);

                    hdType.Value = "Season";
                    hdNum.Value = num;
                    hddtB.Value = seasonB.ToString();
                    hddtE.Value = seasonE.ToString();
                    hddtF.Value = seasonF.ToString();
                    hddtT.Value = seasonT.ToString();
                    hddtFlag.Value = "1";
                    hdPointId.Value = string.Join(",", pointIds);
                }
                if (radlDataType.SelectedValue == "Week")
                {
                    int weekB = weekBegin.SelectedDate.Value.Year;
                    int weekE = weekEnd.SelectedDate.Value.Year;
                    int newyear = DateTime.ParseExact(weekFrom.SelectedValue, "yyyy-MM-dd", null).AddDays(6).Year;
                    int nyear = DateTime.ParseExact(weekTo.SelectedValue, "yyyy-MM-dd", null).Year;
                    int weekF = 0;
                    int weekT = 0;
                    if (newyear > weekB)
                    {
                        weekF = ChinaDate.WeekOfYear(DateTime.ParseExact(weekFrom.SelectedValue, "yyyy-MM-dd", null));
                    }
                    else
                        weekF = ChinaDate.WeekOfYear(DateTime.ParseExact(weekFrom.SelectedValue, "yyyy-MM-dd", null).AddDays(6));

                    if (weekE > nyear)
                    {
                        weekT = ChinaDate.WeekOfYear(DateTime.ParseExact(weekTo.SelectedValue, "yyyy-MM-dd", null).AddDays(6));
                    }
                    else
                        weekT = ChinaDate.WeekOfYear(DateTime.ParseExact(weekTo.SelectedValue, "yyyy-MM-dd", null));

                    hdType.Value = "Week";
                    hdNum.Value = num;
                    hddtB.Value = weekB.ToString();
                    hddtE.Value = weekE.ToString();
                    hddtF.Value = weekF.ToString();
                    hddtT.Value = weekT.ToString();
                    hddtFlag.Value = "1";
                    hdPointId.Value = string.Join(",", pointIds);
                }
                if (radlDataType.SelectedValue == "Year")
                {
                    int yearB = yearBegin.SelectedDate.Value.Year;
                    int yearE = yearEnd.SelectedDate.Value.Year;

                    hdType.Value = "Year";
                    hdNum.Value = num;
                    hddtB.Value = yearB.ToString();
                    hddtE.Value = yearE.ToString();
                    hddtF.Value = "";
                    hddtT.Value = "";
                    hddtFlag.Value = "1";
                    hdPointId.Value = string.Join(",", pointIds);
                }
            }
                //原始数据
            else if (ddlDataSource.SelectedIndex == 0)
            {
                if (radlDataTypeOri.SelectedValue == "Min60")
                {
                    string dtBegion = dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:00");
                    string dtEnd = dtpEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:00");

                    hdType.Value = "HourOri";
                    hdNum.Value = num;
                    hddtBegion.Value = dtBegion;
                    hddtEnd.Value = dtEnd;
                    hdPointId.Value = string.Join(",", pointIds);
                }
                if (radlDataTypeOri.SelectedValue == "Min1" || radlDataTypeOri.SelectedValue == "Min5")
                {
                    string dtBegion = dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm");
                    string dtEnd = dtpEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm");

                    hdType.Value = radlDataTypeOri.SelectedValue;
                    hdNum.Value = num;
                    hddtBegion.Value = dtBegion;
                    hddtEnd.Value = dtEnd;
                    hdPointId.Value = string.Join(",", pointIds);
                }
                if (radlDataTypeOri.SelectedValue == "Day")
                {
                    string dtBegion = dayBegin.SelectedDate.Value.ToString("yyyy-MM-dd 00:00:00");
                    string dtEnd = dayEnd.SelectedDate.Value.ToString("yyyy-MM-dd 23:59:59");

                    hdType.Value = "DayOri";
                    hdNum.Value = num;
                    hddtBegion.Value = dtBegion;
                    hddtEnd.Value = dtEnd;
                    hdPointId.Value = string.Join(",", pointIds);
                }
                if (radlDataTypeOri.SelectedValue == "Month")
                {
                    int monthB = monthBegin.SelectedDate.Value.Year;
                    int monthE = monthEnd.SelectedDate.Value.Year;
                    int monthF = monthBegin.SelectedDate.Value.Month;
                    int monthT = monthEnd.SelectedDate.Value.Month;

                    hdType.Value = "MonthOri";
                    hdNum.Value = num;
                    hddtB.Value = monthB.ToString();
                    hddtE.Value = monthE.ToString();
                    hddtF.Value = monthF.ToString();
                    hddtT.Value = monthT.ToString();
                    hddtFlag.Value = "1";
                    hdPointId.Value = string.Join(",", pointIds);
                }
            }

        }
        #endregion
        public static bool IsNumeric(string text)
        {
            return Regex.IsMatch(text, @"^[+-]?\d*[.]?\d*$");
        }
        /// <summary>   
        /// Datatable转换为Json   
        /// </summary>   
        /// <param name="table">Datatable对象</param>   
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
        /// 绑定周
        /// </summary>
        private void BindWeekFComboBox()
        {
            if (weekBegin.SelectedDate > System.DateTime.Now)
            {
                Alert("选择时间必须小于等于当前时间！");
                return;
            }
            weekFrom.DataValueField = "value";
            weekFrom.DataTextField = "text";
            weekFrom.DataSource = ChinaDate.GetWeekOfMonth(weekBegin.SelectedDate.Value);
            weekFrom.DataBind();
            SetLiteral();
        }

        private void BindWeekTComboBox()
        {
            if (weekEnd.SelectedDate > System.DateTime.Now)
            {
                Alert("选择时间必须小于等于当前时间！");
                return;
            }
            weekTo.DataValueField = "value";
            weekTo.DataTextField = "text";
            weekTo.DataSource = ChinaDate.GetWeekOfMonth(weekEnd.SelectedDate.Value);
            weekTo.DataBind();
            SetLiteralT();
        }
        /// <summary>
        /// 显示所选周的日期
        /// </summary>
        private void SetLiteral()
        {
            txtweekF.Text = string.Format("从{0:yyyy-MM-dd}", weekFrom.SelectedValue);
        }
        private void SetLiteralT()
        {
            DateTime endDate = DateTime.ParseExact(weekTo.SelectedValue, "yyyy-MM-dd", null).AddDays(6);
            if (endDate > System.DateTime.Now)
                endDate = System.DateTime.Now;
            txtweekT.Text = string.Format("到{0:yyyy-MM-dd}", endDate);
        }
        #region 周数据更新日期范围
        protected void weekBegin_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            BindWeekFComboBox();
        }

        protected void weekFrom_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            SetLiteral();
        }

        protected void weekEnd_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            BindWeekTComboBox();
        }

        protected void weekTo_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            SetLiteralT();
        }
        #endregion


        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            HiddenNum.Value = "0";
            gridAudit.CurrentPageIndex = 0;
            gridAudit.Rebind();
            if (tabStrip.SelectedTab.Text == "图表")
            {
                RegisterScript("chart();");
            }
            else
            {
                FirstLoadChart.Value = "1";
            }
        }

        protected void gridRTB_ButtonClick(object sender, Telerik.Web.UI.RadToolBarEventArgs e)
        {
            string[] pointIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
            DateTime dtBegion = DateTime.Now;
            DateTime dtEnd = DateTime.Now;
            DataTable dt = new DataTable();
            if (pointIds != null)
            {
                if (ddlDataSource.SelectedIndex == 1)
                {
                    //审核数据日数据
                    if (radlDataType.SelectedValue == "Day")
                    {
                        dtBegion = dayBegin.SelectedDate.Value;
                        dtEnd = dayEnd.SelectedDate.Value.AddDays(1).AddSeconds(-1);
                        dt = s_SuperStation_lijingpuService.GetDataList("Day", pointIds, dtBegion, dtEnd).ToTable();
                    }
                    //审核数据小时数据
                    else if (radlDataType.SelectedValue == "Hour")
                    {
                        dtBegion = Convert.ToDateTime(hourBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:00"));
                        dtEnd = Convert.ToDateTime(hourEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:00"));
                        dt = s_SuperStation_lijingpuService.GetDataList("Hour", pointIds, dtBegion, dtEnd).ToTable();
                    }
                    //审核数据月数据
                    else if (radlDataType.SelectedValue == "Month")
                    {
                        int monthB = monthBegin.SelectedDate.Value.Year;
                        int monthE = monthEnd.SelectedDate.Value.Year;
                        int monthF = monthBegin.SelectedDate.Value.Month;
                        int monthT = monthEnd.SelectedDate.Value.Month;
                        dt = s_SuperStation_lijingpuService.GetDataList("Month", pointIds, monthB, monthE, monthF, monthT).ToTable();
                    }
                    //审核数据周数据
                    else if (radlDataType.SelectedValue == "Week")
                    {
                        int weekB = weekBegin.SelectedDate.Value.Year;
                        int weekE = weekEnd.SelectedDate.Value.Year;
                        int newyear = DateTime.ParseExact(weekFrom.SelectedValue, "yyyy-MM-dd", null).AddDays(6).Year;
                        int nyear = DateTime.ParseExact(weekTo.SelectedValue, "yyyy-MM-dd", null).Year;
                        int weekF = 0;
                        int weekT = 0;
                        if (newyear > weekB)
                        {
                            weekF = ChinaDate.WeekOfYear(DateTime.ParseExact(weekFrom.SelectedValue, "yyyy-MM-dd", null));
                        }
                        else
                            weekF = ChinaDate.WeekOfYear(DateTime.ParseExact(weekFrom.SelectedValue, "yyyy-MM-dd", null).AddDays(6));

                        if (weekE > nyear)
                        {
                            weekT = ChinaDate.WeekOfYear(DateTime.ParseExact(weekTo.SelectedValue, "yyyy-MM-dd", null).AddDays(6));
                        }
                        else
                            weekT = ChinaDate.WeekOfYear(DateTime.ParseExact(weekTo.SelectedValue, "yyyy-MM-dd", null));
                        dt = s_SuperStation_lijingpuService.GetDataList("Week", pointIds, weekB, weekE, weekF, weekT).ToTable();
                    }
                    //审核数据季数据
                    else if (radlDataType.SelectedValue == "Season")
                    {
                        //int seasonF = Convert.ToInt32(seasonFrom.SelectedValue);
                        //int seasonT = Convert.ToInt32(seasonTo.SelectedValue);
                        //DateTime seaBegin = seasonBegin.SelectedDate.Value;
                        //dtBegion = seaBegin.AddMonths(seasonF * 3).AddMonths(-3).AddDays(1);
                        //DateTime seaEnd = seasonEnd.SelectedDate.Value;
                        //dtEnd = seaEnd.AddMonths(seasonT * 3);
                        //var dv = s_SuperStation_lijingpuService.GetDataList("Season", pointIds, dtBegion, dtEnd);
                        int seasonB = seasonBegin.SelectedDate.Value.Year;
                        int seasonE = seasonEnd.SelectedDate.Value.Year;
                        int seasonF = Convert.ToInt32(seasonFrom.SelectedValue);
                        int seasonT = Convert.ToInt32(seasonTo.SelectedValue);
                        dt = s_SuperStation_lijingpuService.GetDataList("Season", pointIds, seasonB, seasonE, seasonF, seasonT).ToTable();
                    }
                    //审核数据年数据
                    else if (radlDataType.SelectedValue == "Year")
                    {
                        int yearB = yearBegin.SelectedDate.Value.Year;
                        int yearE = yearEnd.SelectedDate.Value.Year;
                        dt = s_SuperStation_lijingpuService.GetDataList("Year", pointIds, yearB, yearE, 0, 0).ToTable();
                    }
                }
                else if (ddlDataSource.SelectedIndex == 0)
                {
                    //原始数据数据小时数据
                    if (radlDataTypeOri.SelectedValue == "Min60")
                    {
                        dtBegion = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:00"));
                        dtEnd = Convert.ToDateTime(dtpEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:00"));
                        dt = s_SuperStation_lijingpuService.GetDataList("HourOri", pointIds, dtBegion, dtEnd).ToTable();
                    }
                    //原始数据数据一分钟数据
                    else if (radlDataTypeOri.SelectedValue == "Min1")
                    {
                        dtBegion = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                        dtEnd = Convert.ToDateTime(dtpEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                        dt = s_SuperStation_lijingpuService.GetDataList("Min1", pointIds, dtBegion, dtEnd).ToTable();
                    }
                    //原始数据数据五分钟数据
                    else if (radlDataTypeOri.SelectedValue == "Min5")
                    {
                        dtBegion = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                        dtEnd = Convert.ToDateTime(dtpEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                        dt = s_SuperStation_lijingpuService.GetDataList("Min5", pointIds, dtBegion, dtEnd).ToTable();
                    }
                    //原始数据数据日数据
                    else if (radlDataTypeOri.SelectedValue == "Day")
                    {
                        dtBegion = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                        dtEnd = Convert.ToDateTime(dtpEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                        dt = s_SuperStation_lijingpuService.GetDataList("DayOri", pointIds, dtBegion, dtEnd).ToTable();
                    }
                    //原始数据数据月数据
                    else if (radlDataTypeOri.SelectedValue == "Month")
                    {
                        int monthB = monthBegin.SelectedDate.Value.Year;
                        int monthE = monthEnd.SelectedDate.Value.Year;
                        int monthF = monthBegin.SelectedDate.Value.Month;
                        int monthT = monthEnd.SelectedDate.Value.Month;
                        dt = s_SuperStation_lijingpuService.GetDataList("MonthOri", pointIds, monthB, monthE, monthF, monthT).ToTable();
                    }
                }
            }

            dt.Columns.Add("PointName", typeof(string));
            dt.Columns["PointName"].SetOrdinal(0);
            if (dt.Columns.Contains("DateTime"))
            {
                dt.Columns["DateTime"].ColumnName = "日期";
            }
            if (dt.Columns.Contains("ReciveDateTime"))
            {
                dt.Columns["ReciveDateTime"].ColumnName = "日期";
            }
            if (dt.Columns.Contains("ReportDateTime"))
            {
                dt.Columns["ReportDateTime"].ColumnName = "日期";
            }
            if (dt.Columns.Contains("Year"))
            {
                dt.Columns["Year"].ColumnName = "年份";
            }
            if (dt.Columns.Contains("WeekOfYear"))
            {
                dt.Columns["WeekOfYear"].ColumnName = "周";
            }
            if (dt.Columns.Contains("MonthOfYear"))
            {
                dt.Columns["MonthOfYear"].ColumnName = "月份";
            }
            if (dt.Columns.Contains("SeasonOfYear"))
            {
                dt.Columns["SeasonOfYear"].ColumnName = "季";
            }
            var dv = s_SuperStation_lijingpuService.getLiJingConfig();
            for (int i = 1; i <= 35; i++)
            {
                if (dt.Columns.Contains("data" + i))
                {
                    string name = dv.ToTable().Select("DataCount='" + "data" + i + "'").FirstOrDefault()["DataContent"].ToString();
                    if (name == null || name == "")
                    {
                        dt.Columns.Remove("data" + i);
                        continue;
                    }
                    dt.Columns["data" + i].ColumnName = name;
                }
            }
            foreach (DataRow dr in dt.Rows)
            {
                string pointid = dr["PointId"].ToString();
                points = pointCbxRsm.GetPoints();
                IPoint point = points.FirstOrDefault(x => x.PointID.Equals(pointid));
                dr["PointName"] = point.PointName;
            }
            dt.Columns.Remove("PointId");
            dt.Columns["PointName"].ColumnName = "测点";
            ExcelHelper.DataTableToExcel(dt, "粒径谱数据", "粒径谱数据", this.Page);
        }

        protected void gridAudit_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }

        protected void gridAudit_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                if (item["PointId"] != null)
                {
                    GridTableCell pointCell = (GridTableCell)item["PointId"];
                    points = pointCbxRsm.GetPoints();
                    IPoint point = points.FirstOrDefault(x => x.PointID.Equals(pointCell.Text.Trim()));
                    if (points != null)
                        pointCell.Text = point.PointName;
                }
            }
        }

        protected void gridAudit_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {
            try
            {
                if (e.Column.ColumnType.Equals("GridExpandColumn"))
                    return;
                //追加测点
                GridBoundColumn col = (GridBoundColumn)e.Column;
                if (col.DataField == "PointId")
                {
                    col.HeaderText = "测点";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(80);
                    col.ItemStyle.Width = Unit.Pixel(80);
                }
                else if (col.DataField == "DateTime")
                {
                    col = (GridDateTimeColumn)e.Column;
                    string tstcolformat = "{0:yyyy-MM-dd HH:mm:ss}";
                    col.HeaderText = "日期";
                    col.DataFormatString = tstcolformat;
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(200);
                    col.ItemStyle.Width = Unit.Pixel(200);
                }
                else if (col.DataField == "ReciveDateTime")
                {
                    col = (GridDateTimeColumn)e.Column;
                    string tstcolformat = "{0:yyyy-MM-dd HH:mm:ss}";
                    col.HeaderText = "日期";
                    col.DataFormatString = tstcolformat;
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(200);
                    col.ItemStyle.Width = Unit.Pixel(200);
                }
                else if (col.DataField == "ReportDateTime")
                {
                    col = (GridDateTimeColumn)e.Column;
                    string tstcolformat = "{0:yyyy-MM-dd HH:mm:ss}";
                    col.HeaderText = "日期";
                    col.DataFormatString = tstcolformat;
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(200);
                    col.ItemStyle.Width = Unit.Pixel(200);
                }
                else if (col.DataField == "Year")
                {
                    col = (GridNumericColumn)e.Column;
                    col.HeaderText = "年份";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(120);
                    col.ItemStyle.Width = Unit.Pixel(120);
                }
                else if (col.DataField == "WeekOfYear")
                {
                    col = (GridNumericColumn)e.Column;
                    col.HeaderText = "周";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(120);
                    col.ItemStyle.Width = Unit.Pixel(120);
                }
                else if (col.DataField == "MonthOfYear")
                {
                    col = (GridNumericColumn)e.Column;
                    col.HeaderText = "月份";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(120);
                    col.ItemStyle.Width = Unit.Pixel(120);
                }
                else if (col.DataField == "SeasonOfYear")
                {
                    col = (GridNumericColumn)e.Column;
                    col.HeaderText = "季";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(120);
                    col.ItemStyle.Width = Unit.Pixel(120);
                }
                else if ((col.DataField).Substring(0, 4).ToString() == "data")
                {
                    //粒径配置
                    var dv = s_SuperStation_lijingpuService.getLiJingConfig();
                    string name = dv.ToTable().Select("DataCount='" + col.DataField.ToString() + "'").FirstOrDefault()["DataContent"].ToString();
                    if (name == null || name == "")
                    {
                        col.Visible = false;
                    }
                    col.HeaderText = name;
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(120);
                    col.ItemStyle.Width = Unit.Pixel(120);
                }
                else if (col.DataField != "id" && col.DataField != "OrderByNum" && col.DataField != "Description" && col.DataField != "CreateUser" && col.DataField != "CreateTime")
                {
                    col.HeaderText = col.DataField;
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(80);
                    col.ItemStyle.Width = Unit.Pixel(80);
                }
                else
                {
                    e.Column.Visible = false;
                }
            }
            catch (Exception ex)
            {
            }
        }

        public bool IsOrNotNumber(string a)
        {
            decimal d = 0;
            if (decimal.TryParse(a, out d) == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        protected void preSearch_Click(object sender, EventArgs e)
        {
            if (int.Parse(HiddenNum.Value) - 1 >= 0)
            {
                HiddenNum.Value = (int.Parse(HiddenNum.Value) - 1).ToString();
            }
            else
            {
                HiddenNum.Value = "0";
            }


            gridAudit.CurrentPageIndex = 0;
            gridAudit.Rebind();
            if (tabStrip.SelectedTab.Text == "图表")
            {
                RegisterScript("chart();");
            }
            else
            {
                FirstLoadChart.Value = "1";
            }
        }

        protected void nextSearch_Click(object sender, EventArgs e)
        {
            HiddenNum.Value = (int.Parse(HiddenNum.Value) + 1).ToString();
            gridAudit.CurrentPageIndex = 0;
            gridAudit.Rebind();
            if (tabStrip.SelectedTab.Text == "图表")
            {
                RegisterScript("chart();");
            }
            else
            {
                FirstLoadChart.Value = "1";
            }
        }

        /// <summary>
        /// 原始数据数据类型时间框选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void radlDataTypeOri_SelectedIndexChanged(object sender, EventArgs e)
        {
            gridAudit.CurrentPageIndex = 0;
            //一分钟数据
            if (radlDataTypeOri.SelectedValue == "Min1")
            {
                dtpHour.Visible = true;
                dbtHour.Visible = false;
                dbtDay.Visible = false;
                dbtMonth.Visible = false;
                dbtSeason.Visible = false;
                dbtYear.Visible = false;
                dbtWeek.Visible = false;
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
                dbtSeason.Visible = false;
                dbtYear.Visible = false;
                dbtWeek.Visible = false;
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
                dbtSeason.Visible = false;
                dbtYear.Visible = false;
                dbtWeek.Visible = false;
                dtpBegin.DateInput.DateFormat = "yyyy-MM-dd HH:00";
                dtpEnd.DateInput.DateFormat = "yyyy-MM-dd HH:00";
            }
            //日数据
            else if (radlDataTypeOri.SelectedValue == "Day")
            {
                dtpHour.Visible = false;
                dbtDay.Visible = true;
                dbtHour.Visible = false;
                dbtMonth.Visible = false;
                dbtYear.Visible = false;
                dbtWeek.Visible = false;
                dbtSeason.Visible = false;
            }
            //月数据
            else if (radlDataTypeOri.SelectedValue == "Month")
            {
                dtpHour.Visible = false;
                dbtMonth.Visible = true;
                dbtDay.Visible = false;
                dbtHour.Visible = false;
                dbtYear.Visible = false;
                dbtWeek.Visible = false;
                dbtSeason.Visible = false;
            }
        }
        /// <summary>
        /// 审核数据数据类型时间框选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void radlDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            gridAudit.CurrentPageIndex = 0;
            //小时数据
            if (radlDataType.SelectedValue == "Hour")
            {
                dtpHour.Visible = false;
                dbtHour.Visible = true;
                dbtDay.Visible = false;
                dbtMonth.Visible = false;
                dbtYear.Visible = false;
                dbtWeek.Visible = false;
                dbtSeason.Visible = false;
            }
            //日数据
            else if (radlDataType.SelectedValue == "Day")
            {
                dtpHour.Visible = false;
                dbtDay.Visible = true;
                dbtHour.Visible = false;
                dbtMonth.Visible = false;
                dbtYear.Visible = false;
                dbtWeek.Visible = false;
                dbtSeason.Visible = false;
            }
            //周数据
            else if (radlDataType.SelectedValue == "Week")
            {
                dtpHour.Visible = false;
                dbtWeek.Visible = true;
                dbtYear.Visible = false;
                dbtMonth.Visible = false;
                dbtDay.Visible = false;
                dbtHour.Visible = false;
                dbtSeason.Visible = false;
            }
            //月数据
            else if (radlDataType.SelectedValue == "Month")
            {
                dtpHour.Visible = false;
                dbtMonth.Visible = true;
                dbtDay.Visible = false;
                dbtHour.Visible = false;
                dbtYear.Visible = false;
                dbtWeek.Visible = false;
                dbtSeason.Visible = false;
            }
            //季数据
            else if (radlDataType.SelectedValue == "Season")
            {
                dtpHour.Visible = false;
                dbtSeason.Visible = true;
                dbtMonth.Visible = false;
                dbtDay.Visible = false;
                dbtHour.Visible = false;
                dbtYear.Visible = false;
                dbtWeek.Visible = false;

            }
            //年数据
            else if (radlDataType.SelectedValue == "Year")
            {
                dtpHour.Visible = false;
                dbtYear.Visible = true;
                dbtMonth.Visible = false;
                dbtDay.Visible = false;
                dbtHour.Visible = false;
                dbtWeek.Visible = false;
                dbtSeason.Visible = false;
            }
        }

        /// <summary>
        /// 数据来源选项变化，数据类型选项相应变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlDataSource_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            if (ddlDataSource.SelectedIndex == 0)
            {
                radlDataTypeOri.Visible = true;
                radlDataType.Visible = false;
                radlDataTypeOri.SelectedIndex = 2;
            }
            else
            {
                radlDataTypeOri.Visible = false;
                radlDataType.Visible = true;
                radlDataTypeOri.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 修改要导出的数据表中的列名
        /// </summary>
        /// <param name="dv">原始数据表</param>
        /// <returns></returns>
        //private DataTable UpdateExportColumnName(DataView dv, DataView dvStatistical)
        //{
        //    DataTable dtNew = dv.ToTable();
        //    dtNew.Columns.Add("测点", typeof(string)).SetOrdinal(0);
        //    points = pointCbxRsm.GetPoints();
        //    for (int i = 0; i < dtNew.Rows.Count; i++)
        //    {
        //        DataRow drNew = dtNew.Rows[i];
        //        drNew["测点"] = (points.Count(t => t.PointID == drNew["PointId"].ToString()) > 0)
        //            ? points.Where(t => t.PointID == drNew["PointId"].ToString()).Select(t => t.PointName).FirstOrDefault()
        //            : drNew["PointId"].ToString();
        //    }
        //    for (int i = 0; i < dtNew.Columns.Count; i++)
        //    {
        //        DataColumn dcNew = dtNew.Columns[i];
        //        //追加日期
        //        if (dcNew.ColumnName == "Tstamp")
        //        {
        //            string tstcolformat = "{0:MM-dd HH:mm}";
        //            dcNew.ColumnName = "日期";
        //        }
        //        else if (dcNew.ColumnName == "DateTime")
        //        {
        //            string tstcolformat = "{0:yyyy-MM-dd}";
        //            dcNew.ColumnName = "日期";
        //        }
        //        else if (dcNew.ColumnName == "Year")
        //        {
        //            dcNew.ColumnName = "年份";
        //        }
        //        else if (dcNew.ColumnName == "WeekOfYear")
        //        {
        //            dcNew.ColumnName = "周";
        //        }
        //        else if (dcNew.ColumnName == "MonthOfYear")
        //        {
        //            dcNew.ColumnName = "月份";
        //        }
        //        else if (dcNew.ColumnName == "SeasonOfYear")
        //        {
        //            dcNew.ColumnName = "季";
        //        }
        //        else if (dcNew.ColumnName == "序号" || dcNew.ColumnName == "PointId")
        //        {
        //            dtNew.Columns.Remove(dcNew);
        //            i--;
        //        }
        //    }
        //    if (dvStatistical != null && dvStatistical.Table.Rows.Count > 0)
        //    {
        //        DataTable dtStatistical = dvStatistical.Table;
        //        DataRow drMaxRow = dtNew.NewRow();
        //        drMaxRow["测点"] = "最大值";
        //        DataRow drMinRow = dtNew.NewRow();
        //        drMinRow["测点"] = "最小值";
        //        DataRow drAvgRow = dtNew.NewRow();
        //        drAvgRow["测点"] = "平均值";
        //        for (int i = 0; i < dtStatistical.Rows.Count; i++)
        //        {
        //            DataRow drStatistical = dtStatistical.Rows[i];
        //            if (drStatistical["PollutantCode"] != DBNull.Value && drStatistical["PollutantCode"].ToString() != "")
        //            {
        //                IPollutant factor = factorCbxRsm.GetFactors().FirstOrDefault(x => x.PollutantCode.Equals(drStatistical["PollutantCode"].ToString()));
        //                int pdn = 0;
        //                if (factor != null)
        //                {
        //                    pdn = Convert.ToInt32(string.IsNullOrEmpty(factor.PollutantDecimalNum) ? 3 : Convert.ToInt32(factor.PollutantDecimalNum));
        //                }
        //                if (dtNew.Columns.Contains(factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"))
        //                {
        //                    drMaxRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"] = drStatistical["Value_Max"] != DBNull.Value ? drStatistical["Value_Max"] : "--";
        //                    drMinRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"] = drStatistical["Value_Min"] != DBNull.Value ? drStatistical["Value_Min"] : "--";
        //                    drAvgRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"] = drStatistical["Value_Avg"] != DBNull.Value ? drStatistical["Value_Avg"] : "--";

        //                    if (drMaxRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"].ToString() != "--")
        //                    {
        //                        decimal AVG = Convert.ToDecimal(drMaxRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"]);
        //                        drMaxRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"] = DecimalExtension.GetPollutantValue(AVG, pdn).ToString();
        //                    }
        //                    if (drMinRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"].ToString() != "--")
        //                    {
        //                        decimal MAX = Convert.ToDecimal(drMinRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"]);
        //                        drMinRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"] = DecimalExtension.GetPollutantValue(MAX, pdn).ToString();
        //                    }
        //                    if (drAvgRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"].ToString() != "--")
        //                    {
        //                        decimal MIN = Convert.ToDecimal(drAvgRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"]);
        //                        drAvgRow[factor.PollutantName + "(" + factor.PollutantMeasureUnit + ")"] = DecimalExtension.GetPollutantValue(MIN, pdn).ToString();
        //                    }
        //                }
        //            }
        //        }
        //        dtNew.Rows.Add(drAvgRow);
        //        dtNew.Rows.Add(drMaxRow);
        //        dtNew.Rows.Add(drMinRow);
        //    }
        //    return dtNew;
        //}
    }
}