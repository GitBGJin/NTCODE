<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DataEffectRateAnalyze.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.DataAnalyze.DataEffectRateAnalyze" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<%@ Register TagPrefix="CbxRsm" TagName="FactorCbxRsm" Src="~/Controls/FactorCbxRsm.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="../../../Resources/JavaScript/JQuery/jquery-1.10.2.js"></script>
    <script type="text/javascript" language="javascript">

        $(function () {
            $("#dbtMonth,#dbtYear,#dbtWeek,#quarter").hide();//初始化隐藏
            //类型切换
            $("#rbtnlType input[type='radio']").click(function () {

                var typeVale = $(this).val();
                switch (typeVale) {
                    case "day": $("#dbtWeek,#dbtMonth,#quarter,#dbtYear").hide(); $("#dbtDay").show(); break;
                    case "month": $("#dbtDay,#dbtWeek,#quarter,#dbtYear").hide(); $("#dbtMonth").show(); break;
                    case "year": $("#dbtDay,#dbtWeek,#dbtMonth,#quarter").hide(); $("#dbtYear").show(); break;
                    case "week": $("#dbtDay,#dbtMonth,#quarter,#dbtYear").hide(); $("#dbtWeek").show(); break;
                    case "quarter": $("#dbtDay,#dbtMonth,#dbtWeek,#dbtYear").hide(); $("#quarter").show(); break;
                    default: alert("数据类型选择有误，请重新选择"); break;
                }
            });

        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
            <script type="text/javascript" language="javascript">
                var myRadGrid;
                function RadGridCreating(sender, eventArgs) { myRadGrid = sender; }

                //页面刷新
                function Refresh_Grid(args) {
                    if (args) {
                        var MasterTable = $find("<%= gridEffectRateAnalyze.ClientID %>").get_masterTableView();
                        MasterTable.rebind();
                    }
                }

                //控制导出时按钮不会隐藏掉处理
                function onRequestStart(sender, args) {
                    if (args.EventArgument == 6 || args.EventArgument == 7 ||
                        args.get_eventTarget().indexOf("ExportToExcelButton") >= 0 ||
                            args.get_eventTarget().indexOf("ExportToWordButton") >= 0 ||
                            args.get_eventTarget().indexOf("ExportToCsvButton") >= 0) {
                        args.set_enableAjax(false);
                    }
                }

                function OnClientClicking() {
                    var type = $("#rbtnlType input[type='radio']:checked").val();
                    if (type == "day") {
                        var dateBegin = $find("<%= dayBegin.ClientID %>").get_selectedDate();
                        var dateEnd = $find("<%= dayEnd.ClientID %>").get_selectedDate();
                        if (dateBegin == null) {
                            alert("开始时间不能为空！");
                            return false;
                        }
                        else if (dateEnd == null) {
                            alert("截至时间不能为空！");
                            return false;
                        }
                        else if (dateBegin > dateEnd) {
                            alert("开始时间不能大于结束时间！");
                            return false;
                        }
                        return true;
                    }
                    else if (type == "week") {
                        var dateBegin = $find("<%= weekBegin.ClientID %>").get_selectedDate();
                        var dateEnd = $find("<%= weekEnd.ClientID %>").get_selectedDate();
                        var startWeek = $("#weekFrom").val();
                        var endWeek = $("#weekTo").val();
                        if (dateBegin == null) {
                            alert("开始时间不能为空！");
                            return false;
                        }
                        else if (dateEnd == null) {
                            alert("截至时间不能为空！");
                            return false;
                        }
                        else if (dateBegin > dateEnd) {
                            alert("开始时间不能大于结束时间！");
                            return false;
                        }
                        else if (dateBegin = dateEnd) {
                            if (startWeek > endWeek) {
                                alert("开始时间不能大于结束时间！");
                                return false;
                            }
                        }
                        return true;
                    }
                    else if (type == "month") {
                        var dateBegin = $find("<%= monthBegin.ClientID %>").get_selectedDate();
                        var dateEnd = $find("<%= monthEnd.ClientID %>").get_selectedDate();
                        if (dateBegin == null) {
                            alert("开始时间不能为空！");
                            return false;
                        }
                        else if (dateEnd == null) {
                            alert("截至时间不能为空！");
                            return false;
                        }
                        else if (dateBegin > dateEnd) {
                            alert("开始时间不能大于结束时间！");
                            return false;
                        }
                        return true;
                    }
                    else if (type == "quarter") {
                        var dateBegin = $("#ddlQuarterYearBegin span[class='rddlFakeInput']").text();
                        var dateEnd = $("#ddlQuarterYearEnd span[class='rddlFakeInput']").text();

                        var quarterBegin = $("#ddlQuarterBegin").val();
                        var quarterEnd = $("#ddlQuarterEnd").val();

                        if (dateBegin == null) {
                            alert("开始时间不能为空！");
                            return false;
                        }
                        else if (dateEnd == null) {
                            alert("截至时间不能为空！");
                            return false;
                        }
                        else if (dateBegin > dateEnd) {
                            alert("开始时间不能大于结束时间！");
                            return false;
                        }
                        else if (dateBegin = dateEnd) {
                            if (quarterBegin > quarterEnd) {
                                alert("开始时间不能大于结束时间！");
                                return false;
                            }
                        }
                        return true;
                    }
                    else if (type == "year") {
                        var dateBegin = $("#ddlYearBegin span[class='rddlFakeInput']").text();
                        var dateEnd = $("#ddlYearEnd span[class='rddlFakeInput']").text();
                        if (dateBegin == null) {
                            alert("开始时间不能为空！");
                            return false;
                        }
                        else if (dateEnd == null) {
                            alert("截至时间不能为空！");
                            return false;
                        }
                        else if (dateBegin > dateEnd) {
                            alert("开始时间不能大于结束时间！");
                            return false;
                        }
                        return true;
                    }
            return true;

        }
        //行编辑按钮
        function ShowDetails(effectRateInfoId) {
            //判断时间
            var returnValue = OnClientClicking();
            if (!returnValue) {
                return false;
            }
            //类型切换
            var typeVale = $("#rbtnlType input[type='radio']:checked").val();
            var date1 = new Date(); var date2 = new Date();
            var strDate1 = ""; var strDate2 = "";
            var startWeek = ""; var endWeek = "";

            var Parameter = "";// 0 格式为日 1格式为月  2格式为年 3格式为周  4格式季 

            switch (typeVale) {
                case "day": date1 = $find("<%=dayBegin.ClientID %>").get_selectedDate();
                    date2 = $find("<%=dayEnd.ClientID %>").get_selectedDate();
                    strDate1 = date1.getFullYear().toString() + "-" + (date1.getMonth() + 1).toString()
                           + "-" + date1.getDate().toString();
                    strDate2 = date2.getFullYear().toString() + "-" + (date2.getMonth() + 1).toString()
                                  + "-" + date2.getDate().toString();
                    Parameter = "0";
                    break;
                case "month": date1 = $find("<%=monthBegin.ClientID %>").get_selectedDate();
                    date2 = $find("<%=monthEnd.ClientID %>").get_selectedDate();
                    strDate1 = date1.getFullYear().toString() + "-" + (date1.getMonth() + 1).toString();
                    strDate2 = date2.getFullYear().toString() + "-" + (date2.getMonth() + 1).toString();
                    Parameter = "1";

                    break;
                case "year":
                    strDate1 = $('#ddlYearBegin').val();
                    strDate2 = $('#ddlYearEnd').val();
                    Parameter = "2";
                    break;
                case "week":
                    date1 = $find("<%=weekBegin.ClientID %>").get_selectedDate();
                    date2 = $find("<%=weekEnd.ClientID %>").get_selectedDate();
                    strDate1 = date1.getFullYear().toString() + "-" + (date1.getMonth() + 1).toString();
                    strDate2 = date2.getFullYear().toString() + "-" + (date2.getMonth() + 1).toString();
                    startWeek = $("#weekFrom").val();
                    endWeek = $("#weekTo").val();

                    Parameter = "3";

                    break;
                    //季和周传的参数类型相同
                case "quarter": strDate1 = $("#ddlQuarterYearBegin").val(); strDate2 = $("#ddlQuarterYearEnd").val();
                    startWeek = $("#ddlQuarterBegin").val();
                    endWeek = $("#ddlQuarterEnd").val();
                    Parameter = "4";
                    break;
                default: alert("数据类型选择有误，请重新选择"); break;
            }

            var uri = "DataEffectRateAnalyzeInfo.aspx?effectRateInfoId=" + effectRateInfoId
            + "&dtmBegin=" + strDate1 + "&dtmEnd=" + strDate2 + "&parameterType=" + Parameter + "&startWeek=" + startWeek + "&endWeek=" + endWeek + "";
            window.radopen(encodeURI(uri), "DataDataEffectRateAnalyze");
        }
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

        //按钮行处理
        function gridRTB_ClientButtonClicking(sender, args) {
            var masterTable = $find("<%= gridEffectRateAnalyze.ClientID %>").get_masterTableView();
            var CurrentBtn = args.get_item();
            var CurrentBtnName = CurrentBtn.get_text();
            var CurrentBtnCommandName = CurrentBtn.get_commandName();

            args.set_cancel(!OnClientClicking());
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
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridEffectRateAnalyze">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridEffectRateAnalyze" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridRTB">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridEffectRateAnalyze" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="splitter">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridEffectRateAnalyze" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridEffectRateAnalyze" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rbtnlType">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rbtnlType" />
                        <telerik:AjaxUpdatedControl ControlID="comboPort" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="comboCityProper" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="comboCity" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="comboCityModel" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>

                <telerik:AjaxSetting AjaxControlID="weekBegin">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="weekFrom" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>

                <telerik:AjaxSetting AjaxControlID="weekEnd">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="weekTo" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
            <ClientEvents OnRequestStart="onRequestStart" />
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" BorderWidth="0"
            Width="100%">
            <telerik:RadPane ID="paneWhere" runat="server" Height="60px" Width="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <table id="Tb" style="width: 100%; height: 100%" cellspacing="1" cellpadding="0" class="Table_Customer"
                    border="0">
                    <tr>
                        <td class="title" style="width: 60px; text-align: center;">测点:
                        </td>
                        <td class="content" style="width: 200px;">
                            <CbxRsm:PointCbxRsm runat="server" ApplicationType="Air" CbxWidth="220" CbxHeight="350" DefaultAllSelected="false" MultiSelected="true" DropDownWidth="520" ID="pointCbxRsm"></CbxRsm:PointCbxRsm>

                        </td>
                        <td class="title" style="width: 80px; text-align: center;">开始时间:
                        </td>
                        <td class="content" style="width: 500px;">
                            <!--日-->
                            <div id="dbtDay">
                                <telerik:RadDatePicker ID="dayBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                    DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd"
                                    Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                    Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                    Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                    TimeView-HeaderText="小时" />
                                &nbsp;&nbsp;结束时间&nbsp;&nbsp;
                            <telerik:RadDatePicker ID="dayEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM-dd"
                                Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                TimeView-HeaderText="小时" />
                            </div>
                            <!--周-->
                            <div id="dbtWeek">
                                <div style="float: left;">
                                    <telerik:RadMonthYearPicker ID="weekBegin" runat="server" MinDate="1900-01-01" Width="90" BorderWidth="1px" DateInput-DateFormat="yyyy-MM"
                                        DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday"
                                        Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                        Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                        Calendar-FastNavigationSettings-CancelButtonCaption="取消" OnSelectedDateChanged="weekBegin_SelectedDateChanged" AutoPostBack="true" />
                                </div>
                                <div style="float: left;">
                                    &nbsp;&nbsp;第
                           <telerik:RadDropDownList ID="weekFrom" runat="server" Width="40px">
                           </telerik:RadDropDownList>
                                    周
                            &nbsp;&nbsp;结束时间
                                </div>

                                <div style="float: left;">
                                    <telerik:RadMonthYearPicker ID="weekEnd" runat="server" MinDate="1900-01-01" Width="90" BorderWidth="1px" DateInput-DateFormat="yyyy-MM"
                                        DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday"
                                        Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                        Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                        Calendar-FastNavigationSettings-CancelButtonCaption="取消" OnSelectedDateChanged="weekEnd_SelectedDateChanged" AutoPostBack="true" />
                                </div>

                                <div style="float: left;">
                                    &nbsp;&nbsp;第
                            <telerik:RadDropDownList ID="weekTo" runat="server" Width="40px">
                            </telerik:RadDropDownList>
                                    周 
                                </div>
                                <%--      <br />时间：<label runat="server" id="weektime"></label>  --%>
                            </div>
                            <!--月-->
                            <div id="dbtMonth">
                                <div style="float: left;">
                                    <telerik:RadMonthYearPicker ID="monthBegin" runat="server" MinDate="1900-01-01" BorderWidth="1px"
                                        DatePopupButton-ToolTip="打开日历选择" TimePopupButton-ToolTip="打开小时选择" Calendar-FirstDayOfWeek="Monday" DateInput-DateFormat="yyyy-MM"
                                        Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                        Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                        Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                        TimeView-HeaderText="小时" />
                                </div>
                                <div style="float: left;">
                                    &nbsp;&nbsp;结束时间&nbsp;&nbsp;
                                </div>

                                <div style="float: left;">
                                    <telerik:RadMonthYearPicker ID="monthEnd" runat="server" MinDate="1900-01-01" BorderWidth="1px" DateInput-DateFormat="yyyy-MM"
                                        DatePopupButton-ToolTip="打开日历选择" Calendar-FirstDayOfWeek="Monday"
                                        Calendar-FastNavigationSettings-TodayButtonCaption="当前年月"
                                        Calendar-FastNavigationSettings-OkButtonCaption="确定"
                                        Calendar-FastNavigationSettings-CancelButtonCaption="取消"
                                        TimeView-HeaderText="小时" />
                                </div>
                            </div>
                            <!--季-->
                            <div id="quarter">

                                <telerik:RadDropDownList ID="ddlQuarterYearBegin" runat="server" DefaultMessage="Select.." Width="120px" DataTextField="portName" DataValueField="RowGuid"></telerik:RadDropDownList>
                                第
                                <telerik:RadDropDownList ID="ddlQuarterBegin" runat="server" Width="40px">

                                    <Items>
                                        <telerik:DropDownListItem runat="server" Selected="True" Text="1" Value="1" />
                                        <telerik:DropDownListItem runat="server" Value="2" Text="2" />
                                        <telerik:DropDownListItem runat="server" Value="3" Text="3" />
                                        <telerik:DropDownListItem runat="server" Value="4" Text="4" />

                                    </Items>

                                </telerik:RadDropDownList>
                                季
                                &nbsp;&nbsp;结束时间
                                <telerik:RadDropDownList ID="ddlQuarterYearEnd" runat="server" DefaultMessage="Select.." Width="120px" DataTextField="portName" DataValueField="RowGuid"></telerik:RadDropDownList>
                                第
                                <telerik:RadDropDownList ID="ddlQuarterEnd" runat="server" Width="40px">

                                    <Items>
                                        <telerik:DropDownListItem runat="server" Selected="True" Text="1" Value="1" />
                                        <telerik:DropDownListItem runat="server" Value="2" Text="2" />
                                        <telerik:DropDownListItem runat="server" Value="3" Text="3" />
                                        <telerik:DropDownListItem runat="server" Value="4" Text="4" />

                                    </Items>

                                </telerik:RadDropDownList>
                                季
                            </div>
                            <!--年-->
                            <div id="dbtYear">
                                <telerik:RadDropDownList ID="ddlYearBegin" runat="server" DefaultMessage="Select.." Width="120px" DataTextField="portName" DataValueField="RowGuid"></telerik:RadDropDownList>

                                &nbsp;&nbsp;结束时间&nbsp;&nbsp;
                                <telerik:RadDropDownList ID="ddlYearEnd" runat="server" DefaultMessage="Select.." Width="120px" DataTextField="portName" DataValueField="RowGuid"></telerik:RadDropDownList>


                            </div>

                        </td>
                        <td class="content" align="left">
                            <asp:ImageButton ID="btnSearch" runat="server" OnClientClick="return OnClientClicking()" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />
                        </td>
                    </tr>
                    <tr>
                        <td class="title" style="width: 60px; text-align: center;">数据类型:
                        </td>
                        <td class="content" style="width: 200px;">
                            <asp:RadioButtonList ID="rbtnlType" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Selected="True" Value="day">日</asp:ListItem>
                                <asp:ListItem Value="week">周</asp:ListItem>
                                <asp:ListItem Value="month">月</asp:ListItem>
                                <asp:ListItem Value="quarter">季</asp:ListItem>
                                <asp:ListItem Value="year">年</asp:ListItem>
                            </asp:RadioButtonList>

                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <telerik:RadGrid ID="gridEffectRateAnalyze" runat="server" GridLines="None" Height="100%" Width="100%"
                    AllowPaging="True" PageSize="24" AllowCustomPaging="true" AllowSorting="false" ShowFooter="true"
                    AutoGenerateColumns="true" AllowMultiRowSelection="false"
                    EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
                    ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
                    OnNeedDataSource="gridEffectRateAnalyze_NeedDataSource" OnColumnCreated="gridEffectRateAnalyze_ColumnCreated" OnItemDataBound="gridEffectRateAnalyze_ItemDataBound"
                    CssClass="RadGrid_Customer">
                    <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
                    <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="Top" IsFilterItemExpanded="False"
                        InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                        <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true" RefreshText="查询" ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                        <CommandItemTemplate>
                            <telerik:RadToolBar BorderWidth="0" ID="gridRTB" SkinID="Query" CssClass="RadToolBar_Customer" AutoPostBack="true"
                                runat="server" Width="50%" OnButtonClick="gridRTB_ButtonClick" OnClientButtonClicking="gridRTB_ClientButtonClicking" />
                        </CommandItemTemplate>
                        <Columns>
                            <%--<telerik:GridTemplateColumn HeaderText="序号" UniqueName="Row" HeaderStyle-Width="30px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate><%#Container.DataSetIndex + 1 %></ItemTemplate>
                            </telerik:GridTemplateColumn>--%>
                            <%--<telerik:GridTemplateColumn>
                                <HeaderTemplate>
                                    子站名称
                                </HeaderTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <a href='javascript:void(0)' onclick='ShowDetails(<%#Eval("PointId") %>)'><%#Eval("PointName") %></a>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn HeaderText="运行天数" UniqueName="Days" DataField="Days" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" EmptyDataText="--" />
                            <telerik:GridBoundColumn HeaderText="监测数据" UniqueName="ShouldCount" DataField="ShouldCount" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" EmptyDataText="--" />
                            <telerik:GridBoundColumn HeaderText="有效数据" UniqueName="EffectCount" DataField="RealCount" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" EmptyDataText="--" />
                            <telerik:GridBoundColumn HeaderText="数据有效率" UniqueName="EffectRate" DataField="EffectRate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" EmptyDataText="--" />--%>
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
            </telerik:RadPane>
        </telerik:RadSplitter>
        <telerik:RadWindowManager ID="radWM" runat="server" VisibleStatusbar="false" Behaviors="Move,Pin,Reload,Close" EnableShadow="true">
            <Windows>
                <telerik:RadWindow ID="DataDataEffectRateAnalyze" runat="server" Height="510px" Width="800px" Skin="Outlook" IconUrl="~/App_Themes/Office2007/images/RadGridHeaderBg2.png"
                    Title="查看有效率统计详情" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" />
            </Windows>
            <Localization Close="关闭" Maximize="最大化" Minimize="最小化" Restore="还原" Reload="刷新" PinOff="移动"
                PinOn="固定" />
        </telerik:RadWindowManager>
    </form>
</body>
</html>
