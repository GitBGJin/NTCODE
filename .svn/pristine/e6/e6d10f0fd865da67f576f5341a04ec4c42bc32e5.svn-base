<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImageShow.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.DataAnalyze.ImageShow" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script type="text/javascript">
                function OnClientClicking() {
                    var date1 = new Date();
                    var date2 = new Date();
                    date1 = $find("<%= dayBegin.ClientID %>").get_selectedDate();
                    date2 = $find("<%= dayEnd.ClientID %>").get_selectedDate();
                    if ((date1 == null) || (date2 == null)) {
                        alert("开始时间或者终止时间，不能为空！");
                        //sender.set_autoPostBack(false);
                        return false;
                    }
                    if (date1 > date2) {
                        alert("开始时间不能大于终止时间！");
                        return false;
                    } else {
                        return true;
                    }
                }
            </script>
        </telerik:RadCodeBlock>
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <%--<telerik:RadSkinManager ID="RadSkinManager1" runat="server" ShowChooser="false" />--%>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" Width="100%"
            BorderWidth="0" BorderStyle="None" BorderSize="0">
            <telerik:RadPane ID="paneWhere" runat="server" Height="60px" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="height: 100%; width: 80%" cellspacing="1" cellpadding="0" class="Table_Customer"
                    border="0">
                    <tr>
                        <td class="title" style="width: 50px; text-align: center;">测点:
                        </td>
                        <td class="content" style="width: 30%; text-align: left">
                            <telerik:RadComboBox runat="server" ID="rcbPoint" Localization-CheckAllString="全选" Width="250px" CheckBoxes="true" Visible="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                                <Items>
                                    <telerik:RadComboBoxItem Text="上方山" Value="上方山站" Checked="true" />
                                    <telerik:RadComboBoxItem Text="南门" Value="南门站" />
                                    <telerik:RadComboBoxItem Text="彩香" Value="彩香站" />
                                    <telerik:RadComboBoxItem Text="轧钢厂" Value="轧钢厂站" />
                                    <telerik:RadComboBoxItem Text="吴中区" Value="吴中区" />
                                    <telerik:RadComboBoxItem Text="高新区" Value="新区站" />
                                    <telerik:RadComboBoxItem Text="工业园区" Value="园区站" />
                                    <telerik:RadComboBoxItem Text="相城区" Value="相城区" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                        <td class="title" style="width: 100px; text-align: center;">时间:</td>
                        <td class="content" style="width: 360px;">
                            <table>
                                <tr>
                                    <td>
                                        <telerik:RadDatePicker ID="dayBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd"
                                            DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                            Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                            Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                            Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                            TimeView-HeaderText="小时" />
                                    </td>
                                    <td>&nbsp;&nbsp;至&nbsp;&nbsp;
                                    </td>
                                    <td>
                                        <telerik:RadDatePicker ID="dayEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd"
                                            DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                            Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                            Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                            Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                            TimeView-HeaderText="小时" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td class="content" align="left">
                            <asp:ImageButton ID="btnSearch" runat="server" OnClientClick="return OnClientClicking()" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <div class="demo-container">
                    <div class="right-image-gallery">
                        <telerik:RadImageGallery RenderMode="Lightweight" DisplayAreaMode="Image" ID="RadImageGallery2" runat="server" Width="100%" Height="100%" LoopItems="true">
                            <ThumbnailsAreaSettings Mode="Thumbnails" />
                            <ToolbarSettings ShowSlideshowButton="false" />
                            <ImageAreaSettings Width="100%" ShowNextPrevImageButtons="true" NextImageButtonText="下一张" PrevImageButtonText="上一张" />
                        </telerik:RadImageGallery>
                    </div>
                </div>
            </telerik:RadPane>
        </telerik:RadSplitter>
    </form>
</body>
</html>
