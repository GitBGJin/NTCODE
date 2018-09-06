﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MutilPointAuditDataWeibo.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Audit.MutilPointAuditDataWeibo" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
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
                <telerik:AjaxSetting AjaxControlID="refreshData">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridAuditData"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="auditChart"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="factorNames"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="PointIDHidden"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="submitButton">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridAuditData"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="auditChart"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="factorNames"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="auditState"></telerik:AjaxUpdatedControl>
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
                <telerik:AjaxSetting AjaxControlID="radioPoint">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="radioPollutant"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="factorNames"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="radioPoint"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="PointIDHidden"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="IsShowTotal">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridAuditData"></telerik:AjaxUpdatedControl>
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
                <telerik:AjaxSetting AjaxControlID="chartFactorRadio">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="chartFactorRadio"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="auditChart"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="factorNames"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
            <ClientEvents OnRequestStart="onRequestStart" OnResponseEnd="onResponseEnd" />
        </telerik:RadAjaxManager>
        <div runat="server" id="pagediv" style="position: absolute; z-index: -1;"></div>
        <div id="AuditSubmitDiv" style="display: none; vertical-align: middle; text-align: center; background-color: white; opacity: 0.7; filter: alpha(opacity=70); z-index: 100; position: absolute;">
            <p style="text-align: center; vertical-align: middle; padding-top: 20%; font-weight: bold; font-size: 18px; color: #b4aa38;">正在提交...</p>
        </div>
        <telerik:RadSplitter runat="server" ID="splitterContent" BorderSize="0" Width="100%" Height="100%" OnClientLoad="loadSplitter">
            <!-- 左侧测点、因子-->
            <telerik:RadPane runat="server" ID="LeftPane" Width="20%">
                <telerik:RadSlidingZone ID="RadSlidingZone1" runat="server" DockedPaneId="RadSlidingPane_Point" BorderSize="0">
                    <telerik:RadSlidingPane ID="RadSlidingPane_Point" Width="200" runat="server" Title="测点" UndockText="收缩" DockText="固定" CollapseText="关闭"
                        OnClientExpanding="HidePanel" OnClientBeforeDock="HidePanel" OnClientBeforeUndock="ShowPanel" OnClientCollapsed="ShowPanel"
                        EnableDock="true">
                        <div>
                            <asp:CheckBoxList runat="server" ID="radioPoint" onclick="CheckSelect1()" AutoPostBack="true" RepeatDirection="Horizontal" RepeatColumns="2" OnSelectedIndexChanged="radioPoint_SelectedIndexChanged"></asp:CheckBoxList>
                        </div>
                        <div style="padding-top: 10px;">
                            <div style="width: 100%; height: 28px; color: #fff; background-color: #3A94D3;">
                                <div style="padding-top: 6px; padding-left: 6px;">因子</div>
                            </div>
                            <asp:CheckBoxList runat="server" onclick="CheckSelect()" ID="radioPollutant" AutoPostBack="true" RepeatDirection="Vertical" RepeatColumns="1" Font-Bold="true" Font-Size="11" OnSelectedIndexChanged="radioPollutant_SelectedIndexChanged"></asp:CheckBoxList>
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
                        </div>
                    </telerik:RadSlidingPane>
                </telerik:RadSlidingZone>
            </telerik:RadPane>
            <!--中间-->
            <telerik:RadPane ID="MiddlePane" runat="server" Scrolling="None" Height="100%" BorderSize="0" Width="78%">
                <telerik:RadSplitter ID="RadSplitter1" runat="server" Orientation="Horizontal" BorderSize="0" Height="100%">
                    <!--按钮及表格-->
                    <telerik:RadPane ID="RadPane2" runat="server" Scrolling="None" Width="100%" Height="58%" OnClientCollapsed="GridClientCollapsed" OnClientExpanded="ChartClientExpanded" OnClientResized="SpliterResized">
                        <!--按钮-->
                        <div id="ButtonDiv" style="height: 66px;">
                            <table style="text-align: left; width: 100%;">
                                <tr>
                                    <td>
                                        <div style="float: left;">
                                            <%--  <asp:Label runat="server" ID="portName" ForeColor="Black" Font-Bold="true" Font-Size="14"></asp:Label>--%>
                                            <asp:Label runat="server" ForeColor="Black" Font-Bold="true" Font-Size="14" Text="审核状态："></asp:Label>
                                        </div>
                                        <div style="float: left;">
                                            <asp:Label runat="server" ID="auditState" ForeColor="Red" Font-Bold="true" Font-Size="13"></asp:Label>
                                        </div>
                                    </td>
                                    <td style="width: 60px;">因子</td>
                                    <td style="width: 130px;">
                                        <telerik:RadDropDownList runat="server" ID="weibo" AutoPostBack="false">
                                            <Items>
                                                <telerik:DropDownListItem Value="401" Text="温度" Selected="true" />
                                                <telerik:DropDownListItem Value="402" Text="蒸汽密度" />
                                                <telerik:DropDownListItem Value="404" Text="相对湿度" />
                                            </Items>
                                        </telerik:RadDropDownList>
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
                                            ClientEvents-OnDateSelected="EndDateSelected" DateInput-ClientEvents-OnValueChanging="EndDateChanging" OnSelectedDateChanged="RadDatePickerBegin_SelectedDateChanged">
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
                                        <telerik:RadButton ID="SubmitAudit" runat="server" BackColor="#3A94D3" ForeColor="White" AutoPostBack="false" OnClientClicking="SubmitAuditClicked" OnClick="SubmitAudit_Click">
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
                                        <td style="width: 40%; display: none;">
                                            <table style="text-align: right; width: 100%;">
                                                <tr>
                                                    <td></td>
                                                    <td style="width: 100px;">
                                                        <asp:CheckBox runat="server" ID="IsShowTotal" AutoPostBack="true" Text="显示统计信息" OnCheckedChanged="IsShowTotal_CheckedChanged" />
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
                                AllowPaging="true" PageSize="24"
                                AutoGenerateColumns="false" AllowMultiRowSelection="false"
                                EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                                ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0" ShowFooter="true"
                                CssClass="RadGrid_Customer" OnNeedDataSource="gridAuditData_NeedDataSource">
                                <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
                                <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False" ClientDataKeyNames="PointId,DateTime,PollutantCode"
                                    InsertItemPageIndexAction="ShowItemOnCurrentPage" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" NoMasterRecordsText="没有数据" EditMode="Batch" HeaderStyle-HorizontalAlign="Center">
                                    <BatchEditingSettings EditType="Cell" OpenEditingEvent="DblClick" />
                                    <PagerStyle FirstPageToolTip="首页" AlwaysVisible="true" Position="Bottom" NextPageToolTip="下页" LastPageToolTip="尾页" PrevPageToolTip="上页"
                                        PageSizeLabelText="显示记录数:" PagerTextFormat="跳转页面:{4}&amp;nbsp;显示 &lt;strong&gt;{2}&lt;/strong&gt; - &lt;strong&gt;{3}&lt;/strong&gt;条,共 &lt;strong&gt;{5}&lt;/strong&gt;条"></PagerStyle>
                                </MasterTableView>
                                <CommandItemStyle Width="100%" />
                                <ClientSettings AllowKeyboardNavigation="true">
                                    <Selecting UseClientSelectColumnOnly="true" CellSelectionMode="MultiCell" />
                                    <ClientEvents OnCellSelecting="cellSelecting"
                                        OnCellDeselecting="cellDeselecting" OnCellSelected="CellSelected" OnCellDeselected="CellDeselected"
                                        OnBatchEditCellValueChanged="BatchEditCellValueChanged" OnBatchEditOpening="BatchEidtOpening"
                                        OnBatchEditClosing="BatchEditClosing" OnBatchEditClosed="BatchEditClosed" />
                                    <Scrolling AllowScroll="True" UseStaticHeaders="true" />
                                </ClientSettings>
                            </telerik:RadGrid>
                            <telerik:RadContextMenu ID="RadMenuData" runat="server" EnableRoundedCorners="true" EnableShadows="true"
                                OnClientItemClicked="RadContextMenuClicked">
                            </telerik:RadContextMenu>
                            <telerik:RadContextMenu ID="RadMenuAuditLog" runat="server" EnableRoundedCorners="true" EnableShadows="true"
                                OnClientItemClicked="RadContextMenuClicked">
                            </telerik:RadContextMenu>
                            <telerik:RadContextMenu ID="ContextMenuChart" runat="server" EnableRoundedCorners="true" EnableShadows="true"
                                OnClientItemClicked="RadContextMenuClicked">
                            </telerik:RadContextMenu>
                        </div>
                    </telerik:RadPane>
                </telerik:RadSplitter>
            </telerik:RadPane>
        </telerik:RadSplitter>
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" VisibleStatusbar="false" Behaviors="Move,Close" Skin="Metro" OnClientClose="WindowClosed"
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

            <telerik:RadButton ID="submitButton" runat="server" AutoPostBack="true" OnClick="submitButton_Click"></telerik:RadButton>

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
            <script src="../../../Resources/JavaScript/AuditOperator/MutilPointAuditDataSuper.js"></script>

            <script type="text/javascript">

                //鼠标左右键（右键不触发选择单元格信息）
                var cancelSelection = true;
                var IsBatchChanged = 0;
                var PointID = new Array();
                var FactorCode = new Array();
                var DataTime = new Array();
                var NewData = new Array();
                var Pollutant = new Array();
                var Cell = new Array();
                var moveover = "grid";
                var windowState = null;
                var celledit = 0;//表格双击修改时置为1，以防止鼠标点击别的单元格

                function CheckSelect1() {
                    var tb = document.getElementById("radioPoint");
                    for (var i = 0; i < tb.rows.length; i++) {
                        for (var j = 0; j < tb.rows[i].cells.length; j++) {
                            var chk = tb.rows[i].cells[j].firstChild;
                            if (chk != null && chk != event.srcElement) {
                                chk.checked = false;
                            }
                        }
                    }
                };

                function CheckSelect() {
                    var tb = document.getElementById("radioPollutant");
                    for (var i = 0; i < tb.rows.length; i++) {
                        for (var j = 0; j < tb.rows[i].cells.length; j++) {
                            var chk = tb.rows[i].cells[j].firstChild;
                            if (chk != null && chk != event.srcElement) {
                                chk.checked = false;
                            }
                        }
                    }
                };

                //异步刷新时需要清空选择的单元格数据，否则会导致选择的信息错乱
                function ClearSelectedInfo() {
                    try {
                        PointID = new Array();;
                        FactorCode = new Array();
                        DataTime = new Array();
                        NewData = new Array();
                        Pollutant = new Array();
                        Cell = new Array();
                    } catch (e) {
                        //alert("清空单元格选择信息：" + e.message);
                    }
                }

                $(document).ready(function () {
                    ResizePageDiv();//设置蒙版div的高度、宽度
                    if ('<%=Session["applicationUID"]%>' != null && '<%=Session["applicationUID"]%>' != "airaaira-aira-aira-aira-airaairaaira") {
                        $("#tdUpperColor").css("display", "none");//隐藏倒挂的图例
                        $("#tdUpperText").css("display", "none");
                    }
                });


                //Ajax开始
                function onRequestStart(sender, args) {
                    //AjaxPreData();//数据缺失加载预处理数据
                }

                //AJAX结束
                function onResponseEnd(sender, args) {
                    try {
                        ClearSelectedInfo();
                        //if (document.getElementById('refreshData').value == 1) LoadingData();//等Grid异步刷新后加载Chart，否则隐藏域里的值获取的是上一次的结果
                        //document.getElementById('refreshData').value = 0;//初始化隐藏域状态
                        GridResize();//重新设置表格高度 catch (e) {
                        ResizePageDiv();
                    } catch (e) {
                    }
                }

                //Splite加载事件（初始化Chart）
                function loadSplitter(sender) {
                    var bodyWidth = document.body.clientWidth;
                    var bodyHeight = document.body.clientHeight;
                    sender.set_width(bodyWidth);//初始化Splitter高度及宽度
                    sender.set_height(bodyHeight);
                    var grid = $find("<%= gridAuditData.ClientID %>");
                    grid.repaint();
                    //Refresh_Grid(true);
                    //GridResize();
                    //LoadingData();//加载Echarts图表数据
                    //$("#chartMenu").hide();
                }

                //加载ECharts数据
                function LoadingData() {
                    try {
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
                }

                //保存修改数据
                function ModifyAuditData(flag) {
                    if (flag != "-1") {
                        var reason = document.getElementById('auditReason').value;
                        //alert(DataTime.join(";"));
                        AjaxAuditOperateDataWeibo(PointID.join(";"), DataTime.join(";"), FactorCode.join(";"), NewData.join(";"), Pollutant.join(";"), reason, '<%=Session["UserGuid"]%>', "AuditAjaxHandler.ashx?DataType=ModifyAuditDataSuper&flag=" + flag);
                    } else {
                        CellCancel();
                    }
                    windowState = null;
                    //ClearSelectedInfo();
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
                    if (!confirm("确定提交审核吗？")) { args._cancel = true; return; }
                    $('#AuditSubmitDiv').css("display", "");

                    var StartTime = $find("<%=RadDatePickerBegin.ClientID%>").get_selectedDate().format("yyyy/MM/dd HH:mm:ss");
                    var EndTime = $find("<%=RadDatePickerEnd.ClientID%>").get_selectedDate().format("yyyy/MM/dd HH:mm:ss");
                    var pointId = $("#PointIDHidden").val();
                    AjaxSubmitAuditWeibo(StartTime, EndTime, pointId, '<%=Session["UserGuid"]%>', '<%=ViewState["pointType"]%>');

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
                    }
                }

                //审核操作窗口关闭需清空临时数组中的数据
                function WindowClosed() {
                    celledit = 0;
                    CellCancel();
                    windowState = null;
                }


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
                    var value = eventArgs.get_item().get_value();
                    switch (menuItemValue) {
                        case "互动信息":
                            window.radopen("AuditFactorLogInfo.aspx?PointID=" + PointID.join(";") + "&DataTime=" + DataTime.join(";") + "&factorCode=" + FactorCode.join(";"), "AuditLog"); //填写审核理由
                            break;
                        case "查看因子数据":
                            window.radopen("MutilPointChart.aspx?factorCode=" + FactorCode.join(";") + "&startTime=" + DataTime.join(";") + "&pointType=" + '<%= Request.QueryString["pointType"]%>', "FactorInfo"); //查看因子数据
                            break;
                        case "修改":
                            window.radopen("AuditModityData.aspx?data=" + NewData.join(";"), "ModifyData")//修改数据
                            break;
                        case "恢复":
                            window.radopen("AuditReason.aspx?operator=restore", "WriteAuditReason"); //数据恢复
                            break;
                        default:
                            window.radopen("AuditReason.aspx?operator=" + value, "WriteAuditReason"); //置为无效
                            break;
                            //case "修改":
                            //    window.radopen("AuditModityData.aspx?data=" + NewData.join(";"), "ModifyData")//修改数据
                            //    break;
                            //case "无效":
                            //    window.radopen("AuditReason.aspx?operator=1", "WriteAuditReason"); //置为无效
                            //    break;
                            //case "有效":
                            //    window.radopen("AuditReason.aspx?operator=2", "WriteAuditReason"); //置为有效
                            //    break;
                            //case "恢复":
                            //    window.radopen("AuditReason.aspx?operator=3", "WriteAuditReason"); //数据恢复
                            //    break;
                    }
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

                //选中Chart节点并存储在数组中
                function ChartPointSelect(param, data) {
                    moveover = "chart";
                    ClearSelectedInfo();
                    var paramName = param.split(';');
                    PointID.push(paramName[2]);
                    FactorCode.push(paramName[1]);
                    DataTime.push(paramName[3]);
                    NewData.push(data);
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
                        PointID.push(MasterTable.get_dataItems()[rowindex].getDataKeyValue("PointId"));
                        DataTime.push(MasterTable.get_dataItems()[rowindex].getDataKeyValue("DateTime"));
                        FactorCode.push(args.get_columnUniqueName());
                        NewData.push(args.get_editorValue());
                        Pollutant.push(MasterTable.get_dataItems()[rowindex].getDataKeyValue("PollutantCode"));
                        //Cell.push(args.get_cell());
                        celledit = 1;
                        window.radopen("AuditReason.aspx?operator=modify", "WriteAuditReason"); //填写审核理由 
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

                //单元选中后需将点位、因子、时间、单元格cell存入数组中，以便后续的审核操作
                function CellSelected(sender, args) {
                    try {
                        if (!cancelSelection) return;
                        if (cancelSelection) {
                            var uniqueName = args.get_column().get_uniqueName();
                            if (uniqueName == 'DateTime' || uniqueName == 'PointId') {
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
                                DataTime.push(gridDataItem.getDataKeyValue("DateTime"));
                                Cell.push(args.get_cellIndexHierarchical());
                            }
                        }
                    } catch (e) {
                        //alert("[多选]单元格选择后：" + e.message);
                    }
                }

                //单元格取消选择，清空存放数据的数组
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

        //解决Grid多选单元格后点击右键菜单后选择数据未选中的问题
        function cellIsSelected(element) {
            if (element) {
                if (element.className.indexOf("rgSelectedCell") >= 0)
                    return true;
                else
                    return cellIsSelected(element.parentElement);
            }
            return false;
        }

        //单元格选中前
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

        //单元格取消选中前
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

        //单元格右键菜单
        function rightButtonClicked(e) {
            var theEvent = window.event || arguments.callee.caller.arguments[0];
            if (!e) e = new Sys.UI.DomEvent(theEvent);
            if ((e.type == 'mousedown') || (e.type == 'mouseup')) {
                return e.button > 0;
            }
            return false;
        }


        //时间选择事件
        function BeginDateSelected(sender, args) {
            var beginTime = $find("<%= RadDatePickerBegin.ClientID %>").get_selectedDate();
            var endTime = $find("<%= RadDatePickerEnd.ClientID %>").get_selectedDate();
            var date = endTime.getTime() - beginTime.getTime()  //时间差的毫秒数                       
            var days = Math.floor(date / (24 * 3600 * 1000)) //计算出相差天数
            var auditDays = "<%=GetAuditDays()%>";
            if (auditDays != -1 && days >= auditDays) {
                //alert("时间范围不能超过" + auditDays + "天");
                //args._cancel = true;
                $find("<%= RadDatePickerEnd.ClientID %>").set_selectedDate(beginTime);
            }
            //LoadingData();//加载Echarts图表数据
        }

        //时间选择事件
        function EndDateSelected(sender, args) {
            var beginTime = $find("<%= RadDatePickerBegin.ClientID %>").get_selectedDate();
            var endTime = $find("<%= RadDatePickerEnd.ClientID %>").get_selectedDate();
            var date = endTime.getTime() - beginTime.getTime()  //时间差的毫秒数                       
            var days = Math.floor(date / (24 * 3600 * 1000)) //计算出相差天数
            var auditDays = "<%=GetAuditDays()%>";
            if (auditDays != -1 && days >= auditDays) {
                $find("<%= RadDatePickerBegin.ClientID %>").set_selectedDate(endTime);
            }
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
                    //alert("开始时间不能大于结束时间！");
                    $find("<%= RadDatePickerEnd.ClientID %>").set_selectedDate(beginTime);
                    //args._cancel = true;
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
                        //alert("开始时间不能大于结束时间！");
                        $find("<%= RadDatePickerBegin.ClientID %>").set_selectedDate(endTime);
                        //args._cancel = true;
                    }
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
            GridResize();
            var grid = $find("<%= gridAuditData.ClientID %>");
            grid.repaint();
        }

        //Chart或表格显示
        function ChartClientExpanded(send, args) {
            //GridResize();
            //LoadingData();//加载Echarts图表数据

            GridResize();
            //RedrawChart();
            var grid = $find("<%= gridAuditData.ClientID %>");
            grid.repaint();
        }

        //表格隐藏
        function GridClientCollapsed(send, args) {
            //LoadingData();//加载Echarts图表数据
            //RedrawChart();
        }

        //鼠标拖动改变图形和表格的大小
        function SpliterResized(sender, args) {
            //GridResize();
            //LoadingData();//加载Echarts图表数据
            GridResize();
            //RedrawChart();
            var grid = $find("<%= gridAuditData.ClientID %>");
             grid.repaint();
         }

         //重新设置表格高度
         function GridResize() {
             try {
                 //var gridHeight = parseInt($('#RadPane2').css("height")) - parseInt($('#ButtonDiv').css("height")) - 18;
                 //alert(parseFloat($('.rgHeader').css("height")));
                 //if (parseFloat($('.rgHeader').css("height")) > 40)
                 //    $('#gridAuditData_GridData').css("height", gridHeight - 26 - 20);//设置表格高度 
                 //else
                 //    $('#gridAuditData_GridData').css("height", gridHeight - 26 - 5);//设置表格高度 

                 var gridHeight = parseInt($('#RadPane2').css("height")) - parseInt($('#ButtonDiv').css("height"));
                 $('#gridAuditData').css("height", gridHeight);
             } catch (e) {
             }
         }

         //设置蒙版div的高度、宽度
         function ResizePageDiv() {
             var bodyWidth = document.body.clientWidth;
             var bodyHeight = document.body.clientHeight;
             $('#pagediv').css("height", bodyHeight);
             $('#pagediv').css("width", bodyWidth);
             $('#AuditSubmitDiv').css("height", bodyHeight);
             $('#AuditSubmitDiv').css("width", bodyWidth);
         }

         function checkAll(button, args) {
             for (var i = 0; i < document.getElementById("radioPoint").getElementsByTagName("input").length; i++) {
                 document.getElementById("radioPoint_" + i).checked = true;
             }
         }
         function deleteAll(button, args) {
             for (var i = 0; i < document.getElementById("radioPoint").getElementsByTagName("input").length; i++) {
                 document.getElementById("radioPoint_" + i).checked = false;
             }
         }
         function ReverseAll(button, args) {
             for (var i = 0; i < document.getElementById("radioPoint").getElementsByTagName("input").length; i++) {
                 var objCheck = document.getElementById("radioPoint_" + i);
                 if (objCheck.checked)
                     objCheck.checked = false;
                 else
                     objCheck.checked = true;
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

            </script>
        </telerik:RadCodeBlock>
    </form>
</body>
</html>
