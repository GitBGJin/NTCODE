using SmartEP.Core.Generic;
using SmartEP.Service.AutoMonitoring.Air;
using SmartEP.Utilities.Office;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.SuperStationManagement
{
    public partial class MicrowaveRadiation : SmartEP.WebUI.Common.BasePage
    {
        /// <summary>
        /// 粒径谱数据服务层
        /// </summary>
        MicrowaveRadiationService m_weiBoService = Singleton<MicrowaveRadiationService>.GetInstance();
        protected void Page_Load(object sender, EventArgs e)
        {
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
            //时间框初始化
            dayBegin.SelectedDate = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 00:00:00"));
            dayEnd.SelectedDate = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 23:59:59"));
        }
        #endregion
        #region 绑定grid
        /// <summary>
        /// 绑定grid
        /// </summary>
        private void BindGrid()
        {
                string[] portIds = { cbPoint.SelectedValue };

                string factor = "";
                string factorId = "";
                foreach (RadComboBoxItem item in cbFactor.CheckedItems)
                {
                    factor += (item.Value.ToString() + ",");
                    factorId += ("9" + item.Value.ToString() + ",");
                }
                factor = factor.Trim(',');
                factorId = factor.Trim(',');
                string[] factors = factor.Split(',');
                string[] factorIds = factorId.Split(',');
                //每页显示数据个数            
                int pageSize = gridWeibo.PageSize;
                //当前页的序号
                int pageNo = gridWeibo.CurrentPageIndex;

                DataView auditData = new DataView();

                DateTime dtBegion = dayBegin.SelectedDate.Value;
                DateTime dtEnd = dayEnd.SelectedDate.Value;
                auditData = m_weiBoService.GetWeiboDataPager(portIds, factors, dtBegion, dtEnd);
                gridWeibo.DataSource = auditData;
                gridWeibo.VirtualItemCount = auditData.Count;
            if (tabStrip.SelectedTab.Text == "图表")
            {
                string[] factorCode = { "401", "402", "404" };
                auditData = m_weiBoService.GetWeiboDataPager(portIds, factorCode, dtBegion, dtEnd);
                DataView wendudv = new DataView(auditData.ToTable());
                DataView midudv = new DataView(auditData.ToTable());
                DataView shidudv = new DataView(auditData.ToTable());
                //【给隐藏域赋值，用于显示Chart】    
                string wendu = "";

                wendudv.RowFilter = "factorName='温度'";
                if (int.Parse(HiddenNum.Value) < wendudv.Count)
                {
                    wendu = wendudv[int.Parse(HiddenNum.Value)]["factorName"].ToString() + "时间：" + Convert.ToDateTime(wendudv[int.Parse(HiddenNum.Value)]["DateTime"]).ToString("yyyy-MM-dd HH:mm:ss") + ";";
                }
                hdwenduData.Value = ToJson(wendudv, "温度", HiddenNum.Value);


                string midu = "";

                midudv.RowFilter = "factorName='蒸汽密度'";
                if (int.Parse(HiddenNum.Value) < midudv.Count)
                {
                    midu = midudv[int.Parse(HiddenNum.Value)]["factorName"].ToString() + "时间：" + Convert.ToDateTime(midudv[int.Parse(HiddenNum.Value)]["DateTime"]).ToString("yyyy-MM-dd HH:mm:ss") + ";";
                }
                hdmiduData.Value = ToJson(midudv, "蒸汽密度", HiddenNum.Value);


                string shidu = "";

                shidudv.RowFilter = "factorName='相对湿度'";
                if (int.Parse(HiddenNum.Value) < shidudv.Count)
                {
                    shidu = shidudv[int.Parse(HiddenNum.Value)]["factorName"].ToString() + "时间：" + Convert.ToDateTime(shidudv[int.Parse(HiddenNum.Value)]["DateTime"]).ToString("yyyy-MM-dd HH:mm:ss") + ";";
                }
                hdshiduData.Value = ToJson(shidudv, "相对湿度", HiddenNum.Value);


                string wenduhidden = "";
                foreach (DataColumn dc in auditData.ToTable().Columns)
                {
                    if (IsOrNotNumber(dc.ColumnName))
                    {
                        wenduhidden += dc.ColumnName + ";";
                    }
                }
                hiddendiameter.Value = wenduhidden;
                lbTime.InnerText = wendu + midu + shidu;
            }
            


            //if (tabStrip.SelectedTab.Text == "图表")
            //{
            //    string[] factorCode = { "401", "402", "404" };
            //    auditData = m_weiBoService.GetWeiboDataPager(portIds, factorCode, dtBegion, dtEnd); //月类型 按日数据查询
            //}
            //else
            //{
            //    auditData = m_weiBoService.GetWeiboDataPager(portIds, factors, dtBegion, dtEnd); //月类型 按日数据查询
            //}
            //DataView dv = new DataView(auditData.ToTable());

            //gridWeibo.DataSource = dv;
            //gridWeibo.VirtualItemCount = dv.Count;
            //if (tabStrip.SelectedTab.Text == "图表")
            //{

            //    DataView wendudv = new DataView(auditData.ToTable());
            //    DataView midudv = new DataView(auditData.ToTable());
            //    DataView shidudv = new DataView(auditData.ToTable());
            //    //【给隐藏域赋值，用于显示Chart】    
            //    string wendu = "";

            //    wendudv.RowFilter = "factorName='温度'";
            //    if (int.Parse(HiddenNum.Value) < wendudv.Count)
            //    {
            //        wendu = wendudv[int.Parse(HiddenNum.Value)]["factorName"].ToString() + "时间：" + Convert.ToDateTime(wendudv[int.Parse(HiddenNum.Value)]["DateTime"]).ToString("yyyy-MM-dd HH:mm:ss") + ";";
            //    }
            //    hdwenduData.Value = ToJson(wendudv, "温度", HiddenNum.Value);


            //    string midu = "";

            //    midudv.RowFilter = "factorName='蒸汽密度'";
            //    if (int.Parse(HiddenNum.Value) < midudv.Count)
            //    {
            //        midu = midudv[int.Parse(HiddenNum.Value)]["factorName"].ToString() + "时间：" + Convert.ToDateTime(midudv[int.Parse(HiddenNum.Value)]["DateTime"]).ToString("yyyy-MM-dd HH:mm:ss") + ";";
            //    }
            //    hdmiduData.Value = ToJson(midudv, "蒸汽密度", HiddenNum.Value);


            //    string shidu = "";

            //    shidudv.RowFilter = "factorName='相对湿度'";
            //    if (int.Parse(HiddenNum.Value) < shidudv.Count)
            //    {
            //        shidu = shidudv[int.Parse(HiddenNum.Value)]["factorName"].ToString() + "时间：" + Convert.ToDateTime(shidudv[int.Parse(HiddenNum.Value)]["DateTime"]).ToString("yyyy-MM-dd HH:mm:ss") + ";";
            //    }
            //    hdshiduData.Value = ToJson(shidudv, "相对湿度", HiddenNum.Value);


            //    string wenduhidden = "";
            //    foreach (DataColumn dc in dv.ToTable().Columns)
            //    {
            //        if (IsOrNotNumber(dc.ColumnName))
            //        {
            //            wenduhidden += dc.ColumnName + ";";
            //        }
            //    }
            //    hiddendiameter.Value = wenduhidden;
            //    lbTime.InnerText = wendu + midu + shidu;
            //}
            else if (tabStrip.SelectedTab.Text == "热力图")
            {
                ////监测因子
                //List<string> items = new List<string>();
                //foreach (RadComboBoxItem item in cbFactor.CheckedItems)
                //{
                //    items.Add(item.Value.ToString());
                //}
                ////温度
                //if (items.Contains("401"))
                //{
                //    string[] MonitoringFactors = { "401" };
                //    this.Hdv1.Style.Add("display", "block");
                //    BindChart1(MonitoringFactors);
                //}
                //else
                //{
                //    this.Hdv1.Style.Add("display", "none");
                //}
                ////蒸汽密度
                //if (items.Contains("402"))
                //{
                //    string[] MonitoringFactors = { "402" };
                //    this.Hdv2.Style.Add("display", "block");
                //    BindChart2(MonitoringFactors);
                //}   
                //else
                //{
                //    this.Hdv2.Style.Add("display", "none");
                //}
                ////相对湿度
                //if (items.Contains("404"))
                //{
                //    string[] MonitoringFactors = { "404" };
                //    this.Hdv3.Style.Add("display", "block");
                //    BindChart3(MonitoringFactors);
                //}
                //else
                //{
                //    this.Hdv3.Style.Add("display", "none");
                //}
                //string[] MonitoringFactors1 = { "401" };
                //BindChart1(MonitoringFactors1);
                //string[] MonitoringFactors2 = { "402" };
                //BindChart1(MonitoringFactors2);
                //string[] MonitoringFactors3 = { "404" };
                //BindChart1(MonitoringFactors3);
                BindChart();
            }
        }
        #region 热力图

        public void BindChart()
        {
            string[] portIds = { cbPoint.SelectedValue };
            string[] factorCode = { "401", "402", "404" };
            DateTime dtBegion = dayBegin.SelectedDate.Value;
            DateTime dtEnd = dayEnd.SelectedDate.Value;
            DataView dvL = new DataView();
            dvL = m_weiBoService.GetWeiboDataPager(portIds, factorCode, dtBegion, dtEnd);
            DataView wendudv = new DataView(dvL.ToTable());
            DataView midudv = new DataView(dvL.ToTable());
            DataView shidudv = new DataView(dvL.ToTable());
            wendudv.RowFilter = "factorName='温度'";
            DataTable dtL1 = new DataTable();
            dtL1.Columns.Add("xValue", typeof(string));
            dtL1.Columns.Add("yValue", typeof(string));
            dtL1.Columns.Add("zValue", typeof(string));
            foreach (DataRow dr in wendudv.ToTable().Rows)
            {
                foreach (DataColumn dc in wendudv.ToTable().Columns)
                {
                    if (IsNumeric(dc.ColumnName))
                    {
                        DataRow drNew = dtL1.NewRow();
                        drNew["xValue"] = dr["DateTime"];
                        drNew["yValue"] = dc.ColumnName;
                        drNew["zValue"] = dr[dc.ColumnName];
                        dtL1.Rows.Add(drNew);
                    }
                }
            }
            MicrowaveRadiationTemperature.Value = ToJson(dtL1);
            midudv.RowFilter = "factorName='蒸汽密度'";
            DataTable dtL2 = new DataTable();
            dtL2.Columns.Add("xValue", typeof(string));
            dtL2.Columns.Add("yValue", typeof(string));
            dtL2.Columns.Add("zValue", typeof(string));
            foreach (DataRow dr in midudv.ToTable().Rows)
            {
                foreach (DataColumn dc in midudv.ToTable().Columns)
                {
                    if (IsNumeric(dc.ColumnName))
                    {
                        DataRow drNew = dtL2.NewRow();
                        drNew["xValue"] = dr["DateTime"];
                        drNew["yValue"] = dc.ColumnName;
                        drNew["zValue"] = dr[dc.ColumnName];
                        dtL2.Rows.Add(drNew);
                    }
                }
            }
            MicrowaveRadiationVaporDensity.Value = ToJson(dtL2);
            shidudv.RowFilter = "factorName='相对湿度'";
            DataTable dtL3 = new DataTable();
            dtL3.Columns.Add("xValue", typeof(string));
            dtL3.Columns.Add("yValue", typeof(string));
            dtL3.Columns.Add("zValue", typeof(string));
            foreach (DataRow dr in shidudv.ToTable().Rows)
            {
                foreach (DataColumn dc in shidudv.ToTable().Columns)
                {
                    if (IsNumeric(dc.ColumnName))
                    {
                        DataRow drNew = dtL3.NewRow();
                        drNew["xValue"] = dr["DateTime"];
                        drNew["yValue"] = dc.ColumnName;
                        drNew["zValue"] = dr[dc.ColumnName];
                        dtL3.Rows.Add(drNew);
                    }
                }
            }
            MicrowaveRadiationRelativeHumidity.Value = ToJson(dtL3);
            //string wendu = "";

            //wendudv.RowFilter = "factorName='温度'";
            //if (int.Parse(HiddenNum.Value) < wendudv.Count)
            //{
            //    wendu = wendudv[int.Parse(HiddenNum.Value)]["factorName"].ToString() + "时间：" + Convert.ToDateTime(wendudv[int.Parse(HiddenNum.Value)]["DateTime"]).ToString("yyyy-MM-dd HH:mm:ss") + ";";
            //}
            //MicrowaveRadiationTemperature.Value = ToJson(wendudv, "温度", HiddenNum.Value);


            //string midu = "";

            //midudv.RowFilter = "factorName='蒸汽密度'";
            //if (int.Parse(HiddenNum.Value) < midudv.Count)
            //{
            //    midu = midudv[int.Parse(HiddenNum.Value)]["factorName"].ToString() + "时间：" + Convert.ToDateTime(midudv[int.Parse(HiddenNum.Value)]["DateTime"]).ToString("yyyy-MM-dd HH:mm:ss") + ";";
            //}
            //MicrowaveRadiationVaporDensity.Value = ToJson(midudv, "蒸汽密度", HiddenNum.Value);


            //string shidu = "";

            //shidudv.RowFilter = "factorName='相对湿度'";
            //if (int.Parse(HiddenNum.Value) < shidudv.Count)
            //{
            //    shidu = shidudv[int.Parse(HiddenNum.Value)]["factorName"].ToString() + "时间：" + Convert.ToDateTime(shidudv[int.Parse(HiddenNum.Value)]["DateTime"]).ToString("yyyy-MM-dd HH:mm:ss") + ";";
            //}
            //MicrowaveRadiationRelativeHumidity.Value = ToJson(shidudv, "相对湿度", HiddenNum.Value);


            //string wenduhidden = "";
            //foreach (DataColumn dc in dvL.ToTable().Columns)
            //{
            //    if (IsOrNotNumber(dc.ColumnName))
            //    {
            //        wenduhidden += dc.ColumnName + ";";
            //    }
            //}
            //hiddendiameter.Value = wenduhidden;
        }
        

        public static bool IsNumeric(string text)
        {
            return Regex.IsMatch(text, @"^[+-]?\d*[.]?\d*$");
        }

        /// <summary>
        /// DataTable
        /// </summary>
        /// <param name="dt">DataTable对象</param>
        /// <returns>Json字符串</returns>
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
        #endregion
        #endregion

        
        
        /// <summary>
        /// DataView转换为Json 
        /// </summary>
        /// <param name="dv">DataView对象</param>
        /// <param name="str">监测因子</param>
        /// <param name="value">值</param>
        /// <returns>Json字符串</returns>
        public static string ToJson(DataView dv, string str, string value)
        {
            //if (str == "温度")
            //    dv.RowFilter = "factorName='温度' and DateTime='" + Convert.ToDateTime(dv[int.Parse(value)]["DateTime"]) + "'";
            //if (str == "蒸汽密度")
            //    dv.RowFilter = "factorName='蒸汽密度' and DateTime='" + Convert.ToDateTime(dv[int.Parse(value)]["DateTime"]) + "'";
            //if (str == "相对湿度")
            //    dv.RowFilter = "factorName='相对湿度' and DateTime='" + Convert.ToDateTime(dv[int.Parse(value)]["DateTime"]) + "'";
            DataTable dt = new DataTable();
            if (dv.Count > 0)
            {
                if (str == "温度")
                    dv.RowFilter = "factorName='温度' and DateTime='" + Convert.ToDateTime(dv[int.Parse(value)]["DateTime"]) + "'";
                if (str == "蒸汽密度")
                    dv.RowFilter = "factorName='蒸汽密度' and DateTime='" + Convert.ToDateTime(dv[int.Parse(value)]["DateTime"]) + "'";
                if (str == "相对湿度")
                    dv.RowFilter = "factorName='相对湿度' and DateTime='" + Convert.ToDateTime(dv[int.Parse(value)]["DateTime"]) + "'";
                dt = dv.ToTable();
            }

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

        /// <summary>
        /// 页面隐藏域控件赋值，将数据需要的参数放入隐藏域，各个参数间用‘|’分割，每个参数内部用‘；’分割
        /// </summary>
        /// <param name="portIds"></param>
        /// <param name="factors"></param>
        /// <param name="dtBegin"></param>
        /// <param name="dtEnd"></param>
        private void SetHiddenData(string[] portIds, DateTime dtBegin, DateTime dtEnd)
        {
            if (portIds != null)
            {
                HiddenData.Value = string.Join(";", portIds) + "|" + ""
                                 + "|" + dtBegin + "|" + dtEnd;
            }
        }
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            HiddenNum.Value = "0";
            hdwenduData.Value = "";
            hdmiduData.Value = "";
            hdshiduData.Value = "";
            gridWeibo.CurrentPageIndex = 0;
            gridWeibo.Rebind();
            if (tabStrip.SelectedTab.Text == "图表")
            {
                FirstLoadChart.Value = "1";
                RegisterScript("GetChart();");
            }
            else if (tabStrip.SelectedTab.Text == "热力图")
            {

                SecondLoadChart.Value = "1";
                RegisterScript("chart();");
                //FirstLoadChart.Value = "1";
                //RegisterScript("chart();");
                //监测因子
                //List<string> items = new List<string>();
                //foreach (RadComboBoxItem item in cbFactor.CheckedItems)
                //{
                //    items.Add(item.Value.ToString());
                //}
                ////温度
                //if (items.Contains("401"))
                //{
                //    FirstLoadChart.Value = "1";
                //    RegisterScript("chart1();");
                //}
                ////蒸汽密度
                //if (items.Contains("402"))
                //{
                //    FirstLoadChart.Value = "1";
                //    RegisterScript("chart2();");
                //}
                ////相对湿度
                //if (items.Contains("404"))
                //{
                //    FirstLoadChart.Value = "1";
                //    RegisterScript("chart3();");
                //}
            }
            else
            {
                FirstLoadChart.Value = "1";
                SecondLoadChart.Value = "1";
            }
        }

        protected void gridRTB_ButtonClick(object sender, RadToolBarEventArgs e)
        {
            try
            {
                Telerik.Web.UI.RadToolBarButton button = (Telerik.Web.UI.RadToolBarButton)e.Item;
                if (button.CommandName == "ExportToExcel")
                {
                    string[] portIds = { cbPoint.SelectedValue };

                    string factor = "";
                    string factorId = "";
                    foreach (RadComboBoxItem item in cbFactor.CheckedItems)
                    {
                        factor += (item.Value.ToString() + ",");
                        factorId += ("9" + item.Value.ToString() + ",");
                    }
                    factor = factor.Trim(',');
                    factorId = factor.Trim(',');
                    string[] factors = factor.Split(',');
                    string[] factorIds = factorId.Split(',');
                    //每页显示数据个数            
                    int pageSize = gridWeibo.PageSize;
                    //当前页的序号
                    int pageNo = gridWeibo.CurrentPageIndex;

                    DataTable auditData = new DataTable();

                    DateTime dtBegion = dayBegin.SelectedDate.Value;
                    DateTime dtEnd = dayEnd.SelectedDate.Value;
                    auditData = m_weiBoService.GetWeiboDataPager(portIds, factors, dtBegion, dtEnd).ToTable(); //月类型 按日数据查询
                    auditData.Columns["factorName"].ColumnName = "因子";
                    auditData.Columns["portName"].ColumnName = "测点";
                    auditData.Columns["DateTime"].ColumnName = "日期";
                    ExcelHelper.DataTableToExcel(auditData, "微波辐射", "微波辐射", this.Page);
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void gridWeibo_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }

        protected void gridWeibo_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                DataRowView drv = e.Item.DataItem as DataRowView;
            }
        }

        protected void gridWeibo_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {
            try
            {
                if (e.Column.ColumnType.Equals("GridExpandColumn"))
                    return;
                //追加测点
                GridBoundColumn col = (GridBoundColumn)e.Column;

                if (col.DataField == "portName")
                {
                    col.HeaderText = "测点";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "factorName")
                {
                    col.HeaderText = "因子";
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
                else if (col.DataField == "DateTime")
                {
                    col = (GridDateTimeColumn)e.Column;
                    //string tstcolformat = "{0:yyyy-MM-dd}";
                    string tstcolformat = "{0:MM-dd HH:mm:ss}";
                    col.HeaderText = "日期";
                    col.DataFormatString = tstcolformat;
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(120);
                    col.ItemStyle.Width = Unit.Pixel(120);
                }
                else
                {
                    col.HeaderText = col.DataField;
                    col.EmptyDataText = "--";
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.Width = Unit.Pixel(110);
                    col.ItemStyle.Width = Unit.Pixel(110);
                }
            }
            catch (Exception ex) { }
        }

        public bool IsOrNotNumber(string a)
        {
            decimal d = 0;
            if (decimal.TryParse(a, out d) == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        protected void preSearch_Click(object sender, EventArgs e)
        {
            hdwenduData.Value = "";
            hdmiduData.Value = "";
            hdshiduData.Value = "";
            if (int.Parse(HiddenNum.Value) - 1 >= 0)
            {
                HiddenNum.Value = (int.Parse(HiddenNum.Value) - 1).ToString();
            }
            else
            {
                HiddenNum.Value = "0";
            }
            gridWeibo.CurrentPageIndex = 0;
            gridWeibo.Rebind();
            if (tabStrip.SelectedTab.Text == "图表")
            {
                RegisterScript("GetChart();");
            }
            else
            {
                FirstLoadChart.Value = "1";
            }
        }

        protected void nextSearch_Click(object sender, EventArgs e)
        {
            hdwenduData.Value = "";
            hdmiduData.Value = "";
            hdshiduData.Value = "";
            HiddenNum.Value = (int.Parse(HiddenNum.Value) + 1).ToString();
            gridWeibo.CurrentPageIndex = 0;
            gridWeibo.Rebind();
            if (tabStrip.SelectedTab.Text == "图表")
            {
                RegisterScript("GetChart();");
            }
            else
            {
                FirstLoadChart.Value = "1";
            }
        }

    }
}