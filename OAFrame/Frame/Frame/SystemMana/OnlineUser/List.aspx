<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Com.Sinoyd.Frame.WebUI.SystemMana.OnlineUser.List" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel2"
            runat="server"></f:PageManager>

        <f:Panel ID="Panel2" runat="server" ShowBorder="true"
            ShowHeader="false" Width="900px" Height="300px" Layout="Anchor" Title="代码管理">
            <Items>
                <f:Form ID="Form2" ShowBorder="False" BodyPadding="5px"
                    ShowHeader="False" runat="server">
                    <Rows>
                        <f:FormRow>
                            <Items>
                                <f:ContentPanel runat="server" ID="cp1" ShowBorder="false" ShowHeader="false">
                                    <table>
                                        <tr>
                                            <td>用户姓名：</td>
                                            <td width="150">
                                                <f:TextBox ID="txt_DisplayName" Label="用户姓名" runat="server" ShowLabel="false"  MaxLength="20"></f:TextBox>
                                            </td>
                                            <td>
                                                <f:Button ID="Button6" Text="查找" runat="server" OnClick="Search_Click" Icon="Magnifier"></f:Button>
                                            </td>
                                            <td style="display: none;">
                                                <f:TextBox runat="server" ID="txt_RowGuid" Width="100"></f:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </f:ContentPanel>
                            </Items>
                        </f:FormRow>
                    </Rows>
                </f:Form>
                <f:Panel ID="Panel1" ShowBorder="false" ShowHeader="false" runat="server" AnchorValue="100% -36"
                    Layout="Fit">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <f:Button ID="btnDelete" Text="踢出" runat="server" Icon="Delete" ConfirmText="您确定踢出选中的用户吗？" OnClick="btnDelete_Click"></f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Items>
                        <f:Grid ID="Grid1" Title="Grid1" ShowBorder="false" ShowHeader="false"
                            PageSize="10000" runat="server" EnableCheckBoxSelect="true" DataKeyNames="UserGuid"
                            AllowPaging="true" EnableHeaderMenu="false" OnPageIndexChange="Grid1_PageIndexChange" IsDatabasePaging="true">
                            <Columns>
                                <f:TemplateField HeaderText="所属部门" Width="250" TextAlign="Center">
                                    <ItemTemplate>
                                        <div style="text-align: left;">
                                            <%# DataBinder.Eval(Container.DataItem, "DeptName").ToString()%>
                                        </div>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:TemplateField HeaderText="用户姓名" Width="150" TextAlign="Center">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "DisplayName").ToString()%>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:TemplateField HeaderText="登录IP地址" Width="150" TextAlign="Center">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "LastLoginIP").ToString()%>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:TemplateField HeaderText="最后刷新时间" Width="150" TextAlign="Center">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "LastUpdateTime").ToString()%>
                                    </ItemTemplate>
                                </f:TemplateField>
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Panel>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
