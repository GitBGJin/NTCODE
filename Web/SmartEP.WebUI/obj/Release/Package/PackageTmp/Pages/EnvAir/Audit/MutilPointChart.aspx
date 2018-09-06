<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MutilPointChart.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Audit.MutilPointChart" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link type="text/css" rel="stylesheet" href="../../../App_Themes/Neptune/Telerik.Web.UI.Skins/Metro/RadSplitterAudit.css" />
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script src="../../../Resources/JavaScript/Echarts/build/dist/echarts.js"></script>
        <script src="../../../Resources/JavaScript/JQuery/jquery-1.9.0.js"></script>
        <script type="text/javascript">
            //Splite加载事件（初始化Chart）
            function loadSplitter(sender) {
                var bodyWidth = document.body.clientWidth;
                var bodyHeight = document.body.clientHeight;
                sender.set_width(bodyWidth);//初始化Splitter高度及宽度
                sender.set_height(bodyHeight);
                LoadingData();//加载Echarts图表数据
            }

            //加载ECharts数据
            function LoadingData() {
                try {
                    //var pointID = "";
                    //var PointName = "";
                    //var factorCode = 'a05024';
                    //var factorName = '臭氧';
                    //var startTime = "2015-08-28";
                    var pointID = "";
                    var PointName = "";
                    var factorCode = '<%=Request.QueryString["factorCode"]%>';
                    var startTime = '<%=Request.QueryString["startTime"]%>';
                    var treeNodes = $find("<%= RadPortTree.ClientID %>").get_checkedNodes();
                    for (var i = 0; i < treeNodes.length ; i++) {
                        var node = treeNodes[i];
                        if (i == 0) {
                            pointID += node.get_value();
                            PointName += node.get_text();
                        }
                        else {
                            pointID += ";" + node.get_value();
                            PointName += ";" + node.get_text();
                        }
                    }
                    $.ajax({
                        type: "POST", //用POST方式传输
                        data: {
                            "FactorCode": factorCode,
                            "StartTime": startTime,
                            "PointID": pointID,
                            "PointName": PointName,
                            "ApplicationUID": '<%=Session["applicationUID"]%>'
                        },
                        dataType: "", //数据格式:JSON                  
                        url: "AuditAjaxHandler.ashx?DataType=SingleFactor",
                        cache: false, //指令
                        async: false, //取消同步

                        beforeSend: function () {
                        }, //发送数据之前
                        success: function (msg) {
                            var tsm = eval("(" + msg + ")");
                            InitChart(tsm);
                        },
                        error: function (res) {
                            //alert("无数据！");
                        }
                    });
                } catch (e) {
                }
            }

            function InitChart(tsm) {
                // 路径配置
                require.config({
                    paths: {
                        echarts: '../../../Resources/JavaScript/Echarts/build/dist'
                    }
                });

                // 使用
                require(
                    [
                        'echarts',
                        'echarts/chart/line', // 使用柱状图就加载bar模块，按需加载
                          'echarts/chart/bar' // 使用柱状图就加载bar模块，按需加载
                    ],
                     function (ec) {
                         // 基于准备好的dom，初始化echarts图表
                         var myChart = ec.init(document.getElementById('auditChart'), "macarons");

                         var option = {
                             calculable: true,
                             title: {
                                 text: tsm.title,
                                 x: 'center',
                                 textStyle: {
                                     fontSize: 14,
                                     fontWeight: 'normal',
                                     color: '#333'
                                 }
                             },
                             tooltip: {
                                 trigger: 'axis'
                             },
                             toolbox: {
                                 show: false,
                                 feature: {
                                     mark: { show: true },
                                     dataView: { show: true, readOnly: false },
                                     magicType: { show: true, type: ['line', 'bar', 'stack', 'tiled'] },
                                     restore: { show: true },
                                     saveAsImage: { show: true }
                                 }
                             },
                             dataZoom: {
                                 show: false
                                 // start: 70
                             },
                             legend: {
                                 data: ['监测值(折线图)', '监测值(柱状图)'],
                                 y: 'bottom'
                             },
                             grid: {
                                 y2: 80
                             },
                             xAxis: [
                                 {
                                     type: 'category',
                                     data: tsm.category
                                 }
                             ],
                             yAxis: [
                                 {
                                     type: 'value'
                                 }
                             ],
                             series: tsm.seriesList
                         };
                         // 为echarts对象加载数据 
                         myChart.setOption(option);
                     }
            );
            }

            function NodeChecked(sender, args) {
                LoadingData();
            }

            //隐藏测点按钮
            function HidePanel(send, args) {
                $('#RAD_SLIDING_PANE_TAB_RadSlidingPane_Point').css("display", "none");
            }

            //显示测点按钮
            function ShowPanel(send, args) {
                $('#RAD_SLIDING_PANE_TAB_RadSlidingPane_Point').css("display", "block");

            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body scroll="no">
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSplitter runat="server" ID="RadSplitterChart" BorderSize="0" Width="100%" Height="100%" OnClientLoad="loadSplitter">
            <!-- 左侧测点、因子-->
            <telerik:RadPane ID="LeftPanel" runat="server" Width="22">
                <telerik:RadSlidingZone ID="RadSlidingZone1" runat="server" Width="22px" DockedPaneId="RadSlidingPane_Point">
                    <telerik:RadSlidingPane ID="RadSlidingPane_Point" Width="150" runat="server" Title="测点" UndockText="收缩" DockText="固定" CollapseText="关闭"
                        OnClientExpanding="HidePanel" OnClientBeforeDock="HidePanel" OnClientBeforeUndock="ShowPanel" OnClientCollapsed="ShowPanel"
                        EnableDock="true" MinWidth="200" MinHeight="225">
                        <telerik:RadTreeView ID="RadPortTree" runat="server" Width="100%" CheckBoxes="true" OnClientNodeChecked="NodeChecked"></telerik:RadTreeView>
                    </telerik:RadSlidingPane>
                </telerik:RadSlidingZone>
            </telerik:RadPane>
            <telerik:RadPane runat="server" ID="MiddlePane" Width="99%" Height="100%" BorderSize="0" Scrolling="None">
                <div id="auditChart" style="width: 100%; height: 100%">
                </div>
            </telerik:RadPane>
        </telerik:RadSplitter>
    </form>
</body>
</html>
