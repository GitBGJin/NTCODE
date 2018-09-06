<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClassAnalyze.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.DataAnalyze.ClassAnalyze" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="../../../Resources/JavaScript/JQuery/jquery-1.10.2.min.js" type="text/javascript"></script>
    <%--<script src="../../../Resources/JavaScript/Highcharts-4.1.9/highcharts.js" type="text/javascript"></script>--%>
    <script src="../../../Resources/JavaScript/HighCharts/highcharts.js" type="text/javascript"></script>
    <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
        <script type="text/javascript">
            //function aa() {
            //;
            $(document).ready(function () {
                var width = document.body.clientWidth;
                $('#tb3').css("width", width);
                GetHighchartsPie();
            });

            function GetHighchartsPie() {
                var chart;
                chart = new Highcharts.Chart({
                    chart: {
                        renderTo: 'chart1',          //放置图表的容器
                        type: 'pie'
                        //options3d: {
                        //    enabled: true,
                        //    alpha: 45,
                        //    beta: 0
                        //}
                    },
                    title: {
                        text: '<%=ViewState["title1"]%>'
                    },
                    plotOptions: {
                        pie: {
                            allowPointSelect: true,
                            cursor: 'pointer',
                            depth: 35,
                            dataLabels: {
                                enabled: true,
                                format: '{point.name}'
                            }
                        }
                    },
                    series: [{
                        type: 'pie',
                        name: '天数',
                        data: [
                                 <%=ViewState["ReturnValue"]%>

                        ]
                    }]
                });

                chart = new Highcharts.Chart({
                    chart: {
                        renderTo: 'chart2',          //放置图表的容器
                        type: 'pie'
                        //options3d: {
                        //    enabled: true,
                        //    alpha: 45,
                        //    beta: 0
                        //}
                    },
                    title: {
                        text: '<%=ViewState["title2"]%>'
                    },
                    plotOptions: {
                        pie: {
                            allowPointSelect: true,
                            cursor: 'pointer',
                            depth: 35,
                            dataLabels: {
                                enabled: true,
                                format: '{point.name}'
                            }
                        }
                    },
                    series: [{
                        type: 'pie',
                        name: '天数',
                        data: [
                                 <%=ViewState["ReturnValue2"]%>

                        ]
                    }]
                });
            }

            function loadSplitter(sender) {
                var bodyWidth = document.body.clientWidth;
                var bodyHeight = document.body.clientHeight;
                sender.set_width(bodyWidth);//初始化Splitter高度及宽度
                sender.set_height(bodyHeight);
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
                <telerik:AjaxSetting AjaxControlID="typeList">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="typeList" />
                        <telerik:AjaxUpdatedControl ControlID="divPoint" />
                        <telerik:AjaxUpdatedControl ControlID="ddlPoint" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ReportTypeList">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ReportTypeList" />
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
                <%-- <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="tb3" />
                    </UpdatedControls>
                </telerik:AjaxSetting>--%>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" BorderWidth="0" OnClientLoad="loadSplitter"
            Width="100%" BorderSize="0">
            <telerik:RadPane ID="paneWhere" runat="server" Height="80" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="height: 100%;" class="Table_Customer">
                    <tr>
                        <td class="title" style="text-align: right; width: 100px;">选择类型：
                        </td>
                        <td class="content" style="text-align: left; width: 120px;">
                            <asp:RadioButtonList ID="typeList" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="typeList_SelectedIndexChanged" AutoPostBack="true" Width="120px">
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
                            <asp:DropDownList runat="server" ID="ReportTypeList" Width="120px" OnSelectedIndexChanged="ReportTypeList_SelectedIndexChanged" AutoPostBack="true">
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
                                        <asp:DropDownList runat="server" ID="yearList" Width="100px"></asp:DropDownList>
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
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None" BorderStyle="None">
                <telerik:RadGrid ID="RadGrid1" runat="server" GridLines="None" Width="100%"
                    AllowPaging="false" PageSize="24" AllowCustomPaging="false" AllowSorting="false" ShowFooter="false"
                    AutoGenerateColumns="true" AllowMultiRowSelection="false"
                    EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                    ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                    OnNeedDataSource="RadGrid1_NeedDataSource" CssClass="RadGrid_Customer">
                    <ExportSettings HideStructureColumns="false" IgnorePaging="true" OpenInNewWindow="true" ExportOnlyData="true" />
                    <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" AlternatingItemStyle-HorizontalAlign="Center"
                        ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" NoMasterRecordsText="没有数据">
                        <EditFormSettings PopUpSettings-Modal="true" />
                    </MasterTableView>
                </telerik:RadGrid>
                <%-- <table id="tb3" runat="server" style="width: 98%; height: 400px;" class="table_customer">
                    <tr style="height: 400px;">
                        <td id="td1" style="width: 50%; height: 400px; text-align: center" runat="server">
                            <div id="chart1" style="width: 100%; height: 400px; margin: 0 auto;">
                            </div>
                        </td>
                        <td id="td2" style="width: 50%; height: 400px; text-align: center" runat="server">
                            <div id="chart2" style="width: 100%; height: 400px; margin: 0 auto;">
                            </div>
                        </td>
                    </tr>
                </table>--%>

                 <table id="tb3" runat="server" style="width: 98%; height: 400px;" class="table_customer">
                    <tr style="height: 400px;">
                        <td id="td1" style="width: 50%; text-align: center" runat="server">
                            <div id="chart1" style="width: 100%; height:100%; margin: 0 auto;">
                            </div>
                        </td>
                        <td id="td2" style="width: 50%;  text-align: center" runat="server">
                            <div id="chart2" style="width: 100%; height: 100%; margin: 0 auto;">
                            </div>
                        </td>
                    </tr>
                </table>
               <%-- <div id="tb3" style="width: 98%; height: 500px;">
                    <div id="chart1" style="width: 50%; height: 100%; margin: 0 auto; float: left;">
                    </div>
                    <div id="chart2" style="width: 50%; height: 100%; margin: 0 auto; float: left;">
                    </div>
                    <div style="clear: both;"></div>
                </div>--%>
            </telerik:RadPane>
        </telerik:RadSplitter>


    </form>
</body>
</html>

