using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmartEP.Core.Enums;
using SmartEP.Core.Interfaces;
using SmartEP.Data.SqlServer.MonitoringBusiness.Air;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Utilities.Caching;
using Telerik.Web.UI;
using SmartEP.Utilities.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.DataAnalyze
{
    /// <summary>
    /// 名称：ExcessiveSetEdit.aspx.cs
    /// 创建人：
    /// 创建日期：
    /// 维护人员：
    /// 最新维护人员：吕云
    /// 最新维护日期：2017-5-23
    /// 功能摘要：超标配置
    /// 虚拟分页类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class ExcessiveSetEdit : SmartEP.WebUI.Common.BasePage
    {
        ExcessiveSettingDAL excessiveSet = new ExcessiveSettingDAL();

        /// <summary>
        /// 选择站点
        /// </summary>
        private IList<IPoint> points = null;

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
                
                if (Request.QueryString["ExcessiveUid"] != null)//编辑
                {
                    ViewState["ExcessiveUid"] = Request.QueryString["ExcessiveUid"].ToString();
                    BindData(ViewState["ExcessiveUid"].ToString());
                }
                else 
                {
                    rbtEnableOrNot.SelectedIndex = 1;
                    rbtNotifyOrNot.SelectedIndex = 1;
                    if (Request.QueryString["ApplicationUid"] != null)//新增
                    {
                        ViewState["ApplicationUid"] = Request.QueryString["ApplicationUid"].ToString();
                        if (ViewState["ApplicationUid"].ToString() == "airaaira-aira-aira-aira-airaairaaira")
                        {
                            div1.Visible = true;
                            div3.Visible = true;
                            div2.Visible = false;
                            div4.Visible = false;
                        }
                        else
                        {
                            div1.Visible = false;
                            div3.Visible = false;
                            div2.Visible = true;
                            div4.Visible = true;
                        }
                    }
                }
            }
        }
        #region 数据绑定
        public void BindData(string ExcessiveUid)
        {
            string useFor=string.Empty;
            DataView excessiveSetDV = excessiveSet.GetExcessiveSettingData(string.Empty, string.Empty, string.Empty, string.Empty, ExcessiveUid, string.Empty);
            ViewState["ApplicationUid"] = excessiveSetDV[0]["ApplicationUid"].ToString();
            if (excessiveSetDV.Count > 0)
            {
                if (excessiveSetDV[0]["ApplicationUid"].ToString() == "airaaira-aira-aira-aira-airaairaaira")
                {
                    div1.Visible = true;
                    div3.Visible = true;
                    div2.Visible = false;
                    div4.Visible = false;
                    pointCbxRsmA.SetPointValuesFromNames(excessiveSetDV[0]["MonitoringPointName"].ToString());
                    factorCbxRsmA.SetFactorValuesFromNames(excessiveSetDV[0]["PollutantName"].ToString());
                }
                else
                {
                    div2.Visible = true;
                    div4.Visible = true;
                    div1.Visible = false;
                    div3.Visible = false;
                    pointCbxRsmW.SetPointValuesFromNames(excessiveSetDV[0]["MonitoringPointName"].ToString());
                    factorCbxRsmW.SetFactorValuesFromNames(excessiveSetDV[0]["PollutantName"].ToString());
                }
                
                dataTypeCmb.SelectedValue = excessiveSetDV[0]["DataTypeUid"].ToString();
                notifyGradeCmb.SelectedValue = excessiveSetDV[0]["NotifyGradeUid"].ToString();
                tbxAdvanceRange.Text = excessiveSetDV[0]["AdvanceRange"].ToString();
                tbxAdvanceLow.Text = excessiveSetDV[0]["AdvanceLow"].ToString();
                tbxAdvanceUpper.Text = excessiveSetDV[0]["AdvanceUpper"].ToString();
                tbxExcessiveRange.Text = excessiveSetDV[0]["ExcessiveRange"].ToString();
                tbxExcessiveRatio.Text = excessiveSetDV[0]["ExcessiveRatio"].ToString();
                useForCmb.SelectedValue = excessiveSetDV[0]["UseForUid"].ToString();
                tbxExcessiveUpper.Text = excessiveSetDV[0]["ExcessiveUpper"].ToString();
                tbxExcessiveLow.Text = excessiveSetDV[0]["ExcessiveLow"].ToString();
                tbxReplaceStatus.Text = excessiveSetDV[0]["ReplaceStatus"].ToString();
                tbxStandardType.Text = excessiveSetDV[0]["StandardType"].ToString();
                rbtEnableOrNot.SelectedIndex = excessiveSetDV[0]["EnableOrNot"].ToString() == "True" ? 1 : 0;
                rbtNotifyOrNot.SelectedIndex = excessiveSetDV[0]["NotifyOrNot"].ToString() == "True" ? 1 : 0;
                tbxDescription.Text = excessiveSetDV[0]["Description"].ToString();
            }
        }
        #endregion

        public void SetInitialInfo()
        {
            #region 绑定下拉框
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
                        dataTypeCmb.Items.Add(item);
                    }
                }
                RadComboBoxItem selectedItem1 = new RadComboBoxItem();
                selectedItem1.Text = "-请选择-";
                selectedItem1.Value = string.Empty;
                dataTypeCmb.Items.Insert(0, selectedItem1);
                dataTypeCmb.DataBind();
                dataTypeCmb.SelectedValue = string.Empty;

                codeDV.RowFilter = string.Format("mainguid='{0}'", "5e9b42d6-97a0-4e1a-8cf5-355d271b52ef");//通知级别
                if (codeDV.Count > 0)
                {
                    for (int i = 0; i < codeDV.Count; i++)
                    {
                        RadComboBoxItem item = new RadComboBoxItem();
                        item.Text = codeDV[i]["itemText"].ToString();
                        item.Value = codeDV[i]["rowguid"].ToString();
                        notifyGradeCmb.Items.Add(item);
                    }
                }
                RadComboBoxItem selectedItem2 = new RadComboBoxItem();
                selectedItem2.Text = "-请选择-";
                selectedItem2.Value = string.Empty;
                notifyGradeCmb.Items.Insert(0, selectedItem2);
                notifyGradeCmb.DataBind();
                notifyGradeCmb.SelectedValue = string.Empty;

                codeDV.RowFilter = string.Format("mainguid='{0}'", "3e8f63ea-64ea-4c23-8aba-c369129dde13");//规则用途类型
                codeDV.Sort = "ItemValue";
                if (codeDV.Count > 0)
                {
                    for (int i = 0; i < codeDV.Count; i++)
                    {
                        RadComboBoxItem item = new RadComboBoxItem();
                        item.Text = codeDV[i]["itemText"].ToString();
                        item.Value = codeDV[i]["rowguid"].ToString();
                        useForCmb.Items.Add(item);
                    }
                }
                RadComboBoxItem selectedItem3 = new RadComboBoxItem();
                selectedItem3.Text = "-请选择-";
                selectedItem3.Value = string.Empty;
                useForCmb.Items.Insert(0, selectedItem3);
                useForCmb.DataBind();
                if (this.ViewState["RuleType"] != null)
                {
                    if (this.ViewState["RuleType"].ToString() == "1")
                    {
                        useForCmb.SelectedValue = codeDV[0]["rowguid"].ToString();
                    }
                    else if (this.ViewState["RuleType"].ToString() == "2")
                    {
                        useForCmb.SelectedValue = codeDV[1]["rowguid"].ToString();
                    }
                }
                //useForCmb.SelectedValue = string.Empty;
            }
            #endregion

            //获取账号信息
            DataView userDV = excessiveSet.GetUserInfo();
            if (userDV.Count > 0)
            {
                ViewState["AddUserGuid"] = SessionHelper.Get("UserGuid");
                ViewState["AddUserName"] = userDV[0]["displayName"].ToString();
                ViewState["AddOUGuid"] = userDV[0]["rowguid"].ToString();
                ViewState["AddOUName"] = userDV[0]["deptName"].ToString();
            }
        }

        /// <summary>
        /// 站点因子联动
        /// </summary>
        /// 
        protected void pointCbxRsmA_SelectedChanged()
        {
            points = pointCbxRsmA.GetPoints();
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
            factorCbxRsmA.SetFactorValuesFromNames(factors);
        }

        protected void pointCbxRsmW_SelectedChanged()
        {
            points = pointCbxRsmW.GetPoints();
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
            factorCbxRsmW.SetFactorValuesFromNames(factors);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string[] portIds = null;
            string[] factorCodes = null;
            string ApplicationUid = ViewState["ApplicationUid"].ToString();
            if (ApplicationUid == "airaaira-aira-aira-aira-airaairaaira")
            {
                portIds = pointCbxRsmA.GetPointValues(CbxRsmReturnType.Guid);//站点
                factorCodes = factorCbxRsmA.GetFactorValues(SmartEP.Core.Enums.CbxRsmReturnType.Code);//因子
            }
            else
            {
                portIds = pointCbxRsmW.GetPointValues(CbxRsmReturnType.Guid);//站点
                factorCodes = factorCbxRsmW.GetFactorValues(SmartEP.Core.Enums.CbxRsmReturnType.Code);//因子
            }
            if (portIds.Length == 0)
            { Alert("请选择站点"); return; }
            if (factorCodes.Length == 0)
            { Alert("请选择因子"); return; }



            string DataTypeUid = string.Empty;
            if (dataTypeCmb.SelectedValue != string.Empty)
            {
                DataTypeUid = dataTypeCmb.SelectedValue.ToString();//数据类型
            }
            else
            { Alert("请选择数据类型"); return; }

            string NotifyGradeUid = string.Empty;
            if (notifyGradeCmb.SelectedValue != string.Empty)
            {
                NotifyGradeUid = notifyGradeCmb.SelectedValue.ToString();//通知级别
            }
            else
            { Alert("请选择通知级别"); return; }

            string UseForUid = string.Empty;
            if (useForCmb.SelectedValue != string.Empty)
            {
                UseForUid = useForCmb.SelectedValue.ToString();//规则用途类型
            }
            else
            { Alert("请选择规则用途类型"); return; }

            string AdvanceRange = tbxAdvanceRange.Text;//警戒范围

            decimal? AdvanceLow = null;
            if (tbxAdvanceLow.Text.ToString() != "")
                AdvanceLow = Convert.ToDecimal(tbxAdvanceLow.Text.ToString());//警戒下限

            decimal? AdvanceUpper = null;
            if (tbxAdvanceUpper.Text.ToString() != "")
                AdvanceUpper = Convert.ToDecimal(tbxAdvanceUpper.Text);//警戒上限

            string ExcessiveRange = tbxExcessiveRange.Text;//超标范围

            decimal? ExcessiveRatio = null;
            if (tbxExcessiveRatio.Text.ToString() != "")
                ExcessiveRatio = Convert.ToDecimal(tbxExcessiveRatio.Text);//超标系数

            decimal? ExcessiveUpper = null;
            if (tbxExcessiveUpper.Text.ToString() != "")
                ExcessiveUpper = Convert.ToDecimal(tbxExcessiveUpper.Text);//超标上限

            decimal? ExcessiveLow = null;
            if (tbxExcessiveLow.Text.ToString() != "")
                ExcessiveLow = Convert.ToDecimal(tbxExcessiveLow.Text);//超标下限

            string ReplaceStatus = tbxReplaceStatus.Text;//替换状态
            string StandardType = tbxStandardType.Text;//限值类别

            int EnableOrNot = Convert.ToInt32(rbtEnableOrNot.SelectedValue);//是否使用
            int NotifyOrNot = Convert.ToInt32(rbtNotifyOrNot.SelectedValue);//是否通知

            string InstrumentChannelsUid = string.Empty;
            string Description = tbxDescription.Text;//备注

            string AddUserGuid = ViewState["AddUserGuid"].ToString();
            string AddUserName = ViewState["AddUserName"].ToString();
            string AddOUGuid = ViewState["AddOUGuid"].ToString();
            string AddOUName = ViewState["AddOUName"].ToString();
            try
            {
                //循环站点和因子
                for (int i = 0; i < portIds.Length; i++)
                {
                    string pointId = portIds[i].ToString();
                    //根据站点查询InstrumentChannelsUid
                    DataView InstrumentChDV = excessiveSet.GetInstrumentChBypoint(pointId);
                    for (int j = 0; j < factorCodes.Length; j++)
                    {
                        string factorCode = factorCodes[j].ToString();
                        //查询InstrumentChannelsUid
                        InstrumentChDV.RowFilter = string.Format("PollutantCode='{0}'", factorCode);
                        if (InstrumentChDV.Count > 0)
                        {
                            InstrumentChannelsUid = InstrumentChDV[0]["InstrumentChannelsUid"].ToString();

                            //判断记录是否存在
                            bool isExist = excessiveSet.IsExistByInfo(pointId, InstrumentChannelsUid, DataTypeUid, NotifyGradeUid, ApplicationUid, UseForUid);
                            if (isExist)
                            {
                                excessiveSet.UpdateExcessiveSet(ApplicationUid, pointId, InstrumentChannelsUid, DataTypeUid, NotifyGradeUid, AdvanceRange, AdvanceLow, AdvanceUpper, ExcessiveRange,
                                      ExcessiveRatio, ExcessiveUpper, ExcessiveLow, ReplaceStatus, StandardType, EnableOrNot, NotifyOrNot, Description, AddUserGuid, AddUserName, AddOUGuid, AddOUName, UseForUid);//更新
                            }
                            else
                            {
                                excessiveSet.InsertExcessiveSet(ApplicationUid, pointId, InstrumentChannelsUid, DataTypeUid, NotifyGradeUid, AdvanceRange, AdvanceLow, AdvanceUpper, ExcessiveRange,
                                      ExcessiveRatio, ExcessiveUpper, ExcessiveLow, ReplaceStatus, StandardType, EnableOrNot, NotifyOrNot, Description, AddUserGuid, AddUserName, AddOUGuid, AddOUName, UseForUid);//插入
                            }
                        }

                    }
                }
                RadAjaxManager1.ResponseScripts.Add("RefreshParentPage()");
            }
            catch (Exception ee)
            {
                RadWindowManager1.RadAlert(ee.Message, 300, 200, "提示", "");
            }

        }

    }
}