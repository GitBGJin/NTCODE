<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AuditData.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Audit.AuditData" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="FactorRsmAudit" Src="~/Controls/FactorRsmAudit.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link type="text/css" rel="stylesheet" href="../../../App_Themes/Neptune/Telerik.Web.UI.Skins/Metro/RadSplitterAudit.css" />

</head>
<body scroll="no">
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" UpdatePanelHeight="100%">
        </telerik:RadAjaxLoadingPanel>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
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
                <telerik:AjaxSetting AjaxControlID="auditState">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="auditState"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="refreshData">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridAuditData" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="auditChart"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="factorNames"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="factorCbxRsm"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="refreshData">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="refreshData"></telerik:AjaxUpdatedControl>
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
                        <telerik:AjaxUpdatedControl ControlID="gridAuditData" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="factorNames"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="radioPoint"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <%--    <telerik:AjaxSetting AjaxControlID="SubmitAudit">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridAuditData" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>--%>
                <telerik:AjaxSetting AjaxControlID="factorNames">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="auditChart"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="radioPoint">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="factorCbxRsm"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="factorNames"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="gridAuditData" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="auditChart" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="portName"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="auditState"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="radioPoint"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="RadDatePickerBegin">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridAuditData" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="auditChart" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="LastDay1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridAuditData" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="auditChart" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="NextDay">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridAuditData" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="auditChart" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="lastPort">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridAuditData"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="auditChart"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="nextPort">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridAuditData"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="auditChart"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="IsShowTotal">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridAuditData"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
            <ClientEvents OnRequestStart="onRequestStart" OnResponseEnd="onResponseEnd" />
        </telerik:RadAjaxManager>

        <telerik:RadSplitter runat="server" ID="splitterContent" BorderSize="0" Width="100%" Height="100%" OnClientLoad="loadSplitter">
            <!-- 左侧测点、因子-->
            <telerik:RadPane runat="server" ID="LeftPane" Width="20%">
                <div>
                    <div style="width: 100%; height: 28px; color: #fff; background-color: #3A94D3;">
                        <div style="padding-top: 6px; padding-left: 6px;">测点</div>
                    </div>
                    <asp:RadioButtonList runat="server" ID="radioPoint" AutoPostBack="true" RepeatDirection="Horizontal" RepeatColumns="2" OnSelectedIndexChanged="radioPoint_SelectedIndexChanged"></asp:RadioButtonList>
                </div>
                <div style="padding-top: 10px;">
                    <div style="width: 100%; height: 28px; color: #fff; background-color: #3A94D3;">
                        <div style="padding-top: 6px; padding-left: 6px;">因子</div>
                    </div>
                    <CbxRsm:FactorRsmAudit runat="server" ApplicationType="Air" ID="factorCbxRsm"></CbxRsm:FactorRsmAudit>
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
                        ShowHeader="false" OnNeedDataSource="StatusGrid_NeedDataSource">
                        <MasterTableView GridLines="None" CommandItemDisplay="None" IsFilterItemExpanded="False" NoMasterRecordsText="没有数据">
                            <Columns>
                                <telerik:GridBoundColumn UniqueName="StatusIdentify" DataField="StatusIdentify"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="StatusName" DataField="StatusName"></telerik:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                        <CommandItemStyle Width="100%" />
                    </telerik:RadGrid>
                </div>
                <%--                <telerik:RadSlidingZone ID="RadSlidingZone1" runat="server" DockedPaneId="RadSlidingPane_Point" BorderSize="0">
                    <telerik:RadSlidingPane ID="RadSlidingPane_Point" Width="240" runat="server" Title="测点" UndockText="收缩" DockText="固定" CollapseText="关闭"
                        OnClientExpanding="HidePanel" OnClientBeforeDock="HidePanel" OnClientBeforeUndock="ShowPanel" OnClientCollapsed="ShowPanel"
                        EnableDock="true">
                        <div style="padding-top: 10px;">
                            <asp:RadioButtonList runat="server" ID="radioPoint" AutoPostBack="true" RepeatDirection="Horizontal" RepeatColumns="2" OnSelectedIndexChanged="radioPoint_SelectedIndexChanged"></asp:RadioButtonList>
                        </div>
                        <div style="padding-top: 10px;">
                            <div style="width: 100%; height: 28px; color: #fff; background-color: #3A94D3;">
                                <div style="padding-top: 6px; padding-left: 6px;">因子</div>
                            </div>
                            <CbxRsm:FactorRsmAudit runat="server" ApplicationType="Air" ID="factorCbxRsm"></CbxRsm:FactorRsmAudit>
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
                                ShowHeader="false" OnNeedDataSource="StatusGrid_NeedDataSource">
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
                </telerik:RadSlidingZone>--%>
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
                                            <asp:Label runat="server" ID="portName" ForeColor="Black" Font-Bold="true" Font-Size="14"></asp:Label>
                                        </div>
                                        <div style="float: left;">
                                            <asp:Label runat="server" ID="auditState" ForeColor="Red" Font-Bold="true" Font-Size="14"></asp:Label>
                                        </div>
                                    </td>
                                    <%--  <td style="width: 90px;">
                                      
                                    </td>--%>
                                    <td style="width: 60px;">日期：</td>
                                    <td style="width: 130px;">
                                        <telerik:RadDatePicker runat="server" Width="100%" ID="RadDatePickerBegin" AutoPostBack="true" 
                                            MinDate="1900-01-01" DateInput-Font-Size="10" DateInput-Font-Bold="false" DateInput-DateFormat="yyyy-MM-dd"
                                            DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                            Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                            Calendar-FastNavigationSettings-CancelButtonCaption="取消" Timeview-HeaderText="小时"
                                            ClientEvents-OnDateSelected="BeginDateSelected" DateInput-ClientEvents-OnValueChanging="BeginDateChanging" OnSelectedDateChanged="RadDatePickerBegin_SelectedDateChanged" Calendar-FastNavigationStep="12">
                                        </telerik:RadDatePicker>
                                    </td>
                                    <td style="width: 70px;">
                                        <telerik:RadButton ID="LastDay1" runat="server" BackColor="#3A94D3" ForeColor="White" AutoPostBack="false" OnClientClicked="LastDayClicked">
                                            <ContentTemplate>
                                                <asp:Label runat="server" ID="Label2" ForeColor="White" Text="前一天"></asp:Label>
                                            </ContentTemplate>
                                        </telerik:RadButton>
                                    </td>
                                    <td style="width: 70px;">
                                        <telerik:RadButton ID="NextDay" runat="server" BackColor="#3A94D3" ForeColor="White" AutoPostBack="false" OnClientClicked="NextDayClicked">
                                            <ContentTemplate>
                                                <asp:Label runat="server" ID="Label3" ForeColor="White" Text="后一天"></asp:Label>
                                            </ContentTemplate>
                                        </telerik:RadButton>
                                    </td>
                                    <td id="SubmitAuditTD" style="width: 80px;" runat="server">
                                        <telerik:RadButton ID="SubmitAudit" runat="server" BackColor="#3A94D3" ForeColor="White" AutoPostBack="false" OnClientClicked="SubmitAuditClicked">
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
                                    <%--  <td></td>--%>
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
                                                    <%--
                                                    <td style="width: 60px;">
                                                        <asp:LinkButton ID="lastPort" runat="server" Text="上一测点" OnClientClick="LastPortClick();"></asp:LinkButton>
                                                    </td>
                                                    <td style="width: 60px;">
                                                        <asp:LinkButton ID="nextPort" runat="server" Text="下一测点" OnClientClick="NextPortClick();"></asp:LinkButton></td>--%>
                                                    <td style="width: 100px;">
                                                        <asp:CheckBox runat="server" ID="IsShowTotal" AutoPostBack="true" Text="显示统计信息" OnCheckedChanged="IsShowTotal_CheckedChanged" />
                                                        <%--    <asp:LinkButton ID="hideAvg" runat="server" Text="隐藏统计信息"  Visible="false" OnClick="showAvg_Click" OnClientClick="HideAvg();"></asp:LinkButton>
                                                        <asp:LinkButton ID="showAvg" runat="server" Text="显示统计信息" OnClick="showAvg_Click" OnClientClick="ShowAvg();"></asp:LinkButton>--%>
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
                                    InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据" EditMode="Batch" HeaderStyle-HorizontalAlign="Center">
                                    <BatchEditingSettings EditType="Cell" OpenEditingEvent="DblClick" />
                                </MasterTableView>
                                <CommandItemStyle Width="100%" />
                                <ClientSettings AllowKeyboardNavigation="true">
                                    <Selecting UseClientSelectColumnOnly="true" CellSelectionMode="MultiCell" />
                                    <ClientEvents OnRowContextMenu="RowContextMenu" OnCellSelecting="cellSelecting"
                                        OnCellDeselecting="cellDeselecting" OnCellSelected="CellSelected" OnCellDeselected="CellDeselected"
                                        OnBatchEditCellValueChanged="BatchEditCellValueChanged" OnBatchEditOpening="BatchEidtOpening"
                                        OnBatchEditClosing="BatchEditClosing" OnBatchEditClosed="BatchEditClosed" />
                                    <%--  <Scrolling AllowScroll="True" EnableVirtualScrollPaging="false" UseStaticHeaders="false"  ></Scrolling>--%>
                                    <Scrolling AllowScroll="True" UseStaticHeaders="true" />
                                </ClientSettings>
                            </telerik:RadGrid>
                            <telerik:RadContextMenu ID="RadMenuData" runat="server" EnableRoundedCorners="true" EnableShadows="true"
                                OnClientItemClicked="RadContextMenuClicked">
                                <Items>
                                    <telerik:RadMenuItem Text="无效" ToolTip="置为无效(不参加计算)" />
                                    <telerik:RadMenuItem Text="有效" ToolTip="置为有效(参加计算)" />
                                    <telerik:RadMenuItem Text="恢复" ToolTip="恢复至原始数据" />
                                </Items>
                            </telerik:RadContextMenu>
                            <telerik:RadContextMenu ID="RadMenuAuditLog" runat="server" EnableRoundedCorners="true" EnableShadows="true"
                                OnClientItemClicked="RadContextMenuClicked">
                                <Items>
                                    <telerik:RadMenuItem Text="无效" ToolTip="置为无效(不参加计算)" />
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
                        <div id="auditChart" style="width: 100%; height: 112%;">
                        </div>
                    </telerik:RadPane>
                </telerik:RadSplitter>
            </telerik:RadPane>
        </telerik:RadSplitter>
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" VisibleStatusbar="false" Behaviors="Close" Skin="Metro"
            EnableShadow="true">
            <Windows>
                <telerik:RadWindow ID="WriteAuditReason" runat="server" Width="500px" Height="350px" ViewStateMode="Enabled" Title="填写审核理由"
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
        <div>
            <%--修改数据提交--%>
            <input type="button" id="dataSubmit" style="display: none;" />
            <%--存放审核理由--%>
            <input type="hidden" id="auditReason" value="" />
            <%--选择因子后重新刷新界面（因子自定义控件中触发该事件）--%>
            <input type="button" id="refreshData" style="display: none;" value="0" onclick="Refresh_Grid(true);" />
            <%--  隐藏域存放选中的因刷新子，在Grid刷新后刷新隐藏域及Chart图表--%>
            <asp:HiddenField ID="factorNames" runat="server" Value="" />
        </div>

        <telerik:RadCodeBlock ID="RadCodeBlock3" runat="server">
            <script src="../../../Resources/JavaScript/Echarts/build/dist/echarts.js"></script>
            <script src="../../../Resources/JavaScript/JQuery/jquery-1.9.0.js"></script>
            <script src="../../../Resources/JavaScript/AuditOperator/AuditData.js"></script>
            <%--       <script src="<%=ResolveClientUrl("../../../Resources/JavaScript/telerikControls/RadSiteMap.js")%>" type="text/javascript"></script>--%>
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
            </style>
            <script type="text/javascript">
                $(document).ready(function () {
                    if ('<%=Session["applicationUID"]%>' != null && '<%=Session["applicationUID"]%>' != "airaaira-aira-aira-aira-airaairaaira") {
                        $("#tdUpperColor").css("display", "none");//隐藏倒挂的图例
                        $("#tdUpperText").css("display", "none");
                    }
                });
                //鼠标左右键（右键不触发选择单元格信息）
                var cancelSelection = true;
                var IsBatchChanged = 0;
                var PointID = new Array();
                var FactorCode = new Array();
                var DataTime = new Array();
                var NewData = new Array();
                var Cell = new Array();
                //异步刷新时需要清空选择的单元格数据，否则会导致选择的信息错乱
                function ClearSelectedInfo() {
                    try {
                        PointID = new Array();;
                        FactorCode = new Array();
                        DataTime = new Array();
                        NewData = new Array();
                        Cell = new Array();
                    } catch (e) { alert("清空单元格选择信息：" + e.message); }
                }

                //加载ECharts数据
                function LoadingData() {
                    //var ff = document.getElementById('factorNames');
                    var factorNames = document.getElementById('factorNames').value;//隐藏域存放sitemap因子
                    var facCode = "";
                    if (factorNames != "") {
                        facCode = factorNames.split('|')[0];
                    }
                    var StartTime = $find("<%=RadDatePickerBegin.ClientID%>").get_selectedDate().format("yyyy/M/dd");
                    var pointId = $("#radioPoint").find("[checked]").val();
                    AjaxLoadingMutilFactor(facCode, StartTime, pointId, '<%=Session["applicationUID"]%>');
                }

                //保存修改数据
                function ModifyAuditData(flag) {
                    var reason = document.getElementById('auditReason').value;
                    AjaxAuditOperateData(PointID.join(";"), DataTime.join(";"), FactorCode.join(";"), NewData.join(";"), '<%=Session["applicationUID"]%>', reason, '<%=Session["UserGuid"]%>', "AuditAjaxHandler.ashx?DataType=ModifyAuditData&flag=" + flag);
                }

                //恢复到原始数据
                function RestorAuditData() {
                    var reason = document.getElementById('auditReason').value;
                    AjaxAuditOperateData(PointID.join(";"), DataTime.join(";"), FactorCode.join(";"), NewData.join(";"), '<%=Session["applicationUID"]%>', reason, '<%=Session["UserGuid"]%>', "AuditAjaxHandler.ashx?DataType=RestoreAuditData");
                }

                //审核提交
                function SubmitAuditClicked(sender, args) {
                    // var currentLoadingPanel = $find("<%= RadAjaxLoadingPanel1.ClientID %>");
                    var StartTime = $find("<%=RadDatePickerBegin.ClientID%>").get_selectedDate().format("yyyy/M/dd");
                    var pointId = $("#radioPoint").find("[checked]").val();
                    AjaxSubmitAudit(StartTime, pointId, '<%=Session["applicationUID"]%>');
                }

                //刷新列表
                function Refresh_Grid(args) {
                    try {
                        if (args) {
                            var MasterTable = $find("<%= gridAuditData.ClientID %>").get_masterTableView();
                            MasterTable.rebind();

                            var gridHeight = parseInt($('#RadPane2').css("height")) - parseInt($('#ButtonDiv').css("height"));
                            //$('#GridDiv').css("height", gridHeight);//设置表格高度
                            $('#gridAuditData_GridData').css("height", gridHeight);//设置表格高度 
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
                    //var gridHeight = parseInt($('#RadPane2').css("height")) - parseInt($('#ButtonDiv').css("height"));
                    //$('#GridDiv').css("height", gridHeight);//设置表格高度
                    //$('#gridAuditData').css("height", gridHeight);//设置表格高度
                    //alert( $('#gridAuditData').css("height"));
                    //$('#gridAuditData_GridData').css("height", gridHeight);//设置表格高度 
                    //Refresh_Grid(true);
                    LoadingData();//加载Echarts图表数据
                }

                //时间选择事件
                function BeginDateSelected(sender, args) {
                    //Refresh_Grid(true);
                    LoadingData();//加载Echarts图表数据
                }

                //控制时间范围
                function BeginDateChanging(sender, args) {
                    var begin = new Date(Date.parse(args._newValue.replace(/-/g, "/")));
                    //alert(begin + "===" + new Date().format("yyyy/MM/dd"));
                    if (begin == null) {
                        alert("开始时间或者终止时间，不能为空！");
                        args._cancel = true;
                    } else if (begin > new Date()) {
                        alert("审核时间超出范围！");
                        args._cancel = true;
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

            //右键菜单点击
            function RadContextMenuClicked(sender, eventArgs) {
                var menuItemValue = eventArgs.get_item().get_text();
                switch (menuItemValue) {
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
                    ClearSelectedInfo();
                    if (document.getElementById('refreshData').value == 1) LoadingData();//等Grid异步刷新后加载Chart，否则隐藏域里的值获取的是上一次的结果
                    document.getElementById('refreshData').value = 0;//初始化隐藏域状态

                    GridResize();//重新设置表格高度
                } catch (e) { alert("AJAX结束：" + e.message); }
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
                if (index >= 0) {
                    Cell.splice(index, 1);//移除数组里存放的单元格信息
                    PointID.splice(index, 1);
                    FactorCode.splice(index, 1);
                    DataTime.splice(index, 1);
                }
            }

            //单元格选择
            function CellSelected(sender, args) {
                try {
                    //var row = args.get_gridDataItem();;
                    //var cell = args.get_tableView().getCellByColumnUniqueName(row, args.get_column().get_uniqueName());
                    //cell.className=cell.className.replace("rgActiveCell", "");
                    //SelectedStr = SelectedStr.replace(DesSelectedStr, '');
                    if (!cancelSelection) return;
                    if (cancelSelection) {
                        var uniqueName = args.get_column().get_uniqueName();
                        if (uniqueName == 'DataDateTime' || uniqueName == 'PointId') {
                            ClearSelectedInfo();
                            //args.set_cancel(true);
                            return;
                        }
                        if (Cell.join(";").indexOf(args.get_cellIndexHierarchical()) < 0) {
                            var gridDataItem = args.get_gridDataItem();
                            PointID.push(gridDataItem.getDataKeyValue("PointID"));
                            FactorCode.push(uniqueName);
                            var factorNames = document.getElementById('factorNames').value;//隐藏域存放sitemap因子
                            var FactorCodeIndex = factorNames.split('|')[0];//获取隐藏域里存放的小数位数
                            DataTime.push(gridDataItem.getDataKeyValue("DataDateTime"));
                            Cell.push(args.get_cellIndexHierarchical());
                        }
                        //alert(Cell + "-----SELECTED");
                    }
                } catch (e) { alert("[多选]单元格选择后：" + e.message); }
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
                var MasterTable = $find("<%= gridAuditData.ClientID %>").get_masterTableView();
                var row = args.get_row();
                PointID.push(MasterTable.get_dataItems()[row.rowIndex].getDataKeyValue("PointID"));
                DataTime.push(MasterTable.get_dataItems()[row.rowIndex].getDataKeyValue("DataDateTime"));
                FactorCode.push(args.get_columnUniqueName());
                NewData.push(args.get_editorValue());
                window.radopen("AuditReason.aspx?operator=0", "WriteAuditReason"); //填写审核理由
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

            ////添加数据右键事件，确保在右键时存放单元格的数组不被写入数据
            //function rowMouseDown(e) {
            //    try {
            //        cancelSelection = true;
            //        if (e.button != 0) {
            //            cancelSelection = false;
            //        }
            //    } catch (e) { alert("鼠标右键事件：" + e.message); }
            //}
            //function onRowCreated(sender, args) {
            //    try {
            //        $addHandler($get(args.get_id()), "mousedown", rowMouseDown)
            //    } catch (e) { alert("添加鼠标右键事件：" + e.message); }
            //}

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
                //cancelSelection = true;
                if ((e.type == 'mousedown') || (e.type == 'mouseup')) {
                    //cancelSelection = false;
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
                    //alert(e.message);
                }
            }


            //function DBClick(sender, args) {
            //    var row = args.get_gridDataItem();;
            //    var cell = args.get_tableView().getCellByColumnUniqueName(row,"DataDateTime");
            //    if (cell.innerText.indexOf("样本") >= 0 || cell.innerText.indexOf("最") >= 0 || cell.innerText.indexOf("平均") >= 0)
            //        args.set_cancel(true);

            //}

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
                //var gridHeight = parseInt($('#RadPane2').css("height")) - parseInt($('#ButtonDiv').css("height"));
                //$('#GridDiv').css("height", gridHeight);//设置表格高度
                //$('#gridAuditData_GridData').css("height", gridHeight);//设置表格高度 
                //Refresh_Grid(true);
            }

            //Chart或表格显示
            function ChartClientExpanded(send, args) {
                GridResize();
                //var gridHeight = parseInt($('#RadPane2').css("height")) - parseInt($('#ButtonDiv').css("height"));
                //$('#GridDiv').css("height", gridHeight);//设置表格高度
                //$('#gridAuditData_GridData').css("height", gridHeight);//设置表格高度 
                //Refresh_Grid(true);
                LoadingData();//加载Echarts图表数据
            }

            //表格隐藏
            function GridClientCollapsed(send, args) {
                LoadingData();//加载Echarts图表数据
            }

            //重新设置表格高度
            function GridResize() {
                var gridHeight = parseInt($('#RadPane2').css("height")) - parseInt($('#ButtonDiv').css("height")) - 18;
                //alert($('#gridAuditData_GridData').css("height") + '=====' + gridHeight+"---"+parseInt($('#ButtonDiv').css("height")));
                $('#gridAuditData_GridData').css("height", gridHeight - 26);//设置表格高度 
            }

            function SpliterResized(sender, args) {
                GridResize();
                LoadingData();//加载Echarts图表数据
            }
            </script>
        </telerik:RadCodeBlock>
    </form>
</body>
</html>
