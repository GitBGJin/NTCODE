<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add1.aspx.cs" Inherits="Com.Sinoyd.Mis.WebUI.Report.Add1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>报表信息</title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Panel1" EnableAjax="true"></f:PageManager>
        <f:Panel ID="Panel1" runat="server" ShowBorder="False" ShowHeader="false" BodyPadding="5px"
            AutoScroll="false">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <f:Button ID="btnNew" Text="保存" runat="server" Icon="Disk" ValidateForms="Form3"
                            OnClick="btnNew_Click">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:Form Title="基本信息" BodyPadding="5px" ID="Form3" ShowHeader="false" ShowBorder="false"
                    runat="server" LabelWidth="100" EnableCollapse="true">
                    <Rows>
                        <f:FormRow>
                            <Items>
                                <f:TextBox runat="server" ID="txt_Title" Label="标题" Required="true" ShowRedStar="true">
                                </f:TextBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow>
                            <Items>
                                <f:DropDownList runat="server" ID="ddl_ReportType" Label="报表类型" Required="true" ShowRedStar="true"
                                    Readonly="true">
                                    <f:ListItem Selected="true" Text="简易报表" Value="1" />
                                    <f:ListItem Text="固定报表" Value="2" />
                                    <f:ListItem Text="分组报表" Value="3" />
                                    <f:ListItem Text="交叉统计表" Value="4" />
                                </f:DropDownList>
                                <f:DropDownList runat="server" ID="ddl_EnableNumber" Label="显示序号" Required="true"
                                    ShowRedStar="true">
                                    <f:ListItem Text="是" Value="1" />
                                    <f:ListItem Selected="true" Text="否" Value="0" />
                                </f:DropDownList>
                            </Items>
                        </f:FormRow>
                        <f:FormRow>
                            <Items>
                                <f:DropDownList runat="server" ID="ddl_EnablePage" Label="启用分页" Required="true" ShowRedStar="true">
                                    <f:ListItem Text="是" Value="1" />
                                    <f:ListItem Selected="true" Text="否" Value="0" />
                                </f:DropDownList>
                                <f:NumberBox runat="server" ID="txt_PageSize" Required="true" ShowRedStar="true"
                                    Text="0" Label="单页数据">
                                </f:NumberBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow>
                            <Items>
                                <f:TextArea runat="server" ID="txt_SourceSql" Label="主查询sql" Required="true" ShowRedStar="true"
                                    Height="40">
                                </f:TextArea>
                            </Items>
                        </f:FormRow>
                        <f:FormRow>
                            <Items>
                                <f:TextArea runat="server" ID="txt_FilterSql" Label="过滤sql" Required="true" ShowRedStar="true"
                                    Text="where 1=1" Height="40">
                                </f:TextArea>
                            </Items>
                        </f:FormRow>
                        <f:FormRow>
                            <Items>
                                <f:TextArea runat="server" ID="txt_OrderSql" Label="排序sql" Height="40">
                                </f:TextArea>
                            </Items>
                        </f:FormRow>
                        <f:FormRow>
                            <Items>
                                <f:TextBox runat="server" ID="txt_FieldListSelect" Label="查询字段列表" Required="true"
                                    ShowRedStar="true">
                                </f:TextBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow Hidden="true">
                            <Items>
                                <f:TextBox runat="server" ID="txt_TotalFieldList" Label="统计字段列表" Hidden="true">
                                </f:TextBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow>
                            <Items>
                                <f:Form runat="server" ID="FormGrid" Title="Excel备注信息设置">
                                    <Toolbars>
                                        <f:Toolbar runat="server" ID="Toolbar11">
                                            <Items>
                                                <f:Button ID="btnGrid_Add" Text="新增" runat="server" Icon="Add" OnClick="btnGrid_Add_Click">
                                                </f:Button>
                                                <f:Button ID="btnGrid_Edit" Text="保存" runat="server" Icon="Disk" OnClick="btnGrid_Edit_Click">
                                                </f:Button>
                                                <f:Button ID="btnGrid_Delete" Text="删除" runat="server" Icon="Cross" OnClick="btnGrid_Delete_Click" ConfirmText="您确定删除选定的记录吗？">
                                                </f:Button>
                                            </Items>
                                        </f:Toolbar>
                                    </Toolbars>
                                    <Rows>
                                        <f:FormRow>
                                            <Items>
                                                <f:Grid ID="Grid1" Title="Grid1" PageSize="20" ShowBorder="false" ShowHeader="False"
                                                    SortDirection="ASC" AllowPaging="false" runat="server" EnableCheckBoxSelect="true"
                                                    DataKeyNames="RowGuid" IsDatabasePaging="true"
                                                    EnableTextSelection="true" EnableHeaderMenu="false" AutoScroll="true" Height="130">
                                                    <Columns>
                                                        <f:RowNumberField></f:RowNumberField>
                                                        <f:TemplateField ExpandUnusedSpace="true" HeaderText="内容"
                                                            TextAlign="Center">
                                                            <ItemTemplate>
                                                                <div style="text-align: left; padding-top: 2px;">
                                                                    <asp:TextBox runat="server" ID="txt_GridNote" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "Note").ToString()%>'></asp:TextBox>
                                                                </div>
                                                            </ItemTemplate>
                                                        </f:TemplateField>
                                                        <f:TemplateField Width="60" HeaderText="排序值"
                                                            TextAlign="Center">
                                                            <ItemTemplate>
                                                                <div style="text-align: left; padding-top: 2px;">
                                                                    <asp:TextBox runat="server" ID="txt_GridSortNumber" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "SortNumber").ToString()%>'></asp:TextBox>
                                                                </div>
                                                            </ItemTemplate>
                                                        </f:TemplateField>
                                                    </Columns>
                                                </f:Grid>
                                            </Items>
                                        </f:FormRow>
                                    </Rows>
                                </f:Form>
                            </Items>
                        </f:FormRow>
                    </Rows>
                </f:Form>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
