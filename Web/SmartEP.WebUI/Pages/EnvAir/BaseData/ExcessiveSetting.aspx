<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExcessiveSetting.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.DataAnalyze.ExcessiveSetting" %>

<!DOCTYPE html>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CbxRsm" TagName="PointCbxRsm" Src="~/Controls/PointCbxRsm.ascx" %>
<%@ Register TagPrefix="CbxRsm" TagName="FactorCbxRsm" Src="~/Controls/FactorCbxRsm.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>超标值限配置</title>

    <style type="text/css">
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:radscriptmanager id="RadScriptManager1" runat="server" />
        <input id="Application" type="hidden" runat="server" />
        <telerik:radajaxmanager id="RadAjaxManager1" runat="server"
            updatepanelsrendermode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="RadGrid1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnAdd">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadWindowManager1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="pointCbxRsmA">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="pointCbxRsmA" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="pointCbxRsmW">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="pointCbxRsmW" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="factorCbxRsmA">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="factorCbxRsmA" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="factorCbxRsmW">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="factorCbxRsmW" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:radajaxmanager>
        <telerik:radscriptblock id="RadScriptBlock1" runat="server">
        <script type="text/javascript" language="javascript">

            //全选
            function SelectAll(obj) {
                var theBox = obj;
                xState = theBox.checked;
                elem = theBox.form.elements;
                for (i = 0; i < elem.length; i++)
                    if (elem[i].type == "checkbox" && elem[i].name != theBox.name && elem[i].name.split('$')[0] == theBox.name.split('$')[0]) {
                        if (elem[i].checked != xState)
                            elem[i].click();
                    }
            }
            //页面刷新
            function RefreshOnClick() {
                var Win1 = $find('ExcessiveSetEditWin');
                Win1.set_modal(false);
                Win1.hide();

                document.getElementById("btnSearch").click();
            }
            //编辑页面
            function openExcessiveSetEdit(ExcessiveUid) {
                radopen("ExcessiveSetEdit.aspx?ExcessiveUid=" + ExcessiveUid, "ExcessiveSetEditWin");
            }
            //按钮行处理
            function gridRTB_ClientButtonClicking(sender, args) {
                var masterTable = $find("<%= RadGrid1.ClientID %>").get_masterTableView();
                var CurrentBtn = args.get_item();
                var CurrentBtnName = CurrentBtn.get_text();
                var CurrentBtnCommandName = CurrentBtn.get_commandName();
                switch (CurrentBtnCommandName) {
                    case "InitInsert":
                        {
                            //增加
                            var RuleType = document.getElementById("<%=hdType.ClientID%>").value;
                            var type = document.getElementById("Application").value
                            var oWnd = window.radopen("ExcessiveSetEdit.aspx?ApplicationUid=" + type + "&RuleType=" + RuleType, "ExcessiveSetEditWin");
                            //oWnd.maximize();//全屏
                            args.set_cancel(true);
                            break;
                        }
                    case "DeleteSelected":
                        try {
                            //删除
                            var selItems = masterTable.get_selectedItems();
                            if (selItems.length <= 0) {
                                alert("请选择要删除的记录！");
                                return false;
                            }
                            else {
                                args.set_cancel(!confirm('确定删除所有选中的记录？'));
                            }
                        } catch (e) { }
                        break;
                    case "RebindGrid":
                        masterTable.rebind();
                        break;
                    default:
                        break;
                }
            }

        </script>
    </telerik:radscriptblock>
        <asp:HiddenField ID="hdType" runat="server" />
        <telerik:radajaxloadingpanel id="RadAjaxLoadingPanel1" runat="server" />
        <telerik:radsplitter id="MainSplitter" runat="server" orientation="Horizontal" height="100%" width="100%"
            bordersize="0" borderstyle="None" borderwidth="0">
            <telerik:RadPane ID="ToolBarRadPane" runat="server" Height="35px" Width="100%" Scrolling="None"
                CssClass="DivToolBar" BorderStyle="None" BorderWidth="0">
                <div>
                    <table>
                        <tr>
                            <td>站点:</td>
                            <td>
                                 <div id="divAir" runat="server" visible="false">
                                <CbxRsm:PointCbxRsm runat="server" ApplicationType="Air" CbxWidth="200" CbxHeight="350" MultiSelected="true" DropDownWidth="520" ID="pointCbxRsmA"></CbxRsm:PointCbxRsm>
                            </div>
                                  <div id="divWater" runat="server" visible="false">
                                <CbxRsm:PointCbxRsm runat="server" ApplicationType="Water" CbxWidth="200" CbxHeight="350" MultiSelected="true" DropDownWidth="520" ID="pointCbxRsmW"></CbxRsm:PointCbxRsm>
                            </div>
                             </td>
                            <td>监测因子:
                    </td>
                    <td>
                        <div id="FactorAir" runat="server" visible="false">
                            <CbxRsm:FactorCbxRsm runat="server" ApplicationType="Air" DropDownWidth="420" ID="factorCbxRsmA" CbxWidth="200"></CbxRsm:FactorCbxRsm>
                        </div>
                        <div id="FactorWater" runat="server" visible="false">
                            <CbxRsm:FactorCbxRsm runat="server" ApplicationType="Water" DropDownWidth="420" ID="factorCbxRsmW" CbxWidth="200"></CbxRsm:FactorCbxRsm>
                        </div>
                    </td>
                            <td>数据类型:</td>
                            <td>
                                <telerik:RadComboBox ID="cmbDataType" runat="server" Width="100px" AutoPostBack="false">
                                </telerik:RadComboBox>
                            </td>
                            <td>通知级别:</td>
                            <td>
                                <telerik:RadComboBox ID="cmbNotifyGrade" runat="server" Width="100px" AutoPostBack="false">
                                </telerik:RadComboBox>
                            </td>
                            <%--<td>规则用途类型:</td>--%>
                            <td style="display:none">
                                <telerik:RadComboBox ID="cmbUseFor" runat="server" Width="100px" AutoPostBack="false">
                                </telerik:RadComboBox>
                            </td>

                            <td class="title" colspan="10">
                                <asp:ImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" SkinID="ImgBtnSearch" />

                            </td>
                        </tr>
                    </table>
                </div>

            </telerik:RadPane>
            <telerik:RadPane ID="RadGridRadPane" runat="server" Height="100%" Width="100%" Scrolling="None"
                BorderStyle="None" BorderWidth="0">
                <telerik:RadGrid ID="RadGrid1" runat="server" GridLines="None" AllowPaging="True" CssClass="RadGrid_Customer"
                    Height="100%" PageSize="30" AllowCustomPaging="false" AllowSorting="True" AllowMultiRowSelection="true"
                    AutoGenerateColumns="False" EnableHeaderContextMenu="True" EnableHeaderContextFilterMenu="True"  OnItemCommand="grid_ItemCommand"
                    ShowStatusBar="true" OnNeedDataSource="RadGrid1_NeedDataSource" OnItemDataBound="RadGrid1_ItemDataBound">
                    <ExportSettings HideStructureColumns="True" IgnorePaging="True" OpenInNewWindow="True" ExportOnlyData="true" />
                    <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="Top" IsFilterItemExpanded="False"
                        ClientDataKeyNames="ExcessiveUid" InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                        <CommandItemTemplate>
                        <telerik:RadToolBar ID="gridRTB" runat="server" AutoPostBack="true" CssClass="RadToolBar_Customer" SkinID="CD"
                            Width="100%" OnClientButtonClicking="gridRTB_ClientButtonClicking" />
                    </CommandItemTemplate>
                        <Columns>
                            <telerik:GridClientSelectColumn HeaderText="选择" UniqueName="ClientSelectColumn" Exportable="false">
                            <HeaderStyle Width="40px"></HeaderStyle>
                        </telerik:GridClientSelectColumn>
                            <telerik:GridTemplateColumn HeaderText="序号" UniqueName="Row"
                                HeaderStyle-Width="40px" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%#Container.DataSetIndex + 1%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="操作" UniqueName="ScrappedInstrumentUid" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderButtonType="None" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                            <ItemTemplate>
                                  编辑<img id="btnEdit" style="cursor: pointer;" alt="编辑" title="点击编辑" src="../../../Resources/Images/icons/page_edit.png"
                                    onclick="openExcessiveSetEdit('<%# DataBinder.Eval(Container, "DataItem.ExcessiveUid")%>')" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="MonitoringPointName" HeaderText="站点" UniqueName="MonitoringPointName"
                                HeaderStyle-Width="160px" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="160px" 
                                ItemStyle-HorizontalAlign="Left" HeaderButtonType="None">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="PollutantName" HeaderText="监测因子" UniqueName="PollutantName"
                                HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="80px" 
                                ItemStyle-HorizontalAlign="Center" HeaderButtonType="None">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="DataType" HeaderText="数据类型" UniqueName="DataType"
                                HeaderStyle-Width="160px" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="160px"
                                ItemStyle-HorizontalAlign="Left" HeaderButtonType="None">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="NotifyGrade" HeaderText="通知级别" UniqueName="NotifyGrade"
                                HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="80px"
                                ItemStyle-HorizontalAlign="Center" HeaderButtonType="None">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="UseFor" HeaderText="规则用途类型" UniqueName="UseFor"
                                HeaderStyle-Width="160px" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="160px"
                                ItemStyle-HorizontalAlign="Left" HeaderButtonType="None">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ExcessiveUpper" HeaderText="超标上限" UniqueName="ExcessiveUpper"
                                HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="80px"
                                ItemStyle-HorizontalAlign="Center" HeaderButtonType="None">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ExcessiveLow" HeaderText="超标下限" UniqueName="ExcessiveLow"
                                HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="80px"
                                ItemStyle-HorizontalAlign="Center" HeaderButtonType="None">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="EnableOrNot" HeaderText="是否使用" UniqueName="EnableOrNot"
                                HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="80px"
                                ItemStyle-HorizontalAlign="Center" HeaderButtonType="None">
                            </telerik:GridBoundColumn>
                        </Columns>
                        <PagerStyle Visible="true" FirstPageToolTip="首页" NextPageToolTip="下页" LastPageToolTip="尾页"
                            PrevPageToolTip="上页" PageSizeLabelText="显示记录数:" PagerTextFormat="跳转页面:{4}&amp;nbsp;[第 &lt;strong&gt;{0}&lt;/strong&gt; 页共 &lt;strong&gt;{1}&lt;/strong&gt;页]&amp;nbsp;&amp;nbsp;[第 &lt;strong&gt;{2}&lt;/strong&gt; 到 &lt;strong&gt;{3}&lt;/strong&gt;,共 &lt;strong&gt;{5}&lt;/strong&gt;条信息]"></PagerStyle>
                    </MasterTableView>
                    <CommandItemStyle Width="100%" />
                    <ClientSettings Resizing-AllowColumnResize="true">
                        <Scrolling AllowScroll="true" EnableVirtualScrollPaging="false" UseStaticHeaders="true"
                            SaveScrollPosition="false"></Scrolling>
                        <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" />
                    </ClientSettings>
                </telerik:RadGrid>
            </telerik:RadPane>
        </telerik:radsplitter>
        <telerik:radwindowmanager id="RadWindowManager1" runat="server" enableshadow="true"
            visiblestatusbar="false">
            <Windows>
                <telerik:RadWindow ID="ExcessiveSetEditWin" runat="server" Height="600px" Width="800px"
                    Title="超标值限配置" Top="100px" Left="300px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" />
            </Windows>
            <Localization Maximize="最大化" Minimize="最小化" Restore="还原" Reload="刷新" PinOff="移动"
                PinOn="固定" />
        </telerik:radwindowmanager>
    </form>
</body>
</html>
