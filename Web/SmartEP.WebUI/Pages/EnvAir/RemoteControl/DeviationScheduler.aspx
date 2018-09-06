<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeviationScheduler.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.RemoteControl.DeviationScheduler" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link type="text/css" rel="stylesheet" href="../../../App_Themes/Neptune/Telerik.Web.UI.Skins/Metro/Scheduler.Outlook.css" />
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            function GotoURL(sender, args) {
                //args._appointment_subject
                //window.open("http://baidu.com");
                //alert(args._appointment._serializedResources[0].key);
                window.open("DeviationReport.aspx?startTime=" + args._appointment.get_end().format("yyyy/MM/dd 00:00") + "&portName=" + encodeURI(args._appointment._subject));
                return false;
            }
            function hideActiveToolTip() {
                var tooltip = Telerik.Web.UI.RadToolTip.getCurrent();
                if (tooltip) {
                    tooltip.hide();
                }
            }

            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(beginRequestHandler);
            function beginRequestHandler(sender, args) {
                var prm = Sys.WebForms.PageRequestManager.getInstance();
                if (args.get_postBackElement().id.indexOf('RadScheduler1') != -1) {
                    hideActiveToolTip();
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1"
            UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="RadSchedulerDeviation">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadSchedulerDeviation" />
                        <telerik:AjaxUpdatedControl ControlID="DetalToolTip" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="pointCbxRsm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="pointCbxRsm" />
                        <telerik:AjaxUpdatedControl ControlID="RadSchedulerDeviation" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="DetalToolTip" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="RadDatePickerBegin">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadSchedulerDeviation" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="RadDatePickerEnd">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadSchedulerDeviation" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" Width="100%"
            BorderWidth="0" BorderStyle="None" BorderSize="0">
            <telerik:RadPane ID="paneWhere" runat="server" Height="50px" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <div style="padding-left: 10px;">
                    <table>
                        <tr>
                            <td style="width: 40px">测点</td>
                            <td>
                                <CbxRsm:PointCbxRsm runat="server" ApplicationType="Air" CbxWidth="380" CbxHeight="350" MultiSelected="true" DropDownWidth="520" ID="pointCbxRsm"
                                    OnSelectedChanged="pointCbxRsm_SelectedChanged"></CbxRsm:PointCbxRsm>
                            </td>
                            <td style="width: 60px">开始时间</td>
                            <td>
                                <telerik:RadDatePicker ID="RadDatePickerBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px" AutoPostBack="true"
                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd"
                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                    Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" OnSelectedDateChanged="RadDatePickerBegin_SelectedDateChanged" />
                            </td>
                            <td style="width: 60px">结束时间</td>
                            <td>
                                <telerik:RadDatePicker ID="RadDatePickerEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px" AutoPostBack="true"
                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd"
                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                    Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消"  OnSelectedDateChanged="RadDatePickerBegin_SelectedDateChanged"/>
                            </td>
                        </tr>
                    </table>
                </div>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="90%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <div style="padding-left: 10px;">
                    <telerik:RadScheduler runat="server" ID="RadSchedulerDeviation" Width="99%" Height="98%"
                        FirstDayOfWeek="Monday" LastDayOfWeek="Sunday" OverflowBehavior="Auto" Skin="Outlook"
                        AllowEdit="false" AllowDelete="false"  AllowInsert="false"
                        TimeZoneOffset="00:00:00" HoursPanelTimeFormat="H:mm" ShowFullTime="true"
                        OnClientAppointmentClick="GotoURL" Localization-ShowMore="详情.." Localization-HeaderDay="日"
                        Localization-HeaderMonth="月" Localization-HeaderWeek="周" Localization-HeaderToday="今天"
                        Localization-AllDay="全天" Localization-AdvancedCalendarToday="今天" Localization-AdvancedCalendarOK="确定"
                        Localization-AdvancedCalendarCancel="取消" Localization-Show24Hours="显示24小时" Localization-ShowBusinessHours="显示上班时间"
                        OnAppointmentCreated="RadSchedulerDeviation_AppointmentCreated" OnNavigationCommand="RadSchedulerDeviation_NavigationCommand"
                        OnAppointmentDataBound="RadSchedulerDeviation_AppointmentDataBound" 
                        MonthView-VisibleAppointmentsPerDay="3" OnLoad="RadSchedulerDeviation_Load">
                        <AdvancedForm Modal="true"></AdvancedForm>
                        <WeekView UserSelectable="false" />
                        <AppointmentTemplate>
                            <div style="text-align: center;">
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("Subject") %>' Font-Bold="true"></asp:Label>
                                <%--                                 <asp:Label ID="Label1" runat="server" Text='<%# Eval("Subject") %>'  ForeColor='<%#getPointColor(Eval("TimeZoneId"))%>' Font-Bold="true"></asp:Label>--%>
                            </div>
                        </AppointmentTemplate>
                        <TimelineView UserSelectable="false" />
                        <MonthView UserSelectable="true" />
                        <TimeSlotContextMenuSettings EnableDefault="true"></TimeSlotContextMenuSettings>
                        <AppointmentContextMenuSettings EnableDefault="true"></AppointmentContextMenuSettings>
                    </telerik:RadScheduler>
                    <telerik:RadToolTipManager runat="server" ID="DetalToolTip"
                        Animation="None" Position="BottomRight" HideEvent="LeaveToolTip" Text="Loading..."
                        Width="450" Height="200" AutoTooltipify="true" RelativeTo="Element" OnAjaxUpdate="DetalToolTip_AjaxUpdate">
                    </telerik:RadToolTipManager>
                </div>
            </telerik:RadPane>
        </telerik:RadSplitter>



    </form>
</body>
</html>
