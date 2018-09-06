﻿namespace SmartEP.Service.ReportLibrary.Air
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using System.Data;
    using SmartEP.Service.DataAnalyze.Air.DataQuery;
    using SmartEP.Core.Interfaces;
    using System.Collections.Generic;
    using SmartEP.Service.BaseData.MPInfo;
    using System.Linq;
    using SmartEP.Core.Generic;
    using SmartEP.DomainModel.BaseData;

    /// <summary>
    /// Summary description for AirQualityAutoMonthReport.
    /// </summary>
    public partial class AirQualityAutoMonthReportService : Telerik.Reporting.Report
    {
        DataQueryByHourService dataByHourService = new DataQueryByHourService();
        MonitoringPointAirService g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
        double pointSize = 9;//表格字体大小
        double tableWidth = 23.7;//表格宽度
        //double tableHight = 18.5 - 0.5 - 1 - 1;//表格单元格高度
        double tdcolumnHight = 0.5;

        public AirQualityAutoMonthReportService()
        {
            InitializeComponent();
     
        }

        public void BindingPortReport(string[] pointID, string[] factorCode, string[] factorName, IList<IPollutant> factors, DateTime beginTime, DateTime endTime)
        {
            DataTable dtData = dataByHourService.GetAQAutoMonthReportExportData(pointID, factors, beginTime, endTime).ToTable();
            DataView dv = dtData.DefaultView;
            string[] CommonField = ("日期;时间" + (factorCode.Length > 0 ? ";" + string.Join(";", factorCode) : "")).Split(';');
            string[] CommonName = ("日期;时间" + (factorName.Length > 0 ? ";" + string.Join(";", factorName) : "")).Split(';');
            double sourceY = 0;
            //GetHeaderHtmlTextBox(0, sourceY, tableWidth, 0.5, 10, false, "center", "环境空气质量自动监测数据报表");//创建页眉
            GetHtmlTextBox(0, sourceY, tableWidth, 0.5, 12, true, "center", "环境空气质量自动监测数据报表");
            for (int i = 0; i < pointID.Length; i++)
            {
                dv.RowFilter = "";
                dv.RowFilter = "PointId=" + pointID[i];
                MonitoringPointEntity pointEntity = g_MonitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(pointID[i]));
                MonitoringPointExtensionForEQMSAirEntity extensionEntity = g_MonitoringPointAir.RetrieveAirExtensionPointListByPointUids(pointEntity.MonitoringPointUid.Split(';')).FirstOrDefault();
                //string pointName = g_MonitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(pointID[i])).MonitoringPointName;

                sourceY += 0.5;
                GetHtmlTextBox(0, sourceY, tableWidth, 0.5, 10, false, "left", " 测点名称 " + pointEntity.MonitoringPointName + "       测点代码 " + (extensionEntity.Stcode!=null?extensionEntity.Stcode:""));
                GetHtmlTextBox(0, sourceY, tableWidth, 0.5, 10, false, "right", "WXHJ-JL-ZB-24");
                sourceY += 0.6;
                CreateTableAutoMonthReport(this.detail, dv.ToTable(), new DataTable(), CommonField, CommonName, factors, sourceY);
                sourceY += tdcolumnHight * dv.Count + 1;
            }
        }

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
        public Telerik.Reporting.Table CreateTableAutoMonthReport(DetailSection detail, DataTable dt, DataTable dtRow, string[] columnField, string[] columnName, IList<IPollutant> factors, double souceY)
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

            int ColCount = columnName.Length;
            //int ColCount = columnName != null ? dt.Columns.Count - columnName.Length : dt.Columns.Count;

            /*判断是否包含因子--*/
            int num = 0;
            int numAQI = 0;
            double width = (ColCount - num - 1) != 0 ? (tableWidth * 10 - 28 - 21 * num - 10 * numAQI) / (ColCount - num - 1 - numAQI) : 0;
            if (width == 0) width = 20;
            if (columnName == null) width = (tableWidth * 10 - 45) / 7;
            for (int i = 0; i <= ColCount - 1; i++)
            {
                #region 设置宽度
                SizeU colSize = new SizeU
                {
                    Height = Unit.Mm(tdcolumnHight * 10),
                    Width = Unit.Mm(width)
                };
                //表字段名称
                string name = columnName[i];
                if (name.Contains("日期")) colSize.Width = Unit.Mm(28);

                if (name.Equals("PM2.5"))
                    name = "<span style='font-size: 10pt; font-family: 宋体'><p>PM<span style='font-size: 7pt'>2.5</span></p></span>";
                else if (name.Equals("SO2"))
                    name = "<span style='font-size: 10pt; font-family: 宋体'><p>SO<span style='font-size: 7pt'>2</span></p></span>";
                else if (name.Equals("NO2"))
                    name = "<span style='font-size: 10pt; font-family: 宋体'><p>NO<span style='font-size: 7pt'>2</span></p></span>";
                else if (name.Equals("NOX"))
                    name = "<span style='font-size: 10pt; font-family: 宋体'><p>NO<span style='font-size: 7pt'>x</span></p></span>";
                else if (name.Equals("O3"))
                    name = "<span style='font-size: 10pt; font-family: 宋体'><p>O<span style='font-size: 7pt'>3</span></p></span>";
                else if (name.Equals("PM10"))
                    name = "<span style='font-size: 10pt; font-family: 宋体'><p>PM<span style='font-size: 7pt'>10</span></p></span>";

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

                //监测点日报审核日志表头字体以及高度设置
                if (columnName != null)
                {
                    textboxGroup.Style.Font.Size = Unit.Point(pointSize);
                    textboxGroup.Size = colSize;
                }
                else
                {
                    textboxGroup.Style.Font.Size = Unit.Point(10);
                    textboxGroup.Size = new SizeU
                    {
                        Height = Unit.Mm(8),
                        Width = Unit.Mm(width)
                    };
                }
                textboxGroup.Style.TextAlign = HorizontalAlign.Center;
                textboxGroup.Style.VerticalAlign = VerticalAlign.Middle;

                if (i > 1)
                {
                    if (factors.Where(x => x.PollutantCode.ToString().Equals(columnField[i])).Count() <= 0)
                    {
                        textboxGroup.Value = name;//列名         
                    }
                    else
                    {
                        string unit = factors.Where(x => x.PollutantCode.ToString().Equals(columnField[i])).Select(x => x.PollutantMeasureUnit).FirstOrDefault();
                        if (unit.Equals("mg/L"))
                            unit = "(mg/m<sup>3</sup>)";
                        else if (unit.Equals("度"))
                            unit = "(<sup>。</sup>)";
                        else
                            unit = "(" + unit + ")";
                        textboxGroup.Value = unit;//列名 
                    }
                }
                else
                {
                    textboxGroup.Value = name;//列名 
                }

                tableGroupColumn.ReportItem = textboxGroup;
                tableGroupColumn.Name = i.ToString();//不加只显示第一列数据


                Telerik.Reporting.HtmlTextBox textBox = new Telerik.Reporting.HtmlTextBox();
                textBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3D), Telerik.Reporting.Drawing.Unit.Cm(0.8D));
                textBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                textBox.Style.BorderWidth.Default = Unit.Pixel(0.5);
                textboxGroup.Style.Font.Size = Unit.Point(pointSize);
                textBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
                textBox.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;

                //多表头
                if (factors.Where(x => x.PollutantCode.ToString().Equals(columnField[i])).Count() > 0)
                {
                    parentGroup = new Telerik.Reporting.TableGroup();
                    textBox.Value = name;
                    parentGroup.ChildGroups.Add(tableGroupColumn);
                    parentGroup.ReportItem = textBox;
                    tb.ColumnGroups.Add(parentGroup);
                }
                else
                {
                    tb.ColumnGroups.Add(tableGroupColumn);
                }

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
     
    }
}