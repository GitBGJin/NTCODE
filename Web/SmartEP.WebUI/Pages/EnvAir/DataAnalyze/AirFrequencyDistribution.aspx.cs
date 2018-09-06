using SmartEP.Core.Generic;
using SmartEP.DomainModel.BaseData;
using SmartEP.Service.BaseData.Channel;
using SmartEP.Service.DataAnalyze.Air;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.DataAnalyze
{
    public partial class AirFrequencyDistribution : SmartEP.WebUI.Common.BasePage
    {
        AirFrequencyService g_AirFrequencyService = Singleton<AirFrequencyService>.GetInstance();
        AirPollutantService m_AirPollutantService = Singleton<AirPollutantService>.GetInstance();
        /// <summary>
        /// 选择站点
        /// </summary>
        private IList<SmartEP.Core.Interfaces.IPoint> points = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitControl();
                setUnit();
            }
        }
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            dtpBegin.SelectedDate = new DateTime(DateTime.Now.Year, 1, 1);
            dtpEnd.SelectedDate = DateTime.Now;
            IQueryable<PollutantCodeEntity> Pollutant = m_AirPollutantService.RetrieveListByCalAQI();
            rcbFactors.DataSource = Pollutant;
            rcbFactors.DataTextField = "PollutantName";
            rcbFactors.DataValueField = "PollutantCode";
            rcbFactors.DataBind();
            rcbFactors.SelectedText = "PM2.5";
            //factorCbxRsm.SetFactorValuesFromNames("PM2.5");
        }
        /// <summary>
        /// 绑定grid
        /// </summary>
        public void BindGrid()
        {
            points = pointCbxRsm.GetPoints();
            string[] pointIds = pointCbxRsm.GetPointValues(Core.Enums.CbxRsmReturnType.ID);
            //string[] factorcodes = factorCbxRsm.GetFactorValues(Core.Enums.CbxRsmReturnType.Code);
            //string[] factorNames = factorCbxRsm.GetFactorValues(Core.Enums.CbxRsmReturnType.Name);
            if (pointIds != null)
            {
                string factorcode = rcbFactors.SelectedValue;
                //if (factorcodes.Length > 0)
                //{
                //    factorcode = factorcodes[0];
                //}
                string factorName = rcbFactors.SelectedText;
                //if (factorNames.Length > 0)
                //{
                //    factorName = factorNames[0];
                //}
                DateTime dtStart = DateTime.Parse(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd 00:00:00"));
                DateTime dtEnd = DateTime.Parse(dtpEnd.SelectedDate.Value.ToString("yyyy-MM-dd 23:59:59"));
                DataTable dt = g_AirFrequencyService.getAirFrequencyData(pointIds, factorcode, dtStart, dtEnd);
                if (dt == null)
                {
                    Alert("未查到" + factorName + "配置信息");
                    return;
                }
                gridOriginal.DataSource = dt;
                gridOriginal.VirtualItemCount = dt.Rows.Count;
                if (tabStrip.SelectedTab.Text == "图表")
                {
                    iframeOCM.Attributes.Add("src", "AirFrequencyDistributionChart.aspx?pointIds=" + string.Join(";", pointIds) + "&factor=" + factorcode + "&factorname=" + factorName
                        + "&dtStart=" + dtStart.ToString("yyyy-MM-dd HH:mm:ss") + "&dtEnd=" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss"));
                }
            }
            else
            {
                gridOriginal.DataSource = null;
            }
        }
        /// <summary>
        /// 绑定图表
        /// </summary>
        public void BindChart()
        {
            //string[] pointIds = pointCbxRsm.GetPointValues(Core.Enums.CbxRsmReturnType.ID);
            //string[] factorcodes = factorCbxRsm.GetFactorValues(Core.Enums.CbxRsmReturnType.Code);
            //string factorcode = string.Empty;
            //if (factorcodes.Length > 0)
            //{
            //    factorcode = factorcodes[0];
            //}
            //DateTime dtStart = DateTime.Parse(dtpBegin.SelectedDate.Value.ToString("yyyy-MM-dd 00:00:00"));
            //DateTime dtEnd = DateTime.Parse(dtpEnd.SelectedDate.Value.ToString("yyyy-MM-dd 23:59:59"));
            //DataTable dt = g_AirFrequencyService.getAirFrequencyAllData(pointIds, factorcode, dtStart, dtEnd);
            //hdFrequencyData.Value = ToJson(dt);
            //string hiddenS = "";
            //foreach (DataColumn dc in dt.Columns)
            //{
            //    hiddenS += dc.ColumnName + ";";
            //}
            //hdFrequencyRange.Value = hiddenS;
        }
        /// <summary>
        /// datatable转json
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToJson(DataTable dt)
        {
            StringBuilder jsonString = new StringBuilder();
            jsonString.Append("[");
            DataRowCollection drc = dt.Rows;
            for (int i = 0; i < drc.Count; i++)
            {
                jsonString.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string strKey = dt.Columns[j].ColumnName;
                    string strValue = drc[i][j].ToString();
                    Type type = dt.Columns[j].DataType;
                    jsonString.Append("\"" + strKey + "\":");
                    strValue = StringFormat(strKey, strValue, type);
                    if (j < dt.Columns.Count - 1)
                    {
                        jsonString.Append(strValue + ",");
                    }
                    else
                    {
                        jsonString.Append(strValue);
                    }
                }
                jsonString.Append("},");
            }
            if (jsonString.ToString() != "[") jsonString.Remove(jsonString.Length - 1, 1);
            jsonString.Append("]");
            return jsonString.ToString();
        }
        /// <summary>  
        /// 格式化字符型、日期型、布尔型  
        /// </summary>  
        /// <param name="str"></param>  
        /// <param name="type"></param>  
        /// <returns></returns>  
        private static string StringFormat(string key, string str, Type type)
        {
            if (key.Equals("Tstamp", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(str))
            {
                str = ConvertDateTimeToInt(Convert.ToDateTime(str)).ToString();
            }
            if (key.Equals("DateTime", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(str))
            {
                str = ConvertDateTimeToInt(Convert.ToDateTime(str)).ToString();
            }
            if (type == typeof(string))
            {
                str = String2Json(str);
                str = "\"" + str + "\"";
            }
            else if (type == typeof(DateTime))
            {
                str = "\"" + str + "\"";
            }
            else if (type == typeof(bool))
            {
                str = str.ToLower();
            }
            else if (string.IsNullOrEmpty(str))
            {
                str = "null";
            }
            return str;
        }
        /// <summary>  
        /// 过滤特殊字符  
        /// </summary>  
        /// <param name="s"></param>  
        /// <returns></returns>  
        private static string String2Json(String s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s.ToCharArray()[i];
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\""); break;
                    case '\\':
                        sb.Append("\\\\"); break;
                    case '/':
                        sb.Append("\\/"); break;
                    case '\b':
                        sb.Append("\\b"); break;
                    case '\f':
                        sb.Append("\\f"); break;
                    case '\n':
                        sb.Append("\\n"); break;
                    case '\r':
                        sb.Append("\\r"); break;
                    case '\t':
                        sb.Append("\\t"); break;
                    default:
                        sb.Append(c); break;
                }
            }
            return sb.ToString();
        }
        /// <summary>  
        ///DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time">时间</param>  
        /// <returns>long</returns>  
        public static long ConvertDateTimeToInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.AddHours(-8).Ticks) / 10000;   //除10000调整为13位      
            return t;
        }

        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            gridOriginal.CurrentPageIndex = 0;
            gridOriginal.Rebind();
            if (tabStrip.SelectedTab.Text == "图表")
            {
                //RegisterScript("chart();");
            }
            else
            {
                FirstLoadChart.Value = "1";
            }
        }

        protected void gridOriginal_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }

        protected void gridOriginal_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                DataRowView drv = e.Item.DataItem as DataRowView;
                if (item["pointIds"] != null)
                {
                    GridTableCell pointCell = (GridTableCell)item["pointIds"];
                    SmartEP.Core.Interfaces.IPoint point = points.FirstOrDefault(x => x.PointID.Equals(pointCell.Text.Trim()));
                    if (points != null)
                        pointCell.Text = point.PointName;
                }
            }
        }

        protected void gridOriginal_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {
            try
            {
                GridBoundColumn col = e.Column as GridBoundColumn;
                if (col == null)
                    return;
                if (col.DataField == "pointIds")
                {
                    col.HeaderText = "测点";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(80);
                    col.ItemStyle.Width = Unit.Pixel(80);
                }
                else if (col.DataField == "volidDays")
                {
                    col.HeaderText = "无效天数";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(80);
                    col.ItemStyle.Width = Unit.Pixel(80);
                }
                else
                {
                    col.HeaderText = col.DataField;
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(80);
                    col.ItemStyle.Width = Unit.Pixel(80);
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void factorCbxRsm_SelectedChanged()
        {
            setUnit();
        }
        public void setUnit()
        {
            SmartEP.Core.Interfaces.IPollutant factors = GetPollutantName("Air", rcbFactors.SelectedValue);
            string measureUnit = factors.PollutantMeasureUnit;
            string code = rcbFactors.SelectedValue;
            string name = rcbFactors.SelectedText;
            //foreach (SmartEP.Core.Interfaces.IPollutant factor in factors)
            //{
            //    measureUnit += factor.PollutantMeasureUnit;
            //    code += factor.PollutantCode;
            //    name += factor.PollutantName;
            //}
            unit.Text = measureUnit;
            hdfactorname.Value = name;
            hdunit.Value = measureUnit;
            hdfactorcode.Value = code;
        }

        protected void rcbFactors_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            setUnit();
        }
        /// <summary>
        /// 根据因子编码获取污染物名称
        /// </summary>
        /// <param name="pageType">应用程序类型</param>
        /// <param name="pollutantCode">因子编码</param>
        /// <returns></returns>
        private SmartEP.Core.Interfaces.IPollutant GetPollutantName(string pageType, string pollutantCode)
        {
            try
            {
                if (pageType.Equals("Air"))
                {
                    AirPollutantService airService = new AirPollutantService();
                    return airService.GetPollutantInfo(pollutantCode);
                }
                else if (pageType.Equals("Water"))
                {
                    WaterPollutantService waterService = new WaterPollutantService();
                    return waterService.GetPollutantInfo(pollutantCode);
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }

        }
    }
}