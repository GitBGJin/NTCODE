using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Service.Core.Enums;
using SmartEP.WebControl.CbxRsm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SmartEP.Core.Enums;
using SmartEP.Core.Interfaces;
using SmartEP.Utilities.Caching;

namespace SmartEP.WebUI.Controls
{
    public partial class FactorCbxRsm : System.Web.UI.UserControl
    {
        /// <summary>  
        /// 定义委托  
        /// </summary>  
        /// <param name="sender"></param>  
        /// <param name="e"></param>  
        public delegate void SelectedChangeHandler();
        public event SelectedChangeHandler SelectedChanged;
        CbxRsmControl myRSM = new CbxRsmControl();

        /// <summary>
        /// ComboBox宽度
        /// </summary>
        private Int32 _cbxWidth = 100;
        public Int32 CbxWidth
        {
            set { this._cbxWidth = value; }
        }
        /// <summary>
        /// 下拉宽度
        /// </summary>
        private Int32 _dropDownWidth = 200;
        public Int32 DropDownWidth
        {
            set { this._dropDownWidth = value; }
        }

        /// <summary>
        /// 复选
        /// </summary>
        private bool _multiSelected = true;
        public bool MultiSelected
        {
            set { this._multiSelected = value; }
        }

        /// <summary>
        /// 默认全选
        /// </summary>
        private bool _defaultAllSelected = false;
        public bool DefaultAllSelected
        {
            set { this._defaultAllSelected = value; }
        }

        /// <summary>
        /// 默认所有因子不做个性化筛选
        /// </summary>
        private bool _defaultAllPollutant = false;
        public bool DefaultAllPollutant
        {
            set { this._defaultAllPollutant = value; }
        }
        /// <summary>
        /// 设置默认参数为0
        /// </summary>
        private string _defaultAudit = "0";
        /// <summary>
        /// 传一个参数判断是否为超级站
        /// </summary>
        /// <param name="isSuper">是否为超级站</param>
        /// <returns></returns>
        public void isAudit(string isAudit)
        {
            _defaultAudit = isAudit;
        }

        /// <summary>
        /// 应用类型
        /// </summary>
        private ApplicationType _applicationType = ApplicationType.Air;
        public ApplicationType ApplicationType
        {
            set { this._applicationType = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            //在所有控件都已初始化且已应用所有外观设置后引发。使用该事件来读取或初始化控件属性。
            base.OnInit(e);
            if (!this.IsPostBack)
            {
                RadCBoxFactor.Width = Unit.Pixel(_cbxWidth);
                RadCBoxFactor.DropDownWidth = Unit.Pixel(_dropDownWidth);
                if (!_multiSelected)
                {
                    RadButton selectAll = ((RadButton)RadCBoxFactor.Header.FindControl("selectAll"));
                    RadButton inverse = ((RadButton)RadCBoxFactor.Header.FindControl("inverse"));
                    RadButton unselect = ((RadButton)RadCBoxFactor.Header.FindControl("unselect"));
                    selectAll.Visible = false;
                    inverse.Visible = false;
                    unselect.Visible = false;
                }
                if (_defaultAudit == "0")
                {
                    myRSM.BindRSM(RadCBoxFactor, _applicationType, CbxRsmType.ChannelFactor, RsmPointMode.Type, factorNames.Value, "", SessionHelper.Get("UserGuid"), _defaultAllSelected, _defaultAllPollutant);
                }
                else
                {
                    myRSM.BindRSM(RadCBoxFactor, _applicationType, CbxRsmType.ChannelFactor, RsmPointMode.Type, factorNames.Value, "", SessionHelper.Get("UserGuid"), _defaultAudit, "", _defaultAllSelected, _defaultAllPollutant);
                }
                factorNames.Value = myRSM.GetRSMValue(RadCBoxFactor, CbxRsmType.ChannelFactor, CbxRsmReturnType.Name);
            }

        }

        /// <summary>
        /// 因子选择变化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnFactorChange_Click(object sender, EventArgs e)
        {
            //将自定义事件绑定到控件事件上  
            if (SelectedChanged != null)
            {
                //RadCBoxPoint
                SelectedChanged();
            }
        }

        /// <summary>
        /// 各子节点绑定处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RSM_NodeDataBound(object sender, RadSiteMapNodeEventArgs e)
        {
            RadSiteMapNode node = e.Node;
            if (node.Level == 0)
            {
                if (node.FindControl("RsmChkA") != null)
                {
                    CheckBox routeChb = node.FindControl("RsmChkA") as CheckBox;
                    routeChb.Attributes.Remove("onclick");
                    if (_multiSelected)
                    {
                        routeChb.Enabled = true;
                        routeChb.Attributes.Add("onclick", "onSelectParentNode(this,'multi')");
                    }
                    else
                    {
                        routeChb.Enabled = false;
                        routeChb.Attributes.Add("onclick", "onSelectParentNode(this,'single')");
                    }
                }
            }
            else if (node.Level == 1)
            {
                if (node.FindControl("RsmChkB") != null)
                {
                    CheckBox routeChb = node.FindControl("RsmChkB") as CheckBox;
                    routeChb.Attributes.Remove("onclick");
                    if (_multiSelected)
                    {
                        routeChb.Attributes.Add("onclick", "onSelectParentNode(this,'multi')");
                    }
                    else
                    {
                        routeChb.Attributes.Add("onclick", "onSelectParentNode(this,'single')");
                    }
                }
            }
        }

        /// <summary>
        /// 取得因子选中值
        /// </summary>
        /// <returns></returns>
        public List<IPollutant> GetFactors()
        {
            return myRSM.GetRSMFactorValue(RadCBoxFactor);
        }

        /// <summary>
        /// 取得因子选中值
        /// </summary>
        /// <returns></returns>
        public string[] GetFactorValues(CbxRsmReturnType returnType)
        {
            string guids = GetFactorValuesStr(returnType);
            if (string.IsNullOrEmpty(guids))
                return null;
            return guids.Split(';');
        }

        /// <summary>
        /// 取得因子选中值
        /// </summary>
        /// <returns></returns>
        public string GetFactorValuesStr(CbxRsmReturnType returnType)
        {
            return myRSM.GetRSMValue(RadCBoxFactor, CbxRsmType.ChannelFactor, returnType);
        }

        /// <summary>
        /// 设置默认因子
        /// </summary>
        /// <param name="points"></param>
        public void SetFactorValuesFromNames(string factors)
        {
            factorNames.Value = factors;
            myRSM.SetSelect(RadCBoxFactor, factors);
            //myRSM.BindRSM(RadCBoxFactor, _applicationType, CbxRsmType.ChannelFactor, RsmPointMode.Type, factorNames.Value, "", "94aa9ad5-8e83-4566-ada5-1c3a45b01175");
        }

        public void SetFactorValuesFromCodes(string factorCodes)
        {
            factorNames.Value = factorCodes;
            myRSM.SetSelectNew(RadCBoxFactor, factorCodes);
            //myRSM.BindRSM(RadCBoxFactor, _applicationType, CbxRsmType.ChannelFactor, RsmPointMode.Type, factorNames.Value, "", "94aa9ad5-8e83-4566-ada5-1c3a45b01175");
        }
    }
}