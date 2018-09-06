<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InstrumentFaultAdd.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Instrument.InstrumentFaultAdd" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script language="javascript" type="text/javascript">
                function GetRadWindow() {
                    var oWindow = null;
                    if (window.radWindow)
                        oWindow = window.RadWindow; //Will work in Moz in all cases, including clasic dialog
                    else if (window.frameElement.radWindow)
                        oWindow = window.frameElement.radWindow; //IE (and Moz as well)
                    return oWindow;
                }
                //function RefreshParent() {
                //    this.parent.Refresh_Grid(true);
                //}
            </script>
        </telerik:RadCodeBlock>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="pointCbxRsm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="InstrumentName" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                  <telerik:AjaxSetting AjaxControlID="pointCbxRsm1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="InstrumentName" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <table id="maintable" class="Table_Customer" width="100%">
            <tr class="btnTitle">
                <td class="btnTitle" colspan="4">
                    <asp:ImageButton ID="btnSave" runat="server" OnClick="btnSave_Click" SkinID="ImgBtnSave" />
                </td>
            </tr>
            <tr>
                <td class="title" style="width: 120px; text-align: center;">故障站点:
                </td>
                <td class="content" style="width: 200px;">
                    <CbxRsm:PointCbxRsm runat="server" ApplicationType="Air" CbxWidth="180" Visible="false" CbxHeight="350" MultiSelected="false" DropDownWidth="520" ID="pointCbxRsm"
                        OnSelectedChanged="pointCbxRsm_SelectedChanged"></CbxRsm:PointCbxRsm>
                    <CbxRsm:PointCbxRsm runat="server" ApplicationType="Water" CbxWidth="180" Visible="false" CbxHeight="350" MultiSelected="false" DropDownWidth="520" ID="pointCbxRsm1"
                        OnSelectedChanged="pointCbxRsm_SelectedChanged"></CbxRsm:PointCbxRsm>
                </td>
                <td class="title" style="width: 120px; text-align: center;">仪器名称:
                </td>
                <td class="content" style="width: 240px;">
                    <telerik:RadComboBox runat="server" ID="InstrumentName" Width="220">
                    </telerik:RadComboBox>
                </td>

            </tr>
            <tr>
                <td class="title" style="width: 120px; text-align: center;">状态:
                </td>
                <td class="content" style="width: 200px;">
                    <telerik:RadComboBox runat="server" ID="occurStatus" Width="180px" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                        <Items>
                            <telerik:RadComboBoxItem Text="停用" Value="停用" />
                            <telerik:RadComboBoxItem Text="已换下" Value="已换下" />
                        </Items>
                    </telerik:RadComboBox>
                </td>
                <td class="title" style="width: 120px; text-align: center;">时间:
                </td>
                <td class="content" style="width: 220px;">
                    <telerik:RadDateTimePicker ID="OccurTime" runat="server" MinDate="1900-01-01" BorderWidth="1px" Width="180px"
                        DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd HH:mm:ss"
                        Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                        Calendar-FastNavigationSettings-OkButtonCaption="确定"
                        Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                        TimeView-HeaderText="小时" />
                </td>
            </tr>
            <tr>
                <td class="title" style="width: 120px; text-align: center;">操作人员:
                </td>
                <td class="content" style="width: 200px;">
                    <telerik:RadComboBox runat="server" ID="UsedUser" Width="180px" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                    </telerik:RadComboBox>
                </td>
                <td class="title" style="width: 120px; text-align: center;">结果:
                </td>
                <td colspan="3">
                    <asp:TextBox runat="server" ID="OperateResult" Width="220"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="title" style="width: 120px; text-align: center;">说明:
                </td>
                <td colspan="3">
                    <asp:TextBox runat="server" ID="note" Height="50px" Width="98%" TextMode="MultiLine" MaxLength="500"></asp:TextBox>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
