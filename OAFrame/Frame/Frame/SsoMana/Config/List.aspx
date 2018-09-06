<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Com.Sinoyd.Frame.WebUI.SsoMana.Config.List" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>站点设置</title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1"
            runat="server"></f:PageManager>
        <f:Panel ID="Panel1" ShowBorder="false" ShowHeader="false" runat="server" AnchorValue="100% -36"
            Layout="Fit">
            <Items>
                <f:Grid ID="Grid1" Title="Grid1" ShowBorder="false" ShowHeader="false"
                    PageSize="10" runat="server" EnableCheckBoxSelect="false" DataKeyNames="RowGuid"
                    AllowPaging="true" EnableHeaderMenu="false" OnPageIndexChange="Grid1_PageIndexChange" IsDatabasePaging="true">
                    <Columns>
                        <f:TemplateField HeaderText="配置项名称" Width="200" TextAlign="Center">
                            <ItemTemplate>
                                <div style="text-align: left;">
                                    <%# DataBinder.Eval(Container.DataItem, "ConfigName").ToString()%>
                                </div>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:TemplateField HeaderText="配置项值" Width="200" TextAlign="Center">
                            <ItemTemplate>
                                <div style="text-align: left;">
                                    <%# DataBinder.Eval(Container.DataItem, "ConfigValue").ToString()%>
                                </div>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:TemplateField HeaderText="备注" TextAlign="Center" ExpandUnusedSpace="true">
                            <ItemTemplate>
                                <div style="text-align: left;">
                                    <%# DataBinder.Eval(Container.DataItem, "Note").ToString()%>
                                </div>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:TemplateField HeaderText="操作" Width="80" TextAlign="Center">
                            <ItemTemplate>
                                <a href="<%# GetEditUrl(DataBinder.Eval(Container.DataItem, "[RowGuid]")) %>">
                                    <img src="../../Content/icon/page_edit.png" /></a>
                            </ItemTemplate>
                        </f:TemplateField>
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>
        <f:Window ID="Window1" Title="Edit" EnableIFrame="true" runat="server"
            EnableConfirmOnClose="true" IFrameUrl="about:blank" Target="Self" IsModal="true"
            Width="500px" Height="300px" Hidden="true" OnClose="Window1_Close" EnableDrag="false">
        </f:Window>
    </form>
</body>
</html>
