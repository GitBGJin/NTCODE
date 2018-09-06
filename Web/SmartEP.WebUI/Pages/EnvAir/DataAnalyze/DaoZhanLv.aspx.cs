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
using SmartEP.Core.Interfaces;
using SmartEP.Service.DataAnalyze.Air.AQIReport;
using SmartEP.Core.Generic;
using SmartEP.Service.Frame;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.Core.Enums;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.Channel;
using SmartEP.Data.SqlServer.AutoMonitoring;
using Telerik.Web.Design;
using System.Web.Services;
using System.Web.Script.Serialization;
using SmartEP.Core.Enums;

namespace SmartEP.WebUI.Pages.EnvAir.DataAnalyze
{
    /// <summary>
    /// 名称：DaoZhangLv.aspx.cs
    /// 创建人：徐义招
    /// 创建日期：2016-07-12
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：到站率统计
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class DaoZhanLv : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        /// 
        DaoZhanLvTongJi m_DZL = new DaoZhanLvTongJi();
        HourAQIService m_HourAQIService = Singleton<HourAQIService>.GetInstance();
        DictionaryService dicService = new DictionaryService();
        MonitoringPointAirService pointAirService = new MonitoringPointAirService();
        InstrumentChannelService m_InstrumentChannelService = new InstrumentChannelService();
        AirPollutantService m_AirPollutantService = new AirPollutantService();
        ApplicationValue mode = new ApplicationValue();
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
            DataView DV;
            DV = m_DZL.CreatDataView("SELECT *  FROM  TB_OMMP_MaintenanceTeam  where  ObjectType=2", "Frame_Connection"); this.rcbMaintenanceTeam.DataSource = DV;
            rcbMaintenanceTeam.DataTextField = "TeamName";
            rcbMaintenanceTeam.DataValueField = "RowGuid";
            rcbMaintenanceTeam.DataBind();                   
        }
        #endregion

        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {
            DataTable dt = new DataTable();
            //页面大小
            int pageSize = grdPAnalyze.PageSize;
            //当前页的序号
            int currentPageIndex = grdPAnalyze.CurrentPageIndex;
            int recordTotal = 0;              
            string rcbType="";
            foreach (RadComboBoxItem item in rcbMaintenanceTeam.CheckedItems)
            {
                rcbType += (item.Value.ToString() + ",");
            }
            string[] qulityType = rcbType.Trim(',').Split(',');         
            string beginDate = dtpBegin.SelectedDate.FormatToString("yyyy-MM-dd 00:00:00");
            string endDate = dtpEnd.SelectedDate.FormatToString("yyyy-MM-dd 00:00:00");
            if (mode == ApplicationValue.Air)           
            grdPAnalyze.DataSource = m_DZL.GetAccessInformation(qulityType, beginDate, endDate, "airaaira-aira-aira-aira-airaairaaira");
            dt = m_DZL.GetAccessInformation(qulityType, beginDate, endDate, "airaaira-aira-aira-aira-airaairaaira");
            recordTotal = dt.Rows.Count;        
            grdPAnalyze.VirtualItemCount = recordTotal;             
         }
        #endregion
        
        /// <summary>
        /// 页面隐藏域控件赋值，将数据需要的参数放入隐藏域，各个参数间用‘|’分割，每个参数内部用‘；’分割
        /// </summary>
        /// <param name="portIds"> 点位</param>
        /// <param name="factors"> 刷卡、签到</param>
        /// <param name="dtBegin">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        private void SetHiddenData(string portIds, DateTime dtBegin, DateTime dtEnd)
        {
           
        }
       
        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {

            grdPAnalyze.Rebind();
            //if (tabStrip.SelectedTab.Text == "图表")
            //{
            //    RegisterScript("RefreshChart();");
            //}
            //else
            //{
            //    FirstLoadChart.Value = "1";
            //}
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
            grdPAnalyze.Rebind();
        }

        /// <summary>
        /// 图表类型选择（折线图、柱形图、点状图）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ChartType_SelectedIndexChanged(object sender, EventArgs e)
        {
            HiddenChartType.Value = ChartType.SelectedValue;
            RegisterScript("ChartTypeChanged('" + ChartType.SelectedValue + "');");
        }

        /// <summary>
        /// ToolBar事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridRTB_ButtonClick(object sender, RadToolBarEventArgs e)
        {
                            
        }
        /// <summary>
        /// 修改要导出的数据表中的列名
        /// </summary>
        /// <param name="dv">污染物变化分析数据表</param>
        /// <returns></returns>
        private DataTable UpdateExportColumnName(DataView dv)
        {
            DataTable dtNew = dv.ToTable();         
            return dtNew;
        }
        /// <summary>
        /// 查询类型切换事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rbtnlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        #endregion    
      
    }
}