<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectData_Unique.aspx.cs" Inherits="Com.Sinoyd.Mis.WebUI.TableInfo.SelectData_Unique" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>选择用户</title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Panel1" />
        <f:Panel ID="Panel1" runat="server" Layout="Fit" ShowBorder="False" ShowHeader="false"
            BodyPadding="5px">
            <Toolbars>
                <f:Toolbar ID="Toolbar2" runat="server">
                    <Items>
                        <f:Button ID="Button1" Text="确定" runat="server" Icon="Accept" OnClick="Save_Click">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:Tree ID="Tree1" EnableArrows="true" ShowBorder="false" ShowHeader="false"
                    runat="server" Title="" Height="400">
                    <Nodes>
                    </Nodes>
                </f:Tree>
                <f:TextBox runat="server" ID="hideNodeList" Hidden="true"></f:TextBox>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
