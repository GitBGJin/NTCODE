<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Com.Sinoyd.Mis.WebUI.Report.List" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>报表管理</title>
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
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" Theme="Neptune">
    </f:PageManager>
    <f:Panel ID="Panel1" runat="server" BodyPadding="3px" ShowBorder="false" ShowHeader="false"
        Height="300px" Layout="Anchor">
        <Items>
            <f:ContentPanel ID="ContentPanel1" runat="server" BodyPadding="5px" ShowBorder="false"
                ShowHeader="false" Title="ContentPanel">
                <table>
                    <tr>
                        <td>
                            &nbsp;&nbsp;<a href="#" style="cursor:pointer;color:#3558DB;text-decoration:none;">标题：</a>
                        </td>
                        <td width="225">
                            <f:TextBox ID="txt_Title" runat="server" Width="220">
                            </f:TextBox>
                        </td>
                        <td>
                            <f:Button ID="btnSeach" Text="查询" runat="server" Icon="Magnifier" OnClick="btnSearch_Click">
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
                            <f:Button ID="btnNew1" Text="添加(简易表)" runat="server" Icon="Add" EnablePostBack="false">
                            </f:Button>
                            <f:Button ID="btnNew2" Text="添加(固定表)" runat="server" Icon="Add" EnablePostBack="false">
                            </f:Button>
                            <f:Button ID="btnNew3" Text="添加(分组报表)" runat="server" Icon="Add" EnablePostBack="false">
                            </f:Button>
                            <f:Button ID="btnNew4" Text="添加(交叉统计表)" runat="server" Icon="Add" EnablePostBack="false">
                            </f:Button>
                            <f:Button ID="btnDel" Text="删除" runat="server" Icon="Cancel" EnablePostBack="true"
                                OnClick="btnDel_OnClick" ConfirmText="您确定删除选中的记录吗？">
                            </f:Button>
                        </Items>
                    </f:Toolbar>
                </Toolbars>
                <Items>
                    <f:Grid ID="Grid1" Title="Grid1" PageSize="20" ShowBorder="false" ShowHeader="False"
                        SortDirection="ASC" AllowPaging="true" runat="server" EnableCheckBoxSelect="true"
                        DataKeyNames="RowGuid" IsDatabasePaging="true" OnPageIndexChange="Grid1_PageIndexChange"
                        EnableTextSelection="true" EnableHeaderMenu="false">
                        <Columns>
                            <f:RowNumberField ID="NumberID"></f:RowNumberField>
                            <%--<f:BoundField ColumnID="Title" HeaderText="标题" DataField="Title" />
                            <f:BoundField ColumnID="ReportType" HeaderText="报表类型" DataField="ReportType" />--%>
                            <f:TemplateField ColumnID="Title" SortField="Title" ExpandUnusedSpace="true" HeaderText="标题"
                                TextAlign="Center">
                                <ItemTemplate>
                                    <div style="text-align: left; padding-top: 2px;">
                                        <%# DataBinder.Eval(Container.DataItem, "Title").ToString()%></div>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="100px" HeaderText="报表类型" TextAlign="Center" ColumnID="ReportType">
                                <ItemTemplate>
                                    <div style="text-align: center; padding-top: 2px;">
                                        <%# GetReportTypeName(DataBinder.Eval(Container.DataItem, "ReportType").ToString())%></div>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField HeaderText="搜索设置" Width="80" TextAlign="Center">
                                <ItemTemplate>
                                    <div style="text-align: center; padding-top: 2px;">
                                        <a href="<%# GetEditUrl3(DataBinder.Eval(Container.DataItem, "[Title]"), DataBinder.Eval(Container.DataItem, "[RowGuid]"))%>">
                                            <img alt="搜索设置" src="../images/icons/page_edit.png"></a>
                                    </div>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField HeaderText="列设置" Width="80" TextAlign="Center">
                                <ItemTemplate>
                                    <div style="text-align: center; padding-top: 2px;">
                                        <a href="<%# GetEditUrl2(DataBinder.Eval(Container.DataItem, "ReportType"),DataBinder.Eval(Container.DataItem, "[Title]"), DataBinder.Eval(Container.DataItem, "[RowGuid]"))%>">
                                            <img alt="列设置" src="../images/icons/page_edit.png"></a>
                                    </div>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField HeaderText="方法注册" Width="80" TextAlign="Center">
                                <ItemTemplate>
                                    <div style="text-align: center; padding-top: 2px;">
                                        <a href="<%# GetEditUrl9(DataBinder.Eval(Container.DataItem, "[Title]"), DataBinder.Eval(Container.DataItem, "[RowGuid]"))%>">
                                            <img alt="方法注册" src="../images/icons/page_edit.png"></a>
                                    </div>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField HeaderText="预览" Width="80" TextAlign="Center">
                                <ItemTemplate>
                                    <div style="text-align: center; padding-top: 2px;height:20px;">
                                        <a target="_blank" href="<%# GetPreviewUrl(DataBinder.Eval(Container.DataItem, "[RowGuid]").ToString(),DataBinder.Eval(Container.DataItem, "[ReportType]").ToString())%>">
                                            <img src="../images/icons/zoom.png"></a>
                                    </div>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField HeaderText="操作" Width="70" TextAlign="Center">
                                <ItemTemplate>
                                    <div style="text-align: center; padding-top: 2px;">
                                        <a href="<%# GetEditUrl(DataBinder.Eval(Container.DataItem, "ReportType"),DataBinder.Eval(Container.DataItem, "[Title]"), DataBinder.Eval(Container.DataItem, "[RowGuid]"))%>">
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
        EnableDrag="false" IFrameUrl="about:blank" Target="Parent" EnableMaximize="true"
        AutoScroll="true" Width="700px" Height="550px" CloseAction="HidePostBack" OnClose="Window1_OnClose"
        Hidden="true">
    </f:Window>
    <f:Window ID="Window2" Title="Edit" EnableIFrame="true" runat="server" EnableCollapse="true"
        EnableDrag="false" IFrameUrl="about:blank" Target="Parent" EnableMaximize="true"
        AutoScroll="true" Width="1200px" Height="550px" CloseAction="HidePostBack" Hidden="true">
    </f:Window>
    </form>
</body>
</html>
