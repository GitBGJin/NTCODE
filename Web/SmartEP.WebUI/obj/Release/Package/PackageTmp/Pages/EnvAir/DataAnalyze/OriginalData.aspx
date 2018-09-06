<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OriginalData.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvWater.DataAnalyze.OriginalData" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<%@ Register TagPrefix="CbxRsm" TagName="FactorCbxRsm" Src="~/Controls/FactorCbxRsm.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel="stylesheet" href="../../../Resources/CSS/pagination.css" />
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

            <script src="../../../Resources/JavaScript/JQuery/jquery-1.9.0.js"></script>
            <script src="../../../Resources/JavaScript/ChartOperator/merge.js"></script>
            <script type="text/javascript">
                var isfirst = 1;//是否首次加载
                //var issecond = 1;
                function TabSelected(sender, args) {
                    try {
                        var isfirst = document.getElementById("<%=FirstLoadChart.ClientID%>");
                        var text = $.trim(args._tab._element.innerText);
                        if (text == '图表') {
                            if (isfirst.value == "1") {
                                isfirst.value = 0;
                                //GetChart();
                                InitGroupChart();
                            }
                        }
                        //if (text == '热力图') {
                        //    if (issecond.value == "1") {
                        //        issecond.value = 0;
                        //        GetHighChart();
                        //    }
                        //}
                    } catch (e) {
                        //InitTogetherChart();
                    }
                }
                //获取热力图
                //function GetHighChart() {
                //    var flags = document.getElementById("hdFlag").value;
                //    var staT = document.getElementById("hdstaT").value;
                //    var endT = document.getElementById("hdendT").value;
                //    var pointIds = document.getElementById("hdPoint").value;
                //    var factor = document.getElementById("hdFactor").value;
                //    $("#container").html("");
                //    var chartdiv = "";
                //    $.each(pointIds.split(','), function (chartNo, value) {
                //        chartdiv = "";
                //        chartdiv += '<div style=" width:100%; height:600px;">';
                //        chartdiv += '<iframe name="chartIframe" id="frame' + value + '" src="../Chart/HighChartFrame.aspx?pointIds=' + value + '&factors=' + factor + '&dtStart=' + staT + '&dtEnd=' + endT + '&flags='+ flags +'" frameborder="0" marginheight="0" marginwidth="0" width="100%" height="100%" scrolling="no"></iframe>';
                //        chartdiv += '</div>';
                //        $("#container").append(chartdiv);

                //    });
                //    $("#container").css("overflow-y", "auto");
                //    $("#container").height("650px");//设置图表Iframe的高度
                //    $("#container").width("100%");//设置图表Iframe的宽度

                //}

                //根据类型获取图表显示方式
                function GetChart() {
                    var showType = $("#ShowType").find("[checked]")[0].defaultValue;
                    if (showType == '合并')
                        InitTogetherChart();
                    else
                        InitGroupChart();
                }

                //画图
                function InitGroupChart() {
                    try {
                        var pointF = $("#HiddenPointFactor").val();
                        if (pointF == "point") {                //已测点画图
                            var hiddenData = $("#HiddenData").val().split('|');
                            var hdGroupFac = $("#hdGroupFac").val();
                            var hdGroupName = $("#hdGroupName").val();
                            var height = parseInt(parseInt($("#pvChart").css("height")) - 65);

                            groupChartByPointid(hiddenData[0], "../Chart/ChartFrame.aspx", height);

                        } else if (pointF == "factor") {        //以因子画图
                            var hiddenData = $("#HiddenData").val().split('|');
                            groupChart(hiddenData[1], "", "", "../Chart/ChartFrame.aspx", (parseInt($("#pvChart").css("height")) - 65));
                        }
                    } catch (e) {
                    }
                }

                /*分屏(按因子)*/
                function groupChartByRegion(regions, pointids, factors, names, url, iframeHeight) {
                    $("#ChartContainer").html("");
                    var chartdiv = "";
                    var height = iframeHeight;
                    //if (names.indexOf('|') != -1) {
                    var Names = names.split('|')
                    //}

                    $.each(regions.split(';'), function (chartNo, value) {
                        $.each(factors.split('|'), function (chartNoF, valueF) {
                            if (Names.length > 0) {
                                var name = Names[chartNoF];
                                chartdiv = "";
                                chartdiv += '<div style=" width:100%; height:' + height + 'px;">';
                                chartdiv += '<iframe name="chartIframe" id="frame' + value + chartNoF + '" src="' + url + '?Region=' + value + '&fac=' + valueF + '&name=' + name + ' " frameborder="0" marginheight="0" marginwidth="0" width="100%" height="100%" scrolling="no"></iframe>';
                                chartdiv += '</div>';
                                $("#ChartContainer").append(chartdiv);
                            }
                            else {
                                chartdiv = "";
                                chartdiv += '<div style=" width:100%; height:' + height + 'px;">';
                                chartdiv += '<iframe name="chartIframe" id="frame' + value + chartNoF + '" src="' + url + '?Region=' + value + '&fac=' + valueF + '" frameborder="0" marginheight="0" marginwidth="0" width="100%" height="100%" scrolling="no"></iframe>';
                                chartdiv += '</div>';
                                $("#ChartContainer").append(chartdiv);
                            }
                        });
                    });

                    $("#ChartContainer").css("overflow-y", "auto");
                    $("#ChartContainer").height(iframeHeight + 50 + "px");//设置图表Iframe的高度
                    $("#ChartContainer").width("100%");//设置图表Iframe的宽度
                    //$("#ChartContainer").html(chartdiv);


                    //$("#ChartContainer").html("");
                    //var chartdiv = "";
                    //var height = pointids.split(';').length == 1 ?iframeHeight:iframeHeight / 2;
                    //if (height == 0) height = 300;
                    //$.each(pointids.split(';'), function (chartNo, value) {
                    //    chartdiv += '<div style=" width:100%; height:' + height + 'px;">';
                    //    chartdiv += '<iframe name="chartIframe" id="pointid' + value + '" src="' + url + '?PointID=' + value + '&DataType='+(chartNo==0?1:0)+'" frameborder="0" marginheight="0" marginwidth="0" width="100%" height="100%" scrolling="no"></iframe>';
                    //    chartdiv += '</div>';
                    //});
                    //$("#ChartContainer").css("overflow-y", "auto");
                    //$("#ChartContainer").height((iframeHeight + 50) + "px");//设置图表Iframe的高度
                    //$("#ChartContainer").width("100%");//设置图表Iframe的宽度
                    //$("#ChartContainer").html(chartdiv);
                }
                //function InitGroupChart() {
                //    try {
                //        //var hiddenData = $("#HiddenData", window.parent.document).val().split('|');
                //        var hiddenData = $("#HiddenData").val().split('|');
                //        groupChart(hiddenData[1], "", "", "../Chart/ChartFrame.aspx", (parseInt($("#pvChart").css("height")) - 40));
                //    } catch (e) {
                //    }
                //}

                function InitTogetherChart() {
                    try {
                        //var bodyHeight = document.body.clientHeight;
                        //togetherChart("../Chart/ChartFrame.aspx", (parseInt(bodyHeight) - 112 - 40));

                        var hiddenData = $("#HiddenData").val().split('|');
                        var height = parseInt(parseInt($("#pvChart").css("height")) - 40);
                        groupChartByPointid(hiddenData[0], "../Chart/ChartFrame.aspx", height);//以站点分组
                    } catch (e) {
                    }
                }

                //Chart图形切换
                function ChartTypeChanged(item) {
                    try {
                        var chartIframe = document.getElementsByName('chartIframe');
                        //var item = args.get_item().get_value();
                        for (var i = 0; i < chartIframe.length; i++) {
                            document.getElementById(chartIframe[i].id).contentWindow.HighChartTypeChange(item);
                        }
                    } catch (e) {
                    }
                }

                //X轴切换
                function PointFactor(item) {
                    try {
                        InitGroupChart();
                    } catch (e) {
                    }
                }
                function OnClientClicking() {
                    var rbl = document.getElementsByName("radlDataType");
                    for (var i = 0; i < rbl.length; i++) {
                        if (rbl[i].checked && rbl[i].value != "Day") {
                            var hourB = $find("<%= dtpBegin.ClientID %>").get_selectedDate();
                            var hourE = $find("<%= dtpEnd.ClientID %>").get_selectedDate();
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
                        else {
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
                        if (rbl[i].checked && rbl[i].value != "Day") {
                            var hourB = $find("<%= dtpBegin.ClientID %>").get_selectedDate();
                        var hourE = $find("<%= dtpEnd.ClientID %>").get_selectedDate();
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
                    else {
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
            </script>
        </telerik:RadCodeBlock>
        <input id="radGridColWidth" type="hidden" runat="server" value="120" />
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <asp:HiddenField ID="FirstLoadChart" Value="1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1"
            UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="pointCbxRsm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="pointCbxRsm" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="factorCbxRsm" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <%--<telerik:AjaxUpdatedControl ControlID="IsStatistical" />--%>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridOriginal">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridOriginal" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="radlDataType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="radlDataType" />
                        <telerik:AjaxUpdatedControl ControlID="tbHour" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="tbDay" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <%--<telerik:AjaxUpdatedControl ControlID="IsStatistical" />--%>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridRTB">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridOriginal" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridOriginal" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenData" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenChartType" />
                        <telerik:AjaxUpdatedControl ControlID="FirstLoadChart" />
                        <telerik:AjaxUpdatedControl ControlID="hdGroupFac" />
                        <telerik:AjaxUpdatedControl ControlID="hdGroupName" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ShowType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ChartContainer" />
                        <telerik:AjaxUpdatedControl ControlID="ShowType" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ChartType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ChartType" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenChartType" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <%-- <telerik:AjaxSetting AjaxControlID="PointFactor">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ChartContainer" />
                    </UpdatedControls>
                </telerik:AjaxSetting>--%>
            </AjaxSettings>
            <ClientEvents OnRequestStart="onRequestStart" />
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" Width="100%"
            BorderWidth="0" BorderStyle="None" BorderSize="0">
            <telerik:RadPane ID="paneWhere" runat="server" Height="60px" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="width: 100%; height: 100%" cellspacing="1" cellpadding="0" class="Table_Customer"
                    border="0">
                    <tr>
                        <td class="title" style="width: 80px">工位号:
                        </td>
                        <td class="content" style="width: 180px;">
                            <CbxRsm:PointCbxRsm runat="server" ApplicationType="Water" DefaultAllSelected="true" CbxWidth="360" CbxHeight="350" MultiSelected="true" DropDownWidth="520" ID="pointCbxRsm" OnSelectedChanged="pointCbxRsm_SelectedChanged"></CbxRsm:PointCbxRsm>
                        </td>
                        <td class="title" style="width: 80px">监测因子:
                        </td>
                        <td class="content" style="width: 370px;">
                            <CbxRsm:FactorCbxRsm runat="server" ApplicationType="Water" DefaultAllSelected="true" CbxWidth="360" DropDownWidth="420" ID="factorCbxRsm"></CbxRsm:FactorCbxRsm>
                        </td>
                        <td class="content" align="left" style="width: 70px;" rowspan="2">
                            <asp:CheckBox ID="IsStatistical" Checked="false" Text="统计行" runat="server" Visible="false" />
                        </td>
                        <td class="content" align="left" rowspan="2">
                            <asp:ImageButton ID="btnSearch" runat="server" OnClientClick="return OnClientClicking()" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                        </td>
                    </tr>
                    <tr>
                        <td class="title" style="width: 80px; text-align: center;">数据类型:
                        </td>
                        <td class="content" style="width: 180px;">
                            <asp:RadioButtonList ID="radlDataType" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="radlDataType_SelectedIndexChanged">
                            </asp:RadioButtonList>
                        </td>
                        <td class="title" style="width: 80px; text-align: center;">开始时间:
                        </td>
                        <td class="content" style="width: 360px;">
                            <table id="tbHour" style="display: normal" runat="server">
                                <tr>
                                    <td>
                                        <telerik:RadDateTimePicker ID="dtpBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd HH:mm"
                                            DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                            Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                            Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                            Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                            TimeView-HeaderText="小时" />
                                    </td>
                                    <td>&nbsp;&nbsp;至&nbsp;&nbsp;
                                    </td>
                                    <td>
                                        <telerik:RadDateTimePicker ID="dtpEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd HH:mm"
                                            DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                            Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                            Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                            Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                            TimeView-HeaderText="小时" />
                                    </td>
                                </tr>
                            </table>
                            <table id="tbDay" style="display: none" runat="server">
                                <tr>
                                    <td>
                                        <telerik:RadDatePicker ID="dayBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                            DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd"
                                            Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                            Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                            Calendar-FastNavigationSettings-CancelButtonCaption="取消" />
                                    </td>
                                    <td>&nbsp;&nbsp;至&nbsp;&nbsp;
                                    </td>
                                    <td>
                                        <telerik:RadDatePicker ID="dayEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                            DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd"
                                            Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                            Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                            Calendar-FastNavigationSettings-CancelButtonCaption="取消" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneTab" runat="server" Scrolling="None" Width="100%" Height="26px"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadTabStrip ID="tabStrip" runat="server" SelectedIndex="0" MultiPageID="multiPage" OnClientTabSelecting="OnClientSelectedIndexChanging"
                    CssClass="RadTabStrip_Customer" OnClientTabSelected="TabSelected">
                    <Tabs>
                        <telerik:RadTab Text="列表">
                        </telerik:RadTab>
                        <telerik:RadTab Text="图表">
                        </telerik:RadTab>
                    </Tabs>
                </telerik:RadTabStrip>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadMultiPage ID="multiPage" runat="server" Width="100%" Height="100%" ScrollBars="Hidden" SelectedIndex="0">
                    <telerik:RadPageView ID="pvGrid" runat="server" Visible="true">
                        <div id="GridDiv" style="width: 100%; height: 100%">
                            <telerik:RadGrid ID="gridOriginal" runat="server" GridLines="None" Height="100%" Width="100%"
                                AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                                AutoGenerateColumns="true" AllowMultiRowSelection="false"
                                EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                                ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                                OnNeedDataSource="gridOriginal_NeedDataSource" OnItemDataBound="gridOriginal_ItemDataBound" OnColumnCreated="gridOriginal_ColumnCreated"
                                CssClass="RadGrid_Customer">
                                <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
                                <%--       <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="Top" IsFilterItemExpanded="False  EditMode="Batch""
                                    InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">--%>
                                <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="Top" IsFilterItemExpanded="False"
                                    InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true" RefreshText="查询" ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                                    <CommandItemTemplate>
                                        <div style="width: 100%; position: relative;">
                                            <telerik:RadToolBar BorderWidth="0" ID="gridRTB" SkinID="Query" CssClass="RadToolBar_Customer" AutoPostBack="true"
                                                runat="server" Width="50%" OnButtonClick="gridRTB_ButtonClick" />
                                        </div>
                                    </CommandItemTemplate>
                                    <Columns>
                                        <telerik:GridTemplateColumn HeaderText="序号" UniqueName="rowNum" HeaderStyle-Width="50px" HeaderStyle-Height="50px"
                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <%#Container.DataSetIndex + 1%>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                    <PagerStyle FirstPageToolTip="首页" AlwaysVisible="true" Position="Bottom" NextPageToolTip="下页" LastPageToolTip="尾页" PrevPageToolTip="上页"
                                        PageSizeLabelText="显示记录数:" PageSizes="24 48 96" PagerTextFormat="跳转页面:{4}&amp;nbsp;显示 &lt;strong&gt;{2}&lt;/strong&gt; - &lt;strong&gt;{3}&lt;/strong&gt;条,共 &lt;strong&gt;{5}&lt;/strong&gt;条"></PagerStyle>
                                </MasterTableView>
                                <CommandItemStyle Width="100%" />
                                <ClientSettings>
                                    <Scrolling AllowScroll="True" EnableVirtualScrollPaging="false" UseStaticHeaders="True" FrozenColumnsCount="3"
                                        SaveScrollPosition="true"></Scrolling>
                                </ClientSettings>
                            </telerik:RadGrid>

                        </div>
                    </telerik:RadPageView>
                    <telerik:RadPageView ID="pvChart" runat="server" Visible="true">
                        <div style="padding-top: 6px;">
                            <div style="float: left; display: none;">
                                <asp:RadioButtonList ID="ShowType" runat="server" AutoPostBack="true" RepeatDirection="Horizontal" OnSelectedIndexChanged="ShowType_SelectedIndexChanged">
                                    <asp:ListItem Text="合并" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="分屏"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            <div style="float: left;">
                                <asp:RadioButtonList runat="server" ID="PointFactor" AutoPostBack="true" Visible="true" RepeatDirection="Vertical" RepeatColumns="2" OnSelectedIndexChanged="PointFactor_SelectedIndexChanged">
                                    <asp:ListItem Text="按因子分类" Value="factor"></asp:ListItem>
                                    <asp:ListItem Text="按工位号分类" Value="point" Selected="True"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            <div style="float: right;">
                                <asp:RadioButtonList runat="server" ID="ChartType" AutoPostBack="true" RepeatDirection="Horizontal" RepeatColumns="3" OnSelectedIndexChanged="ChartType_SelectedIndexChanged">
                                    <asp:ListItem Text="折线图" Value="spline" Selected="True"></asp:ListItem>
                                    <%--<asp:ListItem Text="柱形图" Value="column"></asp:ListItem>--%>
                                    <%--<asp:ListItem Text="点图" Value="scatter"></asp:ListItem>--%>
                                </asp:RadioButtonList>
                            </div>
                            <div style="clear: both">
                            </div>
                        </div>

                        <div id="ChartContainer">
                        </div>
                    </telerik:RadPageView>
                </telerik:RadMultiPage>

            </telerik:RadPane>
        </telerik:RadSplitter>
        <%--  隐藏域存放选中的因子，在Grid刷新后更新隐藏域数据--%>
        <asp:HiddenField ID="HiddenData" runat="server" Value="" />
        <asp:HiddenField ID="AjaxURL" runat="server" Value="../ChartAjaxRequest/AuditData.ashx" />
        <asp:HiddenField ID="HiddenChartType" runat="server" Value="spline" />
        <asp:HiddenField ID="HiddenPointFactor" runat="server" Value="point" />
        <asp:HiddenField ID="hdGroupFac" runat="server" Value="0" />
        <asp:HiddenField ID="hdGroupName" runat="server" Value="0" />
    </form>
</body>
</html>
