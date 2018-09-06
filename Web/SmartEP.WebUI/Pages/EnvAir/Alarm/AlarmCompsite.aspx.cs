using SmartEP.Utilities.Office;
//using SmartEP.Service.AutoMonitoring.DataAnalyze;
using SmartEP.Service.BaseData;
using SmartEP.Service.Core.Enums;
using SmartEP.WebControl.CbxRsm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SmartEP.Core.Enums;
using SmartEP.WebUI.Common;
using SmartEP.Service.Frame;
using SmartEP.DomainModel;
using SmartEP.Service.BaseData.Alarm;
using SmartEP.DomainModel.BaseData;
using SmartEP.Core.Interfaces;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Data.SqlServer.BaseData;
using SmartEP.Utilities.Web.UI;
using SmartEP.Service.BaseData.MPInfo;
using System.Text;

namespace SmartEP.WebUI.Pages.EnvAir.Alarm
{
    /// <summary>
    /// 名称：AlarmCompsite.cs
    /// 创建人：李飞
    /// 创建日期：2015-08-20
    /// 维护人员：
    /// 最新维护人员：徐阳
    /// 最新维护日期：2017-06-20
    /// 功能摘要：报警信息综合
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class AlarmCompsite : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        private CreateAlarmService m_CreatAlarmService;
        /// <summary>
        /// 应用程序Uid
        /// </summary>
        private string g_ApplicationValue = SmartEP.Core.Enums.EnumMapping.GetApplicationValue(ApplicationValue.Air);
        DictionaryService g_dicService = new DictionaryService();

        protected void Page_Load(object sender, EventArgs e)
        {
            m_CreatAlarmService = new CreateAlarmService();
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
            string pointId = PageHelper.GetQueryString("pointId");
            string DateBegin = PageHelper.GetQueryString("DateBegin");
            string DateEnd = PageHelper.GetQueryString("DateEnd");
            string flag = PageHelper.GetQueryString("flag");
            if (flag == "1")
            {
                chbAllData.Checked = true;
                chbAllData.Style["Checked"] = "true";
                chbAllData.Style.Add("Checked", "true");
            }
            if (!string.IsNullOrWhiteSpace(pointId))
            {
                pointCbxRsm.SetPointValuesFromNames(pointId);
            }
            if (!string.IsNullOrWhiteSpace(DateBegin))
            {
                dtpBegin.SelectedDate = DateTime.Parse(DateBegin);
            }
            else
            {
                dtpBegin.SelectedDate = DateTime.Now.AddDays(-1);
            }
            if (!string.IsNullOrWhiteSpace(DateEnd))
            {
                dtpEnd.SelectedDate = DateTime.Parse(DateEnd);
            }
            else
            {
                dtpEnd.SelectedDate = DateTime.Now;
            }
            BindRad();

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
            dt.Columns.Add("RecordDateTime", typeof(DateTime));
            //报警类型
            dt.Columns.Add("AlarmEventName", typeof(string));
            //报警内容
            dt.Columns.Add("Content", typeof(string));
            //颜色
            dt.Columns.Add("AlarmGradeUid", typeof(string));
            //是否处理
            dt.Columns.Add("dealFlag", typeof(string));
            //处理人
            dt.Columns.Add("dealMan", typeof(string));
            //处理时间
            dt.Columns.Add("dealTime", typeof(DateTime));
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
                DataView dv = new AlarmDAL().GetAlarmInfoNew(GetWhereString());
                gridMonitor.DataSource = dv;
                //数据总行数
                gridMonitor.VirtualItemCount = dv.Count;
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
            if (button.CommandName == "ExportToExcel")
            {
                DataView dv = m_CreatAlarmService.GetExportData(GetWhereString(), string.Empty);
                if (dv != null)
                {
                    ExcelHelper.DataTableToExcel(dv.ToTable(), "报警信息", "报警信息", this.Page);
                    dv.Dispose();
                }
            }
            if (button.CommandName == "deal")
            {
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
                    RadWindowManager1.RadAlert("请选择要处理的项！", 250, 150, "提示", "");
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
                GridEditableItem editItem = (GridEditableItem)e.Item;
                string keyId = editItem.GetDataKeyValue("AlarmUid").ToString();
                GridDataItem item = e.Item as GridDataItem;
                DataRowView drv = e.Item.DataItem as DataRowView;
                if (item["AlarmGradeUid"] != null)
                {
                    GridTableCell colorCell = (GridTableCell)item["AlarmGradeUid"];
                    colorCell.BackColor = System.Drawing.Color.Red;
                    colorCell.Text = string.Empty;
                }
                if (item["dealFlag"] != null)
                {
                    GridTableCell dealFlag = (GridTableCell)item["dealFlag"];
                    if ((Convert.IsDBNull(dealFlag.Text)) || (dealFlag.Text.Trim() == "&nbsp;") || dealFlag.Text.Trim() == "0")
                    {
                        dealFlag.Text = "未处理";
                    }
                    else
                    {
                        dealFlag.Text = "已处理";
                    }
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
                    }
                }
                DataView dv = new AlarmDAL().GetAlarmInfoByAlarmId(keyId);
                if (dv != null && dv.Count > 0)
                {
                    if (dv[0]["auditFlag"] != DBNull.Value)
                    {
                        if (dv[0]["auditFlag"].ToString() == "True")
                        {
                            CheckBox cbx = (CheckBox)(e.Item.Cells[2].FindControl("selAlarmConfig"));
                            cbx.Enabled = false;
                        }
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
            where = string.Format(" ApplicationUid = '{0}' ", SmartEP.Core.Enums.EnumMapping.GetDesc(ApplicationType.Air));

            if (portIds != null && portIds.Length > 0)
            {
                where += string.Format(" AND MonitoringPointUid in ('{0}')", StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), "','"));
            }

            //是否处理
            if (cbxHaveDeal.Checked && !cbxNoDeal.Checked)
            {
                where += " AND dealFlag = 1 ";
            }
            else if (!cbxHaveDeal.Checked && cbxNoDeal.Checked)
            {
                where += " AND (dealFlag = 0 OR dealFlag IS NULL)";
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