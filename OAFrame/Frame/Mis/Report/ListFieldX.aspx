<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListFieldX.aspx.cs" Inherits="Com.Sinoyd.Mis.WebUI.Report.ListFieldX" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>报表字段管理</title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel2" runat="server"></f:PageManager>
    <f:Panel ID="Panel2" ShowBorder="false" ShowHeader="false" runat="server" AnchorValue="100% -36"
        Layout="Fit" AutoScroll="true">
        <Items>
            <f:Panel runat="server" ID="set1" ShowBorder="false" ShowHeader="false" Title="基本设置"
                EnableCollapse="true">
                <Toolbars>
                    <f:Toolbar runat="server" ID="Toolbar2">
                        <Items>
                            <f:Button ID="btnNew" Text="添加" runat="server" Icon="Add" OnClick="btnAdd_Click">
                            </f:Button>
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
                    <f:Grid ID="Grid1" Title="基本设置" PageSize="100" ShowBorder="false" ShowHeader="false"
                        AllowPaging="false" runat="server" EnableCheckBoxSelect="true" DataKeyNames="RowGuid"
                        IsDatabasePaging="true" OnPageIndexChange="Grid1_PageIndexChange" OnRowDataBound="Grid1_RowDataBound"
                        Height="420" EnableHeaderMenu="false" AutoScroll="true">
                        <Columns>
                            <f:RowNumberField></f:RowNumberField>
                            <f:TemplateField Width="75px" HeaderText="编码" TextAlign="Center">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txt_XCode" Width="60" Text='<%# DataBinder.Eval(Container.DataItem, "XCode").ToString()%>'>
                                    </asp:TextBox>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="115px" HeaderText="显示名称" TextAlign="Center">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txt_ShowFieldName" Width="100" Text='<%# DataBinder.Eval(Container.DataItem, "ShowFieldName").ToString()%>'>
                                    </asp:TextBox>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="115px" HeaderText="数据源字段" TextAlign="Center">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txt_SQLFieldName" Width="100" Text='<%# DataBinder.Eval(Container.DataItem, "SQLFieldName").ToString()%>'>
                                    </asp:TextBox>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField ExpandUnusedSpace="true" HeaderText="数据源Sql" TextAlign="Center">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txt_SourceSql" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "SourceSql").ToString()%>'>
                                    </asp:TextBox>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="75px" HeaderText="显示位置" TextAlign="Center">
                                <ItemTemplate>
                                    <asp:DropDownList Width="60" runat="server" ID="ddl_FieldType">
                                        <asp:ListItem Text="居左" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="居中" Value="2" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="居右" Value="3"></asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="105px" HeaderText="扩展方式" TextAlign="Center">
                                <ItemTemplate>
                                    <asp:DropDownList Width="90" runat="server" ID="ddl_HaveChild">
                                        <asp:ListItem Text="不扩展" Value="-1" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="子节点扩展" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="横向扩展" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="150" HeaderText="扩展数据源Sql" TextAlign="Center">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txt_ChildSql" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "ChildSql").ToString()%>'>
                                    </asp:TextBox>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="75px" HeaderText="显示宽度" TextAlign="Center">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txt_ShowWidth" Width="60" Text='<%# DataBinder.Eval(Container.DataItem, "ShowWidth").ToString()%>'>
                                    </asp:TextBox>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="75px" HeaderText="合计列" TextAlign="Center">
                                <ItemTemplate>
                                    <asp:DropDownList Width="60" runat="server" ID="ddl_IsSummary">
                                        <asp:ListItem Text="是" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="否" Value="0" Selected="True"></asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="120px" HeaderText="外调方法" TextAlign="Center">
                                <ItemTemplate>
                                    <asp:DropDownList Width="100" runat="server" ID="ddl_MethodGuid">
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </f:TemplateField>
                            <%--<f:TemplateField Width="75px" HeaderText="排序值" TextAlign="Center">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txt_SortNumber" Width="60" Text='<%# DataBinder.Eval(Container.DataItem, "SortNumber").ToString()%>'>
                                    </asp:TextBox>
                                </ItemTemplate>
                            </f:TemplateField>--%>
                        </Columns>
                    </f:Grid>
                </Items>
            </f:Panel>
        </Items>
    </f:Panel>
    </form>
</body>
</html>
