<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AcqusionInstrumentEdit.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.BaseData.AcqusionInstrumentEdit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>数采仪信息</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
    <table id="maintable" cellspacing="1" width="100%" cellpadding="0" class="Table_Customer"
        border="0">
         <tr class="btnTitle">
            <td class="btnTitle" colspan="4">
                <asp:ImageButton ID="btnEdit" SkinID="ImgBtnSave" runat="server" OnClick="btnEdit_Click" />
            </td>
        </tr>
        <tr>
            <td class="title">
                监测点
            </td>
            <td class="content">
                <telerik:RadTextBox   ID="txtMonitoringPoint" runat="server" Enabled="false" MaxLength="50"></telerik:RadTextBox>
            </td>
            <td class="title">
                数采仪名称
            </td>
            <td class="content">
                <telerik:RadTextBox   ID="txtAcqName" runat="server" MaxLength="50"></telerik:RadTextBox>
            </td>
        </tr>
        <tr>
           <%-- <td class="title">
                数据类型
            </td>
            <td class="content">
                <telerik:RadComboBox ID="rcbDataType" runat="server"   Width="150px">
                </telerik:RadComboBox>
            </td>--%>
          <%--  <td class="title">
                数采仪厂商
            </td>
            <td class="content">
                <telerik:RadComboBox ID="manufacturerList" runat="server"   Width="150px">
                </telerik:RadComboBox>
            </td>--%>
              <td class="title">
                MN
            </td>
            <td class="content">
                <telerik:RadTextBox   ID="txtMN" runat="server" MaxLength="14"></telerik:RadTextBox>
                 <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtMN"
                    ErrorMessage="14位数字" ValidationExpression="\d{14,}" /><asp:RequiredFieldValidator
                        ID="RequiredFieldValidator1" runat="server"  ControlToValidate="txtMN" ErrorMessage="不能为空"></asp:RequiredFieldValidator>
            </td>
             <td class="title">
                通讯端口
            </td>
            <td class="content" colspan="3">
                <telerik:RadTextBox   ID="txtCommunicationPort" runat="server" MaxLength="50"></telerik:RadTextBox>
            </td>
        </tr>       
         
        
        <tr>
           <td class="title">
                数采仪IP
            </td>
            <td class="content">
                <telerik:RadTextBox   ID="txtIP" runat="server" MaxLength="50"></telerik:RadTextBox>
            </td>
           <%--  <td class="title">
                采集频率
            </td>
            <td class="content">
                <telerik:RadComboBox ID="acqFrequencyList" runat="server"   Width="150px" MaxLength="50">
                </telerik:RadComboBox>
            </td>--%>
           <td class="title">
                加密密钥
            </td>
            <td class="content" colspan="3">
                <telerik:RadTextBox   ID="txtEncryptionKey" runat="server"></telerik:RadTextBox>
            </td>
        </tr>
        <tr>
            <td class="title">
                IP用户名
            </td>
            <td class="content">
                <telerik:RadTextBox   ID="txtIPUser" runat="server" MaxLength="50"></telerik:RadTextBox>
            </td>
            <td class="title">
                IP密码
            </td>
            <td class="content">
                <telerik:RadTextBox   ID="txtIPPwd" runat="server" MaxLength="50"></telerik:RadTextBox>
            </td>
        </tr>
        
        <tr>
            <td class="title">
                超标次数
            </td>
            <td class="content">
                <telerik:RadTextBox   ID="txtExcessiveCount" runat="server"></telerik:RadTextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtExcessiveCount"
                    ErrorMessage="非负整数" ValidationExpression="^\d+$" />
            </td>
            <td class="title">
                重发次数
            </td>
            <td class="content">
                <telerik:RadTextBox   ID="txtRetransmissionNum" runat="server" ></telerik:RadTextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtRetransmissionNum"
                    ErrorMessage="非负整数" ValidationExpression="^\d+$" />
            </td>
        </tr>
        <tr>
           
           
        </tr>
        
        <tr>
            <td class="title" >
            配置选项           
            </td>
             <td class="content" colspan="3">
             是否ACK <asp:CheckBox ID="cbxIsACK" runat="server" />
                是否CRC校验 <asp:CheckBox ID="cbxIsCRC" runat="server" />
                是否加密 <asp:CheckBox ID="cbxIsEncryption" runat="server" />
            </td>
        </tr>
        <tr>
         <td class="title">
                备注
            </td>
            <td class="content" colspan="3">
                <telerik:RadTextBox   ID="txtMemo" runat="server" Height="50px" Width="600px" MaxLength="500"
                    TextMode="MultiLine"></telerik:RadTextBox>
            </td>
        </tr>
         
    </table>
    </div>
    </form>
</body>
</html>
