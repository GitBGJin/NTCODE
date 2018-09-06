<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AirQualityDayReportNew.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Report.AirQualityDayReportNew" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script type="text/javascript" src="../../../Resources/JavaScript/JQuery/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="../../../Resources/JavaScript/echarts/echarts-2.2.7/build/dist/echarts.js"></script>
    <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
        <script type="text/javascript">
            $(function () {
                ShowECharts();
            });

            function ShowECharts() {
                /*var obj = str.parseJSON(); //由JSON字符串转换为JSON对象
                  或者 var obj = JSON.parse(str); //由JSON字符串转换为JSON对象
                */
                var strAirQuality = document.getElementById("<%=hdAirQuality.ClientID%>").value;
                var AirQuality = JSON.parse(strAirQuality);
                var value1 = [];
                var value2 = [];
                var dataXArr = [];
                var dataYArr = [];
                var colorArr = [];
                var j = 0;
                var rtColor = function (i) {
                    var color;
                    switch (i) {
                        case 0: colo = '#2FF103';
                            break;
                        case 1: colo = '#FFFF02';
                            break;
                        case 2: colo = '#FF8500';
                            break;
                        case 3: colo = '#FB0000';
                            break;
                        case 4: colo = '#970041';
                            break;
                        case 5: colo = '#7F0020';
                            break;
                    }
                    return color;

                };
                $.each(AirQuality, function (key, obj) {
                    value1.push(obj.Type);
                    value2.push(obj.Value);
                });
                for (var i in value2) {
                    if (value2[i] != 0) {

                        dataYArr.push({ value: value2[i], name: value1[i] })
                        dataXArr.push(value1[i]);
                        colorArr = rtColor(i);
                        j++;
                    }
                }
                require.config({
                    paths: {
                        echarts: '../../../Resources/JavaScript/echarts/echarts-2.2.7/build/dist'
                    }
                });
                require(
                    [
                        'echarts',
                        'echarts/chart/pie',
                        'echarts/chart/line',
                        'echarts/chart/map'
                    ],
                    function (ec) {
                        //--- 饼图 ---
                        var myChart = ec.init(document.getElementById('divAirQualityLevel'));
                        var opinion = {

                            tooltip: {
                                trigger: 'vertical'
                            },
                            legend: {
                                x: 'left',
                                y: 'bottom',
                                orient: 'vertical',
                                data: dataXArr
                            },
                            toolbox: {
                                show: true,
                                feature: {
                                    mark: { show: false },//辅助线开关
                                    dataView: { show: false, readOnly: false },//数据视图
                                    magicType: {
                                        show: false, type: ['pie', 'funnel'],
                                        option: {
                                            funnel: {
                                                x: '25%',
                                                width: '50%',
                                                funnelAlign: 'left',
                                                max: 1700
                                            }
                                        }
                                    },
                                    restore: { show: false },//还原
                                    saveAsImage: { show: false }//保存为图片
                                }
                            },
                            calculable: false,//是否启用拖拽重计算特性，默认关闭

                            series: [
                              {
                                  type: 'pie',
                                  center: ['40%', '50%'],
                                  radius: '80%',
                                  data: dataYArr,
                                  itemStyle: {
                                      normal: {
                                          color: function (params) {
                                              var colorList = [
                                                '#2FF103',
                                                '#FFFF02',
                                                '#FF8500',
                                                '#FB0000',
                                                '#970041',
                                                '#7F0020'
                                              ];
                                              return colorList[params.dataIndex]
                                          },
                                          label: {
                                              show: true,
                                              formatter: '{b} : {c} (天) '
                                          },
                                          labelLine: { show: true }
                                      }
                                  }
                              }
                            ]
                        };
                        myChart.setOption(opinion);
                    }
                );
            }
        </script>
    </telerik:RadScriptBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" BorderWidth="0"
            Width="100%">
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="45%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadGrid ID="RadGrid1" runat="server" GridLines="None" Height="100%" Width="100%"
                    AutoGenerateColumns="false" AllowMultiRowSelection="false" CssClass="RadGrid_Customer"
                    BorderStyle="None" OnNeedDataSource="RadGrid1_NeedDataSource" OnItemDataBound="RadGrid1_ItemDataBound">
                    <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" NoMasterRecordsText="没有数据">
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="序号" UniqueName="Row" HeaderStyle-Width="30px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate><%#Container.DataSetIndex + 1%></ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn HeaderText="测点名称" UniqueName="PointName" DataField="PointName"
                                HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridDateTimeColumn HeaderText="日期" UniqueName="DateTime" DataField="DateTime" DataFormatString="{0:yyyy-MM-dd}" EmptyDataText="--"
                                HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="二氧化硫(SO<sub>2</sub>)<br />浓度(μg/m<sup>3</sup>)" UniqueName="SO2" DataField="SO2" EmptyDataText="--"
                                HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="二氧化氮(NO<sub>2</sub>)<br />浓度(μg/m<sup>3</sup>)" UniqueName="NO2" DataField="NO2" EmptyDataText="--"
                                HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="PM<sub>10</sub><br />浓度(μg/m<sup>3</sup>)" UniqueName="PM10" DataField="PM10" EmptyDataText="--"
                                HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="一氧化碳(CO)<br />浓度(mg/m<sup>3</sup>)" UniqueName="CO" DataField="CO" EmptyDataText="--"
                                HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="臭氧1(O<sub>3</sub>)<br />浓度(μg/m<sup>3</sup>)" UniqueName="O3" DataField="O3" EmptyDataText="--"
                                HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="臭氧8(O<sub>3</sub>)<br />浓度(μg/m<sup>3</sup>)" UniqueName="Max8HourO3" DataField="Max8HourO3" EmptyDataText="--"
                                HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="PM<sub>2.5</sub><br />浓度(μg/m<sup>3</sup>)" UniqueName="PM25" DataField="PM25" EmptyDataText="--"
                                HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="AQI" UniqueName="AQIValue" DataField="AQIValue" EmptyDataText="--"
                                HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="首要<br />污染物" UniqueName="PrimaryPollutant" DataField="PrimaryPollutant" EmptyDataText="--"
                                HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn HeaderText="指数<br />级别" UniqueName="Grade" DataField="Grade" EmptyDataText="--"
                                HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridTemplateColumn HeaderText="类别" UniqueName="Class"
                                HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%#GetClassRGB(Eval("Class"),Eval("RGBValue")) %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                    <CommandItemStyle Width="100%" />
                    <ClientSettings>
                        <Scrolling AllowScroll="True" EnableVirtualScrollPaging="false" UseStaticHeaders="True"
                            SaveScrollPosition="true"></Scrolling>
                    </ClientSettings>
                </telerik:RadGrid>
            </telerik:RadPane>
            <telerik:RadPane ID="paneChart" runat="server" Width="100%" Height="45%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table style="height: 100%; width: 100%">
                    <tr>
                        <td>
                            <div id="divAirQualityLevel" runat="server" style="height: 100%; width: 60%; min-height: 380px; min-width: 800px"></div>
                        </td>
                        <td>
                            <label id="total" runat="server" style="font-size:30px;"></label>
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="hdAirQuality" runat="server" />
            </telerik:RadPane>
        </telerik:RadSplitter>
    </form>
</body>
</html>
