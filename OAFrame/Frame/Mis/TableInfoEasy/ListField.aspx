<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListField.aspx.cs" Inherits="Com.Sinoyd.Mis.WebUI.TableInfoEasy.ListField" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>字段管理</title>
    <style>
        .sortnumber {
            border-right: #B5B8C8 1px solid;
            border-top: #B5B8C8 1px solid;
            border-left: #B5B8C8 1px solid;
            border-bottom: #B5B8C8 1px solid;
            height: 20px;
        }

        .textbox1 {
            border-right: #B5B8C8 1px solid;
            border-top: #B5B8C8 1px solid;
            border-left: #B5B8C8 1px solid;
            border-bottom: #B5B8C8 1px solid;
            height: 20px;
        }

        .textbox2 {
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
                                        <td>&nbsp;&nbsp;数据表名称：
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
                                        <td>&nbsp;&nbsp;字段数量：
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
                            <td colspan="2" height="20" style="font-size: 11pt;" align="right">操作说明：1、数据源如果多个，中间请用英文的分号";"隔开。&nbsp;2、显示宽度、表单宽度如自适应请填&nbsp;0&nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                        </tr>
                    </table>
                </f:ContentPanel>
                <f:Panel ID="Panel2" ShowBorder="True" ShowHeader="false" runat="server" AnchorValue="100% -65" Layout="Fit">
                    <Toolbars>
                        <f:Toolbar runat="server" ID="Toolbar2">
                            <Items>
                                <f:Button ID="btnSave1" Text="保存" runat="server" Icon="PageSave" EnablePostBack="true" OnClick="btnSave1_Click">
                                </f:Button>
                                <f:Button ID="btnDel" Text="删除" runat="server" Icon="Cancel" EnablePostBack="true" OnClick="btnDel_OnClick" ConfirmText="您确定删除选中的字段吗？">
                                </f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Items>
                        <f:Grid ID="Grid1" Title="基本设置" PageSize="1000" ShowBorder="false" ShowHeader="false" AllowPaging="false"
                            runat="server" EnableCheckBoxSelect="true" DataKeyNames="RowGuid" IsDatabasePaging="true"
                            OnPageIndexChange="Grid1_PageIndexChange" OnRowDataBound="Grid1_RowDataBound" RowVerticalAlign="Middle" EnableHeaderMenu="false">
                            <Columns>
                                <f:RowNumberField></f:RowNumberField>
                                <f:TemplateField Width="80px" HeaderText="显示名称" TextAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txt_ShowFieldName" Width="70" Text='<%# DataBinder.Eval(Container.DataItem, "ShowFieldName").ToString()%>' CssClass="textbox1">
                                        </asp:TextBox>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:TemplateField Width="90px" HeaderText="显示类型" TextAlign="Center">
                                    <ItemTemplate>
                                        <asp:DropDownList Width="80" runat="server" ID="ddl_FieldType" CssClass="textbox2">
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
                                <f:TemplateField Width="80px" HeaderText="小数位数" TextAlign="Center">
                                    <ItemTemplate>
                                        <asp:DropDownList Width="70" runat="server" ID="ddl_DecimalPrecision" CssClass="textbox2">
                                            <asp:ListItem Text="" Value=""></asp:ListItem>
                                            <asp:ListItem Text="0" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:TemplateField Width="200" HeaderText="数据源" TextAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txt_Source" Width="98%" CssClass="textbox1" Text='<%# DataBinder.Eval(Container.DataItem, "SourceName").ToString()%>'></asp:TextBox>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:TemplateField Width="65px" HeaderText="排序值" TextAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txt_SortNumber" Width="55" CssClass="sortnumber" Text='<%# DataBinder.Eval(Container.DataItem, "SortNumber").ToString()%>'>
                                        </asp:TextBox>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:TemplateField Width="80px" HeaderText="列表显示" TextAlign="Center">
                                    <ItemTemplate>
                                        <asp:DropDownList Width="70" runat="server" ID="ddl_ListIsShow">
                                            <asp:ListItem Text="是" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="否" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:TemplateField Width="80px" HeaderText="显示位置" TextAlign="Center" Hidden="true">
                                    <ItemTemplate>
                                        <asp:DropDownList Width="70" runat="server" ID="ddl_ListPosition" CssClass="textbox2">
                                            <asp:ListItem Text="左" Value="l"></asp:ListItem>
                                            <asp:ListItem Text="中" Value="m" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="右" Value="r"></asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:TemplateField Width="80px" HeaderText="显示宽度" TextAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txt_ListGridWidth" Width="70" Text='<%# DataBinder.Eval(Container.DataItem, "ListGridWidth").ToString()%>' CssClass="textbox1">
                                        </asp:TextBox>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:TemplateField Width="80px" HeaderText="搜索字段" TextAlign="Center">
                                    <ItemTemplate>
                                        <asp:DropDownList Width="70" runat="server" ID="ddl_SearchIsShow" CssClass="textbox2">
                                            <asp:ListItem Text="是" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="否" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:TemplateField Width="90px" HeaderText="搜索框宽度" TextAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txt_SearchWidth" Width="80" CssClass="textbox1" Text='<%# DataBinder.Eval(Container.DataItem, "SearchWidth").ToString()%>'>
                                        </asp:TextBox>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:TemplateField Width="80px" HeaderText="是否必填" TextAlign="Center">
                                    <ItemTemplate>
                                        <asp:DropDownList Width="70" runat="server" ID="ddl_Required" CssClass="textbox2">
                                            <asp:ListItem Text="是" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="否" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:TemplateField Width="80px" HeaderText="表单宽度" TextAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txt_FormItemWidth" Width="70" CssClass="textbox1" Text='<%# DataBinder.Eval(Container.DataItem, "FormItemWidth").ToString()%>'>
                                        </asp:TextBox>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:TemplateField Width="105px" HeaderText="下一字段新行" TextAlign="Center">
                                    <ItemTemplate>
                                        <asp:DropDownList Width="90" runat="server" ID="ddl_FormIsNewTR" CssClass="textbox2">
                                            <asp:ListItem Text="是" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="否" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:TemplateField Width="100px" HeaderText="表单验证" TextAlign="Center">
                                    <ItemTemplate>
                                        <asp:DropDownList Width="90" runat="server" ID="ddl_ValidateType" CssClass="textbox2">
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
