<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListField.aspx.cs" Inherits="Com.Sinoyd.Mis.WebUI.Chart.ListField" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Mis表字段管理</title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel2" runat="server"></f:PageManager>
    <f:Panel ID="Panel2" ShowBorder="false" ShowHeader="false" runat="server" AnchorValue="100% -1" Layout="Fit" AutoScroll="true">
        <Items>
            <f:Panel runat="server" ID="set1" ShowBorder="false" ShowHeader="false" Title="基本设置" EnableCollapse="true">
                <Toolbars>
                    <f:Toolbar runat="server" ID="Toolbar2">
                        <Items>
                            <f:Button ID="btnNew" Text="添加" runat="server" Icon="Add" OnClick="btnAdd_Click">
                            </f:Button>
                            <f:Button ID="btnSave1" Text="保存" runat="server" Icon="PageSave" EnablePostBack="true" OnClick="btnSave1_Click">
                            </f:Button>
                            <f:Button ID="btnDel" Text="删除" runat="server" Icon="Cancel" EnablePostBack="true" OnClick="btnDel_OnClick" ConfirmText="您确定删除选中的字段吗？">
                            </f:Button>
                        </Items>
                    </f:Toolbar>
                </Toolbars>
                <Items>
                    <f:Grid ID="Grid1" Title="基本设置" PageSize="200" ShowBorder="false" ShowHeader="false" AllowPaging="false" runat="server" EnableCheckBoxSelect="true" DataKeyNames="RowGuid" IsDatabasePaging="true" OnPageIndexChange="Grid1_PageIndexChange" OnRowDataBound="Grid1_RowDataBound" Height="430">
                        <Columns><f:RowNumberField></f:RowNumberField>
                            <f:TemplateField Width="115px" HeaderText="<div style='text-align:center;'>显示名称&nbsp;</div>">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txt_ShowFieldName" Width="105" Text='<%# DataBinder.Eval(Container.DataItem, "ShowFieldName").ToString()%>'>
                                    </asp:TextBox>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="115px" HeaderText="<div style='text-align:center;'>SQL字段名称&nbsp;</div>">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txt_SQLFieldName" Width="105" Text='<%# DataBinder.Eval(Container.DataItem, "SQLFieldName").ToString()%>'>
                                    </asp:TextBox>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="115px" HeaderText="<div style='text-align:center;'>显示类型&nbsp;</div>">
                                <ItemTemplate>
                                    <asp:DropDownList Width="105" runat="server" ID="ddl_FieldType">
                                        <asp:ListItem Text="文本框" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="文本框(多行)" Value="9"></asp:ListItem>
                                        <asp:ListItem Text="下拉框" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="数字框" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="单选框" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="复选框" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="日期框" Value="6"></asp:ListItem>
                                        <asp:ListItem Text="时间(分)" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="时间(秒)" Value="8"></asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="115px" HeaderText="<div style='text-align:center;color:red;'>显示宽度&nbsp;</div>">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txt_ShowWidth" Width="105" Text='<%# DataBinder.Eval(Container.DataItem, "ShowWidth").ToString()%>'>
                                    </asp:TextBox>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="75px" HeaderText="<div style='text-align:center;'>是否必填&nbsp;</div>">
                                <ItemTemplate>
                                    <asp:DropDownList Width="65" runat="server" ID="ddl_Required">
                                        <asp:ListItem Text="是" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="否" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="75px" HeaderText="<div style='text-align:center;'>小数位数&nbsp;</div>">
                                <ItemTemplate>
                                    <asp:DropDownList Width="65" runat="server" ID="ddl_DecimalPrecision">
                                        <asp:ListItem Text="" Value=""></asp:ListItem>
                                        <asp:ListItem Text="0" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </f:TemplateField>
                            <%--                            <f:TemplateField Width="115px" HeaderText="<div style='text-align:center;'>数据源&nbsp;</div>">
                                <ItemTemplate>
                                    <asp:DropDownList Width="105" runat="server" ID="ddl_SourceName">
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="85px" HeaderText="<div style='text-align:center;'>数据源Sql&nbsp;</div>">
                                <ItemTemplate>
                                    <f:TextBox runat="server" ID="txt_SourceSql" Width="75" Text='<%# DataBinder.Eval(Container.DataItem, "SourceSql").ToString()%>'>
                                    
                                
                            --%>
                            <f:TemplateField Width="75px" HeaderText="<div style='text-align:center;'>排序值&nbsp;</div>">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txt_SortNumber" Width="65" Text='<%# DataBinder.Eval(Container.DataItem, "SortNumber").ToString()%>'>
                                    </asp:TextBox>
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
