﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PolaryWindDirection.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.DataAnalyze.PolaryWindDirection" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link type="text/css" rel="stylesheet" href="../../../App_Themes/Neptune/Telerik.Web.UI.Skins/Metro/RadSplitter.css" />
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript" src="../../../Resources/JavaScript/JQuery/jquery-1.9.0.js"></script>
        <script src="../../../Resources/JavaScript/Highcharts/highcharts.js"></script>
        <script src="../../../Resources/JavaScript/ChartOperator/ShowChart.js"></script>
        <script type="text/javascript" src="../../../Resources/JavaScript/ChartOperator/merge.js"></script>
        <%--<script type="text/javascript" src="../../../Resources/JavaScript/Echarts/build/dist/echarts.js"></script>--%>
        <script src="../../../Resources/JavaScript/Highcharts/highcharts-more.js"></script>
        <script src="../../../Resources/JavaScript/HighCharts/exporting.js"></script>
        <script type="text/javascript">
            function SpliterLoaded(sender) {
                //var bodyWidth = document.body.clientWidth;
                //var bodyHeight = document.body.clientHeight;
                //sender.set_width(bodyWidth);//初始化Splitter高度及宽度
                //sender.set_height(bodyHeight);
            }

            function AjaxLoadingPolary(PointID, FactorCode, dtBegin, dtEnd, radlDataType, WindDir, PolaryType, pointId) {
                //$('#Container').css("height", 500);                
                $('#Container').html("");
                var chartdiv = "";
                chartdiv = "";
                chartdiv += '<div style=" width:100%; height:600px;">';
                chartdiv += '<iframe name="chartIframe" id="frame' + Math.random() + '" src="../Chart/PolaryChart.aspx?pointIds=' + PointID + '&factor=' + FactorCode + '&WindDir=' + WindDir + '&PolaryType=' + PolaryType + '&radlDataType=' + radlDataType + '&dtBegin=' + dtBegin + '&dtEnd=' + dtEnd + '&flag=' + pointId + '"  frameborder="0" marginheight="0" marginwidth="0" width="100%" height="90%" scrolling="no"></iframe>';
                chartdiv += '</div>';
                $("#Container").append(chartdiv);

                //$("#Container").css("overflow-y", "auto");
                $("#Container").height("500px");//设置图表Iframe的高度
                $("#Container").width("100%");//设置图表Iframe的宽度

            }

            //function InitChart(tsm) {
            //    // 路径配置
            //    require.config({
            //        paths: {
            //            echarts: '../../../Resources/JavaScript/Echarts/build/dist'
            //        }
            //    });

            //    // 使用
            //    require(
            //        [
            //            'echarts',
            //            'echarts/chart/line', // 使用柱状图就加载bar模块，按需加载
            //              'echarts/chart/radar' // 雷达图
            //        ],
            //        function (ec) {
            //            // 基于准备好的dom，初始化echarts图表
            //            var myChart = ec.init(document.getElementById('Container'), "macarons");

            //            var option = {
            //                title: {
            //                    text: tsm.titleText,
            //                    x: 'center',
            //                    y: 'bottom'
            //                    //subtext: '纯属虚构'
            //                },
            //                tooltip: {
            //                    trigger: 'axis',
            //                    formatter: function (params) {
            //                        var series = params[0];
            //                        if (tsm.legendShow != undefined && tsm.legendShow == true)
            //                            return series.name + '<br/>' + series.indicator + ':' + (series.value == '' ? '--' : series.value);
            //                        else
            //                            return series.indicator + ':' + (series.value == '' ? '--' : series.value);
            //                    }
            //                },

            //                legend: {
            //                    x: 'center',
            //                    enabled: false,
            //                    data: tsm.legend,
            //                    show: tsm.legendShow
            //                },
            //                toolbox: {
            //                    show: true,
            //                    y: 'bottom',
            //                    feature: {
            //                        mark: { show: true },
            //                        dataView: { show: true, readOnly: false },
            //                        restore: { show: true },
            //                        saveAsImage: { show: true }
            //                    }
            //                },
            //                calculable: true,
            //                polar: [
            //                    {
            //                        indicator: tsm.indicator,
            //                        //indicator:[{ text: '北风', max: 100 },{ text: '东北风', max: 100 },{ text: '东风', max: 100 },{ text: '东南风', max: 100 },{ text: '南风', max: 100 },{ text: '西南风', max: 100 },{ text: '西风', max: 100 },{ text: '西北风', max: 100 }],
            //                        radius: 140
            //                    }
            //                ],
            //                series: [
            //                    {
            //                        type: 'radar',
            //                        itemStyle: { normal: { areaStyle: { type: 'default' } } },
            //                        data: tsm.data
            //                        //data: [{ name: 'a01008', value: [155,6,3,4,5,6,7,8] }]
            //                    }
            //                ]

            //            };
            //            // 为echarts对象加载数据 
            //            myChart.setOption(option);
            //        }
            //);
            //}

            function OnClientClicking() {
                var rbl = document.getElementsByName("radlDataType");
                for (var i = 0; i < rbl.length; i++) {
                    if (rbl[i].checked && rbl[i].value == "Hour") {
                        var hourB = $find("<%= hourBegin.ClientID %>").get_selectedDate();
                        var hourE = $find("<%= hourEnd.ClientID %>").get_selectedDate();
                        if ((hourB == null) || (hourE == null)) {
                            alert("开始时间或者终止时间，不能为空！");
                            return false;
                        }
                        if (hourB > hourE) {
                            alert("开始时间不能大于终止时间！");
                            return false;
                        } else {
                            return true;
                        }
                    }
                    else if (rbl[i].checked && rbl[i].value == "Day") {
                        var dayB = $find("<%= dayBegin.ClientID %>").get_selectedDate();
                        var dayE = $find("<%= dayEnd.ClientID %>").get_selectedDate();
                        if ((dayB == null) || (dayE == null)) {
                            alert("开始时间或者终止时间，不能为空！");
                            return false;
                        }
                        if (dayB > dayE) {
                            alert("开始时间不能大于终止时间！");
                            return false;
                        } else {
                            return true;
                        }
                    }
            }
        }

        //tab页切换时时间检查
        function OnClientSelectedIndexChanging(sender, args) {
            var rbl = document.getElementsByName("radlDataType");
            for (var i = 0; i < rbl.length; i++) {
                if (rbl[i].checked && rbl[i].value == "Hour") {
                    var hourB = $find("<%= hourBegin.ClientID %>").get_selectedDate();
                    var hourE = $find("<%= hourEnd.ClientID %>").get_selectedDate();
                    if ((hourB == null) || (hourE == null)) {
                        alert("开始时间或者终止时间，不能为空！");
                        return false;
                    }
                    if (hourB > hourE) {
                        alert("开始时间不能大于终止时间！");
                        return false;
                    } else {
                        return true;
                    }
                }
                else if (rbl[i].checked && rbl[i].value == "Day") {
                    var dayB = $find("<%= dayBegin.ClientID %>").get_selectedDate();
                    var dayE = $find("<%= dayEnd.ClientID %>").get_selectedDate();
                    if ((dayB == null) || (dayE == null)) {
                        alert("开始时间或者终止时间，不能为空！");
                        return false;
                    }
                    if (dayB > dayE) {
                        alert("开始时间不能大于终止时间！");
                        return false;
                    } else {
                        return true;
                    }
                }
        }
    }

    function onRequestStart(sender, args) {
        if (args.EventArgument == "")
            return;
        if (args.EventArgument == 0 || args.EventArgument == 1 ||
            args.get_eventTarget().indexOf("ExportToExcelButton") >= 0 ||
                args.get_eventTarget().indexOf("ExportToWordButton") >= 0 ||
                args.get_eventTarget().indexOf("ExportToCsvButton") >= 0) {
            args.set_enableAjax(false);
        }
    }
    function onRequestEnd(sender, args) {
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
                <telerik:AjaxSetting AjaxControlID="pointCbxRsm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="pointCbxRsm" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="pointForWind">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="pointForWind" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="radlDataType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="radlDataType" />
                        <telerik:AjaxUpdatedControl ControlID="dbtHour" />
                        <telerik:AjaxUpdatedControl ControlID="dbtDay" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="Container" />
                        <telerik:AjaxUpdatedControl ControlID="polaryGrid" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="polaryGrid">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="Container" />
                        <telerik:AjaxUpdatedControl ControlID="polaryGrid" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
            <ClientEvents OnRequestStart="onRequestStart" OnResponseEnd="onRequestEnd" />
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" Width="100%" OnClientLoaded="SpliterLoaded"
            BorderWidth="0" BorderStyle="None" BorderSize="0">
            <telerik:RadPane ID="paneWhere" runat="server" Height="60px" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="width: 100%; height: 100%" cellspacing="1" cellpadding="0" class="Table_Customer"
                    border="0">
                    <tr>
                        <td class="title" style="width: 80px">因子站点:
                        </td>
                        <td class="content" style="width: 400px;">
                            <table>
                                <tr>
                                    <td>
                                        <CbxRsm:PointCbxRsm runat="server" ApplicationType="Air" CbxWidth="160" CbxHeight="350" MultiSelected="true" DropDownWidth="520" ID="pointCbxRsm"></CbxRsm:PointCbxRsm>
                                    </td>
                                    <td class="title" style="width: 80px">气向站点:
                                    </td>
                                    <td>
                                        <CbxRsm:PointCbxRsm runat="server" ApplicationType="Air" CbxWidth="160" CbxHeight="350" MultiSelected="false" DropDownWidth="520" ID="pointForWind"></CbxRsm:PointCbxRsm>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td class="title" style="width: 80px">风向:
                        </td>
                        <td class="content" style="width: 400px;">
                            <table>
                                <tr>
                                    <td>
                                        <asp:RadioButtonList ID="WindDirRadioButton" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="radlDataType_SelectedIndexChanged" AutoPostBack="false">
                                        </asp:RadioButtonList></td>
                                    <td id="tdTitle" style="width: 60px; text-align: right;" runat="server">因子</td>
                                    <td id="tdContent" runat="server">
                                        <telerik:RadComboBox ID="factorCom" runat="server" Width="100" SkinID="Default" Skin="Default" CheckBoxes="true" Localization-CheckAllString="全选" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                                            <Items>
                                                <telerik:RadComboBoxItem runat="server" Text="SO2" Value="a21026" Checked="false" />
                                                <telerik:RadComboBoxItem runat="server" Text="NO2" Value="a21004" Checked="false" />
                                                <telerik:RadComboBoxItem runat="server" Text="O3" Value="a05024" Checked="false" />
                                                <telerik:RadComboBoxItem runat="server" Text="CO" Value="a21005" Checked="false" />
                                                <telerik:RadComboBoxItem runat="server" Text="PM2.5" Value="a34004" Checked="true" />
                                                <telerik:RadComboBoxItem runat="server" Text="PM10" Value="a34002" Checked="false" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </td>
                                    <%--<td id="td1" style="width: 60px; text-align: right;" runat="server">风向点位</td>
                                    <td id="td2" runat="server">
                                        <telerik:RadComboBox ID="radWindPoint" runat="server" Width="80" SkinID="Default" Skin="Default" CheckBoxes="false" Localization-CheckAllString="全选" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                                        </telerik:RadComboBox>
                                    </td>--%>
                                </tr>
                            </table>
                        </td>

                        <td class="content" align="left" rowspan="3">
                            <asp:ImageButton ID="btnSearch" runat="server" OnClientClick="return OnClientClicking()" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                        </td>
                    </tr>
                    <tr>
                        <td class="title" style="width: 80px; text-align: center;">数据类型:
                        </td>
                        <td class="content" style="width: 400px;">
                            <asp:RadioButtonList ID="radlDataType" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="radlDataType_SelectedIndexChanged" AutoPostBack="true">
                            </asp:RadioButtonList>
                        </td>
                        <td class="title" style="width: 80px; text-align: center;">时间:
                        </td>
                        <td class="content" style="width: 400px;">
                            <div runat="server" id="dbtHour">
                                <telerik:RadDateTimePicker ID="hourBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                    DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd HH:00"
                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                    Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                    TimeView-HeaderText="小时" Calendar-FastNavigationStep="12" />
                                &nbsp;&nbsp;至&nbsp;&nbsp;
                            <telerik:RadDateTimePicker ID="hourEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd HH:00"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                TimeView-HeaderText="小时"  Calendar-FastNavigationStep="12" />
                            </div>
                            <div runat="server" id="dbtDay">
                                <telerik:RadDatePicker ID="dayBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd"
                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                    Calendar-FastNavigationSettings-OkButtonCaption="确定" Calendar-FastNavigationStep="12" 
                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" />
                                &nbsp;&nbsp;至&nbsp;&nbsp;
                            <telerik:RadDatePicker ID="dayEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定" Calendar-FastNavigationStep="12" 
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消" />
                            </div>
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadMultiPage ID="multiPage" runat="server" Width="100%" Height="100%" ScrollBars="Hidden" SelectedIndex="0">
                    <telerik:RadPageView ID="grvGrid" runat="server" Visible="true">
                        <div style="text-align: center;">
                            <div style="float: left;">
                                <div style="padding-top: 20px; padding-left: 60px;">
                                    <div id="Container" style="height: 500px; min-width: 500px;">
                                    </div>
                                </div>
                            </div>
                            <div style="float: left;">
                                <div style="padding-top: 60px;">
                                    <telerik:RadGrid ID="polaryGrid" runat="server" GridLines="None" Width="450"
                                        AllowPaging="true" PageSize="12" Height="350"
                                        AutoGenerateColumns="false" AllowMultiRowSelection="false"
                                        EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                                        ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0" ShowFooter="false"
                                        CssClass="RadGrid_Customer" OnNeedDataSource="polaryGrid_NeedDataSource" OnLoad="polaryGrid_Load" OnItemDataBound="polaryGrid_ItemDataBound">
                                        <PagerStyle FirstPageToolTip="首页" AlwaysVisible="true" Position="Bottom" NextPageToolTip="下页" LastPageToolTip="尾页" PrevPageToolTip="上页"
                                            PageSizeLabelText="显示记录数:" PageSizes="12" PagerTextFormat="跳转页面:{4}&amp;nbsp;显示 &lt;strong&gt;{2}&lt;/strong&gt; - &lt;strong&gt;{3}&lt;/strong&gt;条,共 &lt;strong&gt;{5}&lt;/strong&gt;条"></PagerStyle>
                                        <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
                                        <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False"
                                            InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据" EditMode="Batch" HeaderStyle-HorizontalAlign="Center">
                                            <BatchEditingSettings EditType="Cell" OpenEditingEvent="DblClick" />
                                        </MasterTableView>
                                        <CommandItemStyle Width="100%" />
                                        <ClientSettings>
                                            <Scrolling AllowScroll="True" EnableVirtualScrollPaging="false" UseStaticHeaders="True"
                                                SaveScrollPosition="true"></Scrolling>
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </div>
                            </div>
                            <div style="clear: both;">
                            </div>
                        </div>
                    </telerik:RadPageView>
                </telerik:RadMultiPage>
            </telerik:RadPane>
        </telerik:RadSplitter>
    </form>
</body>
</html>
