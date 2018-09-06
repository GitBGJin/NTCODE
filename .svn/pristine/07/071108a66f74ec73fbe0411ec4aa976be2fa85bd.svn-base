<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InstrumentDataSearch.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.QualityControl.InstrumentDataSearch" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript" src="../../../Resources/JavaScript/FrameJS.js"></script>
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
                        var MasterTable = $find("<%= gridInstrument.ClientID %>").get_masterTableView();
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
                function RowClick() {
                    var moduleGuide = "caa63d83-99e4-4d9c-9444-f8fca8381064";
                    OpenFineUIWindow(moduleGuide, "Pages/EnvAir/OperatingMaintenance/QueryByPoint.aspx?systemType=2", "按测点查询")
                }
            </script>
        </telerik:RadScriptBlock>
        <input id="radGridColWidth" type="hidden" runat="server" value="120" />
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
             <%--   <telerik:AjaxSetting AjaxControlID="instrumentName">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="InstrumentSN" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>--%>
                <telerik:AjaxSetting AjaxControlID="gridInstrument">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridInstrument" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridRTB">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridInstrument" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="splitter">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridInstrument" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridInstrument" LoadingPanelID="RadAjaxLoadingPanel1" />
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
                        <td class="title" style="width: 120px; text-align: center;">仪器编号:
                        </td>
                        <td class="content" style="width: 200px;">
                            <asp:DropDownList runat="server" ID="InstrumentSN" Width="180px">
                            </asp:DropDownList>
                        </td>
                        <td class="title" style="width: 150px; text-align: center;">仪器类型:
                        </td>
                        <td class="content" style="width: 260px;">
                            <telerik:RadComboBox runat="server" ID="instrumentName" Width="240px" CheckBoxes="true" Localization-CheckAllString="全选" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput" OnSelectedIndexChanged="instrumentName_SelectedIndexChanged" AutoPostBack="true">
                            </telerik:RadComboBox>
                        </td>
                        <td class="title" style="width: 150px">测点:
                        </td>
                        <td class="content" style="width: 240px;">
                            <CbxRsm:PointCbxRsm runat="server" ApplicationType="Air" CbxWidth="220" CbxHeight="350" MultiSelected="true" DropDownWidth="520" ID="pointCbxRsm"></CbxRsm:PointCbxRsm>
                        </td>
                        <td class="title" style="width: 120px; text-align: center;">仪器状态:
                        </td>
                        <td class="content" style="width: 200px;">
                            <telerik:RadComboBox runat="server" ID="instrumentState" Width="160px" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Localization-CheckAllString="全选" CheckedItemsTexts="DisplayAllInInput">
                            </telerik:RadComboBox>
                        </td>
                        <td class="content" align="left" rowspan="2">
                            <asp:ImageButton ID="btnSearch" runat="server" OnClientClick="return OnClientClicking()" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                        </td>
                    </tr>
                    <tr>
                        <td class="title" style="width: 120px; text-align: center;">制造商:
                        </td>
                        <td class="content" style="width: 200px;">
                            <telerik:RadComboBox runat="server" ID="Operator" Width="180px" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Localization-CheckAllString="全选" CheckedItemsTexts="DisplayAllInInput">
                            </telerik:RadComboBox>
                        </td>
                        <td class="title" style="width: 150px; text-align: center;">购置开始时间:
                        </td>
                        <td class="content">
                            <telerik:RadDatePicker ID="dayBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消" />
                        </td>
                        <td class="title" style="width: 150px; text-align: center;">购置截止时间：
                        </td>
                        <td class="content">
                            <telerik:RadDatePicker ID="dayEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消" />
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadGrid ID="gridInstrument" runat="server" GridLines="None" Height="100%" Width="100%"
                    AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                    AutoGenerateColumns="False" AllowMultiRowSelection="false"
                    EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                    ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                    OnNeedDataSource="gridInstrument_NeedDataSource" OnItemDataBound="gridInstrument_ItemDataBound"
                    CssClass="RadGrid_Customer">
                    <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
                    <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="Top" IsFilterItemExpanded="False"
                        InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                        <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true" RefreshText="查询" ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                        <CommandItemTemplate>
                            <telerik:RadToolBar BorderWidth="0" ID="gridRTB" SkinID="Query" CssClass="RadToolBar_Customer" AutoPostBack="true"
                                runat="server" Width="50%" OnButtonClick="gridRTB_ButtonClick" />
                        </CommandItemTemplate>
                        <Columns>
                            <telerik:GridBoundColumn HeaderText="仪器类型" UniqueName="InstrumentType" DataField="InstrumentType" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="140px" />
                            <telerik:GridBoundColumn HeaderText="仪器编号" UniqueName="InstrumentName" DataField="InstrumentName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" />
                            <telerik:GridBoundColumn HeaderText="仪器型号" UniqueName="SpecificationModel" DataField="SpecificationModel" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" />
                            <telerik:GridBoundColumn HeaderText="测点" UniqueName="pointName" DataField="pointName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" />
                            <telerik:GridBoundColumn HeaderText="仪器状态" UniqueName="State" DataField="State" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" />
                            <telerik:GridBoundColumn HeaderText="购置时间" UniqueName="BuyDate" DataField="BuyDate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" />
                            <telerik:GridBoundColumn HeaderText="登记时间" UniqueName="RegistrationDate" DataField="RegistrationDate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" />
                            <telerik:GridBoundColumn HeaderText="最后检定时间" UniqueName="LastDate" DataField="LastDate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" />
                            <telerik:GridBoundColumn HeaderText="最近维护时间" UniqueName="RecentDate" DataField="RecentDate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" />
                            <telerik:GridBoundColumn HeaderText="下次鉴定时间" UniqueName="NextDate" DataField="NextDate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" />
                            <telerik:GridBoundColumn HeaderText="品牌" UniqueName="Brand" DataField="Brand" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" />
                            <telerik:GridBoundColumn HeaderText="制造商" UniqueName="ManufacturerName" DataField="ManufacturerName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" />
                            <telerik:GridBoundColumn HeaderText="维护商" UniqueName="TeamName" DataField="TeamName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" />
                            <telerik:GridBoundColumn HeaderText="维护联系人/电话" UniqueName="UserInfo" DataField="UserInfo" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" />
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
