using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmartEP.Core.Enums;
using SmartEP.Core.Interfaces;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.DataAuditing.AuditInterfaces;
using SmartEP.Utilities.Office;
using Telerik.Web.UI;
using SmartEP.Core.Generic;

namespace SmartEP.WebUI.Pages.EnvAir.Audit
{
    /// <summary>
    /// 名称：AuditLog.aspx.cs
    /// 创建人：徐龙超
    /// 创建日期：2015-10-10
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：审核日志查询
    /// 虚拟分页类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class AuditLog : SmartEP.WebUI.Common.BasePage
    {
        AuditLogService auditlogService = new AuditLogService();
        //因子
        SmartEP.Service.BaseData.Channel.AirPollutantService m_AirPollutantService = Singleton<SmartEP.Service.BaseData.Channel.AirPollutantService>.GetInstance();
        /// <summary>
        /// 选择因子
        /// </summary>
        private IList<IPollutant> factors = null;

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

        #region 初始化控件
        private void InitControl()
        {
            dtpBegin.SelectedDate = DateTime.Now.AddDays(-1);
            dtpEnd.SelectedDate = DateTime.Now;
            //rbtnlType.Items.Add(new ListItem("站点", "PointId"));
            rbtnlType.Items.Add(new ListItem("仪器", "Instrument"));
            rbtnlType.SelectedValue = "Instrument";
            //因子控件
            foreach (RadComboBoxItem item in factorCom.Items)
            {
                item.Checked = true;
            }
        }
        #endregion

        #region 事件
        protected void auditLogGrid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
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
            auditLogGrid.Rebind();
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
                //auditLogGrid
                //ExcelHelper.DataTableToExcel(auditLogGrid.DataSource, "数据查询", "数据查询", this.Page);
            }
        }
        #endregion

        #region 方法
        #region Grid绑定
        private void BindGrid()
        {
            if (!IsPostBack)
            {
                //因子关联
                pointCbxRsm_SelectedChanged();
            }
            string[] factorCodes, portIds;
            //string[] portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
            if (rbtnlType.SelectedValue == "PointId")
            {
                factorCodes = factorCbxRsm.GetFactorValues(SmartEP.Core.Enums.CbxRsmReturnType.Code);
                portIds = pointCbxRsm.GetPointValues(SmartEP.Core.Enums.CbxRsmReturnType.ID);
            }
            else
            {
                factorCodes = factorCbxRsm.GetFactorValues(SmartEP.Core.Enums.CbxRsmReturnType.Code);
                portIds = new string[]{"204"};
            }
            DateTime dtBegion = dtpBegin.SelectedDate.Value;
            DateTime dtEnd = dtpEnd.SelectedDate.Value;
            
            factors = factorCbxRsm.GetFactors();
            var auditLogData = auditlogService.RerieveAirLog(portIds, factorCodes, dtBegion, dtEnd,Session["UserGuid"].ToString()).OrderByDescending(x=>x.tstamp);
            
            auditLogGrid.DataSource = auditLogData;
        }
        #endregion

        /// <summary>
        /// 站点因子联动
        /// </summary>
        protected void pointCbxRsm_SelectedChanged()
        {
            points = pointCbxRsm.GetPoints();
            InstrumentChannelService m_InstrumentChannelService = new InstrumentChannelService();
            IList<string> list = new List<string>();
            string[] factor;
            string factors = string.Empty;
            foreach (IPoint point in points)
            {
                IQueryable<PollutantCodeEntity> p = m_InstrumentChannelService.RetrieveChannelListByPointUid(point.PointGuid);
                list = list.Union(p.Select(t => t.PollutantName)).ToList();
            }
            factor = list.ToArray();
            foreach (string f in factor)
            {
                factors += f + ";";
            }
            factorCbxRsm.SetFactorValuesFromNames(factors);
        }
        #endregion

        /// <summary>
        /// 数据导出格式化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void auditLogGrid_GridExporting(object sender, GridExportingArgs e)
        {
            if (e.ExportType == ExportType.Excel || e.ExportType == ExportType.Word)
            {
                string css = "<style> td { border:solid 0.1pt #000000; }</style>";
                e.ExportOutput = e.ExportOutput.Replace("</head>", css + "</head>");
            }
        }

        protected void auditLogGrid_ItemDataBound(object sender, GridItemEventArgs e)
        {
                try
                {
                    RadGrid myRadGrid = ((RadGrid)sender);
                    if (e.Item is GridDataItem && myRadGrid.MasterTableView.DataSourceCount > 0)
                    {
                        GridDataItem item = (GridDataItem)e.Item;
                        foreach (GridColumn col in myRadGrid.MasterTableView.RenderColumns)
                        {
                            string CurrUName = col.UniqueName;
                            if (CurrUName.Equals("数据时间") && col.Visible == true)
                            {
                                AuditLogInfo log = (AuditLogInfo)item.DataItem;
                                TableCell cell = item[CurrUName];
                                if (log.AuditReason == "提交审核")
                                {
                                    cell.Text = Convert.ToDateTime(log.tstamp).ToString("yyyy/MM/dd");
                                }
                                else
                                {
                                    cell.Text = Convert.ToDateTime(log.tstamp).ToString();
                                }
                            }
                            if (CurrUName.Equals("因子") && col.Visible == true)
                            {
                                AuditLogInfo log = (AuditLogInfo)item.DataItem;
                                TableCell cell = item[CurrUName];
                                if (log.AuditPollutantDataValue.Split(',').Length > 3)
                                {
                                    cell.Text = log.AuditPollutantDataValue.Substring(0, log.AuditPollutantDataValue.IndexOf(",",0));
                                }
                                else
                                {
                                    cell.Text = log.AuditPollutantDataValue;
                                }
                                cell.ToolTip = log.AuditPollutantDataValue;
                            }
                        }
                    }
                }
                catch
                {
                }
        }

        protected void rbtnlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rbtnlType.SelectedValue == "PointId")
            {
                pointCbxRsm.Visible = true;
                factorCom.Visible = false;
            }
            else if (rbtnlType.SelectedValue == "Instrument")
            {
                factorCom.Visible = true;
                pointCbxRsm.Visible = false;
            }
        }

        protected void factorCom_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            string str = factorCom.Text;
            string PollutantCode = "";
            string PCode = "";
            if (str.Contains("黑碳分析仪"))
            {
                BindFactors("e5b6d666-24d1-473a-b15a-33a36245d44f", PCode, out PollutantCode);
                PCode = PollutantCode;
            }
            if (str.Contains("离子色谱仪"))
            {
                BindFactors("5575a0e1-d948-4566-9dcd-4b4767688add", PCode, out PollutantCode);
                PCode = PollutantCode;
            }
            if (str.Contains("太阳辐射仪"))
            {
                BindFactors("aabe91e0-29a4-427c-becc-0b29f1224422", PCode, out PollutantCode);
                PCode = PollutantCode;
            }
            if (str.Contains("粒径谱仪"))
            {
                BindFactors("da92c7c1-4932-4007-a6d5-2866aa8c63f1", PCode, out PollutantCode);
                PCode = PollutantCode;
            }
            if (str.Contains("常规参数"))
            {
                BindFactors("339B72C4-7295-4D31-B9EB-23342CB3697E", PCode, out PollutantCode);
                PCode = PollutantCode;
            }
            if (str.Contains("EC/OC有机碳元素碳"))
            {
                BindFactors("14b38adf-d899-4362-99ff-6a9e9dd35485", PCode, out PollutantCode);
                PCode = PollutantCode;
            }
            if (str.Contains("VOCs"))
            {
                BindFactors("fbc6dyta-d06c-678g-b5y0-89b12be5bda3", PCode, out PollutantCode);
                PCode = PollutantCode;
            }
            if (str.Contains("三波长浊度仪"))
            {
                BindFactors("59f02681-093f-48f0-9cac-ac59acd7038f", PCode, out PollutantCode);
                PCode = PollutantCode;
            }
            
            factorCbxRsm.SetFactorValuesFromCodes(PollutantCode.TrimEnd(';'));
        }
        #region 绑定因子
        public void BindFactors(string CategoryUid, string PCode, out string code, string type = "name")
        {
            IQueryable<PollutantCodeEntity> Pollutant = m_AirPollutantService.RetrieveList().Where(x => x.CategoryUid == CategoryUid);
            //string PollutantCode = "";
            string[] pollutantCodearry = Pollutant.Select(p => p.PollutantCode).ToArray();
            foreach (string strName in pollutantCodearry)
            {
                PCode += strName + ";";
            }
            code = PCode;
        }
        #endregion
    }
}