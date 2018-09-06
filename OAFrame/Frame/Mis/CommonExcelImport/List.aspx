<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Com.Sinoyd.Mis.WebUI.CommonExcelImport.List" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>通用Excel管理</title>
</head>
<body>
    <form id="form1" runat="server">
        <script type="text/javascript">
            function showtab1(a, b, c) {
                parent.window.showwindows(a, b, c);
            }

            function showmore1(a, b, c) {
                parent.window.showwindows(a, b, "培训管理-" + c);
            }

            function showtabedit(a, b) {
                var pm = "<%=Request["ModuleGuid"] %>";
                var sm = a + "edit";
                parent.window.showwindows(sm, b + '&pm=' + pm + '&sm=' + sm, "客户信息修改");
            }
        </script>
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" Theme="Neptune"></f:PageManager>
        <f:Panel ID="Panel1" runat="server" BodyPadding="3px" ShowBorder="false" ShowHeader="false"
            Height="300px" Layout="Anchor">
            <Items>
                <f:ContentPanel ID="ContentPanel1" runat="server" BodyPadding="5px" ShowBorder="false"
                    ShowHeader="false" Title="ContentPanel">
                    <table>
                        <tr>
                            <td>&nbsp;&nbsp;标题：
                            </td>
                            <td width="125">
                                <f:TextBox ID="txt_Title" runat="server" Width="200">
                                </f:TextBox>
                            </td>
                            <td>
                                <f:Button ID="btnSeach" Text="查找" runat="server" Icon="Magnifier" OnClick="btnSearch_Click">
                                </f:Button>
                            </td>
                        </tr>
                    </table>
                </f:ContentPanel>
                <f:Panel ID="Panel2" ShowBorder="True" ShowHeader="false" runat="server" AnchorValue="100% -36"
                    Layout="Fit">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <f:Button ID="btnNew" Text="添加" runat="server" Icon="Add" EnablePostBack="false">
                                </f:Button>
                                <f:Button ID="btnDel" Text="删除" runat="server" Icon="Cancel" EnablePostBack="true"
                                    OnClick="btnDel_OnClick" ConfirmText="您确定删除选中的记录吗？">
                                </f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Items>
                        <f:Grid runat="server" ID="Grid1" Title="Grid1" PageSize="20" ShowBorder="false"
                            ShowHeader="False" SortDirection="ASC" AllowPaging="true" EnableCheckBoxSelect="true"
                            DataKeyNames="RowGuid" IsDatabasePaging="true" OnSort="Grid1_Sort" OnPageIndexChange="Grid1_PageIndexChange"
                            EnableTextSelection="true" EnableHeaderMenu="false">
                            <Columns>
                                <f:RowNumberField></f:RowNumberField>
                                <f:TemplateField HeaderText="标题" ExpandUnusedSpace="true" TextAlign="Center">
                                    <ItemTemplate>
                                        <div style="text-align: left;">
                                            <%# DataBinder.Eval(Container.DataItem, "Title").ToString()%>
                                        </div>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:TemplateField Width="150px" HeaderText="SQL表名" TextAlign="Center">
                                    <ItemTemplate>
                                        <div style="text-align: Center;">
                                            <%# DataBinder.Eval(Container.DataItem, "SQLTableName").ToString()%>
                                        </div>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:TemplateField Width="150px" HeaderText="Sheet名" TextAlign="Center">
                                    <ItemTemplate>
                                        <div style="text-align: Center;">
                                            <%# DataBinder.Eval(Container.DataItem, "SheetName").ToString()%>
                                        </div>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:TemplateField HeaderText="字段设置" Width="80" TextAlign="Center">
                                    <ItemTemplate>
                                        <div style="text-align: center;">
                                            <a href="<%# GetEditUrl2(DataBinder.Eval(Container.DataItem, "[Title]"), DataBinder.Eval(Container.DataItem, "[RowGuid]"))%>">
                                                <img alt="字段设置" src="../images/icons/arrow_ne.png"></a>
                                        </div>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:TemplateField HeaderText="预览" Width="50" TextAlign="Center">
                                    <ItemTemplate>
                                        <a target="_blank" href="ListQuery.aspx?TableRowGuid=<%# DataBinder.Eval(Container.DataItem, "[RowGuid]")%>">
                                            <img src="../images/icons/zoom.png"></a>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <%--<f:TemplateField HeaderText="导出SQL" Width="75" TextAlign="Center">
                                    <ItemTemplate>
                                        <a target="_Blank" href='../AutoCreateCode/InitSql.aspx?TableRowGuid=<%# DataBinder.Eval(Container.DataItem, "[RowGuid]")%>'>
                                            <img src="../images/icons/script_code.png"></a>
                                    </ItemTemplate>
                                </f:TemplateField>--%>
                                <f:TemplateField HeaderText="编辑" Width="50" TextAlign="Center">
                                    <ItemTemplate>
                                        <div style="text-align: center;">
                                            <a href="<%# GetEditUrl(DataBinder.Eval(Container.DataItem, "[Title]"), DataBinder.Eval(Container.DataItem, "[RowGuid]"))%>">
                                                <img alt="编辑" src="../images/icons/page_edit.png"></a>
                                        </div>
                                    </ItemTemplate>
                                </f:TemplateField>
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Panel>
            </Items>
        </f:Panel>
        <f:Window ID="Window1" Title="Edit" EnableIFrame="true" runat="server" EnableCollapse="true"
            EnableDrag="true" IFrameUrl="about:blank" Target="Self" EnableMaximize="true"
            AutoScroll="true" Width="1000px" Height="450px" CloseAction="HidePostBack" OnClose="Window1_OnClose"
            Hidden="true">
        </f:Window>
        <f:Window ID="Window2" Title="Edit" EnableIFrame="true" runat="server" EnableCollapse="true"
            EnableDrag="true" IFrameUrl="about:blank" Target="Parent" EnableMaximize="false"
            AutoScroll="false" Width="1000px" Height="483px" CloseAction="HidePostBack" Hidden="true">
        </f:Window>
    </form>
</body>
</html>
