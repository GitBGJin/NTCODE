<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListApply_Circle.aspx.cs" Inherits="TK.Mis.Web.TableInfoSet.ListApply_Circle" %>

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
                            <f:TemplateField TextAlign="Center" Width="60" HeaderText="周期申报">
                                <ItemTemplate>
                                    <div style="text-align: center; padding-top: 2px;">
                                        <a href="<%# GetViewUrl(DataBinder.Eval(Container.DataItem, "[TableName]"),DataBinder.Eval(Container.DataItem, "[RowGuid]"))%>">
                                            <img src="../../images/icons/arrow_right.png"></a>
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
