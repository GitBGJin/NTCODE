<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="24HSinglePollutantDataAnalyze.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Dock._24HSinglePollutantDataAnalyze" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">

        <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
            <script type="text/javascript">
                function GetData() {
                    var obj = new Object();
                    obj.PointNames = document.getElementById("<%=hdPointNames.ClientID%>").value;
                    obj.FactorName = document.getElementById("<%=hdFactorName.ClientID%>").value;
                    return obj;
                }
            </script>
            <!--Step:2 Import echarts.js-->
            <!--Step:2 引入echarts.js-->
            <script src="../../../Resources/JavaScript/JQuery/jquery-1.10.2.js"></script>
            <script src="../../../Resources/JavaScript/echarts/echarts-2.2.7/build/dist/echarts.js"></script>
            <script type="text/javascript" language="javascript">
                var myRadGrid;
                function RadGridCreating(sender, eventArgs) { myRadGrid = sender; }

                //控制导出时按钮不会隐藏掉处理
                function onRequestStart(sender, args) {
                    if (args.EventArgument == 6 || args.EventArgument == 7 ||
                        args.get_eventTarget().indexOf("ExportToExcelButton") >= 0 ||
                            args.get_eventTarget().indexOf("ExportToWordButton") >= 0 ||
                            args.get_eventTarget().indexOf("ExportToCsvButton") >= 0) {
                        args.set_enableAjax(false);
                    }
                }

                $(function () {
                    ShowECharts();
                });

                function ShowECharts() {
                    /*var obj = str.parseJSON(); //由JSON字符串转换为JSON对象
                      或者 var obj = JSON.parse(str); //由JSON字符串转换为JSON对象
                    */
                    var windowHeight = document.body.clientHeight;//网页可见区域高 屏幕可用工作区高度：window.screen.availHeight 
                    var windowWidth = document.body.clientWidth;
                    var divDataEffectRateHeight = (windowHeight - 70).toString() + "px";
                    var divDataEffectRateWidth = (windowWidth).toString() + "px";
                    document.getElementById("divDataEffectRate").style.height = divDataEffectRateHeight;
                    document.getElementById("divDataEffectRate").style.width = divDataEffectRateWidth;
                    var strEffectData = document.getElementById("<%=hdSinglePollutant.ClientID%>").value;
                    var effectData = JSON.parse(strEffectData);
                    var strpointiddata = document.getElementById("<%=hdpointiddata.ClientID%>").value;
                    var pointiddata = JSON.parse(strpointiddata);
                    var timevalue = [];
                    var dataarray = [];
                    var pointname = [];
                    $.each(effectData, function (key, obj) {

                        timevalue.push(obj.DateTime);
                    });

                    $.each(pointiddata, function (key1, obj1) {
                        pointname.push(obj1.pointName);
                        var pointobj = new Object();
                        pointobj.name = obj1.pointName;
                        pointobj.type = "line";
                        pointobj.smooth = true;
                        pointobj.data = [];
                        $.each(effectData, function (key, obj) {
                            pointobj.data.push(obj[obj1.pointId]);
                        });
                        dataarray.push(pointobj);
                    })


                    require.config({
                        paths: {
                            echarts: '../../../Resources/JavaScript/echarts/echarts-2.2.7/build/dist'
                        }
                    });
                    require(
                        [
                            'echarts',
                            'echarts/chart/bar',
                            'echarts/chart/line',
                            'echarts/chart/map'
                        ],
                        function (ec) {
                            //--- 折柱 ---
                            var myChart = ec.init(document.getElementById('divDataEffectRate'));
                            myChart.setOption({
                                title: {

                                },
                                grid: {
                                    x: 40,
                                    y: 40,
                                    x2: 20,
                                    y2: 30,
                                },
                                tooltip: {
                                    trigger: 'axis'
                                },
                                legend: {
                                    data: pointname
                                },
                                toolbox: {
                                    show: true,
                                    feature: {
                                        mark: { show: false },//辅助线开关
                                        dataView: { show: false, readOnly: false },//数据视图
                                        magicType: { show: false, type: ['line', 'bar'] },
                                        restore: { show: false },//还原
                                        saveAsImage: { show: false }//保存为图片
                                    }
                                },
                                calculable: false,//是否启用拖拽重计算特性，默认关闭
                                xAxis: [
                                    {

                                        type: 'category',
                                        data: timevalue
                                    }
                                ],
                                yAxis: [
                                    {
                                        name: '浓度(mg/m3)',
                                        type: 'value',
                                        splitArea: { show: true }
                                    }
                                ],
                                series: dataarray

                            });
                        }
                    );
                }
            </script>
        </telerik:RadScriptBlock>
        <input id="radGridColWidth" type="hidden" runat="server" value="120" />
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="pointCbxRsm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="pointCbxRsm" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdPointNames" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rcbFactors">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rcbFactors" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdFactorName" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="hdSinglePollutant" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdpointiddata" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="divDataEffectRate" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdFactorName" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="hdPointNames" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
            <ClientEvents OnRequestStart="onRequestStart" />
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" BorderWidth="0" BorderSize="0" BorderStyle="None"
            Width="100%">
            <telerik:RadPane ID="paneWhere" runat="server" Height="35px" Width="100%" Scrolling="None" MaxHeight="100"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="height: 100%; width: 100%" cellspacing="1" cellpadding="0" class="Table_Customer"
                    border="0">
                    <tr>
                        <td class="title" style="width: 50px; text-align: center;">测点:
                        </td>
                        <td class="content" style="width: 30%; text-align: left">
                            <%--<CbxRsm:PointCbxRsm runat="server" ApplicationType="Air" CbxWidth="250" CbxHeight="350" MultiSelected="true" DropDownWidth="400" ID="pointCbxRsm"></CbxRsm:PointCbxRsm>--%>
                            <telerik:RadComboBox runat="server" OnSelectedIndexChanged="rcbPoint_SelectedIndexChanged" ID="rcbPoint" Localization-CheckAllString="全选" Width="250px" CheckBoxes="true" Visible="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                            </telerik:RadComboBox>
                        </td>
                        <td class="title" style="width: 90px; text-align: center;">监测因子:
                        </td>
                        <td class="content" style="width: 30%; text-align: left">
                            <telerik:RadDropDownList runat="server" OnSelectedIndexChanged="rcbFactors_SelectedIndexChanged" ID="rcbFactors" Width="250px">
                                <%--    <Items>
                                    <telerik:DropDownListItem Text="二氧化硫" Value="SO2" />
                                    <telerik:DropDownListItem Text="二氧化氮" Value="NO2" />
                                    <telerik:DropDownListItem Text="细微颗粒物" Value="PM25" />
                                    <telerik:DropDownListItem Text="可吸入颗粒物" Value="PM10" />
                                    <telerik:DropDownListItem Text="一氧化碳" Value="CO" />
                                    <telerik:DropDownListItem Text="臭氧1小时" Value="O3" />
                                    <telerik:DropDownListItem Text="臭氧8小时" Value="Recent8HoursO3" />
                                </Items>--%>
                            </telerik:RadDropDownList>
                        </td>
                        <td>
                            <asp:ImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                        </td>
                        <%--  <td class="content" >
                            <asp:ImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                        </td>--%>
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneImage" runat="server" Width="100%" Height="80%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <!--Step:1 Prepare a dom for ECharts which (must) has size (width & hight)-->
                <!--Step:1 为ECharts准备一个具备大小（宽高）的Dom-->
                <div id="divDataEffectRate" style="height: 80%; border-width: 0px; border-style: none;"></div>
                <asp:HiddenField ID="hdSinglePollutant" runat="server" />
                <asp:HiddenField ID="hdpointiddata" runat="server" />
                <asp:HiddenField ID="hdPointNames" runat="server" />
                <asp:HiddenField ID="hdFactorName" runat="server" />
            </telerik:RadPane>
        </telerik:RadSplitter>
    </form>
</body>
</html>
