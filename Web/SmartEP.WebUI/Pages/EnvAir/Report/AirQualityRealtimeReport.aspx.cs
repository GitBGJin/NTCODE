using Aspose.Cells;
using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.BusinessRule;
using SmartEP.Service.BaseData.Channel;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.Core.Enums;
using SmartEP.Service.DataAnalyze.Air.AQIReport;
using SmartEP.Service.Frame;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Utilities.Office;
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

namespace SmartEP.WebUI.Pages.EnvAir.Report
{
    /// <summary>
    /// 名称：AirQualityRealtimeReport.cs
    /// 创建人：王秉晟
    /// 创建日期：2015-08-14
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：空气质量实时报
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class AirQualityRealtimeReport : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        private HourAQIService m_HourAQIService;

        /// <summary>
        /// 选择站点
        /// </summary>
        private IList<IPoint> points = null;

        /// <summary>
        /// 选择因子
        /// </summary>
        private IList<PollutantCodeEntity> factors = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            m_HourAQIService = new HourAQIService();
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
            DictionaryService dicService = new DictionaryService();
            MonitoringPointAirService pointAirService = new MonitoringPointAirService();
            dtpBegin.SelectedDate = DateTime.Parse(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:00"));
            dtpEnd.SelectedDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:00"));

            string UserGuid = Session["UserGuid"].ToString();
            string CfgUserGuid = ConfigurationManager.AppSettings["ManagerRole"].ToString();
            string CfgAreaSelect = ConfigurationManager.AppSettings["AreaSelect"].ToString();
            if (UserGuid.Equals(CfgUserGuid))
            {
                foreach (RadComboBoxItem item in comboCity.Items)
                {
                    if (item.Value.ToString().Equals(CfgAreaSelect))
                    {
                        item.Checked = true;
                        item.Visible = true;
                    }
                    else
                    {
                        item.Checked = false;
                        item.Visible = false;
                    }
                }
            }
            //foreach (RadComboBoxItem item in comboCity.Items)
            //{
            //    if (item.Index > 4)
            //        item.Checked = true;
            //}

            //城市均值（创模点）
            //comboCityModel.DataSource = dicService.RetrieveCityList().OrderBy(t => t.SortNumber);//pointAirService.RetrieveAirMPListByCityModel(CityType.SuZhou);
            //comboCityModel.DataTextField = "ItemText"; //"MonitoringPointName";
            //comboCityModel.DataValueField = "ItemGuid"; //"PointId";
            //comboCityModel.DataBind();
            //foreach (RadComboBoxItem item in comboCityModel.Items)
            //{
            //    item.Checked = true;
            //}
            //if (comboCityModel.Items.Count > 0)
            //{
            //    comboCityModel.Items[0].Checked = true;
            //}
        }
        #endregion

        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {
            string UserGuid = Session["UserGuid"].ToString();
            string CfgUserGuid = ConfigurationManager.AppSettings["ManagerRole"].ToString();
            string CfgAreaSelect = ConfigurationManager.AppSettings["AreaSelect"].ToString();
            if (UserGuid.Equals(CfgUserGuid))
            {
                foreach (RadComboBoxItem item in comboCity.Items)
                {
                    if (item.Value.ToString().Equals(CfgAreaSelect))
                    {
                        item.Checked = true;
                        item.Visible = true;
                    }
                    else
                    {
                        item.Checked = false;
                        item.Visible = false;
                    }
                }
            }
            if (!IsPostBack)
            {
                MonitoringPointAirService m_MonitoringPointAirService = Singleton<MonitoringPointAirService>.GetInstance();
                IQueryable<MonitoringPointEntity> monitoringPointQueryable = m_MonitoringPointAirService.RetrieveAirMPListByEnable();//获取所有启用的空气点位列表
                monitoringPointQueryable = monitoringPointQueryable.Where(p => p.ContrlUid == "6fadff52-2338-4319-9f1d-7317823770ad"     //国控点
                                                                               || p.ContrlUid == "c1158eb6-4d69-4846-a963-d16b9d2794ca"  //对照点
                                                                               || p.ContrlUid == "bdf0837a-eb59-4c4a-a05f-c774a17f3077");//路边站
                string pointNames = monitoringPointQueryable.Select(t => t.MonitoringPointName)
                                        .Aggregate((a, b) => a + ";" + b);
                pointCbxRsm.SetPointValuesFromNames(pointNames);
            }
            if (dtpBegin.SelectedDate == null || dtpEnd.SelectedDate == null)
            {
                return;
            }
            else if (dtpBegin.SelectedDate > dtpEnd.SelectedDate)
            {
                return;
            }

            DateTime dtmBegion = dtpBegin.SelectedDate.Value;
            DateTime dtmEnd = dtpEnd.SelectedDate.Value;
            points = pointCbxRsm.GetPoints();
            factors = GetPollutantListByCalAQI();//获取参与评价AQI的常规6因子
            int pageSize = gridRealTimeAQI.PageSize;  //每页显示数据个数  
            int pageNo = gridRealTimeAQI.CurrentPageIndex;   //当前页的序号
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
                    dataView = m_HourAQIService.GetAirQualityRTReport(points.Select(t => t.PointID).ToArray(), dtmBegion, dtmEnd, pageSize, pageNo, out recordTotal, orderBy);
                }
                else
                {
                    dataView = null;
                }
            }
            else if (rbtnlType.SelectedValue == "City")
            {
                string[] regionUids = { };
                switch (rbtnlType.SelectedValue)
                {
                    case "City":
                        regionUids = comboCity.CheckedItems.Select(t => t.Value).ToArray();
                        break;
                    default: break;
                        
                }
                if (regionUids != null)
                {
                    orderBy = "OrderByNum,DateTime Desc";
                    if (TimeSort.SelectedValue == "时间升序")
                        orderBy = "OrderByNum,DateTime Asc";
                    dataView = m_HourAQIService.GetRegionAirQualityRTReport(regionUids, dtmBegion, dtmEnd, pageSize, pageNo, out recordTotal, orderBy);
                }
                else
                {
                    dataView = null;
                }
            }
            //else if (rbtnlType.SelectedValue == "CityModel")
            //{
            //    string[] regionUids = comboCityModel.CheckedItems.Select(t => t.Value).ToArray();
            //    if (regionUids != null)
            //    {
            //        orderBy = "MonitoringRegionUid,DateTime Desc";
            //        dataView = m_HourAQIService.GetRegionAirQualityRTReport(regionUids, dtmBegion, dtmEnd, pageSize, pageNo, out recordTotal, orderBy);
            //    }
            //    else
            //    {
            //        dataView = null;
            //    }
            //}
            if (dataView == null)
            {
                gridRealTimeAQI.DataSource = new DataTable();
            }
            else
            {
                //dataView = GetNewViewByTurnData(dataView);
                gridRealTimeAQI.DataSource = dataView;
            }
            gridRealTimeAQI.VirtualItemCount = recordTotal;
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
                if (!string.IsNullOrWhiteSpace(dr["Recent24HoursPM10"].ToString()))
                {
                    dr["Recent24HoursPM10"] = (decimal.Parse(dr["Recent24HoursPM10"].ToString()) * 1000).ToString("G0");
                }
                if (!string.IsNullOrWhiteSpace(dr["Recent24HoursPM25"].ToString()))
                {
                    dr["Recent24HoursPM25"] = (decimal.Parse(dr["Recent24HoursPM25"].ToString()) * 1000).ToString("G0");
                }
                if (!string.IsNullOrWhiteSpace(dr["O3"].ToString()))
                {
                    dr["O3"] = (decimal.Parse(dr["O3"].ToString()) * 1000).ToString("G0");
                }
                if (!string.IsNullOrWhiteSpace(dr["Recent8HoursO3"].ToString()))
                {
                    dr["Recent8HoursO3"] = (decimal.Parse(dr["Recent8HoursO3"].ToString()) * 1000).ToString("G0");
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
        /// 数据行绑定处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grid_ItemDataBound(object sender, GridItemEventArgs e)
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
                        RadComboBox comboBox = null;
                        switch (rbtnlType.SelectedValue)
                        {
                            case "City":
                                comboBox = comboCity;
                                break;
                            default: break;
                        }
                        if (comboBox != null)
                        {
                            string regionName = comboBox.Items.Where(t => t.Value == drv["MonitoringRegionUid"].ToString())
                                                 .Select(t => t.Text).FirstOrDefault();
                            pointCell.Text = regionName;
                        }
                    }
                    //else if (rbtnlType.SelectedValue == "CityModel")
                    //{
                    //    string regionName = comboCityModel.Items.Where(t => t.Value == drv["MonitoringRegionUid"].ToString())
                    //                             .Select(t => t.Text).FirstOrDefault();
                    //    pointCell.Text = regionName;
                    //}
                }
                if (item["DateTime"] != null)
                {
                    GridTableCell dateTimeCell = (GridTableCell)item["DateTime"];
                    DateTime dateTime;
                    if (DateTime.TryParse(dateTimeCell.Text, out dateTime))
                    {
                        dateTimeCell.Text = dateTime.AddHours(1).ToString("MM-dd HH时");
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
                    string[] uniqueNames = GetUniqueNameByPollutantName(factors[i].PollutantName);
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

        private string[] GetUniqueNameByPollutantName(string pollutantName)
        {
            string[] returnValues = new string[0];
            switch (pollutantName)
            {
                case "二氧化硫":
                    returnValues = new string[] { "SO2" };
                    break;
                case "二氧化氮":
                    returnValues = new string[] { "NO2" };
                    break;
                case "PM10":
                    returnValues = new string[] { "PM10" };
                    break;
                case "一氧化碳":
                    returnValues = new string[] { "CO" };
                    break;
                case "臭氧":
                    returnValues = new string[] { "O3" };
                    break;
                case "PM2.5":
                    returnValues = new string[] { "PM25" };
                    break;
                default: break;
            }
            return returnValues;
        }

        /// <summary>
        /// 查询按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            gridRealTimeAQI.CurrentPageIndex = 0;
            gridRealTimeAQI.Rebind();
        }

        /// <summary>
        /// ToolBar事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridRTB_ButtonClick(object sender, RadToolBarEventArgs e)
        {
            Telerik.Web.UI.RadToolBarButton button = (Telerik.Web.UI.RadToolBarButton)e.Item;
            if (button.CommandName == "ExportToExcel")
            {
                if (dtpBegin.SelectedDate == null || dtpEnd.SelectedDate == null)
                {
                    //Alert("开始时间或者终止时间，不能为空！");
                    return;
                }
                else if (dtpBegin.SelectedDate > dtpEnd.SelectedDate)
                {
                    //Alert("开始时间不能大于终止时间！");
                    return;
                }

                string orderBy = "";
                string[] pointIds = pointCbxRsm.GetPointValues(CbxRsmReturnType.ID);//portIds.Select(p => p.PointID).ToArray()
                DateTime dtmBegion = dtpBegin.SelectedDate.Value;
                DateTime dtmEnd = dtpEnd.SelectedDate.Value;
                DataView dv = new DataView();
                if (rbtnlType.SelectedValue == "Port")
                {
                    orderBy = "PointId,DateTime Desc";
                    if (TimeSort.SelectedValue == "时间升序")
                        orderBy = "PointId,DateTime Asc";
                    dv = m_HourAQIService.GetPortExportData(pointIds, dtmBegion, dtmEnd, orderBy);
                }
                else if (rbtnlType.SelectedValue == "City")
                {
                    string[] regionUids = { };
                    switch (rbtnlType.SelectedValue)
                    {
                        case "City":
                            regionUids = comboCity.CheckedItems.Select(t => t.Value).ToArray();
                            break;
                        default: break;
                    }
                    orderBy = "OrderByNum,DateTime Desc";
                    if (TimeSort.SelectedValue == "时间升序")
                        orderBy = "OrderByNum,DateTime Asc";
                    dv = m_HourAQIService.GetRegionExportData(regionUids, dtmBegion, dtmEnd, orderBy);
                }
                //else if (rbtnlType.SelectedValue == "CityModel")
                //{
                //    string[] regionUids = comboCityModel.CheckedItems.Select(t => t.Value).ToArray();
                //    if (regionUids != null)
                //    {
                //        orderBy = "MonitoringRegionUid,DateTime Desc";
                //        dv = m_HourAQIService.GetRegionExportData(regionUids, dtmBegion, dtmEnd, orderBy);
                //    }
                //}
                DataTableToExcel(dv, "空气质量实时报", "空气质量实时报");
            }
        }

        /// <summary>
        /// 导出空气质量实时报
        /// </summary>
        /// <param name="dv">原始数据表</param>
        /// <returns></returns>
        private void DataTableToExcel(DataView dv, string fileName, string sheetName)
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
                else if (rbtnlType.SelectedValue == "City")
                {
                    RadComboBox comboBox = null;
                    switch (rbtnlType.SelectedValue)
                    {
                        case "City":
                            comboBox = comboCity;
                            break;
                        default: break;
                    }
                    if (comboBox != null)
                    {
                        drNew["PointName"] = (comboBox.Items.Count(t => t.Value == drNew["MonitoringRegionUid"].ToString()) > 0)
                         ? comboBox.Items.Where(t => t.Value == drNew["MonitoringRegionUid"].ToString()).Select(t => t.Text).FirstOrDefault()
                         : drNew["MonitoringRegionUid"].ToString();
                    }
                }
                //else if (rbtnlType.SelectedValue == "CityModel")
                //{
                //    drNew["PointName"] = (comboCityModel.Items.Count(t => t.Value == drNew["MonitoringRegionUid"].ToString()) > 0)
                //         ? comboCityModel.Items.Where(t => t.Value == drNew["MonitoringRegionUid"].ToString()).Select(t => t.Text).FirstOrDefault()
                //         : drNew["MonitoringRegionUid"].ToString();
                //}
                for (int j = 0; j < factors.Count; j++)
                {
                    string[] uniqueNames = GetUniqueNameByPollutantName(factors[j].PollutantName);
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
                cells[rowIndex, 0].PutValue(drNew["PointName"].ToString());
                if (dtNew.Columns.Contains("DateTime"))
                {
                    DateTime dateTime = DateTime.Parse(drNew["DateTime"].ToString()).AddHours(1);
                    cells[rowIndex, 1].PutValue(string.Format("{0:yyyy-MM-dd HH时}", dateTime));
                }
                else
                {
                    DateTime reportDateTime = DateTime.Parse(drNew["ReportDateTime"].ToString()).AddHours(1);
                    cells[rowIndex, 1].PutValue(string.Format("{0:yyyy-MM-dd HH时}", reportDateTime));
                }
                cells[rowIndex, 2].PutValue(drNew["PM25"].ToString());
                cells[rowIndex, 3].PutValue(drNew["PM25_IAQI"].ToString());
                cells[rowIndex, 4].PutValue(drNew["PM10"].ToString());
                cells[rowIndex, 5].PutValue(drNew["PM10_IAQI"].ToString());
                cells[rowIndex, 6].PutValue(drNew["NO2"].ToString());
                cells[rowIndex, 7].PutValue(drNew["NO2_IAQI"].ToString());
                cells[rowIndex, 8].PutValue(drNew["SO2"].ToString());
                cells[rowIndex, 9].PutValue(drNew["SO2_IAQI"].ToString());
                cells[rowIndex, 10].PutValue(drNew["CO"].ToString());
                cells[rowIndex, 11].PutValue(drNew["CO_IAQI"].ToString());
                cells[rowIndex, 12].PutValue(drNew["O3"].ToString());
                cells[rowIndex, 13].PutValue(drNew["O3_IAQI"].ToString());
                //cells[rowIndex, 12].PutValue(drNew["Recent8HoursO3"].ToString());
                //cells[rowIndex, 13].PutValue(drNew["Recent8HoursO3_IAQI"].ToString());
                cells[rowIndex, 14].PutValue(drNew["AQIValue"].ToString());
                cells[rowIndex, 15].PutValue(drNew["PrimaryPollutant"].ToString() != "" ? drNew["PrimaryPollutant"].ToString() : "--");
                cells[rowIndex, 16].PutValue(drNew["Grade"].ToString());
                cells[rowIndex, 17].PutValue(drNew["Class"].ToString());
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
                if (!cell.IsStyleSet)
                {
                    cell.SetStyle(cellStyle);
                }
            }

            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", string.Format("attachment;filename={0}.xls", HttpUtility.UrlEncode(fileName) + DateTime.Now.ToString("yyyyMMddHHmmss")));
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.ContentType = "application/ms-excel";
            Response.BinaryWrite(workbook.SaveToStream().ToArray());
            Response.End();
        }

        /// <summary>
        /// 查询类型切换事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rbtnlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //pointCbxRsm.Visible = false;
            dvPoint.Style["display"] = "none";
            comboCity.Visible = false;
            switch (rbtnlType.SelectedValue)
            {
                case "City":
                    comboCity.Visible = true;
                    break;
                case "Port":
                    //pointCbxRsm.Visible = true;
                    dvPoint.Style["display"] = "normal";
                    break;
            }
        }
        #endregion

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
    }
}