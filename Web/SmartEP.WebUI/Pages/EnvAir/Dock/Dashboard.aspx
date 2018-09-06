<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Dock.Dashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script type="text/javascript" src="../../../Resources/JavaScript/FrameJS.js"></script>
    <style>
        .SpanTitle {
            font-size: 16px;
            color: #6995CA;
            vertical-align: middle;
            font-family: 'Microsoft YaHei',SimSun;
            margin-top: 8px;
            margin-left: 10px;
        }

        .fieldsetTitle {
            /*padding-left: 10px;
            padding-right: 10px;
            padding-bottom: 10px;*/
            border-color: #E6F2FE;
            border: 1px solid #CECDCD;
            background-color: white;
        }

        .divTitle {
            width: 100%;
            height: 25px;
        }
    </style>
    <script type="text/javascript">
        function ShowEditForm(id) {
            var oWnd = radopen("../Alarm/AlarmHandle.aspx?AlarmUid=" + id, "AlarmHandle");
            return false;
        }
        function onclickS(){
            var childStatus = document.getElementById("iframe1").contentWindow;
            if(childStatus.GetData!=undefined){
                var ChlidData=childStatus.GetData();
                var pointNames=ChlidData.PointNames;
                var FactorName=ChlidData.FactorName;
                OpenFineUIWindow("e6137d5a-8393-45dd-bcd1-e018a6fcadd6", "Pages/EnvAir/RealTimeData/RealTimeData.aspx?pointNames="+pointNames+"&FactorName="+FactorName, "实时数据");
                return false;
            }
        }

        function onclickQ(){
            var childStatus = document.getElementById("iframe4").contentWindow;
            if(childStatus.GetData!=undefined){
                var ChlidData=childStatus.GetData();
                var regionUid=ChlidData.regionUid;
                var DateBegin=ChlidData.DateBegin;
                var DateEnd=ChlidData.DateEnd;
                OpenFineUIWindow("bc1e261c-3d83-4acd-9299-3a0b81beb7a1", "Pages/EnvAir/Report/AirQualityDayReport.aspx?regionUid="+regionUid+"&DateBegin="+DateBegin+"&DateEnd="+DateEnd, "空气质量日报");
                return false;
            }
        }

        function onclickAlertInfo(){
            var childStatus = document.getElementById("iframe5").contentWindow;
            if(childStatus.GetData!=undefined){
                var ChlidData=childStatus.GetData();
                var pointId=ChlidData.pointid;
                var DateBegin=ChlidData.beginTime;
                var DateEnd=ChlidData.endTime;
                OpenFineUIWindow("bc1e261c-3d83-4acd-9299-3a0b81beb7a1", "Pages/EnvAir/Alarm/AlarmCompsite.aspx?pointId="+pointId+"&DateBegin="+DateBegin+"&DateEnd="+DateEnd, "报警信息");
                return false;
            }
        }

        function onclickRealTime(){
            var childStatus = document.getElementById("iframe3").contentWindow;
            if(childStatus.GetData!=undefined){
                var ChlidData=childStatus.GetData();
                var cityTypeUids=ChlidData.CityTypeUids;
                OpenFineUIWindow("d2c46160-b6c0-4a75-a2ab-0509a44e0754", "Pages/EnvAir/RealTimeData/RealTimeAirQuality.aspx?CityTypeUids="+cityTypeUids, "实时环境空气质量");
                return false;
            }
        }
        function onclickRealOnline(){
            
            var childStatus = document.getElementById("iframeOCM").contentWindow;
            if(childStatus.GetData!=undefined){
                var ChlidData=childStatus.GetData();
                var pointNames=ChlidData.PointNames;
                var FactorName=ChlidData.FactorName;
                
                OpenFineUIWindow("49502372-21c4-440b-9243-e71571712dba", "Pages/EnvAir/RealTimeData/RealTimeOnlineState.aspx?pointNames="+pointNames+"&FactorName="+FactorName, "实时在线状态信息");
                return false;
            }

            
        }
        function Refresh_Grid(args)
        {
            
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxManager ID="ajaxManager" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="gridAlarm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridAlarm" LoadingPanelID="radAjaxLoadingPanel2" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <table style="width: 99%; margin: auto;">
            <tr>
                <td style="width: 50%;">
                    <div class="fieldsetTitle">
                        <div class="divTitle">
                            <div class="SpanTitle">
                                实时在线信息
                                 <span style="float: right; font-size: 12px; margin-top: 6px; margin-bottom: 0;">
                                     <a href="#" onclick="return onclickRealOnline()">更多..</a>
                                 </span>
                            </div>
                        </div>
                        <div style="height: 320px; overflow: hidden; border-top: 3px solid #6995CA;" id="divToday">
                            <iframe id="iframeOCM" src="OnlineInfo.aspx" style="width: 100%; height: 320px;"
                                marginheight="0" marginwidth="0" frameborder="0" scrolling="no" runat="server"></iframe>
                        </div>
                    </div>
                </td>
                <%-- <td style="width: 50%;">
                    <div class="fieldsetTitle">
                        <div class="divTitle">
                            <div class="SpanTitle">运行状态</div>
                        </div>
                        <div style="height: 320px; overflow: hidden; border-top: 3px solid #6995CA;">
                            <iframe id="iframe2" src="RunStateInfo.aspx" style="width: 100%; height: 320px;"
                                marginheight="0" marginwidth="0" frameborder="0" scrolling="no" runat="server"></iframe>
                        </div>
                    </div>
                </td>--%>
                <td style="width: 50%;">
                    <div class="fieldsetTitle">
                        <div class="divTitle">
                            <div class="SpanTitle">审核状态提醒</div>
                        </div>
                        <div style="height: 320px; overflow: hidden; border-top: 3px solid #6995CA;">
                            <iframe id="iframe2" src="AirAuditStateRemind.aspx" style="width: 100%; height: 320px;"
                                marginheight="0" marginwidth="0" frameborder="0" scrolling="no" runat="server"></iframe>
                        </div>
                    </div>
                </td>
                <td style="width: 50%;"></td>
            </tr>
            <tr>
                <td style="width: 50%;">
                    <div class="fieldsetTitle">
                        <div class="divTitle">
                            <div class="SpanTitle">
                                实时空气质量状况
                                <span style="float: right; font-size: 12px; margin-top: 6px; margin-bottom: 0;">
                                    <a href="#" onclick="return onclickRealTime()">更多..</a>
                                </span>
                            </div>
                        </div>
                        <div style="height: 360px; overflow: hidden; border-top: 3px solid #6995CA;">
                            <iframe id="iframe3" src="RealTimeAirQualityState.aspx" style="width: 100%; height: 360px;"
                                marginheight="0" marginwidth="0" frameborder="0" scrolling="no" runat="server"></iframe>
                        </div>
                    </div>
                </td>
                <td style="width: 50%;">
                    <div class="fieldsetTitle">
                        <div class="divTitle">
                            <div class="SpanTitle">
                                空气质量等级分布
                                <span style="float: right; font-size: 12px; margin-top: 6px; margin-bottom: 0;">
                                    <%--<a href="#" onclick="return onclickQ()">更多..</a>--%>
                                </span>
                            </div>
                        </div>
                        <div style="height: 360px; overflow: hidden; border-top: 3px solid #6995CA;">
                            <iframe id="iframe4" src="AirQualityLevel.aspx" style="width: 100%; height: 360px;"
                                marginheight="0" marginwidth="0" frameborder="0" scrolling="no" runat="server"></iframe>
                        </div>
                    </div>
                </td>
            </tr>
            <%--            <tr>
                <td style="width: 50%;">
                    <div class="fieldsetTitle">
                        <div class="divTitle">
                            <div class="SpanTitle">
                                最新24小时AQI
                            </div>
                        </div>
                        <div style="height: 360px; overflow: hidden; border-top: 3px solid #6995CA;">
                            <iframe id="iframe5" src="AQIof24Hours.aspx" style="width: 100%; height: 360px;"
                                marginheight="0" marginwidth="0" frameborder="0" scrolling="no" runat="server"></iframe>
                        </div>
                    </div>
                </td>
                <td style="width: 50%;">
                    <div class="fieldsetTitle">
                        <div class="divTitle">
                            <div class="SpanTitle">
                                最新30天AQI
                            </div>
                        </div>
                        <div style="height: 360px; overflow: hidden; border-top: 3px solid #6995CA;">
                            <iframe id="iframe6" src="AQIof7Days.aspx" style="width: 100%; height: 360px;"
                                marginheight="0" marginwidth="0" frameborder="0" scrolling="no" runat="server"></iframe>
                        </div>
                    </div>
                </td>
            </tr>--%>
            <tr>
                <td colspan="2" style="width: 100%;">
                    <div class="fieldsetTitle">
                        <div class="divTitle">
                            <div class="SpanTitle">
                                24小时单参数变化趋势分析<span style="float: right; font-size: 12px; margin-top: 6px; margin-bottom: 0;">
                                    <a href="#" onclick="return onclickS()">更多..</a>
                                </span>
                            </div>
                        </div>
                        <div style="height: 360px; overflow: hidden; border-top: 3px solid #6995CA;">
                            <iframe id="iframe1" src="24HSinglePollutantDataAnalyzeNew.aspx" style="width: 100%; height: 360px;"
                                marginheight="0" marginwidth="0" frameborder="0" scrolling="no" runat="server"></iframe>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="width: 50%;">
                    <div class="fieldsetTitle">
                        <div class="divTitle">
                            <div class="SpanTitle">
                                报警情况
                                <span style="float: right; font-size: 12px; margin-top: 6px; margin-bottom: 0;">
                                    <a href="#" onclick="return onclickAlertInfo()">更多..</a>
                                </span>
                            </div>
                        </div>
                        <div style="height: 360px; overflow: hidden; border-top: 3px solid #6995CA;">
                            <iframe id="iframe5" src="AlarmInfoStatistic.aspx" style="width: 100%; height: 360px;"
                                marginheight="0" marginwidth="0" frameborder="0" scrolling="no" runat="server"></iframe>
                        </div>
                    </div>
                </td>
            </tr>
        </table>

        <telerik:RadNotification runat="server" ID="AlarmNotification" EnableRoundedCorners="true" ShowTitleMenu="false"
            EnableShadow="true" VisibleOnPageLoad="false" OffsetX="-20" AutoCloseDelay="0" Animation="Slide"
            Title="最新报警信息" Width="600px" Height="350px" TitleIcon="">
            <ContentTemplate>
                <telerik:RadGrid ID="gridAlarm" runat="server" CssClass="RadGrid_Customer" GridLines="None" Height="310px"
                    AutoGenerateColumns="false" ToolTip="鼠标在表格内单击选定记录后，可点编辑和删除来修改！"
                    AllowPaging="true" OnNeedDataSource="gridAlarm_NeedDataSource"
                    ShowHeader="true" ShowStatusBar="true">
                    <MasterTableView DataKeyNames="AlarmUid" ClientDataKeyNames="AlarmUid" GridLines="None" PageSize="10" CommandItemDisplay="None" EditMode="InPlace"
                        NoMasterRecordsText="没有数据" NoDetailRecordsText="没有数据" ShowHeadersWhenNoRecords="true">
                        <PagerStyle FirstPageToolTip="首页" AlwaysVisible="true" Position="Bottom" NextPageToolTip="下页" LastPageToolTip="尾页" PrevPageToolTip="上页"
                            PageSizeLabelText="显示记录数:" PagerTextFormat="跳转页面:{4}&amp;nbsp;显示 &lt;strong&gt;{2}&lt;/strong&gt; - &lt;strong&gt;{3}&lt;/strong&gt;条,共 &lt;strong&gt;{5}&lt;/strong&gt;条"></PagerStyle>
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="序号" UniqueName="Row" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate><%#Container.DataSetIndex + 1%></ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="MonitoringPointName" HeaderText="测点名称" UniqueName="MonitoringPointName" HeaderStyle-Width="80px" />
                            <telerik:GridBoundColumn DataField="AlarmEventName" HeaderText="报警类型" UniqueName="alarmTypeName" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn DataField="RecordDateTime" HeaderText="报警时间" UniqueName="tstamp" HeaderStyle-Width="80px" DataFormatString="{0:MM-dd HH:mm:ss}" ItemStyle-HorizontalAlign="Center" />
                            <telerik:GridBoundColumn DataField="Content" HeaderText="报警内容" UniqueName="alarmCon" HeaderStyle-Width="200px">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn HeaderText="处理" UniqueName="TemplateEditColumn" HeaderStyle-Width="40px"
                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <input id="Button1" type="image" value="button" src="../../../Resources/Images/telerik/common/action_edit.gif"
                                        onclick="return ShowEditForm('<%#DataBinder.Eval(Container.DataItem,"AlarmUid")%>    ')">
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                    <ClientSettings>
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" />
                        <Scrolling AllowScroll="true" EnableVirtualScrollPaging="false" SaveScrollPosition="true" UseStaticHeaders="true"></Scrolling>
                    </ClientSettings>
                </telerik:RadGrid>
            </ContentTemplate>
        </telerik:RadNotification>
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" VisibleStatusbar="false"
            Width="600px" Height="350px" Behaviors="Close" EnableShadow="true" Title="报警处理">
            <Windows>
                <telerik:RadWindow ID="AlarmHandle" runat="server" Width="600px" Height="350px" ViewStateMode="Enabled" Style="z-index: 30000001"
                    ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Localization-Cancel="取消"
                    Localization-Close="关闭" Localization-Maximize="最大化" Localization-Minimize="最小化"
                    Localization-Reload="刷新" Localization-PinOff="浮动" Localization-PinOn="固定" />
            </Windows>
        </telerik:RadWindowManager>
    </form>
</body>
</html>

