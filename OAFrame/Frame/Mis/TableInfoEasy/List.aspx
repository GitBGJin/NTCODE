<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Com.Sinoyd.Mis.WebUI.TableInfoEasy.List" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Mis表管理(简易功能)</title>
</head>
<body>
    <form id="form1" runat="server">
        <script type="text/javascript">
            function showtab1(a, b, c) {
                parent.window.showwindows(a, b, c);
            }
        </script>
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server"></f:PageManager>
        <f:Panel ID="Panel1" runat="server" BodyPadding="3px" ShowBorder="false" ShowHeader="false" Height="300px" Layout="Anchor">
            <Items>
                <f:ContentPanel ID="ContentPanel1" runat="server" BodyPadding="5px" ShowBorder="false" ShowHeader="false" Title="ContentPanel">
                    <table>
                        <tr>
                            <td>&nbsp;&nbsp;数据表名称：
                            </td>
                            <td width="125">
                                <f:TextBox ID="txt_ShowTableName" runat="server" Width="200">
                                </f:TextBox>
                            </td>
                            <td>
                                <f:Button ID="btnSeach" Text="查询" runat="server" Icon="SystemSearch" OnClick="btnSearch_Click">
                                </f:Button>
                            </td>
                        </tr>
                    </table>
                </f:ContentPanel>
                <f:Panel ID="Panel2" ShowBorder="false" ShowHeader="false" runat="server" AnchorValue="100% -36" Layout="Fit">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <f:Button ID="btnNew" Text="新增" runat="server" Icon="Add" EnablePostBack="false">
                                </f:Button>
                                <f:Button ID="btnDel" Text="删除" runat="server" Icon="Delete" EnablePostBack="true" OnClick="btnDel_OnClick" ConfirmText="您确定删除选中的记录吗？">
                                </f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Items>
                        <f:Grid ID="Grid1" Title="Grid1" PageSize="20" ShowBorder="false" ShowHeader="False"
                             SortDirection="ASC" AllowPaging="true" runat="server" EnableCheckBoxSelect="true"
                             DataKeyNames="RowGuid"  IsDatabasePaging="true" OnSort="Grid1_Sort"
                             OnPageIndexChange="Grid1_PageIndexChange" EnableTextSelection="true" RowVerticalAlign="Middle" EnableHeaderMenu="false">
                            <Columns>
                                <f:RowNumberField></f:RowNumberField>
                                <f:TemplateField ColumnID="ShowTableName" SortField="ShowTableName" ExpandUnusedSpace="true" TextAlign="Center" HeaderText="数据表名称">
                                    <ItemTemplate>
                                        <div style="text-align: left;">
                                            <%# DataBinder.Eval(Container.DataItem, "ShowTableName").ToString()%>
                                        </div>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:TemplateField ColumnID="FormAttach" SortField="FormAttach" HeaderText="有无附件" Width="80" TextAlign="Center">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "FormAttach").ToString()=="1"?"有":"无"%>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:TemplateField ColumnID="FormWidth" SortField="FormWidth" HeaderText="弹窗宽度" Width="80" TextAlign="Center">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "FormWidth").ToString()%>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:TemplateField ColumnID="FormHeight" SortField="FormHeight" HeaderText="弹窗高度" Width="80" TextAlign="Center">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "FormHeight").ToString()%>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:TemplateField TextAlign="Center" Width="80" HeaderText="字段设置">
                                    <ItemTemplate>
                                        <a href="#" onclick="<%# GetEditUrl2(DataBinder.Eval(Container.DataItem, "[ShowTableName]"), DataBinder.Eval(Container.DataItem, "[RowGuid]"))%>">
                                            <img alt="字段设置" src="../images/icons/page_edit.png"></a>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:TemplateField TextAlign="Center" Width="60" HeaderText="编辑">
                                    <ItemTemplate>
                                        <a href="<%# GetEditUrl(DataBinder.Eval(Container.DataItem, "[ShowTableName]"), DataBinder.Eval(Container.DataItem, "[RowGuid]"))%>">
                                            <img alt="编辑" src="../images/icons/page_edit.png"></a>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:TemplateField TextAlign="Center" Width="60" HeaderText="预览">
                                    <ItemTemplate>
                                        <div style="text-align: center; padding-top: 2px;">
                                            <a href="<%# GetViewUrl(DataBinder.Eval(Container.DataItem, "[ShowTableName]"),DataBinder.Eval(Container.DataItem, "[RowGuid]"))%>">
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
        <f:Window ID="Window1" Title="Edit" EnableIFrame="true" runat="server" EnableCollapse="true" EnableDrag="true" IFrameUrl="about:blank" Target="Parent" EnableMaximize="true" AutoScroll="true" Width="620px" Height="250px" CloseAction="HidePostBack" OnClose="Window1_OnClose" Hidden="true">
        </f:Window>
        <f:Window ID="Window2" Title="Edit" EnableIFrame="true" runat="server" EnableCollapse="true" EnableDrag="true" IFrameUrl="about:blank" Target="Parent" EnableMaximize="true" AutoScroll="true" Width="1350px" Height="500px" CloseAction="HidePostBack" Hidden="true">
        </f:Window>
    </form>
</body>
</html>
