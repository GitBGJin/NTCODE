<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RemoteControl2.aspx.cs"
    Inherits="SmartEP.WebUI.Pages.EnvAir.RemoteControl.RemoteControl2" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        .btnDisable .rbPrimaryIcon {
            background: url(img/loading2.gif) !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="ScriptManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" ClientEvents-OnResponseEnd="EnableButtons">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="btnDownload" EventName="Click">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="txtQueryProvider" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script type="text/javascript">

                function OnClientClicked(sender, eventArgs) {
                    sender.set_enabled(false);
                    sender.set_text("发送命令中...");
                    $get("downloadStatus").style.display = "";

                }

                function AlertTest() {
                    //alert("success");
                }

                function EnableButtons(sender, eventArgs) {
                    var btnDownload = $find("<%=btnDownload.ClientID%>");
                    btnDownload.set_enabled(true);

                    btnDownload.set_text(">>发送命令>>");
                    $get("downloadStatus").style.display = "none";

                    //刷新远程控制历史日志
                    var contentUrl = "RemoteControlLog.aspx";
                    window.parent.RefreshLogs(contentUrl)
                }
            </script>
        </telerik:RadCodeBlock>
        <table style="width: 100%; height: 100%;">
            <tr>
                <td style="width: 50%; vertical-align:top" rowspan="2">
                    <fieldset>
                        <legend style="color: Red; font-weight: bold;">反控命令▼</legend>
                        <telerik:RadFilter Width="100%" runat="server" ID="RadFilter1" Culture="zh-CN" ApplyButtonText="应用" CssClass="RadFilter_Customer"
                            AddExpressionToolTip="新增表达式" AddGroupToolTip="新增表达式组" RemoveToolTip="删除" ShowApplyButton="false" 
                            OnFieldEditorCreating="RadFilter1_FieldEditorCreating" OnPreRender="RadFilter1_PreRender"
                            OnExpressionItemCreated="RadFilter1_ExpressionItemCreated">
                            <Localization GroupOperationAnd="数据协议" GroupOperationOr="命令参数" GroupOperationNotAnd=""
                                GroupOperationNotOr="" FilterFunctionContains="" FilterFunctionDoesNotContain=""
                                FilterFunctionStartsWith="" FilterFunctionEndsWith="" FilterFunctionEqualTo="="
                                FilterFunctionNotEqualTo="" FilterFunctionGreaterThan="" FilterFunctionLessThan=""
                                FilterFunctionGreaterThanOrEqualTo="" FilterFunctionLessThanOrEqualTo="" FilterFunctionBetween=""
                                FilterFunctionNotBetween="" FilterFunctionIsEmpty="" FilterFunctionNotIsEmpty=""
                                FilterFunctionIsNull="" FilterFunctionNotIsNull=""></Localization>
                        </telerik:RadFilter>

                    </fieldset>
                </td>

                <td style="width: 50%;vertical-align:top">
                    <fieldset>
                        <legend style="color: Blue; font-weight: bold;">事件日志▼</legend>
                        <asp:TextBox ID="txtQueryProvider" runat="server" TextMode="MultiLine" Height="200px" Width="450px"></asp:TextBox>
                    </fieldset>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; vertical-align:top">
                    <telerik:RadButton ID="btnDownload" runat="server" OnClick="btnDownload_Click" Text=">> 发送命令 >>"
                        DisabledButtonCssClass="btnDisable" UseSubmitBehavior="false" OnClientClicked="OnClientClicked">
                        <Icon PrimaryIconCssClass="rbDownload" />
                    </telerik:RadButton>
                    <img src="../../../Resources/Images/loading/loading1.gif" id="downloadStatus" alt="Loading Gif" style="display: none" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
