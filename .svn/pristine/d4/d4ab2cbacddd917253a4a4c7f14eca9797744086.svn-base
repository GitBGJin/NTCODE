<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InvalidDaysYearAnalyze.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.DataAnalyze.InvalidDaysYearAnalyze" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<%@ Register TagPrefix="CbxRsm" TagName="FactorCbxRsm" Src="~/Controls/FactorCbxRsm.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
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
                        var MasterTable = $find("<%= gridInvalidDays.ClientID %>").get_masterTableView();
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

                //按钮行处理
                function gridRTB_ClientButtonClicking(sender, args) {
                    var masterTable = $find("<%= gridInvalidDays.ClientID %>").get_masterTableView();
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

                //行编辑按钮
                function ShowEditForm(OffLineSettingUid) {
                    window.radopen("ConfigOfflineEdit.aspx?OffLineSettingUid=" + OffLineSettingUid, "ConfigOfflineDialog");
                }

                //行双击事件
                function OnRowDblClick(sender, args) {
                    var selectIndex = myRadGrid._selectedIndexes.length > 0 ? myRadGrid._selectedIndexes[0] : -1;
                    var selectKeyValues = selectIndex >= 0 ? myRadGrid._clientKeyValues[selectIndex] : null;
                    if (selectKeyValues != null && selectKeyValues["OfflineSettingUid"] != null) {
                        window.radopen("ConfigOfflineEdit.aspx?OffLineSettingUid=" + selectKeyValues["OfflineSettingUid"], "ConfigOfflineDialog");
                    }
                }

                function OnClientClicking() {
                    var date1 = new Date();
                    var date2 = new Date();
                    // var date1 = $find("<= dtpBegin.ClientID %>").get_selectedDate();
                    //     var date2 = $find("<= dtpEnd.ClientID %>").get_selectedDate();
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
                    // var date1 = $find("<= dtpBegin.ClientID %>").get_selectedDate();
                    //   var date2 = $find("<= dtpEnd.ClientID %>").get_selectedDate();
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
        <input id="radGridColWidth" type="hidden" runat="server" value="120" />
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="pointCbxRsm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="pointCbxRsm" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridInvalidDays">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridInvalidDays" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridRTB">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridInvalidDays" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="splitter">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridInvalidDays" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridInvalidDays" LoadingPanelID="RadAjaxLoadingPanel1" />
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
                        <td class="title" style="width: 120px; text-align: center;">时间
                        </td>
                        <td class="content">
                            <telerik:RadDropDownList runat="server" ID="ddlBegin">
                            </telerik:RadDropDownList>
                        </td>
                        <td class="title" style="width: 120px; text-align: center;">截止日期
                        </td>
                        <td class="content">
                            <telerik:RadDropDownList runat="server" ID="ddlEnd">
                            </telerik:RadDropDownList>
                        </td>
                        <td class="content" align="left" rowspan="2">
                            <asp:ImageButton ID="btnSearch" runat="server" OnClientClick="return OnClientClicking()" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                        </td>
                    </tr>
                    <tr>
                        <td class="title" style="width: 120px; text-align: center;">查询类型:
                        </td>
                        <td class="content" style="width: 400px;">
                            <asp:RadioButtonList ID="rbtnlType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rbtnlType_SelectedIndexChanged" RepeatLayout="Flow" RepeatColumns="5">
                                <asp:ListItem Text="点位" Value="Port" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="苏州市区" Value="CityProper"></asp:ListItem>
                                <asp:ListItem Text="城市均值" Value="City"></asp:ListItem>
                                <asp:ListItem Text="城市均值（创模点）" Value="CityModel"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td class="title" style="width: 120px; text-align: center;">查询点位（区域）:
                        </td>
                        <td class="content" style="width: 300px;">
                            <telerik:RadComboBox runat="server" ID="comboPort" Width="180px" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                                <Items>
                                    <telerik:RadComboBoxItem Text="南门" Value="28" Checked="true" />
                                    <telerik:RadComboBoxItem Text="彩香" Value="29" Checked="true" />
                                    <telerik:RadComboBoxItem Text="轧钢厂" Value="31" Checked="true" />
                                    <telerik:RadComboBoxItem Text="吴中区" Value="34" Checked="true" />
                                    <telerik:RadComboBoxItem Text="高新区" Value="35" Checked="true" />
                                    <telerik:RadComboBoxItem Text="工业园区" Value="88" Checked="true" />
                                    <telerik:RadComboBoxItem Text="相城区" Value="101" Checked="true" />
                                    <telerik:RadComboBoxItem Text="上方山" Value="103" Checked="true" />
                                    <telerik:RadComboBoxItem Text="兴福子站" Value="110" Checked="true" />
                                    <telerik:RadComboBoxItem Text="菱塘子站" Value="109" Checked="true" />
                                    <telerik:RadComboBoxItem Text="海虞子站" Value="108" Checked="true" />
                                    <telerik:RadComboBoxItem Text="张家港市监测站" Value="116" Checked="true" />
                                    <telerik:RadComboBoxItem Text="城北小学" ToolTip="" Value="117" Checked="true" />
                                    <telerik:RadComboBoxItem Text="昆山第二中学" Value="114" Checked="true" />
                                    <telerik:RadComboBoxItem Text="震川中学" Value="115" Checked="true" />
                                    <telerik:RadComboBoxItem Text="吴江环保局" Value="105" Checked="true" />
                                    <telerik:RadComboBoxItem Text="教师进修学校" Value="106" Checked="true" />
                                    <telerik:RadComboBoxItem Text="吴江开发区" Value="107" Checked="true" />
                                    <telerik:RadComboBoxItem Text="太仓监测站" Value="111" Checked="true" />
                                    <telerik:RadComboBoxItem Text="科教新城实小" Value="112" Checked="true" />
                                </Items>
                            </telerik:RadComboBox>
                            <telerik:RadComboBox runat="server" ID="comboCityProper" Width="180px" CheckBoxes="true" Visible="false" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                                <Items>
                                    <telerik:RadComboBoxItem Text="姑苏区" Value="6a4e7093-f2c6-46b4-a11f-0f91b4adf379" Checked="true" />
                                    <telerik:RadComboBoxItem Text="吴中区" Value="e1c104f3-aaf3-4d0e-9591-36cdc83be15a" Checked="true" />
                                    <telerik:RadComboBoxItem Text="高新区" Value="f320aa73-7c55-45d4-a363-e21408e0aac3" Checked="true" />
                                    <telerik:RadComboBoxItem Text="工业园区" Value="69a993ff-78c6-459b-9322-ee77e0c8cd68" Checked="true" />
                                    <telerik:RadComboBoxItem Text="相城区" ToolTip="" Value="8756bd44-ff18-46f7-aedf-615006d7474c" Checked="true" />
                                    <telerik:RadComboBoxItem Text="市区合计" Value="7e05b94c-bbd4-45c3-919c-42da2e63fd43" Checked="true" />
                                </Items>
                            </telerik:RadComboBox>
                            <telerik:RadComboBox runat="server" ID="comboCity" Width="180px" CheckBoxes="true" Visible="false" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                                <Items>
                                    <telerik:RadComboBoxItem Text="苏州市区" Value="7e05b94c-bbd4-45c3-919c-42da2e63fd43" Checked="true" />
                                    <telerik:RadComboBoxItem Text="张家港" Value="4296ce53-78d3-4741-9eda-6306e3e5b399" Checked="true" />
                                    <telerik:RadComboBoxItem Text="常熟" Value="f7444783-a425-411c-a54b-f9fed72ec72e" Checked="true" />
                                    <telerik:RadComboBoxItem Text="太仓" Value="d993d02f-fcc3-4ea6-b52b-9414fbd9b8e6" Checked="true" />
                                    <telerik:RadComboBoxItem Text="昆山" ToolTip="" Value="636775d8-091d-4754-9ed2-cd9dfef1f6ab" Checked="true" />
                                    <telerik:RadComboBoxItem Text="吴江" Value="48d749e6-d07c-4764-8d50-50f170defe0b" Checked="true" />
                                    <telerik:RadComboBoxItem Text="全市合计" Value="00000000000000000000000000000000000000000000000000" Checked="true" />
                                </Items>
                            </telerik:RadComboBox>
                            <telerik:RadComboBox runat="server" ID="comboCityModel" Width="180px" CheckBoxes="true" Visible="false" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                                <Items>
                                    <telerik:RadComboBoxItem Text="苏州市区" Value="7e05b94c-bbd4-45c3-919c-42da2e63fd43" Checked="true" />
                                    <telerik:RadComboBoxItem Text="张家港" Value="4296ce53-78d3-4741-9eda-6306e3e5b399" Checked="true" />
                                    <telerik:RadComboBoxItem Text="常熟" Value="f7444783-a425-411c-a54b-f9fed72ec72e" Checked="true" />
                                    <telerik:RadComboBoxItem Text="太仓" Value="d993d02f-fcc3-4ea6-b52b-9414fbd9b8e6" Checked="true" />
                                    <telerik:RadComboBoxItem Text="昆山" ToolTip="" Value="636775d8-091d-4754-9ed2-cd9dfef1f6ab" Checked="true" />
                                    <telerik:RadComboBoxItem Text="吴江" Value="48d749e6-d07c-4764-8d50-50f170defe0b" Checked="true" />
                                    <telerik:RadComboBoxItem Text="全市合计" Value="00000000000000000000000000000000000000000000000000" Checked="true" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadGrid ID="gridInvalidDays" runat="server" GridLines="None" Height="100%" Width="100%"
                    AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                    AutoGenerateColumns="False" AllowMultiRowSelection="false"
                    EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                    ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                    OnNeedDataSource="grid_NeedDataSource" OnItemDataBound="grid_ItemDataBound"
                    CssClass="RadGrid_Customer">
                    <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
                    <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="Top" IsFilterItemExpanded="False"
                        InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                        <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true" RefreshText="查询" ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                        <CommandItemTemplate>
                            <telerik:RadToolBar BorderWidth="0" ID="gridRTB" SkinID="Query" CssClass="RadToolBar_Customer" AutoPostBack="true"
                                runat="server" Width="50%" OnButtonClick="gridRTB_ButtonClick" />
                        </CommandItemTemplate>
                        <ColumnGroups>
                            <telerik:GridColumnGroup Name="时间" HeaderText="时间："
                                HeaderStyle-HorizontalAlign="Center" />
                            <telerik:GridColumnGroup Name="SO2" HeaderText="SO<sub>2</sub>"
                                HeaderStyle-HorizontalAlign="Center">
                            </telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="NO2" HeaderText="NO<sub>2</sub>"
                                HeaderStyle-HorizontalAlign="Center">
                            </telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="PM10" HeaderText="PM<sub>10</sub>"
                                HeaderStyle-HorizontalAlign="Center">
                            </telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="CO" HeaderText="CO"
                                HeaderStyle-HorizontalAlign="Center">
                            </telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="O38" HeaderText="O<sub>3</sub>-8"
                                HeaderStyle-HorizontalAlign="Center">
                            </telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="PM2.5" HeaderText="PM<sub>2.5</sub>"
                                HeaderStyle-HorizontalAlign="Center">
                            </telerik:GridColumnGroup>
                        </ColumnGroups>
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="序号" UniqueName="Row" HeaderStyle-Width="30px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate><%#Container.DataSetIndex + 1%></ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn HeaderText="城市名称" UniqueName="PortName" DataField="PortName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridNumericColumn HeaderText="实际达标天数" UniqueName="SO2ValidDays" DataField="SO2ValidDays" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="SO2" />
                            <telerik:GridBoundColumn HeaderText="无效" UniqueName="SO2InvalidDate" DataField="SO2InvalidDate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="SO2" />
                            <telerik:GridBoundColumn HeaderText="无效天数" UniqueName="SO2InvalidDate" DataField="SO2InvalidDate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="SO2" />
                            <telerik:GridBoundColumn HeaderText="达标率" UniqueName="SO2ValidRate" DataField="SO2ValidRate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="SO2" />
                            <telerik:GridNumericColumn HeaderText="实际达标天数" UniqueName="NO2ValidDays" DataField="NO2ValidDays" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="NO2" />
                            <telerik:GridBoundColumn HeaderText="无效" UniqueName="NO2InvalidDate" DataField="NO2InvalidDate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="NO2" />
                            <telerik:GridBoundColumn HeaderText="无效天数" UniqueName="NO2InvalidDate" DataField="NO2InvalidDate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="NO2" />
                            <telerik:GridBoundColumn HeaderText="达标率" UniqueName="NO2ValidRate" DataField="NO2ValidRate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="NO2" />
                            <telerik:GridNumericColumn HeaderText="实际达标天数" UniqueName="PM10ValidDays" DataField="PM10ValidDays" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="PM10" />
                            <telerik:GridBoundColumn HeaderText="无效" UniqueName="PM10InvalidDate" DataField="PM10InvalidDate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="PM10" />
                            <telerik:GridBoundColumn HeaderText="无效天数" UniqueName="PM10InvalidDate" DataField="PM10InvalidDate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="PM10" />
                            <telerik:GridBoundColumn HeaderText="达标率" UniqueName="PM10ValidRate" DataField="PM10ValidRate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="PM10" />
                            <telerik:GridNumericColumn HeaderText="实际达标天数" UniqueName="COValidDays" DataField="COValidDays" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="CO" />
                            <telerik:GridBoundColumn HeaderText="无效" UniqueName="COInvalidDate" DataField="COInvalidDate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="CO" />
                            <telerik:GridBoundColumn HeaderText="无效天数" UniqueName="COInvalidDate" DataField="COInvalidDate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="CO" />
                            <telerik:GridBoundColumn HeaderText="达标率" UniqueName="COValidRate" DataField="COValidRate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="CO" />
                            <telerik:GridNumericColumn HeaderText="实际达标天数" UniqueName="O38ValidDays" DataField="O38ValidDays" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="O38" />
                            <telerik:GridBoundColumn HeaderText="无效" UniqueName="O38InvalidDate" DataField="O38InvalidDate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="O38" />
                            <telerik:GridBoundColumn HeaderText="无效天数" UniqueName="O38InvalidDate" DataField="O38InvalidDate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="O38" />
                            <telerik:GridBoundColumn HeaderText="达标率" UniqueName="O38ValidRate" DataField="O38ValidRate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="O38" />
                            <telerik:GridNumericColumn HeaderText="实际达标天数" UniqueName="PM25ValidDays" DataField="PM25ValidDays" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="PM2.5" />
                            <telerik:GridBoundColumn HeaderText="无效" UniqueName="PM25InvalidDate" DataField="PM25InvalidDate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="PM2.5" />
                            <telerik:GridBoundColumn HeaderText="无效天数" UniqueName="PM25InvalidDate" DataField="PM25InvalidDate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="PM2.5" />
                            <telerik:GridBoundColumn HeaderText="达标率" UniqueName="PM25ValidRate" DataField="PM25ValidRate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="PM2.5" />
                            <telerik:GridBoundColumn HeaderText="有效监测天数" UniqueName="MonitorDays" DataField="MonitorDays" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="综合达标天数" UniqueName="AllValidDays" DataField="AllValidDays" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="综合达标率" UniqueName="AllValidRate" DataField="AllValidRate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        </Columns>
                        <PagerStyle FirstPageToolTip="首页" AlwaysVisible="true" Position="Bottom" NextPageToolTip="下页" LastPageToolTip="尾页" PrevPageToolTip="上页"
                            PageSizeLabelText="显示记录数:" PagerTextFormat="跳转页面:{4}&amp;nbsp;显示 &lt;strong&gt;{2}&lt;/strong&gt; - &lt;strong&gt;{3}&lt;/strong&gt;条,共 &lt;strong&gt;{5}&lt;/strong&gt;条"></PagerStyle>
                    </MasterTableView>
                    <CommandItemStyle Width="100%" />
                    <ClientSettings>
                        <Scrolling AllowScroll="True" EnableVirtualScrollPaging="false" UseStaticHeaders="True" FrozenColumnsCount="2"
                            SaveScrollPosition="true"></Scrolling>
                    </ClientSettings>
                </telerik:RadGrid>
            </telerik:RadPane>
        </telerik:RadSplitter>
    </form>
</body>
</html>


