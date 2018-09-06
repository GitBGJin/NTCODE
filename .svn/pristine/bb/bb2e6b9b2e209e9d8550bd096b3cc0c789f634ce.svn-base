<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DataAvgDayCharts.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.DataAnalyze.DataAvgDayCharts" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <script type="text/javascript">
            function GetParentData() {
                var parentData = window.parent.GetData();
                document.getElementById("hdPointId").value = parentData.pointIds;
                document.getElementById("hdDtStart").value = parentData.dtStart;
                document.getElementById("hdDtEnd").value = parentData.dtEnd;
                document.getElementById("hdPointType").value = parentData.pointType;
            }
            function SaveData() {
                var buttonSave = document.getElementById("btnSave");
                buttonSave.click();
            }

        </script>
        <script src="../../../Resources/JavaScript/JQuery/jquery-1.9.0.min.js"></script>
        <script src="../../../Resources/JavaScript/Highcharts/highcharts.js"></script>
        <script src="../../../Resources/JavaScript/ChartOperator/ShowChart.js"></script>
        <script src="../../../Resources/JavaScript/ChartOperator/merge.js"></script>
        <script src="../../../Resources/JavaScript/Highcharts/highcharts-more.js"></script>
        <script type="text/javascript">
            $(function () {
                var flag = $('#hdFlag').val();
                if (flag == "" || flag == "0") {
                    InitTogetherChart();
                } else {
                    createChart(flag);
                }
            });
            //绘制除K线图之外的其他图形
            function createChart(flag) {
                var hiddenData = $("#hdPointNames").val();
                var diameter = JSON.parse(hiddenData);
                var strData = [];
                $.each(diameter, function (key, obj) {
                    strData.push(obj.RegionName);
                });
                var allData = $("#hdjsonData").val();
                var effectData = eval("(" + allData + ")");
                var factorname = $("#hdFactor").val();
                var chart = {
                    type: "",
                };
                var title;
                if (flag == "1") {
                    title = {
                        text: factorname + '超标统计'
                    };
                }
                else {
                    title = {
                        text: factorname + '百分位统计'
                    };
                }
                var xAxis = {
                    categories: strData,
                };
                var yAxis;
                if (flag == "1") {
                    yAxis = [{
                        title: {
                            text: '天数（天）'
                        },
                        plotLines: [{
                            value: 0,
                            width: 1,
                            color: '#808080'
                        }],
                    },
                {
                    opposite: true,
                    title: {
                        text: '超标率（%）'
                    },
                    plotLines: [{
                        value: 0,
                        width: 1,
                        color: '#808080'
                    }],
                }
                    ];
                }
                else {
                    yAxis = [{
                        title: {
                            text: '百分位数浓度'
                        },
                        plotLines: [{
                            value: 0,
                            width: 1,
                            color: '#808080'
                        }],
                    },
                {
                    opposite: true,
                    title: {
                        text: '百分位数超标倍数'
                    },
                    plotLines: [{
                        value: 0,
                        width: 1,
                        color: '#808080'
                    }],
                }
                    ];
                }

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
                json.xAxis = xAxis;
                json.yAxis = yAxis;
                json.tooltip = tooltip;
                json.legend = legend;
                json.series = series;
                json.plotOptions = plotOptions;
                json.credits = credits;

                $('#container').highcharts(json);
            }
            function InitTogetherChart() {
                var windowHeight = document.body.clientHeight;//网页可见区域高 屏幕可用工作区高度：window.screen.availHeight 
                var windowWidth = document.body.clientWidth;
                var divDataEffectRateHeight = (windowHeight - 70).toString() + "px";
                var divDataEffectRateWidth = (windowWidth).toString() + "px";
                document.getElementById("container").style.height = divDataEffectRateHeight;
                document.getElementById("container").style.width = divDataEffectRateWidth;
                $("#container").html("");

                var seriesOptions = [];
                var strData = [];

                createChart = function () {
                    Highcharts.setOptions({

                        lang: {
                            rangeSelectorZoom: ''    //隐藏Zoom
                        }
                    });
                    $('#container').highcharts({
                        chart: {
                            type: 'boxplot'
                        },

                        title: {
                            text: ''
                        },

                        legend: {
                            enabled: true
                        },

                        xAxis: {
                            categories: strData,
                            title: {
                                text: ''
                            }
                        },

                        yAxis: [{
                            title: {
                                text: 'μg/m3'
                            },
                            plotLines: [{
                                value: 932,
                                color: 'red',
                                width: 1,
                                label: {
                                    text: 'Theoretical mean: 932',
                                    align: 'center',
                                    style: {
                                        color: 'gray'
                                    }
                                }
                            }]
                        },
                        {
                            opposite:true,
                            title: {
                                text: 'mg/m3'
                            },
                            plotLines: [{
                                value: 932,
                                color: 'red',
                                width: 1,
                                label: {
                                    text: 'Theoretical mean: 932',
                                    align: 'center',
                                    style: {
                                        color: 'gray'
                                    }
                                }
                            }]
                        }],

                        series: seriesOptions
                    })
                };
                var heavyMetalMonitor = $("#hdpointiddata").val();
                var hiddenData = $("#hdPointNames").val();
                var FactorNamejson = $("#hdFactorName").val();
                var allData = [];
                var effectData = JSON.parse(heavyMetalMonitor);
                var diameter = JSON.parse(hiddenData);
                var FactorName = JSON.parse(FactorNamejson);
                $.each(diameter, function (key, obj) {

                    strData.push(obj.RegionName);
                });
                var i = 0;
                $.each(FactorName, function (key, obj) {
                    var pointobj = new Object();

                    pointobj.data = [];

                    $.each(diameter, function (key, obj1) {
                        pointobj.name = obj.FactorName;
                        if (obj.FactorName.indexOf('CO') >= 0) {
                            pointobj.yAxis = 1;
                        }
                        else {
                            pointobj.yAxis = 0;
                        }
                        $.each(effectData, function (key, obj2) {
                            if (obj2.FactorName == obj.FactorName && obj1.RegionName == obj2.RegionName) {
                                var data = [];
                                if (obj2.lower == null || obj2.lower == "" || obj2.MedianLower == null || obj2.MedianLower == "" || obj2.MedianValueu == null || obj2.MedianValueu == "" || obj2.MedianUpper == null || obj2.MedianUpper == "" || obj2.upper == null || obj2.upper == "") {
                                    data.push(null, null, null, null, null);
                                } else {
                                    data.push(parseFloat(obj2.lower), parseFloat(obj2.MedianLower), parseFloat(obj2.MedianValueu), parseFloat(obj2.MedianUpper), parseFloat(obj2.upper));
                                }
                                pointobj.data.push(data);
                            }

                        });

                    });
                    seriesOptions.push(pointobj);
                });
                createChart();
            }
        </script>
        <table style="width: 100%; display: none" class="Table_Customer">
            <tr class="btnTitle">
                <td class="btnTitle">
                    <asp:ImageButton ID="btnSave" runat="server" OnClick="btnSave_Click" OnClientClick="GetParentData()" SkinID="ImgBtnSave" />
                </td>
            </tr>
        </table>
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" BorderWidth="0" BorderSize="0" BorderStyle="None"
            Width="100%">
            <telerik:RadPane ID="paneImage" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <!--Step:1 Prepare a dom for ECharts which (must) has size (width & hight)-->
                <!--Step:1 为ECharts准备一个具备大小（宽高）的Dom-->
                <div id="container" style="height: 80%; border-width: 0px; border-style: none;"></div>
                <asp:HiddenField ID="hdpointiddata" runat="server" />
                <asp:HiddenField ID="hdPointNames" runat="server" />
                <asp:HiddenField ID="hdFactorName" runat="server" />

                <asp:HiddenField ID="hdPointId" runat="server" />
                <asp:HiddenField ID="hdDtStart" runat="server" />
                <asp:HiddenField ID="hdDtEnd" runat="server" />
                <asp:HiddenField ID="hdPointType" runat="server" />
                <asp:HiddenField ID="hdFlag" runat="server" />
                <asp:HiddenField ID="hdjsonData" runat="server" />
                <asp:HiddenField ID="hdFactor" runat="server" />
            </telerik:RadPane>
        </telerik:RadSplitter>
    </form>
</body>
</html>
