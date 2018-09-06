namespace SmartEP.Service.ReportLibrary.Water
{
    using System;
    using System.Web;
    using System.ComponentModel;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using System.Collections.Generic;
    using System.Linq;
    using SmartEP.Core.Interfaces;
    using SmartEP.Service.DataAnalyze.Water.DataQuery;
    using SmartEP.DomainModel.BaseData;
    using SmartEP.Service.BaseData.MPInfo;
    using SmartEP.Core.Generic;

    /// <summary>
    /// Summary description for TaihuWQMonitoringReport.
    /// </summary>
    public partial class TaihuAutoMonitoringReport : Telerik.Reporting.Report
    {
        MonitoringPointWaterService g_MonitoringPointWater = Singleton<MonitoringPointWaterService>.GetInstance();
        //public static string[] portIds = null;
        //public static IList<IPollutant> factors;
        //public static DateTime dateStart = DateTime.Now;
        //public static DateTime dateEnd = DateTime.Now;

        double pointSize = 9;//表格字体大小
        double tableWidth = 23.7;//表格宽度
        double tdcolumnHight = 0.5;

        public TaihuAutoMonitoringReport()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 获取数据源
        /// </summary>
        /// <param name="pointID"></param>
        /// <param name="factors"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        public void BindingPortReport(string[] pointID, IList<IPollutant> factors, DateTime beginTime, DateTime endTime)
        {
            DataQueryByHourService hourService = new DataQueryByHourService();
            DataView dtData = hourService.GetHourExportDataReport(pointID, factors, beginTime, endTime);
            DataView dtStatis = hourService.GetHourStatisticalData(pointID, factors.Select(x => x.PollutantCode).ToArray(), beginTime, endTime);
            string[] CommonField = ("PortName;Month;Day;Hour" + (factors.Count > 0 ? ";" + string.Join(";", factors.Select(x => x.PollutantCode).ToArray()) : "")).Split(';');
            double sourceY = 0;
            GetHtmlTextBox(0, sourceY, tableWidth, 0.5, 12, true, "center", "太湖湖体报表");
            sourceY += 0.5;
            for (int i = 0; i < pointID.Length; i++)
            {
                #region 标题
                string pointName = g_MonitoringPointWater.RetrieveEntityByPointId(Convert.ToInt32(pointID[i])).MonitoringPointName;
                GetHtmlTextBox(0, sourceY, tableWidth, 0.5, 10, false, "left", string.Format(@"测点名称：{0}", pointName));
                GetHtmlTextBox(0, sourceY, tableWidth, 0.5, 10, false, "right", string.Format(@"日期：{0}", beginTime.ToString("yyyy-MM-dd") + "-" + endTime.ToString("yyyy-MM-dd")));
                sourceY += 0.6;
                #endregion
                dtData.RowFilter = "";
                dtData.RowFilter = "PointId=" + pointID[i];
                dtStatis.RowFilter = "";
                dtStatis.RowFilter = "PointId=" + pointID[i];
                CreateTableReport(this.detail, dtData.ToTable(), DealStatisData(pointID[i], factors, dtStatis), CommonField, factors, sourceY);
                sourceY += tdcolumnHight * dtData.Count + dtStatis.Count + 1;
            }
        }

        private DataTable DealStatisData(string pointid, IList<IPollutant> factors, DataView dtStatis)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PortName");
            dt.Columns.Add("Month");
            dt.Columns.Add("Day");
            dt.Columns.Add("Hour");
            DataRow rowAvg = dt.NewRow();//平均值
            DataRow rowMax = dt.NewRow();//最大值
            DataRow rowMin = dt.NewRow();//最小值
            foreach (IPollutant poll in factors)
            {
                dt.Columns.Add(poll.PollutantCode);
                dtStatis.RowFilter = "PollutantCode='" + poll.PollutantCode + "' AND " + "PointId=" + pointid;
                if (dtStatis.Count > 0)
                {
                    rowAvg[poll.PollutantCode] = dtStatis[0]["Value_Avg"];
                    rowMax[poll.PollutantCode] = dtStatis[0]["Value_Max"];
                    rowMin[poll.PollutantCode] = dtStatis[0]["Value_Min"];
                }
            }
            rowAvg["PortName"] = "平均值";
            rowMax["PortName"] = "最大值";
            rowMin["PortName"] = "最小值";
            dt.Rows.Add(rowAvg);
            dt.Rows.Add(rowMax);
            dt.Rows.Add(rowMin);
            return dt;
        }

        #region 生成报表
        /// <summary>
        /// 创建表格
        /// </summary>
        /// <param name="detail"></param>
        /// <param name="dt"></param>
        /// <param name="dtRow"></param>
        /// <param name="columnField"></param>
        /// <param name="columnName"></param>
        /// <param name="factors"></param>
        /// <param name="souceY"></param>
        /// <returns></returns>
        public Telerik.Reporting.Table CreateTableReport(DetailSection detail, DataTable dt, DataTable dtRow, string[] columnField, IList<IPollutant> factors, double souceY)
        {
            double tbHight = dt.Rows.Count;//表格高度

            //表格设置
            Telerik.Reporting.TableGroup parentGroup = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.Table tb = new Table();
            Telerik.Reporting.HtmlTextBox textboxGroup;
            Telerik.Reporting.HtmlTextBox textBoxTable;
            tb.Size = new SizeU(Unit.Cm(tableWidth), Unit.Cm(tbHight * tdcolumnHight));
            detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] { tb });
            tb.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(souceY));
            tb.DataSource = dt;
            tb.Style.Font.Name = "宋体";
            tb.Style.Font.Size = Unit.Point(pointSize);
            tb.ColumnGroups.Clear();
            tb.Body.Columns.Clear();
            tb.Body.Rows.Clear();

            //添加行
            tb.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(tdcolumnHight)));
            TableGroup tableGroupRow = new Telerik.Reporting.TableGroup();
            tableGroupRow.Groupings.Add(new Telerik.Reporting.Grouping(null));
            tb.RowGroups.Add(tableGroupRow);

            //统计行条数
            for (int i = 0; i < dtRow.Rows.Count; i++)
            {
                tb.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(tdcolumnHight)));
                TableGroup Row = new Telerik.Reporting.TableGroup();
                tb.RowGroups.Add(Row);
            }

            int ColCount = columnField.Length;
            //int ColCount = columnName != null ? dt.Columns.Count - columnName.Length : dt.Columns.Count;

            /*判断是否包含因子--*/
            int num = 0;
            int numAQI = 0;
            double width = (ColCount - num - 1) != 0 ? (tableWidth * 10 - 28 - 21 * num - 10 * numAQI) / (ColCount - num - 1 - numAQI) : 0;
            if (width == 0) width = 20;
            if (columnField == null) width = (tableWidth * 10 - 45) / 7;
            for (int i = 0; i <= ColCount - 1; i++)
            {
                #region 设置宽度
                SizeU colSize = new SizeU
                {
                    Height = Unit.Mm(tdcolumnHight * 10),
                    Width = Unit.Mm(width)
                };
                //表字段名称
                string name = columnField[i];
                //if (name.Contains("日期")) colSize.Width = Unit.Mm(28);
                if (name.Contains("PortName")) name = "站点名称";
                else if (name.Contains("Month")) name = "月";
                else if (name.Contains("Day")) name = "日期";
                else if (name.Contains("Hour")) name = "时间";
                else
                {
                    IPollutant pollutant = factors.Where(x => x.PollutantCode.ToString().Equals(columnField[i])).FirstOrDefault();
                    if (pollutant != null)
                    {
                        name = pollutant.PollutantName + "<br/>" + pollutant.PollutantMeasureUnit;
                    }
                }
                #endregion

                //添加列并对列设置样式
                Telerik.Reporting.TableGroup tableGroupColumn = new Telerik.Reporting.TableGroup();
                //tb.ColumnGroups.Add(tableGroupColumn);
                tb.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn
                {

                });
                textboxGroup = new Telerik.Reporting.HtmlTextBox();
                textboxGroup.Style.BorderColor.Default = Color.Black;
                textboxGroup.Style.BorderStyle.Default = BorderType.Solid;
                textboxGroup.Style.BorderWidth.Default = Unit.Pixel(0.5);
                textboxGroup.Style.Font.Name = "宋体";

                ////监测点日报审核日志表头字体以及高度设置
                //if (columnName != null)
                //{
                textboxGroup.Style.Font.Size = Unit.Point(pointSize);
                textboxGroup.Size = colSize;
                //}
                //else
                //{
                //    textboxGroup.Style.Font.Size = Unit.Point(10);
                //    textboxGroup.Size = new SizeU
                //    {
                //        Height = Unit.Mm(8),
                //        Width = Unit.Mm(width)
                //    };
                //}
                textboxGroup.Style.TextAlign = HorizontalAlign.Center;
                textboxGroup.Style.VerticalAlign = VerticalAlign.Middle;

                //if (i > 1)
                //{
                //    if (factors.Where(x => x.PollutantCode.ToString().Equals(columnField[i])).Count() <= 0)
                //    {
                //        textboxGroup.Value = name;//列名         
                //    }
                //    else
                //    {
                //        string unit = factors.Where(x => x.PollutantCode.ToString().Equals(columnField[i])).Select(x => x.PollutantMeasureUnit).FirstOrDefault();
                //        if (unit.Equals("mg/L"))
                //            unit = "(mg/m<sup>3</sup>)";
                //        else if (unit.Equals("度"))
                //            unit = "(<sup>。</sup>)";
                //        else
                //            unit = "(" + unit + ")";
                //        textboxGroup.Value = unit;//列名 
                //    }
                //}
                //else
                //{
                textboxGroup.Value = name;//列名 
                //}

                tableGroupColumn.ReportItem = textboxGroup;
                tableGroupColumn.Name = i.ToString();//不加只显示第一列数据


                Telerik.Reporting.HtmlTextBox textBox = new Telerik.Reporting.HtmlTextBox();
                textBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3D), Telerik.Reporting.Drawing.Unit.Cm(0.8D));
                textBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                textBox.Style.BorderWidth.Default = Unit.Pixel(0.5);
                textboxGroup.Style.Font.Size = Unit.Point(pointSize);
                textBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
                textBox.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;

                ////多表头
                //if (factors.Where(x => x.PollutantCode.ToString().Equals(columnField[i])).Count() > 0)
                //{
                //    parentGroup = new Telerik.Reporting.TableGroup();
                //    textBox.Value = name;
                //    parentGroup.ChildGroups.Add(tableGroupColumn);
                //    parentGroup.ReportItem = textBox;
                //    tb.ColumnGroups.Add(parentGroup);
                //}
                //else
                //{
                tb.ColumnGroups.Add(tableGroupColumn);
                //}

                textBoxTable = new Telerik.Reporting.HtmlTextBox();
                textBoxTable.Style.BorderColor.Default = Color.Black;
                textBoxTable.Style.BorderStyle.Default = BorderType.Solid;
                textBoxTable.Style.BorderWidth.Default = Unit.Pixel(0.5);
                textBoxTable.Style.TextAlign = HorizontalAlign.Center;
                textBoxTable.Style.VerticalAlign = VerticalAlign.Middle;
                textBoxTable.Style.Font.Size = Unit.Point(pointSize);
                textBoxTable.Size = colSize;
                textBoxTable.Value = "=IsNull(Fields." + columnField[i] + ", '--')";

                tb.Body.SetCellContent(0, i, textBoxTable);//添加列数据
                tb.Items.AddRange(new ReportItemBase[] { textBoxTable, textboxGroup });//表格添加列名及列数据 
                if (dtRow.Rows.Count > 0)
                {
                    for (int k = 0; k < dtRow.Rows.Count; k++)
                    {
                        textBoxTable = new Telerik.Reporting.HtmlTextBox();
                        textBoxTable.Style.BorderColor.Default = Color.Black;
                        textBoxTable.Style.BorderStyle.Default = BorderType.Solid;
                        textBoxTable.Style.BorderWidth.Default = Unit.Pixel(0.5);
                        textBoxTable.Style.TextAlign = HorizontalAlign.Center;
                        textBoxTable.Style.VerticalAlign = VerticalAlign.Middle;
                        textBoxTable.Style.Font.Size = Unit.Point(pointSize);
                        if (dtRow.Rows[k][i] == DBNull.Value)
                            textBoxTable.Value = "--";
                        else
                            textBoxTable.Value = dtRow.Rows[k][i].ToString();
                        textBoxTable.Size = colSize;
                        tb.Body.SetCellContent(k + 1, i, textBoxTable);
                        tb.Items.AddRange(new ReportItemBase[] { textBoxTable });//表格添加列名及列数据 
                    }
                }
            }

            //没有数据添加----暂无数据行   
            if (dt.Rows.Count <= 0 && dtRow.Rows.Count <= 0)
            {
                tb.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(tdcolumnHight)));
                TableGroup Row = new Telerik.Reporting.TableGroup();
                tb.RowGroups.Add(Row);

                SizeU colSize = new SizeU
                {
                    Height = Unit.Mm(tdcolumnHight * 10),
                    Width = Unit.Cm(20)
                };
                textBoxTable = new Telerik.Reporting.HtmlTextBox();
                textBoxTable.Style.BorderColor.Default = Color.Black;
                textBoxTable.Style.BorderStyle.Default = BorderType.Solid;
                textBoxTable.Style.BorderWidth.Default = Unit.Pixel(0.5);
                textBoxTable.Style.TextAlign = HorizontalAlign.Center;
                textBoxTable.Style.VerticalAlign = VerticalAlign.Middle;
                textBoxTable.Value = " 没有数据";
                textBoxTable.Size = colSize;
                tb.Body.SetCellContent(1, 0, textBoxTable, 1, ColCount);
                tb.Items.AddRange(new ReportItemBase[] { textBoxTable });//表格添加列名及列数据 
            }

            return tb;
        }

        /// <summary>
        /// 创建HtmlTextBox控件
        /// </summary>
        /// <param name="x"></param>X轴位置
        /// <param name="y"></param>Y轴位置
        /// <param name="width"></param>控件宽度
        /// <param name="height"></param>控件高度
        /// <param name="font_Size"></param>字体大小
        /// <param name="isBold"></param>是否加粗
        /// <param name="center"></param>是否居中
        /// <param name="value"></param>绑定值
        private void GetHtmlTextBox(double x, double y, double width, double height, double font_Size, bool isBold, string location, string value)
        {
            Telerik.Reporting.HtmlTextBox htmlTextBox = new HtmlTextBox();
            htmlTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(x), Telerik.Reporting.Drawing.Unit.Cm(y));
            //htmlTextBox.Name = "htmlTextBox1";
            htmlTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(width), Telerik.Reporting.Drawing.Unit.Cm(height));
            htmlTextBox.Style.Font.Bold = isBold;
            htmlTextBox.Style.Font.Name = "宋体";
            htmlTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(font_Size);
            if (location.Equals("center"))
                htmlTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            else if (location.Equals("left"))
                htmlTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            else if (location.Equals("right"))
                htmlTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            htmlTextBox.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            htmlTextBox.Value = value;
            detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] { htmlTextBox });
            //return htmlTextBox;
        }
        #endregion
    }
}