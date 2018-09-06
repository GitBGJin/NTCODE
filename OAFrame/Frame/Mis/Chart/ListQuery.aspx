<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListQuery.aspx.cs" Inherits="Com.Sinoyd.Mis.WebUI.Chart.ListQuery"
    Debug="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <%--<style>
        .x-panel-body-noborder
        {
            background-color: Red;
        }
    </style>--%>
</head>
<body>
    <form id="form1" runat="server">
    <script type="text/javascript">
    </script>
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server"></f:PageManager>
    <f:Panel ID="Panel1" runat="server" BodyPadding="3px" ShowBorder="false" ShowHeader="false" Height="300px" Layout="Anchor" CssStyle="background-color:red;">
        <Items>
            <f:ContentPanel ID="ContentPanel1" runat="server" BodyPadding="5px" ShowBorder="false" ShowHeader="false" Title="ContentPanel">
                <table>
                    <tr>
                        <td valign="baseline">
                            <table id="table1" runat="server">
                            </table>
                        </td>
                        <td>
                            <f:Button ID="btnImport" Text="统计" runat="server" Icon="ChartCurveGo" OnClick="btnSearch_Click">
                            </f:Button>
                        </td>
                    </tr>
                </table>
            </f:ContentPanel>
            <f:TabStrip ID="TabStrip1" ShowBorder="true" ActiveTabIndex="0" runat="server" EnableTitleBackgroundColor="False" Height="460">
                <Tabs>
                    <f:Tab ID="Tab1" BodyPadding="5px" Title="折线图" runat="server" EnableIFrame="true" IFrameUrl="DChart.aspx">
                    </f:Tab>
                    <f:Tab ID="Tab2" BodyPadding="5px" Title="数据列表" runat="server" EnableIFrame="true" IFrameUrl="DChart.aspx">
                    </f:Tab>
                </Tabs>
            </f:TabStrip>
        </Items>
    </f:Panel>
    </form>
</body>
</html>
