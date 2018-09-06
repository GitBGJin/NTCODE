﻿using SmartEP.Core.Enums;
using SmartEP.Core.Interfaces;
using SmartEP.Data.SqlServer.BaseData;
using SmartEP.DomainModel;
using SmartEP.Service.BaseData.Alarm;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.Core.Enums;
using SmartEP.Service.Frame;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Utilities.Office;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.Alarm
{
    /// <summary>
    /// 名称：AlarmAuditList.cs
    /// 创建人：李飞
    /// 创建日期：2015-10-24
    /// 维护人员：
    /// 最新维护人员：徐阳
    /// 最新维护日期：2017-06-20
    /// 功能摘要：报警信息审核
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class AlarmAuditList : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        private CreateAlarmService m_CreatAlarmService;
        DictionaryService g_dicService = new DictionaryService();

        protected void Page_Load(object sender, EventArgs e)
        {
            m_CreatAlarmService = new CreateAlarmService();
            if (!IsPostBack)
            {
                InitControl();
                BindRad();
            }
        }

        #region 页面控件初始化
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {

            dtpBegin.SelectedDate = DateTime.Now.AddDays(-1);
            dtpEnd.SelectedDate = DateTime.Now;

            ////通知级别
            //IQueryable<V_CodeMainItemEntity> alarmGradeEntities = g_dicService.RetrieveList(DictionaryType.AMS, "通知级别");
            //if (alarmGradeEntities != null)
            //{
            //    comboAlarmGrade.DataSource = alarmGradeEntities.OrderByDescending(x => x.SortNumber);
            //    comboAlarmGrade.DataTextField = "ItemText";
            //    comboAlarmGrade.DataValueField = "ItemGuid";
            //    comboAlarmGrade.DataBind();

            //    foreach (RadComboBoxItem item in comboAlarmGrade.Items)
            //    {
            //        item.Checked = true;
            //    }
            //}
            //报警类型
            //IQueryable<V_CodeMainItemEntity> alarmTypeEntites = g_dicService.RetrieveList(DictionaryType.AMS, "报警类型");
            //if (alarmTypeEntites != null)
            //{
            //    comboAlarmType.DataSource = alarmTypeEntites.OrderByDescending(x => x.SortNumber);
            //    comboAlarmType.DataTextField = "ItemText";
            //    comboAlarmType.DataValueField = "ItemGuid";
            //    comboAlarmType.DataBind();

            //    foreach (RadComboBoxItem item in comboAlarmType.Items)
            //    {
            //        item.Checked = true;
            //    }
            //}

        }
        #endregion

        private DataTable CreateDataTable()
        {
            DataTable dt = new DataTable();
            //点位名称
            dt.Columns.Add("AlarmUid", typeof(string));
            //点位名称
            dt.Columns.Add("MonitoringPointName", typeof(string));
            //时间
            dt.Columns.Add("CreatDateTime", typeof(DateTime));
            //报警类型
            dt.Columns.Add("AlarmEventName", typeof(string));
            //报警内容
            dt.Columns.Add("Content", typeof(string));
            //颜色
            dt.Columns.Add("AlarmGradeUid", typeof(string));
            //处理人
            dt.Columns.Add("dealMan", typeof(string));
            //处理时间
            dt.Columns.Add("dealTime", typeof(DateTime));
            //是否审核
            dt.Columns.Add("auditFlag", typeof(string));
            //审核人
            dt.Columns.Add("auditMan", typeof(string));
            //审核时间
            dt.Columns.Add("auditTime", typeof(DateTime));
            return dt;
        }

        #region 绑定Grid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {
            if (chbAllData.Checked == true)
            {
                //每页显示数据个数            
                int pageSize = gridMonitor.PageSize;
                //当前页的序号
                int pageNo = gridMonitor.CurrentPageIndex + 1;

                //数据总行数
                int recordTotal = 0;
                var dataView = m_CreatAlarmService.GetGridViewPager(pageSize, pageNo, GetWhereString(), out recordTotal);


                gridMonitor.DataSource = dataView;
                //数据总行数
                gridMonitor.VirtualItemCount = recordTotal;
            }
            else
            {
                DataView dv = new AlarmDAL().GetAlarmInfo(GetWhereString());
                string[] str = new string[] { "ApplicationUid", "monitoringPointUid", "alarmEventUid", "itemName" };    //去除重复的字段

                DataTable dataView1 = dv.ToTable(true, str);
                DataView dw = dataView1.DefaultView;
                DataTable sourceDT = CreateDataTable();
                if (dw != null && dw.Count > 0)
                {
                    for (int i = 0; i < dw.Count; i++)
                    {
                        DataView lastDv = new AlarmDAL().GetAuditLastNewAlarmInfo(dw[i]["ApplicationUid"].ToString(), dw[i]["monitoringPointUid"].ToString(), dw[i]["alarmEventUid"].ToString(), dw[i]["itemName"].ToString(), dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss"), dtpEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));

                        DataRow dr = sourceDT.NewRow();
                        dr["AlarmUid"] = lastDv[0]["AlarmUid"];
                        dr["MonitoringPointName"] = lastDv[0]["MonitoringPointName"];
                        dr["CreatDateTime"] = lastDv[0]["CreatDateTime"];
                        dr["AlarmEventName"] = lastDv[0]["AlarmEventName"];
                        dr["Content"] = lastDv[0]["Content"];
                        dr["AlarmGradeUid"] = lastDv[0]["AlarmGradeUid"];
                        dr["dealMan"] = lastDv[0]["dealMan"];
                        dr["dealTime"] = lastDv[0]["dealTime"];
                        dr["auditFlag"] = lastDv[0]["auditFlag"];
                        dr["auditMan"] = lastDv[0]["auditMan"];
                        dr["auditTime"] = lastDv[0]["auditTime"];
                        sourceDT.Rows.Add(dr);
                    }
                }
                //dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss")
                gridMonitor.DataSource = sourceDT.DefaultView;
                //数据总行数
                gridMonitor.VirtualItemCount = sourceDT.DefaultView.Count;
            }
        }
        #endregion

        #region 服务器端控件事件处理
        /// <summary>
        /// 绑定数据源（用于分页、排序）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridMonitor_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }

        /// <summary>
        /// 查询按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gridMonitor.Rebind();
        }

        /// <summary>
        /// ToolBar事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridRTB_ButtonClick(object sender, RadToolBarEventArgs e)
        {
            Telerik.Web.UI.RadToolBarButton button = (Telerik.Web.UI.RadToolBarButton)e.Item;
            if (button.CommandName == "audit")
            {
                //OMOnlineReported onlineReportedModel = new OMOnlineReported();
                int countCheck = 0;
                string ids = string.Empty;
                for (int i = 0; i < gridMonitor.Items.Count; i++)
                {
                    GridEditableItem editItem = (GridEditableItem)gridMonitor.Items[i];
                    System.Web.UI.WebControls.CheckBox sel = (System.Web.UI.WebControls.CheckBox)editItem.FindControl("selAlarmConfig");
                    if (sel.Checked)
                    {
                        if (!string.IsNullOrEmpty(ids)) ids += ";";
                        string keyId = editItem.GetDataKeyValue("AlarmUid").ToString();
                        ids += keyId;
                        countCheck++;
                    }
                }
                if (countCheck == 0)
                {
                    RadWindowManager1.RadAlert("请选择要审核的项！", 250, 150, "提示", "");
                    //Response.Write("<script>alert('请选择要审核的项')</script>");
                    return;
                }
                else
                {
                    RadAjaxManager1.ResponseScripts.Add("ShowEditForm('" + ids + "','mulite')");
                }
            }
        }

        /// <summary>
        /// 数据行绑定处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridMonitor_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                DataRowView drv = e.Item.DataItem as DataRowView;
                if (item["AlarmGradeUid"] != null)
                {
                    GridTableCell colorCell = (GridTableCell)item["AlarmGradeUid"];
                    switch (colorCell.Text.TrimEnd())
                    {
                        case "9df318d1-b104-4425-a846-69e66222d53c":
                            //一级报警
                            colorCell.BackColor = System.Drawing.Color.Yellow;
                            break;
                        case "a2bbb49e-f8c6-4c29-b49e-c3974e4f38a1":
                            //二级报警
                            colorCell.BackColor = System.Drawing.Color.Gold;
                            break;
                        case "e1111f5e-6ef7-4f95-8778-bd7062ce2aae":
                            //三级报警
                            colorCell.BackColor = System.Drawing.Color.Red;
                            break;
                    }
                    colorCell.Text = string.Empty;
                }
                if (item["auditFlag"] != null)
                {
                    GridTableCell auditFlag = (GridTableCell)item["auditFlag"];
                    if ((Convert.IsDBNull(auditFlag.Text)) || (auditFlag.Text.Trim() == "&nbsp;") || auditFlag.Text.Trim() == "0")
                    {
                        auditFlag.Text = "未审核";
                    }
                    else
                    {
                        auditFlag.Text = "已审核";
                        CheckBox cbx = (CheckBox)(e.Item.Cells[2].FindControl("selAlarmConfig"));
                        cbx.Enabled = false;
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// 取得查询条件
        /// </summary>
        /// <returns></returns>
        private string GetWhereString()
        {
            string where = string.Empty;
            string[] portIds = pointCbxRsm.GetPointValues(CbxRsmReturnType.Guid);
            where = string.Format(" dealFlag = 1 and ApplicationUid = '{0}' ", SmartEP.Core.Enums.EnumMapping.GetDesc(ApplicationType.Air));
            //站点
            if (portIds != null && portIds.Length > 0)
            {
                where += string.Format(" AND MonitoringPointUid in ('{0}')", StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), "','"));
            }

            //是否审核
            if (cbxHaveAudit.Checked && !cbxNoAudit.Checked)
            {
                where += " AND auditFlag = 1 ";
            }
            else if (!cbxHaveAudit.Checked && cbxNoAudit.Checked)
            {
                where += " AND (auditFlag = 0 OR auditFlag IS NULL)";
            }
            //时间范围
            if (dtpBegin.SelectedDate != null)
                where += string.Format(" AND CreatDateTime >= '{0}'", dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            if (dtpEnd.SelectedDate != null)
                where += string.Format(" AND CreatDateTime <= '{0}'", dtpEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            //报警类型
            if (comboAlarmType.CheckedItems.Count > 0)
            {
                string alarmTypes = StringExtensions.GetArrayStrNoEmpty(comboAlarmType.CheckedItems.Select(x => x.Value).ToList<string>(), "','");
                where += string.Format(" AND AlarmEventUid in ('{0}')", alarmTypes);
            }

            ////报警级别
            //if (comboAlarmGrade.CheckedItems.Count > 0)
            //{
            //    string alarmGrades = StringExtensions.GetArrayStrNoEmpty(comboAlarmGrade.CheckedItems.Select(x => x.Value).ToList<string>(), "','");
            //    where += string.Format(" AND AlarmGradeUid in ('{0}')", alarmGrades);
            //}
            return where;
        }

        //绑定报警类型和控制区域和站点控件的隐藏
        public void BindRad()
        {
            IQueryable<V_CodeMainItemEntity> siteTypeEntites = null;
            //siteTypeEntites = g_dicService.RetrieveList(DictionaryType.AMS, "报警类型").Where(t => t.ItemText.Contains("重度污染") || t.ItemText.Contains("AQI"));
            siteTypeEntites = g_dicService.RetrieveList(DictionaryType.AMS, "报警类型").Where(t => t.ItemText.Contains("超") || t.ItemText.Contains("数据缺失") || t.ItemText.Contains("重复") || t.ItemText.Contains("离线"));
            comboAlarmType.DataSource = siteTypeEntites;
            comboAlarmType.DataTextField = "ItemText";
            comboAlarmType.DataValueField = "ItemGuid";
            comboAlarmType.DataBind();

        }

    }
}