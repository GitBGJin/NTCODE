<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AirMonthReportEdit.aspx.cs" Inherits="SmartEP.WebUI.Pages.EnvAir.Report.AirMonthReportEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>空气月报在线编辑</title>
    <style type="text/css">
        .btn {
            BORDER-RIGHT: #0066cc 1px solid;
            BORDER-TOP: #3366ff 1px solid;
            FONT-SIZE: 12px;
            PADDING-BOTTOM: 3px;
            MARGIN: 1px 0px 2px 3px;
            BORDER-LEFT: #0066cc 1px solid;
            WIDTH: 128px;
            COLOR: #ffffff;
            PADDING-TOP: 3px;
            BORDER-BOTTOM: #3366ff 1px solid;
            FONT-STYLE: normal;
            HEIGHT: 24px;
            BACKGROUND-COLOR: #638eef;
        }
    </style>
</head>
<body onload="WebOffice1_NotifyCtrlReady();" onunload="window_onunload();">
    <script language="javascript" type="text/javascript">
        //控制窗口大小自适应
        self.moveTo(0, 0)
        self.resizeTo(document.body.clientWidth, document.body.clientHeight)
        /****************************************************
        *
        *		控件初始化WebOffice方法
        *
        ****************************************************/
        function WebOffice1_NotifyCtrlReady() {
            document.all.WebOffice1.OptionFlag |= 128;
            //设置weboffice自带工具栏显示或隐藏
            document.all.WebOffice1.ShowToolBar = false;
            //文件本地绝对路径
            var filepath = "<%=FileExists()%>";
            //带格式的文件名
            var filename = filepath.substring(filepath.lastIndexOf("\\") + 1, filepath.length);
            //文件格式
            var filetype = filename.substring(filename.lastIndexOf(".") + 1, filename.length);
            //不带格式的文件名
            var filetitle = filename.substring(0, filename.lastIndexOf("."));
            //完整URL
            var curWwwPath = window.document.location.href;
            //获取主机地址之后的目录
            var pathName = window.location.pathname;
            //获取主机地址，如： http://localhost:8083
            var localhostPaht = curWwwPath.substring(0, curWwwPath.indexOf(pathName));
            //特殊处理主机地址之后的目录，去除多余的/
            for (var i = 0; i < pathName.length; i++) {
                if (pathName.substr(1).indexOf('/') > 0) {
                    break;
                }
                pathName = pathName.substr(1);
            }
            //获取带"/"的项目名
            var projectName = pathName.substring(0, pathName.substr(1).indexOf('/') + 1);
            //判断是本地访问还是服务器访问
            if (localhostPaht.indexOf("localhost") == -1) {
                //定义文件保存在根目录下的地址
                var projectpath = "Files/TempFile/Word/AirMonthReport/DownLoad/" + filename;
                //获取文件服务器绝对路径
                filepath = localhostPaht + projectName + "/" + projectpath;
            }
            //打开文件
            document.all.WebOffice1.LoadOriginalFile(filepath, filetype);
        }

        /****************************************************
        *
        *					保存文档
        *
        /****************************************************/
        function newSave() {
            try {
                var webObj = document.getElementById("WebOffice1");
                //文件本地绝对路径
                var filepath = "<%=FileExists()%>";
                //完整URL
                var curWwwPath = window.location.href;
                //获取主机地址之后的目录
                var pathName = window.location.pathname;
                //获取主机地址，如： http://localhost:8083
                var localhostPaht = curWwwPath.substring(0, curWwwPath.indexOf(pathName));
                //特殊处理主机地址之后的目录，去除多余的/
                for (var i = 0; i < pathName.length; i++) {
                    if (pathName.substr(1).indexOf('/') > 0) {
                        break;
                    }
                    pathName = pathName.substr(1);
                }
                //获取带"/"的项目名
                var projectName = pathName.substring(0, pathName.substr(1).indexOf('/') + 1);
                //判断是本地访问还是服务器访问
                if (localhostPaht.indexOf("localhost") == -1) {
                    //带格式的文件名
                    var filename = filepath.substring(filepath.lastIndexOf("\\") + 1, filepath.length);
                    //文件格式
                    var filetype = filename.substring(filename.lastIndexOf(".") + 1, filename.length);
                    //不带格式的文件名
                    var filetitle = filename.substring(0, filename.lastIndexOf("."));
                    //定义文件保存在根目录下的地址
                    var projectpath = "Files/TempFile/Word/AirMonthReport/DownLoad/" + filename;
                    //var projectpath = "Files/枯水期环境监测快报/" + filename;
                    //获取文件服务器绝对路径
                    filepath = localhostPaht + projectName + "/" + projectpath;
                    //定义文件上传绝对路径（通用）
                    var savepath = localhostPaht + projectName + "/weboffice/upload.aspx";
                    //初始化Http引擎
                    webObj.HttpInit();
                    // 添加相应的Post元素 
                    webObj.HttpAddPostString("id", "<%=Guid.NewGuid().ToString()%>");
                    webObj.HttpAddPostString("DocPath", escape(projectpath));
                    webObj.HttpAddPostString("DocName", escape(filename));
                    // 上传文件
                    webObj.HttpAddPostCurrFile("DocContent", "");
                    // 判断上传是否成功
                    returnValue = webObj.HttpPost(savepath);
                    if (returnValue == "success") {
                        alert("文件保存成功");
                    }
                    else {
                        alert("文件保存失败");
                    }
                }
                else {
                    //本地文件
                    webObj.Save();
                    alert("文件保存成功");
                }
            } catch (e) {
                alert("异常\r\nError:" + e + "\r\nError Code:" + e.number + "\r\nError Des:" + e.description);
            }
        }

        /****************************************************
        *
        *					另存为文档
        *
        /****************************************************/
        function SaveAsTo() {
            try {
                var webObj = document.getElementById("WebOffice1");
                webObj.ShowDialog(84);
            } catch (e) {
                alert("异常\r\nError:" + e + "\r\nError Code:" + e.number + "\r\nError Des:" + e.description);
            }
        }

        /****************************************************
        *
        *					全屏
        *
        /****************************************************/
        function bToolBar_FullScreen_onclick() {
            try {
                var webObj = document.getElementById("WebOffice1");
                webObj.FullScreen = true;
            } catch (e) {
                alert("异常\r\nError:" + e + "\r\nError Code:" + e.number + "\r\nError Des:" + e.description);
            }
        }

        /****************************************************
        *
        *		关闭页面时调用此函数，关闭文件 
        *
        ****************************************************/
        function window_onunload() {
            try {
                var webObj = document.getElementById("WebOffice1");
                webObj.Close();
            } catch (e) {
                alert("异常\r\nError:" + e + "\r\nError Code:" + e.number + "\r\nError Des:" + e.description);
            }
        }

        /****************************************************
        *
        *					关闭事件
        *
        /****************************************************/
        function CloseWord() {
            document.all.WebOffice1.CloseDoc(0);
        }
    </script>
    <!-- --------------------=== 调用Weboffice初始化方法 ===--------------------- -->
    <script language="javascript" event="NotifyCtrlReady" for="WebOffice1" type="text/javascript">
        /****************************************************
        *
        *	在装载完Weboffice(执行<object>...</object>)
        *	控件后执行 "WebOffice1_NotifyCtrlReady"方法
        *
        ****************************************************/
        //WebOffice1_NotifyCtrlReady()
    </script>
    <form id="form1" runat="server" name="myform" action="#" method="post">
        <table style="width: 100%; height: 100%" id="TableMain">
            <tr style="height: 30px">
                <td style="text-align: left;">
                    <input class="btn" id="SaveFile" onclick="newSave()" type="button" value="保存" name="SaveFile" />
                    <input class="btn" id="SaveFileAsTo" onclick="SaveAsTo()" type="button" value="另存为" name="SaveFileAsTo" />
                    <input class="btn" id="FullScreen" onclick="return bToolBar_FullScreen_onclick()" type="button" value="全  屏" name="FullScreen" />
                </td>
            </tr>
            <tr>
                <td valign="top" style="height: 100%;" id="tdObject">
                    <!-- -----------------------------== 装载weboffice控件 ==--------------------------------- -->
                    <script src="../../../weboffice/LoadWebOffice.js" type="text/javascript"></script>
                    <!-- --------------------------------== 结束装载控件 ==----------------------------------- -->
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
