
function OpenFineUIWindow(moduleGuid, url, title) {
    var hrefs = window.location.href.split('?');
    var localhostUrl = getCurrentUrl();
    if (localhostUrl != "") {
        var frame = parent.window;
        var pareIndex = 0;
        while (frame.location.href.indexOf("Frame") < 0 && pareIndex < 10) {
            frame = frame.parent.window;
            pareIndex = pareIndex + 1;
        }
        //frame.showwindows("EditDeliveryPlan" + Math.random(), url + "//Business/FEC/Delivery/DeliveryPlanManageList.aspx", "计划管理");
        frame.showwindows(moduleGuid, localhostUrl + "/" + url, title);
    }
}

function getCurrentUrl() {
    var hrefs = window.location.href.split('?');
    var url = "";
    var moduleGuid = "";
    if (hrefs.length > 0) {
        url = hrefs[0];
    }
    var index = url.lastIndexOf('/Pages')
    if (index > 0) {
        url = url.substring(0, index);
    }
    else {
        url = "";
    }
    return url;
}