<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyImage.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.DataAnalyze.MyImage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">      
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <%--<telerik:RadSkinManager ID="RadSkinManager1" runat="server" ShowChooser="false" />--%>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" Width="100%"
            BorderWidth="0" BorderStyle="None" BorderSize="0">                        
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <div class="demo-container">
                    <div class="right-image-gallery">
                        <telerik:RadImageGallery RenderMode="Lightweight" DisplayAreaMode="Image" ID="RadImageGallery2" runat="server" Width="100%" Height="100%" LoopItems="true" ImagesFolderPath="">
                            <ThumbnailsAreaSettings Mode="Thumbnails" />
                            <ToolbarSettings ShowSlideshowButton="false" />
                            <ImageAreaSettings Width="100%" ShowNextPrevImageButtons="true" NextImageButtonText="下一张" PrevImageButtonText="上一张" />
                        </telerik:RadImageGallery> 
                    </div>
                </div>
            </telerik:RadPane>
        </telerik:RadSplitter>
    </form>
</body>
</html>
