﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OzonePrecursorTwoChart.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Chart.OzonePrecursorTwoChart" %>
<%@ Register TagPrefix="CbxRsm" TagName="FactorCbxRsm" Src="~/Controls/FactorCbxRsm.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script src="../../../Resources/JavaScript/PieChart/jquery-1.8.3.min.js"></script>
        <script src="../../../Resources/JavaScript/PieChart/highcharts.js"></script>
        <script src="../../../Resources/JavaScript/PieChart/exporting.js"></script>
        <script src="../../../Resources/JavaScript/DrillDown/highcharts-zh_CN.js"></script>
        <script type="text/javascript">
            $("document").ready(function () {
                generate();

            });
            function generate() {
                var value = eval("(" + hdBrandData.value + ")");
                console.log(value);
                $('#container').highcharts({
                    chart: {
                        plotBackgroundColor: null,
                        plotBorderWidth: null,
                        plotShadow: false
                    },
                    title: {
                        text: 'VOCs二级类饼图'
                    },
                    tooltip: {
                        headerFormat: '{series.name}<br>',
                        pointFormat: '{point.name}:<b>{point.percentage:.1f}%</b>'
                    },
                    plotOptions: {
                        pie: {
                            allowPointSelect: true,
                            cursor: 'pointer',
                            dataLabels: {
                                enabled: true,
                                format: '<b>{point.name}</b>:{point.percentage:.1f} %',
                                style: {
                                    color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                                }
                            }
                        }
                    },
                    series: [{
                        type: 'pie',
                        name: '二级类占比图',
                        data: value

                    }]
                });
            };

            </script>
        </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
        <div id="container" style="min-width: 310px; max-width: 600px; height: 400px; margin: 0 auto"></div>
        <pre id="tsv" style="display:none" runat="server"></pre>
        <asp:HiddenField ID="hdBrandData" runat="server" value=""/>
        <CbxRsm:FactorCbxRsm runat="server" ApplicationType="Air"  Visible="false" DefaultAllSelected="true" CbxWidth="380" DropDownWidth="420" ID="factorCbxRsm"></CbxRsm:FactorCbxRsm>
    </form>
</body>
</html>
