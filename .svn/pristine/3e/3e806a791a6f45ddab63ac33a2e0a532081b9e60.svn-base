using SmartEP.Utilities.Office;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SmartEP.WebUI.Pages.EnvAir.QualityControl
{
    /// <summary>
    /// 名称：StandardTransfer.cs
    /// 创建人：王秉晟
    /// 创建日期：2015-10-05
    /// 维护人员：
    /// 最新维护人员：
    /// 最新维护日期：
    /// 功能摘要：流量标准传递
    /// 版权所有(C)：江苏远大信息股份有限公司
    /// </summary>
    public partial class StandardTransfer : SmartEP.WebUI.Common.BasePage
    {
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
            ddlTransferFashion.SelectedValue = "0";//传递方式
            lblCheckTime.Text = "2015-09-30";//检测时间
            lblCheckPlace.Text = "南门";//检测地点
            lblRoomTemperature.Text = "25";//站房温度
            lblHumidityStation.Text = "18";//站房湿度
            lblAmbientPressure.Text = "350";//环境气压
            lblCorrectedValue.Text = "2.6";//温度对压力计读数的修正值
            lblSaturatedVaporPressure.Text = "230";//饱和蒸气压
            lblSlope.Text = "1.09";//斜率
            lblIntercept.Text = "3.71";//截距
            lblCorrelationCoefficient.Text = "1.1";//相关系数
            lblIsQualified.Text = "是";//是否合格
        }
        #endregion

        #region 绑定Grid
        /// <summary>
        /// 绑定流量计标准的Grid数据
        /// </summary>
        public void BindGridFlowMeterStandard()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("data1");
            dt.Columns.Add("data2");
            dt.Columns.Add("data3");
            dt.Columns.Add("data4");
            dt.Columns.Add("data5");
            dt.Columns.Add("data6");
            dt.Columns.Add("data7");
            DataRow dr1 = dt.NewRow();
            dr1["data1"] = "一级标准";//流量计标准
            dr1["data2"] = "皂膜";//类型
            dr1["data3"] = "N0031";//编号
            dr1["data4"] = "2015-09-28 13:33";//上次标定时间
            dr1["data5"] = "1.23";//斜率
            dr1["data6"] = "3.35";//截距
            dr1["data7"] = "1.05";//相关系数
            dt.Rows.Add(dr1);
            DataRow dr2 = dt.NewRow();
            dr2["data1"] = "工作标准";
            dr2["data2"] = "质量";
            dr2["data3"] = "N0105";
            dr2["data4"] = "2015-09-30 10:47";
            dr2["data5"] = "1.06";
            dr2["data6"] = "3.21";
            dr2["data7"] = "0.97";
            dt.Rows.Add(dr2);

            gridFlowMeterStandard.DataSource = dt;
            gridFlowMeterStandard.VirtualItemCount = dt.Rows.Count;
        }

        /// <summary>
        /// 绑定质量流量计读数的Grid数据
        /// </summary>
        public void BindGridMassFlowMeterReading()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("data1");
            dt.Columns.Add("data2");
            dt.Columns.Add("data3");
            dt.Columns.Add("data4");
            for (int i = 0; i < 10; i++)
            {
                DataRow dr = dt.NewRow();
                dr["data1"] = (i > 5) ? (i + 10) * 10 / (i - 5) : (i + 20) * 10 / (i + 10);//质量流量计读数
                dr["data2"] = (i > 5) ? (i + 1) * 10 / (i - 2) : (i + 2) * 10 / (i + 1);//平均时间
                dr["data3"] = (i + 3) * 10 / (i + 2);//气体体积
                dr["data4"] = (i + 1) * 100 / (i + 3);//一级标准的质量流量Qs
                dt.Rows.Add(dr);
            }

            gridMassFlowMeterReading.DataSource = dt;
            gridMassFlowMeterReading.VirtualItemCount = dt.Rows.Count;
        }

        /// <summary>
        /// 绑定量程百分比的Grid数据
        /// </summary>
        public void BindGridMeasuringRange()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("data1");
            dt.Columns.Add("data2");
            dt.Columns.Add("data3");
            dt.Columns.Add("data4");
            for (int i = 0; i < 4; i++)
            {
                string percent = (20 * (i + 1)).ToString() + "%";
                DataRow dr = dt.NewRow();
                dr["data1"] = percent;//量程百分比
                dr["data2"] = (i > 5) ? (i + 11) * 10 / (i - 5) : (i + 20) * 10 / (i + 10);//质量流量计读数
                dr["data3"] = (i + 5) * 100 / (i + 6);//一级标准实测流量
                dr["data4"] = (i + 1) * 100 / (i + 4);//一级标准质量流量Qs
                dt.Rows.Add(dr);
            }

            gridMeasuringRange.DataSource = dt;
            gridMeasuringRange.VirtualItemCount = dt.Rows.Count;
            hdCalibrationCurveData.Value = ToJson(dt);
        }

        /// <summary>   
        /// Datatable转换为Json   
        /// </summary>   
        /// <param name="table">Datatable对象</param>   
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
                    strValue = StringFormat(strValue, type);
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
        private static string StringFormat(string str, Type type)
        {
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
        #endregion

        #region 服务器端控件事件处理
        /// <summary>
        /// 绑定数据源（用于分页、排序）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGrid grid = sender as RadGrid;
            switch (grid.ID)
            {
                case "gridFlowMeterStandard":
                    BindGridFlowMeterStandard();
                    break;
                case "gridMassFlowMeterReading":
                    BindGridMassFlowMeterReading();
                    break;
                case "gridMeasuringRange":
                    BindGridMeasuringRange();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 数据行绑定处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grid_ItemDataBound(object sender, GridItemEventArgs e)
        {
            //if (e.Item is GridDataItem)
            //{
            //    GridDataItem item = e.Item as GridDataItem;
            //    DataRowView drv = e.Item.DataItem as DataRowView;
            //    if (item["PointId"] != null)
            //    {
            //        GridTableCell pointCell = (GridTableCell)item["PointId"];
            //        string pointName = points.Where(x => x.PointID.Equals(drv["PointId"].ToString().Trim()))
            //                           .Select(t => t.PointName).FirstOrDefault();
            //        pointCell.Text = pointName;
            //    }
            //    if (drv.DataView.Table.Columns.Contains("RGBValue")) //if (item["RGBValue"] != null)
            //    {
            //        //GridTableCell cell = item["RGBValue"] as GridTableCell;
            //        //cell.Style.Add("background-color", cell.Text);
            //        //cell.Text = string.Empty;
            //        GridTableCell cell = item["Class"] as GridTableCell;
            //        cell.Style.Add("background-color", drv["RGBValue"].ToString().Trim());
            //    }
            //}
        }

        /// <summary>
        /// 数据列生成处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grid_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            //try
            //{
            //    GridBoundColumn col = e.Column as GridBoundColumn;
            //    if (col == null)
            //        return;
            //}
            //catch (Exception ex) { }
        }

        /// <summary>
        /// ToolBar事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridRTB_ButtonClick(object sender, RadToolBarEventArgs e)
        {
            //Telerik.Web.UI.RadToolBarButton button = (Telerik.Web.UI.RadToolBarButton)e.Item;
            //if (button.CommandName == "ExportToExcel")
            //{
            //    string[] portIds = pointCbxRsm.GetPointValues(CbxRsmReturnType.ID);
            //    DateTime dtmEnd = DateTime.Now;
            //    DateTime dtmBegion = dtmEnd.AddDays(-7);
            //    DataView dv = m_HourAQIService.GetPortExportData(portIds, dtmBegion, dtmEnd);
            //    DataTable dt = UpdateExportColumnName(dv);
            //    ExcelHelper.DataTableToExcel(dt, "空气质量实时报", "空气质量实时报", this.Page);
            //}
        }
        #endregion
    }
}