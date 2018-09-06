﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LadarBMPThermodynamic.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Special.LadarBMPThermodynamic" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="../../../Resources/JavaScript/Ladar/jquery-1.9.0.min.js"></script>
    <script src="../../../Resources/JavaScript/Ladar/highcharts.js"></script>
    <script src="../../../Resources/JavaScript/Ladar/heatmap.js"></script>
    <script src="../../../Resources/JavaScript/Ladar/ChartHeatmap.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
                //控制时间范围
                function BeginDateChanging(sender, args) {

                    var beginTime = new Date(Date.parse(args._newValue.replace(/-/g, "/")));
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
                function CreatCharts() {
                    
                    $('#ChartContainer').html("");
                    //获取隐藏域的值
                    var strJSON = document.getElementById("HiddenDataNew").value;
                    var quality = document.getElementById("Quality").value;
                    var dtStart = document.getElementById("DtStart").value;
                    var dtEnd = document.getElementById("DtEnd").value;
                    var hdMin = document.getElementById("hdMin").value;
                    var hdMax = document.getElementById("hdMax").value;
                    document.getElementById("HiddenDataNew").value = "";
                    //if (quality != "border") {
                    //    ajaxHighChart("热力图", "热力图", strJSON, "ChartContainer", "时间", "Km", "值");
                    //}
                    //else {

                    //}
                    if (quality != "height") {
                        if (quality == "extin532") {
                            ajaxHighChart("激光雷达", "消光系数532", strJSON, "ChartContainer", "时间", "Km", "值", hdMin, hdMax);
                        } else if (quality == "extin355") {
                            ajaxHighChart("激光雷达", "消光系数355", strJSON, "ChartContainer", "时间", "Km", "值", hdMin, hdMax);
                        } else if (quality == "depol") {
                            ajaxHighChart("激光雷达", "退偏振度", strJSON, "ChartContainer", "时间", "Km", "值", hdMin, hdMax);
                        }
                        $("#ChartContainer").css("overflow-y", "auto");
                        $("#ChartContainer").height("500px");//设置图表Iframe的高度
                        $("#ChartContainer").width("600px");//设置图表Iframe的宽度
                        $("#ChartContainer").css("margin", "0 auto");
                    }
                    else {
                        
                        var Border = document.getElementById("hdBorder").value;
                        var Time = document.getElementById("hdTime").value;
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
                                categories: c
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
                    }
                    
                }
            </script>
        </telerik:RadCodeBlock>
        <input id="radGridColWidth" type="hidden" runat="server" value="120" />
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1"
            UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridAudit" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenDataNew" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenChartType" />
                        <telerik:AjaxUpdatedControl ControlID="hdHeavyMetalMonitor" />
                        <telerik:AjaxUpdatedControl ControlID="hdBorder" />
                        <telerik:AjaxUpdatedControl ControlID="hdTime" />
                        <telerik:AjaxUpdatedControl ControlID="hdMin" />
                        <telerik:AjaxUpdatedControl ControlID="hdMax" />
                        <telerik:AjaxUpdatedControl ControlID="first" />
                         <telerik:AjaxUpdatedControl ControlID="divImg" />
                        <telerik:AjaxUpdatedControl ControlID="hdCount" />
                        <telerik:AjaxUpdatedControl ControlID="ChartContainer" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="FirstLoadChart" />
                        <telerik:AjaxUpdatedControl ControlID="hdPointName" />
                        <telerik:AjaxUpdatedControl ControlID="Quality" />
                        <telerik:AjaxUpdatedControl ControlID="DtStart" />
                        <telerik:AjaxUpdatedControl ControlID="DtEnd" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" Width="100%"
            BorderWidth="0" BorderStyle="None" BorderSize="0">
            <telerik:RadPane ID="paneWhere" runat="server" Height="80px" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="width: 100%; height: 100%" cellspacing="1" cellpadding="0" class="Table_Customer"
                    border="0">
                    <tr>
                        <td class="title" style="width: 80px">数据来源:
                        </td>
                        <td class="content" style="width: 120px;">
                            <telerik:RadDropDownList ID="ddlDataSource" runat="server" Width="100px" AutoPostBack="true" Visible="true">
                                <Items>
                                    <telerik:DropDownListItem Text="消光系数532" Value="extin532" Selected="true" />
                                    <telerik:DropDownListItem Text="消光系数355" Value="extin355" />
                                    <telerik:DropDownListItem Text="边界层高度" Value="border" />
                                    <telerik:DropDownListItem Text="退偏振度" Value="depol" />
                                </Items>
                            </telerik:RadDropDownList>
                        </td>


                        <td class="title" style="width: 80px;">开始时间:
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
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"  ClientEvents-OnDateSelected="EndDateSelected"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"  DateInput-ClientEvents-OnValueChanging="EndDateChanging"
                                TimeView-HeaderText="小时" Calendar-FastNavigationStep="12" />
                            </div>
                        </td>
                        <td class="content" align="right" rowspan="1">
                            <asp:ImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                        </td>
                        <td>
                             <b>目前默认两天加载图片,支持最大<br/>选择四天数据画图,但画图速度会变慢</b>
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table style="width: 99%; margin: auto;">
                    <tr>
                        <td style="width: 100%;">
                            <div style="padding-top: 6px;text-align:center">
                                <div id="ChartContainer" runat="server" style="width: 500px; height: 480px;text-align:center">
                                </div>
                                <img  visible="false" runat="server" id="divImg" style=" height:500px;text-align:center" src="http://www.sjxh.org/"/>
                            </div>

                        </td>
                    </tr>
                </table>

            </telerik:RadPane>
        </telerik:RadSplitter>
        <%--  隐藏域存放选中的因子，在Grid刷新后更新隐藏域数据--%>
        <asp:HiddenField ID="Quality" runat="server" />
        <asp:HiddenField ID="DtStart" runat="server" />
        <asp:HiddenField ID="DtEnd" runat="server" />
        <asp:HiddenField ID="hdBorder" runat="server" />
        <asp:HiddenField ID="hdTime" runat="server" />
        <asp:HiddenField ID="HiddenDataNew" runat="server" Value="" />
        <asp:HiddenField ID="firstKey" runat="server"  Value="0"/>
        <asp:HiddenField ID="hdMin" runat="server" />
        <asp:HiddenField ID="hdMax" runat="server" />
        <script src="../../../Resources/JavaScript/HighCharts/exporting.js"></script>
    </form>
</body>
</html>
