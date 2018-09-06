<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add2.aspx.cs" Inherits="Com.Sinoyd.Mis.WebUI.Report.Add2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>报表信息</title>
</head>
<body>
    <form id="form1" runat="server">
    <script type="text/javascript">
        function showtab1(a, b, c) {
            parent.window.showwindows(a, b, c);
        }

        function showtabedit(RowGuid) {
            parent.window.showwindows(RowGuid, "http://localhost:20000/SinoydMis10/Report/Templet/View.aspx?RowGuid=" + RowGuid, "模板编辑");
        }
    </script>
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Panel1" EnableAjax="true">
    </f:PageManager>
    <f:Panel ID="Panel1" runat="server" ShowBorder="False" ShowHeader="false" BodyPadding="5px"
        AutoScroll="true">
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
                                <f:ListItem Text="简易报表" Value="1" />
                                <f:ListItem Text="固定报表" Value="2" Selected="true" />
                                <f:ListItem Text="分组报表" Value="3" />
                                <f:ListItem Text="交叉统计表" Value="4" />
                            </f:DropDownList>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:DropDownList runat="server" ID="ddl_EnablePage" Label="数据来源" Required="true" ShowRedStar="true">
                                <f:ListItem Selected="true" Text="SQL语句" Value="1" />
                                <f:ListItem Text="外调方法" Value="0" />
                            </f:DropDownList>
                            <f:DropDownList runat="server" ID="ddl_MethodGuid" Label="外调方法">
                            </f:DropDownList>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:TextArea runat="server" ID="txt_SourceSql" Label="主查询sql" Height="60">
                            </f:TextArea>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:TextArea runat="server" ID="txt_FilterSql" Label="过滤sql" Text="where 1=1" Height="60">
                            </f:TextArea>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:TextBox runat="server" ID="txt_FieldListSelect" Label="查询字段列表">
                            </f:TextBox>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:Form runat="server" ID="formTemplet" Title="模板编辑" Height="150" AutoScroll="true">
                                <Toolbars>
                                    <f:Toolbar ID="toolbar11" runat="server">
                                        <Items>
                                            <f:Button ID="btnTempletNew" Text="添加" runat="server" Icon="Add">
                                            </f:Button>
                                            <f:Button ID="btnTempletEdit" Text="编辑" runat="server" Icon="PageEdit" OnClick="btnEdit_Click">
                                            </f:Button>
                                            <f:Button ID="btnTempletDelete" Text="删除" runat="server" Icon="Delete" OnClick="btnDel_OnClick">
                                            </f:Button>
                                            <%--<f:Button ID="btnTempletView" Text="预览" runat="server" Icon="Zoom" OnClick="btnDetail_Click">
                                            </f:Button>--%>
                                        </Items>
                                    </f:Toolbar>
                                </Toolbars>
                                <Rows>
                                    <f:FormRow>
                                        <Items>
                                            <f:Grid ID="Grid1" Title="Grid1" PageSize="20" ShowBorder="false" ShowHeader="False"
                                                SortDirection="ASC" AllowPaging="false" runat="server" EnableCheckBoxSelect="true"
                                                DataKeyNames="RowGuid" IsDatabasePaging="true" EnableHeaderMenu="false">
                                                <Columns>
                                                    <f:TemplateField ExpandUnusedSpace="true" HeaderText="模板名称" TextAlign="Center">
                                                        <ItemTemplate>
                                                            <div style="text-align: left; padding-top: 2px;">
                                                                <%# DataBinder.Eval(Container.DataItem, "Title").ToString()%></div>
                                                        </ItemTemplate>
                                                    </f:TemplateField>
                                                    <f:TemplateField HeaderText="正文编辑" TextAlign="Center">
                                                        <ItemTemplate>
                                                                <a href='Templet/View.aspx?RowGuid=<%# DataBinder.Eval(Container.DataItem, "RowGuid").ToString()%>&ID=<%# Guid.NewGuid().ToString()%>' target="_blank">
                                                                    正文编辑</a>
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
    <f:Window ID="Window1" Title="Edit" EnableIFrame="true" runat="server" EnableCollapse="true"
        EnableDrag="false" IFrameUrl="about:blank" Target="Parent" EnableMaximize="true"
        AutoScroll="true" Width="400px" Height="250px" CloseAction="HidePostBack" OnClose="Window1_OnClose"
        Hidden="true">
    </f:Window>
    </form>
</body>
</html>
