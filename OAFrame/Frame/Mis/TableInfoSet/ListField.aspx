<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListField.aspx.cs" Inherits="TK.Mis.Web.TableInfoSet.ListField" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>字段管理</title>
    <style>
        .sortnumber
        {
            border-right: #B5B8C8 1px solid;
            border-top: #B5B8C8 1px solid;
            border-left: #B5B8C8 1px solid;
            border-bottom: #B5B8C8 1px solid;
            height: 18px;
        }
        .textbox1
        {
            border-right: #B5B8C8 1px solid;
            border-top: #B5B8C8 1px solid;
            border-left: #B5B8C8 1px solid;
            border-bottom: #B5B8C8 1px solid;
            height: 18px;
        }
        .textbox2
        {
            border-right: #B5B8C8 1px solid;
            border-top: #B5B8C8 1px solid;
            border-left: #B5B8C8 1px solid;
            border-bottom: #B5B8C8 1px solid;
            height: 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server"></f:PageManager>
    <f:Panel ID="Panel1" runat="server" BodyPadding="3px" ShowBorder="false" ShowHeader="false" Height="300px" Layout="Anchor">
        <Items>
            <f:ContentPanel ID="ContentPanel1" runat="server" BodyPadding="5px" ShowBorder="false" ShowHeader="false" Title="ContentPanel">
                <table width="100%">
                    <tr>
                        <td width="50%">
                            <table>
                                <tr>
                                    <td>
                                        &nbsp;&nbsp;数据表名称：
                                    </td>
                                    <td width="200">
                                        <f:TextBox ID="txt_ShowTableName" runat="server" Width="200">
                                        </f:TextBox>
                                    </td>
                                    <td>
                                        <f:Button ID="btnSeach" Text="查找" runat="server" Icon="Magnifier" OnClick="btnrefresh_Click">
                                        </f:Button>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td width="50%" align="right">
                            <table>
                                <tr>
                                    <td>
                                        &nbsp;&nbsp;字段数量：
                                    </td>
                                    <td width="50">
                                        <f:NumberBox ID="txt_AddFieldCount" runat="server" Width="50" DecimalPrecision="0" MinValue="1" MaxValue="100">
                                        </f:NumberBox>
                                    </td>
                                    <td>
                                        <f:Button ID="btnAdd" Text="添加" runat="server" Icon="Add" OnClick="btnAdd_Click">
                                        </f:Button>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" height="20" style="font-size: 11pt;" align="right">
                            操作说明：1、数据源如果多个，中间请用英文的分号";"隔开。&nbsp;2、显示宽度、表单宽度如自适应请填&nbsp;0&nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                    </tr>
                </table>
            </f:ContentPanel>
            <f:Panel ID="Panel2" ShowBorder="True" ShowHeader="false" runat="server" AnchorValue="100% -60" Layout="Fit">
                <Toolbars>
                    <f:Toolbar runat="server" ID="Toolbar2">
                        <Items>
                            <f:Button ID="btnSave1" Text="保存" runat="server" Icon="PageSave" EnablePostBack="true" OnClick="btnSave1_Click">
                            </f:Button>
                            <f:Button ID="btnDel" Text="删除" runat="server" Icon="Cancel" EnablePostBack="true" OnClick="btnDel_OnClick" ConfirmText="您确定删除选中的字段吗？">
                            </f:Button>
                            <f:Button ID="Button1" Text="仅基本信息" runat="server" Icon="PageRefresh" EnablePostBack="true" OnClick="Button1_Click" Hidden="true">
                            </f:Button>
                            <f:Button ID="Button2" Text="仅列表、搜索" runat="server" Icon="PageRefresh" EnablePostBack="true" OnClick="Button2_Click" Hidden="true">
                            </f:Button>
                            <f:Button ID="Button3" Text="仅表单" runat="server" Icon="PageRefresh" EnablePostBack="true" OnClick="Button3_Click" Hidden="true">
                            </f:Button>
                            <f:Button ID="Button4" Text="全部显示" runat="server" Icon="PageRefresh" EnablePostBack="true" OnClick="Button4_Click" Hidden="true">
                            </f:Button>
                        </Items>
                    </f:Toolbar>
                </Toolbars>
                <Items>
                    <f:Grid ID="Grid1" Title="基本设置" PageSize="1000" ShowBorder="false" ShowHeader="false" AllowPaging="false" runat="server" EnableCheckBoxSelect="true" DataKeyNames="RowGuid" IsDatabasePaging="true" OnPageIndexChange="Grid1_PageIndexChange" OnRowDataBound="Grid1_RowDataBound" RowVerticalAlign="Middle">
                        <Columns><f:RowNumberField></f:RowNumberField>
                            <f:TemplateField Width="85px" HeaderText="<div style='text-align:center;'>显示名称</div>" TextAlign="Center">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txt_ShowFieldName" Width="75" Text='<%# DataBinder.Eval(Container.DataItem, "ShowFieldName").ToString()%>' CssClass="textbox1">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="85px" HeaderText="<div style='text-align:center;'>显示类型</div>" TextAlign="Center">
                                <ItemTemplate>
                                    <asp:DropDownList Width="75" runat="server" ID="ddl_FieldType" CssClass="textbox2">
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
                            <f:TemplateField Width="60px" HeaderText="<div style='text-align:center;'>小数位数</div>" TextAlign="Center">
                                <ItemTemplate>
                                    <asp:DropDownList Width="50" runat="server" ID="ddl_DecimalPrecision" CssClass="textbox2">
                                        <asp:ListItem Text="" Value=""></asp:ListItem>
                                        <asp:ListItem Text="0" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="205px" HeaderText="<div style='text-align:center;'>数据源</div>" TextAlign="Center">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txt_Source" Width="190" CssClass="textbox1" Text='<%# DataBinder.Eval(Container.DataItem, "SourceName").ToString()%>'></asp:TextBox>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="55px" HeaderText="<div style='text-align:center;'>排序值</div>" TextAlign="Center">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txt_SortNumber" Width="45" CssClass="sortnumber" Text='<%# DataBinder.Eval(Container.DataItem, "SortNumber").ToString()%>'>
                                    </asp:TextBox>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="65px" HeaderText="<div style='text-align:center;color:red;'>列表显示</div>">
                                <ItemTemplate>
                                    <asp:DropDownList Width="55" runat="server" ID="ddl_ListIsShow">
                                        <asp:ListItem Text="是" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="否" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="65px" HeaderText="<div style='text-align:center;color:red;'>显示位置</div>" TextAlign="Center" Hidden="true">
                                <ItemTemplate>
                                    <asp:DropDownList Width="55" runat="server" ID="ddl_ListPosition" CssClass="textbox2">
                                        <asp:ListItem Text="左" Value="l"></asp:ListItem>
                                        <asp:ListItem Text="中" Value="m" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="右" Value="r"></asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="65px" HeaderText="<div style='text-align:center;color:red;'>显示宽度</div>" TextAlign="Center">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txt_ListGridWidth" Width="55" Text='<%# DataBinder.Eval(Container.DataItem, "ListGridWidth").ToString()%>' CssClass="textbox1">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="65px" HeaderText="<div style='text-align:center;color:red;'>搜索字段</div>" TextAlign="Center">
                                <ItemTemplate>
                                    <asp:DropDownList Width="55" runat="server" ID="ddl_SearchIsShow" CssClass="textbox2">
                                        <asp:ListItem Text="是" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="否" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="75px" HeaderText="<div style='text-align:center;color:red;'>搜索框宽度</div>" TextAlign="Center">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txt_SearchWidth" Width="65" CssClass="textbox1" Text='<%# DataBinder.Eval(Container.DataItem, "SearchWidth").ToString()%>'>
                                    </asp:TextBox>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="65px" HeaderText="<div style='text-align:center;'>是否必填</div>" TextAlign="Center">
                                <ItemTemplate>
                                    <asp:DropDownList Width="55" runat="server" ID="ddl_Required" CssClass="textbox2">
                                        <asp:ListItem Text="是" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="否" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="65px" HeaderText="<div style='text-align:center;'>表单宽度</div>" TextAlign="Center">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txt_FormItemWidth" Width="55" CssClass="textbox1" Text='<%# DataBinder.Eval(Container.DataItem, "FormItemWidth").ToString()%>'>
                                    </asp:TextBox>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="85px" HeaderText="<div style='text-align:center;'>下一字段新行</div>" TextAlign="Center">
                                <ItemTemplate>
                                    <asp:DropDownList Width="75" runat="server" ID="ddl_FormIsNewTR" CssClass="textbox2">
                                        <asp:ListItem Text="是" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="否" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="85px" HeaderText="<div style='text-align:center;'>表单验证</div>" TextAlign="Center">
                                <ItemTemplate>
                                    <asp:DropDownList Width="75" runat="server" ID="ddl_ValidateType" CssClass="textbox2">
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
