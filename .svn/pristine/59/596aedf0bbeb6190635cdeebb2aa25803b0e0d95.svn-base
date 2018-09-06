using SmartEP.Core.Generic;
using SmartEP.Service.DataAnalyze.Air;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartEP.WebUI.Pages.EnvAir.Forecast
{
    public partial class AQIForecast : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 数据处理服务
        /// </summary>
        RPT_AQIForecastService m_AQIForecast = Singleton<RPT_AQIForecastService>.GetInstance();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (true)
            {
                DataView myDV= m_AQIForecast.GetAQIList().DefaultView;
                if (myDV != null && myDV.Count > 0)
                {


                    this.SpanAQITimeA.InnerHtml = myDV[0]["AQITimeA"].ToString();
                    this.SpanAQIClassA.InnerHtml = myDV[0]["AQIClassA"].ToString();
                    this.SpanPrimaryPollutantA.InnerHtml = ReplaceSub(myDV[0]["PrimaryPollutantA"].ToString());
                    this.SpanAQIA.InnerHtml = myDV[0]["AQIA"].ToString();

                    this.SpanAQITimeB.InnerHtml = myDV[0]["AQITimeB"].ToString();
                    this.SpanAQIClassB.InnerHtml = myDV[0]["AQIClassB"].ToString();
                    this.SpanPrimaryPollutantB.InnerHtml = ReplaceSub(myDV[0]["PrimaryPollutantB"].ToString());
                    this.SpanAQIB.InnerHtml = myDV[0]["AQIB"].ToString();

                    this.SpanAQITimeC.InnerHtml = myDV[0]["AQITimeC"].ToString();
                    this.SpanAQIClassC.InnerHtml = myDV[0]["AQIClassC"].ToString();
                    this.SpanPrimaryPollutantC.InnerHtml = ReplaceSub(myDV[0]["PrimaryPollutantC"].ToString());
                    this.SpanAQIC.InnerHtml = myDV[0]["AQIC"].ToString();

                    this.SpanDescription.InnerHtml = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + myDV[0]["Description"].ToString();
                    this.SpanIssuedUnit.InnerHtml = myDV[0]["IssuedUnit"].ToString();
                    this.SpanDT.InnerHtml = Convert.ToDateTime(myDV[0]["IssuedTime"]).ToString("M月d日H时发布");
                }
                else
                {
                    this.SpanAQITimeA.InnerHtml = "--";
                    this.SpanAQIClassA.InnerHtml = "--";
                    this.SpanPrimaryPollutantA.InnerHtml = "--";
                    this.SpanAQIA.InnerHtml = "--";

                    this.SpanAQITimeB.InnerHtml = "--";
                    this.SpanAQIClassB.InnerHtml = "--";
                    this.SpanPrimaryPollutantB.InnerHtml = "--";
                    this.SpanAQIB.InnerHtml = "--";

                    this.SpanAQITimeA.InnerHtml = "--";
                    this.SpanAQIClassA.InnerHtml = "--";
                    this.SpanPrimaryPollutantA.InnerHtml = "--";
                    this.SpanAQIC.InnerHtml = "--";

                    this.SpanDescription.InnerHtml = "--";
                    this.SpanIssuedUnit.InnerHtml = "--";
                    this.SpanDT.InnerHtml = "--";
                }


            }

        }

        private string ReplaceSub(String strFactor)
        {
            switch (strFactor)
            {
                case "SO2": return "SO<sub>2</sub>";
                case "NO2": return "NO<sub>2</sub>";
                case "PM10": return "PM<sub>10</sub>";
                case "CO": return "CO";
                case "O3": return "O<sub>3</sub>";
                case "PM2.5": return "PM<sub>2.5</sub>";
                default: return "";
            }
        }
    }
}