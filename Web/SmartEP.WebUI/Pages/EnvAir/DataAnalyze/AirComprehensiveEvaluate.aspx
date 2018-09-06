<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AirComprehensiveEvaluate.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.DataAnalyze.AirComprehensiveEvaluate" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<%@ Register TagPrefix="CbxRsm" TagName="FactorCbxRsm" Src="~/Controls/FactorCbxRsm.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
            <script type="text/javascript" language="javascript">
                var myRadGrid;
                function RadGridCreating(sender, eventArgs) { myRadGrid = sender; }

                //页面刷新
                function Refresh_Grid(args) {
                    if (args) {
                        var MasterTable = $find("<%= grdComprehensive.ClientID %>").get_masterTableView();
                        MasterTable.rebind();
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
            </script>
        </telerik:RadScriptBlock>
        <input id="radGridColWidth" type="hidden" runat="server" value="120" />
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="grdComprehensive">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdComprehensive" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridRTB">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdComprehensive" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="pointCbxRsm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="pointCbxRsm" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdComprehensive" LoadingPanelID="RadAjaxLoadingPanel1" />
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
                        <td class="title" style="width: 50px; text-align: center;">区域
                        </td>
                        <td class="content" style="width: 220px;">
                            <telerik:RadComboBox runat="server" ID="comboCity" Width="180px" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput"
                                Localization-CheckAllString="选择全部">
                                <Items>
                                    <telerik:RadComboBoxItem Text="姑苏区" Value="6a4e7093-f2c6-46b4-a11f-0f91b4adf379" />
                                    <telerik:RadComboBoxItem Text="吴中区" Value="e1c104f3-aaf3-4d0e-9591-36cdc83be15a" />
                                    <telerik:RadComboBoxItem Text="高新区" Value="f320aa73-7c55-45d4-a363-e21408e0aac3" />
                                    <telerik:RadComboBoxItem Text="工业园区" Value="69a993ff-78c6-459b-9322-ee77e0c8cd68" />
                                    <telerik:RadComboBoxItem Text="相城区" Value="8756bd44-ff18-46f7-aedf-615006d7474c" />
                                    <telerik:RadComboBoxItem Text="苏州市区" Value="7e05b94c-bbd4-45c3-919c-42da2e63fd43" Checked="true" />
                                    <telerik:RadComboBoxItem Text="张家港市" Value="66d2abd1-ca39-4e39-909f-da872704fbfd" Checked="true" />
                                    <telerik:RadComboBoxItem Text="常熟市" Value="d7d7a1fe-493a-4b3f-8504-b1850f6d9eff" Checked="true" />
                                    <telerik:RadComboBoxItem Text="太仓市" Value="57b196ed-5038-4ad0-a035-76faee2d7a98" Checked="true" />
                                    <telerik:RadComboBoxItem Text="昆山市" Value="2e2950cd-dbab-43b3-811d-61bd7569565a" Checked="true" />
                                    <telerik:RadComboBoxItem Text="吴江区" Value="2fea3cb2-8b95-45e6-8a71-471562c4c89c" Checked="true" />
                                    <telerik:RadComboBoxItem Text="全市" Value="5a566145-4884-453c-93ad-16e4344c85c9" Checked="true" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                        <td class="title" style="width: 80px; text-align: center;">开始时间
                        </td>
                        <td class="content">
                            <telerik:RadDatePicker ID="dtpBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd"
                                DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday"
                                MonthYearNavigationSettings-CancelButtonCaption="取消" Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                MonthYearNavigationSettings-OkButtonCaption="确定" Width="150" Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                MonthYearNavigationSettings-TodayButtonCaption="当前年月" Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" />
                        </td>
                        <td class="title" style="width: 80px; text-align: center;">结束时间
                        </td>
                        <td class="content" style="width: 200px">
                            <telerik:RadDatePicker ID="dtpEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd"
                                DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday"
                                MonthYearNavigationSettings-CancelButtonCaption="取消" Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                MonthYearNavigationSettings-OkButtonCaption="确定" Width="150" Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                MonthYearNavigationSettings-TodayButtonCaption="当前年月" Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" />
                        </td>
                        <td class="title" style="width: 80px">基数年份：
                        </td>
                        <td class="content" style="width: 100px;">
                            <telerik:RadComboBox runat="server" ID="Year" Width="90px" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                                <Items>
                                    <telerik:RadComboBoxItem Text="2013" Value="2013" Selected="true" />
                                    <telerik:RadComboBoxItem Text="2015" Value="2015" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                        <td class="content" align="left">
                            <asp:ImageButton ID="btnSearch" runat="server" OnClientClick="return OnClientClicking()" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                        </td>
                    </tr>

                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadGrid ID="grdComprehensive" runat="server" GridLines="None" Height="100%" Width="100%"
                    AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                    AutoGenerateColumns="true" AllowMultiRowSelection="false"
                    EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                    ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                    OnNeedDataSource="grid_NeedDataSource" OnItemDataBound="grid_ItemDataBound" OnColumnCreated="grdComprehensive_ColumnCreated"
                    CssClass="RadGrid_Customer">
                    <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
                    <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="Top" IsFilterItemExpanded="False"
                        InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                        <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true" RefreshText="查询" ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                        <CommandItemTemplate>
                            <telerik:RadToolBar BorderWidth="0" ID="gridRTB" SkinID="Query" CssClass="RadToolBar_Customer" AutoPostBack="true"
                                runat="server" Width="50%" OnButtonClick="gridRTB_ButtonClick" OnClientButtonClicking="gridRTB_ClientButtonClicking" />
                        </CommandItemTemplate>
                        <PagerStyle FirstPageToolTip="首页" AlwaysVisible="true" Position="Bottom" NextPageToolTip="下页" LastPageToolTip="尾页" PrevPageToolTip="上页"
                            PageSizeLabelText="显示记录数:" PagerTextFormat="跳转页面:{4}&amp;nbsp;显示 &lt;strong&gt;{2}&lt;/strong&gt; - &lt;strong&gt;{3}&lt;/strong&gt;条,共 &lt;strong&gt;{5}&lt;/strong&gt;条"></PagerStyle>
                    </MasterTableView>
                    <CommandItemStyle Width="100%" />
                    <ClientSettings>
                        <Scrolling AllowScroll="True" EnableVirtualScrollPaging="false" UseStaticHeaders="True"
                            SaveScrollPosition="true"></Scrolling>
                    </ClientSettings>
                </telerik:RadGrid>
            </telerik:RadPane>
        </telerik:RadSplitter>
    </form>
</body>
</html>

