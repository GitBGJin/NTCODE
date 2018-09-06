<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MutilPointAuditDataWX.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Audit.MutilPointAuditDataWX" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="FactorRsmAuditWX" Src="~/Controls/FactorRsmAuditWX.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link type="text/css" rel="stylesheet" href="../../../App_Themes/Neptune/Telerik.Web.UI.Skins/Metro/RadSplitterAudit.css" />
    <style type="text/css">
        /*#gridAuditData_GridData {
                 height:100% !important;
             }*/
        div.RemoveHorizontalBorders {
            border-width: 1px 0;
        }

        .rgActiveCell {
            background-color: #FFD583 !important;
        }

        .rgAltRow {
            text-align: center !important;
            vertical-align: middle !important;
        }

        .rgGroupCol {
            background: none !important;
            border: solid 0px !important;
            /*padding-left: 0 !important;
            padding-right: 0 !important;
            font-size: 1px !important;*/
        }

        /*.rgGroupHeader {
        }*/

        /*.rgGroupCol {
            padding-left: 0 !important;
            padding-right: 0 !important;
            font-size: 1px !important;
        }

        .rgExpand,
        .rgCollapse {
            display: none !important;
        }*/
    </style>
</head>
<body scroll="no">
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" UpdatePanelHeight="100%">
        </telerik:RadAjaxLoadingPanel>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" EnablePageHeadUpdate="false">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="typeList">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="typeList" />
                        <telerik:AjaxUpdatedControl ControlID="RadPortTree" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="dataSubmit">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridAuditData"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="auditChart"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="auditState"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="chartFactorRadio">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="chartFactorRadio"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="auditChart"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="factorNames"></telerik:AjaxUpdatedControl>

                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="refreshData">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridAuditData"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="auditChart"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="factorNames"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="PointIDHidden"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="SubmitAudit">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridAuditData"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="auditChart"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="auditState"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="pagediv" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="RadMenuData">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridAuditData"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="auditChart"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridAuditData">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridAuditData"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="factorNames"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="PointIDHidden"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="radioPoint"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ButtonDiv"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="auditChart"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="factorNames">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="auditChart"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="AuditType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="AuditType"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="radioPoint"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="factorCbxRsm"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="factorNames"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="PointIDHidden"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="Search">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="chartFactorRadio"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="factorNames"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="PointIDHidden"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="gridAuditData"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="auditChart"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="auditState"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="pagediv" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
            <ClientEvents OnRequestStart="onRequestStart" OnResponseEnd="onResponseEnd" />
        </telerik:RadAjaxManager>
        <div runat="server" id="pagediv" style="position: absolute; z-index: -1;"></div>
        <telerik:RadSplitter runat="server" ID="splitterContent" BorderSize="0" Width="100%" Height="100%" OnClientLoad="loadSplitter">
            <!-- 左侧测点、因子-->
            <telerik:RadPane runat="server" ID="LeftPane" Width="20%">
                <%--<div>
                    <div style="width: 100%; height: 28px; color: #fff; background-color: #3A94D3;">
                        <div style="padding-top: 6px; padding-left: 6px;">测点</div>
                    </div>
                    <asp:RadioButtonList runat="server" ID="AuditType" AutoPostBack="true" RepeatDirection="Horizontal" RepeatColumns="2" OnSelectedIndexChanged="AuditType_SelectedIndexChanged">
                    </asp:RadioButtonList>
                    <div style="width: 100%; height: 2px; color: #fff; background-color: #3A94D3;">
                    </div>
                    <div style="padding-top: 10px;">
                        <table>
                            <tr>
                                <td>
                                    <telerik:RadButton runat="server" ID="selectAll" BackColor="#3A94D3" ForeColor="White" AutoPostBack="false" ToolTip="全选" OnClientClicked="checkAll" OnClick="selectAll_Click">
                                        <ContentTemplate>
                                            <asp:Label runat="server" ID="Label4" ForeColor="White" Text="全选"></asp:Label>
                                        </ContentTemplate>
                                    </telerik:RadButton>
                                    <telerik:RadButton runat="server" ID="inverse" BackColor="#3A94D3" ForeColor="White" AutoPostBack="false" ToolTip="反选" OnClientClicked="ReverseAll" OnClick="inverse_Click">
                                        <ContentTemplate>
                                            <asp:Label runat="server" ID="Label4" ForeColor="White" Text="反选"></asp:Label>
                                        </ContentTemplate>
                                    </telerik:RadButton>
                                    <telerik:RadButton runat="server" ID="unselect" Visible="false" BackColor="#3A94D3" ForeColor="White" AutoPostBack="false" ToolTip="不选" OnClientClicked="deleteAll" OnClick="unselect_Click">
                                        <ContentTemplate>
                                            <asp:Label runat="server" ID="Label4" ForeColor="White" Text="不选"></asp:Label>
                                        </ContentTemplate>
                                    </telerik:RadButton>
                                </td>
                            </tr>
                        </table>


                        <asp:CheckBoxList runat="server" ID="radioPoint" AutoPostBack="false" RepeatDirection="Horizontal" RepeatColumns="2" OnSelectedIndexChanged="radioPoint_SelectedIndexChanged"></asp:CheckBoxList>
                    </div>
                </div>
                <div style="padding-top: 10px;">
                    <div style="width: 100%; height: 28px; color: #fff; background-color: #3A94D3;">
                        <div style="padding-top: 6px; padding-left: 6px;">因子</div>
                    </div>
                    <CbxRsm:FactorRsmAuditWX runat="server" ApplicationType="Air" ID="factorCbxRsm"></CbxRsm:FactorRsmAuditWX>
                </div>
                <div style="padding-top: 10px;">
                    <div style="width: 100%; height: 28px; color: #fff; background-color: #3A94D3;">
                        <div style="padding-top: 6px; padding-left: 6px;">数据标识</div>
                    </div>
                    <telerik:RadGrid ID="StatusGrid" runat="server" GridLines="None" Width="80%"
                        AllowPaging="false"
                        AutoGenerateColumns="false" AllowMultiRowSelection="false"
                        EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                        ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="1" ShowFooter="false"
                        ShowHeader="false" OnNeedDataSource="StatusGrid_NeedDataSource" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                        <MasterTableView GridLines="None" CommandItemDisplay="None" IsFilterItemExpanded="False" NoMasterRecordsText="没有数据">
                            <Columns>
                                <telerik:GridBoundColumn UniqueName="StatusIdentify" DataField="StatusIdentify"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="StatusName" DataField="StatusName"></telerik:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                        <CommandItemStyle Width="100%" />
                    </telerik:RadGrid>
                </div>--%>
                <telerik:RadSlidingZone ID="RadSlidingZone1" runat="server" DockedPaneId="RadSlidingPane_Point" BorderSize="0">
                    <telerik:RadSlidingPane ID="RadSlidingPane_Point" Width="200" runat="server" Title="测点" UndockText="收缩" DockText="固定" CollapseText="关闭"
                        OnClientExpanding="HidePanel" OnClientBeforeDock="HidePanel" OnClientBeforeUndock="ShowPanel" OnClientCollapsed="ShowPanel"
                        EnableDock="true">
                        <div>
                           <%-- <div style="width: 100%; height: 28px; color: #fff; background-color: #3A94D3;">
                                <div style="padding-top: 6px; padding-left: 6px;">测点</div>
                            </div>--%>
                            <asp:RadioButtonList runat="server" ID="AuditType" AutoPostBack="true" RepeatDirection="Horizontal" RepeatColumns="2" OnSelectedIndexChanged="AuditType_SelectedIndexChanged">
                            </asp:RadioButtonList>
                            <div style="width: 100%; height: 2px; color: #fff; background-color: #3A94D3;">
                            </div>
                            <div style="padding-top: 10px;">
                                <table>
                                    <tr>
                                        <td>
                                            <telerik:RadButton runat="server" ID="selectAll" BackColor="#3A94D3" ForeColor="White" AutoPostBack="false" ToolTip="全选" OnClientClicked="checkAll" OnClick="selectAll_Click">
                                                <ContentTemplate>
                                                    <asp:Label runat="server" ID="Label4" ForeColor="White" Text="全选"></asp:Label>
                                                </ContentTemplate>
                                            </telerik:RadButton>
                                            <telerik:RadButton runat="server" ID="inverse" BackColor="#3A94D3" ForeColor="White" AutoPostBack="false" ToolTip="反选" OnClientClicked="ReverseAll" OnClick="inverse_Click">
                                                <ContentTemplate>
                                                    <asp:Label runat="server" ID="Label4" ForeColor="White" Text="反选"></asp:Label>
                                                </ContentTemplate>
                                            </telerik:RadButton>
                                            <telerik:RadButton runat="server" ID="unselect" Visible="false" BackColor="#3A94D3" ForeColor="White" AutoPostBack="false" ToolTip="不选" OnClientClicked="deleteAll" OnClick="unselect_Click">
                                                <ContentTemplate>
                                                    <asp:Label runat="server" ID="Label4" ForeColor="White" Text="不选"></asp:Label>
                                                </ContentTemplate>
                                            </telerik:RadButton>
                                        </td>
                                    </tr>
                                </table>
                                <asp:CheckBoxList runat="server" ID="radioPoint" AutoPostBack="false" RepeatDirection="Horizontal" RepeatColumns="2" OnSelectedIndexChanged="radioPoint_SelectedIndexChanged"></asp:CheckBoxList>
                            </div>
                        </div>
                        <div style="padding-top: 10px;">
                            <div style="width: 100%; height: 28px; color: #fff; background-color: #3A94D3;">
                                <div style="padding-top: 6px; padding-left: 6px;">因子</div>
                            </div>
                            <CbxRsm:FactorRsmAuditWX runat="server" ApplicationType="Air" ID="factorCbxRsm"></CbxRsm:FactorRsmAuditWX>
                        </div>
                        <div style="padding-top: 10px;">
                            <div style="width: 100%; height: 28px; color: #fff; background-color: #3A94D3;">
                                <div style="padding-top: 6px; padding-left: 6px;">数据标识</div>
                            </div>
                            <telerik:RadGrid ID="StatusGrid" runat="server" GridLines="None" Width="100%"
                                AllowPaging="false"
                                AutoGenerateColumns="false" AllowMultiRowSelection="false"
                                EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                                ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="1" ShowFooter="false"
                                ShowHeader="false" OnNeedDataSource="StatusGrid_NeedDataSource" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                <MasterTableView GridLines="None" CommandItemDisplay="None" IsFilterItemExpanded="False" NoMasterRecordsText="没有数据">
                                    <Columns>
                                        <telerik:GridBoundColumn UniqueName="StatusIdentify" DataField="StatusIdentify"></telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="StatusName" DataField="StatusName"></telerik:GridBoundColumn>
                                    </Columns>
                                </MasterTableView>
                                <CommandItemStyle Width="100%" />
                            </telerik:RadGrid>
                        </div>
                    </telerik:RadSlidingPane>
                </telerik:RadSlidingZone>
            </telerik:RadPane>
            <!--中间-->
            <telerik:RadPane ID="MiddlePane" runat="server" Scrolling="None" Height="100%" BorderSize="0" Width="78%">
                <telerik:RadSplitter ID="RadSplitter1" runat="server" Orientation="Horizontal" BorderSize="0" Height="100%">
                    <!--按钮及表格-->
                    <telerik:RadPane ID="RadPane2" runat="server" Scrolling="None" Width="100%" Height="58%" OnClientCollapsed="GridClientCollapsed" OnClientExpanded="ChartClientExpanded">
                        <!--按钮-->
                        <div id="ButtonDiv" style="height: 66px;">
                            <table style="text-align: left; width: 100%;">
                                <tr>
                                    <td>
                                        <div style="float: left;">
                                            <%--  <asp:Label runat="server" ID="portName" ForeColor="Black" Font-Bold="true" Font-Size="14"></asp:Label>--%>
                                            <asp:Label ID="Label1" runat="server" ForeColor="Black" Font-Bold="true" Font-Size="14" Text="审核状态："></asp:Label>
                                        </div>
                                        <div style="float: left;">
                                            <asp:Label runat="server" ID="auditState" ForeColor="Red" Font-Bold="true" Font-Size="14"></asp:Label>
                                        </div>
                                    </td>
                                    <td style="width: 60px;">日期：</td>
                                    <td style="width: 130px;">
                                        <telerik:RadDatePicker runat="server" Width="100%" ID="RadDatePickerBegin" AutoPostBack="false"
                                            MinDate="1900-01-01" DateInput-Font-Size="10" DateInput-Font-Bold="false" DateInput-DateFormat="yyyy-MM-dd"
                                            DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                            Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                            Calendar-FastNavigationSettings-CancelButtonCaption="取消" Timeview-HeaderText="小时"
                                            ClientEvents-OnDateSelected="BeginDateSelected" DateInput-ClientEvents-OnValueChanging="BeginDateChanging" OnSelectedDateChanged="RadDatePickerBegin_SelectedDateChanged">
                                        </telerik:RadDatePicker>
                                    </td>
                                    <td>至</td>
                                    <td style="width: 130px;">
                                        <telerik:RadDatePicker runat="server" Width="100%" ID="RadDatePickerEnd" AutoPostBack="false"
                                            MinDate="1900-01-01" DateInput-Font-Size="10" DateInput-Font-Bold="false" DateInput-DateFormat="yyyy-MM-dd"
                                            DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                            Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                            Calendar-FastNavigationSettings-CancelButtonCaption="取消" Timeview-HeaderText="小时"
                                            ClientEvents-OnDateSelected="BeginDateSelected" DateInput-ClientEvents-OnValueChanging="EndDateChanging" OnSelectedDateChanged="RadDatePickerBegin_SelectedDateChanged">
                                        </telerik:RadDatePicker>
                                    </td>
                                    <td style="width: 70px;">
                                        <telerik:RadButton ID="Search" runat="server" BackColor="#3A94D3" Visible="true" ForeColor="White" AutoPostBack="true" OnClientClicking="SearchClicking" OnClick="Search_Click">
                                            <ContentTemplate>
                                                <asp:Label ID="Label2" runat="server" ForeColor="White" Text="查询"></asp:Label>
                                            </ContentTemplate>
                                        </telerik:RadButton>
                                    </td>
                                    <td id="SubmitAuditTD" style="width: 80px;" runat="server">
                                        <telerik:RadButton ID="SubmitAudit" runat="server" BackColor="#3A94D3" ForeColor="White" AutoPostBack="true" OnClientClicking="SubmitAuditClicked" OnClick="SubmitAudit_Click">
                                            <ContentTemplate>
                                                <asp:Label runat="server" ID="Label4" ForeColor="White" Text="审核提交"></asp:Label>
                                            </ContentTemplate>
                                        </telerik:RadButton>
                                    </td>
                                    <td style="width: 70px;">
                                        <telerik:RadButton ID="Back" runat="server" BackColor="#3A94D3" ForeColor="White" AutoPostBack="true" OnClick="Back_Click">
                                            <ContentTemplate>
                                                <asp:Label runat="server" ID="Label4" ForeColor="White" Text="返回"></asp:Label>
                                            </ContentTemplate>
                                        </telerik:RadButton>
                                    </td>
                                </tr>
                            </table>
                            <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                                <table style="width: 100%; text-align: left;">
                                    <tr>
                                        <td style="width: 60%; text-align: left;">
                                            <table>
                                                <tr>
                                                    <td style="vertical-align: Top;">
                                                        <div style="width: 14px; height: 14px; background-color: <%=getAuditConfigColor(0)%>" />
                                                    </td>
                                                    <td style="text-align: left; vertical-align: bottom;">超标异常</td>
                                                    <td style="vertical-align: Top;" id="tdUpperColor">
                                                        <div style="width: 14px; height: 14px; background-color: <%=getAuditConfigColor(1)%>" />
                                                    </td>
                                                    <td style="text-align: left; vertical-align: bottom;" id="tdUpperText">PM10、PM2.5倒挂</td>
                                                </tr>
                                            </table>
                                        </td>

                                        <td style="width: 40%;">
                                            <table style="text-align: right; width: 100%;">
                                                <tr>
                                                    <td></td>
                                                    <td style="width: 100px;">
                                                        <asp:CheckBox runat="server" ID="IsShowTotal" AutoPostBack="false" Text="显示统计信息" OnCheckedChanged="IsShowTotal_CheckedChanged" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </telerik:RadCodeBlock>
                        </div>
                        <!--表格-->
                        <div id="GridDiv">
                            <telerik:RadGrid ID="gridAuditData" runat="server" GridLines="None"
                                AllowPaging="false"
                                AutoGenerateColumns="false" AllowMultiRowSelection="false"
                                EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                                ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0" ShowFooter="false"
                                CssClass="RadGrid_Customer" OnNeedDataSource="gridAuditData_NeedDataSource"
                                OnLoad="gridAuditData_Load" OnItemDataBound="gridAuditData_ItemDataBound">
                                <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
                                <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False" ClientDataKeyNames="PointID,DataDateTime"
                                    InsertItemPageIndexAction="ShowItemOnCurrentPage" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" NoMasterRecordsText="" EditMode="Batch" HeaderStyle-HorizontalAlign="Center" GroupLoadMode="Client" GroupHeaderItemStyle-BackColor="#3A94D3" GroupHeaderItemStyle-ForeColor="White">
                                    <BatchEditingSettings EditType="Cell" OpenEditingEvent="DblClick" />
                                    <GroupByExpressions>
                                        <telerik:GridGroupByExpression>
                                            <SelectFields>
                                                <telerik:GridGroupByField FieldName="PointID" HeaderText="PointID" />
                                            </SelectFields>
                                            <GroupByFields>
                                                <telerik:GridGroupByField FieldName="PointID" SortOrder="Descending" />
                                            </GroupByFields>
                                        </telerik:GridGroupByExpression>
                                    </GroupByExpressions>
                                </MasterTableView>
                                <CommandItemStyle Width="100%" />
                                <ClientSettings AllowKeyboardNavigation="true">
                                    <Selecting UseClientSelectColumnOnly="true" CellSelectionMode="MultiCell" />
                                    <ClientEvents OnRowContextMenu="RowContextMenu" OnCellSelecting="cellSelecting"
                                        OnCellDeselecting="cellDeselecting" OnCellSelected="CellSelected" OnCellDeselected="CellDeselected"
                                        OnBatchEditCellValueChanged="BatchEditCellValueChanged" OnBatchEditOpening="BatchEidtOpening"
                                        OnBatchEditClosing="BatchEditClosing" OnBatchEditClosed="BatchEditClosed" />
                                    <Scrolling AllowScroll="True" UseStaticHeaders="true" />
                                </ClientSettings>
                            </telerik:RadGrid>
                            <telerik:RadContextMenu ID="RadMenuData" runat="server" EnableRoundedCorners="true" EnableShadows="true"
                                OnClientItemClicked="RadContextMenuClicked">
                                <Items>
                                    <telerik:RadMenuItem Text="无效" ToolTip="置为无效(不参加计算)" />
                                    <telerik:RadMenuItem Text="停电" ToolTip="置为停电" Visible="true" />
                                    <telerik:RadMenuItem Text="质控" ToolTip="置为质控" Visible="true" />
                                    <telerik:RadMenuItem Text="有效" ToolTip="置为有效(参加计算)" />
                                    <telerik:RadMenuItem Text="恢复" ToolTip="恢复至原始数据" />
                                </Items>
                            </telerik:RadContextMenu>
                            <telerik:RadContextMenu ID="RadMenuAuditLog" runat="server" EnableRoundedCorners="true" EnableShadows="true"
                                OnClientItemClicked="RadContextMenuClicked">
                                <Items>
                                    <telerik:RadMenuItem Text="无效" ToolTip="置为无效(不参加计算)" />
                                    <telerik:RadMenuItem Text="停电" ToolTip="置为停电" Visible="true" />
                                    <telerik:RadMenuItem Text="质控" ToolTip="置为质控" Visible="true" />
                                    <telerik:RadMenuItem Text="有效" ToolTip="置为有效(参加计算)" />
                                    <telerik:RadMenuItem Text="恢复" ToolTip="恢复至原始数据" />
                                    <telerik:RadMenuItem Text="互动信息" ToolTip="查看审核记录" />
                                    <telerik:RadMenuItem Text="查看因子数据" ToolTip="查看所有点位的因子数据" />
                                </Items>
                            </telerik:RadContextMenu>
                            <telerik:RadContextMenu ID="ContextMenuChart" runat="server" EnableRoundedCorners="true" EnableShadows="true"
                                OnClientItemClicked="RadContextMenuClicked">
                                <Items>
                                    <telerik:RadMenuItem Text="修改" ToolTip="修改数据" />
                                    <telerik:RadMenuItem Text="无效" ToolTip="置为无效(不参加计算)" />
                                    <telerik:RadMenuItem Text="停电" ToolTip="置为停电" Visible="true" />
                                    <telerik:RadMenuItem Text="质控" ToolTip="置为质控" Visible="true" />
                                    <telerik:RadMenuItem Text="有效" ToolTip="置为有效(参加计算)" />
                                    <telerik:RadMenuItem Text="恢复" ToolTip="恢复至原始数据" />
                                    <telerik:RadMenuItem Text="互动信息" ToolTip="查看审核记录" />
                                    <telerik:RadMenuItem Text="查看因子数据" ToolTip="查看所有点位的因子数据" />
                                </Items>
                            </telerik:RadContextMenu>
                        </div>
                    </telerik:RadPane>
                    <telerik:RadSplitBar ID="RadSplitBar1" runat="server" CollapseMode="Both" Width="100%">
                    </telerik:RadSplitBar>
                    <telerik:RadPane ID="RadPane3" runat="server" Scrolling="None" Width="100%" Height="40%" OnClientCollapsed="ChartPanelCollapsed" OnClientExpanded="ChartClientExpanded" OnClientResized="SpliterResized">
                        <!--Chart-->
                        <div style="width: 100%; height: 7%;">
                            <div style="padding-left: 10px;">
                                <asp:RadioButtonList runat="server" ID="chartFactorRadio" AutoPostBack="true" RepeatDirection="Horizontal" OnLoad="chartFactorRadio_Load" OnSelectedIndexChanged="chartFactorRadio_SelectedIndexChanged">
                                </asp:RadioButtonList>
                            </div>
                        </div>
                        <div id="auditChart" style="width: 100%; height: 93%;">
                        </div>
                    </telerik:RadPane>
                </telerik:RadSplitter>
            </telerik:RadPane>
        </telerik:RadSplitter>
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" VisibleStatusbar="false" Behaviors="Close" Skin="Metro" OnClientClose="WindowClosed"
            EnableShadow="true">
            <Windows>
                <telerik:RadWindow ID="WriteAuditReason" runat="server" Width="500px" Height="350px" ViewStateMode="Enabled" Title="填写审核理由"
                    ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Localization-Cancel="取消"
                    Localization-Close="关闭" Localization-Maximize="最大化" Localization-Minimize="最小化"
                    Localization-Reload="刷新" Localization-PinOff="浮动" Localization-PinOn="固定" />
                <telerik:RadWindow ID="ModifyData" runat="server" Width="500px" Height="350px" ViewStateMode="Enabled" Title="数据修改"
                    ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Localization-Cancel="取消"
                    Localization-Close="关闭" Localization-Maximize="最大化" Localization-Minimize="最小化"
                    Localization-Reload="刷新" Localization-PinOff="浮动" Localization-PinOn="固定" />
                <telerik:RadWindow ID="AuditLog" runat="server" Width="800px" Height="400px" ViewStateMode="Enabled" Title="查看互动信息"
                    ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Localization-Cancel="取消"
                    Localization-Close="关闭" Localization-Maximize="最大化" Localization-Minimize="最小化"
                    Localization-Reload="刷新" Localization-PinOff="浮动" Localization-PinOn="固定" />
                <telerik:RadWindow ID="FactorInfo" runat="server" Width="1000px" Height="500px" ViewStateMode="Enabled" Title="查看因子数据"
                    ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Localization-Cancel="取消"
                    Localization-Close="关闭" Localization-Maximize="最大化" Localization-Minimize="最小化"
                    Localization-Reload="刷新" Localization-PinOff="浮动" Localization-PinOn="固定" />
            </Windows>
        </telerik:RadWindowManager>
        <div style="display: none">
            <%--修改数据提交--%>
            <input type="button" id="dataSubmit" style="display: none;" />
            <%--存放审核理由--%>
            <input type="hidden" id="auditReason" value="" />
            <%--选择因子后重新刷新界面（因子自定义控件中触发该事件）--%>
            <%--  <input type="button" id="refreshData" style="display: none;" value="0" onclick="Refresh_Grid(true);" runat="server" />--%>
            <telerik:RadButton ID="refreshData" runat="server" AutoPostBack="true" OnClick="refreshData_Click"></telerik:RadButton>

            <input type="button" id="modifyDataHidden" style="display: none;" value="0" onclick="Refresh_Grid(true);" runat="server" />
            <%--  隐藏域存放选中的因刷新子，在Grid刷新后刷新隐藏域及Chart图表--%>
            <asp:HiddenField ID="factorNames" runat="server" Value="" />

            <%--  隐藏域存放选中的测点，在Grid刷新后刷新隐藏域及Chart图表--%>
            <asp:HiddenField ID="PointIDHidden" runat="server" Value="" />
        </div>

        <telerik:RadCodeBlock ID="RadCodeBlock3" runat="server">
            <%--      <script src="../../../Resources/JavaScript/Echarts/build/dist/echarts_mhf.js"></script>--%>
            <script src="../../../Resources/JavaScript/Echarts/build/dist/echarts.js"></script>
            <script src="../../../Resources/JavaScript/JQuery/jquery-1.9.0.js"></script>
            <script src="../../../Resources/JavaScript/AuditOperator/MutilPointAuditData.js"></script>

            <script type="text/javascript">

                $(document).ready(function () {
                    ResizePageDiv();
                    if ('<%=Session["applicationUID"]%>' != null && '<%=Session["applicationUID"]%>' != "airaaira-aira-aira-aira-airaairaaira") {
                        $("#tdUpperColor").css("display", "none");//隐藏倒挂的图例
                        $("#tdUpperText").css("display", "none");
                    }
                });
                function ResizePageDiv() {
                    var bodyWidth = document.body.clientWidth;
                    var bodyHeight = document.body.clientHeight;
                    $('#pagediv').css("height", bodyHeight);
                    $('#pagediv').css("width", bodyWidth);
                }

                //鼠标左右键（右键不触发选择单元格信息）
                var cancelSelection = true;
                var IsBatchChanged = 0;
                var PointID = new Array();
                var FactorCode = new Array();
                var DataTime = new Array();
                var NewData = new Array();
                var Cell = new Array();
                var moveover = "grid";
                var windowState = null;
                var celledit = 0;//表格双击修改时置为1，以防止鼠标点击别的单元格
                //异步刷新时需要清空选择的单元格数据，否则会导致选择的信息错乱
                function ClearSelectedInfo() {
                    try {
                        PointID = new Array();;
                        FactorCode = new Array();
                        DataTime = new Array();
                        NewData = new Array();
                        Cell = new Array();
                    } catch (e) {
                        //alert("清空单元格选择信息：" + e.message);
                    }
                }

                //查询按钮判断条件
                function SearchClicking(sender, args) {
                    var factorNames = document.getElementById('factorNames').value;//隐藏域存放sitemap因子

                    var flag = 0;
                    for (var i = 0; i < document.getElementById("radioPoint").getElementsByTagName("input").length; i++) {
                        if (document.getElementById("radioPoint_" + i).checked == true) {
                            flag = 1;
                            break;
                        }
                    }
                    if (flag == 0) {
                        alert("请选择测点！");
                        args._cancel = true;
                    }
                    //else if (factorNames == "") {
                    //    alert("请选择因子！");
                    //    args._cancel = true;
                    //}
                }

                //加载ECharts数据
                function LoadingData() {
                    try {
                        //var ff = document.getElementById('factorNames');
                        var factorNames = document.getElementById('factorNames').value;//隐藏域存放sitemap因子
                        var facCode = "";
                        if (factorNames != "") {
                            facCode = factorNames.split('|')[0];
                        }
                        var StartTime = $find("<%=RadDatePickerBegin.ClientID%>").get_selectedDate().format("yyyy/M/dd");
                        var EndTime = $find("<%=RadDatePickerEnd.ClientID%>").get_selectedDate().format("yyyy/M/dd");
                        //var pointId = $("#radioPoint").find("[checked]").val();
                        var pointId = $("#PointIDHidden").val();
                        AjaxLoadingMutilFactor(facCode, StartTime, EndTime, pointId, '<%=Session["applicationUID"]%>');
                    } catch (e) {
                    }
                }

                //保存修改数据
                function ModifyAuditData(flag) {
                    if (flag != -1) {
                        var reason = document.getElementById('auditReason').value;
                        //alert(DataTime.join(";"));
                        AjaxAuditOperateData(PointID.join(";"), DataTime.join(";"), FactorCode.join(";"), NewData.join(";"), '<%=Session["applicationUID"]%>', reason, '<%=Session["UserGuid"]%>', "AuditAjaxHandler.ashx?DataType=ModifyAuditData&flag=" + flag);
                    } else {
                        CellCancel();
                    }
                    windowState = null;
                    //ClearSelectedInfo();
                }

                function WindowClosed() {
                    celledit = 0;
                    CellCancel();
                    windowState = null;
                }

                //取消单元格修改
                function CellCancel() {
                    try {
                        if (windowState != null) {
                            document.getElementById('refreshData').click();
                            //Refresh_Grid(true);
                            //windowState._tableView.cancelAll();
                            //GridResize();
                        }
                    } catch (e) {
                    }
                }

                //保存修改数据
                function ChartModifyAuditData(flag, modifyData) {
                    try {
                        var reason = document.getElementById('auditReason').value;
                        AjaxAuditOperateData(PointID.join(";"), DataTime.join(";"), FactorCode.join(";"), modifyData, '<%=Session["applicationUID"]%>', reason, '<%=Session["UserGuid"]%>', "AuditAjaxHandler.ashx?DataType=ModifyAuditData&flag=" + flag);
                        //ClearSelectedInfo();
                    } catch (e) {
                        //alert(e.message);
                    }
                }        //恢复到原始数据
                function RestorAuditData() {
                    var reason = document.getElementById('auditReason').value;
                    AjaxAuditOperateData(PointID.join(";"), DataTime.join(";"), FactorCode.join(";"), NewData.join(";"), '<%=Session["applicationUID"]%>', reason, '<%=Session["UserGuid"]%>', "AuditAjaxHandler.ashx?DataType=RestoreAuditData");
                    //ClearSelectedInfo();
                }

                //审核提交
                function SubmitAuditClicked(sender, args) {
                    if (!confirm("确定提交审核吗？")) { args._cancel = true; }
                    // var StartTime = $find("<%=RadDatePickerBegin.ClientID%>").get_selectedDate().format("yyyy/M/dd");
                    // var pointId = $("#PointIDHidden").val();
                    // AjaxSubmitAudit(StartTime, pointId, '<%=Session["applicationUID"]%>');

                }

                //刷新列表
                function Refresh_Grid(args) {
                    try {
                        if (args) {
                            var MasterTable = $find("<%= gridAuditData.ClientID %>").get_masterTableView();
                            MasterTable.rebind();
                            //GridResize();
                            //var gridHeight = parseInt($('#RadPane2').css("height")) - parseInt($('#ButtonDiv').css("height"));
                            ////$('#GridDiv').css("height", gridHeight);//设置表格高度
                            //$('#gridAuditData_GridData').css("height", gridHeight);//设置表格高度 
                        }
                    } catch (e) {
                        //alert(e+"###");
                    }
                }
                //Splite加载事件（初始化Chart）
                function loadSplitter(sender) {
                    var bodyWidth = document.body.clientWidth;
                    var bodyHeight = document.body.clientHeight;
                    sender.set_width(bodyWidth);//初始化Splitter高度及宽度
                    sender.set_height(bodyHeight);
                    GridResize();
                    LoadingData();//加载Echarts图表数据
                    //$("#chartMenu").hide();
                }

                //时间选择事件
                function BeginDateSelected(sender, args) {
                    //LoadingData();//加载Echarts图表数据
                }

                //控制时间范围
                function BeginDateChanging(sender, args) {
                    var beginTime = new Date(Date.parse(args._newValue.replace(/-/g, "/")));
                    if (beginTime == null) {
                        alert("时间不能为空！");
                        args._cancel = true;
                    } else if (beginTime > new Date()) {
                        alert("审核时间超出范围！");
                        args._cancel = true;
                    } else {
                        var endTime = $find("<%= RadDatePickerEnd.ClientID %>").get_selectedDate();
                        if (beginTime > endTime) {
                            alert("开始时间不能大于结束时间！");
                            args._cancel = true;
                        } else {
                            var date = endTime.getTime() - beginTime.getTime()  //时间差的毫秒数                       
                            var days = Math.floor(date / (24 * 3600 * 1000)) //计算出相差天数
                            var auditDays = "<%=GetAuditDays()%>";
                            if (auditDays != -1 && days >= auditDays) {
                                alert("时间范围不能超过" + auditDays + "天");
                                args._cancel = true;
                            }
                        }
                    }
            }

            //控制时间范围
            function EndDateChanging(sender, args) {
                var endTime = new Date(Date.parse(args._newValue.replace(/-/g, "/")));
                if (endTime == null) {
                    alert("时间不能为空！");
                    args._cancel = true;
                } else if (endTime > new Date()) {
                    alert("审核时间超出范围！");
                    args._cancel = true;
                } else {
                    var beginTime = $find("<%= RadDatePickerBegin.ClientID %>").get_selectedDate();
                    if (beginTime > endTime) {
                        alert("开始时间不能大于结束时间！");
                        args._cancel = true;
                    } else {
                        var date = endTime.getTime() - beginTime.getTime()  //时间差的毫秒数                       
                        var days = Math.floor(date / (24 * 3600 * 1000)) //计算出相差天数
                        var auditDays = "<%=GetAuditDays()%>";
                        if (auditDays != -1 && days >= auditDays) {
                            alert("时间范围不能超过" + auditDays + "天");
                            args._cancel = true;
                        }
                    }
                }
        }

        var formatDate = function (date) {
            var y = date.getFullYear();
            var m = date.getMonth() + 1;
            m = m < 10 ? '0' + m : m;
            var d = date.getDate();
            d = d < 10 ? ('0' + d) : d;
            return y + '-' + m + '-' + d;
        };

        //显示右键菜单
        function RowContextMenu(sender, eventArgs) {
            //sender.get_masterTableView().selectItem(sender.get_masterTableView().get_dataItems()[index].get_element(), true);
            var menu = $find("<%=RadMenuData.ClientID %>");
            var evt = eventArgs.get_domEvent();
            if (PointID.length == 0) return;
            else
                if (PointID.length == 1) menu = $find("<%=RadMenuAuditLog.ClientID %>");
            menu.show(evt);
        }

        //图表显示右键菜单
        function showMenu(e) {
            var menu = $find("<%=ContextMenuChart.ClientID %>");
            menu.show(e);
        }


        //右键菜单点击
        function RadContextMenuClicked(sender, eventArgs) {
            var menuItemValue = eventArgs.get_item().get_text();
            switch (menuItemValue) {
                case "修改":
                    window.radopen("AuditModityData.aspx?data=" + NewData.join(";"), "ModifyData")//修改数据
                    break;
                case "无效":
                    window.radopen("AuditReason.aspx?operator=1", "WriteAuditReason"); //置为无效
                    break;
                case "有效":
                    window.radopen("AuditReason.aspx?operator=2", "WriteAuditReason"); //置为有效
                    break;
                case "恢复":
                    window.radopen("AuditReason.aspx?operator=3", "WriteAuditReason"); //数据恢复
                    break;
                case "互动信息":
                    window.radopen("AuditFactorLogInfo.aspx?PointID=" + PointID.join(";") + "&DataTime=" + DataTime.join(";") + "&factorCode=" + FactorCode.join(";"), "AuditLog"); //填写审核理由
                    break;
                case "查看因子数据":
                    window.radopen("MutilPointChart.aspx?factorCode=" + FactorCode.join(";") + "&startTime=" + DataTime.join(";") + "&pointType=" + '<%= Request.QueryString["pointType"]%>', "FactorInfo"); //查看因子数据
                    break;
                case "停电":
                    window.radopen("AuditReason.aspx?operator=4", "WriteAuditReason"); //置为停电
                    break;
                case "质控":
                    window.radopen("AuditReason.aspx?operator=5", "WriteAuditReason"); //置为质控
                    break;
            }
        }

        //点击前一天
        function LastDayClicked(sender, args) {
            var picker = $find("<%=RadDatePickerBegin.ClientID%>");
            var date = picker.get_selectedDate();
            date.setDate(date.getDate() - 1);
            picker.set_selectedDate(date);
        }

        //点击后一天
        function NextDayClicked(sender, args) {
            var picker = $find("<%=RadDatePickerBegin.ClientID%>");
            var date = picker.get_selectedDate();
            date.setDate(date.getDate() + 1);
            picker.set_selectedDate(date);
        }

        //Ajax开始
        function onRequestStart(sender, args) {
            //AjaxPreData();//数据缺失加载预处理数据
        }

        //AJAX结束
        function onResponseEnd(sender, args) {
            try {
                //alert(1);
                ClearSelectedInfo();
                //if (document.getElementById('refreshData').value == 1) LoadingData();//等Grid异步刷新后加载Chart，否则隐藏域里的值获取的是上一次的结果
                //document.getElementById('refreshData').value = 0;//初始化隐藏域状态
                GridResize();//重新设置表格高度 catch (e) {
                ResizePageDiv();
                //alert("AJAX结束：" + e.message);
            } catch (e) {
            }
        }

        //单元格鼠标悬停
        function MouseOver(index, uniqueName) {
            var MasterTable = $find("<%= gridAuditData.ClientID %>").get_masterTableView();
            if (MasterTable != 'undefined') {
                var row = MasterTable.get_dataItems()[index];
                cell = MasterTable.getCellByColumnUniqueName(row, uniqueName);
                celltext = cell.innerHTML;
                var title = MasterTable.getCellByColumnUniqueName(row, "时间");
                if (uniqueName == "测点")
                    currentColumnName = "";
                else
                    currentColumnName = uniqueName;
            }
        }

        //单元格取消选择
        function CellDeselected(sender, args) {
            try {
                var grid = $find("<%=gridAuditData.ClientID%>");
                if (grid.get_batchEditingManager().get_currentlyEditedCell() != null)
                    grid.get_batchEditingManager()._tryCloseEdits(grid.get_masterTableView());//提交编辑框数据
            } catch (e) {
            }
            var index = Cell.join(";").indexOf(args.get_cellIndexHierarchical());
            if (index >= 0 && celledit == 0) {
                Cell.splice(index, 1);//移除数组里存放的单元格信息
                PointID.splice(index, 1);
                FactorCode.splice(index, 1);
                DataTime.splice(index, 1);
            }
        }

        //单元格选择
        function CellSelected(sender, args) {
            try {
                if (!cancelSelection) return;
                if (cancelSelection) {
                    var uniqueName = args.get_column().get_uniqueName();
                    if (uniqueName == 'DataDateTime' || uniqueName == 'PointId') {
                        ClearSelectedInfo();
                        //args.set_cancel(true);
                        return;
                    }
                    if (moveover != "grid") ClearSelectedInfo();
                    moveover = "grid";
                    if (Cell.join(";").indexOf(args.get_cellIndexHierarchical()) < 0 && celledit == 0) {
                        var gridDataItem = args.get_gridDataItem();
                        PointID.push(gridDataItem.getDataKeyValue("PointID"));
                        FactorCode.push(uniqueName);
                        //var factorNames = document.getElementById('factorNames').value;//隐藏域存放sitemap因子
                        //var FactorCodeIndex = factorNames.split('|')[0];//获取隐藏域里存放的小数位数
                        DataTime.push(gridDataItem.getDataKeyValue("DataDateTime"));
                        Cell.push(args.get_cellIndexHierarchical());
                    }
                }
            } catch (e) {
                //alert("[多选]单元格选择后：" + e.message);
            }
        }

        //选中Chart节点并存储在数组中
        function ChartPointSelect(param, data) {
            moveover = "chart";
            ClearSelectedInfo();
            var paramName = param.split(';');
            //alert(paramName);
            PointID.push(paramName[2]);
            FactorCode.push(paramName[1]);
            DataTime.push(paramName[3]);
            NewData.push(data);
            //Cell.push(args.get_cellIndexHierarchical());
        }


        //注册图表右键事件
        function RegChartRightClickEvents() {
            //图表容器的右键鼠标事件
            $("#auditChart").bind("mouseup", function (oEvent) {
                if (!oEvent) oEvent = window.event;
                //alert(oEvent.button);
                if (oEvent.button == 2 && PointID.length > 0) {
                    showMenu(oEvent);
                }
            });
        }


        //表格修改关闭
        function BatchEditClosing(sender, args) {
            var cell = args.get_cell();
            cell.className = cell.className.replace("rgBatchCurrent", "")
            if (cell.className.indexOf("rgBatchChanged") >= 0) IsBatchChanged = 1
        }

        //表格修改关闭
        function BatchEditClosed(sender, args) {
            if (IsBatchChanged == 1) args.get_cell().className = "rgBatchChanged";
            IsBatchChanged = 0;
        }


        //双击表格修改值
        function BatchEditCellValueChanged(sender, args) {
            ClearSelectedInfo();
            var MasterTable = $find("<%= gridAuditData.ClientID %>").get_masterTableView();
            var row = args.get_row();
            var table = args.get_tableView();

            if (table._firstRow.rowIndex >= 0) {
                var rowindex = table._firstRow.rowIndex == 0 ? row.rowIndex : row.rowIndex - 1;
                PointID.push(MasterTable.get_dataItems()[rowindex].getDataKeyValue("PointID"));
                DataTime.push(MasterTable.get_dataItems()[rowindex].getDataKeyValue("DataDateTime"));
                FactorCode.push(args.get_columnUniqueName());
                NewData.push(args.get_editorValue());
                //Cell.push(args.get_cell());
                celledit = 1;
                window.radopen("AuditReason.aspx?operator=0", "WriteAuditReason"); //填写审核理由 
                windowState = args;
            }
        }


        //统计行取消编辑操作
        function BatchEidtOpening(sender, args) {
            try {
                var row = args.get_row();
                if (row.innerText.indexOf("样本") >= 0 || row.innerText.indexOf("最") >= 0 || row.innerText.indexOf("平均") >= 0)
                    args.set_cancel(true);
            } catch (e) {
            }
        }

        function cellIsSelected(element) {
            if (element) {
                if (element.className.indexOf("rgSelectedCell") >= 0)
                    return true;
                else
                    return cellIsSelected(element.parentElement);
            }
            return false;
        }
        function rightButtonClicked(e) {
            var theEvent = window.event || arguments.callee.caller.arguments[0];
            if (!e) e = new Sys.UI.DomEvent(theEvent);
            if ((e.type == 'mousedown') || (e.type == 'mouseup')) {
                return e.button > 0;
            }
            return false;
        }

        function cellSelecting(sender, args) {
            try {
                var theEvent = window.event || arguments.callee.caller.arguments[0];
                //var theEvent = window.event || args.target;
                var e = new Sys.UI.DomEvent(theEvent);
                var selectedColumn = args.get_column();
                var uniqueName = selectedColumn.get_uniqueName();
                var selectedRow = args.get_row();
                if (selectedColumn._data.Editable == false || uniqueName == 'DataDateTime' || uniqueName == 'PointId')
                    args.set_cancel(true);
                if (selectedRow.innerText.indexOf("样本") >= 0 || selectedRow.innerText.indexOf("最") >= 0 || selectedRow.innerText.indexOf("平均") >= 0) args.set_cancel(true);
                if (rightButtonClicked(e) && cellIsSelected(e.target)) {
                    args.set_cancel(true);
                }
            } catch (e) {
                //alert(e.message);
            }
        }

        function cellDeselecting(sender, args) {
            try {
                var theEvent = window.event || arguments.callee.caller.arguments[0];
                var e = new Sys.UI.DomEvent(theEvent);
                if (rightButtonClicked(e) && cellIsSelected(e.target)) {
                    args.set_cancel(true);
                }

            } catch (e) {
            }
        }

        //隐藏测点按钮
        function HidePanel(send, args) {
            $('#RAD_SLIDING_PANE_TAB_RadSlidingPane_Point').css("display", "none");
        }

        //显示测点按钮
        function ShowPanel(send, args) {
            $('#RAD_SLIDING_PANE_TAB_RadSlidingPane_Point').css("display", "block");

        }

        //Chart隐藏
        function ChartPanelCollapsed(send, args) {
            GridResize()
        }

        //Chart或表格显示
        function ChartClientExpanded(send, args) {
            GridResize();
            LoadingData();//加载Echarts图表数据
        }

        //表格隐藏
        function GridClientCollapsed(send, args) {
            LoadingData();//加载Echarts图表数据
        }

        //重新设置表格高度
        function GridResize() {
            try {
                var gridHeight = parseInt($('#RadPane2').css("height")) - parseInt($('#ButtonDiv').css("height")) - 18;
                //alert(parseFloat($('.rgHeader').css("height")));
                if (parseFloat($('.rgHeader').css("height")) > 40)
                    $('#gridAuditData_GridData').css("height", gridHeight - 26 - 20);//设置表格高度 
                else
                    $('#gridAuditData_GridData').css("height", gridHeight - 26 - 5);//设置表格高度 
            } catch (e) {
            }
        }

        function SpliterResized(sender, args) {
            GridResize();
            LoadingData();//加载Echarts图表数据
        }

        //全选
        function checkAll(button, args) {
            for (var i = 0; i < document.getElementById("radioPoint").getElementsByTagName("input").length; i++) {
                document.getElementById("radioPoint_" + i).checked = true;
            }
        }

        //不选
        function deleteAll(button, args) {
            for (var i = 0; i < document.getElementById("radioPoint").getElementsByTagName("input").length; i++) {
                document.getElementById("radioPoint_" + i).checked = false;
            }
        }

        //反选
        function ReverseAll(button, args) {
            for (var i = 0; i < document.getElementById("radioPoint").getElementsByTagName("input").length; i++) {
                var objCheck = document.getElementById("radioPoint_" + i);
                if (objCheck.checked)
                    objCheck.checked = false;
                else
                    objCheck.checked = true;
            }
        }
            </script>
        </telerik:RadCodeBlock>
    </form>
</body>
</html>

