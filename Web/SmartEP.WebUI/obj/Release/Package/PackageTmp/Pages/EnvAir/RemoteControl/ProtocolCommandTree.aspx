<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="ProtocolCommandTree.aspx.cs"
    Inherits="SmartEP.WebUI.Pages.EnvAir.RemoteControl.ProtocolCommandTree" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
       
        function onNodeChecking(sender, args) {

           
            //call a method from the parent page
            var radPaneObject = window.parent;

            var treeNode = args.get_node();
            var parentNode = treeNode.get_parent();
            var contentUrl = "RemoteControl2.aspx?cn=" + treeNode.get_value() + '&cmdDesc=' + treeNode.get_text() + '&timestamp=' + (new Date()).getTime();
            
            ////控制RadSplitbar2 隐藏/显示
            //radPaneObject.GetRadBottomPane();

            radPaneObject.Test();

            radPaneObject.RefreshRemoteControl(contentUrl)
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%;">
        <tr>
            <td valign="top">
                <telerik:RadScriptManager ID="ScriptManager" runat="server" />
                <telerik:RadTreeView ID="RadTreeView1" runat="server" OnClientNodeClicking="onNodeChecking">
                </telerik:RadTreeView>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>

