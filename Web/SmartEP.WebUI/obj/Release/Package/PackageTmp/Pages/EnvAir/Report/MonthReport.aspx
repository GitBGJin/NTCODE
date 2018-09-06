<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MonthReport.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Report.MonthReport" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
            <script type="text/javascript">
                function RefreshParent() {
                    this.parent.Refresh_Grid(true);
                }
                //行编辑按钮 pageTypeID和waterOrAirType参数名称固定pageTypeID：该页面的ID;waterOrAirType：水或气，0：表示水，1：表示气
                function ShowDetails() {

                }
            </script>
        </telerik:RadScriptBlock>
        <table>
            <tr>
                <td colspan="2">
                    <asp:ImageButton ID="btnSave" OnClick="btnSave_Click" runat="server" CssClass="RadToolBar_Customer" SkinID="ImgBtnSave" /></td>
            </tr>
        <%--    <tr>
                <td style="text-align: center">测点：</td>
                <td colspan="2">
                    <CbxRsm:PointCbxRsm runat="server" ApplicationType="Air" CbxWidth="400" CbxHeight="350" MultiSelected="true" DropDownWidth="420" ID="pointCbxRsm"></CbxRsm:PointCbxRsm>
                </td>
            </tr>--%>
            <tr>
                <td style="text-align: center">报表时间：</td>
                <td>
                    <telerik:RadMonthYearPicker ID="dtpBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM"
                        DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday"
                        Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                        Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-OkButtonCaption="确定"
                        Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-CancelButtonCaption="取消" />
                </td>
            </tr>
            <%--    <tr>
                <td>日期范围：</td>
                <td colspan="2">
                    <asp:TextBox runat="server" BorderWidth="0" BorderColor="#ffffff" ID="txtDateF" ReadOnly="true" Width="100px"></asp:TextBox>~<asp:TextBox runat="server" ID="txtDateT" BorderWidth="0" BorderColor="#ffffff" ReadOnly="true" Width="100px"></asp:TextBox></td>
            </tr>--%>
            <tr>
                <td></td>
                <td colspan="2">
                    <%--  <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="保存" OnClientClick="RefreshParent()" />--%>
                    <asp:Button ID="btnExport" runat="server" Text="下载" OnClick="btnExport_Click" Visible="false" /></td>
            </tr>
        </table>
        <asp:HiddenField ID="hdPDate" runat="server" />
    </form>
</body>
</html>



