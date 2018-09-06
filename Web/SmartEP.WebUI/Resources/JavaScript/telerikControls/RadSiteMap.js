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

var CurrentChks;                            //当前RadSiteMap中的复选框集
var CurrentChksText;                        //当前RadSiteMap中的复选框名称集
var radComboxSiteMapParentNodeIdSuffix = '_RsmChkA';     //radSiteMap父节点ID后缀
var radComboxSiteMapChildNodeIdSuffix = '_RsmChkB';      //radSiteMap子节点ID后缀
var checkModeValue = checkMode.multiCheck;  //默认多选模式

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

//选择父节点
function onSelectParentNode(chk, SingleOrMulit) {
    checkModeValue = SingleOrMulit;
    //判断节点check方式（是否选中）
    var selType = (chk.checked ? selectType.selectAll : selectType.unselect);

    CurrentChks = document.getElementById("RSM").getElementsByTagName('input');
    CurrentChksText = document.getElementById("RSM").getElementsByTagName('label');

    //执行选择CheckBox，并记录选择项值
    executeCheck(nodeType.checkParentNode, selType, chk.id.replace(radComboxSiteMapParentNodeIdSuffix, ''), checkModeValue);
}

//选择子节点
function onSelectChildNode(chk, SingleOrMulit) {
    checkModeValue = SingleOrMulit;
    //判断节点check方式（是否选中）
    var selType = (chk.checked ? selectType.selectAll : selectType.unselect);
    //当前父节点ID
    var radComboxSiteMapParentNodeId = chk.id.substring(0, chk.id.replace(radComboxSiteMapChildNodeIdSuffix, '').lastIndexOf('_'));
    //判断选择模式（单选还是多选）

    CurrentChks = document.getElementById("RSM").getElementsByTagName('input');
    CurrentChksText = document.getElementById("RSM").getElementsByTagName('label');

    if (checkModeValue == checkMode.singleCheck)
        executeCheck(nodeType.checkChildNode, selType, chk.id, checkModeValue);
    else
        executeCheck(nodeType.checkChildNode, selType, radComboxSiteMapParentNodeId, checkModeValue);
}

function executeCheck(NodeType, SelType, CheckNodeID, SingleOrMulit) {
    checkModeValue = SingleOrMulit;
    var NodeID = '', AllSelChkName = '', ChkedItemsCount = 0, NotChkedItemsCount = 0;
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
        if (CurrentChks[i].checked == true && NodeID.indexOf(radComboxSiteMapParentNodeIdSuffix) < 0)
            AllSelChkName += AllSelChkName == '' ? CurrentChksText[i].innerText : ';' + CurrentChksText[i].innerText;
    }
}

function OnRsmBtn(button, args) {
    CurrentChks = document.getElementById("RSM").getElementsByTagName('input');
    CurrentChksText = document.getElementById("RSM").getElementsByTagName('label');

    var BtnTxt = button._text;
    var BtnCmdName = button._commandName;
    executeCheck('all', BtnCmdName, '', 'multi');
}