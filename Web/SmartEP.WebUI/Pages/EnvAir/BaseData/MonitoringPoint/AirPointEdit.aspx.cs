using SmartEP.DomainModel;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.Core.Enums;
using SmartEP.Service.Frame;
using SmartEP.Utilities.Web.UI;
using SmartEP.WebUI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.BaseData
{
    public partial class AirPointEdit : BasePage
    {
        DictionaryService dicService = new DictionaryService();
        MonitoringPointService pointService = new MonitoringPointService();
        MonitoringPointAirService airPointService = new MonitoringPointAirService();
        AcquisitionInstrumentService acquisitionService = new AcquisitionInstrumentService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                string pointGuid = PageHelper.GetQueryString("MonitoringPointUid");
                if (!string.IsNullOrEmpty(pointGuid))
                {
                    this.ViewState["PointUid"] = pointGuid;
                    InitControl();
                    BindUI();
                }
            }
        }

        /// <summary>
        /// 初始化下拉列表
        /// </summary>
        private void InitControl()
        {
            //行政区划
            IQueryable<V_CodeMainItemEntity> regionEntites = dicService.RetrieveList(DictionaryType.AMS, "行政区划");
            regionList.DataSource = regionEntites;
            regionList.DataTextField = "ItemText";
            regionList.DataValueField = "ItemGuid";
            regionList.DataBind();
            regionList.Items.Insert(0, new RadComboBoxItem("", ""));

            //监测区域
            IQueryable<V_CodeMainItemEntity> monitoringRegionEntites = dicService.RetrieveList(DictionaryType.AMS, "监测区域");
            monitoringRegionList.DataSource = monitoringRegionEntites;
            monitoringRegionList.DataTextField = "ItemText";
            monitoringRegionList.DataValueField = "ItemGuid";
            monitoringRegionList.DataBind();
            monitoringRegionList.Items.Insert(0, new RadComboBoxItem("", ""));

            //空气站点类型
            IQueryable<V_CodeMainItemEntity> siteTypeEntites = dicService.RetrieveList(DictionaryType.Air, "空气站点类型");
            siteTypeList.DataSource = siteTypeEntites;
            siteTypeList.DataTextField = "ItemText";
            siteTypeList.DataValueField = "ItemGuid";
            siteTypeList.DataBind();
            siteTypeList.Items.Insert(0, new RadComboBoxItem("", ""));

            //集成商类型
            IQueryable<V_CodeMainItemEntity> integratorEntites = dicService.RetrieveList(DictionaryType.AMS, "集成商类型");
            integratorList.DataSource = integratorEntites;
            integratorList.DataTextField = "ItemText";
            integratorList.DataValueField = "ItemGuid";
            integratorList.DataBind();
            integratorList.Items.Insert(0, new RadComboBoxItem("", ""));

            //控制类型
            IQueryable<V_CodeMainItemEntity> controlEntites = dicService.RetrieveList(DictionaryType.Air, "空气站点属性类型");
            contrlCodeList.DataSource = controlEntites;
            contrlCodeList.DataTextField = "ItemText";
            contrlCodeList.DataValueField = "ItemGuid";
            contrlCodeList.DataBind();
            contrlCodeList.Items.Insert(0, new RadComboBoxItem("", ""));

            //运行状态类型
            IQueryable<V_CodeMainItemEntity> runStatusEntites = dicService.RetrieveList(DictionaryType.AMS, "站点运行状态");
            runStatusList.DataSource = runStatusEntites;
            runStatusList.DataTextField = "ItemText";
            runStatusList.DataValueField = "ItemGuid";
            runStatusList.DataBind();
            runStatusList.Items.Insert(0, new RadComboBoxItem("", ""));

            //城市类型
            cityList.DataSource = dicService.RetrieveCityList();
            cityList.DataTextField = "ItemText";
            cityList.DataValueField = "ItemGuid";
            cityList.DataBind();
            cityList.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void BindUI()
        {
            //绑定控件值
            MonitoringPointEntity newPoint = pointService.RetrieveEntityByUid(ViewState["PointUid"].ToString());
            if (newPoint != null)
            {
                regionList.SelectedValue = string.IsNullOrEmpty(newPoint.RegionUid) ? string.Empty : newPoint.RegionUid.ToLower();
                siteTypeList.SelectedValue = string.IsNullOrEmpty(newPoint.SiteTypeUid) ? string.Empty : newPoint.SiteTypeUid.ToLower();
                //监测区域Uid暂时未配置
                //monitoringRegionList.SelectedValue = newPoint.MonitoringRegionUid.ToLower();
                if (!string.IsNullOrEmpty(newPoint.InstrumentIntegratorUid))
                    integratorList.SelectedValue = newPoint.InstrumentIntegratorUid.ToLower();
                contrlCodeList.SelectedValue = string.IsNullOrEmpty(newPoint.ContrlUid) ? string.Empty : newPoint.ContrlUid.ToLower();
                runStatusList.SelectedValue = string.IsNullOrEmpty(newPoint.RunStatusUid) ? string.Empty : newPoint.RunStatusUid.ToLower();

                txtPointName.Text = newPoint.MonitoringPointName;
                txtAddress.Text = newPoint.Address;
                txtBuildTime.SelectedDate = newPoint.BuildTime;
                txtMemo.Text = newPoint.Description;
                txtX.Text = newPoint.X == (Nullable<decimal>)null ? string.Empty : newPoint.X.ToString();
                txtY.Text = newPoint.Y == (Nullable<decimal>)null ? string.Empty : newPoint.Y.ToString();
                tbxOrderNumber.Text = newPoint.OrderByNum == (Nullable<int>)null ? string.Empty : newPoint.OrderByNum.ToString();
                cbxIsRefer.Checked = newPoint.IsReferencePoint == null ? false : newPoint.IsReferencePoint.Value;
                cbxIsShow.Checked = newPoint.ShowOrNot == null ? false : newPoint.ShowOrNot.Value;
                cbxIsUse.Checked = newPoint.EnableOrNot == null ? false : newPoint.EnableOrNot.Value;

                //扩展信息
                cbxIsCalRegionAQI.Checked = newPoint.MonitoringPointExtensionForEQMSAirEntity.CalRegionAQIOrNot == null ? false : newPoint.MonitoringPointExtensionForEQMSAirEntity.CalRegionAQIOrNot.Value;
                cbxIsCalAQI.Checked = newPoint.MonitoringPointExtensionForEQMSAirEntity.CalAQIOrNot == null ? false : newPoint.MonitoringPointExtensionForEQMSAirEntity.CalAQIOrNot.Value;
                cbxIsModel.Checked = newPoint.MonitoringPointExtensionForEQMSAirEntity.ModelOrNot == null ? false : newPoint.MonitoringPointExtensionForEQMSAirEntity.ModelOrNot.Value;
                //增加是否超级站
                cbxIsSuper.Checked = newPoint.MonitoringPointExtensionForEQMSAirEntity.SuperOrNot == null ? false : newPoint.MonitoringPointExtensionForEQMSAirEntity.SuperOrNot.Value;

                cityList.SelectedValue = newPoint.MonitoringPointExtensionForEQMSAirEntity.CityTypeUid;
                LoadCityRegion(cityList.SelectedItem.Text);
                cityRegionList.SelectedValue = newPoint.MonitoringPointExtensionForEQMSAirEntity.RegionTypeUid;
            }
        }

        /// <summary>
        /// 保存按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                MonitoringPointEntity newPoint = pointService.RetrieveEntityByUid(ViewState["PointUid"].ToString());
                if (newPoint != null)
                {
                    //判定点位是否已经存在
                    if (newPoint.MonitoringPointName != txtPointName.Text.Trim() && airPointService.IsExistName(txtPointName.Text))
                    {
                        Alert("站点名称已存在！");
                        return;
                    }
                    else newPoint.MonitoringPointName = txtPointName.Text;
                    if (txtX.Text.Trim() == "")
                        newPoint.X = 0;
                    else
                    {
                        newPoint.X = Convert.ToDecimal(txtX.Text.Trim());
                        if (newPoint.X < 0 || newPoint.X > 180)
                        {
                            Alert("经度必须是0和180之间的数值！");
                            return;
                        }
                    }
                    if (txtY.Text.Trim() == "")
                        newPoint.Y = 0;
                    else
                    {
                        newPoint.Y = Convert.ToDecimal(txtY.Text.Trim());
                        if (newPoint.Y < 0 || newPoint.Y > 90)
                        {
                            Alert("经度必须是0和90之间的数值！");
                            return;
                        }
                    }
                    newPoint.StationUid = "AA20D246-A8F4-4EC3-A5DE-376435235297";//暂时写死监测站Uid
                    newPoint.Address = txtAddress.Text;

                    newPoint.BuildTime = txtBuildTime.SelectedDate;

                    newPoint.ContrlUid = contrlCodeList.SelectedValue;
                    newPoint.InstrumentIntegratorUid = integratorList.SelectedValue;
                    newPoint.MonitoringRegionUid = monitoringRegionList.SelectedValue;
                    newPoint.RegionUid = regionList.SelectedValue;
                    newPoint.SiteTypeUid = siteTypeList.SelectedValue;
                    newPoint.RunStatusUid = runStatusList.SelectedValue;


                    newPoint.ShowOrNot = cbxIsShow.Checked;
                    newPoint.EnableOrNot = cbxIsUse.Checked;

                    //增加排序号和备注的修改
                    newPoint.OrderByNum = Convert.ToInt32(tbxOrderNumber.Text);
                    newPoint.Description = txtMemo.Text;
                    //增加是否背景点
                    newPoint.IsReferencePoint = cbxIsRefer.Checked;
                    //空气点位扩展表信息


                    newPoint.MonitoringPointExtensionForEQMSAirEntity.MonitoringPointUid = newPoint.MonitoringPointUid;
                    newPoint.MonitoringPointExtensionForEQMSAirEntity.ModelOrNot = cbxIsModel.Checked;
                    newPoint.MonitoringPointExtensionForEQMSAirEntity.SuperOrNot = cbxIsSuper.Checked;
                    newPoint.MonitoringPointExtensionForEQMSAirEntity.CalAQIOrNot = cbxIsCalAQI.Checked;
                    newPoint.MonitoringPointExtensionForEQMSAirEntity.CalRegionAQIOrNot = cbxIsCalRegionAQI.Checked;

                    newPoint.MonitoringPointExtensionForEQMSAirEntity.CityTypeUid = cityList.SelectedValue;
                    newPoint.MonitoringPointExtensionForEQMSAirEntity.RegionTypeUid = cityRegionList.SelectedValue;
                    //添加空气站点
                    airPointService.Update(newPoint);
                    Alert("更新成功！");
                }
            }
            catch
            {
                Alert("更新站点失败！");
                return;
            }
        }

        protected void cityList_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            LoadCityRegion(cityList.SelectedItem.Text);
        }

        private void LoadCityRegion(string cityName)
        {
            cityRegionList.Items.Clear();
            cityRegionList.DataSource = dicService.RetrieveCityRegionList(cityName);
            cityRegionList.DataTextField = "ItemText";
            cityRegionList.DataValueField = "ItemGuid";
            cityRegionList.DataBind();
            cityRegionList.Items.Insert(0, new RadComboBoxItem("", ""));
        }
    }
}