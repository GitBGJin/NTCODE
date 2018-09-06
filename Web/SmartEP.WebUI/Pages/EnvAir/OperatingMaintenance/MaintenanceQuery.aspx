<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MaintenanceQuery.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.OperatingMaintenance.MaintenanceQuery" %>

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
                function OnClientClicking() {
                    var date1 = new Date();
                    var date2 = new Date();
                    date1 = $find("<%= dayBegin.ClientID %>").get_selectedDate();
                    date2 = $find("<%= dayEnd.ClientID %>").get_selectedDate();
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
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1"
            UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="rdlTaskType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rdlTaskType" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="rcbCarry" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridMaintenance" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="radlDataType" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridMaintenance">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridMaintenance" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridRTB">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridMaintenance" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="radlDataType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="dvEvent" />
                        <telerik:AjaxUpdatedControl ControlID="dvDevice" />
                        <telerik:AjaxUpdatedControl ControlID="dvInspect" />
                        <telerik:AjaxUpdatedControl ControlID="radlDataType" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
            <ClientEvents OnRequestStart="onRequestStart" OnResponseEnd="onRequestEnd" />
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="RadSplitter2" runat="server" Width="100%" LiveResize="true" ResizeWithParentPane="true" Height="100%">
            <telerik:RadPane ID="navigationPane" runat="server" Width="20%"
                ShowContentDuringLoad="false">
                <telerik:RadTreeView ID="RadTreeView1" runat="server" CheckBoxes="true" TriStateCheckBoxes="true" CheckChildNodes="true">
                </telerik:RadTreeView>
            </telerik:RadPane>
            <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Both">
            </telerik:RadSplitBar>
            <telerik:RadPane ID="contentPane" runat="server" Scrolling="none">
                <telerik:RadSplitter ID="RadSplitter1" runat="server" Orientation="Horizontal" Height="100%" Width="100%"
                    BorderWidth="0" BorderStyle="None" BorderSize="0">
                    <telerik:RadPane ID="paneWhere" runat="server" Height="80px" Width="100%" Scrolling="None"
                        BorderWidth="0" BorderStyle="None" BorderSize="0">
                        <table id="Tb" style="height: 100%;" cellspacing="1" cellpadding="0" class="Table_Customer"
                            border="0">
                            <tr>
                                <td class="title" style="width: 100px; text-align: center;">查询维度:
                                </td>
                                <td class="content" style="width: 360px;">
                                    <asp:RadioButtonList ID="radlDataType" Width="330px" OnSelectedIndexChanged="radlDataType_SelectedIndexChanged" runat="server" AutoPostBack="true" RepeatDirection="Horizontal">
                                    </asp:RadioButtonList>
                                </td>
                                <td class="title" style="width: 100px; text-align: center;">开始时间:
                                </td>
                                <td class="content" style="width: 300px;" id="timeq">
                                    <telerik:RadDatePicker ID="dayBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                        DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd"
                                        Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                        Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                        Calendar-FastNavigationSettings-CancelButtonCaption="取消" />
                                </td>
                                <td class="title" style="width: 110px; text-align: center;">结束时间:
                                </td>
                                <td class="content" style="width: 300px;" id="Td1">
                                    <telerik:RadDatePicker ID="dayEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                        DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd"
                                        Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                        Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                        Calendar-FastNavigationSettings-CancelButtonCaption="取消" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div runat="server" id="dvInspect">
                                        <table>
                                            <tr>
                                                <td class="title" style="width: 110px; text-align: center;">任务类型:
                                                </td>
                                                <td class="content" style="width: 150px;">
                                                    <telerik:RadDropDownList ID="rdlTaskType" runat="server" Width="150px" AutoPostBack="true" OnSelectedIndexChanged="rdlTaskType_SelectedIndexChanged">
                                                    </telerik:RadDropDownList>
                                                    <%-- <telerik:RadComboBox runat="server" ID="rcbTaskType" Localization-CheckAllString="全选" Width="150px" CheckBoxes="true" Visible="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                                                    </telerik:RadComboBox>--%>
                                                </td>
                                                <td class="title" style="width: 110px; text-align: center;">执行项目:
                                                </td>
                                                <td class="content" style="width: 150px;">
                                                    <telerik:RadComboBox runat="server" ID="rcbCarry" Localization-CheckAllString="全选" Width="150px" CheckBoxes="true" Visible="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                                                    </telerik:RadComboBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div runat="server" id="dvDevice">
                                        <table>
                                            <tr>
                                                <td class="title" style="width: 110px; text-align: center;">故障类型:
                                                </td>
                                                <td class="content" style="width: 150px;">
                                                    <telerik:RadComboBox runat="server" ID="faultTypes" Localization-CheckAllString="全选" Width="150px" CheckBoxes="true" Visible="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                                                    </telerik:RadComboBox>
                                                </td>
                                                <td class="title" style="width: 110px; text-align: center;">处理天数:
                                                </td>
                                                <td class="content" style="width: 150px;">
                                                    <telerik:RadComboBox runat="server" ID="dealDays" Localization-CheckAllString="全选" Width="150px" CheckBoxes="true" Visible="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                                                    </telerik:RadComboBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div runat="server" id="dvEvent">
                                        <table>
                                            <tr>
                                                <td class="title" style="width: 110px; text-align: center;">事件类型:
                                                </td>
                                                <td class="content" style="width: 150px;">
                                                    <telerik:RadComboBox runat="server" ID="RadComboBox1" Localization-CheckAllString="全选" Width="150px" CheckBoxes="true" Visible="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                                                        <Items>
                                                            <telerik:RadComboBoxItem Text="进站事由" Value="0" Checked="true" />
                                                        </Items>
                                                    </telerik:RadComboBox>
                                                </td>
                                                <td class="title" style="width: 110px; text-align: center;">执行工作:
                                                </td>
                                                <td class="content" style="width: 150px;">
                                                    <telerik:RadComboBox runat="server" ID="rcbWork" Localization-CheckAllString="全选" Width="150px" CheckBoxes="true" Visible="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                                                    </telerik:RadComboBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                                <td class="title" style="width: 100px; text-align: center;">处理人:
                                </td>
                                <td class="title" style="width: 100px; text-align: left;">
                                    <telerik:RadTextBox runat="server" ID="rtbPerson"></telerik:RadTextBox>
                                </td>
                                <td class="content" align="left" style="width: 110px;">
                                    <asp:CheckBox ID="IsStatistical" Checked="false" Text="统计次数" runat="server" />
                                </td>
                                <td class="content" align="left">
                                    <asp:ImageButton ID="btnSearch" runat="server" OnClientClick="return OnClientClicking()" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                                </td>
                            </tr>
                        </table>
                    </telerik:RadPane>
                    <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None"
                        BorderWidth="0" BorderStyle="None" BorderSize="0">
                        <telerik:RadGrid ID="gridMaintenance" runat="server" GridLines="None" Height="100%" Width="100%"
                            AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                            AutoGenerateColumns="true" AllowMultiRowSelection="false"
                            EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                            ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                            OnNeedDataSource="gridMaintenance_NeedDataSource" OnItemDataBound="gridMaintenance_ItemDataBound" OnColumnCreated="gridMaintenance_ColumnCreated"
                            CssClass="RadGrid_Customer">
                            <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
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
                                    <telerik:GridTemplateColumn HeaderText="" UniqueName="rowNum" HeaderStyle-Width="20px" HeaderStyle-Height="50px"
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
                                <Scrolling AllowScroll="True" EnableVirtualScrollPaging="false" UseStaticHeaders="True" FrozenColumnsCount="2"
                                    SaveScrollPosition="true"></Scrolling>
                            </ClientSettings>
                        </telerik:RadGrid>
                    </telerik:RadPane>
                </telerik:RadSplitter>
            </telerik:RadPane>
        </telerik:RadSplitter>
    </form>
</body>
</html>
