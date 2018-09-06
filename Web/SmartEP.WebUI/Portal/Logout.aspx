<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Logout.aspx.cs" Inherits="SmartEP.WebUI.Portal.Logout" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <script type="text/javascript">
            var SsoLoginUrl = "<%= System.Configuration.ConfigurationManager.AppSettings["SsoLoginUrl"].ToString()%>";
            window.top.location = SsoLoginUrl;
        </script>
    </form>
</body>
</html>
