<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AuditModityData.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Audit.AuditModityData" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            function GetRadWindow() {
                var oWindow = null;
                if (window.radWindow)
                    oWindow = window.RadWindow; //Will work in Moz in all cases, including clasic dialog
                else if (window.frameElement.radWindow)
                    oWindow = window.frameElement.radWindow; //IE (and Moz as well)
                return oWindow;
            }
            function CloseMe(args) {
                try {
                    //args 是否提交数据
                    if (args) {
                        var reason = $find("<%=resonText.ClientID %>");
                        var data = $find("<%=NewDataTextBox.ClientID %>").get_value();
                        if (data == '<%= Request.QueryString["data"]%>') {
                            Cls(true);
                            return;
                        }
                        if (data == "") {
                            alert("请输入修改值！");
                            return;
                        } else {
                            parent.document.getElementById('auditReason').value = reason.get_value();
                            parent.ChartModifyAuditData("modify", data);
                            parent.document.getElementById('dataSubmit').click();
                        }
                    }
                    Cls(true);
                }
                catch (e) {
                    //alert(e.message);
                }
            }

            function Cls(args) {
                var oWindow = GetRadWindow();
                oWindow.Close();
            }

            function SubmitClientClicked(sender, arg) {
                var reason = $find("<%=resonText.ClientID %>");
        if (reason.get_value() == "")
            alert("必须填写理由！");
        else
            CloseMe(true);
    }
    function CancelClientClicked(sender, arg) {
        CloseMe(false);
    }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <div style="text-align: center;">
            <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
            <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1"
                UpdatePanelsRenderMode="Inline">
                <AjaxSettings>
                    <telerik:AjaxSetting AjaxControlID="ResonCombox">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="resonText" LoadingPanelID="RadAjaxLoadingPanel1" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManager>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="修改值"></asp:Label>
                    </td>
                    <td>
                        <telerik:RadNumericTextBox ID="NewDataTextBox" runat="server" MaxLength="6" NumberFormat-DecimalDigits="3"></telerik:RadNumericTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="选择原因"></asp:Label>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ResonCombox" runat="server" Width="370" AutoPostBack="true" OnSelectedIndexChanged="ResonCombox_SelectedIndexChanged"></telerik:RadComboBox>
                    </td>
                </tr>
            </table>
            <telerik:RadTextBox ID="resonText" runat="server" Resize="None" TextMode="MultiLine" Width="80%" Height="200"></telerik:RadTextBox>
            <table>
                <tr>
                    <td>
                        <telerik:RadButton ID="SubmitButton" runat="server" BackColor="#3A94D3" ForeColor="White" AutoPostBack="true" OnClientClicked="SubmitClientClicked" OnClick="SubmitButton_Click">
                            <ContentTemplate>
                                <asp:Label runat="server" ID="Label4" ForeColor="White" Text="提交"></asp:Label>
                            </ContentTemplate>
                        </telerik:RadButton>
                        <%--                        <telerik:RadButton ID="SubmitButton" runat="server" Text="提交" OnClientClicked="SubmitClientClicked"></telerik:RadButton>--%>
                    </td>
                    <td>
                        <telerik:RadButton ID="CancelButton" runat="server" BackColor="#3A94D3" ForeColor="White" AutoPostBack="true" OnClientClicked="CancelClientClicked">
                            <ContentTemplate>
                                <asp:Label runat="server" ID="Label4" ForeColor="White" Text="取消"></asp:Label>
                            </ContentTemplate>
                        </telerik:RadButton>
                        <%--                        <telerik:RadButton ID="CancelButton" runat="server" Text="取消" OnClientClicked="CancelClientClicked"></telerik:RadButton>--%>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>

