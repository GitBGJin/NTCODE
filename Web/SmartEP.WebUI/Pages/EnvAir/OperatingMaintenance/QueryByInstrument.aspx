<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QueryByInstrument.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.OperatingMaintenance.QueryByInstrument" %>

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
                    date1 = $find("<%= RadDatePickerBegin.ClientID %>").get_selectedDate();
                    date2 = $find("<%= RadDatePickerEnd.ClientID %>").get_selectedDate();
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
                <telerik:AjaxSetting AjaxControlID="TreeView1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ddlInstrument" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="TreeView1" />
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
                        <telerik:AjaxUpdatedControl ControlID="radlDataType" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
            <ClientEvents OnRequestStart="onRequestStart" OnResponseEnd="onRequestEnd" />
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="RadSplitter1" runat="server" BorderSize="0" Width="100%" Height="100%" OnClientLoad="loadSplitter">
            <!-- 左侧测点、因子-->
            <telerik:RadPane runat="server" ID="LeftPane" Width="20%">
                <div>
                    <telerik:RadTreeView ID="TreeView1" runat="server" CheckBoxes="true" OnNodeCheck="TreeView1_NodeCheck" CheckChildNodes="true" TriStateCheckBoxes="true">
                    </telerik:RadTreeView>
                </div>
            </telerik:RadPane>
            <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Both"></telerik:RadSplitBar>
            <!--中间-->
            <telerik:RadPane ID="MiddlePane" runat="server" Scrolling="Y" Width="78%" Height="100%">
                <telerik:RadSplitter ID="RadSplitter2" runat="server" Orientation="Horizontal" BorderSize="0" Width="100%" Height="100%">
                    <telerik:RadPane ID="paneWhere" runat="server" Height="80px" Width="100%" Scrolling="None"
                        BorderWidth="0" BorderStyle="None" BorderSize="0">
                        <table style="width: 100%; text-align: left">
                            <tr>
                                <td class="title" style="width: 70px; text-align: center;">仪器编号：
                                </td>
                                <td class="content" style="width: 280px;">
                                    <telerik:RadComboBox runat="server" ID="ddlInstrument" Localization-CheckAllString="全选" Width="280px" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                                    </telerik:RadComboBox>
                                </td>
                                <td style="width: 70px; text-align: center;">开始时间：</td>
                                <td style="width: 140px;">
                                    <telerik:RadDatePicker Width="120" runat="server" ID="RadDatePickerBegin" AutoPostBack="false"
                                        MinDate="1900-01-01" DateInput-Font-Size="10" DateInput-Font-Bold="false" DateInput-DateFormat="yyyy-MM-dd"
                                        DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                        Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                        Calendar-FastNavigationSettings-CancelButtonCaption="取消" Timeview-HeaderText="小时">
                                    </telerik:RadDatePicker>
                                </td>
                                <td style="width: 70px; text-align: center;">结束时间：</td>
                                <td style="width: 140px;">
                                    <telerik:RadDatePicker Width="120" runat="server" ID="RadDatePickerEnd" AutoPostBack="false"
                                        MinDate="1900-01-01" DateInput-Font-Size="10" DateInput-Font-Bold="false" DateInput-DateFormat="yyyy-MM-dd"
                                        DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                        Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                        Calendar-FastNavigationSettings-CancelButtonCaption="取消" Timeview-HeaderText="小时">
                                    </telerik:RadDatePicker>
                                </td>
                                <td class="content" align="left" rowspan="2">
                                    <asp:ImageButton ID="btnSearch" runat="server" OnClientClick="return OnClientClicking()" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                                </td>
                            </tr>
                            <tr>
                                <td class="title" style="width: 70px; text-align: center;">查询维度：
                                </td>
                                <td class="content" style="width: 280px;" colspan="5">
                                    <asp:RadioButtonList ID="radlDataType" Width="500px" OnSelectedIndexChanged="radlDataType_SelectedIndexChanged" runat="server" AutoPostBack="true" RepeatDirection="Horizontal">
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
                                    <%-- <telerik:GridBoundColumn HeaderText="仪器类型" UniqueName="InstanceName" DataField="InstanceName" ItemStyle-Width="120px" HeaderStyle-Width="120" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                    <telerik:GridBoundColumn HeaderText="仪器名称/编号" UniqueName="instance" DataField="instance" HeaderStyle-Width="120px" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                    <telerik:GridBoundColumn HeaderText="时间" UniqueName="OperateDate" DataField="OperateDate" HeaderStyle-Width="150px" ItemStyle-Width="150px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                    <telerik:GridBoundColumn HeaderText="状态" UniqueName="State" DataField="State" HeaderStyle-Width="120px" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                    <telerik:GridBoundColumn HeaderText="位置" UniqueName="RoomName" DataField="RoomName" HeaderStyle-Width="200px" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
                                    <telerik:GridBoundColumn HeaderText="执行人" UniqueName="OperateUserName" DataField="OperateUserName" HeaderStyle-Width="120px" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
                                    <telerik:GridBoundColumn HeaderText="备注" UniqueName="OperateContent" DataField="OperateContent" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Left" />--%>
                                    <%--  <telerik:GridBoundColumn HeaderText="备注" UniqueName="Note" DataField="Note" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
                                    --%><%--<telerik:GridBoundColumn HeaderText="备注" UniqueName="Note" DataField="Note" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />--%>
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
    </form>
</body>
</html>
