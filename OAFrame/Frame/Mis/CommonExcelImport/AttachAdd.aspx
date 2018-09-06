<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AttachAdd.aspx.cs" Inherits="Com.Sinoyd.Mis.WebUI.CommonExcelImport.AttachAdd" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>附件上传</title>
    <style>
        .TextBox {
            border-right: 1px solid #94C7E7;
            border-top: 1px solid #94C7E7;
            border-left: 1px solid #94C7E7;
            border-bottom: 1px solid #94C7E7;
            font-size: 12px;
            vertical-align: middle;
            height: 26px;
            width: 98%;
            padding: 2px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Panel1" EnableAjax="false" />
        <f:Panel ID="Panel1" runat="server" Layout="Fit" ShowBorder="False" ShowHeader="false"
            BodyPadding="5px">
            <Items>
                <f:Form Title="Form1" BodyPadding="5px" ID="Form3" ShowHeader="false" ShowBorder="false"
                    runat="server">
                    <Rows>
                        <f:FormRow>
                            <Items>
                                <f:ContentPanel runat="server" ID="AttachPanel" ShowBorder="false" ShowHeader="false">
                                    <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td style="display: none;">
                                                <asp:TextBox runat="server" ID="txt_AttachTypeList" Text="xls"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td height="15"></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:FileUpload ID="FileUpload1" runat="server" CssClass="TextBox"></asp:FileUpload>
                                            </td>
                                            <td width="60" align="left">
                                                <f:Button ID="UpLoad" runat="server" Text="导入" OnClick="UpLoad_Click">
                                                </f:Button>
                                            </td>
                                        </tr>
                                    </table>
                                </f:ContentPanel>
                            </Items>
                        </f:FormRow>
                    </Rows>
                </f:Form>
            </Items>
        </f:Panel>
    </form>
</body>
</html>