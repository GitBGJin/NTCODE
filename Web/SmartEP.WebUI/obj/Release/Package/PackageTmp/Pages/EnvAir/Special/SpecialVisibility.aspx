﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SpecialVisibility.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Special.SpecialVisibility" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
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
    <script src="../../../Resources/JavaScript/JQuery/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        //页面加载初始化页面高度
        $(document).ready(function () {
            GetShow();
        });

        //数据行选中事件
        function GetSelect(obj) {
            //添加选中效果
            $(obj).parent().find("tr").each(function (index) {
                if (this.id != "tr_title") {
                    $(this).css('background-color', '#FFFFFF');
                }
            })
            $(obj).css('background-color', '#FFA544');

            //获取图片路径
            var arr = [];
            $(obj).find("input:hidden").each(function (index) {
                arr.push(this.value);
            });
            //清除历史图片
            document.getElementById("divImg").style.background = "#FFFFFF";
            //document.getElementById("divPT").style.background = "#FFFFFF";
            //重新加载图片

            var count = arr.length;
            if (count > 0) {
                GetImgUrl(arr[0]);
                if (count == 2) {
                    GetImgUrl(arr[1])
                }
            }
        }

        //重新加载图片
        function GetImgUrl(imgurl) {
            var filename = imgurl.substring(imgurl.lastIndexOf('/') + 1).toLowerCase();
            var fileurl = imgurl.replace('~', '../../..');
            var rbl = document.getElementsByName("radlDataType");
            document.getElementById("imgName").innerText = "城市影像/能见度";
            //document.getElementById("divImg").style.background = 'url(' + fileurl + ')';
            document.getElementById("divImg").src = fileurl;
            document.getElementById("divImg").style.backgroundSize = '100% 100%';

            console.log(fileurl)
        }
        function onTimer(i) {
            var count = 4;
            var imgId = "tr_" + i;
            var comment = document.getElementById(imgId);
            // IE
            if (document.all) {
                comment.onclick();
            }

                // 其它浏览器
            else {
                var e = document.createEvent("HTMLEvents");
                e.initEvent("click", true, true);
                comment.dispatchEvent(e);
            }
        }

        function GetShow() {
            var maxh = document.body.clientHeight;
            var maxw = document.body.clientWidth;
            document.getElementById("divImg").style.width = maxw - 260 + 'px';
            document.getElementById("divImg").style.height = maxh - 50 + 'px';
            //document.getElementById("divPT").style.width = maxw - 260 + 'px';
            //document.getElementById("divPT").style.height = maxh + 'px';
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
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
                <%--<telerik:AjaxSetting AjaxControlID="radlDataType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="radlDataType" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>--%>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="img"  LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="div1"  LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="imgName"  LoadingPanelID="RadAjaxLoadingPanel1" />
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
                    <%--<div style="text-align: center; width: 100%; vertical-align: central">
                        <asp:RadioButtonList ID="radlDataType" runat="server" TextAlign="Right" RepeatDirection="Horizontal">
                        </asp:RadioButtonList>
                    </div>--%>
                    <br />
                    <br />
                    <asp:ImageButton SkinID="ImgBtnSearch" runat="server" ID="btnSearch" OnClick="btnSearch_Click" />
                    <br />
                    <br />
                    <div style="width: 100%; max-height: 350px; overflow-x: hidden;" runat="server" id="div1"></div>
                    <br />
                    <asp:Timer runat="server" ID="timer" Interval="3000" Enabled="false" OnTick="timer_Tick"></asp:Timer>
                    <table>
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
                    </table>
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
                    <img  runat="server" id="divImg" style=" height:600px;"/>
                    <%--<div runat="server" id="divImg"></div>--%>
                    <%--<iframe runat="server" id="divImg" visible="false"></iframe>--%>
                    <%-- <table runat="server" id="tabImg" style="width: 100%; height: 100%; min-height: 680px;">
                        <tr style="height: 50%">
                            <td style="height: 100%; width: 100%;">
                                <div runat="server" id="divXG"></div>
                            </td>
                        </tr>
                        <tr style="height: 50%">
                            <td style="height: 100%; width: 100%;">
                                <div runat="server" id="divPT"></div>
                            </td>
                        </tr>
                    </table>--%>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
