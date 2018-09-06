<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AroundInspectList.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.OperatingMaintenance.AroundInspectList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadSplitter ID="splitterWeek" runat="server" Orientation="Horizontal" Height="80%" Width="100%"
            BorderWidth="0" BorderStyle="None" BorderSize="0">
            <telerik:RadPane ID="paneWhere" runat="server" Height="60px" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="width: 80%; height: 100%;" cellspacing="1" cellpadding="0" class="Table_Customer"
                    border="0">
                    <tr>
                        <td style="float: left">
                            <asp:ImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" SkinID="ImgBtnWord" />
                        </td>
                    </tr>
                    <tr>
                        <td class="title" style="width: 100px; text-align: center;">进站日期：
                        </td>
                        <td class="content" style="width: 360px;">
                            <asp:Label runat="server" ID="dtpTime"></asp:Label>
                        </td>
                        <td class="title" style="width: 180px; text-align: center;">维护/处理时间:
                        </td>
                        <td class="content" id="timeq">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="dtpBegin"></asp:Label>
                                    </td>
                                    <td>&nbsp;&nbsp;至&nbsp;&nbsp;
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="dtpEnd"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="80%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadGrid ID="gridAroundInspect" runat="server" GridLines="None" Height="100%" Width="100%"
                    AllowPaging="false" AllowCustomPaging="false" AllowSorting="false" ShowFooter="false"
                    AutoGenerateColumns="False" AllowMultiRowSelection="false"
                    EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                    ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                    OnNeedDataSource="gridAroundInspect_NeedDataSource" OnItemDataBound="gridAroundInspect_ItemDataBound"
                    CssClass="RadGrid_Customer">
                    <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False"
                        InsertItemPageIndexAction="ShowItemOnCurrentPage" DataKeyNames="RowGuid" NoMasterRecordsText="运维任务没有配置运维项目！">
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="序号" UniqueName="RowGuid" DataField="RowGuid" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate><%#Container.DataSetIndex + 1%></ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn HeaderText="项目分类" UniqueName="UpItem" DataField="UpItem" HeaderStyle-Width="110px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="监测项目" UniqueName="ItemName" DataField="ItemName" HeaderStyle-Width="300px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"></telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn HeaderText="是/否" HeaderStyle-Width="40px" ItemStyle-Width="40px" UniqueName="yes" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:CheckBoxList ID="cblYes" runat="server">
                                        <asp:ListItem Value="1" Text="" Enabled="false"></asp:ListItem>
                                    </asp:CheckBoxList>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn HeaderText="维护日期" UniqueName="maintenancedate" HeaderStyle-Width="300px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="备注" UniqueName="remark" HeaderStyle-Width="300px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"></telerik:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </telerik:RadPane>
        </telerik:RadSplitter>
    </form>
</body>
</html>
