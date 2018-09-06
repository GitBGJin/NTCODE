using SmartEP.DomainModel.AirAutoMonitoring;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.BusinessRule;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.DataAnalyze.Air;
using SmartEP.Utilities.Caching;
using SmartEP.Utilities.Office;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartEP.WebUI.Pages.EnvAir.DataAnalyze
{
    public partial class QCRevise : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 气系统标识
        /// </summary>
        private string AirUid = SmartEP.Core.Enums.EnumMapping.GetDesc(SmartEP.Core.Enums.ApplicationType.Air);
        /// <summary>
        /// 授权测点接口
        /// </summary>
        PersonalizedSetService g_PersonalizedSetService = new PersonalizedSetService();
        /// <summary>
        /// 气系统测点接口
        /// </summary>
        MonitoringPointAirService g_MonitoringPointAirService = new MonitoringPointAirService();
        /// <summary>
        /// 校准数据接口
        /// </summary>
        AirCalibrationService g_AirCalibrationService = new AirCalibrationService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitControl();
            }
        }

        #region 初始化控件
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            txtStartDate.SelectedDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 00:00"));
            txtEndDate.SelectedDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:00"));

            //初始化授权测点
            IList<MonitoringPointEntity> PointList = GetPointList();
            ddlPoint.DataTextField = "MonitoringPointName";
            ddlPoint.DataValueField = "PointId";
            ddlPoint.DataSource = PointList;
            ddlPoint.DataBind();
        }
        #endregion

        #region 获取授权测点数据
        public IList<MonitoringPointEntity> GetPointList()
        {
            DataTable dt = g_PersonalizedSetService.GetPersonalizedSetByUserGuid(SessionHelper.Get("UserGuid"), AirUid);
            List<string> PointUids = new List<string>() { };
            foreach (DataRow dr in dt.Rows)
            {
                PointUids.Add(Convert.ToString(dr["ParameterUid"]));
            }
            IQueryable<MonitoringPointEntity> AirPointList = g_MonitoringPointAirService.RetrieveAirMPList();
            return AirPointList.Where(x => PointUids.Contains(x.MonitoringPointUid)).OrderByDescending(x => x.OrderByNum).ToList<MonitoringPointEntity>();
        }
        #endregion

        #region 绑定数据源
        public void BindGrid()
        {
            DateTime StartDate = txtStartDate.SelectedDate == null ? DateTime.Parse("1900-01-01 00:00") : Convert.ToDateTime(txtStartDate.SelectedDate);
            DateTime EndDate = txtEndDate.SelectedDate == null ? DateTime.Parse("1900-01-01 00:00") : Convert.ToDateTime(txtEndDate.SelectedDate);
            if (StartDate == DateTime.Parse("1900-01-01 00:00") || EndDate == DateTime.Parse("1900-01-01 00:00") || EndDate < StartDate)
            {
                Alert("请选择查询日期！");
                return;
            }
            List<int> PointIds = new List<int>() { Convert.ToInt32(ddlPoint.SelectedValue) };
            List<string> CalTypeCodes = new List<string>();
            for (int i = 0; i < cblLogType.Items.Count; i++)
            {
                if (cblLogType.Items[i].Selected)
                {
                    CalTypeCodes.Add(cblLogType.Items[i].Value);
                }
            }
            if (CalTypeCodes.Count == 0)
            {
                Alert("请选择校准类型！");
                return;
            }
            DataTable dt = g_AirCalibrationService.GetData(PointIds, StartDate, EndDate, CalTypeCodes);
            //数据总行数
            int recordTotal = dt.Rows.Count;
            //数据总行数
            RadGrid1.VirtualItemCount = recordTotal;
            RadGrid1.DataSource = dt;
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

        #region Grid数据绑定
        /// <summary>
        /// Grid数据绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }
        #endregion

        #region 导出按钮方法
        /// <summary>
        /// 导出按钮方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridRTB_ButtonClick(object sender, Telerik.Web.UI.RadToolBarEventArgs e)
        {
            Telerik.Web.UI.RadToolBarButton button = (Telerik.Web.UI.RadToolBarButton)e.Item;
            if (button.CommandName == "ExportToExcel")
            {
                DateTime StartDate = txtStartDate.SelectedDate == null ? DateTime.Parse("1900-01-01 00:00") : Convert.ToDateTime(txtStartDate.SelectedDate);
                DateTime EndDate = txtEndDate.SelectedDate == null ? DateTime.Parse("1900-01-01 00:00") : Convert.ToDateTime(txtEndDate.SelectedDate);
                if (StartDate == DateTime.Parse("1900-01-01 00:00") || EndDate == DateTime.Parse("1900-01-01 00:00") || EndDate < StartDate)
                {
                    Alert("请选择查询日期！");
                    return;
                }
                List<int> PointIds = new List<int>() { Convert.ToInt32(ddlPoint.SelectedValue) };
                List<string> CalTypeCodes = new List<string>();
                for (int i = 0; i < cblLogType.Items.Count; i++)
                {
                    if (cblLogType.Items[i].Selected)
                    {
                        CalTypeCodes.Add(cblLogType.Items[i].Value);
                    }
                }
                if (CalTypeCodes.Count == 0)
                {
                    Alert("请选择校准类型！");
                    return;
                }
                DataTable dt = g_AirCalibrationService.GetData(PointIds, StartDate, EndDate, CalTypeCodes);
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    DataColumn dcNew = dt.Columns[i];
                    if (dcNew.ColumnName == "MonitoringPointName")
                    {
                        dcNew.ColumnName = "测点名称";
                    }
                    else if (dcNew.ColumnName == "calTime")
                    {
                        dcNew.ColumnName = "校准开始时间";
                    }
                    else if (dcNew.ColumnName == "overTime")
                    {
                        dcNew.ColumnName = "校准结束时间";
                    }
                    else if (dcNew.ColumnName == "calNameCode")
                    {
                        dcNew.ColumnName = "校准名称";
                    }
                    else if (dcNew.ColumnName == "calTypeCode")
                    {
                        dcNew.ColumnName = "校准类型";
                    }
                    else if (dcNew.ColumnName == "calConc")
                    {
                        dcNew.ColumnName = "校准浓度";
                    }
                    else if (dcNew.ColumnName == "calFlow")
                    {
                        dcNew.ColumnName = "校准流量";
                    }
                    else
                    {
                        dt.Columns.Remove(dcNew);
                        i--;
                    }
                }
                ExcelHelper.DataTableToExcel(dt, "校准数据", "校准数据", this.Page);
            }
        }
        #endregion
    }
}