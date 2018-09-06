<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OnlineInfoNew.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Dock.OnlineInfoNew" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<%@ Register TagPrefix="CbxRsm" TagName="FactorCbxRsm" Src="~/Controls/FactorCbxRsm.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="../../../Resources/JavaScript/FrameJS.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
            <script type="text/javascript">
                function GetData() {
                    var obj = new Object();
                    obj.PointNames = document.getElementById("<%=hdPointNames.ClientID%>").value;
                    obj.FactorName = document.getElementById("<%=hdFactorName.ClientID%>").value;

                    return obj;
                }
            </script>

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
                    var windowWidth = document.body.clientWidth;
                    var divDataEffectRateHeight = (windowHeight).toString() + "px";
                    var divRunStateInfoWidth = (windowWidth).toString() + "px";
                    document.getElementById("divDataAirOnlineInfo").style.height = divDataEffectRateHeight;
                    document.getElementById("divDataAirOnlineInfo").style.width = divRunStateInfoWidth;
                    var strEffectData = document.getElementById("<%=hdAirOnlineInfo.ClientID%>").value;
                    var effectData = JSON.parse(strEffectData);
                    var statusName = [];//获取在线状态名称
                    var statusCode = [];//获取在线状态的code
                    var totalValue = [];//总的数据

                    // 把Code和Name 对应起来放到数组中 先把总数计算出来
                    $.each(effectData, function (key, obj) {
                        if (obj.OnlineCount != null) {
                            statusCode.push("OnlineCount");
                            statusName.push("在线");
                        } if (obj.OfflineCount != null) {
                            statusCode.push("OfflineCount");
                            statusName.push("离线");
                        }
                        //if (obj.WarnCount != null) {
                        //    statusCode.push("WarnCount");
                        //    statusName.push("报警");
                        //} if (obj.FaultCount != null) {
                        //    statusCode.push("FaultCount");
                        //    statusName.push("故障");
                        //} if (obj.StopCount != null) {
                        //    statusCode.push("StopCount");
                        //    statusName.push("停运");
                        //} if (obj.AlwaysOnlineCount != null) {
                        //    statusCode.push("AlwaysOnlineCount");
                        //    statusName.push("始终在线");
                        //}

                        totalValue.push(obj.TotalCount);

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
                            var myChart = ec.init(document.getElementById('divDataAirOnlineInfo'));
                            //单击事件
                            //myChart.on('click', function (param) {

                            //    switch (param.dataIndex) {
                            //        case 0: OpenFineUIWindow("49502372-21c4-440b-9243-e71571712dba", "Pages/EnvAir/RealTimeData/RealTimeOnlineState.aspx?online=0", "实时在线状态信息"); break;//在线
                            //        case 1: OpenFineUIWindow("49502372-21c4-440b-9243-e71571712dba", "Pages/EnvAir/RealTimeData/RealTimeOnlineState.aspx?online=1", "实时在线状态信息"); break;//离线
                            //    }

                            //});

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
                                    formatter: "{a} <br/>{b} {d}% {c}个"
                                },
                                legend: {
                                    show: false,
                                    orient: 'vertical',
                                    x: 'left',
                                    //data: ['在线', '离线', '报警', '故障', '停运', '始终在线']
                                    data: statusName
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
                                                    x: '45%',
                                                    width: '80%',
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
                                series: (function () {
                                    //拼接series数据
                                    var series = [];
                                    series.push(
                                        {
                                            name: '实时在线信息',
                                            type: 'pie',
                                            radius: ['40%', '70%'],
                                            itemStyle: {
                                                normal: {

                                                    label: {
                                                        show: true,
                                                        formatter: '{b} {d}% {c}个'
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
                                            data: (function () {

                                                var seriesData = [];
                                                //添加data数组用于显示数据
                                                for (var i = 0; i < statusCode.length; i++) {
                                                    var objCode = statusCode[i];
                                                    var objName = statusName[i];
                                                    
                                                    $.each(effectData, function (key, obj) {

                                                        if (obj[objCode] != null) {
                                                            
                                                            if (objCode == "OnlineCount") {
                                                                seriesData.push(
                                                                        {
                                                                            value: parseInt(obj[objCode]),
                                                                            name: objName,
                                                                            itemStyle: {
                                                                                normal: { color: '#7DC734' }
                                                                            }
                                                                        }
                                                                    )
                                                            }
                                                            if (objCode == "OfflineCount") {
                                                                seriesData.push(
                                                                        {
                                                                            value: parseInt(obj[objCode]),
                                                                            name: objName,
                                                                            itemStyle: {
                                                                                normal: { color: '#EA7B82' }
                                                                            }
                                                                        }
                                                                    )
                                                            }
                                                        }
                                                    });
                                                }

                                                return seriesData;
                                            })()

                                            //data: [
                                            //    {
                                            //        value: onlineValue, name: '在线'
                                            //    },
                                            //    {
                                            //        value: offOnlineValue, name: '离线'
                                            //    }, {
                                            //        value: warnValue, name: '报警'
                                            //    }, {
                                            //        value: faultValue, name: '故障'
                                            //    }, {
                                            //        value: stopValue, name: '停运'
                                            //    }, {
                                            //        value: alwaysOnlineValue, name: '始终在线'
                                            //    }

                                            //]
                                        }

                                        );
                                    return series;
                                })()


                            };

                            // 为echarts对象加载数据 
                            myChart.setOption(option);
                        }
                    );

                    //详情
                    //$("#btnDetail").click(function () {
                    //    window.parent.location.href = "../RealTimeData/RealTimeOnlineState.aspx";
                    //});

                }
                //刷新
                //function RefreshWindow() {
                //    window.location.href = window.location.href;
                //}
            </script>
        </telerik:RadScriptBlock>
        <input id="radGridColWidth" type="hidden" runat="server" value="120" />
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="hdAirOnlineInfo" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdPointNames" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdFactorName" LoadingPanelID="RadAjaxLoadingPanel1" />

                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
            <ClientEvents OnRequestStart="onRequestStart" />
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" BorderWidth="0"
            Width="100%" BorderSize="0" BorderStyle="None">
            <%--            <telerik:RadPane ID="paneWhere" runat="server" Height="65px" Width="100%" Scrolling="None" MaxHeight="100"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="width: 100%; height: 100%" cellspacing="1" cellpadding="0" class="Table_Customer"
                    border="0">
                    <tr>
                        <td class="content" colspan="3">
                            <span style="float: right;">
                                <input id="btnRefresh" type="button" value="刷新" onclick="RefreshWindow(); return false;" />
                                <input id="btnDetail" type="button" value="详细" />
                            </span>
                            <span style="clear: both;"></span>
                        </td>
                    </tr>

                </table>
            </telerik:RadPane>--%>
            <telerik:RadPane ID="paneWhere" runat="server" Height="35px" Width="100%" Scrolling="None" MaxHeight="100"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="width: 100%; height: 100%" cellspacing="1" cellpadding="0" class="Table_Customer"
                    border="0">
                    <tr>
                        <td class="title" style="width: 10%; text-align: center;">区域：
                        </td>
                        <td class="content" style="width: 32%;">
                            <telerik:RadComboBox runat="server" ID="comboCityProper" Localization-CheckAllString="全选" Width="180px" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                            </telerik:RadComboBox>
                        </td>
                        <td class="content" style="text-align: center;">
                            <asp:ImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                        </td>

                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneImage" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <!--Step:1 Prepare a dom for ECharts which (must) has size (width & hight)-->
                <!--Step:1 为ECharts准备一个具备大小（宽高）的Dom-->
                <div id="divDataAirOnlineInfo" style="height: 90%; margin-top: -20px;"></div>
                <asp:HiddenField ID="hdAirOnlineInfo" runat="server" />

                <asp:HiddenField ID="hdPointNames" runat="server" />
                <asp:HiddenField ID="hdFactorName" runat="server" />
            </telerik:RadPane>
        </telerik:RadSplitter>
    </form>
</body>
</html>
