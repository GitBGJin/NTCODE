<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AirPointEdit.aspx.cs" MasterPageFile="~/WebMaster/MasterPage.Master" Inherits="SmartEP.WebUI.Pages.EnvAir.BaseData.AirPointEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="maintable" cellspacing="1" width="100%" cellpadding="0" class="Table_Customer" border="0">
        <tr class="btnTitle">
            <td class="btnTitle" colspan="4">
                <asp:ImageButton ID="btnEdit" SkinID="ImgBtnSave" runat="server" OnClick="btnEdit_Click" />
            </td>
        </tr>
        <tr>
          <%--  <td class="title">监测站</td>
            <td class="content">
                <telerik:RadComboBox ID="stationList"
                    runat="server" Width="150px">
                </telerik:RadComboBox>
            </td>--%>
            <td class="title">监测点名称</td>
            <td class="content">
                <telerik:RadTextBox ID="txtPointName" runat="server" Width="150px" MaxLength="50"></telerik:RadTextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*测点名称不能为空" ControlToValidate="txtPointName"></asp:RequiredFieldValidator>
            </td>
              <td class="title">建造时间</td>
             <td class="content"> <telerik:RadDatePicker Runat="server" ID="txtBuildTime"  >
              </telerik:RadDatePicker></td>
        </tr>
        <tr>
            <td class="title">地址</td>
            <td class="content" colspan="3">
                <telerik:RadTextBox ID="txtAddress" runat="server" Width="80%" MaxLength="50"></telerik:RadTextBox></td>
        </tr>
        <tr>
            <td class="title">地区</td>
            <td class="content">
                <telerik:RadComboBox ID="regionList"
                    runat="server" Width="150px">
                </telerik:RadComboBox>
            </td>
            <td class="title">监测区域</td>
        <td class="content"> <telerik:RadComboBox ID="monitoringRegionList" 
                runat="server"    Width="150px"></telerik:RadComboBox></td>
        </tr>
        <tr>
            <td class="title">城市类型</td>
            <td class="content">
                <telerik:RadComboBox ID="cityList" AutoPostBack="true"
                    runat="server" Width="150px" OnSelectedIndexChanged="cityList_SelectedIndexChanged">
                </telerik:RadComboBox>
            </td>
            <td class="title">城市区域类型</td>
        <td class="content"> <telerik:RadComboBox ID="cityRegionList" 
                runat="server"    Width="150px"></telerik:RadComboBox></td>
        </tr>
        <tr>
            <td class="title">X(经度)</td>
            <td class="content">
                <telerik:RadTextBox ID="txtX" runat="server" Width="150px"></telerik:RadTextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtX" ErrorMessage="数字型" ValidationExpression="^[-]?(\d+\.?\d*|\.\d+)$" />
            </td>
            <td class="title">Y(纬度)</td>
            <td class="content">
                <telerik:RadTextBox ID="txtY" runat="server" Width="150px"></telerik:RadTextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtY" ErrorMessage="数字型" ValidationExpression="^[-]?(\d+\.?\d*|\.\d+)$" />
            </td>
        </tr>
           <tr>
        <td class="title">集成商</td>
        <td class="content"><telerik:RadComboBox ID="integratorList" 
                runat="server"    Width="150px"></telerik:RadComboBox>  </td>
         <td class="title">站点类型</td>
        <td class="content"><telerik:RadComboBox ID="siteTypeList" 
                runat="server"    Width="150px"></telerik:RadComboBox></td>
        </tr>
         <tr>
            
        <td class="title">运行状态</td>
        <td class="content"><telerik:RadComboBox ID="runStatusList" runat="server"    Width="150px"></telerik:RadComboBox></td>
           <td class="title">控制类型</td>
        <td class="content"><telerik:RadComboBox ID="contrlCodeList" runat="server"    Width="150px"></telerik:RadComboBox></td>
        </tr>
        
         <tr>
        
           <td class="title">配置选项</td>
        <td class="content" colspan="3" >是否参与全市AQI统计<asp:CheckBox ID="cbxIsCalAQI" runat="server" />
            是否参与区域AQI统计<asp:CheckBox ID="cbxIsCalRegionAQI" runat="server" />
            
            是否背景点<asp:CheckBox ID="cbxIsRefer" runat="server" />
            是否创模点<asp:CheckBox ID="cbxIsModel" runat="server" />
            是否超级站<asp:CheckBox ID="cbxIsSuper" runat="server" />
            是否使用<asp:CheckBox ID="cbxIsUse" runat="server" />
            是否显示<asp:CheckBox ID="cbxIsShow" runat="server" />

        </td>
        
        </tr>
        <%-- <tr>
        <td class="title">站房照片</td>
        <td class="content" colspan="3">
         <div class="upload-panel">
        <telerik:RadProgressManager runat="server" ID="RadProgressManager1" />                                                      
        <telerik:RadAsyncUpload runat="server" ID="AsyncUpload1"  MultipleFileSelection="Automatic" MaxFileInputsCount="5"  AllowedFileExtensions="jpg,jpeg,png,gif" Width="50%" Localization-Select="浏览">
        </telerik:RadAsyncUpload>
        <telerik:RadProgressArea runat="server" ID="RadProgressArea1">
        </telerik:RadProgressArea>
        
        <asp:Repeater runat="server" ID="Repeater1" OnItemCommand= "Repeater1_ItemCommand"> 
        <HeaderTemplate>
        <table width="100%">  
        </table> 
        </HeaderTemplate> 
            <ItemTemplate>
             <tr>
              <td><telerik:RadButton ID="btnDeleteIMG" runat="server" Text="删除图片" CommandName="DeleteIMG"  OnClientClicked="OnClientClicked" OnClick= "btnDeleteIMG_Click ">
                        <Icon PrimaryIconCssClass="rbRemove" PrimaryIconLeft="4" PrimaryIconTop="4" />
                    </telerik:RadButton></td><td><asp:TextBox ID="TextBox1" runat="server" Text='<%#Eval("AttachGuid") %>' Visible="false" ></asp:TextBox> </td></tr>         
            <tr ><td colspan="2">
                <telerik:RadBinaryImage runat="server" ID="radBinaryImage1" AutoAdjustImageControlSize="false"  DataValue='<%#Eval("content") %>'  Width="500px" Height="300px"/> 
                <telerik:RadToolTip runat="server" ID="RadToolTip1" TargetControlID="radBinaryImage1" 
                    Position="Center">  
                </telerik:RadToolTip> 
 
                </td></tr> 
            </ItemTemplate>
            <FooterTemplate>  </FooterTemplate> 
        </asp:Repeater> 
         </div>
        </td>       
        </tr>--%>
        <tr>
        <td class="title">排序号</td>
        <td class="content" colspan="3" ><telerik:RadTextBox   ID="tbxOrderNumber" runat="server" MaxLength="9"></telerik:RadTextBox><asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="tbxOrderNumber" ErrorMessage="非负整数" ValidationExpression="^\d+$" />
         <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="*不能为空" ControlToValidate="tbxOrderNumber"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td class="title">备注</td>
            <td class="content" colspan="3">
                <telerik:RadTextBox ID="txtMemo" runat="server" MaxLength="50"
                    Height="84px" Width="600px">
                </telerik:RadTextBox></td>
        </tr>

    </table>
</asp:Content>
