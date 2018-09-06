<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/WebMaster/MasterPage.Master" CodeBehind="UserPersonalizedConfig.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.BaseData.UserPersonalizedConfig" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID='ContentPlaceHolder1' runat="server">
    <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
        <script src="<%=ResolveClientUrl("~/Resources/JavaScript/JQuery/jquery-1.9.0.min.js")%>" type="text/javascript"></script>
        <script type="text/javascript" language="javascript">
            function onRequestStart(sender, args) {
                if (args.get_eventTarget().indexOf("ExportToExcelButton") >= 0 ||
                        args.get_eventTarget().indexOf("ExportToWordButton") >= 0 ||
                        args.get_eventTarget().indexOf("ExportToCsvButton") >= 0) {
                    args.set_enableAjax(false);
                }
            }

            var selectType = {
                selectAll: 'selectAll',  //全选
                inverse: 'inverse',    //反选
                unselect: 'unselect'    //不选
            }

            var nodeType = {
                checkAllNode: 'all',            //选择父节点
                checkParentNode: 'parent',     //选择父节点
                checkChildNode: 'child'        //选择子节点
            }

            var checkMode = {
                singleCheck: 'single',     //单选
                multiCheck: 'multi'        //多选
            }

            var CurrentChks;                            //当前RadSiteMap中的复选框集
            var CurrentChksText;                        //当前RadSiteMap中的复选框名称集
            var radComboxSiteMapParentNodeIdSuffix = '_RsmChkA';     //radSiteMap父节点ID后缀
            var radComboxSiteMapChildNodeIdSuffix = '_RsmChkB';      //radSiteMap子节点ID后缀
            var checkModeValue = checkMode.multiCheck;  //默认多选模式

            function setTemplateValue(templateControl, itemText) {
                var arrItemText = itemText.replace(/;$/, '').split(';');
                for (var i = 0; i < arrItemText.length; i++) {
                    for (var j = 0; j < CurrentChks.length - 1; j++) {
                        if (arrItemText[i] == CurrentChksText[j].innerText && CurrentChks[j].checked == false) {
                            CurrentChks[j].checked = true;
                            //判断childnode是否全部选中，如全选，则parentnode选中
                            onSelectChildNode(CurrentChks[j], checkModeValue);
                            break;
                        }
                    }
                }
            }

            //选择父节点
            function onSelectParentNode(chk, SingleOrMulit, Type) {
                if (Type == 'Point') {
                    radComboxSiteMap = $find("<%= RSMPoint.ClientID%>");
                }
                else {
                    radComboxSiteMap = $find("<%= RSMFactor.ClientID%>");
                }
                CurrentChks = radComboxSiteMap.get_element().getElementsByTagName('input');
                CurrentChksText = radComboxSiteMap.get_element().getElementsByTagName('label');

                checkModeValue = SingleOrMulit;
                //判断节点check方式（是否选中）
                var selType = (chk.checked ? selectType.selectAll : selectType.unselect);
                //执行选择CheckBox，并记录选择项值
                executeCheck(nodeType.checkParentNode, selType, chk.id.replace(radComboxSiteMapParentNodeIdSuffix, ''), checkModeValue);
            }

            //选择子节点
            function onSelectChildNode(chk, SingleOrMulit, Type) {
                if (Type == 'Point') {
                    radComboxSiteMap = $find("<%= RSMPoint.ClientID%>");
                }
                else {
                    radComboxSiteMap = $find("<%= RSMFactor.ClientID%>");
                }
                CurrentChks = radComboxSiteMap.get_element().getElementsByTagName('input');
                CurrentChksText = radComboxSiteMap.get_element().getElementsByTagName('label');

                checkModeValue = SingleOrMulit;
                //判断节点check方式（是否选中）
                var selType = (chk.checked ? selectType.selectAll : selectType.unselect);
                //当前父节点ID
                var radComboxSiteMapParentNodeId = chk.id.substring(0, chk.id.replace(radComboxSiteMapChildNodeIdSuffix, '').lastIndexOf('_'));
                //判断选择模式（单选还是多选）
                if (checkModeValue == checkMode.singleCheck)
                    executeCheck(nodeType.checkChildNode, selType, chk.id, checkModeValue);
                else
                    executeCheck(nodeType.checkChildNode, selType, radComboxSiteMapParentNodeId, checkModeValue);
            }

            function executeCheck(NodeType, SelType, CheckNodeID, SingleOrMulit) {
                checkModeValue = SingleOrMulit;
                var NodeID = '', AllSelChkName = '', ChkedItemsCount = 0, NotChkedItemsCount = 0;
                for (var i = 0; i < CurrentChks.length - 1; i++) {
                    NodeID = CurrentChks[i].id;
                    if (checkModeValue == checkMode.singleCheck)
                        CurrentChks[i].checked = (NodeID == CheckNodeID ? true : false);
                    else {
                        if (NodeType == nodeType.checkAllNode || NodeType == nodeType.checkParentNode) {
                            if (CheckNodeID == '' || (NodeID.indexOf(CheckNodeID) >= 0 && NodeID.replace(CheckNodeID, '').substring(0, 1) == '_')) {
                                switch (SelType) {
                                    case selectType.selectAll:
                                        CurrentChks[i].checked = true;
                                        break;
                                    case selectType.inverse:
                                        CurrentChks[i].checked = !(CurrentChks[i].checked)
                                        break;
                                    case selectType.unselect:
                                        CurrentChks[i].checked = false;
                                        break;
                                }
                            }
                        }
                        else if (NodeID.indexOf(CheckNodeID) >= 0 && NodeID.replace(CheckNodeID, '').substring(0, 1) == '_' && NodeID.indexOf(radComboxSiteMapParentNodeIdSuffix) < 0) {
                            if (CurrentChks[i].checked == true)
                                ChkedItemsCount++;
                            else
                                NotChkedItemsCount++;

                            if (NotChkedItemsCount <= 0)
                                ($get(CheckNodeID + radComboxSiteMapParentNodeIdSuffix)).checked = true;
                            else 
                                ($get(CheckNodeID + radComboxSiteMapParentNodeIdSuffix)).checked = false;

                            if (ChkedItemsCount <= 0)
                                ($get(CheckNodeID + radComboxSiteMapParentNodeIdSuffix)).checked = false;
                        }
                    }
                    if (CurrentChks[i].checked == true && NodeID.indexOf(radComboxSiteMapParentNodeIdSuffix) < 0)
                        AllSelChkName += AllSelChkName == '' ? CurrentChksText[i].innerText : ';' + CurrentChksText[i].innerText;
                }
            }

            function OnClientClicked(button, args) {
                if (button._value == 'Point') {
                    radComboxSiteMap = $find("<%= RSMPoint.ClientID%>");
                }
                else {
                    radComboxSiteMap = $find("<%= RSMFactor.ClientID%>");
                }
                CurrentChks = radComboxSiteMap.get_element().getElementsByTagName('input');
                CurrentChksText = radComboxSiteMap.get_element().getElementsByTagName('label');

                var BtnTxt = button._text;
                var BtnCmdName = button._commandName;
                executeCheck('all', BtnCmdName, '', 'multi');
            }
        </script>
        
    </telerik:RadScriptBlock>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    </telerik:RadCodeBlock>
    <telerik:RadAjaxLoadingPanel runat="server" ID="LoadingPanel1"></telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadBtnPoint">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadMultiPage1" LoadingPanelID="LoadingPanel1"/>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadBtnPointSave">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RSMPoint" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadBtnFactorSave">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RSMFactor" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="RadTabStrip1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadTabStrip1" />
                        <telerik:AjaxUpdatedControl ControlID="RadMultiPage1" LoadingPanelID="LoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="RSMPoint" />
                        <telerik:AjaxUpdatedControl ControlID="RSMFactor" />
                    </UpdatedControls>
                </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="RadMultiPage1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadMultiPage1" LoadingPanelID="LoadingPanel1"/>
                    <telerik:AjaxUpdatedControl ControlID="RSMPoint" />
                    <telerik:AjaxUpdatedControl ControlID="RSMFactor" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
        <ClientEvents OnRequestStart="onRequestStart" />
    </telerik:RadAjaxManager>
    <telerik:RadTabStrip runat="server" ID="RadTabStrip1" SelectedIndex="0" MultiPageID="RadMultiPage1" CssClass="RadTabStrip_Customer" ontabclick="RadTabStrip1_TabClick">
        <Tabs>
            <telerik:RadTab Text="关注测点" runat="server" Selected="True" />
            <telerik:RadTab Text="关注因子" runat="server" />
        </Tabs>
    </telerik:RadTabStrip>
    <telerik:RadMultiPage ID="RadMultiPage1" runat="server" SelectedIndex="0" Width="100%"
        ScrollBars="Hidden">
        <telerik:RadPageView ID="RPV_Point" runat="server">
            <table class="Table_Customer" border="0" width="100%">
                <tr>
                    <td>
                     <table class="Table_Customer" border="0" width="100%">
                     <tr>
                     <td style="width:50%;">
                         <div style="display:none">
                            <telerik:RadButton runat="server" UseSubmitBehavior="false" ID="RadBtnPoint"
                                CommandName="selectAll" Text="类型/区域" ToolTip="区域" OnClick="RadBtnPointChange_Click" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                         </div>
                        <telerik:RadButton runat="server" UseSubmitBehavior="false" AutoPostBack="false"
                            ID="selectAllP" CommandName="selectAll" Text="全选" ToolTip="全选" Value="Point" OnClientClicked="OnClientClicked" />
                        <telerik:RadButton runat="server" UseSubmitBehavior="false" AutoPostBack="false"
                            ID="inverseP" CommandName="inverse" Text="反选" ToolTip="反选" Value="Point" OnClientClicked="OnClientClicked" />
                        <telerik:RadButton runat="server" UseSubmitBehavior="false" AutoPostBack="false"
                            ID="unselectP" CommandName="unselect" Text="不选" ToolTip="不选" Value="Point" OnClientClicked="OnClientClicked" />
                    </td>
                    <td >
                     <telerik:RadButton ID="RadBtnPointSave" runat="server" UseSubmitBehavior="false" Text="保存测点" OnClick="RadBtnSave_Click" >
                            <Icon PrimaryIconCssClass="rbSave" PrimaryIconLeft="4" PrimaryIconTop="4"/>
                        </telerik:RadButton>
                     </td>
                     </tr>
                     </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadSiteMap runat="server" ID="RSMPoint">
                            <LevelSettings>
                                <telerik:SiteMapLevelSetting Level="0" Layout="List" ListLayout-AlignRows="true">
                                    <NodeTemplate>
                                        <br />
                                        <asp:CheckBox runat="server" ID="RsmChkA" Font-Bold="true" Enabled="true" onclick="onSelectParentNode(this,'multi','Point')"
                                            Text='<%# DataBinder.Eval(Container.DataItem, "PName")%>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "CGuid")%>' />
                                    </NodeTemplate>
                                </telerik:SiteMapLevelSetting>
                                <telerik:SiteMapLevelSetting Level="1" Layout="Flow">
                                    <NodeTemplate>
                                        <div class="ContentHolder">
                                            <asp:CheckBox runat="server" ID="RsmChkB" onclick="onSelectChildNode(this, 'multi','Point')"
                                                Text='<%# DataBinder.Eval(Container.DataItem, "PName")%>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "CGuid")%>' />
                                        </div>
                                    </NodeTemplate>
                                </telerik:SiteMapLevelSetting>
                            </LevelSettings>
                        </telerik:RadSiteMap>
                    </td>
                </tr>
            </table>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RPV_Channel" runat="server">
            <table class="Table_Customer" border="0" width="100%">
                <tr>
                    <td>
                    <table class="Table_Customer" border="0" width="100%">
                     <tr>
                       <td style="width:50%;">
                            <telerik:RadButton runat="server" UseSubmitBehavior="false" AutoPostBack="false"
                                ID="selectAllF" CommandName="selectAll" Text="全选" ToolTip="全选" Value="Factor" OnClientClicked="OnClientClicked" />
                            <telerik:RadButton runat="server" UseSubmitBehavior="false" AutoPostBack="false"
                                ID="inverseF" CommandName="inverse" Text="反选" ToolTip="反选" Value="Factor" OnClientClicked="OnClientClicked" />
                            <telerik:RadButton runat="server" UseSubmitBehavior="false" AutoPostBack="false"
                                ID="unselectF" CommandName="unselect" Text="不选" ToolTip="不选" Value="Factor" OnClientClicked="OnClientClicked" />
                        </td>
                        <td>
                            <telerik:RadButton ID="RadBtnFactorSave" runat="server" UseSubmitBehavior="false" Text="保存因子" OnClick="RadBtnSave_Click">
                               <Icon PrimaryIconCssClass="rbSave" PrimaryIconLeft="4" PrimaryIconTop="4" />
                            </telerik:RadButton>
                        </td>
                     </tr>
                     </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadSiteMap runat="server" ID="RSMFactor">
                            <LevelSettings>
                                <telerik:SiteMapLevelSetting Level="0" Layout="List" ListLayout-AlignRows="true">
                                    <NodeTemplate>
                                        <br />
                                        <asp:CheckBox runat="server" ID="RsmChkA" Font-Bold="true" Enabled="true" onclick="onSelectParentNode(this,'multi','Factor')"
                                            Text='<%# DataBinder.Eval(Container.DataItem, "PName")%>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "CGuid")%>' />
                                    </NodeTemplate>
                                </telerik:SiteMapLevelSetting>
                                <telerik:SiteMapLevelSetting Level="1" Layout="Flow">
                                    <NodeTemplate>
                                        <div class="ContentHolder">
                                            <asp:CheckBox runat="server" ID="RsmChkB" onclick="onSelectChildNode(this, 'multi','Factor')"
                                                Text='<%# DataBinder.Eval(Container.DataItem, "PName")%>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "CGuid")%>' />
                                        </div>
                                    </NodeTemplate>
                                </telerik:SiteMapLevelSetting>
                            </LevelSettings>
                        </telerik:RadSiteMap>
                    </td>
                </tr>
            </table>
        </telerik:RadPageView>

    </telerik:RadMultiPage>
</asp:Content>
