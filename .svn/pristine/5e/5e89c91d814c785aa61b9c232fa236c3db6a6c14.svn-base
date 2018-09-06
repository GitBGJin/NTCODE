using SmartEP.DomainModel;
using SmartEP.Service.BaseData.BusinessRule;
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
using SmartEP.Core.Enums;
using SmartEP.Service.DataAnalyze.Air.AQIReport;
using SmartEP.Core.Generic;
using SmartEP.Service.Frame;
using SmartEP.Service.Core.Enums;
using SmartEP.Service.BaseData.MPInfo;
using System.Text.RegularExpressions;
using SmartEP.Core.Interfaces;
using Aspose.Cells;
namespace SmartEP.WebUI.Pages.EnvAir.DataAnalyze
{
    /// <summary>
    /// 名称：PollutantSituationReport.aspx.cs
    /// 创建人：樊垂贺
    /// 创建日期：2015-08-20
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：污染持续天数及污染程度简表
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class PollutantSituationReport : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        DayAQIService m_DayAQIService = Singleton<DayAQIService>.GetInstance();
        DictionaryService dicService = new DictionaryService();
        IQueryable<V_CodeDictionaryEntity> cityTypeEntites = null;
        MonitoringPointAirService pointAirService = new MonitoringPointAirService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitControl();
                lastText.Visible = false;
                lastingDays.Visible = false;
                dvPoints.Style["display"] = "none";
                rcbCityProper.Visible = true;
                textClass.Style["display"] = "normal";
                textFactor.Style["display"] = "none";
                dvClass.Style["display"] = "normal";
                dvFactor.Style["display"] = "none";
            }
        }
        
        public void GroupName(int col1,int col2)
        {
          
            int count = 0;
            if (grdSituation.Items.Count > 0)
            {
              
                if (grdSituation.Columns.Count > col2)
                {
                  var OldRegionName = grdSituation.Items[count][grdSituation.Columns[col1]];

                  var OldDateName = grdSituation.Items[count][grdSituation.Columns[col2]];
                  for (int i = count + 1; i < grdSituation.Items.Count; i++)
                  {
                    var NewRegionName = grdSituation.Items[i][grdSituation.Columns[col1]];
                    var NewDateName = grdSituation.Items[i][grdSituation.Columns[col2]];
                    if (OldRegionName.Text == NewRegionName.Text && OldDateName.Text == NewDateName.Text)
                    {
                      NewRegionName.Visible = false;
                      if (OldRegionName.RowSpan == 0 || OldDateName.RowSpan == 0)
                      {
                        OldRegionName.RowSpan = 1;
                        OldDateName.RowSpan = 1;
                      }
                      OldRegionName.RowSpan++;
                      OldDateName.RowSpan++;
                      OldRegionName.VerticalAlign = VerticalAlign.Middle;
                      OldDateName.VerticalAlign = VerticalAlign.Middle;
                    }
                    else
                    {
                      count = i;
                      break;
                    }
                  }
                }
              }
            
          
         
        }
        #region 页面控件初始化
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            dtpBegin.SelectedDate = DateTime.Now.AddMonths(-1);
            dtpEnd.SelectedDate = DateTime.Now;
        }
        #endregion

        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {
          try
          {
            cityTypeEntites = dicService.RetrieveRegionList(CityType.SuZhou);
            string[] portIds = null;

            portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
            string regionGuid = "";
            string classDic = "";
            string factorDic = "";
            Dictionary<string, string> regionName = new Dictionary<string, string>();
            Dictionary<string, string> className = new Dictionary<string, string>();
          
            foreach (RadComboBoxItem item in rcbFactors.CheckedItems)
            {
              classDic += (item.Value.ToString() + ",");
              className.Add(item.Value.ToString(), item.Text.ToString());
            }
            string[] classDics = classDic.Trim(',').Split(',');
            foreach (RadComboBoxItem item in rcbFactor.CheckedItems)
            {
              factorDic += (item.Value.ToString() + ",");
            }
            string[] factorDics = factorDic.Trim(',').Split(',');
            DateTime dtBegion = dtpBegin.SelectedDate.Value;
            //dtBegion = new DateTime(dtBegion.Year, dtBegion.Month, 1);
            DateTime dtEnd = dtpEnd.SelectedDate.Value;
            //DateTime dtEndA = new DateTime(dtEnd.Year, dtEnd.Month, 1);
            dtBegion = Convert.ToDateTime(dtBegion.FormatToString("yyyy-MM-dd 00:00:00"));
            dtEnd = Convert.ToDateTime(dtEnd.FormatToString("yyyy-MM-dd 23:59:59"));
            //var dataView = new DataView();// g_OfflineBiz.GetGridViewPager(pageSize, currentPageIndex, GetWhereString(), out recordTotal);
            //if (dataView == null)
            //{
            //    dataView = new DataView();
            ////}

            string lastDays = lastingDays.Text;
          
            
            
              string TotalType = rdlType.SelectedValue;
              if (rbtnlType.SelectedValue == "CityProper")
              {
                foreach (RadComboBoxItem item in rcbCityProper.CheckedItems)
                {
                  regionGuid += (item.Value.ToString() + ",");
                  regionName.Add(item.Value.ToString(), item.Text.ToString());
                }
                string[] regionGuids = regionGuid.Trim(',').Split(',');
                DataTable dtClass = m_DayAQIService.GetAreaPolluateClassList(regionGuids, classDics, factorDics,dtBegion, dtEnd, TotalType, regionName, className,lastDays);
                grdSituation.DataSource = dtClass;
                grdSituation.VirtualItemCount = dtClass.Rows.Count;
              }
              else if (rbtnlType.SelectedValue == "Port")
              {

                DataTable dtClass = m_DayAQIService.GetPortPolluateClassList(portIds, classDics,factorDics, dtBegion, dtEnd, TotalType, className,lastDays);
                grdSituation.DataSource = dtClass;
                grdSituation.VirtualItemCount = dtClass.Rows.Count;
              }
              /* var analyzeDate = new DataView();
               if (portIds != null)
               {
                   analyzeDate = m_DayAQIService.GetPortsContinuousDaysTable(portIds, dtBegion, dtEnd);
               }
               if (analyzeDate != null)
               {
                   grdSituation.DataSource = analyzeDate;//dataView;
                   grdSituation.VirtualItemCount = analyzeDate.Count;
               }
               else
               {
                   grdSituation.DataSource = new DataTable();
               }*/
            
          }
          catch (Exception e)
          {

          }
        }

        #endregion

        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            grdSituation.Rebind();
        }

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

         
          GridDataItem item = e.Item as GridDataItem;
          DataRowView drv = e.Item.DataItem as DataRowView;
          switch (rbtnlType.SelectedValue)
          {
            case "Port":
              /// <summary>
              /// 选择站点
              /// </summary>
              IList<IPoint> points = null;
              points = pointCbxRsm.GetPoints();

              if (e.Item is GridDataItem)
              {
                if (item["PointName"] != null)
                {
                  GridTableCell pointCell = (GridTableCell)item["PointName"];
                  IPoint point = points.FirstOrDefault(x => x.PointID.Equals(pointCell.Text.Trim()));
                  if (points != null)
                    pointCell.Text = point.PointName;
                }

              }
              break;
            
          }
        }


        /// <summary>
        /// ToolBar事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridRTB_ButtonClick(object sender, RadToolBarEventArgs e)
        {
            Telerik.Web.UI.RadToolBarButton button = (Telerik.Web.UI.RadToolBarButton)e.Item;
            cityTypeEntites = dicService.RetrieveRegionList(CityType.SuZhou);
            string[] portIds = null;

            portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
            string regionGuid = "";
            string classDic = "";
            string factorDic = "";
            Dictionary<string, string> regionName = new Dictionary<string, string>();
            Dictionary<string, string> className = new Dictionary<string, string>();

            foreach (RadComboBoxItem item in rcbFactors.CheckedItems)
            {
              classDic += (item.Value.ToString() + ",");
              className.Add(item.Value.ToString(), item.Text.ToString());
            }
            string[] classDics = classDic.Trim(',').Split(',');
            foreach (RadComboBoxItem item in rcbFactor.CheckedItems)
            {
              factorDic += (item.Value.ToString() + ",");
            }
            string[] factorDics = factorDic.Trim(',').Split(',');
            DateTime dtBegion = dtpBegin.SelectedDate.Value;
            //dtBegion = new DateTime(dtBegion.Year, dtBegion.Month, 1);
            DateTime dtEnd = dtpEnd.SelectedDate.Value;
            //DateTime dtEndA = new DateTime(dtEnd.Year, dtEnd.Month, 1);
            dtBegion = Convert.ToDateTime(dtBegion.FormatToString("yyyy-MM-dd 00:00:00"));
            dtEnd = Convert.ToDateTime(dtEnd.FormatToString("yyyy-MM-dd 23:59:59"));
            //var dataView = new DataView();// g_OfflineBiz.GetGridViewPager(pageSize, currentPageIndex, GetWhereString(), out recordTotal);
            //if (dataView == null)
            //{
            //    dataView = new DataView();
            ////}

            string lastDays = lastingDays.Text;
            string TotalType = rdlType.SelectedValue;
          //每页显示数据个数            
            int pageSize = int.MaxValue;
           //当前页的序号
            int currentPageIndex = 0;
            if (button.CommandName == "ExportToExcel")
            {
              if (rbtnlType.SelectedValue == "CityProper")
              {
                foreach (RadComboBoxItem item in rcbCityProper.CheckedItems)
                {
                  regionGuid += (item.Value.ToString() + ",");
                  regionName.Add(item.Value.ToString(), item.Text.ToString());
                }
                string[] regionGuids = regionGuid.Trim(',').Split(',');
                DataTable dtClass = m_DayAQIService.GetAreaPolluateClassList(regionGuids, classDics, factorDics, dtBegion, dtEnd, TotalType, regionName, className, lastDays);
                dtClass.Columns["RegionName"].SetOrdinal(0);
                if (TotalType == "1")
                {
                 
                 
                  DataTableToExcel(dtClass, "污染情况统计表(区域)", "质量类别",TotalType,rbtnlType.SelectedValue );
                }
                if (TotalType == "2")
                {
                  
                  DataTableToExcel(dtClass, "污染情况统计表(区域)", "污染持续",TotalType,rbtnlType.SelectedValue );
                }
                if (TotalType == "3")
                {
                 
                  DataTableToExcel(dtClass, "污染情况统计表(区域)", "首要污染物", TotalType, rbtnlType.SelectedValue);
                }
              }
              else if (rbtnlType.SelectedValue == "Port")
              {

                DataTable dtClass = m_DayAQIService.GetPortPolluateClassList(portIds, classDics, factorDics, dtBegion, dtEnd, TotalType, className, lastDays);
                foreach (DataRow dr in dtClass.Rows)
                {
                  if (dr["PointName"] != DBNull.Value)
                  {
                    string pointid = dr["PointName"].ToString();
                    IPoint point = pointCbxRsm.GetPoints().FirstOrDefault(x => x.PointID.Equals(pointid));
                    dr["PointName"] = point.PointName;
                  }
                }
                dtClass.Columns["PointName"].SetOrdinal(0);
                if (TotalType == "1")
                {

                  
                  DataTableToExcel(dtClass, "污染情况统计表(测点)", "质量类别", TotalType, rbtnlType.SelectedValue);
                }
                if (TotalType == "2")
                {
                  
                DataTableToExcel(dtClass, "污染情况统计表(测点)", "污染持续", TotalType, rbtnlType.SelectedValue);
                }
                if (TotalType == "3")
                {
                 
                  DataTableToExcel(dtClass, "污染情况统计表(测点)", "首要污染物", TotalType, rbtnlType.SelectedValue);
                }
              }
              
               /* string[] portIds = null;

                portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);


                DateTime dtBegion = dtpBegin.SelectedDate.Value;
                dtBegion = new DateTime(dtBegion.Year, dtBegion.Month, 1);
                DateTime dtEnd = dtpEnd.SelectedDate.Value;
                DateTime dtEndA = new DateTime(dtEnd.Year, dtEnd.Month, 1);
                dtEnd = Convert.ToDateTime(dtEndA.AddMonths(1).AddDays(-1).FormatToString("yyyy-MM-dd 23:59:59"));
                var analyzeDate = m_DayAQIService.GetPortsContinuousDaysTable(portIds, dtBegion, dtEnd);
                DataTable dtNew = analyzeDate.ToTable();
                dtNew.Columns["DateTime"].ColumnName = "日期";
                dtNew.Columns["ContinuousDays"].ColumnName = "持续天数";
                dtNew.Columns["LightPollution"].ColumnName = "轻度污染";
                dtNew.Columns["ModeratePollution"].ColumnName = "中度污染";
                dtNew.Columns["HighPollution"].ColumnName = "重度污染";
                dtNew.Columns["SeriousPollution"].ColumnName = "严重污染";
                dtNew.Columns.Remove("LightPollutionDay");
                dtNew.Columns.Remove("ModeratePollutionDay");
                dtNew.Columns.Remove("HighPollutionDay");
                dtNew.Columns.Remove("SeriousPollutionDay");
                ExcelHelper.DataTableToExcel(dtNew, "污染程度持续简表", "污染程度持续简表", this.Page);*/
            }
        }
      /// <summary>
      /// 根据不同的类型创建不同的EXCEL表格并导出
      /// </summary>
      /// <param name="dt"></param>
      /// <param name="fileName"></param>
      /// <param name="sheetName"></param>
      /// <param name="totalType"></param>
      /// <param name="type"></param>
        private void DataTableToExcel(DataTable dt, string fileName, string sheetName, string totalType, string type)
        {
          DataTable dtNew = dt;
          Workbook workbook = new Workbook();
          Worksheet sheet = workbook.Worksheets[0];
          Cells cells = sheet.Cells;
          Aspose.Cells.Style cellStyle = workbook.Styles[workbook.Styles.Add()];
          Aspose.Cells.Style cellStyle1 = workbook.Styles[workbook.Styles.Add()];
          cellStyle1.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Left; // 设置居中 
          cellStyle1.Font.Name = "宋体"; //文字字体
          cellStyle1.Font.Size = 12;//文字大小
          cellStyle1.IsLocked = false; //单元格解锁
          cellStyle1.IsTextWrapped = true; //单元格内容自动换行
          cellStyle1.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
          //应用边界线 右边界线
          cellStyle1.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
          //应用边界线 上边界线
          cellStyle1.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
          //应用边界线 下边界线
          cellStyle1.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
          workbook.FileName = fileName;
          sheet.Name = sheetName;
          cellStyle.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center; // 设置居中 
          cellStyle.Font.Name = "宋体"; //文字字体
          cellStyle.Font.Size = 12;//文字大小
          cellStyle.IsLocked = false; //单元格解锁
          cellStyle.IsTextWrapped = true; //单元格内容自动换行
          cellStyle.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
          //应用边界线 右边界线
          cellStyle.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
          //应用边界线 上边界线
          cellStyle.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
          //应用边界线 下边界线
          cellStyle.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
          if (type == "CityProper")
          {

            if (totalType == "1")
            {
              cells.Clear();
              
              cells[0, 0].PutValue("区域");
              cells[0, 1].PutValue("类别");
              cells[0, 2].PutValue("天数");
              cells[0, 3].PutValue("具体天(日期：AQI;)");
              for (int i = 0; i < dtNew.Rows.Count; i++)
              {
                DataRow drNew = dtNew.Rows[i];
                int rowIndex = i + 1;
                cells[rowIndex, 0].PutValue(drNew["RegionName"].ToString());
                cells[rowIndex, 1].PutValue(drNew["ClassName"].ToString());
                cells[rowIndex, 2].PutValue(drNew["Days"].ToString());
                cells[rowIndex, 3].PutValue(drNew["SpecficDay"].ToString().Replace("<br />", "\r\n"));
                cells[rowIndex, 3].SetStyle(cellStyle1);

              }
              cells.SetRowHeight(0, 20);//设置行高
              cells.SetColumnWidth(0, 20);//设置列宽
              cells.SetColumnWidth(1, 20);//设置列宽
              cells.SetColumnWidth(2, 10);//设置列宽
              cells.SetColumnWidth(3, 180);//设置列宽

            }
            if (totalType == "2")
            {
              cells.Clear();
              cells[0, 0].PutValue("区域");
              cells[0, 1].PutValue("日期");
              cells[0, 2].PutValue("持续天数");
              cells[0, 3].PutValue("类别");
              cells[0, 4].PutValue("天数");
              cells[0, 5].PutValue("具体天(日期:AQI)");

              for (int i = 0; i < dtNew.Rows.Count; i++)
              {
                DataRow drNew = dtNew.Rows[i];
                int rowIndex = i + 1;

                cells[rowIndex, 0].PutValue(drNew["RegionName"].ToString());
                cells[rowIndex, 1].PutValue(drNew["DateTime"].ToString());
                cells[rowIndex, 2].PutValue(drNew["LastDays"].ToString());
                cells[rowIndex, 3].PutValue(drNew["ClassName"].ToString());
                cells[rowIndex, 4].PutValue(drNew["Days"].ToString());
                cells[rowIndex, 5].PutValue(drNew["SpecficDay"].ToString().Replace("<br />", "\r\n"));
                cells[rowIndex, 5].SetStyle(cellStyle1);
              }

              /* int indexRow = 1;
               int init = 1;
               while (indexRow < cells.Rows.Count)
               {
                 int count = 1;
                 int j = init;
              
                 while (indexRow< cells.Rows.Count)
                 {
                 
                 
                   if (cells[j,0].StringValue == cells[j+1, 0].StringValue && cells[j, 1].StringValue == cells[j+1, 1].StringValue)
                   {
                    
                     count++;
                     indexRow++;
                     j++;
                   }
                   else if(count>1)
                   {
                     string region=cells[init,0].StringValue;
                     string date=cells[init,1].StringValue;
                     string days=cells[init,2].StringValue;
                     cells.Merge(init, 0, count, 1);
                     cells.Merge(init, 1, count, 1);
                     cells.Merge(init, 2, count, 1);
                     cells[init, 0].PutValue(region);
                     cells[init, 1].PutValue(date);
                     cells[init, 2].PutValue(days);
                     init++;
                     break;
                   }
                  
                 }
               }*/
              cells.SetRowHeight(0, 20);//设置行高
              cells.SetColumnWidth(0, 20);//设置列宽
              cells.SetColumnWidth(1, 35);//设置列宽
              cells.SetColumnWidth(2, 10);//设置列宽
              cells.SetColumnWidth(3, 20);//设置列宽
              cells.SetColumnWidth(4, 10);//设置列宽
              cells.SetColumnWidth(5, 150);//设置列宽
            }
            if (totalType == "3")
            {
              cells.Clear();
              cells[0, 0].PutValue("区域");
              cells[0, 1].PutValue("首要污染物");
              cells[0, 2].PutValue("天数");
              cells[0, 3].PutValue("占比例(%)");
              cells[0, 4].PutValue("多个首要污染物具体天数(日期：AQI)");
              for (int i = 0; i < dtNew.Rows.Count; i++)
              {
                DataRow drNew = dtNew.Rows[i];
                int rowIndex = i + 1;
                cells[rowIndex, 0].PutValue(drNew["RegionName"].ToString());
                cells[rowIndex, 1].PutValue(drNew["PolluteName"].ToString());
                cells[rowIndex, 2].PutValue(drNew["Days"].ToString());
                cells[rowIndex, 3].PutValue(drNew["Rates"].ToString());
                cells[rowIndex, 4].PutValue(drNew["MoreSpecficDay"].ToString().Replace("<br />", "\r\n"));
                cells[rowIndex, 4].SetStyle(cellStyle1);
              }
              cells.SetRowHeight(0, 20);//设置行高
              cells.SetColumnWidth(0, 20);//设置列宽
              cells.SetColumnWidth(1, 20);//设置列宽
              cells.SetColumnWidth(2, 20);//设置列宽
              cells.SetColumnWidth(3, 20);//设置列宽
              cells.SetColumnWidth(4, 180);//设置列宽
            }

          }
          else
          {
            if (totalType == "1")
            {
              cells.Clear();
              
              cells[0, 0].PutValue("测点");
              cells[0, 1].PutValue("类别");
              cells[0, 2].PutValue("天数");
              cells[0, 3].PutValue("具体天(日期：AQI;)");
              for (int i = 0; i < dtNew.Rows.Count; i++)
              {
                DataRow drNew = dtNew.Rows[i];
                int rowIndex = i + 1;
                cells[rowIndex, 0].PutValue(drNew["PointName"].ToString());
                cells[rowIndex, 1].PutValue(drNew["ClassName"].ToString());
                cells[rowIndex, 2].PutValue(drNew["Days"].ToString());
                cells[rowIndex, 3].PutValue(drNew["SpecficDay"].ToString().Replace("<br />", "\r\n"));
                cells[rowIndex, 3].SetStyle(cellStyle1);

              }
              cells.SetRowHeight(0, 20);//设置行高
              cells.SetColumnWidth(0, 20);//设置列宽
              cells.SetColumnWidth(1, 20);//设置列宽
              cells.SetColumnWidth(2, 10);//设置列宽
              cells.SetColumnWidth(3, 180);//设置列宽

            }
            if (totalType == "2")
            {
              cells.Clear();
              cells[0, 0].PutValue("测点");
              cells[0, 1].PutValue("日期");
              cells[0, 2].PutValue("持续天数");
              cells[0, 3].PutValue("类别");
              cells[0, 4].PutValue("天数");
              cells[0, 5].PutValue("具体天(日期:AQI)");

              for (int i = 0; i < dtNew.Rows.Count; i++)
              {
                DataRow drNew = dtNew.Rows[i];
                int rowIndex = i + 1;

                cells[rowIndex, 0].PutValue(drNew["PointName"].ToString());
                cells[rowIndex, 1].PutValue(drNew["DateTime"].ToString());
                cells[rowIndex, 2].PutValue(drNew["LastDays"].ToString());
                cells[rowIndex, 3].PutValue(drNew["ClassName"].ToString());
                cells[rowIndex, 4].PutValue(drNew["Days"].ToString());
                cells[rowIndex, 5].PutValue(drNew["SpecficDay"].ToString().Replace("<br />", "\r\n"));
                cells[rowIndex, 5].SetStyle(cellStyle1);
              }

              /* int indexRow = 1;
               int init = 1;
               while (indexRow < cells.Rows.Count)
               {
                 int count = 1;
                 int j = init;
              
                 while (indexRow< cells.Rows.Count)
                 {
                 
                 
                   if (cells[j,0].StringValue == cells[j+1, 0].StringValue && cells[j, 1].StringValue == cells[j+1, 1].StringValue)
                   {
                    
                     count++;
                     indexRow++;
                     j++;
                   }
                   else if(count>1)
                   {
                     string region=cells[init,0].StringValue;
                     string date=cells[init,1].StringValue;
                     string days=cells[init,2].StringValue;
                     cells.Merge(init, 0, count, 1);
                     cells.Merge(init, 1, count, 1);
                     cells.Merge(init, 2, count, 1);
                     cells[init, 0].PutValue(region);
                     cells[init, 1].PutValue(date);
                     cells[init, 2].PutValue(days);
                     init++;
                     break;
                   }
                  
                 }
               }*/
              cells.SetRowHeight(0, 20);//设置行高
              cells.SetColumnWidth(0, 20);//设置列宽
              cells.SetColumnWidth(1, 35);//设置列宽
              cells.SetColumnWidth(2, 10);//设置列宽
              cells.SetColumnWidth(3, 20);//设置列宽
              cells.SetColumnWidth(4, 10);//设置列宽
              cells.SetColumnWidth(5, 150);//设置列宽
            }
            if (totalType == "3")
            {
              cells.Clear();
              cells[0, 0].PutValue("测点");
              cells[0, 1].PutValue("首要污染物");
              cells[0, 2].PutValue("天数");
              cells[0, 3].PutValue("占比例(%)");
              cells[0, 4].PutValue("多个首要污染物具体天数(日期：AQI)");
              for (int i = 0; i < dtNew.Rows.Count; i++)
              {
                DataRow drNew = dtNew.Rows[i];
                int rowIndex = i + 1;
                cells[rowIndex, 0].PutValue(drNew["PointName"].ToString());
                cells[rowIndex, 1].PutValue(drNew["PolluteName"].ToString());
                cells[rowIndex, 2].PutValue(drNew["Days"].ToString());
                cells[rowIndex, 3].PutValue(drNew["Rates"].ToString());
                cells[rowIndex, 4].PutValue(drNew["MoreSpecficDay"].ToString().Replace("<br />", "\r\n"));
                cells[rowIndex, 4].SetStyle(cellStyle1);
              }
              cells.SetRowHeight(0, 20);//设置行高
              cells.SetColumnWidth(0, 20);//设置列宽
              cells.SetColumnWidth(1, 20);//设置列宽
              cells.SetColumnWidth(2, 20);//设置列宽
              cells.SetColumnWidth(3, 20);//设置列宽
              cells.SetColumnWidth(4, 180);//设置列宽

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
          Response.AppendHeader("Content-Disposition", string.Format("attachment;filename={0}.xls", HttpUtility.UrlEncode(fileName)));
          Response.ContentEncoding = System.Text.Encoding.UTF8;
          Response.ContentType = "application/ms-excel";
          Response.BinaryWrite(workbook.SaveToStream().ToArray());
          Response.End();
        }

        private DataTable UpdateExportColumnName(DataTable dtClass)
        {
          DataTable dtNew = dtClass;
          for (int i = 0; i < dtNew.Columns.Count; i++)
          {
            if (dtNew.Columns[i].ColumnName == "RegionName")
            {
              
                dtNew.Columns[i].ColumnName = "区域";
            }
            else if(dtNew.Columns[i].ColumnName=="PointName")
            {
                dtNew.Columns[i].ColumnName = "测点";
            }
            
            else if (dtNew.Columns[i].ColumnName == "ClassName")
            {
              dtNew.Columns[i].ColumnName = "类别";
            }
            else if (dtNew.Columns[i].ColumnName == "Days")
            {
              dtNew.Columns[i].ColumnName = "天数";
            }
            else if (dtNew.Columns[i].ColumnName == "SpecficDay")
            {
              dtNew.Columns[i].ColumnName = "具体天(日期：AQI;)";
            }
            else if (dtNew.Columns[i].ColumnName == "LastDays")
            {
              dtNew.Columns[i].ColumnName = "持续天数";
            }
           
            else if (dtNew.Columns[i].ColumnName == "Rate")
            {
              dtNew.Columns[i].ColumnName = "占比例(%)";
            }
            else if (dtNew.Columns[i].ColumnName == "PolluteName")
            {
              dtNew.Columns[i].ColumnName = "首要污染物";
            }
            else if (dtNew.Columns[i].ColumnName == "MoreSpecficDay")
            {
              dtNew.Columns[i].ColumnName = "多个首要污染物具体天数";
            }
           
            else
            {
              dtNew.Columns.Remove(dtNew.Columns[i].ColumnName);
              i--;
            }
          }
          return dtNew;
        }

        #endregion
        #region 查询类型的切换事件
        protected void rbtnlType_SelectedIndexChanged(object sender, EventArgs e)
        {
         
          dvPoints.Style["display"] = "none";
          rcbCityProper.Visible = false;
          switch (rbtnlType.SelectedValue)
          {
            case "Port":
              dvPoints.Style["display"] = "normal";
              break;
            case "CityProper":
              rcbCityProper.Visible = true;
              break;
          }
        }
        #endregion
        #region 统计类型切换的事件
        protected void rdlType_SelectedIndexChanged(object sender, EventArgs e)
        {
          if (rdlType.SelectedValue == "1")
          {
            lastText.Visible = false;
            lastingDays.Visible = false;
            lastingDays.Text = "2";
            textClass.Style["display"] = "normal";
            textFactor.Style["display"] = "none";
            dvClass.Style["display"] = "normal";
            dvFactor.Style["display"] = "none";
          
          }
          else if (rdlType.SelectedValue == "2")
          {

            lastText.Visible = true;
            lastingDays.Visible = true;
            lastingDays.Text = "2";
            textClass.Style["display"] = "none";
            textFactor.Style["display"] = "none";
            dvClass.Style["display"] = "none";
            dvFactor.Style["display"] = "none";
          }
          else
          {
            lastText.Visible = false;
            lastingDays.Visible = false;
            lastingDays.Text = "2";
            textClass.Style["display"] = "none";
            textFactor.Style["display"] = "normal";
            dvClass.Style["display"] = "none";
            dvFactor.Style["display"] = "normal";
          }
        }
        #endregion
     /// <summary>
     /// 
     /// 列名称绑定
     /// </summary>
     /// <param name="sender"></param>
     /// <param name="e"></param>
        protected void grdSituation_ColumnCreated(object sender, GridColumnCreatedEventArgs e) 
        {
          try
          {
            GridBoundColumn col = e.Column as GridBoundColumn;
            if (col == null)
            {
              return;
            }
            if (col.DataField == "RegionName")
            {
              col.HeaderText = "区域";
              col.EmptyDataText = "--";
              col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
              col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
              col.HeaderStyle.Width = Unit.Pixel(80);
              col.ItemStyle.Width = Unit.Pixel(80);
            }
            else if (col.DataField == "PointName")
            {
              col.HeaderText = "测点";
              col.EmptyDataText = "--";
              col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
              col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
              col.HeaderStyle.Width = Unit.Pixel(80);
              col.ItemStyle.Width = Unit.Pixel(80);
            }
            if (col.DataField =="ClassName")
            {
              col.HeaderText = "类别";
              col.EmptyDataText = "--";
              col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
              col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
              col.HeaderStyle.Width = Unit.Pixel(70);
              col.ItemStyle.Width = Unit.Pixel(70);
            }
            if (col.DataField == "DateTime")
            {
              col.HeaderText = "日期";
              col.EmptyDataText = "--";
              col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
              col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
              col.HeaderStyle.Width = Unit.Pixel(150);
              col.ItemStyle.Width = Unit.Pixel(150);
            }
            if (col.DataField == "LastDays")
            {
              col.HeaderText = "持续天数";
              col.EmptyDataText = "--";
              col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
              col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
              col.HeaderStyle.Width = Unit.Pixel(60);
              col.ItemStyle.Width = Unit.Pixel(60);
            }
            if (col.DataField == "PolluteName")
            {
              col.HeaderText = "首要污染物";
              col.EmptyDataText = "--";
              col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
              col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
              col.HeaderStyle.Width = Unit.Pixel(80);
              col.ItemStyle.Width = Unit.Pixel(80);
            }
            if (col.DataField == "Days")
            {
              col.HeaderText = "天数";
              col.EmptyDataText = "--";
              col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
              col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
              col.HeaderStyle.Width = Unit.Pixel(60);
              col.ItemStyle.Width = Unit.Pixel(60);
            }
            if (col.DataField == "Rates")
            {
              col.HeaderText = "占比例(%)";
              col.EmptyDataText = "--";
              col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
              col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
              col.HeaderStyle.Width = Unit.Pixel(60);
              col.ItemStyle.Width = Unit.Pixel(60);
            }
            if (col.DataField == "SpecficDay")
            {
              col.HeaderText = "具体天(日期：AQI值；)";
              col.EmptyDataText = "--";
              col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
              col.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
              col.HeaderStyle.Width = Unit.Pixel(700);
              col.ItemStyle.Width = Unit.Pixel(700);
            }
            if (col.DataField == "MoreSpecficDay")
            {
              col.HeaderText = "多个首要污染物具体天数(日期：AQI值；)";
              col.EmptyDataText = "--";
              col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
              col.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
              col.HeaderStyle.Width = Unit.Pixel(700);
              col.ItemStyle.Width = Unit.Pixel(600);
            }

          }
          catch (Exception ex)
          {
          }
        }


    }
}