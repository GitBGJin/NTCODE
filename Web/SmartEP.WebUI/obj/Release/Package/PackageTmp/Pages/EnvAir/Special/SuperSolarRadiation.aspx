<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SuperSolarRadiation.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Special.SuperSolarRadiation" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<%@ Register TagPrefix="CbxRsm" TagName="FactorCbxRsm" Src="~/Controls/FactorCbxRsm.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" href="../../../Resources/CSS/pagination.css" />
    <title></title>
    <script src="../../../Resources/JavaScript/Ladar/jquery-1.9.0.min.js"></script>
    <script src="../../../Resources/JavaScript/telerikControls/highcharts.js"></script>
    <script src="../../../Resources/JavaScript/HighCharts/exporting.js"></script>
    <script type="text/javascript" src="../../../Resources/JavaScript/ChartOperator/merge.js"></script>
    <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
        <script type="text/javascript">

            function OnClientClicking() {

                var rbl = document.getElementsByName("radlDataType");
                var rb2 = document.getElementsByName("radlDataTypeOri");
                for (var i = 0; i < rb2.length; i++) {
                    if (rb2[i].checked && rb2[i].value == "Min1") {
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
                            } else {
                                return true;
                            }
                        }
                    }
                    else if (rb2[i].checked && rb2[i].value == "Min5") {
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
                            } else {
                                return true;
                            }
                        }
                    }
                    else if (rb2[i].checked && rb2[i].value == "Min60") {
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
                            } else {
                                return true;
                            }
                        }
                    }
                    else if (rb2[i].checked && rb2[i].value == "OriDay") {
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
                            } else {
                                return true;
                            }
                        }
                    }
                    else if (rb2[i].checked && rb2[i].value == "OriMonth") {
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
                            } else {
                                return true;
                            }
                        }
                    }
}
    for (var i = 0; i < rbl.length; i++) {
        if (rbl[i].checked && rbl[i].value == "Hour") {
            if ($find("<%= hourBegin.ClientID %>") != null && $find("<%= hourEnd.ClientID %>") != null) {
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
        }
        else if (rbl[i].checked && rbl[i].value == "Day") {
            if ($find("<%= dayBegin.ClientID %>") != null && $find("<%= dayEnd.ClientID %>") != null) {
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
        }
        else if (rbl[i].checked && rbl[i].value == "Month") {
            if ($find("<%= monthBegin.ClientID %>") != null && $find("<%= monthEnd.ClientID %>") != null) {
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
        }

}
}

function RefreshChart() {
    try {
        var Time = eval("(" + document.getElementById("hdDTime").value + ")");
        var Data = eval("(" + document.getElementById("hdDate").value + ")");
        console.log(Time);
        console.log(Data);
        var chartPage = document.getElementById("pvChart");
        chartPage.children[0].contentWindow.InitChart();
    } catch (e) {
    }
}
function ReChart() {
    var chart = document.getElementById("ChartContainer");
    console.log(document.getElementById("hdDate").value);
    var Time = eval("(" + document.getElementById("hdDTime").value + ")");
    var Data = eval("(" + document.getElementById("hdDate").value + ")");
    console.log(Time);
    console.log(Data);
    var chart = new Highcharts.Chart("ChartContainer", {
        title: {
            text: '多因子分析图',
            x: -20
        },
        chart: {
            type: 'spline'
        },
        subtitle: {
            text: '',
            x: -20
        },
        xAxis: {
            //type: 'datetime',
            //labels: {
            //    format: '{value: %m/%d %H时 }',
            //    align: 'right',
            //    rotation: -30
            //},
            categories: Time,
            tickInterval: Math.ceil(Time.length / 5)
        },
        yAxis: [{
            title: {
                text: 'ppb',
                labels: { format: '{value:.,0f}' }
            }, opposite: false
        }, {
            title: {
                text: 'μg/m3',
                labels: { format: '{value:.,0f}' },
            }, opposite: true
        }, {
            title: {
                text: 'mg/m3',
                labels: { format: '{value:.,0f}' },
            }, opposite: true
        }, {
            title: {
                text: 'W/m²',
                labels: { format: '{value:.,0f}' },
            }, opposite: false
        }],
        //tooltip: {
        //    //valueSuffix: ''
        //},
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'middle',
            borderWidth: 0
        },
        series: Data
    });
}
        </script>
    </telerik:RadScriptBlock>

</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="divAirQualityLevel" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="div1" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="div2" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdAirWeather" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenData" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdDate" />
                        <telerik:AjaxUpdatedControl ControlID="hdDTime" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenDataNew" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenChartType" />
                        <telerik:AjaxUpdatedControl ControlID="FirstLoadChart" />
                        <telerik:AjaxUpdatedControl ControlID="img" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="diVisibility" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="imgName" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="Quality" />
                        <telerik:AjaxUpdatedControl ControlID="DtStart" />
                        <telerik:AjaxUpdatedControl ControlID="DtEnd" />
                        <telerik:AjaxUpdatedControl ControlID="ChartContainer" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="imgName">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="divImg" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="diVisibility">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="divImg" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ddlDataSource">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="radlDataTypeOri" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="radlDataType" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dtpHour" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtHour" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtDay" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtMonth" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtSeason" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtYear" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtWeek" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ddlVOC">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="factorCbxRsm" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="radlDataType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="radlDataType" />
                        <telerik:AjaxUpdatedControl ControlID="dtpHour" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtHour" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtDay" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtMonth" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtSeason" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtYear" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtWeek" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>

                <telerik:AjaxSetting AjaxControlID="radlDataTypeOri">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="radlDataTypeOri" />
                        <telerik:AjaxUpdatedControl ControlID="dtpHour" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtHour" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtDay" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtMonth" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtSeason" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtYear" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="dbtWeek" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="start">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="start" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="stop">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="stop" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <%--<telerik:AjaxSetting AjaxControlID="radlDataType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="radlDataType" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>--%>
                <telerik:AjaxSetting AjaxControlID="pointCbxRsmCG">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="pointCbxRsmCG" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="timer">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="img" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="diVisibility" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="imgName" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" BorderWidth="0" BorderSize="0" BorderStyle="None"
            Width="100%">
            <telerik:RadPane ID="paneWhere" runat="server" Height="100px" Width="100%" Scrolling="None" MaxHeight="100"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="width: 100%; height: 100%" class="Table_Customer">
                        <tr>
                            <td class="title" style="width: 80px">站点:
                        </td>
                        <td class="content" style="width: 200px;" colspan="3">
                            <CbxRsm:PointCbxRsm runat="server" ApplicationType="Air" CbxWidth="380" CbxHeight="350" MultiSelected="true" DropDownWidth="520" ID="pointCbxRsmCG"
                                OnSelectedChanged="pointCbxRsmCG_SelectedChanged" DefaultIPointMode="Region"></CbxRsm:PointCbxRsm>
                        </td>
                            <td class="title" style="width: 200px">单位:
                        </td>
                        <td class="content"  style="width: 120px;">
                            <telerik:RadComboBox runat="server" ID="uunit" Width="90px"  colspan="1">
                                <Items>
                                    <telerik:RadComboBoxItem Text="ppb" Value="ppb" Selected="true" />
                                    <telerik:RadComboBoxItem Text="μg/m³" Value="μg/m³" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                            </tr>
                    <tr>
                            <td class="title" style="width: 280px">常规因子:
                        </td>
                        <td class="content" align="left" style="width: 100px;">
                            <telerik:RadComboBox ID="factorCom" runat="server" Width="90" SkinID="Default" Skin="Default" CheckBoxes="true" Localization-CheckAllString="全选" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                                <Items>
                                    <telerik:RadComboBoxItem runat="server" Text="PM2.5" Value="a34004" />
                                    <telerik:RadComboBoxItem runat="server" Text="PM10" Value="a34002" />
                                    <telerik:RadComboBoxItem runat="server" Text="二氧化硫" Value="a21026" />
                                    <telerik:RadComboBoxItem runat="server" Text="二氧化氮" Value="a21004" />
                                    <telerik:RadComboBoxItem runat="server" Text="一氧化碳" Value="a21005" />
                                    <telerik:RadComboBoxItem runat="server" Text="臭氧" Value="a05024" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                              <td class="title" style="width: 200px">Voc分类:
                        </td>
                        <td class="content" style="width: 120px;">
                            <%--<telerik:RadDropDownList ID="ddlVOC" runat="server" Width="140px" OnSelectedIndexChanged="ddlVOC_SelectedIndexChanged" DefaultMessage="请选择" AutoPostBack="true">
                                <Items>
                                    <telerik:DropDownListItem Text="非甲烷碳氢化合物" Value="0" />
                                    <telerik:DropDownListItem Text="卤代烃类VOCs" Value="1" />
                                    <telerik:DropDownListItem Text="含氧（氮）类" Value="2" />
                                </Items>
                            </telerik:RadDropDownList>--%>
                            <telerik:RadComboBox ID="ddlVOC" runat="server" Width="170" SkinID="Default" Skin="Default" CheckBoxes="true" Localization-CheckAllString="全选" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                                <Items>
                                    <%--<telerik:RadComboBoxItem runat="server" Text="VOC一级类总值" Value="0" />
                                    <telerik:RadComboBoxItem runat="server" Text="VOC二级类总值" Value="1" />--%>
                                    <telerik:RadComboBoxItem runat="server" Text="TVOC" Value="17" />
                                    <telerik:RadComboBoxItem runat="server" Text="非甲烷碳氢化合物" Value="0" />
                                    <telerik:RadComboBoxItem runat="server" Text="卤代烃类" Value="1" />
                                    <telerik:RadComboBoxItem runat="server" Text="含氧（氮）类" Value="2" />
                                    <telerik:RadComboBoxItem runat="server" Text="高碳烷烃C6-C12" Value="3" />
                                    <telerik:RadComboBoxItem runat="server" Text="炔烃" Value="4" />
                                    <telerik:RadComboBoxItem runat="server" Text="高碳烯烃C6-C12" Value="5" />
                                    <telerik:RadComboBoxItem runat="server" Text="低碳烯烃C2-C5" Value="6" />
                                    <telerik:RadComboBoxItem runat="server" Text="苯系物" Value="7" />
                                    <telerik:RadComboBoxItem runat="server" Text="低碳烷烃C2-C5" Value="8" />
                                    <telerik:RadComboBoxItem runat="server" Text="氟利昂" Value="9" />
                                    <telerik:RadComboBoxItem runat="server" Text="卤代芳香烃" Value="10" />
                                    <telerik:RadComboBoxItem runat="server" Text="卤代烯烃" Value="11" />
                                    <telerik:RadComboBoxItem runat="server" Text="卤代烷烃" Value="12" />
                                    <telerik:RadComboBoxItem runat="server" Text="含氮有机物" Value="13" />
                                    <telerik:RadComboBoxItem runat="server" Text="醛类有机物" Value="14" />
                                    <telerik:RadComboBoxItem runat="server" Text="酮类有机物" Value="15" />
                                    <telerik:RadComboBoxItem runat="server" Text="醚类有机物" Value="16" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                        
                        <%--<td class="title" style="width: 80px">站点:
                        </td>--%>
                        <%--<td class="content" style="width: 300px;">--%>
                        <CbxRsm:PointCbxRsm runat="server" ApplicationType="Air" CbxWidth="1" CbxHeight="350" Visible="false" MultiSelected="false" DropDownWidth="1" ID="pointCbxRsm"></CbxRsm:PointCbxRsm>
                        <%--</td>--%>
                        <td class="title" style="width: 250px">特征因子:
                        </td>
                        <td class="content" style="width: 380px;">
                            <CbxRsm:FactorCbxRsm runat="server" ApplicationType="Air" CbxWidth="360"  DropDownWidth="380" ID="factorCbxRsm"></CbxRsm:FactorCbxRsm>
                        </td>
                        <td class="title" style="width: 300px">数据来源:
                        </td>
                        <td class="content" align="left" style="width: 90px;">
                            <telerik:RadDropDownList ID="ddlDataSource" runat="server" Width="80px" OnSelectedIndexChanged="ddlDataSource_SelectedIndexChanged" AutoPostBack="true">
                                <Items>
                                    <telerik:DropDownListItem Text="原始数据" Value="OriData" Selected="true" />
                                    <telerik:DropDownListItem Text="审核数据" Value="AuditData" />
                                </Items>
                            </telerik:RadDropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="title" style="width: 80px; text-align: center;">数据类型:
                        </td>
                        <td class="content" style="width: 360px;" colspan="3">
                            <asp:RadioButtonList ID="radlDataType" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="radlDataType_SelectedIndexChanged" AutoPostBack="true" Visible="false">
                            </asp:RadioButtonList>
                            <asp:RadioButtonList ID="radlDataTypeOri" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="radlDataTypeOri_SelectedIndexChanged" AutoPostBack="true">
                            </asp:RadioButtonList>
                        </td>
                        <td class="title" style="width: 80px; text-align: center;">开始时间:
                        </td>
                        <td class="content" style="width: 420px;" colspan="3">
                            <div runat="server" id="dtpHour" visible="false">
                                <telerik:RadDateTimePicker ID="dtpBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd HH:00"
                                    DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                    Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                    TimeView-HeaderText="小时" Calendar-FastNavigationStep="12"  />
                                结束时间:
                            <telerik:RadDateTimePicker ID="dtpEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd HH:00"
                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                TimeView-HeaderText="小时" Calendar-FastNavigationStep="12"  />
                            </div>
                            <div runat="server" id="dbtHour">
                                <telerik:RadDateTimePicker ID="hourBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                    DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd HH:00"
                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                    Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                    TimeView-HeaderText="小时" Calendar-FastNavigationStep="12"  />
                                结束时间;
                            <telerik:RadDateTimePicker ID="hourEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd HH:00"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                TimeView-HeaderText="小时" Calendar-FastNavigationStep="12"  />
                            </div>
                            <div runat="server" id="dbtDay">
                                <telerik:RadDatePicker ID="dayBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd"
                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                    Calendar-FastNavigationSettings-OkButtonCaption="确定" Calendar-FastNavigationStep="12" 
                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" />
                                结束时间;
                            <telerik:RadDatePicker ID="dayEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定" Calendar-FastNavigationStep="12" 
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消" />
                            </div>
                            <div runat="server" id="dbtMonth">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>
                                                <telerik:RadMonthYearPicker ID="monthBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px" Width="105px"
                                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM"
                                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                                                    Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-OkButtonCaption="确定"
                                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-CancelButtonCaption="取消" />
                                            </td>
                                            <td>&nbsp;&nbsp;至&nbsp;&nbsp;</td>
                                            <td>
                                                <telerik:RadMonthYearPicker ID="monthEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM"
                                                    DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday" Width="105px"
                                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" MonthYearNavigationSettings-TodayButtonCaption="当前年月"
                                                    Calendar-FastNavigationSettings-OkButtonCaption="确定" MonthYearNavigationSettings-OkButtonCaption="确定"
                                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消" MonthYearNavigationSettings-CancelButtonCaption="取消" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </td>
                        <td class="content" align="left" rowspan="2">
                            <asp:ImageButton ID="btnSearch" runat="server" OnClientClick="return OnClientClicking()" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                        </td>
                    </tr>
                
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneImage" runat="server" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <%--<table style="width: 99%; margin: auto;">--%>
                <div id="ChartContainer" runat="server" style="width:1200px;margin:auto">
                            </div>
                    <%--<telerik:RadMultiPage ID="multiPage" runat="server" Width="100%" Height="100%" ScrollBars="Hidden" SelectedIndex="0">

                        <telerik:RadPageView ID="pvChart" runat="server" ContentUrl="~/Pages/EnvAir/Chart/ChartFrame.aspx">
                           
                        </telerik:RadPageView>

                    </telerik:RadMultiPage>--%>
                <%--</table>--%>



            </telerik:RadPane>
        </telerik:RadSplitter>
        
        <asp:HiddenField ID="HiddenData" runat="server" Value="" />
        <asp:HiddenField ID="HDyAxis" runat="server" Value="" />
        <asp:HiddenField ID="hdDTime" runat="server" Value=""/>
        <asp:HiddenField ID="hdDate" runat="server" Value=""/>
        <asp:HiddenField ID="AjaxURL" runat="server" Value="../ChartAjaxRequest/GranuleSpecial.ashx" />
    </form>
</body>
</html>
