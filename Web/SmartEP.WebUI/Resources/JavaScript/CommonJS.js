/*
	$RCSfile: common.js,v $
	$Revision: 1.001 $
	$Date: 2013/07/30 09:16:52 $
*/

var lang = new Array();
var userAgent = navigator.userAgent.toLowerCase();
var is_opera = userAgent.indexOf('opera') != -1 && opera.version();
var is_moz = (navigator.product == 'Gecko') && userAgent.substr(userAgent.indexOf('firefox') + 8, 3);
var is_ie = (userAgent.indexOf('msie') != -1 && !is_opera) && userAgent.substr(userAgent.indexOf('msie') + 5, 3);

function checkall(form, prefix, checkall) {
    var checkall = checkall ? checkall : 'chkall';
    for (var i = 0; i < form.elements.length; i++) {
        var e = form.elements[i];
        if (e.name && e.name != checkall && (!prefix || (prefix && e.name.match(prefix)))) {
            e.checked = form.elements[checkall].checked;
        }
    }
}

function doane(event) {
    e = event ? event : window.event;
    if (is_ie) {
        e.returnValue = false;
        e.cancelBubble = true;
    } else if (e) {
        e.stopPropagation();
        e.preventDefault();
    }
}

function fetchCheckbox(cbn) {
    return $(cbn) && $(cbn).checked == true ? 1 : 0;
}

function getcookie(name) {
    var cookie_start = document.cookie.indexOf(name);
    var cookie_end = document.cookie.indexOf(";", cookie_start);
    return cookie_start == -1 ? '' : unescape(document.cookie.substring(cookie_start + name.length + 1, (cookie_end > cookie_start ? cookie_end : document.cookie.length)));
}

function in_array(needle, haystack) {
    if (typeof needle == 'string' || typeof needle == 'number') {
        for (var i in haystack) {
            if (haystack[i] == needle) {
                return true;
            }
        }
    }
    return false;
}

function setcopy(text, alertmsg) {
    if (is_ie) {
        clipboardData.setData('Text', text);
        alert(alertmsg);
    } else if (prompt('Press Ctrl+C Copy to Clipboard', text)) {
        alert(alertmsg);
    }
}
////////////////////////////////////////////////////////////////////////
function isUndefined(variable) {
    return typeof variable == 'undefined' ? true : false;
}
/////////////////////////////////////////////////////////////////////

function mb_strlen(str) {
    var len = 0;
    for (var i = 0; i < str.length; i++) {
        len += str.charCodeAt(i) < 0 || str.charCodeAt(i) > 255 ? (charset == 'utf-8' ? 3 : 2) : 1;
    }
    return len;
}

function setcookie(cookieName, cookieValue, seconds, path, domain, secure) {
    var expires = new Date();
    expires.setTime(expires.getTime() + seconds);
    document.cookie = escape(cookieName) + '=' + escape(cookieValue)
		+ (expires ? '; expires=' + expires.toGMTString() : '')
		+ (path ? '; path=' + path : '/')
		+ (domain ? '; domain=' + domain : '')
		+ (secure ? '; secure' : '');
}

function strlen(str) {
    return (is_ie && str.indexOf('\n') != -1) ? str.replace(/\r?\n/g, '_').length : str.length;
}

function updatestring(str1, str2, clear) {
    str2 = '_' + str2 + '_';
    return clear ? str1.replace(str2, '') : (str1.indexOf(str2) == -1 ? str1 + str2 : str1);
}

function _attachEvent(obj, evt, func) {
    if (obj.addEventListener) {
        obj.addEventListener(evt, func, false);
    } else if (obj.attachEvent) {
        obj.attachEvent("on" + evt, func);
    }
}

function fetchOffset(obj) {
    var left_offset = obj.offsetLeft;
    var top_offset = obj.offsetTop;
    while ((obj = obj.offsetParent) != null) {
        left_offset += obj.offsetLeft;
        top_offset += obj.offsetTop;
    }
    return { 'left': left_offset, 'top': top_offset };
}

function SetEmptyMessage(sender, eventArgs) {
    if (eventArgs.get_newValue() == null || eventArgs.get_newValue().trim() == '') {
        sender.set_value('');
        sender.set_emptyMessage(sender.get_emptyMessage());
        sender.updateDisplayValue();
    }
}

String.prototype.trim = function () {
    var str = this,
    str = str.replace(/^\s\s*/, ''),
    ws = /\s/,
    i = str.length;
    while (ws.test(str.charAt(--i)));
    return str.slice(0, i + 1);
}