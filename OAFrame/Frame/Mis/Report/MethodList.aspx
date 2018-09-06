<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MethodList.aspx.cs" Inherits="Com.Sinoyd.Mis.WebUI.Report.MethodList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>方法列表</title>
</head>
<body>
    <form id="form1" runat="server">
    <script type="text/javascript"> 
    </script>
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server"></f:PageManager>
    <f:Panel ID="Panel1" runat="server" BodyPadding="3px" ShowBorder="false" ShowHeader="false"
        Height="300px" Layout="Anchor">
        <Items>
            <f:Panel ID="Panel2" ShowBorder="True" ShowHeader="false" runat="server" AnchorValue="100% -1"
                Layout="Fit">
                <Toolbars>
                    <f:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <f:Button ID="btnNew" Text="添加" runat="server" Icon="Add" EnablePostBack="false">
                            </f:Button>
                            <f:Button ID="btnDel" Text="删除" runat="server" Icon="Cancel" EnablePostBack="true"
                                OnClick="btnDel_OnClick" ConfirmText="您确定删除选中的方法吗？">
                            </f:Button>
                        </Items>
                    </f:Toolbar>
                </Toolbars>
                <Items>
                    <f:Grid ID="Grid1" Title="Grid1" PageSize="200" ShowBorder="false" ShowHeader="False"
                        SortDirection="ASC" AllowPaging="false" runat="server" EnableCheckBoxSelect="true"
                        DataKeyNames="RowGuid" IsDatabasePaging="true" OnPageIndexChange="Grid1_PageIndexChange"
                        EnableTextSelection="true" EnableHeaderMenu="false">
                        <Columns>
                            <f:RowNumberField></f:RowNumberField>
                            <f:TemplateField ColumnID="Title" SortField="Title" Width="200" HeaderText="方法名称"
                                TextAlign="Center">
                                <ItemTemplate>
                                    <div style="text-align: left; padding-top: 2px;">
                                        <%# DataBinder.Eval(Container.DataItem, "MethodName").ToString()%></div>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="250px" HeaderText="方法路径" TextAlign="Center">
                                <ItemTemplate>
                                    <div style="text-align: left; padding-top: 2px;">
                                        <%# DataBinder.Eval(Container.DataItem, "MethodPath").ToString()%></div>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField ExpandUnusedSpace="true" HeaderText="备注" TextAlign="Center">
                                <ItemTemplate>
                                    <div style="text-align: left; padding-top: 2px;">
                                        <%# DataBinder.Eval(Container.DataItem, "Note").ToString()%></div>
                                </ItemTemplate>
                            </f:TemplateField>
<%--                            <f:TemplateField HeaderText="操作" Width="70" TextAlign="Center">
                                <ItemTemplate>
                                    <div style="text-align: center; padding-top: 2px;">
                                        <a href="<%# GetEditUrl(DataBinder.Eval(Container.DataItem, "[MethodName]"), DataBinder.Eval(Container.DataItem, "[RowGuid]"))%>">
                                            <img alt="编辑" src="../images/icons/page_edit.png"></a>
                                    </div>
                                </ItemTemplate>
                            </f:TemplateField>--%>
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
        AutoScroll="true" Width="1000px" Height="500px" CloseAction="HidePostBack" Hidden="true">
    </f:Window>
    </form>
</body>
</html>
