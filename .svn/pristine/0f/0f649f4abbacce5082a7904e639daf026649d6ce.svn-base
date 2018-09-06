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
using SmartEP.Service.DataAnalyze.Common;
using SmartEP.Core.Generic;
using SmartEP.Service.Frame;
using SmartEP.Service.Core.Enums;

namespace SmartEP.WebUI.Pages.EnvAir.DataAnalyze
{
    public partial class SystemLogSearch : BasePage
    {
        /// <summary>
        /// 名称：SystemLogSearch.aspx.cs
        /// 创建人：樊垂贺
        /// 创建日期：2015-08-19
        /// 维护人员：
        /// 最新维护人员：
        /// 最新维护日期：
        /// 功能摘要：系统日志查询
        /// 版权所有(C)：江苏远大信息股份有限公司
        /// </summary>

        ///  /// <summary>
        /// 数据处理服务
        /// </summary>
        ELMAHService m_ELMAHService = Singleton<ELMAHService>.GetInstance();
        /// <summary>
        /// 初始化应用程序
        /// </summary>
        ApplicationType aptype = ApplicationType.Air;
        /// <summary>
        /// 系统配置数据接口
        /// </summary>
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
            dtpBegin.SelectedDate = DateTime.Now.AddMonths(-1);
            dtpEnd.SelectedDate = DateTime.Now;
            //站点类型
            IQueryable<V_CodeMainItemEntity> siteTypeEntites = dicService.RetrieveList(DictionaryType.AMS, "系统日志类型");
            comboCityProper.DataSource = siteTypeEntites;
            comboCityProper.DataTextField = "ItemText";
            comboCityProper.DataValueField = "ItemValue";
            comboCityProper.DataBind();
            for (int i = 0; i < comboCityProper.Items.Count; i++)
            {
                comboCityProper.Items[i].Checked = true;
            }
        }
        #endregion

        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {
            string[] portIds = pointCbxRsm.GetPointValues(CbxRsmReturnType.ID);
            DateTime dtBegion = dtpBegin.SelectedDate.Value;
            DateTime dtEnd = dtpEnd.SelectedDate.Value;
            //string[] types = null;
            string type = "";
            foreach (RadComboBoxItem item in comboCityProper.CheckedItems)
            {
                type += (item.Value.ToString() + ",");
            };
            string[] types = type.Trim(',').Split(',');
            //每页显示数据个数            
            int pageSize = gridSystemLogSearch.PageSize;
            //当前页的序号
            int currentPageIndex = gridSystemLogSearch.CurrentPageIndex;
            //查询记录的开始序号
            int startRecordIndex = pageSize * currentPageIndex;
            string orderby = "";
            int recordTotal = 0;
            DataView monitorData = m_ELMAHService.GetDataPager(aptype, portIds, types, dtBegion, dtEnd, pageSize, currentPageIndex, out recordTotal, orderby);
            gridSystemLogSearch.DataSource = monitorData;//dataView;

            //数据分页的页数
            gridSystemLogSearch.VirtualItemCount = monitorData.Count;
        }

        #endregion

        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            gridSystemLogSearch.Rebind();
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
        }

        /// <summary>
        /// 查询按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gridSystemLogSearch.Rebind();
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
                string[] portIds = pointCbxRsm.GetPointValues(CbxRsmReturnType.ID);
                DateTime dtBegion = dtpBegin.SelectedDate.Value;
                DateTime dtEnd = dtpEnd.SelectedDate.Value;
                //string[] types = null;
                string type = "";
                foreach (RadComboBoxItem item in comboCityProper.CheckedItems)
                {
                    type += (item.Value.ToString() + ",");
                };
                string[] types = type.Trim(',').Split(',');
                //每页显示数据个数            
                int pageSize = gridSystemLogSearch.PageSize;
                //当前页的序号
                int currentPageIndex = gridSystemLogSearch.CurrentPageIndex + 1;
                string orderby = "";
                int recordTotal = 0;
                DataView monitorData = m_ELMAHService.GetDataPager(aptype, portIds, types, dtBegion, dtEnd, pageSize, currentPageIndex, out recordTotal, orderby);
                DataTable dtNew = monitorData.ToTable();
                for (int i = 0; i < dtNew.Columns.Count; i++)
                {
                    DataColumn dcNew = dtNew.Columns[i];
                    if (dcNew.ColumnName == "PointName")
                    {
                        dcNew.ColumnName = "监测点名称";
                    }
                    else if (dcNew.ColumnName == "ItemText")
                    {
                        dcNew.ColumnName = "日志类型";
                    }
                    else if (dcNew.ColumnName == "signature")
                    {
                        dcNew.ColumnName = "用户";
                    }
                    else if (dcNew.ColumnName == "logContent")
                    {
                        dcNew.ColumnName = "异常信息";
                    }
                    else if (dcNew.ColumnName == "tstamp")
                    {
                        dcNew.ColumnName = "异常时间";
                    }
                    //else if (dcNew.ColumnName == "HandleUser")
                    //{
                    //    dcNew.ColumnName = "处理人";
                    //}
                    //else if (dcNew.ColumnName == "HandleDateTime")
                    //{
                    //    dcNew.ColumnName = "处理信息";
                    //}
                    //else if (dcNew.ColumnName == "HandleMessage")
                    //{
                    //    dcNew.ColumnName = "处理意见";
                    //}
                    else
                    {
                        dtNew.Columns.Remove(dcNew);
                        i--;
                    }
                }
                ExcelHelper.DataTableToExcel(dtNew, "监测点系统日志", "监测点系统日志", this.Page);
            }
        }


        #endregion
    }
}