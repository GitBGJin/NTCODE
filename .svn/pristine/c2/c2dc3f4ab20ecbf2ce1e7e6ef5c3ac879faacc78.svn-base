<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AlarmInfoStatistic.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Dock.AlarmInfoStatistic" %>

<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script type="text/javascript">
                function GetData() {
                    var obj = new Object();
                    obj.beginTime = document.getElementById("<%=hdBeginTime.ClientID%>").value;
                    obj.pointid = document.getElementById("<%=hdpointiddata.ClientID%>").value;
                    obj.endTime = document.getElementById("<%=hdEndTime.ClientID%>").value;
                    return obj;
                }
            </script>
            <!--Step:2 引入echarts.js-->
            <script src="../../../Resources/JavaScript/JQuery/jquery-1.10.2.js"></script>
            <script src="../../../Resources/JavaScript/echarts/echarts-2.2.7/build/dist/echarts.js"></script>
            <script type="text/javascript">
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
                    var divDataEffectRateHeight = (windowHeight - 70).toString() + "px";
                    var divDataEffectRateWidth = (windowWidth).toString() + "px";
                    document.getElementById("container").style.height = divDataEffectRateHeight;
                    document.getElementById("container").style.width = divDataEffectRateWidth;
                    var strEffectData = document.getElementById("<%=hdSinglePollutant.ClientID%>").value;
                    var effectData = JSON.parse(strEffectData);
                    var Yvalue = [];
                    var dataarray = [];
                    $.each(effectData, function (key, obj) {
                        Yvalue.push(obj.AlarmType);
                    });
                    $.each(effectData, function (key, obj) {
                        dataarray.push(obj.AlarmTotal);
                    })
                    require.config({
                        paths: {
                            echarts: '../../../Resources/JavaScript/echarts/echarts-2.2.7/build/dist'
                        }
                    });
                    require(
                        [
                            'echarts',
                            'echarts/chart/bar',
                            'echarts/chart/line',
                            'echarts/chart/map'
                        ], function (ec) {
                            var myChart = ec.init(document.getElementById('container'));
                            myChart.setOption({
                                title: {
                                },
                                tooltip: {
                                    trigger: 'axis'
                                },
                                grid: {
                                    x: 60,
                                    y: 20,
                                    x2: 30,
                                    y2: 25,
                                },
                                toolbox: {
                                    show: true,
                                    feature: {
                                        mark: { show: false },//辅助线开关
                                        dataView: { show: false, readOnly: false },//数据视图
                                        magicType: { show: false, type: ['line', 'bar'] },
                                        restore: { show: false },//还原
                                        saveAsImage: { show: false }//保存为图片
                                    }
                                },
                                calculable: false,//是否启用拖拽重计算特性，默认关闭
                                xAxis: [
                            {

                                name: '条',
                                type: 'value'
                            }
                                ],
                                yAxis: [
                                    {
                                        type: 'category',
                                        data: Yvalue
                                    }
                                ],
                                series: [
                                       {
                                           type: 'bar',
                                           itemStyle: {
                                               normal: {
                                                   color: function (params) {
                                                       // build a color map as your need.
                                                       var colorList = [
                                                         '#FF5151', ' #FF8000', '#C07AB8', '#7D7DFF', '#27727B',
                                                          '#FE8463', '#9BCA63', '#FAD860', '#F3A43B', '#60C0DD',
                                                          '#D7504B', '#C6E579', '#F4E001', '#F0805A', '#26C0C0'
                                                       ];
                                                       return colorList[params.dataIndex]
                                                   },
                                                   label: {
                                                       show: true,
                                                       position: 'right',
                                                       formatter: '{c}'
                                                   }
                                               }
                                           },
                                           data: dataarray
                                       }
                                ]
                            })
                        }
                        );
                }
            </script>
        </telerik:RadCodeBlock>
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="pointCbxRsm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="pointCbxRsm" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdpointiddata" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="hdSinglePollutant" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="container" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdBeginTime" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdEndTime" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="dayBegin">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="dayBegin" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdBeginTime" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdEndTime" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="dayEnd">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="dayEnd" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdBeginTime" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdEndTime" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
            <ClientEvents OnRequestStart="onRequestStart" />
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" Width="100%"
            BorderWidth="0" BorderStyle="None" BorderSize="0">
            <telerik:RadPane ID="paneWhere" runat="server" Height="35px" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" class="Table_Customer">
                    <tr>
                        <td class="title" style="width: 100px;">测点:
                        </td>
                        <td class="content" style="width: 120px;">
                            <CbxRsm:PointCbxRsm runat="server" OnSelectedChanged="pointCbxRsm_SelectedChanged" ApplicationType="Air" CbxWidth="175" CbxHeight="350" MultiSelected="true" DropDownWidth="400" ID="pointCbxRsm"></CbxRsm:PointCbxRsm>
                        </td>
                        <td class="title" style="width: 50px;">日期:
                        </td>
                        <td class="content" style="width: 120px;">
                            <telerik:RadDatePicker ID="dayBegin" runat="server" MinDate="1900-01-01" OnSelectedDateChanged="dayBegin_SelectedDateChanged" AutoPostBack="true"
                                DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" Width="120"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定" />
                        </td>
                        <td class="title" style="width: 30px;">至
                        </td>
                        <td class="content" style="width: 120px;">
                            <telerik:RadDatePicker ID="dayEnd" runat="server" MinDate="1900-01-01" OnSelectedDateChanged="dayEnd_SelectedDateChanged" AutoPostBack="true"
                                DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" Width="120"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定" />
                        </td>
                        <td class="content">
                            <asp:ImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <%--<div id="dv" runat="server" style="width: 100%; height: 100%">
                    <div id="container" style="width: 100%; height: 500px">
                    </div>
                </div>--%>
                <div id="container" runat="server" style="height: 100%; border-width: 0px; border-style: none;"></div>
                <asp:HiddenField ID="hdSinglePollutant" runat="server" />
                <asp:HiddenField ID="hdpointiddata" runat="server" />
                <asp:HiddenField ID="hdBeginTime" runat="server" />
                <asp:HiddenField ID="hdEndTime" runat="server" />
            </telerik:RadPane>
        </telerik:RadSplitter>
    </form>
</body>
</html>
