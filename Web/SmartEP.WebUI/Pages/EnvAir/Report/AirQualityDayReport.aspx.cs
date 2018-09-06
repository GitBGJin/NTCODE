using Aspose.Cells;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.BusinessRule;
using SmartEP.Service.BaseData.Channel;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.Core.Enums;
using SmartEP.Service.DataAnalyze.Air.AQIReport;
using SmartEP.Service.DataAnalyze.Enums;
using SmartEP.Service.Frame;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Utilities.Office;
using SmartEP.Utilities.Web.UI;
using SmartEP.WebControl.CbxRsm;
using SmartEP.WebUI.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Configuration;
using SmartEP.Core.Enums;
using SmartEP.Service.DataAnalyze.Air;
using log4net;
using System.Text;

namespace SmartEP.WebUI.Pages.EnvAir.Report
{
    /// <summary>
    /// 名称：AirQualityDayReport.cs
    /// 创建人：王秉晟
    /// 创建日期：2015-08-14
    /// 维护人员：
    /// 最新维护人员：徐阳
    /// 最新维护日期：2017-05-24
    /// 功能摘要：空气质量日报
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class AirQualityDayReport : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        private DayAQIService m_DayAQIService;
        private HourAQIService m_HourAQIService;
        private AQICalculateService m_AQICalculateService;


        /// <summary>
        /// 空气站点信息服务
        /// </summary>
        private MonitoringPointAirService pointAirService;

        /// <summary>
        /// 选择站点
        /// </summary>
        private IList<IPoint> points = null;

        /// <summary>
        /// 选择因子
        /// </summary>
        private IList<PollutantCodeEntity> factors = null;

        /// <summary>
        /// 区域Uid集合
        /// </summary>
        List<string> listRegionUids = new List<string>();

        ILog log = LogManager.GetLogger("FileLogging");//获取一个日志记录器
        static DateTime dtms = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00"));
        protected void Page_Load(object sender, EventArgs e)
        {
            m_DayAQIService = new DayAQIService();
            m_HourAQIService = new HourAQIService();
            m_AQICalculateService = new AQICalculateService();
            pointAirService = new MonitoringPointAirService();

            if (!IsPostBack)
            {
                InitControl();
                //timer.Enabled = true;
            }
        }

        #region 页面控件初始化
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            //数据类型控件
            radlDataType.Items.Add(new ListItem("小时数据", SmartEP.Core.Enums.PollutantDataType.Hour.ToString()));
            radlDataType.Items.Add(new ListItem("日数据", SmartEP.Core.Enums.PollutantDataType.Day.ToString()));
            radlDataType.SelectedValue = SmartEP.Core.Enums.PollutantDataType.Hour.ToString();

            string regionUid = PageHelper.GetQueryString("regionUid");
            string DateBegin = PageHelper.GetQueryString("DateBegin");
            string DateEnd = PageHelper.GetQueryString("DateEnd");
            string portName = PageHelper.GetQueryString("portName");
            string type = PageHelper.GetQueryString("type");
            string starttime = PageHelper.GetQueryString("starttime");
            string endtime = PageHelper.GetQueryString("endtime");
            string days = PageHelper.GetQueryString("days");
            if (regionUid != "")
            {
                rbtnlType.SelectedValue = "CityProper";
                dtpBegin.SelectedDate = Convert.ToDateTime(DateTime.Parse(DateBegin).ToString("yyyy-MM-dd"));
                dtpEnd.SelectedDate = Convert.ToDateTime(DateTime.Parse(DateEnd).ToString("yyyy-MM-dd"));

                hourBegin.SelectedDate = Convert.ToDateTime(DateTime.Parse(DateBegin).ToString("yyyy-MM-dd HH:00"));
                hourEnd.SelectedDate = Convert.ToDateTime(DateTime.Parse(DateEnd).ToString("yyyy-MM-dd HH:00"));
            }

            else
            {
                dtpBegin.SelectedDate = DateTime.Parse(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
                dtpEnd.SelectedDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));

                hourBegin.SelectedDate = DateTime.Parse(DateTime.Now.AddHours(-2).ToString("yyyy-MM-dd HH:00"));
                hourEnd.SelectedDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:00"));
            }
            if (type == "Port")
            {
                gridDayAQI.Visible = true;
                gridRealTimeAQI.Visible = false;
                divType.Visible = true;
                divTypeContent.Visible = true;
                dtpBegin.Visible = true;
                dtpEnd.Visible = true;
                hourBegin.Visible = false;
                hourEnd.Visible = false;
                rbtnlType.SelectedValue = "Port";
                radlDataType.SelectedValue = "Day";
                ddlDataFrom.SelectedValue = "AuditData";
                pointCbxRsm.SetPointValuesFromNames(portName);
                string[] types = { };
                switch (days)
                {
                    case "StandardDays":
                        types = new string[] { "优", "良" };
                        break;
                    case "OverDays":
                        types = new string[] { "轻度污染", "中度污染", "重度污染", "严重污染" };
                        break;
                    case "InvalidDays":
                        types = new string[] { "无效天" };
                        break;
                    case "Good":
                        types = new string[] { "优" };
                        break;
                    case "Moderate":
                        types = new string[] { "良" };
                        break;
                    case "LightlyPolluted":
                        types = new string[] { "轻度污染" };
                        break;
                    case "ModeratelyPolluted":
                        types = new string[] { "中度污染" };
                        break;
                    case "HeavilyPolluted":
                        types = new string[] { "重度污染" };
                        break;
                    case "SeverelyPolluted":
                        types = new string[] { "严重污染" };
                        break;
                    default:
                        types = new string[] { "优", "良", "轻度污染", "中度污染", "重度污染", "严重污染", "无效天" };
                        break;
                }

                rcbCityProper.ClearCheckedItems();
                foreach (RadComboBoxItem item in rcbCityProper.Items)
                {
                    for (int i = 0; i < types.Length; i++)
                    {
                        if (item.Text == types[i])
                        {
                            item.Checked = true;
                        }
                    }
                }
            }
            else if (type == "CityProper")
            {
                gridDayAQI.Visible = true;
                gridRealTimeAQI.Visible = false;
                divType.Visible = true;
                divTypeContent.Visible = true;
                dtpBegin.Visible = true;
                dtpEnd.Visible = true;
                hourBegin.Visible = false;
                hourEnd.Visible = false;
                rbtnlType.SelectedValue = "CityProper";
                radlDataType.SelectedValue = "Day";
                ddlDataFrom.SelectedValue = "AuditData";
                string[] types = { };
                switch (days)
                {
                    case "StandardDays":
                        types = new string[] { "优", "良" };
                        break;
                    case "OverDays":
                        types = new string[] { "轻度污染", "中度污染", "重度污染", "严重污染" };
                        break;
                    case "InvalidDays":
                        types = new string[] { "无效天" };
                        break;
                    case "Good":
                        types = new string[] { "优" };
                        break;
                    case "Moderate":
                        types = new string[] { "良" };
                        break;
                    case "LightlyPolluted":
                        types = new string[] { "轻度污染" };
                        break;
                    case "ModeratelyPolluted":
                        types = new string[] { "中度污染" };
                        break;
                    case "HeavilyPolluted":
                        types = new string[] { "重度污染" };
                        break;
                    case "SeverelyPolluted":
                        types = new string[] { "严重污染" };
                        break;
                    default:
                        types = new string[] { "优", "良", "轻度污染", "中度污染", "重度污染", "严重污染", "无效天" };
                        break;
                }

                rcbCityProper.ClearCheckedItems();
                foreach (RadComboBoxItem item in rcbCityProper.Items)
                {
                    for (int i = 0; i < types.Length; i++)
                    {
                        if (item.Text == types[i])
                        {
                            item.Checked = true;
                        }
                    }
                }
                string portId = PageHelper.GetQueryString("portId");
                pointCbxRsm.SetPointValuesFromNames(portId);

            }
            else
            {
                string names = ConfigurationManager.AppSettings["NTRegionPointName"].ToString();    //从配置文件获取默认站点
                pointCbxRsm.SetPointValuesFromNames(names);
            }
            if (starttime != "")
            {
                dtpBegin.SelectedDate = Convert.ToDateTime(DateTime.Parse(starttime).ToString("yyyy-MM-dd"));
            }
            if (endtime != "")
            {
                dtpEnd.SelectedDate = Convert.ToDateTime(DateTime.Parse(endtime).ToString("yyyy-MM-dd"));
            }

            //string UserGuid = Session["UserGuid"].ToString();
            //string CfgUserGuid = ConfigurationManager.AppSettings["ManagerRole"].ToString();
            //string CfgAreaSelect = ConfigurationManager.AppSettings["AreaSelect"].ToString();
            //if (UserGuid.Equals(CfgUserGuid))
            //{
            //    foreach (RadComboBoxItem item in comboCity.Items)
            //    {
            //        if (item.Value.ToString().Equals(CfgAreaSelect))
            //        {
            //            item.Checked = true;
            //            item.Visible = true;
            //        }
            //        else
            //        {
            //            item.Checked = false;
            //            item.Visible = false;
            //        }
            //    }
            //}
        }
        #endregion

        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {
            //log.Info("绑定开始-------");
            try
            {
                #region
                //string UserGuid = Session["UserGuid"].ToString();
                //string CfgUserGuid = ConfigurationManager.AppSettings["ManagerRole"].ToString();
                //string CfgAreaSelect = ConfigurationManager.AppSettings["AreaSelect"].ToString();
                //if (UserGuid.Equals(CfgUserGuid))
                //{
                //    foreach (RadComboBoxItem item in comboCity.Items)
                //    {
                //        if (item.Value.ToString().Equals(CfgAreaSelect))
                //        {
                //            item.Checked = true;
                //            item.Visible = true;
                //        }
                //        else
                //        {
                //            item.Checked = false;
                //            item.Visible = false;
                //        }
                //    }
                //}
                //string orderByAQI = PageHelper.GetQueryString("orderBy");
                if (dtpBegin.SelectedDate == null || dtpEnd.SelectedDate == null)
                {
                    return;
                }
                else if (dtpBegin.SelectedDate > dtpEnd.SelectedDate)
                {
                    return;
                }
                string rcbType = "";
                foreach (RadComboBoxItem item in rcbCityProper.CheckedItems)
                {
                    rcbType += (item.Text.ToString() + ",");
                }
                string[] qulityType = rcbType.Trim(',').Split(',');
                DateTime dtmBegion = dtpBegin.SelectedDate.Value;
                DateTime dtmEnd = dtpEnd.SelectedDate.Value;
                DateTime dtmhourBegin = hourBegin.SelectedDate.Value;
                DateTime dtmhourEnd = hourEnd.SelectedDate.Value;
                points = pointCbxRsm.GetPoints();
                factors = GetPollutantListByCalAQI();//获取参与评价AQI的常规6因子
                int pageSize = gridDayAQI.PageSize;  //每页显示数据个数  
                int pageNo = gridDayAQI.CurrentPageIndex;   //当前页的序号
                int pageSizeHour = gridRealTimeAQI.PageSize;  //小时每页显示数据个数  
                int pageNoHour = gridRealTimeAQI.CurrentPageIndex;   //小时当前页的序号
                int recordTotal = 0;  //数据总行数
                var dataView = new DataView();
                var dataViewHour = new DataView();
                string orderBy = "";

                if (rbtnlType.SelectedValue == "Port")
                {
                    //log.Info("测点计算开始-------");
                    if (points != null && points.Count > 0)
                    {
                        orderBy = "PointId,DateTime Desc";
                        if (TimeSort.SelectedValue == "时间升序")
                            orderBy = "PointId,DateTime Asc";
                        if (radlDataType.SelectedValue == "Day" && ddlDataFrom.SelectedValue == "AuditData")
                        {
                            string[] typeArr = { "优", "良", "轻度污染", "中度污染", "重度污染", "严重污染", "无效天" };
                            dataView = m_DayAQIService.GetAirQualityDayReportNew(points.Select(t => t.PointID).ToArray(), dtmBegion, dtmEnd, typeArr, pageSize, pageNo, out recordTotal, orderBy);
                            StringBuilder sb = new StringBuilder();    //区域数据筛选条件
                            for (int i = 0; i < qulityType.Length; i++)
                            {
                                sb.Append("'" + qulityType[i] + "',");
                            }
                            string filter = sb.ToString().Substring(0, sb.Length - 1);
                            if (filter.Contains("无效天"))
                            {
                                if (qulityType.Length > 1)
                                {
                                    dataView.RowFilter = "Class in (" + sb.ToString().Substring(0, sb.Length - 1) + ") or Class is null or Class = ''";
                                }
                                else
                                {
                                    dataView.RowFilter = "Class is null or Class = ''";
                                }
                            }
                            else
                            {
                                dataView.RowFilter = "Class in (" + sb.ToString().Substring(0, sb.Length - 1) + ")";
                            }

                            //将数据存入隐藏域，供绘图使用
                            string[] pointIds = points.Select(t => t.PointID).ToArray();
                            string dtBegion = dtmBegion.ToString("yyyy-MM-dd 00:00:00");
                            string dtEnd = dtmEnd.ToString("yyyy-MM-dd 23:59:59");
                            hdPointId.Value = string.Join(",", pointIds);
                            hddtBegion.Value = dtBegion;
                            hddtEnd.Value = dtEnd;
                            hdQuality.Value = rcbType;
                            hdDSType.Value = "PDayAudit";
                        }
                        if (radlDataType.SelectedValue == "Day" && ddlDataFrom.SelectedValue == "OriData")
                        {
                            string[] typeArr = { "优", "良", "轻度污染", "中度污染", "重度污染", "严重污染", "无效天" };
                            dataView = m_DayAQIService.GetAirQualityOriDayReportNew(points.Select(t => t.PointID).ToArray(), dtmBegion, dtmEnd, typeArr, pageSize, pageNo, out recordTotal, orderBy);
                            StringBuilder sb = new StringBuilder();    //区域数据筛选条件
                            for (int i = 0; i < qulityType.Length; i++)
                            {
                                sb.Append("'" + qulityType[i] + "',");
                            }
                            string filter = sb.ToString().Substring(0, sb.Length - 1);
                            if (filter.Contains("无效天"))
                            {
                                if (qulityType.Length > 1)
                                {
                                    dataView.RowFilter = "Class in (" + sb.ToString().Substring(0, sb.Length - 1) + ") or Class is null or Class = ''";
                                }
                                else
                                {
                                    dataView.RowFilter = "Class is null or Class = ''";
                                }
                            }
                            else
                            {
                                dataView.RowFilter = "Class in (" + sb.ToString().Substring(0, sb.Length - 1) + ")";
                            }

                            //将数据存入隐藏域，供绘图使用
                            string[] pointIds = points.Select(t => t.PointID).ToArray();
                            string dtBegion = dtmBegion.ToString("yyyy-MM-dd 00:00:00");
                            string dtEnd = dtmEnd.ToString("yyyy-MM-dd 23:59:59");
                            hdPointId.Value = string.Join(",", pointIds);
                            hddtBegion.Value = dtBegion;
                            hddtEnd.Value = dtEnd;
                            hdQuality.Value = rcbType;
                            hdDSType.Value = "PDayOri";
                        }
                        if (radlDataType.SelectedValue == "Hour")
                        {
                            //小时数据区分原始审核
                            if (ddlDataFrom.SelectedValue == "OriData")
                            {
                                orderBy = "PointId,DateTime Desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderBy = "PointId,DateTime Asc";
                                dataViewHour = m_HourAQIService.GetAirQualityOriRTReport(points.Select(t => t.PointID).ToArray(), dtmhourBegin, dtmhourEnd.AddHours(1).AddSeconds(-1), pageSizeHour, pageNoHour, out recordTotal, orderBy);

                                //将数据存入隐藏域，供绘图使用
                                string[] pointIds = points.Select(t => t.PointID).ToArray();
                                string dtBegion = dtmhourBegin.ToString("yyyy-MM-dd HH:00:00");
                                string dtEnd = dtmhourEnd.AddHours(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss");
                                hdPointId.Value = string.Join(",", pointIds);
                                hddtBegion.Value = dtBegion;
                                hddtEnd.Value = dtEnd;
                                //hdQuality.Value = rcbType;
                                hdDSType.Value = "PHourOri";
                            }
                            if (ddlDataFrom.SelectedValue == "AuditData")
                            {
                                orderBy = "PointId,DateTime Desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderBy = "PointId,DateTime Asc";
                                dataViewHour = m_HourAQIService.GetAirQualityRTReport(points.Select(t => t.PointID).ToArray(), dtmhourBegin, dtmhourEnd.AddHours(1).AddSeconds(-1), pageSizeHour, pageNoHour, out recordTotal, orderBy);

                                //将数据存入隐藏域，供绘图使用
                                string[] pointIds = points.Select(t => t.PointID).ToArray();
                                string dtBegion = dtmhourBegin.ToString("yyyy-MM-dd HH:00:00");
                                string dtEnd = dtmhourEnd.AddHours(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss");
                                hdPointId.Value = string.Join(",", pointIds);
                                hddtBegion.Value = dtBegion;
                                hddtEnd.Value = dtEnd;
                                //hdQuality.Value = rcbType;
                                hdDSType.Value = "PHourAudit";
                            }

                        }
                    }
                    else
                    {
                        dataView = null;
                        dataViewHour = null;
                    }
                }
                else if (rbtnlType.SelectedValue == "CityProper")
                {
                    if (points != null && points.Count > 0)
                    {
                        string[] pointIds = points.Select(t => t.PointID).ToArray();

                        if (radlDataType.SelectedValue == "Day" && ddlDataFrom.SelectedValue == "AuditData")
                        {
                            if (CalOrCheck(points))
                            {
                                string[] typeArr = { "优", "良", "轻度污染", "中度污染", "重度污染", "严重污染", "无效天" };
                                orderBy = "DateTime Desc";
                                dataView = m_DayAQIService.GetAirQualityRegionDayReportNew(dtmBegion, dtmEnd, typeArr, pageSize, pageNo, "Audit", out recordTotal, orderBy);
                            }
                            else
                            {
                                dataView = m_AQICalculateService.GetRegionAQI(pointIds, dtmBegion, dtmEnd, 24, "2").AsDataView();
                            }

                            dataView.Sort = "PointId,DateTime Desc";
                            if (TimeSort.SelectedValue == "时间升序")
                            {
                                dataView.Sort = "PointId,DateTime Asc";
                            }
                            StringBuilder sb = new StringBuilder();    //区域数据筛选条件
                            for (int i = 0; i < qulityType.Length; i++)
                            {
                                sb.Append("'" + qulityType[i] + "',");
                            }
                            string filter = sb.ToString().Substring(0, sb.Length - 1);
                            if (filter.Contains("无效天"))
                            {
                                if (qulityType.Length > 1)
                                {
                                    dataView.RowFilter = "Class in (" + sb.ToString().Substring(0, sb.Length - 1) + ") or Class is null or Class = ''";
                                }
                                else
                                {
                                    dataView.RowFilter = "Class is null or Class = ''";
                                }
                            }
                            else
                            {
                                dataView.RowFilter = "Class in (" + sb.ToString().Substring(0, sb.Length - 1) + ")";
                            }

                            //将数据存入隐藏域，供绘图使用
                            //string[] pointIds = points.Select(t => t.PointID).ToArray();
                            string dtBegion = dtmBegion.ToString("yyyy-MM-dd 00:00:00");
                            string dtEnd = dtmEnd.ToString("yyyy-MM-dd 23:59:59");
                            hdPointId.Value = string.Join(",", pointIds);
                            hddtBegion.Value = dtBegion;
                            hddtEnd.Value = dtEnd;
                            hdDSType.Value = "RDayAudit";
                        }
                        if (radlDataType.SelectedValue == "Day" && ddlDataFrom.SelectedValue == "OriData")
                        {
                            if (CalOrCheck(points))
                            {
                                string[] typeArr = { "优", "良", "轻度污染", "中度污染", "重度污染", "严重污染", "无效天" };
                                orderBy = "DateTime Desc";
                                dataView = m_DayAQIService.GetAirQualityRegionDayReportNew(dtmBegion, dtmEnd, typeArr, pageSize, pageNo, "Ori", out recordTotal, orderBy);
                            }
                            else
                            {
                                dataView = m_AQICalculateService.GetRegionAQI(pointIds, dtmBegion, dtmEnd, 24, "1").AsDataView();
                            }

                            dataView.Sort = "PointId,DateTime Desc";
                            if (TimeSort.SelectedValue == "时间升序")
                            {
                                dataView.Sort = "PointId,DateTime Asc";
                            }
                            StringBuilder sb = new StringBuilder();    //区域数据筛选条件
                            for (int i = 0; i < qulityType.Length; i++)
                            {
                                sb.Append("'" + qulityType[i] + "',");
                            }
                            string filter = sb.ToString().Substring(0, sb.Length - 1);
                            if (filter.Contains("无效天"))
                            {
                                if (qulityType.Length > 1)
                                {
                                    dataView.RowFilter = "Class in (" + sb.ToString().Substring(0, sb.Length - 1) + ") or Class is null or Class = ''";
                                }
                                else
                                {
                                    dataView.RowFilter = "Class is null or Class = ''";
                                }
                            }
                            else
                            {
                                dataView.RowFilter = "Class in (" + sb.ToString().Substring(0, sb.Length - 1) + ")";
                            }
                            //将数据存入隐藏域，供绘图使用
                            //string[] pointIds = points.Select(t => t.PointID).ToArray();
                            string dtBegion = dtmBegion.ToString("yyyy-MM-dd 00:00:00");
                            string dtEnd = dtmEnd.ToString("yyyy-MM-dd 23:59:59");
                            hdPointId.Value = string.Join(",", pointIds);
                            hddtBegion.Value = dtBegion;
                            hddtEnd.Value = dtEnd;
                            //hdQuality.Value = rcbType;
                            hdDSType.Value = "RDayOri";
                        }
                        if (radlDataType.SelectedValue == "Hour")
                        {
                            //区域小时原始数据
                            if (ddlDataFrom.SelectedValue == "OriData")
                            {
                                if (CalOrCheck(points))
                                {
                                    orderBy = "DateTime Desc";
                                    if (TimeSort.SelectedValue == "时间升序")
                                    {
                                        orderBy = "DateTime Asc";
                                    }
                                    dataViewHour = m_HourAQIService.GetAirQualityRegionRTReport(dtmhourBegin, dtmhourEnd.AddHours(1).AddSeconds(-1), pageSizeHour, pageNoHour, "Ori", out recordTotal, orderBy).AsDataView();
                                }
                                else
                                {
                                    dataViewHour = m_AQICalculateService.GetRegionAQI(pointIds, dtmhourBegin, dtmhourEnd, 1, "1").AsDataView();
                                    recordTotal = dataViewHour.ToTable().Rows.Count;
                                }

                                //将数据存入隐藏域，供绘图使用
                                string dtBegion = dtmhourBegin.ToString("yyyy-MM-dd HH:00:00");
                                string dtEnd = dtmhourEnd.AddHours(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss");
                                hdPointId.Value = string.Join(",", pointIds);
                                hddtBegion.Value = dtBegion;
                                hddtEnd.Value = dtEnd;
                                hdDSType.Value = "RHourOri";
                            }
                            if (ddlDataFrom.SelectedValue == "AuditData")
                            {
                                if (CalOrCheck(points))
                                {
                                    orderBy = "DateTime Desc";
                                    if (TimeSort.SelectedValue == "时间升序")
                                    {
                                        orderBy = "DateTime Asc";
                                    }
                                    dataViewHour = m_HourAQIService.GetAirQualityRegionRTReport(dtmhourBegin, dtmhourEnd.AddHours(1).AddSeconds(-1), pageSizeHour, pageNoHour, "Audit", out recordTotal, orderBy).AsDataView();
                                }
                                else
                                {
                                    dataViewHour = m_AQICalculateService.GetRegionAQI(pointIds, dtmhourBegin, dtmhourEnd, 1, "2").AsDataView();
                                    recordTotal = dataViewHour.ToTable().Rows.Count;
                                }
                                //将数据存入隐藏域，供绘图使用
                                string dtBegion = dtmhourBegin.ToString("yyyy-MM-dd HH:00:00");
                                string dtEnd = dtmhourEnd.AddHours(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss");
                                hdPointId.Value = string.Join(",", pointIds);
                                hddtBegion.Value = dtBegion;
                                hddtEnd.Value = dtEnd;
                                hdDSType.Value = "RHourAudit";
                            }
                            dataViewHour.Sort = "PointId,DateTime Desc";
                            if (TimeSort.SelectedValue == "时间升序")
                            {
                                dataViewHour.Sort = "PointId,DateTime Asc";
                            }
                        }
                    }
                    else
                    {
                        dataView = null;
                        dataViewHour = null;
                    }
                }

                if (radlDataType.SelectedValue == "Day")
                {
                    if (dataView == null)
                    {
                        gridDayAQI.DataSource = new DataTable();
                    }
                    if (dataView != null)
                    {
                        gridDayAQI.DataSource = dataView;
                    }
                    //if (rbtnlType.SelectedValue == "Port")
                    //{
                    //    gridDayAQI.VirtualItemCount = recordTotal;
                    //}
                    //if (rbtnlType.SelectedValue == "CityProper" && dataView != null)
                    //{
                    gridDayAQI.VirtualItemCount = dataView.ToTable().Rows.Count;
                    //}
                }
                if (radlDataType.SelectedValue == "Hour")
                {
                    if (dataViewHour == null)
                    {
                        gridRealTimeAQI.DataSource = new DataTable();
                    }
                    if (dataViewHour != null)
                    {
                        gridRealTimeAQI.DataSource = dataViewHour;
                    }
                    if (rbtnlType.SelectedValue == "Port")
                    {
                        gridRealTimeAQI.VirtualItemCount = recordTotal;
                    }
                    if (rbtnlType.SelectedValue == "CityProper" && dataViewHour != null)
                    {
                        //gridRealTimeAQI.VirtualItemCount = dataViewHour.ToTable().Rows.Count;
                        gridRealTimeAQI.VirtualItemCount = recordTotal;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                log.Info(ex.ToString());
            }
        }
        /// <summary>
        /// 获取根据单位转换浓度值后的新数据视图
        /// </summary>
        /// <param name="dv"></param>
        /// <returns></returns>
        private DataView GetNewViewByTurnData(DataView dv)
        {
            DataView dvNew = new DataView();
            DataTable dt = dv.ToTable();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];

                //mg/m3转成μg/m3的浓度值
                if (!string.IsNullOrWhiteSpace(dr["SO2"].ToString()))
                {
                    dr["SO2"] = (decimal.Parse(dr["SO2"].ToString()) * 1000).ToString("G0");
                }
                if (!string.IsNullOrWhiteSpace(dr["NO2"].ToString()))
                {
                    dr["NO2"] = (decimal.Parse(dr["NO2"].ToString()) * 1000).ToString("G0");
                }
                if (!string.IsNullOrWhiteSpace(dr["PM10"].ToString()))
                {
                    dr["PM10"] = (decimal.Parse(dr["PM10"].ToString()) * 1000).ToString("G0");
                }
                if (!string.IsNullOrWhiteSpace(dr["PM25"].ToString()))
                {
                    dr["PM25"] = (decimal.Parse(dr["PM25"].ToString()) * 1000).ToString("G0");
                }
                if (!string.IsNullOrWhiteSpace(dr["Max8HourO3"].ToString()))
                {
                    dr["Max8HourO3"] = (decimal.Parse(dr["Max8HourO3"].ToString()) * 1000).ToString("G0");
                }
                if (!string.IsNullOrWhiteSpace(dr["MaxOneHourO3"].ToString()))
                {
                    dr["MaxOneHourO3"] = (decimal.Parse(dr["MaxOneHourO3"].ToString()) * 1000).ToString("G0");
                }
            }
            dvNew = dt.AsDataView();
            return dvNew;
        }
        #endregion

        #region 服务器端控件事件处理
        /// <summary>
        /// 绑定数据源（用于分页、排序）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }

        /// <summary>
        /// 日数据行绑定处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grid_ItemDataBoundDay(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                DataRowView drv = e.Item.DataItem as DataRowView;
                if (item["PointId"] != null)
                {
                    GridTableCell pointCell = (GridTableCell)item["PointId"];
                    GridTableCell pointDateTime = (GridTableCell)item["DateTime"];
                    if (rbtnlType.SelectedValue == "Port")
                    {
                        IPoint point = points.FirstOrDefault(x => x.PointID.Equals(drv["PointId"].ToString().Trim()));
                        if (point != null)
                            pointCell.Text = point.PointName;
                        pointDateTime.Text = string.Format("{0:yyyy-MM-dd}", drv["DateTime"]);
                    }
                }
                if (item["RGBValue"] != null)
                {
                    GridTableCell cell = item["RGBValue"] as GridTableCell;
                    cell.Style.Add("background-color", cell.Text);
                    cell.Text = string.Empty;
                }
                //if (item["MaxOneHourO3"] != null && item["MaxOneHourO3"].Text.ToString() != "--")
                //{
                //    GridTableCell cell = item["MaxOneHourO3"] as GridTableCell;
                //    double DecimalNum = double.Parse((cell.Text).ToString());
                //    cell.Text = (DecimalNum * 1000).ToString("G0");
                //}
                for (int i = 0; i < factors.Count; i++)
                {
                    string[] uniqueNames = GetUniqueNameByPollutantNameDay(factors[i].PollutantCode);
                    foreach (string uniqueName in uniqueNames)
                    {
                        if (drv.DataView.Table.Columns.Contains(uniqueName) && item[uniqueName] != null)
                        {
                            GridTableCell factorCell = (GridTableCell)item[uniqueName];
                            decimal pollutantValue;

                            if (decimal.TryParse(factorCell.Text, out pollutantValue))
                            {
                                //获取因子小数位,channelCode 因子代码 例：CO的code是a21005  （ channelCode=a21005）
                                AirPollutantService m_AirPollutantService = new AirPollutantService();
                                int DecimalNum = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo(factors[i].PollutantCode).PollutantDecimalNum);

                                //保留小数位数,value 需要进行小数位处理的数据 类型Decimal
                                if (uniqueName == "CO")
                                {
                                    factorCell.Text = DecimalExtension.GetPollutantValue(pollutantValue, DecimalNum).ToString();
                                }
                                else
                                {
                                    factorCell.Text = (DecimalExtension.GetPollutantValue(pollutantValue, DecimalNum) * 1000).ToString("G0");
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 小时数据行绑定处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grid_ItemDataBoundHour(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                DataRowView drv = e.Item.DataItem as DataRowView;
                if (item["PointId"] != null)
                {
                    GridTableCell pointCell = (GridTableCell)item["PointId"];
                    if (rbtnlType.SelectedValue == "Port")
                    {
                        IPoint point = points.FirstOrDefault(x => x.PointID.Equals(drv["PointId"].ToString().Trim()));
                        if (point != null)
                            pointCell.Text = point.PointName;
                    }
                    else if (rbtnlType.SelectedValue == "City")
                    {
                    }
                }
                if (item["DateTime"] != null)
                {
                    GridTableCell dateTimeCell = (GridTableCell)item["DateTime"];
                    DateTime dateTime;
                    if (DateTime.TryParse(dateTimeCell.Text, out dateTime))
                    {
                        dateTimeCell.Text = dateTime.ToString("MM-dd HH:00");
                    }
                }
                if (item["RGBValue"] != null)
                {
                    GridTableCell cell = item["RGBValue"] as GridTableCell;
                    cell.Style.Add("background-color", cell.Text);
                    cell.Text = string.Empty;
                }
                for (int i = 0; i < factors.Count; i++)
                {
                    string[] uniqueNames = GetUniqueNameByPollutantNameHour(factors[i].PollutantCode);
                    foreach (string uniqueName in uniqueNames)
                    {
                        if (drv.DataView.Table.Columns.Contains(uniqueName) && item[uniqueName] != null)
                        {
                            GridTableCell factorCell = (GridTableCell)item[uniqueName];
                            decimal pollutantValue;

                            if (decimal.TryParse(factorCell.Text, out pollutantValue))
                            {
                                //获取因子小数位,channelCode 因子代码 例：CO的code是a21005  （ channelCode=a21005）
                                AirPollutantService m_AirPollutantService = new AirPollutantService();
                                DataTable dtFactor = m_AQICalculateService.GetPollutantUnit(factors[i].PollutantCode);
                                int DecimalNum = Convert.ToInt32(dtFactor.Rows[0]["DecimalDigit"]);

                                //int DecimalNum = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo(factors[i].PollutantCode).PollutantDecimalNum);
                                //保留小数位数,value 需要进行小数位处理的数据 类型Decimal
                                if (uniqueName == "CO")
                                {
                                    factorCell.Text = DecimalExtension.GetPollutantValue(pollutantValue, DecimalNum).ToString();
                                }
                                else
                                {
                                    factorCell.Text = (DecimalExtension.GetPollutantValue(pollutantValue, DecimalNum) * 1000).ToString("G0");
                                }
                            }
                        }
                    }
                }
            }
        }

        //日数据获取因子
        private string[] GetUniqueNameByPollutantNameDay(string pollutantName)
        {
            string[] returnValues = new string[0];
            switch (pollutantName)
            {
                case "a21026":
                    returnValues = new string[] { "SO2" };
                    break;
                case "a21004":
                    returnValues = new string[] { "NO2" };
                    break;
                case "a34002":
                    returnValues = new string[] { "PM10" };
                    break;
                case "a21005":
                    returnValues = new string[] { "CO" };
                    break;
                case "a05024":
                    returnValues = new string[] { "Max8HourO3" };
                    break;
                case "a34004":
                    returnValues = new string[] { "PM25" };
                    break;
                default: break;
            }
            return returnValues;
        }
        //小时数据获取因子
        private string[] GetUniqueNameByPollutantNameHour(string pollutantName)
        {
            string[] returnValues = new string[0];
            switch (pollutantName)
            {
                case "a21026":
                    returnValues = new string[] { "SO2" };
                    break;
                case "a21004":
                    returnValues = new string[] { "NO2" };
                    break;
                case "a34002":
                    returnValues = new string[] { "PM10" };
                    break;
                case "a21005":
                    returnValues = new string[] { "CO" };
                    break;
                case "a05024":
                    returnValues = new string[] { "O3" };
                    break;
                case "a34004":
                    returnValues = new string[] { "PM25" };
                    break;
                default: break;
            }
            return returnValues;
        }

        /// <summary>
        /// 查询按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            if (radlDataType.SelectedValue == "Day")
            {
                gridDayAQI.Visible = true;
                gridRealTimeAQI.Visible = false;
                gridDayAQI.CurrentPageIndex = 0;
                gridDayAQI.Rebind();
            }
            if (radlDataType.SelectedValue == "Hour")
            {
                if (dtms == Convert.ToDateTime(hourEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:00")))
                {
                    hourEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00"));
                }
                gridDayAQI.Visible = false;
                gridRealTimeAQI.Visible = true;
                gridRealTimeAQI.CurrentPageIndex = 0;
                gridRealTimeAQI.Rebind();
            }
            if (tabStrip.SelectedTab.Text == "图表")
            {
                BindChart();
            }
            else
            {
                FirstLoadChart.Value = "1";
            }
        }

        /// <summary>
        /// 绑定图表
        /// </summary>
        private void BindChart()
        {
            RegisterScript("CreatChart();");
        }

        /// <summary>
        /// 日数据ToolBar事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridRTB_ButtonClickDay(object sender, RadToolBarEventArgs e)
        {
            Telerik.Web.UI.RadToolBarButton button = (Telerik.Web.UI.RadToolBarButton)e.Item;
            if (button.CommandName == "ExportToExcel")
            {
                if (dtpBegin.SelectedDate == null || dtpEnd.SelectedDate == null)
                {
                    return;
                }
                else if (dtpBegin.SelectedDate > dtpEnd.SelectedDate)
                {
                    return;
                }
                //string orderByAQI = PageHelper.GetQueryString("orderBy");
                string rcbType = "";
                foreach (RadComboBoxItem item in rcbCityProper.CheckedItems)
                {
                    rcbType += (item.Text.ToString() + ",");
                }
                string[] qulityType = rcbType.Trim(',').Split(',');
                DateTime dtmBegion = dtpBegin.SelectedDate.Value;
                DateTime dtmEnd = dtpEnd.SelectedDate.Value;
                points = pointCbxRsm.GetPoints();
                int pageSize = 9999;  //每页显示数据个数  
                int pageNo = 0;   //当前页的序号
                int recordTotal = 0;  //数据总行数
                var dataView = new DataView();
                string orderBy = "";

                if (rbtnlType.SelectedValue == "Port")
                {
                    if (points != null && points.Count > 0)
                    {
                        orderBy = "PointId,DateTime Desc";
                        if (TimeSort.SelectedValue == "时间升序")
                            orderBy = "PointId,DateTime Asc";
                        if (ddlDataFrom.SelectedValue == "AuditData")
                        {
                            string[] typeArr = { "优", "良", "轻度污染", "中度污染", "重度污染", "严重污染", "无效天" };
                            dataView = m_DayAQIService.GetAirQualityDayReportNew(points.Select(t => t.PointID).ToArray(), dtmBegion, dtmEnd, typeArr, pageSize, pageNo, out recordTotal, orderBy);
                        }
                        if (ddlDataFrom.SelectedValue == "OriData")
                        {
                            string[] typeArr = { "优", "良", "轻度污染", "中度污染", "重度污染", "严重污染", "无效天" };
                            dataView = m_DayAQIService.GetAirQualityOriDayReportNew(points.Select(t => t.PointID).ToArray(), dtmBegion, dtmEnd, typeArr, pageSize, pageNo, out recordTotal, orderBy);
                        }
                    }
                    else
                    {
                        dataView = null;
                    }
                }
                else if (rbtnlType.SelectedValue == "CityProper")
                {
                    if (points != null && points.Count > 0)
                    {
                        //给datatable增加列
                        if (ddlDataFrom.SelectedValue == "AuditData")
                        {
                            if (CalOrCheck(points))
                            {
                                string[] typeArr = { "优", "良", "轻度污染", "中度污染", "重度污染", "严重污染", "无效天" };
                                orderBy = "DateTime Desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderBy = "DateTime Asc";
                                dataView = m_DayAQIService.GetAirQualityRegionDayReportNew(dtmBegion, dtmEnd, typeArr, pageSize, pageNo, "Audit", out recordTotal, orderBy);
                            }
                            else
                            {
                                dataView = m_AQICalculateService.GetRegionAQI(points.Select(t => t.PointID).ToArray(), dtmBegion, dtmEnd, 24, "2").AsDataView();
                                dataView.Sort = "DateTime Desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                {
                                    dataView.Sort = "DateTime Asc";
                                }
                            }
                        }
                        if (ddlDataFrom.SelectedValue == "OriData")
                        {
                            if (CalOrCheck(points))
                            {
                                string[] typeArr = { "优", "良", "轻度污染", "中度污染", "重度污染", "严重污染", "无效天" };
                                orderBy = "DateTime Desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderBy = "DateTime Asc";
                                dataView = m_DayAQIService.GetAirQualityRegionDayReportNew(dtmBegion, dtmEnd, typeArr, pageSize, pageNo, "Ori", out recordTotal, orderBy);
                            }
                            else
                            {
                                dataView = m_AQICalculateService.GetRegionAQI(points.Select(t => t.PointID).ToArray(), dtmBegion, dtmEnd, 24, "1").AsDataView();
                                dataView.Sort = "DateTime Desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                {
                                    dataView.Sort = "DateTime Asc";
                                }
                            }
                        }
                    }
                    else
                    {
                        dataView = null;
                    }
                }
                DataTableToExcelDay(dataView, "空气质量日报", "空气质量日报");
            }
        }

        /// <summary>
        /// 小时数据ToolBar事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridRTB_ButtonClickHour(object sender, RadToolBarEventArgs e)
        {
            Telerik.Web.UI.RadToolBarButton button = (Telerik.Web.UI.RadToolBarButton)e.Item;
            if (button.CommandName == "ExportToExcel")
            {
                if (hourBegin.SelectedDate == null || hourEnd.SelectedDate == null)
                {
                    return;
                }
                else if (hourBegin.SelectedDate > hourEnd.SelectedDate)
                {
                    return;
                }
                //string orderByAQI = PageHelper.GetQueryString("orderBy");
                string rcbType = "";
                foreach (RadComboBoxItem item in rcbCityProper.CheckedItems)
                {
                    rcbType += (item.Text.ToString() + ",");
                }
                string[] qulityType = rcbType.Trim(',').Split(',');
                DateTime dtmhourBegin = hourBegin.SelectedDate.Value;
                DateTime dtmhourEnd = hourEnd.SelectedDate.Value;
                points = pointCbxRsm.GetPoints();
                int pageSizeHour = 9999;  //小时每页显示数据个数  
                int pageNoHour = 0;   //小时当前页的序号
                int recordTotal = 0;  //数据总行数
                var dataViewHour = new DataView();
                string orderBy = "";

                if (rbtnlType.SelectedValue == "Port")
                {
                    if (points != null && points.Count > 0)
                    {
                        //小时数据区分原始审核
                        if (ddlDataFrom.SelectedIndex == 0)
                        {
                            orderBy = "PointId,DateTime Desc";
                            if (TimeSort.SelectedValue == "时间升序")
                                orderBy = "PointId,DateTime Asc";
                            dataViewHour = m_HourAQIService.GetAirQualityOriRTReport(points.Select(t => t.PointID).ToArray(), dtmhourBegin, dtmhourEnd.AddHours(1).AddSeconds(-1), pageSizeHour, pageNoHour, out recordTotal, orderBy);
                        }
                        if (ddlDataFrom.SelectedIndex == 1)
                        {
                            orderBy = "PointId,DateTime Desc";
                            if (TimeSort.SelectedValue == "时间升序")
                                orderBy = "PointId,DateTime Asc";
                            dataViewHour = m_HourAQIService.GetAirQualityRTReport(points.Select(t => t.PointID).ToArray(), dtmhourBegin, dtmhourEnd.AddHours(1).AddSeconds(-1), pageSizeHour, pageNoHour, out recordTotal, orderBy);
                        }
                    }
                    else
                    {
                        dataViewHour = null;
                    }
                }
                else if (rbtnlType.SelectedValue == "CityProper")
                {
                    if (points != null && points.Count > 0)
                    {
                        if (ddlDataFrom.SelectedValue == "AuditData")
                        {
                            if (CalOrCheck(points))
                            {
                                orderBy = "DateTime Desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderBy = "DateTime Asc";
                                dataViewHour = m_HourAQIService.GetAirQualityRegionRTReport(dtmhourBegin, dtmhourEnd.AddHours(1).AddSeconds(-1), pageSizeHour, pageNoHour, "Audit", out recordTotal, orderBy).AsDataView();
                            }
                            else
                            {
                                dataViewHour = m_AQICalculateService.GetRegionAQI(points.Select(t => t.PointID).ToArray(), dtmhourBegin, dtmhourEnd, 1, "2").AsDataView();
                                dataViewHour.Sort = "DateTime Desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                {
                                    dataViewHour.Sort = "DateTime Asc";
                                }
                            }
                        }
                        if (ddlDataFrom.SelectedValue == "OriData")
                        {
                            if (CalOrCheck(points))
                            {
                                orderBy = "DateTime Desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                    orderBy = "DateTime Asc";
                                dataViewHour = m_HourAQIService.GetAirQualityRegionRTReport(dtmhourBegin, dtmhourEnd.AddHours(1).AddSeconds(-1), pageSizeHour, pageNoHour, "Ori", out recordTotal, orderBy).AsDataView();
                            }
                            else
                            {
                                dataViewHour = m_AQICalculateService.GetRegionAQI(points.Select(t => t.PointID).ToArray(), dtmhourBegin, dtmhourEnd, 1, "1").AsDataView();
                                dataViewHour.Sort = "DateTime Desc";
                                if (TimeSort.SelectedValue == "时间升序")
                                {
                                    dataViewHour.Sort = "DateTime Asc";
                                }
                            }
                        }
                    }
                    else
                    {
                        dataViewHour = null;
                    }
                }
                DataTableToExcelHour(dataViewHour, "空气质量实时报", "空气质量实时报");
            }
        }

        /// <summary>
        /// 导出空气质量实时报
        /// </summary>
        /// <param name="dv">原始数据表</param>
        /// <returns></returns>
        private void DataTableToExcelHour(DataView dv, string fileName, string sheetName)
        {
            DataTable dtNew = dv.ToTable();
            points = pointCbxRsm.GetPoints();
            Workbook workbook = new Workbook();
            Worksheet sheet = workbook.Worksheets[0];
            Cells cells = sheet.Cells;
            Aspose.Cells.Style cellStyle = workbook.Styles[workbook.Styles.Add()];
            workbook.FileName = fileName;
            sheet.Name = sheetName;
            cellStyle.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center; // 设置居中 
            cellStyle.Font.Name = "宋体"; //文字字体
            cellStyle.Font.Size = 12;//文字大小
            cellStyle.IsLocked = false; //单元格解锁
            cellStyle.IsTextWrapped = true; //单元格内容自动换行

            #region 数据修改
            AirPollutantService m_AirPollutantService = new AirPollutantService();
            factors = GetPollutantListByCalAQI();//获取参与评价AQI的常规6因子
            if (!dtNew.Columns.Contains("PointName"))
            {
                dtNew.Columns.Add("PointName");
            }
            for (int i = 0; i < dtNew.Rows.Count; i++)
            {
                DataRow drNew = dtNew.Rows[i];
                if (rbtnlType.SelectedValue == "Port")
                {
                    drNew["PointName"] = (points.Count(t => t.PointID == drNew["PointId"].ToString()) > 0)
                     ? points.Where(t => t.PointID == drNew["PointId"].ToString()).Select(t => t.PointName).FirstOrDefault()
                     : drNew["PointId"].ToString();
                }
                for (int j = 0; j < factors.Count; j++)
                {
                    string[] uniqueNames = GetUniqueNameByPollutantNameHour(factors[j].PollutantCode);
                    foreach (string uniqueName in uniqueNames)
                    {
                        if (dtNew.Columns.Contains(uniqueName) && !string.IsNullOrWhiteSpace(drNew[uniqueName].ToString()))
                        {
                            //获取因子小数位,channelCode 因子代码 例：CO的code是a21005  （ channelCode=a21005）
                            int DecimalNum = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo(factors[j].PollutantCode).PollutantDecimalNum);
                            decimal pollutantValue = decimal.TryParse(drNew[uniqueName].ToString(), out pollutantValue) ? pollutantValue : 0;

                            //保留小数位数,value 需要进行小数位处理的数据 类型Decimal
                            if (uniqueName == "CO")
                            {
                                drNew[uniqueName] = DecimalExtension.GetPollutantValue(pollutantValue, DecimalNum).ToString();
                            }
                            else
                            {
                                drNew[uniqueName] = (DecimalExtension.GetPollutantValue(pollutantValue, DecimalNum) * 1000).ToString("G0");
                            }
                        }
                        else if (dtNew.Columns.Contains(uniqueName) && string.IsNullOrWhiteSpace(drNew[uniqueName].ToString()))
                        {
                            drNew[uniqueName] = "--";
                        }
                    }
                }
            }
            #endregion

            #region 表头
            //第一行
            cells[0, 0].PutValue("监测点位名称");
            cells.Merge(0, 0, 3, 1);
            cells[0, 1].PutValue("日期");
            cells.Merge(0, 1, 3, 1);
            cells[0, 2].PutValue("污染物浓度及空气质量分指数（IAQI）");
            cells.Merge(0, 2, 1, 12);
            cells[0, 14].PutValue("空气质量指数(AQI)");
            cells.Merge(0, 14, 3, 1);
            cells[0, 15].PutValue("首要污染物");
            cells.Merge(0, 15, 3, 1);
            cells[0, 16].PutValue("空气质量指数级别");
            cells.Merge(0, 16, 3, 1);
            cells[0, 17].PutValue("空气质量指数类别");
            cells.Merge(0, 17, 2, 2);

            //第二行
            cells[1, 2].PutValue("PM2.5 1小时平均");
            cells.Merge(1, 2, 1, 2);
            cells[1, 4].PutValue("PM10 1小时平均");
            cells.Merge(1, 4, 1, 2);
            cells[1, 6].PutValue("二氧化氮(NO2)1小时平均");
            cells.Merge(1, 6, 1, 2);
            cells[1, 8].PutValue("二氧化硫(SO2)1小时平均");
            cells.Merge(1, 8, 1, 2);
            cells[1, 10].PutValue("一氧化碳(CO)1小时平均");
            cells.Merge(1, 10, 1, 2);
            cells[1, 12].PutValue("臭氧(O3)1小时平均");
            cells.Merge(1, 12, 1, 2);


            //第三行
            cells[2, 2].PutValue("浓度/(μg/m3)");
            cells[2, 3].PutValue("分指数");
            cells[2, 4].PutValue("浓度/(μg/m3)");
            cells[2, 5].PutValue("分指数");
            cells[2, 6].PutValue("浓度/(μg/m3)");
            cells[2, 7].PutValue("分指数");
            cells[2, 8].PutValue("浓度/(μg/m3)");
            cells[2, 9].PutValue("分指数");
            cells[2, 10].PutValue("浓度/(mg/m3)");
            cells[2, 11].PutValue("分指数");
            cells[2, 12].PutValue("浓度/(μg/m3)");
            cells[2, 13].PutValue("分指数");
            cells[2, 17].PutValue("类别");
            cells[2, 18].PutValue("颜色");
            cells.SetRowHeight(0, 20);//设置行高
            cells.SetRowHeight(1, 30);//设置行高
            cells.SetColumnWidth(0, 20);//设置列宽
            cells.SetColumnWidth(1, 13);//设置列宽
            cells.SetColumnWidth(17, 10);//设置列宽
            cells.SetColumnWidth(17, 10);//设置列宽
            cells.SetColumnWidth(18, 10);//设置列宽
            for (int i = 2; i <= 15; i++)
            {
                cells.SetColumnWidth(i, 10);//设置列宽
            }
            #endregion

            for (int i = 0; i < dtNew.Rows.Count; i++)
            {
                DataRow drNew = dtNew.Rows[i];
                int rowIndex = i + 3;
                if (rbtnlType.SelectedValue == "Port")
                {
                    cells[rowIndex, 0].PutValue(drNew["PointName"].ToString());
                }
                if (rbtnlType.SelectedValue == "CityProper")
                {
                    cells[rowIndex, 0].PutValue(drNew["PointId"].ToString());
                }
                if (dtNew.Columns.Contains("DateTime"))
                {
                    DateTime dateTime = DateTime.Parse(drNew["DateTime"].ToString());
                    cells[rowIndex, 1].PutValue(string.Format("{0:yyyy-MM-dd HH:00}", dateTime));
                }
                else
                {
                    DateTime reportDateTime = DateTime.Parse(drNew["ReportDateTime"].ToString());
                    cells[rowIndex, 1].PutValue(string.Format("{0:yyyy-MM-dd HH:00}", reportDateTime));
                }
                int a = 0;
                decimal b = 0.0M;
                if (int.TryParse(drNew["PM25"].ToString(), out a))
                {
                    cells[rowIndex, 2].PutValue(a);
                }
                if (int.TryParse(drNew["PM25_IAQI"].ToString(), out a))
                {
                    cells[rowIndex, 3].PutValue(a);
                }
                if (int.TryParse(drNew["PM10"].ToString(), out a))
                {
                    cells[rowIndex, 4].PutValue(a);
                }
                if (int.TryParse(drNew["PM10_IAQI"].ToString(), out a))
                {
                    cells[rowIndex, 5].PutValue(a);
                }
                if (int.TryParse(drNew["NO2"].ToString(), out a))
                {
                    cells[rowIndex, 6].PutValue(a);
                }
                if (int.TryParse(drNew["NO2_IAQI"].ToString(), out a))
                {
                    cells[rowIndex, 7].PutValue(a);
                }
                if (int.TryParse(drNew["SO2"].ToString(), out a))
                {
                    cells[rowIndex, 8].PutValue(a);
                }
                if (int.TryParse(drNew["SO2_IAQI"].ToString(), out a))
                {
                    cells[rowIndex, 9].PutValue(a);
                }
                if (decimal.TryParse(drNew["CO"].ToString(), out b))
                {
                    cells[rowIndex, 10].PutValue(b);
                }
                if (int.TryParse(drNew["CO_IAQI"].ToString(), out a))
                {
                    cells[rowIndex, 11].PutValue(a);
                }
                if (int.TryParse(drNew["O3"].ToString(), out a))
                {
                    cells[rowIndex, 12].PutValue(a);
                }
                if (int.TryParse(drNew["O3_IAQI"].ToString(), out a))
                {
                    cells[rowIndex, 13].PutValue(a);
                }
                if (int.TryParse(drNew["AQIValue"].ToString(), out a))
                {
                    cells[rowIndex, 14].PutValue(a);
                }
                cells[rowIndex, 15].PutValue(drNew["PrimaryPollutant"].ToString() != "" ? drNew["PrimaryPollutant"].ToString() : "--");
                cells[rowIndex, 16].PutValue(drNew["Grade"].ToString() != "" ? drNew["Grade"].ToString() : "--");
                cells[rowIndex, 17].PutValue(drNew["Class"].ToString() != "" ? drNew["Class"].ToString() : "--");
                cells[rowIndex, 18].PutValue("");
                if (drNew["RGBValue"].ToString() != "")
                {
                    Aspose.Cells.Style styleTemp = cells[rowIndex, 18].GetStyle();
                    styleTemp.ForegroundColor = System.Drawing.ColorTranslator.FromHtml(drNew["RGBValue"].ToString());//设置背景色
                    styleTemp.Pattern = BackgroundType.Solid;//设置背景样式
                    cells[rowIndex, 18].SetStyle(styleTemp);
                }
            }
            foreach (Cell cell in cells)
            {
                if (!cell.IsStyleSet&&(cell.Column!=1||cell.Row==0))
                {
                    cell.SetStyle(cellStyle);
                }
            }

            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "utf-8";
            //Response.AppendHeader("Content-Disposition", string.Format("attachment;filename={0}.xls", HttpUtility.UrlEncode(fileName) + DateTime.Now.ToString("yyyyMMddHHmmss")));
            Response.AppendHeader("Content-Disposition", string.Format("attachment;filename={0}.xls", HttpUtility.UrlEncode(fileName)));
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.ContentType = "application/ms-excel";
            Response.BinaryWrite(workbook.SaveToStream().ToArray());
            Response.End();
        }

        /// <summary>
        /// 导出空气质量日报
        /// </summary>
        /// <param name="dv">原始数据表</param>
        /// <returns></returns>
        private void DataTableToExcelDay(DataView dv, string fileName, string sheetName)
        {
            DataTable dtNew = dv.ToTable();
            points = pointCbxRsm.GetPoints();
            Workbook workbook = new Workbook();
            Worksheet sheet = workbook.Worksheets[0];
            Cells cells = sheet.Cells;
            Aspose.Cells.Style cellStyle = workbook.Styles[workbook.Styles.Add()];
            workbook.FileName = fileName;
            sheet.Name = sheetName;
            cellStyle.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center; // 设置居中 
            cellStyle.Font.Name = "宋体"; //文字字体
            cellStyle.Font.Size = 12;//文字大小
            cellStyle.IsLocked = false; //单元格解锁
            cellStyle.IsTextWrapped = true; //单元格内容自动换行

            #region 数据修改
            AirPollutantService m_AirPollutantService = new AirPollutantService();
            factors = GetPollutantListByCalAQI();//获取参与评价AQI的常规6因子
            if (!dtNew.Columns.Contains("PointName"))
            {
                dtNew.Columns.Add("PointName");
            }
            for (int i = 0; i < dtNew.Rows.Count; i++)
            {
                DataRow drNew = dtNew.Rows[i];
                if (rbtnlType.SelectedValue == "Port")
                {
                    drNew["PointName"] = (points.Count(t => t.PointID == drNew["PointId"].ToString()) > 0)
                     ? points.Where(t => t.PointID == drNew["PointId"].ToString()).Select(t => t.PointName).FirstOrDefault()
                     : drNew["PointId"].ToString();
                }
                for (int j = 0; j < factors.Count; j++)
                {
                    string[] uniqueNames = GetUniqueNameByPollutantNameDay(factors[j].PollutantCode);
                    foreach (string uniqueName in uniqueNames)
                    {
                        if (dtNew.Columns.Contains(uniqueName) && !string.IsNullOrWhiteSpace(drNew[uniqueName].ToString()))
                        {
                            //获取因子小数位,channelCode 因子代码 例：CO的code是a21005  （ channelCode=a21005）
                            int DecimalNum = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo(factors[j].PollutantCode).PollutantDecimalNum);
                            decimal pollutantValue = decimal.TryParse(drNew[uniqueName].ToString(), out pollutantValue) ? pollutantValue : 0;

                            //保留小数位数,value 需要进行小数位处理的数据 类型Decimal
                            if (uniqueName == "CO")
                            {
                                drNew[uniqueName] = DecimalExtension.GetPollutantValue(pollutantValue, DecimalNum).ToString();
                            }
                            else
                            {
                                drNew[uniqueName] = (DecimalExtension.GetPollutantValue(pollutantValue, DecimalNum) * 1000).ToString("G0");
                            }
                        }
                    }
                }
            }
            #endregion

            #region 表头
            //第一行
            cells[0, 0].PutValue("监测点位名称");
            cells.Merge(0, 0, 3, 1);
            cells[0, 1].PutValue("日期");
            cells.Merge(0, 1, 3, 1);
            cells[0, 2].PutValue("污染物浓度及空气质量分指数（IAQI）");
            cells.Merge(0, 2, 1, 12);
            cells[0, 14].PutValue("空气质量指数(AQI)");
            cells.Merge(0, 14, 3, 1);
            cells[0, 15].PutValue("首要污染物");
            cells.Merge(0, 15, 3, 1);
            cells[0, 16].PutValue("空气质量指数级别");
            cells.Merge(0, 16, 3, 1);
            cells[0, 17].PutValue("空气质量指数类别");
            cells.Merge(0, 17, 2, 2);

            //第二行
            cells[1, 2].PutValue("PM2.5 24小时平均值");
            cells.Merge(1, 2, 1, 2);
            cells[1, 4].PutValue("PM10 24小时平均值");
            cells.Merge(1, 4, 1, 2);
            cells[1, 6].PutValue("二氧化氮(NO2)24小时平均值");
            cells.Merge(1, 6, 1, 2);
            cells[1, 8].PutValue("二氧化硫(SO2)24小时平均值");
            cells.Merge(1, 8, 1, 2);
            cells[1, 10].PutValue("一氧化碳(CO)24小时平均值");
            cells.Merge(1, 10, 1, 2);
            cells[1, 12].PutValue("臭氧(O3)最大8小时滑动平均值");
            cells.Merge(1, 12, 1, 2);
            //cells[1, 14].PutValue("臭氧(O3)最近8小时滑动平均值");
            //cells.Merge(1, 14, 1, 2);

            //第三行
            cells[2, 2].PutValue("浓度/(μg/m3)");
            cells[2, 3].PutValue("分指数");
            cells[2, 4].PutValue("浓度/(μg/m3)");
            cells[2, 5].PutValue("分指数");
            cells[2, 6].PutValue("浓度/(μg/m3)");
            cells[2, 7].PutValue("分指数");
            cells[2, 8].PutValue("浓度/(μg/m3)");
            cells[2, 9].PutValue("分指数");
            cells[2, 10].PutValue("浓度/(mg/m3)");
            cells[2, 11].PutValue("分指数");
            cells[2, 12].PutValue("浓度/(μg/m3)");
            cells[2, 13].PutValue("分指数");
            //cells[2, 14].PutValue("浓度/(μg/m3)");
            //cells[2, 15].PutValue("分指数");
            cells[2, 17].PutValue("类别");
            cells[2, 18].PutValue("颜色");
            cells.SetRowHeight(0, 20);//设置行高
            cells.SetRowHeight(1, 30);//设置行高
            cells.SetColumnWidth(0, 20);//设置列宽
            cells.SetColumnWidth(1, 20);//设置列宽
            cells.SetColumnWidth(15, 10);//设置列宽
            cells.SetColumnWidth(17, 10);//设置列宽
            cells.SetColumnWidth(18, 10);//设置列宽

            for (int i = 2; i <= 13; i++)
            {
                cells.SetColumnWidth(i, 10);//设置列宽
            }
            #endregion

            for (int i = 0; i < dtNew.Rows.Count; i++)
            {
                DataRow drNew = dtNew.Rows[i];
                int rowIndex = i + 3;
                if (rbtnlType.SelectedValue == "Port")
                {
                    cells[rowIndex, 0].PutValue(drNew["PointName"].ToString());
                    cells[rowIndex, 1].PutValue(string.Format("{0:yyyy-MM-dd}", drNew["DateTime"]));
                }
                if (rbtnlType.SelectedValue == "CityProper")
                {
                    cells[rowIndex, 0].PutValue(drNew["PointId"].ToString());
                    cells[rowIndex, 1].PutValue(string.Format("{0:yyyy-MM-dd}", drNew["DateTime"]));
                }
                int a = 0;
                decimal b = 0.0M;
                if (int.TryParse(drNew["PM25"].ToString(), out a))
                {
                    cells[rowIndex, 2].PutValue(a);
                }
                if (int.TryParse(drNew["PM25_IAQI"].ToString(), out a))
                {
                    cells[rowIndex, 3].PutValue(a);
                }
                if (int.TryParse(drNew["PM10"].ToString(), out a))
                {
                    cells[rowIndex, 4].PutValue(a);
                }
                if (int.TryParse(drNew["PM10_IAQI"].ToString(), out a))
                {
                    cells[rowIndex, 5].PutValue(a);
                }
                if (int.TryParse(drNew["NO2"].ToString(), out a))
                {
                    cells[rowIndex, 6].PutValue(a);
                }
                if (int.TryParse(drNew["NO2_IAQI"].ToString(), out a))
                {
                    cells[rowIndex, 7].PutValue(a);
                }
                if (int.TryParse(drNew["SO2"].ToString(), out a))
                {
                    cells[rowIndex, 8].PutValue(a);
                }
                if (int.TryParse(drNew["SO2_IAQI"].ToString(), out a))
                {
                    cells[rowIndex, 9].PutValue(a);
                }
                if (decimal.TryParse(drNew["CO"].ToString(), out b))
                {
                    cells[rowIndex, 10].PutValue(b);
                }
                if (int.TryParse(drNew["CO_IAQI"].ToString(), out a))
                {
                    cells[rowIndex, 11].PutValue(a);
                }
                if (int.TryParse(drNew["Max8HourO3"].ToString(), out a))
                {
                    cells[rowIndex, 12].PutValue(a);
                }
                if (int.TryParse(drNew["Max8HourO3_IAQI"].ToString(), out a))
                {
                    cells[rowIndex, 13].PutValue(a);
                }
                if (int.TryParse(drNew["AQIValue"].ToString(), out a))
                {
                    cells[rowIndex, 14].PutValue(a);
                }
                cells[rowIndex, 15].PutValue(drNew["PrimaryPollutant"].ToString() != "" ? drNew["PrimaryPollutant"].ToString() : "--");
                cells[rowIndex, 16].PutValue(drNew["Grade"].ToString() != "" ? drNew["Grade"].ToString() : "--");
                cells[rowIndex, 17].PutValue(drNew["Class"].ToString() != "" ? drNew["Class"].ToString() : "--");
                cells[rowIndex, 18].PutValue("");
                if (drNew["RGBValue"].ToString() != "")
                {
                    Aspose.Cells.Style styleTemp = cells[rowIndex, 18].GetStyle();
                    styleTemp.ForegroundColor = System.Drawing.ColorTranslator.FromHtml(drNew["RGBValue"].ToString());//设置背景色
                    styleTemp.Pattern = BackgroundType.Solid;//设置背景样式
                    cells[rowIndex, 18].SetStyle(styleTemp);
                }
            }
            foreach (Cell cell in cells)
            {
                if (!cell.IsStyleSet && (cell.Column != 1 || cell.Row == 0))
                {
                    cell.SetStyle(cellStyle);
                }
            }

            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "utf-8";
            //Response.AppendHeader("Content-Disposition", string.Format("attachment;filename={0}.xls", HttpUtility.UrlEncode(fileName) + DateTime.Now.ToString("yyyyMMddHHmmss")));
            Response.AppendHeader("Content-Disposition", string.Format("attachment;filename={0}.xls", HttpUtility.UrlEncode(fileName)));
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.ContentType = "application/ms-excel";
            Response.BinaryWrite(workbook.SaveToStream().ToArray());
            Response.End();
        }

        #endregion

        /// <summary>
        /// 判断是否满足条件，决定是查表还是动态计算
        /// </summary>
        /// <returns></returns>
        private bool CalOrCheck(IList<IPoint> points)
        {
            if (points.Count != 4)
            {
                return false;
            }
            else
            {
                List<string> list = new List<string>();
                list.Add("紫琅学院");
                list.Add("星湖花园");
                list.Add("虹桥");
                list.Add("城中");
                foreach (IPoint ip in points)
                {
                    if (!list.Contains(ip.PointName))
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// 根据测点Id数组获取因子列
        /// </summary>
        /// <param name="pointIds">测点Id数组</param>
        /// <returns></returns>
        private IList<PollutantCodeEntity> GetPollutantCodesByPointIds(string[] pointIds)
        {
            MonitoringPointAirService monitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            IQueryable<MonitoringPointEntity> monitorPointQueryable = monitoringPointAir.RetrieveListByPointIds(pointIds);//根据站点ID数组获取站点
            IList<PollutantCodeEntity> pollutantList = new List<PollutantCodeEntity>();
            InstrumentChannelService instrumentChannelService = Singleton<InstrumentChannelService>.GetInstance();//提供仪器通道信息服务
            foreach (MonitoringPointEntity monitoringPointEntity in monitorPointQueryable)
            {
                IQueryable<PollutantCodeEntity> pollutantCodeQueryable =
                    instrumentChannelService.RetrieveChannelListByPointUid(monitoringPointEntity.MonitoringPointUid);//根据站点Uid获取所有监测通道
                pollutantList = pollutantList.Union(pollutantCodeQueryable).ToList();
            }
            return pollutantList;
        }

        /// <summary>
        /// 获取参与评价AQI的常规6因子
        /// </summary>
        /// <returns></returns>
        private IList<PollutantCodeEntity> GetPollutantListByCalAQI()
        {
            AirPollutantService airPollutantService = new AirPollutantService();
            return airPollutantService.RetrieveListByCalAQI().ToList();
        }

        protected void rcbCityProper_ItemCreated(object sender, RadComboBoxItemEventArgs e)
        {
            e.Item.Checked = true;
        }

        /// <summary>
        /// 数据类型时间框选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void radlDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            gridDayAQI.CurrentPageIndex = 0;
            gridRealTimeAQI.CurrentPageIndex = 0;
            if (radlDataType.SelectedValue == "Hour")
            {
                divType.Visible = false;
                divTypeContent.Visible = false;
                dtpBegin.Visible = false;
                dtpEnd.Visible = false;
                hourBegin.Visible = true;
                hourEnd.Visible = true;
                hdFactors.Value = "H";
            }
            else if (radlDataType.SelectedValue == "Day")
            {
                divType.Visible = true;
                divTypeContent.Visible = true;
                dtpBegin.Visible = true;
                dtpEnd.Visible = true;
                hourBegin.Visible = false;
                hourEnd.Visible = false;
                hdFactors.Value = "D";
            }
        }

        /// <summary>
        /// 根据所选站点ID获取相应区域信息
        /// </summary>
        /// <param name="pointIds"></param>
        /// <returns></returns>
        public DataView GetRegionByPointId(string[] pointIds)
        {
            return pointAirService.GetRegionByPointId(pointIds);
        }

        /// <summary>
        /// 绘图类型变化(折线、柱形)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ChartType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string PicType = ChartType.SelectedValue;
            hdChartType.Value = PicType;
            RegisterScript("PointFactor('" + PicType + "');");
        }

        /// <summary>
        /// 图表类型变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ChartContent_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            string Pic = ChartContent.SelectedValue;
            hdChartContent.Value = Pic;
            RegisterScript("PointFactor('" + Pic + "');");
        }

        protected void timer_Tick(object sender, EventArgs e)
        {
            gridRealTimeAQI.CurrentPageIndex = 0;
            gridRealTimeAQI.Rebind();
            timer.Enabled = false;
        }
    }
}