<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HourReportUpLoad_SZ.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Report.HourReportUpLoad_SZ" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>区、县数据上报</title>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script src="../../../Resources/JavaScript/JQuery/jquery-1.9.0.js"></script>
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
            <p style="text-align: center; vertical-align: middle; padding-top: 20%; font-weight: bold; font-size: 18px; color: #b4aa38;">数据上传中...</p>
        </div>
        <telerik:RadSplitter ID="RadSplitter1" runat="server" Orientation="Horizontal" Height="100%" Width="100%">
            <telerik:RadPane ID="ToolBarRadPane" runat="server" Height="100px" Width="100%" Scrolling="None" BorderWidth="0">
                <table class="Table_Customer" style="height: 100%; width: 100%">
                    <tr>
                        <td class="title" style="width: 100px;">上报模板：
                        </td>
                        <td class="title" colspan="4" style="text-align: left;">
                            <a href="../../../Files/区县上报/区县上报数据导入模板（苏州）.xls" style="color: red; font-weight: bolder; font-size: 16px;">区县上报数据导入模板（苏州）.xls</a>
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
                    </tr>
                    <tr>
                        <td class="title" style="width: 100px;">监测点：
                        </td>
                        <td class="content" style="width: 300px;">
                            <%--  <telerik:RadComboBox runat="server" ID="rcbPoint" CheckBoxes="true" Width="100%"
                                EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">--%>
                            <telerik:RadComboBox runat="server" ID="rcbPoint" Localization-CheckAllString="全选" Width="100%" CheckBoxes="true" Visible="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                                <Items>
                                    <telerik:RadComboBoxItem Text="张家港" Value="4296ce53-78d3-4741-9eda-6306e3e5b399" Checked="true" />
                                    <telerik:RadComboBoxItem Text="常熟" Value="f7444783-a425-411c-a54b-f9fed72ec72e" Checked="true" />
                                    <telerik:RadComboBoxItem Text="太仓" Value="d993d02f-fcc3-4ea6-b52b-9414fbd9b8e6" Checked="true" />
                                    <telerik:RadComboBoxItem Text="昆山" Value="636775d8-091d-4754-9ed2-cd9dfef1f6ab" Checked="true" />
                                    <telerik:RadComboBoxItem Text="吴江" Value="48d749e6-d07c-4764-8d50-50f170defe0b" Checked="true" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>

                        <td class="title" style="width: 80px; text-align: right;">开始时间:
                        </td>
                        <td class="content" style="width: 100px; text-align: right;">
                            <telerik:RadDatePicker runat="server" ID="dtBegin" Width="120px">
                                <Calendar runat="server" ID="Calendar1" EnableKeyboardNavigation="true">
                                </Calendar>
                            </telerik:RadDatePicker>
                        </td>
                        <td class="title" style="width: 80px; text-align: right;">截止时间:
                        </td>
                        <td class="content" style="width: 100px; text-align: left;">
                            <telerik:RadDatePicker runat="server" ID="dtEnd" Width="120px">
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

    </form>
</body>
</html>
