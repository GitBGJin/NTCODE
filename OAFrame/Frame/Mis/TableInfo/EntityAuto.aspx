<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EntityAuto.aspx.cs" Inherits="TK.Mis.Web.TableInfo.EntityAuto" %>

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
                            &nbsp;&nbsp;名称：
                        </td>
                        <td width="125">
                            <f:TextBox ID="txt_Name" runat="server" Width="120">
                            </f:TextBox>
                        </td>
                        <td>
                            &nbsp;&nbsp;类型：
                        </td>
                        <td width="125">
                            <f:DropDownList runat="server" ID="ddl_type" Width="80">
                                <f:ListItem Selected="true" Text="所有类型" Value="所有类型"></f:ListItem>
                                <f:ListItem Text="表" Value="U"></f:ListItem>
                                <f:ListItem Text="视图" Value="V"></f:ListItem>
                            </f:DropDownList>
                        </td>
                        <td>
                            <f:Button ID="btnSeach" Text="查找" runat="server" Icon="Magnifier" OnClick="btnSearch_Click">
                            </f:Button>
                        </td>
                    </tr>
                </table>
            </f:ContentPanel>
            <f:Panel ID="Panel2" ShowBorder="True" ShowHeader="false" runat="server" AnchorValue="100% -36" Layout="Fit">
                <Toolbars>
                    <f:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <f:Label ID="lblMmkj" runat="server" Text="&nbsp;&nbsp;命名空间：">
                            </f:Label>
                            <f:TextBox ID="txt_Mmkj" runat="server" Width="200" EmptyText="示例：Zjgnw.Web.Pages.Company">
                            </f:TextBox>
                            <f:Label ID="lblSclj" runat="server" Text="&nbsp;&nbsp;生成路径：">
                            </f:Label>
                            <f:TextBox ID="txt_Sclj" runat="server" Width="200" EmptyText="示例：Zjgnw/Web/Pages/Company">
                            </f:TextBox>
                            <f:Button ID="btnCreate" Text="生成" runat="server" Icon="ApplicationGo" OnClick="btnCreate_OnClick" ConfirmText="您确定生成页面文件吗？">
                            </f:Button>
                        </Items>
                    </f:Toolbar>
                </Toolbars>
                <Items>
                    <f:Grid ID="Grid1" Title="数据表(视图)" PageSize="500" ShowBorder="false" ShowHeader="False" SortDirection="ASC" AllowPaging="true" runat="server" EnableCheckBoxSelect="true" DataKeyNames="Name" IsDatabasePaging="true" EnableTextSelection="true">
                        <Columns><f:RowNumberField></f:RowNumberField>
                            <f:TemplateField ColumnID="ShowTableName" SortField="ShowTableName" ExpandUnusedSpace="true" HeaderText="<div style='text-align:center;'>名称&nbsp;</div>">
                                <ItemTemplate>
                                    <div style="text-align: left; padding-top: 2px;">
                                        <%# DataBinder.Eval(Container.DataItem, "Name").ToString()%></div>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="80" HeaderText="<div style='text-align:center;'>类型&nbsp;</div>">
                                <ItemTemplate>
                                    <div style="text-align: center; padding-top: 2px;">
                                        <%# DataBinder.Eval(Container.DataItem, "type").ToString().Trim().ToUpper() == "U" ? "表" : "视图"%></div>
                                </ItemTemplate>
                            </f:TemplateField>
                        </Columns>
                    </f:Grid>
                </Items>
            </f:Panel>
        </Items>
    </f:Panel>
    </form>
</body>
</html>
