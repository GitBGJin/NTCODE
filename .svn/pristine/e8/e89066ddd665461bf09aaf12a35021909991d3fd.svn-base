<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CityDayAnalyze_Year.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.DataAnalyze.CityDayAnalyze_Year" %>

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
                <telerik:AjaxSetting AjaxControlID="grdCityYear">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdCityYear" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridRTB">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdCityYear" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="splitter">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdCityYear" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdCityYear" LoadingPanelID="RadAjaxLoadingPanel1" />
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
                        <td class="title" style="width: 120px; text-align: center;">区域</td>
                        <td class="content">
                            <telerik:RadComboBox runat="server" ID="comboCityProper" EmptyMessage="选择区域" Localization-CheckAllString="全选" Width="180px" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                                <Items>
                                    <telerik:RadComboBoxItem Text="苏州市区" Value="7e05b94c-bbd4-45c3-919c-42da2e63fd43" Checked="true" />
                                    <telerik:RadComboBoxItem Text="吴江区" Value="2fea3cb2-8b95-45e6-8a71-471562c4c89c" Checked="true" />
                                    <telerik:RadComboBoxItem Text="昆山市" Value="2e2950cd-dbab-43b3-811d-61bd7569565a" Checked="true" />
                                    <telerik:RadComboBoxItem Text="太仓市" Value="57b196ed-5038-4ad0-a035-76faee2d7a98" Checked="true" />
                                    <telerik:RadComboBoxItem Text="常熟市" Value="d7d7a1fe-493a-4b3f-8504-b1850f6d9eff" Checked="true" />
                                    <telerik:RadComboBoxItem Text="张家港市" Value="66d2abd1-ca39-4e39-909f-da872704fbfd" Checked="true" />
                                    <%-- <telerik:RadComboBoxItem Text="吴中区" Value="e1c104f3-aaf3-4d0e-9591-36cdc83be15a" Checked="true" />
                                    <telerik:RadComboBoxItem Text="相城区" Value="8756bd44-ff18-46f7-aedf-615006d7474c" Checked="true" />
                                    <telerik:RadComboBoxItem Text="姑苏区" Value="6a4e7093-f2c6-46b4-a11f-0f91b4adf379" Checked="true" />
                                    <telerik:RadComboBoxItem Text="高新区" Value="f320aa73-7c55-45d4-a363-e21408e0aac3" Checked="true" />
                                    <telerik:RadComboBoxItem Text="工业园区" Value="69a993ff-78c6-459b-9322-ee77e0c8cd68" Checked="true" />
                                    <telerik:RadComboBoxItem Text="苏州大市" Value="1" Checked="true" />--%>
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                        <td class="title" style="width: 120px; text-align: center;">选择年份
                        </td>
                        <td class="content">
                            <telerik:RadDropDownList runat="server" ID="ddlYear">
                            </telerik:RadDropDownList>
                        </td>
                        <td class="content" align="left">
                            <asp:ImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadGrid ID="grdCityYear" runat="server" GridLines="None" Height="100%" Width="100%"
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
                            <div style="position: relative;">
                                <telerik:RadToolBar BorderWidth="0" ID="gridRTB" SkinID="Query" CssClass="RadToolBar_Customer" AutoPostBack="true"
                                    runat="server" Width="50%" OnButtonClick="gridRTB_ButtonClick" />
                                <div style="position: absolute; right: 10px; top: 5px;">
                                    <table style="font-weight: bold;">
                                        <tr>
                                            <td>单位（mg/m³）</td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </CommandItemTemplate>
                        <ColumnGroups>
                            <telerik:GridColumnGroup Name="日均值" HeaderText="日均值"
                                HeaderStyle-HorizontalAlign="Center" />
                            <telerik:GridColumnGroup Name="年均值" HeaderText="年均值"
                                HeaderStyle-HorizontalAlign="Center" />
                        </ColumnGroups>
                        <Columns>
                            <%--<telerik:GridTemplateColumn HeaderText="序号" UniqueName="Row" HeaderStyle-Width="30px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" >
                                <ItemTemplate><%#Container.DataSetIndex + 1%></ItemTemplate>
                            </telerik:GridTemplateColumn>--%>
                            <telerik:GridBoundColumn HeaderText="地区" UniqueName="RegionName" DataField="RegionName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
                            <telerik:GridBoundColumn HeaderText="监测指标" UniqueName="FactorName" DataField="FactorName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="最小值" UniqueName="DayMinValue" DataField="DayMinValue" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="日均值" />
                            <telerik:GridBoundColumn HeaderText="最大值" UniqueName="DayMaxValue" DataField="DayMaxValue" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="日均值" />
                            <telerik:GridBoundColumn HeaderText="超标天数" UniqueName="OutDays" DataField="OutDays" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="日均值" />
                            <telerik:GridBoundColumn HeaderText="监测天数" UniqueName="MonitorDays" DataField="MonitorDays" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="日均值" />
                            <telerik:GridBoundColumn HeaderText="超标率" UniqueName="OutRate" DataField="OutRate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="日均值" />
                            <telerik:GridBoundColumn HeaderText="最大超标倍数" UniqueName="OutBiggestFactor" DataField="OutBiggestFactor" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="日均值" />
                            <telerik:GridBoundColumn HeaderText="年均值浓度" UniqueName="YearAverage" DataField="YearAverage" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="年均值" />
                            <telerik:GridBoundColumn HeaderText="年均值超标倍数" UniqueName="YearOutRate" DataField="YearOutRate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="年均值" />
                            <telerik:GridBoundColumn HeaderText="百分位数浓度" UniqueName="YearPercent" DataField="YearPercent" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="年均值" />
                            <telerik:GridBoundColumn HeaderText="百分位数超标倍数" UniqueName="YearPerOutRate" DataField="YearPerOutRate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ColumnGroupName="年均值" />
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
            </telerik:RadPane>
        </telerik:RadSplitter>
    </form>
</body>
</html>
