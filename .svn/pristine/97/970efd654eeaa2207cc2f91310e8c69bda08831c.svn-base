<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AirQualityReportMessage.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Report.AirQualityReportMessage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script type="text/javascript" src="../../../Resources/JavaScript/FrameJS.js"></script>
    <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
        <script type="text/javascript" language="javascript">
            function OnClientClicking(sender, args) {
                var dayDate = $find("<%= dayDate.ClientID %>").get_selectedDate();
                var date1 = $find("<%= dtpBegin.ClientID %>").get_selectedDate();
                var date2 = $find("<%= dtpEnd.ClientID %>").get_selectedDate();
                var date3 = $find("<%= seasonBegin.ClientID %>").get_selectedDate();
                var date4 = $find("<%= seasonEnd.ClientID %>").get_selectedDate();
                if (date1 == null || date1 == "" || date2 == null || date2 == "") {
                    args._cancel = true;
                    alert("时间不能为空！");
                    //sender.set_autoPostBack(false);
                    //return false;

                }
                else {
                    return true;
                }
                if (date3 == null || date3 == "" || date4 == null || date4 == "") {
                    args._cancel = true;
                    alert("时间不能为空！");
                    //sender.set_autoPostBack(false);
                    //return false;

                }
                else {
                    return true;
                }
                if (dayDate == null || dayDate == "") {
                    args._cancel = true;
                    alert("时间不能为空！");
                    //sender.set_autoPostBack(false);
                    //return false;

                }
                else {
                    return true;
                }
                if (date1 > date2) {
                    alert("开始时间不能大于终止时间！");
                    return false;
                } else {
                    return true;
                }
                if (date3 > date4) {
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

            function RowClick() {
                var moduleGuide = "";
                OpenFineUIWindow(moduleGuide, "http://www.1183300.com/index.htm", "企信通")
                OpenFineUIWindow(moduleGuide, "http://www.szhbj.gov.cn/hbjlogin/WebBuilderMis/webbuilder_login.aspx?ReturnUrl=%2fhbjlogin%2fLogin.aspx", "环保局后平台")
            }
        </script>
    </telerik:RadScriptBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1"
            UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="dayDate">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="taMessage" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="dtpEnd">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="dtpBegin" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="taMessage" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="dtpBegin">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="dtpEnd" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="taMessage" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="seasonBegin">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="seasonEnd" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="taMessage" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="seasonEnd">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="seasonBegin" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="taMessage" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ddlAuditor">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="taMessage" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="DeleteAudit">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="taMessage" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridMessage" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
            <ClientEvents OnRequestStart="onRequestStart" OnResponseEnd="onRequestEnd" />
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" BorderWidth="0" BorderSize="0" Height="100%"
            Width="100%">
            <telerik:RadPane ID="paneWhere" runat="server" Width="100%" Scrolling="None" Height="200"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <fieldset>
                    <legend style="color: Blue; font-weight: bold;">编辑区▼</legend>

                    <table id="Tb" style="width: 100%;" cellspacing="1" cellpadding="0" class="Table_Customer"
                        border="0">
                        <tr>
                            <td class="title" style="width: 80px; text-align: center">日期:
                            </td>
                            <td class="content" style="width: 300px;">
                                <div runat="server" id="dbtDay">
                                    <table>
                                        <tr>
                                            <td>
                                                <telerik:RadDatePicker runat="server" ID="dayDate" AutoPostBack="true" OnSelectedDateChanged="dayDate_SelectedDateChanged"
                                                    MinDate="1900-01-01" DateInput-Font-Size="10" DateInput-Font-Bold="false" DateInput-DateFormat="yyyy-MM-dd"
                                                    DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" Timeview-HeaderText="小时">
                                                </telerik:RadDatePicker>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div runat="server" id="dbtWeek">
                                    <table>
                                        <tr>
                                            <td>
                                                <telerik:RadDatePicker runat="server" ID="dtpBegin" AutoPostBack="true" OnSelectedDateChanged="dtpBegin_SelectedDateChanged"
                                                    MinDate="1900-01-01" DateInput-Font-Size="10" DateInput-Font-Bold="false" DateInput-DateFormat="yyyy-MM-dd"
                                                    DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" Timeview-HeaderText="小时">
                                                </telerik:RadDatePicker>
                                            </td>
                                            <td>&nbsp;至&nbsp;
                                            </td>
                                            <td>
                                                <telerik:RadDatePicker ID="dtpEnd" runat="server" AutoPostBack="true" OnSelectedDateChanged="dtpEnd_SelectedDateChanged"
                                                    MinDate="1900-01-01" DateInput-Font-Size="10" DateInput-Font-Bold="false" DateInput-DateFormat="yyyy-MM-dd"
                                                    DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" Timeview-HeaderText="小时">
                                                </telerik:RadDatePicker>
                                            </td>

                                        </tr>
                                    </table>
                                </div>
                                <div runat="server" id="dbtSeason">
                                    <table>
                                        <tr>
                                            <td>
                                                <telerik:RadMonthYearPicker runat="server" ID="seasonBegin" AutoPostBack="true" OnSelectedDateChanged="seasonBegin_SelectedDateChanged"
                                                    MinDate="1900-01-01" DateInput-Font-Size="10" DateInput-Font-Bold="false" DateInput-DateFormat="yyyy-MM" MonthYearNavigationSettings-CancelButtonCaption="取消"
                                                    DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday" MonthYearNavigationSettings-OkButtonCaption="确定"
                                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" Timeview-HeaderText="小时">
                                                </telerik:RadMonthYearPicker>
                                            </td>
                                            <td>&nbsp;至&nbsp;
                                            </td>
                                            <td>
                                                <telerik:RadMonthYearPicker ID="seasonEnd" runat="server" AutoPostBack="true" OnSelectedDateChanged="seasonEnd_SelectedDateChanged"
                                                    MinDate="1900-01-01" DateInput-Font-Size="10" DateInput-Font-Bold="false" DateInput-DateFormat="yyyy-MM" MonthYearNavigationSettings-CancelButtonCaption="取消"
                                                    DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday" MonthYearNavigationSettings-OkButtonCaption="确定"
                                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" Timeview-HeaderText="小时">
                                                </telerik:RadMonthYearPicker>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                            <td style="width: 200px;">
                                <div runat="server" id="auditor">
                                    <table>
                                        <tr>
                                            <td style="width: 80px">审核人</td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlAuditor" Width="120px" OnSelectedIndexChanged="ddlAuditor_SelectedIndexChanged" AutoPostBack="true">
                                                    <asp:ListItem Text="姚玉刚" Value="0" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="邹强" Value="1"></asp:ListItem>
                                                </asp:DropDownList></td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                            <td class="content" style="width: 100px;">
                                <telerik:RadButton ID="DeleteAudit" runat="server" BackColor="#3A94D3" ForeColor="White" AutoPostBack="true" OnClick="DeleteAudit_Click">
                                    <ContentTemplate>
                                        <asp:Label runat="server" ID="Label4" ForeColor="White" Text="重新加载"></asp:Label>
                                    </ContentTemplate>
                                </telerik:RadButton>
                            </td>
                            <td class="content" style="width: 100px;">
                                <asp:ImageButton ID="btnExport" runat="server" OnClientClick="return OnClientClicking()" OnClick="btnExport_Click" SkinID="ImgBtnSave" />
                            </td>
                            <td></td>
                            <td style="width: 200px;"></td>
                        </tr>
                        <tr>
                            <td class="title" style="width: 80px; vertical-align: top">短信内容
                            </td>
                            <td class="content" style="text-align: left; font-size: large" colspan="4">
                                <textarea style="width: 90%; max-width: 100%; height: auto; overflow-y: visible; overflow-wrap: normal; font-size: large" runat="server" id="taMessage"></textarea>
                            </td>
                            <td class="content" style="width: 80px; text-align: left; font-size: large">
                                <a href="http://www.1183300.com" target="_blank">企信通</a>
                            </td>
                                  <td class="content" style="width: 100px; text-align: left; font-size: large">
                                <a href="http://www.szhbj.gov.cn/hbjlogin/WebBuilderMis/webbuilder_login.aspx?ReturnUrl=%2fhbjlogin%2fLogin.aspx" target="_blank">环保局后平台</a>
                            </td>
                        </tr>

                    </table>
                </fieldset>
                <fieldset>
                    <legend style="color: Blue; font-weight: bold;">数据查询▼</legend>

                    <table>
                        <tr>
                            <td class="title" style="width: 80px; text-align: center">日期:
                            </td>
                            <td class="content" style="width: 500px;">
                                <div runat="server" id="Div1">
                                    <table>
                                        <tr>
                                            <td>
                                                <telerik:RadDatePicker runat="server" ID="daybegin"
                                                    MinDate="1900-01-01" DateInput-Font-Size="10" DateInput-Font-Bold="false" DateInput-DateFormat="yyyy-MM-dd"
                                                    DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" Timeview-HeaderText="小时">
                                                </telerik:RadDatePicker>
                                            </td>
                                            <td>
                                                <telerik:RadDatePicker runat="server" ID="dayend"
                                                    MinDate="1900-01-01" DateInput-Font-Size="10" DateInput-Font-Bold="false" DateInput-DateFormat="yyyy-MM-dd"
                                                    DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" Timeview-HeaderText="小时">
                                                </telerik:RadDatePicker>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div runat="server" id="Div2">
                                    <table>
                                        <tr>
                                            <td>
                                                <telerik:RadDatePicker runat="server" ID="weekbegin"
                                                    MinDate="1900-01-01" DateInput-Font-Size="10" DateInput-Font-Bold="false" DateInput-DateFormat="yyyy-MM-dd"
                                                    DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" Timeview-HeaderText="小时">
                                                </telerik:RadDatePicker>
                                            </td>
                                            <td>&nbsp;至&nbsp;
                                            </td>
                                            <td>
                                                <telerik:RadDatePicker ID="weekend" runat="server"
                                                    MinDate="1900-01-01" DateInput-Font-Size="10" DateInput-Font-Bold="false" DateInput-DateFormat="yyyy-MM-dd"
                                                    DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" Timeview-HeaderText="小时">
                                                </telerik:RadDatePicker>
                                            </td>

                                        </tr>
                                    </table>
                                </div>
                                <div runat="server" id="Div3">
                                    <table>
                                        <tr>
                                            <td>
                                                <telerik:RadMonthYearPicker runat="server" ID="monthbegin"
                                                    MinDate="1900-01-01" DateInput-Font-Size="10" DateInput-Font-Bold="false" DateInput-DateFormat="yyyy-MM" MonthYearNavigationSettings-CancelButtonCaption="取消"
                                                    DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday" MonthYearNavigationSettings-OkButtonCaption="确定"
                                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" Timeview-HeaderText="小时">
                                                </telerik:RadMonthYearPicker>
                                            </td>
                                            <td>&nbsp;至&nbsp;
                                            </td>
                                            <td>
                                                <telerik:RadMonthYearPicker ID="monthend" runat="server"
                                                    MinDate="1900-01-01" DateInput-Font-Size="10" DateInput-Font-Bold="false" DateInput-DateFormat="yyyy-MM" MonthYearNavigationSettings-CancelButtonCaption="取消"
                                                    DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday" MonthYearNavigationSettings-OkButtonCaption="确定"
                                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" Timeview-HeaderText="小时">
                                                </telerik:RadMonthYearPicker>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                            <td class="content" style="text-align: left">
                                <asp:ImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </telerik:RadPane>
            <telerik:RadPane ID="panegrid" runat="server" Width="100%" Scrolling="None" Height="100%"
                BorderWidth="0" BorderStyle="None" BorderSize="0">

                <telerik:RadGrid ID="gridMessage" runat="server" GridLines="None" Height="100%" Width="100%"
                    AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                    AutoGenerateColumns="False" AllowMultiRowSelection="false"
                    EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                    ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                    OnNeedDataSource="gridMessage_NeedDataSource" OnItemDataBound="gridMessage_ItemDataBound"
                    CssClass="RadGrid_Customer">
                    <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False"
                        InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="序号" UniqueName="Row" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate><%#Container.DataSetIndex + 1%></ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn HeaderText="时间" HeaderStyle-Width="110px" DataField="daterange" UniqueName="daterange" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="短信内容" DataField="messagetext" UniqueName="messagetext" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="编制" HeaderStyle-Width="100px" ItemStyle-Width="100px" DataField="CreatUser" UniqueName="CreatUser" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="更新" HeaderStyle-Width="100px" ItemStyle-Width="100px" DataField="UpdateUser" UniqueName="UpdateUser" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                        </Columns>
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
