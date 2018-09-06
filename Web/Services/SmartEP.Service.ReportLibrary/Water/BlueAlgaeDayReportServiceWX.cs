namespace SmartEP.Service.ReportLibrary.Water
{
    using SmartEP.Core.Generic;
    using SmartEP.Core.Interfaces;
    using SmartEP.Service.AutoMonitoring.Interfaces;
    using SmartEP.Service.BaseData.MPInfo;
    using SmartEP.Service.DataAnalyze.Water.DataQuery;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using System.Linq;

    /// <summary>
    /// Summary description for BlueAlgaeDayReportReportServiceWX.
    /// </summary>
    public partial class BlueAlgaeDayReportServiceWX : Telerik.Reporting.Report
    {
        double pointSize = 9;//表格字体大小
        double tableWidth = 11;//表格宽度cm（除因子列）
        double tdcolumnWidth = 2;//因子列列宽cm
        double tdcolumnHight = 0.5;//表格每行高度cm

        public BlueAlgaeDayReportServiceWX()
        {
            InitializeComponent();
        }

        #region 加载报表
        /// <summary>
        /// 加载报表
        /// </summary>
        /// <param name="pointID">测点</param>
        /// <param name="factorList">因子</param>
        /// <param name="dv">数据源</param>
        public void BindingPortReport(string[] pointID, IList<IPollutant> factorList, DataTable dt)
        {
            //计算Report总宽度
            double width = (double)factorList.Count * tdcolumnWidth + tableWidth;
            double sourceY = 0;
            CreateHtmlTextBox(0, sourceY, width, tdcolumnHight, 12, true, "center", "浮标水质自动监测预警报告");
            sourceY += 0.5;
            CreateHtmlTextBox(0, sourceY, width, tdcolumnHight, 10, false, "right", DateTime.Now.ToString("yyyy年MM月dd日"));
            sourceY += 0.5;
            CreateTable(sourceY, factorList, dt);
        }
        #endregion

        #region 创建HtmlTextBox控件
        /// <summary>
        /// 创建HtmlTextBox控件
        /// </summary>
        /// <param name="x">X轴位置</param>
        /// <param name="y">Y轴位置</param>
        /// <param name="width">控件宽度</param>
        /// <param name="height">控件高度</param>
        /// <param name="font_Size">字体大小</param>
        /// <param name="isBold">是否加粗</param>
        /// <param name="center">是否居中</param>
        /// <param name="value">绑定值</param>
        private void CreateHtmlTextBox(double x, double y, double width, double height, double font_Size, bool isBold, string location, string value)
        {
            HtmlTextBox htmlTextBox = new HtmlTextBox();
            htmlTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(x), Telerik.Reporting.Drawing.Unit.Cm(y));
            htmlTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(width), Telerik.Reporting.Drawing.Unit.Cm(height));
            htmlTextBox.Style.Font.Bold = isBold;
            htmlTextBox.Style.Font.Name = "宋体";
            htmlTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(font_Size);
            if (location.Equals("center"))
            {
                htmlTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            }
            else if (location.Equals("left"))
            {
                htmlTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            }
            else if (location.Equals("right"))
            {
                htmlTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            }
            htmlTextBox.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            htmlTextBox.Value = value;
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] { htmlTextBox });
        }
        #endregion

        #region 创建表格
        /// <summary>
        /// 创建表格
        /// </summary>
        /// <param name="souceY">Y轴坐标</param>
        /// <param name="factors">因子</param>
        /// <param name="dt">数据源</param>
        public void CreateTable(double souceY, IList<IPollutant> factors, DataTable dt)
        {
            //数据行数
            int rowcount = dt.Rows.Count;
            //组装因子Code,Name,Unit
            string factorcode = "", factorname = "", factorunit = "";
            foreach (IPollutant factor in factors)
            {
                factorcode += factor.PollutantCode + ";";
                factorname += factor.PollutantName + ";";
                factorunit += factor.PollutantMeasureUnit + ";";
            }
            factorcode = factorcode.Substring(0, factorcode.Length - 1);
            string[] codelist = factorcode.Split(';');
            factorname = factorname.Substring(0, factorname.Length - 1);
            string[] namelist = factorname.Split(';');
            factorunit = factorunit.Substring(0, factorunit.Length - 1);
            string[] unitlist = factorunit.Split(';');
            //组装Table标题
            string[] tabletitle = ("水域;站点名称;月;日;" + factorname + ";预警类别").Split(';');
            //table列数
            int columncount = tabletitle.Length;

            //表格设置
            Telerik.Reporting.Table tb = new Table();
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] { tb });
            //table位置
            tb.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(souceY));

            //字体样式
            tb.Style.Font.Name = "宋体";
            tb.Style.Font.Size = Unit.Point(pointSize);
            //必须清除原有的行列样式
            tb.ColumnGroups.Clear();
            tb.Body.Columns.Clear();
            tb.Body.Rows.Clear();

            #region 创建列，注意宽度与tableWidth对应
            tb.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Cm(3D)));//水域
            tb.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Cm(3D)));//站点名称
            tb.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Cm(1D)));//月
            tb.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Cm(1D)));//日
            //因子列
            for (int i = 0; i < namelist.Length; i++)
            {
                tb.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Cm(2D)));
            }
            tb.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Cm(3D)));//预警类别

            #endregion

            #region 创建行

            #region 表头

            for (int columnid = 0; columnid < columncount; columnid++)
            {
                Telerik.Reporting.TextBox tb_textBox = new Telerik.Reporting.TextBox();
                tb_textBox.Style.Font.Size = Unit.Point(10D);
                tb_textBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
                tb_textBox.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                tb_textBox.Style.BorderColor.Default = Color.Black;
                tb_textBox.Style.BorderStyle.Default = BorderType.Solid;
                tb_textBox.Style.BorderWidth.Default = Unit.Pixel(0.5);

                Telerik.Reporting.TableGroup tb_group = new Telerik.Reporting.TableGroup();

                //需要合并单元格的列
                if (columnid < 4 || columnid == 11)
                {
                    tb_group.ReportItem = tb_textBox;
                    tb.ColumnGroups.Add(tb_group);
                    if (columnid == 2 || columnid == 3)
                    {
                        tb_textBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1D), Telerik.Reporting.Drawing.Unit.Cm(tdcolumnHight * 2));
                    }
                    else
                    {
                        tb_textBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3D), Telerik.Reporting.Drawing.Unit.Cm(tdcolumnHight * 2));
                    }
                    tb_textBox.Value = tabletitle[columnid];
                    tb.Items.AddRange(new Telerik.Reporting.ReportItemBase[] { tb_textBox });
                }
                else
                {
                    //创建因子标题行
                    Telerik.Reporting.TextBox tb_textBox_new = new Telerik.Reporting.TextBox();
                    tb_textBox_new.Style.Font.Size = Unit.Point(10D);
                    tb_textBox_new.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
                    tb_textBox_new.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    tb_textBox_new.Style.BorderColor.Default = Color.Black;
                    tb_textBox_new.Style.BorderStyle.Default = BorderType.Solid;
                    tb_textBox_new.Style.BorderWidth.Default = Unit.Pixel(0.5);

                    Telerik.Reporting.TableGroup tb_group_new = new Telerik.Reporting.TableGroup();
                    tb_group_new.ReportItem = tb_textBox_new;
                    tb_group.ChildGroups.Add(tb_group_new);
                    tb_group.ReportItem = tb_textBox;
                    tb.ColumnGroups.Add(tb_group);

                    tb_textBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2D), Telerik.Reporting.Drawing.Unit.Cm(tdcolumnHight));
                    tb_textBox.Value = tabletitle[columnid];

                    tb_textBox_new.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2D), Telerik.Reporting.Drawing.Unit.Cm(tdcolumnHight));
                    tb_textBox_new.Value = unitlist[columnid - 4];

                    tb.Items.AddRange(new Telerik.Reporting.ReportItemBase[] { tb_textBox, tb_textBox_new });
                }
            }

            #endregion

            #region 数据

            Telerik.Reporting.TableGroup datagroup = new Telerik.Reporting.TableGroup();
            for (int rowid = 0; rowid < rowcount; rowid++)
            {
                Telerik.Reporting.TableGroup tb_group = new Telerik.Reporting.TableGroup();
                datagroup.ChildGroups.Add(tb_group);
                tb.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(tdcolumnHight)));
                for (int columnid = 0; columnid < columncount; columnid++)
                {
                    Telerik.Reporting.TextBox tb_textBox = new Telerik.Reporting.TextBox();
                    tb_textBox.Style.Font.Size = Unit.Point(pointSize);
                    tb_textBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
                    tb_textBox.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    tb_textBox.Style.BorderColor.Default = Color.Black;
                    tb_textBox.Style.BorderStyle.Default = BorderType.Solid;
                    tb_textBox.Style.BorderWidth.Default = Unit.Pixel(0.5);

                    tb.Body.SetCellContent(rowid, columnid, tb_textBox);
                    //单元格宽度
                    double tdcolumn = 2D;
                    if (columnid < 2 || columnid == 11)
                    {
                        tdcolumn = 3D;
                    }
                    if (columnid == 2 || columnid == 3)
                    {
                        tdcolumn = 1D;
                    }
                    tb_textBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(tdcolumn), Telerik.Reporting.Drawing.Unit.Cm(tdcolumnHight));

                    //单元格内容
                    if (columnid == 0)
                    {
                        //水域
                        tb_textBox.Value = Convert.ToString(dt.Rows[rowid]["Region"]).ToString();
                    }
                    else if (columnid == 1)
                    {
                        //站点名称
                        tb_textBox.Value = Convert.ToString(dt.Rows[rowid]["PortName"]).ToString();
                    }
                    else if (columnid == 2)
                    {
                        //月
                        tb_textBox.Value = Convert.ToString(dt.Rows[rowid]["Month"]).ToString();
                    }
                    else if (columnid == 3)
                    {
                        //日
                        tb_textBox.Value = Convert.ToString(dt.Rows[rowid]["Day"]).ToString();
                    }
                    else if (columnid > 3 && columnid < 11)
                    {
                        //因子
                        tb_textBox.Value = Convert.ToString(dt.Rows[rowid][codelist[columnid - 4]]).ToString();
                    }
                    else
                    {
                        //预警类别
                        tb_textBox.Value = Convert.ToString(dt.Rows[rowid]["Class"]).ToString();
                    }

                    tb.Items.AddRange(new Telerik.Reporting.ReportItemBase[] { tb_textBox });
                }
            }
            datagroup.Groupings.Add(new Telerik.Reporting.Grouping(null));
            tb.RowGroups.Add(datagroup);
            #endregion

            #endregion

            //长宽,计算高度需要加上2行表头
            double width = (double)codelist.Length * tdcolumnWidth + tableWidth;
            double height = (double)(rowcount + 2) * tdcolumnHight;
            tb.Size = new SizeU(Unit.Cm(width), Unit.Cm(height));

        }
        #endregion
    }
}