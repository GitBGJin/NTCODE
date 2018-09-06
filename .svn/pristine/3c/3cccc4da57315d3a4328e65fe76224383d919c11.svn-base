<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DayDevelementChart.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Chart.DayDevelementChart" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
                var CorL = document.getElementById("CorL").value;

                var effectData = eval("(" + allData + ")");

                var chart = {
                    type: CorL
                };
                var title = {
                    text: '日变化趋势图'
                };
                var subtitle = {
                    text: ''
                };

                var xAxis = {
                    categories: ['0时', '1时', '2时', '3时', '4时', '5时', '6时', '7时', '8时', '9时', '10时', '11时', '12时', '13时', '14时', '15时', '16时', '17时', '18时', '19时', '20时', '21时', '22时', '23时']
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

                //var tooltip = {
                //    valueSuffix: '',
                //    //pointFormat: '<tr><td style="color: {series.color}">{series.name}: </td>' + '<td "><b>{point.y} </b> </td>' + '<td></td>' + '</tr>'
                //    formatter: function () {
                //        return '<b>' + this.series.name + '</b>:' + this.y;
                //    }
                //}

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
                //json.tooltip = tooltip;
                json.legend = legend;
                json.series = series;

                $('#container').highcharts(json);

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
        <asp:HiddenField ID="CorL" runat="server" />
    </form>
</body>
</html>
