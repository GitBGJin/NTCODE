<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DaoZhanLv.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.DataAnalyze.DaoZhanLv" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<%@ Register TagPrefix="CbxRsm" TagName="FactorCbxRsm" Src="~/Controls/FactorCbxRsm.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel="stylesheet" href="../../../Resources/CSS/pagination.css" />
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script src="../../../Resources/JavaScript/JQuery/jquery-1.9.0.js" type="text/javascript"></script>
            <script  src ="../../../Resources/JavaScript/ChartOperator/merge.js" type="text/javascript"></script>                        
            <script type="text/javascript">
                var isfirst = 1;//是否首次加载
                function OnClientClicking() {
                    var date1 = new Date();
                    var date2 = new Date();
                    date1 = $find("<%= dtpBegin.ClientID %>").get_selectedDate();
                    date2 = $find("<%= dtpEnd.ClientID %>").get_selectedDate();
                    if ((date1 == null) || (date2 == null)) {
                        alert("开始时间或者终止时间，不能为空!");                        
                        return false;
                    }
                    if (date1 > date2) {
                        alert("开始时间不能大于终止时间！");
                        return false;
                    } else {
                        return true;
                    }
                }
                //tab页切换时时间检查
                function OnClientSelectedIndexChanging(sender, args) {
                    var date1 = new Date();
                    var date2 = new Date();
                    date1 = $find("<%= dtpBegin.ClientID %>").get_selectedDate();
                    date2 = $find("<%= dtpEnd.ClientID %>").get_selectedDate();
                    if ((date1 == null) || (date2 == null)) {
                        alert("开始时间或者终止时间，不能为空！");
                        args.set_cancel(true);
                        return;
                    }
                    if (date1 > date2) {
                        alert("开始时间不能大于终止时间！");
                        args.set_cancel(true);
                        return;
                    } else {
                        return;
                    }

                }
                //Grid按钮行处理
                function gridRTB_ClientButtonClicking(sender, args) {
                    args.set_cancel(!OnClientClicking());
                }
                function onRequestStart(sender, args) {
                    if (args.EventArgument == "")
                        return;
                    if (args.EventArgument == 0 || args.EventArgument == 1 ||
                        args.get_eventTarget().indexOf("ExportToExcelButton") >= 0 ||
                            args.get_eventTarget().indexOf("ExportToWordButton") >= 0 ||
                            args.get_eventTarget().indexOf("ExportToCsvButton") >= 0) {
                        args.set_enableAjax(false);
                    }
                }

                function onRequestEnd(sender, args) {
                }

                //Chart图形切换
                function ChartTypeChanged(item) {
                    try {
                        var chartIframe = document.getElementsByName('chartIframe');                   
                        for (var i = 0; i < chartIframe.length; i++) {
                            document.getElementById(chartIframe[i].id).contentWindow.HighChartTypeChange(item);
                        }
                    } catch (e) {
                    }
                }

                //查看图表
                function TabSelected(sender, args) {
                    try {
                        var isfirst = document.getElementById("<%=FirstLoadChart.ClientID%>");
                        var text = $.trim(args._tab._element.innerText);
                        if (text == '图表') {
                            if (isfirst.value == "1") {
                                isfirst.value = 0;
                                try {
                                    ChartPager(0, defaultPagesize);//图表分页
                                }
                                catch (e) {
                                }                               
                                RefreshChart();                       
                            }
                        }
                    } catch (e) {                      
                    }
                }

                //图表刷新
                function RefreshChart() {
                    try {
                        var hiddenData = $("#HiddenData").val().split('|');
                        var height = parseInt(parseInt($("#pvChart").css("height")) - 40);
                        groupChartByPointid(hiddenData[0], "../Chart/ChartFrame.aspx", height);//以站点分组
                    } catch (e) {
                   
                    }               

                    //try {
                    //    var chartPage = document.getElementById("pvChart");
                    //    chartPage.children[0].contentWindow.InitChart();
                    //} catch (e) {
                    //}
                }
                function  GetImageDetail(MN,DT)
                {
                    //var oWnd = radopen("MyImage.aspx", "AlarmHandle");
                    //return false;
                    var oWnd = radopen("MyImage.aspx?MN=" + MN + "&DT=" + DT, "AlarmHandle");
                    return false;
                }
            </script>
        </telerik:RadCodeBlock>
        <input id="radGridColWidth" type="hidden" runat="server" value="120" />
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <asp:HiddenField ID="FirstLoadChart" Value="1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1"
            UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="grdPAnalyze">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdPAnalyze" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridRTB">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdPAnalyze" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="grdPAnalyze" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenData" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenChartType" />
                        <telerik:AjaxUpdatedControl ControlID="FirstLoadChart" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rbtnlType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rbtnlType" />
                        <telerik:AjaxUpdatedControl ControlID="dvProper" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dvPoints" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ChartType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ChartType" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenChartType" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
            <ClientEvents OnRequestStart="onRequestStart" OnResponseEnd="onRequestEnd" />
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" Width="100%"
            BorderWidth="0" BorderStyle="None" BorderSize="0">
            <telerik:RadPane ID="paneWhere" runat="server" Height="60px" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="width: 100%; height: 100%" cellspacing="1" cellpadding="0" class="Table_Customer"
                    border="0">
                    <tr>
                        <td class="title" style="width: 80px">运维小组：</td>
                        <td class="content">
                            <telerik:RadComboBox runat="server"  ID="rcbMaintenanceTeam"  Width="150px"  CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput"
                                    Localization-CheckAllString="选择全部">                    
                            </telerik:RadComboBox>
                        </td>
                        <td class="title" style="width: 80px; text-align: left;">开始时间：</td>
                        <td style="width: 5px; text-align: left; ">
                           <telerik:RadDateTimePicker ID="dtpBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy年-MM月-dd日"
                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                TimeView-HeaderText="小时" />
                        </td>
                      
                        <td style="width: 30px; text-align: left;">至：</td>
                        <td style="width: 50px; text-align: left;">
                            <telerik:RadDateTimePicker ID="dtpEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy年-MM月-dd日 "
                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                TimeView-HeaderText="小时" />
                        </td>
                        <td style="width: 450px; text-align: right;"></td>
                        <td class="content" align="right">
                            <asp:ImageButton ID="btnSearch" runat="server" OnClientClick="return OnClientClicking()" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
               <telerik:RadPane ID="paneTab" runat="server" Scrolling="None" Width="100%" Height="26px"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadTabStrip ID="tabStrip" runat="server" SelectedIndex="0" MultiPageID="multiPage" OnClientTabSelecting="OnClientSelectedIndexChanging"
                    CssClass="RadTabStrip_Customer" OnClientTabSelected="TabSelected">
                    <Tabs>
                        <telerik:RadTab Text="列表">
                        </telerik:RadTab>
                       <%-- <telerik:RadTab Text="图表">
                        </telerik:RadTab>--%>
                    </Tabs>
                </telerik:RadTabStrip>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadMultiPage ID="multiPage" runat="server" Width="100%" Height="100%" ScrollBars="Hidden" SelectedIndex="0">
                    <telerik:RadPageView ID="pvGrid" runat="server" Visible="true">
                        <telerik:RadGrid ID="grdPAnalyze" runat="server" GridLines="None" Height="100%" Width="100%"
                            AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false"
                            AutoGenerateColumns="false" AllowMultiRowSelection="false"
                            EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                            ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                            OnNeedDataSource="grid_NeedDataSource" OnItemDataBound="grid_ItemDataBound"
                            CssClass="RadGrid_Customer">                   
                            <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="Top" IsFilterItemExpanded="False"
                                InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据" DataKeyNames="MN,DT">
                                <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true" RefreshText="查询" ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                                <CommandItemTemplate>
                                    <%--<div style="position: relative;">
                                        <telerik:RadToolBar BorderWidth="0" ID="gridRTB" SkinID="Query" CssClass="RadToolBar_Customer" AutoPostBack="true"
                                            runat="server" Width="50%" OnButtonClick="gridRTB_ButtonClick" OnClientButtonClicking="gridRTB_ClientButtonClicking" />                                      
                                    </div>--%>
                                </CommandItemTemplate>
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="序号" UniqueName="rowNum" HeaderStyle-Width="50px" HeaderStyle-Height="50px"
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%#Container.DataSetIndex + 1%>                                          
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>                                                              
                                     <telerik:GridBoundColumn DataField="TeamName" HeaderText="运维小组" UniqueName="TeamName"  EmptyDataText = "--"
                                     HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                    <telerik:GridBoundColumn DataField="PointName" HeaderText="测点" UniqueName="PointName"  EmptyDataText = "--"   HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                    <telerik:GridBoundColumn DataField="DT" HeaderText="时间" UniqueName="DT"  EmptyDataText = "--"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  />
                                    <telerik:GridBoundColumn DataField="CodeNum" HeaderText="卡号" UniqueName="CodeNum"  EmptyDataText = "--"   HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                    <telerik:GridBoundColumn DataField="N1" HeaderText="刷卡次数" UniqueName="N1"  EmptyDataText = "--"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                    <telerik:GridBoundColumn DataField="N2" HeaderText="Pad签到次数" UniqueName="N2"  EmptyDataText = "--"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  />
                                    <telerik:GridTemplateColumn   HeaderText="图片浏览" UniqueName="图片浏览"  HeaderStyle-Width="100px" 
                                        HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" >
                                        <ItemTemplate>
                                             <input id="BtnUserInfo" type="button" value="查看图片"  title="查看图片"
                            onclick="GetImageDetail('<%#DataBinder.Eval(Container.DataItem,"MN")%>   ','<%#DataBinder.Eval(Container.DataItem,"DT")%> '); return false;">
                                          
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                                <PagerStyle FirstPageToolTip="首页" AlwaysVisible="true" Position="Bottom" NextPageToolTip="下页" LastPageToolTip="尾页" PrevPageToolTip="上页"
                                    PageSizeLabelText="显示记录数:" PageSizes="24 48 96" PagerTextFormat="跳转页面:{4}&amp;nbsp;显示 &lt;strong&gt;{2}&lt;/strong&gt; - &lt;strong&gt;{3}&lt;/strong&gt;条,共 &lt;strong&gt;{5}&lt;/strong&gt;条"></PagerStyle>                          
                            </MasterTableView>
                              <ClientSettings EnableRowHoverStyle="True" AllowColumnHide="True" AllowColumnsReorder="false"
            AllowDragToGroup="false" AllowRowHide="false" AllowRowsDragDrop="false">
            <Selecting AllowRowSelect="True" ></Selecting>
           </ClientSettings>
                            <CommandItemStyle Width="100%" />
                            <ClientSettings>
                                <Scrolling AllowScroll="True" EnableVirtualScrollPaging="false" UseStaticHeaders="True"
                                    SaveScrollPosition="true"></Scrolling>
                                    
                            </ClientSettings>
                        </telerik:RadGrid>
                    </telerik:RadPageView>
                    <telerik:RadPageView ID="pvChart" runat="server" Visible="true">
                        <div style="padding-top: 6px;">
                            <div style="float: right;">
                                <asp:RadioButtonList runat="server" ID="ChartType" AutoPostBack="true" RepeatDirection="Horizontal" RepeatColumns="3" OnSelectedIndexChanged="ChartType_SelectedIndexChanged">
                                    <asp:ListItem Text="折线图" Value="spline" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="柱形图" Value="column"></asp:ListItem>
                                    <asp:ListItem Text="点图" Value="scatter"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            <div style="clear: both">
                            </div>
                        </div>
                        <div id="ChartContainer">
                        </div>
                    </telerik:RadPageView>
                </telerik:RadMultiPage>
            </telerik:RadPane>
        </telerik:RadSplitter>
        <%--  隐藏域存放选中的因子，在Grid刷新后更新隐藏域数据--%> 
        <asp:HiddenField ID="HiddenData" runat="server" Value="" />
        <asp:HiddenField ID="AjaxURL" runat="server" Value="../ChartAjaxRequest/RealTimeData.ashx.ashx" />
        <asp:HiddenField ID="HiddenChartType" runat="server" Value="spline" />
         <telerik:RadWindowManager ID="RadWindowManager1" runat="server"  VisibleStatusbar="false"  Behaviors="Maximize,Move,Pin,Reload,Close"
        EnableShadow="true">
        <Windows>
            <telerik:RadWindow ID="AlarmHandle" runat="server"  Width="850px" Height="500px" ViewStateMode="Enabled"
                ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Localization-Cancel="取消"
                Localization-Close="关闭" Localization-Maximize="最大化" Localization-Minimize="最小化"
                Localization-Reload="刷新" Localization-PinOff="浮动" Localization-PinOn="固定" />
        </Windows>
    </telerik:RadWindowManager>
    </form>
</body>
</html>
