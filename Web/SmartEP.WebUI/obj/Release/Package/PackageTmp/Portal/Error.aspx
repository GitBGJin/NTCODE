<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="SmartEP.WebUI.Portal.Error" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <script type="text/javascript">
            var PortalUrl = "<%= System.Configuration.ConfigurationManager.AppSettings["PortalUrl"].ToString()+System.Configuration.ConfigurationManager.AppSettings["PortalName"].ToString()%>";
            PortalUrl = PortalUrl + "/Portal";
            var url = PortalUrl + "/404.aspx";
            var errorCode = "<%=Request.QueryString["ErrorCode"]%>";
            switch (errorCode) {
                case "404":
                    url = PortalUrl + "/404.aspx";
                    break;
            }
            window.top.location = url;
        </script>
    </form>
</body>
</html>
