<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QualityControlShow.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.QualityControl.QualityControlShow" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="../../../Resources/JavaScript/JQuery/jquery-1.10.2.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var divTaskInfoTitle = document.getElementById("divTaskInfoTitle");
            var divTaskInfo = document.getElementById("divTaskInfo");
            var windowHeight = document.body.clientHeight;//网页可见区域高
            divTaskInfo.style.height = (windowHeight - divTaskInfoTitle.offsetHeight - 5).toString() + "px";
        })
    </script>
</head>
<body>
    <script type="text/javascript">
        //保存业务表单数据 
        function SaveFormData(formID, taskCode, activityFlag, wfOpeType, formOpeType, pointGuid) {
            //formID表示即配置的业务表单的guid，已经具体到你们的页面，是否还有必要传？ 
            //taskID表示当前任务的标识，guid型；taskCode表示任务编号 
            //activityFlag表示步骤标识，传值为1、2、3，对应第一(任务填报)、二(任务初审)、三(任务复审)步 
            //wfOpeType表示工作流按钮操作标识，传值为1、2、3、4，对应保存、提交、退回、终止 
            //formOpeType表示表单展现方式，传值为1、2，对应编辑、明细 
            //pointGuid表示测点Guid

            //业务系统需要有返回值，返回1、0，表示成功、失败 
            //如果失败的话，需要记录日志到任务日志表中，平台提供接口，业务系统调用，这项不急，演示后在具体定 

            return 1;
        }
    </script>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1"
            UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="pointCbxRsm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="pointCbxRsm" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadSplitter ID="splitter" runat="server" Orientation="Horizontal" Height="100%" Width="100%"
            BorderWidth="0" BorderStyle="None" BorderSize="0">
            <telerik:RadPane ID="paneTable" runat="server" Scrolling="None" Width="100%" Height="40px"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <div id="divTaskInfoTitle" style="height: 30px;">
                    <table id="Tb" style="width: 100%; height: 100%; display: none" cellspacing="1" cellpadding="0" class="Table_Customer"
                        border="0">
                        <tr>
                            <td class="title" style="width: 80px">任务编号:
                            </td>
                            <td class="content" style="width: 200px;">
                                <asp:Label ID="lblTaskCode" runat="server"></asp:Label>
                            </td>
                            <td class="title" style="width: 80px">任务名称:
                            </td>
                            <td class="content" style="width: 200px;">
                                <asp:Label ID="lblTaskName" runat="server"></asp:Label>
                            </td>
                            <td class="content">
                                <asp:ImageButton ID="btnExcel" runat="server" OnClick="btnExcel_Click" SkinID="ImgBtnExcel" />
                            </td>
                        </tr>
                    </table>
                </div>
            </telerik:RadPane>
            <telerik:RadPane ID="paneGrid" runat="server" Width="100%" Height="100%" Scrolling="None"
                BorderWidth="0" BorderStyle="None" BorderSize="0">
                <div id="divTaskInfo" style="overflow: hidden; border-top: 0px solid #6995CA;">
                    <iframe id="iframeTaskInfo" src="" style="width: 100%; height: 100%;"
                        marginheight="0" marginwidth="0" frameborder="0" scrolling="auto" runat="server"></iframe>
                </div>
            </telerik:RadPane>
        </telerik:RadSplitter>
    </form>
</body>
</html>
