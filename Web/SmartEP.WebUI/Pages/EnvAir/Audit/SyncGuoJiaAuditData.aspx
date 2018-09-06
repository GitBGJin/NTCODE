<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SyncGuoJiaAuditData.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Audit.SyncGuoJiaAuditData" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 80px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1"
            UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="btnSync_Click">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rcbPoint" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="rdpBegin" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="rdpEnd" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="rbtCalibration">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridSyncGuoJiaAuditLog" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="rbtCalibration" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
            <%--  <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="RTBUpdate">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridSyncGuoJiaAuditLog" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>--%>
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" Width="100%"
            BorderWidth="0" BorderStyle="None" BorderSize="0">
            <telerik:RadPane ID="paneWhere" runat="server" Height="120px" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="width: 80%; height: 100%" cellspacing="1" cellpadding="0" class="Table_Customer"
                    border="0">
                    <tr>
                        <td class="title" style="width: 80px">同步测点:
                        </td>
                        <td class="content" style="width: 180px;">
                            <telerik:RadComboBox runat="server" ID="rcbPoint" Localization-CheckAllString="全选" Width="360px" CheckBoxes="true" Visible="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                            </telerik:RadComboBox>
                        </td>
                        <td class="auto-style1">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="title" style="width: 80px">同步因子:
                        </td>
                        <td class="content" style="width: 180px;">
                            <telerik:RadComboBox runat="server" ID="rcbFactor" Localization-CheckAllString="全选" Width="360px" CheckBoxes="true" Visible="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                            </telerik:RadComboBox>
                        </td>
                        <td class="auto-style1">&nbsp;</td>
                    </tr>
                    <%--<tr>
                        <td class="title" style="width: 80px">数据类型:
                        </td>
                        <td class="content" style="width: 180px;">
                            <asp:CheckBox ID="cbxAuditHour" runat="server" Checked="true" />审核小时数据&nbsp;
                            <asp:CheckBox ID="cbxAuditDayAQI" runat="server"  Checked="true" />审核日AQI数据&nbsp;
                            <asp:CheckBox ID="cbxAuditCityAQI" runat="server"  Checked="true" />审核全市日AQI数据
                        </td>
                        <td class="auto-style1">&nbsp;</td>
                    </tr>--%>
                    <tr>
                        <td class="title" style="width: 80px">数据时间:
                        </td>
                        <td class="content" style="width: 360px;">
                            <telerik:RadDatePicker ID="rdpBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消" />
                            &nbsp;&nbsp;至&nbsp;&nbsp;
                            <telerik:RadDatePicker ID="rdpEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消" />
                            &nbsp;
                        </td>
                        <td class="auto-style1">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="title" style="width: 80px">&nbsp;
                        </td>
                        <td class="content" style="width: 180px;">
                            <asp:CheckBox runat="server" Checked="true" ForeColor="Red" ID="CoverData" Text="覆盖原有日数据" ToolTip="勾选后将覆盖原有日数据" Visible="false" />&nbsp;&nbsp;
                            <telerik:RadButton ID="rbtnSync" runat="server" BackColor="#3A94D3" ForeColor="White" AutoPostBack="true" OnClick="btnSync_Click">
                                <ContentTemplate>
                                    <asp:Label runat="server" ID="lblSync" ForeColor="White" Text="同步数据"></asp:Label>
                                </ContentTemplate>
                            </telerik:RadButton>
                            <telerik:RadButton ID="rbtCalibration" runat="server" BackColor="#3A94D3" ForeColor="White" AutoPostBack="true" OnClick="rbtCalibration_Click">
                                <ContentTemplate>
                                    <asp:Label runat="server" ID="lblCalibration" ForeColor="White" Text="一致性检查"></asp:Label>
                                </ContentTemplate>
                            </telerik:RadButton>
                        </td>

                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadGrid ID="gridSyncGuoJiaAuditLog" runat="server" GridLines="None" Height="100%" Width="100%"
                    AllowPaging="false" PageSize="24" PagerStyle-ShowPagerText="false" AllowCustomPaging="false" AllowSorting="false" ShowFooter="false"
                    AutoGenerateColumns="false" AllowMultiRowSelection="false"
                    EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                    ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                    OnNeedDataSource="gridSyncGuoJiaAuditLog_NeedDataSource" OnItemDataBound="gridSyncGuoJiaAuditLog_ItemDataBound"
                    CssClass="RadGrid_Customer">
                    <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
                    <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False"
                        InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="数据一致！">
                        <%--    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true" RefreshText="查询" ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                        <CommandItemTemplate>
                            <telerik:RadButton ID="RTBUpdate" runat="server" BackColor="#3A94D3" ForeColor="White" AutoPostBack="true" OnClick="RTBUpdate_Click">
                                <ContentTemplate>
                                    <asp:Label runat="server" ID="lblCalibration" ForeColor="White" Text="数据更新"></asp:Label>
                                </ContentTemplate>
                            </telerik:RadButton>
                        </CommandItemTemplate>--%>

                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="序号" UniqueName="rowNum" HeaderStyle-Width="50px" HeaderStyle-Height="50px"
                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%#Container.DataSetIndex + 1%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn HeaderText="点位名称" UniqueName="PointId" DataField="PointId" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="项目" UniqueName="PollutantName" DataField="PollutantName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="时间点" UniqueName="TimePoint" DataField="TimePoint" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="操作" UniqueName="VerifyDataType" DataField="VerifyDataType" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="原始值" UniqueName="SrcValue" DataField="SrcValue" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="审核值" UniqueName="VerifyValueMark" DataField="VerifyValueMark" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="审核人" UniqueName="VerifyPerson" DataField="VerifyPerson" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="审核时间" UniqueName="VerifyTime" DataField="VerifyTime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="审核批注" UniqueName="Description" DataField="Description" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>

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
