<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListFieldAll.aspx.cs" Inherits="Com.Sinoyd.Mis.WebUI.TableInfo.ListFieldAll" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" runat="server" />
    <f:TabStrip ID="TabStrip1" Width="1188px" Height="440px" ShowBorder="true" ActiveTabIndex="0"
        runat="server" BodyPadding="5">
        <Tabs>
            <f:Tab ID="Tab1" Title="基本信息" runat="server" EnableIFrame="true">
            </f:Tab>
            <f:Tab ID="Tab2" Title="列表、搜索" EnableIFrame="true" runat="server">
            </f:Tab>
            <f:Tab ID="Tab3" EnableIFrame="true" Title="表单信息" runat="server">
            </f:Tab>
        </Tabs>
    </f:TabStrip>
    </form>
</body>
</html>
