<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AuditCfgBase.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Audit.AuditCfgBase" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
        <script type="text/javascript" language="javascript">
            function onRequestStart(sender, args) {
                if (args.EventArgument == 10 || args.EventArgument == 11 ||
                    args.EventTarget.indexOf("ExportToExcelButton") >= 0 ||
                    args.EventTarget.indexOf("ExportToWordButton") >= 0 ||
                    args.EventTarget.indexOf("ExportToCsvButton") >= 0 ||
                    args.EventTarget.indexOf("ExportToPdfButton") >= 0) {
                    args.set_enableAjax(false);
                }
                try {
                    if (args.EventTargetElement.control._itemData[args.EventArgument].commandArgument == "Export") {
                        args.set_enableAjax(false);
                    }
                }
                catch (e) {

                }
            }
        </script>
    </telerik:RadScriptBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1">
            <AjaxSettings>
            </AjaxSettings>
            <ClientEvents OnRequestStart="onRequestStart" />
        </telerik:RadAjaxManager>
        <telerik:RadMultiPage ID="RadMultCfgBase" runat="server" SelectedIndex="0" Width="100%" ScrollBars="Hidden">
            <telerik:RadPageView ID="RadPageViewBaseCfg" runat="server">
                <table class="Table_Customer" width="100%">
                    <tr>
                        <td class="title">数据类型</td>
                        <td class="title">数据缺失</td>
                        <td class="title">数据重复</td>
                        <td class="title">接近检出限</td>
                        <td class="title">不合理数据</td>
                        <td class="title">离群</td>
                        <td class="title">超标数据</td>
                        <td class="title">数据修改</td>
                        <td class="title">标记位修改</td>
                        <td class="title">数据清零</td>
                        <td class="title">补遗</td>
                    </tr>
                    <tr>
                        <td class="title">普通数据</td>
                        <td class="content">
                            <telerik:RadColorPicker runat="server" ShowIcon="true" PaletteModes="HSB" ID="RadColorA1" Localization-ApplyButtonText="应用" />
                        </td>
                        <td class="content">
                            <telerik:RadColorPicker runat="server" ShowIcon="true" PaletteModes="HSB" ID="RadColorA2" Localization-ApplyButtonText="应用" />
                        </td>
                        <td class="content">
                            <telerik:RadColorPicker runat="server" ShowIcon="true" PaletteModes="HSB" ID="RadColorA3" Localization-ApplyButtonText="应用" />
                        </td>
                        <td class="content">
                            <telerik:RadColorPicker runat="server" ShowIcon="true" PaletteModes="HSB" ID="RadColorA4" Localization-ApplyButtonText="应用" />
                        </td>
                        <td class="content">
                            <telerik:RadColorPicker runat="server" ShowIcon="true" PaletteModes="HSB" ID="RadColorA5" Localization-ApplyButtonText="应用" />
                        </td>
                        <td class="content">
                            <telerik:RadColorPicker runat="server" ShowIcon="true" PaletteModes="HSB" ID="RadColorA6" Localization-ApplyButtonText="应用" />
                        </td>
                        <td class="content">
                            <telerik:RadColorPicker runat="server" ShowIcon="true" PaletteModes="HSB" ID="RadColorA7" Localization-ApplyButtonText="应用" />
                        </td>
                        <td class="content">
                            <telerik:RadColorPicker runat="server" ShowIcon="true" PaletteModes="HSB" ID="RadColorA8" Localization-ApplyButtonText="应用" />
                        </td>
                        <td class="content">
                            <telerik:RadColorPicker runat="server" ShowIcon="true" PaletteModes="HSB" ID="RadColorA9" Localization-ApplyButtonText="应用" />
                        </td>
                        <td class="content">
                            <telerik:RadColorPicker runat="server" ShowIcon="true" PaletteModes="HSB" ID="RadColorA10" Localization-ApplyButtonText="应用" />
                        </td>
                    </tr>
                    <tr>
                        <td class="content" colspan="11">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="title">审核状态</td>
                        <td class="title">无数据</td>
                        <td class="title">未审核</td>
                        <td class="title">部分审核</td>
                        <td class="title">完成审核</td>
                        <td class="content" colspan="6">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="title">普通数据</td>
                        <td class="content">
                            <telerik:RadColorPicker runat="server" ShowIcon="true" PaletteModes="HSB" ID="RadColorA11" Localization-ApplyButtonText="应用" />
                        </td>
                        <td class="content">
                            <telerik:RadColorPicker runat="server" ShowIcon="true" PaletteModes="HSB" ID="RadColorA12" Localization-ApplyButtonText="应用" />
                        </td>
                        <td class="content">
                            <telerik:RadColorPicker runat="server" ShowIcon="true" PaletteModes="HSB" ID="RadColorA13" Localization-ApplyButtonText="应用" />
                        </td>
                        <td class="content">
                            <telerik:RadColorPicker runat="server" ShowIcon="true" PaletteModes="HSB" ID="RadColorA14" Localization-ApplyButtonText="应用" />
                        </td>
                        <td class="content" colspan="6">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="content" colspan="11">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="title">API小时范围（小时）</td>
                        <td class="content">
                            <telerik:RadNumericTextBox Width="100%" runat="server" ID="RadNTxtAPIDT"
                                MinValue="0" MaxValue="24" NumberFormat-DecimalDigits="0" MaxLength="2" /></td>
                        <td class="content" colspan="9">从昨天开始小时数,如:为0 昨天00:00:00点到昨天23:59:59,为24 今天00:00:00点到今天23:59:59</td>
                    </tr>
                    <tr>
                        <td class="title">AQI小时范围（小时）</td>
                        <td class="content">
                            <telerik:RadNumericTextBox Width="100%" runat="server" ID="RadNTxtAQIDT"
                                MinValue="0" MaxValue="24" NumberFormat-DecimalDigits="0" MaxLength="2" /></td>
                        <td class="content" colspan="9">从昨天开始小时数,如:为0 昨天00:00:00点到昨天23:59:59,为24 今天00:00:00点到今天23:59:59</td>
                    </tr>
                    <tr>
                        <td class="title">生成AQI数据类型</td>
                        <td class="content">
                            <asp:CheckBox ID="chkisAQIDAY" runat="server" Text="日数据" Checked="true" /></td>
                        <td class="content">
                            <asp:CheckBox ID="chkisAQIWEEK" runat="server" Text="周数据" Checked="true" /></td>
                        <td class="content">
                            <asp:CheckBox ID="chkisAQIMONTH" runat="server" Text="月数据" Checked="true" /></td>
                        <td class="content" colspan="7">
                            <asp:CheckBox ID="chkisAQIYEAR" runat="server" Text="年数据" Checked="true" /></td>
                    </tr>
                    <tr>
                        <td class="title">审核方式</td>
                        <td class="content">
                            <asp:CheckBox ID="ChkIsAutoAudit" runat="server" Text="自动审核" Checked="true" /></td>
                        <td class="content" colspan="9">
                            <asp:CheckBox ID="ChkIsCustAudit" runat="server" Text="人工审核" Checked="true" /></td>
                    </tr>
                    <tr>
                        <td class="title">生成上报文件</td>
                        <td class="content">
                            <asp:CheckBox ID="ChkIsExFile0" runat="server" Text="生成国家站" Checked="true" /></td>
                        <td class="content">
                            <asp:CheckBox ID="ChkIsExFile1" runat="server" Text="生成省站" Checked="true" /></td>
                        <td class="content" colspan="8">
                            <asp:CheckBox ID="ChkIsExFile2" runat="server" Text="生成月报" Checked="true" /></td>
                    </tr>
                    <tr>
                        <td class="title">审核操作</td>
                        <td class="content">
                            <asp:CheckBox ID="chkisedit0" runat="server" Text="允许修改标记位" Checked="true" /></td>
                        <td class="content" colspan="9">
                            <asp:CheckBox ID="chkisedit1" runat="server" Text="允许修改数据" Checked="true" /></td>
                    </tr>
                    <tr>
                        <td class="title">审核数据类型</td>
                        <td class="content" colspan="9">
                            <asp:RadioButtonList Width="100%" ID="RblAuditData" runat="server" RepeatLayout="Flow" RepeatColumns="3">
                                <asp:ListItem Text="AuditData60" Value="AuditData60" Selected="True">小时数据</asp:ListItem>
                                <asp:ListItem Text="AuditData5" Value="AuditData5">5分钟数据</asp:ListItem>
                                <asp:ListItem Text="AuditData10" Value="AuditData10">10分钟数据</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="content" colspan="10">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="title" colspan="10" align="center">
                            <telerik:RadButton ID="RadBtnSave" runat="server" Text="保存">
                                <Icon PrimaryIconCssClass="rbSave" PrimaryIconLeft="4" PrimaryIconTop="4" />
                            </telerik:RadButton>
                        </td>
                    </tr>
                </table>
            </telerik:RadPageView>
        </telerik:RadMultiPage>
    </form>
</body>
</html>
