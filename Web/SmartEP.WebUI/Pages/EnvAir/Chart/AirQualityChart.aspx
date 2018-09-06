﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AirQualityChart.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Chart.AirQualityChart" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%--<script src="../../../Resources/jquery-1.9.0.min.js"></script>--%>
        <script src="../../../Resources/JavaScript/JQuery/jquery-1.9.0.js"></script>
        <script src="../../../Resources/JavaScript/Polary/highcharts.js"></script>
        <script src="../../../Resources/JavaScript/HighCharts/exporting.js"></script>
        <script src="../../../Resources/JavaScript/Polary/highcharts-zh_CN.js"></script>
        <script type="text/javascript">
            $(function () {
                generate();
            });

            function generate() {
                var allData = document.getElementById("hdjsonData").value;
                var chartType = document.getElementById("hdchartType").value;
                var chartContent = document.getElementById("hdchartContent").value;
                var title = document.getElementById("hdtitle").value;
                var unit = document.getElementById("hdunit").value;
                var effectData = eval("(" + allData + ")");

                if (chartContent == "primaryAQI") {
                    var chart = {
                        type: chartType,
                        //plotBackgroundColor: '#ffff00',
                    };
                    var title = {
                        text: '空气质量指数(AQI)'
                    };
                    var subtitle = {
                        text: ''
                    };

                    var xAxis = {
                        type: 'datetime',
                        dateTimeLabelFormats: {
                            millisecond: '%H:%M:%S.%L',
                            second: '%H:%M:%S',
                            minute: '%H:%M',
                            hour: '%H:%M',
                            day: '%m-%d',
                            week: '%m-%d',
                            month: '%Y-%m',
                            year: '%Y'
                        }
                    };


                    var yAxis = {
                        title: {
                            text: 'AQI'
                        },
                        plotLines: [{
                            value: 0,
                            width: 1,
                            color: '#808080'
                        }],
                        plotBands: [{
                            //color: '#ffff00', // Color value
                            color: 'red',
                            from: 100, // Start of the plot band
                            to: 100 // End of the plot band         
                        }]
                    };

                    var tooltip = {
                        valueSuffix: '',
                        //pointFormat: '<tr><td style="color: {series.color}">{series.name}: </td>' + '<td "><b>{point.y} </b> </td>' + '<td></td>' + '</tr>'
                        formatter: function () {
                            return '<b>' + this.series.name + '</b>:' + this.y;
                        }
                    };

                    var plotOptions = {
                        series: {
                            connectNulls: true
                        }
                    };

                    var legend = {
                        layout: 'vertical',
                        align: 'right',
                        verticalAlign: 'middle',
                        borderWidth: 0
                    };
                    var credits = {
                        enabled: false
                    }

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
                    json.plotOptions = plotOptions;
                    json.credits = credits;

                    $('#container').highcharts(json);
                }

                if (chartContent == "primaryValue") {
                    var chart = {
                        type: chartType
                    };
                    var title = {
                        text: '首要污染物浓度'
                    };
                    var subtitle = {
                        text: ''
                    };

                    var xAxis = {
                        type: 'datetime',
                        dateTimeLabelFormats: {
                            millisecond: '%H:%M:%S.%L',
                            second: '%H:%M:%S',
                            minute: '%H:%M',
                            hour: '%H:%M',
                            day: '%m-%d',
                            week: '%m-%d',
                            month: '%Y-%m',
                            year: '%Y'
                        }
                    };

                    var plotOptions = {
                        series: {
                            connectNulls: true
                        }
                    };

                    var yAxis = {
                        title: {
                            text: '浓度'
                        },
                        plotLines: [{
                            value: 0,
                            width: 1,
                            color: '#808080'
                        }]
                    };
                    var tooltip = {
                        valueSuffix: '',
                        //pointFormat: '<tr><td style="color: {series.color}">{series.name}: </td>' + '<td "><b>' + '{point.z}  {point.y} </b> {point.u}</td>' + '<td></td>' + '</tr>'
                        formatter: function () {
                            return '<b>' + this.series.name + '</b>:' + this.point.z + ' ' + this.y + ' ' + this.point.u;
                        }
                    }

                    var legend = {
                        layout: 'vertical',
                        align: 'right',
                        verticalAlign: 'middle',
                        borderWidth: 0
                    };
                    var credits = {
                        enabled: false
                    }

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
                    json.plotOptions = plotOptions;
                    json.credits = credits;

                    $('#container').highcharts(json);
                }

                if (chartContent == "factorValue") {
                    var chart = {
                        type: chartType
                    };
                    var title = {
                        text: title
                    };
                    var subtitle = {
                        text: ''
                    };

                    var plotOptions = {
                        series: {
                            connectNulls: true
                        }
                    };

                    var xAxis = {
                        type: 'datetime',
                        dateTimeLabelFormats: {
                            millisecond: '%H:%M:%S.%L',
                            second: '%H:%M:%S',
                            minute: '%H:%M',
                            hour: '%H:%M',
                            day: '%m-%d',
                            week: '%m-%d',
                            month: '%Y-%m',
                            year: '%Y'
                        }
                    };
                    var yAxis = {
                        title: {
                            text: '浓度' + unit
                        },
                        plotLines: [{
                            value: 0,
                            width: 1,
                            color: '#808080'
                        }]
                    };

                    var tooltip = {
                        valueSuffix: '',
                        //pointFormat: '<tr><td style="color: {series.color}">{series.name}: </td>' + '<td "><b>{point.y} </b> </td>' + '<td></td>' + unit + '</tr>'
                        formatter: function () {
                            return '<b>' + this.series.name + '</b>:' + this.y + ' ' + unit;
                        }
                    }

                    var legend = {
                        layout: 'vertical',
                        align: 'right',
                        verticalAlign: 'middle',
                        borderWidth: 0
                    };
                    var credits = {
                        enabled: false
                    }

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
                    json.plotOptions = plotOptions;
                    json.credits = credits;

                    $('#container').highcharts(json);
                }

                if (chartContent == "factorIAQI") {
                    var chart = {
                        type: chartType
                    };
                    var title = {
                        text: title
                    };
                    var subtitle = {
                        text: ''
                    };

                    var plotOptions = {
                        series: {
                            connectNulls: true
                        }
                    };

                    var xAxis = {
                        type: 'datetime',
                        dateTimeLabelFormats: {
                            millisecond: '%H:%M:%S.%L',
                            second: '%H:%M:%S',
                            minute: '%H:%M',
                            hour: '%H:%M',
                            day: '%m-%d',
                            week: '%m-%d',
                            month: '%Y-%m',
                            year: '%Y'
                        }
                    };

                    var yAxis = {
                        title: {
                            text: 'IAQI'
                        },
                        plotLines: [{
                            value: 0,
                            width: 1,
                            color: '#808080'
                        }]
                    };

                    var tooltip = {
                        valueSuffix: '',
                        //pointFormat: '<tr><td style="color: {series.color}">{series.name}: </td>' + '<td "><b>{point.y} </b> </td>' + '<td></td>' + '</tr>'
                        formatter: function () {
                            return '<b>' + this.series.name + '</b>:' + ' ' + this.y;
                        }
                    }

                    var legend = {
                        layout: 'vertical',
                        align: 'right',
                        verticalAlign: 'middle',
                        borderWidth: 0
                    };

                    var credits = {
                        enabled: false
                    }

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
                    json.plotOptions = plotOptions;
                    json.credits = credits;

                    $('#container').highcharts(json);
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
        <div id="container" style="height: 500px; width: 100%;" runat="server"></div>
        <asp:HiddenField ID="hdjsonData" runat="server" />
        <asp:HiddenField ID="hdchartType" runat="server" />
        <asp:HiddenField ID="hdchartContent" runat="server" />
        <asp:HiddenField ID="hdtitle" runat="server" />
        <asp:HiddenField ID="hdunit" runat="server" />
    </form>
</body>
</html>
