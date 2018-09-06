<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserFrame.aspx.cs" Inherits="Com.Sinoyd.Frame.WebUI.SystemMana.User.UserFrame" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>用户管理</title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1"
            runat="server"></f:PageManager>
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region2" Width="250px"
                    Margins="3 3 3 3" ShowHeader="false" Title="用户" Icon="Outline" EnableCollapse="true"
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
                            ShowHeader="false" Width="900px" Height="300px" Layout="Anchor" Title="用户管理">
                            <Items>
                                <f:Form ID="Form2" ShowBorder="False" BodyPadding="5px"
                                    ShowHeader="False" runat="server">
                                    <Rows>
                                        <f:FormRow>
                                            <Items>
                                                <f:ContentPanel runat="server" ID="cp1" ShowBorder="false" ShowHeader="false">
                                                    <table>
                                                        <tr>
                                                            <td>姓名：</td>
                                                            <td width="100">
                                                                <f:TextBox ID="txt_DisplayName" runat="server" Width="100" MaxLength="30"></f:TextBox>
                                                            </td>
                                                            <td>启用：</td>
                                                            <td width="100">
                                                                <f:DropDownList runat="server" ID="ddl_IsEnabled" Width="100">
                                                                    <f:ListItem Text="所有选项" Value="2" />
                                                                    <f:ListItem Text="是" Value="1" Selected="true" />
                                                                    <f:ListItem Text="否" Value="0" />
                                                                </f:DropDownList>
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
                                        <f:Toolbar ID="Toolbar1" runat="server" Hidden="true">
                                            <Items>
                                                <f:Button ID="btnNew" Text="新增" runat="server" Icon="Add">
                                                </f:Button>
                                                <f:Button ID="btnDelete" Text="禁用" runat="server" Icon="BulletCross" OnClick="Delete_Click" ConfirmText="您确定要对选中的用户进行删除？">
                                                </f:Button>
                                                <f:Button ID="btnInit" Text="初始化密码" runat="server" Icon="Key" OnClick="btnInit_Click" ConfirmText="您确定初始化选中用户的密码吗？">
                                                </f:Button>
                                            </Items>
                                        </f:Toolbar>
                                    </Toolbars>
                                    <Items>
                                        <f:Grid ID="Grid1" Title="Grid1" ShowBorder="true" ShowHeader="false"
                                            PageSize="20" runat="server" EnableCheckBoxSelect="true" DataKeyNames="RowGuid"
                                            AllowPaging="true" EnableHeaderMenu="false" EnableTextSelection="true" OnPageIndexChange="Grid1_PageIndexChange" IsDatabasePaging="true">
                                            <Columns>
                                                <f:RowNumberField Width="30" TextAlign="Center" />
                                                <f:TemplateField HeaderText="部门" TextAlign="Center" ExpandUnusedSpace="true">
                                                    <ItemTemplate>
                                                        <div style="text-align: left;">
                                                            <%# GetDeptName(DataBinder.Eval(Container.DataItem, "DeptGuid").ToString()) %>
                                                        </div>
                                                    </ItemTemplate>
                                                </f:TemplateField>
                                                <f:TemplateField HeaderText="登录名" Width="100" TextAlign="Center">
                                                    <ItemTemplate>
                                                        <%# DataBinder.Eval(Container.DataItem, "LoginID") %>
                                                    </ItemTemplate>
                                                </f:TemplateField>
                                                <f:TemplateField HeaderText="姓名" Width="100" TextAlign="Center">
                                                    <ItemTemplate>
                                                        <%# DataBinder.Eval(Container.DataItem, "DisplayName").ToString()%>
                                                    </ItemTemplate>
                                                </f:TemplateField>
                                                <f:CheckBoxField DataField="IsEnabled" HeaderText="启用" RenderAsStaticField="true"
                                                    Width="60px" TextAlign="Center" />
                                                <f:TemplateField HeaderText="排序号" Width="70" TextAlign="Center">
                                                    <ItemTemplate>
                                                        <%# DataBinder.Eval(Container.DataItem, "SortNumber") %>
                                                    </ItemTemplate>
                                                </f:TemplateField>
                                                <f:TemplateField HeaderText="菜单设置" Width="80" TextAlign="Center">
                                                    <ItemTemplate>
                                                        <a href="<%# GetEditModuleUrl(DataBinder.Eval(Container.DataItem, "[RowGuid]")) %>">
                                                            <img src="../../Content/icon/Application.png" /></a>
                                                    </ItemTemplate>
                                                </f:TemplateField>
                                                <f:TemplateField HeaderText="操作" Width="50" TextAlign="Center">
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
        <f:Window ID="Window1" Width="650px" Height="400px" Title="窗体" Hidden="true"
            EnableMaximize="false" EnableCollapse="false" runat="server" EnableResize="true"
            IsModal="true" Layout="Fit" EnableDrag="false" EnableIFrame="true" OnClose="Window1_Close">
        </f:Window>
        <f:Window ID="Window2" Width="380px" Height="560px" Title="窗体" Hidden="true"
            EnableMaximize="false" EnableCollapse="false" runat="server" EnableResize="true"
            IsModal="true" Layout="Fit" EnableDrag="false" EnableIFrame="true" Target="Top">
        </f:Window>
    </form>
</body>
</html>
