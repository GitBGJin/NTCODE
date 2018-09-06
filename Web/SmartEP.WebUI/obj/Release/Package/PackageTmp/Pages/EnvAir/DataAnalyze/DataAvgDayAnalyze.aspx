<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DataAvgDayAnalyze.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.DataAnalyze.DataAvgDayAnalyze" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<%@ Register TagPrefix="CbxRsm" TagName="FactorCbxRsm" Src="~/Controls/FactorCbxRsm.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script type="text/javascript">
    
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
            <script src="../../../Resources/JavaScript/JQuery/jquery-1.9.0.js"></script>
            <script type="text/javascript">
                function GetData() {
                    var obj = new Object();
                    obj.pointIds = document.getElementById("<%=hdPointIds.ClientID%>").value;
                    obj.dtStart = document.getElementById("<%=hdStartdt.ClientID%>").value;
                    obj.dtEnd = document.getElementById("<%=hdEnddt.ClientID%>").value;
                    obj.pointType = document.getElementById("<%=hdPointType.ClientID%>").value;
                    return obj;
                }
                var isfirst = 1;//是否首次加载
                function TabSelected(sender, args) {
                    try {
                        var isfirst = document.getElementById("<%=FirstLoadChart.ClientID%>");
                        var text = $.trim(args._tab._element.innerText);
                        if (text == '图表') {
                            if (isfirst.value == "1") {
                                isfirst.value = 0;
                                SearchData();
                            }
                            $("#DataType").hide();
                            $("#DataTypeList").hide();
                        }
                        else {
                            $("#DataType").show();
                            $("#DataTypeList").show();
                        }
                    } catch (e) {
                        //InitTogetherChart();
                    }
                }
                //图形类型变化隐藏折线柱形
                function ChartContent_change() {

                }
                function PointFactor() {
                    try {
                        SearchData();
                    } catch (e) {
                    }
                }
                function SearchData() {
                    
                    $('#container').html("");
                    //var buttonSearch = document.getElementById("<%=btnSearch.ClientID%>");
                    // buttonSearch.click();
                    var pointIds = $("#hdPointIds").val();
                    var dtBegion = $("#hdStartdt").val();
                    var dtEnd = $("#hdEnddt").val();
                    var pointType = $("#hdPointType").val();
                    var factors = $("#hdFactors").val();
                    var flag = $("#hdFlag").val();

                    var chartdiv = "";
                    if (flag == "0") {
                        chartdiv += '<div style=" width:100%; height:600px;">';
                        chartdiv += '<iframe name="chartIframe" id="frame' + Math.random() + '" src="DataAvgDayCharts.aspx?pointIds=' + pointIds + '&factors=' + factors + '&Type=' + pointType + '&dtStart=' + dtBegion + '&dtEnd=' + dtEnd + '"  frameborder="0" marginheight="0" marginwidth="0" width="100%" height="95%" scrolling="no"></iframe>';
                        chartdiv += '</div>';
                        $("#container").append(chartdiv);

                        $("#container").css("overflow-y", "auto");
                        $("#container").height("100%");//设置图表Iframe的高度
                        $("#container").width("100%");//设置图表Iframe的宽度
                    }
                    else {
                        $.each(factors.split(','), function (chartNo, value) {
                            chartdiv = "";
                            chartdiv += '<div style=" width:100%; height:600px;">';
                            chartdiv += '<iframe name="chartIframe" id="frame' + chartNo + '" src="DataAvgDayCharts.aspx?pointIds=' + pointIds + '&factors=' + value + '&Type=' + pointType + '&dtStart=' + dtBegion + '&dtEnd=' + dtEnd + '&flag=' + flag + '"  frameborder="0" marginheight="0" marginwidth="0" width="100%" height="95%" scrolling="no"></iframe>';
                            chartdiv += '</div>';
                            $("#container").append(chartdiv);
                        });
                        $("#container").css("overflow-y", "auto");
                        $("#container").height("100%");//设置图表Iframe的高度
                        $("#container").width("100%");//设置图表Iframe的宽度
                    }                   
                }
                //控制导出时按钮不会隐藏掉处理
                function onRequestStart(sender, args) {
                    if (args.EventArgument == 6 || args.EventArgument == 7 ||
                        args.get_eventTarget().indexOf("ExportToExcelButton") >= 0 ||
                            args.get_eventTarget().indexOf("ExportToWordButton") >= 0 ||
                            args.get_eventTarget().indexOf("ExportToCsvButton") >= 0) {
                        args.set_enableAjax(false);
                    }
                }
                function OnClientClicking() {
                    //var childSpares = window.frames["pvChart"].children[0].contentWindow;
                    //if (childSpares.SaveData != undefined) {
                    //    childSpares.SaveData();
                    //}
                    date1 = $find("<%= rdpBegin.ClientID %>").get_selectedDate();
                    date2 = $find("<%= rdpEnd.ClientID %>").get_selectedDate();
                    if ((date1 == null) || (date2 == null)) {
                        alert("开始时间或者终止时间，不能为空！");
                        //sender.set_autoPostBack(false);
                        return false;
                    }
                    if (date1 > date2) {
                        alert("开始时间不能大于终止时间！");
                        return false;
                    } else {
                        return true;
                    }
                }
                function ClientButtonClicking(sender, args) {
                    var uri = "DataAvgDayAnalyzeWindow.aspx";
                    var oWindow = window.radopen(encodeURI(uri), "ConfigOfflineDialog");
                }
                //tab页切换时时间检查
                function OnClientSelectedIndexChanging(sender, args) {
                    var date1 = new Date();
                    var date2 = new Date();
                    date1 = $find("<%= rdpBegin.ClientID %>").get_selectedDate();
                    date2 = $find("<%= rdpEnd.ClientID %>").get_selectedDate();
                    if ((date1 == null) || (date2 == null)) {
                        alert("开始时间或者终止时间，不能为空！");
                        args.set_cancel(true);
                        return;
                    }
                    if (date1 > date2) {
                        alert("开始时间不能大于终止时间！");
                        args.set_cancel(true);
                        return;
                    } else {
                        return;
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
        </telerik:RadScriptBlock>
        <asp:HiddenField ID="FirstLoadChart" Value="1" runat="server" />
        <input id="radGridColWidth" type="hidden" runat="server" value="120" />
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <asp:HiddenField ID="HiddenField1" Value="1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="grdAvgDay">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdAvgDay" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridRTB">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdAvgDay" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="splitter">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdAvgDay" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch" EventName="Click">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdAvgDay" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="FirstLoadChart" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dv" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdPointIds" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdStartdt" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdEnddt" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdPointType" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdFactors" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdFlag" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <%--<telerik:AjaxUpdatedControl ControlID="iframeOCM" LoadingPanelID="RadAjaxLoadingPanel1" />--%>
                        <telerik:AjaxUpdatedControl ControlID="container" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ChartContent">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ChartContent" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdChartType" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdFlag" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="container" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
            <ClientEvents OnRequestStart="onRequestStart" />
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" BorderWidth="0"
            Width="100%">
            <telerik:RadPane ID="paneWhere" runat="server" Height="60px" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="height: 100%" cellspacing="1" cellpadding="0" class="Table_Customer"
                    border="0">
                    <tr>
                        <td class="title" style="width: 80px; text-align: center;">查询类型:
                        </td>
                        <td class="content" style="width: 240px;">
                            <asp:RadioButtonList ID="rbtnlType" runat="server" AutoPostBack="true" RepeatLayout="Flow" RepeatColumns="5">
                                <asp:ListItem Text="区域" Value="CityProper"></asp:ListItem>
                                <asp:ListItem Text="测点" Value="Port" Selected="True"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td class="title" style="width: 120px; text-align: center;">站点:
                        </td>
                        <td class="content" style="width: 300px;">
                            <%--<div runat="server" id="dvCity">
                                <telerik:RadComboBox runat="server" ID="comboCity" Width="180px" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput"
                                    Localization-CheckAllString="选择全部">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="市辖区" Value="b6e983c4-4f92-4be3-bbac-d9b71c470640" Checked="true" />
                                        <telerik:RadComboBoxItem Text="崇川区" Value="be0839ef-2059-437a-aeb3-84586cdc24c6" Checked="true" />
                                        <telerik:RadComboBoxItem Text="港闸区" Value="c1008828-0413-4762-9976-87134de58453" Checked="true" />
                                        <telerik:RadComboBoxItem Text="开发区" Value="62cf6841-187e-4ec2-a139-90aef3547d06" Checked="true" />
                                        <telerik:RadComboBoxItem Text="海安县" Value="6e66a670-f866-4bb9-abe1-317702d333dc" Checked="true" />
                                        <telerik:RadComboBoxItem Text="海门市" Value="0ea68e7c-5f43-4d59-b913-d8392a8457a4" Checked="true" />
                                        <telerik:RadComboBoxItem Text="启东市" Value="6305492a-7145-4f54-b88d-031be0d62282" Checked="true" />
                                        <telerik:RadComboBoxItem Text="如东市" Value="afc66edb-0a2a-493c-b340-d84249cd9afd" Checked="true" />
                                        <telerik:RadComboBoxItem Text="如皋市" Value="52d96e0a-728a-4973-bbf5-1b33367687fd" Checked="true" />
                                        <telerik:RadComboBoxItem Text="通州区" Value="fcc5ab39-41cc-4329-ac72-248ccbcd8b29" Checked="true" />
                                        <telerik:RadComboBoxItem Text="通州湾示范区" Value="9ac74b62-25ae-4b32-9ea9-14baed98f2b2" Checked="true" />
                                    </Items>
                                </telerik:RadComboBox>
                            </div>--%>
                            <div runat="server" id="dvPoint">
                                <CbxRsm:PointCbxRsm runat="server" DefaultIPointMode="Region" ApplicationType="Air" CbxWidth="220" CbxHeight="350" MultiSelected="true" DropDownWidth="520" ID="pointCbxRsm"></CbxRsm:PointCbxRsm>
                            </div>
                        </td>
                        <td class="title" style="width: 80px; text-align: center;">监测因子:
                        </td>
                        <td class="content" style="width: 10%; text-align: center;">
                            <telerik:RadComboBox runat="server" ID="rcbfactor" Width="180px" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput"
                                Localization-CheckAllString="选择全部">
                                <Items>
                                    <telerik:RadComboBoxItem Text="PM2.5" Value="PM25" Checked="true" />
                                    <telerik:RadComboBoxItem Text="PM10" Value="PM10" Checked="true" />
                                    <telerik:RadComboBoxItem Text="NO2" Value="NO2" Checked="true" />
                                    <telerik:RadComboBoxItem Text="SO2" Value="SO2" Checked="true" />
                                    <telerik:RadComboBoxItem Text="CO" Value="CO" Checked="true" />
                                    <telerik:RadComboBoxItem Text="O3_8h" Value="O3_8h" Checked="true" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                        <td class="content" style="text-align: center;" rowspan="2">
                            <asp:ImageButton ID="btnSearch" runat="server" OnClientClick="return OnClientClicking()" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                        </td>
                    </tr>
                    <tr>
                        <td class="title" style="width: 80px; text-align: center;">开始时间:
                        </td>
                        <td class="content" style="width: 240px;">
                            <telerik:RadDatePicker ID="rdpBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px" Width="105px"
                                DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-CancelButtonCaption="取消" Calendar-FastNavigationStep="12" />
                        </td>
                        <td class="title" style="width: 80px; text-align: center;">结束时间:
                        </td>
                        <td class="content" style="width: 300px;">
                            <telerik:RadDatePicker ID="rdpEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd"
                                DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" Width="105px"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-CancelButtonCaption="取消" Calendar-FastNavigationStep="12" />
                        </td>
                        <td class="title" style="width: 80px; text-align: center;" id="DataType">数据类型:
                        </td>
                        <td class="content" style="width: 10%; text-align: center;" id="DataTypeList">
                            <telerik:RadComboBox runat="server" ID="cbType" Width="180px" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput" Localization-CheckAllString="选择全部" EmptyMessage="请选择">
                            </telerik:RadComboBox>
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
                        <telerik:RadGrid ID="grdAvgDay" runat="server" GridLines="None" Height="100%" Width="100%"
                            AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                            AutoGenerateColumns="true" AllowMultiRowSelection="false"
                            EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                            ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                            OnNeedDataSource="grid_NeedDataSource" OnItemDataBound="grdAvgDay_ItemDataBound" OnColumnCreated="grdAvgDay_ColumnCreated"
                            CssClass="RadGrid_Customer">
                            <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
                            <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="Top" IsFilterItemExpanded="False"
                                InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                                <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true" RefreshText="查询" ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                                <CommandItemTemplate>
                                    <div style="position: relative;">
                                        <telerik:RadToolBar BorderWidth="0" ID="gridRTB" SkinID="Query" CssClass="RadToolBar_Customer" AutoPostBack="true"
                                            runat="server" Width="50%" OnButtonClick="gridRTB_ButtonClick" />
                                        <div style="position: absolute; right: 10px; top: 5px;">
                                            <table style="font-weight: bold;">
                                                <tr>
                                                    <td></td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div style="position: absolute; right: 10px; top: 5px;">

                                            <telerik:RadButton ID="RadButton1" runat="server" BackColor="#3A94D3" ForeColor="White" OnClientClicking="ClientButtonClicking">
                                                <ContentTemplate>
                                                    <asp:Label runat="server" ID="Label4" ForeColor="White" Text="百分位说明"></asp:Label>
                                                </ContentTemplate>
                                            </telerik:RadButton>
                                        </div>
                                    </div>
                                </CommandItemTemplate>
                                <PagerStyle FirstPageToolTip="首页" AlwaysVisible="true" Position="Bottom" NextPageToolTip="下页" LastPageToolTip="尾页" PrevPageToolTip="上页"
                                    PageSizeLabelText="显示记录数:" PageSizes="24 48 96" PagerTextFormat="跳转页面:{4}&amp;nbsp;显示 &lt;strong&gt;{2}&lt;/strong&gt; - &lt;strong&gt;{3}&lt;/strong&gt;条,共 &lt;strong&gt;{5}&lt;/strong&gt;条"></PagerStyle>
                            </MasterTableView>
                            <CommandItemStyle Width="100%" />
                            <ClientSettings>
                                <Scrolling AllowScroll="True" EnableVirtualScrollPaging="false" UseStaticHeaders="True"
                                    SaveScrollPosition="false"></Scrolling>
                            </ClientSettings>
                        </telerik:RadGrid>
                    </telerik:RadPageView>
                    <telerik:RadPageView ID="pvChart" runat="server" Visible="true">
                        <div style="padding-top: 6px;">
                            <div style="float: left;">
                                &nbsp &nbsp 图形：
                                <telerik:RadDropDownList runat="server" ID="ChartContent" Width="140px" AutoPostBack="true" OnClientSelectedIndexChanged="ChartContent_change" OnSelectedIndexChanged="ChartContent_SelectedIndexChanged">
                                    <Items>
                                        <telerik:DropDownListItem Text="浓度统计图" Value="K_Value" Selected="true" />
                                        <telerik:DropDownListItem Text="超标统计图" Value="OutRate" />
                                        <telerik:DropDownListItem Text="百分位统计图" Value="Percent" />
                                    </Items>
                                </telerik:RadDropDownList>
                            </div>
                            <%--                            <div style="float: right; display: none;" id="dvChartType">
                                <asp:RadioButtonList runat="server" ID="ChartType" AutoPostBack="true" RepeatDirection="Horizontal" RepeatColumns="4" OnSelectedIndexChanged="ChartType_SelectedIndexChanged">
                                    <asp:ListItem Text="折线图" Value="spline" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="柱形图" Value="column"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>--%>
                            <div style="clear: both">
                            </div>
                        </div>
                        <div id="container" runat="server">
                        </div>
                        <%--<iframe id="iframeOCM" style="width: 100%; height: 100%;"
                            marginheight="0" marginwidth="0" frameborder="0" scrolling="no" runat="server"></iframe>--%>
                    </telerik:RadPageView>
                </telerik:RadMultiPage>
                <telerik:RadWindowManager ID="RadWindowManager1" runat="server" VisibleStatusbar="false" Behaviors="Move,Pin,Reload,Close" EnableShadow="true">
                    <Windows>
                        <telerik:RadWindow ID="ConfigOfflineDialog" runat="server" Height="410px" Width="530px" Skin="Outlook" IconUrl="~/App_Themes/Office2007/images/RadGridHeaderBg2.png"
                            Title="百分位说明" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" />
                    </Windows>
                    <Localization Close="关闭" Maximize="最大化" Minimize="最小化" Restore="还原" Reload="刷新" PinOff="移动"
                        PinOn="固定" />
                </telerik:RadWindowManager>
            </telerik:RadPane>
        </telerik:RadSplitter>
        <asp:HiddenField ID="hdPointIds" runat="server" />
        <asp:HiddenField ID="hdFactors" runat="server" />
        <asp:HiddenField ID="hdStartdt" runat="server" />
        <asp:HiddenField ID="hdEnddt" runat="server" />
        <asp:HiddenField ID="hdPointType" runat="server" />
        <asp:HiddenField ID="hdChartType" runat="server" Value="K_Value" />
        <asp:HiddenField ID="hdFlag" runat="server" Value="0" />
    </form>
</body>
</html>
