<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrequencySetting.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.DataAnalyze.FrequencySetting" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <table id="tbContent" style="width: 100%; height: 100%; text-align: center" class="Table_Customer" border="0">
            <tr style="text-align: left">
                <td style="width: 80px">
                    <asp:ImageButton ID="btnSave" OnClick="btnSave_Click" runat="server" CssClass="RadToolBar_Customer" SkinID="ImgBtnSave" />
                </td>
            </tr>
            <tr style="text-align: right">
                <td>单位：<asp:Label runat="server" ID="unit"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView runat="server" ID="gvAtt" AutoGenerateColumns="False" DataKeyNames="Num" Width="100%" Height="100%" OnDataBound="gvAtt_DataBound"
                        OnRowEditing="gvAtt_RowEditing" OnRowUpdating="gvAtt_RowUpdating">
                        <Columns>
                            <asp:TemplateField HeaderText="序号" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="20%" ItemStyle-Width="20%" HeaderStyle-Height="28px" ItemStyle-Height="28px">
                                <ItemTemplate>
                                    <asp:TextBox ID="Num" runat="server" Width="100%" Height="100%" Text='<%# Eval("Num")%>' CssClass="TextBox" ReadOnly="true"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="因子" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="20%" ItemStyle-Width="20%" HeaderStyle-Height="28px" ItemStyle-Height="28px">
                                <ItemTemplate>
                                    <asp:TextBox ID="PollutantName" runat="server" Width="100%" Height="100%" Text='<%# Eval("PollutantName")%>' CssClass="TextBox" ReadOnly="true"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="下限值" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="30%" ItemStyle-Width="30%" HeaderStyle-Height="28px" ItemStyle-Height="28px">
                                <ItemTemplate>
                                    <asp:TextBox ID="Lower" runat="server" Width="100%" Height="100%" onkeypress="if (event.keyCode < 48 || event.keyCode >57) event.returnValue = false;"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="上限值" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="30%" ItemStyle-Width="30%" HeaderStyle-Height="28px" ItemStyle-Height="28px">
                                <ItemTemplate>
                                    <asp:TextBox ID="Upper" runat="server" Width="100%" Height="100%" onkeypress="if (event.keyCode < 48 || event.keyCode >57) event.returnValue = false;" CssClass="TextBox"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%-- <asp:CommandField HeaderText="编辑" ShowEditButton="True"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                 HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-Height="28px" ItemStyle-Height="28px" />--%>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr style="text-align: right">
                <td>
                    <asp:Button ID="AddRow" Text="新增一条范围" runat="server" OnClick="AddRow_Click" /></td>
            </tr>
        </table>
    </form>
</body>
</html>
