using Aspose.Cells;
using Aspose.Cells.Charts;
using Aspose.Words;
using SmartEP.Core.Generic;
using SmartEP.DomainModel.MonitoringBusiness;
using SmartEP.Service.DataAnalyze.Air.AQIReport;
using SmartEP.Service.DataAnalyze.Air.DataQuery;
using SmartEP.Service.DataAnalyze.Report;
using SmartEP.Service.DataAnalyze.Water;
using SmartEP.Utilities.Office;
using SmartEP.Utilities.Web.UI;
using SmartEP.WebUI.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.Report
{
    /// <summary>
    /// 名称：AirQualittyMonthReport.cs
    /// 创建人：王秉晟
    /// 创建日期：2016-03-10
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：环境空气质量月报
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class AirQualittyMonthReport : BasePage
    {
        /// 数据处理服务
        /// </summary>
        private YearAQIService m_YearAQIService = new YearAQIService();
        private MonthAQIService m_MonthAQIService = new MonthAQIService();
        ReportLogService ReportLogService = new ReportLogService();
        DataQueryByDayService m_DataQueryByDayService = Singleton<DataQueryByDayService>.GetInstance();
        ReportContentService m_ReportContentService = new ReportContentService();
        DateTime dt = DateTime.Now;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.ViewState["DisplayPerson"] = PageHelper.GetQueryString("DisplayPerson");
                InitControl();
            }
        }

        #region 页面控件初始化
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            rdcbxYear.Items.Clear();//年开始
            int yearNow = DateTime.Now.Year;
            int years = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["Year"]);
            int monthNow = DateTime.Now.Month;

            for (int i = yearNow; i >= years; i--)
            {
                //年开始
                RadComboBoxItem cmbItemYearBegin = new RadComboBoxItem();
                cmbItemYearBegin.Text = i.ToString();
                cmbItemYearBegin.Value = i.ToString();
                if (monthNow == 1)
                {
                    if (i == yearNow - 1)
                        cmbItemYearBegin.Selected = true;
                }
                else
                {
                    if (i == yearNow)
                        cmbItemYearBegin.Selected = true;
                }
                rdcbxYear.Items.Add(cmbItemYearBegin);
            }
            rdcbxYear.DataBind();

            rdcbxJiYear.Items.Clear();//基数年开始
            BindType();
            if (rdcbxJiYear.Items.Count > 0)
                rdcbxJiYear.Items[0].Selected = true;

            rdcbxMonth.Items.Clear();//月开始
            for (int i = 1; i <= 12; i++)
            {
                RadComboBoxItem cmbItemMonthBegin = new RadComboBoxItem();
                cmbItemMonthBegin.Text = i.ToString();
                cmbItemMonthBegin.Value = i.ToString();
                if (monthNow > 1)
                {
                    if (i == (monthNow - 1))
                        cmbItemMonthBegin.Selected = true;
                }
                else
                {
                    if (i == 12)
                        cmbItemMonthBegin.Selected = true;
                }
                rdcbxMonth.Items.Add(cmbItemMonthBegin);
            }

            SetDateFromTo();
            Bind();
        }
        #endregion

        #region 绑定基数类型
        public void BindType()
        {
            DataTable dvType = m_DataQueryByDayService.GetCheckRegionDataType();
            rdcbxJiYear.DataSource = dvType;
            rdcbxJiYear.DataTextField = "DataType";
            rdcbxJiYear.DataValueField = "DataType";
            rdcbxJiYear.DataBind();

            if (rdcbxJiYear.Items.Count == 0)
            {
                rdcbxJiYear.Items.Add(new RadComboBoxItem("2013", "2013"));
            }
        }
        #endregion

        #region 绑定数据
        public void Bind()
        {
            string yearSelected = rdcbxYear.SelectedValue;
            string yearJiShu = rdcbxJiYear.SelectedValue;
            string monthSelected = rdcbxMonth.SelectedValue;
            DateTime selectDateTime = DateTime.TryParse(yearSelected + "-" + monthSelected + "-01", out selectDateTime)
                        ? selectDateTime : new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);//选择的月
            DateTime dateNow = selectDateTime;//.AddMonths(-1);
            string yearNow = dateNow.Year.ToString();
            string monthNow = dateNow.Month.ToString();
            yearJiShu = string.IsNullOrWhiteSpace(yearJiShu) ? yearNow : yearJiShu;
            DateTime dtimeBegin = DateTime.TryParse(yearNow + "-" + monthNow + "-01", out dtimeBegin)
                        ? dtimeBegin : new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);//当前月
            DateTime dtimeEnd = (DateTime.TryParse(yearNow + "-" + monthNow + "-01", out dtimeEnd)
                        ? dtimeEnd : new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)).AddMonths(1).AddDays(-1);
            string titleText = string.Empty;
            //if (dtimeEnd > DateTime.Now)
            //    dtimeEnd = DateTime.Now;
            string strNowYearMonth = dtimeBegin.ToString("yyyy年M月");//当前月
            string yearLast = dtimeBegin.AddYears(-1).Year.ToString();
            string strMonthRange = (monthNow == "1") ? "1月" : "1～" + monthNow + "月";
            //Document doc = new Document(System.IO.Path.Combine(MapPath("DocumentsTemplet"), "AirQualittyMonthReport.doc"));
            //DocumentBuilder builder = new DocumentBuilder(doc);
            DataSet ds = m_MonthAQIService.GetRegionsMonthAllData(dtimeBegin, dtimeEnd, yearJiShu);
            DataTable dtText = ds.Tables["Text"];
            DataTable dtBiaoAirInfo = ds.Tables["BiaoAirInfo"];
            DataTable dtBiao1 = ds.Tables["Biao1"];
            DataTable dtBiao2 = ds.Tables["Biao2"];
            DataTable dtBiao3 = ds.Tables["Biao3"];
            DataTable dtBiao4 = ds.Tables["Biao4"];
            DataTable dtTu1 = ds.Tables["Tu1"];
            DataTable dtTu2 = ds.Tables["Tu2"];
            DataTable dtNew = new DataTable();
            lblDate.Text = strNowYearMonth;
            lblTu1.Text = strNowYearMonth + "全市环境空气质量类别分布";//图1文字
            lblTu2.Text = strNowYearMonth + "苏州市区环境空气质量类别分布";//图2文字
            lblTu3.Text = strNowYearMonth + "全市PM2.5月均浓度分布";//图3文字
            lblTu4.Text = strNowYearMonth + "全市PM10月均浓度分布";//图4文字
            lblTu5.Text = strNowYearMonth + "全市NO2月均浓度分布";//图5文字
            lblTu6.Text = strNowYearMonth + "全市SO2月均浓度分布";//图6文字
            lblTu7.Text = strNowYearMonth + "全市CO月均浓度分布";//图7文字
            lblTu8.Text = strNowYearMonth + "全市酸雨频率示意图";//图8文字
            lblTu9.Text = strNowYearMonth + "降水量和AQI逐日变化（降水：毫米）";//图9文字
            lblTu10.Text = strNowYearMonth + "最高气温、平均风速、相对湿度和AQI逐日变化";//图10文字
            lblBiao1.Text = strNowYearMonth + "苏州市环境空气质量";//表1文字
            lblBiao2.Text = strNowYearMonth + "国控点环境空气质量";//表1文字
            lblBiao3.Text = strNowYearMonth + "苏州市各项污染物月均值";//表2文字
            lblBiao4.Text = strNowYearMonth + "苏州市降水监测结果统计";//表3文字
            dtNew = m_MonthAQIService.GetMonthReportTimeDataTable(dtimeBegin, dtimeEnd);
            #region 文本信息
            if (dtText.Rows.Count > 0)
            {
                DataRow drText = dtText.Rows[0];

                #region 环境空气质量重点信息
                txtImportant1.Text = string.Format("截止{0}底，苏州市区PM2.5浓度平均值为{1}微克/立方米；"
                                    + "与上年同期{2}，与{4}年同期{3}。",
                                    strNowYearMonth, drText["Important1_PM25"], drText["Important1_LastBi"], drText["Important1_JiShuBi"],
                                    yearJiShu);
                txtImportant2.Text = string.Format("截止{0}底，苏州市区环境空气质量达标率为{1}，与上年同期{2}；"
                                   + "苏州全市环境空气质量达标率为{3}，与上年同期{4}。",
                                   strNowYearMonth, drText["Important2_DaBiaoRateShiQu"], drText["Important2_LastBiShiQu"],
                                   drText["Important2_DaBiaoRateQuanShi"], drText["Important2_LastBiQuanShi"]);
                txtImportant3.Text = string.Format("{0}，苏州市区酸雨频率为【】，与上年{1}月相比上升了【】个百分点；"
                                   + "苏州全市酸雨频率为【】%，与上年{1}月相比上升了【】个百分点。",
                                   strNowYearMonth, monthNow);
                #endregion

                #region 一、苏州市环境空气质量
                txtM101.Text = string.Format("    按照《环境空气质量标准》（GB3095-2012）评价，{0}，"
                        + "苏州市区、吴江区和四市（县）环境空气质量达标率介于{1}之间（表1），AQI介于{2}之间。"
                        + "全市环境空气质量达标率为{3}（图1）。在超标天数中，以{4}为首要污染物天数最多{5}",
                        strNowYearMonth, drText["M101_DaBiaoRateRange"], drText["M101_AQIRange"],
                        drText["M101_DaBiaoRateQuanShi"], drText["M101_ChaoFactorOneQuanShi"],
                        drText["M101_ChaoFactorTowQuanShi"]);
                txtM102.Text = string.Format("    苏州市区AQI介于{0}之间，本月AQI均值为{1}，"
                    + "环境空气质量达标率为{2}（图2）。在超标天数中，以{3}为首要污染物天数最多{4}",
                    drText["M102_AQIRange"], drText["M102_AQIAvg"], drText["M102_DaBiaoRate"],
                    drText["M102_ChaoFactorOne"], drText["M102_ChaoFactorTwo"]);
                txtM103.Text = string.Format("    与上年{0}月相比，{1}。"
                            + "与上年{0}月相比，苏州市区环境空气质量达标率{2}。",
                            monthNow, drText["M103_LastToNowDaRateQuanShi"],
                            drText["M103_LastToNowDaRateShiQu"]);
                txtM106.Text = string.Format("    与上年{0}月相比，{1}，其中{2}点位下降幅度最大（表2）。",
                         monthNow, drText["M106"], drText["M106_DuiBi"]);
                txtM104.Text = string.Format("注：市区按国控评价点位均值统计；吴江区及四县（市）按省控点位纳入统计。");
                txtM105.Text = string.Format("    全市降水pH月均值为【】，酸雨频率为【】%，与上年{0}月相比上升了【】个百分点。"
                            + "其中苏州市区降水pH月均值为【】，酸雨频率为【】%，与上年{0}月相比上升了【】个百分点。", monthNow);
                #endregion

                #region 二、主要污染物状况
                string msgM202PM25 = string.Empty;
                string msgM203PM10 = string.Empty;
                string msgM204NO2 = string.Empty;
                string msgM204SO2 = string.Empty;
                string msgM204CO = string.Empty;
                string msgM204O38 = string.Empty;
                txtM201.Text = string.Format("    {0}，{1}；{2}。",
                            strNowYearMonth, drText["M201_FactorDuiBiQuanShi"], drText["M201_FactorDuiBiLastMQuanShi"]);

                if (drText["M202_PM25RangeQuanShi"].ToString().Trim().StartsWith("全市"))
                {
                    msgM202PM25 = drText["M202_PM25RangeQuanShi"].ToString();
                }
                else
                {
                    msgM202PM25 = string.Format("    全市PM2.5月均浓度范围为{0}微克/立方米（图3），"
                                    + "苏州市区月均浓度为{1}微克/立方米，{2}，{3}。",
                                    drText["M202_PM25RangeQuanShi"], drText["M202_PM25ShiQu"],
                                    drText["M202_PM25BiLastYShiQu"], drText["M202_PM25BiLastMShiQu"]);
                }
                if (drText["M203_PM10RangeQuanShi"].ToString().Trim().StartsWith("全市"))
                {
                    msgM203PM10 = drText["M203_PM10RangeQuanShi"].ToString();
                }
                else
                {
                    msgM203PM10 = string.Format("    全市PM10月均浓度范围为{0}微克/立方米（图4），"
                                    + "苏州市区月均浓度为{1}微克/立方米，{2}，{3}。",
                                    drText["M203_PM10RangeQuanShi"], drText["M203_PM10ShiQu"],
                                    drText["M203_PM10BiLastYShiQu"], drText["M203_PM10BiLastMShiQu"]);
                }
                if (drText["M204_NO2RangeQuanShi"].ToString().Trim().StartsWith("全市"))
                {
                    msgM204NO2 = drText["M204_NO2RangeQuanShi"].ToString();
                }
                else
                {
                    msgM204NO2 = string.Format("    全市NO2月均浓度范围为{0}微克/立方米（图5），"
                        + "苏州市区月均浓度为{1}微克/立方米，{2}，{3}。\r\n",
                        drText["M204_NO2RangeQuanShi"], drText["M204_NO2ShiQu"], drText["M204_NO2BiLastYShiQu"],
                        drText["M204_NO2BiLastMShiQu"]);
                }

                if (drText["M204_SO2RangeQuanShi"].ToString().Trim().StartsWith("全市"))
                {
                    msgM204SO2 = drText["M204_SO2RangeQuanShi"].ToString();
                }
                else
                {
                    msgM204SO2 = string.Format("    全市SO2月均浓度范围为{0}微克/立方米（图6），"
                        + "苏州市区月均浓度为{1}微克/立方米，{2}，{3}。\r\n",
                        drText["M204_SO2RangeQuanShi"], drText["M204_SO2ShiQu"],
                        drText["M204_SO2BiLastYShiQu"], drText["M204_SO2BiLastMShiQu"]);
                }
                if (drText["M204_CORangeQuanShi"].ToString().Trim().StartsWith("全市"))
                {
                    msgM204CO = drText["M204_CORangeQuanShi"].ToString();
                }
                else
                {
                    msgM204CO = string.Format("    全市CO月均浓度范围为{0}毫克/立方米（图7），"
                        + "苏州市区月均浓度为{1}微克/立方米，{2}，{3}。\r\n",
                        drText["M204_CORangeQuanShi"], drText["M204_COShiQu"], drText["M204_COBiLastYShiQu"],
                        drText["M204_COBiLastMShiQu"]);
                }
                if (drText["M204_O38RangeQuanShi"].ToString().Trim().StartsWith("全市"))
                {
                    msgM204O38 = drText["M204_O38RangeQuanShi"].ToString();
                }
                else
                {
                    msgM204O38 = string.Format("    全市O3月均浓度范围为{0}微克/立方米，"
                        + "苏州市区月均浓度为{1}微克/立方米，{2}，{3}。",
                        drText["M204_O38RangeQuanShi"], drText["M204_O38ShiQu"],
                        drText["M204_O38BiLastYShiQu"], drText["M204_O38BiLastMShiQu"]);
                }
                txtM202.Text = msgM202PM25;
                txtM203.Text = msgM203PM10;
                txtM204.Text = msgM204NO2 + msgM204SO2 + msgM204CO + msgM204O38.TrimEnd('\\', 'r', '\\', 'n');
                #endregion

                #region 三、酸雨状况
                txtM301.Text = string.Format("    {0}，全市共采集降水样品【】个，其中酸雨样品（pH＜5.6）【】个，"
                        + "酸雨频率为【】%，全市降水pH均值为【】。苏州市区、吴江区和四市（县）降水pH均值在【】～【】之间；"
                        + "酸雨频率介于【】%～【】%，其中市区最高（见表3）。与上年{1}月相比，常熟酸雨频率持平，"
                        + "其余地区酸雨频率均有所上升（图8）。",
                        strNowYearMonth, monthNow);
                #endregion

                #region 四、空气质量气象条件分析
                txtM401.Text = string.Format("    进入{0}月，随着冬季到来，气温下降，边界层高度降低，冷空气活动增多，"
                            + "空气污染扩散气象条件总体趋于转差，较容易导致重污染天气出现。"
                            + "{0}月市区降水量【】毫米，较常年同期偏多【】%，属于异常偏多。"
                            + "降水日数【】天，比常年同期偏多【】天。降水主要集中在【】日前，"
                            + "受其影响，该时段内AQI总体维持较低，以良或轻度污染为主。"
                            + "【】日起，随着冷空气影响，降水结束，空气质量明显趋于转差。",
                            monthNow);
                txtM402.Text = string.Format("    12月市区平均气温【】℃，较常年同期偏高【】℃，属于偏高。"
                                    + "其中上、中、下旬分别为【】℃、【】℃、【】℃。总日照时数【】小时，较常年同期偏少【】%，属于偏少。"
                                    + "中、下旬冷空气活动偏多，气温较上旬有所下降。其中，【】日、【】日、【】日前后冷空气较强，"
                                    + "对空气质量无明显不利影响；下半月多弱冷空气，受此影响，中重度污染天气时有发生。\r\n"
                                    + "    {0}月平均相对湿度【】%，上、中、下旬分别为【】%、【】%、【】%。上旬相对湿度偏高与多降水有关；"
                                    + "下旬降水较弱，持续时间短，但平均相对湿度达到【】%，阴天较多，有利于颗粒物吸湿性增长，"
                                    + "对空气质量造成不利影响。\r\n"
                                    + "    {0}月平均风速【】m/s，上、中、下旬分别为【】m/s、【】m/s、【】m/s。"
                                    + "中、下旬多弱冷空气，导致风速较上旬有所下降，风向以西北风、偏西风居多，"
                                    + "有利于上游污染物输入，造成短时污染。\r\n"
                                    + "    各旬空气污染气象条件分析：\r\n"
                                    + "    上旬降水量显著偏多，有一次强冷空气影响，空气污染气象条件【】级，较好。\r\n"
                                    + "    中旬前期有强冷空气南下，【】日后降水停止，受弱冷空气影响，空气质量明显转差，"
                                    + "出现【】天重度污染，但随着【】日偏北风力加大又迅速好转。中旬空气污染气象条件总体【】级，一般。\r\n"
                                    + "    下旬降水总体较弱，但湿度明显偏大；多弱冷空气和均压场等静稳天气影响，"
                                    + "空气质量总体较上、中旬明显偏差，出现【】天重度污染。总体来看，下旬空气污染气象条件【】级，较差。",
                                    monthNow);
                #endregion
            }
            #endregion

            #region 苏州市环境空气PM2.5浓度和达标天数比例情况
            lblPM25Last.Text = string.Format("{0}年", yearLast);
            lblPM25Now.Text = string.Format("{0}年", yearNow);
            lblPM25Ji.Text = string.Format("与{0}年", yearJiShu);
            lblDaBiaoJi.Text = string.Format("{0}年", yearLast);//原来以为是基数年，确认后发现是上一年，现在以上一年为准
            lblDaBiaoNow.Text = string.Format("{0}年", yearNow);
            lblDaBiaoLast.Text = string.Format("与{0}年", yearLast);
            lblMonthRange1.Text = strMonthRange;
            lblMonthRange2.Text = strMonthRange;
            lblMonthRange3.Text = strMonthRange;
            lblMonthRange4.Text = strMonthRange;
            BindTableFieldByDataTable(tbBiaoImportant, dtBiaoAirInfo);
            #endregion

            #region 表1  XXXX年XX月苏州市环境空气质量
            lblBiao1LastMonthRate.Text = string.Format("上年{0}月达标率（%）", monthNow);

            //用DataTable中的数据绑定Table中的控件
            BindTableFieldByDataTable(tbBiao1, dtBiao1);
            #endregion
            #region 表2  XXXX年XX月苏州市环境空气质量
            lblBiao2LastMonthRate.Text = string.Format("上年{0}月达标率（%）", monthNow);

            //用DataTable中的数据绑定Table中的控件
            BindTableFieldByDataTable(tbBiao2, dtBiao2);
            #endregion

            #region 表3  XXXX年XX月苏州市各项污染物月均值
            //用DataTable中的数据绑定Table中的控件
            BindTableFieldByDataTable(tbBiao3, dtBiao3);
            #endregion

            #region 表3  XXXX年XX月苏州市降水监测结果统计
            //用DataTable中的数据绑定Table中的控件
            BindTableFieldByDataTable(tbBiao4, dtBiao4);
            #endregion

            #region 生成图片
            #region 图
            string filename = "imgTu";
            int k = 1;
            string[] titleArray = new string[2];

            for (k = 1; k <= 2; k++)
            {
                string folderPath = filename + k.ToString() + ".xls";
                string mySaveFolder = System.Web.HttpContext.Current.Server.MapPath(".") + @"\Tmp\" + folderPath + @"";
                DataTable dtTu = dtTu1;
                DataRow drTuTemp = null;
                DataColumn dcTuTemp = null;
                decimal total = 0;
                decimal minValue = 1;

                if (k == 2)
                {
                    dtTu = dtTu2;
                }
                foreach (DataRow drTu in dtTu.Rows)
                {
                    for (int m = 0; m < dtTu.Columns.Count; m++)
                    {
                        DataColumn dcTu = dtTu.Columns[m];
                        decimal value = decimal.TryParse(drTu[dcTu].ToString(), out value) ? value : 0;

                        if (value == 0)
                        {
                            drTu[dcTu] = DBNull.Value;
                            dtTu.Columns.Remove(dcTu);
                            m--;
                        }
                        else
                        {
                            decimal realValue = value / 100;
                            drTu[dcTu] = realValue;
                            total += realValue;

                            if (minValue > realValue)
                            {
                                minValue = realValue;
                                drTuTemp = drTu;
                                dcTuTemp = dcTu;
                            }
                        }
                    }
                }
                if (total < 1)
                {
                    drTuTemp[dcTuTemp] = decimal.Parse(drTuTemp[dcTuTemp].ToString()) + 1 - total;
                }

                //导出excel ，数据源格式固定
                if (ExcelHelper.CreateExcel(dtTu, mySaveFolder, filename + k.ToString()))//[K表示图表个数，例如：6个因子6张图  ]
                {
                    #region 数据转成图
                    string title = "Pie3D" + DateTime.Now.ToString("yyyyMMddHHmmssffff");
                    ToChart(folderPath, title, filename + k.ToString());
                    titleArray[k - 1] = title;
                    #endregion

                    #region 折线图转成图片
                    System.Web.UI.WebControls.Image image = imgTu1;
                    if (k == 2)
                    {
                        image = imgTu2;
                    }
                    ChartToImg(title, filename + k.ToString(), image);
                    #endregion
                }
            }
            ViewState["ImageNames"] = titleArray;
            #endregion
            #endregion
        }
        #endregion

        #region 服务器端控件事件处理
        /// <summary>
        /// ToolBar事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExport_Click(object sender, EventArgs e)
        {
            Document doc = new Document(System.IO.Path.Combine(MapPath("DocumentsTemplet"), "AirQualittyMonthReport.doc"));
            string strTarget = SaveDoc(doc);

            doc.Save(this.Response, "AirQualittyMonthReport.doc", Aspose.Words.ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(Aspose.Words.SaveFormat.Docx));
            Response.End();
        }
        #endregion

        /// <summary>
        /// 保存按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            DateTime dateBegin = Convert.ToDateTime(rdcbxYear.SelectedValue + "-" + rdcbxMonth.SelectedValue + "-01");
            DateTime dateEnd = Convert.ToDateTime(rdcbxYear.SelectedValue + "-" + rdcbxMonth.SelectedValue + "-01").AddMonths(1).AddDays(-1);
            Document doc = new Document(System.IO.Path.Combine(MapPath("DocumentsTemplet"), "AirQualittyMonthReport.doc"));
            string strTarget = SaveDoc(doc);
            string pageid = "AirQualittyMonthReport";
            string[] ptitle;
            Dictionary<string, string> pcontent = new Dictionary<string, string>();
            List<string> r = new List<string>();

            //if (dateEnd > DateTime.Now)
            //    dateEnd = DateTime.Now;

            //循环table中的数据到字典中
            AddTextToDictionaryByHtmlTable(r, pcontent, tbReport);

            ptitle = r.ToArray();

            m_ReportContentService.insertTable(pcontent, ptitle, pageid, dateBegin, dateEnd);
            if (!Directory.Exists(strTarget))
            {
                Alert("保存成功！");
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "reload", "<script type=\"text/javascript\">self.parent.location.reload();</script>", false);
        }

        protected string SaveDoc(Document doc)
        {
            DataTable dtnew = new DataTable();
            DateTime dateBegin = Convert.ToDateTime(rdcbxYear.SelectedValue + "-" + rdcbxMonth.SelectedValue + "-01");
            DateTime dateEnd = Convert.ToDateTime(rdcbxYear.SelectedValue + "-" + rdcbxMonth.SelectedValue + "-01").AddMonths(1).AddDays(-1);
            string strTarget = string.Empty;
            string year = rdcbxJiYear.SelectedValue;
            DocumentBuilder builder = new DocumentBuilder(doc);

            //if (dateEnd > DateTime.Now)
            //    dateEnd = DateTime.Now;

            //循环table中的数据并存到集合中
            AddToDocumentBuilderByHtmlTable(builder, tbReport);

            #region 图片插入Word
            string[] imageNames = ViewState["ImageNames"] as string[];

            for (int k = 1; k <= 2; k++)
            {
                string mark = "imgTu" + k.ToString();

                if (imageNames.Length > k - 1)
                {
                    string imageName = imageNames[k - 1];
                    //string path = Server.MapPath("Tmp/asposeword.docx");
                    //Aspose.Words.Document doc = new Aspose.Words.Document(path);
                    //Aspose.Words.DocumentBuilder builder = new Aspose.Words.DocumentBuilder(doc);

                    Aspose.Words.Drawing.Shape shape2 = new Aspose.Words.Drawing.Shape(doc, Aspose.Words.Drawing.ShapeType.Image);
                    shape2.ImageData.SetImage(Server.MapPath("Cache/" + imageName + ".jpg"));
                    shape2.Width = 200;//500;
                    shape2.Height = 180;
                    if (k == 1)
                    {
                        shape2.HorizontalAlignment = Aspose.Words.Drawing.HorizontalAlignment.None;
                        shape2.VerticalAlignment = Aspose.Words.Drawing.VerticalAlignment.Center;
                    }
                    else
                    {
                        shape2.HorizontalAlignment = Aspose.Words.Drawing.HorizontalAlignment.Outside;
                        shape2.VerticalAlignment = Aspose.Words.Drawing.VerticalAlignment.Top;
                    }
                    if (doc.Range.Bookmarks[mark] != null)
                    {
                        builder.MoveToBookmark(mark);
                        //builder.InsertImage(Server.MapPath("/Templete/img.jpg"));
                        builder.InsertNode(shape2);
                        //builder.Write();
                        //string p = Server.MapPath("TmpNew/asposeword.docx");
                        //doc.Save(p);
                        //System.Diagnostics.Process.Start(p);
                    }
                }
            }
            #endregion

            doc.MailMerge.DeleteFields();
            //    DateTime dtBegin = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-01"));  //本月第一天
            //endDate = Convert.ToDateTime(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd"));   //本月当天
            string[] strPointCodes = { };//站点Code
            string pointCodes = string.Join(";", strPointCodes);
            string[] strPointNames = { };//站点名称
            string pointNames = string.Join(";", strPointNames);
            string filename = "(" + dt.ToString("yyyyMMddHHmmssffff") + ")" + "AirQualittyMonthReport" + ".doc";

            //添加实体类对象
            ReportLogEntity customDatum = new ReportLogEntity();
            customDatum.PointIds = "7e05b94c-bbd4-45c3-919c-42da2e63fd43;4296ce53-78d3-4741-9eda-6306e3e5b399;f7444783-a425-411c-a54b-f9fed72ec72e;d993d02f-fcc3-4ea6-b52b-9414fbd9b8e6;636775d8-091d-4754-9ed2-cd9dfef1f6ab;48d749e6-d07c-4764-8d50-50f170defe0b";//测点Code
            customDatum.PointNames = "市区均值;张家港;常熟市;太仓市;昆山市;吴江区";//测点名称
            customDatum.FactorCodes = "a21026;a21004;a34002;a34004;a21005;a05024";//因子Code
            customDatum.FactorsNames = "二氧化硫;二氧化氮;PM10;PM2.5;一氧化碳;臭氧";//因子名称
            customDatum.DateTimeRange = dateBegin.Year.ToString() + "年" + dateBegin.Month.ToString() + "月";
            customDatum.WaterOrAirType = 1;//水或气 0：水，1：气
            customDatum.PageTypeID = "AirQualittyMonthReport";//页面ID
            customDatum.StartDateTime = dateBegin;
            customDatum.EndDateTime = dateEnd;
            customDatum.CreatUser = this.ViewState["DisplayPerson"].ToString();
            customDatum.ReportName = ("../../../Pages/EnvAir/Report/ReportFile/AirQualittyMonthReport/" + dateBegin.Year + "/" + dateBegin.Month + "/" + filename).ToString();
            customDatum.CreatDateTime = DateTime.Now;

            //添加数据
            ReportLogService.ReportLogAdd(customDatum);

            strTarget = Server.MapPath("../../../Pages/EnvAir/Report/ReportFile/AirQualittyMonthReport/" + dateBegin.Year + "/" + dateBegin.Month + "/" + filename);
            doc.Save(strTarget);

            return strTarget;
        }

        /// <summary>
        /// 循环table中的数据并存到集合中
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="table"></param>
        private void AddToDocumentBuilderByHtmlTable(DocumentBuilder builder, HtmlTable table)
        {
            foreach (HtmlTableRow tr in table.Rows)
            {
                foreach (HtmlTableCell td in tr.Cells)
                {
                    foreach (Control control in td.Controls)
                    {
                        if (control is Label)
                        {
                            Label label = control as Label;
                            builder.MoveToMergeField(label.ID.TrimStart('l', 'b', 'l'));
                            if (label.Style.Count > 0 && label.CssClass.Contains("ExportHtml"))
                            {
                                string divHtml = GetSubTextByText(label.Text);
                                divHtml = divHtml.Insert(0, "<span style=\"" + label.Style.Value + "\" >");
                                divHtml = divHtml.Insert(divHtml.Length, "</span>");
                                builder.InsertHtml(divHtml);
                            }
                            else
                            {
                                builder.Write(label.Text.Replace("    ", ""));
                            }
                        }
                        else if (control is TextBox)
                        {
                            TextBox textBox = control as TextBox;
                            builder.MoveToMergeField(textBox.ID.TrimStart('t', 'x', 't'));
                            if (textBox.Style.Count > 0 && textBox.CssClass.Contains("ExportHtml"))
                            {
                                string divHtml = GetSubTextByText(textBox.Text);
                                divHtml = divHtml.Insert(0, "<span style=\"" + textBox.Style.Value + "\" >");
                                divHtml = divHtml.Insert(divHtml.Length, "</span>");
                                builder.InsertHtml(divHtml);
                            }
                            else
                            {
                                builder.Write(textBox.Text.Replace("    ", ""));
                            }
                        }
                        else if (control is HtmlTable)
                        {
                            HtmlTable tableChild = control as HtmlTable;
                            AddToDocumentBuilderByHtmlTable(builder, tableChild);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 循环table中的数据到字典中
        /// </summary>
        /// <param name="r"></param>
        /// <param name="pcontent"></param>
        /// <param name="table"></param>
        private void AddTextToDictionaryByHtmlTable(List<string> r, Dictionary<string, string> pcontent, HtmlTable table)
        {
            foreach (HtmlTableRow tr in table.Rows)
            {
                foreach (HtmlTableCell td in tr.Cells)
                {
                    foreach (Control control in td.Controls)
                    {
                        int index = r.Count + 1;

                        if (control is Label)
                        {
                            Label label = control as Label;
                            r.Add(index.ToString());
                            pcontent.Add(index.ToString(), label.Text);
                        }
                        else if (control is TextBox)
                        {
                            TextBox textBox = control as TextBox;
                            r.Add(index.ToString());
                            pcontent.Add(index.ToString(), textBox.Text);
                        }
                        else if (control is HtmlTable)
                        {
                            HtmlTable tableChild = control as HtmlTable;
                            AddTextToDictionaryByHtmlTable(r, pcontent, tableChild);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 用DataTable中的数据绑定Table中的控件
        /// </summary>
        /// <param name="table"></param>
        /// <param name="dt"></param>
        private void BindTableFieldByDataTable(HtmlTable table, DataTable dt)
        {
            if (dt == null && dt.Rows.Count == 0)
            {
                return;
            }
            foreach (HtmlTableRow tr in table.Rows)
            {
                foreach (HtmlTableCell td in tr.Cells)
                {
                    foreach (Control control in td.Controls)
                    {
                        if (control is TextBox)
                        {
                            TextBox textBox = control as TextBox;
                            string txtID = textBox.ID;

                            foreach (DataRow dr in dt.Rows)
                            {
                                string regionEn = dr["RegionEn"].ToString();

                                foreach (DataColumn dc in dt.Columns)
                                {
                                    string columnName = dc.ColumnName;

                                    if (txtID == "txt" + columnName + regionEn)
                                    {
                                        textBox.Text = dr[dc].ToString();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        protected void Year_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            SetDateFromTo();
            Bind();
        }

        /// <summary>
        /// 获取加上下标的文字
        /// </summary>
        /// <param name="oldText"></param>
        /// <returns></returns>
        private string GetSubTextByText(string oldText)
        {
            Regex regex = new Regex("[A-Za-z0-9]+-?\\.?[A-Za-z0-9]*%?(～[A-Za-z0-9]+-?\\.?[A-Za-z0-9]*%?)?");
            MatchCollection matches = regex.Matches(oldText);
            string newText = oldText;
            int index = 0;

            foreach (Match match in matches)
            {
                string value = match.Value;
                string valueNew = "<span style=\"font-family: Times New Roman\">" + value + "</span>";
                index = newText.IndexOf(value, index);
                newText = newText.Remove(index, value.Length);
                newText = newText.Insert(index, valueNew);
                index = index + valueNew.Length;
            }
            newText = newText.Trim(' ')
                             .Replace("PM2.5", "PM<sub>2.5</sub>").Replace("PM10", "PM<sub>10</sub>")
                             .Replace("NO2", "NO<sub>2</sub>").Replace("SO2", "SO<sub>2</sub>")
                             .Replace("O3", "O<sub>3</sub>").Replace("\r\n", "<br/>")
                             .Replace("    ", "&nbsp;&nbsp;");


            return newText;
        }

        /// <summary>
        /// 设置开始和结束时间
        /// </summary>
        private void SetDateFromTo()
        {
            string yearSelected = rdcbxYear.SelectedValue;
            string yearJiShu = rdcbxJiYear.SelectedValue;
            string monthSelected = rdcbxMonth.SelectedValue;
            DateTime selectDateTime = DateTime.TryParse(yearSelected + "-" + monthSelected + "-01", out selectDateTime)
                        ? selectDateTime : new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);//选择的月
            DateTime dateNow = selectDateTime;//.AddMonths(-1);
            string yearReal = dateNow.Year.ToString();
            string monthReal = dateNow.Month.ToString();
            DateTime dtB = Convert.ToDateTime(yearReal + "-" + monthReal + "-01");
            DateTime dtE = Convert.ToDateTime(yearReal + "-" + monthReal + "-01").AddMonths(1).AddDays(-1);

            //if (dtE > DateTime.Now)
            //    dtE = DateTime.Now;
            txtDateF.Text = dtB.ToString("yyyy年MM月dd日");
            txtDateT.Text = dtE.ToString("yyyy年MM月dd日");
        }

        #region 图片处理
        /// <summary>
        /// 数据转成chart图表
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="title"></param>
        /// <param name="sheetName"></param>
        private void ToChart(string filename, string title, string sheetName)
        {
            try
            {
                WorkbookDesigner designer = new WorkbookDesigner();
                string path = Server.MapPath("Tmp/" + filename);
                designer.Workbook.Open(path);
                Workbook workbook = designer.Workbook;

                //创建一个chart到页面
                CreateChart(workbook, title, sheetName);
                designer.Process();
                designer.Workbook.Save(Server.MapPath("Cache/" + title + ".xls"));//[建立Cache 文件夹 和Tmp同级在 report文件夹下]
                //Response.Flush();
                //Response.Close();
                designer = null;
                // Response.End();
            }
            catch (Exception e)
            {
                Response.Write(e.ToString());
            }
        }

        /// <summary>
        /// 图转成图片
        /// </summary>
        /// <param name="title"></param>
        private void ChartToImg(string title, string sheetName, System.Web.UI.WebControls.Image image)
        {
            try
            {
                Workbook workbook = new Workbook(Server.MapPath("Cache/" + title + ".xls"));
                Aspose.Cells.Charts.Chart chart = workbook.Worksheets[sheetName].Charts[0];
                int angle = Convert.ToInt16(360 - (90.00 / 100.00) * 360 / 2.00);
                chart.RotationAngle = angle;
                chart.Elevation = 85;
                string imgPath = Server.MapPath("Cache/" + title + ".jpg");
                chart.ToImage(imgPath, ImageFormat.Jpeg);
                image.ImageUrl = "Cache/" + title + ".jpg";
                File.Delete(Server.MapPath("Cache/" + title + ".xls"));
            }
            catch (Exception e)
            {
                Response.Write(e.ToString());
            }
        }

        /// <summary>
        /// 创建图片
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="title"></param>
        /// <param name="sheetName"></param>
        private void CreateChart(Workbook workbook, string title, string sheetName)
        {
            try
            {
                //创建一个图
                Worksheet worksheet = workbook.Worksheets[0];
                worksheet.Charts.Add(ChartType.Pie3D, 1, 1, 13, 3);
                Cells cells = worksheet.Cells;

                //Aspose.Cells.Drawing.RectangleShape panelDrawing = worksheet.Shapes[0];

                Aspose.Cells.Charts.Chart chart = worksheet.Charts[0];

                chart.ChartArea.Border.Color = Color.White;//chart图标边框不显示
                chart.ChartArea.Area.BackgroundColor = Color.White;
                chart.PlotArea.Area.BackgroundColor = Color.White;
                chart.PlotArea.Border.Color = Color.White;
                //chart.PlotArea.Area= LabelPositionType.Center;
                chart.PlotAreaWithoutTickLabels.Area.BackgroundColor = Color.White;
                chart.PlotAreaWithoutTickLabels.Border.Color = Color.White;
                //折线区域竖线设置为显示颜色设置为灰色
                //chart.CategoryAxis.MajorGridLines.IsVisible = false;
                chart.CategoryAxis.MajorGridLines.Color = Color.White;
                //折线区域设置横着的网格线显示          
                //chart.MajorGridLines.IsVisible = true;
                //chart.MajorGridLines.Color = Color.Gray;

                //设置title样式
                //chart.Title.Text = title + "水质变化趋势";
                //chart.Title.Text = title;
                //chart.Title.Font.Color = Color.Black;
                //chart.Title.Font.IsBold = true;
                //chart.Title.Font.Size = 12;

                //int count = Convert.ToInt32(6);
                //数字英语对照
                //string Eng = GetEng(count);
                //Set Properties of nseries
                //chart.NSeries.Add("Sheet1!B2:" + Eng + "5", false);
                string column = GetExcelColumnByNum(cells.Rows[0].LastCell.Column + 1);
                chart.NSeries.Add(sheetName + "!A2:" + column + "2", false);//[Excel中数据区  ]
                //chart.NSeries.Add("Sheet1!B2:B5", false);

                //Set NSeries Category Datasource
                //chart.NSeries.CategoryData = "Sheet1!B1:" + Eng + "1";
                chart.NSeries.CategoryData = sheetName + "!A1:" + column + "1";//[Excel中标头区  作为类别]
                //chart.NSeries.CategoryData = "Sheet1!A2:A5";

                //chart.NSerie
                //loop over the Nseriese

                for (int i = 0; i < chart.NSeries.Count; i++)
                {
                    Series aSeries = chart.NSeries[i];
                    ChartPointCollection chartPoints = aSeries.Points;
                    string value = "";
                    decimal temp = 1;
                    for (int j = 0; j < chartPoints.Count; j++)
                    {
                        if (temp > Convert.ToDecimal(cells[1, j].Value))
                        {
                            temp = Convert.ToDecimal(cells[1, j].Value);
                            value = cells[0, j].Value.ToString();
                        }
                    }

                    for (int j = 0; j < chartPoints.Count; j++)
                    {
                        ChartPoint point = chartPoints[j];
                        point.Area.ForegroundColor = GetColor(cells[0, j].Value.ToString());
                        point.Border.IsVisible = false;
                        if (value == cells[0, j].Value.ToString())
                            point.Explosion = 3;
                        else
                            point.Explosion = 0;
                    }

                    #region 扇形图
                    ////每个扇形图显示出值
                    chart.NSeries[i].DataLabels.ShowValue = true;
                    chart.NSeries[i].DataLabels.NumberFormat = "0.0%";
                    chart.NSeries[i].DataLabels.ShowCategoryName = false;
                    chart.NSeries[i].DataLabels.Font.Size = 9;
                    chart.NSeries[i].DataLabels.Postion = LabelPositionType.Center;
                    #endregion
                }

                #region 饼图

                //设置x轴上数据的样式为灰色
                chart.CategoryAxis.TickLabels.Font.Color = Color.White;
                chart.CategoryAxis.TickLabelPosition = TickLabelPositionType.NextToAxis;
                chart.CategoryAxis.HasMultiLevelLabels = true;

                //设置y轴的样式
                chart.ValueAxis.TickLabelPosition = TickLabelPositionType.Low;
                chart.ValueAxis.TickLabels.Font.Color = Color.White;
                chart.ValueAxis.TickLabels.NumberFormat = "";
                chart.ValueAxis.HasMultiLevelLabels = true;
                // chart.ValueAxis.TickLabels.TextDirection = TextDirectionType.LeftToRight;
                //设置Legend位置以及样式         
                chart.ShowLegend = true;
                chart.Legend.Position = LegendPositionType.Bottom;
                //chart.Legend.Border.IsVisible = true;
                chart.Legend.TextFont.Color = Color.Black;
                chart.Legend.Border.Color = Color.White;
                chart.Legend.Font.Size = 6;

                #endregion
            }
            catch (Exception e)
            {
                Response.Write(e.ToString());
            }
        }

        /// <summary>
        /// 根据列序号获取颜色
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private Color GetColor(int index)
        {
            Color color = Color.White;

            switch (index)
            {
                case 0:
                    color = Color.LawnGreen;
                    break;
                case 1:
                    color = Color.Yellow;
                    break;
                case 2:
                    color = Color.DarkOrange;
                    break;
                case 3:
                    color = Color.Red;
                    break;
                case 4:
                    color = Color.Purple;
                    break;
                case 5:
                    color = Color.Black;
                    break;
            }
            return color;
        }

        /// <summary>
        /// 根据列名获取颜色
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        private Color GetColor(string columnName)
        {
            Color color = Color.White;

            switch (columnName.Trim())
            {
                case "优":
                    color = Color.Green;
                    break;
                case "良":
                    color = Color.Yellow;
                    break;
                case "轻度污染":
                    color = Color.Orange;
                    break;
                case "中度污染":
                    color = Color.Red;
                    break;
                case "重度污染":
                    color = Color.Purple;
                    break;
                case "严重污染":
                    color = Color.Black;
                    break;
                default: break;
            }

            return color;
        }

        /// <summary>
        /// 根据数字获取相应的EXCEL列字母
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private string GetExcelColumnByNum(int num)
        {
            string column = "A";

            switch (num)
            {
                case 1:
                    column = "A";
                    break;
                case 2:
                    column = "B";
                    break;
                case 3:
                    column = "C";
                    break;
                case 4:
                    column = "D";
                    break;
                case 5:
                    column = "E";
                    break;
                case 6:
                    column = "F";
                    break;
                case 7:
                    column = "G";
                    break;
                case 8:
                    column = "H";
                    break;
                case 9:
                    column = "I";
                    break;
                case 10:
                    column = "J";
                    break;
                default: break;
            }
            return column;
        }
        #endregion
    }
}