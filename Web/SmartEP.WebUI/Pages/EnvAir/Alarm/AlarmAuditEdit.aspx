<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AlarmAuditEdit.aspx.cs" MasterPageFile="~/WebMaster/MasterPage.Master" Inherits="SmartEP.WebUI.Pages.EnvAir.Alarm.AlarmAuditEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            function OnClientClicking() {
                var date = $find("<%= dateAudit.ClientID %>").get_selectedDate();
                if (date == null) {
                    alert("请选择审核时间！");
                    return false;
                } else {
                    return true;
                }
            }

            function RefreshParent() {
                this.parent.Refresh_Grid(true);
            }
        </script>
    </telerik:RadCodeBlock>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="btnEdit">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="dtpHandle" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="txtDesc" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <table id="maintable" class="Table_Customer" width="100%">
        <tr class="btnTitle">
            <td class="btnTitle" colspan="2">
                <asp:ImageButton ID="btnEdit" SkinID="ImgBtnSave" runat="server" OnClientClick="return OnClientClicking()" OnClick="btnEdit_Click" />
            </td>
        </tr>
        <tr id="alarmContent" runat="server">
            <td class="title" style="width: 80px;">报警内容:</td>
            <td class="content">
                <telerik:RadTextBox ID="txtAlarmCon" runat="server" TextMode="MultiLine" ReadOnly="true" Enabled="false" Width="350px" Height="80px" /></td>
        </tr>
        <tr id="alarmTime" runat="server">
            <td class="title">处理时间:</td>
            <td class="content">
                <telerik:RadTextBox ID="txtDealTime" runat="server" ReadOnly="true" Enabled="false" Width="150px" />
            </td>
        </tr>
        <tr id="alarmSuggest" runat="server">
            <td class="title">处理意见:</td>
            <td class="content">
                <telerik:RadTextBox ID="txtDealContext" runat="server" TextMode="MultiLine" Width="350px" Enabled="false" ReadOnly="true" Height="80px" /></td>
        </tr>
        <tr>
            <td class="title">审核时间:</td>
            <td class="content">
                <telerik:RadDateTimePicker ID="dateAudit" BorderWidth="1" BorderColor="Red" runat="server" Width="200px" MinDate="1900-01-01 00:00:00"
                    DateInput-Font-Size="10" DateInput-Font-Bold="true" DateInput-DateFormat="yyyy-MM-dd HH:mm:ss"
                    DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" Calendar-FastNavigationSettings-OkButtonCaption="确定"
                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" TimeView-HeaderText="小时" />
            </td>
        </tr>
        <tr>
            <td class="title">审核意见:</td>
            <td class="content">
                <telerik:RadTextBox ID="txtAuditContext" runat="server" TextMode="MultiLine" Width="350px" MaxLength="2000" Height="80px" /></td>
        </tr>
        <tr  id="pLAudit" runat="server">
            <td class="title">批量审核:</td>
            <td class="content">
                <telerik:radcombobox id="alarmPLAudit" runat="server" width="220px" emptymessage="请选择">
                    <Items>
                        <telerik:RadComboBoxItem Text="无" Value="0" Selected="true"/>
                        <telerik:RadComboBoxItem Text="审核当天数据" Value="1" />
                        <telerik:RadComboBoxItem Text="审核当前测点类型因子的全部时间数据" Value="2" />
                    </Items>
                </telerik:radcombobox>
            </td>
        </tr>
    </table>
</asp:Content>
