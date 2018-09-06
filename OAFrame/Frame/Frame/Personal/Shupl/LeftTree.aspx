<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LeftTree.aspx.cs" Inherits="Com.Sinoyd.Frame.WebUI.Personal.Shupl.LeftTree" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta content="text/html; charset=utf-8" http-equiv="Content-Type">
    <title></title>
    <link rel="stylesheet" type="text/css" href="LeftTree.css?a=12">
    <script src="jquery.min.js"></script>
    <script>
        $(function () {
            //菜单隐藏展开
            var tabs_i = -1;
            $('.vtitle').click(function () {
                var _self = $(this);
                var j = $('.vtitle').index(_self);

                if (tabs_i == j) return false; tabs_i = j;
                $('.vtitle em').each(function (e) {
                    if (e == tabs_i) {
                        $('em', _self).removeClass('v01').addClass('v02');
                        setMenu1CheckIndex(tabs_i);
                        $("#first_" + e).css("background-color", "#FDAA32");
                    } else {
                        $(this).removeClass('v02').addClass('v01');
                        $("#first_" + e).css("background-color", "#319DE4");
                    }
                });
                $('.vcon').slideUp().eq(tabs_i).slideDown();
            });

            // $("#txt_Height").val("500px");
        })

        function setMenu1CheckIndex(menu1CheckIndex) {
            $.ajax({
                type: "post",
                url: "../../Handle/SetMenu1CheckIndex.ashx?Menu1CheckIndex=" + menu1CheckIndex + "&id=" + Math.random(),
                dataType: "json",
                success: function (data) {
                    if (data[0].Status == "1") {
                        //alert('登录成功！');
                    }
                    else {
                        //alert(data[0].Message);
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //alert(errorThrown);
                }
            });
        }

        function setModuleCheckId(moduleCheckId) {
            $.ajax({
                type: "post",
                url: "../../Handle/SetModuleCheckId.ashx?ModuleCheckId=" + moduleCheckId + "&id=" + Math.random(),
                dataType: "json",
                success: function (data) {
                    if (data[0].Status == "1") {
                        //alert('登录成功！');
                    }
                    else {
                        //alert(data[0].Message);
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //alert(errorThrown);
                }
            });
        }

        function check(id, otherList) {
            //alert(1);
            $("#" + id).addClass('select');
            var moduleGuid = $("#" + id).attr("name");
            var url = $("#" + id).attr("url");
            var title = $("#" + id).attr("title1");
            //alert(url);
            parent.showwindows(moduleGuid, url, title);
            setModuleCheckId(moduleGuid);
            //alert(moduleGuid);
            var arryList = otherList.split(';');
            for (var i = 0; i < arryList.length; i++) {
                if (arryList[i] != "") {
                    $("#" + arryList[i]).removeClass('select');
                }
            }
        }

        function selectMenu1Init(menu1CheckIndex) {
            alert(menu1CheckIndex);
            //if (e == tabs_i) {
            //    $('em', _self).removeClass('v01').addClass('v02');
            //    setMenu1CheckIndex(tabs_i);
            //    // var menu1CheckID = _self.id;//$("#" + id).attr("url");
            //    //alert(tabs_i);
            //    //session.setAttribute("Menu1CheckIndex", tabs_i);
            //} else {
            //    $(this).removeClass('v02').addClass('v01');
            //    //alert(_self.id);
            //}
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="width: 230px;">
            <asp:Literal runat="server" ID="ltl_LeftTree"></asp:Literal>
        </div>
    </form>
</body>
</html>
