<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MonthMaintenance.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.OperatingMaintenance.MonthMaintenance" %>

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
         <table style="width: 100%" class="Table_Customer">
            <tr class="btnTitle">
                <td class="btnTitle">
                    <asp:ImageButton ID="btnSave" runat="server" OnClick="btnSave_Click" SkinID="ImgBtnSave" />
                </td>
            </tr>

        </table>
        <table id="Tb" style="width: 100%; height: 60px" cellspacing="1" cellpadding="0" class="Table_Customer"
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
        <telerik:RadGrid ID="gridMonth" runat="server" GridLines="None" Height="100%" Width="100%"
            AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
            AutoGenerateColumns="False" AllowMultiRowSelection="false"
            EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
            ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
            OnNeedDataSource="gridMonth_NeedDataSource" OnItemDataBound="gridMonth_ItemDataBound"
            CssClass="RadGrid_Customer">
            <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False"
                InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="序号" UniqueName="Row" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate><%#Container.DataSetIndex + 1%></ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn HeaderText="月维护保养" DataField="TestItems" UniqueName="TestItems" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn HeaderText="实施日期" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <telerik:RadDateTimePicker ID="RadDateTimePicker1" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd HH:00"
                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" Width="220"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                TimeView-HeaderText="小时" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="备注" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <telerik:RadTextBox ID="RadTextBox1" runat="server">
                            </telerik:RadTextBox>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
    </form>
</body>
</html>
