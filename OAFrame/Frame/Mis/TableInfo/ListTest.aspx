<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListTest.aspx.cs" Inherits="TK.Mis.Web.TableInfo.ListTest" %>

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
                        <td>&nbsp;&nbsp;显示表名：</td>
                        <td Width="125">
                            <f:TextBox ID="txt_ShowTableName" runat="server" Width="120">
                            </f:TextBox>
                        </td>
                        <td>
                            &nbsp;&nbsp;SQL表名：
                        </td>
                        <td Width="125">
                            <f:TextBox ID="txt_SQLTableName" runat="server" Width="120">
                            </f:TextBox>
                        </td>
                        <td>
                            &nbsp;&nbsp;表类型：
                        </td>
                        <td Width="125">
                            <f:DropDownList runat="server" ID="ddl_TableType" Width="120">
                                <f:ListItem Selected="true" Text="所有类型" Value="所有类型"></f:ListItem>
                                <f:ListItem Text="表" Value="1"></f:ListItem>
                                <f:ListItem Text="视图" Value="2"></f:ListItem>
                            </f:DropDownList>                            
                        </td>
                        <td>
                            <f:Button ID="btnSeach" Text="查找" runat="server" Icon="Magnifier" OnClick="btnSearch_Click">
                            </f:Button></td>
                    </tr> 
            </table>
            </f:ContentPanel>
            <f:Panel ID="Panel2" ShowBorder="True" ShowHeader="false" runat="server" AnchorValue="100% -36" Layout="Anchor" AutoScroll="true">
                <Toolbars>
                    <f:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <f:Button ID="btnNew" Text="添加" runat="server" Icon="Add" EnablePostBack="false">
                            </f:Button>
                            <f:Button ID="btnDel" Text="删除" runat="server" Icon="Cancel" EnablePostBack="true" OnClick="btnDel_OnClick" ConfirmText="您确定删除选中的客户吗？">
                            </f:Button>
                            <f:Button ID="btnImport" Text="数据导入" runat="server" Icon="DatabaseAdd" Hidden="true">
                            </f:Button>
                        </Items>
                    </f:Toolbar>
                </Toolbars>
                <Items>
                    <f:Grid ID="Grid1" Title="Grid1" PageSize="20" ShowBorder="false" ShowHeader="False" SortDirection="ASC" AllowPaging="true" runat="server" EnableCheckBoxSelect="true" DataKeyNames="RowGuid" IsDatabasePaging="true" OnSort="Grid1_Sort" OnPageIndexChange="Grid1_PageIndexChange">
                        <Columns><f:RowNumberField></f:RowNumberField>
                            <f:TemplateField ColumnID="ShowTableName" SortField="ShowTableName" Width="180px" HeaderText="<div style='text-align:center;'>显示表名&nbsp;</div>">
                                <ItemTemplate>
                                    <div style="text-align: left; padding-top: 2px;">
                                        <%# DataBinder.Eval(Container.DataItem, "ShowTableName").ToString()%></div>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="180px" HeaderText="<div style='text-align:center;'>SQL表名&nbsp;</div>">
                                <ItemTemplate>
                                    <div style="text-align: left; padding-top: 2px;">
                                        <%# DataBinder.Eval(Container.DataItem, "SQLTableName").ToString()%></div>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="80px" HeaderText="<div style='text-align:center;'>表类型&nbsp;</div>">
                                <ItemTemplate>
                                    <div style="text-align: center; padding-top: 2px;">
                                        <%# DataBinder.Eval(Container.DataItem, "TableType").ToString()=="1"?"表":"视图"%></div>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField ExpandUnusedSpace="true" HeaderText="<div style='text-align:center;'>排序sql&nbsp;</div>">
                                <ItemTemplate>
                                    <div style="text-align: left; padding-top: 2px;">
                                        <%# DataBinder.Eval(Container.DataItem, "SortSql").ToString()%></div>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField HeaderText="<div style='text-align:center;'>编辑&nbsp;</div>" Width="60">
                                <ItemTemplate>
                                    <div style="text-align: center; padding-top: 2px;">
                                        <a href="<%# GetEditUrl(DataBinder.Eval(Container.DataItem, "[ShowTableName]"), DataBinder.Eval(Container.DataItem, "[RowGuid]"))%>">
                                            <img alt="编辑" src="../../images/icons/page_edit.png"></a>
                                    </div>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField HeaderText="<div style='text-align:center;'>字段设置&nbsp;</div>" Width="60">
                                <ItemTemplate>
                                    <div style="text-align: center; padding-top: 2px;">
                                        <a href="<%# GetEditUrl2(DataBinder.Eval(Container.DataItem, "[ShowTableName]"), DataBinder.Eval(Container.DataItem, "[RowGuid]"))%>">
                                            <img alt="字段设置" src="../../images/icons/page_edit.png"></a>
                                    </div>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField HeaderText="<div style='text-align:center;'>参数设置&nbsp;</div>" Width="60">
                                <ItemTemplate>
                                    <div style="text-align: center; padding-top: 2px;">
                                        <a href="<%# GetEditUrl3(DataBinder.Eval(Container.DataItem, "[ShowTableName]"), DataBinder.Eval(Container.DataItem, "[RowGuid]"))%>">
                                            <img alt="参数设置" src="../../images/icons/page_edit.png"></a>
                                    </div>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField HeaderText="<div style='text-align:center;'>唯一性设置&nbsp;</div>" Width="80">
                                <ItemTemplate>
                                    <div style="text-align: center; padding-top: 2px;">
                                        <a href="<%# GetEditUrl4(DataBinder.Eval(Container.DataItem, "[ShowTableName]"), DataBinder.Eval(Container.DataItem, "[RowGuid]"))%>">
                                            <img alt="唯一性设置" src="../../images/icons/page_edit.png"></a>
                                    </div>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField HeaderText="<div style='text-align:center;'>预览&nbsp;</div>" Width="60">
                                <ItemTemplate>
                                    <div style="text-align: center; padding-top: 2px;">
                                        <a href="<%# GetViewUrl(DataBinder.Eval(Container.DataItem, "[ShowTableName]"),DataBinder.Eval(Container.DataItem, "[RowGuid]"))%>">
                                            <img src="../../images/icons/zoom.png"></a>
                                    </div>
                                </ItemTemplate>
                            </f:TemplateField>
                        </Columns>
                    </f:Grid>
                    <f:Grid ID="Grid2" Title="Grid1" PageSize="20" ShowBorder="false" ShowHeader="False" SortDirection="ASC" AllowPaging="true" runat="server" EnableCheckBoxSelect="true" DataKeyNames="RowGuid" IsDatabasePaging="true" OnSort="Grid1_Sort" OnPageIndexChange="Grid1_PageIndexChange">
                        <Columns><f:RowNumberField></f:RowNumberField>
                            <f:TemplateField ColumnID="ShowTableName" SortField="ShowTableName" Width="180px" HeaderText="<div style='text-align:center;'>显示表名&nbsp;</div>">
                                <ItemTemplate>
                                    <div style="text-align: left; padding-top: 2px;">
                                        <%# DataBinder.Eval(Container.DataItem, "ShowTableName").ToString()%></div>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="180px" HeaderText="<div style='text-align:center;'>SQL表名&nbsp;</div>">
                                <ItemTemplate>
                                    <div style="text-align: left; padding-top: 2px;">
                                        <%# DataBinder.Eval(Container.DataItem, "SQLTableName").ToString()%></div>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="80px" HeaderText="<div style='text-align:center;'>表类型&nbsp;</div>">
                                <ItemTemplate>
                                    <div style="text-align: center; padding-top: 2px;">
                                        <%# DataBinder.Eval(Container.DataItem, "TableType").ToString()=="1"?"表":"视图"%></div>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField ExpandUnusedSpace="true" HeaderText="<div style='text-align:center;'>排序sql&nbsp;</div>">
                                <ItemTemplate>
                                    <div style="text-align: left; padding-top: 2px;">
                                        <%# DataBinder.Eval(Container.DataItem, "SortSql").ToString()%></div>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField HeaderText="<div style='text-align:center;'>编辑&nbsp;</div>" Width="60">
                                <ItemTemplate>
                                    <div style="text-align: center; padding-top: 2px;">
                                        <a href="<%# GetEditUrl(DataBinder.Eval(Container.DataItem, "[ShowTableName]"), DataBinder.Eval(Container.DataItem, "[RowGuid]"))%>">
                                            <img alt="编辑" src="../../images/icons/page_edit.png"></a>
                                    </div>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField HeaderText="<div style='text-align:center;'>字段设置&nbsp;</div>" Width="60">
                                <ItemTemplate>
                                    <div style="text-align: center; padding-top: 2px;">
                                        <a href="<%# GetEditUrl2(DataBinder.Eval(Container.DataItem, "[ShowTableName]"), DataBinder.Eval(Container.DataItem, "[RowGuid]"))%>">
                                            <img alt="字段设置" src="../../images/icons/page_edit.png"></a>
                                    </div>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField HeaderText="<div style='text-align:center;'>参数设置&nbsp;</div>" Width="60">
                                <ItemTemplate>
                                    <div style="text-align: center; padding-top: 2px;">
                                        <a href="<%# GetEditUrl3(DataBinder.Eval(Container.DataItem, "[ShowTableName]"), DataBinder.Eval(Container.DataItem, "[RowGuid]"))%>">
                                            <img alt="参数设置" src="../../images/icons/page_edit.png"></a>
                                    </div>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField HeaderText="<div style='text-align:center;'>唯一性设置&nbsp;</div>" Width="80">
                                <ItemTemplate>
                                    <div style="text-align: center; padding-top: 2px;">
                                        <a href="<%# GetEditUrl4(DataBinder.Eval(Container.DataItem, "[ShowTableName]"), DataBinder.Eval(Container.DataItem, "[RowGuid]"))%>">
                                            <img alt="唯一性设置" src="../../images/icons/page_edit.png"></a>
                                    </div>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField HeaderText="<div style='text-align:center;'>预览&nbsp;</div>" Width="60">
                                <ItemTemplate>
                                    <div style="text-align: center; padding-top: 2px;">
                                        <a href="<%# GetViewUrl(DataBinder.Eval(Container.DataItem, "[ShowTableName]"),DataBinder.Eval(Container.DataItem, "[RowGuid]"))%>">
                                            <img src="../../images/icons/zoom.png"></a>
                                    </div>
                                </ItemTemplate>
                            </f:TemplateField>
                        </Columns>
                    </f:Grid>
                    <f:Grid ID="Grid3" Title="Grid1" PageSize="20" ShowBorder="false" ShowHeader="False" SortDirection="ASC" AllowPaging="true" runat="server" EnableCheckBoxSelect="true" DataKeyNames="RowGuid" IsDatabasePaging="true" OnSort="Grid1_Sort" OnPageIndexChange="Grid1_PageIndexChange">
                        <Columns><f:RowNumberField></f:RowNumberField>
                            <f:TemplateField ColumnID="ShowTableName" SortField="ShowTableName" Width="180px" HeaderText="<div style='text-align:center;'>显示表名&nbsp;</div>">
                                <ItemTemplate>
                                    <div style="text-align: left; padding-top: 2px;">
                                        <%# DataBinder.Eval(Container.DataItem, "ShowTableName").ToString()%></div>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="180px" HeaderText="<div style='text-align:center;'>SQL表名&nbsp;</div>">
                                <ItemTemplate>
                                    <div style="text-align: left; padding-top: 2px;">
                                        <%# DataBinder.Eval(Container.DataItem, "SQLTableName").ToString()%></div>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField Width="80px" HeaderText="<div style='text-align:center;'>表类型&nbsp;</div>">
                                <ItemTemplate>
                                    <div style="text-align: center; padding-top: 2px;">
                                        <%# DataBinder.Eval(Container.DataItem, "TableType").ToString()=="1"?"表":"视图"%></div>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField ExpandUnusedSpace="true" HeaderText="<div style='text-align:center;'>排序sql&nbsp;</div>">
                                <ItemTemplate>
                                    <div style="text-align: left; padding-top: 2px;">
                                        <%# DataBinder.Eval(Container.DataItem, "SortSql").ToString()%></div>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField HeaderText="<div style='text-align:center;'>编辑&nbsp;</div>" Width="60">
                                <ItemTemplate>
                                    <div style="text-align: center; padding-top: 2px;">
                                        <a href="<%# GetEditUrl(DataBinder.Eval(Container.DataItem, "[ShowTableName]"), DataBinder.Eval(Container.DataItem, "[RowGuid]"))%>">
                                            <img alt="编辑" src="../../images/icons/page_edit.png"></a>
                                    </div>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField HeaderText="<div style='text-align:center;'>字段设置&nbsp;</div>" Width="60">
                                <ItemTemplate>
                                    <div style="text-align: center; padding-top: 2px;">
                                        <a href="<%# GetEditUrl2(DataBinder.Eval(Container.DataItem, "[ShowTableName]"), DataBinder.Eval(Container.DataItem, "[RowGuid]"))%>">
                                            <img alt="字段设置" src="../../images/icons/page_edit.png"></a>
                                    </div>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField HeaderText="<div style='text-align:center;'>参数设置&nbsp;</div>" Width="60">
                                <ItemTemplate>
                                    <div style="text-align: center; padding-top: 2px;">
                                        <a href="<%# GetEditUrl3(DataBinder.Eval(Container.DataItem, "[ShowTableName]"), DataBinder.Eval(Container.DataItem, "[RowGuid]"))%>">
                                            <img alt="参数设置" src="../../images/icons/page_edit.png"></a>
                                    </div>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField HeaderText="<div style='text-align:center;'>唯一性设置&nbsp;</div>" Width="80">
                                <ItemTemplate>
                                    <div style="text-align: center; padding-top: 2px;">
                                        <a href="<%# GetEditUrl4(DataBinder.Eval(Container.DataItem, "[ShowTableName]"), DataBinder.Eval(Container.DataItem, "[RowGuid]"))%>">
                                            <img alt="唯一性设置" src="../../images/icons/page_edit.png"></a>
                                    </div>
                                </ItemTemplate>
                            </f:TemplateField>
                            <f:TemplateField HeaderText="<div style='text-align:center;'>预览&nbsp;</div>" Width="60">
                                <ItemTemplate>
                                    <div style="text-align: center; padding-top: 2px;">
                                        <a href="<%# GetViewUrl(DataBinder.Eval(Container.DataItem, "[ShowTableName]"),DataBinder.Eval(Container.DataItem, "[RowGuid]"))%>">
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
    <f:Window ID="Window1" Title="Edit" EnableIFrame="true" runat="server" EnableCollapse="true" EnableDrag="true" IFrameUrl="about:blank" Target="Parent" EnableMaximize="true" AutoScroll="true" Width="620px" Height="400px" CloseAction="HidePostBack" OnClose="Window1_OnClose" Hidden="true">
    </f:Window>
    <f:Window ID="Window2" Title="Edit" EnableIFrame="true" runat="server" EnableCollapse="true" EnableDrag="true" IFrameUrl="about:blank" Target="Parent" EnableMaximize="true" AutoScroll="true" Width="1350px" Height="500px" CloseAction="HidePostBack" Hidden="true">
    </f:Window>
    </form>
</body>
</html>
