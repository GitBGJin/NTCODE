<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QueryByPoint.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.OperatingMaintenance.QueryByPoint" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <script type="text/javascript" src="../../../Resources/JavaScript/FrameJS.js"></script>
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script type="text/javascript">
                function QueryString(name) {
                    var AllVars = window.location.search.substring(1);
                    var Vars = AllVars.split("&");
                    for (i = 0; i < Vars.length; i++) {
                        var Var = Vars[i].split("=");
                        if (Var[0] == name) return Var[1];
                    }
                    return "";
                }
                function MoveMaintain() {
                    var infoType = parseFloat(QueryString("IsSpareParts")) + 1;
                    OpenFineUIWindow("c71f5a78-80f5-4aba-91db-be41d0af0d2b", "Pages/EnvWater/OperatingMaintenance/MaintainInstrumentList.aspx?ObjectType=" + "<%=_IsSpareParts%>" + "&InfoType=" + infoType, "维修信息");
                    return false;
                }
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

                //行编辑按钮
                function ShowEditInstrumentForm(InstrumentInstanceGuid) {
                    var oWnd = window.radopen("ShowFitting.aspx?Type=edit&ScrappedInstrumentUid=" + ScrappedInstrumentUid, "PointDialog");
                    oWnd.SetTitle("添加仪器信息")
                    oWnd.Title = "添加仪器信息";
                    oWnd.maximize();
                }

                function ShowEditFittingForm(InstrumentInstanceGuid) {
                    
                    var oWnd = window.radopen("ShowFitting.aspx?systemType=<%=hdType.Value%>&InstrumentInstanceGuid=" + InstrumentInstanceGuid, "PointDialog");
                    oWnd.SetTitle("显示配件信息")
                    oWnd.Title = "显示配件信息";
                    oWnd.maximize();
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
                        <telerik:AjaxUpdatedControl ControlID="radlDataType" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
            <ClientEvents OnRequestStart="onRequestStart" OnResponseEnd="onRequestEnd" />
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="RadSplitter2" runat="server" Width="100%" LiveResize="true" ResizeWithParentPane="true" Height="100%">
            <telerik:RadPane ID="navigationPane" runat="server" Width="20%"
                ShowContentDuringLoad="false">
                <telerik:RadTreeView ID="RadTreeView1" runat="server" CheckBoxes="true" CheckChildNodes="true" TriStateCheckBoxes="true">
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
                                <td class="title" style="width: 100px; text-align: center;">开始时间:
                                </td>
                                <td class="content" style="width: 200px;" id="timeq">
                                    <telerik:RadDatePicker ID="dayBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                        DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd"
                                        Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                        Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                        Calendar-FastNavigationSettings-CancelButtonCaption="取消" />
                                </td>
                                <td class="title" style="width: 100px; text-align: center;">结束时间:
                                </td>
                                <td class="content" style="width: 200px;" id="Td1">
                                    <telerik:RadDatePicker ID="dayEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                        DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd"
                                        Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                        Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                        Calendar-FastNavigationSettings-CancelButtonCaption="取消" />
                                </td>
                                <td class="content" align="left" style="width: 140px;">
                                    <asp:CheckBox ID="IsStatistical" Checked="false" Text="曾用仪器查询" runat="server" />
                                </td>
                                <td class="content" align="left" rowspan="2">
                                    <asp:ImageButton ID="btnSearch" runat="server" OnClientClick="return OnClientClicking()" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                                </td>
                            </tr>
                            <tr>
                                <td class="title" style="width: 100px; text-align: center;">查询维度:
                                </td>
                                <td class="content" style="width: 460px;" colspan="4">
                                    <asp:RadioButtonList ID="radlDataType" Width="400px" OnSelectedIndexChanged="radlDataType_SelectedIndexChanged" runat="server" AutoPostBack="true" RepeatDirection="Horizontal">
                                    </asp:RadioButtonList>
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
                            OnNeedDataSource="gridMaintenance_NeedDataSource" OnColumnCreated="gridMaintenance_ColumnCreated"
                            CssClass="RadGrid_Customer">
                            <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
                            <MasterTableView DataKeyNames="InstrumentInstanceGuid" ClientDataKeyNames="InstrumentInstanceGuid" GridLines="None" TableLayout="Fixed" CommandItemDisplay="Top" IsFilterItemExpanded="False"
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
                                    <telerik:GridTemplateColumn HeaderText="操作" UniqueName="InstrumentInstanceGuid" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" HeaderButtonType="None" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                                        <ItemTemplate>
                                            <%--当前测点添加仪器<img id="btnInstrument" style="cursor: pointer;" alt="编辑" title="点击编辑" src="../../../Resources/Images/icons/page_edit.png"
                                    onclick="" />
                                     <br>--%>
                                  当前仪器添加配件<img id="btnFitting" style="cursor: pointer;" alt="编辑" title="点击编辑" src="../../../Resources/Images/icons/page_edit.png"
                                      onclick="ShowEditFittingForm('<%# DataBinder.Eval(Container, "DataItem.InstrumentInstanceGuid")%>')" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <%--                                    <telerik:GridBoundColumn HeaderText="测点" UniqueName="ObjectName" DataField="ObjectName" HeaderStyle-Width="100px" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                    <telerik:GridBoundColumn HeaderText="仪器类型" UniqueName="InstanceName" DataField="InstanceName" ItemStyle-Width="120px" HeaderStyle-Width="120" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                    <telerik:GridBoundColumn HeaderText="仪器型号" UniqueName="SpecificationModel" DataField="SpecificationModel" HeaderStyle-Width="100px" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                    <telerik:GridBoundColumn HeaderText="系统编号" UniqueName="FixedAssetNumber" DataField="FixedAssetNumber" HeaderStyle-Width="100px" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
                                    <telerik:GridBoundColumn HeaderText="时间" UniqueName="OperateDate" DataField="OperateDate" HeaderStyle-Width="150px" ItemStyle-Width="150px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                    <telerik:GridBoundColumn HeaderText="事件" UniqueName="Status" DataField="Status" HeaderStyle-Width="120px" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                    <telerik:GridBoundColumn HeaderText="执行人" UniqueName="Description" DataField="Description" HeaderStyle-Width="200px" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
                                    <telerik:GridBoundColumn HeaderText="备注" UniqueName="Note" DataField="Note" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />--%>
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
            </telerik:RadPane>
        </telerik:RadSplitter>
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" VisibleStatusbar="false" Behaviors="Move,Pin,Reload,Close" EnableShadow="true">
            <Windows>
                <telerik:RadWindow ID="InstrumentDialog" runat="server" Height="1000px" Width="530px" Skin="Outlook" IconUrl="~/App_Themes/Office2007/images/RadGridHeaderBg2.png"
                    Title="监测仪器" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" />
            </Windows>
            <Localization Close="关闭" Maximize="最大化" Minimize="最小化" Restore="还原" Reload="刷新" PinOff="移动"
                PinOn="固定" />
        </telerik:RadWindowManager>
        <asp:HiddenField ID="hdType" runat="server" />
    </form>
</body>
</html>
