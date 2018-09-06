<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="TK.Mis.Web.TableInfoSet.Circle.List" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Mis表管理</title>
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
    <f:Panel ID="Panel1" runat="server" BodyPadding="3px" ShowBorder="false" ShowHeader="false" Height="300px" Layout="Anchor">
        <Items>
            <f:ContentPanel ID="ContentPanel1" runat="server" BodyPadding="5px" ShowBorder="false" ShowHeader="false" Title="ContentPanel">
                <table>
                    <tr>
                        <td>
                            &nbsp;&nbsp;标题：
                        </td>
                        <td width="200">
                            <f:TextBox ID="txt_ShowTableName" runat="server" Width="200">
                            </f:TextBox>
                        </td>
                        <td style="display: none;">
                            &nbsp;&nbsp;状态：
                        </td>
                        <td width="100" style="display: none;">
                            <f:DropDownList runat="server" ID="rbl_Type" Label="状态" Width="100">
                                <f:ListItem Selected="true" Text="所有状态" Value="所有状态"></f:ListItem>
                                <f:ListItem Text="未开始" Value="0"></f:ListItem>
                                <f:ListItem Text="进行中" Value="1"></f:ListItem>
                                <f:ListItem Text="已结束" Value="2"></f:ListItem>
                            </f:DropDownList>
                        </td>
                        <td>
                            <f:Button ID="btnSeach" Text="查找" runat="server" Icon="Magnifier" OnClick="btnSearch_Click">
                            </f:Button>
                        </td>
                    </tr>
                </table>
            </f:ContentPanel>
            <f:Panel ID="Panel2" ShowBorder="false" ShowHeader="false" runat="server" AnchorValue="100% -36" Layout="Fit">
                <Toolbars>
                    <f:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <f:Button ID="btnNew" Text="周期设置" runat="server" Icon="Add" EnablePostBack="false">
                            </f:Button>
                            <f:Button ID="btnDel" Text="删除" runat="server" Icon="Cancel" EnablePostBack="true" OnClick="btnDel_OnClick" ConfirmText="您确定删除选中的记录吗？">
                            </f:Button>
                            <f:Button ID="btnCreate" Text="生成" runat="server" Icon="ApplicationGo" OnClick="btnCreate_OnClick" ConfirmText="您确定生成页面文件吗？" Hidden="true">
                            </f:Button>
                        </Items>
                    </f:Toolbar>
                </Toolbars>
                <Items>
                    <f:Grid ID="Grid1" Title="Grid1" PageSize="20" ShowBorder="false" ShowHeader="False" SortDirection="ASC" AllowPaging="true" runat="server" EnableCheckBoxSelect="true" DataKeyNames="RowGuid" IsDatabasePaging="true" OnSort="Grid1_Sort" OnPageIndexChange="Grid1_PageIndexChange" EnableTextSelection="true" RowVerticalAlign="Middle">
                        <Columns><f:RowNumberField></f:RowNumberField>
                            <f:TemplateField ColumnID="Title" SortField="Title" ExpandUnusedSpace="true" TextAlign="Center" HeaderText="标题">
                                <ItemTemplate>
                                    <div style="text-align: left;">
                                        <%# new TK.DB.MsSql.SqlCommon().ExecuteSQLToStr("select TableName from T_MisTableSetCircle where RowGuid='" + DataBinder.Eval(Container.DataItem, "TableRowGuid").ToString() + "'")+"_" + DataBinder.Eval(Container.DataItem, "Title").ToString()+""%></div>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField TextAlign="Center" Width="80" HeaderText="状态" Hidden="true">
                                <ItemTemplate>
                                    <%# getStatus(DataBinder.Eval(Container.DataItem, "[Status]"))%>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField TextAlign="Center" Width="80" HeaderText="汇总">
                                <ItemTemplate>
                                    <div style="text-align: center; padding-top: 2px;">
                                        <a href="<%# GetViewUrl(DataBinder.Eval(Container.DataItem, "[Title]"),DataBinder.Eval(Container.DataItem, "[RowGuid]"))%>">
                                            <img src="../../../images/icons/Table.png"></a>
                                    </div>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField TextAlign="Center" Width="80" HeaderText="编辑">
                                <ItemTemplate>
                                    <a href="<%# GetEditUrl(DataBinder.Eval(Container.DataItem, "[Title]"), DataBinder.Eval(Container.DataItem, "[RowGuid]"))%>">
                                        <img alt="编辑" src="../../../images/icons/page_edit.png"></a>
                                </ItemTemplate>
                            </f:TemplateField>
                        </Columns>
                    </f:Grid>
                </Items>
            </f:Panel>
        </Items>
    </f:Panel>
    <f:Window ID="Window1" Title="Edit" EnableIFrame="true" runat="server" EnableCollapse="true" EnableDrag="true" IFrameUrl="about:blank" Target="Parent" EnableMaximize="true" AutoScroll="true" Width="500px" Height="300px" CloseAction="HidePostBack" OnClose="Window1_OnClose" Hidden="true">
    </f:Window>
    <f:Window ID="Window2" Title="Edit" EnableIFrame="true" runat="server" EnableCollapse="true" EnableDrag="true" IFrameUrl="about:blank" Target="Parent" EnableMaximize="true" AutoScroll="true" Width="1350px" Height="500px" CloseAction="HidePostBack" Hidden="true">
    </f:Window>
    </form>
</body>
</html>
