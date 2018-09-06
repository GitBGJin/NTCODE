using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.DataAnalyze.Air.AQIReport;
using SmartEP.WebUI.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Service.DataAuditing.AuditInterfaces;

namespace SmartEP.WebUI.Pages.EnvAir.Audit
{
    public partial class SyncGuoJiaAuditData : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 站点服务
        /// </summary>
        MonitoringPointAirService m_MonitoringPointAirService = Singleton<MonitoringPointAirService>.GetInstance();

        //protected void Page_PreRender(object sender, EventArgs e)
        //{
        //    if (!IsPostBack)
        //    {
        //        InitControl();
        //    }
        //}
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            BindData();
        }


        //绑定数据
        private void BindData()
        {
            rdpBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddDays(-1));
            rdpEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.AddDays(-1));


            //取得国控点
            IQueryable<MonitoringPointEntity> ports = m_MonitoringPointAirService.RetrieveAirMPListByCountryControlled();
            rcbPoint.DataValueField = "PointId";
            rcbPoint.DataTextField = "MonitoringPointName";
            rcbPoint.DataSource = ports;
            rcbPoint.DataBind();

            foreach (RadComboBoxItem item in rcbPoint.Items)
            {
                item.Checked = true;
            }

            DataTable channelDt = new SyncGuoJiaDataService().GetChannelNameInfo();
            foreach (DataRow dr in channelDt.Rows)
            {
                int SourceChannel = 0;
                if (int.TryParse(dr["SourceChannel"].ToString(), out SourceChannel))
                {
                    if (SourceChannel >= 108)
                    {
                        dr.Delete();
                    }
                }
            }
            channelDt.AcceptChanges();
            rcbFactor.DataValueField = "LocalChannel";
            rcbFactor.DataTextField = "PollutantName";
            rcbFactor.DataSource = channelDt;
            rcbFactor.DataBind();

            foreach (RadComboBoxItem item in rcbFactor.Items)
            {
                item.Checked = true;
            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitControl();
            }
        }

        /// <summary>
        /// 同步按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSync_Click(object sender, EventArgs e)
        {
            DateTime dtStart = rdpBegin.SelectedDate.Value;
            DateTime dtEnd = rdpEnd.SelectedDate.Value;
            dtStart = Convert.ToDateTime(dtStart.ToString("yyyy-MM-dd 00:00:00"));
            dtEnd = Convert.ToDateTime(dtEnd.ToString("yyyy-MM-dd 23:59:59"));

            if (dtStart > dtEnd)
            {
                Alert("开始时间不能大于结束时间！");
                return;
            }

            if (rcbPoint.CheckedItems.Count <= 0)
            {// 不同步审核全市AQI数据时，需要选着测点
                Alert("请选择需要同步数据的测点！");
                return;
            }

            if (rcbFactor.CheckedItems.Count <= 0)
            {
                Alert("请选择需要同步的因子！");
                return;
            }

            //同步测点
            string pointIds = StringExtensions.GetArrayStrNoEmpty(rcbPoint.CheckedItems.Select(x => x.Value).ToList<string>(), ",");

            string pollutantCodes = StringExtensions.GetArrayStrNoEmpty(rcbFactor.CheckedItems.Select(x => x.Value).ToList<string>(), "','");

            SyncGuoJiaDataService syncService = new SyncGuoJiaDataService();
            syncService.SyncAuditHourData(pointIds, pollutantCodes, dtStart, dtEnd, CoverData.Checked);

            /*
            if (cbxAuditHour.Checked)
            {// 选中同步审核小时数据
                syncService.SyncAuditHourData(pointIds, dtStart, dtEnd);
            }

            if (cbxAuditDayAQI.Checked)
            {// 选中同步审核日AQI数据
                syncService.SyncRTD_DayAQI(pointIds, dtStart, dtEnd);
            }

            if (cbxAuditCityAQI.Checked)
            {// 选中同步审核全市AQI数据
                syncService.SyncRTD_CityDayAQI(dtStart, dtEnd);
            }
            */
            Alert("数据同步成功!");
        }

        protected void rbtCalibration_Click(object sender, EventArgs e)
        {
            DateTime dtStart = rdpBegin.SelectedDate.Value;
            DateTime dtEnd = rdpEnd.SelectedDate.Value;
            dtStart = Convert.ToDateTime(dtStart.ToString("yyyy-MM-dd 00:00:00"));
            dtEnd = Convert.ToDateTime(dtEnd.ToString("yyyy-MM-dd 23:59:59"));
            //同步测点
            //string pointIds = StringExtensions.GetArrayStrNoEmpty(rcbPoint.CheckedItems.Select(x => x.Value).ToList<string>(), ",");
            SyncGuoJiaDataService syncService = new SyncGuoJiaDataService();
            DataTable dt = GetAuditLog();
            if (dt.Rows.Count > 0)
            {
                gridSyncGuoJiaAuditLog.Rebind();

                string[] pointIdArry = dt.AsEnumerable().Select(t => t.Field<string>("PointId")).Distinct().ToArray();
                DateTime?[] dtArry = dt.AsEnumerable().Select(t => t.Field<DateTime?>("TimePoint")).Distinct().ToArray();
                string pointIds = string.Join(",", pointIdArry);
                syncService.copyAuditLog(dt);
                DateTime dtMark = DateTime.Now;
                DateTime MaxDt = dtMark;
                DateTime MinDt = dtMark;
                if (dtArry != null)
                {
                    List<DateTime> timeNew = new List<DateTime>();
                    for (int i = 0; i < dtArry.Length; i++)
                    {
                        if (dtArry[i] != null)
                        {
                            timeNew.Add(dtArry[i].Value);
                        }
                    }
                    if (timeNew.Count > 0)
                    {
                        MaxDt = timeNew[0];
                        MinDt = timeNew[0];
                        for (int j = 0; j < timeNew.Count; j++)
                        {
                            if (timeNew[j] > MaxDt)
                            {
                                MaxDt = timeNew[j];
                            }
                            if (timeNew[j] < MinDt)
                            {
                                MinDt = timeNew[j];
                            }
                        }
                    }
                }

                syncService.UpdateAuditData(pointIds, dt, MaxDt, MinDt, dtMark);
                Alert("差异数据已经同步！");
            }
            else
            {
                gridSyncGuoJiaAuditLog.DataSource = new DataTable();
                Alert("数据一致！");

            }

        }

        public void GetDateTime(DateTime?[] dtArry)
        {
        }
        public void UpdateAuditAirData()
        {
            DataTable dt = GetAuditLog();
        }
        public DataTable GetAuditLog()
        {
            DateTime dtStart = rdpBegin.SelectedDate.Value;
            DateTime dtEnd = rdpEnd.SelectedDate.Value;
            dtStart = Convert.ToDateTime(dtStart.ToString("yyyy-MM-dd 00:00:00"));
            dtEnd = Convert.ToDateTime(dtEnd.ToString("yyyy-MM-dd 23:59:59"));

            if (dtStart > dtEnd)
            {
                Alert("开始时间不能大于结束时间！");
                return null;
            }

            if (rcbPoint.CheckedItems.Count <= 0)
            {// 不同步审核全市AQI数据时，需要选着测点
                Alert("请选择需要同步数据的测点！");
                return null;
            }

            if (rcbFactor.CheckedItems.Count <= 0)
            {
                Alert("请选择需要同步的因子！");
                return null;
            }
            //同步测点
            string pointIds = StringExtensions.GetArrayStrNoEmpty(rcbPoint.CheckedItems.Select(x => x.Value).ToList<string>(), ",");

            string pollutantCodes = StringExtensions.GetArrayStrNoEmpty(rcbFactor.CheckedItems.Select(x => x.Value).ToList<string>(), "','");

            SyncGuoJiaDataService syncService = new SyncGuoJiaDataService();
            DataTable dt = syncService.GuojiaAuditAirLog(pointIds, pollutantCodes, dtStart, dtEnd);
            return dt;
        }
        public void BindGrid()
        {
            DataTable dt = GetAuditLog();
            gridSyncGuoJiaAuditLog.DataSource = dt;
            gridSyncGuoJiaAuditLog.VirtualItemCount = dt.Rows.Count;
        }
        protected void gridSyncGuoJiaAuditLog_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }

        protected void gridSyncGuoJiaAuditLog_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                if (item["VerifyDataType"] != null)
                {
                    GridTableCell VerifyDataType = (GridTableCell)item["VerifyDataType"];
                    switch (VerifyDataType.Text)
                    {
                        case "1":
                            VerifyDataType.Text = "无效审核为有效";
                            break;
                        case "2":
                            VerifyDataType.Text = "有效审核为无效";
                            break;
                        case "3":
                            VerifyDataType.Text = "零值负值数据修约";
                            break;
                        case "5":
                            VerifyDataType.Text = "还原为原始数据";
                            break;
                        case "6":
                            VerifyDataType.Text = "系统自动审核为无效";
                            break;
                        case "7":
                            VerifyDataType.Text = "手工导入";
                            break;
                        default:
                            break;
                    }
                }
                if (item["PointId"] != null)
                {
                    GridTableCell VerifyDataType = (GridTableCell)item["PointId"];
                    VerifyDataType.Text = rcbPoint.Items.FindItemByValue(VerifyDataType.Text).Text;
                }
            }
        }

        protected void RTBUpdate_Click(object sender, EventArgs e)
        {
        }
    }
}