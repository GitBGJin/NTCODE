<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RunStateInfo.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Dock.RunStateInfo" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
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
                    var divRunStateInfoHeight = (windowHeight +20).toString() + "px";
                    var divRunStateInfoWidth = (windowWidth+20).toString() + "px";
                    document.getElementById("divRunStateInfo").style.height = divRunStateInfoHeight;
                    document.getElementById("divRunStateInfo").style.width = divRunStateInfoWidth;
                    var strRunState = document.getElementById("<%=hdRunState.ClientID%>").value;
                    var RunState = JSON.parse(strRunState);

                    var value1 = [];
                    var value2 = [];
                    var value3 = [];
                    var value4 = [];
                    var value5 = [];
                    $.each(RunState, function (key, obj) {
                        value1.push(obj.TotalCount);
                        value2.push(obj.AlarmCount);
                        value3.push(obj.OfflineCount);
                        value4.push(obj.FailureCount);
                        value5.push(obj.StopCount);
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
                            var myChart = ec.init(document.getElementById('divRunStateInfo'));
                            var opinion = {

                                tooltip: {
                                    trigger: 'axis'
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
                                        data: ["超标", "异常", "故障", "停运"]
                                    }
                                ],
                                yAxis: [
                                    {
                                        name: '站点个数',
                                        type: 'value',
                                        splitArea: { show: true },
                                        boundaryGap: [0, 0.1]
                                    }
                                ],
                                series: [
                                    {

                                        type: 'bar',
                                        stack: 'sum',
                                        barCategoryGap: '40%',
                                        itemStyle: {
                                            normal: {
                                                color: function (params) {
                                                    var colorList = [
                                                    '#DB2F40', '#DC73E8', '#FFA754', '#9695A6'
                                                    ];
                                                    return colorList[params.dataIndex]
                                                },
                                                label: {
                                                    show: true, position: 'insideTop'
                                                }
                                            }

                                        },
                                        data: [value2[0], value3[0], value4[0], value5[0]]
                                    },
                                      {
                                          type: 'bar',
                                          stack: 'sum',
                                          itemStyle: {
                                              normal: {
                                                  color: function (params) {
                                                      var colorList = [
                                                      '#EE8866', '#D2A5E9', '#FFD3AB', '#CBCAD2'
                                                      ];
                                                      return colorList[params.dataIndex]
                                                  },

                                                  //barBorderWidth: 1,
                                                  //barBorderRadius: 0,
                                                  label: {
                                                      show: true,
                                                      position: 'top',
                                                      formatter: function (params) {
                                                          for (var i = 0, l = opinion.xAxis[0].data.length; i < l; i++) {

                                                              return value1;

                                                          }
                                                      },
                                                      textStyle: {
                                                          color: 'tomato',
                                                      }
                                                  }
                                              }
                                          },
                                          data: [value1 - value2[0], value1 - value3[0], value1 - value4[0], value1 - value5[0]]
                                      }
                                ]
                            };
                            myChart.setOption(opinion);
                        }
                    );
                }

                //查询
                function OnClientClicking() {
                    var pointIds = document.getElementById("<%=comboCityProper.ClientID%>").control._value;
                    var json = "{portIds:\"" + pointIds + "\"}";
                    var url = "RunStateInfo.aspx/GetYearData";
                    SetHighcharts(url, json);
                }

                function SetHighcharts(url, json) {
                    if (json == undefined || json == "") {
                        var pointIds = document.getElementById("<%=comboCityProper.ClientID%>").control._value;
                        json = "{dateFrom:\"" + dateFrom + "\",dateTo:\"" + dateTo + "\"}";
                    }

                    $.ajax({
                        type: 'POST',
                        url: url,
                        data: json,
                        datatype: "json",
                        contentType: "application/json; charset=UTF-8",
                        success: function (result) {

                            var tmp = $.parseJSON(result.d);
                            var value_1 = [];
                            var value_2 = [];
                            var value_3 = [];
                            $.each(tmp, function (key, obj) {
                                value_1.push(obj.State);
                                value_2.push(obj.RunStateCount);
                                value_3.push(obj.RunStateRate);
                            });
                            CreateCharts(value_1, value_2)
                        },
                        error: function () {
                        }
                    });
                }

                function CreateCharts(value_1, value_2) {
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
                            opinion = {

                                tooltip: {
                                    trigger: 'axis'
                                },
                                toolbox: {
                                    show: true,
                                    feature: {
                                        mark: { show: true },
                                        dataView: { show: true, readOnly: false },
                                        magicType: { show: false, type: ['line', 'bar'] },
                                        restore: { show: true },
                                        saveAsImage: { show: true }
                                    }
                                },
                                calculable: true,
                                xAxis: [
                                    {
                                        type: 'category',
                                        data: value_1
                                    }
                                ],
                                yAxis: [
                                    {
                                        type: 'value',
                                        splitArea: { show: true }
                                    }
                                ],
                                series: [
                                    {
                                        type: 'bar',
                                        stack: 'sum',
                                        barCategoryGap: '50%',
                                        itemStyle: {
                                            normal: {
                                                color: function (params) {
                                                    var colorList = [
                                                    '#DB2F40', '#DC73E8', '#E2A932', '#6363B1'
                                                    ];
                                                    return colorList[params.dataIndex]
                                                },
                                                label: {
                                                    show: true, position: 'insideTop'
                                                }
                                            }

                                        },
                                        data: value_2
                                    },
                                      {
                                          type: 'bar',
                                          stack: 'sum',
                                          itemStyle: {
                                              normal: {
                                                  color: '#fff',
                                                  barBorderColor: 'tomato',
                                                  barBorderWidth: 4,
                                                  barBorderRadius: 0,
                                                  label: {
                                                      show: true,
                                                      position: 'top',
                                                      formatter: function (params) {
                                                          for (var i = 0, l = opinion.xAxis[0].data.length; i < l; i++) {
                                                              if (opinion.xAxis[0].data[i] == params.name) {
                                                                  return parseInt(opinion.series[0].data[i]) + parseInt(params.value);
                                                              }
                                                          }
                                                      },
                                                      textStyle: {
                                                          color: 'tomato'
                                                      }
                                                  }
                                              }
                                          },
                                          data: value_3
                                      }
                                ]
                            };
                            myChart.setOption(opinion);
                        }
                    );
                }
                function RefreshWindow() {
                    window.location.href = window.location.href;
                }
            </script>
        </telerik:RadScriptBlock>
        <input id="radGridColWidth" type="hidden" runat="server" value="120" />
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="hdRunState" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
            <ClientEvents OnRequestStart="onRequestStart" />
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" BorderWidth="0" BorderSize="0" BorderStyle="None"
            Width="100%">
            <telerik:RadPane ID="paneWhere" runat="server" Height="35px" Width="100%" Scrolling="None" MaxHeight="100"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="width: 100%; height: 100%" cellspacing="1" cellpadding="0" class="Table_Customer"
                    border="0">
                    <tr>
                        <td class="title" style="width: 60px; text-align: center;">区域：
                        </td>
                        <td class="content" style="width: 120px;">
                            <telerik:RadComboBox runat="server" ID="comboCityProper" Localization-CheckAllString="全选" Width="120px" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                            </telerik:RadComboBox>
                        </td>
                         <td style="width: 22%; text-align: center;display:none;">
                            <CbxRsm:PointCbxRsm runat="server" ApplicationType="Air" CbxWidth="280" CbxHeight="350" DefaultAllSelected="true" MultiSelected="true" DropDownWidth="520" ID="pointCbxRsm"></CbxRsm:PointCbxRsm>
                        </td>
                        <td class="content" style="text-align: center;">
                            <asp:ImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneImage" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <!--Step:1 Prepare a dom for ECharts which (must) has size (width & hight)-->
                <!--Step:1 为ECharts准备一个具备大小（宽高）的Dom-->
                <div id="divRunStateInfo" style="height: 600px; width: 100%; padding: 10px; color: #f25a01;margin-top:-50px;margin-left:-20px"></div>
                <asp:HiddenField ID="hdRunState" runat="server" />
            </telerik:RadPane>
        </telerik:RadSplitter>
    </form>
</body>
</html>


