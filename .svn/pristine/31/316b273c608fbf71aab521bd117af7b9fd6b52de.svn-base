//判断浏览器内核及版本
var ua = navigator.userAgent.toLowerCase();
//IE
if (ua.match(/msie ([\d.]+)/)) {
    var obj = "<object id='WebOffice1' style='height:100%;width:100%' ";
    obj += "classid='clsid:E77E049B-23FC-4DB8-B756-60529A35FAD5' ";
    obj += "codebase='WebOffice.cab#version=7,0,1,0'>";
    obj += "<param name='_ExtentX' value='6350' />";
    obj += "<param name='_ExtentY' value='6350' />";
    obj += "</object>";
    document.write(obj);
}
    //firefox、chrome
else if (ua.match(/firefox\/([\d.]+)/) || ua.match(/chrome\/([\d.]+)/)) {

    var obj = "<object id='WebOffice1' type='application/x-itst-activex' style='height:100%;width:100%' ";
    obj += "clsid='{E77E049B-23FC-4DB8-B756-60529A35FAD5}' ";
    obj += "event_NotifyCtrlReady='NotifyCtrlReady' ";
    obj += "codeBase='Weboffice.cab#version=7,0,1,0' >";
    obj += "<param name='_ExtentX' value='6350' />";
    obj += "<param name='_ExtentY' value='6350' />";
    obj += "</object>";
    document.write(obj);
}
    //其它
else {
    var obj = "<span>在线编辑功能仅支持 IE/FireFox/Chrome 相关内核的浏览器！</span>";
    document.write(obj);
}


