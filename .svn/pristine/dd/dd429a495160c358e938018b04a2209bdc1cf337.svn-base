<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AuditSetting.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Audit.AuditSetting" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>监测点审核因子配置界面</title>
    <script type="text/javascript" src="../../../Resources/JavaScript/JQuery/jquery-1.10.2.min.js"></script>
    <style type="text/css">
        .border-table {
            min-width: 500px;
            border-width: 1px;
            margin: 0;
            background: #fff;
        }

            .border-table th {
                font-size: 16px;
                font-weight: bolder;
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

                .border-table td input {
                    vertical-align: middle;
                }

                .border-table td .position {
                    position: relative;
                    min-height: 100%;
                }

            .border-table thead th {
                color: #333;
                font-weight: normal;
                white-space: nowrap;
                text-align: center;
                background: #f9f9f9;
            }

            .border-table tbody th {
                padding-right: 5px;
                text-align: right;
                color: #707070;
                background-color: #f9f9f9;
            }

            .border-table td .cbllist input {
                vertical-align: middle;
            }

            .border-table td .cbllist label {
                margin-right: 5px;
                vertical-align: middle;
            }
    </style>
    <script type="text/javascript">
        $(function () {
            //全选所有
            $("input[name='checkAll_All']").click(function () {
                var obj = $(this).parent().parent().siblings("tr").find("input[type='checkbox']");
                if ($(this).prop("checked") == true) {
                    obj.prop("checked", true);
                    $(this).parent().find("input[name='checkAll_AllShow']").prop("checked", true);
                    $(this).parent().find("input[name='checkAll_AllRead']").prop("checked", true);
                } else {
                    obj.prop("checked", false);
                    $(this).parent().find("input[name='checkAll_AllShow']").prop("checked", false);
                    $(this).parent().find("input[name='checkAll_AllRead']").prop("checked", false);
                }
            });
            //全选所有显示配置
            $("input[name='checkAll_AllShow']").click(function () {
                if ($(this).prop("checked") == true) {
                    $(".border-table").find("tr").each(function (i) {
                        var obj = $(this).find("input[type='checkbox']");
                        var len = obj.length;
                        if (len > 1) {
                            var min = 0, max = len / 2;
                            for (var i = min; i < max; i++) {
                                obj[i].checked = true;
                            }
                        }
                    })
                } else {
                    $(".border-table").find("tr").each(function (i) {
                        var obj = $(this).find("input[type='checkbox']");
                        var len = obj.length;
                        if (len > 1) {
                            var min = 0, max = len / 2;
                            for (var i = min; i < max; i++) {
                                obj[i].checked = false;
                            }
                        }
                    })
                }
            });
            //全选所有只读配置
            $("input[name='checkAll_AllRead']").click(function () {
                if ($(this).prop("checked") == true) {
                    $(".border-table").find("tr").each(function (i) {
                        var obj = $(this).find("input[type='checkbox']");
                        var len = obj.length;
                        if (len > 1) {
                            var min = len / 2, max = len;
                            for (var i = min; i < max; i++) {
                                obj[i].checked = true;
                            }
                        }
                    })
                } else {
                    $(".border-table").find("tr").each(function (i) {
                        var obj = $(this).find("input[type='checkbox']");
                        var len = obj.length;
                        if (len > 1) {
                            var min = len / 2, max = len;
                            for (var i = min; i < max; i++) {
                                obj[i].checked = false;
                            }
                        }
                    })
                }
            });
            //显示权限全选
            $("input[name='checkAll_Show']").click(function () {
                var obj = $(this).parent().siblings("td").find("input[type='checkbox']");
                var len = obj.length;
                if (len > 1) {
                    var min = 0, max = (len - 1) / 2;
                    if ($(this).prop("checked") == true) {
                        for (var i = min; i < max; i++) {
                            obj[i].checked = true;
                        }
                    } else {
                        for (var i = min; i < max; i++) {
                            obj[i].checked = false;
                        }
                    }
                }
            });
            //只读权限全选
            $("input[name='checkAll_Read']").click(function () {
                var obj = $(this).parent().siblings("td").find("input[type='checkbox']");
                var len = obj.length;
                if (len > 1) {
                    var min = (len + 1) / 2, max = len;
                    if ($(this).prop("checked") == true) {
                        for (var i = min; i < max; i++) {
                            obj[i].checked = true;
                        }
                    } else {
                        for (var i = min; i < max; i++) {
                            obj[i].checked = false;
                        }
                    }
                }
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <table class="Table_Customer" style="height: 100%; width: 100%">
            <tr style="height: 50px">
                <td class="title" style="width: 120px">监测点类型：</td>
                <td class="content" style="width: 120px">
                    <asp:DropDownList runat="server" ID="ddlContrlUid" Width="100%"></asp:DropDownList>
                </td>
                <td class="content" style="width: 80px; text-align: center">
                    <asp:ImageButton runat="server" ID="btnSearch" SkinID="ImgBtnSearch" OnClick="btnSearch_Click" />
                </td>
                <td class="content" style="width: 80px; text-align: center">
                    <asp:ImageButton runat="server" ID="btnSave" SkinID="ImgBtnSave" OnClick="btnSave_Click" />
                </td>
                <td class="content" style="text-align: left">
                    <input name="checkAll_AllShow" type="checkbox" />
                    <label>全选所有显示配置</label>
                    <input name="checkAll_AllRead" type="checkbox" />
                    <label>全选所有只读配置</label>
                    <input name="checkAll_All" type="checkbox" />
                    <label>全选所有配置</label>
                </td>
            </tr>
            <tr style="height: 100%">
                <td colspan="5" style="vertical-align: top">
                    <table class="border-table" style="width: 100%">
                        <thead>
                            <tr>
                                <th style="width: 10%;">监测点</th>
                                <th style="width: 5%">全选</th>
                                <th style="width: 40%">显示配置</th>
                                <th style="width: 5%">全选</th>
                                <th style="width: 40%">只读配置</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptList" runat="server" OnItemDataBound="rptList_ItemDataBound">
                                <ItemTemplate>
                                    <tr>
                                        <td style="text-align: center">
                                            <asp:HiddenField ID="hidPointUid" Value='<%#Eval("PointUid") %>' runat="server" />
                                            <asp:HiddenField ID="hidContrlUid" Value='<%#Eval("ContrlUid") %>' runat="server" />
                                            <asp:HiddenField ID="hidPollutantName" Value='<%#Eval("PollutantName") %>' runat="server" />
                                            <asp:HiddenField ID="hidPollutantCode" Value='<%#Eval("PollutantCode") %>' runat="server" />
                                            <asp:HiddenField ID="hidPollutantUid" Value='<%#Eval("PollutantUid") %>' runat="server" />
                                            <asp:HiddenField ID="hidPollutantShow" Value='<%#Eval("PollutantShow") %>' runat="server" />
                                            <asp:HiddenField ID="hidPollutantRead" Value='<%#Eval("PollutantRead") %>' runat="server" />
                                            <%#Eval("PointName")%>
                                        </td>
                                        <td style="text-align: center">
                                            <input name="checkAll_Show" type="checkbox" />
                                        </td>
                                        <td>
                                            <asp:CheckBoxList ID="cblPollutant_Show" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" RepeatColumns="5" CssClass="cbllist"></asp:CheckBoxList>
                                        </td>
                                        <td style="text-align: center">
                                            <input name="checkAll_Read" type="checkbox" />
                                        </td>
                                        <td>
                                            <asp:CheckBoxList ID="cblPollutant_Read" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" RepeatColumns="5" CssClass="cbllist"></asp:CheckBoxList>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
