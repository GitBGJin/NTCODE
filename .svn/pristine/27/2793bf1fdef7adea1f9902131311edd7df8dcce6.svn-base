<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FactorRsmAuditHg.ascx.cs" Inherits="SmartEP.WebUI.Controls.FactorRsmAuditHg" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <script src="<%=ResolveClientUrl("../Resources/JavaScript/JQuery/jquery-1.9.0.min.js")%>" type="text/javascript"></script>
    <script src="<%=ResolveClientUrl("../Resources/JavaScript/telerikControls/RadSiteMap.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        //选择父节点
        function SelectParentNode(chk, SingleOrMulit) {
            checkModeValue = SingleOrMulit;
            //判断节点check方式（是否选中）
            var selType = (chk.checked ? selectType.selectAll : selectType.unselect);

            CurrentChks = document.getElementById("<%=RSM.ClientID%>").getElementsByTagName('input');
            CurrentChksText = document.getElementById("<%=RSM.ClientID%>").getElementsByTagName('label');

            //执行选择CheckBox，并记录选择项值
            executeCheck(nodeType.checkParentNode, selType, chk.id.replace(radComboxSiteMapParentNodeIdSuffix, ''), checkModeValue);
        }

        //选择子节点
        function SelectChildNode(chk, SingleOrMulit) {
            checkModeValue = SingleOrMulit;
            //判断节点check方式（是否选中）
            var selType = (chk.checked ? selectType.selectAll : selectType.unselect);
            //当前父节点ID
            var radComboxSiteMapParentNodeId = chk.id.substring(0, chk.id.replace(radComboxSiteMapChildNodeIdSuffix, '').lastIndexOf('_'));
            //判断选择模式（单选还是多选）

            CurrentChks = document.getElementById("<%=RSM.ClientID%>").getElementsByTagName('input');
            CurrentChksText = document.getElementById("<%=RSM.ClientID%>").getElementsByTagName('label');

            if (checkModeValue == checkMode.singleCheck)
                executeCheck(nodeType.checkChildNode, selType, chk.id, checkModeValue);
            else
                executeCheck(nodeType.checkChildNode, selType, radComboxSiteMapParentNodeId, checkModeValue);
        }

        function PartentRefresh() {
            try {
                //alert(1);
                document.getElementById('refreshData').value = 1;
                document.getElementById('refreshData').click();
                //parent.document.getElementById('refreshData').value = 1;
                //parent.document.getElementById('refreshData').click();
            } catch (e) {
            }
        }

    </script>
</telerik:RadCodeBlock>
<%--<telerik:RadAjaxManagerProxy ID="RadAjaxManagerProxy1" runat="server">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="RSM">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="pagediv" LoadingPanelID="RadAjaxLoadingPanel1" />
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManagerProxy>--%>
<telerik:RadSiteMap runat="server" ID="RSM" Width="97%">
    <LevelSettings>
        <telerik:SiteMapLevelSetting Level="0" Layout="List" ListLayout-AlignRows="true">
            <NodeTemplate>
                <br />
                <asp:CheckBox runat="server" ID="RsmChkA" Checked="true" Font-Bold="true" onclick="SelectParentNode(this,'multi');" OnCheckedChanged="RsmChkB_CheckedChanged" AutoPostBack="true"
                    Text='<%# DataBinder.Eval(Container.DataItem, "PName")%>'
                    ToolTip='<%# DataBinder.Eval(Container.DataItem, "PID")%>' />
            </NodeTemplate>
        </telerik:SiteMapLevelSetting>
        <telerik:SiteMapLevelSetting Level="1" Layout="Flow">
            <NodeTemplate>
                <div class="ContentHolder">
                    <asp:CheckBox runat="server" ID="RsmChkB" Checked="true" onclick="SelectChildNode(this,'multi');" OnCheckedChanged="RsmChkB_CheckedChanged" AutoPostBack="true"
                        Text='<%# DataBinder.Eval(Container.DataItem, "PName")%>'
                        ToolTip='<%# DataBinder.Eval(Container.DataItem, "PID")%>' />
                </div>
            </NodeTemplate>
        </telerik:SiteMapLevelSetting>
    </LevelSettings>
</telerik:RadSiteMap>
