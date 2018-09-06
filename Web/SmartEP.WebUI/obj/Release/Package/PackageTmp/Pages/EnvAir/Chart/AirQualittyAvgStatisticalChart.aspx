﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AirQualittyAvgStatisticalChart.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Chart.AirQualittyAvgStatisticalChart" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%--<script src="../../../Resources/jquery-1.9.0.min.js"></script>--%>
        <script src="../../../Resources/JavaScript/JQuery/jquery-1.9.0.js"></script>
        <script src="../../../Resources/highcharts.js"></script>
        <script src="../../../Resources/heatmap.js"></script>
        <script src="../../../Resources/ChartHeatmap.js"></script>
        <script src="../../../Resources/JavaScript/HighCharts/exporting.js"></script>
        <script src="../../../Resources/JavaScript/Polary/highcharts-zh_CN.js"></script>
        <script type="text/javascript">
            $("document").ready(function () {
                generate();
            });

            function generate() {
                var allData = document.getElementById("hdjsonData").value;
                var chartType = document.getElementById("hdxData").value;
                var jsonTitle = document.getElementById("hdjsonTitle").value;
                var effectData = eval("(" + allData + ")");
                if (jsonTitle == "六参数浓度值(mg/m3)") {
                    jsonTitle = "六参数浓度值";
                    var jsonTitle1 = "六参数浓度值(μg/m3)";
                    var jsonTitle2 = "六参数浓度值(mg/m3)";
                }
                if (jsonTitle == "六参数浓度值" || jsonTitle == "六参数分指数(IAQI)") {
                    var categorie = ['PM2.5', 'PM10', 'NO2', 'SO2', 'CO', 'O3'];
                }
                else if (jsonTitle == "空气质量指数(AQI)") {
                    var categorie = ['空气质量指数(AQI)'];
                    var jsonTitle1 = jsonTitle;
                    var jsonTitle2 = jsonTitle;
                }
                else if (jsonTitle == "首要污染物浓度值(mg/m3)") {
                    var categorie = ['首要污染物浓度值'];
                    jsonTitle = "首要污染物浓度值";
                    var jsonTitle1 = jsonTitle;
                    var jsonTitle2 = jsonTitle;
                }
                var title = {
                    text: jsonTitle
                };
                var subtitle = {
                    text: ''
                };
                var chart = {
                    type: chartType
                };
                
                var xAxis = {
                    
                    categories: categorie
                    

                };

                //var yAxis = {
                //    title: {
                //        text: jsonTitle
                //    },
                //    plotLines: [{
                //        value: 0,
                //        width: 1,
                //        color: '#808080'
                //    }]
                //};
                var yAxis = [{
                    title: {
                        text: jsonTitle1,
                        labels: { format: '{value:.,0f}' }
                    }, opposite: false
                }, {
                    title: {
                    text: jsonTitle2,
                    labels: { format: '{value:.,0f}' },
                    }, opposite: true
                }];
                var tooltip = {
                    valueSuffix: ''
                }

                var legend = {
                    layout: 'vertical',
                    align: 'right',
                    verticalAlign: 'middle',
                    borderWidth: 0
                };

                var series = effectData;
                var json = {};
                json.chart = chart;
                json.title = title;
                json.subtitle = subtitle;
                json.xAxis = xAxis;
                json.yAxis = yAxis;
                json.tooltip = tooltip;
                json.legend = legend;
                json.series = series;
                //json.color = ['#058DC7'];
                $('#container').highcharts(json);
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
        <div id="container" style="height:500px; width: 100%;" runat="server"></div>
        <asp:HiddenField ID="hdjsonData" runat="server" />
        <asp:HiddenField ID="hdxData" runat="server" />
        <asp:HiddenField ID="hdjsonTitle" runat="server" />
    </form>
</body>
</html>
