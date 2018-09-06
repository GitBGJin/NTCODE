<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LadarBMP.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.SuperStation.LadarBMP" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        .container {
            width: 100%;
            text-align: center;
            background-color: #f3f3f3;
            height: 100%;
        }

        /*#RadImageGallery2 .rigDescriptionBox {
            background-color: rgba(215, 215, 215, 0.7);
            color: #333;
        }*/

        .div_ImageGallery {
            max-width: 916px;
            margin-left: auto;
            margin-right: auto;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <div class="container">
            <telerik:RadDatePicker runat="server" ID="rdpstartdate" DateInput-Label="开始日期" MinDate="1900-01-01" DateInput-DateFormat="yyyy-MM-dd"></telerik:RadDatePicker>
            <telerik:RadDatePicker runat="server" ID="rdpenddate" DateInput-Label="结束日期" MinDate="1900-01-01" DateInput-DateFormat="yyyy-MM-dd"></telerik:RadDatePicker>
            <telerik:RadComboBox runat="server" ID="cmbtype" Label="类型">
                <Items>
                    <telerik:RadComboBoxItem Value="消光" Text="消光" Selected="true" />
                    <telerik:RadComboBoxItem Value="退偏" Text="退偏" />
                </Items>
            </telerik:RadComboBox>
            <asp:Button runat="server" ID="Button1" OnClick="Button1_Click" Text="查询" />
            <div class="div_ImageGallery">
                <telerik:RadImageGallery ID="RadImageGallery2" runat="server" OnNeedDataSource="RadImageGallery2_NeedDataSource" DataDescriptionField="Description" DataImageField="ImageUrl"
                    DataTitleField="Title" Width="916px" Height="640px" LoopItems="true">
                    <ThumbnailsAreaSettings ThumbnailWidth="140px" Width="140px" Position="Left" ScrollOrientation="Vertical" />
                </telerik:RadImageGallery>
            </div>
        </div>
    </form>
</body>
</html>
