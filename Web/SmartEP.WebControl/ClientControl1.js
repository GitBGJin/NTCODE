/// <reference name="MicrosoftAjax.js"/>


Type.registerNamespace("SmartEP.WebControl");

SmartEP.WebControl.ClientControl1 = function(element) {
    SmartEP.WebControl.ClientControl1.initializeBase(this, [element]);
}

SmartEP.WebControl.ClientControl1.prototype = {
    initialize: function() {
        SmartEP.WebControl.ClientControl1.callBaseMethod(this, 'initialize');
        
        // 在此处添加自定义初始化
    },
    dispose: function() {        
        //在此处添加自定义释放操作
        SmartEP.WebControl.ClientControl1.callBaseMethod(this, 'dispose');
    }
}
SmartEP.WebControl.ClientControl1.registerClass('SmartEP.WebControl.ClientControl1', Sys.UI.Control);

if (typeof(Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();