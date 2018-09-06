<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CodeItemList.aspx.cs" Inherits="Com.Sinoyd.Frame.WebUI.SystemMana.Code.CodeItemList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>代码项设置</title>
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
                                            <td>代码项文本：</td>
                                            <td width="150">
                                                <f:TextBox ID="txt_ItemText" Label="代码项名称" runat="server" ShowLabel="false" MaxLength="30"></f:TextBox>
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
                                <f:Button ID="btnNew" Text="新增" runat="server" Icon="Add">
                                </f:Button>
                                <f:Button ID="btnDelete" Text="删除" runat="server" Icon="Delete" OnClick="Delete_Click" ConfirmText="您确定删除选中的代码项吗？"></f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Items>
                        <f:Grid ID="Grid1" Title="Grid1" ShowBorder="false" ShowHeader="false"
                            PageSize="10" runat="server" EnableCheckBoxSelect="true" DataKeyNames="RowGuid"
                            AllowPaging="true" EnableHeaderMenu="false" OnPageIndexChange="Grid1_PageIndexChange" IsDatabasePaging="true">
                            <Columns>
                                <f:TemplateField HeaderText="代码项文本" Width="150" TextAlign="Center">
                                    <ItemTemplate>
                                        <div style="text-align: left;">
                                            <%# DataBinder.Eval(Container.DataItem, "ItemText").ToString()%>
                                        </div>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:TemplateField HeaderText="代码项值" Width="150" TextAlign="Center">
                                    <ItemTemplate>
                                        <div style="text-align: left;">
                                            <%# DataBinder.Eval(Container.DataItem, "ItemValue").ToString()%>
                                        </div>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:TemplateField HeaderText="排序号" Width="80" TextAlign="Center">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "SortNumber") %>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:TemplateField HeaderText="操作" Width="80" TextAlign="Center">
                                    <ItemTemplate>
                                        <a href="<%# GetEditUrl(DataBinder.Eval(Container.DataItem, "[RowGuid]")) %>">
                                            <img src="../../Content/icon/page_edit.png" /></a>
                                    </ItemTemplate>
                                </f:TemplateField>
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Panel>
            </Items>
        </f:Panel>

        <f:Window ID="Window1" Title="Edit" EnableIFrame="true" runat="server"
            EnableConfirmOnClose="true" IFrameUrl="about:blank" Target="Self" IsModal="true"
            Width="300px" Height="200px" Hidden="true" OnClose="Window1_Close" EnableDrag="false">
        </f:Window>
    </form>
</body>
</html>
