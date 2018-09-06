<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListField1.aspx.cs" Inherits="Com.Sinoyd.Mis.WebUI.TableInfo.ListField1"
    EnableEventValidation="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Mis表字段管理(基本信息)</title>
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
                        <f:TemplateField Width="120px" HeaderText="显示名称" TextAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txt_ShowFieldName" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "ShowFieldName").ToString()%>'>
                                </asp:TextBox>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:TemplateField Width="105px" HeaderText="SQL字段名称" TextAlign="Center">
                            <ItemTemplate>
                                <div style="text-align: center; padding-top: 2px;">
                                    <%# DataBinder.Eval(Container.DataItem, "SQLFieldName").ToString()%>
                                </div>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:TemplateField Width="100px" HeaderText="显示类型" TextAlign="Center">
                            <ItemTemplate>
                                <asp:DropDownList Width="100%" runat="server" ID="ddl_FieldType">
                                    <asp:ListItem Text="文本框" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="文本框(多行)" Value="9"></asp:ListItem>
                                    <asp:ListItem Text="下拉框" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="数字框" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="单选框" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="复选框" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="日期框" Value="6"></asp:ListItem>
                                    <asp:ListItem Text="时间(分)" Value="7"></asp:ListItem>
                                    <asp:ListItem Text="时间(秒)" Value="8"></asp:ListItem>
                                    <asp:ListItem Text="弹出框(字典)" Value="10"></asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:TemplateField Width="80px" HeaderText="小数位数" TextAlign="Center">
                            <ItemTemplate>
                                <asp:DropDownList Width="100%" runat="server" ID="ddl_DecimalPrecision">
                                    <asp:ListItem Text="" Value=""></asp:ListItem>
                                    <asp:ListItem Text="0" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:TemplateField Width="80px" HeaderText="当行列数" TextAlign="Center">
                            <ItemTemplate>
                                <asp:DropDownList Width="100%" runat="server" ID="ddl_RepeatColumns">
                                    <asp:ListItem Text="" Value="100"></asp:ListItem>
                                    <asp:ListItem Text="0" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                    <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                    <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                    <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:TemplateField Width="160px" HeaderText="数据源(代码)" TextAlign="Center">
                            <ItemTemplate>
                                <asp:DropDownList Width="100%" runat="server" ID="ddl_SourceName">
                                </asp:DropDownList>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:TemplateField Width="160px" HeaderText="数据源(字典)" TextAlign="Center" Hidden="true">
                            <ItemTemplate>
                                <asp:DropDownList Width="100%" runat="server" ID="ddl_CodeDictionaryName">
                                </asp:DropDownList>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:TemplateField Width="80px" HeaderText="选择" TextAlign="Center" Hidden="true">
                            <ItemTemplate>
                                <asp:DropDownList Width="100%" runat="server" ID="ddl_Select">
                                    <asp:ListItem Text="" Value=""></asp:ListItem>
                                    <asp:ListItem Text="单选" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="多选" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:TemplateField Width="80px" HeaderText="刷新字段" TextAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txt_RefreshFieldName" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "RefreshFieldName").ToString()%>'>
                                </asp:TextBox>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:TemplateField Width="160px" HeaderText="数据源Sql" TextAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txt_SourceSql" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "SourceSql").ToString()%>'>
                                </asp:TextBox>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:TemplateField Width="75px" HeaderText="排序值" TextAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txt_SortNumber" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "SortNumber").ToString()%>'>
                                </asp:TextBox>
                            </ItemTemplate>
                        </f:TemplateField>
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
