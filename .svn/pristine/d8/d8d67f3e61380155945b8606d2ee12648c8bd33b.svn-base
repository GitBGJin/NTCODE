using SmartEP.Core.Enums;
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
    public partial class AirPointAdd : BasePage
    {
        DictionaryService dicService = new DictionaryService();
        MonitoringPointService pointService = new MonitoringPointService();
        MonitoringPointAirService airPointService = new MonitoringPointAirService();
        AcquisitionInstrumentService acquisitionService = new AcquisitionInstrumentService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                InitControl();
                //增加调用LoadCityRegion方法
                LoadCityRegion(cityList.SelectedItem.Text);
            }
        }
        /// <summary>
        /// 初始化下拉列表
        /// </summary>
        private void InitControl()
        {
            //行政区划
            IQueryable<V_CodeMainItemEntity> regionEntites = dicService.RetrieveList(DictionaryType.AMS,"行政区划");
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
            runStatusList.SelectedIndex = 2;

            //城市类型
            cityList.DataSource = dicService.RetrieveCityList();
            cityList.DataTextField = "ItemText";
            cityList.DataValueField = "ItemGuid";
            cityList.DataBind();
            cityList.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                //站点实体
                MonitoringPointEntity newPoint = new MonitoringPointEntity();
                //站点扩展信息实体
                MonitoringPointExtensionForEQMSAirEntity newExten = new MonitoringPointExtensionForEQMSAirEntity();
                //数采仪实体，默认情况下，新增一个点位，就新增一条数采仪记录
                AcquisitionInstrumentEntity newAcq = new AcquisitionInstrumentEntity();

                //判定点位是否已经存在
                if (airPointService.IsExistName(txtPointName.Text))
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
                
                newPoint.MonitoringPointUid = Guid.NewGuid().ToString();
                newPoint.StationUid = "AA20D246-A8F4-4EC3-A5DE-376435235297";//暂时写死监测站Uid
                newPoint.Address = txtAddress.Text;
                newPoint.ApplicationUid = SmartEP.Core.Enums.EnumMapping.GetApplicationValue(ApplicationValue.Air);
                //newPoint.BuildTime =txtBuildTime.SelectedDate.Value;
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
                newExten.MonitoringPointUid = newPoint.MonitoringPointUid;
                newExten.ModelOrNot = cbxIsModel.Checked;
                newExten.SuperOrNot = cbxIsSuper.Checked;
                newExten.CalAQIOrNot = cbxIsCalAQI.Checked;
                newExten.CalRegionAQIOrNot = cbxIsCalRegionAQI.Checked;

                newExten.CityTypeUid = cityList.SelectedValue;
                newExten.RegionTypeUid = cityRegionList.SelectedValue;

                //添加数采仪信息
                newAcq.MonitoringPointUid = newPoint.MonitoringPointUid;
                newAcq.MN = txtMN.Text;
                //newAcq.DataTypeCode = "";

                //添加空气站点
                airPointService.Add(newPoint);
                Alert("新增成功！");
                //添加空气扩展信息
                airPointService.Add(newExten);
                //添加数采仪信息
                acquisitionService.Add(newAcq);
            }
            catch
            {
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