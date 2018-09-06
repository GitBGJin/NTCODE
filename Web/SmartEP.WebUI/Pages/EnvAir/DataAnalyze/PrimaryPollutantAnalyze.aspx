<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrimaryPollutantAnalyze.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.DataAnalyze.PrimaryPollutantAnalyze" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="../../../Resources/JavaScript/JQuery/jquery-1.10.2.min.js" type="text/javascript"></script>
    <%--<script src="../../../Resources/JavaScript/Highcharts-4.1.9/highcharts.js" type="text/javascript"></script>--%>
    <script src="../../../Resources/JavaScript/HighCharts/highcharts.js" type="text/javascript"></script>
    <telerik:RadScriptBlock runat="server">
        <script type="text/javascript">
            function GetHighchartsPie(content) {
                var content = unescape(content);
                var title1 = content.split('☆')[0], title2 = content.split('☆')[3];
                var name1 = content.split('☆')[1], name2 = content.split('☆')[4];
                var value1 = content.split('☆')[2], value2 = content.split('☆')[5];
                var json1 = { "data": [] }, json2 = { "data": [] };
                //组装series.data1
                if (name1 != "" && value1 != "") {
                    for (var i = 0; i < name1.split('&').length; i++) {
                        json1.data.push({
                            name: name1.split('&')[i],
                            y: parseInt(value1.split('&')[i], 10)
                        });
                    }
                }
                //组装series.data1
                if (name2 != "" && value2 != "") {
                    for (var i = 0; i < name2.split('&').length; i++) {
                        json2.data.push({
                            name: name2.split('&')[i],
                            y: parseInt(value2.split('&')[i], 10)
                        });
                    }
                }

                var charts1 = new Highcharts.Chart({
                    chart: {
                        renderTo: "chart1",
                        plotBackgroundColor: null,
                        plotBorderWidth: null,
                        plotShadow: false
                    },
                    credits: {
                        enabled: false
                    },
                    title: {
                        text: title1
                    },
                    plotOptions: {
                        pie: {
                            allowPointSelect: true,
                            cursor: 'pointer',
                            dataLabels: {
                                enabled: true,
                                color: '#000000',
                                connectorColor: '#000000',
                                format: '<b>{point.name}</b>: {point.percentage:.1f} %'
                            }
                        }
                    },
                    series: [{
                        type: 'pie',
                        name: '天数',
                        data: json1.data
                    }]
                });

                var charts2 = new Highcharts.Chart({
                    chart: {
                        renderTo: "chart2",
                        plotBackgroundColor: null,
                        plotBorderWidth: null,
                        plotShadow: false
                    },
                    credits: {
                        enabled: false
                    },
                    title: {
                        text: title2
                    },
                    plotOptions: {
                        pie: {
                            allowPointSelect: true,
                            cursor: 'pointer',
                            dataLabels: {
                                enabled: true,
                                color: '#000000',
                                connectorColor: '#000000',
                                format: '<b>{point.name}</b>: {point.percentage:.1f} %'
                            }
                        }
                    },
                    series: [{
                        type: 'pie',
                        name: '天数',
                        data: json2.data
                    }]
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
                <telerik:AjaxSetting AjaxControlID="ddlTypeList">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ddlTypeList" />
                        <telerik:AjaxUpdatedControl ControlID="divPoint" />
                        <telerik:AjaxUpdatedControl ControlID="ddlPoint" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ddlReportTypeList">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ddlReportTypeList" />
                        <telerik:AjaxUpdatedControl ControlID="Tab1" />
                        <telerik:AjaxUpdatedControl ControlID="Tab2" />
                        <telerik:AjaxUpdatedControl ControlID="Tab3" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="RadMonthYearPicker1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadMonthYearPicker1" />
                        <telerik:AjaxUpdatedControl ControlID="ddlWeek" />
                        <telerik:AjaxUpdatedControl ControlID="Literal1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ddlWeek">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ddlWeek" />
                        <telerik:AjaxUpdatedControl ControlID="Literal1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="RadGrid1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="tb3" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" BorderWidth="0" Height="180px"
            Width="100%" BorderSize="0">
            <telerik:RadPane ID="paneWhere" runat="server" Height="80px" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="height: 100%;" class="Table_Customer">
                    <tr>
                        <td class="title" style="text-align: right; width: 100px;">选择类型：
                        </td>
                        <td class="content" style="text-align: left; width: 120px;">
                            <asp:RadioButtonList ID="ddlTypeList" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="ddlTypeList_SelectedIndexChanged" AutoPostBack="true" Width="120px">
                                <asp:ListItem Text="单点" Selected="True" Value="0"></asp:ListItem>
                                <asp:ListItem Text="全市" Value="1"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td class="title" style="text-align: right; width: 100px">
                            <div runat="server" id="divPoint">测点：</div>
                        </td>
                        <td class="content" style="text-align: left;">
                            <asp:DropDownList runat="server" ID="ddlPoint" Width="180px"></asp:DropDownList>
                        </td>
                        <td class="content" style="text-align: left;" rowspan="2">
                            <asp:ImageButton ID="btnSearch" runat="server" SkinID="ImgBtnSearch" OnClick="btnSearch_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td class="title" style="text-align: right;">报表类型：
                        </td>
                        <td class="content" style="text-align: left;">
                            <asp:DropDownList runat="server" ID="ddlReportTypeList" Width="120px" OnSelectedIndexChanged="ddlReportTypeList_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Text="周" Value="Week"></asp:ListItem>
                                <asp:ListItem Text="月" Value="Month"></asp:ListItem>
                                <asp:ListItem Text="季" Value="Season"></asp:ListItem>
                                <asp:ListItem Text="年" Value="Year"></asp:ListItem>
                                <asp:ListItem Text="自定义" Value="Self"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="title" style="text-align: right;">日期：
                        </td>
                        <td class="content">
                            <table runat="server" id="Tab1">
                                <tr>
                                    <td>
                                        <telerik:RadMonthYearPicker ID="RadMonthYearPicker1" AutoPostBack="true" runat="server" Width="120px" EnableTyping="false"
                                            OnSelectedDateChanged="RadMonthYearPicker1_SelectedDateChanged">
                                            <MonthYearNavigationSettings TodayButtonCaption="今天" CancelButtonCaption="取消" OkButtonCaption="确定" />
                                        </telerik:RadMonthYearPicker>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlWeek" Width="100px" OnSelectedIndexChanged="ddlWeek_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                            </table>
                            <table runat="server" id="Tab2" visible="false">
                                <tr>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlYear" Width="100px"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlSeason" Width="100px">
                                            <asp:ListItem Text="第一季度" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="第二季度" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="第三季度" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="第四季度" Value="4"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <table runat="server" id="Tab3" visible="false">
                                <tr>
                                    <td>
                                        <telerik:RadDatePicker ID="txtStartDate" EnableTyping="false" runat="server" MinDate="1900-01-01" Width="100px" DateInput-EmptyMessage="开始日期"></telerik:RadDatePicker>
                                    </td>
                                    <td>
                                        <telerik:RadDatePicker ID="txtEndDate" EnableTyping="false" runat="server" MinDate="1900-01-01" Width="100px" DateInput-EmptyMessage="结束日期"></telerik:RadDatePicker>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Scrolling="None" BorderStyle="None">
                <telerik:RadGrid ID="RadGrid1" runat="server" GridLines="None"
                    AllowPaging="false" AllowSorting="true" AutoGenerateColumns="true" ShowStatusBar="false"
                    OnNeedDataSource="RadGrid1_NeedDataSource" CssClass="RadGrid_Customer">
                    <ExportSettings HideStructureColumns="false" IgnorePaging="true" OpenInNewWindow="true" ExportOnlyData="true" />
                    <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" AlternatingItemStyle-HorizontalAlign="Center"
                        ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" NoMasterRecordsText="没有数据">
                        <EditFormSettings PopUpSettings-Modal="true" />
                    </MasterTableView>
                </telerik:RadGrid>
            </telerik:RadPane>
        </telerik:RadSplitter>
        <table id="tb3" runat="server" style="width: 100%; height: 400px;" class="Table_Customer">
            <tr style="height: 400px;">
                <td id="Td1" style="width: 50%; height: 400px; text-align: center" runat="server">
                    <div id="chart1" style="width: 100%; height: 400px; margin: 0 auto;">
                    </div>
                </td>
                <td id="Td2" style="width: 50%; height: 400px; text-align: center" runat="server">
                    <div id="chart2" style="width: 100%; height: 400px; margin: 0 auto;">
                    </div>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
