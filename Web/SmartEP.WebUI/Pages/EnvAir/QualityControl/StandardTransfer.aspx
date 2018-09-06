<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StandardTransfer.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.QualityControl.StandardTransfer" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<%@ Register TagPrefix="CbxRsm" TagName="FactorCbxRsm" Src="~/Controls/FactorCbxRsm.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="../../../Resources/JavaScript/JQuery/jquery-1.10.2.js"></script>
    <script src="../../../Resources/JavaScript/echarts/echarts-2.2.7/build/dist/echarts.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script type="text/javascript">
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

                $(function () {
                    ShowECharts();
                });

                function ShowECharts() {
                    /*var obj = str.parseJSON(); //由JSON字符串转换为JSON对象
                      或者 var obj = JSON.parse(str); //由JSON字符串转换为JSON对象
                    */
                    var windowHeight = document.body.clientHeight;//网页可见区域高 屏幕可用工作区高度：window.screen.availHeight 
                    var windowWidth = document.body.clientWidth;
                    //alert(document.getElementById("Tb2").offsetHeight + ',' + document.getElementById("Tb2").offsetWidth);
                    var divDataEffectRateHeight = (document.getElementById("Tb2").offsetHeight - 70).toString() + "px";//(windowHeight - 80).toString() + "px";
                    var divDataEffectRateWidth = (document.getElementById("Tb2").offsetWidth - 5).toString() + "px";//(windowWidth).toString() + "px";
                    document.getElementById("divCalibrationCurve").style.height = divDataEffectRateHeight;
                    document.getElementById("divCalibrationCurve").style.width = divDataEffectRateWidth;
                    var strEffectData = document.getElementById("<%=hdCalibrationCurveData.ClientID%>").value;
                    var effectData = JSON.parse(strEffectData);
                    var xValue = [];
                    var yValue1 = [];
                    var yValue2 = [];
                    var yValue3 = [];
                    $.each(effectData, function (key, obj) {
                        xValue.push(obj.data1);
                        yValue1.push(obj.data2);
                    });

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
                            var myChart = ec.init(document.getElementById('divCalibrationCurve'));
                            myChart.setOption({
                                //title: {
                                //    text: "数据有效率"
                                //},
                                tooltip: {
                                    trigger: 'axis'
                                    //formatter: function (params, ticket, callback) {
                                    //    var res = params[0].name;//name：x轴名称
                                    //    var effectCount = 0;
                                    //    var unEffectCount = 0;
                                    //    var strEffectRate = "";
                                    //    //if (params[0].value != "" && params[1].value != "") {
                                    //    //    effectCount = parseInt(params[0].value);
                                    //    //    unEffectCount = parseInt(params[1].value);
                                    //    //    if (effectCount + unEffectCount > 0) {
                                    //    //        strEffectRate = (effectCount * 100.0 / (effectCount + unEffectCount)).toString() + '%';
                                    //    //    }
                                    //    //}
                                    //    $.each(effectData, function (key, obj) {
                                    //        if (obj.DateTime == params[0].name) {
                                    //            if (obj.EffectRate != "") {
                                    //                strEffectRate = obj.EffectRate + '%';
                                    //            }
                                    //            return false;
                                    //        }
                                    //    });
                                    //    res += '<br/>有效率 : ' + strEffectRate;
                                    //    for (var i = 0, l = params.length; i < l; i++) {
                                    //        res += '<br/>' + params[i].seriesName + ' : ' + params[i].value;//seriesName：y轴名称，value：y轴的值

                                    //    }
                                    //    return res;
                                    //}
                                    //"{a0}:{c0}%<br/>{a1}:{c1}<br/>{a2}:{c2}"
                                },
                                legend: {
                                    data: ['质量流量计读数']
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
                                        //name: '量程百分比',
                                        data: xValue
                                    }
                                ],
                                yAxis: [
                                    {
                                        type: 'value',
                                        //name: '质量流量计读数',
                                        splitArea: { show: true }
                                    }
                                ],
                                series: [
                                    {
                                        name: '质量流量计读数',
                                        type: 'line',
                                        data: yValue1
                                    }
                                ]
                            });
                        }
                    );
                }
            </script>
        </telerik:RadCodeBlock>
        <input id="radGridColWidth" type="hidden" runat="server" value="120" />
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1"
            UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="pointCbxRsm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="pointCbxRsm" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="factorCbxRsm" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridSamplingRate">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridSamplingRate" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridRTB">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridSamplingRate" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridSamplingRate" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
            <ClientEvents OnRequestStart="onRequestStart" />
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" Width="100%"
            BorderWidth="0" BorderStyle="None" BorderSize="0">
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="Y"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb1" style="width: 100%; height: 85px;" cellspacing="1" cellpadding="0" class="Table_Customer"
                    border="0">
                    <tr>
                        <td class="title" style="width: 80px">传递方式:
                        </td>
                        <td class="content" style="width: 180px;">
                            <asp:DropDownList ID="ddlTransferFashion" runat="server">
                                <asp:ListItem Value="0" Text="第一级间接传递"></asp:ListItem>
                                <asp:ListItem Value="1" Text="第二级间接传递"></asp:ListItem>
                                <asp:ListItem Value="2" Text="直接传递"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="title" style="width: 80px">检测时间:
                        </td>
                        <td class="content" style="width: 180px;">
                            <asp:Label ID="lblCheckTime" runat="server"></asp:Label>
                        </td>
                        <td class="title" style="width: 80px">检测地点:
                        </td>
                        <td class="content">
                            <asp:Label ID="lblCheckPlace" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="title" style="width: 80px; text-align: center;">站房温度:
                        </td>
                        <td class="content">
                            <asp:Label ID="lblRoomTemperature" runat="server"></asp:Label>
                        </td>
                        <td class="title">站房湿度:
                        </td>
                        <td class="content">
                            <asp:Label ID="lblHumidityStation" runat="server"></asp:Label>
                        </td>
                        <td class="title" style="width: 80px">环境气压:
                        </td>
                        <td class="content">
                            <asp:Label ID="lblAmbientPressure" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="title" style="width: 80px; text-align: center;">温度对压力计读数的修正值:
                        </td>
                        <td class="content">
                            <asp:Label ID="lblCorrectedValue" runat="server"></asp:Label>
                        </td>
                        <td class="title" style="text-align: center;">饱和蒸气压:
                        </td>
                        <td class="content" colspan="3">
                            <asp:Label ID="lblSaturatedVaporPressure" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <div style="height: 5px;"></div>
                <telerik:RadGrid ID="gridFlowMeterStandard" runat="server" GridLines="None" Height="80px" Width="100%"
                    AllowPaging="false" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                    AutoGenerateColumns="false" AllowMultiRowSelection="false"
                    EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                    ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                    OnNeedDataSource="grid_NeedDataSource" OnItemDataBound="grid_ItemDataBound"
                    CssClass="RadGrid_Customer">
                    <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
                    <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="Top" IsFilterItemExpanded="False"
                        InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                        <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true" RefreshText="查询" ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                        <CommandItemTemplate>
                            <telerik:RadToolBar BorderWidth="0" ID="gridRTB" SkinID="Query" CssClass="RadToolBar_Customer" AutoPostBack="true"
                                runat="server" Width="50%" OnButtonClick="gridRTB_ButtonClick" OnClientButtonClicking="OnClientClicking" Visible="false" />
                        </CommandItemTemplate>
                        <Columns>
                            <telerik:GridBoundColumn HeaderText="流量计标准" UniqueName="data1" DataField="data1" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="类型" UniqueName="data2" DataField="data2" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="编号" UniqueName="data3" DataField="data3" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="上次标定时间" UniqueName="data4" DataField="data4" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="斜率" UniqueName="data5" DataField="data5" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="截距" UniqueName="data6" DataField="data6" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="相关系数" UniqueName="data7" DataField="data7" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        </Columns>
                        <%--<PagerStyle FirstPageToolTip="首页" AlwaysVisible="true" Position="Bottom" NextPageToolTip="下页" LastPageToolTip="尾页" PrevPageToolTip="上页"
                            PageSizeLabelText="显示记录数:" PagerTextFormat="跳转页面:{4}&amp;nbsp;显示 &lt;strong&gt;{2}&lt;/strong&gt; - &lt;strong&gt;{3}&lt;/strong&gt;条,共 &lt;strong&gt;{5}&lt;/strong&gt;条"></PagerStyle>--%>
                    </MasterTableView>
                    <CommandItemStyle Width="100%" />
                    <ClientSettings>
                        <Scrolling AllowScroll="True" EnableVirtualScrollPaging="false" UseStaticHeaders="True" FrozenColumnsCount="0"
                            SaveScrollPosition="true"></Scrolling>
                    </ClientSettings>
                </telerik:RadGrid>
                <div style="height: 5px;"></div>
                <telerik:RadGrid ID="gridMassFlowMeterReading" runat="server" GridLines="None" Height="270px" Width="100%"
                    AllowPaging="false" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                    AutoGenerateColumns="false" AllowMultiRowSelection="false"
                    EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                    ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                    OnNeedDataSource="grid_NeedDataSource" OnItemDataBound="grid_ItemDataBound"
                    CssClass="RadGrid_Customer" OnColumnCreated="grid_ColumnCreated">
                    <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
                    <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="Top" IsFilterItemExpanded="False"
                        InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                        <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true" RefreshText="查询" ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                        <CommandItemTemplate>
                            <telerik:RadToolBar BorderWidth="0" ID="gridRTB" SkinID="Query" CssClass="RadToolBar_Customer" AutoPostBack="true"
                                runat="server" Width="50%" OnButtonClick="gridRTB_ButtonClick" OnClientButtonClicking="OnClientClicking" Visible="false" />
                        </CommandItemTemplate>
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="序号" UniqueName="Row" HeaderStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate><%#Container.DataSetIndex + 1%></ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn HeaderText="质量流量计读数" UniqueName="data1" DataField="data1" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="平均时间" UniqueName="data2" DataField="data2" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="气体体积" UniqueName="data3" DataField="data3" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="一级标准的质量流量Qs" UniqueName="data4" DataField="data4" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        </Columns>
                        <PagerStyle FirstPageToolTip="首页" AlwaysVisible="true" Position="Bottom" NextPageToolTip="下页" LastPageToolTip="尾页" PrevPageToolTip="上页"
                            PageSizeLabelText="显示记录数:" PagerTextFormat="跳转页面:{4}&amp;nbsp;显示 &lt;strong&gt;{2}&lt;/strong&gt; - &lt;strong&gt;{3}&lt;/strong&gt;条,共 &lt;strong&gt;{5}&lt;/strong&gt;条"></PagerStyle>
                    </MasterTableView>
                    <CommandItemStyle Width="100%" />
                    <ClientSettings>
                        <Scrolling AllowScroll="True" EnableVirtualScrollPaging="false" UseStaticHeaders="True" FrozenColumnsCount="0"
                            SaveScrollPosition="true"></Scrolling>
                    </ClientSettings>
                </telerik:RadGrid>
                <div style="height: 5px;"></div>
                <telerik:RadGrid ID="gridMeasuringRange" runat="server" GridLines="None" Height="140px" Width="100%"
                    AllowPaging="false" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="false"
                    AutoGenerateColumns="false" AllowMultiRowSelection="false"
                    EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                    ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                    OnNeedDataSource="grid_NeedDataSource" OnItemDataBound="grid_ItemDataBound"
                    CssClass="RadGrid_Customer">
                    <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
                    <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="Top" IsFilterItemExpanded="False"
                        InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                        <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true" RefreshText="查询" ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                        <CommandItemTemplate>
                            <telerik:RadToolBar BorderWidth="0" ID="gridRTB" SkinID="Query" CssClass="RadToolBar_Customer" AutoPostBack="true"
                                runat="server" Width="50%" OnButtonClick="gridRTB_ButtonClick" OnClientButtonClicking="OnClientClicking" Visible="false" />
                        </CommandItemTemplate>
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="标定点序号" UniqueName="Row" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate><%#Container.DataSetIndex + 1%></ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn HeaderText="量程百分比" UniqueName="data1" DataField="data1" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="质量流量计读数" UniqueName="data2" DataField="data2" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="一级标准实测流量" UniqueName="data3" DataField="data3" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="一级标准质量流量Qs" UniqueName="data4" DataField="data4" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        </Columns>
                        <PagerStyle FirstPageToolTip="首页" AlwaysVisible="true" Position="Bottom" NextPageToolTip="下页" LastPageToolTip="尾页" PrevPageToolTip="上页"
                            PageSizeLabelText="显示记录数:" PagerTextFormat="跳转页面:{4}&amp;nbsp;显示 &lt;strong&gt;{2}&lt;/strong&gt; - &lt;strong&gt;{3}&lt;/strong&gt;条,共 &lt;strong&gt;{5}&lt;/strong&gt;条"></PagerStyle>
                    </MasterTableView>
                    <CommandItemStyle Width="100%" />
                    <ClientSettings>
                        <Scrolling AllowScroll="True" EnableVirtualScrollPaging="false" UseStaticHeaders="True" FrozenColumnsCount="0"
                            SaveScrollPosition="true"></Scrolling>
                    </ClientSettings>
                </telerik:RadGrid>
                <div style="height: 5px;"></div>
                <table id="Tb2" style="width: 100%;min-height:200px;" cellspacing="1" cellpadding="0" class="Table_Customer"
                    border="0">
                    <tr>
                        <td class="title" colspan="4" style="text-align:left;">校准曲线图
                        </td>
                    </tr>
                    <tr>
                        <td class="title" colspan="4">
                            <div id="divCalibrationCurve" style="height: 200px; border: 1px solid #ccc; padding: 10px; border-width: 0px; border-style: none;"></div>
                            <asp:HiddenField ID="hdCalibrationCurveData" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="title" style="width: 100px; text-align: center;">斜率:
                        </td>
                        <td class="content">
                            <asp:Label ID="lblSlope" runat="server"></asp:Label>
                        </td>
                        <td class="title" style="text-align: center;">截距:</td>
                        <td class="content">
                            <asp:Label ID="lblIntercept" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="title" style="width: 100px; text-align: center;">相关系数:
                        </td>
                        <td class="content">
                            <asp:Label ID="lblCorrelationCoefficient" runat="server"></asp:Label>
                        </td>
                        <td class="title" style="text-align: center;">是否合格:</td>
                        <td class="content">
                            <asp:Label ID="lblIsQualified" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="height:10px;"></td>
                    </tr>
                </table>
            </telerik:RadPane>
        </telerik:RadSplitter>
    </form>
</body>
</html>
