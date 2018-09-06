<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetUser.aspx.cs" Inherits="TK.Mis.Web.TableInfoSet.GetUser" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>获取邮件接收者</title>
</head>
<body>
    <form id="form1" runat="server">
    <script type="text/javascript">

        //清空
        function delall()
        {
            var userid = document.getElementById('<%= userID.ClientID %>');
            var username = document.getElementById('<%= userName.ClientID %>');
            document.getElementById('<%= ListReceiverName.ClientID %>').options.length = 0;
            userid.value = "";
            username.value = "";

        }

        //删除选中
        function delchose()
        {
            var options = document.getElementById('<%= ListReceiverName.ClientID %>').options;
            for (var i = 0; i < options.length; i++)
            {
                if (options[i].selected)
                {
                    //
                    var userid = document.getElementById('<%= userID.ClientID %>');
                    var username = document.getElementById('<%= userName.ClientID %>');
                    userid.value = userid.value.replace(options[i].value + ';', '');
                    username.value = username.value.replace(options[i].text + ';', '');
                    options[i] = null;
                    i--;
                }
            }
        }
    </script>
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1"></f:PageManager>
    <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
        <Regions>
            <f:Region ID="Region2" Width="200px" Margins="3 3 3 3" ShowHeader="false" Title="部门" Icon="Outline" EnableCollapse="true" ShowBorder="false" Layout="Fit" Position="Left" runat="server">
                <Items>
                    <f:Tree ID="Tree1" EnableArrows="true" ShowBorder="false" ShowHeader="false" EnableMultiSelect="false" runat="server" Title="" AutoScroll="true">
                    </f:Tree>
                </Items>
            </f:Region>
            <f:Region ID="mainRegion" ShowHeader="false" Layout="Fit" Margins="3 3 3 3" Position="Center" ShowBorder="false" runat="server">
                <Items>
                    <f:Panel ID="Panel2" ShowBorder="false" ShowHeader="false" runat="server" AnchorValue="100% -36" Layout="Fit">
                        <Toolbars>
                            <f:Toolbar ID="Toolbar2" runat="server">
                                <Items>
                                    <f:Button ID="Button1" Text="确定选择" runat="server" Icon="Add" OnClick="btnSelect_Click">
                                    </f:Button>
                                    <f:Button ID="Button2" Text="删除选中" runat="server" Icon="Delete" EnablePostBack="false" OnClientClick="delchose()">
                                    </f:Button>
                                    <f:Button ID="btnrefresh" Text="清空所有" runat="server" Icon="Reload" EnablePostBack="false" OnClientClick="delall()">
                                    </f:Button>
                                </Items>
                            </f:Toolbar>
                        </Toolbars>
                        <Items>
                            <f:ContentPanel ID="cp1" runat="server" ShowBorder="false" ShowHeader="false">
               
                        <select id="ListReceiverName" runat="server" multiple="true" name="ListReceiverName" style="width: 100%; height: 400px;">
            </select>      <input id="userName" runat="server" type="hidden">
            <input id="userID" runat="server" type="hidden">
                            </f:ContentPanel>
                        </Items>
                    </f:Panel>
                </Items>
            </f:Region>
        </Regions>
    </f:RegionPanel>
    </form>
    <script type="text/javascript">

        function SetNodeCheck(node, treeMenu)
        {
            if (node.hasChildNodes())//如果有子节点
            {
                var childs = node.childNodes;
                for (var i = 0; i < childs.length; i++)
                {
                    if (childs[i].attributes.qtip == "用户")
                    {
                        if (userid.value.indexOf(childs[i].id) != -1)
                        {
                            var _node = treeMenu.getNodeById(childs[i].id).getUI().checkbox.checked = true
                        }
                    }
                    //                        else if (childs[i].hasChildNodes()) {
                    //                            SetNodeCheck(childs[i]);
                    //                        }
                }
            }
        }

        F.ready(function() {

            var treeMenu = F('<%= Tree1.ClientID %>');

            /*
            *	给select赋值
            */
            var ChaoSonNames = '<%=Request["userListName"] %>';
            ChaoSonNames = window.parent.document.getElementById(ChaoSonNames).value;
            //alert(ChaoSonNames);
            var ChaoSonGuids = '<%=Request["userListGuid"] %>';
            ChaoSonGuids = window.parent.document.getElementById(ChaoSonGuids).value;

            var ChaoSonGuidArray;
            var ChaoSonNameArray;
            if (ChaoSonNames == "" && ChaoSonGuids == "")
            {
                ChaoSonGuidArray = "";
                ChaoSonNameArray = "";
            }
            else
            {
                ChaoSonGuidArray = ChaoSonGuids.split(';');
                ChaoSonNameArray = ChaoSonNames.split(';');
            }

            var userid = document.getElementById('<%= userID.ClientID %>');
            var username = document.getElementById('<%= userName.ClientID %>');
            var ListReceiverName = document.getElementById('<%= ListReceiverName.ClientID %>').options;

            for (var j = 0; j < ChaoSonNameArray.length; j++)
            {
                if (ChaoSonNameArray[j] != "")
                {
                    var option = new Option(ChaoSonNameArray[j], ChaoSonGuidArray[j]);
                    ListReceiverName.add(option);
                    userid.value += ChaoSonGuidArray[j] + ';';
                    username.value += ChaoSonNameArray[j] + ';';
                }
            }

            treeMenu.expandAll();
            //初始化树中被勾选的节点
            treeMenu.on('expandnode', function (node)
            {
                if (userid.value != '')
                {
                    //alert("[" + node.text + "]open" + node.hasChildNodes());  
                    if (node.hasChildNodes())//如果有子节点
                    {
                        var childs = node.childNodes;
                        for (var i = 0; i < childs.length; i++)
                        {
                            if (childs[i].attributes.qtip == "用户")
                            {
                                if (userid.value.indexOf(childs[i].id) != -1)
                                {
                                    var _node = treeMenu.getNodeById(childs[i].id).getUI().checkbox.checked = true;
                                }
                            }
                        }
                    }
                }
            });



            //绑定checkbox点击事件
            treeMenu.on('checkchange', function (node, event)
            {
                //触发子节点点击事件
                var checked = node.attributes.checked;
                var childs = node.childNodes;
                for (var i = 0; i < childs.length; i++)
                {
                    if (childs[i].attributes.checked != checked)
                    {
                        childs[i].ui.toggleCheck();
                    }
                }
                if (node.attributes.qtip == "用户")
                {
                    var flag = "0";
                    for (var j = 0; j < ListReceiverName.length; j++)
                    {
                        if (ListReceiverName[j].value == node.id)
                        {
                            ListReceiverName.remove(j);
                            userid.value = userid.value.replace(node.id + ';', '');
                            username.value = username.value.replace(node.text + ';', '');
                            flag = "1";
                            //alert(userid.value);
                            //alert(username.value);
                        }
                    }
                    if (flag == "0")
                    {

                        var k = ListReceiverName.options.length;
                        ListReceiverName.add(new Option(node.text, node.id), k);
                        userid.value += node.id + ';';
                        username.value += node.text + ';';
                        k++;
                        //                        alert(userid.value);
                        //                        alert(username.value);
                    }
                    //                    var userid = document.getElementById('<%= userID.ClientID %>');
                    //                    var username = document.getElementById('<%= userName.ClientID %>');
                    //                    userid.value = "";
                    //                    username.value = "";
                    //                    if (ListReceiverName.length > 0) {
                    //                        for (var i = 0; i < ListReceiverName.length; i++) {


                    //                            userid.value = ListReceiverName[i].value;
                    //                            username.value = ListReceiverName[i].text;
                    //                        }

                    //                    }
                }


            });


        });
        
        
        
    </script>
</body>
</html>
