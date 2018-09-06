<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Com.Sinoyd.Mis.WebUI.Chart.List" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>图表管理</title>
</head>
<body>
    <form id="form1" runat="server">
    <script type="text/javascript">
        function showtab1(a, b,c) {
            parent.window.showwindows(a, b, c);
        }
        
        function showmore1(a, b, c) {
            parent.window.showwindows(a, b, "培训管理-" + c);
        }
        
        function showtabedit(a, b) {
            var pm="<%=Request["ModuleGuid"] %>";
            var sm=a+"edit";
            parent.window.showwindows(sm, b+'&pm='+pm+'&sm='+sm, "客户信息修改");

        }
     
 
    </script>
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server"></f:PageManager>
    <f:Panel ID="Panel1" runat="server" BodyPadding="3px" ShowBorder="false" ShowHeader="false"
        Height="300px" Layout="Anchor">
        <Items>
            <f:ContentPanel ID="ContentPanel1" runat="server" BodyPadding="5px" ShowBorder="false"
                ShowHeader="false" Title="ContentPanel">
                <table>
                    <tr>
                        <td>
                            &nbsp;&nbsp;标题：
                        </td>
                        <td width="225">
                            <f:TextBox ID="txt_Title" runat="server" Width="220">
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
                            <f:Button ID="btnImport" Text="数据导入" runat="server" Icon="DatabaseAdd" Hidden="true">
                            </f:Button>
                            <f:Label ID="Label1" runat="server" Text="&nbsp;&nbsp;" Width="100" Hidden="true">
                            </f:Label>
                            <f:Label ID="lblMmkj" runat="server" Text="&nbsp;&nbsp;命名空间：" Hidden="true">
                            </f:Label>
                            <f:TextBox ID="txt_Mmkj" runat="server" Width="200" EmptyText="示例：Zjgnw.Web.Pages.Company"
                                Hidden="true">
                            </f:TextBox>
                            <f:Label ID="lblSclj" runat="server" Text="&nbsp;&nbsp;生成路径：" Hidden="true">
                            </f:Label>
                            <f:TextBox ID="txt_Sclj" runat="server" Width="200" EmptyText="示例：Zjgnw/Web/Pages/Company"
                                Hidden="true">
                            </f:TextBox>
                            <f:Button ID="btnCreate" Text="生成" runat="server" Icon="ApplicationGo" OnClick="btnCreate_OnClick"
                                ConfirmText="您确定生成页面文件吗？" Hidden="true">
                            </f:Button>
                        </Items>
                    </f:Toolbar>
                </Toolbars>
                <Items>
                    <f:Grid ID="Grid1" Title="Grid1" PageSize="20" ShowBorder="false" ShowHeader="False"
                        SortDirection="ASC" AllowPaging="true" runat="server" EnableCheckBoxSelect="true"
                        DataKeyNames="RowGuid" IsDatabasePaging="true" OnSort="Grid1_Sort" OnPageIndexChange="Grid1_PageIndexChange"
                        EnableTextSelection="true" OnRowDataBound="Grid1_RowDataBound" EnableHeaderMenu="false">
                        <Columns>
                            <f:RowNumberField></f:RowNumberField>
                            <f:TemplateField ColumnID="Title" SortField="Title" ExpandUnusedSpace="true" HeaderText="标题"
                                TextAlign="Center">
                                <ItemTemplate>
                                    <div style="text-align: left; padding-top: 2px;">
                                        <%# DataBinder.Eval(Container.DataItem, "Title").ToString()%></div>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="100px" HeaderText="横轴" TextAlign="Center">
                                <ItemTemplate>
                                    <div style="text-align: center; padding-top: 2px;">
                                        <%# DataBinder.Eval(Container.DataItem, "XValue").ToString()%></div>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="100px" HeaderText="纵轴" TextAlign="Center">
                                <ItemTemplate>
                                    <div style="text-align: center; padding-top: 2px;">
                                        <%# DataBinder.Eval(Container.DataItem, "YValue").ToString()%></div>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField HeaderText="字段设置" Width="75" TextAlign="Center">
                                <ItemTemplate>
                                    <div style="text-align: center; padding-top: 2px;">
                                        <a href="<%# GetEditUrl2(DataBinder.Eval(Container.DataItem, "[Title]"), DataBinder.Eval(Container.DataItem, "[RowGuid]"))%>">
                                            <img alt="字段设置" src="../images/icons/page_edit.png"></a>
                                    </div>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField HeaderText="预览" Width="70" TextAlign="Center">
                                <ItemTemplate>
                                    <div style="text-align: center; padding-top: 2px;">
                                        <a target="_blank" href="ListQuery.aspx?TableRowGuid=<%# DataBinder.Eval(Container.DataItem, "[RowGuid]")%>">
                                            <img src="../images/icons/zoom.png"></a>
                                    </div>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField HeaderText="操作" Width="70" TextAlign="Center">
                                <ItemTemplate>
                                    <div style="text-align: center; padding-top: 2px;">
                                        <a href="<%# GetEditUrl(DataBinder.Eval(Container.DataItem, "[Title]"), DataBinder.Eval(Container.DataItem, "[RowGuid]"))%>">
                                            <img alt="编辑" src="../images/icons/page_edit.png"></a> <a href="<%# GetCreateSql(DataBinder.Eval(Container.DataItem, "[RowGuid]"))%>">
                                                <img src="../images/icons/zoom.png"></a>
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
        EnableDrag="true" IFrameUrl="about:blank" Target="Parent" EnableMaximize="true"
        AutoScroll="true" Width="700px" Height="500px" CloseAction="HidePostBack" OnClose="Window1_OnClose"
        Hidden="true">
    </f:Window>
    <f:Window ID="Window2" Title="Edit" EnableIFrame="true" runat="server" EnableCollapse="true"
        EnableDrag="true" IFrameUrl="about:blank" Target="Parent" EnableMaximize="true"
        AutoScroll="true" Width="800px" Height="400px" CloseAction="HidePostBack" Hidden="true">
    </f:Window>
    </form>
</body>
</html>
