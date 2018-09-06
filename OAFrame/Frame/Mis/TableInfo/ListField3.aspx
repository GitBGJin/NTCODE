<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListField3.aspx.cs" Inherits="Com.Sinoyd.Mis.WebUI.TableInfo.ListField3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Mis表字段管理(表单信息)</title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel2" runat="server"></f:PageManager>
    <f:Panel ID="Panel2" ShowBorder="false" ShowHeader="false" runat="server" AnchorValue="100% -1"
        Layout="Fit" AutoScroll="true">
        <Toolbars>
            <f:Toolbar runat="server" ID="Toolbar2">
                <Items>
                    <f:Button ID="btnSave1" Text="保存" runat="server" Icon="PageSave" EnablePostBack="true"
                        OnClick="btnSave1_Click">
                    </f:Button>
                    <f:Button ID="btnDel" Text="删除" runat="server" Icon="Cancel" EnablePostBack="true"
                        OnClick="btnDel_OnClick" ConfirmText="您确定删除选中的字段吗？">
                    </f:Button>
                    <f:Button ID="btnRefresh" Text="刷新" runat="server" Icon="Reload" EnablePostBack="true"
                        OnClick="btnrefresh_Click">
                    </f:Button>
                </Items>
            </f:Toolbar>
        </Toolbars>
        <Items>
            <f:Grid ID="Grid1" Title="基本设置" PageSize="200" ShowBorder="false" ShowHeader="false"
                AllowPaging="false" runat="server" EnableCheckBoxSelect="true" DataKeyNames="RowGuid"
                IsDatabasePaging="true" OnPageIndexChange="Grid1_PageIndexChange" OnRowDataBound="Grid1_RowDataBound"
                Height="430" AutoScroll="true" EnableHeaderMenu="false">
                <Columns>
                    <f:RowNumberField></f:RowNumberField>
                    <f:TemplateField Width="115px" HeaderText="显示名称" TextAlign="Center">
                        <ItemTemplate>
                            <asp:TextBox runat="server" ID="txt_ShowFieldName" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "ShowFieldName").ToString()%>'>
                            </asp:TextBox>
                        </ItemTemplate>
                    </f:TemplateField>
                    <f:TemplateField Width="115px" HeaderText="SQL字段名称" TextAlign="Center">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container.DataItem, "SQLFieldName").ToString()%>
                        </ItemTemplate>
                    </f:TemplateField>
                    <f:TemplateField Width="85px" HeaderText="表单显示" TextAlign="Center">
                        <ItemTemplate>
                            <asp:DropDownList Width="100%" runat="server" ID="ddl_FormIsShow">
                                <asp:ListItem Text="是" Value="1"></asp:ListItem>
                                <asp:ListItem Text="否" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>
                    </f:TemplateField>
                    <f:TemplateField Width="85px" HeaderText="是否必填" TextAlign="Center">
                        <ItemTemplate>
                            <asp:DropDownList Width="100%" runat="server" ID="ddl_Required">
                                <asp:ListItem Text="是" Value="1"></asp:ListItem>
                                <asp:ListItem Text="否" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>
                    </f:TemplateField>
                    <f:TemplateField Width="85px" HeaderText="表单宽度" TextAlign="Center">
                        <ItemTemplate>
                            <asp:TextBox runat="server" ID="txt_FormItemWidth" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "FormItemWidth").ToString()%>'>
                            </asp:TextBox>
                        </ItemTemplate>
                    </f:TemplateField>
                    <f:TemplateField Width="85px" HeaderText="下行新行" TextAlign="Center">
                        <ItemTemplate>
                            <asp:DropDownList Width="100%" runat="server" ID="ddl_FormIsNewTR">
                                <asp:ListItem Text="是" Value="1"></asp:ListItem>
                                <asp:ListItem Text="否" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>
                    </f:TemplateField>
                    <f:TemplateField Width="115px" HeaderText="默认值" TextAlign="Center">
                        <ItemTemplate>
                            <asp:DropDownList Width="100%" runat="server" ID="ddl_DefaultValue">
                                <asp:ListItem Text="" Value=""></asp:ListItem>
                                <asp:ListItem Text="当前用户姓名" Value="1"></asp:ListItem>
                                <asp:ListItem Text="当前用户标识" Value="2"></asp:ListItem>
                                <asp:ListItem Text="当前部门名称" Value="3"></asp:ListItem>
                                <asp:ListItem Text="当前部门标识" Value="4"></asp:ListItem>
                                <asp:ListItem Text="当前日期" Value="5"></asp:ListItem>
                                <asp:ListItem Text="当前时间" Value="6"></asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>
                    </f:TemplateField>
                    <f:TemplateField Width="115px" HeaderText="表单验证" TextAlign="Center">
                        <ItemTemplate>
                            <asp:DropDownList Width="100%" runat="server" ID="ddl_ValidateType">
                                <asp:ListItem Text="" Value=""></asp:ListItem>
                                <asp:ListItem Text="Email" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Url" Value="2"></asp:ListItem>
                                <asp:ListItem Text="手机号码" Value="3"></asp:ListItem>
                                <asp:ListItem Text="电话号码" Value="4"></asp:ListItem>
                                <asp:ListItem Text="身份证号码" Value="5"></asp:ListItem>
                                <asp:ListItem Text="邮编" Value="6"></asp:ListItem>
                                <asp:ListItem Text="IP地址" Value="7"></asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>
                    </f:TemplateField>
                </Columns>
            </f:Grid>
        </Items>
    </f:Panel>
    </form>
</body>
</html>
