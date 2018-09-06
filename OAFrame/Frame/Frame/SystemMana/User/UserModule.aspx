<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserModule.aspx.cs" Inherits="Com.Sinoyd.Frame.WebUI.SystemMana.User.UserModule" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" Theme="Neptune" />
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region2" Width="380px"
                    Margins="3 3 3 3" ShowHeader="false" Title="人员" Icon="Outline" EnableCollapse="true"
                    ShowBorder="false" Layout="Fit" Position="Center" runat="server">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <f:Button runat="server" ID="btnOK" Text="确定" OnClick="btnSelect_Click" Icon="Accept"></f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Items>
                        <f:Tree ID="Tree1" Width="350px" EnableFrame="true" EnableCollapse="true"
                            ShowHeader="false" Title="树控件" runat="server" OnNodeCheck="Tree1_NodeCheck">
                        </f:Tree>
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>



    </form>
</body>
</html>
