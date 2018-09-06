using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.AutoMonitoring.Air;
using SmartEP.Service.BaseData.Channel;
using SmartEP.Service.BaseData.MPInfo;
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

namespace SmartEP.WebUI.Pages.EnvAir.DataAnalyze
{
    /// <summary>
    /// 名称：DataEffectRateAnalyze.cs
    /// 创建人：尤红兵
    /// 创建日期：2015-08-21
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：数据有效率统计
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class DataEffectRateAnalyze : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        AirDataEffectRateService airDataEffectRate = new AirDataEffectRateService();

        /// <summary>
        /// 选择站点
        /// </summary>
        private IList<IPoint> points = null;

        /// <summary>
        /// 统计
        /// </summary>
        DataView dvStatistical = null;

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
           
            MonitoringPointAirService m_MonitoringPointAirService = Singleton<MonitoringPointAirService>.GetInstance();
            IQueryable<MonitoringPointEntity> monitoringPointQueryable = m_MonitoringPointAirService.RetrieveAirMPListByEnable();//获取所有启用的空气点位列表
            monitoringPointQueryable = monitoringPointQueryable.Where(p => p.ContrlUid == "6fadff52-2338-4319-9f1d-7317823770ad"     //国控点
                                                                           || p.ContrlUid == "c1158eb6-4d69-4846-a963-d16b9d2794ca"  //对照点
                                                                           || p.ContrlUid == "bdf0837a-eb59-4c4a-a05f-c774a17f3077");//路边站
            string pointNames = monitoringPointQueryable.Select(t => t.MonitoringPointName)
                                    .Aggregate((a, b) => a + ";" + b);
            IList<PollutantCodeEntity> pollutantCodeList = GetPollutantListByCalAQI();
            pointCbxRsm.SetPointValuesFromNames(pointNames);
           

            #region 初始化时间
            dayBegin.SelectedDate = DateTime.Now.AddDays(-7);
            dayEnd.SelectedDate = DateTime.Now.AddDays(-1);
            monthBegin.SelectedDate = DateTime.Now.AddMonths(-1);
            monthEnd.SelectedDate = DateTime.Now;

            weekBegin.SelectedDate = DateTime.Now;
            weekEnd.SelectedDate = DateTime.Now;

            int yearNow = DateTime.Now.Year;

            ddlQuarterYearBegin.Items.Clear();//季度开始年
            ddlQuarterYearEnd.Items.Clear();//季度结束年
            ddlYearBegin.Items.Clear();//年开始
            ddlYearEnd.Items.Clear();//年结束
            for (int i = yearNow; i >= yearNow - 6; i--)
            {
                //季度开始年
                DropDownListItem cmbItemBegin = new DropDownListItem();
                cmbItemBegin.Text = i.ToString();
                cmbItemBegin.Value = i.ToString();
                if (i == yearNow)
                {
                    cmbItemBegin.Selected = true;
                }
                ddlQuarterYearBegin.Items.Add(cmbItemBegin);

                //季度结束年
                DropDownListItem cmbItemEnd = new DropDownListItem();
                cmbItemEnd.Text = i.ToString();
                cmbItemEnd.Value = i.ToString();
                if (i == yearNow)
                {
                    cmbItemEnd.Selected = true;
                }
                ddlQuarterYearEnd.Items.Add(cmbItemEnd);

                //年开始
                DropDownListItem cmbItemYearBegin = new DropDownListItem();
                cmbItemYearBegin.Text = i.ToString();
                cmbItemYearBegin.Value = i.ToString();
                if (i == yearNow)
                {
                    cmbItemYearBegin.Selected = true;
                }
                ddlYearBegin.Items.Add(cmbItemYearBegin);

                //年结束
                DropDownListItem cmbItemYearEnd = new DropDownListItem();
                cmbItemYearEnd.Text = i.ToString();
                cmbItemYearEnd.Value = i.ToString();
                if (i == yearNow)
                {
                    cmbItemYearEnd.Selected = true;
                }
                ddlYearEnd.Items.Add(cmbItemYearEnd);
            }
            ddlQuarterYearBegin.DataBind();
            ddlQuarterYearEnd.DataBind();
            ddlYearBegin.DataBind();
            ddlYearEnd.DataBind();
            #endregion

            #region 初始化开始周
            DateTime weekTimeBegin = weekBegin.SelectedDate.Value;//开始时间
            int year = weekTimeBegin.Year;
            int month = weekTimeBegin.Month;
            int weekCount = ShowWeekSinMonth(year, month);//总共几周
            weekFrom.Items.Clear();
            for (int i = 1; i <= weekCount; i++)
            {
                DropDownListItem cmbItemBegin = new DropDownListItem();
                cmbItemBegin.Text = i.ToString();
                cmbItemBegin.Value = i.ToString();

                weekFrom.Items.Add(cmbItemBegin);
            }
            weekFrom.DataBind();
            #endregion

            #region 初始化结束周
            DateTime weekTimeEnd = weekEnd.SelectedDate.Value;//开始时间

            int weekEndCount = ShowWeekSinMonth(weekTimeEnd.Year, weekTimeEnd.Month);//总共几周
            weekTo.Items.Clear();
            for (int i = 1; i <= weekEndCount; i++)
            {
                DropDownListItem cmbItemBegin = new DropDownListItem();
                cmbItemBegin.Text = i.ToString();
                cmbItemBegin.Value = i.ToString();

                weekTo.Items.Add(cmbItemBegin);
            }
            weekTo.DataBind();
            #endregion

        }
        #endregion

        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {


            DateTime dtBegion = DateTime.Now;
            DateTime dtEnd = DateTime.Now;
            if (rbtnlType.SelectedValue == "day")
            {
                dtBegion = dayBegin.SelectedDate.Value;
                dtEnd = dayEnd.SelectedDate.Value;
            }
            else if (rbtnlType.SelectedValue == "week")
            {
                int weekBeginFrom = Convert.ToInt32(weekFrom.SelectedValue);//开始第几周

                DateTime weekTimeBegin = weekBegin.SelectedDate.Value;//开始时间
                int year = weekTimeBegin.Year;
                int month = weekTimeBegin.Month;
                dtBegion = Convert.ToDateTime(GetWeek(year, month, 0, weekBeginFrom));//总共几周

                int weekEndTo = Convert.ToInt32(weekTo.SelectedValue);//结束第几周
                DateTime weekTimeEnd = weekEnd.SelectedDate.Value;//开始时间

                dtEnd = Convert.ToDateTime(GetWeek(weekTimeEnd.Year, weekTimeEnd.Month, 1, weekEndTo));//总共几周


            }
            else if (rbtnlType.SelectedValue == "month")
            {
                DateTime mtBegin = monthBegin.SelectedDate.Value;
                //本月第一天时间 
                dtBegion = mtBegin.AddDays(-(mtBegin.Day) + 1);

                DateTime mtEnd = monthEnd.SelectedDate.Value;
                //将本月月数+1 
                DateTime dt2 = mtEnd.AddMonths(1);
                //本月最后一天时间 
                dtEnd = dt2.AddDays(-(mtEnd.Day));

            }
            else if (rbtnlType.SelectedValue == "quarter")
            {
                string quarterBegin = ddlQuarterBegin.SelectedValue;//第几季
                string quarterYearBegin = ddlQuarterYearBegin.SelectedValue;//年
                switch (quarterBegin)
                {
                    case "1": dtBegion = Convert.ToDateTime(quarterYearBegin + "/01/01");//开始时间
                        break;
                    case "2": dtBegion = Convert.ToDateTime(quarterYearBegin + "/04/01");//开始时间
                        break;
                    case "3": dtBegion = Convert.ToDateTime(quarterYearBegin + "/07/01");//开始时间
                        break;
                    case "4": dtBegion = Convert.ToDateTime(quarterYearBegin + "/10/01");//开始时间
                        break;
                }

                string quarterEnd = ddlQuarterEnd.SelectedValue;//第几季
                string quarterYearEnd = ddlQuarterYearEnd.SelectedValue;//年
                switch (quarterEnd)
                {
                    case "1":
                        DateTime quarter1 = Convert.ToDateTime(quarterYearEnd + "/03");//将本月月数+1 
                        DateTime dt1 = quarter1.AddMonths(1);
                        //本月最后一天时间 
                        dtEnd = dt1.AddDays(-(quarter1.Day));//结束时间
                        break;
                    case "2":
                        DateTime quarter2 = Convert.ToDateTime(quarterYearEnd + "/06");//将本月月数+1 
                        DateTime dt2 = quarter2.AddMonths(1);
                        //本月最后一天时间 
                        dtEnd = dt2.AddDays(-(quarter2.Day));//结束时间;
                        break;
                    case "3":
                        DateTime quarter3 = Convert.ToDateTime(quarterYearEnd + "/09");//将本月月数+1 
                        DateTime dt3 = quarter3.AddMonths(1);
                        //本月最后一天时间 
                        dtEnd = dt3.AddDays(-(quarter3.Day));//结束时间;
                        break;

                    case "4":
                        DateTime quarter4 = Convert.ToDateTime(quarterYearEnd + "/12");//将本月月数+1 
                        DateTime dt4 = quarter4.AddMonths(1);
                        //本月最后一天时间 
                        dtEnd = dt4.AddDays(-(quarter4.Day));//结束时间;
                        break;

                }

            }
            else if (rbtnlType.SelectedValue == "year")
            {
                string yearBegin = ddlYearBegin.SelectedValue;
                string yearEnd = ddlYearEnd.SelectedValue;
                dtBegion = Convert.ToDateTime(yearBegin + "/01/01");
                dtEnd = Convert.ToDateTime(yearEnd + "/12/31");
            }
            points = pointCbxRsm.GetPoints();
            string[] portIds = points.Select(t => t.PointID).ToArray();
            int pageSize = gridEffectRateAnalyze.PageSize;//每页显示数据个数  
            int pageNo = gridEffectRateAnalyze.CurrentPageIndex;//当前页的序号
            int recordTotal = 0;//数据总行数

            string orderBy = "PointId asc,Tstamp desc";
            //绑定数据
            var airDataEffectRateData = airDataEffectRate.GetPointEffectRateStatisticalData(portIds, dtBegion, dtEnd, pageSize, pageNo, out recordTotal, out dvStatistical, orderBy);
            if (airDataEffectRateData == null)
            {
                gridEffectRateAnalyze.DataSource = new DataTable();
            }
            else
            {
                gridEffectRateAnalyze.DataSource = airDataEffectRateData;
            }

            //数据总行数
            gridEffectRateAnalyze.VirtualItemCount = recordTotal;
        }

        #endregion

        #region 服务器端控件事件处理
        /// <summary>
        /// 绑定数据源（用于分页、排序）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void gridEffectRateAnalyze_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }

        /// <summary>
        /// 查询按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gridEffectRateAnalyze.Rebind();
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
                string orderBy = "PointId asc,Tstamp desc";
                DateTime dtBegion = DateTime.Now;
                DateTime dtEnd = DateTime.Now;
                if (rbtnlType.SelectedValue == "day")
                {
                    dtBegion = dayBegin.SelectedDate.Value;
                    dtEnd = dayEnd.SelectedDate.Value;
                }
                else if (rbtnlType.SelectedValue == "week")
                {
                    int weekBeginFrom = Convert.ToInt32(weekFrom.SelectedValue);//开始第几周

                    DateTime weekTimeBegin = weekBegin.SelectedDate.Value;//开始时间
                    int year = weekTimeBegin.Year;
                    int month = weekTimeBegin.Month;
                    dtBegion = Convert.ToDateTime(GetWeek(year, month, 0, weekBeginFrom));//总共几周

                    int weekEndTo = Convert.ToInt32(weekTo.SelectedValue);//结束第几周
                    DateTime weekTimeEnd = weekEnd.SelectedDate.Value;//开始时间

                    dtEnd = Convert.ToDateTime(GetWeek(weekTimeEnd.Year, weekTimeEnd.Month, 1, weekEndTo));//总共几周


                }
                else if (rbtnlType.SelectedValue == "month")
                {
                    DateTime mtBegin = monthBegin.SelectedDate.Value;
                    //本月第一天时间 
                    dtBegion = mtBegin.AddDays(-(mtBegin.Day) + 1);

                    DateTime mtEnd = monthEnd.SelectedDate.Value;
                    //将本月月数+1 
                    DateTime dt2 = mtEnd.AddMonths(1);
                    //本月最后一天时间 
                    dtEnd = dt2.AddDays(-(mtEnd.Day));

                }
                else if (rbtnlType.SelectedValue == "quarter")
                {
                    string quarterBegin = ddlQuarterBegin.SelectedValue;//第几季
                    string quarterYearBegin = ddlQuarterYearBegin.SelectedValue;//年
                    switch (quarterBegin)
                    {
                        case "1": dtBegion = Convert.ToDateTime(quarterYearBegin + "/01/01");//开始时间
                            break;
                        case "2": dtBegion = Convert.ToDateTime(quarterYearBegin + "/04/01");//开始时间
                            break;
                        case "3": dtBegion = Convert.ToDateTime(quarterYearBegin + "/07/01");//开始时间
                            break;
                        case "4": dtBegion = Convert.ToDateTime(quarterYearBegin + "/10/01");//开始时间
                            break;
                    }

                    string quarterEnd = ddlQuarterEnd.SelectedValue;//第几季
                    string quarterYearEnd = ddlQuarterYearEnd.SelectedValue;//年
                    switch (quarterEnd)
                    {
                        case "1":
                            DateTime quarter1 = Convert.ToDateTime(quarterYearEnd + "/03");//将本月月数+1 
                            DateTime dt1 = quarter1.AddMonths(1);
                            //本月最后一天时间 
                            dtEnd = dt1.AddDays(-(quarter1.Day));//结束时间
                            break;
                        case "2":
                            DateTime quarter2 = Convert.ToDateTime(quarterYearEnd + "/06");//将本月月数+1 
                            DateTime dt2 = quarter2.AddMonths(1);
                            //本月最后一天时间 
                            dtEnd = dt2.AddDays(-(quarter2.Day));//结束时间;
                            break;
                        case "3":
                            DateTime quarter3 = Convert.ToDateTime(quarterYearEnd + "/09");//将本月月数+1 
                            DateTime dt3 = quarter3.AddMonths(1);
                            //本月最后一天时间 
                            dtEnd = dt3.AddDays(-(quarter3.Day));//结束时间;
                            break;

                        case "4":
                            DateTime quarter4 = Convert.ToDateTime(quarterYearEnd + "/12");//将本月月数+1 
                            DateTime dt4 = quarter4.AddMonths(1);
                            //本月最后一天时间 
                            dtEnd = dt4.AddDays(-(quarter4.Day));//结束时间;
                            break;

                    }

                }
                else if (rbtnlType.SelectedValue == "year")
                {
                    string yearBegin = ddlYearBegin.SelectedValue;
                    string yearEnd = ddlYearEnd.SelectedValue;
                    dtBegion = Convert.ToDateTime(yearBegin + "/01/01");
                    dtEnd = Convert.ToDateTime(yearEnd + "/12/31");
                }

                DataView exStatistical = null;

                string[] portIds = pointCbxRsm.GetPoints().Select(t => t.PointID).ToArray();
                int recordTotal = 0;

                DataView dv = airDataEffectRate.GetPointEffectRateStatisticalData(portIds, dtBegion, dtEnd, int.MaxValue, 0, out recordTotal, out exStatistical, orderBy);

                DataTable dt = UpdateExportColumnName(dv, exStatistical);
                ExcelHelper.DataTableToExcel(dt, "数据有效率统计", "数据有效率统计", this.Page);

            }
        }

        /// <summary>
        /// 修改要导出的数据表中的列名
        /// </summary>
        /// <param name="dv">原始数据表</param>
        /// <param name="dvStatistical">合计行数据表</param>
        /// <returns></returns>
        private DataTable UpdateExportColumnName(DataView dv, DataView dvStatistical)
        {
            DataTable dtOld = dv.ToTable();
            DataTable dtNew = dtOld.Clone();
            dtNew.Columns["PointId"].DataType = typeof(string);
            //DataTable dtNew = dv.ToTable();
            points = pointCbxRsm.GetPoints();

            //整体数据
            for (int i = 0; i < dtOld.Rows.Count; i++)
            {
                DataRow drOld = dtOld.Rows[i];
                DataRow drNew = dtNew.NewRow();
                foreach (DataColumn dcOld in dtOld.Columns)
                {
                    if (!string.IsNullOrWhiteSpace(drOld[dcOld].ToString()))
                    {
                        drNew[dcOld.ColumnName] = drOld[dcOld].ToString().Replace("<br/>", " \r\n");
                    }

                }
                dtNew.Rows.Add(drNew);
            }


            //尾部合计
            if (dvStatistical != null && dvStatistical.Table.Rows.Count > 0)
            {
                DataTable dtStatistical = dvStatistical.Table;

                for (int i = 0; i < dtStatistical.Rows.Count; i++)
                {
                    DataRow drOld = dtStatistical.Rows[i];
                    DataRow drNew = dtNew.NewRow();
                    drNew["PointId"] = "合计";
                    foreach (DataColumn dcOld in dtStatistical.Columns)
                    {
                        if (!string.IsNullOrWhiteSpace(drOld[dcOld].ToString()) && !dcOld.ToString().Equals("Days"))
                        {
                            drNew[dcOld.ColumnName] = drOld[dcOld].ToString().Replace("<br/>", " \r\n");
                        }

                    }
                    dtNew.Rows.Add(drNew);
                }

            }



            for (int i = 0; i < dtNew.Rows.Count; i++)
            {
                DataRow drNew = dtNew.Rows[i];
                drNew["PointId"] = (points.Count(t => t.PointID == drNew["PointId"].ToString()) > 0)
                    ? points.Where(t => t.PointID == drNew["PointId"].ToString()).Select(t => t.PointName).FirstOrDefault()
                    : drNew["PointId"].ToString();
            }
            for (int i = 0; i < dtNew.Columns.Count; i++)
            {
                DataColumn dcNew = dtNew.Columns[i];
                if (dcNew.ColumnName == "PointId")
                {
                    dcNew.ColumnName = "子站名称";
                }

                else if (dcNew.ColumnName == "Days")
                {
                    dcNew.ColumnName = "运行天数";
                }
                else if (dcNew.ColumnName == "ShouldCount")
                {
                    dcNew.ColumnName = "监测数据";
                }
                else if (dcNew.ColumnName == "EffectCount")
                {
                    dcNew.ColumnName = "有效数据";
                }
                else if (dcNew.ColumnName == "EffectRate")
                {
                    dcNew.ColumnName = "数据有效率";
                }

                else
                {
                    dtNew.Columns.Remove(dcNew);
                    i--;
                }
            }
            return dtNew;

        }


        /// <summary>
        /// 数据行绑定处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridEffectRateAnalyze_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                DataRowView drv = e.Item.DataItem as DataRowView;
                if (item["PointId"] != null)
                {
                    GridTableCell pointCell = (GridTableCell)item["PointId"];
                    IPoint point = points.FirstOrDefault(x => x.PointID.Equals(drv["PointId"].ToString().Trim()));
                    if (points != null)
                    {
                        pointCell.Text = string.Format("<a href='javascript:void(0)' onclick='ShowDetails(\"{0}\")'>{1}</a>", drv["PointId"], point.PointName);
                    }
                }
            }
        }

        /// <summary>
        /// Grid生成列事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridEffectRateAnalyze_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            try
            {
                GridBoundColumn col = e.Column as GridBoundColumn;
                if (col == null)
                    return;

                if (col.DataField == "PointId")
                {
                    col.HeaderText = "子站名称";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterText = "合计";
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "Days")
                {
                    int radGridColWidthValue = int.Parse(radGridColWidth.Value.ToString());
                    col.HeaderText = "运行天数";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(radGridColWidthValue);
                    col.ItemStyle.Width = Unit.Pixel(radGridColWidthValue);

                }
                else if (col.DataField == "ShouldCount")
                {
                    int radGridColWidthValue = int.Parse(radGridColWidth.Value.ToString());
                    col.HeaderText = "监测数据";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(radGridColWidthValue);
                    col.ItemStyle.Width = Unit.Pixel(radGridColWidthValue);
                    SetGridFooterText(col, dvStatistical, "ShouldCount");

                }
                else if (col.DataField == "EffectCount")
                {
                    int radGridColWidthValue = int.Parse(radGridColWidth.Value.ToString());
                    col.HeaderText = "有效数据";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(radGridColWidthValue);
                    col.ItemStyle.Width = Unit.Pixel(radGridColWidthValue);
                    SetGridFooterText(col, dvStatistical, "RealCount");

                }
                else if (col.DataField == "EffectRate")
                {
                    int radGridColWidthValue = int.Parse(radGridColWidth.Value.ToString());
                    col.HeaderText = "数据有效率";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(radGridColWidthValue);
                    col.ItemStyle.Width = Unit.Pixel(radGridColWidthValue);
                    SetGridFooterText(col, dvStatistical, "EffectRate");

                }

                //else if (col.DataField == "blankspaceColumn")
                //{
                //    col.HeaderText = string.Empty;
                //}
                else
                {
                    col.Visible = false;
                }
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// 设置列的Footer信息
        /// </summary>
        /// <param name="col">列</param>
        /// <param name="dvStatistical">要绑定的数据</param>
        private void SetGridFooterText(GridBoundColumn col, DataView dvCityStatistical, string Parameter)
        {
            //统计行
            if (dvCityStatistical != null)
            {
                string total = string.Empty;
                total = dvCityStatistical[0][Parameter] != DBNull.Value ? dvCityStatistical[0][Parameter].ToString() : "--";

                col.FooterText = string.Format("{0}", total);
                col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
            }
        }

        #endregion

        #region 选择日期联动周
        /// <summary>
        /// 开始时间联动周
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void weekBegin_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            DateTime weekTimeBegin = weekBegin.SelectedDate.Value;//开始时间
            int year = weekTimeBegin.Year;
            int month = weekTimeBegin.Month;
            int weekCount = ShowWeekSinMonth(year, month);//总共几周
            weekFrom.Items.Clear();
            for (int i = 1; i <= weekCount; i++)
            {
                DropDownListItem cmbItemBegin = new DropDownListItem();
                cmbItemBegin.Text = i.ToString();
                cmbItemBegin.Value = i.ToString();

                weekFrom.Items.Add(cmbItemBegin);
            }
            weekFrom.DataBind();
        }

        /// <summary>
        /// 结束时间联动周
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void weekEnd_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            DateTime weekTimeEnd = weekEnd.SelectedDate.Value;//开始时间
            int year = weekTimeEnd.Year;
            int month = weekTimeEnd.Month;
            int weekCount = ShowWeekSinMonth(year, month);//总共几周
            weekTo.Items.Clear();
            for (int i = 1; i <= weekCount; i++)
            {
                DropDownListItem cmbItemBegin = new DropDownListItem();
                cmbItemBegin.Text = i.ToString();
                cmbItemBegin.Value = i.ToString();

                weekTo.Items.Add(cmbItemBegin);
            }
            weekTo.DataBind();
        }

        #endregion

        #region 计算一个月有几周
        /// <summary>
        /// 计算一个月有几周
        /// </summary>
        /// <param name="y">年</param>
        /// <param name="m">月</param>
        /// <returns></returns>
        private int ShowWeekSinMonth(int y, int m)
        {
            int days = DateTime.DaysInMonth(y, m);
            int weeks = 0;


            DateTime FirstDayOfMonth = DateTime.Parse(y + "-" + m + "-1");

            DateTime FirstDayOfWeek = FirstDayOfMonth;



            while (FirstDayOfWeek.Month == m)
            {
                weeks += 1;

                DateTime LastDayOfWeek;
                if (FirstDayOfWeek.Day + 7 > days)
                {
                    LastDayOfWeek = FirstDayOfWeek.AddDays(days - FirstDayOfWeek.Day);
                }
                else
                {
                    LastDayOfWeek = FirstDayOfWeek.AddDays(7 - getWeekDay(FirstDayOfWeek) - 1);
                }

                FirstDayOfWeek = LastDayOfWeek.AddDays(1);
            }
            return weeks;
        }

        /// <summary>
        /// 获取改周的时间（0：开始周的时间，1结束周的时间）
        /// </summary>
        /// <param name="y">年</param>
        /// <param name="m">月</param>
        /// <param name="type">类型</param>
        /// <param name="w">第几周</param>
        /// <returns></returns>
        private string GetWeek(int y, int m, int type, int w)
        {
            int days = DateTime.DaysInMonth(y, m);
            int weeks = 0;


            DateTime FirstDayOfMonth = DateTime.Parse(y + "-" + m + "-1");

            DateTime FirstDayOfWeek = FirstDayOfMonth;

            while (FirstDayOfWeek.Month == m)
            {
                weeks += 1;
                if (weeks.Equals(w) && type.Equals(0))
                {
                    return FirstDayOfWeek.ToString("yyyy/MM/dd");
                }

                DateTime LastDayOfWeek;
                if (FirstDayOfWeek.Day + 7 > days)
                {
                    LastDayOfWeek = FirstDayOfWeek.AddDays(days - FirstDayOfWeek.Day);
                }
                else
                {
                    LastDayOfWeek = FirstDayOfWeek.AddDays(7 - getWeekDay(FirstDayOfWeek) - 1);
                }
                if (weeks.Equals(w) && type.Equals(1))
                {
                    return LastDayOfWeek.ToString("yyyy/MM/dd");

                }
                FirstDayOfWeek = LastDayOfWeek.AddDays(1);
            }
            return "";

        }

        private int getWeekDay(DateTime d)
        {
            return (d.Day + 2 * d.Month + 3 * (d.Month + 1) / 5 + d.Year + d.Year / 4 - d.Year / 100 + d.Year / 400 + 1) % 7;
        }

        /// <summary>
        /// 获取参与评价AQI的常规6因子
        /// </summary>
        /// <returns></returns>
        private IList<PollutantCodeEntity> GetPollutantListByCalAQI()
        {
            SmartEP.Service.BaseData.Channel.AirPollutantService airPollutantService = new SmartEP.Service.BaseData.Channel.AirPollutantService();
            return airPollutantService.RetrieveListByCalAQI().ToList();
        }

        #endregion






    }
}