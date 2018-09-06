<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AirQualityLevel.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Dock.AirQualityLevel" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
                var windowHeight = document.body.clientHeight;//网页可见区域高 屏幕可用工作区高度：window.screen.availHeight 
                var windowWidth = document.body.clientWidth;
                var divAirQualityLevelHeight = (windowHeight - 40).toString() + "px";
                var divAirQualityLevelWidth = (windowWidth).toString() + "px";
                document.getElementById("divAirQualityLevel").style.height = divAirQualityLevelHeight;
                document.getElementById("divAirQualityLevel").style.width = divAirQualityLevelWidth;
                var strAirQuality = document.getElementById("<%=hdAirQuality.ClientID%>").value;
                var AirQuality = JSON.parse(strAirQuality);
                var value1 = [];
                var value2 = [];
                var dataXArr = [];
                var dataYArr = [];
                var colorArr = [];
                var colorList = [];
                var j = 0;
                var rtColor = function (i) {
                    var colo;
                    switch (i) {
                        case "0": colo = '#2FF103';
                            break;
                        case "1": colo = '#FFFF02';
                            break;
                        case "2": colo = '#FF8500';
                            break;
                        case "3": colo = '#FB0000';
                            break;
                        case "4": colo = '#970041';
                            break;
                        case "5": colo = '#7F0020';
                            break;
                    }
                    return colo;
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
                        colorList.push(colorArr);
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
                                  center: ['50%', '50%'],
                                  radius: '80%',
                                  data: dataYArr,
                                  itemStyle: {
                                      normal: {
                                          color: function (params) {
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

            function OnClientClicking() {
                var date1 = $find("<%= dtpBegin.ClientID %>").get_selectedDate();
                var date2 = $find("<%= dtpEnd.ClientID %>").get_selectedDate();
                if ((date1 == null) || (date2 == null)) {
                    alert("开始时间或者终止时间，不能为空！");
                    //sender.set_autoPostBack(false);
                    return false;
                }
                else if (date1 > date2) {
                    alert("开始时间不能大于终止时间！");
                    return false;
                }
                else {
                    return true;
                }
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
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" BorderWidth="0" BorderSize="0" BorderStyle="None"
            Width="100%">
            <telerik:RadPane ID="paneWhere" runat="server" Height="35px" Width="100%" Scrolling="None" MaxHeight="100"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="width: 100%; height: 100%" class="Table_Customer">
                    <tr>
                        <td class="title" style="width: 40px;">日期:
                        </td>
                        <td class="content" style="width: 120px">
                            <telerik:RadDatePicker ID="dtpBegin" runat="server" MinDate="1900-01-01" DateInput-DateFormat="yyyy-MM-dd"
                                DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消" Width="100px" />
                        </td>
                        <td class="title" style="width: 20px;">至</td>
                        <td class="content" style="width: 120px">
                            <telerik:RadDatePicker ID="dtpEnd" runat="server" MinDate="1900-01-01" DateInput-DateFormat="yyyy-MM-dd"
                                DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消" Width="100px" />
                        </td>
                        <td class="content">
                            <asp:ImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" OnClientClick="return OnClientClicking()" SkinID="ImgBtnSearch" />
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneImage" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <!--Step:1 Prepare a dom for ECharts which (must) has size (width & hight)-->
                <!--Step:1 为ECharts准备一个具备大小（宽高）的Dom-->
                <div id="divAirQualityLevel" style="height: 90%; width: 60%; margin-left: +10px;"></div>
                <div style="font-size: 15px; position: fixed; right: 10px; top: 28%; width: 30%; margin-right: +2px; display: none">
                    <label id="total" runat="server" style="color: #588CCC; font-size: 18px; font-weight: 400; width: 100px"></label>
                </div>
                <asp:HiddenField ID="hdAirQuality" runat="server" />
                <asp:HiddenField ID="hdregionUid" runat="server" />
                <asp:HiddenField ID="hdDateBegin" runat="server" />
                <asp:HiddenField ID="hdDateEnd" runat="server" />
            </telerik:RadPane>
        </telerik:RadSplitter>
    </form>
</body>
</html>


