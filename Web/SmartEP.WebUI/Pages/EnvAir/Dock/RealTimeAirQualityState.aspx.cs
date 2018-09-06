using SmartEP.Core.Enums;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.Core.Enums;
using SmartEP.Service.DataAnalyze.Air.AQIReport;
using SmartEP.Service.Frame;
using SmartEP.Utilities.Office;
using SmartEP.WebControl.CbxRsm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.Dock
{
    /// <summary>
    /// 名称：RealTimeAirQualityState.cs
    /// 创建人：王秉晟
    /// 创建日期：2015-09-10
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：实时空气质量状况
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class RealTimeAirQualityState : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        private HourAQIService m_HourAQIService;

        /// <summary>
        /// 选择因子
        /// </summary>
        private IList<PollutantCodeEntity> factors = null;

        /// <summary>
        /// 选择站点
        /// </summary>
        private IList<IPoint> points = null;

        /// <summary>
        /// 苏州市的Guid
        /// </summary>
        private string SuZhouGuid
        {
            get
            {
                if (ViewState["SuZhouGuid"] == null)
                {
                    return string.Empty;
                }
                return ViewState["SuZhouGuid"].ToString();
            }
            set
            {
                ViewState["SuZhouGuid"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            m_HourAQIService = new HourAQIService();
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
            BindData();
        }
        #endregion

        #region 绑定数据
        /// <summary>
        /// 绑定数据
        /// </summary>
        public void BindData()
        {
            DateTime dtmEnd = DateTime.Now;
            DateTime dtmBegion = dtmEnd.AddDays(-7);
            DictionaryService dictionaryService = Singleton<DictionaryService>.GetInstance();
            MonitoringPointAirService monitoringPointAirService = Singleton<MonitoringPointAirService>.GetInstance();
            //IQueryable<V_CodeMainItemEntity> codeMainItemQueryable = dictionaryService.RetrieveList(DictionaryType.Air, "点位城市类型");//获取城市类型
            //string guid = dictionaryService.GetValueByText(DictionaryType.Air, "点位城市类型", "苏州市");//获取苏州市（0001）//7e05b94c-bbd4-45c3-919c-42da2e63fd43
            //string[] cityTypeUids = dictionaryService.RetrieveCityList().Where(t => t.ItemText == "苏州市区").Select(t => t.ItemGuid).ToArray(); //{ guid };
            SuZhouGuid = SmartEP.Core.Enums.EnumMapping.GetDesc(CityType.SuZhou).Split(':')[1]; //CityTypeUid 城市均值类型
            //IQueryable<MonitoringPointEntity> monitoringPointQueryable = monitoringPointAirService.RetrieveAirMPListByStatisticalCityType(SuZhouGuid);//根据统计城市类型获取点位列表
            string[] regionGuids = new string[] { SuZhouGuid }; //monitoringPointQueryable.Select(t => t.MonitoringPointExtensionForEQMSAirEntity.RegionTypeUid).Distinct().ToArray();
            hdCityTypeUids.Value = regionGuids.Aggregate((a, b) => a + ";" + b);
            DataTable dtRegionQuality = m_HourAQIService.RealTimeAirRegionsQuality(regionGuids, dtmBegion, dtmEnd).Table;//实时空气质量
            //dtRegionQuality = GetJiaData(dtRegionQuality);//临时使用的模拟数据

            if (dtRegionQuality.Rows.Count > 0)
            {
                DataRow drRegionQuality = dtRegionQuality.NewRow();
                drRegionQuality = dtRegionQuality.Rows[0];
                lblLastIssuedTime.Text = string.Format("({0:yyyy年MM月dd日 HH时})", Convert.ToDateTime(drRegionQuality["DateTime"]).AddHours(1)); ;//最新AQI发布时间(DateTime：数据时间)

                lblTakeStep.Text = drRegionQuality["TakeStep"].ToString();//建议采取的措施
                lblAQIValue.Text = drRegionQuality["AQIValue"].ToString();//AQI指数
                lblClass.Text = drRegionQuality["Class"].ToString();//空气质量指数类别(RGBValue：RGB颜色值)
                lblAQIValue.Style["color"] = drRegionQuality["RGBValue"].ToString();//RGB颜色值
                lblClass.Style["background-color"] = drRegionQuality["RGBValue"].ToString();//RGB颜色值
                string primaryPollutant = drRegionQuality["PrimaryPollutant"].ToString();
                if (primaryPollutant.Split(',').Length > 1)
                    primaryPollutant = primaryPollutant.Split(',')[0];
                if (primaryPollutant == "PM2.5")
                {
                    primaryPollutant = "PM25";
                }
                else if (primaryPollutant == "O3-1")
                {
                    primaryPollutant = "O3";
                }
                else if (primaryPollutant == "O3-8")
                {
                    primaryPollutant = "Recent8HoursO3";
                }

                if (drRegionQuality["PrimaryPollutant"].ToString() == "PM25")
                {
                    lblPrimaryPollutant.Text = "PM<sub>2.5</sub>";
                }
                else if (drRegionQuality["PrimaryPollutant"].ToString() == "PM10")
                {
                    lblPrimaryPollutant.Text = "PM<sub>10</sub>";
                }
                else if (drRegionQuality["PrimaryPollutant"].ToString() == "SO2")
                {
                    lblPrimaryPollutant.Text = "SO<sub>2</sub>";
                }
                else if (drRegionQuality["PrimaryPollutant"].ToString() == "NO2")
                {
                    lblPrimaryPollutant.Text = "NO<sub>2</sub>";
                }
                else if (drRegionQuality["PrimaryPollutant"].ToString() == "O3")
                {
                    lblPrimaryPollutant.Text = "O<sub>3</sub>";
                }
                else
                {
                    lblPrimaryPollutant.Text = drRegionQuality["PrimaryPollutant"].ToString();//首要污染
                }
                if (dtRegionQuality.Columns.Contains(drRegionQuality["PrimaryPollutant"].ToString())
                || drRegionQuality["PrimaryPollutant"].ToString() == "PM25" || drRegionQuality["PrimaryPollutant"].ToString() == "PM10")
                {
                    if (drRegionQuality["PrimaryPollutant"].ToString() == "PM25")
                    {
                        lblPollutantValue.Text = drRegionQuality["Recent24HoursPM25"].ToString();//浓度值
                    }
                    else if (drRegionQuality["PrimaryPollutant"].ToString() == "PM10")
                    {
                        lblPollutantValue.Text = drRegionQuality["Recent24HoursPM10"].ToString();//浓度值
                    }
                    else
                    {
                        lblPollutantValue.Text = drRegionQuality[drRegionQuality["PrimaryPollutant"].ToString()].ToString();//浓度值
                    }
                }
                if (drRegionQuality["PrimaryPollutant"].ToString() == "CO")
                {
                    lblUnit.Text = "毫克/立方米";//"mg/<sup>m3</sup>";//单位
                }
                else
                {
                    if (dtRegionQuality.Columns.Contains(primaryPollutant))
                    {
                        decimal value = Math.Round(Convert.ToDecimal(drRegionQuality[primaryPollutant]) * 1000, 0);
                        lblPollutantValue.Text = value.ToString();
                        lblUnit.Text = "微克/立方米";//"μg/m<sup>3</sup>";//单位
                    }
                    else
                    {
                        lblPrimaryPollutant.Text = "--";
                        lblPollutantValue.Text = "--";
                        lblUnit.Text = "";
                    }
                }
                //if (!string.IsNullOrWhiteSpace(drRegionQuality["PicturePath"].ToString()))
                //{
                //    imgHealthEffect.ImageUrl = drRegionQuality["PicturePath"].ToString();//图片路径//"~\Resources\Images\temp\"
                //}
                SetHealthEffectPicture(lblAQIValue.Text);
                imgHealthEffect.ToolTip = drRegionQuality["HealthEffect"].ToString();//对健康影响情况
            }
            else
            {
                //lblRegionName.Text = "苏州市区";//区域名称
                lblLastIssuedTime.Text = "";//最新AQI发布时间(DateTime：数据时间)
                //lblHealthEffect.Text = "";//对健康影响情况
                lblTakeStep.Text = "";//建议采取的措施
                lblAQIValue.Text = "";//AQI指数
                lblClass.Text = "";//空气质量指数类别(RGBValue：RGB颜色值)
                //lblAQITitle.Text = "AQI指数";//AQI指数标题
                //lblPrimaryPollutantTitle.Text = "首要污染物";//首要污染物标题
                lblPrimaryPollutant.Text = "";//首要污染
                //lblPollutantValueTitle.Text = "浓度值";//浓度值标题
                lblPollutantValue.Text = "";//浓度值
                lblUnit.Text = "";//单位
                //imgHealthEffect.ImageUrl = @"~\Resources\Images\temp;";//图片路径
                imgHealthEffect.ToolTip = "";//对健康影响情况
            }
        }

        private DataTable GetJiaData(DataTable dtOld)
        {
            DataTable dt = dtOld.Clone();
            DataRow dr = dt.NewRow();
            dr["DateTime"] = DateTime.Now;

            if (DateTime.Now.Hour / 2 > 10)
            {
                dr["Class"] = "良";
                dr["AQIValue"] = DateTime.Now.Hour + 50;
                dr["HealthEffect"] = "空气质量可接受，但某些污染物可能对极少数异常敏感人群健康有较弱影响";
                dr["TakeStep"] = "极少数异常敏感人群应减少户外活动";
                dr["RGBValue"] = "#ffff00";
            }
            else
            {
                dr["Class"] = "优";
                dr["AQIValue"] = DateTime.Now.Hour + 20;
                dr["HealthEffect"] = "空气质量令人满意，基本无空气污染";
                dr["TakeStep"] = "各类人群可正常活动";
                dr["RGBValue"] = "#00e400";
            }

            if (DateTime.Now.Hour / 2 == 1)
            {
                dr["PrimaryPollutant"] = "O3";
                dr["O3"] = DateTime.Now.Hour + DateTime.Now.Second / 2;
            }
            else if (DateTime.Now.Hour / 2 == 2)
            {
                dr["PrimaryPollutant"] = "PM10";
                dr["PM10"] = DateTime.Now.Hour + DateTime.Now.Second / 3;
            }
            else if (DateTime.Now.Hour / 2 == 3)
            {
                dr["PrimaryPollutant"] = "SO2";
                dr["SO2"] = DateTime.Now.Hour + DateTime.Now.Second / 2;
            }
            else if (DateTime.Now.Hour / 2 == 4)
            {
                dr["PrimaryPollutant"] = "NO2";
                dr["NO2"] = DateTime.Now.Hour + DateTime.Now.Second / 3;
            }
            //else if (DateTime.Now.Hour / 2 == 5)
            //{
            //    dr["PrimaryPollutant"] = "CO";
            //    dr["CO"] = dr["AQIValue"].ToString();
            //}
            else
            {
                dr["PrimaryPollutant"] = "PM25";
                dr["PM25"] = DateTime.Now.Hour + DateTime.Now.Second / 2 + 5;
            }
            dt.Rows.Add(dr);
            return dt;
        }
        #endregion

        #region 服务器端控件事件处理
        /// <summary>
        /// 查询按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            BindData();
        }

        /// <summary>
        /// 详细按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDetail_Click(object sender, EventArgs e)
        {
            string script = string.Format(@"window.parent.location.href='../RealTimeData/RealTimeAirQuality.aspx?Type={0}&RegionUid={1}';",
                                          "Region", SuZhouGuid);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), script, true);
        }

        /// <summary>
        /// 数据行绑定处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grid_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                DataRowView drv = e.Item.DataItem as DataRowView;
                if (item["PointId"] != null)
                {
                    GridTableCell pointCell = (GridTableCell)item["PointId"];
                    string pointName = points.Where(x => x.PointID.Equals(drv["PointId"].ToString().Trim()))
                                       .Select(t => t.PointName).FirstOrDefault();
                    pointCell.Text = pointName;
                }
                if (item["RGBValue"] != null)
                {
                    GridTableCell cell = item["RGBValue"] as GridTableCell;
                    cell.Style.Add("background-color", cell.Text);
                    cell.Text = string.Empty;
                }
            }
        }
        #endregion

        /// <summary>
        /// 根据测点Id数组获取因子列
        /// </summary>
        /// <param name="pointIds">测点Id数组</param>
        /// <returns></returns>
        private IList<PollutantCodeEntity> GetPollutantCodesByPointIds(string[] pointIds)
        {
            MonitoringPointAirService monitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
            IQueryable<MonitoringPointEntity> monitorPointQueryable = monitoringPointAir.RetrieveListByPointIds(pointIds);//根据站点ID数组获取站点
            IList<PollutantCodeEntity> pollutantList = new List<PollutantCodeEntity>();
            InstrumentChannelService instrumentChannelService = Singleton<InstrumentChannelService>.GetInstance();//提供仪器通道信息服务
            foreach (MonitoringPointEntity monitoringPointEntity in monitorPointQueryable)
            {
                IQueryable<PollutantCodeEntity> pollutantCodeQueryable =
                    instrumentChannelService.RetrieveChannelListByPointUid(monitoringPointEntity.MonitoringPointUid);//根据站点Uid获取所有监测通道
                pollutantList = pollutantList.Union(pollutantCodeQueryable).ToList();
            }
            return pollutantList;
        }

        /// <summary>
        /// 设置AQI图片
        /// </summary>
        /// <param name="aqiValue"></param>
        private void SetHealthEffectPicture(string aqiValue)
        {
            int aqi = -99;
            if (!int.TryParse(aqiValue, out aqi))
                return;
            string path = string.Empty;
            if (aqi >= 0 && aqi <= 50)
            {
                path = "~\\Resources\\Images\\AQI\\1.png";
            }
            else if (aqi >= 51 && aqi <= 100)
            {
                path = "~\\Resources\\Images\\AQI\\2.png";
            }
            else if (aqi >= 101 && aqi <= 150)
            {
                path = "~\\Resources\\Images\\AQI\\3.png";
            }
            else if (aqi >= 151 && aqi <= 200)
            {
                path = "~\\Resources\\Images\\AQI\\4.png";
            }
            else if (aqi >= 201 && aqi <= 300)
            {
                path = "~\\Resources\\Images\\AQI\\5.png";
            }
            else if (aqi >= 301)
            {
                path = "~\\Resources\\Images\\AQI\\6.png";
            }

            imgHealthEffect.ImageUrl = path;
        }
    }
}