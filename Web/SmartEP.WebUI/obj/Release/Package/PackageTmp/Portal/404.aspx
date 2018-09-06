﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="404.aspx.cs" Inherits="SmartEP.WebUI.Portal._404" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server" style="text-align: center; vertical-align: middle">
        <img src="../Resources/Images/login/404.PNG" alt="Planets" usemap="#planetmap" />
        <map name="planetmap">
            <area shape="rectangle" coords="430,225,530,255" href='<%= System.Configuration.ConfigurationManager.AppSettings["PortalUrl"].ToString()+System.Configuration.ConfigurationManager.AppSettings["PortalName"].ToString()%>/Portal/Login.aspx' alt="点击重新登录" />
        </map>
    </form>
</body>
</html>
