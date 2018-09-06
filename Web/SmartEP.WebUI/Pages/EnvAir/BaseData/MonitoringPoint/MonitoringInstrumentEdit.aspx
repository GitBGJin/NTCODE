<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MonitoringInstrumentEdit.aspx.cs" MasterPageFile="~/WebMaster/MasterPage.Master" Inherits="SmartEP.WebUI.Pages.EnvAir.BaseData.MonitoringPoint.MonitoringInstrumentEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <table id="maintable" cellspacing="1" width="100%" cellpadding="0"
            border="0" class="Table_Customer">
             <tr class="btnTitle">
             <td class="btnTitle" colspan="2">
                <asp:ImageButton ID="btnEdit" SkinID="ImgBtnSave" runat="server" OnClick="btnEdit_Click" />
            </td>
        </tr>
            <tr>
                <td class="title">
                    站点名称
                </td>
                <td class="content" colspan="3">
                    <telerik:RadTextBox   ID="txtPointName" runat="server" Enabled="false" ></telerik:RadTextBox>
                </td>
             
            </tr>
            <tr>
         <td class="title">
                选择监测仪器
            </td>
            <td class="content">
                 <telerik:RadComboBox ID="rcbInstrument" runat="server" Width="500px"
                        EmptyMessage="选择仪器" >
                        
                    </telerik:RadComboBox>
                     <td class="title">
                    取数时间
                </td>
                <td class="content">
                    <telerik:RadTextBox   ID="txtFetchDataTime" runat="server" MaxLength="50"></telerik:RadTextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtFetchDataTime"
                        ErrorMessage="非负整数" ValidationExpression="^-?\d+$" />
                </td>
            </td>
        </tr>
             
            <tr>
               <td class="title">
                    通讯串口号
                </td>
                <td class="content">
                    <telerik:RadTextBox   ID="txtSerialPort" runat="server" MaxLength="50"></telerik:RadTextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtSerialPort"
                        ErrorMessage="非负整数" ValidationExpression="^-?\d*$" />
                </td>
                <td class="title">
                    支持接口
                </td>
                <td class="content">
                    <telerik:RadTextBox   ID="txtSupportInterfaces" runat="server" MaxLength="50"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td class="title">
                    使用接口
                </td>
                <td class="content">
                    <telerik:RadTextBox   ID="txtUseInterfaces" runat="server" MaxLength="50"></telerik:RadTextBox>
                </td>
                <td class="title">
                    串口通讯设置
                </td>
                <td class="content">
                    <telerik:RadTextBox   ID="txtSerialSetting" runat="server" MaxLength="50"></telerik:RadTextBox>
                </td>
            </tr> 
            <tr>
                <td class="title">
                    质保期
                </td>
                <td class="content">
                    <telerik:RadTextBox   ID="txtWarrantyPeriod" runat="server" MaxLength="50"></telerik:RadTextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtWarrantyPeriod"
                        ErrorMessage="非负整数" ValidationExpression="^-?\d*$" />
                </td>
                <td class="title">
                    socketIP
                </td>
                <td class="content">
                    <telerik:RadTextBox   ID="txtSocketIP" runat="server" MaxLength="50"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td class="title">
                    socket端口
                </td>
                <td class="content">
                    <telerik:RadTextBox   ID="txtSocketPort" runat="server" MaxLength="50"></telerik:RadTextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtSocketPort"
                        ErrorMessage="非负整数" ValidationExpression="^-?\d*$" />
                </td>
                <td class="title">
                    监测仪器编号
                </td>
                <td class="content">
                    <telerik:RadTextBox   ID="txtInstrumentNumber" runat="server" MaxLength="50"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td class="title">
                    仪器序列号
                </td>
                <td class="content">
                    <telerik:RadTextBox   ID="txtSerialNum" runat="server" MaxLength="50"></telerik:RadTextBox>
                </td>
                <td class="title">
                    socket协议
                </td>
                <td class="content">
                    <telerik:RadTextBox   ID="txtSocketProtocol" runat="server" MaxLength="50"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td class="title">
                    备注
                </td>
                <td class="content" colspan="3">
                    <telerik:RadTextBox   ID="txtMemo" runat="server" Height="50px" Width="80%" MaxLength="500"></telerik:RadTextBox>
                </td>
            </tr>
            </table>
</asp:Content>