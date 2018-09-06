/* 
    Liuzt Updated On 2013-09-28
    @ 1、注意Firefox 不认识 innerText 属性
*/

var selectType = {
    selectAll: 'selectAll',  //全选
    inverse: 'inverse',    //反选
    unselect: 'unselect'    //不选
}

var nodeType = {
    checkAllNode: 'all',            //选择父节点
    checkParentNode: 'parent',     //选择父节点
    checkChildNode: 'child'        //选择子节点
}

var checkMode = {
    singleCheck: 'single',     //单选
    multiCheck: 'multi'        //多选
}
var radCombox;                              //RadCombox控件对象
var radComboxSiteMapID = 'RSM';             //RadCombox控件包含的radSiteMap对象ID
var radComboxSiteMap;                       //RadCombox控件包含的radSiteMap对象
var CurrentChks;                            //当前RadSiteMap中的复选框集
var CurrentChksText;                        //当前RadSiteMap中的复选框名称集
var radComboxSiteMapParentNodeIdSuffix;     //radSiteMap父节点ID后缀
var radComboxSiteMapChildNodeIdSuffix;      //radSiteMap子节点ID后缀
var checkModeValue = checkMode.multiCheck;  //默认多选模式

//初始化变量
function OnClientDropDownOpening(sender, args) {
    radCombox = sender;
    radCombox._openDropDownOnLoad = false;
    radComboxSiteMap = radCombox.get_items().getItem(0).findControl(radComboxSiteMapID);
    CurrentChks = radComboxSiteMap.get_element().getElementsByTagName('input');
    CurrentChksText = radComboxSiteMap.get_element().getElementsByTagName('label');
    radComboxSiteMapParentNodeIdSuffix = '_RsmChkA';
    radComboxSiteMapChildNodeIdSuffix = '_RsmChkB';
}

function OnClientDropDownOpened(sender, args) {
    var selectedItemText = radCombox.get_text();
    //combox有内容，给模板内容赋值
    if (selectedItemText != '')
        setTemplateValue(radComboxSiteMapID, selectedItemText);
}

function setTemplateValue(templateControl, itemText) {
    var arrItemText = itemText.replace(/;$/, '').split(';'), currValue;
    //alert(itemText +'---###'+ arrItemText.length);
    for (var i = 0; i < arrItemText.length; i++) {
        for (var j = 0; j < CurrentChks.length - 1; j++) {
            //alert(arrItemText[i] + '---###' + chksText[j].innerText);
            currValue = CurrentChksText[j].innerText;
            if (typeof (currValue) == 'undefined') {
                currValue = CurrentChksText[j].innerHTML;
            }

            if (arrItemText[i] == currValue && CurrentChks[j].checked == false) {
                CurrentChks[j].checked = true;
                //判断childnode是否全部选中，如全选，则parentnode选中
                onSelectChildNode(CurrentChks[j], checkModeValue);
                break;
            }
        }
    }
}

//遍历选中CheckBox信息
function OnClientDropDownClosed(sender, args) {
    var checkName = '', RCBName = '', currValue = '';
    for (var i = 0; i < CurrentChks.length - 1; i++) {
        //不包含父节点信息，只遍历子节点信息
        if (CurrentChks[i].checked == true && CurrentChks[i].id.indexOf(radComboxSiteMapParentNodeIdSuffix) < 0) {
            currValue = CurrentChksText[i].innerText;
            if (typeof (currValue) == 'undefined') {
                currValue = CurrentChksText[i].innerHTML;
            }
            checkName += checkName == '' ? currValue : ';' + currValue;
        }
    }
    //选中节点信息赋予ComBox
    sender.set_text(checkName);
    //收缩下拉选框
    sender.hideDropDown();
    RCBName = sender._uniqueId.substring(sender._uniqueId.lastIndexOf('$') + 1)
}

//选择父节点
function onSelectParentNode(chk, SingleOrMulit) {
    radComboxSiteMap = radCombox.get_items().getItem(0).findControl(radComboxSiteMapID);
    CurrentChks = radComboxSiteMap.get_element().getElementsByTagName('input');
    CurrentChksText = radComboxSiteMap.get_element().getElementsByTagName('label');
    checkModeValue = SingleOrMulit;
    //判断节点check方式（是否选中）
    var selType = (chk.checked ? selectType.selectAll : selectType.unselect);
    //执行选择CheckBox，并记录选择项值
    executeCheck(nodeType.checkParentNode, selType, chk.id.replace(radComboxSiteMapParentNodeIdSuffix, ''), checkModeValue);
}

//选择子节点
function onSelectChildNode(chk, SingleOrMulit) {
    radComboxSiteMap = radCombox.get_items().getItem(0).findControl(radComboxSiteMapID);
    CurrentChks = radComboxSiteMap.get_element().getElementsByTagName('input');
    CurrentChksText = radComboxSiteMap.get_element().getElementsByTagName('label');
    checkModeValue = SingleOrMulit;
    //判断节点check方式（是否选中）
    var selType = (chk.checked ? selectType.selectAll : selectType.unselect);
    //当前父节点ID
    var radComboxSiteMapParentNodeId = chk.id.substring(0, chk.id.replace(radComboxSiteMapChildNodeIdSuffix, '').lastIndexOf('_'));
    //判断选择模式（单选还是多选）
    if (checkModeValue == checkMode.singleCheck)
        executeCheck(nodeType.checkChildNode, selType, chk.id, checkModeValue);
    else
        executeCheck(nodeType.checkChildNode, selType, radComboxSiteMapParentNodeId, checkModeValue);
}

function executeCheck(NodeType, SelType, CheckNodeID, SingleOrMulit) {
    checkModeValue = SingleOrMulit;
    var NodeID = '', AllSelChkName = '', ChkedItemsCount = 0, NotChkedItemsCount = 0, currValue = '';
    for (var i = 0; i < CurrentChks.length - 1; i++) {
        NodeID = CurrentChks[i].id;
        if (checkModeValue == checkMode.singleCheck)
            CurrentChks[i].checked = (NodeID == CheckNodeID ? true : false);
        else {
            if (NodeType == nodeType.checkAllNode || NodeType == nodeType.checkParentNode) {
                if (CheckNodeID == '' || (NodeID.indexOf(CheckNodeID) >= 0 && NodeID.replace(CheckNodeID, '').substring(0, 1) == '_')) {
                    switch (SelType) {
                        case selectType.selectAll:
                            CurrentChks[i].checked = true;
                            break;
                        case selectType.inverse:
                            CurrentChks[i].checked = !(CurrentChks[i].checked)
                            break;
                        case selectType.unselect:
                            CurrentChks[i].checked = false;
                            break;
                    }
                }
            }
            else if (NodeID.indexOf(CheckNodeID) >= 0 && NodeID.replace(CheckNodeID, '').substring(0, 1) == '_' && NodeID.indexOf(radComboxSiteMapParentNodeIdSuffix) < 0) {

                if (CurrentChks[i].checked == true)
                    ChkedItemsCount++;
                else
                    NotChkedItemsCount++;

                if (NotChkedItemsCount <= 0)
                    ($get(CheckNodeID + radComboxSiteMapParentNodeIdSuffix)).checked = true;
                else 
                    ($get(CheckNodeID + radComboxSiteMapParentNodeIdSuffix)).checked = false;
                if (ChkedItemsCount <= 0)
                    ($get(CheckNodeID + radComboxSiteMapParentNodeIdSuffix)).checked = false;
            }
        }

        currValue = CurrentChksText[i].innerText;
        if (typeof (currValue) == 'undefined') currValue = CurrentChksText[i].innerHTML;
        if (CurrentChks[i].checked == true && NodeID.indexOf(radComboxSiteMapParentNodeIdSuffix) < 0)
            AllSelChkName += AllSelChkName == '' ? currValue : ';' + currValue;
    }
    radCombox.set_text(AllSelChkName);
}


function OnRTVClientDropDownClosed(sender, args) {
    var checkName = '';
    var RadTreeView = sender.get_items().getItem(0).findControl("RTV");
    var checkedNodes = RadTreeView.get_checkedNodes();
    if (checkedNodes != null) {

        for (var i = 0; i < checkedNodes.length; i++) {
            if (checkedNodes[i].get_value() != null) {
                checkName += checkName == '' ? checkedNodes[i].get_text() : ';' + checkedNodes[i].get_text();
            }
        }
    }
    sender.set_text(checkName);
    sender.hideDropDown();
}

function OnRsmBtn(button, args) {
    var BtnTxt = button._text;
    var BtnCmdName = button._commandName;
    executeCheck('all', BtnCmdName, '', 'multi');
}