﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListField1.aspx.cs" Inherits="Com.Sinoyd.Mis.WebUI.CommonExcelImport.ListField1"
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
                        <f:Button ID="btnRefresh" Text="字段初始化" runat="server" Icon="Reload" EnablePostBack="true"
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
                        <f:TemplateField Width="150px" HeaderText="显示名称" TextAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txt_ShowFieldName" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "ShowFieldName").ToString()%>'>
                                </asp:TextBox>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:TemplateField Width="150px" HeaderText="SQL字段名称" TextAlign="Center">
                            <ItemTemplate>
                                <div style="text-align: center;">
                                    <%# DataBinder.Eval(Container.DataItem, "SQLFieldName").ToString()%>
                                </div>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:TemplateField Width="150px" HeaderText="Excel字段名称" TextAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txt_ExcelFieldName" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "ExcelFieldName").ToString()%>'></asp:TextBox>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:TemplateField ExpandUnusedSpace="true" HeaderText="SQL语句" TextAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txt_SourceSql" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "SourceSql").ToString()%>'></asp:TextBox>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:TemplateField Width="75px" HeaderText="排序值" TextAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txt_SortNumber" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "SortNumber").ToString()%>'>
                                </asp:TextBox>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:TemplateField Width="85px" HeaderText="列表显示" TextAlign="Center">
                            <ItemTemplate>
                                <asp:DropDownList Width="100%" runat="server" ID="ddl_ListIsShow">
                                    <asp:ListItem Text="是" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="否" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:TemplateField Width="85px" HeaderText="简单搜索" TextAlign="Center">
                            <ItemTemplate>
                                <asp:DropDownList Width="100%" runat="server" ID="ddl_SearchIsShow">
                                    <asp:ListItem Text="是" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="否" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                        </f:TemplateField>
                        <%--<f:TemplateField Width="150px" HeaderText="数据源(代码)" TextAlign="Center">
                            <ItemTemplate>
                                <asp:DropDownList Width="100%" runat="server" ID="ddl_SourceName">
                                </asp:DropDownList>
                            </ItemTemplate>
                        </f:TemplateField>  --%>
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
