<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MultiPointlinearCheck.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.QualityControl.MultiPointlinearCheck" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        .td {
            border: solid black 1px;
            background-color: #D9D9D9;
            height: 19px;
        }

        table {
            border: solid #000 1px;
            border-collapse: collapse;
            width: 99%;
        }

        .auto-style1 {
            border: solid black 1px;
            height: 19px;
            text-align: center;
        }

        .text {
            border-style: none;
            text-align: center;
            width: 99%;
            height: 99%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
        <div>
            <p>
                <asp:ImageButton ID="btnSave" runat="server" OnClick="btnSave_Click" SkinID="ImgBtnSave" />
            </p>
            <table>
                <tbody>
                    <tr>
                        <td class="td" style="width: 20%">校准地点：</td>
                        <td class="auto-style1" style="width: 30%">
                            <label id="lblPotr" runat="server" />
                        </td>
                        <td class="td" style="width: 20%">校准日期：</td>
                        <td class="auto-style1" style="width: 30%">
                            <label id="lblTime" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td" style="width: 20%">现场温度：</td>
                        <td class="auto-style1" style="width: 30%">
                            <label id="lblC" runat="server" />
                        </td>
                        <td class="td" style="width: 20%">现场气压：</td>
                        <td class="auto-style1" style="width: 30%">
                            <label id="lblP" runat="server" />
                        </td>
                    </tr>
                </tbody>
            </table>
            <table>
                <tbody>
                    <tr>
                        <td class="td" style="text-align: center; font-size: 18px" colspan="6">分析仪信息</td>
                    </tr>
                    <tr>
                        <td class="auto-style1" style="width: 15%">分析仪型号</td>
                        <td class="auto-style1" style="width: 15%">
                            <input type="text" id="txt1" runat="server" value="" class="text"/></td>
                        <td class="auto-style1" style="width: 15%">分析仪编号</td>
                        <td class="auto-style1" style="width: 15%">
                            <input type="text" id="txt2" runat="server" value="" class="text"/></td>
                        <td class="auto-style1" style="width: 15%">上次校准日期</td>
                        <td class="auto-style1" style="width: 15%">
                            <telerik:RadDatePicker runat="server" ID="RadDatePicker4" Width="99%" Height="99%"></telerik:RadDatePicker>
                        </td>
                    </tr>
                </tbody>
            </table>
            <table>
                <tbody>
                    <tr>
                        <td class="td" style="text-align: center; font-size: 18px" colspan="6">多元校准仪信息</td>
                    </tr>
                    <tr>
                        <td class="auto-style1" style="width: 20%">多元校准仪型号/编号</td>
                        <td class="auto-style1" style="width: 15%">
                            <input type="text" id="text3" runat="server" value="" class="text"/></td>
                        <td class="auto-style1" style="text-align: center;" colspan="3">MFC多点校准曲线</td>
                    </tr>
                    <tr>
                        <td class="td" style="width: 20%"></td>
                        <td class="auto-style1" style="width: 20%">校准时间</td>
                        <td class="auto-style1" style="width: 20%">斜率a</td>
                        <td class="auto-style1" style="width: 20%">斜率b</td>
                        <td class="auto-style1" style="width: 20%">相关系数r</td>
                    </tr>
                    <tr>
                        <td class="auto-style1" style="width: 20%">零空气MFC校准</td>
                        <td class="auto-style1" style="width: 20%">
                            <telerik:RadDatePicker runat="server" ID="RadDatePicker1" Width="99%" Height="99%">
<Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" EnableWeekends="True" Culture="zh-CN" FastNavigationNextText="&amp;lt;&amp;lt;"></Calendar>

<DateInput DisplayDateFormat="yyyy/M/d" DateFormat="yyyy/M/d" LabelWidth="40%" Height="99%">
<EmptyMessageStyle Resize="None"></EmptyMessageStyle>

<ReadOnlyStyle Resize="None"></ReadOnlyStyle>

<FocusedStyle Resize="None"></FocusedStyle>

<DisabledStyle Resize="None"></DisabledStyle>

<InvalidStyle Resize="None"></InvalidStyle>

<HoveredStyle Resize="None"></HoveredStyle>

<EnabledStyle Resize="None"></EnabledStyle>
</DateInput>

<DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                            </telerik:RadDatePicker>
                        </td>
                        <td class="auto-style1" style="width: 20%">
                            <input type="text" id="text4" runat="server" value="" class="text"/></td>
                        <td class="auto-style1" style="width: 20%">
                            <input type="text" id="text1" runat="server" value="" class="text"/></td>
                        <td class="auto-style1" style="width: 20%">
                            <input type="text" id="text2" runat="server" value="" class="text"/></td>
                    </tr>
                    <tr>
                        <td class="auto-style1" style="width: 20%">标准气MFC校准</td>
                        <td class="auto-style1" style="width: 20%">
                            <telerik:RadDatePicker runat="server" ID="RadDatePicker2" Width="99%" Height="99%">
<Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" EnableWeekends="True" Culture="zh-CN" FastNavigationNextText="&amp;lt;&amp;lt;"></Calendar>

<DateInput DisplayDateFormat="yyyy/M/d" DateFormat="yyyy/M/d" LabelWidth="40%" Height="99%">
<EmptyMessageStyle Resize="None"></EmptyMessageStyle>

<ReadOnlyStyle Resize="None"></ReadOnlyStyle>

<FocusedStyle Resize="None"></FocusedStyle>

<DisabledStyle Resize="None"></DisabledStyle>

<InvalidStyle Resize="None"></InvalidStyle>

<HoveredStyle Resize="None"></HoveredStyle>

<EnabledStyle Resize="None"></EnabledStyle>
</DateInput>

<DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                            </telerik:RadDatePicker>
                        </td>
                        <td class="auto-style1" style="width: 20%">
                            <input type="text" id="text5" runat="server" value="" class="text"/></td>
                        <td class="auto-style1" style="width: 20%">
                            <input type="text" id="text6" runat="server" value="" class="text"/></td>
                        <td class="auto-style1" style="width: 20%">
                            <input type="text" id="text7" runat="server" value="" class="text"/></td>
                    </tr>
                </tbody>
            </table>
            <table>
                <tbody>
                    <tr>
                        <td class="td" style="text-align: center; font-size: 18px" colspan="6">零空气发生器信息</td>
                    </tr>
                    <tr>
                        <td class="auto-style1" style="width: 25%">零空气发生器型号/编号</td>
                        <td class="auto-style1" style="width: 25%">
                            <input type="text" id="text8" runat="server" value="" class="text"/></td>
                        <td class="auto-style1" style="width: 25%">零气压力</td>
                        <td class="auto-style1" style="width: 25%">
                            <input type="text" id="text9" runat="server" value="" class="text"/></td>
                    </tr>
                </tbody>
            </table>
            <table>
                <tbody>
                    <tr>
                        <td class="td" style="text-align: center; font-size: 18px" colspan="6">标准物质信息</td>
                    </tr>
                    <tr>
                        <td class="auto-style1" style="width: 20%">标准物质类型</td>
                        <td class="auto-style1" style="width: 20%">标准物质编号</td>
                        <td class="auto-style1" style="width: 20%">标准物质浓度</td>
                        <td class="auto-style1" style="width: 20%">标准物质有限期</td>
                        <td class="auto-style1" style="width: 20%">标准物质级别或标准号</td>
                    </tr>
                    <tr>
                        <td class="auto-style1" style="width: 20%">
                            <select id="select" style="width:99%;height:99%">
                                <option selected="selected"></option>
                                <option>钢气瓶</option>
                            </select>
                        </td>
                        <td class="auto-style1" style="width: 20%">
                            <input type="text" id="text10" runat="server" value="" class="text"/></td>
                        <td class="auto-style1" style="width: 20%">
                            <input type="text" id="text11" runat="server" value="" class="text"/></td>
                        <td class="auto-style1" style="width: 20%">
                            <telerik:RadDatePicker runat="server" ID="RadDatePicker3" Width="99%" Height="99%">
<Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" EnableWeekends="True" Culture="zh-CN" FastNavigationNextText="&amp;lt;&amp;lt;"></Calendar>

<DateInput DisplayDateFormat="yyyy/M/d" DateFormat="yyyy/M/d" LabelWidth="40%" Height="99%">
<EmptyMessageStyle Resize="None"></EmptyMessageStyle>

<ReadOnlyStyle Resize="None"></ReadOnlyStyle>

<FocusedStyle Resize="None"></FocusedStyle>

<DisabledStyle Resize="None"></DisabledStyle>

<InvalidStyle Resize="None"></InvalidStyle>

<HoveredStyle Resize="None"></HoveredStyle>

<EnabledStyle Resize="None"></EnabledStyle>
</DateInput>

<DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                            </telerik:RadDatePicker>
                        </td>
                        <td class="td" style="text-align: center; font-size: 18px" colspan="6">
                            <input type="text" id="text12" runat="server" value="" class="text"/></td>
                    </tr>
                </tbody>
            </table>
            <table>
                <tbody>
                    <tr>
                        <td class="td" style="text-align: center; font-size: 18px" colspan="6">上次多点校准结果</td>
                    </tr>
                    <tr>
                        <td style="border: solid black 1px; width: 30%">截距a:<label id="lblA" runat="server" /></td>
                        <td style="border: solid black 1px; width: 30%">截距b:<label id="lblB" runat="server" /></td>
                        <td style="border: solid black 1px; width: 30%">相关系数r:<label id="lblR" runat="server" /></td>
                    </tr>
                </tbody>
            </table>
            <table>
                <tbody>
                    <tr>
                        <td class="td" style="text-align: center; font-size: 18px" colspan="6">多点线性校准</td>
                    </tr>
                    <tr>
                        <td class="auto-style1" style="width: 10%">校准点</td>
                        <td class="auto-style1" style="width: 30%">测试气体浓度(ppb/ppm)</td>
                        <td class="auto-style1" style="width: 30%">分析仪响应(ppb/ppm)</td>
                        <td class="auto-style1" style="width: 30%">响应误差(ppb/ppb)</td>
                    </tr>
                    <tr>
                        <td class="auto-style1" style="width: 10%">1</td>
                        <td class="auto-style1" style="width: 30%">
                            <input type="text" id="text13" runat="server" value="" class="text"/></td>
                        <td class="auto-style1" style="width: 30%">
                            <input type="text" id="text14" runat="server" value="" class="text"/></td>
                        <td class="auto-style1" style="width: 30%">
                            <input type="text" id="text15" runat="server" value="" class="text"/></td>
                    </tr>
                    <tr>
                        <td class="auto-style1" style="width: 10%">2</td>
                        <td class="auto-style1" style="width: 30%">
                            <input type="text" id="text16" runat="server" value="" class="text"/></td>
                        <td class="auto-style1" style="width: 30%">
                            <input type="text" id="text17" runat="server" value="" class="text"/></td>
                        <td class="auto-style1" style="width: 30%">
                            <input type="text" id="text18" runat="server" value="" class="text"/></td>
                    </tr>
                    <tr>
                        <td class="auto-style1" style="width: 10%">3</td>
                        <td class="auto-style1" style="width: 30%">
                            <input type="text" id="text19" runat="server" value="" class="text"/></td>
                        <td class="auto-style1" style="width: 30%">
                            <input type="text" id="text20" runat="server" value="" class="text"/></td>
                        <td class="auto-style1" style="width: 30%">
                            <input type="text" id="text21" runat="server" value="" class="text"/></td>
                    </tr>
                    <tr>
                        <td class="auto-style1" style="width: 10%">4</td>
                        <td class="auto-style1" style="width: 30%">
                            <input type="text" id="text22" runat="server" value="" class="text"/></td>
                        <td class="auto-style1" style="width: 30%">
                            <input type="text" id="text23" runat="server" value="" class="text"/></td>
                        <td class="auto-style1" style="width: 30%">
                            <input type="text" id="text24" runat="server" value="" class="text"/></td>
                    </tr>
                    <tr>
                        <td class="auto-style1" style="width: 10%">5</td>
                        <td class="auto-style1" style="width: 30%">
                            <input type="text" id="text25" runat="server" value="" class="text"/></td>
                        <td class="auto-style1" style="width: 30%">
                            <input type="text" id="text26" runat="server" value="" class="text"/></td>
                        <td class="auto-style1" style="width: 30%">
                            <input type="text" id="text27" runat="server" value="" class="text"/></td>
                    </tr>
                    <tr>
                        <td class="auto-style1" style="width: 10%">6</td>
                        <td class="auto-style1" style="width: 30%">
                            <input type="text" id="text28" runat="server" value="" class="text"/></td>
                        <td class="auto-style1" style="width: 30%">
                            <input type="text" id="text29" runat="server" value="" class="text"/></td>
                        <td class="auto-style1" style="width: 30%">
                            <input type="text" id="text30" runat="server" value="" class="text"/></td>
                    </tr>
                    <tr>
                        <td class="td" rowspan="2" colspan="2"></td>
                        <td class="auto-style1">平均误差</td>
                        <td class="auto-style1">
                            <label id="Label1" runat="server" />
                        </td>
                    </tr>
                    <tr>

                        <td class="auto-style1">标准偏差</td>
                        <td class="auto-style1">
                            <label id="Label2" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="border: solid black 1px;">截距a:<label id="Label3" runat="server" /></td>
                        <td style="border: solid black 1px;">截距b:<label id="Label4" runat="server" /></td>
                        <td style="border: solid black 1px;">相关系数r:<label id="Label5" runat="server" /></td>
                    </tr>
                    <tr>
                        <td colspan="6" style="border: solid black 1px;">分析仪校准曲线：分析仪实际浓度=斜率(b)×仪器响应+截距(a)</td>
                    </tr>
                </tbody>
            </table>
            <table>
                <tbody>
                    <tr>
                        <td class="auto-style1" style="width: 20%">项目</td>
                        <td class="auto-style1" style="width: 20%">初始响应(ppm)</td>
                        <td class="auto-style1" style="width: 20%">24小时后响应(ppm)</td>
                        <td class="auto-style1" style="width: 20%">漂移(ppm)</td>
                    </tr>
                    <tr>
                        <td class="auto-style1" style="width: 20%">零点24小时漂移</td>
                        <td class="auto-style1" style="width: 20%">
                            <input type="text" id="text31" runat="server" value="" class="text"/></td>
                        <td class="auto-style1" style="width: 20%">
                            <input type="text" id="text32" runat="server" value="" class="text"/></td>
                        <td class="auto-style1" style="width: 20%">
                            <input type="text" id="text33" runat="server" value="" class="text"/></td>
                    </tr>
                    <tr>
                        <td class="auto-style1" style="width: 20%">跨度24小时漂移</td>
                        <td class="auto-style1" style="width: 20%">
                            <input type="text" id="text34" runat="server" value="" class="text"/></td>
                        <td class="auto-style1" style="width: 20%">
                            <input type="text" id="text35" runat="server" value="" class="text"/></td>
                        <td class="auto-style1" style="width: 20%">
                            <input type="text" id="text36" runat="server" value="" class="text"/></td>
                    </tr>
                </tbody>
            </table>
        </div>
    </form>
</body>
</html>
