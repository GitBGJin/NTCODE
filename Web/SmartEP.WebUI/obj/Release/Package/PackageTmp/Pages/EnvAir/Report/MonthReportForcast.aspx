<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MonthReportForcast.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Report.MonthReportForcast" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
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
            <tr>
                <td style="text-align: center; width: 80px">区域：</td>
                <td colspan="3">
                    <telerik:RadComboBox runat="server" ID="comboCityProper" Width="360px" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput"
                        Localization-CheckAllString="选择全部">
                        <Items>
                            <telerik:RadComboBoxItem Text="姑苏区" Value="6a4e7093-f2c6-46b4-a11f-0f91b4adf379" Checked="true" />
                            <telerik:RadComboBoxItem Text="吴中区" Value="e1c104f3-aaf3-4d0e-9591-36cdc83be15a" Checked="true" />
                            <telerik:RadComboBoxItem Text="高新区" Value="f320aa73-7c55-45d4-a363-e21408e0aac3" Checked="true" />
                            <telerik:RadComboBoxItem Text="工业园区" Value="69a993ff-78c6-459b-9322-ee77e0c8cd68" Checked="true" />
                            <telerik:RadComboBoxItem Text="相城区" Value="8756bd44-ff18-46f7-aedf-615006d7474c" Checked="true" />
                            <telerik:RadComboBoxItem Text="市区均值" Value="7e05b94c-bbd4-45c3-919c-42da2e63fd43" Checked="true" />
                            <telerik:RadComboBoxItem Text="张家港" Value="66d2abd1-ca39-4e39-909f-da872704fbfd" Checked="true" />
                            <telerik:RadComboBoxItem Text="常熟市" Value="d7d7a1fe-493a-4b3f-8504-b1850f6d9eff" Checked="true" />
                            <telerik:RadComboBoxItem Text="太仓市" Value="57b196ed-5038-4ad0-a035-76faee2d7a98" Checked="true" />
                            <telerik:RadComboBoxItem Text="昆山市" Value="2e2950cd-dbab-43b3-811d-61bd7569565a" Checked="true" />
                            <telerik:RadComboBoxItem Text="吴江区" Value="2fea3cb2-8b95-45e6-8a71-471562c4c89c" Checked="true" />
                            <telerik:RadComboBoxItem Text="全市均值" Value="全市均值" Checked="true" />
                        </Items>
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; width: 80px">报表时间：</td>
                <td style="text-align: left; width: 120px">
                    <telerik:RadMonthYearPicker ID="dtpBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM" Width="100px"
                        DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" OnSelectedDateChanged="dtpBegin_SelectedDateChanged" AutoPostBack="true"
                        Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                        Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-OkButtonCaption="确定"
                        Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-CancelButtonCaption="取消" />
                </td>
                <td style="text-align: center; width: 40px">&nbsp;&nbsp;至</td>
                <td style="text-align: center; width: 120px">
                    <telerik:RadMonthYearPicker ID="dtpEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM" Width="100px"
                        DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" OnSelectedDateChanged="dtpEnd_SelectedDateChanged" AutoPostBack="true"
                        Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                        Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-OkButtonCaption="确定"
                        Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-CancelButtonCaption="取消" />
                </td>
            </tr>
            <tr>
                <td style="text-align: center; width: 80px">日期范围：</td>
                <td colspan="3">
                    <asp:TextBox runat="server" BorderWidth="0" BorderColor="#ffffff" ID="txtDateF" ReadOnly="true" Width="100px"></asp:TextBox>~<asp:TextBox runat="server" ID="txtDateT" BorderWidth="0" BorderColor="#ffffff" ReadOnly="true" Width="100px"></asp:TextBox></td>
            </tr>
            <tr>
                <td></td>
                <td colspan="2">
                    <%-- <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="保存" OnClientClick="RefreshParent()" />--%>
                    <asp:Button ID="btnExport" runat="server" Text="下载" OnClick="btnExport_Click" Visible="false" /></td>
            </tr>
        </table>
    </form>
</body>
</html>


