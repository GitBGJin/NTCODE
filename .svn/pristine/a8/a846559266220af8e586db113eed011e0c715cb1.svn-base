<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AirQualittySeasonReport.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Report.AirQualittySeasonReport" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        .td1 {
            border: solid #000 1px;
            width: 100px;
        }

        .td2 {
            border: solid #000 1px;
            width: 150px;
        }
    </style>
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
                <td>
                    <asp:ImageButton ID="btnSave" OnClick="btnSave_Click" runat="server" CssClass="RadToolBar_Customer" SkinID="ImgBtnSave" /></td>
            </tr>
            <tr>
                <td style="text-align: center; width: 100px">报表时间：
                </td>
                <td class="content" style="width: 100px;">
                    <telerik:RadComboBox runat="server" ID="Year" Width="90px" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput" OnSelectedIndexChanged="Year_SelectedIndexChanged" AutoPostBack="true">
                    </telerik:RadComboBox>
                </td>
                <td class="content" style="width: 100px;">
                    <telerik:RadComboBox runat="server" ID="Season" Width="90px" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput" OnSelectedIndexChanged="Seanson_SelectedIndexChanged" AutoPostBack="true">
                    </telerik:RadComboBox>
                </td>
                <td class="title" style="width: 100px">考核基数：
                </td>
                <td class="content" style="width: 100px;">
                    <telerik:RadComboBox runat="server" ID="YearBegin" Width="90px" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput"
                        Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                        Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-OkButtonCaption="确定"
                        Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-CancelButtonCaption="取消">
                    </telerik:RadComboBox>
                </td>
                <td style="text-align: center; width: 100px">日期范围：</td>
                <td style="width: 400px">
                    <asp:TextBox runat="server" BorderWidth="0" BorderColor="#ffffff" ID="txtDateF" ReadOnly="true" Width="100px"></asp:TextBox>~<asp:TextBox runat="server" ID="txtDateT" BorderWidth="0" BorderColor="#ffffff" ReadOnly="true" Width="100px"></asp:TextBox></td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <%-- <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="保存" OnClientClick="RefreshParent()" />--%>
                    <asp:Button ID="btnExport" runat="server" Text="下载" OnClick="btnExport_Click" Visible="false" /></td>
            </tr>
        </table>
        <table style="margin-left: 1%; margin-right: 10%; width: 100%; text-align: center">
            <tr>
                <td style="text-align: center; font-size: 26px; width: 100%">
                    <label runat="server" id="Label1"></label>
                </td>
            </tr>
            <tr>
                <td style="font-size: 22px; width: 100%">苏州市环境监测中心
                </td>
            </tr>

            <tr>
                <td style="font-size: 20px; margin-left: 5px; width: 100%">
                    <asp:TextBox ID="M1" runat="server" MaxLength="500" Height="50px" Width="80%" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>

            <tr>
                <td style="font-size: 20px; margin-left: 5px; width: 100%">
                    <asp:TextBox ID="M2" runat="server" MaxLength="500" Height="50px" Width="80%" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>

            <tr>
                <td style="font-size: 20px; margin-left: 5px; width: 100%">
                    <asp:TextBox ID="M3" runat="server" MaxLength="500" Height="50px" Width="80%" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 22px; width: 100%">表1  污染持续天数及污染程度简表
                </td>
            </tr>
        </table>
        <table style="margin-left: 20%; margin-right: 5%">
            <telerik:RadGrid ID="grdSituation" runat="server" GridLines="None" Width="90%"
                AllowPaging="True"  AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                AutoGenerateColumns="False" AllowMultiRowSelection="false"
                EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                OnNeedDataSource="grdSituation_NeedDataSource" OnItemDataBound="grdSituation_ItemDataBound"
                CssClass="RadGrid_Customer">
                <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
                <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="Top" IsFilterItemExpanded="False"
                    InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true" RefreshText="查询" ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                    <CommandItemTemplate>
                    </CommandItemTemplate>
                    <Columns>
                        <telerik:GridBoundColumn HeaderText="日期" DataField="DateTime" UniqueName="DateTime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <telerik:GridBoundColumn HeaderText="持续天数" DataField="ContinuousDays" UniqueName="ContinuousDays" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <telerik:GridBoundColumn HeaderText="轻度污染" DataField="LightPollution" UniqueName="LightPollution" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <telerik:GridBoundColumn HeaderText="中度污染" DataField="ModeratePollution" UniqueName="ModeratePollution" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <telerik:GridBoundColumn HeaderText="重度污染" DataField="HighPollution" UniqueName="HighPollution" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <telerik:GridBoundColumn HeaderText="严重污染" DataField="SeriousPollution" UniqueName="SeriousPollution" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </table>
        <table style="margin-left: 1%; margin-right: 10%; width: 100%; text-align: center">
            <tr>
                <td></td>
            </tr>
            <tr>
                <td style="font-size: 20px; margin-left: 5px; width: 100%">
                    <asp:TextBox ID="M4" runat="server" MaxLength="500" Height="50px" Width="80%" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>

            <tr>
                <td style="font-size: 20px; margin-left: 5px; width: 100%">
                    <asp:TextBox ID="M5" runat="server" MaxLength="500" Height="50px" Width="80%" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
