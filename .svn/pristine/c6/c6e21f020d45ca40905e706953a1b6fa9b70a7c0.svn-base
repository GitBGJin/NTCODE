<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AQI-SECDayReportNew.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Report.AQI_SECDayReportNew" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>苏州市城市空气质量日报</title>
    <style type="text/css">
        .table-t table tr td {
            border: thin solid #000000;
        }

        .Title {
            text-align: center;
            font-family: STSong;
            font-size: 11pt;
            font-weight: bold;
        }

        .Content {
            text-align: center;
            vertical-align: central;
            font-family: STSong;
            font-size: 9pt;
            font-weight: bold;
        }

        .TA {
            width: 98%;
            height: 98%;
            border: none;
        }
    </style>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            //关闭遮罩层
            function closeWin() {
                var bgObj = document.getElementById("divbgObj");
                if (bgObj !== null)
                    document.body.removeChild(bgObj);
            }
            //遮罩层
            function alertWin() {
                var iWidth = document.body.clientWidth;
                var iHeight = document.body.clientHeight;

                var bgObj = document.createElement("div");
                bgObj.setAttribute("id", "divbgObj");
                bgObj.style.cssText = "position:absolute;left:0px;top:0px;width:" + iWidth + "px;height:" + Math.max(document.body.clientHeight, iHeight) + "px;filter:Alpha(Opacity=30);opacity:0.3;background: url('../Images/login/BgSpliter.png');background-color:#FEFEFE;z-index:101;text-align:center; vertical-align:middle;color:red;";
                var bgimg = document.createElement("img");
                bgimg.setAttribute("src", "/Skins/Default/Ajax/loading.gif");
                bgObj.appendChild(bgimg);
                document.body.appendChild(bgObj);

            }
            window.onload = function () {
                //遍历页面所有 按钮添加loading效果，目前测试 只用一个
                var target1 = document.getElementById("btnSave");
                var type = "click";
                var func = alertWin;
                if (target1.addEventListener) {
                    target1.addEventListener(type, func, false);
                } else if (target1.attachEvent) {
                    target1.attachEvent("on" + type, func);
                } else {
                    target1["on" + type] = func;
                }
            }

        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1"
            UpdatePanelsRenderMode="Inline">
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" BorderWidth="0" BorderSize="0"
            Width="100%">
            <telerik:RadPane ID="paneWhere" runat="server" Height="40px" Width="100%" Scrolling="None" BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="width: 100%; height: 100%" class="Table_Customer" border="0">
                    <tr>
                        <td class="title" style="width: 100px;">日报时间:</td>
                        <td class="content" style="width: 150px;">
                            <telerik:RadDatePicker runat="server" Width="135px" ID="ReportDate" OnSelectedDateChanged="ReportDate_SelectedDateChanged"
                                MinDate="1900-01-01" DateInput-Font-Size="10" DateInput-Font-Bold="false" DateInput-DateFormat="yyyy-MM-dd" AutoPostBack="true"
                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消" Timeview-HeaderText="小时">
                            </telerik:RadDatePicker>
                        </td>
                        <td class="content" style="width: 100px;">
                            <asp:ImageButton ID="btnSave" runat="server" CssClass="RadToolBar_Customer" SkinID="ImgBtnSave" OnClick="btnSave_Click" />
                        </td>
                        <td></td>
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Scrolling="Y" BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table align="center" style="width: 80%; height: 100%;" class="Table_Customer" border="0">
                    <tr style="text-align: center; font-size: 14pt; font-family: KaiTi; font-weight: bold; height: 40px">
                        <td colspan="4">苏 州 市 环 境 监 测 中 心</td>
                    </tr>
                    <tr style="text-align: center; font-size: 16pt; font-family: SimHei; font-weight: bold; height: 40px">
                        <td colspan="4">苏州市城市空气质量日报表</td>
                    </tr>
                    <tr style="text-align: center; font-size: 11pt; font-family: SimHei; height: 40pt; vertical-align: bottom">
                        <td id="tdReportDate" runat="server"></td>
                        <td id="tdWeek" runat="server"></td>
                        <td>室温：<input type="text" style="width: 100px;" runat="server" id="txtTemp" />
                            ℃</td>
                        <td>天气：
                            <input type="text" style="width: 120px;" runat="server" id="cboWeather" />
                        </td>
                    </tr>
                    <tr class="Title">
                        <td>一、昨日空气质量日报</td>
                        <td></td>
                        <td></td>
                        <td>时间:<input type="text" style="border: none" id="txtTime" runat="server" /></td>
                    </tr>
                    <tr class="Content">
                        <td colspan="4">
                            <div class="table-t">
                                <table style="width: 100%; height: 100%; border-collapse: collapse" runat="server">
                                    <tr class="Content" style="height: 30px;">
                                        <td style="width: 30%">日报结果</td>
                                        <td style="width: 30%">昨日日报</td>
                                        <td style="width: 40%">备注</td>
                                    </tr>
                                    <tr class="Content" style="height: 40px;">
                                        <td>AQI</td>
                                        <td id="AQIValue" runat="server"></td>
                                        <td>
                                            <textarea runat="server" id="taRemark1" class="TA"></textarea></td>
                                    </tr>
                                    <tr class="Content" style="height: 40px;">
                                        <td>首要污染物</td>
                                        <td id="PrimaryPollutant" runat="server"></td>
                                        <td>
                                            <textarea runat="server" id="taRemark2" class="TA"></textarea></td>
                                    </tr>
                                    <tr class="Content" style="height: 40px;">
                                        <td>空气质量类别</td>
                                        <td id="QualityClass" runat="server"></td>
                                        <td>
                                            <textarea runat="server" id="taRemark3" class="TA"></textarea></td>
                                    </tr>
                                    <tr class="Content" style="height: 40px;">
                                        <td>SO2</td>
                                        <td id="SO2" runat="server"></td>
                                        <td>
                                            <textarea runat="server" id="taRemark4" class="TA"></textarea></td>
                                    </tr>
                                    <tr class="Content" style="height: 40px;">
                                        <td>NO2</td>
                                        <td id="NO2" runat="server"></td>
                                        <td>
                                            <textarea runat="server" id="taRemark5" class="TA"></textarea></td>
                                    </tr>
                                    <tr class="Content" style="height: 40px;">
                                        <td>PM10</td>
                                        <td id="PM10" runat="server"></td>
                                        <td>
                                            <textarea runat="server" id="taRemark6" class="TA"></textarea></td>
                                    </tr>
                                    <tr class="Content" style="height: 40px;">
                                        <td>CO</td>
                                        <td id="CO" runat="server"></td>
                                        <td>
                                            <textarea runat="server" id="taRemark7" class="TA"></textarea></td>
                                    </tr>
                                    <tr class="Content" style="height: 40px;">
                                        <td>O3-8h</td>
                                        <td id="O38h" runat="server"></td>
                                        <td>
                                            <textarea runat="server" id="taRemark8" class="TA"></textarea></td>
                                    </tr>
                                    <tr class="Content" style="height: 40px;">
                                        <td>PM2.5</td>
                                        <td id="PM25" runat="server"></td>
                                        <td>
                                            <textarea runat="server" id="taRemark9" class="TA"></textarea></td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr class="Title">
                        <td>二、日报推送情况</td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr class="Content">
                        <td colspan="4">
                            <div class="table-t">
                                <table style="width: 100%; height: 100%; border-collapse: collapse" runat="server">
                                    <tr class="Content" style="height: 30px;">
                                        <td style="width: 7%;">编号</td>
                                        <td style="width: 20%;">单位</td>
                                        <td style="width: 15%;">发送结果</td>
                                        <td style="width: 15%;">发送时刻</td>
                                        <td style="width: 43%">备注</td>
                                    </tr>
                                    <tr class="Content" style="height: 30px;">
                                        <td>1</td>
                                        <td>国家日报平台上报</td>
                                        <td>
                                            <telerik:RadComboBox runat="server" ID="cboSendResult1">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="成功" Value="1" Selected="true" />
                                                    <telerik:RadComboBoxItem Text="失败" Value="0" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </td>
                                        <td>
                                            <telerik:RadComboBox runat="server" ID="cboSendTime1">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="08:00" Value="8" />
                                                    <telerik:RadComboBoxItem Text="09:00" Value="9" />
                                                    <telerik:RadComboBoxItem Text="10:00" Value="10" Selected="true" />
                                                    <telerik:RadComboBoxItem Text="11:00" Value="11" />
                                                    <telerik:RadComboBoxItem Text="12:00" Value="12" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </td>
                                        <td rowspan="4">
                                            <textarea runat="server" id="taRemark10" style="height: 118px; width: 98%; border: none"></textarea>
                                        </td>
                                    </tr>
                                    <tr class="Content" style="height: 30px;">
                                        <td>2</td>
                                        <td>环保局网站信息更新</td>
                                        <td>
                                            <telerik:RadComboBox runat="server" ID="cboSendResult2">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="成功" Value="1" Selected="true" />
                                                    <telerik:RadComboBoxItem Text="失败" Value="0" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </td>
                                        <td>
                                            <telerik:RadComboBox runat="server" ID="cboSendTime2">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="08:00" Value="8" />
                                                    <telerik:RadComboBoxItem Text="09:00" Value="9" />
                                                    <telerik:RadComboBoxItem Text="10:00" Value="10" Selected="true" />
                                                    <telerik:RadComboBoxItem Text="11:00" Value="11" />
                                                    <telerik:RadComboBoxItem Text="12:00" Value="12" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr class="Content" style="height: 30px;">
                                        <td>3</td>
                                        <td>省监测中心上报</td>
                                        <td>
                                            <telerik:RadComboBox runat="server" ID="cboSendResult3">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="成功" Value="1" Selected="true" />
                                                    <telerik:RadComboBoxItem Text="失败" Value="0" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </td>
                                        <td>
                                            <telerik:RadComboBox runat="server" ID="cboSendTime3">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="08:00" Value="8" />
                                                    <telerik:RadComboBoxItem Text="09:00" Value="9" />
                                                    <telerik:RadComboBoxItem Text="10:00" Value="10" Selected="true" />
                                                    <telerik:RadComboBoxItem Text="11:00" Value="11" />
                                                    <telerik:RadComboBoxItem Text="12:00" Value="12" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr class="Content" style="height: 30px;">
                                        <td>4</td>
                                        <td>科室ftp共享更新</td>
                                        <td>
                                            <telerik:RadComboBox runat="server" ID="cboSendResult4">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="成功" Value="1" Selected="true" />
                                                    <telerik:RadComboBoxItem Text="失败" Value="0" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </td>
                                        <td>
                                            <telerik:RadComboBox runat="server" ID="cboSendTime4">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="08:00" Value="8" />
                                                    <telerik:RadComboBoxItem Text="09:00" Value="9" />
                                                    <telerik:RadComboBoxItem Text="10:00" Value="10" Selected="true" />
                                                    <telerik:RadComboBoxItem Text="11:00" Value="11" />
                                                    <telerik:RadComboBoxItem Text="12:00" Value="12" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr class="Title">
                        <td>三、仪器、数据情况记录</td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr class="Content">
                        <td colspan="4">
                            <div class="table-t">
                                <table style="width: 100%; height: 100%; border-collapse: collapse" runat="server" id="tRecords">
                                    <tr class="Content" style="height: 30px;">
                                        <td style="width: 25%;">子站名称</td>
                                        <td style="width: 15%;">异常参数</td>
                                        <td style="width: 35%;">数据剔除时间段</td>
                                        <td style="width: 25%;">剔除说明</td>
                                    </tr>
                                    <tr class="Content" style="height: 30px;">
                                        <td>
                                            <telerik:RadComboBox runat="server" Width="98%" ID="txtPointName1" OnSelectedIndexChanged="PointName1_SelectedIndexChanged" AutoPostBack="true" Localization-CheckAllString="全选" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                                            </telerik:RadComboBox>
                                        </td>
                                        <td>
                                            <telerik:RadComboBox runat="server" ID="cboExecPara1" CheckBoxes="true" CheckedItemsTexts="DisplayAllInInput" EmptyMessage="请选择" Visible="false">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="SO2" Value="SO2" />
                                                    <telerik:RadComboBoxItem Text="NO2" Value="NO2" />
                                                    <telerik:RadComboBoxItem Text="PM10" Value="PM10" />
                                                    <telerik:RadComboBoxItem Text="CO" Value="CO" />
                                                    <telerik:RadComboBoxItem Text="O3-8h" Value="O3-8h" />
                                                    <telerik:RadComboBoxItem Text="PM2.5" Value="PM2.5" />
                                                    <telerik:RadComboBoxItem Text="所有参数" Value="所有参数" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </td>
                                        <td>
                                            <input id="cboEliminateTime1" runat="server" class="TA" style="text-align: center" /></td>
                                        <td>
                                            <telerik:RadComboBox runat="server" ID="cboEliminateRemark1" EmptyMessage="请选择" Width="90%" Height="100%" Visible="false">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="数据有效性不足" Value="1" />
                                                    <telerik:RadComboBoxItem Text="仪器故障" Value="2" />
                                                    <telerik:RadComboBoxItem Text="断电" Value="3" />
                                                    <telerik:RadComboBoxItem Text="校准" Value="4" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr class="Content" style="height: 30px;">
                                        <td>
                                            <telerik:RadComboBox runat="server" Width="98%" ID="txtPointName2" OnSelectedIndexChanged="PointName2_SelectedIndexChanged" AutoPostBack="true" Localization-CheckAllString="全选" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                                            </telerik:RadComboBox>
                                        </td>
                                        <td>
                                            <telerik:RadComboBox runat="server" ID="cboExecPara2" CheckBoxes="true" CheckedItemsTexts="DisplayAllInInput" EmptyMessage="请选择" Visible="false">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="SO2" Value="SO2" />
                                                    <telerik:RadComboBoxItem Text="NO2" Value="NO2" />
                                                    <telerik:RadComboBoxItem Text="PM10" Value="PM10" />
                                                    <telerik:RadComboBoxItem Text="CO" Value="CO" />
                                                    <telerik:RadComboBoxItem Text="O3-8h" Value="O3-8h" />
                                                    <telerik:RadComboBoxItem Text="PM2.5" Value="PM2.5" />
                                                    <telerik:RadComboBoxItem Text="所有参数" Value="所有参数" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </td>
                                        <td>
                                            <input id="cboEliminateTime2" runat="server" class="TA" style="text-align: center" /></td>
                                        <td>
                                            <telerik:RadComboBox runat="server" ID="cboEliminateRemark2" Width="90%" Height="100%" EmptyMessage="请选择" Visible="false">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="数据有效性不足" Value="1" />
                                                    <telerik:RadComboBoxItem Text="仪器故障" Value="2" />
                                                    <telerik:RadComboBoxItem Text="断电" Value="3" />
                                                    <telerik:RadComboBoxItem Text="校准" Value="4" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr class="Content" style="height: 30px;">
                                        <td>
                                            <telerik:RadComboBox runat="server" Width="98%" ID="txtPointName3" OnSelectedIndexChanged="PointName3_SelectedIndexChanged" AutoPostBack="true" Localization-CheckAllString="全选" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                                            </telerik:RadComboBox>
                                        </td>
                                        <td>
                                            <telerik:RadComboBox runat="server" ID="cboExecPara3" CheckBoxes="true" CheckedItemsTexts="DisplayAllInInput" EmptyMessage="请选择" Visible="false">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="SO2" Value="SO2" />
                                                    <telerik:RadComboBoxItem Text="NO2" Value="NO2" />
                                                    <telerik:RadComboBoxItem Text="PM10" Value="PM10" />
                                                    <telerik:RadComboBoxItem Text="CO" Value="CO" />
                                                    <telerik:RadComboBoxItem Text="O3-8h" Value="O3-8h" />
                                                    <telerik:RadComboBoxItem Text="PM2.5" Value="PM2.5" />
                                                    <telerik:RadComboBoxItem Text="所有参数" Value="所有参数" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </td>
                                        <td>
                                            <input id="cboEliminateTime3" runat="server" class="TA" style="text-align: center" />
                                        </td>
                                        <td>
                                            <telerik:RadComboBox runat="server" ID="cboEliminateRemark3" Width="90%" Height="100%" EmptyMessage="请选择" Visible="false">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="数据有效性不足" Value="1" />
                                                    <telerik:RadComboBoxItem Text="仪器故障" Value="2" />
                                                    <telerik:RadComboBoxItem Text="断电" Value="3" />
                                                    <telerik:RadComboBoxItem Text="校准" Value="4" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr class="Content" style="height: 30px;">
                                        <td>
                                            <telerik:RadComboBox runat="server" Width="98%" ID="txtPointName4" OnSelectedIndexChanged="PointName4_SelectedIndexChanged" AutoPostBack="true" Localization-CheckAllString="全选" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                                            </telerik:RadComboBox>
                                        </td>
                                        <td>
                                            <telerik:RadComboBox runat="server" ID="cboExecPara4" CheckBoxes="true" CheckedItemsTexts="DisplayAllInInput" EmptyMessage="请选择" Visible="false">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="SO2" Value="SO2" />
                                                    <telerik:RadComboBoxItem Text="NO2" Value="NO2" />
                                                    <telerik:RadComboBoxItem Text="PM10" Value="PM10" />
                                                    <telerik:RadComboBoxItem Text="CO" Value="CO" />
                                                    <telerik:RadComboBoxItem Text="O3-8h" Value="O3-8h" />
                                                    <telerik:RadComboBoxItem Text="PM2.5" Value="PM2.5" />
                                                    <telerik:RadComboBoxItem Text="所有参数" Value="所有参数" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </td>
                                        <td>
                                            <input id="cboEliminateTime4" runat="server" class="TA" style="text-align: center" />
                                        </td>
                                        <td>
                                            <telerik:RadComboBox runat="server" ID="cboEliminateRemark4" Width="90%" Height="100%" EmptyMessage="请选择" Visible="false">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="数据有效性不足" Value="1" />
                                                    <telerik:RadComboBoxItem Text="仪器故障" Value="2" />
                                                    <telerik:RadComboBoxItem Text="断电" Value="3" />
                                                    <telerik:RadComboBoxItem Text="校准" Value="4" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr class="Content" style="height: 30px;">
                                        <td>
                                            <telerik:RadComboBox runat="server" Width="98%" ID="txtPointName5" OnSelectedIndexChanged="PointName5_SelectedIndexChanged" AutoPostBack="true" Localization-CheckAllString="全选" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                                            </telerik:RadComboBox>
                                        </td>
                                        <td>
                                            <telerik:RadComboBox runat="server" ID="cboExecPara5" CheckBoxes="true" CheckedItemsTexts="DisplayAllInInput" EmptyMessage="请选择" Visible="false">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="SO2" Value="SO2" />
                                                    <telerik:RadComboBoxItem Text="NO2" Value="NO2" />
                                                    <telerik:RadComboBoxItem Text="PM10" Value="PM10" />
                                                    <telerik:RadComboBoxItem Text="CO" Value="CO" />
                                                    <telerik:RadComboBoxItem Text="O3-8h" Value="O3-8h" />
                                                    <telerik:RadComboBoxItem Text="PM2.5" Value="PM2.5" />
                                                    <telerik:RadComboBoxItem Text="所有参数" Value="所有参数" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </td>
                                        <td>
                                            <input id="cboEliminateTime5" runat="server" class="TA" style="text-align: center" />
                                        </td>
                                        <td>
                                            <telerik:RadComboBox runat="server" ID="cboEliminateRemark5" Width="90%" Height="100%" EmptyMessage="请选择" Visible="false">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="数据有效性不足" Value="1" />
                                                    <telerik:RadComboBoxItem Text="仪器故障" Value="2" />
                                                    <telerik:RadComboBoxItem Text="断电" Value="3" />
                                                    <telerik:RadComboBoxItem Text="校准" Value="4" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr class="Content" style="height: 30px;">
                                        <td>
                                            <telerik:RadComboBox runat="server" Width="98%" ID="txtPointName6" OnSelectedIndexChanged="PointName6_SelectedIndexChanged" AutoPostBack="true" Localization-CheckAllString="全选" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                                            </telerik:RadComboBox>
                                        </td>
                                        <td>
                                            <telerik:RadComboBox runat="server" ID="cboExecPara6" CheckBoxes="true" CheckedItemsTexts="DisplayAllInInput" EmptyMessage="请选择" Visible="false">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="SO2" Value="SO2" />
                                                    <telerik:RadComboBoxItem Text="NO2" Value="NO2" />
                                                    <telerik:RadComboBoxItem Text="PM10" Value="PM10" />
                                                    <telerik:RadComboBoxItem Text="CO" Value="CO" />
                                                    <telerik:RadComboBoxItem Text="O3-8h" Value="O3-8h" />
                                                    <telerik:RadComboBoxItem Text="PM2.5" Value="PM2.5" />
                                                    <telerik:RadComboBoxItem Text="所有参数" Value="所有参数" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </td>
                                        <td>
                                            <input id="cboEliminateTime6" runat="server" class="TA" style="text-align: center" />
                                        </td>
                                        <td>
                                            <telerik:RadComboBox runat="server" ID="cboEliminateRemark6" Width="90%" Height="100%" EmptyMessage="请选择" Visible="false">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="数据有效性不足" Value="1" />
                                                    <telerik:RadComboBoxItem Text="仪器故障" Value="2" />
                                                    <telerik:RadComboBoxItem Text="断电" Value="3" />
                                                    <telerik:RadComboBoxItem Text="校准" Value="4" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr class="Content" style="height: 30px;">
                                        <td>
                                            <telerik:RadComboBox runat="server" Width="98%" ID="txtPointName7" OnSelectedIndexChanged="PointName7_SelectedIndexChanged" AutoPostBack="true" Localization-CheckAllString="全选" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                                            </telerik:RadComboBox>
                                        </td>
                                        <td>
                                            <telerik:RadComboBox runat="server" ID="cboExecPara7" CheckBoxes="true" CheckedItemsTexts="DisplayAllInInput" EmptyMessage="请选择" Visible="false">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="SO2" Value="SO2" />
                                                    <telerik:RadComboBoxItem Text="NO2" Value="NO2" />
                                                    <telerik:RadComboBoxItem Text="PM10" Value="PM10" />
                                                    <telerik:RadComboBoxItem Text="CO" Value="CO" />
                                                    <telerik:RadComboBoxItem Text="O3-8h" Value="O3-8h" />
                                                    <telerik:RadComboBoxItem Text="PM2.5" Value="PM2.5" />
                                                    <telerik:RadComboBoxItem Text="所有参数" Value="所有参数" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </td>
                                        <td>
                                            <input id="cboEliminateTime7" runat="server" class="TA" style="text-align: center" />
                                        </td>
                                        <td>
                                            <telerik:RadComboBox runat="server" ID="cboEliminateRemark7" Width="90%" Height="100%" EmptyMessage="请选择" Visible="false">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="数据有效性不足" Value="1" />
                                                    <telerik:RadComboBoxItem Text="仪器故障" Value="2" />
                                                    <telerik:RadComboBoxItem Text="断电" Value="3" />
                                                    <telerik:RadComboBoxItem Text="校准" Value="4" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr class="Content" style="height: 30px;">
                                        <td>
                                            <telerik:RadComboBox runat="server" Width="98%" ID="txtPointName8" OnSelectedIndexChanged="PointName8_SelectedIndexChanged" AutoPostBack="true" Localization-CheckAllString="全选" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                                            </telerik:RadComboBox>
                                        </td>
                                        <td>
                                            <telerik:RadComboBox runat="server" ID="cboExecPara8" CheckBoxes="true" CheckedItemsTexts="DisplayAllInInput" EmptyMessage="请选择" Visible="false">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="SO2" Value="SO2" />
                                                    <telerik:RadComboBoxItem Text="NO2" Value="NO2" />
                                                    <telerik:RadComboBoxItem Text="PM10" Value="PM10" />
                                                    <telerik:RadComboBoxItem Text="CO" Value="CO" />
                                                    <telerik:RadComboBoxItem Text="O3-8h" Value="O3-8h" />
                                                    <telerik:RadComboBoxItem Text="PM2.5" Value="PM2.5" />
                                                    <telerik:RadComboBoxItem Text="所有参数" Value="所有参数" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </td>
                                        <td>
                                            <input id="cboEliminateTime8" runat="server" class="TA" style="text-align: center" />
                                        </td>
                                        <td>
                                            <telerik:RadComboBox runat="server" ID="cboEliminateRemark8" Width="90%" Height="100%" EmptyMessage="请选择" Visible="false">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="数据有效性不足" Value="1" />
                                                    <telerik:RadComboBoxItem Text="仪器故障" Value="2" />
                                                    <telerik:RadComboBoxItem Text="断电" Value="3" />
                                                    <telerik:RadComboBoxItem Text="校准" Value="4" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr class="Content" style="height: 60px;">
                                        <td>数据补传说明：</td>
                                        <td colspan="3">
                                            <textarea runat="server" id="taRemark11" style="height: 58px; width: 98%; border: none"></textarea>
                                        </td>
                                    </tr>
                                    <tr class="Content" style="height: 60px;">
                                        <td>当日备注：</td>
                                        <td colspan="3">
                                            <textarea runat="server" id="taRemark12" style="height: 58px; width: 98%; border: none"></textarea>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr style="font-size: 10pt; font-family: STSong; font-weight: bold; height: 60px;">
                        <td colspan="4">
                            <table style="width: 100%; height: 100%; border-collapse: collapse" runat="server">
                                <tr>
                                    <td style="width: 33%">编制：<input type="text" id="txtAuthor" runat="server" /></td>
                                    <td style="width: 33%">审核：<input type="text" id="txtAuditor" runat="server" /></td>
                                    <td style="width: 33%">签发：<input type="text" id="txtSinger" runat="server" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
        </telerik:RadSplitter>
    </form>
</body>
</html>

