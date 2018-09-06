<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListApply_OneTime.aspx.cs" Inherits="TK.Mis.Web.TableInfoSet.ListApply_OneTime" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>我的申报表</title>
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
                            &nbsp;&nbsp;报表名称：
                        </td>
                        <td width="125">
                            <f:TextBox ID="txt_ShowTableName" runat="server" Width="200">
                            </f:TextBox>
                        </td>
                        <td>
                            <f:Button ID="btnSeach" Text="查找" runat="server" Icon="Magnifier" OnClick="btnSearch_Click">
                            </f:Button>
                        </td>
                    </tr>
                </table>
            </f:ContentPanel>
            <f:Panel ID="Panel2" ShowBorder="true" ShowHeader="false" runat="server" AnchorValue="100% -36" Layout="Fit">
                <Toolbars>
                    <f:Toolbar ID="Toolbar1" runat="server" Hidden="true">
                        <Items>
                            <f:Button ID="btnNew" Text="添加" runat="server" Icon="Add" EnablePostBack="false">
                            </f:Button>
                            <f:Button ID="btnDel" Text="删除" runat="server" Icon="Cancel" EnablePostBack="true" OnClick="btnDel_OnClick" ConfirmText="您确定删除选中的记录吗？">
                            </f:Button>
                            <f:Button ID="btnCreate" Text="生成报表" runat="server" Icon="ApplicationGo" OnClick="btnCreate_OnClick" ConfirmText="您确定生成页面文件吗？" Hidden="true">
                            </f:Button>
                        </Items>
                    </f:Toolbar>
                </Toolbars>
                <Items>
                    <f:Grid ID="Grid1" Title="Grid1" PageSize="20" ShowBorder="false" ShowHeader="False" SortDirection="ASC" AllowPaging="true" runat="server" EnableCheckBoxSelect="false" DataKeyNames="RowGuid" IsDatabasePaging="true" OnPageIndexChange="Grid1_PageIndexChange" EnableTextSelection="true" RowVerticalAlign="Middle">
                        <Columns><f:RowNumberField></f:RowNumberField>
                            <f:TemplateField ColumnID="ShowTableName" SortField="ShowTableName" ExpandUnusedSpace="true" TextAlign="Center" HeaderText="报表名称">
                                <ItemTemplate>
                                    <div style="text-align: left;">
                                        <%# DataBinder.Eval(Container.DataItem, "TableName").ToString()%></div>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField TextAlign="Center" Width="60" HeaderText="是否上报">
                                <ItemTemplate>
                                    <div style="text-align: center; padding-top: 2px;">
                                        <%# DataBinder.Eval(Container.DataItem, "[ApplyStatus]").ToString()=="0"?"<font color='red'>否</font>":"是"%>
                                    </div>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField TextAlign="Center" Width="60" HeaderText="上报">
                                <ItemTemplate>
                                    <%# getApplyUrl(DataBinder.Eval(Container.DataItem, "[TableName]").ToString(), DataBinder.Eval(Container.DataItem, "[TableRowGuid]").ToString(), DataBinder.Eval(Container.DataItem, "[RowGuid]").ToString(), DataBinder.Eval(Container.DataItem, "[ApplyStatus]").ToString())%>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField TextAlign="Center" Width="60" HeaderText="预览" Hidden="true">
                                <ItemTemplate>
                                    <div style="text-align: center; padding-top: 2px;">
                                        <a href="<%# GetViewUrl(DataBinder.Eval(Container.DataItem, "[TableName]"),DataBinder.Eval(Container.DataItem, "[RowGuid]"))%>">
                                            <img src="../../images/icons/zoom.png"></a>
                                    </div>
                                </ItemTemplate>
                            </f:TemplateField>
                        </Columns>
                    </f:Grid>
                </Items>
            </f:Panel>
        </Items>
    </f:Panel>
    <f:Window ID="Window1" Title="Edit" EnableIFrame="true" runat="server" EnableCollapse="true" EnableDrag="true" IFrameUrl="about:blank" Target="Parent" EnableMaximize="true" AutoScroll="true" Width="800px" Height="500px" CloseAction="HidePostBack" OnClose="Window1_OnClose" Hidden="true">
    </f:Window>
    <f:Window ID="Window2" Title="Edit" EnableIFrame="true" runat="server" EnableCollapse="true" EnableDrag="true" IFrameUrl="about:blank" Target="Parent" EnableMaximize="true" AutoScroll="true" Width="1350px" Height="500px" CloseAction="HidePostBack" Hidden="true">
    </f:Window>
    </form>
</body>
</html>
