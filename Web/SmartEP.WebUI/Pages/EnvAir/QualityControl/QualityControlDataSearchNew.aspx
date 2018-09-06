<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QualityControlDataSearchNew.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.QualityControl.QualityControlDataSearchNew" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link type="text/css" rel="stylesheet" href="../../../App_Themes/Neptune/Telerik.Web.UI.Skins/Metro/RadSplitterAudit.css" />
    <style type="text/css">
        .RadCalendar_Neptune .rcTitlebar {
            background: #3A94D3 !important;
            background-image: none !important;
            border: 1px solid #3A94D3 !important;
        }

        #RadCalendar1_Title {
            color: #fff !important;
        }

        .RadCalendar_Neptune .rcTitlebar .RadCalendar_Neptune .rcTitlebar TABLE {
            background: #3A94D3 !important;
        }
    </style>
    <script type="text/javascript">
        function TV2_SetChildNodesCheckStatus(node, isChecked) {
            var childNodes = TV2i_GetChildNodesDiv(node);
            if (childNodes == null)
                return;

            var inputs = WebForm_GetElementsByTagName(childNodes, "INPUT");
            if (inputs == null || inputs.length == 0)
                return;

            for (var i = 0; i < inputs.length; i++) {
                if (IsCheckBox(inputs[i]))
                    inputs[i].checked = isChecked;
            }
        }

        //change parent node checkbox status after child node changed     
        function TV2_NodeOnChildNodeCheckedChanged(tree, node, isChecked) {
            if (node == null)
                return;

            var childNodes = TV2_GetChildNodes(tree, node);

            if (childNodes == null || childNodes.length == 0)
                return;

            var isAllSame = true;

            for (var i = 0; i < childNodes.length; i++) {
                var item = childNodes[i];
                var value = TV2_NodeGetChecked(item);

                if (isChecked != value) {
                    isAllSame = false;
                    break;
                }
            }

            var parent = TV2_GetParentNode(tree, node);
            if (isAllSame) {
                TV2_NodeSetChecked(node, isChecked);
                TV2_NodeOnChildNodeCheckedChanged(tree, parent, isChecked);
            }
            else {
                TV2_NodeSetChecked(node, false);
                TV2_NodeOnChildNodeCheckedChanged(tree, parent, false);
            }
        }

        //get node relative element(etc. checkbox)     
        function TV2_GetNode(tree, element) {
            var id = element.id.replace(tree.id, "");
            id = id.toLowerCase().replace(element.type, "");
            id = tree.id + id;

            var node = document.getElementById(id);
            if (node == null) //leaf node, no "A" node     
                return element;
            return node;
        }

        //get parent node     
        function TV2_GetParentNode(tree, node) {
            var div = WebForm_GetParentByTagName(node, "DIV");

            //The structure of node: <table>information of node</table><div>child nodes</div>     
            var table = div.previousSibling;
            if (table == null)
                return null;

            return TV2i_GetNodeInElement(tree, table);
        }

        //get child nodes array     
        function TV2_GetChildNodes(tree, node) {
            if (TV2_NodeIsLeaf(node))
                return null;

            var children = new Array();
            var div = TV2i_GetChildNodesDiv(node);
            var index = 0;

            for (var i = 0; i < div.childNodes.length; i++) {
                var element = div.childNodes[i];
                if (element.tagName != "TABLE")
                    continue;

                var child = TV2i_GetNodeInElement(tree, element);
                if (child != null)
                    children[index++] = child;
            }
            return children;
        }

        function TV2_NodeIsLeaf(node) {
            return !(node.tagName == "A"); //Todo     
        }

        function TV2_NodeGetChecked(node) {
            var checkbox = TV2i_NodeGetCheckBox(node);
            return checkbox.checked;
        }

        function TV2_NodeSetChecked(node, isChecked) {
            var checkbox = TV2i_NodeGetCheckBox(node);
            if (checkbox != null)
                checkbox.checked = isChecked;
        }

        function IsCheckBox(element) {
            if (element == null)
                return false;
            return (element.tagName == "INPUT" && element.type.toLowerCase() == "checkbox");
        }

        //get tree     
        function TV2_GetTreeById(id) {
            return document.getElementById(id);
        }

        function TV2i_GetChildNodesDiv(node) {
            if (TV2_NodeIsLeaf(node))
                return null;

            var childNodsDivId = node.id + "Nodes";
            return document.getElementById(childNodsDivId);
        }

        //find node in element     
        function TV2i_GetNodeInElement(tree, element) {
            var node = TV2i_GetNodeInElementA(tree, element);
            if (node == null) {
                node = TV2i_GetNodeInElementInput(tree, element);
            }
            return node;
        }

        //find "A" node      
        function TV2i_GetNodeInElementA(tree, element) {
            var as = WebForm_GetElementsByTagName(element, "A");
            if (as == null || as.length == 0)
                return null;

            var regexp = new RegExp("^" + tree.id + "n//d+$");

            for (var i = 0; i < as.length; i++) {
                if (as[i].id.match(regexp)) {
                    return as[i];
                }
            }
            return null;
        }

        //find "INPUT" node     
        function TV2i_GetNodeInElementInput(tree, element) {
            var as = WebForm_GetElementsByTagName(element, "INPUT");
            if (as == null || as.length == 0)
                return null;

            var regexp = new RegExp("^" + tree.id + "n//d+");

            for (var i = 0; i < as.length; i++) {
                if (as[i].id.match(regexp)) {
                    return as[i];
                }
            }
            return null;
        }

        //get checkbox of node     
        function TV2i_NodeGetCheckBox(node) {
            if (IsCheckBox(node))
                return node;

            var id = node.id + "CheckBox";
            return document.getElementById(id);
        }
    </script>
    <telerik:RadCodeBlock ID="RadCodeBlock3" runat="server">
        <script src="../../../Resources/JavaScript/JQuery/jquery-1.9.0.js"></script>

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

            function OnClientClicking() {
                var date1 = new Date();
                var date2 = new Date();
                date1 = $find("<%= RadDatePickerBegin.ClientID %>").get_selectedDate();
                date2 = $find("<%= RadDatePickerEnd.ClientID %>").get_selectedDate();
                if ((date1 == null) || (date2 == null)) {
                    alert("开始时间或者终止时间，不能为空！");
                    //sender.set_autoPostBack(false);
                    return false;
                }
                if (date1 > date2) {
                    alert("开始时间不能大于终止时间！");
                    return false;
                } else {
                    return true;
                }
            }
            //图表刷新
            function RefreshChart() {
                try {
                    var chartPage = document.getElementById("pvChart");
                    chartPage.children[0].contentWindow.InitChart();
                } catch (e) {
                }
            }
            //function InitGroupChart() {
            //    try {
            //            var hiddenData = $("#HiddenData").val().split('|');
            //            var height = parseInt(parseInt($("#pvChart").css("height")) - 65);
            //            groupChartByPointid(hiddenData[0], "../Chart/ChartFrame.aspx", height);

            //    } catch (e) {
            //    }
            //}
            //Chart图形切换
            function ChartTypeChanged(item) {
                try {
                    var chartIframe = document.getElementsByName('chartIframe');
                    //var item = args.get_item().get_value();
                    for (var i = 0; i < chartIframe.length; i++) {
                        document.getElementById(chartIframe[i].id).contentWindow.HighChartTypeChange(item);
                    }
                } catch (e) {
                }
            }
            function onResponseEnd(sender, args) {
                //GridResize();
                //SetCalenderHight();
                //ResizePageDiv();
            }

            //设置容器宽度、高度
            $("document").ready(function () {
                ResizePageDiv();
            });

            function loadSplitter(sender) {
                GridResize();
                SetCalenderHight();
            }

            //蒙版宽度高度设置
            function ResizePageDiv() {
                var bodyWidth = document.body.clientWidth;
                var bodyHeight = document.body.clientHeight;
                $('#pagediv').css("height", bodyHeight);
                $('#pagediv').css("width", bodyWidth);
            }

            //隐藏测点按钮
            function HidePanel(send, args) {
                $('#RAD_SLIDING_PANE_TAB_RadSlidingPane_Point').css("display", "none");
            }

            //显示测点按钮
            function ShowPanel(send, args) {
                $('#RAD_SLIDING_PANE_TAB_RadSlidingPane_Point').css("display", "block");

            }

            //重新设置表格高度
            function GridResize() {
                var bodyHeight = document.body.clientHeight;
                $('#RadGridAnalyze_GridData').css("height", bodyHeight - bodyHeight * 0.58 - 28 - 12);//设置表格高度 
            }

            //设置日历高度
            function SetCalenderHight() {
                var height = (parseFloat($('#RadPane3').css("height")) - 33 - 30 - 60 - 20) / 6;
                if (height > 10)
                    $(".tableAdt").height(height);
                else
                    $(".tableAdt").height(10);
            }

            function OnClientNotificationUpdated(sender, args) {
                var newMsgs = sender.get_value();
                if (newMsgs != 0) {
                    play();
                    sender.show();
                }
            }
            function OnClientNotificationHidden(sender, eventArgs) {
            }

            function CalendarViewChanged(sender, args) {
                //SetCalenderHight();
            }
            function checkAll(button, args) {
                for (var i = 0; i < document.getElementById("radioPoint").getElementsByTagName("input").length; i++) {
                    document.getElementById("radioPoint_" + i).checked = true;
                }
            }
            function deleteAll(button, args) {
                for (var i = 0; i < document.getElementById("radioPoint").getElementsByTagName("input").length; i++) {
                    document.getElementById("radioPoint_" + i).checked = false;
                }
            }
            function ReverseAll(button, args) {
                for (var i = 0; i < document.getElementById("radioPoint").getElementsByTagName("input").length; i++) {
                    var objCheck = document.getElementById("radioPoint_" + i);
                    if (objCheck.checked)
                        objCheck.checked = false;
                    else
                        objCheck.checked = true;
                }
            }
            function OnTreeNodeChecked() {
                var element = element = window.event.srcElement;
                if (!IsCheckBox(element))
                    return;
                var isChecked = element.checked;
                var tree = TV2_GetTreeById("TreeView1");
                var node = TV2_GetNode(tree, element);
                TV2_SetChildNodesCheckStatus(node, isChecked);
                var parent = TV2_GetParentNode(tree, node);
                TV2_NodeOnChildNodeCheckedChanged(tree, parent, isChecked);
            }
            function RowClick(taskCode) {
                var moduleGuide = "";
                OpenFineUIWindow(moduleGuide, "Pages/EnvAir/QualityControl/QualityControlShow.aspx?TaskCode=" + taskCode, "运维表单信息")
            }
        </script>
    </telerik:RadCodeBlock>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script type="text/javascript" src="../../../Resources/JavaScript/FrameJS.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1"
            UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="refreshData">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridDataSearch"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="RadGridAnalyze"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="RadCalendar1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="radioPoint">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="factorCbxRsm"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="selectAll">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="factorCbxRsm"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridRTB">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridDataSearch"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridDataSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridDataSearch"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="TreeView1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="hdInstrument"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ddlSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="factor" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridDataSearch" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="multiPage" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="HiddenData" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenChartType" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
            <ClientEvents OnRequestStart="onRequestStart" OnResponseEnd="onResponseEnd" />
        </telerik:RadAjaxManager>

        <div runat="server" id="pagediv" style="position: absolute; z-index: -1;"></div>
        <telerik:RadSplitter ID="RadSplitter1" runat="server" BorderSize="0" Width="100%" Height="100%" OnClientLoad="loadSplitter">
            <!-- 左侧测点、因子-->
            <telerik:RadPane runat="server" ID="LeftPane" Width="20%">
                <div id="divTree">
                    <telerik:RadTreeView ID="TreeView1" runat="server" CheckBoxes="true" OnNodeCheck="TreeView1_NodeCheck" TriStateCheckBoxes="true" CheckChildNodes="true">
                    </telerik:RadTreeView>
                </div>
            </telerik:RadPane>
            <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Both">
            </telerik:RadSplitBar>
            <!--右侧-->
            <telerik:RadPane ID="MiddlePane" runat="server" Scrolling="Y" Width="78%" Height="100%">
                <telerik:RadSplitter ID="RadSplitter2" runat="server" Orientation="Horizontal" BorderSize="0" Width="100%" Height="100%">
                    <!--头部查询条件绑定-->
                    <telerik:RadPane ID="RadPane2" runat="server" Height="60px" Scrolling="None">
                        <table style="width: 100%; text-align: left">
                            <tr>
                                <%--   <td class="title" style="width: 70px; text-align: center;">仪器编号：
                                </td>
                                <td class="content" style="width: 280px;">
                                    <telerik:RadComboBox runat="server" ID="ddlInstrument" Localization-CheckAllString="全选" Width="280px" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput">
                                    </telerik:RadComboBox>
                                </td>--%>

                                <td class="title" style="width: 70px; text-align: center;">质控类型：
                                </td>
                                <td class="content" style="width: 280px;">
                                    <telerik:RadDropDownList runat="server" ID="ddlSearch" Width="280px" OnSelectedIndexChanged="ddlSearch_SelectedIndexChanged" AutoPostBack="true">
                                        <Items>
                                            <telerik:DropDownListItem Text="Sharp5030颗粒物检查校准记录表" Value="1" />
                                            <telerik:DropDownListItem Text="Thermo1400、1405颗粒物检查校准记录表" Value="2" />
                                            <telerik:DropDownListItem Text="标准流量计检定核查记录表" Value="3" />
                                            <telerik:DropDownListItem Text="臭氧校准仪校准记录表" Value="4" />
                                            <telerik:DropDownListItem Text="氮氧化物分析仪动态校准记录表" Value="5" />
                                            <telerik:DropDownListItem Text="动态校准仪流量（标准气）检查记录表" Value="6" />
                                            <telerik:DropDownListItem Text="动态校准仪流量（稀释气）检查记录表" Value="7" />
                                            <telerik:DropDownListItem Text="零气纯度检查记录表" Value="8" />
                                            <telerik:DropDownListItem Text="气体分析仪精密度检查记录表" Value="9" />
                                            <telerik:DropDownListItem Text="气体分析仪零点、跨度检查与调节记录表" Value="10" />
                                            <telerik:DropDownListItem Text="气体分析仪零漂、标漂检查记录表" Value="11" />
                                            <telerik:DropDownListItem Text="气体分析仪准确度审核记录表" Value="12" />
                                            <telerik:DropDownListItem Text="多点线性校准记录表" Value="13" />
                                        </Items>
                                    </telerik:RadDropDownList>
                                </td>
                                <td class="title" style="width: 70px; text-align: center;">因子：
                                </td>
                                <td class="content" style="width: 240px;">
                                    <telerik:RadComboBox runat="server" ID="factor" Width="240px" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Localization-CheckAllString="全选" CheckedItemsTexts="DisplayAllInInput">
                                    </telerik:RadComboBox>
                                </td>
                                <td class="content" align="left" rowspan="2" style="text-align: center;">
                                    <asp:ImageButton ID="btnSearch" runat="server" OnClientClick="return OnClientClicking()" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 70px; text-align: center;">开始时间：</td>
                                <td style="width: 140px;">
                                    <telerik:RadDateTimePicker runat="server" ID="RadDatePickerBegin" AutoPostBack="false"
                                        MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd HH:mm:ss"
                                        DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                        Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" Width="220px"
                                        Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                        Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                        TimeView-HeaderText="小时" OnSelectedDateChanged="RadDatePickerBegin_SelectedDateChanged">
                                    </telerik:RadDateTimePicker>
                                </td>
                                <td style="width: 70px; text-align: center;">结束时间：</td>
                                <td style="width: 140px;">
                                    <telerik:RadDateTimePicker runat="server" ID="RadDatePickerEnd" AutoPostBack="false"
                                        MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM-dd HH:mm:ss"
                                        DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday"
                                        Calendar-FastNavigationSettings-TodayButtonCaption="当前年月" Width="220px"
                                        Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                        Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                        TimeView-HeaderText="小时" OnSelectedDateChanged="RadDatePickerBegin_SelectedDateChanged">
                                    </telerik:RadDateTimePicker>
                                </td>
                            </tr>
                        </table>
                    </telerik:RadPane>
                    <!--中间grid绑定-->
                    <telerik:RadPane ID="RadPane4" runat="server" Scrolling="Y" Width="78%" Height="70%">
                        <div style="padding: 6px;">
                            <telerik:RadMultiPage ID="multiPage1" runat="server" Width="100%" Height="100%" ScrollBars="Hidden" SelectedIndex="0">
                                <telerik:RadPageView ID="pvGrid" runat="server">
                                    <telerik:RadGrid ID="gridDataSearch" runat="server" GridLines="None"
                                        AllowPaging="True" PageSize="12" AllowCustomPaging="true" AllowSorting="false" ShowFooter="true"
                                        AutoGenerateColumns="true" AllowMultiRowSelection="false"
                                        EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                                        ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0" OnColumnCreated="gridDataSearch_ColumnCreated"
                                        CssClass="RadGrid_Customer" OnNeedDataSource="gridDataSearch_NeedDataSource" OnItemDataBound="gridDataSearch_ItemDataBound" ShowHeader="true">
                                        <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
                                        <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="Top" IsFilterItemExpanded="False"
                                            InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                                            <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true" RefreshText="查询" ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                                            <CommandItemTemplate>
                                                <telerik:RadToolBar BorderWidth="0" ID="gridRTB" SkinID="Query" CssClass="RadToolBar_Customer" AutoPostBack="true"
                                                    runat="server" Width="50%" OnButtonClick="gridRTB_ButtonClick" />
                                            </CommandItemTemplate>
                                            <ColumnGroups>
                                                <telerik:GridColumnGroup Name="零气24小时漂移" HeaderText=""
                                                    HeaderStyle-HorizontalAlign="Center" />
                                                <telerik:GridColumnGroup Name="跨度24小时漂移" HeaderText=""
                                                    HeaderStyle-HorizontalAlign="Center" />
                                                <telerik:GridColumnGroup Name="仪器零点响应" HeaderText=""
                                                    HeaderStyle-HorizontalAlign="Center" />
                                                <telerik:GridColumnGroup Name="仪器跨度响应" HeaderText=""
                                                    HeaderStyle-HorizontalAlign="Center" />
                                                <telerik:GridColumnGroup Name="斜率" HeaderText=""
                                                    HeaderStyle-HorizontalAlign="Center" />
                                                <telerik:GridColumnGroup Name="截距" HeaderText=""
                                                    HeaderStyle-HorizontalAlign="Center" />
                                                <telerik:GridColumnGroup Name="增益" HeaderText=""
                                                    HeaderStyle-HorizontalAlign="Center" />
                                                <telerik:GridColumnGroup Name="零气仪器响应" HeaderText=""
                                                    HeaderStyle-HorizontalAlign="Center" />
                                                <telerik:GridColumnGroup Name="零气参考" HeaderText=""
                                                    HeaderStyle-HorizontalAlign="Center" />
                                            </ColumnGroups>
                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="序号" UniqueName="rowNum" HeaderStyle-Width="50px" HeaderStyle-Height="50px"
                                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <%#Container.DataSetIndex + 1%>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                            <PagerStyle FirstPageToolTip="首页" AlwaysVisible="true" Position="Bottom" NextPageToolTip="下页" LastPageToolTip="尾页" PrevPageToolTip="上页"
                                                PageSizeLabelText="显示记录数:" PageSizes="20 30 40" PagerTextFormat="跳转页面:{4}&amp;nbsp;显示 &lt;strong&gt;{2}&lt;/strong&gt; - &lt;strong&gt;{3}&lt;/strong&gt;条,共 &lt;strong&gt;{5}&lt;/strong&gt;条"></PagerStyle>
                                        </MasterTableView>
                                        <%--<CommandItemStyle Width="100%" />--%>
                                        <ClientSettings>
                                            <Scrolling AllowScroll="True" EnableVirtualScrollPaging="false" UseStaticHeaders="True" FrozenColumnsCount="4"
                                                SaveScrollPosition="true"></Scrolling>
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </telerik:RadPageView>
                            </telerik:RadMultiPage>
                        </div>
                    </telerik:RadPane>
                    <!--底部图表-->
                    <telerik:RadPane ID="paneImage" runat="server" Width="100%" Height="40%" Scrolling="None"
                        BorderWidth="0" BorderStyle="None" BorderSize="0">
                        <telerik:RadMultiPage ID="multiPage" runat="server" Width="100%" Height="100%" ScrollBars="Hidden" SelectedIndex="0">
                            <telerik:RadPageView ID="pvChart" runat="server" ContentUrl="~/Pages/EnvAir/Chart/ChartFrameNew.aspx">
                            </telerik:RadPageView>
                        </telerik:RadMultiPage>
                    </telerik:RadPane>
                </telerik:RadSplitter>
            </telerik:RadPane>
        </telerik:RadSplitter>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Width="1000" Height="1000">
        </telerik:RadAjaxLoadingPanel>
        <input type="button" id="refreshData" style="display: none;" />
        <%--  隐藏域存放选中的因子，在Grid刷新后更新隐藏域数据--%>
        <asp:HiddenField ID="HiddenData" runat="server" Value="" />
        <asp:HiddenField ID="AjaxURL" runat="server" Value="../ChartAjaxRequest/QualityControlDataSearchNew.ashx" />
        <asp:HiddenField ID="HiddenChartType" runat="server" Value="spline" />
        <asp:HiddenField ID="HiddenPoint" runat="server" Value="" />
        <asp:HiddenField ID="hdInstrument" runat="server" Value="" />
    </form>
</body>
</html>
