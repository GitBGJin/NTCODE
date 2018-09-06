<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AppointmentToolTip.ascx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.RemoteControl.AppointmentToolTip" %>
<div style="margin: 5px 5px 0px 5px;">
    <div style="border-bottom: solid 1px #ccc; margin-bottom: 9px; font-size: 11px;">
         <div>
         <asp:Label runat="server" ID="pointName"></asp:Label>
        </div>
         <div>
            校准时间:
            <asp:Label runat="server" ID="EndOn"></asp:Label>
        </div>
       <%-- <div>
            开始时间:
            <asp:Label runat="server" ID="StartingOn"></asp:Label>
        </div>--%>
     <%--   <div>
            结束时间:
            <asp:Label runat="server" ID="EndOn"></asp:Label>
        </div>--%>
    </div>
    <asp:TextBox runat="server" ID="FullText" TextMode="MultiLine" Width="100%" Rows="7" Style="border: 0; font-size: 12px; background: transparent;"></asp:TextBox>
</div>