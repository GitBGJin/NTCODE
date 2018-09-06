<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoleFrame.aspx.cs" Inherits="Com.Sinoyd.Frame.WebUI.SystemMana.Role.RoleFrame" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>角色管理</title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1"
            runat="server"></f:PageManager>
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region2" Width="200px"
                    Margins="3 3 3 3" ShowHeader="false" Title="单位" Icon="Outline" EnableCollapse="true"
                    ShowBorder="false" Layout="Fit" Position="Left" runat="server">
                    <Items>
                        <f:Tree runat="server" EnableArrows="true" ShowBorder="true" BodyPadding="5px"
                            ShowHeader="false" AutoScroll="true" ID="treeOU" OnNodeCommand="Tree1_NodeCommand">
                        </f:Tree>
                    </Items>
                </f:Region>
                <f:Region ID="mainRegion" ShowHeader="false" Layout="Fit" Margins="3 3 3 3" Position="Center"
                    ShowBorder="false" runat="server">
                    <Items>
                        <f:Panel ID="Panel2" runat="server" ShowBorder="true"
                            ShowHeader="false" Width="900px" Height="300px" Layout="Anchor" Title="角色管理">
                            <Items>
                                <f:Form ID="Form2" ShowBorder="False" BodyPadding="5px"
                                    ShowHeader="False" runat="server">
                                    <Rows>
                                        <f:FormRow>
                                            <Items>
                                                <f:ContentPanel runat="server" ID="cp1" ShowBorder="false" ShowHeader="false">
                                                    <table>
                                                        <tr>
                                                            <td>角色名称：</td>
                                                            <td width="150">
                                                                <f:TextBox ID="txt_OrgName" Label="角色名称" runat="server" ShowLabel="false" MaxLength="30"></f:TextBox>
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
                                                <f:Button ID="btnDelete" Text="删除" runat="server" Icon="Delete" OnClick="Delete_Click" ConfirmText="您确定删除选定的角色吗？"></f:Button>
                                            </Items>
                                        </f:Toolbar>
                                    </Toolbars>
                                    <Items>
                                        <f:Grid ID="Grid1" Title="Grid1" ShowBorder="false" ShowHeader="false"
                                            PageSize="200" runat="server" EnableCheckBoxSelect="true" DataKeyNames="RowGuid"
                                            AllowPaging="false" EnableHeaderMenu="false">
                                            <Columns>
                                                <f:TemplateField HeaderText="角色名称" Width="150" TextAlign="Center">
                                                    <ItemTemplate>
                                                        <div style="text-align: left;">
                                                            <%# DataBinder.Eval(Container.DataItem, "RoleName").ToString()%>
                                                        </div>
                                                    </ItemTemplate>
                                                </f:TemplateField>
                                                <f:TemplateField HeaderText="排序号" Width="80" TextAlign="Center">
                                                    <ItemTemplate>
                                                        <%# DataBinder.Eval(Container.DataItem, "SortNumber") %>
                                                    </ItemTemplate>
                                                </f:TemplateField>
                                                <f:TemplateField HeaderText="备注" TextAlign="Center" ExpandUnusedSpace="true">
                                                    <ItemTemplate>
                                                        <div style="text-align: left;">
                                                            <%# DataBinder.Eval(Container.DataItem, "Note").ToString()%>
                                                        </div>
                                                    </ItemTemplate>
                                                </f:TemplateField>
                                                <f:TemplateField HeaderText="菜单设置" Width="80" TextAlign="Center">
                                                    <ItemTemplate>
                                                        <a href="<%# GetEditModuleUrl(DataBinder.Eval(Container.DataItem, "[RowGuid]")) %>">
                                                            <img src="../../Content/icon/Application.png" /></a>
                                                    </ItemTemplate>
                                                </f:TemplateField>
                                                <f:TemplateField HeaderText="人员设置" Width="80" TextAlign="Center">
                                                    <ItemTemplate>
                                                        <a href="<%# GetEditUserUrl(DataBinder.Eval(Container.DataItem, "[RowGuid]")) %>">
                                                            <img src="../../Content/icon/user_go.png" /></a>
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
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>
        <f:Window ID="Window1" Width="550px" Height="400px" Title="窗体" Hidden="true"
            EnableMaximize="false" EnableCollapse="false" runat="server" EnableResize="true"
            IsModal="false" Layout="Fit" EnableDrag="false" EnableIFrame="true" OnClose="Window1_Close" Target="Self">
        </f:Window>
        <f:Window ID="Window2" Width="380px" Height="560px" Title="窗体" Hidden="true"
            EnableMaximize="false" EnableCollapse="false" runat="server" EnableResize="true"
            IsModal="true" Layout="Fit" EnableDrag="false" EnableIFrame="true" Target="Top">
        </f:Window>
    </form>
</body>
</html>
