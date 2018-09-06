﻿using log4net;
using SmartEP.Core.Generic;
using SmartEP.Core.Interfaces;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.AutoMonitoring.Air;
using SmartEP.Service.BaseData.MPInfo;
using SmartEP.Service.Core.Enums;
using SmartEP.Service.DataAnalyze.Air;
using SmartEP.Service.DataAnalyze.Air.AQIReport;
using SmartEP.Service.Frame;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using SmartEP.Utilities.Web.NetWork;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Controls
{
    /// <summary>
    /// 名称：RealTimeAirQualityState.aspx.cs
    /// 创建人：吕云
    /// 创建日期：2017-5-18
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：南通市实时空气质量
    /// 虚拟分页类
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class RealTimeAirQualityState : System.Web.UI.UserControl
    {
        AQICalculateService s_AQICalculateService = new AQICalculateService();
        OrigionAQIService s_OrigionAQIService = new OrigionAQIService();
        string NTRegionPointIds = ConfigurationManager.AppSettings["NTRegionPointId"].ToString();
        ILog log = LogManager.GetLogger("FileLogging");//获取一个日志记录器
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }
        public void BindData()
        {
            BindHourData();
            BindDayData();
            #region 测试
            //WriteStream1();
            //string a = HttpUtility.UrlEncode("http://218.91.209.251:1117/CSYC/2017-09-07/N_2017-09-07%2000-00-00_4.88301.jpg");
            
            //bool r = Networks.IsInnerNetWork("10.32.248.177");
            //string[] PointIds = { "187", "188", "189", "190" };
            //DataTable dt = s_AQICalculateService.GetRegionAQI(PointIds, DateTime.Now.AddDays(-7), DateTime.Now, 1, "1");
            //DataTable dt = new DataTable();
            //dt.Columns.Add("SO2", typeof(string));
            //DataRow dr = dt.NewRow();
            //dr["SO2"] = null;
            //dt.Rows.Add(dr);
            //decimal a = dt.AsEnumerable().Where(d => d.Field<string>("SO2") != "" && d.Field<object>("SO2")!=null).Count();

            //decimal c = dt.AsEnumerable().Select(d => d.Field<string>("SO2")).Count()-dt.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<string>("SO2") == "" || d.Field<object>("SO2") == DBNull.Value)).Count();

            //decimal b = dt.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<object>("SO2") != DBNull.Value&&d.Field<string>("SO2") != "" && d.Field<object>("SO2") != null)).Count();
            int? aqi = s_AQICalculateService.GetIAQI("a34004", 0.028, 24);
            #endregion
        }
        HttpWebRequest httpReq;
        HttpWebResponse httpResp;

        string strBuff = "";
        char[] cbuffer = new char[256];
        int byteRead = 0;
        public void WriteStream() 
        {
            Uri httpURL = new Uri("http://www.semc.gov.cn/aqi/home/Index.aspx");

            httpReq = (HttpWebRequest)WebRequest.Create(httpURL); 
            httpResp = (HttpWebResponse) httpReq.GetResponse(); 
            Stream respStream = httpResp.GetResponseStream();
            StreamReader respStreamReader = new StreamReader(respStream,Encoding.UTF8);
            byteRead = respStreamReader.Read(cbuffer,0,256); 

            while (byteRead != 0) 
            { 
                string strResp = new string(cbuffer,0,byteRead); 
                strBuff = strBuff + strResp; 
                byteRead = respStreamReader.Read(cbuffer,0,256); 
            }
            using (StreamWriter sw = new StreamWriter("D:\\项目\\南通超级站\\代码\\CatchData\\CatchData\\Files\\ouput.html"))//将获取的内容写入文本
            {
                sw.Write(strBuff);
            }
            respStream.Close(); 
        }
        public void WriteStream1()
        {
            try
            {
                WebClient MyWebClient = new WebClient();


                MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据

                Byte[] pageData = MyWebClient.DownloadData("http://www.semc.gov.cn/aqi/home/Station.aspx"); //从指定网站下载数据

                //string pageHtml = Encoding.Default.GetString(pageData);  //如果获取网站页面采用的是GB2312，则使用这句            

                string pageHtml = Encoding.UTF8.GetString(pageData); //如果获取网站页面采用的是UTF-8，则使用这句


                using (StreamWriter sw = new StreamWriter("D:\\项目\\南通超级站\\代码\\CatchData\\CatchData\\Files\\ouput.html"))//将获取的内容写入文本
                {
                    sw.Write(pageHtml);
                }
            }
            catch (WebException webEx)
            {
                log.Error(webEx.Message.ToString());
            }
        }
        /// <summary>
        /// 绑定小时数据
        /// </summary>
        public void BindHourData()
        {
            try
            {
                DataTable dt = s_OrigionAQIService.GetOriRegionHourAQI("b6e983c4-4f92-4be3-bbac-d9b71c470640");
                if (dt.Rows.Count > 0)
                {
                    lblAQIValue.Text = dt.Rows[0]["AQIValue"]==DBNull.Value?"--":dt.Rows[0]["AQIValue"].ToString();
                    lblAQIValue.Style["color"] = dt.Rows[0]["RGBValue"].ToString();//RGB颜色值
                    lblClass.Text = dt.Rows[0]["Class"]==DBNull.Value?"--":dt.Rows[0]["Class"].ToString();
                    if (dt.Rows[0]["AQIValue"] != DBNull.Value)
                    {
                        string AQIAlabo = s_AQICalculateService.GetAQI_Grade(Convert.ToInt32(dt.Rows[0]["AQIValue"]), "Alabo");
                        imgSymbol.Src = "~/Resources/Images/HomePage/smile/" + AQIAlabo + ".png";
                    }
                    lblHour.Text = Convert.ToDateTime(dt.Rows[0]["DateTime"]).ToString("HH") + "时";
                    string PrimaryPollutant = string.Empty;
                    if (dt.Rows[0]["AQIValue"] == DBNull.Value)
                    {
                        PrimaryPollutant = "--";
                        lblPrimaryPollutant.Text = PrimaryPollutant;
                    }
                    else if (Convert.ToInt32(dt.Rows[0]["AQIValue"].ToString()) <= 50)
                    {
                        PrimaryPollutant = "--";
                        lblPrimaryPollutant.Text = PrimaryPollutant;
                    }
                    else
                    {
                        if (dt.Rows[0]["PrimaryPollutant"].ToString().Contains("SO2"))
                        {
                            PrimaryPollutant += "SO<sub>2</sub>,";
                        }
                        else if (dt.Rows[0]["PrimaryPollutant"].ToString().Contains("NO2"))
                        {
                            PrimaryPollutant += "NO<sub>2</sub>,";
                        }
                        else if (dt.Rows[0]["PrimaryPollutant"].ToString().Contains("PM10"))
                        {
                            PrimaryPollutant += "PM<sub>10</sub>,";
                        }
                        else if (dt.Rows[0]["PrimaryPollutant"].ToString().Contains("CO"))
                        {
                            PrimaryPollutant += dt.Rows[0]["PrimaryPollutant"].ToString() + ",";
                        }
                        else if (dt.Rows[0]["PrimaryPollutant"].ToString().Contains("O3"))
                        {
                            PrimaryPollutant += "O<sub>3</sub>,";
                        }
                        else if (dt.Rows[0]["PrimaryPollutant"].ToString().Contains("PM2.5"))
                        {
                            PrimaryPollutant += "PM<sub>2.5</sub>,";
                        }

                        lblPrimaryPollutant.Text = PrimaryPollutant.Substring(0, PrimaryPollutant.Length - 1);
                    }

                    string PollutantValue = string.Empty;
                    #region 浓度值
                    if (dt.Rows[0]["AQIValue"] == DBNull.Value)
                    {
                        PollutantValue = "--";
                    }
                    else if (Convert.ToInt32(dt.Rows[0]["AQIValue"].ToString()) <= 50)
                    {
                        PollutantValue = "--";
                    }
                    else if (!string.IsNullOrWhiteSpace(dt.Rows[0]["PrimaryPollutant"].ToString()))
                    {
                        if (dt.Rows[0]["PrimaryPollutant"].ToString().Contains("SO2"))
                        {
                            DataTable dtFactor = s_AQICalculateService.GetPollutantUnit("a21026");
                            if (dtFactor.Rows.Count > 0)
                            {
                                int decimalNum = Convert.ToInt32(dtFactor.Rows[0]["DecimalDigit"]);

                                if (dtFactor.Rows[0]["MeasureUnitName"].Equals("μg/m³"))
                                {
                                    PollutantValue += (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[0]["SO2"]), decimalNum) * 1000).ToString("G0") + " μg/m<sup>3</sup>,";

                                }
                                else
                                {
                                    PollutantValue += DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[0]["SO2"]), decimalNum).ToString() + " mg/m<sup>3</sup>,";
                                }
                            }
                        }
                        else if (dt.Rows[0]["PrimaryPollutant"].ToString().Contains("NO2"))
                        {
                            DataTable dtFactor = s_AQICalculateService.GetPollutantUnit("a21004");
                            if (dtFactor.Rows.Count > 0)
                            {
                                int decimalNum = Convert.ToInt32(dtFactor.Rows[0]["DecimalDigit"]);

                                if (dtFactor.Rows[0]["MeasureUnitName"].Equals("μg/m³"))
                                {
                                    PollutantValue += (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[0]["NO2"]), decimalNum) * 1000).ToString("G0") + " μg/m<sup>3</sup>,";

                                }
                                else
                                {
                                    PollutantValue += DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[0]["NO2"]), decimalNum).ToString() + " mg/m<sup>3</sup>,";
                                }
                            }
                        }
                        else if (dt.Rows[0]["PrimaryPollutant"].ToString().Contains("PM10"))
                        {
                            DataTable dtFactor = s_AQICalculateService.GetPollutantUnit("a34002");
                            if (dtFactor.Rows.Count > 0)
                            {
                                int decimalNum = Convert.ToInt32(dtFactor.Rows[0]["DecimalDigit"]);

                                if (dtFactor.Rows[0]["MeasureUnitName"].Equals("μg/m³"))
                                {
                                    PollutantValue += (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[0]["PM10"]), decimalNum) * 1000).ToString("G0") + " μg/m<sup>3</sup>,";

                                }
                                else
                                {
                                    PollutantValue += DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[0]["PM10"]), decimalNum).ToString() + " mg/m<sup>3</sup>,";
                                }
                            }
                        }
                        else if (dt.Rows[0]["PrimaryPollutant"].ToString().Contains("CO"))
                        {
                            DataTable dtFactor = s_AQICalculateService.GetPollutantUnit("a21005");
                            if (dtFactor.Rows.Count > 0)
                            {
                                int decimalNum = Convert.ToInt32(dtFactor.Rows[0]["DecimalDigit"]);

                                if (dtFactor.Rows[0]["MeasureUnitName"].Equals("μg/m³"))
                                {
                                    PollutantValue += (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[0]["CO"]), decimalNum) * 1000).ToString("G0") + " μg/m<sup>3</sup>,";

                                }
                                else
                                {
                                    PollutantValue += DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[0]["CO"]), decimalNum).ToString() + " mg/m<sup>3</sup>,";
                                }
                            }
                        }
                        else if (dt.Rows[0]["PrimaryPollutant"].ToString().Contains("O3"))
                        {
                            DataTable dtFactor = s_AQICalculateService.GetPollutantUnit("a05024");
                            if (dtFactor.Rows.Count > 0)
                            {
                                int decimalNum = Convert.ToInt32(dtFactor.Rows[0]["DecimalDigit"]);

                                if (dtFactor.Rows[0]["MeasureUnitName"].Equals("μg/m³"))
                                {
                                    PollutantValue += (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[0]["O3"]), decimalNum) * 1000).ToString("G0") + " μg/m<sup>3</sup>,";

                                }
                                else
                                {
                                    PollutantValue += DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[0]["O3"]), decimalNum).ToString() + " mg/m<sup>3</sup>,";
                                }
                            }
                        }
                        else if (dt.Rows[0]["PrimaryPollutant"].ToString().Contains("PM2.5"))
                        {
                            DataTable dtFactor = s_AQICalculateService.GetPollutantUnit("a34004");
                            if (dtFactor.Rows.Count > 0)
                            {
                                int decimalNum = Convert.ToInt32(dtFactor.Rows[0]["DecimalDigit"]);

                                if (dtFactor.Rows[0]["MeasureUnitName"].Equals("μg/m³"))
                                {
                                    PollutantValue += (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[0]["PM25"]), decimalNum) * 1000).ToString("G0") + " μg/m<sup>3</sup>,";

                                }
                                else
                                {
                                    PollutantValue += DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[0]["PM25"]), decimalNum).ToString() + " mg/m<sup>3</sup>,";
                                }
                            }
                        }
                        PollutantValue = PollutantValue.Substring(0, PollutantValue.Length - 1);
                    }
                    #endregion
                    lblPollutantValue.Text = PollutantValue;

                }
                #region 动态计算区域小时AQI(未用)
                //DateTime Tstamp = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:59:59"));
                //string AQI = BindHourData(Tstamp);
                //while (string.IsNullOrWhiteSpace(AQI))
                //{
                //    Tstamp = Tstamp.AddHours(-1);
                //    AQI = BindHourData(Tstamp);
                //}
                #endregion
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
        }
        /// <summary>
        /// 绑定小时数据
        /// </summary>
        /// <param name="Tstamp">时间</param>
        /// <returns></returns>
        public string BindHourData(DateTime Tstamp)
        {
            try
            {
                string[] PointIds = NTRegionPointIds.Split(',');
                int? AQI_SO2 = null, AQI_NO2 = null, AQI_PM10 = null, AQI_CO = null, AQI_O3 = null, AQI_PM25 = null;
                #region AQI_SO2
                //浓度
                decimal? SO2Value = s_AQICalculateService.GetRegionValue(PointIds, "a21026", Tstamp, 1, "1");
                //分指数
                if (SO2Value != null)
                {
                    AQI_SO2 = s_AQICalculateService.GetIAQI("a21026", Convert.ToDouble(SO2Value), 1);
                }
                #endregion
                #region AQI_NO2
                //浓度
                decimal? NO2Value = s_AQICalculateService.GetRegionValue(PointIds, "a21004", Tstamp, 1, "1");
                //分指数
                if (NO2Value != null)
                {
                    AQI_NO2 = s_AQICalculateService.GetIAQI("a21004", Convert.ToDouble(NO2Value), 1);
                }
                #endregion
                #region AQI_PM10
                //浓度
                decimal? PM10Value = s_AQICalculateService.GetRegionValue(PointIds, "a34002", Tstamp, 1, "1");
                //分指数
                if (PM10Value != null)
                {
                    AQI_PM10 = s_AQICalculateService.GetIAQI("a34002", Convert.ToDouble(PM10Value), 24);
                }
                #endregion
                #region AQI_CO
                //浓度
                decimal? COValue = s_AQICalculateService.GetRegionValue(PointIds, "a21005", Tstamp, 1, "1");
                //分指数
                if (COValue != null)
                {
                    AQI_CO = s_AQICalculateService.GetIAQI("a21005", Convert.ToDouble(COValue), 1);
                }
                #endregion
                #region AQI_O3
                //浓度
                decimal? O3Value = s_AQICalculateService.GetRegionValue(PointIds, "a05024", Tstamp, 1, "1");
                //分指数
                if (O3Value != null)
                {
                    AQI_O3 = s_AQICalculateService.GetIAQI("a05024", Convert.ToDouble(O3Value), 1);
                }
                #endregion
                #region AQI_PM25
                //浓度
                decimal? PM25Value = s_AQICalculateService.GetRegionValue(PointIds, "a34004", Tstamp, 1, "1");
                //分指数
                if (PM25Value != null)
                {
                    AQI_PM25 = s_AQICalculateService.GetIAQI("a34004", Convert.ToDouble(PM25Value), 24);
                }
                #endregion

                string AQI = s_AQICalculateService.GetAQI_Max_CNV(AQI_SO2, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "V");
                string Max_FactorCode = s_AQICalculateService.GetAQI_Max_CNV(AQI_SO2, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "C");
                string PrimaryPollutant = s_AQICalculateService.GetAQI_Max_CNV(AQI_SO2, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "N");
                lblAQIValue.Text = AQI;
                if (!string.IsNullOrWhiteSpace(AQI))
                {
                    string AQIColor = s_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQI), "Color");
                    string AQIClass = s_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQI), "Class");
                    string AQIAlabo = s_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQI), "Alabo");
                    lblAQIValue.Style["color"] = AQIColor;//RGB颜色值
                    lblClass.Text = AQIClass;
                    lblClass.Style["background-color"] = AQIColor;//RGB颜色值
                    imgSymbol.Src = "~/Resources/Images/HomePage/smile/" + AQIAlabo + ".png";
                }
                lblHour.Text = Tstamp.ToString("HH") + "时";
                lblPrimaryPollutant.Text = PrimaryPollutant;
                #region 浓度值
                string value = string.Empty;
                if (string.IsNullOrWhiteSpace(AQI))
                {
                    value = null;
                }
                else if (Convert.ToInt32(AQI) <= 50)
                {
                    value = "--";
                }
                else if (!string.IsNullOrWhiteSpace(PrimaryPollutant))
                {

                    if (Max_FactorCode.Contains("a21026"))
                    {
                        DataTable dtFactor = s_AQICalculateService.GetPollutantUnit("a21026");
                        if (dtFactor.Rows.Count > 0)
                        {
                            int decimalNum = Convert.ToInt32(dtFactor.Rows[0]["DecimalDigit"]);

                            if (dtFactor.Rows[0]["MeasureUnitName"].Equals("μg/m³"))
                            {
                                value += (DecimalExtension.GetPollutantValue(Convert.ToDecimal(SO2Value), decimalNum) * 1000).ToString() + " μg/m<sup>3</sup>";

                            }
                            else
                            {
                                value += DecimalExtension.GetPollutantValue(Convert.ToDecimal(SO2Value), decimalNum).ToString() + " mg/m<sup>3</sup>";
                            }
                        }

                    }
                    if (Max_FactorCode.Contains("a21004"))
                    {
                        DataTable dtFactor = s_AQICalculateService.GetPollutantUnit("a21004");
                        if (dtFactor.Rows.Count > 0)
                        {
                            int decimalNum = Convert.ToInt32(dtFactor.Rows[0]["DecimalDigit"]);
                            if (dtFactor.Rows[0]["MeasureUnitName"].Equals("μg/m³"))
                            {
                                value += (DecimalExtension.GetPollutantValue(Convert.ToDecimal(SO2Value), decimalNum) * 1000).ToString() + " μg/m<sup>3</sup>";

                            }
                            else
                            {
                                value += DecimalExtension.GetPollutantValue(Convert.ToDecimal(SO2Value), decimalNum).ToString() + " mg/m<sup>3</sup>";
                            }
                        }
                    }
                    if (Max_FactorCode.Contains("a34002"))
                    {
                        DataTable dtFactor = s_AQICalculateService.GetPollutantUnit("a34002");
                        if (dtFactor.Rows.Count > 0)
                        {
                            int decimalNum = Convert.ToInt32(dtFactor.Rows[0]["DecimalDigit"]);

                            if (dtFactor.Rows[0]["MeasureUnitName"].Equals("μg/m³"))
                            {
                                value += (DecimalExtension.GetPollutantValue(Convert.ToDecimal(SO2Value), decimalNum) * 1000).ToString() + " μg/m<sup>3</sup>";

                            }
                            else
                            {
                                value += DecimalExtension.GetPollutantValue(Convert.ToDecimal(SO2Value), decimalNum).ToString() + " mg/m<sup>3</sup>";
                            }
                        }
                    }
                    if (Max_FactorCode.Contains("a21005"))
                    {
                        DataTable dtFactor = s_AQICalculateService.GetPollutantUnit("a21005");
                        if (dtFactor.Rows.Count > 0)
                        {
                            int decimalNum = Convert.ToInt32(dtFactor.Rows[0]["DecimalDigit"]);

                            if (dtFactor.Rows[0]["MeasureUnitName"].Equals("μg/m³"))
                            {
                                value += (DecimalExtension.GetPollutantValue(Convert.ToDecimal(SO2Value), decimalNum) * 1000).ToString() + " μg/m<sup>3</sup>";

                            }
                            else
                            {
                                value += DecimalExtension.GetPollutantValue(Convert.ToDecimal(SO2Value), decimalNum).ToString() + " mg/m<sup>3</sup>";
                            }
                        }
                    }
                    if (Max_FactorCode.Contains("a05024"))
                    {
                        DataTable dtFactor = s_AQICalculateService.GetPollutantUnit("a05024");
                        if (dtFactor.Rows.Count > 0)
                        {
                            int decimalNum = Convert.ToInt32(dtFactor.Rows[0]["DecimalDigit"]);

                            if (dtFactor.Rows[0]["MeasureUnitName"].Equals("μg/m³"))
                            {
                                value += (DecimalExtension.GetPollutantValue(Convert.ToDecimal(SO2Value), decimalNum) * 1000).ToString() + " μg/m<sup>3</sup>";

                            }
                            else
                            {
                                value += DecimalExtension.GetPollutantValue(Convert.ToDecimal(SO2Value), decimalNum).ToString() + " mg/m<sup>3</sup>";
                            }
                        }
                    }
                    if (Max_FactorCode.Contains("a34004"))
                    {
                        DataTable dtFactor = s_AQICalculateService.GetPollutantUnit("a34004");
                        if (dtFactor.Rows.Count > 0)
                        {
                            int decimalNum = Convert.ToInt32(dtFactor.Rows[0]["DecimalDigit"]);

                            if (dtFactor.Rows[0]["MeasureUnitName"].Equals("μg/m³"))
                            {
                                value += (DecimalExtension.GetPollutantValue(Convert.ToDecimal(SO2Value), decimalNum) * 1000).ToString() + " μg/m<sup>3</sup>";

                            }
                            else
                            {
                                value += DecimalExtension.GetPollutantValue(Convert.ToDecimal(SO2Value), decimalNum).ToString() + " mg/m<sup>3</sup>";
                            }
                        }
                    }
                }
                lblPollutantValue.Text = value;
                #endregion
                return AQI;
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// 绑定昨日数据
        /// </summary>
        public void BindDayData()
        {
            try
            {
                DataTable dt = s_OrigionAQIService.GetOriRegionLastDayAQI("b6e983c4-4f92-4be3-bbac-d9b71c470640");
                if (dt.Rows.Count > 1)
                {
                    if (Convert.ToDateTime(dt.Rows[1]["ReportDateTime"]).ToString("yyyy-MM-dd") == DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"))
                    {
                        lblLastAQIValue.Text = dt.Rows[1]["AQIValue"] == DBNull.Value ? "--" : dt.Rows[1]["AQIValue"].ToString();
                        //lblLastAQIValue.Text = "48";
                        lblLastClassValue.Text = dt.Rows[1]["Class"] == DBNull.Value ? "--" : dt.Rows[1]["Class"].ToString();
                        //lblLastClassValue.Text = "优";
                        string PrimaryPollutant = string.Empty;
                        if (dt.Rows[1]["AQIValue"] == DBNull.Value)
                        {
                            PrimaryPollutant = "--";
                            lblLastPrimaryPollutantValue.Text = PrimaryPollutant;
                        }
                        else if (Convert.ToInt32(dt.Rows[1]["AQIValue"].ToString()) <= 50)
                        {
                            PrimaryPollutant = "--";
                            lblLastPrimaryPollutantValue.Text = PrimaryPollutant;

                        }
                        else
                        {
                            if (dt.Rows[1]["PrimaryPollutant"].ToString().Contains("SO2"))
                            {
                                PrimaryPollutant += "SO<sub>2</sub>,";
                            }
                            if (dt.Rows[1]["PrimaryPollutant"].ToString().Contains("NO2"))
                            {
                                PrimaryPollutant += "NO<sub>2</sub>,";
                            }
                            if (dt.Rows[1]["PrimaryPollutant"].ToString().Contains("PM10"))
                            {
                                PrimaryPollutant += "PM<sub>10</sub>,";
                            }
                            if (dt.Rows[1]["PrimaryPollutant"].ToString().Contains("CO"))
                            {
                                PrimaryPollutant += "CO,";
                            }
                            if (dt.Rows[1]["PrimaryPollutant"].ToString().Contains("O3"))
                            {
                                PrimaryPollutant += "O<sub>3</sub>,";
                            }
                            if (dt.Rows[1]["PrimaryPollutant"].ToString().Contains("PM2.5"))
                            {
                                PrimaryPollutant += "PM<sub>2.5</sub>,";
                            }
                            lblLastPrimaryPollutantValue.Text = PrimaryPollutant.Substring(0, PrimaryPollutant.Length - 1);
                        }
                        string PollutantValue = string.Empty;
                        #region 浓度值
                        if (string.IsNullOrWhiteSpace(dt.Rows[1]["AQIValue"].ToString()))
                        {
                            PollutantValue = "--";
                        }
                        else if (Convert.ToInt32(dt.Rows[1]["AQIValue"].ToString()) <= 50)
                        {
                            PollutantValue = "--";
                        }
                        else if (!string.IsNullOrWhiteSpace(dt.Rows[1]["PrimaryPollutant"].ToString()))
                        {
                            if (dt.Rows[1]["PrimaryPollutant"].ToString().Contains("SO2"))
                            {
                                DataTable dtFactor = s_AQICalculateService.GetPollutantUnit("a21026");
                                if (dtFactor.Rows.Count > 0)
                                {
                                    int decimalNum = Convert.ToInt32(dtFactor.Rows[0]["DecimalDigit"]);

                                    if (dtFactor.Rows[0]["MeasureUnitName"].Equals("μg/m³"))
                                    {
                                        PollutantValue += (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[1]["SO2"]), decimalNum) * 1000).ToString("G0") + " μg/m<sup>3</sup>,";

                                    }
                                    else
                                    {
                                        PollutantValue += DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[1]["SO2"]), decimalNum).ToString() + " mg/m<sup>3</sup>,";
                                    }
                                }
                            }
                            if (dt.Rows[1]["PrimaryPollutant"].ToString().Contains("NO2"))
                            {
                                DataTable dtFactor = s_AQICalculateService.GetPollutantUnit("a21004");
                                if (dtFactor.Rows.Count > 0)
                                {
                                    int decimalNum = Convert.ToInt32(dtFactor.Rows[0]["DecimalDigit"]);

                                    if (dtFactor.Rows[0]["MeasureUnitName"].Equals("μg/m³"))
                                    {
                                        PollutantValue += (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[1]["NO2"]), decimalNum) * 1000).ToString("G0") + " μg/m<sup>3</sup>,";

                                    }
                                    else
                                    {
                                        PollutantValue += DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[1]["NO2"]), decimalNum).ToString() + " mg/m<sup>3</sup>,";
                                    }
                                }
                            }
                            if (dt.Rows[1]["PrimaryPollutant"].ToString().Contains("PM10"))
                            {
                                DataTable dtFactor = s_AQICalculateService.GetPollutantUnit("a34002");
                                if (dtFactor.Rows.Count > 0)
                                {
                                    int decimalNum = Convert.ToInt32(dtFactor.Rows[0]["DecimalDigit"]);

                                    if (dtFactor.Rows[0]["MeasureUnitName"].Equals("μg/m³"))
                                    {
                                        PollutantValue += (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[1]["PM10"]), decimalNum) * 1000).ToString("G0") + " μg/m<sup>3</sup>,";

                                    }
                                    else
                                    {
                                        PollutantValue += DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[1]["PM10"]), decimalNum).ToString() + " mg/m<sup>3</sup>,";
                                    }
                                }
                            }
                            if (dt.Rows[1]["PrimaryPollutant"].ToString().Contains("CO"))
                            {
                                DataTable dtFactor = s_AQICalculateService.GetPollutantUnit("a21005");
                                if (dtFactor.Rows.Count > 0)
                                {
                                    int decimalNum = Convert.ToInt32(dtFactor.Rows[0]["DecimalDigit"]);

                                    if (dtFactor.Rows[0]["MeasureUnitName"].Equals("μg/m³"))
                                    {
                                        PollutantValue += (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[1]["CO"]), decimalNum) * 1000).ToString("G0") + " μg/m<sup>3</sup>,";

                                    }
                                    else
                                    {
                                        PollutantValue += DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[1]["CO"]), decimalNum).ToString() + " mg/m<sup>3</sup>,";
                                    }
                                }
                            }
                            if (dt.Rows[1]["PrimaryPollutant"].ToString().Contains("O3"))
                            {
                                DataTable dtFactor = s_AQICalculateService.GetPollutantUnit("a05024");
                                if (dtFactor.Rows.Count > 0)
                                {
                                    int decimalNum = Convert.ToInt32(dtFactor.Rows[0]["DecimalDigit"]);

                                    if (dtFactor.Rows[0]["MeasureUnitName"].Equals("μg/m³"))
                                    {
                                        PollutantValue += (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[1]["Max8HourO3"]), decimalNum) * 1000).ToString("G0") + " μg/m<sup>3</sup>,";
                                        //log.Info("日均值：" + PollutantValue);
                                    }
                                    else
                                    {
                                        PollutantValue += DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[1]["Max8HourO3"]), decimalNum).ToString() + " mg/m<sup>3</sup>,";
                                    }
                                }
                            }
                            if (dt.Rows[1]["PrimaryPollutant"].ToString().Contains("PM2.5"))
                            {
                                DataTable dtFactor = s_AQICalculateService.GetPollutantUnit("a34004");
                                if (dtFactor.Rows.Count > 0)
                                {
                                    int decimalNum = Convert.ToInt32(dtFactor.Rows[0]["DecimalDigit"]);

                                    if (dtFactor.Rows[0]["MeasureUnitName"].Equals("μg/m³"))
                                    {
                                        PollutantValue += (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[1]["PM25"]), decimalNum) * 1000).ToString("G0") + " μg/m<sup>3</sup>,";

                                    }
                                    else
                                    {
                                        PollutantValue += DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[1]["PM25"]), decimalNum).ToString() + " mg/m<sup>3</sup>,";
                                    }
                                }
                            }
                            PollutantValue = PollutantValue.Substring(0, PollutantValue.Length - 1);
                        }
                        #endregion
                        lblLastPollutantValue.Text = PollutantValue;
                        //lblLastPrimaryPollutantValue.Text = "--";
                        //lblLastPollutantValue.Text = "--";
                    }
                    if (Convert.ToDateTime(dt.Rows[0]["ReportDateTime"]).ToString("yyyy-MM-dd") == DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"))
                    {
                        lblLastAQIValue.Text = dt.Rows[0]["AQIValue"] == DBNull.Value ? "--" : dt.Rows[0]["AQIValue"].ToString();
                        //lblLastAQIValue.Text = "48";
                        lblLastClassValue.Text = dt.Rows[0]["Class"] == DBNull.Value ? "--" : dt.Rows[0]["Class"].ToString();
                        //lblLastClassValue.Text = "优";
                        string PrimaryPollutant = string.Empty;
                        if (dt.Rows[0]["AQIValue"] == DBNull.Value)
                        {
                            PrimaryPollutant = "--";
                            lblLastPrimaryPollutantValue.Text = PrimaryPollutant;
                        }
                        else if (Convert.ToInt32(dt.Rows[0]["AQIValue"].ToString()) <= 50)
                        {
                            PrimaryPollutant = "--";
                            lblLastPrimaryPollutantValue.Text = PrimaryPollutant;

                        }
                        else
                        {
                            if (dt.Rows[0]["PrimaryPollutant"].ToString().Contains("SO2"))
                            {
                                PrimaryPollutant += "SO<sub>2</sub>,";
                            }
                            if (dt.Rows[0]["PrimaryPollutant"].ToString().Contains("NO2"))
                            {
                                PrimaryPollutant += "NO<sub>2</sub>,";
                            }
                            if (dt.Rows[0]["PrimaryPollutant"].ToString().Contains("PM10"))
                            {
                                PrimaryPollutant += "PM<sub>10</sub>,";
                            }
                            if (dt.Rows[0]["PrimaryPollutant"].ToString().Contains("CO"))
                            {
                                PrimaryPollutant += "CO,";
                            }
                            if (dt.Rows[0]["PrimaryPollutant"].ToString().Contains("O3"))
                            {
                                PrimaryPollutant += "O<sub>3</sub>,";
                            }
                            if (dt.Rows[0]["PrimaryPollutant"].ToString().Contains("PM2.5"))
                            {
                                PrimaryPollutant += "PM<sub>2.5</sub>,";
                            }
                            lblLastPrimaryPollutantValue.Text = PrimaryPollutant.Substring(0, PrimaryPollutant.Length - 1);
                        }
                        string PollutantValue = string.Empty;
                        #region 浓度值
                        if (string.IsNullOrWhiteSpace(dt.Rows[0]["AQIValue"].ToString()))
                        {
                            PollutantValue = "--";
                        }
                        else if (Convert.ToInt32(dt.Rows[0]["AQIValue"].ToString()) <= 50)
                        {
                            PollutantValue = "--";
                        }
                        else if (!string.IsNullOrWhiteSpace(dt.Rows[0]["PrimaryPollutant"].ToString()))
                        {
                            if (dt.Rows[0]["PrimaryPollutant"].ToString().Contains("SO2"))
                            {
                                DataTable dtFactor = s_AQICalculateService.GetPollutantUnit("a21026");
                                if (dtFactor.Rows.Count > 0)
                                {
                                    int decimalNum = Convert.ToInt32(dtFactor.Rows[0]["DecimalDigit"]);

                                    if (dtFactor.Rows[0]["MeasureUnitName"].Equals("μg/m³"))
                                    {
                                        PollutantValue += (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[0]["SO2"]), decimalNum) * 1000).ToString("G0") + " μg/m<sup>3</sup>,";

                                    }
                                    else
                                    {
                                        PollutantValue += DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[0]["SO2"]), decimalNum).ToString() + " mg/m<sup>3</sup>,";
                                    }
                                }
                            }
                            if (dt.Rows[0]["PrimaryPollutant"].ToString().Contains("NO2"))
                            {
                                DataTable dtFactor = s_AQICalculateService.GetPollutantUnit("a21004");
                                if (dtFactor.Rows.Count > 0)
                                {
                                    int decimalNum = Convert.ToInt32(dtFactor.Rows[0]["DecimalDigit"]);

                                    if (dtFactor.Rows[0]["MeasureUnitName"].Equals("μg/m³"))
                                    {
                                        PollutantValue += (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[0]["NO2"]), decimalNum) * 1000).ToString("G0") + " μg/m<sup>3</sup>,";

                                    }
                                    else
                                    {
                                        PollutantValue += DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[0]["NO2"]), decimalNum).ToString() + " mg/m<sup>3</sup>,";
                                    }
                                }
                            }
                            if (dt.Rows[0]["PrimaryPollutant"].ToString().Contains("PM10"))
                            {
                                DataTable dtFactor = s_AQICalculateService.GetPollutantUnit("a34002");
                                if (dtFactor.Rows.Count > 0)
                                {
                                    int decimalNum = Convert.ToInt32(dtFactor.Rows[0]["DecimalDigit"]);

                                    if (dtFactor.Rows[0]["MeasureUnitName"].Equals("μg/m³"))
                                    {
                                        PollutantValue += (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[0]["PM10"]), decimalNum) * 1000).ToString("G0") + " μg/m<sup>3</sup>,";

                                    }
                                    else
                                    {
                                        PollutantValue += DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[0]["PM10"]), decimalNum).ToString() + " mg/m<sup>3</sup>,";
                                    }
                                }
                            }
                            if (dt.Rows[0]["PrimaryPollutant"].ToString().Contains("CO"))
                            {
                                DataTable dtFactor = s_AQICalculateService.GetPollutantUnit("a21005");
                                if (dtFactor.Rows.Count > 0)
                                {
                                    int decimalNum = Convert.ToInt32(dtFactor.Rows[0]["DecimalDigit"]);

                                    if (dtFactor.Rows[0]["MeasureUnitName"].Equals("μg/m³"))
                                    {
                                        PollutantValue += (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[0]["CO"]), decimalNum) * 1000).ToString("G0") + " μg/m<sup>3</sup>,";

                                    }
                                    else
                                    {
                                        PollutantValue += DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[0]["CO"]), decimalNum).ToString() + " mg/m<sup>3</sup>,";
                                    }
                                }
                            }
                            if (dt.Rows[0]["PrimaryPollutant"].ToString().Contains("O3"))
                            {
                                DataTable dtFactor = s_AQICalculateService.GetPollutantUnit("a05024");
                                if (dtFactor.Rows.Count > 0)
                                {
                                    int decimalNum = Convert.ToInt32(dtFactor.Rows[0]["DecimalDigit"]);

                                    if (dtFactor.Rows[0]["MeasureUnitName"].Equals("μg/m³"))
                                    {
                                        PollutantValue += (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[0]["Max8HourO3"]), decimalNum) * 1000).ToString("G0") + " μg/m<sup>3</sup>,";
                                        //log.Info("日均值：" + PollutantValue);
                                    }
                                    else
                                    {
                                        PollutantValue += DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[0]["Max8HourO3"]), decimalNum).ToString() + " mg/m<sup>3</sup>,";
                                    }
                                }
                            }
                            if (dt.Rows[0]["PrimaryPollutant"].ToString().Contains("PM2.5"))
                            {
                                DataTable dtFactor = s_AQICalculateService.GetPollutantUnit("a34004");
                                if (dtFactor.Rows.Count > 0)
                                {
                                    int decimalNum = Convert.ToInt32(dtFactor.Rows[0]["DecimalDigit"]);

                                    if (dtFactor.Rows[0]["MeasureUnitName"].Equals("μg/m³"))
                                    {
                                        PollutantValue += (DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[0]["PM25"]), decimalNum) * 1000).ToString("G0") + " μg/m<sup>3</sup>,";

                                    }
                                    else
                                    {
                                        PollutantValue += DecimalExtension.GetPollutantValue(Convert.ToDecimal(dt.Rows[0]["PM25"]), decimalNum).ToString() + " mg/m<sup>3</sup>,";
                                    }
                                }
                            }
                            PollutantValue = PollutantValue.Substring(0, PollutantValue.Length - 1);
                        }
                        #endregion
                        lblLastPollutantValue.Text = PollutantValue;
                        //lblLastPrimaryPollutantValue.Text = "--";
                        //lblLastPollutantValue.Text = "--";
                    }
                    
                }
                #region 动态计算区域南通市辖区昨日AQI(未用)
                //string NTRegionPointIds = ConfigurationManager.AppSettings["NTRegionPointId"].ToString();
                //string[] PointIds = NTRegionPointIds.Split(',');
                //DateTime Tstamp = DateTime.Now.AddDays(-1);
                //int? AQI_SO2 = null, AQI_NO2 = null, AQI_PM10 = null, AQI_CO = null, AQI_O3 = null, AQI_PM25 = null;
                //#region AQI_SO2
                ////浓度
                //decimal? SO2Value = s_AQICalculateService.GetRegionValue(PointIds, "a21026", Tstamp, 24, "1");
                ////分指数
                //if (SO2Value != null)
                //{
                //    AQI_SO2 = s_AQICalculateService.GetIAQI("a21026", Convert.ToDouble(SO2Value), 24);
                //}
                //#endregion
                //#region AQI_NO2
                ////浓度
                //decimal? NO2Value = s_AQICalculateService.GetRegionValue(PointIds, "a21004", Tstamp, 24, "1");
                ////分指数
                //if (NO2Value != null)
                //{
                //    AQI_NO2 = s_AQICalculateService.GetIAQI("a21004", Convert.ToDouble(NO2Value), 24);
                //}
                //#endregion
                //#region AQI_PM10
                ////浓度
                //decimal? PM10Value = s_AQICalculateService.GetRegionValue(PointIds, "a34002", Tstamp, 24, "1");
                ////分指数
                //if (PM10Value != null)
                //{
                //    AQI_PM10 = s_AQICalculateService.GetIAQI("a34002", Convert.ToDouble(PM10Value), 24);
                //}
                //#endregion
                //#region AQI_CO
                ////浓度
                //decimal? COValue = s_AQICalculateService.GetRegionValue(PointIds, "a21005", Tstamp, 24, "1");
                ////分指数
                //if (COValue != null)
                //{
                //    AQI_CO = s_AQICalculateService.GetIAQI("a21005", Convert.ToDouble(COValue), 24);
                //}
                //#endregion
                //#region AQI_O3
                ////浓度
                //decimal? O3Value = s_AQICalculateService.GetRegionValue(PointIds, "a05024", Tstamp, 8, "1");
                ////分指数
                //if (O3Value != null)
                //{
                //    AQI_O3 = s_AQICalculateService.GetIAQI("a05024", Convert.ToDouble(O3Value), 8);
                //}
                //#endregion
                //#region AQI_PM25
                ////浓度
                //decimal? PM25Value = s_AQICalculateService.GetRegionValue(PointIds, "a34004", Tstamp, 24, "1");
                ////分指数
                //if (PM25Value != null)
                //{
                //    AQI_PM25 = s_AQICalculateService.GetIAQI("a34004", Convert.ToDouble(PM25Value), 24);
                //}
                //#endregion

                //string AQI = s_AQICalculateService.GetAQI_Max_CNV(AQI_SO2, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "V");
                //string Max_FactorCode = s_AQICalculateService.GetAQI_Max_CNV(AQI_SO2, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "C");
                //string PrimaryPollutant = s_AQICalculateService.GetAQI_Max_CNV(AQI_SO2, AQI_NO2, AQI_PM10, AQI_CO, AQI_O3, AQI_PM25, "N");

                //lblLastAQIValue.Text = AQI;
                //if (!string.IsNullOrWhiteSpace(AQI))
                //{
                //    string AQIColor = s_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQI), "Color");
                //    string AQIClass = s_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQI), "Class");
                //    string AQIAlabo = s_AQICalculateService.GetAQI_Grade(Convert.ToInt32(AQI), "Alabo");
                //    //lblAQIValue.Style["color"] = AQIColor;//RGB颜色值
                //    lblLastClassValue.Text = AQIClass;
                //    //lblClass.Style["background-color"] = AQIColor;//RGB颜色值
                //    //imgSymbol.Src = "~/Resources/Images/HomePage/smile/" + AQIAlabo + ".png";
                //}
                ////lblHour.Text = Tstamp.ToString("HH") + "时";
                //lblLastPrimaryPollutantValue.Text = PrimaryPollutant;
                //#region 浓度值
                //string value = string.Empty;
                //if (string.IsNullOrWhiteSpace(AQI))
                //{
                //    value = null;
                //}
                //else if (Convert.ToInt32(AQI) <= 50)
                //{
                //    value = "--";
                //}
                //else if (!string.IsNullOrWhiteSpace(PrimaryPollutant))
                //{

                //    if (Max_FactorCode.Contains("a21026"))
                //    {
                //        DataTable dtFactor = s_AQICalculateService.GetPollutantUnit("a21026");
                //        if (dtFactor.Rows.Count > 0)
                //        {
                //            int decimalNum = Convert.ToInt32(dtFactor.Rows[0]["DecimalDigit"]);

                //            if (dtFactor.Rows[0]["MeasureUnitName"].Equals("μg/m³"))
                //            {
                //                value += DecimalExtension.GetPollutantValue(Convert.ToDecimal(SO2Value * 1000), decimalNum).ToString() + " " + dtFactor.Rows[0]["MeasureUnitName"].ToString();

                //            }
                //            else
                //            {
                //                value += DecimalExtension.GetPollutantValue(Convert.ToDecimal(SO2Value), decimalNum).ToString() + " " + dtFactor.Rows[0]["MeasureUnitName"].ToString();
                //            }
                //        }

                //    }
                //    if (Max_FactorCode.Contains("a21004"))
                //    {
                //        DataTable dtFactor = s_AQICalculateService.GetPollutantUnit("a21004");
                //        if (dtFactor.Rows.Count > 0)
                //        {
                //            int decimalNum = Convert.ToInt32(dtFactor.Rows[0]["DecimalDigit"]);
                //            if (dtFactor.Rows[0]["MeasureUnitName"].Equals("μg/m³"))
                //            {
                //                value += DecimalExtension.GetPollutantValue(Convert.ToDecimal(NO2Value * 1000), decimalNum).ToString() + " " + dtFactor.Rows[0]["MeasureUnitName"].ToString();
                //            }
                //            else
                //            {
                //                value += DecimalExtension.GetPollutantValue(Convert.ToDecimal(NO2Value), decimalNum).ToString() + " " + dtFactor.Rows[0]["MeasureUnitName"].ToString();
                //            }
                //        }
                //    }
                //    if (Max_FactorCode.Contains("a34002"))
                //    {
                //        DataTable dtFactor = s_AQICalculateService.GetPollutantUnit("a34002");
                //        if (dtFactor.Rows.Count > 0)
                //        {
                //            int decimalNum = Convert.ToInt32(dtFactor.Rows[0]["DecimalDigit"]);

                //            if (dtFactor.Rows[0]["MeasureUnitName"].Equals("μg/m³"))
                //            {
                //                value += DecimalExtension.GetPollutantValue(Convert.ToDecimal(PM10Value * 1000), decimalNum).ToString() + " " + dtFactor.Rows[0]["MeasureUnitName"].ToString();
                //            }
                //            else
                //            {
                //                value += DecimalExtension.GetPollutantValue(Convert.ToDecimal(PM10Value), decimalNum).ToString() + " " + dtFactor.Rows[0]["MeasureUnitName"].ToString();
                //            }
                //        }
                //    }
                //    if (Max_FactorCode.Contains("a21005"))
                //    {
                //        DataTable dtFactor = s_AQICalculateService.GetPollutantUnit("a21005");
                //        if (dtFactor.Rows.Count > 0)
                //        {
                //            int decimalNum = Convert.ToInt32(dtFactor.Rows[0]["DecimalDigit"]);

                //            if (dtFactor.Rows[0]["MeasureUnitName"].Equals("μg/m³"))
                //            {
                //                value += DecimalExtension.GetPollutantValue(Convert.ToDecimal(COValue * 1000), decimalNum).ToString() + " " + dtFactor.Rows[0]["MeasureUnitName"].ToString();
                //            }
                //            else
                //            {
                //                value += DecimalExtension.GetPollutantValue(Convert.ToDecimal(COValue), decimalNum).ToString() + " " + dtFactor.Rows[0]["MeasureUnitName"].ToString();
                //            }
                //        }
                //    }
                //    if (Max_FactorCode.Contains("a05024"))
                //    {
                //        DataTable dtFactor = s_AQICalculateService.GetPollutantUnit("a05024");
                //        if (dtFactor.Rows.Count > 0)
                //        {
                //            int decimalNum = Convert.ToInt32(dtFactor.Rows[0]["DecimalDigit"]);

                //            if (dtFactor.Rows[0]["MeasureUnitName"].Equals("μg/m³"))
                //            {
                //                value += DecimalExtension.GetPollutantValue(Convert.ToDecimal(O3Value * 1000), decimalNum).ToString() + " " + dtFactor.Rows[0]["MeasureUnitName"].ToString();
                //            }
                //            else
                //            {
                //                value += DecimalExtension.GetPollutantValue(Convert.ToDecimal(O3Value), decimalNum).ToString() + " " + dtFactor.Rows[0]["MeasureUnitName"].ToString();
                //            }
                //        }
                //    }
                //    if (Max_FactorCode.Contains("a34004"))
                //    {
                //        DataTable dtFactor = s_AQICalculateService.GetPollutantUnit("a34004");
                //        if (dtFactor.Rows.Count > 0)
                //        {
                //            int decimalNum = Convert.ToInt32(dtFactor.Rows[0]["DecimalDigit"]);

                //            if (dtFactor.Rows[0]["MeasureUnitName"].Equals("μg/m³"))
                //            {
                //                value += DecimalExtension.GetPollutantValue(Convert.ToDecimal(PM25Value * 1000), decimalNum).ToString() + " " + dtFactor.Rows[0]["MeasureUnitName"].ToString();
                //            }
                //            else
                //            {
                //                value += DecimalExtension.GetPollutantValue(Convert.ToDecimal(PM25Value), decimalNum).ToString() + " " + dtFactor.Rows[0]["MeasureUnitName"].ToString();
                //            }
                //        }
                //    }
                //}
                //lblLastPollutantValue.Text = value;
                //#endregion
                #endregion
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
        }

    }
}