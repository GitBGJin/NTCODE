<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DChartPie.aspx.cs" Inherits="Com.Sinoyd.Mis.WebUI.Chart.DChartPie" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.2/jquery.min.js"></script>
    <script type="text/javascript" src="http://code.highcharts.com/highcharts.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
    </div>
    <div id="container" style="min-width: 310px; height: 400px; margin: 0 auto">
    </div>
    </form>
</body>
</html>
