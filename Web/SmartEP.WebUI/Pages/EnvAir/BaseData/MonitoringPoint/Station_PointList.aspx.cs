using SmartEP.DomainModel;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.Core.Enums;
using SmartEP.Service.Frame;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.BaseData
{
    public partial class Station_PointList : SmartEP.WebUI.Common.BasePage
    {
        MonitoringPointAirService airPointService = new MonitoringPointAirService();
        DictionaryService dicService = new DictionaryService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                InitControl();
            }
        }

        private void InitControl()
        {
            //行政区划
            IQueryable<V_CodeMainItemEntity> regionEntites = dicService.RetrieveList(DictionaryType.AMS,"行政区划");
            comboRegion.DataSource = regionEntites;
            comboRegion.DataTextField = "ItemText";
            comboRegion.DataValueField = "ItemGuid";
            comboRegion.DataBind();
            comboRegion.Items.Insert(0, new RadComboBoxItem("", ""));


            //站点类型
            IQueryable<V_CodeMainItemEntity> siteTypeEntites = dicService.RetrieveList(DictionaryType.Air,"空气站点类型");
            comboSiteType.DataSource = siteTypeEntites;
            comboSiteType.DataTextField = "ItemText";
            comboSiteType.DataValueField = "ItemGuid";
            comboSiteType.DataBind();
            comboSiteType.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            grid.Rebind();
        }

        /// <summary>
        /// 数据绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            //每页显示数据个数            
            //int pageSize = grid.PageSize;
            //当前页的序号
            //int currentPageIndex = grid.CurrentPageIndex + 1;
            //查询记录的开始序号
            //int startRecordIndex = pageSize * currentPageIndex;

            //int recordTotal = 0;
            IQueryable<MonitoringPointEntity> pointList = airPointService.RetrieveAirMPList(txtPointName.Text.Trim(), comboRegion.SelectedValue, comboSiteType.SelectedValue,rbtnEnableOrNot.SelectedValue == "1" ? true : false);
            grid.DataSource = pointList;

          
            //数据分页的页数
            //grid.VirtualItemCount = pointList.Count();

        }

        /// <summary>
        /// ToolBar按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grid_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            RadGrid radGrid = ((RadGrid)sender);
            int Index = -1;
            switch (e.CommandName)
            {
                case "DeleteSelected":
                    #region DeleteSelected
                    ArrayList SelGuid = new ArrayList();
                    foreach (String item in radGrid.SelectedIndexes)
                    {
                        Index = Convert.ToInt32(item);
                        SelGuid.Add(radGrid.MasterTableView.DataKeyValues[Index]["MonitoringPointUid"].ToString());
                    }
                    string[] SelGid = (string[])SelGuid.ToArray(typeof(string));
                    IQueryable<MonitoringPointExtensionForEQMSAirEntity> extensionEntities = airPointService.RetrieveAirExtensionPointListByPointUids(SelGid);
                    IQueryable<MonitoringPointEntity> entities = airPointService.RetrieveListByPointUids(SelGid);
                    if (entities != null && entities.Count() > 0)
                    {
                        airPointService.BatchDelete(extensionEntities.ToList());
                        airPointService.BatchDelete(entities.ToList());
                        base.Alert("删除成功！");
                    }
                    #endregion
                    break;
            }
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grid_GridExporting(object sender, GridExportingArgs e)
        {
            //DataTable dataTable = g_OfflineBiz.GetGridDataAll(GetWhereString());
            //if (e.ExportType == ExportType.Excel)
            //{
            //    ExcelHelper.DataTableToExcel(dataTable, "离线配置", "离线配置", this.Page);
            //}
            //else if (e.ExportType == ExportType.Word)
            //{
            //    WordHelper.DataTableToWord(dataTable, "离线配置", this.Page);
            //}
        }

        /// <summary>
        /// 获取行政区划
        /// </summary>
        /// <param name="regionUid"></param>
        /// <returns></returns>
        public string GetRegion(string regionUid)
        {
            return dicService.GetTextByGuid(DictionaryType.AMS,"行政区划", regionUid);
        }

        /// <summary>
        /// 获取空气站点类型
        /// </summary>
        /// <param name="siteTypeUid"></param>
        /// <returns></returns>
        public string GetSiteType(string siteTypeUid)
        {
            return dicService.GetTextByGuid(DictionaryType.Air, "空气站点类型", siteTypeUid);
        }

        /// <summary>
        /// 获取站点运行状态
        /// </summary>
        /// <param name="runStatusUid"></param>
        /// <returns></returns>
        public string GetRunStatus(string runStatusUid)
        {
            return dicService.GetTextByGuid(DictionaryType.AMS, "站点运行状态", runStatusUid);
        }

    }
}