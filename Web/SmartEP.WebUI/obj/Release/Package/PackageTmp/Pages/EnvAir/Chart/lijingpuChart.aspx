<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="lijingpuChart.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Chart.lijingpuChart" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%--<script src="../../../Resources/jquery-1.9.0.min.js"></script>--%>
        <script src="../../../Resources/JavaScript/JQuery/jquery-1.9.0.js"></script>
        <script src="../../../Resources/highcharts.js"></script>
        <script src="../../../Resources/heatmap.js"></script>
        <script src="../../../Resources/ChartHeatmap.js"></script>
        <script src="../../../Resources/JavaScript/HighCharts/exporting.js"></script>
        <script type="text/javascript">
            $("document").ready(function () {
                generate();
            });

            function generate() {
                var seriesOptions = [];
                var strData = [];
                var pointname = $("#hdName").val();//查询站点名
                var heavyMetalMonitor = $("#hdHeavyMetalMonitor").val();//查询的总的数据
                var hiddenData = $("#hiddendiameter").val();//粒径
                var allData = [];
                var effectData = JSON.parse(heavyMetalMonitor);
                var diameter = hiddenData.split(';');
                $.each(effectData, function (key, obj) {
                    for (var i = 0; i < diameter.length; i++) {

                        if (obj[diameter[i]] != null) {
                            allData.push([parseFloat(diameter[i]), parseFloat(obj[diameter[i]])]);
                        } else {
                            allData.push([parseFloat(diameter[i]), null]);
                        }
                    }

                });
                seriesOptions[0] = {
                    type: 'column',
                    name: pointname,
                    data: allData,
                    pointStart: 1,
                    zIndex: 1,
                    //yAxis: 0
                }

                createChart = function () {
                    Highcharts.setOptions({

                        lang: {
                            rangeSelectorZoom: ''    //隐藏Zoom
                        }
                    });
                    $('#container').highcharts({

                        chart: {
                            type: 'column',
                            //inverted: true
                        },
                        title: {
                            text: ''
                        },

                        subtitle: {
                            text: ''
                        },
                        xAxis: {
                            reversed: false,
                            //type: 'logarithmic',
                            //categories: strData,
                            //tickInterval: 1
                        },
                        yAxis: { title: { text: 'dw/dlogDp' } },
                        tooltip: { valueSuffix: 'dw/dlogDp' },
                        legend: {
                            layout: 'vertical', align: 'center',
                            verticalAlign: 'bottom', borderWidth: 0
                        },
                        series: seriesOptions
                    })
                };

                createChart();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1"
        UpdatePanelsRenderMode="Inline">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="preSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="hdnum" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="hdHeavyMetalMonitor" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="hiddendiameter" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="nextSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="hdnum" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="hdHeavyMetalMonitor" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="hiddendiameter" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    
        <div>
        </div>
        <div id="container" style="height:500px; width: 100%;" runat="server"></div>
        <div>
            <table>
                <tr>
                    <td>
                        <telerik:RadButton ID="preSearch" runat="server" BackColor="#3A94D3" ForeColor="White" Height="20" OnClick="preSearch_Click">
                            <ContentTemplate>
                                <asp:Label runat="server" ID="Label5" ForeColor="White" Text="上一张"></asp:Label>
                            </ContentTemplate>
                        </telerik:RadButton>
                    </td>
                    <td>
                        <label runat="server" id="lbTime"></label>
                    </td>
                    <td>
                        <div id="dvNext" runat="server">
                            <telerik:RadButton ID="nextSearch" runat="server" BackColor="#3A94D3" ForeColor="White" Height="20" OnClick="nextSearch_Click">
                                <ContentTemplate>
                                    <asp:Label runat="server" ID="Label5" ForeColor="White" Text="下一张"></asp:Label>
                                </ContentTemplate>
                            </telerik:RadButton>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <asp:HiddenField ID="hdnum" runat="server" Value="0"/>
        <asp:HiddenField ID="hdHeavyMetalMonitor" runat="server" />
        <asp:HiddenField ID="hiddendiameter" runat="server" />
        <asp:HiddenField ID="hdName" runat="server" />
    </form>
</body>
</html>
