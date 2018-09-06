<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InspectMaintenance.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.OperatingMaintenance.InspectMaintenance" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1"
            UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="pointCbxRsm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="pointCbxRsm" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" Width="100%"
            BorderWidth="0" BorderStyle="None" BorderSize="0">
            <%-- <telerik:RadPane ID="paneBasicInfo" runat="server" Height="60px" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="width: 100%; height: 100%" cellspacing="1" cellpadding="0" class="Table_Customer"
                    border="0">
                    <tr>
                        <td class="title" style="width: 80px">站点
                        </td>
                        <td class="content" style="width: 180px;">
                            <CbxRsm:PointCbxRsm runat="server" ApplicationType="Water" CbxWidth="220" CbxHeight="350" MultiSelected="false" DropDownWidth="520" ID="pointCbxRsm"></CbxRsm:PointCbxRsm>
                        </td>
                        <td class="title" style="width: 80px; text-align: center;">测定日期
                        </td>
                        <td class="content" style="width: 180px;">
                            <telerik:RadDateTimePicker ID="dtpTime" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd HH:mm:ss"
                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" Width="220"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                TimeView-HeaderText="小时" />
                        </td>
                        <td class="title" style="width: 80px; text-align: center;">操作人员
                        </td>
                        <td class="content" style="width: 180px;">
                            <telerik:RadTextBox runat="server" ID="rtxPerson" Width="220px"></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>--%>
            <telerik:RadPane ID="paneTab" runat="server" Scrolling="None" Width="100%" Height="26px"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadTabStrip ID="tabStrip" runat="server" SelectedIndex="0" MultiPageID="multiPage"
                    CssClass="RadTabStrip_Customer">
                    <Tabs>
                        <telerik:RadTab Text="周巡检">
                        </telerik:RadTab>
                        <telerik:RadTab Text="月巡检">
                        </telerik:RadTab>
                        <telerik:RadTab Text="季巡检">
                        </telerik:RadTab>
                        <telerik:RadTab Text="年巡检">
                        </telerik:RadTab>
                    </Tabs>
                </telerik:RadTabStrip>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadMultiPage ID="multiPage" runat="server" Width="100%" Height="100%" ScrollBars="Hidden" SelectedIndex="0">
                    <telerik:RadPageView ID="pvWeek" runat="server" Visible="true" ContentUrl="WeekMaintenanceSearch.aspx">
                    </telerik:RadPageView>
                    <telerik:RadPageView ID="pvMonth" runat="server" Visible="true" ContentUrl="MonthMaintenance.aspx">
                    </telerik:RadPageView>
                    <telerik:RadPageView ID="pvSeason" runat="server" Visible="true" ContentUrl="SeasonMaintenance.aspx">
                    </telerik:RadPageView>
                    <telerik:RadPageView ID="pvYear" runat="server" Visible="true" ContentUrl="YearMaintenance.aspx">
                    </telerik:RadPageView>
                </telerik:RadMultiPage>
            </telerik:RadPane>
        </telerik:RadSplitter>
    </form>
</body>
</html>
