using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.AutoMonitoring.Air;
//using SmartEP.Service.BaseData.Channel;
using SmartEP.Service.BaseData.MPInfo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.RealTimeData
{
    /// <summary>
    /// 名称：DataEffectRate.cs
    /// 创建人：尤红兵
    /// 创建日期：2015-08-20
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：数据有效率查询
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class _24HDataSamplingRate : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        AirDataSamplingRateService airDataSamplingRate = new AirDataSamplingRateService();

        /// <summary>
        /// 选择站点
        /// </summary>
        private IList<IPoint> points = null;

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
        }
        #endregion

        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {
            points = pointCbxRsm.GetPoints();
            string[] portIds = points.Select(t => t.PointID).ToArray();
            int pageSize = gridDataSamplingRate.PageSize;//每页显示数据个数  
            int pageNo = gridDataSamplingRate.CurrentPageIndex;//当前页的序号
            int recordTotal = 0;//数据总行数
            int hour = Convert.ToInt32(ddlTime.SelectedValue);
            //绑定数据
            var dataSamplingRate = airDataSamplingRate.GetSamplingRateDetailByHours(portIds, hour, pageSize, pageNo, out recordTotal);
            if (dataSamplingRate == null)
            {
                gridDataSamplingRate.DataSource = new DataTable();
            }
            else
            {
                gridDataSamplingRate.DataSource = dataSamplingRate;
            }

            //数据总行数
            gridDataSamplingRate.VirtualItemCount = recordTotal;

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
        }
        /// <summary>
        /// 查询按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gridDataSamplingRate.Rebind();
        }
        #endregion

    }
}