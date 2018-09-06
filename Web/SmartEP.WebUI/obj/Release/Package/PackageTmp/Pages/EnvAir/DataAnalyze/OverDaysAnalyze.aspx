﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OverDaysAnalyze.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.DataAnalyze.OverDaysAnalyze" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script type="text/javascript" src="../../../Resources/JavaScript/FrameJS.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script src="../../../Resources/JavaScript/PChart/jquery-1.8.3.min.js"></script>
            <script src="../../../Resources/JavaScript/PChart/highcharts.js"></script>
            <script src="../../../Resources/JavaScript/PChart/exporting.js"></script>
            <script src="../../../Resources/JavaScript/PChart/highcharts-zh_CN.js"></script>
            <%--<script src="../../../Resources/JavaScript/ChartOperator/merge.js"></script>--%>
            <script type="text/javascript">
                //$("document").ready(function () {
                //    InitGroupChart();

                //});
                var isfirst = 1;//是否首次加载
                function TabSelected(sender, args) {
                    try {
                        var isfirst = document.getElementById("<%=FirstLoadChart.ClientID%>");
                        var text = $.trim(args._tab._element.innerText);
                        if (text == '图表') {
                            if (isfirst.value == "1") {
                                isfirst.value = 1;
                                InitGroupChart();
                            }
                        }
                    } catch (e) {
                    }
                }
                function InitGroupChart() {
                    
                    var hdvalue = eval("(" + HiddenData.value + ")");
                    var hdport = eval("(" + HiddenPortId.value + ")");
                    //var chart = document.getElementById("container");
                    HiddenData.value = "";
                    HiddenPortId.value = "";
                    console.log(hdvalue);
                    //$(function () {
                    var chart = new Highcharts.Chart("container", {
                        //chart: {
                        //    plotBackgroundColor: null,
                        //    plotBorderWidth: null,
                        //    plotShadow: false
                        //},
                        //title: {
                        //    text: '达标天数比例饼图'
                        //},
                        //tooltip: {
                        //    headerFormat: '{series.name}<br>',
                        //    pointFormat: '{point.name}: <b>{point.percentage:.1f}%</b>'
                        //},
                        //plotOptions: {
                        //    pie: {
                        //        allowPointSelect: true,
                        //        cursor: 'pointer',
                        //        dataLabels: {
                        //            enabled: true,
                        //            format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                        //            style: {
                        //                color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                        //            }
                        //        }
                        //    }
                        //},
                        //series: [{
                        //    type: 'pie',
                        //    name: '达标天数占比',
                        //    data: hdvalue
                        //}]
                        ////堆叠柱形图
                        //chart: {
                        //    type: 'column'
                        //},
                        //title: {
                        //    text: '达标天数占比'
                        //},
                        //xAxis: {
                        //    //站点名称
                        //    categories: hdport
                        //},
                        //yAxis: {
                        //    min: 0,
                        //    title: {
                        //        text: '各站点达标天数占比'
                        //    },
                        //    stackLabels: {
                        //        enabled: true,
                        //        style: {
                        //            fontWeight: 'bold',
                        //            color: (Highcharts.theme && Highcharts.theme.textColor) || 'gray'
                        //        }
                        //    }
                        //},
                        //legend: {
                        //    align: 'right',
                        //    x: -30,
                        //    verticalAlign: 'top',
                        //    y: 25,
                        //    floating: true,
                        //    backgroundColor: (Highcharts.theme && Highcharts.theme.background2) || 'white',
                        //    borderColor: '#CCC',
                        //    borderWidth: 1,
                        //    shadow: false
                        //},
                        //tooltip: {
                        //    formatter: function () {
                        //        return '<b>' + this.x + '</b><br/>' +
                        //            this.series.name + ': ' + this.y + '<br/>' +
                        //            '总天数: ' + this.point.stackTotal;
                        //    }
                        //},
                        //plotOptions: {
                        //    column: {
                        //        stacking: 'normal',
                        //        pointWidth: 40,
                        //        dataLabels: {
                        //            enabled: true,
                        //            color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'white',
                        //            style: {
                        //                textShadow: '0 0 3px black'
                        //            }
                        //        }
                        //    }
                        //},
                        //series: hdvalue

                        title: {
                            text: '达标天数占比'
                        },
                        xAxis: {
                            categories: hdport
                        },
                        yAxis: {
                            min: 0,
                            title: {
                                text: '各站点达标天数占比'
                            },
                            stackLabels: {
                                enabled: true,
                                style: {
                                    fontWeight: 'bold',
                                    color: (Highcharts.theme && Highcharts.theme.textColor) || 'gray'
                                }
                            }
                        },
                        plotOptions: {
                            series: {
                                stacking: 'normal',
                                pointWidth: 30,
                                dataLabels: {
                                                enabled: true,
                                                color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'white',
                                                style: {
                                                    textShadow: '0 0 3px black'
                                                }
                                            }
                            }
                        },
                        labels: {
                            items: [{
                                html: '各站点达标率',
                                style: {
                                    left: '100px',
                                    top: '18px',
                                    color: (Highcharts.theme && Highcharts.theme.textColor) || 'black'
                                }
                            }]
                        },
                        legend: {
                            align: 'right',
                            x: -30,
                            verticalAlign: 'top',
                            y: 25,
                            floating: true,
                            backgroundColor: (Highcharts.theme && Highcharts.theme.background2) || 'white',
                            borderColor: '#CCC',
                            borderWidth: 1,
                            shadow: false
                        },
                        tooltip: {
                            formatter: function () {
                                return '<b>' + this.x + '</b><br/>' +
                                    this.series.name + ': ' + this.y + '<br/>' +
                                    '总天数: ' + this.point.stackTotal;
                            }
                        },
                        series: hdvalue
                    });
                    //});
                    };
                
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
                function RowClick(pointID, portName, type, starttime, endtime) {
                    var moduleGuide = "bc1e261c-3d83-4acd-9299-3a0b81beb7a1";
                    OpenFineUIWindow(moduleGuide, "Pages/EnvAir/Report/AirQualityDayReport.aspx?portId=" + pointID + "&portName="
                        + portName + "&type=" + type + "&starttime=" + starttime + "&endtime=" + endtime + "&orderBy=AQIValue", "空气质量日报")
                }
                function RowClick2(pointID, portName, type, starttime, endtime, days) {
                    var moduleGuide = "bc1e261c-3d83-4acd-9299-3a0b81beb7a1";
                    OpenFineUIWindow(moduleGuide, "Pages/EnvAir/Report/AirQualityDayReport.aspx?portId=" + pointID + "&portName="
                        + portName + "&type=" + type + "&starttime=" + starttime + "&endtime=" + endtime + "&days=" + days + "&orderBy=AQIValue", "空气质量日报")
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

            </script>
        </telerik:RadCodeBlock>
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <asp:HiddenField ID="FirstLoadChart" Value="1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1"
            UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="gridOverDays">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridOverDays" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridRTB">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridOverDays" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rbtnlType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rbtnlType" />
                        <telerik:AjaxUpdatedControl ControlID="dvPoints" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rdlType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rdlType" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dvCb" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dvFatext" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="rcbFactors" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridOverDays" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenData"/>
                        <telerik:AjaxUpdatedControl ControlID="HiddenPortId"/>
                        <telerik:AjaxUpdatedControl ControlID="container" LoadingPanelID="RadAjaxLoadingPanel1"/>
                        <telerik:AjaxUpdatedControl ControlID="ChartContainer" LoadingPanelID="RadAjaxLoadingPanel1"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="tabStrip">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="FirstLoadChart" />
                        <telerik:AjaxUpdatedControl ControlID="container" LoadingPanelID="RadAjaxLoadingPanel1"/>
                        <telerik:AjaxUpdatedControl ControlID="ChartContainer" LoadingPanelID="RadAjaxLoadingPanel1"/>
                        <telerik:AjaxUpdatedControl ControlID="HiddenData"/>
                        <telerik:AjaxUpdatedControl ControlID="HiddenPortId"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
            <ClientEvents OnRequestStart="onRequestStart" OnResponseEnd="onRequestEnd" />
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" Width="100%"
            BorderWidth="0" BorderStyle="None" BorderSize="0">
            <telerik:RadPane ID="paneWhere" runat="server" Height="90px" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="height: 100%" class="Table_Customer"
                    border="0">
                    <tr>
                        <td class="title" style="width: 80px; text-align: center;">查询类型
                        </td>
                        <td class="content">
                            <asp:RadioButtonList ID="rbtnlType" runat="server"  RepeatLayout="Flow" RepeatColumns="5">
                                <asp:ListItem Text="区域" Value="CityProper" ></asp:ListItem>
                                <asp:ListItem Text="测点" Value="Port" Selected="True"></asp:ListItem>

                            </asp:RadioButtonList>
                        </td>
                        <td class="title" style="width: 100px; text-align: center;">站点
                        </td>
                        <td class="content" style="width: auto;">
                            <div runat="server" id="dvPoints">
                                <CbxRsm:PointCbxRsm runat="server" DefaultIPointMode="Region"  ApplicationType="Air" CbxWidth="350" CbxHeight="350" MultiSelected="true" DropDownWidth="520" ID="pointCbxRsm"></CbxRsm:PointCbxRsm>
                            </div>
                        </td>
                        <td class="title" style="width: 80px">
                            <div id="dvFatext" runat="server">监测因子</div>
                        </td>
                        <td class="content">
                            <telerik:RadComboBox runat="server" ID="rcbFactors" Localization-CheckAllString="全选" Width="150px" CheckBoxes="true" Visible="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                                <Items>
                                    <telerik:RadComboBoxItem Text="PM2.5" Value="PM25" Checked="true" />
                                    <telerik:RadComboBoxItem Text="PM10" Value="PM10" Checked="true" />
                                    <telerik:RadComboBoxItem Text="二氧化氮" Value="NO2" Checked="true" />
                                    <telerik:RadComboBoxItem Text="二氧化硫" Value="SO2" Checked="true" />
                                    <telerik:RadComboBoxItem Text="一氧化碳" Value="CO" Checked="true" />
                                    <%--<telerik:RadComboBoxItem Text="臭氧1小时" Value="RecentoneHoursO3" Checked="true" />--%>
                                    <telerik:RadComboBoxItem Text="臭氧8小时" Value="Recent8HoursO3" Checked="true" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                        <td class="content" align="left" rowspan="2">
                            <asp:ImageButton ID="btnSearch" runat="server" OnClientClick="return OnClientClicking();" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                        </td>
                    </tr>
                    <tr>
                        <td class="title" style="width: 80px; text-align: center;">统计类型</td>
                        <td class="content" style="width: auto;">
                            <table>
                                <tr>
                                    <td>
                                        <telerik:RadDropDownList ID="rdlType" runat="server" Width="80px" AutoPostBack="true" OnSelectedIndexChanged="rdlType_SelectedIndexChanged">
                                            <Items>
                                                <telerik:DropDownListItem runat="server" Selected="True" Value="2" Text="质量类别" />
                                                <telerik:DropDownListItem runat="server" Text="因子" Value="1" />
                                            </Items>
                                        </telerik:RadDropDownList>
                                    </td>
                                    <td>
                                        <div runat="server" id="dvCb">
                                            <asp:CheckBoxList ID="cbList" runat="server" RepeatColumns="3">
                                                <asp:ListItem Value="1" Text="超标" Selected="True"></asp:ListItem>
                                                <asp:ListItem Value="0" Text="达标"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="首要污染物"></asp:ListItem>
                                            </asp:CheckBoxList>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td class="title" style="width: 80px; text-align: center;">开始时间
                        </td>
                        <td class="content" style="width: 400px;">
                            <%--日--%>
                            <div runat="server" id="dbtDay">
                                <table>
                                    <tr>
                                        <td>
                                            <telerik:RadDatePicker ID="dayBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd"
                                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                                TimeView-HeaderText="小时" Calendar-FastNavigationStep="12" />
                                        </td>
                                        <td>结束时间
                                        </td>
                                        <td>
                                            <telerik:RadDatePicker ID="dayEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd"
                                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                                TimeView-HeaderText="小时" Calendar-FastNavigationStep="12" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                        <td class="title" style="width: 80px">
                            <div id="Div1" runat="server">比对年份</div>
                        </td>
                        <td class="content">
                            <telerik:RadComboBox runat="server" ID="compareYear" Localization-CheckAllString="全选" Width="150px" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneTab" runat="server" Scrolling="None" Width="100%" Height="26px"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadTabStrip ID="tabStrip" runat="server" SelectedIndex="0" MultiPageID="multiPage"
                    CssClass="RadTabStrip_Customer" OnClientTabSelected="TabSelected">
                    <Tabs>
                        <telerik:RadTab Text="列表">
                        </telerik:RadTab>
                        <telerik:RadTab Text="图表">
                        </telerik:RadTab>
                    </Tabs>
                </telerik:RadTabStrip>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadMultiPage ID="multiPage" runat="server" Width="100%" Height="100%" ScrollBars="Hidden" SelectedIndex="0">
                <telerik:RadPageView ID="grvGrid" runat="server" Visible="true">
                <telerik:RadGrid ID="gridOverDays" runat="server" GridLines="None" Height="100%" Width="100%"
                    AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false"
                    AutoGenerateColumns="true" AllowMultiRowSelection="false"
                    EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                    ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                    OnNeedDataSource="gridOverDays_NeedDataSource" OnItemDataBound="gridOverDays_ItemDataBound" OnColumnCreated="gridOverDays_ColumnCreated"
                    CssClass="RadGrid_Customer">
                    <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
                    <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="Top" IsFilterItemExpanded="False"
                        InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                        <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true" RefreshText="查询" ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                        <CommandItemTemplate>
                            <div style="position: relative;">
                            <telerik:RadToolBar BorderWidth="0" ID="gridRTB" SkinID="Query" CssClass="RadToolBar_Customer" AutoPostBack="true"
                                runat="server" Width="50%" OnButtonClick="gridRTB_ButtonClick" OnClientButtonClicking="gridRTB_ClientButtonClicking" />
                            <div style="position: absolute; right: 10px; top: 10px;">
                                            <b>达标天数指优、良天数，超标天数指轻度污染、中度污染、重度污染、严重污染天数，无效天数指AQI无效天数</b>
                                        </div>
                                </div>
                        </CommandItemTemplate>
                        <ColumnGroups>
                            <telerik:GridColumnGroup Name="OverDays" HeaderText="超标天数"
                                HeaderStyle-HorizontalAlign="Center">
                            </telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="StandDays" HeaderText="达标天数"
                                HeaderStyle-HorizontalAlign="Center">
                            </telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="PrimaryDays" HeaderText="首要污染物天数"
                                HeaderStyle-HorizontalAlign="Center">
                            </telerik:GridColumnGroup>
                        </ColumnGroups>

                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="序号" UniqueName="rowNum" HeaderStyle-Width="50px" HeaderStyle-Height="50px"
                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%#Container.DataSetIndex + 1%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle FirstPageToolTip="首页" AlwaysVisible="true" Position="Bottom" NextPageToolTip="下页" LastPageToolTip="尾页" PrevPageToolTip="上页"
                            PageSizeLabelText="显示记录数:" PageSizes="24 48 96" PagerTextFormat="跳转页面:{4}&amp;nbsp;显示 &lt;strong&gt;{2}&lt;/strong&gt; - &lt;strong&gt;{3}&lt;/strong&gt;条,共 &lt;strong&gt;{5}&lt;/strong&gt;条"></PagerStyle>
                    </MasterTableView>
                    <CommandItemStyle Width="100%" />
                    <ClientSettings>
                        <Scrolling AllowScroll="True" EnableVirtualScrollPaging="false" UseStaticHeaders="True"
                            SaveScrollPosition="false"></Scrolling>
                    </ClientSettings>
                </telerik:RadGrid>
                </telerik:RadPageView>
                <telerik:RadPageView ID="pvChart" runat="server" Visible="true">
                    <div style="padding-top: 6px;">
                        <div id="container" style="min-width: 310px; max-width: 1000px; height: 500px; margin: 0 auto">
                        </div>
                    </div>
                </telerik:RadPageView>
                </telerik:RadMultiPage>
            </telerik:RadPane>
        </telerik:RadSplitter>
        <asp:HiddenField ID="HiddenData" runat="server" Value="" />
        <asp:HiddenField ID="HiddenPortId" runat="server" Value="" />
    </form>
</body>
</html>
