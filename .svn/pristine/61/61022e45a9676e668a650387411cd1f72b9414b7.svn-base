<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InstrumentMaintenance.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.OperatingMaintenance.InstrumentMaintenance" %>

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
            <script type="text/javascript" language="javascript">
                var myRadGrid;
                function RadGridCreating(sender, eventArgs) { myRadGrid = sender; }

                //页面刷新
                function Refresh_Grid(args) {
                    if (args) {
                        var MasterTable = $find("<%= gridInstrumentMaintenance.ClientID %>").get_masterTableView();
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
                function onRequestStart(sender, args) {
                    var masterTable = $find("<%= gridInstrumentMaintenance.ClientID %>").get_masterTableView();
                    var CurrentBtn = args.get_item();
                    var CurrentBtnName = CurrentBtn.get_text();
                    var CurrentBtnCommandName = CurrentBtn.get_commandName();
                    switch (CurrentBtnCommandName) {
                        case "InitInsert":
                            {
                                //增加
                                window.radopen("ConfigOfflineAdd.aspx", "ConfigOfflineDialog");
                                args.set_cancel(true);
                                break;
                            }
                        case "DeleteSelected":
                            try {
                                //删除
                                var selItems = masterTable.get_selectedItems();
                                if (selItems.length <= 0) { alert("请选择要删除的记录！") }
                                else
                                {
                                    args.set_cancel(!confirm('确定删除所有选中的记录？'));
                                }
                            } catch (e) { }
                            break;
                        case "RebindGrid":
                            masterTable.rebind();
                            break;
                        default:
                            break;
                    }
                }

                function OnClientClicking() {

                    var dateBegin = $find("<%= dtpBegin.ClientID %>").get_selectedDate();
                    var dateEnd = $find("<%= dtpEnd.ClientID %>").get_selectedDate();
                    if (dateBegin == null) {
                        alert("开始时间不能为空！");
                        return false;
                    }
                    else if (dateEnd == null) {
                        alert("截至时间不能为空！");
                        return false;
                    }
                    else if (dateBegin > dateEnd) {
                        alert("开始时间不能大于截至时间！");
                        return false;
                    }

                }
                //行编辑按钮
                function ShowDetails(effectRateInfoId) {

                    var date1 = $find("<%= dtpBegin.ClientID %>").get_selectedDate();
                    var date2 = $find("<%= dtpEnd.ClientID %>").get_selectedDate();
                    var strDate1 = date1.getFullYear().toString() + "-" + (date1.getMonth() + 1).toString()
                                   + "-" + date1.getDate().toString();
                    var strDate2 = date2.getFullYear().toString() + "-" + (date2.getMonth() + 1).toString()
                                  + "-" + date2.getDate().toString();
                    var uri = "DataEffectRateInfo.aspx?effectRateInfoId=" + effectRateInfoId
                    + "&dtmBegin=" + strDate1 + "&dtmEnd=" + strDate2 + "";
                    window.radopen(encodeURI(uri), "DataEffectRateDialog");
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

                function gridRTB_ClientButtonClicking(sender, args) {
                    var masterTable = $find("<%= gridInstrumentMaintenance.ClientID %>").get_masterTableView();
                    var CurrentBtn = args.get_item();
                    var CurrentBtnName = CurrentBtn.get_text();
                    var CurrentBtnCommandName = CurrentBtn.get_commandName();
                    switch (CurrentBtnCommandName) {
                        case "InitInsert":
                            {
                                //增加
                                window.radopen("ConfigOfflineAdd.aspx", "ConfigOfflineDialog");
                                args.set_cancel(true);
                                break;
                            }
                        case "DeleteSelected":
                            try {
                                //删除
                                var selItems = masterTable.get_selectedItems();
                                if (selItems.length <= 0) { alert("请选择要删除的记录！") }
                                else
                                {
                                    args.set_cancel(!confirm('确定删除所有选中的记录？'));
                                }
                            } catch (e) { }
                            break;
                        case "RebindGrid":
                            masterTable.rebind();
                            break;
                        default:
                            break;
                    }
                }

                //行双击事件
                function OnRowDblClick(sender, args) {
                    var selectIndex = myRadGrid._selectedIndexes.length > 0 ? myRadGrid._selectedIndexes[0] : -1;
                    var selectKeyValues = selectIndex >= 0 ? myRadGrid._clientKeyValues[selectIndex] : null;
                    if (selectKeyValues != null && selectKeyValues["OfflineSettingUid"] != null) {
                        window.radopen("ConfigOfflineEdit.aspx?OffLineSettingUid=" + selectKeyValues["OfflineSettingUid"], "ConfigOfflineDialog");
                    }
                }

            </script>
        </telerik:RadScriptBlock>
        <input id="radGridColWidth" type="hidden" runat="server" value="120" />
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="pointCbxRsm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="pointCbxRsm" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="factorCbxRsm" LoadingPanelID="RadAjaxLoadingPanel1" />

                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridInstrumentMaintenance">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridInstrumentMaintenance" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridRTB">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridInstrumentMaintenance" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="splitter">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridInstrumentMaintenance" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridInstrumentMaintenance" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rbtnlType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rbtnlType" />
                        <telerik:AjaxUpdatedControl ControlID="comboPort" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="comboCityProper" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="comboCity" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="comboCityModel" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
            <ClientEvents OnRequestStart="onRequestStart" />
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" BorderWidth="0"
            Width="100%">
            <telerik:RadPane ID="paneWhere" runat="server" Height="60px" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="width: 100%; height: 100%" cellspacing="1" cellpadding="0" class="Table_Customer"
                    border="0">
                    <tr>
                        <td class="title" style="width: 8%; text-align: center;">测点:
                        </td>
                        <td style="width: 20%;">
                            <CbxRsm:PointCbxRsm runat="server" ApplicationType="Air" CbxWidth="260" CbxHeight="350" MultiSelected="true" DropDownWidth="520" ID="pointCbxRsm"></CbxRsm:PointCbxRsm>
                            <CbxRsm:FactorCbxRsm runat="server" ApplicationType="Air" CbxWidth="260" DropDownWidth="420" ID="factorCbxRsm" Visible="false"></CbxRsm:FactorCbxRsm>

                        </td>
                        <td class="title" style="width: 8%; text-align: center;">仪器型号/编号：</td>
                        <td class="content" style="width: 6%; text-align: center;">
                            <telerik:RadDropDownList ID="ddlQuarterEnd" runat="server" Width="60px">

                                <Items>
                                    <telerik:DropDownListItem runat="server" Selected="True"  Value="1" Text="S001" />
                                    <telerik:DropDownListItem runat="server" Value="2" Text="S002" />
                                    <telerik:DropDownListItem runat="server" Value="3" Text="S003" />
                                    <telerik:DropDownListItem runat="server" Value="4" Text="S004" />

                                </Items>

                            </telerik:RadDropDownList>
                        </td>
                        <td class="title" style="width: 5%; text-align: center;">是否审核：</td>
                        <td class="content" style="width: 8%; text-align: center;">
                            <telerik:RadDropDownList ID="RadDropDownList1" runat="server" Width="80px">

                                <Items>
                                    <telerik:DropDownListItem runat="server" Selected="True" Text="审核" Value="1" />
                                    <telerik:DropDownListItem runat="server" Value="2" Text="未审核" />

                                </Items>

                            </telerik:RadDropDownList>
                        </td>
                        <td class="content" rowspan="2">
                            <asp:ImageButton ID="btnSearch" runat="server" OnClientClick="return OnClientClicking()" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                        </td>
                    </tr>
                    <tr>
                        <td class="title" style="width: 8%; text-align: center;">开始时间:
                        </td>
                        <td class="content" style="width: 20%;">
                            <telerik:RadDateTimePicker ID="dtpBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd"
                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                TimeView-HeaderText="小时" />
                        </td>
                        <td style="width: 8%; text-align: center;">截止时间:

                        </td>
                        <td class="content" style="width: 20%;" colspan="3">
                            <telerik:RadDateTimePicker ID="dtpEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd"
                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                TimeView-HeaderText="小时" />

                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadGrid ID="gridInstrumentMaintenance" runat="server" GridLines="None" Height="100%" Width="100%"
                    AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="true"
                    AutoGenerateColumns="false" AllowMultiRowSelection="false"
                    EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                    ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                    OnNeedDataSource="grid_NeedDataSource" OnItemDataBound="grid_ItemDataBound"
                    CssClass="RadGrid_Customer">
                    <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
                    <MasterTableView GridLines="None" CommandItemDisplay="Top" IsFilterItemExpanded="False"
                        InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                        <%--<CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true" RefreshText="查询" ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />--%>
                        <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true" RefreshText="查询" ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                        <CommandItemTemplate>
                            <%--<telerik:RadToolBar BorderWidth="0" ID="gridRTB" SkinID="Query" CssClass="RadToolBar_Customer" AutoPostBack="true"
                                        runat="server" Width="50%" OnButtonClick="gridRTB_ButtonClick" />--%>

                            <telerik:RadToolBar ID="RadToolBar1" runat="server" AutoPostBack="true" CssClass="RadToolBar_Customer" SkinID="CRUD" Width="100%" OnClientButtonClicking="gridRTB_ClientButtonClicking" />

                        </CommandItemTemplate>
                        <Columns>
                            <telerik:GridClientSelectColumn HeaderText="选择" UniqueName="ClientSelectColumn" Exportable="false">
                                <HeaderStyle Width="40px"></HeaderStyle>
                            </telerik:GridClientSelectColumn>
                            <telerik:GridTemplateColumn HeaderText="序号" UniqueName="Row" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate><%#Container.DataSetIndex + 1%></ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn HeaderText="站点名称" UniqueName="MonitoringResult" DataField="MonitoringResult" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" EmptyDataText="--" />
                            <telerik:GridBoundColumn HeaderText="仪器型号/编号" UniqueName="MonitoringResult" DataField="MonitoringResult" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" EmptyDataText="--" />
                            <telerik:GridBoundColumn HeaderText="维修保养人" UniqueName="MonitoringResult" DataField="MonitoringResult" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" EmptyDataText="--" />
                            <telerik:GridBoundColumn HeaderText="日期" UniqueName="MonitoringResult" DataField="MonitoringResult" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" EmptyDataText="--" />
                            <telerik:GridBoundColumn HeaderText="是否审核" UniqueName="MonitoringResult" DataField="MonitoringResult" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" EmptyDataText="--" />
                            <telerik:GridTemplateColumn HeaderText="操作" UniqueName="Row" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <input type="button" value="审核" />&nbsp;&nbsp;<input type="button" value="详情" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle FirstPageToolTip="首页" AlwaysVisible="true" Position="Bottom" NextPageToolTip="下页" LastPageToolTip="尾页" PrevPageToolTip="上页"
                            PageSizeLabelText="显示记录数:" PagerTextFormat="跳转页面:{4}&amp;nbsp;显示 &lt;strong&gt;{2}&lt;/strong&gt; - &lt;strong&gt;{3}&lt;/strong&gt;条,共 &lt;strong&gt;{5}&lt;/strong&gt;条"></PagerStyle>
                    </MasterTableView>
                    <CommandItemStyle Width="100%" />
                    <ClientSettings>
                        <Selecting AllowRowSelect="true" />
                        <ClientEvents OnGridCreating="RadGridCreating" OnRowDblClick="OnRowDblClick" />
                        <Scrolling AllowScroll="true" EnableVirtualScrollPaging="false" UseStaticHeaders="true" FrozenColumnsCount="2"
                            SaveScrollPosition="true"></Scrolling>

                    </ClientSettings>
                </telerik:RadGrid>
            </telerik:RadPane>
        </telerik:RadSplitter>
        <telerik:RadWindowManager ID="radWM" runat="server" VisibleStatusbar="false" Behaviors="Move,Pin,Reload,Close" EnableShadow="true">
            <Windows>
                <telerik:RadWindow ID="DataEffectRateDialog" runat="server" Height="500px" Width="730px" Skin="Outlook" IconUrl="~/App_Themes/Office2007/images/RadGridHeaderBg2.png"
                    Title="离线配置" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" />
            </Windows>
            <Localization Close="关闭" Maximize="最大化" Minimize="最小化" Restore="还原" Reload="刷新" PinOff="移动"
                PinOn="固定" />
        </telerik:RadWindowManager>
    </form>
</body>
</html>
