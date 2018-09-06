<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AirFrequencyDistributionChart.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.DataAnalyze.AirFrequencyDistributionChart" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <script type="text/javascript">
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
        <script src="../../../Resources/JavaScript/HighCharts/exporting.js"></script>
        <script type="text/javascript">
            $(function () {
                InitTogetherChart();
            });
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
                var ytext;
                createChart = function () {
                    Highcharts.setOptions({

                        lang: {
                            rangeSelectorZoom: ''    //隐藏Zoom
                        }
                    });
                    $('#container').highcharts({
                        chart: {
                            type: 'column'
                            //inverted: true
                        },
                        title: {
                            text: ''
                        },

                        subtitle: {
                            text: ''
                        },
                        xAxis: {
                            categories: strData,
                            title: {
                                text: ''
                            }
                        },
                        yAxis: {

                            lineWidth: 1,
                            title: { text: ytext }
                        },
                        legend: {
                            layout: 'vertical', align: 'center',
                            verticalAlign: 'bottom', borderWidth: 0
                        },
                        series: seriesOptions
                    })
                };
                var heavyMetalMonitor = $("#hdFrequencyData").val();//查询的总的数据
                var hiddenData = $("#hdFrequencyRange").val();//频数
                var factorname = $("#hdFactorName").val();
                var diameter = JSON.parse(hiddenData);
                ytext = factorname + "浓度日均值频率分布<br>(%)";
                
                var effectData = JSON.parse(heavyMetalMonitor);
                $.each(diameter, function (key, obj) {

                    strData.push(obj.RegionName);
                });
                var pointobj = new Object();

                pointobj.data = [];
                pointobj.name = factorname;
                $.each(effectData, function (key, obj) {
                    $.each(diameter, function (key, obj1) {
                        var allData = [];

                        if (obj[obj1.RegionName] != null) {
                            allData.push(obj1.RegionName, parseFloat(obj[obj1.RegionName]));
                        } else {
                            allData.push(obj1.RegionName, null);
                        }
                        pointobj.data.push(allData);

                    });

                });
                seriesOptions.push(pointobj);
                createChart();
            }
        </script>
        <table style="width: 100%; display: none" class="Table_Customer">
            <tr class="btnTitle">
                <td class="btnTitle">
                    <asp:ImageButton ID="btnSave" runat="server" OnClick="btnSave_Click" SkinID="ImgBtnSave" />
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
                <asp:HiddenField ID="hdFrequencyData" runat="server" />
                <asp:HiddenField ID="hdFrequencyRange" runat="server" />
                <asp:HiddenField ID="hdFactorName" runat="server" />
            </telerik:RadPane>
        </telerik:RadSplitter>
    </form>
</body>
</html>
