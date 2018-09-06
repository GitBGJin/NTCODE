<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PollutantAnalyze.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.DataAnalyze.PollutantAnalyze" %>

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
            <script src="../../../Resources/JavaScript/ChartOperator/merge.js"></script>
            <script type="text/javascript">
                var isfirst = 1;//是否首次加载

                function OnClientClicking() {
                    var date1 = new Date();
                    var date2 = new Date();
                    date1 = $find("<%= dtpBegin.ClientID %>").get_selectedDate();
                    date2 = $find("<%= dtpEnd.ClientID %>").get_selectedDate();
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
                //tab页切换时时间检查
                function OnClientSelectedIndexChanging(sender, args) {
                    var date1 = new Date();
                    var date2 = new Date();
                    date1 = $find("<%= dtpBegin.ClientID %>").get_selectedDate();
                    date2 = $find("<%= dtpEnd.ClientID %>").get_selectedDate();
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
                //Grid按钮行处理
                function gridRTB_ClientButtonClicking(sender, args) {
                    args.set_cancel(!OnClientClicking());
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

                //查看图表
                function TabSelected(sender, args) {
                    try {
                        var isfirst = document.getElementById("<%=FirstLoadChart.ClientID%>");
                        var text = $.trim(args._tab._element.innerText);
                        if (text == '图表') {
                            if (isfirst.value == "1") {
                                isfirst.value = 0;
                                try {
                                    ChartPager(0, defaultPagesize);//图表分页
                                } catch (e) {
                                }
                                RefreshChart();
                            }
                        }
                    } catch (e) {
                        //InitTogetherChart();
                    }
                }

                //图表刷新
                function RefreshChart() {
                    try {
                        //var height = document.body.clientHeight - 112 - 40;
                        //togetherChart("../Chart/ChartFrame.aspx", height);

                        var hiddenData = $("#HiddenData").val().split('|');
                        var height = parseInt(parseInt($("#pvChart").css("height")) - 40);
                        groupChartByPointid(hiddenData[0], "../Chart/ChartFrame.aspx", height);//以站点分组
                    } catch (e) {
                    }
                    //try {
                    //    var chartPage = document.getElementById("pvChart");
                    //    chartPage.children[0].contentWindow.InitChart();
                    //} catch (e) {
                    //}
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
                <telerik:AjaxSetting AjaxControlID="grdPAnalyze">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdPAnalyze" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridRTB">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdPAnalyze" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdPAnalyze" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenData" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenChartType" />
                        <telerik:AjaxUpdatedControl ControlID="FirstLoadChart" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rbtnlType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rbtnlType" />
                        <telerik:AjaxUpdatedControl ControlID="dvProper" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dvPoints" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ChartType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ChartType" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenChartType" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
            <ClientEvents OnRequestStart="onRequestStart" OnResponseEnd="onRequestEnd" />
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" Width="100%"
            BorderWidth="0" BorderStyle="None" BorderSize="0">
            <telerik:RadPane ID="paneWhere" runat="server" Height="60px" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="width: 100%; height: 100%" cellspacing="1" cellpadding="0" class="Table_Customer"
                    border="0">
                    <tr>
                        <td class="title" style="width: 120px; text-align: center;">查询范围：
                        </td>
                        <td class="content" style="width: 400px;">
                            <asp:RadioButtonList ID="rbtnlType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rbtnlType_SelectedIndexChanged" RepeatLayout="Flow" RepeatColumns="5">

                                <asp:ListItem Text="区域" Value="CityProper" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="测点" Value="Port"></asp:ListItem>

                            </asp:RadioButtonList>
                        </td>
                        <td class="title" style="width: 120px; text-align: center;">查询区域/测点：
                        </td>
                        <td class="content" style="width: auto;">
                            <div runat="server" id="dvProper">
                                <telerik:RadDropDownList runat="server" ID="ddlCityProper" Width="350px">
                                    <Items>
                                        <telerik:DropDownListItem Text="姑苏区" Value="6a4e7093-f2c6-46b4-a11f-0f91b4adf379" />
                                        <telerik:DropDownListItem Text="吴中区" Value="e1c104f3-aaf3-4d0e-9591-36cdc83be15a" />
                                        <telerik:DropDownListItem Text="高新区" Value="f320aa73-7c55-45d4-a363-e21408e0aac3" />
                                        <telerik:DropDownListItem Text="工业园区" Value="9a993ff-78c6-459b-9322-ee77e0c8cd68" />
                                        <telerik:DropDownListItem Text="相城区" Value="8756bd44-ff18-46f7-aedf-615006d7474c" />
                                        <telerik:DropDownListItem Text="苏州市区" Value="7e05b94c-bbd4-45c3-919c-42da2e63fd43" Selected="true" />
                                        <telerik:DropDownListItem Text="张家港市" Value="66d2abd1-ca39-4e39-909f-da872704fbfd" />
                                        <telerik:DropDownListItem Text="常熟市" Value="d7d7a1fe-493a-4b3f-8504-b1850f6d9eff" />
                                        <telerik:DropDownListItem Text="太仓市" Value="57b196ed-5038-4ad0-a035-76faee2d7a98" />
                                        <telerik:DropDownListItem Text="昆山市" Value="2e2950cd-dbab-43b3-811d-61bd7569565a" />
                                        <telerik:DropDownListItem Text="吴江区" Value="2fea3cb2-8b95-45e6-8a71-471562c4c89c" />
                                        <telerik:DropDownListItem Text="全市" Value="5a566145-4884-453c-93ad-16e4344c85c9" />
                                    </Items>
                                </telerik:RadDropDownList>
                            </div>
                            <div runat="server" id="dvPoints" style="display: none">
                                <CbxRsm:PointCbxRsm runat="server" ApplicationType="Air" CbxWidth="380" CbxHeight="350" MultiSelected="false" DropDownWidth="520" ID="pointCbxRsm"></CbxRsm:PointCbxRsm>
                                <%--  <CbxRsm:PointCbxRsm runat="server" ApplicationType="Air" Visible="false" CbxWidth="350" CbxHeight="350" MultiSelected="false" DropDownWidth="520" ID="pointCbxRsm"></CbxRsm:PointCbxRsm>--%>
                            </div>
                        </td>
                        <td class="content" align="left" rowspan="2">
                            <asp:ImageButton ID="btnSearch" runat="server" OnClientClick="return OnClientClicking()" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                        </td>
                    </tr>
                    <tr>
                        <td class="title" style="width: 80px">监测因子：
                        </td>
                        <td class="content">
                            <telerik:RadComboBox runat="server" ID="rcbFactors" Localization-CheckAllString="全选" Width="250px" CheckBoxes="true" Visible="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                                <Items>
                                    <telerik:RadComboBoxItem Text="细颗粒物" Value="PM25" Checked="true" />
                                    <telerik:RadComboBoxItem Text="可吸入颗粒物" Value="PM10" Checked="true" />
                                    <telerik:RadComboBoxItem Text="二氧化氮" Value="NO2" Checked="true" />
                                    <telerik:RadComboBoxItem Text="二氧化硫" Value="SO2" Checked="true" />
                                    <telerik:RadComboBoxItem Text="一氧化碳" Value="CO" Checked="true" />
                                    <telerik:RadComboBoxItem Text="臭氧1小时" Value="O3" Checked="true" />
                                    <telerik:RadComboBoxItem Text="臭氧8小时" Value="Recent8HoursO3" Checked="true" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                        <td class="title" style="width: 80px; text-align: center;">开始时间：
                        </td>
                        <td class="content" style="width: 420px;">
                            <table id="Table1" style="width: 100%; height: 100%" cellspacing="1" cellpadding="0" class="Table_Customer"
                                border="0">
                                <tr>
                                    <td style="width: 50px; text-align: left;">
                                        <telerik:RadDatePicker ID="dtpBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px" Width="110px" DateInput-DateFormat="yyyy年MM月dd日"
                                            DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday"
                                            Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                            Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                            Calendar-FastNavigationSettings-CancelButtonCaption="取消" />
                                    </td>
                                    <td style="width: 40px; text-align: left;">结束时间</td>
                                    <td style="width: 50px; text-align: left;">
                                        <telerik:RadDatePicker ID="dtpEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px" Width="110px" DateInput-DateFormat="yyyy年MM月dd日"
                                            DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday"
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
                        <telerik:RadGrid ID="grdPAnalyze" runat="server" GridLines="None" Height="100%" Width="100%"
                            AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false"
                            AutoGenerateColumns="true" AllowMultiRowSelection="false"
                            EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                            ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                            OnNeedDataSource="grid_NeedDataSource" OnItemDataBound="grid_ItemDataBound" OnColumnCreated="grdPAnalyze_ColumnCreated"
                            CssClass="RadGrid_Customer">
                            <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
                            <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="Top" IsFilterItemExpanded="False"
                                InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                                <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true" RefreshText="查询" ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                                <CommandItemTemplate>
                                    <div style="position: relative;">
                                        <telerik:RadToolBar BorderWidth="0" ID="gridRTB" SkinID="Query" CssClass="RadToolBar_Customer" AutoPostBack="true"
                                            runat="server" Width="50%" OnButtonClick="gridRTB_ButtonClick" OnClientButtonClicking="gridRTB_ClientButtonClicking" />
                                        <div style="position: absolute; right: 10px; top: 5px;">
                                            <table style="font-weight: bold;">
                                                <tr>
                                                    <td>注：CO单位（mg/m³），其它的单位（μg/m³）</td>
                                                </tr>
                                            </table>
                                        </div>
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
                                <Scrolling AllowScroll="True" EnableVirtualScrollPaging="false" UseStaticHeaders="True"
                                    SaveScrollPosition="true"></Scrolling>
                            </ClientSettings>
                        </telerik:RadGrid>
                    </telerik:RadPageView>
                    <telerik:RadPageView ID="pvChart" runat="server" Visible="true">
                        <div style="padding-top: 6px;">
                            <div style="float: right;">
                                <asp:RadioButtonList runat="server" ID="ChartType" AutoPostBack="true" RepeatDirection="Horizontal" RepeatColumns="3" OnSelectedIndexChanged="ChartType_SelectedIndexChanged">
                                    <asp:ListItem Text="折线图" Value="spline" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="柱形图" Value="column"></asp:ListItem>
                                    <asp:ListItem Text="点图" Value="scatter"></asp:ListItem>
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
        <asp:HiddenField ID="AjaxURL" runat="server" Value="../ChartAjaxRequest/PollutantAnalyze.ashx" />
        <asp:HiddenField ID="HiddenChartType" runat="server" Value="spline" />
    </form>
</body>
</html>
