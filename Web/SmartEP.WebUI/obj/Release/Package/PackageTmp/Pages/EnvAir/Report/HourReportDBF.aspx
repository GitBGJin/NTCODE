<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HourReportDBF.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Report.HourReportDBF" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>区、县数据上报</title>
    <link type="text/css" rel="stylesheet" href="../../../App_Themes/Neptune/Telerik.Web.UI.Skins/Metro/RadSplitterAudit.css" />
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script src="../../../Resources/JavaScript/JQuery/jquery-1.9.0.js"></script>
            <script type="text/javascript" src="../../../Resources/JavaScript/FrameJS.js"></script>
            <script type="text/javascript" src="../../../Resources/JavaScript/ChartOperator/merge.js"></script>
            <script src="../../../Resources/JavaScript/AuditOperator/MutilPointAuditData.js"></script>
            <script type="text/javascript">
                function OnClientClicking() {
                    var date1 = new Date();
                    var date2 = new Date();
                    var date1 = $find("<%= dtBegin.ClientID %>").get_selectedDate();
                    var date2 = $find("<%= dtEnd.ClientID %>").get_selectedDate();
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
                function InitGroupChart() {
                    try {
                        var hiddenData = $("#HiddenData").val();
                        var pointId = $("#PointIDHidden").val();
                        if (confirm(hiddenData)) {
                            $('#AuditSubmitDiv').css("display", "");
                            var StartTime = $find("<%=dtBegin.ClientID%>").get_selectedDate().format("yyyy/M/dd");
                            var EndTime = $find("<%=dtEnd.ClientID%>").get_selectedDate().format("yyyy/M/dd");
                            AjaxSubmitDBF(StartTime, EndTime, pointId);
                        }
                    
                        else {
                            var pointName = $("#HiddenName").val();
                            var DateBegin = $find("<%=dtBegin.ClientID%>").get_selectedDate().format("yyyy/M/dd 00:00:00");
                            var DateEnd = $find("<%=dtEnd.ClientID%>").get_selectedDate().format("yyyy/M/dd 23:00:00");
                            OpenFineUIWindow("bc1e261c-3d83-4acd-9299-3a0b81beb7a1", "Pages/EnvAir/DataAnalyze/OriginalData.aspx?PointName=" + pointName + "&DTBegin=" + DateBegin + "&DTEnd=" + DateEnd, "原始小时");

                        }
                    } catch (e) { }
                }
                function OnClient() {
                    ;
                    OpenFineUIWindow("bc1e261c-3d83-4acd-9299-3a0b81beb7a1", "Pages/EnvAir/Exchange/DayReportDBFUpload.aspx?", "省中心上报平台");

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
                <telerik:AjaxSetting AjaxControlID="dtBegin">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="dtEnd" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="dtEnd">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="dtBegin" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnBDF">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="HiddenData" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="PointIDHidden" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="HiddenName" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <div runat="server" id="pagediv" style="position: absolute; z-index: -1;"></div>
        <div id="AuditSubmitDiv" style="display: none; vertical-align: middle; text-align: center; background-color: white; opacity: 0.7; filter: alpha(opacity=70); z-index: 100; position: absolute;">
            <p style="text-align: center; vertical-align: middle; padding-top: 20%; font-weight: bold; font-size: 18px; color: #b4aa38;">数据上传中...</p>
        </div>
        <telerik:RadSplitter ID="RadSplitter1" runat="server" Orientation="Horizontal" Height="100%" Width="100%">
            <telerik:RadPane ID="ToolBarRadPane" runat="server" Height="100px" Width="100%" Scrolling="None" BorderWidth="0">
                <table class="Table_Customer" style="height: 100%; width: 100%">
                    <tr>
                        <td class="title" style="width: 100px;">上报模板：
                        </td>
                        <td class="title" colspan="5" style="text-align: left;">
                            <a href="../../../Files/区县上报/区县上报数据导入模板（苏州）.xls" style="color: red; font-weight: bolder; font-size: 16px;">上报数据导入模板（苏州）.xls</a>
                        </td>
                        <td rowspan="2" colspan="6">
                            <div style="height: 70px; width: 100%; overflow: auto; text-align: right" runat="server" id="div1"></div>
                        </td>
                    </tr>
                    <tr>
                        <td class="title" style="width: 100px;">选择Excel文件：
                        </td>
                        <td class="content" style="width: 500px;" colspan="2">
                            <asp:FileUpload runat="server" ID="FileUpload1" Width="100%"></asp:FileUpload>
                        </td>
                        <td class="content" style="text-align: center; width: 100px">
                            <asp:ImageButton runat="server" ID="btnUpLoad" SkinID="ImgBtnUpload" OnClick="btnUpLoad_Click" OnClientClick="return OnClientNewClicking()" />
                        </td>
                        <td class="content" style="width: 100px">
                            <asp:ImageButton runat="server" ID="btnExcel" SkinID="ImgBtnExcel" OnClick="btnExcel_Click" />
                        </td>
                        <td class="content" style="width: 100px;text-align:left;" >
                            <asp:Button runat="server" ID="btnBDF" Text="生成DBF" OnClick="btnBDF_Click" />
                        </td>
                        <%--  <td>
                            <telerik:RadButton ID="submitButton" Visible="false" runat="server" AutoPostBack="true" OnClick="submitButton_Click"></telerik:RadButton>
                        </td>--%>
                    </tr>
                    <tr>
                        <td class="title" style="width: 100px;">监测点：
                        </td>
                        <td class="content" style="width: 300px;">
                            <%--  <telerik:RadComboBox runat="server" ID="rcbPoint" CheckBoxes="true" Width="100%"
                                EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">--%>
                            <telerik:RadComboBox runat="server" ID="rcbPoint" Localization-CheckAllString="全选" Width="100%" CheckBoxes="true" Visible="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                                <Items>
                                    <telerik:RadComboBoxItem Text="文昌中学" Value="85C481F6-0D64-4A7A-AAE9-2D19637A4E1B" Checked="true" />
                                    <telerik:RadComboBoxItem Text="昆山花桥" Value="531A47F1-038E-4A96-BB12-1CC7207C2BC8" Checked="true" />
                                    <telerik:RadComboBoxItem Text="方洲公园" Value="5ED74863-F4B2-411E-89A3-4CDBC31AFDB4" Checked="true" />
                                    <telerik:RadComboBoxItem Text="东山" Value="A320AD97-443D-46FF-83A4-959FDF66EBFA" Checked="true" />
                                    <telerik:RadComboBoxItem Text="拙政园" Value="47D8CC8C-8A0A-472D-B786-D3E401824E54" Checked="true" />
                                    <telerik:RadComboBoxItem Text="香山" Value="14278fc8-b823-4bf6-894c-c31bcdb72d6b" Checked="true" />
                                    <telerik:RadComboBoxItem Text="东南开发区子站" Value="6db34abc-d7d9-4759-8781-ee5a322c0731" Checked="true" />
                                    <telerik:RadComboBoxItem Text="氟化工业园区" Value="393ee500-4adc-40ad-ad09-358868699d46" Checked="true" />
                                    <telerik:RadComboBoxItem Text="沿江开发区" Value="5d328e0d-5cc3-4949-b5ea-8818465a57b1" Checked="true" />
                                    <telerik:RadComboBoxItem Text="乐余广电站" Value="19b7f805-f70a-46a7-ac92-527a01c2dc9c" Checked="true" />
                                    <telerik:RadComboBoxItem Text="张家港农业示范园" Value="7C22B387-1287-4982-9D5C-8BD421E52D8C" Checked="true" />
                                    <telerik:RadComboBoxItem Text="托普学院	" Value="829c259d-c0a0-4720-8f15-58e2a52572a1" Checked="true" />
                                    <telerik:RadComboBoxItem Text="淀山湖党校" Value="92d3f31a-7f7f-4a86-a434-0b4a2a7e122c" Checked="true" />
                                    <telerik:RadComboBoxItem Text="太仓三水厂" Value="77d81766-b278-486c-9fc0-d52f394ae1d8" Checked="true" />
                                    <telerik:RadComboBoxItem Text="太仓气象观测站" Value="9871d05e-0b3e-497b-b731-4093fb693e1a" Checked="true" />
                                    <telerik:RadComboBoxItem Text="双凤生态园" Value="507a0d9b-723d-45ba-b762-0a441118eebf" Checked="true" />
                                    <telerik:RadComboBoxItem Text="荣文学校" Value="74efcd38-3020-4821-9f26-d64e74340ea6" Checked="true" />
                                    <telerik:RadComboBoxItem Text="青剑湖" Value="205e21d3-eb57-4951-bd53-8fa3604af44f" Checked="true" />
                                    <telerik:RadComboBoxItem Text="苏州大学高教区" Value="eb63d66f-c1f3-4b7f-8128-be83a539a554" Checked="true" />
                                    <telerik:RadComboBoxItem Text="东部工业区" Value="44d8c029-7cd6-4a10-90c2-a614084810ad" Checked="true" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>

                        <td class="title" style="width: 80px; text-align: right;">开始时间:
                        </td>
                        <td class="content" style="width: 100px; text-align: right;">
                            <telerik:RadDatePicker runat="server" ID="dtBegin" Width="120px" OnSelectedDateChanged="dtBegin_SelectedDateChanged" AutoPostBack="true">
                                <Calendar runat="server" ID="Calendar1" EnableKeyboardNavigation="true">
                                </Calendar>
                            </telerik:RadDatePicker>
                        </td>
                        <td class="title" style="width: 80px; text-align: right;">截止时间:
                        </td>
                        <td class="content" style="width: 100px; text-align: left;">
                            <telerik:RadDatePicker runat="server" ID="dtEnd" Width="120px" OnSelectedDateChanged="dtEnd_SelectedDateChanged" AutoPostBack="true">
                                <Calendar ID="Calendar2" runat="server" EnableKeyboardNavigation="true">
                                </Calendar>
                            </telerik:RadDatePicker>
                        </td>
                        <td class="content" style="width: 150px; text-align: center;">文件是否存在<asp:CheckBox ID="cbxInsertService" runat="server" />
                        </td>
                        <td class="content" style="width: 150px; text-align: center;">是否解析成功<asp:CheckBox ID="cbxSuccess" runat="server" />
                        </td>
                        <td class="content" style="text-align: left; width: 100px">
                            <asp:ImageButton runat="server" ID="btnSearch" SkinID="ImgBtnSearch" OnClientClick="return OnClientClicking()" OnClick="btnSearch_Click" />
                        </td>
                    </tr>
                    <tr style="display: none;">
                        <td class="content" style="text-align: center; width: 100px">
                            <asp:ImageButton runat="server" ID="btnSave" SkinID="ImgBtnSave" OnClick="btnSave_Click" />
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="RadPane1" runat="server" Height="100%" Width="100%" Scrolling="None" BorderWidth="0">
                <telerik:RadGrid ID="gridQuxian" runat="server" GridLines="None" CssClass="RadGrid_Customer" Width="100%" Height="100%" BorderStyle="None" AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                    AllowMultiRowSelection="false" EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false" ShowStatusBar="false" BorderWidth="0" BorderSize="0"
                    AutoGenerateColumns="false" OnNeedDataSource="gridQuxian_NeedDataSource" OnItemDataBound="gridQuxian_ItemDataBound">
                    <MasterTableView GridLines="None" CommandItemDisplay="None" NoMasterRecordsText="没有数据">
                        <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" RefreshText="查询" ShowExportToExcelButton="false" ShowExportToWordButton="false" ShowExportToPdfButton="false" />
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="序号" UniqueName="Row"
                                HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%#Container.DataSetIndex + 1%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn HeaderText="站点" UniqueName="PointId" DataField="PointId" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="80px" ItemStyle-Width="80px" Visible="False">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="PointName" UniqueName="PointName" HeaderText="测点"
                                HeaderStyle-Width="100px" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Tstamp" UniqueName="Tstamp" HeaderText="时间" DataFormatString="{0:yyyy-MM-dd}"
                                HeaderStyle-Width="80px" ItemStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="文件是否存在" UniqueName="IsInsertService" DataField="IsInsertService" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="80px" ItemStyle-Width="800px">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="传入服务器时间" UniqueName="InsertServiceTime" DataField="InsertServiceTime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="120px" ItemStyle-Width="120px" Visible="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="是否解析成功" UniqueName="IsSuccess" DataField="IsSuccess" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="80px" ItemStyle-Width="80px">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="导入时间" UniqueName="InsertTime" DataField="InsertTime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="120px" ItemStyle-Width="120px" Visible="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="异常数据" UniqueName="ProblemData" DataField="ProblemData" HeaderStyle-Width="200px" ItemStyle-Width="200px"
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
        <asp:HiddenField ID="HiddenData" runat="server" Value="" />
        <asp:HiddenField ID="PointIDHidden" runat="server" Value="" />
        <asp:HiddenField ID="HiddenName" runat="server" Value="" />

    </form>
</body>
</html>
