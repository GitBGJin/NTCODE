<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExcelShow.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.QualityControl.ExcelShow" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" Width="100%"
            BorderWidth="0" BorderStyle="None" BorderSize="0">
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <asp:ImageButton ID="btnExcel" runat="server" OnClick="btnExcel_Click" SkinID="ImgBtnExcel" />
                <div id="divTaskInfo" style="overflow: hidden; border-top: 0px solid #6995CA; height: 100%">
                    <iframe id="iframeTaskInfo" src="" style="width: 100%; height: 100%;"
                        marginheight="0" marginwidth="0" frameborder="0" scrolling="auto" runat="server"></iframe>
                </div>
            </telerik:RadPane>
        </telerik:RadSplitter>
    </form>
</body>
</html>
