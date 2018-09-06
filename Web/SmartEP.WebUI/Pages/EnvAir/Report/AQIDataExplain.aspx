<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AQIDataExplain.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.DataAnalyze.AQIDataExplain" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="AQIData" runat="server">
    <div>
       <b>城市日AQI的评价项目包括：SO<sub>2</sub>、NO<sub>2</sub>、CO、PM<sub>10</sub>、PM<sub>2.5</sub>的24小时平均和O<sub>3</sub>日最大8小时滑动平均等6个指标。<br />当且仅当时间范围为24h，并且范围是0点到23点的时候，臭氧8h按照日数据计算，其他时间段臭氧8h取24笔数据计算，并且取其中的最大值。</b>
    </div>
    </form>
</body>
</html>
