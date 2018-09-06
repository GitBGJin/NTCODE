﻿using SmartEP.Utilities.DataTypes.ExtensionMethods;
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
    public partial class PointCbxRsm : System.Web.UI.UserControl
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
        /// ComboBox高度
        /// </summary>
        private Int32 _cbxHeight = 300;
        public Int32 CbxHeight
        {
            set { this._cbxHeight = value; }
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
        /// 默认所有站点不做个性化筛选
        /// </summary>
        private bool _defaultAllPoint = false;
        public bool DefaultAllPoint
        {
            set { this._defaultAllPoint = value; }
        }

        private string _defaultSuper = "0";
        /// <summary>
        /// 传一个参数判断是否为超级站
        /// </summary>
        /// <param name="isSuper">是否为超级站</param>
        /// <returns></returns>
        public void isSuper(string isSuper)
        {
            _defaultSuper = isSuper;
        }

        /// <summary>
        /// 应用类型
        /// </summary>
        private ApplicationType _applicationType = ApplicationType.Air;
        public ApplicationType ApplicationType
        {
            set { this._applicationType = value; }
        }

        /// <summary>
        /// 默认站点选择分类
        /// </summary>
        private RsmPointMode _defaultIPointMode = RsmPointMode.Region;
        public RsmPointMode DefaultIPointMode
        {
            set { this._defaultIPointMode = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            //在所有控件都已初始化且已应用所有外观设置后引发。使用该事件来读取或初始化控件属性。
            base.OnInit(e);
            if (!this.IsPostBack)
            {
                RadCBoxPoint.Width = Unit.Pixel(_cbxWidth);
                RadCBoxPoint.DropDownWidth = Unit.Pixel(_dropDownWidth);
                RadCBoxPoint.Height = Unit.Pixel(_cbxHeight);
                InitPointType();
                if (!_multiSelected)
                {
                    RadButton selectAll = ((RadButton)RadCBoxPoint.Header.FindControl("selectAll"));
                    RadButton inverse = ((RadButton)RadCBoxPoint.Header.FindControl("inverse"));
                    RadButton unselect = ((RadButton)RadCBoxPoint.Header.FindControl("unselect"));
                    selectAll.Visible = false;
                    inverse.Visible = false;
                    unselect.Visible = false;
                }
                if (_defaultSuper == "0")
                {
                    myRSM.BindRSM(RadCBoxPoint, _applicationType, CbxRsmType.Point, _defaultIPointMode, pointNames.Value, "", SessionHelper.Get("UserGuid"), _defaultAllSelected, _defaultAllPoint);
                }
                if (_defaultSuper == "1")
                {
                    myRSM.BindRSM(RadCBoxPoint, _applicationType, CbxRsmType.Point, _defaultIPointMode, pointNames.Value, "", SessionHelper.Get("UserGuid"), _defaultSuper, _defaultAllSelected, _defaultAllPoint);
                }
                if (_defaultSuper == "AirDER")
                {
                    myRSM.BindRSM(RadCBoxPoint, _applicationType, CbxRsmType.Point, _defaultIPointMode, pointNames.Value, "", SessionHelper.Get("UserGuid"), _defaultSuper, _defaultAllSelected, _defaultAllPoint);
                }
                pointNames.Value = myRSM.GetRSMValue(RadCBoxPoint, CbxRsmType.Point, CbxRsmReturnType.Name);
            }
        }

        /// <summary>
        /// 站点分类切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void pointType_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadCBoxPoint.OpenDropDownOnLoad = true;
            //RadioButtonList pointType = sender as RadioButtonList;
            RadioButtonList pointType = ((RadioButtonList)RadCBoxPoint.Header.FindControl("radPointType"));
            string type = pointType.SelectedValue;
            if (_defaultSuper == "0")
            {
                myRSM.BindRSM(RadCBoxPoint, _applicationType, CbxRsmType.Point, TypeConversionExtensions.TryTo<Object, RsmPointMode>(type), myRSM.GetRSMValue(RadCBoxPoint, 0, CbxRsmReturnType.Name), "", SessionHelper.Get("UserGuid"), _defaultAllPoint);
            }
            if (_defaultSuper == "1")
            {
                myRSM.BindRSM(RadCBoxPoint, _applicationType, CbxRsmType.Point, TypeConversionExtensions.TryTo<Object, RsmPointMode>(type), myRSM.GetRSMValue(RadCBoxPoint, 0, CbxRsmReturnType.Name), "", SessionHelper.Get("UserGuid"), _defaultSuper,_defaultAllPoint);

            }
        }

        /// <summary>
        /// 测点选择变化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPointChange_Click(object sender, EventArgs e)
        {
            RadCBoxPoint.OpenDropDownOnLoad = false;
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
        /// 根据区域名称获取下属站点的名称
        /// </summary>
        /// <param name="regionName"></param>
        /// <returns></returns>
        public List<string> GetPointNameByRegion(string regionName)
        {
            return myRSM.GetPointNameByRegion(regionName);
        }

        /// <summary>
        /// 取得选中测点
        /// </summary>
        /// <returns></returns>
        public List<IPoint> GetPoints()
        {
            return myRSM.GetRSMPointValue(RadCBoxPoint);
        }

        /// <summary>
        /// 取得所有测点
        /// </summary>
        /// <returns></returns>
        public List<IPoint> GetAllPoints()
        {
            return myRSM.GetAllPointValue(RadCBoxPoint);
        }

        /// <summary>
        /// 取得测点选中值
        /// </summary>
        /// <returns></returns>
        public string[] GetPointValues(CbxRsmReturnType returnType)
        {
            string guids = GetPointValuesStr(returnType);
            if (string.IsNullOrEmpty(guids))
                return null;
            return guids.Split(';');
        }

        /// <summary>
        /// 取得测点选中值
        /// </summary>
        /// <returns></returns>
        public string GetPointValuesStr(CbxRsmReturnType returnType)
        {
            return myRSM.GetRSMValue(RadCBoxPoint, CbxRsmType.Point, returnType);
        }

        /// <summary>
        /// 设置默认点位
        /// </summary>
        /// <param name="points"></param>
        public void SetPointValuesFromNames(string points)
        {
            pointNames.Value = points;
            myRSM.SetSelect(RadCBoxPoint, points);
            //myRSM.BindRSM(RadCBoxPoint, _applicationType, CbxRsmType.Point, RsmPointMode.Property, pointNames.Value, "", "94aa9ad5-8e83-4566-ada5-1c3a45b01175");
        }

        #region << 私有方法 >>
        private void InitPointType()
        {
            RadioButtonList radPointType = ((RadioButtonList)RadCBoxPoint.Header.FindControl("radPointType"));
            if (_applicationType == ApplicationType.Air)
            {
                radPointType.Items.Add(new ListItem("区域", RsmPointMode.Region.ToString()));
                radPointType.Items.Add(new ListItem("类型", RsmPointMode.Type.ToString()));                
                //radPointType.Items.Add(new ListItem("降水降尘", RsmPointMode.Property.ToString()));
            }
            else if (_applicationType == ApplicationType.Water)
            {
                radPointType.Items.Add(new ListItem("类型", RsmPointMode.Type.ToString()));
                radPointType.Items.Add(new ListItem("级别", RsmPointMode.Class.ToString()));
                radPointType.Items.Add(new ListItem("区域", RsmPointMode.Region.ToString()));
                radPointType.Items.Add(new ListItem("运维商", RsmPointMode.Business.ToString()));
                radPointType.Items.Add(new ListItem("属性", RsmPointMode.Property.ToString()));

            }
            else
            {
                radPointType.Items.Add(new ListItem("蓝藻", RsmPointMode.Type.ToString()));

            }
            radPointType.SelectedValue = _defaultIPointMode.ToString();
        }
        #endregion

    }
}