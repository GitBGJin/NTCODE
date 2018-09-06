<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadReport.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Report.UploadReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script language="javascript" type="text/javascript">
        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow)
                oWindow = window.RadWindow; //Will work in Moz in all cases, including clasic dialog
            else if (window.frameElement.radWindow)
                oWindow = window.frameElement.radWindow; //IE (and Moz as well)
            return oWindow;
        }
        function RefreshParent() {

            this.parent.Refresh_Grid(true);
            window.close();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <%--<telerik:RadScriptManager ID="RadScriptManager1" runat="server" />--%>
        <%--  <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1"
            UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="btnUpload">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="tbUp" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="fileuolp" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>--%>
        <table id="tbUp">
            <tr>
                <td>
                    <asp:ImageButton ID="btnMonthReport" OnClick="btnMonthReport_Click" runat="server" CssClass="RadToolBar_Customer" SkinID="ImgBtnUpload" />
                    <asp:ImageButton ID="brnCancel" runat="server" OnClientClick="RefreshParent();return false;" CssClass="RadToolBar_Customer" SkinID="ImgBtnCancel" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:FileUpload ID="fileuolp" runat="server" />

                </td>
                <%--  <td>
                    <asp:Button ID="btnUpload" runat="server" Text="上传" OnClick="btnUpload_Click" />
                </td>
                <td>
                    <asp:Button ID="btnFinish" runat="server" Text="取消" OnClientClick="RefreshParent()" /></td>--%>
            </tr>
        </table>
    </form>
</body>
</html>
