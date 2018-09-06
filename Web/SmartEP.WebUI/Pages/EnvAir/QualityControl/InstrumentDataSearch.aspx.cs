using Aspose.Cells;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel;
using SmartEP.Service.Core.Enums;
using SmartEP.Service.Frame;
using SmartEP.Service.OperatingMaintenance.Air;
using SmartEP.Utilities.Web.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.QualityControl
{
    public partial class InstrumentDataSearch : SmartEP.WebUI.Common.BasePage
    {
        //private OfflineSettingBiz g_OfflineBiz = new OfflineSettingBiz();
        //private MonitorDataQueryBiz m_MonitorDataQueryBiz;
        QualityControlDataSearchService dataSearchService = new QualityControlDataSearchService();//
        DictionaryService dicService = new DictionaryService();
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
            string objectType = PageHelper.GetQueryString("ObjectType");
            dayBegin.SelectedDate = DateTime.Now.AddDays(-1);
            dayEnd.SelectedDate = DateTime.Now;
            DataView dvTree = dataSearchService.GetDataBasePager();
            DataView dv = dataSearchService.GetDataPager(objectType);

            InstrumentSN.Items.Add(new ListItem("", ""));
            for (var i = 0; i < dvTree.Count; i++)
            {
                if (dvTree[i]["RowGuid"].ToString() != "")
                    instrumentName.Items.Add(new RadComboBoxItem(dvTree[i]["InstrumentType"].ToString(), dvTree[i]["RowGuid"].ToString()));

                string guid = dvTree[i]["RowGuid"].ToString();

                dv.RowFilter = "Infoguid='" + guid + "'";
                for (var j = 0; j < dv.Count; j++)
                {
                    if (dv[j]["FixedAssetNumber"].ToString() != "")
                        InstrumentSN.Items.Add(new ListItem(dv[j]["InstrumentName"].ToString(), dv[j]["FixedAssetNumber"].ToString()));
                }
            }

            //IQueryable<V_CodeMainItemEntity> integratorEntites = dicService.RetrieveList(DictionaryType.AMS, "运维商");
            Operator.DataSource = dataSearchService.GetDataByOperations();
            Operator.DataTextField = "ItemText";
            Operator.DataValueField = "ItemValue";
            Operator.DataBind();
            for (int i = 0; i < Operator.Items.Count; i++)
            {
                Operator.Items[i].Checked = true;
            }
            //仪器状态信息
            DataView dvState = dataSearchService.GetDataStatePager();
            foreach (DataRowView RowNew in dvState)
            {
                instrumentState.Items.Add(new RadComboBoxItem(RowNew["State"].ToString(), RowNew["RowGuid"].ToString()));
            }
            //for (int i = 0; i < instrumentState.Items.Count; i++)
            //{
            //    instrumentState.Items[i].Checked = true;
            //}
        }
        #endregion

        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {
            //每页显示数据个数            
            int pageSize = gridInstrument.PageSize;
            //当前页的序号
            int currentPageIndex = gridInstrument.CurrentPageIndex + 1;
            //查询记录的开始序号
            int startRecordIndex = pageSize * currentPageIndex;
            //测点
            string[] portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
            //仪器类型
            string SNType = "";
            foreach (RadComboBoxItem item in instrumentName.CheckedItems)
            {
                SNType += (item.Value.ToString() + ",");
            }
            SNType = SNType.Trim(',');
            string[] SN = SNType.Split(',');
            //仪器状态
            string State = "";
            foreach (RadComboBoxItem item in instrumentState.CheckedItems)
            {
                State += (item.Text.ToString() + ",");
            }
            State = State.Trim(',');
            string[] inState = State.Split(',');
            //运维商
            string perator = "";
            foreach (RadComboBoxItem item in Operator.CheckedItems)
            {
                perator += (item.Value.ToString() + ",");
            }
            perator = perator.Trim(',');
            string[] Operators = perator.Split(',');
            //仪器编号
            string instrumentSN = InstrumentSN.SelectedValue;
            DateTime dtBegin = dayBegin.SelectedDate.Value;
            DateTime dtEnd = dayEnd.SelectedDate.Value;
            DataTable dt = new DataTable();
            dt = dataSearchService.GetAllDataPager(portIds, instrumentSN, SN, inState, Operators, dtBegin, dtEnd).Table;

            gridInstrument.DataSource = dt;

            //数据分页的页数
            gridInstrument.VirtualItemCount = dt.Rows.Count;
        }


        #endregion

        #region 服务器端控件事件处理
        /// <summary>
        /// 绑定数据源（用于分页、排序）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridInstrument_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }
        /// <summary>
        /// 数据行绑定处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridInstrument_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            GridDataItem item = e.Item as GridDataItem;
            DataRowView drv = e.Item.DataItem as DataRowView;


            /// <summary>
            /// 选择站点
            /// </summary>
            IList<IPoint> points = null;
            points = pointCbxRsm.GetPoints();

            if (e.Item is GridDataItem)
            {
                if (item["pointName"] != null)
                {
                    GridTableCell pointCell = (GridTableCell)item["pointName"];
                    if (points != null)
                        pointCell.Text = string.Format("<a href='#' onclick='RowClick()'>{0}</a>", pointCell.Text);
                }

            }
        }
        /// <summary>
        /// 查询按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gridInstrument.CurrentPageIndex = 0;
            gridInstrument.Rebind();
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
                DateTime dtBegin = dayBegin.SelectedDate.Value;
                DateTime dtEnd = dayEnd.SelectedDate.Value;
                //测点
                string[] portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
                //仪器类型
                string SNType = "";
                foreach (RadComboBoxItem item in instrumentName.CheckedItems)
                {
                    SNType += (item.Value.ToString() + ",");
                }
                SNType = SNType.Trim(',');
                string[] SN = SNType.Split(',');
                //仪器状态
                string State = "";
                foreach (RadComboBoxItem item in instrumentState.CheckedItems)
                {
                    State += (item.Text.ToString() + ",");
                }
                State = State.Trim(',');
                string[] inState = State.Split(',');
                //运维商
                string perator = "";
                foreach (RadComboBoxItem item in Operator.CheckedItems)
                {
                    perator += (item.Value.ToString() + ",");
                }
                perator = perator.Trim(',');
                string[] Operators = perator.Split(',');
                //仪器编号
                string instrumentSN = InstrumentSN.SelectedValue;
                DataTable dt = new DataTable();
                dt = dataSearchService.GetAllDataPager(portIds, instrumentSN, SN, inState, Operators, dtBegin, dtEnd).Table;
                DataTableToExcel(dt.DefaultView, "仪器信息查询", "仪器信息查询");
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
            //第一行
            cells[0, 0].PutValue("仪器类型");
            cells.Merge(0, 0, 1, 1);
            cells[0, 1].PutValue("仪器编号");
            cells.Merge(0, 1, 1, 1);
            cells[0, 2].PutValue("仪器型号");
            cells.Merge(0, 2, 1, 1);
            cells[0, 3].PutValue("测点");
            cells.Merge(0, 3, 1, 1);
            cells[0, 4].PutValue("仪器状态");
            cells.Merge(0, 4, 1, 1);
            cells[0, 5].PutValue("购置时间");
            cells.Merge(0, 5, 1, 1);
            cells[0, 6].PutValue("登记时间");
            cells.Merge(0, 6, 1, 1);
            cells[0, 7].PutValue("最后检定时间");
            cells.Merge(0, 7, 1, 1);
            cells[0, 8].PutValue("最近维护时间");
            cells.Merge(0, 8, 1, 1);
            cells[0, 9].PutValue("下次鉴定时间");
            cells.Merge(0, 9, 1, 1);
            cells[0, 10].PutValue("品牌");
            cells.Merge(0, 10, 1, 1);
            cells[0, 11].PutValue("制造商");
            cells.Merge(0, 11, 1, 1);
            cells[0, 12].PutValue("维护商");
            cells.Merge(0, 12, 1, 1);
            cells[0, 13].PutValue("维护联系人/电话");
            cells.Merge(0, 13, 1, 1);

            cells.SetRowHeight(0, 20);//设置行高
            for (int i = 0; i <= 13; i++)
            {
                cells.SetColumnWidth(i, 15);//设置列宽
            }

            for (int i = 0; i < dtNew.Rows.Count; i++)
            {
                DataRow drNew = dtNew.Rows[i];
                int rowIndex = i + 1;
                cells[rowIndex, 0].PutValue(drNew["InstrumentType"].ToString());
                cells[rowIndex, 1].PutValue(drNew["InstrumentName"].ToString());
                cells[rowIndex, 2].PutValue(drNew["SpecificationModel"].ToString());
                cells[rowIndex, 3].PutValue(drNew["pointName"].ToString());
                cells[rowIndex, 4].PutValue(drNew["State"].ToString());
                cells[rowIndex, 5].PutValue(string.Format("{0:yyyy-MM-dd}", drNew["BuyDate"]));
                cells[rowIndex, 6].PutValue(string.Format("{0:yyyy-MM-dd}", drNew["RegistrationDate"]));
                cells[rowIndex, 7].PutValue(string.Format("{0:yyyy-MM-dd}", drNew["LastDate"]));
                cells[rowIndex, 8].PutValue(string.Format("{0:yyyy-MM-dd}", drNew["RecentDate"]));
                cells[rowIndex, 9].PutValue(string.Format("{0:yyyy-MM-dd}", drNew["NextDate"]));
                cells[rowIndex, 10].PutValue(drNew["Brand"].ToString());
                cells[rowIndex, 11].PutValue(drNew["ManufacturerName"].ToString());
                cells[rowIndex, 12].PutValue(drNew["TeamName"].ToString());
                cells[rowIndex, 13].PutValue(drNew["UserInfo"].ToString());
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

        #endregion

        protected void instrumentName_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {

        }
    }
}