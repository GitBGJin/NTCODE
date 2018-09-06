﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PolaryChart.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Chart.PolaryChart" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script src="../../../Resources/JavaScript/Polary/jquery-1.8.3.min.js"></script>
        <script src="../../../Resources/JavaScript/Polary/highcharts.js"></script>
        <script src="../../../Resources/JavaScript/Polary/highcharts-more.js"></script>
        <script src="../../../Resources/JavaScript/Polary/exporting.js"></script>
        <script src="../../../Resources/JavaScript/Polary/highcharts-zh_CN.js"></script>
        <script type="text/javascript">
            $("document").ready(function () {
                var allData = document.getElementById("hddataForAjax").value;
                var PointID = allData.split(',')[0];
                var FactorCode = allData.split(',')[4];
                var dtBegin = allData.split(',')[5];
                var dtEnd = allData.split(',')[6];
                var radlDataType = allData.split(',')[3];
                var WindDir = allData.split(',')[1];
                var PolaryType = allData.split(',')[2];
                var flag = allData.split(',')[7];

                $.ajax({
                    type: "POST", //用POST方式传输
                    data: {
                        "PointID": PointID,
                        "FactorCode": FactorCode,
                        "dtBegin": dtBegin,
                        "dtEnd": dtEnd,
                        "radlDataType": radlDataType,
                        "WindDir": WindDir,
                        "PolaryType": PolaryType,
                        "flag": flag
                    },
                    dataType: "", //数据格式:JSON                  
                    url: "../ChartAjaxRequest/PolaryAjax.ashx",
                    cache: false, //指令
                    async: false, //取消同步

                    beforeSend: function () {
                    }, //发送数据之前
                    success: function (msg) {
                        if (msg == "Without Wind") {
                            InitA();
                        }
                        else {
                            var tsm = eval("(" + msg + ")");
                            //var tsm = "";
                            InitChart(tsm);
                        }
                    },
                    error: function (res) {
                        //alert(res);
                    }
                });
            });

            function InitA() {
                alert("无效风速风向因子，无法绘制图形");
            }

            function InitChart(tsm) {
                $('#container').highcharts({
                    chart: {
                        polar: true
                    },
                    title: {
                        text: '污染物分布玫瑰图'
                    },
                    pane: {
                        startAngle: 0,
                        endAngle: 360
                    },
                    xAxis: {
                        categories: tsm.indicator
                    },
                    yAxis: [{
                        lineWidth: 1,
                        lineColor: Highcharts.getOptions().colors[0],
                        tickAmount: 4,
                        title: {
                        },
                    }, {
                        angle: 90,
                        lineWidth: 1,
                        //lineColor: Highcharts.getOptions().colors[1],
                        lineColor: 'blue',
                        tickAmount: 4,
                        title: {
                        },
                    }, {
                        angle: 180,
                        lineWidth: 1,
                        //lineColor: Highcharts.getOptions().colors[2],
                        lineColor: 'green',
                        tickAmount: 4,
                        title: {
                        },
                    }, {
                        angle: 270,
                        lineWidth: 1,
                        //lineColor: Highcharts.getOptions().colors[3],        
                        lineColor: 'yellow',
                        tickAmount: 4,
                        title: {
                        },
                    }],
                    plotOptions: {
                        column: {
                            pointPadding: 0,
                            groupPadding: 0
                        }
                    },
                    series: tsm.series
                });
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
        <div id="container" style="height: 400px; width: 400px;" runat="server"></div>
        <asp:HiddenField ID="hddataForAjax" runat="server" />
    </form>
</body>
</html>
