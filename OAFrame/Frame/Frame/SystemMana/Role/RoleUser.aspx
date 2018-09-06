<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoleUser.aspx.cs" ValidateRequest="false" Inherits="Com.Sinoyd.Frame.WebUI.SystemMana.Role.RoleUser" EnableEventValidation="false" ViewStateEncryptionMode="Never" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" Theme="Neptune" />
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region2" Width="200px"
                    Margins="3 3 3 3" ShowHeader="false" Title="角色" Icon="Outline" EnableCollapse="true"
                    ShowBorder="false" Layout="Fit" Position="Left" runat="server">
                    <Items>
                        <f:Tree ID="Tree1" OnNodeCheck="Tree1_NodeCheck" Width="250px" EnableFrame="true" EnableCollapse="true"
                            ShowHeader="false" Title="树控件" runat="server">
                        </f:Tree>
                        <f:TextBox runat="server" ID="hideNodeList" Hidden="true"></f:TextBox>
                    </Items>
                </f:Region>
                <f:Region ID="mainRegion" ShowHeader="false" Layout="Fit" Margins="3 3 3 3" Position="Center"
                    ShowBorder="false" runat="server">
                    <Toolbars>
                        <f:Toolbar runat="server">
                            <Items>
                                <f:Button runat="server" ID="btnOK" Text="确定" OnClick="btnSelect_Click" Icon="Accept"></f:Button>
                                <f:Button runat="server" ID="btnDelete" Text="删除" ConfirmText="您确定删除选定的人员吗？" OnClick="btnDelete_Click" Icon="BasketDelete">
                                </f:Button>
                                <f:Button runat="server" ID="btnDeleteAll" Text="全部删除" ConfirmText="您确定删除全部删除吗？" OnClick="btnDeleteAll_Click" Icon="BulletCross">
                                </f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Items>
                        <f:Grid ID="Grid1" Title="Grid1" ShowBorder="false" ShowHeader="false"
                            PageSize="200" runat="server" EnableCheckBoxSelect="true" DataKeyNames="RowGuid,UserGuid"
                            AllowPaging="false" EnableHeaderMenu="false">
                            <Columns>
                                <f:RowNumberField EnablePagingNumber="true" Width="30" TextAlign="Center" />
                                <f:TemplateField HeaderText="部门名称" Width="150" TextAlign="left">
                                    <ItemTemplate>
                                        <%# new Com.Sinoyd.Frame.BLL.B_FrameDepartment().SelectByRowGuid(new Com.Sinoyd.Frame.BLL.B_FrameUser().SelectByRowGuid(DataBinder.Eval(Container.DataItem, "UserGuid").ToString()).DeptGuid).DeptName %>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:TemplateField HeaderText="人员" ExpandUnusedSpace="true" TextAlign="left">
                                    <ItemTemplate>
                                        <%# new Com.Sinoyd.Frame.BLL.B_FrameUser().SelectByRowGuid(DataBinder.Eval(Container.DataItem, "UserGuid").ToString()).DisplayName %>
                                    </ItemTemplate>
                                </f:TemplateField>
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>



    </form>
</body>
</html>
