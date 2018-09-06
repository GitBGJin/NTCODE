<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListField.aspx.cs" Inherits="Com.Sinoyd.Mis.WebUI.TableInfo.ListField" EnableEventValidation="true"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Mis表字段管理</title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel2" runat="server"></f:PageManager>
    <f:Panel ID="Panel2" ShowBorder="false" ShowHeader="false" runat="server" AnchorValue="100% -1"
        Layout="Fit" AutoScroll="true">
        <Items>
            <f:Panel runat="server" ID="set1" ShowBorder="false" ShowHeader="false" Title="基本设置"
                EnableCollapse="true" AutoScroll="true">
                <Toolbars>
                    <f:Toolbar runat="server" ID="Toolbar2">
                        <Items>
                            <f:Button ID="btnSave1" Text="保存" runat="server" Icon="PageSave" EnablePostBack="true"
                                OnClick="btnSave1_Click">
                            </f:Button>
                            <f:Button ID="btnDel" Text="删除" runat="server" Icon="Cancel" EnablePostBack="true"
                                OnClick="btnDel_OnClick" ConfirmText="您确定删除选中的字段吗？">
                            </f:Button>
                            <f:Button ID="Button1" Text="基本信息" runat="server" Icon="PageRefresh" EnablePostBack="true"
                                OnClick="Button1_Click">
                            </f:Button>
                            <f:Button ID="Button2" Text="列表、搜索" runat="server" Icon="PageRefresh" EnablePostBack="true"
                                OnClick="Button2_Click">
                            </f:Button>
                            <f:Button ID="Button3" Text="表单" runat="server" Icon="PageRefresh" EnablePostBack="true"
                                OnClick="Button3_Click">
                            </f:Button>
                            <f:Button ID="Button4" Text="全部显示" runat="server" Icon="PageRefresh" EnablePostBack="true"
                                OnClick="Button4_Click">
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
                            <f:TemplateField Width="85px" HeaderText="<div style='text-align:center;'>显示名称&nbsp;</div>">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txt_ShowFieldName" Width="75" Text='<%# DataBinder.Eval(Container.DataItem, "ShowFieldName").ToString()%>'>
                                    </asp:TextBox>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="115px" HeaderText="<div style='text-align:center;'>SQL字段名称&nbsp;</div>">
                                <ItemTemplate>
                                    <div style="text-align: center; padding-top: 2px;">
                                        <%# DataBinder.Eval(Container.DataItem, "SQLFieldName").ToString()%></div>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="85px" HeaderText="<div style='text-align:center;'>显示类型&nbsp;</div>">
                                <ItemTemplate>
                                    <asp:DropDownList Width="75" runat="server" ID="ddl_FieldType">
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
                            <f:TemplateField Width="55px" HeaderText="<div style='text-align:center;'>小数位数&nbsp;</div>">
                                <ItemTemplate>
                                    <asp:DropDownList Width="45" runat="server" ID="ddl_DecimalPrecision">
                                        <asp:ListItem Text="" Value=""></asp:ListItem>
                                        <asp:ListItem Text="0" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="115px" HeaderText="<div style='text-align:center;'>数据源&nbsp;</div>">
                                <ItemTemplate>
                                    <asp:DropDownList Width="105" runat="server" ID="ddl_SourceName">
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="85px" HeaderText="<div style='text-align:center;'>数据源Sql&nbsp;</div>">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txt_SourceSql" Width="75" Text='<%# DataBinder.Eval(Container.DataItem, "SourceSql").ToString()%>'>
                                    </asp:TextBox>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="75px" HeaderText="<div style='text-align:center;'>排序值&nbsp;</div>">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txt_SortNumber" Width="65" Text='<%# DataBinder.Eval(Container.DataItem, "SortNumber").ToString()%>'>
                                    </asp:TextBox>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="75px" HeaderText="<div style='text-align:center;color:red;'>列表显示&nbsp;</div>">
                                <ItemTemplate>
                                    <asp:DropDownList Width="65" runat="server" ID="ddl_ListIsShow">
                                        <asp:ListItem Text="是" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="否" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="75px" HeaderText="<div style='text-align:center;color:red;'>显示位置&nbsp;</div>">
                                <ItemTemplate>
                                    <asp:DropDownList Width="65" runat="server" ID="ddl_ListPosition">
                                        <asp:ListItem Text="左" Value="l"></asp:ListItem>
                                        <asp:ListItem Text="中" Value="m" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="右" Value="r"></asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="75px" HeaderText="<div style='text-align:center;color:red;'>显示宽度&nbsp;</div>">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txt_ListGridWidth" Width="65" Text='<%# DataBinder.Eval(Container.DataItem, "ListGridWidth").ToString()%>'>
                                    </asp:TextBox>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="75px" HeaderText="<div style='text-align:center;color:red;'>启用排序&nbsp;</div>">
                                <ItemTemplate>
                                    <asp:DropDownList Width="65" runat="server" ID="ddl_ListIsSort">
                                        <asp:ListItem Text="是" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="否" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="75px" HeaderText="<div style='text-align:center;color:red;'>简单搜索&nbsp;</div>">
                                <ItemTemplate>
                                    <asp:DropDownList Width="65" runat="server" ID="ddl_SearchIsShow">
                                        <asp:ListItem Text="是" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="否" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="75px" HeaderText="<div style='text-align:center;color:red;'>高级搜索&nbsp;</div>">
                                <ItemTemplate>
                                    <asp:DropDownList Width="65" runat="server" ID="ddl_SearchIsShowAdv">
                                        <asp:ListItem Text="是" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="否" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="85px" HeaderText="<div style='text-align:center;color:red;'>搜索类型&nbsp;</div>">
                                <ItemTemplate>
                                    <asp:DropDownList Width="75" runat="server" ID="ddl_SearchType">
                                        <asp:ListItem Text="" Value=""></asp:ListItem>
                                        <asp:ListItem Text="模糊查找" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="精确查找" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="75px" HeaderText="<div style='text-align:center;color:red;'>搜索框宽度&nbsp;</div>">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txt_SearchWidth" Width="65"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "SearchWidth").ToString()%>'>
                                    </asp:TextBox>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="75px" HeaderText="<div style='text-align:center;'>表单显示&nbsp;</div>">
                                <ItemTemplate>
                                    <asp:DropDownList Width="65" runat="server" ID="ddl_FormIsShow">
                                        <asp:ListItem Text="是" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="否" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
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
                            <f:TemplateField Width="75px" HeaderText="<div style='text-align:center;'>表单宽度&nbsp;</div>">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txt_FormItemWidth" Width="65" DecimalPrecision="0"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "FormItemWidth").ToString()%>'>
                                    </asp:TextBox>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="75px" HeaderText="<div style='text-align:center;'>是否新行&nbsp;</div>">
                                <ItemTemplate>
                                    <asp:DropDownList Width="65" runat="server" ID="ddl_FormIsNewTR">
                                        <asp:ListItem Text="是" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="否" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="95px" HeaderText="<div style='text-align:center;'>默认值&nbsp;</div>">
                                <ItemTemplate>
                                    <asp:DropDownList Width="85" runat="server" ID="ddl_DefaultValue">
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
                            <f:TemplateField Width="95px" HeaderText="<div style='text-align:center;'>表单验证&nbsp;</div>">
                                <ItemTemplate>
                                    <asp:DropDownList Width="85" runat="server" ID="ddl_ValidateType">
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
        </Items>
    </f:Panel>
    </form>
</body>
</html>
