using SmartEP.Core.Generic;
using SmartEP.DomainModel.BaseData;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.Service.BaseData.Channel;
using SmartEP.Service.DataAnalyze.Air.AQIReport;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Utilities.IO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.Report
{
    public partial class AirQualityDayReportNew : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 日数据接口
        /// </summary>
        DayAQIService g_DayAQIService = Singleton<DayAQIService>.GetInstance();
        /// <summary>
        /// 因子接口
        /// </summary>
        AirPollutantService g_AirPollutantService = Singleton<AirPollutantService>.GetInstance();
        /// <summary>
        /// 测点组
        /// </summary>
        string PointIds = System.Configuration.ConfigurationManager.AppSettings["PointIdsForAirQualityDayReportNew"].ToString();
        /// <summary>
        /// 无锡市区Uid
        /// </summary>
        string CityUid = SmartEP.Core.Enums.EnumMapping.GetDesc(SmartEP.Service.Core.Enums.CityType.SuZhou).Split(':')[1];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitControl();
            }
        }

        /// <summary>
        /// 初始化全市AQI数据
        /// </summary>
        private void InitControl()
        {
            DateTime date = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            DateTime BeginDate = Convert.ToDateTime(date.Year.ToString() + "-01-01");
            IQueryable<RegionDayAQIReportEntity> ItemLists = g_DayAQIService.regionRetrieve(x => x.MonitoringRegionUid == CityUid && x.StatisticalType == "CG" && x.ReportDateTime >= BeginDate && x.ReportDateTime <= date).OrderByDescending(x => x.ReportDateTime);
            string All = "";
            int Alldays = ItemLists.Count();
            int days = 0;

            DataTable IAQI_dt = new DataTable();
            IAQI_dt.Columns.Add("Type", typeof(string));
            IAQI_dt.Columns.Add("level", typeof(string));
            IAQI_dt.Columns.Add("Value", typeof(string));

            DataRow IAQI_dr = IAQI_dt.NewRow();
            int thisdays = ItemLists.Where(x => x.Grade == "一级").Count();
            days += thisdays;
            IAQI_dr["Type"] = "优";
            IAQI_dr["level"] = "一级";
            IAQI_dr["Value"] = thisdays.ToString();
            IAQI_dt.Rows.Add(IAQI_dr);
            if (thisdays > 0)
            {
                All += "一级" + thisdays.ToString() + "天；";
            }

            IAQI_dr = IAQI_dt.NewRow();
            thisdays = ItemLists.Where(x => x.Grade == "二级").Count();
            days += thisdays;
            IAQI_dr["Type"] = "良";
            IAQI_dr["level"] = "二级";
            IAQI_dr["Value"] = thisdays.ToString();
            IAQI_dt.Rows.Add(IAQI_dr);
            if (thisdays > 0)
            {
                All += "二级" + thisdays.ToString() + "天；";
            }

            IAQI_dr = IAQI_dt.NewRow();
            thisdays = ItemLists.Where(x => x.Grade == "三级").Count();
            IAQI_dr["Type"] = "轻度污染";
            IAQI_dr["level"] = "三级";
            IAQI_dr["Value"] = thisdays.ToString();
            IAQI_dt.Rows.Add(IAQI_dr);
            if (thisdays > 0)
            {
                All += "三级" + thisdays.ToString() + "天；";
            }

            IAQI_dr = IAQI_dt.NewRow();
            thisdays = ItemLists.Where(x => x.Grade == "四级").Count();
            IAQI_dr["Type"] = "中度污染";
            IAQI_dr["level"] = "四级";
            IAQI_dr["Value"] = thisdays.ToString();
            IAQI_dt.Rows.Add(IAQI_dr);
            if (thisdays > 0)
            {
                All += "四级" + thisdays.ToString() + "天；";
            }

            IAQI_dr = IAQI_dt.NewRow();
            thisdays = ItemLists.Where(x => x.Grade == "五级").Count();
            IAQI_dr["Type"] = "重度污染";
            IAQI_dr["level"] = "五级";
            IAQI_dr["Value"] = thisdays.ToString();
            IAQI_dt.Rows.Add(IAQI_dr);
            if (thisdays > 0)
            {
                All += "五级" + thisdays.ToString() + "天；";
            }

            IAQI_dr = IAQI_dt.NewRow();
            thisdays = ItemLists.Where(x => x.Grade == "六级").Count();
            IAQI_dr["Type"] = "严重污染";
            IAQI_dr["level"] = "六级";
            IAQI_dr["Value"] = thisdays.ToString();
            IAQI_dt.Rows.Add(IAQI_dr);
            if (thisdays > 0)
            {
                All += "六级" + thisdays.ToString() + "天；";
            }

            hdAirQuality.Value = JsonHelper.ToJson(IAQI_dt);//有效数据
            double exl = (Convert.ToDouble(days) / Convert.ToDouble(Alldays)) * 100;
            total.InnerText = BeginDate.ToString("yyyy年MM月dd日") + "至" + date.ToString("yyyy年MM月dd日") + ",市区空气质量：" + All + "优良天数累计" + days.ToString() + "天," + "占" + exl.ToString("0.00") + "%";
        }

        #region 绑定RadGrid
        /// <summary>
        /// 绑定RadGrid
        /// </summary>
        public void BindGrid()
        {
            List<int> PointIdLists = new List<int>();
            if (PointIds.Length > 0)
            {
                foreach (string id in PointIds.Split(','))
                {
                    PointIdLists.Add(Convert.ToInt32(id));
                }
            }
            PollutantCodeEntity[] Factors = g_AirPollutantService.RetrieveListByCalAQI().ToArray();
            DateTime date = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            DataTable dt = g_DayAQIService.GetDayAQIData(PointIdLists, date, date);
            IQueryable<RegionDayAQIReportEntity> ItemLists = g_DayAQIService.regionRetrieve(x => x.MonitoringRegionUid == CityUid && x.StatisticalType == "CG" && x.ReportDateTime == date).OrderByDescending(x => x.ReportDateTime);
            RegionDayAQIReportEntity item = ItemLists.FirstOrDefault();
            DataRow dr = dt.NewRow();
            dr["PointName"] = "无锡市";
            dr["DateTime"] = date;
            if (item != null)
            {
                dr["SO2"] = item.SO2;
                dr["SO2_IAQI"] = item.SO2_IAQI;
                dr["NO2"] = item.NO2;
                dr["NO2_IAQI"] = item.NO2_IAQI;
                dr["PM10"] = item.PM10;
                dr["PM10_IAQI"] = item.PM10_IAQI;
                dr["CO"] = item.CO;
                dr["CO_IAQI"] = item.CO_IAQI;
                dr["O3"] = item.MaxOneHourO3;
                dr["O3_IAQI"] = item.MaxOneHourO3_IAQI;
                dr["Max8HourO3"] = item.Max8HourO3;
                dr["Max8HourO3_IAQI"] = item.Max8HourO3_IAQI;
                dr["PM25"] = item.PM25;
                dr["PM25_IAQI"] = item.PM25_IAQI;
                dr["AQIValue"] = item.AQIValue;
                dr["PrimaryPollutant"] = item.PrimaryPollutant;
                dr["Grade"] = item.Grade;
                dr["Class"] = item.Class;
                dr["RGBValue"] = item.RGBValue;
            }
            dt.Rows.Add(dr);
            int recordTotal = dt != null ? dt.Rows.Count : 0;
            RadGrid1.DataSource = dt;
            RadGrid1.VirtualItemCount = recordTotal;
        }
        #endregion

        #region 绑定数据源
        /// <summary>
        /// 绑定数据源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }
        #endregion

        #region 数据行绑定处理
        /// <summary>
        /// 数据行绑定处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RadGrid1_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            PollutantCodeEntity[] Factors = g_AirPollutantService.RetrieveListByCalAQI().ToArray();
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                DataRowView drv = e.Item.DataItem as DataRowView;
                for (int i = 0; i < Factors.Length; i++)
                {
                    string[] uniqueNames = GetUniqueNameByPollutantName(Factors[i].PollutantName);
                    foreach (string uniqueName in uniqueNames)
                    {
                        if (drv.DataView.Table.Columns.Contains(uniqueName) && item[uniqueName] != null)
                        {
                            GridTableCell factorCell = (GridTableCell)item[uniqueName];
                            decimal pollutantValue;

                            if (decimal.TryParse(factorCell.Text, out pollutantValue))
                            {
                                //获取因子小数位,channelCode 因子代码 例：CO的code是a21005  （ channelCode=a21005）
                                AirPollutantService m_AirPollutantService = new AirPollutantService();
                                int DecimalNum = Convert.ToInt32(m_AirPollutantService.GetPollutantInfo(Factors[i].PollutantCode).PollutantDecimalNum);
                                //保留小数位数,value 需要进行小数位处理的数据 类型Decimal
                                if (uniqueName == "CO")
                                {
                                    factorCell.Text = DecimalExtension.GetPollutantValue(pollutantValue, DecimalNum).ToString();
                                }
                                else
                                {
                                    factorCell.Text = (DecimalExtension.GetPollutantValue(pollutantValue, DecimalNum) * 1000).ToString("G0");
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region 因子名称转换
        /// <summary>
        /// 因子名称转换
        /// </summary>
        /// <param name="pollutantName"></param>
        /// <returns></returns>
        private string[] GetUniqueNameByPollutantName(string pollutantName)
        {
            string[] returnValues = new string[0];
            switch (pollutantName)
            {
                case "二氧化硫":
                    returnValues = new string[] { "SO2" };
                    break;
                case "二氧化氮":
                    returnValues = new string[] { "NO2" };
                    break;
                case "PM10":
                    returnValues = new string[] { "PM10" };
                    break;
                case "一氧化碳":
                    returnValues = new string[] { "CO" };
                    break;
                case "臭氧":
                    returnValues = new string[] { "O3" };
                    break;
                case "PM2.5":
                    returnValues = new string[] { "PM25" };
                    break;
                default: break;
            }
            return returnValues;
        }
        #endregion

        #region 转换空气质量类别与颜色
        /// <summary>
        /// 转换空气质量类别与颜色
        /// </summary>
        /// <param name="Class"></param>
        /// <param name="RGBValue"></param>
        /// <returns></returns>
        public string GetClassRGB(object Class, object RGBValue)
        {
            string retstr = "";
            if (!string.IsNullOrEmpty(Class.ToString()) && !string.IsNullOrEmpty(RGBValue.ToString()))
            {
                retstr = "<font color='" + RGBValue.ToString() + "'>" + Class.ToString() + "</font>";
            }
            return retstr;
        }
        #endregion
    }
}