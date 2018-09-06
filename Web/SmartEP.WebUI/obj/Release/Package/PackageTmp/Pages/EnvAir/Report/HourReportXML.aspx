<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HourReportXML.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Report.HourReportXML" %>

<!DOCTYPE html>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script src="../../../Resources/JavaScript/JQuery/jquery-1.9.0.js"></script>
            <script type="text/javascript">
                function OnClientClicking() {
                    var date1 = new Date();
                    var date2 = new Date();
                    var date1 = $find("<%= hourBegin.ClientID %>").get_selectedDate();
                    var date2 = $find("<%= hourEnd.ClientID %>").get_selectedDate();
                    //if ((date1 == null) || (date2 == null)) {
                    //    alert("开始时间或者终止时间，不能为空！");
                    //    return false;
                    //}
                    if (date1 > date2) {
                        alert("开始时间不能大于终止时间！");
                        return false;
                    } else {
                        return true;
                    }
                }
                function OnClientNewClicking() {
                    $('#AuditSubmitDiv').css("display", "");
                    return true;
                }
                $(document).ready(function () {
                    ResizePageDiv();//设置蒙版div的高度、宽度
                });
                function ResizePageDiv() {
                    var bodyWidth = document.body.clientWidth;
                    var bodyHeight = document.body.clientHeight;
                    $('#pagediv').css("height", bodyHeight);
                    $('#pagediv').css("width", bodyWidth);
                    $('#AuditSubmitDiv').css("height", bodyHeight);
                    $('#AuditSubmitDiv').css("width", bodyWidth);
                }
            </script>
        </telerik:RadCodeBlock>
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="FileUpload1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="FileUpload1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnUpLoad">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="RadGrid1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <div runat="server" id="pagediv" style="position: absolute; z-index: -1;"></div>
        <div id="AuditSubmitDiv" style="display: none; vertical-align: middle; text-align: center; background-color: white; opacity: 0.7; filter: alpha(opacity=70); z-index: 100; position: absolute;">
            <p style="text-align: center; vertical-align: middle; padding-top: 20%; font-weight: bold; font-size: 18px; color: #b4aa38;">数据生成中...</p>
        </div>
        <telerik:RadSplitter ID="RadSplitter1" runat="server" Orientation="Horizontal" Height="100%" Width="100%">
            <telerik:RadPane ID="ToolBarRadPane" runat="server" Height="40px" Width="100%" Scrolling="None" BorderWidth="0">
                <table class="Table_Customer" style="height: 100%; width: 100%">
                    <tr>
                        <td class="title" style="width: 100px;">监测点：
                        </td>
                        <td class="content" style="width: 300px;">
                            <%--  <telerik:RadComboBox runat="server" ID="rcbPoint" CheckBoxes="true" Width="100%"
                                EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">--%>
                            <telerik:RadComboBox runat="server" ID="rcbPoint" Localization-CheckAllString="全选" Width="100%" CheckBoxes="true" Visible="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                                <Items>
                                    <telerik:RadComboBoxItem Text="文昌中学" Value="26" Checked="true" />
                                    <telerik:RadComboBoxItem Text="昆山花桥" Value="27" Checked="true" />
                                    <telerik:RadComboBoxItem Text="方舟公园" Value="28" Checked="true" />
                                    <telerik:RadComboBoxItem Text="东山" Value="29" Checked="true" />
                                    <telerik:RadComboBoxItem Text="拙政园" Value="33" Checked="true" />
                                    <telerik:RadComboBoxItem Text="香山" Value="168" Checked="true" />
                                    <telerik:RadComboBoxItem Text="东南开发区子站" Value="179" Checked="true" />
                                    <telerik:RadComboBoxItem Text="氟化工业园区" Value="180" Checked="true" />
                                    <telerik:RadComboBoxItem Text="沿江开发区" Value="181" Checked="true" />
                                    <telerik:RadComboBoxItem Text="乐余广电站" Value="176" Checked="true" />
                                    <telerik:RadComboBoxItem Text="张家港农业示范园" Value="25" Checked="true" />
                                    <telerik:RadComboBoxItem Text="托普学院	" Value="177" Checked="true" />
                                    <telerik:RadComboBoxItem Text="淀山湖党校" Value="178" Checked="true" />
                                    <telerik:RadComboBoxItem Text="太仓三水厂" Value="172" Checked="true" />
                                    <telerik:RadComboBoxItem Text="太仓气象观测站" Value="173" Checked="true" />
                                    <telerik:RadComboBoxItem Text="双凤生态园" Value="174" Checked="true" />
                                    <telerik:RadComboBoxItem Text="荣文学校" Value="175" Checked="true" />
                                    <telerik:RadComboBoxItem Text="青剑湖" Value="184" Checked="true" />
                                    <telerik:RadComboBoxItem Text="苏州大学高教区" Value="182" Checked="true" />
                                    <telerik:RadComboBoxItem Text="东部工业区" Value="183" Checked="true" />
                                </Items>

                            </telerik:RadComboBox>
                        </td>

                        <td class="title" style="width: 80px; text-align: right;">开始时间:
                        </td>
                        <td class="content" style="width: 100px; text-align: right;">
                            <telerik:RadDateTimePicker ID="hourBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd HH:00"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                TimeView-HeaderText="小时" />
                        </td>
                        <td class="title" style="width: 80px; text-align: right;">截止时间:
                        </td>
                        <td class="content" style="width: 100px; text-align: left;">
                            <telerik:RadDateTimePicker ID="hourEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd HH:00"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                TimeView-HeaderText="小时" />
                        </td>
                        <td class="content" style="text-align: left; width: 100px">
                            <asp:ImageButton runat="server" ID="btnSearch" SkinID="ImgBtnSearch" OnClientClick="return OnClientClicking()" OnClick="btnSearch_Click" />
                        </td>
                        <td class="btns" style="text-align: left; width: 200px;">
                            <asp:Button ID="btnBTF" runat="server" Text="重新生成" OnClick="btnBTF_Click" OnClientClick="return OnClientNewClicking()" /></td>
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="RadPane1" runat="server" Height="100%" Width="100%" Scrolling="None" BorderWidth="0">
                <telerik:RadGrid ID="gridXML" runat="server" GridLines="None" CssClass="RadGrid_Customer" Width="100%" Height="100%" BorderStyle="None" AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                    AllowMultiRowSelection="false" EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false" ShowStatusBar="false" BorderWidth="0" BorderSize="0"
                    AutoGenerateColumns="false" OnNeedDataSource="gridXML_NeedDataSource">
                    <MasterTableView GridLines="None" CommandItemDisplay="None" NoMasterRecordsText="没有数据">
                        <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" RefreshText="查询" ShowExportToExcelButton="false" ShowExportToWordButton="false" ShowExportToPdfButton="false" />
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="序号" UniqueName="Row"
                                HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%#Container.DataSetIndex + 1%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="MonitoringPointName" UniqueName="MonitoringPointName" HeaderText="测点"
                                HeaderStyle-Width="100px" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="PointAQM" UniqueName="PointAQM" HeaderText="子站编号"
                                HeaderStyle-Width="100px" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="DateTime" UniqueName="DateTime" HeaderText="时间" DataFormatString="{0:yyyy-MM-dd HH:mm}"
                                HeaderStyle-Width="80px" ItemStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="顺序号" UniqueName="Number" DataField="Number" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="80px" ItemStyle-Width="80px">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="记录数" UniqueName="RecordNumber" DataField="RecordNumber" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="80px" ItemStyle-Width="80px">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="文件名" UniqueName="FileName" DataField="FileName" HeaderStyle-Width="200px" ItemStyle-Width="200px"
                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                            </telerik:GridBoundColumn>

                        </Columns>
                        <PagerStyle FirstPageToolTip="首页" AlwaysVisible="true" Position="Bottom" NextPageToolTip="下页" LastPageToolTip="尾页" PrevPageToolTip="上页"
                            PageSizeLabelText="显示记录数:" PagerTextFormat="跳转页面:{4}&amp;nbsp;显示 &lt;strong&gt;{2}&lt;/strong&gt; - &lt;strong&gt;{3}&lt;/strong&gt;条,共 &lt;strong&gt;{5}&lt;/strong&gt;条"></PagerStyle>
                    </MasterTableView>
                    <ClientSettings>
                        <Scrolling AllowScroll="true" EnableVirtualScrollPaging="false" UseStaticHeaders="true"
                            SaveScrollPosition="false"></Scrolling>
                    </ClientSettings>
                </telerik:RadGrid>
            </telerik:RadPane>
        </telerik:RadSplitter>

    </form>
</body>
</html>
