<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AirAuditStateRemind.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Dock.AirAuditStateRemind" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script type="text/javascript" src="../../../Resources/JavaScript/FrameJS.js"></script>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            function RowClick(pointID, datetime) {
                var PointName = $find("<%= ports.ClientID %>")._selectedText;
                var PointType = "";
                var moduleGuide = "";
                if (PointName == "国控") {
                    PointType = "0";
                    moduleGuide = "8a796776-05e9-4cb6-983d-0d9bd6aa1f3c";
                }
                if (PointName == "省控") {
                    PointType = "3";
                    moduleGuide = "63a3268d-b3bf-400d-9497-78aaaeee9bf5";
                }
                if (PointName == "路边站") {
                    PointType = "7";
                    moduleGuide = "f7b792d0-eedd-4059-bece-30d69b2f8164";
                }
                if (PointName == "区控") {
                    PointType = "4";
                    moduleGuide = "fa1e24cb-2c52-4b9b-be6f-379f3e1eb649";
                }
                if (PointType == "0")
                    OpenFineUIWindow(moduleGuide, "Pages/EnvAir/BaseData/ExternalLinks.aspx?url=http://192.168.0.44:81/Account/Login?ReturnUrl=/")
                else
                    OpenFineUIWindow(moduleGuide, "Pages/EnvAir/Audit/MutilPointAuditData.aspx?PointType=" + PointType + "&DTBegin=" + datetime + "&PointID=" + pointID, PointName + "审核")
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="ports">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ports" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridRemind" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" Width="100%"
            BorderWidth="0" BorderStyle="None" BorderSize="0">
            <telerik:RadPane ID="paneWhere" runat="server" Height="35px" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" class="Table_Customer">
                    <tr>
                        <td class="title" style="width: 100px;">测点类型:
                        </td>
                        <td class="content" style="width: 120px;">
                            <telerik:RadDropDownList runat="server" ID="ports" Width="120px">
                            </telerik:RadDropDownList>
                        </td>
                        <td class="title" style="width: 50px;">日期:
                        </td>
                        <td class="content" style="width: 120px;">
                            <telerik:RadDatePicker ID="dayBegin" runat="server" MinDate="1900-01-01"
                                DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" Width="120"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定" />
                        </td>
                        <td class="title" style="width: 30px;">至
                        </td>
                        <td class="content" style="width: 120px;">
                            <telerik:RadDatePicker ID="dayEnd" runat="server" MinDate="1900-01-01"
                                DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" Width="120"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定" />
                        </td>
                        <td class="content">
                            <asp:ImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadGrid ID="gridRemind" runat="server" GridLines="None" Height="100%" Width="100%"
                    AutoGenerateColumns="false" CssClass="RadGrid_Customer"
                    OnNeedDataSource="gridRemind_NeedDataSource" OnItemDataBound="gridRemind_ItemDataBound">
                    <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False"
                        InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                        <Columns>
                            <telerik:GridBoundColumn HeaderText="测点" DataField="MonitoringPointName" UniqueName="MonitoringPointName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" ItemStyle-Width="0px" />
                            <telerik:GridBoundColumn HeaderText="日期" DataField="Date" UniqueName="Date" DataFormatString="{0:yyyy-MM-dd}" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" ItemStyle-Width="120px" />
                            <telerik:GridBoundColumn HeaderText="审核状况" DataField="AuditStatus" UniqueName="AuditStatus" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" ItemStyle-Width="120px" />
                            <telerik:GridBoundColumn HeaderText="异常情况" DataField="Abnormal" UniqueName="Abnormal" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
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
