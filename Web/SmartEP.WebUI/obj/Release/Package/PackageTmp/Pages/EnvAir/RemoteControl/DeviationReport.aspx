<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeviationReport.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.RemoteControl.DeviationReport" %>


<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=9.0.15.324, Culture=neutral, PublicKeyToken=a9d7983dfcc261be" Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" id="html">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=9" />
    <title>Telerik Report Viewer</title>
    <style type="text/css">			
		html#html, body#body, form#form1, div#content, center#center
		{	
			border: 0px solid black;
			padding: 0px;
			margin: 0px;
			height: 100%;
		}
    </style>
</head>
<body id="body">
    <form id="form1" runat="server">              
    <div id="content"><center id="center">    
        <telerik:ReportViewer ID="ReportViewer1" runat="server"  style="border:1px solid #ccc;" 
			width="99%" height="99%">
<Resources PrintToolTip="打印"></Resources>
</telerik:ReportViewer>
        </center></div>
    </form>
</body>
</html>