<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExcessiveSetEdit.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.DataAnalyze.ExcessiveSetEdit" %>

<!DOCTYPE html>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<%@ Register TagPrefix="CbxRsm" TagName="FactorCbxRsm" Src="~/Controls/FactorCbxRsm.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>超标值限配置</title>
    <script type="text/javascript">
        function RefreshParentPage() {
            alert('保存成功！');
            window.parent.RefreshOnClick();
        }

        //保存文本框数字判断
        //function OnClientClicking(sender, eventArgs)
        function OnClientClicking() {
            var AdvanceLow = document.getElementById('tbxAdvanceLow').value;
            if (isNaN(AdvanceLow)) {
                alert("警戒下限不是数字");
                return false;
                //sender.set_autoPostBack(false);
                //return;
            }
            var AdvanceUpper = document.getElementById('tbxAdvanceUpper').value;
            if (isNaN(AdvanceUpper)) {
                alert("警戒上限不是数字");
                return false;
                //sender.set_autoPostBack(false);
                //return;
            }
            var ExcessiveRatio = document.getElementById('tbxExcessiveRatio').value;
            if (isNaN(ExcessiveRatio)) {
                alert("超标系数不是数字");
                return false;
                //sender.set_autoPostBack(false);
                //return;
            }
            var ExcessiveUpper = document.getElementById('tbxExcessiveUpper').value;
            if (isNaN(ExcessiveUpper)) {
                alert("超标上限不是数字");
                return false;
                //sender.set_autoPostBack(false);
                //return;
            }
            var ExcessiveLow = document.getElementById('tbxExcessiveLow').value;
            if (isNaN(ExcessiveLow)) {
                alert("超标下限不是数字");
                return false;
                //sender.set_autoPostBack(false);
                //return;
            }
        }
    </script>
    <style type="text/css">
        .FirstTdInTr
        {
            background-color: #e3eefb;
            border-bottom: solid 1px #99bbe8;
            border-right: solid 1px #99bbe8;
            text-align: left;
            width: 20%;
            height: 30px;
        }

        .SecondTdInTr
        {
            border-bottom: solid 1px #99bbe8;
            border-right: solid 1px #99bbe8;
            text-align: left;
            width: 30%;
            height: 30px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:radscriptmanager id="RadScriptManager1" runat="server" />
        <telerik:radajaxloadingpanel id="RadAjaxLoadingPanel1" runat="server" />
        <telerik:radajaxmanager id="RadAjaxManager1" runat="server"
            updatepanelsrendermode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="btnSave">
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="pointCbxRsmA">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="pointCbxRsmA" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="factorCbxRsmA" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="pointCbxRsmW">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="pointCbxRsmW" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="factorCbxRsmW" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:radajaxmanager>
        <table style="width: 100%; border-top: solid 1px #99bbe8; border-left: solid 1px #99bbe8">
            <tbody>
                <tr>
                    <td colspan="4" class="SecondTdInTr" style="float: left; width: 100%">
                        <asp:ImageButton ID="btnSave" runat="server" OnClick="btnSave_Click" SkinID="ImgBtnSave" OnClientClick="return OnClientClicking()" />
                    </td>
                </tr>
            </tbody>
        </table>
        <table style="width: 100%; border-top: solid 1px #99bbe8; border-left: solid 1px #99bbe8">
            <tbody>
                <%--<tr>
                    <td class="FirstTdInTr">应用程序:
                    </td>
                    <td class="SecondTdInTr">
                        <telerik:radcombobox id="applicationCmb" runat="server" autopostback="true" onselectedindexchanged="applicationCmb_SelectedIndexChanged">
                             <Items>
                                    <telerik:RadComboBoxItem Text="环境空气" Value="airaaira-aira-aira-aira-airaairaaira" />
                                    <telerik:RadComboBoxItem Text="地表水" Value="watrwatr-watr-watr-watr-watrwatrwatr" />
                                </Items>
                        </telerik:radcombobox>
                    </td>
                    <td class="FirstTdInTr"></td>
                    <td class="SecondTdInTr"></td>
                </tr>--%>
                <tr>
                    <td class="FirstTdInTr">点位:
                    </td>
                    <td class="SecondTdInTr">
                        <div id="div1" runat="server" visible="false">
                            <CbxRsm:PointCbxRsm runat="server" CbxHeight="350" MultiSelected="true" DropDownWidth="520" ID="pointCbxRsmA" ApplicationType="Air" CbxWidth="200"></CbxRsm:PointCbxRsm>
                        </div>
                        <div id="div2" runat="server" visible="false">
                            <CbxRsm:PointCbxRsm runat="server" CbxHeight="350" MultiSelected="true" DropDownWidth="520" ID="pointCbxRsmW" ApplicationType="Water" CbxWidth="200"></CbxRsm:PointCbxRsm>
                        </div>
                    </td>
                    <td class="FirstTdInTr">监测因子:
                    </td>
                    <td class="SecondTdInTr">
                        <div id="div3" runat="server" visible="false">
                            <CbxRsm:FactorCbxRsm runat="server" ApplicationType="Air" DropDownWidth="420" ID="factorCbxRsmA" CbxWidth="200"></CbxRsm:FactorCbxRsm>
                        </div>
                        <div id="div4" runat="server" visible="false">
                            <CbxRsm:FactorCbxRsm runat="server" ApplicationType="Water" DropDownWidth="420" ID="factorCbxRsmW" CbxWidth="200"></CbxRsm:FactorCbxRsm>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="FirstTdInTr">数据类型:
                    </td>
                    <td class="SecondTdInTr">
                        <telerik:radcombobox id="dataTypeCmb" runat="server" autopostback="false">
                        </telerik:radcombobox>
                    </td>
                    <td class="FirstTdInTr">通知级别：</td>
                    <td class="SecondTdInTr">
                        <telerik:radcombobox id="notifyGradeCmb" runat="server" autopostback="false">
                        </telerik:radcombobox>
                    </td>
                </tr>
                <tr>
                    <td class="FirstTdInTr">警戒范围:
                    </td>
                    <td class="SecondTdInTr">
                        <telerik:radtextbox id="tbxAdvanceRange" runat="server">
                        </telerik:radtextbox>
                    </td>
                    <td class="FirstTdInTr"></td>
                    <td class="SecondTdInTr"></td>
                </tr>
                <tr>
                    <td class="FirstTdInTr">警戒下限:
                    </td>
                    <td class="SecondTdInTr">
                        <telerik:radtextbox id="tbxAdvanceLow" runat="server">
                        </telerik:radtextbox>
                    </td>
                    <td class="FirstTdInTr">警戒上限:
                    </td>
                    <td class="SecondTdInTr">
                        <telerik:radtextbox id="tbxAdvanceUpper" runat="server">
                        </telerik:radtextbox>
                    </td>
                </tr>
                <tr>
                    <td class="FirstTdInTr">超标范围:
                    </td>
                    <td class="SecondTdInTr">
                        <telerik:radtextbox id="tbxExcessiveRange" runat="server">
                        </telerik:radtextbox>
                    </td>
                    <td class="FirstTdInTr">超标系数:
                    </td>
                    <td class="SecondTdInTr">
                        <telerik:radtextbox id="tbxExcessiveRatio" runat="server">
                        </telerik:radtextbox>
                    </td>
                </tr>
                <tr>
                    <td class="FirstTdInTr">规则用途类型：</td>
                    <td class="SecondTdInTr">
                        <telerik:radcombobox id="useForCmb" runat="server" autopostback="false" Enabled="false">
                        </telerik:radcombobox>
                    </td>
                    <td class="FirstTdInTr"></td>
                    <td class="SecondTdInTr"></td>
                </tr>
                <tr>
                    <td class="FirstTdInTr">超标上限:
                    </td>
                    <td class="SecondTdInTr">
                        <telerik:radtextbox id="tbxExcessiveUpper" runat="server">
                        </telerik:radtextbox>
                    </td>
                    <td class="FirstTdInTr">超标下限:
                    </td>
                    <td class="SecondTdInTr">
                        <telerik:radtextbox id="tbxExcessiveLow" runat="server">
                        </telerik:radtextbox>
                    </td>
                </tr>
                <tr>
                    <td class="FirstTdInTr">替换状态:
                    </td>
                    <td class="SecondTdInTr">
                        <telerik:radtextbox id="tbxReplaceStatus" runat="server">
                        </telerik:radtextbox>
                    </td>
                    <td class="FirstTdInTr">限值类别:
                    </td>
                    <td class="SecondTdInTr">
                        <telerik:radtextbox id="tbxStandardType" runat="server">
                        </telerik:radtextbox>
                    </td>
                </tr>
                <tr>
                    <td class="FirstTdInTr">是否使用:
                    </td>
                    <td class="SecondTdInTr">
                        <asp:RadioButtonList ID="rbtEnableOrNot" runat="server" AutoPostBack="false">
                            <asp:ListItem Text="否" Value="0"></asp:ListItem>
                            <asp:ListItem Text="是" Value="1"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td class="FirstTdInTr">是否通知:
                    </td>
                    <td class="SecondTdInTr">
                        <asp:RadioButtonList ID="rbtNotifyOrNot" runat="server" AutoPostBack="false">
                            <asp:ListItem Text="否" Value="0"></asp:ListItem>
                            <asp:ListItem Text="是" Value="1"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td class="FirstTdInTr">备注:
                    </td>
                    <td class="SecondTdInTr" colspan="3">
                        <telerik:radtextbox id="tbxDescription" runat="server" textmode="MultiLine">
                        </telerik:radtextbox>
                    </td>
                </tr>
            </tbody>
        </table>
        <%--<table style="width: 100%; border-top: solid 1px #99bbe8; border-left: solid 1px #99bbe8">
            <tbody>
                
            </tbody>
        </table>--%>
        <telerik:radwindowmanager id="RadWindowManager1" runat="server" enableshadow="true"
            visiblestatusbar="false">
            <Windows>
            </Windows>
            <Localization Close="关闭" Maximize="最大化" Minimize="最小化" Restore="还原" Reload="刷新" PinOff="移动"
                PinOn="固定" />
        </telerik:radwindowmanager>
    </form>
</body>
</html>
