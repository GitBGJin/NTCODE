using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Utilities.Web.UI;
using SmartEP.WebUI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.BaseData.MonitoringPoint
{
    public partial class PointFactorList : BasePage
    {
        InstrumentChannelService instrumentChannelService = new InstrumentChannelService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                //获取点位Uid
                string pointGuid = PageHelper.GetQueryString("MonitoringPointUid");
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
        /// 数据绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            grid.DataSource = instrumentChannelService.RetrieveChannelListByPointUid(ViewState["PointUid"].ToString()).GroupBy(p => new { p.PollutantCode }).Select(g => g.First());
        }

        /// <summary>
        /// 获取通道类型Uid获取类型名称
        /// </summary>
        /// <param name="typeUid"></param>
        /// <returns></returns>
        public string GetChannelTypeName(string typeUid)
        {
            if (typeUid == "ae39f55e-5c43-4b4a-b224-0b925b5f3c9f")
                return "通道";
            else return "仪器状态";
            
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grid_GridExporting(object sender, GridExportingArgs e)
        {
            
        }
    }
}