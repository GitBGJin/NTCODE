<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LeftTreeLZ.aspx.cs" Inherits="SmartEP.WebUI.Portal.LeftTreeLZ" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>左侧菜单处理</title>
    <link id="LeftTreeTheme" href="../App_Themes/Neptune/LeftTree.css" rel="stylesheet" />
    <script type="text/javascript" src="../Resources/JavaScript/JQuery/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="../Resources/JavaScript/JQuery/jquery.cookie.js"></script>
    <%--<link href="App_Themes/Fresh/LeftTree.css" rel="stylesheet" />--%>
    <%--<link href="App_Themes/Feminine/LeftTree.css" rel="stylesheet" />--%>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
            <script type="text/javascript">
                function OnClientItemClicking(sender, args) {
                    var NavigateUrl = args.get_item().get_value();
                    if (NavigateUrl == null || NavigateUrl == "#")
                        return;
                    var values = NavigateUrl.split(';')
                    var title = args.get_item().get_text();
                    if (values.length == 2 && values[1] != null && values[1] != "") {
                        var moduleGuid = "LeftTree" + title;
                        parent.showwindows(values[1], values[0], title);
                    }
                }

                window.onload = function () {
                    cssChange();
                }

                function cssChange() {
                    var titlef = parent.document.getElementById("frame1").title;
                    var href = "../App_Themes/" + titlef + "/LeftTree.css";
                    if (href != document.getElementById("LeftTreeTheme").href) {
                        document.getElementById("LeftTreeTheme").href = href;
                    }
                    //setTimeout(cssChange, 500);
                }
            </script>
        </telerik:RadScriptBlock>
        <telerik:RadPanelBar ID="RadPanelBar1" Width="100%" Height="100%" ExpandMode="SingleExpandedItem" runat="server"
            Skin="Silk" OnClientItemClicking="OnClientItemClicking">
        </telerik:RadPanelBar>
        <label title="" style="display: none;" id="lbl_title">title</label>
    </form>
</body>
</html>

