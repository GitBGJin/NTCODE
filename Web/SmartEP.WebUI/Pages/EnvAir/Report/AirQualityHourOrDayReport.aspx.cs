using Aspose.Cells;
using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.Channel;
using SmartEP.Service.DataAnalyze.Air.AQIReport;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.Report
{
    public partial class AirQualityHourOrDayReport : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 小时数据接口
        /// </summary>
        HourAQIService g_HourAQIService = Singleton<HourAQIService>.GetInstance();
        /// <summary>
        /// 日数据接口
        /// </summary>
        DayAQIService g_DayAQIService = Singleton<DayAQIService>.GetInstance();
        /// <summary>
        /// 因子接口
        /// </summary>
        AirPollutantService g_AirPollutantService = Singleton<AirPollutantService>.GetInstance();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //小时，24小时
                dtpBegin.SelectedDate = DateTime.Parse(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:00"));
                dtpEnd.SelectedDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:00"));
                //日，7天
                BeginDate.SelectedDate = DateTime.Parse(DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd"));
                EndDate.SelectedDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                //平均值(默认是小时数据)
                ViewState["AVGHour"] = null;
                ViewState["AVGDay"] = null;
                //IPoint[] Points = pointCbxRsm.GetPoints().ToArray();
                //List<int> PointIds = new List<int>();
                //foreach (IPoint point in Points)
                //{
                //    PointIds.Add(Convert.ToInt32(point.PointID));
                //}
                //DataTable dt = g_HourAQIService.GetAvgHourAQIData(PointIds, dtpBegin.SelectedDate.Value, dtpEnd.SelectedDate.Value);
                //if (dt != null && dt.Rows.Count == 1)
                //{
                //    ViewState["AVGHour"] = dt;
                //}

            }
        }

        #region 数据类型变更事件
        /// <summary>
        /// 数据类型变更事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rbtnlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rbtnlType.SelectedValue == "HourData")
            {
                div_hour.Visible = true;
                div_day.Visible = false;
            }
            if (rbtnlType.SelectedValue == "DayData")
            {
                div_hour.Visible = false;
                div_day.Visible = true;
            }
        }
        #endregion

        #region 查询按钮事件
        /// <summary>
        /// 查询按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            RadGrid1.Rebind();
        }
        #endregion

        #region 绑定RadGrid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {
            IPoint[] Points = pointCbxRsm.GetPoints().ToArray();
            List<int> PointIds = new List<int>();
            foreach (IPoint point in Points)
            {
                PointIds.Add(Convert.ToInt32(point.PointID));
            }
            PollutantCodeEntity[] Factors = g_AirPollutantService.RetrieveListByCalAQI().ToArray();
            //小时数据
            if (rbtnlType.SelectedValue == "HourData")
            {
                DateTime dtmBegion = dtpBegin.SelectedDate == null ? Convert.ToDateTime("1900-01-01 01:00") : Convert.ToDateTime(dtpBegin.SelectedDate);
                DateTime dtmEnd = dtpEnd.SelectedDate == null ? Convert.ToDateTime("1900-01-01 01:00") : Convert.ToDateTime(dtpEnd.SelectedDate);
                if (dtpBegin.SelectedDate <= Convert.ToDateTime("1900-01-01 01:00") || dtpEnd.SelectedDate <= Convert.ToDateTime("1900-01-01 01:00"))
                {
                    Alert("请选择开始、截止时间");
                    return;
                }
                if (dtmBegion > dtmEnd)
                {
                    Alert("请选择正确的开始、截止时间");
                    return;
                }
                DataTable dt = g_HourAQIService.GetHourAQIData(PointIds, dtmBegion, dtmEnd);
                int recordTotal = dt != null ? dt.Rows.Count : 0;
                RadGrid1.DataSource = dt;
                RadGrid1.VirtualItemCount = recordTotal;
                //计算平均值
                DataTable AVG_dt = g_HourAQIService.GetAvgHourAQIData(PointIds, dtmBegion, dtmEnd);
                if (AVG_dt != null && AVG_dt.Rows.Count == 1)
                {
                    ViewState["AVGHour"] = AVG_dt;
                }
            }
            //日数据
            if (rbtnlType.SelectedValue == "DayData")
            {
                DateTime dtmBegion = BeginDate.SelectedDate == null ? Convert.ToDateTime("1900-01-01") : Convert.ToDateTime(BeginDate.SelectedDate);
                DateTime dtmEnd = EndDate.SelectedDate == null ? Convert.ToDateTime("1900-01-01") : Convert.ToDateTime(EndDate.SelectedDate);
                if (dtpBegin.SelectedDate <= Convert.ToDateTime("1900-01-01") || dtpEnd.SelectedDate <= Convert.ToDateTime("1900-01-01"))
                {
                    Alert("请选择开始、截止时间");
                    return;
                }
                if (dtmBegion > dtmEnd)
                {
                    Alert("请选择正确的开始、截止时间");
                    return;
                }
                DataTable dt = g_DayAQIService.GetDayAQIData(PointIds, dtmBegion, dtmEnd);
                int recordTotal = dt != null ? dt.Rows.Count : 0;
                RadGrid1.DataSource = dt;
                RadGrid1.VirtualItemCount = recordTotal;
                //计算平均值
                DataTable AVG_dt = g_DayAQIService.GetAvgDayAQIData(PointIds, dtmBegion, dtmEnd);
                if (AVG_dt != null && AVG_dt.Rows.Count == 1)
                {
                    ViewState["AVGDay"] = AVG_dt;
                }
            }
        }
        #endregion

        #region ToolBar事件
        /// <summary>
        /// ToolBar事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridRTB_ButtonClick(object sender, Telerik.Web.UI.RadToolBarEventArgs e)
        {
            Telerik.Web.UI.RadToolBarButton button = (Telerik.Web.UI.RadToolBarButton)e.Item;
            if (button.CommandName == "ExportToExcel")
            {
                string[] pointIds = pointCbxRsm.GetPointValues(CbxRsmReturnType.ID);
                string orderBy = "PointId,DateTime Desc";
                //小时数据
                if (rbtnlType.SelectedValue == "HourData")
                {
                    DateTime dtmBegion = dtpBegin.SelectedDate == null ? Convert.ToDateTime("1900-01-01 01:00") : Convert.ToDateTime(dtpBegin.SelectedDate);
                    DateTime dtmEnd = dtpEnd.SelectedDate == null ? Convert.ToDateTime("1900-01-01 01:00") : Convert.ToDateTime(dtpEnd.SelectedDate);
                    if (dtpBegin.SelectedDate <= Convert.ToDateTime("1900-01-01 01:00") || dtpEnd.SelectedDate <= Convert.ToDateTime("1900-01-01 01:00"))
                    {
                        Alert("请选择开始、截止时间");
                        return;
                    }
                    if (dtmBegion > dtmEnd)
                    {
                        Alert("请选择正确的开始、截止时间");
                        return;
                    }
                    DataView dv = g_HourAQIService.GetPortExportData(pointIds, dtmBegion, dtmEnd, orderBy);
                    DataTableToExcelForHour(dv, "空气质量实时报", "空气质量实时报");
                }
                //日数据
                if (rbtnlType.SelectedValue == "DayData")
                {
                    DateTime dtmBegion = BeginDate.SelectedDate == null ? Convert.ToDateTime("1900-01-01") : Convert.ToDateTime(BeginDate.SelectedDate);
                    DateTime dtmEnd = EndDate.SelectedDate == null ? Convert.ToDateTime("1900-01-01") : Convert.ToDateTime(EndDate.SelectedDate);
                    if (dtpBegin.SelectedDate <= Convert.ToDateTime("1900-01-01") || dtpEnd.SelectedDate <= Convert.ToDateTime("1900-01-01"))
                    {
                        Alert("请选择开始、截止时间");
                        return;
                    }
                    if (dtmBegion > dtmEnd)
                    {
                        Alert("请选择正确的开始、截止时间");
                        return;
                    }
                    DataView dv = g_DayAQIService.GetPortExportData(pointIds, dtmBegion, dtmEnd, orderBy);
                    DataTableToExcelForDay(dv, "空气质量日报", "空气质量日报");
                }
            }
        }
        #endregion

        #region 导出空气质量日报Excel
        /// <summary>
        /// 导出空气质量日报Excel
        /// </summary>
        /// <param name="dv">数据源</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="sheetName">Sheet页名称</param>
        /// <returns></returns>
        private void DataTableToExcelForDay(DataView dv, string fileName, string sheetName)
        {
            IList<IPoint> points = pointCbxRsm.GetPoints();
            IList<PollutantCodeEntity> factors = g_AirPollutantService.RetrieveListByCalAQI().ToList();
            DataTable dtNew = dv.ToTable();

            Workbook workbook = new Workbook();
            Worksheet sheet = workbook.Worksheets[0];
            Cells cells = sheet.Cells;
            Aspose.Cells.Style cellStyle = workbook.Styles[workbook.Styles.Add()];
            workbook.FileName = fileName;
            sheet.Name = sheetName;

            //样式
            cellStyle.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center; // 设置居中 
            cellStyle.Font.Name = "宋体"; //文字字体
            cellStyle.Font.Size = 12;//文字大小
            cellStyle.IsLocked = false; //单元格解锁
            cellStyle.IsTextWrapped = true; //单元格内容自动换行

            #region 数据修改

            if (!dtNew.Columns.Contains("PointName"))
            {
                dtNew.Columns.Add("PointName");
            }
            for (int i = 0; i < dtNew.Rows.Count; i++)
            {
                DataRow drNew = dtNew.Rows[i];
                drNew["PointName"] = (points.Count(t => t.PointID == drNew["PointId"].ToString()) > 0)
                 ? points.Where(t => t.PointID == drNew["PointId"].ToString()).Select(t => t.PointName).FirstOrDefault()
                 : drNew["PointId"].ToString();
                for (int j = 0; j < factors.Count; j++)
                {
                    string[] uniqueNames = GetUniqueNameByPollutantName(factors[j].PollutantName);
                    foreach (string uniqueName in uniqueNames)
                    {
                        if (dtNew.Columns.Contains(uniqueName) && !string.IsNullOrWhiteSpace(drNew[uniqueName].ToString()))
                        {
                            //获取因子小数位,channelCode 因子代码 例：CO的code是a21005  （ channelCode=a21005）
                            int DecimalNum = Convert.ToInt32(g_AirPollutantService.GetPollutantInfo(factors[j].PollutantCode).PollutantDecimalNum);
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
            cells.Merge(0, 2, 1, 14);
            cells[0, 16].PutValue("空气质量指数(AQI)");
            cells.Merge(0, 16, 3, 1);
            cells[0, 17].PutValue("首要污染物");
            cells.Merge(0, 17, 3, 1);
            cells[0, 18].PutValue("空气质量指数级别");
            cells.Merge(0, 18, 3, 1);
            cells[0, 19].PutValue("空气质量指数类别");
            cells.Merge(0, 19, 2, 2);

            //第二行
            cells[1, 2].PutValue("二氧化硫(SO2)24小时平均值");
            cells.Merge(1, 2, 1, 2);
            cells[1, 4].PutValue("二氧化氮(NO2)24小时平均值");
            cells.Merge(1, 4, 1, 2);
            cells[1, 6].PutValue("PM10 24小时平均值");
            cells.Merge(1, 6, 1, 2);
            cells[1, 8].PutValue("一氧化碳(CO)24小时平均值");
            cells.Merge(1, 8, 1, 2);
            cells[1, 10].PutValue("臭氧(O3)最大1小时平均值");
            cells.Merge(1, 10, 1, 2);
            cells[1, 12].PutValue("臭氧(O3)最大8小时滑动平均值");
            cells.Merge(1, 12, 1, 2);
            cells[1, 14].PutValue("PM2.5 24小时平均值");
            cells.Merge(1, 14, 1, 2);

            //第三行
            cells[2, 2].PutValue("浓度/(μg/m3)");
            cells[2, 3].PutValue("分指数");
            cells[2, 4].PutValue("浓度/(μg/m3)");
            cells[2, 5].PutValue("分指数");
            cells[2, 6].PutValue("浓度/(μg/m3)");
            cells[2, 7].PutValue("分指数");
            cells[2, 8].PutValue("浓度/(mg/m3)");
            cells[2, 9].PutValue("分指数");
            cells[2, 10].PutValue("浓度/(μg/m3)");
            cells[2, 11].PutValue("分指数");
            cells[2, 12].PutValue("浓度/(μg/m3)");
            cells[2, 13].PutValue("分指数");
            cells[2, 14].PutValue("浓度/(μg/m3)");
            cells[2, 15].PutValue("分指数");
            cells[2, 19].PutValue("类别");
            cells[2, 20].PutValue("颜色");
            cells.SetRowHeight(1, 30);//设置行高
            cells.SetColumnWidth(0, 20);//设置列宽
            cells.SetColumnWidth(1, 13);//设置列宽
            cells.SetColumnWidth(17, 10);//设置列宽
            cells.SetColumnWidth(19, 10);//设置列宽
            cells.SetColumnWidth(20, 10);//设置列宽
            for (int i = 2; i <= 15; i++)
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
                    cells[rowIndex, 1].PutValue(string.Format("{0:yyyy-MM-dd}", drNew["DateTime"]));
                }
                else
                {
                    cells[rowIndex, 1].PutValue(string.Format("{0:yyyy-MM-dd}", drNew["ReportDateTime"]));
                }
                cells[rowIndex, 2].PutValue(drNew["SO2"].ToString());
                cells[rowIndex, 3].PutValue(drNew["SO2_IAQI"].ToString());
                cells[rowIndex, 4].PutValue(drNew["NO2"].ToString());
                cells[rowIndex, 5].PutValue(drNew["NO2_IAQI"].ToString());
                cells[rowIndex, 6].PutValue(drNew["PM10"].ToString());
                cells[rowIndex, 7].PutValue(drNew["PM10_IAQI"].ToString());
                cells[rowIndex, 8].PutValue(drNew["CO"].ToString());
                cells[rowIndex, 9].PutValue(drNew["CO_IAQI"].ToString());
                cells[rowIndex, 10].PutValue(drNew["MaxOneHourO3"].ToString());
                cells[rowIndex, 11].PutValue(drNew["MaxOneHourO3_IAQI"].ToString());
                cells[rowIndex, 12].PutValue(drNew["Max8HourO3"].ToString());
                cells[rowIndex, 13].PutValue(drNew["Max8HourO3_IAQI"].ToString());
                cells[rowIndex, 14].PutValue(drNew["PM25"].ToString());
                cells[rowIndex, 15].PutValue(drNew["PM25_IAQI"].ToString());
                cells[rowIndex, 16].PutValue(drNew["AQIValue"].ToString());
                cells[rowIndex, 17].PutValue(drNew["PrimaryPollutant"].ToString());
                cells[rowIndex, 18].PutValue(drNew["Grade"].ToString());
                cells[rowIndex, 19].PutValue(drNew["Class"].ToString());
                cells[rowIndex, 20].PutValue("");
                if (drNew["RGBValue"].ToString() != "")
                {
                    Aspose.Cells.Style styleTemp = cells[rowIndex, 20].GetStyle();
                    styleTemp.ForegroundColor = System.Drawing.ColorTranslator.FromHtml(drNew["RGBValue"].ToString());//设置背景色
                    styleTemp.Pattern = BackgroundType.Solid;//设置背景样式
                    cells[rowIndex, 20].SetStyle(styleTemp);
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
        #endregion

        #region 导出空气质量实时报Excel
        /// <summary>
        /// 导出空气质量实时报Excel
        /// </summary>
        /// <param name="dv">数据源</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="sheetName">Sheet页名称</param>
        private void DataTableToExcelForHour(DataView dv, string fileName, string sheetName)
        {
            IList<IPoint> points = pointCbxRsm.GetPoints();
            IList<PollutantCodeEntity> factors = g_AirPollutantService.RetrieveListByCalAQI().ToList();
            DataTable dtNew = dv.ToTable();

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
            if (!dtNew.Columns.Contains("PointName"))
            {
                dtNew.Columns.Add("PointName");
            }
            for (int i = 0; i < dtNew.Rows.Count; i++)
            {
                DataRow drNew = dtNew.Rows[i];
                drNew["PointName"] = (points.Count(t => t.PointID == drNew["PointId"].ToString()) > 0)
                 ? points.Where(t => t.PointID == drNew["PointId"].ToString()).Select(t => t.PointName).FirstOrDefault()
                 : drNew["PointId"].ToString();
                for (int j = 0; j < factors.Count; j++)
                {
                    string[] uniqueNames = GetUniqueNameByPollutantName(factors[j].PollutantName);
                    foreach (string uniqueName in uniqueNames)
                    {
                        if (dtNew.Columns.Contains(uniqueName) && !string.IsNullOrWhiteSpace(drNew[uniqueName].ToString()))
                        {
                            //获取因子小数位,channelCode 因子代码 例：CO的code是a21005  （ channelCode=a21005）
                            int DecimalNum = Convert.ToInt32(g_AirPollutantService.GetPollutantInfo(factors[j].PollutantCode).PollutantDecimalNum);
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
            cells[1, 2].PutValue("二氧化硫(SO2)1小时平均");
            cells.Merge(1, 2, 1, 2);
            cells[1, 4].PutValue("二氧化氮(NO2)1小时平均");
            cells.Merge(1, 4, 1, 2);
            cells[1, 6].PutValue("PM10 1小时平均");
            cells.Merge(1, 6, 1, 2);
            cells[1, 8].PutValue("一氧化碳(CO)1小时平均");
            cells.Merge(1, 8, 1, 2);
            cells[1, 10].PutValue("臭氧(O3)1小时平均");
            cells.Merge(1, 10, 1, 2);
            //cells[1, 12].PutValue("臭氧(O3)8小时滑动平均");
            //cells.Merge(1, 12, 1, 2);
            cells[1, 12].PutValue("PM2.5 1小时平均");
            cells.Merge(1, 12, 1, 2);

            //第三行
            cells[2, 2].PutValue("浓度/(μg/m3)");
            cells[2, 3].PutValue("分指数");
            cells[2, 4].PutValue("浓度/(μg/m3)");
            cells[2, 5].PutValue("分指数");
            cells[2, 6].PutValue("浓度/(μg/m3)");
            cells[2, 7].PutValue("分指数");
            cells[2, 8].PutValue("浓度/(mg/m3)");
            cells[2, 9].PutValue("分指数");
            cells[2, 10].PutValue("浓度/(μg/m3)");
            cells[2, 11].PutValue("分指数");
            cells[2, 12].PutValue("浓度/(μg/m3)");
            cells[2, 13].PutValue("分指数");
            //cells[2, 14].PutValue("浓度/(μg/m3)");
            //cells[2, 15].PutValue("分指数");
            cells[2, 17].PutValue("类别");
            cells[2, 18].PutValue("颜色");
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
                cells[rowIndex, 2].PutValue(drNew["SO2"].ToString());
                cells[rowIndex, 3].PutValue(drNew["SO2_IAQI"].ToString());
                cells[rowIndex, 4].PutValue(drNew["NO2"].ToString());
                cells[rowIndex, 5].PutValue(drNew["NO2_IAQI"].ToString());
                cells[rowIndex, 6].PutValue(drNew["PM10"].ToString());
                cells[rowIndex, 7].PutValue(drNew["PM10_IAQI"].ToString());
                cells[rowIndex, 8].PutValue(drNew["CO"].ToString());
                cells[rowIndex, 9].PutValue(drNew["CO_IAQI"].ToString());
                cells[rowIndex, 10].PutValue(drNew["O3"].ToString());
                cells[rowIndex, 11].PutValue(drNew["O3_IAQI"].ToString());
                //cells[rowIndex, 12].PutValue(drNew["Recent8HoursO3"].ToString());
                //cells[rowIndex, 13].PutValue(drNew["Recent8HoursO3_IAQI"].ToString());
                cells[rowIndex, 12].PutValue(drNew["PM25"].ToString());
                cells[rowIndex, 13].PutValue(drNew["PM25_IAQI"].ToString());
                cells[rowIndex, 14].PutValue(drNew["AQIValue"].ToString());
                cells[rowIndex, 15].PutValue(drNew["PrimaryPollutant"].ToString());
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

        #endregion

        #region 绑定数据源
        /// <summary>
        /// 绑定数据源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }
        #endregion

        #region 数据行绑定处理
        /// <summary>
        /// 数据行绑定处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RadGrid1_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            IPoint[] Points = pointCbxRsm.GetPoints().ToArray();
            PollutantCodeEntity[] Factors = g_AirPollutantService.RetrieveListByCalAQI().ToArray();
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                DataRowView drv = e.Item.DataItem as DataRowView;
                if (item["DateTime"] != null)
                {
                    GridTableCell dateTimeCell = (GridTableCell)item["DateTime"];
                    DateTime dateTime;
                    if (rbtnlType.SelectedValue == "HourData")
                    {
                        if (DateTime.TryParse(dateTimeCell.Text, out dateTime))
                        {
                            dateTimeCell.Text = dateTime.AddHours(1).ToString("MM-dd HH时");
                        }
                    }
                    if (rbtnlType.SelectedValue == "DayData")
                    {
                        if (DateTime.TryParse(dateTimeCell.Text, out dateTime))
                        {
                            dateTimeCell.Text = dateTime.ToString("yyyy-MM-dd");
                        }
                    }
                }
                if (item["RGBValue"] != null)
                {
                    GridTableCell cell = item["RGBValue"] as GridTableCell;
                    cell.Style.Add("background-color", cell.Text);
                    cell.Text = string.Empty;
                }
                for (int i = 0; i < Factors.Length; i++)
                {
                    string[] uniqueNames = GetUniqueNameByPollutantName(Factors[i].PollutantName);
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
                                int DecimalNum = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo(Factors[i].PollutantCode).PollutantDecimalNum);
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
        #endregion

        #region 因子名称转换
        /// <summary>
        /// 因子名称转换
        /// </summary>
        /// <param name="pollutantName"></param>
        /// <returns></returns>
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
        #endregion

        #region 列创建事件
        /// <summary>
        /// 列创建事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RadGrid1_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            DataTable dt = new DataTable();
            //小时数据
            if (rbtnlType.SelectedValue == "HourData" && ViewState["AVGHour"] != null)
            {
                dt = (DataTable)ViewState["AVGHour"];
            }
            //日数据
            if (rbtnlType.SelectedValue == "DayData" && ViewState["AVGDay"] != null)
            {
                dt = (DataTable)ViewState["AVGDay"];
            }
            try
            {
                if (e.Column.ColumnType.Equals("GridExpandColumn"))
                {
                    return;
                }
                GridBoundColumn col = (GridBoundColumn)e.Column;
                if (col.DataField == "PointName")
                {
                    col.HeaderText = "监测点位名称";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(120);
                    col.ItemStyle.Width = Unit.Pixel(120);
                    col.FooterText = "";
                }
                else if (col.DataField == "DateTime")
                {
                    col.HeaderText = "日期";
                    if (rbtnlType.SelectedValue == "DayData")
                    {
                        col.DataFormatString = "{0:yyyy-MM-dd}";
                    }
                    if (rbtnlType.SelectedValue == "HourData")
                    {
                        col.DataFormatString = "{0:yyyy-MM-dd HH:00}";
                    }
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(120);
                    col.ItemStyle.Width = Unit.Pixel(120);
                    col.FooterText = "平均值：";
                }
                else if (col.DataField == "SO2")
                {
                    col.HeaderText = "浓度/(μg/m<sup>3</sup>)";
                    col.EmptyDataText = "--";
                    col.ColumnGroupName = "SO2";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(90);
                    col.ItemStyle.Width = Unit.Pixel(90);
                    col.FooterText = "";
                    if (dt != null && dt.Rows.Count == 1 && !Convert.IsDBNull(dt.Rows[0]["AVGSO2"]))
                    {
                        col.FooterText = Convert.ToDouble(Math.Round(Convert.ToDecimal(dt.Rows[0]["AVGSO2"]) * 1000, 3)).ToString();
                    }
                }
                else if (col.DataField == "SO2_IAQI")
                {
                    col.HeaderText = "分指数";
                    col.EmptyDataText = "--";
                    col.ColumnGroupName = "SO2";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(60);
                    col.ItemStyle.Width = Unit.Pixel(60);
                    col.FooterText = "";
                    if (dt != null && dt.Rows.Count == 1 && !Convert.IsDBNull(dt.Rows[0]["AVGSO2_IAQI"]))
                    {
                        col.FooterText = Convert.ToDouble(Math.Round(Convert.ToDecimal(dt.Rows[0]["AVGSO2_IAQI"]), 3)).ToString();
                    }
                }
                else if (col.DataField == "NO2")
                {
                    col.HeaderText = "浓度/(μg/m<sup>3</sup>)";
                    col.EmptyDataText = "--";
                    col.ColumnGroupName = "NO2";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(90);
                    col.ItemStyle.Width = Unit.Pixel(90);
                    col.FooterText = "";
                    if (dt != null && dt.Rows.Count == 1 && !Convert.IsDBNull(dt.Rows[0]["AVGNO2"]))
                    {
                        col.FooterText = Convert.ToDouble(Math.Round(Convert.ToDecimal(dt.Rows[0]["AVGNO2"]) * 1000, 3)).ToString();
                    }
                }
                else if (col.DataField == "NO2_IAQI")
                {
                    col.HeaderText = "分指数";
                    col.EmptyDataText = "--";
                    col.ColumnGroupName = "NO2";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(60);
                    col.ItemStyle.Width = Unit.Pixel(60);
                    col.FooterText = "";
                    if (dt != null && dt.Rows.Count == 1 && !Convert.IsDBNull(dt.Rows[0]["AVGNO2_IAQI"]))
                    {
                        col.FooterText = Convert.ToDouble(Math.Round(Convert.ToDecimal(dt.Rows[0]["AVGNO2_IAQI"]), 3)).ToString();
                    }
                }
                else if (col.DataField == "PM10")
                {
                    col.HeaderText = "浓度/(μg/m<sup>3</sup>)";
                    col.EmptyDataText = "--";
                    col.ColumnGroupName = "PM10";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(90);
                    col.ItemStyle.Width = Unit.Pixel(90);
                    col.FooterText = "";
                    if (dt != null && dt.Rows.Count == 1 && !Convert.IsDBNull(dt.Rows[0]["AVGPM10"]))
                    {
                        col.FooterText = Convert.ToDouble(Math.Round(Convert.ToDecimal(dt.Rows[0]["AVGPM10"]) * 1000, 3)).ToString();
                    }
                }
                else if (col.DataField == "PM10_IAQI")
                {
                    col.HeaderText = "分指数";
                    col.EmptyDataText = "--";
                    col.ColumnGroupName = "PM10";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(60);
                    col.ItemStyle.Width = Unit.Pixel(60);
                    col.FooterText = "";
                    if (dt != null && dt.Rows.Count == 1 && !Convert.IsDBNull(dt.Rows[0]["AVGPM10_IAQI"]))
                    {
                        col.FooterText = Convert.ToDouble(Math.Round(Convert.ToDecimal(dt.Rows[0]["AVGPM10_IAQI"]), 3)).ToString();
                    }
                }
                else if (col.DataField == "CO")
                {
                    col.HeaderText = "浓度/(mg/m<sup>3</sup>)";
                    col.EmptyDataText = "--";
                    col.ColumnGroupName = "CO";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(90);
                    col.ItemStyle.Width = Unit.Pixel(90);
                    col.FooterText = "";
                    if (dt != null && dt.Rows.Count == 1 && !Convert.IsDBNull(dt.Rows[0]["AVGCO"]))
                    {
                        col.FooterText = Convert.ToDouble(Math.Round(Convert.ToDecimal(dt.Rows[0]["AVGCO"]), 3)).ToString();
                    }
                }
                else if (col.DataField == "CO_IAQI")
                {
                    col.HeaderText = "分指数";
                    col.EmptyDataText = "--";
                    col.ColumnGroupName = "CO";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(60);
                    col.ItemStyle.Width = Unit.Pixel(60);
                    col.FooterText = "";
                    if (dt != null && dt.Rows.Count == 1 && !Convert.IsDBNull(dt.Rows[0]["AVGCO_IAQI"]))
                    {
                        col.FooterText = Convert.ToDouble(Math.Round(Convert.ToDecimal(dt.Rows[0]["AVGCO_IAQI"]), 3)).ToString();
                    }
                }
                else if (col.DataField == "O3")
                {
                    col.HeaderText = "浓度/(μg/m<sup>3</sup>)";
                    col.EmptyDataText = "--";
                    col.ColumnGroupName = "O31";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(90);
                    col.ItemStyle.Width = Unit.Pixel(90);
                    col.FooterText = "";
                    if (dt != null && dt.Rows.Count == 1 && !Convert.IsDBNull(dt.Rows[0]["AVGO3"]))
                    {
                        col.FooterText = Convert.ToDouble(Math.Round(Convert.ToDecimal(dt.Rows[0]["AVGO3"]) * 1000, 3)).ToString();
                    }
                }
                else if (col.DataField == "O3_IAQI")
                {
                    col.HeaderText = "分指数";
                    col.EmptyDataText = "--";
                    col.ColumnGroupName = "O31";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(60);
                    col.ItemStyle.Width = Unit.Pixel(60);
                    col.FooterText = "";
                    if (dt != null && dt.Rows.Count == 1 && !Convert.IsDBNull(dt.Rows[0]["AVGO3_IAQI"]))
                    {
                        col.FooterText = Convert.ToDouble(Math.Round(Convert.ToDecimal(dt.Rows[0]["AVGO3_IAQI"]), 3)).ToString();
                    }
                }
                else if (col.DataField == "Max8HourO3")
                {
                    if (rbtnlType.SelectedValue == "DayData")
                    {
                        col.HeaderText = "浓度/(μg/m<sup>3</sup>)";
                        col.EmptyDataText = "--";
                        col.ColumnGroupName = "O38";
                        col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                        col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                        col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                        col.HeaderStyle.Width = Unit.Pixel(90);
                        col.ItemStyle.Width = Unit.Pixel(90);
                        col.FooterText = "";
                        if (dt != null && dt.Rows.Count == 1 && !Convert.IsDBNull(dt.Rows[0]["AVGO3_8"]))
                        {
                            col.FooterText = Convert.ToDouble(Math.Round(Convert.ToDecimal(dt.Rows[0]["AVGO3_8"]) * 1000, 3)).ToString();
                        }
                    }
                    else
                    {
                        e.Column.Visible = false;
                    }
                }
                else if (col.DataField == "Max8HourO3_IAQI")
                {
                    if (rbtnlType.SelectedValue == "DayData")
                    {
                        col.HeaderText = "分指数";
                        col.EmptyDataText = "--";
                        col.ColumnGroupName = "O38";
                        col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                        col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                        col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                        col.HeaderStyle.Width = Unit.Pixel(60);
                        col.ItemStyle.Width = Unit.Pixel(60);
                        col.FooterText = "";
                        if (dt != null && dt.Rows.Count == 1 && !Convert.IsDBNull(dt.Rows[0]["AVGO3_8_IAQI"]))
                        {
                            col.FooterText = Convert.ToDouble(Math.Round(Convert.ToDecimal(dt.Rows[0]["AVGO3_8_IAQI"]), 3)).ToString();
                        }
                    }
                    else
                    {
                        e.Column.Visible = false;
                    }
                }
                else if (col.DataField == "PM25")
                {
                    col.HeaderText = "浓度/(μg/m<sup>3</sup>)";
                    col.EmptyDataText = "--";
                    col.ColumnGroupName = "PM2.5";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(90);
                    col.ItemStyle.Width = Unit.Pixel(90);
                    col.FooterText = "";
                    if (dt != null && dt.Rows.Count == 1 && !Convert.IsDBNull(dt.Rows[0]["AVGPM25"]))
                    {
                        col.FooterText = Convert.ToDouble(Math.Round(Convert.ToDecimal(dt.Rows[0]["AVGPM25"]) * 1000, 3)).ToString();
                    }
                }
                else if (col.DataField == "PM25_IAQI")
                {
                    col.HeaderText = "分指数";
                    col.EmptyDataText = "--";
                    col.ColumnGroupName = "PM2.5";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(60);
                    col.ItemStyle.Width = Unit.Pixel(60);
                    col.FooterText = "";
                    if (dt != null && dt.Rows.Count == 1 && !Convert.IsDBNull(dt.Rows[0]["AVGPM25_IAQI"]))
                    {
                        col.FooterText = Convert.ToDouble(Math.Round(Convert.ToDecimal(dt.Rows[0]["AVGPM25_IAQI"]), 3)).ToString();
                    }
                }
                else if (col.DataField == "AQIValue")
                {
                    col.HeaderText = "空气质量<br />指数(AQI)";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(70);
                    col.ItemStyle.Width = Unit.Pixel(70);
                    col.FooterText = "";
                    if (dt != null && dt.Rows.Count == 1 && !Convert.IsDBNull(dt.Rows[0]["AQI_MaxValue"]))
                    {
                        col.FooterText = Convert.ToDouble(Math.Round(Convert.ToDecimal(dt.Rows[0]["AQI_MaxValue"]), 3)).ToString();
                    }
                }
                else if (col.DataField == "PrimaryPollutant")
                {
                    col.HeaderText = "首要污染物";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(80);
                    col.ItemStyle.Width = Unit.Pixel(80);
                    col.FooterText = "";
                }
                else if (col.DataField == "Grade")
                {
                    col.HeaderText = "空气质量<br />指数级别";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(70);
                    col.ItemStyle.Width = Unit.Pixel(70);
                    col.FooterText = "";
                }
                else if (col.DataField == "Class")
                {
                    col.HeaderText = "类别";
                    col.EmptyDataText = "--";
                    col.ColumnGroupName = "空气质量指数类别";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(65);
                    col.ItemStyle.Width = Unit.Pixel(65);
                    col.FooterText = "";
                }
                else if (col.DataField == "RGBValue")
                {
                    col.HeaderText = "颜色";
                    col.EmptyDataText = "--";
                    col.ColumnGroupName = "空气质量指数类别";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(65);
                    col.ItemStyle.Width = Unit.Pixel(65);
                    col.FooterText = "";
                }
                else
                {
                    e.Column.Visible = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}