﻿using SmartEP.Core.Generic;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.DataAnalyze.Air.DataQuery;
using SmartEP.Utilities.Office;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.DataAnalyze
{
    /// <summary>
    /// 名称：DataCaptureRateNew.aspx.cs
    /// 创建人：刘晋
    /// 创建日期：2017-09-11
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：有效数据捕获率
    /// 虚拟分页类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class DataCaptureRateNew : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        DataQueryByHourService m_HourData = Singleton<DataQueryByHourService>.GetInstance();
        /// <summary>
        /// 测点类
        /// </summary>
        MonitoringPointAirService g_MonitoringPointAir = null;
        string ddlSel = "1";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitControl();
            }
        }
        protected override void OnPreInit(EventArgs e)
        {
            string isAudit = "NTTJ";
            if (isAudit != null && isAudit != "")
            {
                factorCbxRsm.isAudit(isAudit);
                pointCbxRsmSuper.isSuper("1");
                pointCbxRsm.isSuper("AirDER");
            }
        }
        #region 页面控件初始化
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            rdpBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
            rdpEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
            rbtnlType.Items.Add(new ListItem("超级站", "Super"));
            rbtnlType.Items.Add(new ListItem("常规站", "General"));
            rbtnlType.SelectedValue = "Super";
            factorCbxRsm.Visible = true;
            foreach (RadComboBoxItem item in factorCom.Items)
            {
                item.Checked = true;
            }
            //rbtnlType.Items[1].Selected = true;
            //国控点，对照点，路边站
            //MonitoringPointAirService m_MonitoringPointAirService = Singleton<MonitoringPointAirService>.GetInstance();
            //string strpointName = "";
            //IQueryable<MonitoringPointEntity> EnableOrNotports = m_MonitoringPointAirService.RetrieveAirMPListByEnable();
            //string[] EnableOrNotportsarry = EnableOrNotports.Where(p => p.ContrlUid == "6fadff52-2338-4319-9f1d-7317823770ad" || p.ContrlUid == "c1158eb6-4d69-4846-a963-d16b9d2794ca").Select(p => p.MonitoringPointName).ToArray();
            //foreach (string point in EnableOrNotportsarry)
            //{
            //    strpointName += point + ";";
            //}
            //pointCbxRsm.SetPointValuesFromNames(strpointName);
            string pollutantName = System.Configuration.ConfigurationManager.AppSettings["NTTJ"];
            factorCbxRsm.SetFactorValuesFromCodes(pollutantName);

        }
        #endregion

        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {
            DateTime dtBegion = Convert.ToDateTime(rdpBegin.SelectedDate.Value.ToString("yyyy-MM-dd 00:00:00"));
            DateTime dtEnd = Convert.ToDateTime(rdpEnd.SelectedDate.Value.ToString("yyyy-MM-dd 23:59:59"));

            //生成RadGrid的绑定列
            //每页显示数据个数            
            int pageSize = grdDataCaptureRateNew.PageSize;
            //当前页的序号
            int pageNo = grdDataCaptureRateNew.CurrentPageIndex;
            var AvgDayData = new DataView();
            if (rbtnlType.SelectedValue == "Super")
            {
                string[] portIds = pointCbxRsmSuper.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
                string[] factors = factorCbxRsm.GetFactorValues(SmartEP.Core.Enums.CbxRsmReturnType.Code);
                //点位
                if (portIds != null && factors != null)
                {
                    AvgDayData = m_HourData.GetSuperCaptureDataPager(portIds, factors, dtBegion, dtEnd);
                    grdDataCaptureRateNew.DataSource = AvgDayData;
                    grdDataCaptureRateNew.VirtualItemCount = AvgDayData.Count;
                }
                else
                {
                    grdDataCaptureRateNew.DataSource = new DataTable();
                }
            }
            else
            {
                //List<string> list = new List<string>();
                //string[] portId = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
                //for (int i = 0; i < portId.Length; i++)
                //{
                //    if (portId[i] != "204")
                //    {
                //        list.Add(portId[i]);
                //    }
                //}
                string[] portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
                string[] factors = factorCom.CheckedItems.Select(x => x.Value).ToArray();
                //点位
                if (portIds != null && factors != null)
                {
                    AvgDayData = m_HourData.GetCaptureDataPager(portIds, factors, dtBegion, dtEnd);
                    grdDataCaptureRateNew.DataSource = AvgDayData;
                    grdDataCaptureRateNew.VirtualItemCount = AvgDayData.Count;
                }
                else
                {
                    grdDataCaptureRateNew.DataSource = new DataTable();
                }
            }

        }

        #endregion

        protected void grdDataCaptureRateNew_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }

        protected void grdDataCaptureRateNew_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void grdDataCaptureRateNew_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {
            try
            {
                GridBoundColumn col = e.Column as GridBoundColumn;
                if (col == null)
                    return;

                col.HeaderText = col.DataField;
                col.EmptyDataText = "--";
                col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                col.HeaderStyle.Width = Unit.Pixel(130);
                col.ItemStyle.Width = Unit.Pixel(130);

            }
            catch (Exception ex) { }
        }
        /// <summary>
        /// 查询按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            grdDataCaptureRateNew.CurrentPageIndex = 0;
            grdDataCaptureRateNew.Rebind();
        }
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridRTB_ButtonClick(object sender, Telerik.Web.UI.RadToolBarEventArgs e)
        {
            try
            {
                Telerik.Web.UI.RadToolBarButton button = (Telerik.Web.UI.RadToolBarButton)e.Item;
                if (button.CommandName == "ExportToExcel")
                {
                    //string[] portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
                    if (rbtnlType.SelectedValue == "Super")
                    {
                        string[] portIds = pointCbxRsmSuper.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
                        string[] factors = factorCbxRsm.GetFactorValues(SmartEP.Core.Enums.CbxRsmReturnType.Code);
                        DateTime dtBegion = Convert.ToDateTime(rdpBegin.SelectedDate.Value.ToString("yyyy-MM-dd 00:00:00"));
                        DateTime dtEnd = Convert.ToDateTime(rdpEnd.SelectedDate.Value.ToString("yyyy-MM-dd 23:59:59"));
                        DataTable dt = m_HourData.GetSuperCaptureDataPager(portIds, factors, dtBegion, dtEnd, true).Table;
                        ExcelHelper.DataTableToExcel(dt, "有效数据捕获率", "有效数据捕获率", this.Page);
                    }
                    else
                    {
                        string[] portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
                        string[] factors = factorCom.CheckedItems.Select(x => x.Value).ToArray();
                        DateTime dtBegion = Convert.ToDateTime(rdpBegin.SelectedDate.Value.ToString("yyyy-MM-dd 00:00:00"));
                        DateTime dtEnd = Convert.ToDateTime(rdpEnd.SelectedDate.Value.ToString("yyyy-MM-dd 23:59:59"));
                        DataTable dt = m_HourData.GetCaptureDataPager(portIds, factors, dtBegion, dtEnd, true).Table;
                        ExcelHelper.DataTableToExcel(dt, "有效数据捕获率", "有效数据捕获率", this.Page);
                    }
                    
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void rbtnlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdDataCaptureRateNew.CurrentPageIndex = 0;
            if (rbtnlType.SelectedValue == "General")
            {
                //dvfc.Visible = true;
                //dvfcr.Visible = false;
                factorCbxRsm.Visible = false;
                factorCom.Visible = true;
                pointCbxRsm.Visible = true;
                pointCbxRsmSuper.Visible = false;
            }
            else if (rbtnlType.SelectedValue == "Super")
            {
                //dvfc.Visible = false;
                //dvfcr.Visible = true;
                factorCbxRsm.Visible = true;
                factorCom.Visible = false;
                pointCbxRsm.Visible = false;
                pointCbxRsmSuper.Visible = true;
            }
        }
    }
}