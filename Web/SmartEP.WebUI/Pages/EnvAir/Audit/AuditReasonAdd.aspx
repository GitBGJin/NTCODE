<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AuditReasonAdd.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Audit.AuditReasonAdd" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            function RefreshParent() {
                this.parent.Refresh_Grid(true);
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <table id="maintable" cellspacing="1" width="100%" cellpadding="0" class="Table_Customer" border="0">
            <tr class="btnTitle">
                <td class="btnTitle" colspan="4">
                    <asp:ImageButton ID="btnAdd" SkinID="ImgBtnAdd" runat="server" OnClick="btnAdd_Click" />
                    <asp:ImageButton ID="btnSave" SkinID="ImgBtnSave" runat="server" OnClick="btnSave_Click" Visible="false" />
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <br />
                </td>
            </tr>
            <tr>
                <td class="title">审核理由：</td>
                <td class="content" colspan="3">
                    <telerik:RadTextBox ID="txtReasonContent" runat="server" Width="250px"
                        MaxLength="50">
                    </telerik:RadTextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*审核理由不能为空" ControlToValidate="txtReasonContent"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="title">排序：</td>
                <td class="content" colspan="3">
                    <telerik:RadNumericTextBox runat="server" ID="txtOrderByNum" MinValue="0" MaxValue="999" NumberFormat-DecimalDigits="0" MaxLength="3"></telerik:RadNumericTextBox>
                    <span>0 - 999，越大越靠前</span>
                </td>
            </tr>
            <tr>
                <td class="title">详细说明：</td>
                <td class="content" colspan="3">
                    <textarea runat="server" id="txtDescription" style="width: 300px; height: 200px;" maxlength="500"></textarea>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
