using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.Frame;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Utilities.Web.UI;
using SmartEP.WebUI.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.BaseData
{
    public partial class MonitoringInstrumentList : BasePage
    {
        DictionaryService dicService = new DictionaryService();
        MonitoringInstrumentService instrumentService = new MonitoringInstrumentService();
        public string pointGuid;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                //获取点位Uid
                pointGuid = PageHelper.GetQueryString("MonitoringPointUid");
                if (!string.IsNullOrEmpty(pointGuid))
                {
                    this.ViewState["PointUid"] = pointGuid;
                }
                else
                {
                    Alert("未获取到点位信息！");
                    return;
                }
            }
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
            IQueryable<InstrumentEntity> instrumentList = instrumentService.RetrieveListByPointUid(ViewState["PointUid"].ToString(),txtInstrumentName.Text);
            grid.DataSource = instrumentList;
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
                       // SelGuid.Add(TypeConversionExtensions.TryTo<object, Guid>(radGrid.MasterTableView.DataKeyValues[Index]["Id"]));
                        SelGuid.Add(radGrid.MasterTableView.DataKeyValues[Index]["RowGuid"].ToString());
                    }
                    string[] SelGid = (string[])SelGuid.ToArray(typeof(string));
                    
                    IQueryable<MonitoringInstrumentEntity> entities = instrumentService.RetrieveListByUids(SelGid);
                    if (entities != null && entities.Count() > 0)
                    {
                        instrumentService.BatchDelete(entities.ToList());
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

        public string GetInstrumentTypeName(string instrumentTypeUid)
        {
            return dicService.GetTextByGuid(Service.Core.Enums.DictionaryType.AMS, "气体检测仪仪器类型", instrumentTypeUid);
        }
    }
}