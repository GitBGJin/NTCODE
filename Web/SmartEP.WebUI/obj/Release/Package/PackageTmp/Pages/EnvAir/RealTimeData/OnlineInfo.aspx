<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OnlineInfo.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.RealTimeData.OnlineInfo" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<%@ Register TagPrefix="CbxRsm" TagName="FactorCbxRsm" Src="~/Controls/FactorCbxRsm.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
            <!--Step:2 Import echarts.js-->
            <!--Step:2 引入echarts.js-->
            <script src="../../../Resources/JavaScript/JQuery/jquery-1.10.2.js"></script>
            <script src="../../../Resources/JavaScript/echarts/echarts-2.2.7/build/dist/echarts.js"></script>
            <script type="text/javascript" language="javascript">
                var myRadGrid;
                function RadGridCreating(sender, eventArgs) { myRadGrid = sender; }

                //控制导出时按钮不会隐藏掉处理
                function onRequestStart(sender, args) {
                    if (args.EventArgument == 6 || args.EventArgument == 7 ||
                        args.get_eventTarget().indexOf("ExportToExcelButton") >= 0 ||
                            args.get_eventTarget().indexOf("ExportToWordButton") >= 0 ||
                            args.get_eventTarget().indexOf("ExportToCsvButton") >= 0) {
                        args.set_enableAjax(false);
                    }
                }

                $(function () {
                    ShowECharts();
                });

                function ShowECharts() {

                    /*var obj = str.parseJSON(); //由JSON字符串转换为JSON对象
                      或者 var obj = JSON.parse(str); //由JSON字符串转换为JSON对象
                    */
                    var windowHeight = document.body.clientHeight;//网页可见区域高 屏幕可用工作区高度：window.screen.availHeight 
                    var divDataEffectRateHeight = (windowHeight - 100).toString() + "px";
                    document.getElementById("divDataEffectRate").style.height = divDataEffectRateHeight;
                    var strEffectData = document.getElementById("<%=hdEffectData.ClientID%>").value;
                    var effectData = JSON.parse(strEffectData);
                    var onlineValue = [];//在线总数
                    var offOnlineValue = [];//离线总数
                    var totalValue = [];//总的在线总数
                    $.each(effectData, function (key, obj) {
                        if (obj.OnlineCount == 0 && obj.OfflineCount == 0) {
                            onlineValue.push(2);//给在线的默认值为2 否则会出现无任何效果的饼图
                            offOnlineValue.push(2);
                            totalValue.push(4);
                        }
                        else {
                            onlineValue.push(obj.OnlineCount);
                            offOnlineValue.push(obj.OfflineCount);
                            totalValue.push(obj.TotalCount);
                        }
                    });

                    // 路径配置
                    require.config({
                        paths: {
                            echarts: '../../../Resources/JavaScript/echarts/echarts-2.2.7/build/dist'

                        }
                    });

                    // 使用
                    require(
                        [
                            'echarts',
                            'echarts/chart/pie' // 使用柱状图就加载bar模块，按需加载
                        ],
                        function (ec) {
                            // 基于准备好的dom，初始化echarts图表
                            var myChart = ec.init(document.getElementById('divDataEffectRate'));
                            //单击事件
                            myChart.on('click', function (param) {

                                switch (param.dataIndex) {
                                    case 0: window.location.href = "RealTimeOnlineState.aspx?online=0"; break;//在线
                                    case 1: window.location.href = "RealTimeOnlineState.aspx?online=1"; break;//离线
                                }

                            });

                            option = {
                                title: {
                                    text: totalValue,
                                    subtext: '站点总数',
                                    x: 'center',
                                    y: 'center',
                                    itemGap: 20,
                                    textStyle: {
                                        color: 'rgba(30,144,255,0.8)',
                                        fontFamily: '微软雅黑',
                                        fontSize: 20,
                                        fontWeight: 'bolder'
                                    }
                                },
                                tooltip: {
                                    trigger: 'item',
                                    formatter: "点击详情"
                                },
                                legend: {
                                    show:false,
                                    orient: 'vertical',
                                    x: 'left',
                                    data: ['在线', '离线']
                                },
                                toolbox: {
                                    show: false,
                                    feature: {
                                        mark: { show: false },
                                        dataView: { show: false, readOnly: false },
                                        magicType: {
                                            show: false,
                                            type: ['pie', 'funnel'],
                                            option: {
                                                funnel: {
                                                    x: '25%',
                                                    width: '30%',
                                                    funnelAlign: 'center',
                                                    max: 1548
                                                }
                                            }
                                        },
                                        restore: { show: false },
                                        saveAsImage: { show: false }
                                    }
                                },
                                calculable: false,
                                series: [
                                    {
                                        name: '访问来源',
                                        type: 'pie',
                                        radius: ['30%', '50%'],
                                        itemStyle: {
                                            normal: {
                                                
                                                label: {
                                                    show: true,
                                                    formatter: '{b} : {c}个 ({d}%)'
                                                },
                                                labelLine: {
                                                    
                                                    show: true
                                                }
                                            },
                                            emphasis: {
                                                label: {
                                                    show: false,
                                                    position: 'center',
                                                    textStyle: {
                                                        fontSize: '30',
                                                        fontWeight: 'bold'
                                                    }
                                                }
                                            }
                                        },
                                        data: [
                                            {
                                                value: onlineValue, name: '在线'
                                            },
                                            
                                            {
                                                value: offOnlineValue, name: '离线', itemStyle: {
                                                    normal: { color: 'green' }
                                                }
                                            }

                                        ]
                                    }
                                ]
                            };

                            // 为echarts对象加载数据 
                            myChart.setOption(option);
                        }
                    );
                }
            </script>
        </telerik:RadScriptBlock>
        <input id="radGridColWidth" type="hidden" runat="server" value="120" />
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="hdEffectData" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
            <ClientEvents OnRequestStart="onRequestStart" />
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" BorderWidth="0"
            Width="100%">

            <telerik:RadPane ID="paneImage" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <!--Step:1 Prepare a dom for ECharts which (must) has size (width & hight)-->
                <!--Step:1 为ECharts准备一个具备大小（宽高）的Dom-->
                <div id="divDataEffectRate" style=" height:50%; width:50%;"></div>
                <asp:HiddenField ID="hdEffectData" runat="server" />
            </telerik:RadPane>
        </telerik:RadSplitter>
    </form>
</body>
</html>
