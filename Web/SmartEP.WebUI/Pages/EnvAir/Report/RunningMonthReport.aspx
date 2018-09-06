<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RunningMonthReport.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Report.RunningMonthReport" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<%@ Register TagPrefix="CbxRsm" TagName="FactorCbxRsm" Src="~/Controls/FactorCbxRsm.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
            <script type="text/javascript">
                //行编辑按钮 pageTypeID和waterOrAirType参数名称固定pageTypeID：该页面的ID;waterOrAirType：水或气，0：表示水，1：表示气
                function ShowDetails() {
                    //var uri = "../../EnvWater/Report/CustomDialog.aspx?pageTypeID=RunningMonthReport &waterOrAirType=1";
                    //window.radopen(encodeURI(uri), "DialogOpen");

                }
                function RefreshParent() {
                    this.parent.Refresh_Grid(true);
                    window.close();
                }
                //关闭遮罩层
                function closeWin() {
                    var bgObj = document.getElementById("divbgObj");
                    if (bgObj !== null)
                        document.body.removeChild(bgObj);
                }
                //遮罩层
                function alertWin() {
                    var iWidth = document.body.clientWidth;
                    var iHeight = document.body.clientHeight;

                    var bgObj = document.createElement("div");
                    bgObj.setAttribute("id", "divbgObj");
                    bgObj.style.cssText = "position:absolute;left:0px;top:0px;width:" + iWidth + "px;height:" + Math.max(document.body.clientHeight, iHeight) + "px;filter:Alpha(Opacity=30);opacity:0.3;background: url('../Images/login/BgSpliter.png');background-color:#FEFEFE;z-index:101;text-align:center; vertical-align:middle;color:red;";
                    var bgimg = document.createElement("img");
                    bgimg.setAttribute("src", "/Skins/Default/Ajax/loading.gif");
                    bgObj.appendChild(bgimg);
                    document.body.appendChild(bgObj);

                }
                window.onload = function () {
                    //遍历页面所有 按钮添加loading效果，目前测试 只用一个
                    //var target = document.getElementById("btnMonthReport");
                    var target1 = document.getElementById("btnExport");
                    var type = "click";
                    var func = alertWin;
                    if (target1.addEventListener) {
                        target1.addEventListener(type, func, false);
                    } else if (target1.attachEvent) {
                        target1.attachEvent("on" + type, func);
                    } else {
                        target1["on" + type] = func;
                    }
                }
            </script>
        </telerik:RadScriptBlock>
        <table>
            <tr>
                <%--                <td style="text-align: center">报表配置方案</td>--%>
                <%--<td>
                    <telerik:RadDropDownList runat="server" ID="ddlConfigScheme" OnSelectedIndexChanged="ddlConfigScheme_SelectedIndexChanged">
                    </telerik:RadDropDownList>
                </td>
                <td>
                    <telerik:RadButton ID="btnSave" runat="server" Skin="Default" Width="80px" Text="保存分组因子" OnClick="btnSave_Click">
                    </telerik:RadButton>
                </td>
                <td>
                    <telerik:RadButton ID="btnCustom" runat="server" Skin="Default" Width="80px" Text="自定义" OnClientClicked="ShowDetails" AutoPostBack="false">
                    </telerik:RadButton>
                </td>--%>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:ImageButton ID="btnExport" OnClick="btnExport_Click" runat="server" CssClass="RadToolBar_Customer" SkinID="ImgBtnSave" /></td>
            </tr>
            <tr>
                <td style="text-align: center">测点</td>
                <td colspan="3">
                    <%-- <CbxRsm:PointCbxRsm runat="server" ApplicationType="Air" CbxWidth="360" CbxHeight="350" MultiSelected="true" DropDownWidth="410" ID="pointCbxRsm"></CbxRsm:PointCbxRsm>--%>
                    <telerik:RadComboBox runat="server" ID="pointCbx"  Localization-CheckAllString="全选" Width="400px" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                        <Items>
                            <telerik:RadComboBoxItem Text="上方山" Value="8" Selected="true" />
                            <telerik:RadComboBoxItem Text="南门" Value="1" Selected="true" />
                            <telerik:RadComboBoxItem Text="彩香" Value="2" Selected="true" />
                            <telerik:RadComboBoxItem Text="轧钢厂" Value="3" Selected="true" />
                            <telerik:RadComboBoxItem Text="吴中区" Value="4" Selected="true" />
                            <telerik:RadComboBoxItem Text="高新区" Value="5" Selected="true" />
                            <telerik:RadComboBoxItem Text="工业园区" Value="6" Selected="true" />
                            <telerik:RadComboBoxItem Text="相城区" Value="7" Selected="true" />
                        </Items>
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: center">因子</td>
                <td colspan="3">
                    <%--<CbxRsm:FactorCbxRsm runat="server" ApplicationType="Air" CbxWidth="400" CbxHeight="350" DropDownWidth="420" ID="factorCbxRsm"></CbxRsm:FactorCbxRsm>--%>
                    <telerik:RadComboBox runat="server" ID="factorCbx" Width="400px" CheckBoxes="true" Localization-CheckAllString="全选" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput" AutoPostBack="true">
                        <Items>
                            <telerik:RadComboBoxItem Text="二氧化硫" Value="a21026" Selected="true" />
                            <telerik:RadComboBoxItem Text="二氧化氮" Value="a21004" Selected="true" />
                            <telerik:RadComboBoxItem Text="一氧化碳" Value="a21005" Selected="true" />
                            <telerik:RadComboBoxItem Text="臭氧" Value="a05024" Selected="true" />
                            <telerik:RadComboBoxItem Text="PM10" Value="a34002" Selected="true" />
                            <telerik:RadComboBoxItem Text="PM2.5" Value="a34004" Selected="true" />
                        </Items>
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: center">报表时间</td>
                <td>
                    <telerik:RadMonthYearPicker ID="rmypMonth" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM"
                        DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday"
                        MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                        MonthYearNavigationSettings-OkButtonCaption="确定"
                        MonthYearNavigationSettings-CancelButtonCaption="取消" />
                </td>
            </tr>
            <tr>
                <td></td>
                <td colspan="2">
                    <%--<asp:Button ID="btnExport" runat="server" Text="保存" OnClick="btnExport_Click" />--%>
                    <asp:Button ID="btnMonthReport" runat="server" Visible="false" Text="下载" OnClick="btnMonthReport_Click" />
                </td>
            </tr>
        </table>
        <telerik:RadWindowManager ID="radWM" runat="server" VisibleStatusbar="false" Behaviors="Move,Pin,Reload,Close" EnableShadow="true">
            <Windows>
                <telerik:RadWindow ID="DialogOpen" runat="server" Height="510px" Width="780px" Skin="Outlook" IconUrl="~/App_Themes/Office2007/images/RadGridHeaderBg2.png"
                    Title="站点因子分组" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" />
            </Windows>
            <Localization Close="关闭" Maximize="最大化" Minimize="最小化" Restore="还原" Reload="刷新" PinOff="移动"
                PinOn="固定" />
        </telerik:RadWindowManager>
        <asp:HiddenField ID="hdpointiddata" runat="server" />
    </form>
</body>
</html>
