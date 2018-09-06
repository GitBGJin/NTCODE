<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PollutantAvgDayRange.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.DataAnalyze.PollutantAvgDayRange" %>

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
                        var MasterTable = $find("<%= grdAvgDayRange.ClientID %>").get_masterTableView();
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
                //按钮行处理
                function gridRTB_ClientButtonClicking(sender, args) {
                    var masterTable = $find("<%= grdAvgDayRange.ClientID %>").get_masterTableView();
                    var CurrentBtn = args.get_item();
                    var CurrentBtnName = CurrentBtn.get_text();
                    var CurrentBtnCommandName = CurrentBtn.get_commandName();
                    switch (CurrentBtnCommandName) {
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
                    args.set_cancel(!OnClientClicking());
                }
            </script>
        </telerik:RadScriptBlock>
        <input id="radGridColWidth" type="hidden" runat="server" value="120" />
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="grdAvgDayRange">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdAvgDayRange" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridRTB">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdAvgDayRange" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="splitter">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdAvgDayRange" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdAvgDayRange" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rbtnlType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rbtnlType" />
                        <telerik:AjaxUpdatedControl ControlID="dvPoint" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dvCity" LoadingPanelID="RadAjaxLoadingPanel1" />
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
                        <td class="title" style="width: 120px; text-align: center;">查询范围:
                        </td>
                        <td class="content" style="width: 320px;">
                            <asp:RadioButtonList ID="rbtnlType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rbtnlType_SelectedIndexChanged" RepeatLayout="Flow" RepeatColumns="5">
                                <asp:ListItem Text="区域" Value="CityProper" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="测点" Value="Port"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td class="title" style="width: 120px; text-align: center;">查询区域/测点:
                        </td>
                        <td class="content" style="width: 300px;">
                            <div runat="server" id="dvCity">
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
                            </div>
                            <div runat="server" id="dvPoint" style="display: none">
                                <CbxRsm:PointCbxRsm runat="server" ApplicationType="Air" CbxWidth="220" CbxHeight="350" MultiSelected="true" DropDownWidth="520" ID="pointCbxRsm"></CbxRsm:PointCbxRsm>
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
                                    <telerik:RadComboBoxItem Text="O3_8h" Value="Max8HourO3" Checked="true" />
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
                        <td class="content" style="width: 300px;">
                            <telerik:RadDatePicker ID="rdpBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px" Width="105px"
                                DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-CancelButtonCaption="取消" />
                        </td>
                        <td class="title" style="width: 80px; text-align: center;">结束时间:
                        </td>
                        <td class="content" style="width: 300px;">
                            <telerik:RadDatePicker ID="rdpEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd"
                                DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" Width="105px"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-CancelButtonCaption="取消" />
                        </td>
                        <td></td>
                        <td class="content" style="width: 70px;">
                            <asp:CheckBox ID="IsStatistical" Checked="false" Text="统计行" runat="server" />
                        </td>
                    </tr>

                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadGrid ID="grdAvgDayRange" runat="server" GridLines="None" Height="100%" Width="100%"
                    AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                    AutoGenerateColumns="true" AllowMultiRowSelection="false"
                    EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                    ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                    OnNeedDataSource="grid_NeedDataSource" OnItemDataBound="grid_ItemDataBound" OnColumnCreated="grdAvgDayRange_ColumnCreated"
                    CssClass="RadGrid_Customer">
                    <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
                    <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="Top" IsFilterItemExpanded="False"
                        InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                        <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true" RefreshText="查询" ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                        <CommandItemTemplate>
                            <telerik:RadToolBar BorderWidth="0" ID="gridRTB" SkinID="Query" CssClass="RadToolBar_Customer" AutoPostBack="true"
                                runat="server" Width="50%" OnButtonClick="gridRTB_ButtonClick" OnClientButtonClicking="gridRTB_ClientButtonClicking" />
                        </CommandItemTemplate>
                        <ColumnGroups>
                            <telerik:GridColumnGroup Name="PM25Range" HeaderText="PM<sub>2.5</sub>" HeaderStyle-HorizontalAlign="Center" />
                            <telerik:GridColumnGroup Name="PM10Range" HeaderText="PM<sub>10</sub>" HeaderStyle-HorizontalAlign="Center">
                            </telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="NO2Range" HeaderText="NO<sub>2</sub>" HeaderStyle-HorizontalAlign="Center">
                            </telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="SO2Range" HeaderText="SO<sub>2</sub>" HeaderStyle-HorizontalAlign="Center">
                            </telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="CO" HeaderText="CO" HeaderStyle-HorizontalAlign="Center">
                            </telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="Max8HourO3Range" HeaderText="O<sub>3</sub>-8小时" HeaderStyle-HorizontalAlign="Center">
                            </telerik:GridColumnGroup>
                            <%--        <telerik:GridColumnGroup Name="O3-1小时" HeaderText="O<sub>3</sub>-1小时" HeaderStyle-HorizontalAlign="Center">
                            </telerik:GridColumnGroup>--%>
                        </ColumnGroups>
                        <%--      <Columns>
                            <telerik:GridDateTimeColumn HeaderText="日期" UniqueName="DateTime" DataField="DateTime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" DataFormatString="{0:yyyy-MM}" />
                            <telerik:GridBoundColumn HeaderText="点位/区域" UniqueName="RegionName" DataField="RegionName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" />
                            <telerik:GridNumericColumn HeaderText="μg/m³" UniqueName="PM25Range" DataField="PM25Range" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="PM2.5" HeaderStyle-Width="105px" />
                            <telerik:GridNumericColumn HeaderText="AQI" UniqueName="PM25AQI" DataField="PM25AQI" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="PM2.5" HeaderStyle-Width="105px" />
                            <telerik:GridNumericColumn HeaderText="μg/m³" UniqueName="PM10Range" DataField="PM10Range" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="PM10" HeaderStyle-Width="105px" />
                            <telerik:GridNumericColumn HeaderText="AQI" UniqueName="PM10AQI" DataField="PM10AQI" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="PM10" HeaderStyle-Width="105px" />
                            <telerik:GridNumericColumn HeaderText="μg/m³" UniqueName="NO2Range" DataField="NO2Range" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="NO2" HeaderStyle-Width="105px" />
                            <telerik:GridNumericColumn HeaderText="AQI" UniqueName="NO2AQI" DataField="NO2AQI" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="NO2" HeaderStyle-Width="105px" />
                            <telerik:GridNumericColumn HeaderText="μg/m³" UniqueName="SO2Range" DataField="SO2Range" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="SO2" HeaderStyle-Width="105px" />
                            <telerik:GridNumericColumn HeaderText="AQI" UniqueName="SO2AQI" DataField="SO2AQI" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="SO2" HeaderStyle-Width="105px" />
                            <telerik:GridNumericColumn HeaderText="mg/m³" UniqueName="CORange" DataField="CORange" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="CO" HeaderStyle-Width="105px" />
                            <telerik:GridNumericColumn HeaderText="AQI" UniqueName="COAQI" DataField="COAQI" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="CO" HeaderStyle-Width="105px" />
                            <telerik:GridNumericColumn HeaderText="μg/m³" UniqueName="Max8HourO3Range" DataField="Max8HourO3Range" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="O3-8小时" HeaderStyle-Width="105px" />
                            <telerik:GridNumericColumn HeaderText="AQI" UniqueName="Max8HourO3AQI" DataField="Max8HourO3AQI" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="O3-8小时" HeaderStyle-Width="105px" />
                            <telerik:GridNumericColumn HeaderText="μg/m³" UniqueName="MaxOneHourO3Range" DataField="MaxOneHourO3Range" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="O3-1小时" HeaderStyle-Width="105px" />
                            <telerik:GridNumericColumn HeaderText="AQI" UniqueName="MaxOneHourO3AQI" DataField="MaxOneHourO3AQI" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="O3-1小时" HeaderStyle-Width="105px" />
                        </Columns>--%>
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
