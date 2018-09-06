﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LadarBMPThermodynamic.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.SuperStation.LadarBMPThermodynamic" %>


<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script src="../../../Resources/JavaScript/Ladar/jquery-1.9.0.min.js"></script>
    <script src="../../../Resources/JavaScript/Ladar/highcharts.js"></script>
    <script src="../../../Resources/JavaScript/Ladar/heatmap.js"></script>
    <script src="../../../Resources/JavaScript/HighCharts/exporting.js"></script>
    <script src="../../../Resources/JavaScript/Ladar/ChartHeatmap.js"></script>
    <script src="../../../Resources/JavaScript/Polary/highcharts-zh_CN.js"></script>
    <script src="../../../Resources/JavaScript/FrameJGLD.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <%--<script type="text/javascript" src="../../../Resources/JavaScript/JQuery/jquery-1.9.0.js"></script>--%>
            <script type="text/javascript" src="../../../Resources/JavaScript/ChartOperator/merge.js"></script>
            
            <script type="text/javascript">
                
                function OnClientClicking() {
                    
                    var date1 = new Date();
                    var date2 = new Date();
                    if ($find("<%= dtpBegin.ClientID %>") != null && $find("<%= dtpEnd.ClientID %>") != null) {
                                date1 = $find("<%= dtpBegin.ClientID %>").get_selectedDate();
                                date2 = $find("<%= dtpEnd.ClientID %>").get_selectedDate();
                                if ((date1 == null) || (date2 == null)) {
                                    alert("开始时间或者终止时间，不能为空！");
                                    return false;
                                }
                                if (date1 > date2) {
                                    alert("开始时间不能大于终止时间！");
                                    return false;
                                }
                                else {
                                    return true;
                                }
                            }
                }
                //function ValueChanging() {
                //    var value1 = 
                //    var value2 =
                //    if(value1>value2 || value1==value2){
                //        alert("色标下限不能大于或等于上限！");
                //        return false;
                //   }
                //    else {
                //        return true;
                //    }
                //}
                //控制时间范围
                function BeginDateChanging(sender, args) {
                    
                    var beginTime = new Date(Date.parse(args._newValue.replace(/-/g,"/")));
                    if (beginTime == null) {
                        alert("时间不能为空！");
                        args._cancel = true;
                    } else {
                        var endTime = $find("<%= dtpEnd.ClientID %>").get_selectedDate();
                        if (beginTime > endTime) {
                            //alert("开始时间不能大于结束时间！");
                            $find("<%= dtpEnd.ClientID %>").set_selectedDate(beginTime);
                        }
                    }
                }

                //控制时间范围
                function EndDateChanging(sender, args) {
                    
                    var endTime = new Date(Date.parse(args._newValue.replace(/-/g, "/")));
                    if (endTime == null) {
                        alert("时间不能为空！");
                        args._cancel = true;
                    } else {
                        var beginTime = $find("<%= dtpBegin.ClientID %>").get_selectedDate();
                    if (beginTime > endTime) {
                        //alert("开始时间不能大于结束时间！");
                        $find("<%= dtpBegin.ClientID %>").set_selectedDate(endTime);
                //args._cancel = true;
            }
        }
    }
                //function ClientButtonClicking(sender, args) {
                //    var uri = "../../EQMSMisNT/TableInfo/ListQuery.aspx?TableRowGuid=38b11663-5d1b-4e94-bb89-614054d88d15";
                //    var oWindow = window.radopen(encodeURI(uri), "ConfigOfflineDialog");
                //}
                //时间选择事件
                function BeginDateSelected(sender, args) {
                    var beginTime = $find("<%= dtpBegin.ClientID %>").get_selectedDate();
                    var endTime = $find("<%= dtpEnd.ClientID %>").get_selectedDate();
                    var date = endTime.getTime() - beginTime.getTime()  //时间差的毫秒数                       
                    var days = Math.floor(date / (96 * 3600 * 1000)) //计算出相差天数
                    var auditDays = "<%=GetAuditDays()%>";
                    if (auditDays != -1 && days >= auditDays) {
                        //alert("时间范围不能超过" + auditDays + "天");
                        //args._cancel = true;
                        $find("<%= dtpEnd.ClientID %>").set_selectedDate(new Date(beginTime.getTime() + 96 * 60 * 60 * 1000));
                    }
                    //LoadingData();//加载Echarts图表数据
                }

                //时间选择事件
                function EndDateSelected(sender, args) {
                    var beginTime = $find("<%= dtpBegin.ClientID %>").get_selectedDate();
                    var endTime = $find("<%= dtpEnd.ClientID %>").get_selectedDate();
                    var date = endTime.getTime() - beginTime.getTime()  //时间差的毫秒数                       
                    var days = Math.floor(date / (96 * 3600 * 1000)) //计算出相差天数
                    var auditDays = "<%=GetAuditDays()%>";
                    if (auditDays != -1 && days >= auditDays) {
                        $find("<%= dtpBegin.ClientID %>").set_selectedDate(new Date(endTime.getTime() - 96 * 60 * 60 * 1000));
                    }
                    //LoadingData();//加载Echarts图表数据
                }
                var isfirst = 1;//是否首次加载
                function TabSelected(sender, args) {
                    
                    try {
                        //var isfirst = document.getElementById("<%=FirstLoadChart.ClientID%>");
                        //var text = $.trim(args._tab._element.innerText);
                        //if (text == '图表') {
                        //    if (isfirst.value == "1") {
                        //        isfirst.value = 0;
                        //        //GetChart();
                        //        CreatCharts();
                        //    }
                        //}
                       
                    } catch (e) {
                    }
                }

                function CreatCharts() {
                    $('#ChartContainer').html("");
                    //获取隐藏域的值
                    //var strJSON = HiddenData.value;
                    var strJSON = document.getElementById("HiddenData").value;
                    //console.log(strJSON);
                    var quality = document.getElementById("Quality").value;
                    var dtStart = document.getElementById("DtStart").value;
                    var dtEnd = document.getElementById("DtEnd").value;
                    var stopChart = document.getElementById("stopChart").value;
                    var hdMin = document.getElementById("hdMin").value;
                    var hdMax = document.getElementById("hdMax").value;
                    hdMin.value = "";
                    hdMax.value = "";
                    //console.log(Border);
                    //console.log(Time);
                    document.getElementById("HiddenData").value = "";
                    if (quality != "height") {
                        if (quality == "extin532")
                        {
                            ajaxHighChart("激光雷达", "消光系数532", strJSON, "ChartContainer", "时间", "Km", "值",hdMin,hdMax);
                        } else if (quality == "extin355"){
                            ajaxHighChart("激光雷达", "消光系数355", strJSON, "ChartContainer", "时间", "Km", "值", hdMin, hdMax);
                        } else if (quality == "depol") {
                            ajaxHighChart("激光雷达", "退偏图", strJSON, "ChartContainer", "时间", "Km", "值", hdMin, hdMax);
                        }
                        $("#ChartContainer").css("overflow-y", "auto");
                        $("#ChartContainer").height("500px");//设置图表Iframe的高度
                        $("#ChartContainer").width("600px");//设置图表Iframe的宽度
                        $("#ChartContainer").css("margin", "0 auto");
                    }
                    else
                    {
                        var Border = document.getElementById("hdBorder").value;
                        var Time = document.getElementById("hdTime").value;
                        var count = document.getElementById("hdCount").value;
                        console.log(count);
                        var b = eval("(" + Border + ")");
                        var c = eval("(" + Time + ")");
                        var chart = new Highcharts.Chart('ChartContainer', {
                            title: {
                                text: '激光雷达',
                                x: -20
                            },
                            subtitle: {
                                text: '边界层高度',
                                x: -20
                            },
                            xAxis: {
                                //labels: { 
                                //    formatter: function() { 
                        
                                //        return Highcharts.dateFormat('%Y-%m-%d', this.value);
                                //    } 
                                //} 
                                type: Time.type,
                                tickInterval: Math.ceil(count / 3),
                                categories: c,
                                showLastLabel: true,
                                dateTimeLabelFormats: {
                            day: '%m/%d %H时',
                            hour: '%m/%d %H时',
                            month: '%m/%d',
                            week: '%m-%d',

                            second: '%d日%H点',
                            minute: '%d日%H点'
                        }
                            },
                            yAxis: {
                                title: {
                                    text: '边界层高度 (Km)'
                                },
                                plotLines: [{
                                    value: 0,
                                    width: 1,
                                    color: '#808080'
                                }]
                            },
                            tooltip: {
                                valueSuffix: 'Km'
                            },
                            legend: {
                                layout: 'vertical',
                                align: 'right',
                                verticalAlign: 'middle',
                                borderWidth: 0
                            },
                            series: [{
                                name: '边界层高度',
                                data: b
                            }
                            ]
                        });
                        //console.log(Border);
                        //console.log(Time);
                    }
                    
                }
                //根据类型获取图表显示方式
                function GetChart() {
                    var showType = $("#ShowType").find("[checked]")[0].defaultValue;
                    if (showType == '合并')
                        InitTogetherChart();
                    else
                        InitGroupChart();
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
//新增tab
function MoveMaintain(path, title) {
    OpenFineUIWindow(Math.random(), path, title);
    return false;
}

            </script>
            <%--<a href=""></a>--%>
             
        </telerik:RadCodeBlock>
        <input id="radGridColWidth" type="hidden" runat="server" value="120" />
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"  />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <asp:HiddenField ID="FirstLoadChart" Value="1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1"
            UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
               
                <telerik:AjaxSetting AjaxControlID="gridAudit">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridAudit" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ChartContainer">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ChartContainer" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridRTB">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridAudit" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridAudit" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenData" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenChartType" />
                        <telerik:AjaxUpdatedControl ControlID="hdHeavyMetalMonitor" />
                        <telerik:AjaxUpdatedControl ControlID="hdBorder" />
                        <telerik:AjaxUpdatedControl ControlID="hdTime" />
                        <telerik:AjaxUpdatedControl ControlID="hdMin" />
                        <telerik:AjaxUpdatedControl ControlID="hdMax" />
                        <telerik:AjaxUpdatedControl ControlID="first" />
                         <telerik:AjaxUpdatedControl ControlID="divImg" />
                        <telerik:AjaxUpdatedControl ControlID="hdCount" />
                        <telerik:AjaxUpdatedControl ControlID="stopChart" />
                        
                        <telerik:AjaxUpdatedControl ControlID="ChartContainer" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="FirstLoadChart" />
                        <telerik:AjaxUpdatedControl ControlID="hdPointName" />
                        <telerik:AjaxUpdatedControl ControlID="Quality" />
                        <telerik:AjaxUpdatedControl ControlID="DtStart" />
                        <telerik:AjaxUpdatedControl ControlID="DtEnd" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <%--<telerik:AjaxSetting AjaxControlID="radlDataType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="radlDataType" />
                        <telerik:AjaxUpdatedControl ControlID="dbtHour" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtDay" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtSeason" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtMonth" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtYear" LoadingPanelID="RadAjaxLoadingPanel1" />

                        <telerik:AjaxUpdatedControl ControlID="dbtWeek" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="container" />
                    </UpdatedControls>
                </telerik:AjaxSetting>--%>
                <%--<telerik:AjaxSetting AjaxControlID="ddlDataSource">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ddlDataSource" />
                    </UpdatedControls>
                    </telerik:AjaxSetting>--%>
                <telerik:AjaxSetting AjaxControlID="dtpBegin">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="dtpBegin" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dtpEnd" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="dtpEnd">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="dtpEnd" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dtpBegin" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
                <%--<telerik:AjaxSetting AjaxControlID="timer">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridAudit" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dtpEnd" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dtpBegin" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenData"/>
                        <telerik:AjaxUpdatedControl ControlID="Quality"/>
                        <telerik:AjaxUpdatedControl ControlID="DtStart" />
                        <telerik:AjaxUpdatedControl ControlID="DtEnd" />
                        <telerik:AjaxUpdatedControl ControlID="hdBorder" />
                        <telerik:AjaxUpdatedControl ControlID="hdTime" />
                         <telerik:AjaxUpdatedControl ControlID="divImg" />
                        <telerik:AjaxUpdatedControl ControlID="first" />
                        <telerik:AjaxUpdatedControl ControlID="hdCount" />
                        <telerik:AjaxUpdatedControl ControlID="stopChart" />
                        <telerik:AjaxUpdatedControl ControlID="ChartContainer" LoadingPanelID="RadAjaxLoadingPanel1" />
                        
                    </UpdatedControls>
                </telerik:AjaxSetting>--%>

                <%--<telerik:AjaxSetting AjaxControlID="tabStrip">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="FirstLoadChart" />
                        <telerik:AjaxUpdatedControl ControlID="ChartContainer" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenData" />
                    </UpdatedControls>
                </telerik:AjaxSetting>--%>
                <telerik:AjaxSetting AjaxControlID="ChartType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ChartType" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
            <ClientEvents OnRequestStart="onRequestStart" OnResponseEnd="onRequestEnd" />
        </telerik:RadAjaxManager>
        <%--<telerik:RadSplitter ID="RadSplitter1" runat="server" Orientation="Horizontal" Height="100%" Width="100%"
            BorderWidth="0" BorderStyle="None" BorderSize="0" >
        
            </telerik:RadSplitter>--%>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" Width="100%"
            BorderWidth="0" BorderStyle="None" BorderSize="0" >
            <telerik:RadPane ID="paneWhere" runat="server" Height="80px" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="width: 100%; height: 100%" cellspacing="1" cellpadding="0" class="Table_Customer"
                    border="0">
                    <tr>
                        <td class="title" style="width: 80px" >数据来源:
                                    </td>
                                    <td class="content" style="width: 120px;">
                                        <telerik:RadDropDownList ID="ddlDataSource" runat="server" Width="100px"  Visible="true">
                                            <Items>
                                                <telerik:DropDownListItem  Text="消光系数532" Value="extin532" Selected="true"/>
                                                <telerik:DropDownListItem  Text="消光系数355" Value="extin355" />
                                                <telerik:DropDownListItem  Text="边界层高度" Value="height" />
                                                <telerik:DropDownListItem  Text="退偏图" Value="depol" />
                                            </Items>
                                        </telerik:RadDropDownList>
                                    </td>
                        
                        
                        <td class="title" style="width: 80px; ">开始时间:
                        </td>
                        <td class="content" style="width: 400px;">
                            <div runat="server" id="dtpHour" visible="true">
                            <telerik:RadDateTimePicker ID="dtpBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd HH:mm" 
                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" 
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"  ClientEvents-OnDateSelected="BeginDateSelected"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"  DateInput-ClientEvents-OnValueChanging="BeginDateChanging"
                                TimeView-HeaderText="小时" Calendar-FastNavigationStep="12" />
                                结束时间：
                            <telerik:RadDateTimePicker ID="dtpEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd HH:mm"
                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" ClientEvents-OnDateSelected="EndDateSelected"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"  
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"  DateInput-ClientEvents-OnValueChanging="EndDateChanging"
                                TimeView-HeaderText="小时" Calendar-FastNavigationStep="12" />
                            </div>
                        </td>
                        <%--<td class="title" style="width: 80px;">色标下限
                        </td>
                        <td class="content" style="width: 80px;">
                            <telerik:RadNumericTextBox ID="Min" Width="60px" Type="Number" MinValue="0" MaxValue="5" Value="0" runat="server" ClientEvents-OnValueChanged="ValueChanging"></telerik:RadNumericTextBox>
                        </td>
                        <td class="title" style="width: 80px;">色标上限
                        </td>
                        <td class="content" style="width: 80px;">
                             <telerik:RadNumericTextBox ID="Max" Width="60px" Type="Number" MinValue="0" MaxValue="5" Value="1" runat="server" ClientEvents-OnValueChanged="ValueChanging"></telerik:RadNumericTextBox>
                        </td>--%>
                        <td class="content" align="right" rowspan="1">
<%--                            <asp:ImageButton ID="btnSearch" runat="server" OnClientClicking="return OnClientClicking()"  OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />--%>
                            <asp:ImageButton ID="btnSearch" runat="server"  OnClick="btnSearch_Click" SkinID="ImgBtnSearch" OnClientClick="return OnClientClicking()" />
                        </td>
                        <%--<td>
                            <telerik:RadButton ID="RadButton1" runat="server" BackColor="#1984CA" AutoPostBack="false" ForeColor="White"  OnClientClicking="ClientButtonClicking" Text="标记位说明">
                                                <ContentTemplate>
                                                        <asp:Label runat="server" ID="Label4" ForeColor="White" Text="标记位说明"></asp:Label>
                                                </ContentTemplate>
                                        </telerik:RadButton>
                        </td>--%>
                        <td>
                            <a style="text-decoration:none" href="#" onclick="MoveMaintain('http://218.91.209.251:1117///EQMSMisNT/TableInfo/ListQuery.aspx?TableRowGuid=38b11663-5d1b-4e94-bb89-614054d88d15&Token=3F39B8DB7964E63723B489567A263463231EB91F2C2E39BC0DCBB5798A2DEA64F658521A5B3AC1AA91A59BB9E500497C','激光雷达色标及高度配置')">激光雷达色标及高度配置>></a>
                        </td>
                        <td>
                             <b>目前默认两天加载图片,支持最大<br/>选择四天数据画图,但画图速度会变慢</b>
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneTab" runat="server" Scrolling="None" Width="100%" Height="26px"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadTabStrip ID="tabStrip" runat="server" SelectedIndex="0" MultiPageID="multiPage" 
                    CssClass="RadTabStrip_Customer" OnClientTabSelected="" OnTabClick="tabStrip_TabClick">
                    <Tabs>
                        <telerik:RadTab  Text="图谱">
                        </telerik:RadTab>
                        <telerik:RadTab Text="列表">
                        </telerik:RadTab>
                        
                    </Tabs>
                </telerik:RadTabStrip>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None" 
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadMultiPage ID="multiPage" runat="server" Width="100%" Height="100%" ScrollBars="Hidden" SelectedIndex="0">
                    <telerik:RadPageView ID="pvChart" runat="server" Visible="true" Width="100%" >
                        <div style="padding-top: 6px;text-align:center" >
                            <div id="ChartContainer" runat="server" style="width: 500px; height: 500px;text-align:center">
                                
                            </div>
                            <img  visible="false" runat="server" id="divImg" style=" height:500px;text-align:center" src="http://www.sjxh.org/"/>
                        </div>
                    </telerik:RadPageView>
                    <telerik:RadPageView ID="grvGrid" runat="server" Visible="true">
                        <telerik:RadGrid ID="gridAudit" runat="server" GridLines="None" Height="100%" Width="100%"
                            AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                            AutoGenerateColumns="true" AllowMultiRowSelection="false"
                            EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                            ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                            OnNeedDataSource="gridAudit_NeedDataSource" OnItemDataBound="gridAudit_ItemDataBound" OnColumnCreated="gridAudit_ColumnCreated"
                            CssClass="RadGrid_Customer">
                            <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
                            <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="Top" IsFilterItemExpanded="False"
                                InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                                <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true" RefreshText="查询" ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                                <CommandItemTemplate>
                                    <div style="position: relative;">
                                    <telerik:RadToolBar BorderWidth="0" ID="gridRTB" SkinID="Query" CssClass="RadToolBar_Customer" AutoPostBack="true" Visible="false"
                                        runat="server" Width="50%" OnButtonClick="gridRTB_ButtonClick" />
                                        </div>
                                </CommandItemTemplate>
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
                                <Scrolling AllowScroll="True" EnableVirtualScrollPaging="false" UseStaticHeaders="True" FrozenColumnsCount="3"
                                    SaveScrollPosition="false"></Scrolling>
                            </ClientSettings>
                        </telerik:RadGrid>
                    </telerik:RadPageView>
                    
                </telerik:RadMultiPage>
            </telerik:RadPane>
        </telerik:RadSplitter>
        <%--<telerik:RadWindowManager ID="RadWindowManager1" runat="server" VisibleStatusbar="false" Behaviors="Move,Pin,Reload,Close" EnableShadow="true">
            <Windows>
                <telerik:RadWindow ID="ConfigOfflineDialog" runat="server" Height="410px" Width="530px" Skin="Outlook" IconUrl="~/App_Themes/Fminine/images/RadGridHeaderBg2.png"
                    Title="标记位说明" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" />
            </Windows>
            <Localization Close="关闭" Maximize="最大化" Minimize="最小化" Restore="还原" Reload="刷新" PinOff="移动"
                PinOn="固定" />
        </telerik:RadWindowManager>--%>
        <%--<a href="TextFile1.txt">TextFile1.txt</a>--%>
        <%--<asp:Timer runat="server" ID="timer" Interval="1" Enabled="false" OnTick="timer_Tick"></asp:Timer>--%>
        <%--  隐藏域存放选中的因子，在Grid刷新后更新隐藏域数据--%>
        <asp:HiddenField ID="Quality" runat="server"  />
        <asp:HiddenField ID="DtStart" runat="server"  />
        <asp:HiddenField ID="DtEnd" runat="server" />
        <asp:HiddenField ID="HiddenData" runat="server" Value="" />
        <asp:HiddenField ID="AjaxURL" runat="server" Value="../ChartAjaxRequest/AuditData.ashx" />
        <asp:HiddenField ID="HiddenChartType" runat="server" Value="spline" />
        <asp:HiddenField ID="hdHeavyMetalMonitor" runat="server" />
        <asp:HiddenField ID="hdPointName" runat="server" />
        <asp:HiddenField ID="hdBorder" runat="server" Value="[]" />
        <asp:HiddenField ID="hdTime" runat="server"  Value="[]"/>
        <asp:HiddenField ID="hdCount" runat="server"  Value="1"/>
        <asp:HiddenField ID="stopChart" runat="server"  Value="0"/>
        <asp:HiddenField ID="HiddenPointFactor" runat="server" Value="point" />
        <asp:HiddenField ID="firstKey" runat="server"  Value="0"/>
        <asp:HiddenField ID="hdMin" runat="server" />
        <asp:HiddenField ID="hdMax" runat="server" />
    </form>
</body>
</html>
