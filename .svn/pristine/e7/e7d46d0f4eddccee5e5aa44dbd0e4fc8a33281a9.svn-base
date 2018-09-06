<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AirFlowChart.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.RealTimeData.AirFlowChart" %>

<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
</head>
<body style="background-color: #ddddda; height: 700px;" onload="RealStart();setInterval('RealStart();',60000*3);">
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1"
            UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="pointCbxRsm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="pointCbxRsm" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="frameFlowChart" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>

        <table cellpadding="0" cellspacing="0" width="100%" class="Table_Customer">
            <tr>
                <td class="title" style="width: 80px">
                    <b>选择测点：</b>
                </td>
                <td class="content" style="width: 260px">
                    <CbxRsm:PointCbxRsm ID="pointCbxRsm" runat="server" ApplicationType="Air" CbxWidth="220" CbxHeight="350" MultiSelected="false" DropDownWidth="520"
                        OnSelectedChanged="pointCbxRsm_SelectedChanged" />
                </td>
                <td class="content">
                    <asp:ImageButton ID="BtnSearch" runat="server" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" Style="display: none;" />
                </td>
            </tr>
            <tr>
                <td align="center" valign="top" colspan="3">
                    <iframe id="frameFlowChart" runat="server" width="100%" height="730px" frameborder="0"></iframe>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
