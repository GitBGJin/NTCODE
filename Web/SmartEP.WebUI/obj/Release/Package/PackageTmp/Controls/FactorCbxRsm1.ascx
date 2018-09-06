<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FactorCbxRsm1.ascx.cs" Inherits="SmartEP.WebUI.Controls.FactorCbxRsm1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<script src="<%=ResolveClientUrl("../Resources/JavaScript/JQuery/jquery-1.9.0.min.js")%>" type="text/javascript"></script>
<script src="<%=ResolveClientUrl("../Resources/JavaScript/telerikControls/RadComboxIncludeSiteMap.js")%>" type="text/javascript"></script>
<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <script type="text/javascript">
        //遍历选中CheckBox信息
        function OnFactorClientDropDownClosed_Change(sender, args) {
            radComboxSiteMap = radCombox.get_items().getItem(0).findControl(radComboxSiteMapID);
            CurrentChks = radComboxSiteMap.get_element().getElementsByTagName('input');
            CurrentChksText = radComboxSiteMap.get_element().getElementsByTagName('label');
            var checkName = '', RCBName = '', currValue = '';
            for (var i = 0; i < CurrentChks.length - 1; i++) {
                //不包含父节点信息，只遍历子节点信息
                if (CurrentChks[i].checked == true && CurrentChks[i].id.indexOf(radComboxSiteMapParentNodeIdSuffix) < 0) {
                    currValue = CurrentChksText[i].innerText;
                    if (typeof (currValue) == 'undefined') {
                        currValue = CurrentChksText[i].innerHTML;
                    }
                    checkName += checkName == '' ? currValue : ';' + currValue;
                }
            }
            //选中节点信息赋予ComBox
            sender.set_text(checkName);
            sender._openDropDownOnLoad = false;
            //收缩下拉选框
            sender.hideDropDown();
            RCBName = sender._uniqueId.substring(sender._uniqueId.lastIndexOf('$') + 1);
            var factors = document.getElementById("<%=factorNames.ClientID%>");
            if (factors.value == checkName) {
                return;
            }
            factors.value = checkName;
            document.getElementById("<%=btnFactorChange.ClientID%>").click();
        }

        //下拉选项不自动收回
        function StopPropagation(e) {
            e.cancelBubble = true;
            if (e.stopPropagation) {
                e.stopPropagation();
            }
        }
    </script>
</telerik:RadCodeBlock>

<div style="display: none">
    <asp:HiddenField ID="factorNames" runat="server" Value="" />
    <telerik:RadButton ID="btnFactorChange" runat="server" AutoPostBack="true" OnClick="btnFactorChange_Click"></telerik:RadButton>
</div>
<telerik:RadComboBox ID="RadCBoxFactor" runat="server" AutoPostBack="false" SkinID="Default" Skin="Default"
    OnClientDropDownOpening="OnClientDropDownOpening"
    OnClientDropDownOpened="OnClientDropDownOpened"
>
    <HeaderTemplate>
        <table>
            <tr>
                <td>
                    <telerik:RadButton runat="server" AutoPostBack="false" Visible="true" ID="selectAll" CommandName="selectAll" Text="全选" ToolTip="全选" OnClientClicked="OnRsmBtn" />
                    <telerik:RadButton runat="server" AutoPostBack="false" Visible="true" ID="inverse" CommandName="inverse" Text="反选" ToolTip="反选" OnClientClicked="OnRsmBtn" />
                    <telerik:RadButton runat="server" AutoPostBack="false" Visible="true" ID="unselect" CommandName="unselect" Text="不选" ToolTip="不选" OnClientClicked="OnRsmBtn" />
                </td>
            </tr>
        </table>
    </HeaderTemplate>
    <ItemTemplate>
        <div onclick="StopPropagation(event)">
        <telerik:RadSiteMap runat="server" ID="RSM" Width="100%" OnNodeDataBound="RSM_NodeDataBound">
            <LevelSettings>
                <telerik:SiteMapLevelSetting Level="0" Layout="List" ListLayout-AlignRows="true">
                    <NodeTemplate>
                        <br />
                        <asp:CheckBox runat="server" ID="RsmChkA" Font-Bold="true" onclick="onSelectParentNode(this,'multi')"
                            Text='<%# DataBinder.Eval(Container.DataItem, "RsmName")%>'
                            ToolTip='<%# DataBinder.Eval(Container.DataItem, "RsmValue")%>' />
                    </NodeTemplate>
                </telerik:SiteMapLevelSetting>
                <telerik:SiteMapLevelSetting Level="1" Layout="Flow">
                    <NodeTemplate>
                        <div class="ContentHolder">
                            <asp:CheckBox runat="server" ID="RsmChkB" onclick="onSelectChildNode(this, 'multi')"
                                Text='<%# DataBinder.Eval(Container.DataItem, "RsmName")%>'
                                ToolTip='<%# DataBinder.Eval(Container.DataItem, "RsmValue")%>' />
                        </div>
                    </NodeTemplate>
                </telerik:SiteMapLevelSetting>
            </LevelSettings>
        </telerik:RadSiteMap>
        </div>
    </ItemTemplate>
    <Items>
        <telerik:RadComboBoxItem Text="" />
    </Items>
</telerik:RadComboBox>

