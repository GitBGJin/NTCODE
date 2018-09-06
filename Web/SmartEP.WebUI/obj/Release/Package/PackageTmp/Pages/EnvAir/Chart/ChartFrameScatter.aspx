<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChartFrameScatter.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Chart.ChartFrameScatter" validateRequest="false"  %>

<%@ Register TagPrefix="CbxRsm" TagName="FactorCbxRsm" Src="~/Controls/FactorCbxRsm.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>

</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <%--<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1" UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="factorCbxRsm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="factorCbxRsm" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenX" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="factorCbxRsm1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="factorCbxRsm1" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenY" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManager>--%>
        <table style="width: 100%; height: 100%">
            <tr style="height: 40px">
                <td style="width: 40%; text-align: right">X轴： 
                    <CbxRsm:FactorCbxRsm runat="server" ApplicationType="Air" CbxWidth="150" OnSelectedChanged="factorCbxRsm_SelectedChanged" MultiSelected="false" DefaultAllSelected="false" DropDownWidth="210" ID="factorCbxRsmX"></CbxRsm:FactorCbxRsm>
                </td>
                <td style="width: 20%; text-align: center">VS</td>
                <td style="width: 40%; text-align: left">Y轴:
                    <CbxRsm:FactorCbxRsm runat="server" ApplicationType="Air" CbxWidth="150" OnSelectedChanged="factorCbxRsm_SelectedChanged" MultiSelected="false" DefaultAllSelected="false" DropDownWidth="210" ID="factorCbxRsmY"></CbxRsm:FactorCbxRsm>
                </td>
            </tr>
            <tr style="height: 100%">
                <td colspan="3">
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
                            
                            var xTitle = $("#HiddenX").val();
                            var yTitle = $("#HiddenY").val();
                            var data = [];
                            var valueTitle = $("#HiddenValue").val();
                            var dataarray = eval('(' + valueTitle + ')');
                            for (var obj1 in dataarray) {
                                data.push([parseFloat(dataarray[obj1].Xvalue), parseFloat(dataarray[obj1].Yvalue)]);
                            }
                            var data1 = [];
                            var valueTitle1 = $("#HiddenValueNew").val();
                            var dataarray1 = eval('(' + valueTitle1 + ')');
                            for (var obj2 in dataarray1) {
                                data1.push([parseFloat(dataarray1[obj2].xvalue), parseFloat(dataarray1[obj2].yvalue)]);
                            }

                            $('#container').highcharts({
                                xAxis: {
                                    title: {
                                        text: xTitle
                                    }
                                },
                                yAxis: {
                                    title: {
                                        text: yTitle
                                    }
                                },
                                title: {
                                    text: ''
                                },
                                series: [{
                                    type: 'line',
                                    name: '线性回归线',
                                    data: data1,
                                    marker: {
                                        enabled: false
                                    },
                                    states: {
                                        hover: {
                                            lineWidth: 0
                                        }
                                    },
                                    enableMouseTracking: false
                                }, {
                                    type: 'scatter',
                                    name: '浓度值',
                                    data: data,
                                    marker: {
                                        radius: 4
                                    }
                                }]
                            });
                        }
                    </script>
                    <div id="container" style="height: 100%; width: 100%;"></div>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="HiddenX" runat="server" Value="" />
        <asp:HiddenField ID="HiddenY" runat="server" Value="" />
        <asp:HiddenField ID="HiddenValue" runat="server" Value="" />
        <asp:HiddenField ID="HiddenValueNew" runat="server" Value="" />
    </form>
</body>
</html>
