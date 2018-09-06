<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AlarmNotifyAdd.aspx.cs" MasterPageFile="~/WebMaster/MasterPage.Master" Inherits="SmartEP.WebUI.Pages.EnvAir.Alarm.AlarmNotifyAdd" %>

<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rbtnlType">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rbtnlType" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="dvProper" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="dvPoints" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="ddlCityProper" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="alarmEventList" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <table id="maintable" width="100%" class="Table_Customer">
        <tr class="btnTitle">
            <td class="btnTitle" colspan="4">
                <asp:ImageButton ID="btnAdd" SkinID="ImgBtnAdd" runat="server" OnClick="btnAdd_Click" />
            </td>
        </tr>
        <tr>
            <td class="title" style="width: 15%;">查询范围：</td>
            <td class="content" colspan="3">
                <asp:RadioButtonList ID="rbtnlType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rbtnlType_SelectedIndexChanged" RepeatLayout="Flow" RepeatColumns="5">
                    <asp:ListItem Text="测点" Value="Port" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="区域" Value="CityProper"></asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td class="title" style="width: 15%;">
                <div id="dvName">监测点</div>
            </td>
            <td class="content" colspan="3">
                <div runat="server" id="dvPoints">
                    <CbxRsm:PointCbxRsm runat="server" ApplicationType="Air" CbxWidth="400" CbxHeight="350" MultiSelected="true" DropDownWidth="400" ID="pointCbxRsm"></CbxRsm:PointCbxRsm>
                </div>
                <div runat="server" id="dvProper" style="display: none">
                    <telerik:RadDropDownList runat="server" ID="ddlCityProper" Width="180px">
                        <Items>
                            <telerik:DropDownListItem Text="苏州市区" Value="7e05b94c-bbd4-45c3-919c-42da2e63fd43" Selected="true" />
                        </Items>
                    </telerik:RadDropDownList>
                </div>
            </td>
        </tr>
        <tr>
            <td class="title" style="width: 15%;">策略名称</td>
            <td class="content" colspan="3">
                <telerik:RadTextBox ID="txtNotifyStrategyName" runat="server" Width="80%" MaxLength="50"></telerik:RadTextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*不能为空" ControlToValidate="txtNotifyStrategyName"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="title" style="width: 15%;">报警等级</td>
            <td class="content" style="width: 35%;">
                <telerik:RadComboBox ID="notifyGradeList" runat="server" Width="150px">
                </telerik:RadComboBox>
            </td>
            <td class="title" style="width: 15%;">报警类型</td>
            <td class="content" style="width: 35%;">
                <telerik:RadComboBox ID="alarmEventList" runat="server" Width="150px">
                </telerik:RadComboBox>
            </td>
        </tr>
        <tr>
            <td class="title">通知号码</td>
            <td class="content">
                <telerik:RadComboBox ID="notifyNumberList" Enabled="true" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput"
                    Width="80%" DataTextField="Name" DataValueField="RowGuid" Localization-CheckAllString="选择全部" EmptyMessage="请选择">
                </telerik:RadComboBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*不能为空" ControlToValidate="notifyNumberList"></asp:RequiredFieldValidator>
            </td>
            <td class="title">通知邮箱</td>
            <td class="content">
                <telerik:RadComboBox ID="notifyMailList" Enabled="true" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput"
                    Width="80%" DataTextField="Name" DataValueField="RowGuid" Localization-CheckAllString="选择全部" EmptyMessage="请选择">
                </telerik:RadComboBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="*不能为空" ControlToValidate="notifyNumberList"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="title">开始时间</td>
            <td class="content">
                <telerik:RadTimePicker runat="server" ID="txtBeginTime" DateInput-DateFormat="HH:00" Width="150px"></telerik:RadTimePicker>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*不能为空" ControlToValidate="txtBeginTime"></asp:RequiredFieldValidator>
            </td>
            <td class="title">结束时间</td>
            <td class="content">
                <telerik:RadTimePicker runat="server" ID="txtEndTime" DateInput-DateFormat="HH:00" Width="150px"></telerik:RadTimePicker>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="*不能为空" ControlToValidate="txtEndTime"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="title">通知次数</td>
            <td class="content">
                <telerik:RadNumericTextBox runat="server" ID="txtNotifyCount" Width="100px" NumberFormat-DecimalDigits="0" MinValue="0" Value="1"></telerik:RadNumericTextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="*不能为空" ControlToValidate="txtNotifyCount"></asp:RequiredFieldValidator>
            </td>
            <td class="title">通知间隔</td>
            <td class="content">
                <telerik:RadNumericTextBox runat="server" ID="txtNotifySpan" Width="100px" NumberFormat-DecimalDigits="0" MinValue="0" Value="60"></telerik:RadNumericTextBox>
                <label style="width: 50px">分钟</label>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="*不能为空" ControlToValidate="txtNotifySpan"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="title">开始日期</td>
            <td class="content">
                <telerik:RadDatePicker ID="beginDate" runat="server" Width="150px" EnableTyping="false">
                </telerik:RadDatePicker>
            </td>
            <td class="title">结束日期</td>
            <td class="content">
                <telerik:RadDatePicker ID="endDate" runat="server" Width="150px" EnableTyping="false">
                </telerik:RadDatePicker>
            </td>
        </tr>
        <tr>
            <td class="title">是否启用</td>
            <td class="content">
                <asp:RadioButtonList ID="rbtnEnableOrNot" Width="90px" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Text="是" Value="1" Selected="True" />
                    <asp:ListItem Text="否" Value="0" />
                </asp:RadioButtonList>
            </td>
            <td class="title">排序号</td>
            <td class="content">
                <telerik:RadNumericTextBox runat="server" ID="tbxOrderByNum" Width="150px" NumberFormat-DecimalDigits="0" MinValue="0" Value="0"></telerik:RadNumericTextBox>
            </td>
        </tr>
        <tr>
            <td class="title">备注</td>
            <td class="content" colspan="3">
                <telerik:RadTextBox ID="txtDescription" runat="server" MaxLength="50" Height="100px" Width="90%">
                </telerik:RadTextBox></td>
        </tr>
    </table>
</asp:Content>
