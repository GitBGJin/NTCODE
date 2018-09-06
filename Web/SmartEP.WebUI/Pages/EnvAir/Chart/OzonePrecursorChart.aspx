﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OzonePrecursorChart.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Chart.OzonePrecursorChart" %>

<%@ Register TagPrefix="CbxRsm" TagName="FactorCbxRsm" Src="~/Controls/FactorCbxRsm.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%--<script src="../../../Resources/JavaScript/JQuery/jquery-1.9.0.js"></script>
        <script src="../../../Resources/highcharts.js"></script>
        <script src="../../../Resources/heatmap.js"></script>
        <script src="../../../Resources/ChartHeatmap.js"></script>--%>


        <%--<script src="../../../Resources/JavaScript/HighCharts/exporting.js"></script>
        <script src="../../../Resources/JavaScript/HighCharts/data.js"></script>--%>
        <script src="../../../Resources/JavaScript/DrillDown/jquery-1.8.3.min.js"></script>
        <script src="../../../Resources/JavaScript/DrillDown/highcharts.js"></script>
        <script src="../../../Resources/JavaScript/DrillDown/data.js"></script>
        <script src="../../../Resources/JavaScript/DrillDown/drilldown.js"></script>
        <script src="../../../Resources/JavaScript/DrillDown/exporting.js"></script>
        <%--<script src="../../../Resources/JavaScript/PieHighCharts/jquery-1.8.3.min.js"></script>--%>
        <script type="text/javascript">
            $("document").ready(function () {
                generate();

            });

            function generate() {
                
                Highcharts.data({
                    csv: document.getElementById('hdBrandData').value,
                    itemDelimiter: '\t',
                    parsed: function (columns) {
                        var brands = {},
                            brandsData = [],
                            versions = {},
                            drilldownSeries = [];
                        // 解析百分比字符串
                        columns[1] = $.map(columns[1], function (value) {
                            if (value.indexOf('%') === value.length - 1) {
                                value = parseFloat(value);
                            }
                            return value;
                        });
                        $.each(columns[0], function (i, name) {
                            var brand,
                                version;
                            if (i > 0) {
                                // Remove special edition notes
                                name = name.split(' -')[0];
                                // 拆分
                                version = name.match(/\[.*\]/);
                                if (version) {
                                    version = version[0];
                                }
                                brand = name.replace(version, '');
                                //创建主数据
                                if (!brands[brand]) {
                                    brands[brand] = columns[1][i];
                                } else {
                                    brands[brand] += columns[1][i];
                                }
                                // 创建版本数据
                                if (version !== null) {
                                    if (!versions[brand]) {
                                        versions[brand] = [];
                                    }
                                    versions[brand].push([version, columns[1][i]]);
                                }
                            }
                        });
                        $.each(brands, function (name, y) {
                            brandsData.push({
                                name: name,
                                y: y,
                                drilldown: versions[name] ? name : null
                            });
                        });
                        $.each(versions, function (key, value) {
                            drilldownSeries.push({
                                name: key,
                                id: key,
                                data: value
                            });
                        });
                        // 创建图例
                        $('#container').highcharts({
                            chart: {
                                type: 'pie'
                            },
                            title: {
                                text: '非甲烷碳氢化合物各因子浓度占比'
                            },
                            subtitle: {
                                text: '单击每个二级类查看每个因子所占比例'
                            },
                            plotOptions: {
                                series: {
                                    dataLabels: {
                                        enabled: true,
                                        format: '{point.name}<br/>{point.y:.3f}%'
                                    }
                                }
                            },
                            tooltip: {
                                headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                                pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.3f}%</b><br/>'
                            },
                            series: [{
                                name: '因子',
                                colorByPoint: true,
                                data: brandsData
                            }],
                            drilldown: {
                                series: drilldownSeries
                            }
                        });
                    }
                });
            }
            
            //$(function () {
                
            //});

        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
        <div id="container" style="min-width: 310px; max-width: 600px; height: 400px; margin: 0 auto"></div>
        <pre id="tsv" style="display:none" runat="server"></pre>
        <asp:HiddenField ID="hdBrandData" runat="server" value="Browser Version	Total Market Share "/>
        <CbxRsm:FactorCbxRsm runat="server" ApplicationType="Air"  Visible="false" DefaultAllSelected="true" CbxWidth="380" DropDownWidth="420" ID="factorCbxRsm"></CbxRsm:FactorCbxRsm>
    </form>
</body>
</html>
