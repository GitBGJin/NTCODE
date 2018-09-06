<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LadarBMP_new.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.SuperStation.LadarBMP_new" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        /*表格样式*/
        .border-table {
            width: 100%;
            min-width: 198px;
            border-width: 1px;
            margin: 0;
            background: #fff;
            text-align: center;
            font-size: 14px;
        }

            .border-table th, .border-table td {
                margin: 0;
                padding: 2px 10px;
                line-height: 26px;
                height: 28px;
                border: 1px solid #eee;
                vertical-align: middle;
                white-space: nowrap;
                word-break: keep-all;
            }

            .border-table thead th {
                color: #333;
                font-weight: bold;
                white-space: nowrap;
                text-align: center;
                background: #B1D1EC;
            }

            .border-table tbody th {
                padding-right: 5px;
                text-align: right;
                color: #707070;
                background-color: #f9f9f9;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script type="text/javascript" src="../../../Resources/JavaScript/JQuery/jquery-1.9.0.js"></script>
            <script type="text/javascript" src="../../../Resources/JavaScript/ChartOperator/merge.js"></script>
            <script type="text/javascript">
        function CreatCharts() {
            $('#divJGLD').html("");
            //获取隐藏域的值

            var quality = document.getElementById("Quality").value;
            var dtStart = document.getElementById("DtStart").value;
            var dtEnd = document.getElementById("dtEnd").value;
            console.log(quality);
            console.log(dtStart);
            console.log(dtEnd);
            //根据值跳转画图页面
            var chartdiv = "";
            //$.each(pointIds.split(','), function (chartNo, value) {
            chartdiv = "";
            chartdiv += '<div style=" width:60%; height:600px;">';
            chartdiv += '<iframe name="chartIframe" id="frame' + Math.random() + '" src="../Chart/LadarBMPThermodynamicChart.aspx?quality=' + quality +'&dtStart='+dtStart+'&dtEnd='+dtEnd+ '"  frameborder="0" marginheight="0" marginwidth="0" width="100%" height="90%" scrolling="no"></iframe>';
            chartdiv += '</div>';
            $("#divJGLD").append(chartdiv);

            //});
            $("#divJGLD").css("overflow-y", "auto");
            $("#divJGLD").height("650px");//设置图表Iframe的高度
            $("#divJGLD").width("60%");//设置图表Iframe的宽度
        }
                 </script>
        </telerik:RadCodeBlock>
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1"
            UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
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
                <telerik:AjaxSetting AjaxControlID="radlDataType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="radlDataType" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="img"  LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="div1"  LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="imgName"  LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="divJGLD"  LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="Quality" />
                        <telerik:AjaxUpdatedControl ControlID="DtStart" />
                        <telerik:AjaxUpdatedControl ControlID="DtEnd" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="timer">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="img"  LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="div1"  LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="imgName"  LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>

            </AjaxSettings>
        </telerik:RadAjaxManager>
        <table style="width: 100%; height: 98%; text-align: center;">
            <tr style="width: 100%; height: 100%;">
                <td style="width: 200px; height: 100%; vertical-align: top">
                    <br />
                    <telerik:RadDatePicker runat="server" ID="rdpstartdate" DateInput-Label="开始日期" MinDate="1900-01-01" DateInput-DateFormat="yyyy-MM-dd"></telerik:RadDatePicker>
                    <br />
                    <br />
                    <telerik:RadDatePicker runat="server" ID="rdpenddate" DateInput-Label="截止日期" MinDate="1900-01-01" DateInput-DateFormat="yyyy-MM-dd"></telerik:RadDatePicker>
                    <br />
                    <br />
                    <div style="text-align: center; width: 100%; vertical-align: central">
                        <asp:RadioButtonList ID="radlDataType" runat="server" TextAlign="Right" RepeatDirection="Horizontal">
                        </asp:RadioButtonList>
                    </div>
                    <br />
                    <br />
                    <asp:ImageButton SkinID="ImgBtnSearch" runat="server" ID="btnSearch" OnClick="btnSearch_Click"  />
                    <br />
                    <br />
                    <div style="width: 100%; max-height: 350px; overflow-x: hidden;" runat="server" id="div1"></div>
                    <br />
                    <asp:Timer runat="server" ID="timer" Interval="3000" Enabled="false" OnTick="timer_Tick"></asp:Timer>
                    <%--<table>
                        <tr>
                            <td>
                                <telerik:RadNumericTextBox ID="tbSecond" runat="server" Width="20px"></telerik:RadNumericTextBox>
                                秒
                            </td>
                            <td>
                                <telerik:RadButton ID="start" runat="server" BackColor="#3A94D3" ForeColor="White" Height="20" AutoPostBack="True" OnClick="start_Click">
                                    <ContentTemplate>
                                        <asp:Label runat="server" ID="Label4" ForeColor="White" Text="自动播放"></asp:Label>
                                    </ContentTemplate>
                                </telerik:RadButton>
                            </td>
                            <td>
                                <telerik:RadButton ID="stop" runat="server" BackColor="#3A94D3" ForeColor="White" Height="20" AutoPostBack="True" OnClick="stop_Click">
                                    <ContentTemplate>
                                        <asp:Label runat="server" ID="Label4" ForeColor="White" Text="停止"></asp:Label>
                                    </ContentTemplate>
                                </telerik:RadButton>
                            </td>
                        </tr>
                    </table>--%>
                </td>
                <td style="width: 2px; height: 100%; background-color: #f3f3f3"></td>
                <td style="width: 20px; height: 100%">
                    <label id="imgName" runat="server"></label>
                    <%--   <table style="height: 100%; width: 100%; text-align: center; vertical-align: middle; font-size: 14px; font-weight: bold;">
                        <tr style="height: 50%">
                            <td>消光系数</td>
                        </tr>
                        <tr style="height: 50%">
                            <td>退偏振度</td>
                        </tr>
                    </table>--%>
                </td>
                <td style="height: 100%">
                    <div runat="server" id="divImg" visible="false"></div>
                    <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" Width="100%"
            BorderWidth="0" BorderStyle="None" BorderSize="0">
                    <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                    <telerik:RadMultiPage runat="server">
                    <telerik:RadPageView ID="pChart" runat="server" Visible="true">
                        <div id="divJGLD" style="background-size:100% 100%;height:500px"></div>
                    </telerik:RadPageView>
                    </telerik:RadMultiPage>
                        </telerik:RadPane>
                        </telerik:RadSplitter>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="Quality" runat="server"  />
    <asp:HiddenField ID="DtStart" runat="server"  />
    <asp:HiddenField ID="DtEnd" runat="server" />
    </form>
    
</body>
</html>
