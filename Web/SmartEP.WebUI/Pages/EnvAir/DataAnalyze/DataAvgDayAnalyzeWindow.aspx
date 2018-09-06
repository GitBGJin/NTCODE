<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DataAvgDayAnalyzeWindow.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.DataAnalyze.DataAvgDayAnalyzeWindow" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style>
        table.Table_Customer {
    border: 1px solid #FFFFFF;
   
    font-size: 12px;
}

    </style>
</head>
<body>
    <form id="form1" runat="server">
    <table  style="width: 100%; height: 100%; text-align: center"  border="1"  >
        <tr>
            <td rowspan="6" align="center">
                年评价
            </td>
            <td>
                SO<sub>2</sub>年平均、SO<sub>2</sub>24小时平均第 98 百分位数
            </td>
        </tr>
        <tr>
            <td>
                NO<sub>2</sub>年平均、NO<sub>2</sub>24小时平均第 98 百分位数
            </td>
        </tr>
        <tr>
            <td>
                PM<sub>10</sub>年平均、PM<sub>10</sub>24小时平均第 95 百分位数
            </td>
        </tr>
        <tr>
            <td>
               
                PM<sub>2.5</sub>年平均、PM<sub>2.5</sub>24小时平均第 95 百分位数
            </td>
        </tr>
        <tr>
            <td>
               
                CO24小时平均第 95 百分位数
            </td>
        </tr>
        <tr>
            <td>
                
                O<sub>3</sub>日最大8小时滑动平均值的第 90 百分位数
            </td>
        </tr>
    </table>
    </form>
</body>
</html>

