<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AlarmHandle.aspx.cs" MasterPageFile="~/WebMaster/MasterPage.Master" Inherits="SmartEP.WebUI.Pages.EnvAir.Alarm.AlarmHandle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:radcodeblock id="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            function OnClientClicking() {
                var date = $find("<%= dtpHandle.ClientID %>").get_selectedDate();
                if (date == null) {
                    alert("请选择处理时间！");
                    return false;
                } else {
                    return true;
                }
            }

            function RefreshParent() {
                this.parent.Refresh_Grid(true);
            }
        </script>
    </telerik:radcodeblock>
    <telerik:radajaxmanager id="RadAjaxManager1" runat="server" defaultloadingpanelid="RadAjaxLoadingPanel1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="btnEdit">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="dtpHandle" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="txtDesc" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:radajaxmanager>
    <table id="maintable" class="Table_Customer" width="100%">
        <tr class="btnTitle">
            <td class="btnTitle" colspan="2">
                <asp:ImageButton ID="btnEdit" SkinID="ImgBtnSave" runat="server" OnClientClick="return OnClientClicking()" OnClick="btnEdit_Click" />
            </td>
        </tr>
        <tr id="alarmContent" runat="server">
            <td class="title" style="text-align: center; width: 80px;">报警内容:</td>
            <td class="content">
                <telerik:radtextbox id="txtAlarmCon" runat="server" textmode="MultiLine" readonly="true" enabled="false" width="400px" height="80px" />
            </td>
        </tr>
        <tr>
            <td class="title">处理时间:</td>
            <td class="content">
                <telerik:raddatetimepicker id="dtpHandle" borderwidth="1" bordercolor="Red" runat="server" width="200px" mindate="1900-01-01 00:00:00"
                    dateinput-font-size="10" dateinput-font-bold="true" dateinput-dateformat="yyyy-MM-dd HH:mm:ss"
                    datepopupbutton-tooltip="打开日历选择" timepopupbutton-tooltip="打开小时选择" calendar-firstdayofweek="Monday"
                    calendar-fastnavigationsettings-todaybuttoncaption="当前年月" calendar-fastnavigationsettings-okbuttoncaption="确定"
                    calendar-fastnavigationsettings-cancelbuttoncaption="取消" timeview-headertext="小时" />
            </td>
        </tr>
        <tr>
            <td class="title">处理意见:</td>
            <td class="content">
                <telerik:radtextbox id="txtDesc" runat="server" textmode="MultiLine" width="400px" maxlength="2000" height="80px" />
            </td>
        </tr>
        <tr  id="pLDeal" runat="server">
            <td class="title">批量处理:</td>
            <td class="content">
                <telerik:radcombobox id="alarmPLHandle" runat="server" width="220px" emptymessage="请选择">
                    <Items>
                        <telerik:RadComboBoxItem Text="无" Value="0" Selected="true"/>
                        <telerik:RadComboBoxItem Text="处理当天数据" Value="1" />
                        <telerik:RadComboBoxItem Text="处理当前测点类型因子的全部时间数据" Value="2" />
                    </Items>
                </telerik:radcombobox>
            </td>
        </tr>
    </table>
</asp:Content>
