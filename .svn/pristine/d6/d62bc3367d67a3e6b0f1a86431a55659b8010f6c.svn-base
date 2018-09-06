using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.Service.DataAnalyze.Air.AQIReport;
using System;
using System.Data;
using System.Linq;
using System.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.Dock
{
    public partial class RealTimeAirQualityState_WX : BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        private HourAQIService m_HourAQIService = Singleton<HourAQIService>.GetInstance();

        /// <summary>
        /// 无锡市区域标识
        /// </summary>
        private string WuXiGuid = System.Configuration.ConfigurationManager.AppSettings["WuXiGuid"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //页面初始化
                BindData();
            }
        }

        #region 绑定数据
        /// <summary>
        /// 绑定数据
        /// </summary>
        public void BindData()
        {
            DateTime dtmEnd = DateTime.Now;
            DateTime dtmBegion = dtmEnd.AddDays(-7);
            string[] regionGuids = new string[] { WuXiGuid };
            hdCityTypeUids.Value = regionGuids.Aggregate((a, b) => a + ";" + b);
            DataTable dtRegionQuality = m_HourAQIService.RealTimeAirRegionsQuality(regionGuids, dtmBegion, dtmEnd).Table;//实时空气质量
            if (dtRegionQuality.Rows.Count > 0)
            {
                DataRow drRegionQuality = dtRegionQuality.Rows[0];
                lblLastIssuedTime.Text = string.Format("({0:yyyy年MM月dd日 HH时})", drRegionQuality["DateTime"]); ;//最新AQI发布时间(DateTime：数据时间)
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
                    lblPrimaryPollutant.Text = "PM2.5";
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
                if (!string.IsNullOrWhiteSpace(drRegionQuality["PicturePath"].ToString()))
                {
                    imgHealthEffect.ImageUrl = drRegionQuality["PicturePath"].ToString();//图片路径//"~\Resources\Images\temp\"
                }
                imgHealthEffect.ToolTip = drRegionQuality["HealthEffect"].ToString();//对健康影响情况
            }
            else
            {
                lblLastIssuedTime.Text = "";//最新AQI发布时间(DateTime：数据时间)
                lblTakeStep.Text = "";//建议采取的措施
                lblAQIValue.Text = "";//AQI指数
                lblClass.Text = "";//空气质量指数类别(RGBValue：RGB颜色值)
                lblPrimaryPollutant.Text = "";//首要污染
                lblPollutantValue.Text = "";//浓度值
                lblUnit.Text = "";//单位
                imgHealthEffect.ToolTip = "";//对健康影响情况
            }
        }
        #endregion

        #region 查询按纽事件
        /// <summary>
        /// 查询按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            BindData();
        }
        #endregion
    }
}