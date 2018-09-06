namespace SmartEP.Service.ReportLibrary.Air
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
    /// Summary description for AirQualityRoutineMonthReport.
    /// </summary>
    public partial class AirQualityRoutineMonthReportService : Telerik.Reporting.Report
    {
        DataQueryByDayService dataByDayService = new DataQueryByDayService();
        MonitoringPointAirService g_MonitoringPointAir = Singleton<MonitoringPointAirService>.GetInstance();
        double pointSize = 9;//表格字体大小
        double tableWidth = 36;//表格宽度
        //double tableHight = 18.5 - 0.5 - 1 - 1;//表格单元格高度
        double tdcolumnHight = 0.5;

        public AirQualityRoutineMonthReportService()
        {
            InitializeComponent();

        }

        public void BindingPortReport(string[] pointID, DataTable dtStatistical, string[] factorField, string[] factorName, IList<IPollutant> factors, DateTime beginTime, DateTime endTime)
        {
            DataTable dtData = dataByDayService.GetAQRoutineMonthReportExportData(pointID, factors, beginTime, endTime).ToTable();
            DataView dv = dtData.DefaultView;
            string[] CommonField = ("测点名称;测点代码;monthBegin;dayBegin;hourBegin;minuteBegin;monthEnd;dayEnd;hourEnd;minuteEnd" + (factorField.Length > 0 ? ";" + string.Join(";", factorField) : "")).Split(';');
            string[] CommonName = ("测点名称;测点代码;月;日;时;分;月;日;时;分" + (factorName.Length > 0 ? ";" + string.Join(";", factorName) : "")).Split(';');
            double sourceY = 0;
            GetHtmlTextBox(0, sourceY, tableWidth, 0.5, 12, true, "center", "环境空气质量例行监测成果表");
            for (int i = 0; i < pointID.Length; i++)
            {
                MonitoringPointEntity pointEntity = g_MonitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(pointID[i]));
                MonitoringPointExtensionForEQMSAirEntity extensionEntity = g_MonitoringPointAir.RetrieveAirExtensionPointListByPointUids(pointEntity.MonitoringPointUid.Split(';')).FirstOrDefault();
                //string pointName = g_MonitoringPointAir.RetrieveEntityByPointId(Convert.ToInt32(pointID[i])).MonitoringPointName;
                dv.RowFilter = "PointId=" + pointID[i];
                DataTable resultDT = dv.ToTable();
                resultDT.Columns.Add("测点名称").SetOrdinal(0);
                resultDT.Columns.Add("测点代码").SetOrdinal(1);
                if (resultDT.Rows.Count <= 0)
                {
                    DataRow row = resultDT.NewRow();
                    resultDT.Rows.Add(row);
                }
                resultDT.Rows[0][0] = pointEntity.MonitoringPointName;
                resultDT.Rows[0][1] = extensionEntity.Stcode!=null?extensionEntity.Stcode:"";

                DataView resultStatistical = dtStatistical.DefaultView;
                resultStatistical.RowFilter = "PointId=" + pointID[i];

                sourceY += 0.5;
                GetHtmlTextBox(0, sourceY, tableWidth, 0.5, 10, false, "left", "单位(公章)          测点代码 320200       月度 " + beginTime.Month);
                sourceY += 0.6;
                CreateTableAutoMonthReport(this.detail, resultDT, resultStatistical.ToTable(), CommonField, CommonName, factors, sourceY);
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
            double width = (ColCount - 1) != 0 ? (tableWidth * 10 - 28) / (ColCount - 1) : 20;
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
                string code = columnField[i];
                if (code.Equals("MaxOneHourO3") || code.Equals("Max8HourO3")) code = "a05024";
                if (name.Contains("测点名称")) colSize.Width = Unit.Mm(28);
                else if (name.Contains("测点名称")) colSize.Width = Unit.Mm(28);

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
                    if (i < 10)
                    {
                        textboxGroup.Value = name;//列名        
                    }
                    else
                    {
                        string unit = factors.Where(x => x.PollutantCode.ToString().Equals(code)).Select(x => x.PollutantMeasureUnit).FirstOrDefault();
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

                if (i < 2)
                    tb.ColumnGroups.Add(tableGroupColumn);
                else if (i > 1 && i < 6)
                {
                    textBox.Value = "采样开始时间";
                    parentGroup.ChildGroups.Add(tableGroupColumn);
                    parentGroup.ReportItem = textBox;
                    if (i == 5)
                    {
                        tb.ColumnGroups.Add(parentGroup);
                        parentGroup = new Telerik.Reporting.TableGroup();
                    }

                }
                else if (i > 5 && i < 10)
                {
                    textBox.Value = "采样结束时间";
                    parentGroup.ChildGroups.Add(tableGroupColumn);
                    parentGroup.ReportItem = textBox;
                    if (i == 9)
                    {
                        tb.ColumnGroups.Add(parentGroup);
                        parentGroup = new Telerik.Reporting.TableGroup();
                    }

                }
                else
                {
                    parentGroup = new Telerik.Reporting.TableGroup();
                    textBox.Value = name;
                    parentGroup.ChildGroups.Add(tableGroupColumn);
                    parentGroup.ReportItem = textBox;
                    tb.ColumnGroups.Add(parentGroup);
                }

                textBoxTable = new Telerik.Reporting.HtmlTextBox();
                textBoxTable.Style.BorderColor.Default = Color.Black;
                textBoxTable.Style.BorderStyle.Default = BorderType.Solid;
                textBoxTable.Style.BorderWidth.Default = Unit.Pixel(0.5);
                textBoxTable.Style.TextAlign = HorizontalAlign.Center;
                textBoxTable.Style.VerticalAlign = VerticalAlign.Middle;
                textBoxTable.Style.Font.Size = Unit.Point(pointSize);
                textBoxTable.Size = colSize;
                if (i < 2)
                    textBoxTable.Value = "=Fields." + columnField[i];
                else
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
                        textBoxTable.Size = colSize;

                        if (i == 9)
                        {
                            textBoxTable.Value = dtRow.Rows[k]["Statistical"].ToString();
                            tb.Body.SetCellContent(k + 1, i, textBoxTable);
                        }
                        else
                        {
                            try
                            {
                                if (dtRow.Columns.Contains(columnField[i]))
                                {
                                    if (dtRow.Rows[k][columnField[i]] == DBNull.Value)
                                        textBoxTable.Value = "--";
                                    else
                                        textBoxTable.Value = dtRow.Rows[k][columnField[i]].ToString();
                                }
                                else textBoxTable.Value = "";
                            }
                            catch
                            {
                                textBoxTable.Value = "";
                            }
                            tb.Body.SetCellContent(k + 1, i, textBoxTable);
                        }
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