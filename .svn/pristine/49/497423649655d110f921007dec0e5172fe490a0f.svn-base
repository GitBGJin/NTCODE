using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SmartEP.Core.Generic;
using SmartEP.DomainModel;
using SmartEP.DomainModel.BaseData;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.Service.AutoMonitoring.common;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.BaseData.Standard;
using SmartEP.Data.SqlServer.MonitoringBusiness.Air;
using System.Data;
using SmartEP.Core.Enums;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System.Collections;
using SmartEP.Service.BaseData.BusinessRule;
using SmartEP.Utilities.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.DataAnalyze
{
    /// <summary>
    /// 名称：ExcessiveSetting.aspx.cs
    /// 创建人：
    /// 创建日期：
    /// 维护人员：
    /// 最新维护人员：吕云
    /// 最新维护日期：2017-5-23
    /// 功能摘要：超标配置列表
    /// 虚拟分页类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class ExcessiveSetting : SmartEP.WebUI.Common.BasePage
    {
        ExcessiveSettingDAL excessiveSet = new ExcessiveSettingDAL();
        ExcessiveSettingService Service = new ExcessiveSettingService();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (!string.IsNullOrWhiteSpace(PageHelper.GetQueryString("RuleType")))//审核/报警
                {
                    string RuleType = PageHelper.GetQueryString("RuleType");
                    this.ViewState["RuleType"] = RuleType;
                }
                SetInitialInfo();
                if (Request.QueryString["ObjectType"] != null)
                {
                    ViewState["ObjectType"] = Request.QueryString["ObjectType"].ToString();
                    if (ViewState["ObjectType"].ToString() == "2")
                    {
                        divAir.Visible = true;
                        FactorAir.Visible = true;
                        divWater.Visible = false;
                        FactorWater.Visible = false;
                        Application.Value = "airaaira-aira-aira-aira-airaairaaira";
                    }
                    else
                    {
                        divAir.Visible = false;
                        FactorAir.Visible = false;
                        divWater.Visible = true;
                        FactorWater.Visible = true;
                        Application.Value = "watrwatr-watr-watr-watr-watrwatrwatr";
                    }
                }
                else
                {
                    divAir.Visible = true;
                    FactorAir.Visible = true;
                    divWater.Visible = false;
                    FactorWater.Visible = false;
                    Application.Value = "airaaira-aira-aira-aira-airaairaaira";
                }
                BindData();
            }
        }
        #region 绑定下拉框
        public void SetInitialInfo()
        {
            DataView codeDV = excessiveSet.GetCode();
            if (codeDV.Count > 0)
            {
                codeDV.RowFilter = string.Format("mainguid='{0}'", "0d24484d-d315-4f12-b2bc-f64552c4f6dd");//数据类型
                if (codeDV.Count > 0)
                {
                    for (int i = 0; i < codeDV.Count; i++)
                    {
                        RadComboBoxItem item = new RadComboBoxItem();
                        item.Text = codeDV[i]["itemText"].ToString();
                        item.Value = codeDV[i]["rowguid"].ToString();
                        cmbDataType.Items.Add(item);
                    }
                }
                RadComboBoxItem selectedItem1 = new RadComboBoxItem();
                selectedItem1.Text = "所有选项";
                selectedItem1.Value = string.Empty;
                cmbDataType.Items.Insert(0, selectedItem1);
                cmbDataType.DataBind();
                cmbDataType.SelectedValue = string.Empty;

                codeDV.RowFilter = string.Format("mainguid='{0}'", "5e9b42d6-97a0-4e1a-8cf5-355d271b52ef");//通知级别
                if (codeDV.Count > 0)
                {
                    for (int i = 0; i < codeDV.Count; i++)
                    {
                        RadComboBoxItem item = new RadComboBoxItem();
                        item.Text = codeDV[i]["itemText"].ToString();
                        item.Value = codeDV[i]["rowguid"].ToString();
                        cmbNotifyGrade.Items.Add(item);
                    }
                }
                RadComboBoxItem selectedItem2 = new RadComboBoxItem();
                selectedItem2.Text = "所有选项";
                selectedItem2.Value = string.Empty;
                cmbNotifyGrade.Items.Insert(0, selectedItem2);
                cmbNotifyGrade.DataBind();
                cmbNotifyGrade.SelectedValue = string.Empty;

                codeDV.RowFilter = string.Format("mainguid='{0}'", "3e8f63ea-64ea-4c23-8aba-c369129dde13");//规则用途类型
                codeDV.Sort = "ItemValue";
                if (codeDV.Count > 0)
                {
                    for (int i = 0; i < codeDV.Count; i++)
                    {
                        RadComboBoxItem item = new RadComboBoxItem();
                        item.Text = codeDV[i]["itemText"].ToString();
                        item.Value = codeDV[i]["rowguid"].ToString();
                        cmbUseFor.Items.Add(item);
                    }
                }
                RadComboBoxItem selectedItem3 = new RadComboBoxItem();
                selectedItem3.Text = "所有选项";
                selectedItem3.Value = string.Empty;
                cmbUseFor.Items.Insert(0, selectedItem3);
                cmbUseFor.DataBind();
                if (this.ViewState["RuleType"] != null)
                {
                    hdType.Value = this.ViewState["RuleType"].ToString();
                    if (this.ViewState["RuleType"].ToString() == "1")
                    {
                        cmbUseFor.SelectedValue = codeDV[0]["rowguid"].ToString();
                    }
                    else if (this.ViewState["RuleType"].ToString() == "2")
                    {
                        cmbUseFor.SelectedValue = codeDV[1]["rowguid"].ToString();
                    }
                }

            }
        }
        #endregion

        #region 数据绑定
        public void BindData()
        {

            string DataType = cmbDataType.SelectedValue.ToString();
            string NotifyGrade = cmbNotifyGrade.SelectedValue.ToString();
            string UseFor = cmbUseFor.SelectedValue.ToString();
            string[] portIds = null;
            string[] factorCodes = null;
            if (ViewState["ObjectType"] != null)
            {
                if (ViewState["ObjectType"].ToString() == "2")
                {
                    portIds = pointCbxRsmA.GetPointValues(CbxRsmReturnType.Guid);
                    factorCodes = factorCbxRsmA.GetFactorValues(SmartEP.Core.Enums.CbxRsmReturnType.Code);
                }
                else
                {
                    portIds = pointCbxRsmW.GetPointValues(CbxRsmReturnType.Guid);
                    factorCodes = factorCbxRsmW.GetFactorValues(SmartEP.Core.Enums.CbxRsmReturnType.Code);
                }
            }
            else
            {
                portIds = pointCbxRsmA.GetPointValues(CbxRsmReturnType.Guid);
                factorCodes = factorCbxRsmA.GetFactorValues(SmartEP.Core.Enums.CbxRsmReturnType.Code);
            }

            DataView excessiveSetDV = excessiveSet.GetExcessiveSettingData(DataType, NotifyGrade, UseFor, StringExtensions.GetArrayStrNoEmpty(portIds.ToList<string>(), "','"), string.Empty, StringExtensions.GetArrayStrNoEmpty(factorCodes.ToList<string>(), "','"));
            DataView codeDV = excessiveSet.GetCode();
            //新建表
            DataTable ExcSetDT = CreateDayRptDataTable();

            if (excessiveSetDV.Count > 0)
            {
                for (int i = 0; i < excessiveSetDV.Count; i++)
                {
                    DataRow dr = ExcSetDT.NewRow();
                    dr["ExcessiveUid"] = excessiveSetDV[i]["ExcessiveUid"];
                    dr["MonitoringPointName"] = excessiveSetDV[i]["MonitoringPointName"];
                    dr["PollutantName"] = excessiveSetDV[i]["PollutantName"];
                    if (excessiveSetDV[i]["DataTypeUid"] != DBNull.Value)
                    {
                        codeDV.RowFilter = string.Format("rowguid='{0}'", excessiveSetDV[i]["DataTypeUid"].ToString());
                        if (codeDV.Count > 0)
                        {
                            dr["DataType"] = codeDV[0]["itemText"];
                        }
                    }
                    if (excessiveSetDV[i]["NotifyGradeUid"] != DBNull.Value)
                    {
                        codeDV.RowFilter = string.Format("rowguid='{0}'", excessiveSetDV[i]["NotifyGradeUid"].ToString());
                        if (codeDV.Count > 0)
                        {
                            dr["NotifyGrade"] = codeDV[0]["itemText"];
                        }
                    }
                    if (excessiveSetDV[i]["UseForUid"] != DBNull.Value)
                    {
                        codeDV.RowFilter = string.Format("rowguid='{0}'", excessiveSetDV[i]["UseForUid"].ToString());
                        if (codeDV.Count > 0)
                        {
                            dr["UseFor"] = codeDV[0]["itemText"];
                        }
                    }
                    dr["ExcessiveUpper"] = excessiveSetDV[i]["ExcessiveUpper"] != DBNull.Value ? Convert.ToDecimal(excessiveSetDV[i]["ExcessiveUpper"]).ToString() : null;

                    dr["ExcessiveLow"] = excessiveSetDV[i]["ExcessiveLow"] != DBNull.Value ? Convert.ToDecimal(excessiveSetDV[i]["ExcessiveLow"]).ToString() : null;

                    if (excessiveSetDV[i]["EnableOrNot"] != DBNull.Value)
                    {
                        dr["EnableOrNot"] = excessiveSetDV[i]["EnableOrNot"].ToString() == "True" ? "是" : "否";
                    }
                    ExcSetDT.Rows.Add(dr);
                }
            }
            RadGrid1.DataSource = ExcSetDT;
        }

        //新建数据表
        private DataTable CreateDayRptDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ExcessiveUid", typeof(string));
            dt.Columns.Add("MonitoringPointName", typeof(string));
            dt.Columns.Add("PollutantName", typeof(string));
            dt.Columns.Add("DataType", typeof(string));
            dt.Columns.Add("NotifyGrade", typeof(string));
            dt.Columns.Add("UseFor", typeof(string));
            dt.Columns.Add("ExcessiveUpper", typeof(string));
            dt.Columns.Add("ExcessiveLow", typeof(string));
            dt.Columns.Add("EnableOrNot", typeof(string));
            return dt;
        }
        protected void RadGrid1_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            BindData();
        }
        #endregion

        #region 编辑
        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
            {

                GridEditableItem editItem = (GridEditableItem)e.Item;
                string ExcessiveUid = editItem.GetDataKeyValue("ExcessiveUid").ToString();
                string cmd = string.Format("openArtificialRounding('{0}')", ExcessiveUid);
                e.Item.Attributes.Add("ondblclick", cmd);
            }
        }
        #endregion

        #region 删除
        //protected void btnDelete_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        for (int i = 0; i < RadGrid1.Items.Count; i++)
        //        {
        //            GridEditableItem editItem = (GridEditableItem)RadGrid1.Items[i];
        //            System.Web.UI.WebControls.CheckBox sel = (System.Web.UI.WebControls.CheckBox)editItem.FindControl("chkSel");
        //            if (sel.Checked)
        //            {
        //                string ExcessiveUid = editItem.GetDataKeyValue("ExcessiveUid").ToString();
        //                excessiveSet.DeleteByExcessiveUid(ExcessiveUid);
        //            }
        //        }
        //        Alert("删除成功！");
        //        RadGrid1.Rebind();
        //    }
        //    catch (Exception ee)
        //    {
        //        Alert(ee.ToString());
        //    }
        //}

        /// <summary>
        /// ToolBar按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grid_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                int selItems = 0;
                RadGrid radGrid = ((RadGrid)sender);
                int Index = -1;
                switch (e.CommandName)
                {
                    case "DeleteSelected":
                        #region DeleteSelected
                        foreach (String item in radGrid.SelectedIndexes)
                        {
                            selItems++;
                            Index = Convert.ToInt32(item);
                            string ExcessiveUid = radGrid.MasterTableView.DataKeyValues[Index]["ExcessiveUid"].ToString();
                            excessiveSet.DeleteByExcessiveUid(ExcessiveUid);
                        }
                        if (selItems > 0)
                        {
                            base.Alert("删除成功！");
                        }
                        #endregion
                        break;
                }
            }
            catch (Exception ee)
            {
                Alert(ee.ToString());
            }
        }

        #endregion
        #region 查询
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            RadGrid1.Rebind();
        }
        #endregion
    }
}