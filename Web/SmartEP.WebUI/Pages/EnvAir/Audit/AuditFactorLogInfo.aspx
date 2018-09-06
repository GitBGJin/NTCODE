<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AuditFactorLogInfo.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Audit.AuditFactorLogInfo" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body scroll="no">
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadGrid ID="gridAuditLog" runat="server" GridLines="None" Height="90%" Width="100%"
            AllowPaging="false" PageSize="24" AllowSorting="false"
            AutoGenerateColumns="false" AllowMultiRowSelection="false"
            EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false"
            ShowStatusBar="false" BorderWidth="0" BorderStyle="None" BorderSize="0"
            CssClass="RadGrid_Customer" OnNeedDataSource="gridAuditLog_NeedDataSource" OnItemDataBound="gridAuditLog_ItemDataBound">
            <MasterTableView GridLines="None" TableLayout="Fixed" CommandItemDisplay="None" IsFilterItemExpanded="False"
                InsertItemPageIndexAction="ShowItemOnCurrentPage" NoMasterRecordsText="没有数据">
                <Columns>
                    <telerik:GridBoundColumn HeaderText="数据时间" UniqueName="tstamp" DataField="tstamp" DataFormatString="{0:dd日HH点}"/>
                    <telerik:GridBoundColumn HeaderText="因子" UniqueName="PollutantName" DataField="PollutantName" />
                    <telerik:GridBoundColumn HeaderText="原始值" UniqueName="SourcePollutantDataValue" DataField="SourcePollutantDataValue" />
                    <telerik:GridBoundColumn HeaderText="修改值" UniqueName="AuditPollutantDataValue" DataField="AuditPollutantDataValue" />
                    <telerik:GridBoundColumn HeaderText="审核人" UniqueName="UpdateUser" DataField="UpdateUser" />
                    <telerik:GridBoundColumn HeaderText="审核日期" UniqueName="UpdateDateTime" DataField="UpdateDateTime" />
                </Columns>
            </MasterTableView>
            <ClientSettings>
                <Scrolling AllowScroll="True" ></Scrolling>
            </ClientSettings>
        </telerik:RadGrid>
    </form>
</body>
</html>
