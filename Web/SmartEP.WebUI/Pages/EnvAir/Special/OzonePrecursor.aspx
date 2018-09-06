<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OzonePrecursor.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Special.OzonePrecursor" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
            <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script type="text/javascript" src="../../../Resources/JavaScript/JQuery/jquery-1.9.0.js"></script>
            <script type="text/javascript" src="../../../Resources/JavaScript/ChartOperator/merge.js"></script>
            <script type="text/javascript">
                
                function SearchData() {
                    var buttonSearch = document.getElementById("<%=btnSearch.ClientID%>");
                    buttonSearch.click();
                }
                function CreatCharts() {
                    $('#PieChart1').html("");
                    $('#PieChart2').html("");
                    $('#PieChart3').html("");
                    //获取隐藏域的值

                    //var BrandData = document.getElementById("hdBrandData").value;
                    var TypeNames = document.getElementById("hdTypes").value;
                    var Points = document.getElementById("hdPoints").value;
                    var Begion = document.getElementById("hdBegion").value;
                    var End = document.getElementById("hdEnd").value;
                    var OrderBy = document.getElementById("hdOrderBy").value;
                    var DataType = document.getElementById("hdDataType").value;
                    var TimeType = document.getElementById("hdTimeType").value;
                    //根据值跳转画图页面
                    var chartdiv1 = "";
                    var chartdiv2 = "";
                    var chartdiv3 = "";
                    //$.each(pointIds.split(','), function (chartNo, value) {
                    chartdiv1 = "";
                    chartdiv1 += '<div style=" width:100%; height:600px;" >';
                    chartdiv1 += '<iframe name="chartIframe1" id="frame1' + Math.random() + '" src="../Chart/OzonePrecursorChart.aspx?TypeNames=' + TypeNames + '&Points=' + Points + '&Begion=' + Begion + '&End=' + End + '&OrderBy=' + OrderBy + '&DataType=' + DataType + '&TimeType=' + TimeType + '"  frameborder="0" marginheight="0" marginwidth="0" width="100%" height="90%" scrolling="no"></iframe>';
                    chartdiv1 += '</div>';

                    chartdiv2 = "";
                    chartdiv2 += '<div style=" width:100%; height:600px;" >';
                    chartdiv2 += '<iframe name="chartIframe2" id="frame2' + Math.random() + '" src="../Chart/OzonePrecursorChart1.aspx?TypeNames=' + TypeNames + '&Points=' + Points + '&Begion=' + Begion + '&End=' + End + '&OrderBy=' + OrderBy + '&DataType=' + DataType + '&TimeType=' + TimeType + '"  frameborder="0" marginheight="0" marginwidth="0" width="100%" height="90%" scrolling="no"></iframe>';
                    chartdiv2 += '</div>';

                    chartdiv3 = "";
                    chartdiv3 += '<div style=" width:100%; height:600px;" >';
                    chartdiv3 += '<iframe name="chartIframe3" id="frame3' + Math.random() + '" src="../Chart/OzonePrecursorChart2.aspx?TypeNames=' + TypeNames + '&Points=' + Points + '&Begion=' + Begion + '&End=' + End + '&OrderBy=' + OrderBy + '&DataType=' + DataType + '&TimeType=' + TimeType + '"  frameborder="0" marginheight="0" marginwidth="0" width="100%" height="90%" scrolling="no"></iframe>';
                    chartdiv3 += '</div>';
                    console.log(chartdiv2)
                    console.log(chartdiv3)
                    $("#PieChart1").append(chartdiv1);
                    $('#PieChart2').append(chartdiv2);
                    $('#PieChart3').append(chartdiv3);
                    //});
                    $("#PieChart1").css("overflow-y", "auto");
                    $("#PieChart1").height("650px");//设置图表Iframe的高度
                    $("#PieChart1").width("100%");//设置图表Iframe的宽度

                    $("#PieChart2").css("overflow-y", "auto");
                    $("#PieChart2").height("650px");//设置图表Iframe的高度
                    $("#PieChart2").width("100%");//设置图表Iframe的宽度

                    $("#PieChart3").css("overflow-y", "auto");
                    $("#PieChart3").height("650px");//设置图表Iframe的高度
                    $("#PieChart3").width("100%");//设置图表Iframe的宽度
                }
                
                function OnClientClicking() {
                    var rbl = document.getElementsByName("radlDataType");
                    var rb2 = document.getElementsByName("RadioButtonList1");
                    for (var i = 0; i < rb2.length; i++) {
                        if (rb2[i].checked && rb2[i].value == "Min1") {
                            var date1 = new Date();
                            var date2 = new Date();
                            date1 = $find("<%= dtpBegin.ClientID %>").get_selectedDate();
                            date2 = $find("<%= dtpEnd.ClientID %>").get_selectedDate();
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
                        else if (rb2[i].checked && rb2[i].value == "Min5") {
                            var date1 = new Date();
                            var date2 = new Date();
                            date1 = $find("<%= dtpBegin.ClientID %>").get_selectedDate();
                            date2 = $find("<%= dtpEnd.ClientID %>").get_selectedDate();
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
                        else if (rb2[i].checked && rb2[i].value == "Min60") {
                            var date1 = new Date();
                            var date2 = new Date();
                            date1 = $find("<%= dtpBegin.ClientID %>").get_selectedDate();
                            date2 = $find("<%= dtpEnd.ClientID %>").get_selectedDate();
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
            }
            for (var i = 0; i < rbl.length; i++) {
                if (rbl[i].checked && rbl[i].value == "Hour") {
                    var hourB = $find("<%= hourBegin.ClientID %>").get_selectedDate();
                    var hourE = $find("<%= hourEnd.ClientID %>").get_selectedDate();
                    if ((hourB == null) || (hourE == null)) {
                        alert("开始时间或者终止时间，不能为空！");
                        return false;
                    }
                    if (hourB > hourE) {
                        alert("开始时间不能大于终止时间！");
                        return false;
                    } else {
                        return true;
                    }
                }
                else if (rbl[i].checked && rbl[i].value == "Day") {
                    var dayB = $find("<%= dayBegin.ClientID %>").get_selectedDate();
                    var dayE = $find("<%= dayEnd.ClientID %>").get_selectedDate();
                    if ((dayB == null) || (dayE == null)) {
                        alert("开始时间或者终止时间，不能为空！");
                        return false;
                    }
                    if (dayB > dayE) {
                        alert("开始时间不能大于终止时间！");
                        return false;
                    } else {
                        return true;
                    }
                }
                else if (rbl[i].checked && rbl[i].value == "Month") {
                    var monthB = $find("<%= monthBegin.ClientID %>").get_selectedDate();
                            var monthE = $find("<%= monthEnd.ClientID %>").get_selectedDate();
                            if ((monthB == null) || (monthE == null)) {
                                alert("开始时间或者终止时间，不能为空！");
                                return false;
                            }
                            if (monthB > monthE) {
                                alert("开始时间不能大于终止时间！");
                                return false;
                            } else {
                                return true;
                            }
                        }
                        else if (rbl[i].checked && rbl[i].value == "Season") {
                            var seasondateB = $find("<%= seasonBegin.ClientID %>").get_selectedDate();
                            var seasondateE = $find("<%= seasonEnd.ClientID %>").get_selectedDate();
                            var seasondateF = $find("<%= seasonFrom.ClientID %>")._selectedValue;
                            var seasondateT = $find("<%= seasonTo.ClientID %>")._selectedValue;
                            if ((seasondateB == null) || (seasondateE == null)) {
                                alert("开始时间或者终止时间，不能为空！");
                                return false;
                            }
                            if (seasondateB > seasondateE) {
                                alert("开始时间不能大于终止时间！");
                                return false;
                            }
                            if (seasondateB < seasondateE) {
                                return true;
                            } else {
                                if (parseInt(seasondateF) > parseInt(seasondateT)) {
                                    alert("同年季开始时间不能大于终止时间！");
                                    return false;
                                }
                            }
                        }
                        else if (rbl[i].checked && rbl[i].value == "Year") {
                            var yearB = $find("<%= yearBegin.ClientID %>").get_selectedDate();
                                var yearE = $find("<%= yearEnd.ClientID %>").get_selectedDate();
                                if ((yearB == null) || (yearE == null)) {
                                    alert("开始时间或者终止时间，不能为空！");
                                    return false;
                                }
                                if (yearB > yearE) {
                                    alert("开始时间不能大于终止时间！");
                                    return false;
                                } else {
                                    return true;
                                }
                            }
                            else if (rbl[i].checked && rbl[i].value == "Week") {
                                var weekdateB = $find("<%= weekBegin.ClientID %>").get_selectedDate();
                                var weekdateF = $find("<%= weekFrom.ClientID %>")._selectedIndex;
                                var weekdateE = $find("<%= weekEnd.ClientID %>").get_selectedDate();
                                var weekdateT = $find("<%= weekTo.ClientID %>")._selectedIndex;
                                if ((weekdateB == null) || (weekdateE == null)) {
                                    alert("开始时间或者终止时间，不能为空！");
                                    return false;
                                }
                                if (weekdateB > weekdateE) {
                                    alert("开始时间不能大于终止时间！");
                                    return false;
                                }
                                if (weekdateB < weekdateE) {
                                    return true;
                                } else {
                                    if (parseInt(weekdateF) > parseInt(weekdateT)) {
                                        alert("同年月周开始时间不能大于终止时间！");
                                        return false;
                                    }
                                }
                            }

    }
}

//tab页切换时时间检查
function OnClientSelectedIndexChanging(sender, args) {
    var rbl = document.getElementsByName("radlDataType");
    var rb2 = document.getElementsByName("radlDataTypeOri");
    for (var i = 0; i < rb2.length; i++) {
        if (rb2[i].checked && rb2[i].value == "Min1") {
            var date1 = new Date();
            var date2 = new Date();
            date1 = $find("<%= dtpBegin.ClientID %>").get_selectedDate();
            date2 = $find("<%= dtpEnd.ClientID %>").get_selectedDate();
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
        else if (rb2[i].checked && rb2[i].value == "Min5") {
            var date1 = new Date();
            var date2 = new Date();
            date1 = $find("<%= dtpBegin.ClientID %>").get_selectedDate();
            date2 = $find("<%= dtpEnd.ClientID %>").get_selectedDate();
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
        else if (rb2[i].checked && rb2[i].value == "Min60") {
            var date1 = new Date();
            var date2 = new Date();
            date1 = $find("<%= dtpBegin.ClientID %>").get_selectedDate();
                    date2 = $find("<%= dtpEnd.ClientID %>").get_selectedDate();
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
    }
    for (var i = 0; i < rbl.length; i++) {
        if (rbl[i].checked && rbl[i].value == "Hour") {
            var hourB = $find("<%= hourBegin.ClientID %>").get_selectedDate();
                    var hourE = $find("<%= hourEnd.ClientID %>").get_selectedDate();
                    if ((hourB == null) || (hourE == null)) {
                        alert("开始时间或者终止时间，不能为空！");
                        return false;
                    }
                    if (hourB > hourE) {
                        alert("开始时间不能大于终止时间！");
                        return false;
                    } else {
                        return true;
                    }
                }
                else if (rbl[i].checked && rbl[i].value == "Day") {
                    var dayB = $find("<%= dayBegin.ClientID %>").get_selectedDate();
                    var dayE = $find("<%= dayEnd.ClientID %>").get_selectedDate();
                    if ((dayB == null) || (dayE == null)) {
                        alert("开始时间或者终止时间，不能为空！");
                        return false;
                    }
                    if (dayB > dayE) {
                        alert("开始时间不能大于终止时间！");
                        return false;
                    } else {
                        return true;
                    }
                }
                else if (rbl[i].checked && rbl[i].value == "Month") {
                    var monthB = $find("<%= monthBegin.ClientID %>").get_selectedDate();
                    var monthE = $find("<%= monthEnd.ClientID %>").get_selectedDate();
                    if ((monthB == null) || (monthE == null)) {
                        alert("开始时间或者终止时间，不能为空！");
                        return false;
                    }
                    if (monthB > monthE) {
                        alert("开始时间不能大于终止时间！");
                        return false;
                    } else {
                        return true;
                    }
                }
                else if (rbl[i].checked && rbl[i].value == "Season") {
                    var seasondateB = $find("<%= seasonBegin.ClientID %>").get_selectedDate();
                    var seasondateE = $find("<%= seasonEnd.ClientID %>").get_selectedDate();
                    var seasondateF = $find("<%= seasonFrom.ClientID %>")._selectedValue;
                    var seasondateT = $find("<%= seasonTo.ClientID %>")._selectedValue;
                    if ((seasondateB == null) || (seasondateE == null)) {
                        alert("开始时间或者终止时间，不能为空！");
                        return false;
                    }
                    if (seasondateB > seasondateE) {
                        alert("开始时间不能大于终止时间！");
                        return false;
                    }
                    if (seasondateB < seasondateE) {
                        return true;
                    } else {
                        if (parseInt(seasondateF) > parseInt(seasondateT)) {
                            alert("同年季开始时间不能大于终止时间！");
                            return false;
                        }
                    }
                }
                else if (rbl[i].checked && rbl[i].value == "Year") {
                    var yearB = $find("<%= yearBegin.ClientID %>").get_selectedDate();
                            var yearE = $find("<%= yearEnd.ClientID %>").get_selectedDate();
                            if ((yearB == null) || (yearE == null)) {
                                alert("开始时间或者终止时间，不能为空！");
                                return false;
                            }
                            if (yearB > yearE) {
                                alert("开始时间不能大于终止时间！");
                                return false;
                            } else {
                                return true;
                            }
                        }
                        else if (rbl[i].checked && rbl[i].value == "Week") {
                            var weekdateB = $find("<%= weekBegin.ClientID %>").get_selectedDate();
                            var weekdateF = $find("<%= weekFrom.ClientID %>")._selectedIndex;
                            var weekdateE = $find("<%= weekEnd.ClientID %>").get_selectedDate();
                            var weekdateT = $find("<%= weekTo.ClientID %>")._selectedIndex;
                            if ((weekdateB == null) || (weekdateE == null)) {
                                alert("开始时间或者终止时间，不能为空！");
                                return false;
                            }
                            if (weekdateB > weekdateE) {
                                alert("开始时间不能大于终止时间！");
                                return false;
                            }
                            if (weekdateB < weekdateE) {
                                return true;
                            } else {
                                if (parseInt(weekdateF) > parseInt(weekdateT)) {
                                    alert("同年月周开始时间不能大于终止时间！");
                                    return false;
                                }
                            }
                        }

}
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
        <input id="radGridColWidth" type="hidden" runat="server" value="120" />
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1"
            UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="HiddenData" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenChartType" />
                        <telerik:AjaxUpdatedControl ControlID="hdHeavyMetalMonitor" />
                        <telerik:AjaxUpdatedControl ControlID="FirstLoadChart" />
                        <telerik:AjaxUpdatedControl ControlID="hdPointName" />
                        <telerik:AjaxUpdatedControl ControlID="ChartContainer" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdBrandData" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdPoints" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdTypes" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdTimeType" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdDataType" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdBegion" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdEnd" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdOrderBy" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="PieChart1" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="PieChart2" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="PieChart3" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="radlDataType">
                    <UpdatedControls>
                        
                        <telerik:AjaxUpdatedControl ControlID="dbtHour" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtDay" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtSeason" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtMonth" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtYear" LoadingPanelID="RadAjaxLoadingPanel1" />

                        <telerik:AjaxUpdatedControl ControlID="dbtWeek" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="RadioButtonList1">
                    <UpdatedControls>
                        
                        <telerik:AjaxUpdatedControl ControlID="dtpHour" LoadingPanelID="RadAjaxLoadingPanel1" />
                        
                        <telerik:AjaxUpdatedControl ControlID="dbtHour" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtDay" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtSeason" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtMonth" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtYear" LoadingPanelID="RadAjaxLoadingPanel1" />

                        <telerik:AjaxUpdatedControl ControlID="dbtWeek" LoadingPanelID="RadAjaxLoadingPanel1" />

                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ddlDataSource">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="radlDataType" />
                        <telerik:AjaxUpdatedControl ControlID="dbtHour" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtDay" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtSeason" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtMonth" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtYear" LoadingPanelID="RadAjaxLoadingPanel1" />
                        
                        <telerik:AjaxUpdatedControl ControlID="dbtWeek" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="RadioButtonList1" />
                        <telerik:AjaxUpdatedControl ControlID="dtpHour" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="weekBegin">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="weekFrom" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="weekEnd">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="weekTo" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
            <ClientEvents OnRequestStart="onRequestStart" OnResponseEnd="onRequestEnd" />
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" Width="100%"
            BorderWidth="0" BorderStyle="None" BorderSize="0">
            <telerik:RadPane ID="paneWhere" runat="server" Height="100px" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="width: 100%; height: 100%" cellspacing="1" cellpadding="0" class="Table_Customer"
                    border="0">
                    <tr>
                        <td class="title" style="width: 80px">数据来源:
                                    </td>
                                    <td class="content" style="width: 600px;">
                                        <telerik:RadDropDownList ID="ddlDataSource" runat="server" Width="200px" OnSelectedIndexChanged="ddlDataSource_SelectedIndexChanged" AutoPostBack="true">
                                            <Items>
                                                <telerik:DropDownListItem  Text="原始数据" Value="OriData" Selected="true"/>
                                                <telerik:DropDownListItem  Text="审核数据" Value="AuditData" />
                                            </Items>
                                        </telerik:RadDropDownList>
                                    </td>
                        <td class="content" align="right" colspan="3" >
                            <asp:ImageButton ID="btnSearch" runat="server" OnClientClick="return OnClientClicking()" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                        </td>
                    </tr>
                    <tr>
                        <td class="title" style="width: 80px; text-align: center;">数据类型:
                        </td>
                        <td class="content" style="width: 420px;" >
                            <asp:RadioButtonList ID="radlDataType" runat="server" RepeatDirection="Horizontal" Visible="false" OnSelectedIndexChanged="radlDataType_SelectedIndexChanged" AutoPostBack="true">
                            </asp:RadioButtonList>
                            <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatDirection="Horizontal"  OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged" AutoPostBack="true">
                            </asp:RadioButtonList>
                        </td>
                        <td class="title" style="width: 80px; text-align: center;">开始时间:
                        </td>
                        <td class="content" style="width: 500px;">
                            <div runat="server" id="dtpHour" visible="true">
                            <telerik:RadDateTimePicker ID="dtpBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd HH:mm"
                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                TimeView-HeaderText="小时" Calendar-FastNavigationStep="12" />结束时间:
                            <telerik:RadDateTimePicker ID="dtpEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd HH:mm"
                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                TimeView-HeaderText="小时" Calendar-FastNavigationStep="12" />
                        </div>
                            <div runat="server" id="dbtHour" visible="false">
                                <telerik:RadDateTimePicker ID="hourBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                    DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd HH:00"
                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                    Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                    TimeView-HeaderText="小时" Calendar-FastNavigationStep="12" />
                                &nbsp;&nbsp;至&nbsp;&nbsp;
                            <telerik:RadDateTimePicker ID="hourEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd HH:00"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                TimeView-HeaderText="小时" Calendar-FastNavigationStep="12" />
                            </div>
                            <div runat="server" id="dbtDay" visible="false">
                                <telerik:RadDatePicker ID="dayBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd"
                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                    Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" Calendar-FastNavigationStep="12" />
                                &nbsp;&nbsp;至&nbsp;&nbsp;
                            <telerik:RadDatePicker ID="dayEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消" Calendar-FastNavigationStep="12" />
                            </div>
                           
                            <div runat="server" id="dbtWeek" visible="false">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>
                                                <telerik:RadMonthYearPicker ID="weekBegin" runat="server" MinDate="1900-01-01" Width="85px" BorderWidth="1px" DateInput-DateFormat="yyyy-MM"
                                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday"
                                                    MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                                                    MonthYearNavigationSettings-OkButtonCaption="确定"
                                                    MonthYearNavigationSettings-CancelButtonCaption="取消" OnSelectedDateChanged="weekBegin_SelectedDateChanged" AutoPostBack="true" />
                                            </td>
                                            <td>&nbsp;&nbsp;</td>
                                            <td>
                                                <telerik:RadDropDownList ID="weekFrom" runat="server" Width="60px" OnSelectedIndexChanged="weekFrom_SelectedIndexChanged" AutoPostBack="true">
                                                </telerik:RadDropDownList>
                                            </td>
                                            <td>&nbsp;&nbsp;至</td>
                                            <td>
                                                <telerik:RadMonthYearPicker ID="weekEnd" runat="server" MinDate="1900-01-01" Width="85px" BorderWidth="1px" DateInput-DateFormat="yyyy-MM"
                                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday"
                                                    MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                                                    MonthYearNavigationSettings-OkButtonCaption="确定"
                                                    MonthYearNavigationSettings-CancelButtonCaption="取消" OnSelectedDateChanged="weekEnd_SelectedDateChanged" AutoPostBack="true" />
                                            </td>
                                            <td>&nbsp;&nbsp;</td>
                                            <td>
                                                <telerik:RadDropDownList ID="weekTo" runat="server" Width="60px" OnSelectedIndexChanged="weekTo_SelectedIndexChanged" AutoPostBack="true">
                                                </telerik:RadDropDownList>
                                            </td>
                                            <td></td>
                                        </tr>
                                    </tbody>
                                </table>
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>日期范围：<asp:TextBox runat="server" BorderWidth="0" BorderColor="#ffffff" ID="txtweekF" ReadOnly="true" Width="70px"></asp:TextBox>~<asp:TextBox runat="server" ID="txtweekT" BorderWidth="0" BorderColor="#ffffff" ReadOnly="true" Width="70px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>

                            <div runat="server" id="dbtMonth" visible="false">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>
                                                <telerik:RadMonthYearPicker ID="monthBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px" Width="95px"
                                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM"
                                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="今天"
                                                    Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-CancelButtonCaption="取消"
                                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-OkButtonCaption="确定" />
                                            </td>
                                            <td>&nbsp;&nbsp;至&nbsp;&nbsp;</td>
                                            <td>
                                                <telerik:RadMonthYearPicker ID="monthEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM"
                                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" Width="95px"
                                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="今天"
                                                    Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-CancelButtonCaption="取消"
                                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-OkButtonCaption="确定" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>

                            <div runat="server" id="dbtSeason" visible="false">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>
                                                <telerik:RadMonthYearPicker ID="seasonBegin" runat="server" MinDate="1900-01-01" Width="85px" BorderWidth="1px" DateInput-DateFormat="yyyy"
                                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday"
                                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="今天"
                                                    Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-CancelButtonCaption="取消"
                                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-OkButtonCaption="确定" />
                                            </td>
                                            <td>&nbsp;&nbsp;第</td>
                                            <td>
                                                <telerik:RadDropDownList ID="seasonFrom" runat="server" Width="40px">
                                                    <Items>
                                                        <telerik:DropDownListItem runat="server" Selected="True" Text="1" Value="1" />
                                                        <telerik:DropDownListItem runat="server" Value="2" Text="2" />
                                                        <telerik:DropDownListItem runat="server" Value="3" Text="3" />
                                                        <telerik:DropDownListItem runat="server" Value="4" Text="4" />
                                                    </Items>
                                                </telerik:RadDropDownList>
                                            </td>
                                            <td>季&nbsp;&nbsp;至</td>
                                            <td>
                                                <telerik:RadMonthYearPicker ID="seasonEnd" runat="server" MinDate="1900-01-01" Width="85px" BorderWidth="1px" DateInput-DateFormat="yyyy"
                                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday"
                                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="今天"
                                                    Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-CancelButtonCaption="取消"
                                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-OkButtonCaption="确定" />
                                            </td>
                                            <td>&nbsp;&nbsp;第</td>
                                            <td>
                                                <telerik:RadDropDownList ID="seasonTo" runat="server" Width="40px">
                                                    <Items>
                                                        <telerik:DropDownListItem runat="server" Value="1" Text="1" />
                                                        <telerik:DropDownListItem runat="server" Value="2" Text="2" />
                                                        <telerik:DropDownListItem runat="server" Value="3" Text="3" />
                                                        <telerik:DropDownListItem runat="server" Value="4" Text="4" Selected="True" />
                                                    </Items>
                                                </telerik:RadDropDownList>
                                            </td>
                                            <td>季 </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div runat="server" id="dbtYear" visible="false">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>
                                                <telerik:RadMonthYearPicker ID="yearBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px" Width="95px"
                                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy"
                                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="今天"
                                                    Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-CancelButtonCaption="取消"
                                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-OkButtonCaption="确定" />
                                            </td>
                                            <td>&nbsp;&nbsp;至&nbsp;&nbsp;</td>
                                            <td>
                                                <telerik:RadMonthYearPicker ID="yearEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px" Width="95px"
                                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy"
                                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="今天"
                                                    Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-CancelButtonCaption="取消"
                                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-OkButtonCaption="确定" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadMultiPage ID="multiPage" runat="server" Width="100%" Height="100%" ScrollBars="Hidden" SelectedIndex="0">
                    <telerik:RadPageView ID="pChart" runat="server" Visible="true">
                           <table style="width:100%"; margin: auto;">
                               <tr>
                                   <td style="width:33%">
                                       <div id="PieChart1"  style=" width: 100%;  height: 500px;"></div>
                                   </td>
                                   <td style="width:33%">
                                       <div id="PieChart2" style="width: 100%; height: 500px;" ></div>
                                   </td>
                                   <td style="width:33%">
                                       <div id="PieChart3" style="width: 100%; height: 500px" ></div>
                                   </td>
                               </tr>
                           </table>
                    </telerik:RadPageView>
                </telerik:RadMultiPage>
            </telerik:RadPane>
        </telerik:RadSplitter>
        <%--  隐藏域存放选中的因子，在Grid刷新后更新隐藏域数据--%>
        <asp:HiddenField ID="HiddenData" runat="server" Value="" />
        <asp:HiddenField ID="AjaxURL" runat="server" Value="../ChartAjaxRequest/AuditData.ashx" />
        <asp:HiddenField ID="HiddenChartType" runat="server" Value="spline" />
        <asp:HiddenField ID="hdHeavyMetalMonitor" runat="server" />
        <asp:HiddenField ID="hdPointName" runat="server" />
        <asp:HiddenField ID="hdBrandData" runat="server" value="0"/>
        <asp:HiddenField ID="hdPoints" runat="server" value="0"/>
        <asp:HiddenField ID="hdTypes" runat="server" value="0"/>
        <asp:HiddenField ID="hdTimeType" runat="server" value="0"/>
        <asp:HiddenField ID="hdDataType" runat="server" value="0"/>
        <asp:HiddenField ID="hdBegion" runat="server" value="0"/>
        <asp:HiddenField ID="hdEnd" runat="server" value="0"/>
        <asp:HiddenField ID="hdOrderBy" runat="server" value="0"/>
        <asp:HiddenField ID="HiddenPointFactor" runat="server" Value="point" />
        <script src="../../../Resources/JavaScript/JQuery/jquery-1.10.2.js"></script>
        <script src="../../../Resources/JavaScript/HighCharts/highstock.js"></script>
        <script src="../../../Resources/JavaScript/HighCharts/exporting.js"></script>
    </form>
</body>
</html>
