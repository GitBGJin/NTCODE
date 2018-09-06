﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AuditReason.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Audit.AuditReason" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
                debugger;
                try {
                    //args 是否提交数据
                    if (args) {
                        var reason = $find("<%=resonText.ClientID %>");
                        parent.document.getElementById('auditReason').value = reason.get_value();
                        if ('<%=Request.QueryString["operator"]%>' == "modify")//修改
                            parent.ModifyAuditData("modify");
                        else if ('<%=Request.QueryString["operator"]%>' == "restore")//恢复
                            parent.RestorAuditData();
                        else if ('<%=Request.QueryString["operator"]%>' == "restoreRow")//恢复
                            parent.RestorAuditDataRow();
                        else if ('<%=Request.QueryString["operator"]%>' == "654CB7E0-09E4-4C14-A2A3-46BAD96709A5")//时间段无效
                            parent.ModifyAuditDataForRow('<%=Request.QueryString["operator"]%>');
                        else if('<%=Request.QueryString["operator"]%>' == "9AF2FF12-3889-4391-9AF3-4596F5A652F5")//时间段有效
                            parent.ModifyAuditDataRow('<%=Request.QueryString["operator"]%>');
                        else //审核标记位操作
                            parent.ModifyAuditData('<%=Request.QueryString["operator"]%>');
                    parent.document.getElementById('dataSubmit').click();
                } else {
                    if ('<%=Request.QueryString["operator"]%>' == "modify")
                            parent.ModifyAuditData("-1");
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
